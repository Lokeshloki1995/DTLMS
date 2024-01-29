using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;


namespace IIITS.DTLMS.Maintainance
{
    public partial class TcMaintainance : System.Web.UI.Page
    {
        string strFormCode = "TcMaintainance";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                else
                {
                    objSession = (clsSession)Session["clsSession"];
                    lblMessage.Text = string.Empty;
                    txtInspDate.Attributes.Add("readonly", "readonly");

                    CalendarExtender1.EndDate = System.DateTime.Now;

                    if (!IsPostBack)
                    {

                        Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'OL' ORDER BY \"MD_ORDER_BY\"", "--Select--", cmboilLevel);
                        Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'LI' ORDER BY \"MD_ORDER_BY\"", "--Select--", cmbArrester);

                        //Redirected from  Maintainance Details
                        if (Request.QueryString["MaintId"] != null && Convert.ToString(Request.QueryString["MaintId"]) != "")
                        {
                            txtMaintanceID.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["MaintId"]));
                            GetTcMaintainanceDeatils(txtMaintanceID.Text);
                            cmdReset.Visible = false;
                            txtDtcCode.ReadOnly = true;
                        }

                        //Redirected from Preventive Maintainance Details
                        if (Request.QueryString["DtcCode"] != null && Convert.ToString(Request.QueryString["DtcCode"]) != "")
                        {
                            txtDtcCode.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["DtcCode"]));
                            txtform.Text = "Preventive";
                            GetTCDetials(txtDtcCode.Text);
                            txtDtcCode.ReadOnly = true;
                        }


                        string strQry = string.Empty;
                        strQry = "Title=Search and Select Transformer Centre Details&";
                        strQry += "Query=select \"DT_CODE\",\"DT_NAME\"  FROM \"TBLDTCMAST\" WHERE CAST(\"DT_OM_SLNO\" AS TEXT) LIKE '" + objSession.OfficeCode + "%' AND CAST({0} AS TEXT) like %{1}% order by \"DT_NAME\"&";
                        strQry += "DBColName=\"DT_NAME\"~\"DT_CODE\"&";
                        strQry += "ColDisplayName=\"DT_NAME\"~\"DT_CODE\"&";

                        strQry = strQry.Replace("'", @"\'");
                        cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtDtcCode.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + txtDtcCode.ClientID + ")");

                        txtInspDate.Attributes.Add("onblur", "return ValidateDate(" + txtInspDate.ClientID + ");");
                    }
                }
                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                string[] Arr = new string[2];
                clsTcMaintainance objTcMainainance = new clsTcMaintainance();

                if (ValidateForm() == true)
                {
                    objTcMainainance.sMaintainanceId = txtMaintanceID.Text;                 
                    objTcMainainance.sTcCode = txtTCCode.Text;
                    objTcMainainance.sDTCCode = txtDtcCode.Text;
                    objTcMainainance.sTmDate = txtInspDate.Text;
                    objTcMainainance.sOilLeakage = Convert.ToString(cmboilLevel.SelectedValue);                   
                    objTcMainainance.sDescription = txtTmDescription.Text;
                    objTcMainainance.sRadiator = txtRadiator.Text;
                    objTcMainainance.sArrestor = Convert.ToString(cmbArrester.SelectedValue);     
                    objTcMainainance.sMaintainBy = txtInspectedBy.Text;
                    objTcMainainance.sCrBy = objSession.UserId;

                    Arr = objTcMainainance.SaveUpdateTcMaintainance(objTcMainainance);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " Maintainance ");
                    }

                    if (Arr[1].ToString() == "0")
                    {
                        ShowMsgBox(Arr[0]);
                        txtMaintanceID.Text = objTcMainainance.sMaintainanceId;
                        cmdSave.Text = "Update";                       
                        return;
                    }
                    if (Arr[1].ToString() == "1")
                    {
                        ShowMsgBox(Arr[0]);
                        return;
                    }
                    if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0]);
                        return;
                    }
                }
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }


        bool ValidateForm()
        {
            bool bValidate = false;
            try
            { 
                if (txtDtcCode.Text.Trim().Length == 0)
                {
                    txtDtcCode.Focus();
                    ShowMsgBox("Enter Transformer Transformer Centre code");
                    return false;
                }
                if (txtTCCode.Text.Trim().Length == 0)
                {
                    txtTCCode.Focus();
                    ShowMsgBox("Enter Transformer Code");
                    return false;
                }

                if (cmboilLevel.SelectedIndex == 0)
                {
                    cmboilLevel.Focus();
                    ShowMsgBox("Select Oil Level");
                    return false;
                }

                if (cmbArrester.SelectedIndex == 0)
                {
                    cmbArrester.Focus();
                    ShowMsgBox("Select Value for Arrestor ");
                    return false;
                }
                if (txtRadiator.Text.Trim().Length == 0)
                {
                    txtRadiator.Focus();
                    ShowMsgBox("Enter Value for Radiator ");
                    return false;
                }

                if (txtInspectedBy.Text.Trim().Length == 0)
                {
                    txtInspectedBy.Focus();
                    ShowMsgBox("Enter the Inspector Name ");
                    return false;
                }

                if (txtInspDate.Text.Trim().Length == 0)
                {
                    txtInspDate.Focus();
                    ShowMsgBox("Select Inspected date ");
                    return false;
                }
                if (txtTmDescription.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Enter Description");
                    txtTmDescription.Focus();
                    return false;
                }
                if (txtLastServiceDate.Text.Trim() != "")
                {
                    string sResult = Genaral.DateComparision(txtInspDate.Text, txtLastServiceDate.Text, false, false);
                    if (sResult == "2")
                    {
                        ShowMsgBox("Inspected Date should be Greater than Last Service Date");
                        return bValidate;
                    }
                }

                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return bValidate;
            }
        }


        public void GetTcMaintainanceDeatils(string strId)
        {
            try
            {
                clsTcMaintainance objTcMaintainance = new clsTcMaintainance();
                objTcMaintainance.sMaintainanceId = strId;
                objTcMaintainance.GetMaintainaceDetails(objTcMaintainance);
                //txtTcID.Text = objTcMaintainance.sTcCode;
                txtTCCode.Text = objTcMaintainance.sTcCode;
                txtDtcCode.Text = objTcMaintainance.sDTCCode;
                txtInspDate.Text = objTcMaintainance.sTmDate;
               // cmboilLevel.SelectedValue = objTcMaintainance.sTmOilLevel;               
                txtTmDescription.Text = objTcMaintainance.sDescription;             
                txtRadiator.Text = objTcMaintainance.sRadiator;
                cmbArrester.SelectedValue = objTcMaintainance.sArrestor;
                //txtInspectedBy.Text = objTcMaintainance.sInspectedby;
                
                //To get TC Details
                GetTCDetials(txtDtcCode.Text);
                cmdSave.Text = "Update";
                if (Convert.ToDateTime(objTcMaintainance.sCrOn).ToString("dd/MM/yyyy") != System.DateTime.Today.ToString("dd/MM/yyyy"))
                {
                    cmdSave.Enabled = false;
                }
                              
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        public void GetTCDetials(string strDTCCode)
        {
            try
            {
                clsTcMaintainance objmaintainance = new clsTcMaintainance();
                objmaintainance.sDTCCode = strDTCCode;
                string sResult = objmaintainance.GetTCDetails(objmaintainance);
                if (sResult != "")
                {
                    //txtTCSlno.Text = sResult.Split('~').GetValue(0).ToString();
                    if (txtTCCode.Text.Trim() == "")
                    {
                        txtTCCode.Text = sResult.Split('~').GetValue(1).ToString();
                        txtLastServiceDate.Text = sResult.Split('~').GetValue(2).ToString();
                    }
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
                String sShowMsg = string.Empty;
                sShowMsg = "<script language=javascript> alert ('" + sMsg + "')</script>";
                this.Page.RegisterStartupScript("Msg", sShowMsg);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                clsTcMaintainance objTcMaintainance = new clsTcMaintainance();
                objTcMaintainance.sDTCCode = txtDtcCode.Text;
                string sResult = objTcMaintainance.GetTCDetails(objTcMaintainance);
                if (sResult != "")
                {
                    //txtTCSlno.Text = sResult.Split('~').GetValue(0).ToString();
                    txtTCCode.Text = sResult.Split('~').GetValue(1).ToString();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
           

        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            txtMaintanceID.Text = string.Empty;
            txtDtcCode.Text = string.Empty;
            cmbArrester.SelectedIndex = 0;
            txtInspectedBy.Text = string.Empty;
            txtRadiator.Text = string.Empty;          
            txtInspDate.Text = string.Empty;
            txtTmDescription.Text = string.Empty;
            cmboilLevel.SelectedIndex = 0;
            cmdSave.Text = "Save";
           
            txtTCCode.Text = string.Empty;
            
            txtTmId.Text = string.Empty;
           
        }

        protected void cmdclose_Click(object sender, EventArgs e)
        {
            if (txtform.Text == "Preventive")
            {
                Response.Redirect("PrevMaintanance.aspx",false);
            }
            else
            {
                Response.Redirect("TcMaintainanceView.aspx",false);
            }
        }
    }
}