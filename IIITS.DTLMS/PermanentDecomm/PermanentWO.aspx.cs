using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IIITS.DTLMS.BL;
using System.Globalization;
using System.Data.OleDb;
using System.IO;
using System.Collections;


namespace IIITS.DTLMS.PermanentDecomm
{
    public partial class PermanentWO : System.Web.UI.Page
    {
        string strFormCode = "PermanentWorkOrder";
        clsSession objSession;
        string sFailId = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                Form.DefaultButton = cmdSave.UniqueID;
                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;
                string sWo_id = string.Empty;
                bool sViewRecord = false;
                
               
                txtDeDate.Attributes.Add("readonly", "readonly");
                

                if (!IsPostBack)
                {
                    Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" from \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='I' ORDER BY \"MD_ORDER_BY\" ", "--Select--", cmbIssuedBy);
                    Genaral.Load_Combo("SELECT \"MD_NAME\",\"MD_NAME\" from \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='C' ORDER BY \"MD_ORDER_BY\" ", "--Select--", cmbCapacity);
                    Genaral.Load_Combo("SELECT \"SCHM_ID\",\"SCHM_NAME\" FROM \"TBLDTCSCHEME\" ORDER BY \"SCHM_ID\" ", "--Select--", cmbDtc_Scheme_Type);

                    bool bAccResult = CheckAccessRights("4");
                    if (bAccResult == false)
                    {
                        return;
                    }

                    DisableCopy();

                   
                    CalendarExtender_txtDeDate.EndDate = System.DateTime.Now;

