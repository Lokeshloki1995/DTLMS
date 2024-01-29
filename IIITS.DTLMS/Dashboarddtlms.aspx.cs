using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Configuration;
using System.Web.UI.DataVisualization.Charting;
using System.Xml;

namespace IIITS.DTLMS
{
    public partial class Dashboarddtlms : System.Web.UI.Page
    {
        string strFormCode = "Dashboarddtlms";
        protected void Page_Load(object sender, EventArgs e)
        {
          
           

          
               
                   
                        lblLocation.Text = "";
                        hdfLocationCode.Value = "";
                    

                    
                        lnkChange.Visible = false;
                    

                    Genaral.Load_Combo("SELECT \"MD_NAME\",\"MD_NAME\" from \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='C' ORDER BY \"MD_ORDER_BY\"", "-Select-", cmbCapacity);
                    Genaral.Load_Combo("SELECT \"OM_CODE\", \"OM_CODE\" || '-' || \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE CAST(\"OM_CODE\" AS TEXT) LIKE '" + "" + "%' ORDER BY \"OM_CODE\"", "--Select--", cmbSection);

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


                

                //DTC Failure Chart
                LoadFailureChart();

            
        }


        public void DashboardFunctions()
        {
            try
            {
                GetPendingFailureCount();
                GetFaultyDTrCount();
                GetDTrCount();
                LoadFailureChart();
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
               

                if (objDashboard.sOfficeCode == "0")
                {
                    objDashboard.sOfficeCode = null;
                }


                DataTable dt = objDashboard.getFailurePendingCounts(objDashboard.sOfficeCode);
                //Total Pending Count
                lblToatlPending.Text = Convert.ToString(dt.Rows[0]["TOTA_DTC_FAILURE"]);

                //Total Failufre Approval Pending
                lblFailureApprove.Text = Convert.ToString(dt.Rows[0]["FAILURE_APPROVE"]);

                //Pending for Estimation
                lblPendingEstimation.Text = Convert.ToString(dt.Rows[0]["PENDING_ESTIMATION"]);

                // Pending for WorkOrder
                objDashboard.sBOId = "11,74";
                lblPendingWO.Text = Convert.ToString(dt.Rows[0]["PEN_MULTI_COIL_WOR"]);

                objDashboard.sBOId = "11,74";
                lblSingleWO.Text = Convert.ToString(dt.Rows[0]["PEN_SINGLE_COIL_WOR"]);

                objDashboard.sBOId = "46,48";
                lblReceiveTC.Text = Convert.ToString(dt.Rows[0]["PENDING_RECIEVE_DTR"]);

                objDashboard.sBOId = "47";
                lblComission.Text = Convert.ToString(dt.Rows[0]["PENDING_COMMISS"]);
                // Pending for Indent
                objDashboard.sBOId = "12";
                lblPendingIndent.Text = Convert.ToString(dt.Rows[0]["PENDING_INDENT"]);


                // Pending for Commission
                objDashboard.sBOId = "13,29";
                lblPendingCommission.Text = Convert.ToString(dt.Rows[0]["PENDING_MAJOR_INV"]);

                objDashboard.sBOId = "14";
                lblPendingDeCommission.Text = Convert.ToString(dt.Rows[0]["PENDING_DECOMMI"]);

                // Pending for RI             
                lblPendingRI.Text = Convert.ToString(dt.Rows[0]["PENDING_RI/CR"]);
                

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
               

                objDashboard.sOfficeCode = hdfLocationCode.Value;
                if (objDashboard.sOfficeCode == "0")
                {
                    objDashboard.sOfficeCode = null;
                }

                DataTable dtTcDet = objDashboard.GetTcDetails(objDashboard);
                lblTotalDtrDetails.Text = Convert.ToString(dtTcDet.Rows[0]["total_tc"]);
                lblTotalFileldDtr.Text = Convert.ToString(dtTcDet.Rows[0]["field_count"]);
                lblTotalStoreDtr.Text = Convert.ToString(dtTcDet.Rows[0]["TotalStoreDtr"]);
                lblTotalRepairerDtr.Text = Convert.ToString(dtTcDet.Rows[0]["repairer_count"]);
                lblTotalBankDtr.Text = Convert.ToString(dtTcDet.Rows[0]["bank_count"]);
              
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

                objDashboard.sOfficeCode = "";
                objDashboard.sRoleId ="3";

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
                LoadOfficeGrid("");
                this.mdlPopup.Show();

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
                    ViewState["FailureGraph"] = null;
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
            Chart1.Titles["NewTitle"].Text = "DTR Failure/Month";

            Chart1.Series[0].LegendText = "Previous Year";
            Chart1.Series[1].LegendText = "Current Year";


            Axis xaxis = Chart1.ChartAreas[0].AxisX;
            //xaxis.IntervalOffsetType = DateTimeIntervalType.Months;
            xaxis.Interval = 1;
            Axis yaxis = Chart1.ChartAreas[0].AxisY;
            //yaxis.Interval = 20;

            //Chart1.Series[1].LegendText = "Count of Dtc Failured";
            Chart1.ChartAreas[0].AxisX.Title = "Month";

            Chart1.ChartAreas[0].AxisY.Title = "No. of DTR Failure";
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
                lblTotalDTC.Text = objDashboard.GetTotalDTCCount(objDashboard);
                //lblTotalDTR.Text = objtcmaster.getTCCount(objDashboard.sOfficeCode, "3","");
                lblTotalDTR.Text = objtcmaster.getTCCount(objDashboard.sOfficeCode, "3", "","","","");

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

        protected void lnkFailureApprove_Click(object sender, EventArgs e)
        {
            if (lblFailureApprove.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePendingOverview.aspx?value=FailurePendingApproval&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');";
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }
        }

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

        protected void Singleworkorder_Click(object sender, EventArgs e)
        {
            if (lblSingleWO.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePendingOverview.aspx?value=Singleworkorder&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }

        }

        protected void ReceiveTC_Click(object sender, EventArgs e)
        {
            if (lblReceiveTC.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePendingOverview.aspx?value=ReceiveTC&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }

        }

        protected void Comission_Click(object sender, EventArgs e)
        {
            if (lblComission.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePendingOverview.aspx?value=SingleComission&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }

        }

        protected void indent_Click(object sender, EventArgs e)
        {
            if (lblPendingIndent.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePendingOverview.aspx?value=indent&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }

        }

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

        protected void Decommissioning_Click(object sender, EventArgs e)
        {
            if (lblPendingDeCommission.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePendingOverview.aspx?value=DeCommission&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }

        }

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