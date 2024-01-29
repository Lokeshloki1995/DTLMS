using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using IIITS.DTLMS.BL.Billing;
using System.Data;

namespace IIITS.DTLMS.Billing
{

   
    public partial class MajorBillingView : System.Web.UI.Page
    {
        string strFormCode = "MajorBillingView";
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
                        CheckAccessRights("1");
                        LoadDetails();
                    }
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

        protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton imgEdit = (ImageButton)sender;
                GridViewRow rw = (GridViewRow)imgEdit.NamingContainer;
                String sEstId = ((Label)rw.FindControl("lblEstNo")).Text;
                String sInvId = ((Label)rw.FindControl("lblId")).Text;
                sEstId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sEstId));
                sInvId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sInvId));
                Response.Redirect("MajorDTRBilling.aspx?EstId=" + sEstId + "&InvId=" + sInvId + "", false);
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

        public void LoadDetails()
        {
            try
            {
                clsMajorBilling obj = new clsMajorBilling();
                obj.sOfficeCode = objSession.OfficeCode;
                obj.sRoleType = objSession.sRoleType;
                obj.GetBillingDetails(obj);
                grdBilling.DataSource = obj.dtInvoiceDetails;
                grdBilling.DataBind();
                ViewState["Billing"] = obj.dtInvoiceDetails;
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
                Response.Redirect("MajorDTRBilling.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdBilling_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdBilling.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Billing"];
                grdBilling.DataSource = SortDataTable(dt as DataTable, true);
                grdBilling.DataBind();
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
                        ViewState["Billing"] = dataView.ToTable();
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["Billing"] = dataView.ToTable();
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

        protected void grdBilling_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                GridViewSortExpression = e.SortExpression;
                int pageIndex = grdBilling.PageIndex;
                DataTable dt = (DataTable)ViewState["Billing"];
                string sortingDirection = string.Empty;
                if (dt.Rows.Count > 0)
                {

                    grdBilling.DataSource = SortDataTable(dt as DataTable, false);
                }
                else
                {
                    grdBilling.DataSource = dt;
                }
                grdBilling.DataBind();
                grdBilling.PageIndex = pageIndex;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdBilling_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtWo = (TextBox)row.FindControl("txtWo");
                    TextBox txtEstNo = (TextBox)row.FindControl("txtEstNo");
                    TextBox txtInvNo = (TextBox)row.FindControl("txtInvNo");

                    DataTable dt = (DataTable)ViewState["Billing"];
                    dv = dt.DefaultView;

                    if (txtWo.Text != "")
                    {
                        sFilter = "RWO_NO Like '%" + txtWo.Text.Replace("'", "'") + "%' AND";
                    }

                    if (txtEstNo.Text != "")
                    {
                        sFilter = "RESTD_NO  Like '%" + txtEstNo.Text.Replace("'", "'") + "%' AND";
                    }

                    if (txtInvNo.Text != "")
                    {
                        sFilter = "MJB_INV_NO  Like '%" + txtInvNo.Text.Replace("'", "'") + "%' AND";
                    }

                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdBilling.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            dt = dv.ToTable();
                            grdBilling.DataSource = dt;
                            grdBilling.DataBind();
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
    }
}