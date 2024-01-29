using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Renci.SshNet;
using Renci.SshNet.Common;
using Renci.SshNet.Sftp;
using System.IO;
using System.Net;
using System.Threading;
using System.Web;
using System.Data.OleDb;
using System.Configuration;
using System.Data;


namespace IIITS.DTLMS.BL
{
    public class clsSFTP
    {
        string strFormCode = "clsSFTP";
        int port = 22;
        private string host = null;
        private string username = null;
        private string password = null;

        public clsSFTP(string hostIP, string userName, string pass) { host = hostIP; username = userName; password = pass; }


        public bool Download(string remoteFile, string localFile)
        {
            try
            {
                string localDirectory = HttpContext.Current.Server.MapPath(@"\DTLMSFiles\");
                using (var sftp = new SftpClient(host, username, password))
                {
                    sftp.Connect();

                    using (Stream file1 = File.OpenWrite(localDirectory + localFile))
                    {
                        sftp.DownloadFile(remoteFile, file1);
                    }
                }
                FileInfo fileInfo = new FileInfo(localDirectory + localFile);
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + fileInfo.Name);
                HttpContext.Current.Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                HttpContext.Current.Response.ContentType = "text/csv";
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.WriteFile(fileInfo.FullName);
                HttpContext.Current.Response.End();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool IsfileExiest(string sDirectory)
        {

            int port = 22;
            bool status = false;
            using (var client = new SftpClient(host, port, username, password))
            {
                client.Connect();
                status = client.Exists(sDirectory);
                if (status == true)
                {
                    IEnumerable<SftpFile> enumFiles = client.ListDirectory(sDirectory);
                    foreach (SftpFile fl in enumFiles)
                    {
                        if (fl.Name == "." || fl.Name == "..")
                            continue;
                        return true;

                    }
                }
            }
            return false;
        }

        //public bool Download(string remoteFile, string localFile)
        //{
        //    try
        //    {
        //        //SftpClient client = new SftpClient("bescomdtlms.com", 22, "FTP_USER", "Idea@2017");
        //        //client.Connect();
        //        //string sFilePath = "C:\\Users\\nithin.r\\Pictures\\Screenshots\\Screenshot (1).png";
        //        //client.ChangeDirectory("android_test");
        //        //using (FileStream fs = new FileStream(sFilePath, FileMode.Open))
        //        //{
        //        //    Console.WriteLine(sFilePath);
        //        //    client.BufferSize = 4 * 1024;
        //        //    client.UploadFile(fs, Path.GetFileName(sFilePath));

        //        //}
        //        //client.Disconnect();
        //     //   string localDirectory = @"C:\New folder";

        //        using (var sftp = new SftpClient(host, port, username, password))
        //        {
        //            sftp.Connect();
        //            using (FileStream file1 = File.OpenWrite( localFile))
        //            {                       
        //                sftp.DownloadFile(remoteFile, file1);                       
        //                return true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        /* Upload File */
        /// <summary>
        /// Upload file by passing the path
        /// </summary>
        /// <param name="UploadFile"></param>
        #region
        //public bool upload(string sFTPFolder, string remoteFile, string localFile, string Directory1 = "", string Directory2 = "", string Directory3 = "", string Directory4 = "")
        //{
        //    bool status = false;
        //    try
        //    {
        //        using (var client = new SftpClient(host, port, username, password))
        //        {
        //            client.Connect();
        //            if (Directory1.Length > 0)
        //            {
        //                if (!client.Exists(Directory1))
        //                {
        //                    client.CreateDirectory(Directory1);
        //                }
        //                if (Directory2.Length > 0)
        //                {
        //                    if (!client.Exists(Directory2))
        //                    {
        //                        client.CreateDirectory(Directory2);
        //                    }
        //                }
        //            }
        //            using (var fileStream = new FileStream(localFile, FileMode.Open))
        //            {
        //                client.UploadFile(fileStream, sFTPFolder + remoteFile);
        //                client.Disconnect();
        //                status = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        status = false;
        //    }
        //    return status;
        //}
        #endregion


        public bool upload(string sFTPFolder, string remoteFile, string localFile)
        {
            bool status = false;
            try
            {
                using (var client = new SftpClient(host, port, username, password))
                {
                    client.Connect();
                    using (var fileStream = new FileStream(localFile, FileMode.Open))
                    {
                        client.UploadFile(fileStream, sFTPFolder + remoteFile);
                        client.Disconnect();
                        status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                status = false;
            }
            return status;
        }
        public bool Delete(string UploadFile)
        {

            bool status = false;
            try
            {
                using (var client = new SftpClient(host, port, username, password))
                {
                    client.Connect();
                    client.DeleteFile(UploadFile);
                    client.Disconnect();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }

        public bool Rename(string sActualFileName, string sRenamedFileName)
        {
            bool status = false;
            try
            {
                using (var client = new SftpClient(host, port, username, password))
                {
                    client.Connect();
                    client.RenameFile(sActualFileName, sRenamedFileName);
                    status = true;
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }

        public bool FtpDirectoryExists(string directoryPath)
        {
            bool status = false;
            try
            {
                using (var client = new SftpClient(host, port, username, password))
                {
                    client.Connect();
                    status = client.Exists(directoryPath);
                    if (status == false)
                    {
                        //clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, directoryPath, directoryPath);
                    }
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                status = false;
            }
            return status;
        }

        public bool createDirectory(string newDirectory)
        {
            bool status = false;
            try
            {
                using (var client = new SftpClient(host, port, username, password))
                {
                    client.Connect();
                    if (!client.Exists(newDirectory))
                    {
                        client.CreateDirectory(newDirectory);
                    }
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                status = false;
            }
            return status;
        }
        //public string GetFileName(string sPath)
        //{
        //    string result =string.Empty;
        //    bool status = false;
        //    try
        //    {
        //        using (var client = new SftpClient(host, port, username, password))
        //        {

        //            client.Connect();
        //            status = client.Exists(sPath);
        //            if (status == true)
        //            {
        //                IEnumerable<SftpFile> enumFiles = client.ListDirectory(sPath);
        //                foreach (SftpFile fl in enumFiles)
        //                {
        //                    if (fl.Name == "." || fl.Name == "..")
        //                        continue;
        //                    return result;

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result = "";
        //    }
        //    return result;
        //}


        public string GetFileName(string sPath)
        {
            string result = string.Empty;
            bool status = false;
            try
            {
                using (var client = new SftpClient(host, port, username, password))
                {

                    client.Connect();
                    status = client.Exists(sPath);
                    if (status == true)
                    {
                        IEnumerable<SftpFile> enumFiles = client.ListDirectory(sPath);
                        foreach (SftpFile fl in enumFiles)
                        {
                            if (fl.Name == "." || fl.Name == "..")
                                continue;
                            result = fl.Name;
                            return result;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = "";
            }
            return result;
        }
        public DataTable GetListOfFiles(string sPath)
        {
            DataTable dtFiles = new DataTable();
            dtFiles.Columns.AddRange(new DataColumn[1] { new DataColumn("Name", typeof(string)) });
            bool status = false;
            try
            {
                using (var client = new SftpClient(host, port, username, password))
                {

                    client.Connect();
                    status = client.Exists(sPath);
                    if (status == true)
                    {
                        IEnumerable<SftpFile> enumFiles = client.ListDirectory(sPath);
                        foreach (SftpFile fl in enumFiles)
                        {
                            if (fl.Name == "." || fl.Name == "..")
                                continue;
                            dtFiles.Rows.Add(fl.Name);
                        }
                    }
                }
                return dtFiles;
            }
            catch (Exception ex)
            {

            }
            return dtFiles;
        }


        /// <summary>
        /// Verify uploaded file is success or not 
        /// </summary>
        /// <param name="UploadFile"></param>
        /// <returns></returns>
        public bool VerifyUploadedFile(string UploadFile)
        {
            bool status = false;
            try
            {
                using (var client = new SftpClient(host, port, username, password))
                {
                    client.Connect();
                    status = client.Exists(Path.GetFileName(UploadFile));
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }

        //public void ListSFTPDirContents()
        //{
        //    try
        //    {
        //        string strFtpFilename = string.Empty;
        //        string strFtpFileSize = string.Empty;
        //        string strFtpFileModTime = string.Empty;
        //        StreamWriter sw = null;

        //        strLogFilename = DateTime.Today.ToString("dd-MMM-yyyy") + "_abstract" + ".txt";
        //        if (File.Exists(Path.Combine(strFTPLogPath, strLogFilename)))
        //        {
        //            File.WriteAllText(strFTPLogPath + strLogFilename, "");
        //            sw = File.AppendText(strFTPLogPath + strLogFilename);
        //        }
        //        else
        //        {
        //            var logFile = new FileStream(strFTPLogPath + strLogFilename, FileMode.OpenOrCreate | FileMode.Append, FileAccess.Write);
        //            logFile.Close();
        //            sw = new StreamWriter(strFTPLogPath + strLogFilename, true);

        //        }


        //        using (var client = new SftpClient(host, port, username, password))
        //        {
        //            client.Connect();
        //            client.ChangeDirectory(workingdirectory);
        //            IEnumerable<SftpFile> enumFiles = client.ListDirectory(workingdirectory);
        //            sw.WriteLine("FileName|FileSize_in_Bytes|LastModifiedTime");
        //            foreach (SftpFile fl in enumFiles)
        //            {
        //                if (fl.Name == "." || fl.Name == "..")
        //                    continue;
        //                strFtpFilename = fl.Name;
        //                strFtpFileSize = Convert.ToString(fl.Length);
        //                strFtpFileModTime = fl.LastWriteTime.ToString("dd-MM-yyyy hh:mm:ss tt");
        //                sw.WriteLine(strFtpFilename + "|" + strFtpFileSize + "|" + strFtpFileModTime);
        //            }
        //        }
        //        sw.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        //clsException.InfyErrorLog("clsSFTP.cs", "ListSFTPDirContents", ex.Message, ex.StackTrace, "INFYUPLOAD", "");
        //    }
        //}

        //public void WriteFileUploadLog(string strLogText)
        //{
        //    try
        //    {
        //        StreamWriter sw = null;
        //        strLogFilename = DateTime.Today.ToString("dd-MMM-yyyy") + ".txt";
        //        if (File.Exists(Path.Combine(strFTPLogPath, strLogFilename)))
        //        {
        //            sw = File.AppendText(strFTPLogPath + strLogFilename);
        //        }
        //        else
        //        {
        //            var logFile = new FileStream(strFTPLogPath + strLogFilename, FileMode.OpenOrCreate | FileMode.Append, FileAccess.Write);
        //            logFile.Close();
        //            sw = new StreamWriter(strFTPLogPath + strLogFilename, true);
        //        }
        //        sw.WriteLine(strLogText);
        //        sw.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        //clsException.InfyErrorLog("clsSFTP.cs", "WriteFileUploadLog", ex.Message, ex.StackTrace, "INFYUPLOAD", "");
        //    }
        //}

    }
}
