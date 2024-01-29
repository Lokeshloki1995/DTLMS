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
    public partial class ScrapEntry : System.Web.UI.Page
    {
        string strFormCode = "ScrapEntry";
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
                txtOMDate.Attributes.Add("readonly", "readonly");
                CalendarExtender2.EndDate = System.DateTime.Now;
                if (!IsPostBack)
                {
                    if (Request.QueryString["TcId"] != null && Request.QueryString["TcId"].ToString() != "")
                    {
                        //txtSelectedTcId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TcId"]));
                        txtSelectedTcId.Text = Convert.ToString(Session["TCId"]);
                        LoadFaultTc();
                    }                  

                    string strQry = string.Empty;
                    strQry = "Title=Search and Select Transformer Details&";
                    strQry += "Query=SELECT \"TC_CODE\",\"TC_SLNO\",\"TM_NAME\" FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\"  WHERE  \"TC_LOCATION_ID\" LIKE '" + objSession.sStoreID + "' AND ";
                    strQry+= " \"TC_MAKE_ID\"= \"TM_ID\" and \"TC_STATUS\" =3 AND  \"TC_CURRENT_LOCATION\" =1  ";
                    strQry += " AND CAST(\"TC_CODE\" AS TEXT) NOT IN (SELECT \"WO_DATA_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" ='30' AND \"WO_PREV_APPROVE_ID\" ='0')";
                    strQry += " AND {0} like %{1}% order by \"TC_CODE\"&";
                    strQry += "DBColName=CAST(\"TC_CODE\" AS TEXT)~CAST(\"TC_SLNO\" AS TEXT)~CAST(\"TM_NAME\" AS TEXT)&";
                    strQry += "ColDisplayName=DTr CODE~DTr SLNO~MAKE NAME&";

                    strQry = strQry.Replace("'", @"\'");

                    //cmdSearchTC.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtTcCode.ClientID + "&btn=" + cmdSearchTC.ClientID + "',520,520," + txtTcCode.ClientID + ")");


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

        //protected void cmdSearchTC_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        clsDtcMaster objDtcMaster = new clsDtcMaster();

        //        objDtcMaster.sTcCode = txtTcCode.Text;

        //        objDtcMaster.GetTCDetails(objDtcMaster);

        //        txtMake.Text = objDtcMaster.sTCMakeName;
        //        txtTcCode.Text = objDtcMaster.sTcCode;
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdSearchTC_Click");
        //    }
        //}

       

        public void LoadFaultTc()
        {
            try
            {
                DataTable dt = new DataTable();

                clsScrap objScrap= new clsScrap();

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
                dt = objScrap.LoadTCForScrap(objScrap);
                grdFaultTC.DataSource = dt;
                ViewState["FaultTC"] = dt;
                grdFaultTC.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
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

                clsScrap objTcRepair = new clsScrap();
                string[] Arr = new string[2];
                int i = 0;
                if (ValidateForm() == true)
                {
                    objTcRepair.sRemarks=txtRemarks.Text.Trim().Replace("'","''");
                    objTcRepair.sCrby = objSession.UserId;
                    objTcRepair.sScrapId = txtScrapId.Text;
             

                    // string sWONo = txtWoNo1.Text.Trim().Replace("'", "") + "/" + txtWoNo2.Text.Trim().Replace("'", "") + "/" + txtWoNo3.Text.Trim().Replace("'", "");
                    objTcRepair.sWorkOrderNo = txtOMNo.Text.Trim().ToUpper();
                    objTcRepair.sWorkOrderDate = txtOMDate.Text;
                    objTcRepair.sOfficeCode = objSession.OfficeCode;

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
                        ShowMsgBox("No Transformer Exists to do Scrap Entry");
                        return;
                    }


                    objTcRepair.sDTrCount = i;
                    Arr = objTcRepair.SaveScrapEntry(strQryVallist,objTcRepair);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (ScrapEntry) Scrap ");
                    }

                    if (Arr[1].ToString() == "0")
                    {
                        ShowMsgBox(Arr[0].ToString());
                        Reset();
                        //txtTcCode.Text = string.Empty;
                        //txtMake.Text = string.Empty;
                        grdFaultTC.DataSource = null;
                        grdFaultTC.DataBind();
                        txtSelectedTcId.Text = string.Empty;
                        ViewState["FaultTC"] = null;
                        return;
                    }
                    else if (Arr[1].ToString() == "1")
                    {
                        ShowMsgBox(Arr[0].ToString());
                        return;
                    }
                    else  if (Arr[1].ToString() == "2")
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
                clsDTrRepairActivity objTCRepair = new clsDTrRepairActivity();

                if (ValidateGridValue(sTcCode) == true)
                {

                    objTCRepair.sTcCode = sTcCode;

                    objTCRepair.GetFaultTCDetails(objTCRepair);

                    if (ViewState["FaultTC"] != null)
                    {
                        DataTable dtFaultTc = (DataTable)ViewState["FaultTC"];
                        DataRow drow;
                        if (dtFaultTc.Rows.Count > 0)
                        {
                            drow = dtFaultTc.NewRow();

                            drow["TC_ID"] = objTCRepair.sTcId;
                            drow["TC_CODE"] = objTCRepair.sTcCode;
                            drow["TC_SLNO"] = objTCRepair.sTcSlno;
                            drow["TM_NAME"] = objTCRepair.sMakeName;
                            drow["TC_CAPACITY"] = objTCRepair.sCapacity;
                            drow["TC_MANF_DATE"] = objTCRepair.sManfDate;
                            drow["TC_PURCHASE_DATE"] = objTCRepair.sPurchaseDate;
                            drow["TC_WARANTY_PERIOD"] = objTCRepair.sWarrantyPeriod;
                            drow["TS_NAME"] = objTCRepair.sSupplierName;

                            dtFaultTc.Rows.Add(drow);
                            grdFaultTC.DataSource = dtFaultTc;
                            grdFaultTC.DataBind();
                            ViewState["FaultTC"] = dtFaultTc;
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

                        drow["TC_ID"] = objTCRepair.sTcId;
                        drow["TC_CODE"] = objTCRepair.sTcCode;
                        drow["TC_SLNO"] = objTCRepair.sTcSlno;
                        drow["TM_NAME"] = objTCRepair.sMakeName;
                        drow["TC_CAPACITY"] = objTCRepair.sCapacity;
                        drow["TC_MANF_DATE"] = objTCRepair.sManfDate;
                        drow["TC_PURCHASE_DATE"] = objTCRepair.sPurchaseDate;
                        drow["TC_WARANTY_PERIOD"] = objTCRepair.sWarrantyPeriod;
                        drow["TS_NAME"] = objTCRepair.sSupplierName;

                        dtFaultTc.Rows.Add(drow);
                        grdFaultTC.DataSource = dtFaultTc;
                        grdFaultTC.DataBind();
                        ViewState["FaultTC"] = dtFaultTc;

                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public bool ValidateGridValue(string sTcCode)
        {
            bool bValidate = false;
            try
            {
                ArrayList objArrlist = new ArrayList();

                foreach (GridViewRow row in grdFaultTC.Rows)
                {
                    objArrlist.Add(((Label)row.FindControl("lblTCCode")).Text.Trim());
                }

                if (objArrlist.Contains(sTcCode))
                {
                    ShowMsgBox("Transformer Already Added");
                    DataTable dtFaultTc = (DataTable)ViewState["FaultTC"];
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

        //protected void cmdLoad_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (txtTcCode.Text  == "")
        //        {
        //            ShowMsgBox("Select Transformer Code");
        //            return;
        //        }
        //        AddTCtoGrid(txtTcCode.Text);


        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdLoad_Click");
        //    }
        //}

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
                //txtWoNo1.Text = string.Empty;
                //txtWoNo2.Text = string.Empty;
                txtOMNo.Text = string.Empty;
                txtOMDate.Text = string.Empty;
                txtRemarks.Text = string.Empty;
              
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
                if (txtOMNo.Text.Trim().Length == 0 )
                {
                   
                    ShowMsgBox("Enter  OM Number");
                    return bValidate;
                }

                if (txtOMDate.Text.Trim() == "")
                {
                    txtOMDate.Focus();
                    ShowMsgBox("Enter OM Date");
                    return bValidate;
                }
                if (txtRemarks.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Valid Remarks");
                    txtRemarks.Focus();
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

        protected void cmdClose_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("TCSearch.aspx",false);

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

                objApproval.sFormName = "ScrapEntry";
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

                    DataTable dt = (DataTable)ViewState["FaultTC"];
                    dt.Rows[iRowIndex].Delete();
                    if (dt.Rows.Count == 0)
                    {
                        ViewState["FaultTC"] = null;
                    }
                    else
                    {
                        ViewState["FaultTC"] = dt;
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

                txtOMDate.Text = objScrap.sWorkOrderDate;
                txtRemarks.Text = objScrap.sRemarks;
                txtOMNo.Text = objScrap.sWorkOrderNo;
                //txtWoNo2.Text = objScrap.sWorkOrderNo.Split('/').GetValue(1).ToString();
                //txtWoNo3.Text = objScrap.sWorkOrderNo.Split('/').GetValue(2).ToString();

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

        //protected void grdFaultTC_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    try
        //    {
        //        grdFaultTC.PageIndex = e.NewPageIndex;
        //        DataTable dt = (DataTable)ViewState["FaultTC"];
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