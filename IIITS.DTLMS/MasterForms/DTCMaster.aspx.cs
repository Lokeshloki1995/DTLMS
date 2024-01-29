using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;

namespace IIITS.DTLMS.MasterForms
{
    public partial class DTCMaster : System.Web.UI.Page
    {
        string strFormCode = "DTCMaster";
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
                txtConnectionDate.Attributes.Add("readonly", "readonly");
                txtInspectionDate.Attributes.Add("readonly", "readonly");
                txtServiceDate.Attributes.Add("readonly", "readonly");
                txtCommisionDate.Attributes.Add("readonly", "readonly");
                txtFeederChngDate.Attributes.Add("readonly", "readonly");

                CalendarExtender1.EndDate = System.DateTime.Now;
                txtCommisionDate_CalendarExtender1.EndDate = System.DateTime.Now;
                txtServiceDate_CalendarExtender1.EndDate = System.DateTime.Now;
                txtInspectionDate_CalendarExtender1.EndDate = System.DateTime.Now;
                txtConnectionDate_CalendarExtender1.EndDate = System.DateTime.Now;

                if (!IsPostBack)
                    {

                        if (Request.QueryString["QryDtcId"] != null && Request.QueryString["QryDtcId"].ToString() != "")
                        {
                            txtDTCId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryDtcId"]));
                        }

                        if (txtDTCId.Text != "")
                        {
                            LoadDtcDetails(txtDTCId.Text);
                        }

                        string strQry = string.Empty;
                        strQry = "Title=Search and Select Location Details&";
                        strQry += "Query=select \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\"  where  {0} like %{1}% order by \"OM_NAME\"&";
                        strQry += "DBColName=\"OM_NAME\"~\"OM_CODE\"&";
                        strQry += "ColDisplayName=\"OM_NAME\"~\"OM_CODE\"&";

                        btnOmSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtOMSection.ClientID + "&btn=" + btnOmSearch.ClientID + "',520,520," + txtOMSection.ClientID + ")");

                        strQry = "Title=Search and Select Tc Details&";
                        strQry += "Query=SELECT \"TC_SLNO\",\"TC_CODE\",\"TM_NAME\",TO_CHAR(\"TC_CAPACITY\") TC_CAPACITY  FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\"  WHERE ";
                        strQry += " \"TC_LOCATION_ID\" LIKE '" + objSession.sStoreID + "' AND ";
                        strQry += " \"TC_MAKE_ID\"= \"TM_ID\" and \"TC_STATUS\" in (1,2) AND \"TC_CURRENT_LOCATION\"=1 and {0} like %{1}% order by \"TC_CODE\"&";
                        strQry += "DBColName=\"TC_SLNO\"~\"TC_CODE\"~\"TM_NAME\"&";
                        strQry += "ColDisplayName=\"TC_SLNO\"~\"TC_CODE\"~MAKE_NAME&";

                        strQry = strQry.Replace("'", @"\'");

                        cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + hdfTcCode.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + hdfTcCode.ClientID + ")");

