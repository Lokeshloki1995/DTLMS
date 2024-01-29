using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;

namespace IIITS.DTLMS.Maintainance
{
    public partial class TcMaintainanceView : System.Web.UI.Page
    {
        string strFormCode = "TcMaintainanceView";
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
                    lblMessage.Text = string.Empty;
                    objSession = (clsSession)Session["clsSession"];
                    if (!IsPostBack)
                    {
                        LoadTcMaintainanceDetails();
                    }
                }
                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
           
        }


        public void LoadTcMaintainanceDetails()
        {
            string strId = string.Empty;
            try
            {
                clsTcMaintainance objDetails = new clsTcMaintainance();
                DataTable dt = objDetails.LoadDTCMaintainance(objSession.OfficeCode);

                grdTcMaintainance.DataSource = dt;
                grdTcMaintainance.DataBind();
                ViewState["Maintainance"] = dt;
                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void LoadHalfYearTcMaintainanceDetails()
        {
            string strId = string.Empty;
            try
            {
                clsTcMaintainance objDetails = new clsTcMaintainance();
                DataTable dt = objDetails.LoadHalfYearDTCMaintainance(objSession.OfficeCode);

                grdTcMaintainance.DataSource = dt;
                grdTcMaintainance.DataBind();
                ViewState["Maintainance"] = dt;

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
                //Check AccessRights
                bool bAccResult = CheckAccessRights("3");
                if (bAccResult == false)
                {
                    return;
                }

                ImageButton imgEdit = (ImageButton)sender;
                GridViewRow rw = (GridViewRow)imgEdit.NamingContainer;

                String strTcMasterViewId = ((Label)rw.FindControl("lblMaintainId")).Text;
                strTcMasterViewId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(strTcMasterViewId));
                //Response.Redirect("DTCMaintance.aspx?MaintId=" + strTcMasterViewId + "", false);
                Response.Redirect("DTCMaintance.aspx?MaintId=" + strTcMasterViewId + "&ID=1", false);
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
                Response.Redirect("DTCMaintance.aspx", false);

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



        protected void grdTcMaintainance_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdTcMaintainance.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Maintainance"];
                grdTcMaintainance.DataSource = SortDataTable(dt as DataTable, true);
                grdTcMaintainance.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdTcMaintainance_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdTcMaintainance.PageIndex;
            DataTable dt = (DataTable)ViewState["Maintainance"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdTcMaintainance.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdTcMaintainance.DataSource = dt;
            }
            grdTcMaintainance.DataBind();
            grdTcMaintainance.PageIndex = pageIndex;
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
                        ViewState["Maintainance"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["Maintainance"] = dataView.ToTable();

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

       

        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "TcMaintainance";
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

        protected void grdTcMaintainance_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
              
                if (e.CommandName == "search")
                {
                    DataView dv = new DataView();
                    string sFilter = string.Empty;
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtcCode = (TextBox)row.FindControl("txtDTCCode");
                    TextBox txtDtcname = (TextBox)row.FindControl("txtDTCName");
                    TextBox txtTcCode = (TextBox)row.FindControl("txtTCCode");
                    DataTable dt = (DataTable)ViewState["Maintainance"];
                    dv = dt.DefaultView;

                    if (txtDtcCode.Text != "")
                    {
                        sFilter = "TM_DT_CODE Like '%" + txtDtcCode.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtTcCode.Text != "")
                    {
                        sFilter += " TM_TC_CODE Like '%" + txtTcCode.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtDtcname.Text != "")
                    {
                        sFilter += " DTCNAME Like '%" + txtDtcname.Text.Replace("'", "`") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdTcMaintainance.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdTcMaintainance.DataSource = dv;
                            ViewState["Maintainance"] = dv.ToTable();
                            grdTcMaintainance.DataBind();
                        }
                        else
                        {
                            ViewState["Maintainance"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        if (rdbQuarterly.Checked == true)
                        {
                            LoadTcMaintainanceDetails();
                        }
                        else if (rdbHalfYearly.Checked == true)
                        {
                            LoadHalfYearTcMaintainanceDetails();
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
                dt.Columns.Add("TM_ID");
                dt.Columns.Add("DTCNAME");
                dt.Columns.Add("TM_MAINTAIN_TYPE");
                dt.Columns.Add("TM_TC_CODE");
                dt.Columns.Add("TM_DT_CODE");
                dt.Columns.Add("TM_DATE");
                dt.Columns.Add("TM_MAINTAIN_BY");
                grdTcMaintainance.DataSource = dt;
                grdTcMaintainance.DataBind();

                int iColCount = grdTcMaintainance.Rows[0].Cells.Count;
                grdTcMaintainance.Rows[0].Cells.Clear();
                grdTcMaintainance.Rows[0].Cells.Add(new TableCell());
                grdTcMaintainance.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdTcMaintainance.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }

        protected void rdbQuarterly_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LoadTcMaintainanceDetails();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void rdbHalfYearly_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LoadHalfYearTcMaintainanceDetails();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void Export_clickTCMaintainance(object sender, EventArgs e)
        {

            //try
            //{
            //    DataTable dt = new DataTable();
            //    if (rdbHalfYearly.Checked)
            //    {
            //        clsTcMaintainance objDetails = new clsTcMaintainance();
            //         dt = objDetails.LoadHalfYearDTCMaintainance(objSession.OfficeCode);
            //    }
            //    else
            //    {
            //        clsTcMaintainance objDetails = new clsTcMaintainance();
            //         dt = objDetails.LoadDTCMaintainance(objSession.OfficeCode);

            //    }

                 DataTable dt = (DataTable)ViewState["Maintainance"];
                if (dt.Rows.Count > 0)
                {

                    dt.Columns["DTCNAME"].ColumnName = "Transformer Centre Name";
                    dt.Columns["TM_DT_CODE"].ColumnName = "Transformer Centre CODE";
                    dt.Columns["TM_TC_CODE"].ColumnName = "Transformer CODE";
                    dt.Columns["TM_MAINTAIN_TYPE"].ColumnName = "MAINTENANCE TYPE";
                    dt.Columns["TM_DATE"].ColumnName = "LAST SERVICE DATE";
                    dt.Columns["TM_MAINTAIN_BY"].ColumnName = "MAINTENANCE BY";

                    dt.Columns["Transformer Centre Name"].SetOrdinal(1);

                    List<string> listtoRemove = new List<string> {  "TM_ID" };
                    string filename = "TCMaintainanceDetails" + DateTime.Now + ".xls";
                    string pagetitle = " Maintenance View";

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
            //    //lblMessage.Text = clsException.ErrorMsg();
            //    //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Export_clickDTCMaster");
            //}



        }
    }
}