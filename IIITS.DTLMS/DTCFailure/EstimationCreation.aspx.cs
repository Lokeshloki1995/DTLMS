using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.IO;
using System.Net;
using System.Configuration;
using System.Text;
using System.Data.OleDb;
using System.Text.RegularExpressions;

namespace IIITS.DTLMS.DTCFailure
{
    public partial class EstimationCreation : System.Web.UI.Page
    {
        string strFormCode = "EstimationCreation";
        clsSession objSession = new clsSession();
        DataTable dtMaterial = new DataTable();
        DataTable dtLabour = new DataTable();
        DataTable dtSalvage = new DataTable();
        DataTable dtDocumentDetails = new DataTable();
        bool RetriveFromXML = false;
        string sFailId = string.Empty;
        string sEstId = string.Empty;
        static string sFailType = string.Empty;

        public string fileName3 { get; set; }


        string sFtpServerPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["EstimatioinVirtualPath"]);
        string sUserName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_USER"]);
        string sPassword = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_PASS"]);
        string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
        string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
            {
                Response.Redirect("~/Login.aspx", false);
            }
            else
            {
                Form.DefaultButton = cmdSave.UniqueID;
                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;
                txtEstDate.Attributes.Add("readonly", "readonly");
                lblFailType.Visible = false;
                lblEnhanceType.Visible = false;

                if (!IsPostBack)
                {
                    hdfsavestatus.Value = "0";
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("<span style='font-family: Calibri; font-size: 15pt'>* Note:</span><span style='margin-left:13px; position:absolute' > Please Select Repairer to popup rates </span>");
                    sb.AppendLine("<span style='margin-left:70px;' >WGP :WITHIN GUARANTEE PERIOD </span>");
                    sb.AppendLine("<span style='margin-left:70px;' >AGP:AFTER GUARANTEE PERIOD</span>");
                    sb.AppendLine("<span style='margin-left:70px;' >WRGP:WITHIN REPAIRER GUARANTEE PERIOD</span>");

                    sb.AppendLine("<span style='margin-left:70px;' > ANNEXURE-2 : Joint Inspection Report (PHYSICAL)  </span>");
                    sb.AppendLine("<span style='margin-left:70px;' > ANNEXURE-3 : Joint Inspection Report (INTERNAL)  </span>");
                    sb.AppendLine("<span style='margin-left:70px;' > ANNEXURE-4 : Analysis of Failed Distribution Transformer In WGP </span>");

                    lblNote.Text = sb.ToString().Replace(Environment.NewLine, "<br />");

                    //lblNote.Text = "* Note: Please Select Repairer to popup rates" + Environment.NewLine ;
                    CalendarExtender1.EndDate = System.DateTime.Now;
                    if (Request.QueryString["OfficeCode"] != null && Convert.ToString(Request.QueryString["OfficeCode"]) != "")
                    {
                        txtssOfficeCode.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["OfficeCode"]));

                    }
                    changefailtype();
                    if (objSession.RoleId == "2")
                    {
                        Genaral.Load_Combo("SELECT \"TR_ID\", UPPER(\"TR_NAME\") FROM \"TBLTRANSREPAIRER\",\"TBLTRANSREPAIREROFFCODE\"  WHERE \"TR_ID\"=\"TRO_TR_ID\"  AND   " + clsStoreOffice.GetOfficeCode(objSession.OfficeCode, "TRO_OFF_CODE") + " GROUP BY \"TR_ID\"  ORDER BY \"TR_NAME\"", "--Select--", cmbRepairer);
                    }
                    else
                    {
                        if (objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                        {
                            Genaral.Load_Combo("SELECT \"TR_ID\", UPPER(\"TR_NAME\") FROM \"TBLTRANSREPAIRER\" ,\"TBLTRANSREPAIREROFFCODE\" WHERE \"TR_ID\"=\"TRO_TR_ID\" AND CAST(\"TRO_OFF_CODE\" AS TEXT) = SUBSTR(CAST('" + txtssOfficeCode.Text + "' AS TEXT),1,'" + Constants.Division + "') ORDER BY \"TR_NAME\" ", "--Select--", cmbRepairer);
                        }
                        else
                        {
                            Genaral.Load_Combo("SELECT \"TR_ID\", UPPER(\"TR_NAME\") FROM \"TBLTRANSREPAIRER\" ,\"TBLTRANSREPAIREROFFCODE\" WHERE \"TR_ID\"=\"TRO_TR_ID\" AND CAST(\"TRO_OFF_CODE\" AS TEXT) = SUBSTR(CAST('" + objSession.OfficeCode + "' AS TEXT),1,'" + Constants.Division + "') ORDER BY \"TR_NAME\" ", "--Select--", cmbRepairer);
                        }
                    }
                    cmbCoreType_SelectedIndexChanged(sender, e);
                    if (!(Convert.ToString(Request.QueryString["EstID"]) == null))
                    {
                        sFailId = Genaral.UrlDecrypt(Convert.ToString(Request.QueryString["FailId"]));
                        sEstId = Genaral.UrlDecrypt(Convert.ToString(Request.QueryString["EstID"]));
                        txtActiontype.Text = Genaral.UrlDecrypt(Convert.ToString(Request.QueryString["ActionType"]));
                        hdfEstId.Value = sEstId;
                        Session["WFOId"] = "0";
                        LoadFailedDetails(sFailId);

                        lnkBudgetstat.Visible = false;
                        GetDatafromMainTable(sEstId);
                    }
                    else
                    {
                        if (!(Convert.ToString(Request.QueryString["FailId"]) == null))
                        {
                            sFailId = Genaral.UrlDecrypt(Convert.ToString(Request.QueryString["FailId"]));
                            hdfEstId.Value = sFailId;
                            txtActiontype.Text = Genaral.UrlDecrypt(Convert.ToString(Request.QueryString["ActionType"]));
                            lnkBudgetstat.Visible = false;
                            for (int i = 0; i < Session.Contents.Count; i++)
                            {
                                var key = Session.Keys[i];
                                var value = Session[i];
                                if (key.ToString() == "WFDataId")
                                {
                                    hdfWFDataId.Value = Convert.ToString(HttpContext.Current.Session["WFDataId"]);
                                }
                            }
                        }

                        if (txtActiontype.Text == "M")
                        {
                            cmdSave.Text = "Modify And Approve";
                            if (cmbFailType.SelectedValue == "2")
                            {
                                cmdEst.Visible = false;
                                cmdViewPGRS.Visible = true;

                            }
                            else
                            {
                                cmdEst.Visible = false;
                                cmdViewPGRS.Visible = true;
                            }

                        }
                        else if (txtActiontype.Text == "R")
                        {
                            cmdSave.Text = "Reject";
                        }

                        if (txtActiontype.Text == "V")
                        {
                            GetFailId(sFailId);
                        }
                        else
                        {
                            LoadFailedDetails(sFailId);
                        }
                        WorkFlowConfig();
                        BindgridView(SFTPmainfolder, sUserName, sPassword);

                        if (objSession.sRoleType != "4")
                        {
                            if (Convert.ToString(Session["BOID"]) == "73")
                            {
                                Session["BOID"] = "73";
                                ViewState["BOID"] = "73";
                            }
                            else
                            {
                                Session["BOID"] = "45";
                                ViewState["BOID"] = "45";
                            }
                        }
                        else
                        {
                            ViewState["BOID"] = Convert.ToString(Session["BOID"]);
                        }

                    }
                }

                if (hdfsavestatus.Value == "1")
                {
                    cmdSave.Enabled = false;
                }
                else
                {
                    cmdSave.Enabled = true;
                }


                hdfEnhancementId.Value = txtFailureId.Text;
                ApprovalHistoryView.BOID = Convert.ToString(ViewState["BOID"]);
                ApprovalHistoryView.sRecordId = hdfEstId.Value;

