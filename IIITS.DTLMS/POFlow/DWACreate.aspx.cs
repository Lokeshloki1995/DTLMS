using IIITS.DTLMS.BL;
using IIITS.DTLMS.BL.POFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
using System.Configuration;
using System.Reflection;
using System.Globalization;
using Renci.SshNet;

namespace IIITS.DTLMS.POFlow
{
    public partial class DWACreate : System.Web.UI.Page
    {
        clsSession objSession;
        string sUserName = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["FTP_USER"]);
        string sPassword = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["FTP_PASS"]);

        string SFTPPath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SFTPPath"]);
        string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SFTPmainfolder"]);
      //  private readonly object divProjectName;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];
                // mdlPopupContainer.Visible = false;
                txtDWADate.Attributes.Add("readonly", "readonly");
                txtDWAPeriod.Attributes.Add("readonly", "readonly");
                txtExtendedUpTo.Attributes.Add("readonly", "readonly");
                txtAwardDate.Attributes.Add("readonly", "readonly");
                txtAward.Attributes.Add("readonly", "readonly");
                txtExtend.Attributes.Add("readonly", "readonly");

                if (!IsPostBack)
                {
                    string qry = "SELECT \"LM_ID\",\"LM_CONTRACTOR_NAME\" FROM \"TBLLECMASTER\" WHERE \"LM_STATUS\" = 'A' ORDER BY \"LM_ID\"";
                    Genaral.Load_Combo(qry, "--Select--", cmdIsChooseLicencedContractor);
                    Genaral.Load_Combo("select \"MD_ID\",\"MD_NAME\" from \"TBLMASTERDATA\"  WHERE \"MD_TYPE\"='PT' ORDER BY \"MD_ID\"",
                        "--Select--", cmdIsChooseProject);

                    if (Request.QueryString["QryDWAId"] != null && Request.QueryString["QryDWAId"].ToString() != "")
                    {
                        hdndwaid.Value = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryDWAId"]));
                        cmdSave.Visible = false;
                        ExtendedDiv.Visible = false;
                        cmdIsChooseLicencedContractor.Enabled = false;
                        txtDWADate.Enabled = false;
                        txtDWAPeriod.Enabled = false;
                        BindgridView(SFTPmainfolder, sUserName, sPassword);
                        txtAwardDate.Enabled = false;
                        txtAward.Enabled = false;
                        fupdDoc.Enabled = false;
                        // added on 12-09-2023 by santhosh.
                        string[] Arr = new string[2];
                        Arr = checkIsExtended(hdndwaid.Value);
                        if (Arr[0] == "0")
                        {
                            txtExtendedUpTo.Text = Arr[1];
                            ExtendedDiv.Visible = true;
                            txtExtendedUpTo.Enabled = false;
                        }
                        else
                        {
                            txtExtendedUpTo.Enabled = false;
                        }
                        //pnlfile.Enabled = false;
                        //fileup1.Visible = false;
                    }
                    else
                    {
                        ExtendedDiv.Visible = false;
                        cmdExtendDwa.Visible = false;
                        cmdUpdate.Visible = false;
                    }
                    if (hdndwaid.Value.Length > 0)
                    {
                        GetContractorDwaDetails();
                      
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


        protected void cmdIsChooseProject_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                if (cmdIsChooseProject.SelectedValue != null && cmdIsChooseProject.SelectedValue == "11")
                {
              
                    divProjectName.Style.Add("display", "block");
                }
                else
                {
                    divProjectName.Style.Add("display", "none");
                    txtProject.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();

                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }

      


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string[] checkIsExtended(string DWAId)
        {
            ClsDwa Obj = new ClsDwa();
            string[] Result = new string[2];
            try
            {
                Obj.DwaId = DWAId;
                Result = Obj.checkIsExtended(Obj);
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return Result;
        }
        public DataTable BindgridView(string FtpServer, string username, string password)
        {
            DataTable dtFiles = new DataTable();
            try
            {
                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SFTPmainfolder"]);

                string dwaNo = Regex.Replace(hdndwaid.Value, @"[^0-9a-zA-Z]+", "");
                FtpServer += "DWA_DOCS/" + dwaNo;
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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtFiles;
            }
        }
        protected void DownloadFile(object sender, EventArgs e)
        {

            string fileName = (sender as LinkButton).CommandArgument;

            try
            {
                bool status = false;
                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["VirtualPath"]);

                string PoNo = Regex.Replace(hdndwaid.Value, @"[^0-9a-zA-Z]+", "");
                clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);

                string path = SFTPmainfolder + "DWA_DOCS/" + PoNo + "/" + fileName;
                RegisterStartupScript("Print", "<script>window.open('" + path + "','_blank')</script>");

            }
            catch (WebException ex)
            {
                lblMessage.Text = clsException.ErrorMsg();

                //  throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }
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
                    string SFTPmainfolder = Convert.ToString(ConfigurationManager.AppSettings["VirtualPath"]);

                    string PoNo = Regex.Replace(hdndwaid.Value, @"[^0-9a-zA-Z]+", "");
                    clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);

                    string url = SFTPmainfolder + "DWA_DOCS/" + PoNo + "/" + fileName1;
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
        private string getFilename(string hreflink)
        {
            Uri uri = new Uri(hreflink);

            string filename = System.IO.Path.GetFileName(uri.LocalPath);

            return filename;
        }
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
                            // dataView.Sort = string.Format("{0} {1} ", GridViewSortExpression, GridViewSortDirection);
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
                            // dataView.Sort = string.Format("{0} {1} ", GridViewSortExpression, GetSortDirection());
                            ViewState["dt"] = dataView.ToTable(); ;
                        }
                        //dv.Sort = string.Format("{0} {1} ", GridViewSortExpression, GetSortDirection());
                    }
                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

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

       // public object divProjectName { get; private set; }

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
        /// For getting LEC Contarctor Details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GetContractorDetails(object sender, EventArgs e)
        {
            try
            {

                ClsDwa objContractor = new ClsDwa();
                objContractor.Id = cmdIsChooseLicencedContractor.SelectedValue;
                objContractor.GetContractorDetails(objContractor);

                txtLicenceNumber.Text = objContractor.LicenceNum;
                txtGSTNumber.Text = objContractor.GstNum;
                txtAddress.Text = objContractor.Address;
                txtLicenceRegDate.Text = objContractor.LicenceRegDate;
                txtLicenceValidUpTo.Text = objContractor.LicenceValidUpTo;
                txtContractorEmailId.Text = objContractor.MailId;
                txtContactNumber.Text = objContractor.Mobileno;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();

                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Save and Update DWA Award details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveAwardDetails(object sender, EventArgs e)
        {
            try
            {
                string[] Arr = new string[3];
                if (ValidateForm() == true)
                {

                    ClsDwa objSaveContractor = new ClsDwa();
                    objSaveContractor.Id = cmdIsChooseLicencedContractor.SelectedValue;

                    objSaveContractor.DWANumber = txtDWANumber.Text.Replace(" ", "").ToUpper();
                    objSaveContractor.DwaDate = txtDWADate.Text;
                    objSaveContractor.DwaPeriod = txtDWAPeriod.Text;
                    objSaveContractor.DwaExtendedUpTo = txtDWAPeriod.Text;
                    if (cmdIsChooseProject.SelectedValue == "11")
                    {
                        objSaveContractor.DwaProject = txtProject.Text;
                    }
                    objSaveContractor.DwaProjectType = cmdIsChooseProject.SelectedValue;
                    objSaveContractor.ProjectName = txtWork.Text.ToUpper().Replace("'", "");
                    objSaveContractor.AwardAmount = txtDWAAmt.Text;
                    objSaveContractor.UpBy = objSession.UserId;

                    objSaveContractor.DwaId = hdndwaid.Value;

                    Arr = objSaveContractor.SaveContractorDetails(objSaveContractor);

                    if (Arr[1].ToString() == "0")
                    {
                        if (objSaveContractor.DwaId == "" || objSaveContractor.DwaId == null)
                        {
                            objSaveContractor.DwaId = Arr[2].ToString();
                        }
                        uploadbudgetdocument(objSaveContractor);

                        objSaveContractor.DwaFilePath = txtdwafilepath.Text;

                        Arr = objSaveContractor.SaveContractorFilePath(objSaveContractor);
                        cmdReset.Enabled = false;
                        cmdSave.Enabled = false;
                        ShowMsgBox(Arr[0]);
                    }
                    if (Arr[1].ToString() == "1")
                    {
                        if ((hdndwaid.Value ?? "").Length == 0)
                        {
                            uploadbudgetdocument(objSaveContractor);

                            objSaveContractor.DwaFilePath = txtdwafilepath.Text;

                            Arr = objSaveContractor.SaveContractorFilePath(objSaveContractor);
                        }
                        cmdReset.Enabled = false;
                        ShowMsgBox(Arr[0]);
                    }
                    if(Arr[1].ToString() == "2")
                    {
                        cmdReset.Enabled = false;
                        ShowMsgBox(Arr[0]);
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();

                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// For getting Contractor and Dwa Award detils
        /// </summary>
        protected void GetContractorDwaDetails()
        {
            try
            {

                ClsDwa obGetContractorDwaDetails = new ClsDwa();
                obGetContractorDwaDetails.DwaId = hdndwaid.Value;
                Genaral.Load_Combo("SELECT \"LM_ID\",\"LM_CONTRACTOR_NAME\" FROM \"TBLLECMASTER\"  ORDER BY \"LM_ID\"",
                    "--Select--", cmdIsChooseLicencedContractor);

                obGetContractorDwaDetails.GetContractorDwaDetails(obGetContractorDwaDetails);

                cmdIsChooseLicencedContractor.SelectedValue = obGetContractorDwaDetails.Id;
                txtLicenceNumber.Text = obGetContractorDwaDetails.LicenceNum;
                txtGSTNumber.Text = obGetContractorDwaDetails.GstNum;
                txtAddress.Text = obGetContractorDwaDetails.Address;
                txtLicenceRegDate.Text = obGetContractorDwaDetails.LicenceRegDate;
                txtLicenceValidUpTo.Text = obGetContractorDwaDetails.LicenceValidUpTo;
                txtContractorEmailId.Text = obGetContractorDwaDetails.MailId;
                txtContactNumber.Text = obGetContractorDwaDetails.Mobileno;
                txtDWANumber.Text = obGetContractorDwaDetails.DWANumber;
                txtDWADate.Text = obGetContractorDwaDetails.DwaDate;
                txtDWAPeriod.Text = obGetContractorDwaDetails.DwaPeriod;
                txtExtendedUpTo.Text = obGetContractorDwaDetails.DwaExtendedUpTo;
              
                
               cmdIsChooseProject.SelectedValue = obGetContractorDwaDetails.DwaProjectType;
                if (cmdIsChooseProject.SelectedValue == "11")
                {

                    divProjectName.Style.Add("display", "block");
                    txtProject.Text = obGetContractorDwaDetails.DwaProject;
                }
                //  txtProject.Text = obGetContractorDwaDetails.DwaProject;
                // txtProject.Text = obGetContractorDwaDetails.DwaProject;


                txtWork.Text = obGetContractorDwaDetails.DwaWorkName;
                txtDWAAmt.Text = obGetContractorDwaDetails.AwardAmount;
                txtAwardDate.Text = obGetContractorDwaDetails.DwaDate;
                txtAward.Text = obGetContractorDwaDetails.DwaPeriod;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();

                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Save and Update DWA Extended Date and Document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveExtendDwaclick(object sender, EventArgs e)
        {
            try
            {
                string[] Arr = new string[2];
                string[] IsUplaodedResult = new string[2];

                if (ValidateExtendForm() == true)
                {
                    ClsDwa objSaveExtendDwa = new ClsDwa();
                    objSaveExtendDwa.DwaId = hdndwaid.Value;
                    objSaveExtendDwa.UpBy = objSession.UserId;

                    IsUplaodedResult = uploadExtendeddocument(objSaveExtendDwa);
                    if (IsUplaodedResult[0] == "0")
                    {
                        Arr = new string[2];
                        objSaveExtendDwa.DwaFilePath = txtDocumnetpath.Text;
                        objSaveExtendDwa.DwaExtendedUpTo = txtExtend.Text;
                        Arr = objSaveExtendDwa.SaveContractorFilePath(objSaveExtendDwa);
                        if (Arr[1].ToString() == "0")
                        {
                            Arr = objSaveExtendDwa.UpdateExtendedUpTo(objSaveExtendDwa);
                            if (Arr[1].ToString() == "1")
                            {
                                ShowMsgBox(Arr[0]);
                            }
                        }
                    }
                    else
                    {

                        ShowMsgBox(IsUplaodedResult[1]);
                        mdlPopup.Show();
                    }
                    BindgridView(SFTPmainfolder, sUserName, sPassword);
                    GetContractorDwaDetails();
                    ExtendedDiv.Visible = true;
                    return;
                }
                else
                {
                    ExtendedDiv.Visible = true;
                    mdlPopup.Show();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();

                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// For getting  Extending DWA Pop Up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Showmodelpopup(object sender, EventArgs e)
        {
            try
            {
                txtExtend.Text = string.Empty;
                mdlPopup.Show();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();

                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// For Displaying msg
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
        /// For Reset the data in form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                if ((hdndwaid.Value ?? "").Length > 0)
                {
                    txtDWANumber.Text = string.Empty;
                    txtWork.Text = string.Empty;
                    txtDWAAmt.Text = string.Empty;
                    cmdIsChooseProject.SelectedIndex = 0;
                }
                else
                {
                    cmdIsChooseLicencedContractor.SelectedIndex = 0;
                    txtLicenceNumber.Text = string.Empty;
                    txtGSTNumber.Text = string.Empty;
                    txtAddress.Text = string.Empty;
                    txtLicenceRegDate.Text = string.Empty;
                    txtLicenceValidUpTo.Text = string.Empty;
                    txtContractorEmailId.Text = string.Empty;
                    txtContactNumber.Text = string.Empty;
                    txtDWANumber.Text = string.Empty;
                    txtDWADate.Text = string.Empty;
                    txtDWAPeriod.Text = string.Empty;
                    txtWork.Text = string.Empty;
                    txtDWAAmt.Text = string.Empty;
                    cmdIsChooseProject.SelectedIndex = 0;
                    txtDWAPeriod.Text = string.Empty;
                    txtExtendedUpTo.Text = string.Empty;
                    txtProject.Text = string.Empty;

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();

                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        /// <summary>
        /// Validate the data in save time
        /// </summary>
        /// <returns></returns>
        bool ValidateForm()
        {
            bool bValidate = false;
            try
            {
                if (cmdIsChooseLicencedContractor.SelectedIndex == 0)
                {
                    cmdIsChooseLicencedContractor.Focus();
                    ShowMsgBox("Please Select Licenced Contractor");
                    return bValidate;
                }

                if (txtLicenceNumber.Text == "")
                {
                    txtLicenceNumber.Focus();
                    ShowMsgBox("Please Select Licenced Contractor");
                    return bValidate;
                }

                if (txtGSTNumber.Text == "")
                {
                    txtGSTNumber.Focus();
                    ShowMsgBox("Please Select Licenced Contractor");
                    return bValidate;
                }
                if (txtAddress.Text == "")
                {
                    txtAddress.Focus();
                    ShowMsgBox("Please Select Licenced Contractor");
                    return bValidate;
                }

                if (txtLicenceRegDate.Text == "")
                {
                    txtLicenceRegDate.Focus();
                    ShowMsgBox("Please Select Licenced Contractor");
                    return bValidate;
                }

                if (txtLicenceValidUpTo.Text == "")
                {
                    txtLicenceValidUpTo.Focus();
                    ShowMsgBox("Please Select Licenced Contractor");
                    return bValidate;
                }

                if (txtContractorEmailId.Text == "")
                {
                    txtContractorEmailId.Focus();
                    ShowMsgBox("Please Select Licenced Contractor");
                    return bValidate;
                }

                if (txtContactNumber.Text == "")
                {
                    txtContactNumber.Focus();
                    ShowMsgBox("Please Select Licenced Contractor");
                    return bValidate;
                }

                if (txtDWANumber.Text == "")
                {
                    txtDWANumber.Focus();
                    ShowMsgBox("Please Enter DWA Number");
                    return bValidate;
                }
                else if (txtDWANumber.Text != "")
                {
                    if ((hdndwaid.Value ?? "").Length == 0)
                    {
                        ClsDwa Obj = new ClsDwa();
                        Obj.DWANumber = txtDWANumber.Text.Replace(" ", "").ToUpper();

                        if (Obj.checkDWANumberExist(Obj))
                        {
                            txtDWANumber.Focus();
                            ShowMsgBox("DWA Number Already exist.");
                            return bValidate;
                        }
                    }


                }

                if (txtDWADate.Text == "")
                {
                    txtDWADate.Focus();
                    ShowMsgBox("Please Select DWA Date");
                    return bValidate;
                }

                if (txtDWAPeriod.Text == "")
                {
                    txtDWAPeriod.Focus();
                    ShowMsgBox("Please Select DWA Expire Date.");
                    return bValidate;
                }
                if (txtDWAPeriod.Text != "" && txtDWADate.Text != "")
                {
                    string Resultt = Genaral.DateComparision(txtDWAPeriod.Text, txtDWADate.Text, false, false);
                    if (Resultt == "2")
                    {
                        ShowMsgBox("DWA Expire Date Should be Greater Than Or Equal To DWA Date.");
                        return bValidate;
                    }
                }

                if (cmdIsChooseProject.SelectedIndex == 0)
                {
                    cmdIsChooseProject.Focus();
                    ShowMsgBox("Please Select Project.");
                    return bValidate;
                }

                if (cmdIsChooseProject.SelectedValue == "11")
                {

                    if (txtProject.Text == "")
                    {
                        txtProject.Focus();
                        ShowMsgBox("Please Enter Name of the Project");
                        return bValidate;
                    }
                }

                if (txtWork.Text == "")
                {
                    txtWork.Focus();
                    ShowMsgBox("Please Enter Name Of the Work");
                    return bValidate;
                }
                
                if (txtDWAAmt.Text == "")
                {
                    txtDWAAmt.Focus();
                    ShowMsgBox("Please Enter DWA Amount");
                    return bValidate;
                }


                else
                {
                    if (txtDWAAmt.Text.Trim().StartsWith("."))
                    {
                        txtDWAAmt.Focus();
                        ShowMsgBox("Please Enter Valid DWA Amount.");
                        return bValidate;
                    }
                    //  if (!System.Text.RegularExpressions.Regex.IsMatch(txtDWAAmt.Text, "^(^\\d{1,6}(\\.\\d{0,2})?$)"))
                    if (!Regex.IsMatch(txtDWAAmt.Text, "^(^\\d{1,6}(\\.\\d{0,2})?$)"))
                    {
                        txtDWAAmt.Focus();
                        ShowMsgBox("Please Enter Valid DWA Amount (eg:111111.00).");
                        return false;
                    }
                }
                if ((hdndwaid.Value ?? "").Length == 0)
                {
                    if (fupdDoc.FileName == "")
                    {
                        fupdDoc.Focus();
                        ShowMsgBox("Please Upload DWA Document");
                        return bValidate;
                    }
                }


                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();

                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return bValidate;
            }
        }


        bool ValidateExtendForm()
        {
            bool bValidate = false;
            try
            {
                if (txtExtend.Text == "")
                {
                    txtExtend.Focus();
                    ShowMsgBox("Please Select Extending Up-To Date");
                    return bValidate;
                }
                string sResultt = Genaral.DateComparision(txtExtend.Text, txtAward.Text, false, false);
                if (sResultt == "2")
                {
                    ShowMsgBox("Extending Up-to date should be Greater than or Equal to Awarded Up-to date");
                    return bValidate;
                }
                if (DocumentUpload.FileName == "")
                {
                    DocumentUpload.Focus();
                    ShowMsgBox("Please Upload Extended DWA Document");
                    return bValidate;
                }

                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();

                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return bValidate;
            }
        }
        /// <summary>
        /// Uploading document in Save time
        /// </summary>
        /// <param name="objdwa"></param>
        public void uploadbudgetdocument(ClsDwa objdwa)
        {
            try
            {
                string sMainFolderName = "DWA_DOCS";
                clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);
                bool Isuploaded;
                string strpath = System.IO.Path.GetExtension(fupdDoc.FileName);
                string filename = Path.GetFileName(fupdDoc.PostedFile.FileName);
                filename = Regex.Replace(filename, @"[^0-9a-zA-Z.()/-_']+", "");
                objdwa.FileName = filename;
                //string Dwanumber = Regex.Replace(txtDWANumber.Text, @"[^0-9a-zA-Z]+", "");
                if (fupdDoc.PostedFile != null)
                {
                    if (fupdDoc.PostedFile.ContentLength != 0)
                    {
                        string strExt = filename.Substring(filename.LastIndexOf('.') + 1);
                        string sFileExt = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["FileFormat"]);
                        string sAnxFileExt = System.IO.Path.GetExtension(fupdDoc.FileName).ToString().ToLower();
                        sAnxFileExt = ";" + sAnxFileExt.Remove(0, 1) + ";";
                        if (!sFileExt.Contains(sAnxFileExt))
                        {
                            ShowMsgBox("Invalid File Format");
                            return;
                        }
                        fupdDoc.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + fupdDoc.FileName));
                        string sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + fupdDoc.FileName);

                        if (File.Exists(sDirectory))
                        {
                            bool IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName);
                            if (IsExists == false)
                            {
                                objFtp.createDirectory(SFTPmainfolder + sMainFolderName);
                            }
                            //IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName + "/" + Dwanumber);
                            IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName + "/" + objdwa.DwaId);

                            if (IsExists == false)
                            {
                                //objFtp.createDirectory(SFTPmainfolder + sMainFolderName + "/" + Dwanumber);
                                objFtp.createDirectory(SFTPmainfolder + sMainFolderName + "/" + objdwa.DwaId);

                            }
                            //Isuploaded = objFtp.upload(SFTPmainfolder + sMainFolderName + "/" + Dwanumber + "/", filename, sDirectory);
                            Isuploaded = objFtp.upload(SFTPmainfolder + sMainFolderName + "/" + objdwa.DwaId + "/", filename, sDirectory);

                            if (Isuploaded == true & File.Exists(sDirectory))
                            {
                                File.Delete(sDirectory);
                                //sDirectory = Dwanumber + "/" + filename;
                                sDirectory = objdwa.DwaId + "/" + filename;

                                //txtdwafilepath.Text = sMainFolderName + "/" + Dwanumber + "/" + filename;
                                txtdwafilepath.Text = sMainFolderName + "/" + objdwa.DwaId + "/" + filename;

                            }
                        }
                        Session["FileUpload"] = null;
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

        /// <summary>
        /// Uploading Documnet while extend DWA
        /// </summary>
        /// <param name="objdwa"></param>
        public string[] uploadExtendeddocument(ClsDwa objdwa)
        {
            string[] IsUploaded = new string[2];
            try
            {
                string sMainFolderName = "DWA_DOCS";
                clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);
                bool Isuploaded;
                string strpath = System.IO.Path.GetExtension(DocumentUpload.FileName);
                string filename = Path.GetFileName(DocumentUpload.PostedFile.FileName);
                filename = Regex.Replace(filename, @"[^0-9a-zA-Z.()/-_']+", "");
                objdwa.FileName = filename;
                //string Dwanumber = Regex.Replace(txtDWANumber.Text, @"[^0-9a-zA-Z]+", "");
                if (DocumentUpload.PostedFile != null)
                {
                    if (DocumentUpload.PostedFile.ContentLength != 0)
                    {
                        string strExt = filename.Substring(filename.LastIndexOf('.') + 1);
                        string sFileExt = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["FileFormat"]);
                        string sAnxFileExt = System.IO.Path.GetExtension(DocumentUpload.FileName).ToString().ToLower();
                        sAnxFileExt = ";" + sAnxFileExt.Remove(0, 1) + ";";
                        if (!sFileExt.Contains(sAnxFileExt))
                        {
                            //ShowMsgBox("Invalid File Format");
                            IsUploaded[0] = "1";
                            IsUploaded[1] = "Invalid File Format";
                            return IsUploaded;
                        }
                        DocumentUpload.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + DocumentUpload.FileName));
                        string sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + DocumentUpload.FileName);

                        if (File.Exists(sDirectory))
                        {
                            bool IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName);
                            if (IsExists == false)
                            {
                                objFtp.createDirectory(SFTPmainfolder + sMainFolderName);
                            }
                            //IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName + "/" + Dwanumber);
                            IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName + "/" + objdwa.DwaId);

                            if (IsExists == false)
                            {
                                //objFtp.createDirectory(SFTPmainfolder + sMainFolderName + "/" + Dwanumber);
                                objFtp.createDirectory(SFTPmainfolder + sMainFolderName + "/" + objdwa.DwaId);

                            }
                            // added on 13-09-2023 to check the File Name alrady exist in the sftp folder directory.
                            bool IsFileNameExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName + "/" + objdwa.DwaId + "/" + filename);

                            if (IsFileNameExists == true)
                            {
                                IsUploaded[0] = "1";
                                IsUploaded[1] = "The file already exists with the same name, If want to update the Document, Please Rename the File and Upload it.";
                                return IsUploaded;
                            }
                            else
                            {
                                Isuploaded = objFtp.upload(SFTPmainfolder + sMainFolderName + "/" + objdwa.DwaId + "/", filename, sDirectory);
                            }
                            //Isuploaded = objFtp.upload(SFTPmainfolder + sMainFolderName + "/" + Dwanumber + "/", filename, sDirectory);


                            if (Isuploaded == true & File.Exists(sDirectory))
                            {
                                File.Delete(sDirectory);
                                //sDirectory = Dwanumber + "/" + filename;
                                sDirectory = objdwa.DwaId + "/" + filename;

                                //txtdwafilepath.Text = sMainFolderName + "/" + Dwanumber + "/" + filename;
                                txtDocumnetpath.Text = sMainFolderName + "/" + objdwa.DwaId + "/" + filename;

                            }
                        }
                        Session["FileUpload"] = null;

                    }
                    IsUploaded[0] = "0";
                    IsUploaded[1] = "Success";
                }
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();

                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return IsUploaded;
        }

        protected void loadDWA_Expire_Date(object sender, EventArgs e)
        {
            try
            {
                txtDWAPeriod.Text = string.Empty;

                string DWADate = txtDWADate.Text;

                // Convert the date string to a DateTime object.
                DateTime dwaDate = DateTime.ParseExact(DWADate.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                // Set the start date of the calendar extender to the dwaDate object.
                txtDWAPeriod_CalendarExtender1.StartDate = dwaDate;
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