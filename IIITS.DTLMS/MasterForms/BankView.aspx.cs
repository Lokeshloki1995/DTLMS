using IIITS.DTLMS.BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.MasterForms
{
    public partial class BankView : System.Web.UI.Page
    {
        string strFormCode = "BankView";
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
               // txtEffectFrom.Attributes.Add("readonly", "readonly");

              //  CalendarExtender1.EndDate = System.DateTime.Now;

                if (!IsPostBack)
                {
                    CheckAccessRights("4");
                    LoadBankDetails();

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
                Response.Redirect("BankCreate.aspx", false);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdBank_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdBank.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Bank"];
                grdBank.DataSource = SortDataTable(dt as DataTable, true);
                grdBank.DataBind();
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }
        protected void grdBank_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdBank.PageIndex;
            DataTable dt = (DataTable)ViewState["Bank"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                //ViewState["Store"] = dt;
                grdBank.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                // ViewState["Store"] = dt;
                grdBank.DataSource = dt;
            }
            grdBank.DataBind();
            grdBank.PageIndex = pageIndex;
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
                        ViewState["Bank"] = dataView.ToTable();


                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["Bank"] = dataView.ToTable();


                    }


                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

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
        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "ASC"; }
            set { ViewState["SortDirection"] = value; }


        }
        protected void grdBank_RowCommand(object sender, GridViewCommandEventArgs e)
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

                    string strBankId = ((Label)row.FindControl("lblBankId")).Text;
                    strBankId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(strBankId));
                    Response.Redirect("BankCreate.aspx?QryBankId=" + strBankId + "", false);
                }
                if (e.CommandName == "status")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    string sBankId = ((Label)row.FindControl("lblBankId")).Text;
                    string sStatus = ((Label)row.FindControl("lblStatus")).Text;

                    ImageButton imgDeactive = new ImageButton();
                    ImageButton imgActive = new ImageButton();
                    imgDeactive = (ImageButton)row.FindControl("imgDeactive");
                    imgActive = (ImageButton)row.FindControl("imgActive");

                    clsBank objBAnk = new clsBank();
                    objBAnk.sBankCode = sBankId;
                    ViewState["BM_ID"] = sBankId;
                    ViewState["BM_STATUS"] = sStatus;
                    //this.mdlPopup.Show();


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
                LoadBankDetails();
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtBankName = (TextBox)row.FindControl("txtBankName");
                    TextBox txtLocation = (TextBox)row.FindControl("txtLocation");

                    DataTable dt = (DataTable)ViewState["Bank"];
                    dv = dt.DefaultView;

                    if (txtBankName.Text != "")
                    {
                        sFilter = "BM_NAME Like '%" + txtBankName.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtLocation.Text != "")
                    {
                        sFilter += " BM_SUBDIV_CODE Like '%" + txtLocation.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdBank.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdBank.DataSource = dv;
                            ViewState["Bank"] = dv.ToTable();
                            grdBank.DataBind();

                        }
                        else
                        {
                            ViewState["Bank"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadBankDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }
        public void LoadBankDetails()
        {

            try
            {
                clsBank ObjBank = new clsBank();
                ObjBank.sOfficeCode = objSession.OfficeCode;
                DataTable dtBankDetails = ObjBank.LoadBankGrid(ObjBank);
                if (dtBankDetails.Rows.Count == 0)
                {

                    ShowEmptyGrid();
                    ViewState["Bank"] = dtBankDetails;
                }
                else
                {
                    grdBank.DataSource = dtBankDetails;
                    grdBank.DataBind();
                    ViewState["Bank"] = dtBankDetails;
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
                dt.Columns.Add("BM_ID");
                dt.Columns.Add("BM_NAME");
                dt.Columns.Add("BM_SUBDIV_CODE");
                dt.Columns.Add("BM_STORE_ID");
                dt.Columns.Add("BM_BANK_INCHARGE");
                dt.Columns.Add("BM_EMAILID");
                dt.Columns.Add("BM_STATUS");
                dt.Columns.Add("BM_MOBILENO");

                grdBank.DataSource = dt;
                grdBank.DataBind();

                int iColCount = grdBank.Rows[0].Cells.Count;
                grdBank.Rows[0].Cells.Clear();
                grdBank.Rows[0].Cells.Add(new TableCell());
                grdBank.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdBank.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }
        protected void grdBank_RowDataBound(object sender, GridViewRowEventArgs e)
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
                        imgBtnEdit.ToolTip = "Bank is DeActivated,You Cannot Edit";
                    }
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);


            }
        }
        protected void cmdSubmit_Click(object sender, EventArgs e)
        {
            try
            {               
                    clsBank objBank = new clsBank();
                  //  objStore.sReason = txtReason.Text;
                    //bjStore.sEffectFrom = txtEffectFrom.Text;
                    objBank.sSlNo = Convert.ToString(ViewState["BM_ID"]);
                    objBank.sStatus = Convert.ToString(ViewState["BM_STATUS"]);
                    ImageButton imgDeactive = new ImageButton();
                    ImageButton imgActive = new ImageButton();
                    if (objBank.sStatus == "A")
                    {
                        objBank.sStatus = "D";
                        bool bResult = objBank.ActiveDeactiveBank(objBank);
                        if (bResult == true)
                        {
                            imgDeactive.Visible = true;
                            imgActive.Visible = false;
                            ShowMsgBox("Bank Deactivated Successfully");
                            LoadBankDetails();
                           // txtEffectFrom.Text = "";
                           /// txtReason.Text = "";

                        }
                    }
                    else
                    {
                        objBank.sStatus = "A";
                        bool bResult = objBank.ActiveDeactiveBank(objBank);
                        if (bResult == true)
                        {
                            imgDeactive.Visible = false;
                            imgActive.Visible = true;
                            ShowMsgBox("Bank Activated Successfully");
                            LoadBankDetails();
                            //txtEffectFrom.Text = "";
                           // txtReason.Text = "";
                        }
                    }
                


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        //public bool ValidateEnableDisable()
        //{
        //    bool bValidate = false;
        //    try
        //    {
        //        if (txtEffectFrom.Text.Trim() == "")
        //        {
        //            lblMsg.Text = "Enter Effect From";
        //            txtEffectFrom.Focus();
        //            mdlPopup.Show();
        //            return bValidate;
        //        }
        //        if (txtReason.Text.Trim() == "")
        //        {
        //            lblMsg.Text = "Enter Reason";
        //            txtReason.Focus();
        //            mdlPopup.Show();
        //            return bValidate;
        //        }

        //      //  string sResult = Genaral.DateComparision(txtEffectFrom.Text, "", true, false);
        //        if (sResult == "2")
        //        {
        //            ShowMsgBox("Effect From Date should be Greater than Current Date");
        //            txtEffectFrom.Focus();
        //            mdlPopup.Show();
        //            return bValidate;
        //        }

        //        bValidate = true;
        //        return bValidate;
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return bValidate;
        //    }
        //}
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
        protected void Export_clickBank(object sender, EventArgs e)
        {

            clsBank ObjBank = new clsBank();
            //  ObjStore.sOfficeCode = objSession.OfficeCode;
            // DataTable dtStoreDetails = ObjStore.LoadStoreGrid(ObjStore);

            DataTable dtBankDetails = (DataTable)ViewState["Bank"];

            if (dtBankDetails.Rows.Count > 0)
            {
                dtBankDetails.Columns["BM_NAME"].ColumnName = "BANK NAME";
                dtBankDetails.Columns["BM_SUBDIV_CODE"].ColumnName = "SUB DIVISION";                       
                dtBankDetails.Columns["BM_BANK_INCHARGE"].ColumnName = "INCHARGE NAME";
                dtBankDetails.Columns["BM_MOBILENO"].ColumnName = "MOBILE NO";
                dtBankDetails.Columns["BM_EMAILID"].ColumnName = "EMAIL ID";

                dtBankDetails.Columns["SUB DIVISION"].SetOrdinal(2);
                List<string> listtoRemove = new List<string> { "BM_ID", "BM_STATUS" };
                string filename = "BankDetails" + DateTime.Now + ".xls";
                string pagetitle = "Bank Details";

                Genaral.getexcel(dtBankDetails, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");
                ShowEmptyGrid();
            }



        }
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
       
 
    }
}