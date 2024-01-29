using IIITS.DTLMS.BL;
using IIITS.DTLMS.BL.POFlow;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.MasterForms
{
    public partial class PMCTcAllotmentView : System.Web.UI.Page
    {
        clsSession objSession;
        string sUserName = Convert.ToString(ConfigurationManager.AppSettings["FTP_USER"]);
        string sPassword = Convert.ToString(ConfigurationManager.AppSettings["FTP_PASS"]);

        string SFTPPath = Convert.ToString(ConfigurationManager.AppSettings["SFTPPath"]);
        string SFTPmainfolder = Convert.ToString(ConfigurationManager.AppSettings["SFTPmainfolder"]);
        /// <summary>
        ///  page load method to set default values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    CheckAccessRights(Constants.CheckAccessRights.CheckAccessRightsReadOnly);
                    DataTable dt = new DataTable();
                    clsPMCAllotment obj = new clsPMCAllotment();
                    dt = obj.GetDeliveryViewDetails(obj);
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["TOTALTC"] = dt;
                        cmdexport.Visible = false;
                    }
                    grdDIPendingTC.DataSource = dt;
                    grdDIPendingTC.DataBind();

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }    
        }
   
        /// <summary>
        ///  on change method for indexing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdAllotmentView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdAllotmentView.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["PMC_ALT_VIEW"];
                grdAllotmentView.DataSource = SortDataTable(dt as DataTable, true);
                grdAllotmentView.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        ///  to sort that data table
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
                        ViewState["PMC_ALT_VIEW"] = dataView.ToTable();
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["PMC_ALT_VIEW"] = dataView.ToTable();
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
        ///  to sort the grid baased on command
        /// </summary>
        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "ASC"; }
            set { ViewState["SortDirection"] = value; }
        }
        /// <summary>
        /// to sort the view baased on command
        /// </summary>
        private string GridViewSortExpression
        {
            get { return ViewState["SortExpression"] as string ?? string.Empty; }
            set { ViewState["SortExpression"] = value; }
        }
        /// <summary>
        /// to get sort the direction
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
        ///  to get dort the data based on arrow
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
        /// to get sorted the grid based on allotment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdAllotmentView_Sorting(object sender, GridViewSortEventArgs e)
        {

            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdAllotmentView.PageIndex;
            DataTable dt = (DataTable)ViewState["PMC_ALT_VIEW"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdAllotmentView.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdAllotmentView.DataSource = dt;
            }
            grdAllotmentView.DataBind();
            grdAllotmentView.PageIndex = pageIndex;
        }
         /// <summary>
         /// to get excegute based on row command
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
        protected void grdAllotmentView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtaltNumber = (TextBox)row.FindControl("txtaltNumber");
                    TextBox txtPoNo = (TextBox)row.FindControl("txtPoNo");
                    TextBox txtDiNo = (TextBox)row.FindControl("txtDiNo");
                    TextBox txtStoreName = (TextBox)row.FindControl("txtStoreName");
                    DataTable dt = (DataTable)ViewState["PMC_ALT_VIEW"];
                    dv = dt.DefaultView;

                    if (txtaltNumber.Text != "")
                    {
                        sFilter = " PDRA_TC_CODE Like '%" + txtaltNumber.Text.Replace("'", "'").Replace("%", "").Replace("*", "") + "%' ";
                    }

                    if (txtPoNo.Text != "")
                    {
                        sFilter = " PMC_PO_NO Like '%" + txtPoNo.Text.Replace("'", "'").Replace("%", "").Replace("*", "") + "%' ";
                    }

                    if (txtDiNo.Text != "")
                    {
                        sFilter = " PMC_DIM_DI_NO Like '%" + txtDiNo.Text.Replace("'", "'").Replace("%", "").Replace("*", "") + "%' ";
                    }
                    

                    if (txtStoreName.Text != "")
                    {
                        sFilter = " SM_NAME Like '%" + txtStoreName.Text.Replace("'", "'").Replace("%", "").Replace("*", "") + "%' ";
                    }
                    if (sFilter.Length > 0)
                    {
                        grdAllotmentView.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdAllotmentView.DataSource = dv;
                            ViewState["PMC_ALT_VIEW"] = dv.ToTable();
                            grdAllotmentView.DataBind();
                        }
                        else
                        {
                            DataTable dtPoDetails = new DataTable();
                            DataRow newRow = dtPoDetails.NewRow();
                            dtPoDetails.Rows.Add(newRow);
                            
                            dtPoDetails.Columns.Add("PDRA_ID");
                            dtPoDetails.Columns.Add("PMC_PO_NO");
                            dtPoDetails.Columns.Add("PMC_DIM_DI_NO");
                            dtPoDetails.Columns.Add("PDRA_TC_CODE");
                            dtPoDetails.Columns.Add("SM_NAME");
                            dtPoDetails.Columns.Add("MD_NAME");
                            dtPoDetails.Columns.Add("PMC_DI_CAPACITY");
                            dtPoDetails.Columns.Add("PDRA_TC_SL_NO");
                            dtPoDetails.Columns.Add("PDRA_MANUFACTURE_DATE");
                            dtPoDetails.Columns.Add("PDRA_TC_LIFE_SPAN");
                            dtPoDetails.Columns.Add("PDRA_TC_WARRENTY_PERIOD");
                            dtPoDetails.Columns.Add("PDRA_OIL_TYPE");
                            dtPoDetails.Columns.Add("PDRA_OIL_CAPACITY");
                            dtPoDetails.Columns.Add("PDRA_OIL_WEIGHT");
                            dtPoDetails.Columns.Add("TM_NAME");
                            dtPoDetails.Columns.Add("PDRA_DTR_AMOUNT");

                            grdAllotmentView.DataSource = dtPoDetails;
                            grdAllotmentView.DataBind();

                            int iColCount = grdAllotmentView.Rows[0].Cells.Count;
                            grdAllotmentView.Rows[0].Cells.Clear();
                            grdAllotmentView.Rows[0].Cells.Add(new TableCell());
                            grdAllotmentView.Rows[0].Cells[0].ColumnSpan = iColCount;
                            grdAllotmentView.Rows[0].Cells[0].Text = ConfigurationManager.AppSettings["EmptyData"];
                            ViewState["PMC_ALT_VIEW"] = dt;
                        }
                    }
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
        ///  to show the popup based on validation
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
        ///  to create new record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdNew_Click(object sender, EventArgs e)
        {
            try
            {
                //Check AccessRights
                bool bAccResult = CheckAccessRights(Constants.CheckAccessRights.CheckAccessRightsAll);
                if (bAccResult == false)
                {
                    return;
                }
                Response.Redirect("PMCTcAllotment.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        ///  to generate export excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Export_click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["PMC_ALT_VIEW"];
            if (dt != null)
            {

                if (dt.Rows.Count > 0)
                {
                    dt.Columns["PMC_PO_NO"].ColumnName = "PO No";
                    dt.Columns["PMC_DIM_DI_NO"].ColumnName = "DI No";
                    dt.Columns["PDRA_TC_CODE"].ColumnName = "DTr Code";
                    dt.Columns["SM_NAME"].ColumnName = "Store Name";
                    dt.Columns["MD_NAME"].ColumnName = "Rating";
                    dt.Columns["PMC_DI_CAPACITY"].ColumnName = "Capacity";
                    dt.Columns["PDRA_TC_SL_NO"].ColumnName = "TC Sl No";
                    dt.Columns["PDRA_MANUFACTURE_DATE"].ColumnName = "Manufacture Date";
                    dt.Columns["PDRA_TC_LIFE_SPAN"].ColumnName = "Life Span";
                    dt.Columns["PDRA_TC_WARRENTY_PERIOD"].ColumnName = "Warrenty Period";
                    dt.Columns["PDRA_OIL_TYPE"].ColumnName = "Oil Type";
                    dt.Columns["PDRA_OIL_CAPACITY"].ColumnName = "DTr Oil Capacity";
                    dt.Columns["PDRA_OIL_WEIGHT"].ColumnName = "DTr Weight";
                    dt.Columns["TM_NAME"].ColumnName = "Make Name";
                    dt.Columns["PDRA_DTR_AMOUNT"].ColumnName = "DTr Amount";
                    List<string> listtoRemove = new List<string> { "PDRA_ID", "PMC_DI_ID" };
                    string filename = "DTr_AllotmentDetails" + DateTime.Now + ".xls";
                    string pagetitle = "DTr Allotment Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox(ConfigurationManager.AppSettings["EmptyData"]);

                    DataTable dtPoDetails = new DataTable();
                    DataRow newRow = dtPoDetails.NewRow();
                    dtPoDetails.Rows.Add(newRow);

                    dtPoDetails.Columns.Add("PDRA_ID");
                    dtPoDetails.Columns.Add("PMC_PO_NO");
                    dtPoDetails.Columns.Add("PMC_DIM_DI_NO");
                    dtPoDetails.Columns.Add("PDRA_TC_CODE");
                    dtPoDetails.Columns.Add("SM_NAME");
                    dtPoDetails.Columns.Add("MD_NAME");
                    dtPoDetails.Columns.Add("PMC_DI_CAPACITY");
                    dtPoDetails.Columns.Add("PDRA_TC_SL_NO");
                    dtPoDetails.Columns.Add("PDRA_MANUFACTURE_DATE");
                    dtPoDetails.Columns.Add("PDRA_TC_LIFE_SPAN");
                    dtPoDetails.Columns.Add("PDRA_TC_WARRENTY_PERIOD");
                    dtPoDetails.Columns.Add("PDRA_OIL_TYPE");
                    dtPoDetails.Columns.Add("PDRA_OIL_CAPACITY");
                    dtPoDetails.Columns.Add("PDRA_OIL_WEIGHT");
                    dtPoDetails.Columns.Add("TM_NAME");
                    dtPoDetails.Columns.Add("PDRA_DTR_AMOUNT");

                    grdAllotmentView.DataSource = dtPoDetails;
                    grdAllotmentView.DataBind();

                    int iColCount = grdAllotmentView.Rows[0].Cells.Count;
                    grdAllotmentView.Rows[0].Cells.Clear();
                    grdAllotmentView.Rows[0].Cells.Add(new TableCell());
                    grdAllotmentView.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdAllotmentView.Rows[0].Cells[0].Text = ConfigurationManager.AppSettings["EmptyData"];

                }
            }
            else
            {
                ShowMsgBox(Convert .ToString(ConfigurationManager.AppSettings["EmptyData"]));
            }
        }
        /// <summary>
        /// to check the access rights
        /// </summary>
        /// <param name="sAccessType"></param>
        /// <returns></returns>
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "PMCTcAllotment";
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sAccessType = Constants.CheckAccessRights.CheckAccessRightsAll + "," + sAccessType;
                bool bResult = objApproval.CheckAccessRights(objApproval);
                if (bResult == false)
                {
                    if (sAccessType == Constants.CheckAccessRights.CheckAccessRightsReadOnly)
                    {
                        Response.Redirect("~/UserRestrict.aspx", false);
                    }
                    else
                    {
                        ShowMsgBox(Constants.General.AccessDenaidMsg);
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

        /// <summary>
        ///  to search tha data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            try
            {

                clsAllotement obj = new clsAllotement();

                obj.GetDispatchCount(obj);
                BindgridView(SFTPmainfolder, sUserName, sPassword);
                DataTable dt = new DataTable();
                dt = obj.GetDeliveryDetails(obj);
                if (dt.Rows.Count > 0)
                {
                    ViewState["TOTALTC"] = dt;
                }
               
                grdDIPendingTC.DataSource = dt;
                grdDIPendingTC.DataBind();
              
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        ///  to get changes based on index page 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDIPendingTC_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdDIPendingTC.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["TOTALTC"];
                grdDIPendingTC.DataSource = SortDataTable(dt as DataTable, true);
                grdDIPendingTC.DataBind();

                DataTable dt1= (DataTable)ViewState["PMC_ALT_VIEW"];
                if (dt1.Rows.Count == 0)
                {
                    int iColCount = grdAllotmentView.Rows[0].Cells.Count;
                    grdAllotmentView.Rows[0].Cells.Clear();
                    grdAllotmentView.Rows[0].Cells.Add(new TableCell());
                    grdAllotmentView.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdAllotmentView.Rows[0].Cells[0].Text = "No Records Found";
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to fetch the file data from server
        /// </summary>
        /// <param name="FtpServer"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public DataTable BindgridView(string FtpServer, string username, string password)
        {
            DataTable dtFiles = new DataTable();
            try
            {
                string SFTPPath = Convert.ToString(ConfigurationManager.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(ConfigurationManager.AppSettings["SFTPmainfolder"]);
                clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);

                //path for get files from ftp
                bool IsExists = objFtp.FtpDirectoryExists(FtpServer);

                return dtFiles;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtFiles;
            }
        }
        /// <summary>
        ///  to load the details of allotment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdLoad_click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            clsPMCAllotment objDi = new clsPMCAllotment();
            try
            {


                LinkButton lnkdwn = (LinkButton)sender;
                GridViewRow rw = (GridViewRow)lnkdwn.NamingContainer;
                objDi.sDino = ((Label)rw.FindControl("lblDINO")).Text;
                objDi.sMake = ((Label)rw.FindControl("lblMake")).Text;
                objDi.sStorename = ((Label)rw.FindControl("lblstore")).Text;
                objDi.sCapacity = ((Label)rw.FindControl("lblCapacity")).Text;
                objDi.sRating = ((Label)rw.FindControl("lblRating")).Text;
                objDi.sTotqty = ((Label)rw.FindControl("lblQuantity")).Text;
                objDi.sStartrange = ((Label)rw.FindControl("lblstartrange")).Text;
                objDi.sEndrange = ((Label)rw.FindControl("lblendrange")).Text;
                objDi.sMakeId = ((Label)rw.FindControl("lblMakeId")).Text;
                objDi.sDimDino = ((Label)rw.FindControl("lbldimid")).Text;
                objDi.sPoid = ((Label)rw.FindControl("lblpoid")).Text;

                dt = objDi.LoadALlotmentdetails(objDi);

                if (dt.Rows.Count <= 0)
                {

                    DataTable dtPoDetails = new DataTable();
                    DataRow newRow = dtPoDetails.NewRow();
                    dtPoDetails.Rows.Add(newRow);
                    
                    dtPoDetails.Columns.Add("PDRA_ID");
                    dtPoDetails.Columns.Add("PMC_PO_NO");
                    dtPoDetails.Columns.Add("PMC_DIM_DI_NO");
                    dtPoDetails.Columns.Add("PDRA_TC_CODE");
                    dtPoDetails.Columns.Add("SM_NAME");
                    dtPoDetails.Columns.Add("MD_NAME");
                    dtPoDetails.Columns.Add("PMC_DI_CAPACITY");
                    dtPoDetails.Columns.Add("PDRA_TC_SL_NO");
                    dtPoDetails.Columns.Add("PDRA_MANUFACTURE_DATE");
                    dtPoDetails.Columns.Add("PDRA_TC_LIFE_SPAN");
                    dtPoDetails.Columns.Add("PDRA_TC_WARRENTY_PERIOD");
                    dtPoDetails.Columns.Add("PDRA_OIL_TYPE");
                    dtPoDetails.Columns.Add("PDRA_OIL_CAPACITY");
                    dtPoDetails.Columns.Add("PDRA_OIL_WEIGHT");
                    dtPoDetails.Columns.Add("TM_NAME");
                    dtPoDetails.Columns.Add("PDRA_DTR_AMOUNT");
                    grdAllotmentView.DataSource = dtPoDetails;
                    grdAllotmentView.DataBind();

                    int iColCount = grdAllotmentView.Rows[0].Cells.Count;
                    grdAllotmentView.Rows[0].Cells.Clear();
                    grdAllotmentView.Rows[0].Cells.Add(new TableCell());
                    grdAllotmentView.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdAllotmentView.Rows[0].Cells[0].Text = "No Records Found";
                    ViewState["PMC_ALT_VIEW"] = dt;
                }

                else
                {
                    grdAllotmentView.DataSource = dt;
                    grdAllotmentView.DataBind();
                    ViewState["PMC_ALT_VIEW"] = dt;
                    cmdexport.Visible = true;

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
        ///  to get exceguted based on row commands
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void grdDIPendingTC_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "Search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtdiNumber = (TextBox)row.FindControl("txtdiNumber");

                    DataTable dt = (DataTable)ViewState["TOTALTC"];
                    dv = dt.DefaultView;

    
                    if (txtdiNumber.Text != "")
                    {
                        sFilter = "PMC_DIM_DI_NO Like '%" + txtdiNumber.Text.Replace("'", "'") + "%' ";
                    }
           

                    if (sFilter.Length > 0)
                    {

                        grdDIPendingTC.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdDIPendingTC.DataSource = dv;
                            ViewState["TOTALTC"] = dv.ToTable();
                            grdDIPendingTC.DataBind();

                        }
                        else
                        {
                            ShowEmptyGridForAllotmentpending();
                        }
                    }
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
        /// This function used to show empty grid for allotment pending 
        /// </summary>
        public void ShowEmptyGridForAllotmentpending()
        {
            try
            {
                DataTable dtPoDetails = new DataTable();
                DataRow newRow = dtPoDetails.NewRow();
                dtPoDetails.Rows.Add(newRow);
                dtPoDetails.Columns.Add("PMC_DIM_DI_NO");
                dtPoDetails.Columns.Add("PMC_DI_MAKE_ID");
                dtPoDetails.Columns.Add("MAKE_NAME");
                dtPoDetails.Columns.Add("STORE_NAME");
                dtPoDetails.Columns.Add("PMC_DI_CAPACITY");
                dtPoDetails.Columns.Add("STAR_RATE");
                dtPoDetails.Columns.Add("PENDING");
                dtPoDetails.Columns.Add("TOTAL");
                dtPoDetails.Columns.Add("START_RANGE");
                dtPoDetails.Columns.Add("END_RANGE");
                dtPoDetails.Columns.Add("PMC_DIM_ID");
                dtPoDetails.Columns.Add("PMC_DIM_PO_ID");
                dtPoDetails.Columns.Add("STATUS");

                grdDIPendingTC.DataSource = dtPoDetails;
                ViewState["TOTALTC"] = dtPoDetails;

                grdDIPendingTC.DataBind();

                int iColCount = grdDIPendingTC.Rows[0].Cells.Count;
                grdDIPendingTC.Rows[0].Cells.Clear();
                grdDIPendingTC.Rows[0].Cells.Add(new TableCell());
                grdDIPendingTC.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdDIPendingTC.Rows[0].Cells[0].Text = ConfigurationManager.AppSettings["EmptyData"];
            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
          
        }
    }
}