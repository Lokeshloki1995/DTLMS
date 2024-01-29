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
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;

namespace IIITS.DTLMS.DTCFailure
{
    public partial class FailureEntry : System.Web.UI.Page
    {

        string strFormCode = "FailureEntry";
        string flag;
        clsSession objSession;
        string sFileServerPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["EstimatioinVirtualPath"]);
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
                else
                {
                    Form.DefaultButton = cmdSave.UniqueID;
                    objSession = (clsSession)Session["clsSession"];
                    lblMessage.Text = string.Empty;

                    txtDTrCommDate.Attributes.Add("readonly", "readonly");
                    txtFailedDate.Attributes.Add("readonly", "readonly");
                    txtdocketdate.Attributes.Add("readonly", "readonly");



                    CalendarExtender1.EndDate = System.DateTime.Now;
                    txtcstcomptno2.Attributes.Add("readonly", "readonly");
                    CalendarExtender3.StartDate = System.DateTime.Now.AddDays(-3);
                    CalendarExtender3.EndDate = System.DateTime.Now;
                    txtcstcomptno2.Text = System.DateTime.Now.ToString("yyyyMMdd");
                    CalendarExtender2.StartDate = System.DateTime.Now.AddDays(-3);
                    CalendarExtender2.EndDate = System.DateTime.Now;

                    if (!IsPostBack)
                    {
                        CalendarExtender2.EndDate = System.DateTime.Now;
                        Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'FT' ORDER BY \"MD_ORDER_BY\" ", "--Select--", cmbFailureType);
                        Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'ARM' ORDER BY \"MD_ORDER_BY\" ", "--Select--", cmbReplaceEntry);
                        Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'POU' ORDER BY \"MD_ORDER_BY\" ", "--Select--", cmbPurpose);

                        Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'SG' ORDER BY \"MD_ORDER_BY\" ", "--Select--", cmbSilica);
                        Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'OT' ORDER BY \"MD_ORDER_BY\" ", "--Select--", cmbOilType);
                        Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'DTCT' ORDER BY \"MD_ORDER_BY\" ", "--Select--", cmbDTCType);

                        Genaral.Load_Combo(" SELECT \"MD_COMM_ACCCODE\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='CWIP' AND \"MD_ID\" in(1,2,3) ", "--Select--", cmbWorkName);
                        string qry = "SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'C'  AND cast(\"MD_NAME\" as text) > '" + txtCapacity.Text + "' ORDER BY \"MD_ORDER_BY\" ";
                        Genaral.Load_Combo(qry, "--Select--", cmbEnhanceCapacity);
                        uploadfileid.Visible = false;
                        if (Request.QueryString["DTCId"] != null && Convert.ToString(Request.QueryString["DTCId"]) != "")
                        {

                            txtDtcId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["DTCId"]));
                            txtFailurId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["FailureId"]));


                            if (!txtFailurId.Text.Contains("-"))
                            {
                                GetFailureDetails();
                                if (cmbGuarenteeType.SelectedIndex == 0)
                                {
                                    cmbGuarenteeType.Enabled = true;
                                }
                                else
                                    cmbGuarenteeType.Enabled = false;
                                //ValidateFormUpdate();
                            }

                            if (txtFailedDate.Text.Trim() != "")
                            {
                                if (Request.QueryString["ActionType"] != null && Convert.ToString(Request.QueryString["ActionType"]) != "")
                                {
                                    txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));
                                    if (txtActiontype.Text == "V")
                                    {
                                        if (txtCapacity.Text != cmbEnhanceCapacity.SelectedItem.Text)
                                        {
                                            rdbFail.Checked = false;
                                            rdbFail.Enabled = false;
                                            rdbFailEnhance.Checked = true;
                                            cmbEnhanceCapacity.Enabled = false;
                                            cmbEnhanceCapacity.Visible = true;
                                            lblEnCap.Visible = true;
                                        }
                                        else
                                        {
                                            rdbFail.Checked = true;
                                            rdbFailEnhance.Enabled = false;
                                            rdbFailEnhance.Checked = false;
                                            cmbEnhanceCapacity.Visible = false;
                                            lblEnCap.Visible = false;
                                        }
                                    }
                                }
                                cmdSave.Text = "View";
                                //rdbFailEnhance.Enabled = false;

                            }
                        }
                        getoilcapacitydetails();

                        //Call Search Window
                        LoadSearchWindow();

                        //WorkFlow / Approval
                        WorkFlowConfig();

                        if (objSession.RoleId == "4")
                        {
                            Session["BOID"] = "9";
                            ViewState["BOID"] = "9";
                        }
                        else
                        {
                            ViewState["BOID"] = Convert.ToString(Session["BOID"]);
                        }
                    }

                    ApprovalHistoryView.BOID = Convert.ToString(ViewState["BOID"]);
                    ApprovalHistoryView.sRecordId = txtFailurId.Text;

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


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// Validating a file before saving
        /// </summary>
        /// <returns></returns>
        protected bool Validatemaf_File()
        {
            try
            {
                if (fupMaf.PostedFile != null)
                {
                    if (fupMaf.PostedFile.ContentLength == 0)
                    {
                        ShowMsgBox("Please Upload Transformer Survey Report");
                        fupMaf.Focus();
                        return false;
                    }

                    string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FileFormat"]);
                    string sLTVRFileExt = System.IO.Path.GetExtension(fupMaf.FileName).ToString().ToLower();
                    sLTVRFileExt = ";" + sLTVRFileExt.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sLTVRFileExt))
                    {
                        ShowMsgBox("Invalid File Format");
                        return false;
                    }

                    string sFileName = Path.GetFileName(fupMaf.PostedFile.FileName);

                    fupMaf.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sFileName));
                    string sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sFileName);

                    DataTable dt = new DataTable();

                    if (ViewState["DOCUMENTS"] == null)
                    {
                        dt.Columns.Add("NAME");
                        dt.Columns.Add("PATH");
                    }
                    else
                    {
                        dt = (DataTable)ViewState["DOCUMENTS"];
                    }

                    int Id = dt.Rows.Count + 1;
                    DataRow Row = dt.NewRow();
                    Row["NAME"] = sFileName;
                    Row["PATH"] = sDirectory;
                    dt.Rows.Add(Row);
                    ViewState["DOCUMENTS"] = dt;



                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }
        /// <summary>
        /// to save Survey documents
        /// </summary>
        /// <returns></returns>
        public string SaveDocumments()
        {
            string sPath = string.Empty;
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
                if (dtDocs != null && dtDocs.Rows.Count > 0)
                {

                    clsSFTP objFtp = new clsSFTP(SFTPPath, sFTPUserName, sFTPPassword);

                    bool Isuploaded;
                    string sMainFolderName = "FAILUREENTRY";

                    string sName = Convert.ToString(dtDocs.Rows[0]["NAME"]);
                    sPath = Convert.ToString(dtDocs.Rows[0]["PATH"]);

                    sName = Regex.Replace(sName, @"[^0-9a-zA-Z_\-\.]+", "");

                    string failureid = txtFailurId != null ? txtFailurId.Text.Trim() : string.Empty;
                    // string failureid = txtFailurId.Text.Trim();
                    string sName1 = failureid + sName;

                    if (File.Exists(sPath))
                    {
                        bool IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName);
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(SFTPmainfolder + sMainFolderName + "/");
                        }
                        IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName + "/" + failureid + "/");
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(SFTPmainfolder + sMainFolderName + "/" + failureid + "/");
                        }

                        Isuploaded = objFtp.upload(SFTPmainfolder + sMainFolderName + "/" + failureid + "/", sName, sPath);
                        if (Isuploaded == true & File.Exists(sPath))
                        {
                            File.Delete(sPath);
                            sPath = SFTPmainfolder + sMainFolderName + "/" + failureid + "/" + sName;
                        }
                    }
                    dtDocs.Rows[0]["PATH"] = sPath;

                    ViewState["DOCUMENTS"] = dtDocs;
                    return sPath;
                }

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return sPath;
            }
            return sPath;
        }

        /// <summary>
        /// to get oil and capacity details
        /// </summary>
        public void getoilcapacitydetails()
        {
            string oilcap = string.Empty;
            string grtytp = string.Empty;
            try
            {
                clsFailureEntry objFailure = new clsFailureEntry();
                string dtrcode = txtTcCode.Text;
                oilcap = objFailure.getoilcapacitydetails(dtrcode);
                txtQuantityOfOil.Text = oilcap;
                txtQuantityOfOil.ReadOnly = true;

                grtytp = objFailure.getguarrentytype(dtrcode);
                txtguarrentytype.Text = grtytp;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }
        /// <summary>
        /// save methos to declare failure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                cmdReset.Enabled = false;
                if (txtCapacity.Text == cmbEnhanceCapacity.SelectedItem.Text)
                {
                    flag = "1";
                }
                else
                {
                    flag = "4";
                }
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
                    if (hdfApproveStatus.Value != "")
                    {
                        if (hdfApproveStatus.Value == "1" || hdfApproveStatus.Value == "2")
                        {
                            EstimationReport(flag);
                        }
                    }
                    else
                    {
                        EstimationReport(flag);
                    }
                    return;
                }

                if (ValidateForm() == true)
                {
                    if (oiltankcapacity.SelectedItem.Text == "<75%")
                    {
                        if (Validatemaf_File() != true)
                        {
                            ShowMsgBox("Invalid file Format");
                            cmdSave.Enabled = true;
                            return;
                        }
                    }
                    clsFailureEntry objFailure = new clsFailureEntry();

                    string[] Arr = new string[2];
                    objFailure.sFailureId = txtFailurId.Text;
                    if (cmbReplaceEntry.SelectedIndex > 0)
                    {
                        objFailure.sAlternateReplaceType = cmbReplaceEntry.SelectedValue;
                    }
                    else
                    {
                        objFailure.sAlternateReplaceType = "0";
                    }
                    if (cmbSilica.SelectedIndex > 0)
                    {
                        objFailure.sSilicaCondition = cmbSilica.SelectedValue;
                    }
                    else
                    {
                        objFailure.sSilicaCondition = "0";
                    }

                    objFailure.sDtcId = txtDtcId.Text;
                    objFailure.sDtcTcCode = txtTcCode.Text;
                    objFailure.sDtcCode = txtDTCCode.Text.Replace("'", "");
                    objFailure.sFailureDate = txtFailedDate.Text.Replace("'", "");
                    objFailure.sOilType = cmbOilType.SelectedValue;
                    objFailure.sDTCType = cmbDTCType.SelectedValue;
                    objFailure.sModem = cmbModem.SelectedValue;
                    objFailure.sWorkNameValue = cmbWorkName.SelectedValue;

                    objFailure.sGuarantyType = txtguarrentytype.Text;
                    objFailure.sOilCapacityTank = oiltankcapacity.SelectedItem.Text;
                    string cstcmptno = txtcstcomptno1.Text.Trim() + txtcstcomptno2.Text.Trim() + txtcstcomptno3.Text.Trim();
                    objFailure.CustomerCompliantNo = cstcmptno;
                    if (txtdocket1.Text == "")
                    {
                        objFailure.sdocketno = null;
                    }
                    else
                    {
                        string temp = /*txtdocket.Text +*/ txtdocket1.Text;
                        objFailure.sdocketno = temp.ToUpper();
                    }
                    if (txtdocketdate.Text == "")
                    {
                        objFailure.sdocketDate = null;
                    }
                    else
                    {
                        objFailure.sdocketDate = txtdocketdate.Text.Replace("'", "");
                    }



                    if (cmbOilLevel.SelectedIndex > 0)
                    {
                        objFailure.sOilLevel = cmbOilLevel.SelectedValue;
                    }
                    else
                    {
                        objFailure.sOilLevel = "0";
                    }


                    if (txtCustName.Text == "")
                    {
                        objFailure.sCustName = null;
                    }
                    else
                    {
                        objFailure.sCustName = txtCustName.Text;
                    }
                    if (txtCustNo.Text == "")
                    {
                        objFailure.sCustNo = null;
                    }
                    else
                    {
                        objFailure.sCustNo = txtCustNo.Text;
                    }
                    if (txtMeggerValue.Text == "" || txtMeggerValue.Text == null)
                    {
                        objFailure.sMeggerValue = "0";
                    }
                    else
                    {
                        objFailure.sMeggerValue = txtMeggerValue.Text.Trim().Replace("'", "").Replace("\"", "").Replace(";", "").Replace(",", "ç");
                    }

                    objFailure.sFailureReasure = txtReason.Text.Trim().Replace("'", "").Replace("\"", "").Replace(";", "").Replace(",", "ç");

                    objFailure.sDtcReadings = txtDTCRead.Text.Trim().Replace("'", "");
                    if (objFailure.sDtcReadings == null || objFailure.sDtcReadings == "")
                    {
                        // objFailure.sDtcReadings = txtDTCRead.Text.Trim().Replace("'", "");
                        objFailure.sDtcReadings = "0";
                    }
                    //else
                    //{
                    //   // objFailure.sDtcReadings = "0";
                    //    objFailure.sDtcReadings = txtDTCRead.Text.Trim().Replace("'", "");
                    //}

                    objFailure.sCrby = objSession.UserId;
                    objFailure.sEnhancedCapacity = cmbEnhanceCapacity.SelectedItem.Text;
                    objFailure.sDTrCommissionDate = txtDTrCommDate.Text;
                    string myString = txtDTrCommDate.Text;
                    DateTime myDateTime = DateTime.ParseExact(myString.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string myString_new = Convert.ToDateTime(myDateTime).ToString("yyyy-MM-dd");

                    objFailure.sDtrSaveCommissionDate = myString_new;
                    if (objSession.RoleId == "4")
                    {
                        if (txtDTrCommDate.Enabled == true)
                        {
                            objFailure.UpdateDtrCommDate(objFailure);
                            if (objSession.sTransactionLog == "1")
                            {
                                Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "Update DTR Commission Date in Failure ");
                            }
                        }

                        if (cmbGuarenteeType.Enabled == true)
                        {
                            objFailure.sGuarantySource = "From DropDown(User Selected)";
                        }
                        else
                        {
                            objFailure.sGuarantySource = "From Query(Automatic)";
                        }
                    }
                    else
                    {
                        if (hdfGuarenteeSource.Value == "" || hdfGuarenteeSource.Value == null)
                        {

                        }
                        else
                        {
                            objFailure.sGuarantySource = hdfGuarenteeSource.Value;
                        }
                    }

                    objFailure.sFailureType = cmbFailureType.SelectedValue.Trim();
                    objFailure.sOilQuantity = txtQuantityOfOil.Text;

                    if (cmbPurpose.SelectedIndex > 0)
                    {
                        objFailure.sPurpose = cmbPurpose.SelectedValue.Trim();
                    }
                    else
                    {
                        objFailure.sPurpose = "0";
                    }

                    if (cmbHtBusing.SelectedIndex > 0)
                    {
                        objFailure.sHTBusing = cmbHtBusing.SelectedValue.Trim();
                    }
                    if (cmbLtBusing.SelectedIndex > 0)
                    {
                        objFailure.sLTBusing = cmbLtBusing.SelectedValue.Trim();
                    }
                    if (cmbHtBusingRod.SelectedIndex > 0)
                    {
                        objFailure.sHTBusingRod = cmbHtBusingRod.SelectedValue.Trim();
                    }
                    if (cmbLtBusingRod.SelectedIndex > 0)
                    {
                        objFailure.sLTBusingRod = cmbLtBusingRod.SelectedValue.Trim();
                    }
                    if (cmbOilLevel.SelectedIndex > 0)
                    {
                        objFailure.sOilLevel = cmbOilLevel.SelectedValue.Trim();
                    }
                    if (cmbTankCondition.SelectedIndex > 0)
                    {
                        objFailure.sTankCondition = cmbTankCondition.SelectedValue.Trim();
                    }

                    if (cmbExplosion.SelectedIndex > 0)
                    {
                        objFailure.sExplosionValve = cmbExplosion.SelectedValue.Trim();
                    }
                    if (cmbDrainValve.SelectedIndex > 0)
                    {
                        objFailure.sDrainValve = cmbDrainValve.SelectedValue.Trim();
                    }
                    if (cmbBreather.SelectedIndex > 0)
                    {
                        objFailure.sBreather = cmbBreather.SelectedValue.Trim();
                    }
                    if (cmbSilica.SelectedIndex > 0)
                    {
                        objFailure.sSilicaCondition = cmbSilica.SelectedValue;
                    }

                    if (txtConnectionDate.Enabled == true)
                    {
                        objFailure.sCommissionDate = txtConnectionDate.Text;
                        bool res = objFailure.saveDtcCommissionDate(objFailure);
                        if (objSession.sTransactionLog == "1")
                        {
                            if (res == true)
                            {
                                Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "(DTC Commission date update syuccess) Failure ");
                            }
                            else
                            {
                                Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "(DTC Commission date update Failure) Failure ");
                            }

                        }
                    }

                    if (txtActiontype.Text == "A" || txtActiontype.Text == "R")
                    {
                        ApproveRejectAction();
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " Failure ");
                        }
                        EstimationReport(flag);
                        return;
                    }

                    if (txtActiontype.Text == "A" || txtActiontype.Text == "M")
                    {
                        if (cmbEnhanceCapacity.SelectedItem.Text == txtCapacity.Text)
                        {
                            objFailure.sFailtype = "1";
                        }
                        else
                        {
                            objFailure.sFailtype = "4";
                        }
                    }
                    else
                    {
                        if (cmbEnhanceCapacity.Enabled == false)
                        {
                            objFailure.sFailtype = "1";
                        }
                        else
                        {
                            objFailure.sFailtype = "4";
                        }
                    }


                    //Workflow
                    WorkFlowObjects(objFailure);

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
                        objFailure.sFailureId = "";
                        objFailure.sActionType = txtActiontype.Text;
                        objFailure.sOfficeCode = txtFailureOfficCode.Text;
                        objFailure.sCrby = hdfCrBy.Value;
                        Arr = objFailure.SaveFailureDetails(objFailure);
                        if (Arr[1].ToString() == "0")
                        {
                            hdfWFDataId.Value = objFailure.sWFDataId;
                            ApproveRejectAction();
                            Session["WFOId"] = objFailure.sWFDataId;
                            EstimationReport(flag);
                            return;
                        }
                        if (Arr[1].ToString() == "2")
                        {
                            ShowMsgBox(Arr[0]);
                            cmdSave.Enabled = true;
                            return;
                        }
                    }

                    #endregion

                    Arr = objFailure.SaveFailureDetails(objFailure);
                    txtFailureOfficCode.Text = objSession.OfficeCode;
                    string sOffCode = txtFailureOfficCode.Text;
                    string sDtcCode = txtDTCCode.Text;

                    string sWoID = objFailure.getWoIDforEstimation(sOffCode, sDtcCode);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " Failure ");
                    }

                    if (Arr[1].ToString() == "0")
                    {
                        txtFailurId.Text = objFailure.sFailureId;
                        cmdSave.Text = "Update";
                        string path = SaveDocumments();
                        objFailure.updatepathtotable(path, txtFailurId.Text);

                        txtDTCCode.Enabled = false;
                        Session["WFOId"] = objFailure.sWFDataId;
                        EstimationReport(flag);
                        ShowMsgBox(Arr[0].ToString());
                        cmdSave.Enabled = false;
                        return;
                    }

                    if (Arr[1].ToString() == "1")
                    {
                        ShowMsgBox(Arr[0]);
                    }

                    if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0]);
                        cmdSave.Enabled = true;
                        return;
                    }
                }
                cmdReset.Enabled = true;

            }
            catch (Exception ex)
            {
                //lblMessage.Text = clsException.ErrorMsg();
                ShowMsgBox("Something went wrong while saving, Please Declare Failure Again.");
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }


        /// <summary>
        /// popup for aleart messages
        /// </summary>
        /// <param name="sMsg"></param>
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

        /// <summary>
        /// to get failure details
        /// </summary>
        public void GetFailureDetails()
        {
            try
            {
                clsFailureEntry objFailure = new clsFailureEntry();

                objFailure.sFailureId = txtFailurId.Text;
                objFailure.sDtcId = txtDtcId.Text;

                objFailure.GetFailureDetails(objFailure);

                if (objFailure.sCommissionDate == "" || objFailure.sCommissionDate == null)
                {
                    txtConnectionDate.Enabled = false;
                    txtConnectionDate.ReadOnly = true;
                }
                else
                {
                    txtConnectionDate.Enabled = false;
                    txtConnectionDate.ReadOnly = true;
                }

                if (objFailure.sDTrCommissionDate == objFailure.sDTrEnumerationDate)
                {
                    txtDTrCommDate.Enabled = false;
                    txtDTrCommDate.ReadOnly = true;
                }
                else
                {
                    txtDTrCommDate.Enabled = false;
                    txtDTrCommDate.ReadOnly = true;
                }

                txtDtcId.Text = objFailure.sDtcId;
                txtDTCCode.Text = objFailure.sDtcCode;
                txtDTCName.Text = objFailure.sDtcName;
                txtLoadKW.Text = objFailure.sDtcLoadKw;
                txtLoadHP.Text = objFailure.sDtcLoadHp;
                txtConnectionDate.Text = objFailure.sCommissionDate;
                txtCapacity.Text = objFailure.sDtcCapacity;
                txtLocation.Text = objFailure.sDtcLocation;

                txtConditionOfTC.Text = objFailure.sConditionoftc;


                txtTcCode.Text = objFailure.sDtcTcCode;
                txtTCSlno.Text = objFailure.sDtcTcSlno;
                txtTCId.Text = objFailure.sTCId;
                txtdocket1.Text = objFailure.sdocketno;
                if (txtdocket1.Text != "")
                {
                    string no1 = txtdocket1.Text.Substring(1, 8);
                    txtcstcomptno2.Text = no1;
                    string no2 = txtdocket1.Text.Substring(9, 4);
                    txtcstcomptno3.Text = no2;
                }
                txtdocketdate.Text = objFailure.sdocketDate;
                txtCustName.Text = objFailure.sCustName;
                txtCustNo.Text = objFailure.sCustNo;
                txtMeggerValue.Text = objFailure.sMeggerValue;
                cmbOilType.SelectedValue = objFailure.sOilType;
                cmbDTCType.SelectedValue = objFailure.sDTCType;
                cmbModem.SelectedValue = objFailure.sModem;
                txtrate.Text = objFailure.sRating;
                cmbPurpose.SelectedValue = objFailure.sPurpose;
                cmbReplaceEntry.SelectedValue = objFailure.sAlternateReplaceType;

                if (objFailure.sPurpose != null && objFailure.sPurpose != "0")
                {
                    cmbPurpose.Enabled = false;
                }
                if (rdbFail.Checked)
                {
                    if (Request.QueryString["ActionType"] != null && Convert.ToString(Request.QueryString["ActionType"]) != "")
                    {
                        txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));
                        if (txtActiontype.Text == "V")
                        {
                            if (txtCapacity.Text != cmbEnhanceCapacity.SelectedItem.Text)
                            {
                                rdbFailEnhance.Checked = true;
                                rdbFail.Checked = false;
                            }
                            if (cmbEnhanceCapacity.SelectedItem.Text == "--Select--" || cmbEnhanceCapacity.SelectedItem.Text == "")
                            {
                                rdbFailEnhance.Checked = false;
                                rdbFail.Checked = true;
                                cmbEnhanceCapacity.SelectedItem.Text = txtCapacity.Text.Trim();
                                lblEnCap.Visible = false;
                                cmbEnhanceCapacity.Visible = false;
                            }
                        }
                    }
                    else
                    {
                        if (txtFailurId.Text != "0")
                        {
                            if (cmbEnhanceCapacity.SelectedItem.Text == txtCapacity.Text)
                            {
                                cmbEnhanceCapacity.SelectedItem.Text = txtCapacity.Text.Trim();
                            }
                            else
                            {
                                if (cmbEnhanceCapacity.SelectedItem.Text == "--Select--" || cmbEnhanceCapacity.SelectedItem.Text == "")
                                {
                                    cmbEnhanceCapacity.SelectedItem.Text = txtCapacity.Text.Trim();
                                    rdbFailEnhance.Checked = false;
                                    rdbFail.Checked = true;
                                    rdbFailEnhance.Enabled = false;
                                    cmbEnhanceCapacity.Visible = false;
                                    lblEnCap.Visible = false;
                                }
                                else
                                {
                                    cmbEnhanceCapacity.SelectedItem.Text = objFailure.sEnhancedCapacity;
                                    rdbFailEnhance.Checked = true;
                                    rdbFail.Checked = false;
                                    cmbEnhanceCapacity.Visible = true;
                                    cmbEnhanceCapacity.Enabled = false;
                                    rdbFail.Enabled = false;
                                    lblEnCap.Visible = true;
                                }
                            }
                        }
                    }
                }
                if (rdbFailEnhance.Checked)
                {
                    cmbEnhanceCapacity.SelectedItem.Text = objFailure.sEnhancedCapacity;
                }

                txtLastRepairDate.Text = objFailure.sLastRepairedDate;
                txtLastRepairer.Text = objFailure.sLastRepairedBy;
                cmbGuarenteeType.SelectedValue = objFailure.sGuarantyType;

                txtManfDate.Text = objFailure.sManfDate;
                txtDTrCommDate.Text = objFailure.sDTrCommissionDate;

                txtTCMake.Text = objFailure.sDtcTcMake;
                txtFailedDate.Text = objFailure.sFailureDate;
                txtReason.Text = objFailure.sFailureReasure;
                txtDTCRead.Text = objFailure.sDtcReadings;
                txtDTCCode.Enabled = false;
                txtFailureOfficCode.Text = objFailure.sOfficeCode;


                cmbFailureType.SelectedValue = objFailure.sFailureType;
                if (objFailure.sHTBusing != "")
                {
                    cmbHtBusing.SelectedValue = objFailure.sHTBusing;
                }
                if (objFailure.sLTBusing != "")
                {
                    cmbLtBusing.SelectedValue = objFailure.sLTBusing;
                }
                if (objFailure.sHTBusingRod != "")
                {
                    cmbHtBusingRod.SelectedValue = objFailure.sHTBusingRod;
                }
                if (objFailure.sLTBusingRod != "")
                {
                    cmbLtBusingRod.SelectedValue = objFailure.sLTBusingRod;
                }
                if (objFailure.sDrainValve != "")
                {
                    cmbDrainValve.SelectedValue = objFailure.sDrainValve;
                }
                if (objFailure.sOilLevel != "")
                {
                    cmbOilLevel.SelectedValue = objFailure.sOilLevel;
                }
                if (objFailure.sOilQuantity != "")
                {
                    txtQuantityOfOil.Text = objFailure.sOilQuantity;

                }
                if (objFailure.sTankCondition != "")
                {
                    cmbTankCondition.SelectedValue = objFailure.sTankCondition;
                }

                if (objFailure.sExplosionValve != "")
                {
                    cmbExplosion.SelectedValue = objFailure.sExplosionValve;
                }
                if (objFailure.sBreather != "")
                {
                    cmbBreather.SelectedValue = objFailure.sBreather;
                }
                if (objFailure.sSilicaCondition != "")
                {
                    cmbSilica.SelectedValue = objFailure.sSilicaCondition;
                }
                if (objFailure.sOilQuantityTank != "")
                {
                    oiltankcapacity.SelectedItem.Text = objFailure.sOilQuantityTank;
                    BindgridView(SFTPmainfolder, sUserName, sPassword);
                }
                if (objFailure.sAccHead != "" && objFailure.sAccHead != null)
                {

                    cmbWorkName.SelectedValue = objFailure.sAccHead;

                }

                if (objFailure.sFailureId != "0")
                {
                    cmdSearch.Visible = false;
                    cmdReset.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        /// <summary>
        /// search button for DTC code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdSearch_Click(object sender, EventArgs e)
        {

            try
            {
                clsFailureEntry objFailure = new clsFailureEntry();

                objFailure.sDtcCode = txtDTCCode.Text;

                objFailure.SearchFailureDetails(objFailure);

                if (objFailure.sDtcName == null)
                {

                }
                else
                {
                    txtDTCName.Text = objFailure.sDtcName;

                    if (objFailure.sDTrCommissionDate == objFailure.sDTrEnumerationDate)
                    {
                        txtDTrCommDate.Enabled = true;
                        txtDTrCommDate.ReadOnly = false;
                    }
                    else
                    {
                        txtDTrCommDate.Enabled = false;
                        txtDTrCommDate.ReadOnly = true;
                    }

                    txtDtcId.Text = objFailure.sDtcId;
                    txtDTCCode.Text = objFailure.sDtcCode;
                    txtDTCName.Text = objFailure.sDtcName;
                    txtLoadKW.Text = objFailure.sDtcLoadKw;
                    txtLoadHP.Text = objFailure.sDtcLoadHp;
                    txtConnectionDate.Text = objFailure.sCommissionDate;
                    txtCapacity.Text = objFailure.sDtcCapacity;
                    txtLocation.Text = objFailure.sDtcLocation;
                    txtTcCode.Text = objFailure.sDtcTcCode;
                    txtTCSlno.Text = objFailure.sDtcTcSlno;
                    txtTCId.Text = objFailure.sTCId;
                    if (objFailure.sGuarantyType == "")
                    {
                        objFailure.sGuarantyType = "0";
                    }
                    cmbGuarenteeType.SelectedValue = objFailure.sGuarantyType;

                    if (cmbGuarenteeType.SelectedValue == "0")
                    {
                        if (cmbGuarenteeType.SelectedIndex == 0)
                        {
                            cmbGuarenteeType.Enabled = true;
                        }
                        else
                            cmbGuarenteeType.Enabled = false;
                    }

                    cmbEnhanceCapacity.SelectedItem.Text = objFailure.sEnhancedCapacity;

                    if (rdbFail.Checked)
                    {
                        if (Request.QueryString["ActionType"] != null && Convert.ToString(Request.QueryString["ActionType"]) != "")
                        {
                            txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));
                            if (txtActiontype.Text == "V")
                            {
                                if (txtCapacity.Text != cmbEnhanceCapacity.SelectedItem.Text)
                                {
                                    rdbFailEnhance.Checked = true;
                                    rdbFail.Checked = false;
                                }
                                if (cmbEnhanceCapacity.SelectedItem.Text == "--Select--" || cmbEnhanceCapacity.SelectedItem.Text == "")
                                {
                                    rdbFailEnhance.Checked = false;
                                    rdbFail.Checked = true;
                                    cmbEnhanceCapacity.SelectedItem.Text = txtCapacity.Text.Trim();
                                    lblEnCap.Visible = false;
                                    cmbEnhanceCapacity.Visible = false;
                                }
                            }
                        }
                        else
                        {
                            if (txtFailurId.Text == "0")
                            {

                            }
                            else
                            {
                                if (cmbEnhanceCapacity.SelectedItem.Text == txtCapacity.Text)
                                {
                                    cmbEnhanceCapacity.SelectedItem.Text = txtCapacity.Text.Trim();
                                }
                                else
                                {
                                    if (cmbEnhanceCapacity.SelectedItem.Text == "--Select--" || cmbEnhanceCapacity.SelectedItem.Text == "")
                                    {
                                        cmbEnhanceCapacity.SelectedItem.Text = txtCapacity.Text.Trim();
                                        rdbFailEnhance.Checked = false;
                                        rdbFail.Checked = true;
                                        rdbFailEnhance.Enabled = false;
                                        cmbEnhanceCapacity.Visible = false;
                                        lblEnCap.Visible = false;
                                    }
                                    else
                                    {
                                        cmbEnhanceCapacity.SelectedItem.Text = objFailure.sEnhancedCapacity;
                                        rdbFailEnhance.Checked = true;
                                        rdbFail.Checked = false;
                                        cmbEnhanceCapacity.Visible = true;
                                        cmbEnhanceCapacity.Enabled = false;
                                        rdbFail.Enabled = false;
                                        lblEnCap.Visible = true;
                                    }
                                }
                            }
                        }
                    }
                    if (txtDTCName.Text.Trim() == "")
                    {
                        EmptyDTCDetails();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }



        }
        /// <summary>
        ///  to reset the fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
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
                txtFailedDate.Text = string.Empty;
                txtReason.Text = string.Empty;
                txtDTCRead.Text = string.Empty;
                txtDTCCode.Enabled = true;
                cmdSearch.Visible = true;

                cmbFailureType.SelectedIndex = 0;
                cmbHtBusing.SelectedIndex = 0;
                cmbLtBusing.SelectedIndex = 0;
                cmbDrainValve.SelectedIndex = 0;
                cmbHtBusingRod.SelectedIndex = 0;
                cmbLtBusingRod.SelectedIndex = 0;
                cmbOilLevel.SelectedIndex = 0;
                // txtQuantityOfOil.Text = string.Empty;
                cmbReplaceEntry.SelectedIndex = 0;
                cmbWorkName.SelectedIndex = 0;
                cmbOilType.SelectedIndex = 0;
                cmbDTCType.SelectedIndex = 0;
                cmbModem.SelectedIndex = 0;
                txtcstcomptno3.Text = string.Empty;
                oiltankcapacity.SelectedIndex = 0;

                oiltankcapacity_SelectedIndexChanged(sender,e);
                cmbTankCondition.SelectedIndex = 0;
                cmbExplosion.SelectedIndex = 0;
                cmbBreather.SelectedIndex = 0;
                cmbSilica.SelectedIndex = 0;
                txtDTCRead.Text = string.Empty;
                cmbPurpose.SelectedIndex = 0;
                txtMeggerValue.Text = string.Empty;
                txtdocket1.Text = string.Empty;
                txtdocketdate.Text = string.Empty;
                txtCustName.Text = string.Empty;
                txtCustNo.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// close button to close the current document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    Response.Redirect("FailureEntryView.aspx", false);
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to validate the data if invalid
        /// </summary>
        public void ValidateFormUpdate()
        {
            try
            {
                clsFailureEntry objFailure = new clsFailureEntry();
                if (objFailure.ValidateUpdate(txtFailurId.Text) == true)
                {
                    cmdReset.Enabled = false;
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

        /// <summary>
        /// to validate the master parameters for dtc and dtr
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbValidateMasterParameters(object sender, EventArgs e)
        {
            ValidateMasterParameters();

        }
        /// <summary>
        /// to validate the master parameters for dtr
        /// </summary>
        public void ValidateMasterParameters()
        {
            if (txtConnectionDate.Text.Trim() == "" || txtConnectionDate.Text.Trim() == null)
            {
                txtConnectionDate.Focus();
                ShowMsgBox("Enter Commission Date .NOTE : 1) Goto Masters -> Transformer Centre Master 2) Search DTC Code 3) Click on EDIT button 4) Enter DTC Commission Date");
                cmdSave.Enabled = true;
                cmbFailureType.ClearSelection();
                cmbReplaceEntry.ClearSelection();
                return;
            }
            if (txtDTrCommDate.Text.Trim() == "")
            {
                txtFailedDate.Focus();
                ShowMsgBox("Dtr Commission Date is Empty Please verify");

                cmdSave.Enabled = true;
                cmbFailureType.ClearSelection();
                cmbReplaceEntry.ClearSelection();
                return;

            }
            string sResult = Genaral.DateComparision(txtDTrCommDate.Text, txtConnectionDate.Text, false, false);

            if (sResult == "2")
            {
                ShowMsgBox("DTr Commission Date should be Greater than or equal to Transformer Centre Commission Date");
                cmdSave.Enabled = true;
                cmbFailureType.ClearSelection();
                cmbReplaceEntry.ClearSelection();
            }
            if (txtQuantityOfOil.Text.Trim() == "")
            {
                txtCustName.Focus();
                ShowMsgBox("Enter Quantity Of Oil(as per Tank Capacity) .NOTE : 1) Goto Masters -> DTR Master 2) Search DTr Code 3) Click on EDIT button 4) Enter Oil Capacity(in Litre) 5) Click On Update Button");
                cmdSave.Enabled = true;
                cmbFailureType.ClearSelection();
                cmbReplaceEntry.ClearSelection();

            }

        }

        /// <summary>
        /// to validate the fields if data is invalid
        /// </summary>
        /// <returns></returns>
        public bool ValidateForm()
        {
            bool bValidate = false;
            string sResultPO = string.Empty;
            try
            {
                if (txtQuantityOfOil.Text.Trim() == "")
                {
                    txtCustName.Focus();
                    ShowMsgBox("Oil Quantity can be Empty,So please update it from Exsisting DTR Entry");
                    cmdSave.Enabled = true;
                    return bValidate;
                }

                //Validation for customer Complaint no
                if (txtcstcomptno2.Text.Trim() == "")
                {
                    txtcstcomptno2.Focus();
                    ShowMsgBox("Please Enter valid Date for Customer Complaint Number");
                    cmdSave.Enabled = true;
                    return bValidate;
                }
                if (txtcstcomptno3.Text.Length < 4)
                {
                    txtcstcomptno3.Focus();
                    ShowMsgBox("Please Enter valid 4 digit number for Customer Complaint Number");
                    cmdSave.Enabled = true;
                    return bValidate;
                }
                if (txtcstcomptno3.Text.Contains("0000") || txtcstcomptno3.Text.Contains("."))
                {
                    txtcstcomptno3.Focus();
                    ShowMsgBox("Please Enter valid 4 digit number for Customer Complaint Number");
                    cmdSave.Enabled = true;
                    return bValidate;
                }


                if (string.IsNullOrWhiteSpace(txtCustNo.Text))
                {
                    txtCustNo.Focus();
                    ShowMsgBox("Please Enter Customer Mobile NO");
                    cmdSave.Enabled = true;
                    return bValidate;
                }

                string mobileNumber = txtCustNo.Text.Replace(" ", "");
                string CustNoregex = @"^\d+$";
                Match match = Regex.Match(mobileNumber, CustNoregex);

                if (mobileNumber.Length != 10 || (!match.Success))
                {
                    txtCustNo.Focus();
                    ShowMsgBox("Please Enter Valid Customer Mobile No");
                    cmdSave.Enabled = true;
                    return bValidate;
                }
                if (txtCustNo.Text.Contains('.') || txtCustNo.Text.Length.ToString() != "10" && txtCustNo.Text.Contains(""))
                {
                    txtCustNo.Focus();
                    ShowMsgBox("Please Enter Valid Customer Mobile No");
                    cmdSave.Enabled = true;
                    return bValidate;
                }

                if (cmbBreather.SelectedValue == "1" && cmbSilica.SelectedIndex == 0)
                {
                    cmbSilica.Focus();
                    ShowMsgBox("Select Silica Condition");
                    cmdSave.Enabled = true;
                    return bValidate;
                }

                if (txtCustName.Text.Trim() == "")
                {
                    txtCustName.Focus();
                    ShowMsgBox("Please Enter Customer Name");
                    cmdSave.Enabled = true;
                    return bValidate;
                }
                if (txtCustName.Text.Trim().StartsWith("."))
                {
                    txtCustName.Focus();
                    ShowMsgBox("Customer Name not Start with DOT(.)");
                    cmdSave.Enabled = true;
                    return bValidate;
                }
                //if (txtCustNo.Text.Trim() == "" )
                //{
                //    txtCustNo.Focus();
                //    ShowMsgBox("Please Enter Customer Mobile NO");
                //    cmdSave.Enabled = true;
                //    return bValidate;

                //}


                if (txtCustNo.Text[0] == '0' || txtCustNo.Text[0] == '1' || txtCustNo.Text[0] == '2')
                {
                    ShowMsgBox("Please Enter Valid Customer Mobile No");
                    txtCustNo.Focus();
                    cmdSave.Enabled = true;
                    return false;
                }

                if (cmbReplaceEntry.SelectedItem.Text == "--Select--")
                {
                    cmbReplaceEntry.Focus();
                    ShowMsgBox(" Please Select Alternate Replacement");
                    cmdSave.Enabled = true;
                    return bValidate;
                }
                else
                {
                    if (txtMeggerValue.Text.Contains("."))
                    {
                        if (txtMeggerValue.Text.Length == 1)
                        {
                            ShowMsgBox("Please Enter Proper Value");
                            cmdSave.Enabled = true;
                            return bValidate;
                        }
                    }
                }
                if (txtQuantityOfOil.Text.Trim() == "")
                {
                    txtQuantityOfOil.Focus();
                    ShowMsgBox("Please Enter Oil Quantity");
                    cmdSave.Enabled = true;
                    return bValidate;
                }

                if (cmbOilType.SelectedItem.Text == "--Select--")
                {
                    cmbOilType.Focus();
                    ShowMsgBox(" Please Select Oil Type");
                    cmdSave.Enabled = true;
                    return bValidate;
                }

                if (cmbDTCType.SelectedItem.Text == "--Select--")
                {
                    cmbDTCType.Focus();
                    ShowMsgBox(" Please Select DTC Type");
                    cmdSave.Enabled = true;
                    return bValidate;
                }

                if (cmbModem.SelectedItem.Text == "--Select--")
                {
                    cmbModem.Focus();
                    ShowMsgBox(" Please Select Modem");
                    cmdSave.Enabled = true;
                    return bValidate;
                }
                if (cmbWorkName.SelectedItem.Text == "--Select--")
                {
                    cmbWorkName.Focus();
                    ShowMsgBox(" Please Select Work Name");
                    cmbWorkName.Enabled = true;
                    return bValidate;
                }

                String a = cmbGuarenteeType.SelectedItem.Text;
                if (cmbFailureType.SelectedItem.Text == "--Select--")
                {
                    cmbFailureType.Focus();
                    ShowMsgBox(" Please Select Failure Type");
                    cmdSave.Enabled = true;
                    return bValidate;
                }
                if (txtDTrCommDate.Text.Trim() == "")
                {
                    txtFailedDate.Focus();
                    ShowMsgBox("Dtr Commission Date is Empty Please verify");

                    cmdSave.Enabled = true;
                    return bValidate;
                }
                if (txtFailedDate.Text.Trim() == "")
                {
                    txtFailedDate.Focus();
                    ShowMsgBox("Enter Failure Date");
                    cmdSave.Enabled = true;
                    return bValidate;
                }
                if (txtTcCode.Text.Trim() == "0" || txtTcCode.Text.Trim() == "" || txtTcCode.Text.Trim() == null)
                {
                    ShowMsgBox("This Transformer Centre is Currently having No TC, please contact the DTLMS Team");
                    cmdSave.Enabled = true;
                    return bValidate;
                }
                string sResult = Genaral.DateComparision(txtFailedDate.Text, txtDTrCommDate.Text, false, false);
                if (sResult == "2")
                {
                    ShowMsgBox("Failure Date should be Greater than or equal to DTr Commission Date");
                    cmdSave.Enabled = true;
                    return bValidate;
                }
                sResult = Genaral.DateComparision(txtFailedDate.Text, "", true, false);
                if (sResult == "1")
                {
                    ShowMsgBox("Failure Date should be Less than Current Date");
                    cmdSave.Enabled = true;
                    return bValidate;
                }
                if (txtReason.Text.Trim() == "")
                {
                    txtReason.Focus();

                    ShowMsgBox("Enter the Failure Reason");
                    cmdSave.Enabled = true;
                    return bValidate;
                }
                if (rdbFailEnhance.Checked == true)
                {
                    if (cmbEnhanceCapacity.SelectedItem.Text == "--Select--")
                    {
                        cmbEnhanceCapacity.Focus();
                        ShowMsgBox("Select Enhance Capacity");
                        cmdSave.Enabled = true;
                        return bValidate;
                    }

                    if (cmbEnhanceCapacity.SelectedItem.Text == txtCapacity.Text)
                    {
                        cmbEnhanceCapacity.Focus();
                        ShowMsgBox("Select Different Capacity");
                        cmdSave.Enabled = true;
                        return bValidate;
                    }
                }
                if (txtConnectionDate.Text.Trim() == "" || txtConnectionDate.Text.Trim() == null)
                {
                    txtConnectionDate.Focus();
                    ShowMsgBox("Enter Commission Date");
                    cmdSave.Enabled = true;
                    return bValidate;
                }

                if (txtTCMake.Text != "NNP")
                {
                    if (txtManfDate.Text.Trim() == "")
                    {
                        ShowMsgBox("Please Enter Manf. Date");
                        cmdSave.Enabled = true;
                        return bValidate;
                    }
                }
                if (cmbOilLevel.SelectedValue == "")
                {
                    ShowMsgBox("Please Select Oil Level Gauge");
                    cmdSave.Enabled = true;
                    return bValidate;
                }
                string sResultt = Genaral.DateComparision(txtDTrCommDate.Text, txtConnectionDate.Text, false, false);
                if (sResultt == "2")
                {
                    ShowMsgBox("DTr Commission Date Should be Greater Than Or Equal To Transformer Centre Commission Date");
                    cmdSave.Enabled = true;
                    return bValidate;
                }

                if (oiltankcapacity.SelectedItem.Text == "" || oiltankcapacity.SelectedIndex == 0)
                {
                    ShowMsgBox("Please Select Oil Level");
                    cmdSave.Enabled = true;
                    return bValidate;
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
        /// <summary>
        /// to reset the dtc details
        /// </summary>
        public void EmptyDTCDetails()
        {
            try
            {
                txtDTCName.Text = string.Empty;
                txtLoadKW.Text = string.Empty;
                txtLoadHP.Text = string.Empty;
                txtConnectionDate.Text = string.Empty;
                txtCapacity.Text = string.Empty;
                txtLocation.Text = string.Empty;
                txtTcCode.Text = string.Empty;
                txtTCMake.Text = string.Empty;
                txtTCSlno.Text = string.Empty;
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

        #endregion
        /// <summary>
        /// to get the basic parametere details for save
        /// </summary>
        /// <param name="objFailure"></param>
        public void WorkFlowObjects(clsFailureEntry objFailure)
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


                objFailure.sFormName = "FailureEntry";
                objFailure.sOfficeCode = objSession.OfficeCode;
                objFailure.sClientIP = sClientIP;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        #region Workflow/Approval
        /// <summary>
        /// to set the controls based on conditions
        /// </summary>
        public void SetControlText()
        {
            try
            {
                txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));

                txtdocket1.Enabled = true;
                txtdocketdate.Enabled = true;
                txtCustName.Enabled = true;
                txtCustNo.Enabled = true;
                txtMeggerValue.Enabled = true;
                cmbPurpose.Enabled = true;

                if (txtActiontype.Text == "A")
                {
                    if (txtCapacity.Text == cmbEnhanceCapacity.SelectedItem.Text)
                    {
                        rdbFail.Checked = true;
                    }
                    else
                    {
                        if (objSession.RoleId == "4")
                        {
                            if (txtCapacity.Text == cmbEnhanceCapacity.SelectedItem.Text)
                            {
                                rdbFail.Checked = true;
                            }
                            else
                            {
                                rdbFailEnhance.Checked = true;
                                rdbFail.Checked = false;
                                cmbEnhanceCapacity.Visible = true;
                                cmbEnhanceCapacity.Enabled = true;
                                lblEnCap.Visible = true;
                            }
                        }
                        else
                        {
                            rdbFail.Enabled = false;
                            rdbFail.Checked = false;
                            rdbFailEnhance.Checked = true;
                            lblEnCap.Enabled = false;
                            cmbEnhanceCapacity.Enabled = false;
                            lblEnCap.Visible = true;
                            cmbEnhanceCapacity.Visible = true;
                        }
                    }

                    cmdSave.Text = "Approve";
                    pnlApproval.Enabled = false;
                }
                if (txtActiontype.Text == "R")
                {
                    if (txtCapacity.Text == cmbEnhanceCapacity.SelectedItem.Text)
                    {
                        rdbFail.Checked = true;
                        rdbFailEnhance.Enabled = false;
                    }
                    else
                    {
                        rdbFail.Enabled = false;
                        rdbFail.Checked = false;
                        rdbFailEnhance.Checked = true;
                        lblEnCap.Enabled = false;
                        cmbEnhanceCapacity.Enabled = false;
                        lblEnCap.Visible = true;
                        cmbEnhanceCapacity.Visible = true;
                    }
                    cmdSave.Text = "Reject";
                }
                if (txtActiontype.Text == "M")
                {
                    if (txtCapacity.Text == cmbEnhanceCapacity.SelectedItem.Text)
                    {
                        rdbFail.Checked = true;
                        rdbFailEnhance.Enabled = false;
                    }
                    else
                    {
                        rdbFail.Enabled = false;
                        rdbFail.Checked = false;
                        rdbFailEnhance.Checked = true;
                        lblEnCap.Enabled = false;
                        cmbEnhanceCapacity.Enabled = true;
                        lblEnCap.Visible = true;
                        cmbEnhanceCapacity.Visible = true;
                    }
                    cmdSave.Text = "Modify and Approve";
                    pnlApproval.Enabled = true;
                }

                if (txtActiontype.Text == "V" && hdfWFDataId.Value != "")
                {
                    if (txtCapacity.Text == cmbEnhanceCapacity.SelectedItem.Text)
                    {
                        rdbFail.Checked = true;
                        rdbFailEnhance.Enabled = false;
                    }
                    else
                    {
                        rdbFail.Enabled = false;
                        rdbFail.Checked = false;
                        rdbFailEnhance.Checked = true;
                        lblEnCap.Enabled = false;
                        cmbEnhanceCapacity.Enabled = false;
                        lblEnCap.Visible = true;
                        cmbEnhanceCapacity.Visible = true;

                    }
                }

                dvComments.Style.Add("display", "block");
                cmdReset.Enabled = false;

                // Check for Creator of Form
                bool bResult = CheckFormCreatorLevel();
                if (bResult == true)
                {
                    pnlApproval.Enabled = true;

                    // To handle Record From Reject 
                    if (txtActiontype.Text == "A" && hdfWFDataId.Value != "")
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
        /// <summary>
        /// method to save the details
        /// </summary>
        public void ApproveRejectAction()
        {
            bool res = false;
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
                    //  bResult = objApproval.ModifyApproveWFRequest(objApproval);
                    bResult = objApproval.ModifyApproveWFRequest_Latest(objApproval);

                }
                else
                {
                    // bResult = objApproval.ApproveWFRequest(objApproval);
                    bResult = objApproval.ApproveWFRequest_Latest(objApproval);

                }

                if (bResult == true)
                {

                    if (objApproval.sApproveStatus == "1")
                    {
                        ShowMsgBox("Approved Successfully");
                        txtFailurId.Text = objApproval.sNewRecordId;

                        cmdSave.Enabled = false;

                        if (objSession.RoleId == "1")
                        {
                            clsFailureEntry objFailure = new clsFailureEntry();
                        }
                        if (objSession.RoleId == "4")
                        {
                            string sWoID = string.Empty;
                            string sOffCode = objSession.OfficeCode;
                            clsFailureEntry ObjFailure = new clsFailureEntry();
                            sWoID = ObjFailure.getWoIDforEstimation(sOffCode, txtDTCCode.Text);
                        }

                    }
                    else if (objApproval.sApproveStatus == "3")
                    {
                        ShowMsgBox("Rejected Successfully");
                        cmdSave.Enabled = false;
                    }
                    else if (objApproval.sApproveStatus == "2")
                    {
                        if (cmdSave.Text == "Modify and Approve" || cmdSave.Text == "Approve")
                        {
                            res = GetFailDetails(objApproval.sNewRecordId);
                        }

                        if (res == true)
                        {
                            ShowMsgBox("Modified and Approved Successfully");
                            txtFailurId.Text = objApproval.sNewRecordId;
                            cmdSave.Enabled = false;

                            clsFailureEntry objFailure = new clsFailureEntry();
                            if (txtCapacity.Text == cmbEnhanceCapacity.SelectedItem.Text)
                            {
                                flag = "1";
                            }
                            else
                            {
                                flag = "4";
                            }
                        }
                        else
                        {
                            ShowMsgBox("Something went wrong PGRS Docket no not updated");
                            txtFailurId.Text = objApproval.sNewRecordId;
                            cmdSave.Enabled = false;

                            clsFailureEntry objFailure = new clsFailureEntry();
                            if (txtCapacity.Text == cmbEnhanceCapacity.SelectedItem.Text)
                            {
                                flag = "1";
                            }
                            else
                            {
                                flag = "4";
                            }
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
                // lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                throw ex;

            }
        }
        /// <summary>
        /// to get failure details
        /// </summary>
        /// <param name="sFailure_id"></param>
        /// <returns></returns>
        public bool GetFailDetails(string sFailure_id)
        {
            bool res = false;
            try
            {
                clsFailureEntry objFailDetails = new clsFailureEntry();
                string pgrsdocketno = Convert.ToString(ConfigurationSettings.AppSettings["PgrsDocketno"]).ToUpper();
                string temp = /*txtdocket.Text +*/ txtdocket1.Text;
                string pgrsno = temp.ToUpper();
                objSession = (clsSession)Session["clsSession"];
                int roleid = Convert.ToInt32(objSession.RoleId);
                string consumermobilenum = txtCustName.Text;
                string consumername = txtCustNo.Text;
                return res;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return res;
            }
        }
        /// <summary>
        ///  to get work flow details to declare a failure
        /// </summary>
        public void WorkFlowConfig()
        {
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

                    if (hdfWFDataId.Value != "0")
                    {
                        GetFailureDetailsFromXML(hdfWFDataId.Value);
                    }
                    SetControlText();
                    ControlEnableDisable();
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

                DisableControlForView();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// to check the form in creator level or approval level
        /// </summary>
        /// <returns></returns>
        public bool CheckFormCreatorLevel()
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                string sResult = objApproval.GetFormCreatorLevel("", objSession.RoleId, "FailureEntry");
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
        /// <summary>
        /// to disables the controls for view purpose
        /// </summary>
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
        /// <summary>
        ///  to get failure report
        /// </summary>
        /// <param name="flag"></param>
        public void EstimationReport(string flag)
        {
            string sWFO_ID = string.Empty;
            string strParam1 = string.Empty;
            string strParam = string.Empty;
            clsEstimation objEst = new clsEstimation();
            try
            {
                if (txtFailurId.Text.Contains("-") || txtFailurId.Text == "0" || txtFailurId.Text == "")
                {



                    if (txtActiontype.Text == "R" && hdfWFOId.Value != null && hdfWFOId.Value != "" || hdfApproveStatus.Value != "")
                    {
                        sWFO_ID = hdfWFDataId.Value;
                    }

                    else if (hdfWFOId.Value != null || hdfWFOId.Value != "")
                    {

                        sWFO_ID = Convert.ToString(Session["WFOId"]);

                    }
                    strParam1 = "id=PgrsDocketSO&sWFOID=" + sWFO_ID;
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam1 + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                    return;
                }
                objEst.sOfficeCode = txtFailureOfficCode.Text;
                objEst.sFailureId = txtFailurId.Text;
                objEst.sLastRepair = txtLastRepairer.Text;
                objEst.sCrby = objSession.UserId;

                strParam = "id=PgrsDocket&FailureId=" + txtFailurId.Text;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        ///  to get failure report for section officer
        /// </summary>
        /// <param name="sWoID"></param>
        public void EstimationReportSO(string sWoID)
        {
            try
            {


                string STCcode = txtTcCode.Text;
                string strParam = string.Empty;
                strParam = "id=EstimationSO&TCcode=" + STCcode + "&WOId=" + sWoID + "&Failtype=" + flag;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to fetch dtr details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkDTrDetails_Click(object sender, EventArgs e)
        {
            string sTCId = string.Empty;

            try
            {

                if (txtTcCode.Text.Trim() == "" || txtTcCode.Text.Trim() == null)
                {
                    txtTcCode.Focus();
                    ShowMsgBox("Enter DTR Code ");
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
        /// <summary>
        ///  to fetch dtc details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        ///  buttons to enable or disable based on conditions
        /// </summary>
        public void ControlEnableDisable()
        {
            try
            {
                txtDTCCode.Enabled = false;
                cmdSearch.Visible = false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        #region Load From XML
        public void GetFailureDetailsFromXML(string sWFDataId)
        {
            try
            {
                // If the Data saved in Main Table then this function shd not execute, so done restriction like below
                // And commented for temprary purpose.. nee to change in future

                //if (!txtFailurId.Text.Contains("-"))
                //{
                //    return;
                //}

                clsFailureEntry objFailure = new clsFailureEntry();
                objFailure.sWFDataId = sWFDataId;
                objFailure.GetFailureDetailsFromXML(objFailure);

                txtDtcId.Text = objFailure.sDtcId;
                txtDTCCode.Text = objFailure.sDtcCode;
                txtDTCName.Text = objFailure.sDtcName;
                txtLoadKW.Text = objFailure.sDtcLoadKw;
                txtLoadHP.Text = objFailure.sDtcLoadHp;
                txtConnectionDate.Text = objFailure.sCommissionDate;
                txtCapacity.Text = objFailure.sDtcCapacity;
                txtLocation.Text = objFailure.sDtcLocation;
                txtTcCode.Text = objFailure.sDtcTcCode;
                txtTCSlno.Text = objFailure.sDtcTcSlno;
                if (txtdocket1.Text == "" || txtdocket1.Text == null)
                {
                    txtdocket1.Text = objFailure.sdocketno;
                }

                txtCustName.Text = objFailure.sCustName;
                txtCustNo.Text = objFailure.sCustNo;
                txtMeggerValue.Text = objFailure.sMeggerValue;
                cmbPurpose.SelectedValue = objFailure.sPurpose;
                cmbReplaceEntry.SelectedValue = objFailure.sAlternateReplaceType;
                txtConditionOfTC.Text = objFailure.sConditionoftc;
                cmbSilica.SelectedValue = objFailure.sSilicaCondition;
                //here is The changes :commented the Repairer feild              
                //cmbRepairer.SelectedValue = objFailure.sRepairer;
                if (objFailure.sEnhancedCapacity != null)
                    if (txtCapacity.Text == objFailure.sEnhancedCapacity)
                    {
                        cmbEnhanceCapacity.SelectedItem.Text = txtCapacity.Text;
                    }
                cmbEnhanceCapacity.SelectedItem.Text = objFailure.sEnhancedCapacity;

                if (objFailure.sEnhancedCapacity == "" || objFailure.sEnhancedCapacity == null)
                {
                    cmbEnhanceCapacity.SelectedItem.Text = txtCapacity.Text;
                }

                txtTCId.Text = objFailure.sTCId;

                txtLastRepairDate.Text = objFailure.sLastRepairedDate;
                txtLastRepairer.Text = objFailure.sLastRepairedBy;
                cmbGuarenteeType.SelectedItem.Text = objFailure.sGuarantyType;
                hdfGuarenteeSource.Value = objFailure.sGuarantySource;

                txtManfDate.Text = objFailure.sManfDate;
                txtDTrCommDate.Text = objFailure.sDTrCommissionDate;

                txtTCMake.Text = objFailure.sDtcTcMake;
                txtFailedDate.Text = objFailure.sFailureDate;
                txtReason.Text = objFailure.sFailureReasure;
                txtDTCRead.Text = objFailure.sDtcReadings;
                txtDTCCode.Enabled = false;
                txtFailureOfficCode.Text = objFailure.sOfficeCode;
                hdfCrBy.Value = objFailure.sCrby;
                cmbOilType.SelectedValue = objFailure.sOilType;
                cmbDTCType.SelectedValue = objFailure.sDTCType;
                cmbModem.SelectedValue = objFailure.sModem;

                if (txtdocketdate.Text == "" || txtdocketdate.Text == null)
                {
                    txtdocketdate.Text = objFailure.sdocketDate;
                }

                cmbFailureType.SelectedValue = objFailure.sFailureType.Trim();
                if (objFailure.sHTBusing != "")
                {
                    cmbHtBusing.SelectedValue = objFailure.sHTBusing;
                }
                if (objFailure.sLTBusing != "")
                {
                    cmbLtBusing.SelectedValue = objFailure.sLTBusing;
                }
                if (objFailure.sHTBusingRod != "")
                {
                    cmbHtBusingRod.SelectedValue = objFailure.sHTBusingRod;
                }
                if (objFailure.sLTBusingRod != "")
                {
                    cmbLtBusingRod.SelectedValue = objFailure.sLTBusingRod;
                }
                if (objFailure.sDrainValve != "")
                {
                    cmbDrainValve.SelectedValue = objFailure.sDrainValve;
                }
                if (objFailure.sOilLevel != "")
                {
                    cmbOilLevel.SelectedValue = objFailure.sOilLevel;
                }
                if (objFailure.sOilQuantity != "")
                {
                    txtQuantityOfOil.Text = objFailure.sOilQuantity;

                }
                if (objFailure.sTankCondition != "")
                {
                    cmbTankCondition.SelectedValue = objFailure.sTankCondition;
                }

                if (objFailure.sExplosionValve != "")
                {
                    cmbExplosion.SelectedValue = objFailure.sExplosionValve;
                }
                if (objFailure.sBreather != "")
                {
                    cmbBreather.SelectedValue = objFailure.sBreather;
                }
                if (objFailure.sSilicaCondition != "")
                {
                    if (objFailure.sSilicaCondition == "0")
                    {
                        cmbSilica.SelectedValue = objFailure.sSilicaCondition;
                        silicagel.Visible = false;
                    }
                    else
                    {
                        cmbSilica.SelectedValue = objFailure.sSilicaCondition;
                    }

                }

                if (objFailure.sFailureId != "0")
                {
                    cmdSearch.Visible = false;
                    cmdReset.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }
        #endregion
        /// <summary>
        ///  search window to search dtc code
        /// </summary>
        public void LoadSearchWindow()
        {
            try
            {
                string strQry = string.Empty;
                strQry = "Title=Search and Select Transformer Centre Failure Details&";
                strQry += "Query=select \"DT_CODE\",\"DT_NAME\" FROM \"TBLDTCMAST\" WHERE CAST(\"DT_OM_SLNO\" AS TEXT) LIKE '" + objSession.OfficeCode + "%' AND ";
                strQry += " \"DT_CODE\" NOT IN (SELECT \"DF_DTC_CODE\" FROM \"TBLDTCFAILURE\" WHERE  \"DF_REPLACE_FLAG\" = 0 ) AND {0} like %{1}% order by \"DT_CODE\" &";
                strQry += "DBColName=\"DT_CODE\"~\"DT_NAME\"&";
                strQry += "ColDisplayName=Transformer Centre Code~Transformer Centre Name&";

                strQry = strQry.Replace("'", @"\'");

                cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtDTCCode.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + txtDTCCode.ClientID + ")");

                txtFailedDate.Attributes.Add("onblur", "return ValidateDate(" + txtFailedDate.ClientID + ");");
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to check failure enhance
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdbFailEnhance_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbFailEnhance.Checked == true)
            {
                lblEnCap.Visible = true;
                cmbEnhanceCapacity.Visible = true;
                cmbEnhanceCapacity.Enabled = true;
                Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'C' AND cast(\"MD_NAME\" as int) > '" + txtCapacity.Text + "' ORDER BY \"MD_ORDER_BY\" ", "--Select--", cmbEnhanceCapacity);
                Genaral.Load_Combo(" SELECT \"MD_COMM_ACCCODE\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='CWIP' AND \"MD_ID\" in(1,3) ", "--Select--", cmbWorkName);
            }
            else
            {
                lblEnCap.Visible = false;
                cmbEnhanceCapacity.Visible = false;
                cmbEnhanceCapacity.Enabled = false;
                cmbEnhanceCapacity.SelectedItem.Text = txtCapacity.Text.Trim();
            }
        }

        /// <summary>
        /// to check based on breather condition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbBreather_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string sBreather = cmbBreather.SelectedValue;
                if (sBreather == "2")
                {
                    silicagel.Visible = false;
                }

                else
                {
                    silicagel.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        /// <summary>
        /// on click method for oil tank capacity
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void oiltankcapacity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string sBreather = oiltankcapacity.SelectedValue;

                if (sBreather == "1")
                {
                    uploadfileid.Visible = true;
                }

                else
                {
                    uploadfileid.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        ///  method to view and save the document
        /// </summary>
        /// <param name="FtpServer"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public DataTable BindgridView(string FtpServer, string username, string password)
        {
            DataTable dtFiles = new DataTable();
            try
            {
                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);

                string failureid = Regex.Replace(txtFailurId.Text, @"[^0-9a-zA-Z]+", "");
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
        /// <summary>
        ///  dounload method to download the file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                    string failureid = Regex.Replace(txtFailurId.Text, @"[^0-9a-zA-Z]+", "");
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
        /// <summary>
        ///  to fet a file name for downloading the files
        /// </summary>
        /// <param name="hreflink"></param>
        /// <returns></returns>
        private string getFilename(string hreflink)
        {
            Uri uri = new Uri(hreflink);

            string filename = System.IO.Path.GetFileName(uri.LocalPath);

            return filename;
        }

        /// <summary>
        /// to download the file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DownloadFile(object sender, EventArgs e)
        {

            string fileName = (sender as LinkButton).CommandArgument;

            try
            {
                bool status = false;
                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["VirtualDirectoryDocs"]);

                string failureid = Regex.Replace(txtFailurId.Text, @"[^0-9a-zA-Z]+", "");
                clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);

                string path = SFTPmainfolder + "FAILUREENTRY/" + failureid + "/" + fileName;
                RegisterStartupScript("Print", "<script>window.open('" + path + "','_blank')</script>");

            }
            catch (WebException ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// method for pageindexing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        ///  to sort a grid based on directions as asending or descending
        /// </summary>
        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "ASC"; }
            set { ViewState["SortDirection"] = value; }
        }
        /// <summary>
        ///  to sort a files based on conditions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// to sort a files based on conditions
        /// </summary>
        private string GridViewSortExpression
        {
            get { return ViewState["SortExpression"] as string ?? string.Empty; }
            set { ViewState["SortExpression"] = value; }
        }
        /// <summary>
        /// to sort a files based on conditions
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// to sort a data table based on conditions
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="isPageIndexChanging"></param>
        /// <returns></returns>
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