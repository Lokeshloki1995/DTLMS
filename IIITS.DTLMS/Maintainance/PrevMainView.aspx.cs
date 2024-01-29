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
    public partial class PrevMainView : System.Web.UI.Page
    {
        string strFormCode = "PrevMainView";
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
                        LoadQuarterlyTcMaintainanceDetails();

                    }
                }
                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }


        public void LoadQuarterlyTcMaintainanceDetails()
        {
            string strId = string.Empty;
            try
            {
                clsTcMaintainance objDetails = new clsTcMaintainance();
                DataTable dt = objDetails.LoadQuarPendingDTCMaintainance(objSession.OfficeCode);
                grdPendingTcMaintainance.DataSource = dt;
                grdPendingTcMaintainance.DataBind();
                ViewState["PendingMaintainance"] = dt;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }



        public void LoadHalfYearlyTcMaintainanceDetails()
        {
            string strId = string.Empty;
            try
            {
                clsTcMaintainance objDetails = new clsTcMaintainance();
                DataTable dt = objDetails.LoadHalfYearlyPendingDTCMaintainance(objSession.OfficeCode);

                grdPendingTcMaintainance.DataSource = dt;
                grdPendingTcMaintainance.DataBind();
                ViewState["PendingMaintainance"] = dt;

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

                String strTcMasterViewId = ((Label)rw.FindControl("lblTmDtCode")).Text;
                String strMainType = ((Label)rw.FindControl("lblMaintainType")).Text;
                strMainType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(strMainType));
                strTcMasterViewId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(strTcMasterViewId));
                //Response.Redirect("DTCMaintance.aspx?PendingMaintId=" + strTcMasterViewId + "&MaintType=" + strMainType, false);
                Response.Redirect("DTCMaintance.aspx?PendingMaintId=" + strTcMasterViewId + "&MaintType=" + strMainType + "&ID=0", false);

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
                grdPendingTcMaintainance.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["PendingMaintainance"];
                grdPendingTcMaintainance.DataSource = SortDataTable(dt as DataTable, true);
                grdPendingTcMaintainance.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdPendingTcMaintainance_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdPendingTcMaintainance.PageIndex;
            DataTable dt = (DataTable)ViewState["PendingMaintainance"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdPendingTcMaintainance.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdPendingTcMaintainance.DataSource = dt;
            }
            grdPendingTcMaintainance.DataBind();
            grdPendingTcMaintainance.PageIndex = pageIndex;
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
                        ViewState["PendingMaintainance"] = dataView.ToTable();
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["PendingMaintainance"] = dataView.ToTable();
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
                  
                    DataTable dt = (DataTable)ViewState["PendingMaintainance"];
                    dv = dt.DefaultView;

                    if (txtDtcCode.Text != "")
                    {
                        sFilter = "DT_CODE Like '%" + txtDtcCode.Text.Replace("'", "`") + "%' AND";
                    }
                   
                    if (txtDtcname.Text != "")
                    {
                        sFilter += " DT_NAME Like '%" + txtDtcname.Text.Replace("'", "`") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdPendingTcMaintainance.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdPendingTcMaintainance.DataSource = dv;
                            ViewState["PendingMaintainance"] = dv.ToTable();
                            grdPendingTcMaintainance.DataBind();
                        }
                        else
                        {
                            ViewState["PendingMaintainance"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        if (rdbQuarterly.Checked == true)
                        {
                            LoadQuarterlyTcMaintainanceDetails();
                        }
                        else if (rdbHalfYearly.Checked == true)
                        {
                            LoadHalfYearlyTcMaintainanceDetails();
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

                dt.Columns.Add("DT_NAME");
                dt.Columns.Add("DT_CODE");
                dt.Columns.Add("DT_TC_ID");
                dt.Columns.Add("TM_MAINTAIN_TYPE");
                dt.Columns.Add("DT_LAST_SERVICE_DATE");
                
                grdPendingTcMaintainance.DataSource = dt;
                grdPendingTcMaintainance.DataBind();

                int iColCount = grdPendingTcMaintainance.Rows[0].Cells.Count;
                grdPendingTcMaintainance.Rows[0].Cells.Clear();
                grdPendingTcMaintainance.Rows[0].Cells.Add(new TableCell());
                grdPendingTcMaintainance.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdPendingTcMaintainance.Rows[0].Cells[0].Text = "No Records Found";

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
                LoadQuarterlyTcMaintainanceDetails();
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
                LoadHalfYearlyTcMaintainanceDetails();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void Export_clickMaintainance(object sender, EventArgs e)
        {

           
                //DataTable dt = new DataTable();
                //if (rdbHalfYearly.Checked)
                //{
                //    clsTcMaintainance objDetails = new clsTcMaintainance();
                //     dt = objDetails.LoadHalfYearlyPendingDTCMaintainance(objSession.OfficeCode);
                //}
                //else
                //{
                //    clsTcMaintainance objDetails = new clsTcMaintainance();
                //     dt = objDetails.LoadQuarPendingDTCMaintainance(objSession.OfficeCode);

                //}

                DataTable dt = (DataTable)ViewState["PendingMaintainance"];
                if (dt.Rows.Count > 0)
                {

                    dt.Columns["DT_NAME"].ColumnName = "Transformer Centre Name";
                    dt.Columns["DT_CODE"].ColumnName = "Transformer Centre CODE";
                    dt.Columns["DT_TC_ID"].ColumnName = "DTR CODE";
                    dt.Columns["TM_MAINTAIN_TYPE"].ColumnName = "MAINTENANCE TYPE";
                    dt.Columns["DT_LAST_SERVICE_DATE"].ColumnName = "LAST SERVICE DATE";

                    dt.Columns["Transformer Centre Name"].SetOrdinal(1);
                    dt.Columns["Transformer Centre CODE"].SetOrdinal(2);
                    dt.Columns["DTR CODE"].SetOrdinal(3);
                    dt.Columns["MAINTENANCE TYPE"].SetOrdinal(4);
                    dt.Columns["LAST SERVICE DATE"].SetOrdinal(5);


                    List<string> listtoRemove = new List<string> { "DT_OM_SLNO"};
                    string filename = "MaintainanceDetails" + DateTime.Now + ".xls";
                    string pagetitle = "Preventive Maintenance View";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }
            
            



        }


    }
}