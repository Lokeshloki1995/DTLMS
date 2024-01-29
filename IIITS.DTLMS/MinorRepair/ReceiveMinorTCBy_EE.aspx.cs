using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;

namespace IIITS.DTLMS.MinorRepair
{
    public partial class ReceiveMinorTCBy_EE : System.Web.UI.Page
    {
        string strFormCode = "ReceiveTC";
        clsSession objSession = new clsSession();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
            {
                Response.Redirect("~/Login.aspx", false);
            }
            Form.DefaultButton = cmdSave.UniqueID;
            objSession = (clsSession)Session["clsSession"];
            lblMessage.Text = string.Empty;
            string sFailId = string.Empty;
            txtTransDate.Attributes.Add("readonly", "readonly");

            txtTransDate_CalendarExtender1.EndDate = System.DateTime.Now;

            if (!IsPostBack)
            {
                Genaral.Load_Combo("SELECT \"TR_ID\", \"TR_NAME\" FROM \"TBLTRANSREPAIRER\" WHERE \"TR_BLACK_LISTED\" = '0' ORDER BY \"TR_NAME\" ", "--Select--", cmbRepairer);

                if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
                {
                    txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));
                }
                if (Request.QueryString["WOId"] != null && Request.QueryString["WOId"].ToString() != "")
                {
                    txtwrkId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["WOId"]));
                }

                if (txtActiontype.Text != "V")
                {

                }

                if (txtwrkId.Text.Contains("-"))
                {
                    string sWO_ID = Session["WFOId"].ToString();
                    GetWOSLID(sWO_ID);
                }
                LoadWODetails();
                LoadSearchWindow();
                WorkFlowConfig();
            }
        }

        public void LoadWODetails()
        {
            try
            {
                clsReceiveMinorTC objTc = new clsReceiveMinorTC();
                objTc.sWoSlno = txtwrkId.Text;
                if (txtActiontype.Text == "V")
                {
                    objTc.sWoSlno = objTc.GetWOSLNO(txtwrkId.Text);
                    cmdSave.Text = "View";
                    cmdSave.Enabled = false;
                }
                objTc.LoadReceiveMinorTCDetails(objTc);
                //objTc.LoadWODetails(objTc);
                txtWoNo.Text = objTc.sWONumber;
                txtWrkdate.Text = objTc.sWODate;
                txtwrkId.Text = objTc.sWoSlno;
                txtRaisedBy.Text = objTc.sWOCrby;
                txtFailureId.Text = objTc.sFailureID;
                txtEstimationNo.Text = objTc.sESTNumber;
                txtEstDate.Text = objTc.sESTDate;
                txtDTCName.Text = objTc.sDTCName;
                txtOldTcCode.Text = objTc.sOldDTRCode;
                txtoldtccapacity.Text = objTc.sOldDTRCapacity;
                cmbRepairer.SelectedValue = objTc.sRepairer;
                txtTCCode.Text = objTc.sNewDTRCode;
                txtTransDate.Text = objTc.sReceivedate;
                txtWoNo.Enabled = false;
                cmbRepairer.Enabled = false;
                txtTransDate.Enabled = false;
                clsReceiveMinorTC objReceive = new clsReceiveMinorTC();
                objReceive.sNewDTRCode = txtTCCode.Text;
                objReceive.GetTCDetails(objReceive);

                if (objReceive.sNewDTRCode == "")
                {
                    ShowMsgBox("TC NOT IN STORE OR GOOD CONDITION");
                    txtTCCode.Text = "";
                }
                else
                {
                    txtTcMake.Text = objReceive.sNewDTRMake;
                    txtCapacity.Text = objReceive.sNewDTRCapacity;
                    txtTCCode.Text = objReceive.sNewDTRCode;
                    txtSLNo.Text = objReceive.sNewDTRSLNo;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetWOSLID(string sWO_ID)
        {
            try
            {
                clsReceiveMinorTC objReceive = new clsReceiveMinorTC();
                string sWOSL_ID = objReceive.GetWOSLID(sWO_ID);
                txtwrkId.Text = sWOSL_ID;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                clsReceiveMinorTC objReceive = new clsReceiveMinorTC();
                // txtTCCode.Text = hdfTCCode.Value;
                objReceive.sNewDTRCode = txtTCCode.Text;
                objReceive.sOfficeCode = objSession.OfficeCode;
                objReceive.GetTCDetails(objReceive);

                if (objReceive.sNewDTRCode == "")
                {
                    ShowMsgBox("TC NOT IN STORE OR GOOD CONDITION");
                    txtTCCode.Text = "";
                }
                else
                {
                    txtTcMake.Text = objReceive.sNewDTRMake;
                    txtCapacity.Text = objReceive.sNewDTRCapacity;
                    txtTCCode.Text = objReceive.sNewDTRCode;
                    txtSLNo.Text = objReceive.sNewDTRSLNo;
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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadSearchWindow()
        {
            try
            {

                //string strQry = string.Empty;
                //strQry = "Title=Search and Select DTC Failure Details&";
                //strQry += "Query=select \"DT_CODE\",\"DT_NAME\" FROM \"TBLDTCMAST\" WHERE  ";
                //strQry += " \"DT_CODE\" NOT IN (SELECT \"DF_DTC_CODE\" FROM \"TBLDTCFAILURE\" WHERE  \"DF_REPLACE_FLAG\" = 0 ) AND {0} like %{1}% order by \"DT_CODE\" &";
                //strQry += "DBColName=\"DT_CODE\"~\"DT_NAME\"&";
                //strQry += "ColDisplayName=DTC Code~DTC Name&";

                //strQry = strQry.Replace("'", @"\'");

                //btnSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtTCCode.ClientID + "&btn=" + btnSearch.ClientID + "',520,520," + txtTCCode.ClientID + ")");


                //" + objSession.OfficeCode + "
                string strQry = string.Empty;
                strQry = "Title=Search and Select DTr Details&";
                strQry += "Query=SELECT \"TC_CODE\", \"TM_NAME\", \"TC_CAPACITY\"  FROM \"TBLTCMASTER\", \"TBLDTCFAILURE\", \"TBLESTIMATIONDETAILS\", ";
                strQry += " \"TBLTRANSMAKES\" WHERE \"EST_FAILUREID\" = \"DF_ID\" AND  \"DF_EQUIPMENT_ID\" = \"TC_CODE\" AND \"TC_CURRENT_LOCATION\" = '3' ";
                strQry += " AND \"TC_MAKE_ID\" = \"TM_ID\" AND CAST(\"TC_LOCATION_ID\" AS TEXT) LIKE '%' AND ";
                strQry += " \"EST_REPAIRER\" = '" + cmbRepairer.SelectedValue + "' AND {0} like %{1}% &";
                strQry += "DBColName=\"TC_CODE\"~\"TM_NAME\"~CAST(\"TC_CAPACITY\" AS TEXT)&";
                strQry += "ColDisplayName=DTr Code~Make Name~Capacity&";

                ////strQry += "Query=SELECT \"TC_CODE\",\"TM_NAME\", \"TC_CAPACITY\" AS TC_CAPACITY,CASE \"TC_STATUS\" WHEN 1 THEN 'BRAND NEW' WHEN 2 THEN 'REPAIRED GOOD' END TC_STATUS ";
                ////strQry += " FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\" WHERE   \"TC_CURRENT_LOCATION\"='1' AND \"TC_STATUS\" IN (1,2)";
                ////strQry += " AND \"TC_MAKE_ID\"= \"TM_ID\" and \"TC_STATUS\" in (1,2) AND \"TC_CURRENT_LOCATION\" =1  AND {0} like %{1}% &";
                ////strQry += "DBColName=\"TC_SLNO\"~\"TC_CODE\"~\"TM_NAME\"~CAST(\"TC_CAPACITY\" AS TEXT)&";
                ////strQry += "ColDisplayName=DTr SlNo~DTr Code~Make Name~Capacity&";

                strQry = strQry.Replace("'", @"\'");
                btnSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtTCCode.ClientID + "&btn=" + btnSearch.ClientID + "',520,520," + txtTCCode.ClientID + ")");

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
                bool bAccResult = true;
                if (cmdSave.Text == "Update")
                {
                    bAccResult = CheckAccessRights("3");
                }
                else if (cmdSave.Text == "Save")
                {
                    bAccResult = CheckAccessRights("2");
                }

                if (bAccResult == false)
                {
                    return;
                }

                if (cmdSave.Text == "View")
                {
                    if (hdfApproveStatus.Value != "")
                    {
                        if (hdfApproveStatus.Value == "1" || hdfApproveStatus.Value == "2")
                        {

                            //GenerateIndentReport();
                        }
                    }
                    else
                    {
                        //GenerateIndentReport();
                    }
                    return;
                }

                if (ValidateForm() == true)
                {
                    string[] Arr = new string[2];
                    clsReceiveMinorTC objMinor = new clsReceiveMinorTC();
                    objMinor.sNewDTRCode = txtTCCode.Text;
                    objMinor.sOldDTRCode = txtOldTcCode.Text;
                    objMinor.sWoSlno = txtwrkId.Text;
                    objMinor.sReceivedate = txtTransDate.Text;
                    objMinor.sCrby = objSession.UserId;
                    objMinor.sNewDTRMake = txtTcMake.Text;
                    objMinor.sNewDTRCapacity = txtCapacity.Text;
                    objMinor.sNewDTRSLNo = txtSLNo.Text;
                    objMinor.sWFDataId = hdfWFOId.Value;
                    objMinor.sApproveComment = txtComment.Text;                   

                    if (txtActiontype.Text == "A" || txtActiontype.Text == "R")
                    {
                            ApproveRejectAction();

                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (ReceiveMinorTCBy_EE) Failure ");
                        }

                        return;                        
                    }

                    if (Arr[1].ToString() == "0")
                    {
                        ShowMsgBox(Arr[0]);
                        cmdSave.Text = "Update";
                        //GenerateIndentReport();
                        cmdSave.Enabled = false;
                        return;
                    }
                    if (Arr[1].ToString() == "1")
                    {
                        if (cmdSave.Text == "Update")
                        {
                            ShowMsgBox(Arr[0]);
                            return;
                        }
                        //if (txtActiontype.Text == "A" || txtActiontype.Text == "R")
                        //{
                        //    ApproveRejectAction();
                        //    return;
                        //}
                        if (txtActiontype.Text == "M")
                        {
                            ShowMsgBox("Modified and Approved Successfully");
                        }
                        else
                        {

                            //GenerateIndentReport();
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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void WorkFlowObjects(clsReceiveMinorTC objReceiveTC)
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


                objReceiveTC.sFormName = "ReceiveMinorTC";
                objReceiveTC.sOfficeCode = objSession.OfficeCode;
                objReceiveTC.sClientIP = sClientIP;
                objReceiveTC.sWFOId = hdfWFOId.Value;
                objReceiveTC.sWFAutoId = hdfWFOAutoId.Value;

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


                if (txtDrawingDescription.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Comments/Remarks");
                    txtComment.Focus();
                    return;

                }

                objApproval.sCrby = objSession.UserId;
                objApproval.sOfficeCode = objSession.OfficeCode;
                objApproval.sApproveComments = txtDrawingDescription.Text.Trim();
                objApproval.sWFObjectId = hdfWFOId.Value;
                objApproval.sWFAutoId = hdfWFOAutoId.Value;
                objApproval.sFormName = "ReceiveMinorTCBy_EE";
                //objApproval.sFailType = "2";

                //Approve
                if (txtActiontype.Text == "A")
                {
                    objApproval.sApproveStatus = "1";
                }
                //Reject
                if (txtActiontype.Text == "R")
                {
                    objApproval.sApproveStatus = "3";
                }
                //Abort
                if (txtActiontype.Text == "D")
                {
                    objApproval.sApproveStatus = "4";
                }
                //Modify and Approve
                if (txtActiontype.Text == "M")
                {
                    objApproval.sApproveStatus = "2";
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
                //if (txtActiontype.Text == "M")
                //{
                //    objApproval.sWFDataId = hdfWFDataId.Value;
                //    if (hdfRejectApproveRef.Value == "RA")
                //    {
                //        objApproval.sApproveStatus = "1";
                //    }
                //    bResult = objApproval.ModifyApproveWFRequest(objApproval);
                //}
                //else
                //{
                    bResult = objApproval.ApproveWFRequest(objApproval);
                //}
                if (bResult == true)
                {

                    if (objApproval.sApproveStatus == "1")
                    {
                        ShowMsgBox("Approved Successfully");
                        cmdSave.Enabled = false;
                        //if (txtType.Text != "3")
                        //{
                        //    if (objSession.RoleId == "1")
                        //    {
                        //        txtIndentId.Text = objApproval.sNewRecordId;
                        //        GenerateIndentReport();

                        //    }
                        //}
                    }
                    else if (objApproval.sApproveStatus == "3")
                    {

                        ShowMsgBox("Rejected Successfully");
                        cmdSave.Enabled = false;
                    }
                    else if (objApproval.sApproveStatus == "4")
                    {

                        ShowMsgBox("Aborted Successfully");
                        cmdSave.Enabled = false;
                    }
                    else if (objApproval.sApproveStatus == "2")
                    {
                        ShowMsgBox("Modified and Approved Successfully");
                        cmdSave.Enabled = false;
                        //if (txtType.Text != "3")
                        //{
                        //    if (objSession.RoleId == "1")
                        //    {
                        //        txtIndentId.Text = objApproval.sNewRecordId;
                        //        GenerateIndentReport();

                        //    }
                        //}
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
                        GetDataFromXML(hdfWFDataId.Value);
                    }
                }
                else
                {
                    cmdSave.Text = "View";
                }
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
                if (txtTCCode.Text == string.Empty)
                {
                    ShowMsgBox("Please Select the Proper DTR");
                    return bValidate;
                }
                if (txtTransDate.Text == string.Empty)
                {
                    ShowMsgBox("Please Enter Replacement Date");
                    return bValidate;
                }

                string sResult = Genaral.DateComparisionTransaction(txtTransDate.Text, txtWrkdate.Text, false, false);
                if (sResult == "1")
                {
                    ShowMsgBox("Transactional Date should be Greater than Work Order Date");
                    return bValidate;
                }
                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return bValidate;
            }
        }
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "FailureEntry";
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


        protected void txtTCCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtTCCode.Text.Trim() != "")
                {
                    btnSearch_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void GetDataFromXML(string sWFO_ID)
        {
            try
            {
                clsReceiveMinorTC objReceive = new clsReceiveMinorTC();
                objReceive.sWFO_id = sWFO_ID;
                objReceive.GetWODetailsFromXML(objReceive);
                txtTCCode.Text = objReceive.sNewDTRCode;
                txtCapacity.Text = objReceive.sNewDTRCapacity;
                txtSLNo.Text = objReceive.sNewDTRSLNo;
                txtTcMake.Text = objReceive.sNewDTRMake;
                txtTransDate.Text = objReceive.sReceivedate;
                txtTransDate.Enabled = false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdViewWO_Click(object sender, EventArgs e)
        {
            try
            {
                string sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt("1"));
                string sRecordId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtwrkId.Text));
                string sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt("V"));
                string sFailType = HttpUtility.UrlEncode(Genaral.UrlEncrypt("1"));

                string url = "/DTCFailure/WorkOrder.aspx?TypeValue=" + sTaskType + "&ReferID=" + sRecordId + "&ActionType=" + sActionType + "&FailType=" + sFailType;
                string s = "window.open('" + url + "', '_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);

                //Response.Redirect("/DTCFailure/WorkOrder.aspx?TypeValue=" + sTaskType + "&ReferID=" + sRecordId + "&ActionType=" + sActionType + "&FailType=" + sFailType, false);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}