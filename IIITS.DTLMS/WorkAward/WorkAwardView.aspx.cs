using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;

namespace IIITS.DTLMS.WorkAward
{
    public partial class WorkAwardView : System.Web.UI.Page
    {
        string strFormCode = "WorkAward";
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
                    LoadDetails();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
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
                objApproval.sFormName = strFormCode;
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

        public void LoadDetails()
        {
            try
            {
                clsWorkAward obj = new clsWorkAward();
                obj.sOfficeCode = objSession.OfficeCode;
                obj.GetWorkAwardDetails(obj);
                grdWorkAward.DataSource = obj.dtWODetails;
                grdWorkAward.DataBind();
                ViewState["WORKAWARD"] = obj.dtWODetails;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdWorkAward_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdWorkAward.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["WORKAWARD"];
                grdWorkAward.DataSource = SortDataTable(dt as DataTable, true);
                grdWorkAward.DataBind();
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
                        ViewState["WORKAWARD"] = dataView.ToTable();
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["WORKAWARD"] = dataView.ToTable();
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

        protected void grdWorkAward_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtRepairer = (TextBox)row.FindControl("txtRepairer");
                    TextBox txtwoano = (TextBox)row.FindControl("txtwoano");

                    DataTable dt = (DataTable)ViewState["WORKAWARD"];
                    dv = dt.DefaultView;

                    if (txtRepairer.Text != "")
                    {
                        sFilter = "WOA_TR_ID Like '%" + txtRepairer.Text.Replace("'", "'") + "%' AND";
                    }

                    if (txtwoano.Text != "")
                    {
                        sFilter = "WOA_NO  Like '%" + txtwoano.Text.Replace("'", "'") + "%' AND";
                    }
                               

                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdWorkAward.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            dt = dv.ToTable();
                            grdWorkAward.DataSource = dt;
                            grdWorkAward.DataBind();
                        }
                    }
                    else
                    {
                        LoadDetails();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdWorkAward_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                GridViewSortExpression = e.SortExpression;
                int pageIndex = grdWorkAward.PageIndex;
                DataTable dt = (DataTable)ViewState["WORKAWARD"];
                string sortingDirection = string.Empty;
                if (dt.Rows.Count > 0)
                {

                    grdWorkAward.DataSource = SortDataTable(dt as DataTable, false);
                }
                else
                {
                    grdWorkAward.DataSource = dt;
                }
                grdWorkAward.DataBind();
                grdWorkAward.PageIndex = pageIndex;
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
                String sWAId = ((Label)rw.FindControl("lblId")).Text;
                sWAId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWAId));
                Response.Redirect("WorkAward.aspx?WAId=" + sWAId + "", false);
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
                Response.Redirect("WorkAward.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}