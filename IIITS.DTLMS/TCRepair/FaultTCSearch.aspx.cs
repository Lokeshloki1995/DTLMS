using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Collections;
using System.Configuration;

namespace IIITS.DTLMS.TCRepair
{
    public partial class FaultTCSearch : System.Web.UI.Page
    {
        string strFormCode = "FaultTCSearch";
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
                    divremarks.Visible = false;
                    CheckAccessRights("4");
                    LoadComboFiled();
                    GetStoreId();
                    string stroffCode = clsStoreOffice.GetZone_Circle_Div_Offcode(objSession.OfficeCode, objSession.RoleId);
                    Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'SR'",
                        "--Select--", cmbStarRated);
                    if (objSession.OfficeCode == "")
                    {
                        Genaral.Load_Combo("select DISTINCT \"TR_ID\",\"TR_NAME\" from \"TBLREPAIRERRATES\" inner join \"TBLTRANSREPAIRER\" on \"TR_ID\"=\"RR_REP_ID\" ", "--Select--", cmbRepairer);
                    }
                    else
                    {
                        Genaral.Load_Combo("select DISTINCT \"TR_ID\",\"TR_NAME\" from \"TBLREPAIRERRATES\" inner join \"TBLTRANSREPAIRER\" on \"TR_ID\"=\"RR_REP_ID\" where \"RR_DIV_ID\"=(select \"DIV_ID\" from \"TBLDIVISION\" where \"DIV_CODE\"='" + stroffCode.Substring(0, 3) + "' ) ORDER BY \"TR_ID\"", "--Select--", cmbRepairer);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbRepairer_SelectedIndexChanged(object sender, EventArgs e)
        {
            int count = Convert.ToInt16(ConfigurationManager.AppSettings["MegaRepairersCount"]);
            string[] rep = new string[count];
            try
            {
                string megarep = Convert.ToString(ConfigurationManager.AppSettings["MegaRepairers"]);
                for (int i = 0; i < count; i++)
                {
                    rep[i] = megarep.Split(',').GetValue(i).ToString().Trim();
                }
                for (int i = 0; i < count; i++)
                {
                    //rep[i] = megarep.Split(',').GetValue(i).ToString().Trim();
                    if (cmbRepairer.SelectedValue == rep[i])
                    {
                        ShowMsgBox("You have selected Mega Repairer so please Enter Remarks");
                        divremarks.Visible = true;
                        hdnmrep.Value = "1";
                        txtRemarks.Text = string.Empty;
                        return;
                    }
                    else
                    {
                        divremarks.Visible = false;
                        hdnmrep.Value = "0";
                        txtRemarks.Text = string.Empty;
                    }

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadComboFiled()
        {
            try
            {
                string strQry = string.Empty;
                string sLocCode = clsStoreOffice.Getofficecode(objSession.OfficeCode);
                strQry = " select * from \"TBLTRANSREPAIRER\" inner join \"TBLTRANSREPAIREROFFCODE\" on \"TR_ID\"=\"TRO_TR_ID\" ";
                strQry += " and \"TRO_OFF_CODE\"='" + sLocCode + "' ORDER BY \"TR_NAME\"";


                int Division = Convert.ToInt32(ConfigurationManager.AppSettings["Division_code"]);
                string sOfficeCode = string.Empty;
                if (objSession.sRoleType != "2")
                {
                    if (objSession.OfficeCode.Length > 1)
                    {
                        sOfficeCode = objSession.OfficeCode.Substring(0, Division);
                    }
                    else
                    {
                        sOfficeCode = objSession.OfficeCode;
                    }
                    if (objSession.OfficeCode == "")
                    {
                        Genaral.Load_Combo("SELECT \"SM_ID\",\"SM_NAME\" FROM \"TBLSTOREMAST\",\"TBLSTOREOFFCODE\" WHERE \"SM_ID\"=\"STO_SM_ID\" AND CAST(\"SM_STATUS\" AS TEXT)='A' ORDER BY \"SM_NAME\"", "--Select--", cmbStore);
                    }
                    else
                    {
                        Genaral.Load_Combo("SELECT \"SM_ID\",\"SM_NAME\" FROM \"TBLSTOREMAST\",\"TBLSTOREOFFCODE\" WHERE \"SM_ID\"=\"STO_SM_ID\" AND CAST(\"SM_STATUS\" AS TEXT)='A' AND CAST(\"STO_OFF_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%' ORDER BY \"SM_NAME\"", "--Select--", cmbStore);
                    }
                }
                else
                {
                    sOfficeCode = objSession.OfficeCode;
                    Genaral.Load_Combo("SELECT \"SM_ID\",\"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE CAST(\"SM_STATUS\" AS TEXT)='A' AND CAST(\"SM_ID\" AS TEXT) LIKE '" + sOfficeCode + "%' ORDER BY \"SM_NAME\"", "--Select--", cmbStore);
                }
                if (objSession.OfficeCode == "")
                {
                    Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\"  ORDER BY \"DIV_CODE\"", "--Select--", cmbDivision);
                }
                else
                {
                    if (sOfficeCode.Length == 2)
                    {
                        sOfficeCode = clsStoreOffice.Getofficecode(sOfficeCode);
                    }
                    Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE \"DIV_CODE\"='" + sOfficeCode + "'  ORDER BY \"DIV_CODE\"", cmbDivision);
                    cmbDivision.Enabled = false;
                }
                //cmbDivision.Enabled = false;

                Genaral.Load_Combo("SELECT \"MD_NAME\",\"MD_NAME\" from \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='C' ORDER BY \"MD_ORDER_BY\"", "--Select--", cmbCapacity);
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
                if (objSession.RoleId == "43")
                {
                    if (cmbDivision.SelectedIndex == 0)
                    {
                        ShowMsgBox("Please Select the Division");
                        return;
                    }
                }
                LoadFaultTc();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadFaultTc()
        {
            try
            {
                DataTable dt = new DataTable();
                clsDTrRepairActivity objTcFailure = new clsDTrRepairActivity();



                if (cmbCapacity.SelectedIndex > 0)
                {
                    objTcFailure.sCapacity = cmbCapacity.SelectedValue;
                }
                if (cmbStore.SelectedIndex > 0)
                {
                    objTcFailure.sStoreId = cmbStore.SelectedValue;
                }
                if (cmbStarRated.SelectedIndex > 0)
                {
                    objTcFailure.sstarrate = cmbStarRated.SelectedValue;
                }
                if (cmbGuarantyType.SelectedIndex > 0)
                {
                    objTcFailure.sGuarantyType = cmbGuarantyType.SelectedValue;
                }
                if (objSession.OfficeCode.Length > 2)
                {
                    objTcFailure.sOfficeCode = objSession.OfficeCode.Substring(0, 3);
                    objTcFailure.sStoreId = clsStoreOffice.GetStoreID(objSession.OfficeCode);
                }
                else
                {
                    objTcFailure.sOfficeCode = objSession.OfficeCode;
                }

                if (objSession.RoleId == "43")
                {
                    if (cmbDivision.SelectedIndex > 0)
                    {
                        string DivCode = cmbDivision.SelectedValue;
                        objTcFailure.sStoreId = clsStoreOffice.GetStoreID(DivCode);
                    }
                    if (cmbRepairer.SelectedIndex > 0)
                    {
                        objTcFailure.sSupplierId = cmbRepairer.SelectedValue;
                    }
                }
                objTcFailure.UserId = objSession.UserId;
                objTcFailure.sroletype = objSession.sRoleType;
                objTcFailure.sroleid = objSession.RoleId;

                dt = objTcFailure.LoadFaultTCsearch(objTcFailure);
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.Add("ESTIMATION_AMOUNT", typeof(Double));
                    dt.AcceptChanges();
                    ViewState["FaultTC"] = dt;
                    grdFaultTC.DataSource = SortDataTable(dt as DataTable, true);

                    grdFaultTC.DataBind();
                    foreach (GridViewRow row in grdFaultTC.Rows)
                    {
                        Label lblstatus = (Label)row.FindControl("lblStatus");
                        if (lblstatus.Text == "ALREADY SENT")
                        {
                            ((CheckBox)row.FindControl("chkSelect")).Enabled = false;
                            ((ImageButton)row.FindControl("imgBtnEdit")).Enabled = false;
                        }
                    }
                    grdFaultTC.Visible = true;
                    cmdLoadItemCode.Visible = true;

                    grdItemCode.Visible = true;
                }
                else
                {
                    grdFaultTC.Visible = true;
                    cmdLoadItemCode.Visible = false;
                    ViewState["FaultTC"] = dt;
                    grdFaultTC.DataSource = dt;  //sort datatable
                    grdFaultTC.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdFaultTC_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdFaultTC.PageIndex;
            DataTable dt = (DataTable)ViewState["FaultTC"];
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
                            ViewState["FaultTC"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {
                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();
                            dataView = new DataView(dt);
                            ViewState["FaultTC"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["FaultTC"] = dataView.ToTable();
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
                            ViewState["FaultTC"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {
                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FaultTC"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["FaultTC"] = dataView.ToTable();
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

        protected void Export_ClickTcsearch(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["FaultTC"];
            if (dt == null)
            {
                ShowMsgBox("No record found");
                return;
            }
            if (dt.Rows.Count > 0 || dt == null)
            {
                dt.Columns["TC_CODE"].ColumnName = "DTR CODE";
                dt.Columns["TC_SLNO"].ColumnName = "DTR SLNO";
                dt.Columns["TM_NAME"].ColumnName = "MAKE NAME";
                dt.Columns["TC_CAPACITY"].ColumnName = "CAPACITY(IN KVA)";
                dt.Columns["TC_MANF_DATE"].ColumnName = "MANF. DATE";
                dt.Columns["TC_STAR_RATE"].ColumnName = "STAR RATE";
                dt.Columns["RCOUNT"].ColumnName = "SENT TO REPAIRER";
                dt.Columns["TC_GUARANTY_TYPE"].ColumnName = "GUARANTEE TYPE";
                dt.Columns["REMARKS"].ColumnName = "REMARKS";
                dt.Columns["REASON"].ColumnName = "REASON";
                dt.Columns["STATUS"].ColumnName = "STATUS";

                List<string> listtoRemove = new List<string> { "TC_ID", "TC_PURCHASE_DATE", "TC_WARANTY_PERIOD", "TS_NAME" };
                string filename = "FaultTCDetails" + DateTime.Now + ".xls";
                string pagetitle = "Faulty TC Details";
                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");
            }
        }

        protected void grdFaultTC_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Edit")
                {
                    GridViewRow row = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
                    int rowindex = row.RowIndex;
                    Label lblTCId = (Label)grdFaultTC.Rows[rowindex].FindControl("lblTCId");

                    string TCId = ((Label)row.FindControl("lblTCId")).Text;
                    TCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(TCId));
                    Response.Redirect("/TCRepair/TransformerDetails.aspx?TcId=" + TCId + "&EditOp=" + 1 + "&ViewOp=" + 0 + "", true);
                }
                if (e.CommandName == "View")
                {
                    GridViewRow row = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
                    int rowindex = row.RowIndex;
                    Label lblTCId = (Label)grdFaultTC.Rows[rowindex].FindControl("lblTCId");
                    string TCId = ((Label)row.FindControl("lblTCId")).Text;
                    TCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(TCId));
                    Response.Redirect("TransformerDetails.aspx?TcId=" + TCId + "&EditOp=" + 0 + "&ViewOp=" + 1 + "", true);
                }
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtrCode = (TextBox)row.FindControl("txtDTRCode");
                    TextBox txtDtrSlNo = (TextBox)row.FindControl("txtSlNo");

                    clsDTrRepairActivity objTcRepair = new clsDTrRepairActivity();
                    if (txtDtrCode.Text != "")
                    {
                        objTcRepair.sTcCode = txtDtrCode.Text;
                    }
                    if (txtDtrSlNo.Text != "")
                    {
                        objTcRepair.sTcSlno = txtDtrSlNo.Text;
                    }

                    SaveCheckedValues();
                    objTcRepair.sOfficeCode = objSession.OfficeCode;
                    objTcRepair.sStoreId = cmbStore.SelectedValue;
                    DataTable dt = (DataTable)ViewState["FaultTC"];
                    dv = dt.DefaultView;
                    if (txtDtrCode.Text != "")
                    {
                        sFilter = string.Format("convert(TC_CODE , 'System.String') Like '%{0}%' ", txtDtrCode.Text.Replace("'", "'"));
                    }
                    if (txtDtrSlNo.Text != "")
                    {
                        sFilter = string.Format("convert(TC_SLNO , 'System.String') Like '%{0}%' ", txtDtrSlNo.Text.Replace("'", "'"));
                    }
                    if (sFilter.Length > 0)
                    {
                        grdFaultTC.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdFaultTC.DataSource = dv;
                            ViewState["FaultTC"] = dv.ToTable();
                            grdFaultTC.DataBind();
                            foreach (GridViewRow rows in grdFaultTC.Rows)
                            {
                                Label lblstatus = (Label)rows.FindControl("lblStatus");
                                if (lblstatus.Text == "ALREADY SENT")
                                {
                                    ((CheckBox)rows.FindControl("chkSelect")).Enabled = false;
                                }
                            }
                        }
                        else
                        {
                            ViewState["FaultTC"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        dt = objTcRepair.LoadFaultTCsearch(objTcRepair);
                        grdFaultTC.DataSource = dt;
                        grdFaultTC.DataBind();
                        foreach (GridViewRow rows in grdFaultTC.Rows)
                        {
                            Label lblstatus = (Label)rows.FindControl("lblStatus");
                            if (lblstatus.Text == "ALREADY SENT")
                            {
                                ((CheckBox)rows.FindControl("chkSelect")).Enabled = false;
                            }
                        }
                        ViewState["FaultTC"] = dt;
                    }
                    PopulateCheckedValues();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void cmdLoadItemCode_Click(object sender, EventArgs e)
        {
            string[] Arr = new string[2];
            try
            {
                if (objSession.RoleId == "43")
                {
                    ShowMsgBox("Sorry You Are Not Authorized To Access");
                    return;
                }

                if (hdnmrep.Value == "1")
                {
                    if (txtRemarks.Text.Length == 0)
                    {
                        ShowMsgBox("Please Enter the Remarks");
                        return;
                    }
                }
                SaveCheckedValues();
                divgrdItem.Visible = true;
                // to get the checked  tc code from the faulty tc grid .......
                bool AtleastOneApp = false;

                ArrayList arrCheckedItems = new ArrayList();
                if (ViewState["CHECKED_ITEMS"] != null)
                {
                    arrCheckedItems = (ArrayList)ViewState["CHECKED_ITEMS"];
                    if (arrCheckedItems.Count == 0)
                    {
                        ShowMsgBox("Please Select DTr Codes");
                        return;
                    }
                }
                if (ViewState["CHECKED_ITEMS"] != null)
                    AtleastOneApp = true;

                if (AtleastOneApp == false)
                {
                    ShowMsgBox("Please Select DTr Codes");
                    return;
                }
                if (cmbRepairer.SelectedIndex == 0)
                {
                    ShowMsgBox("Please Select the Repairer");
                    return;
                }
                LoadFaultTc();

                //to check whether there are two selections 
                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["CheckedItem"];
                ArrayList arrItemCode = (ArrayList)ViewState["CHECKED_ITEMS"];
                Arr = UpdateItemCode();
                if (Arr[0] == "0")
                {
                    return;
                }
                DisableCheckBox(arrCheckedItems);
                cmdSend.Visible = true;
                cmbCapacity.SelectedIndex = 0;
                cmbRepairer.Enabled = false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public string[] UpdateItemCode()
        {
            string[] Arr = new string[2];
            clsDTrRepairActivity objRepair = new clsDTrRepairActivity();
            try
            {
                string off = objSession.OfficeCode;
                string StoreId = objRepair.getdivid(off);

                DataTable itemCode = new DataTable();
                ArrayList arrItemCode = (ArrayList)ViewState["CHECKED_ITEMS"];

                itemCode = (DataTable)ViewState["FaultTC"];
                DataTable dtDet = new DataTable();
                DataTable dt = itemCode.Copy();
                dt.Clear();
                foreach (int s in arrItemCode)
                {
                    var filteredMRList = itemCode.AsEnumerable().Where(r => r.Field<Int64>("TC_ID") == (Convert.ToInt64(s)));
                    if (filteredMRList.Any())
                    {
                        dtDet = filteredMRList.CopyToDataTable();
                        Double Amount = objRepair.GetEstAmt(StoreId, cmbRepairer.SelectedValue, Convert.ToString(dtDet.Rows[0][6]), Convert.ToString(dtDet.Rows[0][5]));
                        string cap = Convert.ToString(dtDet.Rows[0][5]);
                        string starRate = Convert.ToString(dtDet.Rows[0][6]);

                        if (Amount == 0)
                        {
                            ShowMsgBox("For this Repairer, Rates are not available for the Capacity " + cap + " and Star Rate of " + starRate + " ");
                            Arr[0] = "0";

                            return Arr;
                        }
                        if (hdnmrep.Value == "1")
                        {
                            if (Convert.ToString(dtDet.Rows[0][11]) == "WRGP")
                            {
                                dtDet.Rows[0]["ESTIMATION_AMOUNT"] = Convert.ToDouble("0");
                            }
                            else
                            {
                                dtDet.Rows[0]["ESTIMATION_AMOUNT"] = Amount;
                            }
                        }
                        else
                        {
                            if (Convert.ToString(dtDet.Rows[0][11]) == "WRGP")
                            {
                                dtDet.Rows[0]["ESTIMATION_AMOUNT"] = Convert.ToDouble(0);
                            }
                            else
                            {
                                dtDet.Rows[0]["ESTIMATION_AMOUNT"] = Amount;
                            }
                        }
                        dtDet.AcceptChanges();
                        dt.ImportRow(dtDet.Rows[0]);
                    }
                    //getting null here
                }

                if (dt.Rows.Count > 0)
                {
                    DataTable dtFinal = new DataTable();
                    if (ViewState["CheckedItem"] != null)
                    {
                        dtFinal = (DataTable)ViewState["CheckedItem"];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string sTCCode = Convert.ToString(dt.Rows[i]["TC_CODE"]);
                            string sTCID = Convert.ToString(dt.Rows[i]["TC_ID"]);
                            string sTCSlno = Convert.ToString(dt.Rows[i]["TC_SLNO"]);
                            string sTCrating = Convert.ToString(dt.Rows[i]["TC_STAR_RATE"]);
                            string sTCName = Convert.ToString(dt.Rows[i]["TM_NAME"]);
                            string sTCCapacity = Convert.ToString(dt.Rows[i]["TC_CAPACITY"]);
                            string sGuarantee = Convert.ToString(dt.Rows[i]["TC_GUARANTY_TYPE"]);
                            Double dEstAmt = Convert.ToDouble(dt.Rows[i]["ESTIMATION_AMOUNT"]);
                            DataView dv = new DataView();
                            dv = dtFinal.DefaultView;
                            string sFilter = string.Empty;
                            sFilter = " TC_CODE = '" + sTCCode + "'";
                            if (sFilter.Length > 0)
                            {
                                dv.RowFilter = sFilter;
                                if (dv.Count == 0)
                                {
                                    DataRow rw = dtFinal.NewRow();
                                    rw["TC_ID"] = sTCID;
                                    rw["TC_CODE"] = sTCCode;
                                    rw["TC_SLNO"] = sTCSlno;
                                    rw["TC_STAR_RATE"] = sTCrating;
                                    rw["TM_NAME"] = sTCName;
                                    rw["TC_CAPACITY"] = sTCCapacity;
                                    rw["TC_GUARANTY_TYPE"] = sGuarantee;
                                    rw["ESTIMATION_AMOUNT"] = dEstAmt;
                                    dtFinal.Rows.Add(rw);
                                    dtFinal.AcceptChanges();
                                }
                            }
                        }
                    }
                    else
                    {
                        dtFinal = dt;
                    }
                    dt = dtFinal.Copy();
                    var result = dt.AsEnumerable()
                                   .GroupBy(r => r.Field<Int64>("TC_ID"))
                                   .Select(g => g.First())
                                   .CopyToDataTable();
                    var value = dt.AsEnumerable().Sum(g => g.Field<Double>("ESTIMATION_AMOUNT"));

                    txtEstimationAmount.Text = Convert.ToString(value);
                    txtqnty.Text = Convert.ToString(result.Rows.Count);

                    ViewState["CheckedItem"] = result;
                    Session["sessionamtdata"] = ViewState["CheckedItem"];

                    if (dt.Rows.Count > 0)
                    {
                        grdItemCode.DataSource = dt;
                        grdItemCode.DataBind();
                    }
                    else
                    {
                        ShowEmptyGrid_grdItemCode();
                    }
                }
                Arr[0] = "1";
                return Arr;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }

        public void grdItemCode_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Remove")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    Label sTcId = (Label)row.FindControl("lblTCId");
                    DataTable dt = (DataTable)ViewState["CheckedItem"];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (sTcId.Text == Convert.ToString(dt.Rows[i]["TC_ID"]))
                        {
                            dt.Rows[i].Delete();
                            dt.AcceptChanges();
                        }
                    }
                    dt.AcceptChanges();
                    if (dt.Rows.Count == 0)
                    {
                        txtEstimationAmount.Text = "";
                        txtqnty.Text = "";
                        ViewState["CheckedItem"] = null;
                    }
                    else
                    {
                        var value = dt.AsEnumerable().Sum(g => g.Field<Double>("ESTIMATION_AMOUNT"));
                        txtEstimationAmount.Text = Convert.ToString(value);
                        txtqnty.Text = Convert.ToString(dt.Rows.Count);
                        ViewState["CheckedItem"] = dt;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        grdItemCode.DataSource = dt;
                        grdItemCode.DataBind();
                    }
                    else
                    {
                        ShowEmptyGrid_grdItemCode();
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                    }
                    if (ViewState["ArrTcId"] != null)
                    {
                        ArrayList tempTCId = (ArrayList)ViewState["ArrTcId"];
                        ArrayList permTCId = (ArrayList)tempTCId.Clone();
                        int i = 0;
                        foreach (var rows in tempTCId)
                        {
                            if (Convert.ToInt32(rows) == Convert.ToInt32(sTcId.Text))
                            {
                                permTCId.RemoveAt(i);
                            }
                            i++;
                        }
                        ViewState["ArrTcId"] = permTCId;
                        DisableCheckBox(permTCId);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        private void DisableCheckBox(ArrayList arrCheckedItems)
        {
            try
            {
                if (arrCheckedItems != null)
                {
                    ArrayList tempArrayList = null;
                    LoadFaultTc();

                    if (ViewState["ArrTcId"] != null)
                    {
                        tempArrayList = (ArrayList)ViewState["ArrTcId"];
                        foreach (var row in arrCheckedItems)
                        {
                            if (!(tempArrayList.Contains(row)))
                                tempArrayList.Add((row));
                        }
                    }
                    if (tempArrayList != null)
                    {
                        arrCheckedItems = tempArrayList;
                    }
                    foreach (GridViewRow rows in grdFaultTC.Rows)
                    {
                        Label lblTCId = (Label)rows.FindControl("lblTCId");
                        foreach (var row in arrCheckedItems)
                        {
                            if (Convert.ToString(row) == lblTCId.Text)
                            {
                                ((CheckBox)rows.FindControl("chkSelect")).Enabled = false;
                            }
                        }
                    }

                    ViewState["ArrTcId"] = arrCheckedItems;

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
                dt.Columns.Add("TC_ID");
                dt.Columns.Add("TC_CODE");
                dt.Columns.Add("TC_SLNO");
                dt.Columns.Add("TM_NAME");
                dt.Columns.Add("TC_CAPACITY");
                dt.Columns.Add("TC_MANF_DATE");
                dt.Columns.Add("TC_STAR_RATE");
                dt.Columns.Add("TC_PURCHASE_DATE");
                dt.Columns.Add("TC_WARANTY_PERIOD");
                dt.Columns.Add("TS_NAME");
                dt.Columns.Add("RCOUNT");
                dt.Columns.Add("TC_GUARANTY_TYPE");
                dt.Columns.Add("REMARKS");
                dt.Columns.Add("REASON");
                dt.Columns.Add("STATUS");

                grdFaultTC.DataSource = dt;
                grdFaultTC.DataBind();
                int iColCount = grdFaultTC.Rows[0].Cells.Count;
                grdFaultTC.Rows[0].Cells.Clear();
                grdFaultTC.Rows[0].Cells.Add(new TableCell());
                grdFaultTC.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdFaultTC.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void ShowEmptyGrid_grdItemCode()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("TC_ID");
                dt.Columns.Add("TC_CODE");
                dt.Columns.Add("TC_SLNO");
                dt.Columns.Add("TM_NAME");
                dt.Columns.Add("TC_CAPACITY");
                dt.Columns.Add("TC_STAR_RATE");
                dt.Columns.Add("TC_GUARANTY_TYPE");
                dt.Columns.Add("REMARKS");
                dt.Columns.Add("REASON");
                dt.Columns.Add("ESTIMATION_AMOUNT");

                grdItemCode.DataSource = dt;
                grdItemCode.DataBind();
                int iColCount = grdItemCode.Rows[0].Cells.Count;
                grdItemCode.Rows[0].Cells.Clear();
                grdItemCode.Rows[0].Cells.Add(new TableCell());
                grdItemCode.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdItemCode.Rows[0].Cells[0].Text = "No Records Found";

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
                bool AtleastOneApp = false;
                int i = 0;

                if (ViewState["CheckedItem"] != null)
                {
                    foreach (GridViewRow row in grdItemCode.Rows)
                    {
                        Label lblguarentee = (Label)row.FindControl("lblguarentee");
                        if (lblguarentee.Text == "WGP")
                        {
                            ShowMsgBox("WGP Transformers can not send to Repairer");
                            return;
                        }
                    }
                    //Check AccessRights
                    bool bAccResult = CheckAccessRights("2");
                    if (bAccResult == false)
                    {
                        return;
                    }
                    DataTable dt = (DataTable)ViewState["CheckedItem"];
                    string[] strQryVallist = new string[dt.Rows.Count];

                    foreach (DataRow dtRow in dt.Rows)
                    {
                        strQryVallist[i] = dtRow["TC_ID"].ToString();
                        AtleastOneApp = true;
                        i++;
                    }
                    SaveCheckedValues();
                    LoadFaultTc();
                    PopulateCheckedValues();

                    if (!AtleastOneApp)
                    {
                        ShowMsgBox("Please Select DTr to Send for Repairer/Supplier");
                        SaveCheckedValues();
                        LoadFaultTc();
                        PopulateCheckedValues();
                        return;
                    }
                    var n = Convert.ToDouble(ConfigurationManager.AppSettings["RepairerMaxAmount"]);
                    if (Convert.ToDouble(txtEstimationAmount.Text) > Convert.ToDouble(ConfigurationManager.AppSettings["RepairerMaxAmount"]))
                    {
                        ShowMsgBox("Cost of Repair Exceeds DoP.");
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
                    string sTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sSelectedValue));
                   Session["TcId"] = sSelectedValue;
                    Session["sessionamtdata"] = ViewState["CheckedItem"];

                    string sStoreId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(cmbStore.SelectedValue));
                    string sRepairerId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(cmbRepairer.SelectedValue));
                    string sRemarks = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtRemarks.Text));
                    string TCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sSelectedValue));
                    Response.Redirect("TCRepairIssue.aspx?StoreId=" + sStoreId + "&RepairerId=" + sRepairerId + "&Remarks=" + sRemarks + "&TCId=" + TCId, false);
                }
                else
                {
                    ShowMsgBox("Please Select DTr Codes");
                    return;
                }
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
                        if (grdFaultTC.DataKeys[gvrow.RowIndex].Values[0].ToString() != "")
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
                    if (grdFaultTC.DataKeys[gvrow.RowIndex].Values[0].ToString() != "")
                    {
                        index = Convert.ToInt32(grdFaultTC.DataKeys[gvrow.RowIndex].Values[0]);
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
                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["FaultTC"];
                grdFaultTC.DataSource = dt;
                grdFaultTC.DataBind();
                foreach (GridViewRow rows in grdFaultTC.Rows)
                {
                    Label lblstatus = (Label)rows.FindControl("lblStatus");
                    if (lblstatus.Text == "ALREADY SENT")
                    {
                        ((CheckBox)rows.FindControl("chkSelect")).Enabled = false;
                    }
                }
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
                grdFaultTC.Visible = false;
                grdItemCode.Visible = false;
                ShowEmptyGrid_grdItemCode();
                ViewState["CheckedItem"] = null;
                ViewState["ArrTcId"] = null;
                divgrdItem.Visible = false;
                cmdLoadItemCode.Visible = false;
                cmbRepairer.SelectedIndex = 0;
                cmbRepairer.Enabled = true;
                txtEstimationAmount.Text = string.Empty;
                txtqnty.Text = string.Empty;
                txtRemarks.Text = string.Empty;
                cmdSend.Visible = false;
                cmbStarRated.SelectedIndex = 0;
                cmbGuarantyType.SelectedIndex = 0;
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
            string strId = string.Empty;
            try
            {
                clsTcMaster objTcMaster = new clsTcMaster();
                if (objSession.sRoleType == "2")
                {
                    strId = objSession.OfficeCode;
                }
                else
                {
                    strId = objTcMaster.GetStoreId(objSession.OfficeCode);
                }
                cmbStore.SelectedValue = strId;
                if (objSession.OfficeCode == "" || objSession.OfficeCode.Length == 1)
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



        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY
                clsApproval objApproval = new clsApproval();
                objApproval.sFormName = "FaultTCSearch";
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