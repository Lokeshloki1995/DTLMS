using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Globalization;

namespace IIITS.DTLMS.DTCFailure
{
    public partial class FailureEntryView : System.Web.UI.Page
    {
        string strFormCode = "FailureEntryView";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
           
            try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                else
                {
                    objSession = (clsSession)Session["clsSession"];
                    lblMessage.Text = string.Empty;
                    

                    if (!IsPostBack)
                    {
                        if (CheckAccessRights("4"))
                        {
                            LoadAllDTCFailure();
                        }
                       
                    }
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
                Response.Redirect("FailureEntry.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }




        public void LoadAllDTCFailure()
        {
            try
            {
               clsFailureEntry objFailure = new clsFailureEntry();
               objFailure.sOfficeCode = objSession.OfficeCode;
               DataTable dtDetails = objFailure.LoadAllDTCFailure(objFailure);

               grdFailureDetails .DataSource = dtDetails;
               grdFailureDetails.DataBind();
               ViewState["Failure"] = dtDetails;

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }



        public void LoadAlreadyFailure()
        {
            try
            {
                clsFailureEntry objFailure = new clsFailureEntry();
                objFailure.sOfficeCode = objSession.OfficeCode;
                DataTable dtDetails = objFailure.LoadAlreadyFailure(objFailure);
                grdFailureDetails.DataSource = dtDetails;
                grdFailureDetails.DataBind();
                ViewState["Failure"] = dtDetails;

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }


        protected void Export_ClickFailureEntry(object sender, EventArgs e)
        {
            //DataTable dtDetails=new DataTable();

            //if (rdbViewAll.Checked)
            //{
            //clsFailureEntry objFailure = new clsFailureEntry();
            //objFailure.sOfficeCode = objSession.OfficeCode;
            // dtDetails = objFailure.LoadAllDTCFailure(objFailure);
            //}
            //else{

            //clsFailureEntry objFailure = new clsFailureEntry();
            //objFailure.sOfficeCode = objSession.OfficeCode;
            // dtDetails = objFailure.LoadAlreadyFailure(objFailure);
            //}

            DataTable dtDetails = (DataTable)ViewState["Failure"];

            if (dtDetails.Rows.Count > 0)
            {

                dtDetails.Columns["DT_CODE"].ColumnName = "TRANSFROMER CENTRE CODE";
                dtDetails.Columns["DT_NAME"].ColumnName = "TRANSFROMER CENTRE NAME";
                dtDetails.Columns["TC_CODE"].ColumnName = "DTR CODE";
                dtDetails.Columns["TM_NAME"].ColumnName = "MAKE NAME";
                dtDetails.Columns["TC_CAPACITY"].ColumnName = "Capacity(in KVA)";
                

                //dtLoadDetails.Columns["FEEDER NAME"].SetOrdinal(0);

                List<string> listtoRemove = new List<string> { "DF_ID", "DT_ID", "TC_SLNO", "STATUS", "DT_PROJECTTYPE" };
                string filename = "AllFailureDetails" + DateTime.Now + ".xls";
                string pagetitle = "Failure Details View";

                Genaral.getexcel(dtDetails, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");
                ShowEmptyGrid();
            }



        }


        protected void grdFailureDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdFailureDetails.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Failure"];
                grdFailureDetails.DataSource = SortDataTable(dt as DataTable, true);
                grdFailureDetails.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdFailureDetails_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdFailureDetails.PageIndex;
            DataTable dt = (DataTable)ViewState["Failure"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdFailureDetails.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdFailureDetails.DataSource = dt;
            }
            grdFailureDetails.DataBind();
            grdFailureDetails.PageIndex = pageIndex;
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
                        ViewState["Failure"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["Failure"] = dataView.ToTable();

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


        //protected void cmbIndexSelection_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (cmbIndexSelection.SelectedValue == "0")
        //    {
        //        LoadToBeCreated();

        //    }
        //    else
        //    {
        //        LoadAlreadyCreated();

        //    }
        //}

        protected void grdFailureDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string flag;
            try
            {
               
                if (e.CommandName == "Create" || e.CommandName == "CreateNew")
                {
                    bool bAccResult;
                    if (e.CommandName == "CreateNew")
                    {
                        //Check AccessRights
                         bAccResult = CheckAccessRights("2");
                         if (bAccResult == false)
                         {
                             return;
                         }
                    }
                                       
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    Label lblDtc = (Label)row.FindControl("lblId");
                    Label lblFailureId = (Label)row.FindControl("lblFailureId");
                    Label lblDtcCode = (Label)row.FindControl("lblDtcCode");
                    Label lblProjectType = (Label)row.FindControl("lblProjectType");
                    

                    // Check failure Entry Waiting For approval
                    string[] Arr = new string[2];
                    Arr = CheckAlreadyFailureEnry(lblDtcCode.Text);
                    if (Arr[1].ToString()=="2")
                    {
                        ShowMsgBox(Arr[0].ToString());
                        return;
                    }


                    //Check Self Execution DTC to Declare Failure
                    Arr = CheckSelfExecutionDTC(lblDtcCode.Text, lblProjectType.Text);
                    if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0].ToString());
                        return;
                    }

                    string sDTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblDtc.Text));
                    string  sFailureID = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblFailureId.Text));

                    Response.Redirect("FailureEntry.aspx?DTCId=" + sDTCId + "&FailureId=" + sFailureID, false);
                   
                }

