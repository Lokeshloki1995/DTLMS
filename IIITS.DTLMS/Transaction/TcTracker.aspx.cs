using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Data.OleDb;
namespace IIITS.DTLMS.Transaction
{
    public partial class TcTracker : System.Web.UI.Page
    {
        string strFormCode = "TcTracker";
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
                    strQry += "Title=Search and Select DTR Code Details&";
                    strQry += "Query=select  \"TC_CODE\",\"TC_SLNO\" FROM \"TBLTCMASTER\"  WHERE CAST(\"TC_LOCATION_ID\"  AS TEXT) LIKE '" + objSession.OfficeCode + "%' AND {0} like %{1}% order by \"TC_CODE\" &";
                    strQry += "DBColName=CAST(\"TC_CODE\" AS TEXT)~CAST(\"TC_SLNO\" AS TEXT)&";
                    strQry += "ColDisplayName=DTr Code~DTr Slno&";

                    strQry = strQry.Replace("'", @"\'");

                    btnSearchId.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtTcCode.ClientID + "&btn=" + btnSearchId.ClientID + "',520,520," + txtTcCode.ClientID + ")");                    

                    txtFromDate.Attributes.Add("onblur", "return ValidateDate(" + txtFromDate.ClientID + ");");
                    txtToDate.Attributes.Add("onblur", "return ValidateDate(" + txtToDate.ClientID + ");");

