using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IIITS.DTLMS.BL;
using System.Reflection;

namespace IIITS.DTLMS.MasterForms
{
    public partial class DeviceRegister : System.Web.UI.Page
    {
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToString(Session["clsSession"] ?? "").Length == 0)
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                else
                {
                    objSession = (clsSession)Session["clsSession"];
                    lblMessage.Text = string.Empty;
                    if (!IsPostBack)
                    {
                        CheckAccessRights("4");
                        // AdminAccess(); 
                        LoadDeviceRegistrerDetails("", "");
                    }
                    btnPendingRecords.BackColor = System.Drawing.Color.Empty;
                    btnApprovedRecords.BackColor = System.Drawing.Color.Empty;
                    btnDesabeldRecords.BackColor = System.Drawing.Color.Empty;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, 
                    ex.Message,
                    ex.StackTrace);
            }
        }
        /// <summary>
        /// Check Access Rights
        /// </summary>
        /// <param name="sAccessType"></param>
        /// <returns></returns>
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY
                clsApproval objApproval = new clsApproval();
                objApproval.sFormName = "DeviceRegistration";
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
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
                return false;
            }
        }
        /// <summary>
        /// Load Device Registrer Details
        /// </summary>
        /// <param name="sDeviceNo"></param>
        /// <param name="sName"></param>
        public void LoadDeviceRegistrerDetails(string sDeviceNo = "", string sName = "", string ButtonFlag = "")
        {
            try
            {
                clsDeviceRegister objDevice = new clsDeviceRegister();
                objDevice.sDeviceId = sDeviceNo;
                objDevice.sFullName = sName;
                objDevice.soffcode = objSession.OfficeCode;
                objDevice.sUserId = objSession.UserId;
                if (ButtonFlag == "")
                {
                    objDevice.DeviceStatus = "P";
                    hidbuttonVal.Text = objDevice.DeviceStatus;
                }
                else
                {
                    objDevice.DeviceStatus = ButtonFlag;
                }
                
                DataTable dtDeviceDetails = objDevice.LoadDeviceGrid(objDevice);
                if (dtDeviceDetails.Rows.Count <= 0)
                {
                    DataTable dtDevDetails = new DataTable();
                    DataRow newRow = dtDevDetails.NewRow();
                    dtDevDetails.Rows.Add(newRow);
                    dtDevDetails.Columns.Add("US_ID");
                    dtDevDetails.Columns.Add("MR_REQUEST_BY");
                    dtDevDetails.Columns.Add("MR_ID");
                    dtDevDetails.Columns.Add("MR_DEVICE_ID");
                    dtDevDetails.Columns.Add("US_FULL_NAME");
                    dtDevDetails.Columns.Add("MR_APPROVE_STATUS");
                    dtDevDetails.Columns.Add("MR_CRON");
                    grdDeviceRegister.DataSource = dtDevDetails;
                    grdDeviceRegister.DataBind();
                    int iColCount = grdDeviceRegister.Rows[0].Cells.Count;
                    grdDeviceRegister.Rows[0].Cells.Clear();
                    grdDeviceRegister.Rows[0].Cells.Add(new TableCell());
                    grdDeviceRegister.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdDeviceRegister.Rows[0].Cells[0].Text = "No Records Found";
                    ViewState["Device"] = dtDeviceDetails;
                }
                else
                {
                    grdDeviceRegister.DataSource = dtDeviceDetails;
                    grdDeviceRegister.DataBind();
                    ViewState["Device"] = dtDeviceDetails;
                }
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sMsg"></param>
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
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDeviceRegister_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdDeviceRegister.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Device"];
                dt.Columns["MR_DEVICE_ID"].AllowDBNull = true;
                dt.Columns["US_FULL_NAME"].AllowDBNull = true;
                grdDeviceRegister.DataSource = SortDataTable(dt as DataTable, true);
                grdDeviceRegister.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }
        /// <summary>
        /// 
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
                        ViewState["Device"] = dataView.ToTable();
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());

                        ViewState["Device"] = dataView.ToTable();
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
        /// <summary>
        /// 
        /// </summary>
        private string GridViewSortExpression
        {
            get { return ViewState["SortExpression"] as string ?? string.Empty; }
            set { ViewState["SortExpression"] = value; }
        }
        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDeviceRegister_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdDeviceRegister.PageIndex;
            DataTable dt = (DataTable)ViewState["Device"];
            string sortingDirection = string.Empty;
            Image sortImage = new Image();
            Image sortImageboth = new Image();
            if (dt.Rows.Count > 0)
            {
                grdDeviceRegister.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdDeviceRegister.DataSource = dt;
            }
            grdDeviceRegister.DataBind();
            grdDeviceRegister.PageIndex = pageIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDeviceRegister_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtSearchName = (TextBox)row.FindControl("txtsFullName");
                    TextBox txtSearchDevice = (TextBox)row.FindControl("txtIvDeviceId");
                    string HiddenField = hidbuttonVal.Text;

                    LoadDeviceRegistrerDetails(txtSearchDevice.Text, txtSearchName.Text, HiddenField);
                }
                if (e.CommandName == "status")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    string sUserId = ((Label)row.FindControl("lblUserId")).Text;
                    string sMuId = ((Label)row.FindControl("lblMrId")).Text;
                    ImageButton imgBtnApproval;
                    imgBtnApproval = (ImageButton)row.FindControl("imgBtnApproval");
                    clsDeviceRegister objDevice = new clsDeviceRegister();
                    objDevice.sRequestedBy = sUserId;
                    objDevice.sMuId = sMuId;
                    objDevice.sApprovedBy = objSession.UserId;
                    objDevice.soffcode = objSession.OfficeCode;
                    objDevice.sDeviceId = "";
                    objDevice.sFullName = "";
                    objDevice.sUserId = objSession.UserId;

                    if (imgBtnApproval.Visible == true)
                    {
                        objDevice.sApprovalStatus = "1";
                    }
                    else
                    {
                        objDevice.sApprovalStatus = "2";
                    }
                    bool status = objDevice.UpdateDeviceStatus(objDevice);
                    if (status == true)
                    {
                        ShowMsgBox("Device Updated SuccessFully");

                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(
                                objSession.sClientIP,
                                objSession.UserId, 
                                "Device Register successfully");
                        }
                    }
                    else
                    {
                        ShowMsgBox("Device Approval Failed");
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(
                                objSession.sClientIP, 
                                objSession.UserId, 
                                "Device Register Failed");
                        }
                    }
                    DataTable dtDeviceDetails = new DataTable();
                    string HiddenField = hidbuttonVal.Text;
                    objDevice.DeviceStatus = HiddenField;

                    dtDeviceDetails = objDevice.LoadDeviceGrid(objDevice);
                    grdDeviceRegister.DataSource = dtDeviceDetails;
                    grdDeviceRegister.DataBind();
                }
                if (e.CommandName == "activate")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    string sUserId = ((Label)row.FindControl("lblUserId")).Text;
                    string sMuId = ((Label)row.FindControl("lblMrId")).Text;
                    ImageButton imgDeactive;
                    imgDeactive = (ImageButton)row.FindControl("imgDeactive");
                    clsDeviceRegister objDevice = new clsDeviceRegister();
                    objDevice.sRequestedBy = sUserId;
                    objDevice.sMuId = sMuId;
                    objDevice.sApprovedBy = objSession.UserId;
                    objDevice.soffcode = objSession.OfficeCode;
                    objDevice.sDeviceId = "";
                    objDevice.sFullName = "";
                    objDevice.sUserId = objSession.UserId;
                    if (imgDeactive.Visible == true)
                    {
                        objDevice.sApprovalStatus = "1";
                    }
                    bool status = objDevice.UpdateDeviceStatus(objDevice);
                    if (status == true)
                    {
                        ShowMsgBox("Device Updated SuccessFully");

                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(
                                objSession.sClientIP,
                                objSession.UserId, 
                                " Re-activated Device successfully");
                        }
                    }
                    DataTable dtDeviceDetails = new DataTable();

                    string HiddenField = hidbuttonVal.Text;
                    objDevice.DeviceStatus = HiddenField;

                    dtDeviceDetails = objDevice.LoadDeviceGrid(objDevice);
                    grdDeviceRegister.DataSource = dtDeviceDetails;
                    grdDeviceRegister.DataBind();
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }
        /// <summary>
        /// grd Device Register Row DataBound 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDeviceRegister_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblStatus;
                    lblStatus = (Label)e.Row.FindControl("lblStatus");
                    ImageButton imgBtnApproval;
                    imgBtnApproval = (ImageButton)e.Row.FindControl("imgBtnApproval");
                    ImageButton imgBtnReject;
                    ImageButton imgDeactive;
                    imgDeactive = (ImageButton)e.Row.FindControl("imgDeactive");
                    if (lblStatus.Text == "APPROVED")
                    {
                        imgBtnReject = (ImageButton)e.Row.FindControl("imgBtnReject");
                        imgBtnReject.Enabled = true;
                        imgBtnReject.Visible = true;
                        imgBtnApproval.ToolTip = "User has been approved";
                    }
                    else if (lblStatus.Text == "DISABLED")
                    {
                        imgDeactive = (ImageButton)e.Row.FindControl("imgDeactive");
                        //imgDeactive.Enabled = true; //commented on 15-05-2023
                        imgDeactive.Visible = true;                        
                        //imgDeactive.ToolTip = "User has been Disabled"; //commented on 15-05-2023
                        imgDeactive.ToolTip = "Click here to Enabel the Device.";
                    }
                    else
                    {
                        imgBtnApproval = (ImageButton)e.Row.FindControl("imgBtnApproval");
                        imgBtnApproval.Enabled = true;
                        imgBtnApproval.Visible = true;
                        imgBtnApproval.ToolTip = "Pending for Approval";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }
        /// <summary>
        /// Export click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Export_clickDeviceRegister(object sender, EventArgs e)
        {           
            DataTable dtDeviceDetails = (DataTable)ViewState["Device"];
            if (dtDeviceDetails.Rows.Count > 0)
            {
                dtDeviceDetails.Columns["MR_DEVICE_ID"].ColumnName = "DEVICE ID";
                dtDeviceDetails.Columns["US_FULL_NAME"].ColumnName = "NAME";
                dtDeviceDetails.Columns["MR_APPROVE_STATUS"].ColumnName = "APPROVAL STATUS";
                dtDeviceDetails.Columns["MR_CRON"].ColumnName = "CREATED ON";
                List<string> listtoRemove = new List<string> { "MR_REQUEST_BY", "MR_ID", "US_ID" };
                string filename = "DeviceRegisterDetails" + DateTime.Now + ".xls";
                string pagetitle = "Device Register Details";
                Genaral.getexcel(dtDeviceDetails, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");
                DataTable dtDevDetails = new DataTable();
                DataRow newRow = dtDevDetails.NewRow();
                dtDevDetails.Rows.Add(newRow);
                dtDevDetails.Columns.Add("US_ID");
                dtDevDetails.Columns.Add("MR_REQUEST_BY");
                dtDevDetails.Columns.Add("MR_ID");
                dtDevDetails.Columns.Add("MR_DEVICE_ID");
                dtDevDetails.Columns.Add("US_FULL_NAME");
                dtDevDetails.Columns.Add("MR_APPROVE_STATUS");
                dtDevDetails.Columns.Add("MR_CRON");
                grdDeviceRegister.DataSource = dtDevDetails;
                grdDeviceRegister.DataBind();
                int iColCount = grdDeviceRegister.Rows[0].Cells.Count;
                grdDeviceRegister.Rows[0].Cells.Clear();
                grdDeviceRegister.Rows[0].Cells.Add(new TableCell());
                grdDeviceRegister.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdDeviceRegister.Rows[0].Cells[0].Text = "No Records Found";
            }
        }
        /// <summary>
        /// View click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoadView_clickDeviceRegister(object sender, EventArgs e)
        {
            LoadDeviceRegistrerDetails("", "");
        }
        /// <summary>
        /// Pending click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoadPending_clickDeviceRegister(object sender, EventArgs e)
        {
            try
            {
                clsDeviceRegister objDevice = new clsDeviceRegister();
                DataTable dtDeviceDetails = new DataTable();
                objDevice.sDeviceId = "";
                objDevice.sFullName = "";
                objDevice.soffcode = objSession.OfficeCode;
                objDevice.sUserId = objSession.UserId;
                objDevice.DeviceStatus = "P";
                hidbuttonVal.Text = objDevice.DeviceStatus;
                //dtDeviceDetails = objDevice.LoadpendingDeviceGrid(objDevice);
                dtDeviceDetails = objDevice.LoadDeviceGrid(objDevice);
                if (dtDeviceDetails.Rows.Count <= 0)
                {
                    DataTable dtDevDetails = new DataTable();
                    DataRow newRow = dtDevDetails.NewRow();
                    dtDevDetails.Rows.Add(newRow);
                    dtDevDetails.Columns.Add("US_ID");
                    dtDevDetails.Columns.Add("MR_REQUEST_BY");
                    dtDevDetails.Columns.Add("MR_ID");
                    dtDevDetails.Columns.Add("MR_DEVICE_ID");
                    dtDevDetails.Columns.Add("US_FULL_NAME");
                    dtDevDetails.Columns.Add("MR_APPROVE_STATUS");
                    dtDevDetails.Columns.Add("MR_CRON");
                    grdDeviceRegister.DataSource = dtDevDetails;
                    grdDeviceRegister.DataBind();
                    int iColCount = grdDeviceRegister.Rows[0].Cells.Count;
                    grdDeviceRegister.Rows[0].Cells.Clear();
                    grdDeviceRegister.Rows[0].Cells.Add(new TableCell());
                    grdDeviceRegister.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdDeviceRegister.Rows[0].Cells[0].Text = "No Records Found";
                    ViewState["Device"] = dtDeviceDetails;
                }
                else
                {
                    grdDeviceRegister.DataSource = dtDeviceDetails;
                    grdDeviceRegister.DataBind();
                    ViewState["Device"] = dtDeviceDetails;
                }

                btnPendingRecords.BackColor = System.Drawing.Color.Gray;

                //if (btnPendingRecords.BackColor == System.Drawing.Color.Empty)
                //{
                //    btnPendingRecords.BackColor = System.Drawing.Color.Gray;
                //}
                //else if (btnPendingRecords.BackColor == System.Drawing.Color.Gray)
                //{
                //    btnPendingRecords.BackColor = System.Drawing.Color.Empty;
                //}
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }
        /// <summary>
        /// Approval click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoadApproval_clickDeviceRegister(object sender, EventArgs e)
        {
            try
            {
                clsDeviceRegister objDevice = new clsDeviceRegister();
                DataTable dtDeviceDetails = new DataTable();
                objDevice.sDeviceId = "";
                objDevice.sFullName = "";
                objDevice.soffcode = objSession.OfficeCode;
                objDevice.sUserId = objSession.UserId;
                objDevice.DeviceStatus = "A";
                hidbuttonVal.Text = objDevice.DeviceStatus;
                //dtDeviceDetails = objDevice.LoadApprovedDeviceGrid(objDevice);
                dtDeviceDetails = objDevice.LoadDeviceGrid(objDevice);
                if (dtDeviceDetails.Rows.Count <= 0)
                {
                    DataTable dtDevDetails = new DataTable();
                    DataRow newRow = dtDevDetails.NewRow();
                    dtDevDetails.Rows.Add(newRow);
                    dtDevDetails.Columns.Add("US_ID");
                    dtDevDetails.Columns.Add("MR_REQUEST_BY");
                    dtDevDetails.Columns.Add("MR_ID");
                    dtDevDetails.Columns.Add("MR_DEVICE_ID");
                    dtDevDetails.Columns.Add("US_FULL_NAME");
                    dtDevDetails.Columns.Add("MR_APPROVE_STATUS");
                    dtDevDetails.Columns.Add("MR_CRON");
                    grdDeviceRegister.DataSource = dtDevDetails;
                    grdDeviceRegister.DataBind();
                    int iColCount = grdDeviceRegister.Rows[0].Cells.Count;
                    grdDeviceRegister.Rows[0].Cells.Clear();
                    grdDeviceRegister.Rows[0].Cells.Add(new TableCell());
                    grdDeviceRegister.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdDeviceRegister.Rows[0].Cells[0].Text = "No Records Found";
                    ViewState["Device"] = dtDeviceDetails;
                }
                else
                {
                    grdDeviceRegister.DataSource = dtDeviceDetails;
                    grdDeviceRegister.DataBind();
                    ViewState["Device"] = dtDeviceDetails;
                }
                btnApprovedRecords.BackColor = System.Drawing.Color.Gray;
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }
        /// <summary>
        /// Disabled click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoadDisabled_clickDeviceRegister(object sender, EventArgs e)
        {
            try
            {
                clsDeviceRegister objDevice = new clsDeviceRegister();
                DataTable dtDeviceDetails = new DataTable();
                objDevice.sDeviceId = "";
                objDevice.sFullName = "";
                objDevice.soffcode = objSession.OfficeCode;
                objDevice.sUserId = objSession.UserId;
                objDevice.DeviceStatus = "D";
                hidbuttonVal.Text = objDevice.DeviceStatus;
                //dtDeviceDetails = objDevice.LoadDisabledDeviceGrid(objDevice);
                dtDeviceDetails = objDevice.LoadDeviceGrid(objDevice);
                if (dtDeviceDetails.Rows.Count <= 0)
                {
                    DataTable dtDevDetails = new DataTable();
                    DataRow newRow = dtDevDetails.NewRow();
                    dtDevDetails.Rows.Add(newRow);
                    dtDevDetails.Columns.Add("US_ID");
                    dtDevDetails.Columns.Add("MR_REQUEST_BY");
                    dtDevDetails.Columns.Add("MR_ID");
                    dtDevDetails.Columns.Add("MR_DEVICE_ID");
                    dtDevDetails.Columns.Add("US_FULL_NAME");
                    dtDevDetails.Columns.Add("MR_APPROVE_STATUS");
                    dtDevDetails.Columns.Add("MR_CRON");
                    grdDeviceRegister.DataSource = dtDevDetails;
                    grdDeviceRegister.DataBind();
                    int iColCount = grdDeviceRegister.Rows[0].Cells.Count;
                    grdDeviceRegister.Rows[0].Cells.Clear();
                    grdDeviceRegister.Rows[0].Cells.Add(new TableCell());
                    grdDeviceRegister.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdDeviceRegister.Rows[0].Cells[0].Text = "No Records Found";
                    ViewState["Device"] = dtDeviceDetails;
                }
                else
                {
                    grdDeviceRegister.DataSource = dtDeviceDetails;
                    grdDeviceRegister.DataBind();
                    ViewState["Device"] = dtDeviceDetails;
                }
                btnDesabeldRecords.BackColor = System.Drawing.Color.Gray;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }
        #region Access Rights
        /// <summary>
        /// Admin Access
        /// </summary>
        public void AdminAccess()
        {
            try
            {
                if (objSession.RoleId != "8")
                {
                    Response.Redirect("~/UserRestrict.aspx", false);
                }
                //if (objSession.RoleId != "11")
                //{
                //    Response.Redirect("~/UserRestrict.aspx", false);
                //}
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }
        #endregion
    }
}