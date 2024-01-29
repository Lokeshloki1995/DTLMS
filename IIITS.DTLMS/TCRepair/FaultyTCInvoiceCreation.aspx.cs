using IIITS.DTLMS.BL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.TCRepair
{
    public partial class FaultyTCInvoiceCreation : System.Web.UI.Page
    {
        string strFormCode = "TCRepairIssue";
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
                txtindentdate.Attributes.Add("readonly", "readonly");
                txtPODate.Attributes.Add("readonly", "readonly");
                txtInvoiceDate.Attributes.Add("readonly", "readonly");
                txtEstimationDate.Attributes.Add("readonly", "readonly");
                txtWorkOrderDate.Attributes.Add("readonly", "readonly");
                txtEstimationDate.Enabled = false;
                txtWorkOrderDate.Enabled = false;
                txtPODate.Enabled = false;
                CalendarExtender_txtInvoiceDate.EndDate = System.DateTime.Now;
                CalendarExtender_txtindentdate.EndDate = System.DateTime.Now;





                // txtindentdate.Enabled = false;
                //  txtInvoiceDate.Enabled = false;

                if (!IsPostBack)
                {
                    if (Request.QueryString["StoreId"] != null && Request.QueryString["StoreId"].ToString() != "")
                    {
                        //DisableCopy();
                        //Session["TcId"]
                        if (Session["TcId"] != null && Session["TcId"].ToString() != "")
                        {
                           // txtSelectedTcId.Text = Session["TcId"].ToString();
                            Session["TcId"] = null;
                        }
                       
                        txtEstimationDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                        txtWorkOrderDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                        txtPODate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                         txtInvoiceDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                        txtindentdate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                        CalendarExtender_txtPODate.StartDate = System.DateTime.Now;
                        //CalendarExtender_txtInvoiceDate.StartDate = System.DateTime.Now;
                        //CalendarExtender_txtindentdate.StartDate = System.DateTime.Now;
                      //  LoadFaultTc();
                    }
                    if (Request.QueryString["ReferID"] != null && Convert.ToString(Request.QueryString["ReferID"]) != "")
                    {
                        txtRSMID.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ReferID"]));
                        Genaral.Load_Combo("select DISTINCT \"TR_ID\",\"TR_NAME\" from \"TBLREPAIRERRATES\" inner join \"TBLTRANSREPAIRER\" on \"TR_ID\"=\"RR_REP_ID\" ORDER BY \"TR_ID\"", cmbRepairer);
                        hdfGuarenteeType.Value = txtRSMID.Text;
                        getRepDetails(txtRSMID.Text);
                        cmbRepairer.Enabled = false;

                        if(txtIndentNO.Text =="" && txtInvoiceNo.Text =="")
                        {
                            GenerateInvoiceNo();
                            GenerateIndentNo();
                        }
                      
                      //  CalendarExtender_txtInvoiceDate.StartDate = txtPODate.Text.ToString("dd/MM/yyyy");

                        DateTime POdate = Convert.ToDateTime(Convert.ToString(txtPODate.Text));
                        int Poodate = Convert.ToInt32((DateTime.Now.AddDays(-1) - POdate).TotalDays);
                        // CalendarExtender_txtInvoiceDate.StartDate = DateTime.Today.AddDays(-Poodate);
                        CalendarExtender_txtInvoiceDate.StartDate = POdate;


                        DateTime PO1date = Convert.ToDateTime(Convert.ToString(txtPODate.Text));
                        int Poo1date = Convert.ToInt32((DateTime.Now.AddDays(-2) - PO1date).TotalDays);
                        CalendarExtender_txtindentdate.StartDate = PO1date;
;

                    }
                    txtPODate.Attributes.Add("onblur", "return ValidateDate(" + txtPODate.ClientID + ");");
                    //  CheckAccessRights("2");

                    bool bAccResult = CheckAccessRights("2");
                    if (bAccResult == false)
                    {
                        return;
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

        public void getRepDetails(string id)
        {
            clsDTrRepairActivity objRepair = new clsDTrRepairActivity();
            objRepair.sRSMID = id;
            objRepair.GetRepairerTCDetails(objRepair);

            txtEstimationNo.Text = objRepair.sEstimationNo;
            txtEstimationDate.Text = objRepair.sEstimationDate;
            txtWorkorderNo.Text = objRepair.sWorkorderNo;
            txtWorkOrderDate.Text = objRepair.sWorkorderDate;
            txtPONo.Text = objRepair.sPurchaseOrderNo;
            txtPODate.Text = objRepair.sPurchaseDate;
            txtEstimationAmount.Text = objRepair.sEstimationAmount;
            txtqnty.Text = objRepair.sQty;
            cmbRepairer.SelectedValue = objRepair.sRepairerId;

            txtindentdate.Text = objRepair.sIndentDate;
            txtInvoiceDate.Text = objRepair.sInvoiceDate;
            txtIndentNO.Text = objRepair.sIndenteNo;
            txtInvoiceNo.Text = objRepair.sInvoiceNo;

            LoadRepairSentDTR();
        }
        public void LoadRepairSentDTR()
        {
            try
            {
                DataTable dt = new DataTable();
                clsDTrRepairActivity objRepair = new clsDTrRepairActivity();

                dt = objRepair.LoadRepairSentDTR(txtRSMID.Text);
                if (dt.Rows.Count > 0)
                {
                    ViewState["FaultTC"] = dt;
                    grdFaultyTCAmt.DataSource = dt;
                    grdFaultyTCAmt.DataBind();
                    grdFaultyTCAmt.Visible = true;
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
                clsDTrRepairActivity objTcRepair = new clsDTrRepairActivity();
                string[] Arr = new string[2];
                //Check AccessRights
                bool bAccResult = CheckAccessRights("2");
                if (bAccResult == false)
                {
                    return;
                }
                if (ValidateForm() == true)
                {
                    objTcRepair.sInvoiceNo = txtInvoiceNo.Text;
                    objTcRepair.sInvoiceDate = txtInvoiceDate.Text;
                    objTcRepair.sIndenteNo = txtIndentNO.Text;
                    objTcRepair.sIndentDate = txtindentdate.Text;
                   
                    objTcRepair.sCrby = objSession.UserId;
                    objTcRepair.sOfficeCode = objSession.OfficeCode;
                    objTcRepair.sRSMID = txtRSMID.Text;
                    objTcRepair.sWFObjectId = hdfWFOId.Value;

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
                    objTcRepair.sClientIP = sClientIP;

                    Arr = objTcRepair.SaveRepairIvoiceDetails(objTcRepair);


                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (TCRepairerIssue) Repair ");
                    }

                    if (Arr[1].ToString() == "0")
                    {
                        ShowMsgBox(Arr[0].ToString());
                      
                        cmdSave.Enabled = false;
                        cmdGatePass.Enabled = true;

                        return;
                    }


                    if (Arr[1].ToString() == "0")
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID, "alert('Transformers Issued Sucessfully to Repairer/Supplier'); location.href='FaultTCSearch.aspx';", true);
                        Reset();
                        cmdGatePass.Enabled = true;
                        cmdSave.Enabled = false;
                        return;
                    }
                    else if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0].ToString());
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

    
      protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                Reset();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void Reset()
        {
            try
            {
               txtInvoiceNo.Text = string.Empty;
                txtInvoiceDate.Text = string.Empty;
                txtIndentNO.Text = string.Empty;
                txtindentdate.Text = string.Empty;
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
                if (txtInvoiceDate.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Invoice Date");
                    txtInvoiceDate.Focus();
                    return bValidate;
                }
                if (txtindentdate.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Indent Date");
                    txtindentdate.Focus();
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
   
        //  #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "RepairerInvoiceCreation";
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
        public void GenerateInvoiceNo()
        {
            try
            {
                clsInvoice objInvoice = new clsInvoice();
                string sRoletype = objSession.sRoleType;
                txtInvoiceNo.Text = objInvoice.GenerateInvoiceNoForRepairerTC(objSession.OfficeCode, sRoletype);
                txtInvoiceNo.ReadOnly = true;
               // hdfInvoiceNo.Value = txtInvoiceNo.Text;
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
                clsIndent objIndent = new clsIndent();

                txtIndentNO.Text = objIndent.GenerateIndentNoForRepairerTC(objSession.OfficeCode);
                txtIndentNO.ReadOnly = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
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
                return bValidate;
            }
        }

        protected void cmdGatePass_Click(object sender, EventArgs e)
        {
            try
            {
                clsInvoice objInvoice = new clsInvoice();
                string[] Arr = new string[2];
                if (txtActiontype.Text == "V")
                {
                    //string strParam = "id=RepairGatepass&InvoiceId=" + txtInvoiceNo.Text + "&TransId=" + txtRepairMasterId.Text;
                    string strParam = "id=RepairGatepass&InvoiceId=" + txtInvoiceNo.Text + "&TransId=" + txtRSMID.Text;
                    RegisterStartupScript("PrintD", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                    //LoadRepairSentDTR();
                    return;
                }
                if (ValidateGatePass() == true)
                {
                    objInvoice.sGatePassId = txtGpId.Text;
                    objInvoice.sVehicleNumber = txtVehicleNo.Text.Replace("'", "");
                    objInvoice.sReceiptientName = txtReciepient.Text.Replace("'", "");
                    objInvoice.sChallenNo = txtChallen.Text.Replace("'", "");
                    objInvoice.sCreatedBy = objSession.UserId;

                    if (ViewState["FaultTC"] != null)
                    {
                        DataTable dt = (DataTable)ViewState["FaultTC"];
                        string sTCCode = "";
                        //foreach (GridViewRow row in grdFaultyTCAmt.Rows)

                        //{
                        //    sTCCode += ((Label)row.FindControl("lblTCCode")).Text.Trim() + ",";
                        //    objInvoice.sIssueQty = Convert.ToString(grdFaultyTCAmt.Rows.Count);              
                        //}
                        objInvoice.sTcCode = sTCCode;
                    }

                 //   objInvoice.sInvoiceNo = hdfInvoiceNo.Value.Replace("'", "");
                    objInvoice.sInvoiceNo = txtInvoiceNo.Text.Replace("'", "");

                    Arr = objInvoice.SaveUpdateGatePassDetails(objInvoice);

                    if (Arr[0].ToString() == "0")
                    {
                        txtGpId.Text = objInvoice.sGatePassId;
                        string strParam = "id=RepairGatepass&InvoiceId=" + txtInvoiceNo.Text + "&TransId=" + txtRSMID.Text;
                        RegisterStartupScript("PrintD", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

                        return;
                    }
                    if (Arr[0].ToString() == "1")
                    {
                        //string strParam = "id=RepairGatepass&InvoiceId=" + txtInvoiceNo.Text + "&TransId=" + txtRepairMasterId.Text; 
                        string strParam = "id=RepairGatepass&InvoiceId=" + txtInvoiceNo.Text + "&TransId=" + txtRSMID.Text;
                        RegisterStartupScript("PrintD", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

                        return;
                    }

                    if (Arr[0].ToString() == "2")
                    {
                        ShowMsgBox(Arr[1]);
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

   
        public void WorkFlowObjects(clsDTrRepairActivity objDTRRepair)
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

                objDTRRepair.sFormName = "FaultTCSearch";
                objDTRRepair.sOfficeCode = objSession.OfficeCode;
                objDTRRepair.sClientIP = sClientIP;
                objDTRRepair.sWFObjectId = hdfWFOId.Value;
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
                    Panel1.Enabled = false;
                    cmdSave.Enabled = true;
                    txtComment.Visible = true;
                }
                if (txtActiontype.Text == "R")
                {
                    cmdSave.Text = "Reject";
                    pnlApproval.Enabled = false;
                    Panel1.Enabled = false;
                    txtComment.Visible = true;
                }
                if (txtActiontype.Text == "M")
                {
                    cmdSave.Text = "Modify and Approve";
                    pnlApproval.Enabled = true;
                    Panel1.Enabled = false;

                    txtComment.Visible = true;
                    txtPONo.Enabled = false;
                    //  txtInvoiceDate.Enabled = false;
                    //  txtInvoiceNo.Enabled = false;
                }

                if (objSession.RoleId == "2" || objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                {
                    dvComments.Style.Add("display", "block");

                 //   grdFaultTC.Columns[10].Visible = false;
                    // grdFaultTC.Columns.RemoveAt(10);    //Index is the index of the column you want to remove
                    // grdFaultTC.DataBind();


                }
                //cmdReset.Enabled = false;

                if (hdfWFOAutoId.Value != "0")
                {
                    cmdSave.Text = "Save";

                }

                // Check for Creator of Form
                bool bResult = CheckFormCreatorLevel();
                if (bResult == true)
                {
                    cmdSave.Text = "Save";
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

                if (objSession.RoleId != "5")
                {
                    if (txtComment.Text.Trim() == "")
                    {
                        ShowMsgBox("Enter Comments/Remarks");
                        txtComment.Focus();
                        return;

                    }
                }


                objApproval.sCrby = objSession.UserId;
                objApproval.sOfficeCode = objSession.OfficeCode;
                // objApproval.sOfficeCode = clsStoreOffice.Getofficecode(objSession.OfficeCode);
                objApproval.sApproveComments = txtComment.Text.Trim();
                objApproval.sWFObjectId = hdfWFOId.Value;

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
                clsDTrRepairActivity objTcRepair = new clsDTrRepairActivity();

                bool bResult = objApproval.ApproveWFRequest(objApproval);

                //objTcRepair.updatemodifyeddata(objTcRepair.sIssueDate, objTcRepair.sPurchaseDate, objTcRepair.sManualInvoiceNo, objTcRepair.sOldPONo, objTcRepair.sPORemarks, objTcRepair.sRSMID);

                if (bResult == true)
                {

                    if (objApproval.sApproveStatus == "1")
                    {
                        ShowMsgBox("Approved Successfully");

                        if (objSession.RoleId == "2")
                        {
                            clsRIApproval objRI = new clsRIApproval();
                            // objRI.SendSMStoSectionOfficer(txtDtrCode.Text, txtDecommId.Text, txtFailureId.Text);

                        }


                        cmdSave.Enabled = false;
                    }
                    else if (objApproval.sApproveStatus == "3")
                    {

                        ShowMsgBox("Rejected Successfully");
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

        public void WorkFlowConfig()
        {
            try
            {
                if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
                {

                    txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));
                    if (Session["WFOId"] != null && Session["WFOId"].ToString() != "")
                    {
                        hdfWFDataId.Value = Session["WFDataId"].ToString();
                        hdfWFOId.Value = Convert.ToString(Session["WFOId"]);
                        hdfWFOAutoId.Value = Convert.ToString(Session["WFOAutoId"]);

                        Session["WFDataId"] = null;
                        Session["WFOId"] = null;
                        Session["WFOAutoId"] = null;
                    }

                   
                    if (txtActiontype.Text == "V")
                    {
                        cmdSave.Text = "View";
                        cmdSave.Enabled = false;
                        dvComments.Style.Add("display", "none");
                        txtVehicleNo.Enabled = false;
                        txtChallen.Enabled = false;
                        txtReciepient.Enabled = false;
                    }
                }
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
                string sResult = objApproval.GetFormCreatorLevel("", objSession.RoleId, "TCRepairIssue");
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
        #endregion

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
                    Response.Redirect("FaultTCSearch.aspx", false);
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdSearchPO_Click(object sender, EventArgs e)
        {
            try
            {
                string[] Arr = new string[2];
                clsDtcMaster objDtcMaster = new clsDtcMaster();
                //  Arr = objDtcMaster.GetPONo(txtPonum.Text);
                if (Arr[1] != "1")
                {
                    ShowMsgBox(Arr[0]);
                    //  txtPonum.Text = "";
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
