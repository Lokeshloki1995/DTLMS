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
    public partial class StoreIndentView : System.Web.UI.Page
    {
        string strFormCode = "StoreIndentView";
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
                    LoadIndentDetails();
                    CheckAccessRights("4");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadIndentDetails()
        {
            try
            {
                clsStoreIndent objTcTransfer = new clsStoreIndent();
                DataTable dt = new DataTable();
                dt = objTcTransfer.LoadIndentGrid(objSession.OfficeCode);
                grdIndentView.DataSource = dt;
                grdIndentView.DataBind();
                ViewState["StoreIndent"] = dt;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadCompletedIndentDetails()
        {
            try
            {
                clsStoreIndent objTcTransfer = new clsStoreIndent();
                DataTable dt = new DataTable();
                dt = objTcTransfer.LoadCompletedIndentGrid(objSession.OfficeCode);
                grdIndentView.DataSource = dt;
                grdIndentView.DataBind();
                ViewState["StoreIndent"] = dt;
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
                Response.Redirect("StoreIndent.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdIndentView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdIndentView.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["StoreIndent"];
                grdIndentView.DataSource = SortDataTable(dt as DataTable, true);
                grdIndentView.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdIndentView_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdIndentView.PageIndex;
            DataTable dt = (DataTable)ViewState["StoreIndent"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {

                grdIndentView.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdIndentView.DataSource = dt;
            }
            grdIndentView.DataBind();
            grdIndentView.PageIndex = pageIndex;
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
                        ViewState["StoreIndent"] = dataView.ToTable();
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["StoreIndent"] = dataView.ToTable();

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
        protected void grdIndentView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                clsStoreIndent objTcTransfer = new clsStoreIndent();
                if (e.CommandName == "Submit")
                {

                    //Check AccessRights
                    bool bAccResult = CheckAccessRights("3");
                    if (bAccResult == false)
                    {
                        return;
                    }

                    GridViewRow row = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
                    int rowindex = row.RowIndex;
                    Label lblIndentId = (Label)grdIndentView.Rows[rowindex].FindControl("lblIndentId");
                    string strIndentId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblIndentId.Text));
                    bool istrue = objTcTransfer.CheckForInvoice(lblIndentId.Text);
                     if (istrue)
                     {
                         //ShowMsgBox("Invoice Already Created...You Cannot Modify");
                         //return;
                     }
                    Response.Redirect("StoreIndent.aspx?QryIndentId=" + strIndentId,false);
                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtIndentNo = (TextBox)row.FindControl("txtIndentNo");


                    DataTable dt = (DataTable)ViewState["StoreIndent"];
                    dv = dt.DefaultView;
                    if (txtIndentNo.Text != "")
                    {
                        sFilter = "SI_NO Like '%" + txtIndentNo.Text.Replace("'", "`") + "%' AND";
                    }

                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdIndentView.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdIndentView.DataSource = dv;
                            ViewState["StoreIndent"] = dv.ToTable();
                            grdIndentView.DataBind();

                        }
                        else
                        {
                            ViewState["StoreIndent"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        if (rdbPendingIndent.Checked == true)
                        {
                            LoadIndentDetails();
                        }
                        if (rdbAlreadyCompleted.Checked == true)
                        {
                            LoadCompletedIndentDetails();
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
                dt.Columns.Add("SO_QNTY");
                dt.Columns.Add("SI_TO_STORE");


                grdIndentView.DataSource = dt;
                grdIndentView.DataBind();

                int iColCount = grdIndentView.Rows[0].Cells.Count;
                grdIndentView.Rows[0].Cells.Clear();
                grdIndentView.Rows[0].Cells.Add(new TableCell());
                grdIndentView.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdIndentView.Rows[0].Cells[0].Text = "No Records Found";

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

                objApproval.sFormName = "StoreIndent";
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

        protected void rdbPendingIndent_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LoadIndentDetails();
               
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
                LoadCompletedIndentDetails();
               
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void Export_clickStoreindent(object sender, EventArgs e)
        {

            //try
            //{
            //    DataTable dt = new DataTable();
            //    if (rdbAlreadyCompleted.Checked)
            //    {
            //        clsStoreIndent objTcTransfer = new clsStoreIndent();
                 
            //        dt = objTcTransfer.LoadCompletedIndentGrid(objSession.OfficeCode);
            //    }
            //    else
            //    {
            //        clsStoreIndent objTcTransfer = new clsStoreIndent();
                   
            //        dt = objTcTransfer.LoadIndentGrid(objSession.OfficeCode);

            //    }
            DataTable dt = (DataTable)ViewState["StoreIndent"];
                if (dt.Rows.Count > 0)
                {

                    dt.Columns["SI_NO"].ColumnName = "INDENT NUMBER";
                    dt.Columns["SI_DATE"].ColumnName = "INDENT DATE";
                    dt.Columns["SO_QNTY"].ColumnName = "REQUESTED NO. OF TRANSFORMERS";
                    dt.Columns["SI_TO_STORE"].ColumnName = "TO STORE";


                    dt.Columns["INDENT NUMBER"].SetOrdinal(1);

                    List<string> listtoRemove = new List<string> { "SI_ID" };
                    string filename = "StoreIndentDetails" + DateTime.Now + ".xls";
                    string pagetitle = "Store Indent Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
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