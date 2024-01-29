using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Configuration;

namespace IIITS.DTLMS.StoreTransfer
{
    public partial class StoreIndent : System.Web.UI.Page
    {
        string strFormCode = "StoreIndent";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                lblMessage.Text = string.Empty;
                objSession = (clsSession)Session["clsSession"];
                Form.DefaultButton = cmdSave.UniqueID;
                txtIndentDate.Attributes.Add("readonly", "readonly");
                txtIndentDate_CalendarExtender1.EndDate = System.DateTime.Now;
                if (!IsPostBack)
                {
                    txtIndentDate_CalendarExtender1.EndDate = System.DateTime.Now;
                    GenerateInvoiceNo();
                    txtIndentDate.Attributes.Add("onblur", "return ValidateDate(" + txtIndentDate.ClientID + ");");
                    Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" ='C' ORDER BY \"MD_ORDER_BY\" ", "--Select--", ddlCapacity);

                    //loading store dropdown except current logged in User store Name 
                    string sOfficeCode = string.Empty;
                    int Division = Convert.ToInt32(ConfigurationSettings.AppSettings["Division_code"]);

                    //string QRY = "SELECT \"SM_ID\",\"SM_NAME\" FROM \"TBLSTOREMAST\" where  " + clsStoreOffice.GetOfficeCodenot(objSession.OfficeCode, "SM_OFF_CODE") + " order by \"SM_NAME\" ";
                    Genaral.Load_Combo("SELECT \"SM_ID\",\"SM_NAME\" FROM \"TBLSTOREMAST\" where \"SM_ID\" <> " + objSession.OfficeCode + " order by \"SM_NAME\" ", "--Select--", ddlStore);

