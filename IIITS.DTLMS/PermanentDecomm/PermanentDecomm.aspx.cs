using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IIITS.DTLMS.BL;
using System.Globalization;
using System.Data.OleDb;

namespace IIITS.DTLMS.PermanentDecomm
{
    public partial class PermanentDecomm : System.Web.UI.Page
    {
        string strFormCode = "PermanentDecomm";
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
                txtDecommDate.Attributes.Add("readonly", "readonly");
                txtRIDate.Attributes.Add("readonly", "readonly");
                TxtCommDate.Attributes.Add("readonly", "readonly");
                if (!IsPostBack)
                {
                    CalendarExtender2.EndDate = System.DateTime.Now;
                    Genaral.Load_Combo("SELECT \"SM_ID\",\"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"SM_STATUS\"='A' OR \"SM_ID\"=" + clsStoreOffice.GetZone_Circle_Div_Offcode(objSession.OfficeCode, objSession.RoleId) + "", cmbStore);

                    if (Request.QueryString["TypeValue"] != null && Request.QueryString["TypeValue"].ToString() != "")
                    {
                        txtType.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TypeValue"]));
                        ChangeLabelText();
                        GenerateRINo();
                    }

                    if (Request.QueryString["ReferID"] != null && Request.QueryString["ReferID"].ToString() != "")
                    {
                       // txtFailureId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ReferID"]));
                        txtFailureId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ReferID"]));

                        if (Request.QueryString["ReplaceId"] != null && Request.QueryString["ReplaceId"].ToString() != "")
                        {
                            txtReplaceId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ReplaceId"]));
                        }

                        hdfFailureId.Value = txtFailureId.Text;

                        GetBasicDetails();

                        if (txtReplaceId.Text != "0" && txtReplaceId.Text != "")
                        {
                            if (!txtReplaceId.Text.Contains("-"))
                            {
                                GetReplaceDetails();
                            }

                        }
                        else
                        {
                            txtReplaceId.Text = "";
                        }
                    }

                    //Search Window Call
                    LoadSearchWindow();

                    //WorkFlow / Approval
                    WorkFlowConfig();
                    if (objSession.RoleId == "4")
                    {
                        Session["BOID"] = "65";
                        ViewState["BOID"] = "65";
                    }
                    else
                    {
                        ViewState["BOID"] = Session["BOID"].ToString();
                    }


                }

                ApprovalHistoryView.BOID = ViewState["BOID"].ToString();
                ApprovalHistoryView.sRecordId = txtReplaceId.Text;

                if (txtActiontype.Text == "M")
                {
                    clsApproval objLevel = new clsApproval();
                    string sLevel = objLevel.sGetApprovalLevel(ViewState["BOID"].ToString(), objSession.RoleId);
                    if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                    {
                        cmdSave.Text = "Modify and Submit";
                    }
                    else if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                    {
                        cmdSave.Text = "Modify and Approve";
                    }
                }
                else if (txtActiontype.Text == "A")
                {
                    clsApproval objLevel = new clsApproval();
                    string sLevel = objLevel.sGetApprovalLevel(ViewState["BOID"].ToString(), objSession.RoleId);
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
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }


        }

