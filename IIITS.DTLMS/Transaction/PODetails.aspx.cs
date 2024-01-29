using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;

namespace IIITS.DTLMS.Transaction
{
    public partial class PODetails : System.Web.UI.Page
    {
        public string strFormCode = "PODetails";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];

                if (!IsPostBack)
                {
                    if (Request.QueryString["POID"] != null && Request.QueryString["POID"].ToString() != "")
                    {
                        txtPOId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["POID"]));
                        txtPONumber.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["PONO"]));
                        txtPODate.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["PODATE"]));
                        LoadDeliveryDetails();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        private void LoadDeliveryDetails()
        {
            try
            {
                clsPoTracker obj = new clsPoTracker();
                obj.sPoId = txtPOId.Text;
                obj.GetDeliveryDetails(obj);
                grdPODetails.DataSource = obj.dtDIDetails;
                grdPODetails.DataBind();
                ViewState["DIDETAILS"] = obj.dtDIDetails;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdPODetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdPODetails.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["DIDETAILS"];
                grdPODetails.DataSource = SortDataTable(dt as DataTable, true);
                grdPODetails.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdPODetails_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                GridViewSortExpression = e.SortExpression;
                int pageIndex = grdPODetails.PageIndex;
                DataTable dt = (DataTable)ViewState["DIDETAILS"];
                string sortingDirection = string.Empty;
                if (dt.Rows.Count > 0)
                {
                    grdPODetails.DataSource = SortDataTable(dt as DataTable, false);
                }
                else
                {
                    grdPODetails.DataSource = dt;
                }
                grdPODetails.DataBind();
                grdPODetails.PageIndex = pageIndex;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
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
                        ViewState["DIDETAILS"] = dataView.ToTable();
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["DIDETAILS"] = dataView.ToTable();
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

        protected void lnkBtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton imgEdit = (LinkButton)sender;
                GridViewRow rw = (GridViewRow)imgEdit.NamingContainer;
                String sDIId = ((Label)rw.FindControl("lblDelNO")).Text;
                clsPoTracker obj = new clsPoTracker();
                obj.sDINo = sDIId;
                obj.GetTCDetails(obj);
                grdTCDetails.DataSource = obj.dtDTRDetails;
                grdTCDetails.DataBind();
                ViewState["DTRDETAILS"] = obj.dtDTRDetails;
                dvDTR.Visible = true;
                dvDTR.Style.Add("display", "block");
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdTCDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdTCDetails.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["DTRDETAILS"];
                grdTCDetails.DataSource = SortDataTable(dt as DataTable, true);
                grdTCDetails.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdTCDetails_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdPODetails_RowCommand_For_PODetails(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDiNO = (TextBox)row.FindControl("txtDiNO");
                    TextBox txtStore = (TextBox)row.FindControl("txtStore");
                    TextBox txtMake = (TextBox)row.FindControl("txtMake");

                    DataTable dt = (DataTable)ViewState["DIDETAILS"];
                    dv = dt.DefaultView;

                    if (txtDiNO.Text != "")
                    {
                        sFilter = "DI_NO Like '%" + txtDiNO.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtStore.Text != "")
                    {
                        sFilter += " SM_NAME Like '%" + txtStore.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtMake.Text != "")
                    {
                        sFilter += " TM_NAME Like '%" + txtMake.Text.Replace("'", "'") + "%' AND";
                    }

                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdPODetails.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdPODetails.DataSource = dv;
                            ViewState["DIDETAILS"] = dv.ToTable();
                            grdPODetails.DataBind();

                        }
                        else
                        {
                            ViewState["DIDETAILS"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadDeliveryDetails();
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
                dt.Columns.Add("DI_ID");
                dt.Columns.Add("DI_NO");
                dt.Columns.Add("SM_NAME");
                dt.Columns.Add("TM_NAME");
                dt.Columns.Add("DI_CAPACITY");
                dt.Columns.Add("DI_DATE");
                dt.Columns.Add("DI_QUANTITY");
                dt.Columns.Add("INWARD");
                dt.Columns.Add("PENDING");

                grdPODetails.DataSource = dt;
                grdPODetails.DataBind();

                int iColCount = grdPODetails.Rows[0].Cells.Count;
                grdPODetails.Rows[0].Cells.Clear();
                grdPODetails.Rows[0].Cells.Add(new TableCell());
                grdPODetails.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdPODetails.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }
    }
}