using IIITS.DTLMS.BL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.DashboardForm
{
    public partial class UserManualUpload : System.Web.UI.Page
    {
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

                if (objSession.RoleId != "11")
                {
                    Response.Redirect("~/UserRestrict.aspx", false);
                }
                if (!IsPostBack)
                {
                    Genaral.Load_Combo("select \"UMT_ID\",\"UMT_NAME\" FROM \"TBLUSERMANUALTYPE\" WHERE \"UMT_STATUS\"='1' ORDER BY \"UMT_ID\"", "--Select--", cmbManualType);

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdClose_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/DashboardForm/DownLoad.aspx", false);
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                cmbManualType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
            string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);
            string sUserName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_USER"]);
            string sPassword = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_PASS"]);
          //  bool Isuploaded;
          //  string sMainFolderName = "UserManual";
            string SubFolderName = string.Empty;
            string sDirectory = string.Empty;
            string[] Arr = new string[2];
            clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);
            try
            {
                if (cmbManualType.SelectedIndex == 0)
                {
                    cmbManualType.Focus();
                    ShowMsgBox("Please select Manual Type");
                    return;
                }
                if (fupdDoc.FileName == "")
                {
                    ShowMsgBox("Please upload the Document.");
                    fupdDoc.Focus();
                    return;
                }
                if (cmbManualType.SelectedIndex > 0)
                {
                    switch (cmbManualType.SelectedIndex)
                    {
                        case 1:
                            SubFolderName = "Enumeration_User_Manual";
                            break;
                        case 2:
                            SubFolderName = "User_Manual";
                            break;
                        case 3:
                            SubFolderName = "Work_flow";
                            break;
                    }
                }
                if (Session["FileUpload"] != null && (!fupdDoc.HasFile))
                {
                    fupdDoc = (FileUpload)Session["FileUpload"];
                    lblFilename.Text = fupdDoc.FileName;
                }
                else
                {
                    if (fupdDoc.PostedFile.FileName != null && fupdDoc.PostedFile.FileName != "")
                    {
                        fupdDoc.SaveAs(Server.MapPath("~/UserManual" + "/"+ SubFolderName + "/"+ fupdDoc.FileName));
                    }
                    else
                    {
                        ShowMsgBox("Please upload the Document.");
                        fupdDoc.Focus();
                        return;
                    }
                }
                clsDashboard obj = new clsDashboard();
                obj.Manual_Type = cmbManualType.SelectedValue;
                obj.File_Name = fupdDoc.FileName;
                Arr = obj.SaveUploadedDocinUserManual(obj);

                ShowMsgBox(Arr[0]);
                btnReset_Click(sender, e);



                #region
                //if (Arr[1].ToString() == "0")
                //{
                //    if (cmbManualType.SelectedIndex > 0)
                //    {
                //        switch (cmbManualType.SelectedIndex)
                //        {
                //            case 1:
                //                SubFolderName = "Enumeration_User_Manual";
                //                break;
                //            case 2:
                //                SubFolderName = "User_Manual";
                //                break;
                //            case 3:
                //                SubFolderName = "Work_flow";
                //                break;
                //        }
                //    }
                //    string filename = Path.GetFileName(fupdDoc.PostedFile.FileName);

                //    if (fupdDoc.PostedFile != null)
                //    {
                //        if (fupdDoc.PostedFile.ContentLength != 0)
                //        {
                //            string strExt = filename.Substring(filename.LastIndexOf('.') + 1);

                //            string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FileFormat"]);
                //            string sAnxFileExt = System.IO.Path.GetExtension(fupdDoc.FileName).ToString().ToLower();
                //            sAnxFileExt = ";" + sAnxFileExt.Remove(0, 1) + ";";

                //            if (!sFileExt.Contains(sAnxFileExt))
                //            {
                //                ShowMsgBox("Invalid File Format");
                //                return;
                //            }
                //            sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + fupdDoc.FileName);

                //            //if (File.Exists(sDirectory))
                //            //{
                //            //    bool IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName);
                //            //    if (IsExists == false)
                //            //    {
                //            //        objFtp.createDirectory(SFTPmainfolder + sMainFolderName);
                //            //    }
                //            //    IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName + "/" + SubFolderName);
                //            //    if (IsExists == false)
                //            //    {

                //            //        objFtp.createDirectory(SFTPmainfolder + sMainFolderName + "/" + SubFolderName);
                //            //    }
                //            //    Isuploaded = objFtp.upload(SFTPmainfolder + sMainFolderName + "/" + SubFolderName + "/", filename, sDirectory);
                //            //    if (Isuploaded == true & File.Exists(sDirectory))
                //            //    {
                //            //        File.Delete(sDirectory);
                //            //        sDirectory = SubFolderName + "/" + filename;
                //            //    }
                //            //}
                //        }
                //        ShowMsgBox(Arr[0]);
                //        return;
                //    }
                //    else
                //    {
                //        ShowMsgBox("Please upload the File");
                //        return;
                //    }
                //}
                #endregion
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
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
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                                   System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

    }
}