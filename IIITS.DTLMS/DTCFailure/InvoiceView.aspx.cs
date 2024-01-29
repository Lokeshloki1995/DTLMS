using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Data.OleDb;

namespace IIITS.DTLMS.DTCFailure
{
    public partial class InvoiceView : System.Web.UI.Page
    {
        string strFormCode = "InvoiceView";
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
                        if (rdbAlready.Checked == true)
                        {
                            LoadExistingInvoice("1");
                        }
                        else
                        {
                            LoadAllInvoices("1");
                        }

                        CheckAccessRights("4");
                    }
                }
                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void Export_ClickInvoice(object sender, EventArgs e)
        {
            //clsInvoice objInvoice = new clsInvoice();
            //string sType = "";




            //if (cmbType.SelectedValue == "1")
            //{
            //    sType = "1";
            //}
            //if (cmbType.SelectedValue == "2")
            //{
            //    sType = "2";
            //}
            //if (cmbType.SelectedValue == "4")
            //{
            //    sType = "4";
            //}


            //objInvoice.sTaskType = sType;
            //objInvoice.sOfficeCode = objSession.OfficeCode;

            //DataTable dt = objInvoice.LoadExistingInvoice(objInvoice);

            DataTable dt = (DataTable)ViewState["Invoice"];

            if (dt.Rows.Count > 0)
            {

                dt.Columns["DT_NAME"].ColumnName = "Transformer Centre Name";
                dt.Columns["TC_CODE"].ColumnName = "DTR Code";
                dt.Columns["WO_NO"].ColumnName = "Work Order No.";
                dt.Columns["TI_INDENT_NO"].ColumnName = "Indent NO";
                dt.Columns["IN_INV_NO"].ColumnName = "Invoice No.";

                dt.Columns["Work Order No."].SetOrdinal(3);
                dt.Columns["Indent NO"].SetOrdinal(4);
                dt.Columns["Invoice No."].SetOrdinal(5);

                List<string> listtoRemove = new List<string> { "IN_NO", "TI_ID", "STATUS"};
                string filename = "Invoice" + DateTime.Now + ".xls";
                string pagetitle="Invoice View";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");

                ShowEmptyGrid();
            }



        }
      

        protected void grdInvoice_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "Create" || e.CommandName == "CreateNew")
                {
                    if (e.CommandName == "CreateNew")
                    {
                        //Check AccessRights
                        bool bAccResult = CheckAccessRights("2");
                        if (bAccResult == false)
                        {
                            return;
                        }
                    }

                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    Label lblIndentId = (Label)row.FindControl("lblIndentId");
                    Label lblInvoiceId = (Label)row.FindControl("lblInvoiceId");

                    string sReferId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblIndentId.Text));
                    string sInvoiceId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblInvoiceId.Text));
                    string sType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(cmbType.SelectedValue));

                    Response.Redirect("InvoiceCreation.aspx?ReferID=" + sReferId + "&TypeValue=" + sType + "&InvoiceId=" + sInvoiceId, false);
                   
                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtcName = (TextBox)row.FindControl("txtDtcName");
                    TextBox txtDtrCode = (TextBox)row.FindControl("txtDtrCode");
                    TextBox txtWoNo = (TextBox)row.FindControl("txtWoNo");
                    TextBox txtIndentNo = (TextBox)row.FindControl("txtIndentNo");
                    ;

                    DataTable dt = (DataTable)ViewState["Invoice"];
                    dv = dt.DefaultView;
                    if (txtDtcName.Text != "")
                    {
                        sFilter = "DT_NAME Like '%" + txtDtcName.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtDtrCode.Text != "")
                    {
                        sFilter = "TC_CODE Like '%" + txtDtrCode.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtWoNo.Text != "")
                    {
                        sFilter = "WO_NO Like '%" + txtWoNo.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtIndentNo.Text != "")
                    {
                        sFilter += " TI_INDENT_NO Like '%" + txtIndentNo.Text.Replace("'", "`") + "%' AND";
                    }

                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdInvoice.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdInvoice.DataSource = dv;
                            ViewState["Invoice"] = dv.ToTable();
                            grdInvoice.DataBind();

                        }
                        else
                        {
                            ViewState["Invoice"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        if (rdbAlready.Checked == true)
                        {

                            LoadExistingInvoice(cmbType.SelectedValue);
                        }
                        else if (rdbViewAll.Checked == true)
                        {
                            LoadAllInvoices(cmbType.SelectedValue);
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

                string sType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(cmbType.SelectedValue));
                Response.Redirect("InvoiceCreation.aspx?TypeValue=" + sType, false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void rdbViewAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbType.SelectedValue != "3")
                {
                    LoadAllInvoices(cmbType.SelectedValue);
                }
                else
                {
                    LoadNewDTCAllInvoice();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void rdbAlready_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbType.SelectedValue != "3")
                {
                    LoadExistingInvoice(cmbType.SelectedValue);
                }
                else
                {
                    LoadNewDTCInvoiceAlreadyCreated();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbType.SelectedValue == "1")
                {
                    grdNewDTCInvoice.Visible = false;
                    grdInvoice.Visible = true;
                   
                    //Temp
                    rdbAlready.Checked = true;
                    rdbViewAll.Checked = false;
                    cmbExport.Visible = true;

                    if (rdbViewAll.Checked == true)
                    {
                        rdbViewAll_CheckedChanged(sender, e);
                    }
                    else
                    {
                        rdbAlready_CheckedChanged(sender, e);
                    }
                }
                 if (cmbType.SelectedValue == "2")
                {
                    grdNewDTCInvoice.Visible = false ;
                    grdInvoice.Visible = true;
                    cmbExport.Visible = true;
                    //Temp
                    rdbAlready.Checked = true;
                    rdbViewAll.Checked = false;
                    

                    if (rdbViewAll.Checked == true)
                    {
                        rdbViewAll_CheckedChanged(sender, e);
                    }
                    else
                    {
                        rdbAlready_CheckedChanged(sender, e);
                    }
                }
                if (cmbType.SelectedValue == "4")
                {
                    grdNewDTCInvoice.Visible = false;
                    grdInvoice.Visible = true;
                   
                    //Temp
                    rdbAlready.Checked = true;
                    rdbViewAll.Checked = false;
                    cmbExport.Visible = true;

                    if (rdbViewAll.Checked == true)
                    {
                        rdbViewAll_CheckedChanged(sender, e);
                    }
                    else
                    {
                        rdbAlready_CheckedChanged(sender, e);
                    }
                }
                if (cmbType.SelectedValue == "3")
                {
                    grdNewDTCInvoice.Visible = true;
                    grdInvoice.Visible = false;
                    cmbExport.Visible = false;
                    if (rdbViewAll.Checked == true)
                    {
                        rdbViewAll_CheckedChanged(sender, e);
                    }
                    else
                    {
                        rdbAlready_CheckedChanged(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadExistingInvoice(string sType)
        {
            try
            {
                clsInvoice objInvoice = new clsInvoice();
                objInvoice.sTaskType = sType;
               

                if (objSession.sRoleType == "2")
                {
                    string sLocCode = clsStoreOffice.Getofficecode(objSession.OfficeCode);

                    objInvoice.sOfficeCode = sLocCode;
                                       
                }
                else
                {
                    objInvoice.sOfficeCode = objSession.OfficeCode;
                }


                DataTable dt = objInvoice.LoadExistingInvoice(objInvoice);
                grdInvoice.DataSource = dt;
                grdInvoice.DataBind();
                ViewState["Invoice"] = dt;
                if (sType == "1")
                {
                    lblGridType.Text = "Transformer Centre Failure Invoice Details";
                }
                else if (sType == "2")
                {
                    lblGridType.Text = "Transformer Centre Enhancement Invoice Details";
                }
                else if (sType == "4")
                {
                    lblGridType.Text = "Transformer Centre Failure with Enhancement Invoice Details";
                }

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadAllInvoices(string sType)
        {

            try
            {
                clsInvoice objInvoice = new clsInvoice();
                string sMsg = string.Empty;

                objInvoice.sTaskType = sType;
                objInvoice.sOfficeCode = objSession.OfficeCode;
                DataTable dt = objInvoice.LoadAllInvoiceDetails(objInvoice);
                //To show the Type of Gridview
                if (sType == "1")
                {                   
                    lblGridType.Text = "Transformer Centre Failure Invoice Details :";
                    sMsg = "Failure";
                }
                else if (sType == "2")
                {                   
                    lblGridType.Text = "Transformer Centre Enhancement Invoice Details :";
                    sMsg = "Enhancement";
                }
                else if (sType == "4")
                {
                    lblGridType.Text = "Transformer Centre Failure with Enhancement Invoice Details :";
                    sMsg = "Enhancement";
                }

                if (dt.Rows.Count > 0)
                {
                    grdInvoice.DataSource = dt;
                    grdInvoice.DataBind();
                    ViewState["Invoice"] = dt;
                    lblMessage.Text = "";
                }
                else
                {
                    lblMessage.Text = "Note : No " + sMsg + " Transformer Centre Available Please Declare the Transformer Centre  " + sMsg + " before creating a Invoice";
                    grdInvoice.DataSource = dt;
                    grdInvoice.DataBind();
                }


            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdInvoice_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdInvoice.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Invoice"];
                grdInvoice.DataSource = SortDataTable(dt as DataTable, true);
                grdInvoice.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdInvoice_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdInvoice.PageIndex;
            DataTable dt = (DataTable)ViewState["Invoice"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdInvoice.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdInvoice.DataSource = dt;
            }
            grdInvoice.DataBind();
            grdInvoice.PageIndex = pageIndex;
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
                        ViewState["Invoice"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["Invoice"] = dataView.ToTable();

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

        protected void grdInvoice_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    LinkButton lnkUpdate = (LinkButton)e.Row.FindControl("lnkUpdate");
                    LinkButton lnkCreate = (LinkButton)e.Row.FindControl("lnkCreate");
                    Label lblStatus = (Label)e.Row.FindControl("lblStatus");


                    if (lblStatus.Text == "YES")
                    {
                        lnkUpdate.Visible = true;
                        lnkCreate.Visible = false;
                    }
                    else
                    {
                        lnkUpdate.Visible = false;
                        lnkCreate.Visible = true;
                    }
                }
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

                objApproval.sFormName = "InvoiceCreation";
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
                String sShowMsg = string.Empty;
                sShowMsg = "<script language=javascript> alert ('" + sMsg + "')</script>";
                this.Page.RegisterStartupScript("Msg", sShowMsg);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("IN_NO");
                dt.Columns.Add("TI_ID");
                dt.Columns.Add("DT_NAME");
                dt.Columns.Add("TC_CODE");
                dt.Columns.Add("WO_NO");
                dt.Columns.Add("TI_INDENT_NO");
                dt.Columns.Add("STATUS");
                dt.Columns.Add("IN_INV_NO");

                grdInvoice.DataSource = dt;
                grdInvoice.DataBind();

                int iColCount = grdInvoice.Rows[0].Cells.Count;
                grdInvoice.Rows[0].Cells.Clear();
                grdInvoice.Rows[0].Cells.Add(new TableCell());
                grdInvoice.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdInvoice.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }

        #region NewDTCInvoice
        public void LoadNewDTCInvoiceAlreadyCreated()
        {

            try
            {
                clsInvoice objInvoice = new clsInvoice();

                objInvoice.sOfficeCode = objSession.OfficeCode;

                DataTable dt = objInvoice.LoadAlreadyNewDTCInvoice(objInvoice);
                grdNewDTCInvoice.DataSource = dt;
                grdNewDTCInvoice.DataBind();
                ViewState["NewDTCInvoice"] = dt;

                lblGridType.Text = "New Transformer Centre Commission Invoice Details :";


            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadNewDTCAllInvoice()
        {

            try
            {
                clsInvoice objInvoice = new clsInvoice();
                string sMsg = string.Empty;

                objInvoice.sOfficeCode = objSession.OfficeCode;
                DataTable dt = objInvoice.LoadAllNewDTCInvoice(objInvoice);

                lblGridType.Text = "New Transformer Centre Commission Invoice Details :";
                sMsg = "New Transformer Centre Commission ";
                if (dt.Rows.Count > 0)
                {
                    grdNewDTCInvoice.DataSource = dt;
                    grdNewDTCInvoice.DataBind();
                    ViewState["NewDTCInvoice"] = dt;
                }
                else
                {
                    lblMessage.Text = "Note : No " + sMsg + " Available Please Declare the  " + sMsg + " Indent before creating a Invoice";
                    grdNewDTCInvoice.DataSource = dt;
                    grdNewDTCInvoice.DataBind();
                }


            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void grdNewDTCInvoice_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdNewDTCInvoice.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["NewDTCInvoice"];
                grdNewDTCInvoice.DataSource = dt;
                grdNewDTCInvoice.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdNewDTCInvoice_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "Create" || e.CommandName == "CreateNew")
                {
                    if (e.CommandName == "CreateNew")
                    {
                        //Check AccessRights
                        bool bAccResult = CheckAccessRights("2");
                        if (bAccResult == false)
                        {
                            return;
                        }
                    }

                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    Label lblIndentId = (Label)row.FindControl("lblIndentId1");
                    Label lblInvoiceId = (Label)row.FindControl("lblInvoiceId1");

                    string sReferId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblIndentId.Text));
                    string sInvoiceId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblInvoiceId.Text));
                    string sType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(cmbType.SelectedValue));

                    Response.Redirect("InvoiceCreation.aspx?ReferID=" + sReferId + "&TypeValue=" + sType + "&InvoiceId=" + sInvoiceId, false);

                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtWoNo = (TextBox)row.FindControl("txtWoNo");
                    TextBox txtIndentno = (TextBox)row.FindControl("txtIndentno");

                    DataTable dt = (DataTable)ViewState["NewDTCInvoice"];
                    dv = dt.DefaultView;
                    if (txtWoNo.Text != "")
                    {
                        sFilter = "WO_NO Like '%" + txtWoNo.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtIndentno.Text != "")
                    {
                        sFilter = "TI_INDENT_NO Like '%" + txtIndentno.Text.Replace("'", "`") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdNewDTCInvoice.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdNewDTCInvoice.DataSource = dv;
                            ViewState["NewDTCInvoice"] = dv.ToTable();
                            grdNewDTCInvoice.DataBind();

                        }
                        else
                        {

                            ShowEmptyGridForNewDtc();
                        }
                    }
                    else
                    {
                        if (rdbAlready.Checked == true)
                        {

                            LoadNewDTCInvoiceAlreadyCreated();
                        }
                        else if (rdbViewAll.Checked == true)
                        {
                            LoadNewDTCAllInvoice();
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

        protected void grdNewDTCInvoice_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    LinkButton lnkUpdate = (LinkButton)e.Row.FindControl("lnkUpdate1");
                    LinkButton lnkCreate = (LinkButton)e.Row.FindControl("lnkCreate1");
                    Label lblStatus = (Label)e.Row.FindControl("lblStatus1");


                    if (lblStatus.Text == "YES")
                    {
                        lnkUpdate.Visible = true;
                        lnkCreate.Visible = false;
                    }
                    else
                    {
                        lnkUpdate.Visible = false;
                        lnkCreate.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void ShowEmptyGridForNewDtc()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("TI_ID");
                dt.Columns.Add("WO_SLNO");
                dt.Columns.Add("IN_NO");
                dt.Columns.Add("WO_NO");
                dt.Columns.Add("TI_INDENT_NO");
                dt.Columns.Add("TI_INDENT_DATE");
                dt.Columns.Add("STATUS");
                dt.Columns.Add("IN_INV_NO");
                dt.Columns.Add("IN_DATE");

                grdNewDTCInvoice.DataSource = dt;
                grdNewDTCInvoice.DataBind();

                int iColCount = grdNewDTCInvoice.Rows[0].Cells.Count;
                grdNewDTCInvoice.Rows[0].Cells.Clear();
                grdNewDTCInvoice.Rows[0].Cells.Add(new TableCell());
                grdNewDTCInvoice.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdNewDTCInvoice.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }
        #endregion

       
    }
}