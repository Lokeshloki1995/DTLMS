using IIITS.DTLMS.BL;
using IIITS.DTLMS.BL.MasterForms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Configuration;

namespace IIITS.DTLMS.MasterForms
{
    public partial class WorkOrderRangeAllocation : System.Web.UI.Page
    {
        clsSession objSession;
        /// <summary>
        /// to set default values or check for postBacks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];
                cmbDivision.Enabled = false;
                txtallocationDate.ReadOnly = true;
                if (!IsPostBack)
                {
                    txtallocationDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                    string QryDiv = string.Empty;
                    if (objSession.sRoleType == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["RoleTypeStore"]))
                    {
                        QryDiv = "SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" where \"DIV_CODE\"=(select \"SM_CODE\" ";
                        QryDiv += "from \"TBLSTOREMAST\" where \"SM_ID\"='" + objSession.OfficeCode + "') ";
                    }
                    else
                    {
                        QryDiv = "SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" where \"DIV_CODE\"=(select \"SM_CODE\" ";
                        QryDiv += "from \"TBLSTOREMAST\" where \"SM_ID\"='" + clsStoreOffice.GetStoreID(objSession.OfficeCode) + "') ";
                    }
                    Genaral.Load_Combo(QryDiv, cmbDivision);
                    string QryAccHd = "select \"MD_ID\", case when \"MD_COMM_ACCCODE\" = '' then \"MD_DECOMM_ACCCODE\" else ";
                    QryAccHd += "\"MD_COMM_ACCCODE\" END from \"TBLMASTERDATA\" where \"MD_TYPE\"='CWIP' AND \"MD_STATUS\" ='1' ORDER BY \"MD_ID\"";
                    Genaral.Load_Combo(QryAccHd, "--Select--", cmbAccHead);

