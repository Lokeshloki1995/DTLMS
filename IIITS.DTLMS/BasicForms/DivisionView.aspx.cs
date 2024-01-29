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
    public partial class DivisionView : System.Web.UI.Page
    {
        string strFormCode = "DivisionView";
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
                    LoadDivisionDetails();
                }
            }
        }

        protected void cmdNewDivision_Click(object sender, EventArgs e)
        {
            try
            {
                //Check AccessRights
                bool bAccResult = CheckAccessRights("2");
                if (bAccResult == false)
                {
                    return;
                }

                Response.Redirect("Division.aspx", false);
            }
            catch (Exception ex)
            {
                // lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadDivisionDetails()
        {
            try
            {
                DataTable dt = new DataTable();
                clsDivision objDivision = new clsDivision();
                dt = objDivision.LoadAllDivisionDetails();
                ViewState["DIVISION"] = dt;
                grdDivisionMaster.DataSource = dt;
                grdDivisionMaster.DataBind();

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
                        ViewState["DIVISION"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["DIVISION"] = dataView.ToTable();

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

        protected void grdDivisionMaster_Sorting(object sender, GridViewSortEventArgs e)
        {


            int columnIndex = 0;
            foreach (DataControlFieldHeaderCell headerCell in grdDivisionMaster.HeaderRow.Cells)
            {
                if (headerCell.ContainingField.SortExpression == e.SortExpression)
                {
                    columnIndex = grdDivisionMaster.HeaderRow.Cells.GetCellIndex(headerCell);
                }
            }

            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdDivisionMaster.PageIndex;
            DataTable dt = (DataTable)ViewState["DIVISION"];
            string sortingDirection = string.Empty;

            Image sortImage = new Image();
            Image sortImageboth = new Image();


            if (dt.Rows.Count > 0)
            {

                grdDivisionMaster.DataSource = SortDataTable(dt as DataTable, false);
            }

            else
            {
                grdDivisionMaster.DataSource = dt;
            }

            grdDivisionMaster.DataBind();
            grdDivisionMaster.PageIndex = pageIndex;
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
                String DivID = ((Label)rw.FindControl("lblDivID")).Text;
                DivID = HttpUtility.UrlEncode(Genaral.UrlEncrypt(DivID));
                Response.Redirect("Division.aspx?DivId=" + DivID + "", false);
          
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void grdDivisionMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                grdDivisionMaster.PageIndex = e.NewPageIndex;
                dt = (DataTable)ViewState["DIVISION"];

                grdDivisionMaster.DataSource = SortDataTable(dt as DataTable, true);

                grdDivisionMaster.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void grdDivisionMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                LoadDivisionDetails();
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDivCode = (TextBox)row.FindControl("txtDivisionCode");
                    TextBox txtDivName = (TextBox)row.FindControl("txtDivisionName");

                    DataTable dt = (DataTable)ViewState["DIVISION"];
                    dv = dt.DefaultView;

                    if (txtDivCode.Text != "")
                    {
                        sFilter = "DIV_CODE Like '%" + txtDivCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtDivName.Text != "")
                    {
                        sFilter += " DIV_NAME Like '%" + txtDivName.Text.Replace("'", "'") + "%' AND";
                    }

                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdDivisionMaster.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdDivisionMaster.DataSource = dv;
                            ViewState["DIVISION"] = dv.ToTable();
                            grdDivisionMaster.DataBind();

                        }
                        else
                        {
                            ViewState["DIVISION"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadDivisionDetails();
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
                dt.Columns.Add("DIV_ID");
                dt.Columns.Add("DIV_CODE");
                dt.Columns.Add("DIV_NAME");
                dt.Columns.Add("DIV_HEAD_EMP");
                dt.Columns.Add("DIV_MOBILE_NO");
                



                grdDivisionMaster.DataSource = dt;
                grdDivisionMaster.DataBind();

                int iColCount = grdDivisionMaster.Rows[0].Cells.Count;
                grdDivisionMaster.Rows[0].Cells.Clear();
                grdDivisionMaster.Rows[0].Cells.Add(new TableCell());
                grdDivisionMaster.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdDivisionMaster.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }
        protected void Export_clicKDivision(object sender, EventArgs e)
        {
            //DataTable dt = new DataTable();
            //clsDivision objDivision = new clsDivision();
            //dt = objDivision.LoadAllDivisionDetails();
            DataTable dt = (DataTable)ViewState["DIVISION"];

            if (dt.Rows.Count > 0)
            {

                dt.Columns["div_code"].ColumnName = "DIVISION CODE";
                dt.Columns["div_name"].ColumnName = "DIVISION NAME";
                dt.Columns["div_mobile_no"].ColumnName = "MOBILE NO";
                dt.Columns["div_head_emp"].ColumnName = "DIVISION HEAD";

                List<string> listtoRemove = new List<string> { "DIV_ID" };
                string filename = "DivisionDetails" + DateTime.Now + ".xls";
                string pageTitle = "Division Details";

                Genaral.getexcel(dt, listtoRemove, filename, pageTitle);
            }
            else
            {
                ShowMsgBox("No record found");
                ShowEmptyGrid();
            }

        }

        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "Division";
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
    }
}