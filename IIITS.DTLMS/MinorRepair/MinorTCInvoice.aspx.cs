using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Globalization;
using System.Data;
using System.Data.OleDb;

namespace IIITS.DTLMS.MinorRepair
{
    public partial class MinorTCInvoice : System.Web.UI.Page
    {
        string strFormCode = "MinorTCInvoice";
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
                txtTansDate.Attributes.Add("readonly", "readonly");

                txtTansDate_CalendarExtender1.EndDate = System.DateTime.Now;

                if (!IsPostBack)
                {
                    if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
                    {
                        txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));
                    }
                    if (Request.QueryString["WOId"] != null && Request.QueryString["WOId"].ToString() != "")
                    {
                        txtwrkId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["WOId"]));
                    }

                    if(txtActiontype.Text == "V")
                    {
                        if (Request.QueryString["sdataid"] != null && Request.QueryString["sdataid"].ToString() != "")
                        {
                            txtwrkId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["sdataid"]));
                        }
                        cmdSave.Text = "View";
                        cmdSave.Enabled = false;
                    }

                    GetMinorFailureDetails();
                    WorkFlowConfig();
                }
                    
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        //private void GenerateInvoiceNo()
        //{
        //    try
        //    {
        //        clsInvoice objInvoice = new clsInvoice();
        //        txtInvoiceNo.Text = objInvoice.GenerateInvoiceNo(objSession.OfficeCode);
        //        txtInvoiceNo.ReadOnly = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GenerateInvoiceNo");
        //    }
        //}


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
                        hdfApproveStatus.Value = Convert.ToString(Session["ApproveStatus"]);

                        Session["WFDataId"] = null;
                        Session["WFOId"] = null;
                        Session["WFOAutoId"] = null;
                        Session["ApproveStatus"] = null;
                    }


                    if (hdfWFDataId.Value != "0")
                    {
                        GetDataFromXML(hdfWFDataId.Value);
                    }
                }
                else
                {
                    cmdSave.Text = "View";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void GetDataFromXML(string sWFO_ID)
        {
            try
            {
                clsMinorTcComission objFailDetail = new clsMinorTcComission();
                objFailDetail.sWFO_id = sWFO_ID;
                objFailDetail.GetFailDetailsFromXML(objFailDetail);
                txtComment.Text = objFailDetail.sApproveComment;
                txtTansDate.Text = objFailDetail.sReceivedate;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetMinorFailureDetails()
        {
            DataTable dtFailDetails = new DataTable();
            try
            {
                clsMinorTcComission objFailDetail = new clsMinorTcComission();
                dtFailDetails = objFailDetail.GetFailDetails(txtwrkId.Text);
                txtTCCode.Text = dtFailDetails.Rows[0]["RD_NEW_TCCODE"].ToString();
                txtTcCapacity.Text = dtFailDetails.Rows[0]["TC_CAPACITY"].ToString();
                txtSLNo.Text = dtFailDetails.Rows[0]["TC_SLNO"].ToString();
                txtWorkOrderNo.Text = dtFailDetails.Rows[0]["WO_NO"].ToString();
                TxtDtcCode.Text = dtFailDetails.Rows[0]["DF_DTC_CODE"].ToString();
                txtFailureId.Text = dtFailDetails.Rows[0]["DF_ID"].ToString();
                txtReceiveDate.Text = dtFailDetails.Rows[0]["RD_RECEIVE_DATE"].ToString();
            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }                       

        protected void cmdSave_Click(object sender, EventArgs e)
        {            
            try
            {
                bool bAccResult = true;
                if (cmdSave.Text == "Update")
                {
                    bAccResult = CheckAccessRights("3");
                }
                else if (cmdSave.Text == "Save")
                {
                    bAccResult = CheckAccessRights("1");
                }

                if (bAccResult == false)
                {
                    return;
                }

                if (cmdSave.Text == "View")
                {
                    if (hdfApproveStatus.Value != "")
                    {
                        if (hdfApproveStatus.Value == "1" || hdfApproveStatus.Value == "2")
                        {

                            //GenerateIndentReport();
                        }
                    }
                    else
                    {
                        //GenerateIndentReport();
                    }
                    return;
                }
                if (ValidateForm() == true)
                {
                    string[] Arr = new string[2];
                    clsMinorTcComission objcomission = new clsMinorTcComission();
                    objcomission.sReceivedate = txtTansDate.Text;
                    objcomission.sApproveComment = txtComment.Text;
                    objcomission.sRsd_id = txtwrkId.Text;
                    objcomission.sCrBy = objSession.UserId;
                    objcomission.sFailureID = txtFailureId.Text;
                    objcomission.sDTRCode = txtTCCode.Text;
                    objcomission.sDtcCode = TxtDtcCode.Text;

                    objcomission.sOldDtcCode = objcomission.GetOldDTCCode(txtTCCode.Text);

                    if (txtActiontype.Text == "A" || txtActiontype.Text == "R" || txtActiontype.Text == "D")
                    {
                        if (hdfWFDataId.Value != "0")
                        {

                            ApproveRejectAction();

                            if (objSession.sTransactionLog == "1")
                            {
                                Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (MinorTCInvoice) Failure ");
                            }
                            return;
                        }
                    }
                    WorkFlowObjects(objcomission);

                    #region Modify and Approve

                    // For Modify and Approve
                    if (txtActiontype.Text == "M")
                    {
                        if (txtComment.Text.Trim() == "")
                        {
                            ShowMsgBox("Enter Comments/Remarks");
                            txtComment.Focus();
                            return;

                        }
                        objcomission.sActionType = txtActiontype.Text;
                        objcomission.sCrBy = objSession.UserId;

                        Arr = objcomission.SaveDetails(objcomission);

                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (MinorTCInvoice) Failure ");
                        }

                        if (Arr[1].ToString() == "0")
                        {
                            hdfWFDataId.Value = objcomission.sWFDataId;
                            ApproveRejectAction();
                            return;
                        }
                        if (Arr[1].ToString() == "2")
                        {
                            ShowMsgBox(Arr[0]);
                            return;
                        }
                    }

                    Arr = objcomission.SaveDetails(objcomission);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (MinorTCInvoice) Failure ");
                    }


                    if (Arr[1].ToString() == "0")
                    {
                        ShowMsgBox(Arr[0]);
                        cmdSave.Text = "Update";
                        //GenerateIndentReport();
                        cmdSave.Enabled = false;
                        return;
                    }
                    if (Arr[1].ToString() == "1")
                    {
                        if (cmdSave.Text == "Update")
                        {
                            ShowMsgBox(Arr[0]);
                            return;
                        }
                        //if (txtActiontype.Text == "A" || txtActiontype.Text == "R")
                        //{
                        //    ApproveRejectAction();
                        //    return;
                        //}
                        if (txtActiontype.Text == "M")
                        {
                            ShowMsgBox("Modified and Approved Successfully");
                        }
                        else
                        {

                            //GenerateIndentReport();
                        }
                        return;

                    }
                    if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0]);
                        return;
                    }

                    #endregion
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void WorkFlowObjects(clsMinorTcComission objComission)
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


                objComission.sFormName = "MinorTCInvoice";
                objComission.sOfficeCode = objSession.OfficeCode;
                objComission.sClientIP = sClientIP;
                objComission.sWFOId = hdfWFOId.Value;
                objComission.sWFAutoId = hdfWFOAutoId.Value;

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
                if (txtActiontype.Text == "A")
                {
                    objApproval.sApproveStatus = "1";
                }
                //Reject
                if (txtActiontype.Text == "R")
                {
                    objApproval.sApproveStatus = "3";
                }
                //Abort
                if (txtActiontype.Text == "D")
                {
                    objApproval.sApproveStatus = "4";
                }
                //Modify and Approve
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

                bool bResult = false;
                if (txtActiontype.Text == "M")
                {
                    objApproval.sWFDataId = hdfWFDataId.Value;
                    if (hdfRejectApproveRef.Value == "RA")
                    {
                        objApproval.sApproveStatus = "1";
                    }
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
                        cmdSave.Enabled = false;
                        //if (txtType.Text != "3")
                        //{
                        //    if (objSession.RoleId == "1")
                        //    {
                        //        txtIndentId.Text = objApproval.sNewRecordId;
                        //        GenerateIndentReport();

                        //    }
                        //}
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
                        //if (txtType.Text != "3")
                        //{
                        //    if (objSession.RoleId == "1")
                        //    {
                        //        txtIndentId.Text = objApproval.sNewRecordId;
                        //        GenerateIndentReport();

                        //    }
                        //}
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


        public bool ValidateForm()
        {
            bool bValidate = false;

            try
            {
                if (txtTansDate.Text.Trim().Length == 0)
                {
                    txtTansDate.Focus();
                    ShowMsgBox("Enter Replacement Date");
                    return bValidate;
                }
                string sResult = Genaral.DateComparisionTransaction(txtTansDate.Text, txtReceiveDate.Text, false, false);
                if (sResult == "1")
                {
                    ShowMsgBox("Transactional Date should be Greater than DTR Receive Date");
                    return bValidate;
                }
                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                bValidate = false;
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return bValidate;
            }
        }

        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "MinorTCInvoice";
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

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtComment.Text = string.Empty;
                txtTansDate.Text = string.Empty;
            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void Reset()
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdViewDTR_Click(object sender, EventArgs e)
        {
            try
            {
                string sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt("1"));
                string sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtwrkId.Text));
                string sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt("V"));
                string sFailType = HttpUtility.UrlEncode(Genaral.UrlEncrypt("1"));

                string url = "/MinorRepair/ReceiveMinorTC.aspx?TypeValue=" + sTaskType + "&ReferID=" + sRecordId + "&ActionType=" + sActionType + "&FailType=" + sFailType;
                string s = "window.open('" + url + "', '_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
            catch(Exception ex)
            {
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
    }
}