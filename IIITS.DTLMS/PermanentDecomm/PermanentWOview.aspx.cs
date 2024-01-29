using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Globalization;

namespace IIITS.DTLMS.PermanentDecomm
{
    public partial class PermanentWOview : System.Web.UI.Page
    {
        string strFormCode = "PermanentWOview";
        clsSession objSession;
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
                        LoadWOAlreadyCreated("1");
                    }
                    else
                    {
                        LoadAllWorkOrder("1");
                    }
                   // CheckAccessRights("4");
                }
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
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
                Response.Redirect("WorkOrder.aspx?TypeValue=" + sType, false);

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

        public void LoadWOAlreadyCreated(string sType)
        {

            try
            {
                clsPermanentWO objWO = new clsPermanentWO();

                objWO.sTaskType = sType;
                objWO.sOfficeCode = objSession.OfficeCode;

                DataTable dt = objWO.LoadAlreadyWorkOrder(objWO);
                grdWorkOrder.DataSource = dt;
                grdWorkOrder.DataBind();
                ViewState["Workorder"] = dt;
                if (sType == "1")
                {
                    lblGridType.Text = "Transformer Centre Permanent WorkOrder Details :";
                }
                else if (sType == "2")
                {
                    lblGridType.Text = "Transformer Centre Permanent WorkOrder Details :";
                }
                else if (sType == "4")
                {
                    lblGridType.Text = "Transformer Centre Permanent WorkOrder Details :";
                }

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void Export_ClickWorkorder(object sender, EventArgs e)
        {
            //clsWorkOrder objWO = new clsWorkOrder();
            //string sType = "";

            //if (cmbType.SelectedValue == "1")
            //{
            //    sType = "1";
            //}
            // if (cmbType.SelectedValue == "2")
            //{
            //    sType = "2";
            //}
            // if (cmbType.SelectedValue == "4")
            //{
            //    sType = "4";
            //}
            //else
            //{
            //    cmbExport.Visible = false;
            //}

            //objWO.sTaskType = sType;
            //objWO.sOfficeCode = objSession.OfficeCode;

            //DataTable dt = objWO.LoadAlreadyWorkOrder(objWO);

            DataTable dt = (DataTable)ViewState["Workorder"];

            if (dt.Rows.Count > 0)
            {

                dt.Columns["DT_CODE"].ColumnName = "Transformer Centre Code";
                dt.Columns["DT_NAME"].ColumnName = "Transformer Centre Name";
                dt.Columns["TC_CODE"].ColumnName = "DTR Code";
                dt.Columns["WO_NO"].ColumnName = "WO No";

                List<string> listtoRemove = new List<string> { "DF_ID", "STATUS" };
                string filename = "WorkOrder" + DateTime.Now + ".xls";
                string pagetitle = " Work Order View";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");
                ShowEmptyGrid();
            }



        }

        public void LoadAllWorkOrder(string sType)
        {

            try
            {
                clsPermanentWO objWO = new clsPermanentWO();
                string sMsg = string.Empty;

                objWO.sTaskType = sType;
                objWO.sOfficeCode = objSession.OfficeCode;

                DataTable dt = objWO.LoadAllWorkOrder(objWO);

                //To show the Type of Gridview
                if (sType == "1")
                {
                    //Gridview column visible true/false based on conditions
                    grdWorkOrder.Columns[0].Visible = true;
                    grdWorkOrder.Columns[1].Visible = false;

                    lblGridType.Text = "Transformer Centre  WorkOrder Details :";
                    sMsg = "";
                }
                else if (sType == "2")
                {
                    //Gridview column visible true/false based on conditions
                    grdWorkOrder.Columns[1].Visible = true;
                    grdWorkOrder.Columns[0].Visible = false;

                    lblGridType.Text = "Transformer Centre  WorkOrder Details :";
                    sMsg = "";
                }
                else if (sType == "4")
                {
                    //Gridview column visible true/false based on conditions
                    grdWorkOrder.Columns[1].Visible = true;
                    grdWorkOrder.Columns[0].Visible = false;

                    lblGridType.Text = "Transformer Centre  WorkOrder Details :";
                    sMsg = "";
                }


                if (dt.Rows.Count > 0)
                {
                    grdWorkOrder.DataSource = dt;
                    grdWorkOrder.DataBind();
                    ViewState["Workorder"] = dt;
                }
                else
                {

                    lblMessage.Text = "Note : No " + sMsg + " Transformer Centre Available Please Declare the Transformer Centre " + sMsg + " before creating a Work Order";
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





        protected void grdWorkOrder_RowCommand(object sender, GridViewCommandEventArgs e)
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

                    //It should be Either Failure or Enhancement Id
                    Label lblFailureId = (Label)row.FindControl("lblFailureId");

                    string sReferId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblFailureId.Text));
                    string sType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(cmbType.SelectedValue));

                    Response.Redirect("PermanentWO.aspx?ReferID=" + sReferId + "&TypeValue=" + sType, false);

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
                        sFilter = "PEST_ID Like '%" + txtFailureId.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtEnhanceId.Text != "")
                    {
                        sFilter = "PEST_ID Like '%" + txtEnhanceId.Text.Replace("'", "`") + "%' AND";
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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

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

        protected void rdbAlready_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LoadWOAlreadyCreated(cmbType.SelectedValue);


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
                LoadAllWorkOrder(cmbType.SelectedValue);
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
                else if (cmbType.SelectedValue == "2")
                {
                    rdbAlready.Visible = true;
                    grdWorkOrder.Visible = true;
                  //  grdNewDTC.Visible = false;
                    //cmbExport.Visible = true;

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
                    //cmdNew.Visible = false;
                }
                else if (cmbType.SelectedValue == "4")
                {
                    rdbAlready.Visible = true;
                    grdWorkOrder.Visible = true;
                  //  grdNewDTC.Visible = false;
                   // cmbExport.Visible = true;

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
                    //cmdNew.Visible = false;
                }
                else
                {
                    rdbAlready.Visible = false;
                    grdWorkOrder.Visible = false;
                   // grdNewDTC.Visible = true;
                    rdbViewAll.Checked = true;
                    LoadNewDTCWorkOrder();
                  //  cmdNew.Visible = true;
                  //  cmbExport.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdWorkOrder_RowDataBound(object sender, GridViewRowEventArgs e)
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

        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("PEST_ID");
                dt.Columns.Add("DT_CODE");
                dt.Columns.Add("DT_NAME");
                dt.Columns.Add("TC_CODE");
                dt.Columns.Add("STATUS");
                dt.Columns.Add("PWO_NO_DECOM");


                grdWorkOrder.DataSource = dt;
                grdWorkOrder.DataBind();

                int iColCount = grdWorkOrder.Rows[0].Cells.Count;
                grdWorkOrder.Rows[0].Cells.Clear();
                grdWorkOrder.Rows[0].Cells.Add(new TableCell());
                grdWorkOrder.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdWorkOrder.Rows[0].Cells[0].Text = "No Records Found";

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

                objApproval.sFormName = "PermanentWOview";
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

        #region NewDTC

        public void LoadNewDTCWorkOrder()
        {

            try
            {
                clsPermanentWO objWO = new clsPermanentWO();
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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

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

                    Response.Redirect("PermanentWO.aspx?ReferID=" + sReferId + "&TypeValue=" + sType, false);

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
                        sFilter = "PWO_NO_DECOM Like '%" + txtWoNo.Text.Replace("'", "`") + "%' AND";
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
                dt.Columns.Add("PWO_SLNO");
                dt.Columns.Add("PWO_NO_DECOM");
                dt.Columns.Add("PWO_DATE_DECOM");
                dt.Columns.Add("PWO_ACCCODE_DECOM");



                grdNewDTC.DataSource = dt;
                grdNewDTC.DataBind();

                int iColCount = grdNewDTC.Rows[0].Cells.Count;
                grdNewDTC.Rows[0].Cells.Clear();
                grdNewDTC.Rows[0].Cells.Add(new TableCell());
                grdNewDTC.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdNewDTC.Rows[0].Cells[0].Text = "No Records Found";

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