using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Configuration;

namespace IIITS.DTLMS.DTCFailure
{
    public partial class NewDTCCommission : System.Web.UI.Page
    {
        string strFormCode = "NewDTCCommission";
        clsSession objSession;
        string soffcode;
        clsDTCCommision objDtcMaster = new clsDTCCommision();
        int Division;
        int SubDivision;
        int Section;

        string sDTCXmlData = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
            {
                Response.Redirect("~/Login.aspx", false);
            }
            else
            {
                Division = Convert.ToInt32(ConfigurationSettings.AppSettings["Division_code"]);
                SubDivision = Convert.ToInt32(ConfigurationSettings.AppSettings["SubDiv_code"]);
                Section = Convert.ToInt32(ConfigurationSettings.AppSettings["Section_code"]);

                Form.DefaultButton = cmdSave.UniqueID;
                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;
                txtCommisionDate_CalendarExtender1.EndDate = System.DateTime.Now.AddDays(0);
                txtEleDate.Attributes.Add("readonly", "readonly");
                txtCommisionDate.Attributes.Add("readonly", "readonly");
                txtServiceDate.Attributes.Add("readonly", "readonly");
                txtFeederChngDate.Attributes.Add("readonly", "readonly");
                txtManufactureDate.Attributes.Add("readonly", "readonly");

                CalendarExtender1.EndDate = System.DateTime.Now;
                txtServiceDate_CalendarExtender1.EndDate = System.DateTime.Now;
                txtCommisionDate_CalendarExtender1.EndDate = System.DateTime.Now;
                CalendarExtender3.EndDate = System.DateTime.Now;
                CalendarExtender2.EndDate = System.DateTime.Now;

                if (!IsPostBack)
                {
                    if (objSession.OfficeCode.Length > 3) // Enabled to load feeder dropdown
                    {
                        soffcode = objSession.OfficeCode.Substring(0, SubDivision);
                    }
                    else
                        soffcode = objSession.OfficeCode;
                    Genaral.Load_Combo("SELECT \"FD_FEEDER_CODE\" ||'-'|| \"FD_FEEDER_NAME\",\"FD_FEEDER_CODE\" ||'-'|| \"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" WHERE \"FD_FEEDER_ID\"=\"FDO_FEEDER_ID\" AND CAST(\"FDO_OFFICE_CODE\" AS TEXT) LIKE'" + soffcode + "%'", "--Select--", cmbFeeder);
                    //Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'PT' ORDER BY \"MD_ORDER_BY\" ", "--Select--", cmbprojecttype);
                    Genaral.Load_Combo("SELECT \"SCHM_ID\", \"SCHM_NAME\" FROM \"TBLDTCSCHEME\" ORDER BY \"SCHM_NAME\" ", "--Select--", cmbprojecttype);
                    Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='SR' ORDER BY \"MD_ORDER_BY\" ", "-Select-", cmbRating);
                    Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='C' ORDER BY \"MD_ORDER_BY\" ", "--Select--", cmbCapacity);
                    Genaral.Load_Combo("SELECT DISTINCT \"TM_ID\",\"TM_NAME\" FROM \"TBLTRANSMAKES\" ORDER BY \"TM_NAME\"", "--Select--", cmbMake);

                    if (Request.QueryString["QryDtcId"] != null && Convert.ToString(Request.QueryString["QryDtcId"]) != "")
                    {
                        txtDTCId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryDtcId"]));
                    }

