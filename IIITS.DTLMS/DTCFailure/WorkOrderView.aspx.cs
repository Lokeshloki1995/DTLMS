using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Globalization;
using System.Configuration;

namespace IIITS.DTLMS.DTCFailure
{
    public partial class WorkOrderView : System.Web.UI.Page
    {
        string strFormCode = "WorkOrderView";
        clsSession objSession;

        int Zone_code = Convert.ToInt32(ConfigurationManager.AppSettings["Zone_code"]);
        int Circle_code = Convert.ToInt32(ConfigurationManager.AppSettings["Circle_code"]);
        int Division_code = Convert.ToInt32(ConfigurationManager.AppSettings["Division_code"]);
        int SubDiv_code = Convert.ToInt32(ConfigurationManager.AppSettings["SubDiv_code"]);
        int Section_code = Convert.ToInt32(ConfigurationManager.AppSettings["Section_code"]);
        /// <summary>
        /// page load method to sewt default values
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
                else
                {
                    objSession = (clsSession)Session["clsSession"];
                    lblMessage.Text = string.Empty;
                    txtFromDate.Attributes.Add("readonly", "readonly");
                    txtToDate.Attributes.Add("readonly", "readonly");
                    CalendarExtender1.EndDate = System.DateTime.Now;
                    CalendarExtender2.EndDate = System.DateTime.Now;
                    if (!IsPostBack)
                    {
                        if (rdbAlready.Checked == true)
                        {
                            LoadWOAlreadyCreated(Constants.DTCFailure.TypeFailure);
                        }
                        else
                        {
                            LoadAllWorkOrder(Constants.DTCFailure.TypeFailure);
                        }
                        CheckAccessRights(Constants.CheckAccessRights.CheckAccessRightsReadOnly);

                        string stroffCode = string.Empty;
                        if (objSession.OfficeCode.Length <= 2 && objSession.OfficeCode.Length != 0)
                        {
                            stroffCode = clsStoreOffice.GetZone_Circle_Div_Offcode(objSession.OfficeCode, objSession.RoleId).Substring(0, Constants.Circle);
                        }
                        else
                        {
                            stroffCode = objSession.OfficeCode;
                        }
                        if (objSession.sRoleType == Convert.ToString(ConfigurationManager.AppSettings["RoleTypeStore"]))
                        {
                            stroffCode = clsStoreOffice.Getofficecode(objSession.OfficeCode);
                        }
                        string stroffCode1 = stroffCode;
                        if (stroffCode == null || stroffCode == "")
                        {
                            Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone);
                        }

                        if (stroffCode.Length >= Constants.Zone)
                        {
                            Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone);
                            stroffCode = stroffCode.Substring(0, Zone_code);
                            cmbZone.Items.FindByValue(stroffCode).Selected = true;
                            cmbZone.Enabled = false;
                            stroffCode = stroffCode1;
                        }
                        if (stroffCode.Length >= Constants.Zone)
                        {
                            Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_ZO_ID\"='"
                                + cmbZone.SelectedValue + "'", "--Select--", cmbCircle);

