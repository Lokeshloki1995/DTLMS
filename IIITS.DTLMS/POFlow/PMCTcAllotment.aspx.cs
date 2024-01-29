using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Data.OleDb;
using System.Collections;
using OfficeOpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Reflection;
using IIITS.DTLMS.BL.POFlow;

namespace IIITS.DTLMS.MasterForms
{
    public partial class PMCTcAllotment : System.Web.UI.Page
    {
        clsSession objSession;
        string sUserName = Convert.ToString(ConfigurationManager.AppSettings["FTP_USER"]);
        string sPassword = Convert.ToString(ConfigurationManager.AppSettings["FTP_PASS"]);

        string SFTPPath = Convert.ToString(ConfigurationManager.AppSettings["SFTPPath"]);
        string SFTPmainfolder = Convert.ToString(ConfigurationManager.AppSettings["SFTPmainfolder"]);

        /// <summary>
        /// This Function used in page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
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
                    LoadSearchWindow();
                }
            }
        }

        /// <summary>
        /// This function used to bind the uploaded di documents
        /// </summary>
        /// <param name="FtpServer"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public DataTable BindgridView(string FtpServer, string username, string password)
        {
            DataTable dtFiles = new DataTable();
            try
            {
                string SFTPPath = Convert.ToString(ConfigurationManager.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(ConfigurationManager.AppSettings["SFTPmainfolder"]);

                string DiNo = Regex.Replace(txtDINumber.Text, @"[^0-9a-zA-Z]+", "");
                FtpServer += "PMC_DI_DOCS/" + DiNo;
                clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);

                //path for get files from ftp
                bool IsExists = objFtp.FtpDirectoryExists(FtpServer);
                // checking related ponumber directory is there are not!
                if (IsExists == false)
                {
                    gvFiles.Visible = false;
                    return dtFiles;
                }
                else
                {
                    dtFiles = objFtp.GetListOfFiles(FtpServer);
                }
                gvFiles.DataSource = dtFiles;
                gvFiles.DataBind();

                return dtFiles;

            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtFiles;
            }
        }
        /// <summary>
        /// This function used to display pop up message
        /// </summary>
        /// <param name="sMsg"></param>
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
        /// <summary>
        /// This function used to redirect PMCTcAllotmentView page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdClose_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("PMCTcAllotmentView.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// This function used to call and get the di details and bind it in grid  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            try
            {

                clsPMCAllotment obj = new clsPMCAllotment();
                string temp = txtDINumber.Text;
                txtDINumber.Text = temp.ToUpper();
                obj.sDINo = temp.ToUpper();
                obj.GetDispatchCount(obj);

                if ((obj.Dwavalidupto ?? "").Length > 0 && (obj.Lecvalidupto ?? "").Length > 0)
                {
                    string sResult = Genaral.DateComparision(obj.Dwavalidupto, "", true, false);
                    if (sResult == "2" || obj.Dwastatus == "D")
                    {
                        string msg = "DWA date got expired for DWA Number as " + obj.DwaNumber
                            + ", Please extend the DWA expiry Date or The DWA No is Inactive.";

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID, "alert('" + msg + "');", true);
                        return;

                    }
                    string LecResult = Genaral.DateComparision(obj.Lecvalidupto, "", true, false);
                    if (LecResult == "2" || obj.Lecstatus == "D")
                    {

                        string msg = "LEC date got expired for LEC Name as " + obj.LecNumber
                            + ", Please extend the LEC expiry Date or The LEC No is Inactive.";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID, "alert('" + msg + "');", true);
                        return;
                    }
                    if ((obj.Poavailableamount ?? "").Length == 0)
                    {
                        obj.Poavailableamount = obj.Poamount;
                    }
                }
                txtPoamount.Text = obj.Poamount;
                txtPoNumber.Text = obj.Ponumber;
                txtAvailableAmt.Text = obj.Poavailableamount;


                BindgridView(SFTPmainfolder, sUserName, sPassword);
                DataTable dt = new DataTable();
                dt = obj.GetPMCDeliveryDetails(obj);
                if (dt.Rows.Count > 0)
                {
                    object sumObject;
                    sumObject = dt.Compute("Sum(pmc_di_quantity)", string.Empty);
                    txtTotalQuantity.Text = Convert.ToString(sumObject);
                    ViewState["PMC_TOTALTC"] = dt;
                }
                else
                {
                    string msg = "Please Select Valid DI Number";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID, "alert('" + msg + "');", true);
                }
                grdDIPendingTC.DataSource = dt;
                grdDIPendingTC.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// This function used to get the di numbers in serach field
        /// </summary>
        public void LoadSearchWindow()
        {
            try
            {
                string strQry = string.Empty;
                strQry = "Title=Search and Select  Dispatch Instructions Details&";
                strQry += "Query=SELECT  \"PMC_DIM_DI_NO\" AS \"DI NO\"   FROM \"TBLPMC_DELIVERYINSTMASTER\" ";
                strQry += "WHERE {0} like %{1}%  group by \"PMC_DIM_DI_NO\" &";
                strQry += "DBColName=\"PMC_DIM_DI_NO\" &";
                strQry += "ColDisplayName=DI Number &";
                strQry = strQry.Replace("'", @"\'");
                cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry
                    + "tb=" + txtDINumber.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + txtDINumber.ClientID + ")");

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// This function used to view di uploaded file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DownloadFile(object sender, EventArgs e)
        {

            string fileName = (sender as LinkButton).CommandArgument;

            try
            {
                string SFTPmainfolderpath = Convert.ToString(ConfigurationManager.AppSettings["VirtualDirectoryPMCDocs"]);

                string DINo = Regex.Replace(txtDINumber.Text, @"[^0-9a-zA-Z]+", "");

                clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);

                string path = SFTPmainfolderpath + "PMC_DI_DOCS/" + DINo + "/" + fileName;
                RegisterStartupScript("Print", "<script>window.open('" + path + "','_blank')</script>");

            }
            catch (WebException ex)
            {

            }

        }
        /// <summary>
        /// This function used to download di uploaded document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DownloadFiledwnld(object sender, EventArgs e)
        {
            string fileName1 = (sender as LinkButton).CommandArgument;
            try
            {
                //Create a stream for the file
                Stream stream = null;

                //This controls how many bytes to read at a time and send to the client
                int bytesToRead = 10000;

                // Buffer to read bytes in chunk size specified above
                byte[] buffer = new Byte[bytesToRead];

                // The number of bytes read
                try
                {
                    string SFTPmainfolderpath = Convert.ToString(ConfigurationManager.AppSettings["VirtualDirectoryPMCDocs"]);

                    string DINo = Regex.Replace(txtDINumber.Text, @"[^0-9a-zA-Z]+", "");
                    clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);

                    string url = SFTPmainfolderpath + "PMC_DI_DOCS/" + DINo + "/" + fileName1;
                    string fileName = getFilename(url);
                    //Create a WebRequest to get the file
                    HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(url);

                    //Create a response for this request
                    HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();

                    if (fileReq.ContentLength > 0)
                        fileResp.ContentLength = fileReq.ContentLength;

                    //Get the Stream returned from the response
                    stream = fileResp.GetResponseStream();

                    // prepare the response to the client. resp is the client Response
                    var resp = HttpContext.Current.Response;

                    //Indicate the type of data being sent
                    resp.ContentType = "application/octet-stream";

                    //Name the file 
                    resp.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
                    resp.AddHeader("Content-Length", fileResp.ContentLength.ToString());

                    int length;
                    do
                    {
                        // Verify that the client is connected.
                        if (resp.IsClientConnected)
                        {
                            // Read data into the buffer.
                            length = stream.Read(buffer, 0, bytesToRead);

                            // and write it out to the response's output stream
                            resp.OutputStream.Write(buffer, 0, length);

                            // Flush the data
                            resp.Flush();

                            //Clear the buffer
                            buffer = new Byte[bytesToRead];
                        }
                        else
                        {
                            // cancel the download if client has disconnected
                            length = -1;
                        }
                    } while (length > 0); //Repeat until no data is read
                }
                finally
                {
                    if (stream != null)
                    {
                        //Close the input stream
                        stream.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("(404) Not Found"))
                {
                    ShowMsgBox("File Not Found");
                }
                else
                {
                    lblMessage.Text = clsException.ErrorMsg();
                    clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                }

            }

        }
        /// <summary>
        /// This function used to get the file name
        /// </summary>
        /// <param name="hreflink"></param>
        /// <returns></returns>
        private string getFilename(string hreflink)
        {
            Uri uri = new Uri(hreflink);

            string filename = System.IO.Path.GetFileName(uri.LocalPath);

            return filename;
        }

        protected void grdDIPendingTC_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdDIPendingTC.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["PMC_TOTALTC"];
                grdDIPendingTC.DataSource = SortDataTable(dt as DataTable, true);
                grdDIPendingTC.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// Uploaded di documents page indexing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFiles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvFiles.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["dt"];
                gvFiles.DataSource = SortDataTable(dt as DataTable, true);
                gvFiles.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// Sort the di documents in grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFiles_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = gvFiles.PageIndex;
            DataTable dt = (DataTable)ViewState["dt"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                gvFiles.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                gvFiles.DataSource = dt;
            }
            gvFiles.DataBind();
            gvFiles.PageIndex = pageIndex;
        }
        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "ASC"; }
            set { ViewState["SortDirection"] = value; }


        }
        private string GridViewSortExpression
        {
            get { return ViewState["SortExpression"] as string ?? string.Empty; }
            set { ViewState["SortExpression"] = value; }
        }

        private string GetSortDirection()
        {
            switch (GridViewSortDirection)
            {
                case "ASC":
                    GridViewSortDirection = "DESC";

                    break;
                case "DESC":
                    GridViewSortDirection = "ASC";

                    break;
            }


            return GridViewSortDirection;
        }

        /// <summary>
        /// This function used to sort the grid
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="isPageIndexChanging"></param>
        /// <returns></returns>
        protected DataView SortDataTable(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1} ", GridViewSortExpression, GridViewSortDirection);

                        if (Convert.ToString(dataView.Sort) == "Name ASC ")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("Name")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["dt"] = dataView.ToTable();
                        }
                        else if (Convert.ToString(dataView.Sort) == "Name DESC ")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("Name")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["dt"] = dataView.ToTable();
                        }

                        else
                        {
                            ViewState["dt"] = dataView.ToTable();
                        }
                    }
                    else
                    {

                        dataView.Sort = string.Format("{0} {1} ", GridViewSortExpression, GetSortDirection());

                        if (Convert.ToString(dataView.Sort) == "Name ASC ")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("Name")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["dt"] = dataView.ToTable();
                        }
                        else if (Convert.ToString(dataView.Sort) == "Name DESC ")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("Name")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["dt"] = dataView.ToTable();
                        }



                        else
                        {
                            ViewState["dt"] = dataView.ToTable();
                        }
                    }
                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }

        /// <summary>
        /// This function used to download particular di excel sheet to fill and upload.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DownloadDiRecords(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            clsPMCAllotment objDi = new clsPMCAllotment();

            LinkButton lnkdwn = (LinkButton)sender;
            GridViewRow rw = (GridViewRow)lnkdwn.NamingContainer;
            objDi.sDino = ((Label)rw.FindControl("lblDI")).Text;
            objDi.sDimno = ((Label)rw.FindControl("lblDINO")).Text;
            objDi.sMake = ((Label)rw.FindControl("lblMake")).Text;
            objDi.sStorename = ((Label)rw.FindControl("lblstore")).Text;
            objDi.sCapacity = ((Label)rw.FindControl("lblCapacity")).Text;
            objDi.sRating = ((Label)rw.FindControl("lblRating")).Text;
            objDi.sTotqty = ((Label)rw.FindControl("lblQuantity")).Text;
            objDi.sStartrange = ((Label)rw.FindControl("lblstartrange")).Text.Replace("HP", "");
            objDi.sEndrange = ((Label)rw.FindControl("lblendrange")).Text.Replace("HP", "");
            objDi.sMakeId = ((Label)rw.FindControl("lblMakeId")).Text;
            objDi.sDimDino = ((Label)rw.FindControl("lbldimid")).Text;
            objDi.sPoid = ((Label)rw.FindControl("lblpoid")).Text;

            dt = objDi.PrintDi(objDi);

            if (dt.Rows.Count > 0)
            {
                Export_Excel_template(dt, objDi);

                string[] arrAlpha = Genaral.getalpha();

                string sMergeRange = arrAlpha[dt.Columns.Count - 1];
                string excelPath = Server.MapPath("~/Excel/Allotment_Excel.xlsx") + Path.GetFileName(fupdDoc.PostedFile.FileName);
            }
            else
            {
                ShowMsgBox("No Records Found");
            }



        }

        /// <summary>
        /// This function used to upload excel doc 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="objDtrUpload"></param>
        protected void FTPUpload(object sender, EventArgs e, clsPMCAllotment objDtrUpload)
        {
            string strUpload = string.Empty;
            try
            {
                string SFTPPath = Convert.ToString(ConfigurationManager.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(ConfigurationManager.AppSettings["SFTPmainfolder"]);
                string sFtpvirtual = Convert.ToString(ConfigurationManager.AppSettings["VirtualDirectoryDocs"]);

                DateTime currentDateTime = DateTime.Now;

                string fileName = (Path.GetFileName(fupdDoc.FileName)).Trim();

                objDtrUpload.sfilename = fileName;

                if (fileName == "" || fileName == null)
                {
                    ShowMsgBox("Please select the File!");
                    return;
                }

                string sFileExt = Convert.ToString(ConfigurationManager.AppSettings["UploadFormat"]);
                string sAnxFileExt = System.IO.Path.GetExtension(fupdDoc.FileName).ToString().ToLower();
                sAnxFileExt = ";" + sAnxFileExt.Remove(0, 1) + ";";
                clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);
                bool Isuploaded;
                string sMainFolderName = SFTPmainfolder + "/" + "PMC_ALLOTEMENT_UPLOAD_DOCS/";

                fupdDoc.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + fupdDoc.FileName));
                string sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + fupdDoc.FileName);
                Session["fupdDoc"] = fileName;

                if (File.Exists(sDirectory))
                {

                    bool IsExists = objFtp.FtpDirectoryExists(sMainFolderName);
                    if (IsExists == false)
                    {
                        objFtp.createDirectory(sMainFolderName);
                    }

                    Isuploaded = objFtp.upload(sMainFolderName, fileName, sDirectory);


                    if (Isuploaded == true & File.Exists(sDirectory))
                    {
                        objDtrUpload.sFilepath = SFTPmainfolder + "PMC_ALLOTEMENT_UPLOAD_DOCS/" + fileName;
                        File.Delete(sDirectory);
                        sDirectory = fileName;
                        ShowMsgBox("File Uploaded Successfully");
                        strUpload = "File:" + fileName + " Is Uploaded";
                        ViewState["Path"] = fileName;
                        lblmsg.Text = strUpload;
                        return;

                    }

                }
            }
            catch (WebException ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }
        /// <summary>
        /// This function will once click on upload
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdUpload_click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            clsPMCAllotment objDtrUpload = new clsPMCAllotment();
            clsPMCAllotment objDIS = new clsPMCAllotment();
            try
            {
                string dino = string.Empty;
                string diid = string.Empty;

                bool AtleastOneApp = false;
                int i = 0;
                string[] Arr = new string[1];
                //grdDIPendingTC.AllowPaging = false;
                grdDIPendingTC.AllowPaging = true;
                PopulateCheckedValues();
                string[] strQryVallist = new string[grdDIPendingTC.Rows.Count];
                string[] strDispatch = new string[grdDIPendingTC.Rows.Count];
                foreach (GridViewRow row in grdDIPendingTC.Rows)
                {

                    if (((CheckBox)row.FindControl("chkSelect")).Checked == true)
                    {
                        strQryVallist[i] = ((Label)row.FindControl("lblDI")).Text.Trim();
                        AtleastOneApp = true;
                        //txtPONo.Text = ((Label)row.FindControl("lblPONo")).Text.Trim();
                        strDispatch[i] = ((Label)row.FindControl("lblDINO")).Text.Trim();

                        diid = ((Label)row.FindControl("lblDI")).Text.Trim();
                        dino = ((Label)row.FindControl("lblDINO")).Text.Trim();

                    }
                    i++;

                }
                if (!AtleastOneApp)
                {
                    ShowMsgBox("Please Select Dispatch Number");
                    PopulateCheckedValues();
                    return;
                }

                if (fupdDoc.PostedFile.ContentLength == 0)
                {
                    ShowMsgBox("Please Select the File");
                    fupdDoc.Focus();
                    return;
                }



                string excelPath = Server.MapPath("~/DTLMSDocs/") + objSession.UserId + '~' + Path.GetFileName(fupdDoc.PostedFile.FileName);
                fupdDoc.SaveAs(excelPath);
                string conString = string.Empty;
                string FileName = Path.GetFileName(fupdDoc.PostedFile.FileName);
                string extension = Path.GetExtension(fupdDoc.PostedFile.FileName);
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
                DataTable dtExcelData1 = new DataTable();
                using (OleDbConnection excel_con = new OleDbConnection(conString))
                {
                    try
                    {

                        excel_con.Open();

                        string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();

                        using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", excel_con))
                        {
                            oda.Fill(dtExcelData1);

                        }
                        dt = dtExcelData1;
                        ViewState["PMCAllotment"] = dtExcelData1;
                        double PoTotalAmount = Convert.ToDouble(txtPoamount.Text);
                        double PoAvailableamt = Convert.ToDouble(txtAvailableAmt.Text);
                        objDIS = objDIS.ValidateExcelSheet(dt, diid, dino, PoAvailableamt);

                        if (objDIS.Tcamountstatus < 0)
                        {
                            ShowMsgBox("Total DTr Allocation Amount Exceeding PO available amount.");
                            return;
                        }

                        if (objDIS.statusId > 0)
                        {
                            ShowMsgBox("Please Choose Valid File.");
                            return;
                        }

                        if (objDIS.statusId < 0)
                        {
                            PopulateCheckedValues();
                            MemoryStream ms = new MemoryStream();
                            TextWriter tw = new StreamWriter(ms);
                            tw.WriteLine(objDIS.Validation.ToString());
                            tw.Flush();
                            byte[] bytes = ms.ToArray();
                            ms.Close();
                            Response.Clear();
                            Response.ContentType = "application/force-download";
                            Response.AddHeader("content-disposition", "attachment;    filename=Errorfile.txt");
                            Response.BinaryWrite(bytes);
                            Response.End();

                        }
                        else
                        {
                            ShowMsgBox("File Uploaded Successfully");
                            cmdsave.Enabled = true;
                            cmdupload.Enabled = false;

                        }

                    }
                    catch (Exception ex)
                    {
                        ShowMsgBox("Please Choose Valid File.");
                        excel_con.Close();
                        System.IO.File.Delete(excelPath);
                        return;
                    }
                }
                FTPUpload(sender, e, objDtrUpload);
                DateTime currentDateTime = DateTime.Now;
                string sFileExt = Convert.ToString(ConfigurationManager.AppSettings["UploadFormat"]);
                string sUploadFileExt = System.IO.Path.GetExtension(fupdDoc.FileName).ToString().ToLower();
                sUploadFileExt = ";" + sUploadFileExt.Remove(0, 1) + ";";



            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                ShowMsgBox("Something went Wrong Please Try again....!!!");

            }
        }
        /// <summary>
        /// This function used to check the selected di
        /// </summary>
        private void PopulateCheckedValues()
        {
            try
            {
                ArrayList arrCheckedValues = (ArrayList)ViewState["CHECKED_ITEMS"];
                if (arrCheckedValues != null && arrCheckedValues.Count > 0)
                {
                    foreach (GridViewRow gvrow in grdDIPendingTC.Rows)
                    {
                        int index = Convert.ToInt32(grdDIPendingTC.DataKeys[gvrow.RowIndex].Values[0]);
                        if (arrCheckedValues.Contains(index))
                        {
                            CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkSelect");
                            myCheckBox.Checked = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }
        /// <summary>
        /// this function used to save the allotment details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdsave_click(object sender, EventArgs e)
        {
            DataTable dtExcelData1 = new DataTable();
            clsPMCAllotment objDtrUpload = new clsPMCAllotment();
            DataTable dt = new DataTable();
            string sPath = Convert.ToString(ViewState["Path"]);


            if (sPath == "")
            {
                ShowMsgBox("Please Upload File");
                return;
            }

            dtExcelData1 = (DataTable)ViewState["PMCAllotment"];
            dt = dtExcelData1;
            foreach (GridViewRow row in grdDIPendingTC.Rows)
            {
                if (((CheckBox)row.FindControl("chkSelect")).Checked == true)
                {
                    objDtrUpload.sTotqty = ((Label)row.FindControl("lblQuantity")).Text.Trim();
                }
            }
            objDtrUpload.Poamount = txtPoamount.Text;
            objDtrUpload.Poavailableamount = txtAvailableAmt.Text;
            objDtrUpload.Ponumber = txtPoNumber.Text;

            if (dtExcelData1.Rows.Count > 0)
            {
                bool result = objDtrUpload.SaveAllotmentUploadDetails(dt, sPath, objDtrUpload);
                if (result == true)
                {
                    ViewState["Path"] = null;
                    cmdupload.Enabled = true;
                    cmdsave.Enabled = false;
                    string msg = "DTr Allocated Successfully";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID, "alert('"
                        + msg + "'); location.href='PMCTcAllotmentView.aspx';", true);

                }
                else
                {
                    ShowMsgBox("Something Went Wrong");
                    cmdsave.Enabled = true;
                }

            }

        }

        /// <summary>
        /// This function will download the particualar di details for allotment
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="objDi"></param>
        public void Export_Excel_template(DataTable dt, clsPMCAllotment objDi)
        {
            string ExcelPath = Server.MapPath("~") + "Excel\\PMC_ALLOTMENT_TEMPLATE.xlsx";
            MemoryStream ms = new MemoryStream();
            FileInfo file = new FileInfo(ExcelPath);
            ExcelPackage xlPackage = new ExcelPackage(file);

            try
            {
                using (ExcelPackage excelPackage = new ExcelPackage(file))
                {
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    ExcelWorkbook excelWorkBook = excelPackage.Workbook;
                    ExcelWorksheet excelWorksheet = excelWorkBook.Worksheets["DtrReport"];


                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            excelWorksheet.Column(j + 1).Width = 15;
                            excelWorksheet.Cells[i + 2, j + 1].Value = Convert.ToString(dt.Rows[i][j]);
                        }
                    }
                    try
                    {
                        //Export Excel- download excel
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename="
                            + objDi.sStorename + "_PMC_" + objDi.sDimno + "_" + DateTime.Now + ".xlsx");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            excelPackage.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
            }


            catch (Exception ex)
            {
                if (!ex.Message.Contains("Thread was being aborted"))
                {
                    lblMessage.Text = clsException.ErrorMsg();
                    clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                }

            }
        }
        /// <summary>
        /// This function used once click on reset button it will redirect to page load 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdreset_click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);

        }

        /// <summary>
        /// This function purpose is to check the every di number indent status 
        /// If indent status is 1 then disabling radiobutton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDIPendingTC_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    clsPMCAllotment objCheckIndentstatus = new clsPMCAllotment();
                    RadioButton rsbtcchecked = (RadioButton)e.Row.FindControl("chkSelect");
                    objCheckIndentstatus.sDiId = ((Label)e.Row.FindControl("lblDI")).Text;
                    objCheckIndentstatus.sStartrange = ((Label)e.Row.FindControl("lblstartrange")).Text;
                    objCheckIndentstatus.sEndrange = ((Label)e.Row.FindControl("lblendrange")).Text;

                    DataTable DTindentstatus = objCheckIndentstatus.CheckIndentstatus(objCheckIndentstatus);
                    if (DTindentstatus.Rows.Count > 0)
                    {
                        rsbtcchecked.Enabled = false;
                    }

                }
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