                    if (Request.QueryString["QryIndentId"] != null && Request.QueryString["QryIndentId"].ToString() != "")
                    {
                        txtIndentId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryIndentId"]));
                        if (!txtIndentId.Text.Contains("-"))
                        {
                            LoadIndentDetails(txtIndentId.Text);
                            cmdSave.Text = "View";
                            btnReset.Visible = false;
                        }


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
        public void LoadTcCapacity(string strIndentId)
        {
            DataTable dtTcCapacity = new DataTable();
            try
            {
                clsStoreIndent objTcTransfer = new clsStoreIndent();
                objTcTransfer.sIndentId = Convert.ToString(strIndentId);

                dtTcCapacity = objTcTransfer.LoadCapacityGrid(objTcTransfer);

                grdTcTransfer.DataSource = dtTcCapacity;
                grdTcTransfer.DataBind();
                // cmdSave.Text = "Update";
                ViewState["dt"] = dtTcCapacity;


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void LoadCapacity(DataTable dt)
        {
            try
            {
                //dt = objTcTransfer.LoadCapacity();
                grdTcTransfer.DataSource = dt;
                grdTcTransfer.DataBind();
                grdTcTransfer.Visible = true;
                ddlCapacity.SelectedIndex = 0;
                txtQuantity.Text = string.Empty;
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
        public void LoadIndentDetails(string strIndentId)
        {
            try
            {
                clsStoreIndent objTcTransfer = new clsStoreIndent();
                objTcTransfer.sIndentId = Convert.ToString(strIndentId);

                objTcTransfer.GetIndentDetails(objTcTransfer);

                txtSiId.Text = objTcTransfer.sSiId;
                ddlCapacity.SelectedValue = objTcTransfer.sTcCapacity;
                txtQuantity.Text = objTcTransfer.sQuantity;
                txtIndentNumber.Text = objTcTransfer.sIndentNo;
                txtIndentDate.Text = objTcTransfer.sIndentDate;
                ddlStore.SelectedValue = objTcTransfer.sToStoreId;
                txtDescription.Text = objTcTransfer.sDescription;

                LoadTcCapacity(txtIndentId.Text);

                //cmdSave.Enabled = false;
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
                //GetTcDetails();

                //View For Generate Report
                if (cmdSave.Text == "View")
                {
                    if (hdfApproveStatus.Value != "")
                    {
                        if (hdfApproveStatus.Value == "1" || hdfApproveStatus.Value == "2")
                        {

                            GenerateIndentReport();
                        }
                    }
                    else
                    {
                        GenerateIndentReport();
                    }
                    return;
                }

                //to check whether capacity and quantity are added
                if (ViewState["dt"] != null)
                {
                    //CheckCapacity();
                    SaveStoreTransfer();
                    if (txtActiontype.Text == "A" || txtActiontype.Text == "R" || txtActiontype.Text == "M")
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID, "location.href='/Approval/ApprovalInbox.aspx';", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID, "location.href='StoreIndentView.aspx';", true);
                    }
                }
                else
                {
                    ShowMsgBox("Please Add Capacity and Quantity");
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

                string[] Arr = new string[3];
                if (txtIndentNumber.Text.Trim() == string.Empty)
                {
                    ShowMsgBox("Please Enter Indent Number");
                    return bValidate;
                }
                if (txtDescription.Text.Trim() == string.Empty)
                {
                    ShowMsgBox("Please Enter Description");
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
        public void SaveStoreTransfer()
        {
            clsStoreIndent objStoreIndent = new clsStoreIndent();
            DataTable dt;
            try
            {
                if (ValidateForm() == true)
                {
                    string[] Arr = new string[2];
                    dt = (DataTable)ViewState["dt"];
                    objStoreIndent.sToStoreId = ddlStore.SelectedValue;
                    objStoreIndent.sIndentNo = txtIndentNumber.Text.Replace("'", "");
                    objStoreIndent.sIndentDate = txtIndentDate.Text.Replace("'", "");
                    objStoreIndent.sDescription = txtDescription.Text.Replace("'", "");
                    objStoreIndent.sCrBy = objSession.UserId;
                    objStoreIndent.sOfficeCode = objSession.OfficeCode;
                    if (txtSiId.Text != "")
                    {
                        objStoreIndent.sSiId = txtSiId.Text;
                    }
                    objStoreIndent.ddtCapacityGrid = dt;
                    objStoreIndent.sToStoreName = ddlStore.SelectedItem.Text.ToString();


                    if (txtActiontype.Text == "A" || txtActiontype.Text == "R")
                    {
                        ApproveRejectAction();

                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (storeIndent) InterStore ");
                        }

                        return;
                    }


                    //Workflow
                    WorkFlowObjects(objStoreIndent);


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

                        objStoreIndent.sActionType = txtActiontype.Text;
                        objStoreIndent.sCrBy = objSession.UserId;

                        Arr = objStoreIndent.SaveStoreTransfer(objStoreIndent);
                        if (Arr[1].ToString() == "0")
                        {
                            hdfWFDataId.Value = objStoreIndent.sWFDataId;
                            ApproveRejectAction();

                            if (objSession.sTransactionLog == "1")
                            {
                                Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (storeIndent) InterStore ");
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

                    Arr = objStoreIndent.SaveStoreTransfer(objStoreIndent);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (storeIndent) InterStore ");
                    }

                    if (Arr[1].ToString() == "0")
                    {
                        txtSiId.Text = objStoreIndent.sSiId;
                        cmdSave.Enabled = false;
                        ShowMsgBox(Arr[0]);

                        return;
                    }
                    else
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
        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtActiontype.Text != "R")
                {
                    txtDescription.Text = string.Empty;
                    txtIndentNumber.Text = string.Empty;
                    txtIndentDate.Text = string.Empty;
                    ddlStore.SelectedIndex = 0;
                    txtIndentNumber.Enabled = true;
                    cmdSave.Text = "Save";
                    txtSiId.Text = string.Empty;
                    ddlCapacity.SelectedIndex = 0;
                    txtQuantity.Text = string.Empty;
                    grdTcTransfer.Visible = false;
                    ViewState["dt"] = null;
                    lblMessage.Text = string.Empty;
                    grdCapacityDetails.Visible = false;
                    GenerateInvoiceNo();
                }
                else
                {
                    txtComment.Text = string.Empty;
                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void grdTcTransfer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            clsStoreIndent objTcTransfer = new clsStoreIndent();
            try
            {
                if (e.CommandName == "Remove")
                {
                    DataTable dt = (DataTable)ViewState["dt"];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                        Label lblCapacity = (Label)row.FindControl("lblCapacity");
                        //to remove selected Capacity from grid
                        if (lblCapacity.Text == Convert.ToString(dt.Rows[i]["SO_CAPACITY"]))
                        {
                            dt.Rows[i].Delete();
                            dt.AcceptChanges();
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["dt"] = dt;
                    }
                    else
                    {
                        ViewState["dt"] = null;
                    }
                    LoadCapacity(dt);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                //if datatble is empty,add columns to table
                if (ViewState["dt"] != null)
                {
                    dt = (DataTable)ViewState["dt"];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (ddlCapacity.SelectedItem.Text == Convert.ToString(dt.Rows[i]["SO_CAPACITY"]))
                        {
                            ShowMsgBox("Capacity Already Added");
                            return;

                        }

                    }
                }

                if (ViewState["dt"] == null)
                {
                    dt.Columns.Add("SO_ID");
                    dt.Columns.Add("SO_CAPACITY");
                    dt.Columns.Add("SO_QNTY");
                }
                else
                {
                    //load datatble from viewstate
                    dt = (DataTable)ViewState["dt"];
                }
                DataRow dRow = dt.NewRow();
                int qnty = Convert.ToInt32(txtQuantity.Text);
                //dRow["SO_QNTY"] =Convert.ToInt32(txtQuantity.Text);
                dRow["SO_QNTY"] = qnty;
                if (Convert.ToString(dRow["SO_QNTY"]) == "0")
                {
                    ShowMsgBox("Quantity Should Not be Zero");
                    return;
                }
                dRow["SO_CAPACITY"] = ddlCapacity.SelectedItem.Text;
                dt.Rows.Add(dRow);
                ViewState["dt"] = dt;
                LoadCapacity(dt);
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
                    Response.Redirect("StoreIndentView.aspx", false);
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdTcTransfer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtTcCapacity = new DataTable();
                grdTcTransfer.PageIndex = e.NewPageIndex;
                dtTcCapacity = (DataTable)ViewState["dt"];
                LoadCapacity(dtTcCapacity);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdTcTransfer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (Request.QueryString["ActionType"] != ""   && Request.QueryString["ActionType"] != null )
                {


                    txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));

                    ImageButton imgBtnDelete;
                    imgBtnDelete = (ImageButton)e.Row.FindControl("imgBtnDelete");

                    if (txtActiontype.Text == "A" && imgBtnDelete != null )
                    {
                        imgBtnDelete.Visible = true;
                        imgBtnDelete.Enabled = false;
                        
                    }
                    
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void GenerateInvoiceNo()
        {
            try
            {
                clsStoreIndent objInvoice = new clsStoreIndent();
                string sRoletype = objSession.sRoleType;
                txtIndentNumber.Text = objInvoice.GenerateInvoiceNo(objSession.OfficeCode, sRoletype);
                txtIndentNumber.ReadOnly = true;
                //hdfInvoiceNo.Value = txtInvoiceNo.Text;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlStore.SelectedIndex > 0)
                {
                    clsStoreIndent objSIndent = new clsStoreIndent();
                    DataTable dt = new DataTable();
                    dt = objSIndent.LoadStoreCapacityGrid(ddlStore.SelectedValue);
                    grdCapacityDetails.DataSource = dt;
                    grdCapacityDetails.DataBind();
                    grdCapacityDetails.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdCapacityDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtTcCapacity = new DataTable();
                grdCapacityDetails.PageIndex = e.NewPageIndex;
                dtTcCapacity = (DataTable)ViewState["dt"];
                clsStoreIndent objSIndent = new clsStoreIndent();
                DataTable dt = new DataTable();
                dt = objSIndent.LoadStoreCapacityGrid(ddlStore.SelectedValue);
                grdCapacityDetails.DataSource = dt;
                grdCapacityDetails.DataBind();
                grdCapacityDetails.Visible = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void WorkFlowObjects(clsStoreIndent objStoreIndent)
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


                objStoreIndent.sFormName = "StoreIndent";
                objStoreIndent.sOfficeCode = objSession.OfficeCode;
                objStoreIndent.sClientIP = sClientIP;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        #region Load From XML
        public void GetStoreIndentDetailsFromXML(string sWFDataId)
        {
            try
            {

                // If the Data saved in Main Table then this function shd not execute, so done restriction like below
                // And commented for temprary purpose.. nee to change in future

                //if (!txtIndentId.Text.Contains("-"))
                //{
                //    return;
                //}

                clsStoreIndent objStoreIndent = new clsStoreIndent();
                objStoreIndent.sWFDataId = sWFDataId;
                objStoreIndent.GetStoreIndentDetailsFromXML(objStoreIndent);

                txtIndentNumber.Text = objStoreIndent.sIndentNo;
                txtIndentDate.Text = objStoreIndent.sIndentDate;
                ddlStore.SelectedValue = objStoreIndent.sToStoreId;
                txtDescription.Text = objStoreIndent.sDescription;


                DataTable dt = objStoreIndent.ddtCapacityGrid;

                grdTcTransfer.DataSource = dt;
                grdTcTransfer.DataBind();

                ViewState["dt"] = dt;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }
        #endregion


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
                    pnlApprovalIndent.Enabled = false;
                }
                if (txtActiontype.Text == "R")
                {
                    cmdSave.Text = "Reject";
                    pnlApproval.Enabled = false;
                    pnlApprovalIndent.Enabled = false;
                    grdTcTransfer.Enabled = false;
                }
                if (txtActiontype.Text == "M")
                {
                    cmdSave.Text = "Modify and Approve";
                    pnlApproval.Enabled = true;
                    pnlApprovalIndent.Enabled = true;
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
                    pnlApprovalIndent.Enabled = true;

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
                objApproval.sOfficeCode = GetOfficeCodeFromStore();
                objApproval.sApproveComments = txtComment.Text.Trim();
                objApproval.sWFObjectId = hdfWFOId.Value;

                //Approve
                if (txtActiontype.Text == "A")
                {
                    objApproval.sApproveStatus = "1";
                    objApproval.sRefOfficeCode = GetOfficeCodeFromStore();
                }
                //Reject
                if (txtActiontype.Text == "R")
                {
                    objApproval.sApproveStatus = "3";
                }

                //Modify and Approve
                if (txtActiontype.Text == "M")
                {
                    objApproval.sApproveStatus = "2";
                    if (objSession.RoleId != "5")
                    {
                        objApproval.sRefOfficeCode = GetOfficeCodeFromStore();
                    }
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
                        //ShowMsgBox("Approved Successfully");
                        //txtIndentId.Text = objApproval.sNewRecordId;
                        //GenerateIndentReport();
                        //cmdSave.Enabled = false;
                        ShowMsgBox("Approved Successfully");
                        txtIndentId.Text = objApproval.sNewRecordId;

                        string RoleId = objSession.RoleId;
                        if(RoleId != "5")
                        {
                            if (objApproval.sRecordId.Contains("-"))
                            {
                                GenerateIndentReport();
                            }
                            else
                            {
                                GenerateIndentReport();
                            }
                        }
                        cmdSave.Enabled = false;

                    }
                    else if (objApproval.sApproveStatus == "3")
                    {

                        ShowMsgBox("Rejected Successfully");

                        cmdSave.Enabled = false;
                    }

                    else if (objApproval.sApproveStatus == "2")
                    {
                        ShowMsgBox("Modified and Approved Successfully");
                        txtIndentId.Text = objApproval.sNewRecordId;
                        GenerateIndentReport();
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

                        GetStoreIndentDetailsFromXML(hdfWFDataId.Value);

                    }

                    SetControlText();

                    if (txtActiontype.Text == "V")
                    {
                        cmdSave.Text = "View";
                        cmdSave.Enabled = false;
                        dvComments.Style.Add("display", "none");
                    }

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
                string sResult = objApproval.GetFormCreatorLevel("", objSession.RoleId, "StoreIndent");
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


        public string GetOfficeCodeFromStore()
        {
            try
            {
                clsStoreIndent objStoreIndent = new clsStoreIndent();
                string sOfficeCode = objStoreIndent.GetOfficeCodeFromStore(ddlStore.SelectedValue);
                return sOfficeCode;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }


        public void GenerateIndentReport()
        {
            try
            {

                string strParam = string.Empty;
                strParam = "id=InterStoreIndent&IndentId=" + txtIndentId.Text;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}