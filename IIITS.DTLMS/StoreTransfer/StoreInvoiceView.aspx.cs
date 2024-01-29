using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;

namespace IIITS.DTLMS.StoreTransfer
{
    public partial class StoreInvoiceView : System.Web.UI.Page
    {
        string strFormCode = "StoreInvoiceView";
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
                    LoadInvoiceDetails();
                    CheckAccessRights("4");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadInvoiceDetails()
        {
            try
            {
                clsStoreInvoice objInvoice = new clsStoreInvoice();
                DataTable dt = new DataTable();
                dt = objInvoice.LoadInvoiceGrid(objSession.OfficeCode);
                grdInvoice.DataSource = dt;
                grdInvoice.DataBind();
                ViewState["StoreInvoice"] = dt;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadCompletedInvoiceDetails()
        {
            try
            {
                clsStoreInvoice objInvoice = new clsStoreInvoice();
                DataTable dt = new DataTable();
                dt = objInvoice.LoadCompletedInvoiceGrid(objSession.OfficeCode);
                grdInvoice.DataSource = dt;
                grdInvoice.DataBind();
                ViewState["StoreInvoice"] = dt;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        
        protected void cmdNew_Click(object sender, EventArgs e)
        {
            try
            {
                //Check AccessRights
                bool bAccResult = CheckAccessRights("2");
                if (bAccResult == false)
                {
                    return;
                }
                Response.Redirect("StoreInvoiceCreation.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdInvoice_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Submit")
                {
                    //Check AccessRights
                    bool bAccResult = CheckAccessRights("2");
                    if (bAccResult == false)
                    {
                        return;
                    }

                    GridViewRow row = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
                    int rowindex = row.RowIndex;
                    Label lblIndentId = (Label)grdInvoice.Rows[rowindex].FindControl("lblIndentId");
                    string strIndentId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblIndentId.Text));

                    string strRefType = string.Empty;
                    if (rdbPendingInvoice.Checked == true)
                    {
                        strRefType = "Edit";
                    }
                    if (rdbAlreadyCompleted.Checked == true)
                    {
                        strRefType = "View";
                    }
                    strRefType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(strRefType));
                    Response.Redirect("StoreInvoiceCreation.aspx?QryIndentId=" + strIndentId + "&RefType=" + strRefType, false);
                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtIndentNo = (TextBox)row.FindControl("txtIndentNo");
                    TextBox txtInvoiceNo = (TextBox)row.FindControl("txtInvoiceNo");

                    DataTable dt = (DataTable)ViewState["StoreInvoice"];
                    dv = dt.DefaultView;
                    if (txtIndentNo.Text != "")
                    {
                        sFilter = "SI_NO Like '%" + txtIndentNo.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtInvoiceNo.Text != "")
                    {
                        sFilter = "IS_NO Like '%" + txtIndentNo.Text.Replace("'", "`") + "%' AND";
                    }

                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdInvoice.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdInvoice.DataSource = dv;
                            ViewState["StoreInvoice"] = dv.ToTable();
                            grdInvoice.DataBind();

                        }
                        else
                        {
                            ViewState["StoreInvoice"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        if (rdbPendingInvoice.Checked == true)
                        {
                            LoadInvoiceDetails();
                        }
                        if (rdbAlreadyCompleted.Checked == true)
                        {
                            LoadCompletedInvoiceDetails();
                        }
                        
                    }

                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdInvoice_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdInvoice.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["StoreInvoice"];
                grdInvoice.DataSource = SortDataTable(dt as DataTable, true);
                grdInvoice.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdInvoice_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdInvoice.PageIndex;
            DataTable dt = (DataTable)ViewState["StoreInvoice"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdInvoice.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdInvoice.DataSource = dt;
            }
            grdInvoice.DataBind();
            grdInvoice.PageIndex = pageIndex;
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
                        ViewState["StoreInvoice"] = dataView.ToTable();
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["StoreInvoice"] = dataView.ToTable();
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


        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("SI_ID");
                dt.Columns.Add("SI_NO");
                dt.Columns.Add("SI_DATE");
                dt.Columns.Add("REQ_QNTY");
                dt.Columns.Add("PENDINGCOUNT");
                dt.Columns.Add("SI_FROM_STORE");
                dt.Columns.Add("IS_NO");

                grdInvoice.DataSource = dt;
                grdInvoice.DataBind();

                int iColCount = grdInvoice.Rows[0].Cells.Count;
                grdInvoice.Rows[0].Cells.Clear();
                grdInvoice.Rows[0].Cells.Add(new TableCell());
                grdInvoice.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdInvoice.Rows[0].Cells[0].Text = "No Records Found";

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



        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "StoreInvoiceCreation";
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sAccessType = "1" + "," + sAccessType;
                bool bResult = objApproval.CheckAccessRights(objApproval);
                if (bResult == false)
                {
                    if (sAccessType == "4")
                    {
                        Response.Redirect("~/UserRestrict.aspx", false);
                    }
                    else
                    {
                        ShowMsgBox("Sorry , You are not authorized to Access");
                    }
                }
                return bResult;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;

            }
        }

        #endregion

        protected void rdbPendingInvoice_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LoadInvoiceDetails();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void rdbAlreadyCompleted_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LoadCompletedInvoiceDetails();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void Export_clickStoreinvoice(object sender, EventArgs e)
        {

            //try
            //{
            //    DataTable dt = new DataTable();
            //    if (rdbAlreadyCompleted.Checked)
            //    {
            //        clsStoreInvoice objInvoice = new clsStoreInvoice();
                   
            //        dt = objInvoice.LoadCompletedInvoiceGrid(objSession.OfficeCode);
            //    }
            //    else
            //    {
            //        clsStoreInvoice objInvoice = new clsStoreInvoice();
                   
            //        dt = objInvoice.LoadInvoiceGrid(objSession.OfficeCode);

            //    }
            DataTable dt = (DataTable)ViewState["StoreInvoice"];
                if (dt.Rows.Count > 0)
                {

                    dt.Columns["SI_NO"].ColumnName = "INDENT NUMBER";
                    dt.Columns["PENDINGCOUNT"].ColumnName = "PENDING NO. TRANSFORMERS";
                    dt.Columns["REQ_QNTY"].ColumnName = "REQUESTED NO. OF TRANSFORMERS";
                    dt.Columns["SI_FROM_STORE"].ColumnName = "FROM STORE";


                    List<string> listtoRemove = new List<string> { "SI_ID", "IS_NO", "SI_DATE" };
                    string filename = "StoreInvoiceDetails" + DateTime.Now + ".xls";
                    string pagetitle = "Store Invoice Details";

                    Genaral.getexcel(dt, listtoRemove, filename,pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");

                    ShowEmptyGrid();
                }
            //}
            //catch (Exception ex)
            //{
                
            //}



        }

       
    }
}