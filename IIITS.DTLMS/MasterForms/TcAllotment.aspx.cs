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
using System.Text.RegularExpressions;
using System.Configuration;
using ClosedXML.Excel;
using System.Data.OleDb;
using System.Text;
using System.Collections;
using System.ComponentModel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using NPOI.XSSF.UserModel;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Reflection;

namespace IIITS.DTLMS.MasterForms
{
    public partial class TcAllotement : System.Web.UI.Page
    {
        public string strFormCode = "TcAllotement";
        clsSession objSession;
        // string sFileServerPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["EstimatioinVirtualPath"]);
        string sUserName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_USER"]);
        string sPassword = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_PASS"]);

        string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
        string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
            {
                Response.Redirect("~/Login.aspx", false);
            }
            else
            {
                objSession = (clsSession)Session["clsSession"];

               // cmdsave.Enabled = false;

                if (!IsPostBack)
                {
                    LoadSearchWindow();
                    // LoadComboField();
                    if (Request.QueryString["QryAltid"] != null && Request.QueryString["QryAltid"].ToString() != "")
                    {
                        string Alt_id = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryAltid"]));
                        string Alt_No = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryAltNo"]));
                        LoadAllotedIst(Alt_No);
                        Create.Visible = true;
                        CreateDI.Visible = true;

                    }
                    if (Request.QueryString["QryDiNo"] != null && Request.QueryString["QryDiNo"].ToString() != "")
                    {
                        txtDINumber.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryDiNo"]));
                        DataTable dt = new DataTable();
                        clsAllotement obj = new clsAllotement();
                        obj.sDINo = txtDINumber.Text;
                        //cmdSearch_Click(sender, e);
                        dt = obj.GetDeliveryDetails(obj);
                        if (dt.Rows.Count > 0)
                        {
                            ViewState["TOTALTC"] = dt;
                            //cmdAdd.Enabled = true;
                            txtDINumber.Text = Convert.ToString(dt.Rows[0]["DI_NO"]);
                        }
                        //txtDINumber.Text = Convert.ToString(dt.Rows[0]["DI_NO"]);
                        obj.sDINo = txtDINumber.Text;
                        obj.GetDispatchCount(obj);
                        txtTotalQuantity.Text = obj.sTotalTC;
                        LoadComboField();
                        BindgridView(SFTPmainfolder, sUserName, sPassword);
                        BindAllotdocs(SFTPmainfolder, sUserName, sPassword);
                        grdDIPendingTC.DataSource = dt;
                        grdDIPendingTC.DataBind();
                    }
                }
            }
        }
        public void LoadComboField()
        {
            try
            {


                //Genaral.Load_Combo("select \"SM_ID\",\"SM_NAME\"   from \"TBLSTOREMAST\" ,\"TBLDELIVERYINSTRUCTION\" WHERE   \"SM_ID\"=\"DI_STORE_ID\" AND  \"DI_NO\"='" + txtDINumber.Text + "' and \"DI_STATUS\"=1  GROUP BY \"SM_ID\",\"SM_NAME\"", "--Select--", cmbStore);
                //Genaral.Load_Combo("select \"SM_ID\",\"SM_NAME\"   from \"TBLSTOREMAST\" ,\"TBLDELIVERYINSTMASTER\",\"TBLDELIVERYINSTRUCTION\" WHERE   \"SM_ID\"=\"DI_STORE_ID\" AND  \"DIM_DI_NO\"='" + txtDINumber.Text + "' and \"DIM_STATUS\"=1  GROUP BY \"SM_ID\",\"SM_NAME\"", "--Select--", cmbStore);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,MethodBase.GetCurrentMethod().Name, ex.Message,ex.StackTrace);
            }
        }
        protected void cmbStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strQry = string.Empty;
                //strQry = "SELECT \"TM_ID\",\"TM_NAME\" FROM \"TBLTRANSMAKES\" ,\"TBLDELIVERYINSTRUCTION\" WHERE \"TM_ID\"=\"DI_MAKE_ID\" AND  \"DI_NO\"='" + txtDINumber.Text + "'  AND \"DI_STORE_ID\"="+cmbStore.SelectedValue+" and \"DI_STATUS\"=1 GROUP BY \"TM_ID\",\"TM_NAME\"";

