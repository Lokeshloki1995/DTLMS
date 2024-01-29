using IIITS.DTLMS.BL;
using IIITS.DTLMS.BL.POFlow;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.POFlow
{
    public partial class PMCPoMaterView : System.Web.UI.Page
    {
        string strFormCode = "PoMasterView";
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
                    LoadPoMasterDetails();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to load po details
        /// </summary>
        /// <param name="dwaNumber"></param>
        public void LoadPoMasterDetails(string dwaNumber = "", string poNumber = "")
        {
            try
            {
                ClsPMCPoMaster objPoMaster = new ClsPMCPoMaster();
                DataTable dt = new DataTable();
                objPoMaster.DWANO = dwaNumber;
                objPoMaster.sPoNo = poNumber;

                dt = objPoMaster.LoadPMCPoDetailGrid(objPoMaster);
                if (dt.Rows.Count <= 0)
                {
                    DataTable dtPoDetails = new DataTable();
                    DataRow newRow = dtPoDetails.NewRow();
                    dtPoDetails.Rows.Add(newRow);

                    dtPoDetails.Columns.Add("PMC_PO_ID");
                    dtPoDetails.Columns.Add("DM_ID");
                    dtPoDetails.Columns.Add("DM_NUMBER");
                    dtPoDetails.Columns.Add("DM_DATE");
                    dtPoDetails.Columns.Add("DM_EXTENDED_UPTO");
                    dtPoDetails.Columns.Add("DM_AMOUNT");
                    dtPoDetails.Columns.Add("PMC_PO_NO");
                    dtPoDetails.Columns.Add("PMC_PO_DATE");
                    dtPoDetails.Columns.Add("PMC_PO_AMOUNT");
                    dtPoDetails.Columns.Add("TS_NAME");
                    dtPoDetails.Columns.Add("PMC_PB_QUANTITY");

                    grdPoMasterView.DataSource = dtPoDetails;
                    grdPoMasterView.DataBind();

                    int iColCount = grdPoMasterView.Rows[0].Cells.Count;
                    grdPoMasterView.Rows[0].Cells.Clear();
                    grdPoMasterView.Rows[0].Cells.Add(new TableCell());
                    grdPoMasterView.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdPoMasterView.Rows[0].Cells[0].Text = "No Records Found";
                }
                else
                {
                    grdPoMasterView.DataSource = dt;
                    grdPoMasterView.DataBind();
                }
                ViewState["PO"] = dt;
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
                grdPoMasterView.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["PO"];
                grdPoMasterView.DataSource = SortDataTable(dt as DataTable, true);
                grdPoMasterView.DataBind();
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
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                    }
                    ViewState["PO"] = dataView.ToTable();
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

        protected void grdPOmaster_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdPoMasterView.PageIndex;
            DataTable dt = (DataTable)ViewState["PO"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdPoMasterView.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdPoMasterView.DataSource = dt;
            }
            grdPoMasterView.DataBind();
            grdPoMasterView.PageIndex = pageIndex;
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
                    Label lblPoId = (Label)grdPoMasterView.Rows[rowindex].FindControl("lblPoId");
                    Label lblPoQunatity = (Label)grdPoMasterView.Rows[rowindex].FindControl("lblPoQuantity");
                    Label lbldmId = (Label)grdPoMasterView.Rows[rowindex].FindControl("lbldmId");
                    
                    string strPoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblPoId.Text));
                    string strPoQnty = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblPoQunatity.Text));
                    string strDmId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lbldmId.Text));
                    Response.Redirect("PMCPoMasterCreate.aspx?QryPoId=" + strPoId + "&QryPoQnty=" + strPoQnty + "&QryDMId=" + strDmId, false);
                }
                if (e.CommandName == "search")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtsDWANumber = (TextBox)row.FindControl("txtDWANumber");
                    TextBox txtpoNumber = (TextBox)row.FindControl("txtPONumberSerch");
                    LoadPoMasterDetails(txtsDWANumber.Text, txtpoNumber.Text);
                }
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
                bool bAccResult = CheckAccessRights(Constants.CheckAccessRights.CheckAccessRightsCreate);
                if (bAccResult == false)
                {
                    return;
                }
                Response.Redirect("PMCPoMasterCreate.aspx", false);
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

        protected void Export_clickPOMaster(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["PO"];

            if (dt.Rows.Count > 0)
            {

                #region adding Slno
                int Slno = 0;
                dt.Columns.Add("SlNo", typeof(int));
                // Iterate through the rows in the DataTable
                foreach (DataRow row in dt.Rows)
                {
                    #region adding Slno.
                    Slno = Slno + 1;
                    row["SlNo"] = Slno;
                    #endregion
                }
                #endregion

                dt.Columns["DM_NUMBER"].ColumnName = "DWA No";
                dt.Columns["DM_DATE"].ColumnName = "DWA Date";
                dt.Columns["DM_EXTENDED_UPTO"].ColumnName = "DWA Expiry Date";
                dt.Columns["DM_AMOUNT"].ColumnName = "DWA Amount";
                dt.Columns["PMC_PO_NO"].ColumnName = "PO No";
                dt.Columns["PMC_PO_DATE"].ColumnName = "PO Date";
                dt.Columns["PMC_PO_AMOUNT"].ColumnName = "PO Amount";
                dt.Columns["TS_NAME"].ColumnName = "Supplier Name";
                dt.Columns["PMC_PB_QUANTITY"].ColumnName = "PO Quantity";

                dt.Columns["SlNo"].SetOrdinal(0);

                List<string> listtoRemove = new List<string> { "PMC_PO_ID","DM_ID" };
                string filename = "PMCPODetails" + DateTime.Now + ".xls";
                string pagetitle = "Purchase Order Details";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);                
            }
            else
            {
                ShowMsgBox("No record found");

                DataTable dtPoDetails = new DataTable();
                DataRow newRow = dtPoDetails.NewRow();
                dtPoDetails.Rows.Add(newRow);

                dtPoDetails.Columns.Add("PMC_PO_ID");
                dtPoDetails.Columns.Add("DM_ID");
                dtPoDetails.Columns.Add("DM_NUMBER");
                dtPoDetails.Columns.Add("DM_DATE");
                dtPoDetails.Columns.Add("DM_EXTENDED_UPTO");
                dtPoDetails.Columns.Add("DM_AMOUNT");
                dtPoDetails.Columns.Add("PMC_PO_NO");
                dtPoDetails.Columns.Add("PMC_PO_DATE");
                dtPoDetails.Columns.Add("PMC_PO_AMOUNT");
                dtPoDetails.Columns.Add("TS_NAME");
                dtPoDetails.Columns.Add("PMC_PB_QUANTITY");
                
                grdPoMasterView.DataSource = dtPoDetails;
                grdPoMasterView.DataBind();

                int iColCount = grdPoMasterView.Rows[0].Cells.Count;
                grdPoMasterView.Rows[0].Cells.Clear();
                grdPoMasterView.Rows[0].Cells.Add(new TableCell());
                grdPoMasterView.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdPoMasterView.Rows[0].Cells[0].Text = "No Records Found";

            }
        }
        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY
                clsApproval objApproval = new clsApproval();
                objApproval.sFormName = "PMCPOMaster"; 
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sAccessType = "1" + "," + sAccessType;
                bool bResult = objApproval.CheckAccessRights(objApproval);
                if (bResult == false)
                {
                        Response.Redirect("~/UserRestrict.aspx", false);                    
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