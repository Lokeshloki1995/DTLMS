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
    public partial class PermanentIndent : System.Web.UI.Page
    {
        string strFormCode = "PermanentIndent";
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
                Form.DefaultButton = cmdSave.UniqueID;
                txtIndentDate.Attributes.Add("readonly", "readonly");
                if (!IsPostBack)
                {
                    txtIndentDate_CalendarExtender1.EndDate = System.DateTime.Now;
                    Genaral.Load_Combo("SELECT \"SM_ID\",\"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"SM_STATUS\" ='A' AND \"SM_ID\"=" + clsStoreOffice.GetStoreID(objSession.OfficeCode) + " ", cmbStoreName);

                    if (Request.QueryString["TypeValue"] != null && Request.QueryString["TypeValue"].ToString() != "")
                    {
                        txtType.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TypeValue"]));
                        if (txtType.Text.Contains('~'))
                        {
                            hdfGuarenteeType.Value = txtType.Text.Split('~').GetValue(1).ToString();
                            txtType.Text = txtType.Text.Split('~').GetValue(0).ToString();
                        }
                        ChangeLabelText();

                        txtIndentDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                    }

                    if (Request.QueryString["ReferID"] != null && Request.QueryString["ReferID"].ToString() != "")
                    {
                        txtWOSlno.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ReferID"]));

                        if (Request.QueryString["IndentId"] != null && Request.QueryString["IndentId"].ToString() != "")
                        {
                            txtIndentId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["IndentId"]));
                        }
                        GetBasicDetails();
                        GenerateIndentNo();
                        if (txtIndentId.Text != "0" && txtIndentId.Text != "")
                        {
                            if (!txtIndentId.Text.Contains("-"))
                            {
                                GetIndentDetails();
                            }

                            cmdSave.Text = "View";
                        }
                        else
                        {
                            txtIndentId.Text = "";
                        }

                       

                    }

                    
                    LoadSearchWindow();

                    
                    WorkFlowConfig();

                    if (cmbStoreType.SelectedValue != "0")
                    {
                        ShowTCQuantity(cmbStoreType.SelectedValue);
                    }

                    if (objSession.RoleId == "4")
                    {
                        Session["BOID"] = "64";
                        ViewState["BOID"] = "64";
                    }
                    else
                    {
                        ViewState["BOID"] = Session["BOID"].ToString();
                    }
                   
                }
                ApprovalHistoryView.BOID = ViewState["BOID"].ToString();
                ApprovalHistoryView.sRecordId = txtIndentId.Text;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void GetIndentDetails()
        {
            try
            {

                clsPermanentIndent objIndent = new clsPermanentIndent();
                objIndent.sIndentId = txtIndentId.Text;

                objIndent.GetIndentDetails(objIndent);

                txtIndentNo.Text = objIndent.sIndentNo;
                txtIndentDesc.Text = objIndent.sIndentDescription.Replace("ç", ",");
                txtIndentDate.Text = objIndent.sIndentDate;
                cmbStoreName.SelectedValue = objIndent.sStoreName;
                //cmdSave.Text = "Update";
                hdfCron.Value = objIndent.sCrOn;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        public void GetBasicDetails()
        {
            try
            {
                clsPermanentWO objWO = new clsPermanentWO();

                objWO.sWOId = txtWOSlno.Text;

                
                    objWO.GetWOBasicDetails(objWO);
                    txtDTCName.Text = objWO.sDTCName;
                    txtTcCode.Text = objWO.sTCCode;
                   txtFailureID.Text = objWO.sFailureId;
                   // txtFailureDate.Text = objWO.sFailureDate;
                    txtDTCCode.Text = objWO.sDTCCode;
                    txtDTCId.Text = objWO.sDTCId;
                    txtTCId.Text = objWO.sTCId;
                    hdfLocCode.Value = objWO.sLocationCode;
              

                txtWONo.Text = objWO.sCommWoNo;
                txtWODate.Text = objWO.sCommDate;
                txtIssuedBy.Text = objWO.sCrBy;
                txtNewCapacity.Text = objWO.sNewCapacity;

                //ShowTCQuantity();

                txtWONo.Enabled = false;
                cmdSearch.Visible = false;
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

                if (txtIndentNo.Text.Length == 0)
                {
                    txtIndentNo.Focus();
                    ShowMsgBox("Enter Indent No");
                    return bValidate;
                }
                if (txtIndentDate.Text.Length == 0)
                {
                    txtIndentDate.Focus();
                    ShowMsgBox("Enter Indent Date");
                    return bValidate;
                }
                string sResult = Genaral.DateComparisionTransaction(txtIndentDate.Text, txtWODate.Text, false, false);
                if (sResult == "1")
                {
                    ShowMsgBox("Commisioning Indent Date should be Greater than Work Order Date");
                    return bValidate;
                }
                //if (cmbStoreType.SelectedIndex == 0)
                //{
                //    cmbStoreType.Focus();
                //    ShowMsgBox("Select Store Type");
                //    return bValidate;
                //}

             
                
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

                //View For Generate Report
                if (cmdSave.Text == "View")
                {
                    clsPermanentIndent ObjIndent = new clsPermanentIndent();
                    if (hdfApproveStatus.Value != "" && hdfApproveStatus.Value != "0")
                    {
                        if (hdfApproveStatus.Value == "1" || hdfApproveStatus.Value == "2")
                        {

                            GenerateIndentReport(ObjIndent);
                        }
                    }
                    else
                    {
                        GenerateIndentReport(ObjIndent);
                    }
                    return;
                }

                if (ValidateForm() == true)
                {
                    string[] Arr = new string[2];
                    clsPermanentIndent ObjIndent = new clsPermanentIndent();

                    ObjIndent.sIndentNo = txtIndentNo.Text;
                    ObjIndent.sIndentDate = txtIndentDate.Text;
                    ObjIndent.sIndentDescription = txtIndentDesc.Text.Replace("'", "").Replace("\"", "").Replace(";", "").Replace(",", "ç");
                    ObjIndent.sStoreName = cmbStoreName.SelectedValue;
                    ObjIndent.sIndentId = txtIndentId.Text;
                    ObjIndent.sWOSlno = txtWOSlno.Text;
                    ObjIndent.sCrBy = objSession.UserId;
                    ObjIndent.sWoNo = txtWONo.Text.Trim();

                    ObjIndent.sDTCCode = txtDTCCode.Text;
                    ObjIndent.sDTCName = txtDTCName.Text;
                    ObjIndent.sFailureId = txtFailureID.Text;

                    //Commented by sandeep on 08/04/2022 bcz of data not saving in SaveWorkflowObjects
                    //ObjIndent.sTasktype = txtType.Text;
                    ObjIndent.sStoreType = cmbStoreType.SelectedValue;

                    if (hdfAvailQuantity.Value == "0")
                    {
                        ObjIndent.sAlertFlg = "1";
                    }
                    else
                    {
                        ObjIndent.sAlertFlg = "0";
                    }

                    if (txtActiontype.Text == "A" || txtActiontype.Text == "R" || txtActiontype.Text == "D")
                    {
                        if (hdfWFDataId.Value != "0")
                        {

                            ApproveRejectAction();

                            if (objSession.sTransactionLog == "1")
                            {
                                Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "(Indent)PermanentDecommissioning");
                            }

                            GenerateIndentReport(ObjIndent);
                            return;
                        }

                    }



                    //Workflow
                    WorkFlowObjects(ObjIndent);

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
                        ObjIndent.sIndentId = "";
                        ObjIndent.sActionType = txtActiontype.Text;
                        ObjIndent.sCrBy = hdfCrBy.Value;

                        Arr = ObjIndent.SaveUpdateIndentDetails(ObjIndent);
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "(Indent)PermanentDecommissioning");
                        }
                        if (Arr[1].ToString() == "0")
                        {
                            hdfWFDataId.Value = ObjIndent.sWFDataId;
                            ApproveRejectAction();
                            GenerateIndentReport(ObjIndent);
                            return;
                        }
                        if (Arr[1].ToString() == "2")
                        {
                            ShowMsgBox(Arr[0]);
                            return;
                        }
                    }

                    #endregion

                    Arr = ObjIndent.SaveUpdateIndentDetails(ObjIndent);
                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "(Indent)PermanentDecommissioning");
                    }

                    if (Arr[1].ToString() == "0")
                    {
                        ShowMsgBox(Arr[0]);
                        txtIndentId.Text = ObjIndent.sIndentId;
                        cmdSave.Text = "Update";
                        GenerateIndentReport(ObjIndent);
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
                }

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
                   // Response.Redirect("IndentView.aspx", false);
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
                clsPermanentIndent objIndent = new clsPermanentIndent();
                if (objIndent.ValidateUpdate(txtIndentId.Text) == true)
                {

                    //cmdSave.Enabled = false;
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

        public void ChangeLabelText()
        {
            try
            {
                    lblIDText.Text = "Failure ID";
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
                txtWOSlno.Text = hdfWOslno.Value;
                GetBasicDetails();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void ShowTCQuantity(string sStoreType)
        {
            try
            {
                clsPermanentIndent objIndent = new clsPermanentIndent();
                hdfAvailQuantity.Value = objIndent.GetTransformerCount(objSession.OfficeCode, txtWOSlno.Text, sStoreType, objSession.sRoleType);
                if (sStoreType != "0")
                {
                    lnkQuantityMsg.Visible = true;
                }
                else
                {
                    lnkQuantityMsg.Visible = false;
                }

                if (sStoreType == "1")
                {
                    chkAlert.Text = "Alert me once stock available in Store";
                    lnkQuantityMsg.Text = hdfAvailQuantity.Value + " Number of  " + txtNewCapacity.Text + " KVA Capacity Transformers Available in the Store";
                }
                else
                {
                    chkAlert.Text = "Alert me once stock available in Bank";
                    lnkQuantityMsg.Text = hdfAvailQuantity.Value + " Number of  " + txtNewCapacity.Text + " KVA Capacity Transformers Available in the Bank";
                }

                spanQuant.Visible = true;
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
                if (objSession.sRoleType == "1")
                {
                    cmbStoreName.SelectedValue = objTcMaster.GetStoreId(objSession.OfficeCode);
                }
                else
                {
                    cmbStoreName.SelectedValue = objSession.OfficeCode;
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

                objApproval.sFormName = "PermanentIndent";
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

        public void WorkFlowObjects(clsPermanentIndent objIndent)
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


                objIndent.sFormName = "PermanentIndent";
                objIndent.sOfficeCode = objSession.OfficeCode;
                objIndent.sClientIP = sClientIP;
                objIndent.sWFOId = hdfWFOId.Value;
                objIndent.sWFAutoId = hdfWFOAutoId.Value;

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
                clsPermanentIndent objIndent = new clsPermanentIndent();

                txtIndentNo.Text = objIndent.GenerateIndentNo(hdfLocCode.Value);
                txtIndentNo.ReadOnly = true;
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
                    clsPermanentIndent ObjIndent = new clsPermanentIndent();
                    if (objApproval.sApproveStatus == "1")
                    {
                        ShowMsgBox("Approved Successfully");
                        cmdSave.Enabled = false;
                        if (txtType.Text != "3")
                        {
                            if (objApproval.sNewRecordId != null)
                            {
                                txtIndentId.Text = objApproval.sNewRecordId;
                               // GenerateIndentReport(ObjIndent);

                            }
                        }
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
                        if (txtType.Text != "3")
                        {
                            if (objSession.RoleId == "1")
                            {
                                txtIndentId.Text = objApproval.sNewRecordId;
                                GenerateIndentReport(ObjIndent);

                            }
                        }
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
                        GetIndentDetailsFromXML(hdfWFDataId.Value);
                    }

                    SetControlText();
                    ControlEnableDisable();
                    if (txtActiontype.Text == "V")
                    {
                        cmdSave.Text = "View";
                        dvComments.Style.Add("display", "none");
                    }

                    if (txtActiontype.Text != "V")
                    {
                        CheckIndentCreation3DaysExceeds();
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
                string sResult = objApproval.GetFormCreatorLevel("", objSession.RoleId, "PermanentIndent");
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

        public void GenerateIndentReport(clsPermanentIndent objIndent)
        {
            try
            {
                string sWFDataId = objIndent.sWFDataId;
                if (txtIndentId.Text.Contains("-"))
                {
                    sWFDataId = hdfWFDataId.Value;
                    string strParam = string.Empty;
                    strParam = "id=IndentReportPermanent&IndentId=" + sWFDataId + "&WFDataId=" + sWFDataId + "&OffCode=" + objSession.OfficeCode;
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                    return;
                }

                if (txtType.Text == "1" ||txtType.Text=="2"|| txtType.Text == "4")
                {
                    string strParam = string.Empty;
                    strParam = "id=IndentReportPermanent&IndentId=" + txtIndentId.Text + "&WFDataId=" + sWFDataId + "&OffCode=" + objSession.OfficeCode;
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
                txtWONo.Enabled = false;
                cmdSearch.Visible = false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        #region Load From XML
        public void GetIndentDetailsFromXML(string sWFDataId)
        {
            try
            {
             
                clsPermanentIndent objIndent = new clsPermanentIndent();
                objIndent.sWFDataId = sWFDataId;

                objIndent.GetIndentDetailsFromXML(objIndent);

                //txtIndentNo.Text = objIndent.sIndentNo;
                txtIndentDesc.Text = objIndent.sIndentDescription.Replace("ç", ",");
                txtIndentDate.Text = objIndent.sIndentDate;
                cmbStoreName.SelectedValue = objIndent.sStoreName;
                hdfCron.Value = objIndent.sCrOn;
                hdfCrBy.Value = objIndent.sCrBy;
                cmbStoreType.SelectedValue = objIndent.sStoreType;
                //cmdSave.Text = "Update";

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
                string strQry = string.Empty;
                if (txtType.Text != "3")
                {

                    strQry = "Title=Search and Select Work Order Details&";
                    strQry += "Query= SELECT \"WO_SLNO\",\"DT_NAME\",\"WO_NO\",\"DF_DTC_CODE\" FROM \"TBLDTCMAST\",\"TBLDTCFAILURE\",\"TBLWORKORDER\" WHERE  \"DT_CODE\" = \"DF_DTC_CODE\" AND \"DF_REPLACE_FLAG\" = 0 ";
                    strQry += " AND \"WO_SLNO\" NOT IN (SELECT \"TI_WO_SLNO\" FROM \"TBLINDENT\") AND \"DF_ID\" = \"WO_DF_ID\" AND  \"DF_STATUS_FLAG\" =" + txtType.Text + "  ";
                    strQry += " AND \"DF_LOC_CODE\" LIKE '" + objSession.OfficeCode + "%' AND {0} like %{1}% &";
                    strQry += "DBColName=\"WO_NO\"~\"DT_NAME\"~\"DF_DTC_CODE\"&";
                    strQry += "ColDisplayName=WO NO~DTC_NAME~DTC_CODE&";
                }
                else
                {
                    strQry = "Title=Search and Select Work Order Details&";
                    strQry += "Query= SELECT \"WO_SLNO\",\"WO_NO\",TO_CHAR(\"WO_DATE\",'DD-MON-YYYY') WO_DATE FROM TBLWORKORDER ";
                    strQry += " WHERE \"WO_REPLACE_FLG\"='0'  AND \"WO_DF_ID\" IS NULL AND \"WO_OFF_CODE\" LIKE '" + objSession.OfficeCode + "%'";
                    strQry += " AND  \"WO_SLNO\" NOT IN (SELECT \"TI_WO_SLNO\" FROM \"TBLINDENT\") AND {0} like %{1}% &";
                    strQry += "DBColName=\"WO_NO\"&";
                    strQry += "ColDisplayName=WO NO&";
                }

                strQry = strQry.Replace("'", @"\'");
                cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + hdfWOslno.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + hdfWOslno.ClientID + ")");

                txtIndentDate.Attributes.Add("onblur", "return ValidateDate(" + txtIndentDate.ClientID + ");");

                GetStoreId();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void CheckIndentCreation3DaysExceeds()
        {
            try
            {
                clsPermanentIndent objIndent = new clsPermanentIndent();

                bool bResult = objIndent.CheckIndentCreation3DaysExceeds(hdfCron.Value);
                if (bResult == true)
                {
                    ShowMsgBox("Indent has been Expired");
                    cmdSave.Text = "Abort";
                    txtActiontype.Text = "D";
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdViewWO_Click(object sender, EventArgs e)
        {
            try
            {
                string sReferId = string.Empty;
              
                sReferId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtWOSlno.Text));

                txtType.Text = txtType.Text + "~" + hdfGuarenteeType.Value;
                string sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtType.Text));
                string sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt("V"));

                string url = "/PermanentDecomm/PermanentWO.aspx?TypeValue=" + sTaskType + "&ReferID=" + sReferId + "&ActionType=" + sActionType;
                string s = "window.open('" + url + "', '_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetStore_TcDetails()
        {

        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(objSession.OfficeCode));
                string WOSLNO = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtWOSlno.Text));
                string url = "/DashboardForm/FailurePendingOverview.aspx?value=InvoiceTCDetails&OfficeCode=" + sOfficeCode + "&WOSLNO=" + WOSLNO;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);

                //clsIndent objIndent = new clsIndent();
                //DataTable dtTcDeatils = objIndent.GetStore_TcDetails(objSession.OfficeCode, txtWOSlno.Text);
                //HyperQuantityMsg.Text = hdfAvailQuantity.Value + " Number of  " + txtNewCapacity.Text + " KVA Capacity Transformers Available in the Store";
                //spanQuant.Visible = true;
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
                sfailureId = txtIndentId.Text;
                string sBOId = "12";
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

        protected void cmbStoreType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ShowTCQuantity(cmbStoreType.SelectedValue);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}