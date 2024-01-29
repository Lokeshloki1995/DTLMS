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
    public partial class ReceiveTcCreate : System.Web.UI.Page
    {
        string strFormCode = "ReceiveTcCreate";
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
                    Genaral.Load_Combo("SELECT \"SM_ID\",\"SM_NAME\" FROM \"TBLSTOREMAST\" where \"SM_STATUS\" ='A'", "--Select--", ddlFromStore);
                    if (Request.QueryString["QryInvoiceId"] != null && Request.QueryString["QryInvoiceId"].ToString() != "")
                    {
                        txtInvoiceId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryInvoiceId"]));

                        LoadReceivedInvoiceDetails(txtInvoiceId.Text);

                        if (Request.QueryString["RefType"] != null && Request.QueryString["RefType"].ToString() != "")
                        {
                            string sRefType = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["RefType"]));

                            EnableDisableControl(sRefType);
                        }
                      
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

        public void LoadInvoiceDetails(string strInvoicetId)
        {
            try
            {
                clsReceiveTrans objInvoice = new clsReceiveTrans();
                objInvoice.sInvoiceId = Convert.ToString(strInvoicetId);
                objInvoice.sInvoiceNo = txtInvoiceNumber.Text.Replace("'", "");

                objInvoice.LoadInvoiceDetails(objInvoice);

                txtInvoiceId.Text = objInvoice.sInvoiceId;
                txtInvoiceNumber.Text = objInvoice.sInvoiceNo;
                ddlFromStore.SelectedValue = objInvoice.sFromStoreId;
                txtInvoicetDate.Text = objInvoice.sInvoiceDate;
                txtQuantity.Text = objInvoice.sQuantity;
                LoadTcCapacity(txtInvoiceId.Text);
                cmdSave.Enabled = true;

                txtIndentDate.Text = objInvoice.sIndentDate;
                txtIndentNumber.Text = objInvoice.sIndentNo;
                txtIndentId.Text = objInvoice.sIndentId;
                LoadIndentDetGrid(txtIndentId.Text);

            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadReceivedInvoiceDetails(string strInvoicetId)
        {
            try
            {
                clsReceiveTrans objInvoice = new clsReceiveTrans();
                objInvoice.sInvoiceId = Convert.ToString(strInvoicetId);
                objInvoice.sInvoiceNo = txtInvoiceNumber.Text.Replace("'", "");

                objInvoice.LoadReceivedInvoiceDetails(objInvoice);

                txtInvoiceId.Text = objInvoice.sInvoiceId;
                txtInvoiceNumber.Text = objInvoice.sInvoiceNo;
                ddlFromStore.SelectedValue = objInvoice.sFromStoreId;
                txtInvoicetDate.Text = objInvoice.sInvoiceDate;
                txtQuantity.Text = objInvoice.sQuantity;
                txtRemarks.Text = objInvoice.sRemarks;
                txtRVNo.Text = objInvoice.sRVNo;
                LoadTcCapacity(txtInvoiceId.Text);
                cmdSave.Enabled = true;

                txtIndentDate.Text = objInvoice.sIndentDate;
                txtIndentNumber.Text = objInvoice.sIndentNo;
                txtIndentId.Text = objInvoice.sIndentId;
                LoadIndentDetGrid(txtIndentId.Text);

            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadTcCapacity(string strInvoiceId)
        {
            DataTable dtTcCapacity = new DataTable();
            try
            {
                clsReceiveTrans objInvoice = new clsReceiveTrans();
                objInvoice.sInvoiceId = Convert.ToString(strInvoiceId);
                objInvoice.sOfficeCode = objSession.OfficeCode;

                dtTcCapacity = objInvoice.LoadCapacityGrid(objInvoice);

                grdTcTransfer.DataSource = dtTcCapacity;
                grdTcTransfer.DataBind();
                ViewState["dtTcCapacity"] = dtTcCapacity;
                grdTcTransfer.Visible = true;
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
                clsReceiveTrans objInvoice = new clsReceiveTrans();

                //View For Generate Report
                if (cmdSave.Text == "View")
                {

                    GenerateRecieptVoucherReport();                  
                    return;
                }

                SaveStoreInvoice();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID, "location.href='/Approval/ApprovalInbox.aspx';", true);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public string GetOfficeCodeFromStore()
        {
            try
            {
                clsStoreIndent objStoreIndent = new clsStoreIndent();
                string sOfficeCode = objStoreIndent.GetOfficeCodeFromStore(ddlFromStore.SelectedValue);
                return sOfficeCode;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        public void SaveStoreInvoice()
        {
            clsReceiveTrans objInvoice = new clsReceiveTrans();
            DataTable dtTCDetails;
            try
            {
                string[] Arr = new string[3];
                dtTCDetails = (DataTable)ViewState["TCDetails"];
                objInvoice.sInvoiceDate = txtInvoicetDate.Text.Replace("'", "");
                objInvoice.sInvoiceNo = txtInvoiceNumber.Text.Replace("'", "");
                objInvoice.sRemarks = txtRemarks.Text.Replace("'", "");
                objInvoice.sCreatedBy = objSession.UserId;
                objInvoice.sRVNo = txtRVNo.Text;

                objInvoice.sInvoiceId = txtInvoiceId.Text.Replace("'", "");
                objInvoice.sQuantity = txtQuantity.Text.Replace("'", "");
                objInvoice.sOfficeCode = objSession.OfficeCode;
                objInvoice.sRefofficecode =GetOfficeCodeFromStore();

                //Workflow
                WorkFlowObjects(objInvoice);

                Arr = objInvoice.RecieveTransformer(objInvoice);

                if (objSession.sTransactionLog == "1")
                {
                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (ReceiveTransCreate) InterStore ");
                }

                if (Arr[1].ToString() == "0")
                {
                    //txtSiId.Text = objTransfer.sSiId;
                    cmdSave.Enabled = false;
                    txtInvoiceId.Text = objInvoice.sInvoiceId;
                    LoadTcCapacity(objInvoice.sInvoiceId);
                    ShowMsgBox(Arr[0]);
                    GenerateRecieptVoucherReport();
                    return;
                }
                else
                {
                    cmdSave.Enabled = false;
                    //LoadTcCapacity(objInvoice.sInvoiceId);
                    ShowMsgBox(Arr[0]); 
                    return;
                }
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
                if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
                {
                    Response.Redirect("/Approval/ApprovalInbox.aspx", false);
                }
                else
                {
                    Response.Redirect("RecieveTransView.aspx", false);
                }

               
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void EnableDisableControl(string sRefType)
        {
            try
            {
                if (sRefType == "View")
                {
                    cmdSave.Visible = true;
                }
                if (sRefType == "Edit")
                {
                    cmdSave.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void LoadIndentDetGrid(string strIndentId)
        {
            DataTable dtIndentDet = new DataTable();
            try
            {
                clsReceiveTrans objInvoice = new clsReceiveTrans();
                objInvoice.sIndentId = Convert.ToString(strIndentId);
                objInvoice.sOfficeCode = objSession.OfficeCode;

                dtIndentDet = objInvoice.LoadIndentDetGrid(objInvoice);

                grdIndentDetails.DataSource = dtIndentDet;
                grdIndentDetails.DataBind();
                ViewState["dtIndDetails"] = dtIndentDet;
                grdIndentDetails.Visible = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void WorkFlowObjects(clsReceiveTrans objRecieveTrans)
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


                objRecieveTrans.sFormName = "RecieveTransCreate";
                objRecieveTrans.sOfficeCode = objSession.OfficeCode;
                objRecieveTrans.sClientIP = sClientIP;
                objRecieveTrans.sWFOId = hdfWFOId.Value;

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
                        hdfWFOId.Value = Convert.ToString(Session["WFOId"]);

                        Session["WFOId"] = null;
                        Session["WFDataId"] = null;
                        Session["WFOAutoId"] = null;

                        txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));
                    }

                    //SetControlText();
                    if (txtActiontype.Text == "V")
                    {
                        cmdSave.Text = "View";

                        //dvComments.Style.Add("display", "none");
                    }
                }
                else
                {
                    cmdSave.Text = "View";
                }

                DisableControlForView();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void DisableControlForView()
        {
            try
            {
                if (cmdSave.Text.Contains("View"))
                {
                    pnlApproval.Enabled = false;
                    pnlApproval1.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        #endregion

        public void GenerateRecieptVoucherReport()
        {
            try
            {

                string strParam = string.Empty;
                strParam = "id=RecieveDTR&InvoiceId=" + txtInvoiceId.Text;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdTcTransfer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdTcTransfer.PageIndex = e.NewPageIndex;
                LoadTcCapacity(txtInvoiceId.Text);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdIndentDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdIndentDetails.PageIndex = e.NewPageIndex;
                LoadIndentDetGrid(txtIndentId.Text);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}