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
    public partial class PoMasterView : System.Web.UI.Page
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
                    CheckAccessRights("4");
                    LoadPoMasterDetails();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void LoadPoMasterDetails(string sPoNumber = "")
        {
            try
            {
                clsPoMaster objPoMaster = new clsPoMaster();
                DataTable dt = new DataTable();
                objPoMaster.sPoNo = sPoNumber;

                dt = objPoMaster.LoadPoDetailGrid(objPoMaster);

                if (dt.Rows.Count <= 0)
                {
                    DataTable dtPoDetails = new DataTable();
                    DataRow newRow = dtPoDetails.NewRow();
                    dtPoDetails.Rows.Add(newRow);
                    dtPoDetails.Columns.Add("PO_ID");
                    dtPoDetails.Columns.Add("PO_NO");
                    dtPoDetails.Columns.Add("PO_DATE");
                    dtPoDetails.Columns.Add("PB_QUANTITY");
                    dtPoDetails.Columns.Add("PO_SUPPLIER_ID");

                    grdPoMasterView.DataSource = dtPoDetails;
                    grdPoMasterView.DataBind();

                    int iColCount = grdPoMasterView.Rows[0].Cells.Count;
                    grdPoMasterView.Rows[0].Cells.Clear();
                    grdPoMasterView.Rows[0].Cells.Add(new TableCell());
                    grdPoMasterView.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdPoMasterView.Rows[0].Cells[0].Text = "No Records Found";
                    ViewState["PO"] = dt;

                }
                else
                {
                    grdPoMasterView.DataSource = dt;
                    grdPoMasterView.DataBind();
                    ViewState["PO"] = dt;

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
                        ViewState["PO"] = dataView.ToTable();

                    }
                    else
                    {
                        
                            dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                            ViewState["PO"] = dataView.ToTable();
                       
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
                    string strPoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblPoId.Text));
                    string strPoQnty = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblPoQunatity.Text));
                    Response.Redirect("PoMaster.aspx?QryPoId=" + strPoId + "&QryPoQnty=" + strPoQnty, false);
                }
                if (e.CommandName == "search")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtsPoNumber = (TextBox)row.FindControl("txtsPoNumber");
                    LoadPoMasterDetails(txtsPoNumber.Text);
                }
               // LoadPoMasterDetails();
                 
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
                Response.Redirect("PoMaster.aspx", false);
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

            //clsPoMaster objPoMaster = new clsPoMaster();
            //DataTable dt = new DataTable();
            //string sPoNumber = "";
            //objPoMaster.sPoNo = sPoNumber;

            //dt = objPoMaster.LoadPoDetailGrid(objPoMaster);
            DataTable dt = (DataTable)ViewState["PO"];

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

                dt.Columns["PO_NO"].ColumnName = "PO No";
                dt.Columns["PO_DATE"].ColumnName = "PO DATE";
                dt.Columns["PO_SUPPLIER_ID"].ColumnName = "SUPPLIER NAME";
                dt.Columns["PB_QUANTITY"].ColumnName = "QUANTITY";

                dt.Columns["SUPPLIER NAME"].SetOrdinal(3);
                List<string> listtoRemove = new List<string> { "PO_ID" };
                string filename = "PODetails" + DateTime.Now + ".xls";
                string pagetitle = "Purchase Order Details";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);


            }
            else {
                ShowMsgBox("No record found");

                DataTable dtPoDetails = new DataTable();
                DataRow newRow = dtPoDetails.NewRow();
                dtPoDetails.Rows.Add(newRow);
                dtPoDetails.Columns.Add("PO_ID");
                dtPoDetails.Columns.Add("PO_NO");
                dtPoDetails.Columns.Add("PO_DATE");
                dtPoDetails.Columns.Add("PB_QUANTITY");
                dtPoDetails.Columns.Add("PO_SUPPLIER_ID");

                grdPoMasterView.DataSource = dtPoDetails;
                grdPoMasterView.DataBind();

                int iColCount = grdPoMasterView.Rows[0].Cells.Count;
                grdPoMasterView.Rows[0].Cells.Clear();
                grdPoMasterView.Rows[0].Cells.Add(new TableCell());
                grdPoMasterView.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdPoMasterView.Rows[0].Cells[0].Text = "No Records Found";
              
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