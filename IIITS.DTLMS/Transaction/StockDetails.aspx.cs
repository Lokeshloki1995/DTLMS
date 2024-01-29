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
    public partial class StockDetails : System.Web.UI.Page
    {
        string strFormCode = "StockDetails";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;
                if (!IsPostBack)
                {
                    string sStoreId = string.Empty;
                    LoadCombo();
                    if (Request.QueryString["StoreID"] != null && Request.QueryString["StoreID"].ToString() != "")
                    {
                        txtStoreId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["StoreID"]));
                        clsStockStatus obj = new clsStockStatus();
                        string sStoreid = obj.GetStoreID(txtStoreId.Text);
                        cmbStore.SelectedValue = sStoreid;
                    }                    
                    LoadTcDetails();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadCombo()
        {
            try
            {
                Genaral.Load_Combo("SELECT \"TM_ID\",\"TM_NAME\" FROM \"TBLTRANSMAKES\" ORDER BY \"TM_NAME\"", "-Select-", cmbMake);
                Genaral.Load_Combo("SELECT  \"SM_ID\", \"SM_NAME\" FROM \"TBLSTOREMAST\" ORDER BY \"SM_NAME\"", " -Select-", cmbStore);
                Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'SR' ORDER BY \"MD_ORDER_BY\" ", "-Select-", cmbrating);
                Genaral.Load_Combo("SELECT \"MD_NAME\",\"MD_NAME\" from \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='C' ORDER BY \"MD_ORDER_BY\"", "-Select-", cmbCapacity);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadTcDetails()
        {
            try
            {
                clsStockStatus objstock = new clsStockStatus();
                objstock.sStoreId = objSession.OfficeCode;

                if (cmbMake.SelectedIndex > 0)
                {
                    objstock.sMake = cmbMake.SelectedValue;
                }
                if (cmbCapacity.SelectedIndex > 0)
                {
                    objstock.sCapacity = cmbCapacity.SelectedValue;
                }
                if (cmbStore.SelectedIndex > 0)
                {
                    objstock.sStoreId = cmbStore.SelectedValue;
                }
                if (cmbrating.SelectedIndex > 0)
                {
                    objstock.sRating = cmbrating.SelectedValue;
                }

                DataTable dt = objstock.LoadTcDetails(objstock);
                if (dt.Rows.Count == 0)
                {
                    ShowEmptyGrid();
                    ViewState["TC"] = dt;
                }
                else
                {
                    grdTcMaster.DataSource = dt;
                    grdTcMaster.DataBind();
                    ViewState["TC"] = dt;
                }

                lblTotalDTr.Text = "Total Transformer Count : " + Convert.ToString(dt.Rows.Count);

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
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

        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("TC_ID");
                dt.Columns.Add("TC_CODE");
                dt.Columns.Add("TC_SLNO");
                dt.Columns.Add("TC_MAKE_ID");
                dt.Columns.Add("TC_CAPACITY");
                dt.Columns.Add("TC_LIFE_SPAN");
                dt.Columns.Add("TC_RATING");


                grdTcMaster.DataSource = dt;
                grdTcMaster.DataBind();

                int iColCount = grdTcMaster.Rows[0].Cells.Count;
                grdTcMaster.Rows[0].Cells.Clear();
                grdTcMaster.Rows[0].Cells.Add(new TableCell());
                grdTcMaster.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdTcMaster.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                LoadTcDetails();
            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdTcMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdTcMaster.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["TC"];
                grdTcMaster.DataSource = SortDataTable(dt as DataTable, true);
                grdTcMaster.DataBind();
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
                        ViewState["TC"] = dataView.ToTable();
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["TC"] = dataView.ToTable();
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

        protected void grdTcMaster_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                GridViewSortExpression = e.SortExpression;
                int pageIndex = grdTcMaster.PageIndex;
                DataTable dt = (DataTable)ViewState["TC"];
                string sortingDirection = string.Empty;
                if (dt.Rows.Count > 0)
                {
                    grdTcMaster.DataSource = SortDataTable(dt as DataTable, false);
                }
                else
                {
                    grdTcMaster.DataSource = dt;
                }
                grdTcMaster.DataBind();
                grdTcMaster.PageIndex = pageIndex;
            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}