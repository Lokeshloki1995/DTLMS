using IIITS.DTLMS.BL;
using IIITS.DTLMS.BL.MasterForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.MinorRepair
{
    public partial class RepairerRatesView : System.Web.UI.Page
    {
        string strFormCode = "RepairerRatesView";
        clsSession objSession;
        DataTable dtDetails = new DataTable();

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

                if (!IsPostBack)
                {

                    CheckAccessRights("4");

                    txtEffectFrom.Attributes.Add("readonly", "readonly");
                    txtEffectTo.Attributes.Add("readonly", "readonly");
                    txtPoDate.Attributes.Add("readonly", "readonly");


                    Genaral.Load_Combo("SELECT \"DIV_ID\",\"DIV_NAME\" FROM \"TBLDIVISION\"   ORDER BY \"DIV_ID\"", "--Select--", cmbDivision);
                    // Genaral.Load_Combo("SELECT \"TR_ID\",\"TR_NAME\" FROM \"TBLTRANSREPAIRER\" ORDER BY \"TR_ID\" ", "--Select--", cmdRepairer);
                    Genaral.Load_Combo("SELECT distinct \"MD_ID\",\"MD_NAME\" FROM \"TBLREPAIRERRATES\" INNER JOIN \"TBLMASTERDATA\" ON \"MD_ID\"=\"RR_CAP_ID\"  AND \"MD_TYPE\"='C' ORDER BY \"MD_ID\" ", "--Select--", cmdCapacity);

                    Genaral.Load_Combo("SELECT \"DIV_ID\",\"DIV_NAME\" FROM \"TBLDIVISION\"   ORDER BY \"DIV_ID\"", "--Select--", cmbDivision1);
                    Genaral.Load_Combo("SELECT \"TR_ID\",\"TR_NAME\" FROM \"TBLTRANSREPAIRER\" ORDER BY \"TR_ID\" ", "--Select--", cmdRepairer1);
                    Genaral.Load_Combo("SELECT distinct \"MD_ID\",\"MD_NAME\" FROM \"TBLREPAIRERRATES\" INNER JOIN \"TBLMASTERDATA\" ON \"MD_ID\"=\"RR_CAP_ID\"  AND \"MD_TYPE\"='C' ORDER BY \"MD_ID\" ", "--Select--", cmdCapacity1);

                    if (Request.QueryString["RepairerId"] != null && Request.QueryString["RepairerId"].ToString() != "")
                    {                                             
                        cmbDivision.SelectedValue = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["Offcode"]));
                        cmdDivChangeClick(sender, e);
                        cmdRepairer.SelectedValue = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["RepairerId"]));

                        cmdLoad_Click(sender, e);
                        grdRatesView.Columns[16].Visible = false;
                        cmdReset.Enabled = false;
                        cmbDivision.Enabled = false;
                        cmdRepairer.Enabled = false;
                        cmdNew.Visible = false;
                    }
                    else
                    {
                        LoadRepairerRate();
                        grdRatesView.Columns[16].Visible = true;
                        cmdReset.Enabled = true;
                        cmbDivision.Enabled = true;
                        cmdRepairer.Enabled = true;
                        cmdNew.Visible = true;
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }
        private void LoadRepairerRate()
        {
            try
            {
                clsRepairerRates obj = new clsRepairerRates();
                dtDetails = obj.GetRepairerRateDetails(obj);
                grdRatesView.DataSource = dtDetails;
                grdRatesView.DataBind();
                ViewState["RatesView"] = dtDetails;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdRatesView_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }


        protected void grdRatesView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdRatesView.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["RatesView"];

                grdRatesView.DataSource = SortDataTable(dt as DataTable, true);
                grdRatesView.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdRatesView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                clsRepairerRates obj = new clsRepairerRates();
                string[] strResult = new string[2];
                if (e.CommandName == "EditClick")
                {

                    cmdCapacity1.ClearSelection();
                    cmdRepairer1.ClearSelection();
                    cmdStarrate1.ClearSelection();
                    cmbDivision1.ClearSelection();
                    txtEffectFrom.Text = "";
                    txtEffectTo.Text = "";
                    txtPoDate.Text = "";
                    txtPoNo.Text = "";
                    txtCost.Text = "";

                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    string strRecordId = ((Label)row.FindControl("lblid")).Text;
                    hdnID.Value = strRecordId;
                    cmdCapacity1.SelectedValue = ((Label)row.FindControl("lblCAP")).Text;
                    cmdRepairer1.SelectedValue = ((Label)row.FindControl("lbltrID")).Text;
                    cmdStarrate1.SelectedValue = ((Label)row.FindControl("lblstarID")).Text;
                    cmbDivision1.SelectedValue = ((Label)row.FindControl("lbldivID")).Text;
                    string from = ((Label)row.FindControl("lblFrom")).Text;
                    string to = ((Label)row.FindControl("lblTo")).Text;
                    string podate = ((Label)row.FindControl("lblPoDate")).Text;
                    txtPoNo.Text = ((Label)row.FindControl("lblPono")).Text;
                    txtCost.Text = ((Label)row.FindControl("lblamount")).Text;

                    if (from != "")
                    {

                        DateTime fromdate = Convert.ToDateTime(from);
                        txtEffectFrom.Text = Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy");

                    }
                    if (to != "")
                    {
                        DateTime fromdate = Convert.ToDateTime(to);
                        txtEffectTo.Text = Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy");
                    }
                    if (podate != "")
                    {
                        DateTime fromdate = Convert.ToDateTime(podate);
                        txtPoDate.Text = Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy");
                    }

                    cmdSubmit.Text = "UPDATE";

                    this.mdlPopup.Show();
                }
                if (e.CommandName == "DeleteClick")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    string strRecordId = ((Label)row.FindControl("lblid")).Text;
                    strResult = obj.DeleteRepairerRates(strRecordId);
                    ShowMsgBox(strResult[1]);
                    LoadRepairerRate();
                }
                if (e.CommandName == "DownloadClick")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    hdfdivid.Value = ((Label)row.FindControl("lbldivID")).Text;
                    hdfrepid.Value = ((Label)row.FindControl("lbltrID")).Text;

                    clsDTrRepairActivity objTcRepair = new clsDTrRepairActivity();
                    string vl = objTcRepair.getdivcode(hdfdivid.Value);
                        clsTransRepairer objRepairer = new clsTransRepairer();
                    string fileName = objRepairer.GetRepairfilepath(hdfrepid.Value, vl);
                    if (fileName == "")
                    {
                        ShowMsgBox("File Not Found");
                        return;
                    }
                    else
                    {
                        DownloadFiledwnld(sender, e);
                    }
                    //strResult = obj.DeleteRepairerRates(strRecordId);
                    //ShowMsgBox(strResult[1]);
                    //LoadRepairerRate();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void DownloadFiledwnld(object sender, EventArgs e)
        {
          //  string fileName1 = (sender as ImageButton).CommandArgument;
            clsTransRepairer objRepairer = new clsTransRepairer();
            try
            {
                Stream stream = null;
                int bytesToRead = 10000;
                byte[] buffer = new Byte[bytesToRead];
                try
                {
                    string SFTPmainfolder = Convert.ToString(ConfigurationSettings.AppSettings["VirtualDirectoryDocs"]);

                //    string PoNo = Regex.Replace(txtPONo.Text, @"[^0-9a-zA-Z]+", "");
                    clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);
                    clsDTrRepairActivity objTcRepair = new clsDTrRepairActivity();
                    string divcd = objTcRepair.getdivcode(hdfdivid.Value);
                    //  string fileName = getFilename(url);
                    string fileName = objRepairer.GetRepairfilepath(hdfrepid.Value, divcd);
                    string url = SFTPmainfolder + fileName;
                    HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(url);
                    HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();

                    if (fileReq.ContentLength > 0)
                        fileResp.ContentLength = fileReq.ContentLength;
                    stream = fileResp.GetResponseStream();
                    var resp = HttpContext.Current.Response;
                    resp.ContentType = "application/octet-stream";
                    resp.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
                    resp.AddHeader("Content-Length", fileResp.ContentLength.ToString());
                    int length;
                    do
                    {
                        if (resp.IsClientConnected)
                        {
                            length = stream.Read(buffer, 0, bytesToRead);
                            resp.OutputStream.Write(buffer, 0, length);
                            resp.Flush();
                            buffer = new Byte[bytesToRead];
                        }
                        else
                        {
                            length = -1;
                        }
                    } while (length > 0);
                }
                finally
                {
                    if (stream != null)
                    {
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

        protected void cmdDivChangeClick(object sender, EventArgs e)
        {
            if (cmbDivision.SelectedIndex > 0)
            {
                string str = " SELECT  DISTINCT \"TR_ID\",\"TR_NAME\" FROM \"TBLTRANSREPAIRER\" INNER JOIN \"TBLTRANSREPAIREROFFCODE\"  ON \"TR_ID\"=\"TRO_TR_ID\"   INNER JOIN \"TBLDIVISION\" ON \"TRO_OFF_CODE\"=\"DIV_CODE\" INNER JOIN \"TBLREPAIRERRATES\" ON \"RR_DIV_ID\"=\"DIV_ID\" WHERE \"DIV_ID\"= '" + cmbDivision.SelectedValue + "'  ORDER BY \"TR_ID\" ";
                Genaral.Load_Combo(str, "--Select--", cmdRepairer);
            }
        }
        protected void cmdDiv1ChangeClick(object sender, EventArgs e)
        {
            if (cmbDivision1.SelectedIndex > 0)
            {
                string str = " SELECT  DISTINCT \"TR_ID\",\"TR_NAME\" FROM \"TBLTRANSREPAIRER\" INNER JOIN \"TBLTRANSREPAIREROFFCODE\"  ON \"TR_ID\"=\"TRO_TR_ID\"   INNER JOIN \"TBLDIVISION\" ON \"TRO_OFF_CODE\"=\"DIV_CODE\" INNER JOIN \"TBLREPAIRERRATES\" ON \"RR_DIV_ID\"=\"DIV_ID\" WHERE \"DIV_ID\"= '" + cmbDivision1.SelectedValue + "'  ORDER BY \"TR_ID\" ";
                Genaral.Load_Combo(str, "--Select--", cmdRepairer1);
            }
            this.mdlPopup.Show();
        }

        protected void cmdRepairer1ChangeClick(object sender, EventArgs e)
        {
            clsRepairerRates obj = new clsRepairerRates();
            string div_id = cmbDivision1.SelectedValue;
            string rep_id = cmdRepairer1.SelectedValue;
            string rep_name = cmdRepairer1.SelectedItem.Text;

            DataTable dt = obj.GetRepairRatesDetails(rep_id);
            txtPoNo.Text = Convert.ToString(dt.Rows[0]["TR_DWA_NO"]);
            txtPoDate.Text = Convert.ToString(dt.Rows[0]["TR_DWA_DATE"]);
            txtEffectFrom.Text = Convert.ToString(dt.Rows[0]["TR_CON_STR_DATE"]);
            txtEffectTo.Text = Convert.ToString(dt.Rows[0]["TR_CON_END_DATE"]);

            this.mdlPopup.Show();
        }
        protected void cmdSubmit_Click(object sender, EventArgs e)
        {
            clsRepairerRates obj = new clsRepairerRates();
            string[] strResult = new string[2];

            if (ValidateForm())
            {
                if (cmbDivision1.SelectedValue != "--Select--")
                {
                    obj.div_id = cmbDivision1.SelectedValue;

                }
                if (cmdRepairer1.SelectedValue != "--Select--")
                {
                    obj.rep_id = cmdRepairer1.SelectedValue;
                    obj.rep_name = cmdRepairer1.SelectedItem.Text;
                }
                if (cmdCapacity1.SelectedValue != "--Select--")
                {
                    obj.capacity_id = cmdCapacity1.SelectedValue;
                    obj.capacity = cmdCapacity1.SelectedItem.Text;
                }
                if (cmdStarrate1.SelectedValue != "--Select--")
                {
                    obj.StarRate_id = cmdStarrate1.SelectedValue;
                    obj.StarRate = cmdStarrate1.SelectedItem.Text;
                }
                if (txtEffectFrom.Text != "")
                {
                    obj.EffectiveFrom = txtEffectFrom.Text;
                }
                if (txtEffectTo.Text != "")
                {
                    obj.EffectiveTo = txtEffectTo.Text;
                }
                if (txtPoDate.Text != "")
                {
                    obj.PoDate = txtPoDate.Text;
                }
                if (txtPoNo.Text != "")
                {
                    obj.Po_No = txtPoNo.Text;
                }

                obj.Cost = Convert.ToDouble(txtCost.Text);

                obj.pkid = hdnID.Value;

                obj.sCrby = objSession.UserId;

                strResult = obj.SaveRepairerRates(obj);

                ShowMsgBox(strResult[1]);
                LoadRepairerRate();
            }
        }
        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("sDIV_NAME");
                dt.Columns.Add("sVENDOR_TYPE");
                dt.Columns.Add("sVENDOR_NAME");
                dt.Columns.Add("sMD_NAME");
                dt.Columns.Add("sFROM");
                dt.Columns.Add("sTO");
                dt.Columns.Add("srr_rep_id");
                dt.Columns.Add("srr_cap_id");
                dt.Columns.Add("srr_id");
                dt.Columns.Add("srr_amount");
                dt.Columns.Add("spo_no");
                dt.Columns.Add("spo_date");
                dt.Columns.Add("sdiv_id");
                dt.Columns.Add("sstar_id");

                grdRatesView.DataSource = dt;
                grdRatesView.DataBind();

                int iColCount = grdRatesView.Rows[0].Cells.Count;
                grdRatesView.Rows[0].Cells.Clear();
                grdRatesView.Rows[0].Cells.Add(new TableCell());
                grdRatesView.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdRatesView.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdRatesView_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                GridViewSortExpression = e.SortExpression;
                int pageIndex = grdRatesView.PageIndex;
                DataTable dt = (DataTable)ViewState["RatesView"];
                string sortingDirection = string.Empty;

                if (dt.Rows.Count > 0)
                {
                    grdRatesView.DataSource = SortDataTable(dt as DataTable, false);
                }
                else
                {
                    grdRatesView.DataSource = dt;
                }
                grdRatesView.DataBind();
                grdRatesView.PageIndex = pageIndex;
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
                        ViewState["RatesView"] = dataView.ToTable();
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["RatesView"] = dataView.ToTable();
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
        protected void cmdLoad_Click(object sender, EventArgs e)
        {

            clsRepairerRates obj = new clsRepairerRates();

            if (cmbDivision.SelectedValue != "--Select--")
            {
                obj.div_id = cmbDivision.SelectedValue;
            }
            if (cmdRepairer.SelectedValue != "--Select--")
            {
                obj.rep_id = cmdRepairer.SelectedValue;
            }
            if (cmdCapacity.SelectedValue != "--Select--")
            {
                obj.capacity = cmdCapacity.SelectedValue;
            }
            if (cmdStarrate.SelectedValue != "--Select--")
            {
                obj.StarRate = cmdStarrate.SelectedValue;
            }

            dtDetails = obj.GetRepairerRateDetails(obj);
            grdRatesView.DataSource = dtDetails;
            grdRatesView.DataBind();





            ViewState["RatesView"] = dtDetails;
        }
        protected void cmdReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("RepairerRatesView.aspx", false);
        }

        protected void cmdNew_Click(object sender, EventArgs e)
        {

            cmdCapacity1.ClearSelection();
            cmdRepairer1.ClearSelection();
            cmdStarrate1.ClearSelection();
            cmbDivision1.ClearSelection();
            txtEffectFrom.Text = "";
            txtEffectTo.Text = "";
            txtPoDate.Text = "";
            txtPoNo.Text = "";
            txtCost.Text = "";
            cmdSubmit.Text = "Submit";
            this.mdlPopup.Show();
        }
        protected bool ValidateForm()
        {

            if (cmbDivision1.SelectedValue == "--Select--")
            {
                ShowMsgBox("Please Select Division");
                cmbDivision1.Focus();
                return false;
            }
            if (cmdRepairer1.SelectedValue == "--Select--")
            {
                ShowMsgBox("Please Select Repairer");
                cmdRepairer1.Focus();
                return false;

            }
            if (cmdCapacity1.SelectedValue == "--Select--")
            {
                ShowMsgBox("Please Select Capacity");
                cmdCapacity1.Focus();
                return false;
            }
            if (cmdStarrate1.SelectedValue == "--Select--")
            {
                ShowMsgBox("Please Select Star Rate");
                cmdStarrate1.Focus();
                return false;
            }
            if (txtEffectFrom.Text == "")
            {
                ShowMsgBox("Please Select Effective From Date");
                txtEffectFrom.Focus();
                return false;
            }
            if (txtEffectTo.Text == "")
            {
                ShowMsgBox("Please Select Effective To Date");
                txtEffectTo.Focus();
                return false;
            }
            string sResult = Genaral.DateComparision(txtEffectTo.Text, txtEffectFrom.Text, false, false);

            if (sResult == "2")
            {
                ShowMsgBox("Effective To Date should be Greater than or equal to Effective From Date");
                return false;
            }


            return true;
        }
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "RepairerRates";
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

    }
}