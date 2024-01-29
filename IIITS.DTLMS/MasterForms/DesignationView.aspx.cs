using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;

namespace IIITS.DTLMS.MasterForms
{
    public partial class DesignationView : System.Web.UI.Page
    {       
        string strFormCode = "DesignationView.aspx";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                else
                {
                    objSession = (clsSession)Session["clsSession"];
                    lblMessage.Text = string.Empty;
                    if (!IsPostBack)
                    {
                        CheckAccessRights("4");
                        LoadDesignationGrid();
                    }
                }
                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
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
                Response.Redirect("Designation.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void LoadDesignationGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                clsDesignation objDesgn = new clsDesignation();

                dt = objDesgn.LoadDetails();
                ViewState["Designationdetails"] = dt;
                grdDesignationDetails.DataSource = dt;
                grdDesignationDetails.DataBind();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdDesignationDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdDesignationDetails.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Designationdetails"];

                grdDesignationDetails.DataSource = SortDataTable(dt as DataTable, true);
                grdDesignationDetails.DataBind();
               // LoadDesignationGrid();
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
                        dataView.Sort = string.Format("{0} {1}  ", GridViewSortExpression, GridViewSortDirection);
                        ViewState["Designationdetails"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["Designationdetails"] = dataView.ToTable();

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

        protected void grdDesignationDetails_Sorting(object sender, GridViewSortEventArgs e)
        {            
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdDesignationDetails.PageIndex;
            DataTable dt = (DataTable)ViewState["Designationdetails"];
            string sortingDirection = string.Empty;            

            if (dt.Rows.Count > 0)
            {

                grdDesignationDetails.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdDesignationDetails.DataSource = dt;
            }
            grdDesignationDetails.DataBind();
            grdDesignationDetails.PageIndex = pageIndex;
        }

        protected void grdDesignationDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            LoadDesignationGrid();
            if (e.CommandName == "search")
            {
                string sFilter = string.Empty;
                DataView dv = new DataView();

                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                TextBox txtDesignation = (TextBox)row.FindControl("txtDesignation");

                DataTable dt = (DataTable)ViewState["Designationdetails"];
                dv = dt.DefaultView;

                if (txtDesignation.Text != "")
                {
                    sFilter = "DM_NAME Like '%" + txtDesignation.Text.Replace("'", "'") + "%'";
                }

                if (sFilter.Length > 0)
                {

                    grdDesignationDetails.PageIndex = 0;
                    dv.RowFilter = sFilter;
                    if (dv.Count > 0)
                    {
                        ViewState["Designationdetails"] = dv.ToTable();
                      
                        grdDesignationDetails.DataSource = dv;
                        grdDesignationDetails.DataBind();

                    }
                    else
                    {
                        ViewState["Designationdetails"] = dv.ToTable();
                        ShowEmptyGrid();
                    }
                }

                else
                {
                    LoadDesignationGrid();
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
                dt.Columns.Add("DM_DESGN_ID");
                dt.Columns.Add("DM_NAME");
                dt.Columns.Add("DM_DESC");


                grdDesignationDetails.DataSource = dt;
                grdDesignationDetails.DataBind();

                int iColCount = grdDesignationDetails.Rows[0].Cells.Count;
                grdDesignationDetails.Rows[0].Cells.Clear();
                grdDesignationDetails.Rows[0].Cells.Add(new TableCell());
                grdDesignationDetails.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdDesignationDetails.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }
        protected void imgBtnEdit_Click(object sender, EventArgs e)
        {
            try
            {

                ImageButton imgEdit = (ImageButton)sender;
                GridViewRow rw = (GridViewRow)imgEdit.NamingContainer;
                String DesignationId = ((Label)rw.FindControl("lblId")).Text;
                DesignationId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(DesignationId));
                Response.Redirect("Designation.aspx?StrQryId=" + DesignationId + "", false);
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

                objApproval.sFormName = "Designation";
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
                ClientScript.RegisterStartupScript(this.GetType(), "Msg", sShowMsg);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
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

        protected void Export_clickDest(object sender, EventArgs e)
        {
            //    DataTable dt1 = new DataTable();
            //    clsDesignation objDesgn = new clsDesignation();

            //    dt1 = objDesgn.LoadDetails();

            //    ViewState["Designationdetails"] = dt1;
            //    DataTable dt = (DataTable)ViewState["Designationdetails"];



            DataTable dt = (DataTable)ViewState["Designationdetails"];

            dt.Columns["DM_NAME"].ColumnName = "DESIGNATION ";
            dt.Columns["DM_DESC"].ColumnName = "DESIGNATION DESCRIPTION";

            List<string> listtoRemove = new List<string> { "DM_DESGN_ID" };
            string filename = "DesignationDetails" + DateTime.Now + ".xls";
             string pagetitle="Designation View";

             Genaral.getexcel(dt, listtoRemove, filename, pagetitle);

        }
    }
}