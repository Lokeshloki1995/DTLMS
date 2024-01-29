using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using Renci.SshNet;
using Renci.SshNet.Common;
using Renci.SshNet.Sftp;

namespace IIITS.DTLMS.DashboardForm
{
    public partial class UploadCirculars : System.Web.UI.Page
    {
        string strFormCode = "UploadCirculars";

        public DataTable dtFiles { get; set; }


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

                    if (!IsPostBack)
                    {
                        CheckAccessRights("4");
                        string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                        string bindviewPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["CircularsVirtualPath"]);
                        string sUserName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_USER"]);
                        string sPassword = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_PASS"]);

                        Bindgridview(SFTPPath, bindviewPath, sUserName, sPassword);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void FTPUpload(object sender, EventArgs e)
        {
            try
            {

                //FTP Folder name. Leave blank if you want to upload to root folder.
                // string ftpFolder = "Uploads/";
                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                string sFileUploadloadPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["CircularsVirtualPath"]);
                string sUserName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_USER"]);
                string sPassword = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_PASS"]);
                string fileName = Path.GetFileName(FileUpload1.FileName);
                if (fileName == "" || fileName == null)
                {
                    ShowMsgBox("Please Select the file");
                    return;

                }
                string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["MaterialallotmentFormat"]);
                string sAnxFileExt = System.IO.Path.GetExtension(FileUpload1.FileName).ToString().ToLower();
                sAnxFileExt = ";" + sAnxFileExt.Remove(0, 1) + ";";

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
                    gvFiles.Visible = true;
                    dt = (DataTable)ViewState["DOCUMENTS"];
                }


                DataTable dtDocs = new DataTable();
                dtDocs = (DataTable)ViewState["DOCUMENTS"];
                clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);
                bool Isuploaded;


                if (File.Exists(sDirectory))
                {
                    bool IsExists = objFtp.FtpDirectoryExists(sFileUploadloadPath);

                    if (IsExists == false)
                    {
                        objFtp.createDirectory(sFileUploadloadPath);
                    }
                    Isuploaded = objFtp.upload(  sFileUploadloadPath, fileName, sDirectory);
                    if (Isuploaded == true & File.Exists(sDirectory))
                    {
                        File.Delete(sDirectory);
                        sDirectory = fileName;
                    }

                }
                ViewState["DOCUMENTS"] = dtDocs;
                Response.Redirect("UploadCirculars.aspx", false);
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

        public DataTable Bindgridview(string FtpServer, string PATH, string username, string password)
        {
            try
            {
                int port = 22;
                bool status = false;
                DataTable dtFiles = new DataTable();
                dtFiles.Columns.AddRange(new DataColumn[1] { new DataColumn("Name", typeof(string)) });

                List<string> directories = new List<string>();

                using (var client = new SftpClient(FtpServer, port, username, password))
                {
                    client.Connect();
                    status = client.Exists(PATH);
                    if (status == true)
                    {
                        IEnumerable<SftpFile> enumFiles = client.ListDirectory(PATH);
                        foreach (SftpFile fl in enumFiles)
                        {
                            if (fl.Name == "." || fl.Name == "..")
                                continue;
                            dtFiles.Rows.Add(fl.Name);

                        }
                    }
                }
                if (dtFiles.Rows.Count > 0)
                {
                    gvFiles.DataSource = dtFiles;
                    gvFiles.DataBind();
                }


                return dtFiles;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

                return dtFiles;
            }
        }

        protected void DownloadFile(object sender, EventArgs e)
        {
            string sFileDownloadPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["CircularsVirtualPath"]);
            string sUserName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_USER"]);
            string sPassword = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_PASS"]);
            string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
            string fileName = (sender as LinkButton).CommandArgument;
            bool status = false;
            try
            {
                clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);
                status= objFtp.Download(sFileDownloadPath+fileName, fileName);


                ////Create FTP Request.
                //FtpWebRequest request = (FtpWebRequest)WebRequest.Create(sFileDownloadPath + fileName);
                //request.Method = WebRequestMethods.Ftp.DownloadFile;

                ////Enter FTP Server credentials.
                //request.Credentials = new NetworkCredential(sUserName, sPassword);
                //request.UsePassive = true;
                //request.UseBinary = true;
                //request.EnableSsl = false;

                ////Fetch the Response and read it into a MemoryStream object.
                //FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                //using (MemoryStream stream = new MemoryStream())
                //{
                //    //Download the File.
                //    response.GetResponseStream().CopyTo(stream);
                //    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //    Response.BinaryWrite(stream.ToArray());
                //    Response.End();
                //}
            }
            catch (Exception ex)
            {
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
                objApproval.sFormName = "Upload Files";
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
        protected void GridView1_RowDataBound(object sender,
                         GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton l = (LinkButton)e.Row.FindControl("lnkDelete");
                l.Attributes.Add("onclick", "javascript:return " +
                "confirm('Are you sure you want to delete this File " +
                DataBinder.Eval(e.Row.DataItem, "Name") + "')");
            }
        }
        protected void DeleteFile1(object sender, EventArgs e)
        {
            bool status = false;
            string sFileDeletePath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["CircularsVirtualPath"]);
            string sUserName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_USER"]);
            string sPassword = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_PASS"]);
            string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
            string fileName = (sender as LinkButton).CommandArgument;
            //clsFtp objFtp = new clsFtp(sFileDeletePath, sUserName, sPassword);
            //objFtp.delete(fileName);
            clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);
            status = objFtp.Delete(sFileDeletePath + fileName );



            Response.Redirect("UploadCirculars.aspx", false);
        }

    }
}