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
    public partial class NewDtcCommission : System.Web.UI.Page
    {
        string strFormCode = "NewDtcCommission";
        clsSession objSession;
        int Circle_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Circle_code"]);
        int Division_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Division_code"]);
        int SubDiv_code = Convert.ToInt32(ConfigurationSettings.AppSettings["SubDiv_code"]);
        int Section_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Section_code"]);
        int Feeder_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Feeder_code"]);
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

                txtCommisionDate.Attributes.Add("readonly", "readonly");
                txtServiceDate.Attributes.Add("readonly", "readonly");
                txtFeederChngDate.Attributes.Add("readonly", "readonly");
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

                lblMessage.Text = string.Empty;
                if (!IsPostBack)
                {
                    //Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" ORDER BY \"CM_CIRCLE_CODE\"", "--Select--", cmbCircle);

                    objSession = (clsSession)Session["clsSession"];
                    string stroffCode = string.Empty;

                    stroffCode = objSession.OfficeCode;
                    string stroffCode1 = stroffCode;
                    Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" Where \"CM_CIRCLE_CODE\"= '" + objSession.OfficeCode.Substring(0, Circle_code) + "'  ORDER BY \"CM_CIRCLE_CODE\"", "--Select--", cmbCircle);
                    stroffCode = stroffCode.Substring(0, Circle_code);
                    cmbCircle.Items.FindByValue(stroffCode).Selected = true;
                    cmbCircle.Enabled = false;

                    if (cmbCircle.SelectedIndex > 0)
                    {
                        Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\"  WHERE \"DIV_CODE\"='" + objSession.OfficeCode.Substring(0, Division_code) + "' AND \"DIV_CICLE_CODE\"='" + cmbCircle.SelectedValue + "' ORDER BY \"DIV_CODE\"", "--Select--", cmbDivision);
                    }
                    else
                    {
                        cmbDivision.Items.Clear();
                        cmbsubdivision.Items.Clear();
                        cmbSection.Items.Clear();
                        cmbFeeder.Items.Clear();
                    }
                    string strQry = string.Empty;

                    objSession = (clsSession)Session["clsSession"];

                    if (objSession.OfficeCode.Length <= 2 && objSession.OfficeCode.Length != 0)
                    {
                        stroffCode = clsStoreOffice.GetZone_Circle_Div_Offcode(objSession.OfficeCode, objSession.RoleId).Substring(0, Constants.Circle);
                    }
                    else
                    {
                        stroffCode = objSession.OfficeCode;
                    }

                    if (stroffCode.Length >= 2)
                    {
                        if (stroffCode.Length >= 3)
                        {
                            stroffCode = stroffCode.Substring(0, Division_code);
                            cmbDivision.Items.FindByValue(stroffCode).Selected = true;
                            cmbDivision.Enabled = false;



                            stroffCode = stroffCode1;
                        }
                    }

                    if (stroffCode.Length >= 2)
                    {


                        Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\", \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\"  WHERE \"SD_SUBDIV_CODE\"='" + objSession.OfficeCode.Substring(0, SubDiv_code) + "' AND \"SD_DIV_CODE\"='" + cmbDivision.SelectedValue + "' ORDER BY \"SD_SUBDIV_CODE\"", "--Select--", cmbsubdivision);



                        if (stroffCode.Length >= 3)
                        {
                            stroffCode = stroffCode.Substring(0, SubDiv_code);
                            cmbsubdivision.Items.FindByValue(stroffCode).Selected = true;
                            cmbsubdivision.Enabled = false;



                            stroffCode = stroffCode1;
                        }
                    }
                    if (stroffCode.Length >= 4)
                    {


                        //Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\", \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\"  WHERE \"SD_SUBDIV_CODE\"='" + objSession.OfficeCode.Substring(0, SubDiv_code) + "' AND \"SD_DIV_CODE\"='" + cmbDivision.SelectedValue + "' ORDER BY \"SD_SUBDIV_CODE\"", "--Select--", cmbsubdivision);
                        Genaral.Load_Combo("SELECT \"OM_CODE\", \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_CODE\"='" + objSession.OfficeCode.Substring(0, Section_code) + "' AND \"OM_SUBDIV_CODE\" = '" + cmbsubdivision.SelectedValue + "' ORDER BY \"OM_CODE\"", "--Select--", cmbSection);


                        if (stroffCode.Length >= 4)
                        {
                            stroffCode = stroffCode.Substring(0, Section_code);
                            cmbSection.Items.FindByValue(stroffCode).Selected = true;
                            cmbSection.Enabled = false;
                            stroffCode = stroffCode1;
                            strQry = "SELECT \"FD_FEEDER_CODE\",\"FD_FEEDER_CODE\" || '-' || \"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\", \"TBLFEEDEROFFCODE\" WHERE  \"FD_FEEDER_ID\" = \"FDO_FEEDER_ID\" AND";
                            strQry += " cast(\"FDO_OFFICE_CODE\" as text) LIKE '" + cmbSection.SelectedValue + "%' ORDER BY \"FD_FEEDER_CODE\"";
                            Genaral.Load_Combo(strQry, "--Select--", cmbFeeder);
                        }
                    }
                    Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'PT'", "--Select--", cmbprojecttype);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\"  WHERE \"DIV_CICLE_CODE\"='" + cmbCircle.SelectedValue + "' ORDER BY \"DIV_CODE\" ", "--Select--", cmbDivision);
                }
                else
                {
                    cmbDivision.Items.Clear();
                    cmbsubdivision.Items.Clear();
                    cmbSection.Items.Clear();
                    cmbFeeder.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbCircle_SelectedIndexChanged");
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDivision.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\", \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\"  WHERE \"SD_DIV_CODE\"='" + cmbDivision.SelectedValue + "' ORDER BY \"SD_SUBDIV_CODE\"", "--Select--", cmbsubdivision);
                }
                else
                {

                    cmbsubdivision.Items.Clear();
                    cmbSection.Items.Clear();
                    cmbFeeder.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbCircle_SelectedIndexChanged");
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbsubdivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbsubdivision.SelectedIndex > 0)
                {
                    string strQry = string.Empty;
                    Genaral.Load_Combo("SELECT \"OM_CODE\", \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_SUBDIV_CODE\" = '" + cmbsubdivision.SelectedValue + "' ORDER BY \"OM_CODE\"", "--Select--", cmbSection);
                  strQry = "SELECT \"FD_FEEDER_CODE\",\"FD_FEEDER_CODE\" || '-' || \"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\", \"TBLFEEDEROFFCODE\" WHERE  \"FD_FEEDER_ID\" = \"FDO_FEEDER_ID\" AND";
                    strQry += " CAST(\"FDO_OFFICE_CODE\" AS TEXT) LIKE '" + cmbsubdivision.SelectedValue + "%' ORDER BY \"FD_FEEDER_CODE\" ";
                    Genaral.Load_Combo(strQry, "--Select--", cmbFeeder);
                }
                else
                {

                    cmbSection.Items.Clear();
                    cmbFeeder.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbsubdivision_SelectedIndexChanged");
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbFeeder_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbFeeder.SelectedIndex > 0)
                {
                    clsFieldEnumeration ObjField = new clsFieldEnumeration();
                    ObjField.sFeederCode = cmbFeeder.SelectedValue;
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

                    ObjField.sClientIP = sClientIP;
                    ObjField.sCrBy = objSession.UserId;
                    //txtDTCCode.Text = ObjField.GeneratefeederCode(ObjField);
                    //txtTCCode.Text = ObjField.Generatetccode(ObjField);
                    txtDTCCode.ReadOnly = true;
                }
                else
                {

                    //cmbSection.Items.Clear();
                    //cmbFeeder.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbFeeder_SelectedIndexChanged");
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void cmdSave_Click(object sender, EventArgs e)
        {
            clsNewDtcCommission objDtcCommision = new clsNewDtcCommission();
          
            try
            {
                if (ValidateForm() == true)
                {
                    string[] Arr = new string[3];
                    
                    objDtcCommision.sDtcName = txtDTCName.Text;
                    objDtcCommision.iConnectedHP = Convert.ToString(txtConnectedHP.Text);
                    objDtcCommision.iConnectedKW = Convert.ToString(txtConnectedKW.Text);
                    objDtcCommision.sInternalCode = txtInternalCode.Text;
                    objDtcCommision.sFeederChangeDate = txtFeederChngDate.Text;
                    objDtcCommision.sServiceDate = txtServiceDate.Text;
                    objDtcCommision.sOMSectionName = cmbSection.SelectedValue;
                    objDtcCommision.sOfficeCode = cmbSection.SelectedValue;
                    objDtcCommision.sDtcCode = txtDTCCode.Text;
                    objDtcCommision.iKWHReading = txtKWHReading.Text;
                    objDtcCommision.sCommisionDate = txtCommisionDate.Text;
                    //objDtcCommision.sConnectionDate = txtConnectionDate.Text;
                    objDtcCommision.sTcCode = txtTCCode.Text;
                    objDtcCommision.sCrBy = objSession.UserId;
                    objDtcCommision.sSaveType = cmdSave.Text;
                    objDtcCommision.sFeederCode = cmbFeeder.SelectedValue;
                    objDtcCommision.sLatitude = string.Empty;
                    objDtcCommision.sLongitude = string.Empty;

                    objDtcCommision.sOldTcCode = txtOldTCCode.Text;

                    objDtcCommision.sWOslno = txtWOslno.Text;

                    string temp = Convert.ToString(fupDTCCode.FileName);
                    temp = Convert.ToString(fupDTCStructure.FileName);


                    if (cmbprojecttype.SelectedIndex > 0)
                    {
                        objDtcCommision.sProjecttype = cmbprojecttype.SelectedValue;
                    }
                    else
                    {
                        objDtcCommision.sProjecttype = "0";
                    }

                    objDtcCommision.sFormName = strFormCode;

                    Arr = objDtcCommision.SaveUpdateDtcDetails(objDtcCommision);

                    if (Arr[1].ToString() == "0")
                    {
                        bool bResult = SaveImagesPath(objDtcCommision);
                        txtDTCId.Text = objDtcCommision.lDtcId;
                        objDtcCommision.sCrBy = objSession.UserId;
                        cmdSave.Text = "Update";
                        cmbCircle.Enabled = false;
                        cmbDivision.Enabled = false;
                        cmbsubdivision.Enabled = false;
                        cmbSection.Enabled = false;
                        cmbFeeder.Enabled = false;
                        txtDTCCode.Enabled = false;
                        string strDtcId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtDTCId.Text));
                        if (Arr[1].ToString() == "0")
                        {
                            ShowMsgBox(Arr[0]);
                            return;
                        }

                        // Response.Redirect("DTCDetails.aspx?QryDtcId=" + strDtcId + "&Ref=" + sReference, false);

                        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID, "alert('Saved Successfully'); location.href='DTCDetails.aspx?QryDtcId=" + strDtcId + "';", true);
                    }
                    else if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0]);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdSave_Click");
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public bool SaveImagesPath(clsFieldEnumeration objFieldEnum)
        {
            try
            {
                //FTP Parameter
                string sFTPLink = string.Empty;
                string sFTPUserName = string.Empty;
                string sFTPPassword = string.Empty;

                // File Name Parameter
                string sDirectory = string.Empty;
                string sDTLMSCodeFileName = string.Empty;
                string sDTCFileName = string.Empty;

                // File Path Parameter

                string sSaveDTLMSCodeFilePath = string.Empty;
                string sSaveDTCFilePath = string.Empty;

                //FileType Parameter

                string sDTLMSCodePhotoExtension = string.Empty;
                string sDTCPhotoExtension = string.Empty;

                string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FileFormat"]);

                clsCommon objComm = new clsCommon();
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


                //clsFtp objFtp = new clsFtp(sFTPLink, sFTPUserName, sFTPPassword);
                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);
                clsSFTP objFtp = new clsSFTP(SFTPPath, sFTPUserName, sFTPPassword);

                bool Isuploaded;


                string sDTLMSFolderName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["DTLMSCodeFolder"].ToUpper());
                string sDTCFolderName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["DTCPhoto"].ToUpper());

                bool IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder+sFTPLink + objFieldEnum.sEnumDetailsID + "/");
                if (IsExists == false)
                {

                    objFtp.createDirectory(SFTPmainfolder+objFieldEnum.sEnumDetailsID);
                }

               


                if (txtDTCCodePath.Text.Trim() != "")
                {
                    sDTLMSCodePhotoExtension = System.IO.Path.GetExtension(txtDTCCodePath.Text).ToString().ToLower();
                    sDTLMSCodePhotoExtension = ";" + sDTLMSCodePhotoExtension.Remove(0, 1) + ";";


                    if (!sFileExt.Contains(sDTLMSCodePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sDTLMSCodeFileName = Path.GetFileName(txtDTCCodePath.Text);
                    sDirectory = txtDTCCodePath.Text;
                    objFieldEnum.sDTLMSCodePhotoPath = txtDTCCodePath.Text;
                }


              //  else // (fupDTCCode.PostedFile.ContentLength != 0)
                else if (fupDTCCode.PostedFile.ContentLength != 0)
                {

                    sDTLMSCodePhotoExtension = System.IO.Path.GetExtension(fupDTCCode.FileName).ToString().ToLower();
                    sDTLMSCodePhotoExtension = ";" + sDTLMSCodePhotoExtension.Remove(0, 1) + ";";


                    if (!sFileExt.Contains(sDTLMSCodePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sDTLMSCodeFileName = Path.GetFileName(fupDTCCode.PostedFile.FileName);

                    fupDTCCode.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTLMSCodeFileName));
                    sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTLMSCodeFileName);
                }


                if (sDTLMSCodeFileName != "")
                {
                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder+sFTPLink + objFieldEnum.sEnumDetailsID + "/" + sDTLMSFolderName + "/");
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(SFTPmainfolder+objFieldEnum.sEnumDetailsID + "/" + sDTLMSFolderName);
                        }

                        Isuploaded = objFtp.upload(SFTPmainfolder+objFieldEnum.sEnumDetailsID + "/" + sDTLMSFolderName , sDTLMSCodeFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objFieldEnum.sDTCPhotoPath = objFieldEnum.sEnumDetailsID + "/" + sDTLMSFolderName + "/" + sDTLMSCodeFileName;
                            txtDTCCodePath.Text = objFieldEnum.sDTCPhotoPath;
                        }
                    }

                }



                if (txtDTCStructurePath.Text.Trim() != "")
                {
                    sDTCPhotoExtension = System.IO.Path.GetExtension(txtDTCStructurePath.Text).ToString().ToLower();
                    sDTCPhotoExtension = ";" + sDTCPhotoExtension.Remove(0, 1) + ";";


                    if (!sFileExt.Contains(sDTCPhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sDTCFileName = Path.GetFileName(txtDTCStructurePath.Text);
                    sDirectory = txtDTCStructurePath.Text;
                    objFieldEnum.sDTCPhotoPath = txtDTCStructurePath.Text;
                }
                else if (fupDTCStructure.PostedFile.ContentLength != 0)
                {

                    sDTCPhotoExtension = System.IO.Path.GetExtension(fupDTCStructure.FileName).ToString().ToLower();
                    sDTCPhotoExtension = ";" + sDTCPhotoExtension.Remove(0, 1) + ";";


                    if (!sFileExt.Contains(sDTCPhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sDTCFileName = Path.GetFileName(fupDTCStructure.PostedFile.FileName);

                    fupDTCStructure.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTCFileName));
                    sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTCFileName);
                }



                if (sDTCFileName != "")
                {
                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder+sFTPLink + objFieldEnum.sEnumDetailsID + "/" + sDTCFolderName + "/");
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(SFTPmainfolder+objFieldEnum.sEnumDetailsID + "/" + sDTCFolderName);
                        }

                        Isuploaded = objFtp.upload(SFTPmainfolder+objFieldEnum.sEnumDetailsID + "/" + sDTCFolderName , sDTCFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objFieldEnum.sDTLMSCodePhotoPath = objFieldEnum.sEnumDetailsID + "/" + sDTCFolderName + "/" + sDTCFileName;
                            txtDTCStructurePath.Text = objFieldEnum.sDTLMSCodePhotoPath;
                        }
                    }

                }

                bool bResult;

               // clsException.SaveFunctionExecLog("SaveImagePathDetails --- START");
                objFieldEnum.sCrBy = objSession.UserId;
                //if (txtEnumDetailsId.Text.Trim() == "")
                //{
                   
                //    bResult = objFieldEnum.SaveImagePathDetails(objFieldEnum);
                //}
                //else
                //{
                //    bResult = objFieldEnum.UpdateImagePathDetails(objFieldEnum);
                //}
                bResult = objFieldEnum.UpdateImagePathDetails(objFieldEnum);


                // clsException.SaveFunctionExecLog("SaveImagePathDetails --- END");
                // clsException.SaveFunctionExecLog("ENDImageSaveFunction --- END");
                return bResult;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveImagesPath");
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
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
                    ShowMsgBox("Enter 6 digit DTC Code");
                    return bValidate;

                }
                if (txtDTCCode.Text.Trim().Length == 0)
                {
                    txtDTCCode.Focus();
                    ShowMsgBox("Enter DTC Code");
                    return bValidate;
                }
                if (txtDTCName.Text.Trim().Length == 0)
                {
                    txtDTCName.Focus();
                    ShowMsgBox("Enter DTC Name");
                    return bValidate;
                }

                if (cmbCircle.SelectedIndex != 0)
                {
                    if (cmbDivision.SelectedIndex != 0)
                    {
                        if (cmbsubdivision.SelectedIndex != 0)
                        {
                            if (cmbSection.SelectedIndex != 0)
                            {
                                if (cmbFeeder.SelectedIndex != 0)
                                {

                                }
                                else
                                {
                                    cmbFeeder.Focus();
                                    ShowMsgBox("Select Feeder");
                                    return bValidate;
                                }
                            }
                            else
                            {
                                cmbSection.Focus();
                                ShowMsgBox("Select Section");
                                return bValidate;
                            }
                        }
                        else
                        {
                            cmbsubdivision.Focus();
                            ShowMsgBox("Select SubDivision");
                            return bValidate;
                        }
                    }
                    else
                    {
                        cmbDivision.Focus();
                        ShowMsgBox("Select Division");
                        return bValidate;
                    }
                }
                else
                {
                    cmbCircle.Focus();
                    ShowMsgBox("Select Circle");
                    return bValidate;
                }
                string FeederCode =  txtDTCCode.Text.Substring(0, 6);
                //string SelectedFeederCode= cmbFeeder.SelectedValue.ToString();
                //if (FeederCode != SelectedFeederCode)
                //{
                //    ShowMsgBox("Code Does Not Match With The Feeder Code");
                //}
                
                if (txtConnectedKW.Text.Trim() != "")
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtConnectedKW.Text, "^(\\d{1,4})?(\\.\\d{1,2})?$"))
                    {
                        ShowMsgBox("Enter valid Connected KW (eg:1234.11)");
                        return false;
                    }

                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtConnectedKW.Text, "[-+]?[0-9]{0,3}\\.?[0-9]{1,2}"))
                    {
                        ShowMsgBox("Enter valid Connected KW (eg:1234.11)");
                        return false;
                    }
                }
                
                if (txtConnectedHP.Text.Trim() != "")
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtConnectedHP.Text, "^(\\d{1,4})?(\\.\\d{1,2})?$"))
                    {
                        ShowMsgBox("Enter valid Connected HP (eg:1234.11)");
                        return false;
                    }

                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtConnectedHP.Text, "[-+]?[0-9]{0,3}\\.?[0-9]{1,2}"))
                    {
                        ShowMsgBox("Enter valid Connected HP (eg:1234.11)");
                        return false;
                    }
                }

                if (txtKWHReading.Text.Trim() != "")
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtKWHReading.Text, "^(\\d{1,6})?(\\.\\d{1,2})?$"))
                    {
                        ShowMsgBox("Enter Valid KWH Reading (eg:1234.11)");
                        return false;
                    }

                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtKWHReading.Text, "[-+]?[0-9]{0,5}\\.?[0-9]{0,2}"))
                    {
                        ShowMsgBox("Enter Valid KWH Reading (eg:1234.11)");
                        return false;
                    }
                }
                //if (txtTCCode.Text.Trim().Length == 0)
                //{
                //    txtTCCode.Focus();
                //    ShowMsgBox("Enter Valid TC Code");
                //    return bValidate;
                //}
                

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
                // image validation  
                if (fupDTCCode.PostedFile.ContentLength == 0 && txtDTCCodePath.Text.Trim() == "")
                {
                    fupDTCCode.Focus();
                    ShowMsgBox("Select DTC Photo to Upload");
                    return false;
                }


                if (fupDTCStructure.PostedFile.ContentLength == 0 && txtDTCStructurePath.Text.Trim() == "")
                {
                    fupDTCStructure.Focus();
                    ShowMsgBox("Select DTC Structure Photo to Upload");
                    return false;
                }

              

                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdSave_Click");
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
                //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowMsgBox");
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtDTCCode.Text = string.Empty;
                txtDTCName.Text = string.Empty;
                txtInternalCode.Text = string.Empty;
                txtConnectedKW.Text = string.Empty;
                txtCommisionDate.Text = string.Empty;
                txtServiceDate.Text = string.Empty;
                cmbprojecttype.SelectedIndex = 0;
                txtFeederChngDate.Text = string.Empty;
                txtKWHReading.Text = string.Empty;
                txtConnectedHP.Text = string.Empty;

                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdReset_Click");
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}