                    if (Request.QueryString["ActionType"] != null && Convert.ToString(Request.QueryString["ActionType"]) != "")
                    {
                        txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));
                    }
                    clsApproval objLevel = new clsApproval();
                    string sLevel = objLevel.sGetApprovalLevel(Convert.ToString(System.Configuration.ConfigurationManager.
                        AppSettings["WorkOrderRangeAllocation"]), objSession.RoleId);
                    if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                    {
                        cmdSave.Text = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SubmitText"]);
                    }
                    else if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                    {
                        cmdSave.Text = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ApproveText"]);
                    }
                    if (txtActiontype.Text != "")
                    {
                        cmdWORngAllctnView.Visible = false;
                    }

                    WorkFlowConfig();
                }
                txtFinancialYear.Text = GetFinancialYear();
                txtallocationDate.Attributes.Add("readonly", "readonly");
            }
            catch(Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            }

        /// <summary>
        /// to get current financial year
        /// </summary>
        /// <returns></returns>
        public string GetFinancialYear()
        {
            string year = string.Empty;
            try
            {
                ClsWorkOrderRangeAllocation objwo = new ClsWorkOrderRangeAllocation();
                year = objwo.GetFinancialYearWO();
                return year;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return year;
            }
        }
        /// <summary>
        /// to get description based on selected account head  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbAccHead_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool commid = false;
            string[] Arr = new string[3];
            try
            {
                txtQuantity.Text = string.Empty;
                txtWostRng1.Text = string.Empty;
                txtWostRng3.Text = string.Empty;
                txtWoEdRng1.Text = string.Empty;
                txtWoEdRng3.Text = string.Empty;
                txtWostRng1.ReadOnly = false;
                ClsWorkOrderRangeAllocation objwo = new ClsWorkOrderRangeAllocation();


                commid = objwo.checkCommDecommID(cmbAccHead.SelectedItem.Text, cmbDivision.SelectedValue, txtFinancialYear.Text);
                if (!commid)
                {
                    string msg = "Work Order Number Range is Allocated for " + cmbAccHead.SelectedItem.Text + "";
                    msg += " Account Head, Pending with EE Division.";
                    ShowMsgBox(msg);
                    cmbAccHead.SelectedIndex = 0;
                    txtaccHeadDesc.Text = string.Empty;
                    return;
                }
                if (cmbAccHead.SelectedIndex > 0)
                {
                    txtaccHeadDesc.Text = GetAccHeadDes(cmbAccHead.SelectedItem.Text);
                    Arr = objwo.GetWOSeries(cmbAccHead.SelectedItem.Text, cmbDivision.SelectedValue, txtFinancialYear.Text);
                    if ((Arr[1] ?? "").Length > 0)
                    {
                        txtWostRng1.ReadOnly = true;
                        txtWoEdRng1.Text = Arr[0];
                        txtWostRng1.Text = Arr[0];
                        txtWoEdRng1.ReadOnly = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
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
                    Response.Redirect("WorkOrderRangeAllocationView.aspx", false);
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// to get account head description
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public string GetAccHeadDes(string acc)
        {
            string acchead = string.Empty;
            try
            {
                ClsWorkOrderRangeAllocation objwo = new ClsWorkOrderRangeAllocation();
                acchead = objwo.GetAccountHeadDescription(acc);
                return acchead;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return acchead;
            }
        }

        /// <summary>
        /// on text change method for work order start range 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtWostRng1_TextChanged(object sender, EventArgs e)
        {
            if (txtWostRng1.Text.Length > 0)
            {
                txtWoEdRng1.Text = txtWostRng1.Text.ToUpper();
                txtWostRng1.Text = txtWoEdRng1.Text.ToUpper();
            }
            txtQuantity_TextChanged(sender, e);
        }

        /// <summary>
        /// based quantity work order  end range will get auto bind
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            int val = 0;
            string count = string.Empty;
            string Qty = txtQuantity.Text.Replace("0", "");
            string Srange = txtWostRng3.Text.Replace("0", "");

            if (txtQuantity.Text.Length > 0 && txtWostRng3.Text.Length == 4 &&  Convert.ToInt16(txtWostRng3.Text) > 0)
            {
                if (txtQuantity.Text == "0" || Convert.ToInt16(txtQuantity.Text) < 0 ||
                txtQuantity.Text.Length > 4 )
                {
                    ShowMsgBox("Please Enter Valid Quantity");
                    return;
                }
                if(!(Qty.Length >0))
                {
                    ShowMsgBox("Please Enter Valid Quantity");
                    return;
                }
                if(!(Srange.Length > 0))
                {
                    ShowMsgBox("Please Enter Valid 4 Digit Work Order Start Range");
                    return;

                }
                if (txtWostRng3.Text.Length != 4 || Convert.ToInt16(txtWostRng3.Text) == 0)
                {
                    txtWostRng3.Focus();
                    ShowMsgBox("Please Enter Valid 4 Digit Work Order Start Range");
                    return;
                }

                val = Convert.ToInt32(txtWostRng3.Text) + Convert.ToInt32(txtQuantity.Text) - 1;

                count = Convert.ToString(val);
                switch (count.Length)
                {
                    case 1:
                        count = "000" + count;
                        break;
                    case 2:
                        count = "00" + count;
                        break;
                    case 3:
                        count = "0" + count;
                        break;
                }

            }
            else if(txtWostRng3.Text.Length != 4)
            {
                ShowMsgBox("Please Enter Valid 4 Digit Work Order Start Range");
                return;
            }



            txtWoEdRng1.Text = txtWostRng1.Text;
            txtWoEdRng3.Text = count;
        }
        /// <summary>
        /// ro reset the field in the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                cmbAccHead.SelectedIndex = 0;
                txtaccHeadDesc.Text = string.Empty;
                //  txtallocationDate.Text = string.Empty;
                txtQuantity.Text = string.Empty;
                txtWostRng1.Text = string.Empty;
                txtWostRng3.Text = string.Empty;
                txtWoEdRng1.Text = string.Empty;
                txtWoEdRng3.Text = string.Empty;
                cmdSave.Enabled = true;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to validate the entered values in the field beform saving
        /// </summary>
        /// <returns></returns>
        public bool ValidateForm()
        {
            bool bValidate = false;
            try
            {

                if (cmbAccHead.SelectedValue.Length == 0)
                {
                    cmbAccHead.Focus();
                    ShowMsgBox("Please Select Account Head");
                    return bValidate;
                }
                if (txtallocationDate.Text.Length == 0)
                {
                    txtallocationDate.Focus();
                    ShowMsgBox("Please Enter Allocation Date ");
                    return bValidate;
                }
                if (txtQuantity.Text.Length == 0)
                {
                    txtQuantity.Focus();
                    ShowMsgBox("Please Enter Quantity");
                    return bValidate;
                }
                if (txtWostRng3.Text.Length == 0)
                {
                    txtWostRng3.Focus();
                    ShowMsgBox("Please Enter Work Order Start Range");
                    return bValidate;
                }
                if (txtWostRng3.Text.Length != 4)
                {
                    txtWostRng3.Focus();
                    ShowMsgBox("Please Enter Valid 4 Digit Work Order Start Range");
                    return bValidate;
                }
                string Qty = txtQuantity.Text.Replace("0","");
                string Srange = txtWostRng3.Text.Replace("0","");
                if (!((Qty ?? "").Length > 0))
                {
                    ShowMsgBox("Please Enter Valid Quantity");
                    return bValidate;
                }
                if (!((Srange ?? "").Length > 0))
                {
                    ShowMsgBox("Please Enter Valid 4 Digit Work Order Start Range");
                    return bValidate;
                }
                if (txtWoEdRng3.Text.Trim().Length > 4)
                {
                    txtWostRng3.Focus();
                    ShowMsgBox("Work Order End Range Exceeds 9999");
                    return bValidate;
                }
                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return bValidate;
            }
        }
        /// <summary>
        /// to display the popup message
        /// </summary>
        /// <param name="Msg"></param>
        private void ShowMsgBox(string Msg)
        {
            try
            {
                string ShowMsg = string.Empty;
                ShowMsg = "<script language=javascript> alert ('" + Msg + "')</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "Msg", ShowMsg);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to save the work order range allocation details into the table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                string[] Arrmsg = new string[3];
                if (ValidateForm() == true)
                {
                    ClsWorkOrderRangeAllocation Objworangeallocation = new ClsWorkOrderRangeAllocation();

                    Objworangeallocation.Divcode = cmbDivision.SelectedValue;
                    Objworangeallocation.Accounthead = cmbAccHead.SelectedItem.Text;
                    Objworangeallocation.Allocationdate = txtallocationDate.Text;
                    Objworangeallocation.Financialyear = txtFinancialYear.Text;
                    Objworangeallocation.Description = txtaccHeadDesc.Text;
                    Objworangeallocation.Quantity = txtQuantity.Text;
                    Objworangeallocation.Crby = objSession.UserId;
                    Objworangeallocation.offcode = objSession.OfficeCode;
                    Objworangeallocation.Comm_Decomm_flag = Objworangeallocation.getCommDecommStatus(cmbAccHead.SelectedItem.Text);

                    if (txtWostRng1.Text.Length != 0)
                    {
                        Objworangeallocation.Workorderstartrange = txtWostRng1.Text.Trim().Replace("'", "")
                        + txtWostRng2.Text.Trim().Replace("'", "") + txtWostRng3.Text.Trim().Replace("'", "");
                    }
                    else
                    {
                        Objworangeallocation.Workorderstartrange = txtWostRng3.Text.Trim().Replace("'", "");
                    }
                    if (txtWoEdRng1.Text.Length != 0)
                    {
                        Objworangeallocation.Workorderendrange = txtWoEdRng1.Text.Trim().Replace("'", "")
                        + txtWoEdRng2.Text.Trim().Replace("'", "") + txtWoEdRng3.Text.Trim().Replace("'", "");
                    }
                    else
                    {
                        Objworangeallocation.Workorderendrange = txtWoEdRng3.Text.Trim().Replace("'", "");
                    }

                    Objworangeallocation.Startrangenumber = Regex.Replace(Objworangeallocation.Workorderstartrange, "[^0-9.]", "");
                    Objworangeallocation.Endrangenumber = Regex.Replace(Objworangeallocation.Workorderendrange, "[^0-9.]", "");

                    Objworangeallocation.StartrangeAlphabet = Regex.Replace(Objworangeallocation.Workorderstartrange, @"[\d]", string.Empty); ;
                    Objworangeallocation.EndrangeAlphabet = Regex.Replace(Objworangeallocation.Workorderendrange, @"[/]", string.Empty);
                    Objworangeallocation.sboid = ConfigurationManager.AppSettings["WorkOrderRangeAllocation"];
                    Objworangeallocation.sActionType = txtActiontype.Text;
                    Objworangeallocation.sRecordId = hdfrecordid.Value;
                    #region Approve

                    if (txtActiontype.Text == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ActionTypeApprove"]) || txtActiontype.Text == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ActionTypeReject"]))
                    {
                        if (hdfWFDataId.Value != "0")
                        {
                            ApproveRejectAction(Objworangeallocation);

                            if (objSession.sTransactionLog == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["TransactionLog"]))
                            {
                                Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (Workorder) Range Allocation ");
                            }
                            return;
                        }
                    }
                    WorkFlowObjects(Objworangeallocation);

                    #endregion

                    #region Modify and Approve

                    if (txtActiontype.Text == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ActionTypeModifyApprove"]))
                    {
                        if (txtComment.Text.Trim() == "")
                        {
                            ShowMsgBox(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ApproversCommentreq"]));
                            txtComment.Focus();
                            return;

                        }

                        Objworangeallocation.sActionType = txtActiontype.Text;
                        Objworangeallocation.Crby = objSession.UserId;
                        Objworangeallocation.sboid = ConfigurationManager.AppSettings["WorkOrderRangeAllocation"];

                        Arrmsg = Objworangeallocation.SaveWorkorderallocationdetails(Objworangeallocation);
                        if (Arrmsg[1] == "-1")
                        {
                            ShowMsgBox(Arrmsg[0]);
                            return;
                        }
                        ApproveRejectAction(Objworangeallocation);
                        return;

                    }

                    #endregion
                    Arrmsg = Objworangeallocation.SaveWorkorderallocationdetails(Objworangeallocation);

                    if (Arrmsg[1].ToString() == "0")
                    {
                        cmdSave.Enabled = false;
                        ShowMsgBox(Arrmsg[0]);
                        cmdReset_Click(sender, e);
                        return;
                    }
                    if (Arrmsg[1].ToString() == "2")
                    {
                        ShowMsgBox(Arrmsg[0]);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        ///  to get basic details to save workflowobject table
        /// </summary>
        /// <param name="Objworangeallocation"></param>
        public void WorkFlowObjects(ClsWorkOrderRangeAllocation Objworangeallocation)
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
                Objworangeallocation.sClientIP = sClientIP;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to get configeration data from record 
        /// </summary>
        public void WorkFlowConfig()
        {
            try
            {
                if (Request.QueryString["ActionType"] != null && Convert.ToString(Request.QueryString["ActionType"]) != "")
                {
                    if (Session["WFOId"] != null && Convert.ToString(Session["WFOId"]) != "")
                    {
                        hdfWFOId.Value = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Convert.ToString(Session["WFOId"])));
                        hdfWFDataId.Value = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Convert.ToString(Session["WFDataId"])));
                        hdfrecordid.Value = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["Allocateid"]));

                        Session["WFOId"] = null;
                        Session["WFOAutoId"] = null;
                        Session["ApproveStatus"] = null;
                    }
                    if (hdfWFDataId.Value != "0")
                    {
                        GetCRDetailsFromXML(hdfWFDataId.Value);
                    }
                    SetControlText();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to get the controles to the fields
        /// </summary>
        public void SetControlText()
        {
            try
            {
                txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));

                if (txtActiontype.Text == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ActionTypeApprove"]))
                {
                    txtQuantity.ReadOnly = true;
                    cmdSave.Text = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ApproveText"]);
                }
                if (txtActiontype.Text == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ActionTypeReject"]))
                {
                    cmdSave.Text = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["RejectText"]);
                }
                if (txtActiontype.Text == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ActionTypeModifyApprove"]))
                {
                    txtQuantity.ReadOnly = false;
                    cmdSave.Text = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ModifyText"]);
                }
                dvComments.Style.Add("display", "block");
                if (txtActiontype.Text != "")
                {
                    cmbDivision.Enabled = false;
                    cmbAccHead.Enabled = false;
                    txtWostRng1.ReadOnly = true;
                    txtWostRng3.ReadOnly = true;

                    if (txtActiontype.Text == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ActionTypeModifyApprove"]))
                    {
                        txtWostRng3.ReadOnly = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// method to save based on approval stage
        /// </summary>
        /// <param name="Objworangeallocation"></param>
        public void ApproveRejectAction(ClsWorkOrderRangeAllocation Objworangeallocation)
        {
            try
            {
                clsApproval objApproval = new clsApproval();

                bool bResult = false;
                if (txtComment.Text.Trim() == "")
                {
                    ShowMsgBox(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ApproversCommentreq"]));
                    txtComment.Focus();
                    return;
                }
                objApproval.sCrby = objSession.UserId;
                objApproval.sOfficeCode = objSession.OfficeCode;
                objApproval.sApproveComments = txtComment.Text.Trim();
                objApproval.sWFObjectId = hdfWFOId.Value;
                objApproval.sWFAutoId = hdfWFOAutoId.Value;

                if (txtActiontype.Text == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ActionTypeApprove"]))
                {
                    objApproval.sApproveStatus = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ApproveStatusApproved"]);
                }
                if (txtActiontype.Text == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ActionTypeModifyApprove"]))
                {
                    objApproval.sApproveStatus = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ApproveStatusModifyandApprove"]);
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


                objApproval.sDescription = "Workorder Range Allocation from " + Objworangeallocation.Workorderstartrange + "" +
                " to " + Objworangeallocation.Workorderendrange + " For Account Head  " + Objworangeallocation.Accounthead + "";

                if (Objworangeallocation.sActionType == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ActionTypeApprove"]))
                {
                    bResult = objApproval.ApproveWFRequest(objApproval);
                }
                else if (Objworangeallocation.sActionType == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ActionTypeModifyApprove"]))
                {
                    bResult = objApproval.ModifyApproveWFRequest(objApproval);
                }
                if (bResult == true)
                {

                    if (objApproval.sApproveStatus == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ApproveStatusApproved"]))
                    {
                        ShowMsgBox(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SuccessApprove"]));
                        cmdSave.Enabled = false;
                    }

                    else if (objApproval.sApproveStatus == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ApproveStatusModifyandApprove"]))
                    {
                        ShowMsgBox(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SuccessModify"]));
                        cmdSave.Enabled = false;
                    }
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to get data from XML
        /// </summary>
        /// <param name="sWFDataId"></param>
        public void GetCRDetailsFromXML(string sWFDataId)
        {
            try
            {
                ClsWorkOrderRangeAllocation objworkorderrangedetails = new ClsWorkOrderRangeAllocation();
                objworkorderrangedetails.sWFDataId = sWFDataId;

                objworkorderrangedetails.GetworangeDetailsFromXML(objworkorderrangedetails);

                cmbDivision.SelectedItem.Text = objworkorderrangedetails.Divcode;
                cmbAccHead.SelectedItem.Text = objworkorderrangedetails.Accounthead;
                txtallocationDate.Text = objworkorderrangedetails.Allocationdate;
                if (objworkorderrangedetails.StartrangeAlphabet!=null)
                {
                    txtWostRng1.Text = objworkorderrangedetails.StartrangeAlphabet.Trim('-');
                }
                txtWostRng2.Text = "-";
                txtWostRng3.Text = objworkorderrangedetails.Startrangenumber;
                txtFinancialYear.Text = objworkorderrangedetails.Financialyear;
                txtQuantity.Text = objworkorderrangedetails.Quantity;
                if (objworkorderrangedetails.StartrangeAlphabet != null)
                {
                    txtWoEdRng1.Text = objworkorderrangedetails.StartrangeAlphabet.Trim('-');
                }
                txtWoEdRng2.Text = "-";
                txtWoEdRng3.Text = objworkorderrangedetails.Endrangenumber;
                txtaccHeadDesc.Text = objworkorderrangedetails.GetAccountHeadDescription(objworkorderrangedetails.Accounthead);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}