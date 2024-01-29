using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Text.RegularExpressions;
using System.Net;
using System.Configuration;
using System.IO;

namespace IIITS.DTLMS.DTCFailure
{
    public partial class CRReport : System.Web.UI.Page
    {
        string strFormCode = "CRReport";
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
                else
                {
                    txtCRDate.Attributes.Add("readonly", "readonly");
                    txtInvQty.Attributes.Add("readonly", "readonly");
                    txtDecommInventry.Attributes.Add("readonly", "readonly");


                    CalendarExtender1.EndDate = System.DateTime.Now;
                    objSession = (clsSession)Session["clsSession"];
                    lblMessage.Text = string.Empty;
                    if (!IsPostBack)
                    {
                        if (Request.QueryString["DecommId"] != null && Convert.ToString(Request.QueryString["DecommId"]) != "")
                        {
                            txtDecommId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["DecommId"]));
                        }
                        if (Request.QueryString["TypeValue"] != null && Convert.ToString(Request.QueryString["TypeValue"]) != "")
                        {
                            txtType.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TypeValue"]));

                        }


                        GetDetailsForCR();
                        BindgridView(SFTPmainfolder, sUserName, sPassword);

                        if (Textackdate.Text != null && Textackdate.Text != "")
                        {
                            DateTime decommDateTime = DateTime.Now;
                            decommDateTime = Convert.ToDateTime(Textackdate.Text);
                            CalendarExtender1.StartDate = decommDateTime;
                        }


                        //WorkFlow / Approval
                        WorkFlowConfig();

                        ApprovalHistoryView.BOID = Convert.ToString(Session["BOID"]);
                        ApprovalHistoryView.sRecordId = txtDecommId.Text;
                        ViewState["BOID"] = Convert.ToString(Session["BOID"]);
                        if (txtActiontype.Text == "M")
                        {
                            clsApproval objLevel = new clsApproval();
                            string sLevel = objLevel.sGetApprovalLevel(Convert.ToString(ViewState["BOID"]), objSession.RoleId);

                            if (sLevel != "" && sLevel != null)
                            {
                                if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                                {
                                    cmdCR.Text = "Modify and Submit";
                                }
                                else if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                                {
                                    cmdCR.Text = "Modify and Approve";
                                }
                            }
                        }
                        else if (txtActiontype.Text == "A")
                        {
                            clsApproval objLevel = new clsApproval();
                            string sLevel = objLevel.sGetApprovalLevel(Convert.ToString(ViewState["BOID"]), objSession.RoleId);
                            if (sLevel != "" && sLevel != null)
                            {
                                if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                                {
                                    cmdCR.Text = "Submit";
                                }
                                else if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                                {
                                    cmdCR.Text = "Approve";
                                }
                            }

                        }
                    }
                    ApprovalHistoryView.BOID = Convert.ToString(Session["BOID"]);
                    ApprovalHistoryView.sRecordId = txtDecommId.Text;
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
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

