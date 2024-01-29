using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Collections;
namespace IIITS.DTLMS.ScrapEntry
{
    public partial class ScrapDisposal : System.Web.UI.Page
    {
        string strFormCode = "ScrapDisposal";
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
                
                Form.DefaultButton = cmdSave.UniqueID;
                txtInvoiceDate.Attributes.Add("readonly", "readonly");
                CalendarExtender1.EndDate = System.DateTime.Now;
                if (!IsPostBack)
                {
                    GenerateInvoiceNo();
                    txtInvoiceDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");

                    if (Request.QueryString["TcCount"] != null && Request.QueryString["TcCount"].ToString() != "")
                    {
                        txtQuantity.Text  = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TcCount"]));
                        txtSelectedTcId.Text = Convert.ToString(Session["TCId"]);
                        LoadScrapTc();
                    }


                    string strQry = string.Empty;
                    string strWoNo = Convert.ToString(Session["WorkorderNo"]);
                    strQry = "Title=Search and Select Transformer Details&";
                    strQry += "Query=SELECT \"TC_CODE\",\"TC_SLNO\",\"TM_NAME\" FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\",\"TBLSCRAPOBJECT\",\"TBLSCRAPTC\" WHERE \"TC_LOCATION_ID\" LIKE '" + objSession.sStoreID + "' AND ";
                    strQry += " \"TC_MAKE_ID\"= \"TM_ID\" AND \"TC_STATUS\"=4 AND \"TC_CURRENT_LOCATION\"<>3  AND \"SO_TC_CODE\" = \"TC_CODE\" AND \"ST_ID\"=\"SO_ST_ID\" AND \"ST_OM_NO\" ='" + strWoNo + "'  and {0} like %{1}% order by \"TC_CODE\" &";
                    strQry += "DBColName=CAST(\"TC_CODE\" AS TEXT)~CAST(\"TC_SLNO\" AS TEXT)~CAST(\"TM_NAME\" AS TEXT)&";
                    strQry += "ColDisplayName=DTr CODE~DTr SLNO~MAKE NAME&";
                    strQry = strQry.Replace("'", @"\'");
                   
                    cmdSearchTC.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtTcCode.ClientID + "&btn=" + cmdSearchTC.ClientID + "',520,520," + txtTcCode.ClientID + ")");

                    txtInvoiceDate.Attributes.Add("onblur", "return ValidateDate(" + txtInvoiceDate.ClientID + ");");

