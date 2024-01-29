using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Collections;

namespace IIITS.DTLMS.TCRepair
{
    public partial class TestPendingSearch : System.Web.UI.Page
    {
        string strFormCode = "TestPendingSearch";
        clsSession objSession;

        /// <summary>
        /// for load the data of pending to test form
        /// current user details
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
                lblMessage.Text = string.Empty;
                objSession = (clsSession)Session["clsSession"];
                if (!IsPostBack)
                {
                    if (CheckAccessRights("4"))
                    {
                        LoadComboFiled();
                        LoadTestingPendingDetails();

                        if(objSession.RoleId== Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                        {
                            txtsstoreid.Text = clsStoreOffice.GetStoreID(objSession.OfficeCode);
                        }
                        string strQry = string.Empty;

                        if (objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                        {
                            strQry = "Title=Search and Select Purchase Order Details&";
                            strQry += "Query= SELECT * FROM (SELECT DISTINCT \"RSM_PO_NO\" ,\"RSM_INV_NO\",\"DIV_NAME\" FROM \"TBLREPAIRSENTMASTER\",\"TBLREPAIRSENTDETAILS\",";
                            strQry += "\"TBLREPAIRERINVOICE\",\"TBLTCMASTER\",\"TBLDIVISION\"  WHERE CAST(\"RSM_DIV_CODE\" AS TEXT) LIKE '" + objSession.sStoreID + "' AND ";
                            strQry += "\"RSM_ID\"=\"RSD_RSM_ID\" AND \"RSD_TC_CODE\" = \"TC_CODE\" AND \"TC_CURRENT_LOCATION\"=3  AND  ";
                            strQry += "\"RSM_ID\"=\"RI_RSM_ID\"  and \"DIV_CODE\"=\"RSM_DIV_CODE\"    AND CAST(\"RSD_DELIVARY_DATE\" AS TEXT) IS NULL";
                            strQry += " UNION  ";
                            strQry += "SELECT DISTINCT \"RSM_PO_NO\" ,\"RSM_INV_NO\" ,\"DIV_NAME\"  FROM \"TBLREPAIRSENTMASTER\",\"TBLREPAIRSENTDETAILS\",";
                            strQry += "\"TBLREPAIRERINVOICE\",\"TBLTCMASTER\",\"TBLDIVISION\",\"TBLTRANSREPAIRER\" WHERE   \"RSM_ID\"=\"RSD_RSM_ID\" ";
                            strQry += "AND \"RSD_TC_CODE\" = \"TC_CODE\" AND \"TC_CURRENT_LOCATION\"=3  AND  \"RSM_ID\"=\"RI_RSM_ID\"  and ";
                            strQry += "\"DIV_CODE\"=\"RSM_DIV_CODE\"  AND \"TR_ID\"=\"RSM_SUPREP_ID\"   AND CAST(\"RSD_DELIVARY_DATE\" AS TEXT) IS NULL AND \"TR_MEGA_R_FLAG\"=1)A WHERE {0} like %{1}% &";
                            strQry += "DBColName=CAST(\"RSM_PO_NO\" AS TEXT)~CAST(\"RSM_INV_NO\" AS TEXT)~CAST(\"DIV_NAME\" AS TEXT)&";
                            strQry += "ColDisplayName=PO No~Invoice No~Division Name&";
                        }
                        else
                        {
                            if(objSession.RoleId=="43")
                            {
                                strQry = "Title=Search and Select Purchase Order Details&";
                                strQry += "Query= SELECT * FROM (SELECT DISTINCT \"RSM_PO_NO\" ,\"RSM_INV_NO\",\"DIV_NAME\" FROM \"TBLREPAIRSENTMASTER\",\"TBLREPAIRSENTDETAILS\",";
                                strQry += "\"TBLREPAIRERINVOICE\",\"TBLTCMASTER\",\"TBLDIVISION\"  WHERE CAST(\"RSM_DIV_CODE\" AS TEXT) LIKE '" + objSession.sStoreID + "' AND ";
                                strQry += "\"RSM_ID\"=\"RSD_RSM_ID\" AND \"RSD_TC_CODE\" = \"TC_CODE\" AND \"TC_CURRENT_LOCATION\"=3  AND  ";
                                strQry += "\"RSM_ID\"=\"RI_RSM_ID\"  and \"DIV_CODE\"=\"RSM_DIV_CODE\"    AND CAST(\"RSD_DELIVARY_DATE\" AS TEXT) IS NULL";
                                strQry += " UNION  ";
                                strQry += "SELECT DISTINCT \"RSM_PO_NO\" ,\"RSM_INV_NO\" ,\"DIV_NAME\"  FROM \"TBLREPAIRSENTMASTER\",\"TBLREPAIRSENTDETAILS\",";
                                strQry += "\"TBLREPAIRERINVOICE\",\"TBLTCMASTER\",\"TBLDIVISION\",\"TBLTRANSREPAIRER\" WHERE   \"RSM_ID\"=\"RSD_RSM_ID\" ";
                                strQry += "AND \"RSD_TC_CODE\" = \"TC_CODE\" AND \"TC_CURRENT_LOCATION\"=3  AND  \"RSM_ID\"=\"RI_RSM_ID\"  and ";
                                strQry += "\"DIV_CODE\"=\"RSM_DIV_CODE\"  AND \"TR_ID\"=\"RSM_SUPREP_ID\"   AND CAST(\"RSD_DELIVARY_DATE\" AS TEXT) IS NULL AND \"TR_MEGA_R_FLAG\"=1)A WHERE {0} like %{1}% &";
                                strQry += "DBColName=CAST(\"RSM_PO_NO\" AS TEXT)~CAST(\"RSM_INV_NO\" AS TEXT)~CAST(\"DIV_NAME\" AS TEXT)&";
                                strQry += "ColDisplayName=PO No~Invoice No~Division Name&";

                            }
                            else
                            {
                                // for future ref
                                //strQry = "Title=Search and Select Purchase Order Details&";
                                //strQry += "Query=SELECT DISTINCT \"RSM_PO_NO\",\"RSM_INV_NO\" FROM \"TBLREPAIRSENTMASTER\",\"TBLREPAIRSENTDETAILS\",\"TBLREPAIRERINVOICE\",\"TBLTCMASTER\" WHERE CAST(\"RSM_DIV_CODE\" AS TEXT) ";
                                ////strQry += "LIKE '" + clsStoreOffice.GetStoreID(objSession.sStoreID) + "' AND \"RSM_ID\"=\"RSD_RSM_ID\" AND \"RSD_TC_CODE\" = \"TC_CODE\" AND \"TC_CURRENT_LOCATION\"=3 ";
                                //strQry += "LIKE '" + objSession.sStoreID + "' AND \"RSM_ID\"=\"RSD_RSM_ID\" AND \"RSD_TC_CODE\" = \"TC_CODE\" AND \"TC_CURRENT_LOCATION\"=3  AND  \"RSM_ID\"=\"RI_RSM_ID\" ";
                                //strQry += " AND CAST(\"RSD_DELIVARY_DATE\" AS TEXT) IS NULL and {0} like %{1}% &";
                                //strQry += "DBColName=CAST(\"RSM_PO_NO\" AS TEXT)~CAST(\"RSM_INV_NO\" AS TEXT)&";
                                //strQry += "ColDisplayName=PO No~Invoice No&";

                                strQry = "Title=Search and Select Purchase Order Details&";
                                strQry += "Query= SELECT * FROM (SELECT DISTINCT \"RSM_PO_NO\" ,\"RSM_INV_NO\",\"DIV_NAME\" FROM \"TBLREPAIRSENTMASTER\",\"TBLREPAIRSENTDETAILS\",";
                                strQry += "\"TBLREPAIRERINVOICE\",\"TBLTCMASTER\",\"TBLDIVISION\"  WHERE CAST(\"RSM_DIV_CODE\" AS TEXT) LIKE '" + objSession.sStoreID + "' AND ";
                                strQry += "\"RSM_ID\"=\"RSD_RSM_ID\" AND \"RSD_TC_CODE\" = \"TC_CODE\" AND \"TC_CURRENT_LOCATION\"=3  AND  ";
                                strQry += "\"RSM_ID\"=\"RI_RSM_ID\"  and \"DIV_CODE\"=\"RSM_DIV_CODE\"    AND CAST(\"RSD_DELIVARY_DATE\" AS TEXT) IS NULL";
                                strQry += " UNION  ";
                                strQry += "SELECT DISTINCT \"RSM_PO_NO\" ,\"RSM_INV_NO\" ,\"DIV_NAME\"  FROM \"TBLREPAIRSENTMASTER\",\"TBLREPAIRSENTDETAILS\",";
                                strQry += "\"TBLREPAIRERINVOICE\",\"TBLTCMASTER\",\"TBLDIVISION\",\"TBLTRANSREPAIRER\" WHERE   \"RSM_ID\"=\"RSD_RSM_ID\" ";
                                strQry += "AND \"RSD_TC_CODE\" = \"TC_CODE\" AND \"TC_CURRENT_LOCATION\"=3  AND  \"RSM_ID\"=\"RI_RSM_ID\"  and ";
                                strQry += "\"DIV_CODE\"=\"RSM_DIV_CODE\"  AND \"TR_ID\"=\"RSM_SUPREP_ID\"   AND CAST(\"RSD_DELIVARY_DATE\" AS TEXT) IS NULL AND \"TR_MEGA_R_FLAG\"=1)A WHERE {0} like %{1}% &";
                                strQry += "DBColName=CAST(\"RSM_PO_NO\" AS TEXT)~CAST(\"RSM_INV_NO\" AS TEXT)~CAST(\"DIV_NAME\" AS TEXT)&";
                                strQry += "ColDisplayName=PO No~Invoice No~Division Name&";

                            }
                        }
                        strQry = strQry.Replace("'", @"\'");

                        cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtPONo.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + txtPONo.ClientID + ")");
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadComboFiled()
        {
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT \"TM_ID\",\"TM_NAME\" FROM \"TBLTRANSMAKES\" ORDER BY \"TM_NAME\"";
                Genaral.Load_Combo(strQry, "--SELECT--", cmbMake);
                Genaral.Load_Combo("SELECT \"MD_NAME\",\"MD_NAME\" from \"TBLMASTERDATA\" WHERE \"MD_TYPE\" ='C' ORDER BY \"MD_ORDER_BY\" ", "--Select--", cmbCapacity);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// for load the test pending tc detials
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                LoadTestingPendingTCDetails();
              
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        private void LoadTestingPendingDetails()
        {
            try
            {
                clsDTrRepairActivity objDeliverPending = new clsDTrRepairActivity();

                objDeliverPending.sOfficeCode = objSession.OfficeCode;
                objDeliverPending.sTestingDone = "1";

                objDeliverPending.sPurchaseOrderNo = "";
                DataTable dt = new DataTable();
                dt = objDeliverPending.LoadPendingForTestingDetails(objDeliverPending);

                grdTestPending.DataSource = dt;
                grdTestPending.DataBind();
                ViewState["TestPending"] = dt;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// for load the test pending tc detials
        /// </summary>
        private void LoadTestingPendingTCDetails()
        {
            try
            {
                clsDTrRepairActivity objTestpending = new clsDTrRepairActivity();

                if (cmbCapacity.SelectedIndex > 0)
                {
                    objTestpending.sCapacity = cmbCapacity.SelectedValue;
                }
                if (cmbMake.SelectedIndex > 0)
                {
                    objTestpending.sMakeId = cmbMake.SelectedValue;
                }
                
                objTestpending.sPurchaseOrderNo = txtPONo.Text.Trim();
                objTestpending.sOfficeCode = objSession.OfficeCode;
                objTestpending.sTestingDone = "0";

                DataTable dt = new DataTable();
                dt = objTestpending.LoadTestOrDeliverPendingDTR(objTestpending);
                if (dt.Rows.Count > 0)
                {
                    ViewState["testpending"] = dt;
                    grdPendingTc.DataSource = SortDataTable(dt as DataTable, false);
                    grdPendingTc.DataBind();
                    cmdDeliver.Visible = true;
                    grdPendingTc.Visible = true;
                }
                else
                {
                    ViewState["testpending"] = dt;
                    cmdDeliver.Visible = false;
                    grdPendingTc.DataSource = dt;//sort;
                    grdPendingTc.DataBind();
                }

               
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void Export_ClickTestPendingSearch(object sender, EventArgs e)
        {
            cmdLoad_Click(sender,e);

            DataTable dt = (DataTable)ViewState["testpending"];
                if (dt.Rows.Count > 0 || dt == null)
                {

                    dt.Columns["TC_CODE"].ColumnName = "DTR CODE";
                    dt.Columns["TC_SLNO"].ColumnName = "DTR SLNO";
                    dt.Columns["TM_NAME"].ColumnName = "MAKE NAME";
                    dt.Columns["CAPACITY"].ColumnName = "CAPACITY(IN KVA)";
                    dt.Columns["RSM_ISSUE_DATE"].ColumnName = "ISSUE DATE";
                    dt.Columns["RSM_PO_NO"].ColumnName = "PO NO";
                   
                    List<string> listtoRemove = new List<string> { "RSD_ID", "SUP_REPNAME" };
                    string filename = "TestPendingDetails" + DateTime.Now + ".xls";
                    string pagetitle = "Test Pending Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                }
            
        }

        protected void grdTestPending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdTestPending.PageIndex = e.NewPageIndex;
                PopulateCheckedValues();
                DataTable dt = (DataTable)ViewState["TestPending"];
                grdTestPending.DataSource = SortDataTablePending(dt as DataTable, true);
                grdTestPending.DataBind();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdTestPending_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdTestPending.PageIndex;
            DataTable dt = (DataTable)ViewState["TestPending"];
            string sortingDirection = string.Empty;

            grdTestPending.DataSource = SortDataTablePending(dt as DataTable, false);
            grdTestPending.DataBind();
            grdTestPending.PageIndex = pageIndex;
        }

        protected DataView SortDataTablePending(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        ViewState["TestPending"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["TestPending"] = dataView.ToTable();

                    }

                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }

        protected void Export_ClickPendingTesting(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            if (ViewState["TestPending"] != null)
            {
                dt = (DataTable)ViewState["TestPending"];
            }
            else
            {
                clsDTrRepairActivity objDeliverPending = new clsDTrRepairActivity();

                objDeliverPending.sOfficeCode = objSession.OfficeCode;
                objDeliverPending.sTestingDone = "1";

                objDeliverPending.sPurchaseOrderNo = "";

                dt = objDeliverPending.LoadPendingForTestingDetails(objDeliverPending);
            }
            if (dt.Rows.Count > 0)
            {
                dt.Columns["PONO"].ColumnName = "PO No";
                dt.Columns["PODATE"].ColumnName = "PO Date  ";
                dt.Columns["ISSUEDATE"].ColumnName = "Issue Date";
                dt.Columns["SUP_REPNAME"].ColumnName = "Supplier/Repairer";
                dt.Columns["PO_QUANTITY"].ColumnName = "Total Quantity";
                dt.Columns["PENDING_QNTY"].ColumnName = "Pending Qty for Testing";
                List<string> listtoRemove = new List<string> { "DELIVERED_QNTY" };
                string filename = "PendingForTestingDetails" + DateTime.Now + ".xls";
                string pagetitle = "Pending For Testing Details";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");
            }

        }

        protected void cmdDeliver_Click(object sender, EventArgs e)
        {
            try
            {

                //Check AccessRights
                //bool bAccResult = CheckAccessRights("2");
                //if (bAccResult == false)
                //{
                //    return;
                //}
                if(objSession.RoleId=="43")
                {
                    ShowMsgBox("Sorry You Are Not Authorized To Access");
                    return;
                }

                bool AtleastOneApp = false;
                int i = 0;
                string[] Arr = new string[3];
                grdPendingTc.AllowPaging = false;
                SaveCheckedValues();
                LoadTestingPendingTCDetails();

                PopulateCheckedValues();
                string[] strQryVallist = new string[grdPendingTc.Rows.Count];
                foreach (GridViewRow row in grdPendingTc.Rows)
                {
                  
                    if (((CheckBox)row.FindControl("chkSelect")).Checked == true)
                    {
                        strQryVallist[i] = ((Label)row.FindControl("lblRepairDetailsId")).Text.Trim();
                        AtleastOneApp = true;
                        txtPONo.Text = ((Label)row.FindControl("lblPONo")).Text.Trim();
                    }
                    i++;

                }

                grdPendingTc.AllowPaging = true;
                SaveCheckedValues();
                LoadTestingPendingTCDetails();
                PopulateCheckedValues();

                if (!AtleastOneApp)
                {
                    ShowMsgBox("Please Select DTr to Inspect");
                    SaveCheckedValues();
                    LoadTestingPendingTCDetails();
                    PopulateCheckedValues();
                    return;
                }

                string sSelectedValue = string.Empty;
                for (int j = 0; j < strQryVallist.Length; j++)
                {
                    if (strQryVallist[j] != null)
                    {
                        sSelectedValue += strQryVallist[j].ToString() + "~";
                    }
                }
                string sRepairDetailsId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sSelectedValue));
                Session["RepairDetailsId"] = sSelectedValue;
                string sPONo = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtPONo.Text));
                Response.Redirect("TCTesting.aspx?PoNo=" + sPONo, false);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdPendingTc_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try 
            {
                SaveCheckedValues();
                grdPendingTc.PageIndex = e.NewPageIndex;
                LoadTestingPendingTCDetails();
                PopulateCheckedValues();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdPendingTc_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdPendingTc.PageIndex;
            DataTable dt = (DataTable)ViewState["testpending"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {

                grdPendingTc.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdPendingTc.DataSource = dt;
            }
            grdPendingTc.DataBind();
            grdPendingTc.PageIndex = pageIndex;
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
                        ViewState["testpending"] = dataView.ToTable();


                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["testpending"] = dataView.ToTable();


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

        private void PopulateCheckedValues()
        {
            try
            {
                ArrayList arrCheckedValues = (ArrayList)ViewState["CHECKED_ITEMS"];
                if (arrCheckedValues != null && arrCheckedValues.Count > 0)
                {
                    foreach (GridViewRow gvrow in grdPendingTc.Rows)
                    {
                        int index = Convert.ToInt32(grdPendingTc.DataKeys[gvrow.RowIndex].Values[0]);
                        if (arrCheckedValues.Contains(index))
                        {
                            CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkSelect");
                            myCheckBox.Checked = true;
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


        //This method is used to save the checkedstate of values
        private void SaveCheckedValues()
        {
            try
            {
                ArrayList arrCheckedValues = new ArrayList();
                int index = -1;
                foreach (GridViewRow gvrow in grdPendingTc.Rows)
                {
                    index = Convert.ToInt32(grdPendingTc.DataKeys[gvrow.RowIndex].Values[0]); ;

                    bool result = ((CheckBox)gvrow.FindControl("chkSelect")).Checked;

                    // Check in the viewstate
                    if (ViewState["CHECKED_ITEMS"] != null)
                        arrCheckedValues = (ArrayList)ViewState["CHECKED_ITEMS"];
                    if (result)
                    {
                        if (!arrCheckedValues.Contains(index))
                            arrCheckedValues.Add(index);
                    }
                    else
                        arrCheckedValues.Remove(index);
                }
                if (arrCheckedValues != null && arrCheckedValues.Count > 0)
                    ViewState["CHECKED_ITEMS"] = arrCheckedValues;

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

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                cmbCapacity.SelectedIndex = 0;
                //txtNoofDays.Text = string.Empty;
                txtPONo.Text = string.Empty;
                //cmbRepairer.SelectedIndex = 0;
                cmbMake.SelectedIndex = 0;
                //LoadPendingDetails();
                grdPendingTc.DataSource = null;
                grdPendingTc.DataBind();
                //cmbSupplier.SelectedIndex = 0;
                grdPendingTc.Visible = false;
                cmdDeliver.Visible = false;
              
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

                objApproval.sFormName = "TestPendingSearch";
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
    }
}