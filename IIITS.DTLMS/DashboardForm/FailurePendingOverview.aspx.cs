using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace IIITS.DTLMS.DashboardForm
{

    public partial class FailurePendingOverview : System.Web.UI.Page
    {
        string strFormCode = "FailurePendingOverview";
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
                    if (!IsPostBack)
                    {
                        if (Request.QueryString["OfficeCode"] != null && Convert.ToString(Request.QueryString["OfficeCode"]) != "")
                        {
                            hdfOffCode.Value = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["OfficeCode"]));
                        }
                        else
                        {
                            hdfOffCode.Value = objSession.OfficeCode;
                        }

                        LoadFailurePendingDetails();
                    }
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadFailurePendingDetails()
        {
            try
            {
                clsDashboard objDashboard = new clsDashboard();
                DataTable dtLoadDetails = new DataTable();
                if (Request.QueryString["value"] == "Failure")
                {
                    dtLoadDetails = objDashboard.LoadFailurePendingDetails(hdfOffCode.Value);
                    grdFailurePending.Visible = true;
                    grdFailurePending.DataSource = dtLoadDetails;
                    grdFailurePending.DataBind();
                    ViewState["FailurePending"] = dtLoadDetails;
                    failure.Text = "Failure Pending Details";
                    failureText.Text = "Failure Pending Details";
                }

                if (Request.QueryString["value"] == "FailurePendingApproval")
                {
                    dtLoadDetails = objDashboard.LoadFailureApprovalPendingDetails(hdfOffCode.Value);
                    grdFailureApprovalPending.Visible = true;
                    grdFailureApprovalPending.DataSource = dtLoadDetails;
                    grdFailureApprovalPending.DataBind();
                    ViewState["FailurePendingApproval"] = dtLoadDetails;
                    failure.Text = "Failure Approval Pending Details";
                    failureText.Text = "Failure Approval Pending Details";
                }

                if (Request.QueryString["value"] == "estimation")
                {
                    dtLoadDetails = objDashboard.LoadEstimationPendingDetails(hdfOffCode.Value);
                    grdEstimationPending.Visible = true;
                    grdEstimationPending.DataSource = dtLoadDetails;
                    grdEstimationPending.DataBind();
                    ViewState["estimation"] = dtLoadDetails;
                    failureText.Text = "Estimation Pending Details";
                    failure.Text = "Estimation Pending Details";
                }

                if (Request.QueryString["value"] == "workorder")
                {
                    dtLoadDetails = objDashboard.LoadWorkorderPendingDetails(hdfOffCode.Value);
                    grdWorkorderPending.Visible = true;
                    grdWorkorderPending.DataSource = dtLoadDetails;
                    grdWorkorderPending.DataBind();
                    ViewState["workorder"] = dtLoadDetails;
                    failureText.Text = "Workorder Pending Details";
                    failure.Text = "Workorder Pending Details";
                }

                //if (Request.QueryString["value"] == "indent")
                //{
                //    dtLoadDetails = objDashboard.LoadIndentPendingDetails(hdfOffCode.Value);
                //    grdIndentPending.Visible = true;
                //    grdIndentPending.DataSource = dtLoadDetails;
                //    grdIndentPending.DataBind();
                //    ViewState["indent"] = dtLoadDetails;
                //    failureText.Text = "Indent Pending Details";
                //    failure.Text = "Indent Pending Details";
                //}

                if (Request.QueryString["value"] == "invoice")
                {
                    dtLoadDetails = objDashboard.LoadInvoicePendingDetails(hdfOffCode.Value);
                    grdinvoicePending.Visible = true;
                    grdinvoicePending.DataSource = dtLoadDetails;
                    grdinvoicePending.DataBind();
                    ViewState["invoice"] = dtLoadDetails;
                    failureText.Text = "Invoice Pending Details";
                    failure.Text = "Invoice Pending Details";
                }

                //if (Request.QueryString["value"] == "DeCommission")
                //{
                //    dtLoadDetails = objDashboard.LoadDeCommissionPendingDetails(hdfOffCode.Value);
                //    grdDecommissionPending.Visible = true;
                //    grdDecommissionPending.DataSource = dtLoadDetails;
                //    grdDecommissionPending.DataBind();
                //    ViewState["DeCommission"] = dtLoadDetails;
                //    failureText.Text = "DeCommission Pending Details";
                //    failure.Text = "DeCommission Pending Details";
                //}

                if (Request.QueryString["value"] == "RI")
                {
                    dtLoadDetails = objDashboard.LoadRIPendingDetails(hdfOffCode.Value);
                    grdRIPending.Visible = true;
                    grdRIPending.DataSource = dtLoadDetails;
                    grdRIPending.DataBind();
                    ViewState["RI"] = dtLoadDetails;
                    failureText.Text = "RI Pending Details";
                    failure.Text = "RI Pending Details";
                }


                if (Request.QueryString["value"] == "CR")
                {
                    dtLoadDetails = objDashboard.LoadCRPendingDetails(hdfOffCode.Value);
                    grdCRPending.Visible = true;
                    grdCRPending.DataSource = dtLoadDetails;
                    grdCRPending.DataBind();
                    ViewState["CR"] = dtLoadDetails;
                    failureText.Text = "CR Pending Details";
                    failure.Text = "CR Pending Details";
                }

                if (Request.QueryString["value"] == "InvoiceTCDetails")
                {

                    string WOSLNO = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["WOSLNO"]));
                    dtLoadDetails = objDashboard.GetStore_TcDetails(hdfOffCode.Value, WOSLNO, objSession.sRoleType);
                    grdInvoiceTCDetails.Visible = true;
                    grdInvoiceTCDetails.DataSource = dtLoadDetails;
                    grdInvoiceTCDetails.DataBind();
                    ViewState["TCdetails"] = dtLoadDetails;
                    failureText.Text = "Transformer Available in Store";
                    failure.Text = "Transformer Available in Store";
                }
                if (Request.QueryString["value"] == "Singleworkorder")
                {
                    dtLoadDetails = objDashboard.LoadSingleWorkorderPendingDetails(hdfOffCode.Value);
                    grdWorkorderPending.Visible = true;
                    grdWorkorderPending.DataSource = dtLoadDetails;
                    grdWorkorderPending.DataBind();
                    ViewState["Singleworkorder"] = dtLoadDetails;
                    failureText.Text = "Minor Workorder Pending Details";
                    failure.Text = "Minor Workorder Pending Details";
                }

                if (Request.QueryString["value"] == "ReceiveTC")
                {
                    dtLoadDetails = objDashboard.LoadReceiveTCPendingDetails(hdfOffCode.Value);
                    grdReceiveTC.Visible = true;
                    grdReceiveTC.DataSource = dtLoadDetails;
                    grdReceiveTC.DataBind();
                    ViewState["ReceiveTC"] = dtLoadDetails;
                    failureText.Text = "ReceiveTC Pending Details";
                    failure.Text = "ReceiveTC Pending Details";
                }

                if (Request.QueryString["value"] == "SingleComission")
                {
                    dtLoadDetails = objDashboard.LoadSingleComissionPendingDetails(hdfOffCode.Value);
                    grdSingleComission.Visible = true;
                    grdSingleComission.DataSource = dtLoadDetails;
                    grdSingleComission.DataBind();
                    ViewState["SingleComission"] = dtLoadDetails;
                    failureText.Text = "Minor Comission Pending Details";
                    failure.Text = "Minor Comission Pending Details";
                }
                if (Request.QueryString["value"] == "Condition")
                {
                    dtLoadDetails = objDashboard.LoadConditionOfTCDetails(hdfOffCode.Value);
                    grdConditionOfTC.Visible = true;
                    grdConditionOfTC.DataSource = dtLoadDetails;
                    grdConditionOfTC.DataBind();
                    ViewState["Condition"] = dtLoadDetails;
                    failureText.Text = "Condition Of Transformer Centre Details";
                    failure.Text = "Condition Of Transformer Centre Details";
                }
                if (Request.QueryString["value"] == "NewTCcount")
                {
                    dtLoadDetails = objDashboard.LoadConditionOfNewTCDetails(hdfOffCode.Value);
                    grdConditionOfTC.Visible = true;
                    grdConditionOfTC.DataSource = dtLoadDetails;
                    grdConditionOfTC.DataBind();
                    ViewState["NewTCcount"] = dtLoadDetails;
                    failureText.Text = "Condition Of New Transformer Details";
                    failure.Text = "Condition Of New Transformer Details";
                }
                if (Request.QueryString["value"] == "ReleaseTCcount")
                {
                    dtLoadDetails = objDashboard.LoadConditionOfREGoodTCDetails(hdfOffCode.Value);
                    grdConditionOfTC.Visible = true;
                    grdConditionOfTC.DataSource = dtLoadDetails;
                    grdConditionOfTC.DataBind();
                    ViewState["ReleaseTCcount"] = dtLoadDetails;
                    failureText.Text = "Condition Of Release Transformer Details";
                    failure.Text = "Condition Of Release Transformer Details";
                }
                if (Request.QueryString["value"] == "RepairTCcount")
                {
                    dtLoadDetails = objDashboard.LoadConditionOfRPGoodTCDetails(hdfOffCode.Value);
                    grdConditionOfTC.Visible = true;
                    grdConditionOfTC.DataSource = dtLoadDetails;
                    grdConditionOfTC.DataBind();
                    ViewState["RepairTCcount"] = dtLoadDetails;
                    failureText.Text = "Condition Of Repair Transformer Details";
                    failure.Text = "Condition Of Repair Transformer Details";
                }
                if (Request.QueryString["value"] == "FaultyTCcount")
                {
                    dtLoadDetails = objDashboard.LoadConditionOfFaultyTCDetails(hdfOffCode.Value);
                    grdConditionOfTC.Visible = true;
                    grdConditionOfTC.DataSource = dtLoadDetails;
                    grdConditionOfTC.DataBind();
                    ViewState["FaultyTCcount"] = dtLoadDetails;
                    failureText.Text = "Condition Of Failure Transformer Details";
                    failure.Text = "Condition Of Failure Transformer Details";
                }
                if (Request.QueryString["value"] == "mobileTCcount")
                {
                    dtLoadDetails = objDashboard.LoadConditionOfMobileTCDetails(hdfOffCode.Value);
                    grdConditionOfTC.Visible = true;
                    grdConditionOfTC.DataSource = dtLoadDetails;
                    grdConditionOfTC.DataBind();
                    ViewState["mobileTCcount"] = dtLoadDetails;
                    failureText.Text = "Condition Of Scrap Transformer Details";
                    failure.Text = "Condition Of Scrap Transformer Details";
                }
                if (Request.QueryString["value"] == "TCless25_count")
                {
                    dtLoadDetails = objDashboard.Loadless25CapacityTCDetails(hdfOffCode.Value);
                    grdTCCapacityWise.Visible = true;
                    grdTCCapacityWise.DataSource = dtLoadDetails;
                    grdTCCapacityWise.DataBind();
                    ViewState["TCless25_count"] = dtLoadDetails;
                    failureText.Text = "Details of capacity less than 25";
                    failure.Text = "Details of capacity less than 25";
                }
                if (Request.QueryString["value"] == "TC25_100_count")
                {
                    dtLoadDetails = objDashboard.Loadbtw25_100CapacityTCDetails(hdfOffCode.Value);
                    grdTCCapacityWise.Visible = true;
                    grdTCCapacityWise.DataSource = dtLoadDetails;
                    grdTCCapacityWise.DataBind();
                    ViewState["TC25_100_count"] = dtLoadDetails;
                    failureText.Text = "Details of capacity between 25 and 100";
                    failure.Text = "Details of capacity between 25 and 100";
                }
                if (Request.QueryString["value"] == "TC125_250_count")
                {
                    dtLoadDetails = objDashboard.Loadbtw125_250CapacityTCDetails(hdfOffCode.Value);
                    grdTCCapacityWise.Visible = true;
                    grdTCCapacityWise.DataSource = dtLoadDetails;
                    grdTCCapacityWise.DataBind();
                    ViewState["TC125_250_count"] = dtLoadDetails;
                    failureText.Text = "Details of capacity between 125 and 250";
                    failure.Text = "Details of capacity between 125 and 250";
                }
                if (Request.QueryString["value"] == "TCgreater250_count")
                {
                    dtLoadDetails = objDashboard.Loadgreater250CapacityTCDetails(hdfOffCode.Value);
                    grdTCCapacityWise.Visible = true;
                    grdTCCapacityWise.DataSource = dtLoadDetails;
                    grdTCCapacityWise.DataBind();
                    ViewState["TCgreater250_count"] = dtLoadDetails;
                    failureText.Text = "Details of capacity greater than 250";
                    failure.Text = "Details of capacity greater than 250";
                }
                if (Request.QueryString["value"] == "TCpending_issue_count")
                {
                    dtLoadDetails = objDashboard.LoadTCpending_issue_countDetails(hdfOffCode.Value);
                    grdTCPendingDetails.Visible = true;
                    grdTCPendingDetails.DataSource = dtLoadDetails;
                    grdTCPendingDetails.DataBind();
                    ViewState["TCpending_issue_count"] = dtLoadDetails;
                    failureText.Text = "Details of TC Pending for Issue ";
                    failure.Text = "Details of TC Pending for Issue";
                }

                if (Request.QueryString["value"] == "TCpending_repair_count")
                {
                    dtLoadDetails = objDashboard.LoadTCpending_repair_countDetails(hdfOffCode.Value);
                    grdTCPendingDetails.Visible = true;
                    grdTCPendingDetails.DataSource = dtLoadDetails;
                    grdTCPendingDetails.DataBind();
                    ViewState["TCpending_repair_count"] = dtLoadDetails;
                    failureText.Text = "Details of TC Pending for Repair ";
                    failure.Text = "Details of TC Pending for Repair";
                }
                if (Request.QueryString["value"] == "TCpending_release_count")
                {
                    dtLoadDetails = objDashboard.LoadTCpending_release_countDetails(hdfOffCode.Value);
                    grdTCPendingDetails.Visible = true;
                    grdTCPendingDetails.DataSource = dtLoadDetails;
                    grdTCPendingDetails.DataBind();
                    ViewState["TCpending_release_count"] = dtLoadDetails;
                    failureText.Text = "Details of TC Pending for Release ";
                    failure.Text = "Details of TC Pending for Release";
                }
                if (Request.QueryString["value"] == "TotalDTRDetails")
                {
                    LoadDtrDetails(hdfOffCode.Value, "0");
                    failureText.Text = "Total DTR Details";
                    failure.Text = "Total DTR Details";
                }
                if (Request.QueryString["value"] == "TotalFieldDTRDetails")
                {
                    LoadDtrDetails(hdfOffCode.Value, "2");
                    failureText.Text = "Total Field DTRDetails";
                    failure.Text = "Total Field DTRDetails";
                }
                if (Request.QueryString["value"] == "TotalBankDTRDetails")
                {
                    LoadDtrDetails(hdfOffCode.Value, "5");
                    failureText.Text = "Total BankDTR Details";
                    failure.Text = "Total BankDTR Details";
                }
                if (Request.QueryString["value"] == "TotalStoreDTRDetails")
                {
                    LoadDtrDetails(hdfOffCode.Value, "1");
                    failureText.Text = "Total StoreDTR Details";
                    failure.Text = "Total StoreDTR Details";
                }
                if (Request.QueryString["value"] == "TotalRepairerDTRDetails")
                {
                    LoadDtrDetails(hdfOffCode.Value, "3");
                    failureText.Text = "Total RepairerDTR Details";
                    failure.Text = "Total RepairerDTR Details";
                }
                if (Request.QueryString["value"] == "TcGoodCount")
                {
                    dtLoadDetails = objDashboard.Load_Tc_Good(hdfOffCode.Value);
                    GridTcGood.Visible = true;
                    GridTcGood.DataSource = dtLoadDetails;
                    GridTcGood.DataBind();
                    ViewState["TcGoodCount"] = dtLoadDetails;
                    failureText.Text = "Details Of Good Transformers";
                    failure.Text = "Details Of Good Transformers";
                }
                if (Request.QueryString["value"] == "TcReleaseGoodCount")
                {
                    dtLoadDetails = objDashboard.Load_Tc_Release_Good(hdfOffCode.Value);
                    GridTcReleaseGood.Visible = true;
                    GridTcReleaseGood.DataSource = dtLoadDetails;
                    GridTcReleaseGood.DataBind();
                    ViewState["TcReleaseGoodCount"] = dtLoadDetails;
                    failureText.Text = "Details Of Release Good Transformers";
                    failure.Text = "Details Of Release Good Transformers";
                }
                if (Request.QueryString["value"] == "TcRepairGoodCount")
                {
                    dtLoadDetails = objDashboard.Load_Tc_Repair_Good(hdfOffCode.Value);
                    GridRepairGood.Visible = true;
                    GridRepairGood.DataSource = dtLoadDetails;
                    GridRepairGood.DataBind();
                    ViewState["TcRepairGoodCount"] = dtLoadDetails;
                    failureText.Text = "Details Of Repair Good Transformers ";
                    failure.Text = "Details Of Repair Good Transformers";
                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadDtrDetails(string sofficeCode, string type)
        {
            DataTable dtinfo = new DataTable();
            //DataSet dsinfo = new DataSet();
            //StringReader readerinfo = new StringReader(Convert.ToString( Session["dtInfo"]));
            //dsinfo.ReadXml(readerinfo);
            //dtinfo = dsinfo.Tables[0];
            //ViewState["DTRDetails"] = dtinfo;
            clsDashboard obj = new clsDashboard();

            dtinfo = obj.GetDtrDetails(sofficeCode, type);
            ViewState["DTRDetails"] = dtinfo;
            grdTotalDTRDetails.Visible = true;
            grdTotalDTRDetails.DataSource = dtinfo;
            grdTotalDTRDetails.DataBind();
        }
        protected void grdReceiveTC_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtCode = (TextBox)row.FindControl("txtDtCode");
                    TextBox txtDTCName = (TextBox)row.FindControl("txtDTCName");

                    DataTable dt = (DataTable)ViewState["ReceiveTC"];
                    dv = dt.DefaultView;

                    if (txtDtCode.Text != "")
                    {
                        sFilter = "DT_CODE Like '%" + txtDtCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtDTCName.Text != "")
                    {
                        sFilter += " DT_NAME Like '%" + txtDTCName.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdReceiveTC.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdReceiveTC.DataSource = dv;
                            ViewState["ReceiveTC"] = dv.ToTable();
                            grdReceiveTC.DataBind();

                        }
                        else
                        {
                            ViewState["ReceiveTC"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailurePendingDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void grdSingleComission_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtCode = (TextBox)row.FindControl("txtDtCode");
                    TextBox txtDTCName = (TextBox)row.FindControl("txtDTCName");

                    DataTable dt = (DataTable)ViewState["SingleComission"];
                    dv = dt.DefaultView;

                    if (txtDtCode.Text != "")
                    {
                        sFilter = "DT_CODE Like '%" + txtDtCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtDTCName.Text != "")
                    {
                        sFilter += " DT_NAME Like '%" + txtDTCName.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdSingleComission.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdSingleComission.DataSource = dv;
                            ViewState["SingleComission"] = dv.ToTable();
                            grdSingleComission.DataBind();

                        }
                        else
                        {
                            ViewState["SingleComission"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailurePendingDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void grdConditionOfTC_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                LoadFailurePendingDetails();
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();
                    String var = "";
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtcCode = (TextBox)row.FindControl("txtDtcCode");
                    TextBox txtslno = (TextBox)row.FindControl("txtslno");
                    if (Request.QueryString["value"] == "NewTCcount")
                    {
                        var = "NewTCcount";
                    }
                    else if (Request.QueryString["value"] == "RepairTCcount")
                    {
                        var = "RepairTCcount";
                    }
                    else if (Request.QueryString["value"] == "ReleaseTCcount")
                    {
                        var = "ReleaseTCcount";
                    }
                    else if (Request.QueryString["value"] == "FaultyTCcount")
                    {
                        var = "FaultyTCcount";
                    }
                    else
                    {
                        var = "mobileTCcount";
                    }
                    DataTable dt = (DataTable)ViewState[var];
                    dv = dt.DefaultView;

                    if (txtDtcCode.Text != "")
                    {
                        sFilter = "Convert(TC_CODE,System.String) Like '%" + txtDtcCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtslno.Text != "")
                    {
                        sFilter += "Convert(TC_SLNO,System.String)  Like '%" + txtslno.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdConditionOfTC.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdConditionOfTC.DataSource = dv;
                            ViewState[var] = dv.ToTable();
                            grdConditionOfTC.DataBind();

                        }
                        else
                        {
                            ViewState[var] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailurePendingDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdTCCapacityWise_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                LoadFailurePendingDetails();
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();
                    String var = "";

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtcCode = (TextBox)row.FindControl("txtDtcCode");
                    TextBox txtslno = (TextBox)row.FindControl("txtslno");
                    if (Request.QueryString["value"] == "TCless25_count")
                    {
                        var = "TCless25_count";
                    }
                    else if (Request.QueryString["value"] == "TC25_100_count")
                    {
                        var = "TC25_100_count";
                    }
                    else if (Request.QueryString["value"] == "TC125_250_count")
                    {
                        var = "TC125_250_count";
                    }
                    else
                    {
                        var = "TCgreater250_count";
                    }
                    DataTable dt = (DataTable)ViewState[var];
                    dv = dt.DefaultView;

                    if (txtDtcCode.Text != "")
                    {
                        sFilter = "Convert(TC_CODE,System.String) Like '%" + txtDtcCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtslno.Text != "")
                    {
                        sFilter += " Convert(TC_SLNO,System.String)  Like '%" + txtslno.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdTCCapacityWise.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdTCCapacityWise.DataSource = dv;
                            ViewState[var] = dv.ToTable();
                            grdTCCapacityWise.DataBind();

                        }
                        else
                        {
                            ViewState[var] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailurePendingDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdTCPendingDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                LoadFailurePendingDetails();
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    String var = "";
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtcCode = (TextBox)row.FindControl("txtDtcCode");
                    TextBox txtslno = (TextBox)row.FindControl("txtslno");

                    if (Request.QueryString["value"] == "TCpending_release_count")
                    {
                        var = "TCpending_release_count";
                    }
                    else if (Request.QueryString["value"] == "TCpending_issue_count")
                    {
                        var = "TCpending_issue_count";
                    }
                    else
                    {
                        var = "TCpending_repair_count";
                    }
                    DataTable dt = (DataTable)ViewState[var];
                    dv = dt.DefaultView;

                    if (txtDtcCode.Text != "")
                    {
                        sFilter = "Convert(TC_CODE,System.String) Like '%" + txtDtcCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtslno.Text != "")
                    {
                        sFilter += " Convert(TC_SLNO,System.String) Like '%" + txtslno.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdTCPendingDetails.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdTCPendingDetails.DataSource = dv;
                            ViewState[var] = dv.ToTable();
                            grdTCPendingDetails.DataBind();

                        }
                        else
                        {
                            ViewState[var] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailurePendingDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdFailurePending_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtCode = (TextBox)row.FindControl("txtDtCode");
                    TextBox txtDTCName = (TextBox)row.FindControl("txtDTCName");
                    //TextBox txtFailID = (TextBox)row.FindControl("txtFailureID");
                    TextBox txtSectionCode = (TextBox)row.FindControl("txtsection");
                    TextBox txtfailid = (TextBox)row.FindControl("txtfailid");

                    DataTable dt = (DataTable)ViewState["FailurePending"];
                    // dt.Columns[5].DataType = typeof(string);
                    dv = dt.DefaultView;

                    if (txtDtCode.Text != "")
                    {
                        sFilter = "DT_CODE Like '%" + txtDtCode.Text.Replace("'", "'").Trim() + "%' AND";
                    }
                    if (txtDTCName.Text != "")
                    {
                        sFilter += " DT_NAME Like '%" + txtDTCName.Text.Replace("'", "'").Trim() + "%' AND";
                    }
                    //if(txtFailID.Text != "")
                    //{
                    //    sFilter += " DF_ID  = '"+txtFailID.Text+"' AND";
                    //}
                    if (txtSectionCode.Text != "")
                    {
                        sFilter += " OMSECTION Like '%" + txtSectionCode.Text.Replace("'", "'").Trim() + "%' AND";
                    }
                    if (txtfailid.Text != "")
                    {
                        //sFilter += "DF_ID Like '%" + txtfailid.Text.Trim() + "%' AND";
                        sFilter += " DF_ID = '" + txtfailid.Text + "' AND";
                    }

                    //  sFilter += "DF_EQUIPMENT_ID = '" + txtDTRCode.Text + "' AND";
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdFailurePending.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdFailurePending.DataSource = dv;
                            ViewState["FailurePending"] = dv.ToTable();
                            grdFailurePending.DataBind();

                        }
                        else
                        {
                            ViewState["FailurePending"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailurePendingDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdFailureApprovalPending_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "search")
            {
                string sFilter = string.Empty;
                DataView dv = new DataView();

                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                TextBox txtDtCode = (TextBox)row.FindControl("txtDtCode");
                TextBox txtDTCName = (TextBox)row.FindControl("txtDTCName");
                //TextBox txtSectionCode = (TextBox)row.FindControl("txtsection");

                DataTable dt = (DataTable)ViewState["FailurePendingApproval"];
                dv = dt.DefaultView;

                if (txtDtCode.Text != "")
                {
                    sFilter = "DT_CODE Like '%" + txtDtCode.Text.Replace("'", "'") + "%' AND";
                }
                if (txtDTCName.Text != "")
                {
                    sFilter += " DT_NAME Like '%" + txtDTCName.Text.Replace("'", "'") + "%' AND";
                }
                //if(txtSectionCode.Text != "")
                //{
                //    sFilter += " OMSECTION Like '%" + txtSectionCode.Text.Replace("'", "'") + "%' AND";
                //}
                if (sFilter.Length > 0)
                {
                    sFilter = sFilter.Remove(sFilter.Length - 3);
                    grdFailureApprovalPending.PageIndex = 0;
                    dv.RowFilter = sFilter;
                    if (dv.Count > 0)
                    {
                        grdFailureApprovalPending.DataSource = dv;
                        ViewState["FailurePendingApproval"] = dv.ToTable();
                        grdFailureApprovalPending.DataBind();

                    }
                    else
                    {
                        ViewState["FailurePendingApproval"] = dv.ToTable();
                        ShowEmptyGrid();
                    }
                }
                else
                {
                    LoadFailurePendingDetails();
                }
            }
        }

        protected void grdEstimationPending_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtCode = (TextBox)row.FindControl("txtDtCode");
                    TextBox txtDTCName = (TextBox)row.FindControl("txtDTCName");
                    TextBox txtDTRCode = (TextBox)row.FindControl("txtDTRCode");

                    DataTable dt = (DataTable)ViewState["estimation"];
                    dv = dt.DefaultView;

                    if (txtDtCode.Text != "")
                    {
                        sFilter = "DT_CODE Like '%" + txtDtCode.Text.Replace("'", "'").Trim() + "%' AND";
                    }
                    if (txtDTCName.Text != "")
                    {
                        sFilter += " DT_NAME Like '%" + txtDTCName.Text.Replace("'", "'").Trim() + "%' AND";
                    }
                    //if (txtDTRCode.Text != "")
                    //{
                    //    sFilter += "DF_EQUIPMENT_ID = '" + txtDTRCode.Text + "' AND";
                    //}



                    if (txtDTRCode.Text != "")
                    {
                        sFilter += " DF_EQUIPMENT_ID Like '%" + txtDTRCode.Text.Replace("'", "'").Trim() + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdEstimationPending.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdEstimationPending.DataSource = dv;
                            ViewState["estimation"] = dv.ToTable();
                            grdEstimationPending.DataBind();

                        }
                        else
                        {
                            ViewState["estimation"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailurePendingDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdWorkorderPending_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();
                    DataTable dt = new DataTable();
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtCode = (TextBox)row.FindControl("txtDtCode");
                    TextBox txtDTCName = (TextBox)row.FindControl("txtDTCName");
                    TextBox txtDTRCode = (TextBox)row.FindControl("txtDTRCode");
                    if (Request.QueryString["value"] == "workorder")
                    {

                        dt = (DataTable)ViewState["workorder"];

                    }
                    else
                    {
                        dt = (DataTable)ViewState["Singleworkorder"];

                    }

                    //   dt = (DataTable)ViewState["Singleworkorder"];
                    dv = dt.DefaultView;

                    if (txtDtCode.Text != "")
                    {
                        // sFilter = "DT_CODE Like '%" + txtDtCode.Text.Replace("'", "'") + "%' AND";
                        sFilter = "DT_CODE Like '%" + txtDtCode.Text.Replace("'", "'").Trim() + "%' AND";
                    }
                    if (txtDTCName.Text != "")
                    {
                        sFilter += " DT_NAME Like '%" + txtDTCName.Text.Replace("'", "'").Trim() + "%' AND";
                    }
                    if (txtDTRCode.Text != "")
                    {
                        sFilter += " DF_EQUIPMENT_ID Like '%" + txtDTRCode.Text.Replace("'", "'").Trim() + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdWorkorderPending.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdWorkorderPending.DataSource = dv;
                            if (Request.QueryString["value"] == "workorder")
                            {

                                // dt = (DataTable)ViewState["workorder"];
                                ViewState["workorder"] = dv.ToTable();

                            }
                            else
                            {


                                //dt = (DataTable)ViewState["Singleworkorder"];
                                ViewState["Singleworkorder"] = dv.ToTable();

                            }

                            grdWorkorderPending.DataBind();

                        }
                        else
                        {
                            grdWorkorderPending.DataSource = dv;
                            if (Request.QueryString["value"] == "workorder")
                            {
                                ViewState["workorder"] = dv.ToTable();
                            }
                            else
                            {
                                ViewState["Singleworkorder"] = dv.ToTable();
                            }
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailurePendingDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdIndentPending_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtCode = (TextBox)row.FindControl("txtDtCode");
                    TextBox txtDTCName = (TextBox)row.FindControl("txtDTCName");

                    DataTable dt = (DataTable)ViewState["indent"];
                    dv = dt.DefaultView;

                    if (txtDtCode.Text != "")
                    {
                        sFilter = "DT_CODE Like '%" + txtDtCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtDTCName.Text != "")
                    {
                        sFilter += " DT_NAME Like '%" + txtDTCName.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdIndentPending.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdIndentPending.DataSource = dv;
                            ViewState["indent"] = dv.ToTable();
                            grdIndentPending.DataBind();

                        }
                        else
                        {
                            ViewState["indent"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailurePendingDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdinvoicePending_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtCode = (TextBox)row.FindControl("txtDtCode");
                    TextBox txtDTCName = (TextBox)row.FindControl("txtDTCName");
                    TextBox txtDTRCode = (TextBox)row.FindControl("txtDTRCode");
                    TextBox txtWorkorderNumber = (TextBox)row.FindControl("txtWorkorderNumber");
                    TextBox txtWorkorderDate = (TextBox)row.FindControl("txtWorkorderDate");
                    DataTable dt = (DataTable)ViewState["invoice"];
                    dv = dt.DefaultView;

                    if (txtDtCode.Text != "")
                    {
                        sFilter = "DT_CODE Like '%" + txtDtCode.Text.Replace("'", "'").Trim() + "%' AND";
                    }
                    if (txtDTCName.Text != "")
                    {
                        sFilter += " DT_NAME Like '%" + txtDTCName.Text.Replace("'", "'").Trim() + "%' AND";
                        // sFilter += " DF_ID " + txtDTCName.Text + "' AND";
                    }
                    if (txtDTRCode.Text != "")
                    {
                        sFilter += " DF_EQUIPMENT_ID Like '%" + txtDTRCode.Text.Replace("'", "'").Trim() + "%' AND";
                    }
                    if (txtWorkorderNumber.Text != "")
                    {
                        sFilter += " WO_NO Like '%" + txtWorkorderNumber.Text.Replace("'", "'").Trim() + "%' AND";
                    }
                    if (txtWorkorderDate.Text != "")
                    {
                        sFilter += " WO_DATE Like '%" + txtWorkorderDate.Text.Replace("'", "'").Trim() + "%' AND";
                    }

                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdinvoicePending.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdinvoicePending.DataSource = dv;
                            ViewState["invoice"] = dv.ToTable();
                            grdinvoicePending.DataBind();

                        }
                        else
                        {
                            ViewState["invoice"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailurePendingDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdDecommissionPending_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtCode = (TextBox)row.FindControl("txtDtCode");
                    TextBox txtDTCName = (TextBox)row.FindControl("txtDTCName");

                    DataTable dt = (DataTable)ViewState["DeCommission"];
                    dv = dt.DefaultView;

                    if (txtDtCode.Text != "")
                    {
                        sFilter = "DT_CODE Like '%" + txtDtCode.Text.Replace("'", "'").Trim() + "%' AND";
                    }
                    if (txtDTCName.Text != "")
                    {
                        sFilter += " DT_NAME Like '%" + txtDTCName.Text.Replace("'", "'").Trim() + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdDecommissionPending.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdDecommissionPending.DataSource = dv;
                            ViewState["DeCommission"] = dv.ToTable();
                            grdDecommissionPending.DataBind();

                        }
                        else
                        {
                            ViewState["DeCommission"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailurePendingDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdRIPending_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtCode = (TextBox)row.FindControl("txtDtCode");
                    TextBox txtDTCName = (TextBox)row.FindControl("txtDTCName");
                    TextBox txtFaultyDTRCode = (TextBox)row.FindControl("txtFaultyDTRCode");
                    TextBox txtInvoicedDTrCode = (TextBox)row.FindControl("txtInvoicedDTrCode");
                    //  TextBox txtDTRCode = (TextBox)row.FindControl("txtDTRCode");
                    DataTable dt = (DataTable)ViewState["RI"];
                    dv = dt.DefaultView;

                    if (txtDtCode.Text != "")
                    {
                        sFilter = "DT_CODE Like '%" + txtDtCode.Text.Replace("'", "'").Trim() + "%' AND";
                    }
                    if (txtDTCName.Text != "")
                    {
                        sFilter += " DT_NAME Like '%" + txtDTCName.Text.Replace("'", "'").Trim() + "%' AND";
                    }
                    if (txtFaultyDTRCode.Text != "")
                    {
                        sFilter += " DF_EQUIPMENT_ID  Like '%" + txtFaultyDTRCode.Text.Replace("'", "'").Trim() + "%' AND";
                    }
                    if (txtInvoicedDTrCode.Text != "")
                    {
                        sFilter += " DT_TC_ID like '%" + txtInvoicedDTrCode.Text.Replace("'", "'").Trim() + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdRIPending.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdRIPending.DataSource = dv;
                            ViewState["RI"] = dv.ToTable();
                            grdRIPending.DataBind();

                        }
                        else
                        {
                            ViewState["RI"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailurePendingDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void grdCRPending_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtCode = (TextBox)row.FindControl("txtDtCode");
                    TextBox txtDTCName = (TextBox)row.FindControl("txtDTCName");
                    TextBox txtInvoicedDTRCode = (TextBox)row.FindControl("txtInvoicedDTRCode");
                    TextBox txtFaultyDTRCode = (TextBox)row.FindControl("txtFaultyDTRCode");

                    DataTable dt = (DataTable)ViewState["CR"];
                    dv = dt.DefaultView;

                    if (txtDtCode.Text != "")
                    {
                        sFilter = "DT_CODE Like '%" + txtDtCode.Text.Replace("'", "'").Trim() + "%' AND";
                    }
                    if (txtDTCName.Text != "")
                    {
                        sFilter += " DT_NAME Like '%" + txtDTCName.Text.Replace("'", "'").Trim() + "%' AND";
                    }

                    if (txtFaultyDTRCode.Text != "")
                    {
                        sFilter += " DF_EQUIPMENT_ID  Like '%" + txtFaultyDTRCode.Text.Replace("'", "'").Trim() + "%' AND";
                    }
                    if (txtInvoicedDTRCode.Text != "")
                    {
                        sFilter += " DT_TC_ID like '%" + txtInvoicedDTRCode.Text.Replace("'", "'").Trim() + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdCRPending.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdCRPending.DataSource = dv;
                            ViewState["RI"] = dv.ToTable();
                            grdCRPending.DataBind();

                        }
                        else
                        {
                            ViewState["CR"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailurePendingDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);

                if (Request.QueryString["value"] == "Failure")
                {
                    dt.Columns.Add("DT_CODE");
                    dt.Columns.Add("DT_NAME");
                    dt.Columns.Add("OMSECTION");
                    dt.Columns.Add("OM_CODE");
                    dt.Columns.Add("SUBDIVSION");
                    dt.Columns.Add("DF_ID");
                    dt.Columns.Add("DF_DATE");
                    dt.Columns.Add("FL_STATUS");
                    dt.Columns.Add("DIVSION");

                    grdFailurePending.DataSource = dt;
                    grdFailurePending.DataBind();

                    int iColCount = grdFailurePending.Rows[0].Cells.Count;
                    grdFailurePending.Rows[0].Cells.Clear();
                    grdFailurePending.Rows[0].Cells.Add(new TableCell());
                    grdFailurePending.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdFailurePending.Rows[0].Cells[0].Text = "No Records Found";
                }

                if (Request.QueryString["value"] == "FailurePendingApproval")
                {
                    dt.Columns.Add("DT_CODE");
                    dt.Columns.Add("DT_NAME");
                    dt.Columns.Add("OMSECTION");
                    dt.Columns.Add("OM_CODE");
                    dt.Columns.Add("SUBDIVSION");
                    dt.Columns.Add("DAYS_FROM_PENDING");
                    dt.Columns.Add("DIVSION");

                    grdFailureApprovalPending.DataSource = dt;
                    grdFailureApprovalPending.DataBind();

                    int iColCount = grdFailureApprovalPending.Rows[0].Cells.Count;
                    grdFailureApprovalPending.Rows[0].Cells.Clear();
                    grdFailureApprovalPending.Rows[0].Cells.Add(new TableCell());
                    grdFailureApprovalPending.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdFailureApprovalPending.Rows[0].Cells[0].Text = "No Records Found";
                }

                if (Request.QueryString["value"] == "estimation")
                {
                    dt.Columns.Add("DT_CODE");
                    dt.Columns.Add("DT_NAME");
                    dt.Columns.Add("OMSECTION");
                    //dt.Columns.Add("OM_CODE");
                    dt.Columns.Add("SUBDIVSION");
                    dt.Columns.Add("EST_NO");
                    //dt.Columns.Add("EST_CRON");
                    //dt.Columns.Add("FL_STATUS");
                    dt.Columns.Add("DF_EQUIPMENT_ID");
                    dt.Columns.Add("DAYS_FROM_PENDING");
                    dt.Columns.Add("DF_DATE");
                    dt.Columns.Add("DIVSION");

                    grdEstimationPending.DataSource = dt;
                    grdEstimationPending.DataBind();

                    int iColCount = grdEstimationPending.Rows[0].Cells.Count;
                    grdEstimationPending.Rows[0].Cells.Clear();
                    grdEstimationPending.Rows[0].Cells.Add(new TableCell());
                    grdEstimationPending.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdEstimationPending.Rows[0].Cells[0].Text = "No Records Found";
                }
                if (Request.QueryString["value"] == "workorder")
                {
                    dt.Columns.Add("DT_CODE");
                    dt.Columns.Add("DT_NAME");
                    dt.Columns.Add("OMSECTION");
                    //dt.Columns.Add("OM_CODE");
                    dt.Columns.Add("SUBDIVSION");
                    dt.Columns.Add("DF_DATE");
                    dt.Columns.Add("DAYS_FROM_PENDING");
                    dt.Columns.Add("DF_EQUIPMENT_ID");
                    dt.Columns.Add("DIVSION");
                    dt.Columns.Add("DF_ID");


                    grdWorkorderPending.DataSource = dt;
                    grdWorkorderPending.DataBind();

                    int iColCount = grdWorkorderPending.Rows[0].Cells.Count;
                    grdWorkorderPending.Rows[0].Cells.Clear();
                    grdWorkorderPending.Rows[0].Cells.Add(new TableCell());
                    grdWorkorderPending.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdWorkorderPending.Rows[0].Cells[0].Text = "No Records Found";
                }

                if (Request.QueryString["value"] == "Singleworkorder")
                {
                    dt.Columns.Add("DT_CODE");
                    dt.Columns.Add("DT_NAME");
                    dt.Columns.Add("OMSECTION");
                    //  dt.Columns.Add("OM_CODE");
                    dt.Columns.Add("SUBDIVSION");
                    dt.Columns.Add("DF_DATE");
                    dt.Columns.Add("DIVSION");
                    dt.Columns.Add("DAYS_FROM_PENDING");
                    dt.Columns.Add("DF_EQUIPMENT_ID");


                    grdWorkorderPending.DataSource = dt;
                    grdWorkorderPending.DataBind();

                    int iColCount = grdWorkorderPending.Rows[0].Cells.Count;
                    grdWorkorderPending.Rows[0].Cells.Clear();
                    grdWorkorderPending.Rows[0].Cells.Add(new TableCell());
                    grdWorkorderPending.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdWorkorderPending.Rows[0].Cells[0].Text = "No Records Found";
                }
                if (Request.QueryString["value"] == "ReceiveTC")
                {
                    dt.Columns.Add("DT_CODE");
                    dt.Columns.Add("DT_NAME");
                    dt.Columns.Add("OMSECTION");
                    dt.Columns.Add("OM_CODE");
                    dt.Columns.Add("DIVSION");
                    dt.Columns.Add("SUBDIVSION");
                    dt.Columns.Add("WO_DATE");
                    dt.Columns.Add("WO_NO");
                    dt.Columns.Add("DAYS_FROM_PENDING");

                    grdReceiveTC.DataSource = dt;
                    grdReceiveTC.DataBind();

                    int iColCount = grdReceiveTC.Rows[0].Cells.Count;
                    grdReceiveTC.Rows[0].Cells.Clear();
                    grdReceiveTC.Rows[0].Cells.Add(new TableCell());
                    grdReceiveTC.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdReceiveTC.Rows[0].Cells[0].Text = "No Records Found";
                }
                if (Request.QueryString["value"] == "SingleComission")
                {
                    dt.Columns.Add("DT_CODE");
                    dt.Columns.Add("DT_NAME");
                    dt.Columns.Add("OMSECTION");
                    dt.Columns.Add("OM_CODE");
                    dt.Columns.Add("SUBDIVSION");
                    dt.Columns.Add("WO_DATE");
                    dt.Columns.Add("DIVSION");
                    dt.Columns.Add("WO_NO");
                    dt.Columns.Add("DAYS_FROM_PENDING");
                    dt.Columns.Add("COMMISSION_STATUS");

                    grdSingleComission.DataSource = dt;
                    grdSingleComission.DataBind();

                    int iColCount = grdSingleComission.Rows[0].Cells.Count;
                    grdSingleComission.Rows[0].Cells.Clear();
                    grdSingleComission.Rows[0].Cells.Add(new TableCell());
                    grdSingleComission.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdSingleComission.Rows[0].Cells[0].Text = "No Records Found";
                }
                if (Request.QueryString["value"] == "indent")
                {
                    dt.Columns.Add("DT_CODE");
                    dt.Columns.Add("DT_NAME");
                    dt.Columns.Add("OMSECTION");
                    dt.Columns.Add("OM_CODE");
                    dt.Columns.Add("SUBDIVSION");
                    dt.Columns.Add("DIVSION");
                    //dt.Columns.Add("EST_NO");
                    dt.Columns.Add("DF_DATE");
                    dt.Columns.Add("DAYS_FROM_PENDING");

                    grdIndentPending.DataSource = dt;
                    grdIndentPending.DataBind();

                    int iColCount = grdIndentPending.Rows[0].Cells.Count;
                    grdIndentPending.Rows[0].Cells.Clear();
                    grdIndentPending.Rows[0].Cells.Add(new TableCell());
                    grdIndentPending.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdIndentPending.Rows[0].Cells[0].Text = "No Records Found";
                }
                if (Request.QueryString["value"] == "invoice")
                {
                    dt.Columns.Add("DT_CODE");
                    dt.Columns.Add("DT_NAME");
                    dt.Columns.Add("OMSECTION");
                    // dt.Columns.Add("OM_CODE");
                    dt.Columns.Add("DIVSION");
                    dt.Columns.Add("SUBDIVSION");
                    dt.Columns.Add("DF_DATE");
                    dt.Columns.Add("DAYS_FROM_PENDING");
                    dt.Columns.Add("DF_EQUIPMENT_ID");
                    dt.Columns.Add("WO_NO");
                    dt.Columns.Add("WO_DATE");
                    dt.Columns.Add("DF_ID");



                    grdinvoicePending.DataSource = dt;
                    grdinvoicePending.DataBind();

                    int iColCount = grdinvoicePending.Rows[0].Cells.Count;
                    grdinvoicePending.Rows[0].Cells.Clear();
                    grdinvoicePending.Rows[0].Cells.Add(new TableCell());
                    grdinvoicePending.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdinvoicePending.Rows[0].Cells[0].Text = "No Records Found";
                }

                if (Request.QueryString["value"] == "DeCommission")
                {
                    dt.Columns.Add("DT_CODE");
                    dt.Columns.Add("DT_NAME");
                    dt.Columns.Add("OMSECTION");
                    // dt.Columns.Add("OM_CODE");
                    dt.Columns.Add("SUBDIVSION");
                    dt.Columns.Add("DIVSION");
                    dt.Columns.Add("DAYS_FROM_PENDING");
                    dt.Columns.Add("DF_EQUIPMENT_ID");


                    grdDecommissionPending.DataSource = dt;
                    grdDecommissionPending.DataBind();

                    int iColCount = grdDecommissionPending.Rows[0].Cells.Count;
                    grdDecommissionPending.Rows[0].Cells.Clear();
                    grdDecommissionPending.Rows[0].Cells.Add(new TableCell());
                    grdDecommissionPending.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdDecommissionPending.Rows[0].Cells[0].Text = "No Records Found";
                }
                if (Request.QueryString["value"] == "RI")
                {
                    dt.Columns.Add("DT_CODE");
                    dt.Columns.Add("DT_NAME");
                    dt.Columns.Add("OMSECTION");
                    dt.Columns.Add("OM_CODE");
                    dt.Columns.Add("SUBDIVSION");
                    dt.Columns.Add("DIVSION");
                    // dt.Columns.Add("CR_DAYS_FROM_PENDING");
                    dt.Columns.Add("DAYS_FROM_PENDING");
                    dt.Columns.Add("DT_TC_ID");
                    dt.Columns.Add("DF_EQUIPMENT_ID");

                    grdRIPending.DataSource = dt;
                    grdRIPending.DataBind();

                    int iColCount = grdRIPending.Rows[0].Cells.Count;
                    grdRIPending.Rows[0].Cells.Clear();
                    grdRIPending.Rows[0].Cells.Add(new TableCell());
                    grdRIPending.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdRIPending.Rows[0].Cells[0].Text = "No Records Found";
                }

                if (Request.QueryString["value"] == "CR")
                {
                    dt.Columns.Add("DT_CODE");
                    dt.Columns.Add("DT_NAME");
                    dt.Columns.Add("OMSECTION");
                    dt.Columns.Add("OM_CODE");
                    dt.Columns.Add("SUBDIVSION");
                    dt.Columns.Add("DIVSION");
                    dt.Columns.Add("CR_DAYS_FROM_PENDING");
                    dt.Columns.Add("DT_TC_ID");
                    dt.Columns.Add("DF_EQUIPMENT_ID");
                    //dt.Columns.Add("DAYS_FROM_PENDING");

                    grdCRPending.DataSource = dt;
                    grdCRPending.DataBind();

                    int iColCount = grdCRPending.Rows[0].Cells.Count;
                    grdCRPending.Rows[0].Cells.Clear();
                    grdCRPending.Rows[0].Cells.Add(new TableCell());
                    grdCRPending.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdCRPending.Rows[0].Cells[0].Text = "No Records Found";
                }
                if ((Request.QueryString["value"] == "NewTCcount") || (Request.QueryString["value"] == "RepairTCcount") || (Request.QueryString["value"] == "ReleaseTCcount") || (Request.QueryString["value"] == "FaultyTCcount") || (Request.QueryString["value"] == "mobileTCcount"))
                {
                    dt.Columns.Add("TC_CODE");
                    dt.Columns.Add("TC_SLNO");
                    dt.Columns.Add("TC_CAPACITY");
                    dt.Columns.Add("TM_NAME");
                    dt.Columns.Add("TC_RATING");


                    grdConditionOfTC.DataSource = dt;
                    grdConditionOfTC.DataBind();

                    int iColCount = grdConditionOfTC.Rows[0].Cells.Count;
                    grdConditionOfTC.Rows[0].Cells.Clear();
                    grdConditionOfTC.Rows[0].Cells.Add(new TableCell());
                    grdConditionOfTC.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdConditionOfTC.Rows[0].Cells[0].Text = "No Records Found";
                }
                if ((Request.QueryString["value"] == "TCless25_count") || (Request.QueryString["value"] == "TC25_100_count") || (Request.QueryString["value"] == "TC125_250_count") || (Request.QueryString["value"] == "TCgreater250_count"))
                {
                    dt.Columns.Add("TC_CODE");
                    dt.Columns.Add("TC_SLNO");
                    dt.Columns.Add("TC_CAPACITY");
                    dt.Columns.Add("TM_NAME");
                    dt.Columns.Add("TC_RATING");


                    grdTCCapacityWise.DataSource = dt;
                    grdTCCapacityWise.DataBind();

                    int iColCount = grdTCCapacityWise.Rows[0].Cells.Count;
                    grdTCCapacityWise.Rows[0].Cells.Clear();
                    grdTCCapacityWise.Rows[0].Cells.Add(new TableCell());
                    grdTCCapacityWise.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdTCCapacityWise.Rows[0].Cells[0].Text = "No Records Found";
                }
                if ((Request.QueryString["value"] == "TCpending_issue_count") || (Request.QueryString["value"] == "TCpending_repair_count") || (Request.QueryString["value"] == "TCpending_release_count"))
                {
                    dt.Columns.Add("TC_CODE");
                    dt.Columns.Add("TC_SLNO");
                    dt.Columns.Add("TC_CAPACITY");
                    dt.Columns.Add("TM_NAME");
                    dt.Columns.Add("TC_RATING");


                    grdTCPendingDetails.DataSource = dt;
                    grdTCPendingDetails.DataBind();

                    int iColCount = grdTCPendingDetails.Rows[0].Cells.Count;
                    grdTCPendingDetails.Rows[0].Cells.Clear();
                    grdTCPendingDetails.Rows[0].Cells.Add(new TableCell());
                    grdTCPendingDetails.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdTCPendingDetails.Rows[0].Cells[0].Text = "No Records Found";
                }

                if (Request.QueryString["value"] == "TotalDTRDetails" || Request.QueryString["value"] == "TotalFieldDTRDetails" || Request.QueryString["value"] == "TotalBankDTRDetails" || Request.QueryString["value"] == "TotalStoreDTRDetails" || Request.QueryString["value"] == "TotalRepairerDTRDetails")
                {
                    dt.Columns.Add("TC_CODE");
                    dt.Columns.Add("TC_SLNO");
                    dt.Columns.Add("TC_CAPACITY");
                    dt.Columns.Add("TM_NAME");
                    dt.Columns.Add("TC_RATING");


                    grdTotalDTRDetails.DataSource = dt;
                    grdTotalDTRDetails.DataBind();

                    int iColCount = grdTotalDTRDetails.Rows[0].Cells.Count;
                    grdTotalDTRDetails.Rows[0].Cells.Clear();
                    grdTotalDTRDetails.Rows[0].Cells.Add(new TableCell());
                    grdTotalDTRDetails.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdTotalDTRDetails.Rows[0].Cells[0].Text = "No Records Found";
                }

                if (Request.QueryString["value"] == "TcGoodCount" || Request.QueryString["value"] == "TcReleaseGoodCount" || Request.QueryString["value"] == "TcRepairGoodCount")
                {
                    dt.Columns.Add("TC_CODE");
                    dt.Columns.Add("TC_SLNO");
                    dt.Columns.Add("TC_CAPACITY");
                    dt.Columns.Add("TM_NAME");
                    dt.Columns.Add("TC_RATING");
                    int iColCount = 0;
                    
                    switch (Request.QueryString["value"])
                    {
                        case "TcGoodCount":                                                        
                            GridTcGood.DataSource = dt;
                            GridTcGood.DataBind();
                            iColCount = GridTcGood.Rows[0].Cells.Count;
                            GridTcGood.Rows[0].Cells.Clear();
                            GridTcGood.Rows[0].Cells.Add(new TableCell());
                            GridTcGood.Rows[0].Cells[0].ColumnSpan = iColCount;
                            GridTcGood.Rows[0].Cells[0].Text = "No Records Found";
                            break;

                        case "TcReleaseGoodCount":
                            GridTcReleaseGood.DataSource = dt;
                            GridTcReleaseGood.DataBind();
                            iColCount = GridTcReleaseGood.Rows[0].Cells.Count;
                            GridTcReleaseGood.Rows[0].Cells.Clear();
                            GridTcReleaseGood.Rows[0].Cells.Add(new TableCell());
                            GridTcReleaseGood.Rows[0].Cells[0].ColumnSpan = iColCount;
                            GridTcReleaseGood.Rows[0].Cells[0].Text = "No Records Found";
                            break;

                        case "TcRepairGoodCount":
                            GridRepairGood.DataSource = dt;
                            GridRepairGood.DataBind();
                            iColCount = GridRepairGood.Rows[0].Cells.Count;
                            GridRepairGood.Rows[0].Cells.Clear();
                            GridRepairGood.Rows[0].Cells.Add(new TableCell());
                            GridRepairGood.Rows[0].Cells[0].ColumnSpan = iColCount;
                            GridRepairGood.Rows[0].Cells[0].Text = "No Records Found";
                            break;
                    }
                }               
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }
        protected void grdConditionOfTC_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                string var = "";
                grdConditionOfTC.PageIndex = e.NewPageIndex;
                if (Request.QueryString["value"] == "NewTCcount")
                {
                    var = "NewTCcount";
                }
                else if (Request.QueryString["value"] == "RepairTCcount")
                {
                    var = "RepairTCcount";
                }
                else if (Request.QueryString["value"] == "ReleaseTCcount")
                {
                    var = "ReleaseTCcount";
                }
                else if (Request.QueryString["value"] == "FaultyTCcount")
                {
                    var = "FaultyTCcount";
                }
                else
                {
                    var = "mobileTCcount";
                }
                dtComplete = (DataTable)ViewState[var];

                grdConditionOfTC.DataSource = SortDataTablePending(dtComplete as DataTable, true);
                grdConditionOfTC.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdTCCapacityWise_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                string var = "";
                grdTCCapacityWise.PageIndex = e.NewPageIndex;

                if (Request.QueryString["value"] == "TCless25_count")
                {
                    var = "TCless25_count";
                }
                else if (Request.QueryString["value"] == "TC25_100_count")
                {
                    var = "TC25_100_count";
                }
                else if (Request.QueryString["value"] == "TC125_250_count")
                {
                    var = "TC125_250_count";
                }
                else
                {
                    var = "TCgreater250_count";
                }
                dtComplete = (DataTable)ViewState[var];
                grdTCCapacityWise.DataSource = SortDataTablePending(dtComplete as DataTable, true);
                grdTCCapacityWise.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdTCPendingDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                string var = "";
                grdTCPendingDetails.PageIndex = e.NewPageIndex;
                if (Request.QueryString["value"] == "TCpending_issue_count")
                {
                    var = "TCpending_issue_count";
                }
                else if (Request.QueryString["value"] == "TCpending_repair_count")
                {
                    var = "TCpending_repair_count";
                }

                else
                {
                    var = "TCpending_release_count";
                }
                dtComplete = (DataTable)ViewState[var];
                grdTCPendingDetails.DataSource = SortDataTablePending(dtComplete as DataTable, true);
                grdTCPendingDetails.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }



        protected void grdFailurePending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdFailurePending.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["FailurePending"];
                grdFailurePending.DataSource = SortDataTablePending(dtComplete as DataTable, true);
                grdFailurePending.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdFailureApprovalPending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdFailureApprovalPending.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["FailurePendingApproval"];
                grdFailureApprovalPending.DataSource = SortDataTableEstimation(dtComplete as DataTable, true);
                grdFailureApprovalPending.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdEstimationPending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdEstimationPending.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["estimation"];
                grdEstimationPending.DataSource = SortDataTableEstimation(dtComplete as DataTable, true);
                grdEstimationPending.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdWorkorderPending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (Request.QueryString["value"] == "workorder")
                {
                    DataTable dtComplete = new DataTable();
                    grdWorkorderPending.PageIndex = e.NewPageIndex;
                    dtComplete = (DataTable)ViewState["workorder"];
                    grdWorkorderPending.DataSource = SortDataTableWorkorder(dtComplete as DataTable, true);
                    grdWorkorderPending.DataBind();
                }
                else
                {
                    DataTable dtComplete = new DataTable();
                    grdWorkorderPending.PageIndex = e.NewPageIndex;
                    dtComplete = (DataTable)ViewState["Singleworkorder"];
                    grdWorkorderPending.DataSource = SortDataTableWorkorder(dtComplete as DataTable, true);
                    grdWorkorderPending.DataBind();
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdReceiveTC_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdReceiveTC.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["ReceiveTC"];
                grdReceiveTC.DataSource = SortDataTableWorkorder(dtComplete as DataTable, true);
                grdReceiveTC.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdSingleComission_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdSingleComission.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["SingleComission"];
                grdSingleComission.DataSource = SortDataTableWorkorder(dtComplete as DataTable, true);
                grdSingleComission.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdIndentPending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdIndentPending.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["indent"];
                grdIndentPending.DataSource = SortDataTableIndent(dtComplete as DataTable, true);
                grdIndentPending.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdinvoicePending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdinvoicePending.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["invoice"];
                grdinvoicePending.DataSource = SortDataTableCommission(dtComplete as DataTable, true);
                grdinvoicePending.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdDecommissionPending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdDecommissionPending.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["DeCommission"];
                grdDecommissionPending.DataSource = SortDataTableDecommission(dtComplete as DataTable, true);
                grdDecommissionPending.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdRIPending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdRIPending.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["RI"];
                grdRIPending.DataSource = SortDataTableRi(dtComplete as DataTable, true);
                grdRIPending.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdCRPending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdCRPending.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["CR"];
                grdCRPending.DataSource = SortDataTableRi(dtComplete as DataTable, true);
                grdCRPending.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void grdFailurePending_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdFailurePending.PageIndex;
            DataTable dt = (DataTable)ViewState["FailurePending"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdFailurePending.DataSource = SortDataTablePending(dt as DataTable, false);
            }
            else
            {
                grdFailurePending.DataSource = dt;
            }
            grdFailurePending.DataBind();
            grdFailurePending.PageIndex = pageIndex;
        }
        protected void grdConditionOfTC_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdConditionOfTC.PageIndex;
            DataTable dt = (DataTable)ViewState["Condition"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdConditionOfTC.DataSource = SortDataTableCondition(dt as DataTable, false);
            }
            else
            {
                grdConditionOfTC.DataSource = dt;
            }
            grdConditionOfTC.DataBind();
            grdConditionOfTC.PageIndex = pageIndex;
        }
        protected void grdTCCapacityWise_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdTCCapacityWise.PageIndex;
            DataTable dt = (DataTable)ViewState["CapacityWise"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdTCCapacityWise.DataSource = SortDataTableCondition(dt as DataTable, false);
            }
            else
            {
                grdTCCapacityWise.DataSource = dt;
            }
            grdTCCapacityWise.DataBind();
            grdTCCapacityWise.PageIndex = pageIndex;
        }
        protected void grdTCPendingDetails_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdTCPendingDetails.PageIndex;
            DataTable dt = (DataTable)ViewState["TCpending"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdTCPendingDetails.DataSource = SortDataTableCondition(dt as DataTable, false);
            }
            else
            {
                grdTCPendingDetails.DataSource = dt;
            }
            grdTCPendingDetails.DataBind();
            grdTCPendingDetails.PageIndex = pageIndex;
        }
        protected void grdReceiveTC_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdReceiveTC.PageIndex;
            DataTable dt = (DataTable)ViewState["ReceiveTC"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdReceiveTC.DataSource = SortDataTableWorkorder(dt as DataTable, false);
            }
            else
            {
                grdReceiveTC.DataSource = dt;
            }
            grdReceiveTC.DataBind();
            grdReceiveTC.PageIndex = pageIndex;
        }

        protected void grdSingleComission_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdSingleComission.PageIndex;
            DataTable dt = (DataTable)ViewState["SingleComission"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdSingleComission.DataSource = SortDataTableWorkorder(dt as DataTable, false);
            }
            else
            {
                grdSingleComission.DataSource = dt;
            }
            grdSingleComission.DataBind();
            grdSingleComission.PageIndex = pageIndex;
        }

        protected void grdFailureApprovalPending_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdFailureApprovalPending.PageIndex;
            DataTable dt = (DataTable)ViewState["FailurePendingApproval"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdFailureApprovalPending.DataSource = SortDataTableEstimation(dt as DataTable, false);
            }

            else
            {
                grdFailureApprovalPending.DataSource = dt;
            }
            grdFailureApprovalPending.DataBind();
            grdFailureApprovalPending.PageIndex = pageIndex;
        }

        protected void grdEstimationPending_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdEstimationPending.PageIndex;
            DataTable dt = (DataTable)ViewState["estimation"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdEstimationPending.DataSource = SortDataTableEstimation(dt as DataTable, false);
            }

            else
            {
                grdEstimationPending.DataSource = dt;
            }
            grdEstimationPending.DataBind();
            grdEstimationPending.PageIndex = pageIndex;
        }
        protected void grdWorkorderPending_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdWorkorderPending.PageIndex;
            DataTable dt;
            if ((DataTable)ViewState["Singleworkorder"] != null)
            {
                dt = (DataTable)ViewState["Singleworkorder"];
            }
            else
            {
                dt = (DataTable)ViewState["workorder"];
            }

            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdWorkorderPending.DataSource = SortDataTableWorkorder(dt as DataTable, false);
            }
            else
            {
                grdWorkorderPending.DataSource = dt;
            }
            grdWorkorderPending.DataBind();
            grdWorkorderPending.PageIndex = pageIndex;
        }
        protected void grdIndentPending_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdIndentPending.PageIndex;
            DataTable dt = (DataTable)ViewState["indent"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdIndentPending.DataSource = SortDataTableIndent(dt as DataTable, false);
            }
            else
            {
                grdIndentPending.DataSource = dt;
            }

            grdIndentPending.DataBind();
            grdIndentPending.PageIndex = pageIndex;
        }
        protected void grdinvoicePending_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdinvoicePending.PageIndex;
            DataTable dt = (DataTable)ViewState["invoice"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {

                grdinvoicePending.DataSource = SortDataTableCommission(dt as DataTable, false);
            }
            else
            {
                grdinvoicePending.DataSource = dt;
            }
            grdinvoicePending.DataBind();
            grdinvoicePending.PageIndex = pageIndex;
        }
        protected void grdDecommissionPending_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdDecommissionPending.PageIndex;
            DataTable dt = (DataTable)ViewState["invoice"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdDecommissionPending.DataSource = SortDataTableDecommission(dt as DataTable, false);
            }
            else
            {
                grdDecommissionPending.DataSource = dt;
            }
            grdDecommissionPending.DataBind();
            grdDecommissionPending.PageIndex = pageIndex;
        }
        protected void grdRIPending_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdRIPending.PageIndex;
            DataTable dt = (DataTable)ViewState["RI"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdRIPending.DataSource = SortDataTableRi(dt as DataTable, false);
            }

            else
            {
                grdRIPending.DataSource = dt;

            }
            grdRIPending.DataBind();
            grdRIPending.PageIndex = pageIndex;
        }

        protected void grdCRPending_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdCRPending.PageIndex;
            DataTable dt = (DataTable)ViewState["CR"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdCRPending.DataSource = SortDataTableRi(dt as DataTable, false);
            }

            else
            {
                grdCRPending.DataSource = dt;

            }
            grdCRPending.DataBind();
            grdCRPending.PageIndex = pageIndex;
        }

        protected DataView SortDataTablePending(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        ViewState["FailurePending"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["FailurePending"] = dataView.ToTable();


                    }
                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }
        protected DataView SortDataTableCondition(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        ViewState["Condition"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["Condition"] = dataView.ToTable();


                    }
                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }

        protected DataView SortDataTableFailureApprovalPending(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        ViewState["FailurePendingApproval"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["FailurePendingApproval"] = dataView.ToTable();


                    }
                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }
        protected DataView SortDataTableEstimation(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        ViewState["estimation"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["estimation"] = dataView.ToTable();


                    }
                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }
        protected DataView SortDataTableWorkorder(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        ViewState["workorder"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["workorder"] = dataView.ToTable();


                    }
                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }
        protected DataView SortDataTableIndent(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        ViewState["indent"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["indent"] = dataView.ToTable();


                    }
                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }
        protected DataView SortDataTableCommission(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        ViewState["invoice"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["invoice"] = dataView.ToTable();


                    }
                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }
        protected DataView SortDataTableDecommission(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        ViewState["invoice"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["invoice"] = dataView.ToTable();


                    }
                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }
        protected DataView SortDataTableRi(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        ViewState["RI"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["RI"] = dataView.ToTable();


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

        protected void Export_clickFailurePendingOverview(object sender, EventArgs e)
        {
            clsDashboard objDashboard = new clsDashboard();
            DataTable dt = new DataTable();


            if (Request.QueryString["value"] == "Failure")
            {
                //dt = objDashboard.LoadFailurePendingDetails(hdfOffCode.Value);
                dt = (DataTable)ViewState["FailurePending"];
                //dt.Clear();
                if (dt.Rows.Count > 0)
                {

                    dt.Columns["DT_CODE"].ColumnName = "TRANSFORMER CENTRE CODE";
                    dt.Columns["DT_NAME"].ColumnName = "TRANSFORMER CENTRE NAME";
                    dt.Columns["OMSECTION"].ColumnName = "SECTION NAME";
                    dt.Columns["SUBDIVSION"].ColumnName = "SUBDIVSION NAME";
                    dt.Columns["DF_ID"].ColumnName = "FAILURE ID";
                    dt.Columns["DF_DATE"].ColumnName = "FAILURE DATE";
                    dt.Columns["DIVSION"].ColumnName = "DIVSION NAME";
                    dt.Columns["FL_STATUS"].ColumnName = "FAILURE STATUS";

                    List<string> listtoRemove = new List<string> { "OM_CODE" };
                    string filename = "Failure" + DateTime.Now + ".xls";
                    string pagetitle = "Failure Pending Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }

            if (Request.QueryString["value"] == "ReceiveTC")
            {
                dt = (DataTable)ViewState["ReceiveTC"];
                //dt.Clear();
                if (dt.Rows.Count > 0)
                {

                    dt.Columns["DT_CODE"].ColumnName = "TRANSFORMER CENTRE CODE";
                    dt.Columns["DT_NAME"].ColumnName = "TRANSFORMER CENTRE NAME";
                    dt.Columns["DF_EQUIPMENT_ID"].ColumnName = "DTr Code";
                    dt.Columns["WO_NO"].ColumnName = "WO NO";
                    dt.Columns["WO_DATE"].ColumnName = "WO DATE";
                    dt.Columns["OMSECTION"].ColumnName = "OMSECTION";
                    dt.Columns["SUBDIVSION"].ColumnName = "SUBDIVSION";
                    // dt.Columns["DF_DATE"].ColumnName = "WO Date";

                    List<string> listtoRemove = new List<string> { "DF_ID", "DF_DATE" };
                    string filename = "ReceiveTC" + DateTime.Now + ".xls";
                    string pagetitle = "Receive TC Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }
            }
            if (Request.QueryString["value"] == "FailurePendingApproval")
            {
                dt = (DataTable)ViewState["FailurePendingApproval"];
                if (dt.Rows.Count > 0)
                {
                    dt.Columns["DT_CODE"].ColumnName = "TRANSFORMER CENTRE CODE";
                    dt.Columns["DT_NAME"].ColumnName = "TRANSFORMER CENTRE NAME";
                    dt.Columns["SUBDIVSION"].ColumnName = "SUBDIVSION NAME";
                    dt.Columns["OMSECTION"].ColumnName = "SECTION NAME";
                    dt.Columns["FL_STATUS"].ColumnName = "FAILURE PENDING STATUS";

                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "FailurePending" + DateTime.Now + ".xls";
                    string pagetitle = "Failure Pending Details";
                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }
            }

            if (Request.QueryString["value"] == "estimation")
            {
                //dt = objDashboard.LoadEstimationPendingDetails(hdfOffCode.Value);
                dt = (DataTable)ViewState["estimation"];
                if (dt.Rows.Count > 0)
                {

                    dt.Columns["DT_CODE"].ColumnName = "TRANSFORMER CENTRE CODE";
                    dt.Columns["DT_NAME"].ColumnName = "TRANSFORMER CENTRE NAME";
                    dt.Columns["SUBDIVSION"].ColumnName = "SUBDIVSION NAME";
                    dt.Columns["OMSECTION"].ColumnName = "SECTION NAME";
                    dt.Columns["DIVSION"].ColumnName = "DIVSION NAME";
                    dt.Columns["DF_DATE"].ColumnName = "FAILURE DATE";
                    dt.Columns["DF_EQUIPMENT_ID"].ColumnName = "DTr CODE";
                    dt.Columns["DAYS_FROM_PENDING"].ColumnName = "ESTIMATION STATUS";

                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "estimation" + DateTime.Now + ".xls";
                    string pagetitle = "Estimation Pending Details";


                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }

            if (Request.QueryString["value"] == "Singleworkorder")
            {
                // dt = objDashboard.LoadWorkorderPendingDetails(hdfOffCode.Value);
                dt = (DataTable)ViewState["Singleworkorder"];
                if (dt.Rows.Count > 0)
                {

                    dt.Columns["DT_CODE"].ColumnName = "TRANSFORMER CENTRE CODE";
                    dt.Columns["DT_NAME"].ColumnName = "TRANSFORMER CENTRE NAME";
                    dt.Columns["DF_EQUIPMENT_ID"].ColumnName = "DTr CODE";
                    dt.Columns["SUBDIVSION"].ColumnName = "SUBDIVSION NAME";
                    dt.Columns["OMSECTION"].ColumnName = "SECTION NAME";
                    dt.Columns["DF_DATE"].ColumnName = "WORK ORDER DATE";
                    dt.Columns["DAYS_FROM_PENDING"].ColumnName = "WORK ORDER STATUS";
                    // dt.Columns["WO_NO"].ColumnName = "WORK ORDER NO";

                    //dt.Columns["WORK ORDER NO"].SetOrdinal(6);

                    List<string> listtoRemove = new List<string> { "DF_ID" };
                    string filename = "workorder" + DateTime.Now + ".xls";
                    string pagetitle = "Workorder Pending Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }

            if (Request.QueryString["value"] == "workorder")
            {
                // dt = objDashboard.LoadWorkorderPendingDetails(hdfOffCode.Value);
                dt = (DataTable)ViewState["workorder"];
                if (dt.Rows.Count > 0)
                {

                    dt.Columns["DT_CODE"].ColumnName = "TRANSFORMER CENTRE CODE";
                    dt.Columns["DT_NAME"].ColumnName = "TRANSFORMER CENTRE NAME";
                    dt.Columns["OMSECTION"].ColumnName = "SECTION NAME";
                    dt.Columns["SUBDIVSION"].ColumnName = "SUBDIVSION NAME";
                    dt.Columns["DIVSION"].ColumnName = "DIVSION NAME";
                    dt.Columns["DF_ID"].ColumnName = "FAILURE ID";
                    dt.Columns["DF_DATE"].ColumnName = "FAILURE DATE";
                    dt.Columns["DF_EQUIPMENT_ID"].ColumnName = "DTr CODE";
                    dt.Columns["DAYS_FROM_PENDING"].ColumnName = "WORK ORDER STATUS";

                    //dt.Columns["WORK ORDER NO"].SetOrdinal(6);

                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "workorder" + DateTime.Now + ".xls";
                    string pagetitle = "Workorder Pending Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }

            if (Request.QueryString["value"] == "indent")
            {
                //dt = objDashboard.LoadIndentPendingDetails(hdfOffCode.Value);
                dt = (DataTable)ViewState["indent"];
                if (dt.Rows.Count > 0)
                {

                    dt.Columns["DT_CODE"].ColumnName = "TRANSFORMER CENTRE CODE";
                    dt.Columns["DT_NAME"].ColumnName = "TRANSFORMER CENTRE NAME";
                    dt.Columns["SUBDIVSION"].ColumnName = "SUBDIVSION NAME";
                    dt.Columns["OMSECTION"].ColumnName = "SECTION NAME";
                    dt.Columns["DF_DATE"].ColumnName = "FAILURE DATE";
                    dt.Columns["DAYS_FROM_PENDING"].ColumnName = "INDENT STATUS";

                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "indent" + DateTime.Now + ".xls";
                    string pagetitle = "Indent Pending Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }

            if (Request.QueryString["value"] == "invoice")
            {
                // dt = objDashboard.LoadInvoicePendingDetails(hdfOffCode.Value);
                dt = (DataTable)ViewState["invoice"];
                if (dt.Rows.Count > 0)
                {

                    dt.Columns["DT_CODE"].ColumnName = "TRANSFORMER CENTRE CODE";
                    dt.Columns["DT_NAME"].ColumnName = "TRANSFORMER CENTRE NAME";
                    dt.Columns["SUBDIVSION"].ColumnName = "SUBDIVSION NAME";
                    dt.Columns["OMSECTION"].ColumnName = "SECTION NAME";
                    dt.Columns["DIVSION"].ColumnName = "DIVSION NAME";
                    dt.Columns["DF_DATE"].ColumnName = "FAILURE DATE";
                    dt.Columns["DF_EQUIPMENT_ID"].ColumnName = "DTr CODE";
                    dt.Columns["WO_NO"].ColumnName = "WORKORDER NUMBER";
                    dt.Columns["WO_DATE"].ColumnName = "WORKORDER DATE";
                    dt.Columns["DF_ID"].ColumnName = "FAILURE ID";
                    dt.Columns["DAYS_FROM_PENDING"].ColumnName = "INVOICE STATUS";

                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "invoice" + DateTime.Now + ".xls";
                    string pagetitle = "Invoice Pending Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }

            if (Request.QueryString["value"] == "DeCommission")
            {
                // dt = objDashboard.LoadDeCommissionPendingDetails(hdfOffCode.Value);
                dt = (DataTable)ViewState["DeCommission"];
                if (dt.Rows.Count > 0)
                {

                    dt.Columns["DT_CODE"].ColumnName = "TRANSFORMER CENTRE CODE";
                    dt.Columns["DT_NAME"].ColumnName = "TRANSFORMER CENTRE NAME";
                    dt.Columns["SUBDIVSION"].ColumnName = "SUBDIVSION NAME";
                    dt.Columns["OMSECTION"].ColumnName = "SECTION NAME";
                    dt.Columns["DAYS_FROM_PENDING"].ColumnName = "DECOMM STATUS";


                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "DeCommission" + DateTime.Now + ".xls";
                    string pagetitle = "Invoice Pending Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }

            if (Request.QueryString["value"] == "RI")
            {
                // dt = objDashboard.LoadRIPendingDetails(hdfOffCode.Value);
                dt = (DataTable)ViewState["RI"];
                if (dt.Rows.Count > 0)
                {

                    dt.Columns["DT_CODE"].ColumnName = "TRANSFORMER CENTRE CODE";
                    dt.Columns["DT_NAME"].ColumnName = "TRANSFORMER CENTRE NAME";
                    dt.Columns["SUBDIVSION"].ColumnName = "SUBDIVSION NAME";
                    dt.Columns["OMSECTION"].ColumnName = "SECTION NAME";
                    dt.Columns["DIVSION"].ColumnName = "DIVSION NAME";
                    dt.Columns["DF_EQUIPMENT_ID"].ColumnName = "FAULTY DTr";
                    dt.Columns["DT_TC_ID"].ColumnName = "INVOICED DTr";
                    //dt.Columns["TR_RI_DATE"].ColumnName = "RI DATE";
                    dt.Columns["DAYS_FROM_PENDING"].ColumnName = "RI STATUS";
                    // dt.Columns["CR_DAYS_FROM_PENDING"].ColumnName = "CR STATUS";

                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "RI" + DateTime.Now + ".xls";
                    string pagetitle = "RI  Pending Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }

            if (Request.QueryString["value"] == "CR")
            {
                // dt = objDashboard.LoadRIPendingDetails(hdfOffCode.Value);
                dt = (DataTable)ViewState["CR"];
                if (dt.Rows.Count > 0)
                {

                    dt.Columns["DT_CODE"].ColumnName = "TRANSFORMER CENTRE CODE";
                    dt.Columns["DT_NAME"].ColumnName = "TRANSFORMER CENTRE NAME";
                    dt.Columns["SUBDIVSION"].ColumnName = "SUBDIVSION NAME";
                    dt.Columns["OMSECTION"].ColumnName = "SECTION NAME";
                    dt.Columns["DIVSION"].ColumnName = "DIVSION NAME";
                    //dt.Columns["DF_EQUIPMENT_ID"].ColumnName = "INVOICED DTR";
                    //dt.Columns["DT_TC_ID"].ColumnName = "FAILURE DTR";
                    dt.Columns["DF_EQUIPMENT_ID"].ColumnName = "FAILURE DTr";
                    dt.Columns["DT_TC_ID"].ColumnName = "INVOICED DTr";
                    //dt.Columns["TR_RI_DATE"].ColumnName = "RI DATE";
                    // dt.Columns["DAYS_FROM_PENDING"].ColumnName = "RI STATUS";
                    dt.Columns["CR_DAYS_FROM_PENDING"].ColumnName = "CR STATUS";

                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "CR" + DateTime.Now + ".xls";
                    string pagetitle = "CR  Pending Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }

            if (Request.QueryString["value"] == "InvoiceTCDetails")
            {
                dt = (DataTable)ViewState["TCdetails"];
                string sStoreName = Convert.ToString(dt.Rows[0]["SM_NAME"]);
                string sCapacity = Convert.ToString(dt.Rows[0]["TC_CAPACITY"]);
                dt.Columns.Remove("SM_NAME");
                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "DTr CODE";
                    dt.Columns["TC_SLNO"].ColumnName = "DTr SLNO";
                    dt.Columns["TC_CAPACITY"].ColumnName = "DTr CAPACITY";
                    dt.Columns["STATUS"].ColumnName = "STATUS";

                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "DTRDetailsInStore" + DateTime.Now + ".xls";
                    string pagetitle = "DTr Details of " + sCapacity + " Capacity in " + sStoreName + " Store";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }
            }
            if (Request.QueryString["value"] == "NewTCcount")
            {

                dt = (DataTable)ViewState["NewTCcount"];

                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "TRANSFORMER CODE";
                    dt.Columns["TC_SLNO"].ColumnName = "TRANSFORMER SLNO";
                    dt.Columns["TC_CAPACITY"].ColumnName = "CAPACITY";
                    dt.Columns["TM_NAME"].ColumnName = "MAKE";
                    dt.Columns["TC_RATING"].ColumnName = "RATING";


                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "NewTC" + DateTime.Now + ".xls";
                    string pagetitle = "Condition Of New Transformer Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }
            if (Request.QueryString["value"] == "RepairTCcount")
            {

                dt = (DataTable)ViewState["RepairTCcount"];

                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "TRANSFORMER CODE";
                    dt.Columns["TC_SLNO"].ColumnName = "TRANSFORMER SLNO";
                    dt.Columns["TC_CAPACITY"].ColumnName = "CAPACITY";
                    dt.Columns["TM_NAME"].ColumnName = "MAKE";
                    dt.Columns["TC_RATING"].ColumnName = "RATING";


                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "Repair" + DateTime.Now + ".xls";
                    string pagetitle = "Condition Of Repair Transformer Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }
            if (Request.QueryString["value"] == "ReleaseTCcount")
            {

                dt = (DataTable)ViewState["ReleaseTCcount"];

                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "TRANSFORMER CODE";
                    dt.Columns["TC_SLNO"].ColumnName = "TRANSFORMER SLNO";
                    dt.Columns["TC_CAPACITY"].ColumnName = "CAPACITY";
                    dt.Columns["TM_NAME"].ColumnName = "MAKE";
                    dt.Columns["TC_RATING"].ColumnName = "RATING";


                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "ReleaseTC" + DateTime.Now + ".xls";
                    string pagetitle = "Condition Of Release Transformer Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }
            if (Request.QueryString["value"] == "FaultyTCcount")
            {

                dt = (DataTable)ViewState["FaultyTCcount"];

                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "TRANSFORMER CODE";
                    dt.Columns["TC_SLNO"].ColumnName = "TRANSFORMER SLNO";
                    dt.Columns["TC_CAPACITY"].ColumnName = "CAPACITY";
                    dt.Columns["TM_NAME"].ColumnName = "MAKE";
                    dt.Columns["TC_RATING"].ColumnName = "RATING";


                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "FaultyTC" + DateTime.Now + ".xls";
                    string pagetitle = "Condition Of Faluty Transformer Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }
            if (Request.QueryString["value"] == "mobileTCcount")
            {

                dt = (DataTable)ViewState["mobileTCcount"];

                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "TRANSFORMER CODE";
                    dt.Columns["TC_SLNO"].ColumnName = "TRANSFORMER SLNO";
                    dt.Columns["TC_CAPACITY"].ColumnName = "CAPACITY";
                    dt.Columns["TM_NAME"].ColumnName = "MAKE";
                    dt.Columns["TC_RATING"].ColumnName = "RATING";


                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "ScrapTC" + DateTime.Now + ".xls";
                    string pagetitle = "Condition Of Scrap Transformer Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }
            if (Request.QueryString["value"] == "TCless25_count")
            {

                dt = (DataTable)ViewState["TCless25_count"];

                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "DTC CODE";
                    dt.Columns["TC_SLNO"].ColumnName = "DTC SLNO";
                    dt.Columns["TC_CAPACITY"].ColumnName = "CAPACITY";
                    dt.Columns["TM_NAME"].ColumnName = "MAKE";
                    dt.Columns["TC_RATING"].ColumnName = "RATING";


                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "TCcapacity25" + DateTime.Now + ".xls";
                    string pagetitle = "Details Of TC with Capacity <25";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }
            if (Request.QueryString["value"] == "TC25_100_count")
            {

                dt = (DataTable)ViewState["TC25_100_count"];

                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "DTC CODE";
                    dt.Columns["TC_SLNO"].ColumnName = "DTC SLNO";
                    dt.Columns["TC_CAPACITY"].ColumnName = "CAPACITY";
                    dt.Columns["TM_NAME"].ColumnName = "MAKE";
                    dt.Columns["TC_RATING"].ColumnName = "RATING";


                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "TCcapacity25_100" + DateTime.Now + ".xls";
                    string pagetitle = "Details Of TC with Capacity 25-100";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }
            if (Request.QueryString["value"] == "TC125_250_count")
            {

                dt = (DataTable)ViewState["TC125_250_count"];

                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "DTC CODE";
                    dt.Columns["TC_SLNO"].ColumnName = "DTC SLNO";
                    dt.Columns["TC_CAPACITY"].ColumnName = "CAPACITY";
                    dt.Columns["TM_NAME"].ColumnName = "MAKE";
                    dt.Columns["TC_RATING"].ColumnName = "RATING";


                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "TCcapacity125_250" + DateTime.Now + ".xls";
                    string pagetitle = "Details Of TC with Capacity 125-250";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }
            if (Request.QueryString["value"] == "TCgreater250_count")
            {

                dt = (DataTable)ViewState["TCgreater250_count"];

                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "DTC CODE";
                    dt.Columns["TC_SLNO"].ColumnName = "DTC SLNO";
                    dt.Columns["TC_CAPACITY"].ColumnName = "CAPACITY";
                    dt.Columns["TM_NAME"].ColumnName = "MAKE";
                    dt.Columns["TC_RATING"].ColumnName = "RATING";


                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "TCcapacitygreater250" + DateTime.Now + ".xls";
                    string pagetitle = "Details Of TC with Capacity >250";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }
            if (Request.QueryString["value"] == "TCpending_issue_count")
            {

                dt = (DataTable)ViewState["TCpending_issue_count"];

                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "DTC CODE";
                    dt.Columns["TC_SLNO"].ColumnName = "DTC SLNO";
                    dt.Columns["TC_CAPACITY"].ColumnName = "CAPACITY";
                    dt.Columns["TM_NAME"].ColumnName = "MAKE";
                    dt.Columns["TC_RATING"].ColumnName = "RATING";


                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "TCpendingIssue" + DateTime.Now + ".xls";
                    string pagetitle = " Number of TC Pending For Issue";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }
            if (Request.QueryString["value"] == "TCpending_repair_count")
            {

                dt = (DataTable)ViewState["TCpending_repair_count"];

                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "DTC CODE";
                    dt.Columns["TC_SLNO"].ColumnName = "DTC SLNO";
                    dt.Columns["TC_CAPACITY"].ColumnName = "CAPACITY";
                    dt.Columns["TM_NAME"].ColumnName = "MAKE";
                    dt.Columns["TC_RATING"].ColumnName = "RATING";


                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "TCpendingRepair" + DateTime.Now + ".xls";
                    string pagetitle = "Number of TC Pending For Repair";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }
            if (Request.QueryString["value"] == "TCpending_release_count")
            {

                dt = (DataTable)ViewState["TCpending_release_count"];

                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "DTC CODE";
                    dt.Columns["TC_SLNO"].ColumnName = "DTC SLNO";
                    dt.Columns["TC_CAPACITY"].ColumnName = "CAPACITY";
                    dt.Columns["TM_NAME"].ColumnName = "MAKE";
                    dt.Columns["TC_RATING"].ColumnName = "RATING";


                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "TCpendingRelease" + DateTime.Now + ".xls";
                    string pagetitle = "Number of TC Pending For Release";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }
            if (Request.QueryString["value"] == "TcGoodCount")
            {

                dt = (DataTable)ViewState["TcGoodCount"];

                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "DTC CODE";
                    dt.Columns["TC_SLNO"].ColumnName = "DTC SLNO";
                    dt.Columns["TC_CAPACITY"].ColumnName = "CAPACITY";
                    dt.Columns["TM_NAME"].ColumnName = "MAKE";
                    dt.Columns["TC_RATING"].ColumnName = "RATING";


                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "TcGoodCount" + DateTime.Now + ".xls";
                    string pagetitle = "Number of GOOD TC ";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }
            if (Request.QueryString["value"] == "TcReleaseGoodCount")
            {

                dt = (DataTable)ViewState["TcReleaseGoodCount"];

                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "DTC CODE";
                    dt.Columns["TC_SLNO"].ColumnName = "DTC SLNO";
                    dt.Columns["TC_CAPACITY"].ColumnName = "CAPACITY";
                    dt.Columns["TM_NAME"].ColumnName = "MAKE";
                    dt.Columns["TC_RATING"].ColumnName = "RATING";


                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "TcReleaseGoodCount" + DateTime.Now + ".xls";
                    string pagetitle = "Number of RELEASE GOOD TC ";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }
            if (Request.QueryString["value"] == "TcRepairGoodCount")
            {

                dt = (DataTable)ViewState["TcRepairGoodCount"];

                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "DTC CODE";
                    dt.Columns["TC_SLNO"].ColumnName = "DTC SLNO";
                    dt.Columns["TC_CAPACITY"].ColumnName = "CAPACITY";
                    dt.Columns["TM_NAME"].ColumnName = "MAKE";
                    dt.Columns["TC_RATING"].ColumnName = "RATING";


                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "TcRepairGoodCount" + DateTime.Now + ".xls";
                    string pagetitle = "Number of REPAIR GOOD TC ";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }


            if (Request.QueryString["value"] == "TotalDTRDetails" || Request.QueryString["value"] == "TotalFieldDTRDetails" || Request.QueryString["value"] == "TotalBankDTRDetails" || Request.QueryString["value"] == "TotalStoreDTRDetails" || Request.QueryString["value"] == "TotalRepairerDTRDetails")
            {
                //dt=objDashboard
                if (Request.QueryString["value"] == "TotalDTRDetails")
                {

                    dt = LoadDtrDetailsexcel(hdfOffCode.Value, "0");
                }
                if (Request.QueryString["value"] == "TotalFieldDTRDetails")
                {
                    dt = LoadDtrDetailsexcel(hdfOffCode.Value, "2");
                }
                if (Request.QueryString["value"] == "TotalBankDTRDetails")
                {
                    dt = LoadDtrDetailsexcel(hdfOffCode.Value, "5");
                }
                if (Request.QueryString["value"] == "TotalStoreDTRDetails")
                {
                    dt = LoadDtrDetailsexcel(hdfOffCode.Value, "1");
                }
                if (Request.QueryString["value"] == "TotalRepairerDTRDetails")
                {
                    dt = LoadDtrDetailsexcel(hdfOffCode.Value, "3");
                }



                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "TRANSFORMER CODE";
                    dt.Columns["TC_SLNO"].ColumnName = "TRANSFORMER SERIAL NUM";
                    dt.Columns["TC_RATING"].ColumnName = "TC RATING";
                    dt.Columns["TC_CAPACITY"].ColumnName = "TRANSFORMER CAPACITY";
                    dt.Columns["TM_NAME"].ColumnName = "MAKE NAME";
                    // dt.Columns["DF_DATE"].ColumnName = "FAILURE DATE";

                    List<string> listtoRemove = new List<string> { "TC_ID" };
                    string filename = "DTR Details" + DateTime.Now + ".xls";
                    string pagetitle = "DTr Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

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

        protected void grdInvoiceTCDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txttccode = (TextBox)row.FindControl("TxtTcCode");
                    TextBox txtslno = (TextBox)row.FindControl("txtTCSLNO");

                    DataTable dt = (DataTable)ViewState["TCdetails"];
                    dv = dt.DefaultView;

                    if (txttccode.Text != "")
                    {
                        sFilter = "TC_CODE = '" + txttccode.Text.Replace("'", "'") + "' AND";
                    }
                    if (txtslno.Text != "")
                    {
                        sFilter += " TC_SLNO Like '%" + txtslno.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdInvoiceTCDetails.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdInvoiceTCDetails.DataSource = dv;
                            ViewState["TCdetails"] = dv.ToTable();
                            grdInvoiceTCDetails.DataBind();

                        }
                        else
                        {
                            ViewState["TCdetails"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailurePendingDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdInvoiceTCDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdInvoiceTCDetails.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["TCdetails"];
                grdInvoiceTCDetails.DataSource = dtComplete;
                grdInvoiceTCDetails.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdTotalDTRDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtDTRDetails = new DataTable();
                grdTotalDTRDetails.PageIndex = e.NewPageIndex;
                dtDTRDetails = (DataTable)ViewState["DTRDetails"];
                grdTotalDTRDetails.DataSource = SortDataTableDecommission(dtDTRDetails as DataTable, true);
                grdTotalDTRDetails.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected DataView SortDataTableDTRDetails(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        ViewState["DTRDetails"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["DTRDetails"] = dataView.ToTable();


                    }
                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }

        protected void grdTotalDTRDetails_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdTotalDTRDetails.PageIndex;
            DataTable dt = (DataTable)ViewState["DTRDetails"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdTotalDTRDetails.DataSource = SortDataTableDTRDetails(dt as DataTable, false);
            }
            else
            {
                grdTotalDTRDetails.DataSource = dt;
            }
            grdTotalDTRDetails.DataBind();
            grdTotalDTRDetails.PageIndex = pageIndex;
        }

        /// <summary>
        /// Used by 3 Cards Good, Relese Good & Repair Good
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdTransformerInBank_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                LoadFailurePendingDetails();
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();
                    DataTable dt = new DataTable();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtrCode = (TextBox)row.FindControl("txtDtrCode");
                    TextBox txtDtrslno = (TextBox)row.FindControl("txtDtrslno");
                    
                    switch (Request.QueryString["value"])
                    {
                        case "TcGoodCount":
                            dt = (DataTable)ViewState["TcGoodCount"];
                            break;
                        case "TcReleaseGoodCount":
                            dt = (DataTable)ViewState["TcReleaseGoodCount"];
                            break;
                        case "TcRepairGoodCount":
                            dt = (DataTable)ViewState["TcRepairGoodCount"];
                            break;
                    }

                    dv = dt.DefaultView;

                    if (objSession.OfficeCode == "" || objSession.OfficeCode == null)
                    {
                        if (Request.QueryString["value"] == "TotalDTRDetails")
                        {
                            LoadDtrDetailssearch(hdfOffCode.Value, "0", txtDtrCode.Text.Replace("'", "'").Trim(), txtDtrslno.Text.Replace("'", "'").Trim());
                        }
                        if (Request.QueryString["value"] == "TotalFieldDTRDetails")
                        {
                            LoadDtrDetailssearch(hdfOffCode.Value, "2", txtDtrCode.Text.Replace("'", "'").Trim(), txtDtrslno.Text.Replace("'", "'").Trim());
                        }
                        if (Request.QueryString["value"] == "TotalBankDTRDetails")
                        {
                            LoadDtrDetailssearch(hdfOffCode.Value, "5", txtDtrCode.Text.Replace("'", "'").Trim(), txtDtrslno.Text.Replace("'", "'").Trim());
                        }
                        if (Request.QueryString["value"] == "TotalStoreDTRDetails")
                        {
                            LoadDtrDetailssearch(hdfOffCode.Value, "1", txtDtrCode.Text.Replace("'", "'").Trim(), txtDtrslno.Text.Replace("'", "'").Trim());
                        }
                        if (Request.QueryString["value"] == "TotalRepairerDTRDetails")
                        {
                            LoadDtrDetailssearch(hdfOffCode.Value, "3", txtDtrCode.Text.Replace("'", "'").Trim(), txtDtrslno.Text.Replace("'", "'").Trim());
                        }
                    }
                    else
                    {
                        if (txtDtrCode.Text != "")
                        {
                            sFilter = "TC_CODE Like '%" + txtDtrCode.Text.Replace("'", "'").Trim() + "%' AND";
                        }
                        if (txtDtrslno.Text != "")
                        {
                            sFilter += " TC_SLNO Like '%" + txtDtrslno.Text.Replace("'", "'") + "%' AND";
                        }
                        if (sFilter.Length > 0)
                        {
                            sFilter = sFilter.Remove(sFilter.Length - 3);
                            switch (Request.QueryString["value"])
                            {
                                case "TcGoodCount":
                                    GridTcGood.PageIndex = 0;
                                    break;
                                case "TcReleaseGoodCount":
                                    GridTcReleaseGood.PageIndex = 0;
                                    break;
                                case "TcRepairGoodCount":
                                    GridRepairGood.PageIndex = 0;
                                    break;
                            }                            
                            dv.RowFilter = string.Format(sFilter);
                            if (dv.Count > 0)
                            {                               
                                switch (Request.QueryString["value"])
                                {
                                    case "TcGoodCount":
                                        GridTcGood.DataSource = dv;
                                        ViewState["TcGoodCount"] = dv.ToTable();
                                        GridTcGood.DataBind();
                                        break;

                                    case "TcReleaseGoodCount":
                                        GridTcReleaseGood.DataSource = dv;
                                        ViewState["TcReleaseGoodCount"] = dv.ToTable();
                                        GridTcReleaseGood.DataBind();
                                        break;

                                    case "TcRepairGoodCount":
                                        GridRepairGood.DataSource = dv;
                                        ViewState["TcRepairGoodCount"] = dv.ToTable();
                                        GridRepairGood.DataBind();
                                        break;
                                }
                            }
                            else
                            {                                                               
                                switch (Request.QueryString["value"])
                                {
                                    case "TcGoodCount":
                                        ViewState["TcGoodCount"] = dv.ToTable();                                        
                                        break;

                                    case "TcReleaseGoodCount":
                                        ViewState["TcReleaseGoodCount"] = dv.ToTable();                                        
                                        break;

                                    case "TcRepairGoodCount":
                                        ViewState["TcRepairGoodCount"] = dv.ToTable();                                       
                                        break;
                                }
                                ShowEmptyGrid();
                            }
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdTotalDTRDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                LoadFailurePendingDetails();
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();
                    DataTable dt = new DataTable();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtrCode = (TextBox)row.FindControl("txtDtrCode");
                    TextBox txtDtrslno = (TextBox)row.FindControl("txtDtrslno");

                    dt = (DataTable)ViewState["DTRDetails"];
                    dv = dt.DefaultView;


                    if (objSession.OfficeCode == "" || objSession.OfficeCode == null)
                    {
                        if (Request.QueryString["value"] == "TotalDTRDetails")
                        {
                            LoadDtrDetailssearch(hdfOffCode.Value, "0", txtDtrCode.Text.Replace("'", "'").Trim(), txtDtrslno.Text.Replace("'", "'").Trim());
                        }
                        if (Request.QueryString["value"] == "TotalFieldDTRDetails")
                        {
                            LoadDtrDetailssearch(hdfOffCode.Value, "2", txtDtrCode.Text.Replace("'", "'").Trim(), txtDtrslno.Text.Replace("'", "'").Trim());
                        }
                        if (Request.QueryString["value"] == "TotalBankDTRDetails")
                        {
                            LoadDtrDetailssearch(hdfOffCode.Value, "5", txtDtrCode.Text.Replace("'", "'").Trim(), txtDtrslno.Text.Replace("'", "'").Trim());
                        }
                        if (Request.QueryString["value"] == "TotalStoreDTRDetails")
                        {
                            LoadDtrDetailssearch(hdfOffCode.Value, "1", txtDtrCode.Text.Replace("'", "'").Trim(), txtDtrslno.Text.Replace("'", "'").Trim());
                        }
                        if (Request.QueryString["value"] == "TotalRepairerDTRDetails")
                        {
                            LoadDtrDetailssearch(hdfOffCode.Value, "3", txtDtrCode.Text.Replace("'", "'").Trim(), txtDtrslno.Text.Replace("'", "'").Trim());
                        }
                    }
                    else
                    {

                        if (txtDtrCode.Text != "")
                        {
                            // sFilter = "TC_CODE = '" + txtDtrCode.Text.Replace("'", "'") + "'";
                            sFilter = "TC_CODE Like '%" + txtDtrCode.Text.Replace("'", "'").Trim() + "%' AND";
                        }
                        if (txtDtrslno.Text != "")
                        {
                            sFilter += " TC_SLNO  '%" + txtDtrslno.Text.Replace("'", "'") + "%' AND";
                        }
                        if (sFilter.Length > 0)
                        {
                            sFilter = sFilter.Remove(sFilter.Length - 3);
                            grdTotalDTRDetails.PageIndex = 0;
                            dv.RowFilter = string.Format(sFilter);
                            if (dv.Count > 0)
                            {
                                grdTotalDTRDetails.DataSource = dv;
                                ViewState["DTRDetails"] = dv.ToTable();
                                grdTotalDTRDetails.DataBind();

                            }
                            else
                            {
                                ViewState["DTRDetails"] = dv.ToTable();
                                ShowEmptyGrid();
                            }
                        }
                        else
                        {
                            if (Request.QueryString["value"] == "TotalDTRDetails")
                            {
                                LoadDtrDetails(hdfOffCode.Value, "0");
                            }
                            if (Request.QueryString["value"] == "TotalFieldDTRDetails")
                            {
                                LoadDtrDetails(hdfOffCode.Value, "2");
                            }
                            if (Request.QueryString["value"] == "TotalBankDTRDetails")
                            {
                                LoadDtrDetails(hdfOffCode.Value, "5");
                            }
                            if (Request.QueryString["value"] == "TotalStoreDTRDetails")
                            {
                                LoadDtrDetails(hdfOffCode.Value, "1");
                            }
                            if (Request.QueryString["value"] == "TotalRepairerDTRDetails")
                            {
                                LoadDtrDetails(hdfOffCode.Value, "3");
                            }
                        }

                    }

                }
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadDtrDetailssearch(string sofficeCode, string type, string tccode, string tcslno)
        {
            DataTable dtinfo = new DataTable();

            clsDashboard obj = new clsDashboard();

            dtinfo = obj.GetDtrDetailssearchtcslno(sofficeCode, type, tccode, tcslno);
            ViewState["DTRDetails"] = dtinfo;
            grdTotalDTRDetails.Visible = true;
            grdTotalDTRDetails.DataSource = dtinfo;
            grdTotalDTRDetails.DataBind();
        }

        public DataTable LoadDtrDetailsexcel(string sofficeCode, string type)
        {
            DataTable dtinfo = new DataTable();

            clsDashboard obj = new clsDashboard();

            dtinfo = obj.LoadDtrDetailsexcel(sofficeCode, type);

            return dtinfo;

        }
    }
}