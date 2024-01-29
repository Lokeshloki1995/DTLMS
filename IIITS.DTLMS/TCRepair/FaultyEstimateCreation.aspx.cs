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
using ClosedXML.Excel;
using System.Data.OleDb;
using System.Configuration;
using System.Text;
using IIITS.DTLMS.BL.TCRepair;
using System.Text.RegularExpressions;

namespace IIITS.DTLMS.TCRepair
{
    public partial class FaultyEstimateCreation : System.Web.UI.Page
    {
        string strFormCode = "FaultyEstimateCreation";
        clsSession objSession = new clsSession();
        DataTable dtMaterial = new DataTable();
        DataTable dtLabour = new DataTable();
        DataTable dtSalvage = new DataTable();
        DataTable dtoil = new DataTable();
        DataTable dtDocumentDetails = new DataTable();
        bool RetriveFromXML = false;
        string sFailId = string.Empty;
        string sEstId = string.Empty;
        string stccode = string.Empty;
        static string sFailType = string.Empty;

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

                if (!IsPostBack)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("<span style='font-family: Calibri; font-size: 15pt'>* Note:</span><span style='margin-left:13px; position:absolute' > Please Select Repairer to popup rates </span>");
                    sb.AppendLine("<span style='margin-left:70px;' >WGP :WITHIN GUARANTEE PERIOD </span>");
                    sb.AppendLine("<span style='margin-left:70px;' >AGP:AFTER GUARANTEE PERIOD</span>");
                    sb.AppendLine("<span style='margin-left:70px;' >WRGP:WITHIN REPAIRER GUARANTEE PERIOD</span>");

                    sb.AppendLine("<span style='margin-left:70px;' > ANNEXURE-2 : Joint Inspection Report (PHYSICAL)  </span>");
                    sb.AppendLine("<span style='margin-left:70px;' > ANNEXURE-3 : Joint Inspection Report (INTERNAL)  </span>");
                    sb.AppendLine("<span style='margin-left:70px;' > ANNEXURE-4 : Analysis of Failed Distribution Transformer In WGP </span>");

                    lblNote.Text = sb.ToString().Replace(Environment.NewLine, "<br />");

                    CalendarExtender1.EndDate = System.DateTime.Now;
                    if (objSession.RoleId == "2")
                    {
                        Genaral.Load_Combo("SELECT \"TR_ID\", UPPER(\"TR_NAME\") FROM \"TBLTRANSREPAIRER\",\"TBLTRANSREPAIREROFFCODE\"  WHERE \"TR_ID\"=\"TRO_TR_ID\"  AND   " + clsStoreOffice.GetOfficeCode(objSession.OfficeCode, "TRO_OFF_CODE") + " GROUP BY \"TR_ID\"  ORDER BY \"TR_NAME\"", "--Select--", cmbRepairer);
                    }
                    else
                    {
                        if(objSession.RoleId == Convert.ToString(ConfigurationManager.AppSettings["SupAdminRole"]))
                        {
                            string sOfficeCode= string.Empty;
                            if (objSession.OfficeCode.Length > 3)
                            {
                                 sOfficeCode = objSession.OfficeCode.Substring(0, Constants.Division);
                            }
                            else
                            {
                                sOfficeCode = objSession.OfficeCode;
                            }
                            Genaral.Load_Combo("SELECT  \"TR_ID\", UPPER(\"TR_NAME\") FROM \"TBLTRANSREPAIRER\" ,\"TBLTRANSREPAIREROFFCODE\" WHERE \"TR_ID\"=\"TRO_TR_ID\" AND CAST(\"TRO_OFF_CODE\" AS TEXT) like '" + sOfficeCode + "%' ORDER BY \"TR_NAME\" ", "--Select--", cmbRepairer);
                        }
                        else
                        {
                            Genaral.Load_Combo("SELECT \"TR_ID\", UPPER(\"TR_NAME\") FROM \"TBLTRANSREPAIRER\" ,\"TBLTRANSREPAIREROFFCODE\" WHERE \"TR_ID\"=\"TRO_TR_ID\" AND CAST(\"TRO_OFF_CODE\" AS TEXT) = SUBSTR(CAST('" + objSession.OfficeCode + "' AS TEXT),1,'" + Constants.Division + "') ORDER BY \"TR_NAME\" ", "--Select--", cmbRepairer);
                        }
                      //  Genaral.Load_Combo("SELECT \"TR_ID\", UPPER(\"TR_NAME\") FROM \"TBLTRANSREPAIRER\" ,\"TBLTRANSREPAIREROFFCODE\" WHERE \"TR_ID\"=\"TRO_TR_ID\" AND CAST(\"TRO_OFF_CODE\" AS TEXT) = SUBSTR(CAST('" + objSession.OfficeCode + "' AS TEXT),1,'" + Constants.Division + "') ORDER BY \"TR_NAME\" ", "--Select--", cmbRepairer);
                    }