                // Genaral.Load_Combo(strQry, "-Select-", cmbMake);
                // Genaral.Load_Combo(" select  \"DIV_ID\",\"DIV_NAME\"  from \"TBLSTOREOFFCODE\", \"TBLSTOREMAST\",\"TBLDIVISION\",\"TBLDELIVERYINSTRUCTION\"  WHERE \"STO_SM_ID\"=\"SM_ID\" AND \"STO_OFF_CODE\"=\"DIV_CODE\"  AND  \"SM_ID\"=" + cmbStore.SelectedValue + " and \"DI_STATUS\"=1 AND  \"DI_NO\"='" + txtDINumber.Text + "' GROUP BY  \"DIV_ID\",\"DIV_NAME\" ", "--Select--", cmbDiv);

            }
            catch (Exception ex)
            {
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbMake_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\",\"TBLDELIVERYINSTRUCTION\" WHERE \"MD_TYPE\"='C' AND \"DI_CAPACITY_ID\"=\"MD_ID\" AND \"DI_NO\"='" + txtDINumber.Text + "' AND \"DI_STORE_ID\"=" + cmbStore.SelectedValue + " and \"DI_STATUS\"=1 and \"DI_MAKE_ID\"="+ cmbMake.SelectedValue +"  GROUP BY \"MD_ID\",\"MD_NAME\"", "--Select--", cmbCapacity);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbCapacity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                //Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\",\"TBLDELIVERYINSTRUCTION\" WHERE \"MD_TYPE\"='SR' AND \"DI_STARTTYPE\"=\"MD_ID\" AND \"DI_NO\"='" + txtDINumber.Text + "' AND \"DI_STORE_ID\"=" + cmbStore.SelectedValue + " and \"DI_STATUS\"=1 AND \"DI_CAPACITY\" =" + cmbCapacity.SelectedItem.Text + " and \"DI_MAKE_ID\"=" + cmbMake.SelectedValue + " GROUP BY \"MD_ID\",\"MD_NAME\"", "-Select-", cmbRating);


            }
            catch (Exception ex)
            {
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadAllotedIst(string Alt_No)
        {
            try
            {

                clsAllotement objAllotment = new clsAllotement();
                DataTable dt = new DataTable();

                objAllotment.sALTNumber = Alt_No;
                dt = objAllotment.GetAllotedDetails(Alt_No);
                ViewState["ALTCapacity"] = dt;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public DataTable BindgridView(string FtpServer, string username, string password)
        {
            DataTable dtFiles = new DataTable();
            try
            {
                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);

                string DiNo = Regex.Replace(txtDINumber.Text, @"[^0-9a-zA-Z]+", "");
                FtpServer += "DI_DOCS/" + DiNo;
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
              //  lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtFiles;
            }
        }
        public DataTable BindAllotdocs(string FtpServer, string username, string password)
        {
            DataTable dtFiles = new DataTable();
            try
            {
                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);

                clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);

                //path for get files from ftp
                bool IsExists = objFtp.FtpDirectoryExists(FtpServer);
                // checking related ponumber directory is there are not!
                if (IsExists == false)
                {
                    //grdDIdocs.Visible = false;
                    return dtFiles;
                }
                else
                {
                    dtFiles = objFtp.GetListOfFiles(FtpServer);
                }
                //grdDIdocs.DataSource = dtFiles;
                //grdDIdocs.DataBind();

                return dtFiles;


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtFiles;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
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
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmdClose_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("TcAllotmentView.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void ResetGrid()
        {
            DataTable dt = (DataTable)ViewState["ALTCapacity"];
            if (dt.Rows.Count > 0)
            {
                int counter = 0;
                foreach (DataRow row1 in dt.Rows)
                {
                    counter++;
                    row1["ID"] = counter;
                }
                ViewState["ALTCapacity"] = dt;
            }
            else
            {
                ViewState["ALTCapacity"] = null;
            }
            dt = (DataTable)ViewState["ALTCapacity"];
        }
        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            try
            {

                clsAllotement obj = new clsAllotement();
                //string temp = txtDINumber.Text;
                string temp = txtDINumber.Text;
                txtDINumber.Text = temp.ToUpper();
                obj.sDINo = temp.ToUpper();
                obj.GetDispatchCount(obj);
                txtTotalQuantity.Text = obj.sTotalTC;
                BindgridView(SFTPmainfolder, sUserName, sPassword);
                //txtDiId.Text = obj.GetPOId(obj);
                DataTable dt = new DataTable();
                dt = obj.GetDeliveryDetails(obj);
                if (dt.Rows.Count > 0)
                {
                    ViewState["TOTALTC"] = dt;
                    //cmdAdd.Enabled = true;
                }
                else
                {
                   // ShowMsgBox("DTr already Inwarded, Can not Allocate");
                    string msg = "DTr already Inwarded, Can not Allocate";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID, "alert('" + msg + "'); location.href='TcAllotmentView.aspx';", true);
                }
                grdDIPendingTC.DataSource = dt;
                grdDIPendingTC.DataBind();
                LoadComboField();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        private void Reset()
        {
            try
            {
                // txtALTNumber.Text = "";
                //txtALTDate.Text = "";
                //cmbStore.SelectedIndex = 0;
                //cmbDiv.SelectedIndex = 0;
                //cmbCapacity.ClearSelection(); 
                //txtQuantity.Text = "";
                //cmbRating.ClearSelection(); 
                //cmbMake.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void DownloadFile1(object sender, EventArgs e)
        {

            string fileName = (sender as LinkButton).CommandArgument;

            try
            {
                string SFTPmainfolderpath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["VirtualDirectoryDocs"]);
                bool status = false;
                //string ALTNo = Regex.Replace(txtALTNumber.Text, @"[^0-9a-zA-Z]+", "");

                clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);

            }
            catch (WebException ex)
            {
                //  throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
            }

        }

        public void LoadSearchWindow()
        {
            try
            {
                string strQry = string.Empty;
                strQry = "Title=Search and Select  Dispatch Instructions Details&";
                strQry += "Query=SELECT  \"DIM_DI_NO\"  FROM \"TBLDELIVERYINSTMASTER\" ";
                strQry += "WHERE {0} like %{1}% AND \"DIM_STATUS\"=1  group by \"DIM_DI_NO\" &";
                strQry += "DBColName=\"DIM_DI_NO\" &";
                strQry += "ColDisplayName=DI Number &";
                strQry = strQry.Replace("'", @"\'");
                cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtDINumber.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + txtDINumber.ClientID + ")");

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void DownloadFile(object sender, EventArgs e)
        {

            string fileName = (sender as LinkButton).CommandArgument;

            try
            {
                string SFTPmainfolderpath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["VirtualDirectoryDocs"]);
                bool status = false;

                string DINo = Regex.Replace(txtDINumber.Text, @"[^0-9a-zA-Z]+", "");

                clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);

                string path = SFTPmainfolderpath + "DI_DOCS/" + DINo + "/" + fileName;
                RegisterStartupScript("Print", "<script>window.open('" + path + "','_blank')</script>");

            }
            catch (WebException ex)
            {
                
            }

        }
        // to download the document
        protected void DownloadFiledwnld(object sender, EventArgs e)
        {
            bool endRequest = false;
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
                    string SFTPmainfolderpath = Convert.ToString(ConfigurationSettings.AppSettings["VirtualDirectoryDocs"]);

                    string DINo = Regex.Replace(txtDINumber.Text, @"[^0-9a-zA-Z]+", "");
                    clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);

                    string url = SFTPmainfolderpath + "DI_DOCS/" + DINo + "/" + fileName1;
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
                    clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                }

            }

        }
        // to get the file name
        private string getFilename(string hreflink)
        {
            Uri uri = new Uri(hreflink);

            string filename = System.IO.Path.GetFileName(uri.LocalPath);

            return filename;
        }

        protected void grdAllotement_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            clsAllotement Allot = new clsAllotement();
            try
            {
                if (e.CommandName == "Remove")
                {
                    DataTable dt = (DataTable)ViewState["ALTCapacity"];
                    DataTable InwardCont;
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    int sRowIndex = row.RowIndex;
                    Label lblId = (Label)row.FindControl("lblId");
                    Label lblAltDINo = (Label)row.FindControl("lblAltDINo");
                    Label lblAltNo = (Label)row.FindControl("lblAltNo");
                    Label lblAltDate = (Label)row.FindControl("lblAltDate");
                    Label lblDivId = (Label)row.FindControl("lblDivId");
                    Label lblStoreId = (Label)row.FindControl("lblStoreId");
                    Label lblMakeId = (Label)row.FindControl("lblMakeId");
                    Label lblCapacity = (Label)row.FindControl("lblCapacity");
                    Label lblRating = (Label)row.FindControl("lblRating");
                    Label lblQuantity = (Label)row.FindControl("lblQuantity");

                    Allot.sALTNumber = lblAltNo.Text;
                    Allot.sCapacity = lblCapacity.Text;
                    Allot.sMakeId = lblMakeId.Text;
                    Allot.sStoreId = lblStoreId.Text;
                    Allot.sDivId = lblDivId.Text;
                    InwardCont = Allot.GetInwardedCount(Allot);
                    // TO Check Before Deleting Whether Allotment No Inwarded or not 
                    if (InwardCont.Rows.Count > 0)
                    {
                        int delivered = Convert.ToInt32(InwardCont.Rows[0]["INWARDED"]);

                        string Msg = Convert.ToString(delivered);
                        ShowMsgBox("This Capacity Already Inwarded To Some Division  Quantity " + Msg + ", So You Can`t Delete This Record !");
                        InwardCont = null;
                        return;
                    }
                    else
                    {
                        // To Remove Row Data
                        dt.Rows[sRowIndex].Delete();
                        dt.AcceptChanges();
                    }

                    if (dt.Rows.Count > 0)
                    {
                        ViewState["ALTCapacity"] = dt;
                    }
                    else
                    {
                        ViewState["ALTCapacity"] = null;
                    }
                }

                if (e.CommandName == "EditQNTY")
                {
                    DataTable dt = (DataTable)ViewState["ALTCapacity"];
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    int sRowIndex = row.RowIndex;

                    Label lblId = (Label)row.FindControl("lblId");
                    Label lblAltDINo = (Label)row.FindControl("lblAltDINo");
                    Label lblAltNo = (Label)row.FindControl("lblAltNo");
                    Label lblAltDate = (Label)row.FindControl("lblAltDate");
                    Label lblDivId = (Label)row.FindControl("lblDivId");
                    Label lblStoreId = (Label)row.FindControl("lblStoreId");
                    Label lblMakeId = (Label)row.FindControl("lblMakeId");
                    Label lblCapacity = (Label)row.FindControl("lblCapacityId");
                    Label lblRating = (Label)row.FindControl("lblRating");
                    Label lblQuantity = (Label)row.FindControl("lblQuantity");

                    cmbStore_SelectedIndexChanged(sender, e);
                    //to remove selected Capacity from grid
                    dt.Rows[sRowIndex].Delete();
                    dt.AcceptChanges();
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["ALTCapacity"] = dt;
                    }
                    else
                    {
                        ViewState["ALTCapacity"] = null;
                    }

                }

            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdAllotement_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtTcCapacity = new DataTable();
                dtTcCapacity = (DataTable)ViewState["ALTCapacity"];
                LoadCapacity(dtTcCapacity);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void LoadCapacity(DataTable dt)
        {
            try
            {
                Reset();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdDIPendingTC_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdDIPendingTC.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["TOTALTC"];
                grdDIPendingTC.DataSource = SortDataTable(dt as DataTable, true);
                grdDIPendingTC.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
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
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
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
        protected void DownloadDiRecords(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            clsAllotement objDi = new clsAllotement();

            LinkButton lnkdwn = (LinkButton)sender;
            GridViewRow rw = (GridViewRow)lnkdwn.NamingContainer;
            objDi.sDino = ((Label)rw.FindControl("lblDI")).Text;
            objDi.sDimno = ((Label)rw.FindControl("lblDINO")).Text;
            objDi.sMake = ((Label)rw.FindControl("lblMake")).Text;
            objDi.sStorename = ((Label)rw.FindControl("lblstore")).Text;
            objDi.sCapacity = ((Label)rw.FindControl("lblCapacity")).Text;
            objDi.sRating = ((Label)rw.FindControl("lblRating")).Text;
            objDi.sTotqty = ((Label)rw.FindControl("lblQuantity")).Text;
            objDi.sStartrange = ((Label)rw.FindControl("lblstartrange")).Text.Split('H').GetValue(1).ToString();
            objDi.sEndrange = ((Label)rw.FindControl("lblendrange")).Text.Split('H').GetValue(1).ToString();
            objDi.sMakeId = ((Label)rw.FindControl("lblMakeId")).Text;
            objDi.sDimDino = ((Label)rw.FindControl("lbldimid")).Text;
            objDi.sPoid = ((Label)rw.FindControl("lblpoid")).Text;

            dt = objDi.PrintDi(objDi);

            if (dt.Rows.Count > 0)
            {
                Export_Excel_template(dt,objDi);

                string[] arrAlpha = Genaral.getalpha();

                string sMergeRange = arrAlpha[dt.Columns.Count - 1];
                string excelPath = Server.MapPath("~/Excel/Allotment_Excel.xlsx")  + Path.GetFileName(fupdDoc.PostedFile.FileName);
            }
            else
            {
                ShowMsgBox("No Records Found");
            }



        }
        protected void FTPUpload(object sender, EventArgs e, clsAllotement objDtrUpload)
        {
            string strUpload = string.Empty;
            try
            {
                //FTP Folder name. Leave blank if you want to upload to root folder.
                // string ftpFolder = "Uploads/";       
                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);
                string sFtpvirtual = Convert.ToString(ConfigurationSettings.AppSettings["VirtualDirectoryDocs"]);

                DateTime currentDateTime = DateTime.Now;
                
                string fileName = (Path.GetFileName(fupdDoc.FileName)).Trim();

                objDtrUpload.sfilename = fileName;

                if (fileName == "" || fileName == null)
                {
                    ShowMsgBox("Please select the File!");
                    return;
                }

                string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["UploadFormat"]);
                string sAnxFileExt = System.IO.Path.GetExtension(fupdDoc.FileName).ToString().ToLower();
                sAnxFileExt = ";" + sAnxFileExt.Remove(0, 1) + ";";
                clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);
                bool Isuploaded;
                bool IsFileExiest;
                string sMainFolderName = SFTPmainfolder + "/" + "ALLOTEMENT_UPLOAD_DOCS/";
               
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
                        objDtrUpload.sFilepath = SFTPmainfolder + "ALLOTEMENT_UPLOAD_DOCS/" + fileName;
                        File.Delete(sDirectory);
                        sDirectory = fileName;
                        ShowMsgBox("File Uploaded Successfully");
                        strUpload = "File:"+ fileName +" Is Uploaded";
                        ViewState["Path"] = fileName;
                        lblmsg.Text = strUpload; 
                        return ;
                        
                    }

                }
            }
            catch (WebException ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }

        protected void cmdUpload_click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            clsAllotement objDtrUpload = new clsAllotement();
            clsAllotement objDIS = new clsAllotement();
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
                        ViewState["Allotment"] = dtExcelData1;
                        objDIS = objDIS.ValidateExcelSheet(dt, diid, dino);

                        if(objDIS.statusId > 0)
                        {
                            ShowMsgBox("Please Choose Valid File.");
                            return;
                        }
                       
                        if (objDIS.statusId < 0  )
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
                       // clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                        excel_con.Close();
                        System.IO.File.Delete(excelPath);
                        return;
                    }
                }
              FTPUpload(sender, e, objDtrUpload);
                DateTime currentDateTime = DateTime.Now;
                string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["UploadFormat"]);
                string sUploadFileExt = System.IO.Path.GetExtension(fupdDoc.FileName).ToString().ToLower();
                sUploadFileExt = ";" + sUploadFileExt.Remove(0, 1) + ";";
              


            }
            catch (Exception ex)
            {
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                ShowMsgBox("Something went Wrong Please Try again....!!!");

            }
        }

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
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }

        protected void cmdsave_click(object sender, EventArgs e)
        {
            DataTable dtExcelData1 = new DataTable();
            clsAllotement objDtrUpload = new clsAllotement();
            DataTable dt = new DataTable();

            //string excelPath = Server.MapPath("~/DTLMSDocs/") + objSession.UserId + '~' + Path.GetFileName(fupdDoc.PostedFile.FileName);
            //string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["UploadFormat"]);
            string sPath = Convert.ToString(ViewState["Path"]);
            
            if (sPath == "")
            {
                ShowMsgBox("Please Upload File");
                return;
            }

            dtExcelData1 = (DataTable)ViewState["Allotment"];
            dt = dtExcelData1;
            string CrBy = objSession.UserId;
            if (dtExcelData1.Rows.Count > 0)
            {
                bool result = objDtrUpload.SaveAllotmentUploadDetails(dt,sPath,CrBy);
                if (result == true)
                {
                //    ShowMsgBox("Tc Allocated Successfully");
                //    Response.Redirect("TcAllotmentView.aspx", false);
                    ViewState["Path"] = null;
                    cmdupload.Enabled = true;
                    cmdsave.Enabled = false;
                    string msg = "Tc Allocated Successfully";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID, "alert('" + msg + "'); location.href='TcAllotmentView.aspx';", true);

                }
                else
                {
                    ShowMsgBox("Something Went Wrong");
                    cmdsave.Enabled = true;
                }

            }

        }
        public void Export_Excel_template(DataTable dt, clsAllotement objDi)
        {

            //File Path To Read Exce
            
            string ExcelPath = Server.MapPath("~") + "Excel\\ALLOTMENT_TEMPLATE.xlsx";
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
                            excelWorksheet.Column(j+1).Width = 15;
                            excelWorksheet.Cells[i+2, j+1].Value = Convert.ToString(dt.Rows[i][j]);   
                        }
                    }
                    try
                    {
                        //Export Excel- download excel
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename="+ objDi.sStorename+ "_"+ objDi.sDimno + "_" + DateTime.Now + ".xlsx");
                        //Response.AddHeader("content-disposition", "attachment;filename=Template" + DateTime.Now.ToString("dd_MM_yyyy") + ".xlsx");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            excelPackage.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                    catch(Exception ex)
                    {

                    }
                   
                }
            }


            catch (Exception ex)
            {
                if (!ex.Message.Contains("Thread was being aborted"))
                {
                    lblMessage.Text = clsException.ErrorMsg();
                    clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                }

            }
        }

        protected void cmdreset_click(object sender, EventArgs e)
        {
            Response.Redirect("TcAllotment.aspx", false); 
        }

        public void Export_Excel(DataTable dt)
        {

            //File Path To Read Excel
            int k, RowCount, ColCount;


            //string ExcelPath = Server.MapPath("~") + "ExcelWorkbook\\Bill_Register_Report.xlsx";
            string ExcelPath = Server.MapPath("~") + "Excel\\ALLOTMENT_TEMPLATE.xlsx";
            MemoryStream ms = new MemoryStream();
            FileInfo file = new FileInfo(ExcelPath);
            ExcelPackage xlPackage = new ExcelPackage(file);
            // DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();

            try
            {
                using (ExcelPackage excelPackage = new ExcelPackage(file))
                {
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    ExcelWorkbook excelWorkBook = excelPackage.Workbook;
                    ExcelWorksheet excelWorksheet = excelWorkBook.Worksheets["DtrReport"];
                    // dt1 = dt.Tables[0];
                    using (XLWorkbook wb = new XLWorkbook())
                    {

                        wb.Worksheets.Add(dt, "DtrReport");
                        XSSFWorkbook wb1 = null;

                        //wb1 = new XSSFWorkbook (wb);
                       // wb1 = wb;
                        wb1= (XSSFWorkbook) wb.AddWorksheet(dt, "DtrReport1");

                        wb1.GetSheetAt(0).GetRow(0).GetCell(0).SetCellValue("Sample");

                        using (var file2 = new FileStream(ExcelPath, FileMode.Create,
                               FileAccess.ReadWrite))
                        {
                            wb1.Write(file2);
                            file2.Close();
                        }

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=GridView.xlsx");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }


        }

    }
}