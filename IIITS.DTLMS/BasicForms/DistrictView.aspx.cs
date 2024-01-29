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
    public partial class DistrictView : System.Web.UI.Page
    {
        string strFormCode = "DistrictView";
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
                    LoadDistrictDetails();
                }
            }
            
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

                String DistrictID = ((Label)rw.FindControl("lblDistId")).Text;
                DistrictID = HttpUtility.UrlEncode(Genaral.UrlEncrypt(DistrictID));

                Response.Redirect("District.aspx?DistrictId=" + DistrictID + "", false);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadDistrictDetails()
        {
            try
            {
                DataTable dt = new DataTable();
                clsDistrict objDist = new clsDistrict();
                dt = objDist.LoadAllDistDetails();
                ViewState["DistDetails"] = dt;
                grdDistrictdetails.DataSource = dt;
                grdDistrictdetails.DataBind();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdDistrictdetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                grdDistrictdetails.PageIndex = e.NewPageIndex;
                dt = (DataTable)ViewState["DistDetails"];
                grdDistrictdetails.DataSource = SortDataTable(dt as DataTable, true);
                grdDistrictdetails.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdDistrictdetails_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdDistrictdetails.PageIndex;
            DataTable dt = (DataTable)ViewState["DistDetails"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdDistrictdetails.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdDistrictdetails.DataSource = dt;

            }
            grdDistrictdetails.DataBind();
            grdDistrictdetails.PageIndex = pageIndex;
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
                        ViewState["DistDetails"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["DistDetails"] = dataView.ToTable();

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
        protected void cmdNewDistrict_Click(object sender, EventArgs e)
        {
            try
            {
                //Check AccessRights
                bool bAccResult = CheckAccessRights("2");
                if (bAccResult == false)
                {
                    return;
                }
                Response.Redirect("District.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void grdDistrictdetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                LoadDistrictDetails();
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtdistCode = (TextBox)row.FindControl("txtDistrictCode");
                    TextBox txtdistName = (TextBox)row.FindControl("txtDistrictName");



                    DataTable dt = (DataTable)ViewState["DistDetails"];
                    dv = dt.DefaultView;

                    if (txtdistCode.Text != "")
                    {
                        sFilter = "DT_CODE Like '%" + txtdistCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtdistName.Text != "")
                    {
                        sFilter += " DT_NAME Like '%" + txtdistName.Text.Replace("'", "'") + "%' AND";
                    }

                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdDistrictdetails.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdDistrictdetails.DataSource = dv;
                            ViewState["DistDetails"] = dv.ToTable();
                            grdDistrictdetails.DataBind();

                        }
                        else
                        {
                            ViewState["DistDetails"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadDistrictDetails();
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
                dt.Columns.Add("DT_ID");
                dt.Columns.Add("DT_CODE");
                dt.Columns.Add("DT_NAME");


                grdDistrictdetails.DataSource = dt;
                grdDistrictdetails.DataBind();

                int iColCount = grdDistrictdetails.Rows[0].Cells.Count;
                grdDistrictdetails.Rows[0].Cells.Clear();
                grdDistrictdetails.Rows[0].Cells.Add(new TableCell());
                grdDistrictdetails.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdDistrictdetails.Rows[0].Cells[0].Text = "No Records Found";

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

                objApproval.sFormName = "District";
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

        protected void Export_clickDistrict(object sender, EventArgs e)
        {
            //DataTable dt = new DataTable();
            //clsDistrict objDist = new clsDistrict();
            //dt = objDist.LoadAllDistDetails();

            DataTable dt = (DataTable)ViewState["DistDetails"];

            if (dt.Rows.Count > 0)
            {

                dt.Columns["DT_CODE"].ColumnName = "District Code";
                dt.Columns["DT_NAME"].ColumnName = "District Name";
                //dt.Columns[""].ColumnName = "Mobile No";
                //dt.Columns[""].ColumnName = "Division Head";
                
                List<string> listtoRemove = new List<string> { "DT_ID" };
                string filename = "DistrictDetails" + DateTime.Now + ".xls";
                string pageTitle = "District Details";

                Genaral.getexcel(dt, listtoRemove, filename, pageTitle);
            }
            else
            {
                ShowMsgBox("No record found");
                ShowEmptyGrid();
            }

        }
    }
}