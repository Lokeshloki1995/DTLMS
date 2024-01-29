using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;

namespace IIITS.DTLMS.WorkAward
{
    public partial class WorkAward : System.Web.UI.Page
    {
        public string strFormCode = "WorkAward";
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
                txtWOADate.Attributes.Add("readonly", "readonly");
                WOACalender.EndDate = System.DateTime.Now;
                if (!IsPostBack)
                {
                    txtWOADate.Attributes.Add("readonly", "readonly");
                    WOACalender.EndDate = System.DateTime.Now;
                    LoadSearchWindow();

                    if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
                    {
                        txtActionType.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));
                    }
                    if (Request.QueryString["WOId"] != null && Request.QueryString["WOId"].ToString() != "")
                    {
                        txtWOId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["WOId"]));
                        
                    }

                    if (txtActionType.Text == "V")
                    {
                        cmdSave.Text = "View";
                        cmdSave.Enabled = false;
                    }
                    if (Request.QueryString["WAId"] != null && Request.QueryString["WAId"].ToString() != "")
                    {                       
                        txtWOAId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["WAId"]));
                        txtActionType.Text = "V";  
                        cmdSave.Enabled = false;
                        cmdReset.Enabled = false;
                        cmdAdd.Visible = false;
                        cmdSearch.Visible = false;
                        cmdReport.Visible = true;
                        GetDatafromMainTable();
                    }
                    else
                    {
                        WorkFlowConfig();
                    }
                }
                if (txtActionType.Text == "M")
                {
                    cmdSave.Text = "Modify And Approve";
                }
                else if (txtActionType.Text == "R")
                {
                    cmdSave.Text = "Reject";
                }
                else if (txtActionType.Text == "V")
                {
                    cmdSave.Text = "View";
                    cmdReset.Enabled = false;
                    dvComments.Style.Add("display", "none");
                }
            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetDatafromMainTable()
        {
            try
            {
                clsWorkAward obj = new clsWorkAward();
                obj.sWOAId = txtWOAId.Text;
                obj.GetWorkAwardobjects(obj);
                txtWOANo.Text = obj.sWOANo;
                txtWOADate.Text = obj.sWOADate;
                cmbRepairer.SelectedValue = obj.sRepairer;               
                grdestimation.DataSource = obj.dtEstDetails;
                grdestimation.DataBind();
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
                        hdfApproveStatus.Value = Convert.ToString(Session["ApproveStatus"]);
                        hdfWFOAutoId.Value = Convert.ToString(Session["WFOAutoId"]);

                        Session["WFDataId"] = null;
                        Session["WFOId"] = null;
                        Session["ApproveStatus"] = null;
                        Session["WFOAutoId"] = null;
                    }

                    if (hdfWFDataId.Value != "0")
                    {
                        GetDataFromXML(hdfWFDataId.Value);                      
                    }

                    dvComments.Style.Add("display", "block");

                    if (hdfWFOAutoId.Value != "0")
                    {
                        dvComments.Style.Add("display", "none");
                    }
                    if (txtActionType.Text == "V")
                    {
                        cmdSave.Text = "View";
                        cmdReset.Enabled = false;
                        dvComments.Style.Add("display", "none");
                    }
                }
                else
                {
                    if (cmdSave.Text != "Save" && cmdSave.Text != "Submit" && cmdSave.Text != "Approve" && cmdSave.Text != "View")
                    {
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


        public void GetDataFromXML(string sWFDataId)
        {
            try
            {
                clsWorkAward obj = new clsWorkAward();
                obj.sWFDataId = sWFDataId;
                obj.GetEstimateDetailsFromXML(obj);
                txtWOANo.Text = obj.sWOANo;
                txtWOADate.Text = obj.sWOADate;
                txtWoAmount.Text = Convert.ToString(obj.WoaAmount);
                cmbRepairer.SelectedValue = obj.sRepairer;
                grdestimation.DataSource = obj.dtWODetails;
                grdestimation.DataBind();
                ViewState["WODETAILS"] = obj.dtWODetails;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void lnkBudgetstat_Click(object sender, EventArgs e)
        {
            try
            {

                string url = "/MasterForms/BudgetStatus.aspx";
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);

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
                Genaral.Load_Combo("SELECT \"TR_ID\", \"TR_NAME\" FROM \"TBLTRANSREPAIRER\", \"TBLTRANSREPAIREROFFCODE\" WHERE \"TR_ID\" = \"TRO_TR_ID\" AND CAST(\"TRO_OFF_CODE\" AS TEXT) LIKE '" + objSession.OfficeCode + "%'","--Select--", cmbRepairer);

                string strQry = string.Empty;
                strQry = "Title=Search and Select Purchase Order Details&";
                strQry += "Query=SELECT \"WO_SLNO\",\"WO_NO\", \"EST_NO\" FROM \"TBLWORKORDER\", \"TBLESTIMATIONDETAILS\", \"TBLDTCFAILURE\" WHERE \"WO_DF_ID\" = \"DF_ID\" AND \"DF_ID\" = \"EST_FAILUREID\" AND \"WO_SLNO\" NOT IN (SELECT \"WAD_WO_SLNO\" FROM \"TBLWORKAWARDDETAILS\") ";
                strQry += "AND CAST({0} AS TEXT) like %{1}% and  cast(\"WO_OFF_CODE\" as text) like '" + objSession.OfficeCode + "%' order by \"WO_SLNO\" &";
                strQry += "DBColName=\"WO_SLNO\"~\"WO_NO\"~\"EST_NO\"&";
                strQry += "ColDisplayName=Wo ID~WO NO~EST No&";
                strQry = strQry.Replace("'", @"\'");
                cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtWONo.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + txtWONo.ClientID + ")");
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
                string sWOID = txtWONo.Text;
                clsWorkAward obj = new clsWorkAward();
                obj.sWoId = sWOID;
                obj.GetWoDetails(obj);
                txtWOId.Text = obj.sWoId;
                txtWONo.Text = obj.sWoNo;
                txtWodate.Text = obj.sWoDate;
                txtWoAmount.Text = obj.sWoAmount;
            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void Reset()
        {
            txtWOId.Text = string.Empty;
            txtWONo.Text = string.Empty;
            txtWodate.Text = string.Empty;
            txtWoAmount.Text = string.Empty;
        }

        protected void cmdAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtWONo.Text.Trim() == string.Empty)
                {
                    txtWONo.Focus();
                    ShowMsgBox("Please Select Work Order");
                    return;
                }            

                string sWoId = txtWOId.Text;
                clsWorkAward obj = new clsWorkAward();
                obj.sWoId = sWoId;
                obj.GetEstimationDetails(obj);
                DataTable dt = new DataTable();
                dt = obj.dtEstDetails;
                DataTable FinalDt = new DataTable();
                if(ViewState["WODETAILS"] != null)
                {
                    DataTable dtWoDetails = (DataTable)ViewState["WODETAILS"];

                    for (int i = 0; i < dtWoDetails.Rows.Count; i++)
                    {
                        if (txtWONo.Text == Convert.ToString(dtWoDetails.Rows[i]["WO_NO"]))
                        {
                            ShowMsgBox("Selected WorkOrder Already Added");
                            return;
                        }
                    }

                    FinalDt = (DataTable)ViewState["WODETAILS"];
                    dt.Merge(FinalDt);
                }
                ViewState["WODETAILS"] = dt;
                grdestimation.DataSource = dt;

                double total = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    total += Convert.ToDouble(dr["WO_AMT"]);
                }

                
                grdestimation.Columns[9].FooterText = total.ToString();

                grdestimation.Columns[1].FooterText = "Total";
                grdestimation.Columns[1].FooterStyle.Font.Bold = true;
                grdestimation.Columns[1].FooterStyle.ForeColor = System.Drawing.Color.Black;
                grdestimation.Columns[1].FooterStyle.HorizontalAlign = HorizontalAlign.Left;
                grdestimation.Columns[9].FooterStyle.Font.Bold = true;
               // grdestimation.Columns[2].FooterText = total.ToString();


                grdestimation.DataBind();
                Reset();
            }
            catch(Exception ex)
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
                grdestimation.DataSource = null;
                grdestimation.DataBind();
                cmbRepairer.SelectedIndex = 0;
                txtWOANo.Text = "";
                txtWOADate.Text = "";
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
                if(!ValidateForm())
                {
                    return;
                }

                if (txtActionType.Text == "A" || txtActionType.Text == "R" || txtActionType.Text == "D")
                {
                    if (hdfWFDataId.Value != "0")
                    {
                        ApproveRejectAction();
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "(WorkAward)PermanentDecommissioning");
                        }
                        return;
                    }
                }

                clsWorkAward obj = new clsWorkAward();
                obj.sWOANo = txtWOANo.Text.Trim().ToUpper();
                obj.sWOADate = txtWOADate.Text.Trim().ToUpper();
                obj.sRepairer = cmbRepairer.SelectedValue;
                obj.sOfficeCode = objSession.OfficeCode;
                obj.sUserId = objSession.UserId;
                obj.dtWODetails = (DataTable)ViewState["WODETAILS"];
                WorkFlowObjects(obj);
                string[] Arr = new string[3];

                #region Modify and Approve

                // For Modify and Approve
                if (txtActionType.Text == "M")
                {
                    if (txtComment.Text.Trim() == "")
                    {
                        ShowMsgBox("Enter Comments/Remarks");
                        txtComment.Focus();
                        return;

                    }
                    obj.sActionType = txtActionType.Text;                    

                    Arr = obj.SaveWorkAwardDetails(obj);
                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "(WorkAward)PermanentDecommissioning");
                    }
                    if (Arr[1].ToString() == "0")
                    {
                        hdfWFDataId.Value = obj.sWFDataId;
                        ApproveRejectAction();
                        return;
                    }
                    if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0]);
                        return;
                    }
                }
                #endregion

                
                Arr =  obj.SaveWorkAwardDetails(obj);
                if (objSession.sTransactionLog == "1")
                {
                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "(WorkAward)PermanentDecommissioning");
                }
                ShowMsgBox(Arr[0]);
                cmdSave.Enabled = false;

            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void WorkFlowObjects(clsWorkAward objAwrd)
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


                objAwrd.strFormCode = "WorkAward";
                objAwrd.sOfficeCode = objSession.OfficeCode;
                objAwrd.sClientIp = sClientIP;
                objAwrd.sWFO_id = hdfWFOId.Value;
                objAwrd.sWFAutoId = hdfWFOAutoId.Value;

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
                if (txtActionType.Text == "A")
                {
                    objApproval.sApproveStatus = "1";
                }
                //Reject
                if (txtActionType.Text == "R")
                {
                    objApproval.sApproveStatus = "3";
                }
                //Abort
                if (txtActionType.Text == "D")
                {
                    objApproval.sApproveStatus = "4";
                }
                //Modify and Approve
                if (txtActionType.Text == "M")
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
                if (txtActionType.Text == "M")
                {
                    objApproval.sWFDataId = hdfWFDataId.Value;
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
                        txtWOAId.Text = objApproval.sNewRecordId;
                        GenerateIndentReport();
                        cmdSave.Enabled = false;
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

        public bool ValidateForm()
        {
            bool Verify = false;
            try
            {
                if(txtWOANo.Text == string.Empty)
                {
                    ShowMsgBox("Please Enter the Work Award No");
                    txtWOANo.Focus();
                    return Verify;
                }

                if (txtWOADate.Text == string.Empty)
                {
                    ShowMsgBox("Please Select the Work Award Date");
                    txtWOADate.Focus();
                    return Verify;
                }

                if (cmbRepairer.SelectedIndex == 0)
                {
                    ShowMsgBox("Please Select the Repairer");
                    cmbRepairer.Focus();
                    return Verify;
                }

                if(grdestimation.Rows.Count == 0)
                {
                    ShowMsgBox("Please Select Alleast One Work Order");
                    return Verify;
                }


                Verify = true;
                return Verify;
            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Verify;
            }
        }

        protected void grdestimation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdestimation.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["WODETAILS"];
                grdestimation.DataSource = SortDataTable(dt as DataTable, true);
                grdestimation.DataBind();
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
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
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        ViewState["WODETAILS"] = dataView.ToTable();
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["WODETAILS"] = dataView.ToTable();
                    }
                }
                return dataView;
            }
            else
            {
                return new DataView();
            }
        }

        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "ASC"; }
            set { ViewState["SortDirection"] = value; }


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

        protected void grdestimation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Remove")
                {
                    DataTable dt = (DataTable)ViewState["WODETAILS"];
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    Label lblId = (Label)row.FindControl("lblWoId");

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //to remove selected Capacity from grid
                        if (lblId.Text == Convert.ToString(dt.Rows[i]["WO_SLNO"]))
                        {
                            dt.Rows[i].Delete();
                            dt.AcceptChanges();
                        }
                    }

                    dt.AcceptChanges();

                    if (dt.Rows.Count > 0)
                    {                       
                        ViewState["WODETAILS"] = dt;
                    }
                    else
                    {
                        ViewState["WODETAILS"] = null;
                    }
                    grdestimation.DataSource = dt;


                    double total = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        total += Convert.ToDouble(dr["WO_AMT"]);
                    }


                    grdestimation.Columns[9].FooterText = total.ToString();

                    grdestimation.Columns[1].FooterText = "Total";
                    grdestimation.Columns[1].FooterStyle.Font.Bold = true;
                    grdestimation.Columns[1].FooterStyle.ForeColor = System.Drawing.Color.Black;
                    grdestimation.Columns[1].FooterStyle.HorizontalAlign = HorizontalAlign.Left;
                    grdestimation.Columns[9].FooterStyle.Font.Bold = true;

                    grdestimation.DataBind();
                }
                else
                {
                    DataTable dt = (DataTable)ViewState["WODETAILS"];
                    grdestimation.DataSource = dt;
                    grdestimation.DataBind();
                }
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GenerateIndentReport()
        {
            try
            {    
                if(!txtWOAId.Text.Contains("-") && txtWOAId.Text.Length > 0)
                {
                    string strParam = string.Empty;
                    strParam = "id=WorkAwardReport&WorkAwardId=" + txtWOAId.Text;
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                }                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdReport_Click(object sender, EventArgs e)
        {
            GenerateIndentReport();
        }
    }
}