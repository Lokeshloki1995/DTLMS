using IIITS.DTLMS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using ClosedXML.Excel;
using System.Configuration;
namespace IIITS.DTLMS.Reports
{
    public partial class RepairerTransformer : System.Web.UI.Page
    {
        string strFormCode = "RepairerTransformer";
        clsSession objSession;
        int Zone_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Zone_code"]);
        int Circle_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Circle_code"]);
        int Division_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Division_code"]);
        int SubDiv_code = Convert.ToInt32(ConfigurationSettings.AppSettings["SubDiv_code"]);
        int Section_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Section_code"]);
        int Feeder_code = Convert.ToInt32(ConfigurationSettings.AppSettings["feeder_code"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];
                //objSession = (clsSession)Session["sOfficeCode"];
                string stroffCode=string.Empty;
                string stroffCode1= string.Empty;

                //if(objSession.OfficeCode.Length>1)
                //{
                //    stroffCode = objSession.OfficeCode.Substring(0, Zone_code);
                //}
                //else
                //{
                //    stroffCode=objSession.OfficeCode;
                //}
                stroffCode = clsStoreOffice.GetZone_Circle_Div_Offcode(objSession.OfficeCode, objSession.RoleId);
                stroffCode1 = stroffCode;

                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");
                CalendarExtender4.EndDate = System.DateTime.Now;
                CalendarExtender3.EndDate = System.DateTime.Now;
                txtFromDate1.Attributes.Add("readonly", "readonly");
                txtToDate1.Attributes.Add("readonly", "readonly");
                CalendarExtender5.EndDate = System.DateTime.Now;
                CalendarExtender6.EndDate = System.DateTime.Now;

                if (!IsPostBack)
                {
                    if (stroffCode != "")
                    {
                        Genaral.Load_Combo("SELECT DISTINCT \"TR_ID\", \"TR_NAME\"  FROM \"TBLTRANSREPAIRER\",\"TBLTRANSREPAIREROFFCODE\" WHERE \"TR_ID\"=\"TRO_TR_ID\" AND \"TR_STATUS\"='A' AND \"TR_ID\" NOT IN (SELECT \"TR_ID\" FROM \"TBLTRANSREPAIRER\",\"TBLTRANSREPAIREROFFCODE\" WHERE \"TR_ID\"=\"TRO_TR_ID\" AND \"TR_BLACK_LISTED\"=1 AND \"TR_BLACKED_UPTO\">=CURRENT_DATE ) AND CAST(\"TRO_OFF_CODE\" AS TEXT) LIKE'" + stroffCode.Substring(0, 2) + "%' ORDER BY \"TR_NAME\"", "--Select--", cmbRepairerName);
                    }
                    else
                    {
                        Genaral.Load_Combo("SELECT DISTINCT \"TR_ID\", \"TR_NAME\"  FROM \"TBLTRANSREPAIRER\",\"TBLTRANSREPAIREROFFCODE\" WHERE \"TR_ID\"=\"TRO_TR_ID\" AND \"TR_STATUS\"='A' AND \"TR_ID\" NOT IN (SELECT \"TR_ID\" FROM \"TBLTRANSREPAIRER\",\"TBLTRANSREPAIREROFFCODE\" WHERE  \"TR_ID\"=\"TRO_TR_ID\" AND \"TR_BLACK_LISTED\"=1 AND \"TR_BLACKED_UPTO\">=CURRENT_DATE) AND CAST(\"TRO_OFF_CODE\" AS TEXT) LIKE'" + stroffCode + "%' ORDER BY \"TR_NAME\"", "--Select--", cmbRepairerName);
                    }
                    Genaral.Load_Combo("SELECT \"MD_ORDER_BY\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='C' ORDER BY \"MD_ORDER_BY\"", "--Select--", cmbCapacity);
                    Genaral.Load_Combo("SELECT \"MD_ORDER_BY\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='C' ORDER BY \"MD_ORDER_BY\"", "--Select--", cmbCapacity2);

                    //if (stroffCode.Length >= 1)
                    //{
                    //    Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\"  ORDER BY \"CM_CIRCLE_CODE\"", "--Select--", cmbCircle);
                    //    cmbCircle.Items.FindByValue(stroffCode).Selected = true;
                    //    stroffCode = string.Empty;
                    //    stroffCode = objSession.OfficeCode;
                    //}
                    Genaral.Load_Combo("SELECT \"TM_ID\",\"TM_NAME\" FROM \"TBLTRANSMAKES\" ORDER BY \"TM_ID\"", "--Select--", cmbMake1);
                    Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone4);


                    if (stroffCode.Length >= 1)
                    {
                        Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone);
                        cmbZone.Items.FindByValue(stroffCode.Substring(0,Constants.Zone)).Selected = true;
                        cmbZone.Enabled = false;
                        Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone4);
                        cmbZone4.Items.FindByValue(stroffCode.Substring(0, Constants.Zone)).Selected = true;
                        cmbZone4.Enabled = false;
                        stroffCode = stroffCode1;
                    }

