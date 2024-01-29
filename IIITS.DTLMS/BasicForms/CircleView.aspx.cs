using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Text.RegularExpressions;

namespace IIITS.DTLMS.BasicForms
{
    public partial class CircleView : System.Web.UI.Page
    {
        string strFormCode = "CircleView.aspx";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
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
                    CheckAccessRights("4");
                    LoadCircleDetails();
                }
            }
            
        }

        protected void cmdNewCircle_Click(object sender, EventArgs e)
        {
            try
            {
                //Check AccessRights
                bool bAccResult = CheckAccessRights("2");
                if (bAccResult == false)
                {
                    return;
                }
                Response.Redirect("Circle.aspx", false);
            }
            catch (Exception ex)
            {
                // lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void LoadCircleDetails()
        {
            try
            {
                DataTable dt = new DataTable();
                clsCircle objCircle = new clsCircle();
                dt = objCircle.LoadAllCircleDetails();
                ViewState["Circle"] = dt;
                grdCirclemaster.DataSource = dt;
                grdCirclemaster.DataBind();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }



        protected void grdCirclemaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                grdCirclemaster.PageIndex = e.NewPageIndex;
                dt = (DataTable)ViewState["Circle"];
                grdCirclemaster.DataSource = SortDataTable(dt as DataTable, true);
                grdCirclemaster.DataBind();
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
                        ViewState["Circle"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["Circle"] = dataView.ToTable();

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

        protected void grdCirclemaster_Sorting(object sender, GridViewSortEventArgs e)
        {


            int columnIndex = 0;
            //foreach (DataControlFieldHeaderCell headerCell in grdCirclemaster.HeaderRow.Cells)
            //{
            //    if (headerCell.ContainingField.SortExpression == e.SortExpression)
            //    {
            //        columnIndex = grdCirclemaster.HeaderRow.Cells.GetCellIndex(headerCell);
            //    }
            //}

            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdCirclemaster.PageIndex;
            DataTable dt = (DataTable)ViewState["Circle"];
            string sortingDirection = string.Empty;

            Image sortImage = new Image();
            Image sortImageboth = new Image();


            if (dt.Rows.Count > 0)
            {

                grdCirclemaster.DataSource = SortDataTable(dt as DataTable, false);
            }

            else
            {
                grdCirclemaster.DataSource = dt;

            }
            grdCirclemaster.DataBind();
            grdCirclemaster.PageIndex = pageIndex;
        }


        protected void imgBtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                //Check AccessRights
                bool bAccResult = CheckAccessRights("3");
                if (bAccResult == false)
                {
                    return;
                }

                ImageButton imgEdit = (ImageButton)sender;
                GridViewRow rw = (GridViewRow)imgEdit.NamingContainer;

                String CircleID = ((Label)rw.FindControl("lblCirId")).Text;
                CircleID = HttpUtility.UrlEncode(Genaral.UrlEncrypt(CircleID));

                Response.Redirect("Circle.aspx?CircleId=" + CircleID + "", false);


            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }

        }

        
        protected void grdCirclemaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                LoadCircleDetails();
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtCircleCode = (TextBox)row.FindControl("txtCircleCode");
                    TextBox txtCircleName = (TextBox)row.FindControl("txtCircleName");



                    DataTable dt = (DataTable)ViewState["Circle"];
                    dv = dt.DefaultView;

                    if (txtCircleCode.Text != "")
                    {
                        sFilter = "CM_CIRCLE_CODE Like '%" + txtCircleCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtCircleName.Text != "")
                    {
                        sFilter += "CM_CIRCLE_NAME Like '%" + txtCircleName.Text.Replace("'", "'") + "%' AND";
                    }

                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdCirclemaster.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {


                            grdCirclemaster.DataSource = dv;
                            ViewState["Circle"] = dv.ToTable();
                            grdCirclemaster.DataBind();

                        }
                        else
                        {
                            ViewState["Circle"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadCircleDetails();
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
                dt.Columns.Add("CM_ID");
                dt.Columns.Add("CM_CIRCLE_CODE");
                dt.Columns.Add("CM_CIRCLE_NAME");
                dt.Columns.Add("CM_HEAD_EMP");
                dt.Columns.Add("CM_MOBILE_NO");
                




                grdCirclemaster.DataSource = dt;
                grdCirclemaster.DataBind();

                int iColCount = grdCirclemaster.Rows[0].Cells.Count;
                grdCirclemaster.Rows[0].Cells.Clear();
                grdCirclemaster.Rows[0].Cells.Add(new TableCell());
                grdCirclemaster.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdCirclemaster.Rows[0].Cells[0].Text = "No Records Found";

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

                objApproval.sFormName = "Circle";
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

        protected void Export_clicKcircle(object sender, EventArgs e)
        {
           // DataTable dt = new DataTable();
           // clsCircle objCircle = new clsCircle();
            //dt = objCircle.LoadAllCircleDetails();


            DataTable dt = (DataTable)ViewState["Circle"];


            if (dt.Rows.Count > 0)
            {

                dt.Columns["CM_CIRCLE_CODE"].ColumnName = "CIRCLE CODE";
                dt.Columns["CM_CIRCLE_NAME"].ColumnName = "CIRCLE NAME";
                dt.Columns["cm_head_emp"].ColumnName = "CIRCLE HEAD";
                dt.Columns["cm_mobile_no"].ColumnName = "PHONE NUMBER";

                List<string> listtoRemove = new List<string> { "CM_ID" };
                string filename = "CircleDetails" + DateTime.Now + ".xls";
                string pagetitle = "Circle Details";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");
                ShowEmptyGrid();
            }

        }

        protected void grdCirclemaster_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}