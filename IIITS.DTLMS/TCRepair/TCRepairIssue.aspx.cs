using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using IIITS.DTLMS.BL.TCRepair;
using System.Data;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Net;

namespace IIITS.DTLMS.TCRepair
{
    public partial class TCRepairIssue : System.Web.UI.Page
    {
        string strFormCode = "TCRepairIssue";
        clsSession objSession;

        string sUserName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_USER"]);
        string sPassword = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_PASS"]);

        string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
        string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;
                Form.DefaultButton = cmdSave.UniqueID;
                txtPODate.Attributes.Add("readonly", "readonly");
                txtEstimationDate.Attributes.Add("readonly", "readonly");
                txtWorkOrderDate.Attributes.Add("readonly", "readonly");
                CalendarExtender_txtPODate.EndDate = System.DateTime.Now;
                txtEstimationDate.Enabled = false;
                txtWorkOrderDate.Enabled = false;
                txtPODate.Enabled = false;

                if (!IsPostBack)
                {
                    if (Request.QueryString["StoreId"] != null && Request.QueryString["StoreId"].ToString() != "")
                    {

                        if(Request.QueryString["TcId"] != null && Request.QueryString["TcId"].ToString() != "")
                        {
                            txtSelectedTcId.Text= Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TcId"]));
                        }
                    //    if (Session["TcId"] != null && Session["TcId"].ToString() != "")
                    //    {
                    //        txtSelectedTcId.Text = Session["TcId"].ToString();
                    //        Session["TcId"] = null;
                    //    }
                        txtStoreId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["StoreId"]));
                        GenerateInvoiceNo();
                        GenerateEstimationNo();
                        GenerateWorkorderNo();
                        txtEstimationDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                        txtWorkOrderDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                        txtPODate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                        CalendarExtender_txtPODate.StartDate = System.DateTime.Now;
                        LoadFaultTc();
                    }
                    if (Request.QueryString["RepairerId"] != null && Request.QueryString["RepairerId"].ToString() != "")
                    {
                        hdnrepid.Value = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["RepairerId"]));
                        gvFiles.Visible = false;
                        gvFiles1.Visible = false;

                        if (Request.QueryString["Remarks"] != null && Request.QueryString["Remarks"].ToString() != "")
                        {
                            txtRemarks.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["Remarks"]));
                            if (txtRemarks.Text == "")
                            {
                                txtRemarks.Visible = false;
                                divrm.Visible = false;
                            }
                            else
                            {
                                txtRemarks.Enabled = false;
                            }
                        }
                    }
                    else
                    {
                        pnlfile.Enabled = false;
                        file1.Visible = false;
                        file2.Visible = false;
                        file1.Attributes.Add("readonly", "readonly");
                        file2.Attributes.Add("readonly", "readonly");
                        divrm.Visible = false;
                    }
                    if (Session["sessionamtdata"] != null)
                    {
                        ViewState["datatogrid"] = Session["sessionamtdata"];
                        Session["sessionamtdata"] = null;

                        DataTable dt = (DataTable)ViewState["datatogrid"];
                        grdFaultyTCAmt.DataSource = dt;
                        grdFaultyTCAmt.DataBind();

                        var value = dt.AsEnumerable().Sum(g => g.Field<Double>("ESTIMATION_AMOUNT"));
                        txtEstimationAmount.Text = Convert.ToString(value);

                        grdFaultTC.Visible = false;
                        grdFaultyTCAmt.Visible = true;
                    }
                    Genaral.Load_Combo("select DISTINCT \"TR_ID\",\"TR_NAME\" from \"TBLREPAIRERRATES\" inner join \"TBLTRANSREPAIRER\" on \"TR_ID\"=\"RR_REP_ID\" ORDER BY \"TR_ID\"", cmbRepairer);
                    //From DTR Tracker
                    if (Request.QueryString["TransId"] != null && Request.QueryString["TransId"].ToString() != "")
                    {
                        if (Session["WFDataId"] != null && Session["WFDataId"].ToString() != "")
                        {
                            hdnwfoid.Value = Session["WFDataId"].ToString();
                        }
                        if (Session["ActionType"] != null && Session["ActionType"].ToString() != "")
                        {
                            txtActiontype.Text = Session["ActionType"].ToString();
                        }

                        txtRepairMasterId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TransId"]));
                        GetRepairSentDetails();
                        cmbRepairer.SelectedValue = hdnrepid.Value;
                        cmbRepairer_SelectedIndexChanged(sender, e);
                        LoadRepairSentDTR();
                        grdFaultyTCAmt.Visible = true;
                        dvComments.Visible = true;
                        dvComments.Style.Add("display", "block");
                    }
                    else
                    {
                        cmbRepairer.SelectedValue = hdnrepid.Value;
                        cmbRepairer_SelectedIndexChanged(sender, e);
                        cmbRepairer.Enabled = false;
                    }
                    string strQry = string.Empty;

                    strQry = "Title=Search and Select DTC Failure Details&";
                    strQry += "Query=select \"TC_CODE\",\"TC_SLNO\" FROM \"TBLTCMASTER\" WHERE \"TC_CODE\" NOT IN ";
                    strQry += "(SELECT \"RSD_TC_CODE\" from \"TBLREPAIRSENTDETAILS\" where \"RSD_DELIVARY_DATE\" is NULL ) AND \"TC_STATUS\"=3 AND  \"TC_CURRENT_LOCATION\"=1 AND ";
                    strQry += " CAST(\"TC_LOCATION_ID\" AS TEXT) LIKE '" + objSession.sStoreID + "' AND  {0} like %{1}% order by \"TC_CODE\" &";
                    strQry += "DBColName= CAST(\"TC_CODE\" AS TEXT)~CAST(\"TC_SLNO\" AS TEXT) &";
                    strQry += "ColDisplayName=DTr Code~DTr SlNo&";
                    strQry = strQry.Replace("'", @"\'");
                    cmdSearchTC.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtTcCode.ClientID + "&btn=" + cmdSearchTC.ClientID + "',520,520," + txtTcCode.ClientID + ")");
                    strQry = "Title=Search and Select Already Entered Reference No&";
                    strQry += "Query=SELECT UPPER(\"RSM_PO_NO\")RSM_PO_NO FROM \"TBLREPAIRSENTMASTER\" WHERE CAST(\"RSM_DIV_CODE\" AS TEXT) = '" + objSession.OfficeCode + "' AND {0} like %{1}% &";
                    strQry += "DBColName=CAST(\"RSM_PO_NO\" AS TEXT)&";
                    strQry += "ColDisplayName=Repairer Reference No&";

                    strQry = strQry.Replace("'", @"\'");
                    txtPODate.Attributes.Add("onblur", "return ValidateDate(" + txtPODate.ClientID + ");");
                    CheckAccessRights("2");
                    WorkFlowConfig();
                    BindgridView(SFTPmainfolder, sUserName, sPassword);
                    BindgridPOView(SFTPmainfolder, sUserName, sPassword);
                }
                if (objSession.RoleId == "2")
                {
                    DTrCODE.Visible = false;
                    MAKE.Visible = false;
                }
                else
                {
                    DTrCODE.Visible = true;
                    MAKE.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            clsDTrRepairActivity objTcRepair = new clsDTrRepairActivity();
            try
            {
                string off = objTcRepair.getdivid(objSession.OfficeCode);
                string repid = HttpUtility.UrlEncode(Genaral.UrlEncrypt(cmbRepairer.SelectedValue));
                string offcod = HttpUtility.UrlEncode(Genaral.UrlEncrypt(off));

                string url = "/MinorRepair/RepairerRatesView.aspx?RepairerId=" + repid + "&Offcode=" + offcod;
                string s = "window.open('" + url + "', '_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void DownloadFile(object sender, EventArgs e)
        {
            string fileName = (sender as LinkButton).CommandArgument;
            try
            {
                string SFTPmainfolderpath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["VirtualDirectoryDocs"]);
                string PONo = Regex.Replace(txtPONo.Text, @"[^0-9a-zA-Z]+", "");

                clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);

                string path = SFTPmainfolderpath + "REPAIRER_BUDGET_DOCS/" + PONo + "/" + fileName;
                RegisterStartupScript("Print", "<script>window.open('" + path + "','_blank')</script>");
            }
            catch (Exception ex)
            {
            }
        }
        protected void DownloadFile1(object sender, EventArgs e)
        {
            string fileName = (sender as LinkButton).CommandArgument;
            try
            {
                string SFTPmainfolderpath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["VirtualDirectoryDocs"]);
                string PONo = Regex.Replace(txtPONo.Text, @"[^0-9a-zA-Z]+", "");
                clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);
                string path = SFTPmainfolderpath + "REPAIRER_PO_DOCS/" + PONo + "/" + fileName;
                RegisterStartupScript("Print", "<script>window.open('" + path + "','_blank')</script>");
            }
            catch (Exception ex)
            {
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
                    string SFTPmainfolder = Convert.ToString(ConfigurationSettings.AppSettings["VirtualDirectoryDocs"]);

                    string PoNo = Regex.Replace(txtPONo.Text, @"[^0-9a-zA-Z]+", "");
                    clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);

                    string url = SFTPmainfolder + "REPAIRER_BUDGET_DOCS/" + PoNo + "/" + fileName1;
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
                    clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                }
            }
        }
        protected void DownloadFiledwnld1(object sender, EventArgs e)
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
                    string SFTPmainfolder = Convert.ToString(ConfigurationSettings.AppSettings["VirtualDirectoryDocs"]);

                    string PoNo = Regex.Replace(txtPONo.Text, @"[^0-9a-zA-Z]+", "");
                    clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);

                    string url = SFTPmainfolder + "REPAIRER_PO_DOCS/" + PoNo + "/" + fileName1;
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
                    clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                }
            }
        }
        protected void cmdSearchTC_Click(object sender, EventArgs e)
        {
            try
            {
                clsDtcMaster objDtcMaster = new clsDtcMaster();
                objDtcMaster.sTcCode = txtTcCode.Text;
                objDtcMaster.GetTCDetails(objDtcMaster);
                txtMake.Text = objDtcMaster.sTCMakeName;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        private string getFilename(string hreflink)
        {
            Uri uri = new Uri(hreflink);
            string filename = System.IO.Path.GetFileName(uri.LocalPath);
            return filename;
        }
        public void DisableCopy()
        {
            try
            {
                cmbGuarantyType.Attributes.Add("onmousedown", "return noCopyMouse(event);");
                cmbRepairer.Attributes.Add("onkeydown", "return noCopyKey(event);");
                ddlType.Attributes.Add("onkeydown", "return noCopyKey(event);");
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public DataTable BindgridView(string FtpServer, string username, string password)
        {
            DataTable dtFiles = new DataTable();
            try
            {
                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);
                string failureid = Regex.Replace(txtPONo.Text, @"[^0-9a-zA-Z]+", "");
                FtpServer += "REPAIRER_BUDGET_DOCS/" + failureid;
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
               // lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtFiles;
            }
        }
        public DataTable BindgridPOView(string FtpServer, string username, string password)
        {
            DataTable dtFiles = new DataTable();
            try
            {
                clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);
                string DiNo = Regex.Replace(txtPONo.Text, @"[^0-9a-zA-Z]+", "");
                FtpServer += "REPAIRER_PO_DOCS/" + DiNo;
                //path for get files from ftp
                bool IsExists = objFtp.FtpDirectoryExists(FtpServer);
                // checking related ponumber directory is there are not!
                dtFiles = objFtp.GetListOfFiles(FtpServer);
                if (dtFiles.Rows.Count > 0)
                {
                    gvFiles1.DataSource = dtFiles;
                    gvFiles1.DataBind();
                }
                if (IsExists == false)
                {
                    gvFiles1.Visible = false;
                    return dtFiles;
                }
                ViewState["DiDocs"] = dtFiles;
                gvFiles1.DataBind();
                return dtFiles;
            }
            catch (Exception ex)
            {
              //  lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtFiles;
            }
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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void gvFiles1_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
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
        protected void gvFiles1_Sorting(object sender, GridViewSortEventArgs e)
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
                            ViewState["dt"] = dataView.ToTable(); ;
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
        protected void cmbRepairer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbRepairer.SelectedIndex >= 0)
                {
                    if (ddlType.SelectedValue == "2")
                    {
                        clsTransRepairer objRepair = new clsTransRepairer();
                        objRepair.RepairerId = cmbRepairer.SelectedValue;
                        objRepair.GetRepairerDetails(objRepair);
                        txtAddress.Text = objRepair.RegisterAddress;
                        txtName.Text = objRepair.RepairerName;
                        txtPhone.Text = objRepair.RepairerPhoneNo;
                    }
                    else
                    {
                        clsTransSupplier objSupplier = new clsTransSupplier();
                        objSupplier.SupplierId = cmbRepairer.SelectedValue;
                        objSupplier.GetSupplierDetails(objSupplier);
                        txtAddress.Text = objSupplier.RegisterAddress;
                        txtName.Text = objSupplier.SupplierName;
                        txtPhone.Text = objSupplier.SupplierPhoneNo;
                    }
                }
                else
                {
                    txtAddress.Text = string.Empty;
                    txtPhone.Text = string.Empty;
                    txtName.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadFaultTc()
        {
            try
            {
                DataTable dt = new DataTable();
                clsDTrRepairActivity objRepair = new clsDTrRepairActivity();
                txtSelectedTcId.Text = txtSelectedTcId.Text.Replace("~", ",");
                if (!txtSelectedTcId.Text.StartsWith(","))
                {
                    txtSelectedTcId.Text = "," + txtSelectedTcId.Text;
                }
                if (!txtSelectedTcId.Text.EndsWith(","))
                {
                    txtSelectedTcId.Text = txtSelectedTcId.Text + ",";
                }
                if (objSession.OfficeCode.Length > 1)
                {
                    objRepair.sOfficeCode = objSession.OfficeCode.Substring(0, 3);
                    objRepair.sStoreId = clsStoreOffice.GetStoreID(objSession.OfficeCode);
                }
                else
                {
                    objRepair.sOfficeCode = objSession.OfficeCode;
                }
                //if (txtSelectedTcId.Text.Length >= 2)
                //{
                //    txtSelectedTcId.Text = txtSelectedTcId.Text.Substring(1, txtSelectedTcId.Text.Length - 2);
                //    txtSelectedTcId.Text = txtSelectedTcId.Text.Substring(0, txtSelectedTcId.Text.Length - 2);
                //}

                //if (txtSelectedTcId.Text.Length <= 2)
                //{
                //    txtSelectedTcId.Text = txtSelectedTcId.Text.Substring(1, txtSelectedTcId.Text.Length - 1);
                //    txtSelectedTcId.Text = txtSelectedTcId.Text.Substring(0, txtSelectedTcId.Text.Length - 1);
                //}
                
                    txtSelectedTcId.Text = txtSelectedTcId.Text.Substring(1, txtSelectedTcId.Text.Length - 1);
                    txtSelectedTcId.Text = txtSelectedTcId.Text.Substring(0, txtSelectedTcId.Text.Length - 1);
                


                objRepair.sOfficeCode = objSession.OfficeCode;
                objRepair.sTcId = txtSelectedTcId.Text;
                objRepair.UserId = objSession.UserId;
                if (objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                {
                    objRepair.sStoreId = txtStoreId.Text;
                }
                dt = objRepair.LoadFaultTCsearch(objRepair);

                ViewState["FaultTC"] = dt;
                grdFaultTC.DataSource = dt;
                grdFaultTC.DataBind();
                if (dt.Rows.Count > 0)
                {
                    txtqnty.Text = Convert.ToString(dt.Rows.Count);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadRepairSentDTR()
        {
            try
            {
                DataTable dt = new DataTable();
                clsDTrRepairActivity objRepair = new clsDTrRepairActivity();

                dt = objRepair.LoadRepairSentDTR(txtRepairMasterId.Text);
                ViewState["FaultTC"] = dt;
                grdFaultyTCAmt.DataSource = dt;
                grdFaultyTCAmt.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                clsDTrRepairActivity objTcRepair = new clsDTrRepairActivity();
                string[] Arr = new string[2];
                //Check AccessRights
                bool bAccResult = CheckAccessRights("2");
                if (bAccResult == false)
                {
                    return;
                }
                if (txtActiontype.Text != "A" && txtActiontype.Text != "M" && txtActiontype.Text != "R")
                {
                    if (Session["FileUpload"] != null && (!fupdDoc.HasFile))
                    {
                        fupdDoc = (FileUpload)Session["FileUpload"];
                        lblFilename.Text = fupdDoc.FileName;
                    }
                    else
                    {
                        if (fupdDoc.PostedFile.FileName != null && fupdDoc.PostedFile.FileName != "")
                        {
                            fupdDoc.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + fupdDoc.FileName));
                        }
                        else
                        {
                            if (objSession.RoleId == "7")
                            {
                                ShowMsgBox("Please upload the Budget Certificate");
                                fupdDoc.Focus();
                                return;
                            }
                        }
                    }
                    if (Session["FileUpload"] != null && (!fuppodoc.HasFile))
                    {
                        fuppodoc = (FileUpload)Session["FileUpload"];
                        lblpoFilename.Text = fuppodoc.FileName;
                    }
                    else
                    {
                        if (fuppodoc.PostedFile.FileName != null && fuppodoc.PostedFile.FileName != "")
                        {
                            fuppodoc.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + fuppodoc.FileName));
                        }
                        else
                        {
                            if (objSession.RoleId == "7")
                            {
                                ShowMsgBox("Please upload Draft Purchase Order Document");
                                fuppodoc.Focus();
                                return;
                            }
                        }
                    }
                }
                if (ValidateForm() == true)
                {
                    objTcRepair.sEstimationNo = txtEstimationNo.Text;
                    objTcRepair.sWorkorderNo = txtWorkorderNo.Text;
                    objTcRepair.sEstimationAmount = txtEstimationAmount.Text;
                    objTcRepair.sEstimationDate = txtEstimationDate.Text;
                    objTcRepair.sWorkorderDate = txtWorkOrderDate.Text;
                    objTcRepair.swoslno = hdfWorkorderNo.Value;
                    objTcRepair.sSupRepId = cmbRepairer.SelectedValue;
                    objTcRepair.sInvoiceDate = "";
                    objTcRepair.sInvoiceNo = "";
                    objTcRepair.sManualInvoiceNo = "";
                    objTcRepair.sIssueDate = "";
                    objTcRepair.sPurchaseDate = txtPODate.Text;
                    objTcRepair.sPurchaseOrderNo = txtPONo.Text;
                    objTcRepair.sCrby = objSession.UserId;
                    objTcRepair.sStoreId = txtStoreId.Text;
                    objTcRepair.sOfficeCode = objSession.OfficeCode;
                    objTcRepair.sActionType = txtActiontype.Text;
                    objTcRepair.sRSMID = txtRepairMasterId.Text;
                    objTcRepair.sWFDataId = hdnwfoid.Value;

                    if (cmbStarRated.SelectedIndex != 0)
                    {
                        objTcRepair.sstarrate = cmbStarRated.SelectedValue;
                    }
                    else
                    {
                        objTcRepair.sstarrate = "0";
                    }
                    if (cmbGuarantyType.SelectedIndex != 0)
                    {
                        objTcRepair.sGuarantyType = cmbGuarantyType.SelectedValue;
                    }
                    else
                    {
                        objTcRepair.sGuarantyType = "";
                    }
                    objTcRepair.sOldPONo = "";
                    objTcRepair.sPORemarks = txtRemarks.Text;
                    objTcRepair.sType = ddlType.SelectedValue;
                    objTcRepair.newdivcode = cmbRepairer.SelectedItem.Text;
                    if (txtActiontype.Text == "A" || txtActiontype.Text == "R" || txtActiontype.Text == "M")
                    {
                        if (hdfWFDataId.Value != "0")
                        {
                            ApproveRejectAction();
                            objTcRepair.updatemodifyeddata(objTcRepair.sIssueDate, objTcRepair.sPurchaseDate, objTcRepair.sManualInvoiceNo, objTcRepair.sOldPONo, objTcRepair.sPORemarks, objTcRepair.sRSMID);
                            if (objSession.sTransactionLog == "1")
                            {
                                Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (TCRepairerIssue) Repair ");
                            }
                            return;
                        }
                    }
                    
                    //To check Selected Transformers Already Sent for Supplier/Repair and Waiting For Approval
                    clsApproval objApproval = new clsApproval();
                    int i = 0;
                    string[] strQryVallist = new string[grdFaultyTCAmt.Rows.Count];
                    bool bDataExist = false;
                    foreach (GridViewRow row in grdFaultyTCAmt.Rows)
                    {
                        strQryVallist[i] = "'" + ((Label)row.FindControl("lblTCCode")).Text.Trim() + "'" +
                            "~" + ((Label)row.FindControl("lblGuarantyType")).Text.Trim() + "" +
                            "~" + ((Label)row.FindControl("lblEstimationAmount")).Text.Trim() + "" +
                            "~" + ((Label)row.FindControl("lblGuarantyType")).Text.Trim() + "" +
                            "~" + ((Label)row.FindControl("lblRemarks")).Text.Trim() + "" +
                            "~" + ((Label)row.FindControl("lblReason")).Text.Trim();
                        i++;
                        objTcRepair.sQty = Convert.ToString(grdFaultyTCAmt.Rows.Count);
                        string sTCCode = ((Label)row.FindControl("lblTCCode")).Text.Trim();
                        bDataExist = true;
                    }
                    if (bDataExist == false)
                    {
                        ShowMsgBox("No Transformer Exists to Issue for Repairer/Supplier");
                        return;
                    }
                    //Workflow
                    WorkFlowObjects(objTcRepair);
                    Arr = objTcRepair.SaveRepairIssueDetails(strQryVallist, objTcRepair);
                    if (Arr[1].ToString() == "0")
                    {
                        uploadbudgetdocument(objTcRepair);
                        uploadPOdocument(objTcRepair);
                        objTcRepair.updatefilepath(txtbudgetfilepath.Text, txtpofilepath.Text, Arr[2]);
                    }
                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (TCRepairerIssue) Repair ");
                    }

                    if (Arr[1].ToString() == "0")
                    {
                        ShowMsgBox(Arr[0].ToString());
                        cmdSave.Enabled = false;
                        return;
                    }
                    if (Arr[1].ToString() == "0")
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID, "alert('Transformers Issued Sucessfully to Repairer/Supplier'); location.href='FaultTCSearch.aspx';", true);
                        Reset();
                        txtTcCode.Text = string.Empty;
                        txtMake.Text = string.Empty;
                        grdFaultyTCAmt.DataSource = null;
                        grdFaultyTCAmt.DataBind();
                        ViewState["FaultTC"] = null;
                        txtSelectedTcId.Text = string.Empty;
                        cmdSave.Enabled = false;
                        return;
                    }
                    else if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0].ToString());
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void uploadbudgetdocument(clsDTrRepairActivity objTcRepair)
        {
            string sMainFolderName = "REPAIRER_BUDGET_DOCS";
            clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);
            bool Isuploaded;
            string strpath = System.IO.Path.GetExtension(fupdDoc.FileName);
            string filename = Path.GetFileName(fupdDoc.PostedFile.FileName);
            filename = Regex.Replace(filename, @"[^0-9a-zA-Z.()/-_']+", "");
            objTcRepair.sFileName = filename;
            string PoNo = Regex.Replace(txtPONo.Text, @"[^0-9a-zA-Z]+", "");

            if (fupdDoc.PostedFile != null)
            {
                if (fupdDoc.PostedFile.ContentLength != 0)
                {
                    string strExt = filename.Substring(filename.LastIndexOf('.') + 1);
                    string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FileFormat"]);
                    string sAnxFileExt = System.IO.Path.GetExtension(fupdDoc.FileName).ToString().ToLower();
                    sAnxFileExt = ";" + sAnxFileExt.Remove(0, 1) + ";";
                    if (!sFileExt.Contains(sAnxFileExt))
                    {
                        ShowMsgBox("Invalid File Format");
                        return;
                    }
                    string sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + fupdDoc.FileName);
                    if (File.Exists(sDirectory))
                    {
                        bool IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName);
                        if (IsExists == false)
                        {
                            objFtp.createDirectory(SFTPmainfolder + sMainFolderName);
                        }
                        IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName + "/" + PoNo);
                        if (IsExists == false)
                        {
                            objFtp.createDirectory(SFTPmainfolder + sMainFolderName + "/" + PoNo);
                        }
                        Isuploaded = objFtp.upload(SFTPmainfolder + sMainFolderName + "/" + PoNo + "/", filename, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            sDirectory = PoNo + "/" + filename;
                            txtbudgetfilepath.Text = sMainFolderName + "/" + PoNo + "/" + filename;
                        }
                    }
                    Session["FileUpload"] = null;
                }
            }
        }
        public void uploadPOdocument(clsDTrRepairActivity objTcRepair)
        {
            string sMainFolderName = "REPAIRER_PO_DOCS";
            clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);
            bool Isuploaded;

            string strpath = System.IO.Path.GetExtension(fuppodoc.FileName);

            string filename = Path.GetFileName(fuppodoc.PostedFile.FileName);
            filename = Regex.Replace(filename, @"[^0-9a-zA-Z.()/-_']+", "");

            objTcRepair.sFileName = filename;
            string PoNo = Regex.Replace(txtPONo.Text, @"[^0-9a-zA-Z]+", "");

            if (fuppodoc.PostedFile != null)
            {
                if (fuppodoc.PostedFile.ContentLength != 0)
                {
                    string strExt = filename.Substring(filename.LastIndexOf('.') + 1);
                    string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FileFormat"]);
                    string sAnxFileExt = System.IO.Path.GetExtension(fuppodoc.FileName).ToString().ToLower();
                    sAnxFileExt = ";" + sAnxFileExt.Remove(0, 1) + ";";
                    if (!sFileExt.Contains(sAnxFileExt))
                    {
                        ShowMsgBox("Invalid File Format");
                        return;
                    }
                    string sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + fuppodoc.FileName);
                    if (File.Exists(sDirectory))
                    {
                        bool IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName);
                        if (IsExists == false)
                        {
                            objFtp.createDirectory(SFTPmainfolder + sMainFolderName);
                        }
                        IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName + "/" + PoNo);
                        if (IsExists == false)
                        {
                            objFtp.createDirectory(SFTPmainfolder + sMainFolderName + "/" + PoNo);
                        }
                        Isuploaded = objFtp.upload(SFTPmainfolder + sMainFolderName + "/" + PoNo + "/", filename, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            sDirectory = PoNo + "/" + filename;
                            txtpofilepath.Text = sMainFolderName + "/" + PoNo + "/" + filename;
                        }
                    }
                    Session["FileUpload"] = null;
                }
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

        public void AddTCtoGrid(string sTcCode)
        {
            try
            {
                clsDTrRepairActivity objTCRepair = new clsDTrRepairActivity();

                if (ValidateGridValue(sTcCode) == true)
                {
                    objTCRepair.sTcCode = sTcCode;
                    objTCRepair.AddFaultTCDetails(objTCRepair);
                    if (ViewState["FaultTC"] != null)
                    {
                        DataTable dtFaultTc = (DataTable)ViewState["FaultTC"];
                        DataRow drow;
                        if (objTCRepair.sTcId != null)
                        {
                            if (dtFaultTc.Rows.Count > 0)
                            {
                                drow = dtFaultTc.NewRow();
                                drow["TC_ID"] = objTCRepair.sTcId;
                                drow["TC_CODE"] = objTCRepair.sTcCode;
                                drow["TC_SLNO"] = objTCRepair.sTcSlno;
                                drow["TM_NAME"] = objTCRepair.sMakeName;
                                drow["TC_CAPACITY"] = objTCRepair.sCapacity;
                                drow["TC_MANF_DATE"] = objTCRepair.sManfDate;
                                drow["TC_PURCHASE_DATE"] = objTCRepair.sPurchaseDate;
                                drow["TC_WARANTY_PERIOD"] = objTCRepair.sWarrantyPeriod;
                                drow["TS_NAME"] = objTCRepair.sSupplierName;
                                drow["TC_GUARANTY_TYPE"] = objTCRepair.sGuarantyType;

                                dtFaultTc.Rows.Add(drow);
                                grdFaultTC.DataSource = dtFaultTc;
                                grdFaultTC.DataBind();
                                ViewState["FaultTC"] = dtFaultTc;
                            }
                        }

                        ShowMsgBox("TC is not in Store or Good Condition");
                        txtTcCode.Text = "";
                    }
                    else
                    {
                        DataTable dtFaultTc = new DataTable();
                        DataRow drow;

                        dtFaultTc.Columns.Add(new DataColumn("TC_ID"));
                        dtFaultTc.Columns.Add(new DataColumn("TC_CODE"));
                        dtFaultTc.Columns.Add(new DataColumn("TC_SLNO"));
                        dtFaultTc.Columns.Add(new DataColumn("TM_NAME"));
                        dtFaultTc.Columns.Add(new DataColumn("TC_CAPACITY"));
                        dtFaultTc.Columns.Add(new DataColumn("TC_MANF_DATE"));
                        dtFaultTc.Columns.Add(new DataColumn("TC_PURCHASE_DATE"));
                        dtFaultTc.Columns.Add(new DataColumn("TC_WARANTY_PERIOD"));
                        dtFaultTc.Columns.Add(new DataColumn("TS_NAME"));
                        dtFaultTc.Columns.Add(new DataColumn("TC_GUARANTY_TYPE"));

                        drow = dtFaultTc.NewRow();

                        drow["TC_ID"] = objTCRepair.sTcId;
                        drow["TC_CODE"] = objTCRepair.sTcCode;
                        drow["TC_SLNO"] = objTCRepair.sTcSlno;
                        drow["TM_NAME"] = objTCRepair.sMakeName;
                        drow["TC_CAPACITY"] = objTCRepair.sCapacity;
                        drow["TC_MANF_DATE"] = objTCRepair.sManfDate;
                        drow["TC_PURCHASE_DATE"] = objTCRepair.sPurchaseDate;
                        drow["TC_WARANTY_PERIOD"] = objTCRepair.sWarrantyPeriod;
                        drow["TS_NAME"] = objTCRepair.sSupplierName;
                        drow["TC_GUARANTY_TYPE"] = objTCRepair.sGuarantyType;

                        dtFaultTc.Rows.Add(drow);
                        grdFaultTC.DataSource = dtFaultTc;
                        grdFaultTC.DataBind();
                        ViewState["FaultTC"] = dtFaultTc;

                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public bool ValidateGridValue(string sTcCode)
        {
            bool bValidate = false;
            try
            {
                ArrayList objArrlist = new ArrayList();

                foreach (GridViewRow row in grdFaultTC.Rows)
                {
                    objArrlist.Add(((Label)row.FindControl("lblTCCode")).Text.Trim());
                }

                if (objArrlist.Contains(sTcCode))
                {
                    ShowMsgBox("Transformer Already Added");
                    DataTable dtFaultTc = (DataTable)ViewState["FaultTC"];
                    grdFaultTC.DataSource = dtFaultTc;
                    grdFaultTC.DataBind();
                    return bValidate;
                }

                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return bValidate;
            }
        }

        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTcCode.Text.Trim() == "")
                {
                    ShowMsgBox("Select Transformer Code");
                    return;
                }
                AddTCtoGrid(txtTcCode.Text);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void btnCalEstAmt_Click(object sender, EventArgs e)
        {
            clsDTrRepairActivity objRepair = new clsDTrRepairActivity();
            try
            {
                bool res = objRepair.RepairDateIsValidnew(cmbRepairer.SelectedValue, objSession.OfficeCode);
                if (res == false)
                {
                    ShowMsgBox("Effective Date Expired");
                    return;
                }
                cmbStarRated.Enabled = false;
                ddlType.Enabled = false;
                string sSupRepId = cmbRepairer.SelectedValue;
                string sStarRated = cmbStarRated.SelectedValue;
                string off = objSession.OfficeCode;
                string StoreId = clsStoreOffice.GetStoreID(off);
                DataTable dt = new DataTable();
                double ttlamt = 0;
                if (ViewState["FaultTC"] != null)
                {
                    dt = (DataTable)ViewState["FaultTC"];
                }
                DataTable dt1 = new DataTable();
                dt1.Columns.Add("TC_ID");
                dt1.Columns.Add("TC_CODE");
                dt1.Columns.Add("TC_SLNO");
                dt1.Columns.Add("TM_NAME");
                dt1.Columns.Add("TC_MANF_DATE");
                dt1.Columns.Add("TC_CAPACITY");
                dt1.Columns.Add("TC_STAR_RATE");
                dt1.Columns.Add("RCOUNT");
                dt1.Columns.Add("TC_PURCHASE_DATE");
                dt1.Columns.Add("TC_WARANTY_PERIOD");
                dt1.Columns.Add("TS_NAME");
                dt1.Columns.Add("TC_GUARANTY_TYPE");
                dt1.Columns.Add("STATUS");
                dt1.Columns.Add("ESTIMATION_AMOUNT");
                DataRow dRow = dt1.NewRow();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dRow["TC_ID"] = dt.Rows[i][0];
                    dRow["TC_CODE"] = dt.Rows[i][1];
                    dRow["TC_SLNO"] = dt.Rows[i][2];
                    dRow["TM_NAME"] = dt.Rows[i][3];
                    dRow["TC_MANF_DATE"] = dt.Rows[i][4];
                    dRow["TC_CAPACITY"] = dt.Rows[i][5];
                    dRow["TC_STAR_RATE"] = dt.Rows[i][6];
                    dRow["RCOUNT"] = dt.Rows[i][7];
                    dRow["TC_PURCHASE_DATE"] = dt.Rows[i][8];
                    dRow["TC_WARANTY_PERIOD"] = dt.Rows[i][9];
                    dRow["TS_NAME"] = dt.Rows[i][10];
                    dRow["TC_GUARANTY_TYPE"] = dt.Rows[i][11];
                    dRow["STATUS"] = dt.Rows[i][12];

                    Double Amount = objRepair.GetEstAmt(StoreId, sSupRepId, Convert.ToString(dt.Rows[i][6]), Convert.ToString(dt.Rows[i][5]));
                    string cap = Convert.ToString(dt.Rows[i][5]);
                    string starRate = Convert.ToString(dt.Rows[i][6]);
                    if (Amount == 0)
                    {
                        ShowMsgBox("For this Repairer, Rates are not available for the Capacity " + cap + " and Star Rate of " + starRate + " ");
                        cmdSave.Enabled = false;
                        return;
                    }
                    dRow["ESTIMATION_AMOUNT"] = Amount;
                    dt1.Rows.Add(dRow);
                    dRow = dt1.NewRow();
                    ttlamt = ttlamt + Convert.ToDouble(Amount);

                }
                dt1.AcceptChanges();
                if (dt.Rows.Count != 0)
                {
                    txtEstimationAmount.Text = Convert.ToString(ttlamt);
                }
                grdFaultyTCAmt.DataSource = dt1;
                grdFaultyTCAmt.DataBind();
                grdFaultTC.Visible = false;
                grdFaultyTCAmt.Visible = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                Reset();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void Reset()
        {
            try
            {
                txtPONo.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public bool ValidateForm()
        {
            bool bValidate = false;
            try
            {
                if (txtEstimationDate.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Valid Estimation Date");
                    txtEstimationDate.Focus();
                    return bValidate;
                }
                if (txtWorkOrderDate.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Valid Work Order Date");
                    txtWorkOrderDate.Focus();
                    return bValidate;
                }

                if (txtEstimationAmount.Text.Trim() == "")
                {
                    ShowMsgBox("Caluclate the Estimation Amount");
                    txtEstimationAmount.Focus();
                    return bValidate;
                }
                if (txtPONo.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Valid Purchase Order No");
                    txtPONo.Focus();
                    return bValidate;
                }
                if (txtPODate.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Valid Purchase Order Date");
                    txtPODate.Focus();
                    return bValidate;
                }
                if (objSession.RoleId == "7")
                {
                    if (fupdDoc.FileName == fuppodoc.FileName)
                    {
                        fupdDoc.Focus();
                        fuppodoc.Focus();
                        ShowMsgBox("Please upload different documents");
                        return false;
                    }
                }
                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return bValidate;
            }
        }


        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlType.SelectedValue == "2")
                {
                    string stroffCode = string.Empty;
                    string stroffCode1 = string.Empty;
                    stroffCode = clsStoreOffice.GetZone_Circle_Div_Offcode(objSession.OfficeCode, objSession.RoleId);
                    stroffCode1 = stroffCode;
                    Genaral.Load_Combo("select DISTINCT \"TR_ID\",\"TR_NAME\" from \"TBLREPAIRERRATES\" inner join \"TBLTRANSREPAIRER\" on \"TR_ID\"=\"RR_REP_ID\" where \"RR_DIV_ID\"=(select \"STO_ID\" from \"TBLSTOREOFFCODE\" where \"STO_OFF_CODE\"='" + stroffCode.Substring(0, 3) + "' ) ORDER BY \"TR_ID\"", "--Select--", cmbRepairer);
                    lblSuppRep.Text = "Repairer";
                }
                else if (ddlType.SelectedValue == "1")
                {
                    Genaral.Load_Combo("SELECT \"TS_ID\",\"TS_NAME\"  FROM \"TBLTRANSSUPPLIER\"  WHERE \"TS_STATUS\" ='A' AND \"TS_ID\" NOT IN (SELECT \"TS_ID\" FROM \"TBLTRANSSUPPLIER\" WHERE \"TS_BLACK_LISTED\" =1 AND \"TS_BLACKED_UPTO\" >= NOW()) ORDER BY \"TS_NAME\" ", "--Select--", cmbRepairer);
                    lblSuppRep.Text = "Supplier";
                }
                else
                {
                    cmbRepairer.Items.Clear();
                    txtAddress.Text = string.Empty;
                    txtPhone.Text = string.Empty;
                    txtName.Text = string.Empty;
                }
                Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'SR' ", "--Select--", cmbStarRated);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY
                clsApproval objApproval = new clsApproval();
                objApproval.sFormName = "FaultTCSearch";
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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }

        #endregion

        protected void grdFaultTC_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Remove")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    int iRowIndex = row.RowIndex;

                    DataTable dt = (DataTable)ViewState["FaultTC"];
                    dt.Rows[iRowIndex].Delete();
                    dt.AcceptChanges();
                    if (dt.Rows.Count == 0)
                    {
                        ViewState["FaultTC"] = null;
                    }
                    else
                    {
                        ViewState["FaultTC"] = dt;
                    }
                    grdFaultTC.DataSource = dt;
                    grdFaultTC.DataBind();
                    if (dt.Rows.Count > 0)
                    {
                        txtqnty.Text = Convert.ToString(dt.Rows.Count);
                        cmdSave.Enabled = true;
                    }
                    cmbRepairer.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GenerateInvoiceNo()
        {
            try
            {
                clsInvoice objInvoice = new clsInvoice();
                string sRoletype = objSession.sRoleType;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void GenerateEstimationNo()
        {
            try
            {
                clsInvoice objInvoice = new clsInvoice();
                string sRoletype = objSession.sRoleType;
                txtEstimationNo.Text = objInvoice.GenerateEstimationNoForRepairer(objSession.OfficeCode, sRoletype);
                hdfEstimateNo.Value = txtEstimationNo.Text;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void GenerateWorkorderNo()
        {
            try
            {
                clsInvoice objInvoice = new clsInvoice();
                string sRoletype = objSession.sRoleType;

                txtWorkorderNo.Text = objInvoice.GenerateWorkorderNoForRepairer(objSession.OfficeCode, sRoletype);
                hdfWorkorderNo.Value = txtWorkorderNo.Text.Split('/').GetValue(3).ToString().Trim();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        bool ValidateGatePass()
        {
            bool bValidate = false;

            try
            {
                if (txtVehicleNo.Text.Length == 0)
                {
                    txtVehicleNo.Focus();
                    ShowMsgBox("Enter Vehicle No");
                    return bValidate;
                }
                if (txtChallen.Text.Length == 0)
                {
                    txtChallen.Focus();
                    ShowMsgBox("Enter Challen Number");
                    return bValidate;
                }

                if (txtReciepient.Text.Trim().Length == 0)
                {
                    txtReciepient.Focus();
                    ShowMsgBox("Enter Reciepient Name");
                    return bValidate;
                }
                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                bValidate = false;
                return bValidate;
            }
        }

        protected void cmdGatePass_Click(object sender, EventArgs e)
        {
            try
            {
                clsInvoice objInvoice = new clsInvoice();
                string[] Arr = new string[2];
                if (txtActiontype.Text == "V")
                {
                    string strParam = "id=RepairGatepass&InvoiceId=" + hdfInvoiceNo.Value + "&TransId=" + txtRepairMasterId.Text;
                    RegisterStartupScript("PrintD", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                    return;
                }
                if (ValidateGatePass() == true)
                {
                    objInvoice.sGatePassId = txtGpId.Text;
                    objInvoice.sVehicleNumber = txtVehicleNo.Text.Replace("'", "");
                    objInvoice.sReceiptientName = txtReciepient.Text.Replace("'", "");
                    objInvoice.sChallenNo = txtChallen.Text.Replace("'", "");
                    objInvoice.sCreatedBy = objSession.UserId;
                    if (ViewState["FaultTC"] != null)
                    {
                        DataTable dt = (DataTable)ViewState["FaultTC"];
                        string sTCCode = "";
                        foreach (GridViewRow row in grdFaultyTCAmt.Rows)
                        {
                            sTCCode += ((Label)row.FindControl("lblTCCode")).Text.Trim() + ",";
                            objInvoice.sIssueQty = Convert.ToString(grdFaultyTCAmt.Rows.Count);
                        }
                        objInvoice.sTcCode = sTCCode;
                    }

                    objInvoice.sInvoiceNo = hdfInvoiceNo.Value.Replace("'", "");

                    Arr = objInvoice.SaveUpdateGatePassDetails(objInvoice);

                    if (Arr[0].ToString() == "0"|| Arr[0].ToString() == "1")
                    {
                        return;
                    }
                    if (Arr[0].ToString() == "2")
                    {
                        ShowMsgBox(Arr[1]);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetRepairSentDetails()
        {
            try
            {
                clsDTrRepairActivity objRepair = new clsDTrRepairActivity();
                clsApproval objApproval = new clsApproval();
                objRepair.sRepairMasterId = txtRepairMasterId.Text;
                objRepair.GetRepairSentDetails(objRepair);
                Genaral.Load_Combo("select DISTINCT \"TR_ID\",\"TR_NAME\" from \"TBLREPAIRERRATES\" inner join \"TBLTRANSREPAIRER\" on \"TR_ID\"=\"RR_REP_ID\"  ORDER BY \"TR_ID\" ", cmbRepairer);
                Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'SR' ", cmbStarRated);

                cmbGuarantyType.SelectedValue = objRepair.sGuarantyType;
                ddlType.SelectedValue = objRepair.sType;

                hdfRepairId.Value = objRepair.sSupRepId;
                cmbRepairer.SelectedValue = objRepair.sSupRepId;
                cmbStarRated.SelectedValue = objRepair.sstarrate;
                txtEstimationNo.Text = objRepair.sEstimationNo;
                txtWorkorderNo.Text = objRepair.sWorkorderNo;
                txtEstimationAmount.Text = objRepair.sEstimationAmount;
                txtEstimationDate.Text = objRepair.sEstimationDate;
                txtWorkOrderDate.Text = objRepair.sWorkorderDate;
                txtPONo.Text = objRepair.sPurchaseOrderNo;
                txtPODate.Text = objRepair.sPurchaseDate;
                txtStoreId.Text = objRepair.sStoreId;
                txtRemarks.Text = objRepair.sPORemarks;
                hdfInvoiceNo.Value = objRepair.sInvoiceNo;
                txtqnty.Text = objRepair.sQty;
                cmdSave.Enabled = true;
                cmdReset.Enabled = false;
                cmdReset.Visible = false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void WorkFlowObjects(clsDTrRepairActivity objDTRRepair)
        {
            try
            {
                string sClientIP = string.Empty;

                string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(ip))
                {
                    string[] ipRange = ip.Split(',');
                    int le = ipRange.Length - 1;
                    sClientIP = ipRange[0];
                }
                else
                {
                    sClientIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                objDTRRepair.sFormName = "FaultTCSearch";
                objDTRRepair.sOfficeCode = objSession.OfficeCode;
                objDTRRepair.sClientIP = sClientIP;
                objDTRRepair.sWFObjectId = hdfWFOId.Value;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        #region Workflow/Approval
        public void SetControlText()
        {
            try
            {
                txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));

                if (txtActiontype.Text == "A")
                {
                    cmdSave.Text = "Approve";
                    pnlApproval.Enabled = false;
                    Panel1.Enabled = false;
                    cmdSave.Enabled = true;
                    txtComment.Visible = true;
                    lnkRepairerDetails.Enabled = true;
                }
                if (txtActiontype.Text == "R")
                {
                    cmdSave.Text = "Reject";
                    pnlApproval.Enabled = false;
                    Panel1.Enabled = false;
                    txtComment.Visible = true;
                }
                if (txtActiontype.Text == "M")
                {
                    cmdSave.Text = "Modify and Approve";
                    pnlApproval.Enabled = true;
                    Panel1.Enabled = false;

                    txtComment.Visible = true;
                    txtPONo.Enabled = false;
                    //  txtInvoiceDate.Enabled = false;
                    //  txtInvoiceNo.Enabled = false;
                }

                if (objSession.RoleId == "2" || objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                {
                    dvComments.Style.Add("display", "block");

                    grdFaultTC.Columns[10].Visible = false;
                    // grdFaultTC.Columns.RemoveAt(10);    //Index is the index of the column you want to remove
                    // grdFaultTC.DataBind();


                }
                //cmdReset.Enabled = false;

                if (hdfWFOAutoId.Value != "0")
                {
                    cmdSave.Text = "Save";

                }

                // Check for Creator of Form
                bool bResult = CheckFormCreatorLevel();
                if (bResult == true)
                {
                    cmdSave.Text = "Save";
                    pnlApproval.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void ApproveRejectAction()
        {
            try
            {
                clsApproval objApproval = new clsApproval();

                if (objSession.RoleId != "5")
                {
                    if (txtComment.Text.Trim() == "")
                    {
                        ShowMsgBox("Enter Comments/Remarks");
                        txtComment.Focus();
                        cmdSave.Enabled = true;
                        return;

                    }
                }


                objApproval.sCrby = objSession.UserId;
                objApproval.sOfficeCode = objSession.OfficeCode;
                // objApproval.sOfficeCode = clsStoreOffice.Getofficecode(objSession.OfficeCode);
                objApproval.sApproveComments = txtComment.Text.Trim();
                objApproval.sWFObjectId = hdfWFOId.Value;

                if (txtActiontype.Text == "A")
                {
                    objApproval.sApproveStatus = "1";
                }
                if (txtActiontype.Text == "R")
                {
                    objApproval.sApproveStatus = "3";
                }
                if (txtActiontype.Text == "M")
                {
                    objApproval.sApproveStatus = "2";
                }

                string sClientIP = string.Empty;

                string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(ip))
                {
                    string[] ipRange = ip.Split(',');
                    int le = ipRange.Length - 1;
                    sClientIP = ipRange[0];
                }
                else
                {
                    sClientIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                objApproval.sClientIp = sClientIP;
                objApproval.sWFDataId = hdfWFDataId.Value;
                clsDTrRepairActivity objTcRepair = new clsDTrRepairActivity();

                bool bResult = objApproval.ApproveWFRequest(objApproval);

                //objTcRepair.updatemodifyeddata(objTcRepair.sIssueDate, objTcRepair.sPurchaseDate, objTcRepair.sManualInvoiceNo, objTcRepair.sOldPONo, objTcRepair.sPORemarks, objTcRepair.sRSMID);

                if (bResult == true)
                {

                    if (objApproval.sApproveStatus == "1")
                    {
                        ShowMsgBox("Approved Successfully");
                        if (objSession.RoleId == "2")
                        {
                            clsRIApproval objRI = new clsRIApproval();
                            // objRI.SendSMStoSectionOfficer(txtDtrCode.Text, txtDecommId.Text, txtFailureId.Text);

                        }


                        cmdSave.Enabled = false;
                    }
                    else if (objApproval.sApproveStatus == "3")
                    {

                        ShowMsgBox("Rejected Successfully");
                        cmdSave.Enabled = false;
                    }
                }
                cmdSave.Enabled = false;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void WorkFlowConfig()
        {
            try
            {
                if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
                {


                    if (Session["WFOId"] != null && Session["WFOId"].ToString() != "")
                    {
                        hdfWFDataId.Value = Session["WFDataId"].ToString();
                        hdfWFOId.Value = Convert.ToString(Session["WFOId"]);
                        hdfWFOAutoId.Value = Convert.ToString(Session["WFOAutoId"]);

                        Session["WFDataId"] = null;
                        Session["WFOId"] = null;
                        Session["WFOAutoId"] = null;
                    }

                    //if (hdfWFDataId.Value != "0")
                    //{
                    //    GetRIDetailsFromXML(hdfWFDataId.Value);
                    //}
                    SetControlText();
                    if (txtActiontype.Text == "V")
                    {
                        cmdSave.Text = "View";
                        cmdSave.Enabled = false;
                        dvComments.Style.Add("display", "none");
                        txtVehicleNo.Enabled = false;
                        txtChallen.Enabled = false;
                        txtReciepient.Enabled = false;
                        cmbRepairer.Enabled = false;
                        txtPONo.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public bool CheckFormCreatorLevel()
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                string sResult = objApproval.GetFormCreatorLevel("", objSession.RoleId, "TCRepairIssue");
                if (sResult == "1")
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }
        #endregion

        protected void cmdClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
                {
                    Response.Redirect("/Approval/ApprovalInbox.aspx", false);
                }
                else
                {
                    Response.Redirect("FaultTCSearch.aspx", false);
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdSearchPO_Click(object sender, EventArgs e)
        {
            try
            {
                string[] Arr = new string[2];
                clsDtcMaster objDtcMaster = new clsDtcMaster();
                if (Arr[1] != "1")
                {
                    ShowMsgBox(Arr[0]);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}
