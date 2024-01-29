using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.IO;

namespace IIITS.DTLMS.DashboardForm
{
    public partial class FailurePendingDetails : System.Web.UI.Page
    {
        string strFormCode = "FailurePendingDetails";
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
                dtLoadDetails = objDashboard.LoadFailurePendingDetails(hdfOffCode.Value);
                grdFailurePending.DataSource = dtLoadDetails;
                grdFailurePending.DataBind();
                ViewState["FailurePending"] = dtLoadDetails;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdFailurePending_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    GridView HeaderGrid = (GridView)sender;
                    GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                    TableCell HeaderCell = new TableCell();
                    HeaderCell.Text = "";

                    HeaderCell.ColumnSpan = 2;
                    HeaderGridRow.Cells.Add(HeaderCell);
                    HeaderCell = new TableCell();
                    HeaderCell.Text = "Section";

                    HeaderCell.ColumnSpan = 1;
                    HeaderGridRow.Cells.Add(HeaderCell);
                    HeaderCell = new TableCell();
                    HeaderCell.Text = "Sub Division";

                    HeaderCell.ColumnSpan = 1;
                    HeaderGridRow.Cells.Add(HeaderCell);
                    HeaderCell = new TableCell();
                    HeaderCell.Text = "Failure";

                    HeaderCell.ColumnSpan = 2;
                    HeaderGridRow.Cells.Add(HeaderCell);
                    HeaderCell = new TableCell();
                    HeaderCell.Text = "Estimation";


                    HeaderCell.ColumnSpan = 3;
                    HeaderGridRow.Cells.Add(HeaderCell);
                    HeaderCell = new TableCell();
                    HeaderCell.Text = "Work Order";

                    HeaderCell.ColumnSpan = 3;
                    HeaderGridRow.Cells.Add(HeaderCell);
                    HeaderCell = new TableCell();
                    HeaderCell.Text = "Indent";

                    HeaderCell.ColumnSpan = 3;
                    HeaderGridRow.Cells.Add(HeaderCell);
                    HeaderCell = new TableCell();
                    HeaderCell.Text = "Commission";

                    HeaderCell.ColumnSpan = 3;
                    HeaderGridRow.Cells.Add(HeaderCell);
                    HeaderCell = new TableCell();
                    HeaderCell.Text = "Decommission / RI";


                    HeaderCell.ColumnSpan = 3;
                    HeaderGridRow.Cells.Add(HeaderCell);
                    HeaderCell = new TableCell();
                    HeaderCell.Text = "CR";

                    HeaderCell.ColumnSpan = 3;
                    HeaderGridRow.Cells.Add(HeaderCell);
                    grdFailurePending.Controls[0].Controls.AddAt(0, HeaderGridRow);

                }
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
                grdFailurePending.DataSource = SortDataTable(dtComplete as DataTable, true);
                grdFailurePending.DataBind();
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
                grdFailurePending.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdFailurePending.DataSource = dt;
            }
            grdFailurePending.DataBind();
            grdFailurePending.PageIndex = pageIndex;
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


                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());


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

        public void ExportToExcel(DataTable dtCompleteDetails)
        {
            try
            {
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Failure_Pending_Details.xls"));
                Response.ContentType = "application/ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                grdFailurePending.AllowPaging = false;
                LoadFailurePendingDetails();
                //Change the Header Row back to white color
                grdFailurePending.HeaderRow.Style.Add("background-color", "#FFFFFF");
                //Applying stlye to gridview header cells
                for (int i = 0; i < grdFailurePending.HeaderRow.Cells.Count; i++)
                {
                    grdFailurePending.HeaderRow.Cells[i].Style.Add("background-color", "#709eea");
                }
                grdFailurePending.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

        protected void cmbExport_Click(object sender, EventArgs e)
        {
            //try
            //{
                DataTable dtComplete = new DataTable();
                dtComplete = (DataTable)ViewState["FailurePending"];
                ExportToExcel(dtComplete);
            //}
            //catch (Exception ex)
            //{
            //    lblMessage.Text = clsException.ErrorMsg();
            //    clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbExport_Click");
            //}
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

                    DataTable dt = (DataTable)ViewState["FailurePending"];
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
                dt.Columns.Add("DT_CODE");
                dt.Columns.Add("DT_NAME");
                dt.Columns.Add("OMSECTION");
                dt.Columns.Add("OM_CODE");
                dt.Columns.Add("SUBDIVSION");
                dt.Columns.Add("DF_ID");
                dt.Columns.Add("DF_DATE");
                dt.Columns.Add("EST_NO");
                dt.Columns.Add("EST_CRON");
                dt.Columns.Add("FL_STATUS");
                dt.Columns.Add("WO_NO");
                dt.Columns.Add("WO_DATE");
                dt.Columns.Add("WO_STATUS");
                dt.Columns.Add("TI_INDENT_NO");
                dt.Columns.Add("TI_INDENT_DATE");
                dt.Columns.Add("INDT_STATUS");
                dt.Columns.Add("IN_INV_NO");
                dt.Columns.Add("IN_DATE");
                dt.Columns.Add("INV_STATUS");
                dt.Columns.Add("TR_RI_NO");
                dt.Columns.Add("TR_RI_DATE");
                dt.Columns.Add("RI_STATUS");
                dt.Columns.Add("CR_STATUS");
               

                grdFailurePending.DataSource = dt;
                grdFailurePending.DataBind();

                int iColCount = grdFailurePending.Rows[0].Cells.Count;
                grdFailurePending.Rows[0].Cells.Clear();
                grdFailurePending.Rows[0].Cells.Add(new TableCell());
                grdFailurePending.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdFailurePending.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }
    }
}

