using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Globalization;

namespace IIITS.DTLMS.PermanentDecomm
{
    public partial class Permanentestimationview : System.Web.UI.Page
    {
        string strFormCode = "Permanentestimationview";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;

                if (!IsPostBack)
                {
                    LoadAllDTC();
                    CheckAccessRights("4");
                }
            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdNew_Click(object sender, EventArgs e)
        {
            try
            {
              
                bool bAccResult = CheckAccessRights("2");
                if (bAccResult == false)
                {
                    return;
                }

                Response.Redirect("Permanentestimation.aspx?DTCId=" + "" + "&EstID=" + "" + "&ActionType=" + "", false);
               // Response.Redirect("Permanentestimation.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "Permanentestimationview";
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
                //dt.Columns.Add("DF_ID");
                dt.Columns.Add("PEST_ID"); 
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

                grdPermanentDecommDetails.DataSource = dt;
                grdPermanentDecommDetails.DataBind();

                int iColCount = grdPermanentDecommDetails.Rows[0].Cells.Count;
                grdPermanentDecommDetails.Rows[0].Cells.Clear();
                grdPermanentDecommDetails.Rows[0].Cells.Add(new TableCell());
                grdPermanentDecommDetails.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdPermanentDecommDetails.Rows[0].Cells[0].Text = "No Records Found";
        

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void grdPermanentDecomm_RowDataBound(object sender, GridViewRowEventArgs e)
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


                    if (lblStatus.Text == "YES")
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
                        Arr = CheckAlreadyEstimateEntry(lblDtcCode.Text);

                        if (Arr[1].ToString() == "2")
                        {
                           lnkWaiting.Visible = true;
                           // lnkEstPrev.Visible = true;
                            lnkCreate.Visible = false;
                            lnkCreateDTC.Enabled = false;
                            lnkCreateDTR.Enabled = false;
                        }
                        else
                        {
                            lnkWaiting.Visible = false;
                           // lnkEstPrev.Visible = false;

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

        public string[] CheckAlreadyEstimateEntry(string sDTCCode)
        {
            string[] Arr = new string[2];
            try
            {
                clsApproval objApproval = new clsApproval();
                bool bResult = objApproval.CheckAlreadyExistEntryId(sDTCCode, "62");
                if (bResult == true)
                {
                    Arr[0] = " Declare Already done for Transfromer Centre Code " + sDTCCode + ", Waiting for Approval";
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


        protected void grdPermanentDecomm_RowCommand(object sender, GridViewCommandEventArgs e)
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
                    Label lblEstId = (Label)row.FindControl("lblEstId");
                    Label lblDtcCode = (Label)row.FindControl("lblDtcCode");
                    Label lblProjectType = (Label)row.FindControl("lblProjectType");


                    // Check failure Entry Waiting For approval
                    string[] Arr = new string[2];
                    Arr = CheckAlreadyEstimateEntry(lblDtcCode.Text);
                    if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0].ToString());
                        return;
                    }


                    //Check Self Execution DTC to Declare 
                    Arr = CheckSelfExecutionDTC(lblDtcCode.Text, lblProjectType.Text);
                    if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0].ToString());
                        return;
                    }
                    string sEstimateID=string.Empty;
                    string sActionType = string.Empty;
                    string sDTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblDtcCode.Text));
                    
                    if (lblEstId.Text == "")
                    {
                        sEstimateID = "";
                    }
                    else
                    {
                        sEstimateID = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblEstId.Text));
                    }

                    
                    sActionType = "";
                    

                    
                   // string sFailureID = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblFailureId.Text));

                    Response.Redirect("Permanentestimation.aspx?DTCId=" + sDTCId + "&EstID=" + sEstimateID + "&ActionType=" + sActionType, false);

                }

                if (e.CommandName == "Preview1"||e.CommandName == "Preview")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    Label lblDTCCode = (Label)row.FindControl("lblDtcCode");
                    Label lblTCcode = (Label)row.FindControl("lblTcCode");

                    Label lblOldCap = (Label)row.FindControl("lblcapacity");
                    Label lblNewCap = (Label)row.FindControl("lblEnhancecapacity");

                    clsPermanentEstimation objPerestimate = new clsPermanentEstimation();

                    Label lblDtc = (Label)row.FindControl("lblId");
                    Label lblEstId = (Label)row.FindControl("lblEstId");
                    Label lblDtcCode = (Label)row.FindControl("lblDtcCode");
                    Label lblProjectType = (Label)row.FindControl("lblProjectType");


                    // Check failure Entry Waiting For approval
                    string[] Arr = new string[2];
                    Arr = CheckAlreadyEstimateEntry(lblDtcCode.Text);
                    if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0].ToString());
                        return;
                    }


                    //Check Self Execution DTC to Declare 
                    Arr = CheckSelfExecutionDTC(lblDtcCode.Text, lblProjectType.Text);
                    if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0].ToString());
                        return;
                    }
                    string sEstimateID = string.Empty;
                    string sActionType = string.Empty;
                    string sDTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblDtcCode.Text));

                    if (lblEstId.Text == "")
                    {
                        sEstimateID = "";
                    }
                    else
                    {
                        sEstimateID = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblEstId.Text));
                    }


                     sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt("VIEW"));

                    Response.Redirect("Permanentestimation.aspx?DTCId=" + sDTCId + "&EstID=" + sEstimateID + "&ActionType=" + sActionType, false);
                    
                }
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtCode = (TextBox)row.FindControl("txtDtCode");
                    TextBox txtDtName = (TextBox)row.FindControl("txtDtName");
                    TextBox txtDtrCode = (TextBox)row.FindControl("txtDtrCode");

                    DataTable dt = (DataTable)ViewState["PermanentDecomm"];
                    dv = dt.DefaultView;

                    if (txtDtCode.Text != "")
                    {
                        sFilter = "DT_CODE Like '%" + txtDtCode.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtDtName.Text != "")
                    {
                        sFilter += " DT_NAME Like '%" + txtDtName.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtDtrCode.Text != "")
                    {
                        sFilter += " TC_CODE Like '%" + txtDtrCode.Text.Replace("'", "`") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdPermanentDecommDetails.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdPermanentDecommDetails.DataSource = dv;
                            ViewState["PermanentDecomm"] = dv.ToTable();
                            grdPermanentDecommDetails.DataBind();

                        }
                        else
                        {
                            ViewState["PermanentDecomm"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        if (rdbAlready.Checked == true)
                        {
                            LoadAlreadyFailure();
                        }
                        else if (rdbViewAll.Checked == true)
                        {
                            LoadAllDTC();
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

        protected void grdPermanentDecomm_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdPermanentDecommDetails.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["PermanentDecomm"];
                grdPermanentDecommDetails.DataSource = SortDataTable(dt as DataTable, true);
                grdPermanentDecommDetails.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void grdPermanentDecomm_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdPermanentDecommDetails.PageIndex;
            DataTable dt = (DataTable)ViewState["PermanentDecomm"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdPermanentDecommDetails.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdPermanentDecommDetails.DataSource = dt;
            }
            grdPermanentDecommDetails.DataBind();
            grdPermanentDecommDetails.PageIndex = pageIndex;
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


        public void LoadAllDTC()
        {
            try
            {
                clsPermanentEstimation objperestimate = new clsPermanentEstimation();
                if (objSession.OfficeCode == "" || objSession.OfficeCode == null)
                {
                    objSession.OfficeCode = "";
                }
                else
                {
                    objperestimate.sOfficeCode = objSession.OfficeCode;
                }
            //    objperestimate.sOfficeCode = objSession.OfficeCode;
                DataTable dtDetails = objperestimate.LoadAllDTCEstimate(objperestimate);

                grdPermanentDecommDetails.DataSource = dtDetails;
                grdPermanentDecommDetails.DataBind();
                ViewState["PermanentDecomm"] = dtDetails;

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

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
                        ViewState["PermanentDecomm"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["PermanentDecomm"] = dataView.ToTable();

                    }


                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }

        public string[] CheckSelfExecutionDTC(string sDTCCode, string sProjectType)
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

        public void LoadAlreadyFailure()
        {
            try
            {
                clsPermanentEstimation objFailure = new clsPermanentEstimation();
                objFailure.sOfficeCode = objSession.OfficeCode;
                DataTable dtDetails = objFailure.LoadAlreadyFailure(objFailure);
                grdPermanentDecommDetails.DataSource = dtDetails;
                grdPermanentDecommDetails.DataBind();
                ViewState["PermanentDecomm"] = dtDetails;

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
                LoadAllDTC();
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
    }
}