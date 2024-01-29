using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;


namespace IIITS.DTLMS.Approval
{
    public partial class ApprovalHistory : System.Web.UI.Page
    {
        string strFormCode = "ApprovalHistory";
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
                    if (!IsPostBack)
                    {
                        if (Request.QueryString["RecordId"] != null && Convert.ToString(Request.QueryString["RecordId"]) != "")
                        {
                            txtRecordId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["RecordId"]));
                            txtBOId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["BOId"]));
                            LoadApprovalHistory();
                            GetCurrentStatus();
                            GetDTCDetails();

                            EnableDisableControls();
                        }
                    }
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
                String sShowMsg = string.Empty;
                sShowMsg = "<script language=javascript> alert ('" + sMsg + "')</script>";
                this.Page.RegisterStartupScript("Msg", sShowMsg);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadApprovalHistory()
        {
            try
            {
                clsAprovalHistory objHistory = new clsAprovalHistory();
                DataTable dt = new DataTable();

                dt = objHistory.LoadApprovalHistory(txtRecordId.Text,txtBOId.Text);
                if (dt.Rows.Count > 0)
                {
                    objHistory.OfcCode = Convert.ToString(dt.Rows[0]["WO_REF_OFFCODE"]);
                    hdfOfcCode.Value = objHistory.OfcCode;
                }
                grdApprovalHistory.DataSource = dt; 
                grdApprovalHistory.DataBind();
                ViewState["Approval"] = dt;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetCurrentStatus()
        {
            try
            {
                clsAprovalHistory objHistory = new clsAprovalHistory();
                objHistory.sRecordId = txtRecordId.Text;
                objHistory.sBOId = txtBOId.Text;
                objHistory.sroletype = objSession.sRoleType;
                objHistory.sroleID = objSession.RoleId;
                objHistory.OfcCode = hdfOfcCode.Value;
                objHistory.GetStatusofApproval(objHistory);
                lblCurrentStatus.Text = objHistory.sStatus;
                lblWorkName.Text = objHistory.sDescription;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void GetDTCDetails()
        {
            try
            {
                clsAprovalHistory objHistory = new clsAprovalHistory();

                objHistory.sRecordId = txtRecordId.Text;
                objHistory.sBOId = txtBOId.Text;

                objHistory.GetDTCDetails(objHistory);

                txtDTCCode.Text = objHistory.sDTCCode;
                txtDTCName.Text = objHistory.sDTCName;
                txtDTrCode.Text = objHistory.sDTRCode;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdApprovalHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdApprovalHistory.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Approval"];
                grdApprovalHistory.DataSource = dt;
                grdApprovalHistory.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void EnableDisableControls()
        {
            try
            {
                if (txtBOId.Text == "23" || txtBOId.Text == "24" || txtBOId.Text == "30" || txtBOId.Text=="32" || txtBOId.Text=="7" || txtBOId.Text=="15")
                {
                    dvDTCPanel.Style.Add("display", "none");
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