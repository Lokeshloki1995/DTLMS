using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.IO;
using ClosedXML.Excel;

namespace IIITS.DTLMS.Reports
{
    public partial class AbstractReport : System.Web.UI.Page
    {
        clsSession objSession = new clsSession();
        string strFormCode = "AbstractReport";
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
                //txtFromDate1.Attributes.Add("readonly", "readonly");
                //txtToDate1.Attributes.Add("readonly", "readonly");
                //CalendarExtender1.EndDate = System.DateTime.Now;
                //CalendarExtender2.EndDate = System.DateTime.Now;

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
                    if (stroffCode.Length >= 1)
                    {
                        Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone1);
                        Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone2);
                        stroffCode = stroffCode.Substring(0, Constants.Zone);
                        cmbZone1.Items.FindByValue(stroffCode).Selected = true;
                        cmbZone2.Items.FindByValue(stroffCode).Selected = true;
                        cmbZone1.Enabled = false;
                        cmbZone2.Enabled = false;
                        stroffCode = stroffCode1;
                    }
                    if (stroffCode == null || stroffCode == "")
                    {
                        Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone1);
                        Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone2);
                    }
                    if (stroffCode.Length >= 1)
                    {
                        Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_ZO_ID\"='" + cmbZone1.SelectedValue + "'", "--Select--", cmbCircle1);
                        Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_ZO_ID\"='" + cmbZone2.SelectedValue + "'", "--Select--", cmbCircle2);

                        {
                            stroffCode = stroffCode.Substring(0, Constants.Circle);
                            cmbCircle1.Items.FindByValue(stroffCode).Selected = true;
                            cmbCircle2.Items.FindByValue(stroffCode).Selected = true;
                            cmbCircle1.Enabled = false;
                            cmbCircle2.Enabled = false;
                            stroffCode = stroffCode1;

                        }


                    }
                    if (stroffCode.Length >= 2)
                    {
                        Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE \"DIV_CICLE_CODE\"='" + cmbCircle1.SelectedValue + "'", "--Select--", cmbDiv1);
                        Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE \"DIV_CICLE_CODE\"='" + cmbCircle2.SelectedValue + "'", "--Select--", cmbDiv2);
                        //cmbCircle1.Items.FindByValue(stroffCode).Selected = true;
                        //cmbCircle2.Items.FindByValue(stroffCode).Selected = true;
                        //stroffCode = string.Empty;
                        //stroffCode = objSession.OfficeCode;
                        if (stroffCode.Length >= 3)
                        {
                            stroffCode = stroffCode.Substring(0, Constants.Division);
                            cmbDiv1.Items.FindByValue(stroffCode).Selected = true;
                            cmbDiv2.Items.FindByValue(stroffCode).Selected = true;
                            cmbDiv1.Enabled = false;
                            cmbDiv2.Enabled = false;
                            stroffCode = stroffCode1;

                        }

                    }

                    if (stroffCode.Length >= 3)
                    {
                        Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE \"SD_DIV_CODE\"='" + cmbDiv1.SelectedValue + "'", "--Select--", cmbSubDiv1);
                        Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE \"SD_DIV_CODE\"='" + cmbDiv2.SelectedValue + "'", "--Select--", cmbSubDiv2);
                        if (stroffCode.Length >= 4)
                        {
                            stroffCode = stroffCode.Substring(0, Constants.SubDivision);
                            cmbSubDiv1.Items.FindByValue(stroffCode).Selected = true;
                            cmbSubDiv2.Items.FindByValue(stroffCode).Selected = true;
                            cmbSubDiv1.Enabled = false;
                            cmbSubDiv2.Enabled = false;
                            stroffCode = stroffCode1;

                        }
                    }
                    if (stroffCode.Length >= 4)
                    {
                        Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_SUBDIV_CODE\"='" + cmbSubDiv1.SelectedValue + "'", "--Select--", cmbSection1);
                        Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_SUBDIV_CODE\"='" + cmbSubDiv2.SelectedValue + "'", "--Select--", cmbSection);
                        Genaral.Load_Combo("SELECT \"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" and CAST(\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbSubDiv1.SelectedValue + "%'  ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName1);
                        Genaral.Load_Combo("SELECT \"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" and CAST(\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbSubDiv2.SelectedValue + "%'  ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName2);
                        if (stroffCode.Length >= 5)
                        {
                            stroffCode = stroffCode.Substring(0, Constants.Section);
                            cmbSection1.Items.FindByValue(stroffCode).Selected = true;
                            cmbSection.Items.FindByValue(stroffCode).Selected = true;
                            cmbSection1.Enabled = false;
                            cmbSection.Enabled = false;
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

        protected void cmdReport_Click(object sender, EventArgs e)
        {
            try
            {
                string strOfficeCode = string.Empty;
                string strParam = string.Empty;

                clsReports objReport = new clsReports();

                string sResult = string.Empty;
                //if (txtFromDate1.Text != "")
                //{
                //    sResult = Genaral.DateValidation(txtFromDate1.Text);
                //    if (sResult != "")
                //    {
                //        ShowMsgBox(sResult);
                //        txtFromDate1.Focus();
                //        return;
                //    }
                //}

                //if (txtToDate1.Text != "")
                //{
                //    sResult = Genaral.DateValidation(txtToDate1.Text);
                //    if (sResult != "")
                //    {
                //        ShowMsgBox(sResult);
                //        txtToDate1.Focus();
                //        return;
                //    }
                //}

                //if (txtFromDate1.Text != "" && txtToDate1.Text != "")
                //{
                //    sResult = Genaral.DateComparision(txtToDate1.Text, txtFromDate1.Text, false, false);
                //    if (sResult == "2")
                //    {
                //        ShowMsgBox("To Date should be Greater than From Date");
                //        txtToDate1.Focus();
                //        return;

                //    }
                //}
                if (cmbFeederName1.SelectedIndex != 0)
                {
                    objReport.sFeeder = cmbFeederName1.SelectedValue;
                }


                if (cmbSection1.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbSection1.SelectedValue;
                }

                else if (cmbSubDiv1.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbSubDiv1.SelectedValue;
                }
                else if (cmbDiv1.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbDiv1.SelectedValue;
                }
                else if (cmbCircle1.SelectedIndex > 0)
                    objReport.sOfficeCode = cmbCircle1.SelectedValue;
                else if (cmbZone1.SelectedIndex > 0)
                    objReport.sOfficeCode = cmbZone1.SelectedValue;

                else objReport.sOfficeCode = "";

                //objReport.sFromDate = txtFromDate1.Text;
                //objReport.sTodate = txtToDate1.Text;


                objReport.sFromDate = string.Empty;
                objReport.sTodate = string.Empty;

                strParam = "id=AbstractReport&Officecode=" + objReport.sOfficeCode + "&FromDate=" + objReport.sFromDate + "&FeederName=" + objReport.sFeeder + "&ToDate=" + objReport.sTodate; ;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbZone1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                    cmbCircle1.Items.Clear();
                    cmbDiv1.Items.Clear();
                    cmbSubDiv1.Items.Clear();
                    cmbSection1.Items.Clear();
                    cmbFeederName1.Items.Clear();
                    Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE CAST(\"CM_CIRCLE_CODE\" AS TEXT) LIKE'" + cmbZone1.SelectedValue + "%'", "--Select--", cmbCircle1);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbZone2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbCircle2.Items.Clear();
                cmbDiv2.Items.Clear();
                cmbSubDiv2.Items.Clear();
                cmbSection.Items.Clear();
                cmbFeederName2.Items.Clear();
                Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE CAST(\"CM_CIRCLE_CODE\" AS TEXT) LIKE'" + cmbZone2.SelectedValue + "%'", "--Select--", cmbCircle2);
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
                    Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CICLE_CODE\" AS TEXT)='" + cmbCircle1.SelectedValue + "'", "--Select--", cmbDiv1);
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
       protected void cmbCircle2_SelectedIndexChanged(object sender, EventArgs e)
       {
           try
           {
             

                if (cmbCircle2.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CICLE_CODE\" AS TEXT)='" + cmbCircle2.SelectedValue + "'", "--Select--", cmbDiv2);
                    cmbSubDiv2.Items.Clear();
                    cmbSection.Items.Clear();
                }
                else
                {
                 
                    cmbDiv2.Items.Clear();
                    cmbSubDiv2.Items.Clear();
                    cmbSection.Items.Clear();
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
                if (cmbDiv1.SelectedIndex > 0 )
                {
                    Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE CAST(\"SD_DIV_CODE\" AS TEXT)='" + cmbDiv1.SelectedValue + "'", "--Select--", cmbSubDiv1);
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
        protected void cmbDiv2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDiv2.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE CAST(\"SD_DIV_CODE\" AS TEXT)='" + cmbDiv2.SelectedValue + "'", "--Select--", cmbSubDiv2);
                    cmbSection.Items.Clear();
                }
                else
                {
                    cmbSubDiv2.Items.Clear();
                    cmbSection.Items.Clear();
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
                if (cmbSubDiv1.SelectedIndex > 0 )
                {
                    Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE CAST(\"OM_SUBDIV_CODE\" AS TEXT)='" + cmbSubDiv1.SelectedValue + "'", "--Select--", cmbSection1);

                    Genaral.Load_Combo("SELECT DISTINCT \"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" and CAST(\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbSubDiv1.SelectedValue + "%'  ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName1);
                    
                }

                else
                {
                    cmbSection1.Items.Clear();
                    cmbFeederName1.Items.Clear();

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void cmbSubDiv2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubDiv2.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE CAST(\"OM_SUBDIV_CODE\" AS TEXT)='" + cmbSubDiv2.SelectedValue + "'", "--Select--", cmbSection);
                    Genaral.Load_Combo("SELECT DISTINCT \"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" and CAST(\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbSubDiv2.SelectedValue + "%'  ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName2);
                }
                else
                {
                    cmbSection.Items.Clear();
                    cmbFeederName2.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                clsReports objReport = new clsReports();
               

                 string sResult=string.Empty;
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
                 if (cmbFeederName2.SelectedIndex != 0)
                 {
                     objReport.sFeeder = cmbFeederName2.SelectedValue;
                 }

                     if (cmbSection.SelectedIndex > 0)
                     {
                         objReport.sOfficeCode = cmbSection.SelectedValue;
                     }

                     else if (cmbSubDiv2.SelectedIndex > 0)
                     {
                         objReport.sOfficeCode = cmbSubDiv2.SelectedValue;
                     }
                     else if (cmbDiv2.SelectedIndex > 0)
                     {
                         objReport.sOfficeCode = cmbDiv2.SelectedValue;
                     }
                     else if (cmbCircle2.SelectedIndex > 0)
                         objReport.sOfficeCode = cmbCircle2.SelectedValue;
                     else if(cmbZone2.SelectedIndex >0 )
                         objReport.sOfficeCode = cmbZone2.SelectedValue;
                     else objReport.sOfficeCode = "";

                     objReport.sFromDate = txtFromDate.Text;
                     objReport.sTodate = txtToDate.Text;

                     string sParam = "id=CRAbstract&Officecode=" + objReport.sOfficeCode + "&FromDate=" + objReport.sFromDate + "&FeederName=" + objReport.sFeeder + "&ToDate=" + objReport.sTodate;
                     RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + sParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
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

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            DataTable dtCRDetails = new DataTable();
            try
            {
                clsCRReport objCrReport =new clsCRReport();
                if (txtDTCCode.Text == "" || txtDTCCode.Text == null)
                {
                    ShowMsgBox("Please Enter DTC CODE that completed CR");
                }
                dtCRDetails = objCrReport.GetCRDetails(txtDTCCode.Text);
                
                if (dtCRDetails.Rows.Count > 0)
                {
                    FailureID.Value = dtCRDetails.Rows[0]["DF_ID"].ToString();
                    ViewState["CRDetails"] = dtCRDetails;
                    GrdCrDetail.DataSource = dtCRDetails;
                    GrdCrDetail.DataBind();
                }
                else
                {
                    ShowMsgBox("DTC Not Failed or CR Not completed");
                    ShowEmptyGrid();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("DF_DTC_CODE");
                dt.Columns.Add("DF_EQUIPMENT_ID");
                dt.Columns.Add("WO_NO");
                dt.Columns.Add("WO_DATE");
                dt.Columns.Add("WO_NO_DECOM");

                GrdCrDetail.DataSource = dt;
                GrdCrDetail.DataBind();

                int iColCount = GrdCrDetail.Rows[0].Cells.Count;
                GrdCrDetail.Rows[0].Cells.Clear();
                GrdCrDetail.Rows[0].Cells.Add(new TableCell());
                GrdCrDetail.Rows[0].Cells[0].ColumnSpan = iColCount;
                GrdCrDetail.Rows[0].Cells[0].Text = "No Records Found";
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void GrdCrDetail_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "View")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    string sDTCCODE = ((Label)row.FindControl("lblDtcCode")).Text;

                    string sParam = "id=CRDetails&FailureId=" + Genaral.Encrypt( FailureID.Value) + "&DTCCODE=" + Genaral.Encrypt( sDTCCODE);
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + sParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                    //Response.Redirect("/Reports/ReportView.aspx?id=CRDetails&FailureId=" + Genaral.Encrypt( FailureID.Value) + "&DTCCODE=" + Genaral.Encrypt( sDTCCODE), false);

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void GrdCrDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdCrDetail.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["CRDetails"];
                GrdCrDetail.DataSource = dt;
                GrdCrDetail.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void BtnReset_Click(object sender, EventArgs e)
        {
            try
            {
               

                txtDTCCode.Text = "";
                GrdCrDetail.DataSource = null;
                GrdCrDetail.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void BtnReset1_Click(object sender, EventArgs e)
        {
            try
            {
                cmbZone1.SelectedIndex = 0;
                cmbCircle1.Items.Clear();
                cmbDiv1.Items.Clear();
                cmbSubDiv1.Items.Clear();
                cmbSection1.Items.Clear();
                cmbFeederName1.Items.Clear();
          
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void BtnReset2_Click(object sender, EventArgs e)
        {
            try
            {
                txtFromDate.Text = "";
                txtToDate.Text = "";

                cmbZone2.SelectedIndex = 0;
                cmbCircle2.Items.Clear();
                cmbDiv2.Items.Clear();
                cmbSubDiv2.Items.Clear();
                cmbSection.Items.Clear();
                cmbFeederName2.Items.Clear();

                //  cmbCircle2.SelectedIndex = 0;
                // cmbDiv2.Items.Clear();
                //  cmbSubDiv2.Items.Clear();
                // cmbSection.Items.Clear();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void Export_clickFailureAbstract(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            clsReports objReport = new clsReports();
            string sResult = string.Empty;
          

            if (cmbSection1.SelectedIndex > 0)
            {
                objReport.sOfficeCode = cmbSection1.SelectedValue;
            }

            else if (cmbSubDiv1.SelectedIndex > 0)
            {
                objReport.sOfficeCode = cmbSubDiv1.SelectedValue;
            }
            else if (cmbDiv1.SelectedIndex > 0)
            {
                objReport.sOfficeCode = cmbDiv1.SelectedValue;
            }
            else if (cmbCircle1.SelectedIndex > 0)
                objReport.sOfficeCode = cmbCircle1.SelectedValue;
            else objReport.sOfficeCode = "";
            

            dt = objReport.PrintAbstractReport(objReport);

            if (dt.Rows.Count > 0)
            {
                string[] arrAlpha = Genaral.getalpha();

                string sMergeRange = arrAlpha[dt.Columns.Count - 1];
                using (XLWorkbook wb = new XLWorkbook())
                {

                    
                    dt.Columns["CIRCLE"].ColumnName = "Circle";
                    dt.Columns["DIV"].ColumnName = "Division";
                    dt.Columns["SUBDIV"].ColumnName = "SubDivision";
                    dt.Columns["ONEWEEKCOUNT"].ColumnName = "1 Week";
                    dt.Columns["FORTNIGHTCOUNT"].ColumnName = "Fort Night";
                    dt.Columns["MONTHCOUNT"].ColumnName = "1 Month";
                    dt.Columns["MORETHENMONTHCOUNT"].ColumnName = "> 1 Month";


                    dt.Columns["TC_CAPACITY"].SetOrdinal(4);
                    dt.Columns["TC_CAPACITY"].ColumnName = "TcCapacity";
                    dt.Columns.Remove("FROMDATE");
                    dt.Columns.Remove("TODATE");
                    wb.Worksheets.Add(dt, "FailureAbstract");

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

                   rangeReporthead.SetValue("TRANSFORMER FAILURE ABSTRACT REPORT AS ON " + DateTime.Now);

                    //if (txtFromDate1.Text != "" && txtToDate1.Text != "")
                    //{
                    //    rangeReporthead.SetValue("TRANSFORMER FAILURE ABSTRACT REPORT  From " + objReport.sFromDate + "  To " + objReport.sTodate);
                    //}
                    //if (txtFromDate1.Text != "" && txtToDate1.Text == "")
                    //{
                    //    rangeReporthead.SetValue("TRANSFORMER FAILURE ABSTRACT REPORT  From " + objReport.sFromDate + "  To " + DateTime.Now);
                    //}
                    //if (txtFromDate1.Text == "" && txtToDate1.Text != "")
                    //{
                    //    rangeReporthead.SetValue("TRANSFORMER FAILURE ABSTRACT REPORT  as on " + objReport.sTodate);
                    //}
                    //if (txtFromDate1.Text == "" && txtToDate1.Text == "")
                    //{
                    //    rangeReporthead.SetValue("TRANSFORMER FAILURE ABSTRACT REPORT as on " + DateTime.Now);
                    //}
                    wb.Worksheet(1).Cell(3, 8).Value = DateTime.Now;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    string FileName = "FailureAbstract " + DateTime.Now + ".xls";
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
        protected void Export_clickCrdeatails(object sender, EventArgs e)
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
            
            if (cmbSection.SelectedIndex > 0)
            {
                objReport.sOfficeCode = cmbSection.SelectedValue;
            }

            else if (cmbSubDiv2.SelectedIndex > 0)
            {
                objReport.sOfficeCode = cmbSubDiv2.SelectedValue;
            }
            else if (cmbDiv2.SelectedIndex > 0)
            {
                objReport.sOfficeCode = cmbDiv2.SelectedValue;
            }
            else if (cmbCircle2.SelectedIndex > 0)
                objReport.sOfficeCode = cmbCircle2.SelectedValue;
            else objReport.sOfficeCode = "";

            //objReport.sFromDate = txtFromDate.Text;
            //objReport.sTodate = txtToDate.Text;


            if (txtFromDate.Text != null && txtFromDate.Text != "")
            {
                objReport.sFromDate = txtFromDate.Text;
                DateTime DToDate = DateTime.ParseExact(objReport.sFromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");
            }
            if (txtToDate.Text != null && txtToDate.Text != "")
            {
                objReport.sTodate = txtToDate.Text;
                DateTime DToDate = DateTime.ParseExact(objReport.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
            }

            dt = objReport.CRAbstract(objReport);

            if (dt.Rows.Count > 0)
            {
                string[] arrAlpha = Genaral.getalpha();

              
                using (XLWorkbook wb = new XLWorkbook())
                {
                    dt.Columns["CIRCLE"].ColumnName = "Circle";
                    dt.Columns["DIVISION"].ColumnName = "Division";
                    dt.Columns["SUBDIVISION"].ColumnName = "SubDivision";
                    dt.Columns["SECTION"].ColumnName = "Section";
                    dt.Columns["NOMENCLATURE"].ColumnName = "Failure Type";
                    dt.Columns["DF_DTC_CODE"].ColumnName = "DTC Code";
                    dt.Columns["DF_EQUIPMENT_ID"].ColumnName = "DTR Code";
                    dt.Columns["WO_NO"].ColumnName = "Wo No";
                    dt.Columns["WO_NO_DECOM"].ColumnName = "Decommissioning Wo/No";
                    dt.Columns["WO_DATE"].ColumnName = "Wo Date";
                    dt.Columns["EST_NO"].ColumnName = "Estimation No";
                    dt.Columns["EST_CRON"].ColumnName = "Estimation Date";
                    dt.Columns["TI_INDENT_NO"].ColumnName = "Indent No";
                    dt.Columns["TI_INDENT_DATE"].ColumnName = "Indent Date";
                    dt.Columns["IN_INV_NO"].ColumnName = "Invoice No";
                    dt.Columns["IN_DATE"].ColumnName = "Invoice Date";
                    dt.Columns["TR_RI_NO"].ColumnName = "RI No";
                    dt.Columns["TR_RI_DATE"].ColumnName = "RI Date";
                    dt.Columns["TR_RV_NO"].ColumnName = "RV No";
                    dt.Columns["TR_RV_DATE"].ColumnName = "RV Date";
                    dt.Columns["TR_CR_DATE"].ColumnName = "CR Date";
                    dt.Columns["TM_MAPPING_DATE"].SetOrdinal(27);

                    dt.Columns["TM_MAPPING_DATE"].ColumnName = "New Commission Date";
                    dt.Columns["TODATE"].SetOrdinal(5);
                    dt.Columns.Remove("TODATE");
                    dt.Columns.Remove("FROMDATE");
                    dt.Columns.Remove("CURRENTDATE");

                    wb.Worksheets.Add(dt, "CRDetails");



            

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
                        rangeReporthead.SetValue("CR ABSTRACT REPORT  From " + objReport.sFromDate + "  To " + objReport.sTodate);
                    }
                    if (txtFromDate.Text != "" && txtToDate.Text == "")
                    {
                        rangeReporthead.SetValue("CR ABSTRACT REPORT  From " + objReport.sFromDate + "  To " + DateTime.Now);
                    }
                    if (txtFromDate.Text == "" && txtToDate.Text != "")
                    {
                        rangeReporthead.SetValue("CR ABSTRACT REPORT  as on " + objReport.sTodate);
                    }
                    if (txtFromDate.Text == "" && txtToDate.Text == "")
                    {
                        rangeReporthead.SetValue("CR ABSTRACT REPORT as on " + DateTime.Now);
                    }

                    //if (txtFromDate.Text != "" && txtToDate.Text != "")
                    //{
                    //    rangeReporthead.SetValue("CR ABSTRACT REPORT  From " + objReport.sFromDate + "  To " + objReport.sTodate);
                    //}

                    //else
                    //{
                    //    rangeReporthead.SetValue("CR ABSTRACT REPORT  ");

                    //}

                    wb.Worksheet(1).Cell(3, 20).Value = DateTime.Now;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    string FileName = "CRDetails " + DateTime.Now + ".xls";
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