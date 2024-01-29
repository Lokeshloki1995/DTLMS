using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;

namespace IIITS.DTLMS.DTCFailure
{
    public partial class EstimationView : System.Web.UI.Page
    {
        string strFormCode = "EstimationView";
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
                        LoadEstimationDetails();
                        //CheckAccessRights("4");
                    }
                }               
            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        private void LoadEstimationDetails(string sFailuretype = "")
        {
            try
            {
                clsEstimate obj = new clsEstimate();
                DataTable dt = new DataTable();
                dt = obj.LoadEstimationDetails(sFailuretype,objSession.OfficeCode);
                if(dt.Rows.Count > 0)
                {
                    grdEstimation.DataSource = dt;
                    grdEstimation.DataBind();
                    ViewState["Estimation"] = dt;
                }              
            }
            catch(Exception ex)
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

        protected void grdEstimation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if(e.CommandName == "View")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    Label lblFailId = (Label)row.FindControl("lblFailureId");
                    Label lblEstId = (Label)row.FindControl("lblEstId");

                    string sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblFailId.Text));
                    string sESTId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblEstId.Text));
                    //sStoreInvoiceId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sStoreInvoiceId));
                    string sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt("V"));
                    Session["WFDataId"] = "0";

                    Response.Redirect("EstimationCreation.aspx?EstID="+ sESTId+ "&FailId=" + sRecordId + "&ActionType=" + sActionType, false);
                }
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtEstNo = (TextBox)row.FindControl("txtEstNo");
                    TextBox txtCapacity = (TextBox)row.FindControl("txtCapacity");
                    TextBox txtDtCode = (TextBox)row.FindControl("txtDtcCode");
                    TextBox txtfailuretype = (TextBox)row.FindControl("txtfailuretype");
                    TextBox txtDtrCode = (TextBox)row.FindControl("txtDtrCode");

                    DataTable dt = (DataTable)ViewState["Estimation"];
                    dv = dt.DefaultView;
                    if (txtEstNo.Text != "")
                    {
                        sFilter = "EST_NO Like '%" + txtEstNo.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtCapacity.Text != "")
                    {
                        sFilter = "EST_CAPACITY Like '%" + txtCapacity.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtDtCode.Text != "")
                    {
                        sFilter = "DF_DTC_CODE Like '%" + txtDtCode.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtfailuretype.Text != "")
                    {
                        sFilter += " FAILURETYPE Like '%" + txtfailuretype.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtDtrCode.Text != "")
                    {
                        sFilter += " DF_EQUIPMENT_ID Like '%" + txtDtrCode.Text.Replace("'", "`") + "%' AND";
                    }

                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdEstimation.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdEstimation.DataSource = dv;
                            ViewState["Estimation"] = dv.ToTable();
                            grdEstimation.DataBind();

                        }
                        else
                        {
                            ViewState["Estimation"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadEstimationDetails();
                    }

                }
            }
            catch(Exception ex)
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
                dt.Columns.Add("EST_ID");
                dt.Columns.Add("EST_FAILUREID");
                dt.Columns.Add("DF_DTC_CODE");
                dt.Columns.Add("DF_EQUIPMENT_ID");
                dt.Columns.Add("EST_NO");
                dt.Columns.Add("EST_CRON");
                dt.Columns.Add("WOUND_TYPE");
                dt.Columns.Add("EST_CAPACITY");
                dt.Columns.Add("FAILURETYPE");


                grdEstimation.DataSource = dt;
                grdEstimation.DataBind();

                int iColCount = grdEstimation.Rows[0].Cells.Count;
                grdEstimation.Rows[0].Cells.Clear();
                grdEstimation.Rows[0].Cells.Add(new TableCell());
                grdEstimation.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdEstimation.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void Export_Clic(object sender, EventArgs e)
        {
         

            DataTable dt = (DataTable)ViewState["Estimation"];

            if (dt.Rows.Count > 0)
            {

                dt.Columns["DF_DTC_CODE"].ColumnName = "Transformer Centre Code";
                dt.Columns["EST_NO"].ColumnName = "Estimation Number";
                dt.Columns["DF_EQUIPMENT_ID"].ColumnName = "DTR Code";
                dt.Columns["EST_CAPACITY"].ColumnName = "Capacity";
                dt.Columns["FAILURETYPE"].ColumnName = "Fail Type";
                dt.Columns["EST_CRON"].ColumnName = "Estimation Date";

                List<string> listtoRemove = new List<string> { "EST_FAILUREID", "EST_ID", "RATE_TYPE", "WOUND_TYPE" };
                string filename = "EstimationView" + DateTime.Now + ".xls";
                string pagetitle = " Estimation View";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");
                ShowEmptyGrid();
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
                        ViewState["Workorder"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["Workorder"] = dataView.ToTable();

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

        protected void grdEstimation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdEstimation.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Estimation"];
                grdEstimation.DataSource = SortDataTable(dt as DataTable, true);
                grdEstimation.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdEstimation_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                GridViewSortExpression = e.SortExpression;
                int pageIndex = grdEstimation.PageIndex;
                DataTable dt = (DataTable)ViewState["Workorder"];
                string sortingDirection = string.Empty;
                if (dt.Rows.Count > 0)
                {
                    grdEstimation.DataSource = SortDataTable(dt as DataTable, false);
                }
                else
                {
                    grdEstimation.DataSource = dt;

                }
                grdEstimation.DataBind();
                grdEstimation.PageIndex = pageIndex;
            }
            catch(Exception ex)
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

                objApproval.sFormName = "EstimationCreation";
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

        //protected void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if(cmbType.SelectedIndex > 0)
        //        {
        //            string sFailuretype = string.Empty;
        //            sFailuretype = Convert.ToString(cmbType.SelectedValue);
        //            LoadEstimationDetails(sFailuretype);
        //        }
        //        else
        //        {
        //            LoadEstimationDetails();
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}
    }
}