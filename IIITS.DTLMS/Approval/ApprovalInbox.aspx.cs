using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;

namespace IIITS.DTLMS.Approval
{
    public partial class ApprovalInbox : System.Web.UI.Page
    {
        string strFormCode = "ApprovalInbox";
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
                    txtFromDate.Attributes.Add("readonly", "readonly");
                    txtToDate.Attributes.Add("readonly", "readonly");
                    if (objSession.OfficeCodeName != null || objSession.OfficeCodeName != "")
                    {
                        lblLocation.Text = objSession.OfficeCodeName;
                    }
                    //   lblLocation.Text = objSession.OfficeCodeName;
                    txtFromDate_CalendarExtender1.EndDate = System.DateTime.Now;
                    txtToDate_CalendarExtender1.EndDate = System.DateTime.Now;

                    if (!IsPostBack)
                    {
                        LoadCombo();
                        LoadPendingApprovalInbox();
                        //CheckAccessRights("4");

                        //if (lblLocation.Text.Length == 0)
                        //{
                        //    ShowMsgBox("Please Select the Location");
                        //}
                        if (objSession.RoleId != Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                        {
                            lnkChange.Visible = false;
                            lblLoc.Visible = false;
                        }
                        else
                        {
                            lnkChange.Visible = true;
                            lblLoc.Visible = true;
                        }

                        if (Request.QueryString["RefType"] != null && Convert.ToString(Request.QueryString["RefType"]) != "")
                        {
                            hdfRefType.Value = Convert.ToString(Request.QueryString["RefType"]);
                            if (hdfRefType.Value == "1")
                            {
                                rdbAlready.Checked = true;
                                rdbPending.Checked = false;
                                LoadAlreadyApprovedInbox();
                            }
                            if (hdfRefType.Value == "3")
                            {
                                rdbAlready.Checked = false;
                                rdbPending.Checked = false;
                                rdbRejected.Checked = true;
                                //LoadAlreadyApprovedInbox();
                                LoadRejectedApprovedInbox();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }


        public void LoadCombo()
        {
            try
            {
                if (objSession.RoleId != Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                {
                    Genaral.Load_Combo("SELECT \"BO_ID\",\"BO_NAME\" FROM \"TBLWORKFLOWMASTER\",\"TBLBUSINESSOBJECT\" WHERE \"WM_ROLEID\" ='" + objSession.RoleId + "' AND \"WM_BOID\"=\"BO_ID\"", "-Select--", cmbSubject);
                }
                else
                {
                    Genaral.Load_Combo("SELECT DISTINCT \"BO_ID\",\"BO_NAME\" FROM \"TBLWORKFLOWMASTER\",\"TBLBUSINESSOBJECT\" WHERE   \"WM_BOID\"=\"BO_ID\"", "-Select--", cmbSubject);
                }


                string strQry = " SELECT \"US_ID\",\"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_ROLE_ID\" IN (SELECT \"WM_ROLEID\" FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_BOID\" IN ";
                strQry += " (SELECT \"WM_BOID\" FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_ROLEID\" ='" + objSession.RoleId + "'))  ";

                if (objSession.sRoleType == "1")
                {
                    strQry += " AND \"US_OFFICE_CODE\" LIKE '" + objSession.OfficeCode + "%' ";
                }
                else
                {
                    string sOffCode = clsStoreOffice.GetOfficeCode(objSession.OfficeCode, "US_OFFICE_CODE");
                    if (sOffCode != "" && sOffCode != null)
                    {
                        strQry += "AND ";
                        strQry += sOffCode;
                        //strQry += "AND \"US_OFFICE_CODE\" = "+ sOffCode + "'"; old
                    }
                }

                strQry += " AND \"US_ROLE_ID\" <>'" + objSession.RoleId + "'";

                Genaral.Load_Combo(strQry, "-Select--", cmbSentBy);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                lblMessage.Text = clsException.ErrorMsg();
            }
        }
        protected void lnkChange_Click(object sender, EventArgs e)
        {
            try
            {
                LoadOfficeGrid(objSession.OfficeCode);
                this.mdlLocations.Show();

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void LoadOfficeGrid(string sOfficeCode = "", string sOffName = "")
        {
            try
            {

                clsFeederMast objFeeder = new clsFeederMast();
                DataTable dt = new DataTable();

                objFeeder.OfficeCode = sOfficeCode;
                objFeeder.OfficeName = sOffName;

                dt = objFeeder.LoadOfficeDetails(objFeeder);
                if (dt.Rows.Count > 0)
                {
                    grdOffices.DataSource = dt;
                    grdOffices.DataBind();
                    ViewState["Office"] = dt;
                }
                else
                {
                    ShowEmptyGrid1();
                }

            }

            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdOffices_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtOffCode = (TextBox)row.FindControl("txtOffCode");
                    TextBox txtOffName = (TextBox)row.FindControl("txtOffName");

                    LoadOfficeGrid(txtOffCode.Text.Trim().Replace("'", "''"), txtOffName.Text.Trim().Replace("'", "''"));
                    lblLocation.Text = txtOffName.Text.Trim();
                    objSession.OfficeCode = txtOffCode.Text.Trim();

                    objSession.OfficeCodeName = txtOffName.Text.Trim();

                    this.mdlLocations.Show();
                    //LoadFailureChart();
                }

                if (e.CommandName == "submit")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    string sOffCode = ((Label)row.FindControl("lblOffCode")).Text;
                    string soffName = ((Label)row.FindControl("lblOffName")).Text;

                    lblLocation.Text = soffName.Trim();
                    hdfLocationCode.Value = sOffCode;
                    objSession.OfficeCode = sOffCode;
                    objSession.OfficeCodeName = soffName;
                    Session["OffCode"] = sOffCode;

                    Session["OfficeCodeName"] = soffName;

                    cmdLoad_Click(sender, e);

                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdOffices_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdOffices.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Office"];
                grdOffices.DataSource = dt;
                grdOffices.DataBind();

                this.mdlLocations.Show();
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadPendingApprovalInbox(string sFormName = "", string sDesc = "")
        {
            try
            {
                string sFilter = string.Empty;
                DataTable dtView = new DataTable();
                DataView dv = new DataView();
                clsApproval objApproval = new clsApproval();
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sOfficeCode = objSession.OfficeCode == "" ? hdfLocationCode.Value : objSession.OfficeCode;
                objApproval.sRoleType = objSession.sRoleType;

                if (cmbSubject.SelectedIndex > 0)
                {
                    objApproval.sBOId = cmbSubject.SelectedValue;
                }
                if (cmbSentBy.SelectedIndex > 0)
                {
                    objApproval.sCrby = cmbSentBy.SelectedValue;
                }

                objApproval.sFromDate = txtFromDate.Text;
                objApproval.sToDate = txtToDate.Text;

                objApproval.sFormName = sFormName;
                objApproval.sDescription = sDesc;

                if (objApproval.sBOId == "7")
                {
                    dtView = (DataTable)ViewState["Approval"];
                    dv = dtView.DefaultView;
                    sFilter = "BO_NAME ='DeCommission Entry' AND WO_DESCRIPTION LIKE 'Commissioning of DTC%' ";
                    dv.RowFilter = sFilter;
                    if (dv.Count > 0)
                    {
                        grdApprovalInbox.DataSource = dv;
                        ViewState["Approval"] = dv.ToTable();
                        grdApprovalInbox.DataBind();

                    }
                    else
                    {

                        ShowEmptyGrid();
                    }

                }
                else
                {

                    DataTable dt = objApproval.LoadPendingApprovalInbox(objApproval);
                    if (dt.Rows.Count == 0)
                    {
                        ShowEmptyGrid();
                        ViewState["Approval"] = dt;
                    }
                    else
                    {
                        grdApprovalInbox.DataSource = dt;
                        grdApprovalInbox.DataBind();
                        ViewState["Approval"] = dt;
                    }
                }

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void LoadAlreadyApprovedInbox(string sFormName = "", string sDesc = "")
        {
            try
            {
                string sFilter = string.Empty;
                DataTable dtView = new DataTable();
                DataView dv = new DataView();
                clsApproval objApproval = new clsApproval();
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sOfficeCode = objSession.OfficeCode == "" ? hdfLocationCode.Value : objSession.OfficeCode;
                objApproval.sRoleType = objSession.sRoleType;
                if (cmbSubject.SelectedIndex > 0)
                {
                    objApproval.sBOId = cmbSubject.SelectedValue;
                }
                if (cmbSentBy.SelectedIndex > 0)
                {
                    objApproval.sCrby = cmbSentBy.SelectedValue;
                }
                else
                {
                    objApproval.sCrby = objSession.UserId;
                }



                objApproval.sFromDate = txtFromDate.Text;
                objApproval.sToDate = txtToDate.Text;

                objApproval.sFormName = sFormName;
                objApproval.sDescription = sDesc;

                if (objApproval.sBOId == "7")
                {
                    dtView = (DataTable)ViewState["Approval"];
                    dv = dtView.DefaultView;
                    sFilter = "BO_NAME ='DeCommission Entry' AND WO_DESCRIPTION LIKE 'Commissioning of DTC%'";
                    dv.RowFilter = sFilter;
                    if (dv.Count > 0)
                    {
                        grdApprovalInbox.DataSource = dv;
                        ViewState["Approval"] = dv.ToTable();
                        grdApprovalInbox.DataBind();

                    }
                    else
                    {

                        ShowEmptyGrid();
                    }

                }
                else
                {

                    DataTable dt = objApproval.LoadAlreadyApprovedInbox(objApproval);
                    if (dt.Rows.Count == 0)
                    {
                        ShowEmptyGrid();
                        ViewState["Approval"] = dt;
                    }
                    else
                    {
                        grdApprovalInbox.DataSource = dt;
                        grdApprovalInbox.DataBind();
                        ViewState["Approval"] = dt;
                    }
                }

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadRejectedApprovedInbox(string sFormName = "", string sDesc = "")
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sOfficeCode = objSession.OfficeCode == "" ? hdfLocationCode.Value : objSession.OfficeCode;
                objApproval.sRoleType = objSession.sRoleType;
                if (cmbSubject.SelectedIndex > 0)
                {
                    objApproval.sBOId = cmbSubject.SelectedValue;
                }
                if (cmbSentBy.SelectedIndex > 0)
                {
                    objApproval.sCrby = cmbSentBy.SelectedValue;
                }

                objApproval.sFromDate = txtFromDate.Text;
                objApproval.sToDate = txtToDate.Text;

                objApproval.sFormName = sFormName;
                objApproval.sDescription = sDesc;

                DataTable dt = objApproval.LoadRejectedApprovedInbox(objApproval);
                if (dt.Rows.Count == 0)
                {
                    ShowEmptyGrid();
                    ViewState["Approval"] = dt;
                }
                else
                {
                    grdApprovalInbox.DataSource = dt;
                    grdApprovalInbox.DataBind();
                    ViewState["Approval"] = dt;
                }

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
        public void ShowEmptyGrid1()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("OFF_CODE");
                dt.Columns.Add("OFF_NAME");

                grdOffices.DataSource = dt;
                grdOffices.DataBind();

                int iColCount = grdOffices.Rows[0].Cells.Count;
                grdOffices.Rows[0].Cells.Clear();
                grdOffices.Rows[0].Cells.Add(new TableCell());
                grdOffices.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdOffices.Rows[0].Cells[0].Text = "No Records Found";

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
                dt.Columns.Add("WO_DATA_ID");
                dt.Columns.Add("WO_ID");
                dt.Columns.Add("WO_RECORD_ID");
                dt.Columns.Add("WO_BO_ID");
                dt.Columns.Add("BO_NAME");
                dt.Columns.Add("USER_NAME");
                dt.Columns.Add("CR_ON");
                dt.Columns.Add("STATUS");
                dt.Columns.Add("CURRENT_STATUS");
                dt.Columns.Add("RO_NAME");
                dt.Columns.Add("WO_APPROVE_STATUS");
                dt.Columns.Add("WO_DESCRIPTION");
                dt.Columns.Add("WOA_ID");
                dt.Columns.Add("WO_WFO_ID");
                dt.Columns.Add("WO_INITIAL_ID");
                dt.Columns.Add("CREATOR");
                dt.Columns.Add("WO_REF_OFFCODE");


                grdApprovalInbox.DataSource = dt;
                grdApprovalInbox.DataBind();
                int iColCount = grdApprovalInbox.Rows[0].Cells.Count;
                grdApprovalInbox.Rows[0].Cells.Clear();
                grdApprovalInbox.Rows[0].Cells.Add(new TableCell());
                grdApprovalInbox.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdApprovalInbox.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }

        protected void grdApprovalInbox_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdApprovalInbox.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Approval"];
                grdApprovalInbox.DataSource = SortDataTable(dt as DataTable, true);
                grdApprovalInbox.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdApprovalInbox_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdApprovalInbox.PageIndex;
            DataTable dt = (DataTable)ViewState["Approval"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdApprovalInbox.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdApprovalInbox.DataSource = dt;
            }
            grdApprovalInbox.DataBind();
            grdApprovalInbox.PageIndex = pageIndex;
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

        protected void grdApprovalInbox_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                {
                    if (lblLocation.Text.Length == 0 && objSession.OfficeCode == "")
                    {
                        ShowMsgBox("Please Select the Location");
                        return;
                    }
                }
                if (e.CommandName == "Approve")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    //string sWFOId = ((Label)row.FindControl("lblWFOId")).Text;
                    //string sApproveStatus = "1";

                    //ViewState["WFObject"] = sWFOId;
                    //ViewState["AppStatus"] = sApproveStatus;

                    //cmdApprove.Text = "Approve";
                    //txtComment.Text = "";

                    //cmdApprove.Enabled = true;
                    //this.mdlPopup.Show();

                    string sRecordId = ((Label)row.FindControl("lblRecordId")).Text;
                    string sBOId = ((Label)row.FindControl("lblBOId")).Text;
                    string sWFOId = ((Label)row.FindControl("lblWFOId")).Text;
                    string sWFAutoId = ((Label)row.FindControl("lblWFAutoId")).Text;
                    string sWFDataId = ((Label)row.FindControl("lblWFDataId")).Text;
                    string sWFInitialId = ((Label)row.FindControl("lblInitialId")).Text;
                    string sApproveStatus = ((Label)row.FindControl("lblApproveStatus")).Text;
                    string ssOfficeCode = ((Label)row.FindControl("lblOfficeCode")).Text;

                    //  ssOfficeCode

                    string sdataid = ((Label)row.FindControl("lbldataId")).Text;

                    RedirectToForm(sBOId, sRecordId, "A", sWFOId, sWFAutoId, sWFDataId, sWFInitialId, sApproveStatus, ssOfficeCode, sdataid);

                }

                if (e.CommandName == "Reject")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);


                    string sRecordId = ((Label)row.FindControl("lblRecordId")).Text;
                    string sBOId = ((Label)row.FindControl("lblBOId")).Text;
                    string sWFOId = ((Label)row.FindControl("lblWFOId")).Text;
                    string sWFAutoId = ((Label)row.FindControl("lblWFAutoId")).Text;
                    string sWFDataId = ((Label)row.FindControl("lblWFDataId")).Text;
                    string sWFInitialId = ((Label)row.FindControl("lblInitialId")).Text;
                    string sApproveStatus = ((Label)row.FindControl("lblApproveStatus")).Text;
                    string sdataid = ((Label)row.FindControl("lbldataId")).Text;
                    string ssOfficeCode = ((Label)row.FindControl("lblOfficeCode")).Text;


                    RedirectToForm(sBOId, sRecordId, "R", sWFOId, sWFAutoId, sWFDataId, sWFInitialId, sApproveStatus, ssOfficeCode, sdataid);

                }
                //Modify
                if (e.CommandName == "Modify")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    string sRecordId = ((Label)row.FindControl("lblRecordId")).Text;
                    string sBOId = ((Label)row.FindControl("lblBOId")).Text;
                    string sWFOId = ((Label)row.FindControl("lblWFOId")).Text;
                    string sWFAutoId = ((Label)row.FindControl("lblWFAutoId")).Text;
                    string sWFDataId = ((Label)row.FindControl("lblWFDataId")).Text;
                    string sWFInitialId = ((Label)row.FindControl("lblInitialId")).Text;
                    string sApproveStatus = ((Label)row.FindControl("lblApproveStatus")).Text;
                    string ssOfficeCode = ((Label)row.FindControl("lblOfficeCode")).Text;


                    string sdataid = ((Label)row.FindControl("lbldataId")).Text;

                    RedirectToForm(sBOId, sRecordId, "M", sWFOId, sWFAutoId, sWFDataId, sWFInitialId, sApproveStatus, ssOfficeCode, sdataid);

                }

                if (e.CommandName == "View")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    string sRecordId = ((Label)row.FindControl("lblRecordId")).Text;
                    string sBOId = ((Label)row.FindControl("lblBOId")).Text;
                    string sWFOId = ((Label)row.FindControl("lblWFOId")).Text;
                    string sWFAutoId = ((Label)row.FindControl("lblWFAutoId")).Text;
                    string sWFDataId = ((Label)row.FindControl("lblWFDataId")).Text;
                    string sWFInitialId = ((Label)row.FindControl("lblInitialId")).Text;
                    string sApproveStatus = ((Label)row.FindControl("lblApproveStatus")).Text;
                    string ssOfficeCode = ((Label)row.FindControl("lblOfficeCode")).Text;


                    string sdataid = ((Label)row.FindControl("lbldataId")).Text;

                    RedirectToForm(sBOId, sRecordId, "V", sWFOId, sWFAutoId, sWFDataId, sWFInitialId, sApproveStatus, ssOfficeCode, sdataid);

                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtFormName = (TextBox)row.FindControl("txtFormName");
                    TextBox txtDesc = (TextBox)row.FindControl("txtDesc");

                    if (rdbAlready.Checked == true)
                    {
                        LoadAlreadyApprovedInbox(txtFormName.Text.Trim().Replace("'", "''"), txtDesc.Text.Trim().Replace("'", "''"));
                    }
                    if (rdbPending.Checked == true)
                    {
                        LoadPendingApprovalInbox(txtFormName.Text.Trim().Replace("'", "''"), txtDesc.Text.Trim().Replace("'", "''"));
                    }
                    if (rdbRejected.Checked == true)
                    {
                        LoadRejectedApprovedInbox(txtFormName.Text.Trim().Replace("'", "''"), txtDesc.Text.Trim().Replace("'", "''"));
                    }

                }

                if (e.CommandName == "History")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    string sRecordId = ((Label)row.FindControl("lblRecordId")).Text;
                    string sBOId = ((Label)row.FindControl("lblBOId")).Text;
                    string sWFOId = ((Label)row.FindControl("lblWFOId")).Text;
                    string sWFAutoId = ((Label)row.FindControl("lblWFAutoId")).Text;
                    string sWFInitialId = ((Label)row.FindControl("lblInitialId")).Text;
                    string sSubject = ((Label)row.FindControl("lblSubject")).Text;

                    if (sBOId == "14" && sSubject.Contains("Commissioning"))
                    {
                        sBOId = "7";
                    }

                    sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                    sBOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sBOId));


                    Response.Redirect("ApprovalHistory.aspx?RecordId=" + sRecordId + "&BOId=" + sBOId, false);

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

                objApproval.sFormName = "TcMaster";
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

        protected void cmdApprove_Click(object sender, EventArgs e)
        {
            try
            {
                clsApproval objApproval = new clsApproval();


                if (txtComment.Text.Trim() == "")
                {
                    lblMsg.Text = "Enter Comments/Remarks";
                    txtComment.Focus();
                    this.mdlPopup.Show();
                    return;

                }

                objApproval.sCrby = objSession.UserId;
                objApproval.sOfficeCode = objSession.OfficeCode;
                objApproval.sApproveComments = txtComment.Text.Trim();
                objApproval.sWFObjectId = Convert.ToString(ViewState["WFObject"]);
                objApproval.sApproveStatus = Convert.ToString(ViewState["AppStatus"]);


                string sClientIP = string.Empty;

                string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(ip))
                {
                    string[] ipRange = ip.Split(',');
                    int le = ipRange.Length - 1;
                    sClientIP = ipRange[0];
                }
                else
                {
                    sClientIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                objApproval.sClientIp = sClientIP;

                bool bResult = objApproval.ApproveWFRequest(objApproval);
                if (bResult == true)
                {

                    if (objApproval.sApproveStatus == "1")
                    {
                        LoadPendingApprovalInbox();
                        ShowMsgBox("Approved Successfully");
                        cmdApprove.Enabled = false;
                    }
                    else if (objApproval.sApproveStatus == "3")
                    {
                        LoadAlreadyApprovedInbox();
                        ShowMsgBox("Rejected Successfully");
                        cmdApprove.Enabled = false;
                    }
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdApprovalInbox_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    LinkButton lnkApprove = (LinkButton)e.Row.FindControl("lnkApprove");
                    LinkButton lnkReject = (LinkButton)e.Row.FindControl("lnkReject");
                    LinkButton lnkModify = (LinkButton)e.Row.FindControl("lnkModify");
                    LinkButton lnkView = (LinkButton)e.Row.FindControl("lnkView");
                    LinkButton lnkHistory = (LinkButton)e.Row.FindControl("lnkHistory");

                    string sWFAutoId = ((Label)e.Row.FindControl("lblWFAutoId")).Text;



                    string sBOId = ((Label)e.Row.FindControl("lblBOId")).Text;
                    string sSubject = ((Label)e.Row.FindControl("lblSubject")).Text;

                    if (rdbAlready.Checked == true || rdbRejected.Checked == true)
                    {
                        lnkApprove.Visible = false;
                        lnkReject.Visible = false;
                        lnkModify.Visible = false;
                        lnkView.Visible = true;
                    }
                    if (rdbPending.Checked == true)
                    {
                        lnkApprove.Visible = true;
                        lnkReject.Visible = true;
                        lnkModify.Visible = true;
                        lnkView.Visible = false;

                        //From Auto Table ID
                        if (sWFAutoId != "0")
                        {
                            lnkReject.Visible = false;
                            lnkHistory.Visible = false;
                        }

                        // Check for Creator of Form
                        bool bResult = CheckFormCreatorLevel(sBOId, sWFAutoId);
                        if (bResult == true)
                        {
                            lnkReject.Visible = false;
                            lnkModify.Visible = false;
                        }
                        if (sBOId == "30"|| sBOId == "1020")
                        {
                           // lnkReject.Visible = false;
                            lnkModify.Visible = false;
                        }
                        if (sBOId == "9" && objSession.RoleId != "4")
                        {
                            lnkApprove.Visible = false;
                        }
                        if (sBOId == "30")
                        {
                            lnkReject.Visible = false;
                            lnkModify.Visible = false;
                        }
                        if (sBOId == "20")
                        {
                            lnkReject.Visible = false;
                            lnkModify.Visible = false;
                        }
                        if (sBOId == "24")
                        {
                            lnkReject.Visible = false;
                            lnkModify.Visible = false;
                        }

                        if (sBOId == "76")
                        {
                            lnkView.Visible = false;
                            lnkHistory.Visible = false;
                        }

                        if (sBOId == "1012")
                        {
                            lnkReject.Visible = false;
                        }

                    }

                    //For NEW DTC Commssion
                    if (sBOId == "14" && sSubject.Contains("Commissioning"))
                    {
                        Label lblFormName = (Label)e.Row.FindControl("lblFormName");
                        lblFormName.Text = "Commissioning of DTC";
                    }

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public bool CheckFormCreatorLevel(string sBOId, string sWOA_id)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                string sResult = objApproval.GetFormCreatorLevel(sBOId, objSession.RoleId, "", sWOA_id);
                if (sResult == "1")
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }

        protected void rdbAlready_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LoadAlreadyApprovedInbox();
                grdApprovalInbox.Columns[7].Visible = false;
                grdApprovalInbox.Columns[10].Visible = false;
                grdApprovalInbox.Columns[11].Visible = true;
                grdApprovalInbox.Columns[12].Visible = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }

        protected void rdbPending_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LoadPendingApprovalInbox();
                grdApprovalInbox.Columns[10].Visible = true;
                grdApprovalInbox.Columns[11].Visible = false;
                grdApprovalInbox.Columns[12].Visible = false;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }


        protected void rdbRejected_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LoadRejectedApprovedInbox();
                grdApprovalInbox.Columns[7].Visible = false;
                grdApprovalInbox.Columns[10].Visible = false;
                grdApprovalInbox.Columns[11].Visible = true;
                grdApprovalInbox.Columns[12].Visible = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }

        public void RedirectToForm(string sBOId, string sRecordId, string sActionType, string sWFOId,
            string sWFOAutoId, string sWFDataId, string sWFInitialId, string sApproveStatus, string ssOfficeCode, string sdataid)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                clsFormValues objForm = new clsFormValues();

                //Declaration
                string sDTCId = string.Empty;
                string sApprove = string.Empty;
                string sTaskType = string.Empty;
                string sFailType = string.Empty;
                string sRecid = string.Empty;

                Session["BOId"] = sBOId;
                Session["DataId"] = sWFDataId;
                Session["AutoID"] = sWFOId;
                string sFormName = objApproval.GetFormName(sBOId);

                if (sFormName == "EstimationCreation_sdo")
                {
                    sFormName = "EstimationCreation" + ".aspx";
                }

                else if (sFormName == "WorkOrder_sdo")
                {
                    sFormName = "WorkOrder" + ".aspx";
                }
                else if (sFormName == "FaultTCSearch")
                {
                    sFormName = "TCRepairIssue" + ".aspx";
                }
                else if (sFormName == "RepairerInvoiceCreation")
                {
                    sFormName = "FaultyTCInvoiceCreation" + ".aspx";
                }
                else
                {
                    sFormName = sFormName + ".aspx";
                }



                switch (sFormName)
                {
                    case "FailureEntry.aspx":

                        objForm.sFailureId = sRecordId;
                        sDTCId = objForm.GetDTCId(objForm);

                        sDTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sDTCId));
                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                        //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                        //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));


                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["ApproveStatus"] = sApproveStatus;
                        Session["WFOAutoId"] = sWFOAutoId;

