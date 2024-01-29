using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Net;
using System.IO;
using System.Configuration;
using IIITS.DTLMS.BL.Dashboard;

namespace IIITS.DTLMS.DashboardForm
{
    public partial class DownLoad : System.Web.UI.Page
    {
        string strFormCode = "DownLoad";
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
                    lblMessage.Text = string.Empty;
                    string bindviewPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["CircularsVirtualPath"]);
                    string sUserName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_USER"]);
                    string sPassword = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_PASS"]);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }


        }
        protected void cmdNew_Click(object sender, EventArgs e)
        {
            try
            {

                if (objSession.RoleId != "11")
                {
                    Response.Redirect("~/UserRestrict.aspx", false);
                    return;
                }

                Response.Redirect("UserManualUpload.aspx", false);

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
                objApproval.sFormName = "FeederMast";
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
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;

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
        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            try
            {
                ClsDownload obj = new ClsDownload();
               string path= obj.GetEnumUserManualDoc("1");
                string Filename = string.Empty;
                //string Filename = MapPath("~/UserManual/HESCOM_Enumeration_ User Manual.pdf");
                if (path!="")
                {
                     Filename = MapPath("~/UserManual/Enumeration_User_Manual/" + path);
                }
                else
                {
                    ShowMsgBox("File Not Found");
                    return;
                }
                // This is an important header part that informs the client to download this file.
                Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(Filename));
                Response.ContentType = "Application/pdf";
                //Write the file directly to the HTTP content output stream.
                Response.WriteFile(Filename);

                if (objSession.sTransactionLog == "1")
                {
                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " WebUser Manual Download ");
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void lnkDownload_Click1(object sender, EventArgs e)
        {
            try
            {
                ClsDownload obj = new ClsDownload();
                string path = obj.GetEnumUserManualDoc("3");
                string Filename = string.Empty;
                //string Filename = MapPath("~/UserManual/HESCOM _ Work Flow.pdf");
                if (path != "")
                {
                    Filename = MapPath("~/UserManual/Work_flow/" + path);
                }
                else
                {
                    ShowMsgBox("File Not Found");
                    return;
                }
                // This is an important header part that informs the client to download this file.
                Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(Filename));
                Response.ContentType = "Application/pdf";
                //Write the file directly to the HTTP content output stream.
                Response.WriteFile(Filename);

                if (objSession.sTransactionLog == "1")
                {
                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " WebUser Manual Download ");
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void lnkDownload_Click2(object sender, EventArgs e)
        {
            try
            {
                ClsDownload obj = new ClsDownload();
                string path = obj.GetEnumUserManualDoc("2");
                string Filename = string.Empty;
                //string Filename = MapPath("~/UserManual/HESCOM_DTLMS_User Manual_V_1.3.pdf");
                if (path != "")
                {
                     Filename = MapPath("~/UserManual/User_Manual/" + path);
                }
                else
                {
                    ShowMsgBox("File Not Found");
                    return;
                }
                // This is an important header part that informs the client to download this file.
                Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(Filename));
                Response.ContentType = "Application/pdf";
                //Write the file directly to the HTTP content output stream.
                Response.WriteFile(Filename);

                if (objSession.sTransactionLog == "1")
                {
                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " WebUser Manual Download ");
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void lnkAndroidManual_Click(object sender, EventArgs e)
        {
            try
            {
                string Filename = MapPath("~/UserManual/Android.pdf");
                // This is an important header part that informs the client to download this file.
                Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(Filename));
                Response.ContentType = "Application/pdf";
                //Write the file directly to the HTTP content output stream.
                Response.WriteFile(Filename);

                if (objSession.sTransactionLog == "1")
                {
                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " Android Manual Download ");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void lnkAndroidApk_Click(object sender, EventArgs e)
        {
            try
            {
                clsApkDownload objApk = new clsApkDownload();   
                String FTP_HOST = ConfigurationManager.AppSettings["FTP_HOST"].ToString();
                String FTP_USER = ConfigurationManager.AppSettings["FTP_USER"].ToString();
                String FTP_PASS = ConfigurationManager.AppSettings["FTP_PASS"].ToString();
                String ApkFileName = ConfigurationManager.AppSettings["ApkFileName"].ToString();
                string sFoldername = objApk.RetrieveLatestApkDetails();
                FTP_HOST = FTP_HOST + sFoldername;
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(FTP_HOST + ApkFileName);
                    request.Method = WebRequestMethods.Ftp.DownloadFile;

                    //Enter FTP Server credentials.
                    request.Credentials = new NetworkCredential(FTP_USER, FTP_PASS);
                    request.UsePassive = true;
                    request.UseBinary = true;
                    request.EnableSsl = false;

                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    
                    using (MemoryStream stream = new MemoryStream())
                    {
                        //Stream responseStream = response.GetResponseStream();
                        response.GetResponseStream().CopyTo(stream);
                        Response.AddHeader("content-disposition", "attachment;filename=" + ApkFileName);
                        Response.ContentType = "application/msi";
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.BinaryWrite(stream.ToArray());
                        Response.OutputStream.Close();
                    }


                //string Filename = MapPath("~/UserManual/DTLMSAPK.zip");
                // This is an important header part that informs the client to download this file.
                //Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(Filename));
                //Response.ContentType = "Application/pdf";
                //Write the file directly to the HTTP content output stream.
                //Response.WriteFile(Filename);

                if (objSession.sTransactionLog == "1")
                {
                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " Android APK Download ");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void LnkImgTansactionFlow_Click(object sender, EventArgs e)
        {
            try
            {
                string Filename = MapPath("~/UserManual/TransactionFlow.pdf");
                // This is an important header part that informs the client to download this file.
                Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(Filename));
                Response.ContentType = "Application/pdf";
                //Write the file directly to the HTTP content output stream.
                Response.WriteFile(Filename);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
       

        protected void lnkCirDownload_Click(object sender, EventArgs e)
        {
            try
            {
                //To open the PDF and also download
                //string pdfPath = Server.MapPath("~/UserManual/1.pdf");
                //WebClient client = new WebClient();
                //Byte[] buffer = client.DownloadData(pdfPath);
                //Response.ContentType = "application/pdf";
                //Response.AddHeader("content-length", buffer.Length.ToString());
                //Response.BinaryWrite(buffer);

                //Only To Download Pdf
                string Filename = MapPath("~/UserManual/Circular.pdf");
                // This is an important header part that informs the client to download this file.
                Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(Filename));
                Response.ContentType = "Application/pdf";
                //Write the file directly to the HTTP content output stream.
                Response.WriteFile(Filename);

                if (objSession.sTransactionLog == "1")
                {
                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " Circular Manual Download ");
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void lnkProDownload_Click(object sender, EventArgs e)
        {
            try
            {
                //To open the PDF and also download
                //string pdfPath = Server.MapPath("~/UserManual/1.pdf");
                //WebClient client = new WebClient();
                //Byte[] buffer = client.DownloadData(pdfPath);
                //Response.ContentType = "application/pdf";
                //Response.AddHeader("content-length", buffer.Length.ToString());
                //Response.BinaryWrite(buffer);

                //Only To Download Pdf
                string Filename = MapPath("~/UserManual/Procedure.pdf");
                // This is an important header part that informs the client to download this file.
                Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(Filename));
                Response.ContentType = "Application/pdf";
                //Write the file directly to the HTTP content output stream.
                Response.WriteFile(Filename);

                if (objSession.sTransactionLog == "1")
                {
                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " Procedure Manual Download ");
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
   
        protected void DownloadFile(object sender, EventArgs e)
        {
            string sFileDownloadPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["CircularsVirtualPath"]);
            string sUserName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_USER"]);
            string sPassword = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_PASS"]);
            string fileName = (sender as LinkButton).CommandArgument;

            try
            {

                // string sFileName = Path.GetFileName(fupAnx.PostedFile.FileName).Replace(",", "");

                //  FileUpload1.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + fileName));
                // string sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + fileName);
                // string localpath = "/Circulars/";
                //    DataTable dtDocs = new DataTable();
                //    dtDocs = (DataTable)ViewState["DOCUMENTS"];
                //    clsFtp objFtp = new clsFtp(sFileDownloadPath, sUserName, sPassword);

                //    objFtp.download(fileName,   + fileName);

                //    Response.Redirect("UploadCirculars.aspx", false);  
                //    }
                //    catch (WebException ex)
                //    {
                //        throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
                //    }
                //}


                //Create FTP Request.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(sFileDownloadPath + fileName);
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
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.BinaryWrite(stream.ToArray());
                    Response.End();
                }
            }
            catch (WebException ex)
            {
                throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
            }

        }
        protected void lnkAnx1Download_Click(object sender, EventArgs e)
        {
            try
            {
                //To open the PDF and also download
                //string pdfPath = Server.MapPath("~/UserManual/1.pdf");
                //WebClient client = new WebClient();
                //Byte[] buffer = client.DownloadData(pdfPath);
                //Response.ContentType = "application/pdf";
                //Response.AddHeader("content-length", buffer.Length.ToString());
                //Response.BinaryWrite(buffer);

                //Only To Download Pdf
                string Filename = MapPath("~/UserManual/Annexure1.pdf");
                // This is an important header part that informs the client to download this file.
                Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(Filename));
                Response.ContentType = "Application/pdf";
                //Write the file directly to the HTTP content output stream.
                Response.WriteFile(Filename);

                if (objSession.sTransactionLog == "1")
                {
                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " Annexure1RFT Manual Download ");
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void lnkAnx2Download_Click(object sender, EventArgs e)
        {
            try
            {
                //To open the PDF and also download
                //string pdfPath = Server.MapPath("~/UserManual/1.pdf");
                //WebClient client = new WebClient();
                //Byte[] buffer = client.DownloadData(pdfPath);
                //Response.ContentType = "application/pdf";
                //Response.AddHeader("content-length", buffer.Length.ToString());
                //Response.BinaryWrite(buffer);

                //Only To Download Pdf
                string Filename = MapPath("~/UserManual/Annexure2.pdf");
                // This is an important header part that informs the client to download this file.
                Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(Filename));
                Response.ContentType = "Application/pdf";
                //Write the file directly to the HTTP content output stream.
                Response.WriteFile(Filename);

                if (objSession.sTransactionLog == "1")
                {
                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " Annexure2JIRFT Manual Download ");
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void lnkAnx3Download_Click(object sender, EventArgs e)
        {
            try
            {
                //To open the PDF and also download
                //string pdfPath = Server.MapPath("~/UserManual/1.pdf");
                //WebClient client = new WebClient();
                //Byte[] buffer = client.DownloadData(pdfPath);
                //Response.ContentType = "application/pdf";
                //Response.AddHeader("content-length", buffer.Length.ToString());
                //Response.BinaryWrite(buffer);

                //Only To Download Pdf
                string Filename = MapPath("~/UserManual/Annexure3.pdf");
                // This is an important header part that informs the client to download this file.
                Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(Filename));
                Response.ContentType = "Application/pdf";
                //Write the file directly to the HTTP content output stream.
                Response.WriteFile(Filename);

                if (objSession.sTransactionLog == "1")
                {
                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " Annexure3JIRFT Manual Download ");
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void lnkAnx4Download_Click(object sender, EventArgs e)
        {
            try
            {
                //To open the PDF and also download
                //string pdfPath = Server.MapPath("~/UserManual/1.pdf");
                //WebClient client = new WebClient();
                //Byte[] buffer = client.DownloadData(pdfPath);
                //Response.ContentType = "application/pdf";
                //Response.AddHeader("content-length", buffer.Length.ToString());
                //Response.BinaryWrite(buffer);

                //Only To Download Pdf
                string Filename = MapPath("~/UserManual/Annexure4.pdf");
                // This is an important header part that informs the client to download this file.
                Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(Filename));
                Response.ContentType = "Application/pdf";
                //Write the file directly to the HTTP content output stream.
                Response.WriteFile(Filename);

                if (objSession.sTransactionLog == "1")
                {
                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " Annexure4AFT Manual Download ");
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }




        public DataTable dtFiles { get; set; }
    }
}