using IIITS.DTLMS.BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.MinorRepair
{
    public partial class RatesUpload : System.Web.UI.Page
    {
        string strFormCode = "RatesUpload";
        clsSession objSession = new clsSession();
        DataTable dtMeasurement = new DataTable();
     
        string sFileServerPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["EstimatioinVirtualPath"]);
        string sUserName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_USER"]);
        string sPassword = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_PASS"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }

                if (!IsPostBack)
                {
                    //CheckAccessRights("4");
                    Genaral.Load_Combo("SELECT \"TR_ID\",\"TR_NAME\" FROM \"TBLTRANSREPAIRER\" WHERE \"TR_STATUS\"='A'", "--Select--", cmbRepairer);
                    Genaral.Load_Combo("SELECT \"DIV_ID\", \"DIV_NAME\" FROM \"TBLDIVISION\" ORDER BY \"DIV_CODE\"", "--Select--", cmbDivision);
                }


            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sFormName = "RatesUpload";
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
        protected void FTPUpload(object sender, EventArgs e)
        {
            try
            {

                //FTP Folder name. Leave blank if you want to upload to root folder.
                // string ftpFolder = "Uploads/";      
                if (cmbDivision.Text != "" && cmbDivision.Text != "--Select--" && cmbRepairer.Text != "" && cmbRepairer.Text != "--Select--")
                {

                    string fileName = Path.GetFileName(FileUpload1.FileName);

                    if (fileName == "" || fileName == null)
                    {
                        ShowMsgBox("Please select the File!");
                        return;
                    }

                    string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                    string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);

                    string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["MaterialallotmentFormat"]);
                    string sAnxFileExt = System.IO.Path.GetExtension(FileUpload1.FileName).ToString().ToLower();
                    sAnxFileExt = ";" + sAnxFileExt.Remove(0, 1) + ";";
                    // clsFtp objFtp = new clsFtp(sFileServerPath, sUserName, sPassword);

                    clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);
                    bool Isuploaded;
                    bool IsFileExiest;
                    string sMainFolderName = "ESTIMATIONRATES";
                    if (!sFileExt.Contains(sAnxFileExt))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return;
                    }

                    // string sFileName = Path.GetFileName(fupAnx.PostedFile.FileName).Replace(",", "");

                    FileUpload1.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + fileName));
                    string sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + fileName);


                    DataTable dt = new DataTable("NEWTABLE");

                    if (ViewState["DOCUMENTS"] == null)
                    {
                        dt.Columns.Add("ID");
                        dt.Columns.Add("NAME");
                        dt.Columns.Add("TYPE");
                        dt.Columns.Add("PATH");
                    }
                    else
                    {

                        dt = (DataTable)ViewState["DOCUMENTS"];
                    }
                    DataTable dtDocs = new DataTable();




                    if (File.Exists(sDirectory))
                    {

                        bool IsExists = objFtp.FtpDirectoryExists(sFileServerPath + "/" + sMainFolderName + "/" + cmbDivision.Text);
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(sFileServerPath + "/" + sMainFolderName + "/" + cmbDivision.Text);
                        }
                        IsExists = objFtp.FtpDirectoryExists(sFileServerPath + "/" + sMainFolderName + "/" + cmbDivision.Text + "/" + cmbRepairer.Text);
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(sFileServerPath + "/" + sMainFolderName + "/" + cmbDivision.Text + "/" + cmbRepairer.Text);
                        }
                        IsFileExiest = objFtp.IsfileExiest(sFileServerPath + "/" + sMainFolderName + "/" + cmbDivision.Text + "/" + cmbRepairer.Text);
                        if (IsFileExiest == false)
                        {
                            Isuploaded = objFtp.upload(sFileServerPath + "/" + sMainFolderName + "/" + cmbDivision.Text + "/" + cmbRepairer.Text, fileName, sDirectory);
                            if (Isuploaded == true & File.Exists(sDirectory))
                            {
                                File.Delete(sDirectory);
                                sDirectory = cmbDivision.Text + "/" + cmbRepairer.Text + "/" + fileName;
                                ShowMsgBox("Successfully Uploaded your File!");
                                return;
                            }
                        }



                    }

                }
                else
                {
                    ShowMsgBox("Please select Division and Repairer");
                    return;
                }

            }
            catch (Exception ex)
            {
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

    }
}