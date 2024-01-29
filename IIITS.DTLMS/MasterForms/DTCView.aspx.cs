using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;

namespace IIITS.DTLMS.MasterForms
{
    public partial class DTCView : System.Web.UI.Page
    {
        string strFormCode = "DTCView";
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
                if (!IsPostBack)
                {
                    CheckAccessRights("4");
                    LoadCombo();
                    if (Request.QueryString["URLRedirect"] != null && Request.QueryString["URLRedirect"].ToString() != "" && objSession.OfficeCode.Length < 5)
                    {
                        if (Request.QueryString["Locationcode"].ToString() != "" && Request.QueryString["Locationcode"] != null)
                        {

                            string locid = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["Locationcode"]));

                        }
                        Details.Style.Add("display", "block");
                        LoadHeirachy();
                    }

                    
                    LoadDtcDetails();                    
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        public void LoadCombo()
        {
            try
            {
                string sOfficeCode = string.Empty;
                if (objSession.OfficeCode.Length > 3)
                {
                    sOfficeCode = objSession.OfficeCode.Substring(0, 4);
                }
                Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone);
                string strQry = "SELECT DISTINCT \"FD_FEEDER_CODE\",\"FD_FEEDER_CODE\" || '-' || \"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\", \"TBLFEEDEROFFCODE\" WHERE  \"FD_FEEDER_ID\" = \"FDO_FEEDER_ID\" AND";
                strQry += " cast(\"FDO_OFFICE_CODE\" as text) LIKE '" + sOfficeCode + "%' ORDER BY \"FD_FEEDER_CODE\"";
                Genaral.Load_Combo(strQry, "--Select--", cmbFeeder);

                Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'PT' ORDER BY \"MD_ORDER_BY\" ", "--Select--", cmbProjectType);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        public void LoadDtcDetails()
        {

            try
            {
                clsDtcMaster ObjDtcMaster = new clsDtcMaster();
                if (Request.QueryString["Locationcode"] != null && Request.QueryString["Locationcode"].ToString() != "")
                {
                    ObjDtcMaster.sOfficeCode = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["Locationcode"]));
                }

                if (ObjDtcMaster.sOfficeCode == "" || ObjDtcMaster.sOfficeCode == null)
                {
                    ObjDtcMaster.sOfficeCode = objSession.OfficeCode;
                }

                if (cmbFeeder.SelectedIndex > 0)
                {
                    ObjDtcMaster.sFeederCode = cmbFeeder.SelectedValue;
                }
                if (cmbProjectType.SelectedIndex > 0)
                {
                    ObjDtcMaster.sProjectType = cmbProjectType.SelectedValue;
                }

                DataTable dtDtcDetails = ObjDtcMaster.LoadDtcGrid(ObjDtcMaster);
                if (dtDtcDetails.Rows.Count == 0)
                {

                    ShowEmptyGrid();
                    ViewState["DTC"] = dtDtcDetails;
                }
                else
                {
                    grdDtc.DataSource = dtDtcDetails;
                    grdDtc.DataBind();
                    ViewState["DTC"] = dtDtcDetails;
                }

