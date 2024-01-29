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
    public partial class ZoneView : System.Web.UI.Page
    {
        string strFormCode = "ZoneView.aspx";
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
                    LoadZoneDetails();
                }
            }
            
        }
        protected void cmdNewZone_Click(object sender, EventArgs e)
        {
            try
            {
                //Check AccessRights
                bool bAccResult = CheckAccessRights("2");
                if (bAccResult == false)
                {
                    return;
                }
                Response.Redirect("Zone.aspx", false);
            }
            catch (Exception ex)
            {
                // lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void LoadZoneDetails()
        {
            try
            {
                DataTable dt = new DataTable();
                clsZone objZone = new clsZone();
                dt = objZone.LoadAllZoneDetails();
                ViewState["Zone"] = dt;
                grdZonemaster.DataSource = dt;
                grdZonemaster.DataBind();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }
        protected void grdzonemaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                grdZonemaster.PageIndex = e.NewPageIndex;
                dt = (DataTable)ViewState["Zone"];
                grdZonemaster.DataSource = SortDataTable(dt as DataTable, true);
                grdZonemaster.DataBind();
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
                        ViewState["Zone"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["Zone"] = dataView.ToTable();

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

        protected void grdzonemaster_Sorting(object sender, GridViewSortEventArgs e)
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
            int pageIndex = grdZonemaster.PageIndex;
            DataTable dt = (DataTable)ViewState["Zone"];
            string sortingDirection = string.Empty;

            Image sortImage = new Image();
            Image sortImageboth = new Image();


            if (dt.Rows.Count > 0)
            {

                grdZonemaster.DataSource = SortDataTable(dt as DataTable, false);
            }

            else
            {
                grdZonemaster.DataSource = dt;

            }
            grdZonemaster.DataBind();
            grdZonemaster.PageIndex = pageIndex;
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

                String ZoneID = ((Label)rw.FindControl("lblZoneId")).Text;
                ZoneID = HttpUtility.UrlEncode(Genaral.UrlEncrypt(ZoneID));

                Response.Redirect("Zone.aspx?ZoneId=" + ZoneID + "", false);


            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }

        }


        protected void grdzonemaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                LoadZoneDetails();
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtCircleCode = (TextBox)row.FindControl("txtZoneId");
                    TextBox txtCircleName = (TextBox)row.FindControl("txtZoneName");



                    DataTable dt = (DataTable)ViewState["Zone"];
                    dv = dt.DefaultView;

                    if (txtCircleCode.Text != "")
                    {
                        sFilter = "ZO_ID Like '%" + txtCircleCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtCircleName.Text != "")
                    {
                        sFilter += "ZO_NAME Like '%" + txtCircleName.Text.Replace("'", "'") + "%' AND";
                    }

                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdZonemaster.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {


                            grdZonemaster.DataSource = dv;
                            ViewState["Zone"] = dv.ToTable();
                            grdZonemaster.DataBind();

                        }
                        else
                        {
                            ViewState["Zone"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadZoneDetails();
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
                dt.Columns.Add("ZO_ID");

                dt.Columns.Add("ZO_NAME");
                dt.Columns.Add("ZO_HEAD_EMP");
                dt.Columns.Add("ZO_MOBILE");
                




                grdZonemaster.DataSource = dt;
                grdZonemaster.DataBind();

                int iColCount = grdZonemaster.Rows[0].Cells.Count;
                grdZonemaster.Rows[0].Cells.Clear();
                grdZonemaster.Rows[0].Cells.Add(new TableCell());
                grdZonemaster.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdZonemaster.Rows[0].Cells[0].Text = "No Records Found";

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

                objApproval.sFormName = "Zone";
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

        protected void Export_clicKZone(object sender, EventArgs e)
        {
            // DataTable dt = new DataTable();
            // clsCircle objCircle = new clsCircle();
            //dt = objCircle.LoadAllCircleDetails();


            DataTable dt = (DataTable)ViewState["Zone"];


            if (dt.Rows.Count > 0)
            {

                dt.Columns["ZO_ID"].ColumnName = "ZONE ID";
                dt.Columns["ZO_NAME"].ColumnName = "ZONE NAME";
                dt.Columns["zo_head_emp"].ColumnName = "ZONE HEAD";
                dt.Columns["zo_mobile"].ColumnName = "MOBILE NUMBER";

                List<string> listtoRemove = new List<string> { "ZONE ID" };
                string filename = "ZoneDetails" + DateTime.Now + ".xls";
                string pagetitle = "Zone Details";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");
                ShowEmptyGrid();
            }

        }

        protected void grdzonemaster_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
  
    }
}