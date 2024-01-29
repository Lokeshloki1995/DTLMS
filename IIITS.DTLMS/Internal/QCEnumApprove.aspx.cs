using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Collections;
using System.Data;
using System.IO;
using System.Configuration;

namespace IIITS.DTLMS.Internal
{
    public partial class QCEnumApprove : System.Web.UI.Page
    {
        string strFormCode = "QCEnumApprove";
        clsSession objSession;
        static int iIncrment = 2;

        int Circle_code = Convert.ToInt32(ConfigurationManager.AppSettings["Circle_code"]);
        int Division_code = Convert.ToInt32(ConfigurationManager.AppSettings["Division_code"]);
        int SubDiv_code = Convert.ToInt32(ConfigurationManager.AppSettings["SubDiv_code"]);
        int Section_code = Convert.ToInt32(ConfigurationManager.AppSettings["Section_code"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] Delete_Session_array = new string[7];
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];
                txtManufactureDate.Attributes.Add("readonly", "readonly");
                txtwelddate.Attributes.Add("readonly", "readonly");
                txtCommisionDate.Attributes.Add("readonly", "readonly");
                txtDtrcommissiondate.Attributes.Add("readonly", "readonly");
                txtServiceDate.Attributes.Add("readonly", "readonly");

                txtLongitude.Attributes.Add("readonly", "readonly");
                txtLatitude.Attributes.Add("readonly", "readonly");

                if (Session["arrSave_ImageSession_String"] != null)
                {
                    Delete_Session_array = Session["arrSave_ImageSession_String"] as string[];
                    for (int i = 0; i < 7; i++)
                    {
                        if (Delete_Session_array[i] != "")
                        {
                            Session.Remove(Delete_Session_array[i]);
                        }
                    }
                    Session.Remove("arrSave_ImageSession_String");
                }

                if (CheckAccessRights("4"))
                    txtManufactureDate_CalendarExtender.EndDate = System.DateTime.Now;
                txtCommisionDate_CalendarExtender1.EndDate = System.DateTime.Now;
                txtServiceDate_CalendarExtender1.EndDate = System.DateTime.Now;
                txtDTCCode.Attributes.Add("readonly", "readonly");
                cmbCircle.Attributes.Add("readonly", "readonly");
                cmbDivision.Attributes.Add("readonly", "readonly");
                cmbsubdivision.Attributes.Add("readonly", "readonly");
                cmbSection.Attributes.Add("readonly", "readonly");
                cmbCircle.Enabled = false;
                cmbsubdivision.Enabled = false;
                cmbSection.Enabled = false;
                txtwelddate.Enabled = false; cmbDivision.Enabled = false;
                if (!IsPostBack)
                {
                    Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" ORDER BY \"CM_CIRCLE_CODE\"", "--Select--", cmbCircle);
                    Genaral.Load_Combo("SELECT \"TM_ID\",\"TM_NAME\" FROM \"TBLTRANSMAKES\" ORDER BY \"TM_ID\"", "--Select--", cmbMake);
                    Genaral.Load_Combo("SELECT \"MD_NAME\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'C'", "--Select--", cmbCapacity);
                    Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'PT'", "--Select--", cmbProjecttype);
                    Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'LT'", "--Select--", cmbLoadtype);
                    Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'SR'", "--Select--", cmbRating);
                    Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'PLT'", "--Select--", cmbPlatformType);
                    Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'LC_TYPE'", "--Select--", cmdloctype);

