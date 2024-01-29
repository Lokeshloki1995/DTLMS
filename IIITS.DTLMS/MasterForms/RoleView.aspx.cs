using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;

using System.IO;

namespace IIITS.DTLMS.MasterForms
{
   
    public partial class RoleView : System.Web.UI.Page
    {
        string strFormCode = "RoleView.aspx";
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
                    CheckAccessRights("4");
                    LoadRoleGrid();

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void LoadRoleGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                clsRole objRole = new clsRole();
                dt = objRole.LoadDetails();
                grdRoleDetails.DataSource = dt;
                ViewState["Role"] = dt;
                grdRoleDetails.DataBind();
                

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdRoleDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdRoleDetails.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Role"];

                grdRoleDetails.DataSource = SortDataTable(dt as DataTable, true);
                grdRoleDetails.DataBind();

                //LoadRoleGrid();

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
                        ViewState["Role"] = dataView.ToTable();


                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["Role"] = dataView.ToTable();

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

        protected void grdRole_Sorting(object sender, GridViewSortEventArgs e)
        {

            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdRoleDetails.PageIndex;
            DataTable dt = (DataTable)ViewState["Role"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {

                grdRoleDetails.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdRoleDetails.DataSource = dt;
            }
            grdRoleDetails.DataBind();
            grdRoleDetails.PageIndex = pageIndex;
        }

      

        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "Role";
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
                Response.Redirect("Role.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                ImageButton imgEdit = (ImageButton)sender;
                GridViewRow rw = (GridViewRow)imgEdit.NamingContainer;
                String RoleId = ((Label)rw.FindControl("lblId")).Text;
                RoleId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(RoleId));
                Response.Redirect("Role.aspx?StrQryId=" + RoleId + "", false);
            }

            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }

        }
        
        protected void grdRoleDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            LoadRoleGrid();
            if (e.CommandName == "search")
            {
                string sFilter = string.Empty;
                DataView dv = new DataView();

                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                TextBox txtRole = (TextBox)row.FindControl("txtRole");

                DataTable dt = (DataTable)ViewState["Role"];
                dv = dt.DefaultView;

                if (txtRole.Text != "")
                {
                    sFilter = "RO_NAME Like '%" + txtRole.Text.Replace("'", "'") + "%'";
                }

                if (sFilter.Length > 0)
                {

                    grdRoleDetails.PageIndex = 0;
                    dv.RowFilter = sFilter;
                    if (dv.Count > 0)
                    {


                        // ViewState["Role"] = null;
                        ViewState["Role"] = dv.ToTable();
                        //ViewState.Add("Role1", dv);


                        grdRoleDetails.DataSource = dv;
                        grdRoleDetails.DataBind();

                    }
                    else
                    {
                        ViewState["Role"] = dv.ToTable();
                        ShowEmptyGrid();
                    }
                }

                else
                {
                    LoadRoleGrid();
                }
            }

        }

        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("RO_ID");
                dt.Columns.Add("RO_NAME");
                dt.Columns.Add("RO_DESIGNATION");


                grdRoleDetails.DataSource = dt;
                grdRoleDetails.DataBind();

                int iColCount = grdRoleDetails.Rows[0].Cells.Count;
                grdRoleDetails.Rows[0].Cells.Clear();
                grdRoleDetails.Rows[0].Cells.Add(new TableCell());
                grdRoleDetails.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdRoleDetails.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }

      

       

        public int GetSortColumnIndex()
        {
            // Iterate through the Columns collection to determine the index 
            // of the column being sorted. 
            foreach (DataControlField field in grdRoleDetails.Columns)
            {
                if (field.SortExpression == this.grdRoleDetails.SortExpression)
                {
                    return this.grdRoleDetails.Columns.IndexOf(field);
                }
            }
            return -1;
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

        protected void Export_clickRole(object sender, EventArgs e)
        {
            DataTable dt1 = new DataTable();
            clsRole objRole = new clsRole();
           // dt = objRole.LoadDetails();



            DataTable dt = (DataTable)ViewState["Role"];

            if (dt.Rows.Count > 0)
            {
                dt.Columns["RO_NAME"].ColumnName = "ROLE ";
                dt.Columns["RO_DESIGNATION"].ColumnName = "DESIGNATION";

                List<string> listtoRemove = new List<string> { "RO_ID" };
                string filename = "RoleDetails" + DateTime.Now + ".xls";
                string pagetitle = "Role Details";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No records found");
                ShowEmptyGrid();
            }



        }


    }

}