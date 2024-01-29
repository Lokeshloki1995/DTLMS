using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IIITS.DTLMS.BL.TCRepair;
using IIITS.DTLMS.BL;
using System.Globalization;
using System.Data.OleDb;
using System.IO;
using System.Collections;

namespace IIITS.DTLMS.TCRepair
{
    public partial class RepairerWorkOrder : System.Web.UI.Page
    {
        string strFormCode = "RepairerWorkOrder";
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
                    Form.DefaultButton = cmdSave.UniqueID;
                    objSession = (clsSession)Session["clsSession"];
                    lblMessage.Text = string.Empty;
                    string sWo_id = string.Empty;
                    bool sViewRecord = false;
                    string sFailId = string.Empty;
                    txtRepdate.Attributes.Add("readonly", "readonly");
                    txtSSDate.Attributes.Add("readonly", "readonly");
                    CalendarExtender_txtRepdate.EndDate = System.DateTime.Now;
                    CalendarExtender_txtSSDate.EndDate = System.DateTime.Now;

                    if (!IsPostBack)
                    {
                        Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" from \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='I' ORDER BY \"MD_ORDER_BY\" ", "--Select--", cmbIssuedBy);
                        Genaral.Load_Combo("SELECT \"MD_NAME\",\"MD_NAME\" from \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='C' ORDER BY \"MD_ORDER_BY\" ", "--Select--", cmbCapacity);
                        if (HttpUtility.UrlDecode(Request.QueryString["FailType"]) == "" || HttpUtility.UrlDecode(Request.QueryString["FailType"]) == null)
                        {
                            txtFailType.Text = "";
                        }
                        else
                        {
                            txtFailType.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["FailType"]));

                        }
          

                        bool bAccResult = CheckAccessRights("2");
                        if (bAccResult == false)
                        {
                            return;
                        }

                        DisableCopy();

                        CalendarExtender_txtRepdate.EndDate = System.DateTime.Now;
                       

                        if (Request.QueryString["ActionType"] != null && Convert.ToString(Request.QueryString["ActionType"]) != "")
                        {
                            txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));
                        }
                        if (Request.QueryString["OfficeCode"] != null && Convert.ToString(Request.QueryString["OfficeCode"]) != "")
                        {
                            txtssOfficeCode.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["OfficeCode"]));
                        }

                        if (Request.QueryString["TypeValue"] != null && Convert.ToString(Request.QueryString["TypeValue"]) != "")
                        {
                            txtType.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TypeValue"]));
                            if (txtType.Text.Contains('~'))
                            {
                                hdfGuarenteeType.Value = txtType.Text.Split('~').GetValue(1).ToString();
                                txtType.Text = txtType.Text.Split('~').GetValue(0).ToString();
                            }
                            if (Convert.ToString(Request.QueryString["FailType"]) != null)
                            {
                                txtFailType.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["FailType"]));
                                if (txtFailType.Text == "1")
                                {
                                    txtFailureId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ReferID"]));
                                }
                            }
                            else
                            {
                                if (txtType.Text != "3")
                                {
                                    if (Request.QueryString["ReferID"] != null && Convert.ToString(Request.QueryString["ReferID"]) != "")
                                    {

                                        sWo_id = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ReferID"]));
                                        clsWorkOrder objWO = new clsWorkOrder();
                                        if (txtActiontype.Text == "V")
                                        {
                                            sWo_id = objWO.GetFailCoilType(sWo_id, txtActiontype.Text);
                                        }
                                        else
                                        {
                                            sWo_id = objWO.GetFailCoilType(sWo_id, txtActiontype.Text);
                                        }

                                        txtFailType.Text = sWo_id.Split('~').GetValue(0).ToString();
                                        sViewRecord = true;
                                    }
                                }
                            }
                            ChangeLabelText();
                            cmbIssuedBy.SelectedValue = "2";

                        }

                        if (Request.QueryString["ReferID"] != null && Convert.ToString(Request.QueryString["ReferID"]) != "")
                        {
                            if (txtType.Text != "3")
                            {
                                if (sViewRecord == true)
                                {
                                    sWo_id = sWo_id.Split('~').GetValue(1).ToString();
                                }
                                else
                                {
                                    sWo_id = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ReferID"]));
                                }
                            }


                          

                            if (txtType.Text == "3")
                            {
                                txtWOId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ReferID"]));
                                if (!txtWOId.Text.Contains("-"))
                                {
                                    GetWODetailsNewDTC();
                                    
                                }

                                cmdSave.Text = "View";
                            }
                            else if (txtType.Text == "1" && txtFailType.Text == "1")
                            {
                                if (sViewRecord == true)
                                {
                                    sFailId = sWo_id;
                                }
                                else
                                {
                                    sFailId = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ReferID"]));
                                }

                                if (txtActiontype.Text == "V")
                                {
                                    if (sFailId.Contains("-"))
                                    {
                                        GetFailureId(sFailId);
                                        dvSaleofscrap.Style.Add("display", "block");
                                    }
                                    else
                                    {
                                        GetFailureId(sFailId);
                                        GetWorkOrderDetails();
                                    }
                                }
                                else
                                {
                                    if (sFailId.Contains("-"))
                                    {
                                        GetFailureId(sFailId);
                                    }
                                    else
                                    {
                                        GetFailureDetails(sFailId, txtType.Text);
                                    }
                                }

                            }
                            else
                            {
                                if (sViewRecord == true)
                                {
                                    txtFailureId.Text = sWo_id;
                                    GetFailureDetails(txtFailureId.Text, txtType.Text);
                                    GetWorkOrderDetails();
                                }
                                else
                                {
                                    txtFailureId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ReferID"]));
                                    if (txtActiontype.Text == "V")
                                    {
                                        clsWorkOrder objworkOrder = new clsWorkOrder();
                                        if (!txtFailureId.Text.Contains('-'))
                                        {
                                            txtFailureId.Text = objworkOrder.getFailureId(txtFailureId.Text, "1");
                                            goto FailureDetails;
                                        }
                                        else if (txtFailureId.Text.Contains('-'))
                                        {
                                            txtFailureId.Text = objworkOrder.getFailureId(txtFailureId.Text, "2");
                                            goto FailureDetails;
                                        }
                                    }
                                FailureDetails:
                                    GetFailureDetails(txtFailureId.Text, txtType.Text);
                                }

                                cmbCapacity.Enabled = false;
                               
                               
                                cmdSearch_Click(sender, e);
                                cmbCapacity_SelectedIndexChanged(sender, e);


                            }

                           
                        }


                       
                        WorkFlowConfig();
                        if (objSession.sRoleType == "1")
                        {
                           
                           
                                Session["BOID"] = "72";
                                ViewState["BOID"] = "72";
                            
                        }
                        else
                        {
                            ViewState["BOID"] = Convert.ToString(Session["BOID"]);
                        }


                    }
                    ApprovalHistoryView.BOID = Convert.ToString(ViewState["BOID"]);
                    if (Convert.ToString(Request.QueryString["sWoRecordID"]) != null && Convert.ToString(Request.QueryString["sWoRecordID"]) != "")
                    {
                        if (Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["sWoRecordID"])).Contains('-'))
                        {
                            ApprovalHistoryView.sRecordId = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["sWoRecordID"]));
                        }
                        else
                        {
                            ApprovalHistoryView.sRecordId = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["sWoRecordID"]));
                        }
                    }
                    else
                    {
                        ApprovalHistoryView.sRecordId = sWo_id;
                    }


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

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }
        protected void ChkSaleOfScapCheck_CheckedChanged(object sender, EventArgs e)
        {
            string Off_Code = string.Empty;
            try
            {
                if (ChkSSCheck.Checked == true)
                {
                    // dvDecomm.Style.Add("class", "span6");
                    //dvComm.Attributes.Add("class", "span6");
                    dvSaleofscrap.Visible = true;
                    dvSaleofscrap.Style.Add("display", "block");
                    //if (ChkCredit.Checked == true)
                    //{
                    dvSaleofscrap.Attributes.Add("class", "span11");
                    //  //  divCreditWO.Attributes.Add("class", "span6");
                    //}
                    //else
                    //{
                    //    dvSaleofscrap.Attributes.Add("class", "span11");
                    //}

                    //dvBasic.Style.Add("display", "none");
                    //dvSection.Style.Add("display", "none");
                    //dvRepairer.Style.Add("display", "block");
                    //if (objSession.OfficeCode.Length > Constants.Division)
                    //{
                    //    Off_Code = objSession.OfficeCode.Substring(0, Constants.Division);
                    //}
                    //else
                    //    Off_Code = objSession.OfficeCode;
                    //Genaral.Load_Combo("SELECT \"TR_ID\", UPPER(\"TR_NAME\") FROM \"TBLTRANSREPAIRER\",\"TBLTRANSREPAIREROFFCODE\" WHERE \"TR_ID\"=\"TRO_TR_ID\" AND \"TRO_OFF_CODE\" =  '" + Off_Code + "' ORDER BY \"TR_NAME\" ", "--Select--", cmbRepairer);
                    //lnkDTCDetails.Visible = false;
                    //lnkDTrDetails.Visible = false;
                    //cmbRepairer.Enabled = true;
                    //cmdViewEstimate.Visible = false;
                }
                else
                {
                    dvSaleofscrap.Visible = false;
                    //  divCreditWO.Attributes.Add("class", "span11");
                    //dvComm.Attributes.Add("class", "span12");
                    dvSaleofscrap.Attributes.Add("display", "none");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetFailureDetails(string sFailId, string FailType = "")
        {
            DataTable dtWODetails = new DataTable();
            try
            {
                ClsRepairerWorkorder objWorkOrder = new ClsRepairerWorkorder();

                if (Convert.ToString(Request.QueryString["sWoRecordID"]) != null && Convert.ToString(Request.QueryString["sWoRecordID"]) != "")
                {
                    string sWoRecordID = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["sWoRecordID"]));
                    if (sWoRecordID.Contains('-'))
                    {
                        dtWODetails = objWorkOrder.FailDetails(sFailId, FailType, txtFailType.Text, txtActiontype.Text, hdfGuarenteeType.Value, sWoRecordID);
                    }
                    else
                    {
                        dtWODetails = objWorkOrder.FailDetails(sFailId, FailType, txtFailType.Text, txtActiontype.Text, hdfGuarenteeType.Value, sWoRecordID);
                    }
                }
                else
                {
                    dtWODetails = objWorkOrder.FailDetails(sFailId, FailType, txtFailType.Text, txtActiontype.Text, hdfGuarenteeType.Value);
                }

                if (dtWODetails.Rows.Count > 0)
                {
                    cmbCapacity.SelectedValue = Convert.ToString(dtWODetails.Rows[0]["RESTD_CAPACITY"]);
                }

                if (FailType != "2" && txtFailType.Text != "2")
                {
                    if (dtWODetails.Rows.Count > 0)
                    {
                        //cmbRepairer.SelectedValue = Convert.ToString(dtWODetails.Rows[0]["TR_ID"]);
                    }
                }
                else
                    cmbRepairer.SelectedValue = "0";
                if (dtWODetails.Rows.Count > 0)
                {
                    txtFailureId.Text = Convert.ToString(dtWODetails.Rows[0]["RESTD_ID"]);
                   
                    txtCapacity.Text = Convert.ToString(dtWODetails.Rows[0]["RESTD_CAPACITY"]);
                    txtFailType.Text = Convert.ToString(dtWODetails.Rows[0]["RESTD_FAIL_TYPE"]);
                    txtTCCode.Text = Convert.ToString(dtWODetails.Rows[0]["RESTD_TC_CODE"]);
                    txtAcCode.Text = "74-116";
                    cmbCapacity.Enabled = false;
                    objWorkOrder.sCapacity = cmbCapacity.Text;
                    objWorkOrder.GetCommDecommAccCode(objWorkOrder);
                    txtSSAccCode.Text = objWorkOrder.sScrapAccCode;

                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetFailureId(string sFailId)
        {
            DataTable dtWODetails = new DataTable();
            string sFailure_id = string.Empty;
            try
            {
                ClsRepairerWorkorder objWorkOrder = new ClsRepairerWorkorder();
                if (Request.QueryString["sWoRecordID"] != null && Convert.ToString(Request.QueryString["sWoRecordID"]) != "")
                {
                    string sWoRecordID = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["sWoRecordID"]));
                    if (sWoRecordID.Contains('-'))
                    {
                        sFailure_id = objWorkOrder.FailureId(sFailId, sWoRecordID);
                    }
                    else
                    {
                        sFailure_id = objWorkOrder.FailureId(sFailId);
                    }
                }
                else
                {
                    sFailure_id = objWorkOrder.FailureId(sFailId);
                }

                txtFailureId.Text = sFailure_id;
                
                GetFailureDetails(sFailure_id);
               

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void DisableCopy()
        {
            try
            {
                txtRepWoNo1.Attributes.Add("onmousedown", "return noCopyMouse(event);");
                txtRepWoNo2.Attributes.Add("onmousedown", "return noCopyMouse(event);");
                txtRepWoNo3.Attributes.Add("onmousedown", "return noCopyMouse(event);");


                txtRepWoNo1.Attributes.Add("onkeydown", "return noCopyKey(event);");
                txtRepWoNo2.Attributes.Add("onkeydown", "return noCopyKey(event);");
                txtRepWoNo3.Attributes.Add("onkeydown", "return noCopyKey(event);");

                txtSSWoNo1.Attributes.Add("onmousedown", "return noCopyMouse(event);");
                txtSSWoNo2.Attributes.Add("onmousedown", "return noCopyMouse(event);");
                txtSSWoNo3.Attributes.Add("onmousedown", "return noCopyMouse(event);");


                txtSSWoNo1.Attributes.Add("onkeydown", "return noCopyKey(event);");
                txtSSWoNo2.Attributes.Add("onkeydown", "return noCopyKey(event);");
                txtSSWoNo3.Attributes.Add("onkeydown", "return noCopyKey(event);");

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
                string sWoRecordID = string.Empty;
                ClsRepairerWorkorder objWorkOrder = new ClsRepairerWorkorder();
                objWorkOrder.sEstId = txtFailureId.Text;

                if (Convert.ToString(Request.QueryString["sWoRecordID"]) != null && Convert.ToString(Request.QueryString["sWoRecordID"]) != "")
                {
                    sWoRecordID = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["sWoRecordID"]));
                }

                if ((sWoRecordID != null && sWoRecordID != "") && !sWoRecordID.Contains('-'))
                {
                    objWorkOrder.GetWorkOrderDetails(objWorkOrder);

                    txtWOId.Text = objWorkOrder.sWOId;
                    txtFailureId.Text = objWorkOrder.sEstId;
                   
                    hdfFailureId.Value = objWorkOrder.sEstId;
                    if (txtType.Text == "2" || txtType.Text == "4")
                    {
                        cmbCapacity.SelectedValue = objWorkOrder.sEnhancedCapacity;
                    }
                    else
                        cmbCapacity.SelectedValue = objWorkOrder.sCapacity;

                    if (objWorkOrder.sCommWoNo != null && objWorkOrder.sCommWoNo != "0")
                    {
                        cmbIssuedBy.SelectedValue = objWorkOrder.sIssuedBy;
                        txtAcCode.Text = objWorkOrder.sAccCode;


                        txtRepWoNo1.Text = objWorkOrder.sCommWoNo.Split('/').GetValue(2).ToString();
                        txtRepWoNo1.Text = objWorkOrder.sCommWoNo.Split('/').GetValue(3).ToString();
                        txtRepWoNo1.Text = objWorkOrder.sCommWoNo.Split('/').GetValue(4).ToString();

                        
                        txtRepdate.Text = objWorkOrder.sCommDate;
                        txtRepAmount.Text = objWorkOrder.sCommAmmount;

                        cmbCapacity.SelectedValue = objWorkOrder.sNewCapacity;

                       

                        if (txtType.Text == "3")
                        {
                            //cmbSection.SelectedValue = objWorkOrder.sRequestLoc;
                        }
                        
                        cmbRepairer.SelectedValue = objWorkOrder.sRepairer;

                        //cmdSave.Text = "Update";
                        txtFailureId.Enabled = false;
                       
                    }

                    else
                    {
                        //cmdSave.Text = "Save";
                        txtFailureId.Enabled = false;
                       
                    }
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        public void GetWODetailsNewDTC()
        {
            try
            {
                clsWorkOrder objWorkOrder = new clsWorkOrder();
                objWorkOrder.sWOId = txtWOId.Text;
                objWorkOrder.GetWODetailsForNewDTC(objWorkOrder);

                cmbIssuedBy.SelectedValue = objWorkOrder.sIssuedBy;
                txtAcCode.Text = objWorkOrder.sAccCode;

                if (objWorkOrder.sCommWoNo != null)
                {
                    txtRepWoNo1.Text = objWorkOrder.sCommWoNo.Split('/').GetValue(2).ToString();
                    txtRepWoNo1.Text = objWorkOrder.sCommWoNo.Split('/').GetValue(3).ToString();
                    txtRepWoNo1.Text = objWorkOrder.sCommWoNo.Split('/').GetValue(4).ToString();
                }
                //txtComWoNo.Text = objWorkOrder.sCommWoNo;
                txtRepdate.Text = objWorkOrder.sCommDate;
                txtRepAmount.Text = objWorkOrder.sCommAmmount;
                cmbCapacity.SelectedValue = objWorkOrder.sNewCapacity;
                
                
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

        public bool ValidateForm()
        {
            bool bValidate = false;
            string estimatedate = string.Empty;

            try
            {
                if (txtType.Text != "3" && txtFailType.Text != "1")
                {
                    //if (txtFailureId.Text.Trim().Length == 0)
                    //{
                    //    txtFailureId.Focus();
                    //    ShowMsgBox("Enter Failure Id");
                    //    return bValidate;
                    //}
                }

                if (cmbIssuedBy.SelectedIndex == 0)
                {
                    cmbIssuedBy.Focus();
                    ShowMsgBox("Select Issued By");
                    return bValidate;
                }
                if (cmbCapacity.SelectedIndex == 0)
                {
                    cmbCapacity.Focus();
                    ShowMsgBox("Select Capacity");
                    return bValidate;
                }

              

                if (txtRepWoNo1.Text.Trim().Length == 0 || txtRepWoNo2.Text.Trim().Length == 0 || txtRepWoNo3.Text.Trim().Length == 0)
                {
                    //txtComWoNo.Focus();
                    ShowMsgBox("Enter the WO Number");
                                                                         
                    if (txtRepWoNo1.Text.Trim().Length == 0)
                    {
                        txtRepWoNo1.Focus();
                    }
                    else if (txtRepWoNo2.Text.Trim().Length == 0)
                    {
                        txtRepWoNo2.Focus();
                    }
                    else
                    {
                        txtRepWoNo3.Focus();
                    }
                    return bValidate;
                }

                if (txtRepdate.Text.Trim() == "")
                {
                    txtRepdate.Focus();
                    ShowMsgBox("Enter Repairer Date");
                    return bValidate;
                }
                if (txtType.Text != "3")
                {
                    clsWorkOrder objwo = new clsWorkOrder();
                    estimatedate = objwo.getestimatedate(txtFailureId.Text);
                }
                if (txtType.Text != "3")
                {

                    if (txtRepdate.Text != "" && txtRepdate.Text != null && txtRepdate.Text != "0")
                    {
                        string sResult = Genaral.DateComparisionTransaction(txtRepdate.Text, estimatedate, false, false);
                        if (sResult == "1")
                        {
                            ShowMsgBox(" Work Order Date should be Greater than or Equal to Estimation Date");
                            return bValidate;
                        }
                    }
                }

                if (txtRepAmount.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Enter the Repairer Amount");
                    txtRepAmount.Focus();
                    return bValidate;
                }
                //if (txtInncured.Text.Trim().Length == 0)
                //{
                //    ShowMsgBox("Enter the Inncured Amount");
                //    txtInncured.Focus();
                //    return bValidate;
                //}
                if (txtAcCode.Text.Trim().Length == 0)
                {
                    txtAcCode.Focus();
                    ShowMsgBox("Enter  Account Code");
                    return bValidate;
                }

                

                if (!System.Text.RegularExpressions.Regex.IsMatch(txtRepAmount.Text, "^(\\d{1,8})?(\\.\\d{1,2})?$"))
                {
                    ShowMsgBox("Enter Valid  Amount (eg:111111.00)");
                    return false;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtInncured.Text, "^(\\d{1,8})?(\\.\\d{1,2})?$"))
                {
                    ShowMsgBox("Enter Valid  Amount (eg:111111.00)");
                    return false;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(txtRepAmount.Text, "[-+]?[0-9]{0,7}\\.?[0-9]{0,2}"))
                {
                    ShowMsgBox("Enter Valid Commissioning Amount (eg:111111.00)");
                    return false;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtInncured.Text, "[-+]?[0-9]{0,7}\\.?[0-9]{0,2}"))
                {
                    ShowMsgBox("Enter Valid Inncured Amount (eg:111111.00)");
                    return false;
                }

                if (ChkSSCheck.Checked == true)
                {

                    if (txtSSWoNo1.Text.Trim().Length == 0 || txtSSWoNo2.Text.Trim().Length == 0 || txtSSWoNo3.Text.Trim().Length == 0)
                    {                    
                        ShowMsgBox("Enter the Sale of scrap WO Number");
                        txtSSWoNo1.Focus();
                        if (txtSSWoNo1.Text.Trim().Length == 0)
                        {
                            txtSSWoNo1.Focus();
                        }
                        else if (txtSSWoNo2.Text.Trim().Length == 0)
                        {
                            txtSSWoNo2.Focus();
                        }
                        else
                        {
                            txtSSWoNo3.Focus();
                        }                     
                        return bValidate;
                    }
                    if (txtSSDate.Text.Trim() == "")
                    {
                        txtSSDate.Focus();
                        ShowMsgBox("Enter Sale of scrap Date");
                        return bValidate;
                    }
                    if (txtSSDate.Text != "" && txtSSDate.Text != null && txtSSDate.Text != "0")
                    {
                        string sResult = Genaral.DateComparisionTransaction(txtSSDate.Text, estimatedate, false, false);
                        if (sResult == "1")
                        {
                            ShowMsgBox("Sale of scrap Work Order Date should be Greater than or Equal to Estimation Date ");
                            txtSSDate.Focus();
                            return bValidate;
                        }
                    }
                    if (txtSSAccCode.Text == "" || txtSSAccCode.Text == null)
                    {
                        ShowMsgBox("Enter sale of scrap Account code ");
                        txtSSAccCode.Focus();
                        return bValidate;
                    }
                    if (txtSSAmount.Text == "" || txtSSAmount.Text == null)
                    {
                        ShowMsgBox("Enter sale of scrap amount ");
                        txtSSAmount.Focus();
                        return bValidate;
                    }
                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtSSAmount.Text, "[-+]?[0-9]{0,7}\\.?[0-9]{0,2}"))
                    {
                        ShowMsgBox("Enter Valid  sale of scrap Amount (eg:111111.00)");
                        txtSSAmount.Focus();
                        return bValidate;
                    }
                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtSSAmount.Text, "^(\\d{1,8})?(\\.\\d{1,2})?$"))
                    {
                        ShowMsgBox("Enter Valid  Amount (eg:111111.00)");
                        txtSSAmount.Focus();
                        return bValidate;
                    }
                    
                }

                //  Photo Save DTLMSDocs
                string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["NotAllowFileFormat"]);

                if (fupWODocument.PostedFile.ContentLength != 0)
                {

                    string sWOFileExt = string.Empty;
                    string sWOFileName = string.Empty;
                    string sDirectory = string.Empty;


                    sWOFileExt = System.IO.Path.GetExtension(fupWODocument.FileName).ToString().ToLower();
                    sWOFileExt = ";" + sWOFileExt.Remove(0, 1) + ";";

                    if (sFileExt.Contains(sWOFileExt))
                    {
                        ShowMsgBox("Invalid File Format");
                        return false;
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


        public void Reset()
        {
            try
            {
               

                txtWOId.Text = string.Empty;


                txtRepWoNo1.Text = string.Empty;
                txtRepWoNo2.Text = string.Empty;
                txtRepWoNo3.Text = string.Empty;

                txtRepdate.Text = string.Empty;
                txtRepAmount.Text = string.Empty;

                txtSSWoNo1.Text = string.Empty;
                txtSSWoNo2.Text = string.Empty;
                txtSSWoNo3.Text = string.Empty;

                txtSSAmount.Text = string.Empty;
                txtSSDate.Text = string.Empty;

                txtInncured.Text = string.Empty;

                hdfFailureId.Value = string.Empty;
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
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                string[] Arr = new string[2];
                ClsRepairerWorkorder objWorkOrder = new ClsRepairerWorkorder();
                DataTable dtOffName = new DataTable();
                //Check AccessRights
                bool bAccResult = true;
                if (cmdSave.Text == "Update")
                {
                    bAccResult = CheckAccessRights("3");
                }
                else if (cmdSave.Text == "Save" || cmdSave.Text == "Submit" || cmdSave.Text == "Approve")
                {
                    bAccResult = CheckAccessRights("2");
                }

                if (bAccResult == false)
                {
                    return;
                }

                if (cmdSave.Text == "View")
                {
                    ClsRepairerWorkorder objWrkOrder = new ClsRepairerWorkorder();
                    if (hdfApproveStatus.Value != "")
                    {

                        if (hdfApproveStatus.Value != "3")
                        {
                            objWrkOrder.sWFDataId = hdfWFDataId.Value;
                            objWrkOrder.sTaskType = txtType.Text;
                            if (txtType.Text == "3")
                            {
                            }
                            else
                            {
                                GenerateWorkOrderReport(objWrkOrder);
                            }
                        }
                    }
                    else
                    {
                        if (txtType.Text == "1" || txtType.Text == "2")
                        {
                            objWrkOrder.sWFDataId = objWorkOrder.getWoDataId(txtFailureId.Text);
                            objWrkOrder.sTaskType = txtType.Text;
                            GenerateWorkOrderReport(objWrkOrder);
                        }
                        else
                            if (txtType.Text == "3")
                        {
                            objWrkOrder.sWOId = txtWOId.Text;
                            objWrkOrder.sTaskType = txtType.Text;

                            if (txtType.Text == "3")
                            {
                            }
                            else
                            {
                                GenerateWorkOrderReport(objWrkOrder);
                            }
                        }
                        else
                        {
                            objWrkOrder.sWFDataId = objWorkOrder.getWoDataId(txtFailureId.Text);
                            objWrkOrder.sWOId = txtWOId.Text;
                            objWrkOrder.sTaskType = txtType.Text;
                            GenerateWorkOrderReport(objWrkOrder);
                        }
                    }

                    return;
                }

                if (ValidateForm() == true)
                {
                    objWorkOrder.sWOId = txtWOId.Text;

                    objWorkOrder.sEstId = txtFailureId.Text.Trim();
                   
                    objWorkOrder.sIssuedBy = cmbIssuedBy.SelectedValue;
                    objWorkOrder.sGuarentyType = hdfGuarenteeType.Value;
                    if (txtCapacity.Text != "")
                    {
                        objWorkOrder.sCapacity = txtCapacity.Text.Trim();
                    }
                    else
                    {
                        objWorkOrder.sCapacity = "0";
                    }
                    
                    objWorkOrder.sFailType = txtFailType.Text.Trim();

                    objWorkOrder.sNewCapacity = cmbCapacity.SelectedValue.Trim();

                    if (txtFailureId.Text != "")
                    {
                        dtOffName = objWorkOrder.GetofficeName(txtFailureId.Text);
                    }
                    else
                    {
                        dtOffName = objWorkOrder.GetofficeNameBySectionCode(objSession.OfficeCode);
                    }

                    string sCommWONo = dtOffName.Rows[0]["DIV"].ToString() + "/" + dtOffName.Rows[0]["SUBDIV"].ToString() + "/" + txtRepWoNo1.Text.Trim().Replace("'", "") + "/" + txtRepWoNo2.Text.Trim().Replace("'", "") + "/" + txtRepWoNo3.Text.Trim().Replace("'", "");
                    objWorkOrder.sCommWoNo = sCommWONo.Trim().ToUpper();
                    objWorkOrder.sCommDate = txtRepdate.Text.Trim().Replace("'", "");
                    objWorkOrder.sCommAmmount = txtRepAmount.Text.Trim().Replace("'", "");
                    if(txtInncured.Text!="" && txtInncured.Text!=null)
                    {
                        objWorkOrder.sInncuredcost = txtInncured.Text.Trim().Replace("'", "");
                    }
                    else
                    {
                        objWorkOrder.sInncuredcost = "0";
                    }
                                  
                    objWorkOrder.sAccCode = txtAcCode.Text.Trim().Replace("'", "");


                    objWorkOrder.sCrBy = objSession.UserId;
                    objWorkOrder.sLocationCode = objSession.OfficeCode;
                    objWorkOrder.sTaskType = txtType.Text;
                   
                    objWorkOrder.sRepairer = cmbRepairer.SelectedValue;
                    if (txtType.Text == "3")
                    {
                       // objWorkOrder.sRequestLoc = cmbSection.SelectedValue.Trim();
                    }

                    if (fupWODocument.PostedFile.ContentLength != 0)
                    {

                        string sWOFileName = System.IO.Path.GetFileName(fupWODocument.PostedFile.FileName);

                        fupWODocument.SaveAs(Server.MapPath("~/DTLMSFiles" + "/" + sWOFileName));
                        string sDirectory = Server.MapPath("~/DTLMSFiles" + "/" + sWOFileName);
                        objWorkOrder.sWOFilePath = sDirectory;
                    }

                    #region Approve And Reject

                    if (txtActiontype.Text == "A" || txtActiontype.Text == "R")
                    {
                        if (hdfWFDataId.Value != "0")
                        {
                            ApproveRejectAction();

                            if (objSession.sTransactionLog == "1")
                            {
                                Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (RepairerWorkorder)  ");
                            }

                            return;
                        }
                    }

                    #endregion

                    //Workflow
                    WorkFlowObjects(objWorkOrder);

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
                        objWorkOrder.sWOId = "";
                        objWorkOrder.sActionType = txtActiontype.Text;
                        objWorkOrder.sCrBy = hdfCrBy.Value;
                        objWorkOrder.sboid = Convert.ToString(Session["BOID"]);
                        objWorkOrder.sTCCode = txtTCCode.Text;
                        if (ChkSSCheck.Checked == true)
                        {
                            objWorkOrder.scrapCheck = true;
                            string sScrapWONo = dtOffName.Rows[0]["DIV"].ToString() + "/" + dtOffName.Rows[0]["SUBDIV"].ToString() + "/" + txtSSWoNo1.Text.Trim().Replace("'", "") + "/" + txtSSWoNo2.Text.Trim().Replace("'", "") + "/" + txtSSWoNo3.Text.Trim().Replace("'", "");
                            objWorkOrder.sScrapWoNo = sScrapWONo.Trim().ToUpper();
                            objWorkOrder.sScrapAccCode = txtSSAccCode.Text.Trim().Replace("'", "");
                            objWorkOrder.sScrapDate = txtSSDate.Text.Trim().Replace("'", "");
                            objWorkOrder.sScrapAmmount = txtSSAmount.Text.Trim().Replace("'", "");
                        }
                        else
                        {
                            objWorkOrder.scrapCheck = false;
                        }
                        Arr = objWorkOrder.SaveUpdateWorkOrder(objWorkOrder);
                        if (Arr[1].ToString() == "0")
                        {
                            hdfWFDataId.Value = objWorkOrder.sWFDataId;
                            hdfAppDesc.Value = objWorkOrder.sApprovalDesc;
                            hdfboid.Value = Arr[2];
                            ApproveRejectAction();
                            return;
                        }
                        if (Arr[1].ToString() == "2")
                        {
                            ShowMsgBox(Arr[0]);
                            return;
                        }
                    }

                    #endregion
                    if (ChkSSCheck.Checked == true)
                    {
                        objWorkOrder.scrapCheck = true;
                        string sScrapWONo = dtOffName.Rows[0]["DIV"].ToString() + "/" + dtOffName.Rows[0]["SUBDIV"].ToString() + "/" + txtSSWoNo1.Text.Trim().Replace("'", "") + "/" + txtSSWoNo2.Text.Trim().Replace("'", "") + "/" + txtSSWoNo3.Text.Trim().Replace("'", "");
                        objWorkOrder.sScrapWoNo = sScrapWONo.Trim().ToUpper();
                        objWorkOrder.sScrapAccCode = txtSSAccCode.Text.Trim().Replace("'", "");
                        objWorkOrder.sScrapDate= txtSSDate.Text.Trim().Replace("'", "");
                        objWorkOrder.sScrapAmmount = txtSSAmount.Text.Trim().Replace("'", "");
                         
                    }
                    else
                    {
                        objWorkOrder.scrapCheck = false;
                    }
                    objWorkOrder.sboid = Convert.ToString(Session["BOID"]);
                    objWorkOrder.sTCCode = txtTCCode.Text;
                    Arr = objWorkOrder.SaveUpdateWorkOrder(objWorkOrder);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (RepairerWorkorder)  ");
                    }

                    if (Arr[1].ToString() == "0")
                    {
                        ShowMsgBox(Arr[0]);
                        txtWOId.Text = objWorkOrder.sWOId;
                        //ApproveRejectAction();
                        cmdSave.Text = "Update";
                        cmdSave.Enabled = false;
                        if (txtType.Text == "3")
                        {
                        }
                        else
                        {
                            GenerateWorkOrderReport(objWorkOrder);
                        }
                        return;
                    }
                    if (Arr[1].ToString() == "1")
                    {

                        if (txtActiontype.Text == "A" || txtActiontype.Text == "R")
                        {
                            ApproveRejectAction();
                            if (objSession.sTransactionLog == "1")
                            {
                                Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (RepairerWorkorder) ");
                            }
                            return;
                        }

                        if (txtActiontype.Text == "M")
                        {
                            ShowMsgBox("Modified and Approved Successfully");
                        }
                        else
                        {
                            ShowMsgBox(Arr[0]);
                        }
                        return;
                    }
                    if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0]);
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



        private void GenerateWorkOrderReport(ClsRepairerWorkorder objWorkOrder)
        {
            string strParam = string.Empty;
            string sAET = string.Empty;
            string sSTO = string.Empty;
            string sAO = string.Empty;
            string sDo = string.Empty;
            string sOffcName = string.Empty;
            string sLevelOfApproval = string.Empty;
            ArrayList sNameList = new ArrayList();
            string soffCode = objSession.OfficeCode;
            if (objSession.OfficeCode.Length > 3)
            {
                ClsRepairerWorkorder objOffName = new ClsRepairerWorkorder();
                sOffcName = objOffName.getofficeName(objSession.OfficeCode);
            }
            else
            {
                sOffcName = objSession.OfficeName;
            }

            //sLevelOfApproval = objWorkOrder.getLevelOfApproval(objWorkOrder, txtFailureId.Text, soffCode);
            sLevelOfApproval = Convert.ToString(getApprovalLevel());
            sNameList = objWorkOrder.getCreatedByUserName(txtFailureId.Text, soffCode);
            string sSubDiv = objWorkOrder.getsubdivName(txtFailureId.Text);
            string sWFDataId = objWorkOrder.sWFDataId;
            string sWoId = objWorkOrder.sWOId;
            string sTaskType = objWorkOrder.sTaskType;
            string sIncuredcost = txtInncured.Text;
            

            Session["UserNameList"] = sNameList;
            strParam = "id=WorkOrderPreviewRepairer&WFDataId=" + sWFDataId + "&LApprovel=" + sLevelOfApproval + "&OffCode=" + sOffcName + "&TaskType=" + sTaskType + "&WoId=" + sWoId + "&sSubDivName=" + sSubDiv + "&sIncuredcost=" + sIncuredcost ;
            RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
        }




        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                Reset();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("/Approval/ApprovalInbox.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void ValidateFormUpdate()
        {
            try
            {
                ClsRepairerWorkorder objWorkOrder = new ClsRepairerWorkorder();

                //if (objWorkOrder.ValidateUpdate(txtFailureId.Text, txtWOId.Text, txtType.Text) == true)
                //{
                //    cmdReset.Enabled = false;
                //    cmdSave.Enabled = false;
                //}
                //else
                //{
                    cmdReset.Enabled = true;
                    cmdSave.Enabled = true;
                //}


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
                ClsRepairerEstimate objFailure = new ClsRepairerEstimate();
                ClsRepairerWorkorder objwrkrder = new ClsRepairerWorkorder();
                if ((txtFailType.Text == "2" && txtActiontype.Text == "A") || (txtFailType.Text == "1" && txtActiontype.Text == "A"))
                {

                }
                else if (txtFailType.Text == "1" && txtActiontype.Text == "V")
                {

                }
                else
                {
                    if (hdfFailureId.Value != "")
                    {
                        txtFailureId.Text = hdfFailureId.Value;
                    }

                }


                objFailure.sFailureId = txtFailureId.Text;





              //  objFailure.GetFailureDetails(objFailure);

                
                txtDeclaredBy.Text = objFailure.sCrby;
                txtTCCode.Text = objFailure.sDtcTcCode;
                //cmbCapacity.SelectedValue = objFailure.;
                txtDTCId.Text = objFailure.sDtcId;
                txtTCId.Text = objFailure.sTCId;
                //if (cmbCapacity.SelectedIndex == 0)
                //{
                //txtCapacity.Text = objFailure.sDtcCapacity;

                if (txtType.Text == "1")
                {
                    txtCapacity.Text = objFailure.sDtcCapacity;
                    cmbCapacity.SelectedValue = objFailure.sDtcCapacity;
                }
                else
                {
                    txtCapacity.Text = objFailure.sDtcCapacity;
                   // cmbCapacity.SelectedValue = objFailure.sEnhancedCapacity;
                }



            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void ChangeLabelText()
        {
            string Off_Code = string.Empty;
            try
            {
                // txtType = 1 -> singleCoil, 2 -> MultiCoil
                // txtFailType = 1 -> Failure , 2 -> Enhance , 4 -> Failure with Enhance
                if (ChkSSCheck.Checked == true)
                {
                    dvSaleofscrap.Visible = true;
                    dvSaleofscrap.Style.Add("display", "block");
                }
                else
                {
                    dvSaleofscrap.Style.Add("display", "none");
                }

                if ((txtType.Text == "1" && txtFailType.Text != "1") || (txtType.Text == "4" && txtFailType.Text != "1"))
                {
                    lblIDText.Text = "Failure ID";                   
                }
                
                else if (txtType.Text == "1" && txtFailType.Text == "1")
                {

                    dvComm.Attributes.Add("class", "span12");
                    dvBasic.Style.Add("display", "none");

                    dvRepairer.Style.Add("display", "block");

                    if (objSession.OfficeCode.Length > Constants.Division)
                    {
                        if(objSession.RoleId=="12")
                        {
                            Off_Code = txtssOfficeCode.Text.Substring(0, Constants.Division);
                        }
                        else
                        {
                            Off_Code = objSession.OfficeCode.Substring(0, Constants.Division);
                        }
                       // Off_Code = objSession.OfficeCode.Substring(0, Constants.Division);
                    }
                    else
                    {
                        if(objSession.RoleId=="12")
                        {
                            Off_Code = txtssOfficeCode.Text.Substring(0, Constants.Division);
                        }
                        else
                        {
                            Off_Code = objSession.OfficeCode;
                        }
                      //  Off_Code = objSession.OfficeCode;

                    }
                   // Off_Code = objSession.OfficeCode;
                    Genaral.Load_Combo("SELECT \"TR_ID\", UPPER(\"TR_NAME\") FROM \"TBLTRANSREPAIRER\",\"TBLTRANSREPAIREROFFCODE\" WHERE \"TR_ID\"=\"TRO_TR_ID\" AND \"TRO_OFF_CODE\" =  '" + Off_Code + "' ORDER BY \"TR_NAME\" ", "--Select--", cmbRepairer);

                    lnkDTrDetails.Visible = false;
                    cmbRepairer.Enabled = true;
                    cmdViewEstimate.Visible = true;

                }
                else
                {

                    dvComm.Attributes.Add("class", "span12");
                    dvBasic.Style.Add("display", "none");

                    if (objSession.sRoleType == "2")
                    {
                        // Genaral.Load_Combo("SELECT \"OM_CODE\", \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE " + clsStoreOffice.GetOfficeCode(objSession.OfficeCode, "OM_CODE") + " ORDER BY \"OM_CODE\" ", "--Select--", cmbSection);
                    }
                    else
                    {
                        //  Genaral.Load_Combo("SELECT \"OM_CODE\", \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE CAST(\"OM_CODE\" AS TEXT) LIKE '" + objSession.OfficeCode + "%' ORDER BY \"OM_CODE\" ", "--Select--", cmbSection);
                    }


                    lnkDTrDetails.Visible = false;

                    cmdViewEstimate.Visible = false;
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

              
                    objApproval.sFormName = "RepairerWorkOrder";
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
                        Response.Redirect("~/UserRestrict.aspx", false);
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

        public void WorkFlowObjects(ClsRepairerWorkorder objWorkOrder)
        {
            try
            {
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


                objWorkOrder.sFormName = "RepairerWorkOrder";
                objWorkOrder.sOfficeCode = objSession.OfficeCode;
                objWorkOrder.sClientIP = sClientIP;
                objWorkOrder.sWFOId = hdfWFOId.Value;
                objWorkOrder.sWFAutoId = hdfWFOAutoId.Value;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        #region Workflow/Approval

        public void SetControlText()
        {
            try
            {
                if (txtActiontype.Text == "")
                {
                    txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));
                }
                else
                {
                }

                if (txtActiontype.Text == "A")
                {
                    cmdSave.Text = "Approve";
                    pnlApproval.Enabled = false;
                    cmdReset.Enabled = true;
                   
                }
                if (txtActiontype.Text == "R")
                {
                    cmdSave.Text = "Reject";
                    pnlApproval.Enabled = false;
                    cmbRepairer.Enabled = false;
                    cmdReset.Enabled = false;
                }
                if (txtActiontype.Text == "M")
                {
                    cmdSave.Text = "Modify and Approve";
                    pnlApproval.Enabled = true;
                    cmbRepairer.Enabled = true;
                    cmdReset.Enabled = true;
                }
                if (hdfWFOAutoId.Value == "0")
                {
                    dvComments.Style.Add("display", "block");
                    cmdReset.Enabled = true;
                }

                if (hdfWFOAutoId.Value != "0")
                {
                    dvComments.Style.Add("display", "none");
                    cmdReset.Enabled = true;
                    pnlApproval.Enabled = true;
                    
                }

                // Check for Creator of Form
                bool bResult = CheckFormCreatorLevel();
                if (bResult == true)
                {

                    pnlApproval.Enabled = true;
                    
                    if (txtActiontype.Text == "A" && hdfWFOAutoId.Value == "0")
                    {
                        txtActiontype.Text = "M";
                        hdfRejectApproveRef.Value = "RA";
                    }
                }
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
                clsApproval objApproval = new clsApproval();

                if (txtComment.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Comments/Remarks");
                    txtComment.Focus();
                    return;

                }

                objApproval.sCrby = objSession.UserId;
                objApproval.sOfficeCode = objSession.OfficeCode;
                objApproval.sApproveComments = txtComment.Text.Trim();
                objApproval.sWFObjectId = hdfWFOId.Value;
                objApproval.sWFAutoId = hdfWFOAutoId.Value;
                objApproval.sFailType = txtFailType.Text;
                objApproval.sGuarentyType = hdfGuarenteeType.Value;
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
                    objApproval.sBOId = hdfboid.Value;
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
                    objApproval.sBOId = hdfboid.Value;
                   
                    objApproval.sFormName = "RepairerWorkOrder";
                  
                    bResult = objApproval.ModifyApproveWFRequest(objApproval);
                }
                else
                {
                    bResult = objApproval.ApproveWFRequest(objApproval);
                }

                if (bResult == true)
                {

                    if (objApproval.sApproveStatus == "1")
                    {
                        ShowMsgBox("Approved Successfully");
                        cmdSave.Enabled = false;
                        ClsRepairerWorkorder objWO = new ClsRepairerWorkorder();
                        objWO.sWFDataId = objApproval.sWFDataId;
                        objWO.sWFObjectId = objApproval.sWFObjectId;
                        objWO.sTaskType = txtType.Text;
                        if (txtType.Text == "3")
                        {
                        }
                        else
                        {
                            GenerateWorkOrderReport(objWO);
                        }
                        if (objSession.RoleId == "3")
                        {

                            string sCommWONo = txtRepWoNo1.Text.Trim().Replace("'", "") + "/" + txtRepWoNo2.Text.Trim().Replace("'", "") + "/" + txtRepWoNo3.Text.Trim().Replace("'", "");
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
                        ClsRepairerWorkorder objWo = new ClsRepairerWorkorder();
                        objWo.sWFDataId = objApproval.sWFDataId;
                        objWo.sWFObjectId = objApproval.sWFObjectId;
                        objWo.sTaskType = txtType.Text;
                        if (txtType.Text == "3")
                        {
                        }
                        else
                        {
                            GenerateWorkOrderReport(objWo);
                        }
                        if (objSession.RoleId == "3")
                        {
                            clsWorkOrder objWO = new clsWorkOrder();
                            string sCommWONo = txtRepWoNo1.Text.Trim().Replace("'", "") + "/" + txtRepWoNo2.Text.Trim().Replace("'", "") + "/" + txtRepWoNo3.Text.Trim().Replace("'", "");
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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void WorkFlowConfig()
        {
            string sApproveStatus = string.Empty;
            try
            {
                //WorkFlow / Approval
                if (Request.QueryString["ActionType"] != null && Convert.ToString(Request.QueryString["ActionType"]) != "")
                {

                    if (Session["WFOId"] != null && Convert.ToString(Session["WFOId"]) != "")
                    {
                        hdfWFDataId.Value = Convert.ToString(Session["WFDataId"]);
                        hdfWFOId.Value = Convert.ToString(Session["WFOId"]);
                        hdfWFOAutoId.Value = Convert.ToString(Session["WFOAutoId"]);
                        hdfApproveStatus.Value = Convert.ToString(Session["ApproveStatus"]);

                        Session["WFDataId"] = null;
                        Session["WFOId"] = null;
                        Session["WFOAutoId"] = null;
                        Session["ApproveStatus"] = null;
                    }

                    if (hdfWFDataId.Value != "0")
                    {
                        if (txtActiontype.Text != "V")
                        {
                            sApproveStatus = GetPreviousApproveStatus(hdfWFOId.Value);

                            if (sApproveStatus == "3")
                            {
                                txtActiontype.Text = "M";
                            }
                        }

                        if (txtActiontype.Text == "V")
                        {
                            if (sApproveStatus == "3")
                            {
                                sApproveStatus = "2";
                            }
                        }


                        if (hdfWFDataId.Value != "")
                        {
                            GetWODetailsFromXML(hdfWFDataId.Value);
                        }
                    }
                    SetControlText();
                    ControlEnableDisable();
                    if (txtActiontype.Text == "V")
                    {
                        cmdSave.Text = "View";
                        cmdSave.Enabled = true;
                        cmdReset.Enabled = false;
                        dvComments.Style.Add("display", "none");
                        cmbRepairer.Enabled = false;
                    }
                }
                else
                {
                   
                    if (txtType.Text != "3")
                    {
                        cmdSave.Text = "View";
                    }
                }

                DisableControlForView();
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

        public bool CheckFormCreatorLevel()
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                string sResult = objApproval.GetFormCreatorLevel("", objSession.RoleId, "RepairerWorkOrder");
                if (sResult == "1")
                {
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

        public void DisableControlForView()
        {
            try
            {
                if (cmdSave.Text.Contains("View"))
                {
                    pnlApproval.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        #endregion

        protected void cmdClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["ActionType"] != null && Convert.ToString(Request.QueryString["ActionType"]) != "")
                {
                    Response.Redirect("/Approval/ApprovalInbox.aspx", false);
                }
                else
                {
                    Response.Redirect("/Approval/ApprovalInbox.aspx", false);
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetCommAndDecommAmount()
        {
            try
            {
                ClsRepairerEstimate objEst = new ClsRepairerEstimate();
                objEst.sFailureId = txtFailureId.Text;
                objEst.GetCommAndDecommAmount(objEst);

                txtRepAmount.Text = objEst.sTotal;
                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetCommAndDecommAccountCode()
        {
            try
            {
                ClsRepairerWorkorder objWO = new ClsRepairerWorkorder();
                objWO.sCapacity = cmbCapacity.SelectedValue;

                objWO.GetCommDecommAccCode(objWO);
                txtAcCode.Text = "74-116";

                txtSSAccCode.Text = objWO.sScrapAccCode;

                //if (txtType.Text == "1" || txtType.Text == "4")
                //{
                //    txtAcCode.Text = "74-117";

                //}
                //else if (txtType.Text == "2")
                //{
                //    txtAcCode.Text = "74-117";

                //}
                //else if (txtType.Text == "3")
                //{
                //    txtAcCode.Text = "74-117";

                //}


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
                string sTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtTCId.Text));

                string url = "/MasterForms/TcMaster.aspx?TCId=" + sTCId;
                string s = "window.open('" + url + "', '_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
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
                string sDTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtDTCId.Text));

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

        protected void cmbCapacity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GetCommAndDecommAccountCode();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void ControlEnableDisable()
        {
            try
            {
                txtFailureId.Enabled = false;
               
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        #region Load From XML
        public void GetWODetailsFromXML(string sWFDataId)
        {
            try
            {


                ClsRepairerWorkorder objWorkOrder = new ClsRepairerWorkorder();
                objWorkOrder.sWFDataId = sWFDataId;

                objWorkOrder.GetWODetailsFromXML(objWorkOrder);

                if (txtType.Text != "3" && txtFailType.Text == "1")
                {


                    txtFailureId.Text = objWorkOrder.sEstId;

                    hdfFailureId.Value = objWorkOrder.sEstId;
                    if (objWorkOrder.sCommWoNo != null)
                    {
                        cmbIssuedBy.SelectedValue = objWorkOrder.sIssuedBy;
                        txtAcCode.Text = objWorkOrder.sAccCode;

                        txtRepWoNo1.Text = objWorkOrder.sCommWoNo.Split('/').GetValue(2).ToString();
                        txtRepWoNo2.Text = objWorkOrder.sCommWoNo.Split('/').GetValue(3).ToString();
                        txtRepWoNo3.Text = objWorkOrder.sCommWoNo.Split('/').GetValue(4).ToString();


                        txtRepdate.Text = objWorkOrder.sCommDate;
                        txtRepAmount.Text = objWorkOrder.sCommAmmount;
                        txtInncured.Text = objWorkOrder.sInncuredcost;

                        cmbRepairer.SelectedValue = Convert.ToString(objWorkOrder.sRepairer);
                        cmbCapacity.SelectedValue = objWorkOrder.sNewCapacity;


                        hdfCrBy.Value = objWorkOrder.sCrBy;
                        if (txtType.Text == "3")
                        {
                            // cmbSection.SelectedValue = objWorkOrder.sRequestLoc;
                        }

                        //cmdSave.Text = "Update";
                        txtFailureId.Enabled = false;
                }                                 
                else
                {
                    txtFailureId.Enabled = false;
                }
                    if (objWorkOrder.sScrapWoNo != "0" && objWorkOrder.sScrapWoNo != null && objWorkOrder.sScrapWoNo != "")
                    {
                        dvSaleofscrap.Visible = true;
                        dvSaleofscrap.Style.Add("display", "block");
                        dvSaleofscrap.Attributes.Add("class", "span11");
                        ChkSSCheck.Checked = true;
                        txtSSWoNo1.Text = objWorkOrder.sScrapWoNo.Split('/').GetValue(2).ToString();
                        txtSSWoNo2.Text = objWorkOrder.sScrapWoNo.Split('/').GetValue(3).ToString();
                        txtSSWoNo3.Text = objWorkOrder.sScrapWoNo.Split('/').GetValue(4).ToString();

                        txtSSDate.Text = objWorkOrder.sScrapDate;
                        txtSSAmount.Text = objWorkOrder.sScrapAmmount;

                        txtSSAccCode.Text = objWorkOrder.sScrapAccCode;

                    }
                    else
                    {
                        txtSSWoNo1.Text = string.Empty;
                        txtSSWoNo2.Text = string.Empty;
                        txtSSWoNo3.Text = string.Empty;

                        txtSSDate.Text = string.Empty; 
                        txtSSAmount.Text = string.Empty;
                        if (objWorkOrder.sScrapAccCode != "" && objWorkOrder.sScrapAccCode != null)
                        {
                            txtSSAccCode.Text = objWorkOrder.sScrapAccCode;
                        }
                        else
                        {
                            objWorkOrder.sCapacity = cmbCapacity.SelectedValue;
                            objWorkOrder.GetCommDecommAccCode(objWorkOrder);
                            txtSSAccCode.Text = objWorkOrder.sScrapAccCode;
                        }

                        dvSaleofscrap.Visible = false;
                    }
                }
                else if (txtType.Text != "3")
            {
                //txtWOId.Text = objWorkOrder.sWOId;
                txtFailureId.Text = objWorkOrder.sEstId;
                //txtFailureDate.Text = objWorkOrder.sFailureDate;
                hdfFailureId.Value = objWorkOrder.sEstId;
                cmbRepairer.SelectedValue = Convert.ToString(objWorkOrder.sRepairer);

                if (objWorkOrder.sCommWoNo != null)
                {
                    cmbIssuedBy.SelectedValue = objWorkOrder.sIssuedBy;
                    txtAcCode.Text = objWorkOrder.sAccCode;

                    txtRepWoNo1.Text = objWorkOrder.sCommWoNo.Split('/').GetValue(2).ToString();
                    txtRepWoNo2.Text = objWorkOrder.sCommWoNo.Split('/').GetValue(3).ToString();
                    txtRepWoNo3.Text = objWorkOrder.sCommWoNo.Split('/').GetValue(4).ToString();

                    //txtComWoNo.Text = objWorkOrder.sCommWoNo;
                    txtRepdate.Text = objWorkOrder.sCommDate;
                    txtRepAmount.Text = objWorkOrder.sCommAmmount;
                    txtInncured.Text = objWorkOrder.sInncuredcost;




                    cmbCapacity.SelectedValue = objWorkOrder.sNewCapacity;





                    hdfCrBy.Value = objWorkOrder.sCrBy;
                    if (txtType.Text == "3")
                    {
                        //cmbSection.SelectedValue = objWorkOrder.sRequestLoc;
                    }

                    //cmdSave.Text = "Update";
                    txtFailureId.Enabled = false;

                }

                else
                {
                    //cmdSave.Text = "Save";
                    txtFailureId.Enabled = false;

                }
            }
            else
            {
                cmbIssuedBy.SelectedValue = objWorkOrder.sIssuedBy;
                txtAcCode.Text = objWorkOrder.sAccCode;

                txtRepWoNo1.Text = objWorkOrder.sCommWoNo.Split('/').GetValue(2).ToString();
                txtRepWoNo2.Text = objWorkOrder.sCommWoNo.Split('/').GetValue(3).ToString();
                txtRepWoNo3.Text = objWorkOrder.sCommWoNo.Split('/').GetValue(4).ToString();
                //txtComWoNo.Text = objWorkOrder.sCommWoNo;
                txtRepdate.Text = objWorkOrder.sCommDate;
                txtRepAmount.Text = objWorkOrder.sCommAmmount;
                txtInncured.Text = objWorkOrder.sInncuredcost;

                cmbCapacity.SelectedValue = objWorkOrder.sNewCapacity;

                hdfCrBy.Value = objWorkOrder.sCrBy;

            }     
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }
        #endregion

      

        protected void cmdViewEstimate_Click(object sender, EventArgs e)
        {
            try
            {
                string strParam = string.Empty;

                ClsRepairerEstimate objGetFailID = new ClsRepairerEstimate();
                //string sEst_ID = objGetFailID.GetFailId(txtFailureId.Text);
                string[] Arr = new string[3];
                Arr = GetPreviewReport(txtTCCode.Text);
                if (Arr[1].ToString() == "2")
                {
                    ShowMsgBox(Arr[0].ToString());
                    return;
                }
                //string sEst_ID = txtFailureId.Text; // from preview (workorder preview) 
                if ((txtType.Text == "1" || txtType.Text == "4") && txtFailType.Text != "2")
                {
                    strParam = "id=RefinedEstimationSOrepairer&sWFOID=" + Arr[2] + "&sDtrcode=" + txtTCCode.Text + "&FailType=" + "1";
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                }
                if (txtType.Text == "2" || txtFailType.Text == "2")
                {
                    strParam = "id=RefinedEstimationSOrepairer&sWFOID=" + Arr[2] + "&sDtrcode=" + txtTCCode.Text + "&FailType=" + "1";
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public string[] GetPreviewReport(string sTCCode)
        {
            string[] Arr = new string[3];
            try
            {
                clsApproval objApproval = new clsApproval();
                Arr = objApproval.GetApprovedpreview(sTCCode, "71");
                if (Arr[1] == "2")
                {
                    Arr[0] = "Data Not Available!";
                    Arr[1] = "2";
                    return Arr;
                }
                return Arr;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;

            }
        }

        public int getApprovalLevel()
        {

            int Level = 0;
            try
            {
                if (objSession.RoleId == "7")
                {
                    Level = 1;
                }
                else if (objSession.RoleId == "2")
                {
                    Level = 2;
                }
                else if (objSession.RoleId == "6")
                {
                    Level = 3;
                }
                else
                    Level = 4;
                return Level;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Level;
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

    }

}