using ClosedXML.Excel;
using IIITS.DTLMS.BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.Reports
{
    public partial class FailuresDailyReport : System.Web.UI.Page
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
                    string[] monthNames = { "--Select--", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "Jan", "Feb", "Mar" };

                    // Add each month as an ListItem to the DropDownList
                    foreach (string month in monthNames)
                    {
                        ddlFromDate.Items.Add(new ListItem(month));
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

                        stroffCode = stroffCode.Substring(0, Constants.Circle);
                        cmbCircle.Items.FindByValue(stroffCode).Selected = true;
                        cmbCircle.Enabled = false;
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

                    if (stroffCode.Length >= 3)
                    {
                        Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE \"SD_DIV_CODE\"='" + cmbDiv.SelectedValue + "'", "--Select--", cmbSubDiv);
                        if (stroffCode.Length >= 4)
                        {
                            stroffCode = stroffCode.Substring(0, Constants.SubDivision);
                            cmbSubDiv.Items.FindByValue(stroffCode).Selected = true;
                            cmbSubDiv.Enabled = false;
                            stroffCode = stroffCode1;
                        }
                    }
                    if (stroffCode.Length >= 4)
                    {
                        Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_SUBDIV_CODE\"='" + cmbSubDiv.SelectedValue + "'", "--Select--", cmbSection);
                        if (stroffCode.Length >= 5)
                        {
                            stroffCode = stroffCode.Substring(0, Constants.Section);
                            cmbSection.Items.FindByValue(stroffCode).Selected = true;
                            cmbSection.Enabled = false;
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
        protected void cmbZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbCircle.Items.Clear();
                cmbDiv.Items.Clear();
                cmbSubDiv.Items.Clear();
                cmbSection.Items.Clear();
                Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE CAST(\"CM_CIRCLE_CODE\" AS TEXT) LIKE'" + cmbZone.SelectedValue + "%'", "--Select--", cmbCircle);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                     MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CICLE_CODE\" AS TEXT)='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDiv);
                    cmbSubDiv.Items.Clear();
                    cmbSection.Items.Clear();
                }
                else
                {
                    cmbDiv.Items.Clear();
                    cmbSubDiv.Items.Clear();
                    cmbSection.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                     MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDiv.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE CAST(\"SD_DIV_CODE\" AS TEXT)='" + cmbDiv.SelectedValue + "'", "--Select--", cmbSubDiv);
                    cmbSection.Items.Clear();
                }
                else
                {
                    cmbSubDiv.Items.Clear();
                    cmbSection.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                     MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbSubDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubDiv.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE CAST(\"OM_SUBDIV_CODE\" AS TEXT)='" + cmbSubDiv.SelectedValue + "'", "--Select--", cmbSection);
                }
                else
                {
                    cmbSection.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
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
        protected void BtnReset_Click(object sender, EventArgs e)
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
            try
            {
                cmbFinYear.SelectedIndex = 0;
                ddlFromDate.SelectedIndex = 0;

                if ((OfficeCode ?? "").Length == 0)
                {
                    cmbZone.SelectedIndex = 0;
                    cmbCircle.Items.Clear();
                    cmbDiv.Items.Clear();
                    cmbSubDiv.Items.Clear();
                    cmbSection.Items.Clear();
                }
                else
                {
                    switch (OfficeCode.Length)
                    {
                        case 1:
                            cmbCircle.SelectedIndex = 0;
                            cmbDiv.Items.Clear();
                            cmbSubDiv.Items.Clear();
                            cmbSection.Items.Clear();
                            break;
                        case 2:
                            cmbDiv.SelectedIndex = 0;
                            cmbSubDiv.Items.Clear();
                            cmbSection.Items.Clear();
                            break;
                        case 3:
                            cmbSubDiv.SelectedIndex = 0;
                            cmbSection.Items.Clear();
                            break;
                        case 4:
                            cmbSection.SelectedIndex = 0;
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
        protected void Export_clickFailureDailyReport(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            clsReports objReport = new clsReports();

            string previousyear = string.Empty;
            string presentyear = string.Empty;
            string selectedMonth = string.Empty;
            string CurrentYear = string.Empty;
            string CurrentMonth = string.Empty;
            int NextMonth = 0;

            if (cmbFinYear.Text == "--Select--")
            {
                ShowMsgBox("Please Select The Financial Year");
                cmbFinYear.Focus();
                return;
            }

            previousyear = cmbFinYear.SelectedValue.Split('-').GetValue(0).ToString();
            presentyear = cmbFinYear.SelectedValue.Split('-').GetValue(1).ToString();

            selectedMonth = ddlFromDate.SelectedValue;
            if (Convert.ToString(selectedMonth ?? "") != "--Select--")
            {


                if ((selectedMonth ?? "").Length > 0)
                {
                    switch (selectedMonth)
                    {
                        case "Jan":
                            selectedMonth = selectedMonth.Replace("Jan", "01");
                            break;
                        case "Feb":
                            selectedMonth = selectedMonth.Replace("Feb", "02");
                            break;
                        case "Mar":
                            selectedMonth = selectedMonth.Replace("Mar", "03");
                            break;
                        case "Apr":
                            selectedMonth = selectedMonth.Replace("Apr", "04");
                            break;
                        case "May":
                            selectedMonth = selectedMonth.Replace("May", "05");
                            break;
                        case "Jun":
                            selectedMonth = selectedMonth.Replace("Jun", "06");
                            break;
                        case "Jul":
                            selectedMonth = selectedMonth.Replace("Jul", "07");
                            break;
                        case "Aug":
                            selectedMonth = selectedMonth.Replace("Aug", "08");
                            break;
                        case "Sep":
                            selectedMonth = selectedMonth.Replace("Sep", "09");
                            break;
                        case "Oct":
                            selectedMonth = selectedMonth.Replace("Oct", "10");
                            break;
                        case "Nov":
                            selectedMonth = selectedMonth.Replace("Nov", "11");
                            break;
                        case "Dec":
                            selectedMonth = selectedMonth.Replace("Dec", "12");
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
            else
            {
                previousyear = previousyear + "-04-01";
                presentyear = presentyear + "-04-01";

                objReport.sFromDate = previousyear;
                objReport.sTodate = presentyear;
            }

            if (cmbSection.SelectedIndex > 0)
            {
                objReport.sOfficeCode = cmbSection.SelectedValue;
            }
            else if (cmbSubDiv.SelectedIndex > 0)
            {
                objReport.sOfficeCode = cmbSubDiv.SelectedValue;
            }
            else if (cmbDiv.SelectedIndex > 0)
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

            dt = objReport.PrintFailuresDailyReport(objReport);

            if (dt.Rows.Count > 0)
            {
                string[] arrAlpha = Genaral.getalphanew();

                string sMergeRange = arrAlpha[dt.Columns.Count - 11];
                using (XLWorkbook wb = new XLWorkbook())
                {
                    dt.Columns["ZO_NAME"].ColumnName = "ZONE";
                    dt.Columns["CM_CIRCLE_NAME"].ColumnName = "CIRCLE";
                    dt.Columns["DIV_NAME"].ColumnName = "DIVISION";
                    dt.Columns["SD_SUBDIV_NAME"].ColumnName = "SUBDIVISION";
                    dt.Columns["OM_NAME"].ColumnName = "SECTION";

                    dt.Columns["MONTH"].ColumnName = "MONTH";
                    dt.Columns["NAME_OF_FEEDER"].ColumnName = "NAME OF FEEDER";
                    dt.Columns["LOCATION_TYPE"].ColumnName = "LOCATION TYPE";
                    dt.Columns["LOCATION_NAME"].ColumnName = "LOCATION NAME";
                    dt.Columns["COMPLAINT_NO"].ColumnName = "COMPLAINT NO";
                    dt.Columns["FAILURE_DATE"].ColumnName = "FAILURE DATE";

                    dt.Columns["CONSUMER_MOBILE_NO"].ColumnName = "CONSUMER MOBILE NO";
                    dt.Columns["DTC_CODE"].ColumnName = "DTC CODE";
                    dt.Columns["DTR_CODE"].ColumnName = "DTR CODE";
                    dt.Columns["DTR_CAPACITY"].ColumnName = "DTR CAPACITY";
                    dt.Columns["DTR_TYPE"].ColumnName = "DTR TYPE";

                    dt.Columns["TM_NAME"].ColumnName = "MAKE";
                    dt.Columns["TC_SLNO"].ColumnName = "SL NO";
                    dt.Columns["TC_MANF_DATE"].ColumnName = "MANUFACTURE DATE";
                    dt.Columns["PO_NO"].ColumnName = "PO NO";
                    dt.Columns["PO_DATE"].ColumnName = "PO DATE";
                    dt.Columns["TC_OIL_CAPACITY"].ColumnName = "QTY OF OIL";

                    dt.Columns["REPAIRER_NAME"].ColumnName = "REPAIRER NAME";
                    dt.Columns["REPAIRER_PO_NO"].ColumnName = "REPAIRER PO NO";
                    dt.Columns["REPAIRER_PO_DATE"].ColumnName = "REPAIRER PO DATE";
                    dt.Columns["DATE_OF_TESTING"].ColumnName = "DOT (DATE OF TESTING)";
                    dt.Columns["RV_DATE"].ColumnName = "DOD (RV DATE)";
                    dt.Columns["LOAD_TYPE"].ColumnName = "LOAD TYPE";

                    dt.Columns["CONNECTED_LOAD_IN_KW"].ColumnName = "CONNECTED LOAD IN KW";
                    dt.Columns["CONNECTED_LOAD_IN_HP"].ColumnName = "CONNECTED LOAD IN HP";
                    dt.Columns["FAILURE_REASON"].ColumnName = "FAILURE REASON";
                    dt.Columns["WORK_ORDER_NO"].ColumnName = "WORK ORDER NO";
                    dt.Columns["WORK_ORDER_DATE"].ColumnName = "WORK ORDER DATE";
                    dt.Columns["INVOICE_NO"].ColumnName = "INVOICE NO";

                    dt.Columns["INVOICE_DATE"].ColumnName = "INVOICE DATE";
                    dt.Columns["INVOICE_STATUS"].ColumnName = "INVOICE STATUS";
                    dt.Columns["REMARKS"].ColumnName = "REMARKS";

                    dt.Columns["ZONE"].SetOrdinal(0);
                    dt.Columns["CIRCLE"].SetOrdinal(1);
                    dt.Columns["DIVISION"].SetOrdinal(2);
                    dt.Columns["SUBDIVISION"].SetOrdinal(3);
                    dt.Columns["SECTION"].SetOrdinal(4);
                    dt.Columns["MONTH"].SetOrdinal(5);

                    dt.Columns["NAME OF FEEDER"].SetOrdinal(6);
                    dt.Columns["LOCATION TYPE"].SetOrdinal(7);
                    dt.Columns["LOCATION NAME"].SetOrdinal(8);
                    dt.Columns["COMPLAINT NO"].SetOrdinal(9);
                    dt.Columns["FAILURE DATE"].SetOrdinal(10);
                    dt.Columns["CONSUMER MOBILE NO"].SetOrdinal(11);

                    dt.Columns["DTC CODE"].SetOrdinal(12);
                    dt.Columns["DTR CODE"].SetOrdinal(13);
                    dt.Columns["DTR CAPACITY"].SetOrdinal(14);
                    dt.Columns["DTR TYPE"].SetOrdinal(15);
                    dt.Columns["MAKE"].SetOrdinal(16);
                    dt.Columns["SL NO"].SetOrdinal(17);

                    dt.Columns["MANUFACTURE DATE"].SetOrdinal(18);
                    dt.Columns["PO NO"].SetOrdinal(19);
                    dt.Columns["PO DATE"].SetOrdinal(20);
                    dt.Columns["QTY OF OIL"].SetOrdinal(21);
                    dt.Columns["REPAIRER NAME"].SetOrdinal(22);
                    dt.Columns["REPAIRER PO NO"].SetOrdinal(23);

                    dt.Columns["REPAIRER PO DATE"].SetOrdinal(24);
                    dt.Columns["DOT (DATE OF TESTING)"].SetOrdinal(25);
                    dt.Columns["DOD (RV DATE)"].SetOrdinal(26);
                    dt.Columns["LOAD TYPE"].SetOrdinal(27);
                    dt.Columns["CONNECTED LOAD IN KW"].SetOrdinal(28);
                    dt.Columns["CONNECTED LOAD IN HP"].SetOrdinal(29);

                    dt.Columns["FAILURE REASON"].SetOrdinal(30);
                    dt.Columns["WORK ORDER NO"].SetOrdinal(31);
                    dt.Columns["WORK ORDER DATE"].SetOrdinal(32);
                    dt.Columns["INVOICE NO"].SetOrdinal(33);
                    dt.Columns["INVOICE DATE"].SetOrdinal(34);
                    dt.Columns["INVOICE STATUS"].SetOrdinal(35);
                    dt.Columns["REMARKS"].SetOrdinal(36);

                    dt.Columns.Remove("DF_ID");
                    dt.Columns.Remove("DF_DATE");
                    dt.Columns.Remove("RSD_ID");
                    dt.Columns.Remove("RSD_TC_CODE");
                    dt.Columns.Remove("RSD_CURRENT_DF_ID");
                    dt.Columns.Remove("RSD_NEXT_DF_ID");

                    wb.Worksheets.Add(dt, "Failure and Replacement Detials");

                    wb.Worksheet(1).Row(1).InsertRowsAbove(3);

                    var rangehead = wb.Worksheet(1).Range("A1:AK1");
                    rangehead.Merge().Style.Font.SetBold().Font.FontSize = 16;
                    rangehead.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    rangehead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangehead.SetValue("Hubli Electricity Supply Company Limited, (HESCOM)");

                    var rangeReporthead = wb.Worksheet(1).Range("A2:AK2");
                    rangeReporthead.Merge().Style.Font.SetBold().Font.FontSize = 12;
                    rangeReporthead.Merge().Style.Fill.BackgroundColor = XLColor.AirForceBlue;
                    rangeReporthead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangeReporthead.SetValue(" Month - " + objReport.sFromDate + " ");
                    wb.Worksheet(1).Cell(3, 8).Value = DateTime.Now;

                    var rangeReporthead1 = wb.Worksheet(1).Range("A2:AK2");
                    rangeReporthead.Merge().Style.Font.SetBold().Font.FontSize = 12;
                    rangeReporthead.Merge().Style.Fill.BackgroundColor = XLColor.AirForceBlue;
                    rangeReporthead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangeReporthead.SetValue("Replacement of Failed Distribution Transformers in O & M Hubballi Zone, HESCOM 2023-24 ");
                    wb.Worksheet(1).Cell(3, 8).Value = DateTime.Now;

                    var rangeReporthead2 = wb.Worksheet(1).Range("Q3:V3");
                    rangeReporthead2.Merge().Style.Font.SetBold().Font.FontSize = 12;
                    rangeReporthead2.Merge().Style.Fill.BackgroundColor = XLColor.LightGreen;
                    rangeReporthead2.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    rangeReporthead2.SetValue("New DTr Details");

                    var rangeReporthead3 = wb.Worksheet(1).Range("W3:AA3");
                    rangeReporthead3.Merge().Style.Font.SetBold().Font.FontSize = 12;
                    rangeReporthead3.Merge().Style.Fill.BackgroundColor = XLColor.LightSkyBlue;
                    rangeReporthead3.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    rangeReporthead3.SetValue("Repaired Good DTr Details");


                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    string FileName = "Failed DTr Deatils " + DateTime.Now + ".xls";
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
    }
}