                if (e.CommandName == "Preview")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    Label lblDTCCode = (Label)row.FindControl("lblDtcCode");
                    Label lblTCcode = (Label)row.FindControl("lblTcCode");

                    Label lblOldCap = (Label)row.FindControl("lblcapacity");
                    Label lblNewCap = (Label)row.FindControl("lblEnhancecapacity");

                    clsFailureEntry objFailure = new clsFailureEntry();

                    string sDTCcode = lblDTCCode.Text;
                    string sTCcode = lblTCcode.Text;
                    string sOfcCode = objSession.OfficeCode;
                    string sWoID = getWoID(sDTCcode,sOfcCode);
                    string strParam1 = string.Empty;
                    //strParam = "id=Estimation&FailureId=" + txtFailurId.Text;
                    strParam1 = "id=PgrsDocketSO&sWFOID=" + sWoID;
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam1 + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                }
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtCode = (TextBox)row.FindControl("txtDtCode");
                    TextBox txtDtName = (TextBox)row.FindControl("txtDtName");
                    TextBox txtDtrCode = (TextBox)row.FindControl("txtDtrCode");
                    TextBox txtTimsCode = (TextBox)row.FindControl("txtTimsCode");                    

                    DataTable dt = (DataTable)ViewState["Failure"];
                    dv = dt.DefaultView;