                    if (Request.QueryString["QryEnumId"] != null && Request.QueryString["QryEnumId"].ToString() != "")
                    {
                        txtEnumDetailsId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryEnumId"]));
                        txtEnumType.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["EnumType"]));
                        string sStatus = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["Status"]));

                        RestrictUpdate(sStatus);
                        LoadEnumerationDetails(txtEnumDetailsId.Text);

                        if (txtEnumType.Text == "2")
                        {
                            cmbCircle_SelectedIndexChanged(sender, e);
                            cmbDivision.SelectedValue = hdfDivision.Value;
                            cmbDivision_SelectedIndexChanged(sender, e);
                            cmbsubdivision.SelectedValue = hdfSubdivision.Value;
                            cmbsubdivision_SelectedIndexChanged(sender, e);
                            cmbSection.SelectedValue = hdfSection.Value;
                            cmbsection_SelectedIndexChanged(sender, e);
                            cmbFeeder.SelectedValue = hdfFeeder.Value;
                        }
                        else if (txtEnumType.Text == "1" || txtEnumType.Text == "5")
                        {
                            cmbLocationType_SelectedIndexChanged(sender, e);
                            cmbLocationName.SelectedValue = hdfLocName.Value;
                        }
                        else if (txtEnumType.Text == "3")
                        {
                            cmbLocationType_SelectedIndexChanged(sender, e);
                            cmbLocationName.SelectedValue = hdfLocName.Value.Split('~').GetValue(0).ToString(); ;
                            cmbLocationName_SelectedIndexChanged(sender, e);

                            cmdDiv.SelectedValue = hdfLocName.Value.Split('~').GetValue(1).ToString();

                        }
                        if (cmbRating.SelectedValue == "1")
                        {
                            cmbRating_SelectedIndexChanged(sender, e);
                            cmbStarRated.SelectedValue = hdfStarRate.Value;
                        }

                        if (cmbMake.SelectedValue == "1")
                        {
                            cmbMake_SelectedIndexChanged(sender, e);
                        }

                        VisibilityEnumType();
                    }

                    txtwelddate.Attributes.Add("onblur", "return ValidateDate(" + txtwelddate.ClientID + ");");
                    dvStar.Style.Add("display", "none");

                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to check access rights to the form
        /// </summary>
        /// <param name="sAccessType"></param>
        /// <returns></returns>
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY
                clsApproval objApproval = new clsApproval();
                objApproval.sFormName = "QC EnumApprove";
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
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }
        /// <summary>
        /// to load the enumeration details
        /// </summary>
        /// <param name="strRecordId"></param>
        protected void LoadEnumerationDetails(string strRecordId)
        {
            try
            {
                clsFieldEnumeration objfield = new clsFieldEnumeration();
                objfield.sEnumDetailsID = strRecordId;

                objfield.GetEnumerationDetails(objfield);

                if (objfield.sDTCWithoutDTR != "")
                {
                    Mmake.Visible = false;
                    Mplateno.Visible = false;
                    lblNote.Text = "Note: DTC Without DTR Enumaration";
                    lblNote.Visible = true;
                    hdfDTCWithoutDTrFlag.Value = objfield.sDTCWithoutDTR;
                    Mmfdate.Visible = false;

                }

                clsCommon objComm = new clsCommon();

                //FTP ParameterdivTaggedId
                string sFTPLink = string.Empty;
                string sFTPUserName = string.Empty;
                string sFTPPassword = string.Empty;

                // To bind the Images from Ftp Path to Image Control
                DataTable dt = objComm.GetAppSettings();
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
                sFTPLink = ConfigurationManager.AppSettings["VirtualDirectoryPath"].ToString();
                clsFtp objFtp = new clsFtp(sFTPLink, sFTPUserName, sFTPPassword);

                if (txtEnumType.Text == "2")
                {
                    // DTC Details
                    cmbCircle.SelectedValue = objfield.sOfficeCode.Substring(0, Circle_code);
                    hdfDivision.Value = objfield.sOfficeCode.Substring(0, Division_code);
                    hdfSubdivision.Value = objfield.sOfficeCode.Substring(0, SubDiv_code);
                    hdfSection.Value = objfield.sOfficeCode.Substring(0, Section_code);
                    hdfFeeder.Value = objfield.sFeederCode;

                    txtDTCName.Text = objfield.sDTCName;
                    txtDTCCode.Text = objfield.sDTCCode;
                    txtDTCCode.ReadOnly = true;
                    txtoldDTCCode.Text = objfield.sOldDTCCode;
                    txtIPDTCCode.Text = objfield.sIPDTCCode;
                    cmbTaggedLocation.SelectedValue = objfield.staggedDTR;

                    if (cmbTaggedLocation.SelectedValue != "0")
                    {
                        cmbTaggedDTR.SelectedValue = "1";
                        cmbTaggedDTR.Visible = false;
                    }
                    cmbTaggedDTR.Visible = false;
                    txtIPDTCCode.Text = objfield.sIPDTCCode;
                    txtInternalCode.Text = objfield.sInternalCode;
                    txtConnectedKW.Text = objfield.sConnectedKW;
                    txtConnectedHP.Text = objfield.sConnectedHP;
                    txtKWHReading.Text = objfield.sKWHReading;
                    txtCommisionDate.Text = objfield.sCommisionDate;
                    txtDtrcommissiondate.Text = objfield.sDtrcommissiondate;
                    txtServiceDate.Text = objfield.sLastServiceDate;

                    if (objfield.sPlatformType != "")
                    {
                        cmbPlatformType.SelectedValue = objfield.sPlatformType;
                    }
                    if (objfield.sBreakertype != "")
                    {
                        cmbBreakerType.SelectedValue = objfield.sBreakertype;
                    }
                    if (objfield.sDTCMeters != "")
                    {
                        cmbDTCMetered.SelectedValue = objfield.sDTCMeters;
                    }
                    if (objfield.sHTProtect != "")
                    {
                        cmbHTProtection.SelectedValue = objfield.sHTProtect;
                    }
                    if (objfield.sLTProtect != "")
                    {
                        cmbLTProtection.SelectedValue = objfield.sLTProtect;
                    }
                    if (objfield.sGrounding != "")
                    {
                        cmbGrounding.SelectedValue = objfield.sGrounding;
                    }
                    if (objfield.sArresters != "")
                    {
                        cmbLightArrester.SelectedValue = objfield.sArresters;
                    }
                    if (objfield.sLoadtype != "")
                    {
                        cmbLoadtype.SelectedValue = objfield.sLoadtype;
                    }
                    if (objfield.sProjecttype != "")
                    {
                        cmbProjecttype.SelectedValue = objfield.sProjecttype;
                    }

                    txtltLine.Text = objfield.sLTlinelength;
                    txtHtLine.Text = objfield.sHTlinelength;
                    txtDepreciation.Text = objfield.sDepreciation;
                    txtLatitude.Text = objfield.sLatitude;
                    txtLongitude.Text = objfield.sLongitude;

                    if (objfield.sIPCESCValue == "1")
                    {
                        rdbDTLMS.Checked = true;
                    }
                    else if (objfield.sIPCESCValue == "2")
                    {
                        rdbDTLMS.Enabled = false;
                    }
                    else if (objfield.sIPCESCValue == "3")
                    {
                        rdbDTLMS.Enabled = false;
                    }
                    if (objfield.sOldCodePhotoPath != "")
                    {
                        imgOldCode.ImageUrl = sFTPLink + objfield.sOldCodePhotoPath;
                    }
                    if (objfield.sDTLMSCodePhotoPath != "")
                    {
                        imgDTLMS.ImageUrl = sFTPLink + objfield.sDTLMSCodePhotoPath;
                    }
                    if (objfield.sIPEnumCodePhotoPath != "")
                    {
                        imgIPEnum.ImageUrl = sFTPLink + objfield.sIPEnumCodePhotoPath;
                    }
                    if (objfield.sDTCPhotoPath != "")
                    {
                        if (objfield.sDTCPhotoPath.Contains(';'))
                        {
                            imgDTCPhoto.ImageUrl = sFTPLink + objfield.sDTCPhotoPath.Split(';').GetValue(1).ToString();
                        }
                        else
                        {
                            imgDTCPhoto.ImageUrl = sFTPLink + objfield.sDTCPhotoPath;
                        }
                    }
                }

                else if (txtEnumType.Text == "1" || txtEnumType.Text == "3" || txtEnumType.Text == "5")
                {
                    string qry = "SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" ";
                    qry += "= 'TCL' and \"MD_ID\" <> 2 and \"MD_ID\" <> 3 ORDER BY \"MD_NAME\"";
                    Genaral.Load_Combo(qry, "--Select--", cmbLocationType);

                    if (txtEnumType.Text == "1")
                    {
                        string Query = "SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\"";
                        Query += " WHERE \"MD_TYPE\"  = 'DTR_TYPE'  ORDER BY \"MD_NAME\"";
                        Genaral.Load_Combo(Query, "--Select--", cmbTranstype);
                    }
                    else
                    {
                        string Query = "SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"  ";
                        Query += " = 'DTR_TYPE'and \"MD_ID\" <>  4 and \"MD_ID\" <>  3  ORDER BY \"MD_NAME\"";
                        Genaral.Load_Combo(Query, "--Select--", cmbTranstype);
                    }

                    cmbLocationType.SelectedValue = objfield.sLocType;
                    hdfLocName.Value = objfield.sLocName + "~" + objfield.sOfficeCode;
                    txtLocAddress.Text = objfield.sLocAddress;
                    cmbTranstype.SelectedValue = objfield.sTCType;
                }

                // Transformer Details
                txtTcCode.Text = objfield.sTcCode;
                txtTcCode.ReadOnly = true;
                txtTcslno.Text = objfield.sTCSlno;

                if (objfield.sTCCapacity != "")
                {
                    cmbCapacity.SelectedValue = objfield.sTCCapacity;
                }

                cmbTaggedLocation.SelectedValue = objfield.staggedDTR;

                if (cmbTaggedLocation.SelectedValue != "0")
                {
                    cmbTaggedDTR.SelectedValue = "1";
                    divTaggedId.Visible = true;
                    cmbTaggedLocation.SelectedValue = objfield.sSpecialCase;
                }

                cmbMake.SelectedValue = objfield.sTCMake;
                if (cmbMake.SelectedValue == "1")
                {
                    txtManufactureDate.Text = "";
                }
                else
                {
                    txtManufactureDate.Text = objfield.sTCManfDate;
                }
                if (cmbMake.SelectedValue == "1")
                {
                    Div1.Style.Add("display", "none");
                }
                else
                {
                    Div1.Style.Add("display", "block");
                }
                txtwelddate.Text = objfield.sWeldDate;

                if (objfield.sRating != "")
                {
                    cmbRating.SelectedValue = objfield.sRating;
                }
                if (objfield.sStarRate != "")
                {
                    hdfStarRate.Value = objfield.sStarRate;
                }
                txtTankCapacity.Text = objfield.sTankCapacity;
                cmdloctype.SelectedValue = objfield.sLocation;
                txtEnumDetailsId.Text = objfield.sEnumDetailsID;

                if (txtTcslno.Text == objfield.sEnumDTCID)
                {
                    chkSlnoNotExist.Checked = true;
                }

                if (objfield.sNamePlatePhotoPath != "")
                {
                    imgNamePlate.ImageUrl = sFTPLink + objfield.sNamePlatePhotoPath;
                }
                if (objfield.sSSPlatePhotoPath != "")
                {
                    imgSSPlate.ImageUrl = sFTPLink + objfield.sSSPlatePhotoPath;
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// to reset the fields
        /// </summary>

        public void Reset()
        {
            try
            {

                txtTcCode.Text = string.Empty;
                txtTcslno.Text = string.Empty;
                cmbCapacity.SelectedIndex = 0;
                cmbMake.SelectedIndex = 0;
                txtManufactureDate.Text = string.Empty;

                txtDTCName.Text = string.Empty;
                txtDTCCode.Text = string.Empty;
                txtoldDTCCode.Text = string.Empty;
                txtIPDTCCode.Text = string.Empty;

                txtInternalCode.Text = string.Empty;
                txtConnectedKW.Text = string.Empty;
                txtConnectedKW.Text = string.Empty;
                txtKWHReading.Text = string.Empty;
                txtCommisionDate.Text = string.Empty;
                txtDtrcommissiondate.Text = string.Empty;
                txtServiceDate.Text = string.Empty;
                cmbPlatformType.SelectedIndex = 0;
                cmbBreakerType.SelectedIndex = 0;
                cmbDTCMetered.SelectedIndex = 0;
                cmbHTProtection.SelectedIndex = 0;
                cmbLTProtection.SelectedIndex = 0;
                cmbGrounding.SelectedIndex = 0;
                cmbLightArrester.SelectedIndex = 0;
                cmbLoadtype.SelectedIndex = 0;
                cmbProjecttype.SelectedIndex = 0;
                txtltLine.Text = string.Empty;
                txtDepreciation.Text = string.Empty;
                txtLatitude.Text = string.Empty;
                txtLongitude.Text = string.Empty;

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// validating the fields before save
        /// </summary>
        /// <returns></returns>
        bool ValidateForm()
        {
            bool bValidate = false;

            if (txtEnumType.Text == "2")
            {
                if (cmbCircle.SelectedIndex == 0)
                {
                    cmbCircle.Focus();
                    ShowMsgBox("Please Select the Circle");
                    return false;
                }
                if (cmbDivision.SelectedIndex == 0)
                {
                    cmbDivision.Focus();
                    ShowMsgBox("Please Select the Division");
                    return false;
                }
                if (cmbsubdivision.SelectedIndex == 0)
                {
                    cmbsubdivision.Focus();
                    ShowMsgBox("Please Select the Subdivision");
                    return false;
                }

                if (cmbSection.SelectedIndex == 0)
                {
                    cmbSection.Focus();
                    ShowMsgBox("Please Select the Section");
                    return false;
                }

                if (cmbFeeder.SelectedIndex == 0)
                {
                    cmbFeeder.Focus();
                    ShowMsgBox("Please Select the Feeder");
                    return false;
                }
            }

            if (txtwelddate.Text.Trim().Length == 0)
            {
                txtwelddate.Focus();
                ShowMsgBox("Enter Date of Fixing");
                return false;
            }
            if (hdfDTCWithoutDTrFlag.Value != "1")
            {

                if (txtTcCode.Text.Trim().Length == 0)
                {
                    txtTcCode.Focus();
                    ShowMsgBox("Enter Dtr Code");
                    return false;
                }

                if (cmbMake.SelectedIndex == 0)
                {
                    cmbMake.Focus();
                    ShowMsgBox("Please Select the DTr Make");
                    return false;
                }
            }
            if (cmbLocationName.SelectedIndex == 0)
            {
                cmbLocationName.Focus();
                ShowMsgBox("Please select Store");
                return false;
            }

            if (txtTcslno.Text.Trim().Length == 0)
            {
                txtTcslno.Focus();
                ShowMsgBox("Enter DTr SLNO");
                return false;
            }
            if (cmbCapacity.SelectedIndex == 0)
            {
                cmbCapacity.Focus();
                ShowMsgBox("Select DTr Capacity");
                return false;
            }
            if (cmbRating.SelectedIndex == 0)
            {
                cmbRating.Focus();
                ShowMsgBox("Select Rating");
                return false;
            }

            if (cmbMake.SelectedValue != "1")
            {
                if (txtManufactureDate.Text.Trim().Length == 0)
                {
                    txtwelddate.Focus();
                    ShowMsgBox("Enter Manufacture Date");
                    return false;
                }
            }
            if (txtEnumType.Text != "1" && txtEnumType.Text != "3" && txtEnumType.Text != "5")
            {

                if (cmdloctype.SelectedIndex == 0)
                {
                    cmdloctype.Focus();
                    ShowMsgBox("Select Location Type");
                    return false;
                }

                if (txtConnectedKW.Text.Trim().Length == 0)
                {
                    txtConnectedKW.Focus();
                    ShowMsgBox("Enter Connected KW");
                    return false;
                }

                if (txtConnectedKW.Text.Length > 0)
                {
                    //if (Convert.ToDouble(txtConnectedKW.Text) == 0)
                    //{
                    //    txtConnectedKW.Text = "";
                    //    txtConnectedKW.Focus();
                    //    ShowMsgBox("Please Enter Valid Connected Kw Length (Eg: 111.11)");
                    //    return false;
                    //}

                    if (txtConnectedKW.Text.Contains("."))
                    {
                        string decimalString = txtConnectedKW.Text.ToString();
                        int index = decimalString.IndexOf('.');

                        if (index > 3)
                        {
                            //txtConnectedKW.Text = "";
                            txtConnectedKW.Focus();
                            ShowMsgBox("Please Enter Valid Connected KW Length (Eg: 111.11)");
                            return false;
                        }

                        if ((decimalString.Length - (index + 1)) != 2 && (decimalString.Length - (index + 1)) != 1)
                        {
                            //txtConnectedKW.Text = "";
                            txtConnectedKW.Focus();
                            ShowMsgBox("Please Enter Valid Connected KW Length (Eg: 111.11)");
                            return false;
                        }
                    }
                    else
                    {
                        if (txtConnectedKW.Text.Length > 3)
                        {
                            //txtConnectedKW.Text = "";
                            txtConnectedKW.Focus();
                            ShowMsgBox("Please Enter Valid Connected KW Length (Eg: 111.11)");
                            return false;
                        }
                    }
                }

                if (txtConnectedHP.Text.Trim().Length == 0)
                {
                    txtConnectedHP.Focus();
                    ShowMsgBox("Enter Connected HP");
                    return false;
                }

                if (txtConnectedHP.Text.Length > 0)
                {
                    //if (Convert.ToDouble(txtConnectedHP.Text) == 0)
                    //{
                    //    txtConnectedHP.Text = "";
                    //    txtConnectedHP.Focus();
                    //    ShowMsgBox("Please Enter Valid Connected HP Length (Eg: 111.11)");
                    //    return false;
                    //}

                    if (txtConnectedHP.Text.Contains("."))
                    {
                        string decimalString = txtConnectedHP.Text.ToString();
                        int index = decimalString.IndexOf('.');

                        if (index > 3)
                        {
                            //txtConnectedHP.Text = "";
                            txtConnectedHP.Focus();
                            ShowMsgBox("Please Enter Valid Connected HP Length (Eg: 111.11)");
                            return false;
                        }

                        if ((decimalString.Length - (index + 1)) != 2 && (decimalString.Length - (index + 1)) != 1)
                        {
                            //txtConnectedHP.Text = "";
                            txtConnectedHP.Focus();
                            ShowMsgBox("Please Enter Valid Connected HP Length (Eg: 111.11)");
                            return false;
                        }
                    }
                    else
                    {
                        if (txtConnectedHP.Text.Length > 3)
                        {
                            //txtConnectedHP.Text = "";
                            txtConnectedHP.Focus();
                            ShowMsgBox("Please Enter Valid Connected HP Length (Eg: 111.11)");
                            return false;
                        }
                    }
                }

                if (txtKWHReading.Text.Trim().Length == 0)
                {
                    txtKWHReading.Focus();
                    ShowMsgBox("Enter KWH Reading");
                    return false;
                }

                if (txtKWHReading.Text.Trim() == ".")
                {
                    txtKWHReading.Focus();
                    ShowMsgBox("Enter vaild KWH Reading");
                    return false;
                }

                if (cmbPlatformType.SelectedIndex == 0)
                {
                    cmbPlatformType.Focus();
                    ShowMsgBox("Please select Platform Type ");
                    return false;
                }
                if (cmbBreakerType.SelectedIndex == 0)
                {
                    cmbBreakerType.Focus();
                    ShowMsgBox("Please select Breaker Type");
                    return false;
                }
                if (cmbDTCMetered.SelectedIndex == 0)
                {
                    cmbDTCMetered.Focus();
                    ShowMsgBox("Please select DTC Meters Available");
                    return false;
                }
                if (cmbHTProtection.SelectedIndex == 0)
                {
                    cmbHTProtection.Focus();
                    ShowMsgBox("Please select HT Protection");
                    return false;
                }
                if (cmbLTProtection.SelectedIndex == 0)
                {
                    cmbLTProtection.Focus();
                    ShowMsgBox("Please select LT Protection");
                    return false;
                }
                if (cmbGrounding.SelectedIndex == 0)
                {
                    cmbGrounding.Focus();
                    ShowMsgBox("Please select Grounding");
                    return false;
                }
                if (cmbLightArrester.SelectedIndex == 0)
                {
                    cmbLightArrester.Focus();
                    ShowMsgBox("Please select Lightning Arresters");
                    return false;
                }
                if (cmbLoadtype.SelectedIndex == 0)
                {
                    cmbLoadtype.Focus();
                    ShowMsgBox("Please select Load Type");
                    return false;
                }
                if (cmbProjecttype.SelectedIndex == 0)
                {
                    cmbProjecttype.Focus();
                    ShowMsgBox("Please select Project/Scheme Type");
                    return false;
                }
                if (txtltLine.Text != "")
                {
                    string Val = txtltLine.Text.Trim();
                    string afterDotVal = Val.Contains('.') ? Val.Split('.')[1] : Val;
                    bool ReturnValue = false;
                    if (Val.Length > 3 && Val.Length <= 7 && !Val.Contains('.'))
                    {
                        txtltLine.Focus();
                        ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                        return bValidate;
                    }
                    else if (Val.IndexOf('.') == 0 || afterDotVal.Length == 0)
                    {
                        txtltLine.Focus();
                        ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                        return bValidate;
                    }
                    else if (!(Val.IndexOf('.') <= 3))
                    {
                        txtltLine.Focus();
                        ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                        return bValidate;
                    }
                    else if (Val.Contains('.') && afterDotVal.Length > 3)
                    {
                        int ltindex = txtltLine.Text.IndexOf('.');
                        string ltstr = txtltLine.Text.Substring(ltindex + 1);
                        int val = Convert.ToInt16(ltstr);
                        if (val == 0)
                        {
                            string lltstr = txtltLine.Text.Substring(0, ltindex);

                            if (Convert.ToInt16(lltstr) == 0)
                            {
                                ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                                return bValidate;
                            }

                        }
                        string lltstr1 = txtltLine.Text.Substring(0, ltindex);
                        if (lltstr1.Length > 3)
                        {
                            ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                            return bValidate;
                        }
                    }
                    if (txtltLine.Text.Contains('.'))
                    {
                        switch (Val)
                        {
                            case "0.":
                                ReturnValue = true;
                                break;
                            case "00.":
                                ReturnValue = true;
                                break;
                            case "000.":
                                ReturnValue = true;
                                break;
                            case ".":
                                ReturnValue = true;
                                break;
                            case ".0":
                                ReturnValue = true;
                                break;
                            case ".00":
                                ReturnValue = true;
                                break;
                            case ".000":
                                ReturnValue = true;
                                break;
                            case "0.00":
                                ReturnValue = true;
                                break;
                            case "0.000":
                                ReturnValue = true;
                                break;
                            case "00.0":
                                ReturnValue = true;
                                break;
                            case "00.00":
                                ReturnValue = true;
                                break;
                            case "00.000":
                                ReturnValue = true;
                                break;
                            case "000.0":
                                ReturnValue = true;
                                break;
                            case "000.00":
                                ReturnValue = true;
                                break;
                            case "000.000":
                                ReturnValue = true;
                                break;
                        }
                        if (ReturnValue)
                        {
                            txtltLine.Focus();
                            ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                            return bValidate;
                        }
                    }

                    //if (txtltLine.Text.Trim().Length != 0)
                    //{
                    //    double val = Convert.ToDouble(txtltLine.Text);
                    //    if (val < 0.0 || val > 999.999)
                    //    {
                    //        txtltLine.Focus();
                    //        ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                    //        return bValidate;
                    //    }
                    //    if (txtltLine.Text == ".")
                    //    {
                    //        ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                    //        return bValidate;
                    //    }
                    //}

                    //if (txtltLine.Text != "0")
                    //{
                    //    int ltindex = txtltLine.Text.IndexOf('.');
                    //    string ltstr = txtltLine.Text.Substring(ltindex + 1);
                    //    int val = Convert.ToInt16(ltstr);
                    //    if (val == 0)
                    //    {
                    //        string lltstr = txtltLine.Text.Substring(0, ltindex);

                    //        if (Convert.ToInt16(lltstr) == 0)
                    //        {
                    //            ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                    //            return bValidate;
                    //        }

                    //    }
                    //    string lltstr1 = txtltLine.Text.Substring(0, ltindex);
                    //    if (lltstr1.Length > 3)
                    //    {
                    //        ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                    //        return bValidate;
                    //    }
                    //}
                    //if (txtltLine.Text.Contains('.'))
                    //{

                    //    string LTPattern = @"[0-9]{0,3}^\d+\.\d{0,3}$";
                    //    System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(LTPattern);
                    //    if (!r.IsMatch(txtltLine.Text))
                    //    {
                    //        ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                    //        return bValidate;
                    //    }
                    //}

                }
                if (txtHtLine.Text != "")
                {
                    string Val = txtHtLine.Text.Trim();
                    string afterDotVal = Val.Contains('.') ? Val.Split('.')[1] : Val;
                    bool ReturnValue = false;
                    if (Val.Length > 3 && Val.Length <= 7 && !Val.Contains('.'))
                    {
                        txtHtLine.Focus();
                        ShowMsgBox("Please Enter a valid HT line Length (Eg: 111.111)");
                        return bValidate;
                    }
                    else if (!(Val.IndexOf('.') <= 3))
                    {
                        txtHtLine.Focus();
                        ShowMsgBox("Please Enter a valid HT line Length (Eg: 111.111)");
                        return bValidate;
                    }
                    else if (Val.IndexOf('.') == 0 || afterDotVal.Length == 0)
                    {
                        txtHtLine.Focus();
                        ShowMsgBox("Please Enter a valid HT line Length (Eg: 111.111)");
                        return bValidate;
                    }
                    else if (Val.Contains('.') && afterDotVal.Length > 3)
                    {
                        txtHtLine.Focus();
                        ShowMsgBox("Please Enter a valid HT line Length (Eg: 111.111)");
                        return bValidate;
                    }
                    else if (Val.Length > 0 && Val.Length <= 3 && !(Val.Contains('.')))
                    {
                        switch (Val)
                        {
                            case "00":
                                ReturnValue = true;
                                break;
                            case "000":
                                ReturnValue = true;
                                break;
                        }
                        if (ReturnValue)
                        {
                            txtHtLine.Focus();
                            ShowMsgBox("Please Enter a valid HT line Length (Eg: 111.111)");
                            return bValidate;
                        }
                    }
                    else if (Val.Length > 0 && Val.Contains('.') && Val.Length <= 7)
                    {
                        switch (Val)
                        {
                            case "0.":
                                ReturnValue = true;
                                break;
                            case "00.":
                                ReturnValue = true;
                                break;
                            case "000.":
                                ReturnValue = true;
                                break;
                            case ".":
                                ReturnValue = true;
                                break;
                            case ".0":
                                ReturnValue = true;
                                break;
                            case ".00":
                                ReturnValue = true;
                                break;
                            case ".000":
                                ReturnValue = true;
                                break;
                            case "0.00":
                                ReturnValue = true;
                                break;
                            case "0.000":
                                ReturnValue = true;
                                break;
                            case "00.0":
                                ReturnValue = true;
                                break;
                            case "00.00":
                                ReturnValue = true;
                                break;
                            case "00.000":
                                ReturnValue = true;
                                break;
                            case "000.0":
                                ReturnValue = true;
                                break;
                            case "000.00":
                                ReturnValue = true;
                                break;
                            case "000.000":
                                ReturnValue = true;
                                break;
                        }
                        if (ReturnValue)
                        {
                            txtHtLine.Focus();
                            ShowMsgBox("Please Enter a valid HT line Length (Eg: 111.111)");
                            return bValidate;
                        }
                    }
                    //if (txtHtLine.Text.Trim().Length != 0)
                    //{
                    //    double val = Convert.ToDouble(txtHtLine.Text);
                    //    if (val < 0.0 || val > 999.999)
                    //    {
                    //        txtHtLine.Focus();
                    //        ShowMsgBox("Please Enter a valid HT line Length (Eg: 111.111)");
                    //        return bValidate;
                    //    }
                    //    if (txtHtLine.Text == ".")
                    //    {
                    //        ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                    //        return bValidate;
                    //    }
                    //}
                    //if (txtHtLine.Text != "0")
                    //{
                    //    if (txtHtLine.Text.Contains('.'))
                    //    {
                    //        int ltindex = txtHtLine.Text.IndexOf('.');
                    //        string ltstr = txtHtLine.Text.Substring(ltindex + 1);
                    //        int val = Convert.ToInt16(ltstr);
                    //        if (val == 0)
                    //        {
                    //            string lltstr = txtHtLine.Text.Substring(0, ltindex);

                    //            if (Convert.ToInt16(lltstr) == 0)
                    //            {
                    //                ShowMsgBox("Please Enter a valid HT line Length (Eg: 111.111)");
                    //                return bValidate;
                    //            }

                    //        }
                    //        string lltstr1 = txtHtLine.Text.Substring(0, ltindex);
                    //        if (lltstr1.Length > 3)
                    //        {
                    //            ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                    //            return bValidate;
                    //        }
                    //    }
                    //}
                    //if (txtHtLine.Text.Contains('.'))
                    //{
                    //    string HTPattern = @"[0-9]{0,3}^\d+\.\d{0,3}$";
                    //    System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(HTPattern);
                    //    if (!r.IsMatch(txtHtLine.Text))
                    //    {
                    //        ShowMsgBox("Please Enter a valid HT line Length (Eg: 111.111)");
                    //        return bValidate;
                    //    }
                    //}
                }
                string sResult = Genaral.DateValidation(txtwelddate.Text);
                if (txtDtrcommissiondate.Text != "" && txtCommisionDate.Text != "")
                {
                    sResult = Genaral.DateComparision(txtDtrcommissiondate.Text, txtCommisionDate.Text, false, false);
                }
                if (sResult == "2")
                {
                    ShowMsgBox("DTr Commission Date should be Greater than or equal to DTC Commission Date");
                    return false;
                }
                if (hdfDTCWithoutDTrFlag.Value != "1")
                {
                    if (txtManufactureDate.Text.Trim() != "")
                    {
                        string DatePattern = @"^(0[1-9]|[12][0-9]|3[01])[-/.](0[1-9]|1[012])[-/.](19|20)\d\d$";
                        System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(DatePattern);
                        if (r.IsMatch(txtManufactureDate.Text))
                        {
                        }
                        else
                        {
                            ShowMsgBox("Please Enter a valid Maufacture date in the format (dd/mm/yyyy)");
                            return bValidate;
                        }
                    }
                }
            }
            if (txtEnumType.Text == "2")
            {
                if (txtDTCName.Text.Trim().Length == 0)
                {
                    txtDTCName.Focus();
                    ShowMsgBox("Enter DTC Name");
                    return false;
                }
                if (txtDTCCode.Text.Trim().Length == 0)
                {
                    txtDTCCode.Focus();
                    ShowMsgBox("Enter DTC Code");
                    return false;
                }
            }

            bValidate = true;
            return bValidate;
        }
        /// <summary>
        /// to show the popup message
        /// </summary>
        /// <param name="sMsg"></param>
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
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// onchange method for circle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle.SelectedIndex > 0)
                {
                    string qry = "SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\"  WHERE ";
                    qry += " CAST(\"DIV_CICLE_CODE\" AS TEXT)='" + cmbCircle.SelectedValue + "' ORDER BY \"DIV_CODE\"";
                    Genaral.Load_Combo(qry, "--Select--", cmbDivision);
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbDTCMeters_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDTCMetered.SelectedValue == "2")
                {
                    // txtKWHReading.Enabled = false;
                    idtxtKWHReading.Visible = false;
                    txtKWHReading.Text = "0";
                }
                else
                {
                    //txtKWHReading.Enabled = true;
                    idtxtKWHReading.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// onchange method for division
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDivision.SelectedIndex > 0)
                {
                    string qry = "SELECT \"SD_SUBDIV_CODE\", \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" ";
                    qry += " WHERE CAST(\"SD_DIV_CODE\" AS TEXT)='" + cmbDivision.SelectedValue + "' ORDER BY \"SD_SUBDIV_CODE\"";
                    Genaral.Load_Combo(qry, "--Select--", cmbsubdivision);
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// onchange method for subdivision
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbsubdivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbsubdivision.SelectedIndex > 0)
                {
                    string qry = "SELECT \"OM_CODE\", \"OM_NAME\" FROM \"TBLOMSECMAST\" ";
                    qry += " WHERE CAST(\"OM_SUBDIV_CODE\" AS TEXT) = '" + cmbsubdivision.SelectedValue + "' ORDER BY \"OM_CODE\"";
                    Genaral.Load_Combo(qry, "--Select--", cmbSection);
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// onchange method for section
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbsection_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbsubdivision.SelectedIndex > 0)
                {
                    string strQry = "SELECT DISTINCT \"FD_FEEDER_CODE\",\"FD_FEEDER_CODE\" || '-' || \"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\", ";
                    strQry += " \"TBLFEEDEROFFCODE\" WHERE  CAST(\"FD_FEEDER_ID\" AS TEXT) = CAST(\"FDO_FEEDER_ID\" AS TEXT) AND";
                    strQry += " CAST(\"FDO_OFFICE_CODE\" AS TEXT) LIKE '" + cmbSection.SelectedValue + "%' ORDER BY \"FD_FEEDER_CODE\"";
                    Genaral.Load_Combo(strQry, "--Select--", cmbFeeder);
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to save the enumeration details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnApproval_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTcslno.Text.Contains("'"))
                {
                    txtTcslno.Text = txtTcslno.Text.Replace("'", "");
                }
                if (ValidateForm() == true)
                {
                    ApproveEnumerationDetails();
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to reset the fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {

                txtTcCode.Text = string.Empty;
                txtTcslno.Text = string.Empty;
                cmbCapacity.SelectedIndex = 0;
                cmbMake.SelectedIndex = 0;
                txtManufactureDate.Text = string.Empty;

                txtDTCName.Text = string.Empty;
                txtDTCCode.Text = string.Empty;
                txtoldDTCCode.Text = string.Empty;
                txtIPDTCCode.Text = string.Empty;

                txtInternalCode.Text = string.Empty;
                txtConnectedKW.Text = string.Empty;
                txtConnectedKW.Text = string.Empty;
                txtKWHReading.Text = string.Empty;
                txtCommisionDate.Text = string.Empty;
                txtDtrcommissiondate.Text = string.Empty;
                txtServiceDate.Text = string.Empty;
                cmbPlatformType.SelectedIndex = 0;
                cmbBreakerType.SelectedIndex = 0;
                cmbDTCMetered.SelectedIndex = 0;
                cmbHTProtection.SelectedIndex = 0;
                cmbLTProtection.SelectedIndex = 0;
                cmbGrounding.SelectedIndex = 0;
                cmbLightArrester.SelectedIndex = 0;
                cmbLoadtype.SelectedIndex = 0;
                cmbProjecttype.SelectedIndex = 0;
                txtltLine.Text = string.Empty;
                txtDepreciation.Text = string.Empty;
                txtLatitude.Text = string.Empty;
                txtLongitude.Text = string.Empty;
                txtConnectedKW.Text = string.Empty;
                cmbCircle.SelectedIndex = 0;
                cmbDivision.SelectedIndex = 0;
                cmbsubdivision.SelectedIndex = 0;
                cmbSection.SelectedIndex = 0;
                cmbFeeder.SelectedIndex = 0;
                txtwelddate.Text = string.Empty;

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to validate and to save the details
        /// </summary>
        public void ApproveEnumerationDetails()
        {
            try
            {
                clsFieldEnumeration objFieldEnum = new clsFieldEnumeration();
                string[] Arr = new string[2];

                if (txtRemark.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Comments");
                    return;

                }

                objFieldEnum.sQCApprovalId = txtApproveId.Text;
                objFieldEnum.sEnumDetailsID = txtEnumDetailsId.Text;

                objFieldEnum.sWeldDate = txtwelddate.Text;
                objFieldEnum.sEnumDate = txtwelddate.Text;
                objFieldEnum.staggedDTR = cmbTaggedDTR.SelectedValue;

                //TC Details
                objFieldEnum.sTcCode = txtTcCode.Text;
                objFieldEnum.sTCMake = cmbMake.SelectedValue;
                if (cmbCapacity.SelectedIndex > 0)
                {
                    objFieldEnum.sTCCapacity = cmbCapacity.SelectedValue;
                }
                objFieldEnum.sTCSlno = txtTcslno.Text;
                objFieldEnum.sTCManfDate = txtManufactureDate.Text;

                if (txtEnumType.Text == "2")
                {
                    objFieldEnum.sOfficeCode = cmbSection.SelectedValue;
                    objFieldEnum.sFeederCode = cmbFeeder.SelectedValue;

                    // DTC Details
                    objFieldEnum.sDTCName = txtDTCName.Text;
                    objFieldEnum.sDTCCode = txtDTCCode.Text;
                    objFieldEnum.sOldDTCCode = txtoldDTCCode.Text;
                    objFieldEnum.sIPDTCCode = txtIPDTCCode.Text;
                    objFieldEnum.sEnumDate = txtwelddate.Text;

                    // DTC Other Details
                    objFieldEnum.sDTCWithoutDTR = hdfDTCWithoutDTrFlag.Value;

                    objFieldEnum.sInternalCode = txtInternalCode.Text;
                    objFieldEnum.sConnectedKW = txtConnectedKW.Text;
                    objFieldEnum.sConnectedHP = txtConnectedHP.Text;
                    objFieldEnum.sKWHReading = txtKWHReading.Text;
                    objFieldEnum.sCommisionDate = txtCommisionDate.Text;
                    objFieldEnum.sDtrcommissiondate = txtDtrcommissiondate.Text;
                    objFieldEnum.sLastServiceDate = txtServiceDate.Text;

                    if (cmbPlatformType.SelectedIndex > 0)
                    {
                        objFieldEnum.sPlatformType = cmbPlatformType.SelectedValue;
                    }
                    if (cmbBreakerType.SelectedIndex > 0)
                    {
                        objFieldEnum.sBreakertype = cmbBreakerType.SelectedValue;
                    }
                    if (cmbDTCMetered.SelectedIndex > 0)
                    {
                        objFieldEnum.sDTCMeters = cmbDTCMetered.SelectedValue;
                    }
                    if (cmbHTProtection.SelectedIndex > 0)
                    {
                        objFieldEnum.sHTProtect = cmbHTProtection.SelectedValue;
                    }
                    if (cmbLTProtection.SelectedIndex > 0)
                    {
                        objFieldEnum.sLTProtect = cmbLTProtection.SelectedValue;
                    }
                    if (cmbGrounding.SelectedIndex > 0)
                    {
                        objFieldEnum.sGrounding = cmbGrounding.SelectedValue;
                    }
                    if (cmbLightArrester.SelectedIndex > 0)
                    {
                        objFieldEnum.sArresters = cmbLightArrester.SelectedValue;
                    }
                    if (cmbLoadtype.SelectedIndex > 0)
                    {
                        objFieldEnum.sLoadtype = cmbLoadtype.SelectedValue;
                    }
                    if (cmbProjecttype.SelectedIndex > 0)
                    {
                        objFieldEnum.sProjecttype = cmbProjecttype.SelectedValue;
                    }
                    objFieldEnum.sLTlinelength = txtltLine.Text.Trim();

                    if (Convert.ToString(txtHtLine.Text ?? "").Length > 0)
                    {
                        objFieldEnum.sHTlinelength = txtHtLine.Text.Trim();
                    }

                    objFieldEnum.sDepreciation = txtDepreciation.Text;
                    objFieldEnum.sLatitude = txtLatitude.Text;
                    objFieldEnum.sLongitude = txtLongitude.Text;

                }
                else if (txtEnumType.Text == "1" || txtEnumType.Text == "3" || txtEnumType.Text == "5")
                {
                    objFieldEnum.sOfficeCode = cmbLocationName.SelectedValue.Split('~').GetValue(0).ToString();
                    objFieldEnum.sLocName = cmbLocationName.SelectedValue.Split('~').GetValue(0).ToString();
                    objFieldEnum.sLocAddress = txtLocAddress.Text.Trim().Replace("'", "");
                    objFieldEnum.sTCType = cmbTranstype.SelectedValue;

                }
                if (txtEnumType.Text == "1" || txtEnumType.Text == "5")
                {
                    objFieldEnum.sLocation = cmbLocationName.SelectedValue.Split('~').GetValue(1).ToString();
                }
                else if (txtEnumType.Text == "3")
                {
                    if (cmdDiv.SelectedValue == "--Select--")
                    {
                        ShowMsgBox("Please Select Division");
                        return;
                    }

                    objFieldEnum.sLocation = cmdDiv.SelectedValue;
                }

                objFieldEnum.sCrBy = objSession.UserId;
                objFieldEnum.sEnumType = txtEnumType.Text;
                objFieldEnum.locationtype = cmdloctype.SelectedValue;
                if (cmdloctype.SelectedValue == "--Select--")
                {
                    objFieldEnum.locationtype = "";
                }
                objFieldEnum.sTankCapacity = txtTankCapacity.Text;
                if (txtEnumType.Text == "2")
                {
                    objFieldEnum.sLocation = cmbSection.SelectedValue;
                }

                if (cmbRating.SelectedIndex > 0)
                {
                    objFieldEnum.sRating = cmbRating.SelectedValue;
                }
                if (cmbStarRated.SelectedIndex > 0)
                {
                    objFieldEnum.sStarRate = cmbStarRated.SelectedValue;
                }
                objFieldEnum.sUserType = "0";
                objFieldEnum.sStatus = "0";

                // checking make and serial No in main table.
                string[] Array = new string[2];
                Array = objFieldEnum.CheckMakeSerialNoOfTC(cmbMake.SelectedValue, txtTcslno.Text, txtTcCode.Text);
                if (Array[1].ToString() == "2")
                {
                    ShowMsgBox(Array[0].ToString());
                    return;
                }

                Arr = objFieldEnum.ApproveQCEnumerationDetails(objFieldEnum);

                if (Arr[1].ToString() == "0")
                {
                    ShowMsgBox(Arr[0].ToString());

                    //btnPending.Enabled = false;
                    btnReject.Enabled = false;
                    btnApproval.Enabled = false;
                    btnReject.Visible = false;
                    return;
                }

                if (Arr[1].ToString() == "2" || Arr[1].ToString() == "3")
                {
                    ShowMsgBox(Arr[0]);
                    return;
                }

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// method for reject case
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReject_Click(object sender, EventArgs e)
        {
            string[] Arr = new string[3];
            try
            {
                clsFieldEnumeration objFieldEnum = new clsFieldEnumeration();

                if (txtRemark.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Comments");
                    return;

                }

                objFieldEnum.sRemark = txtRemark.Text;
                objFieldEnum.sEnumDetailsID = txtEnumDetailsId.Text;
                objFieldEnum.sCrBy = objSession.UserId;

                //  bool bResult = objFieldEnum.RejectEnumerationDetails(objFieldEnum);
                Arr = objFieldEnum.RejectEnumerationDetails(objFieldEnum);
                if (Arr[0] == "1")
                {
                    ShowMsgBox("Enumeration Details Rejected Successfully");

                    btnApproval.Enabled = false;
                    //btnPending.Enabled = false;
                    btnReject.Enabled = false;
                    cmdNextDetails.Visible = false;
                }
                else if (Arr[0] == "0")
                {
                    ShowMsgBox(Arr[1].ToString());
                    return;
                }
                else
                {
                    ShowMsgBox("Reject Failed");
                    return;
                }

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        // UnUsed Method

        /// <summary>
        /// method for pending details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void btnPending_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        clsFieldEnumeration objFieldEnum = new clsFieldEnumeration();

        //        if (txtRemark.Text.Trim() == "")
        //        {
        //            ShowMsgBox("Enter Comments");
        //            return;

        //        }

        //        objFieldEnum.sRemark = txtRemark.Text;
        //        objFieldEnum.sEnumDetailsID = txtEnumDetailsId.Text;
        //        objFieldEnum.sCrBy = objSession.UserId;

        //        bool bResult = objFieldEnum.PendingForClarification(objFieldEnum);

        //        if (bResult == true)
        //        {
        //            ShowMsgBox("Enumeration Details Sent for Clarification");

        //            btnApproval.Enabled = false;
        //            btnReject.Enabled = false;
        //            btnPending.Enabled = false;
        //        }
        //        else
        //        {
        //            ShowMsgBox("Sending Failed");
        //            return;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        lblmessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }

        //}


        /// <summary>
        /// onchange method for location name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbLocationName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                clsStoreEnumeration objenumeration = new clsStoreEnumeration();
                if (cmbLocationName.SelectedIndex > 0)
                {

                    if (cmbLocationType.SelectedValue == "3")
                    {
                        objenumeration.sValue = "3";
                        objenumeration.sSelectedValue = cmbLocationName.SelectedValue.Split('~').GetValue(0).ToString();
                        string qry = "Select \"DIV_CODE\" ,\"DIV_NAME\" from \"TBLTRANSREPAIRER\" INNER JOIN \"TBLTRANSREPAIREROFFCODE\" ";
                        qry += " ON \"TR_ID\"=\"TRO_TR_ID\"  INNER JOIN \"TBLDIVISION\" ON \"TRO_OFF_CODE\"=\"DIV_CODE\" AND \"TR_ID\"= ";
                        qry += " " + cmbLocationName.SelectedValue.Split('~').GetValue(0).ToString() + "";
                        Genaral.Load_Combo(qry, "--Select--", cmdDiv);
                    }

                }


            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// method to change visibality fields for various cases
        /// </summary>
        public void VisibilityEnumType()
        {
            try
            {
                if (txtEnumType.Text == "1" || txtEnumType.Text == "3" || txtEnumType.Text == "5")
                {
                    dvCircle.Style.Add("display", "none");
                    dvDiv.Style.Add("display", "none");
                    dvSub.Style.Add("display", "none");
                    dvSection.Style.Add("display", "none");
                    dvFeeder.Style.Add("display", "none");

                    dvLocAddress.Style.Add("display", "block");
                    dvLocName.Style.Add("display", "block");
                    dvLocType.Style.Add("display", "block");
                    dvTransType.Style.Add("display", "block");

                    liDTCDetails.Style.Add("display", "none");
                    liOtherDetails.Style.Add("display", "none");
                    dvfieldLocation.Style.Add("display", "none");

                }
                else
                {
                    dvCircle.Style.Add("display", "block");
                    dvDiv.Style.Add("display", "block");
                    dvSub.Style.Add("display", "block");
                    dvSection.Style.Add("display", "block");
                    dvFeeder.Style.Add("display", "block");

                    dvLocAddress.Style.Add("display", "none");
                    dvLocName.Style.Add("display", "none");
                    dvLocType.Style.Add("display", "none");
                    dvTransType.Style.Add("display", "none");
                    dvfieldLocation.Style.Add("display", "block");
                    liDTCDetails.Style.Add("display", "block");
                    liOtherDetails.Style.Add("display", "block");
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// onchange method for location type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbLocationType_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                if (cmbLocationType.SelectedValue == "1" || cmbLocationType.SelectedValue == "5")
                {
                    if (objSession.RoleId == "8")
                    {
                        string qry = "SELECT \"SM_ID\" || '~' || \"STO_OFF_CODE\",\"DIV_NAME\" FROM \"TBLSTOREMAST\", \"TBLSTOREOFFCODE\", ";
                        qry += " \"TBLDIVISION\" WHERE \"SM_ID\" = \"STO_SM_ID\"  AND \"STO_OFF_CODE\" = \"DIV_CODE\" ORDER BY \"DIV_NAME\"";
                        Genaral.Load_Combo(qry, "--Select--", cmbLocationName);
                    }
                    else
                    {
                        string qry = "SELECT \"SM_ID\" || '~' || \"STO_OFF_CODE\",\"DIV_NAME\" FROM \"TBLSTOREMAST\", \"TBLSTOREOFFCODE\", ";
                        qry += " \"TBLDIVISION\" WHERE \"SM_ID\" = \"STO_SM_ID\" And \"SM_ID\" = '" + objSession.OfficeCode + "' ";
                        qry += " AND \"STO_OFF_CODE\" = \"DIV_CODE\" ORDER BY \"DIV_NAME\"";
                        Genaral.Load_Combo(qry, "--Select--", cmbLocationName);
                    }
                    lblRepairerName.Text = "Store Name";
                    lblAddress.Text = "Store Address";
                    dvdivision.Visible = false;
                }
                else if (cmbLocationType.SelectedValue == "3")
                {
                    if (objSession.RoleId == "8")
                    {
                        string qry = "SELECT \"TR_ID\",\"TR_NAME\"  FROM \"TBLTRANSREPAIRER\" ,\"TBLTRANSREPAIREROFFCODE\"  , \"TBLSTOREMAST\", ";
                        qry += " \"TBLSTOREOFFCODE\"   Where \"TRO_TR_ID\" = \"TR_ID\"  and \"STO_OFF_CODE\" =\"TRO_OFF_CODE\" and \"SM_ID\" = ";
                        qry += " \"STO_SM_ID\"  GROUP BY \"TR_ID\",\"TR_NAME\"";
                        Genaral.Load_Combo(qry, "--Select--", cmbLocationName);
                    }
                    else
                    {
                        string qry = "SELECT \"TR_ID\",\"TR_NAME\"  FROM \"TBLTRANSREPAIRER\" ,\"TBLTRANSREPAIREROFFCODE\"  , \"TBLSTOREMAST\", ";
                        qry += " \"TBLSTOREOFFCODE\"   Where \"TRO_TR_ID\" = \"TR_ID\"  and \"STO_OFF_CODE\" =\"TRO_OFF_CODE\" and \"SM_ID\" = ";
                        qry += " \"STO_SM_ID\" and \"SM_ID\" = '" + objSession.OfficeCode + "' GROUP BY \"TR_ID\",\"TR_NAME\"";
                        Genaral.Load_Combo(qry, "--Select--", cmbLocationName);
                    }
                    lblRepairerName.Text = "Repairer Name";
                    lblAddress.Text = "Repairer Address";

                    dvdivision.Visible = true;
                }

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// method to load next enumeration details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdNextDetails_Click(object sender, EventArgs e)
        {
            try
            {
                //if (btnApproval.Enabled == true || btnPending.Enabled == true || btnReject.Enabled == true)
                if (btnApproval.Enabled == true || btnReject.Enabled == true)
                {
                    ShowMsgBox("Please take Action for current Record/Details");
                    return;
                }

                GetEnumerationId();

                LoadEnumerationDetails(txtEnumDetailsId.Text);

                if (txtEnumType.Text == "2")
                {
                    cmbCircle_SelectedIndexChanged(sender, e);
                    cmbDivision.SelectedValue = hdfDivision.Value;
                    cmbDivision_SelectedIndexChanged(sender, e);
                    cmbsubdivision.SelectedValue = hdfSubdivision.Value;
                    cmbsubdivision_SelectedIndexChanged(sender, e);
                    cmbSection.SelectedValue = hdfSection.Value;
                    cmbFeeder.SelectedValue = hdfFeeder.Value;
                }
                else if (txtEnumType.Text == "1" || txtEnumType.Text == "5")
                {
                    cmbLocationType_SelectedIndexChanged(sender, e);
                    cmbLocationName.SelectedValue = hdfLocName.Value;
                }
                else if (txtEnumType.Text == "3")
                {
                    cmbLocationName.SelectedValue = hdfLocName.Value.Split('~').GetValue(0).ToString();
                }

                btnApproval.Enabled = true;
                //btnPending.Enabled = true;

                VisibilityEnumType();
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to get enumeration id
        /// </summary>
        public void GetEnumerationId()
        {
            try
            {
                // AllEnumID
                if (Session["AllEnumID"] != null && Session["AllEnumID"].ToString() != "")
                {
                    string sEnumerationId = Session["AllEnumID"].ToString();
                    Session["AllEnumID"] = null;
                    if (!sEnumerationId.StartsWith("~"))
                    {
                        sEnumerationId = "~" + sEnumerationId;
                    }
                    if (!sEnumerationId.EndsWith("~"))
                    {
                        sEnumerationId = sEnumerationId + "~";
                    }

                    string[] strarrEnum = sEnumerationId.Split('~');

                    string[] strDetailVal = strarrEnum.ToArray();
                    if (iIncrment > 1)
                    {

                        if (strDetailVal[iIncrment] != "")
                        {
                            txtEnumDetailsId.Text = strDetailVal[iIncrment].Split('`').GetValue(0).ToString();
                            txtEnumType.Text = strDetailVal[iIncrment].Split('`').GetValue(1).ToString();
                        }
                        iIncrment++;
                    }

                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// onchange method for rating
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbRating_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbRating.SelectedValue == "1")
                {
                    dvStar.Style.Add("display", "block");
                    Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'SRT'", "--Select--", cmbStarRated);
                }
                else
                {
                    dvStar.Style.Add("display", "none");
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// onchange method for make
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbMake_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbMake.SelectedValue == "1")
                {

                    if (txtTcCode.Text.Trim() == "")
                    {
                        ShowMsgBox("Enter DTr Code Number");
                        cmbMake.SelectedIndex = 0;
                        return;
                    }
                    txtTcslno.Enabled = false;
                    txtTcslno.Text = "NNP" + txtTcCode.Text;
                    Div1.Style.Add("display", "none");
                }
                else
                {
                    txtTcslno.Enabled = true;
                    txtTcslno.Text = "";
                    Div1.Style.Add("display", "block");
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// method to disable the buttons based on conditions
        /// </summary>
        /// <param name="sStatusFlag"></param>
        public void RestrictUpdate(string sStatusFlag)
        {
            try
            {
                if (sStatusFlag == "2" || sStatusFlag == "3")
                {
                    btnApproval.Enabled = false;
                    //btnPending.Enabled = false;
                    btnReject.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}