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
using System.Configuration;
using System.Net;
using System.Security.Principal;

namespace IIITS.DTLMS.DTCFailure
{
    public partial class Enhancement : System.Web.UI.Page
    {

        string strFormCode = "Enhancement";
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
                    txtDTrCommDate.Attributes.Add("readonly", "readonly");
                    txtEnhanceDate.Attributes.Add("readonly", "readonly");

                    CalendarExtender2.EndDate = System.DateTime.Now;
                    //txtwork.Text = "14.1697";
                    txtwork.Text = "REPLACEMENT_OF_DISTRIBUTION_TRANSFORMERS_BY_HIGHER_CAPACITY_TRANSFORMERS";
                    txtwork.ReadOnly = true;
                    if (!IsPostBack)
                    {


                        //if(txtDTCCode.Text != "")
                        //{
                        //    clsEnhancement objGetCap = new clsEnhancement();
                        //    txtEnhancementId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["DTCId"]));
                        //    string FailCap = objGetCap.GetTCCapacity(txtEnhancementId.Text);
                        //    Genaral.Load_Combo("SELECT \"MD_NAME\",\"MD_NAME\" from \"TBLMASTERDATA\" WHERE \"MD_TYPE\" ='C' AND \"MD_NAME\" <> '" + FailCap + "' ORDER BY \"MD_ID\"", "--Select--", cmbCapacity);
                        //}

                        txtEnhanceDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                        cmdSearch.Visible = true;
                        if (Request.QueryString["DTCId"] != null && Convert.ToString(Request.QueryString["DTCId"]) != "")
                        {
                            txtDtcId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["DTCId"]));
                            txtEnhancementId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["EnhanceId"]));
                            cmdSearch.Visible = false;
                            if (!txtEnhancementId.Text.Contains("-"))
                            {
                                GetEnhancementDetails();
                                ValidateFormUpdate();
                            }
                        }

                        //Search Window Call
                        LoadSearchWindow();

                        //WorkFlow / Approval
                        //Genaral.Load_Combo("SELECT \"MD_NAME\",\"MD_NAME\" from \"TBLMASTERDATA\" WHERE \"MD_TYPE\" ='C' AND \"MD_NAME\" <> '" + txtCapacity.Text + "' ORDER BY \"MD_ORDER_BY\"", "--Select--", cmbCapacity);
                        WorkFlowConfig();

                        if (objSession.RoleId == "4")
                        {
                            Session["BOID"] = "10";
                            ViewState["BOID"] = "10";
                        }
                        else
                        {
                            ViewState["BOID"] = Convert.ToString(Session["BOID"]);
                        }
                    }
                    ApprovalHistoryView.BOID = Convert.ToString(ViewState["BOID"]);
                    ApprovalHistoryView.sRecordId = txtEnhancementId.Text;

                    if (txtActiontype.Text == "M")
                    {
                        clsApproval objLevel = new clsApproval();
                        string sLevel = objLevel.sGetApprovalLevel(Convert.ToString(ViewState["BOID"]), objSession.RoleId);
                        if (sLevel != "" && sLevel != null)
                        {
                            if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                            {
                                cmdSave.Text = "Modify and Submit";
                            }
                            else if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                            {
                                cmdSave.Text = "Modify and Approve";
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

    
        protected void cmdSave_Click(object sender, EventArgs e)
        {
           try
            {
                 
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
                               //EstimationReport();
                           }
                       }
                       else
                       {
                          // EstimationReport();
                       }
                       return;
                   }
                   
                  
                    if (ValidateForm() == true)
                    {
                    DataTable dtDocs = new DataTable();
                    clsEnhancement objEnhancement = new clsEnhancement();
                        string[] Arr = new string[2];

                        objEnhancement.sEnhancementId = txtEnhancementId.Text;

                        objEnhancement.sDtcId = txtDtcId.Text;
                        objEnhancement.sTcCode = txtTcCode.Text;
                        objEnhancement.sDtcCode = txtDTCCode.Text.Replace("'", "");
                        objEnhancement.sEnhancementDate = txtEnhanceDate.Text.Replace("'", "");
                        objEnhancement.sDtrcommdate = txtDTrCommDate.Text;
                        objEnhancement.sReason = txtReason.Text.Replace("'", "").Replace("\"", "").Replace(";", "").Replace(",", "ç");
                        objEnhancement.sDtcReadings = txtDTCRead.Text.Replace("'", "");
                        objEnhancement.sCrby = objSession.UserId;
                        objEnhancement.sEnhancedCapacity = cmbCapacity.SelectedValue;

                        if (txtActiontype.Text == "A" || txtActiontype.Text == "R")
                        {
                            ApproveRejectAction();
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " Enhancement ");
                        }
                        return;
                        }

                        //Workflow
                        WorkFlowObjects(objEnhancement);



                    #region Modify and Approve


                    
                    // For Modify and Approve
                    if (txtActiontype.Text == "M")
                        {
                        SaveDocumments(objEnhancement);
                        dtDocs = (DataTable)ViewState["DOCUMENTS"];
                        objEnhancement.dtDocuments = dtDocs;
                        if (txtComment.Text.Trim() == "")
                            {
                                ShowMsgBox("Enter Comments/Remarks");
                                txtComment.Focus();
                                return;

                            }
                            objEnhancement.sEnhancementId = "";
                            objEnhancement.sActionType = txtActiontype.Text;
                            objEnhancement.sOfficeCode = hdfEnhanceOffcode.Value;
                            objEnhancement.sCrby = hdfCrBy.Value;
                            Arr = objEnhancement.SaveEnhancementDetails(objEnhancement);

                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " Enhancement ");
                        }

                        if (Arr[1].ToString() == "0")
                            {
                                hdfWFDataId.Value = objEnhancement.sWFDataId;
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


                    SaveDocumments(objEnhancement);
                    dtDocs = (DataTable)ViewState["DOCUMENTS"];
                    objEnhancement.dtDocuments = dtDocs;

                    Arr = objEnhancement.SaveEnhancementDetails(objEnhancement);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " Enhancement ");
                    }

                    string sOfcCode = objSession.OfficeCode;
                        string sdtcCode = txtDTCCode.Text;
                        string sWoid = objEnhancement.getWoIDforEstimation(sOfcCode, sdtcCode);

                        if (Arr[1].ToString() == "0")
                        {
                            txtEnhancementId.Text = objEnhancement.sEnhancementId;
                            cmdSave.Text = "Update";
                            ShowMsgBox(Arr[0].ToString());
                            cmdSave.Enabled = false;
                            //EstimationReportSO(sWoid);
                            return;
                        }
                        if (Arr[1].ToString() == "1")
                        {
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

        public void SaveDocumments(clsEnhancement objEnhancement)
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
                string sMainFolderName = "ENHANCEMENTDOCS";

                string sName = Convert.ToString(dtDocs.Rows[0]["NAME"]);
                string sPath = Convert.ToString(dtDocs.Rows[0]["PATH"]);

                if (File.Exists(sPath))
                {
                    bool IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName + "/" );
                    if (IsExists == false)
                    {

                        objFtp.createDirectory(SFTPmainfolder + sMainFolderName + "/"  );
                    }
                    IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName + "/" + objEnhancement.sDtcCode + "/");
                    if (IsExists == false)
                    {

                        objFtp.createDirectory(SFTPmainfolder + sMainFolderName + "/" + objEnhancement.sDtcCode);
                    }
                    Isuploaded = objFtp.upload(SFTPmainfolder + sMainFolderName + "/" + objEnhancement.sDtcCode + "/", sName, sPath);
                    if (Isuploaded == true & File.Exists(sPath))
                    {
                        File.Delete(sPath);
                        sPath =  sMainFolderName + "/" + objEnhancement.sDtcCode + "/" + sName;
                    }
                }
                dtDocs.Rows[0]["PATH"] = sPath;

                ViewState["DOCUMENTS"] = dtDocs;
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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


      public void GetEnhancementDetails()
      {
          try
          {                
              clsEnhancement objEnhancement = new clsEnhancement();  

              DataTable dtDetails = new DataTable();
              objEnhancement.sEnhancementId = txtEnhancementId.Text;            
              objEnhancement.sDtcId = txtDtcId.Text;

              objEnhancement.GetEnhancementDetails(objEnhancement);

              txtDtcId.Text = objEnhancement.sDtcId;
              txtDTCCode.Text = objEnhancement.sDtcCode;
              txtDTCName.Text = objEnhancement.sDtcName;
              txtServiceDate.Text = objEnhancement.sServicedate;
              txtLoadKW.Text = objEnhancement.sLoadKw;
              txtLoadHP.Text = objEnhancement.sLoadHp;
              txtConnectionDate.Text = objEnhancement.sCommissionDate;
              txtCapacity.Text = objEnhancement.sCapacity;
              txtLocation.Text = objEnhancement.sLocation;
              txtTcCode.Text = objEnhancement.sTcCode;
              txtTCSlno.Text = objEnhancement.sTcSlno;
              txtTCMake.Text = objEnhancement.sTcMake;
              txtEnhanceDate.Text = objEnhancement.sEnhancementDate ;
              txtReason.Text = objEnhancement.sReason;
              txtDTCRead.Text = objEnhancement.sDtcReadings;
              hdfTCId.Value = objEnhancement.sTCId;
              txtDTrCommDate.Text = objEnhancement.sDtrcommdate;

              txtDTCCode.Enabled = false;
              hdfEnhanceOffcode.Value = objEnhancement.sOfficeCode;
              hdfCrBy.Value = objEnhancement.sCrby;
              if (objEnhancement.sEnhancementId  != "0")
              {
                  //cmdSave.Text = "Update";
                  cmdSearch.Visible = false;
              }

              if (objEnhancement.sEnhancedCapacity == null || objEnhancement.sEnhancedCapacity == "")
              {
                    //Genaral.Load_Combo("SELECT \"MD_NAME\",\"MD_NAME\" from \"TBLMASTERDATA\" WHERE \"MD_TYPE\" ='C' AND \"MD_NAME\" <> '" + txtCapacity.Text + "' ORDER BY \"MD_ORDER_BY\"", "--Select--", cmbCapacity);                    
                    Genaral.Load_Combo(" select \"MD_NAME\",\"MD_NAME\" from \"TBLMASTERDATA\" WHERE \"MD_TYPE\" ='C'  AND cast(\"MD_NAME\" as int) > '" + txtCapacity.Text + "' ORDER BY \"MD_ORDER_BY\" ", "--Select--", cmbCapacity);
                }
              else
              {
                    Genaral.Load_Combo("SELECT \"MD_NAME\",\"MD_NAME\" from \"TBLMASTERDATA\" WHERE \"MD_TYPE\" ='C' AND \"MD_NAME\" <> '" + txtCapacity.Text + "' ORDER BY \"MD_ORDER_BY\"", "--Select--", cmbCapacity);
                    cmbCapacity.SelectedValue = objEnhancement.sEnhancedCapacity.Trim();
                    //Genaral.Load_Combo("SELECT \"MD_NAME\",\"MD_NAME\" from \"TBLMASTERDATA\" WHERE \"MD_TYPE\" ='C' ORDER BY \"MD_ORDER_BY\"", "--Select--", cmbCapacity);
                }

              if (txtEnhancementId.Text != "0")
              {
                  cmdSave.Text = "View";
              }
              clsEnhancement objGetCap = new clsEnhancement();
              string FailCap = objGetCap.GetTCCapacity(txtDtcId.Text);
              //Genaral.Load_Combo("SELECT \"MD_NAME\",\"MD_NAME\" from \"TBLMASTERDATA\" WHERE \"MD_TYPE\" ='C' AND \"MD_NAME\" <> '" + FailCap + "' ORDER BY \"MD_ORDER_BY\"", "--Select--", cmbCapacity);

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
              clsFailureEntry objFailure = new clsFailureEntry();
              //txtDTCCode.Text = hdfDTCcode.Value;
              objFailure.sDtcCode = txtDTCCode.Text;

              objFailure.SearchFailureDetails(objFailure);

              txtDtcId.Text = objFailure.sDtcId;
              txtDTCCode.Text = objFailure.sDtcCode;
              txtDTCName.Text = objFailure.sDtcName;
              //txtServiceDate.Text = objFailure.sDtcServicedate;
              txtLoadKW.Text = objFailure.sDtcLoadKw;
              txtLoadHP.Text = objFailure.sDtcLoadHp;
              txtConnectionDate.Text = objFailure.sCommissionDate;
              txtCapacity.Text = objFailure.sDtcCapacity;
              txtLocation.Text = objFailure.sDtcLocation;
              txtTcCode.Text = objFailure.sDtcTcCode;
              txtTCSlno.Text = objFailure.sDtcTcSlno;
              txtTCMake.Text = objFailure.sDtcTcMake;

              txtDTCCode.Enabled = false;
                //txtLastRepairDate.Text = objFailure.sLastRepairedDate;
                //txtLastRepairer.Text = objFailure.sLastRepairedBy;
                //cmbGuarenteeType.SelectedValue = objFailure.sGuarantyType;

                //txtManfDate.Text = objFailure.sManfDate;
                //txtDTrCommDate.Text = objFailure.sDTrCommissionDate;
                clsEnhancement objGetCap = new clsEnhancement();
                string FailCap = objGetCap.GetTCCapacity(txtDtcId.Text);
                Genaral.Load_Combo("SELECT \"MD_NAME\",\"MD_NAME\" from \"TBLMASTERDATA\" WHERE \"MD_TYPE\" ='C' AND \"MD_NAME\" <> '" + FailCap + "' ORDER BY \"MD_ORDER_BY\"", "--Select--", cmbCapacity);

                if (txtDTCName.Text.Trim() == "")
              {
                  EmptyDTCDetails();
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
              txtDTCCode.Text = string.Empty;
              txtDTCName.Text = string.Empty;
              txtServiceDate.Text = string.Empty;
              txtLoadKW.Text = string.Empty;
              txtLoadHP.Text = string.Empty;
              txtConnectionDate.Text = string.Empty;
              txtCapacity.Text = string.Empty;
              txtLocation.Text = string.Empty;
              txtTCSlno.Text = string.Empty;
              txtTcCode.Text = string.Empty;
              txtTCMake.Text = string.Empty;
              txtEnhanceDate.Text = string.Empty;
              txtReason.Text = string.Empty;
              txtDTCRead.Text = string.Empty;
              txtDtcId.Text = string.Empty;
              txtEnhancementId.Text = string.Empty;
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
                cmdSearch.Visible = true;
              txtDTCCode.Enabled = true;
          }
          catch (Exception ex)
          {
              lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
      }

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
                  Response.Redirect("EnhancementView.aspx", false);
              }

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
              clsEnhancement objEnhancement = new clsEnhancement();
              if (objEnhancement.ValidateEnhancementUpdate(txtEnhancementId.Text) == true)
              {
                  cmdReset.Enabled = false;
                  //cmdSave.Enabled = false;
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


      public bool  ValidateForm()
      {
          bool bValidate = false;
          try
          {             
              if (cmbCapacity.SelectedIndex == 0)
              {
                  cmbCapacity.Focus();
                  ShowMsgBox("Select Enhanced Capacity");
                  return bValidate;
              }

              if (txtReason.Text.Trim() == "")
              {
                  txtReason.Focus();
                  ShowMsgBox("Enter the Enhancement Reason");
                  return bValidate;
              }


               if(ValidateLTVR_File() == true || (txtActiontype.Text == "A" || txtActiontype.Text == "M" || txtActiontype.Text == "R"))
                {
                    return true;
                }
                else
                {
                    return false;
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


        protected bool ValidateLTVR_File()
        {
            string sFileName = string.Empty;
            string sDirectory = string.Empty;
            try
            {                
                if(fupLTVR.PostedFile != null)
                {
                    if (txtActiontype.Text != "M")
                    {
                        if (fupLTVR.PostedFile.ContentLength == 0)
                        {
                            ShowMsgBox("Please Select the LTVR File");
                            fupLTVR.Focus();
                            return false;
                        }
                    

                    string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FileFormat"]);
                    string sLTVRFileExt = System.IO.Path.GetExtension(fupLTVR.FileName).ToString().ToLower();
                    sLTVRFileExt = ";" + sLTVRFileExt.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sLTVRFileExt))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sFileName = Path.GetFileName(fupLTVR.PostedFile.FileName).Replace(",","");

                    fupLTVR.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sFileName));
                    sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sFileName);
                    }
                    DataTable dt = new DataTable();

                    if (ViewState["DOCUMENTS"] == null)
                    {
                        //dt.Columns.Add("ID");
                        dt.Columns.Add("NAME");
                        //dt.Columns.Add("TYPE");
                        dt.Columns.Add("PATH");
                    }
                    else
                    {
                        dt = (DataTable)ViewState["DOCUMENTS"];
                    }

                    if (txtActiontype.Text != "M")
                    {
                        int Id = dt.Rows.Count + 1;
                        DataRow Row = dt.NewRow();
                        //Row["ID"] = Id;
                        Row["NAME"] = sFileName;
                        //Row["TYPE"] = cmbFileType.SelectedItem;
                        Row["PATH"] = sDirectory;
                        dt.Rows.Add(Row);                        
                    }
                    else
                    {
                        DataRow Row = dt.NewRow();
                        Row["NAME"] = HdfLTRVpath.Value.Split('/').GetValue(2);
                        Row["PATH"] = HdfLTRVpath.Value;
                        dt.Rows.Add(Row);                        
                    }

                    ViewState["DOCUMENTS"] = dt;

                    //grdDocuments.DataSource = dt;
                    //grdDocuments.DataBind();

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

        public void EmptyDTCDetails()
      {
          try
          {
              txtDTCName.Text = string.Empty;
              txtServiceDate.Text = string.Empty;
              txtLoadKW.Text = string.Empty;
              txtLoadHP.Text = string.Empty;
              txtConnectionDate.Text = string.Empty;
              txtCapacity.Text = string.Empty;
              txtLocation.Text = string.Empty;
              txtTcCode.Text = string.Empty;
              txtTCMake.Text = string.Empty;
              //txtTCSlno.Text = string.Empty;
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

              objApproval.sFormName = "Enhancement";
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

      public void WorkFlowObjects(clsEnhancement objEnhance)
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


              objEnhance.sFormName = "Enhancement";
              objEnhance.sOfficeCode = objSession.OfficeCode;
              objEnhance.sClientIP = sClientIP;

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
              string sRole = Genaral.GetFirstLevelRole(objSession.RoleId);
                         
              if (txtActiontype.Text == "A")
              {
                  cmdSave.Text = "Approve";
                    if (sRole == "")
                    {
                        pnlApproval.Enabled = false;
                        DivUpload.Visible = false;
                        DivDownload.Visible = true;
                    }                    
                }
              else if (txtActiontype.Text == "R")
              {
                  cmdSave.Text = "Reject";
                  pnlApproval.Enabled = false;
                    DivUpload.Visible = false;
                    DivDownload.Visible = true;
                }
             else if (txtActiontype.Text == "M")
              {
                  cmdSave.Text = "Modify and Approve";
                  pnlApproval.Enabled = true;
                    DivUpload.Visible = true;
                    DivDownload.Visible = true;
                }
                else if (txtActiontype.Text == "V")
                {
                    DivDownload.Visible = true;
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
              objApproval.sWFObjectId = hdfWFOId.Value ;
              objApproval.sWFAutoId = hdfWFOAutoId.Value;
                objApproval.sbfm_type = "1";
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
                  bResult = objApproval.ModifyApproveWFRequest(objApproval);
              }
              else
              {
                  bResult = objApproval.ApproveWFRequest(objApproval);
              }
              
              if (bResult == true)
              {
                    if(objApproval.sApproveStatus == "1" || objApproval.sApproveStatus == "2" || objApproval.sApproveStatus == "3")
                    {
                        SaveFinalDocumments(objApproval);                        
                        UpdatePath(objApproval.sNewRecordId, objApproval.sFilePath);
                    }

                  if (objApproval.sApproveStatus == "1")
                  {
                      ShowMsgBox("Approved Successfully");
                      txtEnhancementId.Text = objApproval.sNewRecordId;

                      cmdSave.Enabled = false;

                      if (objSession.RoleId == "1")
                      {
                          clsFailureEntry objFailure = new clsFailureEntry();
                          //objFailure.SendSMStoSectionOfficer(hdfEnhanceOffcode.Value, txtDTCCode.Text);
                          //EstimationReport();
                      }
                      if (objSession.RoleId == "4")
                      {
                          clsEnhancement objEnhancen = new clsEnhancement();
                          string sWoid = objEnhancen.getWoIDforEstimation(objSession.OfficeCode, txtDTCCode.Text);
                          //EstimationReportSO(sWoid);
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
                      txtEnhancementId.Text = objApproval.sNewRecordId;
                      cmdSave.Enabled = false;

                        if (objSession.RoleId == "1")
                        {
                            clsFailureEntry objFailure = new clsFailureEntry();
                            objFailure.SendSMStoSectionOfficer(hdfEnhanceOffcode.Value, txtDTCCode.Text);
                            //EstimationReport();
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

        public void UpdatePath(string sDF_ID,string sFile_Path)
        {
            try
            {
                clsEnhancement objUpdate = new clsEnhancement();
                objUpdate.UpdatePath(sDF_ID, sFile_Path);
            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
      public void WorkFlowConfig()
      {
          try
          {
              if (Request.QueryString["ActionType"] != null && Convert.ToString(Request.QueryString["ActionType"]) != "")
              {


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

                  GetEnhancementDetailsFromXML(hdfWFDataId.Value);
                  SetControlText();
                  ControlEnableDisable();
                  if (txtActiontype.Text == "V")
                  {
                      cmdSave.Text = "View";
                      //cmdSave.Enabled = false;
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

      public bool CheckFormCreatorLevel()
      {
          try
          {
              clsApproval objApproval = new clsApproval();
              string sResult = objApproval.GetFormCreatorLevel("", objSession.RoleId, "Enhancement");
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

      public void LoadSearchWindow()
      {
          try
          {
             

              string strQry = string.Empty;
              strQry += "Title=Search and Select Transformer Centre Details&";
              strQry += "Query=select \"DT_CODE\",\"DT_NAME\" FROM \"TBLDTCMAST\" WHERE CAST(\"DT_OM_SLNO\" AS TEXT) LIKE '" + objSession.OfficeCode + "%' AND ";
              strQry += " \"DT_CODE\" NOT IN (SELECT \"DF_DTC_CODE\" FROM \"TBLDTCFAILURE\" WHERE  \"DF_REPLACE_FLAG\" = 0) AND {0} like %{1}% order by \"DT_CODE\" &";
              strQry += "DBColName=\"DT_CODE\"~\"DT_NAME\"&";
              strQry += "ColDisplayName=Transformer Centre Code~Transformer Centre Name&";

              strQry = strQry.Replace("'", @"\'");

              cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtDTCCode.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + txtDTCCode.ClientID + ")");

              txtEnhanceDate.Attributes.Add("onblur", "return ValidateDate(" + txtEnhanceDate.ClientID + ");");
          }
          catch (Exception ex)
          {
              lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
      }

      #region Load From XML
      public void GetEnhancementDetailsFromXML(string sWFDataId)
      {
          try
          {
              if (!txtEnhancementId.Text.Contains("-"))
              {
                  return;
              }

              clsEnhancement objEnhancement = new clsEnhancement();

              DataTable dtDetails = new DataTable();
              objEnhancement.sWFDataId = sWFDataId;

              objEnhancement.GetEnhancementDetailsFromXML(objEnhancement);

              txtDtcId.Text = objEnhancement.sDtcId;
              txtDTCCode.Text = objEnhancement.sDtcCode;
              txtDTCName.Text = objEnhancement.sDtcName;
              txtServiceDate.Text = objEnhancement.sServicedate;
              txtLoadKW.Text = objEnhancement.sLoadKw;
              txtLoadHP.Text = objEnhancement.sLoadHp;
              txtConnectionDate.Text = objEnhancement.sCommissionDate;
              txtCapacity.Text = objEnhancement.sCapacity;
              txtLocation.Text = objEnhancement.sLocation;
              txtTcCode.Text = objEnhancement.sTcCode;
              txtTCSlno.Text = objEnhancement.sTcSlno;
              txtTCMake.Text = objEnhancement.sTcMake;
              txtEnhanceDate.Text = objEnhancement.sEnhancementDate;
              txtReason.Text = objEnhancement.sReason;
              txtDTCRead.Text = objEnhancement.sDtcReadings;
              txtDTCCode.Enabled = false;
              hdfEnhanceOffcode.Value = objEnhancement.sOfficeCode;
              hdfCrBy.Value = objEnhancement.sCrby;
              txtDTrCommDate.Text = objEnhancement.sDtrcommdate;
              HdfLTRVpath.Value = objEnhancement.sFilePath;              
              if (objEnhancement.sEnhancementId != "0")
              {
                  //cmdSave.Text = "Update";
                  cmdSearch.Visible = false;
              }


              if (objEnhancement.sEnhancedCapacity != "")
              {
                    Genaral.Load_Combo("SELECT \"MD_NAME\",\"MD_NAME\" from \"TBLMASTERDATA\" WHERE \"MD_TYPE\" ='C' AND \"MD_NAME\" <> '" + objEnhancement.sCapacity + "' ORDER BY \"MD_ORDER_BY\"", "--Select--", cmbCapacity);
                    cmbCapacity.SelectedValue = objEnhancement.sEnhancedCapacity.Trim();
              }

          }
          catch (Exception ex)
          {
              lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

      }
        #endregion

        #region Unused function
        //public void EstimationReport()
        //{
        //    try
        //    {
        //        if (txtEnhancementId.Text.Contains("-"))
        //        {
        //            return;
        //        }
        //        //if (cmdSave.Text == "Save")
        //        //{
        //        clsEstimation objEst = new clsEstimation();
        //        objEst.sOfficeCode = hdfEnhanceOffcode.Value;
        //        objEst.sFailureId = txtEnhancementId.Text;
        //        objEst.sCrby = objSession.UserId;
        //        //objEst.sLastRepair = txtLastRepairer.Text;

        //        objEst.SaveEstimationDetails(objEst);
        //          //}

        //          string strParam = string.Empty;
        //          strParam = "id=EnhanceEstimation&EnhanceId=" + txtEnhancementId.Text ;
        //          RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
        //      }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //          clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //      }
        //}
        #endregion

        protected void lnkDTrDetails_Click(object sender, EventArgs e)
      {
          try
          {
              string sTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfTCId.Value));

              string url = "/MasterForms/TcMaster.aspx?TCId=" + sTCId;
              //string s = "window.open('" + url + "', 'popup_window', 'width=300,height=100,left=100,top=100,resizable=yes');";
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
              string sDTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtDtcId.Text));

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

      #region FunForEstimationSO

      public void EstimationReportSO(string sWoID)
      {
          try
          {
              string STCcode = txtTcCode.Text;
              string strParam = string.Empty;
              strParam = "id=EnhanceEstimationSO&TCcode=" + STCcode + "&WOId=" + sWoID;
              RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
          }
          catch (Exception ex)
          {
              lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
      }

        #endregion

        protected void lnkDwnld_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable DtLTVRDoc = new DataTable();
                clsEnhancement objGetLTVR_FilePath = new clsEnhancement();                

                DtLTVRDoc = objGetLTVR_FilePath.GetLTVR_FilePath(hdfWFDataId.Value, txtEnhancementId.Text);

                hdfLTVRPath.Value = Convert.ToString(DtLTVRDoc.Rows[0]["PATH"]);
                string sFileName = Convert.ToString(DtLTVRDoc.Rows[0]["NAME"]);
                if (DtLTVRDoc.Columns.Contains("PATH"))
                {
                    download(hdfLTVRPath.Value, sFileName);
                }
                else
                {
                    ShowMsgBox("File not Exist");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        private void download(string sFilePath,string sFileName)
        {
            //System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            bool endRequest = false;
            try
            {
                if (txtEnhancementId.Text.Contains("-"))
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
                        string url = sVirtualDirpath + sFilePath.Trim();
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

                    //String FTP_HOST = ConfigurationManager.AppSettings["FTP_HOST_DOC"].ToString();
                    //String FTP_USER = ConfigurationManager.AppSettings["FTP_USER"].ToString();
                    //String FTP_PASS = ConfigurationManager.AppSettings["FTP_PASS"].ToString();
                    //String ApkFileName = ConfigurationManager.AppSettings["ApkFileName"].ToString();
                    ////string sAnxFileExt = System.IO.Path.GetExtension(fupAnx.FileName).ToString().ToLower();
                    ////string sFoldername = objApk.RetrieveLatestApkDetails();
                    //FTP_HOST = FTP_HOST + sFilePath.Trim();
                    //FTP_HOST = FTP_HOST.Replace(" ", "");
                    //FtpWebRequest request = (FtpWebRequest)WebRequest.Create(FTP_HOST);
                    //request.Method = WebRequestMethods.Ftp.DownloadFile;

                    ////Enter FTP Server credentials.
                    //request.Credentials = new NetworkCredential(FTP_USER, FTP_PASS);
                    //request.UsePassive = true;
                    //request.UseBinary = true;
                    //request.EnableSsl = false;

                    //FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                    //using (MemoryStream stream = new MemoryStream())
                    //{
                    //    //Stream responseStream = response.GetResponseStream();
                    //    response.GetResponseStream().CopyTo(stream);
                    //    Response.AddHeader("content-disposition", "attachment;filename=" + sFileName);
                    //    Response.ContentType = "application/msi";
                    //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    //    Response.BinaryWrite(stream.ToArray());
                    //    Response.OutputStream.Close();
                    //}
                }
                else
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
                        string url = sVirtualDirpath + sFilePath.Trim();
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
            }
            catch (Exception ex)
            {
                if(ex.Message.Contains ("(404) Not Found"))
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

        private string getFilename(string hreflink)
        {
            Uri uri = new Uri(hreflink);

            string filename = System.IO.Path.GetFileName(uri.LocalPath);

            return filename;
        }

        public void SaveFinalDocumments(clsApproval objApproval)
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
                clsEnhancement objGetLTVR_FilePath = new clsEnhancement();
                dtDocs = objGetLTVR_FilePath.GetLTVR_FilePath(hdfWFDataId.Value, txtEnhancementId.Text);
                string FILE_VIRTUAL_PATH = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FILE_VIRTUAL_PATH"]);
                string sFromPath = FILE_VIRTUAL_PATH + objApproval.sDataReferenceId;
                string sToPath = sFromPath.Replace(objApproval.sDataReferenceId, objApproval.sNewRecordId);



                //   clsFtp objFtp = new clsFtp(sFTPLink, sFTPUserName, sFTPPassword);
                clsSFTP objFtp = new clsSFTP(SFTPPath, sFTPUserName, sFTPPassword);

                string sMainFolderName = "ENHANCEMENTDOCS";

                string sName = Convert.ToString(dtDocs.Rows[0]["NAME"]);
                string sPath = Convert.ToString(dtDocs.Rows[0]["PATH"]);
                objApproval.sFilePath = sPath.Replace(objApproval.sDataReferenceId, objApproval.sNewRecordId);
                //sPath = sFTPLink + sMainFolderName + "/" + objApproval.sNewRecordId;

                if (!Directory.Exists(sToPath))
                {
                    Directory.CreateDirectory(sToPath);
                }

                //bool IsExists = objFtp.FtpDirectoryExists(sToPath , sFTPUserName, sFTPPassword);
                //if (IsExists == false)
                //{

                //    objFtp.createDirectory(sMainFolderName + "/" + objApproval.sNewRecordId);
                //}

                //WindowsIdentity idnt = new WindowsIdentity("Administrator", "AdminIdea@2016+");

                //WindowsImpersonationContext context = idnt.Impersonate();
                //File.Move(pathToSourceFile, "\\\\Server\\Folder");
                //File.Copy(sFromPath +"\\" + sName, sToPath + "\\" + sName, true);

                //context.Undo();

                //  string sSourceFileName = Convert.ToString(drUnProcessed["file_name"]);
                sName = sName.Trim();
                string sFilePathName = sFromPath + "\\" + sName;
                string sDestinationDire = sToPath;
                string destFile = System.IO.Path.Combine(sDestinationDire, sName);
                if (!System.IO.Directory.Exists(sDestinationDire))
                {
                    System.IO.Directory.CreateDirectory(sDestinationDire);
                }
                System.IO.File.Copy(sFilePathName, destFile, true);
                File.Delete(sFilePathName);
                Directory.Delete(sFromPath);

                //string Fromfol = sFTPLink + Convert.ToString(dtDocs.Rows[0]["PATH"]);
                //string Tofol = Fromfol.Replace(objApproval.sDataReferenceId, objApproval.sNewRecordId);
                //Directory.Move( Fromfol, Tofol);
                //Isuploaded = objFtp.upload(sMainFolderName + "/" + objApproval.sNewRecordId + "/" + sName, sPath);
                //if (Isuploaded == true & File.Exists(sPath))
                //{
                //    File.Delete(sPath);
                //    sPath = sMainFolderName + "/" + objApproval.sNewRecordId + "/" + sName;


                //    //string Fromfol = "\\[Foldername] \\";
                //    //string Tofol = "\\[Foldername] \\";
                //    //Directory.Move(path + Fromfol, path + Tofol);

                //}

                //dtDocs.Rows[0]["PATH"] = sPath;

                //ViewState["DOCUMENTS"] = dtDocs;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

    }
}