                            if (stroffCode.Length >= Constants.Circle)
                            {
                                stroffCode = stroffCode.Substring(0, Circle_code);
                                cmbCircle.Items.FindByValue(stroffCode).Selected = true;
                                cmbCircle.Enabled = false;
                                stroffCode = stroffCode1;
                            }
                        }
                        if (stroffCode.Length >= Constants.Circle)
                        {
                            Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CICLE_CODE\" AS TEXT)='"
                                + cmbCircle.SelectedValue + "'", "--Select--", cmbDiv);
                            if (stroffCode.Length >= Constants.Division)
                            {
                                stroffCode = stroffCode.Substring(0, Division_code);
                                cmbDiv.Items.FindByValue(stroffCode).Selected = true;
                                cmbDiv.Enabled = false;
                                stroffCode = stroffCode1;
                            }
                        }
                        if (stroffCode.Length >= Constants.Division)
                        {
                            Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE CAST(\"SD_DIV_CODE\" AS TEXT)='"
                                + cmbDiv.SelectedValue + "'", "--Select--", cmbSubDivision);

                            if (stroffCode.Length >= Constants.SubDivision)
                            {
                                stroffCode = stroffCode.Substring(0, SubDiv_code);
                                cmbSubDivision.Items.FindByValue(stroffCode).Selected = true;
                                cmbSubDivision.Enabled = false;
                                stroffCode = stroffCode1;
                            }
                        }
                        if (stroffCode.Length >= Constants.SubDivision)
                        {
                            Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_SUBDIV_CODE\"='"
                                + cmbSubDivision.SelectedValue + "'", "--Select--", cmbOMSection);
                            if (stroffCode.Length >= Constants.Section)
                            {
                                stroffCode = stroffCode.Substring(0, Section_code);
                                cmbOMSection.Items.FindByValue(stroffCode).Selected = true;
                                cmbOMSection.Enabled = false;
                            }
                        }
                    }
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
        /// to load work order details to grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    string sResult = Genaral.DateComparision(txtToDate.Text, txtFromDate.Text, false, false);
                    if (sResult == "2")
                    {
                        ShowMsgBox("To Date should be Greater than From Date");
                        txtToDate.Focus();
                        return;
                    }
                }
                LoadWOAlreadyCreated(cmbType.SelectedValue);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to reset the details in the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                if(objSession.sRoleType==ConfigurationManager.AppSettings["RoleTypeAdmin"])
                {
                    cmbZone.SelectedIndex = 0;
                    cmbCircle.Items.Clear();
                    cmbDiv.Items.Clear();
                    cmbSubDivision.Items.Clear();
                    cmbOMSection.Items.Clear();
                }
                txtFromDate.Text = string.Empty;
                txtToDate.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// on change method for zone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbZone.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_ZO_ID\"='"
                        + cmbZone.SelectedValue + "'", "--Select--", cmbCircle);
                    cmbDiv.Items.Clear();
                    cmbSubDivision.Items.Clear();
                    cmbOMSection.Items.Clear();
                }
                else
                {
                    cmbCircle.Items.Clear();
                    cmbDiv.Items.Clear();
                    cmbSubDivision.Items.Clear();
                    cmbOMSection.Items.Clear();
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
        /// on change method for circle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE \"DIV_CICLE_CODE\"='"
                        + cmbCircle.SelectedValue + "'", "--Select--", cmbDiv);
                    cmbSubDivision.Items.Clear();
                    cmbOMSection.Items.Clear();
                }
                else
                {
                    cmbDiv.Items.Clear();
                    cmbSubDivision.Items.Clear();
                    cmbOMSection.Items.Clear();
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
        /// on change method for division
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDiv.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE CAST(\"SD_DIV_CODE\" AS TEXT)='"
                        + cmbDiv.SelectedValue + "'", "--Select--", cmbSubDivision);
                    cmbOMSection.Items.Clear();
                }
                else
                {
                    cmbSubDivision.Items.Clear();
                    cmbOMSection.Items.Clear();
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
        /// on change method for subdivision
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbSubDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubDivision.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE CAST(\"OM_SUBDIV_CODE\" AS TEXT)='"
                        + cmbSubDivision.SelectedValue + "'", "--Select--", cmbOMSection);
                }
                else
                {
                    cmbOMSection.Items.Clear();
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
        /// to rediect for work order page to create new 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                string sType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(cmbType.SelectedValue));
                Response.Redirect("WorkOrder.aspx?TypeValue=" + sType, false);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to display popup message for validation
        /// </summary>
        /// <param name="sMsg"></param>
        private void ShowMsgBox(string sMsg)
        {
            try
            {
                string sShowMsg = string.Empty;
                sShowMsg = "<script language=javascript> alert ('" + sMsg + "')</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "Msg", sShowMsg);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to load work orders which is already created 
        /// </summary>
        /// <param name="sType"></param>
        public void LoadWOAlreadyCreated(string sType)
        {
            try
            {
                clsWorkOrder objWO = new clsWorkOrder();
                objWO.sTaskType = sType;
                if (cmbOMSection.SelectedIndex > 0)
                {
                    objWO.sOfficeCode = cmbOMSection.SelectedValue;
                }
                else if (cmbSubDivision.SelectedIndex > 0)
                {
                    objWO.sOfficeCode = cmbSubDivision.SelectedValue;
                }
                else if (cmbDiv.SelectedIndex > 0)
                {
                    objWO.sOfficeCode = cmbDiv.SelectedValue;
                }
                else if (cmbCircle.SelectedIndex > 0)
                {
                    objWO.sOfficeCode = cmbCircle.SelectedValue;
                }
                else if (cmbZone.SelectedIndex > 0)
                {
                    objWO.sOfficeCode = cmbZone.SelectedValue;
                }
                else
                {
                    objWO.sOfficeCode = objSession.OfficeCode;
                }
                if(objSession.sRoleType==Convert.ToString(ConfigurationManager.AppSettings["RoleTypeStore"]))
                {
                    objWO.sOfficeCode = clsStoreOffice.Getofficecode(objSession.OfficeCode);
                    cmbSubDivision.Enabled = false;
                    cmbOMSection.Enabled = false;
                }
                objWO.FromDate = txtFromDate.Text;
                //DateTime DToDate = DateTime.ParseExact(objWO.FromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //objWO.FromDate = DToDate.ToString("yyyy/MM/dd");

                objWO.ToDate = txtToDate.Text;
                //DateTime DToDate1 = DateTime.ParseExact(objWO.ToDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //objWO.ToDate = DToDate1.ToString("yyyy/MM/dd");

                DataTable dt = objWO.LoadAlreadyWorkOrder(objWO);
                grdWorkOrder.DataSource = dt;
                grdWorkOrder.DataBind();
                ViewState["Workorder"] = dt;
                if (sType == Constants.DTCFailure.TypeFailure)
                {
                    lblGridType.Text = "Transformer Centre Failure WorkOrder Details :";
                }
                else if (sType == Constants.DTCFailure.TypeEnhancement)
                {
                    lblGridType.Text = "Transformer Centre Enhancement WorkOrder Details :";
                }
                else if (sType == Constants.DTCFailure.TypeFailureWithEnhancement)
                {
                    lblGridType.Text = "Transformer Centre Failure With Enhancement WorkOrder Details :";
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
        /// to generate export excel for work order details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Export_ClickWorkorder(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["Workorder"];
            if (dt.Rows.Count > 0)
            {
                dt.Columns["DIVISION"].ColumnName = "Division Name";
                dt.Columns["SUBDIVISION"].ColumnName = "SubDivision Name";
                dt.Columns["SECTION"].ColumnName = "Section Name";
                dt.Columns["DT_CODE"].ColumnName = "Transformer Centre Code";
                dt.Columns["DT_NAME"].ColumnName = "Transformer Centre Name";
                dt.Columns["TC_CODE"].ColumnName = "DTR Code";
                dt.Columns["WO_NO"].ColumnName = "Comm WO No";
                dt.Columns["WO_DATE"].ColumnName = "Comm WO Date";
                dt.Columns["WO_NO_DECOM"].ColumnName = "DeComm WO No";
                dt.Columns["WO_DATE_DECOM"].ColumnName = "DeComm WO Date";

                List<string> listtoRemove = new List<string> { "DF_ID", "STATUS" };
                string filename = "WorkOrder" + DateTime.Now + ".xls";
                string pagetitle = " Work Order View";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox(Convert.ToString(ConfigurationManager.AppSettings["EmptyData"]));
                ShowEmptyGrid();
            }
        }
        /// <summary>
        /// to get all work orders created
        /// </summary>
        /// <param name="sType"></param>
        public void LoadAllWorkOrder(string sType)
        {
            try
            {
                clsWorkOrder objWO = new clsWorkOrder();
                string sMsg = string.Empty;
                objWO.sTaskType = sType;
                objWO.sOfficeCode = objSession.OfficeCode;
                DataTable dt = objWO.LoadAllWorkOrder(objWO);
                //To show the Type of Gridview
                if (sType == Constants.DTCFailure.TypeFailure)
                {
                    //Gridview column visible true/false based on conditions
                    grdWorkOrder.Columns[0].Visible = true;
                    grdWorkOrder.Columns[1].Visible = false;

                    lblGridType.Text = "Transformer Centre Failure WorkOrder Details :";
                    sMsg = "Failure";
                }
                else if (sType == Constants.DTCFailure.TypeEnhancement)
                {
                    //Gridview column visible true/false based on conditions
                    grdWorkOrder.Columns[1].Visible = true;
                    grdWorkOrder.Columns[0].Visible = false;

                    lblGridType.Text = "Transformer Centre Enhancement WorkOrder Details :";
                    sMsg = "Enhancement";
                }
                else if (sType == Constants.DTCFailure.TypeFailureWithEnhancement)
                {
                    //Gridview column visible true/false based on conditions
                    grdWorkOrder.Columns[1].Visible = true;
                    grdWorkOrder.Columns[0].Visible = false;

                    lblGridType.Text = "Transformer Centre Failure with Enhancement WorkOrder Details :";
                    sMsg = "Enhancement";
                }
                if (dt.Rows.Count > 0)
                {
                    grdWorkOrder.DataSource = dt;
                    grdWorkOrder.DataBind();
                    ViewState["Workorder"] = dt;
                }
                else
                {
                    lblMessage.Text = "Note : No " + sMsg + " Transformer Centre Available Please Declare the Transformer Centre "
                        + sMsg + " before creating a Work Order";
                    grdWorkOrder.DataSource = dt;
                    grdWorkOrder.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// method to perform based on row command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdWorkOrder_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Create" || e.CommandName == "CreateNew")
                {
                    if (e.CommandName == "CreateNew")
                    {
                        //Check AccessRights
                        bool bAccResult = CheckAccessRights(Constants.CheckAccessRights.CheckAccessRightsCreate);
                        if (bAccResult == false)
                        {
                            return;
                        }
                    }
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    //It should be Either Failure or Enhancement Id
                    Label lblFailureId = (Label)row.FindControl("lblFailureId");
                    string sReferId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblFailureId.Text));
                    string sType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(cmbType.SelectedValue));
                    string sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(Convert.ToString(ConfigurationManager.AppSettings["ActionTypeView"])));
                    Response.Redirect("WorkOrder.aspx?ReferID=" + sReferId + "&TypeValue=" + sType + "&ActionType=" + sActionType, false);
                }
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtFailureId = (TextBox)row.FindControl("txtFailureId");
                    TextBox txtEnhanceId = (TextBox)row.FindControl("txtEnhanceId");
                    TextBox txtDtCode = (TextBox)row.FindControl("txtDtcCode");
                    TextBox txtDtName = (TextBox)row.FindControl("txtdtcName");
                    TextBox txtDtrCode = (TextBox)row.FindControl("txtDtrCode");
                    DataTable dt = (DataTable)ViewState["Workorder"];
                    dv = dt.DefaultView;
                    if (txtFailureId.Text != "")
                    {
                        sFilter = "DF_ID Like '%" + txtFailureId.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtEnhanceId.Text != "")
                    {
                        sFilter = "DF_ID Like '%" + txtEnhanceId.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtDtCode.Text != "")
                    {
                        sFilter = "DT_CODE Like '%" + txtDtCode.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtDtName.Text != "")
                    {
                        sFilter += " DT_NAME Like '%" + txtDtName.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtDtrCode.Text != "")
                    {
                        sFilter += " TC_CODE Like '%" + txtDtrCode.Text.Replace("'", "`") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdWorkOrder.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdWorkOrder.DataSource = dv;
                            ViewState["Workorder"] = dv.ToTable();
                            grdWorkOrder.DataBind();
                        }
                        else
                        {
                            ViewState["Workorder"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        if (rdbAlready.Checked == true)
                        {
                            LoadWOAlreadyCreated(cmbType.SelectedValue);
                        }
                        else if (rdbViewAll.Checked == true)
                        {
                            LoadAllWorkOrder(cmbType.SelectedValue);
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
        /// method to get change based on index change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdWorkOrder_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdWorkOrder.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Workorder"];
                grdWorkOrder.DataSource = SortDataTable(dt as DataTable, true);
                grdWorkOrder.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        ///  to sort the grid based on asc or desc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdWorkOrder_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdWorkOrder.PageIndex;
            DataTable dt = (DataTable)ViewState["Workorder"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdWorkOrder.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdWorkOrder.DataSource = dt;
            }
            grdWorkOrder.DataBind();
            grdWorkOrder.PageIndex = pageIndex;
        }
        /// <summary>
        /// to sort the data tab;le as asc and desc
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
                        ViewState["Workorder"] = dataView.ToTable();
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["Workorder"] = dataView.ToTable();
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
        /// to get sort based on ascending
        /// </summary>
        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "ASC"; }
            set { ViewState["SortDirection"] = value; }
        }
        /// <summary>
        /// to get sort in the grid
        /// </summary>
        private string GridViewSortExpression
        {
            get { return ViewState["SortExpression"] as string ?? string.Empty; }
            set { ViewState["SortExpression"] = value; }
        }
        /// <summary>
        /// to get sort based on asending or descending
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
        /// to check already created work orders
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdbAlready_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LoadWOAlreadyCreated(cmbType.SelectedValue);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to check all created work orders
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdbViewAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LoadAllWorkOrder(cmbType.SelectedValue);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// on change method for type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbType.SelectedValue == Constants.DTCFailure.TypeFailure)
                {
                    rdbAlready.Visible = true;
                    grdWorkOrder.Visible = true;
                    grdNewDTC.Visible = false;
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
                    cmdNew.Visible = false;
                }
                else if (cmbType.SelectedValue == Constants.DTCFailure.TypeEnhancement)
                {
                    rdbAlready.Visible = true;
                    grdWorkOrder.Visible = true;
                    grdNewDTC.Visible = false;
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
                    cmdNew.Visible = false;
                }
                else if (cmbType.SelectedValue == Constants.DTCFailure.TypeFailureWithEnhancement)
                {
                    rdbAlready.Visible = true;
                    grdWorkOrder.Visible = true;
                    grdNewDTC.Visible = false;
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
                    cmdNew.Visible = false;
                }
                else
                {
                    rdbAlready.Visible = false;
                    grdWorkOrder.Visible = false;
                    grdNewDTC.Visible = true;
                    rdbViewAll.Checked = true;
                    LoadNewDTCWorkOrder();
                    cmdNew.Visible = true;
                    cmbExport.Visible = false;
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
        /// to get perform based on row data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdWorkOrder_RowDataBound(object sender, GridViewRowEventArgs e)
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
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to show the empty grid
        /// </summary>
        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("DF_ID");
                dt.Columns.Add("DIVISION");
                dt.Columns.Add("SUBDIVISION");
                dt.Columns.Add("SECTION");
                dt.Columns.Add("DT_CODE");
                dt.Columns.Add("DT_NAME");
                dt.Columns.Add("TC_CODE");
                dt.Columns.Add("STATUS");
                dt.Columns.Add("WO_NO");
                dt.Columns.Add("WO_DATE");
                dt.Columns.Add("WO_NO_DECOM");
                dt.Columns.Add("WO_DATE_DECOM");
                grdWorkOrder.DataSource = dt;
                grdWorkOrder.DataBind();
                int iColCount = grdWorkOrder.Rows[0].Cells.Count;
                grdWorkOrder.Rows[0].Cells.Clear();
                grdWorkOrder.Rows[0].Cells.Add(new TableCell());
                grdWorkOrder.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdWorkOrder.Rows[0].Cells[0].Text = Convert.ToString(ConfigurationManager.AppSettings["EmptyData"]);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }
        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY
                clsApproval objApproval = new clsApproval();
                objApproval.sFormName = "WorkOrder";
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

        #endregion

        #region NewDTC
        /// <summary>
        /// to load all details of new dtc work order
        /// </summary>
        public void LoadNewDTCWorkOrder()
        {

            try
            {
                clsWorkOrder objWO = new clsWorkOrder();
                string sMsg = string.Empty;

                objWO.sOfficeCode = objSession.OfficeCode;
                DataTable dt = objWO.LoadNewDTCWO(objWO);
                lblGridType.Text = "New Transformer Centre Commission WorkOrder Details :";
                sMsg = "New DTC Commission WorkOrder Details ";
                if (dt.Rows.Count > 0)
                {
                    grdNewDTC.DataSource = dt;
                    grdNewDTC.DataBind();
                    ViewState["NewDTC"] = dt;
                }
                else
                {

                    lblMessage.Text = "Note : No " + sMsg + " Available";
                    grdNewDTC.DataSource = dt;
                    grdNewDTC.DataBind();
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
        /// to get changes based on index
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdNewDTC_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdNewDTC.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["NewDTC"];
                grdNewDTC.DataSource = SortDataTable(dt as DataTable, true);
                grdNewDTC.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to get sort based on grid values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdNewDTC_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdNewDTC.PageIndex;
            DataTable dt = (DataTable)ViewState["NewDTC"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdNewDTC.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdNewDTC.DataSource = dt;

            }
            grdNewDTC.DataBind();
            grdNewDTC.PageIndex = pageIndex;
        }


        /// <summary>
        /// to get change based on row command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdNewDTC_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Create")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    //It should be Either Failure or Enhancement Id
                    Label lblWOSlno = (Label)row.FindControl("lblWOSlno");
                    string sReferId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblWOSlno.Text));
                    string sType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(cmbType.SelectedValue));
                    Response.Redirect("WorkOrder.aspx?ReferID=" + sReferId + "&TypeValue=" + sType, false);
                }
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtWoNo = (TextBox)row.FindControl("txtWoNo");
                    DataTable dt = (DataTable)ViewState["NewDTC"];
                    dv = dt.DefaultView;
                    if (txtWoNo.Text != "")
                    {
                        sFilter = "WO_NO Like '%" + txtWoNo.Text.Replace("'", "`") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdNewDTC.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdNewDTC.DataSource = dv;
                            ViewState["NewDTC"] = dv.ToTable();
                            grdNewDTC.DataBind();
                        }
                        else
                        {
                            ShowEmptyGridForNewDtc();
                        }
                    }
                    else
                    {
                        LoadNewDTCWorkOrder();
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
        /// to show empty grid
        /// </summary>
        public void ShowEmptyGridForNewDtc()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("WO_SLNO");
                dt.Columns.Add("WO_NO");
                dt.Columns.Add("WO_DATE");
                dt.Columns.Add("WO_ACC_CODE");
                grdNewDTC.DataSource = dt;
                grdNewDTC.DataBind();
                int iColCount = grdNewDTC.Rows[0].Cells.Count;
                grdNewDTC.Rows[0].Cells.Clear();
                grdNewDTC.Rows[0].Cells.Add(new TableCell());
                grdNewDTC.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdNewDTC.Rows[0].Cells[0].Text = ConfigurationManager.AppSettings["EmptyData"];
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        #endregion
    }
}