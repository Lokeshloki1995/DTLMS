using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;

namespace IIITS.DTLMS.StoreTransfer
{
    public partial class RecieveTransView : System.Web.UI.Page
    {
        string strFormCode = "RecieveTransView";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                lblMessage.Text = string.Empty;
                objSession = (clsSession)Session["clsSession"];
                if (!IsPostBack)
                {
                    LoadPendingReceiveTransDetails();
                    CheckAccessRights("4");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadPendingReceiveTransDetails()
        {
            try
            {
                clsReceiveTrans objReceiveTc = new clsReceiveTrans();
                DataTable dt = new DataTable();
                dt = objReceiveTc.LoadReceiveTcGrid(objSession.OfficeCode);
                grdReceiveTrans.DataSource = dt;
                ViewState["ReceveTrans"] = dt;
                grdReceiveTrans.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadCompletedReceiveTransDetails()
        {
            try
            {
                clsReceiveTrans objReceiveTc = new clsReceiveTrans();
                DataTable dt = new DataTable();
                dt = objReceiveTc.LoadComplededReceiveTcGrid(objSession.OfficeCode);
                grdReceiveTrans.DataSource = dt;
                ViewState["ReceveTrans"] = dt;
                grdReceiveTrans.DataBind();
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

                Response.Redirect("RecieveTransCreate.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdReceiveTrans_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Submit")
                {

                    //Check AccessRights
                    bool bAccResult = CheckAccessRights("2");
                    if (bAccResult == false)
                    {
                        return;
                    }

                    GridViewRow row = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
                    int rowindex = row.RowIndex;
                    Label lblInvoiceId = (Label)grdReceiveTrans.Rows[rowindex].FindControl("lblInvoiceId");
                    string strInvoiceId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblInvoiceId.Text));
                    string strRefType = string.Empty;
                    if (rdbPendingToReceive.Checked==true)
                    {
                        strRefType = "Edit";
                    }
                    if (rdbAlreadyReceived.Checked==true)
                    {
                        strRefType = "View";
                    }
                    strRefType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(strRefType));
                    Response.Redirect("RecieveTransCreate.aspx?QryInvoiceId=" + strInvoiceId + "&RefType=" + strRefType, false);
                    
                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtInvoiceNo = (TextBox)row.FindControl("txtInvoiceNo");
                    TextBox txtIndentNo = (TextBox)row.FindControl("txtIndentNo");

                    DataTable dt = (DataTable)ViewState["ReceveTrans"];
                    dv = dt.DefaultView;
                    if (txtInvoiceNo.Text != "")
                    {
                        sFilter = "IS_NO Like '%" + txtInvoiceNo.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtIndentNo.Text != "")
                    {
                        sFilter = "SI_NO Like '%" + txtIndentNo.Text.Replace("'", "`") + "%' AND";
                    }

                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdReceiveTrans.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdReceiveTrans.DataSource = dv;
                            ViewState["ReceveTrans"] = dv.ToTable();
                            grdReceiveTrans.DataBind();

                        }
                        else
                        {
                            ViewState["ReceveTrans"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        if (rdbPendingToReceive.Checked == true)
                        {
                            LoadPendingReceiveTransDetails();
                        }
                        if (rdbAlreadyReceived.Checked == true)
                        {
                            LoadCompletedReceiveTransDetails();
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

        protected void grdReceiveTrans_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdReceiveTrans.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["ReceveTrans"];
                grdReceiveTrans.DataSource = SortDataTable(dt as DataTable, true);
                grdReceiveTrans.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdReceiveTrans_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdReceiveTrans.PageIndex;
            DataTable dt = (DataTable)ViewState["ReceveTrans"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {

                grdReceiveTrans.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdReceiveTrans.DataSource = dt;
            }
            grdReceiveTrans.DataBind();
            grdReceiveTrans.PageIndex = pageIndex;
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
                        ViewState["ReceveTrans"] = dataView.ToTable();
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["ReceveTrans"] = dataView.ToTable();
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

        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("IS_ID");
                dt.Columns.Add("IS_NO");
                dt.Columns.Add("IS_DATE");
                dt.Columns.Add("SI_FROM_STORE");
                dt.Columns.Add("IO_TCCODE");
                dt.Columns.Add("SI_NO");

                grdReceiveTrans.DataSource = dt;
                grdReceiveTrans.DataBind();

                int iColCount = grdReceiveTrans.Rows[0].Cells.Count;
                grdReceiveTrans.Rows[0].Cells.Clear();
                grdReceiveTrans.Rows[0].Cells.Add(new TableCell());
                grdReceiveTrans.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdReceiveTrans.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
   
        protected void rdbPendingToReceive_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LoadPendingReceiveTransDetails();
                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void rdbAlreadyReceived_CheckedChanged1(object sender, EventArgs e)
        {
            try
            {
                LoadCompletedReceiveTransDetails();
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

                objApproval.sFormName = "RecieveTransCreate";
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

        protected void Export_clickrecivetrans(object sender, EventArgs e)
        {

            //try
            //{
            //    DataTable dt = new DataTable();
            //    if (rdbAlreadyReceived.Checked)
            //    {
            //        clsReceiveTrans objReceiveTc = new clsReceiveTrans();
                    
            //        dt = objReceiveTc.LoadComplededReceiveTcGrid(objSession.OfficeCode);
            //    }
            //    else
            //    {
            //        clsReceiveTrans objReceiveTc = new clsReceiveTrans();
             
            //        dt = objReceiveTc.LoadReceiveTcGrid(objSession.OfficeCode);

            //    }
            DataTable dt = (DataTable)ViewState["ReceveTrans"];
                if (dt.Rows.Count > 0)
                {

                    dt.Columns["SI_NO"].ColumnName = "INDENT NUMBER";
                    dt.Columns["IS_DATE"].ColumnName = "INVOICE DATE";
                    dt.Columns["IS_NO"].ColumnName = "INVOICE NUMBER";
                    dt.Columns["SI_FROM_STORE"].ColumnName = "FROM STORE";
                    dt.Columns["IO_TCCODE"].ColumnName = "NO OF TRANSFORMERS";

                    dt.Columns["INDENT NUMBER"].SetOrdinal(1);
                    List<string> listtoRemove = new List<string> { "IS_ID", "IO_IS_ID" };
                    string filename = "ReceiveTransformerDetails" + DateTime.Now + ".xls";
                    string pagetitle = "Receive Transformer Details";

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
                
            //}



        }

    }
}