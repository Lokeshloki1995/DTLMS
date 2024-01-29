using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.IO;
using System.Net;
using System.Configuration;
using System.Data.OleDb;

namespace IIITS.DTLMS.Transaction
{
    public partial class ExcelUpload : System.Web.UI.Page
    {
        string strFormCode = "ExcelUpload";
        clsSession objSession;

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
                objSession = (clsSession)Session["clsSession"];

            }
            catch (Exception ex)
            {
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
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);

                string fileName = Path.GetFileName(fupUpload.FileName);

                if (fileName == "" || fileName == null)
                {
                    ShowMsgBox("Please select the File!");
                    return;
                }

                string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["UploadFormat"]);
                string sAnxFileExt = System.IO.Path.GetExtension(fupUpload.FileName).ToString().ToLower();
                sAnxFileExt = ";" + sAnxFileExt.Remove(0, 1) + ";";
                clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);
                //   clsFtp objFtp = new clsFtp(sFileServerPath, sUserName, sPassword);
                bool Isuploaded;
                bool IsFileExiest;
                string sMainFolderName = "DTC_KWH_TEMPLATE";
                if (!sFileExt.Contains(sAnxFileExt))
                {
                    ShowMsgBox("Invalid Image Format");
                    return;
                }

                fupUpload.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + fileName));
                string sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + fileName);

                // DataTable dt = ConvertoxlxToDataTable(sDirectory);


                if (File.Exists(sDirectory))
                {

                    bool IsExists = objFtp.FtpDirectoryExists(sFileServerPath + "/" + sMainFolderName);
                    if (IsExists == false)
                    {
                        objFtp.createDirectory(sFileServerPath + "/" + sMainFolderName);
                    }

                    IsFileExiest = objFtp.IsfileExiest(sFileServerPath + "/" + sMainFolderName + "/");
                    if (IsFileExiest == false)
                    {
                        Isuploaded = objFtp.upload(sFileServerPath + "/" + sMainFolderName, fileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            sDirectory = fileName;
                            ShowMsgBox("Successfully Uploaded your File!");
                            return;
                        }
                    }

                }

            }
            catch (WebException ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdUpload_click(object sender, EventArgs e)
        {
            try
            {
               
               
                if (fupUpload.PostedFile.ContentLength == 0)
                {
                    ShowMsgBox("Please Select the File");
                    fupUpload.Focus();
                    return;
                }

                string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["UploadFormat"]);
                string sUploadFileExt = System.IO.Path.GetExtension(fupUpload.FileName).ToString().ToLower();
                sUploadFileExt = ";" + sUploadFileExt.Remove(0, 1) + ";";

                if (!sFileExt.Contains(sUploadFileExt))
                {
                    ShowMsgBox("Invalid File Format");
                    return;
                }
               string excelPath = Server.MapPath("~/DTLMSDocs/") + Path.GetFileName(fupUpload.PostedFile.FileName);
               fupUpload.SaveAs(excelPath);
                string conString = string.Empty;
                string extension = Path.GetExtension(fupUpload.PostedFile.FileName);
                switch (extension)
                {
                    case ".xls": //Excel 97-03
                        conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07 or higher
                        conString = ConfigurationManager.ConnectionStrings["Excel07+ConString"].ConnectionString;
                        break;
                }
                conString = string.Format(conString, excelPath);
                DataTable dtExcelData = new DataTable();
                using (OleDbConnection excel_con = new OleDbConnection(conString))
                {
                    excel_con.Open();
                    string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();

                   // dtExcelData.Columns.AddRange(new DataColumn[2] { new DataColumn("DT_CODE", typeof(string)),
                   //new DataColumn("DT_KWH_READING", typeof(string)) });
          

                    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", excel_con))
                    {
                        oda.Fill(dtExcelData);
                       
                    }
                    if (dtExcelData.Columns.Contains("DT_CODE") && dtExcelData.Columns.Contains("DT_KWH_READING") && dtExcelData.Columns.Contains("DT_AVERAGE_LOAD(in KVA)")
                        && dtExcelData.Columns.Contains("DT_PEAK_LOAD(in KVA)") && dtExcelData.Columns.Contains("DT_SURPLUS_CAPACITY(in KVA)"))
                    {
                        ClsExcelUpload obj = new ClsExcelUpload();
                        string Userid = objSession.UserId;
                        string result = obj.ExecuteUploadExecl(dtExcelData, Userid);
                        if (result == "1")
                        {
                            ShowMsgBox("Uploaded Successfully");
                        }
                        else
                        {
                            ShowMsgBox("Something went Wrong Please Try again....!!!");
                        }
                        excel_con.Close();
                        System.IO.File.Delete(excelPath);
                        //excelPath
                        
                    }
                    else
                    {

                        ShowMsgBox("Please Upload the file in correct Format....!!!");
                        excel_con.Close();
                        System.IO.File.Delete(excelPath);
                    }
                    
                }
                
              
               

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                ShowMsgBox("Something went Wrong Please Try again....!!!");
                
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
        public string GetFileName(string sFTPLink, string sFTPUserName, string sFTPPassword)
        {
            string fileName = string.Empty;
            try
            {
                WebRequest request = (WebRequest)WebRequest.Create(sFTPLink);
                request.Credentials = new NetworkCredential(sFTPUserName, sFTPPassword);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                try
                {
                    using (request.GetResponse())
                    {
                        StreamReader streamReader = new StreamReader(request.GetResponse().GetResponseStream());
                        fileName = streamReader.ReadLine();
                        streamReader.Close();
                        return fileName;
                    }
                }
                catch (WebException)
                {
                    return fileName;
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return fileName;
            }
        }
        protected void DownloadFile(object sender, EventArgs e)
        {
            try
            {
                 

                    string fileName = GetFileName(sFileServerPath + "/DTC_KWH_TEMPLATE/" , sUserName, sPassword);
                    if (fileName != null && fileName != "")
                    {

                        //Create FTP Request.
                        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(sFileServerPath + "/DTC_KWH_TEMPLATE/"  + fileName);
                        request.Method = WebRequestMethods.Ftp.DownloadFile;

                        //Enter FTP Server credentials.
                        request.Credentials = new NetworkCredential(sUserName, sPassword);
                        request.UsePassive = true;
                        request.UseBinary = true;
                        request.EnableSsl = false;

                        //Fetch the Response and read it into a MemoryStream object.
                        FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                        using (MemoryStream stream = new MemoryStream())
                        {
                            //Download the File.
                            response.GetResponseStream().CopyTo(stream);
                            Response.AddHeader("content-disposition", "attachment;filename=" + "DTC_KWH_Template" + ".xlsx");
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.BinaryWrite(stream.ToArray());
                            Response.End();
                        }
                    }
                }
                 
            
            catch (WebException ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }
        protected void cmdClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (objSession.sRoleType == "2")
                {
                    Response.Redirect("~/StoreDashboard.aspx", false);
                }
                else
                {
                    Response.Redirect("~/Dashboard.aspx", false);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}