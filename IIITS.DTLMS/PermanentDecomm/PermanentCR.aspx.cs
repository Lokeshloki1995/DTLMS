using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;


namespace IIITS.DTLMS.PermanentDecomm
{
    public partial class PermanentCR : System.Web.UI.Page
    {
        string strFormCode = "PermanentCR";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;
                if (!IsPostBack)
                {
                    if (Request.QueryString["DecommId"] != null && Request.QueryString["DecommId"].ToString() != "")
                    {
                        txtDecommId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["DecommId"]));
                    }
                    if (Request.QueryString["TypeValue"] != null && Request.QueryString["TypeValue"].ToString() != "")
                    {
                        txtType.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TypeValue"]));

                    }


                    GetDetailsForCR();

                    //WorkFlow / Approval
                    WorkFlowConfig();

                    //ApprovalHistoryView.BOID = Session["BOID"].ToString();
                    //ApprovalHistoryView.sRecordId = txtDecommId.Text;
                    ViewState["BOID"] = Session["BOID"].ToString();
                    if (txtActiontype.Text == "M")
                    {
                        clsApproval objLevel = new clsApproval();
                        string sLevel = objLevel.sGetApprovalLevel(ViewState["BOID"].ToString(), objSession.RoleId);
                        if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                        {
                            cmdCR.Text = "Modify and Submit";
                        }
                        else if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                        {
                            cmdCR.Text = "Modify and Approve";
                        }
                    }
                    else if (txtActiontype.Text == "A")
                    {
                        clsApproval objLevel = new clsApproval();
                        string sLevel = objLevel.sGetApprovalLevel(ViewState["BOID"].ToString(), objSession.RoleId);
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
                ApprovalHistoryView.BOID = Session["BOID"].ToString();
                ApprovalHistoryView.sRecordId = txtDecommId.Text;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetDetailsForCR()
        {
            try
            {
                clsPermanentCR objRIApproval = new clsPermanentCR();
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

                txtDTCCode.Text = objRIApproval.sDTCCode;
                txtDTCId.Text = objRIApproval.sDTCId;
                txtFailureDTr.Text = objRIApproval.sTCCode;
                txtFailDTrId.Text = objRIApproval.sFailureTCId;
                txtNewDTr.Text = objRIApproval.sNewTCCode;
                txtNewDTrId.Text = objRIApproval.sNewTCId;
                hdfFailureId.Value = objRIApproval.sFailureId;

                txtInvQty.Text = objRIApproval.sInventoryQty;
                txtDecommInventry.Text = objRIApproval.sDecommInventoryQty;


                //if (txtType.Text == "2")
                //{
                //    lblFailDTr.Text = "Enhance DTr Code";
                //}
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
                       
                            GenerateCRReport();
                        
                    }
                    else
                    {
                        GenerateCRReport();
                    }
                    return;
                }

                clsPermanentCR objCR = new clsPermanentCR();
                string[] Arr = new string[2];



                objCR.sDTCCode = txtDTCCode.Text;
                objCR.sCrby = objSession.UserId;

                objCR.sInventoryQty = txtInvQty.Text;
                objCR.sDecommId = txtDecommId.Text;
                objCR.sDecommInventoryQty = txtDecommInventry.Text;
                objCR.sFailureId = hdfFailureId.Value;

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

                    objCR.sActionType = txtActiontype.Text;

                    Arr = objCR.SaveCompletionReport(objCR);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "(CR)PermanentDecommissioning");
                    }

                    if (Arr[1].ToString() == "0")
                    {
                        hdfWFDataId.Value = objCR.sWFDataId;
                        ApproveRejectAction();

                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "(CR)PermanentDecommissioning");
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

                if (txtActiontype.Text == "RA")
                {
                    ApproveRejectAction();

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "(CR)PermanentDecommissioning");
                    }
                    return;
                }

                if (objSession.RoleId == "4")
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
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "(CR)PermanentDecommissioning");
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
                    //txtWFDataId.Text = objRIApproval.sWFDataId;
                }

                if (txtActiontype.Text == "A" || txtActiontype.Text == "R")
                {
                    ApproveRejectAction();

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "(CR)PermanentDecommissioning");
                    }
                    return;
                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void GenerateCRReport()
        {
            try
            {                              
                    string strParam = "id=CRReportpermanent&DecommId=" + txtDecommId.Text;
                    RegisterStartupScript("PrintD", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
               
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
                bool bResult = objApproval.ApproveWFRequest(objApproval);
                if (bResult == true)
                {

                    if (objApproval.sApproveStatus == "1")
                    {
                        ShowMsgBox("Approved Successfully");

                       
                            GenerateCRReport();
                        
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
                        txtWFOId.Text = Convert.ToString(Session["WFOId"]);
                        txtWFOAuto.Text = Convert.ToString(Session["WFOAutoId"]);
                        hdfApproveStatus.Value = Convert.ToString(Session["ApproveStatus"]);

                        //Session["WFDataId"] = null;
                        Session["WFOId"] = null;
                        Session["WFOAutoId"] = null;
                        Session["ApproveStatus"] = null;
                    }


                    if (hdfWFDataId.Value != "0")
                    {
                        GetCRDetailsFromXML(hdfWFDataId.Value);
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

        public void WorkFlowObjects(clsPermanentCR objRIApproval)
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


                objRIApproval.sFormName = "PermanentCR";
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
                string sResult = objApproval.GetFormCreatorLevel("", objSession.RoleId, "PermanentCR");
                if (sResult == "1")
                {
                    if (txtActiontype.Text != "V")
                    {
                        txtcertify.Checked = false;
                        txtcertify.Enabled = true;
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

                clsPermanentCR objRIApproval = new clsPermanentCR();
                objRIApproval.sWFDataId = sWFDataId;

                objRIApproval.GetCRDetailsFromXML(objRIApproval);

                txtInvQty.Text = objRIApproval.sInventoryQty;
                txtDecommInventry.Text = objRIApproval.sDecommInventoryQty;

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

                string url = "/PermanentDecomm/PermanentRI.aspx?TypeValue=" + sTaskType + "&DecommId=" + sDecommId + "&FailureId=" + sFailureId;
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