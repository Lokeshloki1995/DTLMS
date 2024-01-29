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

namespace IIITS.DTLMS.PermanentDecomm
{
    public partial class PermanentEstimation : System.Web.UI.Page
    {
        string strFormCode = "PermanentEstimation";
        clsSession objSession = new clsSession();
        DataTable dtLabour = new DataTable();
        bool RetriveFromXML = false;
        string sFailId = string.Empty; 
        string sEstId = string.Empty;
        string DTCId = string.Empty;
        static string sFailType = string.Empty;
        string intialid = string.Empty;
        clsPermanentEstimation obj = new clsPermanentEstimation();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
            {
                Response.Redirect("~/Login.aspx", false);
            }
            Form.DefaultButton = cmdSave.UniqueID;
            objSession = (clsSession)Session["clsSession"];
            lblMessage.Text = string.Empty;
            txtEstDate.Attributes.Add("readonly", "readonly");

            if (!IsPostBack)
            {
                CalendarExtender1.EndDate = System.DateTime.Now;

                if (objSession.sRoleType== "2")
                {
                    Genaral.Load_Combo("SELECT \"TR_ID\", UPPER(\"TR_NAME\") FROM \"TBLTRANSREPAIRER\",\"TBLTRANSREPAIREROFFCODE\"  WHERE \"TR_ID\"=\"TRO_TR_ID\"  AND   " + clsStoreOffice.GetOfficeCode(objSession.OfficeCode, "TRO_OFF_CODE") + " GROUP BY \"TR_ID\"  ORDER BY \"TR_NAME\"", "--Select--", cmbRepairer);
                }
                else
                {
                    Genaral.Load_Combo("SELECT \"TR_ID\", UPPER(\"TR_NAME\") FROM \"TBLTRANSREPAIRER\" ,\"TBLTRANSREPAIREROFFCODE\" WHERE \"TR_ID\"=\"TRO_TR_ID\" AND CAST(\"TRO_OFF_CODE\" AS TEXT) = SUBSTR(CAST('" + objSession.OfficeCode + "' AS TEXT),1,'" + Constants.Division + "') ORDER BY \"TR_NAME\" ", "--Select--", cmbRepairer);
                }
                if (Request.QueryString["EstID"].ToString() == "")
                {

                    DTCId = Genaral.UrlDecrypt(Request.QueryString["DTCId"].ToString());
                    txtActiontype.Text = Genaral.UrlDecrypt(Request.QueryString["ActionType"].ToString());
                    sEstId = Genaral.UrlDecrypt(Request.QueryString["EstID"].ToString());
                    hdfEstId.Value = sEstId;
                    Session["WFOId"] = "0";
                    LoadFailedDetails(DTCId);
                    LoadSearchWindow();
                 
                }
                
                else
                {
                    DTCId = Genaral.UrlDecrypt(Request.QueryString["DTCId"].ToString());
                    txtActiontype.Text = Genaral.UrlDecrypt(Request.QueryString["ActionType"].ToString());
                    sEstId = Genaral.UrlDecrypt(Request.QueryString["EstID"].ToString());
                    hdfEstId.Value = sEstId;
                    if (txtActiontype.Text=="VIEW")
                    {
                    }
                    else
                    {
                          intialid=Request.QueryString["sWFInitialId"].ToString();

                    }
                    
                    for (int i = 0; i < Session.Contents.Count; i++)
                    {
                        var key = Session.Keys[i];
                        var value = Session[i];
                        if (key.ToString() == "WFDataId")
                        {
                            hdfWFDataId.Value = Convert.ToString(Session["WFDataId"]);
                        }

                    }
                    if (!hdfEstId.Value.Contains("-"))
                    {
                       
                            GetDatafromMainTable(sEstId);
                        
                    }
                   

                  
                }

                if ( hdfWFDataId.Value != "")
                {
                    if (objSession.RoleId != "4")
                    {
                        GetDataFromXML(intialid, txtActiontype.Text);
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
                    GetFailId(sFailId);
                }
                else
                {
                    LoadFailedDetails(DTCId);
                }
                WorkFlowConfig();
                if (objSession.sRoleType != "4")
                {
                    Session["BOID"] = "62";
                    ViewState["BOID"] = "62";
                }
                else
                {
                    ViewState["BOID"] = Convert.ToString(Session["BOID"]);
                }
            }

            ApprovalHistoryView.BOID = ViewState["BOID"].ToString();
            ApprovalHistoryView.sRecordId = hdfEstId.Value;

            if (txtActiontype.Text == "M")
            {
                clsApproval objLevel = new clsApproval();
                string sLevel = objLevel.sGetApprovalLevel(Convert.ToString(ViewState["BOID"]), objSession.RoleId);
                if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                {
                    cmdSave.Text = " Modify and Submit";
                    
                    
                }
                else if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                {
                    cmdSave.Text = " Modify and Approve";
                   
                    
                }
            }
            else if (txtActiontype.Text == "A")
            {
                clsApproval objLevel = new clsApproval();
                string sLevel = objLevel.sGetApprovalLevel(Convert.ToString(ViewState["BOID"]), objSession.RoleId);
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

        public void LoadSearchWindow()
        {
            try
            {
                string strQry = string.Empty;
                strQry = "Title=Search and Select Transformer Centre  Details&";
                strQry += "Query=SELECT CAST(\"DT_CODE\" AS TEXT), CAST(\"DT_NAME\" AS TEXT) FROM \"TBLDTCMAST\"  inner join \"TBLTCMASTER\" on \"DT_TC_ID\"=\"TC_CODE\" left join \"TBLPERMANENTESTIMATIONDETAILS\" on \"PEST_TC_CODE\"=\"TC_CODE\" WHERE CAST(\"DT_OM_SLNO\" AS TEXT) LIKE '" + objSession.OfficeCode + "%' AND ";
                strQry += "  \"PEST_ID\" IS  NULL and \"TC_CURRENT_LOCATION\" <> '5' and \"DT_TC_ID\" <> '0' AND \"DT_CODE\" NOT IN (SELECT \"DF_DTC_CODE\" FROM \"TBLDTCFAILURE\" WHERE  \"DF_REPLACE_FLAG\" = 0  )  AND {0} like %{1}% order by \"DT_CODE\" &";
                strQry += "DBColName=\"DT_CODE\"~\"DT_NAME\"&";
                strQry += "ColDisplayName=Transformer Centre Code~Transformer Centre Name&";

                strQry = strQry.Replace("'", @"\'");

                cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtDTCCode.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + txtDTCCode.ClientID + ")");

               

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
                clsPermanentEstimation objest = new clsPermanentEstimation();


                objest.sDtcCode = Request.Form[txtDTCCode.UniqueID];

                LoadFailedDetails(objest.sDtcCode);

                //if (objest.sDtcName == null)
                //{

                //}
                //else
                //{
                //    txtDTCName.Text = objest.sDtcName;


                //    txtDtcId.Text = objest.sDtcId;
                //    txtDTCCode.Text = objest.sDtcCode;
                //    txtDTCName.Text = objest.sDtcName;
                //    txtTCId.Text = objest.sTCId;
                //    //cmbRepairer.SelectedValue = objest.sRepairer;
                //    if (objest.sGuarantyType == "")
                //    {
                //        objest.sGuarantyType = "0";
                //    }
                //    cmbGuarenteeType.SelectedValue = objest.sGuarantyType;

                //    if (cmbGuarenteeType.SelectedValue == "0")
                //    {
                //        if (cmbGuarenteeType.SelectedIndex == 0)
                //        {
                //            cmbGuarenteeType.Enabled = true;
                //        }
                //        else
                //            cmbGuarenteeType.Enabled = false;
                //    }

                   

                  
                    
                //}
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }



        }


        public void GetWorkOrderDetails()
        {
            try
            {

                clsPermanentWO objWorkOrder = new clsPermanentWO();
               

                objWorkOrder.GetWorkOrderDetails(objWorkOrder);

          
                hdfFailureId.Value = objWorkOrder.sFailureId;
               //  cmbCapacity.SelectedValue = objWorkOrder.sCapacity;

                if (objWorkOrder.sCommWoNo != null || objWorkOrder.sCommWoNo != "0")
                {
                    
                    cmbRepairer.SelectedValue = objWorkOrder.sRepairer;

                }

                else
                {
                    //cmdSave.Text = "Save";
                   
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        public void LoadFailedDetails(string sdtcid)
        {
            try
            {
                clsPermanentEstimation objest = new clsPermanentEstimation();
                //txtFailureId.Text = hdfFailureId.Value;
                objest.sDtcId = sdtcid;

                objest.GetFailureDetails(objest);

                txtDTCName.Text = objest.sDtcName;
                txtDTCCode.Text = objest.sDtcCode;
                txtDeclaredBy.Text = objest.sCrby;
                txtTCCode.Text = objest.sDtcTcCode;
                txtCapacity.Text = objest.sDtcCapacity;
                cmbRepairer.SelectedValue = objest.sLastRepairedBy;
                hdfFailureId.Value = objest.sOfficeCode;

              //  hdfStatusflag.Value = objest.sStatus_flag;
                for (int i = 0; i < Session.Contents.Count; i++)
                {
                    var key = Session.Keys[i];
                    var value = Session[i];
                    if (key == "WFOId")
                    {
                        hdfWFOId.Value = Convert.ToString(Session["WFOId"]);
                    }

                }


                hdfGuarenteeSource.Value = objest.sGuarantyType;
                cmbGuarenteeType.SelectedValue = objest.sGuarantyType;
                //ViewState["OffCode"] = objest.sOfficeCode;
                LOadEstimateNumber(objest.sOfficeCode);
                txtDtcId.Text = objest.sDtcId;
                txtTCId.Text = objest.sTCId;

                    //sFailType = "2";
                    //cmbFailType.SelectedValue = "2";
                    //cmbFailType.Visible = false;
                    //cmbRepairer.Visible = false;
                    //divRepairerFail.Visible = false;
                    //divLabourCost.Visible = false;

              
                ViewState["BOID"] = "62";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void WorkFlowConfig()
        {
            try
            {
                if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
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
                    

                    if (hdfWFDataId.Value != "0")
                    {
                        
                        string sApproveStatus = GetPreviousApproveStatus(hdfWFOId.Value);
                       // string sApproveStatus = string.Empty;
                        if (hdfWFDataId.Value != "")
                        {
                           // hdfWFDataId.Value = Convert.ToString(Session["sWFInitialId"]);
                            if (objSession.RoleId != "4")
                            {
                                GetDataFromXML(Convert.ToString(Session["sWFInitialId"]), sApproveStatus);
                            }
                            else if (sApproveStatus == "3")
                            {
                                GetDataFromXML(Convert.ToString(Session["sWFInitialId"]), sApproveStatus);
                            }
                            else
                            {
                                GetDataFromXML(hdfWFDataId.Value, sApproveStatus);
                            }

                            if (txtActiontype.Text == "M")
                            {
                                cmdCalc.Visible = true;
                            }
                            else
                                cmdCalc.Visible = false;
                        }

                    }

                    dvComments.Style.Add("display", "block");
                    //cmdReset.Enabled = false;

                    if (hdfWFOAutoId.Value != "0")
                    {

                        dvComments.Style.Add("display", "none");
                    }
                    if (txtActiontype.Text == "V")
                    {
                        cmdSave.Text = "View";
                        cmdReset.Enabled = false;
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


        public void LOadEstimateNumber(string officeCode)
        {
            try
            {
                clsPermanentEstimation objEstmate = new clsPermanentEstimation();
                string sEstNo = objEstmate.GenerateEstimationNo(officeCode);
                txtEstNo.Text = sEstNo;
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

        public void LoadLabourDetails(string sWoundType)
        {
            try
            {
                DataTable dt = new DataTable();
                clsPermanentEstimation objEstimate = new clsPermanentEstimation();
                objEstimate.eUser = cmbRepairer.SelectedValue;
                objEstimate.eCapacity = txtCapacity.Text;
                objEstimate.sWoundType = sWoundType;
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

        protected void GetDataFromXML(string intialid, string sApproveStatus)
        {
            try
            {
                DataTable dtEstDetails = new DataTable();
                clsPermanentEstimation objest = new clsPermanentEstimation();
                string sWo_Id = string.Empty;
                //txtFailureId.Text = hdfFailureId.Value;

                   sWo_Id = intialid;
                   dtEstDetails = objest.getFailId(sWo_Id);
                   txtDtcId.Text = dtEstDetails.Rows[0]["WO_DATA_ID"].ToString();
                   objest.sDtcId = txtDtcId.Text;

                   objest.GetFailureDetails(objest);

                   sWo_Id = hdfWFDataId.Value;

               // txtFailureDate.Text = objFailure.sFailureDate;
                   txtDTCName.Text = objest.sDtcName;
                   txtDTCCode.Text = objest.sDtcCode;
                   txtDeclaredBy.Text = objest.sCrby;
                   txtTCCode.Text = objest.sDtcTcCode;
                   txtCapacity.Text = objest.sDtcCapacity;
                   cmbRepairer.SelectedValue = objest.sLastRepairedBy;
                   hdfFailureId.Value = objest.sOfficeCode;
                   cmbGuarenteeType.SelectedValue = objest.sGuarantyType;

                   hdfGuarenteeSource.Value = objest.sGuarantyType;
                   LOadEstimateNumber(objest.sOfficeCode);
                   txtDtcId.Text = objest.sDtcId;
                   txtTCId.Text = objest.sTCId;
                   cmdSearch.Visible = false;
                //   if (objest.sStatus_flag == "2")
                //{

                //    cmbFailType.SelectedValue = "2";
                //    cmbFailType.Visible = false;
                //    cmbRepairer.Visible = false;
                //    divRepairerFail.Visible = false;
                //    divLabourCost.Visible = false;
                   
                //}
                clsPermanentEstimation obj = new clsPermanentEstimation();
                if (txtActiontype.Text == "V" || txtActiontype.Text == "M" || txtActiontype.Text == "R" || txtActiontype.Text == "A")
                {
                    obj.sWFO_id = hdfWFDataId.Value;
                }
                else
                {
                    obj.sWFO_id = dtEstDetails.Rows[0]["WO_WFO_ID"].ToString();
                }

                if (sWo_Id != null && sWo_Id != "")
                {
                    //hdfWFOId.Value = sWo_Id;
                   // hdfWFDataId.Value = sWo_Id;
                }

                if (sApproveStatus != "3")
                {
                    obj.GetEstimateDetailsFromXML(obj);
                    cmbRepairer.SelectedValue = obj.sLastRepair;
                    cmbMaterialType.SelectedValue = obj.sWoundType;
                    cmbFailType.SelectedValue = obj.sFailType;
                    cmbGuarenteeType.SelectedValue = obj.sGuaranteetype;
                    txtEstDate.Text = obj.sEstDate;
                    txtreason.Text = obj.sReasons;
                    RetriveFromXML = true;

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


                    cmbFailType.Enabled = false;
                 //   txtFailureId.Enabled = false;
                    cmbRepairer.Enabled = false;
                    cmbMaterialType.Enabled = false;
                    cmbGuarenteeType.Enabled = false;
                    txtEstDate.Enabled = false;
                    txtreason.Enabled = false;
                  //  cmdAdd.Enabled = false;

                }
                else
                {
                    obj.GetEstimateDetailsFromXML(obj);
                    cmbRepairer.SelectedValue = obj.sLastRepair;
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
                clsPermanentEstimation obj = new clsPermanentEstimation();
                obj.sEstimationId = sEstId;
                obj.GetDetailsfromMainDB(obj);

                cmbRepairer.SelectedValue = obj.sLastRepair;
                cmbFailType.SelectedValue = obj.sFailType;
                cmbMaterialType.SelectedValue = obj.sWoundType;
                cmbGuarenteeType.SelectedValue = obj.sGuaranteetype;
                txtEstDate.Text = obj.sEstDate;
                txtreason.Text = obj.sReasons;
                txtDeclaredBy.Text = obj.sCrby;
                txtTCCode.Text = obj.sDtrCode;
                txtEstNo.Text = obj.sEstimationNo;
                txtCapacity.Text = obj.sFaultCapacity;
                txtDTCName.Text = obj.sDtcName;
                txtDTCCode.Text = obj.sDtcCode;
                cmdSearch.Visible = false;
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

                grdLabourMast.Columns[9].Visible = false;

                cmbFailType.Enabled = false;      
                cmbRepairer.Enabled = false;
                cmbMaterialType.Enabled = false;
                cmbGuarenteeType.Enabled = false;
                txtEstDate.Enabled = false;
                txtreason.Enabled = false;
                cmdSave.Text = "View";
                cmdReset.Enabled = false;
                if (txtActiontype.Text == "M")
                {
                    cmdCalc.Visible = true;
                }
                else
                {
                    cmdCalc.Visible = false;
                }
                dvComments.Style.Add("display", "none");

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
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

         protected void cmdSave_Click(object sender, EventArgs e)
        {            
            string[] Arr = new string[2];
            try
            {
                clsPermanentEstimation obj = new clsPermanentEstimation();
                if (cmdSave.Text == "View")
                {
                    EstimationReport(hdfEstId.Value);
                }
                else
                {
                    if (ValidateForm() == true)
                    {

                        //  obj.sFailureId = txtFailureId.Text;

                        obj.sFaultCapacity = txtCapacity.Text;
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
                        obj.sWFO_id = hdfWFOId.Value;
                        obj.sEstComment = "";
                        hdfAppDesc.Value = obj.sEstComment;
                        obj.sDtrCode = txtTCCode.Text;
                        obj.sActionType = txtActiontype.Text;
                        obj.sDtcCode = txtDTCCode.Text;
                        obj.sWoundType = cmbMaterialType.SelectedValue;
                        obj.sEstDate = txtEstDate.Text;
                        obj.sReasons = txtreason.Text;
                        obj.sDtcId = txtDtcId.Text;
                        if (lblTotalCharges.Text == "")
                        {
                            obj.sFinalTotalAmount = "0";
                        }
                        else
                        {
                            obj.sFinalTotalAmount = lblTotalCharges.Text;
                        }

                        int j = 0;
                        string[] sLabourlist = new string[grdLabourMast.Rows.Count];
                        bool bChecked = false;
                        foreach (GridViewRow row in grdLabourMast.Rows)
                        {
                            if (((CheckBox)row.FindControl("chkElabour")).Checked == true)
                            {

                                sLabourlist[j] = ((Label)row.FindControl("lblLabourId")).Text.Trim() + "~" + ((TextBox)row.FindControl("txtLqty")).Text.Trim() + "~" +
                                    ((Label)row.FindControl("lbllabrate")).Text.Trim() + "~" + ((Label)row.FindControl("lbllabtax")).Text.Trim() + "~" +
                                    ((Label)row.FindControl("lbllabtotal")).Text.Trim() + "~" + ((Label)row.FindControl("lbllabunit")).Text.Trim() + "~" +
                                    ((Label)row.FindControl("lblLabourName")).Text.Trim().Replace(",", "ç") + "~" + ((Label)row.FindControl("lblLabunitName")).Text.Trim() + "~" + "Labour Charges" + "~" + ((Label)row.FindControl("lblLabourItemId")).Text.Trim();
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

                        if (txtActiontype.Text == "A" || txtActiontype.Text == "R")
                        {
                            if (hdfWFDataId.Value != "0")
                            {
                                ApproveRejectAction();
                                cmdSave.Enabled = false;
                                cmdCalc.Enabled = false;
                                hdfWFOId.Value = hdfWFDataId.Value;
                                if (objSession.RoleId == "4")
                                {
                                   // EstimationReport(hdfEstId.Value);
                                }
                                else
                                {
                                    EstimationReport(hdfEstId.Value);
                               }
                               
                                return;
                            }
                        }

                        if (txtActiontype.Text == "M")
                        {
                            if (txtComment.Text.Trim() == "")
                            {
                                ShowMsgBox("Enter Comments/Remarks");
                                txtComment.Focus();
                                return;

                            }
                            // obj.sFailureId = txtFailureId.Text;
                            obj.sFaultCapacity = txtCapacity.Text;
                            //obj.sLastRepair = cmbRepairer.SelectedValue;
                            obj.sOfficeCode = hdfFailureId.Value;
                            obj.sCrby = objSession.UserId;
                            obj.sFailType = cmbFailType.SelectedValue;
                            obj.sWoundType = cmbMaterialType.SelectedValue;
                            obj.sWFO_id = hdfWFOId.Value;
                            obj.sEstComment = "";
                            hdfAppDesc.Value = obj.sEstComment;
                            obj.sDtrCode = txtTCCode.Text;
                            obj.sActionType = txtActiontype.Text;
                            obj.sDtcCode = txtDTCCode.Text;
                            obj.sDtcId = txtDtcId.Text;
                            Arr = obj.SavePermanentEstimation(obj, sLabourlist);

                            if (objSession.sTransactionLog == "1")
                            {
                                Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "(Estimation)PermanentDecommissioning");
                            }

                            if (Arr[1].ToString() == "0")
                            {
                                hdfWFDataId.Value = obj.sWFDataId;
                                ApproveRejectAction();
                                if (objSession.sTransactionLog == "1")
                                {
                                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "(Estimation)PermanentDecommissioning");
                                }
                                hdfWFOId.Value = obj.sWFDataId;

                                EstimationReport(hdfEstId.Value);
                                return;
                            }
                            if (Arr[1].ToString() == "2")
                            {
                                ShowMsgBox(Arr[0]);
                                return;
                            }
                        }

                        Arr = obj.SavePermanentEstimation(obj, sLabourlist);
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "(Estimation)PermanentDecommissioning");
                        }
                        hdfWFDataId.Value = Arr[2];
                        string estid = Arr[3];
                        EstimationReport(estid);
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

         public void EstimationReport(string EstimationId)
         {
             try
             {
               
                
                     if (EstimationId.Contains("-") || EstimationId.Contains("0"))
                        {
                         string sWFO_ID = hdfWFDataId.Value;
                         string strParam1 = string.Empty;
                         strParam1 = "id=RefinedpermanentEstimationSO&sWFOID=" + sWFO_ID + "&sDtrcode=" + txtTCCode.Text;
                         RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam1 + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                         return;

                     }

                     string strParam = string.Empty;
                     strParam = "id=RefinedpermanentEstimation&EstimationId=" + EstimationId;
                     RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                 

             }
             catch (Exception ex)
             {
                 lblMessage.Text = clsException.ErrorMsg();
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
                 
                 //objApproval.sApproveComments = hdfAppDesc.Value;
                 
                 objApproval.sBOId = "62";

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
                         hdfEstId.Value = objApproval.sRecordId;
                     }
                     else
                     {
                         hdfEstId.Value = objApproval.sNewRecordId;
                     }

                 }
                 else
                 {
                     bResult = objApproval.ApproveWFRequest(objApproval);
                     if (objApproval.sNewRecordId == null)
                     {
                         hdfEstId.Value = objApproval.sRecordId;
                     }
                     else
                     {
                         hdfEstId.Value = objApproval.sNewRecordId;
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
                     }
                     else if (objApproval.sApproveStatus == "2")
                     {
                         ShowMsgBox("Modified and Approved Successfully");
                         cmdSave.Enabled = false;
                         clsWorkOrder objWo = new clsWorkOrder();
                         objWo.sWFDataId = objApproval.sWFDataId;
                         objWo.sWFObjectId = objApproval.sWFObjectId;
                         objWo.sTaskType = txtType.Text;
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

             try
            {
                string sResult = string.Empty;

                DataTable dt = new DataTable();

                  if (cmbRepairer.SelectedIndex == 0)
                {
                    if (!(sFailType == "2"))
                    {
                        ShowMsgBox("Please Select the Repairer");
                        cmbRepairer.Focus();
                        return bValidate;
                    }
                }

                if (txtEstDate.Text == "")
                {
                    ShowMsgBox("Please Enter Estimation Date");
                    txtEstDate.Focus();
                    return bValidate;
                }

                // if (cmbFailType.SelectedIndex == 0)
                //{
                //    if (!(sFailType == "2"))
                //    {
                //        ShowMsgBox("Please Select the FailureType");
                //        cmbFailType.Focus();
                //        return bValidate;
                //    }
                //}

                  if (cmbGuarenteeType.SelectedValue == "0")
                {
                    if (!(sFailType == "2"))
                    {
                        ShowMsgBox("Please Select Guarantee Type");
                        cmbGuarenteeType.Focus();
                        return bValidate;
                    }
                }

                 if (RetriveFromXML == true && txtActiontype.Text == "A" || RetriveFromXML == true && txtActiontype.Text == "M")
                {
                    string sQuantity = string.Empty;
                    string sBaseRate = string.Empty;
                    string sTaxRate = string.Empty;
                    string sTotal = string.Empty;

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
                 }

                  if (RetriveFromXML == false)
                {
                    string sQuantity = string.Empty;

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
                }
                  if (sFailType != "2")
                  {
                      cmdCalc_Click(new object(), new EventArgs());
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

        protected void cmdCalc_Click(object sender, EventArgs e)
        {
            string sLabelQty = string.Empty;
         //   double MaterialTotal = 0;
            double LabourTotal = 0;
           // double SalvageTotal = 0;

            try
            {
                

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
                    lblTotalCharges.Text = Convert.ToString(LabourTotal);

                }

                grdLabourMast.FooterRow.Cells[9].Text = Convert.ToString(Math.Round(LabourTotal, 2));

                
                


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
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

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtComment.Text = string.Empty;
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
                 string sWoundType = cmbMaterialType.SelectedValue ;

                  if (cmbRepairer.SelectedValue != "--Select--")
                    {
                        clsPermanentEstimation objEst = new clsPermanentEstimation();
                        bool res = objEst.RepairDateIsValid(cmbRepairer.SelectedValue);

                        if (res == true)
                        {
                           // grdMaterialMast.Visible = true;
                            grdLabourMast.Visible = true;
                            //grdSalvageMast.Visible = true;
                          //  LoadMaterialDetails(sWoundType);
                            LoadLabourDetails(sWoundType);
                          //  LoadSalvageDetails(sWoundType);
                        }
                        else
                        {
                            ShowMsgBox("Effective Date Expired");
                        }
                    }

                 else
                    {
                        grdLabourMast.Visible = false;
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
                  if (Genaral.UrlDecrypt(Request.QueryString["ActionType"]) != "VIEW" && Genaral.UrlDecrypt(Request.QueryString["ActionType"]) !="")
                  {
                      Response.Redirect("/Approval/ApprovalInbox.aspx", false);
                  }
                  else
                  {
                      Response.Redirect("Permanentestimationview.aspx", false);
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

          public void GetFailId(string sFail_id)
          {
              try
              {
                  clsEstimation objEstmate = new clsEstimation();
                  string sFailId = objEstmate.GetFailId(sFail_id, "View");
                  //LoadFailedDetails(sFailId);
              }
              catch (Exception ex)
              {
                  lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
          }

    }
}

    

                  