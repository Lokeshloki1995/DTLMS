using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.IO;
using System.Configuration;
using System.Net;

namespace IIITS.DTLMS.StoreTransfer
{
    public partial class BankIndent : System.Web.UI.Page
    {
        public string strFormCode = "BankIndent";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];
                txtIndentDate.Attributes.Add("readonly", "readonly");
                txtOMDate.Attributes.Add("readonly", "readonly");

                txtOMDate_CalendarExtender1.EndDate = System.DateTime.Now;
                txtIndentDate_CalendarExtender1.EndDate = System.DateTime.Now;

                if (!IsPostBack)
                {
                    txtIndentDate_CalendarExtender1.EndDate = System.DateTime.Now;
                    txtIndentDate.Attributes.Add("readonly", "readonly");
                    CheckAccessRights("4");
                    GenerateIndentNo();
                    Genaral.Load_Combo("SELECT \"MD_NAME\" AS \"ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'C' ORDER BY \"MD_ORDER_BY\"", "--Select--", cmbCapacity);

                    if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
                    {
                        txtActionType.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));
                    }
                    if (Request.QueryString["InId"] != null && Request.QueryString["InId"].ToString() != "")
                    {
                        txtIndentId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["InId"]));

                    }

                    if (txtActionType.Text == "V")
                    {
                        cmdSave.Text = "View";
                        cmdSave.Enabled = false;
                    }                  
                   
                    WorkFlowConfig();
                    
                }

                if (txtActionType.Text == "M")
                {
                    cmdSave.Text = "Modify And Approve";
                }
                else if (txtActionType.Text == "R")
                {
                    cmdSave.Text = "Reject";
                }
                else if (txtActionType.Text == "V")
                {
                    cmdSave.Text = "View";
                    cmdReset.Enabled = false;
                    dvComments.Style.Add("display", "none");
                }
            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
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
                        hdfApproveStatus.Value = Convert.ToString(Session["ApproveStatus"]);
                        hdfWFOAutoId.Value = Convert.ToString(Session["WFOAutoId"]);

                        Session["WFDataId"] = null;
                        Session["WFOId"] = null;
                        Session["ApproveStatus"] = null;
                        Session["WFOAutoId"] = null;
                    }

                    if (hdfWFDataId.Value != "0")
                    {
                        GetDataFromXML(hdfWFDataId.Value);
                    }

                    dvComments.Style.Add("display", "block");

                    if (hdfWFOAutoId.Value != "0")
                    {
                        dvComments.Style.Add("display", "none");
                    }
                    if (txtActionType.Text == "V")
                    {
                        cmdSave.Text = "View";
                        cmdReset.Enabled = false;
                        dvComments.Style.Add("display", "none");
                    }
                }
                else
                {
                    if (cmdSave.Text != "Save" && cmdSave.Text != "Submit" && cmdSave.Text != "Approve" && cmdSave.Text != "View")
                    {
                        cmdSave.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetDataFromXML(string sWFDataId)
        {
            try
            {
                clsBankIndent obj = new clsBankIndent();
                obj.sWFDataId = sWFDataId;
                obj.GetIndentDetailsFromXML(obj);
                txtIndentNumber.Text = obj.sIndentNo;
                txtIndentDate.Text = obj.sIndentDate;
                txtOMNo.Text = obj.sOMNo;
                txtOMDate.Text = obj.sOMDate;
                txtFilepath.Text = obj.sFilepath;
                if(txtFilepath.Text.Length > 0)
                {
                    fupOM.Visible = false;
                    lnkPoDownload.Visible = true;
                }
                LoadCapacity(obj.dtCapacity);             
                ViewState["CAPACITY"] = obj.dtCapacity;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
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

        public void LoadCapacity(DataTable dt)
        {
            try
            {
                //dt = objTcTransfer.LoadCapacity();
                grdTcRequest.DataSource = dt;
                grdTcRequest.DataBind();
                grdTcRequest.Visible = true;
                cmbCapacity.SelectedIndex = 0;
                txtQuantity.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                //if datatble is empty,add columns to table
                if (ViewState["CAPACITY"] != null)
                {
                    dt = (DataTable)ViewState["CAPACITY"];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (cmbCapacity.SelectedItem.Text == Convert.ToString(dt.Rows[i]["SO_CAPACITY"]))
                        {
                            ShowMsgBox("Capacity Already Added");
                            return;
                        }
                    }
                }

                if (ViewState["CAPACITY"] == null)
                {
                    dt.Columns.Add("SO_ID");
                    dt.Columns.Add("SO_CAPACITY");
                    dt.Columns.Add("SO_QNTY");
                }
                else
                {
                    //load datatble from viewstate
                    dt = (DataTable)ViewState["CAPACITY"];
                }

                DataRow dRow = dt.NewRow();
                int qnty = Convert.ToInt32(txtQuantity.Text);
                dRow["SO_QNTY"] = qnty;
                if (Convert.ToString(dRow["SO_QNTY"]) == "0")
                {
                    ShowMsgBox("Quantity Should Not be Zero");
                    return;
                }
                dRow["SO_CAPACITY"] = cmbCapacity.SelectedValue;
                dt.Rows.Add(dRow);
                ViewState["CAPACITY"] = dt;
                LoadCapacity(dt);
            }
            catch(Exception ex)
            {
                lblMessage.Text = ex.Message;
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        bool ValidateForm()
        {
            bool bValidate = false;
            try
            {                
                if (txtIndentNumber.Text.Trim() == string.Empty)
                {
                    txtIndentNumber.Focus();
                    ShowMsgBox("Please Enter Indent Number");
                    return bValidate;
                }
                if (txtIndentDate.Text.Trim() == string.Empty)
                {
                    txtIndentDate.Focus();
                    ShowMsgBox("Please Enter Indent Date");
                    return bValidate;
                }
                if (txtOMDate.Text.Trim() == string.Empty)
                {
                    txtOMDate.Focus();
                    ShowMsgBox("Please Enter OM Date");
                    return bValidate;
                }
                if (txtOMNo.Text.Trim() == string.Empty)
                {
                    txtOMNo.Focus();
                    ShowMsgBox("Please Enter OM No");
                    return bValidate;
                }
                if(txtFilepath.Text.Length == 0)
                {
                    if (fupOM.PostedFile.ContentLength == 0)
                    {
                        ShowMsgBox("Please Select the File");
                        fupOM.Focus();
                        return bValidate;
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

        protected void grdTcRequest_RowCommand(object sender, GridViewCommandEventArgs e)
        {           
            try
            {
                if (e.CommandName == "Remove")
                {
                    DataTable dt = (DataTable)ViewState["CAPACITY"];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                        Label lblCapacity = (Label)row.FindControl("lblCapacity");
                        //to remove selected Capacity from grid
                        if (lblCapacity.Text == Convert.ToString(dt.Rows[i]["SO_CAPACITY"]))
                        {
                            dt.Rows[i].Delete();
                            dt.AcceptChanges();
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["CAPACITY"] = dt;
                    }
                    else
                    {
                        ViewState["CAPACITY"] = null;
                    }
                    LoadCapacity(dt);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdTcRequest_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtTcCapacity = new DataTable();
                grdTcRequest.PageIndex = e.NewPageIndex;
                dtTcCapacity = (DataTable)ViewState["CAPACITY"];
                LoadCapacity(dtTcCapacity);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GenerateIndentNo()
        {
            try
            {
                clsIndent objIndent = new clsIndent();
                txtIndentNumber.Text = objIndent.GenerateBankIndentNo(objSession.OfficeCode);             
                txtIndentNumber.ReadOnly = true;
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
                if(!ValidateForm())
                {
                    return;
                }

                if (txtActionType.Text == "A" || txtActionType.Text == "R" || txtActionType.Text == "D")
                {
                    if (hdfWFDataId.Value != "0")
                    {
                        ApproveRejectAction();

                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (BankIndent) InterStore ");
                        }

                        return;
                    }
                }


                string sDirectory = string.Empty;
                string sFileName = string.Empty;
                if(txtFilepath.Text.Length == 0)
                {
                    if (fupOM.PostedFile.ContentLength != 0)
                    {
                        string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FileFormat"]);
                        string sAnxFileExt = System.IO.Path.GetExtension(fupOM.FileName).ToString().ToLower();
                        sAnxFileExt = ";" + sAnxFileExt.Remove(0, 1) + ";";

                        if (!sFileExt.Contains(sAnxFileExt))
                        {
                            ShowMsgBox("Invalid Image Format");
                            return;
                        }

                        sFileName = Path.GetFileName(fupOM.PostedFile.FileName);
                        fupOM.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sFileName));
                        sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sFileName);
                        SaveDocumments(sDirectory, sFileName);
                    }
                }
                

                clsBankIndent obj = new clsBankIndent();
                obj.sIndentNo = txtIndentNumber.Text.Trim().ToUpper();
                obj.sIndentDate = txtIndentDate.Text.Trim().ToUpper();
                obj.sOMNo = txtOMNo.Text.Trim().ToUpper();
                obj.sOMDate = txtOMDate.Text.Trim().ToUpper();
                obj.sOfficeCode = objSession.OfficeCode;
                obj.sUserId = objSession.UserId;
                obj.sFilepath = txtFilepath.Text;
                obj.dtCapacity = (DataTable)ViewState["CAPACITY"];
                WorkFlowObjects(obj);

                string[] Arr = new string[3];

                #region Modify and Approve

                // For Modify and Approve
                if (txtActionType.Text == "M")
                {
                    if (txtComment.Text.Trim() == "")
                    {
                        ShowMsgBox("Enter Comments/Remarks");
                        txtComment.Focus();
                        return;

                    }
                    obj.sActionType = txtActionType.Text;

                    Arr = obj.SaveBankIndent(obj);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (BankIndent) InterStore ");
                    }

                    if (Arr[1].ToString() == "0")
                    {
                        hdfWFDataId.Value = obj.sWFDataId;
                        ApproveRejectAction();

                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (BankIndent) InterStore ");
                        }

                        return;
                    }
                    if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0]);
                        return;
                    }
                }
                #endregion

                Arr = obj.SaveBankIndent(obj);

                if (objSession.sTransactionLog == "1")
                {
                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (BankIndent) InterStore ");
                }

                ShowMsgBox(Arr[0]);
                cmdSave.Enabled = false;

            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void SaveDocumments(string sFilePath, string sFileName)
        {
            try
            {
                //FTP Parameter
                string sFTPLink = string.Empty;
                string sFTPUserName = string.Empty;
                string sFTPPassword = string.Empty;
                string sActualPath = string.Empty;
                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);


                clsCommon objComm = new clsCommon();
                DataTable dt = objComm.GetAppSettings();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPMAINLINK")
                    {
                        sFTPLink = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
                    }
                    else if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPUSERNAME")
                    {
                        sFTPUserName = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
                    }
                    else if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPPASSWORD")
                    {
                        sFTPPassword = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
                    }
                }

                string sMainFolderName = "BANKINDENTDOCS";
                //clsFtp objFtp = new clsFtp(sFTPLink, sFTPUserName, sFTPPassword);
                clsSFTP objFtp = new clsSFTP(SFTPPath, sFTPUserName, sFTPPassword);

                bool Isuploaded;


                bool IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName + "/");
                if (IsExists == false)
                {
                    objFtp.createDirectory(SFTPmainfolder + sMainFolderName);
                }

                IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName + "/" + txtIndentNumber.Text + "/");
                if (IsExists == false)
                {
                    objFtp.createDirectory(SFTPmainfolder + sMainFolderName + "/" + txtIndentNumber.Text);
                }

                Isuploaded = objFtp.upload(SFTPmainfolder + sMainFolderName + "/" + txtIndentNumber.Text, sFileName, sFilePath);
                if (Isuploaded == true & File.Exists(sFilePath))
                {
                    File.Delete(sFilePath);
                    sActualPath = SFTPmainfolder + sMainFolderName + "/" + txtIndentNumber.Text + "/" + sFileName;
                }
                txtFilepath.Text = sActualPath;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void WorkFlowObjects(clsBankIndent objBank)
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


                objBank.strFormCode = "BankIndent";
                objBank.sOfficeCode = objSession.OfficeCode;
                objBank.sClientIP = sClientIP;
                objBank.sWFO_id = hdfWFOId.Value;
                objBank.sWFAutoId = hdfWFOAutoId.Value;

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
                objApproval.sWFObjectId = hdfWFOId.Value;
                objApproval.sWFAutoId = hdfWFOAutoId.Value;

                //Approve
                if (txtActionType.Text == "A")
                {
                    objApproval.sApproveStatus = "1";
                }
                //Reject
                if (txtActionType.Text == "R")
                {
                    objApproval.sApproveStatus = "3";
                }
                //Abort
                if (txtActionType.Text == "D")
                {
                    objApproval.sApproveStatus = "4";
                }
                //Modify and Approve
                if (txtActionType.Text == "M")
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

                bool bResult = false;
                if (txtActionType.Text == "M")
                {
                    objApproval.sWFDataId = hdfWFDataId.Value;
                    bResult = objApproval.ModifyApproveWFRequest(objApproval);
                }
                else
                {
                    bResult = objApproval.ApproveWFRequest(objApproval);
                }

                if (bResult == true)
                {
                    if (objApproval.sApproveStatus == "1")
                    {
                        ShowMsgBox("Approved Successfully");
                        txtIndentId.Text = objApproval.sNewRecordId;
                       // GenerateIndentReport();
                        cmdSave.Enabled = false;
                    }
                    else if (objApproval.sApproveStatus == "3")
                    {

                        ShowMsgBox("Rejected Successfully");
                        cmdSave.Enabled = false;
                    }
                    else if (objApproval.sApproveStatus == "4")
                    {

                        ShowMsgBox("Aborted Successfully");
                        cmdSave.Enabled = false;
                    }
                    else if (objApproval.sApproveStatus == "2")
                    {
                        ShowMsgBox("Modified and Approved Successfully");
                        cmdSave.Enabled = false;
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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtOMNo.Text = string.Empty;
            txtOMDate.Text = string.Empty;
            txtIndentNumber.Text = string.Empty;
            txtIndentDate.Text = string.Empty;
            cmbCapacity.SelectedIndex = 0;
            txtIndentNumber.Enabled = true;
            cmdSave.Text = "Save";
            txtIndentId.Text = string.Empty;
            txtQuantity.Text = string.Empty;
            ViewState["CAPACITY"] = null;
            lblMessage.Text = string.Empty;
            grdTcRequest.Visible = false;
            GenerateIndentNo();
        }

        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY
                clsApproval objApproval = new clsApproval();
                objApproval.sFormName = strFormCode;
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

        private string getFilename(string hreflink)
        {
            Uri uri = new Uri(hreflink);

            string filename = System.IO.Path.GetFileName(uri.LocalPath);

            return filename;
        }

        protected void lnkPoDownload_Click(object sender, EventArgs e)
        {
            try
            {
                
                Stream stream = null;
                int bytesToRead = 10000;
                byte[] buffer = new Byte[bytesToRead];
                string sFilePath = txtFilepath.Text;
                try
                {
                    string sVirtualDirpath = Convert.ToString(ConfigurationSettings.AppSettings["VirtualDirectoryDocs"]);
                    string url = sVirtualDirpath + sFilePath;
                    string fileName = getFilename(url);
                    HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(url);
                    HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();
                    if (fileReq.ContentLength > 0)
                        fileResp.ContentLength = fileReq.ContentLength;
                    stream = fileResp.GetResponseStream();
                    var resp = HttpContext.Current.Response;
                    resp.ContentType = "application/octet-stream";
                    resp.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
                    resp.AddHeader("Content-Length", fileResp.ContentLength.ToString());

                    int length;
                    do
                    {
                        if (resp.IsClientConnected)
                        {
                            length = stream.Read(buffer, 0, bytesToRead);
                            resp.OutputStream.Write(buffer, 0, length);
                            resp.Flush();
                            buffer = new Byte[bytesToRead];
                        }
                        else
                        {
                            length = -1;
                        }
                    } while (length > 0); //Repeat until no data is read
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }   
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Approval/ApprovalInbox.aspx", false);
        }
    }
}