                        Response.Redirect("/DTCFailure/" + sFormName + "?DTCId=" + sDTCId + "&FailureId=" + sRecordId + "&ActionType=" + sActionType, false);

                        break;

                    case "Enhancement.aspx":

                        objForm.sFailureId = sRecordId;
                        sDTCId = objForm.GetDTCId(objForm);

                        sDTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sDTCId));
                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                        // sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                        //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));

                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["ApproveStatus"] = sApproveStatus;
                        Session["WFOAutoId"] = sWFOAutoId;

                        Response.Redirect("/DTCFailure/" + sFormName + "?DTCId=" + sDTCId + "&EnhanceId=" + sRecordId + "&ActionType=" + sActionType, false);
                        break;

                    case "WorkOrder.aspx":


                        if (sWFOAutoId == "0")
                        {
                            objForm.sFailureId = sRecordId;
                            objForm.sWFInitialId = sWFInitialId;
                            string sWO_Record_Id = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                            objForm.GetStatusFlagForWOFromWF(objForm);

                            sTaskType = objForm.sTaskType;
                            //string sFailureId = objForm.sFailureId;
                            sFailType = objForm.sFailType;
                            if (sRecordId.Contains('-'))
                            {
                                if (sBOId != "11")
                                {
                                    sRecordId = objForm.sFailureId;
                                }
                                else if (sBOId == "11" && (sActionType == "A" || sActionType == "M" || sActionType == "R"))
                                {
                                    sRecordId = objForm.sFailureId;
                                }
                            }
                            if (sActionType == "V")
                            {
                                if (objForm.sTaskType == "3")
                                {
                                    sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                                }
                                else
                                {
                                    sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sdataid));
                                }
                            }
                            else
                            {
                                sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                            }
                            //if (sRecordId.Contains("-"))
                            //{
                            //    sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sFailureId));
                            //}
                            //else
                            //{
                            //    sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                            //}                      

                            sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                            if (sFailType != null)
                            {
                                sFailType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sFailType));
                            }

                            // sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                            //sWFOAutoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOAutoId));
                            // sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));


                            Session["WFOId"] = sWFOId;
                            Session["WFDataId"] = sWFDataId;
                            Session["WFOAutoId"] = sWFOAutoId;
                            Session["ApproveStatus"] = sApproveStatus;

                            if (sTaskType == "3")
                            {
                                sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                                Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sRecordId + "&ActionType=" + sActionType + "&sWoRecordID=" + sWO_Record_Id, false);
                            }
                            else
                            {
                                sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                                Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sRecordId + "&ActionType=" + sActionType + "&FailType=" + sFailType + "&sWoRecordID=" + sWO_Record_Id, false);
                            }


                        }
                        else
                        {
                            string sWO_Record_Id = "";
                            string FailureID = string.Empty;
                            objForm.sFailureId = sRecordId;
                            sTaskType = objForm.GetStatusFlagForWO(objForm);
                            sFailType = objForm.GetFailType(objForm); // To Identify Single Coil or Multi Coil

                            sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));

                            if (Convert.ToString(sFailType.Split('~').GetValue(0)) == "2" || Convert.ToString(sFailType.Split('~').GetValue(0)) == "1")
                            {
                                sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                                //sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sFailType.Split('~').GetValue(1).ToString()));
                                //FailureID = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                                FailureID = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sFailType.Split('~').GetValue(1).ToString()));
                            }
                            else
                            {
                                //  sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                                sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sFailType.Split('~').GetValue(1).ToString()));
                            }
                            sFailType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sFailType.Split('~').GetValue(0).ToString()));
                            sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));



                            Session["WFOId"] = sWFOId;
                            Session["WFDataId"] = sWFDataId;
                            Session["WFOAutoId"] = sWFOAutoId;
                            Session["ApproveStatus"] = sApproveStatus;

                            Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sRecordId + "&ActionType=" + sActionType + "&FailType=" + sFailType + "&sWoRecordID=" + sWO_Record_Id + "&FailureID=" + FailureID, false);
                        }

                        break;
                    case "WorkOrderRangeAllocation.aspx":

                        objForm.Worangeallocationid = sRecordId;

                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                        sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                        sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));


                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["ApproveStatus"] = sApproveStatus;
                        Session["WFOAutoId"] = sWFOAutoId;

                        Response.Redirect("/MasterForms/" + sFormName + "?sWfoid=" + sWFOId + "&Allocateid=" + sRecordId + "&ActionType=" + sActionType + "&wfdataid=" + sWFDataId, false);

                        break;


                    case "RepairerWorkOrder.aspx":


                        if (sWFOAutoId == "0")
                        {
                            objForm.sFailureId = sRecordId;
                            objForm.sWFInitialId = sWFInitialId;
                            string sWO_Record_Id = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                            ssOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(ssOfficeCode));
                            //  objForm.GetStatusFlagForWOFromWF(objForm);

                            sTaskType = "1";

                            sFailType = "1";
                            if (sRecordId.Contains('-'))
                            {
                                if (sBOId != "72")
                                {
                                    sRecordId = objForm.sFailureId;
                                }
                                else if (sBOId == "72" && (sActionType == "A" || sActionType == "M" || sActionType == "R"))
                                {
                                    sRecordId = objForm.sFailureId;
                                }
                            }
                            sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));


                            sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                            if (sFailType != null)
                            {
                                sFailType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sFailType));
                            }



                            Session["WFOId"] = sWFOId;
                            Session["WFDataId"] = sWFDataId;
                            Session["WFOAutoId"] = sWFOAutoId;
                            Session["ApproveStatus"] = sApproveStatus;

                            if (sTaskType == "3")
                            {
                                sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                                Response.Redirect("/TCRepair/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sRecordId + "&ActionType=" + sActionType + "&sWoRecordID=" + sWO_Record_Id, false);
                            }
                            else
                            {
                                sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                                Response.Redirect("/TCRepair/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sRecordId + "&ActionType=" + sActionType + "&OfficeCode=" + ssOfficeCode + "&FailType=" + sFailType + "&sWoRecordID=" + sWO_Record_Id, false);
                            }


                        }
                        else
                        {
                            string sWO_Record_Id = "";
                            objForm.sFailureId = sRecordId;
                            sTaskType = "1";
                            sFailType = "1"; // To Identify Single Coil or Multi Coil

                            sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));

                            if (Convert.ToString(sFailType.Split('~').GetValue(0)) == "2" || Convert.ToString(sFailType.Split('~').GetValue(0)) == "1")
                            {
                                sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                            }
                            else
                            {
                                sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sFailType.Split('~').GetValue(1).ToString()));
                            }
                            sFailType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sFailType.Split('~').GetValue(0).ToString()));
                            sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));

                            ssOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(ssOfficeCode));



                            Session["WFOId"] = sWFOId;
                            Session["WFDataId"] = sWFDataId;
                            Session["WFOAutoId"] = sWFOAutoId;
                            Session["ApproveStatus"] = sApproveStatus;

                            Response.Redirect("/TCRepair/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sRecordId + "&ActionType=" + sActionType + "&OfficeCode=" + ssOfficeCode + "&FailType=" + sFailType + "&sWoRecordID=" + sWO_Record_Id, false);
                        }

                        break;

                    case "IndentCreation.aspx":

                        if (sWFOAutoId == "0")
                        {
                            objForm.sWFInitialId = sWFInitialId;
                            objForm.GetStatusFlagForIndentFromWF(objForm);

                            sTaskType = objForm.sTaskType;
                            string sWOId = objForm.sWorkOrderId;


                            sWOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWOId));
                            sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                            sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                            sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                            //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                            //sWFOAutoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOAutoId));
                            //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));
                            ssOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(ssOfficeCode));


                            Session["WFOId"] = sWFOId;
                            Session["WFDataId"] = sWFDataId;
                            Session["WFOAutoId"] = sWFOAutoId;
                            Session["ApproveStatus"] = sApproveStatus;

                            Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sWOId + "&ActionType=" + sActionType + "&OfficeCode=" + ssOfficeCode + "&IndentId=" + sRecordId, false);
                        }
                        else
                        {
                            objForm.sWorkOrderId = sRecordId;
                            sTaskType = objForm.GetStatusFlagForIndent(objForm);

                            sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                            sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                            sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                            //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                            //sWFOAutoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOAutoId));
                            //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));
                            ssOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(ssOfficeCode));

                            Session["WFOId"] = sWFOId;
                            Session["WFDataId"] = sWFDataId;
                            Session["WFOAutoId"] = sWFOAutoId;
                            Session["ApproveStatus"] = sApproveStatus;

                            Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sRecordId + "&OfficeCode=" + ssOfficeCode + "&ActionType=" + sActionType, false);
                        }

                        break;

                    case "FaultyTCInvoiceCreation.aspx":

                        if (sWFOAutoId == "0")
                        {
                            objForm.sWFInitialId = sWFInitialId;

                            sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                            sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));

                            ssOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(ssOfficeCode));


                            Session["WFOId"] = sWFOId;
                            Session["WFDataId"] = sWFDataId;
                            Session["WFOAutoId"] = sWFOAutoId;
                            Session["ApproveStatus"] = sApproveStatus;

                            Response.Redirect("/TCRepair/" + sFormName + "?ReferID=" + sRecordId + "&ActionType=" + sActionType + "&OfficeCode=" + ssOfficeCode, false);
                        }
                        else
                        {
                            objForm.sWorkOrderId = sRecordId;
                            sTaskType = objForm.GetStatusFlagForIndent(objForm);

                            sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                            sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                            sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                            //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                            //sWFOAutoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOAutoId));
                            //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));
                            ssOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(ssOfficeCode));

                            Session["WFOId"] = sWFOId;
                            Session["WFDataId"] = sWFDataId;
                            Session["WFOAutoId"] = sWFOAutoId;
                            Session["ApproveStatus"] = sApproveStatus;

                            Response.Redirect("/TCRepair/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sRecordId + "&OfficeCode=" + ssOfficeCode + "&ActionType=" + sActionType, false);
                        }

                        break;

                    case "InvoiceCreation.aspx":

                        if (sWFOAutoId == "0")
                        {
                            objForm.sWFInitialId = sWFInitialId;
                            objForm.GetStatusFlagForInvoiceFromWF(objForm);

                            sTaskType = objForm.sTaskType;
                            string sIndentId = objForm.sIndentId;

                            sIndentId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sIndentId));
                            sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                            sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                            sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                            //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                            //sWFOAutoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOAutoId));
                            //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));
                            ssOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(ssOfficeCode));

                            Session["WFOId"] = sWFOId;
                            //Session["WFDataId"] = sWFDataId;
                            Session["WFOAutoId"] = sWFOAutoId;
                            Session["ApproveStatus"] = sApproveStatus;

                            Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sIndentId + "&ActionType=" + sActionType + "&OfficeCode=" + ssOfficeCode + "&InvoiceId=" + sRecordId, false);

                        }
                        else
                        {
                            objForm.sIndentId = sRecordId;
                            sTaskType = objForm.GetStatusFlagForInvoiceFromIndent(objForm);

                            sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                            sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                            sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                            //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                            //sWFOAutoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOAutoId));
                            //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));
                            ssOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(ssOfficeCode));


                            Session["WFOId"] = sWFOId;
                            //Session["WFDataId"] = sWFDataId;
                            Session["WFOAutoId"] = sWFOAutoId;
                            Session["ApproveStatus"] = sApproveStatus;

                            Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sRecordId + "&OfficeCode=" + ssOfficeCode + "&ActionType=" + sActionType, false);
                        }

                        break;

                    case "BankInvoice.aspx":

                        if (sWFOAutoId == "0")
                        {
                            objForm.sWFInitialId = sWFInitialId;
                            objForm.GetStatusFlagForInvoiceFromWF(objForm);

                            sTaskType = objForm.sTaskType;
                            string sIndentId = objForm.sIndentId;

                            sIndentId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sIndentId));
                            sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                            sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                            sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                            //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                            //sWFOAutoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOAutoId));
                            //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));

                            Session["WFOId"] = sWFOId;
                            //Session["WFDataId"] = sWFDataId;
                            Session["WFOAutoId"] = sWFOAutoId;
                            Session["ApproveStatus"] = sApproveStatus;

                            Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sIndentId + "&ActionType=" + sActionType + "&InvoiceId=" + sRecordId, false);

                        }
                        else
                        {
                            objForm.sIndentId = sRecordId;
                            sTaskType = objForm.GetStatusFlagForInvoiceFromIndent(objForm);

                            sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                            sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                            sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                            //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                            //sWFOAutoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOAutoId));
                            //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));

                            Session["WFOId"] = sWFOId;
                            //Session["WFDataId"] = sWFDataId;
                            Session["WFOAutoId"] = sWFOAutoId;
                            Session["ApproveStatus"] = sApproveStatus;

                            Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sRecordId + "&ActionType=" + sActionType, false);
                        }

                        break;

                    case "DeCommissioning.aspx":
                        //  string ssOfficeCode = ((Label)row.FindControl("lblOfficeCode")).Text;

                        if (sWFOAutoId == "0")
                        {

                            objForm.sWFInitialId = sWFInitialId;
                            objForm.GetStatusFlagForDecommFromWF(objForm);

                            sTaskType = objForm.sTaskType;
                            string sInvoiceId = objForm.sInvoiceId;


                            sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                            //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                            //sWFOAutoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOAutoId));
                            //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));

                            Session["WFOId"] = sWFOId;
                            Session["WFDataId"] = sWFDataId;
                            Session["WFOAutoId"] = sWFOAutoId;
                            Session["ApproveStatus"] = sApproveStatus;

                            if (sTaskType == "3")
                            {
                                objForm.GetWOnoForDTCCommission(objForm);

                                string sWOSlno = objForm.sWorkOrderId;
                                string sTCcode = objForm.sTCcode;

                                sDTCId = objForm.GetDTCIdFromWO(sWOSlno);

                                sWOSlno = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWOSlno));
                                sTCcode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTCcode));

                                if (sDTCId != "")
                                {
                                    sDTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sDTCId));
                                    Response.Redirect("/MasterForms/DTCCommision.aspx?WOSlno=" + sWOSlno + "&TCCode=" + sTCcode + "&ActionType=" + sActionType + "&QryDtcId=" + sDTCId, false);
                                }
                                else
                                {

                                    Response.Redirect("/MasterForms/DTCCommision.aspx?WOSlno=" + sWOSlno + "&TCCode=" + sTCcode + "&ActionType=" + sActionType, false);
                                }
                                break;
                            }
                            else
                            {
                                string sFailureId = sInvoiceId;

                                sFailureId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sFailureId));
                                sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                                sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                                ssOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(ssOfficeCode));

                                Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sFailureId + "&ActionType=" + sActionType + "&OfficeCode=" + ssOfficeCode + "&ReplaceId=" + sRecordId, false);
                                break;
                            }
                        }
                        else
                        {
                            objForm.sInvoiceId = sRecordId;

                            objForm.sWorkOrderId = sRecordId;

                            sTaskType = objForm.GetStatusFlagForDecommissionFromInvoice(objForm);

                            string sFailureId = objForm.GetFailureIdFromInvoice(sRecordId);
                            sFailureId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sFailureId));
                            sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                            //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                            //sWFOAutoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOAutoId));
                            //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));

                            Session["WFOId"] = sWFOId;
                            Session["WFDataId"] = sWFDataId;
                            Session["WFOAutoId"] = sWFOAutoId;
                            Session["ApproveStatus"] = sApproveStatus;

                            if (sTaskType == "3")
                            {
                                objForm.GetWOnoForDTCCommission(objForm);

                                string sWOSlno = objForm.sWorkOrderId;
                                string sTCcode = objForm.sTCcode;

                                sWOSlno = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWOSlno));
                                sTCcode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTCcode));

                                Response.Redirect("/MasterForms/DTCCommision.aspx?WOSlno=" + sWOSlno + "&TCCode=" + sTCcode + "&ActionType=" + sActionType, false);

                                break;
                            }
                            else
                            {

                                sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                                sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                                ssOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(ssOfficeCode));

                                Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sFailureId + "&OfficeCode=" + ssOfficeCode + "&ActionType=" + sActionType, false);
                                break;
                            }
                        }

                        break;
                    case "PseudoWorkOrder.aspx":

                        objForm.sFailureId = sRecordId;
                        sTaskType = objForm.GetStatusFlagForWO(objForm);

                        sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                        sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                        sWFOAutoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOAutoId));

                        Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sRecordId + "&ActionType=" + sActionType + "&WFOId=" + sWFOId + "&WFOAutoId=" + sWFOAutoId, false);
                        break;

                    case "RIApprove.aspx":

                        objForm.sDecommisionId = sRecordId;

                        sTaskType = objForm.GetStatusFlagForDecommission(objForm);

                        string sFailueId = objForm.GetFailureIdFromDecommId(sRecordId);

                        sFailueId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sFailueId));

                        sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                        //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                        //sWFOAutoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOAutoId));
                        //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));
                        ssOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(ssOfficeCode));

                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["WFOAutoId"] = sWFOAutoId;
                        Session["ApproveStatus"] = sApproveStatus;

                        Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&DecommId=" + sRecordId + "&ActionType=" + sActionType + "&OfficeCode=" + ssOfficeCode + "&FailureId=" + sFailueId, false);
                        break;


                    case "CRReport.aspx":

                        objForm.sDecommisionId = sRecordId;

                        // check  if RI and CR has done if no then don't leave to approve for CR .
                        clsCRReport objCR = new clsCRReport();
                        string inNo = objCR.CheckRIandInvDone(sRecordId);

                        if (Convert.ToInt32(inNo) < 0)
                        {
                            ShowMsgBox(" Please complete RI and Invoice to approve for CR");
                            return;
                        }

                        sTaskType = objForm.GetStatusFlagForDecommission(objForm);

                        sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                        //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                        //sWFOAutoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOAutoId));

                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["WFOAutoId"] = sWFOAutoId;
                        Session["ApproveStatus"] = sApproveStatus;

                        Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&DecommId=" + sRecordId + "&ActionType=" + sActionType, false);
                        break;

                    case "PseudoIndent.aspx":

                        objForm.sIndentId = sRecordId;

                        sTaskType = objForm.GetStatusFlagForInvoiceFromIndent(objForm);

                        string sWorkOrderId = objForm.GetWorkOrderId(sRecordId);
                        sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                        sWorkOrderId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWorkOrderId));
                        //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                        //sWFOAutoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOAutoId));
                        //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));
                        ssOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(ssOfficeCode));

                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["WFOAutoId"] = sWFOAutoId;


                        Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sWorkOrderId + "&ActionType=" + sActionType + "&OfficeCode=" + ssOfficeCode + "&IndentId=" + sRecordId, false);
                        // Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sRecordId + "&ActionType=" + sActionType, false);

                        break;

                    case "TCRepairIssue.aspx":

                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));

                        //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                        //sWFOAutoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOAutoId));
                        //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));

                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["WFOAutoId"] = sWFOAutoId;


                        Response.Redirect("/TCRepair/" + sFormName + "?TransId=" + sRecordId + "&ActionType=" + sActionType + "&WFDataId=" + sWFDataId, false);
                        break;

                    case "StoreIndent.aspx":


                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                        //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                        //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));


                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["WFOAutoId"] = sWFOAutoId;
                        Session["ApproveStatus"] = sApproveStatus;

                        Response.Redirect("/StoreTransfer/" + sFormName + "?QryIndentId=" + sRecordId + "&ActionType=" + sActionType, false);

                        break;

                    case "StoreInvoiceCreation.aspx":


                        string sStoreIndentId = objForm.GetStoreIndentIdFromWF(sWFInitialId, sWFOId);
                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));

                        sStoreIndentId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sStoreIndentId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                        //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                        //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));


                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["WFOAutoId"] = sWFOAutoId;
                        Session["ApproveStatus"] = sApproveStatus;

                        Response.Redirect("/StoreTransfer/" + sFormName + "?QryIndentId=" + sStoreIndentId + "&ActionType=" + sActionType + "&RecordId=" + sRecordId, false);

                        break;


                    case "RecieveTransCreate.aspx":


                        string sStoreInvoiceId = objForm.GetStoreInvoiceIdFromWF(sWFOId);
                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sStoreInvoiceId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sStoreInvoiceId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));

                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["WFOAutoId"] = sWFOAutoId;
                        Session["ApproveStatus"] = sApproveStatus;

                        Response.Redirect("/StoreTransfer/" + sFormName + "?QryInvoiceId=" + sStoreInvoiceId + "&ActionType=" + sActionType + "&RecordId=" + sRecordId, false);

                        break;

                    case "EstimationCreation.aspx":

                        //string sStoreInvoiceId = objForm.GetStoreInvoiceIdFromWF(sWFOId);
                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        //sStoreInvoiceId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sStoreInvoiceId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                        ssOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(ssOfficeCode));

                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["WFOAutoId"] = sWFOAutoId;
                        Session["ApproveStatus"] = sApproveStatus;

                        Response.Redirect("/DTCFailure/" + sFormName + "?FailId=" + sRecordId + "&OfficeCode=" + ssOfficeCode + "&ActionType=" + sActionType, false);

                        break;

                    case "FaultyEstimateCreation.aspx":

                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                        ssOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(ssOfficeCode));
                        sdataid = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sdataid));

                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["WFOAutoId"] = sWFOAutoId;
                        Session["ApproveStatus"] = sApproveStatus;

                        Response.Redirect("/TCRepair/" + sFormName + "?TCcode=" + sdataid + "&RestId=" + sRecordId + "&OfficeCode=" + ssOfficeCode + "&ActionType=" + sActionType, false);

                        break;

                    case "ReceiveMinorTC.aspx":
                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));

                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["WFOAutoId"] = sWFOAutoId;
                        Session["ApproveStatus"] = sApproveStatus;

                        Response.Redirect("/MinorRepair/" + sFormName + "?WOId=" + sRecordId + "&ActionType=" + sActionType, false);
                        break;

                    case "MinorTCInvoice.aspx":
                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                        sdataid = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sdataid));

                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["WFOAutoId"] = sWFOAutoId;
                        Session["ApproveStatus"] = sApproveStatus;

                        Response.Redirect("/MinorRepair/" + sFormName + "?WOId=" + sRecordId + "&ActionType=" + sActionType + "&sdataid=" + sdataid, false);
                        break;

                    case "ReceiveMinorTCBy_EE.aspx":
                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));

                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["WFOAutoId"] = sWFOAutoId;
                        Session["ApproveStatus"] = sApproveStatus;

                        Response.Redirect("/MinorRepair/" + sFormName + "?WOId=" + sRecordId + "&ActionType=" + sActionType, false);
                        break;

                    case "DTRBilling.aspx":

                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));


                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["ApproveStatus"] = sApproveStatus;
                        Session["WFOAutoId"] = sWFOAutoId;

                        Response.Redirect("/Billing/" + sFormName + "?WOId=" + sRecordId + "&ActionType=" + sActionType, false);

                        break;

                    case "MajorDTRBilling.aspx":

                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));


                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["ApproveStatus"] = sApproveStatus;
                        Session["WFOAutoId"] = sWFOAutoId;

                        Response.Redirect("/Billing/" + sFormName + "?WOId=" + sRecordId + "&ActionType=" + sActionType, false);

                        break;

                    case "PaymentDetails.aspx":

                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));


                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["ApproveStatus"] = sApproveStatus;
                        Session["WFOAutoId"] = sWFOAutoId;

                        Response.Redirect("/Billing/" + sFormName + "?WOId=" + sRecordId + "&ActionType=" + sActionType, false);

                        break;


                    case "MajorPaymentDetails.aspx":

                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));


                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["ApproveStatus"] = sApproveStatus;
                        Session["WFOAutoId"] = sWFOAutoId;

                        Response.Redirect("/Billing/" + sFormName + "?WOId=" + sRecordId + "&ActionType=" + sActionType, false);

                        break;

                    case "WorkAward.aspx":

                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));


                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["ApproveStatus"] = sApproveStatus;
                        Session["WFOAutoId"] = sWFOAutoId;

                        Response.Redirect("/WorkAward/" + sFormName + "?WOId=" + sRecordId + "&ActionType=" + sActionType, false);

                        break;


                    case "MajorWorkAward.aspx":

                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));


                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["ApproveStatus"] = sApproveStatus;
                        Session["WFOAutoId"] = sWFOAutoId;

                        Response.Redirect("/WorkAward/" + sFormName + "?WOId=" + sRecordId + "&ActionType=" + sActionType, false);

                        break;

                    case "Transiloil.aspx":

                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));


                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["ApproveStatus"] = sApproveStatus;
                        Session["WFOAutoId"] = sWFOAutoId;
                        Session["dataId"] = sdataid;

                        Response.Redirect("/WorkAward/" + sFormName + "?WOId=" + sRecordId + "&ActionType=" + sActionType, false);

                        break;

                    case "BankIndent.aspx":

                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));


                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["ApproveStatus"] = sApproveStatus;
                        Session["WFOAutoId"] = sWFOAutoId;

                        Response.Redirect("/StoreTransfer/" + sFormName + "?InId=" + sRecordId + "&ActionType=" + sActionType, false);

                        break;

                    case "TransBankInvoice.aspx":

                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));


                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["ApproveStatus"] = sApproveStatus;
                        Session["WFOAutoId"] = sWFOAutoId;

                        Response.Redirect("/StoreTransfer/" + sFormName + "?InId=" + sRecordId + "&ActionType=" + sActionType, false);

                        break;

                    case "ReceiveTransBank.aspx":

                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));


                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["ApproveStatus"] = sApproveStatus;
                        Session["WFOAutoId"] = sWFOAutoId;

                        Response.Redirect("/StoreTransfer/" + sFormName + "?InId=" + sRecordId + "&ActionType=" + sActionType, false);

                        break;

                    case "Permanentestimation.aspx":

                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));


                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["ApproveStatus"] = sApproveStatus;
                        Session["WFOAutoId"] = sWFOAutoId;
                        Session["sWFInitialId"] = sWFInitialId;
                        string dtccode = objApproval.getdataid(sWFInitialId);
                        dtccode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(dtccode));


                        Response.Redirect("/PermanentDecomm/" + sFormName + "?EstID=" + sRecordId + "&ActionType=" + sActionType + "&DTCId=" + dtccode + "&sWFInitialId=" + sWFInitialId, false);

                        break;

                    case "PermanentWO.aspx":


                        if (sWFOAutoId == "0")
                        {

                            objForm.sFailureId = sRecordId;
                            objForm.sWFInitialId = sWFInitialId;
                            sRecid = sRecordId;

                            objForm.GetStatusFlagForWOFromWFperdecomm(objForm);

                            sTaskType = objForm.sTaskType;

                            sFailType = objForm.sFailType;
                            if (sRecordId.Contains('-'))
                            {
                                sRecordId = objForm.sFailureId;
                            }
                            sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));


                            sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                            if (sFailType != null)
                            {
                                sFailType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sFailType));
                            }

                            Session["WFOId"] = sWFOId;
                            Session["WFDataId"] = sWFDataId;
                            Session["WFOAutoId"] = sWFOAutoId;
                            Session["ApproveStatus"] = sApproveStatus;

                            if (sTaskType == "3")
                            {
                                sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                                Response.Redirect("/PermanentDecomm/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sRecordId + "&ActionType=" + sActionType + "&RecordId=" + sRecid, false);
                            }
                            else
                            {
                                sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                                Response.Redirect("/PermanentDecomm/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sRecordId + "&ActionType=" + sActionType + "&FailType=" + sFailType + "&RecordId=" + sRecid, false);
                            }


                        }
                        else
                        {
                            objForm.sFailureId = sRecordId;
                            sTaskType = objForm.GetStatusFlagForWOperdecomm(objForm);
                            sFailType = objForm.GetFailType(objForm); // To Identify Single Coil or Multi Coil
                            sRecid = sRecordId;


                            sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                            sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                            sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                            sFailType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sFailType));


                            Session["WFOId"] = sWFOId;
                            Session["WFDataId"] = sWFDataId;
                            Session["WFOAutoId"] = sWFOAutoId;
                            Session["ApproveStatus"] = sApproveStatus;

                            Response.Redirect("/PermanentDecomm/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sRecordId + "&ActionType=" + sActionType + "&FailType=" + sFailType + "&RecordId=" + sRecid, false);
                        }

                        break;

                    case "PermanentIndent.aspx":

                        if (sWFOAutoId == "0")
                        {
                            objForm.sWFInitialId = sWFInitialId;
                            objForm.GetStatusFlagForIndentFromWFper(objForm);

                            sTaskType = objForm.sTaskType;
                            string sWOId = objForm.sWorkOrderId;


                            sWOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWOId));
                            sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                            sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                            sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));


                            Session["WFOId"] = sWFOId;
                            Session["WFDataId"] = sWFDataId;
                            Session["WFOAutoId"] = sWFOAutoId;
                            Session["ApproveStatus"] = sApproveStatus;

                            Response.Redirect("/PermanentDecomm/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sWOId + "&ActionType=" + sActionType + "&IndentId=" + sRecordId, false);
                        }

                        else
                        {
                            objForm.sInvoiceId = sRecordId;
                            sTaskType = objForm.GetStatusFlagForDecommissionFromInvoice(objForm);

                            //string sFailureId = objForm.GetEstIdFromIndent(sRecordId);
                            string sFailureId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                            sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));


                            Session["WFOId"] = sWFOId;
                            Session["WFDataId"] = sWFDataId;
                            Session["WFOAutoId"] = sWFOAutoId;
                            Session["ApproveStatus"] = sApproveStatus;


                            sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                            sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));

                            Response.Redirect("/PermanentDecomm/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sFailureId + "&ActionType=" + sActionType, false);
                            break;

                        }

                        break;

                    case "PermanentDecomm.aspx":

                        if (sWFOAutoId == "0")
                        {

                            objForm.sWFInitialId = sWFInitialId;
                            objForm.GetStatusFlagForDecommFromWFper(objForm);

                            sTaskType = objForm.sTaskType;
                            string sIndentId = objForm.sIndentId;


                            sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));


                            Session["WFOId"] = sWFOId;
                            Session["WFDataId"] = sWFDataId;
                            Session["WFOAutoId"] = sWFOAutoId;
                            Session["ApproveStatus"] = sApproveStatus;



                            string sFailureId = objForm.GetEstIdFromIndent(sIndentId);

                            sFailureId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sFailureId));
                            sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                            sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));

                            Response.Redirect("/PermanentDecomm/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sFailureId + "&ActionType=" + sActionType + "&ReplaceId=" + sRecordId, false);
                            break;
                        }

                        //}
                        //Commented By sandeep on 07/04/2022 bcz of dtc code miss match
                        else
                        {
                            objForm.sInvoiceId = sRecordId;
                            sTaskType = objForm.GetStatusFlagForDecommissionFromInvoiceper(objForm);

                            objForm.sWFInitialId = sWFInitialId;
                            // objForm.GetStatusFlagForDecommFromWFper(objForm);
                            // string sIndentId = objForm.sIndentId;

                            string sFailureId = objForm.GetEstIdFromIndent(sRecordId);

                            sFailureId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sFailureId));
                            sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));


                            Session["WFOId"] = sWFOId;
                            Session["WFDataId"] = sWFDataId;
                            Session["WFOAutoId"] = sWFOAutoId;
                            Session["ApproveStatus"] = sApproveStatus;




                            sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                            sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));

                            Response.Redirect("/PermanentDecomm/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sFailureId + "&ActionType=" + sActionType, false);
                            break;

                        }

                        break;

                    case "PermanentRI.aspx":

                        objForm.sDecommisionId = sRecordId;

                        sTaskType = objForm.GetStatusFlagForDecommissionper(objForm);

                        string sFailueIds = objForm.GetEstIdFromDecommId(sRecordId);

                        sFailueIds = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sFailueIds));

                        sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));


                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["WFOAutoId"] = sWFOAutoId;
                        Session["ApproveStatus"] = sApproveStatus;

                        Response.Redirect("/PermanentDecomm/" + sFormName + "?TypeValue=" + sTaskType + "&DecommId=" + sRecordId + "&ActionType=" + sActionType + "&FailureId=" + sFailueIds, false);
                        break;

                    case "PermanentCR.aspx":

                        objForm.sDecommisionId = sRecordId;

                        sTaskType = objForm.GetStatusFlagForDecommissionper(objForm);

                        sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));

                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["WFOAutoId"] = sWFOAutoId;
                        Session["ApproveStatus"] = sApproveStatus;

                        Response.Redirect("/PermanentDecomm/" + sFormName + "?TypeValue=" + sTaskType + "&DecommId=" + sRecordId + "&ActionType=" + sActionType, false);
                        break;
                    case "NewDTCCommission.aspx":

                        if (sWFOAutoId == "0")
                        {

                            objForm.sWFInitialId = sWFInitialId;
                            objForm.GetStatusFlagForDecommFromWF(objForm);

                            sTaskType = objForm.sTaskType;
                            string sInvoiceId = objForm.sInvoiceId;


                            sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                            //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                            //sWFOAutoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOAutoId));
                            //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));

                            Session["WFOId"] = sWFOId;
                            Session["WFDataId"] = sWFDataId;
                            Session["WFOAutoId"] = sWFOAutoId;
                            Session["ApproveStatus"] = sApproveStatus;

                            if (sTaskType == "3")
                            {
                                objForm.GetWOnoForDTCCommission(objForm);

                                string sWOSlno = objForm.sWorkOrderId;
                                string sTCcode = objForm.sTCcode;

                                sDTCId = objForm.GetDTCIdFromWO(sWOSlno);

                                sWOSlno = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWOSlno));
                                sTCcode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTCcode));

                                if (sDTCId != "")
                                {
                                    sDTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sDTCId));
                                    Response.Redirect("/DTCFailure/NewDTCCommission.aspx?WOSlno=" + sWOSlno + "&TCCode=" + sTCcode + "&ActionType=" + sActionType + "&QryDtcId=" + sDTCId, false);
                                }
                                else
                                {

                                    Response.Redirect("/DTCFailure/NewDTCCommission.aspx?WOSlno=" + sWOSlno + "&TCCode=" + sTCcode + "&ActionType=" + sActionType, false);
                                }
                                break;
                            }
                            //else
                            //{
                            //    string sFailureId = objForm.GetFailureIdFromInvoice(sInvoiceId);

                            //    sFailureId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sFailureId));
                            //    sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                            //    sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));

                            //    Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sFailureId + "&ActionType=" + sActionType + "&ReplaceId=" + sRecordId, false);
                            //    break;
                            //}
                        }
                        else
                        {
                            objForm.sInvoiceId = sRecordId;
                            sTaskType = objForm.GetStatusFlagForDecommissionFromInvoice(objForm);

                            string sFailureId = objForm.GetFailureIdFromInvoice(sRecordId);
                            sFailureId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sFailureId));
                            sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                            //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                            //sWFOAutoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOAutoId));
                            //sWFDataId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFDataId));

                            Session["WFOId"] = sWFOId;
                            Session["WFDataId"] = sWFDataId;
                            Session["WFOAutoId"] = sWFOAutoId;
                            Session["ApproveStatus"] = sApproveStatus;

                            if (sTaskType == "3")
                            {

                                objForm.GetWOnoForDTCCommission(objForm);
                                if (objForm.sWOTTKstatus != "1")
                                {
                                    string sWOSlno = objForm.sWorkOrderId;
                                    string sTCcode = objForm.sTCcode;

                                    sWOSlno = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWOSlno));
                                    sTCcode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTCcode));

                                    Response.Redirect("/DTCFailure/NewDTCCommission.aspx?WOSlno=" + sWOSlno + "&TCCode=" + sTCcode + "&ActionType=" + sActionType, false);

                                    break;
                                }
                                else
                                {
                                    string sWOSlno = objForm.sWorkOrderId;

                                    sWOSlno = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWOSlno));


                                    Response.Redirect("/DTCFailure/NewDTCCommission.aspx?WOSlno=" + sWOSlno + "&sWoTTKSTatus=" + objForm.sWOTTKstatus + "&ActionType=" + sActionType, false);

                                    break;
                                }
                            }
                            //else
                            //{

                            //    sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTaskType));
                            //    sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));

                            //    Response.Redirect("/DTCFailure/" + sFormName + "?TypeValue=" + sTaskType + "&ReferID=" + sFailureId + "&ActionType=" + sActionType, false);
                            //    break;
                            //}
                        }

                        break;

                    case "NewDTCCR.aspx":

                        objForm.sDecommisionId = sRecordId;

                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                        //sWFOId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOId));
                        //sWFOAutoId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sWFOAutoId));

                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["WFOAutoId"] = sWFOAutoId;
                        Session["ApproveStatus"] = sApproveStatus;

                        Response.Redirect("/DTCFailure/" + sFormName + "?DtcId=" + sRecordId + "&ActionType=" + sActionType, false);
                        break;

                    case "PMCInvoiceCreate.aspx":

                        sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRecordId));
                        sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sActionType));
                        sdataid = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sdataid));

                        Session["WFOId"] = sWFOId;
                        Session["WFDataId"] = sWFDataId;
                        Session["WFOAutoId"] = sWFOAutoId;
                        Session["ApproveStatus"] = sApproveStatus;

                        Response.Redirect("/POFlow/" + sFormName + "?WOId=" + sRecordId + "&ActionType=" + sActionType + "&sdataid=" + sdataid, false);
                        break;

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
                if (rdbPending.Checked == true)
                {
                    LoadPendingApprovalInbox();
                }
                else if (rdbAlready.Checked == true)
                {
                    LoadAlreadyApprovedInbox();
                }
                else if (rdbRejected.Checked == true)
                {
                    LoadRejectedApprovedInbox();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(5000);
        }
    }
}