                    if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
                    {
                        txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));
                    }

                    if (Request.QueryString["TypeValue"] != null && Request.QueryString["TypeValue"].ToString() != "")
                    {
                        txtType.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TypeValue"]));
                       
                        //if (txtType.Text.Contains('~'))
                        //{
                            //hdfGuarenteeType.Value = txtType.Text.Split('~').GetValue(1).ToString();
                            txtType.Text = txtType.Text.Split('~').GetValue(0).ToString();
                       // }
                        if (Convert.ToString(Request.QueryString["FailType"]) != null)
                        {
                            txtFailType.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["FailType"]));
                        }
                        else
                        {
                            if (txtType.Text != "3")
                            {
                                if (Request.QueryString["ReferID"] != null && Request.QueryString["ReferID"].ToString() != "")
                                {

                                    sWo_id = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ReferID"]));
                                    clsPermanentWO objWO = new clsPermanentWO();
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

                    if (Request.QueryString["ReferID"] != null && Request.QueryString["ReferID"].ToString() != "")
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
                        else if (txtType.Text == "1" && txtFailType.Text == "1" && (hdfGuarenteeType.Value != "WGP" && hdfGuarenteeType.Value != "WRGP"))
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
                                    GetFailureDetails(sFailId);
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
                                GetFailureDetails(txtFailureId.Text, txtType.Text);
                            }

                            cmbCapacity.Enabled = false;
                            lblDtcScheme.Visible = false;
                            cmbDtc_Scheme_Type.Visible = false;

                          
                            cmdSearch_Click(sender, e);
                            cmbCapacity_SelectedIndexChanged(sender, e);

                           

                        }

                       
                    }

                    
                    LoadSearchWindow();

                    WorkFlowConfig();
                    if (objSession.sRoleType == "1")
                    {
                        Session["BOID"] = "63";
                        ViewState["BOID"] = "63";
                    }
                    else
                    {
                        ViewState["BOID"] = Session["BOID"].ToString();
                    }


                }
                ApprovalHistoryView.BOID = ViewState["BOID"].ToString();
                if (txtActiontype.Text == "A" || txtActiontype.Text == "M")
                {
                    ApprovalHistoryView.sRecordId = Request.QueryString["RecordId"];
                }
                else
                {
                    ApprovalHistoryView.sRecordId = sWo_id;
                }
                

                if (txtActiontype.Text == "M")
                {
                    clsApproval objLevel = new clsApproval();
                    string sLevel = objLevel.sGetApprovalLevel(ViewState["BOID"].ToString(), objSession.RoleId);
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
                    string sLevel = objLevel.sGetApprovalLevel(ViewState["BOID"].ToString(), objSession.RoleId);
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

            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }


       

        public void DisableCopy()
        {
            try
            {
               

                txtDeWoNo1.Attributes.Add("onmousedown", "return noCopyMouse(event);");
                txtDeWoNo2.Attributes.Add("onmousedown", "return noCopyMouse(event);");
                txtDeWoNo3.Attributes.Add("onmousedown", "return noCopyMouse(event);");

              
                txtDeWoNo1.Attributes.Add("onkeydown", "return noCopyKey(event);");
                txtDeWoNo2.Attributes.Add("onkeydown", "return noCopyKey(event);");
                txtDeWoNo3.Attributes.Add("onkeydown", "return noCopyKey(event);");

               
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
                clsPermanentWO objWorkOrder = new clsPermanentWO();
                objWorkOrder.sWOId = txtWOId.Text;
                objWorkOrder.GetWODetailsForNewDTC(objWorkOrder);

                cmbIssuedBy.SelectedValue = objWorkOrder.sIssuedBy;

               
                cmbCapacity.SelectedValue = objWorkOrder.sNewCapacity;
                cmbSection.SelectedValue = objWorkOrder.sRequestLoc;
                //cmdSave.Text = "Update";
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
            try
            {
                clsPermanentWO objWorkOrder = new clsPermanentWO();
                string sFailure_id = objWorkOrder.FailureId(sFailId);
                txtFailureId.Text = sFailure_id;
            
                GetFailureDetails(sFailure_id);
           

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
                objWorkOrder.sFailureId = txtFailureId.Text;

                objWorkOrder.GetWorkOrderDetails(objWorkOrder);

                txtWOId.Text = objWorkOrder.sWOId;
                txtFailureId.Text = objWorkOrder.sFailureId;
                hdfFailureId.Value = objWorkOrder.sFailureId;
                if (txtType.Text == "2" || txtType.Text == "4")
                {
                    cmbCapacity.SelectedValue = objWorkOrder.sEnhancedCapacity;
                }
                else
                    cmbCapacity.SelectedValue = objWorkOrder.sCapacity;

                if (objWorkOrder.sDeWoNo != null || objWorkOrder.sDeWoNo != "0")
                {
                    cmbIssuedBy.SelectedValue = objWorkOrder.sIssuedBy;
                    txtDeWoNo1.Text = objWorkOrder.sDeWoNo.Split('/').GetValue(2).ToString();
                    txtDeWoNo2.Text = objWorkOrder.sDeWoNo.Split('/').GetValue(3).ToString();
                    txtDeWoNo3.Text = objWorkOrder.sDeWoNo.Split('/').GetValue(4).ToString();

                    // txtDeWoNo.Text = objWorkOrder.sDeWoNo;
                    txtDeDate.Text = objWorkOrder.sDeCommDate;
                    txtDeAmount.Text = objWorkOrder.sDeCommAmmount;
                    txtDecAccCode.Text = objWorkOrder.sDecomAccCode;
                    cmbCapacity.SelectedValue = objWorkOrder.sNewCapacity;
                    cmbIssuedBy.SelectedValue = objWorkOrder.sIssuedBy;

                    if (txtType.Text == "3")
                    {
                        cmbSection.SelectedValue = objWorkOrder.sRequestLoc;
                    }
                    cmbDtc_Scheme_Type.SelectedValue = Convert.ToString(objWorkOrder.sDtcScheme);
                    cmbRepairer.SelectedValue = objWorkOrder.sRepairer;

                    //cmdSave.Text = "Update";
                    txtFailureId.Enabled = false;        
                    cmdSearch.Visible = false;
                }

                else
                {
                    //cmdSave.Text = "Save";
                    txtFailureId.Enabled = false; 
                    cmdSearch.Visible = false;
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
                clsPermanentWO objWorkOrder = new clsPermanentWO();
                dtWODetails = objWorkOrder.FailDetails(sFailId, FailType, txtActiontype.Text, hdfGuarenteeType.Value);
                if (dtWODetails.Rows.Count > 0)
                {
                    cmbCapacity.SelectedValue = dtWODetails.Rows[0]["PEST_CAPACITY"].ToString();
                }

                if (FailType != "2" && hdfGuarenteeType.Value != "WGP")
                {
                    cmbRepairer.SelectedValue = dtWODetails.Rows[0]["TR_ID"].ToString();

                }
                else
                    cmbRepairer.SelectedValue = "0";
                if (dtWODetails.Rows.Count > 0)
                {
                    txtFailureId.Text = dtWODetails.Rows[0]["PEST_ID"].ToString();
                    txtDTCCode.Text = dtWODetails.Rows[0]["PEST_DTC_CODE"].ToString();
                    txtDTCName.Text = dtWODetails.Rows[0]["DT_NAME"].ToString();
                    txtCapacity.Text = dtWODetails.Rows[0]["PEST_CAPACITY"].ToString();
                    txtFailType.Text = dtWODetails.Rows[0]["PEST_FAIL_TYPE"].ToString();
                    txtTCCode.Text = dtWODetails.Rows[0]["PEST_TC_CODE"].ToString();
                    txtDeclaredBy.Text=dtWODetails.Rows[0]["PEST_CRBY"].ToString();
                    GetCommAndDecommAccountCode();
                    cmbCapacity.Enabled = false;
                    //cmbIssuedBy.SelectedValue = dtWODetails.Rows[0]["PWO_ISSUED_BY"].ToString();
                    //txtDeWoNo1.Text = dtWODetails.Rows[0]["PWO_NO_DECOM"].ToString();
                    //txtDeDate.Text = dtWODetails.Rows[0]["PWO_DATE_DECOM"].ToString();


                    //txtDeAmount.Text = dtWODetails.Rows[0]["PWO_AMT_DECOM"].ToString();
                    //txtDecAccCode.Text = dtWODetails.Rows[0]["PWO_ACCCODE_DECOM"].ToString();



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
                //if (txtType.Text != "3" && txtFailType.Text != "1")
                //{
                //    if (txtFailureId.Text.Trim().Length == 0)
                //    {
                //        txtFailureId.Focus();
                //        ShowMsgBox("Enter Failure Id");
                //        return bValidate;
                //    }
                //}

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

                //if (txtType.Text == "3")
                //{
                //    if (cmbSection.SelectedIndex == 0)
                //    {
                //        cmbSection.Focus();
                //        ShowMsgBox("Select Section");
                //        return bValidate;
                //    }
                //}

              

               
                if (txtType.Text != "3")
                {
                    clsPermanentWO objwo = new clsPermanentWO();
                    estimatedate = objwo.getestimatedate(sFailId);
                }
             

                if (txtType.Text != "3" && txtFailType.Text != "1")
                {
                    if (txtDeWoNo1.Text.Trim().Length == 0 || txtDeWoNo2.Text.Trim().Length == 0 || txtDeWoNo3.Text.Trim().Length == 0)
                    {
                        ShowMsgBox("Enter Decommissioning Wo No");
                        //txtDeWoNo.Focus();
                        return bValidate;
                    }
                    if (txtDeDate.Text.Trim().Length == 0)
                    {
                        ShowMsgBox("Enter DeCommissioning Date");
                        txtDeDate.Focus();
                        return bValidate;
                    }
                    if (txtType.Text != "3")
                    {

                        string sResult = Genaral.DateComparisionTransaction(txtDeDate.Text, estimatedate, false, false);
                        if (sResult == "1")
                        {
                            ShowMsgBox("DeCommisioning Work Order Date should be Greater than or Equal to Estimation Date ");
                            return bValidate;
                        }


                    }

                    if (txtDeAmount.Text.Length == 0)
                    {
                        txtDeAmount.Focus();
                        ShowMsgBox("Enter DeCommissioning Amount");
                        return bValidate;
                    }
                    if (txtDecAccCode.Text.Trim().Length == 0)
                    {
                        txtDecAccCode.Focus();
                        ShowMsgBox("Enter DeCommissioning Account Code");
                        return bValidate;
                    }
                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtDeAmount.Text, "^(\\d{1,10})?(\\.\\d{1,2})?$"))
                    {
                        ShowMsgBox("Enter Valid DeCommissioning Amount (eg:111111.00)");
                        return false;
                    }

                    //if (!System.Text.RegularExpressions.Regex.IsMatch(txtDeAmount.Text, "[-+]?[0-9]{0,7}\\.?[0-9]{0,2}"))
                    //{
                    //    ShowMsgBox("Enter Valid DeCommissioning Amount (eg:111111.00)");
                    //    return false;
                    //}
                }

                string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["NotAllowFileFormat"]);

              

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
               // txtFailureId.Text = string.Empty;
                cmbIssuedBy.SelectedIndex = 0;
                txtDTCCode.Text = string.Empty;
                txtDTCName.Text = string.Empty;
                txtTCCode.Text = string.Empty;
                txtWOId.Text = string.Empty;

                txtDeDate.Text = string.Empty;
                txtDeAmount.Text = string.Empty;


                txtDeWoNo1.Text = string.Empty;
                txtDeWoNo2.Text = string.Empty;
                txtDeWoNo3.Text = string.Empty;

                txtDecAccCode.Text = string.Empty;
                hdfFailureId.Value = string.Empty;
                clsApproval objLevel = new clsApproval();
                string sLevel = objLevel.sGetApprovalLevel(ViewState["BOID"].ToString(), objSession.RoleId);
                if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                {
                    cmdSave.Text = "Submit";
                }
                else if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                {
                    cmdSave.Text = "Approve";
                }

                cmdSearch.Visible = true;
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
                clsPermanentWO objWorkOrder = new clsPermanentWO();
                DataTable dtOffName = new DataTable();
                //Check AccessRights
                bool bAccResult = true;
                if (cmdSave.Text == "Update")
                {
                    bAccResult = CheckAccessRights("3");
                }
                else if (cmdSave.Text == "Save" || cmdSave.Text == "Submit" || cmdSave.Text == "Approve")
                {
                    bAccResult = CheckAccessRights("4");
                }

                if (bAccResult == false)
                {
                    return;
                }

                if (cmdSave.Text == "View")
                {
                    clsPermanentWO objWrkOrder = new clsPermanentWO();
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

                    objWorkOrder.sFailureId = txtFailureId.Text;
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
                    objWorkOrder.sDTCCode = txtDTCCode.Text.Trim();
                    objWorkOrder.sDTCName = txtDTCName.Text.Trim();
                    objWorkOrder.sFailType = txtFailType.Text.Trim();

                    objWorkOrder.sNewCapacity = cmbCapacity.SelectedValue.Trim();

                    if (txtFailureId.Text != "")
                    {
                        dtOffName = objWorkOrder.GetofficeName(txtFailureId.Text);
                    }
                    //else
                    //{
                    //    dtOffName = objWorkOrder.GetofficeNameBySectionCode(cmbSection.SelectedValue);
                    //}

                    string sCommWONo = dtOffName.Rows[0]["DIV"].ToString() + "/" + dtOffName.Rows[0]["SUBDIV"].ToString() ;
                    objWorkOrder.sCommWoNo = sCommWONo.Trim().ToUpper();

                    string sDeWoNo = dtOffName.Rows[0]["DIV"].ToString() + "/" + dtOffName.Rows[0]["SUBDIV"].ToString() + "/" + txtDeWoNo1.Text.Trim().Replace("'", "") + "/" + txtDeWoNo2.Text.Trim().Replace("'", "") + "/" + txtDeWoNo3.Text.Trim().Replace("'", "");
                    objWorkOrder.sDeWoNo = sDeWoNo.Trim().ToUpper();
                    objWorkOrder.sDeCommDate = txtDeDate.Text.Trim().Replace("'", "");
                    objWorkOrder.sDeCommAmmount = txtDeAmount.Text.Trim().Replace("'", "");
                    objWorkOrder.sDecomAccCode = txtDecAccCode.Text.Trim().Replace("'", "");

                    if (ChkOFCheck.Checked == true)
                    {
                        string sOFWONo = dtOffName.Rows[0]["DIV"].ToString() + "/" + dtOffName.Rows[0]["SUBDIV"].ToString() ;
                        objWorkOrder.sOFCommWoNo = sOFWONo.Trim().ToUpper();
                    }
                    else
                    {
                        objWorkOrder.sOFCommWoNo = "0";
                        objWorkOrder.sOFCommDate = "0";
                        objWorkOrder.sOFCommAmmount = "1";
                        objWorkOrder.sOFAccCode = "0";
                    }



                    if (objWorkOrder.sDeCommAmmount == "")
                    {
                        objWorkOrder.sDeCommAmmount = "0";
                    }

                    objWorkOrder.sCrBy = objSession.UserId;
                    objWorkOrder.sLocationCode = objSession.OfficeCode;
                    objWorkOrder.sTaskType = txtType.Text;
                    objWorkOrder.sDtcScheme = cmbDtc_Scheme_Type.SelectedIndex;
                    objWorkOrder.sRepairer = cmbRepairer.SelectedValue;
                    if (txtType.Text == "3")
                    {
                        objWorkOrder.sRequestLoc = cmbSection.SelectedValue.Trim();
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
                                Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "(WorkOrder)PermanentDecommissioning");
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

                        Arr = objWorkOrder.SaveUpdateWorkOrder(objWorkOrder);
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "(WorkOrder)PermanentDecommissioning");
                        }
                        if (Arr[1].ToString() == "0")
                        {
                            hdfWFDataId.Value = objWorkOrder.sWFDataId;
                            hdfAppDesc.Value = objWorkOrder.sApprovalDesc;
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

                    Arr = objWorkOrder.SaveUpdateWorkOrder(objWorkOrder);
                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "(WorkOrder)PermanentDecommissioning");
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



        private void GenerateWorkOrderReport(clsPermanentWO objWorkOrder)
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
                clsPermanentWO objOffName = new clsPermanentWO();
                sOffcName = objOffName.getofficeName(objSession.OfficeCode);
            }
            else
            {
                sOffcName = objSession.OfficeName;
            }

            //sLevelOfApproval = objWorkOrder.getLevelOfApproval(objWorkOrder, txtFailureId.Text, soffCode);
            sLevelOfApproval = getApprovalLevel().ToString();
            sNameList = objWorkOrder.getCreatedByUserName(txtFailureId.Text, soffCode);
            string sSubDiv = objWorkOrder.getsubdivName(txtFailureId.Text);
            string sWFDataId = objWorkOrder.sWFDataId;
            string sWoId = objWorkOrder.sWOId;
            string sTaskType = objWorkOrder.sTaskType;
            Session["UserNameList"] = sNameList;
            strParam = "id=WorkOrderPreviewpermanent&WFDataId=" + sWFDataId + "&LApprovel=" + sLevelOfApproval + "&OffCode=" + sOffcName + "&TaskType=" + sTaskType + "&WoId=" + sWoId + "&sSubDivName=" + sSubDiv;
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
                Response.Redirect("WorkOrderView.aspx", false);
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
                clsPermanentWO objWorkOrder = new clsPermanentWO();

                if (objWorkOrder.ValidateUpdate(txtFailureId.Text, txtWOId.Text, txtType.Text) == true)
                {
                    cmdReset.Enabled = false;
                    cmdSave.Enabled = false;
                }
                else
                {
                    cmdReset.Enabled = true;
                    cmdSave.Enabled = true;
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
                clsPermanentWO objwo = new clsPermanentWO();
                if ((txtFailType.Text == "2" && txtActiontype.Text == "A") || (txtFailType.Text == "1" && txtActiontype.Text == "A" && hdfGuarenteeType.Value == "WGP" || hdfGuarenteeType.Value == "WRGP"))
                {

                }
                else if (txtFailType.Text == "1" && txtActiontype.Text == "V" && hdfGuarenteeType.Value == "WGP")
                {

                }
                else
                {
                    if (hdfFailureId.Value != "")
                    {
                        txtFailureId.Text = hdfFailureId.Value;
                    }

                }
                objwo.sFailureId = txtFailureId.Text;

                objwo.GetFailureDetails(objwo);

                //txtDTCName.Text = objwo.sDtcName;
                //txtDTCCode.Text = objwo.sDtcCode;
                //txtDeclaredBy.Text = objwo.sCrby;
                //txtTCCode.Text = objwo.sDtcTcCode;
                //cmbCapacity.SelectedValue = objwo.sEnhancedCapacity;
                //txtDTCId.Text = objwo.sDtcId;
                //txtTCId.Text = objwo.sTCId;
             

                //if (txtType.Text == "1")
                //{
                //    txtCapacity.Text = objwo.sDtcCapacity;
                //    cmbCapacity.SelectedValue = objwo.sDtcCapacity;
                //}
                //else
                //{
                //    txtCapacity.Text = objwo.sDtcCapacity;
                //    cmbCapacity.SelectedValue = objwo.sEnhancedCapacity;
                //}



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
                if ((txtType.Text == "1" && txtFailType.Text != "1") || (txtType.Text == "4" && txtFailType.Text != "1"))
                {
                    lblIDText.Text = "Failure ID";
                    lblDateText.Text = "Failure Date";
                    if (ChkOFCheck.Checked == false)
                    {
                    }
                    dvOFCheck.Style.Add("display", "block");
                }
                else if (txtType.Text == "2")
                {
                    dvOFCheck.Style.Add("display", "block");
                    lblIDText.Text = "Enhancement ID";
                    lblDateText.Text = "Enhancement Entry Date";
                    if (ChkOFCheck.Checked == false)
                    {
                    }
                }
                else if (txtType.Text == "1" && txtFailType.Text == "1")
                {
                    if (hdfGuarenteeType.Value == "WGP" || hdfGuarenteeType.Value == "WRGP")
                    {
                        lblIDText.Text = "Failure ID";
                        lblDateText.Text = "Failure Date";
                        //dvOilFileration.Style.Add("display", "none");
                        if (ChkOFCheck.Checked == false)
                        {
                        }
                        dvOFCheck.Style.Add("display", "block");
                    }
                    else
                    {
                       // dvDecomm.Style.Add("display", "none");
                        dvBasic.Style.Add("display", "none");
                        dvSection.Style.Add("display", "none");
                        dvRepairer.Style.Add("display", "block");
                        if (objSession.OfficeCode.Length > Constants.Division)
                        {
                            Off_Code = objSession.OfficeCode.Substring(0, Constants.Division);
                        }
                        else
                            Off_Code = objSession.OfficeCode;
                        Genaral.Load_Combo("SELECT \"TR_ID\", UPPER(\"TR_NAME\") FROM \"TBLTRANSREPAIRER\",\"TBLTRANSREPAIREROFFCODE\" WHERE \"TR_ID\"=\"TRO_TR_ID\" AND \"TRO_OFF_CODE\" =  '" + Off_Code + "' ORDER BY \"TR_NAME\" ", "--Select--", cmbRepairer);
                        lnkDTCDetails.Visible = false;
                        lnkDTrDetails.Visible = false;
                        cmbRepairer.Enabled = true;
                        cmdViewEstimate.Visible = true; // for single coile before it was false
                    }

                }
                else
                {
                   // dvDecomm.Style.Add("display", "none");
                   // dvBasic.Style.Add("display", "none");
                    if (objSession.sRoleType == "2")
                    {
                        //string abc = "SELECT \"OM_CODE\", \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE '" + clsStoreOffice.GetOfficeCode(objSession.OfficeCode, "OM_CODE") + "' ORDER BY \"OM_CODE\" ";
                        Genaral.Load_Combo("SELECT \"OM_CODE\", \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE " + clsStoreOffice.GetOfficeCode(objSession.OfficeCode, "OM_CODE") + " ORDER BY \"OM_CODE\" ", "--Select--", cmbSection);
                    }
                    else
                    {
                        Genaral.Load_Combo("SELECT \"OM_CODE\", \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE CAST(\"OM_CODE\" AS TEXT) LIKE '" + objSession.OfficeCode + "%' ORDER BY \"OM_CODE\" ", "--Select--", cmbSection);
                    }

                    lnkDTCDetails.Visible = false;
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

                objApproval.sFormName = "PermanentWO";
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

        public void WorkFlowObjects(clsPermanentWO objWorkOrder)
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


                objWorkOrder.sFormName = "PermanentWO";
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
                txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));

                if (txtActiontype.Text == "A")
                {
                    cmdSave.Text = "Approve";
                    pnlApproval.Enabled = false;
                  
                }
                if (txtActiontype.Text == "R")
                {
                    cmdSave.Text = "Reject";
                    pnlApproval.Enabled = false;
                    cmbRepairer.Enabled = false;
                }
                if (txtActiontype.Text == "M")
                {
                    cmdSave.Text = "Modify and Approve";
                    pnlApproval.Enabled = true;
                    cmbRepairer.Enabled = true;
                }
                if (hdfWFOAutoId.Value == "0")
                {
                    dvComments.Style.Add("display", "block");
                }

                if (hdfWFOAutoId.Value != "0")
                {
                    dvComments.Style.Add("display", "none");
   
                }

                cmdReset.Enabled = false;


           
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
                        clsPermanentWO objWO = new clsPermanentWO();
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

                            //objWO.SendSMStoSectionOfficer(txtFailureId.Text,txtDTCCode.Text, sCommWONo.ToUpper(), txtDTCName.Text);
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
                        clsPermanentWO objWo = new clsPermanentWO();
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
            try
            {
                //WorkFlow / Approval
                if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
                {

                    if (Session["WFOId"] != null && Session["WFOId"].ToString() != "")
                    {
                        hdfWFDataId.Value = Session["WFDataId"].ToString();
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

        public bool CheckFormCreatorLevel()
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                string sResult = objApproval.GetFormCreatorLevel("", objSession.RoleId, "PermanentWO");
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
                if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
                {
                    Response.Redirect("/Approval/ApprovalInbox.aspx", false);
                }
                else
                {
                    Response.Redirect("PermanentWOview.aspx", false);
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
                clsEstimation objEst = new clsEstimation();
                objEst.sFailureId = txtFailureId.Text;
                objEst.GetCommAndDecommAmount(objEst);
                txtDeAmount.Text = objEst.sDecommTotal;
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
                clsPermanentWO objWO = new clsPermanentWO();
                objWO.sCapacity = cmbCapacity.SelectedValue;

                objWO.GetCommDecommAccCode(objWO);
                txtDecAccCode.Text = objWO.sDecomAccCode;

                


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
               // txtFailureId.Enabled = false;
                cmdSearch.Visible = false;
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

                clsPermanentWO objWorkOrder = new clsPermanentWO();
                objWorkOrder.sWFDataId = sWFDataId;

                objWorkOrder.GetWODetailsFromXML(objWorkOrder);

                if (txtType.Text != "3" && txtFailType.Text == "1")
                {

                   // txtFailureId.Text = objWorkOrder.sFailureId;
                    hdfFailureId.Value = objWorkOrder.sFailureId;
                    //if (objWorkOrder.sCommWoNo != null)
                    //{
                    txtDeclaredBy.Text = objWorkOrder.sUsername;

                        txtDeWoNo1.Text = objWorkOrder.sDeWoNo.Split('/').GetValue(2).ToString();
                        txtDeWoNo2.Text = objWorkOrder.sDeWoNo.Split('/').GetValue(3).ToString();
                        txtDeWoNo3.Text = objWorkOrder.sDeWoNo.Split('/').GetValue(4).ToString();

                        txtDeDate.Text = objWorkOrder.sDeCommDate;
                        txtDeAmount.Text = objWorkOrder.sDeCommAmmount;
                        txtDecAccCode.Text = objWorkOrder.sDecomAccCode;
                        cmbCapacity.SelectedValue = objWorkOrder.sNewCapacity;
                        cmbDtc_Scheme_Type.SelectedValue = objWorkOrder.sDtcScheme.ToString();

                        hdfCrBy.Value = objWorkOrder.sCrBy;
                        if (txtType.Text == "3")
                        {
                            cmbSection.SelectedValue = objWorkOrder.sRequestLoc;
                        }

                        //cmdSave.Text = "Update";
                      //  txtFailureId.Enabled = false;
                        cmdSearch.Visible = false;
                   // }

                    //else
                    //{
                    //    //cmdSave.Text = "Save";
                    //   // txtFailureId.Enabled = false;
                    //    cmdSearch.Visible = false;
                    //}
                }
                else if (txtType.Text != "3")
                {
                    //txtWOId.Text = objWorkOrder.sWOId;
                   // txtFailureId.Text = objWorkOrder.sFailureId;
                    //txtFailureDate.Text = objWorkOrder.sFailureDate;
                    hdfFailureId.Value = objWorkOrder.sFailureId;
                    cmbDtc_Scheme_Type.SelectedValue = objWorkOrder.sDtcScheme.ToString();
                    //if (objWorkOrder.sCommWoNo != null)
                    //{
                    txtDeclaredBy.Text = objWorkOrder.sUsername;
                    

                        txtDeWoNo1.Text = objWorkOrder.sDeWoNo.Split('/').GetValue(2).ToString();
                        txtDeWoNo2.Text = objWorkOrder.sDeWoNo.Split('/').GetValue(3).ToString();
                        txtDeWoNo3.Text = objWorkOrder.sDeWoNo.Split('/').GetValue(4).ToString();

                        // txtDeWoNo.Text = objWorkOrder.sDeWoNo;
                        txtDeDate.Text = objWorkOrder.sDeCommDate;
                        txtDeAmount.Text = objWorkOrder.sDeCommAmmount;
                        txtDecAccCode.Text = objWorkOrder.sDecomAccCode;
                        cmbCapacity.SelectedValue = objWorkOrder.sNewCapacity;

                      

                        hdfCrBy.Value = objWorkOrder.sCrBy;
                        if (txtType.Text == "3")
                        {
                            cmbSection.SelectedValue = objWorkOrder.sRequestLoc;
                        }

                        //cmdSave.Text = "Update";
                       // txtFailureId.Enabled = false;
                        cmdSearch.Visible = false;
                    //}

                    //else
                    //{
                    //    //cmdSave.Text = "Save";
                    // //   txtFailureId.Enabled = false;
                    //    cmdSearch.Visible = false;
                    //}
                }
                else
                {
                    txtDeclaredBy.Text = objWorkOrder.sUsername;
                    cmbCapacity.SelectedValue = objWorkOrder.sNewCapacity;
                    cmbSection.SelectedValue = objWorkOrder.sRequestLoc;
                    hdfCrBy.Value = objWorkOrder.sCrBy;
                    cmbDtc_Scheme_Type.SelectedIndex = objWorkOrder.sDtcScheme;
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }
        #endregion

        public void LoadSearchWindow()
        {
            try
            {
                string strQry = string.Empty;

                strQry = "Title=Search and Select Transformer Centre failure Details&";
                strQry += "Query=SELECT \"DT_NAME\",\"DT_CODE\" from \"TBLDTCMAST\",\"TBLDTCFAILURE\" WHERE \"DF_DTC_CODE\"= \"DT_CODE\" AND \"DF_REPLACE_FLAG\" =0 AND  \"DF_ID\" ";
                strQry += " NOT IN (SELECT \"WO_DF_ID\" FROM  \"TBLWORKORDER\" WHERE \"WO_DF_ID\" IS NOT NULL) ";
                strQry += " AND \"DF_LOC_CODE\" LIKE '" + objSession.OfficeCode + "%' ";
                strQry += " AND \"DF_STATUS_FLAG\" =" + txtType.Text + " AND {0} like %{1}% order by DF_ID&";
                strQry += "DBColName=\"DF_ID\"~\"DT_NAME\"~\"DT_CODE\"&";
                strQry += "ColDisplayName=" + lblIDText.Text + "~DTC_NAME~DTC_CODE&";

                strQry = strQry.Replace("'", @"\'");

                cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + hdfFailureId.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + hdfFailureId.ClientID + ")");


                txtDeDate.Attributes.Add("onblur", "return ValidateDate(" + txtDeDate.ClientID + ");");

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdViewEstimate_Click(object sender, EventArgs e)
        {
            try
            {
                string strParam = string.Empty;

                clsPermanentEstimation objGetFailID = new clsPermanentEstimation();
                //string sEst_ID = objGetFailID.GetFailId(txtFailureId.Text);
                //if (txtType.Text == "1" || txtType.Text == "4")
                //{
                //    strParam = "id=RefinedEstimation&EstimationId=" + sEst_ID;
                //    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                //}
                //if (txtType.Text == "2")
                //{
                //    strParam = "id=EnhanceEstimation&EnhanceId=" + txtFailureId.Text;
                //    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                //}
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
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

        protected void cmbDtc_Scheme_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int Scapacity = cmbCapacity.SelectedIndex;
                clsPermanentWO objWO = new clsPermanentWO();
                objWO.sDtcScheme = cmbDtc_Scheme_Type.SelectedIndex;

                objWO.GetDTCAccCode(objWO);
               
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void ChkOFCheck_CheckedChanged(object sender, EventArgs e)
        {
            string Off_Code = string.Empty;
            try
            {
                if (ChkOFCheck.Checked == true)
                {
                    dvDecomm.Style.Add("class", "span6");
                    if (objSession.OfficeCode.Length > Constants.Division)
                    {
                        Off_Code = objSession.OfficeCode.Substring(0, Constants.Division);
                    }
                    else
                        Off_Code = objSession.OfficeCode;
                    Genaral.Load_Combo("SELECT \"TR_ID\", UPPER(\"TR_NAME\") FROM \"TBLTRANSREPAIRER\",\"TBLTRANSREPAIREROFFCODE\" WHERE \"TR_ID\"=\"TRO_TR_ID\" AND \"TRO_OFF_CODE\" =  '" + Off_Code + "' ORDER BY \"TR_NAME\" ", "--Select--", cmbRepairer);
                    lnkDTCDetails.Visible = false;
                    lnkDTrDetails.Visible = false;
                    cmbRepairer.Enabled = true;
                    cmdViewEstimate.Visible = false;
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

    }
}