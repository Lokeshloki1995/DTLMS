using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.IO;
using System.Configuration;

namespace IIITS.DTLMS.DTCFailure
{
    public partial class NewDTCCR : System.Web.UI.Page
    {
        string strFormCode = "NewDTCCR";
        clsSession objSession;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string sDTCId = string.Empty;
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                else
                {
                    objSession = (clsSession)Session["clsSession"];
                    txtCRDate.Attributes.Add("readonly", "readonly");
                    CalendarExtender1.EndDate = System.DateTime.Now;

                    RetainImageinPostbackSession();
                    RetainImageOnPostback();

                    if (!IsPostBack)
                    {
                        if(Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["DtcId"]))!=null && Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["DtcId"]))!= "")
                        {
                            sDTCId = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["DtcId"]));
                        }
                        getNewDTCDetails(sDTCId);

                        //WorkFlow / Approval
                        WorkFlowConfig();
                    }
                }
                
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public string ValidateImages()
        {
            //   clsException.SaveFunctionExecLog("ValidateImages --- START",Session["FullName"].ToString(), "2");
            string svalidate = string.Empty;
            try
            {
                //FileType Parameter
                string sPlatePhotoExtension = string.Empty;
                string sSSPlatePhotoExtension = string.Empty;
                string sOldCodePhotoExtension = string.Empty;
                string sIPEnumCodePhotoExtension = string.Empty;
                string sDTLMSCodePhotoExtension = string.Empty;
                string sDTCPhotoExtension = string.Empty;
                string sInfosysCodePhotoExtension = string.Empty;

                //  Photo Save DTLMSDocs
                string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FileFormat"]);

                //Name Plate Photo
                if (hdfNamePlatePath.Value.Trim() != "")
                {
                    sPlatePhotoExtension = System.IO.Path.GetExtension(hdfNamePlatePath.Value).ToString().ToLower();
                    sPlatePhotoExtension = ";" + sPlatePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sPlatePhotoExtension))
                    {
                        //ShowMsgBox("Invalid Image Format");
                        return "Invalid Image Format in Name Plate Photo";
                    }

                    //string sFileName = Path.GetFileName(txtNamePlatePhotoPath.Text);
                    //if (File.Exists(txtNamePlatePhotoPath.Text))
                    //{
                    //    return sFileName + " File Already Exists";
                    //}

                }
                else if (fupNamePlate.PostedFile.ContentLength != 0)
                {
                    sPlatePhotoExtension = System.IO.Path.GetExtension(fupNamePlate.FileName).ToString().ToLower();
                    sPlatePhotoExtension = ";" + sPlatePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sPlatePhotoExtension))
                    {
                        //ShowMsgBox("Invalid Image Format");
                        return "Invalid Image Format in Name Plate Photo";
                    }


                }

                if (hdfSSPlatePath.Value.Trim() != "")
                {
                    sSSPlatePhotoExtension = System.IO.Path.GetExtension(hdfSSPlatePath.Value).ToString().ToLower();
                    sSSPlatePhotoExtension = ";" + sSSPlatePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sSSPlatePhotoExtension))
                    {
                        //ShowMsgBox("Invalid Image Format");
                        return "Invalid Image Format in SS plate photo";
                    }

                }

                else if (fupSSPlate.PostedFile.ContentLength != 0)
                {

                    sSSPlatePhotoExtension = System.IO.Path.GetExtension(fupSSPlate.FileName).ToString().ToLower();
                    sSSPlatePhotoExtension = ";" + sSSPlatePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sSSPlatePhotoExtension))
                    {
                        //ShowMsgBox("Invalid Image Format");
                        return "Invalid Image Format in SS plate photo";
                    }
                }

                // Old Code Photo Save

                if (hdfDTCPoto2Path.Value.Trim() != "")
                {
                    sOldCodePhotoExtension = System.IO.Path.GetExtension(hdfDTCPoto2Path.Value).ToString().ToLower();
                    sOldCodePhotoExtension = ";" + sOldCodePhotoExtension.Remove(0, 1) + ";";


                    if (!sFileExt.Contains(sOldCodePhotoExtension))
                    {
                        //ShowMsgBox("Invalid Image Format");
                        return "Invalid Image Format in OLD DTC Code(BESCOM) Photo";
                    }

                }

                else if (fupDTCCODE2.PostedFile.ContentLength != 0)
                {

                    sOldCodePhotoExtension = System.IO.Path.GetExtension(fupDTCCODE2.FileName).ToString().ToLower();
                    sOldCodePhotoExtension = ";" + sOldCodePhotoExtension.Remove(0, 1) + ";";


                    if (!sFileExt.Contains(sOldCodePhotoExtension))
                    {
                        //ShowMsgBox("Invalid Image Format");
                        return "Invalid Image Format in OLD DTC Code(BESCOM) Photo";
                    }

                }

                // IP Enum Code Photo Save
                if (hdfDTCPoto1Path.Value.Trim() != "")
                {
                    sIPEnumCodePhotoExtension = System.IO.Path.GetExtension(hdfDTCPoto1Path.Value).ToString().ToLower();
                    sIPEnumCodePhotoExtension = ";" + sIPEnumCodePhotoExtension.Remove(0, 1) + ";";


                    if (!sFileExt.Contains(sIPEnumCodePhotoExtension))
                    {
                        //ShowMsgBox("Invalid Image Format");
                        return "Invalid Image Format in  DTC Code(IP Enum) Photo";
                    }

                }
                else if (fupIpEnum.PostedFile.ContentLength != 0)
                {

                    sIPEnumCodePhotoExtension = System.IO.Path.GetExtension(fupIpEnum.FileName).ToString().ToLower();
                    sIPEnumCodePhotoExtension = ";" + sIPEnumCodePhotoExtension.Remove(0, 1) + ";";


                    if (!sFileExt.Contains(sIPEnumCodePhotoExtension))
                    {
                        //ShowMsgBox("Invalid Image Format");
                        return "Invalid Image Format in DTC Code(IP Enum) Photo";
                    }

                }

                // DTLMS Code Photo Save

                if (hdfDTLMSCODEPath.Value.Trim() != "")
                {
                    sDTLMSCodePhotoExtension = System.IO.Path.GetExtension(hdfDTLMSCODEPath.Value).ToString().ToLower();
                    sDTLMSCodePhotoExtension = ";" + sDTLMSCodePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sDTLMSCodePhotoExtension))
                    {
                        // ShowMsgBox("Invalid Image Format");
                        return "Invalid Image Format  in DTC Code(DTLMS) Photo";
                    }
                }


                else if (fupDTLMSCODE.PostedFile.ContentLength != 0)
                {

                    sDTLMSCodePhotoExtension = System.IO.Path.GetExtension(fupDTLMSCODE.FileName).ToString().ToLower();
                    sDTLMSCodePhotoExtension = ";" + sDTLMSCodePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sDTLMSCodePhotoExtension))
                    {
                        //ShowMsgBox("Invalid Image Format");
                        return "Invalid Image Format in DTC Code(DTLMS) Photo";
                    }


                }

                // DTC Photo Save
                if (hdfDTCPoto1Path.Value.Trim() != "")
                {
                    sDTCPhotoExtension = System.IO.Path.GetExtension(hdfDTCPoto1Path.Value).ToString().ToLower();
                    sDTCPhotoExtension = ";" + sDTCPhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sDTCPhotoExtension))
                    {
                        //ShowMsgBox("Invalid Image Format");
                        return "Invalid Image Format in DTC Photo";
                    }

                }
                else if (fupDTCCODE1.PostedFile.ContentLength != 0)
                {

                    sDTCPhotoExtension = System.IO.Path.GetExtension(fupDTCCODE1.FileName).ToString().ToLower();
                    sDTCPhotoExtension = ";" + sDTCPhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sDTCPhotoExtension))
                    {
                        //ShowMsgBox("Invalid Image Format");
                        return "Invalid Image Format in DTC Photo";
                    }

                }


                // Infosys Photo Save
                if (hdfInfosysAssetIdPath.Value.Trim() != "")
                {
                    sInfosysCodePhotoExtension = System.IO.Path.GetExtension(hdfInfosysAssetIdPath.Value).ToString().ToLower();
                    sInfosysCodePhotoExtension = ";" + sInfosysCodePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sInfosysCodePhotoExtension))
                    {
                        //ShowMsgBox("Invalid Image Format");
                        return "Invalid Image Format in Infosys Asset ID photo";
                    }

                }
                //else if (fupInfosysId.PostedFile.ContentLength != 0)
                //{
                //    sInfosysCodePhotoExtension = System.IO.Path.GetExtension(fupInfosysId.FileName).ToString().ToLower();
                //    sInfosysCodePhotoExtension = ";" + sInfosysCodePhotoExtension.Remove(0, 1) + ";";

                //    if (!sFileExt.Contains(sInfosysCodePhotoExtension))
                //    {
                //        //ShowMsgBox("Invalid Image Format");
                //        return "Invalid Image Format in Infosys Asset ID photo";
                //    }

                //}

                //       clsException.SaveFunctionExecLog("ValidateImages --- END",Session["FullName"].ToString(), "2");

                return "";
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ValidateImages");
                return ex.Message;
            }
        }

        public void ShowUploadedImages()
        {
            try
            {
                //     clsException.SaveFunctionExecLog("ShowUploadedImages --- START",Session["FullName"].ToString(), "2");

                clsCommon objComm = new clsCommon();

                //FTP Parameter
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

                sFTPLink = ConfigurationSettings.AppSettings["VirtualDirectoryPath"].ToString();
                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);

                //clsFtp objFtp = new clsFtp(sFTPLink, sFTPUserName, sFTPPassword);
                clsSFTP objFtp = new clsSFTP(SFTPPath, sFTPUserName, sFTPPassword);


                if (hdfNamePlatePath.Value != "")
                {
                    dvNamePlate.Style.Add("display", "block");
                    dvfupNamePlate.Style.Add("display", "none");
                    //imgNamePlate.ImageUrl = "ftp://" + sFTPUserName + ":" + sFTPPassword + "@" + sFTPLink.Remove(0, 6) + txtNamePlatePhotoPath.Text;
                    imgNamePlate.ImageUrl = sFTPLink + hdfNamePlatePath.Value;
                    //imgNamePlate.ImageUrl = "~/img/Bescom/BESCOM.png";
                }
                if (hdfSSPlatePath.Value != "")
                {
                    dvSSplate.Style.Add("display", "block");
                    dvfupSSplate.Style.Add("display", "none");
                    //imgSSPlate.ImageUrl = "ftp://" + sFTPUserName + ":" + sFTPPassword + "@" + sFTPLink.Remove(0, 6) + txtSSPlatePath.Text;
                    imgSSplate.ImageUrl = sFTPLink + hdfSSPlatePath.Value;
                }

                if (hdfDTCPoto2Path.Value != "")
                {
                    dvDTCCode2.Style.Add("display", "block");
                    dvfupDTC2.Style.Add("display", "none");
                    //imgOldCode.ImageUrl = "ftp://" + sFTPUserName + ":" + sFTPPassword + "@" + sFTPLink.Remove(0, 6) + txtOLDDTCPath.Text;
                    imgDTCCode2.ImageUrl = sFTPLink + hdfDTCPoto2Path.Value;
                }
                if (hdfDTCPoto1Path.Value != "")
                {
                    dvDTCpoto1.Style.Add("display", "block");
                    dvfupDTC1.Style.Add("display", "none");
                    //imgDTLMS.ImageUrl = "ftp://" + sFTPUserName + ":" + sFTPPassword + "@" + sFTPLink.Remove(0, 6) + txtDTLMSDTCPath.Text;
                    imgDTCpoto1.ImageUrl = sFTPLink + hdfDTCPoto1Path.Value;
                }
                //if (hdfInfosysAssetIdPath.Value != "")
                //{
                //    dvInfosysId.Style.Add("display", "block");
                //    dvfupInfosys.Style.Add("display", "none");
                //    //imgIPEnum.ImageUrl = "ftp://" + sFTPUserName + ":" + sFTPPassword + "@" + sFTPLink.Remove(0, 6) + txtIPDTCPath.Text;
                //    imgInfosys.ImageUrl = sFTPLink + hdfInfosysAssetIdPath.Value;
                //}
                if (hdfIpEnumarationPath.Value != "")
                {
                    dvIPEnum.Style.Add("display", "block");
                    dvfupEnum.Style.Add("display", "none");
                    //imgInfosys.ImageUrl = "ftp://" + sFTPUserName + ":" + sFTPPassword + "@" + sFTPLink.Remove(0, 6) + txtInfosysPath.Text;
                    imgIPEnum.ImageUrl = sFTPLink + hdfIpEnumarationPath.Value;
                }
                if (hdfDTLMSCODEPath.Value != "")
                {
                    dvDTLMSCode.Style.Add("display", "block");
                    dvfupDTLMS.Style.Add("display", "none");


                    //imgDTCPhoto.ImageUrl = "ftp://" + sFTPUserName + ":" + sFTPPassword + "@" + sFTPLink.Remove(0, 6) + txtDTCPath.Text;
                    imgDTLMSCode.ImageUrl = sFTPLink + hdfDTLMSCODEPath.Value;
                }

                //    clsException.SaveFunctionExecLog("ShowUploadedImages --- END",Session["FullName"].ToString(), "2");

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowUploadedImages");

            }
        }

        protected void cmdClose_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("/Approval/ApprovalInbox.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        //public void ShowUploadedImages()
        //{
        //    try
        //    {
        //        clsCommon objComm = new clsCommon();

        //        //FTP Parameter
        //        string sFTPLink = string.Empty;
        //        string sFTPUserName = string.Empty;
        //        string sFTPPassword = string.Empty;

        //        // To bind the Images from Ftp Path to Image Control

        //        System.Data.DataTable dt = objComm.GetAppSettings();
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPLINK")
        //            {
        //                sFTPLink = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
        //            }
        //            else if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPUSERNAME")
        //            {
        //                sFTPUserName = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
        //            }
        //            else if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPPASSWORD")
        //            {
        //                sFTPPassword = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
        //            }
        //        }

        //        sFTPLink = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["VirtualDirectoryPath"]);
        //        clsFtp objFtp = new clsFtp(sFTPLink, sFTPUserName, sFTPPassword);

        //        if (hdfDTCImagePath.Value != "")
        //        {
        //            dvDTLMSCode.Style.Add("display", "block");
        //            //imgDTCCode.ImageUrl = "ftp://" + sFTPUserName + ":" + sFTPPassword + "@" + sFTPLink.Remove(0, 6) + hdfDTCImagePath.Value;
        //            imgDTLMSCode.ImageUrl = sFTPLink + hdfDTCImagePath.Value;
        //        }
        //        if (hdfDTRImagePath.Value != "")
        //        {
        //            dvDTCpoto1.Style.Add("display", "block");
        //            //imgDTrCode.ImageUrl = "ftp://" + sFTPUserName + ":" + sFTPPassword + "@" + sFTPLink.Remove(0, 6) + hdfDTRImagePath.Value;
        //            imgDTCpoto1.ImageUrl = sFTPLink + hdfDTRImagePath.Value;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

        //    }
        //}
        public bool SaveImagesPath(clsNewDTCCR objnewDTC)
        {
            try
            {

                //FTP Parameter
                string sFTPLink = string.Empty;
                string sFTPUserName = string.Empty;
                string sFTPPassword = string.Empty;

                // File Name Parameter
                string sDirectory = string.Empty;
                string sPlateFileName = string.Empty;
                string sSSPlateFileName = string.Empty;
                string sOldCodeFileName = string.Empty;
                string sIPEnumCodeFileName = string.Empty;
                string sDTLMSCodeFileName = string.Empty;
                string sDTCFileName = string.Empty;
                string sInfosysCodeFileName = string.Empty;

                // File Path Parameter
                string sSavePlateFilePath = string.Empty;
                string sSaveSSPlateFilePath = string.Empty;
                string sSaveOldCodeFilePath = string.Empty;
                string sSaveIPEnumCodeFilePath = string.Empty;
                string sSaveDTLMSCodeFilePath = string.Empty;
                string sSaveDTCFilePath = string.Empty;
                string sSaveInfosysCodeFilePath = string.Empty;

                //FileType Parameter
                string sPlatePhotoExtension = string.Empty;
                string sSSPlatePhotoExtension = string.Empty;
                string sOldCodePhotoExtension = string.Empty;
                string sIPEnumCodePhotoExtension = string.Empty;
                string sDTLMSCodePhotoExtension = string.Empty;
                string sDTCPhotoExtension = string.Empty;
                string sInfosysCodePhotoExtension = string.Empty;

                //  clsException.SaveFunctionExecLog("SaveImagesPath --- START", Session["FullName"].ToString(), "2");

                //  Photo Save DTLMSDocs
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

                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder1"]);

                //clsFtp objFtp = new clsFtp(sFTPLink, sFTPUserName, sFTPPassword);
                clsSFTP objFtp = new clsSFTP(SFTPPath, sFTPUserName, sFTPPassword);

                bool Isuploaded;

                string sNamePlateFolderName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["NamePlateFolder"].ToUpper());
                string sSSPlateFolderName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SSPlateFolder"].ToUpper());
                string sOldCodeFolderName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["OldCodeFolder"].ToUpper());
                string sIPEnumFolderName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["IPEnumCodeFolder"].ToUpper());
                string sDTLMSFolderName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["DTLMSCodeFolder"].ToUpper());
                string sDTCFolderName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["DTCPhoto"].ToUpper());
                string sInfosysFolderName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["InfosysCodeFolder"].ToUpper());

                // Create Directory

                bool IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + objnewDTC.sEnumDetailsID + "/");
                if (IsExists == false)
                {

                    objFtp.createDirectory(SFTPmainfolder + objnewDTC.sEnumDetailsID);
                }


                // Name Plate Photo Save
                if (hdfNamePlatePath.Value.Trim() != "")
                {
                    sPlatePhotoExtension = System.IO.Path.GetExtension(hdfNamePlatePath.Value).ToString().ToLower();
                    sPlatePhotoExtension = ";" + sPlatePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sPlatePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sPlateFileName = Path.GetFileName(hdfNamePlatePath.Value);
                    sDirectory = hdfNamePlatePath.Value;
                    objnewDTC.sNamePlatePhotoPath = hdfNamePlatePath.Value;
                }
                else if (fupNamePlate.PostedFile.ContentLength != 0)
                {

                    sPlatePhotoExtension = System.IO.Path.GetExtension(fupNamePlate.FileName).ToString().ToLower();
                    sPlatePhotoExtension = ";" + sPlatePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sPlatePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sPlateFileName = Path.GetFileName(fupNamePlate.PostedFile.FileName);

                    fupNamePlate.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sPlateFileName));
                    sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sPlateFileName);

                }
                if (sPlateFileName != "")
                {
                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + objnewDTC.sEnumDetailsID + "/" + sNamePlateFolderName + "/");
                        if (IsExists == false)
                        {
                            objFtp.createDirectory(SFTPmainfolder + objnewDTC.sEnumDetailsID + "/" + sNamePlateFolderName);
                        }

                        Isuploaded = objFtp.upload(SFTPmainfolder + objnewDTC.sEnumDetailsID + "/" + sNamePlateFolderName + "/", sPlateFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objnewDTC.sNamePlatePhotoPath = objnewDTC.sEnumDetailsID + "/" + sNamePlateFolderName + "/" + sPlateFileName;
                            hdfNamePlatePath.Value = SFTPmainfolder + objnewDTC.sNamePlatePhotoPath;
                            Session["fupNamePlate"] = null;

                        }
                    }
                }


                // SS Plate Photo Save

                if (hdfSSPlatePath.Value.Trim() != "")
                {
                    sSSPlatePhotoExtension = System.IO.Path.GetExtension(hdfSSPlatePath.Value).ToString().ToLower();
                    sSSPlatePhotoExtension = ";" + sSSPlatePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sSSPlatePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sSSPlateFileName = Path.GetFileName(hdfSSPlatePath.Value);
                    sDirectory = hdfSSPlatePath.Value;
                    objnewDTC.sSSPlatePhotoPath = hdfSSPlatePath.Value;
                }

                else if (fupSSPlate.PostedFile.ContentLength != 0)
                {

                    sSSPlatePhotoExtension = System.IO.Path.GetExtension(fupSSPlate.FileName).ToString().ToLower();
                    sSSPlatePhotoExtension = ";" + sSSPlatePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sSSPlatePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sSSPlateFileName = Path.GetFileName(fupSSPlate.PostedFile.FileName);

                    fupSSPlate.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sSSPlateFileName));
                    sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sSSPlateFileName);

                }
                if (sSSPlateFileName != "")
                {
                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + objnewDTC.sEnumDetailsID + "/" + sSSPlateFolderName + "/");
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(SFTPmainfolder + objnewDTC.sEnumDetailsID + "/" + sSSPlateFolderName);
                        }

                        Isuploaded = objFtp.upload(SFTPmainfolder + objnewDTC.sEnumDetailsID + "/" + sSSPlateFolderName + "/", sSSPlateFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objnewDTC.sSSPlatePhotoPath =  objnewDTC.sEnumDetailsID + "/" + sSSPlateFolderName + "/" + sSSPlateFileName;
                            hdfSSPlatePath.Value = SFTPmainfolder + objnewDTC.sSSPlatePhotoPath;
                            Session["fupSSPlate"] = null;
                        }
                    }
                }



                // IP Enum Code Photo Save
                if (hdfIpEnumarationPath.Value.Trim() != "")
                {
                    sIPEnumCodePhotoExtension = System.IO.Path.GetExtension(hdfIpEnumarationPath.Value).ToString().ToLower();
                    sIPEnumCodePhotoExtension = ";" + sIPEnumCodePhotoExtension.Remove(0, 1) + ";";


                    if (!sFileExt.Contains(sIPEnumCodePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sIPEnumCodeFileName = Path.GetFileName(hdfIpEnumarationPath.Value);
                    sDirectory = hdfIpEnumarationPath.Value;
                    objnewDTC.sIPEnumCodePhotoPath = hdfIpEnumarationPath.Value;
                }
                else if (fupIpEnum.PostedFile.ContentLength != 0)
                {

                    sIPEnumCodePhotoExtension = System.IO.Path.GetExtension(fupIpEnum.FileName).ToString().ToLower();
                    sIPEnumCodePhotoExtension = ";" + sIPEnumCodePhotoExtension.Remove(0, 1) + ";";


                    if (!sFileExt.Contains(sIPEnumCodePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sIPEnumCodeFileName = Path.GetFileName(fupIpEnum.PostedFile.FileName);

                    fupIpEnum.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sIPEnumCodeFileName));
                    sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sIPEnumCodeFileName);
                }
                if (sIPEnumCodeFileName != "")
                {
                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + objnewDTC.sEnumDetailsID + "/" + sIPEnumFolderName + "/");
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(SFTPmainfolder + objnewDTC.sEnumDetailsID + "/" + sIPEnumFolderName);
                        }

                        Isuploaded = objFtp.upload(SFTPmainfolder + objnewDTC.sEnumDetailsID + "/" + sIPEnumFolderName + "/", sIPEnumCodeFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objnewDTC.sIPEnumCodePhotoPath =  objnewDTC.sEnumDetailsID + "/" + sIPEnumFolderName + "/" + sIPEnumCodeFileName;
                            hdfIpEnumarationPath.Value = SFTPmainfolder + objnewDTC.sIPEnumCodePhotoPath;
                            Session["fupIPEnum"] = null;
                        }
                    }
                }

                // DTLMS Code Photo Save

                if (hdfDTLMSCODEPath.Value.Trim() != "")
                {
                    sDTLMSCodePhotoExtension = System.IO.Path.GetExtension(hdfDTLMSCODEPath.Value).ToString().ToLower();
                    sDTLMSCodePhotoExtension = ";" + sDTLMSCodePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sDTLMSCodePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sDTLMSCodeFileName = Path.GetFileName(hdfDTLMSCODEPath.Value);
                    sDirectory = hdfDTLMSCODEPath.Value;
                    objnewDTC.sDTLMSCodePhotoPath = hdfDTLMSCODEPath.Value;
                }

                else if (fupDTLMSCODE.PostedFile.ContentLength != 0)
                {

                    sDTLMSCodePhotoExtension = System.IO.Path.GetExtension(fupDTLMSCODE.FileName).ToString().ToLower();
                    sDTLMSCodePhotoExtension = ";" + sDTLMSCodePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sDTLMSCodePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sDTLMSCodeFileName = Path.GetFileName(fupDTLMSCODE.PostedFile.FileName);

                    fupDTLMSCODE.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTLMSCodeFileName));
                    sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTLMSCodeFileName);

                }
                if (sDTLMSCodeFileName != "")
                {
                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + objnewDTC.sEnumDetailsID + "/" + sDTLMSFolderName + "/");
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(SFTPmainfolder + objnewDTC.sEnumDetailsID + "/" + sDTLMSFolderName);
                        }

                        Isuploaded = objFtp.upload(SFTPmainfolder + objnewDTC.sEnumDetailsID + "/" + sDTLMSFolderName + "/", sDTLMSCodeFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objnewDTC.sDTLMSCodePhotoPath =  objnewDTC.sEnumDetailsID + "/" + sDTLMSFolderName + "/" + sDTLMSCodeFileName;
                            hdfDTLMSCODEPath.Value = SFTPmainfolder + objnewDTC.sDTLMSCodePhotoPath;
                            Session["fupDTLMSCodePhoto"] = null;
                        }
                    }
                }


                // DTC1 Photo Save
                if (hdfDTCPoto1Path.Value.Trim() != "")
                {
                    sDTCPhotoExtension = System.IO.Path.GetExtension(hdfDTCPoto1Path.Value).ToString().ToLower();
                    sDTCPhotoExtension = ";" + sDTCPhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sDTCPhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sDTCFileName = Path.GetFileName(hdfDTCPoto1Path.Value);
                    sDirectory = hdfDTCPoto1Path.Value;
                    objnewDTC.sDTCPhoto1Path = hdfDTCPoto1Path.Value;
                }
                else if (fupDTCCODE1.PostedFile.ContentLength != 0)
                {

                    sDTCPhotoExtension = System.IO.Path.GetExtension(fupDTCCODE1.FileName).ToString().ToLower();
                    sDTCPhotoExtension = ";" + sDTCPhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sDTCPhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sDTCFileName = Path.GetFileName(fupDTCCODE1.PostedFile.FileName);


                    fupDTCCODE1.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTCFileName));
                    sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTCFileName);
                }
                if (sDTCFileName != "")
                {
                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + objnewDTC.sEnumDetailsID + "/" + sDTCFolderName + "/");
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(SFTPmainfolder + objnewDTC.sEnumDetailsID + "/" + sDTCFolderName);
                        }

                        Isuploaded = objFtp.upload(SFTPmainfolder + objnewDTC.sEnumDetailsID + "/" + sDTCFolderName + "/", sDTCFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objnewDTC.sDTCPhoto1Path = objnewDTC.sEnumDetailsID + "/" + sDTCFolderName + "/" + sDTCFileName;
                            hdfDTCPoto1Path.Value = SFTPmainfolder + objnewDTC.sDTCPhoto1Path;
                            Session["fupDTCPhoto"] = null;
                      
                        }
                    }
                }
                // DTC2 Photo Save
                if (hdfDTCPoto2Path.Value.Trim() != "")
                {
                    sDTCPhotoExtension = System.IO.Path.GetExtension(hdfDTCPoto2Path.Value).ToString().ToLower();
                    sDTCPhotoExtension = ";" + sDTCPhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sDTCPhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sDTCFileName = Path.GetFileName(hdfDTCPoto2Path.Value);
                    sDirectory = hdfDTCPoto2Path.Value;
                    objnewDTC.sDTCPhoto2Path = hdfDTCPoto2Path.Value;
                }
                else if (fupDTCCODE2.PostedFile.ContentLength != 0)
                {

                    sDTCPhotoExtension = System.IO.Path.GetExtension(fupDTCCODE2.FileName).ToString().ToLower();
                    sDTCPhotoExtension = ";" + sDTCPhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sDTCPhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sDTCFileName = Path.GetFileName(fupDTCCODE2.PostedFile.FileName);


                    fupDTCCODE2.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTCFileName));
                    sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTCFileName);
                }
                if (sDTCFileName != "")
                {
                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + objnewDTC.sEnumDetailsID + "/" + sDTCFolderName + "/");
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(SFTPmainfolder + objnewDTC.sEnumDetailsID + "/" + sDTCFolderName);
                        }

                        Isuploaded = objFtp.upload(SFTPmainfolder + objnewDTC.sEnumDetailsID + "/" + sDTCFolderName + "/", sDTCFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objnewDTC.sDTCPhoto2Path = objnewDTC.sEnumDetailsID + "/" + sDTCFolderName + "/" + sDTCFileName;
                            hdfDTCPoto2Path.Value = SFTPmainfolder + objnewDTC.sDTCPhoto2Path;
                            Session["fupOldCodePhoto"] = null;
                        }
                    }
                }

                //// Infosys Photo Save
                //if (hdfInfosysAssetIdPath.Value.Trim() != "")
                //{
                //    sInfosysCodePhotoExtension = System.IO.Path.GetExtension(hdfInfosysAssetIdPath.Value).ToString().ToLower();
                //    sInfosysCodePhotoExtension = ";" + sInfosysCodePhotoExtension.Remove(0, 1) + ";";

                //    if (!sFileExt.Contains(sInfosysCodePhotoExtension))
                //    {
                //        ShowMsgBox("Invalid Image Format");
                //        return false;
                //    }

                //    sInfosysCodeFileName = Path.GetFileName(hdfInfosysAssetIdPath.Value);
                //    sDirectory = hdfInfosysAssetIdPath.Value;
                //    objnewDTC.sInfosysCodePhotoPath = hdfInfosysAssetIdPath.Value;
                //}
                //else if (fupInfosysId.PostedFile.ContentLength != 0)
                //{
                //    sInfosysCodePhotoExtension = System.IO.Path.GetExtension(fupInfosysId.FileName).ToString().ToLower();
                //    sInfosysCodePhotoExtension = ";" + sInfosysCodePhotoExtension.Remove(0, 1) + ";";

                //    if (!sFileExt.Contains(sInfosysCodePhotoExtension))
                //    {
                //        ShowMsgBox("Invalid Image Format");
                //        return false;
                //    }

                //    sInfosysCodeFileName = Path.GetFileName(fupInfosysId.PostedFile.FileName);

                //    fupInfosysId.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sInfosysCodeFileName));
                //    sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sInfosysCodeFileName);
                //}

                //if (sInfosysCodeFileName != "")
                //{
                //    if (File.Exists(sDirectory))
                //    {
                //        IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + objnewDTC.sEnumDetailsID + "/" + sInfosysFolderName + "/");
                //        if (IsExists == false)
                //        {

                //            objFtp.createDirectory(SFTPmainfolder + objnewDTC.sEnumDetailsID + "/" + sInfosysFolderName);
                //        }

                //        Isuploaded = objFtp.upload(SFTPmainfolder + objnewDTC.sEnumDetailsID + "/" + sInfosysFolderName , sInfosysCodeFileName, sDirectory);
                //        if (Isuploaded == true & File.Exists(sDirectory))
                //        {
                //            File.Delete(sDirectory);
                //            objnewDTC.sInfosysCodePhotoPath = SFTPmainfolder + objnewDTC.sEnumDetailsID + "/" + sInfosysFolderName + "/" + sInfosysCodeFileName;
                //            hdfInfosysAssetIdPath.Value = SFTPmainfolder + objnewDTC.sInfosysCodePhotoPath;
                //        }
                //    }
                //}

                //    clsException.SaveFunctionExecLog("SaveImagesPath --- END", Session["FullName"].ToString(), "2");

                bool bResult;

                //   clsException.SaveFunctionExecLog("SaveImagePathDetails --- START", Session["FullName"].ToString(), "2");

                if (hdfEnumarationId.Value.Trim() == "")
                {
                    bResult = objnewDTC.SaveImagePathDetails(objnewDTC);
                }
                else
                {
                    bResult = objnewDTC.UpdateImagePathDetails(objnewDTC);
                }

                // clsException.SaveFunctionExecLog("SaveImagePathDetails --- END", Session["FullName"].ToString(), "2");
                // clsException.SaveFunctionExecLog("ENDImageSaveFunction --- END", Session["FullName"].ToString(), "2");
                return bResult;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveImages");
                return false;
            }
        }

        public void getNewDTCDetails(string sDTCId)
        {
            try
            {
                DataTable dtNewDTCDetails = new DataTable();
                clsCRReport crDetails = new clsCRReport();
                dtNewDTCDetails = crDetails.GetNewDTCDetails(sDTCId);
                if(dtNewDTCDetails.Rows.Count > 0)
                {
                    txtDTCCode.Text = Convert.ToString(dtNewDTCDetails.Rows[0]["DT_CODE"]);
                    txtNewDTr.Text = Convert.ToString(dtNewDTCDetails.Rows[0]["DT_TC_ID"]);
                    txtWrkOrderDate.Text = Convert.ToString(dtNewDTCDetails.Rows[0]["WO_DATE"]);
                    txtWONO.Text = Convert.ToString(dtNewDTCDetails.Rows[0]["WO_NO"]);
                    txtDtrCode.Text = Convert.ToString(dtNewDTCDetails.Rows[0]["TC_ID"]);


                    if (Convert.ToString(dtNewDTCDetails.Rows[0]["DT_NEWDTC_TTK_FLOW"])== "1")
                    {
                       // txtWONO.Text = Convert.ToString(dtNewDTCDetails.Rows[0]["WO_TTK_MANUAL_NO"]);
                        
                        cmdflowType.SelectedValue = "2";
                        cmdflowType.Enabled = false;
                        hdfTTK_flow.Value = Convert.ToString(dtNewDTCDetails.Rows[0]["DT_NEWDTC_TTK_FLOW"]);
                        

                    }
                    else
                    {
                       // txtWONO.Text = txtWONO.Text.Split('/').GetValue(3).ToString() + " / " + txtWONO.Text.Split('/').GetValue(4).ToString() + " / " + txtWONO.Text.Split('/').GetValue(5).ToString();                    
                        lblwoNo.Visible = true;
                        cmdflowType.SelectedValue = "1";
                        cmdflowType.Enabled = false;
                        hdfTTK_flow.Value ="0";
                        dvfupSSplate.Style.Add("display", "none");
                        dvfupNamePlate.Style.Add("display", "none");
                    }             
                    
                    txtDTCName.Text = Convert.ToString(dtNewDTCDetails.Rows[0]["DT_NAME"]);
                    hdfRefOffCode.Value = Convert.ToString(dtNewDTCDetails.Rows[0]["WO_REF_OFFCODE"]);
                    hdfWOSLNO.Value = Convert.ToString(dtNewDTCDetails.Rows[0]["WO_SLNO"]);
                    hdfRecordId.Value = Convert.ToString(dtNewDTCDetails.Rows[0]["WO_RECORD_ID"]);
                    hdfTcslno.Value = Convert.ToString(dtNewDTCDetails.Rows[0]["TC_SLNO"]);
                    hdfmakeid.Value = Convert.ToString(dtNewDTCDetails.Rows[0]["TC_MAKE_ID"]);
                    hdfcapacity.Value = Convert.ToString(dtNewDTCDetails.Rows[0]["TC_CAPACITY"]);
                    hdftranscommission.Value = Convert.ToString(dtNewDTCDetails.Rows[0]["DT_TRANS_COMMISION_DATE"]);
                    hdfTcRating.Value = Convert.ToString(dtNewDTCDetails.Rows[0]["TC_RATING"]);
                }
                
            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void lnkNewDTr_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtDtrCode.Text.Trim() != "")
                {
                    string sDTRcode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtDtrCode.Text));
                    Response.Redirect("/MasterForms/TcMaster.aspx?TCId=" + sDTRcode, false);
                }
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
                        hdfWFDataId.Value = Convert.ToString(Session["WFDataId"]);
                        hdfWFOId.Value = Convert.ToString(Session["WFOId"]);
                        hdfWFOAuto.Value = Convert.ToString(Session["WFOAutoId"]);
                        hdfApproveStatus.Value = Convert.ToString(Session["ApproveStatus"]);

                        //Session["WFDataId"] = null;
                        Session["WFOId"] = null;
                        Session["WFOAutoId"] = null;
                        Session["ApproveStatus"] = null;
                    }


                    if (hdfWFDataId.Value != "0")
                    {
                        GetCRDetailsFromXML(hdfWFDataId.Value);
                    }
                    SetControlText();
                    if (txtActiontype.Text == "V")
                    {
                        //cmdCR.Enabled = false;
                        cmdCR.Text = "View";
                        dvComments.Style.Add("display", "none");
                        //txtInvQty.ReadOnly = true;
                        //txtDecommInventry.ReadOnly = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void RetainImageinPostbackSession()
        {
            string[] arrSave_ImageSession_String = { "fupNamePlate", "fupSSPlate", "fupDTLMSCodePhoto", "fupIPEnum", "fupOldCodePhoto", "fupInfosys", "fupDTCPhoto" };
            Session["arrSave_ImageSession_String"] = arrSave_ImageSession_String;
            try
            {
                //Name Plate Photo

                //Case: 1 When the page is submitted for the first time(First PostBack) and there is file 
                // in FileUpload control but session is Null then Store the values to Session Object as:
                if (Session["fupNamePlate"] == null && fupNamePlate.HasFile)
                {
                    Session["fupNamePlate"] = fupNamePlate;
                }
                if (Session["fupSSPlate"] == null && fupSSPlate.HasFile)
                {
                    Session["fupSSPlate"] = fupSSPlate;
                }
                if (Session["fupDTLMSCodePhoto"] == null && fupDTLMSCODE.HasFile)
                {
                    Session["fupDTLMSCodePhoto"] = fupDTLMSCODE;
                }
                if (Session["fupIPEnum"] == null && fupIpEnum.HasFile)
                {
                    Session["fupIPEnum"] = fupIpEnum;
                }
                if (Session["fupOldCodePhoto"] == null && fupDTCCODE2.HasFile)
                {
                    Session["fupOldCodePhoto"] = fupDTCCODE2;
                }
                //if (Session["fupInfosys"] == null && fupInfosysId.HasFile)
                //{
                //    Session["fupInfosys"] = fupInfosysId;
                //}
                if (Session["fupDTCPhoto"] == null && fupDTCCODE1.HasFile)
                {
                    Session["fupDTCPhoto"] = fupDTCCODE1;
                }

                // Case 2: On Next PostBack Session has value but FileUpload control is
                // Blank due to PostBack then return the values from session to FileUpload as:

                if (Session["fupNamePlate"] != null && (!fupNamePlate.HasFile))
                {
                    fupNamePlate = (FileUpload)Session["fupNamePlate"];
                }
                if (Session["fupSSPlate"] != null && (!fupSSPlate.HasFile))
                {
                    fupSSPlate = (FileUpload)Session["fupSSPlate"];
                }
                if (Session["fupDTLMSCodePhoto"] != null && (!fupDTLMSCODE.HasFile))
                {
                    fupDTLMSCODE = (FileUpload)Session["fupDTLMSCodePhoto"];
                }
                if (Session["fupIPEnum"] != null && (!fupIpEnum.HasFile))
                {
                    fupIpEnum = (FileUpload)Session["fupIPEnum"];
                }
                if (Session["fupOldCodePhoto"] != null && (!fupDTCCODE2.HasFile))
                {
                    fupDTCCODE2 = (FileUpload)Session["fupOldCodePhoto"];
                }
                //if (Session["fupInfosys"] != null && (!fupInfosysId.HasFile))
                //{
                //    fupInfosysId = (FileUpload)Session["fupInfosys"];
                //}
                if (Session["fupDTCPhoto"] != null && (!fupDTCCODE1.HasFile))
                {
                    fupDTCCODE1 = (FileUpload)Session["fupDTCPhoto"];
                }

                // Case 3: When there is value in Session but user want to change the file then
                // In this case we need to change the file in session object also as:
                if (fupNamePlate.HasFile)
                {
                    Session["fupNamePlate"] = fupNamePlate;
                }
                if (fupSSPlate.HasFile)
                {
                    Session["fupSSPlate"] = fupSSPlate;
                }
                if (fupDTLMSCODE.HasFile)
                {
                    Session["fupDTLMSCodePhoto"] = fupDTLMSCODE;
                }
                if (fupIpEnum.HasFile)
                {
                    Session["fupIPEnum"] = fupIpEnum;
                }
                if (fupDTCCODE2.HasFile)
                {
                    Session["fupOldCodePhoto"] = fupDTCCODE2;
                }
                //if (fupInfosysId.HasFile)
                //{
                //    Session["fupInfosys"] = fupInfosysId;
                //}
                if (fupDTCCODE1.HasFile)
                {
                    Session["fupDTCPhoto"] = fupDTCCODE1;
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "RetainImageinPostback");
            }
        }

        public void GetCRDetailsFromXML(string sWFDataId)
        {
            try
            {

                clsNewDTCCR objCRApproval = new clsNewDTCCR();
                objCRApproval.sWFDataId = sWFDataId;

                objCRApproval.GetCRDetailsFromXML(objCRApproval);

                txtCRDate.Text = objCRApproval.sCRDate;

                hdfDTCPoto1Path.Value = objCRApproval.sDTCPhoto1Path;
                hdfDTCPoto2Path.Value = objCRApproval.sDTCPhoto2Path;
                hdfNamePlatePath.Value = objCRApproval.sNamePlatePhotoPath;
                hdfSSPlatePath.Value = objCRApproval.sSSPlatePhotoPath;
                hdfInfosysAssetIdPath.Value = objCRApproval.sInfosysCodePhotoPath;
                hdfIpEnumarationPath.Value = objCRApproval.sIPEnumCodePhotoPath;
                hdfDTLMSCODEPath.Value = objCRApproval.sDTLMSCodePhotoPath;
                hdfLevel.Value=Convert.ToString(objCRApproval.sLevel);
                hdfEnumarationId.Value = objCRApproval.sEnumDetailsID;

                ShowUploadedImages();
                //txtInvQty.Text = objRIApproval.sInventoryQty;
                //txtDecommInventry.Text = objRIApproval.sDecommInventoryQty;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }
        bool validateform()
        {

            bool bValidate = false;
            if(txtCRDate.Text.Trim().Length==0)
            {
                txtCRDate.Focus();
                ShowMsgBox("Select CR date");
                return false;
            }

            if (fupDTLMSCODE.PostedFile.ContentLength == 0 && hdfDTLMSCODEPath.Value.Trim() == "")
            {
                fupDTLMSCODE.Focus();
                ShowMsgBox("Select DTLMS Code Photo to Upload");
                return false;
            }
            if (fupDTCCODE1.PostedFile.ContentLength == 0 && hdfDTCPoto1Path.Value.Trim() == "")
            {
                fupDTCCODE1.Focus();
                ShowMsgBox("Select DTC Photo1 to Upload");
                return false;
            }
           if(cmdflowType.SelectedValue=="2")
            {
                if (fupNamePlate.PostedFile.ContentLength == 0 && hdfNamePlatePath.Value.Trim() == "")
                {
                    fupNamePlate.Focus();
                    ShowMsgBox("Select TC Name Plate Photo to Upload");
                    return false;
                }
                if (fupSSPlate.PostedFile.ContentLength == 0 && hdfSSPlatePath.Value.Trim() == "")
                {
                    fupSSPlate.Focus();
                    ShowMsgBox("Select Tc SS Plate Photo to Upload");

                    return false;
                }
            }

            if (fupDTCCODE2.PostedFile.ContentLength == 0 && hdfDTCPoto2Path.Value.Trim() == "")
            {
                fupDTCCODE2.Focus();
                ShowMsgBox("Select DTC Code 2 Photo to Upload");
                return false;
            }
            string sValidateResult = ValidateImages();
            if (sValidateResult != "")
            {
                ShowMsgBox(sValidateResult);
                return false;
            }
            bValidate = true;
            return bValidate;
        }
        public void SetControlText()
        {
            try
            {
                txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));

                if (txtActiontype.Text == "A")
                {
                    cmdCR.Text = "Approve";
                    pnlApproval.Enabled = false;
                }
                if (txtActiontype.Text == "R")
                {
                    cmdCR.Text = "Reject";
                    pnlApproval.Enabled = false;
                }
                if (txtActiontype.Text == "M")
                {
                    cmdCR.Text = "Modify and Approve";
                    pnlApproval.Enabled = true;
                }


                dvComments.Style.Add("display", "block");                

                // Check for Creator of Form
                bool bResult = CheckFormCreatorLevel();
                if (bResult == true)
                {

                    pnlApproval.Enabled = true;

                    // To handle Record From Reject 
                    if (txtActiontype.Text == "A" && hdfWFOAuto.Value == "0")
                    {
                        txtActiontype.Text = "M";
                        hdfRejectApproveRef.Value = "RA";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public bool CheckFormCreatorLevel()
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                string sResult = objApproval.GetFormCreatorLevel("", objSession.RoleId, "NewDTCCR");
                if (sResult == "1")
                {
                    if (txtActiontype.Text != "V")
                    {
                        //txtcertify.Checked = false;
                        //txtcertify.Enabled = true;
                    }

                    return true;

                }
                return false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }

        protected void cmdCR_Click(object sender, EventArgs e)
        {
            try
            {
                clsNewDTCCR objNewDTC = new clsNewDTCCR();
                string[] Arr = new string[2];

                if (cmdCR.Text == "View")
                {
                    if (hdfApproveStatus.Value != "")
                    {
                        if (hdfApproveStatus.Value == "1" || hdfApproveStatus.Value == "2")
                        {
                            GenerateCommissionReport(txtDTCCode.Text);
                        }
                    }
                    else
                    {
                        GenerateCommissionReport(txtDTCCode.Text);
                    }
                    GenerateCommissionReport(txtDTCCode.Text);
                    return;
                }
                if (txtComment.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Comments/Remarks");
                    txtComment.Focus();
                    return;

                }
                //Workflow
                WorkFlowObjects(objNewDTC);
                if(validateform()==true)
                {
  
                objNewDTC.sRefOfficeCode = hdfRefOffCode.Value;
                objNewDTC.sWOSlno = hdfWOSLNO.Value;
                objNewDTC.sTcCode = txtNewDTr.Text;
                objNewDTC.sDTCCode = txtDTCCode.Text;
                objNewDTC.sDTCName = txtDTCName.Text;
                objNewDTC.sCRDate = txtCRDate.Text;
                objNewDTC.sActionType = txtActiontype.Text;
                objNewDTC.sCrby = objSession.UserId;
                objNewDTC.sWFObjectId = hdfWFOId.Value;
                objNewDTC.sWFAutoId = hdfWFOAuto.Value;
                objNewDTC.sRecordId = hdfRecordId.Value;
                objNewDTC.sTCCapacity = hdfcapacity.Value;
                    if(hdfEnumarationId.Value!="")
                    {
                        objNewDTC.sEnumDetailsID = hdfEnumarationId.Value;
                    }
                objNewDTC.sTCMake = hdfmakeid.Value;
                objNewDTC.sTcSlno = hdfTcslno.Value;
                objNewDTC.sCrby = objSession.UserId;
                    if(hdfLevel.Value!="")
                    {
                        objNewDTC.sLevel = Convert.ToInt16(hdfLevel.Value);
                    }
                objNewDTC.sRating =hdfTcRating.Value;
                objNewDTC.sTransCommDate = hdftranscommission.Value;
                objNewDTC.sLevel += 1;


                    Arr = objNewDTC.SaveFieldEnumerationDetails(objNewDTC);

                    if (Arr[1] == "0" && objNewDTC.sEnumDetailsID != null || objNewDTC.sEnumDetailsID != "")
                    {
                        bool result = false;
                        result = SaveImagesPath(objNewDTC);
                    }


                    #region Modify and Approve

                    // For Modify and Approve
                    if (txtActiontype.Text == "M")
                {
                    if (hdfRejectApproveRef.Value != "RA")
                    {
                        if (txtComment.Text.Trim() == "")
                        {
                            ShowMsgBox("Enter Comments/Remarks");
                            txtComment.Focus();
                            return;

                        }
                    }

                    objNewDTC.sActionType = txtActiontype.Text;

                    Arr = objNewDTC.SaveCompletionReport(objNewDTC);
                   
                   

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (New DTC CR) NewDtc Commission ");
                    }
                        GenerateCommissionReport(txtDTCCode.Text);
                    if (Arr[1].ToString() == "0")
                    {
                        hdfWFDataId.Value = objNewDTC.sWFDataId;
                        ApproveRejectAction();
                        return;
                    }
                    if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0]);
                        return;
                    }
                }

                    #endregion

                    if (txtActiontype.Text == "A" || txtActiontype.Text == "R")
                    {
                        if (hdfWFDataId.Value != "0")
                        {
                            ApproveRejectAction();
                            if (objSession.sTransactionLog == "1")
                            {
                                Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (New DTC CR) NewDtc Commission ");
                            }
                            return;
                        }
                }

                Arr = objNewDTC.SaveCompletionReport(objNewDTC);
               
                   
                if (objSession.sTransactionLog == "1")
                {
                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (New DTC CR) NewDtc Commission ");
                }
                if (Arr[1] == "0")
                {
                    cmdCR.Enabled = false;
                    ShowMsgBox(Arr[0]);
                    GenerateCommissionReport(txtDTCCode.Text);
                }
              }
            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void RetainImageOnPostback()
        {
            try
            {
                //  clsException.SaveFunctionExecLog("RetainImageOnPostback --- START",Session["FullName"].ToString(), "2");

                string sDirectory = string.Empty;

                string sNamePlateFileName = string.Empty;
                string sSSPlateFileName = string.Empty;
                string sOldCodeFileName = string.Empty;
                string sIPEnumCodeFileName = string.Empty;
                string sDTLMSCodeFileName = string.Empty;
                string sDTCFileName = string.Empty;
                string sInfosysCodeFileName = string.Empty;

                if (fupSSPlate.HasFile)
                {
                    sSSPlateFileName = Path.GetFileName(fupSSPlate.PostedFile.FileName);
                    hdfSSPlatePath.Value = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sSSPlateFileName);
                    if (!File.Exists(hdfSSPlatePath.Value))
                    {
                        fupSSPlate.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sSSPlateFileName));
                    }
                }


                if (fupNamePlate.HasFile)
                {
                    sNamePlateFileName = Path.GetFileName(fupNamePlate.PostedFile.FileName);
                   hdfNamePlatePath.Value = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sNamePlateFileName);
                    if (!File.Exists(hdfNamePlatePath.Value))
                    {
                        fupNamePlate.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sNamePlateFileName));
                    }
                }

                if (fupDTLMSCODE.HasFile)
                {
                    sDTLMSCodeFileName = Path.GetFileName(fupDTLMSCODE.PostedFile.FileName);
                    hdfDTLMSCODEPath.Value = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTLMSCodeFileName);
                    if (!File.Exists(hdfDTLMSCODEPath.Value))
                    {
                        fupDTLMSCODE.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTLMSCodeFileName));
                    }
                }

                if (fupDTCCODE2.HasFile)
                {
                    sOldCodeFileName = Path.GetFileName(fupDTCCODE2.PostedFile.FileName);
                    hdfDTCPoto2Path.Value = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sOldCodeFileName);
                    if (!File.Exists(hdfDTCPoto2Path.Value))
                    {
                        fupDTCCODE2.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sOldCodeFileName));
                    }
                }

                if (fupIpEnum.HasFile)
                {
                    sIPEnumCodeFileName = Path.GetFileName(fupIpEnum.PostedFile.FileName);
                    hdfIpEnumarationPath.Value = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sIPEnumCodeFileName);
                    if (!File.Exists(hdfIpEnumarationPath.Value))
                    {
                        fupIpEnum.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sIPEnumCodeFileName));
                    }
                }

                //if (fupInfosysId.HasFile)
                //{
                //    sInfosysCodeFileName = Path.GetFileName(fupInfosysId.PostedFile.FileName);
                //    hdfInfosysAssetIdPath.Value = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sInfosysCodeFileName);
                //    if (!File.Exists(hdfInfosysAssetIdPath.Value))
                //    {
                //        fupInfosysId.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sInfosysCodeFileName));
                //    }
                //}

                if (fupDTCCODE1.HasFile)
                {
                    sDTCFileName = Path.GetFileName(fupDTCCODE1.PostedFile.FileName);
                    hdfDTCPoto1Path.Value = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTCFileName);
                    if (!File.Exists(hdfDTCPoto1Path.Value))
                    {
                        fupDTCCODE1.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTCFileName));
                    }
                }

                //   clsException.SaveFunctionExecLog("RetainImageOnPostback --- END",Session["FullName"].ToString(), "2");
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void ApproveRejectAction()
        {
            try
            {
                clsApproval objApproval = new clsApproval();


                if (txtComment.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Comments/Remarks");
                    txtComment.Focus();
                    return;
                }
                objApproval.sCrby = objSession.UserId;
                objApproval.sOfficeCode = objSession.OfficeCode;
                objApproval.sApproveComments = txtComment.Text.Trim();
                objApproval.sWFObjectId = hdfWFOId.Value;
                objApproval.sWFAutoId = hdfWFOAuto.Value;

                if (txtActiontype.Text == "A")
                {
                    objApproval.sApproveStatus = "1";
                }
                if (txtActiontype.Text == "R")
                {
                    objApproval.sApproveStatus = "3";
                }
                if (txtActiontype.Text == "RA")
                {
                    objApproval.sApproveStatus = "1";
                }
                if (txtActiontype.Text == "M")
                {
                    objApproval.sApproveStatus = "2";
                }

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

                objApproval.sClientIp = sClientIP;
                objApproval.sWFDataId = hdfWFDataId.Value;
                objApproval.sDescription = "Completion Report For Transformer Centre Code " + txtDTCCode.Text;
                bool bResult = objApproval.ApproveWFRequest(objApproval);
                if (bResult == true)
                {

                    if (objApproval.sApproveStatus == "1")
                    {
                        ShowMsgBox("Approved Successfully");

                        if (objSession.RoleId == "1" || objSession.RoleId == "3" || objSession.RoleId == "4" || objSession.RoleId == "6")
                        {
                            GenerateCommissionReport(txtDTCCode.Text);
                        }
                        cmdCR.Enabled = false;
                    }
                    else if (objApproval.sApproveStatus == "3")
                    {
                        ShowMsgBox("Rejected Successfully");
                        cmdCR.Enabled = false;
                    }
                    else if (objApproval.sApproveStatus == "2")
                    {
                        ShowMsgBox("Modified and Approved Successfully");    
                                         
                            GenerateCommissionReport(txtDTCCode.Text);        
                                        
                        cmdCR.Enabled = false;
                    }
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void GenerateCommissionReport(string sDtcCode)
        {
            try
            {
                string sDTCId = sDtcCode;


                // sWFDataId = hdfWOTTKstatus.Value;
                string strParam = string.Empty;
                strParam = "id=NewDtcCR_CommReport&DtcID=" + sDTCId + "&OffCode=" + objSession.OfficeCode;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                return;


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void WorkFlowObjects(clsNewDTCCR objNewDTC)
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


                objNewDTC.sFormName = "NewDTCCR";
                objNewDTC.sOfficeCode = objSession.OfficeCode;
                objNewDTC.sClientIP = sClientIP;               

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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}