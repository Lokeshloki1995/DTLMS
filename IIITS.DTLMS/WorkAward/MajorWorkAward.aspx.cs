using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL.WorkAward;
using IIITS.DTLMS.BL;
using System.Data;
using System.Collections;

namespace IIITS.DTLMS.WorkAward
{
    public partial class MajorWorkAward : System.Web.UI.Page
    {
        public string strFormCode = "MajorWorkAward";
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

                    Genaral.Load_Combo(" SELECT \"DIV_CODE\",\"DIV_NAME\" from \"TBLDIVISION\" WHERE \"DIV_CICLE_CODE\"='" + objSession.OfficeCode + "'", "--Select--", cmbDiv);


                   

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
                        pnlApproval.Enabled = false;
                    }
                    if (txtActionType.Text == "M")
                    {
                        LoadSearchWindow();
                        cmdSave.Text = "Modify And Approve";
                        // pnlApproval.Enabled = true;
                    }
                    else if (txtActionType.Text == "R")
                    {
                        cmdSave.Text = "Reject";
                        pnlApproval.Enabled = false;
                    }
                    else if (txtActionType.Text == "V")
                    {
                        cmdSave.Text = "View";
                        cmdReset.Enabled = false;
                        pnlApproval.Enabled = false;
                        dvComments.Style.Add("display", "none");
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
                if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")  
                {
                    Response.Redirect("/Approval/ApprovalInbox.aspx", false);
                }
                else
                {
                    Response.Redirect("MajorWorkAwardView.aspx", false);
                }

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
        public void GetDatafromMainTable()
        {
            try
            {
                ClsMajorWorkAward obj = new ClsMajorWorkAward();
                obj.sWOAId = txtWOAId.Text;
                obj.GetWorkAwardobjects(obj);
                txtWOANo.Text = obj.sWOANo;
                txtWOADate.Text = obj.sWOADate;
                Genaral.Load_Combo("SELECT \"TR_ID\", \"TR_NAME\" FROM \"TBLTRANSREPAIRER\", \"TBLTRANSREPAIREROFFCODE\" WHERE \"TR_ID\" = \"TRO_TR_ID\" AND CAST(\"TRO_OFF_CODE\" AS TEXT) LIKE '" + hdfofficecode.Value + "%'", "--Select--", cmbRepairer);
                Genaral.Load_Combo(" SELECT \"DIV_CODE\",\"DIV_NAME\" from \"TBLDIVISION\" WHERE \"DIV_CICLE_CODE\"='" + objSession.OfficeCode + "'", "--Select--", cmbDiv);


                cmbRepairer.SelectedValue = obj.sRepairer;
                cmbDiv.SelectedValue = obj.division; 
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

                        // coded by rudra on 24-03-2020 for session should not close after refresh click
                        //Session["WFDataId"] = null;
                        //Session["WFOId"] = null;
                        //Session["ApproveStatus"] = null;
                        //Session["WFOAutoId"] = null;
                        
                      if(objSession.RoleId=="35")
                        {
                            txtActionType.Text = "M";
                        }
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
                        pnlApproval.Enabled = false;
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
                ClsMajorWorkAward obj = new ClsMajorWorkAward();
                obj.sWFDataId = sWFDataId;
                obj.GetEstimateDetailsFromXML(obj);
                if (txtActionType.Text == "M")
                {
                    billdetails.Visible = true;
                    pnlApproval.Enabled = true;
                   
                }
                txtWONo.Text = obj.sWoNo;
                txtWodate.Text = obj.sWoDate;
                txtWOANo.Text = obj.sWOANo;
                txtWOADate.Text = obj.sWOADate;
                txtWoAmount.Text = Convert.ToString(obj.WoaAmount);
                txtInncured.Text = Convert.ToString(obj.WoaInAmount);
                Genaral.Load_Combo("SELECT \"TR_ID\", \"TR_NAME\" FROM \"TBLTRANSREPAIRER\", \"TBLTRANSREPAIREROFFCODE\" WHERE \"TR_ID\" = \"TRO_TR_ID\" AND CAST(\"TRO_OFF_CODE\" AS TEXT) LIKE '" + hdfofficecode.Value + "%'", "--Select--", cmbRepairer);

                cmbRepairer.SelectedValue = Convert.ToString(obj.sRepairer);
                cmbDiv.SelectedValue = obj.division;
                grdestimation.DataSource = obj.dtWODetails;

                //double total = 0;
                //foreach (DataRow dr in obj.dtWODetails.Rows)
                //{
                //    total += Convert.ToDouble(dr["RWO_AMT"]);
                //}


                //grdestimation.Columns[9].FooterText = total.ToString();

                //grdestimation.Columns[1].FooterText = "Total";
                //grdestimation.Columns[1].FooterStyle.Font.Bold = true;
                //grdestimation.Columns[1].FooterStyle.ForeColor = System.Drawing.Color.Black;
                //grdestimation.Columns[1].FooterStyle.HorizontalAlign = HorizontalAlign.Left;
                //grdestimation.Columns[9].FooterStyle.Font.Bold = true;

                double WOtotal = 0;
                double INtotal = 0;
                foreach (DataRow dr in obj.dtWODetails.Rows)
                {
                    WOtotal += Convert.ToDouble(dr["RWO_AMT"]);
                    INtotal += Convert.ToDouble(dr["RWO_INNC_COST"]);
                }


                grdestimation.Columns[9].FooterText = WOtotal.ToString();
                grdestimation.Columns[10].FooterText = INtotal.ToString();

                grdestimation.Columns[1].FooterText = "Total";
                grdestimation.Columns[1].FooterStyle.Font.Bold = true;
                grdestimation.Columns[1].FooterStyle.ForeColor = System.Drawing.Color.Black;
                grdestimation.Columns[1].FooterStyle.HorizontalAlign = HorizontalAlign.Left;
                grdestimation.Columns[9].FooterStyle.Font.Bold = true;
                grdestimation.Columns[10].FooterStyle.Font.Bold = true;
                grdestimation.DataBind();
                ViewState["WODETAILS"] = obj.dtWODetails;
                if (txtActionType.Text == "V")
                {
                    //cmdSave.Text = "View";
                    pnlApproval.Enabled = false;
                    txtWOANo.Enabled = false;
                    txtWOADate.Enabled = false;
                }

                if (txtActionType.Text == "A")
                {
                    //cmdSave.Text = "View";
                    pnlApproval.Enabled = false;
                    txtWOANo.Enabled = false;
                    txtWOADate.Enabled = false;
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
               // objSession.OfficeCode = objSession.OfficeCode.Substring(0,3);
                Genaral.Load_Combo("SELECT \"TR_ID\", \"TR_NAME\" FROM \"TBLTRANSREPAIRER\", \"TBLTRANSREPAIREROFFCODE\" WHERE \"TR_ID\" = \"TRO_TR_ID\" AND CAST(\"TRO_OFF_CODE\" AS TEXT) LIKE '" + hdfofficecode.Value + "%'", "--Select--", cmbRepairer);

                string strQry = string.Empty;
                strQry = "Title=Search and Select Purchase Order Details&";
                strQry += "Query=SELECT \"RWO_SLNO\",\"RWO_NO\", \"RESTD_NO\" FROM \"TBLREPAIRERWORKORDER\", \"TBLREPAIRERESTIMATIONDETAILS\" WHERE \"RWO_EST_ID\" = \"RESTD_ID\"  AND \"RWO_SLNO\" NOT IN (SELECT \"RWAD_WO_SLNO\" FROM \"TBLREPAIRWORKAWARDDETAILS\") ";
                strQry += "AND CAST({0} AS TEXT) like %{1}% and  cast(\"RWO_OFF_CODE\" as text) like '" + hdfofficecode.Value + "%' order by \"RWO_SLNO\" &";
                strQry += "DBColName=\"RWO_SLNO\"~\"RWO_NO\"~\"RESTD_NO\"&";
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
                ClsMajorWorkAward obj = new ClsMajorWorkAward();
                obj.sWoId = sWOID;
                obj.GetWoDetails(obj);
                txtWOId.Text = obj.sWoId;
                txtWONo.Text = obj.sWoNo;
                txtWodate.Text = obj.sWoDate;
                txtWoAmount.Text = obj.sWoAmount;
                txtInncured.Text = obj.sInnCost;
                hdfTcCode.Value = obj.sTCode;
                cmbRepairer.SelectedValue = obj.sRepairer;
            }
            catch (Exception ex)
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
            txtInncured.Text= string.Empty;
           // cmbRepairer.ClearSelection();
        }
        // changed Rudra on 05-03-2020 /  for Adding Inccored cost in workAward
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
                ClsMajorWorkAward obj = new ClsMajorWorkAward();
                obj.sWoId = sWoId;
                obj.GetEstimationDetails(obj);
                DataTable dt = new DataTable();
                dt = obj.dtEstDetails;
                bool bDuplicate = false;

                // new code for checking workorder before add 
                bDuplicate = obj.CheckAlreadyCreated(hdfTcCode.Value);
                if(bDuplicate== false)
                {
                    ShowMsgBox("Selected WorkOrder Already Created, it is Pending for Approval");
                    Reset();
                    return;
                }
          

                DataTable FinalDt = new DataTable();
                if (ViewState["WODETAILS"] != null)
                {
                    DataTable dtWoDetails = (DataTable)ViewState["WODETAILS"];

                    for (int i = 0; i < dtWoDetails.Rows.Count; i++)
                    {
                        if (txtWONo.Text == Convert.ToString(dtWoDetails.Rows[i]["RWO_NO"]))
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

                double WOtotal = 0;
                double INtotal = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    WOtotal += Convert.ToDouble(dr["RWO_AMT"]);
                    INtotal += Convert.ToDouble(dr["RWO_INNC_COST"]);
                }


                grdestimation.Columns[9].FooterText = WOtotal.ToString();
                grdestimation.Columns[10].FooterText = INtotal.ToString();

                grdestimation.Columns[1].FooterText = "Total";
                grdestimation.Columns[1].FooterStyle.Font.Bold = true;
                grdestimation.Columns[1].FooterStyle.ForeColor = System.Drawing.Color.Black;
                grdestimation.Columns[1].FooterStyle.HorizontalAlign = HorizontalAlign.Left;
                grdestimation.Columns[9].FooterStyle.Font.Bold = true;
                grdestimation.Columns[10].FooterStyle.Font.Bold = true;

                grdestimation.DataBind();
                Reset();
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
                grdestimation.DataSource = null;
                grdestimation.DataBind();
                cmbRepairer.SelectedIndex = 0;
                txtWOANo.Text = "";
                txtWOADate.Text = "";
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
                if (!ValidateForm())
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
                            Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "(MajorWorkAward)");
                        }
                        return;
                    }
                }

                ClsMajorWorkAward obj = new ClsMajorWorkAward();
                obj.sWOANo = txtWOANo.Text.Trim().ToUpper();
                obj.sWOADate = txtWOADate.Text.Trim().ToUpper();
                obj.sRepairer = cmbRepairer.SelectedValue;
                obj.sOfficeCode = hdfofficecode.Value;
                obj.sUserId = objSession.UserId;
                obj.dtWODetails = (DataTable)ViewState["WODETAILS"];
                obj.division = cmbDiv.SelectedValue;

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
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "(MajorWorkAward)");
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


                Arr = obj.SaveWorkAwardDetails(obj);
                if (objSession.sTransactionLog == "1")
                {
                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "(MajorWorkAward)");
                }
                ShowMsgBox(Arr[0]);
                cmdSave.Enabled = false;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void WorkFlowObjects(ClsMajorWorkAward objAwrd)
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


                objAwrd.strFormCode = "MajorWorkAward";
                objAwrd.sOfficeCode = hdfofficecode.Value;
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
                    pnlApproval.Enabled = false;
                }
                //Reject
                if (txtActionType.Text == "R")
                {
                    objApproval.sApproveStatus = "3";
                    pnlApproval.Enabled = false;
                }
                //Abort
                if (txtActionType.Text == "D")
                {
                    objApproval.sApproveStatus = "4";
                    pnlApproval.Enabled = false;
                }
                //Modify and Approve
                if (txtActionType.Text == "M")
                {
                    objApproval.sApproveStatus = "2";
                    pnlApproval.Enabled = true;
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

                if (hdfWFDataId.Value == "0")
                {
                    if (cmbDiv.SelectedIndex == 0)
                    {
                        ShowMsgBox("Please Select the Division");
                        cmbDiv.Focus();
                        return Verify;
                    }
                }

                if (txtWOANo.Text == string.Empty)
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

                if (grdestimation.Rows.Count == 0)
                {
                    ShowMsgBox("Please Select Alleast One Work Orderr");
                    return Verify;
                }


                Verify = true;
                return Verify;
            }
            catch (Exception ex)
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
            catch (Exception ex)
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
                        if (lblId.Text == Convert.ToString(dt.Rows[i]["RWO_SLNO"]))
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
                        total += Convert.ToDouble(dr["RWO_AMT"]);
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
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void GenerateIndentReport()
        {
            try
            {
                ClsMajorWorkAward objOffName = new ClsMajorWorkAward();
                if (!txtWOAId.Text.Contains("-") && txtWOAId.Text.Length > 0)
                {
                    string strParam = string.Empty;
                    string sLevelOfApproval = string.Empty;
                    ArrayList sNameList = new ArrayList();
                    string soffCode = objSession.OfficeCode;
                    string sOffcName = objSession.OfficeCode;

                    if (objSession.OfficeCode.Length > 3)
                    {
                        sOffcName = objOffName.getofficeName(objSession.OfficeCode);
                    }
                    else
                    {
                        sOffcName = objSession.OfficeName;
                    }

                    sLevelOfApproval = Convert.ToString(getApprovalLevel());

                    sNameList = objOffName.getCreatedByUserName(soffCode);

                    Session["UserNameList"] = sNameList;
                    strParam = "id=WorkAwardReportmajor&WorkAwardId=" + txtWOAId.Text + "&LApprovel=" + sLevelOfApproval + "&OffCode=" + sOffcName;    // +"&WorkAwardId=" + sWAId
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public int getApprovalLevel()
        {

            int Level = 0;
            try
            {
                if (objSession.RoleId == "31")
                {
                    Level = 1;
                }
                else if (objSession.RoleId == "35")
                {
                    Level = 2;
                }
                else
                    Level = 3;

                return Level;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Level;
            }

        }
        #region
        //public void GenerateIndentReport()
        //{
        //    try
        //    {
        //        if (!txtWOAId.Text.Contains("-") && txtWOAId.Text.Length > 0)
        //        {
        //            string strParam = string.Empty;
        //            strParam = "id=WorkAwardReportmajor&WorkAwardId=" + txtWOAId.Text;
        //            RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}
        #endregion
        protected void cmdReport_Click(object sender, EventArgs e)
        {
            GenerateIndentReport();
        }

        protected void cmbDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdfofficecode.Value = cmbDiv.SelectedValue;
            cmbDiv.Enabled = false;
            billdetails.Visible = true;
            LoadSearchWindow();

        }
    }
}