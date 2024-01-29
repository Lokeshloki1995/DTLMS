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
using System.Text.RegularExpressions;
using System.IO;
using System.Configuration;
using System.Net;

namespace IIITS.DTLMS.DTCFailure
{
    public partial class InvoiceCreation : System.Web.UI.Page
    {

        string strFormCode = "InvoiceCreation";
        clsSession objSession;
        string sUserName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_USER"]);
        string sPassword = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_PASS"]);
        string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
        string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);
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

                    if (objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                    {
                        // objSession.sStoreID = objSession.OfficeCode;
                        // //        objSession.sStoreID = objSession.OfficeCode.Substring(0, 2);

                        objSession.sStoreID = clsStoreOffice.GetStoreID(objSession.OfficeCode);
                    }
                    else
                    {
                        // objSession.sStoreID = objSession.OfficeCode;
                    }

                    if (!IsPostBack)
                    {
                        Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='SR' ORDER BY \"MD_ORDER_BY\" ", "-Select-", cmbRating);
                        txtInvoiceDate_CalendarExtender1.EndDate = System.DateTime.Now;

                        //if (objSession.RoleId == "12")
                        //{
                        //    txtActiontype.Text = "M";
                        //}
                        if (Request.QueryString["TypeValue"] != null && Convert.ToString(Request.QueryString["TypeValue"]) != "")
                        {
                            txtType.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TypeValue"]));
                            ChangeLabelText();
                            hdfGuarenteeType.Value = txtType.Text;
                            hdfType.Value = txtType.Text;
                            txtInvoiceDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                        }

                        if (Request.QueryString["ReferID"] != null && Convert.ToString(Request.QueryString["ReferID"]) != "")
                        {
                            txtIndentId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ReferID"]));
                            if (Request.QueryString["OfficeCode"] != null && Convert.ToString(Request.QueryString["OfficeCode"]) != "")
                            {
                                txtssOfficeCode.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["OfficeCode"]));
                            }
                            if (Request.QueryString["InvoiceId"] != null && Convert.ToString(Request.QueryString["InvoiceId"]) != "")
                            {
                                txtInvoiceSlNo.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["InvoiceId"]));
                            }

                            GetBasicDetails();
                            BindgridView(SFTPmainfolder, sUserName, sPassword);

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

                            //Check Decommission Done to Update Invoice Details If Decommssion Done Restrict to Update
                            //ValidateFormUpdate();




                        }
                        if (objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                        {

                            objSession.sStoreID = clsStoreOffice.GetStoreID(txtssOfficeCode.Text.Substring(0, 3));
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
                        //  ViewState["BOID"] = Session["BOID"].ToString();                   
                    }
                    ApprovalHistoryView.BOID = Convert.ToString(ViewState["BOID"]);
                    ApprovalHistoryView.sRecordId = txtInvoiceSlNo.Text;
                    cmdViewWO.Visible = true;



                    if (txtActiontype.Text == "M")
                    {
                        clsApproval objLevel = new clsApproval();
                        string sLevel = objLevel.sGetApprovalLevel(Convert.ToString(ViewState["BOID"]), objSession.RoleId);

                        if (sLevel != "" && sLevel != null)
                        {
                            if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                            {
                                cmdSave.Text = " Modify and Submit";
                            }
                            else if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                            {
                                cmdSave.Text = " Modify and Approve";
                            }
                        }
                    }
                    else if (txtActiontype.Text == "A")
                    {
                        clsApproval objLevel = new clsApproval();
                        string sLevel = objLevel.sGetApprovalLevel(Convert.ToString(ViewState["BOID"]), objSession.RoleId);
                        if (sLevel != "" && sLevel != null)
                        {
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
                        objInvoice.sOfficeCode = objSession.OfficeCode;
                    }
                    else
                    {
                        if (objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                        {
                            objInvoice.sOfficeCode = txtssOfficeCode.Text;
                        }
                        else
                        {
                            objInvoice.sOfficeCode = Convert.ToString(ViewState["LOCCODE"]);
                        }
                        //objInvoice.sOfficeCode = Convert.ToString(ViewState["LOCCODE"]);
                    }

                    objInvoice.sDTCName = txtDTCName.Text;
                    objInvoice.sDTCCode = txtDTCCode1.Text;
                    objInvoice.sOldTcCode = txtOldTcCode.Text;
                    objInvoice.sTcNewCapacity = txtNewtccapacity.Text;

                    objInvoice.sTaskType = txtType.Text;


                    //Workflow
                    WorkFlowObjects(objInvoice);

                    Arr = objInvoice.SaveUpdateInvoiceDetails(objInvoice);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (Invoice) Failure/Enhancement ");
                    }

                    if (Arr[1].ToString() == "0")
                    {
                        ShowMsgBox(Arr[0]);
                        txtInvoiceSlNo.Text = objInvoice.sInvoiceSlNo;
                        if (txtType.Text != "3")
                        {
                            GenerateInvoiceReport();
                        }
                        else
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
                // lblMessage.Text = clsException.ErrorMsg();
                ShowMsgBox("Something went wrong while saving, Please approve once again.");
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
                string sResult = Genaral.DateComparisionTransaction(txtInvoiceDate.Text, txtIndentdate.Text, false, false);
                if (sResult == "1")
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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
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
                    //objInvoice.sOfficeCode = Convert.ToString(ViewState["LOCCODE"]);

                    objInvoice.sOfficeCode = objSession.OfficeCode;


                    if (objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                    {

                        objInvoice.sOfficeCode = clsStoreOffice.GetStoreID(txtssOfficeCode.Text.Substring(0, 3));
                    }

                }

                objInvoice.sTcCapacity = txtNewtccapacity.Text;
                objInvoice.sStoreType = "1";
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
                    // cmbRating.SelectedValue = objInvoice.sTcRating;
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

                txtGpId.Text = objInvoice.sGatePassId;
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

                objApproval.sFormName = "InvoiceCreation";
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


                objInvoice.sFormName = "InvoiceCreation";
                if (objSession.sRoleType == "1")
                {
                    objInvoice.sOfficeCode = objSession.OfficeCode;
                }
                else
                {
                    if (objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                    {
                        objInvoice.sOfficeCode = txtssOfficeCode.Text;
                    }
                    else
                    {
                        objInvoice.sOfficeCode = Convert.ToString(ViewState["LOCCODE"]);
                    }
                    // objInvoice.sOfficeCode = Convert.ToString(ViewState["LOCCODE"]);
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

                //if (objSession.RoleId == "12" && txtActiontype.Text == "A")
                //{
                //    txtActiontype.Text = "M";
                //}

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
                if (objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
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
                string sResult = objApproval.GetFormCreatorLevel("", objSession.RoleId, "InvoiceCreation");
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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                bValidate = false;
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

                    if (Arr[0].ToString() == "0")
                    {
                        txtGpId.Text = objInvoice.sGatePassId;
                        string strParam = "id=GatePass&InvoiceId=" + txtInvoiceNo.Text;
                        RegisterStartupScript("PrintD", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

                        return;
                    }
                    if (Arr[0].ToString() == "1")
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
                    if (objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SupAdminRole"]))
                    {
                        string OfficeCode = txtssOfficeCode.Text;
                        if (txtActiontype.Text != "V")
                        {
                            txtInvoiceNo.Text = objInvoice.GenerateInvoiceNoSA(OfficeCode, objSession.sRoleType, objSession.RoleId);
                        }
                    }
                    else
                    {
                        if (txtActiontype.Text != "V")
                        {
                            //  txtInvoiceNo.Text = objInvoice.GenerateInvoiceNo(objSession.OfficeCode, objSession.sRoleType);
                            txtInvoiceNo.Text = objInvoice.GenerateInvoiceNodtcinvoice(objSession.OfficeCode, objSession.sRoleType);

                        }
                    }
                }
                else
                {
                    if (objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SupAdminRole"]))
                    {
                        string OfficeCode = txtssOfficeCode.Text;
                        if (txtActiontype.Text != "V")
                        {
                            txtInvoiceNo.Text = objInvoice.GenerateInvoiceNoSA(OfficeCode, objSession.sRoleType, objSession.RoleId);
                        }
                    }
                    else
                    {
                        if (txtActiontype.Text != "V")
                        {
                            // txtInvoiceNo.Text = objInvoice.GenerateInvoiceNo(Convert.ToString(ViewState["LOCCODE"]), objSession.sRoleType);
                            txtInvoiceNo.Text = objInvoice.GenerateInvoiceNodtcinvoice(Convert.ToString(ViewState["LOCCODE"]), objSession.sRoleType);

                        }
                    }
                    //txtInvoiceNo.Text = objInvoice.GenerateInvoiceNo(Convert.ToString(ViewState["LOCCODE"]), objSession.sRoleType);
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
                    strParam = "id=InvoiceReport&InvoiceId=" + txtInvoiceSlNo.Text + "&OfficeCode=" + objSession.OfficeCode + "&Capacity=" + txtTcCapacity.Text;
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                }
                else if (txtType.Text == "2")
                {
                    string strParam = string.Empty;
                    strParam = "id=EnhanceInvoiceReport&InvoiceId=" + txtInvoiceSlNo.Text + "&OfficeCode=" + objSession.OfficeCode + "&Capacity=" + txtTcCapacity.Text;
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                }
                else
                {

                    string strParam = string.Empty;
                    strParam = "id=NewDtcInvoiceReport&InvoiceId=" + txtInvoiceSlNo.Text + "&OfficeCode=" + objSession.OfficeCode + "&Capacity=" + txtTcCapacity.Text;
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                    return;
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
                strQry += " FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\" WHERE  CAST(\"TC_LOCATION_ID\" AS TEXT) = '" + objSession.sStoreID + "' AND \"TC_CURRENT_LOCATION\"='1' AND \"TC_STATUS\" IN (1,2)";
                strQry += " AND \"TC_MAKE_ID\"= \"TM_ID\" and \"TC_STATUS\" in (1,2) AND \"TC_CURRENT_LOCATION\" =1 AND CAST(\"TC_CAPACITY\" AS INT) ='" + txtNewtccapacity.Text + "' AND {0} like %{1}% &";
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
                    strQry += " AND CAST(\"DF_LOC_CODE\" AS TEXT) LIKE '" + objSession.OfficeCode + "%' AND  {0} like %{1}% &";
                    strQry += "DBColName=\"DF_ID\"~\"DT_NAME\"~\"DT_CODE\"~\"TI_INDENT_NO\"&";
                    strQry += "ColDisplayName=" + sTypeName + " ID~Transformer Centre Name~Transformer Centre Code~Indent NO&";
                }
                else
                {
                    strQry = "Title=Search and Select Indent Details&";
                    strQry += "Query=SELECT \"TI_ID\",\"WO_NO\",\"TI_INDENT_NO\" ";
                    strQry += " FROM \"TBLWORKORDER\",\"TBLINDENT\" WHERE \"WO_SLNO\"=\"TI_WO_SLNO\" AND \"WO_REPLACE_FLG\"='0' AND \"WO_DF_ID\" IS NULL ";
                    strQry += " AND CAST(\"WO_OFF_CODE\" AS TEXT) LIKE '" + objSession.OfficeCode + "%' AND \"TI_ID\" NOT IN (SELECT \"IN_TI_NO\" FROM \"TBLDTCINVOICE\") AND {0} like %{1}% &";
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

        public DataTable BindgridView(string FtpServer, string username, string password)
        {
            DataTable dtFiles = new DataTable();
            try
            {
                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SFTPmainfolder"]);

                string failureid = Regex.Replace(txtFailureId.Text, @"[^0-9a-zA-Z]+", "");
                FtpServer += "FAILUREENTRY/" + failureid;
                clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);
                //path for get files from ftp
                bool IsExists = objFtp.FtpDirectoryExists(FtpServer);
                // checking related ponumber directory is there are not!


                if (IsExists == false)
                {
                    gvFiles.Visible = false;
                    return dtFiles;
                }
                else
                {
                    dtFiles = objFtp.GetListOfFiles(FtpServer);
                }
                gvFiles.DataSource = dtFiles;
                gvFiles.DataBind();

                return dtFiles;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtFiles;
            }
        }

        protected void DownloadFiledwnld(object sender, EventArgs e)
        {


            bool endRequest = false;
            string fileName1 = (sender as LinkButton).CommandArgument;
            try
            {


                //Create a stream for the file
                Stream stream = null;

                //This controls how many bytes to read at a time and send to the client
                int bytesToRead = 10000;

                // Buffer to read bytes in chunk size specified above
                byte[] buffer = new Byte[bytesToRead];

                // The number of bytes read
                try
                {
                    string SFTPmainfolder = Convert.ToString(ConfigurationManager.AppSettings["VirtualDirectoryDocs"]);

                    string failureid = Regex.Replace(txtFailureId.Text, @"[^0-9a-zA-Z]+", "");
                    clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);

                    string url = SFTPmainfolder + "FAILUREENTRY/" + failureid + "/" + fileName1;
                    string fileName = getFilename(url);
                    //Create a WebRequest to get the file
                    HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(url);

                    //Create a response for this request
                    HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();

                    if (fileReq.ContentLength > 0)
                        fileResp.ContentLength = fileReq.ContentLength;

                    //Get the Stream returned from the response
                    stream = fileResp.GetResponseStream();

                    // prepare the response to the client. resp is the client Response
                    var resp = HttpContext.Current.Response;

                    //Indicate the type of data being sent
                    resp.ContentType = "application/octet-stream";

                    //Name the file 
                    resp.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
                    resp.AddHeader("Content-Length", fileResp.ContentLength.ToString());

                    int length;
                    do
                    {
                        // Verify that the client is connected.
                        if (resp.IsClientConnected)
                        {
                            // Read data into the buffer.
                            length = stream.Read(buffer, 0, bytesToRead);

                            // and write it out to the response's output stream
                            resp.OutputStream.Write(buffer, 0, length);

                            // Flush the data
                            resp.Flush();

                            //Clear the buffer
                            buffer = new Byte[bytesToRead];
                        }
                        else
                        {
                            // cancel the download if client has disconnected
                            length = -1;
                        }
                    } while (length > 0); //Repeat until no data is read
                }
                finally
                {
                    if (stream != null)
                    {
                        //Close the input stream
                        stream.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("(404) Not Found"))
                {
                    ShowMsgBox("File Not Found");
                }
                else
                {
                    lblMessage.Text = clsException.ErrorMsg();
                    clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                }

            }

        }

        private string getFilename(string hreflink)
        {
            Uri uri = new Uri(hreflink);

            string filename = System.IO.Path.GetFileName(uri.LocalPath);

            return filename;
        }
        protected void DownloadFile(object sender, EventArgs e)
        {

            string fileName = (sender as LinkButton).CommandArgument;

            try
            {
                bool status = false;
                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectoryDocs"]);

                string failureid = Regex.Replace(txtFailureId.Text, @"[^0-9a-zA-Z]+", "");
                clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);
                //status = objFtp.Download(sFileServerPath + "/PO_DOCS/" + PoNo + "/" + fileName, fileName);

                string path = SFTPmainfolder + "FAILUREENTRY/" + failureid + "/" + fileName;
                RegisterStartupScript("Print", "<script>window.open('" + path + "','_blank')</script>");

            }
            catch (WebException ex)
            {
                //  throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }

        }

        protected void gvFiles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvFiles.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["dt"];
                gvFiles.DataSource = SortDataTable(dt as DataTable, true);
                gvFiles.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "ASC"; }
            set { ViewState["SortDirection"] = value; }


        }
        protected void gvFiles_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = gvFiles.PageIndex;
            DataTable dt = (DataTable)ViewState["dt"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                gvFiles.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                gvFiles.DataSource = dt;
            }
            gvFiles.DataBind();
            gvFiles.PageIndex = pageIndex;
        }
        private string GridViewSortExpression
        {
            get { return ViewState["SortExpression"] as string ?? string.Empty; }
            set { ViewState["SortExpression"] = value; }
        }
        private string GetSortDirection()
        {
            switch (GridViewSortDirection)
            {
                case "ASC":
                    GridViewSortDirection = "DESC";

                    break;
                case "DESC":
                    GridViewSortDirection = "ASC";

                    break;
            }


            return GridViewSortDirection;
        }

        protected DataView SortDataTable(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1} ", GridViewSortExpression, GridViewSortDirection);

                        if (Convert.ToString(dataView.Sort) == "Name ASC ")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("Name")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["dt"] = dataView.ToTable();
                        }
                        else if (Convert.ToString(dataView.Sort) == "Name DESC ")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("Name")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["dt"] = dataView.ToTable();
                        }

                        else
                        {
                            // dataView.Sort = string.Format("{0} {1} ", GridViewSortExpression, GridViewSortDirection);
                            ViewState["dt"] = dataView.ToTable();
                        }
                    }
                    else
                    {

                        dataView.Sort = string.Format("{0} {1} ", GridViewSortExpression, GetSortDirection());

                        if (Convert.ToString(dataView.Sort) == "Name ASC ")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("Name")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["dt"] = dataView.ToTable();
                        }
                        else if (Convert.ToString(dataView.Sort) == "Name DESC ")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("Name")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["dt"] = dataView.ToTable();
                        }



                        else
                        {
                            // dataView.Sort = string.Format("{0} {1} ", GridViewSortExpression, GetSortDirection());
                            ViewState["dt"] = dataView.ToTable(); ;


                        }

                    }
                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }

        protected void cmdViewWO_Click(object sender, EventArgs e)
        {
            try
            {
                string sReferId = string.Empty;
                //if (txtType.Text == "3")
                //{
                //    sReferId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtWOSlno.Text));
                //}
                //else
                //{
                //    sReferId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtFailureID.Text));
                //}
                //sReferId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtWOSlno.Text));
                sReferId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtIndentId.Text));

                txtType.Text = txtType.Text + "~" + hdfGuarenteeType.Value;
                string sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtType.Text));
                string sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt("V"));

                string url = "/DTCFailure/WorkOrder.aspx?TypeValue=" + sTaskType + "&ReferID=" + sReferId + "&ActionType=" + sActionType + "&FailType=" + sTaskType;
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