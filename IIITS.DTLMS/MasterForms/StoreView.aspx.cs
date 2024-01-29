using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IIITS.DTLMS.BL;
 

namespace IIITS.DTLMS.MasterForms
{
    public partial class StoreView : System.Web.UI.Page
    {
        string strFormCode = "StoreView";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                lblMessage.Text = string.Empty;
                objSession = (clsSession)Session["clsSession"];
                txtEffectFrom.Attributes.Add("readonly", "readonly");

                CalendarExtender1.EndDate = System.DateTime.Now;

                if (!IsPostBack)
                {
                     CheckAccessRights("4");
                     LoadStoreDetails();
                    
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
             
        }
        public void LoadStoreDetails()
        {

            try
            {
                clsStore ObjStore = new clsStore();
                ObjStore.sOfficeCode = objSession.OfficeCode;
                DataTable dtStoreDetails = ObjStore.LoadStoreGrid(ObjStore);
                if (dtStoreDetails.Rows.Count == 0)
                {

                    ShowEmptyGrid();
                    ViewState["Store"] = dtStoreDetails;
                }
                else
                {
                    grdStore.DataSource = dtStoreDetails;
                    grdStore.DataBind();
                    ViewState["Store"] = dtStoreDetails;
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
                Response.Redirect("StoreCreate.aspx",false);

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
        protected void grdStore_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdStore.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Store"];
                grdStore.DataSource = SortDataTable(dt as DataTable, true);
                grdStore.DataBind();
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }
        protected void grdStore_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdStore.PageIndex;
            DataTable dt = (DataTable)ViewState["Store"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                //ViewState["Store"] = dt;
                grdStore.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
               // ViewState["Store"] = dt;
                grdStore.DataSource = dt;
            }
            grdStore.DataBind();
            grdStore.PageIndex = pageIndex;
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
                        ViewState["Store"] = dataView.ToTable();


                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["Store"] = dataView.ToTable();


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

        protected void grdStore_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "create")
                {

                    //Check AccessRights
                    bool bAccResult = CheckAccessRights("3");
                    if (bAccResult == false)
                    {
                        return;
                    }

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                    string  strStoreId = ((Label)row.FindControl("lblStoreId")).Text;
                    strStoreId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(strStoreId));
                    Response.Redirect("StoreCreate.aspx?QryStoreId=" + strStoreId + "",false);
                }
                if (e.CommandName == "status")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    string sStoreId = ((Label)row.FindControl("lblStoreId")).Text;
                    string sStatus = ((Label)row.FindControl("lblStatus")).Text;

                    ImageButton imgDeactive = new ImageButton();
                    ImageButton imgActive = new ImageButton();
                    imgDeactive = (ImageButton)row.FindControl("imgDeactive");
                    imgActive = (ImageButton)row.FindControl("imgActive");

                    clsStore objStore = new clsStore();
                    objStore.sStoreCode = sStoreId;
                    ViewState["SM_ID"] = sStoreId;
                    ViewState["SM_STATUS1"] = sStatus;
                    this.mdlPopup.Show();


                    //// Check Access Rights
                    //bool bAccResult = CheckAccessRights("3");
                    //if (bAccResult == false)
                    //{
                    //    return;
                    //}

                    //GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    //string sStoreId = ((Label)row.FindControl("lblStoreId")).Text;
                    //string sStatus = ((Label)row.FindControl("lblStatus")).Text;

                    //ImageButton imgDeactive;
                    //ImageButton imgActive;
                  

                    //imgDeactive = (ImageButton)row.FindControl("imgDeactive");
                    //imgActive = (ImageButton)row.FindControl("imgActive");
                  

                    //clsStore objStore = new clsStore();
                    //objStore.sSlNo = sStoreId;

                    //if (sStatus == "A")
                    //{
                    //    objStore.sStatus = "D";
                    //    bool bResult = objStore.ActiveDeactiveStore(objStore);
                    //    if (bResult == true)
                    //    {
                    //        imgDeactive.Visible = true;
                    //        imgActive.Visible = false;
                          
                    //        ShowMsgBox("Store Deactivated Successfully");
                    //        LoadStoreDetails();
                    //        return;
                    //    }
                    //}
                    //if (sStatus == "D")
                    //{
                    //    objStore.sStatus = "A";
                    //    bool bResult = objStore.ActiveDeactiveStore(objStore);
                    //    if (bResult == true)
                    //    {
                    //        imgDeactive.Visible = false;
                    //        imgActive.Visible = true;
                          
                    //        ShowMsgBox("Store Activated Successfully");
                    //        LoadStoreDetails();
                    //        return;
                    //    }
                    //}
                }
                LoadStoreDetails();
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtStoreName = (TextBox)row.FindControl("txtStoreName");
                    TextBox txtLocation = (TextBox)row.FindControl("txtLocation");

                    DataTable dt = (DataTable)ViewState["Store"];
                    dv = dt.DefaultView;

