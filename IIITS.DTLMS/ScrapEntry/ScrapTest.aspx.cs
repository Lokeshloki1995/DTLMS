using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Collections;
using System.IO;
using System.ComponentModel;
using System.Drawing;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Net;

namespace IIITS.DTLMS.ScrapEntry
{
    public partial class ScrapTest : System.Web.UI.Page
    {
        string strFormCode = "ScrapTest";
        clsSession objSession;
        string sUserName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_USER"]);
        string sPassword = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_PASS"]);

        string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
        string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);
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
                ReferenceCalender.EndDate = System.DateTime.Now;
                txtOmdate.Attributes.Add("readonly", "readonly");
                cmbStore.Enabled = false;
            


                if (!IsPostBack)
                {
                    txtOmdate.Attributes.Add("onblur", "return ValidateDate(" + txtOmdate.ClientID + ");");
                    LoadComboFiled();
                    GetStoreId();
                    CheckAccessRights("4");

                    if (Request.QueryString["OMID"] != null && Request.QueryString["OMID"].ToString() != "")
                    {
                        txtViewOmID.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["OMID"]));
                        txtViewOmNo.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryOMNo"]));
                        txtViewOmdate.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryOmDate"]));

                        DataTable dt = new DataTable();
                        loadScrapDetails();
                        dvscraphead.Style.Add("display", "none");
                        dvclose.Style.Add("display", "block");

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

                string sOfficeCode = string.Empty;
                int Division = Convert.ToInt32(ConfigurationSettings.AppSettings["Division_code"]);
                if (objSession.OfficeCode.Length > 2)
                {
                    sOfficeCode = objSession.OfficeCode.Substring(0, Division);
                }
                else
                {
                    sOfficeCode = objSession.OfficeCode;
                }
                //Genaral.Load_Combo("SELECT \"SM_ID\",\"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"SM_STATUS\" ='A' AND CAST(\"SM_OFF_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%' ORDER BY \"SM_NAME\" ", "--Select--", cmbStore);
                Genaral.Load_Combo("SELECT \"SM_ID\",\"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"SM_STATUS\" ='A' AND CAST(\"SM_ID\" AS TEXT) = '" + sOfficeCode + "' ORDER BY \"SM_NAME\" ", cmbStore);
                Genaral.Load_Combo(strQry, "--Select--", cmbMake);
                Genaral.Load_Combo("SELECT \"MD_NAME\",\"MD_NAME\" from \"TBLMASTERDATA\" WHERE \"MD_TYPE\" ='C' ORDER BY \"MD_ORDER_BY\" ", "--Select--", cmbCapacity);
                Genaral.Load_Combo("SELECT \"TS_ID\",\"TS_NAME\" FROM \"TBLTRANSSUPPLIER\"  WHERE \"TS_STATUS\" ='A' AND \"TS_ID\" NOT IN (SELECT \"TS_ID\" FROM \"TBLTRANSSUPPLIER\" WHERE \"TS_BLACK_LISTED\"=1 AND \"TS_BLACKED_UPTO\" >=NOW()) ORDER BY \"TS_NAME\"", "--Select--", cmbSupplier);
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
                LoadFaultTc();
                //dvcheckedTc.Style.Add("display", "none");
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

                objScrap.sOfficeCode = objSession.OfficeCode;
                dt = objScrap.LoadFaultTCForScrap(objScrap);

