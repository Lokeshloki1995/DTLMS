using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;


namespace IIITS.DTLMS.MinorRepair
{
    public partial class ViewEstimateRate : System.Web.UI.Page
    {
        string strFormCode = "ViewEstimateRate";
        clsSession objSession = new clsSession();
        DataTable dtDetails = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];

                if (!IsPostBack)
                {
                    LoadEstimateRate();
                }
            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        private void LoadEstimateRate()
        {
            try
            {
                clsEstimate obj = new clsEstimate();
                dtDetails = obj.GetEstimationDetails();
                grdEstimation.DataSource = dtDetails;
                grdEstimation.DataBind();
                ViewState["ESTIMATE"] = dtDetails;
            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void grdEstimation_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

     
        protected void grdEstimation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdEstimation.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["ESTIMATE"];

                grdEstimation.DataSource = SortDataTable(dt as DataTable, true);
                grdEstimation.DataBind();
            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdEstimation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "create")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                    //string strRepId = ((Label)row.FindControl("lbltrID")).Text;
                    //string strCapId = ((Label)row.FindControl("lblCAP")).Text;
                    //string strWndtype = ((Label)row.FindControl("lblWndtype")).Text;
                    //string strFrom = ((Label)row.FindControl("lblFrom")).Text;
                    //string strTo = ((Label)row.FindControl("lblTo")).Text;
                    string strRecordId = ((Label)row.FindControl("lblRecordId")).Text;
                    //strRepId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(strRepId));
                    //strCapId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(strCapId));
                    //strWndtype = HttpUtility.UrlEncode(Genaral.UrlEncrypt(strWndtype));
                    //strFrom = HttpUtility.UrlEncode(Genaral.UrlEncrypt(strFrom));
                    //strTo = HttpUtility.UrlEncode(Genaral.UrlEncrypt(strTo));
                    strRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(strRecordId));
                    Response.Redirect("SaveEstimate.aspx?RecordId=" + strRecordId , false);
                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();
                    DataTable dt = new DataTable();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtventype = (TextBox)row.FindControl("txtventype");
                    TextBox txtvenname = (TextBox)row.FindControl("txtvenname");
                    TextBox txtcapacity = (TextBox)row.FindControl("txtcapacity");

                    dt = (DataTable)ViewState["ESTIMATE"];
                    dv = dt.DefaultView;

                    if (txtventype.Text != "")
                    {
                        sFilter = "VENDOR_TYPE Like '%" + txtventype.Text.Replace("'", "'") + "%' AND";
                    }

                    if (txtvenname.Text != "")
                    {
                        sFilter = "VENDOR_NAME Like '%" + txtvenname.Text.Replace("'", "'") + "%' AND";
                    }

                    if (txtcapacity.Text != "")
                    {
                        sFilter = "MD_NAME Like '%" + txtcapacity.Text.Replace("'", "'") + "%' AND";
                    }

                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdEstimation.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdEstimation.DataSource = dv;
                            //ViewState["ESTIMATE"] = dv.ToTable();
                            grdEstimation.DataBind();

                        }
                        else
                        {
                            //ViewState["ESTIMATE"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadEstimateRate();
                    }


                }
            }
            catch(Exception ex)
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
                dt.Columns.Add("DIV_NAME");
                dt.Columns.Add("VENDOR_TYPE");
                dt.Columns.Add("VENDOR_NAME");
                dt.Columns.Add("MD_NAME");
                dt.Columns.Add("WOUND_TYPE");
                dt.Columns.Add("FROM");
                dt.Columns.Add("TO");
                dt.Columns.Add("MRI_TR_ID");
                dt.Columns.Add("MRI_CAPACITY");
                dt.Columns.Add("MRI_RECORD_ID");

                grdEstimation.DataSource = dt;
                grdEstimation.DataBind();

                int iColCount = grdEstimation.Rows[0].Cells.Count;
                grdEstimation.Rows[0].Cells.Clear();
                grdEstimation.Rows[0].Cells.Add(new TableCell());
                grdEstimation.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdEstimation.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdEstimation_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                GridViewSortExpression = e.SortExpression;
                int pageIndex = grdEstimation.PageIndex;
                DataTable dt = (DataTable)ViewState["ESTIMATE"];
                string sortingDirection = string.Empty;

                if (dt.Rows.Count > 0)
                {
                    grdEstimation.DataSource = SortDataTable(dt as DataTable, false);
                }
                else
                {
                    grdEstimation.DataSource = dt;
                }
                grdEstimation.DataBind();
                grdEstimation.PageIndex = pageIndex;
            }
            catch(Exception ex)
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
                        ViewState["Make"] = dataView.ToTable();
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["Make"] = dataView.ToTable();
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

        protected void cmdNew_Click(object sender, EventArgs e)
        {            
            Response.Redirect("SaveEstimate.aspx", false);
        }
    }
}