                if (txtActiontype.Text == "M")
                {
                    clsApproval objLevel = new clsApproval();
                    string sLevel = objLevel.sGetApprovalLevel(Convert.ToString(ViewState["BOID"]), objSession.RoleId);
                    if (sLevel != "" && sLevel != null)
                    {
                        if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                        {
                            cmdSave.Text = " Modify and Submit";
                            if (cmbFailType.SelectedValue == "2")
                            {
                                cmdEst.Visible = false;
                                cmdViewPGRS.Visible = true;
                            }
                            else
                            {
                                cmdEst.Visible = false;
                                cmdViewPGRS.Visible = false;
                            }
                        }

                        else if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                        {
                            cmdSave.Text = " Modify and Approve";
                            if (cmbFailType.SelectedValue == "2")
                            {
                                cmdEst.Visible = false;
                                cmdViewPGRS.Visible = true;
                            }
                            else
                            {
                                cmdEst.Visible = false;
                                cmdViewPGRS.Visible = false;
                            }
                        }
                    }
                }
                else if (txtActiontype.Text == "A")
                {
                    clsApproval objLevel = new clsApproval();
                    string sLevel = objLevel.sGetApprovalLevel(Convert.ToString(ViewState["BOID"]), objSession.RoleId);
                    if (sLevel != "" && sLevel != null)
                    {
                        if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                        {
                            cmdSave.Text = "Submit";
                            if (cmbFailType.SelectedValue == "2")
                            {
                                //cmdEst.Visible = true;
                                cmdEst.Visible = false;
                            }
                            else
                            {
                                cmdEst.Visible = false;
                            }
                        }
                        else if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                        {
                            cmdSave.Text = "Approve";
                            if (cmbFailType.SelectedValue == "2")
                            {
                                cmdEst.Visible = false;
                                cmdViewPGRS.Visible = true;
                            }
                            else
                            {
                                cmdEst.Visible = false;
                                cmdViewPGRS.Visible = false;
                            }
                        }
                    }

                }
            }
            cmbFailType.Visible = false;
            if (cmbFailType.SelectedValue == "2")
            {
                //cmdEst.Visible = true;
                cmdEst.Visible = false;
                cmdViewPGRS.Visible = true;
                cmdSave.Enabled = true;
            }
            else
            {
                cmdEst.Visible = false;
            }
            if (txtActiontype.Text == "V")
            {
                cmdEst.Visible = false;
                cmdViewPGRS.Visible = true;
            }
        }

        public void LOadEstimateNumber(string officeCode)
        {
            try
            {
                clsEstimation objEstmate = new clsEstimation();
                string sEstNo = objEstmate.GenerateEstimationNo(officeCode);
                txtEstNo.Text = sEstNo;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetFailId(string sFail_id)
        {
            try
            {
                string sFailId = string.Empty;
                clsFailureEntry objfailId = new clsFailureEntry();

                if (Session["WFOId"] != null && Convert.ToString(Session["WFOId"]) != "")
                {
                    hdfWFOId.Value = Convert.ToString(Session["WFOId"]);
                }

                sFailId = objfailId.getFailureType(hdfWFOId.Value);

                if (Convert.ToString(sFailId.Split('~').GetValue(0)) == "2")
                {
                    sFailId = Convert.ToString(sFailId.Split('~').GetValue(1));
                }
                else
                {
                    clsEstimation objEstmate = new clsEstimation();
                    sFailId = objEstmate.GetFailId(sFail_id, "View");
                }

                LoadFailedDetails(sFailId);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void WorkFlowConfig()
        {
            string sApproveStatus = string.Empty;
            try
            {
                if (Request.QueryString["ActionType"] != null && Convert.ToString(Request.QueryString["ActionType"]) != "")
                {
                    //WFDataId

                    if (Session["WFOId"] != null && Convert.ToString(Session["WFOId"]) != "")
                    {
                        hdfWFDataId.Value = Convert.ToString(Session["WFDataId"]);
                        hdfWFOId.Value = Convert.ToString(Session["WFOId"]);
                        hdfApproveStatus.Value = Convert.ToString(Session["ApproveStatus"]);
                        hdfWFOAutoId.Value = Convert.ToString(Session["WFOAutoId"]);

                        //Session["WFDataId"] = null;
                        //Session["WFOId"] = null;
                        //Session["ApproveStatus"] = null;
                        //Session["WFOAutoId"] = null;
                    }

                    if (hdfWFDataId.Value != "0" && hdfWFDataId.Value != "")
                    {
                        if (txtActiontype.Text != "V")
                        {
                            sApproveStatus = GetPreviousApproveStatus(hdfWFOId.Value);
                        }


                        if (txtActiontype.Text == "V")
                        {
                            if (sApproveStatus == "3")
                            {
                                sApproveStatus = "2";
                            }
                        }

                        GetDataFromXML(hdfWFDataId.Value, sApproveStatus);
                        if (txtActiontype.Text == "M")
                        {
                            cmdCalc.Visible = true;
                            cmdEst.Enabled = false;
                            cmdViewPGRS.Visible = true;
                            cmbOilType.Enabled = false;
                        }
                        else
                            cmdCalc.Visible = false;
                        if (cmbFailType.SelectedValue == "2")
                        {
                            cmdEst.Enabled = false;
                            cmdViewPGRS.Visible = true;
                        }
                        else
                        {
                            cmdEst.Enabled = false;

                        }

                    }

                    if (cmbFailType.SelectedValue == "2")
                    {
                        if (cmbOilType.SelectedValue == "2")
                        {
                            divtxtoil.Visible = false;
                            divCoretypeHide.Visible = false;
                            divInsulationtypeHide.Visible = true;
                            divman.Visible = false;
                            divOiltypeHide.Visible = false;
                            divstarrating.Visible = true;
                        }
                        else
                        {
                            divman.Visible = false;
                            divtxtoil.Visible = false;
                            divCoretypeHide.Visible = false;
                            divInsulationtypeHide.Visible = true;

                            divOiltypeHide.Visible = true;
                            //  divstarrating.Visible = false;

                        }
                        divOiltypeHide.Visible = true;
                        //if (hdfStatusflag.Value == "2")
                        //{
                        //    lblEnhanceText1.Visible = true;
                        //    lblIDText.Visible = false;
                        //    cmbFailType.Visible = false;
                        //    lblFailType.Visible = false;
                        //}
                        clsFailureEntry objFailure = new clsFailureEntry();
                        if (hdfStatusflag.Value == "4" && hdfWFDataId.Value == "0")
                        {
                            divOiltypeHide.Visible = true;
                        }

                        // divCoretypeHide.Visible = true;
                        //divInsulationtypeHide.Visible = true;
                        divmMulticoilHide.Visible = false;
                        cmdCalc.Visible = false;
                        divLabourCost.Visible = false;
                        divSalvages.Visible = false;
                        divMaterialCost.Visible = false;
                        lblNote.Visible = false;
                        divAnnuFile.Visible = false;
                        cmdViewPGRS.Visible = false;
                        txtcertify.Visible = false;
                        txtcertify.Enabled = false;
                        Lbl.Visible = false;

                    }
                    dvComments.Style.Add("display", "block");
                    //cmdReset.Enabled = false;

                    if (hdfWFOAutoId.Value != "0")
                    {

                        dvComments.Style.Add("display", "none");
                    }
                    CheckFormCreatorLevel();
                    if (txtActiontype.Text == "V")
                    {
                        cmdSave.Text = "View";
                        //cmdReset.Enabled = false;
                        cmdReset.Visible = false;
                        dvComments.Style.Add("display", "none");
                    }
                }
                else
                {
                    if (cmdSave.Text != "Save" && cmdSave.Text != "Submit" && cmdSave.Text != "Approve" && cmdSave.Text != "View")
                    {
                        cmdSave.Enabled = false;

                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public string GetPreviousApproveStatus(string sWo_id)
        {
            string sApprove_id = string.Empty;
            try
            {
                clsFailureEntry objFailure = new clsFailureEntry();
                sApprove_id = objFailure.GetPreviousApproveStatus(sWo_id);
                return sApprove_id;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return sApprove_id;
            }
        }

        public void LoadFailedDetails(string sFailId)
        {
            try
            {
                clsFailureEntry objFailure = new clsFailureEntry();
                //txtFailureId.Text = hdfFailureId.Value;
                objFailure.sFailureId = sFailId;

                objFailure.GetFailureDetails(objFailure);



                txtFailureId.Text = sFailId;
                txtFailureDate.Text = objFailure.sFailureDate;
                txtDTCName.Text = objFailure.sDtcName;
                txtDTCCode.Text = objFailure.sDtcCode;
                txtDeclaredBy.Text = objFailure.sCrby;
                txtTCCode.Text = objFailure.sDtcTcCode;
                txtCapacity.Text = objFailure.sDtcCapacity;
                txtFailureId.Text = objFailure.sFailureId;
                cmbRepairer.SelectedValue = objFailure.sLastRepairedBy;
                hdfFailureId.Value = objFailure.sOfficeCode;
                hdfStatusflag.Value = objFailure.sStatus_flag;
                cmbOilType.SelectedValue = objFailure.sOilType;

                string rating = objFailure.GetTC_Rating(objFailure.sFailureId);
                cmbStarRating.SelectedValue = rating;

                cmbOilType.Enabled = false;
                lblIDText.Enabled = false;
                txtFailureId.Enabled = false;
                for (int i = 0; i < Session.Contents.Count; i++)
                {
                    var key = Session.Keys[i];
                    var value = Session[i];
                    if (key == "WFOId")
                    {
                        hdfWFOId.Value = Convert.ToString(HttpContext.Current.Session["WFOId"]);
                    }

                }
                if (objFailure.sStatus_flag == "2" || objFailure.sStatus_flag == "4")
                {
                    txtEnhanceCapcity.Text = objFailure.sEnhancedCapacity;
                    txtEnhanceCapcity.Visible = true;
                    lblEnhancap.Visible = true;
                }

                //if (Session.Contents("WFOId"))
                //{
                //    hdfWFOId.Value = Session["WFOId"].ToString();
                //}
                //    if (objFailure.sOilQuantityTank != null && objFailure.sOilQuantityTank != "")
                //{
                //BindgridView(SFTPmainfolder, sUserName, sPassword);
                //}

                hdfGuarenteeSource.Value = objFailure.sGuarantyType;
                cmbGuarenteeType.SelectedValue = objFailure.sGuarantyType;
                //ViewState["OffCode"] = objFailure.sOfficeCode;
                LOadEstimateNumber(objFailure.sOfficeCode);
                txtDtcId.Text = objFailure.sDtcId;
                txtTCId.Text = objFailure.sTCId;

                clsEstimation objest = new clsEstimation();
                objest.sFailureId = sFailId;
                DataTable dt = objest.GetEstDetails(objest);
                string failtype = string.Empty;
                //record id will be -ve for pending records 
                if (dt.Rows.Count == 0)
                {
                    failtype = "0";
                }
                else
                {
                    failtype = Convert.ToString(dt.Rows[0]["EST_FAIL_TYPE"]);
                }

                if (objFailure.sStatus_flag == "4")
                {
                    cmbFailType.SelectedValue = "2";
                    cmbFailType.Enabled = false;
                    divOiltypeHide.Visible = true;
                }
                else if (objFailure.sStatus_flag == "2" && failtype == "2")
                {
                    if (dt.Rows.Count > 0)
                    {
                        if (Convert.ToString(dt.Rows[0]["EST_OILTYPE"]) == "2")
                        {
                            cmbOilType.SelectedValue = Convert.ToString(dt.Rows[0]["EST_OILTYPE"]);
                            txtOilValue.Text = Convert.ToString(dt.Rows[0]["EST_OIL_QNTY"]);

                            cmbStarRating.SelectedValue = Convert.ToString(dt.Rows[0]["EST_STAR_RATING"]);
                        }
                        else
                        {
                            cmbOilType.SelectedValue = "1";
                        }


                    }
                    else
                    {

                        cmbOilType.Enabled = false;
                        txtOilValue.Enabled = false;
                    }
                    sFailType = "2";
                    cmbFailType.SelectedValue = "2";

                    cmbFailType.Visible = false;
                    cmbRepairer.Visible = false;
                    divRepairerFail.Visible = false;
                    cmdCalc.Visible = false;
                    divLabourCost.Visible = false;
                    divSalvages.Visible = false;
                    divMaterialCost.Visible = false;
                    lblNote.Visible = false;
                    divAnnuFile.Visible = false;
                    cmdViewPGRS.Visible = false;
                    txtcertify.Visible = false;
                    txtcertify.Enabled = false;
                    Lbl.Visible = false;

                    if (txtActiontype.Text == "V")
                    {
                        cmdSave.Text = "view";
                        dvComments.Visible = false;
                        cmdCalc.Visible = false;
                        cmdEst.Enabled = false;
                    }
                }

                else if (objFailure.sStatus_flag == "2")
                {
                    cmbFailType.SelectedValue = "2";
                    cmbFailType.Enabled = false;
                    cmbOilType.SelectedValue = "1";
                    // cmbOilType.Enabled = false;
                    divOiltypeHide.Visible = true;

                    lblEnhanceText1.Visible = true;
                    lblIDText.Visible = false;
                    lblEnhanceType.Visible = true;
                    lblEnhancap.Visible = true;
                    txtEnhanceCapcity.Visible = true;
                    lblFailType.Visible = false;
                }
                if (Convert.ToString(Session["BOID"]) == "73")
                {

                    ViewState["BOID"] = "73";
                }
                else
                {
                    ViewState["BOID"] = "45";
                }

                //clsEstimation obj = new clsEstimation();
                //obj.GetEstimateDetailsFromXML(obj);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadMaterialDetails(string sWoundType, string srateType)
        {
            try
            {
                DataTable dt = new DataTable();
                clsEstimate objEstimate = new clsEstimate();
                objEstimate.eUser = cmbRepairer.SelectedValue;
                objEstimate.eCapacity = txtCapacity.Text;
                objEstimate.sWoundType = sWoundType;
                objEstimate.srateType = srateType;
                objEstimate.sOfficecode = objSession.OfficeCode.Substring(0, 3);
                dt = objEstimate.LoadExistMaterials(objEstimate);
                ViewState["Material"] = dt;
                grdMaterialMast.DataSource = dt;
                grdMaterialMast.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }

        public void LoadLabourDetails(string sWoundType, string srateType)
        {
            try
            {
                DataTable dt = new DataTable();
                clsEstimate objEstimate = new clsEstimate();
                objEstimate.eUser = cmbRepairer.SelectedValue;
                objEstimate.eCapacity = txtCapacity.Text;
                objEstimate.sWoundType = sWoundType;
                objEstimate.srateType = srateType;
                objEstimate.sOfficecode = objSession.OfficeCode.Substring(0, 3);
                dt = objEstimate.LoadExistLabour(objEstimate);
                ViewState["Labour"] = dt;
                grdLabourMast.DataSource = dt;
                grdLabourMast.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void LoadSalvageDetails(string sWoundType, string srateType)
        {
            try
            {
                DataTable dt = new DataTable();
                clsEstimate objEstimate = new clsEstimate();
                objEstimate.eUser = cmbRepairer.SelectedValue;
                objEstimate.eCapacity = txtCapacity.Text;
                objEstimate.sWoundType = sWoundType;
                objEstimate.srateType = srateType;
                objEstimate.sOfficecode = objSession.OfficeCode.Substring(0, 3);
                dt = objEstimate.LoadExistSalvage(objEstimate);
                ViewState["Salvage"] = dt;
                grdSalvageMast.DataSource = dt;
                grdSalvageMast.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void GetDataFromXML(string WFDataId, string sApproveStatus)
        {
            try
            {
                DataTable dtEstDetails = new DataTable();
                clsFailureEntry objFailure = new clsFailureEntry();
                string sWo_Id = string.Empty;
                //txtFailureId.Text = hdfFailureId.Value;
                if (Session["WFOId"] != null && Convert.ToString(Session["WFOId"]) != "")
                {
                    hdfWFOId.Value = Convert.ToString(Session["WFOId"]);
                }


                string FailType = objFailure.getFailureType(hdfWFOId.Value);


                if (txtActiontype.Text == "V")
                {
                    if (Convert.ToString(FailType.Split('~').GetValue(0)) == "2")
                    {
                        objFailure.sFailureId = Convert.ToString(FailType.Split('~').GetValue(1));
                    }
                    objFailure.sFailureId = txtFailureId.Text;
                }
                else
                {
                    sWo_Id = hdfWFOId.Value;
                    dtEstDetails = objFailure.getFailId(sWo_Id);
                    if (dtEstDetails.Rows.Count > 0)
                    {
                        txtFailureId.Text = Convert.ToString(dtEstDetails.Rows[0]["WO_DATA_ID"]);
                    }
                    objFailure.sFailureId = txtFailureId.Text;
                }
                objFailure.GetFailureDetails(objFailure);

                txtFailureDate.Text = objFailure.sFailureDate;
                txtDTCName.Text = objFailure.sDtcName;
                txtDTCCode.Text = objFailure.sDtcCode;
                txtDeclaredBy.Text = objFailure.sCrby;
                txtTCCode.Text = objFailure.sDtcTcCode;
                txtCapacity.Text = objFailure.sDtcCapacity;
                cmbStarRating.SelectedValue = objFailure.sRating; ////
                cmbRepairer.SelectedValue = objFailure.sLastRepairedBy;
                hdfFailureId.Value = objFailure.sOfficeCode;
                cmbGuarenteeType.SelectedValue = objFailure.sGuarantyType;
                cmbOilType.SelectedValue = objFailure.sOilType;

                hdfGuarenteeSource.Value = objFailure.sGuarantyType;
                if (txtActiontype.Text != "V")
                {
                    LOadEstimateNumber(objFailure.sOfficeCode);
                }
                txtDtcId.Text = objFailure.sDtcId;
                txtTCId.Text = objFailure.sTCId;
                //if (cmbStarRating.SelectedValue == "0")
                //{
                //    if (objFailure.sRating == "1")
                //        cmbStarRating.SelectedValue = "4";
                //    if (objFailure.sRating == "2")
                //        cmbStarRating.SelectedValue = "5";
                //}

                if (hdfStatusflag.Value == null || hdfStatusflag.Value == "")
                {
                    hdfStatusflag.Value = Convert.ToString(FailType.Split('~').GetValue(0));
                }

                if (objFailure.sStatus_flag == "2")
                {
                    divRepairerFail.Visible = true;
                    sFailType = "2";
                    cmbFailType.SelectedValue = "2";
                    hdfStatusflag.Value = "2";
                    cmbFailType.Visible = true;
                    cmbFailType.Enabled = false;
                    cmbRepairer.Visible = false;
                    cmdCalc.Visible = false;
                    divLabourCost.Visible = false;
                    divSalvages.Visible = false;
                    divMaterialCost.Visible = false;
                    lblNote.Visible = false;
                    divAnnuFile.Visible = false;
                    cmdViewPGRS.Visible = false;
                    txtcertify.Visible = false;
                    txtcertify.Enabled = false;
                    Lbl.Visible = false;
                    if (txtActiontype.Text == "M")
                    {
                        cmbCoreType.Enabled = true;
                        cmbInsulationtype.Enabled = true;
                        cmbOilType.Enabled = true;
                        cmbman.Enabled = true;
                        cmbStarRating.Enabled = true;
                    }
                    else
                    {
                        cmbCoreType.Enabled = false;
                        cmbInsulationtype.Enabled = false;
                        cmbOilType.Enabled = false;
                        cmbman.Enabled = false;
                        cmbStarRating.Enabled = false;
                    }

                    lblEnhanceText1.Visible = true;
                    lblIDText.Visible = false;
                    lblEnhanceType.Visible = true;
                    lblEnhancap.Visible = true;
                    txtEnhanceCapcity.Visible = true;
                    lblFailType.Visible = false;

                    if (txtActiontype.Text == "V")
                    {
                        cmdSave.Text = "view";
                        dvComments.Visible = false;
                        cmdCalc.Visible = false;
                    }
                }
                clsEstimation obj = new clsEstimation();

                // new added  by RUdra  for rejected failure and Estimation

                if (objFailure.sStatus_flag == "2" || objFailure.sStatus_flag == "4")
                {
                    txtEnhanceCapcity.Text = objFailure.sEnhancedCapacity;
                    txtEnhanceCapcity.Visible = true;
                    lblEnhancap.Visible = true;

                    cmbFailType.SelectedValue = "2";
                    cmbFailType.Enabled = false;
                    divOiltypeHide.Visible = true;
                }
                if (txtActiontype.Text == "V" || txtActiontype.Text == "M" || txtActiontype.Text == "R")
                {
                    obj.sWFO_id = hdfWFDataId.Value;
                }
                else
                {
                    if (dtEstDetails.Rows.Count > 0)
                    {
                        obj.sWFO_id = Convert.ToString(dtEstDetails.Rows[0]["WO_WFO_ID"]);
                    }
                }

                if (sWo_Id != null && sWo_Id != "")
                {
                    hdfWFOId.Value = sWo_Id;
                    //hdfWFDataId.Value = sWo_Id;
                }

                #region added on 06-12-2023 based on Jira ID: DTLMS-2280

                if (sApproveStatus == "3")
                {
                    obj.GetEstimateDetailsFromXML(obj);
                    txtEstDate.Text = obj.sEstDate;
                    cmbStarRating.SelectedValue = obj.sstarrating;
                    cmbInsulationtype.SelectedValue = obj.sInsulationtype;

                }
                #endregion

                if (sApproveStatus != "3" && (Convert.ToString(FailType.Split('~').GetValue(0)) != "2"))
                {
                    obj.GetEstimateDetailsFromXML(obj);
                    cmbRepairer.SelectedValue = obj.sLastRepair;
                    cmbMaterialType.SelectedValue = obj.sWoundType;

                    cmbRateType.SelectedValue = obj.srateType;
                    cmbFailType.SelectedValue = obj.sFailType;
                    cmbGuarenteeType.SelectedValue = obj.sGuaranteetype;
                    txtEstDate.Text = obj.sEstDate;
                    txtEstNo.Text = obj.sEstimationNo;
                    cmbCoreType.SelectedValue = obj.sCore;

                    if (cmbCoreType.SelectedIndex > 0)
                    {
                        string qry = "SELECT \"TIT_ID\",\"TIT_INSULATION_NAME\" FROM \"TBLTRANSINSULATIONTYPE\" where  \"TIT_ID\"<>0  and  \"TIT_TT_ID\"= " + obj.sCore + "";
                        Genaral.Load_Combo(qry, "--Select--", cmbInsulationtype);
                    }
                    else
                    {
                        string qry = "SELECT \"TIT_ID\",\"TIT_INSULATION_NAME\" FROM \"TBLTRANSINSULATIONTYPE\" where  \"TIT_ID\"<>0 ";
                        Genaral.Load_Combo(qry, "--Select--", cmbInsulationtype);
                    }

                    //Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='OQ'   ORDER BY \"MD_ID\"", "--Select--", cmboilqnty);


                    // textInsulation.Text = obj.sInsulationtype;
                    cmbInsulationtype.SelectedValue = obj.sInsulationtype;
                    RetriveFromXML = true;
                    dtMaterial = obj.dtMaterial;
                    grdMaterialMast.DataSource = obj.dtMaterial;
                    grdMaterialMast.DataBind();

                    //oil concept added
                    if (obj.soiltype == "2")
                    {
                        cmbOilType.SelectedValue = obj.soiltype;
                        //Convert.ToString(cmboilqnty.SelectedItem)
                        txtOilValue.Text = obj.soiltxtvalue;

                        cmbStarRating.SelectedValue = obj.sstarrating;
                    }
                    else
                    {

                        cmbOilType.Enabled = false;
                        txtOilValue.Enabled = false;
                        cmbOilType.SelectedValue = "1";
                    }
                    cmbStarRating.SelectedValue = obj.sstarrating;
                    double MaterialTotal = 0;

                    if (grdMaterialMast.Rows.Count > 0)
                    {
                        foreach (GridViewRow row in grdMaterialMast.Rows)
                        {
                            String sTotal = "";
                            if (((CheckBox)row.FindControl("chkmaterial")).Checked == true)
                            {
                                if (txtActiontype.Text == "M")
                                {
                                    ((TextBox)row.FindControl("txtMqty")).Text = ((Label)row.FindControl("lblQuantity")).Text;
                                }
                                sTotal = ((Label)row.FindControl("lblTotal")).Text;
                            }
                            MaterialTotal = MaterialTotal + Convert.ToDouble(sTotal);
                        }

                        grdMaterialMast.FooterRow.Cells[9].Text = Convert.ToString(MaterialTotal);
                    }


                    dtLabour = obj.dtLabour;
                    grdLabourMast.DataSource = obj.dtLabour;
                    grdLabourMast.DataBind();
                    double LabourTotal = 0;

                    if (grdLabourMast.Rows.Count > 0)
                    {
                        foreach (GridViewRow row in grdLabourMast.Rows)
                        {
                            String sTotal = "";
                            if (((CheckBox)row.FindControl("chkElabour")).Checked == true)
                            {
                                if (txtActiontype.Text == "M")
                                {
                                    ((TextBox)row.FindControl("txtLqty")).Text = ((Label)row.FindControl("lbllabQuantity")).Text;
                                }
                                sTotal = ((Label)row.FindControl("lbllabtotal")).Text;
                            }
                            LabourTotal = LabourTotal + Convert.ToDouble(sTotal);
                        }

                        grdLabourMast.FooterRow.Cells[9].Text = Convert.ToString(LabourTotal);
                    }


                    dtSalvage = obj.dtSalvage;
                    grdSalvageMast.DataSource = obj.dtSalvage;
                    grdSalvageMast.DataBind();
                    double SalvageTotal = 0;

                    if (grdSalvageMast.Rows.Count > 0)
                    {
                        foreach (GridViewRow row in grdSalvageMast.Rows)
                        {
                            String sTotal = "";
                            if (((CheckBox)row.FindControl("chkESalvage")).Checked == true)
                            {
                                if (txtActiontype.Text == "M")
                                {
                                    ((TextBox)row.FindControl("txtSqty")).Text = ((Label)row.FindControl("lblSalQuantity")).Text;
                                }
                                sTotal = ((Label)row.FindControl("lblsaltotal")).Text;
                            }
                            SalvageTotal = SalvageTotal + Convert.ToDouble(sTotal);
                        }

                        grdSalvageMast.FooterRow.Cells[9].Text = Convert.ToString(SalvageTotal);
                    }


                    lblTotalCharges.Text = Convert.ToString(MaterialTotal + LabourTotal - SalvageTotal);

                    dtDocumentDetails = obj.dtDocuments;
                    grdDocuments.DataSource = dtDocumentDetails;
                    grdDocuments.DataBind();
                    ViewState["DOCUMENTS"] = dtDocumentDetails;

                    foreach (GridViewRow row in grdDocuments.Rows)
                    {
                        ((LinkButton)row.FindControl("lnkDelet")).Visible = false;
                        ((LinkButton)row.FindControl("lnkView")).Visible = true;
                    }

                    cmbFailType.Enabled = false;
                    txtFailureId.Enabled = false;
                    cmbRepairer.Enabled = false;
                    cmbMaterialType.Enabled = false;
                    cmbRateType.Enabled = false;
                    cmbGuarenteeType.Enabled = false;
                    txtEstDate.Enabled = false;
                    cmdAdd.Enabled = false;
                    if (txtActiontype.Text == "M")
                    {
                        cmbCoreType.Enabled = true;
                        cmbInsulationtype.Enabled = true;
                        cmbOilType.Enabled = true;
                        cmbman.Enabled = true;
                        cmbStarRating.Enabled = true;
                        txtEstDate.Enabled = true;
                    }
                    else
                    {
                        cmbCoreType.Enabled = false;
                        cmbInsulationtype.Enabled = false;
                        cmbOilType.Enabled = false;
                        cmbman.Enabled = false;
                        cmbStarRating.Enabled = false;
                    }
                    cmbOilType.Enabled = false;
                    txtOilValue.Enabled = false;

                }// for enhance estimation 
                else if ((Convert.ToString(FailType.Split('~').GetValue(0)) == "2"))
                {
                    obj.GetEstimateDetailsFromXML(obj);
                    txtEstDate.Text = obj.sEstDate;
                    cmbCoreType.SelectedIndex = Convert.ToInt16(obj.sCore);
                    if (cmbCoreType.SelectedIndex > 0)
                    {
                        string qry = "SELECT \"TIT_ID\",\"TIT_INSULATION_NAME\" FROM \"TBLTRANSINSULATIONTYPE\" where  \"TIT_ID\"<>0  and  \"TIT_TT_ID\"= " + obj.sCore + "";
                        Genaral.Load_Combo(qry, "--Select--", cmbInsulationtype);
                    }
                    else
                    {
                        string qry = "SELECT \"TIT_ID\",\"TIT_INSULATION_NAME\" FROM \"TBLTRANSINSULATIONTYPE\" where  \"TIT_ID\"<>0 ";
                        Genaral.Load_Combo(qry, "--Select--", cmbInsulationtype);
                    }
                    cmbInsulationtype.SelectedValue = obj.sInsulationtype;
                    cmbOilType.SelectedValue = obj.soiltype;
                    //oil concept added
                    if (obj.soiltype == "2")
                    {
                        cmbOilType.SelectedValue = obj.soiltype;
                        //Convert.ToString(cmboilqnty.SelectedItem)
                        txtOilValue.Text = obj.soiltxtvalue;

                        cmbStarRating.SelectedValue = obj.sstarrating;
                    }
                    //else
                    //{

                    //    cmbOilType.Enabled = false;
                    //    txtOilValue.Enabled = false;
                    //    cmbOilType.SelectedValue = "1";
                    //}

                }
                else
                {
                    txtActiontype.Text = "M";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        private void GetDatafromMainTable(string sEstId)
        {
            try
            {
                clsEstimation obj = new clsEstimation();
                obj.sEstimationId = sEstId;
                obj.GetDetailsfromMainDB(obj);
                RetriveFromXML = true;

                txtEstDate.Text = Convert.ToString(obj.sEstDate);
                cmbRepairer.SelectedValue = obj.sLastRepair;
                cmbFailType.SelectedValue = obj.sFailType;

                cmbMaterialType.SelectedValue = obj.sWoundType;
                cmbRateType.SelectedValue = obj.srateType;

                string qry = "SELECT \"TIT_ID\",\"TIT_INSULATION_NAME\" FROM \"TBLTRANSINSULATIONTYPE\" where  \"TIT_ID\"<>0 ";
                Genaral.Load_Combo(qry, "--Select--", cmbInsulationtype);

                cmbCoreType.SelectedValue = obj.sCore;
                cmbInsulationtype.SelectedValue = obj.sInsulationtype;

                cmbCoreType.Enabled = false;
                cmbInsulationtype.Enabled = false;

                if (obj.soiltype == "2")
                {
                    cmbOilType.SelectedValue = obj.soiltype;
                    //Convert.ToString(cmboilqnty.SelectedItem)
                    txtOilValue.Text = obj.soiltxtvalue;

                    cmbStarRating.SelectedValue = obj.sstarrating;
                }
                else
                {

                    cmbOilType.Enabled = false;
                    txtOilValue.Enabled = false;
                }

                WorkFlowConfig();
                dtMaterial = obj.dtMaterial;
                grdMaterialMast.DataSource = obj.dtMaterial;
                grdMaterialMast.DataBind();



                double MaterialTotal = 0;
                if (dtMaterial.Rows.Count > 0)
                {
                    foreach (GridViewRow row in grdMaterialMast.Rows)
                    {
                        String sTotal = "0";
                        //if (((CheckBox)row.FindControl("chkmaterial")).Checked == true)
                        //{
                        if (txtActiontype.Text == "M")
                        {
                            ((TextBox)row.FindControl("txtMqty")).Text = ((Label)row.FindControl("lblQuantity")).Text;
                        }
                        sTotal = ((Label)row.FindControl("lblTotal")).Text;
                        // }
                        MaterialTotal = MaterialTotal + Convert.ToDouble(sTotal);
                    }
                    grdMaterialMast.FooterRow.Cells[9].Text = Convert.ToString(MaterialTotal);
                }




                dtLabour = obj.dtLabour;
                grdLabourMast.DataSource = obj.dtLabour;
                grdLabourMast.DataBind();
                double LabourTotal = 0;
                if (dtLabour.Rows.Count > 0)
                {
                    foreach (GridViewRow row in grdLabourMast.Rows)
                    {
                        String sTotal = "0";
                        if (((CheckBox)row.FindControl("chkElabour")).Checked == true)
                        {
                            if (txtActiontype.Text == "M")
                            {
                                ((TextBox)row.FindControl("txtLqty")).Text = ((Label)row.FindControl("lbllabQuantity")).Text;
                            }
                            sTotal = ((Label)row.FindControl("lbllabtotal")).Text;
                        }
                        LabourTotal = LabourTotal + Convert.ToDouble(sTotal);
                    }
                    grdLabourMast.FooterRow.Cells[9].Text = Convert.ToString(LabourTotal);
                }



                dtSalvage = obj.dtSalvage;
                grdSalvageMast.DataSource = obj.dtSalvage;
                grdSalvageMast.DataBind();
                double SalvageTotal = 0;
                if (dtSalvage.Rows.Count > 0)
                {
                    foreach (GridViewRow row in grdSalvageMast.Rows)
                    {
                        String sTotal = "0";
                        if (((CheckBox)row.FindControl("chkESalvage")).Checked == true)
                        {
                            if (txtActiontype.Text == "M")
                            {
                                ((TextBox)row.FindControl("txtSqty")).Text = ((Label)row.FindControl("lblSalQuantity")).Text;
                            }
                            sTotal = ((Label)row.FindControl("lblsaltotal")).Text;
                        }
                        SalvageTotal = SalvageTotal + Convert.ToDouble(sTotal);
                    }
                    grdSalvageMast.FooterRow.Cells[9].Text = Convert.ToString(SalvageTotal);
                }




                lblTotalCharges.Text = Convert.ToString(MaterialTotal + LabourTotal - SalvageTotal);


                grdSalvageMast.Columns[9].Visible = false;
                grdMaterialMast.Columns[9].Visible = false;
                grdLabourMast.Columns[9].Visible = false;



                if (obj.dtDocuments.Rows.Count > 0)
                {
                    dtDocumentDetails = obj.dtDocuments;
                    grdDocuments.DataSource = dtDocumentDetails;
                    grdDocuments.DataBind();

                    foreach (GridViewRow row in grdDocuments.Rows)
                    {
                        ((LinkButton)row.FindControl("lnkDelet")).Visible = false;
                        ((LinkButton)row.FindControl("lnkView")).Visible = true;
                    }
                }

                cmbFailType.Enabled = false;
                txtFailureId.Enabled = false;
                cmbRepairer.Enabled = false;
                cmbMaterialType.Enabled = false;
                cmbRateType.Enabled = false;
                cmbGuarenteeType.Enabled = false;
                txtEstDate.Enabled = false;
                cmdAdd.Enabled = false;
                cmbCoreType.Enabled = false;
                cmbInsulationtype.Enabled = false;

                cmbOilType.Enabled = false;
                txtOilValue.Enabled = false;
                cmbStarRating.Enabled = false;


                cmdSave.Text = "View";
                cmdCalc.Visible = false;
                //cmdEst.Enabled = false;
                cmdReset.Enabled = false;
                dvComments.Style.Add("display", "none");

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void txtMqty_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txt.Parent.Parent;
            TextBox txt1 = (TextBox)gvRow.FindControl("txtMqty");
            Label lbltotal = (Label)gvRow.FindControl("lblTotal");
            Label lblBase = (Label)gvRow.FindControl("lblBaserate");
            Label lblTax = (Label)gvRow.FindControl("lbltax");
            double Total = (Convert.ToDouble(txt1.Text) * Convert.ToDouble(lblBase.Text)) + (((Convert.ToDouble(txt1.Text) * Convert.ToDouble(lblBase.Text)) / 100) * 18);
            lbltotal.Text = Convert.ToString(Total);
        }

        protected void txtLqty_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txt.Parent.Parent;
            TextBox txt1 = (TextBox)gvRow.FindControl("txtLqty");
            Label lbltotal = (Label)gvRow.FindControl("lbllabtotal");
            Label lblBase = (Label)gvRow.FindControl("lbllabrate");
            Label lblTax = (Label)gvRow.FindControl("lallabtax");
            double Total = (Convert.ToDouble(txt1.Text) * Convert.ToDouble(lblBase.Text)) + (((Convert.ToDouble(txt1.Text) * Convert.ToDouble(lblBase.Text)) / 100) * 18);
            lbltotal.Text = Convert.ToString(Total);
        }

        protected void txtSqty_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txt.Parent.Parent;
            TextBox txt1 = (TextBox)gvRow.FindControl("txtSqty");
            Label lbltotal = (Label)gvRow.FindControl("lblsaltotal");
            Label lblBase = (Label)gvRow.FindControl("lblsalrate");
            Label lblTax = (Label)gvRow.FindControl("lblsaltax");
            double Total = (Convert.ToDouble(txt1.Text) * Convert.ToDouble(lblBase.Text)) + (((Convert.ToDouble(txt1.Text) * Convert.ToDouble(lblBase.Text)) / 100) * 18);
            lbltotal.Text = Convert.ToString(Total);
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            string[] Arr = new string[2];
            string[] sMateriallist = new string[0];
            string[] sLabourlist = new string[0];
            string[] sSalvageslist = new string[0];
            try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                else
                {
                    clsEstimation obj = new clsEstimation();
                    if (cmdSave.Text == "View")
                    {
                        EstimationReport(hdfEstId.Value);
                    }
                    else
                    {
                        if (ValidateForm() == true)
                        {

                            obj.sFailureId = txtFailureId.Text;
                            obj.sFaultCapacity = txtCapacity.Text;
                            obj.sCore = cmbCoreType.SelectedValue;

                            if (cmbFailType.SelectedValue == "2" && hdfStatusflag.Value == "2" || cmbFailType.SelectedValue == "2" && hdfStatusflag.Value == "4" || cmbFailType.SelectedValue == "2" && hdfStatusflag.Value == "1")
                            {
                                //if (cmbOilType.SelectedValue=="2")
                                //{
                                //    obj.sInsulationtype = "0";
                                //}
                                //else
                                //{
                                //    obj.sInsulationtype = cmbInsulationtype.SelectedValue;
                                //}

                                ///added by rudra on 17-03-2020
                                ///

                                if (cmbOilType.SelectedValue == "2")
                                {
                                    obj.sInsulationtype = "0";
                                    obj.soiltype = cmbOilType.SelectedValue;
                                }
                                else
                                {
                                    obj.sInsulationtype = cmbInsulationtype.SelectedValue;
                                    obj.soiltype = "0";
                                }
                                // obj.sInsulationtype = cmbInsulationtype.SelectedValue;
                                obj.sInsulationtype = cmbInsulationtype.SelectedValue;
                                obj.sstarrating = cmbStarRating.SelectedValue;
                                obj.sStatusFlag = hdfStatusflag.Value;
                                obj.sFailureId = hdfEnhancementId.Value;
                                obj.sWFautoId = hdfWFOAutoId.Value;

                                if (cmbStarRating.SelectedIndex <= 0)
                                {
                                    ShowMsgBox("Please Select Star Rating");
                                    cmbStarRating.Focus();
                                    return;
                                }
                                if (cmbInsulationtype.SelectedIndex <= 0)
                                {
                                    ShowMsgBox("Please Select Insulation Type");
                                    cmbInsulationtype.Focus();
                                    return;
                                }
                                string result = obj.GetItemDetails(obj);

                                if (result == null || result == "")
                                {
                                    if (cmbOilType.SelectedValue != "2")
                                    {
                                        //ShowMsgBox(" selected CoreType, selected InsulationType,  selected StartRasting , Item Rates Not Available ! Please Change CoreType ");
                                        ShowMsgBox(" For selected Capacity, selected InsulationType,  selected Star Rating , Item Rates Not Available ! ");
                                        cmbCoreType.Focus();
                                        return;
                                    }
                                    else
                                    {
                                        ShowMsgBox(" selected Capacity Item Rates Not Available ! ");
                                        cmbOilType.Focus();
                                        return;
                                    }
                                }
                            }

                            //ester oil value
                            if (txtOilValue.Text == null || txtOilValue.Text == string.Empty || txtOilValue.Text == "")
                            {
                                txtOilValue.Text = "0";
                            }
                            else
                            {
                                txtOilValue.Text = txtOilValue.Text;
                            }

                            obj.soiltxtvalue = txtOilValue.Text;
                            double oilvalue = Convert.ToDouble(ConfigurationSettings.AppSettings["EsterOilValue"]);

                            double oiltotal = Convert.ToDouble(obj.soiltxtvalue) * oilvalue;

                            obj.soiltotalvalue = Convert.ToString(oiltotal);


                            obj.soiltype = cmbOilType.SelectedValue;

                            obj.sstarrating = cmbStarRating.SelectedValue;
                            string insulationtype = string.Empty;
                            if (cmbOilType.SelectedValue == "2")
                            {
                                insulationtype = "0";

                            }
                            else
                            {
                                insulationtype = cmbInsulationtype.SelectedValue;
                            }
                            insulationtype = cmbInsulationtype.SelectedValue;
                            if (cmbRepairer.Visible == false)
                            {
                                obj.sLastRepair = "0";
                            }
                            else
                            {
                                obj.sLastRepair = cmbRepairer.SelectedValue;
                            }
                            obj.sGuaranteetype = cmbGuarenteeType.SelectedValue;
                            obj.sOfficeCode = hdfFailureId.Value;
                            obj.sCrby = objSession.UserId;
                            obj.sFailType = cmbFailType.SelectedValue;
                            if (insulationtype == "0")
                            {
                                obj.sInsulationtype = "0";
                            }
                            else
                            {
                                obj.sInsulationtype = cmbInsulationtype.SelectedValue;
                            }
                            obj.sInsulationtype = cmbInsulationtype.SelectedValue;
                            obj.sWFO_id = hdfWFOId.Value;
                            obj.sEstComment = "";
                            hdfAppDesc.Value = obj.sEstComment;
                            obj.sDtrCode = txtTCCode.Text;
                            obj.sActionType = txtActiontype.Text;
                            obj.sEstimationNo = txtEstNo.Text;
                            Session["FailureId"] = txtFailureId.Text;
                            obj.sDtcCode = txtDTCCode.Text;
                            obj.sWoundType = cmbMaterialType.SelectedValue;
                            obj.srateType = cmbRateType.SelectedValue;
                            if (lblTotalCharges.Text == "")
                            {
                                obj.sFinalTotalAmount = "0";
                            }
                            else
                            {
                                obj.sFinalTotalAmount = lblTotalCharges.Text;
                            }

                            obj.sEstDate = txtEstDate.Text;

                            if (cmbFailType.SelectedValue != "2")
                            {
                                int i = 0;
                                sMateriallist = new string[grdMaterialMast.Rows.Count];
                                bool bChecked = false;

                                foreach (GridViewRow row in grdMaterialMast.Rows)
                                {
                                    if (((CheckBox)row.FindControl("chkmaterial")).Checked == true)
                                    {
                                        //if (txtActiontype.Text == "M")
                                        //{
                                        //    ((TextBox)row.FindControl("txtMqty")).Text = ((Label)row.FindControl("lblQuantity")).Text;
                                        //}
                                        sMateriallist[i] = ((Label)row.FindControl("lblMaterialId")).Text.Trim() + "~" + ((TextBox)row.FindControl("txtMqty")).Text.Trim() + "~" +
                                            ((Label)row.FindControl("lblBaserate")).Text.Trim() + "~" + ((Label)row.FindControl("lbltax")).Text.Trim() + "~" +
                                            ((Label)row.FindControl("lblTotal")).Text.Trim() + "~" + ((Label)row.FindControl("lblMatunit")).Text.Trim() + "~" +
                                            ((Label)row.FindControl("lblMaterialName")).Text.Trim().Replace(",", "ç") + "~" + ((Label)row.FindControl("lblMatunitName")).Text.Trim() + "~" + "1 Materials" + "~" + ((Label)row.FindControl("lblMaterialItemId")).Text.Trim();
                                        bChecked = true;
                                    }
                                    i++;
                                }

                                if (!(sFailType == "2"))
                                {
                                    if (bChecked == false && cmbGuarenteeType.SelectedValue != "WGP")
                                    {
                                        ShowMsgBox("Please Select the Material Cost Details");
                                        return;
                                    }
                                }


                                int j = 0;
                                sLabourlist = new string[grdLabourMast.Rows.Count];
                                bChecked = false;
                                foreach (GridViewRow row in grdLabourMast.Rows)
                                {
                                    if (((CheckBox)row.FindControl("chkElabour")).Checked == true)
                                    {
                                        //if (txtActiontype.Text == "M")
                                        //{
                                        //    ((TextBox)row.FindControl("txtLqty")).Text = ((Label)row.FindControl("lbllabQuantity")).Text;
                                        //}
                                        sLabourlist[j] = ((Label)row.FindControl("lblLabourId")).Text.Trim() + "~" + ((TextBox)row.FindControl("txtLqty")).Text.Trim() + "~" +
                                            ((Label)row.FindControl("lbllabrate")).Text.Trim() + "~" + ((Label)row.FindControl("lbllabtax")).Text.Trim() + "~" +
                                            ((Label)row.FindControl("lbllabtotal")).Text.Trim() + "~" + ((Label)row.FindControl("lbllabunit")).Text.Trim() + "~" +
                                            ((Label)row.FindControl("lblLabourName")).Text.Trim().Replace(",", "ç") + "~" + ((Label)row.FindControl("lblLabunitName")).Text.Trim() + "~" + "2 Labour Charges" + "~" + ((Label)row.FindControl("lblLabourItemId")).Text.Trim();
                                        bChecked = true;
                                    }
                                    j++;
                                }

                                if (!(sFailType == "2"))
                                {
                                    if (bChecked == false && cmbGuarenteeType.SelectedValue != "WGP")
                                    {
                                        ShowMsgBox("Please Select the Labour Cost Details");
                                        return;
                                    }
                                }


                                int k = 0;
                                sSalvageslist = new string[grdSalvageMast.Rows.Count];
                                bChecked = false;
                                foreach (GridViewRow row in grdSalvageMast.Rows)
                                {
                                    if (((CheckBox)row.FindControl("chkESalvage")).Checked == true)
                                    {
                                        //if (txtActiontype.Text == "M")
                                        //{
                                        //    ((TextBox)row.FindControl("txtSqty")).Text = ((Label)row.FindControl("lblSalQuantity")).Text;
                                        //}
                                        sSalvageslist[k] = ((Label)row.FindControl("lblSalvageId")).Text.Trim() + "~" + ((TextBox)row.FindControl("txtSqty")).Text.Trim() + "~" +
                                            ((Label)row.FindControl("lblsalrate")).Text.Trim() + "~" + ((Label)row.FindControl("lblsalTax")).Text.Trim() + "~" +
                                            ((Label)row.FindControl("lblsaltotal")).Text.Trim() + "~" + ((Label)row.FindControl("lblsalunit")).Text.Trim() + "~" +
                                            ((Label)row.FindControl("lblSalvageName")).Text.Trim().Replace(",", "ç") + "~" + ((Label)row.FindControl("lblSalunitName")).Text.Trim() + "~" + "3 Salvage" + "~" + ((Label)row.FindControl("lblSalvageItemId")).Text.Trim();
                                        bChecked = true;
                                    }
                                    k++;
                                }
                            }



                            //if (bChecked == false)
                            //{
                            //    ShowMsgBox("Please Select the Salvage Cost Details");
                            //    return;
                            //}

                            if (txtActiontype.Text == "A" || txtActiontype.Text == "R")
                            {
                                if (hdfWFDataId.Value != "0" && hdfWFDataId.Value != "")
                                {
                                    ApproveRejectAction();

                                    if (objSession.sTransactionLog == "1")
                                    {
                                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (Estimation) Failure/Enhancement ");
                                    }

                                    cmdSave.Enabled = false;
                                    cmdCalc.Enabled = false;
                                    // cmdEst.Enabled = false;
                                    hdfWFOId.Value = hdfWFDataId.Value;

                                    //if (objSession.RoleId == "3")
                                    //{

                                    //}
                                    //else
                                    //{
                                    //    txtFailureId.Text = "0";
                                    //}
                                    EstimationReport(txtFailureId.Text);


                                    txtFailureId.Text = Convert.ToString(Session["FailureId"]);
                                    return;
                                }
                            }
                            DataTable dtDocs = new DataTable();
                            #region Modify and Approve

                            // For Modify and Approve
                            if (txtActiontype.Text == "M")
                            {
                                if (txtComment.Text.Trim() == "")
                                {
                                    ShowMsgBox("Enter Comments/Remarks");
                                    txtComment.Focus();
                                    return;

                                }
                                obj.soiltype = cmbOilType.SelectedValue;
                                obj.sFailureId = txtFailureId.Text;
                                obj.sFaultCapacity = txtCapacity.Text;
                                obj.sCore = cmbCoreType.SelectedValue;
                                obj.sInsulationtype = cmbInsulationtype.SelectedValue;
                                //obj.sLastRepair = cmbRepairer.SelectedValue;
                                obj.sOfficeCode = hdfFailureId.Value;
                                obj.sCrby = objSession.UserId;
                                obj.sFailType = cmbFailType.SelectedValue;
                                obj.sWoundType = cmbMaterialType.SelectedValue;
                                obj.srateType = cmbRateType.SelectedValue;
                                obj.sWFO_id = hdfWFOId.Value;
                                obj.sEstComment = "";
                                hdfAppDesc.Value = obj.sEstComment;
                                obj.sDtrCode = txtTCCode.Text;
                                obj.sActionType = txtActiontype.Text;
                                SaveDocumments();
                                dtDocs = (DataTable)ViewState["DOCUMENTS"];
                                obj.dtDocuments = dtDocs;


                                Arr = obj.SaveEstimation(obj, sMateriallist, sLabourlist, sSalvageslist, hdfStatusflag.Value);

                                if (objSession.sTransactionLog == "1")
                                {
                                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (Estimation) Failure/Enhancement ");
                                }

                                if (Arr[1].ToString() == "0")
                                {
                                    hdfWFDataId.Value = obj.sWFDataId;
                                    hdfboid.Value = Arr[3];
                                    ApproveRejectAction();
                                    hdfWFOId.Value = obj.sWFDataId;
                                    //if (txtFailureId.Text != "")
                                    //{
                                    //    EstimationReport(txtFailureId.Text);
                                    //}
                                    EstimationReport(hdfFailureId.Value);
                                    return;
                                }
                                if (Arr[1].ToString() == "2")
                                {
                                    ShowMsgBox(Arr[0]);
                                    return;
                                }
                            }

                            #endregion
                            SaveDocumments();

                            dtDocs = (DataTable)ViewState["DOCUMENTS"];
                            obj.dtDocuments = dtDocs;
                            string sClientIP = string.Empty;

                            string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                            if (!string.IsNullOrEmpty(ip))
                            {
                                string[] ipRange = ip.Split(',');
                                int le = ipRange.Length - 1;
                                sClientIP = ipRange[0];
                            }
                            else
                            {
                                sClientIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                            }

                            obj.sClientIP = sClientIP;

                            //if (obj.sFailType == "2" && hdfStatusflag.Value == "2" || obj.sFailType == "2" && hdfStatusflag.Value == "4")
                            //{

                            //    obj.sInsulationtype = insulationtype;
                            //    obj.sstarrating = cmbStarRating.SelectedValue;
                            //    obj.sStatusFlag = hdfStatusflag.Value;
                            //    obj.sFailureId = hdfEnhancementId.Value;

                            //    string result = obj.GetItemDetails(obj);

                            //    if (result == null || result == "")
                            //    {
                            //        ShowMsgBox(" selected CoreType, selected InsulationType,  selected StartRasting , Item Rates Not Available ! Please Change CoreType ");
                            //        cmbCoreType.Focus();
                            //        return;
                            //    }
                            //}
                            Arr = obj.SaveEstimation(obj, sMateriallist, sLabourlist, sSalvageslist, hdfStatusflag.Value);

                            if (objSession.sTransactionLog == "1")
                            {
                                Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (Estimation) Failure/Enhancement ");
                            }
                            if (Arr[1] == "0")
                            {
                                hdfsavestatus.Value = "1";
                            }
                            hdfWFDataId.Value = Arr[2];
                            hdfFailureId.Value = Arr[3];
                            EstimationReport(hdfFailureId.Value);
                            ShowMsgBox(Arr[0]);
                            hdfsavestatus.Value = "1";
                            cmdSave.Enabled = false;
                            cmdEst.Visible = false;

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ShowMsgBox("Something went wrong while saving, Please approve once again.");
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }



        public void ApproveRejectAction()
        {
            try
            {
                if (txtComment.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Comments/Remarks");
                    txtComment.Focus();
                    return;
                }

                clsApproval objApproval = new clsApproval();
                objApproval.sCrby = objSession.UserId;
                objApproval.sOfficeCode = objSession.OfficeCode;
                objApproval.sWFObjectId = hdfWFOId.Value;
                objApproval.sWFAutoId = hdfWFOAutoId.Value;
                //objApproval.sApproveComments = hdfAppDesc.Value;
                objApproval.sApproveComments = txtComment.Text.Trim();

                if (txtActiontype.Text == "A")
                {
                    objApproval.sApproveStatus = "1";
                }
                if (txtActiontype.Text == "R")
                {
                    objApproval.sApproveStatus = "3";
                }
                if (txtActiontype.Text == "M")
                {
                    objApproval.sApproveStatus = "2";
                    objApproval.sDescription = hdfAppDesc.Value;
                }

                string sClientIP = string.Empty;

                string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(ip))
                {
                    string[] ipRange = ip.Split(',');
                    int le = ipRange.Length - 1;
                    sClientIP = ipRange[0];
                }
                else
                {
                    sClientIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                objApproval.sClientIp = sClientIP;

                bool bResult = false;

                if (txtActiontype.Text == "M")
                {
                    objApproval.sWFDataId = hdfWFDataId.Value;
                    objApproval.sBOId = hdfboid.Value;

                    if (objApproval.sBOId == "45")
                    {
                        objApproval.sFormName = "EstimationCreation";
                    }
                    else
                    {
                        objApproval.sFormName = "EstimationCreation_sdo";
                    }
                    if (hdfRejectApproveRef.Value == "RA")
                    {
                        objApproval.sApproveStatus = "1";
                    }
                    // bResult = objApproval.ModifyApproveWFRequest(objApproval);
                    bResult = objApproval.ModifyApproveWFRequest_Latest(objApproval);

                    if (objApproval.sNewRecordId == null)
                    {
                        hdfFailureId.Value = objApproval.sRecordId;
                    }
                    else
                    {
                        hdfFailureId.Value = objApproval.sNewRecordId;
                    }

                }
                else
                {
                    //  bResult = objApproval.ApproveWFRequest(objApproval);
                    bResult = objApproval.ApproveWFRequest_Latest(objApproval);

                    if (objApproval.sNewRecordId == null)
                    {
                        txtFailureId.Text = objApproval.sRecordId;
                    }
                    else
                    {
                        txtFailureId.Text = objApproval.sNewRecordId; // updating estimation ID
                    }

                }
                if (bResult == true)
                {

                    if (objApproval.sApproveStatus == "1")
                    {
                        ShowMsgBox("Approved Successfully");
                        cmdSave.Enabled = false;
                        //clsWorkOrder objWO = new clsWorkOrder();
                        //objWO.sWFDataId = objApproval.sWFDataId;
                        //objWO.sWFObjectId = objApproval.sWFObjectId;
                        //objWO.sTaskType = txtType.Text;

                        hdfsavestatus.Value = "1";
                        if (txtType.Text == "3")
                        {

                        }
                        else
                        {
                            //GenerateWorkOrderReport(objWO);
                        }
                    }
                    else if (objApproval.sApproveStatus == "3")
                    {

                        ShowMsgBox("Rejected Successfully");
                        cmdSave.Enabled = false;
                        hdfsavestatus.Value = "1";
                    }
                    else if (objApproval.sApproveStatus == "2")
                    {
                        ShowMsgBox("Modified and Approved Successfully");
                        cmdSave.Enabled = false;
                        clsWorkOrder objWo = new clsWorkOrder();
                        objWo.sWFDataId = objApproval.sWFDataId;
                        objWo.sWFObjectId = objApproval.sWFObjectId;
                        objWo.sTaskType = txtType.Text;
                        hdfsavestatus.Value = "1";
                        if (txtType.Text == "3")
                        {
                        }
                        else
                        {
                            //GenerateWorkOrderReport(objWo);
                        }
                    }
                }
                else
                {
                    ShowMsgBox("Selected Record Already Approved");
                    return;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public void EstimationReport(string EstimationId)
        {
            try
            {

                if (EstimationId != null && EstimationId != "")
                {


                    if (cmbFailType.SelectedValue == "2" || sFailType == "2" && (hdfStatusflag.Value == "4" || hdfStatusflag.Value == "2"))
                    {
                        string strParam = string.Empty;

                        string insulationtype = string.Empty;
                        if (cmbOilType.SelectedValue == "2")
                        {
                            insulationtype = "0";

                        }
                        else
                        {
                            insulationtype = cmbInsulationtype.SelectedValue;
                        }
                        insulationtype = cmbInsulationtype.SelectedValue;
                        if (EstimationId.Contains("-"))
                        {
                            if (cmbOilType.SelectedValue == "2")
                            {


                                if (cmbStarRating.SelectedIndex <= 0)
                                {
                                    ShowMsgBox("Please Select Star Rating");
                                    cmbStarRating.Focus();
                                    return;
                                }

                            }

                            clsEstimation objEst = new clsEstimation();
                            if (txtOilValue.Text == null || txtOilValue.Text == "" || txtOilValue.Text == string.Empty)
                            {
                                objEst.soiltxtvalue = "0";
                            }
                            else
                            {
                                objEst.soiltxtvalue = txtOilValue.Text;
                            }
                            double oilvalue = Convert.ToDouble(ConfigurationSettings.AppSettings["EsterOilValue"]);

                            double oiltotal = Convert.ToDouble(objEst.soiltxtvalue) * oilvalue;

                            objEst.soiltotalvalue = Convert.ToString(oiltotal);

                            if (cmbOilType.SelectedValue == "2")
                            {
                                objEst.soiltype = cmbOilType.SelectedValue;
                            }

                            else
                            {
                                objEst.soiltype = "0";

                            }



                            strParam = "id=EnhanceEstimation&EnhanceId=" + hdfEnhancementId.Value + "&sStatus=" + EstimationId + "&FailType=" + cmbFailType.SelectedValue + "&Insulationtype=" + insulationtype + "&oilqnty=" + objEst.soiltxtvalue + "&oiltotal=" + objEst.soiltotalvalue + "&oiltype=" + objEst.soiltype + "&oilprice=" + oilvalue + "&starrating=" + cmbStarRating.SelectedValue + "&statusFlag=" + hdfStatusflag.Value;
                            RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

                        }
                        else
                        {
                            if (cmbOilType.SelectedValue == "2")
                            {


                                if (cmbStarRating.SelectedIndex <= 0)
                                {
                                    ShowMsgBox("Please Select Star Rating");
                                    cmbStarRating.Focus();
                                    return;
                                }

                            }

                            clsEstimation objEst = new clsEstimation();
                            objEst.sCrby = objSession.UserId;
                            objEst.sFailureId = hdfEnhancementId.Value;
                            objEst.sOfficeCode = objSession.OfficeCode;
                            objEst.sEstDate = txtEstDate.Text;
                            objEst.sInsulationtype = cmbInsulationtype.SelectedValue;

                            if (objEst.sInsulationtype == "" || objEst.sInsulationtype == null || objEst.sInsulationtype == "--Select--")
                            {
                                objEst.sInsulationtype = "1";
                            }

                            string insulationtypes = string.Empty;
                            if (cmbOilType.SelectedValue == "2")
                            {
                                insulationtypes = "0";
                                objEst.sInsulationtype = "0";
                            }
                            else
                            {
                                insulationtypes = cmbInsulationtype.SelectedValue;
                                objEst.sInsulationtype = cmbInsulationtype.SelectedValue;
                            }
                            insulationtypes = cmbInsulationtype.SelectedValue;
                            objEst.sInsulationtype = cmbInsulationtype.SelectedValue;
                            if (txtOilValue.Text == "" || txtOilValue.Text == null || txtOilValue.Text == string.Empty)
                            {
                                objEst.soiltxtvalue = "0";
                            }
                            else
                            {
                                objEst.soiltxtvalue = txtOilValue.Text;
                            }
                            double oilvalue = Convert.ToDouble(ConfigurationSettings.AppSettings["EsterOilValue"]);

                            double oiltotal = Convert.ToDouble(objEst.soiltxtvalue) * oilvalue;

                            objEst.soiltotalvalue = Convert.ToString(oiltotal);

                            if (cmbOilType.SelectedValue == "2")
                            {
                                objEst.soiltype = cmbOilType.SelectedValue;
                            }
                            else
                            {
                                objEst.soiltype = "0";
                            }
                            objEst.sstarrating = cmbStarRating.SelectedValue;

                            // objEst.SaveEstimationDetails(objEst);

                            strParam = "id=EnhanceEstimation&EnhanceId=" + hdfEnhancementId.Value + "&sStatus=" + EstimationId + "&FailType=" + cmbFailType.SelectedValue + "&Insulationtype=" + insulationtypes + "&oilqnty=" + objEst.soiltxtvalue + "&oiltotal=" + objEst.soiltotalvalue + "&oiltype=" + objEst.soiltype + "&oilprice=" + oilvalue + "&starrating=" + cmbStarRating.SelectedValue + "&statusFlag=" + hdfStatusflag.Value;
                            RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                        }
                    }
                    else
                    {
                        if (EstimationId.Contains("-") || EstimationId == "0")
                        {
                            string sWFO_ID = hdfWFDataId.Value;
                            string strParam1 = string.Empty;
                            strParam1 = "id=RefinedEstimationSO&sWFOID=" + sWFO_ID + "&sDtrcode=" + txtTCCode.Text + "&FailType=" + cmbFailType.SelectedValue;
                            RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam1 + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                            return;

                        }

                        string strParam = string.Empty;
                        strParam = "id=RefinedEstimation&EstimationId=" + EstimationId;
                        RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        bool ValidateForm()
        {
            bool bValidate = false;
            bool Anex1 = false;
            bool Anex2 = false;
            bool Anex3 = false;
            bool Anex4 = false;
            try
            {
                string sResult = string.Empty;

                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["DOCUMENTS"];

                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (Convert.ToString(dt.Rows[i]["TYPE"]) == "Annexture 1")
                        {
                            Anex1 = true;
                        }
                        if (Convert.ToString(dt.Rows[i]["TYPE"]) == "Annexture 2")
                        {
                            Anex2 = true;
                        }
                        else if (Convert.ToString(dt.Rows[i]["TYPE"]) == "Annexture 3")
                        {
                            Anex3 = true;
                        }
                        else if (Convert.ToString(dt.Rows[i]["TYPE"]) == "Annexture 4")
                        {
                            Anex4 = true;
                        }
                    }
                }

                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count - 1; i++)
                    {
                        for (int j = i + 1; j < dt.Rows.Count; j++)
                        {
                            if (Convert.ToString(dt.Rows[i]["NAME"]) == Convert.ToString(dt.Rows[j]["NAME"]))
                            {
                                ShowMsgBox("File Should not be same , please select the different files to Proced");
                            }
                        }
                    }
                }


                if (cmbFailType.SelectedValue != "2")
                {
                    if (cmbRepairer.SelectedIndex == 0)
                    {
                        if (!(sFailType == "2"))
                        {
                            ShowMsgBox("Please Select the Repairer");
                            cmbRepairer.Focus();
                            return bValidate;
                        }
                    }
                    if (cmbGuarenteeType.SelectedValue == "0")
                    {
                        if (!(sFailType == "2"))
                        {
                            ShowMsgBox("Please Select Guarantee Type");
                            cmbGuarenteeType.Focus();
                            return bValidate;
                        }
                    }
                }
                if (cmbFailType.SelectedValue == "2")
                {
                    if (sFailType != "2")
                    {
                        if (cmbOilType.SelectedValue != "2")
                        {
                            //if (cmbCoreType.SelectedIndex == 0)
                            //{
                            //    ShowMsgBox("Please Select Core Type");
                            //    cmbCoreType.Focus();
                            //    return bValidate;

                            //}
                        }

                        if (cmbOilType.SelectedValue == "2")
                        {


                            if (cmbStarRating.SelectedIndex <= 0)
                            {
                                ShowMsgBox("Please Select Star Rating");
                                cmbStarRating.Focus();
                                return bValidate;
                            }

                        }


                    }


                }
                if (cmbOilType.SelectedValue == "2")
                {


                    if (cmbStarRating.SelectedIndex <= 0)
                    {
                        ShowMsgBox("Please Select Star Rating");
                        cmbStarRating.Focus();
                        return bValidate;
                    }

                }


                if (txtEstDate.Text.TrimEnd() == "")
                {
                    ShowMsgBox("Please Enter Estimation Date");
                    txtEstDate.Focus();
                    return bValidate;
                }

                string sResults = Genaral.DateComparisionTransaction(txtEstDate.Text, txtFailureDate.Text, false, false);
                if (sResults == "1")
                {
                    ShowMsgBox("Estimation Date should be Greater than or Equal to Failure Date");
                    txtEstDate.Focus();
                    return bValidate;
                }

                //if (cmbFailType.SelectedIndex == 0)
                //{
                //    if (!(sFailType == "2"))
                //    {
                //        ShowMsgBox("Please Select the FailureType");
                //        cmbFailType.Focus();
                //        return bValidate;
                //    }
                //}

                //if (cmbMaterialType.SelectedIndex == 0)
                //{
                //    if (!(sFailType == "2"))
                //    {
                //        ShowMsgBox("Please Select Wound Type");
                //        cmbMaterialType.Focus();
                //        return bValidate;
                //    }
                //}


                if (!(sFailType == "2"))
                {
                    if (cmbFailType.SelectedValue == "1")
                    {
                        if ((cmbGuarenteeType.SelectedValue == "WGP" || cmbGuarenteeType.SelectedValue == "WRGP") || (hdfGuarenteeSource.Value != "AGP" && cmbGuarenteeType.SelectedValue != "AGP"))
                        {
                            if (Anex4 == false)
                            {
                                ShowMsgBox("The Transformer failed in Guarantee period, to contimue to estimation upload Annexure-4");
                                return bValidate;
                            }
                            if (Anex2 == true || Anex3 == true)
                            {
                                ShowMsgBox("The Transformer failed in Guarantee period, Annextuxe 2 and  Annextuxe 3 has not mandatory");
                                return bValidate;
                            }
                        }

                        if ((cmbGuarenteeType.SelectedValue == "AGP" && hdfGuarenteeSource.Value == "AGP"))
                        {
                            //if (Anex2 == false)
                            //{
                            //    ShowMsgBox("The Transformer failed in Guarantee period, to contimue to estimation upload Annexure-2");
                            //    return bValidate;
                            //}
                            if (Anex3 == true || Anex4 == true)
                            {
                                ShowMsgBox("The Transformer failed After Guarantee period, Annextuxe 3 and  Annextuxe 4 has not mandatory");
                                return bValidate;
                            }
                        }

                        if ((hdfGuarenteeSource.Value == "WGP" || hdfGuarenteeSource.Value == "WRGP") && cmbGuarenteeType.SelectedValue == "AGP")
                        {
                            if (Anex4 == false)
                            {
                                ShowMsgBox("The Transformer failed in Guarantee period, to contimue to estimation upload Annexure-4");
                                return bValidate;
                            }
                            if (Anex3 == true)
                            {
                                ShowMsgBox("The Transformer failed in Guarantee period, Annextuxe 3 has not mandatory");
                                return bValidate;
                            }
                        }
                    }


                    // commented on 60-11-2018 (for multicoil Estimation is based on SR Based so Commented)
                    //if (cmbFailType.SelectedValue == "2")
                    //{
                    //    if ((cmbGuarenteeType.SelectedValue == "WGP" || cmbGuarenteeType.SelectedValue == "WRGP") || hdfGuarenteeSource.Value != "AGP")
                    //    {
                    //        if (Anex2 == false)
                    //        {
                    //            ShowMsgBox("The Transformer failed in Guarantee period, to contimue to estimation upload Annexure-2");
                    //            return bValidate;
                    //        }
                    //        if (Anex3 == false)
                    //        {
                    //            ShowMsgBox("The Transformer failed in Guarantee period, to contimue to estimation upload Annexure-3");
                    //            return bValidate;
                    //        }
                    //        if (Anex4 == false)
                    //        {
                    //            ShowMsgBox("The Transformer failed in Guarantee period, to contimue to estimation upload Annexure-4");
                    //            return bValidate;
                    //        }
                    //    }

                    //    if ((cmbGuarenteeType.SelectedValue == "AGP" && hdfGuarenteeSource.Value == "AGP"))
                    //    {
                    //        if (Anex2 == false)
                    //        {
                    //            ShowMsgBox("The Transformer failed in After Guarantee period, to contimue to estimation upload Annexure-2");
                    //            return bValidate;
                    //        }
                    //        if (Anex3 == false)
                    //        {
                    //            ShowMsgBox("The Transformer failed in After Guarantee period, to contimue to estimation upload Annexure-3");
                    //            return bValidate;
                    //        }
                    //        if (Anex4 == true)
                    //        {
                    //            ShowMsgBox("The Transformer failed in After Guarantee period, Annextuxe 4 is not mandatory");
                    //            return bValidate;
                    //        }
                    //    }

                    //    if ((hdfGuarenteeSource.Value == "WGP" || hdfGuarenteeSource.Value == "WRGP") && cmbGuarenteeType.SelectedValue == "AGP")
                    //    {
                    //        if (Anex2 == false)
                    //        {
                    //            ShowMsgBox("The Transformer failed in After Guarantee period, to contimue to estimation upload Annexure-2");
                    //            return bValidate;
                    //        }
                    //        if (Anex3 == false)
                    //        {
                    //            ShowMsgBox("The Transformer failed in After Guarantee period, to contimue to estimation upload Annexure-3");
                    //            return bValidate;
                    //        }
                    //        if (Anex4 == false)
                    //        {
                    //            ShowMsgBox("The Transformer failed in After Guarantee period, to contimue to estimation upload Annexure-4");
                    //            return bValidate;
                    //        }
                    //    }
                    //}

                }




                //if (!(sFailType == "2"))
                //{
                //    if (cmbGuarenteeType.SelectedValue == "AGP" && hdfGuarenteeSource.Value == "WGP" && cmbFailType.SelectedValue == "1")
                //    {
                //        if (Anex2 == false)
                //        {
                //            ShowMsgBox("The Transformer failed in Guarantee period, to contimue to estimation upload Annexure-2");
                //            return bValidate;
                //        }
                //        if (Anex4 == false)
                //        {
                //            ShowMsgBox("The Transformer failed in Guarantee period, to contimue to estimation upload Annexure-4");
                //            return bValidate;
                //        }
                //    }

                //    if (cmbGuarenteeType.SelectedValue == "AGP" && hdfGuarenteeSource.Value == "WGP" && cmbFailType.SelectedValue == "2")
                //    {
                //        if (Anex2 == false)
                //        {
                //            ShowMsgBox("The Transformer failed in Guarantee period, to contimue to estimation upload Annexure-2");
                //            return bValidate;
                //        }
                //        if (Anex3 == false)
                //        {
                //            ShowMsgBox("The Transformer failed in Guarantee period, to contimue to estimation upload Annexure-3");
                //            return bValidate;
                //        }
                //        if (Anex4 == false)
                //        {
                //            ShowMsgBox("The Transformer failed in Guarantee period, to contimue to estimation upload Annexure-4");
                //            return bValidate;
                //        }
                //    }

                //    if (cmbGuarenteeType.SelectedValue == "WGP" && hdfGuarenteeSource.Value == "AGP" && cmbFailType.SelectedValue == "1")
                //    {
                //        if (Anex2 == false)
                //        {
                //            ShowMsgBox("The Transformer failed in Guarantee period, to contimue to estimation upload Annexure-2");
                //            return bValidate;
                //        }
                //        if (Anex4 == false)
                //        {
                //            ShowMsgBox("The Transformer failed in Guarantee period, to contimue to estimation upload Annexure-4");
                //            return bValidate;
                //        }
                //    }

                //    if (cmbGuarenteeType.SelectedValue == "AGP" && hdfGuarenteeSource.Value == "AGP" && cmbFailType.SelectedValue == "1")
                //    {
                //        if (Anex2 == false)
                //        {
                //            ShowMsgBox("Please upload Annexure-2 to continue for Estimation");
                //            return bValidate;
                //        }
                //    }

                //    if (cmbGuarenteeType.SelectedValue == "AGP" && hdfGuarenteeSource.Value == "AGP" && cmbFailType.SelectedValue == "2")
                //    {
                //        if (Anex2 == false)
                //        {
                //            ShowMsgBox("Please upload Annexure-2 to continue for Estimation");
                //            return bValidate;
                //        }
                //        if (Anex3 == false)
                //        {
                //            ShowMsgBox("Please upload Annexure-3 to continue for Estimation");
                //            return bValidate;
                //        }
                //    }

                //    if (cmbGuarenteeType.SelectedValue == "WGP" && hdfGuarenteeSource.Value == "AGP" && cmbFailType.SelectedValue == "1")
                //    {
                //        if (Anex2 == false)
                //        {
                //            ShowMsgBox("The Transformer failed in Guarantee period, to contimue to estimation upload Annexure-2");
                //            return bValidate;
                //        }
                //        if (Anex3 == false)
                //        {
                //            ShowMsgBox("The Transformer failed in Guarantee period, to contimue to estimation upload Annexure-3");
                //            return bValidate;
                //        }
                //        if (Anex4 == false)
                //        {
                //            ShowMsgBox("The Transformer failed in Guarantee period, to contimue to estimation upload Annexure-4");
                //            return bValidate;
                //        }

                //    }
                //}




                if (cmbFailType.SelectedValue != "2")
                {
                    if (RetriveFromXML == true && txtActiontype.Text == "A" || RetriveFromXML == true && txtActiontype.Text == "M")
                    {
                        string sQuantity = string.Empty;
                        string sBaseRate = string.Empty;
                        string sTaxRate = string.Empty;
                        string sTotal = string.Empty;

                        foreach (GridViewRow row in grdMaterialMast.Rows)
                        {

                            if (((CheckBox)row.FindControl("chkmaterial")).Checked == true)
                            {
                                sQuantity = ((TextBox)row.FindControl("txtMqty")).Text;

                                if (sQuantity.Length == 0)
                                {
                                    ShowMsgBox("Please Enter Material Quatity");
                                    ((TextBox)row.FindControl("txtMqty")).Focus();
                                    return bValidate;
                                }
                                else if (sQuantity == "0")
                                {
                                    ShowMsgBox("Zero Quantity is not Acceptable");
                                    ((TextBox)row.FindControl("txtMqty")).Focus();
                                    return bValidate;
                                }
                            }
                        }


                        foreach (GridViewRow row1 in grdLabourMast.Rows)
                        {

                            if (((CheckBox)row1.FindControl("chkElabour")).Checked == true)
                            {
                                sQuantity = ((TextBox)row1.FindControl("txtLqty")).Text;

                                if (sQuantity.Length == 0)
                                {
                                    ShowMsgBox("Please Enter Labour Quatity");
                                    ((TextBox)row1.FindControl("txtLqty")).Focus();
                                    return bValidate;
                                }
                                else if (sQuantity == "0")
                                {
                                    ShowMsgBox("Zero Quantity is not Acceptable");
                                    ((TextBox)row1.FindControl("txtLqty")).Focus();
                                    return bValidate;
                                }

                            }
                        }


                        foreach (GridViewRow row2 in grdSalvageMast.Rows)
                        {

                            if (((CheckBox)row2.FindControl("chkESalvage")).Checked == true)
                            {
                                sQuantity = ((TextBox)row2.FindControl("txtSqty")).Text;

                                if (sQuantity.Length == 0)
                                {
                                    ShowMsgBox("Please Enter Labour Quatity");
                                    ((TextBox)row2.FindControl("txtSqty")).Focus();
                                    return bValidate;
                                }
                                else if (sQuantity == "0")
                                {
                                    ShowMsgBox("Zero Quantity is not Acceptable");
                                    ((TextBox)row2.FindControl("txtSqty")).Focus();
                                    return bValidate;
                                }
                            }
                        }
                    }

                    if (RetriveFromXML == false)
                    {
                        string sQuantity = string.Empty;
                        foreach (GridViewRow row in grdMaterialMast.Rows)
                        {
                            sQuantity = ((TextBox)row.FindControl("txtMqty")).Text;
                            if (sQuantity == "0")
                            {
                                ShowMsgBox("Zero Quantity is not Acceptable");
                                ((TextBox)row.FindControl("txtMqty")).Focus();
                                return bValidate;
                            }
                        }
                        foreach (GridViewRow row1 in grdLabourMast.Rows)
                        {
                            sQuantity = ((TextBox)row1.FindControl("txtLqty")).Text;
                            if (sQuantity == "0")
                            {
                                ShowMsgBox("Zero Quantity is not Acceptable");
                                ((TextBox)row1.FindControl("txtLqty")).Focus();
                                return bValidate;
                            }
                            sQuantity = ((TextBox)row1.FindControl("txtLqty")).Text;
                        }
                        foreach (GridViewRow row2 in grdSalvageMast.Rows)
                        {
                            sQuantity = ((TextBox)row2.FindControl("txtSqty")).Text;
                            if (sQuantity == "0")
                            {
                                ShowMsgBox("Zero Quantity is not Acceptable");
                                ((TextBox)row2.FindControl("txtSqty")).Focus();
                                return bValidate;
                            }
                        }
                        if (cmbInsulationtype.SelectedIndex == 0)
                        {
                            ShowMsgBox("Please Select Insulation Type");
                            cmbInsulationtype.Focus();
                            return bValidate;

                        }

                        if (sFailType != "2")
                        {
                            cmdCalc_Click(new object(), new EventArgs());
                        }
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

        protected void grdMaterialMast_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if ((RetriveFromXML == true && (txtActiontype.Text == "A" || txtActiontype.Text == "V" || txtActiontype.Text == "R")) || txtActiontype.Text == "VIEW")
                {
                    foreach (GridViewRow row1 in grdMaterialMast.Rows)
                    {
                        CheckBox chk = ((CheckBox)row1.FindControl("chkmaterial"));
                        chk.Checked = true;
                    }

                    grdMaterialMast.Columns[3].Visible = true;
                    grdMaterialMast.Columns[3].Visible = false;
                    grdMaterialMast.Columns[4].Visible = true;
                    grdMaterialMast.Columns[5].Visible = false;
                }
                if (RetriveFromXML == true && (txtActiontype.Text == "M"))
                {
                    foreach (GridViewRow row1 in grdMaterialMast.Rows)
                    {
                        CheckBox chk = ((CheckBox)row1.FindControl("chkmaterial"));
                        chk.Checked = true;
                    }

                    grdMaterialMast.Columns[3].Visible = false;
                    grdMaterialMast.Columns[3].Visible = true;
                    grdMaterialMast.Columns[4].Visible = true;
                    grdMaterialMast.Columns[5].Visible = false;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void grdLabourMast_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if ((RetriveFromXML == true && (txtActiontype.Text == "A" || txtActiontype.Text == "V" || txtActiontype.Text == "R")) || txtActiontype.Text == "VIEW")
                {
                    foreach (GridViewRow row1 in grdLabourMast.Rows)
                    {
                        CheckBox chk = ((CheckBox)row1.FindControl("chkElabour"));
                        chk.Checked = true;
                    }

                    grdLabourMast.Columns[3].Visible = true;
                    grdLabourMast.Columns[3].Visible = false;
                    grdLabourMast.Columns[4].Visible = true;
                    grdLabourMast.Columns[5].Visible = false;
                }
                if (RetriveFromXML == true && (txtActiontype.Text == "M"))
                {
                    foreach (GridViewRow row1 in grdLabourMast.Rows)
                    {
                        CheckBox chk = ((CheckBox)row1.FindControl("chkElabour"));
                        chk.Checked = true;
                    }

                    grdLabourMast.Columns[3].Visible = false;
                    grdLabourMast.Columns[3].Visible = true;
                    grdLabourMast.Columns[4].Visible = true;
                    grdLabourMast.Columns[5].Visible = false;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdSalvageMast_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if ((RetriveFromXML == true && (txtActiontype.Text == "A" || txtActiontype.Text == "V" || txtActiontype.Text == "R")) || txtActiontype.Text == "VIEW")
                {
                    foreach (GridViewRow row1 in grdSalvageMast.Rows)
                    {
                        CheckBox chk = ((CheckBox)row1.FindControl("chkESalvage"));
                        chk.Checked = true;
                    }

                    grdSalvageMast.Columns[3].Visible = true;
                    grdSalvageMast.Columns[3].Visible = false;
                    grdSalvageMast.Columns[4].Visible = true;
                    grdSalvageMast.Columns[5].Visible = false;
                }
                if (RetriveFromXML == true && (txtActiontype.Text == "M"))
                {
                    foreach (GridViewRow row1 in grdSalvageMast.Rows)
                    {
                        CheckBox chk = ((CheckBox)row1.FindControl("chkESalvage"));
                        chk.Checked = true;
                    }

                    grdSalvageMast.Columns[3].Visible = false;
                    grdSalvageMast.Columns[3].Visible = true;
                    grdSalvageMast.Columns[4].Visible = true;
                    grdSalvageMast.Columns[5].Visible = false;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtComment.Text = string.Empty;
                if (hdfsavestatus.Value == "1")
                {
                    cmdSave.Enabled = false;
                }
                if (cmbFailType.SelectedValue == "2")
                {
                    if (txtActiontype.Text == "M" || hdfWFDataId.Value == "0")
                    {
                        txtEstDate.Text = string.Empty;
                        cmbCoreType.ClearSelection();
                        cmbInsulationtype.ClearSelection();
                        cmbman.ClearSelection();
                        cmbStarRating.ClearSelection();
                        //  cmbOilType.ClearSelection();
                        txtOilValue.Text = string.Empty;
                    }
                }
                else
                {
                    if (txtActiontype.Text == "M" || hdfWFDataId.Value == "0")
                    {
                        txtEstDate.Text = string.Empty;
                        cmbMaterialType.SelectedIndex = 0;
                        cmbRateType.SelectedIndex = 0;
                        cmbRepairer.SelectedIndex = 0;
                        //cmbFailType.SelectedIndex = 0;
                        cmbGuarenteeType.ClearSelection();
                        cmbCoreType.ClearSelection();
                        cmbInsulationtype.ClearSelection();
                    }
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbRepairer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string sWoundType = cmbMaterialType.SelectedValue;
                string srateType = cmbRateType.SelectedValue;

                //if(cmbMaterialType.SelectedValue != "0")
                //{
                if (cmbRepairer.SelectedValue != "--Select--")
                {
                    clsEstimation objEst = new clsEstimation();

                    string offcode = objSession.OfficeCode.Substring(0, 3);
                    bool res = objEst.RepairDateIsValidnew(cmbRepairer.SelectedValue, offcode);

                    if (res == true)
                    {
                        grdMaterialMast.Visible = true;
                        grdLabourMast.Visible = true;
                        grdSalvageMast.Visible = true;
                        LoadMaterialDetails(sWoundType, srateType);
                        LoadLabourDetails(sWoundType, srateType);
                        LoadSalvageDetails(sWoundType, srateType);

                        divQuantityUpload.Visible = true;
                    }
                    else
                    {
                        ShowMsgBox("Effective Date Expired");
                    }
                }
                else
                {
                    grdMaterialMast.Visible = false;
                    grdLabourMast.Visible = false;
                    grdSalvageMast.Visible = false;
                }
                //}
                //else
                //{
                //    ShowMsgBox("Please Select Wound Type");
                //    cmbMaterialType.Focus();
                //    cmbRepairer.SelectedIndex = 0;
                //}

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (Genaral.UrlDecrypt(Request.QueryString["ActionType"]) != "VIEW")
                {
                    Response.Redirect("/Approval/ApprovalInbox.aspx", false);
                }
                else
                {
                    Response.Redirect("EstimationView.aspx", false);
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdCalc_Click(object sender, EventArgs e)
        {
            string sLabelQty = string.Empty;
            double MaterialTotal = 0;
            double LabourTotal = 0;
            double SalvageTotal = 0;

            try
            {
                if (cmbRepairer.SelectedIndex > 0)
                {

                    foreach (GridViewRow row in grdMaterialMast.Rows)
                    {
                        string sQuantity = ((TextBox)row.FindControl("txtMqty")).Text;
                        sLabelQty = ((Label)row.FindControl("lblQuantity")).Text;
                        if (sQuantity == "0")
                        {
                            ShowMsgBox("Zero Quantity is not Acceptable");
                            return;
                        }
                        else if (sQuantity != "")
                        {
                            sLabelQty = sQuantity;
                        }
                        else if (txtActiontype.Text == "M")
                        {
                            if (sQuantity != "")
                            {
                                sLabelQty = sQuantity;
                            }
                        }
                        else if (sLabelQty != "" && sLabelQty != "0")
                        {
                            sQuantity = sLabelQty;
                        }

                        //string sLabelQty = ((Label)row.FindControl("lblQuantity")).Text;
                        Label lbltotal = (Label)row.FindControl("lblTotal");
                        Label lblBase = (Label)row.FindControl("lblBaserate");
                        Label lblTax = (Label)row.FindControl("lbltax");
                        CheckBox chkMaterial = (CheckBox)row.FindControl("chkmaterial");
                        double Total = 0;
                        if (sQuantity.Length > 0 || sLabelQty.Length > 0)
                        {
                            if (sQuantity != "" && sLabelQty != "")
                            {
                                Total = (Convert.ToDouble(sQuantity) * Convert.ToDouble(lblBase.Text)) + (((Convert.ToDouble(sQuantity) * Convert.ToDouble(lblBase.Text)) / 100) * (Convert.ToDouble(lblTax.Text.Replace("%", ""))));
                                lbltotal.Text = Convert.ToString(Math.Round(Total, 2));
                                chkMaterial.Checked = true;
                            }
                            else
                            {
                                lbltotal.Text = Convert.ToString("0.0");
                                chkMaterial.Checked = false;
                            }
                        }
                        else
                        {
                            lbltotal.Text = Convert.ToString("0.0");
                            chkMaterial.Checked = false;
                        }
                        MaterialTotal = MaterialTotal + Total;
                    }

                    grdMaterialMast.FooterRow.Cells[9].Text = Convert.ToString(Math.Round(MaterialTotal, 2));

                    foreach (GridViewRow row in grdLabourMast.Rows)
                    {
                        sLabelQty = string.Empty;
                        string sQuantity = ((TextBox)row.FindControl("txtLqty")).Text;
                        sLabelQty = ((Label)row.FindControl("lbllabQuantity")).Text;
                        if (sQuantity != "")
                        {
                            sLabelQty = sQuantity;
                        }
                        else if (txtActiontype.Text == "M")
                        {
                            if (sQuantity != "")
                            {
                                sLabelQty = sQuantity;
                            }
                        }
                        else if (sLabelQty != "" && sLabelQty != "0")
                        {
                            sQuantity = sLabelQty;
                        }
                        //string sLabelQty = ((Label)row.FindControl("lbllabQuantity")).Text;
                        Label lbltotal = (Label)row.FindControl("lbllabtotal");
                        Label lblBase = (Label)row.FindControl("lbllabrate");
                        Label lblTax = (Label)row.FindControl("lbllabtax");
                        CheckBox chkMaterial = (CheckBox)row.FindControl("chkElabour");
                        double Total = 0;
                        if (sQuantity.Length > 0 || sLabelQty.Length > 0)
                        {
                            if (sQuantity != "" && sLabelQty != "")
                            {
                                Total = (Convert.ToDouble(sQuantity) * Convert.ToDouble(lblBase.Text)) + (((Convert.ToDouble(sQuantity) * Convert.ToDouble(lblBase.Text)) / 100) * (Convert.ToDouble(lblTax.Text.Replace("%", ""))));
                                lbltotal.Text = Convert.ToString(Math.Round(Total, 2));
                                chkMaterial.Checked = true;
                            }
                            else
                            {
                                lbltotal.Text = Convert.ToString("0.0");
                                chkMaterial.Checked = false;
                            }

                        }
                        else
                        {
                            lbltotal.Text = Convert.ToString("0.0");
                            chkMaterial.Checked = false;
                        }

                        LabourTotal = LabourTotal + Total;

                    }

                    grdLabourMast.FooterRow.Cells[9].Text = Convert.ToString(Math.Round(LabourTotal, 2));

                    foreach (GridViewRow row in grdSalvageMast.Rows)
                    {
                        sLabelQty = string.Empty;
                        string sQuantity = ((TextBox)row.FindControl("txtSqty")).Text;
                        sLabelQty = ((Label)row.FindControl("lblSalQuantity")).Text;
                        if (sQuantity != "")
                        {
                            sLabelQty = sQuantity;
                        }
                        else if (txtActiontype.Text == "M")
                        {
                            if (sQuantity != "")
                            {
                                sLabelQty = sQuantity;
                            }
                        }
                        else if (sLabelQty != "" && sLabelQty != "0")
                        {
                            sQuantity = sLabelQty;
                        }
                        //string sLabelQty = ((Label)row.FindControl("lblSalQuantity")).Text;
                        Label lbltotal = (Label)row.FindControl("lblsaltotal");
                        Label lblBase = (Label)row.FindControl("lblsalrate");
                        Label lblTax = (Label)row.FindControl("lblsaltax");
                        CheckBox chkMaterial = (CheckBox)row.FindControl("chkESalvage");
                        double Total = 0;
                        if (sQuantity.Length > 0 || sLabelQty.Length > 0)
                        {
                            if (sQuantity != "" && sLabelQty != "")
                            {
                                Total = (Convert.ToDouble(sQuantity) * Convert.ToDouble(lblBase.Text)) + (((Convert.ToDouble(sQuantity) * Convert.ToDouble(lblBase.Text)) / 100) * (Convert.ToDouble(lblTax.Text.Replace("%", ""))));
                                lbltotal.Text = Convert.ToString(Math.Round(Total, 2));
                                chkMaterial.Checked = true;
                            }
                            else
                            {
                                lbltotal.Text = Convert.ToString("0.0");
                                chkMaterial.Checked = false;
                            }
                        }
                        else
                        {
                            lbltotal.Text = Convert.ToString("0.0");
                            chkMaterial.Checked = false;
                        }

                        SalvageTotal = SalvageTotal + Total;
                    }
                    if (SalvageTotal != 0)
                    {
                        grdSalvageMast.FooterRow.Cells[9].Text = Convert.ToString(Math.Round(SalvageTotal, 2));

                        lblTotalCharges.Text = Convert.ToString(Math.Round(MaterialTotal + LabourTotal - SalvageTotal, 2));
                    }
                    else
                    {
                        lblTotalCharges.Text = Convert.ToString(Math.Round(MaterialTotal + LabourTotal - SalvageTotal, 2));
                    }

                }
                else
                {
                    ShowMsgBox("Plaese select Repairer");
                    cmbRepairer.Focus();
                    return;
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
                string sDTCId = string.Empty;
                if (txtDTCCode.Text.Trim() == "" || txtDTCCode.Text.Trim() == null)
                {
                    txtDTCCode.Focus();
                    ShowMsgBox("Enter the Transformer Centre Code ");
                }
                else
                {
                    sDTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtDtcId.Text));
                    string url = "/MasterForms/DTCCommision.aspx?QryDtcId=" + sDTCId;
                    string s = "window.open('" + url + "', '_blank');";
                    ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
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
                string sTCId = string.Empty;

                if (txtTCCode.Text.Trim() == "" || txtTCCode.Text.Trim() == null)
                {
                    txtTCCode.Focus();
                    ShowMsgBox("Enter DTR Code");
                }
                else
                {
                    sTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtTCId.Text));
                    string url = "/MasterForms/TcMaster.aspx?TCId=" + sTCId;
                    //string s = "window.open('" + url + "', 'popup_window', 'width=300,height=100,left=100,top=100,resizable=yes');";
                    string s = "window.open('" + url + "', '_blank');";
                    ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbMaterialType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbRepairer.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbRateType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbRepairer.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbFileType.SelectedIndex == 0)
                {
                    ShowMsgBox("Please Select the File Type");
                    cmbFailType.Focus();
                    return;
                }

                if (fupAnx.PostedFile.ContentLength == 0)
                {
                    ShowMsgBox("Please Select the File");
                    fupAnx.Focus();
                    return;
                }

                string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FileFormat"]);
                string sAnxFileExt = System.IO.Path.GetExtension(fupAnx.FileName).ToString().ToLower();
                sAnxFileExt = ";" + sAnxFileExt.Remove(0, 1) + ";";

                if (!sFileExt.Contains(sAnxFileExt))
                {
                    ShowMsgBox("Invalid Image Format");
                    return;
                }

                string sFileName = Path.GetFileName(fupAnx.PostedFile.FileName).Replace(",", "");

                fupAnx.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sFileName));
                string sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sFileName);

                DataTable dt = new DataTable("NEWTABLE");

                if (ViewState["DOCUMENTS"] == null)
                {
                    dt.Columns.Add("ID");
                    dt.Columns.Add("NAME");
                    dt.Columns.Add("TYPE");
                    dt.Columns.Add("PATH");
                }
                else
                {
                    grdDocuments.Visible = true;
                    dt = (DataTable)ViewState["DOCUMENTS"];
                }

                int Id = dt.Rows.Count + 1;
                DataRow Row = dt.NewRow();
                Row["ID"] = Id;
                Row["NAME"] = sFileName;
                Row["TYPE"] = cmbFileType.SelectedItem;
                Row["PATH"] = sDirectory;
                //dt.Rows.Add(Row);

                bool exists = dt.Select().ToList().Exists(a => a["NAME"].ToString() == sFileName);

                if (exists)
                {
                    ShowMsgBox("PLEASE SELECT THE DIFFERENT FILE");
                    return;
                }
                else
                {
                    dt.Rows.Add(Row);
                }

                ViewState["DOCUMENTS"] = dt;
                grdDocuments.Visible = true;
                grdDocuments.DataSource = dt;
                grdDocuments.DataBind();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Delete")
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)ViewState["DOCUMENTS"];

                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    Label lblDocId = (Label)row.FindControl("lblId");

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (lblDocId.Text == Convert.ToString(dt.Rows[i]["ID"]))
                        {
                            dt.Rows[i].Delete();
                            dt.AcceptChanges();
                        }
                    }

                    dt.AcceptChanges();

                    if (dt.Rows.Count > 0)
                    {
                        int counter = 0;

                        foreach (DataRow row1 in dt.Rows)
                        {
                            counter++;
                            row1["ID"] = counter;
                        }
                        ViewState["DOCUMENTS"] = dt;
                    }
                    else
                    {
                        ViewState["DOCUMENTS"] = null;
                    }

                    LoadDocs(dt);
                }
                if (e.CommandName == "View")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    Label lblPath = (Label)row.FindControl("lblPath");
                    download(lblPath.Text);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        //private void download(string sFilePath)
        //{
        //    try
        //    {
        //        string sVirtualDirpath = Convert.ToString(ConfigurationSettings.AppSettings["VirtualDirectoryDocs"]);
        //        string fileName = sVirtualDirpath + sFilePath;
        //        System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
        //        response.ClearContent();
        //        response.Clear();
        //        response.ContentType = "text/plain";     //alternatively change to the content-type of your file
        //        response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ";");
        //        WebClient client = new WebClient();
        //        Stream stream = client.OpenRead(fileName);
        //        StreamReader reader = new StreamReader(stream);
        //        String content = reader.ReadToEnd();
        //        response.Write(content);
        //        response.Flush();
        //        response.End();

        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "download");
        //    }


        //}

        private string getFilename(string hreflink)
        {
            Uri uri = new Uri(hreflink);

            string filename = System.IO.Path.GetFileName(uri.LocalPath);

            return filename;
        }

        private void download(string sFilePath)
        {
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
                    string sVirtualDirpath = Convert.ToString(ConfigurationSettings.AppSettings["VirtualDirectoryDocs"]);
                    string url = sVirtualDirpath + sFilePath;
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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadDocs(DataTable dt)
        {
            try
            {
                grdDocuments.DataSource = dt;
                grdDocuments.DataBind();
                grdDocuments.Visible = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void SaveDocumments()
        {
            try
            {
                //FTP Parameter
                string sFTPLink = string.Empty;
                string sFTPUserName = string.Empty;
                string sFTPPassword = string.Empty;
                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);

                clsCommon objComm = new clsCommon();
                DataTable dt = objComm.GetAppSettings();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPMAINLINK")
                    {
                        sFTPLink = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
                    }
                    else if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPUSERNAME")
                    {
                        sFTPUserName = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
                    }
                    else if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPPASSWORD")
                    {
                        sFTPPassword = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
                    }
                }

                DataTable dtDocs = new DataTable();
                dtDocs = (DataTable)ViewState["DOCUMENTS"];

                //clsFtp objFtp = new clsFtp(sFTPLink, sFTPUserName, sFTPPassword);
                clsSFTP objFtp = new clsSFTP(SFTPPath, sFTPUserName, sFTPPassword);

                bool Isuploaded;
                string sMainFolderName = "ESTIMATIONDOCS";



                if (dtDocs != null)
                {
                    for (int i = 0; i < dtDocs.Rows.Count; i++)
                    {
                        string sName = Convert.ToString(dtDocs.Rows[i]["NAME"]);
                        string sPath = Convert.ToString(dtDocs.Rows[i]["PATH"]);

                        if (File.Exists(sPath))
                        {
                            bool IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName + "/" + txtFailureId.Text + "/");
                            if (IsExists == false)
                            {

                                objFtp.createDirectory(SFTPmainfolder + sMainFolderName + "/" + txtFailureId.Text);
                            }

                            Isuploaded = objFtp.upload(SFTPmainfolder + sMainFolderName + "/" + txtFailureId.Text, sName, sPath);
                            if (Isuploaded == true & File.Exists(sPath))
                            {
                                File.Delete(sPath);
                                sPath = SFTPmainfolder + sMainFolderName + "/" + txtFailureId.Text + "/" + sName;
                            }
                        }
                        dtDocs.Rows[i]["PATH"] = sPath;
                    }
                }
                ViewState["DOCUMENTS"] = dtDocs;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdDocuments_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void lnkBudgetstat_Click(object sender, EventArgs e)
        {
            try
            {
                string url = "/MasterForms/BudgetStatus.aspx";
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdViewPGRS_Click(object sender, EventArgs e)
        {
            try
            {

                string strParam = string.Empty;

                strParam = "id=PgrsDocket&FailureId=" + txtFailureId.Text;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

                if (hdfsavestatus.Value == "1")
                {
                    cmdSave.Enabled = false;
                }
                else
                {
                    cmdSave.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbFailType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if (cmbFailType.SelectedValue == "1")
                //{
                //    Response.Write("<script>alert('Probable repairs under minor repair');window.location = 'EstimationCreation.aspx';</script>");


                if (cmbFailType.SelectedValue == "2")
                {
                    cmbFailType.SelectedValue = "2";
                    divmMulticoilHide.Visible = false;
                    cmdCalc.Visible = false;
                    cmdEst.Visible = false;
                    cmdEst.Enabled = true;
                    divLabourCost.Visible = false;
                    divSalvages.Visible = false;
                    divMaterialCost.Visible = false;
                    lblNote.Visible = false;
                    divAnnuFile.Visible = false;
                    cmdViewPGRS.Visible = true;
                    txtcertify.Visible = false;
                    txtcertify.Enabled = false;
                    Lbl.Visible = false;
                    grdDocuments.Visible = false;
                    ViewState["DOCUMENTS"] = null;
                    divCoretypeHide.Visible = false;
                    divInsulationtypeHide.Visible = false;
                    divOiltypeHide.Visible = true;
                    cmbOil_SelectedIndexChanged(sender, e);
                    //divman.Visible = true;
                }
                else
                {
                    divmMulticoilHide.Visible = true;
                    divRepairerFail.Visible = true;
                    cmdCalc.Visible = true;
                    cmdEst.Visible = false;
                    cmdEst.Enabled = false;
                    divLabourCost.Visible = true;
                    divSalvages.Visible = true;
                    divMaterialCost.Visible = true;
                    lblNote.Visible = true;
                    divAnnuFile.Visible = true;
                    cmdViewPGRS.Visible = true;
                    txtcertify.Visible = true;
                    txtcertify.Enabled = true;
                    //txtcheck.Visible = true;
                    Lbl.Visible = true;
                    grdDocuments.Visible = false;
                    ViewState["DOCUMENTS"] = null;
                    divCoretypeHide.Visible = false;
                    divInsulationtypeHide.Visible = false;
                    divOiltypeHide.Visible = false;
                    divman.Visible = false;
                    divtxtoil.Visible = false;
                    // txtOilValue.Text = "0";
                    divstarrating.Visible = false;
                    // divman.Visible = false;
                }


                ////else if (cmbFailType.SelectedValue == "2")
                //    {
                //        cmbFailType.SelectedValue = "2";
                //        divmMulticoilHide.Visible = false;
                //        cmdCalc.Visible = false;
                //        divLabourCost.Visible = false;
                //        divSalvages.Visible = false;
                //        divMaterialCost.Visible = false;
                //        lblNote.Visible = false;
                //        divAnnuFile.Visible = false;
                //        cmdViewPGRS.Visible = false;
                //    }



            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void changefailtype()
        {
            cmbFailType.SelectedValue = "2";
            divmMulticoilHide.Visible = false;
            cmdCalc.Visible = false;
            cmdEst.Visible = false;
            cmdEst.Enabled = true;
            divLabourCost.Visible = false;
            divSalvages.Visible = false;
            divMaterialCost.Visible = false;
            lblNote.Visible = false;
            divAnnuFile.Visible = false;
            cmdViewPGRS.Visible = true;
            txtcertify.Visible = false;
            txtcertify.Enabled = false;
            Lbl.Visible = false;
            grdDocuments.Visible = false;
            ViewState["DOCUMENTS"] = null;
            divCoretypeHide.Visible = false;
            divInsulationtypeHide.Visible = false;
            divOiltypeHide.Visible = true;
            // cmbOil_SelectedIndexChanged(sender, e);
        }

        protected void cmbOil_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {


                if (cmbOilType.SelectedValue == "2")
                {
                    divman.Visible = true;
                    divtxtoil.Visible = true;
                    divstarrating.Visible = true;
                    divCoretypeHide.Visible = false;
                    divInsulationtypeHide.Visible = false;
                    cmbCoreType.ClearSelection();
                    cmbCoreType_SelectedIndexChanged(sender, e);
                    cmbInsulationtype.ClearSelection();
                }
                else
                {
                    divman.Visible = false;
                    divtxtoil.Visible = false;
                    cmbman.ClearSelection();
                    cmbStarRating.ClearSelection();
                    txtOilValue.Text = "";
                    divstarrating.Visible = false;
                    divCoretypeHide.Visible = true;
                    divInsulationtypeHide.Visible = true;
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbCoreType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (divCoretypeHide.Visible == true)
                {
                    divInsulationtypeHide.Visible = true;
                    if (cmbCoreType.SelectedIndex > 0)
                    {
                        //Genaral.Load_Combo("SELECT DISTINCT \"TIT_INSULATION_NAME\" FROM \"TBLTRANSINSULATIONTYPE\" WHERE \"TIT_TT_ID\"='" + cmbCoreType.SelectedValue + "' ", "--Select--", cmbInsulationtype);
                        string qry = "SELECT \"TIT_ID\",\"TIT_INSULATION_NAME\" FROM \"TBLTRANSINSULATIONTYPE\" WHERE \"TIT_TT_ID\"='" + cmbCoreType.SelectedValue + "' and \"TIT_ID\"<>0 ";
                        Genaral.Load_Combo(qry, cmbInsulationtype);
                    }
                }
                else
                {
                    divInsulationtypeHide.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public bool CheckFormCreatorLevel()
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                string sResult = objApproval.GetFormCreatorLevel("", objSession.RoleId, "EstimationCreation");

                if (sResult != "1" && cmbFailType.SelectedValue == "2")
                {
                    //txtcheck.Disabled = true;
                    txtcertify.Visible = false;
                    Lbl.Visible = false;
                }
                else if (sResult == "1")
                {
                    if (txtActiontype.Text != "V")
                    {
                        txtcertify.Checked = false;
                        txtcertify.Enabled = true;
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }
        // cmdDownload_Click hidden by rudra because rate card entry file not using in live
        #region
        //protected void cmdDownload_Click(object sender, EventArgs e)
        //{
        //    if (cmbRepairer.Text != "--Select--")
        //    {
        //        clsEstimation objEstmate = new clsEstimation();

        //        string oficeTrim = objSession.OfficeCode.Substring(0, 3);

        //        string division_code = objEstmate.GetDivId(oficeTrim);

        //        string sMainFolderName = "ESTIMATIONRATES";
        //        clsFtp objFtp = new clsFtp(sFtpServerPath, sUserName, sPassword);
        //        string fileName3 = getFileName(division_code);

        //        try
        //        {
        //            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(sFtpServerPath + "/" + sMainFolderName + "/" + division_code + "/" + cmbRepairer.Text + "/" + fileName3);
        //            request.Method = WebRequestMethods.Ftp.DownloadFile;

        //            //Enter FTP Server credentials.
        //            request.Credentials = new NetworkCredential(sUserName, sPassword);
        //            request.UsePassive = true;
        //            request.UseBinary = true;
        //            request.EnableSsl = false;

        //            //Fetch the Response and read it into a MemoryStream object.
        //            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
        //            using (MemoryStream stream = new MemoryStream())
        //            {
        //                //Download the File.
        //                response.GetResponseStream().CopyTo(stream);
        //                Response.AddHeader("content-disposition", "attachment;filename=" + fileName3);
        //                Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //                Response.BinaryWrite(stream.ToArray());
        //                Response.End();
        //            }
        //        }
        //        catch (WebException ex)
        //        {

        //            ShowMsgBox("Reqired file not Available");

        //        }

        //    }
        //    else
        //    {
        //        ShowMsgBox("please select Repairer !");
        //    }
        //}
        #endregion
        protected void cmdDownload_Click(object sender, EventArgs e)
        {
            try
            {

                DataTable dtDetails = new DataTable();
                dtDetails = (DataTable)ViewState["Material"];
                dtDetails.AcceptChanges();
                dtDetails.Merge((DataTable)ViewState["Labour"]);
                dtDetails.AcceptChanges();
                dtDetails.Merge((DataTable)ViewState["Salvage"]);
                dtDetails.AcceptChanges();
                // dtDetails.Columns[3].DataType = typeof(string);

                DataTable dtCloned = dtDetails.Clone();
                if (dtDetails.Rows.Count > 0)
                {
                    dtCloned.Columns[3].DataType = typeof(string);
                    foreach (DataRow row in dtDetails.Rows)
                    {
                        dtCloned.ImportRow(row);
                    }
                    dtDetails.AcceptChanges();
                }
                else
                {
                    ShowMsgBox("No record found");
                    return;
                }

                for (int i = 0; i <= dtCloned.Rows.Count - 1; i++)
                {
                    dtCloned.Rows[i]["MRI_QUANTITY"] = dtCloned.Rows[i]["MRI_QUANTITY"].ToString().Replace("1", "");
                }
                dtDetails = new DataTable();


                dtDetails = dtCloned;
                dtDetails.AcceptChanges();
                if (dtDetails.Rows.Count > 0)
                {
                    dtDetails.Columns["MRIM_ID"].ColumnName = "MRIM_ID";
                    dtDetails.Columns["MRIM_ITEM_NAME"].ColumnName = "METERIAL NAME";
                    dtDetails.Columns["MRIM_ITEM_ID"].ColumnName = "METERIAL ID";
                    dtDetails.Columns["MRI_QUANTITY"].ColumnName = "QUANTITY";
                    dtDetails.Columns["MD_NAME"].ColumnName = "UNIT";

                    //dtDetails.Columns["MRI_BASE_RATE"].ColumnName = "BASE RATE";
                    //dtDetails.Columns["MRI_TAX"].ColumnName = "TAX RATE";
                    //dtDetails.Columns["MRI_TOTAL"].ColumnName = "TOTAL";
                    // dtDetails.Columns.Add();
                    //dtLoadDetails.Columns["FEEDER NAME"].SetOrdinal(0);

                    List<string> listtoRemove = new List<string> { "MRI_MEASUREMENT", "ESTM_ITEM_QNTY", "MRI_BASE_RATE", "MRI_TAX", "MRI_TOTAL" };
                    string filename = "EstimationDetails" + DateTime.Now + ".xlsx";
                    string pagetitle = " Faulty Estimation View";

                    Genaral.getexcel(dtDetails, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        // coded by rudra for excel upload Quantity upload
        protected void cmdUpload_Click(object sender, EventArgs e)
        {
            try
            {
                string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["UploadFormat"]);
                string sUploadFileExt = System.IO.Path.GetExtension(FtpUpload.FileName).ToString().ToLower();
                sUploadFileExt = ";" + sUploadFileExt.Remove(0, 1) + ";";

                if (!sFileExt.Contains(sUploadFileExt))
                {
                    ShowMsgBox("Invalid File Format");
                    return;
                }
                string excelPath = Server.MapPath("~/DTLMSDocs/") + objSession.UserId + '~' + Path.GetFileName(FtpUpload.PostedFile.FileName);
                FtpUpload.SaveAs(excelPath);
                string conString = string.Empty;
                string FileName = Path.GetFileName(FtpUpload.PostedFile.FileName);
                string extension = Path.GetExtension(FtpUpload.PostedFile.FileName);
                switch (extension)
                {
                    case ".xls": //Excel 97-03
                        conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07 or higher
                        conString = ConfigurationManager.ConnectionStrings["Excel07+ConString"].ConnectionString;
                        break;
                }
                conString = string.Format(conString, excelPath);
                DataTable dtExcelData1 = new DataTable();
                using (OleDbConnection excel_con = new OleDbConnection(conString))
                {
                    try
                    {
                        excel_con.Open();
                        string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();

                        // dtExcelData.Columns.AddRange(new DataColumn[2] { new DataColumn("DT_CODE", typeof(string)),
                        //new DataColumn("DT_KWH_READING", typeof(string)) });

                        using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", excel_con))
                        {
                            oda.Fill(dtExcelData1);

                        }
                        foreach (DataRow dr in dtExcelData1.Rows)
                        {
                            bool blStatus = Regex.IsMatch(Convert.ToString(dr["QUANTITY"]), @"\|~!#$%&/()=?»«@£§€{}-;'<>_,[^a-zA-Z]");

                            if (blStatus == true)
                            {
                                ShowMsgBox(" Excel file Quantity field Should be numbers only....!!!");
                                excel_con.Close();
                                System.IO.File.Delete(excelPath);
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowMsgBox("Please upload correct format file....!!!");
                        excel_con.Close();
                        System.IO.File.Delete(excelPath);
                        return;
                    }
                }
                //DataTable dtGrid = new DataTable();
                //dtGrid = (DataTable)ViewState["Material"];
                //dtGrid.AcceptChanges();
                //dtGrid.Merge((DataTable)ViewState["Labour"]);
                //dtGrid.AcceptChanges();
                //dtGrid.Merge((DataTable)ViewState["Salvage"]);
                //dtGrid.AcceptChanges();

                for (int i = 0; i < dtExcelData1.Rows.Count; i++)
                {
                    foreach (GridViewRow row in grdMaterialMast.Rows)
                    {
                        if (((Label)row.FindControl("lblMaterialId")).Text == Convert.ToString(dtExcelData1.Rows[i]["MRIM_ID"]))
                        {
                            if (Convert.ToString(dtExcelData1.Rows[i]["QUANTITY"]) != "")
                            {
                                ((TextBox)row.FindControl("txtMqty")).Text = Convert.ToString(dtExcelData1.Rows[i]["QUANTITY"]);
                            }
                        }
                    }
                }
                for (int i = 0; i < dtExcelData1.Rows.Count; i++)
                {
                    foreach (GridViewRow row in grdLabourMast.Rows)
                    {
                        if (((Label)row.FindControl("lblLabourId")).Text == Convert.ToString(dtExcelData1.Rows[i]["MRIM_ID"]))
                        {
                            if (Convert.ToString(dtExcelData1.Rows[i]["QUANTITY"]) != "")
                            {
                                ((TextBox)row.FindControl("txtLqty")).Text = Convert.ToString(dtExcelData1.Rows[i]["QUANTITY"]);
                            }
                        }
                    }
                }
                for (int i = 0; i < dtExcelData1.Rows.Count; i++)
                {
                    foreach (GridViewRow row in grdSalvageMast.Rows)
                    {
                        if (((Label)row.FindControl("lblSalvageId")).Text == Convert.ToString(dtExcelData1.Rows[i]["MRIM_ID"]))
                        {
                            if (Convert.ToString(dtExcelData1.Rows[i]["QUANTITY"]) != "")
                            {
                                ((TextBox)row.FindControl("txtSqty")).Text = Convert.ToString(dtExcelData1.Rows[i]["QUANTITY"]);
                            }
                        }
                    }
                }
                dtExcelData1.AcceptChanges();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public string getFileName(string division_code)
        {
            string sMainFolderName = "ESTIMATIONRATES";
            //     clsFtp objFtp = new clsFtp(sFtpServerPath, sUserName, sPassword);
            try
            {
                string fileName3;
                WebRequest request = (WebRequest)WebRequest.Create(sFtpServerPath + "/" + sMainFolderName + "/" + division_code + "/" + cmbRepairer.Text);
                request.Credentials = new NetworkCredential(sUserName, sPassword);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                StreamReader streamReader = new StreamReader(request.GetResponse().GetResponseStream());
                fileName3 = streamReader.ReadLine();
                List<string> directories = new List<string>();
                while (fileName3 != null && fileName3 != "testing12")
                {
                    // dtFiles.Rows.Add(fileName3);
                    directories.Add(fileName3);
                    fileName3 = streamReader.ReadLine();
                }
                streamReader.Close();
                foreach (var fileName in directories)
                {
                    return fileName;
                }
                return fileName3;
            }
            catch (Exception ex)
            {
                ShowMsgBox("sorry for the inconvenience ! This estimation Related file not created yet!   please contact Admin once !!");
                return fileName3;
            }
        }

        protected void cmdEst_Click(object sender, EventArgs e)
        {
            string strParam = string.Empty;
            try
            {
                if (cmbFailType.SelectedValue == "2")
                {
                    //if(cmbOilType.SelectedValue=="0")
                    //{
                    //    ShowMsgBox("Plaese select OilType");
                    //    cmbOilType.Focus();
                    //}
                    string insulationtype = string.Empty;
                    if (cmbOilType.SelectedValue == "2")
                    {
                        insulationtype = "0";
                    }
                    else
                    {
                        insulationtype = cmbInsulationtype.SelectedValue;
                    }
                    insulationtype = cmbInsulationtype.SelectedValue;
                    if (cmbOilType.SelectedValue != "2")
                    {
                        if (cmbCoreType.SelectedIndex > 0 || cmbOilType.SelectedValue == "0")
                        {
                            clsEstimation objEst = new clsEstimation();
                            if (txtOilValue.Text == "" || txtOilValue.Text == null || txtOilValue.Text == string.Empty)
                            {
                                objEst.soiltxtvalue = "0";
                            }
                            else
                            {
                                objEst.soiltxtvalue = txtOilValue.Text;
                            }
                            double oilvalue = Convert.ToDouble(ConfigurationSettings.AppSettings["EsterOilValue"]);

                            double oiltotal = Convert.ToDouble(objEst.soiltxtvalue) * oilvalue;

                            objEst.soiltotalvalue = Convert.ToString(oiltotal);

                            if (cmbOilType.SelectedValue == "2")
                            {
                                objEst.soiltype = "1";

                                if (cmbStarRating.SelectedIndex <= 0)
                                {
                                    ShowMsgBox("Please Select Star Rating");
                                    cmbStarRating.Focus();
                                    return;
                                }
                            }
                            else
                            {
                                objEst.soiltype = "0";
                            }
                            objEst.sInsulationtype = insulationtype;
                            objEst.sstarrating = cmbStarRating.SelectedValue;
                            objEst.sStatusFlag = hdfStatusflag.Value;
                            objEst.sFailureId = hdfEnhancementId.Value;

                            if (cmbStarRating.SelectedIndex <= 0)
                            {
                                ShowMsgBox("Please Select Star Rating");
                                cmbStarRating.Focus();
                                return;
                            }
                            if (cmbInsulationtype.SelectedIndex <= 0)
                            {
                                ShowMsgBox("Please Select Insulation Type");
                                cmbInsulationtype.Focus();
                                return;
                            }
                            string result = objEst.GetItemDetails(objEst);

                            if (result == null || result == "")
                            {
                                //ShowMsgBox(" selected CoreType, selected InsulationType,  selected StartRasting , Item Rates Not Available ! Please Change CoreType ");
                                ShowMsgBox(" For selected Capacity, selected InsulationType,  selected Star Rating , Item Rates Not Available ! ");
                                cmbCoreType.Focus();
                                return;
                            }
                            strParam = "id=EstimationCalc&FailId=" + hdfEnhancementId.Value + "&FailType=" + cmbFailType.SelectedValue + "&Insulationtype=" + insulationtype + "&oilqnty=" + objEst.soiltxtvalue + "&oiltotal=" + objEst.soiltotalvalue + "&oiltype=" + objEst.soiltype + "&oilprice=" + oilvalue + "&starrating=" + cmbStarRating.SelectedValue + "&statusFalg=" + hdfStatusflag.Value;
                            RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                        }
                        else
                        {
                            ShowMsgBox("Plaese select CoreType");
                            cmbRepairer.Focus();
                        }
                    }
                    else
                    {
                        clsEstimation objEst = new clsEstimation();
                        if (txtOilValue.Text == "" || txtOilValue.Text == null || txtOilValue.Text == string.Empty)
                        {
                            objEst.soiltxtvalue = "0";
                        }
                        else
                        {
                            objEst.soiltxtvalue = txtOilValue.Text;
                        }
                        double oilvalue = Convert.ToDouble(ConfigurationSettings.AppSettings["EsterOilValue"]);
                        double oiltotal = Convert.ToDouble(objEst.soiltxtvalue) * oilvalue;
                        objEst.soiltotalvalue = Convert.ToString(oiltotal);
                        if (cmbOilType.SelectedValue == "2")
                        {
                            objEst.soiltype = "1";

                            if (cmbStarRating.SelectedIndex <= 0)
                            {
                                ShowMsgBox("Please Select Star Rating");
                                cmbStarRating.Focus();
                                return;
                            }
                        }
                        else
                        {
                            objEst.soiltype = "0";
                        }
                        objEst.sInsulationtype = insulationtype;
                        objEst.sstarrating = cmbStarRating.SelectedValue;
                        objEst.sStatusFlag = hdfStatusflag.Value;
                        objEst.sFailureId = hdfEnhancementId.Value;

                        string result = objEst.GetItemDetails(objEst);
                        if (cmbStarRating.SelectedIndex <= 0)
                        {
                            ShowMsgBox("Please Select Star Rating");
                            cmbStarRating.Focus();
                            return;
                        }
                        if (cmbInsulationtype.SelectedIndex <= 0)
                        {
                            ShowMsgBox("Please Select Insulation Type");
                            cmbInsulationtype.Focus();
                            return;
                        }
                        if (result == null || result == "")
                        {
                            ShowMsgBox(" selected Capacity Item Rates Not Available !");
                            cmbOilType.Focus();
                            return;
                        }
                        strParam = "id=EstimationCalc&FailId=" + hdfEnhancementId.Value + "&FailType=" + cmbFailType.SelectedValue + "&Insulationtype=" + insulationtype + "&oilqnty=" + objEst.soiltxtvalue + "&oiltotal=" + objEst.soiltotalvalue + "&oiltype=" + objEst.soiltype + "&oilprice=" + oilvalue + "&starrating=" + cmbStarRating.SelectedValue + "&statusFalg=" + hdfStatusflag.Value;
                        RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                    }
                }
                else
                {
                    ShowMsgBox("Please select FailType As Major");
                    cmbRepairer.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbstar_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                clsEstimation objest = new clsEstimation();
                if (hdfStatusflag.Value == "2" || hdfStatusflag.Value == "4")
                {
                    txtOilValue.Text = objest.getoilqnty(txtEnhanceCapcity.Text, cmbStarRating.SelectedValue);
                }
                else
                {
                    txtOilValue.Text = objest.getoilqnty(txtCapacity.Text, cmbStarRating.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public DataTable BindgridView(string FtpServer, string username, string password)
        {
            DataTable dtFiles = new DataTable();
            try
            {
                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);

                string failureid = Regex.Replace(txtFailureId.Text, @"[^0-9a-zA-Z]+", "");
                FtpServer += "FAILUREENTRY/" + failureid;
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
        protected void DownloadFiledwnld(object sender, EventArgs e)
        {
            bool endRequest = false;
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

                    string failureid = Regex.Replace(txtFailureId.Text, @"[^0-9a-zA-Z]+", "");
                    clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);

                    string url = SFTPmainfolder + "FAILUREENTRY/" + failureid + "/" + fileName1;
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
        protected void DownloadFile(object sender, EventArgs e)
        {
            string fileName = (sender as LinkButton).CommandArgument;
            try
            {
                bool status = false;
                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["VirtualDirectoryDocs"]);

                string failureid = Regex.Replace(txtFailureId.Text, @"[^0-9a-zA-Z]+", "");
                clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);
                //status = objFtp.Download(sFileServerPath + "/PO_DOCS/" + PoNo + "/" + fileName, fileName);

                string path = SFTPmainfolder + "FAILUREENTRY/" + failureid + "/" + fileName;
                RegisterStartupScript("Print", "<script>window.open('" + path + "','_blank')</script>");
            }
            catch (WebException ex)
            {
                //  throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void gvFiles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvFiles.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["dt"];
                gvFiles.DataSource = SortDataTable(dt as DataTable, true);
                gvFiles.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "ASC"; }
            set { ViewState["SortDirection"] = value; }

        }
        protected void gvFiles_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = gvFiles.PageIndex;
            DataTable dt = (DataTable)ViewState["dt"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                gvFiles.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                gvFiles.DataSource = dt;
            }
            gvFiles.DataBind();
            gvFiles.PageIndex = pageIndex;
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

                        if (Convert.ToString(dataView.Sort) == "Name ASC ")
                        {
                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("Name")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["dt"] = dataView.ToTable();
                        }
                        else if (Convert.ToString(dataView.Sort) == "Name DESC ")
                        {
                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("Name")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["dt"] = dataView.ToTable();
                        }
                        else
                        {
                            // dataView.Sort = string.Format("{0} {1} ", GridViewSortExpression, GridViewSortDirection);
                            ViewState["dt"] = dataView.ToTable();
                        }
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1} ", GridViewSortExpression, GetSortDirection());

                        if (Convert.ToString(dataView.Sort) == "Name ASC ")
                        {
                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("Name")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["dt"] = dataView.ToTable();
                        }
                        else if (Convert.ToString(dataView.Sort) == "Name DESC ")
                        {
                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("Name")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["dt"] = dataView.ToTable();
                        }
                        else
                        {
                            // dataView.Sort = string.Format("{0} {1} ", GridViewSortExpression, GetSortDirection());
                            ViewState["dt"] = dataView.ToTable(); ;
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
    }
}