using IIITS.DTLMS.BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.DashboardForm
{
    public partial class TcFailed : System.Web.UI.Page
    {
        string strFormCode = "FailureTc";
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

                        LoadFailureDetails();
                    }
                }
                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void LoadFailureDetails()
        {
            try
            {
                clsDashboard objDashboard = new clsDashboard();
                DataTable dtLoadDetails = new DataTable();


                objDashboard.sOfficeCode = hdfOffCode.Value;


                if (Request.QueryString["value"] == "TcFailed")
                {
                    dtLoadDetails = objDashboard.TotalTcfailedview(hdfOffCode.Value);
                    grdFailuretc.Visible = true;
                    grdFailuretc.DataSource = dtLoadDetails;
                    grdFailuretc.DataBind();
                    ViewState["FailureTc"] = dtLoadDetails;
                    failureText.Text = "Repair Good TC Details";
                    failure.Text = "Repair Good TC Details";
                }
                if (Request.QueryString["value"] == "TotalFaulty")
                {

                    dtLoadDetails = objDashboard.GetTotalFaultyTCview(hdfOffCode.Value); 
                    grdtotaldtr.Visible = true;
                    grdtotaldtr.DataSource = dtLoadDetails;
                    grdtotaldtr.DataBind();
                    ViewState["TotalFaulty"] = dtLoadDetails;
                    failureText.Text = "TotalFaulty DTR Details";
                    failure.Text = "TotalFaulty DTR Details";
                }
              
                if (Request.QueryString["value"] == "FaultyField")
                {
                    dtLoadDetails = objDashboard.GetFaultyTCFieldview(hdfOffCode.Value);
                    grdfaultyfield.Visible = true;
                    grdfaultyfield.DataSource = dtLoadDetails;
                    grdfaultyfield.DataBind();
                    ViewState["FaultyField"] = dtLoadDetails;
                    failureText.Text = "Faulty DTr at Field Details";
                    failure.Text = "Faulty DTr at Field Details";
                }
                if (Request.QueryString["value"] == "FaultyStore")
                {
                    dtLoadDetails = objDashboard.GetFaultyTCStoreview(hdfOffCode.Value);
                    grdfaultystore.Visible = true;
                    grdfaultystore.DataSource = dtLoadDetails;
                    grdfaultystore.DataBind();
                    ViewState["FaultyStore"] = dtLoadDetails;
                    failureText.Text = "Faulty DTr at Store Details";
                    failure.Text = "Faulty DTr at Store Details";
                }
                if (Request.QueryString["value"] == "FaultyRepairer")
                {
                    dtLoadDetails = objDashboard.GetFaultyTCRepairview(hdfOffCode.Value);
                    grdfaultyrepairer.Visible = true;
                    grdfaultyrepairer.DataSource = dtLoadDetails;
                    grdfaultyrepairer.DataBind();
                    ViewState["FaultyRepairer"] = dtLoadDetails;
                    failureText.Text = "Faulty DTr at Repairer Details";
                    failure.Text = "Faulty DTr at Repairer Details";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        #region for pageindexing and sort
        protected void grdFailuretc_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdFailuretc.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["FailureTc"];
                grdFailuretc.DataSource = SortDataTableRepaireGood(dtComplete as DataTable, true);
                grdFailuretc.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdtotaldtr_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdtotaldtr.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["TotalFaulty"];
                grdtotaldtr.DataSource = SortDataTableTotal(dtComplete as DataTable, true);
                grdtotaldtr.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdfaultyfield_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdfaultyfield.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["FaultyField"];
                grdfaultyfield.DataSource = SortDataTableField(dtComplete as DataTable, true);
                grdfaultyfield.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdfaultystore_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdfaultystore.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["FaultyStore"];
                grdfaultystore.DataSource = SortDataTableStore(dtComplete as DataTable, true);
                grdfaultystore.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdfaultyrepairer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdfaultyrepairer.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["FaultyRepairer"];
                grdfaultyrepairer.DataSource = SortDataTableRepairer(dtComplete as DataTable, true);
                grdfaultyrepairer.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdfaultyrepairer_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdfaultyrepairer.PageIndex;
            DataTable dt = (DataTable)ViewState["FaultyRepairer"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdfaultyrepairer.DataSource = SortDataTableRepairer(dt as DataTable, false);
            }
            else
            {
                grdfaultyrepairer.DataSource = dt;
            }
            grdfaultyrepairer.DataBind();
            grdfaultyrepairer.PageIndex = pageIndex;
        }
        protected void grdfaultystore_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdfaultystore.PageIndex;
            DataTable dt = (DataTable)ViewState["FaultyStore"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdfaultystore.DataSource = SortDataTableStore(dt as DataTable, false);
            }
            else
            {
                grdfaultystore.DataSource = dt;
            }
            grdfaultystore.DataBind();
            grdfaultystore.PageIndex = pageIndex;
        }
        protected void grdfaultyfield_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdfaultyfield.PageIndex;
            DataTable dt = (DataTable)ViewState["FaultyField"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdfaultyfield.DataSource = SortDataTableField(dt as DataTable, false);
            }
            else
            {
                grdfaultyfield.DataSource = dt;

            }
            grdfaultyfield.DataBind();
            grdfaultyfield.PageIndex = pageIndex;
        }
        protected void grdtotaldtr_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdtotaldtr.PageIndex;
            DataTable dt = (DataTable)ViewState["TotalFaulty"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdtotaldtr.DataSource = SortDataTableTotal(dt as DataTable, false);
            }
            else
            {
                grdtotaldtr.DataSource = dt;
            }
            grdtotaldtr.DataBind();
            grdtotaldtr.PageIndex = pageIndex;
        }
        protected void grdFailuretc_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdFailuretc.PageIndex;
            DataTable dt = (DataTable)ViewState["FailureTc"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdFailuretc.DataSource = SortDataTableRepaireGood(dt as DataTable, false);
            }
            else
            {
                grdFailuretc.DataSource = dt;
            }
            grdFailuretc.DataBind();
            grdFailuretc.PageIndex = pageIndex;
        }


        protected DataView SortDataTableTotal(DataTable dataTable, bool isPageIndexChanging)

        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);

                        if (Convert.ToString(dataView.Sort) == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["TotalFaulty"] = dataView.ToTable();
                        }
                        else if (Convert.ToString(dataView.Sort) == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["TotalFaulty"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["TotalFaulty"] = dataView.ToTable();
                        }

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        if (Convert.ToString(dataView.Sort) == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["TotalFaulty"] = dataView.ToTable();
                        }
                        else if (Convert.ToString(dataView.Sort) == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["TotalFaulty"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["TotalFaulty"] = dataView.ToTable();
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
        protected DataView SortDataTableField(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        if (Convert.ToString(dataView.Sort) == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FaultyField"] = dataView.ToTable();
                        }
                        else if (Convert.ToString(dataView.Sort) == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FaultyField"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["FaultyField"] = dataView.ToTable();
                        }


                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());

                        if (Convert.ToString(dataView.Sort) == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FaultyField"] = dataView.ToTable();
                        }
                        else if (Convert.ToString(dataView.Sort) == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FaultyField"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["FaultyField"] = dataView.ToTable();
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
        protected DataView SortDataTableStore(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        if (Convert.ToString(dataView.Sort) == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FaultyStore"] = dataView.ToTable();
                        }
                        else if (Convert.ToString(dataView.Sort) == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FaultyStore"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["FaultyStore"] = dataView.ToTable();
                        }
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        if (Convert.ToString(dataView.Sort) == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FaultyStore"] = dataView.ToTable();
                        }
                        else if (Convert.ToString(dataView.Sort) == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FaultyStore"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["FaultyStore"] = dataView.ToTable();
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
        protected DataView SortDataTableRepairer(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        if (Convert.ToString(dataView.Sort) == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FaultyRepairer"] = dataView.ToTable();
                        }
                        else if (Convert.ToString(dataView.Sort) == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FaultyRepairer"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["FaultyRepairer"] = dataView.ToTable();
                        }
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        if (Convert.ToString(dataView.Sort) == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FaultyRepairer"] = dataView.ToTable();
                        }
                        else if (Convert.ToString(dataView.Sort) == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FaultyRepairer"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["FaultyRepairer"] = dataView.ToTable();
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
        protected DataView SortDataTableRepaireGood(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        if (Convert.ToString(dataView.Sort) == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FailureTc"] = dataView.ToTable();
                        }
                        else if (Convert.ToString(dataView.Sort) == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FailureTc"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["FailureTc"] = dataView.ToTable();
                        }
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        if (Convert.ToString(dataView.Sort) == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FailureTc"] = dataView.ToTable();
                        }
                        else if (Convert.ToString(dataView.Sort) == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FailureTc"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["FailureTc"] = dataView.ToTable();
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

        #region for searching 

        protected void grdFailuretc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txttCode = (TextBox)row.FindControl("txttCode");
                    TextBox txttcCapacity = (TextBox)row.FindControl("txttcCapacity");

                    DataTable dt = (DataTable)ViewState["FailureTc"];
                    dv = dt.DefaultView;

                    if (txttCode.Text != "")
                    {
                        sFilter = "Convert(TC_CODE, 'System.String') Like '%" + txttCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txttcCapacity.Text != "")
                    {
                        sFilter += " Convert(TC_CAPACITY, 'System.String') Like '%" + txttcCapacity.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdFailuretc.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdFailuretc.DataSource = dv;
                            ViewState["FailureTc"] = dv.ToTable();
                            grdFailuretc.DataBind();

                        }
                        else
                        {
                            ViewState["FailureTc"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailureDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdtotaldtr_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txttCode = (TextBox)row.FindControl("txttCode");
                    TextBox txttcCapacity = (TextBox)row.FindControl("txttcCapacity");

                    DataTable dt = (DataTable)ViewState["TotalFaulty"];
                    dv = dt.DefaultView;

                    if (txttCode.Text != "")
                    {
                        sFilter = "Convert(TC_CODE, 'System.String') Like '%" + txttCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txttcCapacity.Text != "")
                    {
                        sFilter += " Convert(TC_CAPACITY, 'System.String') Like '%" + txttcCapacity.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdtotaldtr.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdtotaldtr.DataSource = dv;
                            ViewState["TotalFaulty"] = dv.ToTable();
                            grdtotaldtr.DataBind();

                        }
                        else
                        {
                            ViewState["TotalFaulty"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailureDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdfaultyfield_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txttCode = (TextBox)row.FindControl("txttCode");
                    TextBox txttcCapacity = (TextBox)row.FindControl("txttcCapacity");

                    DataTable dt = (DataTable)ViewState["FaultyField"];
                    dv = dt.DefaultView;

                    if (txttCode.Text != "")
                    {
                        sFilter = "Convert(TC_CODE, 'System.String') Like '%" + txttCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txttcCapacity.Text != "")
                    {
                        sFilter += " Convert(TC_CAPACITY, 'System.String') Like '%" + txttcCapacity.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdfaultyfield.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdfaultyfield.DataSource = dv;
                            ViewState["FaultyField"] = dv.ToTable();
                            grdfaultyfield.DataBind();

                        }
                        else
                        {
                            ViewState["FaultyField"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailureDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdfaultystore_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txttCode = (TextBox)row.FindControl("txttCode");
                    TextBox txttcCapacity = (TextBox)row.FindControl("txttcCapacity");

                    DataTable dt = (DataTable)ViewState["FaultyStore"];
                    dv = dt.DefaultView;

                    if (txttCode.Text != "")
                    {
                        sFilter = "Convert(TC_CODE, 'System.String') Like '%" + txttCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txttcCapacity.Text != "")
                    {
                        sFilter += " Convert(TC_CAPACITY, 'System.String') Like '%" + txttcCapacity.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdfaultystore.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdfaultystore.DataSource = dv;
                            ViewState["FaultyStore"] = dv.ToTable();
                            grdfaultystore.DataBind();

                        }
                        else
                        {
                            ViewState["FaultyStore"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailureDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdfaultyrepairer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txttCode = (TextBox)row.FindControl("txttCode");
                    TextBox txttcCapacity = (TextBox)row.FindControl("txttcCapacity");

                    DataTable dt = (DataTable)ViewState["FaultyRepairer"];
                    dv = dt.DefaultView;

                    if (txttCode.Text != "")
                    {
                        sFilter = "Convert(TC_CODE, 'System.String') Like '%" + txttCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txttcCapacity.Text != "")
                    {
                        sFilter += " Convert(TC_CAPACITY, 'System.String') Like '%" + txttcCapacity.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdfaultyrepairer.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdfaultyrepairer.DataSource = dv;
                            ViewState["FaultyRepairer"] = dv.ToTable();
                            grdfaultyrepairer.DataBind();

                        }
                        else
                        {
                            ViewState["FaultyRepairer"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailureDetails();
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

        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);

                if (Request.QueryString["value"] == "TcFailed")
                {
                    dt.Columns.Add("TC_CODE");
                    dt.Columns.Add("TC_CAPACITY");
                    dt.Columns.Add("TC_SLNO");
                    dt.Columns.Add("TC_MANF_DATE");

                    grdFailuretc.DataSource = dt;
                    grdFailuretc.DataBind();

                    int iColCount = grdFailuretc.Rows[0].Cells.Count;
                    grdFailuretc.Rows[0].Cells.Clear();
                    grdFailuretc.Rows[0].Cells.Add(new TableCell());
                    grdFailuretc.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdFailuretc.Rows[0].Cells[0].Text = "No Records Found";
                }
                if (Request.QueryString["value"] == "TotalFaulty")
                {
                    dt.Columns.Add("TC_CODE");
                    dt.Columns.Add("TC_CAPACITY");
                    dt.Columns.Add("TC_SLNO");
                    dt.Columns.Add("TC_MANF_DATE");

                    grdtotaldtr.DataSource = dt;
                    grdtotaldtr.DataBind();

                    int iColCount = grdtotaldtr.Rows[0].Cells.Count;
                    grdtotaldtr.Rows[0].Cells.Clear();
                    grdtotaldtr.Rows[0].Cells.Add(new TableCell());
                    grdtotaldtr.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdtotaldtr.Rows[0].Cells[0].Text = "No Records Found";
                }
                if (Request.QueryString["value"] == "FaultyField")
                {
                    dt.Columns.Add("TC_CODE");
                    dt.Columns.Add("TC_CAPACITY");
                    dt.Columns.Add("TC_SLNO");
                    dt.Columns.Add("TC_MANF_DATE");

                    grdfaultyfield.DataSource = dt;
                    grdfaultyfield.DataBind();

                    int iColCount = grdfaultyfield.Rows[0].Cells.Count;
                    grdfaultyfield.Rows[0].Cells.Clear();
                    grdfaultyfield.Rows[0].Cells.Add(new TableCell());
                    grdfaultyfield.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdfaultyfield.Rows[0].Cells[0].Text = "No Records Found";
                }
                if (Request.QueryString["value"] == "FaultyStore")
                {
                    dt.Columns.Add("TC_CODE");
                    dt.Columns.Add("TC_CAPACITY");
                    dt.Columns.Add("TC_SLNO");
                    dt.Columns.Add("TC_MANF_DATE");

                    grdfaultystore.DataSource = dt;
                    grdfaultystore.DataBind();

                    int iColCount = grdfaultystore.Rows[0].Cells.Count;
                    grdfaultystore.Rows[0].Cells.Clear();
                    grdfaultystore.Rows[0].Cells.Add(new TableCell());
                    grdfaultystore.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdfaultystore.Rows[0].Cells[0].Text = "No Records Found";
                }
                if (Request.QueryString["value"] == "FaultyRepairer")
                {
                    dt.Columns.Add("TC_CODE");
                    dt.Columns.Add("TC_CAPACITY");
                    dt.Columns.Add("TC_SLNO");
                    dt.Columns.Add("TC_MANF_DATE");

                    grdfaultyrepairer.DataSource = dt;
                    grdfaultyrepairer.DataBind();

                    int iColCount = grdfaultyrepairer.Rows[0].Cells.Count;
                    grdfaultyrepairer.Rows[0].Cells.Clear();
                    grdfaultyrepairer.Rows[0].Cells.Add(new TableCell());
                    grdfaultyrepairer.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdfaultyrepairer.Rows[0].Cells[0].Text = "No Records Found";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void Export_clickTcFailure(object sender, EventArgs e)
        {
            //clsDashboard objDashboard = new clsDashboard();
            DataTable dt = new DataTable();


            //objDashboard.sOfficeCode = hdfOffCode.Value;


            if (Request.QueryString["value"] == "TcFailed")
            {
                //dt = objDashboard.TotalTcfailedview(hdfOffCode.Value);
                dt = (DataTable)ViewState["FailureTc"];

                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "DTr CODE";
                    dt.Columns["TC_CAPACITY"].ColumnName = "TC CAPACITY";
                    dt.Columns["TC_SLNO"].ColumnName = "TC SLNO";
                    dt.Columns["TC_MANF_DATE"].ColumnName = "TC MANUFACTURE DATE";
                   

                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "TcFailed" + DateTime.Now + ".xls";
                    string pagetitle = "Tc Failure Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }
                
            }
            if (Request.QueryString["value"] == "TotalFaulty")
            {

               // dt = objDashboard.GetTotalFaultyTCview(hdfOffCode.Value);
                dt = (DataTable)ViewState["TotalFaulty"];
                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "DTr CODE";
                    dt.Columns["TC_CAPACITY"].ColumnName = "TC CAPACITY";
                    dt.Columns["TC_SLNO"].ColumnName = "TC SLNO";
                    dt.Columns["TC_MANF_DATE"].ColumnName = "TC MANUFACTURE DATE";

                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "TotalFaulty" + DateTime.Now + ".xls";
                    string pagetitle = "Total Faulty DTr Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }
               
            }

            if (Request.QueryString["value"] == "FaultyField")
            {
               // dt = objDashboard.GetFaultyTCFieldview(hdfOffCode.Value);
                dt = (DataTable)ViewState["FaultyField"];
                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "DTr CODE";
                    dt.Columns["TC_CAPACITY"].ColumnName = "TC CAPACITY";
                    dt.Columns["TC_SLNO"].ColumnName = "TC SLNO";
                    dt.Columns["TC_MANF_DATE"].ColumnName = "TC MANUFACTURE DATE";

                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "FaultyDTratField" + DateTime.Now + ".xls";
                    string pagetitle = " Faulty DTr at Field Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }
               
            }
            if (Request.QueryString["value"] == "FaultyStore")
            {
              //  dt = objDashboard.GetFaultyTCStoreview(hdfOffCode.Value);
                dt = (DataTable)ViewState["FaultyStore"];
                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "DTr CODE";
                    dt.Columns["TC_CAPACITY"].ColumnName = "TC CAPACITY";
                    dt.Columns["TC_SLNO"].ColumnName = "TC SLNO";
                    dt.Columns["TC_MANF_DATE"].ColumnName = "TC MANUFACTURE DATE";

                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "FaultyDtratStore" + DateTime.Now + ".xls";
                    string pagetitle = "Faulty DTr at Store Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }
               
            }
            if (Request.QueryString["value"] == "FaultyRepairer")
            {
               // dt = objDashboard.GetFaultyTCRepairview(hdfOffCode.Value);
                dt = (DataTable)ViewState["FaultyRepairer"];


                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "DTr CODE";
                    dt.Columns["TC_CAPACITY"].ColumnName = "TC CAPACITY";
                    dt.Columns["TC_SLNO"].ColumnName = "TC SLNO";
                    dt.Columns["TC_MANF_DATE"].ColumnName = "TC MANUFACTURE DATE";

                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "FaultyDtratRepairer" + DateTime.Now + ".xls";
                    string pagetitle = "Faulty DTr at Repairer Details";

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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}