using IIITS.DTLMS.BL;
using IIITS.DTLMS.BL.MasterForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.MasterForms
{
    public partial class WorkOrderRangeAllocationView : System.Web.UI.Page
    {
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
                LoadWorkOrderRangeAllocationDetails();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, 
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// to load workorder range allocated details
        /// </summary>
        public void LoadWorkOrderRangeAllocationDetails()
        {
            try
            {
                ClsWorkOrderRangeAllocation objWO = new ClsWorkOrderRangeAllocation();
                if (objSession.sRoleType == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["RoleTypeStore"]))
                {
                    objWO.offcode = clsStoreOffice.Getofficecode(objSession.OfficeCode);
                }
                else
                {
                    objWO.offcode = objSession.OfficeCode;
                }

                DataTable dtWODetails = objWO.LoadWOGrid(objWO);
                if (dtWODetails.Rows.Count == 0)
                {
                    ShowEmptyGrid();
                    ViewState["WODETAILS"] = dtWODetails;
                }
                else
                {
                    grdWO.DataSource = dtWODetails;
                    grdWO.DataBind();
                    ViewState["WODETAILS"] = dtWODetails;
                }
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, 
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to  show the empty row in the grid
        /// </summary>
        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("WRA_DIV_NAME");
                dt.Columns.Add("WRA_DIV");
                dt.Columns.Add("WRA_FINANCIALYEAR");
                dt.Columns.Add("WRA_ACC_HEAD");
                dt.Columns.Add("WRA_ALLOTMENT_DATE");
                dt.Columns.Add("WRA_QUANTITY");
                dt.Columns.Add("WRA_START_RANGE");
                dt.Columns.Add("WRA_END_RANGE");

                grdWO.DataSource = dt;
                grdWO.DataBind();
                int iColCount = grdWO.Rows[0].Cells.Count;
                grdWO.Rows[0].Cells.Clear();
                grdWO.Rows[0].Cells.Add(new TableCell());
                grdWO.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdWO.Rows[0].Cells[0].Text = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EmptyData"]);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// allocation of work order range for account head
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                Response.Redirect("WorkOrderRangeAllocation.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, 
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to display popup msg 
        /// </summary>
        /// <param name="sMsg"></param>
        private void ShowMsgBox(string sMsg)
        {
            try
            {
                string sShowMsg = string.Empty;
                sShowMsg = "<script language=javascript> alert ('" + sMsg + "')</script>";
                ClientScript.RegisterStartupScript(this.GetType(),"Msg", sShowMsg);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, 
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// page indexing method to move for next page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdWO_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdWO.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["WODETAILS"];

                grdWO.DataSource = SortDataTable(dt as DataTable, true);
                grdWO.DataBind();
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, 
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to sort data table as ascending or descending
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="isPageIndexChanging"></param>
        /// <returns></returns>
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
                        ViewState["WODETAILS"] = dataView.ToTable();
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["WODETAILS"] = dataView.ToTable();
                    }
                }
                return dataView;
            }
            else
            {
                return new DataView();
            }
        }
        /// <summary>
        /// to sort as ascending or descending
        /// </summary>
        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "ASC"; }
            set { ViewState["SortDirection"] = value; }
        }
        /// <summary>
        /// to check with empty sort
        /// </summary>
        private string GridViewSortExpression
        {
            get { return ViewState["SortExpression"] as string ?? string.Empty; }
            set { ViewState["SortExpression"] = value; }
        }
        /// <summary>
        /// to sort in the given directions as asending or descending 
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// to sort the recor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdWO_Sorting(object sender, GridViewSortEventArgs e)
        {


            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdWO.PageIndex;
            DataTable dt = (DataTable)ViewState["WODETAILS"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdWO.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdWO.DataSource = dt;
            }
            grdWO.DataBind();
            grdWO.PageIndex = pageIndex;
        }
        #region Access Rights

        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY
                clsApproval objApproval = new clsApproval();
                objApproval.sFormName = "WorkOrderRangeAllocation";
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sAccessType = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["AccessRightsAll"]) + "," + sAccessType;
                bool bResult = objApproval.CheckAccessRights(objApproval);
                if (bResult == false)
                {
                    if (sAccessType == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["AccessRightsReadOnly"]))
                    {
                        Response.Redirect("~/UserRestrict.aspx", false);
                    }
                    else
                    {
                        ShowMsgBox(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["AccessRightsIfDenied"]));
                    }
                }
                return bResult;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, 
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }
        #endregion

        /// <summary>
        /// to sort data in asecnding and descending
        /// </summary>
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
        /// <summary>
        /// to generate export excel for work order Range allocated details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Export_click(object sender, EventArgs e)
        {
            DataTable dtWODetails = (DataTable)ViewState["WODETAILS"];

            if (dtWODetails.Rows.Count > 0)
            {
                dtWODetails.Columns["WRA_DIV_NAME"].ColumnName = "DIVISION NAME";
                dtWODetails.Columns["WRA_FINANCIALYEAR"].ColumnName = "FINANCIAL YEAR ";
                dtWODetails.Columns["WRA_ACC_HEAD"].ColumnName = "ACCOUNT HEAD";
                dtWODetails.Columns["WRA_ALLOTMENT_DATE"].ColumnName = "ALLOTMENT DATE";
                dtWODetails.Columns["WRA_QUANTITY"].ColumnName = "QUANTITY";
                dtWODetails.Columns["WRA_START_RANGE"].ColumnName = "START RANGE";
                dtWODetails.Columns["WRA_END_RANGE"].ColumnName = "END RANGE";
                
                List<string> listtoRemove = new List<string> { "WRA_DIV" };
                string filename = "WorkOrderRangeAllocatedDetails" + DateTime.Now + ".xls";
                string pagetitle = "WorkOrder Range Allocated Details";

                Genaral.getexcel(dtWODetails, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EmptyData"]));
            }
        }

    }
}