        public void GetBasicDetails()
        {
            try
            {
                clsPermanentEstimation objFailure = new clsPermanentEstimation();
                txtFailureId.Text = hdfFailureId.Value;
                objFailure.sFailureId = txtFailureId.Text;

                objFailure.GettcDetails(objFailure);

                //    txtFailureDate.Text = objFailure.sFailureDate;
                txtDTCName.Text = objFailure.sDtcName;
                txtDTCCode.Text = objFailure.sDtcCode;
                txtMake.Text = objFailure.sDtcTcMake;
                txtTCCode.Text = objFailure.sDtcTcCode;
                txtCapcity.Text = objFailure.sDtcCapacity;
                // TxtCommDate.Text = objFailure.sDTrCommissionDate;

                hdfDTCId.Value = objFailure.sDtcId;
                hdfTCId.Value = objFailure.sTCId;

                //To get Invoice ID
                //  GetInvoiceNo();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public string getindentid()
        {
            string indtid=string.Empty;
            try
            {
                clsPermanentDecomm objFailure = new clsPermanentDecomm();
                txtFailureId.Text = hdfFailureId.Value;
                objFailure.sFailureId = txtFailureId.Text;

                indtid = objFailure.GetIndentId(objFailure);

               return indtid;

            

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return indtid;
            }
        }

        public void GetReplaceDetails()
        {
            try
            {
                clsPermanentDecomm objReplace = new clsPermanentDecomm();
                objReplace.sDecommId = txtReplaceId.Text;

                objReplace.GetDecommDetails(objReplace);

                txtRINo.Text = objReplace.sRINo;
                txtRIDate.Text = objReplace.sRIDate;
                txtRemarks.Text = objReplace.sRemarks;
                //txtRvNo.Text = objReplace.sRVNo;
                //txtRvDate.Text = objReplace.sRVDate;
                cmbStore.SelectedValue = objReplace.sStoreId;
                txtOilQuantity.Text = objReplace.sOilQuantity;
                txtTrReading.Text = objReplace.sTRReading;
                txtDecommDate.Text = objReplace.sDecommDate;
                txtManualRINo.Text = objReplace.sManualRINo;
                TxtCommDate.Text = objReplace.sCommDate;

                cmdReset.Enabled = false;
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
                else if (cmdSave.Text == "Save")
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
                           GenerateDecommReport();
                        }
                        else
                        {
                            GenerateDecommReport();
                        }
                    }
                    else
                    {
                        GenerateDecommReport();
                    }
                    return;
                }

                if (ValidateForm() == true)
                {
                    clsPermanentDecomm objReplace = new clsPermanentDecomm();
                    string[] Arr = new string[3];

                   // objReplace.sInvoiceId = txtInvoiceId.Text;
                    objReplace.sFailureId = txtFailureId.Text;
                    objReplace.sDecommId = txtReplaceId.Text;
                    objReplace.sTRReading = txtTrReading.Text.Replace("'", "");
                    objReplace.sRINo = txtRINo.Text.Replace("'", "");
                    objReplace.sRIDate = txtRIDate.Text.Replace("'", "");
                    objReplace.sRemarks = txtRemarks.Text.Replace("'", "").Replace("\"", "").Replace(";", "").Replace(",", "ç");
                    objReplace.sStoreId = cmbStore.SelectedValue;
                    objReplace.sCrby = objSession.UserId;
                    objReplace.sTaskType = txtType.Text;
                    objReplace.sOfficeCode = objSession.OfficeCode;
                    objReplace.sDTCCode = txtDTCCode.Text;
                    //objReplace.sRVNo = txtRvNo.Text;
                    //objReplace.sRVDate = txtRvDate.Text;
                    objReplace.sOilQuantity = txtOilQuantity.Text;
                    objReplace.sDecommDate = txtDecommDate.Text;
                    objReplace.sManualRINo = txtManualRINo.Text;
                    objReplace.sCommDate = TxtCommDate.Text;
                    objReplace.sWOSlno = getindentid();

                    if (txtActiontype.Text == "A" || txtActiontype.Text == "R")
                    {
                        if (hdfWFDataId.Value != "0")
                        {
                            ApproveRejectAction();

                            if (objSession.sTransactionLog == "1")
                            {
                                Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "PermanentDecommissioning");
                            }

                            //Session["WFOId"] = objReplace.sWFDataId;
                            GenerateDecommReport();
                            return;
                        }
                    }

                    //Workflow
                    WorkFlowObjects(objReplace);

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
                        objReplace.sDecommId = "";
                        objReplace.sActionType = txtActiontype.Text;
                        objReplace.sCrby = hdfCrBy.Value;
                        objReplace.sOfficeCode = hdfOfficeCode.Value;

                        Arr = objReplace.SaveReplaceDetails(objReplace);

                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "PermanentDecommissioning");
                        }
                        if (Arr[1].ToString() == "0")
                        {
                            hdfWFDataId.Value = Arr[2].ToString();
                            //Session["WFOId"] = objReplace.sWFDataId;
                            ApproveRejectAction();

                            GenerateDecommReport();
                            return;
                        }
                        if (Arr[1].ToString() == "2")
                        {
                            ShowMsgBox(Arr[0]);
                            return;
                        }
                    }

                    #endregion

                    Arr = objReplace.SaveReplaceDetails(objReplace);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "PermanentDecommissioning");
                    }

                    if (Arr[1].ToString() == "0")
                    {
                        //txtReplaceId.Text = Arr[0].ToString();
                        cmdSave.Text = "Update";
                        hdfWFDataId.Value = Arr[2].ToString();
                        ShowMsgBox("Decommissioning Done Successfully");
                        //Session["WFOId"] = Arr[2];
                        //GenerateDecommReport();
                        cmdSave.Enabled = false;
                        GenerateDecommReport();
                        return;
                    }
                    if (Arr[1].ToString() == "1")
                    {

                        //if (txtActiontype.Text == "A" || txtActiontype.Text == "R")
                        //{
                        //    ApproveRejectAction();
                        //    return;
                        //}
                        ShowMsgBox(Arr[0].ToString());
                        //GenerateDecommReport();
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

        public bool ValidateForm()
        {
            bool bValidate = false;
            try
            {
                if (txtRINo.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Valid RI Number");
                    return bValidate;
                }
                if (txtRIDate.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Valid RI Date");
                    return bValidate;
                }
                string sResult = Genaral.DateComparisionTransaction(txtRIDate.Text, txtFailureDate.Text, false, false);
                if (sResult == "1")
                {
                    ShowMsgBox("RI Date should be Greater than Failure Date");
                    return bValidate;
                }
                sResult = Genaral.DateComparisionTransaction(txtDecommDate.Text, txtFailureDate.Text, false, false);
                if (sResult == "1")
                {
                    ShowMsgBox("Decommissioning Date should be Greater than Failure Date");
                    return bValidate;
                }
                //if (txtRvNo.Text.Trim() == "")
                //{
                //    ShowMsgBox("Enter Valid RV Number");
                //    return bValidate;
                //}
                //if (txtRvDate.Text.Trim() == "")
                //{
                //    ShowMsgBox("Enter Valid RV Date");
                //    return bValidate;
                //}
                //if (txtTrReading.Text.Trim() == "")
                //{
                //    ShowMsgBox("Enter Valid TR Reading");
                //    return bValidate;
                //}
                //if (cmbStore.SelectedIndex == 0)
                //{
                //    ShowMsgBox("Select Store");
                //    return bValidate;
                //}

                if (txtOilQuantity.Text.Trim() == "")
                {
                    ShowMsgBox("Enter oil Quantity");
                    return bValidate;
                }
                if (txtRemarks.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Valid Remarks");
                    return bValidate;
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
                //txtRINo.Text = string.Empty;
                txtRIDate.Text = string.Empty;
                txtTrReading.Text = string.Empty;
                txtRemarks.Text = string.Empty;
                //cmbStore.SelectedIndex = 0;

                hdfFailureId.Value = string.Empty;

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
                    Response.Redirect("PermanentDecommView.aspx", false);
                }
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
                GetBasicDetails();

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
                
                    lblIDText.Text = "Estimation ID";
                    lblDateText.Text = "Estimation Date";
                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetInvoiceNo()
        {
            try
            {
                clsDeCommissioning objDecomm = new clsDeCommissioning();
                txtInvoiceId.Text = objDecomm.GetInvoiceNo(txtFailureId.Text);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetStoreId()
        {
            try
            {
                clsTcMaster objTcMaster = new clsTcMaster();
                cmbStore.SelectedValue = objTcMaster.GetStoreId(objSession.OfficeCode);
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

                objApproval.sFormName = "PermanentDecomm";
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


        public void WorkFlowObjects(clsPermanentDecomm objDecomm)
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


                objDecomm.sFormName = "PermanentDecomm";
                objDecomm.sOfficeCode = objSession.OfficeCode;
                objDecomm.sClientIP = sClientIP;
                objDecomm.sWFOId = hdfWFOId.Value;
                objDecomm.sWFAutoId = hdfWFOAutoId.Value;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GenerateRINo()
        {
            try
            {
                clsPermanentDecomm objDecomm = new clsPermanentDecomm();

                txtRINo.Text = objDecomm.GenerateRINo(objSession.OfficeCode);
                txtRINo.ReadOnly = true;
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
                    pnlApproval.Enabled = false;
                }
                if (txtActiontype.Text == "R")
                {
                    cmdSave.Text = "Reject";
                    pnlApproval.Enabled = false;
                }
                if (txtActiontype.Text == "M")
                {
                    cmdSave.Text = "Modify and Approve";
                    pnlApproval.Enabled = true;
                }

                dvComments.Style.Add("display", "block");
                //cmdReset.Enabled = false;

                if (hdfWFOAutoId.Value != "0")
                {
                    cmdSave.Text = "Save";
                    dvComments.Style.Add("display", "none");
                }

                // Check for Creator of Form
                bool bResult = CheckFormCreatorLevel();
                if (bResult == true)
                {
                    cmdSave.Text = "Save";
                    pnlApproval.Enabled = true;

                    // To handle Record From Reject 
                    if (txtActiontype.Text == "A" && hdfWFOAutoId.Value == "0")
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
                    clsPermanentDecomm objDecomm = new clsPermanentDecomm();


                    if (objApproval.sNewRecordId != "" && objApproval.sNewRecordId != null)
                    {
                        txtReplaceId.Text = objApproval.sNewRecordId;
                    }
                    if (objApproval.sApproveStatus == "1")
                    {
                        ShowMsgBox("Approved Successfully");
                        cmdSave.Enabled = false;

                        if (txtType.Text != "3")
                        {
                            if (objSession.RoleId == "1")
                            {
                                GenerateDecommReport();
                            }
                        }
                    }
                    else if (objApproval.sApproveStatus == "3")
                    {

                        ShowMsgBox("Rejected Successfully");
                        cmdSave.Enabled = false;
                    }
                    else if (objApproval.sApproveStatus == "2")
                    {
                        ShowMsgBox("Modified and Approved Successfully");
                        cmdSave.Enabled = false;

                        if (txtType.Text != "3")
                        {
                            if (objSession.RoleId == "1")
                            {
                                GenerateDecommReport();
                            }
                        }
                    }
                    if (objSession.RoleId == "1")
                    {
                        objDecomm.updateRecord(txtDTCCode.Text);
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
                        GetDecommDetailsFromXML(hdfWFDataId.Value);
                    }

                    SetControlText();
                    if (txtActiontype.Text == "V")
                    {
                        cmdSave.Text = "View";
                        dvComments.Style.Add("display", "none");
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

        public bool CheckFormCreatorLevel()
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                string sResult = objApproval.GetFormCreatorLevel("", objSession.RoleId, "PermanentDecomm");
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
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        #endregion

        public void GenerateDecommReport()
        {
            try
            {
                
                if (txtReplaceId.Text == "" || txtReplaceId.Text.Contains("-"))
                {
                    //string sWFO_ID = Session["WFOId"].ToString(); 
                    string sWFO_ID = hdfWFDataId.Value;
                    string strParam = string.Empty;
                    strParam = "id=RIReportpermanentso&wfoID=" + sWFO_ID + "&sDtrcode=" + txtTCCode.Text + "&sFailurId=" + txtFailureId.Text;
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                }
                else
                {
                    string strParam = string.Empty;
                    strParam = "id=RIReportpermanent&DecommId=" + txtReplaceId.Text;
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        #region Load From XML
        public void GetDecommDetailsFromXML(string sWFDataId)
        {
            try
            {

                clsPermanentDecomm objReplace = new clsPermanentDecomm();
                objReplace.sWFDataId = sWFDataId;

                objReplace.GetDecommDetailsFromXML(objReplace);

                //txtRINo.Text = objReplace.sRINo;
                txtRIDate.Text = objReplace.sRIDate;
                txtRemarks.Text = objReplace.sRemarks;

                cmbStore.SelectedValue = objReplace.sStoreId;
                txtOilQuantity.Text = objReplace.sOilQuantity;
                txtTrReading.Text = objReplace.sTRReading;
                txtDecommDate.Text = objReplace.sDecommDate;

                hdfOfficeCode.Value = objReplace.sOfficeCode;
                hdfCrBy.Value = objReplace.sCrby;
                txtManualRINo.Text = objReplace.sManualRINo;
                TxtCommDate.Text = objReplace.sCommDate;
                //cmdSave.Text = "Update";

                cmdReset.Enabled = false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        #endregion

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

                string strQry = string.Empty;

                strQry = "Title=Search and Select Transformer Centre Failure Details&";
                strQry += "Query=SELECT \"DF_ID\",\"DT_NAME\",\"DT_CODE\" from \"TBLDTCMAST\",\"TBLDTCFAILURE\",\"TBLWORKORDER\",\"TBLINDENT\",\"TBLDTCINVOICE\" WHERE \"DF_DTC_CODE\"= \"DT_CODE\" AND \"DF_REPLACE_FLAG\" =0 AND ";
                strQry += " \"DF_ID\"=\"WO_DF_ID\" AND \"WO_SLNO\"=\"TI_WO_SLNO\" AND \"TI_ID\"=\"IN_TI_NO\" AND  \"IN_NO\" NOT IN (SELECT \"TR_IN_NO\" FROM  \"TBLTCREPLACE\")";
                strQry += " AND \"DF_STATUS_FLAG\" =" + txtType.Text + " ";
                strQry += " AND \"DF_LOC_CODE\" LIKE '" + objSession.OfficeCode + "%' AND {0} like %{1}% order by DF_ID&";
                strQry += "DBColName=\"DF_ID\"~\"DT_NAME\"~\"DT_CODE\"&";
                strQry += "ColDisplayName=" + sTypeName + " ID~DTC_NAME~DTC_CODE&";

                strQry = strQry.Replace("'", @"\'");

                cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + hdfFailureId.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + hdfFailureId.ClientID + ")");


                txtRIDate.Attributes.Add("onblur", "return ValidateDate(" + txtRIDate.ClientID + ");");


                GetStoreId();
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
                string sTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfTCId.Value));

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
                string sDTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfDTCId.Value));

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
        //protected void cmdViewWO_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //       //string sReferId = string.Empty;
        //       clsFormValues objApproval = new clsFormValues();
        //      // string sIndentId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtFailureId.Text));
        //       string sIndentId = txtFailureId.Text;
        //        //txtType.Text = txtType.Text + "~" + hdfGuarenteeType.Value;
        //        string sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtType.Text));
        //        string sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt("V"));
        //        string sWOId = objApproval.GetWoidIdbyestimateid(sIndentId);
        //        string sReferId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWOId));

        //        string url = "/PermanentDecomm/PermanentWO.aspx?TypeValue=" + sTaskType + "&ReferID=" + sReferId + "&ActionType=" + sActionType;
        //        string s = "window.open('" + url + "', '_blank');";
        //        ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}

        //protected void cmdViewIndent_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        clsFormValues objApproval = new clsFormValues();
        //        string sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtType.Text));

        //        string sIndentId = objApproval.GetIndentIdbyestimateid(txtFailureId.Text);
              
        //         string sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt("V"));
        //         // = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sIndentId));
        //         string sWOId = objApproval.Getwoid(sIndentId);
        //         sIndentId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sIndentId));
        //         sWOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWOId));
               

        //        string url = "/PermanentDecomm/PermanentIndent.aspx?TypeValue=" + sTaskType + "&IndentId=" + sIndentId +"&ReferID=" + sWOId + "&ActionType=" + sActionType ;
        //        string s = "window.open('" + url + "', '_blank');";
        //        ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);

               
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}

        protected void lnkHistory_Click(object sender, EventArgs e)
        {
            try
            {
                string sfailureId = string.Empty;
                string sRecordId = string.Empty;
                sfailureId = txtReplaceId.Text;
                string sBOId = "14";
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