                    //From DTC Commission Form
                    if (Request.QueryString["TCCode"] != null && Request.QueryString["TCCode"].ToString() != "")
                    {
                        txtTcCode.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TCCode"]));
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
                if(txtFromDate.Text!="" && txtToDate.Text!="")
                {

                    string sResult = Genaral.DateComparision(txtToDate.Text, txtFromDate.Text, false, false);
                    if (sResult == "2")
                    {
                        ShowMsgBox("To Date should be Greater than From Date");
                        return bValidate;
                    }                  
                }           
                if (txtTcCode.Text == "")
                {
                    ShowMsgBox("Please Enter the DTr Code");
                    txtTcCode.Focus();
                    return bValidate;
                }
      
                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                bValidate = false;
                return bValidate;
            }
        }
        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                LoadDTrTracker();
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadDTrTracker()
        {
            try
            {
                clsTcTracker objTcTracker = new clsTcTracker();

                if (ValidateForm() == true)
                {
                    objTcTracker.sTCCode = txtTcCode.Text;
                    objTcTracker.sFromDate = txtFromDate.Text;
                    objTcTracker.sToDate = txtToDate.Text;
                    if (cmbType.SelectedIndex > 0)
                    {
                        objTcTracker.sTaskType = cmbType.SelectedValue;
                    }

                    objTcTracker.GetTcTrackstatus(objTcTracker);

                    txtTcMake.Text = objTcTracker.sMake;
                    txtCapacity.Text = objTcTracker.sCapacity;
                    txtTcSlNo.Text = objTcTracker.sTCSlno;
                    txtRepairCount.Text = objTcTracker.sRepairCount;

                    grdTcDetails.DataSource = objTcTracker.dTracker;
                    grdTcDetails.DataBind();
                    ViewState["DTRTracker"] = objTcTracker.dTracker;
                }

                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdTcDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {

                grdTcDetails.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["DTRTracker"];
                grdTcDetails.DataSource = SortDataTable(dt as DataTable, true);
                grdTcDetails.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdTcDetails_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdTcDetails.PageIndex;
            DataTable dt = (DataTable)ViewState["DTRTracker"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdTcDetails.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdTcDetails.DataSource = dt;
            }
            grdTcDetails.DataBind();
            grdTcDetails.PageIndex = pageIndex;
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
                        ViewState["DTRTracker"] = dataView.ToTable();
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["DTRTracker"] = dataView.ToTable();
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

        protected void grdTcDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                // LOC_TYPE  1--> Store  2---> Field  3----> Repairer

                //DRT_ACT_REFTYPE   1---> Against Purchase Order  2---> After RI       3-->Failure             4--->Sent To Repairer 
                //                  5----> Recieve from Repairer  6----> Scarp Entry   7---> Scrap Disposal    8------> Inspection
                if (e.CommandName == "View")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();
                    string sUrl = string.Empty;
                    string sUrlPath = string.Empty;

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    Label lblRefType = (Label)row.FindControl("lblRefType");
                    Label lblRefNo = (Label)row.FindControl("lblRefNo");
                    Label lblDTrStatus = (Label)row.FindControl("lblDTrStatus");

                    switch (lblRefType.Text)
                    {
                        case "1":

                           string  sTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblRefNo.Text));

                           sUrlPath = "/MasterForms/TcMaster.aspx?TCId=" + sTCId;
                           sUrl = "window.open('" + sUrlPath + "','_blank');";
                           ClientScript.RegisterStartupScript(this.GetType(), "script", sUrl, true);
                           //Response.Redirect("/MasterForms/TcMaster.aspx?TCId=" + sTCId, false);
                           break;

                        case "2":

                           string sMappingId = lblRefNo.Text;

                           sMappingId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sMappingId));

                           sUrlPath = "/MasterForms/DtcMapping.aspx?MappingId=" + sMappingId;
                           sUrl = "window.open('" + sUrlPath + "','_blank');";
                           ClientScript.RegisterStartupScript(this.GetType(), "script", sUrl, true);

                           //Response.Redirect("/MasterForms/DTCCommision.aspx?QryDtcId=" + sDTCID, false);
                           break;

                        case "3":

                           clsFormValues objForm = new clsFormValues();
                           objForm.sFailureId = lblRefNo.Text;
                           string sDTCId = objForm.GetDTCId(objForm);
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

                           sUrl = "window.open('" + sUrlPath + "','_blank');";
                           ClientScript.RegisterStartupScript(this.GetType(), "script", sUrl, true);
                           //Response.Redirect("/DTCFailure/FailureEntry.aspx?FailureId=" + sFailureId + "&DTCId=" + sDTCId, false);
                           break;

                        case "4":

                           clsDTrRepairActivity objRepair = new clsDTrRepairActivity();
                           string sRepairMasterId = objRepair.GetRepairDetailsId(lblRefNo.Text);
                           sRepairMasterId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRepairMasterId));                           

                          sUrlPath = "/TCRepair/TCRepairIssue.aspx?TransId=" + sRepairMasterId;
                          sUrl = "window.open('" + sUrlPath + "','_blank');";
                          ClientScript.RegisterStartupScript(this.GetType(), "script", sUrl, true);
                          //Response.Redirect("/TCRepair/TCRepairIssue.aspx?TransId=" + sRepairMasterId, false);
                          break;

                        case "5":


                          string sRepairDetailsId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblRefNo.Text));

                          sUrlPath = "/TCRepair/DeliverTC.aspx?TransId=" + sRepairDetailsId;
                          sUrl = "window.open('" + sUrlPath + "','_blank');";
                          ClientScript.RegisterStartupScript(this.GetType(), "script", sUrl, true);
                          //Response.Redirect("/TCRepair/TCRepairIssue.aspx?TransId=" + sRepairMasterId, false);
                          break;

                        case "6":

                          string sScrapDetailsId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblRefNo.Text));

                          sUrlPath = "/ScrapEntry/ScrapEntry.aspx?ScrapDetailId=" + sScrapDetailsId;
                          sUrl = "window.open('" + sUrlPath + "','_blank');";
                          ClientScript.RegisterStartupScript(this.GetType(), "script", sUrl, true);
                          
                          break;

                        case "7":

                          string sScrapDetailsID = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblRefNo.Text));

                          sUrlPath = "/ScrapEntry/ScrapDisposal.aspx?ScrapDetailId=" + sScrapDetailsID;
                          sUrl = "window.open('" + sUrlPath + "', '_blank');";
                          ClientScript.RegisterStartupScript(this.GetType(), "script", sUrl, true);

                          break;

                        case "8":


                          string sInspectionId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblRefNo.Text));
                          string sDTrCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtTcCode.Text));

                          sUrlPath = "/TCRepair/TCTesting.aspx?TransId=" + sInspectionId + "&DTrCode=" + sDTrCode;
                          sUrl = "window.open('" + sUrlPath + "','_blank');";
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


        protected void lnkDTrDetails_Click(object sender, EventArgs e)
        {
            try
            {
                clsTcTracker objTraker = new clsTcTracker();
                string sTCId = objTraker.GetTCIdFromCode(txtTcCode.Text);

                if (sTCId != "")
                {
                    sTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTCId));
                    string url = "/MasterForms/TcMaster.aspx?TCId=" + sTCId;
                    //string s = "window.open('" + url + "', 'popup_window', 'width=300,height=100,left=100,top=100,resizable=yes');";
                    string s = "window.open('" + url + "', '_blank');";
                    ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                }
                else
                {
                    ShowMsgBox("Dtr Code not Exixt Please Add Dtr Code First");
                }
                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void Export_clickTCtracker(object sender, EventArgs e)
        {

            //clsTcTracker objTcTracker = new clsTcTracker();
            // if (ValidateForm() == true)
            // {
            //     objTcTracker.sTCCode = txtTcCode.Text;
            //     objTcTracker.sFromDate = txtFromDate.Text;
            //     objTcTracker.sToDate = txtToDate.Text;
            //     if (cmbType.SelectedIndex > 0)
            //     {
            //         objTcTracker.sTaskType = cmbType.SelectedValue;
            //     }

            //     objTcTracker.GetTcTrackstatus(objTcTracker);

            //     txtTcMake.Text = objTcTracker.sMake;
            //     txtCapacity.Text = objTcTracker.sCapacity;
            //     txtTcSlNo.Text = objTcTracker.sTCSlno;
            //     txtRepairCount.Text = objTcTracker.sRepairCount;

            //     DataTable dt = objTcTracker.dTracker;

            DataTable dt = (DataTable)ViewState["DTRTracker"];

            if (dt != null && dt.Rows.Count > 0)
            {

                dt.Columns["TRANSDATE"].ColumnName = "DATE";
                dt.Columns["LOCATION"].ColumnName = "LOCATION";
                dt.Columns["STATUS"].ColumnName = "STATUS";
                // dt.Columns["SI_FROM_STORE"].ColumnName = "FROM STORE";


                List<string> listtoRemove = new List<string> { "DRT_ACT_REFTYPE", "DRT_DTR_STATUS", "DRT_ACT_REFNO", "DRT_DTR_CODE" };
                string filename = "TCtrackerDetails" + DateTime.Now + ".xls";
                string pagetitle = "TC Tracker Details";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }

            else
            {
                ShowMsgBox("No record found");
            }
        }            
        
    }
}
    




