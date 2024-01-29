using ClosedXML.Excel;
using IIITS.DTLMS.BL;
using IIITS.DTLMS.BL.General;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.Reports
{
    public partial class SubDivisionWiseFailureReport : System.Web.UI.Page
    {
        clsSession objSession = new clsSession();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToString(Session["clsSession"] ?? "").Length == 0)
                {
                    Response.Redirect("~/Login.aspx", false);
                }

                objSession = (clsSession)Session["clsSession"];
                string stroffCode = string.Empty;

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
                    string[] monthNames = { "--Select--", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC", "JAN", "FEB", "MAR" };

                    // Add each month as an ListItem to the DropDownList
                    foreach (string month in monthNames)
                    {
                        ddlMonth.Items.Add(new ListItem(month));
                    }
                    Genaral.Load_Combo("SELECT \"FY_YEARS\",\"FY_YEARS\" FROM \"TBLFINANCIALYEAR\"  WHERE \"FY_STATUS\"='1' AND \"FY_ID\" >=  6", "--Select--", cmbFinYear);
                    if (stroffCode.Length >= 1)
                    {
                        Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone);
                        stroffCode = stroffCode.Substring(0, Constants.Zone);
                        cmbZone.Items.FindByValue(stroffCode).Selected = true;
                        cmbZone.Enabled = false;
                        stroffCode = stroffCode1;
                    }

                    if (stroffCode == null || stroffCode == "")
                    {
                        Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone);
                    }

                    if (stroffCode.Length >= 1)
                    {
                        Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_ZO_ID\"='" + cmbZone.SelectedValue + "'", "--Select--", cmbCircle);
                        //(stroffCode.Length >= 3) ? stroffCode.Substring(0, Constants.Division) : stroffCode;
                        stroffCode = (stroffCode.Length >= 2) ? stroffCode.Substring(0, Constants.Circle) : stroffCode;
                        if (stroffCode.Length >= 2)
                        {
                            cmbCircle.Items.FindByValue(stroffCode).Selected = true;
                            cmbCircle.Enabled = false;
                        }
                        stroffCode = stroffCode1;
                    }

                    if (stroffCode.Length >= 2)
                    {
                        Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE \"DIV_CICLE_CODE\"='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDiv);

                        if (stroffCode.Length >= 3)
                        {
                            stroffCode = stroffCode.Substring(0, Constants.Division);
                            cmbDiv.Items.FindByValue(stroffCode).Selected = true;
                            cmbDiv.Enabled = false;
                            stroffCode = stroffCode1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                     MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// cmbZone_SelectedIndexChanged
        /// for Dropdown Zone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbZone.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE CAST(\"CM_CIRCLE_CODE\" AS TEXT) LIKE'" + cmbZone.SelectedValue + "%'", "--Select--", cmbCircle);
                    cmbDiv.Items.Clear();
                }
                else
                {
                    cmbCircle.Items.Clear();
                    cmbDiv.Items.Clear();
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
        /// cmbCircle_SelectedIndexChanged
        /// for Dropdown Circle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CICLE_CODE\" AS TEXT)='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDiv);
                }
                else
                {
                    cmbDiv.Items.Clear();
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
        /// Show Msg Box
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
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// BtnReset_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnReset_Click(object sender, EventArgs e)
        {

            try
            {
                objSession = (clsSession)Session["clsSession"];
                string OfficeCode = string.Empty;

                if (objSession.OfficeCode.Length <= 2 && objSession.OfficeCode.Length != 0)
                {
                    OfficeCode = clsStoreOffice.GetZone_Circle_Div_Offcode(objSession.OfficeCode, objSession.RoleId);
                    OfficeCode = (OfficeCode.Length >= 3) ? OfficeCode.Substring(0, Constants.Division) : OfficeCode;
                }
                else
                {
                    OfficeCode = objSession.OfficeCode;
                }
                cmbFinYear.SelectedIndex = 0;
                ddlMonth.SelectedIndex = 0;

                if ((OfficeCode ?? "").Length == 0)
                {
                    cmbZone.SelectedIndex = 0;
                    cmbCircle.Items.Clear();
                    cmbDiv.Items.Clear();
                }
                else
                {
                    switch (OfficeCode.Length)
                    {
                        case 1:
                            cmbCircle.SelectedIndex = 0;
                            cmbDiv.Items.Clear();
                            break;
                        case 2:
                            cmbDiv.SelectedIndex = 0;
                            break;
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
        /// Export_click Subdivision Wise Failure Report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Export_clickSubdivisionWiseFailureReport(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            clsReports objReport = new clsReports();


            string previousyear = string.Empty;
            string presentyear = string.Empty;
            string selectedMonth = string.Empty;
            ////string CurrentYear = string.Empty;
            string CurrentMonth = string.Empty;
            int NextMonth = 0;

            if (cmbFinYear.Text == "--Select--")
            {
                ShowMsgBox("Please Select The Financial Year");
                cmbFinYear.Focus();
                return;
            }

            if (ddlMonth.SelectedValue == "--Select--")
            {
                ShowMsgBox("Please Select The Month");
                ddlMonth.Focus();
                return;
            }

            previousyear = cmbFinYear.SelectedValue.Split('-').GetValue(0).ToString();
            presentyear = cmbFinYear.SelectedValue.Split('-').GetValue(1).ToString();
            selectedMonth = ddlMonth.SelectedValue;

            if (Convert.ToString(selectedMonth ?? "") != "--Select--")
            {
                if ((selectedMonth ?? "").Length > 0)
                {
                    switch (selectedMonth)
                    {
                        case "JAN":
                            selectedMonth = selectedMonth.Replace("JAN", "01");
                            break;
                        case "FEB":
                            selectedMonth = selectedMonth.Replace("FEB", "02");
                            break;
                        case "MAR":
                            selectedMonth = selectedMonth.Replace("MAR", "03");
                            break;
                        case "APR":
                            selectedMonth = selectedMonth.Replace("APR", "04");
                            break;
                        case "MAY":
                            selectedMonth = selectedMonth.Replace("MAY", "05");
                            break;
                        case "JUN":
                            selectedMonth = selectedMonth.Replace("JUN", "06");
                            break;
                        case "JUL":
                            selectedMonth = selectedMonth.Replace("JUL", "07");
                            break;
                        case "AUG":
                            selectedMonth = selectedMonth.Replace("AUG", "08");
                            break;
                        case "SEP":
                            selectedMonth = selectedMonth.Replace("SEP", "09");
                            break;
                        case "OCT":
                            selectedMonth = selectedMonth.Replace("OCT", "10");
                            break;
                        case "NOV":
                            selectedMonth = selectedMonth.Replace("NOV", "11");
                            break;
                        case "DEC":
                            selectedMonth = selectedMonth.Replace("DEC", "12");
                            break;
                    }

                    CurrentMonth = selectedMonth;
                    NextMonth = Convert.ToInt16(CurrentMonth) + 1;

                    if (Convert.ToInt16(CurrentMonth) < 4)
                    {
                        objReport.sFromDate = presentyear + "-" + CurrentMonth + "-" + "01";
                    }
                    else
                    {
                        objReport.sFromDate = previousyear + "-" + CurrentMonth + "-" + "01";
                    }
                    if (NextMonth < 10)
                    {
                        string tillMonth = "0" + NextMonth;

                        if (Convert.ToInt16(NextMonth) < 4)
                        {
                            objReport.sTodate = presentyear + "-" + tillMonth + "-" + "01";
                        }
                        else
                        {
                            objReport.sTodate = previousyear + "-" + tillMonth + "-" + "01";
                        }
                    }
                    else
                    {
                        if (Convert.ToInt16(NextMonth) < 4)
                        {
                            objReport.sTodate = presentyear + "-" + Convert.ToString(NextMonth) + "-" + "01";
                        }
                        else
                        {
                            objReport.sTodate = previousyear + "-" + Convert.ToString(NextMonth) + "-" + "01";
                        }
                    }
                }
            }
            if (cmbDiv.SelectedIndex > 0)
            {
                objReport.sOfficeCode = cmbDiv.SelectedValue;
            }
            else if (cmbCircle.SelectedIndex > 0)
            {
                objReport.sOfficeCode = cmbCircle.SelectedValue;
            }
            else if (cmbZone.SelectedIndex > 0)
            {
                objReport.sOfficeCode = cmbZone.SelectedValue;
            }
            else
            {
                objReport.sOfficeCode = "";
            }

            dt = objReport.PrintSubDivisionwiseTransformerFailureReport(objReport);

            if (dt.Rows.Count > 0)
            {
                #region  

                // Use Select to filter rows based on conditions
                DataRow[] filteredRows = dt.Select("SD_SUBDIV_NAME LIKE '%TOTAL DIVISION:%'");

                // Update the desired columns for all matching rows
                foreach (DataRow row in filteredRows)
                {
                    // Keep the existing data and modify only specific columns
                    row.BeginEdit();

                    row["ZONE"] = "";
                    row["CIRCLE"] = "";
                    row["DIV_NAME"] = "";

                    row.EndEdit();
                }

                // Repeat the process for other conditions
                filteredRows = dt.Select("DIV_NAME LIKE '%TOTAL CIRCLE:%'");
                foreach (DataRow row in filteredRows)
                {
                    row.BeginEdit();

                    row["ZONE"] = "";
                    row["CIRCLE"] = "";

                    row.EndEdit();
                }

                filteredRows = dt.Select("CIRCLE LIKE '%TOTAL ZONE:%'");
                foreach (DataRow row in filteredRows)
                {
                    row.BeginEdit();

                    row["ZONE"] = "";

                    row.EndEdit();
                }

                #endregion


                string[] arrAlpha = Genaral.getalpha();

                string sMergeRange = arrAlpha[dt.Columns.Count - 3];
                using (XLWorkbook wb = new XLWorkbook())
                {
                    dt.Columns["ZONE"].ColumnName = "ZONE";
                    dt.Columns["CIRCLE"].ColumnName = "CIRCLE";
                    dt.Columns["DIV_NAME"].ColumnName = "DIVISION";
                    dt.Columns["SD_SUBDIV_NAME"].ColumnName = "SUBDIVISION";

                    dt.Columns["10 KVA"].ColumnName = "10 KVA";
                    dt.Columns["15 KVA"].ColumnName = "15 KVA";
                    dt.Columns["25 KVA"].ColumnName = "25 KVA";
                    dt.Columns["50 KVA"].ColumnName = "50 KVA";
                    dt.Columns["63 KVA"].ColumnName = "63 KVA";
                    dt.Columns["100 KVA"].ColumnName = "100 KVA";

                    dt.Columns["125 KVA"].ColumnName = "125 KVA";
                    dt.Columns["150 KVA"].ColumnName = "150 KVA";
                    dt.Columns["160 KVA"].ColumnName = "160 KVA";
                    dt.Columns["200 KVA"].ColumnName = "200 KVA";
                    dt.Columns["250 KVA"].ColumnName = "250 KVA";

                    dt.Columns["300 KVA"].ColumnName = "300 KVA";
                    dt.Columns["315 KVA"].ColumnName = "315 KVA";
                    dt.Columns["400 KVA"].ColumnName = "400 KVA";
                    dt.Columns["500 KVA"].ColumnName = "500 KVA";
                    dt.Columns["630 KVA"].ColumnName = "630 KVA";
                    dt.Columns["750 KVA"].ColumnName = "750 KVA";

                    dt.Columns["960 KVA"].ColumnName = "960 KVA";
                    dt.Columns["1000 KVA"].ColumnName = "1000 KVA";
                    dt.Columns["1250 KVA"].ColumnName = "1250 KVA";
                    dt.Columns["Total"].ColumnName = "Total";

                    wb.Worksheets.Add(dt, "Sub-Div Wise Failure Details");

                    wb.Worksheet(1).Row(1).InsertRowsAbove(3);

                    var rangehead = wb.Worksheet(1).Range("A1:Y1");
                    rangehead.Merge().Style.Font.SetBold().Font.FontSize = 16;
                    rangehead.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    rangehead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangehead.SetValue("Hubli Electricity Supply Company Limited, (HESCOM)");

                    var rangeReporthead = wb.Worksheet(1).Range("A2:Y2");
                    rangeReporthead.Merge().Style.Font.SetBold().Font.FontSize = 12;
                    rangeReporthead.Merge().Style.Fill.BackgroundColor = XLColor.AirForceBlue;
                    rangeReporthead.Merge().Style.Font.FontColor = XLColor.White;
                    rangeReporthead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangeReporthead.SetValue("Sub-Division wise Transformer Failure (Month: " + ddlMonth.SelectedValue + ") " +
                        cmbFinYear.SelectedValue.Split('-').GetValue(0).ToString() + "-" + cmbFinYear.SelectedValue.Split('-').GetValue(1).ToString());
                    wb.Worksheet(1).Cell(3, 8).Value = DateTime.Now;

                    #region
                    // Apply formatting based on the condition
                    FormatRowsContainingValue(wb.Worksheet(1), "TOTAL DIVISION:", XLColor.FromArgb(161, 171, 232));

                    FormatRowsContainingValue(wb.Worksheet(1), "TOTAL CIRCLE:", XLColor.FromArgb(228, 188, 91));

                    FormatRowsContainingValue(wb.Worksheet(1), "TOTAL ZONE:", XLColor.FromArgb(198, 232, 182));

                    FormatRowsContainingValue(wb.Worksheet(1), "TOTAL HESCOM COUNT", XLColor.FromArgb(117, 154, 191));
                    #endregion

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    string FileName = "Sub-Division wise Transformer Failure Deatils " + DateTime.Now + ".xls";
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
                ShowMsgBox("No Records Found");
            }
        }
        /// <summary>
        /// Format Rows Containing Value
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
        /// Generate_clickSubdivisionWiseFailureReport
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Generate_clickSubdivisionWiseFailureReport(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();

                string fnclyear = string.Empty;

                string sResult = string.Empty;

                if (cmbFinYear.Text == "--Select--")
                {
                    ShowMsgBox("Please Select The Financial Year");
                    cmbFinYear.Focus();
                    return;
                }

                if (ddlMonth.SelectedValue == "--Select--")
                {
                    ShowMsgBox("Please Select The Month");
                    ddlMonth.Focus();
                    return;
                }
                string previousyear = cmbFinYear.SelectedValue.Split('-').GetValue(0).ToString();
                string presentyear = cmbFinYear.SelectedValue.Split('-').GetValue(1).ToString();
                string monthSelected = string.Empty;
                string CurrentMonth = string.Empty;
                int NextMonth = 0;

                string FromDate = string.Empty;
                string Todate = string.Empty;
                string OfficeCode = string.Empty;

                monthSelected = ddlMonth.SelectedValue;
                if ((monthSelected ?? "").Length > 0)
                {
                    switch (monthSelected)
                    {
                        case "JAN":
                            monthSelected = monthSelected.Replace("JAN", "01");
                            break;
                        case "FEB":
                            monthSelected = monthSelected.Replace("FEB", "02");
                            break;
                        case "MAR":
                            monthSelected = monthSelected.Replace("MAR", "03");
                            break;
                        case "APR":
                            monthSelected = monthSelected.Replace("APR", "04");
                            break;
                        case "MAY":
                            monthSelected = monthSelected.Replace("MAY", "05");
                            break;
                        case "JUN":
                            monthSelected = monthSelected.Replace("JUN", "06");
                            break;
                        case "JUL":
                            monthSelected = monthSelected.Replace("JUL", "07");
                            break;
                        case "AUG":
                            monthSelected = monthSelected.Replace("AUG", "08");
                            break;
                        case "SEP":
                            monthSelected = monthSelected.Replace("SEP", "09");
                            break;
                        case "OCT":
                            monthSelected = monthSelected.Replace("OCT", "10");
                            break;
                        case "NOV":
                            monthSelected = monthSelected.Replace("NOV", "11");
                            break;
                        case "DEC":
                            monthSelected = monthSelected.Replace("DEC", "12");
                            break;
                    }

                    CurrentMonth = monthSelected;
                    NextMonth = Convert.ToInt16(CurrentMonth) + 1;

                    if (Convert.ToInt16(CurrentMonth) < 4)
                    {
                        FromDate = presentyear + "-" + CurrentMonth + "-" + "01";
                    }
                    else
                    {
                        FromDate = previousyear + "-" + CurrentMonth + "-" + "01";
                    }

                    if (NextMonth < 10)
                    {
                        string tillMonth = "0" + NextMonth;

                        if (Convert.ToInt16(NextMonth) < 4)
                        {
                            Todate = presentyear + "-" + tillMonth + "-" + "01";
                        }
                        else
                        {
                            Todate = previousyear + "-" + tillMonth + "-" + "01";
                        }
                    }
                    else
                    {
                        if (Convert.ToInt16(NextMonth) < 4)
                        {
                            Todate = presentyear + "-" + Convert.ToString(NextMonth) + "-" + "01";
                        }
                        else
                        {
                            Todate = previousyear + "-" + Convert.ToString(NextMonth) + "-" + "01";
                        }
                    }
                }

                if (cmbDiv.SelectedIndex > 0)
                {
                    OfficeCode = cmbDiv.SelectedValue;
                }
                else if (cmbCircle.SelectedIndex > 0)
                {
                    OfficeCode = cmbCircle.SelectedValue;
                }
                else if (cmbZone.SelectedIndex > 0)
                {
                    OfficeCode = cmbZone.SelectedValue;
                }
                else
                {
                    OfficeCode = "";
                }

                string Type = "CRISTEL REPORT";

                string sParam = "id=SubdivisionWiseFailureDetails&FromDate=" + FromDate + "&Todate=" + Todate + "&OfficeCode=" + OfficeCode + "&Type=" + Type + "&Month=" + ddlMonth.SelectedValue + "&Finyear=" + cmbFinYear.SelectedValue + "";
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + sParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
            }
            catch (Exception ex)
            {
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}