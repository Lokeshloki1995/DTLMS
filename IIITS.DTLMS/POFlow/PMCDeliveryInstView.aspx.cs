using IIITS.DTLMS.BL;
using IIITS.DTLMS.BL.POFlow;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.POFlow
{
    public partial class PMCDeliveryInstView : System.Web.UI.Page
    {
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToString(Session["clsSession"] ?? "").Length == 0)
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;
                if (!IsPostBack)
                {
                    loadDeliveryInstGrid();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void loadDeliveryInstGrid(string sDINumber = "")
        {
            try
            {
                clsPMCDelivery Obj = new clsPMCDelivery();
                DataTable dt = new DataTable();
                Obj.PMCDIid = sDINumber;

                dt = Obj.GetDeliveryInstDetails(Obj);

                if (dt.Rows.Count > 0)
                {
                    grdPMCDelivery.DataSource = dt;
                    grdPMCDelivery.DataBind();
                }
                else
                {
                    ShowEmptyGrid();
                }
                ViewState["PMCDI_Details"] = dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void grdPMCDelivery_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdPMCDelivery.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["PMCDI_Details"];
                grdPMCDelivery.DataSource = SortDataTable(dt as DataTable, true);
                grdPMCDelivery.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdPMCDelivery_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdPMCDelivery.PageIndex;
            DataTable dt = (DataTable)ViewState["PMCDI_Details"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdPMCDelivery.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdPMCDelivery.DataSource = dt;
            }
            grdPMCDelivery.DataBind();
            grdPMCDelivery.PageIndex = pageIndex;
        }
        protected void grdPMCDelivery_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Submit")
                {

                    GridViewRow row = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
                    int rowindex = row.RowIndex;

                    Label lblPMCDIid = (Label)grdPMCDelivery.Rows[rowindex].FindControl("lblPMCDIid");
                    Label lblPMCPoId = (Label)grdPMCDelivery.Rows[rowindex].FindControl("lblPMCPoId");
                    Label lblDiNo = (Label)grdPMCDelivery.Rows[rowindex].FindControl("lblDiNo");
                    Label lblDWAid = (Label)grdPMCDelivery.Rows[rowindex].FindControl("lblDWAid");

                    string PMCDIid = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblPMCDIid.Text));
                    string PMCPoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblPMCPoId.Text));
                    string PMCDiNum = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblDiNo.Text));
                    string DWAid = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblDWAid.Text));

                    Response.Redirect("PMCDeliveryInst.aspx?QryPMCDIid=" + PMCDIid + "&QryPMCPoId=" + PMCPoId + "&QryPMCDiNo=" + PMCDiNum + "&QryDWAid=" + DWAid, false);
                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();
                    DataTable dt = new DataTable();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDWANo = (TextBox)row.FindControl("txtDWANo");
                    TextBox txtPONo = (TextBox)row.FindControl("txtPONo");
                    TextBox txtDINo = (TextBox)row.FindControl("txtDINumber");

                    if ((txtDWANo.Text ?? "").Length > 0)
                    {
                        sFilter = "DWA_No Like '%" + txtDWANo.Text.Replace("'", "'") + "%' AND";
                    }
                    if ((txtPONo.Text ?? "").Length > 0)
                    {
                        sFilter = "PO_No Like '%" + txtPONo.Text.Replace("'", "'") + "%' AND";
                    }
                    if ((txtDINo.Text ?? "").Length > 0)
                    {
                        sFilter = "DI_No Like '%" + txtDINo.Text.Replace("'", "'") + "%' AND";
                    }

                    dt = (DataTable)ViewState["PMCDI_Details"];
                    dv = dt.DefaultView;
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdPMCDelivery.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdPMCDelivery.DataSource = dv;
                            grdPMCDelivery.DataBind();
                            ViewState["PMCDI_Details"] = dv.ToTable();
                        }
                        else
                        {
                            clsPMCDelivery Obj = new clsPMCDelivery();
                            Obj.PMCDWANumber = txtDWANo.Text;
                            Obj.PMCPoNumber = txtPONo.Text;
                            Obj.PMCDINumber = txtDINo.Text;
                            dt = Obj.FetchPMCDeliveryInsDetails(Obj);

                            if (dt.Rows.Count > 0)
                            {
                                grdPMCDelivery.DataSource = dt;
                                grdPMCDelivery.DataBind();
                            }
                            else
                            {
                                ShowEmptyGrid();
                            }
                            ViewState["PMCDI_Details"] = dt;
                        }
                    }
                    else
                    {
                        loadDeliveryInstGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
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
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                    }
                    ViewState["PMCDI_Details"] = dataView.ToTable();
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
        public SortDirection direction
        {
            get
            {
                if (ViewState["directionState"] == null)
                {
                    ViewState["directionState"] = SortDirection.Ascending;
                }
                return (SortDirection)ViewState["directionState"];
            }
            set
            {
                ViewState["directionState"] = value;
            }
        }

        private void ShowMsgBox(string sMsg)
        {
            try
            {
                string sShowMsg = string.Empty;
                sShowMsg = "<script language=javascript> alert ('" + sMsg + "')</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "Msg", sShowMsg);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmdNew_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("PMCDeliveryInst.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("PMC_DIM_ID");
                dt.Columns.Add("DWA_Id");
                dt.Columns.Add("PMC_PO_Id");
                dt.Columns.Add("DWA_No");
                dt.Columns.Add("PO_No");

                dt.Columns.Add("DI_No");
                dt.Columns.Add("DI_DATE");
                dt.Columns.Add("DI_STORE");
                dt.Columns.Add("DI_MAKE");
                dt.Columns.Add("DI_STARRATE");

                dt.Columns.Add("DI_STARRATENAME");
                dt.Columns.Add("DI_CAPACITY");
                dt.Columns.Add("DI_QUANTITY");
                dt.Columns.Add("DI_DUEDATE");
                dt.Columns.Add("DI_START_RANGE");
                dt.Columns.Add("DI_END_RANGE");

                grdPMCDelivery.DataSource = dt;
                grdPMCDelivery.DataBind();

                int iColCount = grdPMCDelivery.Rows[0].Cells.Count;
                grdPMCDelivery.Rows[0].Cells.Clear();
                grdPMCDelivery.Rows[0].Cells.Add(new TableCell());
                grdPMCDelivery.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdPMCDelivery.Rows[0].Cells[0].Text = Convert.ToString(ConfigurationManager.AppSettings["EmptyData"]);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void Export_click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();

            if ((DataTable)(ViewState["PMCDI_Details"]) != null)
            {
                dt = (DataTable)ViewState["PMCDI_Details"];
            }
            else
            {
                ShowMsgBox(Convert.ToString(ConfigurationManager.AppSettings["EmptyData"]));
                return;
            }

            if (dt.Rows.Count > 0)
            {
                #region adding Slno logic
                int Slno = 0;
                dt.Columns.Add("SlNo", typeof(int));
                foreach (DataRow row in dt.Rows)
                {
                    #region adding Slno.
                    Slno = Slno + 1;
                    row["SlNo"] = Slno;
                    #endregion
                }
                #endregion

                dt.Columns["DWA_No"].ColumnName = "DWA No";
                dt.Columns["PO_No"].ColumnName = "PO No";
                dt.Columns["DI_No"].ColumnName = "DI No";
                dt.Columns["DI_DATE"].ColumnName = "Delivery Date";
                dt.Columns["DI_STORE"].ColumnName = "Store Name";
                dt.Columns["DI_MAKE"].ColumnName = "Make Name";
                dt.Columns["DI_STARRATENAME"].ColumnName = "Rating";
                dt.Columns["DI_CAPACITY"].ColumnName = "Capacity";
                dt.Columns["DI_QUANTITY"].ColumnName = "Quantity";
                dt.Columns["DI_DUEDATE"].ColumnName = "Due Date";
                dt.Columns["DI_START_RANGE"].ColumnName = "DTr Start Range";
                dt.Columns["DI_END_RANGE"].ColumnName = "DTr End Range";

                dt.Columns["SlNo"].SetOrdinal(0);
                dt.Columns["DWA No"].SetOrdinal(1);
                dt.Columns["PO No"].SetOrdinal(2);
                dt.Columns["DI No"].SetOrdinal(3);
                dt.Columns["Delivery Date"].SetOrdinal(4);
                dt.Columns["Store Name"].SetOrdinal(5);
                dt.Columns["Make Name"].SetOrdinal(6);
                dt.Columns["Rating"].SetOrdinal(7);
                dt.Columns["Capacity"].SetOrdinal(8);
                dt.Columns["Quantity"].SetOrdinal(9);
                dt.Columns["Due Date"].SetOrdinal(10);
                dt.Columns["DTr Start Range"].SetOrdinal(11);
                dt.Columns["DTr End Range"].SetOrdinal(12);

                List<string> listtoRemove = new List<string> { "PMC_DIM_ID", "DWA_Id", "PMC_PO_Id", "DI_STARRATE" };
                string filename = "DIDetails" + DateTime.Now + ".xls";
                string pagetitle = "Delivery Instruction Details";
                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowEmptyGrid();
            }
        }
    }
}