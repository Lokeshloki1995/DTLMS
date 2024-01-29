using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IIITS.DTLMS.BL;
using System.Collections;

namespace IIITS.DTLMS.MasterForms
{
    public partial class TransSupplierView : System.Web.UI.Page
    {     
        string strFormCode = "TransSupplierView.aspx";
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
                if (!IsPostBack)
                {
                    CheckAccessRights("4");
                    LoadSupplierGrid();
                    
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void imgBtnEdit_Click(object sender, EventArgs e)
        {
            
            try
            {
                //Check AccessRights
                bool bAccResult = CheckAccessRights("3");
                if (bAccResult == false)
                {
                    return;
                }

                ImageButton imgEdit = (ImageButton)sender;
                GridViewRow rw = (GridViewRow)imgEdit.NamingContainer;
                String SupplierId = ((Label)rw.FindControl("lblSuppId")).Text;
                SupplierId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(SupplierId));
                Response.Redirect("TransSupplier.aspx?StrQryId=" + SupplierId + "",false);
            }
            
            catch(Exception ex)
            {
              
                lblMessage.Text=  clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }

        }

        public void LoadSupplierGrid()
        {

            try
            {
                clsTransSupplier objSupplier = new clsTransSupplier();
                DataTable dt = new DataTable();
                dt = objSupplier.LoadSupplierDetails();
                grdSupplierDetails.DataSource = dt;
                grdSupplierDetails.DataBind();
                ViewState["Supplier"] = dt;

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
               Response.Redirect("TransSupplier.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

      
        protected void grdSupplierDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdSupplierDetails.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Supplier"];
                grdSupplierDetails.DataSource = SortDataTable(dt as DataTable, true);
                grdSupplierDetails.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdSupplierDetails_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdSupplierDetails.PageIndex;
            DataTable dt = (DataTable)ViewState["Supplier"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdSupplierDetails.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdSupplierDetails.DataSource = dt;
            }
            grdSupplierDetails.DataBind();
            grdSupplierDetails.PageIndex = pageIndex;
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
                        ViewState["Supplier"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["Supplier"] = dataView.ToTable();

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

        public void AdminAccess()
        {
            try
            {
                if (objSession.UserType != "1")
                {
                    grdSupplierDetails.Columns[7].Visible = false;
                    cmdNew.Enabled = false;
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

                objApproval.sFormName = "TransSupplier";
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
        protected void grdSupplierDetails_RowCommand(object sender, GridViewCommandEventArgs e)
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

                    string sSupplierId = ((Label)row.FindControl("lblSuppId")).Text;
                    sSupplierId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sSupplierId));
                    Response.Redirect("TransSupplier.aspx?StrQryId=" + sSupplierId + "", false);
                }


                if (e.CommandName == "status")
                {
                    // Check Access Rights
                    bool bAccResult = CheckAccessRights("3");
                    if (bAccResult == false)
                    {
                        return;
                    }

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    string sSupplierId = ((Label)row.FindControl("lblSuppId")).Text;
                    string sStatus = ((Label)row.FindControl("lblStatus")).Text;

                    ImageButton imgDeactive;
                    ImageButton imgActive;
                    ImageButton imgBtnEdit ;
                    imgDeactive = (ImageButton)row.FindControl("imgDeactive");
                    imgActive = (ImageButton)row.FindControl("imgActive");
                    imgBtnEdit = (ImageButton)row.FindControl("imgBtnEdit");

                    clsTransSupplier objSupplier = new clsTransSupplier();
                    objSupplier.SupplierId = sSupplierId;

                    if (sStatus == "A")
                    {
                        objSupplier.sStatus = "D";
                        bool bResult = objSupplier.ActiveDeactiveSupplier(objSupplier);
                        if (bResult == true)
                        {
                            imgDeactive.Visible = true;
                            imgActive.Visible = false;
                          
                            ShowMsgBox("Supplier Deactivated Successfully");
                            LoadSupplierGrid();

                            imgBtnEdit.Enabled = false;
                            imgBtnEdit.ToolTip = "Supplier is Deactivated";
                            return;
                        }
                    }
                    if (sStatus == "D")
                    {
                        objSupplier.sStatus = "A";
                        bool bResult = objSupplier.ActiveDeactiveSupplier(objSupplier);
                        if (bResult == true)
                        {
                            imgDeactive.Visible = false;
                            imgActive.Visible = true;
                      
                            ShowMsgBox("Supplier Activated Successfully");
                            LoadSupplierGrid();

                            imgBtnEdit.Enabled = false;
                            imgBtnEdit.ToolTip = "Supplier is Activated";
                            return;
                        }
                    }
                }

                if (e.CommandName == "search")
                {
                    LoadSupplierGrid();
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtSupplierName = (TextBox)row.FindControl("txtSupplierName");


                    DataTable dt = (DataTable)ViewState["Supplier"];
                    dv = dt.DefaultView;

                    if (txtSupplierName.Text != "")
                    {
                        sFilter = "TS_NAME Like '%" + txtSupplierName.Text.Replace("'", "") + "%' AND";
                    }
                  
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdSupplierDetails.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {

                            ViewState["Supplier"] = dv.ToTable();
                            grdSupplierDetails.DataSource = dv;
                          
                            grdSupplierDetails.DataBind();

                        }
                        else
                        {
                            ViewState["Supplier"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadSupplierGrid();
                    }


                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdSupplierDetails_RowDataBound(object sender, GridViewRowEventArgs e)
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
                        imgBtnEdit.ToolTip = "Supplier is DeActivated,You Cannot Edit";
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
                dt.Columns.Add("TS_ID");
                dt.Columns.Add("TS_NAME");
                dt.Columns.Add("TS_ADDRESS");
                dt.Columns.Add("TS_PHONE");
                dt.Columns.Add("TS_EMAIL");
                dt.Columns.Add("TS_BLACK_LISTED");
                dt.Columns.Add("TS_BLACKED_UPTO");
                dt.Columns.Add("TS_STATUS");

                grdSupplierDetails.DataSource = dt;
                grdSupplierDetails.DataBind();

                int iColCount = grdSupplierDetails.Rows[0].Cells.Count;
                grdSupplierDetails.Rows[0].Cells.Clear();
                grdSupplierDetails.Rows[0].Cells.Add(new TableCell());
                grdSupplierDetails.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdSupplierDetails.Rows[0].Cells[0].Text = "No Records Found";

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

        protected void Export_clickSupplier(object sender, EventArgs e)
        {

            //clsTransSupplier objSupplier = new clsTransSupplier();
            //DataTable dt = new DataTable();
            //dt = objSupplier.LoadSupplierDetails();

            DataTable dt = (DataTable)ViewState["Supplier"];

            if (dt.Rows.Count > 0)
            {

                dt.Columns["TS_NAME"].ColumnName = "SUPPLIER NAME";
                dt.Columns["TS_PHONE"].ColumnName = "PHONE NO  ";
                dt.Columns["TS_EMAIL"].ColumnName = "EMAIL ID";
                dt.Columns["TS_BLACK_LISTED"].ColumnName = "BLOCK LISTED  ";
                dt.Columns["TS_BLACKED_UPTO"].ColumnName = "BLOCK LISTED UPTO  ";

                List<string> listtoRemove = new List<string> { "TS_ID", "TS_STATUS", "TS_COMM_ADDRESS", "TS_ADDRESS" };
                string filename = "SupplierDetails" + DateTime.Now + ".xls";
                string pagetitle = "Supplier Details";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");
                ShowEmptyGrid();
            }



        }
    }
}