                clsDashboard objDashboard = new clsDashboard();
                objDashboard.sOfficeCode = ObjDtcMaster.sOfficeCode;
                lblTotalDTC.Text = "Total Transformer Centre Count : " + objDashboard.GetTotalDTCCount(objDashboard);
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
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
                Response.Redirect("DTCCommision.aspx",false);

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
        protected void grdDtc_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdDtc.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["DTC"];
                grdDtc.DataSource = SortDataTable(dt as DataTable, true);
                grdDtc.DataBind();
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }

        protected void grdDtc_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdDtc.PageIndex;
            DataTable dt = (DataTable)ViewState["DTC"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdDtc.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdDtc.DataSource = dt;
            }
            grdDtc.DataBind();
            grdDtc.PageIndex = pageIndex;
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
                            ViewState["DTC"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["DTC"] = dataView.ToTable();
                        }

                        else{
                        ViewState["DTC"] = dataView.ToTable();
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
                            ViewState["DTC"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["DTC"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["DTC"] = dataView.ToTable();
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

        protected void grdDtc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "create")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                    string strDtcId = ((Label)row.FindControl("lblDtcId")).Text;
                     strDtcId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(strDtcId));
                    Response.Redirect("DTCCommision.aspx?QryDtcId=" + strDtcId + "", false);
                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDTCCode = (TextBox)row.FindControl("txtDTCCode");
                    TextBox txtDTCName = (TextBox)row.FindControl("txtDTCName");
                    TextBox txtTCCode = (TextBox)row.FindControl("txtTCCode");
                    TextBox txtFeederName = (TextBox)row.FindControl("txtFeederName"); 



                     DataTable dt = (DataTable)ViewState["DTC"];
                    dv = dt.DefaultView;

                    if (objSession.OfficeCode == "" || objSession.OfficeCode == null)
                    {
                        clsDtcMaster objDtcMaster = new clsDtcMaster();
                        objDtcMaster.sDtcCode = txtDTCCode.Text;
                        objDtcMaster.sDtcName = txtDTCName.Text;
                        objDtcMaster.sTcCode = txtTCCode.Text;
                       // objDtcMaster.sFeederCode = txtFeederName.Text;
                        objDtcMaster.sFeederName = txtFeederName.Text;

                        dt = objDtcMaster.LoadDtcGrid(objDtcMaster);
                        ViewState["DTC"] = dt;
                        grdDtc.DataSource = dt;                        
                        grdDtc.DataBind();
                    }
                    else
                    {
                        if (txtDTCCode.Text != "")
                        {
                            //sFilter = "DT_CODE Like '%" + txtDTCCode.Text.Replace("'", "'") + "%' AND";
                            sFilter = "DT_CODE Like '%" + txtDTCCode.Text.Trim().Replace("'", "") + "%' AND";
                        }
                        if (txtDTCName.Text != "")
                        {
                            //sFilter += " DT_NAME Like '%" + txtDTCName.Text.Replace("'", "'") + "%' AND";
                            sFilter += " DT_NAME Like '%" + txtDTCName.Text.Trim().Replace("'", "") + "%' AND";
                        }
                        if (txtTCCode.Text != "")
                        {
                            //sFilter += " TC_CODE Like '%" + txtTCCode.Text.Replace("'", "'") + "%' AND";
                            sFilter += " TC_CODE Like '%" + txtTCCode.Text.Trim().Replace("'", "") + "%' AND";
                        }
                        if (txtFeederName.Text != "")
                        {
                            //sFilter += " FEEDER_NAME Like '%" + txtFeederName.Text.Replace("'", "'") + "%' AND";
                            sFilter += " FEEDER_NAME Like '%" + txtFeederName.Text.Trim().Replace("'", "") + "%' AND";
                        }
                        if (sFilter.Length > 0)
                        {
                            sFilter = sFilter.Remove(sFilter.Length - 3);
                            grdDtc.PageIndex = 0;
                            dv.RowFilter = sFilter;
                            if (dv.Count > 0)
                            {
                                grdDtc.DataSource = dv;
                                ViewState["DTC"] = dv.ToTable();
                                grdDtc.DataBind();

                            }
                            else
                            {
                                ViewState["DTC"] = dv.ToTable();
                                ShowEmptyGrid();
                            }
                        }

                        else
                        {
                            LoadDtcDetails();
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

        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("DT_ID");
                dt.Columns.Add("DT_CODE");
                dt.Columns.Add("DT_NAME");
                dt.Columns.Add("TC_CODE");
                dt.Columns.Add("DT_TOTAL_CON_KW");
                dt.Columns.Add("DT_TOTAL_CON_HP");
                dt.Columns.Add("TC_CAPACITY");
                dt.Columns.Add("DT_LAST_SERVICE_DATE");
                dt.Columns.Add("FEEDER_NAME");
                dt.Columns.Add("DT_PROJECTTYPE");

                grdDtc.DataSource = dt;
                grdDtc.DataBind();

                int iColCount = grdDtc.Rows[0].Cells.Count;
                grdDtc.Rows[0].Cells.Clear();
                grdDtc.Rows[0].Cells.Add(new TableCell());
                grdDtc.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdDtc.Rows[0].Cells[0].Text = "No Records Found";

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

                objApproval.sFormName = "DTCCommision";
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

        protected void cmbFeeder_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //cmdLoad_Click()
                LoadDtcDetails();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbProjectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadDtcDetails();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void Export_clickDTCMaster(object sender, EventArgs e)
        {

            //clsDtcMaster ObjDtcMaster = new clsDtcMaster();

            //ObjDtcMaster.sOfficeCode = objSession.OfficeCode;

            //if (cmbFeeder.SelectedIndex > 0)
            //{
            //    ObjDtcMaster.sFeederCode = cmbFeeder.SelectedValue;
            //}
            //if (cmbProjectType.SelectedIndex > 0)
            //{
            //    ObjDtcMaster.sProjectType = cmbProjectType.SelectedValue;
            //}

            //DataTable dtDtcDetails = ObjDtcMaster.LoadDtcGrid(ObjDtcMaster);

            DataTable dtDtcDetails = (DataTable)ViewState["DTC"];
            if (dtDtcDetails.Rows.Count > 0)
            {

                dtDtcDetails.Columns["FEEDER_NAME"].ColumnName = "FEEDER NAME";
                dtDtcDetails.Columns["DT_CODE"].ColumnName = "TRANSFORMER CENTRE CODE";
                dtDtcDetails.Columns["DT_NAME"].ColumnName = "TRANSFORMER CENTRE NAME";
                dtDtcDetails.Columns["TC_CODE"].ColumnName = "TRANSFORMER CODE(SS PLATE NO)";
                dtDtcDetails.Columns["TC_CAPACITY"].ColumnName = "DTR CAPACITY(IN KVA)";

                dtDtcDetails.Columns["FEEDER NAME"].SetOrdinal(0);

                List<string> listtoRemove = new List<string> { "DT_ID", "DT_TOTAL_CON_KW", "DT_TOTAL_CON_HP", "DT_LAST_SERVICE_DATE", "DT_PROJECTTYPE" };
                string filename = "DTCDetails" + DateTime.Now + ".xls";
                string pagetitle = "Transformer Centre Details";

                Genaral.getexcel(dtDtcDetails, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");
                ShowEmptyGrid();
            }



        }

        protected void cmbZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbZone.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE CAST(\"CM_ZO_ID\" AS TEXT) = '" + cmbZone.SelectedValue + "'", "--Select--", cmbCircle);                 
                    cmbDiv.Items.Clear();
                }
                else
                {
                    cmbCircle.Items.Clear();
                    cmbDiv.Items.Clear();
                    cmbSubDiv.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE \"DIV_CICLE_CODE\"='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDiv);
                    cmbSubDiv.Items.Clear();
                }
                else
                {
                    cmbDiv.Items.Clear();
                    cmbSubDiv.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDiv.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE \"SD_DIV_CODE\" ='" + cmbDiv.SelectedValue + "'", "--Select--", cmbSubDiv);                    
                }                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbSubDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubDiv.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"ST_ID\",\"ST_NAME\" FROM \"TBLSTATION\" WHERE \"ST_OFF_CODE\"='"+ cmbSubDiv.SelectedValue + "'", "--Select--", cmbStation);
                    Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_SUBDIV_CODE\"='" + cmbSubDiv.SelectedValue + "'", "--Select--", cmbOMSection);                 
                }                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void LoadHeirachy()

        {
            clsDtcMaster ObjDtcMaster = new clsDtcMaster();
            if (Request.QueryString["Locationcode"] != null && Request.QueryString["Locationcode"].ToString() != "")
            {
                ObjDtcMaster.sOfficeCode = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["Locationcode"]));
            }
            else
            {
                ObjDtcMaster.sOfficeCode = objSession.OfficeCode;
            }

            if (ObjDtcMaster.sOfficeCode.Length > 0)
            {
                cmbZone.SelectedValue = ObjDtcMaster.sOfficeCode.Substring(0, 1);
                cmbZone_SelectedIndexChanged(this, EventArgs.Empty);
                cmbZone.Enabled = false;
            }

            if (ObjDtcMaster.sOfficeCode.Length > 1)
            {
                cmbCircle.SelectedValue = ObjDtcMaster.sOfficeCode.Substring(0, 2);
                cmbCircle_SelectedIndexChanged(this, EventArgs.Empty);
                cmbCircle.Enabled = false;
            }

            if (ObjDtcMaster.sOfficeCode.Length > 2)
            {
                cmbDiv.SelectedValue = ObjDtcMaster.sOfficeCode.Substring(0, 3);
                cmbDiv_SelectedIndexChanged(this, EventArgs.Empty);
                cmbDiv.Enabled = false;
            }

            if (ObjDtcMaster.sOfficeCode.Length > 3)
            {
                cmbSubDiv.SelectedValue = ObjDtcMaster.sOfficeCode.Substring(0, 4);
                cmbSubDiv_SelectedIndexChanged(this, EventArgs.Empty);
                cmbSubDiv.Enabled = false;
            }
            if (ObjDtcMaster.sOfficeCode.Length > 4)
            {
                cmbOMSection.SelectedValue = ObjDtcMaster.sOfficeCode;
                cmbOMSection.Enabled = false;
            }
        }

        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                clsDtcMaster ObjDtcMaster = new clsDtcMaster();

                if (cmbOMSection.SelectedIndex > 0)
                {
                    ObjDtcMaster.sOfficeCode = cmbOMSection.SelectedValue;
                }
                else if (cmbSubDiv.SelectedIndex > 0)
                {
                    ObjDtcMaster.sOfficeCode = cmbSubDiv.SelectedValue;
                    string strQry = "SELECT \"FD_FEEDER_CODE\",\"FD_FEEDER_CODE\" || '-' || \"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\", \"TBLFEEDEROFFCODE\" WHERE  \"FD_FEEDER_ID\" = \"FDO_FEEDER_ID\" AND";
                    strQry += " cast(\"FDO_OFFICE_CODE\" as text) LIKE '" + ObjDtcMaster.sOfficeCode + "%' ORDER BY \"FD_FEEDER_CODE\"";
                    Genaral.Load_Combo(strQry, "--Select--", cmbFeeder);
                }
                else if (cmbDiv.SelectedIndex > 0)
                {
                    ObjDtcMaster.sOfficeCode = cmbDiv.SelectedValue;
                }
                else if (cmbCircle.SelectedIndex > 0)
                {
                    ObjDtcMaster.sOfficeCode = cmbCircle.SelectedValue;
                }
                else if (cmbZone.SelectedIndex > 0)
                {
                    ObjDtcMaster.sOfficeCode = cmbZone.SelectedValue;
                }                                                                                            
                else
                {
                    ObjDtcMaster.sOfficeCode = "";
                }
                if(cmbStation.SelectedIndex > 0)
                {
                    ObjDtcMaster.sStation = cmbStation.SelectedValue;
                }                
                if (cmbFeeder.SelectedIndex > 0)
                {
                    ObjDtcMaster.sFeederCode = cmbFeeder.SelectedValue;
                }
                if (cmbProjectType.SelectedIndex > 0)
                {
                    ObjDtcMaster.sProjectType = cmbProjectType.SelectedValue;
                }

                DataTable dtDtcDetails = ObjDtcMaster.LoadDtcGrid(ObjDtcMaster);
                if (dtDtcDetails.Rows.Count == 0)
                {

                    ShowEmptyGrid();
                    ViewState["DTC"] = dtDtcDetails;
                }
                else
                {
                    grdDtc.DataSource = dtDtcDetails;
                    grdDtc.DataBind();
                    ViewState["DTC"] = dtDtcDetails;
                }

                lblTotalDTC.Text = "Total Transformer Centre Count : " + Convert.ToString(dtDtcDetails.Rows.Count);
            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            
        }
    }
}