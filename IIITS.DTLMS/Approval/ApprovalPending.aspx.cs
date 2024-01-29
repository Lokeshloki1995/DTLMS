using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.IO;
using System.Drawing;
using System.Configuration;

namespace IIITS.DTLMS.Approval
{
    public partial class ApprovalPending : System.Web.UI.Page
    {
        string strFormCode = "FailurePendingDetails";
        clsSession objSession;
        int Division;
        int SubDivision;
        int Section;
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

                    string stroffCode = string.Empty;

                    Division = Convert.ToInt32(ConfigurationManager.AppSettings["Division_code"]);
                    SubDivision = Convert.ToInt32(ConfigurationManager.AppSettings["SubDiv_code"]);
                    Section = Convert.ToInt32(ConfigurationManager.AppSettings["Section_code"]);

                    if (objSession.OfficeCode.Length > 2)
                    {
                        stroffCode = objSession.OfficeCode.Substring(0, Division);
                    }
                    else
                    {
                        stroffCode = objSession.OfficeCode;
                    }

                    if (!IsPostBack)
                    {

                        if (Request.QueryString["OfficeCode"] != null && Convert.ToString(Request.QueryString["OfficeCode"]) != "")
                        {
                            hdfOffCode.Value = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["OfficeCode"]));
                        }
                        else
                        {
                            hdfOffCode.Value = objSession.OfficeCode;
                        }

