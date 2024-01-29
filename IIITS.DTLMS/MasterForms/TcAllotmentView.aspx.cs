using IIITS.DTLMS.BL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.MasterForms
{
    public partial class TcAllotmentView : System.Web.UI.Page
    {
        string strFormCode = "AllotmentView";
        clsSession objSession;
        string sUserName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_USER"]);
        string sPassword = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_PASS"]);

        string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
        string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);
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
                    clsAllotement obj = new clsAllotement();
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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }    
        }
         /// <summary>
         ///  to load allotment date
         /// </summary>
         /// <param name="sDINumber"></param>
        public void LoadAllotmentDetails(string sDINumber ="")
        {
            try
            {
                clsAllotement objAllot = new clsAllotement();
                DataTable dt = new DataTable();
                objAllot.sDINo = sDINumber;

                dt = objAllot.GetAllotedDetails(sDINumber.ToUpper());

                if (dt.Rows.Count <= 0)
                {
                    DataTable dtPoDetails = new DataTable();
                    DataRow newRow = dtPoDetails.NewRow();
                    dtPoDetails.Rows.Add(newRow);
                    dtPoDetails.Columns.Add("TCP_ID");
                    dtPoDetails.Columns.Add("DIM_DI_NO");
                    dtPoDetails.Columns.Add("TCP_TC_CODE");
                    dtPoDetails.Columns.Add("SM_NAME");
                    dtPoDetails.Columns.Add("MD_NAME");
                    dtPoDetails.Columns.Add("TCP_OIL_CAPACITY");
                    grdAllotmentView.DataSource = dtPoDetails;
                    grdAllotmentView.DataBind();

                    int iColCount = grdAllotmentView.Rows[0].Cells.Count;
                    grdAllotmentView.Rows[0].Cells.Clear();
                    grdAllotmentView.Rows[0].Cells.Add(new TableCell());
                    grdAllotmentView.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdAllotmentView.Rows[0].Cells[0].Text = ConfigurationManager.AppSettings["EmptyData"];
                    ViewState["ALT_VIEW"] = dt;

                }
                else
                {
                    grdAllotmentView.DataSource = dt;
                    grdAllotmentView.DataBind();
                    ViewState["ALT_VIEW"] = dt;

                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
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
                DataTable dt = (DataTable)ViewState["ALT_VIEW"];
                grdAllotmentView.DataSource = SortDataTable(dt as DataTable, true);
                grdAllotmentView.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
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
                        ViewState["ALT_VIEW"] = dataView.ToTable();
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["ALT_VIEW"] = dataView.ToTable();
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
            DataTable dt = (DataTable)ViewState["ALT_VIEW"];
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
                if (e.CommandName == "Submit")
                {
                    //Check AccessRights
                    bool bAccResult = CheckAccessRights(Constants.CheckAccessRights.CheckAccessRightsModify);
                    if (bAccResult == false)
                    {
                        return;
                    }
                    GridViewRow row = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
                    int rowindex = row.RowIndex;
                    Label lblAltid = (Label)grdAllotmentView.Rows[rowindex].FindControl("lblAltid");
                    Label lblAltNo = (Label)grdAllotmentView.Rows[rowindex].FindControl("lblAltNo");
                    Label lblDiNo = (Label)grdAllotmentView.Rows[rowindex].FindControl("lblDiNo");
                    string strAltid = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblAltid.Text));
                    string strAltNo = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblAltNo.Text));
                    string strDiNo = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblDiNo.Text));
                    Response.Redirect("TcAllotment.aspx?QryAltid=" + strAltid + "&QryDiNo=" + strDiNo + "&QryAltNo=" + strAltNo, false);
                }
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtaltNumber = (TextBox)row.FindControl("txtaltNumber");
                    TextBox txtPoNo = (TextBox)row.FindControl("txtPoNo");
                    TextBox txtDiNo = (TextBox)row.FindControl("txtDiNo");
                    TextBox txtStoreName = (TextBox)row.FindControl("txtStoreName");
                    DataTable dt = (DataTable)ViewState["ALT_VIEW"];
                    dv = dt.DefaultView;

                    if (txtaltNumber.Text != "")
                    {
                        sFilter = " TCP_TC_CODE Like '%" + txtaltNumber.Text.Replace("'", "'").Replace("%", "").Replace("*", "") + "%' ";
                    }

                    if (txtPoNo.Text != "")
                    {
                        sFilter = " PO_NO Like '%" + txtPoNo.Text.Replace("'", "'").Replace("%", "").Replace("*", "") + "%' ";
                    }

                    if (txtDiNo.Text != "")
                    {
                        sFilter = " DIM_DI_NO Like '%" + txtDiNo.Text.Replace("'", "'").Replace("%", "").Replace("*", "") + "%' ";
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
                            ViewState["ALT_VIEW"] = dv.ToTable();
                            grdAllotmentView.DataBind();
                        }
                        else
                        {
                            DataTable dtPoDetails = new DataTable();
                            DataRow newRow = dtPoDetails.NewRow();
                            dtPoDetails.Rows.Add(newRow);
                            
                            dtPoDetails.Columns.Add("TCP_ID");
                            dtPoDetails.Columns.Add("PO_NO");
                            dtPoDetails.Columns.Add("DIM_DI_NO");
                            dtPoDetails.Columns.Add("TCP_TC_CODE");
                            dtPoDetails.Columns.Add("SM_NAME");
                            dtPoDetails.Columns.Add("MD_NAME");
                            dtPoDetails.Columns.Add("DI_CAPACITY");
                            dtPoDetails.Columns.Add("TCP_TC_SL_NO");
                            dtPoDetails.Columns.Add("TCP_MANUFACTURE_DATE");
                            dtPoDetails.Columns.Add("TCP_TC_LIFE_SPAN");
                            dtPoDetails.Columns.Add("TCP_TC_WARRENTY_PERIOD");
                            dtPoDetails.Columns.Add("TCP_OIL_TYPE");
                            dtPoDetails.Columns.Add("TCP_OIL_CAPACITY");
                            dtPoDetails.Columns.Add("TCP_OIL_WEIGHT");
                            dtPoDetails.Columns.Add("TM_NAME");

                            grdAllotmentView.DataSource = dtPoDetails;
                            grdAllotmentView.DataBind();

                            int iColCount = grdAllotmentView.Rows[0].Cells.Count;
                            grdAllotmentView.Rows[0].Cells.Clear();
                            grdAllotmentView.Rows[0].Cells.Add(new TableCell());
                            grdAllotmentView.Rows[0].Cells[0].ColumnSpan = iColCount;
                            grdAllotmentView.Rows[0].Cells[0].Text = ConfigurationManager.AppSettings["EmptyData"];
                            ViewState["ALT_VIEW"] = dt;
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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
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
                Response.Redirect("TcAllotment.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        ///  to generate export excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Export_click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["ALT_VIEW"];
            if (dt != null)
            {

                if (dt.Rows.Count > 0)
                {
                    dt.Columns["PO_NO"].ColumnName = "Po No";
                    dt.Columns["DIM_DI_NO"].ColumnName = "Dispatch Number";
                    dt.Columns["TCP_TC_CODE"].ColumnName = "DTr Code";
                    dt.Columns["SM_NAME"].ColumnName = "Store Name";
                    dt.Columns["MD_NAME"].ColumnName = "Rating";
                    dt.Columns["DI_CAPACITY"].ColumnName = "Capacity";
                    dt.Columns["TCP_TC_SL_NO"].ColumnName = "TC Sl No";
                    dt.Columns["TCP_MANUFACTURE_DATE"].ColumnName = "Manufacture Date";
                    dt.Columns["TCP_TC_LIFE_SPAN"].ColumnName = "Life Span";
                    dt.Columns["TCP_TC_WARRENTY_PERIOD"].ColumnName = "Warrenty Period";
                    dt.Columns["TCP_OIL_TYPE"].ColumnName = "Oil Type";
                    dt.Columns["TCP_OIL_CAPACITY"].ColumnName = "TC Oil Capacity";
                    dt.Columns["TCP_OIL_WEIGHT"].ColumnName = "TC Weight";
                    dt.Columns["TM_NAME"].ColumnName = "Make Name";

                    List<string> listtoRemove = new List<string> { "TCP_ID","DI_ID" };
                    string filename = "AllotmentDetails" + DateTime.Now + ".xls";
                    string pagetitle = "Allotment Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox(ConfigurationManager.AppSettings["EmptyData"]);

                    DataTable dtPoDetails = new DataTable();
                    DataRow newRow = dtPoDetails.NewRow();
                    dtPoDetails.Rows.Add(newRow);
                    dtPoDetails.Columns.Add("TCP_ID");
                    dtPoDetails.Columns.Add("PO_NO");
                    dtPoDetails.Columns.Add("DIM_DI_NO");
                    dtPoDetails.Columns.Add("TCP_TC_CODE");
                    dtPoDetails.Columns.Add("SM_NAME");
                    dtPoDetails.Columns.Add("MD_NAME");
                    dtPoDetails.Columns.Add("DI_CAPACITY");
                    dtPoDetails.Columns.Add("TCP_TC_SL_NO");
                    dtPoDetails.Columns.Add("TCP_MANUFACTURE_DATE");
                    dtPoDetails.Columns.Add("TCP_TC_LIFE_SPAN");
                    dtPoDetails.Columns.Add("TCP_TC_WARRENTY_PERIOD");
                    dtPoDetails.Columns.Add("TCP_OIL_TYPE");
                    dtPoDetails.Columns.Add("TCP_OIL_CAPACITY");
                    dtPoDetails.Columns.Add("TCP_OIL_WEIGHT");
                    dtPoDetails.Columns.Add("TM_NAME");

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

                objApproval.sFormName = "PoMaster";
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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
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

                DataTable dt1= (DataTable)ViewState["ALT_VIEW"];
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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
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
                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);
                clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);

                //path for get files from ftp
                bool IsExists = objFtp.FtpDirectoryExists(FtpServer);

                return dtFiles;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
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
            clsAllotement objDi = new clsAllotement();
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
                objDi.sPendingQty = ((Label)rw.FindControl("lblPenQuantity")).Text;
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
                    
                    dtPoDetails.Columns.Add("TCP_ID");
                    dtPoDetails.Columns.Add("PO_NO");
                    dtPoDetails.Columns.Add("DIM_DI_NO");
                    dtPoDetails.Columns.Add("TCP_TC_CODE");
                    dtPoDetails.Columns.Add("SM_NAME");
                    dtPoDetails.Columns.Add("MD_NAME");
                    dtPoDetails.Columns.Add("DI_CAPACITY");
                    dtPoDetails.Columns.Add("TCP_TC_SL_NO");
                    dtPoDetails.Columns.Add("TCP_MANUFACTURE_DATE");
                    dtPoDetails.Columns.Add("TCP_TC_LIFE_SPAN");
                    dtPoDetails.Columns.Add("TCP_TC_WARRENTY_PERIOD");
                    dtPoDetails.Columns.Add("TCP_OIL_TYPE");
                    dtPoDetails.Columns.Add("TCP_OIL_CAPACITY");
                    dtPoDetails.Columns.Add("TCP_OIL_WEIGHT");
                    dtPoDetails.Columns.Add("TM_NAME");

                    grdAllotmentView.DataSource = dtPoDetails;
                    grdAllotmentView.DataBind();

                    int iColCount = grdAllotmentView.Rows[0].Cells.Count;
                    grdAllotmentView.Rows[0].Cells.Clear();
                    grdAllotmentView.Rows[0].Cells.Add(new TableCell());
                    grdAllotmentView.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdAllotmentView.Rows[0].Cells[0].Text = "No Records Found";
                    ViewState["ALT_VIEW"] = dt;
                }

                else
                {
                    grdAllotmentView.DataSource = dt;
                    grdAllotmentView.DataBind();
                    ViewState["ALT_VIEW"] = dt;
                    cmdexport.Visible = true;

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
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
                        sFilter = "DIM_DI_NO Like '%" + txtdiNumber.Text.Replace("'", "'") + "%' ";
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
                            DataTable dtPoDetails = new DataTable();
                            DataRow newRow = dtPoDetails.NewRow();
                            dtPoDetails.Rows.Add(newRow);
                            dtPoDetails.Columns.Add("DIM_DI_NO"); 
                            dtPoDetails.Columns.Add("DI_MAKE_ID");
                            dtPoDetails.Columns.Add("MAKE_NAME");
                            dtPoDetails.Columns.Add("STORE_NAME");
                            dtPoDetails.Columns.Add("DI_CAPACITY");
                            dtPoDetails.Columns.Add("STAR_RATE");
                            dtPoDetails.Columns.Add("PENDING");
                            dtPoDetails.Columns.Add("TOTAL");
                            dtPoDetails.Columns.Add("START_RANGE");
                            dtPoDetails.Columns.Add("END_RANGE");  
                            dtPoDetails.Columns.Add("DIM_ID");
                            dtPoDetails.Columns.Add("DIM_PO_ID");
                            dtPoDetails.Columns.Add("STATUS");

                            grdDIPendingTC.DataSource = dtPoDetails;
                            grdDIPendingTC.DataBind();

                            int iColCount = grdDIPendingTC.Rows[0].Cells.Count;
                            grdDIPendingTC.Rows[0].Cells.Clear();
                            grdDIPendingTC.Rows[0].Cells.Add(new TableCell());
                            grdDIPendingTC.Rows[0].Cells[0].ColumnSpan = iColCount;
                            grdDIPendingTC.Rows[0].Cells[0].Text = ConfigurationManager.AppSettings["EmptyData"];
                            ViewState["TOTALTC"] = dt;
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
    }
}