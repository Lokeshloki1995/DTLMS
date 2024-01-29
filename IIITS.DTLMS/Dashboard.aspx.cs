using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.IO;
using System.Data;
using System.Configuration;
using System.Web.UI.DataVisualization.Charting;
using System.Xml;
using System.Net;
using System.Text.RegularExpressions;

namespace IIITS.DTLMS
{
    public partial class Dashboard : System.Web.UI.Page
    {
        string strFormCode = "Dashboard";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                objSession = (clsSession)Session["clsSession"];
                // Session["SessionID"]
                if (Session["clsSession"] != null && Convert.ToString(Session["clsSession"]) != "")
                {
                    if (objSession.sRoleType == "2")
                    {
                        Response.Redirect("~/StoreDashboard.aspx");
                    }
                }

                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {

                    Response.Redirect("~/Login.aspx", false);
                }
                else
                {
                    if (!IsPostBack)
                    {
                        if (objSession != null)
                        {
                            lblLocation.Text = objSession.OfficeName;
                            hdfLocationCode.Value = objSession.OfficeCode;
                        }

                        if (objSession.OfficeCode.Length == Constants.Section)
                        {
                            lnkChange.Visible = false;
                        }

                        Genaral.Load_Combo("SELECT \"MD_NAME\",\"MD_NAME\" from \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='C' ORDER BY \"MD_ORDER_BY\"", "-Select-", cmbCapacity);
                        Genaral.Load_Combo("SELECT \"OM_CODE\", \"OM_CODE\" || '-' || \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE CAST(\"OM_CODE\" AS TEXT) LIKE '" + objSession.OfficeCode + "%' ORDER BY \"OM_CODE\"", "--Select--", cmbSection);

                        //DTC Failure Pending Details
                        GetPendingFailureCount();

                        //Faulty DTR Details
                        GetFaultyDTrCount();

                        GetDTrCount();

                        //Total DTC Count
                        GetTotalDTCCount();

                        //Approval Inbox Item Count
                        GetInboxStatus();

                        // DTC Failure Abstract Capcitywise
                        LoadDTCFailureAbstract();

                        //failure abstract office wise
                        LoadDTCFailureAbstractofficewise();


                    }

                    //DTC Failure Chart
                    LoadFailureChart();

                    //DTC Failure Reson Chart
                    LoadFailureResonChart();

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void DashboardFunctions()
        {
            try
            {
                GetPendingFailureCount();
                GetFaultyDTrCount();
                GetDTrCount();
                LoadFailureChart();
                //added by santhosh
                LoadFailureResonChart();

                GetTotalDTCCount();
                LoadDTCFailureAbstract();
                LoadDTCFailureAbstractofficewise();
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        /// <summary>
        /// DTC Failure Pending Count
        /// </summary>
        public void GetPendingFailureCount()
        {
            try
            {
                clsDashboard objDashboard = new clsDashboard();

                objDashboard.sOfficeCode = hdfLocationCode.Value;
                //int Min = System.DateTime.Now.Minute;
                //int Sec = System.DateTime.Now.Second;
                //objDashboard.sUserId = "tbl" + objSession.UserId + Min + Sec;

                if (objDashboard.sOfficeCode == "0")
                {
                    objDashboard.sOfficeCode = null;
                }


                DataTable dt = objDashboard.getFailurePendingCounts(objDashboard.sOfficeCode);
                DataTable dt1 = objDashboard.getFailureCounts(objDashboard.sOfficeCode);
                //Total Pending Count
                lblToatlPending.Text = Convert.ToString(dt1.Rows[0]["TOTA_DTC_FAILURE"]);

                //Total Failufre Approval Pending
                //lblFailureApprove.Text = Convert.ToString(dt.Rows[0]["FAILURE_APPROVE"]);

                //Pending for Estimation
                lblPendingEstimation.Text = Convert.ToString(dt.Rows[0]["PENDING_ESTIMATION"]);

                // Pending for WorkOrder
                objDashboard.sBOId = "11,74";
                lblPendingWO.Text = Convert.ToString(dt.Rows[0]["PEN_MULTI_COIL_WOR"]);

                //objDashboard.sBOId = "11,74";
                //lblSingleWO.Text = Convert.ToString(dt.Rows[0]["PEN_SINGLE_COIL_WOR"]);

                //objDashboard.sBOId = "46,48";
                //lblReceiveTC.Text = Convert.ToString(dt.Rows[0]["PENDING_RECIEVE_DTR"]);

                //objDashboard.sBOId = "47";
                //lblComission.Text = Convert.ToString(dt.Rows[0]["PENDING_COMMISS"]);

                // Pending for Indent
                //objDashboard.sBOId = "12";
                //lblPendingIndent.Text = Convert.ToString(dt.Rows[0]["PENDING_INDENT"]);


                // Pending for Commission
                objDashboard.sBOId = "13,29";
                lblPendingCommission.Text = Convert.ToString(dt.Rows[0]["PENDING_MAJOR_INV"]);

                //objDashboard.sBOId = "14";
                //lblPendingDeCommission.Text = Convert.ToString(dt.Rows[0]["PENDING_DECOMMI"]);

                // Pending for RI             
                lblPendingRI.Text = Convert.ToString(dt.Rows[0]["PENDING_RI"]);

                lblPendingCR.Text = Convert.ToString(dt.Rows[0]["PENDING_CR"]);

                ////Total Pending Count
                //lblToatlPending.Text = objDashboard.GetTotalPendingCount(objDashboard);

                ////Total Failufre Approval Pending
                //lblFailureApprove.Text = objDashboard.getFailureApprovalPendingCount(objDashboard.sOfficeCode);

                ////Pending for Estimation
                //lblPendingEstimation.Text = objDashboard.GetEstimationPendingCount(objDashboard);

                //// Pending for WorkOrder
                //objDashboard.sBOId = "11";
                //lblPendingWO.Text = objDashboard.GetWOPendingCount(objDashboard);

                //objDashboard.sBOId = "11";
                //lblSingleWO.Text = objDashboard.GetSingleWOPendingCount(objDashboard);

                //objDashboard.sBOId = "46,48";
                //lblReceiveTC.Text = objDashboard.GetReceiveTCPendingCount(objDashboard);

                //objDashboard.sBOId = "47";
                //lblComission.Text = objDashboard.GetComissionPendingCount(objDashboard);
                //// Pending for Indent
                //objDashboard.sBOId = "12";
                //lblPendingIndent.Text = objDashboard.GetIndentPendingCount(objDashboard);


                //// Pending for Commission
                //objDashboard.sBOId = "13,29";
                //lblPendingCommission.Text = objDashboard.GetInvoicePendingCount(objDashboard);

                //objDashboard.sBOId = "14";
                //lblPendingDeCommission.Text = objDashboard.GetDecommissionPendingCount(objDashboard);

                //// Pending for RI             
                //lblPendingRI.Text = objDashboard.GetRIPendingCount(objDashboard);




                //Total Pending Count
                // lblToatlPending.Text = Convert.ToString(Convert.ToInt32(lblPendingEstimation.Text) + Convert.ToInt32(lblPendingWO.Text)
                //  + Convert.ToInt32(lblPendingIndent.Text) + Convert.ToInt32(lblPendingCommission.Text) + Convert.ToInt32(lblPendingRI.Text));


            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        /// <summary>
        /// Faulty DTR Count
        /// </summary>
        public void GetFaultyDTrCount()
        {
            try
            {
                clsDashboard objDashboard = new clsDashboard();

                objDashboard.sOfficeCode = hdfLocationCode.Value;
                if (objDashboard.sOfficeCode == "0")
                {
                    objDashboard.sOfficeCode = null;
                }

                DataTable dtFaultTc = objDashboard.GetFaultyTCFields(objDashboard);
                lblTotalFaulty.Text = Convert.ToString(dtFaultTc.Rows[0]["total_count"]);
                lblFaultyField.Text = Convert.ToString(dtFaultTc.Rows[0]["Field_Count"]);
                lblFaultyRepairer.Text = Convert.ToString(dtFaultTc.Rows[0]["Repair_Count"]);
                LabelTcfailed.Text = Convert.ToString(dtFaultTc.Rows[0]["Repair_Good"]);
                lblFaultyStore.Text = Convert.ToString(dtFaultTc.Rows[0]["Store_count"]);
                //Total Faulty DTR
                //lblTotalFaulty.Text = objDashboard.GetTotalFaultyTC(objDashboard);

                //// Faulty DTR at Field
                //lblFaultyField.Text = "688";// objDashboard.GetFaultyTCField(objDashboard);

                //// Faulty DTR at Store
                //lblFaultyStore.Text = objDashboard.GetFaultyTCStore(objDashboard);

                //// Faulty DTR at Repairer
                //lblFaultyRepairer.Text = objDashboard.GetFaultyTCRepair(objDashboard);

                ////TOTAL repair good TC
                // LabelTcfailed.Text = objDashboard.TotalRepairGoodTc(objDashboard);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetDTrCount()
        {
            try
            {
                clsDashboard objDashboard = new clsDashboard();
                DataTable TotalDTRDetails = new DataTable();
                //DataTable TotalFiledDTRDetails = new DataTable();
                //DataTable TotalStoreDTRDetails = new DataTable();
                //DataTable TotalRepairerDTRDetails = new DataTable();
                DataTable TotalBankDTRDetails = new DataTable();

                objDashboard.sOfficeCode = hdfLocationCode.Value;
                objDashboard.sRoleId = objSession.RoleId;
                if (objDashboard.sOfficeCode == "0")
                {
                    objDashboard.sOfficeCode = null;
                }

                DataTable dtTcDet = objDashboard.GetTcDetails(objDashboard);
                lblTotalDtrDetails.Text = Convert.ToString(dtTcDet.Rows[0]["total_tc"]);
                lblTotalFileldDtr.Text = Convert.ToString(dtTcDet.Rows[0]["field_count"]);
                lblTotalStoreDtr.Text = Convert.ToString(dtTcDet.Rows[0]["TotalStoreDtr"]);
                lblTotalRepairerDtr.Text = Convert.ToString(dtTcDet.Rows[0]["repairer_count"]);

                // DataTable dtTcBank = objDashboard.GetTcBankDetails(objDashboard);
                lblTotalBankDtr.Text = Convert.ToString(dtTcDet.Rows[0]["bank_count"]);
                // lblTotalBankDtr.Text = Convert.ToString(dtTcDet.Rows[0]["bank_count"]);
                //lblTotalDtrDetails.Text = objDashboard.GetDtrCount(objDashboard.sOfficeCode, "0");
                ////ViewState["TotalDTRDetails"] = TotalDTRDetails;
                //lblTotalFileldDtr.Text = objDashboard.GetDtrCount(objDashboard.sOfficeCode, "2");
                ////ViewState["TotalFiledDTRDetails"] = TotalFiledDTRDetails;
                //lblTotalStoreDtr.Text = objDashboard.GetDtrCount(objDashboard.sOfficeCode, "1");
                ////ViewState["TotalStoreDTRDetails"] = TotalStoreDTRDetails;
                //lblTotalRepairerDtr.Text = objDashboard.GetDtrCount(objDashboard.sOfficeCode, "3");
                ////ViewState["TotalRepairerDTRDetails"] = TotalRepairerDTRDetails;
                //lblTotalBankDtr.Text = objDashboard.GetDtrCount(objDashboard.sOfficeCode, "5");
                ////ViewState["TotalBankDTRDetails"] = TotalBankDTRDetails;

                //Total Faulty DTR
                //lblTotalDtrDetails.Text = Convert.ToString(TotalDTRDetails.Rows.Count);

                //// Faulty DTR at Field
                //lblTotalFileldDtr.Text = Convert.ToString(TotalDTRDetails.Rows.Count);

                //// Faulty DTR at Store
                //lblTotalStoreDtr.Text = Convert.ToString(TotalDTRDetails.Rows.Count);

                //// Faulty DTR at Repairer
                //lblTotalRepairerDtr.Text = Convert.ToString(TotalDTRDetails.Rows.Count);

                ////TOTAL repair good TC
                //lblTotalBankDtr.Text = Convert.ToString(TotalDTRDetails.Rows.Count);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }



        /// <summary>
        /// Inbox Status
        /// </summary>
        public void GetInboxStatus()
        {
            try
            {
                clsDashboard objDashboard = new clsDashboard();

                objDashboard.sOfficeCode = objSession.OfficeCode;
                objDashboard.sRoleId = objSession.RoleId;

                //Total Inbox count
                lblTotalWorkflow.Text = objDashboard.GetTotalWorkflow(objDashboard);

                // Pending
                lblPendingWorkflow.Text = objDashboard.GetPendingWorkflow(objDashboard);

                // Approved
                lblApprovedWorkflow.Text = objDashboard.GetApprovedWorkflow(objDashboard);

                // Rejected
                lblRejectedWorkflow.Text = objDashboard.GetRejectedWorkflow(objDashboard);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        #region Change Location

        protected void lnkChange_Click(object sender, EventArgs e)
        {
            try
            {
                LoadOfficeGrid(objSession.OfficeCode);
                this.mdlPopup.Show();

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void lnkChange_Click1(object sender, EventArgs e)
        {
            try
            {
                // string val = hdfLocationCode.Value;

                string val = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                Response.Redirect("/MasterForms/TcMasterView.aspx?Locationcode=" + val + "&URLRedirect=" + true, false);


            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void lnkChange_dtc(object sender, EventArgs e)
        {
            try
            {
                // string val = hdfLocationCode.Value;

                string val = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                Response.Redirect("/MasterForms/DTCView.aspx?Locationcode=" + val + "&URLRedirect=" + true, false);


            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadOfficeGrid(string sOfficeCode = "", string sOffName = "")
        {
            try
            {

                clsFeederMast objFeeder = new clsFeederMast();
                DataTable dt = new DataTable();

                objFeeder.OfficeCode = sOfficeCode;
                objFeeder.OfficeName = sOffName;

                dt = objFeeder.LoadOfficeDet(objFeeder);
                if (dt.Rows.Count > 0)
                {
                    grdOffices.DataSource = dt;
                    grdOffices.DataBind();
                    ViewState["Office"] = dt;
                }
                else
                {
                    ShowEmptyGrid();
                }

            }

            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdOffices_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtOffCode = (TextBox)row.FindControl("txtOffCode");
                    TextBox txtOffName = (TextBox)row.FindControl("txtOffName");

                    LoadOfficeGrid(txtOffCode.Text.Trim().Replace("'", "''"), txtOffName.Text.Trim().Replace("'", "''"));

                    this.mdlPopup.Show();
                    //LoadFailureChart();
                }

                if (e.CommandName == "submit")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    string sOffCode = ((Label)row.FindControl("lblOffCode")).Text;
                    string soffName = ((Label)row.FindControl("lblOffName")).Text;

                    lblLocation.Text = soffName;
                    hdfLocationCode.Value = sOffCode;
                    Session["OffCode"] = sOffCode;
                    if (objSession.RoleId == "12")
                    {
                        objSession.OfficeCode = sOffCode;
                    }
                    ViewState["FailureGraph"] = null;
                    ViewState["FailureReasonGraph"] = null;
                    DashboardFunctions();
                    //LoadFailureChart();

                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdOffices_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdOffices.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Office"];
                grdOffices.DataSource = dt;
                grdOffices.DataBind();

                this.mdlPopup.Show();
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        #endregion

        #region Graph
        public void LoadFailureChart()
        {
            Series series = new Series();
            DataTable dtBarGraph = new DataTable();
            clsDashboard objDashboard = new clsDashboard();

            if (ViewState["FailureGraph"] != null)
            {
                dtBarGraph = (DataTable)ViewState["FailureGraph"];
            }
            else
            {
                objDashboard.sOfficeCode = hdfLocationCode.Value;

                if (objDashboard.sOfficeCode == "0")
                {
                    hdfLocationCode.Value = null;
                }

                dtBarGraph = objDashboard.LoadBarGraph(hdfLocationCode.Value);
                ViewState["FailureGraph"] = dtBarGraph;
            }

            string[] XPointMember = new string[dtBarGraph.Rows.Count];
            string[] XPointMember1 = new string[dtBarGraph.Rows.Count];
            int[] YPointMember = new int[dtBarGraph.Rows.Count];
            int[] YPointMember1 = new int[dtBarGraph.Rows.Count];
            //int[] YPointMember2 = new int[dtBarGraph.Rows.Count];

            for (int i = 0; i < dtBarGraph.Rows.Count; i++)
            {

                XPointMember[i] = Convert.ToString(dtBarGraph.Rows[i]["PRESENTMONTH"]);

                YPointMember[i] = Convert.ToInt32(dtBarGraph.Rows[i]["PRESENTCOUNT"]);
                XPointMember1[i] = Convert.ToString(dtBarGraph.Rows[i]["PREVIOUSMONTH"]);

                YPointMember1[i] = Convert.ToInt32(dtBarGraph.Rows[i]["PREVIOUSCOUNT"]);
            }

            var PresentYear = Convert.ToString(dtBarGraph.Rows[0]["PRESENTYEAR"]);
            var PreviousYear = Convert.ToString(dtBarGraph.Rows[0]["PREVIOUSYEAR"]);

            //series.ChartType = SeriesChartType.Bar;

            Chart1.Series[1].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
            Chart1.Series[0].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
            Chart1.Series[1].ToolTip = "(#VALY)Failure on (#VALX " + PresentYear + ")";
            Chart1.Series[0].ToolTip = "(#VALY)Failure on (#VALX " + PreviousYear + ")";
            Chart1.Series[1].Points.DataBindXY(XPointMember, YPointMember);
            Chart1.Series[0].Points.DataBindXY(XPointMember1, YPointMember1);

            //Chart1.Titles["NewTitle"].Text = "Dtc failured per month";
            //series.Color = System.Drawing.Color.Yellow;


            //Chart1.Series.Add(series);
            Chart1.Series[1]["PixelPointWidth"] = "50";
            Chart1.Series[0]["PixelPointWidth"] = "50";

            //Chart1.ChartAreas[0].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
            //Chart1.Series[0].ChartType = SeriesChartType.Bar;
            //Chart1.Series[1].ChartType = SeriesChartType.Bar;
            Chart1.Width = 1000;

            //Chart1.Series[2].IsVisibleInLegend = false;
            Chart1.Titles["NewTitle"].Text = "DTr Failure/Month";                                  //----------> top name

            Chart1.Series[0].LegendText = "Previous Year";                      //----i think it is small name below the graf
            Chart1.Series[1].LegendText = "Current Year";                       //----i think it is small name below the graf


            Axis xaxis = Chart1.ChartAreas[0].AxisX;
            //xaxis.IntervalOffsetType = DateTimeIntervalType.Months;
            xaxis.Interval = 1;
            Axis yaxis = Chart1.ChartAreas[0].AxisY;
            //yaxis.Interval = 20;

            //Chart1.Series[1].LegendText = "Count of Dtc Failured";
            Chart1.ChartAreas[0].AxisX.Title = "Month";                                            //----------> bottom name

            Chart1.ChartAreas[0].AxisY.Title = "No. of DTr Failure";                               //----------> left name

            Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
            Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
            //Chart1.Legends["Legend1"].Enabled = false;
            Chart1.Legends["Legend1"].Docking = Docking.Bottom;
            Chart1.Legends["Legend1"].DockedToChartArea = "ChartArea1";
            Chart1.Legends["Legend1"].IsDockedInsideChartArea = false;
            series.IsValueShownAsLabel = true;

            Chart1.Series[1].IsValueShownAsLabel = true;
            Chart1.Series[0].IsValueShownAsLabel = true;
            //series.Name = "Total No. of DTC Failured";
        }
        //ADDED FOR THE TRANSFORMER FAILURE REASON
        public void LoadFailureResonChart()
        {
            Series series = new Series();
            DataTable dtResonBarGraph = new DataTable();
            clsDashboard objDashboard = new clsDashboard();

            if (ViewState["FailureReasonGraph"] != null)
            {
                dtResonBarGraph = (DataTable)ViewState["FailureReasonGraph"];
            }
            else
            {
                objDashboard.sOfficeCode = hdfLocationCode.Value;

                if (objDashboard.sOfficeCode == "0")
                {
                    hdfLocationCode.Value = null;
                }

                dtResonBarGraph = objDashboard.LoadBarGraphforFailureResonChart(hdfLocationCode.Value);
                ViewState["FailureReasonGraph"] = dtResonBarGraph;
            }

            string[] XPointMember = new string[dtResonBarGraph.Rows.Count];

            int[] YPointMember = new int[dtResonBarGraph.Rows.Count];

            //int[] YPointMember2 = new int[dtBarGraph.Rows.Count];

            for (int i = 0; i < dtResonBarGraph.Rows.Count; i++)
            {

                XPointMember[i] = Convert.ToString(dtResonBarGraph.Rows[i]["REASON"]);

                YPointMember[i] = Convert.ToInt32(dtResonBarGraph.Rows[i]["PRESENTCOUNT"]);

            }

            //i got doubt  what to take as variable name

            var FailureReason = Convert.ToString(dtResonBarGraph.Rows[0]["REASON"]);
            // var PreviousYear = Convert.ToString(dtResonBarGraph.Rows[0]["PREVIOUSYEAR"]);

            //series.ChartType = SeriesChartType.Bar;

            FailureReasonChart.Series[0].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
            // Chart1.Series[0].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
            // Chart1.Series[1].ToolTip = "(#VALY)Failure on (#VALX " + PresentYear + ")";             //i got doubt  what is the use of this tooltip
            FailureReasonChart.Series[0].ToolTip = "(#VALY)Failure on (#VALX " + FailureReason + ")";            //i got doubt  
            FailureReasonChart.Series[0].Points.DataBindXY(XPointMember, YPointMember);


            //Chart1.Titles["NewTitle"].Text = "Dtc failured per month";
            //series.Color = System.Drawing.Color.Yellow;


            //Chart1.Series.Add(series);
            // Chart1.Series[1]["PixelPointWidth"] = "50";                         //i got doubt  whay this is used
            FailureReasonChart.Series[0]["PixelPointWidth"] = "50";

            //Chart1.ChartAreas[0].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
            //Chart1.Series[0].ChartType = SeriesChartType.Bar;
            //Chart1.Series[1].ChartType = SeriesChartType.Bar;
            FailureReasonChart.Width = 1000;

            //Chart1.Series[2].IsVisibleInLegend = false;
            FailureReasonChart.Titles["NewTitle"].Text = "DTr Failure Reason";     //i got doubt  what name i shood give hear                             //----------> top name  

            FailureReasonChart.Series[0].LegendText = "Failure Reason";        //i got doubt              //----i think it is small name below the graf
            //Chart1.Series[1].LegendText = "Current Year";       //i got doubt                  //----i think it is small name below the graf


            Axis xaxis = FailureReasonChart.ChartAreas[0].AxisX;
            //xaxis.IntervalOffsetType = DateTimeIntervalType.Months;
            xaxis.Interval = 1;

            Axis yaxis = FailureReasonChart.ChartAreas[0].AxisY;
            //yaxis.Interval = 20;

            //Chart1.Series[1].LegendText = "Count of Dtc Failured";
            FailureReasonChart.ChartAreas[0].AxisX.Title = "Failure Reason ";                                            //----------> bottom name

            FailureReasonChart.ChartAreas[0].AxisY.Title = "No. of DTr Failure";                               //----------> left name

            FailureReasonChart.ChartAreas["ChartArea2"].AxisX.MajorGrid.Enabled = false;
            FailureReasonChart.ChartAreas["ChartArea2"].AxisY.MajorGrid.Enabled = false;
            //Chart1.Legends["Legend1"].Enabled = false;
            FailureReasonChart.Legends["Legend2"].Docking = Docking.Bottom;
            FailureReasonChart.Legends["Legend2"].DockedToChartArea = "ChartArea2";
            FailureReasonChart.Legends["Legend2"].IsDockedInsideChartArea = false;
            series.IsValueShownAsLabel = true;

            //Chart1.Series[1].IsValueShownAsLabel = true;
            FailureReasonChart.Series[0].IsValueShownAsLabel = true;
            //series.Name = "Total No. of DTC Failured";
        }
        #endregion


        #region DTC Failure Abstract

        public void LoadDTCFailureAbstract()
        {
            try
            {
                clsDashboard objDashboard = new clsDashboard();
                objDashboard.sOfficeCode = hdfLocationCode.Value;
                Genaral.Load_Combo("SELECT \"OM_CODE\", \"OM_CODE\" || '-' || \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE CAST(\"OM_CODE\" AS TEXT) LIKE '" + objDashboard.sOfficeCode + "%' ORDER BY \"OM_CODE\"", "--Select--", cmbSection);

                if (cmbSection.SelectedIndex > 0)
                {
                    objDashboard.sOfficeCode = cmbSection.SelectedValue;
                }
                if (cmbCapacity.SelectedIndex > 0)
                {
                    objDashboard.sCapacity = cmbCapacity.SelectedValue;
                }

                DataTable dt = objDashboard.LoadDTCFailureAbstract(objDashboard);
                grdDTCFailureAbstract.DataSource = dt;
                grdDTCFailureAbstract.DataBind();
                ViewState["DTCFailureAbstract"] = dt;
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void LoadDTCFailureAbstractofficewise()
        {
            try
            {
                clsDashboard objDashboard = new clsDashboard();
                objDashboard.sOfficeCode = hdfLocationCode.Value;



                DataTable dt = objDashboard.LoadDTCFailureAbstractofficewise(objDashboard);

                int PreviousCount = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    PreviousCount = PreviousCount + Convert.ToInt32(dr["PREVIOUSCOUNT"]);
                }
                int PresentCount = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    PresentCount = PresentCount + Convert.ToInt32(dr["PRESENTCOUNT"]);
                }
                int Total_DtcCount = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    Total_DtcCount = Total_DtcCount + Convert.ToInt32(dr["TOTAL_DTCCOUNT"]);
                }

                DataRow drow = dt.NewRow();

                drow["OFF_NAME"] = "TOTAL";
                drow[3] = "TOTAL";
                drow[1] = PresentCount;
                drow[2] = PreviousCount;
                drow[4] = Total_DtcCount;
                dt.Rows.Add(drow);

                if (dt.Rows.Count > 0)
                {
                    grdDTCFailureAbstractoffice.DataSource = dt;
                    grdDTCFailureAbstractoffice.DataBind();
                    ViewState["DTCFailureAbstractofficewise"] = dt;
                }
                else
                {
                    ShowEmptyGridoffice();
                }
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbCapacity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadDTCFailureAbstract();
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadDTCFailureAbstract();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void grdDashboard_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdDTCFailureAbstract.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["DTCFailureAbstract"];
                grdDTCFailureAbstract.DataSource = SortDataTable(dt as DataTable, true);
                grdDTCFailureAbstract.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdDTCFailureAbstractoffice_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdDTCFailureAbstractoffice.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["DTCFailureAbstractofficewise"];
                int PreviousCount = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    PreviousCount = PreviousCount + Convert.ToInt32(dr["PREVIOUSCOUNT"]);
                }
                int PresentCount = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    PresentCount = PresentCount + Convert.ToInt32(dr["PRESENTCOUNT"]);
                }
                int Total_DtcCount = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    Total_DtcCount = Total_DtcCount + Convert.ToInt32(dr["TOTAL_DTCCOUNT"]);
                }

                DataRow drow = dt.NewRow();

                drow["OFF_NAME"] = "TOTAL";
                drow[3] = "TOTAL";
                drow[1] = PresentCount;
                drow[2] = PreviousCount;
                drow[4] = Total_DtcCount;
                dt.Rows.Add(drow);

                grdDTCFailureAbstractoffice.DataSource = SortDataTable(dt as DataTable, true);
                grdDTCFailureAbstractoffice.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdDTCFailureAbstract_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdDTCFailureAbstract.PageIndex;
            DataTable dt = (DataTable)ViewState["DTCFailureAbstract"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdDTCFailureAbstract.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdDTCFailureAbstract.DataSource = dt;
            }
            grdDTCFailureAbstract.DataBind();
            grdDTCFailureAbstract.PageIndex = pageIndex;
        }

        protected void grdDTCFailureAbstractoffice_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdDTCFailureAbstractoffice.PageIndex;
            DataTable dt = (DataTable)ViewState["DTCFailureAbstractofficewise"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdDTCFailureAbstractoffice.DataSource = SortDataTableAbstractofficewise(dt as DataTable, false);
            }
            else
            {
                grdDTCFailureAbstractoffice.DataSource = dt;
            }
            grdDTCFailureAbstractoffice.DataBind();
            grdDTCFailureAbstractoffice.PageIndex = pageIndex;
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

                        if (Convert.ToString(dataView.Sort) == "TC_CAPACITY ASC ")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["DTCFailureAbstract"] = dataView.ToTable();
                        }
                        else if (Convert.ToString(dataView.Sort) == "TC_CAPACITY DESC ")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["DTCFailureAbstract"] = dataView.ToTable();
                        }

                        else
                        {
                            // dataView.Sort = string.Format("{0} {1} ", GridViewSortExpression, GridViewSortDirection);
                            ViewState["DTCFailureAbstract"] = dataView.ToTable();
                        }
                    }
                    else
                    {

                        dataView.Sort = string.Format("{0} {1} ", GridViewSortExpression, GetSortDirection());

                        if (Convert.ToString(dataView.Sort) == "TC_CAPACITY ASC ")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["DTCFailureAbstract"] = dataView.ToTable();
                        }
                        else if (Convert.ToString(dataView.Sort) == "TC_CAPACITY DESC ")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["DTCFailureAbstract"] = dataView.ToTable();
                        }



                        else
                        {
                            // dataView.Sort = string.Format("{0} {1} ", GridViewSortExpression, GetSortDirection());
                            ViewState["DTCFailureAbstract"] = dataView.ToTable(); ;


                        }


                        //dv.Sort = string.Format("{0} {1} ", GridViewSortExpression, GetSortDirection());

                    }
                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }


        protected DataView SortDataTableAbstractofficewise(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1} ", GridViewSortExpression, GridViewSortDirection);

                        if (Convert.ToString(dataView.Sort) == "OFF_NAME ASC ")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("OFF_NAME")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["DTCFailureAbstractofficewise"] = dataView.ToTable();
                        }
                        else if (Convert.ToString(dataView.Sort) == "OFF_NAME DESC ")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("OFF_NAME")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["DTCFailureAbstractofficewise"] = dataView.ToTable();
                        }

                        else
                        {
                            ViewState["DTCFailureAbstractofficewise"] = dataView.ToTable();
                        }
                    }
                    else
                    {

                        dataView.Sort = string.Format("{0} {1} ", GridViewSortExpression, GetSortDirection());

                        if (Convert.ToString(dataView.Sort) == "OFF_NAME ASC ")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("OFF_NAME")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["DTCFailureAbstractofficewise"] = dataView.ToTable();
                        }
                        else if (Convert.ToString(dataView.Sort) == "OFF_NAME DESC ")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("OFF_NAME")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["DTCFailureAbstractofficewise"] = dataView.ToTable();
                        }



                        else
                        {
                            ViewState["DTCFailureAbstractofficewise"] = dataView.ToTable(); ;


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

        #endregion


        protected void lnkSLA_Dashboard_Click(object sender, EventArgs e)
        {
            try
            {
                string url = "/DashboardForm/SLADashboard.aspx";
                string s = "window.open('" + url + "','_blank');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                //ClientScript.RegisterStartupScript(this.GetType(), "script", "window.open('/DashboardForm/SLADashboard.aspx','_blank')", true);

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void lnkFaultyView_Click(object sender, EventArgs e)
        {
            try
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/DTRRepairDetails.aspx?RefId=Custom&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','_blank');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void lnkFailurePend_Click(object sender, EventArgs e)
        {
            try
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePenddingDetails.aspx?RefId=Custom&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','_blank');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        #region DTC Count
        public void GetTotalDTCCount()
        {
            try
            {
                clsDashboard objDashboard = new clsDashboard();
                clsTcMaster objtcmaster = new clsTcMaster();

                objDashboard.sOfficeCode = hdfLocationCode.Value;
                if (objDashboard.sOfficeCode == "0")
                {
                    objDashboard.sOfficeCode = null;
                }

                //Total DTC count
                //lblTotalDTC.Text = objDashboard.GetTotalDTCCount(objDashboard);
                lblDTC.Text = objDashboard.GetTotalDTCCount(objDashboard);
                //lblTotalDTR.Text = objtcmaster.getTCCount(objDashboard.sOfficeCode, objSession.sRoleType, "");
                lblDTR.Text = objtcmaster.getTCCount(objDashboard.sOfficeCode, objSession.sRoleType, "", "", "", "");

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        #endregion

        protected void Failure_Click(object sender, EventArgs e)
        {
            if (lblToatlPending.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePendingOverview.aspx?value=Failure&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }

        }
        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            try
            {
                //To open the PDF and also download
                //string pdfPath = Server.MapPath("~/UserManual/1.pdf");
                //WebClient client = new WebClient();
                //Byte[] buffer = client.DownloadData(pdfPath);
                //Response.ContentType = "application/pdf";
                //Response.AddHeader("content-length", buffer.Length.ToString());
                //Response.BinaryWrite(buffer);

                //Only To Download Pdf
                string Filename = MapPath("~/UserManual/DTLMS_HESCOM_1.1V_PROD.apk");
                // This is an important header part that informs the client to download this file.
                Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(Filename));
                Response.ContentType = "Application/pdf";
                //Write the file directly to the HTTP content output stream.
                Response.WriteFile(Filename);

                if (objSession.sTransactionLog == "1")
                {
                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " Android APK Download ");
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void lnkAndroidManual_Click(object sender, EventArgs e)
        {
            //string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
            //string sUserName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_USER"]);
            //string sPassword = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_PASS"]);
            string SFTPmainfolder = Convert.ToString(ConfigurationSettings.AppSettings["VirtualDirectoryDocs"]);
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
                    clsApkDownload objApk = new clsApkDownload();
                    //ShowMsgBox("before");
                    string sFoldername = objApk.RetrieveLatestApkDetails();
                    // string PoNo = Regex.Replace(txtPoNumber.Text, @"[^0-9a-zA-Z]+", "");
                    // clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);

                    //string url = SFTPmainfolder + "ANDROID_APK_DOWNLOAD/" + sFoldername + "/" + fileName1;
                    //  ShowMsgBox(SFTPmainfolder);

                    string url = SFTPmainfolder + "ANDROID_APK_DOWNLOAD/" + sFoldername;
                    string fileName = getFilename(url);
                    // ShowMsgBox(url);
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
                    // ShowMsgBox("middle");
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
                // ShowMsgBox("finallu");
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
        //{
        //    try
        //    {
        //        string Filename = MapPath("~/UserManual/Android.pdf");
        //        // This is an important header part that informs the client to download this file.
        //        Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(Filename));
        //        Response.ContentType = "Application/pdf";
        //        //Write the file directly to the HTTP content output stream.
        //        Response.WriteFile(Filename);

        //        if (objSession.sTransactionLog == "1")
        //        {
        //            Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " Android Manual Download ");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}
        //protected void lnkFailureApprove_Click(object sender, EventArgs e)
        //{
        //    if (lblFailureApprove.Text == "0")
        //    {
        //        ShowMsgBox("No Record Found");
        //    }
        //    else
        //    {
        //        string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
        //        string url = "/DashboardForm/FailurePendingOverview.aspx?value=FailurePendingApproval&OfficeCode=" + sOfficeCode;
        //        string s = "window.open('" + url + "','mypopup','width=1100,height=800');";
        //        ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
        //    }
        //}

        protected void estimation_Click(object sender, EventArgs e)
        {
            if (lblPendingEstimation.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePendingOverview.aspx?value=estimation&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }

        }

        protected void workorder_Click(object sender, EventArgs e)
        {
            if (lblPendingWO.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePendingOverview.aspx?value=workorder&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }

        }

        //protected void Singleworkorder_Click(object sender, EventArgs e)
        //{
        //    if (lblSingleWO.Text == "0")
        //    {
        //        ShowMsgBox("No Record Found");
        //    }
        //    else
        //    {
        //        string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
        //        string url = "/DashboardForm/FailurePendingOverview.aspx?value=Singleworkorder&OfficeCode=" + sOfficeCode;
        //        string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
        //        ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
        //    }

        //}

        //protected void ReceiveTC_Click(object sender, EventArgs e)
        //{
        //    if (lblReceiveTC.Text == "0")
        //    {
        //        ShowMsgBox("No Record Found");
        //    }
        //    else
        //    {
        //        string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
        //        string url = "/DashboardForm/FailurePendingOverview.aspx?value=ReceiveTC&OfficeCode=" + sOfficeCode;
        //        string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
        //        ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
        //    }

        //}

        //protected void Comission_Click(object sender, EventArgs e)
        //{
        //    if (lblComission.Text == "0")
        //    {
        //        ShowMsgBox("No Record Found");
        //    }
        //    else
        //    {
        //        string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
        //        string url = "/DashboardForm/FailurePendingOverview.aspx?value=SingleComission&OfficeCode=" + sOfficeCode;
        //        string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
        //        ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
        //    }

        //}

        //protected void indent_Click(object sender, EventArgs e)
        //{
        //    if (lblPendingIndent.Text == "0")
        //    {
        //        ShowMsgBox("No Record Found");
        //    }
        //    else
        //    {
        //        string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
        //        string url = "/DashboardForm/FailurePendingOverview.aspx?value=indent&OfficeCode=" + sOfficeCode;
        //        string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
        //        ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
        //    }

        //}

        protected void invoice_Click(object sender, EventArgs e)
        {
            if (lblPendingCommission.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePendingOverview.aspx?value=invoice&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }

        }

        //protected void Decommissioning_Click(object sender, EventArgs e)
        //{
        //    if (lblPendingDeCommission.Text == "0")
        //    {
        //        ShowMsgBox("No Record Found");
        //    }
        //    else
        //    {
        //        string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
        //        string url = "/DashboardForm/FailurePendingOverview.aspx?value=DeCommission&OfficeCode=" + sOfficeCode;
        //        string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
        //        ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
        //    }

        //}

        protected void RI_Click(object sender, EventArgs e)
        {
            if (lblPendingRI.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePendingOverview.aspx?value=RI&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }

        }

        protected void CR_Click(object sender, EventArgs e)
        {
            if (lblPendingCR.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePendingOverview.aspx?value=CR&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }

        }

        protected void lnkBtnFaildDtrDetails_Click(object sender, EventArgs e)
        {
            string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
            string url = "/DashboardForm/TcFailureDetails.aspx?value=Failure&OfficeCode=" + sOfficeCode;
            string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
            ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);


        }

        protected void Tcfailed_Click(object sender, EventArgs e)
        {
            if (LabelTcfailed.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/TcFailed.aspx?value=TcFailed&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }

        }
        protected void TotalFaulty_Click(object sender, EventArgs e)
        {
            if (lblTotalFaulty.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/TcFailed.aspx?value=TotalFaulty&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }

        }
        protected void Faulty_field_Click(object sender, EventArgs e)
        {
            if (lblFaultyField.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/TcFailed.aspx?value=FaultyField&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }

        }
        protected void Faulty_Store_Click(object sender, EventArgs e)
        {
            if (lblFaultyStore.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/TcFailed.aspx?value=FaultyStore&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }

        }
        protected void Faulty_Repairer_Click(object sender, EventArgs e)
        {
            if (lblFaultyRepairer.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/TcFailed.aspx?value=FaultyRepairer&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }

        }
        protected void Export_clickDTCFailureAbstract(object sender, EventArgs e)
        {
            //clsDashboard objDashboard = new clsDashboard();

            //objDashboard.sOfficeCode = hdfLocationCode.Value;
            //if (cmbSection.SelectedIndex > 0)
            //{
            //    objDashboard.sOfficeCode = cmbSection.SelectedValue;
            //}
            //if (cmbCapacity.SelectedIndex > 0)
            //{
            //    objDashboard.sCapacity = cmbCapacity.SelectedValue;
            //}

            //DataTable dt = objDashboard.LoadDTCFailureAbstract(objDashboard);
            DataTable dt = (DataTable)ViewState["DTCFailureAbstract"];
            if (dt.Rows.Count > 0)
            {

                dt.Columns["TC_CAPACITY"].ColumnName = "Capacity(in KVA)";
                dt.Columns["DF_LOC_CODE"].ColumnName = "Section Code";
                dt.Columns["SECTION"].ColumnName = "Section Name";
                dt.Columns["CURRENTMONTH"].ColumnName = "Current Month";
                dt.Columns["PREVIOUSMONTH"].ColumnName = "Previous Month";
                dt.Columns["CURRENTQUARTER"].ColumnName = "Current Quarter";
                dt.Columns["FAILURECOUNTOFYEAR"].ColumnName = "Current Financial Year";

                List<string> listtoRemove = new List<string> { "" };
                string filename = "DTCFailureAbstract" + DateTime.Now + ".xls";
                string pagetitle = "Transformer Centre Failure Abstract";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");

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
                dt.Columns.Add("OFF_CODE");
                dt.Columns.Add("OFF_NAME");

                grdOffices.DataSource = dt;
                grdOffices.DataBind();

                int iColCount = grdOffices.Rows[0].Cells.Count;
                grdOffices.Rows[0].Cells.Clear();
                grdOffices.Rows[0].Cells.Add(new TableCell());
                grdOffices.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdOffices.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void ShowEmptyGridoffice()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("OFF_CODE");
                dt.Columns.Add("PRESENTCOUNT");
                dt.Columns.Add("PREVIOUSCOUNT");
                dt.Columns.Add("OFF_NAME");
                dt.Columns.Add("TOTAL_DTCCOUNT");

                grdDTCFailureAbstractoffice.DataSource = dt;
                grdDTCFailureAbstractoffice.DataBind();

                int iColCount = grdDTCFailureAbstractoffice.Rows[0].Cells.Count;
                grdDTCFailureAbstractoffice.Rows[0].Cells.Clear();
                grdDTCFailureAbstractoffice.Rows[0].Cells.Add(new TableCell());
                grdDTCFailureAbstractoffice.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdDTCFailureAbstractoffice.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdDTCFailureAbstract_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "View")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    Label lblOffcode = (Label)row.FindControl("lblSectionCode");
                    Label lblCapacity = (Label)row.FindControl("lblCapacity");

                    string sOffCode = lblOffcode.Text;

                    clsDashboard obj = new clsDashboard();
                    obj.sOfficeCode = sOffCode;
                    obj.sCapacity = lblCapacity.Text;
                    DataTable dt = new DataTable();
                    dt = obj.LoadDTCFailureAbstract(obj);
                    grdDTCFailureAbstract.DataSource = dt;
                    grdDTCFailureAbstract.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void grdDTCFailureAbstractoffice_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "View")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    Label lblOffcode = (Label)row.FindControl("lbloffcode");


                    string sOffCode = lblOffcode.Text;

                    clsDashboard obj = new clsDashboard();
                    obj.sOfficeCode = sOffCode;

                    DataTable dt = new DataTable();


                    dt = obj.LoadDTCFailureAbstractofficewise(obj);
                    int PreviousCount = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        PreviousCount = PreviousCount + Convert.ToInt32(dr["PREVIOUSCOUNT"]);
                    }
                    int PresentCount = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        PresentCount = PresentCount + Convert.ToInt32(dr["PRESENTCOUNT"]);
                    }
                    int Total_DtcCount = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        Total_DtcCount = Total_DtcCount + Convert.ToInt32(dr["TOTAL_DTCCOUNT"]);
                    }

                    DataRow drow = dt.NewRow();

                    drow["OFF_NAME"] = "TOTAL";
                    drow[3] = "TOTAL";
                    drow[1] = PresentCount;
                    drow[2] = PreviousCount;
                    drow[4] = Total_DtcCount;
                    dt.Rows.Add(drow);
                    grdDTCFailureAbstractoffice.DataSource = dt;
                    grdDTCFailureAbstractoffice.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }



        protected void lnkMD_Dashboard_Click(object sender, EventArgs e)
        {
            try
            {
                string url = "/DashboardForm/MdDashboard.aspx";
                string s = "window.open('" + url + "','_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void lnkTotalDtrDetails_Click(object sender, EventArgs e)
        {

            if (lblTotalDtrDetails.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                //DataTable dt = new DataTable();
                //DataSet ds = new DataSet();
                //dt = (DataTable) ViewState["TotalDTRDetails"];
                //ds.Tables.Add(dt);
                //XmlDataDocument xmldatainfo = new XmlDataDocument(ds);
                //XmlElement xmlElementinfo = xmldatainfo.DocumentElement;
                //string xml_stringinfo = xmldatainfo.OuterXml.ToString();
                //Session["dtInfo"] = xml_stringinfo;

                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePendingOverview.aspx?value=TotalDTRDetails&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }
            //TotalDTRDetails = objDashboard.GetDtrCount(objDashboard.sOfficeCode, "0");
            //ViewState["TotalDTRDetails"] = TotalDTRDetails;
            //TotalFiledDTRDetails = objDashboard.GetDtrCount(objDashboard.sOfficeCode, "2");
            //ViewState["TotalFiledDTRDetails"] = TotalFiledDTRDetails;
            //TotalStoreDTRDetails = objDashboard.GetDtrCount(objDashboard.sOfficeCode, "1");
            //ViewState["TotalStoreDTRDetails"] = TotalStoreDTRDetails;
            //TotalRepairerDTRDetails = objDashboard.GetDtrCount(objDashboard.sOfficeCode, "3");
            //ViewState["TotalRepairerDTRDetails"] = TotalRepairerDTRDetails;
            //TotalBankDTRDetails = objDashboard.GetDtrCount(objDashboard.sOfficeCode, "5");
            //ViewState["TotalBankDTRDetails"] = TotalBankDTRDetails;
        }

        protected void lnkTotalFieldDtr_Click(object sender, EventArgs e)
        {
            if (lblTotalFileldDtr.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePendingOverview.aspx?value=TotalFieldDTRDetails&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }
        }

        protected void lnkTotalBankDtr_Click(object sender, EventArgs e)
        {
            if (lblTotalBankDtr.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePendingOverview.aspx?value=TotalBankDTRDetails&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }
        }

        protected void lnkTotalStoreDtr_Click(object sender, EventArgs e)
        {
            if (lblTotalStoreDtr.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePendingOverview.aspx?value=TotalStoreDTRDetails&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }
        }

        protected void lnkTotalRepairerDtr_Click(object sender, EventArgs e)
        {
            if (lblTotalRepairerDtr.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePendingOverview.aspx?value=TotalRepairerDTRDetails&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }
        }

        protected void cmdBack_Click(object sender, EventArgs e)
        {
            try

            {
                LoadDTCFailureAbstractofficewise();

            }


            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }
    }

}