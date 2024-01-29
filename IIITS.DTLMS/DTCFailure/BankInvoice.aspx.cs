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

namespace IIITS.DTLMS.DTCFailure
{
    public partial class BankInvoice : System.Web.UI.Page
    {
        string strFormCode = "BankInvoice";
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
                    Form.DefaultButton = cmdSave.UniqueID;
                    lblMessage.Text = string.Empty;
                    txtInvoiceDate.Attributes.Add("readonly", "readonly");

                    txtInvoiceDate_CalendarExtender1.EndDate = System.DateTime.Now;

                    if (!IsPostBack)
                    {

                        if (Request.QueryString["TypeValue"] != null && Convert.ToString(Request.QueryString["TypeValue"]) != "")
                        {
                            txtType.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TypeValue"]));
                            ChangeLabelText();

                            hdfType.Value = txtType.Text;
                            txtInvoiceDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                        }

                        if (Request.QueryString["ReferID"] != null && Convert.ToString(Request.QueryString["ReferID"]) != "")
                        {
                            txtIndentId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ReferID"]));
                            if (Request.QueryString["InvoiceId"] != null && Convert.ToString(Request.QueryString["InvoiceId"]) != "")
                            {
                                txtInvoiceSlNo.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["InvoiceId"]));
                            }

                            GetBasicDetails();

                            if (txtInvoiceSlNo.Text != "0" && txtInvoiceSlNo.Text != "")
                            {
                                if (!txtInvoiceSlNo.Text.Contains("-"))
                                {
                                    GetInvoiceDetails();
                                }
                                txtIndentNo.ReadOnly = true;
                                cmdSave.Text = "View";
                            }
                            else
                            {
                                txtInvoiceSlNo.Text = "";
                            }
                        }

                        //Search Window Call
                        LoadSearchWindow();

                        //WorkFlow / Approval
                        WorkFlowConfig();

