using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;


namespace IIITS.DTLMS.Transaction
{
    public partial class DTCTracker : System.Web.UI.Page
    {
        string strFormCode = "DTCTracker";
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
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");
                txtToDate_CalendarExtender1.EndDate = System.DateTime.Now;
                txtFromDate_CalendarExtender1.EndDate = System.DateTime.Now;

                if (!IsPostBack)
                {
                    string strQry = string.Empty;
                    strQry += "Title=Search and Select DTC CODE Details&";
                    strQry += "Query=select  \"DT_CODE\", \"DT_NAME\" FROM \"TBLDTCMAST\"  WHERE CAST(\"DT_OM_SLNO\" AS TEXT) LIKE '" + objSession.OfficeCode + "%' AND {0} like %{1}% order by \"DT_CODE\" limit 500 &";
                    strQry += "DBColName=CAST(\"DT_CODE\" AS TEXT)~CAST(\"DT_NAME\" AS TEXT)&";
                    strQry += "ColDisplayName=DTC Code~DTC Name&";

                    strQry = strQry.Replace("'", @"\'");

                    btnSearchId.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtDTCCode.ClientID + "&btn=" + btnSearchId.ClientID + "',520,520," + txtDTCCode.ClientID + ")");


                    txtFromDate.Attributes.Add("onblur", "return ValidateDate(" + txtFromDate.ClientID + ");");
                    txtToDate.Attributes.Add("onblur", "return ValidateDate(" + txtToDate.ClientID + ");");