                SaveCheckedValues();
                grdFaultTC.DataSource = dt;
                grdFaultTC.DataBind();
                ViewState["TestDTR"] = dt;
                ViewState["TotalDTrs"] = dt;
                //SaveCheckedValues();
                //ViewState["dtUpdatedDtrs"] = null;
                grdFaultTC.Visible = true;
                if (dt.Rows.Count > 0)
                {
                    if (ViewState["CHECKED_ITEMS"] != null)
                    {
                        cmdSend.Visible = true;
                        Load.Visible = true;
                        dvcheckedTc.Style.Add("display", "block");
                        dvHead.Style.Add("display", "block");
                        ArrayList tempTcCodes = (ArrayList)ViewState["CHECKED_ITEMS"];
                        DisableCheckBox(tempTcCodes);
                    }
                    else
                    {
                        cmdSend.Visible = false;
                        Load.Visible = true;
                        dvcheckedTc.Style.Add("display", "none");
                        dvHead.Style.Add("display", "none");
                    }

                }
                else
                {
                    cmdSend.Visible = false;
                    Load.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        //protected void grdFaultTC_RowCommand(object sender, GridViewCommandEventArgs e)
        //{

        //    try
        //    {
        //        if (e.CommandName == "Download")
        //        {

        //            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
        //            string sTcCode = ((Label)row.FindControl("lblTCCode")).Text;
        //            download(sTcCode);
        //            grdFaultTC.Columns[8].Visible = false;

        //        }
        //        if (e.CommandName == "Remove")
        //        {
        //            GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
        //            int iRowIndex = row.RowIndex;

        //            DataTable dt = (DataTable)ViewState["TestDTR"];
        //            dt.Rows[iRowIndex].Delete();
        //            if (dt.Rows.Count == 0)
        //            {
        //                ViewState["TestDTR"] = null;
        //            }
        //            else
        //            {
        //                ViewState["TestDTR"] = dt;
        //            }
        //            grdFaultTC.Columns[8].Visible = false;
        //            grdFaultTC.DataSource = dt;
        //            grdFaultTC.DataBind();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }


        //}

        public bool ValidateForm()
        {
            bool bValidate = false;
            try
            {


                if (txtOmNo.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Please Enter Valid Scrap OM No");
                    txtOmNo.Focus();
                    return bValidate;
                }

                if (txtOmdate.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Please Select Valid Date Of Scrapping");
                    txtOmdate.Focus();
                    return bValidate;
                }
                if (fupDoc.PostedFile.ContentLength == 0)
                {
                    ShowMsgBox("Please Upload OM Document");
                    fupDoc.Focus();
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


        protected void cmdSend_Click(object sender, EventArgs e)
        {
            clsScrap objScrap = new clsScrap();
            try
            {
                if (ValidateForm() == true)
                {

                    objScrap.sOMNo = txtOmNo.Text;
                    objScrap.sOMDate = txtOmdate.Text;
                    objScrap.sFileName = (Path.GetFileName(fupDoc.FileName)).Trim();
                    objScrap.sStoreId = cmbStore.SelectedValue;
                    objScrap.sOfficeCode = objSession.OfficeCode;
                    objScrap.sCrby = objSession.UserId;

                    string[] Arr = new string[2];

                    int i = 0;
                    // objScrap.sTestResult = "0";
                    bool bChecked = false;
                    DataTable dtimage = new DataTable();
                    DataColumn dc = new DataColumn("image");
                    dc.DataType = System.Type.GetType("System.Byte[]");
                    dtimage.Columns.Add(dc);
                    dtimage.Columns.Add("TCID", typeof(string));

                    Byte[] Buffer;
                    string[] strQrylist = new string[grdloadcheckeddtr.Rows.Count];

                    foreach (GridViewRow row in grdloadcheckeddtr.Rows)
                    {
                        bool ChekSelc = false;
                        FileUpload fupDoc = (FileUpload)row.FindControl("fupDoc");

                        objScrap.sTestResult = "1";
                        bChecked = true;
                        ChekSelc = true;
                        string sRemarks = ((TextBox)row.FindControl("txtRemarks")).Text;

                        if (sRemarks == "")
                        {
                            ShowMsgBox("Please Enter Reason For Scrap");
                            return;
                        }
                        #region SaveImage
                        //if (fupDoc.PostedFile.ContentLength != 0)
                        //{
                        //    string filename = Path.GetFileName(fupDoc.PostedFile.FileName);
                        //    string strExt = filename.Substring(filename.LastIndexOf('.') + 1);
                        //    if (strExt.ToLower().Equals("jpg") || strExt.ToLower().Equals("jpeg") || strExt.ToLower().Equals("png") || strExt.ToLower().Equals("gif") || strExt.ToLower().Equals("pdf"))
                        //    {
                        //        Stream strm = fupDoc.PostedFile.InputStream;
                        //        Buffer = new byte[strm.Length];
                        //        strm.Read(Buffer, 0, (int)strm.Length);

                        //        if ((int)strm.Length > 102400)
                        //        {
                        //            ShowMsgBox("File size should not be greater than 100 KB");
                        //            return;
                        //        }

                        //        dtimage.Rows.Add(Buffer, ((Label)row.FindControl("lblTCId")).Text.Trim());
                        //        objScrap.sFileName = filename;

                        //    }
                        //    else
                        //    {
                        //        ShowMsgBox("Invalid File");
                        //        return;
                        //    }
                        //}
                        #endregion

                        strQrylist[i] = ((Label)row.FindControl("lblTCId")).Text.Trim() + "~" + sRemarks.Replace("'", "`") + "~" + objScrap.sFileName + "~" + ((Label)row.FindControl("lblTCCode")).Text.Trim();
                        i++;



                    }


                    if (bChecked == false)
                    {
                        ShowMsgBox("Please Select DTr");
                        return;
                    }
                    Session["fileupload"] = dtimage;
                    objScrap.sDTrCount = i;
                    Arr = objScrap.DeclareTcScrap(strQrylist, objScrap, dtimage);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (ScrapTest) Scrap ");
                    }

                    if (Arr[1].ToString() == "0")
                    {

                        ShowMsgBox(Arr[0].ToString());
                        LoadFaultTc();
                        ViewState["CHECKED_ITEMS"] = null;
        
                    }
                    else if (Arr[1].ToString() == "1")
                    {
                        FTPUpload(sender, e, objScrap);
                        ShowMsgBox(Arr[2].ToString());
                        //cmdSend.Visible = false;
                        cmdSend.Enabled = false;
                        Reset();
                        return;
                    }
                    if (Arr[1].ToString() == "2")
                    {

                        ShowMsgBox(Arr[0].ToString());
                        txtOmNo.Focus();
                        // LoadFaultTc();
                        //ViewState["CHECKED_ITEMS"] = null;
                    }
                }
                return;
            }



            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                lblMessage.Text = clsException.ErrorMsg();
            }
        }

        void Reset()
        {
            try
            {
                cmbCapacity.SelectedIndex = 0;
                cmbMake.SelectedIndex = 0;

                cmbSupplier.SelectedIndex = 0;
                grdFaultTC.DataSource = null;
                grdFaultTC.DataBind();
                grdFaultTC.Visible = false;
                cmdSend.Visible = false;
                dvcheckedTc.Style.Add("display", "none");
                dvHead.Style.Add("display", "none");
                Load.Visible = false;
                txtOmNo.Text = string.Empty;
                txtOmdate.Text = string.Empty;
                ViewState["CHECKED_ITEMS"] = null;
                if (cmbStore.Enabled == true)
                {
                    cmbStore.SelectedIndex = 0;
                }
                //grdloadcheckeddtr.DataSource = null;
                //grdloadcheckeddtr.DataBind();
                ViewState["dtUpdatedDtrs"] = null;
               
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }



        //This method is used to save the checkedstate of values


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
            cmbCapacity.SelectedIndex = 0;
            cmbMake.SelectedIndex = 0;

            cmbSupplier.SelectedIndex = 0;
            grdFaultTC.Visible = false;
            cmdSend.Visible = false;
            dvcheckedTc.Style.Add("display", "none");
            dvHead.Style.Add("display", "none");
            Load.Visible = false;
            txtOmNo.Text = string.Empty;
            txtOmdate.Text = string.Empty;
            ArrayList permTCId = (ArrayList)ViewState["CHECKED_ITEMS"];
            EnableCheckBox(permTCId);
            ViewState["CHECKED_ITEMS"] = null;

            if (cmbStore.Enabled == true)
            {
                cmbStore.SelectedIndex = 0;
            }
            grdloadcheckeddtr.DataSource = null;
            grdloadcheckeddtr.DataBind();
        }
        public void GetStoreId()
        {
            try
            {
                clsTcMaster objTcMaster = new clsTcMaster();
                cmbStore.SelectedValue = objTcMaster.GetStoreId(objSession.OfficeCode);

                if (objSession.OfficeCode == "" || objSession.OfficeCode.Length == 1)
                {
                    cmbStore.Enabled = false;

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

                objApproval.sFormName = "ScrapTest";
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sAccessType = "1" + "," + sAccessType;
                bool bResult = objApproval.CheckAccessRights(objApproval);
                if (bResult == false)
                {
                    Response.Redirect("~/UserRestrict.aspx", false);

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

        protected void grdFaultTC_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string sTcStatus = ((Label)e.Row.FindControl("lblTCStatus")).Text;
                    string sTcCode = ((Label)e.Row.FindControl("lblTCCode")).Text;

                    DataTable dt = (DataTable)ViewState["TestDTR"];
                    DataRow[] dtrow = dt.Select("TC_CODE like '%" + sTcCode + "%'");
                    LinkButton lnkDnld = ((LinkButton)e.Row.FindControl("lnkDwnld"));
                    LinkButton lnkNodnld = ((LinkButton)e.Row.FindControl("lnkNodownload"));
                    grdFaultTC.Columns[8].Visible = false;


                    //if (dtrow[0]["IND_DOC"].ToString() == null || dtrow[0]["IND_DOC"].ToString() == "")
                    //{
                    //    lnkDnld.Visible = false;
                    //    lnkNodnld.Visible = true;
                    //    lnkNodnld.CssClass = "blockpointer";
                    //    grdFaultTC.Columns[8].Visible = false;


                    //}
                    //else
                    //{
                    //    lnkDnld.Enabled = true;
                    //    lnkNodnld.Visible = false;
                    //    lnkDnld.CssClass = "handPointer";
                    //    grdFaultTC.Columns[8].Visible = false;
                    //}


                    if (sTcStatus == "7")
                    {
                        grdFaultTC.Columns[8].Visible = false;
                        grdFaultTC.Columns[12].Visible = false;
                    }
                    else
                    {
                        grdFaultTC.Columns[8].Visible = false;
                        grdFaultTC.Columns[12].Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        private void download(string sTcCode)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["TestDTR"];
                DataRow[] dtrow = dt.Select("TC_CODE like '%" + sTcCode + "%'");
                Byte[] bytes = (Byte[])dtrow[0]["IND_DOC"];


                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "image/png";

                Response.AppendHeader("Content-Disposition", "attachment; filename=" + dtrow[0]["TC_CODE"].ToString() + ".png");

                Response.BinaryWrite(bytes);
                Response.Flush();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }



        public void LoadAlreadyDone()
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

                objScrap.sOfficeCode = objSession.OfficeCode;
                dt = objScrap.LoadAlreadyDone(objScrap);
                grdFaultTC.DataSource = dt;
                ViewState["TestDTR"] = dt;
                grdFaultTC.DataBind();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void Export_clickscraptest(object sender, EventArgs e)
        {

            //try
            //{
            //    DataTable dt = new DataTable();
            //    if (rdbAlreadyTested.Checked)
            //    {
            //       // DataTable dt = new DataTable();
            //        clsScrap objScrap = new clsScrap();
            //        if (cmbCapacity.SelectedIndex > 0)
            //        {
            //            objScrap.sCapacity = cmbCapacity.SelectedValue;
            //        }
            //        if (cmbMake.SelectedIndex > 0)
            //        {
            //            objScrap.sMakeId = cmbMake.SelectedValue;
            //        }
            //        if (cmbStore.SelectedIndex > 0)
            //        {
            //            objScrap.sStoreId = cmbStore.SelectedValue;
            //        }
            //        if (cmbSupplier.SelectedIndex > 0)
            //        {
            //            objScrap.sSupplierId = cmbSupplier.SelectedValue;
            //        }

            //        objScrap.sOfficeCode = objSession.OfficeCode;
            //        dt = objScrap.LoadAlreadyDone(objScrap);  
            //    }
            //    else
            //    {

            //        clsScrap objScrap = new clsScrap();

            //        if (cmbCapacity.SelectedIndex > 0)
            //        {
            //            objScrap.sCapacity = cmbCapacity.SelectedValue;
            //        }
            //        if (cmbMake.SelectedIndex > 0)
            //        {
            //            objScrap.sMakeId = cmbMake.SelectedValue;
            //        }
            //        if (cmbStore.SelectedIndex > 0)
            //        {
            //            objScrap.sStoreId = cmbStore.SelectedValue;
            //        }
            //        if (cmbSupplier.SelectedIndex > 0)
            //        {
            //            objScrap.sSupplierId = cmbSupplier.SelectedValue;
            //        }

            //        objScrap.sOfficeCode = objSession.OfficeCode;
            //        dt = objScrap.LoadFaultTCForScrap(objScrap);

            //    }


            DataTable dt = (DataTable)ViewState["TestDTR"];

            if (dt.Rows.Count > 0)
            {

                dt.Columns["TC_CODE"].ColumnName = "DTR CODE";
                dt.Columns["DT_CODE"].ColumnName = "DTC CODE";
                dt.Columns["TC_SLNO"].ColumnName = "DTR SLNO";
                dt.Columns["TM_NAME"].ColumnName = "MAKE NAME";
                dt.Columns["TC_CAPACITY"].ColumnName = "CAPACITY(IN KVA)";
                dt.Columns["TC_MANF_DATE"].ColumnName = "MANF. DATE";
                dt.Columns["TS_NAME"].ColumnName = "Supplier";


                List<string> listtoRemove = new List<string> { "TC_ID", "TC_PURCHASE_DATE", "TC_STATUS" };
                string filename = "ScrapTest" + DateTime.Now + ".xls";
                string pagetitle = " Faulty Transformer Details";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");
            }

            //}
            //catch (Exception ex)
            //{
            //    lblMessage.Text = clsException.ErrorMsg();
            //    clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Export_clickscraptest");
            //}

        }
        protected void grdFaultTC_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdFaultTC.PageIndex;
            DataTable dt = (DataTable)ViewState["TestDTR"];
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
                            ViewState["TestDTR"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["TestDTR"] = dataView.ToTable();
                        }
                        else
                        {

                            ViewState["TestDTR"] = dataView.ToTable();
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
                            ViewState["TestDTR"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["TestDTR"] = dataView.ToTable();
                        }
                        else
                        {

                            ViewState["TestDTR"] = dataView.ToTable();
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

        protected void grdFaultTC_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                SaveCheckedValues();
                grdFaultTC.PageIndex = e.NewPageIndex;
                if (ViewState["dtUpdatedDtrs"] != "" && ViewState["dtUpdatedDtrs"] != null)
                {
                    dvcheckedTc.Style.Add("display", "block");
                    dvHead.Style.Add("display", "block");
                }
                //LoadFaultTc();
                DataTable dtTcCodes = (DataTable)ViewState["TotalDTrs"];
                grdFaultTC.DataSource = dtTcCodes;
                grdFaultTC.DataBind();

                DataTable tempTCId = (DataTable)ViewState["dtUpdatedDtrs"];



                if (tempTCId != null)
                {
                    foreach (GridViewRow rows in grdFaultTC.Rows)
                    {
                        Label lblTCId = (Label)rows.FindControl("lblTCId");
                        foreach (DataRow dtRow in tempTCId.Rows)
                        {
                            if (Convert.ToString(dtRow) == lblTCId.Text)
                            {
                                ((CheckBox)rows.FindControl("chkSelect")).Enabled = false;
                                ((CheckBox)rows.FindControl("chkSelect")).Checked = true;
                            }

                        }
                    }
                }
                PopulateCheckedValues();


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdPendingTc_PageIndexChanging");
            }
        }
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
                }
                if (arrCheckedValues != null && arrCheckedValues.Count > 0)
                    ViewState["CHECKED_ITEMS"] = arrCheckedValues;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveCheckedValues");
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
                            myCheckBox.Enabled = false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "PopulateCheckedValues");

            }
        }

        protected void cmdTcLoad_Click(object sender, EventArgs e)
        {
            try
            {
                //bool bValidate = false;
                string strvalue = string.Empty;
                string selecteditem = string.Empty;
                string txtPeriod = string.Empty;
                string tempquantity = string.Empty;
                string tempTC = string.Empty;

                bool flag = false;

                ArrayList tempTcCodes = new ArrayList();
                List<String> TcState = new List<string>();
                SaveCheckedValues();
                ArrayList arrItemCode = (ArrayList)ViewState["CHECKED_ITEMS"];
                if (ViewState["CHECKED_ITEMS"] != null)
                {
                    tempTcCodes = arrItemCode;
                }
                else
                {
                    ShowMsgBox("Please Select DTr Code");
                    return;

                }

                //foreach (GridViewRow row in grdFaultTC.Rows)
                //{
                //    if (((CheckBox)row.FindControl("chkSelect")).Checked == true)
                //    {
                //        tempTC = ((Label)row.FindControl("lblTCId")).Text;
                //        tempTcCodes.Add(tempTC);
                //        flag = true;
                //        //strvalue += ((Label)row.FindControl("lblStatus")).Text + ' ';

                //    }
                //}
                //if (!flag)
                //{
                //    ShowMsgBox("Please Select Tc Code");
                //    return;
                //}


                LoadSelectedTcCode(tempTcCodes);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdLoad_Click");
                lblMessage.Text = clsException.ErrorMsg();
            }
        }

        protected void LoadSelectedTcCode(ArrayList tempTcCodes)
        {
            try
            {
                clsScrap objScrap = new clsScrap();
                string state = string.Empty;
                dvcheckedTc.Style.Add("display", "block");
                dvHead.Style.Add("display", "block");
                DataTable dtTcItCode = new DataTable();
                DataTable tempDt = new DataTable();
                objScrap.sOfficeCode = objSession.OfficeCode;
                DataTable dtTcCodes = objScrap.LoadFaultDtrs(objScrap);
                // DataTable dtTcCodes = (DataTable)ViewState["TotalDTrs"];


                if (ViewState["TestDTR"] != null)
                {
                    dtTcItCode = ((DataTable)ViewState["TestDTR"]).Clone();
                }
                //foreach (string s in tempTcCodes)
                foreach (int s in tempTcCodes)
                {
                    //var filteredMRList = dtTcCodes.AsEnumerable().Where(r => r.Field<String>("TC_ID") == s);
                    var filteredMRList = dtTcCodes.AsEnumerable().Where(r => r.Field<String>("TC_ID") == Convert.ToString(s));
                    if (filteredMRList.Any())
                    {
                        dtTcItCode.ImportRow(filteredMRList.CopyToDataTable().Rows[0]);
                    }
                    //getting null here
                }
                ViewState["TestDTR"] = (DataTable)dtTcItCode;



                if (dtTcItCode.Rows.Count > 0)
                {
                    dtTcItCode = dtTcItCode.DefaultView.ToTable(true, "TC_ID", "TC_CODE", "TC_SLNO", "TM_NAME", "TC_CAPACITY", "TC_MANF_DATE", "TC_PURCHASE_DATE", "TC_STATUS");
                }

                dtTcItCode.Columns.Add("STD_REMARKS");

                if (ViewState["AfterRemovedDtrs"] == null)
                {
                    DataRow newRow = dtTcItCode.NewRow();
                    dtTcItCode.Columns.Add("Remarks");
                    // DataRow dRow = dtTcItCode.NewRow();
                    int j = 0;
                    foreach (GridViewRow rows in grdloadcheckeddtr.Rows)
                    {
                        
                            if (dtTcItCode.Rows.Count > j)
                            {
                                TextBox Remarks = (TextBox)rows.FindControl("txtRemarks");
                                dtTcItCode.Rows[j]["Remarks"] = Remarks.Text;
                            }

                        j++;
                    }
                    ViewState["AfterRemovedDtrs"] = dtTcItCode;
                }

                grdloadcheckeddtr.DataSource = dtTcItCode;
                int numberOfRecords = dtTcItCode.Rows.Count;
                TxtTotalDtrs.Text = Convert.ToString(numberOfRecords);
                grdloadcheckeddtr.DataBind();
                grdloadcheckeddtr.Columns[0].Visible = false;
                grdloadcheckeddtr.Columns[14].Visible = false;
                cmdSend.Visible = true;
                cmdSend.Enabled = true;
                // Load.Visible = false;

                ViewState["dtUpdatedDtrs"] = dtTcItCode;

                DisableCheckBox(tempTcCodes);




                if (ViewState["AfterRemovedDtrs"] != null)
                {


                    tempDt = (DataTable)ViewState["AfterRemovedDtrs"];

                    int k = 0;
                    foreach (GridViewRow rows in grdloadcheckeddtr.Rows)
                    {

                        ((TextBox)rows.FindControl("txtRemarks")).Text = tempDt.Rows[k]["Remarks"].ToString();
                        k++;
                    }
                }

                ViewState["AfterRemovedDtrs"] = null;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadUpdatedItemCode");
                lblMessage.Text = clsException.ErrorMsg();
            }
        }

        protected void DisableCheckBox(ArrayList tempTcCodes)
        {
            try
            {

                foreach (GridViewRow rows in grdFaultTC.Rows)
                {
                    Label lblTCId = (Label)rows.FindControl("lblTCId");
                    foreach (var row in tempTcCodes)
                    {
                        if (Convert.ToString(row) == lblTCId.Text)
                        {
                            ((CheckBox)rows.FindControl("chkSelect")).Enabled = false;
                            ((CheckBox)rows.FindControl("chkSelect")).Checked = true;

                        }

                    }
                }
                ViewState["ArrTcId"] = tempTcCodes;
                ViewState["CHECKED_ITEMS"] = tempTcCodes;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "DisableCheckBox");
                lblMessage.Text = clsException.ErrorMsg();
            }
        }
        protected void grdloadcheckeddtr_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {

                if (e.CommandName == "Remove")
                {

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    //TextBox txtRemarks = (TextBox)e.FindControl("txtRemarks");
                    int iRowIndex = row.RowIndex;
                    Label sTcId = (Label)row.FindControl("lblTCId");
                    // DataTable dt = (DataTable)ViewState["TestDTR"];
                    DataTable dt = (DataTable)ViewState["dtUpdatedDtrs"];

                    if (ViewState["AfterRemovedDtrs"] == null)
                    {
                        //DataRow newRow = dt.NewRow();                   
                        //dt.Columns.Add("Remarks");
                        //DataRow dRow = dt.NewRow();
                        int j = 0;
                        foreach (GridViewRow rows in grdloadcheckeddtr.Rows)
                        {

                            TextBox Remarks = (TextBox)rows.FindControl("txtRemarks");
                            dt.Rows[j]["Remarks"] = Remarks.Text;

                            j++;
                        }
                        ViewState["AfterRemovedDtrs"] = dt;
                    }


                    dt.Rows[iRowIndex].Delete();

                    if (dt.Rows.Count == 0)
                    {
                        ViewState["dtUpdatedDtrs"] = null;
                        ViewState["CHECKED_ITEMS"] = null;
                    }
                    else
                    {
                        ViewState["dtUpdatedDtrs"] = dt;
                    }

                    grdloadcheckeddtr.Columns[8].Visible = false;
                    grdloadcheckeddtr.Columns[1].Visible = false;

                    //foreach (GridViewRow rows in grdloadcheckeddtr.Rows)
                    //{
                    //    Label lblTCId = (Label)rows.FindControl("lblTCId");
                    //    if (lblTCId.Text != sTcId.Text)
                    //    {
                    //        TextBox Remarks = (TextBox)rows.FindControl("txtRemarks");
                    //        ((TextBox)rows.FindControl("txtRemarks")).Text = Remarks.Text;
                    //    }

                    //}

                    grdloadcheckeddtr.DataSource = dt;
                    int numberOfRecords = dt.Rows.Count;
                    TxtTotalDtrs.Text = Convert.ToString(numberOfRecords);


                    grdloadcheckeddtr.DataBind();
                    if (dt.Rows.Count == 0)
                    {
                        txtOmdate.Text = string.Empty;
                        txtOmNo.Text = string.Empty;
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
                                EnableCheckBox(permTCId);
                                permTCId.RemoveAt(i);
                            }
                            i++;
                        }
                        ViewState["ArrTcId"] = permTCId;



                        DisableCheckBox(permTCId);



                        int k = 0;
                        foreach (GridViewRow rows in grdloadcheckeddtr.Rows)
                        {

                            ((TextBox)rows.FindControl("txtRemarks")).Text = dt.Rows[k]["Remarks"].ToString();
                            k++;
                        }
                        ViewState["AfterRemovedDtrs"] = null;

                    }
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }


        }
        protected void EnableCheckBox(ArrayList tempTcCodes)
        {
            try
            {

                foreach (GridViewRow rows in grdFaultTC.Rows)
                {
                    Label lblTCId = (Label)rows.FindControl("lblTCId");
                    if(tempTcCodes!=null)
                    {
                        foreach (var row in tempTcCodes)
                        {
                            if (Convert.ToString(row) == lblTCId.Text)
                            {
                                ((CheckBox)rows.FindControl("chkSelect")).Enabled = true;
                                ((CheckBox)rows.FindControl("chkSelect")).Checked = false;

                            }

                        }
                    }
          
                }
                ViewState["ArrTcId"] = tempTcCodes;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "DisableCheckBox");
                lblMessage.Text = clsException.ErrorMsg();
            }
        }
        protected void FTPUpload(object sender, EventArgs e, clsScrap objDtrUpload)
        {
            string strUpload = string.Empty;
            try
            {

                //FTP Folder name. Leave blank if you want to upload to root folder.
                // string ftpFolder = "Uploads/";       
                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);
                string sFtpvirtual = Convert.ToString(ConfigurationSettings.AppSettings["VirtualDirectoryDocs"]);

                DateTime currentDateTime = DateTime.Now;


                //string fileName = (currentDateTime.ToString("ddMMyyyyHHmm") + Path.GetFileName(fupdDoc.FileName)).Trim();
                string fileName = (Path.GetFileName(fupDoc.FileName)).Trim();
                string OMNO = Regex.Replace(txtOmNo.Text, @"[^0-9a-zA-Z]+", "");
                objDtrUpload.sfilename = fileName;

                if (fileName == "" || fileName == null)
                {
                    ShowMsgBox("Please select the File!");
                    return;
                }

                string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["UploadFormat"]);
                string sAnxFileExt = System.IO.Path.GetExtension(fupDoc.FileName).ToString().ToLower();
                sAnxFileExt = ";" + sAnxFileExt.Remove(0, 1) + ";";
                clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);

                bool Isuploaded;
                bool IsFileExiest;
                string sMainFolderName = SFTPmainfolder + "/" + "SCRAP_UPLOAD_DOCS/";

                fupDoc.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + fupDoc.FileName));
                string sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + fupDoc.FileName);
                Session["fupdDoc"] = fileName;

                if (File.Exists(sDirectory))
                {

                    bool IsExists = objFtp.FtpDirectoryExists(sMainFolderName);
                    if (IsExists == false)
                    {
                        objFtp.createDirectory(sMainFolderName);
                    }
                    IsExists = objFtp.FtpDirectoryExists(sMainFolderName + "/" + OMNO);
                    if (IsExists == false)
                    {
                        objFtp.createDirectory(sMainFolderName + "/" + OMNO);
                    }
                    //Isuploaded = objFtp.upload(sMainFolderName, fileName, sDirectory);
                    Isuploaded = objFtp.upload(sMainFolderName + "/" + OMNO + "/", fileName, sDirectory);

                    if (Isuploaded == true & File.Exists(sDirectory))
                    {
                        objDtrUpload.sFilepath = SFTPmainfolder + "SCRAP_UPLOAD_DOCS/" + fileName;
                        File.Delete(sDirectory);
                        sDirectory = fileName;
                        //ShowMsgBox("File Uploaded Successfully");
                        strUpload = "File:" + fileName + " Is Uploaded";
                        ViewState["Path"] = fileName;
                        txtfilepath.Text = sMainFolderName + "/" + OMNO + "/" + fileName;
                        //lblmsg.Text = strUpload;
                        return;

                    }

                }
                Session["FileUpload"] = null;
                //return strUpload;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }
        public void loadScrapDetails()
        {
            try
            {

                clsScrap objScrap = new clsScrap();

                string Viewomno = txtViewOmNo.Text;
                DataTable dt = objScrap.LoadScrapdetailsView(Viewomno);
                //grdviewscrapdtr.DataSource = dt;
                //grdviewscrapdtr.DataBind();
                //dvviewscrapdtr.Style.Add("display", "block");

                grdloadcheckeddtr.DataSource = dt;
                grdloadcheckeddtr.DataBind();

                grdloadcheckeddtr.Columns[0].Visible = false;
                grdloadcheckeddtr.Columns[11].Visible = false;
                grdloadcheckeddtr.Columns[13].Visible = false;
                txtOmNo.Text = txtViewOmNo.Text;
                txtOmdate.Text = txtViewOmdate.Text;
                txtOmNo.Enabled = false;
                BindgridView(SFTPmainfolder, sUserName, sPassword);
                dvcheckedTc.Style.Add("display", "block");
                dvHead.Style.Add("display", "block");
                dvdocument.Style.Add("display", "none");
                dvViewdocument.Style.Add("display", "block");

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }

        }

        protected void grdviewscrapdtr_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {
                if (e.CommandName == "Download")
                {

                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    string sTcCode = ((Label)row.FindControl("lblTCCode")).Text;
                    download(sTcCode);
                    grdFaultTC.Columns[8].Visible = false;

                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }


        }

        protected void grdviewscrapdtr_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {

                grdFaultTC.PageIndex = e.NewPageIndex;
                loadScrapDetails();


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdPendingTc_PageIndexChanging");
            }
        }


        public DataTable BindgridView(string FtpServer, string username, string password)
        {
            DataTable dtFiles = new DataTable();
            try
            {
                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);


                string OMNO = Regex.Replace(txtOmNo.Text, @"[^0-9a-zA-Z]+", "");
                FtpServer += "SCRAP_UPLOAD_DOCS/" + OMNO;

                clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);
                //path for get files from ftp
                bool IsExists = objFtp.FtpDirectoryExists(FtpServer);
                // checking related ponumber directory is there are not!

                if (IsExists == false)
                {
                    gvFiles.Visible = false;
                    return dtFiles;
                }
                else
                {
                    dtFiles = objFtp.GetListOfFiles(FtpServer);
                }
                gvFiles.DataSource = dtFiles;
                gvFiles.DataBind();

                return dtFiles;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtFiles;
            }
        }
        protected void DownloadFile(object sender, EventArgs e)
        {
            string fileName = (sender as LinkButton).CommandArgument;
            try
            {
                string SFTPmainfolderpath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["VirtualDirectoryDocs"]);
                string OMNO = Regex.Replace(txtOmNo.Text, @"[^0-9a-zA-Z]+", "");
                //string PONo = Regex.Replace(txtPONo.Text, @"[^0-9a-zA-Z]+", "");

                clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);

                string path = SFTPmainfolderpath + "SCRAP_UPLOAD_DOCS/" + OMNO + "/" + fileName;
                RegisterStartupScript("Print", "<script>window.open('" + path + "','_blank')</script>");
            }
            catch (Exception ex)
            {

            }
        }
        protected void DownloadFiledwnld(object sender, EventArgs e)
        {
            string fileName1 = (sender as LinkButton).CommandArgument;
            try
            {
                //Create a stream for the file
                Stream stream = null;

                //This controls how many bytes to read at a time and send to the client
                int bytesToRead = 10000;

                // Buffer to read bytes in chunk size specified above
                byte[] buffer = new Byte[bytesToRead];

                // The number of bytes read
                try
                {
                    string SFTPmainfolder = Convert.ToString(ConfigurationSettings.AppSettings["VirtualDirectoryDocs"]);

                    // string PoNo = Regex.Replace(txtPONo.Text, @"[^0-9a-zA-Z]+", "");
                    string OMNO = Regex.Replace(txtOmNo.Text, @"[^0-9a-zA-Z]+", "");
                    clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);

                    string url = SFTPmainfolder + "SCRAP_UPLOAD_DOCS/" + OMNO + "/" + fileName1;
                    string fileName = getFilename(url);
                    //Create a WebRequest to get the file
                    HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(url);

                    //Create a response for this request
                    HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();

                    if (fileReq.ContentLength > 0)
                        fileResp.ContentLength = fileReq.ContentLength;

                    //Get the Stream returned from the response
                    stream = fileResp.GetResponseStream();

                    // prepare the response to the client. resp is the client Response
                    var resp = HttpContext.Current.Response;

                    //Indicate the type of data being sent
                    resp.ContentType = "application/octet-stream";

                    //Name the file 
                    resp.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
                    resp.AddHeader("Content-Length", fileResp.ContentLength.ToString());

                    int length;
                    do
                    {
                        // Verify that the client is connected.
                        if (resp.IsClientConnected)
                        {
                            // Read data into the buffer.
                            length = stream.Read(buffer, 0, bytesToRead);

                            // and write it out to the response's output stream
                            resp.OutputStream.Write(buffer, 0, length);

                            // Flush the data
                            resp.Flush();

                            //Clear the buffer
                            buffer = new Byte[bytesToRead];
                        }
                        else
                        {
                            // cancel the download if client has disconnected
                            length = -1;
                        }
                    } while (length > 0); //Repeat until no data is read
                }
                finally
                {
                    if (stream != null)
                    {
                        //Close the input stream
                        stream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("(404) Not Found"))
                {
                    ShowMsgBox("File Not Found");
                }
                else
                {
                    lblMessage.Text = clsException.ErrorMsg();
                    clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                }
            }
        }
        private string getFilename(string hreflink)
        {
            Uri uri = new Uri(hreflink);

            string filename = System.IO.Path.GetFileName(uri.LocalPath);

            return filename;
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
                dt.Columns.Add("TC_PURCHASE_DATE");
                dt.Columns.Add("TC_STATUS");
                dt.Columns.Add("STD_REMARKS");
                dt.Columns.Add("TC_STAR_RATE");





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

        protected void grdFaultTC_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {
                if (e.CommandName == "Search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtdtrcode = (TextBox)row.FindControl("txtdtrcode");
                    TextBox txtdtrslno = (TextBox)row.FindControl("txtdtrslno");



                    DataTable dt = (DataTable)ViewState["TotalDTrs"];
                    dv = dt.DefaultView;

                    if (txtdtrcode.Text != "")
                    {
                        sFilter = "TC_CODE Like '%" + txtdtrcode.Text.Replace("'", "'") + "%' AND";
                    }

                    if (txtdtrslno.Text != "")
                    {
                        sFilter = "TC_SLNO Like '%" + txtdtrslno.Text.Replace("'", "'") + "%' AND";
                    }

                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdFaultTC.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {


                            grdFaultTC.DataSource = dv;
                            ViewState["ScrapDetails"] = dv.ToTable();
                            grdFaultTC.DataBind();

                        }
                        else
                        {
                            ViewState["ScrapDetails"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFaultTc();
                    }


                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }


        }
        protected void cmdClose_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ScrapView.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


    }
}


