using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using IIITS.DTLMS.BL.Billing;

namespace IIITS.DTLMS.Billing
{
    public partial class MajorPaymentDetails : System.Web.UI.Page
    {
        string strFormCode = "MajorPaymentDetails";
        clsSession objSession;
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
                    objSession = (clsSession)Session["clsSession"];
                    lblMessage.Text = string.Empty;
                    txtDate.Attributes.Add("readonly", "readonly");

                    CalendarExtender1_txtDate.EndDate = System.DateTime.Now;

                    if (!IsPostBack)
                    {
                        if (Request.QueryString["WOId"] != null && Convert.ToString(Request.QueryString["WOId"]) != "")
                        {
                            txtInvId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["WOId"]));
                        }
                        WorkFlowConfig();
                        GetInvoiceDetails();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        private void GetInvoiceDetails()
        {
            try
            {
                string sInvId = txtInvId.Text;
                clsMajorBilling obj = new clsMajorBilling();
                obj.sInvID = sInvId;
                obj.GetInvoiceDetails(obj);
                txtInvNo.Text = obj.sInvNo;
                txtInvdate.Text = obj.sInvDate;
                txtInvAmount.Text = obj.sInvAmount;
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
                if (Request.QueryString["ActionType"] != null && Convert.ToString(Request.QueryString["ActionType"]) != "")
                {
                    if (Session["WFOId"] != null && Convert.ToString(Session["WFOId"]) != "")
                    {
                        hdfWFDataId.Value = Convert.ToString(Session["WFDataId"]);
                        hdfWFOId.Value = Convert.ToString(Session["WFOId"]);
                        hdfApproveStatus.Value = Convert.ToString(Session["ApproveStatus"]);
                        hdfWFOAutoId.Value = Convert.ToString(Session["WFOAutoId"]);

                        Session["WFDataId"] = null;
                        Session["WFOId"] = null;
                        Session["ApproveStatus"] = null;
                        Session["WFOAutoId"] = null;
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

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                string[] Arr = new string[2];
                clsMajorBilling obj = new clsMajorBilling();
                obj.sInvID = txtInvId.Text;
                obj.sPaymentMode = cmbPaymode.SelectedValue;
                obj.sRefNo = txtRefno.Text.Replace("'", "").Trim();
                obj.sTransdate = txtDate.Text.Replace("'", "").Trim();
                obj.sBillNo = txtBillNo.Text.Replace("'", "").Trim();
                obj.sUserId = objSession.UserId;
                obj.sOfficeCode = objSession.OfficeCode;

                if (cmbPaymode.SelectedItem.Text == "--SELECT--")
                {
                    ShowMsgBox("Please select the Payment Mode");
                    cmbPaymode.Focus();
                    return;
                }
                if (txtDate.Text.Trim() == string.Empty)
                {
                    ShowMsgBox("Please Enter the Transaction Date");
                    txtDate.Focus();
                    return;
                }
                if (txtBillNo.Text.Trim() == string.Empty)
                {
                    ShowMsgBox("Please Enter the Bill Number");
                    txtBillNo.Focus();
                    return;
                }
                WorkFlowObjects(obj);
                Arr = obj.SaveTransactionDetails(obj);

                if (objSession.sTransactionLog == "1")
                {
                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "Billing");
                }

                if (Arr[0] == "0")
                {
                    cmdSave.Enabled = false;
                }
                ShowMsgBox(Arr[1]);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void WorkFlowObjects(clsMajorBilling objBilling)
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


                objBilling.strFormCode = "MajorDTRBilling";
                objBilling.sOfficeCode = objSession.OfficeCode;
                objBilling.sClientIp = sClientIP;
                objBilling.sWFO_id = hdfWFOId.Value;
                objBilling.sWFAutoId = hdfWFOAutoId.Value;

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

        protected void lnkInvDetails_Click(object sender, EventArgs e)
        {
            try
            {
                string sInvId = txtInvId.Text;
                clsMajorBilling obj = new clsMajorBilling();
                obj.sInvID = sInvId;
                obj.GetEstimationNo(obj);
                string sEstNo = obj.sEstNo;

                string sEstId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sEstNo));
                sInvId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sInvId));
                // Response.Redirect("DTRBilling.aspx?EstId=" + sEstId + "&InvId=" + sInvId + "", false);

                string url = "MajorDTRBilling.aspx?EstId=" + sEstId + "&InvId=" + sInvId;
                string s = "window.open('" + url + "', '_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);

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
                Response.Redirect("~/Approval/ApprovalInbox.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}