                    //From DTC Commission Form
                    if (Request.QueryString["DTCCode"] != null && Request.QueryString["DTCCode"].ToString() != "")
                    {
                        txtDTCCode.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["DTCCode"]));
                        cmdLoad_Click(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
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

        bool ValidateForm()
        {
            bool bValidate = false;
            try
            {
                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    string sResult = Genaral.DateComparision(txtToDate.Text, txtFromDate.Text, false, false);
                    if (sResult == "2")
                    {
                        ShowMsgBox("To Date should be Greater than From Date");
                        return bValidate;
                    }
                }
                if (txtDTCCode.Text == "")
                {
                    ShowMsgBox("Please enter the DTC Code");
                    txtDTCCode.Focus();
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

        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtTcDetails = new DataTable();
                clsDTCTracker objTracker = new clsDTCTracker();
                objTracker.sDTCCode = txtDTCCode.Text;
                objTracker.sFromDate = txtFromDate.Text;
                objTracker.sToDate = txtToDate.Text;
                if (cmbType.SelectedIndex > 0)
                {
                    objTracker.sTaskType = cmbType.SelectedValue;
                }

                if (ValidateForm() == true)
                {
                    objTracker.GetDTCTrackstatus(objTracker);
                    txtLoad.Text = objTracker.sConnectedLoad;
                    txtDTCCode.Text = objTracker.sDTCCode;
                    txtDTCName.Text = objTracker.sDTCName;
                    txtDTrCode.Text = objTracker.sDTRCode;
                    grdDTcDetails.DataSource = objTracker.dTracker;
                    grdDTcDetails.DataBind();
                    ViewState["DTCTracker"] = objTracker.dTracker;
                }
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }       

        protected void grdDTcDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdDTcDetails.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["DTCTracker"];
                grdDTcDetails.DataSource = SortDataTable(dt as DataTable, true);
                grdDTcDetails.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdDTcDetails_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdDTcDetails.PageIndex;
            DataTable dt = (DataTable)ViewState["DTCTracker"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdDTcDetails.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdDTcDetails.DataSource = dt;
            }
            grdDTcDetails.DataBind();
            grdDTcDetails.PageIndex = pageIndex;
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
                        ViewState["DTCTracker"] = dataView.ToTable();
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["DTCTracker"] = dataView.ToTable();
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


        protected void grdDTcDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                //DRT_ACT_REFTYPE   1---> New DTC Entry  2---> Failure  3-->After RI

                if (e.CommandName == "View")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();
                    string sUrl = string.Empty;
                    string sUrlPath = string.Empty;
                    string sDTCId = string.Empty;

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    Label lblRefType = (Label)row.FindControl("lblRefType");
                    Label lblRefNo = (Label)row.FindControl("lblRefNo");
                    Label lblDTrStatus = (Label)row.FindControl("lblDTrStatus");
                    Label lblDTCCode = (Label)row.FindControl("lblDTCCode");

                    switch (lblRefType.Text)
                    {
                        case "1":

                            sDTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblRefNo.Text));

                            sUrlPath = "/MasterForms/DTCCommision.aspx?QryDtcId=" + sDTCId;
                            //sUrl = "window.open('" + sUrlPath + "', 'popup_window', 'width=300,height=100,left=100,top=100,resizable=yes');";
                            sUrl = "window.open('" + sUrlPath + "', '_blank');";
                            ClientScript.RegisterStartupScript(this.GetType(), "script", sUrl, true);
                            //Response.Redirect("/MasterForms/TcMaster.aspx?TCId=" + sTCId, false);
                            break;

                        case "2":

                           clsFormValues objForm = new clsFormValues();
                           objForm.sFailureId = lblRefNo.Text;
                           sDTCId = objForm.GetDTCId(objForm);
                           string sFailureId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblRefNo.Text));
                           sDTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sDTCId));

                           if (lblDTrStatus.Text == "3")
                           {
                               sUrlPath = "/DTCFailure/FailureEntry.aspx?FailureId=" + sFailureId + "&DTCId=" + sDTCId;
                              
                           }
                           else
                           {
                               sUrlPath = "/DTCFailure/Enhancement.aspx?EnhanceId=" + sFailureId + "&DTCId=" + sDTCId;
                           }

                           sUrl = "window.open('" + sUrlPath + "', '_blank');";
                           ClientScript.RegisterStartupScript(this.GetType(), "script", sUrl, true);
                           //Response.Redirect("/DTCFailure/FailureEntry.aspx?FailureId=" + sFailureId + "&DTCId=" + sDTCId, false);
                           break;

                        case "3":

                           string sMappingId = lblRefNo.Text;

                           sMappingId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sMappingId));

                           sUrlPath = "/MasterForms/DtcMapping.aspx?MappingId=" + sMappingId;
                           sUrl = "window.open('" + sUrlPath + "', '_blank');";
                           ClientScript.RegisterStartupScript(this.GetType(), "script", sUrl, true);
                           break;

                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void lnkDTCDetails_Click(object sender, EventArgs e)
        {
            try
            {
                clsDTCTracker objTraker = new clsDTCTracker();
                string sDTCId = objTraker.GetDTCIdFromCode(txtDTCCode.Text);

                sDTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sDTCId));

                string url = "/MasterForms/DTCCommision.aspx?QryDtcId=" + sDTCId;
                string s = "window.open('" + url + "', '_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void Export_clickDTCtracker(object sender, EventArgs e)
        {

             //DataTable dtTcDetails = new DataTable();
             //   clsDTCTracker objTracker = new clsDTCTracker();
             //   objTracker.sDTCCode = txtDTCCode.Text;
             //   objTracker.sFromDate = txtFromDate.Text;
             //   objTracker.sToDate = txtToDate.Text;
             //   if (cmbType.SelectedIndex > 0)
             //   {
             //       objTracker.sTaskType = cmbType.SelectedValue;
             //   }
             //   if (ValidateForm() == true)
             //   {

             //       objTracker.GetDTCTrackstatus(objTracker);
             //       txtLoad.Text = objTracker.sConnectedLoad;
             //       txtDTCCode.Text = objTracker.sDTCCode;
             //       txtDTCName.Text = objTracker.sDTCName;
             //       txtDTrCode.Text = objTracker.sDTRCode;

             //       DataTable dt = objTracker.dTracker;

            DataTable dt = (DataTable)ViewState["DTCTracker"];

                    if (dt!=null && dt.Rows.Count > 0)
                    {

                        dt.Columns["TRANSDATE"].ColumnName = "DATE";
                        dt.Columns["DCT_DTR_CODE"].ColumnName = "DTR Code";
                        dt.Columns["STATUS"].ColumnName = "STATUS";
                        // dt.Columns["SI_FROM_STORE"].ColumnName = "FROM STORE";


                        List<string> listtoRemove = new List<string> { "DCT_ACT_REFTYPE", "DCT_DTR_STATUS", "DCT_ACT_REFNO", "DCT_DTC_CODE" };
                        string filename = "DTCtrackerDetails" + DateTime.Now + ".xls";
                        string pagetitle = "DTC Tracker Details";

                        Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                    }

                    else
                    {
                        ShowMsgBox("No record found");
                    }
                }
                
    }
}