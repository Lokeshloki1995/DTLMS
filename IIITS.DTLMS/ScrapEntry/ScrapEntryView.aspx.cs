using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using IIITS.DTLMS.BL;
using System.Collections;
using System.Configuration;

namespace IIITS.DTLMS.ScrapEntry
{
    public partial class ScrapEntryView : System.Web.UI.Page
    {
        string strFormCode = "ScrapEntryView";
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
                if (!IsPostBack)
                {

                    LoadComboField();
                   // GetStoreId();
                    CheckAccessRights("4");


                    string strQry = string.Empty;

                    strQry = "Title=Search and Select Work Order Details&";
                    strQry += "Query=SELECT DISTINCT \"ST_OM_NO\" FROM \"TBLSCRAPTC\",\"TBLSCRAPOBJECT\" WHERE \"ST_ID\"=\"SO_ST_ID\" AND \"SO_DISPOSAL_BY\" IS  NULL ";
                    strQry += " AND CAST(\"ST_DIV_CODE\" AS TEXT) LIKE '" + objSession.OfficeCode + "%' and {0} like %{1}% &";
                    strQry += "DBColName=CAST(\"ST_OM_NO\" AS TEXT)&";
                    strQry += "ColDisplayName=OM No&";

                    strQry = strQry.Replace("'", @"\'");

                    cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtWONo.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + txtWONo.ClientID + ")");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadComboField()
        {
            try
            {
                string sOfficeCode = string.Empty;

                int Division = Convert.ToInt32(ConfigurationSettings.AppSettings["Division_code"]);

                if (objSession.sRoleType!="2")
                {
                    sOfficeCode = clsStoreOffice.GetOfficeCode(objSession.OfficeCode, "SM_OFF_CODE");
                    Genaral.Load_Combo("SELECT \"SM_ID\",\"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"SM_STATUS\" ='A' AND CAST(\"SM_ID\" AS TEXT) = " + clsStoreOffice.GetOfficeCode(objSession.OfficeCode, "SM_OFF_CODE") + " ORDER BY \"SM_NAME\"", "--Select--", cmbStore);

                }
                else
                {
                    sOfficeCode = objSession.OfficeCode;
                    Genaral.Load_Combo("SELECT \"SM_ID\",\"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"SM_STATUS\" ='A' AND CAST(\"SM_ID\" AS TEXT) = '" + sOfficeCode + "' ORDER BY \"SM_NAME\"", "--Select--", cmbStore);

                }

                Genaral.Load_Combo("SELECT \"TM_ID\",\"TM_NAME\" FROM \"TBLTRANSMAKES\" ORDER BY \"TM_NAME\"", "--Select--", cmbMake);
                Genaral.Load_Combo("SELECT \"MD_NAME\",\"MD_NAME\" from \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='C' ORDER BY \"MD_ORDER_BY\" ", "--Select--", cmbCapacity);
                Genaral.Load_Combo("SELECT \"TS_ID\",\"TS_NAME\" FROM \"TBLTRANSSUPPLIER\"  WHERE \"TS_STATUS\"='A' AND \"TS_ID\" NOT IN (SELECT \"TS_ID\" FROM \"TBLTRANSSUPPLIER\" WHERE \"TS_BLACK_LISTED\" =1 AND \"TS_BLACKED_UPTO\" >= NOW()) ORDER BY \"TS_NAME\" ", "--Select--", cmbSupplier);

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
                LoadScarpTc();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdFaultTC_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdFaultTC.PageIndex;
            DataTable dt = (DataTable)ViewState["ScrapEntry"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdFaultTC.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdFaultTC.DataSource = dt;

            }
            grdFaultTC.DataBind();
            grdFaultTC.PageIndex = pageIndex;
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
                        if (dataView.Sort.ToString() == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["ScrapEntry"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["ScrapEntry"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["ScrapEntry"] = dataView.ToTable();
                        }
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        if (dataView.Sort.ToString() == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["ScrapEntry"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["ScrapEntry"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["ScrapEntry"] = dataView.ToTable();
                        }

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

        public void LoadScarpTc()
        {
            try
            {
                DataTable dt = new DataTable();
                clsScrap objScrap = new clsScrap();

                if (cmbCapacity.SelectedIndex > 0)
                {
                    objScrap.sCapacity = cmbCapacity.SelectedValue;
                }
                if (cmbMake.SelectedIndex > 0)
                {
                    objScrap.sMakeId = cmbMake.SelectedValue;
                }
                if (cmbStore.SelectedIndex > 0)
                {
                    objScrap.sStoreId = cmbStore.SelectedValue;
                }
                if (cmbSupplier.SelectedIndex > 0)
                {
                    objScrap.sSupplierId = cmbSupplier.SelectedValue;
                }

                objScrap.sWorkOrderNo = txtWONo.Text;

                objScrap.sOfficeCode = objSession.OfficeCode;
                dt = objScrap.LoadScrapTc(objScrap);

                ViewState["ScrapEntry"] = dt;
                grdFaultTC.DataSource = dt;
                
                grdFaultTC.DataBind();
                grdFaultTC.Visible = true;
                if (dt.Rows.Count > 0)
                {                   
                    cmdSend.Visible = true;
                }
                else
                {
                    cmdSend.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdFaultTC_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Submit")
                {
                    GridViewRow row = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
                    int rowindex = row.RowIndex;
                    Label lblTCId = (Label)grdFaultTC.Rows[rowindex].FindControl("lblTCId");
                    
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdSend_Click(object sender, EventArgs e)
        {
            try
            {

                //Check AccessRights
                bool bAccResult = CheckAccessRights("2");
                if (bAccResult == false)
                {
                    return;
                }

                bool AtleastOneApp = false;
                int i = 0;
                string[] Arr = new string[3];
                grdFaultTC.AllowPaging = false;
                SaveCheckedValues();
                LoadScarpTc();

                PopulateCheckedValues();
                string[] strQryVallist = new string[grdFaultTC.Rows.Count];
                string strScrapDetId = string.Empty;
                foreach (GridViewRow row in grdFaultTC.Rows)
                {
                    if (((CheckBox)row.FindControl("chkSelect")).Checked == true)
                    {
                        strQryVallist[i] = ((Label)row.FindControl("lblTCId")).Text.Trim();
                        strScrapDetId = ((Label)row.FindControl("lblStId")).Text.Trim();
                        AtleastOneApp = true;
                    }
                    i++;
                }

                grdFaultTC.AllowPaging = true;
                SaveCheckedValues();
                LoadScarpTc();
                PopulateCheckedValues();

                if (!AtleastOneApp)
                {
                    ShowMsgBox("Please Select DTr to Send for Dispose");
                    SaveCheckedValues();
                    LoadScarpTc();
                    PopulateCheckedValues();
                    return;
                }

                string sSelectedValue = string.Empty;
                int iTcCount = 0;
                for (int j = 0; j < strQryVallist.Length; j++)
                {
                    if (strQryVallist[j] != null)
                    {
                        sSelectedValue += strQryVallist[j].ToString() + "~";
                        iTcCount++;
                    }
                }

                string sTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sSelectedValue));
                string sTCCount = HttpUtility.UrlEncode(Genaral.UrlEncrypt(Convert.ToString(iTcCount)));
                string sScrapDetId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(Convert.ToString(strScrapDetId)));
                Session["TCId"] = sSelectedValue;
                Session["WorkorderNo"] = txtWONo.Text;
                Response.Redirect("ScrapDisposal.aspx?TcCount=" + sTCCount + "&ScrapId=" + sScrapDetId, false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        private void PopulateCheckedValues()
        {
            try
            {
                ArrayList arrCheckedValues = (ArrayList)ViewState["CHECKED_ITEMS"];
                if (arrCheckedValues != null && arrCheckedValues.Count > 0)
                {
                    foreach (GridViewRow gvrow in grdFaultTC.Rows)
                    {
                        int index = Convert.ToInt32(grdFaultTC.DataKeys[gvrow.RowIndex].Values[0]);
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
                foreach (GridViewRow gvrow in grdFaultTC.Rows)
                {
                    index = Convert.ToInt32(grdFaultTC.DataKeys[gvrow.RowIndex].Values[0]); ;

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

        protected void grdFaultTC_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                SaveCheckedValues();
                grdFaultTC.PageIndex = e.NewPageIndex;
                LoadScarpTc();
                PopulateCheckedValues();
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
                cmbMake.SelectedIndex = 0;
                //cmbStore.SelectedIndex = 0;
                cmbSupplier.SelectedIndex = 0;
                grdFaultTC.Visible = false;               
                cmdSend.Visible = false;
                txtWONo.Text = string.Empty;

                if (cmbStore.Enabled == true)
                {
                    cmbStore.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void GetStoreId()
        {
            try
            {
                clsTcMaster objTcMaster = new clsTcMaster();
                cmbStore.SelectedValue = objTcMaster.GetStoreId(objSession.OfficeCode);

                if (objSession.OfficeCode == "")
                {
                    cmbStore.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void Export_clickScrapEntry(object sender, EventArgs e)
        {

              //DataTable dt = new DataTable();
              //  clsScrap objScrap = new clsScrap();

              //  if (cmbCapacity.SelectedIndex > 0)
              //  {
              //      objScrap.sCapacity = cmbCapacity.SelectedValue;
              //  }
              //  if (cmbMake.SelectedIndex > 0)
              //  {
              //      objScrap.sMakeId = cmbMake.SelectedValue;
              //  }
              //  if (cmbStore.SelectedIndex > 0)
              //  {
              //      objScrap.sStoreId = cmbStore.SelectedValue;
              //  }
              //  if (cmbSupplier.SelectedIndex > 0)
              //  {
              //      objScrap.sSupplierId = cmbSupplier.SelectedValue;
              //  }

              //  objScrap.sWorkOrderNo = txtWONo.Text;

              //  objScrap.sOfficeCode = objSession.OfficeCode;
              //  dt = objScrap.LoadScrapTc(objScrap);

            DataTable dt = (DataTable)ViewState["ScrapEntry"];


                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "DTR CODE";
                    dt.Columns["DT_CODE"].ColumnName = "DTC CODE";
                   // dt.Columns["TC_SLNO"].ColumnName = "DTR SLNO";
                    dt.Columns["TM_NAME"].ColumnName = "MAKE NAME";
                    dt.Columns["TC_CAPACITY"].ColumnName = "CAPACITY(IN KVA)";
                    dt.Columns["TC_MANF_DATE"].ColumnName = "MANF. DATE";
                    dt.Columns["TS_NAME"].ColumnName = "SUPPLIER";



                    List<string> listtoRemove = new List<string> { "TC_ID", "ST_ID", "TC_SLNO", "SO_ID", "TC_PURCHASE_DATE", "TC_WARANTY_PERIOD" };
                    string filename = "ScrapTC" + DateTime.Now + ".xls";
                    string pagetitle = " Scrap Entry Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                }

           

        }



        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "ScrapDisposal";
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