                    if (Request.QueryString["ScrapDetailId"] != null && Request.QueryString["ScrapDetailId"].ToString() != "")
                    {
                        hdfScrapDetailsId.Value = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ScrapDetailId"]));
                        GetScrapMasterDetails();
                    }

                    
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        
        public void GetScrapInvoiceReport()
        {
            try
            {
                if (Request.QueryString["ScrapId"] != null && Request.QueryString["ScrapId"].ToString() != "")
                {
                    string sScrapId = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ScrapId"]));
                    string strParam = "id=ScrapInvoice&scrapInvoice=" + sScrapId + "&OfficeCode=" + objSession.OfficeCode + "";
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdSearchTC_Click(object sender, EventArgs e)
        {
            try
            {
                clsDtcMaster objDtcMaster = new clsDtcMaster();
                objDtcMaster.sTcCode = txtTcCode.Text;

                objDtcMaster.GetTCDetails(objDtcMaster);
               
                txtMake.Text = objDtcMaster.sTCMakeName;
                txtTcCode.Text = objDtcMaster.sTcCode;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void LoadScrapTc()
        {
            try
            {
                DataTable dt = new DataTable();
                clsScrap objScrap = new clsScrap();
                txtSelectedTcId.Text = txtSelectedTcId.Text.Replace("~", ",");
                if (!txtSelectedTcId.Text.StartsWith(","))
                {
                    txtSelectedTcId.Text = "," + txtSelectedTcId.Text;
                }
                if (!txtSelectedTcId.Text.EndsWith(","))
                {
                    txtSelectedTcId.Text = txtSelectedTcId.Text + ",";
                }

                txtSelectedTcId.Text = txtSelectedTcId.Text.Substring(1, txtSelectedTcId.Text.Length - 1);
                txtSelectedTcId.Text = txtSelectedTcId.Text.Substring(0, txtSelectedTcId.Text.Length - 1);
                objScrap.sTcId = txtSelectedTcId.Text;
                objScrap.sOfficeCode = objSession.OfficeCode;
                dt = objScrap.LoadScrapTc(objScrap);
                grdFaultTC.DataSource = dt;
                ViewState["ScrapTC"] = dt;
                grdFaultTC.DataBind();
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
            DataTable dt = (DataTable)ViewState["ScrapTC"];
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
                        dataView.Sort = string.Format("{0} {1} ", GridViewSortExpression, GridViewSortDirection);

                        if (dataView.Sort.ToString() == "TC_CAPACITY ASC ")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["ScrapTC"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC ")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["ScrapTC"] = dataView.ToTable();
                        }

                        else
                        {
                            // dataView.Sort = string.Format("{0} {1} ", GridViewSortExpression, GridViewSortDirection);
                            ViewState["ScrapTC"] = dataView.ToTable();
                        }
                    }
                    else
                    {

                        dataView.Sort = string.Format("{0} {1} ", GridViewSortExpression, GetSortDirection());

                        if (dataView.Sort.ToString() == "TC_CAPACITY ASC ")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["ScrapTC"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC ")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["ScrapTC"] = dataView.ToTable();
                        }



                        else
                        {
                            // dataView.Sort = string.Format("{0} {1} ", GridViewSortExpression, GetSortDirection());
                            ViewState["ScrapTC"] = dataView.ToTable(); ;


                        }


                        //dv.Sort = string.Format("{0} {1} ", GridViewSortExpression, GetSortDirection());

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

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                //Check AccessRights
                bool bAccResult = CheckAccessRights("2");
                if (bAccResult == false)
                {
                    return;
                }

                clsScrap objScrap = new clsScrap();
                string[] Arr = new string[2];
                int i = 0;
                int j = 0;
                if (ValidateForm() == true)
                {
                    //objScrap.sOMNo = txtPurchase.Text;
                    objScrap.sAmount = txtAmount.Text;
                    objScrap.sDisposeDesc = txtDescription.Text;
                    objScrap.sCrby = objSession.UserId;
                    objScrap.sInvoiceNo = txtInvoiceNo.Text;
                    objScrap.sInvoiceDate = txtInvoiceDate.Text;
                    objScrap.sSendTo = txtSendTo.Text;

                    string[] strQryVallist = new string[grdFaultTC.Rows.Count];
                    bool bDataExist = false;
                    foreach (GridViewRow row in grdFaultTC.Rows)
                    {

                        strQryVallist[i] = ((Label)row.FindControl("lblTCCode")).Text.Trim();
                        i++;
                        bDataExist = true;
                    }

                    if (bDataExist == false)
                    {
                        ShowMsgBox("No Transformer Exists to Dispose");
                        return;
                    }
                    Arr = objScrap.SaveScrapDispose(strQryVallist,objScrap);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (ScrapDisposal) Scrap ");
                    }

                    if (Arr[1].ToString() == "0")
                    {
                        ShowMsgBox(Arr[0].ToString());

                        cmdSave.Enabled = false;

                        txtTcCode.Text = string.Empty;
                        txtMake.Text = string.Empty;
                        grdFaultTC.DataSource = null;
                        grdFaultTC.DataBind();
                        txtSelectedTcId.Text = string.Empty;
                        cmdGatePass.Enabled = true;
                     
                        GetScrapInvoiceReport();
                        //string strTcCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtTcCode.Text));
                        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID, "alert('Saved Successfully'); location.href='ScrapEntryView.aspx?QryDtcId=" + strTcCode + "';", true);
                       
                        return;
                    }
                    if (Arr[1].ToString() == "1")
                    {
                        ShowMsgBox(Arr[0].ToString());
                       
                        //string strTcCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtTcCode.Text));

                        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID, "alert('Updated Successfully'); location.href='ScrapEntryView.aspx?QryDtcId=" + strTcCode + "';", true);
                   
                       
                        return;
                    }
                    if (Arr[1].ToString() == "4")
                    {
                        ShowMsgBox(Arr[0].ToString());
                        return;
                    }
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

        public void AddTCtoGrid(string sTcCode)
        {
            try
            {
                clsDTrRepairActivity objScrap = new clsDTrRepairActivity();

                if (ValidateGridValue(sTcCode) == true)
                {

                    objScrap.sTcCode = sTcCode;
                    objScrap.sRefString = "Scrap";
                    objScrap.GetFaultTCDetails(objScrap);

                    if (ViewState["ScrapTC"] != null)
                    {
                        DataTable dtFaultTc = (DataTable)ViewState["ScrapTC"];
                        DataRow drow;
                        if (dtFaultTc.Rows.Count > 0)
                        {
                            drow = dtFaultTc.NewRow();

                            drow["TC_ID"] = objScrap.sTcId;
                            drow["TC_CODE"] = objScrap.sTcCode;
                            drow["TC_SLNO"] = objScrap.sTcSlno;
                            drow["TM_NAME"] = objScrap.sMakeName;
                            drow["TC_CAPACITY"] = objScrap.sCapacity;
                            drow["TC_MANF_DATE"] = objScrap.sManfDate;
                            drow["TC_PURCHASE_DATE"] = objScrap.sPurchaseDate;
                            drow["TC_WARANTY_PERIOD"] = objScrap.sWarrantyPeriod;
                            drow["TS_NAME"] = objScrap.sSupplierName;

                            dtFaultTc.Rows.Add(drow);
                            grdFaultTC.DataSource = dtFaultTc;
                            grdFaultTC.DataBind();
                            ViewState["ScrapTC"] = dtFaultTc;
                        }
                    }
                    else
                    {
                        DataTable dtFaultTc = new DataTable();
                        DataRow drow;


                        dtFaultTc.Columns.Add(new DataColumn("TC_ID"));
                        dtFaultTc.Columns.Add(new DataColumn("TC_CODE"));
                        dtFaultTc.Columns.Add(new DataColumn("TC_SLNO"));
                        dtFaultTc.Columns.Add(new DataColumn("TM_NAME"));
                        dtFaultTc.Columns.Add(new DataColumn("TC_CAPACITY"));
                        dtFaultTc.Columns.Add(new DataColumn("TC_MANF_DATE"));
                        dtFaultTc.Columns.Add(new DataColumn("TC_PURCHASE_DATE"));
                        dtFaultTc.Columns.Add(new DataColumn("TC_WARANTY_PERIOD"));
                        dtFaultTc.Columns.Add(new DataColumn("TS_NAME"));

                        drow = dtFaultTc.NewRow();

                        drow["TC_ID"] = objScrap.sTcId;
                        drow["TC_CODE"] = objScrap.sTcCode;
                        drow["TC_SLNO"] = objScrap.sTcSlno;
                        drow["TM_NAME"] = objScrap.sMakeName;
                        drow["TC_CAPACITY"] = objScrap.sCapacity;
                        drow["TC_MANF_DATE"] = objScrap.sManfDate;
                        drow["TC_PURCHASE_DATE"] = objScrap.sPurchaseDate;
                        drow["TC_WARANTY_PERIOD"] = objScrap.sWarrantyPeriod;
                        drow["TS_NAME"] = objScrap.sSupplierName;

                        dtFaultTc.Rows.Add(drow);
                        grdFaultTC.DataSource = dtFaultTc;
                        grdFaultTC.DataBind();
                        ViewState["ScrapTC"] = dtFaultTc;

                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public bool ValidateGridValue(string sTcSlno)
        {
            bool bValidate = false;
            try
            {
                ArrayList objArrlist = new ArrayList();

                foreach (GridViewRow row in grdFaultTC.Rows)
                {
                    objArrlist.Add(((Label)row.FindControl("lblTCCode")).Text.Trim());
                }

                if (objArrlist.Contains(sTcSlno))
                {
                    ShowMsgBox("Transformer Already Added");
                    DataTable dtFaultTc = (DataTable)ViewState["ScrapTC"];
                    grdFaultTC.DataSource = dtFaultTc;
                    grdFaultTC.DataBind();
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

        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTcCode.Text == "")
                {
                    ShowMsgBox("Select Transformer Code");
                    return;
                }

                if (txtQuantity.Text != "")
                {
                    if (grdFaultTC.Rows.Count > Convert.ToInt32(txtQuantity.Text))
                    {
                        ShowMsgBox("Already Added Entered Quantity Transformers");
                        return;
                    }
                }
                AddTCtoGrid(txtTcCode.Text);
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
                Reset();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void Reset()
        {
            try
            {
                txtAmount.Text = "";                
                txtDescription.Text = "";
                txtQuantity.Text = string.Empty;
                txtInvoiceDate.Text = string.Empty;
                GenerateInvoiceNo();
            
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public bool ValidateForm()
        {
            bool bValidate = false;
            try
            {              
                if (txtAmount.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Amount");
                    txtAmount.Focus();
                    return bValidate;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtAmount.Text, "^(\\d{1,11})?(\\.\\d{1,2})?$"))
                {
                    ShowMsgBox("Enter Valid Amount (eg:111111111111.00)");
                    txtAmount.Focus();
                    return false;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(txtAmount.Text, "[-+]?[0-9]{0,11}\\.?[0-9]{0,2}"))
                {
                    ShowMsgBox("Enter Valid Amount (eg:111111111111.00)");
                    txtAmount.Focus();
                    return false;
                }
                if (txtQuantity.Text != "")
                {
                    if (txtQuantity.Text == "0")
                    {
                        ShowMsgBox("Enter Valid Quantity");
                        txtQuantity.Focus();
                        return false;
                    }
                    if (Convert.ToInt32(txtQuantity.Text) > grdFaultTC.Rows.Count)
                    {
                        ShowMsgBox("Entered Quanity and Selected Transformer Count should be same");
                        txtQuantity.Focus();
                        return false;
                    }
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

        protected void cmdClose_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ScrapEntryView.aspx", false);

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

        protected void grdFaultTC_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Remove")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    int iRowIndex = row.RowIndex;

                    DataTable dt = (DataTable)ViewState["ScrapTC"];
                    dt.Rows[iRowIndex].Delete();
                    if (dt.Rows.Count == 0)
                    {
                        ViewState["ScrapTC"] = null;
                    }
                    else
                    {
                        ViewState["ScrapTC"] = dt;
                    }

                    grdFaultTC.DataSource = dt;
                    grdFaultTC.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetScrapMasterDetails()
        {
            try
            {
                clsScrap objScrap = new clsScrap();
                DataTable dt = new DataTable();
                objScrap.sScrapDetailsId = hdfScrapDetailsId.Value;

                objScrap.GetScrapMasterDetails(objScrap);

                //txtPurchase.Text = objScrap.sOMNo;
                txtDescription.Text = objScrap.sDisposeDesc;
                txtAmount.Text = objScrap.sAmount;
                txtQuantity.Text = Convert.ToString(objScrap.sDTrCount);
                txtSendTo.Text = objScrap.sSendTo;
               
                cmdSave.Enabled = false;

                dt = objScrap.LoadScrapGrid(objScrap);
                grdFaultTC.DataSource = dt;
                grdFaultTC.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        #region GatePass

        protected void cmdGatePass_Click(object sender, EventArgs e)
        {
            try
            {
                clsInvoice objInvoice = new clsInvoice();
                string[] Arr = new string[2];
                if (ValidateGatePass() == true)
                {
                    objInvoice.sGatePassId = txtGpId.Text;
                    objInvoice.sVehicleNumber = txtVehicleNo.Text.Replace("'", "");
                    objInvoice.sReceiptientName = txtReciepient.Text.Replace("'", "");
                    objInvoice.sChallenNo = txtChallen.Text.Replace("'", "");
                    objInvoice.sCreatedBy = objSession.UserId;
                    //objInvoice.sTcCode = txtTCCode.Text.Replace("'", "");

                    objInvoice.sInvoiceNo = txtInvoiceNo.Text.Replace("'", "");

                    Arr = objInvoice.SaveUpdateGatePassDetails(objInvoice);

                    if (Arr[1].ToString() == "0")
                    {
                        txtGpId.Text = objInvoice.sGatePassId;
                        string strParam = "id=ScrapGatepass&InvoiceId=" + txtInvoiceNo.Text;
                        RegisterStartupScript("PrintD", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

                        return;
                    }
                    if (Arr[1].ToString() == "1")
                    {
                        string strParam = "id=ScrapGatepass&InvoiceId=" + txtInvoiceNo.Text;
                        RegisterStartupScript("PrintD", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

                        return;
                    }

                    if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0]);
                        return;
                    }
                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        bool ValidateGatePass()
        {
            bool bValidate = false;

            try
            {

                if (txtVehicleNo.Text.Length == 0)
                {
                    txtVehicleNo.Focus();
                    ShowMsgBox("Enter Vehicle No");
                    return bValidate;
                }
                if (txtChallen.Text.Length == 0)
                {
                    txtChallen.Focus();
                    ShowMsgBox("Enter Challen Number");
                    return bValidate;
                }

                if (txtReciepient.Text.Trim().Length == 0)
                {
                    txtReciepient.Focus();
                    ShowMsgBox("Enter Reciepient Name");
                    return bValidate;
                }



                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                bValidate = false;
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return bValidate;
            }
        }

        #endregion


        public void GenerateInvoiceNo()
        {
            try
            {
                clsInvoice objInvoice = new clsInvoice();
                string sRoletype = objSession.sRoleType;
                txtInvoiceNo.Text = objInvoice.GenerateInvoiceNo(objSession.OfficeCode, sRoletype);
                txtInvoiceNo.ReadOnly = true;
                hdfInvoiceNo.Value = txtInvoiceNo.Text;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        //protected void grdFaultTC_PageIndexChanging1(object sender, GridViewPageEventArgs e)
        //{
        //    try
        //    {
        //        grdFaultTC.PageIndex = e.NewPageIndex;
        //        DataTable dt = (DataTable)ViewState["ScrapTC"];
        //        grdFaultTC.DataSource = dt;
        //        grdFaultTC.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdFaultTC_PageIndexChanging");
        //    }
        //}
       
    }
}