using IIITS.DTLMS.BL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
 
using System.Web.UI;
using System.Web.UI.WebControls;
namespace IIITS.DTLMS.DashboardForm
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        
            string strFormCode="TutorialVideos";

            clsSession objSession;
            string localpath = @"E:\\Rudresh\";

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
                        //  hfVideoFile.Value = "file://192.168.4.21/trm-ftp/DTLMS_INTERNAL_TRAINING/Circulars/SampleVideo_1280x720_10mb.mp4";

                        objSession = (clsSession)Session["clsSession"];
                        //FTP Server URL.
                        //string ftp = "ftp://192.168.4.21/trm-ftp/DTLMS_INTERNAL_TRAINING/";

                        ////FTP Folder name. Leave blank if you want to list files from root folder.
                        //string ftpFolder = "Circulars/";




                        //    try
                        //    {
                        //        //Create FTP Request.
                        //        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftp + ftpFolder);
                        //        request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                        //        //Enter FTP Server credentials.
                        //        request.Credentials = new NetworkCredential("FTP_USER", "idea@2017");
                        //        request.UsePassive = true;
                        //        request.UseBinary = true;
                        //        request.EnableSsl = false;

                        //        //Fetch the Response and read it using StreamReader.
                        //        FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                        //        List<string> entries = new List<string>();
                        //        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        //        {
                        //            //Read the Response as String and split using New Line character.
                        //            entries = reader.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        //        }
                        //        response.Close();

                        //        //Create a DataTable.
                              //  DataTable dtFiles = new DataTable();
                        //        dtFiles.Columns.AddRange(new DataColumn[3] { new DataColumn("Name", typeof(string)),
                        //                                            new DataColumn("Size", typeof(decimal)),
                        //                                            new DataColumn("Date", typeof(string))});

                        //        //Loop and add details of each File to the DataTable.
                        //        foreach (string entry in entries)
                        //        {
                        //            string[] splits = entry.Split(new string[] { " ", }, StringSplitOptions.RemoveEmptyEntries);

                        //            //Determine whether entry is for File or Directory.
                        //            bool isFile = splits[0].Substring(0, 1) != "d";
                        //            bool isDirectory = splits[0].Substring(0, 1) == "d";

                        //            //If entry is for File, add details to DataTable.
                        //            if (isFile)
                        //            {
                        //                dtFiles.Rows.Add();
                        //                dtFiles.Rows[dtFiles.Rows.Count - 1]["Size"] = decimal.Parse(splits[4]) / 1024;
                        //                dtFiles.Rows[dtFiles.Rows.Count - 1]["Date"] = string.Join(" ", splits[5], splits[6], splits[7]);
                        //                string name = string.Empty;
                        //                for (int i = 8; i < splits.Length; i++)
                        //                {
                        //                    name = string.Join(" ", name, splits[i]);
                        //                }
                        //                dtFiles.Rows[dtFiles.Rows.Count - 1]["Name"] = name.Trim();
                        //            }


                        //            Bind the GridView.
                        //            gvFiles.DataSource = dtFiles;
                        //            gvFiles.DataBind();
                        //        }

                        //    }
                        //    catch (WebException ex)
                        //    {
                        //        throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
                        //    }

                        //}

                       // string FtpServer = "ftp://192.168.4.21/DTLMS_INTERNAL_TRAINING/Circulars/";
                       // string username = "FTP_USER";
                       // string password = "Idea@2017";
                       //// string localpath = @"E:\\Rudresh\";
                       // DownloadFile(FtpServer, username, password );
                       // //BindData();
                        string filename = "SampleVideo_720x480_20mb";
                        string ext = "mp4";

                        videoStream(filename,ext);
                      
                        
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                    clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                }
            }



            //protected void vdo_click1(object sender, EventArgs e)
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

            //        sFTPLink = ConfigurationSettings.AppSettings["TutorialsVideoPath"].ToString();

            //        //sFTPLink = "https://www.google.com";
            //        clsFtp objFtp = new clsFtp(sFTPLink, sFTPUserName, sFTPPassword);



            //        string sFilename = "SampleVideo_1280x720_1mb";

            //        //imgDTrCode.ImageUrl = "ftp://" + sFTPUserName + ":" + sFTPPassword + "@" + sFTPLink.Remove(0, 6) + hdfDTRImagePath.Value;
            //        lnkbtnVDO1.PostBackUrl = sFTPLink + sFilename;



                    //     // create instance of video reader
                    //  VideoFileReader reader = new VideoFileReader( );
                    //    reader.Open( "test.avi" );
                    //// read 100 video frames out of it
                    //for ( int i = 0; i < 100; i++ )
                    //  {
                    //   Bitmap videoFrame = reader.ReadVideoFrame( );
                    //   videoFrame.Save(i + ".bmp");
                    // // dispose the frame when it is no longer required
                    // videoFrame.Dispose( );
                    //    }
                    //     reader.Close( );



                    //Source_id.src = lnkbtnVDO1;



            //    }
            //    catch (Exception ex)
            //    {
            //       lblMessage.Text= ex.Message;
            //    }
            //}
      //   Video.Attributes.Add("src","E:\\Rudresh\SampleVideo_720x480_30mb.mp4");
            //protected void DownloadFile1(object sender, EventArgs e)
            //{
            //    string FtpServer = "ftp://192.168.4.21/DTLMS_INTERNAL_TRAINING/Circulars/";
            //    string ftpPlay="file://192.168.4.21/trm-ftp/DTLMS_INTERNAL_TRAINING/Circulars/";

            //    string username = "FTP_USER";
            //    string password = "Idea@2017";
            //    string localpath = @"E:\\Rudresh\";
            //    WebRequest request = (WebRequest)WebRequest.Create(FtpServer);
            //    request.Credentials = new NetworkCredential(username, password);
            //    request.Method = WebRequestMethods.Ftp.ListDirectory;
            //    StreamReader streamReader = new StreamReader(request.GetResponse().GetResponseStream());
            //    string fileName3 = streamReader.ReadLine();
            //    List<string> directories = new List<string>();




            //    string filePath = (sender as LinkButton).CommandArgument;

            //    while (fileName3 != null && fileName3 != "testing12")
            //    {


            //        if (fileName3.Equals(filePath))
            //        {
            //            using (WebClient ftpClient = new WebClient())
            //            {
            //                ftpClient.Credentials = new System.Net.NetworkCredential(username, password);

            //                for (int i = 0; i <= directories.Count - 1; i++)
            //                {
            //                    if (directories[i].Contains("."))
            //                    {

            //                        source.Src = ftpPlay + Request.QueryString["id"] + fileName3.ToString();
            //                         //source= FtpServer + fileName3.ToString();
            //                     //   string path = FtpServer + fileName3.ToString();
            //                     //   string trnsfrpth = localpath + fileName3.ToString();
            //                      //  ftpClient.DownloadFile(path, trnsfrpth);
            //                      //  Response.ContentType = ContentType;
            //                      //  Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(trnsfrpth));
            //                      //  Response.WriteFile(trnsfrpth);
                         
            //                       Response.End();
            //                  }                  
            //              }                   
            //           }
            //       }
            //        directories.Add(fileName3);
            //        fileName3 = streamReader.ReadLine();
            //   }
                
            //}

        //public DataTable DownloadFile(string FtpServer, string username, string password )
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
                    
        //            directories.Add(fileName3);
        //            fileName3 = streamReader.ReadLine();
        //            dtFiles.Rows.Add(fileName3);

        //        }
             
        //        streamReader.Close();


        //        using (WebClient ftpClient = new WebClient())
        //        {
        //            ftpClient.Credentials = new System.Net.NetworkCredential(username, password);

        //            for (int i = 0; i <= directories.Count - 1; i++)
        //            {
        //                if (directories[i].Contains("."))
        //                {

        //                    string path = FtpServer + directories[i].ToString();
        //                    //string trnsfrpth = localpath + directories[i].ToString();
        //                  //  ftpClient.DownloadFile(path, trnsfrpth);
        //                    gvFiles.DataSource = dtFiles;
        //                      gvFiles.DataBind();
        //                }
        //            }
        //        }
        //        return dtFiles;
            
         
   
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = ex.Message;
        //        return dtFiles;
        //    }
        //}
        string filename = "SampleVideo_720x480_30mb";
        string ext = "mp4";
   
        public  void videoStream(string filename, string ext)
        {

            _filename = @"E:\\Rudresh\" + filename + "." + ext;
        
        }

        public async void WriteStream(Stream outputstream, HttpContext context, TransportContext transcontent)
        {
            try
            {
                var buffer =new byte[65536];
                using (var video=File.Open(_filename,FileMode.Open,FileAccess.Read))
                {
                    var length=(int)video.Length;
                    var bytesRead=1;
                    while(length>0 && bytesRead >0)
                    {
                        bytesRead=video.Read(buffer,0,Math.Min(length,buffer.Length));
                        await outputstream.WriteAsync(buffer,0,bytesRead);
                        length=bytesRead;
                    }

                }
            }
                 
            catch(Exception ex)
            {
                Response.Write(ex.Message);
            }
            finally
            {
                outputstream.Close();
            }
        }


        protected string GetVideoLink()
        {
            return "UploadMovies/" + Request.QueryString["ID"] + "/SampleVideo_720x480_30mb.mp4";
        }
        //public void BindData()
        //{
        //    DataTable dtGetData = new DataTable();

        //    String ConnString = ConfigurationManager.ConnectionStrings["ConnectionName"].ConnectionString;
        //    PgSqlDataAdapter adapter = new PgSqlDataAdapter();

        //   PGSqlConnection conn = new PGSqlConnection();
            
        //        adapter.SelectCommand = new SqlCommand("select \"V_PATH\" from \"TBL_VDO\"", conn);
        //        adapter.Fill(dtGetData);
            

        //    gvFiles.DataSource = dtGetData;
        //    gvFiles.DataBind();
        //}






        public DataTable dtFiles { get; set; }
  
 
  
public  string _filename { get; set; }} 
}