                    if (Request.QueryString["SWoTTKSTatus"] != null && Convert.ToString(Request.QueryString["SWoTTKSTatus"]) != "")
                    {
                        hdfWOTTKstatus.Value = Request.QueryString["SWoTTKSTatus"];
                        if (Request.QueryString["WOSlno"] != null && Convert.ToString(Request.QueryString["WOSlno"]) != "")
                        {
                            txtWOslno.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["WOSlno"]));
                            GetTCDetails();

                        }
                    }
                    else
                    {
                        if (Request.QueryString["WOSlno"] != null && Convert.ToString(Request.QueryString["WOSlno"]) != "")
                        {
                            txtWOslno.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["WOSlno"]));
                            txtTCCode.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TCCode"]));
                            GetTCDetails();

                        }
                    }

                    if (txtDTCId.Text != "")
                    {
                        LoadDtcDetails(txtDTCId.Text);

                        objDtcMaster.lDtcId = Convert.ToString(txtDTCId.Text);
                        objDtcMaster.GetDTCDetails(objDtcMaster);
                        sDTCXmlData = objDtcMaster.SaveXmlData(objDtcMaster);
                        ViewState["sDTCXmlData"] = sDTCXmlData;
                    }

                    string strQry = string.Empty;
                    FilterLocation();

                    strQry = "Title=Search and Select TC Details&";
                    strQry += "Query=SELECT \"TC_CODE\",\"TC_SLNO\",\"TM_NAME\",CAST(\"TC_CAPACITY\" as TEXT) \"TC_CAPACITY\"  FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\"  WHERE \"TC_LOCATION_ID\" LIKE '" + objSession.sStoreID + "' AND";
                    strQry += " \"TC_MAKE_ID\"= \"TM_ID\" and \"TC_STATUS\" in (1,2) AND \"TC_CURRENT_LOCATION\"=1 and {0} like %{1}% order by \"TC_CODE\"&";
                    strQry += "DBColName=\"TC_CODE\"~\"TC_SLNO\"~\"TM_NAME\"&";
                    strQry += "ColDisplayName=DTr Code~DTr SlNo~Make Name&";
                    strQry = strQry.Replace("'", @"\'");

                    cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + hdfTcCode.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + hdfTcCode.ClientID + ")");

                    txtCommisionDate.Attributes.Add("onblur", "return ValidateDate(" + txtCommisionDate.ClientID + ");");
                    txtFeederChngDate.Attributes.Add("onblur", "return ValidateDate(" + txtFeederChngDate.ClientID + ");");
                    txtServiceDate.Attributes.Add("onblur", "return ValidateDate(" + txtServiceDate.ClientID + ");");
                    //WorkFlow / Approval
                    WorkFlowConfig();
                }
            }            
        }

        public void LoadDtcDetails(string strId)
        {
            try
            {
                clsDTCCommision objDtcMaster = new clsDTCCommision();
                objDtcMaster.lDtcId = Convert.ToString(strId);

                objDtcMaster.GetDTCDetails(objDtcMaster);
                txtWOslno.Text = objDtcMaster.sWOslno;
                txtDTCId.Text = Convert.ToString(objDtcMaster.lDtcId);
                txtDTCName.Text = objDtcMaster.sDtcName;
                txtCommisionDate.Text = objDtcMaster.sCommisionDate;
                txtConnectedHP.Text = objDtcMaster.iConnectedHP;
                txtConnectedKW.Text = objDtcMaster.iConnectedKW;
                txtDTCCode.Text = Convert.ToString(objDtcMaster.sDtcCode);
                txtFeederChngDate.Text = objDtcMaster.sFeederChangeDate;
                txtInternalCode.Text = objDtcMaster.sInternalCode;
                txtKWHReading.Text = objDtcMaster.iKWHReading;
                txtOMSection.Text = objDtcMaster.sOMSectionName;
                txtServiceDate.Text = objDtcMaster.sServiceDate;
                cmbprojecttype.SelectedValue = objDtcMaster.sProjecttype;
                cmbFeeder.SelectedValue = objDtcMaster.sFeedercode;
                cmbCapacity.SelectedItem.Text = objDtcMaster.sTCCapacity;
                cmbMake.SelectedItem.Text = objDtcMaster.sTCMakeName;
               
                txtTCCode.Text = objDtcMaster.sTcCode;
                txtOldTCCode.Text = objDtcMaster.sTcCode;
                txtTimsCode.Text = objDtcMaster.sTims_Code;
                txtMP.Text = objDtcMaster.sMPConst;
                txtMla.Text = objDtcMaster.sMLAConst;
                txtEleDate.Text = objDtcMaster.sEleInsDate;
                txtEleRateNo.Text = objDtcMaster.sEleInsRateNo;
                cmdSave.Text = "Update & Continue";
                cmdNext.Visible = true;
                txtDTCCode.Enabled = false;
                cmbFeeder.Enabled = false;

                clsDTCCommision objDTC = new clsDTCCommision();

                objDTC.sDtcCode = txtDTCCode.Text;
                objDTC.sTcCode = txtTCCode.Text;
                objDTC.GetImagePath(objDTC);
                hdfDTCImagePath.Value = objDTC.sDTCImagePath;
                hdfDTRImagePath.Value = objDTC.sDTrImagePath;

                ShowUploadedImages();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmdSave_Click(object sender, EventArgs e)
        {
            clsDTCCommision objDtcCommision = new clsDTCCommision();
            try
            {
                //Check AccessRights
                bool bAccResult;
                if (cmdSave.Text == "Update")
                {
                    bAccResult = CheckAccessRights("3");
                }
                else
                {
                    bAccResult = CheckAccessRights("2");
                }

                if (bAccResult == false)
                {
                    return;
                }

                string sResult1 = Genaral.DateComparisionTransaction(txtCommisionDate.Text, txtManufactureDate.Text, false, false);
                if (sResult1 == "1")
                {
                    ShowMsgBox("Manufacture Date should be Greater than Commission Date");
                    return;
                }


                if (hdfWOTTKstatus.Value == "1")
                {
                    string[] Arr = new string[3];

                    if (txtTCCode.Text.Length == 0)
                    {
                        ShowMsgBox("please enter Tc Code");
                        txtTCCode.Focus();
                        return;
                    }
                    

                    if (txtTcSlno.Text.Length == 0)
                    {
                        ShowMsgBox("please enter Tc serial number");
                        txtTcSlno.Focus();
                        return;
                    }

                    if (cmbMake.SelectedValue == "0")
                    {
                        ShowMsgBox("please select make name");
                        cmbMake.Focus();
                        return;
                    }
                    if (cmbRating.SelectedValue == "0")
                    {
                        ShowMsgBox("please select Tc Rating");
                        cmbRating.Focus();
                        return;
                    }
                    if (txtPOnum.Text.Length == 0)
                    {
                        ShowMsgBox("please enter Purchase Order / DI Number");
                        txtPOnum.Focus();
                        return;
                    }

                    if (cmbCapacity.SelectedValue == "0")
                    {
                        ShowMsgBox("please select Capacity");
                        cmbCapacity.Focus();
                        return;
                    }
                    objDtcCommision.sTcSlno = txtTcSlno.Text.ToUpper();
                    objDtcCommision.sTcCode = txtTCCode.Text;
                    Arr = objDtcCommision.GetTcAndSrlNumDetails(objDtcCommision);
                    if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0]);
                        return;
                    }
                    objDtcCommision.sTCMakeId = cmbMake.SelectedValue;
                    objDtcCommision.sTcRating = cmbRating.SelectedValue;
                    objDtcCommision.sTcStoreId = cmbStore.SelectedValue;

                    objDtcCommision.sTCCapacity = cmbCapacity.SelectedItem.Text;
                    if (txtServiceDate.Text != null && txtServiceDate.Text != "")
                    {
                        objDtcCommision.sTcLastserviceDate = txtServiceDate.Text;
                    }
                    else
                    {
                        objDtcCommision.sTcLastserviceDate = "0";
                    }
                    if (txtManufactureDate.Text != null && txtManufactureDate.Text != "")
                    {
                        objDtcCommision.sManufactureDate = txtManufactureDate.Text;
                    }
                    else
                    {
                        objDtcCommision.sManufactureDate = "0";
                    }
                    objDtcCommision.sCrBy = objSession.UserId;
                    objDtcCommision.sOfficeCode = objSession.OfficeCode;
                    objDtcCommision.sDTCNewcommTTk_flow = hdfWOTTKstatus.Value;
                    objDtcCommision.sPoNo = txtPOnum.Text;

                    Arr = objDtcCommision.SaveTCDetails(objDtcCommision);
                    if (Arr[0].ToString() == "msg")
                    {
                        ShowMsgBox("something went wrong with tc master");
                        return;
                    }
                }

                if (ValidateForm() == true)
                {
                    string[] Arr = new string[3];
                    objDtcCommision.lDtcId = Convert.ToString(txtDTCId.Text);
                    objDtcCommision.sDtcName = txtDTCName.Text;
                    if (txtConnectedHP.Text == "")
                    {
                        objDtcCommision.iConnectedHP = "0";
                    }
                    else
                    {
                        objDtcCommision.iConnectedHP = Convert.ToString(txtConnectedHP.Text);
                    }
                    if (txtConnectedKW.Text == "")
                    {
                        objDtcCommision.iConnectedKW = "0";
                    }
                    else
                    {
                        objDtcCommision.iConnectedKW = Convert.ToString(txtConnectedKW.Text);
                    }

                    objDtcCommision.sInternalCode = txtInternalCode.Text;
                    objDtcCommision.sFeederChangeDate = txtFeederChngDate.Text;
                    objDtcCommision.sServiceDate = txtServiceDate.Text;
                    objDtcCommision.sOMSectionName = txtOMSection.Text;
                    objDtcCommision.sDtcCode = txtDTCCode.Text;
                    
                    if (txtKWHReading.Text == "")
                    {
                        objDtcCommision.iKWHReading = "0";
                    }
                    else
                    {
                        objDtcCommision.iKWHReading = txtKWHReading.Text;
                    }
                    if (txtAvgLoad.Text == "")
                    {
                        objDtcCommision.sDtAvgLoad = "0";
                    }
                    else
                    {
                        objDtcCommision.sDtAvgLoad = txtAvgLoad.Text;
                    }
                    if (txtPeakLoad.Text == "")
                    {
                        objDtcCommision.sDtPeakLoad = "0";
                    }
                    else
                    {
                        objDtcCommision.sDtPeakLoad = txtPeakLoad.Text;
                    }
                    if (txtSurplusCap.Text == "")
                    {
                        objDtcCommision.sDtsurpluscap = "0";
                    }
                    else
                    {
                        objDtcCommision.sDtsurpluscap = txtSurplusCap.Text;
                    }
                    if (txtEleRateNo.Text == "")
                    {
                        objDtcCommision.sEleInsRateNo = "0";
                    }
                    else
                    {
                        objDtcCommision.sEleInsRateNo = txtEleRateNo.Text;
                    }
                    objDtcCommision.sEleInsDate = txtEleDate.Text;
                    objDtcCommision.sCommisionDate = txtCommisionDate.Text;
                    objDtcCommision.sTcCode = txtTCCode.Text;
                    objDtcCommision.sCrBy = objSession.UserId;
                    objDtcCommision.sOldTcCode = txtOldTCCode.Text;
                    objDtcCommision.sWOslno = txtWOslno.Text;
                    objDtcCommision.sOfficeCode = objSession.OfficeCode;
                    objDtcCommision.sTims_Code = txtTimsCode.Text;

                    if (objDtcCommision.sTims_Code == "")
                    {
                        objDtcCommision.sTims_Code = "0";
                    }
                    if (cmbprojecttype.SelectedIndex > 0)
                    {
                        objDtcCommision.sProjecttype = cmbprojecttype.SelectedValue;
                    }
                    else
                    {
                        objDtcCommision.sProjecttype = "0";
                    }

                    objDtcCommision.sMLAConst = txtMla.Text.ToUpper().Trim();
                    objDtcCommision.sMPConst = txtMP.Text.ToUpper().Trim();

                    //Workflow
                    WorkFlowObjects(objDtcCommision);

                    objDtcCommision.sFormName = strFormCode;
                    objDtcCommision.sDescription = "DATA MODIFIED BY " + Session["FullName"];
                    objDtcCommision.sXmlData = Convert.ToString(ViewState["sDTCXmlData"]);

                    Arr = objDtcCommision.SaveUpdateDtcDetails(objDtcCommision);

                    if (Arr[1].ToString() == "0")
                    {

                        txtDTCId.Text = objDtcCommision.lDtcId;
                        cmdSave.Text = "Update";
                        string strDtcId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtDTCId.Text));

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID, "alert('Saved Successfully'); location.href='../MasterForms/DTCDetails.aspx?QryDtcId=" + strDtcId + "';", true);
                    }

                    if (Arr[1].ToString() == "1")
                    {

                        cmdSave.Text = "Update";
                        //Save data in TBLWFODATA And TBLWORKFLOWOBJECTS 
                        objDtcMaster.lDtcId = objDtcCommision.lDtcId;
                        txtDTCId.Text = objDtcCommision.lDtcId;
                        objDtcCommision.GetDTCDetails(objDtcMaster);
                        objDtcCommision.sXmlData = objDtcCommision.SaveXmlData(objDtcCommision);
                        bool sResult;
                        sResult = objDtcCommision.SaveWorkFlowData(objDtcCommision);

                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " NewDtc Commission ");
                        }

                        objDtcCommision.GetDTCDetails(objDtcMaster);
                        objDtcCommision.sXmlData = objDtcCommision.SaveXmlData(objDtcCommision);
                        //To get last data
                        ViewState["sDTCXmlData"] = sDTCXmlData;

                        string strDtcId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtDTCId.Text));
                        string sReference = HttpUtility.UrlEncode(Genaral.UrlEncrypt("Update"));
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID, "alert('Updated Successfully'); location.href='../MasterForms/DTCDetails.aspx?QryDtcId=" + strDtcId + "&Ref=" + sReference + "';", true);
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

                if (txtConnectedKW.Text.Trim() != "")
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtConnectedKW.Text, "^(\\d{1,4})?(\\.\\d{1,3})?$"))
                    {
                        ShowMsgBox("Enter valid Connected KW (eg:1234.11)");
                        return false;
                    }

                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtConnectedKW.Text, "[-+]?[0-9]{0,3}\\.?[0-9]{1,3}"))
                    {
                        ShowMsgBox("Enter valid Connected KW (eg:1234.11)");
                        return false;
                    }
                }

                if (txtConnectedHP.Text.Trim() != "")
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtConnectedHP.Text, "^(\\d{1,4})?(\\.\\d{1,3})?$"))
                    {
                        ShowMsgBox("Enter valid Connected HP (eg:1234.11)");
                        return false;
                    }

                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtConnectedHP.Text, "[-+]?[0-9]{0,3}\\.?[0-9]{1,3}"))
                    {
                        ShowMsgBox("Enter valid Connected HP (eg:1234.11)");
                        return false;
                    }
                }

                if (txtKWHReading.Text.Trim() != "")
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtKWHReading.Text, "^(\\d{1,6})?(\\.\\d{1,3})?$"))
                    {
                        ShowMsgBox("Enter Valid KWH Reading (eg:1234.11)");
                        return false;
                    }

                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtKWHReading.Text, "[-+]?[0-9]{0,5}\\.?[0-9]{0,3}"))
                    {
                        ShowMsgBox("Enter Valid KWH Reading (eg:1234.11)");
                        return false;
                    }
                }
                if (txtTCCode.Text.Trim().Length == 0)
                {
                    txtTCCode.Focus();
                    ShowMsgBox("Enter Valid Transformer Code");
                    return bValidate;
                }

                if (cmbprojecttype.SelectedIndex > 0)
                {
                    if (cmbprojecttype.SelectedValue == "9" || cmbprojecttype.SelectedValue == "10")
                    {
                        if (txtCommisionDate.Text == "")
                        {
                            txtCommisionDate.Focus();
                            ShowMsgBox("Enter Commission Date");
                            return bValidate;
                        }
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
                txtTCCode.Text = string.Empty;
                hdfTcCode.Value = string.Empty;
                txtServiceDate.Text = string.Empty;
                txtKWHReading.Text = string.Empty;
                txtInternalCode.Text = string.Empty;
                txtFeederChngDate.Text = string.Empty;
                txtDTCName.Text = string.Empty;
                txtDTCId.Text = string.Empty;
                txtDTCCode.Text = string.Empty;
                txtConnectedKW.Text = string.Empty;
                txtConnectedHP.Text = string.Empty;
                txtCommisionDate.Text = string.Empty;
                txtManufactureDate.Text = string.Empty;
                cmbMake.ClearSelection();
                cmbStore.ClearSelection();
                txtPOnum.Text = string.Empty;
                cmbCapacity.ClearSelection();
                cmbRating.ClearSelection();
                txtDTCCode.Enabled = true;
                txtDTCId.Text = string.Empty;
                cmdNext.Visible = false;
                FilterLocation();
                if (txtOMSection.Enabled == true)
                {
                    txtOMSection.Text = string.Empty;
                }
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

                objDTcMaster.sTcCode = hdfTcCode.Value;
                objDTcMaster.GetTCDetails(objDTcMaster);
                cmbMake.SelectedItem.Text = objDTcMaster.sTCMakeName;
                cmbCapacity.SelectedItem.Text = objDTcMaster.sTCCapacity;
                txtTCCode.Text = objDTcMaster.sTcCode;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdNext_Click(object sender, EventArgs e)
        {
            try
            {
                string strDtcId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtDTCId.Text));
                Response.Redirect("DTCDetails.aspx?QryDtcId=" + strDtcId + "", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        public void FilterLocation()
        {
            try
            {
                string strQry = string.Empty;

                if (objSession.OfficeCode.Trim().Length == 5)
                {
                    txtOMSection.Text = objSession.OfficeCode;
                    btnOmSearch.Visible = false;
                    txtOMSection.Enabled = false;
                }
                else
                {
                    strQry = "Title=Search and Select Location Details&";
                    strQry += "Query=select OM_CODE,OM_NAME FROM TBLOMSECMAST  WHERE OM_CODE LIKE '" + objSession.OfficeCode + "%' AND {0} like %{1}% order by OM_NAME&";
                    strQry += "DBColName=OM_NAME~OM_CODE&";
                    strQry += "ColDisplayName=OM_NAME~OM_CODE&";
                    strQry = strQry.Replace("'", @"\'");
                    btnOmSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtOMSection.ClientID + "&btn=" + btnOmSearch.ClientID + "',520,520," + txtOMSection.ClientID + ")");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "NewDTCCommission";
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sAccessType = "1" + "," + sAccessType;
                bool bResult = objApproval.CheckAccessRights(objApproval);
                if (bResult == false)
                {
                    if (sAccessType == "4")
                    {
                        Response.Redirect("~/UserRestrict.aspx", false);
                    }
                    else
                    {
                        ShowMsgBox("Sorry , You are not authorized to Access");
                    }
                }
                return bResult;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;

            }
        }

        #endregion

        //public void GetTCDetails()
        //{
        //    try
        //    {
        //        clsInvoice objInvoice = new clsInvoice();
        //        objInvoice.sTcCode = txtTCCode.Text;
        //        if (objInvoice.sOfficeCode == null)
        //        {
        //            objInvoice.sOfficeCode = objSession.OfficeCode;
        //        }
        //        objInvoice.GetTCDetails(objInvoice);

        //        txtTCMake.Text = objInvoice.sTcMake;
        //        txtCapacity.Text = objInvoice.sTcCapacity;

        //        cmdSearch.Visible = false;
        //        txtTCCode.Enabled = false;

        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}
        public void GetTCDetails()
        {
            try
            {
                clsInvoice objInvoice = new clsInvoice();
                objInvoice.sTcCode = txtTCCode.Text;
                if (objInvoice.sOfficeCode == null)
                {
                    objInvoice.sOfficeCode = objSession.OfficeCode;
                }
                if (hdfWOTTKstatus.Value == "1")
                {

                    cmdSearch.Visible = false;
                    txtTCCode.Enabled = true;
                    cmbMake.Enabled = true;
                    txtTcSlno.Enabled = true;
                    dvPONum.Visible = true;
                    dvStore.Visible = false;
                    Genaral.Load_Combo("select \"SM_ID\" ,\"SM_NAME\" from \"TBLSTOREMAST\",\"TBLSTOREOFFCODE\" where \"SM_ID\"=\"STO_SM_ID\" and \"STO_OFF_CODE\"=" + objSession.OfficeCode.Substring(0, 3) + "", cmbStore);
                    Genaral.Load_Combo("SELECT \"SCHM_ID\", \"SCHM_NAME\" FROM \"TBLDTCSCHEME\" ,\"TBLWORKORDER\" WHERE \"WO_SLNO\"=" + txtWOslno.Text + " AND \"WO_ACC_CODE\"=\"SCHM_ACCCODE\" ORDER BY \"SCHM_NAME\" ", cmbprojecttype);
                    Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\" ,\"TBLWORKORDER\" WHERE \"WO_SLNO\"=" + txtWOslno.Text + " AND \"MD_TYPE\"='SR'AND case when \"WO_RATING\"= '' then cast(\"MD_ID\" as int) in(1,2,3,4,5,6,7,8)  else cast(\"MD_ID\" as text)=\"WO_RATING\" end   ORDER BY \"MD_ORDER_BY\"  ", cmbRating);
                    Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\" ,\"TBLWORKORDER\" WHERE \"WO_SLNO\"=" + txtWOslno.Text + " AND \"MD_TYPE\"='C'AND \"MD_NAME\"=cast(\"WO_NEW_CAP\" as text) ORDER BY \"MD_ORDER_BY\" ", cmbCapacity);
                }
                else
                {
                    Genaral.Load_Combo("select \"SM_ID\" ,\"SM_NAME\" from \"TBLSTOREMAST\",\"TBLSTOREOFFCODE\" where \"SM_ID\"=\"STO_SM_ID\" and \"STO_OFF_CODE\"=" + objSession.OfficeCode.Substring(0, 3) + "", "--Select--", cmbStore);
                    Genaral.Load_Combo("SELECT \"SCHM_ID\", \"SCHM_NAME\" FROM \"TBLDTCSCHEME\" ,\"TBLWORKORDER\" WHERE \"WO_SLNO\"=" + txtWOslno.Text + " AND \"WO_ACC_CODE\"=\"SCHM_ACCCODE\" ORDER BY \"SCHM_NAME\" ", cmbprojecttype);

                    objInvoice.GetTCDetails(objInvoice);

                    cmbMake.SelectedItem.Text = objInvoice.sTcMake;
                    txtManufactureDate.Text = objInvoice.smanufactureDate;
                    cmbCapacity.SelectedItem.Text = objInvoice.sTcCapacity;
                    txtTcSlno.Text = objInvoice.sTcSlNo;
                    cmbRating.SelectedValue = objInvoice.sTcRating;
                    cmbStore.SelectedValue = objInvoice.sStoreId;
                    cmdSearch.Visible = false;
                    txtTCCode.Enabled = false;
                    cmbMake.Enabled = false;
                    cmbCapacity.Enabled = false;
                    txtTcSlno.Enabled = false;
                    cmbRating.Enabled = false;
                    cmbStore.Enabled = false;
                    txtManufactureDate.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void WorkFlowObjects(clsDTCCommision objDTCComm)
        {
            try
            {
                string sClientIP = string.Empty;

                string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(ip))
                {
                    string[] ipRange = ip.Split(',');
                    int le = ipRange.Length - 1;
                    sClientIP = ipRange[0];
                }
                else
                {
                    sClientIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }


                objDTCComm.sFormName = "NewDTCCommission";
                objDTCComm.sOfficeCode = objSession.OfficeCode;
                objDTCComm.sClientIP = sClientIP;
                objDTCComm.sWFOId = txtWFOId.Text;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void WorkFlowConfig()
        {
            try
            {
                if (Request.QueryString["ActionType"] != null && Convert.ToString(Request.QueryString["ActionType"]) != "")
                {
                    if (Session["WFOId"] != null && Convert.ToString(Session["WFOId"]) != "")
                    {
                        txtWFOId.Text = Convert.ToString(Session["WFOId"]);
                        Session["WFOId"] = null;
                        Session["WFDataId"] = null;
                        Session["WFOAutoId"] = null;
                    }

                    //SetControlText();
                    if (txtActiontype.Text == "V")
                    {
                        cmdSave.Text = "View";

                        //dvComments.Style.Add("display", "none");
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void lnkDTCHistory_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtDTCId.Text.Trim() != "")
                {
                    string sDTCCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtDTCCode.Text));
                    Response.Redirect("/Transaction/DTCTracker.aspx?DTCCode=" + sDTCCode, false);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void ShowUploadedImages()
        {
            try
            {
                clsCommon objComm = new clsCommon();

                //FTP Parameter
                string sFTPLink = string.Empty;
                string sFTPUserName = string.Empty;
                string sFTPPassword = string.Empty;
                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);

                // To bind the Images from Ftp Path to Image Control

                System.Data.DataTable dt = objComm.GetAppSettings();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPLINK")
                    {
                        sFTPLink = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
                    }
                    else if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPUSERNAME")
                    {
                        sFTPUserName = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
                    }
                    else if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPPASSWORD")
                    {
                        sFTPPassword = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
                    }
                }

                sFTPLink = Convert.ToString(ConfigurationSettings.AppSettings["VirtualDirectoryPath"]);
                //clsFtp objFtp = new clsFtp(sFTPLink, sFTPUserName, sFTPPassword);
                clsSFTP objFtp = new clsSFTP(SFTPPath, sFTPUserName, sFTPPassword);


                if (hdfDTCImagePath.Value != "")
                {
                    dvDTCCode.Style.Add("display", "block");
                    //imgDTCCode.ImageUrl = "ftp://" + sFTPUserName + ":" + sFTPPassword + "@" + sFTPLink.Remove(0, 6) + hdfDTCImagePath.Value;
                    imgDTCCode.ImageUrl = sFTPLink + hdfDTCImagePath.Value;
                }
                if (hdfDTRImagePath.Value != "")
                {
                    dvDTrCode.Style.Add("display", "block");
                    //imgDTrCode.ImageUrl = "ftp://" + sFTPUserName + ":" + sFTPPassword + "@" + sFTPLink.Remove(0, 6) + hdfDTRImagePath.Value;
                    imgDTrCode.ImageUrl = sFTPLink + hdfDTRImagePath.Value;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }

        protected void cmbFeeder_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string feeder = cmbFeeder.SelectedItem.Text.Substring(0, 6);
               
                clsDTCCommision objDTCCode = new clsDTCCommision();
                objDTCCode.sFeedercode = feeder;
                string sDTCCode = objDTCCode.AutoGenerateDTCCode(objDTCCode);
                if (sDTCCode.Length > 9)
                {
                    ShowMsgBox("Transformer Centre Code already reached 999 so next Transformer Centre Code not Possible to generate");
                    txtDTCCode.Text = "";
                }
                else if (sDTCCode.Length == 1)
                {
                    txtDTCCode.Text = feeder + "001";
                }
                else
                {
                    txtDTCCode.Text = sDTCCode;
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