                        Genaral.Load_Combo("SELECT \"BO_ID\",\"BO_NAME\" FROM \"TBLBUSINESSOBJECT\" WHERE \"BO_MO_ID\" ='2' AND \"BO_STATUS\"=1 ", "--Select--", cmbType);
                        if (stroffCode == null || stroffCode == "")
                        {

                            Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_CODE\" || '-' || \"DIV_NAME\" FROM \"TBLDIVISION\" ORDER BY \"DIV_CODE\"", "--All--", cmbDiv);
                        }

                        if (stroffCode.Length >= 3)
                        {
                            Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_CODE\" || '-' || \"DIV_NAME\" FROM \"TBLDIVISION\" ORDER BY \"DIV_CODE\"", "--All--", cmbDiv);
                            cmbDiv.Items.FindByValue(stroffCode).Selected = true;
                            stroffCode = string.Empty;
                            stroffCode = objSession.OfficeCode;
                        }

                        if (stroffCode.Length >= 3)
                        {
                            Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\", \"SD_SUBDIV_CODE\" || '-' || \"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE \"SD_DIV_CODE\" ='" + cmbDiv.SelectedValue + "' ORDER BY \"SD_SUBDIV_CODE\" ", "--All--", cmbSubDiv);
                            if (stroffCode.Length >= 4)
                            {
                                stroffCode = objSession.OfficeCode.Substring(0, SubDivision);
                                cmbSubDiv.Items.FindByValue(stroffCode).Selected = true;
                                stroffCode = string.Empty;
                                stroffCode = objSession.OfficeCode;
                            }
                        }
                        if (stroffCode.Length >= 4)
                        {
                            Genaral.Load_Combo("SELECT \"OM_CODE\", \"OM_CODE\" || '-' || \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_SUBDIV_CODE\" ='" + cmbSubDiv.SelectedValue + "' ORDER BY \"OM_CODE\"", "--All--", cmbSection);
                            if (stroffCode.Length >= 5)
                            {
                                stroffCode = objSession.OfficeCode.Substring(0, Section);
                                cmbSection.Items.FindByValue(stroffCode).Selected = true;
                            }

                        }

                        LoadApprovalPendingDetails();
                    }
                }
                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadApprovalPendingDetails()
        {
            try
            {
                clsDashboard objDashboard = new clsDashboard();
                DataTable dtLoadDetails = new DataTable();
                string sBOType = string.Empty;
                string sOfficeCode = string.Empty;

                string RoleType = objSession.sRoleType;

                if (cmbDiv.SelectedIndex > 0)
                {
                    sOfficeCode = cmbDiv.SelectedValue;
                }

                if (cmbSubDiv.SelectedIndex > 0)
                {
                    sOfficeCode = cmbSubDiv.SelectedValue;
                }

                if (cmbSection.SelectedIndex > 0)
                {
                    sOfficeCode = cmbSection.SelectedValue;
                }
                if (cmbType.SelectedIndex > 0)
                {
                    sBOType = cmbType.SelectedValue;
                }

                if (sOfficeCode != "")
                {
                    dtLoadDetails = objDashboard.LoadApprovalPendingDetails(sOfficeCode, sBOType, RoleType);
                }
                else
                {
                    if (cmbDiv.SelectedIndex == 0)
                    {
                        sOfficeCode = "";
                        dtLoadDetails = objDashboard.LoadApprovalPendingDetails(sOfficeCode, sBOType, RoleType);
                    }
                    else
                    dtLoadDetails = objDashboard.LoadApprovalPendingDetails(hdfOffCode.Value, sBOType, RoleType);
                }
                grdApprovePending.DataSource = dtLoadDetails;
                grdApprovePending.DataBind();
                ViewState["ApprovePending"] = dtLoadDetails;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdApprovePending_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    //GridView HeaderGrid = (GridView)sender;
                    //GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                    //TableCell HeaderCell = new TableCell();
                    //HeaderCell.Text = "";
                    //HeaderCell.ColumnSpan = 2;
                    //HeaderGridRow.Cells.Add(HeaderCell);
                    //HeaderCell = new TableCell();
                    //HeaderCell.Text = "Section";

                    //HeaderCell.ColumnSpan = 1;
                    //HeaderGridRow.Cells.Add(HeaderCell);
                    //HeaderCell = new TableCell();
                    //HeaderCell.Text = "Sub Division";

                    //HeaderCell.ColumnSpan = 1;
                    //HeaderGridRow.Cells.Add(HeaderCell);
                    //HeaderCell = new TableCell();
                    //HeaderCell.Text = "Failure";

                    //HeaderCell.ColumnSpan = 2;
                    //HeaderGridRow.Cells.Add(HeaderCell);
                    //HeaderCell = new TableCell();
                    //HeaderCell.Text = "Estimation";


                    //HeaderCell.ColumnSpan = 3;
                    //HeaderGridRow.Cells.Add(HeaderCell);
                    //HeaderCell = new TableCell();
                    //HeaderCell.Text = "Work Order";

                    //HeaderCell.ColumnSpan = 3;
                    //HeaderGridRow.Cells.Add(HeaderCell);
                    //HeaderCell = new TableCell();
                    //HeaderCell.Text = "Indent";

                    //HeaderCell.ColumnSpan = 3;
                    //HeaderGridRow.Cells.Add(HeaderCell);
                    //HeaderCell = new TableCell();
                    //HeaderCell.Text = "Commission";

                    //HeaderCell.ColumnSpan = 3;
                    //HeaderGridRow.Cells.Add(HeaderCell);
                    //HeaderCell = new TableCell();
                    //HeaderCell.Text = "Decommission / RI";

                    //HeaderCell.ColumnSpan = 3;
                    //HeaderGridRow.Cells.Add(HeaderCell);
                    //grdApprovePending.Controls[0].Controls.AddAt(0, HeaderGridRow);

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdApprovePending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdApprovePending.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["ApprovePending"];
                grdApprovePending.DataSource = SortDataTable(dtComplete as DataTable, true);
                grdApprovePending.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdApprovePending_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdApprovePending.PageIndex;
            DataTable dt = (DataTable)ViewState["ApprovePending"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdApprovePending.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdApprovePending.DataSource = dt;
            }
            grdApprovePending.DataBind();
            grdApprovePending.PageIndex = pageIndex;
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


        public void ExportToExcel(DataTable dtCompleteDetails)
        {
            try
            {
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Failure_Pending_Details.xls"));
                Response.ContentType = "application/ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                grdApprovePending.AllowPaging = false;
                LoadApprovalPendingDetails();
                //Change the Header Row back to white color
                grdApprovePending.HeaderRow.Style.Add("background-color", "#FFFFFF");
                //Applying stlye to gridview header cells
                for (int i = 0; i < grdApprovePending.HeaderRow.Cells.Count; i++)
                {
                    grdApprovePending.HeaderRow.Cells[i].Style.Add("background-color", "#709eea");
                }
                grdApprovePending.RenderControl(htw);
                Response.Write(Convert.ToString(sw));
                Response.End();
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

        protected void cmbExport_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                dtComplete = (DataTable)ViewState["ApprovePending"];
                ExportToExcel(dtComplete);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdApprovePending_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtCode = (TextBox)row.FindControl("txtDtCode");
                    TextBox txtDTCName = (TextBox)row.FindControl("txtDTCName");
                    TextBox txtDF_ID = (TextBox)row.FindControl("txtDF_ID");

                    DataTable dt = (DataTable)ViewState["ApprovePending"];
                    dv = dt.DefaultView;

                    if (txtDtCode.Text != "")
                    {
                        sFilter = "DT_CODE Like '%" + txtDtCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtDTCName.Text != "")
                    {
                        sFilter += " DT_NAME Like '%" + txtDTCName.Text.Replace("'", "'") + "%' AND";
                    }
                    if(txtDF_ID.Text != "")
                    {
                        ///sFilter += " DF_ID Like '%" + txtDF_ID.Text.Replace("'", "'") + "%' AND";
                        sFilter += " DF_ID ='"+ txtDF_ID.Text + "' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdApprovePending.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdApprovePending.DataSource = dv;
                            ViewState["ApprovePending"] = dv.ToTable();
                            grdApprovePending.DataBind();

                        }
                        else
                        {
                            ViewState["ApprovePending"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadApprovalPendingDetails();
                    }
                }
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
                dt.Columns.Add("DT_CODE");
                dt.Columns.Add("DT_NAME");
                dt.Columns.Add("OMSECTION");
                dt.Columns.Add("STATUS");
                dt.Columns.Add("DF_ID");


                grdApprovePending.DataSource = dt;
                grdApprovePending.DataBind();

                int iColCount = grdApprovePending.Rows[0].Cells.Count;
                grdApprovePending.Rows[0].Cells.Clear();
                grdApprovePending.Rows[0].Cells.Add(new TableCell());
                grdApprovePending.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdApprovePending.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }

        protected void cmbDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDiv.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\", \"SD_SUBDIV_CODE\" || '-' || \"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE \"SD_DIV_CODE\" ='" + cmbDiv.SelectedValue + "' ORDER BY \"SD_SUBDIV_CODE\"", "--All--", cmbSubDiv);
                    cmbSection.Items.Clear();
                }
                else
                {
                    cmbSubDiv.Items.Clear();
                    cmbSection.Items.Clear();
                   
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void cmbSubDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubDiv.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"OM_CODE\", \"OM_CODE\" || '-' || \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_SUBDIV_CODE\" ='" + cmbSubDiv.SelectedValue + "' ORDER BY \"OM_CODE\"", "--All--", cmbSection);
                }
                else
                {

                    cmbSection.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                LoadApprovalPendingDetails();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdApprovePending_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    Label lblStatus = (Label)e.Row.FindControl("lblFailStatus");
                    string status = lblStatus.Text;
                    if (status == "")
                    {
                        return;
                    }
                    if (status.Contains(':'))
                    {
                        int index = status.IndexOf(":");
                        string input = status.Substring(0, index);
                        if (input.StartsWith("WORK ORDER "))
                        {
                            e.Row.Cells[3].ForeColor = Color.Black;
                        }
                        if (input.StartsWith("FAILURE"))
                        {
                            e.Row.Cells[3].ForeColor = Color.Black;
                        }
                        if (input.StartsWith("INDENT "))
                        {
                            e.Row.Cells[3].ForeColor = Color.Black;
                        }
                        if (input.StartsWith("INVOICE "))
                        {
                            e.Row.Cells[3].ForeColor = Color.Black;
                        }
                        if (input.StartsWith("RI APPROVE "))
                        {
                            e.Row.Cells[3].ForeColor = Color.Black;
                        }
                        if (input.StartsWith("CR "))
                        {
                            e.Row.Cells[3].ForeColor = Color.Black;
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

        protected void Export_clickApprovalPending(object sender, EventArgs e)
        {

            try
            {
                clsDashboard objDashboard = new clsDashboard();
                DataTable dtLoadDetails = new DataTable();
                string sBOType = string.Empty;
                string sOfficeCode = string.Empty;

                string sRoleType = objSession.sRoleType;
                if (cmbDiv.SelectedIndex > 0)
                {
                    sOfficeCode = cmbDiv.SelectedValue;
                }

                if (cmbSubDiv.SelectedIndex > 0)
                {
                    sOfficeCode = cmbSubDiv.SelectedValue;
                }

                if (cmbSection.SelectedIndex > 0)
                {
                    sOfficeCode = cmbSection.SelectedValue;
                }
                if (cmbType.SelectedIndex > 0)
                {
                    sBOType = cmbType.SelectedValue;
                }

                if (sOfficeCode != "")
                {
                    dtLoadDetails = objDashboard.LoadApprovalPendingDetails(sOfficeCode, sBOType, sRoleType);
                }
                else
                {
                    if (cmbDiv.SelectedIndex == 0)
                    {
                        sOfficeCode = "";
                        dtLoadDetails = objDashboard.LoadApprovalPendingDetails(sOfficeCode, sBOType, sRoleType);
                    }
                    else
                    dtLoadDetails = objDashboard.LoadApprovalPendingDetails(hdfOffCode.Value, sBOType, sRoleType);
                }


                if (dtLoadDetails.Rows.Count > 0)
                {

                    dtLoadDetails.Columns["DT_CODE"].ColumnName = "Transformer CENTRE CODE";
                    dtLoadDetails.Columns["DT_NAME"].ColumnName = "Transformer CENTRE NAME";
                    dtLoadDetails.Columns["OMSECTION"].ColumnName = "SECTION NAME";
                    dtLoadDetails.Columns["STATUS"].ColumnName = "STATUS";


                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "ApprovalPendingDetails" + DateTime.Now + ".xls";
                    string pagetitle = "Approval Pending Details";

                    Genaral.getexcel(dtLoadDetails, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                }
            }
            catch (Exception ex)
            {
                //lblMessage.Text = clsException.ErrorMsg();
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
    }
}