                        txtInspectionDate.Attributes.Add("onblur", "return ValidateDate(" + txtInspectionDate.ClientID + ");");
                        txtCommisionDate.Attributes.Add("onblur", "return ValidateDate(" + txtCommisionDate.ClientID + ");");
                        txtConnectionDate.Attributes.Add("onblur", "return ValidateDate(" + txtConnectionDate.ClientID + ");");
                        txtFeederChngDate.Attributes.Add("onblur", "return ValidateDate(" + txtFeederChngDate.ClientID + ");");
                        txtServiceDate.Attributes.Add("onblur", "return ValidateDate(" + txtServiceDate.ClientID + ");");                    
                    }
            }
           catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
                  
            
            
            }
        public void LoadDtcDetails(string strId)
        {
            try
            {
                clsDtcMaster objDtcMaster = new clsDtcMaster();
                objDtcMaster.lDtcId = Convert.ToString(strId);

                objDtcMaster.GetDtcDetails(objDtcMaster);

                txtDTCId.Text = Convert.ToString(objDtcMaster.lDtcId);
                txtDTCName.Text = objDtcMaster.sDtcName;
                txtCommisionDate.Text = objDtcMaster.sCommisionDate;
                txtConnectedHP.Text = objDtcMaster.iConnectedHP;
                txtConnectedKW.Text = objDtcMaster.iConnectedKW;
                txtConnectionDate.Text = objDtcMaster.sConnectionDate;
                txtDTCCode.Text =  Convert.ToString(objDtcMaster.sDtcCode);
                txtInspectionDate.Text = objDtcMaster.sInspectionDate;
                txtFeederChngDate.Text = objDtcMaster.sFeederChangeDate;
                txtInternalCode.Text = objDtcMaster.sInternalCode;
                txtKWHReading.Text = objDtcMaster.iKWHReading;
                txtOMSection.Text = objDtcMaster.sOMSectionName;
                txtServiceDate.Text = objDtcMaster.sServiceDate;   
                cmbPlatformType.SelectedValue = objDtcMaster.sPlatformType;
                txtTcSlNo.Text = objDtcMaster.sTcSlno;
                txtCapacity.Text = objDtcMaster.sTCCapacity;
                txtTCMake.Text = objDtcMaster.sTCMakeName;
                txtTCCode.Text = objDtcMaster.sTcCode;
                txtOldTCCode.Text = objDtcMaster.sTcCode;

                ddlBreakertype.SelectedValue = objDtcMaster.sBreakertype ;
                ddlArresters.SelectedValue = objDtcMaster.sArresters;
                ddldtcmeters.SelectedValue = objDtcMaster.sDTCMeters;
                ddlgrounding.SelectedValue = objDtcMaster.sGrounding;
                ddlhtprotection.SelectedValue = objDtcMaster.sHTProtect;
                ddlLTProtection.SelectedValue = objDtcMaster.sLTProtect;
                txtltLine.Text = objDtcMaster.sLtlinelength;
                txthtLine.Text = objDtcMaster.sHtlinelength;

                cmdSave.Text = "Update";
                txtDTCCode.Enabled = false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
       
        protected void cmdSave_Click(object sender, EventArgs e)
        {
            clsDtcMaster objDtcMaster = new clsDtcMaster();
            try
            {
                if (ValidateForm() == true)
                {
                    string[] Arr = new string[3];
                    objDtcMaster.lDtcId = Convert.ToString(txtDTCId.Text);
                    objDtcMaster.sDtcName = txtDTCName.Text;                   
                    objDtcMaster.iConnectedHP = Convert.ToString(txtConnectedHP.Text);
                    objDtcMaster.iConnectedKW = Convert.ToString(txtConnectedKW.Text);
                    objDtcMaster.sInternalCode = txtInternalCode.Text;
                    objDtcMaster.sFeederChangeDate = txtFeederChngDate.Text;
                    objDtcMaster.sInspectionDate = txtInspectionDate.Text;
                    objDtcMaster.sPlatformType = cmbPlatformType.Text;                   
                    objDtcMaster.sServiceDate = txtServiceDate.Text;
                    objDtcMaster.sOMSectionName = txtOMSection.Text;
                    objDtcMaster.sDtcCode = txtDTCCode.Text;
                    objDtcMaster.iKWHReading = txtKWHReading.Text;
                    objDtcMaster.sCommisionDate = txtCommisionDate.Text;
                    objDtcMaster.sConnectionDate = txtConnectionDate.Text;
                    objDtcMaster.sTcCode  = txtTCCode.Text;
                    objDtcMaster.sCrBy = objSession.UserId;
                    objDtcMaster.sOldTcCode = txtOldTCCode.Text;
                    objDtcMaster.sTcSlno = txtTcSlNo.Text;
                    objDtcMaster.sHtlinelength = txthtLine.Text;                  
                    objDtcMaster.sLtlinelength = txtltLine.Text;

                   
                    objDtcMaster.sArresters = ddlArresters.SelectedValue;
                    objDtcMaster.sGrounding = ddlgrounding.SelectedValue;      
                    objDtcMaster.sLTProtect = ddlLTProtection.SelectedValue;
                    objDtcMaster.sHTProtect = ddlhtprotection.SelectedValue;
                    objDtcMaster.sDTCMeters = ddldtcmeters.SelectedValue;
                    objDtcMaster.sBreakertype = ddlBreakertype.SelectedValue;
                    
                    Arr = objDtcMaster.SaveUpdateDtcDetails(objDtcMaster);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " Dtc Master");
                    }

                    if (Arr[1].ToString() == "0")
                    {
                        ShowMsgBox(Arr[0]);
                        txtDTCId.Text = objDtcMaster.lDtcId;
                        cmdSave.Text = "Update";
                        return;
                    }

                    if (Arr[1].ToString() == "1")
                    {
                        ShowMsgBox(Arr[0]);
                        cmdSave.Text = "Update";
                        return;
                    }

                    if (Arr[1].ToString() == "4")
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
                if (txtDTCCode.Text.Trim().Length < 6)
                {
                    txtDTCCode.Focus();
                    ShowMsgBox("Enter 6 digit Transformer Centre Code");
                    return bValidate;
                }
                if (txtDTCCode.Text.Trim().Length == 0)
                {
                    txtDTCCode.Focus();
                    ShowMsgBox("Enter Transformer Centre Code");
                    return bValidate;
                }
                if (txtDTCName.Text.Trim().Length == 0)
                {
                    txtDTCName.Focus();
                    ShowMsgBox("Enter Transformer Centre Name");
                    return bValidate;
                }
                if (txtOMSection.Text.Trim().Length == 0)
                {
                    txtOMSection.Focus();
                    ShowMsgBox("Enter O & M Section");
                    return bValidate;
                }
                if (txtInternalCode.Text.Trim().Length == 0)
                {
                    txtKWHReading.Focus();
                    ShowMsgBox("Enter Internal Code");
                    return bValidate;
                }

                if (txtConnectedKW.Text.Trim().Length == 0)
                {
                    txtConnectedKW.Focus();
                    ShowMsgBox("Enter Connected KW");
                    return bValidate;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtConnectedKW.Text, "^(\\d{1,4})?(\\.\\d{1,2})?$"))
                {
                    ShowMsgBox("Enter valid Connected KW");
                    return false;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(txtConnectedKW.Text, "[-+]?[0-9]{0,3}\\.?[0-9]{1,2}"))
                {
                    ShowMsgBox("Enter valid Connected KW");
                    return false;
                }
                if (txtConnectedHP.Text.Trim().Length == 0)
                {
                    txtConnectedHP.Focus();
                    ShowMsgBox("Enter Connected HP");
                    return bValidate;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtConnectedHP.Text, "^(\\d{1,4})?(\\.\\d{1,2})?$"))
                {
                    ShowMsgBox("Enter valid Connected HP");
                    return false;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(txtConnectedHP.Text, "[-+]?[0-9]{0,3}\\.?[0-9]{1,2}"))
                {
                    ShowMsgBox("Enter valid Connected HP");
                    return false;
                }

                if (txtKWHReading.Text.Trim() != "")
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtKWHReading.Text, "^(\\d{1,6})?(\\.\\d{1,2})?$"))
                    {
                        ShowMsgBox("Enter Valid KWH Reading");
                        return false;
                    }

                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtKWHReading.Text, "[-+]?[0-9]{0,5}\\.?[0-9]{0,2}"))
                    {
                        ShowMsgBox("Enter Valid KWH Reading");
                        return false;
                    }
                }

                if (cmbPlatformType.SelectedValue == "0")
                {
                    cmbPlatformType.Focus();

                    ShowMsgBox("Select Platform Type");
                    return bValidate;
                }
                if (txtTcSlNo.Text.Trim().Length == 0)
                {
                    txtTcSlNo.Focus();
                    ShowMsgBox("Enter Transformer Serial No.");
                    return bValidate;
                }
                if (txtConnectionDate.Text.Trim().Length == 0)
                {
                    txtConnectionDate.Focus();

                    ShowMsgBox("Enter Connection Date");
                    return bValidate;
                }
                if (txtInspectionDate.Text.Trim().Length == 0)
                {
                    txtInspectionDate.Focus();

                    ShowMsgBox("Enter Inspection Date");
                    return bValidate;
                }
                if (txtServiceDate.Text.Trim().Length == 0)
                {
                    txtServiceDate.Focus();
                    ShowMsgBox("Enter Service Date");
                    return bValidate;
                }
                if (txtCommisionDate.Text.Trim().Length == 0)
                {
                    txtCommisionDate.Focus();
                    ShowMsgBox("Enter Commision Date");
                    return bValidate;
                }
                if (txtFeederChngDate.Text.Trim().Length == 0)
                {
                    txtFeederChngDate.Focus();
                    ShowMsgBox("Enter Feeder Changing Date");
                    return bValidate;
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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtTcSlNo.Text = string.Empty;
                txtServiceDate.Text = string.Empty;
                txtOMSection.Text = string.Empty;
                txtKWHReading.Text = string.Empty;
                txtInternalCode.Text = string.Empty;
                txtInspectionDate.Text = string.Empty;
                txtFeederChngDate.Text = string.Empty;
                txtDTCName.Text = string.Empty;
                txtDTCId.Text = string.Empty;
                txtDTCCode.Text = string.Empty;
                txtConnectionDate.Text = string.Empty;
                txtConnectedKW.Text = string.Empty;
                txtConnectedHP.Text = string.Empty;
                txtCommisionDate.Text = string.Empty;
                cmbPlatformType.SelectedIndex = 0;
                cmdSave.Text = "Save";
                ddlArresters.SelectedIndex = 0;
                ddlBreakertype.SelectedIndex = 0;
                ddldtcmeters.SelectedIndex = 0;
                ddlgrounding.SelectedIndex = 0;
                ddlhtprotection.SelectedIndex = 0;
                ddlLTProtection.SelectedIndex = 0;
                txthtLine.Text = string.Empty;
                txtltLine.Text = string.Empty;
                txtTCMake.Text = string.Empty;
                txtCapacity.Text = string.Empty;
                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void cmdClose_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("DTCView.aspx", false);
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
                clsDtcMaster objDTcMaster = new clsDtcMaster();
                objDTcMaster.sTcCode  = hdfTcCode.Value;

                objDTcMaster.GetTCDetails(objDTcMaster);

                txtTCMake.Text = objDTcMaster.sTCMakeName;
                txtCapacity.Text = objDTcMaster.sTCCapacity;
                txtTCCode.Text = objDTcMaster.sTcCode;
                txtTcSlNo.Text = objDTcMaster.sTcSlno;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


    }
}

