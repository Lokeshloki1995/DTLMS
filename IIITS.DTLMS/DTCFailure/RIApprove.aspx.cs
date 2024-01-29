using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using IIITS.PGSQL.DAL;
using System.Data;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Configuration;

namespace IIITS.DTLMS.DTCFailure
{
    public partial class RIApprove : System.Web.UI.Page
    {

        string strFormCode = "RIApprove";
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
                    Form.DefaultButton = cmdApprove.UniqueID;
                    objSession = (clsSession)Session["clsSession"];
                    lblMessage.Text = string.Empty;
                    //txtAckDate.Attributes.Add("readonly", "readonly");
                    //txtNoOfBarrels.Enabled = false;

                    txtAcknoledgeDate.Attributes.Add("readonly", "readonly");
                    txtAckNo.Attributes.Add("readonly", "readonly");

                    if (!IsPostBack)
                    {
                        // CalendarExtender2.EndDate = System.DateTime.Now;
                        CalendarExtender1.EndDate = System.DateTime.Now;
                        if (Request.QueryString["OfficeCode"] != null && Convert.ToString(Request.QueryString["OfficeCode"]) != "")
                        {
                            txtssOfficeCode.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["OfficeCode"]));
                        }
                        if (Request.QueryString["DecommId"] != null && Convert.ToString(Request.QueryString["DecommId"]) != "")
                        {
                            txtDecommId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["DecommId"]));
                            txtFailureId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["FailureId"]));
                            txtType.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TypeValue"]));


                            GetTransformerDetails();
                            BindgridView(SFTPmainfolder, sUserName, sPassword);

                        }
                        DateTime decommDateTime = DateTime.Now;
                        decommDateTime = Convert.ToDateTime(txtDecommDate.Text);
                        CalendarExtender1.StartDate = decommDateTime;

                        //WorkFlow / Approval
                        WorkFlowConfig();
                        if (objSession.RoleId == "2")
                        {
                            Session["BOID"] = "15";
                            ViewState["BOID"] = "15";
                        }
                        else
                        {
                            ViewState["BOID"] = Convert.ToString(Session["BOID"]);
                        }
                        // ViewState["BOID"] = Session["BOID"].ToString();
                    }
                    ApprovalHistoryView.BOID = Convert.ToString(ViewState["BOID"]);
                    ApprovalHistoryView.sRecordId = txtDecommId.Text;
                    cmdViewWO.Visible = true;
                    //if (objSession.RoleId == "12")
                    //{
                    //    txtActiontype.Text = "M";
                    //}
                    if (txtActiontype.Text == "M")
                    {
                        clsApproval objLevel = new clsApproval();
                        string sLevel = objLevel.sGetApprovalLevel(Convert.ToString(ViewState["BOID"]), objSession.RoleId);
                        if (sLevel != "" && sLevel != null)
                        {
                            if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                            {
                                cmdApprove.Text = " Modify and Submit";

                            }
                            else if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                            {
                                cmdApprove.Text = " Modify and Approve";

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
                                cmdApprove.Text = "Submit";

                            }
                            else if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                            {
                                cmdApprove.Text = "Approve";

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
        protected void cmdApprove_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                else
                {
                    clsRIApproval objRIApproval = new clsRIApproval();
                    if (ValidateForm() == true)
                    {
                        string[] Arr = new string[3];

                        objRIApproval.sDecommId = txtDecommId.Text;
                        objRIApproval.sRemarks = txtCommentFromStoreKeeper.Text.Replace("'", "").Replace("\"", "").Replace(";", "").Replace(",", "ç");
                        objRIApproval.sFailureId = txtFailureId.Text;
                        objRIApproval.sTasktype = txtType.Text;
                        objRIApproval.sOilQuantitySK = txtOilQtySK.Text;
                        //      objRIApproval.sBarrels = txtNoOfBarrels.Text;

                        objRIApproval.sTCCode = txtDtrCode.Text;
                        objRIApproval.sCrby = objSession.UserId;
                        objRIApproval.sRVNo = txtAckNo.Text;
                        objRIApproval.sRVDate = txtAcknoledgeDate.Text;
                        //objRIApproval.sOilQuantity = txtOilQuantity.Text;
                        objRIApproval.sOilQntyTank = oiltankcapacity.Text;
                        objRIApproval.sOilQnty = txtQuantityOfOil.Text;
                        
                        if (objSession.sRoleType == "1")
                        {
                            objRIApproval.sOfficeCode = objSession.OfficeCode;
                        }
                        else
                        {
                            if (objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                            {
                                objRIApproval.sOfficeCode = txtssOfficeCode.Text;
                            }
                            else
                            {
                                objRIApproval.sOfficeCode = Convert.ToString(ViewState["LOCCODE"]);
                            }
                        }
                        objRIApproval.sDTCCode = hdfDTCCode.Value;
                        objRIApproval.sManualRVACKNo = txtManualAckNo.Text;
                        objRIApproval.sDecomWorkOrder = txtDeCommWO.Text;

                        if (cmdApprove.Text == "View")
                        {
                            if (hdfApproveStatus.Value != "")
                            {
                                if (hdfApproveStatus.Value == "1" || hdfApproveStatus.Value == "2")
                                {
                                    GenerateRIAckReport();
                                }

                                if (hdfApproveStatus.Value == "0")
                                {
                                    GenerateRIAckReport();
                                }

                            }
                            else
                            {
                                GenerateRIAckReport();
                            }
                            return;
                        }

                        //Workflow
                        WorkFlowObjects(objRIApproval);

                        #region Modify and Approve

                        // For Modify and Approve
                        if (txtActiontype.Text == "M")
                        {
                            if (hdfRejectApproveRef.Value != "RA")
                            {
                                //if (txtComment.Text.Trim() == "")
                                //{
                                //    ShowMsgBox("Enter Comments/Remarks");
                                //    txtComment.Focus();
                                //    return;

                                //}

                                if (objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                                {
                                    if (txtComment.Text.Trim() == "")
                                    {
                                        txtComment.Text = "APPROVED.";
                                    }
                                }
                                else
                                {
                                    if (txtComment.Text.Trim() == "")
                                    {
                                        ShowMsgBox("Enter Comments/Remarks");
                                        txtComment.Focus();
                                        return;

                                    }
                                }
                            }

                            objRIApproval.sActionType = txtActiontype.Text;
                            objRIApproval.sCrby = hdfCrBy.Value;
                            Arr = objRIApproval.UpdateReplaceDetails(objRIApproval);

                            if (objSession.sTransactionLog == "1")
                            {
                                Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (RI) Failure ");
                            }

                            if (Arr[1].ToString() == "1")
                            {
                                hdfWFDataId.Value = objRIApproval.sWFDataId;
                                //Session["WFOId"] = objRIApproval.sWFDataId;
                                ApproveRejectAction();

                                if (objSession.sTransactionLog == "1")
                                {
                                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (RI) Failure ");
                                }

                                GenerateRIAckReport();
                                return;
                            }
                            if (Arr[1].ToString() == "2")
                            {
                                ShowMsgBox(Arr[0]);
                                return;
                            }
                        }

                        #endregion

                        if (objSession.RoleId == "2" || objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                        {
                            if (objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                            {
                                // objRIApproval.checkapprovallevel(objRIApproval);
                                string val;
                                val = objRIApproval.checkapprovallevel(hdfWFOId.Value);
                                if (val == "0")
                                {

                                    Arr = objRIApproval.UpdateReplaceDetails(objRIApproval);
                                    cmdApprove.Enabled = false;

                                    if (Arr[1].ToString() == "1")
                                    {
                                        ShowMsgBox(Arr[0]);
                                        //Session["WFOId"] = Arr[2];
                                        GenerateRIAckReport();
                                        return;
                                    }
                                    if (Arr[1].ToString() == "2")
                                    {
                                        ShowMsgBox(Arr[0]);
                                        return;
                                    }
                                }
                                // return;
                                else
                                {
                                    if (txtActiontype.Text == "A" || txtActiontype.Text == "R")
                                    {
                                        if (hdfWFOAutoId.Value == "0")
                                        {
                                            ApproveRejectAction();

                                            if (objSession.sTransactionLog == "1")
                                            {
                                                Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (RI) Failure ");
                                            }

                                            //Session["WFOId"] = objRIApproval.sWFDataId;
                                            GenerateRIAckReport();
                                            return;
                                        }
                                    }
                                }
                            }

                            Arr = objRIApproval.UpdateReplaceDetails(objRIApproval);
                            cmdApprove.Enabled = false;

                            if (Arr[1].ToString() == "1")
                            {
                                ShowMsgBox(Arr[0]);
                                //Session["WFOId"] = Arr[2];
                                GenerateRIAckReport();
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
                            if (hdfWFOAutoId.Value == "0")
                            {
                                ApproveRejectAction();

                                if (objSession.sTransactionLog == "1")
                                {
                                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (RI) Failure ");
                                }

                                //Session["WFOId"] = objRIApproval.sWFDataId;
                                GenerateRIAckReport();
                                return;
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                //lblMessage.Text = clsException.ErrorMsg();
                ShowMsgBox("Something went wrong while saving, Please approve once again.");
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




        bool ValidateForm()
        {
            bool bValidate = false;
            try
            {
                if (txtOilQtySK.Text.Trim() == "")
                {
                    txtOilQtySK.Focus();
                    ShowMsgBox("Please Enter RI Oil Quantity");
                    return bValidate;
                }
                if (txtCommentFromStoreKeeper.Text.Trim() == "")
                {
                    txtCommentFromStoreKeeper.Focus();
                    ShowMsgBox("Please Enter the Remarks/Comments");
                    return bValidate;
                }
                if (txtAcknoledgeDate.Text == "")
                {
                    txtAcknoledgeDate.Focus();
                    ShowMsgBox("Please select Ack Date");
                    return bValidate;
                }
                string sResult = Genaral.DateComparisionTransaction(txtAcknoledgeDate.Text, hdfDecommDate.Value, false, false);
                if (sResult == "1")
                {
                    txtAcknoledgeDate.Focus();
                    ShowMsgBox("Ack Date should be Greater than Decommission Date");
                    return bValidate;
                }

                //if (txtFailureDate.Text  != "")
                //{
                //    sResult = Genaral.DateComparision(txtAckDate.Text, txtFailureDate.Text, false, false);
                //    if (sResult == "2")
                //    {
                //        txtAckDate.Focus();
                //        ShowMsgBox("Ack Date should be Greater than Failure Date");
                //        return bValidate;
                //    }
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

        protected void cmdApproveView_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["ActionType"] != null && Convert.ToString(Request.QueryString["ActionType"]) != "")
                {
                    Response.Redirect("/Approval/ApprovalInbox.aspx", false);
                }
                else
                {
                    Response.Redirect("RIApprovalView.aspx", false);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        public void GetTransformerDetails()
        {
            try
            {
                clsRIApproval objRIApproval = new clsRIApproval();
                objRIApproval.sFailureId = txtFailureId.Text;
                objRIApproval.sDecommId = txtDecommId.Text;
                objRIApproval.GetFailureTCDetails(objRIApproval);

                txtDeCommWO.Text = objRIApproval.sDecomWorkOrder;
                txtDtrCode.Text = objRIApproval.sTCCode;
                txtMake.Text = objRIApproval.sTcMake;
                txtDTrSlno.Text = objRIApproval.sTcSlno;
                txtDTrId.Text = objRIApproval.sTCId;
                txtFailureDate.Text = objRIApproval.sFailureDate;
                txtQuantityOfOil.Text = objRIApproval.sOilQnty;
                oiltankcapacity.Text = objRIApproval.sOilQntyTank;
                txtRIno.Text = objRIApproval.sRINo;
                txtDecommDate.Text = objRIApproval.sRIDate;
                oiltankcapacity.Enabled = false;
                txtQuantityOfOil.Enabled = false;


                // txtDeCommWO.Text = objRIApproval.sDecomWorkOrder;
                //txtWoCreditNo.Text = objRIApproval.sCreditWorkOrder;

                hdfDTCCode.Value = objRIApproval.sDTCCode;

                objRIApproval.sDecommId = txtDecommId.Text;

                objRIApproval.GetRIDetails(objRIApproval);

                ///txtOilQuantity.Text = objRIApproval.sOilQuantity;
                //txtNoOfBarrels.Text = objRIApproval.sBarrels;

                if (objRIApproval.sOilQuantity != "")
                {

                    int oil = Convert.ToInt32(objRIApproval.sOilQuantity);
                    int value = 0;
                    int brl = 210;


                    if (oil >= 210)
                    {
                        value = (oil / brl);
                        if (oil % 210 != 0)
                        {
                            value = value + 1;
                        }

                    }
                    else
                    {
                        value = 1;
                    }
                    //txtNoOfBarrels.Text = value.ToString();
                }


                txtCommentFromStoreKeeper.Text = objRIApproval.sRemarks;
                hdfDecommDate.Value = objRIApproval.sDecommDate;

                if (objRIApproval.sRVNo != "")
                {
                    txtAcknoledgeDate.Text = objRIApproval.sRVDate;
                    txtAckNo.Text = objRIApproval.sRVNo;
                    txtOilQtySK.Text = objRIApproval.sOilQuantitySK;
                    txtManualAckNo.Text = objRIApproval.sManualRVACKNo;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void WorkFlowObjects(clsRIApproval objRIApproval)
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


                objRIApproval.sFormName = "RIApprove";
                if (objSession.sRoleType == "1")
                {
                    objRIApproval.sOfficeCode = objSession.OfficeCode;
                }
                else
                {
                    objRIApproval.sOfficeCode = Convert.ToString(ViewState["LOCCODE"]);
                }
                if (objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                {
                    // objRIApproval.sOfficeCode = objSession.OfficeCode;
                    objRIApproval.sOfficeCode = txtssOfficeCode.Text;
                }
                objRIApproval.sClientIP = sClientIP;
                objRIApproval.sWFObjectId = hdfWFOId.Value;
                objRIApproval.sWFAutoId = hdfWFOAutoId.Value;
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
                //if (objSession.RoleId == "12")
                //{
                //    txtActiontype.Text = "M";
                //}
                if (txtActiontype.Text == "A")
                {
                    cmdApprove.Text = "Approve";
                    pnlApproval.Enabled = false;
                }
                if (txtActiontype.Text == "R")
                {
                    cmdApprove.Text = "Reject";
                    pnlApproval.Enabled = false;
                }
                if (txtActiontype.Text == "M")
                {
                    //cmdApprove.Text = "Modify and Approve";                    
                    pnlApproval.Enabled = true;
                }

                if (objSession.RoleId == "5")
                {
                    dvComments.Style.Add("display", "block");
                }
                //cmdReset.Enabled = false;

                if (hdfWFOAutoId.Value != "0")
                {

                }

                // Check for Creator of Form
                bool bResult = CheckFormCreatorLevel();
                if (bResult == true)
                {
                    cmdApprove.Text = "Save";
                    pnlApproval.Enabled = true;


                    // To handle Record From Reject 
                    if (txtActiontype.Text == "A" && hdfWFOAutoId.Value == "0")
                    {
                        txtActiontype.Text = "M";
                        hdfRejectApproveRef.Value = "RA";
                    }
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

                if (objSession.RoleId != "2")
                {
                    //if (txtComment.Text.Trim() == "")
                    //{
                    //    ShowMsgBox("Enter Comments/Remarks");
                    //    txtComment.Focus();
                    //    return;

                    //}

                    if (objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                    {
                        if (txtComment.Text.Trim() == "")
                        {
                            txtComment.Text = "APPROVED.";
                        }
                    }
                    else
                    {


                        if (txtComment.Text.Trim() == "")
                        {
                            ShowMsgBox("Enter Comments/Remarks");
                            txtComment.Focus();
                            return;

                        }
                    }


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

                if (objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                {
                    objApproval.sOfficeCode = txtssOfficeCode.Text;
                }
                PGSqlConnection objCon = new PGSQL.DAL.PGSqlConnection(Constants.Password);
                string storeid = objCon.get_value("SELECT \"STO_SM_ID\" from \"TBLSTOREOFFCODE\" WHERE \"STO_OFF_CODE\"='" + txtAckNo.Text.Substring(0, 3) + "'");

                if (storeid != objSession.OfficeCode)
                {
                    objApproval.sStatus = "1";
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
                objApproval.sNewRecordId = txtDecommId.Text;

                bool bResult = false;
                if (txtActiontype.Text == "M")
                {
                    objApproval.sWFDataId = hdfWFDataId.Value;
                    if (hdfRejectApproveRef.Value == "RA")
                    {
                        objApproval.sApproveStatus = "1";
                    }
                //    bResult = objApproval.ModifyApproveWFRequest(objApproval);
                    bResult = objApproval.ModifyApproveWFRequest_Latest(objApproval);

                }
                else
                {
                  //  bResult = objApproval.ApproveWFRequest(objApproval);
                    bResult = objApproval.ApproveWFRequest_Latest(objApproval);

                }
                if (bResult == true)
                {

                    if (objApproval.sApproveStatus == "1")
                    {
                        ShowMsgBox("Approved Successfully");

                        if (objSession.RoleId == "2")
                        {
                            clsRIApproval objRI = new clsRIApproval();
                            //objRI.SendSMStoSectionOfficer(txtDtrCode.Text, txtDecommId.Text,txtFailureId.Text);

                        }
                        GenerateRIAckReport();

                        cmdApprove.Enabled = false;
                    }
                    else if (objApproval.sApproveStatus == "3")
                    {

                        ShowMsgBox("Rejected Successfully");
                        cmdApprove.Enabled = false;
                    }
                    else if (objApproval.sApproveStatus == "2")
                    {
                        ShowMsgBox("Modified and Approved Successfully");

                        if (objSession.RoleId == "2")
                        {
                            clsRIApproval objRI = new clsRIApproval();
                            //objRI.SendSMStoSectionOfficer(txtDtrCode.Text, txtDecommId.Text, txtFailureId.Text);

                        }
                        GenerateRIAckReport();
                        cmdApprove.Enabled = false;
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
               // lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public void WorkFlowConfig()
        {
            string WOID = string.Empty;
            string type = string.Empty;
            try
            {
                if (Request.QueryString["ActionType"] != null && Convert.ToString(Request.QueryString["ActionType"]) != "")
                {


                    if (Session["WFOId"] != null && Convert.ToString(Session["WFOId"]) != "")
                    {
                        hdfWFDataId.Value = Convert.ToString(Session["WFDataId"]);
                        hdfWFOId.Value = Convert.ToString(Session["WFOId"]);
                        hdfWFOAutoId.Value = Convert.ToString(Session["WFOAutoId"]);
                        hdfApproveStatus.Value = Convert.ToString(Session["ApproveStatus"]);

                        Session["WFDataId"] = null;
                        Session["WFOId"] = null;
                        Session["WFOAutoId"] = null;
                        Session["ApproveStatus"] = null;
                    }
                    GenerateAckNo();
                    if (hdfWFDataId.Value != "0")
                    {
                        GetRIDetailsFromXML(hdfWFDataId.Value);
                    }
                    SetControlText();
                    if (txtActiontype.Text == "V")
                    {
                        cmdApprove.Text = "View";
                        dvComments.Style.Add("display", "none");
                    }
                    if (objSession.sRoleType == "2")
                    {
                        //if(objSession.RoleId == "5")
                        //{
                        //    //WOID = hdfWFOAutoId.Value;
                        //    WOID = hdfWFOId.Value;
                        //    type = "2";
                        //}
                        //else
                        //{
                        //    WOID = hdfWFOId.Value;
                        //    type = "1";
                        //}
                        WOID = hdfWFOId.Value;
                        string sLocCode = clsStoreOffice.GetRICurrentOfficeCode(WOID, type);
                        if (sLocCode.Length > 3)
                        {
                            sLocCode = sLocCode.Substring(0, Constants.Division);
                        }
                        ViewState["LOCCODE"] = sLocCode;
                    }
                   
                }

                else
                {
                    cmdApprove.Text = "View";
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
                string sResult = objApproval.GetFormCreatorLevel("", objSession.RoleId, "RIApprove");
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
                if (cmdApprove.Text.Contains("View"))
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
                    Response.Redirect("RIApprovalView.aspx", false);
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
                string sTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtDTrId.Text));

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

        public void GenerateAckNo()
        {
            try
            {
                clsRIApproval objRI = new clsRIApproval();

                txtAckNo.Text = objRI.GenerateAckNo(Convert.ToInt32(txtDecommId.Text));

                txtAckNo.Attributes.Add("readonly", "readonly");
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void GenerateRIAckReport()
        {
            try
            {
                if (txtDecommId.Text != "")
                {
                    string strParam = string.Empty;
                    strParam = "id=RIAckReport&DecommId=" + txtDecommId.Text;
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                }
                else
                {
                    string sWFO_ID = hdfWFDataId.Value;
                    string strParam = string.Empty;
                    strParam = "id=RIReportso&wfoID=" + sWFO_ID + "&sDtrcode=" + txtDtrCode.Text + "&sFailurId=" + txtFailureId.Text;
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
        public void GetRIDetailsFromXML(string sWFDataId)
        {
            try
            {

                clsRIApproval objRIApproval = new clsRIApproval();
                objRIApproval.sWFDataId = sWFDataId;

                objRIApproval.GetRIDetailsFromXML(objRIApproval);

                //txtOilQuantity.Text = objRIApproval.sOilQuantity;
                txtCommentFromStoreKeeper.Text = objRIApproval.sRemarks;
                txtManualAckNo.Text = objRIApproval.sManualRVACKNo;

                txtAcknoledgeDate.Text = objRIApproval.sRVDate;
                txtAckNo.Text = objRIApproval.sRVNo;
                txtOilQtySK.Text = objRIApproval.sOilQuantitySK;
                hdfCrBy.Value = objRIApproval.sCrby;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }
        #endregion

        protected void cmdViewDecomm_Click(object sender, EventArgs e)
        {
            try
            {
                clsFormValues objApproval = new clsFormValues();
                string sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtType.Text));
                string sFailureId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtFailureId.Text));
                string sDecommId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtDecommId.Text));

                string url = "/DTCFailure/DeCommissioning.aspx?TypeValue=" + sTaskType + "&ReferID=" + sFailureId + "&ReplaceId=" + sDecommId;
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
                string sBOId = "15";
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

        public DataTable BindgridView(string FtpServer, string username, string password)
        {
            DataTable dtFiles = new DataTable();
            try
            {
                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);

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
                    string SFTPmainfolder = Convert.ToString(ConfigurationSettings.AppSettings["VirtualDirectoryDocs"]);

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
                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["VirtualDirectoryDocs"]);

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

                sReferId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtDecommId.Text));

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