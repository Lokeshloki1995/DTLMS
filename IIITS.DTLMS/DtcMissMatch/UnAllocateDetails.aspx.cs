using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.IO;
using System.Configuration;
using System.Data;

namespace IIITS.DTLMS.DtcMissMatch
{
    public partial class UnAllocateDetails3 : System.Web.UI.Page
    {
        string strFormCode = "UnAllocateDetails";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
            {
                Response.Redirect("~/Login.aspx", false);
            }
            objSession = (clsSession)Session["clsSession"];
            if (!IsPostBack)
            {
                if (Session["OffCode"] != null && Session["OffCode"].ToString() != "")
                {
                    hdfOffCode.Value = Session["OffCode"].ToString();
                }
                else
                {
                    hdfOffCode.Value = objSession.OfficeCode;
                }

                LoadUnAllocateDetails();
            }
        }

        public void LoadUnAllocateDetails()
        {
            try
            {
                clsDtcMissMatchEntry objMisMatch = new clsDtcMissMatchEntry();
                DataTable dtLoadDetails = new DataTable();
                dtLoadDetails = objMisMatch.LoadUnAllocateDetails(hdfOffCode.Value);
                grdUnAllocateDetails.DataSource = dtLoadDetails;
                grdUnAllocateDetails.DataBind();
                ViewState["UnAllocateDetails"] = dtLoadDetails;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        //protected void grdUnAllocateDetails_RowCreated(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Row.RowType == DataControlRowType.Header)
        //        {
        //            GridView HeaderGrid = (GridView)sender;
        //            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
        //            TableCell HeaderCell = new TableCell();
        //            HeaderCell.Text = "";


        //            HeaderCell.ColumnSpan = 2;
        //            HeaderGridRow.Cells.Add(HeaderCell);
        //            HeaderCell = new TableCell();
        //            HeaderCell.Text = "Section";

        //            HeaderCell.ColumnSpan = 1;
        //            HeaderGridRow.Cells.Add(HeaderCell);
        //            HeaderCell = new TableCell();
        //            HeaderCell.Text = "Sub Division";

        //            HeaderCell.ColumnSpan = 1;
        //            HeaderGridRow.Cells.Add(HeaderCell);
        //            HeaderCell = new TableCell();
        //            HeaderCell.Text = "Failure";

        //            HeaderCell.ColumnSpan = 2;
        //            HeaderGridRow.Cells.Add(HeaderCell);
        //            HeaderCell = new TableCell();
        //            HeaderCell.Text = "Estimation";


        //            HeaderCell.ColumnSpan = 4;
        //            HeaderGridRow.Cells.Add(HeaderCell);
        //            HeaderCell = new TableCell();
        //            HeaderCell.Text = "Work Order";

        //            HeaderCell.ColumnSpan = 3;
        //            HeaderGridRow.Cells.Add(HeaderCell);
        //            HeaderCell = new TableCell();
        //            HeaderCell.Text = "Indent";

        //            HeaderCell.ColumnSpan = 3;
        //            HeaderGridRow.Cells.Add(HeaderCell);
        //            HeaderCell = new TableCell();
        //            HeaderCell.Text = "Commission";

        //            HeaderCell.ColumnSpan = 3;
        //            HeaderGridRow.Cells.Add(HeaderCell);
        //            HeaderCell = new TableCell();
        //            HeaderCell.Text = "Decommission / RI";


        //            HeaderCell.ColumnSpan = 3;
        //            HeaderGridRow.Cells.Add(HeaderCell);
        //            HeaderCell = new TableCell();
        //            HeaderCell.Text = "CR";

        //            HeaderCell.ColumnSpan = 3;
        //            HeaderGridRow.Cells.Add(HeaderCell);
        //            grdUnAllocateDetails.Controls[0].Controls.AddAt(0, HeaderGridRow);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdUnAllocateDetails_RowCreated");
        //    }
        //}

        protected void grdUnAllocateDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdUnAllocateDetails.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["UnAllocateDetails"];
                grdUnAllocateDetails.DataSource = SortDataTable(dtComplete as DataTable, true);
                grdUnAllocateDetails.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdUnAllocateDetails_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdUnAllocateDetails.PageIndex;
            DataTable dt = (DataTable)ViewState["UnAllocateDetails"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {

                grdUnAllocateDetails.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdUnAllocateDetails.DataSource = dt;
            }
            grdUnAllocateDetails.DataBind();
            grdUnAllocateDetails.PageIndex = pageIndex;
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
                        ViewState["UnAllocateDetails"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["UnAllocateDetails"] = dataView.ToTable();

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

        protected void cmbExport_Click(object sender, EventArgs e)
        {

            //clsDtcMissMatchEntry objMisMatch = new clsDtcMissMatchEntry();
            //DataTable dtLoadDetails = new DataTable();
            //dtLoadDetails = objMisMatch.LoadUnAllocateDetails(hdfOffCode.Value);

            DataTable dtLoadDetails = (DataTable)ViewState["UnAllocateDetails"];

            if (dtLoadDetails.Rows.Count > 0)
            {

                dtLoadDetails.Columns["DT_NAME"].ColumnName = "DTC NAME";
                dtLoadDetails.Columns["DT_CODE"].ColumnName = "DTC CODE";
                //dtLoadDetails.Columns["TC_CODE"].ColumnName = "TC CODE";
                dtLoadDetails.Columns["DM_ENTRY_DATE"].ColumnName = "UNALLOCATED DATE";
                dtLoadDetails.Columns["DT_OM_SLNO"].ColumnName = "LOCATION CODE";
                dtLoadDetails.Columns["SECTION"].ColumnName = "SECTION";

                //dtLoadDetails.Columns["FEEDER NAME"].SetOrdinal(0);

                List<string> listtoRemove = new List<string> { "TC_CODE" };
                string filename = "UnAllocateDetails" + DateTime.Now + ".xls";
                string pagetitle = "UnAllocated DTC Details";

                Genaral.getexcel(dtLoadDetails, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");
                ShowEmptyGrid();
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

        protected void grdUnAllocateDetails_RowCommand(object sender, GridViewCommandEventArgs e)
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

                    DataTable dt = (DataTable)ViewState["UnAllocateDetails"];
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
                        grdUnAllocateDetails.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdUnAllocateDetails.DataSource = dv;
                            ViewState["UnAllocateDetails"] = dv.ToTable();
                            grdUnAllocateDetails.DataBind();

                        }
                        else
                        {
                            ViewState["UnAllocateDetails"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadUnAllocateDetails();
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
                dt.Columns.Add("TC_CODE");
                dt.Columns.Add("DM_ENTRY_DATE");
                dt.Columns.Add("DT_OM_SLNO");
                dt.Columns.Add("SECTION");
                grdUnAllocateDetails.DataSource = dt;
                grdUnAllocateDetails.DataBind();

                int iColCount = grdUnAllocateDetails.Rows[0].Cells.Count;
                grdUnAllocateDetails.Rows[0].Cells.Clear();
                grdUnAllocateDetails.Rows[0].Cells.Add(new TableCell());
                grdUnAllocateDetails.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdUnAllocateDetails.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }
    }
}