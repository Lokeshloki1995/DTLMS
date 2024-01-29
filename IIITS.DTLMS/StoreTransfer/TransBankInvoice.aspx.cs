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
    public partial class TransBankInvoice : System.Web.UI.Page
    {
        public string strFormCode = "TransBankInvoice";
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
                txtInvDate.Attributes.Add("readonly", "readonly");
                txtInvDate_CalendarExtender1.EndDate = System.DateTime.Now;
                if (!IsPostBack)
                {
                    txtInvDate_CalendarExtender1.EndDate = System.DateTime.Now;
                    txtInvDate.Attributes.Add("readonly", "readonly");
                    if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
                    {
                        txtActionType.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));
                    }
                    if (Request.QueryString["InId"] != null && Request.QueryString["InId"].ToString() != "")
                    {
                        txtIndentId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["InId"]));
                    }
                    GenerateInvoiceNo();
                    GetIndentDetails();
                    LoadSearchWindow();
                    
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

        public void GenerateInvoiceNo()
        {
            try
            {
                clsInvoice objInvoice = new clsInvoice();
                string sRoletype = objSession.sRoleType;
                txtInvNo.Text = objInvoice.GenerateInvoiceNo(objSession.OfficeCode, sRoletype);
                txtInvNo.ReadOnly = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadSearchWindow()
        {
            try
            {              

                string strQry = string.Empty;
               
                strQry = "Title=Search and Select Tc Details&";
                strQry += "Query=SELECT \"TC_CODE\",CAST(\"TC_CAPACITY\" AS TEXT) TC_CAPACITY,CASE WHEN \"TC_STATUS\" ='1' THEN 'BRAND NEW' WHEN \"TC_STATUS\"='2' THEN 'REPAIR GOOD' WHEN \"TC_STATUS\" ='3' THEN 'FAULTY' END STATUS  FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\"  WHERE \"TC_MAKE_ID\" = \"TM_ID\" ";
                strQry += "AND \"TC_CURRENT_LOCATION\" =1 AND cast(\"TC_LOCATION_ID\" as text) = '" + objSession.OfficeCode + "' AND \"TC_CAPACITY\" IN ";
                strQry += " (SELECT \"BO_CAPACITY\" FROM \"TBLBIOBJECTS\" WHERE \"BO_BI_ID\" ='" + txtIndentId.Text + "') ";
                strQry += " AND {0} like %{1}% order by \"TC_SLNO\" &";
                strQry += "DBColName=CAST(\"TC_CODE\" AS TEXT)~CAST(\"TC_CAPACITY\" AS TEXT)&";
                strQry += "ColDisplayName=DTr Code~DTr Capacity&";

                strQry = strQry.Replace("'", @"\'");

                cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtDTRCode.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + txtDTRCode.ClientID + ")");
                
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

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                clsBankInvoice objBank = new clsBankInvoice();
                objBank.sTCCOde = txtDTRCode.Text;
                objBank.sOfficeCode = objSession.OfficeCode;
                objBank.LoadTcDetails(objBank);
                txtSlno.Text = objBank.sTCSlno;
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
                if (txtDTRCode.Text == String.Empty)
                {
                    ShowMsgBox("Please Select the DTR Code");
                    return;
                }

                if (txtSlno.Text == String.Empty)
                {
                    ShowMsgBox("Please Select the Valid DTR");
                    return;
                }

                clsBankInvoice objBank = new clsBankInvoice();
                objBank.sTCCOde = txtDTRCode.Text;
                objBank.sOfficeCode = objSession.OfficeCode;
                objBank.LoadTcDetails(objBank);
                txtSlno.Text = objBank.sTCSlno;

                DataTable dtDTRList = new DataTable();
                dtDTRList = (DataTable)ViewState["DTRLIST"];

                bool status = checkcapacity(objBank);
                if(status== false)
                {
                    ShowMsgBox("You did not requested transformer of this capacity");
                    return;
                }


                if (ViewState["SELECTEDDTR"] != null)
                {
                    DataTable dtTcDetails = (DataTable)ViewState["SELECTEDDTR"];
                    DataRow drow;

                    for (int i = 0; i < dtTcDetails.Rows.Count; i++)
                    {
                        if (txtDTRCode.Text == Convert.ToString(dtTcDetails.Rows[i]["TC_CODE"]))
                        {
                            ShowMsgBox("DTr Already Added");
                            return;
                        }
                    }

                    if (dtTcDetails.Rows.Count > 0)
                    {
                        status = CheckCapacitywiseCount(objBank);
                        if (status)
                        {
                            drow = dtTcDetails.NewRow();
                            drow["TC_ID"] = objBank.sTCID;
                            drow["TC_CODE"] = objBank.sTCCOde;
                            drow["TC_SLNO"] = objBank.sTCSlno;
                            drow["TM_NAME"] = objBank.sMake;
                            drow["TC_CAPACITY"] = objBank.sTCCapacity;
                            dtTcDetails.Rows.Add(drow);
                            grdTcDetails.DataSource = dtTcDetails;
                            grdTcDetails.DataBind();
                            txtDTRCode.Text = string.Empty;
                            ViewState["SELECTEDDTR"] = dtTcDetails;
                        }
                        else
                        {
                            ShowMsgBox("You Already Allocated Requested number of transformers");
                            return;
                        }
                    }
                }
                else
                {

                    DataTable dtTcDetails = new DataTable();
                    DataRow drow;
                    dtTcDetails.Columns.Add(new DataColumn("TC_ID"));
                    dtTcDetails.Columns.Add(new DataColumn("TC_SLNO"));
                    dtTcDetails.Columns.Add(new DataColumn("TC_CODE"));
                    dtTcDetails.Columns.Add(new DataColumn("TM_NAME"));
                    dtTcDetails.Columns.Add(new DataColumn("TC_CAPACITY"));
                    drow = dtTcDetails.NewRow();
                    drow["TC_ID"] = objBank.sTCID;
                    drow["TC_SLNO"] = objBank.sTCSlno;
                    drow["TC_CODE"] = objBank.sTCCOde;
                    drow["TM_NAME"] = objBank.sMake;
                    drow["TC_CAPACITY"] = objBank.sTCCapacity;
                    dtTcDetails.Rows.Add(drow);
                    grdTcDetails.DataSource = dtTcDetails;
                    grdTcDetails.DataBind();
                    txtDTRCode.Text = string.Empty;
                    ViewState["SELECTEDDTR"] = dtTcDetails;
                    grdTcDetails.Visible = true;
                }
            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public bool checkcapacity(clsBankInvoice objInvoice)
        {
            bool isCapacity = false;
            try
            {
                DataTable dtIndentTcGrid;
                dtIndentTcGrid = (DataTable)ViewState["DTRLIST"];
                for (int i = 0; i < dtIndentTcGrid.Rows.Count; i++)
                {
                    //to check whether selected capacity matches with the requested Capacity
                    if (Convert.ToDouble(dtIndentTcGrid.Rows[i]["BO_CAPACITY"]) == (Convert.ToDouble(objInvoice.sTCCapacity)))
                    {
                        isCapacity = true;
                    }
                }
                return isCapacity;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return isCapacity;
        }
        
        public bool CheckCapacitywiseCount(clsBankInvoice objInvoice)
        {    
            bool isCapacity = false;
            try
            {
                List<clsBankInvoice> lst = new List<clsBankInvoice>();
                List<clsBankInvoice> lstCheckTcCapa = new List<clsBankInvoice>();
                DataTable dtIndentTcGrid;
                DataTable dtTcGrid;
                dtIndentTcGrid = (DataTable)ViewState["DTRLIST"];
                dtTcGrid = (DataTable)ViewState["SELECTEDDTR"];
                foreach (DataRow drow in dtTcGrid.Rows)
                {
                    lst.Add(new clsBankInvoice
                    {
                        sTCID = Convert.ToString(drow["TC_ID"]),
                        sTCSlno = Convert.ToString(drow["TC_SLNO"]),
                        sTCCOde = Convert.ToString(drow["TC_CODE"]),
                        sMake = Convert.ToString(drow["TM_NAME"]),
                        sTCCapacity = Convert.ToString(drow["TC_CAPACITY"])
                    });
                }
                foreach (DataRow drow in dtIndentTcGrid.Rows)
                {
                    lstCheckTcCapa.Add(new clsBankInvoice
                    {
                        sIndentNo = Convert.ToString(drow["BO_ID"]),
                        sQuantity = Convert.ToString(drow["BO_QUANTITY"]),
                        sTCCapacity = Convert.ToString(drow["BO_CAPACITY"])

                    });
                }
                int i = lst.FindAll(item => item.sTCCapacity == Convert.ToString(objInvoice.sTCCapacity)).Count();

                var j = lstCheckTcCapa.Find(item => item.sTCCapacity == Convert.ToString(Convert.ToDouble(objInvoice.sTCCapacity)));

                if (i == Convert.ToInt32(j.sQuantity))
                {
                    return false;
                }
                else
                {
                    return true;
                }

                return isCapacity;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return isCapacity;
        }

        public void GetIndentDetails()
        {
            try
            {
                clsBankInvoice objbank = new clsBankInvoice();
                if(Convert.ToInt32(txtIndentId.Text) <= 0)
                {
                    objbank.sIndentId = txtIndentId.Text;
                    objbank.GetIndentId(objbank);
                    txtIndentId.Text = objbank.sIndentId;
                }
                else
                {
                    objbank.sIndentId = txtIndentId.Text;
                }
                
                objbank.GetIndentDetails(objbank);
                txtIndentNumber.Text = objbank.sIndentNo;
                txtIndentDate.Text = objbank.sIndentDate;
                txtOMNo.Text = objbank.sOMNo;
                txtOMDate.Text = objbank.sOMDate;
                txtFilepath.Text = objbank.sFilepath;
                if(txtFilepath.Text.Length > 0)
                {
                    lnkIndent.Visible = true;
                }
                grdTcRequest.DataSource = objbank.dtCapacity;
                grdTcRequest.DataBind();
                ViewState["DTRLIST"] = objbank.dtCapacity;
            }
            catch(Exception ex)
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

        protected void lnkIndent_Click(object sender, EventArgs e)
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
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtInvNo.Text.Trim() == string.Empty)
                {
                    txtInvNo.Focus();
                    ShowMsgBox("Please Enter Invoice Number");
                    return;
                }
                if (txtInvDate.Text.Trim() == string.Empty)
                {
                    txtIndentDate.Focus();
                    ShowMsgBox("Please Enter Invoice Date");
                    return;
                }

                if(ViewState["SELECTEDDTR"] == null)
                {
                    ShowMsgBox("Please Select the DTR");
                    return;
                }

                if (txtActionType.Text == "A" || txtActionType.Text == "R" || txtActionType.Text == "D")
                {
                    if (hdfWFDataId.Value != "0")
                    {
                        ApproveRejectAction();

                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (TransBankInvoice) InterStore ");
                        }

                        return;
                    }
                }

                clsBankInvoice obj = new clsBankInvoice();
                obj.sInvNo = txtInvNo.Text.Trim().ToUpper();
                obj.sInvDate = txtInvDate.Text.Trim().ToUpper();
                obj.sOfficeCode = objSession.OfficeCode;
                obj.sUserId = objSession.UserId;
                obj.sIndentId = txtIndentId.Text;
                obj.dtDtrList = (DataTable)ViewState["SELECTEDDTR"];
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

                    Arr = obj.SaveInvoiceDetails(obj);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (TransBankInvoice) InterStore ");
                    }

                    if (Arr[1].ToString() == "0")
                    {
                        hdfWFDataId.Value = obj.sWFDataId;
                        ApproveRejectAction();

                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (TransBankInvoice) InterStore ");
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

                Arr = obj.SaveInvoiceDetails(obj);

                if (objSession.sTransactionLog == "1")
                {
                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (TransBankInvoice) InterStore ");
                }

                ShowMsgBox(Arr[0]);
                cmdSave.Enabled = false;
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                lblMessage.Text = ex.Message;
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
                        txtInvid.Text = objApproval.sNewRecordId;
                        if(objApproval.sNewRecordId != null)
                        {
                            dvgatepass.Style.Add("display", "block");
                        }
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

        public void WorkFlowObjects(clsBankInvoice objBank)
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

        protected void cmdGatePass_Click(object sender, EventArgs e)
        {
            try
            {
                clsBankInvoice obj = new clsBankInvoice();
                obj.sVehicleNo = txtVehicleNo.Text.Trim().ToUpper();
                obj.sReceipient = txtReciepient.Text.Trim().ToUpper();
                obj.sChallanNo = txtChallen.Text.Trim().ToUpper();
                obj.sInvId = txtInvid.Text;
                obj.UpdateGatepassDetails(obj);

                string strParam = "id=BankGatepass&InvoiceId=" + txtInvid.Text;
                RegisterStartupScript("PrintD", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void GetDataFromXML(string sWFDataId)
        {
            try
            {
                clsBankInvoice obj = new clsBankInvoice();
                obj.sWFDataId = sWFDataId;
                obj.GetIndentDetailsFromXML(obj);
                txtInvNo.Text = obj.sInvNo;
                txtInvDate.Text = obj.sInvDate;

                grdTcDetails.DataSource = obj.dtDtrList;
                grdTcDetails.DataBind();
                ViewState["SELECTEDDTR"] = obj.dtDtrList;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdTcDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Remove")
                {
                    DataTable dt = (DataTable)ViewState["SELECTEDDTR"];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                        Label lblTcCode = (Label)row.FindControl("lblTcCode");
                        //to remove selected Capacity from grid
                        if (lblTcCode.Text == Convert.ToString(dt.Rows[i]["TC_CODE"]))
                        {
                            dt.Rows[i].Delete();
                            dt.AcceptChanges();
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["SELECTEDDTR"] = dt;
                    }
                    else
                    {
                        ViewState["SELECTEDDTR"] = null;
                    }
                    grdTcDetails.DataSource = dt;
                    grdTcDetails.DataBind();
                }
            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Approval/ApprovalInbox.aspx", false);
        }
    }
}