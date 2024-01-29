using ClosedXML.Excel;
using IIITS.DTLMS.BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.Reports
{
    public partial class CRegAbstract : System.Web.UI.Page
    {
        string strFormCode = "CRegAbstract";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];
                string stroffCode = string.Empty;
                if (objSession.OfficeCode.Length <= 2 && objSession.OfficeCode.Length != 0)
                {
                    stroffCode = clsStoreOffice.GetZone_Circle_Div_Offcode(objSession.OfficeCode, objSession.RoleId).Substring(0, Constants.Circle);
                }
                else
                {
                    stroffCode = objSession.OfficeCode;
                }
                string stroffCode1 = stroffCode;
                //CalendarExtender3.EndDate = System.DateTime.Now.AddDays(0);
                //CalendarExtender4.EndDate = System.DateTime.Now.AddDays(0);
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");

                CalendarExtender4.EndDate = System.DateTime.Now;
                CalendarExtender3.EndDate = System.DateTime.Now;

                if (!IsPostBack)
                {

                    //Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle);
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
                        if (stroffCode.Length >= 2)
                        {
                            stroffCode = stroffCode.Substring(0, Constants.Circle);
                            cmbCircle.Items.FindByValue(stroffCode).Selected = true;
                            cmbCircle.Enabled = false;
                            stroffCode = stroffCode1;
                        }
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
                        Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_SUBDIV_CODE\"='" + cmbSubDiv.SelectedValue + "'", "--Select--", cmbOMSection);
                        Genaral.Load_Combo("SELECT \"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" and CAST(\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbSubDiv.SelectedValue + "%'  ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName);
                        if (stroffCode.Length >= 5)
                        {
                            stroffCode = stroffCode.Substring(0, Constants.Section);
                            cmbOMSection.Items.FindByValue(stroffCode).Selected = true;
                            cmbOMSection.Enabled = false;
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
        protected void cmbZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbZone.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_ZO_ID\"='" + cmbZone.SelectedValue + "'", "--Select--", cmbCircle);
                    cmbDiv.Items.Clear();
                    cmbSubDiv.Items.Clear();
                    cmbOMSection.Items.Clear();
                }

                else
                {
                    cmbCircle.Items.Clear();
                    cmbDiv.Items.Clear();
                    cmbSubDiv.Items.Clear();
                    cmbOMSection.Items.Clear();

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
                    cmbSubDiv.Items.Clear();
                    cmbOMSection.Items.Clear();
                }

                else
                {
                    cmbDiv.Items.Clear();
                    cmbSubDiv.Items.Clear();
                    cmbOMSection.Items.Clear();

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDiv.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE \"SD_DIV_CODE\"='" + cmbDiv.SelectedValue + "'", "--Select--", cmbSubDiv);
                    cmbOMSection.Items.Clear();
                }
                else
                {
                    cmbSubDiv.Items.Clear();
                    cmbOMSection.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbSubDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubDiv.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_SUBDIV_CODE\"='" + cmbSubDiv.SelectedValue + "'", "--Select--", cmbOMSection);

                    Genaral.Load_Combo("SELECT \"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" and CAST(\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbSubDiv.SelectedValue + "%'  ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName);
                }
                else
                {
                    cmbOMSection.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdReport_Click(object sender, EventArgs e)
        {
            try
            {
                clsReports objReport = new clsReports();
                //if (txtFromDate.Text == "")
                //{
                //    ShowMsgBox("Please Enter From Date");
                //    txtFromDate.Focus();
                //    return;

                //}
                //if (txtToDate.Text == "")
                //{
                //    ShowMsgBox("Please Enter To Date");
                //    txtToDate.Focus();
                //    return;
                //}

                //string sResult = Genaral.DateValidation(txtFromDate.Text);
                //if (sResult != "")
                //{
                //    ShowMsgBox(sResult);
                //    return;
                //}

                //sResult = Genaral.DateValidation(txtToDate.Text);
                //if (sResult != "")
                //{
                //    ShowMsgBox(sResult);
                //    return;
                //}

                string sResult = string.Empty;
                if (txtFromDate.Text == "")
                {
                    sResult = Genaral.DateValidation(txtFromDate.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtFromDate.Focus();
                        return;
                    }
                }
                if (txtToDate.Text == "")
                {
                    sResult = Genaral.DateValidation(txtToDate.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtToDate.Focus();
                        return;
                    }
                }

                if (txtToDate.Text != "" && txtFromDate.Text != "")
                {
                    sResult = Genaral.DateComparision(txtToDate.Text, txtFromDate.Text, false, false);
                    if (sResult == "2")
                    {
                        ShowMsgBox("To Date should be Greater than From Date");
                        txtToDate.Focus();
                        return;

                    }

                }
                if (cmbFeederName.SelectedIndex != 0)
                {
                    objReport.sFeeder = cmbFeederName.SelectedValue;
                }
                if (cmbOMSection.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbOMSection.SelectedValue;
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
                    objReport.sOfficeCode = cmbCircle.SelectedValue;
                else if (cmbZone.SelectedIndex > 0)
                    objReport.sOfficeCode = cmbZone.SelectedValue;
                else
                    objReport.sOfficeCode = GetOfficeID();

                objReport.sFromDate = txtFromDate.Text;
                objReport.sTodate = txtToDate.Text;

                //"&Circle=" + objReport.sCircle + "&Division=" + objReport.sDivision + "&SubDivision=" + objReport.sSubDivision+"&Section=" + objReport.sSection
                string sParam = "id=RegAbstract&FromDate=" + objReport.sFromDate + "&ToDate=" + objReport.sTodate + "&offcode=" + objReport.sOfficeCode+"&FeederName=" + objReport.sFeeder ;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + sParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

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

            if (cmbDiv.SelectedIndex > 0)
            {
                strOfficeId = cmbDiv.SelectedValue.ToString();
            }

            if (cmbSubDiv.SelectedIndex > 0)
            {
                strOfficeId = cmbSubDiv.SelectedValue.ToString();
            }
            if (cmbOMSection.SelectedIndex > 0)
            {
                strOfficeId = cmbOMSection.SelectedValue.ToString();
            }

            return (strOfficeId);
        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtFromDate.Text = string.Empty;
                txtToDate.Text = string.Empty;
               // cmbZone.SelectedIndex = 0;
               // cmbCircle.Items.Clear();
                //cmbSubDiv.Items.Clear();
               // cmbDiv.Items.Clear();
               // cmbOMSection.Items.Clear();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void Export_clickCregAbstract(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            clsReports objReport = new clsReports();

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

            if (txtToDate.Text != "" && txtFromDate.Text != "")
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
            objReport.sOfficeCode = strofficecode;
            if (txtFromDate.Text == "")
            {
                objReport.sFromDate = null;
                objReport.sTodate = null;
            }
            else
            {
                objReport.sFromDate = txtFromDate.Text;
                objReport.sTodate = txtToDate.Text;
            }


            dt = objReport.PrintRegAbstact(objReport);

            if (dt.Rows.Count > 0)
            {
                string[] arrAlpha = Genaral.getalpha();

               
                using (XLWorkbook wb = new XLWorkbook())
                {
                    dt.Columns["CIRCLE"].ColumnName = "Circle";
                    dt.Columns["DIVISION"].ColumnName = "Division";
                    dt.Columns["SUBDIVISION"].ColumnName = "SubDivision";
                    dt.Columns["EST_NO"].ColumnName = "Estimation No";
                    dt.Columns["EST_CRON"].ColumnName = "Estimate Date";
                    dt.Columns["WO_NO"].ColumnName = "Wo No";
                    dt.Columns["WO_NO_DECOM"].ColumnName = "Decommission Wo/no";
                    dt.Columns["WO_DATE"].ColumnName = "Wo Date";
                    dt.Columns["WO_DATE_DECOM"].ColumnName = "Decommission Date";
                    dt.Columns["TC_CAPACITY"].ColumnName = "Tc Capacity";
                    dt.Columns["DF_REASON"].ColumnName = "Failure Reason";
                    dt.Columns["IN_INV_NO"].ColumnName = "Invoice No ";
                    dt.Columns["IN_DATE"].ColumnName = "Invoice Date";
                    dt.Columns["REPLACE_CAPACITY"].ColumnName = "Replace Capacity";
                   
                    dt.Columns["SECTION"].SetOrdinal(3);
                    dt.Columns["SECTION"].ColumnName = "Section";
                    dt.Columns.Remove("FROMDATE");
                    dt.Columns.Remove("TODATE");
                    dt.Columns.Remove("currentdate");
                    

                    wb.Worksheets.Add(dt, "CregAbstract");

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

                    if (txtFromDate.Text != "" && txtToDate.Text != "")
                    {
                        rangeReporthead.SetValue("C REGISTER ABSTRACT REPROT  From " + objReport.sFromDate + "  To " + objReport.sTodate);
                    }
                    if (txtFromDate.Text != "" && txtToDate.Text == "")
                    {
                        rangeReporthead.SetValue("C REGISTER ABSTRACT REPROT  From " + objReport.sFromDate + "  To " + DateTime.Now);
                    }
                    if (txtFromDate.Text == "" && txtToDate.Text != "")
                    {
                        rangeReporthead.SetValue("C REGISTER ABSTRACT REPROT  as on " + objReport.sTodate);
                    }
                    if (txtFromDate.Text == "" && txtToDate.Text == "")
                    {
                        rangeReporthead.SetValue("C REGISTER ABSTRACT REPROT as on " + DateTime.Now);
                    }

                   
                    //if (objReport.sFromDate != null)
                    //{
                    //    rangeReporthead.SetValue("C REGISTER ABSTRACT REPROT   From" + objReport.sFromDate + "  To" + objReport.sTodate);
                    //}
                    //else
                    //{
                    //    rangeReporthead.SetValue("C REGISTER ABSTRACT REPROT   ");

                    //}

                    wb.Worksheet(1).Cell(3, 16).Value = DateTime.Now;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    string FileName = "CregAbstract " + DateTime.Now + ".xls";
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