                    if (txtStoreName.Text != "")
                    {
                        sFilter = "SM_NAME Like '%" + txtStoreName.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtLocation.Text != "")
                    {
                        sFilter += " SM_OFF_CODE Like '%" + txtLocation.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdStore.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdStore.DataSource = dv;
                            ViewState["Store"] = dv.ToTable();
                            grdStore.DataBind();
                           
                        }
                        else
                        {
                            ViewState["Store"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadStoreDetails();
                    }

            
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }


        public void  ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("SM_ID");
                dt.Columns.Add("SM_NAME");
                dt.Columns.Add("SM_OFF_CODE");
                dt.Columns.Add("SM_MOBILENO");
                dt.Columns.Add("SM_STORE_INCHARGE");
                dt.Columns.Add("SM_EMAILID");
                dt.Columns.Add("SM_STATUS1");

                grdStore.DataSource = dt;
                grdStore.DataBind();

                int iColCount = grdStore.Rows[0].Cells.Count;
                grdStore.Rows[0].Cells.Clear();
                grdStore.Rows[0].Cells.Add(new TableCell());
                grdStore.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdStore.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }

        protected void grdStore_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblStatus;
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
                        imgBtnEdit.ToolTip = "Store is DeActivated,You Cannot Edit";
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

                objApproval.sFormName = "StoreCreate";
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

        protected void cmdSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateEnableDisable() == true)
                {
                    clsStore objStore = new clsStore();
                    objStore.sReason = txtReason.Text;
                    objStore.sEffectFrom = txtEffectFrom.Text;
                    objStore.sSlNo = Convert.ToString(ViewState["SM_ID"]);
                    objStore.sStatus = Convert.ToString(ViewState["SM_STATUS1"]);
                    ImageButton imgDeactive = new ImageButton();
                    ImageButton imgActive = new ImageButton();
                    if (objStore.sStatus == "A")
                    {
                        objStore.sStatus = "D";
                        bool bResult = objStore.ActiveDeactiveStore(objStore);
                        if (bResult == true)
                        {
                            imgDeactive.Visible = true;
                            imgActive.Visible = false;
                            ShowMsgBox("Store Deactivated Successfully");
                            LoadStoreDetails();
                            txtEffectFrom.Text = "";
                            txtReason.Text = "";

                        }
                    }
                    else
                    {
                        objStore.sStatus = "A";
                        bool bResult = objStore.ActiveDeactiveStore(objStore);
                        if (bResult == true)
                        {
                            imgDeactive.Visible = false;
                            imgActive.Visible = true;
                            ShowMsgBox("Store Activated Successfully");
                            LoadStoreDetails();
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

        //protected void grdStore_Sorting(object sender, GridViewSortEventArgs e)
        //{
        //    DataTable dataTable = ViewState["Store"] as DataTable;
        //    string sortingDirection = string.Empty;
        //    if (direction == SortDirection.Ascending)
        //    {
        //        direction = SortDirection.Descending;
        //        sortingDirection = "Desc";
        //        grdStore.HeaderStyle.CssClass = "descending";

        //    }
        //    else
        //    {
        //        direction = SortDirection.Ascending;
        //        sortingDirection = "Asc";
        //        grdStore.HeaderStyle.CssClass = "ascending";

        //    }
        //    DataView sortedView = new DataView(dataTable);
        //    sortedView.Sort = e.SortExpression + " " + sortingDirection;
        //    Session["SortedView"] = sortedView;
        //    grdStore.DataSource = sortedView;
        //    grdStore.DataBind();
        //}

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

        protected void Export_clickStore(object sender, EventArgs e)
        {

            clsStore ObjStore = new clsStore();
            //  ObjStore.sOfficeCode = objSession.OfficeCode;
            // DataTable dtStoreDetails = ObjStore.LoadStoreGrid(ObjStore);

            DataTable dtStoreDetails = (DataTable)ViewState["Store"];

            if (dtStoreDetails.Rows.Count > 0)
            {

                dtStoreDetails.Columns["SM_OFF_CODE"].ColumnName = "STORE NAME";
                dtStoreDetails.Columns["SM_MOBILENO"].ColumnName = "MOBILE NO  ";
                dtStoreDetails.Columns["SM_NAME"].ColumnName = "LOCATION";
                dtStoreDetails.Columns["SM_STORE_INCHARGE"].ColumnName = "INCHARGE NAME";
                dtStoreDetails.Columns["SM_EMAILID"].ColumnName = "EMAIL ID";

                dtStoreDetails.Columns["LOCATION"].SetOrdinal(2);
                List<string> listtoRemove = new List<string> { "SM_ID", "SM_STATUS", "SM_STATUS1" };
                string filename = "StoreDetails" + DateTime.Now + ".xls";
                string pagetitle = "Store Details";

                Genaral.getexcel(dtStoreDetails, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");
                ShowEmptyGrid();
            }



        }

    }
}