                    if (e.CommandName == "search")
                    {
                        clsFailureEntry objdetails = new clsFailureEntry();
                        objdetails.sDtcCode = txtDtCode.Text.Trim();
                        objdetails.sDtcTcSlno = txtTimsCode.Text.Trim();
                        objdetails.sDtcName = txtDtName.Text.Trim();
                        objdetails.sDtcTcCode = txtDtrCode.Text.Trim();
                        objdetails.sOfficeCode = objSession.OfficeCode;
                        dt = objdetails.LoadsearchDTCFailure(objdetails);
                        if(dt.Rows.Count>0)
                        { 
                        grdFailureDetails.DataSource = dt;
                        grdFailureDetails.DataBind();
                        }
                        else
                        {
                            ViewState["Failure"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }

                    //if (txtDtCode.Text != "")
                    //{
                    //    sFilter = "DT_CODE Like '%" + txtDtCode.Text.Replace("'", "`") + "%' AND";
                    //}
                    //if (txtDtName.Text != "")
                    //{
                    //    sFilter += " DT_NAME Like '%" + txtDtName.Text.Replace("'", "`") + "%' AND";
                    //}
                    //if (txtDtrCode.Text != "")
                    //{
                    //    sFilter += " TC_CODE Like '%" + txtDtrCode.Text.Replace("'", "`") + "%' AND";
                    //}
                    //if(txtTimsCode.Text != "")
                    //{
                    //    sFilter += " DT_TIMS_CODE Like '%" + txtTimsCode.Text.Replace("'", "`") + "%' AND";
                    //}
                    //if (sFilter.Length > 0)
                    //{
                    //    sFilter = sFilter.Remove(sFilter.Length - 3);
                    //    grdFailureDetails.PageIndex = 0;
                    //    dv.RowFilter = sFilter;
                    //    if (dv.Count > 0)
                    //    {
                    //        grdFailureDetails.DataSource = dv;
                    //        ViewState["Failure"] = dv.ToTable();
                    //        grdFailureDetails.DataBind();

                    //    }
                    //    else
                    //    {
                    //        ViewState["Failure"] = dv.ToTable();
                    //        ShowEmptyGrid();
                    //    }
                    //}
                    else
                    {
                        if (rdbAlready.Checked == true)
                        {
                            LoadAlreadyFailure();
                        }
                        else if (rdbViewAll.Checked == true)
                        {
                            LoadAllDTCFailure();
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

        protected void rdbViewAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LoadAllDTCFailure();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void rdbAlready_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LoadAlreadyFailure();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdFailureDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    
                    LinkButton lnkUpdate = (LinkButton)e.Row.FindControl("lnkUpdate");
                    LinkButton lnkCreate = (LinkButton)e.Row.FindControl("lnkCreate");
                    LinkButton lnkCreateDTC = (LinkButton)e.Row.FindControl("lnkCreateDTC");
                    LinkButton lnkCreateDTR = (LinkButton)e.Row.FindControl("lnkCreateDTR");
                    Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                    Label lblDtcCode = (Label)e.Row.FindControl("lblDtcCode");
                    LinkButton lnkWaiting = (LinkButton)e.Row.FindControl("lnkWaiting");
                    LinkButton lnkEstPrev = (LinkButton)e.Row.FindControl("lnkEstPrev");
                 
                   
                    if (lblStatus.Text  == "YES")
                    {
                        lnkUpdate.Visible = true;
                        lnkCreate.Visible = false;
                        lnkCreateDTC.Enabled = false;
                        lnkCreateDTR.Enabled = false;
                    }
                    else
                    {
                        lnkUpdate.Visible = false;
                        lnkCreate.Visible = true;
                        lnkCreateDTC.Enabled = true;
                        lnkCreateDTR.Enabled = true;

                        // Check DTC in waiting for approval

                        string[] Arr = new string[2];
                        Arr = CheckAlreadyFailureEnry(lblDtcCode.Text);

                        if (Arr[1].ToString() == "2")
                        {
                            lnkWaiting.Visible = true;
                            lnkEstPrev.Visible = true;
                            lnkCreate.Visible = false;
                            lnkCreateDTC.Enabled = false;
                            lnkCreateDTR.Enabled = false;
                        }
                        else
                        {
                            lnkWaiting.Visible = false;
                            lnkEstPrev.Visible = false;
                         
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

        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "FailureEntry";
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sAccessType = "1" + "," + sAccessType;
                bool bResult = objApproval.CheckAccessRights(objApproval);
                if (bResult == false)
                {
                    if (sAccessType == "4" )
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

        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("DF_ID");
                dt.Columns.Add("DT_ID");
                dt.Columns.Add("DT_CODE");
                dt.Columns.Add("DT_NAME");
                dt.Columns.Add("TC_CODE");
                dt.Columns.Add("TC_SLNO");
                dt.Columns.Add("TM_NAME");
                dt.Columns.Add("TC_CAPACITY");
                dt.Columns.Add("STATUS");
                dt.Columns.Add("DT_PROJECTTYPE");
                dt.Columns.Add("DT_TIMS_CODE");
                dt.Columns.Add("SLNO");

                grdFailureDetails.DataSource = dt;
                grdFailureDetails.DataBind();

                int iColCount = grdFailureDetails.Rows[0].Cells.Count;
                grdFailureDetails.Rows[0].Cells.Clear();
                grdFailureDetails.Rows[0].Cells.Add(new TableCell());
                grdFailureDetails.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdFailureDetails.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }

        public string[] CheckAlreadyFailureEnry(string sDTCCode)
        {
            string[] Arr = new string[2];
            try
            {
                clsApproval objApproval = new clsApproval();
                bool bResult = objApproval.CheckAlreadyExistEntry(sDTCCode, "9");
                if (bResult == true)
                {
                    Arr[0] = "Failure Declare Already done for Transfromer Centre Code " + sDTCCode + ", Waiting for Approval";
                    Arr[1] = "2";
                    return Arr;
                  
                }

                bResult = objApproval.CheckAlreadyExistEntry(sDTCCode, "10");
                if (bResult == true)
                {
                    Arr[0] = "Capacity Enhancement Already done for Transfromer Centre Code " + sDTCCode + ", Waiting for Approval";
                    Arr[1] = "2";
                    return Arr;
                
                }


                Arr[0] = "Success";
                Arr[1] = "0";
                return Arr;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;

            }
        }


        public string[] CheckSelfExecutionDTC(string sDTCCode,string sProjectType)
        {
            string[] Arr = new string[2];
            try
            {
                clsDTCCommision objDTCComm = new clsDTCCommision();
                objDTCComm.sDtcCode = sDTCCode;
                objDTCComm.sProjecttype = sProjectType;

                bool bResult = objDTCComm.CheckSelfExecutionSchemeType(objDTCComm);
                if (bResult == true)
                {
                    Arr[0] = "Project/Scheme Type of  Transfromer Centre Code " + sDTCCode + " is Self Execution so cannot declare Transfromer Centre Failure untill one Year";
                    Arr[1] = "2";
                    return Arr;
                }

                Arr[0] = "Success";
                Arr[1] = "0";
                return Arr;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }
        public string getWoID(string sDtcCode, string sOfcCode)
        {
            string sWOId = string.Empty;
            clsFailureEntry objFailure = new clsFailureEntry();

            try
            {
                sWOId = objFailure.getWoIDforEstimation(sOfcCode, sDtcCode);
                return sWOId;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return sWOId;
            }
        }
    }
}