                string failureid = Regex.Replace(hdfFailureId.Value, @"[^0-9a-zA-Z]+", "");
                FtpServer += "FAILUREENTRY/" + failureid;
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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtFiles;
            }
        }
        protected void DownloadFile(object sender, EventArgs e)
        {

            string fileName = (sender as LinkButton).CommandArgument;

            try
            {
                bool status = false;
                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["VirtualDirectoryDocs"]);

                string failureid = Regex.Replace(hdfFailureId.Value, @"[^0-9a-zA-Z]+", "");
                clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);
                //status = objFtp.Download(sFileServerPath + "/PO_DOCS/" + PoNo + "/" + fileName, fileName);

                string path = SFTPmainfolder + "FAILUREENTRY/" + failureid + "/" + fileName;
                RegisterStartupScript("Print", "<script>window.open('" + path + "','_blank')</script>");

            }
            catch (WebException ex)
            {
                //  throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }

        }

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
                    string SFTPmainfolder = Convert.ToString(ConfigurationSettings.AppSettings["VirtualDirectoryDocs"]);

                    string failureid = Regex.Replace(hdfFailureId.Value, @"[^0-9a-zA-Z]+", "");
                    clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);

                    string url = SFTPmainfolder + "FAILUREENTRY/" + failureid + "/" + fileName1;
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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
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

        public void GetDetailsForCR()
        {
            try
            {
                clsCRReport objRIApproval = new clsCRReport();
                objRIApproval.sDecommId = txtDecommId.Text;

                objRIApproval.GetDetailsForCR(objRIApproval);

                txtWrkOrderDate.Text = objRIApproval.sWorkOrderDate;
                txtcomWO.Text = objRIApproval.sComWorkOrder;
                txtDEcomWO.Text = objRIApproval.sDecomWorkOrder;


                txtStoreKeepName.Text = objRIApproval.sStoreKeeperName;
                txtStoreOffName.Text = objRIApproval.sStoreOfficerName;
                txtRemarksStoreKeeper.Text = objRIApproval.sCommentByStoreKeeper.Replace("ç", ",");
                txtRemStoreOfficer.Text = objRIApproval.sCommentByStoreOfficer;
                txtOilCapacity.Text = objRIApproval.sOilQuantity;
                txtAcceptDate.Text = objRIApproval.sApprovedDate;
                txtRINo.Text = objRIApproval.sRINo;
                txtRIDate.Text = objRIApproval.sRIDate;
                txtCRDate.Text = objRIApproval.sCRDate;

                txtDTCCode.Text = objRIApproval.sDTCCode;
                txtDTCId.Text = objRIApproval.sDTCId;
                txtFailureDTr.Text = objRIApproval.sTCCode;
                txtFailDTrId.Text = objRIApproval.sFailureTCId;
                txtNewDTr.Text = objRIApproval.sNewTCCode;
                txtNewDTrId.Text = objRIApproval.sNewTCId;
                hdfFailureId.Value = objRIApproval.sFailureId;

                txtInvQty.Text = objRIApproval.sInventoryQty;
                txtDecommInventry.Text = objRIApproval.sDecommInventoryQty;

                Textackno.Text = objRIApproval.sRVNo;
                Textackdate.Text = objRIApproval.sRVDate;


                if (txtType.Text == "2")
                {
                    lblFailDTr.Text = "Enhance DTr Code";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdCR_Click(object sender, EventArgs e)
        {
            try
            {

                clsApproval objAproval = new clsApproval();

                objAproval.sPrevWFOId = txtWFOId.Text;
                objAproval.sWFObjectId = txtWFOId.Text;

                //objAproval.UpdateWFOAutoObject(objAproval);

                if (cmdCR.Text == "View")
                {
                    if (hdfApproveStatus.Value != "")
                    {
                        if (hdfApproveStatus.Value == "1" || hdfApproveStatus.Value == "2")
                        {
                            GenerateCRReport();
                        }
                    }
                    else
                    {
                        GenerateCRReport();
                    }
                    GenerateCRReport();
                    return;
                }

                clsCRReport objCR = new clsCRReport();
                string[] Arr = new string[2];


                string sResults = Genaral.DateComparisionTransaction(txtCRDate.Text, txtWrkOrderDate.Text, false, false);
                if (sResults == "1")
                {
                    ShowMsgBox("CR Date should be Greater than or Equal to RI Date");
                    txtCRDate.Focus();
                    return;
                }

                objCR.sDTCCode = txtDTCCode.Text;
                objCR.sCrby = objSession.UserId;

                objCR.sInventoryQty = txtInvQty.Text;
                objCR.sDecommId = txtDecommId.Text;
                objCR.sDecommInventoryQty = txtDecommInventry.Text;
                objCR.sFailureId = hdfFailureId.Value;
                objCR.sCRDate = txtCRDate.Text;

                //Workflow
                WorkFlowObjects(objCR);

                #region Modify and Approve

                // For Modify and Approve
                if (txtActiontype.Text == "M")
                {
                    if (hdfRejectApproveRef.Value != "RA")
                    {
                        if (txtComment.Text.Trim() == "")
                        {
                            ShowMsgBox("Enter Comments/Remarks");
                            txtComment.Focus();
                            return;

                        }
                    }
                    objCR.sBo_id = Convert.ToString(Session["BOID"]);
                    objCR.sActionType = txtActiontype.Text;

                    Arr = objCR.SaveCompletionReport(objCR);
                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "(CR Report) Failure ");
                    }
                    if (Arr[1].ToString() == "0")
                    {
                        hdfWFDataId.Value = objCR.sWFDataId;
                        ApproveRejectAction();
                        GenerateCRReport();
                        return;
                    }
                    if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0]);
                        return;
                    }
                }

                #endregion

                if (txtActiontype.Text == "RA")
                {
                    ApproveRejectAction();
                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "(CR Report) Failure ");
                    }
                    return;
                }

                if (objSession.RoleId == "4" || objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                {
                    if (txtWFOAuto.Text != "0" && objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                    {
                        if (txtComment.Text.Trim() == "")
                        {
                            ShowMsgBox("Enter Comments/Remarks");
                            txtComment.Focus();
                            return;
                        }

                        Arr = objCR.SaveCompletionReport(objCR);
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "(CR Report) Failure ");
                        }
                        GenerateCRReport();
                        cmdCR.Enabled = false;

                        if (Arr[1].ToString() == "0")
                        {
                            ShowMsgBox(Arr[0]);
                            return;
                        }
                        if (Arr[1].ToString() == "2")
                        {
                            ShowMsgBox(Arr[0]);
                            return;
                        }
                    }
                    else
                    {

                        if (objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                        {

                        }
                        else
                        {


                            if (txtComment.Text.Trim() == "")
                            {
                                ShowMsgBox("Enter Comments/Remarks");
                                txtComment.Focus();
                                return;
                            }

                            Arr = objCR.SaveCompletionReport(objCR);
                            if (objSession.sTransactionLog == "1")
                            {
                                Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "(CR Report) Failure ");
                            }
                            GenerateCRReport();
                            cmdCR.Enabled = false;

                            if (Arr[1].ToString() == "0")
                            {
                                ShowMsgBox(Arr[0]);
                                return;
                            }
                            if (Arr[1].ToString() == "2")
                            {
                                ShowMsgBox(Arr[0]);
                                return;
                            }
                        }
                    }
                    //if (txtComment.Text.Trim() == "")
                    //{
                    //    ShowMsgBox("Enter Comments/Remarks");
                    //    txtComment.Focus();
                    //    return;
                    //}

                    //Arr = objCR.SaveCompletionReport(objCR);
                    //if (objSession.sTransactionLog == "1")
                    //{
                    //    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "(CR Report) Failure ");
                    //}
                    //GenerateCRReport();
                    //cmdCR.Enabled = false;

                    //if (Arr[1].ToString() == "0")
                    //{
                    //    ShowMsgBox(Arr[0]);
                    //    return;
                    //}
                    //if (Arr[1].ToString() == "2")
                    //{
                    //    ShowMsgBox(Arr[0]);
                    //    return;
                    //}
                    //txtWFDataId.Text = objRIApproval.sWFDataId;
                }

                if (txtActiontype.Text == "A" || txtActiontype.Text == "R")
                {
                    ApproveRejectAction();
                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "(CR Report) Failure ");
                    }
                    return;
                }


            }
            catch (Exception ex)
            {
                //lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public int getApprovalLevel()
        {

            int Level = 0;
            try
            {
                if (objSession.RoleId == "4")
                {
                    Level = 1;
                }
                else if (objSession.RoleId == "1")
                {
                    Level = 2;
                }
                else if (objSession.RoleId == "24")
                {
                    Level = 3;
                }
                else
                    Level = 4;
                return Level;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Level;
            }

        }
        public void GenerateCRReport()
        {
            try
            {

                int approvelevel = getApprovalLevel();

                //To show Report Only for Failure Entry
                if (txtType.Text == "1" || txtType.Text == "4")
                {
                    string strParam = "id=CRReport&DecommId=" + txtDecommId.Text + "&iLevel=" + approvelevel;
                    RegisterStartupScript("PrintD", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                }
                else if (txtType.Text == "2")
                {
                    string strParam = "id=EnhanceCRReport&DecommId=" + txtDecommId.Text + "&iLevel=" + approvelevel;
                    RegisterStartupScript("PrintD", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public bool CheckFailureEntry()
        {
            try
            {
                clsFormValues objForm = new clsFormValues();
                objForm.sDecommisionId = txtDecommId.Text;
                string sResult = objForm.GetStatusFlagForDecommission(objForm);
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

        protected void lnkDTCDetails_Click(object sender, EventArgs e)
        {
            try
            {
                string sDTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtDTCId.Text));

                string url = "/MasterForms/DTCCommision.aspx?QryDtcId=" + sDTCId;
                string s = "window.open('" + url + "', '_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void lnkFailDTrDetails_Click(object sender, EventArgs e)
        {
            try
            {
                string sTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtFailDTrId.Text));

                string url = "/MasterForms/TcMaster.aspx?TCId=" + sTCId;
                string s = "window.open('" + url + "', '_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void lnkNewDTr_Click(object sender, EventArgs e)
        {
            try
            {
                string sTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtNewDTrId.Text));

                string url = "/MasterForms/TcMaster.aspx?TCId=" + sTCId;
                string s = "window.open('" + url + "', '_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
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
                    cmdCR.Text = "Approve";
                    pnlApproval.Enabled = false;
                }
                if (txtActiontype.Text == "R")
                {
                    cmdCR.Text = "Reject";
                    pnlApproval.Enabled = false;
                }
                if (txtActiontype.Text == "M")
                {
                    cmdCR.Text = "Modify and Approve";
                    pnlApproval.Enabled = true;
                }


                dvComments.Style.Add("display", "block");

                if (txtWFOAuto.Text != "0")
                {
                    //cmdApprove.Text = "Save";

                }

                // Check for Creator of Form
                bool bResult = CheckFormCreatorLevel();
                if (bResult == true)
                {

                    pnlApproval.Enabled = true;

                    // To handle Record From Reject 
                    if (txtActiontype.Text == "A" && txtWFOAuto.Text == "0")
                    {
                        txtActiontype.Text = "M";
                        hdfRejectApproveRef.Value = "RA";
                    }
                }
                if (objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                {
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


                if (txtComment.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Comments/Remarks");
                    txtComment.Focus();
                    return;
                }
                objApproval.sCrby = objSession.UserId;
                objApproval.sOfficeCode = objSession.OfficeCode;
                objApproval.sApproveComments = txtComment.Text.Trim();
                objApproval.sWFObjectId = txtWFOId.Text;
                objApproval.sWFAutoId = txtWFOAuto.Text;

                if (txtActiontype.Text == "A")
                {
                    objApproval.sApproveStatus = "1";
                }
                if (txtActiontype.Text == "R")
                {
                    objApproval.sApproveStatus = "3";
                }
                if (txtActiontype.Text == "RA")
                {
                    objApproval.sApproveStatus = "1";
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
                objApproval.sDescription = "Completion Report For Transformer Centre Code " + txtDTCCode.Text;
                //  bool bResult = objApproval.ApproveWFRequest(objApproval);
                bool bResult = objApproval.ApproveWFRequest_Latest(objApproval);

                if (bResult == true)
                {

                    if (objApproval.sApproveStatus == "1")
                    {
                        ShowMsgBox("Approved Successfully");

                        //if (objSession.RoleId == "1" || objSession.RoleId == "3" || objSession.RoleId == "4" || objSession.RoleId == "6")
                        //{
                        GenerateCRReport();
                        //}
                        cmdCR.Enabled = false;
                    }
                    else if (objApproval.sApproveStatus == "3")
                    {
                        ShowMsgBox("Rejected Successfully");
                        cmdCR.Enabled = false;
                    }
                    else if (objApproval.sApproveStatus == "2")
                    {
                        ShowMsgBox("Modified and Approved Successfully");

                        if (objSession.RoleId == "3")
                        {
                            GenerateCRReport();
                        }
                        cmdCR.Enabled = false;
                    }
                }
                else
                {
                    ShowMsgBox("Selected Record Already Approved");
                    return;
                }

            }
            catch (Exception ex)
            {
                ShowMsgBox("Selected Record Already Approved");
                //  lblMessage.Text = clsException.ErrorMsg();
                //  ShowMsgBox("Something went wrong while saving, Please Approve Once Again.");
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void WorkFlowConfig()
        {
            try
            {

                if (Request.QueryString["ActionType"] != null && Convert.ToString(Request.QueryString["ActionType"]) != "")
                {
                    if (Session["WFOId"] != null && Convert.ToString(Session["WFOId"]) != "")
                    {
                        hdfWFDataId.Value = Convert.ToString(Session["WFDataId"]);
                        txtWFOId.Text = Convert.ToString(Session["WFOId"]);
                        txtWFOAuto.Text = Convert.ToString(Session["WFOAutoId"]);
                        hdfApproveStatus.Value = Convert.ToString(Session["ApproveStatus"]);
                       // Session["WFDataId"] = null;
                        Session["WFOId"] = null;
                        Session["WFOAutoId"] = null;
                        Session["ApproveStatus"] = null;
                    }


                    if (hdfWFDataId.Value != "0")
                    {
                        GetCRDetailsFromXML(Convert.ToString(Session["WFDataId"]));
                    }
                    SetControlText();
                    if (txtActiontype.Text == "V")
                    {
                        //cmdCR.Enabled = false;
                        cmdCR.Text = "View";
                        dvComments.Style.Add("display", "none");
                        txtInvQty.ReadOnly = true;
                        txtDecommInventry.ReadOnly = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void WorkFlowObjects(clsRIApproval objRIApproval)
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


                objRIApproval.sFormName = "CRReport";
                objRIApproval.sOfficeCode = objSession.OfficeCode;
                objRIApproval.sClientIP = sClientIP;
                objRIApproval.sWFObjectId = txtWFOId.Text;
                objRIApproval.sWFAutoId = txtWFOAuto.Text;
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
                string sResult = objApproval.GetFormCreatorLevel("", objSession.RoleId, "CRReport");
                if (sResult == "1")
                {
                    if (txtActiontype.Text != "V")
                    {
                        txtcertify.Checked = false;
                        txtcertify.Enabled = true;
                        Lbl.Visible = true;
                    }

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

        #region Load From XML
        public void GetCRDetailsFromXML(string sWFDataId)
        {
            try
            {

                clsCRReport objRIApproval = new clsCRReport();
                objRIApproval.sWFDataId = sWFDataId;
              //  objRIApproval.sWFDataId = hdfWFDataId.Value;
                objRIApproval.GetCRDetailsFromXML(objRIApproval);

                txtInvQty.Text = objRIApproval.sInventoryQty;
                txtDecommInventry.Text = objRIApproval.sDecommInventoryQty;
                txtCRDate.Text = objRIApproval.sCRDate;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }
        #endregion

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
        protected void cmdClose_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("/Approval/ApprovalInbox.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdViewRI_Click(object sender, EventArgs e)
        {
            try
            {
                clsFormValues objApproval = new clsFormValues();
                string sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtType.Text));
                string sFailureId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfFailureId.Value));
                string sDecommId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtDecommId.Text));

                string url = "/DTCFailure/RIApprove.aspx?TypeValue=" + sTaskType + "&DecommId=" + sDecommId + "&FailureId=" + sFailureId;
                string s = "window.open('" + url + "', '_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void lnkHistory_Click(object sender, EventArgs e)
        {
            try
            {
                string sfailureId = string.Empty;
                string sRecordId = string.Empty;
                sfailureId = txtDecommId.Text;
                string sBOId = "26";
                string sFormName = "ApprovalHistory.aspx";

                sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sfailureId));
                sBOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sBOId));

                Response.Redirect("/Approval/" + sFormName + "?RecordId=" + sRecordId + "&BOId=" + sBOId, false);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }
    }
}