                    if (!(Convert.ToString(Request.QueryString["estId"]) == null))
                    {
                        stccode = Genaral.UrlDecrypt(Convert.ToString(Request.QueryString["TCcode"]));
                        sEstId = Genaral.UrlDecrypt(Convert.ToString(Request.QueryString["RestId"]));
                        txtActiontype.Text = Genaral.UrlDecrypt(Convert.ToString(Request.QueryString["ActionType"]));
                        hdfEstId.Value = sEstId;
                        Session["WFOId"] = "0";
                        LoadTcDetails(stccode);
                        // GetDatafromMainTable(sEstId);
                    }
                    else
                    {
                        if (!(Convert.ToString(Request.QueryString["RestId"]) == null))
                        {
                            stccode = Genaral.UrlDecrypt(Convert.ToString(Request.QueryString["TCcode"]));
                            sFailId = Genaral.UrlDecrypt(Convert.ToString(Request.QueryString["RestId"]));
                            if(objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                            {
                                hdfOfficeCode.Value = Genaral.UrlDecrypt(Convert.ToString(Request.QueryString["OfficeCode"]));
                            }
                           // sOfficeCode = Genaral.UrlDecrypt(Convert.ToString(Request.QueryString["OfficeCode"]));
                            hdfEstId.Value = sFailId;
                            txtActiontype.Text = Genaral.UrlDecrypt(Convert.ToString(Request.QueryString["ActionType"]));
                            for (int i = 0; i < Session.Contents.Count; i++)
                            {
                                var key = Session.Keys[i];
                                var value = Session[i];
                                if (key.ToString() == "WFDataId")
                                {
                                    hdfWFDataId.Value = Convert.ToString(Session["WFDataId"]);
                                }

                            }

                        }

                        if (txtActiontype.Text == "M")
                        {
                            cmdSave.Text = "Modify And Approve";
                        }
                        else if (txtActiontype.Text == "R")
                        {
                            cmdSave.Text = "Reject";
                        }

                        if (txtActiontype.Text == "V")
                        {
                            txtTCCode.Text = stccode;
                            GetFailId(stccode);

                        }
                        else
                        {
                            LoadTcDetails(stccode);
                        }
                        WorkFlowConfig();
                        if (objSession.sRoleType != "4")
                        {
                            Session["BOID"] = "71";
                            ViewState["BOID"] = "71";
                        }
                        else
                        {
                            ViewState["BOID"] = Convert.ToString(Session["BOID"]);
                        }
                    }
                }
                hdfEnhancementId.Value = txtestId.Value;
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
                        }
                        else if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                        {
                            cmdSave.Text = " Modify and Approve";
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
                        }
                        else if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                        {
                            cmdSave.Text = "Approve";
                        }
                    }
                }
            }
        }

        public void LOadEstimateNumber(string officeCode)
        {
            try
            {
                ClsRepairerEstimate objEstmate = new ClsRepairerEstimate();
                officeCode = objSession.OfficeCode;
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
                    ClsRepairerEstimate objEstmate = new ClsRepairerEstimate();
                    sFailId = objEstmate.GetFailId(sFail_id, "View");
                }
                if (sFailId == "")
                {
                    sFailId = txtTCCode.Text;
                }
                LoadTcDetails(sFailId);
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

                        Session["WFDataId"] = null;
                        Session["WFOId"] = null;
                        Session["ApproveStatus"] = null;
                        Session["WFOAutoId"] = null;
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
                        }
                        else
                            cmdCalc.Visible = false;

                    }


                    dvComments.Style.Add("display", "block");

                    if (hdfWFOAutoId.Value != "0")
                    {

                        dvComments.Style.Add("display", "none");
                    }
                    if (txtActiontype.Text == "V")
                    {
                        cmdSave.Text = "View";
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

        public void LoadTcDetails(string stccode)
        {
            try
            {
                ClsRepairerEstimate objrepair = new ClsRepairerEstimate();

                objrepair.sDtrCode = stccode;

                objrepair.GetTcDetails(objrepair);

                txtestId.Value = sFailId;


                //txtDeclaredBy.Text = objrepair.sCrby;
                txtTCCode.Text = objrepair.sDtrCode;
                txtCapacity.Text = objrepair.sDtcCapacity;


                hdfFailureId.Value = objrepair.sOfficeCode;

                for (int i = 0; i < Session.Contents.Count; i++)
                {
                    var key = Session.Keys[i];
                    var value = Session[i];
                    if (key == "WFOId")
                    {
                        hdfWFOId.Value = Convert.ToString(Session["WFOId"]);
                    }

                }

                LOadEstimateNumber(objrepair.sOfficeCode);

                txtTCId.Text = objrepair.sTCId;

                if (objrepair.sStatus_flag == "4")
                {

                }
                else if (objrepair.sStatus_flag == "2")
                {
                    sFailType = "2";

                    cmbRepairer.Visible = false;
                    divRepairerFail.Visible = false;
                    rybPhase.Visible = false;
                    cmdCalc.Visible = false;
                    divLabourCost.Visible = false;
                    divSalvages.Visible = false;
                    divMaterialCost.Visible = false;
                    Oilcost.Visible = false;
                    lblNote.Visible = false;
                    divAnnuFile.Visible = false;


                    if (txtActiontype.Text == "V")
                    {
                        cmdSave.Text = "view";
                        dvComments.Visible = false;
                        cmdCalc.Visible = false;
                    }
                }
                ViewState["BOID"] = "71";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadMaterialDetails(string sWoundType,string srateType)
        {
            try
            {
                DataTable dt = new DataTable();
                ClsRepairerEstimate objEstimate = new ClsRepairerEstimate();
                objEstimate.sLastRepair = cmbRepairer.SelectedValue;
                objEstimate.scapacity = txtCapacity.Text;
                objEstimate.sWoundType = sWoundType;
                objEstimate.srateType = srateType;
                if(objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                {
                    objEstimate.sOfficeCode = hdfOfficeCode.Value.Substring(0, 3);
                }
                else
                {
                    objEstimate.sOfficeCode = objSession.OfficeCode.Substring(0, 3);
                }
               // objEstimate.sOfficeCode = objSession.OfficeCode.Substring(0, 3);
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
                ClsRepairerEstimate objEstimate = new ClsRepairerEstimate();
                objEstimate.sLastRepair = cmbRepairer.SelectedValue;
                objEstimate.scapacity = txtCapacity.Text;
                objEstimate.sWoundType = sWoundType;
                objEstimate.srateType = srateType;
                if(objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                {
                    objEstimate.sOfficeCode = hdfOfficeCode.Value.Substring(0, 3);
                }
                else
                {
                    objEstimate.sOfficeCode = objSession.OfficeCode.Substring(0, 3);
                }
             //   objEstimate.sOfficeCode = objSession.OfficeCode.Substring(0, 3);
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
                ClsRepairerEstimate objEstimate = new ClsRepairerEstimate();
                objEstimate.sLastRepair = cmbRepairer.SelectedValue;
                objEstimate.scapacity = txtCapacity.Text;
                objEstimate.sWoundType = sWoundType;
                objEstimate.srateType = srateType;
                if(objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                {
                    objEstimate.sOfficeCode = hdfOfficeCode.Value.Substring(0, 3);
                }
                else
                {
                    objEstimate.sOfficeCode = objSession.OfficeCode.Substring(0, 3);
                }
              //  objEstimate.sOfficeCode = objSession.OfficeCode.Substring(0, 3);
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


        public void LoadOilDetails()
        {
            try
            {
                DataTable dt = new DataTable();
                string kavika = Convert.ToString(ConfigurationSettings.AppSettings["KAVIKA_NEW"]);
                ClsRepairerEstimate objEstimate = new ClsRepairerEstimate();
                objEstimate.sDtcTcCode = txtTCCode.Text;
                dt = objEstimate.LoadExistOil(objEstimate);
                if (cmbRepairer.SelectedValue== kavika)
                {

                    hdfkavika.Value = "1";
                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = dt.Rows[i];
                        if (dr["MRIM_ITEM_NAME"].Equals("BAD OIL (Reflects 80% from Existing DTR Entry page)"))
                            dr.Delete();
                    }
                    dt.AcceptChanges();
                }       
                else
                {
                    hdfkavika.Value = "";
                } 
                ViewState["Oil"] = dt;
                GridOil.DataSource = dt;
                GridOil.DataBind();


                if (GridOil.Rows.Count > 0 && GridOil.Rows.Count > 2)
                {
                    GridOil.Rows[dt.Rows.Count-1].Cells[3].Text = Convert.ToString(dt.Rows[dt.Rows.Count-1]["RESTM_ITEM_QNTY"]);

                    //GridOil.Rows[2].Cells[3].Text =Convert.ToString(dt.Rows[2][9]);
                    
                }

               



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
                if (Session["WFOId"] != null && Convert.ToString(Session["WFOId"]) != "")
                {
                    hdfWFOId.Value = Convert.ToString(Session["WFOId"]);
                }


                string FailType = "";


                if (txtActiontype.Text == "V")
                {
                    if (Convert.ToString(FailType.Split('~').GetValue(0)) == "2")
                    {
                        objFailure.sFailureId = Convert.ToString(FailType.Split('~').GetValue(1));
                    }
                    objFailure.sFailureId = txtestId.Value;
                }
                else
                {
                    sWo_Id = hdfWFOId.Value;
                    dtEstDetails = objFailure.getFailId(sWo_Id);
                    txtestId.Value = Convert.ToString(dtEstDetails.Rows[0]["WO_DATA_ID"]);
                    // objFailure.sFailureId = txtestId.Value;
                }
                // objFailure.GetFailureDetails(objFailure);

                //txtFailureDate.Text = objFailure.sFailureDate;
                //txtDTCName.Text = objFailure.sDtcName;
                // txtDTCCode.Text = objFailure.sDtcCode;
                // txtDeclaredBy.Text = objFailure.sCrby;
                //txtTCCode.Text = objFailure.sDtcTcCode;
                //txtCapacity.Text = objFailure.sDtcCapacity;
                //cmbRepairer.SelectedValue = objFailure.sLastRepairedBy;
                //hdfFailureId.Value = objFailure.sOfficeCode;
                //cmbGuarenteeType.SelectedValue = objFailure.sGuarantyType;

                // hdfGuarenteeSource.Value = objFailure.sGuarantyType;
                LOadEstimateNumber(objFailure.sOfficeCode);

                //txtTCId.Text = objFailure.sTCId;

                if (hdfStatusflag.Value == null || hdfStatusflag.Value == "")
                {
                    hdfStatusflag.Value = Convert.ToString(FailType.Split('~').GetValue(0));
                }

                if (objFailure.sStatus_flag == "2")
                {

                    sFailType = "2";
                    hdfStatusflag.Value = "2";
                    cmbFailcoilType.Visible = false;
                    rybPhase.Visible = false;
                    cmbRepairer.Visible = false;
                    divRepairerFail.Visible = false;
                   // rybPhase.Visible = false;
                    cmdCalc.Visible = false;
                    divLabourCost.Visible = false;
                    divSalvages.Visible = false;
                    divMaterialCost.Visible = false;
                    Oilcost.Visible = false;
                    lblNote.Visible = false;
                    divAnnuFile.Visible = false;

                    if (txtActiontype.Text == "V")
                    {
                        cmdSave.Text = "view";
                        dvComments.Visible = false;
                        cmdCalc.Visible = false;
                    }
                }
                ClsRepairerEstimate obj = new ClsRepairerEstimate();
                if (txtActiontype.Text == "V" || txtActiontype.Text == "M" || txtActiontype.Text == "R")
                {
                    obj.sWFO_id = hdfWFDataId.Value;
                }
                else
                {
                    obj.sWFO_id = Convert.ToString(dtEstDetails.Rows[0]["WO_WFO_ID"]);
                }

                if (sWo_Id != null && sWo_Id != "")
                {
                    hdfWFOId.Value = sWo_Id;
                    //hdfWFDataId.Value = sWo_Id;
                }

                if (sApproveStatus != "3" && (Convert.ToString(FailType.Split('~').GetValue(0)) != "2"))
                {
                    obj.GetEstimateDetailsFromXML(obj);
                    txtTCCode.Text = obj.sDtrCode;
                    cmbFailcoilType.SelectedValue = obj.coiltype;
                    string strvalue = obj.sPhases.Replace("`",",");

                    string[] stringArray = strvalue.Split(',');

               
                    //char[] ch = strvalue.ToCharArray();
                    //int n = ch.Length;
                    foreach (string str in stringArray)
                    {
                        //rybPhase.SelectedValue = str;

                        if (str == "1")
                        {
                            rybPhase.Items[0].Selected = true;
                        }
                        if (str == "2")
                        {
                            rybPhase.Items[1].Selected = true;
                        }
                        if (str == "3")
                        {
                            rybPhase.Items[2].Selected = true;
                        }
                        
                    }
                    // char x = '1', y = '2', z = '3';


                    //for (int q = 0; q < n; q++)

                    //{
                    //    rybPhase.SelectedValue =rybPhase.SelectedValue + ch[q].ToString();
                    // bool isTrue = false;
                    //rybPhase.SelectedValue = "";
                    //if (ch[q] == x)
                    //{


                    // rybPhase.SelectedValue = ch[q].ToString();
                    // //rPhase.Value = ch[q].ToString();


                    //}
                    ////rybPhase.SelectedValue = null;
                    //else if (ch[q] == y)
                    //{

                    //rybPhase.SelectedValue = ch[q].ToString();
                    ////yPhase.Value = ch[q].ToString();

                    //}
                    //else if (ch[q] == z)
                    //{
                    //   // bPhase.Value = ch[q].ToString();
                    //rybPhase.SelectedValue = ch[q].ToString();

                    //}

                    //}
                    //rybPhase.DataSource = obj.sPhases;

                    //rybPhase.DataBind();

                    string kavika = Convert.ToString(ConfigurationSettings.AppSettings["KAVIKA_NEW"]);

                    if(kavika.Equals(obj.sLastRepair))
                    {
                        hdfkavika.Value = "1";
                    }

                    cmbRepairer.SelectedValue = obj.sLastRepair;
                    cmbMaterialType.SelectedValue = obj.sWoundType;
                    cmbRateType.SelectedValue = obj.srateType;
                    cmbGuarenteeType.SelectedValue = obj.sGuaranteetype;
                    txtEstDate.Text = obj.sEstDate;
                    txtEstNo.Text = obj.sEstimationNo;
                    RetriveFromXML = true;
                    dtMaterial = obj.dtMaterial;
                    grdMaterialMast.DataSource = obj.dtMaterial;
                    grdMaterialMast.DataBind();

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


                    dtoil = obj.dtOil;
                    ViewState["Oil"]=dtoil;
                    if (dtoil!=null) { 
                    GridOil.DataSource = obj.dtOil;
                    GridOil.DataBind();
                    }
                    double OilTotal = 0;

                   
                  


                    if (GridOil.Rows.Count > 0)
                    {
                        foreach (GridViewRow row in GridOil.Rows)
                        {
                            String oTotal = "";
                            //for minus from bad oil
                           string sbadoil = ((Label)row.FindControl("lblOilName")).Text;
                            if (((CheckBox)row.FindControl("chkEOil")).Checked == true)
                            {
                                if (txtActiontype.Text == "M")
                                {
                                    ((TextBox)row.FindControl("txtOqty")).Text = ((Label)row.FindControl("lblOilQuantity")).Text;
                                }
                                else
                                {
                                    ((Label)row.FindControl("lblOilQuantity")).Text = ((Label)row.FindControl("lblOilQuantity")).Text;
                                }
                                oTotal = ((Label)row.FindControl("lblOiltotal")).Text;
                            }
                            

                            if (sbadoil.Contains("BAD OIL"))
                            {
                                OilTotal = OilTotal-Convert.ToDouble(oTotal) ;
                            }
                            else if (hdfkavika.Value=="1" && sbadoil.Contains("RECLAIMED OIL"))
                            {
                                OilTotal = OilTotal - Convert.ToDouble(oTotal);
                            }
                            else
                            {
                                OilTotal = OilTotal + Convert.ToDouble(oTotal);
                            }
                            //OilTotal = OilTotal + Convert.ToDouble(oTotal);
                        }

                        GridOil.FooterRow.Cells[9].Text = Convert.ToString(OilTotal);
                        lbloilfinaltotal.Text = GridOil.FooterRow.Cells[9].Text;
                    }



                    lblTotalCharges.Text = Convert.ToString(MaterialTotal + LabourTotal + OilTotal - SalvageTotal);
                    lblTotalAmount.Text = Convert.ToString(MaterialTotal + LabourTotal - SalvageTotal);

                    dtDocumentDetails = obj.dtDocuments;
                    grdDocuments.DataSource = dtDocumentDetails;
                    grdDocuments.DataBind();
                    ViewState["DOCUMENTS"] = dtDocumentDetails;

                    foreach (GridViewRow row in grdDocuments.Rows)
                    {
                        ((LinkButton)row.FindControl("lnkDelet")).Visible = false;
                        ((LinkButton)row.FindControl("lnkView")).Visible = true;
                    }

                    // txtFailureId.Enabled = false;
                    cmbFailcoilType.Enabled = false;
                    rybPhase.Enabled = false;
                    cmbRepairer.Enabled = false;
                    cmbMaterialType.Enabled = false;
                    cmbRateType.Enabled = false;
                    cmbGuarenteeType.Enabled = false;
                    txtEstDate.Enabled = false;
                    cmdAdd.Enabled = false;

                }
                else if ((Convert.ToString(FailType.Split('~').GetValue(0)) == "2"))
                {
                    obj.GetEstimateDetailsFromXML(obj);
                    txtEstDate.Text = obj.sEstDate;
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
                ClsRepairerEstimate obj = new ClsRepairerEstimate();
                obj.sEstimationId = sEstId;
                obj.GetDetailsfromMainDB(obj);

                cmbFailcoilType.SelectedValue = obj.coiltype;
                cmbRepairer.SelectedValue = obj.sLastRepair;
                cmbMaterialType.SelectedValue = obj.sWoundType;
                cmbRateType.SelectedValue = obj.srateType;
                dtMaterial = obj.dtMaterial;
                grdMaterialMast.DataSource = obj.dtMaterial;
                grdMaterialMast.DataBind();



                double MaterialTotal = 0;
                if (dtMaterial.Rows.Count > 0)
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
                if (dtLabour.Rows.Count > 0)
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
                if (dtSalvage.Rows.Count > 0)
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


                dtoil = obj.dtOil;
                GridOil.DataSource = obj.dtOil;
                GridOil.DataBind();
                double OilTotal = 0;
                if (dtoil.Rows.Count > 0)
                {
                    foreach (GridViewRow row in GridOil.Rows)
                    {
                        String OTotal = "";
                        string sbadoil = ((Label)row.FindControl("lblOilName")).Text;
                        if (((CheckBox)row.FindControl("chkEOil")).Checked == true)
                        {
                            if (txtActiontype.Text == "M")
                            {
                                ((TextBox)row.FindControl("txtOqty")).Text = ((Label)row.FindControl("lblOilQuantity")).Text;
                            }
                            OTotal = ((Label)row.FindControl("lblOiltotal")).Text;
                        }

                        if (sbadoil.Contains("BAD OIL"))
                        {
                            OilTotal = OilTotal- Convert.ToDouble(OTotal);
                        }
                        else
                        {
                            OilTotal = OilTotal + Convert.ToDouble(OTotal);
                        }
                        
                    }
                    GridOil.FooterRow.Cells[9].Text = Convert.ToString(OilTotal);
                    lbloilfinaltotal.Text = GridOil.FooterRow.Cells[9].Text;
                }





                lblTotalAmount.Text = Convert.ToString(MaterialTotal + LabourTotal - SalvageTotal);
                lblTotalCharges.Text = Convert.ToString(MaterialTotal + LabourTotal + OilTotal - SalvageTotal);
     

                grdSalvageMast.Columns[9].Visible = false;
                grdMaterialMast.Columns[9].Visible = false;
                grdLabourMast.Columns[9].Visible = false;
                GridOil.Columns[9].Visible = false;



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


                //txtFailureId.Enabled = false;
                cmbRepairer.Enabled = false;
                cmbMaterialType.Enabled = false;
                cmbRateType.Enabled = false;
                cmbGuarenteeType.Enabled = false;
                txtEstDate.Enabled = false;
                cmdAdd.Enabled = false;
                cmdSave.Text = "View";
                cmdCalc.Visible = false;
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


        protected void txtOqty_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txt.Parent.Parent;
            TextBox txt1 = (TextBox)gvRow.FindControl("txtOqty");
            Label lbltotal = (Label)gvRow.FindControl("lblOilTotal");
            Label lblBase = (Label)gvRow.FindControl("lblOilrate");
            Label lblTax = (Label)gvRow.FindControl("lblOiltax");
            double Total = (Convert.ToDouble(txt1.Text) * Convert.ToDouble(lblBase.Text)); //base rate is the calculated amount    + (((Convert.ToDouble(txt1.Text) * Convert.ToDouble(lblBase.Text)) / 100) * 18);
            lbltotal.Text = Convert.ToString(Total);
        }
        protected void cmbFailType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt1 = new DataTable();
                dt1 = (DataTable)ViewState["Oil"];
                if (dt1 != null && dt1.Rows.Count>2)
                {
                    GridOil.Rows[dt1.Rows.Count - 1].Cells[3].Text = Convert.ToString(dt1.Rows[dt1.Rows.Count - 1]["RESTM_ITEM_QNTY"]);

                  //  GridOil.Rows[2].Cells[3].Text = Convert.ToString(dt1.Rows[2][9]);
                }
                

                if (cmbFailcoilType.SelectedValue == "1")
                {
                    rybPhase.ClearSelection();

                    ShowMsgBox(" For Single Coil Please Check Any one Phase Checkbox ");
                   
                }
                else if (cmbFailcoilType.SelectedValue == "2")
                {
                    ShowMsgBox(" For Multi Coil Please Check More Than One Phase Checkbox");
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void rybPhase_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbFailcoilType.SelectedValue == "1")
                {
                    //ShowMsgBox(" For Single Coli Please Check Any one Checkbox");
                    for (int i = 0; i < rybPhase.Items.Count; i++)
                    {
                        rybPhase.Items[i].Attributes.Add("onclick", "MutExChkList(this)");
                    }
                }

                DataTable dt1 = new DataTable();
                dt1 = (DataTable)ViewState["Oil"];
                if (dt1 != null && dt1.Rows.Count>2)
                {
                    GridOil.Rows[dt1.Rows.Count - 1].Cells[3].Text = Convert.ToString(dt1.Rows[dt1.Rows.Count - 1]["RESTM_ITEM_QNTY"]);

                }
                //else if (cmbFailcoilType.SelectedValue == "2")
                //{
                //    ShowMsgBox(" For Multi Coli Please Check More Than One Checkbox");
                //}

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            string[] Arr = new string[2];
            string[] sMateriallist = new string[0];
            string[] sLabourlist = new string[0];
            string[] sSalvageslist = new string[0];
            string[] sOillist = new string[0];

            try
            {

                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }

                ClsRepairerEstimate obj = new ClsRepairerEstimate();
                if (cmdSave.Text == "View")
                {
                    EstimationReport(hdfEstId.Value);
                }
                else
                {
                    if (ValidateForm() == true)
                    {

                        obj.sEstid = txtestId.Value;
                        obj.sFaultCapacity = txtCapacity.Text;
                        if (cmbRepairer.Visible == false)
                        {
                            obj.sLastRepair = "0";
                        }
                        else
                        {
                            obj.sLastRepair = cmbRepairer.SelectedValue;
                        }
                        obj.coiltype = cmbFailcoilType.SelectedValue;
                        int count = 0;
                        string strvalue = string.Empty;
                        foreach (ListItem lst in rybPhase.Items)
                        {
                            if (lst.Selected == true)
                            {
                                count++;
                                strvalue += lst.Value + ',';
                            }
                        }
                        if (cmbFailcoilType.SelectedValue == "1")
                        {
                            if (count == 1)
                            {
                                obj.sPhase = strvalue.Remove(strvalue.Length - 1, 1);
                            }
                            else
                            {
                                ShowMsgBox(" For Single Coil Please Check Any One Phase Checkbox");
                                rybPhase.Focus();
                                return;
                            }
                        }
                        else if (cmbFailcoilType.SelectedValue == "2")
                        {
                            if (count == 1 || count == 0)
                            {
                                ShowMsgBox(" For Multi Coil Please Check More Than One Phase Checkbox");
                                rybPhase.Focus();
                                return;
                            }
                            else
                            {
                                obj.sPhase = strvalue.Remove(strvalue.Length - 1, 1);
                            }
                        }
                        obj.sGuaranteetype = cmbGuarenteeType.SelectedValue;
                        obj.sOfficeCode = objSession.OfficeCode;
                        obj.sCrby = objSession.UserId;
                        obj.sWFO_id = hdfWFOId.Value;
                        obj.sEstComment = "";
                        hdfAppDesc.Value = obj.sEstComment;
                        obj.sDtrCode = txtTCCode.Text;
                        obj.sActionType = txtActiontype.Text;
                        Session["FailureId"] = txtestId.Value;
                        // obj.sDtcCode = txtDTCCode.Text;
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
                        if (lbloilfinaltotal.Text == "")
                        {
                            obj.soilfinaltotalamnt = "0";
                        }
                        else
                        {
                            obj.soilfinaltotalamnt = lbloilfinaltotal.Text;
                        }
                        obj.sEstDate = txtEstDate.Text;
                        int i = 0;
                        sMateriallist = new string[grdMaterialMast.Rows.Count];
                        bool bChecked = false;
                        foreach (GridViewRow row in grdMaterialMast.Rows)
                        {
                            if (((CheckBox)row.FindControl("chkmaterial")).Checked == true)
                            {
                                sMateriallist[i] = ((Label)row.FindControl("lblMaterialId")).Text.Trim() + "~" +
                                    ((TextBox)row.FindControl("txtMqty")).Text.Trim() + "~" +
                                    ((Label)row.FindControl("lblBaserate")).Text.Trim() + "~" +
                                    ((Label)row.FindControl("lbltax")).Text.Trim() + "~" +
                                    ((Label)row.FindControl("lblTotal")).Text.Trim() + "~" + 
                                    ((Label)row.FindControl("lblMatunit")).Text.Trim() + "~" +
                                    ((Label)row.FindControl("lblMaterialName")).Text.Trim().Replace(",", "ç") + "~" + 
                                    ((Label)row.FindControl("lblMatunitName")).Text.Trim() + "~" + "1 Materials" + "~" + 
                                    ((Label)row.FindControl("lblMaterialItemId")).Text.Trim();
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
                                sSalvageslist[k] = ((Label)row.FindControl("lblSalvageId")).Text.Trim() + "~" + 
                                    ((TextBox)row.FindControl("txtSqty")).Text.Trim() + "~" +
                                    ((Label)row.FindControl("lblsalrate")).Text.Trim() + "~" + 
                                    ((Label)row.FindControl("lblsalTax")).Text.Trim() + "~" +
                                    ((Label)row.FindControl("lblsaltotal")).Text.Trim() + "~" + 
                                    ((Label)row.FindControl("lblsalunit")).Text.Trim() + "~" +
                                    ((Label)row.FindControl("lblSalvageName")).Text.Trim().Replace(",", "ç") + "~" + 
                                    ((Label)row.FindControl("lblSalunitName")).Text.Trim() + "~" + "3 Salvage" + "~" + 
                                    ((Label)row.FindControl("lblSalvageItemId")).Text.Trim();
                                bChecked = true;
                            }
                            k++;
                        }


                        int l = 0;
                        sOillist = new string[GridOil.Rows.Count];
                        bChecked = false;
                        foreach (GridViewRow row in GridOil.Rows)
                        {
                            if (((CheckBox)row.FindControl("chkEOil")).Checked == true)
                            {
                                string sbadoil = ((Label)row.FindControl("lblOilName")).Text;

                                DataTable dt1 = new DataTable();
                                dt1 = (DataTable)ViewState["Oil"];
                                if (dt1.Rows.Count > 0 && dt1.Rows.Count > 2)
                                {
                                    GridOil.Rows[dt1.Rows.Count - 1].Cells[3].Text = Convert.ToString(dt1.Rows[dt1.Rows.Count - 1]["RESTM_ITEM_QNTY"]);

                                }
                                if (RetriveFromXML == false && txtActiontype.Text == "A" ||  RetriveFromXML == false && txtActiontype.Text == "R")
                                {

                                    if (sbadoil.Contains("BAD OIL"))
                                    {
                                        sOillist[l] = ((Label)row.FindControl("lblOilItemId")).Text.Trim() + "~" +
                                        Convert.ToString(dt1.Rows[dt1.Rows.Count - 1]["RESTM_ITEM_QNTY"]) + "~" +
                                       ((Label)row.FindControl("lblOilrate")).Text.Trim() + "~" + 
                                       ((Label)row.FindControl("lblOiltax")).Text.Trim() + "~" +
                                       ((Label)row.FindControl("lblOiltotal")).Text.Trim() + "~" + 
                                       ((Label)row.FindControl("lblOilunit")).Text.Trim() + "~" +
                                       ((Label)row.FindControl("lblOilName")).Text.Trim().Replace(",", "ç") + "~" + 
                                       ((Label)row.FindControl("lblOilunitName")).Text.Trim() + "~" + "4 Oil" + "~" + 
                                       ((Label)row.FindControl("lblOilId")).Text.Trim();
                                        bChecked = true;
                                    }
                                    else
                                    {
                                        sOillist[l] = ((Label)row.FindControl("lblOilItemId")).Text.Trim() + "~" + 
                                       ((Label)row.FindControl("lblOilQuantity")).Text.Trim() + "~" +
                                       ((Label)row.FindControl("lblOilrate")).Text.Trim() + "~" + 
                                       ((Label)row.FindControl("lblOiltax")).Text.Trim() + "~" +
                                       ((Label)row.FindControl("lblOiltotal")).Text.Trim() + "~" + 
                                       ((Label)row.FindControl("lblOilunit")).Text.Trim() + "~" +
                                       ((Label)row.FindControl("lblOilName")).Text.Trim().Replace(",", "ç") + "~" +
                                       ((Label)row.FindControl("lblOilunitName")).Text.Trim() + "~" + "4 Oil" + "~" +
                                       ((Label)row.FindControl("lblOilId")).Text.Trim();
                                        bChecked = true;
                                    }
                                }
                                else
                                {

                                    if (sbadoil.Contains("BAD OIL"))
                                    {
                                        sOillist[l] = ((Label)row.FindControl("lblOilItemId")).Text.Trim() + "~" +
                                        Convert.ToString(dt1.Rows[dt1.Rows.Count - 1]["RESTM_ITEM_QNTY"]) + "~" +
                                       ((Label)row.FindControl("lblOilrate")).Text.Trim() + "~" + 
                                       ((Label)row.FindControl("lblOiltax")).Text.Trim() + "~" +
                                       ((Label)row.FindControl("lblOiltotal")).Text.Trim() + "~" + 
                                       ((Label)row.FindControl("lblOilunit")).Text.Trim() + "~" +
                                       ((Label)row.FindControl("lblOilName")).Text.Trim().Replace(",", "ç") + "~" + 
                                       ((Label)row.FindControl("lblOilunitName")).Text.Trim() + "~" + "4 Oil" + "~" + 
                                       ((Label)row.FindControl("lblOilId")).Text.Trim();
                                        bChecked = true;
                                    }
                                    else
                                    {
                                        sOillist[l] = ((Label)row.FindControl("lblOilItemId")).Text.Trim() + "~" + 
                                       ((TextBox)row.FindControl("txtOqty")).Text.Trim() + "~" +
                                       ((Label)row.FindControl("lblOilrate")).Text.Trim() + "~" + 
                                       ((Label)row.FindControl("lblOiltax")).Text.Trim() + "~" +
                                       ((Label)row.FindControl("lblOiltotal")).Text.Trim() + "~" + 
                                       ((Label)row.FindControl("lblOilunit")).Text.Trim() + "~" +
                                       ((Label)row.FindControl("lblOilName")).Text.Trim().Replace(",", "ç") + "~" + 
                                       ((Label)row.FindControl("lblOilunitName")).Text.Trim() + "~" + "4 Oil" + "~" + 
                                       ((Label)row.FindControl("lblOilId")).Text.Trim();
                                        bChecked = true;
                                    }
                                }
                            }
                            l++;
                        }


                        if (txtActiontype.Text == "A" || txtActiontype.Text == "R")
                        {
                            if (hdfWFDataId.Value != "0")
                            {
                                ApproveRejectAction();

                                if (objSession.sTransactionLog == "1")
                                {
                                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (Estimation) Failure/Enhancement ");
                                }

                                cmdSave.Enabled = false;
                                cmdCalc.Enabled = false;
                                hdfWFOId.Value = hdfWFDataId.Value;


                                EstimationReport(txtestId.Value);

                                txtestId.Value = Convert.ToString(Session["FailureId"]);
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
                            obj.coiltype = cmbFailcoilType.SelectedValue;
                            obj.sEstid = txtestId.Value;
                            obj.sFaultCapacity = txtCapacity.Text;
                            obj.sOfficeCode = hdfFailureId.Value;
                            obj.sCrby = objSession.UserId;
                            obj.sWoundType = cmbMaterialType.SelectedValue;
                            obj.srateType = cmbRateType.SelectedValue;
                            obj.sWFO_id = hdfWFOId.Value;
                            obj.sEstComment = "";
                            hdfAppDesc.Value = obj.sEstComment;
                            obj.sDtrCode = txtTCCode.Text;
                            obj.sActionType = txtActiontype.Text;
                            if(objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                            {
                                obj.sOfficeCode = hdfOfficeCode.Value;
                            }
                            obj.sOfficeCode = objSession.OfficeCode;
                            obj.sEstimationNo = txtEstNo.Text;
                            SaveDocumments();
                            dtDocs = (DataTable)ViewState["DOCUMENTS"];
                            obj.dtDocuments = dtDocs;
                            Arr = obj.SaveEstimation(obj, sMateriallist, sLabourlist, sSalvageslist, sOillist, hdfStatusflag.Value );



                            if (Arr[1].ToString() == "0")
                            {
                                hdfWFDataId.Value = obj.sWFDataId;
                                ApproveRejectAction();
                                hdfWFOId.Value = obj.sWFDataId;

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
                        obj.sEstimationNo = txtEstNo.Text;
                        if (objSession.RoleId == Convert.ToString(ConfigurationManager.AppSettings["SupAdminRole"]))
                        {
                            obj.sOfficeCode = hdfOfficeCode.Value;
                        }
                     // obj.sOfficeCode = hdfOfficeCode.Value;
                        obj.sClientIP = sClientIP;
                        Arr = obj.SaveEstimation(obj, sMateriallist, sLabourlist, sSalvageslist, sOillist, hdfStatusflag.Value );

                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (FaultyEstimation) TcRepairer");
                        }

                        hdfWFDataId.Value = Arr[2];
                        hdfFailureId.Value = Arr[3];
                        EstimationReport(hdfFailureId.Value);
                        ShowMsgBox(Arr[0]);
                        cmdSave.Enabled = false;

                    }
                }

            }
            catch (Exception ex)
            {
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
                    if (hdfRejectApproveRef.Value == "RA")
                    {
                        objApproval.sApproveStatus = "1";
                    }
                    bResult = objApproval.ModifyApproveWFRequest(objApproval);
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
                    bResult = objApproval.ApproveWFRequest(objApproval);
                    if (objApproval.sNewRecordId == null)
                    {
                        txtestId.Value = objApproval.sRecordId;
                    }
                    else
                    {
                        txtestId.Value = objApproval.sNewRecordId; // updating estimation ID
                    }

                }
                if (bResult == true)
                {

                    if (objApproval.sApproveStatus == "1")
                    {
                        ShowMsgBox("Approved Successfully");
                        cmdSave.Enabled = false;


                    }
                    else if (objApproval.sApproveStatus == "3")
                    {

                        ShowMsgBox("Rejected Successfully");
                        cmdSave.Enabled = false;
                    }
                    else if (objApproval.sApproveStatus == "2")
                    {
                        ShowMsgBox("Modified and Approved Successfully");
                        cmdSave.Enabled = false;

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
            }
        }

        public void EstimationReport(string EstimationId)
        {
            try
            {
                if (sFailType == "2" && (hdfStatusflag.Value == "4" || hdfStatusflag.Value == "2"))
                {
                    string strParam = string.Empty;
                    if (EstimationId.Contains("-"))
                    {
                        strParam = "id=EnhanceEstimation&EnhanceId=" + hdfEnhancementId.Value + "&sStatus=" + EstimationId + "&FailType=" + "2";
                        RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

                    }
                    else
                    {
                        ClsRepairerEstimate objEst = new ClsRepairerEstimate();
                        objEst.sCrby = objSession.UserId;
                        objEst.sEstid = hdfEnhancementId.Value;
                        objEst.sOfficeCode = objSession.OfficeCode;
                        // objEst.SaveEstimationDetails(objEst);

                        strParam = "id=EnhanceEstimation&EnhanceId=" + hdfEnhancementId.Value + "&sStatus=" + EstimationId + "&FailType=" + "2";
                        RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                    }
                }
                else
                {
                    if (EstimationId.Contains("-") || EstimationId.Contains("0"))
                    {
                        string sWFO_ID = hdfWFDataId.Value;
                        string strParam1 = string.Empty;
                        strParam1 = "id=RefinedEstimationSOrepairer&sWFOID=" + sWFO_ID + "&sDtrcode=" + txtTCCode.Text + "&FailType=" + "1" ;
                        RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam1 + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                        return;

                    }
                    string sWFO_ID1 = hdfWFDataId.Value;
                    string strParam = string.Empty;
                    strParam = "id=RefinedEstimationSOrepairer&sWFOID=" + sWFO_ID1 + "&sDtrcode=" + txtTCCode.Text + "&FailType=" + "1" ;

                    //strParam = "id=RefinedEstimationrepairer&EstimationId=" + EstimationId;
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

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

                string kavika = Convert.ToString(ConfigurationSettings.AppSettings["KAVIKA_NEW"]);

                if(cmbRepairer.SelectedValue== kavika)
                {
                    hdfkavika.Value = "1";
                }

                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["DOCUMENTS"];

                if(ViewState["Oil"]!=null)
                {
                    DataTable dt1 = new DataTable();
                    dt1 = (DataTable)ViewState["Oil"];
                    if (dt1.Rows.Count > 0 && dt1.Rows.Count > 2)
                    {
                        GridOil.Rows[dt1.Rows.Count - 1].Cells[3].Text = Convert.ToString(dt1.Rows[dt1.Rows.Count - 1]["RESTM_ITEM_QNTY"]);

                    }
                }
               

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

                if (cmbGuarenteeType.SelectedValue == "0")
                {
                    if (!(sFailType == "2"))
                    {
                        ShowMsgBox("Please Select Guarantee Type");
                        cmbGuarenteeType.Focus();
                        return bValidate;
                    }
                }
                if (cmbMaterialType.SelectedIndex == 0)
                {
                   
                        ShowMsgBox("Please Select Winding Type");
                        cmbMaterialType.Focus();
                        return bValidate;
                   
                }
                if (cmbRateType.SelectedIndex == 0)
                {

                    ShowMsgBox("Please Select Rating Type");
                    cmbRateType.Focus();
                    return bValidate;

                }


                if (cmbRepairer.SelectedIndex == 0)
                {
                    if (!(sFailType == "2"))
                    {
                        ShowMsgBox("Please Select the Repairer");
                        cmbRepairer.Focus();
                        return bValidate;
                    }
                }
                if (txtEstDate.Text == "" || txtEstDate.Text == " ")
                {
                    ShowMsgBox("Please Enter Estimation Date");
                    txtEstDate.Focus();
                    return bValidate;
                }


                if (cmbFailcoilType.SelectedIndex == 0)
                {

                    ShowMsgBox("Please Select Coil Type");
                    cmbFailcoilType.Focus();
                    return bValidate;

                }

                 if (rybPhase.SelectedValue == "")
                {
                    ShowMsgBox("Please Select Phase");
                    rybPhase.Focus();
                    return bValidate;

                }




               // if (!(sFailType == "2"))
                //{
                    //if (cmbFailType.SelectedValue == "1")
                    //{
                    if (cmbGuarenteeType.SelectedValue == "WGP" || cmbGuarenteeType.SelectedValue == "WRGP")
                    {
                        if (Anex4 == false)
                        {
                            ShowMsgBox("Please  upload Annexure-4");
                            return bValidate;
                        }
                        if (Anex2 == false)
                        {
                            ShowMsgBox("Please upload Annexure-2");
                            return bValidate;
                        }
                    }

                    if (cmbGuarenteeType.SelectedValue == "AGP" )
                    {

                        if (Anex2 == false )
                        {
                            ShowMsgBox("Please upload Annexure-2");
                            return bValidate;
                        }
                    }

                //}

                // }

                {

                    //if (cmbFailType.SelectedValue != "2")
                    //{
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


                        foreach (GridViewRow row3 in grdSalvageMast.Rows)
                        {

                            if (((CheckBox)row3.FindControl("chkEOil")).Checked == true)
                            {
                                sQuantity = ((TextBox)row3.FindControl("txtOqty")).Text;

                                if (sQuantity.Length == 0)
                                {
                                    ShowMsgBox("Please Enter Oil Quatity");
                                    ((TextBox)row3.FindControl("txtOqty")).Focus();
                                    return bValidate;
                                }
                                else if (sQuantity == "0")
                                {
                                    ShowMsgBox("Zero Quantity is not Acceptable");
                                    ((TextBox)row3.FindControl("txtOqty")).Focus();
                                    return bValidate;
                                }
                            }
                        }
                    }

                    if (RetriveFromXML == false && txtActiontype.Text == "A" || RetriveFromXML == false && txtActiontype.Text == "M" || RetriveFromXML == false && txtActiontype.Text == "C")
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

                        foreach (GridViewRow row3 in GridOil.Rows)
                        {
                            //for (int i = 0; i < GridOil.Columns.Count; i++)
                            //{
                            //    String cellText = row3.Cells[i].Text;


                                string sbadoil = ((Label)row3.FindControl("lblOilName")).Text;
                                if (sbadoil.Contains("BAD OIL"))
                                {
                                    sQuantity = "";
                                }
                                else
                                {
                                    TextBox txt = (TextBox)GridOil.Rows[0].FindControl("txtOqty");
                                    if (txtActiontype.Text == "C" || txtActiontype.Text == "M")
                                    {
                                        sQuantity = ((TextBox)row3.FindControl("txtOqty")).Text;
                                    }
                                    else
                                    {
                                        sQuantity = ((Label)row3.FindControl("lblOilQuantity")).Text;
                                    }
                                }
                                if (sQuantity == "0")
                                {
                                    ShowMsgBox("Zero Quantity is not Acceptable");
                                    ((TextBox)row3.FindControl("txtOqty")).Focus();
                                    return bValidate;
                                }
                           // }


                            if (sFailType != "2")
                            {
                                cmdCalc_Click(new object(), new EventArgs());
                            }
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


        protected void grdOilMast_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if ((RetriveFromXML == true && (txtActiontype.Text == "A" || txtActiontype.Text == "V" || txtActiontype.Text == "R")) || txtActiontype.Text == "VIEW")
                {
                    foreach (GridViewRow row1 in GridOil.Rows)
                    {
                        CheckBox chk = ((CheckBox)row1.FindControl("chkEOil"));
                        chk.Checked = true;
                    }

                    GridOil.Columns[3].Visible = true;
                    GridOil.Columns[3].Visible = false;
                    GridOil.Columns[4].Visible = true;
                    GridOil.Columns[5].Visible = false;
                }
                if (RetriveFromXML == true && (txtActiontype.Text == "M"))
                {
                    foreach (GridViewRow row1 in GridOil.Rows)
                    {
                        CheckBox chk = ((CheckBox)row1.FindControl("chkEOil"));
                        chk.Checked = true;
                    }


                   

                  
                        foreach (GridViewRow row in GridOil.Rows)
                        {
                            String oTotal = "";
                            //for minus from bad oil
                            string sbadoil = ((Label)row.FindControl("lblOilName")).Text;
                           


                            if (sbadoil.Contains("BAD OIL"))
                            {
                            
                               
                                ((TextBox)row.FindControl("txtOqty")).Text = ((Label)row.FindControl("lblOilQuantity")).Text;

                            TextBox txt = (TextBox)row.FindControl("txtOqty");
                            if (txt != null)
                            {
                                txt.Attributes.Add("readonly", "readonly");
                                // txt.Attributes.Remove("readonly"); To remove readonly attribute
                            }


                        }
                            else
                            {
                               // GridOil.Rows[2].Cells[3].Text = Convert.ToString(dt.Rows[2][9]);
                            }
                           
                        
                    }

                        // GridOil.Columns[3].Visible = true;
                        // GridOil.Columns[3].Visible = false;
                        // GridOil.Columns[4].Visible = true;
                        GridOil.Columns[5].Visible = false;
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
                // bool Checked = false;
                if (objSession.RoleId == "4")
                {
                    txtEstDate.Text = string.Empty;
                    cmbGuarenteeType.SelectedIndex = 0;
                    cmbFileType.SelectedIndex = 0;
                    cmbFailcoilType.SelectedIndex = 0;
                    rybPhase.SelectedIndex = -1;
                    txtEstDate.Text = string.Empty;
                    cmbMaterialType.SelectedIndex = 0;
                    cmbRateType.SelectedIndex = 0;
                    cmbRepairer.SelectedIndex = 0;
                    //grdMaterialMast.DataSource = null;
                    // grdMaterialMast.Rows.Clear();
                    //  this.grdMaterialMast.DataSource = this.GetNewValues();

                    DataTable dt1 = new DataTable();
                    dt1 = (DataTable)ViewState["Oil"];
                    if (dt1 != null && dt1.Rows.Count>2)
                    {
                        GridOil.Rows[dt1.Rows.Count - 1].Cells[3].Text = Convert.ToString(dt1.Rows[dt1.Rows.Count - 1]["RESTM_ITEM_QNTY"]);

                    }

                }
                else
                {
                    txtComment.Text = string.Empty;


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

                //DataTable dt1 = new DataTable();
                //dt1 = (DataTable)ViewState["Oil"];
                //if (dt1.Rows.Count > 0)
                //{
                //    GridOil.Rows[2].Cells[3].Text = Convert.ToString(dt1.Rows[2][9]);
                //}

                if (cmbRepairer.SelectedValue != "--Select--")
                {
                    ClsRepairerEstimate objEst = new ClsRepairerEstimate();
                 //   string off = objSession.OfficeCode;
                    string off = objSession.OfficeCode.Substring(0, 3);

                    bool res = objEst.RepairDateIsValid(cmbRepairer.SelectedValue,off);

                    if (res == true)
                    {
                        grdMaterialMast.Visible = true;
                        grdLabourMast.Visible = true;
                        grdSalvageMast.Visible = true;
                        GridOil.Visible = true;
                        LoadMaterialDetails(sWoundType, srateType);
                        LoadLabourDetails(sWoundType, srateType);
                        LoadSalvageDetails(sWoundType, srateType);
                        LoadOilDetails();
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
                    GridOil.Visible = false;
                }


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
                    Response.Redirect("FaultyTCEstimateview.aspx", false);
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
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
                dtCloned.Columns[3].DataType = typeof(string);
                foreach (DataRow row in dtDetails.Rows)
                {
                    dtCloned.ImportRow(row);
                }


                dtDetails.AcceptChanges();

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

                    List<string> listtoRemove = new List<string> { "MRI_MEASUREMENT", "RESTM_ITEM_QNTY", "MRI_BASE_RATE", "MRI_TAX" , "MRI_TOTAL" };
                    string filename = "FaultyEstimationDetails" + DateTime.Now + ".xlsx";
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
                        if (((Label)row.FindControl("lblMaterialId")).Text ==Convert.ToString(dtExcelData1.Rows[i]["MRIM_ID"]))
                        {
                            if( Convert.ToString(dtExcelData1.Rows[i]["QUANTITY"]) != "")
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
                            if (  Convert.ToString(dtExcelData1.Rows[i]["QUANTITY"]) != "")
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
                        if ((( Label) row.FindControl("lblSalvageId")).Text == Convert.ToString(dtExcelData1.Rows[i]["MRIM_ID"]))
                        {
                            if ( Convert.ToString(dtExcelData1.Rows[i]["QUANTITY"]) != "")
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
        protected void cmdCalc_Click(object sender, EventArgs e)
        {
            string sLabelQty = string.Empty;
            double MaterialTotal = 0;
            double LabourTotal = 0;
            double SalvageTotal = 0;
            double OilTotal = 0;

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

                    //DataTable dt = new DataTable();
                    //if (ViewState["dt"] == null)
                    //{
                    //    dt.Columns.Add("MRIM_ID");
                    //    dt.Columns.Add("MRIM_ITEM_NAME");
                    //    dt.Columns.Add("MRIM_ITEM_ID");
                    //    dt.Columns.Add("RESTM_ITEM_QNTY");
                    //    dt.Columns.Add("MRI_MEASUREMENT");
                    //    dt.Columns.Add("MRI_BASE_RATE");
                    //    dt.Columns.Add("MRI_TAX");
                    //    dt.Columns.Add("MRI_TOTAL");                  
                    //}

                    foreach (GridViewRow row in GridOil.Rows)
                    {
                        //string kavika = Convert.ToString(ConfigurationSettings.AppSettings["KAVIKA_NEW"]);
                        sLabelQty = string.Empty;
                        string sQuantity = string.Empty;
                        string sbadoil = ((Label)row.FindControl("lblOilName")).Text;
                        if (RetriveFromXML == false && txtActiontype.Text == "C" || RetriveFromXML == false && txtActiontype.Text == "M")
                        {
                            if (sbadoil.Contains("BAD OIL"))
                            {
                                sQuantity = ((Label)row.FindControl("lblOilQuantity")).Text;
                                if (sQuantity == "0")
                                {
                                    sQuantity = "";
                                }
                            }
                            else
                            {
                                sQuantity = ((TextBox)row.FindControl("txtOqty")).Text;
                            }
                        }//for minus from bad oil
                        else
                        {
                            if (txtActiontype.Text == "A")
                            {
                                if (sbadoil.Contains("BAD OIL"))
                                {
                                    sQuantity = ((Label)row.FindControl("lblOilQuantity")).Text; 
                                    if (sQuantity == "0")
                                    {
                                        sQuantity = "";
                                    }
                                }
                                else
                                {
                                    sQuantity = ((Label)row.FindControl("lblOilQuantity")).Text;
                                }
                            }
                            else
                            {
                                if (sbadoil.Contains("BAD OIL"))
                                {
                                    sQuantity = ((Label)row.FindControl("lblOilQuantity")).Text;
                                    if (sQuantity == "0")
                                    {
                                        sQuantity = "";
                                    }
                                }
                                else
                                {
                                    sLabelQty = ((Label)row.FindControl("lblOilQuantity")).Text;
                                }
                            }
                        }
                                             
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
                            else
                            {
                                sQuantity = sLabelQty;
                            }
                        }
                        else if (sLabelQty != "" && sLabelQty != "0")
                        {
                            sQuantity = sLabelQty;
                        }
                        //string sLabelQty = ((Label)row.FindControl("lbllabQuantity")).Text;
                        Label lbltotal = (Label)row.FindControl("lblOiltotal");
                        Label lblBase = (Label)row.FindControl("lblOilrate");
                        Label lblTax = (Label)row.FindControl("lblOiltax");
                        CheckBox chkMaterial = (CheckBox)row.FindControl("chkEOil");
                        double Total = 0;
                        if (sQuantity.Length > 0 || sLabelQty.Length > 0)
                        {
                           
                            if (sQuantity != "" && sLabelQty != "")
                            {
                                Total = (Convert.ToDouble(sQuantity) * Convert.ToDouble(lblBase.Text));//+ (((Convert.ToDouble(sQuantity) * Convert.ToDouble(lblBase.Text)) / 100) * (Convert.ToDouble(lblTax.Text.Replace("%", ""))));
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
                        if (sbadoil.Contains("BAD OIL"))
                        {
                            OilTotal = OilTotal- Total;
                        }
                        else if(hdfkavika.Value=="1" && sbadoil.Contains("RECLAIMED OIL"))
                        { 
                            OilTotal = OilTotal - Total;
                        }
                        else
                        {
                            OilTotal = OilTotal + Total;
                        }

                    }
                    
                    //GridOil.DataSource= ViewState["Oil"];
                    //GridOil.DataBind();
                    if(ViewState["Oil"]!=null)
                    {
                        DataTable dt = new DataTable();
                        dt = (DataTable)ViewState["Oil"];
                        if (dt.Rows.Count > 0 && dt.Rows.Count > 2)
                        {
                            GridOil.Rows[dt.Rows.Count - 1].Cells[3].Text = Convert.ToString(dt.Rows[dt.Rows.Count - 1]["RESTM_ITEM_QNTY"]);
                        }
                        if (dt.Rows.Count > 0)
                        {
                            GridOil.FooterRow.Cells[9].Text = Convert.ToString(Math.Round(OilTotal, 2));
                            lbloilfinaltotal.Text = GridOil.FooterRow.Cells[9].Text;
                        }
                    }
                   


                   

                    if (SalvageTotal != 0)
                    {
                        grdSalvageMast.FooterRow.Cells[9].Text = Convert.ToString(Math.Round(SalvageTotal, 2));

                        lblTotalCharges.Text = Convert.ToString(Math.Round(MaterialTotal + LabourTotal + OilTotal - SalvageTotal, 2));
                        lblTotalAmount.Text = Convert.ToString(Math.Round(MaterialTotal + LabourTotal - SalvageTotal, 2));
                    }
                    else
                    {
                        lblTotalCharges.Text = Convert.ToString(Math.Round(MaterialTotal + LabourTotal + OilTotal - SalvageTotal, 2));
                        lblTotalAmount.Text = Convert.ToString(Math.Round(MaterialTotal + LabourTotal - SalvageTotal, 2));
                    }

                }
                else
                {
                    ShowMsgBox("Plaese select Repairer");
                    cmbRepairer.Focus();
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
                DataTable dt1 = new DataTable();
                dt1 = (DataTable)ViewState["Oil"];
                if (dt1 != null && dt1.Rows.Count > 2)
                {
                    GridOil.Rows[dt1.Rows.Count - 1].Cells[3].Text = Convert.ToString(dt1.Rows[dt1.Rows.Count - 1]["RESTM_ITEM_QNTY"]);

                    //GridOil.Rows[2].Cells[3].Text = Convert.ToString(dt1.Rows[2][9]);
                }
                if (txtTCCode.Text.Trim() == "" || txtTCCode.Text.Trim() == null)
                {
                    txtTCCode.Focus();
                    ShowMsgBox("Enter DTR Code");
                }
                else
                {
                    sTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtTCId.Text));
                    string url = "/MasterForms/TcMaster.aspx?TCId=" + sTCId;
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
            DataTable dt1 = new DataTable();
            try
            {
                
                dt1 = (DataTable)ViewState["Oil"];
                if (dt1 != null && dt1.Rows.Count > 2)
                {
                    GridOil.Rows[dt1.Rows.Count - 1].Cells[3].Text = Convert.ToString(dt1.Rows[dt1.Rows.Count - 1]["RESTM_ITEM_QNTY"]);

                }

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
                DataTable dt1 = new DataTable();
                dt1 = (DataTable)ViewState["Oil"];
                if (dt1 != null && dt1.Rows.Count > 2)
                {
                    GridOil.Rows[dt1.Rows.Count - 1].Cells[3].Text = Convert.ToString(dt1.Rows[dt1.Rows.Count - 1]["RESTM_ITEM_QNTY"]);

                }

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
                DataTable dt1 = new DataTable();
                dt1 = (DataTable)ViewState["Oil"];
                if (dt1 != null && dt1.Rows.Count>2)
                {
                    GridOil.Rows[dt1.Rows.Count - 1].Cells[3].Text = Convert.ToString(dt1.Rows[dt1.Rows.Count - 1]["RESTM_ITEM_QNTY"]);

                }

                if (cmbFileType.SelectedIndex == 0)
                {
                    ShowMsgBox("Please Select the File Type");
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

                if (sFileName.Contains("'"))
                {
                    ShowMsgBox("File Name Should not Contain Single quotes");
                    return;
                }

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
                string sMainFolderName = "REPAIRERESTIMATIONDOCS";



                if (dtDocs != null)
                {
                    for (int i = 0; i < dtDocs.Rows.Count; i++)
                    {
                        string sName = Convert.ToString(dtDocs.Rows[i]["NAME"]);
                        string sPath = Convert.ToString(dtDocs.Rows[i]["PATH"]);

                        if (File.Exists(sPath))
                        {
                            bool IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName + "/" );
                            if (IsExists == false)
                            {

                                objFtp.createDirectory(SFTPmainfolder + sMainFolderName + "/");
                            }

                             IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName + "/" + txtEstNo.Text + "/");
                            if (IsExists == false)
                            {

                                objFtp.createDirectory(SFTPmainfolder + sMainFolderName + "/" + txtEstNo.Text);
                            }


                            

                            Isuploaded = objFtp.upload(SFTPmainfolder + sMainFolderName + "/" + txtEstNo.Text + "/", sName, sPath);
                            if (Isuploaded == true & File.Exists(sPath))
                            {
                                File.Delete(sPath);
                                sPath = SFTPmainfolder + sMainFolderName + "/" + txtEstNo.Text + "/" + sName;
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

                DataTable dt1 = new DataTable();
                dt1 = (DataTable)ViewState["Oil"];
                if (dt1 != null && dt1.Rows.Count>2)
                {
                    GridOil.Rows[dt1.Rows.Count].Cells[3].Text = Convert.ToString(dt1.Rows[dt1.Rows.Count]["RESTM_ITEM_QNTY"]);
                }

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




    }
}