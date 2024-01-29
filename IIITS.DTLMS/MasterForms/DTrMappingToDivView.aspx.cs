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
    public partial class DTrMappingToDivView : System.Web.UI.Page
    {
        string strFormCode = "DTrMappingToDivView";
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
                txtEffectFrom.Attributes.Add("readonly", "readonly");

                CalendarExtender1.EndDate = System.DateTime.Now;

                if (!IsPostBack)
                {
                    LoadTCMakeMasterDetails();
                    CheckAccessRights("4");
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
                Response.Redirect("DTrMappingToDiv.aspx", false);


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "DTrMappingToDivView";
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
        protected void grdTcMake_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdTcMake.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Make"];

                grdTcMake.DataSource = SortDataTable(dt as DataTable, true);
                grdTcMake.DataBind();
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
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
                        ViewState["Make"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["Make"] = dataView.ToTable();

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
        protected void grdmakeDetails_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdTcMake.PageIndex;
            DataTable dt = (DataTable)ViewState["Make"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdTcMake.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdTcMake.DataSource = dt;
            }
            grdTcMake.DataBind();
            grdTcMake.PageIndex = pageIndex;
        }
        protected void cmdSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateEnableDisable() == true)
                {
                    clsTcMakeMaster objTcMake = new clsTcMakeMaster();
                    objTcMake.sReason = txtReason.Text.Replace("'", "").Replace("\"", "").Replace(";", "").Replace(",", "ç");
                    objTcMake.sEffectFrom = txtEffectFrom.Text;
                    objTcMake.sMakeId = Convert.ToString(ViewState["TM_ID"]);
                    objTcMake.sStatus = Convert.ToString(ViewState["TM_STATUS1"]);
                    ImageButton imgDeactive = new ImageButton();
                    ImageButton imgActive = new ImageButton();
                    if (objTcMake.sStatus == "A")
                    {
                        objTcMake.sStatus = "D";
                        bool bResult = objTcMake.ActiveDeactiveMake(objTcMake);
                        if (bResult == true)
                        {
                            imgDeactive.Visible = true;
                            imgActive.Visible = false;
                            ShowMsgBox("Make Deactivated Successfully");
                            LoadTCMakeMasterDetails();
                            txtEffectFrom.Text = "";
                            txtReason.Text = "";

                        }
                    }
                    else
                    {
                        objTcMake.sStatus = "A";
                        bool bResult = objTcMake.ActiveDeactiveMake(objTcMake);
                        if (bResult == true)
                        {
                            imgDeactive.Visible = false;
                            imgActive.Visible = true;
                            ShowMsgBox("Make Activated Successfully");
                            LoadTCMakeMasterDetails();
                            txtEffectFrom.Text = "";
                            txtReason.Text = "";
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
        public bool ValidateEnableDisable()
        {
            bool bValidate = false;
            try
            {
                if (txtEffectFrom.Text.Trim() == "")
                {
                    lblMsg.Text = "Enter Effect From";
                    txtEffectFrom.Focus();
                    mdlPopup.Show();
                    return bValidate;
                }
                if (txtReason.Text.Trim() == "")
                {
                    lblMsg.Text = "Enter Reason";
                    txtReason.Focus();
                    mdlPopup.Show();
                    return bValidate;
                }
                if (txtReason.Text.Length > 500)
                {
                    lblMsg.Text = "Enter Below 500 charecters";
                    txtReason.Focus();
                    mdlPopup.Show();
                    return bValidate;
                }

                string sResult = Genaral.DateComparision(txtEffectFrom.Text, "", true, false);
                if (sResult == "2")
                {
                    ShowMsgBox("Effect From Date should be Greater than Current Date");
                    txtEffectFrom.Focus();
                    mdlPopup.Show();
                    return bValidate;
                }

                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return bValidate;
            }
        }
        public void LoadTCMakeMasterDetails()
        {
            try
            {
                //  clsTcMakeMaster objDetails = new clsTcMakeMaster();
                ClsDTrMappingToDiv objDetails = new ClsDTrMappingToDiv();
                DataTable dt = objDetails.LoadTcMakeMaster();
                if (dt.Rows.Count > 0)
                {
                    grdTcMake.DataSource = dt;
                    grdTcMake.DataBind();
                    ViewState["Make"] = dt;
                }
                else
                {
                    ShowEmptyGrid();
                    ViewState["Make"] = dt;
                }

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdTcMake_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                LoadTCMakeMasterDetails();
                if (e.CommandName == "createedit")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                    string strMakeId = ((Label)row.FindControl("lblMakeId")).Text;
                    strMakeId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(strMakeId));
                    Response.Redirect("TcMakeMaster.aspx?MakeId=" + strMakeId + "", false);
                }

                //if (e.CommandName == "status")
                //{

                //    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                //    string sUserId = ((Label)row.FindControl("lblMakeId")).Text;
                //    string sStatus = ((Label)row.FindControl("lblStatus")).Text;
                //    ImageButton imgDeactive = new ImageButton();
                //    ImageButton imgActive = new ImageButton();
                //    imgDeactive = (ImageButton)row.FindControl("imgDeactive");
                //    imgActive = (ImageButton)row.FindControl("imgActive");
                //    clsTcMakeMaster objTcMaster = new clsTcMakeMaster();
                //    objTcMaster.sMakeId = sUserId;
                //    ViewState["TM_ID"] = sUserId;
                //    ViewState["TM_STATUS1"] = sStatus;
                //    this.mdlPopup.Show();

                //}

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtstartrange = (TextBox)row.FindControl("txtstartrange");
                    TextBox txtendrange = (TextBox)row.FindControl("txtendrange");
                    TextBox txtQuantity = (TextBox)row.FindControl("txtQuantity");
                    TextBox txtdivcode = (TextBox)row.FindControl("txtdivcode");

                    string qnty = Convert.ToString(txtQuantity.Text);

                    DataTable dt = (DataTable)ViewState["Make"];
                    dv = dt.DefaultView;

                    if (txtstartrange.Text != "")
                    {
                        sFilter = "DRA_STARTRANGE Like '%" + txtstartrange.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtendrange.Text != "")
                    {
                        sFilter = "DRA_ENDRANGE Like '%" + txtendrange.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtQuantity.Text != "")
                    {
                        sFilter = "QUANTITY = '" + qnty.Replace("'", "'") + "' AND";
                    }
                    if (txtdivcode.Text != "")
                    {
                        sFilter = "DDM_DIV_CODE Like '%" + txtdivcode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdTcMake.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdTcMake.DataSource = dv;
                            ViewState["Make"] = dv.ToTable();
                            grdTcMake.DataBind();

                        }
                        else
                        {
                            ViewState["Make"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadTCMakeMasterDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
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
        protected void Export_click(object sender, EventArgs e)
        {

            DataTable dt = (DataTable)ViewState["Make"];

            if (dt.Rows.Count > 0 )
            {
                dt.Columns["DRA_STARTRANGE"].ColumnName = "DTR START RANGE";
                dt.Columns["DRA_ENDRANGE"].ColumnName = "DTR END RANGE";
                dt.Columns["QUANTITY"].ColumnName = "QUANTITY";
                dt.Columns["DDM_DIV_CODE"].ColumnName = "DIVISION CODE";

                List<string> listtoRemove = new List<string> { "DRA_ID" };
                string filename = "MappedDTrDetails" + DateTime.Now + ".xls";
                string pagetitle = "DTr Mapped To Division ";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("no records found");
                ShowEmptyGrid();
            }


        }
        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("DRA_ID");
                dt.Columns.Add("DRA_STARTRANGE");
                dt.Columns.Add("DRA_ENDRANGE");
                dt.Columns.Add("QUANTITY");
                dt.Columns.Add("DDM_DIV_CODE");

                grdTcMake.DataSource = dt;
                grdTcMake.DataBind();

                int iColCount = grdTcMake.Rows[0].Cells.Count;
                grdTcMake.Rows[0].Cells.Clear();
                grdTcMake.Rows[0].Cells.Add(new TableCell());
                grdTcMake.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdTcMake.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdTcMake_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblStatus;
                    lblStatus = (Label)e.Row.FindControl("lblMakeId");
                    lblStatus = (Label)e.Row.FindControl("lblStatus");
                    ImageButton imgBtnEdit;
                    imgBtnEdit = (ImageButton)e.Row.FindControl("imgBtnEdit");

                    if (lblStatus.Text == "A")
                    {
                        ImageButton imgActive;
                        imgActive = (ImageButton)e.Row.FindControl("imgActive");
                        imgActive.Visible = true;
                        imgBtnEdit.Enabled = true;
                        imgBtnEdit.ToolTip = "";
                    }
                    else
                    {
                        ImageButton imgDeActive;
                        imgDeActive = (ImageButton)e.Row.FindControl("imgDeActive");
                        imgDeActive.Visible = true;
                        imgBtnEdit.Enabled = false;
                        imgBtnEdit.ToolTip = "Make is DeActivated,You Cannot Edit";
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