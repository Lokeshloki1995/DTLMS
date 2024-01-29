using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IIITS.DTLMS.BL;

namespace IIITS.DTLMS.PermanentDecomm
{
    public partial class PermanentDecommView : System.Web.UI.Page
    {
        clsSession objSession;
        string strFormCode = "PermanentDecommView";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }

                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;
                if (!IsPostBack)
                {
                    if (rdbAlready.Checked == true)
                    {
                        LoadDecommAlreadyCreated("1");
                    }
                    else
                    {
                        LoadAllDecomm("1");
                    }

                    CheckAccessRights("4");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void Export_ClickDecomm(object sender, EventArgs e)
        {
           
            
            DataTable dt = (DataTable)ViewState["Decomm"];

            if (dt.Rows.Count > 0)
            {

                dt.Columns["DT_CODE"].ColumnName = "Transformer Centre Code";
                dt.Columns["DT_TC_ID"].ColumnName = "DTR Code";
                //dt.Columns["PTI_INDENT_NO"].ColumnName = "Indent No";
                //dt.Columns["PTI_INDENT_NO"].ColumnName = "Invoice No";
                dt.Columns["PWO_NO_DECOM"].ColumnName = "WorkOrder No";
                //dt.Columns["PWO_SLNO"].ColumnName = "Invoice No";


                List<string> listtoRemove = new List<string> { "PTR_ID", "PEST_ID", "STATUS", "TC_SLNO" };
                string filename = "PermanentDecomm" + DateTime.Now + ".xls";
                string pagetitle = "Permanent Decommissioning Details View";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");
                ShowEmptyGrid();
            }



        }

        protected void grdReplacementDetails_RowCommand(object sender, GridViewCommandEventArgs e)
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

                    Label lblFailureId = (Label)row.FindControl("lblFailureId");
                    Label lblReplaceId = (Label)row.FindControl("lblReplaceId");

                    string sReferId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblFailureId.Text));
                    string sType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(cmbType.SelectedValue));
                    string sReplaceId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblReplaceId.Text));

                    Response.Redirect("PermanentDecomm.aspx?ReferID=" + sReferId + "&TypeValue=" + sType + "&ReplaceId=" + sReplaceId, false);


                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtcCode = (TextBox)row.FindControl("txtDtcCode");
                    TextBox txtDtrCode = (TextBox)row.FindControl("txtDtrCode");


                    DataTable dt = (DataTable)ViewState["Decomm"];
                    dv = dt.DefaultView;
                    if (txtDtcCode.Text != "")
                    {
                        sFilter = "DT_CODE Like '%" + txtDtcCode.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtDtrCode.Text != "")
                    {
                        sFilter += " DT_TC_ID Like '%" + txtDtrCode.Text.Replace("'", "`") + "%' AND";
                    }


                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdReplacementDetails.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdReplacementDetails.DataSource = dv;
                            ViewState["Decomm"] = dv.ToTable();
                            grdReplacementDetails.DataBind();

                        }
                        else
                        {
                            ViewState["Decomm"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        if (rdbAlready.Checked == true)
                        {

                            LoadDecommAlreadyCreated(cmbType.SelectedValue);
                        }
                        else if (rdbViewAll.Checked == true)
                        {
                            LoadAllDecomm(cmbType.SelectedValue);
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


        public void LoadDecommAlreadyCreated(string sType)
        {

            try
            {
                clsPermanentDecomm objDecomm = new clsPermanentDecomm();

                objDecomm.sTaskType = sType;
                objDecomm.sOfficeCode = objSession.OfficeCode;

                DataTable dt = objDecomm.LoadAlreadyDecomm(objDecomm);

                grdReplacementDetails.DataSource = dt;
                grdReplacementDetails.DataBind();
                ViewState["Decomm"] = dt;
                if (sType == "1")
                {
                    lblGridType.Text = "Transformer Centre Permanent Decommissioning Details :";
                }
                else if (sType == "2")
                {
                    lblGridType.Text = "Transformer Centre Permanent Decommissioning Details :";
                }
                else if (sType == "4")
                {
                    lblGridType.Text = "Transformer Centre Permanent Decommissioning Details :";
                }

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadAllDecomm(string sType)
        {

            try
            {
                clsPermanentDecomm objDecomm = new clsPermanentDecomm();
                string sMsg = string.Empty;

                objDecomm.sTaskType = sType;
                objDecomm.sOfficeCode = objSession.OfficeCode;

                DataTable dt = objDecomm.LoadCreateDecommission(objDecomm);

                //To show the Type of Gridview
                if (sType == "1")
                {
                    //Gridview column visible true/false based on conditions
                    grdReplacementDetails.Columns[3].Visible = true;
                    grdReplacementDetails.Columns[4].Visible = false;

                    lblGridType.Text = "Transformer Centre Permanent Decommissioning Details :";
                    sMsg = "Failure";
                }
                else if (sType == "2")
                {

                    //Gridview column visible true/false based on conditions
                    grdReplacementDetails.Columns[3].Visible = false;
                    grdReplacementDetails.Columns[4].Visible = true;

                    lblGridType.Text = "Transformer Centre Permanent Decommissioning Details :";
                    sMsg = "Enhancement";
                }
                else if (sType == "4")
                {

                    //Gridview column visible true/false based on conditions
                    grdReplacementDetails.Columns[3].Visible = false;
                    grdReplacementDetails.Columns[4].Visible = true;

                    lblGridType.Text = "Transformer Centre Permanent Decommissioning Details :";
                    sMsg = "Enhancement";
                }

                if (dt.Rows.Count > 0)
                {
                    grdReplacementDetails.DataSource = dt;
                    grdReplacementDetails.DataBind();
                    ViewState["Decomm"] = dt;
                }
                else
                {
                    lblMessage.Text = "Note : No " + sMsg + " Transformer Centre Available Please Declare the Transformer Centre " + sMsg + " before Decommissioning";
                    grdReplacementDetails.DataSource = dt;
                    grdReplacementDetails.DataBind();
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
                    LoadDecommAlreadyCreated(cmbType.SelectedValue);
                }
                else
                {
                    LoadNewDTCDecomm();
                }


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
                    LoadAllDecomm(cmbType.SelectedValue);
                }
                else
                {
                    LoadNewDTCDecomm();
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
                   // grdNewDTCReplace.Visible = false;
                    grdReplacementDetails.Visible = true;

                    //Temp
                    cmdNew.Visible = false;

                    if (rdbViewAll.Checked == true)
                    {
                        rdbViewAll_CheckedChanged(sender, e);
                    }
                    else
                    {
                        rdbAlready_CheckedChanged(sender, e);
                    }
                }
                else if (cmbType.SelectedValue == "2")
                {
                    //grdNewDTCReplace.Visible = false;
                    grdReplacementDetails.Visible = true;

                    //Temp
                    cmdNew.Visible = false;

                    if (rdbViewAll.Checked == true)
                    {
                        rdbViewAll_CheckedChanged(sender, e);
                    }
                    else
                    {
                        rdbAlready_CheckedChanged(sender, e);
                    }
                }
                else
                {
                   // grdNewDTCReplace.Visible = true;
                    grdReplacementDetails.Visible = false;
                    cmdNew.Visible = false;

                    rdbAlready.Checked = true;
                    rdbViewAll.Checked = false;

                    if (rdbViewAll.Checked == true)
                    {
                        rdbViewAll_CheckedChanged(sender, e);
                    }
                    else
                    {
                        //rdbAlready_CheckedChanged(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdReplacementDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    LinkButton lnkUpdate = (LinkButton)e.Row.FindControl("lnkUpdate");
                    LinkButton lnkCreate = (LinkButton)e.Row.FindControl("lnkCreate");
                    Label lblStatus = (Label)e.Row.FindControl("lblStatus");

                        lnkUpdate.Visible = true;
                        lnkCreate.Visible = false;
                  
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
                Response.Redirect("PermanentDecomm.aspx?TypeValue=" + sType, false);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdReplacementDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdReplacementDetails.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Decomm"];
                grdReplacementDetails.DataSource = SortDataTable(dt as DataTable, true);
                grdReplacementDetails.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdReplacementDetails_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdReplacementDetails.PageIndex;
            DataTable dt = (DataTable)ViewState["Decomm"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdReplacementDetails.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdReplacementDetails.DataSource = dt;
            }
            grdReplacementDetails.DataBind();
            grdReplacementDetails.PageIndex = pageIndex;
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
                        ViewState["Decomm"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["Decomm"] = dataView.ToTable();

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

        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "PermanentDecomm";
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
                dt.Columns.Add("PTR_ID");
                dt.Columns.Add("DT_CODE");
                dt.Columns.Add("DT_TC_ID");
                dt.Columns.Add("PEST_ID");
                dt.Columns.Add("PWO_NO_DECOM");
               // dt.Columns.Add("PTI_INDENT_NO");
                dt.Columns.Add("STATUS");

                grdReplacementDetails.DataSource = dt;
                grdReplacementDetails.DataBind();

                int iColCount = grdReplacementDetails.Rows[0].Cells.Count;
                grdReplacementDetails.Rows[0].Cells.Clear();
                grdReplacementDetails.Rows[0].Cells.Add(new TableCell());
                grdReplacementDetails.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdReplacementDetails.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }

        #region NewDTCInvoice
        public void LoadNewDTCDecomm()
        {

            try
            {
                clsPermanentDecomm objDecomm = new clsPermanentDecomm();

                objDecomm.sOfficeCode = objSession.OfficeCode;

                DataTable dt = objDecomm.LoadCreateNewDTCDecommission(objDecomm);
               // grdNewDTCReplace.DataSource = dt;
              //  grdNewDTCReplace.DataBind();
                ViewState["NewDTCDecomm"] = dt;

                lblGridType.Text = "New Transformer Centre Commission Decommissioning Details :";


            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdNewDTCReplace_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
               // grdNewDTCReplace.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["NewDTCDecomm"];
              //  grdNewDTCReplace.DataSource = dt;
              //  grdNewDTCReplace.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdNewDTCReplace_RowCommand(object sender, GridViewCommandEventArgs e)
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
                    Label lblTCCode = (Label)row.FindControl("lblTcCode1");
                    Label lblWOSlno = (Label)row.FindControl("lblWOSlno1");

                    string sWOslno = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblWOSlno.Text));
                    string sTCCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblTCCode.Text));


                    Response.Redirect("/MasterForms/DTCCommision.aspx?WOSlno=" + sWOslno + "&TCCode=" + sTCCode, false);

                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtWoNo = (TextBox)row.FindControl("txtWoNo");
                    //TextBox txtIndentno = (TextBox)row.FindControl("txtIndentno");
                    //TextBox txtInvoiceNo = (TextBox)row.FindControl("txtInvoiceNo");

                    DataTable dt = (DataTable)ViewState["NewDTCDecomm"];
                    dv = dt.DefaultView;
                    if (txtWoNo.Text != "")
                    {
                        sFilter = "PWO_NO_DECOM Like '%" + txtWoNo.Text.Replace("'", "`") + "%' AND";
                    }
                    //if (txtIndentno.Text != "")
                    //{
                    //    sFilter = "PWO_SLNO Like '%" + txtIndentno.Text.Replace("'", "`") + "%' AND";
                    //}
                    //if (txtInvoiceNo.Text != "")
                    //{
                    //    sFilter = "PTI_INDENT_NO Like '%" + txtInvoiceNo.Text.Replace("'", "`") + "%' AND";
                    //}
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                       // grdNewDTCReplace.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                           // grdNewDTCReplace.DataSource = dv;
                            ViewState["NewDTCDecomm"] = dv.ToTable();
                           // grdNewDTCReplace.DataBind();

                        }
                        else
                        {

                            ShowEmptyGridForNewDtc();
                        }
                    }
                    else
                    {
                        LoadNewDTCDecomm();
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
                dt.Columns.Add("TC_CODE");
                dt.Columns.Add("PWO_SLNO");
               // dt.Columns.Add("IN_NO");
                dt.Columns.Add("PWO_NO_DECOM");
                //dt.Columns.Add("PTI_INDENT_NO");
               // dt.Columns.Add("PTI_INDENT_NO");
                //dt.Columns.Add("PTI_DATE");
                dt.Columns.Add("STATUS");


              //  grdNewDTCReplace.DataSource = dt;
              //  grdNewDTCReplace.DataBind();

              //  int iColCount = grdNewDTCReplace.Rows[0].Cells.Count;
              //  grdNewDTCReplace.Rows[0].Cells.Clear();
               // grdNewDTCReplace.Rows[0].Cells.Add(new TableCell());
              //  grdNewDTCReplace.Rows[0].Cells[0].ColumnSpan = iColCount;
              //  grdNewDTCReplace.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        #endregion

        protected void grdNewDTCReplace_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton lnkCreate1 = (LinkButton)e.Row.FindControl("lnkCreate1");

                    if (cmbType.SelectedValue == "3")
                    {
                        lnkCreate1.Text = "Transformer Centre Commission";
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