using ClosedXML.Excel;
using IIITS.DTLMS.BL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.Reports
{
    public partial class CircleWiseRepairerReport : System.Web.UI.Page
    {
        string strFormCode = "CentralWiseTransformerRepairer";
        clsSession objSession;
        int Zone_code = Convert.ToInt32(ConfigurationManager.AppSettings["Zone_code"]);
        int Circle_code = Convert.ToInt32(ConfigurationManager.AppSettings["Circle_code"]);
        int Division_code = Convert.ToInt32(ConfigurationManager.AppSettings["Division_code"]);
        int SubDiv_code = Convert.ToInt32(ConfigurationManager.AppSettings["SubDiv_code"]);
        int Section_code = Convert.ToInt32(ConfigurationManager.AppSettings["Section_code"]);
        /// <summary>
        /// page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }

                string stroffCode = string.Empty;
                objSession = (clsSession)Session["clsSession"];
                if (objSession.OfficeCode.Length <= 2 && objSession.OfficeCode.Length != 0)
                {

                    //stroffCode = clsStoreOffice.GetZone_Circle_Div_Offcode(objSession.OfficeCode, objSession.RoleId).Substring(0, Circle_code);

                    stroffCode = clsStoreOffice.GetZone_Circle_Div_Offcode(objSession.OfficeCode, objSession.RoleId);
                    stroffCode = (stroffCode.Length >= 3) ? stroffCode.Substring(0, Constants.Division) : stroffCode;

                }
                else
                {
                    stroffCode = objSession.OfficeCode;
                }
                string stroffCode1 = stroffCode;
                if (!IsPostBack)
                {
                    Genaral.Load_Combo("SELECT \"FY_ID\",\"FY_YEARS\" FROM \"TBLFINANCIALYEAR\" WHERE \"FY_STATUS\"='1' AND \"FY_ID\" >=  6 ORDER BY \"FY_ID\"", "--Select--", cmbFinancialyear);

                    if (stroffCode.Length >= 1)
                    {
                        Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE cast(\"CM_CIRCLE_CODE\" as text) like'" + stroffCode + "%'", "--Select--", cmbCircle);
                        stroffCode = (stroffCode.Length >= 2) ? stroffCode.Substring(0, Constants.Circle) : stroffCode;
                        if (stroffCode.Length >= 2)
                        {
                            cmbCircle.Items.FindByValue(stroffCode).Selected = true;
                            cmbCircle.Enabled = false;
                        }
                    }
                    else
                    {
                        Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE cast(\"CM_CIRCLE_CODE\" as text) like'" + stroffCode + "%'", "--Select--", cmbCircle);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        /// <summary>
        /// This function used to generated excel for repairer agency based on circle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void Export_ClickCircleWiseRepairer(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            clsReports objReport = new clsReports();
            string sResult = string.Empty;

            if (cmbFinancialyear.SelectedIndex > 0)
            {
                objReport.FinancialYearId = cmbFinancialyear.SelectedValue;
            }
            else
            {
                ShowMsgBox("Please Select Financial Year");
                return;
            }
            if (cmbCircle.SelectedIndex > 0)
                objReport.sOfficeCode = cmbCircle.SelectedValue;
            else objReport.sOfficeCode = "";

            dt = objReport.GetCircleWiseDTrRepairer(objReport);

            if (dt.Rows.Count > 1)
            {

                // Use Select to filter rows based on conditions
                DataRow[] filteredRows = dt.Select("MONTHS LIKE '%TOTAL%'");

                // Update the desired columns for all matching rows
                foreach (DataRow row in filteredRows)
                {
                    // Keep the existing data and modify only specific columns
                    row.BeginEdit();
                    row["ISSUED_TRNAME"] = row["MONTHS"];
                    row["MONTHS"] = "";
                    row.EndEdit();
                }
                string[] arrAlpha = Genaral.getalpha();

                string sMergeRange = arrAlpha[dt.Columns.Count - 1];
                using (XLWorkbook wb = new XLWorkbook())
                {
                    dt.Columns["MONTHS"].ColumnName = "MONTH";
                    dt.Columns["ISSUED_TRNAME"].ColumnName = "NAME OF THE REPAIR AGENCY";
                    dt.Columns["ISSUED_25_KVA"].ColumnName = "25 KVA";
                    dt.Columns["ISSUED_63_KVA"].ColumnName = "63 KVA";
                    dt.Columns["ISSUED_100_KVA"].ColumnName = "100 KVA";
                    dt.Columns["ISSUED_250_KVA"].ColumnName = "250 KVA";
                    dt.Columns["ISSUED_Grand_Total_KVA"].ColumnName = "TOTAL";

                    dt.Columns["RECEIVED_25_KVA"].ColumnName = "25 KVA ";
                    dt.Columns["RECEIVED_63_KVA"].ColumnName = "63 KVA ";
                    dt.Columns["RECEIVED_100_KVA"].ColumnName = "100 KVA ";
                    dt.Columns["RECEIVED_250_KVA"].ColumnName = "250 KVA ";
                    dt.Columns["RECEIVED_Grand_Total_KVA"].ColumnName = "TOTAL ";
                    dt.Columns["Total_RSD_REP_COST"].ColumnName = "TOTAL COST OF REPAIR";
                    dt.Columns["TOTAL COST OF REPAIR"].SetOrdinal(12);
                    dt.Columns["MONTH"].SetOrdinal(1);
                    dt.Columns["NAME OF THE REPAIR AGENCY"].SetOrdinal(0);
                    wb.Worksheets.Add(dt, "Transformers_repaired_by_agency");

                    wb.Worksheet(1).Row(1).InsertRowsAbove(3);

                    var rangehead = wb.Worksheet(1).Range("A1:" + sMergeRange + "1");
                    rangehead.Merge().Style.Font.SetBold().Font.FontSize = 16;
                    rangehead.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    rangehead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangehead.SetValue("Hubli Electricity Supply Company Limited, (HESCOM)");

                    var rangeReporthead = wb.Worksheet(1).Range("A2:" + sMergeRange + "2");
                    rangeReporthead.Merge().Style.Font.SetBold().Font.FontSize = 12;
                    rangeReporthead.Merge().Style.Fill.BackgroundColor = XLColor.AirForceBlue;
                    rangeReporthead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    string CircleName = string.Empty;
                    if (cmbCircle.SelectedIndex > 0)
                    {

                        CircleName = cmbCircle.SelectedItem.Text;
                        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
                        CircleName = textInfo.ToTitleCase(CircleName.ToLower());

                        CircleName = (CircleName ?? "").Length > 0 ? (CircleName + " Circle ") : "";


                    }

                    rangeReporthead.SetValue(CircleName + "Details Of Transformers Repaired By Agency " + cmbFinancialyear.SelectedItem.Text);

                    //wb.Worksheet(1).Cell(3, 8).Value = DateTime.Now;

                    var rangeHeaderCell = wb.Worksheet(1).Cell(3, 3);
                    rangeHeaderCell.Value = "NO OF TRANSFORMER ISSUED FOR REPAIR";

                    var endColumn = 3 + 5 - 1;
                    var rangeHeaderRange = wb.Worksheet(1).Range(rangeHeaderCell, wb.Worksheet(1).Cell(3, endColumn));
                    rangeHeaderRange.Merge().Style.Font.SetBold().Font.FontSize = 10;
                    rangeHeaderRange.Merge().Style.Fill.BackgroundColor = XLColor.FromArgb(240, 186, 106);
                    rangeHeaderRange.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);



                    var rangeHeaderCellReceived = wb.Worksheet(1).Cell(3, 8);
                    rangeHeaderCellReceived.Value = "NO OF TRANSFORMERS REPAIRED AND RETURNED BY AGENCY";

                    var endColumnReceived = 8 + 5 - 1;
                    var rangeHeaderCellReceivedRange = wb.Worksheet(1).Range(rangeHeaderCellReceived, wb.Worksheet(1).Cell(3, endColumnReceived));
                    rangeHeaderCellReceivedRange.Merge().Style.Font.SetBold().Font.FontSize = 10;
                    rangeHeaderCellReceivedRange.Merge().Style.Fill.BackgroundColor = XLColor.FromArgb(145, 252, 199);
                    rangeHeaderCellReceivedRange.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    wb.Worksheet(1).Cell(3, 1).Value = "DATE: " + DateTime.Now.ToString("dd/MM/yyyy HH:MM");

                    FormatColumnContainingValue(wb.Worksheet(1), 7, "TOTAL", XLColor.FromArgb(161, 171, 232));
                    FormatColumnContainingValue(wb.Worksheet(1), 12, "TOTAL", XLColor.FromArgb(161, 171, 232));


                    FormatRowsContainingValue(wb.Worksheet(1), "Total", XLColor.FromArgb(161, 171, 232));
                    FormatRowsContainingValue(wb.Worksheet(1), "Grand Total", XLColor.FromArgb(161, 171, 232));





                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    string FileName = "Transformers_repaired_by_agency" + DateTime.Now + ".xls";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=" + FileName);

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            else
            {
                grdAbstractDtrDetails.Attributes.Add("style", "display:none");

                ShowMsgBox("No Records Found");
            }

        }
        /// <summary>
        /// Reset page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdReset_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }

        /// <summary>
        /// Show pop up message
        /// </summary>
        /// <param name="sMsg"></param>
        private void ShowMsgBox(string sMsg)
        {
            try
            {
                string sShowMsg = string.Empty;
                sShowMsg = "<script language=javascript> alert ('" + sMsg + "')</script>";
                this.Page.RegisterStartupScript("Msg", sShowMsg);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Formating for rows in excel
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="searchValue"></param>
        /// <param name="backgroundColor"></param>
        protected static void FormatRowsContainingValue(IXLWorksheet worksheet, string searchValue, XLColor backgroundColor)
        {
            foreach (IXLRow row in worksheet.RowsUsed())
            {
                bool foundValue = false;

                foreach (IXLCell cell in row.CellsUsed())
                {
                    if (cell.GetString().Contains(searchValue))
                    {
                        foundValue = true;
                        break; // Break out of the inner loop once the condition is met for the row
                    }
                }

                if (foundValue)
                {
                    // Set background color to the entire row until the end of the table
                    for (int col = 1; col <= row.LastCellUsed().Address.ColumnNumber; col++)
                    {
                        row.Cell(col).Style.Fill.SetBackgroundColor(backgroundColor);
                    }
                    // Make the entire row bold
                    row.Style.Font.SetBold();
                }
            }
        }
        /// <summary>
        /// Formating for column in excel
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="targetColumn"></param>
        /// <param name="searchValue"></param>
        /// <param name="backgroundColor"></param>
        protected static void FormatColumnContainingValue(IXLWorksheet worksheet, int targetColumn, string searchValue, XLColor backgroundColor)
        {
            foreach (IXLRow row in worksheet.RowsUsed())
            {
                IXLCell cell = row.Cell(targetColumn);

                if (cell.GetString().Contains(searchValue))
                {
                    // Set background color for the entire column until the end of the entries
                    for (int rowNumber = row.RowNumber(); rowNumber <= worksheet.LastRowUsed().RowNumber(); rowNumber++)
                    {
                        worksheet.Cell(rowNumber, targetColumn).Style.Fill.SetBackgroundColor(backgroundColor);
                    }

                    // Make the entire column bold
                    for (int rowNumber = row.RowNumber(); rowNumber <= worksheet.LastRowUsed().RowNumber(); rowNumber++)
                    {
                        worksheet.Cell(rowNumber, targetColumn).Style.Font.SetBold();
                    }

                    break; // Break out of the loop once the condition is met for the column
                }
            }
        }


        /// <summary>
        /// Abstract Report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdDtrAbstract_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            clsReports objReport = new clsReports();
            //  string sResult = string.Empty;

            if (cmbFinancialyear.SelectedIndex > 0)
            {
                objReport.FinancialYearId = cmbFinancialyear.SelectedValue;
            }
            else
            {
                ShowMsgBox("Please Select Financial Year");
                return;
            }
            if (cmbCircle.SelectedIndex > 0)
                objReport.sOfficeCode = cmbCircle.SelectedValue;
            else objReport.sOfficeCode = "";
            dt = objReport.GetCircleWiseDTrRepairerAbstract(objReport);

            DataTable Dtamt = objReport.GetCircleWiseRepairerAmount(objReport);
            #region if we want to get poamt based on above dt values
            //int i = 0;
            //foreach (DataRow dr in dt.Rows)
            //{
            //    string customerIDSample1 = dr["ISSUED_TRNAME"].ToString();

            //    int j = 0;
            //    foreach (DataRow dr2 in Dtamt.Rows)
            //    {
            //        if (dr2["TR_NAME"].ToString().Equals(customerIDSample1))
            //        {

            //            dt.Rows[i]["Total_RSD_REP_COST"] = Dtamt.Rows[j]["TOTAL_AMOUNT"];
            //        }
            //        j++;
            //    }
            //    i++;
            //}
            #endregion

            if (dt.Rows.Count > 1)
            {
                grdAbstractDtrDetails.Attributes.Add("style", "display:block");

                grdAbstractDtrDetails.DataSource = dt;
                grdAbstractDtrDetails.DataBind();
                AddHeader("NO OF TRANSFORMER ISSUED FOR REPAIR", 5, "NO OF TRANSFORMERS REPAIRED AND RETURNED BY AGENCY", 5);

            }
            else
            {
                grdAbstractDtrDetails.Attributes.Add("style", "display:none");
                ShowMsgBox("No Records Found");
                return;
            }
        }

        /// <summary>
        /// Adding headers for Abstract Report Grid
        /// </summary>
        /// <param name="headerText1"></param>
        /// <param name="colSpan1"></param>
        /// <param name="headerText2"></param>
        /// <param name="colSpan2"></param>
        private void AddHeader(string headerText1, int colSpan1, string headerText2, int colSpan2)
        {
            // Check if the GridView has at least one row
            if (grdAbstractDtrDetails.Rows.Count > 0)
            {
                // Check if the specified index is within the valid range
                int index = 0;  // You can adjust the index as needed
                if (index >= 0 && index <= grdAbstractDtrDetails.Rows[0].Cells.Count)
                {
                    GridViewRow headerRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                    // Add the first header cell
                    TableCell headerCell0 = new TableCell();
                    headerCell0.Text = "";
                    headerCell0.ColumnSpan = 1;
                    headerCell0.HorizontalAlign = HorizontalAlign.Center;
                    headerRow.Cells.Add(headerCell0);

                    // Add the first header cell
                    TableCell headerCell1 = new TableCell();
                    headerCell1.Text = headerText1;
                    headerCell1.ColumnSpan = colSpan1;
                    //headerCell1.HorizontalAlign = HorizontalAlign.Center;
                    headerCell1.Style["text-align"] = "center !important";
                    headerCell1.Font.Bold = true;
                    headerRow.Cells.Add(headerCell1);

                    // Add the second header cell
                    TableCell headerCell2 = new TableCell();
                    headerCell2.Text = headerText2;
                    headerCell2.ColumnSpan = colSpan2;
                    headerCell2.Style["text-align"] = "center !important";
                    headerCell2.Font.Bold = true;
                    headerRow.Cells.Add(headerCell2);

                    // Add the third header cell
                    TableCell headerCell3 = new TableCell();
                    headerCell3.Text = "";
                    headerCell3.ColumnSpan = 1;
                    headerCell3.HorizontalAlign = HorizontalAlign.Center;
                    headerRow.Cells.Add(headerCell3);

                    // Check if the GridView's Controls collection has at least one control
                    if (grdAbstractDtrDetails.Controls.Count > 0)
                    {
                        // Check if the specified index is within the valid range for the Controls collection
                        if (index >= 0 && index <= grdAbstractDtrDetails.Controls[0].Controls.Count)
                        {
                            grdAbstractDtrDetails.Controls[0].Controls.AddAt(index, headerRow);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Showing Empty Grid
        /// </summary>
        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("ISSUED_TRNAME");
                dt.Columns.Add("ISSUED_25_KVA");
                dt.Columns.Add("ISSUED_63_KVA");
                dt.Columns.Add("ISSUED_100_KVA");
                dt.Columns.Add("ISSUED_250_KVA");
                dt.Columns.Add("ISSUED_Grand_Total_KVA");
                dt.Columns.Add("RECEIVED_25_KVA");
                dt.Columns.Add("RECEIVED_63_KVA");
                dt.Columns.Add("RECEIVED_100_KVA");
                dt.Columns.Add("RECEIVED_250_KVA");
                dt.Columns.Add("RECEIVED_Grand_Total_KVA");
                dt.Columns.Add("Total_RSD_REP_COST");

                grdAbstractDtrDetails.DataSource = dt;
                grdAbstractDtrDetails.DataBind();

                int iColCount = grdAbstractDtrDetails.Rows[0].Cells.Count;
                grdAbstractDtrDetails.Rows[0].Cells.Clear();
                grdAbstractDtrDetails.Rows[0].Cells.Add(new TableCell());
                grdAbstractDtrDetails.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdAbstractDtrDetails.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }

        /// <summary>
        /// Row Data Bound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdAbstractDtrDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    // Your existing code for handling DataRows
                    int rowCount = ((DataTable)grdAbstractDtrDetails.DataSource).Rows.Count;

                    // Check if it's the last row across all pages
                    if (e.Row.RowIndex == rowCount - 1)
                    {
                        foreach (TableCell cell in e.Row.Cells)
                        {
                            cell.Font.Bold = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}