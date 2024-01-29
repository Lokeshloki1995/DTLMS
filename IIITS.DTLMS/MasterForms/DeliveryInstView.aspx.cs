using IIITS.DTLMS.BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.MasterForms
{
    public partial class DeliveryInstView : System.Web.UI.Page
    {
        string strFormCode = "DeliveryInstView";
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
                    LoadDeliveryInstDetails();

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        // to load DI details to grid
        public void LoadDeliveryInstDetails(string sDINumber = "")
        {
            try
            {
                clsDelivery objDeliveryInst = new clsDelivery();
                DataTable dt = new DataTable();
                objDeliveryInst.PONumber = sDINumber;

                dt = objDeliveryInst.GetDeliveredDetails(sDINumber);

                if (dt.Rows.Count <= 0)
                {

                    DataRow newRow = dt.NewRow();
                    dt.Rows.Add(newRow);
                    dt.Columns.Add("DI_ID");
                    dt.Columns.Add("DI_PO_ID");
                    dt.Columns.Add("DIM_DI_NO");
                    dt.Columns.Add("DI_DATE");
                    dt.Columns.Add("DI_STORE");
                    dt.Columns.Add("DI_MAKE");
                    dt.Columns.Add("DI_CAPACITY");
                    dt.Columns.Add("DI_STARRATE");
                    dt.Columns.Add("DI_STARRATENAME");
                    dt.Columns.Add("DI_QUANTITY");
                    dt.Columns.Add("DI_DUEDATE");
                    dt.Columns.Add("DI_START_RANGE");
                    dt.Columns.Add("DI_END_RANGE");

                    grdDeliveryInstView.DataSource = dt;
                    grdDeliveryInstView.DataBind();

                    int iColCount = grdDeliveryInstView.Rows[0].Cells.Count;
                    grdDeliveryInstView.Rows[0].Cells.Clear();
                    grdDeliveryInstView.Rows[0].Cells.Add(new TableCell());
                    grdDeliveryInstView.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdDeliveryInstView.Rows[0].Cells[0].Text = "No Records Found";
                    ViewState["DI"] = dt;

                }
                else
                {
                    grdDeliveryInstView.DataSource = dt;
                    grdDeliveryInstView.DataBind();
                    ViewState["DI"] = dt;

                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdPoMasterView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdDeliveryInstView.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["DI"];
                grdDeliveryInstView.DataSource = SortDataTable(dt as DataTable, true);
                grdDeliveryInstView.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdPOmaster_Sorting(object sender, GridViewSortEventArgs e)
        {

            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdDeliveryInstView.PageIndex;
            DataTable dt = (DataTable)ViewState["DI"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdDeliveryInstView.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdDeliveryInstView.DataSource = dt;
            }
            grdDeliveryInstView.DataBind();
            grdDeliveryInstView.PageIndex = pageIndex;
        }
        protected void grdPoMasterView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Submit")
                {

                    //Check AccessRights
                    bool bAccResult = CheckAccessRights("3");
                    if (bAccResult == false)
                    {
                        return;
                    }

                    GridViewRow row = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
                    int rowindex = row.RowIndex;
                    Label lblDIid = (Label)grdDeliveryInstView.Rows[rowindex].FindControl("lblDIid");
                    Label lblPoId = (Label)grdDeliveryInstView.Rows[rowindex].FindControl("lblPoId");
                    Label lblDiNo = (Label)grdDeliveryInstView.Rows[rowindex].FindControl("lblDiNo");
                    string strDIid = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblDIid.Text));
                    string strPoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblPoId.Text));
                    string strDiNum = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblDiNo.Text));
                    Response.Redirect("DeliveryInst.aspx?QryDIid=" + strDIid + "&QryPoId=" + strPoId + "&QryDiNo=" + strDiNum, false);
                }
                LoadDeliveryInstDetails();
                if (e.CommandName == "search")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDINumber = (TextBox)row.FindControl("txtDINumber");
                    LoadDeliveryInstDetails(txtDINumber.Text);
                }
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
                        ViewState["DI"] = dataView.ToTable();

                    }
                    else
                    {

                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["DI"] = dataView.ToTable();

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
                Response.Redirect("DeliveryInst.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void Export_clickPOMaster(object sender, EventArgs e)
        {

            //clsPoMaster objPoMaster = new clsPoMaster();
            //DataTable dt = new DataTable();
            //string sPoNumber = "";
            //objPoMaster.sPoNo = sPoNumber;

            //dt = objPoMaster.LoadPoDetailGrid(objPoMaster);
            DataTable dt = (DataTable)ViewState["DI"];

            if (dt.Rows.Count > 0)
            {
                //DataTable dtPoDetails = new DataTable();
                //DataRow newRow = dtPoDetails.NewRow();
                //dtPoDetails.Rows.Add(newRow);
                //dtPoDetails.Columns.Add("PO_ID");
                //dtPoDetails.Columns.Add("PO_NO");
                //dtPoDetails.Columns.Add("PO_DATE");
                //dtPoDetails.Columns.Add("PB_QUANTITY");
                //dtPoDetails.Columns.Add("PO_SUPPLIER_ID");

                //grdPoMasterView.DataSource = dtPoDetails;
                //grdPoMasterView.DataBind();

                //int iColCount = grdPoMasterView.Rows[0].Cells.Count;
                //grdPoMasterView.Rows[0].Cells.Clear();
                //grdPoMasterView.Rows[0].Cells.Add(new TableCell());
                //grdPoMasterView.Rows[0].Cells[0].ColumnSpan = iColCount;
                //grdPoMasterView.Rows[0].Cells[0].Text = "No Records Found";
                //ShowMsgBox("No record found");

                dt.Columns["DIM_DI_NO"].ColumnName = "DISPATCH No";
                dt.Columns["DI_DATE"].ColumnName = "DI DATE";
                dt.Columns["DI_STORE"].ColumnName = "STORE NAME";

                dt.Columns["DI_MAKE"].ColumnName = "MAKE NAME";
                dt.Columns["DI_CAPACITY"].ColumnName = "CAPACITY";
                dt.Columns["DI_STARRATENAME"].ColumnName = "STAR RATE";
                dt.Columns["DI_QUANTITY"].ColumnName = "QUANTITY";

                List<string> listtoRemove = new List<string> { "DI_ID" };
                string filename = "DIDetails" + DateTime.Now + ".xls";
                string pagetitle = "DeliveryInstruction Details";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);

            }
            else
            {
                ShowMsgBox("No record found");

                DataTable dtPoDetails = new DataTable();
                DataRow newRow = dtPoDetails.NewRow();
                dtPoDetails.Rows.Add(newRow);
                dtPoDetails.Columns.Add("DI_ID");
                dtPoDetails.Columns.Add("DIM_DI_NO");
                dtPoDetails.Columns.Add("DI_DATE");
                dtPoDetails.Columns.Add("DI_STORE");
                dtPoDetails.Columns.Add("DI_MAKE");
                dtPoDetails.Columns.Add("DI_CAPACITY");
                dtPoDetails.Columns.Add("DI_STARRATE");
                dtPoDetails.Columns.Add("DI_QUANTITY");

                grdDeliveryInstView.DataSource = dtPoDetails;
                grdDeliveryInstView.DataBind();

                int iColCount = grdDeliveryInstView.Rows[0].Cells.Count;
                grdDeliveryInstView.Rows[0].Cells.Clear();
                grdDeliveryInstView.Rows[0].Cells.Add(new TableCell());
                grdDeliveryInstView.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdDeliveryInstView.Rows[0].Cells[0].Text = "No Records Found";

            }
            //else
            //{
            //    ShowMsgBox("No record found");
            //}



        }
        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "PoMaster";
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

    }
}