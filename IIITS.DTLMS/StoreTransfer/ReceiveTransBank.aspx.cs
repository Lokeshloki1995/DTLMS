using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Data.OleDb;

namespace IIITS.DTLMS.StoreTransfer
{
    public partial class ReceiveTransBank : System.Web.UI.Page
    {
        string strFormCode = "ReceiveTransBank";
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
                lblMessage.Text = string.Empty;
                Form.DefaultButton = cmdSave.UniqueID;
                if (!IsPostBack)
                {                    
                    if (Request.QueryString["InId"] != null && Request.QueryString["InId"].ToString() != "")
                    {
                        txtInvoiceId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["InId"]));
                        LoadInvoiceDetails();
                    }
                    //WorkFlow / Approval
                    WorkFlowConfig();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
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

        public void LoadInvoiceDetails()
        {
            try
            {
                clsBankInvoice objInvoice = new clsBankInvoice();
                objInvoice.sInvId = Convert.ToString(txtInvoiceId.Text);

                objInvoice.GetInvoiceDetails(objInvoice);
                txtInvoiceId.Text = objInvoice.sInvId;
                txtInvoiceNumber.Text = objInvoice.sInvNo;
                txtInvoicetDate.Text = objInvoice.sInvDate;
                grdTcTransfer.DataSource = objInvoice.dtDtrList;
                grdTcTransfer.DataBind();
                ViewState["TCDetails"] = objInvoice.dtDtrList;

                objInvoice.GetIndentDetails(objInvoice);
                txtIndentDate.Text = objInvoice.sIndentDate;
                txtIndentNumber.Text = objInvoice.sIndentNo;
                txtIndentId.Text = objInvoice.sIndentId;
                txtOMNo.Text = objInvoice.sOMNo;
                txtOmdate.Text = objInvoice.sOMDate;
                grdIndentDetails.DataSource = objInvoice.dtCapacity;
                grdIndentDetails.DataBind();
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
                SaveStoreInvoice();
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

                objApproval.sCrby = objSession.UserId;
                objApproval.sOfficeCode = objSession.OfficeCode;
                objApproval.sApproveComments = txtRemarks.Text.Trim();
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
                        txtInvoiceId.Text = objApproval.sNewRecordId;                      
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

        public void SaveStoreInvoice()
        {
            clsReceiveTrans objInvoice = new clsReceiveTrans();
            DataTable dtTCDetails;
            try
            {
                if (txtRVNo.Text.Trim() == string.Empty)
                {
                    txtRVNo.Focus();
                    ShowMsgBox("Please Enter RV Number");
                    return;
                }
                if (txtRemarks.Text.Trim() == string.Empty)
                {
                    txtRemarks.Focus();
                    ShowMsgBox("Please Enter Remarks");
                    return;
                }

                if (txtActionType.Text == "A" || txtActionType.Text == "R" || txtActionType.Text == "D")
                {
                    if (hdfWFDataId.Value != "0")
                    {
                        ApproveRejectAction();

                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (ReceiveTransformerBank) InterStore ");
                        }

                        return;
                    }
                }

                clsBankReceiveDtr obj = new clsBankReceiveDtr();
                obj.sInvoiceId = txtInvoiceId.Text;
                obj.sRVNo = txtRVNo.Text;
                obj.sRemarks = txtRemarks.Text;
                obj.dtDTRList = (DataTable)ViewState["TCDetails"];
                obj.sUserId = objSession.UserId;
                obj.sOfficeCode = objSession.OfficeCode;
                WorkFlowObjects(obj);

                string[] Arr = new string[3];

                #region Modify and Approve

                // For Modify and Approve
                if (txtActionType.Text == "M")
                {
                    if (txtRemarks.Text.Trim() == "")
                    {
                        ShowMsgBox("Enter Comments/Remarks");
                        txtRemarks.Focus();
                        return;

                    }
                    obj.sActionType = txtActionType.Text;

                    Arr = obj.SaveRVDetails(obj);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (ReceiveTransformerBank) InterStore ");
                    }

                    if (Arr[1].ToString() == "0")
                    {
                        hdfWFDataId.Value = obj.sWFDataId;
                        ApproveRejectAction();

                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (ReceiveTransformerBank) InterStore ");
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

                Arr = obj.SaveRVDetails(obj);

                if (objSession.sTransactionLog == "1")
                {
                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (ReceiveTransformerBank) InterStore ");
                }

                ShowMsgBox(Arr[1]);
                cmdSave.Enabled = false;
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


        public void WorkFlowObjects(clsBankReceiveDtr objBank)
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


                objBank.strFormCode = "ReceiveDTRToBank";
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

        #region Workflow/Approval

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
                        //GetDataFromXML(hdfWFDataId.Value);
                    }                  
                   
                    if (txtActionType.Text == "V")
                    {
                        cmdSave.Text = "View";                       
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
        #endregion
               
        
    }
}