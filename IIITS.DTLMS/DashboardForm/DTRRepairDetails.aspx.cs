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
    public partial class DTRRepairDetails : System.Web.UI.Page
    {
        string strFormCode = "DTRRepairDetails";
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
                    if (!IsPostBack)
                    {

                        if (Request.QueryString["OfficeCode"] != null && Convert.ToString(Request.QueryString["OfficeCode"]) != "")
                        {
                            hdfOffCode.Value = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["OfficeCode"]));
                        }
                        LoadFaultyDTRDetails();
                    }
                }
                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void LoadFaultyDTRDetails()
        {
            try
            {
                clsDashboard objDashboard = new clsDashboard();
                DataTable dtLoadDetails = new DataTable();

                //dtLoadDetails = objDashboard.LoadFaultyDTRDetails(hdfOffCode.Value);
                grdComplete.DataSource = dtLoadDetails;
                grdComplete.DataBind();
                ViewState["CompleteDetails"] = dtLoadDetails;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdComplete_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    GridView HeaderGrid = (GridView)sender;
                    GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                    TableCell HeaderCell = new TableCell();
                    HeaderCell.Text = "";
                    HeaderCell.ColumnSpan = 4;
                    HeaderGridRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "Field";
                    HeaderCell.ColumnSpan = 2;
                    HeaderGridRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "Store";
                    HeaderCell.ColumnSpan = 3;
                    HeaderGridRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "Repairer";
                    HeaderCell.ColumnSpan = 2;
                    HeaderGridRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "Inspection Details";
                    HeaderCell.ColumnSpan = 2;
                    HeaderGridRow.Cells.Add(HeaderCell);

                    grdComplete.Controls[0].Controls.AddAt(0, HeaderGridRow);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void grdComplete_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdComplete.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["CompleteDetails"];
                grdComplete.DataSource = SortDataTable(dtComplete as DataTable, true);
                grdComplete.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdComplete_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdComplete.PageIndex;
            DataTable dt = (DataTable)ViewState["CompleteDetails"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdComplete.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdComplete.DataSource = dt;
            }
            grdComplete.DataBind();
            grdComplete.PageIndex = pageIndex;
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
                        ViewState["CompleteDetails"] = dataView.ToTable();


                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["CompleteDetails"] = dataView.ToTable();


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

        protected void cmdExport_Click(object sender, EventArgs e)
        {
           
                DataTable dtComplete = new DataTable();
                dtComplete = (DataTable)ViewState["CompleteDetails"];
                ExportToExcel(dtComplete);


            
        }
        public void ExportToExcel(DataTable dtCompleteDetails)
        {
            if (dtCompleteDetails.Rows.Count > 0)
            {
                dtCompleteDetails.Columns["tc_code"].ColumnName = "DTR Code";
                dtCompleteDetails.Columns["TM_NAME"].ColumnName = "MAKE NAME";
                dtCompleteDetails.Columns["TC_SLNO"].ColumnName = "DTR SLNO";
                dtCompleteDetails.Columns["TC_CAPACITY"].ColumnName = "CAPACITY(IN KVA)";
                dtCompleteDetails.Columns["dt_code"].ColumnName = "Transformer CENTRE CODE";
                dtCompleteDetails.Columns["dt_name"].ColumnName = "Transformer CENTRE NAME";
                dtCompleteDetails.Columns["TR_RI_NO"].ColumnName = "RI NO";
                dtCompleteDetails.Columns["TR_RI_DATE"].ColumnName = "RI DATE";
                dtCompleteDetails.Columns["sm_name"].ColumnName = "STORE NAME";
                dtCompleteDetails.Columns["SUP_REPNAME"].ColumnName = "SUPPLIER/REPAIRER NAME";
                dtCompleteDetails.Columns["RSM_ISSUE_DATE"].ColumnName = "ISSUE DATE";
                dtCompleteDetails.Columns["SUP_INSP_DATE"].ColumnName = "INSPECTED DATE";
                dtCompleteDetails.Columns["INSP_BY"].ColumnName = "INSPECTED BY";
                

                List<string> listtoRemove = new List<string> { "" };
                string filename = "Faulty DTR Details" + DateTime.Now + ".xls";
                string pagetitle = "Faulty DTR Details";

                Genaral.getexcel(dtCompleteDetails, listtoRemove, filename, pagetitle);
                 
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

            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

        
    }
}