                    if (stroffCode == null || stroffCode == "")
                    {
                        Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone);
                    }
                    if (stroffCode.Length >= 1)
                    {
                        Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_ZO_ID\"='" + cmbZone.SelectedValue + "'", "--Select--", cmbCircle);
                        Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_ZO_ID\"='" + cmbZone4.SelectedValue + "'", "--Select--", cmbCircle1);
                        if (stroffCode.Length >= 2)
                        {

                            stroffCode = stroffCode1.Substring(0, Circle_code);
                           // stroffCode1 = clsStoreOffice.GetOfficeCode(stroffCode, "STO_OFF_CODE");
                            cmbCircle.Items.FindByValue(stroffCode).Selected = true;
                            cmbCircle.Enabled = false;
                            cmbCircle1.Items.FindByValue(stroffCode).Selected = true;
                            cmbCircle1.Enabled = false;
                            stroffCode = stroffCode1;
                        }
                    }

                    if (stroffCode.Length >= 2)
                    {
                        Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE \"DIV_CICLE_CODE\"='" + cmbCircle1.SelectedValue + "'", "--Select--", cmbDiv1);
                        if (stroffCode.Length >= 3)
                        {
                            stroffCode = stroffCode1.Substring(0, Division_code);
                            cmbDiv1.Items.FindByValue(stroffCode).Selected = true;
                            cmbDiv1.Enabled = false;
                            stroffCode = stroffCode1;
                        }
                    }
                    if (stroffCode.Length >= 3)
                    {
                        Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE \"SD_DIV_CODE\"='" + cmbDiv1.SelectedValue + "'", "--Select--", cmbSubDiv1);
                        if (stroffCode.Length >= 4)
                        {
                            stroffCode = stroffCode1.Substring(0, SubDiv_code);
                            cmbSubDiv1.Items.FindByValue(stroffCode).Selected = true;
                            cmbSubDiv1.Enabled = false;
                            stroffCode = stroffCode1;
                        }
                    }
                    if (stroffCode.Length >= 4)
                    {
                        Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_SUBDIV_CODE\"='" + cmbSubDiv1.SelectedValue + "'", "--Select--", cmbSection1);
                        if (stroffCode.Length >= 5)
                        {
                            stroffCode = stroffCode1;
                            cmbSection1.Items.FindByValue(stroffCode).Selected = true;
                            cmbSection1.Enabled = false;
                            stroffCode = stroffCode1;
                        }
                    }
                }
             
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbZone4_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbZone4.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_ZO_ID\"='" + cmbZone4.SelectedValue + "'", "--Select--", cmbCircle1);
                    cmbDiv1.Items.Clear();
                    cmbSubDiv1.Items.Clear();
                    cmbSection1.Items.Clear();
                }

                else
                {
                    cmbCircle1.Items.Clear();
                    cmbDiv1.Items.Clear();
                    cmbSubDiv1.Items.Clear();
                    cmbSection1.Items.Clear();

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void cmbCircle1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle1.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE \"DIV_CICLE_CODE\"='" + cmbCircle1.SelectedValue + "'", "--Select--", cmbDiv1);
                    cmbSubDiv1.Items.Clear();
                    cmbSection1.Items.Clear();
                }

                else
                {

                    cmbDiv1.Items.Clear();
                    cmbSubDiv1.Items.Clear();
                    cmbSection1.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }
        protected void cmbDiv1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDiv1.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE \"SD_DIV_CODE\"='" + cmbDiv1.SelectedValue + "'", "--Select--", cmbSubDiv1);
                    cmbSection1.Items.Clear();
                }
                else
                {
                    cmbSubDiv1.Items.Clear();
                    cmbSection1.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }




        protected void cmbSubDiv1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubDiv1.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_SUBDIV_CODE\"='" + cmbSubDiv1.SelectedValue + "'", "--Select--", cmbSection1);
                    //Genaral.Load_Combo("SELECT \"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" and CAST(\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbSubDiv1.SelectedValue + "%'  ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName1);
                }

                else
                {
                    cmbSection1.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbZone.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_ZO_ID\"='" + cmbZone.SelectedValue + "'", "--Select--", cmbCircle);
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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE \"DIV_CICLE_CODE\"='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDiv);
                }
                else
                {
                    cmbDiv.Items.Clear();
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        //protected void cmbDiv_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (cmbDiv.SelectedIndex > 0)
        //        {
        //            Genaral.Load_Combo("SELECT SD_SUBDIV_CODE,SD_SUBDIV_NAME from TBLSUBDIVMAST WHERE SD_DIV_CODE='" + cmbDiv.SelectedValue + "'", "--Select--", cmbSubDiv);
        //        }
             
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbDiv_SelectedIndexChanged");
        //    }
        //}

        //protected void cmbSubDiv_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (cmbSubDiv.SelectedIndex > 0)
        //        {
        //            Genaral.Load_Combo("SELECT OM_CODE,OM_NAME FROM TBLOMSECMAST WHERE OM_SUBDIV_CODE='" + cmbSubDiv.SelectedValue + "'", "--Select--", cmbOMSection);
        //        }
          
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbTaluk_SelectedIndexChanged");
        //    }
        //}

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

        protected void cmdGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                string strRepriername = string.Empty;
                if (cmbReportType.SelectedItem.Text.Trim() == "--Select--")
                {
                    ShowMsgBox("Please Select Report Type");
                    cmbReportType.Focus();
                    return;
                }
                if (cmbRepairerName.SelectedItem.Text.Trim() == "--Select--")
                {
                    strRepriername = string.Empty;

                }
                else
                {
                    strRepriername = cmbRepairerName.SelectedValue.ToString();
                }
                string sResult=string.Empty;

                if (txtFromDate.Text != "" )
                {
                    sResult = Genaral.DateValidation(txtFromDate.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtFromDate.Focus();
                        return;
                    }
                }
                if (txtToDate.Text != "")
                {

                    sResult = Genaral.DateValidation(txtToDate.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtToDate.Focus();
                        return;
                    }
                }

                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    sResult = Genaral.DateComparision(txtToDate.Text, txtFromDate.Text, false, false);
                    if (sResult == "2")
                    {
                        ShowMsgBox("To Date should be Greater than From Date");
                        txtToDate.Focus();
                        return;

                    }
                }
                
                clsReports objReport = new clsReports();
                string strofficecode = GetOfficeID();
                objReport.sFromDate = txtFromDate.Text;
                objReport.sTodate = txtToDate.Text;
                string strReportType = cmbRepairerName.SelectedItem.Text;

                if (cmbReportType.SelectedItem.ToString() == "Pending Analysis Report")
                {

                    string sParam = "id=Pending Analysis Report&FromDate=" + objReport.sFromDate + "&ToDate=" + objReport.sTodate + "&offcode=" + strofficecode + "&StrReprierName=" + strRepriername;
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + sParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                }
                if (cmbReportType.SelectedItem.ToString() == "Delivered Analysis Report")
                {

                    string sParam = "id=Delivered Analysis Report&FromDate=" + objReport.sFromDate + "&ToDate=" + objReport.sTodate + "&offcode=" + strofficecode + "&StrReprierName=" + strRepriername;
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + sParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        public string GetOfficeID()
        {
            string strOfficeId = string.Empty;
            if (cmbZone.SelectedIndex > 0)
            {
                strOfficeId = cmbZone.SelectedValue.ToString();
            }
            if (cmbCircle.SelectedIndex > 0)
            {
                strOfficeId = cmbCircle.SelectedValue.ToString();
            }

            //if (cmbDiv.SelectedIndex > 0)
            //{
            //    strOfficeId = cmbDiv.SelectedValue.ToString();
            //}

            //if (cmbSubDiv.SelectedIndex > 0)
            //{
            //    strOfficeId = cmbSubDiv.SelectedValue.ToString();
            //}
            //if (cmbOMSection.SelectedIndex > 0)
            //{
            //    strOfficeId = cmbOMSection.SelectedValue.ToString();
            //}

            return (strOfficeId);
        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
               // cmbZone.SelectedIndex = 0;
               // cmbCircle.SelectedIndex = 0;
               // cmbDiv.SelectedIndex = 0;
                //cmbSubDiv.SelectedIndex = 0;
                //cmbOMSection.SelectedIndex = 0;
                cmbRepairerName.SelectedIndex = 0;
                cmbReportType.SelectedIndex = 0;
                txtFromDate.Text = string.Empty;
                txtToDate.Text = string.Empty;
              
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void cmdReset1_Click(object sender, EventArgs e)
        {
            try
            {
                cmbZone4.ClearSelection(); 
                 cmbCircle1.ClearSelection();
                 cmbDiv1.ClearSelection();
                cmbSubDiv1.ClearSelection();
                cmbSection1.ClearSelection();
                cmbwound.ClearSelection();
                cmbguranty1.ClearSelection();
                cmbCoilType.ClearSelection();
                cmbStarType.ClearSelection();
                cmbStage1.ClearSelection();
                cmbCapacity2.ClearSelection();
                cmbMake1.ClearSelection();
                txtFromDate1.Text = string.Empty;
                txtToDate1.Text = string.Empty;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }
        protected void Export_click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dtcompletedRepairCount = new DataTable();
                clsReports objReport = new clsReports();

                string strRepriername = string.Empty;
                if (cmbReportType.SelectedItem.Text.Trim() == "--Select--")
                {
                    ShowMsgBox("Please Select Report Type");
                    cmbReportType.Focus();
                    return;
                }
                if (cmbRepairerName.SelectedItem.Text.Trim() == "--Select--")
                {
                    strRepriername = null;

                }
                else
                {
                    strRepriername = cmbRepairerName.SelectedValue.ToString();
                }

                string sResult = string.Empty;
                if (txtFromDate.Text != "")
                {
                    sResult = Genaral.DateValidation(txtFromDate.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtFromDate.Focus();
                        return;
                    }
                }
                if (txtToDate.Text != "")
                {
                    sResult = Genaral.DateValidation(txtToDate.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtToDate.Focus();
                        return;
                    }
                }

                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {

                    sResult = Genaral.DateComparision(txtToDate.Text, txtFromDate.Text, false, false);
                    if (sResult == "2")
                    {
                        ShowMsgBox("To Date should be Greater than From Date");
                        txtToDate.Focus();
                        return;

                    }
                }

                string strofficecode = GetOfficeID();

                if (txtFromDate.Text != string.Empty)
                {
                    objReport.sFromDate = txtFromDate.Text;
                    DateTime DFromDate = DateTime.ParseExact(objReport.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    objReport.sFromDate = DFromDate.ToString("yyyyMMdd");
                }
                if (txtToDate.Text != string.Empty)
                {
                    objReport.sTodate = txtToDate.Text;
                    DateTime DToDate = DateTime.ParseExact(objReport.sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    objReport.sTodate = DToDate.ToString("yyyyMMdd");
                }
                if (strofficecode != null && strofficecode != "")
                {
                    objReport.sOfficeCode = strofficecode;
                }
                //objReport.sOfficeCode = strofficecode;

                objReport.sRepriername = strRepriername;

                string strReportType = cmbRepairerName.SelectedItem.Text;

                if (cmbReportType.SelectedItem.ToString() == "Pending Analysis Report")
                {
                    dt = objReport.PENDINGWTREPARIER(objReport);
                    // dt1 = objReport.TransformerWiseDetails(objReport);

                }
                if (cmbReportType.SelectedItem.ToString() == "Delivered Analysis Report")
                {
                    dt = objReport.ReperierCompleted(objReport);
                    //dt1 = objReport.TransformerWiseDetailsCompleted(objReport);

                }


                if (dt.Rows.Count > 0)
                {
                    string[] arrAlpha = Genaral.getalpha();


                    using (XLWorkbook wb = new XLWorkbook())
                    {

                        dt.Columns["CIRCLE"].ColumnName = "Circle";
                        dt.Columns["CIRCLE_CODE"].ColumnName = " Circle Off Code";
                        dt.Columns["DURATION"].ColumnName = "Duration";
                        //dt.Columns["DIV_CODE"].ColumnName = "Div Off code";
                        //dt.Columns["DIVISION_NAME"].ColumnName = "Division";
                        dt.Columns["UPTO_25"].ColumnName = "UPTO_25 Capacity";
                        dt.Columns.Remove("FROMDATE");
                        dt.Columns.Remove("TODATE");
                        dt.Columns.Remove("currentdate");


                        wb.Worksheets.Add(dt, "Repairer Performence");
                        //wb.Worksheet(1).Cell(1,5).InsertTable(dtcompletedRepairCount);

                        wb.Worksheet(1).Row(1).InsertRowsAbove(3);
                        string sMergeRange = arrAlpha[dt.Columns.Count - 1];
                        var rangehead = wb.Worksheet(1).Range("A1:" + sMergeRange + "1");
                        rangehead.Merge().Style.Font.SetBold().Font.FontSize = 16;
                        rangehead.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
                        rangehead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        rangehead.SetValue("Hubli Electricity Supply Company Limited, (HESCOM)");

                        var rangeReporthead = wb.Worksheet(1).Range("A2:" + sMergeRange + "2");
                        rangeReporthead.Merge().Style.Font.SetBold().Font.FontSize = 12;
                        rangeReporthead.Merge().Style.Fill.BackgroundColor = XLColor.AirForceBlue;
                        rangeReporthead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        if (cmbReportType.SelectedItem.ToString() == "Pending Analysis Report")
                        {
                            rangeReporthead.SetValue(" Repairer Performance Pending Report ");
                        }
                        else
                        {
                            rangeReporthead.SetValue(" Repairer Perforamance Delivered  Report");
                        }



                        wb.Worksheet(1).Cell(3, 10).Value = DateTime.Now;

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        string FileName = "Repairer Performence " + DateTime.Now + ".xls";
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
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void Export_click4(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            clsReports objReport = new clsReports();
            //if (txtFromDate.Text == "")
            //{
            //    ShowMsgBox("Please Enter From Date");
            //    txtFromDate.Focus();
            //}
            //if (txtToDate.Text == "")
            //{
            //    ShowMsgBox("Please Enter To Date");
            //    txtToDate.Focus();
            //}

            if(cmbStage1.SelectedValue=="0")
            {
                ShowMsgBox("Please select Stage");
                cmbStage1.Focus();
                return;
            }

            string sResult = string.Empty;
            if (txtFromDate1.Text != "")
            {
                sResult = Genaral.DateValidation(txtFromDate1.Text);
                if (sResult != "")
                {
                    ShowMsgBox(sResult);
                    txtFromDate1.Focus();
                    return;
                }
            }

            if (txtToDate1.Text != "")
            {
                sResult = Genaral.DateValidation(txtToDate1.Text);
                if (sResult != "")
                {
                    ShowMsgBox(sResult);
                    txtToDate1.Focus();
                    return;
                }
            }

            if (txtFromDate1.Text != "" && txtToDate1.Text != "")
            {
                sResult = Genaral.DateComparision(txtToDate1.Text, txtFromDate1.Text, false, false);
                if (sResult == "2")
                {
                    ShowMsgBox("To Date should be Greater than From Date");
                    txtToDate1.Focus();
                    return;

                }
            }


            objReport.sType = cmbStage1.SelectedValue.ToString();


             
            if (txtFromDate1.Text != null && txtFromDate1.Text != "")
            {
                objReport.sFromDate = DateTime.ParseExact(txtFromDate1.Text, "dd/MM/yyyy", null).ToString("yyyy/MM/dd");
            }
            else
            {
                objReport.sFromDate = "";
            }
            if (txtToDate1.Text != null && txtToDate1.Text != "")
            {
                objReport.sTodate = DateTime.ParseExact(txtToDate1.Text, "dd/MM/yyyy", null).ToString("yyyy/MM/dd"); ;
            }
            else
            {
                objReport.sTodate = "";
            }

            string strofficecode = GetOfficeID1();
            // objReport.sFromDate = txtFromDate.Text;
            //objReport.sTodate = txtToDate.Text;

            objReport.sOfficeCode = strofficecode;
            if (cmbCapacity2.SelectedIndex > 0)
            {
                objReport.sCapacity = cmbCapacity2.SelectedItem.Text;
            }
            else { objReport.sCapacity = ""; }
            if (cmbMake1.SelectedIndex > 0)
            {
                objReport.sMake = cmbMake1.SelectedValue.ToString();
            }
            else { objReport.sMake = ""; }
            if (cmbCoilType.SelectedIndex > 0)
            {
                objReport.sCoilType = cmbCoilType.SelectedValue.ToString();
            }
            else { objReport.sCoilType = ""; }
            if (cmbguranty1.SelectedIndex > 0)
            {
                objReport.sGuranteeType = cmbguranty1.SelectedValue.ToString();
            }
            else { objReport.sGuranteeType = ""; }
            if (cmbStarType.SelectedIndex > 0)
            {
                objReport.sStarType = cmbStarType.SelectedValue.ToString();
            }
            else { objReport.sStarType = ""; }

            if (cmbwound.SelectedIndex > 0)
            {
                objReport.sWoundType = cmbwound.SelectedValue.ToString();
            }
            else { objReport.sWoundType = ""; }

            dt = objReport.GetRepairerEstandWorkorderexcel(objReport);

            if (dt.Rows.Count > 0)
            {
                int arrAlpha = dt.Columns.Count;




                using (XLWorkbook wb = new XLWorkbook())
                {

                    dt.Columns["dtr code"].ColumnName = "DTR CODE";
                    dt.Columns["est_no"].ColumnName = "ESTIMATION NUMBER";
                    dt.Columns["est_date"].ColumnName = "ESTIMATION DATE";
                    dt.Columns["est_office"].ColumnName = "LOCATION CODE";
                    dt.Columns["capacity1"].ColumnName = "CAPACITY";
                    dt.Columns["est_total_amt"].ColumnName = "ESTIMATION AMOUNT";
                    dt.Columns["coil_type"].ColumnName = "COIL TYPE";
                    dt.Columns["rate_type"].ColumnName = "STAR RATE";
                    dt.Columns["phase"].ColumnName = "PHASES";
                    dt.Columns["wound_type"].ColumnName = "WOUND TYPE";
                    dt.Columns["guarantee"].ColumnName = "GUARANTEE";
                    dt.Columns["rwo_no"].ColumnName = "WORKORDER NO";
                    dt.Columns["rwo_date"].ColumnName = "WORKORDER DATE";
                    dt.Columns["rwo_accode"].ColumnName = "ACC CODE";
                    dt.Columns["rwo_amt"].ColumnName = "WORKORDER AMOUNT";
                    dt.Columns["rwo_ss_wo_no"].ColumnName = "SALE OF SCRAP WORKORDER NO";
                    dt.Columns["rwo_ss_amt"].ColumnName = " SALE OF SCRAP AMOUNT";
                    dt.Columns["fromdate1"].ColumnName = "FROM DATE";
                    dt.Columns["todate1"].ColumnName = "TO DATE";
                    dt.Columns["today"].ColumnName = "TODAY";
                    dt.Columns["make1"].ColumnName = "MAKE NAME";
                    dt.Columns["report_category"].ColumnName = "REPORT CATEGORY";

                    dt.Columns["zone"].SetOrdinal(0);
                    dt.Columns["zone"].ColumnName = "ZONE";
                    dt.Columns["circle"].SetOrdinal(1);
                    dt.Columns["circle"].ColumnName = "CIRCLE";
                    dt.Columns["division"].SetOrdinal(2);
                    dt.Columns["division"].ColumnName = "DIVISION";
                    dt.Columns["subdiv"].SetOrdinal(3);
                    dt.Columns["subdiv"].ColumnName = "SUB DIVISION";
                    dt.Columns["section"].SetOrdinal(4);
                    dt.Columns["section"].ColumnName = "SECTION";



                    wb.Worksheets.Add(dt, "REPAIRER EST AND WORKORDER");


                    wb.Worksheet(1).Row(1).InsertRowsAbove(3);
                    var rngend = wb.Worksheet(1).Cell(1, arrAlpha).Address.ColumnLetter;
                    //  string sMergeRange = arrAlpha[dt.Columns.Count - 1];
                    var rangeheader = wb.Worksheet(1).Range("A1:" + rngend + "1");
                    rangeheader.Merge().Style.Font.SetBold().Font.FontSize = 16;
                    rangeheader.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    rangeheader.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangeheader.SetValue("Hubli Electricity Supply Company Limited, (HESCOM)");

                    var rangeReporthead = wb.Worksheet(1).Range("A2:" + rngend + "2");
                    rangeReporthead.Merge().Style.Font.SetBold().Font.FontSize = 12;
                    rangeReporthead.Merge().Style.Fill.BackgroundColor = XLColor.AirForceBlue;
                    rangeReporthead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);


                    if (txtFromDate1.Text != "" && txtToDate1.Text != "")
                    {
                        rangeReporthead.SetValue("List of DTC with Details  From " + objReport.sFromDate + "  To " + objReport.sTodate);
                    }
                    if (txtFromDate1.Text != "" && txtToDate1.Text == "")
                    {
                        rangeReporthead.SetValue("List of DTC with Details  From " + objReport.sFromDate + "  To " + DateTime.Now);
                    }
                    if (txtFromDate1.Text == "" && txtToDate1.Text != "")
                    {
                        rangeReporthead.SetValue("List of DTC with Details  as on " + objReport.sTodate);
                    }
                    if (txtFromDate1.Text == "" && txtToDate1.Text == "")
                    {
                        rangeReporthead.SetValue("List of DTC with Details as on " + DateTime.Now);
                    }

                    //rangeReporthead.SetValue("List of DTC with Details ");

                    wb.Worksheet(1).Cell(3, 16).Value = DateTime.Now;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    string FileName = "REPAIRER ESTIMATION AND WORKORDER " + DateTime.Now + ".xls";
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
        public string GetOfficeID1()
        {
            string strOfficeId = string.Empty;
            if (cmbZone4.SelectedIndex > 0)
            {
                strOfficeId = cmbZone4.SelectedValue.ToString();
            }
            if (cmbCircle1.SelectedIndex > 0)
            {
                strOfficeId = cmbCircle1.SelectedValue.ToString();
            }

            if (cmbDiv1.SelectedIndex > 0)
            {
                strOfficeId = cmbDiv1.SelectedValue.ToString();
            }

            if (cmbSubDiv1.SelectedIndex > 0)
            {
                strOfficeId = cmbSubDiv1.SelectedValue.ToString();
            }
            if (cmbSection1.SelectedIndex > 0)
            {
                strOfficeId = cmbSection1.SelectedValue.ToString();
            }

            return (strOfficeId);
        }
        protected void cmbDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDiv.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT DISTINCT \"TR_ID\", \"TR_NAME\"  FROM \"TBLTRANSREPAIRER\",\"TBLTRANSREPAIREROFFCODE\" WHERE \"TR_ID\"=\"TRO_TR_ID\" AND \"TR_STATUS\"='A' AND \"TR_ID\" NOT IN (SELECT \"TR_ID\" FROM \"TBLTRANSREPAIRER\",\"TBLTRANSREPAIREROFFCODE\" WHERE \"TR_ID\"=\"TRO_TR_ID\" AND \"TR_BLACK_LISTED\"=1 AND \"TR_BLACKED_UPTO\">=CURRENT_DATE ) AND CAST(\"TRO_OFF_CODE\" AS TEXT) LIKE'" + cmbDiv.SelectedValue + "%' ORDER BY \"TR_NAME\"", "--Select--", cmbRepairerName);
                }
                else
                {
                    if (objSession.OfficeCode != "")
                    {
                        Genaral.Load_Combo("SELECT DISTINCT \"TR_ID\", \"TR_NAME\"  FROM \"TBLTRANSREPAIRER\",\"TBLTRANSREPAIREROFFCODE\" WHERE \"TR_ID\"=\"TRO_TR_ID\" AND \"TR_STATUS\"='A' AND \"TR_ID\" NOT IN (SELECT \"TR_ID\" FROM \"TBLTRANSREPAIRER\",\"TBLTRANSREPAIREROFFCODE\" WHERE \"TR_ID\"=\"TRO_TR_ID\" AND \"TR_BLACK_LISTED\"=1 AND \"TR_BLACKED_UPTO\">=CURRENT_DATE ) AND CAST(\"TRO_OFF_CODE\" AS TEXT) LIKE'" + objSession.OfficeCode.Substring(0, 2) + "%' ORDER BY \"TR_NAME\"", "--Select--", cmbRepairerName);
                    }
                    Genaral.Load_Combo("SELECT DISTINCT \"TR_ID\", \"TR_NAME\"  FROM \"TBLTRANSREPAIRER\",\"TBLTRANSREPAIREROFFCODE\" WHERE  \"TR_ID\"=\"TRO_TR_ID\" AND \"TR_STATUS\"='A' AND \"TR_ID\" NOT IN (SELECT \"TR_ID\" FROM \"TBLTRANSREPAIRER\",\"TBLTRANSREPAIREROFFCODE\" WHERE \"TR_ID\"=\"TRO_TR_ID\" AND \"TR_BLACK_LISTED\"=1 AND \"TR_BLACKED_UPTO\">=CURRENT_DATE ) AND CAST(\"TRO_OFF_CODE\" AS TEXT) LIKE'" + objSession.OfficeCode + "%' ORDER BY \"TR_NAME\"", "--Select--", cmbRepairerName);
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