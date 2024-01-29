using ClosedXML.Excel;
using IIITS.DTLMS.BL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.Reports
{
    public partial class TimeTakenForDTrReplacement : System.Web.UI.Page
    {
        string strFormCode = "TimeTakenForDTrReplacement";
        clsSession objSession;
        int Circle_code = Convert.ToInt32(ConfigurationManager.AppSettings["Circle_code"]);
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
                    Genaral.Load_Combo("SELECT \"FY_ID\",\"FY_YEARS\" FROM \"TBLFINANCIALYEAR\" WHERE \"FY_STATUS\"='1' AND \"FY_ID\" >=  6 ORDER BY \"FY_ID\"", "--Select--", cmbFinYear);

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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                     MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        
       

        /// <summary>
        /// Generating abstract for DTrReplacmenttimelineReport
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Abstract_DTrReplacmenttimelineReport(object sender, EventArgs e)
            {
            string fromdate = string.Empty;
            string todate = string.Empty;
            string previousyear = string.Empty;
            string presentyear = string.Empty;
            DataTable dt = new DataTable();
            clsReports objReport = new clsReports();
            //  string sResult = string.Empty;

            if (cmbFinYear.SelectedIndex > 0)
            {
                fromdate = cmbFinYear.SelectedItem.Text;
                todate = cmbFinYear.SelectedItem.Text;
            }
            else
            {
                ShowMsgBox("Please Select Financial Year");
                return;
            }

            if(cmbCircle.SelectedIndex==0)
            {
                ShowMsgBox("Please Select Circle");
                return;
            }
            previousyear = cmbFinYear.SelectedItem.Text.Split('-').GetValue(0).ToString();
            presentyear = cmbFinYear.SelectedItem.Text.Split('-').GetValue(1).ToString();



            fromdate = 04 + "-" + previousyear;
            todate = 03 + "-" + presentyear;


            objReport.sFromDate = fromdate;
            objReport.sTodate = todate;
            if (cmbCircle.SelectedIndex > 0)
                objReport.sOfficeCode = cmbCircle.SelectedValue;
            else objReport.sOfficeCode = "";
            dt = objReport.GetCircleWiseDTrRepairerReplacement(objReport);

            if (dt.Rows.Count > 1)

            {
                DataRow[] filteredRows = dt.Select("PARTICULARS LIKE '%TOTAL DIV: %'");

                // Update the desired columns for all matching rows
                foreach (DataRow row in filteredRows)
                {
                    // Keep the existing data and modify only specific columns
                    row.BeginEdit();

                    row["DIVISIONNAME"] = row["PARTICULARS"];
                    row["PARTICULARS"] = "";

                    row.EndEdit();
                }

                grdAbstractDtrReplacementDetails.Attributes.Add("style", "display:block");
                 grdAbstractDtrReplacementDetails.DataSource = dt;
                 grdAbstractDtrReplacementDetails.DataBind();
            }
            else
            {
               grdAbstractDtrReplacementDetails.Attributes.Add("style", "display:none");
                ShowMsgBox("No Records Found");
                return;
            }

        }

        /// <summary>
        /// Generating Excel for DTrReplacmenttimelineReport
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Export_DTrReplacmenttimelineReport(object sender, EventArgs e)
        {
            try
            {

                string fromdate = string.Empty;
                string todate = string.Empty;
                string previousyear = string.Empty;
                string presentyear = string.Empty;
                DataTable dt = new DataTable();
                clsReports objReport = new clsReports();
                string sResult = string.Empty;
                string CurrentYear = string.Empty;
                string CurrentMonth = string.Empty;
                string Month = string.Empty;

                if (cmbFinYear.SelectedIndex > 0)
                {
                    fromdate = cmbFinYear.SelectedItem.Text;
                    todate = cmbFinYear.SelectedItem.Text;
                }
                else
                {
                    ShowMsgBox("Please Select Financial Year");
                    return;
                }
                if (cmbCircle.SelectedIndex == 0)
                {
                    ShowMsgBox("Please Select Circle");
                    return;
                }
                previousyear = cmbFinYear.SelectedItem.Text.Split('-').GetValue(0).ToString();
                presentyear = cmbFinYear.SelectedItem.Text.Split('-').GetValue(1).ToString();


               
                fromdate = 04 + "-" + previousyear;
                todate = 03 + "-" + presentyear;


                objReport.sFromDate = fromdate;
                objReport.sTodate = todate;

                if (cmbCircle.SelectedIndex > 0)
                    objReport.sOfficeCode = cmbCircle.SelectedValue;
                else objReport.sOfficeCode = "";

                dt = objReport.GetCircleWiseDTrRepairerReplacement(objReport);
                if (dt.Rows.Count > 1)
                {
                    // Use Select to filter rows based on conditions
                    DataRow[] filteredRows = dt.Select("PARTICULARS LIKE '%TOTAL DIV: %'");
                   
                    // Update the desired columns for all matching rows
                    foreach (DataRow row in filteredRows)
                    {
                        // Keep the existing data and modify only specific columns
                        row.BeginEdit();
                        
                            row["DIVISIONNAME"] = row["PARTICULARS"];
                            row["PARTICULARS"] = "";                     
                        
                        row.EndEdit();
                    }

                    string[] arrAlpha = Genaral.getalpha();

                    string sMergeRange = arrAlpha[dt.Columns.Count - 1];
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        dt.Columns["DIVISIONNAME"].ColumnName = "DIVISION NAME";
                        dt.Columns["PARTICULARS"].ColumnName = "PARTICULARS";
                        dt.Columns["10_KVA"].ColumnName = "10 KVA";
                        dt.Columns["15_KVA"].ColumnName = "15 KVA";
                        dt.Columns["25_KVA"].ColumnName = "25 KVA";
                        dt.Columns["50_KVA"].ColumnName = "50 KVA";
                        dt.Columns["63_KVA"].ColumnName = "63 KVA";
                        dt.Columns["100_KVA"].ColumnName = "100 KVA";
                        dt.Columns["125_KVA"].ColumnName = "125 KVA";
                        dt.Columns["150_KVA"].ColumnName = "150 KVA";
                        dt.Columns["160_KVA"].ColumnName = "160 KVA";
                        dt.Columns["200_KVA"].ColumnName = "200 KVA";
                        dt.Columns["250_KVA"].ColumnName = "250 KVA";
                        dt.Columns["300_KVA"].ColumnName = "300 KVA";
                        dt.Columns["315_KVA"].ColumnName = "315 KVA";
                        dt.Columns["400_KVA"].ColumnName = "400 KVA";
                        dt.Columns["500_KVA"].ColumnName = "500 KVA";
                        dt.Columns["630_KVA"].ColumnName = "630 KVA";
                        dt.Columns["750_KVA"].ColumnName = "750 KVA";
                        dt.Columns["960_KVA"].ColumnName = "960 KVA";
                        dt.Columns["1000_KVA"].ColumnName = "1000 KVA";
                        dt.Columns["1250_KVA"].ColumnName = "1250 KVA";

                        dt.Columns["PARTICULARS"].SetOrdinal(1);
                        dt.Columns["10 KVA"].SetOrdinal(2);
                        dt.Columns["15 KVA"].SetOrdinal(3);
                        dt.Columns["25 KVA"].SetOrdinal(4);
                        dt.Columns["50 KVA"].SetOrdinal(5);
                        if (wb != null)
                        {
                            wb.Worksheets.Add(dt, "Time_Taken_for_DTr_Replacement");
                            wb.Worksheet(1).Row(1).InsertRowsAbove(3);
                        }

                        var rangehead = wb.Worksheet(1).Range("A1:" + sMergeRange + "1");
                        rangehead.Merge().Style.Font.SetBold().Font.FontSize = 16;
                        rangehead.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
                        rangehead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        rangehead.SetValue("Hubli Electricity Supply Company Limited, (HESCOM)");

                        var rangeReporthead = wb.Worksheet(1).Range("A2:" + sMergeRange + "2");
                        rangeReporthead.Merge().Style.Font.SetBold().Font.FontSize = 12;
                        rangeReporthead.Merge().Style.Fill.BackgroundColor = XLColor.AirForceBlue;
                        rangeReporthead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        var rangereporthead = wb.Worksheet(1).Range("A3:" + sMergeRange + "3");
                        rangereporthead.Merge().Style.Font.SetBold().Font.FontSize = 12;
                        rangereporthead.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
                        rangereporthead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        string CircleName = string.Empty;
                        if (cmbCircle.SelectedIndex > 0)
                        {
                            CircleName = cmbCircle.SelectedItem.Text;
                            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
                            CircleName = textInfo.ToTitleCase(CircleName.ToLower());
                            CircleName = (CircleName ?? "").Length > 0 ? (CircleName + " Circle ") : "";
                        }
                        rangeReporthead.SetValue("Details of Agewise Number of Failed Transformers Replaced During The Year  " + cmbFinYear.SelectedItem.Text);
                        rangereporthead.SetValue( "Name of the Circle : " + CircleName);


                        FormatColumnContainingValue(wb.Worksheet(1), 23, "TOTAL", XLColor.FromArgb(161, 171, 232));
                        
                        FormatRowsContainingValue(wb.Worksheet(1), "TOTAL CIRCLE ", XLColor.FromArgb(161, 171, 232));
                        FormatRowsContainingValue(wb.Worksheet(1), "TOTAL DIV:", XLColor.FromArgb(161, 171, 232));


                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        string FileName = "Time_Taken_for_DTr_Replacement" + DateTime.Now + ".xls";
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
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                     MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// formatting row values
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
        /// formatting column values
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
        /// For Reset Text Fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnReset_Click(object sender, EventArgs e)
        {

            try
            {
               // cmbFinYear.SelectedIndex = 0;
                //cmbCircle.SelectedIndex = 0;
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                     MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// For Displaying Pop Msg
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


        //protected void grdAbstractDtrReplacementDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Row.RowType == DataControlRowType.DataRow)
        //        {

        //            // Your existing code for handling DataRows
        //            int rowCount = ((DataTable)grdAbstractDtrReplacementDetails.DataSource).Rows.Count;

        //            // Check if it's the last row across all pages
        //            if (e.Row.RowIndex == rowCount - 1)
        //            {
        //                foreach (TableCell cell in e.Row.Cells)
        //                {
        //                    cell.Font.Bold = true;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}

        protected void grdAbstractDtrReplacementDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    // Use Select to filter rows based on conditions
                    DataRow[] filteredRows = ((DataTable)grdAbstractDtrReplacementDetails.DataSource).Select("DIVISIONNAME LIKE '%TOTAL DIV: %'");

                    //// Your existing code for handling DataRows
                    int rowCount = ((DataTable)grdAbstractDtrReplacementDetails.DataSource).Rows.Count;

                    // Check if the current row matches the condition
                    if (filteredRows.Any(row => row == ((DataRowView)e.Row.DataItem).Row))
                    {
                        foreach (TableCell cell in e.Row.Cells)
                        {
                            cell.Font.Bold = true;
                        }
                    }

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