                        if (objSession.sRoleType == "2")
                        {
                            Session["BOID"] = "13";
                            ViewState["BOID"] = "13";
                        }
                        else
                        {
                            ViewState["BOID"] = Convert.ToString(Session["BOID"]);
                        }
                    }
                    ApprovalHistoryView.BOID = Convert.ToString(ViewState["BOID"]);
                    ApprovalHistoryView.sRecordId = txtInvoiceSlNo.Text;

                    if (txtActiontype.Text == "M")
                    {
                        clsApproval objLevel = new clsApproval();
                        string sLevel = objLevel.sGetApprovalLevel(Convert.ToString(ViewState["BOID"]), objSession.RoleId);
                        if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                        {
                            cmdSave.Text = " Modify and Submit";
                        }
                        else if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                        {
                            cmdSave.Text = " Modify and Approve";
                        }
                    }
                    else if (txtActiontype.Text == "A")
                    {
                        clsApproval objLevel = new clsApproval();
                        string sLevel = objLevel.sGetApprovalLevel(Convert.ToString(ViewState["BOID"]), objSession.RoleId);
                        if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                        {
                            cmdSave.Text = "Submit";
                        }
                        else if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                        {
                            cmdSave.Text = "Approve";
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

        protected void cmdClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["ActionType"] != null && Convert.ToString(Request.QueryString["ActionType"]) != "")
                {
                    Response.Redirect("/Approval/ApprovalInbox.aspx", false);
                }
                else
                {
                    Response.Redirect("InvoiceView.aspx", false);
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

                //Check AccessRights
                bool bAccResult = true;
                if (cmdSave.Text == "Update")
                {
                    bAccResult = CheckAccessRights("3");
                }
                else if (cmdSave.Text == "Save" || cmdSave.Text == "Submit" || cmdSave.Text == "Approve")
                {
                    bAccResult = CheckAccessRights("2");
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
                            GenerateInvoiceReport();
                        }
                    }
                    else
                    {
                        GenerateInvoiceReport();
                    }
                    return;
                }

                if (ValidateForm() == true)
                {

                    string[] Arr = new string[2];
                    clsInvoice objInvoice = new clsInvoice();

                    objInvoice.sDtcFailId = txtFailureId.Text;
                    objInvoice.sIndentNo = txtIndentNo.Text;

                    objInvoice.sIndentId = txtIndentId.Text;
                    objInvoice.sInvoiceNo = txtInvoiceNo.Text;

                    objInvoice.sInvoiceSlNo = txtInvoiceSlNo.Text;

                    objInvoice.sInvoiceDate = txtInvoiceDate.Text;
                    objInvoice.sInvoiceDescription = txtDrawingDescription.Text.Replace("'", "");
                    objInvoice.sAmount = txtAmount.Text;
                    objInvoice.sCreatedBy = objSession.UserId;
                    objInvoice.sManualInvoiceNo = txtManualInvNo.Text;

                    objInvoice.sTcCode = txtTCCode.Text;
                    objInvoice.sTcMake = txtTcMake.Text;
                    objInvoice.sTcCapacity = txtTcCapacity.Text;
                    if (objSession.sRoleType == "1")
                    {
                        if(objSession.OfficeCode.Length >= 3)
                        {
                            objInvoice.sOfficeCode = clsStoreOffice.GetStoreID( objSession.OfficeCode);
                        }
                        else
                        {
                            objInvoice.sOfficeCode = objSession.OfficeCode;
                        }
                        
                    }
                    else
                    {
                        objInvoice.sOfficeCode = Convert.ToString(ViewState["LOCCODE"]);
                    }

                    objInvoice.sDTCName = txtDTCName.Text;
                    objInvoice.sDTCCode = txtDTCCode1.Text;
                    objInvoice.sTcNewCapacity = txtNewtccapacity.Text;

                    objInvoice.sTaskType = txtType.Text;


                    //Workflow
                    WorkFlowObjects(objInvoice);

                    Arr = objInvoice.SaveUpdateInvoiceDetails(objInvoice);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (BankInvoice) Failure ");
                    }

                    if (Arr[1].ToString() == "0")
                    {
                        ShowMsgBox(Arr[0]);
                        txtInvoiceSlNo.Text = objInvoice.sInvoiceSlNo;
                        if (txtType.Text != "3")
                        {
                            GenerateInvoiceReport();
                        }
                        cmdSave.Text = "Update";
                        cmdGatePass.Enabled = true;
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

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }

        bool ValidateForm()
        {
            bool bValidate = false;

            try
            {

                if (txtInvoiceNo.Text.Length == 0)
                {
                    txtInvoiceNo.Focus();
                    ShowMsgBox("Enter Invoice No");
                    return bValidate;
                }
                if (txtInvoiceDate.Text.Length == 0)
                {
                    txtInvoiceDate.Focus();
                    ShowMsgBox("Enter invoice Date");
                    return bValidate;
                }
                string sResult = Genaral.DateComparision(txtInvoiceDate.Text, txtIndentdate.Text, false, false);
                if (sResult == "2")
                {
                    ShowMsgBox("Commisioning Invoice Date should be Greater than Indent Date");
                    return bValidate;
                }
                //if (hdfFailureDate.Value != "")
                //{
                //    sResult = Genaral.DateComparision(txtInvoiceDate.Text, hdfFailureDate.Value, false, false);
                //    if (sResult == "2")
                //    {
                //        ShowMsgBox("Commisioning Invoice Date should be Greater than Failure Date");
                //        return bValidate;
                //    }
                //}
                if (txtAmount.Text.Length == 0)
                {
                    txtAmount.Focus();
                    ShowMsgBox("Enter Amount");
                    return bValidate;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtAmount.Text, "^(\\d{1,6})?(\\.\\d{1,2})?$"))
                {
                    ShowMsgBox("Enter Valid Amount (eg:111111.00)");
                    return false;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(txtAmount.Text, "[-+]?[0-9]{0,5}\\.?[0-9]{0,2}"))
                {
                    ShowMsgBox("Enter Valid Amount (eg:111111.00)");
                    return false;
                }
                //if (txtDrawingDescription.Text.Length == 0)
                //{
                //    txtDrawingDescription.Focus();
                //    ShowMsgBox("Enter Invoice/Drawing Description");
                //    return bValidate;
                //}
                if (txtTCCode.Text.Trim().Length == 0)
                {
                    txtTCCode.Focus();
                    ShowMsgBox("Enter valid DTR Code");
                    return bValidate;
                }



                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                bValidate = false;
                return bValidate;
            }
        }


        public void GetInvoiceDetails()
        {
            try
            {
                clsInvoice objInvoice = new clsInvoice();

                objInvoice.sInvoiceSlNo = txtInvoiceSlNo.Text;

                objInvoice.GetInvoiceDetails(objInvoice);

                txtTCCode.Text = objInvoice.sTcCode;

                txtTcMake.Text = objInvoice.sTcMake;
                txtTcCapacity.Text = objInvoice.sTcCapacity;

                txtInvoiceSlNo.Text = objInvoice.sInvoiceSlNo;
                txtInvoiceNo.Text = objInvoice.sInvoiceNo;
                txtDrawingDescription.Text = objInvoice.sInvoiceDescription;
                txtAmount.Text = objInvoice.sAmount;
                txtInvoiceDate.Text = objInvoice.sInvoiceDate;
                txtManualInvNo.Text = objInvoice.sManualInvoiceNo;
                txtSLNo.Text = objInvoice.sTcSlNo;
                txtTCCode.Enabled = false;

                //cmdSave.Text = "Update";

                GetGatePassDetails();
                cmdGatePass.Enabled = true;

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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                clsInvoice objInvoice = new clsInvoice();
                // txtTCCode.Text = hdfTCCode.Value;
                objInvoice.sTcCode = txtTCCode.Text;
                if (objSession.sRoleType == "1")
                {
                    objInvoice.sOfficeCode = objSession.OfficeCode;
                }
                else
                {
                    objInvoice.sOfficeCode = Convert.ToString(ViewState["LOCCODE"]);
                }

                objInvoice.sTcCapacity = txtNewtccapacity.Text;
                objInvoice.GetTCDetails(objInvoice);

                if (objInvoice.sTcCode == "")
                {
                    ShowMsgBox("TC NOT IN STORE OR GOOD CONDITION");
                    txtTCCode.Text = "";
                }
                else
                {
                    txtTcMake.Text = objInvoice.sTcMake;
                    txtTcCapacity.Text = objInvoice.sTcCapacity;
                    txtTCCode.Text = objInvoice.sTcCode;
                    txtSLNo.Text = objInvoice.sTcSlNo;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void ValidateFormUpdate()
        {
            try
            {
                clsInvoice objInvoice = new clsInvoice();
                if (objInvoice.ValidateUpdate(txtInvoiceSlNo.Text) == true)
                {
                    cmdSave.Enabled = false;
                }
                else
                {
                    cmdSave.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }



        private void GetBasicDetails()
        {
            try
            {


                if (txtType.Text != "3")
                {
                    clsInvoice objInvoice = new clsInvoice();
                    objInvoice.sIndentId = txtIndentId.Text;
                    objInvoice.GetBasicDetails(objInvoice);

                    txtIndentNo.Text = objInvoice.sIndentNo;
                    txtIndentdate.Text = objInvoice.sIndentDate;
                    txtRaisedBy.Text = objInvoice.sIndentCrby;
                    txtFailureId.Text = objInvoice.sDtcFailId;
                    txtDTCName.Text = objInvoice.sDTCName;
                    txtDTCCode1.Text = objInvoice.sDTCCode;
                    txtOldTcCode.Text = objInvoice.sOldTcCode;
                    txtoldtccapacity.Text = objInvoice.sTcCapacity;
                    txtNewtccapacity.Text = objInvoice.sTcNewCapacity;
                    txtDTCId.Text = objInvoice.sDTCId;
                    txtTCId.Text = objInvoice.sTCId;
                    txtAmount.Text = objInvoice.sAmount;
                    hdfFailureDate.Value = objInvoice.sFailDate;

                }
                else
                {
                    clsIndent objIndent = new clsIndent();
                    objIndent.sIndentId = txtIndentId.Text;
                    objIndent.GetIndentDetails(objIndent);

                    txtIndentNo.Text = objIndent.sIndentNo;
                    txtIndentdate.Text = objIndent.sIndentDate;
                    txtRaisedBy.Text = objIndent.sCrBy;
                    txtNewtccapacity.Text = objIndent.sRequstedCapacity;

                }



            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void ChangeLabelText()
        {
            try
            {
                if (txtType.Text == "1" || txtType.Text == "4")
                {
                    lblIDText.Text = "Failure ID";
                }
                else if (txtType.Text == "2")
                {
                    lblIDText.Text = "Enhancement ID";
                }
                else
                {
                    dvOld.Style.Add("display", "none");
                    txtFailureId.Visible = false;
                    dvFailure.Style.Add("display", "none");
                    lnkDTCDetails.Visible = false;
                    lnkDTrDetails.Visible = false;

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdSearchIndent_Click(object sender, EventArgs e)
        {
            try
            {
                txtIndentId.Text = hdfIndentId.Value;
                GetBasicDetails();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetGatePassDetails()
        {
            try
            {
                clsInvoice objInvoice = new clsInvoice();
                objInvoice.sInvoiceNo = txtInvoiceNo.Text;
                objInvoice.GetGatePassDetials(objInvoice);

                txtChallen.Text = objInvoice.sChallenNo;
                txtVehicleNo.Text = objInvoice.sVehicleNumber;
                txtReciepient.Text = objInvoice.sReceiptientName;

                if (txtVehicleNo.Text != "")
                {
                    txtChallen.ReadOnly = true;
                    txtVehicleNo.ReadOnly = true;
                    txtReciepient.ReadOnly = true;
                }
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

                objApproval.sFormName = "BankInvoice";
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

        public void WorkFlowObjects(clsInvoice objInvoice)
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


                objInvoice.sFormName = "BankInvoice";
                if (objSession.sRoleType == "1")
                {
                    objInvoice.sOfficeCode = objSession.OfficeCode;
                }
                else
                {
                    objInvoice.sOfficeCode = Convert.ToString(ViewState["LOCCODE"]);
                }
                objInvoice.sClientIP = sClientIP;
                objInvoice.sWFOId = hdfWFOId.Value;
                objInvoice.sWFAutoId = hdfWFOAutoId.Value;

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
                    cmdSave.Text = "Approve";
                    pnlApproval.Enabled = true;
                }
                if (txtActiontype.Text == "R")
                {
                    cmdSave.Text = "Reject";
                    pnlApproval.Enabled = false;
                }
                if (txtActiontype.Text == "M")
                {
                    //cmdSave.Text = "Modify and Approve";                    
                    pnlApproval.Enabled = true;
                }

                dvComments.Style.Add("display", "block");
                //cmdReset.Enabled = false;


                if (hdfWFOAutoId.Value != "0")
                {
                    dvComments.Style.Add("display", "none");
                }


                // Check for Creator of Form
                bool bResult = CheckFormCreatorLevel();
                if (bResult == true)
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
                if (objSession.sRoleType == "1")
                {
                    objApproval.sOfficeCode = objSession.OfficeCode;
                }
                else
                {
                    objApproval.sOfficeCode = Convert.ToString(ViewState["LOCCODE"]);
                }

                objApproval.sApproveComments = txtComment.Text.Trim();
                objApproval.sWFObjectId = hdfWFOId.Value;
                objApproval.sWFAutoId = hdfWFOAutoId.Value;

                if (txtActiontype.Text == "A")
                {
                    objApproval.sApproveStatus = "1";
                }
                if (txtActiontype.Text == "R")
                {
                    objApproval.sApproveStatus = "3";
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

                bool bResult = objApproval.ApproveWFRequest(objApproval);
                if (bResult == true)
                {

                    if (objApproval.sApproveStatus == "1")
                    {
                        ShowMsgBox("Approved Successfully");
                        cmdSave.Enabled = false;
                    }
                    else if (objApproval.sApproveStatus == "3")
                    {

                        ShowMsgBox("Rejected Successfully");
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

        public void WorkFlowConfig()
        {
            try
            {
                if (Request.QueryString["ActionType"] != null && Convert.ToString(Request.QueryString["ActionType"]) != "")
                {


                    if (Session["WFOId"] != null && Convert.ToString(Session["WFOId"]) != "")
                    {
                        // txtWFDataId.Text = Session["WFDataId"].ToString();
                        hdfWFOId.Value = Convert.ToString(Session["WFOId"]);
                        hdfWFOAutoId.Value = Convert.ToString(Session["WFOAutoId"]);
                        hdfApproveStatus.Value = Convert.ToString(Session["ApproveStatus"]);

                        //Session["WFDataId"] = null;
                        Session["WFOId"] = null;
                        Session["WFOAutoId"] = null;
                        Session["ApproveStatus"] = null;
                    }

                    SetControlText();
                    ControlEnableDisable();
                    if (txtActiontype.Text == "V")
                    {

                        cmdSave.Text = "View";
                        dvComments.Style.Add("display", "none");
                    }
                    if (objSession.sRoleType == "2")
                    {
                        string sLocCode = clsStoreOffice.GetCurrentOfficeCode(hdfWFOAutoId.Value, "2");
                        if (sLocCode.Length > 0)
                        {
                            sLocCode = sLocCode.Substring(0, Constants.Division);
                        }
                        ViewState["LOCCODE"] = sLocCode;
                    }
                    GenerateInvoiceNo();
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

        public bool CheckFormCreatorLevel()
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                string sResult = objApproval.GetFormCreatorLevel("", objSession.RoleId, "BankInvoice");
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

        public void DisableControlForView()
        {
            try
            {
                if (cmdSave.Text.Contains("View"))
                {
                    pnlApproval.Enabled = false;
                    cmdSearchIndent.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        #endregion

        #region GatePass
        bool ValidateGatePass()
        {
            bool bValidate = false;

            try
            {

                if (txtVehicleNo.Text.Length == 0)
                {
                    txtVehicleNo.Focus();
                    ShowMsgBox("Enter Vehicle No");
                    return bValidate;
                }
                if (txtChallen.Text.Length == 0)
                {
                    txtChallen.Focus();
                    ShowMsgBox("Enter Challen Number");
                    return bValidate;
                }

                if (txtReciepient.Text.Trim().Length == 0)
                {
                    txtReciepient.Focus();
                    ShowMsgBox("Enter Reciepient Name");
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
        protected void cmdGatePass_Click(object sender, EventArgs e)
        {
            try
            {
                clsInvoice objInvoice = new clsInvoice();
                string[] Arr = new string[2];
                if (ValidateGatePass() == true)
                {
                    objInvoice.sGatePassId = txtGpId.Text;
                    objInvoice.sVehicleNumber = txtVehicleNo.Text.Replace("'", "");
                    objInvoice.sReceiptientName = txtReciepient.Text.Replace("'", "");
                    objInvoice.sChallenNo = txtChallen.Text.Replace("'", "");
                    objInvoice.sCreatedBy = objSession.UserId;
                    objInvoice.sTcCode = txtTCCode.Text.Replace("'", "");
                    objInvoice.sInvoiceNo = txtInvoiceNo.Text.Replace("'", "");
                    objInvoice.sDTCCode = txtDtcCode.Text.Replace("'", "");

                    Arr = objInvoice.SaveUpdateGatePassDetails(objInvoice);

                    if (Arr[1].ToString() == "0")
                    {
                        txtGpId.Text = objInvoice.sGatePassId;
                        string strParam = "id=GatePass&InvoiceId=" + txtInvoiceNo.Text;
                        RegisterStartupScript("PrintD", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

                        return;
                    }
                    if (Arr[1].ToString() == "1")
                    {
                        string strParam = "id=GatePass&InvoiceId=" + txtInvoiceNo.Text;
                        RegisterStartupScript("PrintD", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

                        return;
                    }

                    if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0]);
                        return;
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


        public void GenerateInvoiceNo()
        {
            try
            {
                clsInvoice objInvoice = new clsInvoice();
                if (objSession.sRoleType == "1")
                {
                    txtInvoiceNo.Text = objInvoice.GenerateInvoiceNo(objSession.OfficeCode, objSession.sRoleType);
                }
                else
                {
                    txtInvoiceNo.Text = objInvoice.GenerateInvoiceNo(Convert.ToString(ViewState["LOCCODE"]), objSession.sRoleType);
                }
                txtInvoiceNo.ReadOnly = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void txtIndentNo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //cmdSearchIndent_Click(sender, e);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void GenerateInvoiceReport()
        {
            try
            {
                if (txtType.Text == "1" || txtType.Text == "4")
                {                    
                    string strParam = string.Empty;
                    if (objSession.sRoleType == "1")
                    {
                        strParam = "id=InvoiceReport&InvoiceId=" + txtInvoiceSlNo.Text + "&OfficeCode=" + clsStoreOffice.GetStoreID( objSession.OfficeCode )+ "&Capacity=" + txtTcCapacity.Text;
                    }
                    else if (objSession.sRoleType == "2")
                    {
                        strParam = "id=InvoiceReport&InvoiceId=" + txtInvoiceSlNo.Text + "&OfficeCode=" + objSession.OfficeCode + "&Capacity=" + txtTcCapacity.Text;
                    }
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                }
                else if (txtType.Text == "2")
                {
                    string strParam = string.Empty;
                    if (objSession.sRoleType == "1")
                    {
                        strParam = "id=EnhanceInvoiceReport&InvoiceId=" + txtInvoiceSlNo.Text + "&OfficeCode=" + clsStoreOffice.GetStoreID(objSession.OfficeCode) + "&Capacity=" + txtTcCapacity.Text;
                    }
                    else if (objSession.sRoleType == "2")
                    {
                        strParam = "id=EnhanceInvoiceReport&InvoiceId=" + txtInvoiceSlNo.Text + "&OfficeCode=" + objSession.OfficeCode + "&Capacity=" + txtTcCapacity.Text;
                    }
                        
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void lnkDTrDetails_Click(object sender, EventArgs e)
        {
            try
            {
                string sTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtTCId.Text));

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

        public void ControlEnableDisable()
        {
            try
            {
                txtIndentNo.Enabled = false;
                cmdSearchIndent.Visible = false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void txtTCCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtTCCode.Text.Trim() != "")
                {
                    btnSearch_Click(sender, e);
                }
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
                string sTypeName = string.Empty;
                if (txtType.Text == "1" || txtType.Text == "4")
                {
                    sTypeName = "FAILURE";
                }
                else if (txtType.Text == "2")
                {
                    sTypeName = "ENHANCEMENT";
                }

                txtInvoiceDate.Attributes.Add("onblur", "return ValidateDate(" + txtInvoiceDate.ClientID + ");");

                string strQry = string.Empty;
                strQry = "Title=Search and Select DTr Details&";
                strQry += "Query=SELECT \"TC_CODE\",\"TM_NAME\", \"TC_CAPACITY\" AS TC_CAPACITY,CASE \"TC_STATUS\" WHEN 1 THEN 'BRAND NEW' WHEN 2 THEN 'REPAIRED GOOD' END TC_STATUS ";
                if(objSession.sRoleType == "2")
                {
                    strQry += " FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\" WHERE  CAST(\"TC_LOCATION_ID\" AS TEXT) = '" + objSession.sStoreID + "' AND \"TC_CURRENT_LOCATION\"='5' AND \"TC_STATUS\" IN (1,2)";
                }
                else if (objSession.sRoleType == "1")
                {
                    strQry += " FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\" WHERE  CAST(\"TC_LOCATION_ID\" AS TEXT) = '" + clsStoreOffice.GetStoreID( objSession.sStoreID) + "' AND \"TC_CURRENT_LOCATION\"='5' AND \"TC_STATUS\" IN (1,2)";
                }
                strQry += " AND \"TC_MAKE_ID\"= \"TM_ID\" and \"TC_STATUS\" in (1,2) AND \"TC_CURRENT_LOCATION\" =5 AND CAST(\"TC_CAPACITY\" AS INT) ='" + txtNewtccapacity.Text + "' AND {0} like %{1}% &";
                strQry += "DBColName=\"TC_SLNO\"~CAST(\"TC_CODE\" AS TEXT)~\"TM_NAME\"~CAST(\"TC_CAPACITY\" AS TEXT)&";
                strQry += "ColDisplayName=DTr SlNo~DTr Code~Make Name~Capacity&";

                strQry = strQry.Replace("'", @"\'");

                btnSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtTCCode.ClientID + "&btn=" + btnSearch.ClientID + "',520,520," + txtTCCode.ClientID + ")");

                if (txtType.Text != "3")
                {
                    strQry = "Title=Search and Select Indent Details&";
                    strQry += "Query=SELECT \"TI_ID\",\"DF_ID\",\"DT_NAME\",\"DT_CODE\",\"TI_INDENT_NO\" from \"TBLDTCMAST\",\"TBLDTCFAILURE\",\"TBLWORKORDER\",\"TBLINDENT\" ";
                    strQry += " WHERE \"DF_DTC_CODE\"= \"DT_CODE\" AND \"DF_REPLACE_FLAG\" =0 AND  \"DF_STATUS_FLAG\" =" + txtType.Text + " AND ";
                    strQry += " \"WO_DF_ID\" = \"DF_ID\" AND \"TI_WO_SLNO\" = \"WO_SLNO\" AND  \"TI_ID\" NOT IN (SELECT \"IN_TI_NO\" FROM \"TBLDTCINVOICE\") ";
                    if (objSession.sRoleType == "2")
                    {
                        strQry += " AND CAST(\"DF_LOC_CODE\" AS TEXT) LIKE '" + objSession.OfficeCode + "%' AND  {0} like %{1}% &";
                    }
                    else if (objSession.sRoleType == "1")
                    {
                        strQry += " AND CAST(\"DF_LOC_CODE\" AS TEXT) LIKE '" + clsStoreOffice.GetStoreID(objSession.OfficeCode) + "%' AND  {0} like %{1}% &";
                    }
                    strQry += "DBColName=\"DF_ID\"~\"DT_NAME\"~\"DT_CODE\"~\"TI_INDENT_NO\"&";
                    strQry += "ColDisplayName=" + sTypeName + " ID~Transformer Centre Name~Transformer Centre Code~Indent NO&";
                }
                else
                {
                    strQry = "Title=Search and Select Indent Details&";
                    strQry += "Query=SELECT \"TI_ID\",\"WO_NO\",\"TI_INDENT_NO\" ";
                    strQry += " FROM \"TBLWORKORDER\",\"TBLINDENT\" WHERE \"WO_SLNO\"=\"TI_WO_SLNO\" AND \"WO_REPLACE_FLG\"='0' AND \"WO_DF_ID\" IS NULL ";
                    if (objSession.sRoleType == "2")
                    {
                        strQry += " AND CAST(\"WO_OFF_CODE\" AS TEXT) LIKE '" + objSession.OfficeCode + "%' AND \"TI_ID\" NOT IN (SELECT \"IN_TI_NO\" FROM \"TBLDTCINVOICE\") AND {0} like %{1}% &";
                    }
                    else if (objSession.sRoleType == "1")
                    {
                        strQry += " AND CAST(\"WO_OFF_CODE\" AS TEXT) LIKE '" + clsStoreOffice.GetStoreID(objSession.OfficeCode) + "%' AND \"TI_ID\" NOT IN (SELECT \"IN_TI_NO\" FROM \"TBLDTCINVOICE\") AND {0} like %{1}% &";
                    }
                    strQry += "DBColName=\"WO_NO\"~\"TI_INDENT_NO\"&";
                    strQry += "ColDisplayName=Work Order No~Indent NO&";
                }


                strQry = strQry.Replace("'", @"\'");
                cmdSearchIndent.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + hdfIndentId.ClientID + "&btn=" + cmdSearchIndent.ClientID + "',520,520," + hdfIndentId.ClientID + ")");
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdViewIndent_Click(object sender, EventArgs e)
        {
            try
            {
                clsFormValues objApproval = new clsFormValues();
                string sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtType.Text));

                string sWOSlno = objApproval.GetWorkOrderId(txtIndentId.Text);
                sWOSlno = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWOSlno));
                string sIndentId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtIndentId.Text));

                string url = "/DTCFailure/IndentCreation.aspx?TypeValue=" + sTaskType + "&ReferID=" + sWOSlno + "&IndentId=" + sIndentId;
                string s = "window.open('" + url + "', '_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

    }
}