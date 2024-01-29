using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;


namespace IIITS.DTLMS.Maintainance
{
    public partial class DTCMaintance : System.Web.UI.Page
    {
        string strFormCode = "DTCMaintance";
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
                    txtMaintainanceDate.Attributes.Add("readonly", "readonly");

                    txtMaintainanceDate_CalendarExtender1.EndDate = System.DateTime.Now;

                    if (!IsPostBack)
                    {
                        LoadSearchWindow();

                        //Redirected from  Maintainance Details
                        if (Request.QueryString["MaintId"] != null && Convert.ToString(Request.QueryString["MaintId"]) != "")
                        {
                            txtMaintainanceId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["MaintId"]));
                            GetTcMaintainanceDeatils();
                            cmbType_SelectedIndexChanged(sender, e);
                            cmdReset.Visible = false;
                            txtDTCCode.ReadOnly = true;
                            cmdSearch_Click(sender, e);

                            txtTCCode.Text = hdfTCCode.Value;
                            LoadDtcMaintance();
                            cmbType.Enabled = false;
                        }

                        if (Request.QueryString["MaintType"] != null && Convert.ToString(Request.QueryString["MaintType"]) != "")
                        {
                            string strMainType = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["MaintType"]));
                            txtDTCCode.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["PendingMaintId"]));
                            txtDTCCode.ReadOnly = true;
                            if (strMainType == "QUARTERLY")
                            {
                                cmbType.SelectedValue = "1";
                                cmbType_SelectedIndexChanged(sender, e);

                            }
                            if (strMainType == "HALFYEARLY")
                            {
                                cmbType.SelectedValue = "2";
                                cmbType_SelectedIndexChanged(sender, e);

                            }
                            cmbType.Enabled = false;
                            cmdSearch_Click(sender, e);
                            cmdReset.Visible = false;


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

        public void LoadSearchWindow()
        {
            try
            {
                Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'MT' ORDER BY \"MD_ORDER_BY\"", "--Select--", cmbType);
                Genaral.Load_Combo("SELECT \"US_ID\",\"US_FULL_NAME\" FROM \"TBLUSER\" WHERE COALESCE(\"US_EFFECT_FROM\",CURRENT_DATE-1)<=NOW() AND \"US_STATUS\"='A' and \"US_OFFICE_CODE\" LIKE '" + objSession.OfficeCode + "%' ORDER BY \"US_ID\"", "--Select--", cmbMaintainedBy);

                string strQry = string.Empty;
                strQry = "Title=Search and Select Transformer Centre Details&";
                strQry += "Query=select \"DT_CODE\",\"DT_NAME\"  FROM \"TBLDTCMAST\" WHERE CURRENT_DATE > \"DT_LAST_SERVICE_DATE\" AND cast(\"DT_OM_SLNO\" as text)  LIKE '" + objSession.OfficeCode + "%'  AND CAST({0} AS TEXT) like %{1}% order by \"DT_CODE\"&";
                strQry += "DBColName=\"DT_NAME\"~\"DT_CODE\"&";
                strQry += "ColDisplayName=Transformer Centre Name~Transformer Centre Code&";

                //string strQry = string.Empty;
                //strQry = "Title=Search and Select DTC Details&";
                //strQry += "Query=  SELECT DT_CODE , DT_NAME FROM";
                //strQry += "(SELECT DT_CODE, DT_NAME, DT_TC_ID, DT_OM_SLNO, NVL(DT_LAST_SERVICE_DATE,SYSDATE-91) AS  DT_LAST_SERVICE_DATE FROM TBLDTCMAST) A,";
                //strQry += "(SELECT TM_DT_CODE, MAX(TM_DATE) LAST_SERVICE_DATE, MIN(TM_MAINTAIN_TYPE) KEEP (DENSE_RANK LAST ORDER BY TM_DATE) TM_MAINTAIN_TYPE ";
                //strQry += " FROM TBLDTCMAINTENANCE GROUP BY TM_DT_CODE)B WHERE A.DT_CODE = B.TM_DT_CODE(+) AND DT_OM_SLNO LIKE '" + objSession.OfficeCode + "%' AND";
                //strQry += " (TM_MAINTAIN_TYPE IN (1,2) OR TM_MAINTAIN_TYPE IS NULL) AND ";
                //strQry += " (SYSDATE - DT_LAST_SERVICE_DATE > 180 OR SYSDATE - DT_LAST_SERVICE_DATE > 90)  AND {0} like %{1}% order by DT_CODE&";
                //strQry += " DBColName=DT_NAME~DT_CODE&";
                //strQry += " ColDisplayName=DTC Name~DTC Code&";


                strQry = strQry.Replace("'", @"\'");
                cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtDTCCode.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + txtDTCCode.ClientID + ")");

                txtMaintainanceDate.Attributes.Add("onblur", "return ValidateDate(" + txtMaintainanceDate.ClientID + ");");

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


        public void GetTcMaintainanceDeatils()
        {
            try
            {
                clsTcMaintainance objMaintainance = new clsTcMaintainance();
                objMaintainance.sMaintainanceId = txtMaintainanceId.Text;
                objMaintainance.sDTCCode = txtDTCCode.Text;
                objMaintainance.GetMaintainaceDetails(objMaintainance);

                hdfTCCode.Value = objMaintainance.sTcCode;
                txtDTCCode.Text = objMaintainance.sDTCCode;
                txtMaintainanceDate.Text = objMaintainance.sTmDate;
                cmbMaintainedBy.SelectedValue = objMaintainance.sMaintainBy;
                cmbType.SelectedValue = objMaintainance.sMaintainType;

                txtConnections.Text = objMaintainance.sConnections;
                txtFuses.Text = objMaintainance.sFuses;
                txtOil.Text = objMaintainance.sOilLeakage;
                txtBushing.Text = objMaintainance.sBushing;
                txtBreather.Text = objMaintainance.sBreather;
                txtDangerPlates.Text = objMaintainance.sDangerPlate;
                txtAntiClimbing.Text = objMaintainance.sAntiClimbing;
                txtLightningArrestor.Text = objMaintainance.sArrestor;
                txtGoSwitches.Text = objMaintainance.sGOSwitches;
                txtLoadBalancing.Text = objMaintainance.sLoadBalancing;
                txtSupports.Text = objMaintainance.sSupports;
                txtExplosion.Text = objMaintainance.sExplosion;
                txtArcingHorns.Text = objMaintainance.sArcing;
                txtEarthing.Text = objMaintainance.sEarthing;
                txtGeneralCondition.Text = objMaintainance.sConditionNuts;
                txtLTSwitch.Text = objMaintainance.sLTWsitch;
                txtVoltage.Text = objMaintainance.sVoltage;
                txtEarthTesting.Text = objMaintainance.sEarthTesting;

                txtMaintainDeId.Text = objMaintainance.sMaintainanceDetailsId;


                if (Convert.ToDateTime(objMaintainance.sCrOn).ToString("dd/MM/yyyy") != System.DateTime.Today.ToString("dd/MM/yyyy"))
                {
                    cmdSave.Enabled = false;
                }

                cmdSave.Text = "Update";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (cmbType.SelectedIndex > 0)
                {
                    dvType.Style.Add("display", "block");
                }
                else
                {
                    dvType.Style.Add("display", "none");
                }

                if (cmbType.SelectedValue == "1")
                {
                    dvEarthing.Style.Add("display", "block");
                    dvSupport.Style.Add("display", "block");
                    dvExplosion.Style.Add("display", "block");
                    dvArcing.Style.Add("display", "block");
                    dvEarthTesting.Style.Add("display", "none");
                    dvVoltage.Style.Add("display", "none");
                }
                else if (cmbType.SelectedValue == "2")
                {
                    dvEarthTesting.Style.Add("display", "block");
                    dvVoltage.Style.Add("display", "block");
                    dvEarthing.Style.Add("display", "none");
                    dvSupport.Style.Add("display", "none");
                    dvExplosion.Style.Add("display", "none");
                    dvArcing.Style.Add("display", "none");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                clsTcMaintainance objTcMaintainance = new clsTcMaintainance();

                objTcMaintainance.sDTCCode = txtDTCCode.Text;


                objTcMaintainance.GetDtcBasicDetails(objTcMaintainance,objSession.OfficeCode);
                
                txtDTCCode.Text = objTcMaintainance.sDTCCode;
                txtDTCName.Text = objTcMaintainance.sDTCName;
                txtTCCode.Text = objTcMaintainance.sTcCode;

                LoadDtcMaintance();
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
                if (txtTCCode.Text.Trim() == "")
                {
                    ShowMsgBox("Enter valid DTr Code");
                    txtTCCode.Focus();
                    return bValidate;
                }

                if (txtConnections.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Connections");
                    txtConnections.Focus();
                    return bValidate;
                }
                if (txtFuses.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Fuses");
                    txtFuses.Focus();
                    return bValidate;
                }
                if (txtOil.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Oil Leakage");
                    txtOil.Focus();
                    return bValidate;
                }
                if (txtBushing.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Bushing");
                    txtBushing.Focus();
                    return bValidate;
                }

                if (txtBreather.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Breather");
                    txtBreather.Focus();
                    return bValidate;
                }
                if (txtDangerPlates.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Danger Plates");
                    txtDangerPlates.Focus();
                    return bValidate;
                }
                if (txtAntiClimbing.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Anti Climbing Devices");
                    txtAntiClimbing.Focus();
                    return bValidate;
                }
                if (txtLightningArrestor.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Lightening Arrestor");
                    txtLightningArrestor.Focus();
                    return bValidate;
                }
                if (txtGoSwitches.Text.Trim() == "")
                {
                    ShowMsgBox("Enter G.O Switches");
                    txtGoSwitches.Focus();
                    return bValidate;
                }
                if (txtLoadBalancing.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Load Balancing");
                    txtLoadBalancing.Focus();
                    return bValidate;
                }
                if (cmbType.SelectedValue == "1")
                {

                    if (txtSupports.Text.Trim() == "")
                    {
                        ShowMsgBox("Enter Supports");
                        txtSupports.Focus();
                        return bValidate;
                    }
                    if (txtExplosion.Text.Trim() == "")
                    {
                        ShowMsgBox("Enter Explosion Vent");
                        txtExplosion.Focus();
                        return bValidate;
                    }
                    if (txtArcingHorns.Text.Trim() == "")
                    {
                        ShowMsgBox("Enter Arcing horns");
                        txtArcingHorns.Focus();
                        return bValidate;
                    }
                    if (txtEarthing.Text.Trim() == "")
                    {
                        ShowMsgBox("Enter Earthing");
                        txtEarthing.Focus();
                        return bValidate;
                    }
                }
                if (txtGeneralCondition.Text.Trim() == "")
                {
                    ShowMsgBox("Enter General Condition Nuts ");
                    txtGeneralCondition.Focus();
                    return bValidate;
                }
                if (txtLTSwitch.Text.Trim() == "")
                {
                    ShowMsgBox("Enter LT Switch ");
                    txtLTSwitch.Focus();
                    return bValidate;
                }
                if (cmbType.SelectedValue == "2")
                {
                    if (txtVoltage.Text.Trim() == "")
                    {
                        ShowMsgBox("Enter Voltage ");
                        txtVoltage.Focus();
                        return bValidate;
                    }
                    if (txtEarthTesting.Text.Trim() == "")
                    {
                        ShowMsgBox("Enter Earth Testing");
                        txtEarthTesting.Focus();
                        return bValidate;
                    }
                }
                if (txtMaintainanceDate.Text.Trim() != "")
                {
                    string sResult = Genaral.DateComparision(txtMaintainanceDate.Text,"",true, false);
                    if (sResult == "2")
                    {
                        ShowMsgBox("You Cannot select Previous date than today");
                        return bValidate;
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

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                string[] Arr = new string[2];
                if (ValidateForm() == true)
                {

                    clsTcMaintainance objTcMaintainance = new clsTcMaintainance();

                    objTcMaintainance.sTcCode = txtTCCode.Text.Trim().Replace("'", "''");
                    objTcMaintainance.sMaintainType = cmbType.SelectedValue;
                    objTcMaintainance.sMaintainBy = cmbMaintainedBy.SelectedValue.Trim().Replace("'", "''");
                    objTcMaintainance.sMaintainanceId = txtMaintainanceId.Text.Trim().Replace("'", "''");
                    objTcMaintainance.sMaintainanceDetailsId = txtMaintainDeId.Text.Trim().Replace("'", "''");
                    objTcMaintainance.sDTCCode = txtDTCCode.Text;
                    objTcMaintainance.sMaintainDate = txtMaintainanceDate.Text;

                    objTcMaintainance.sAntiClimbing = txtAntiClimbing.Text.Trim().Replace("'", "''");
                    objTcMaintainance.sArrestor = txtLightningArrestor.Text.Trim().Replace("'", "''");
                    objTcMaintainance.sBreather = txtBreather.Text.Trim().Replace("'", "''");
                    objTcMaintainance.sConditionNuts = txtGeneralCondition.Text.Trim().Replace("'", "''");
                    objTcMaintainance.sCrBy = objSession.UserId.Trim().Replace("'", "''");
                    objTcMaintainance.sEarthing = txtEarthing.Text.Trim().Replace("'", "''");
                    objTcMaintainance.sExplosion = txtExplosion.Text.Trim().Replace("'", "''");
                    objTcMaintainance.sGOSwitches = txtGoSwitches.Text.Trim().Replace("'", "''");
                    objTcMaintainance.sConnections = txtConnections.Text.Trim().Replace("'", "''");
                    objTcMaintainance.sOilLeakage = txtOil.Text.Trim().Replace("'", "''");
                    objTcMaintainance.sBushing = txtBushing.Text.Trim().Replace("'", "''");
                    objTcMaintainance.sArcing = txtArcingHorns.Text.Trim().Replace("'", "''");
                    objTcMaintainance.sSupports = txtSupports.Text.Trim().Replace("'", "''");
                    objTcMaintainance.sFuses = txtFuses.Text.Trim().Replace("'", "''");
                    objTcMaintainance.sLTWsitch = txtLTSwitch.Text.Trim().Replace("'", "''");
                    objTcMaintainance.sDangerPlate = txtDangerPlates.Text.Trim().Replace("'", "''");
                    objTcMaintainance.sEarthTesting = txtEarthTesting.Text.Trim().Replace("'", "''");
                    objTcMaintainance.sLoadBalancing = txtLoadBalancing.Text.Trim().Replace("'", "''");
                    objTcMaintainance.sVoltage = txtVoltage.Text.Trim().Replace("'", "''");


                    Arr = objTcMaintainance.SaveUpdateTcMaintainance(objTcMaintainance);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " Maintainance ");
                    }

                    if (Arr[1].ToString() == "0")
                    {
                        cmbType.Enabled = false;
                        txtDTCCode.Enabled = false;
                        cmdSave.Text = "Update";
                        cmdSearch.Enabled = false;
                        txtMaintainanceId.Text = objTcMaintainance.sMaintainanceId;
                        txtMaintainDeId.Text = objTcMaintainance.sMaintainanceDetailsId;
                        ShowMsgBox(Arr[0].ToString());
                        return;
                    }
                    else
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

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtTCCode.Text = string.Empty;
                txtDTCCode.Text = string.Empty;
                txtMaintainanceId.Text = string.Empty;
                txtMaintainDeId.Text = string.Empty;
                txtMaintainanceDate.Text = string.Empty;
                cmbType.SelectedIndex = 0;
                cmbMaintainedBy.SelectedIndex = 0;
                txtDTCName.Text = string.Empty;

                txtConnections.Text = string.Empty;
                txtFuses.Text = string.Empty;
                txtOil.Text = string.Empty;
                txtBreather.Text = string.Empty;
                txtBushing.Text = string.Empty;
                txtDangerPlates.Text = string.Empty;
                txtAntiClimbing.Text = string.Empty;
                txtLightningArrestor.Text = string.Empty;
                txtGoSwitches.Text = string.Empty;
                txtLoadBalancing.Text = string.Empty;
                txtSupports.Text = string.Empty;
                txtExplosion.Text = string.Empty;
                txtArcingHorns.Text = string.Empty;               
                txtEarthing.Text = string.Empty;
                txtGeneralCondition.Text = string.Empty;
                txtLTSwitch.Text = string.Empty;
                txtVoltage.Text = string.Empty;
                txtEarthTesting.Text = string.Empty;
  
                cmdSave.Text = "Save";
                txtDTCCode.Enabled = true;
                cmdSearch.Enabled = true;

                grdDtcMaintainance.DataSource = null;
                grdDtcMaintainance.DataBind();
                cmbType.Enabled = true;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdclose_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["ID"] == "0")
                    Response.Redirect("PrevMainView.aspx", false);
                else
                    Response.Redirect("TcMaintainanceView.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        public void LoadDtcMaintance()
        {
            DataTable dtDtcMainGrid = new DataTable();
            clsTcMaintainance objTcMaintainance = new clsTcMaintainance();
            try
            {
                dtDtcMainGrid = objTcMaintainance.LoadDtcMaintainGrid(txtDTCCode.Text);
                if (dtDtcMainGrid.Rows.Count > 0)
                {
                    grdDtcMaintainance.DataSource = dtDtcMainGrid;
                    grdDtcMaintainance.DataBind();
                    ViewState["dtMainGrid"] = dtDtcMainGrid;

                    dvDTCMaintain.Style.Add("display", "block");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdDtcMaintainance_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                clsTcMaintainance objTcMaintainance = new clsTcMaintainance();
                if (e.CommandName == "search")
                {
                    DataView dv = new DataView();
                    string sFilter = string.Empty;
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtcCode = (TextBox)row.FindControl("txtDTCCode");
                    TextBox txtDtrCode = (TextBox)row.FindControl("txtDTrCode");
                    DataTable dt = (DataTable)ViewState["dtMainGrid"];
                    dv = dt.DefaultView;

                    if (txtDtcCode.Text != "")
                    {
                        sFilter = "TM_DT_CODE Like '%" + txtDtcCode.Text.Replace("'", "`").ToString() + "%' AND";
                    }
                    if (txtDtrCode.Text != "")
                    {
                        sFilter += " TM_TC_CODE Like '%" + txtDtrCode.Text.Replace("'", "`") + "%' AND";
                    }

                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdDtcMaintainance.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdDtcMaintainance.DataSource = dv;
                            ViewState["dtMainGrid"] = dv.ToTable();
                            grdDtcMaintainance.DataBind();

                        }
                        else
                        {
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadDtcMaintance();
                    }


                    //if (dtQcSearch.Rows.Count > 0)
                    //{
                    //    grdQcDone.DataSource = dtQcSearch;
                    //    grdQcDone.DataBind();
                    //}
                    //else
                    //{
                    //    grdQcDone.DataSource = dtQcSearch;
                    //    grdQcDone.DataBind();

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
                dt.Columns.Add("TM_TC_CODE");
                dt.Columns.Add("TM_DT_CODE");
                dt.Columns.Add("TM_DATE");
                dt.Columns.Add("US_FULL_NAME");
                dt.Columns.Add("MD_NAME");
                grdDtcMaintainance.DataSource = dt;
                grdDtcMaintainance.DataBind();

                int iColCount = grdDtcMaintainance.Rows[0].Cells.Count;
                grdDtcMaintainance.Rows[0].Cells.Clear();
                grdDtcMaintainance.Rows[0].Cells.Add(new TableCell());
                grdDtcMaintainance.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdDtcMaintainance.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }

        protected void grdDtcMaintainance_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = (DataTable)ViewState["dtMainGrid"];
                //grdDtcMaintainance.PageIndex = 0;
                grdDtcMaintainance.PageIndex = e.NewPageIndex;
                grdDtcMaintainance.DataSource = dt;
                grdDtcMaintainance.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}