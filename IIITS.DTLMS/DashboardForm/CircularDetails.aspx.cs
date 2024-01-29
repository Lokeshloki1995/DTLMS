using System;

using System.Collections.Generic;

using System.Linq;

using System.Web;

using System.Web.UI;

using System.Data;

using System.Data.SqlClient;

using System.Configuration;

using System.IO;
using Renci.SshNet;
using Renci.SshNet.Common;
using Renci.SshNet.Sftp;


using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Net;



namespace IIITS.DTLMS.DashboardForm

{
    public partial class CircularDetails : System.Web.UI.Page
    {

        clsSession objSession;
        string strFormCode = "CircularDetails";

        public DataTable dtFiles { get; set; }
         
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
                    string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                    string sFileBindGridPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["CircularsVirtualPath"]);
                    string sUserName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_USER"]);
                    string sPassword = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_PASS"]);

                    //if (!IsPostBack)
                    //{
                    //    string[] filePaths = Directory.GetFiles(Server.MapPath("~/Uploads"));
                    //    List<ListItem> files = new List<ListItem>();
                    //    foreach (string filePath in filePaths)
                    //    {
                    //        files.Add(new ListItem(Path.GetFileName(filePath), filePath));
                    //    }
                    //    grdUpload.DataSource = files;
                    //    grdUpload.DataBind();
                    //}

                    Bindgridview(SFTPPath,sFileBindGridPath, sUserName, sPassword);
 
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }


            }
                 
        protected void grdUploadFiles_RowCommand(object sender, GridViewCommandEventArgs e)
        {

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

        //public DataTable BindgridView(string FtpServer, string username, string password)
        //{
        //    try
        //    {
        //        DataTable dtFiles = new DataTable();
        //        dtFiles.Columns.AddRange(new DataColumn[1] { new DataColumn("Name", typeof(string)) });
        //        WebRequest request = (WebRequest)WebRequest.Create(FtpServer);
        //        request.Credentials = new NetworkCredential(username, password);
        //        request.Method = WebRequestMethods.Ftp.ListDirectory;
        //        StreamReader streamReader = new StreamReader(request.GetResponse().GetResponseStream());
        //        string fileName3 = streamReader.ReadLine();

        //        List<string> directories = new List<string>();



        //        while (fileName3 != null && fileName3 != "testing12")
        //        {
        //            dtFiles.Rows.Add(fileName3);
        //            directories.Add(fileName3);
        //            fileName3 = streamReader.ReadLine();


        //        }

        //        streamReader.Close();


        //        using (WebClient ftpClient = new WebClient())
        //        {
        //            ftpClient.Credentials = new System.Net.NetworkCredential(username, password);

        //            for (int i = 0; i < directories.Count; i++)
        //            {
        //                if (directories[i].Contains("."))
        //                {

        //                   // string path = FtpServer + directories[i].ToString();
        //                     //string trnsfrpth = localpath + directories[i].ToString();
        //                     //ftpClient.DownloadFile(path, trnsfrpth);
        //                     gvFiles.DataSource = dtFiles;

        //                }

        //            }

        //            gvFiles.DataBind();
        //        }
        //        return dtFiles;
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = ex.Message;
        //        return dtFiles;
        //    }
        //}

        protected void DownloadFile(object sender, EventArgs e)
        {
            string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
            string sFileDownloadPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["CircularsVirtualPath"]);
            string sUserName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_USER"]);
            string sPassword = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_PASS"]);
            string fileName = (sender as LinkButton).CommandArgument;
            bool status = false;
            try
            {

                clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);
                status = objFtp.Download(sFileDownloadPath + fileName, fileName);




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
            catch (WebException ex)
            {
                throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
            }

        }


    }
}