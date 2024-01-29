using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Collections;
using System.Configuration;

namespace IIITS.DTLMS.TCRepair
{
    public partial class TCTesting : System.Web.UI.Page
    {

        string strFormCode = "TCTesting";
        clsSession objSession;

        string sUserName = Convert.ToString(ConfigurationManager.AppSettings["FTP_USER"]);
        string sPassword = Convert.ToString(ConfigurationManager.AppSettings["FTP_PASS"]);

        string SFTPPath = Convert.ToString(ConfigurationManager.AppSettings["SFTPPath"]);
        string SFTPmainfolder = Convert.ToString(ConfigurationManager.AppSettings["SFTPmainfolder"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            string sOfficeCode = string.Empty;
            try
                {
                    if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                    {
                        Response.Redirect("~/Login.aspx", false);
                    }

                    objSession = (clsSession)Session["clsSession"];
                    lblMessage.Text = string.Empty;
                    Form.DefaultButton = cmdSave.UniqueID;
                //    txtIssueDate.Attributes.Add("readonly", "readonly");
                txtTestedOn.Attributes.Add("readonly", "readonly");
                TestedOnCalender.EndDate = System.DateTime.Now;
              //  CalendarExtender1.EndDate = System.DateTime.Now;
                if (!IsPostBack)
                    {

                        if (Request.QueryString["PoNo"] != null && Request.QueryString["PoNo"].ToString() != "")
                        {
                            if (Session["RepairDetailsId"] != null && Session["RepairDetailsId"].ToString() != "")
                            {
                                txtSelectedDetailsId.Text = Session["RepairDetailsId"].ToString();
                                Session["RepairDetailsId"] = null;
                            }
                        txtPONo.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["PoNo"]));                        

                        if (objSession.sRoleType == "1")
                        {
                            sOfficeCode = clsStoreOffice.GetStoreID(objSession.OfficeCode);
                        }
                        else
                        {
                            sOfficeCode = objSession.OfficeCode;
                        }

                        LoadTestingPendingTC();
                        }

                        

                        Genaral.Load_Combo("SELECT  CAST(\"SM_ID\" AS TEXT) StoreID,\"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"SM_STATUS\" ='A' ORDER BY \"SM_NAME\"", "--Select--", cmbStore);
                    //Genaral.Load_Combo("SELECT \"US_ID\",\"US_FULL_NAME\" FROM \"TBLUSER\" WHERE COALESCE(\"US_EFFECT_FROM\",NOW()  - 1 * interval '1' day )<= NOW() AND \"US_STATUS\"='A' AND \"US_OFFICE_CODE\"='" + sOfficeCode + "' or (\"US_OFFICE_CODE\"='"+ objSession.OfficeCode + "' and \"US_ROLE_ID\"=19) ORDER BY \"US_ID\"", "--Select--", cmbTestedBy);
                    Genaral.Load_Combo("SELECT \"US_ID\",\"US_FULL_NAME\" FROM \"TBLUSER\" WHERE COALESCE(\"US_EFFECT_FROM\",NOW()  - 1 * interval '1' day )<= NOW() AND \"US_STATUS\"='A' AND \"US_OFFICE_CODE\"='" + objSession.OfficeCode + "' and \"US_ROLE_ID\"=19 ORDER BY \"US_ID\"", "--Select--", cmbTestedBy);



                    //From DTR Tracker
                    if (Request.QueryString["TransId"] != null && Request.QueryString["TransId"].ToString() != "")
                        {
                            string sInspId = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TransId"]));
                            string sDTrCode = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["DTrCode"]));
                            LoadTestingDoneDTR(sDTrCode, sInspId);
                            cmdSave.Enabled = false;
                        }


                        txtTestedOn.Attributes.Add("onblur", "return ValidateDate(" + txtTestedOn.ClientID + ");");
                        GetStoreId();
                    }
                }
                catch (Exception ex)
                {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                }
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                string[] Arr = new string[2];

                //Check AccessRights
                bool bAccResult = CheckAccessRights("2");
                if (bAccResult == false)
                {
                    return;
                }


                if (ValidateForm() == true )
                {
                    //if (CheckDublicateImage() == false)
                    //{
                    //    ShowMsgBox("Dublicate Image Not Allowed");
                    //    return;
                    //}
                    clsDTrRepairActivity objTestPending = new clsDTrRepairActivity();

                    objTestPending.sTestedBy = cmbTestedBy.SelectedValue;
                    objTestPending.sStoreId = cmbStore.SelectedValue;
                    //objTestPending.sDesignation = cmbDesignation.SelectedValue;
                    objTestPending.sTestedOn = txtTestedOn.Text;
                    objTestPending.sPurchaseOrderNo = txtPONo.Text;
                    //  objTestPending.sIssueDate = txtIssueDate.Text;
                    objTestPending.sCrby = objSession.UserId;
                    objTestPending.SessionOfcCode = objSession.OfficeCode;

                    objTestPending.sTestLocation = cmbTestLocation.SelectedValue;

                    int i = 0;
                    objTestPending.sTestResult = "0";
                    bool bChecked = false;
                    if(grdDeliverDetails.Rows.Count==0)
                    {
                        ShowMsgBox("Please select atleast one Transformer to Inspect");
                        return;
                    }
                    string[] strQrylist = new string[grdDeliverDetails.Rows.Count];
                    DataTable dtimage = new DataTable();
                    DataColumn dc = new DataColumn("image");
                    dc.DataType = System.Type.GetType("System.Byte[]");
                    dtimage.Columns.Add(dc);
                    dtimage.Columns.Add("RSDID", typeof(string));

                    foreach (GridViewRow row in grdDeliverDetails.Rows)
                    {
                        // For Pass value will be 1 ;   For Fail value will be 0; For Scrap value will be 3; For Send to store (none) value will be 4
                        bool result = ((RadioButton)row.FindControl("rdbPass")).Checked;
                        Byte[] Buffer;
                        bChecked = false;
                        FileUpload fupDoc = (FileUpload)row.FindControl("fupdDoc");
                        if (result)
                        {
                            objTestPending.sTestResult = "1";
                            bChecked = true;
                        }

                        result = ((RadioButton)row.FindControl("rdbFail")).Checked;
                        if (result)
                        {
                            objTestPending.sTestResult = "0";
                            bChecked = true;
                        }
                        #region
                        //New Code For Scrap
                        //result = ((RadioButton)row.FindControl("rdbScrap")).Checked;
                        //if (result)
                        //{
                        //    objTestPending.sTestResult = "3";
                        //    bChecked = true;
                        //}

                        //result = ((RadioButton)row.FindControl("rdbSendToStore")).Checked;
                        //if (result)
                        //{
                        //    objTestPending.sTestResult = "4";
                        //    bChecked = true;
                        //}
                        #endregion

                        string sRemarks = ((TextBox)row.FindControl("txtRemarks")).Text;
                       
                        #region SaveImage
                        if (fupDoc.PostedFile.ContentLength != 0)
                        {
                            string filename = Path.GetFileName(fupDoc.PostedFile.FileName);
                            string strExt = filename.Substring(filename.LastIndexOf('.') + 1);
                            
                            //saving to local folder
                            if (fupDoc.PostedFile.FileName != null && fupDoc.PostedFile.FileName != "")
                            {
                                fupDoc.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + fupDoc.FileName));
                            }

                            if (strExt.ToLower().Equals("jpg") || strExt.ToLower().Equals("jpeg") ||
                                strExt.ToLower().Equals("png") || strExt.ToLower().Equals("gif")|| 
                                strExt.ToLower().Equals("pdf"))
                            {
                                Stream strm = fupDoc.PostedFile.InputStream;
                                Buffer = new byte[strm.Length];
                                strm.Read(Buffer, 0, (int)strm.Length);

                                if ((int)strm.Length > 102400)
                                {
                                    ShowMsgBox("File size should not be greater than 100 KB");
                                    return;
                                }

                                dtimage.Rows.Add(Buffer, ((Label)row.FindControl("lbltransid")).Text.Trim());

                                #region                                
                                bool Isuploaded;
                                clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);
                                string sMainFolderName = "REPAIRER_MT_TESTED_DOCS";
                                string RSD_ID = ((Label)row.FindControl("lbltransid")).Text.Trim();
                                string sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + fupDoc.FileName);
                                if (File.Exists(sDirectory))
                                {
                                    bool IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName);
                                    if (IsExists == false)
                                    {
                                        objFtp.createDirectory(SFTPmainfolder + sMainFolderName);
                                    }
                                    IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName + "/" + RSD_ID);
                                    if (IsExists == false)
                                    {
                                        objFtp.createDirectory(SFTPmainfolder + sMainFolderName + "/" + RSD_ID);
                                    }
                                    Isuploaded = objFtp.upload(SFTPmainfolder + sMainFolderName + "/" + RSD_ID + "/", filename, sDirectory);
                                    if (Isuploaded == true & File.Exists(sDirectory))
                                    {
                                        File.Delete(sDirectory);
                                        sDirectory = RSD_ID + "/" + filename;
                                        txtTestDocfilepath.Text = sMainFolderName + "/" + RSD_ID + "/" + filename;
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                ShowMsgBox("Invalid File, Please Upload JPG, PNG or PDF file format ");
                                return;
                            }
                            objTestPending.sFileName = txtTestDocfilepath.Text;
                        }
                        //else
                        //{
                        //    ShowMsgBox("Please upload the File");
                        //    return;
                        //}        
                        #endregion

                   
                        strQrylist[i] = ((Label)row.FindControl("lbltransid")).Text.Trim() + "~" + objTestPending.sTestResult + "~" + sRemarks.Replace("'", "`") + "~" + objTestPending.sFileName;
                        i++;

                        if (bChecked == false)
                        {
                            ShowMsgBox("Please Add Testing Result to Transformers");
                            return;
                        }
                    }
                    Session["fileupload"] = dtimage;
                    //check for image already saved
                    //if (CheckImageAlreaySaved() == false)
                    //{
                    //    ShowMsgBox("Can Not Upload Dublicate Image");
                    //    return;
                    //}
                    Arr = objTestPending.SaveTestingTCDetails(strQrylist, objTestPending, dtimage);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (TCTesting) Repair ");
                    }

                    if (Arr[1].ToString() == "0")
                    {
                        //ShowMsgBox(Arr[0].ToString());
                        grdDeliverDetails.DataSource = null;
                        grdDeliverDetails.DataBind();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID, "alert('" + Arr[0].ToString() + "'); location.href='TestPendingSearch.aspx';", true);
                        Reset();

                        return;
                    }
                    else
                    {
                        ShowMsgBox("No Transformer Exists to Inspect");
                    }
                }


            }

            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                lblMessage.Text = clsException.ErrorMsg();
            }
        }


        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "TCRepairIssue";
                if(objSession.RoleId == "19")
                {
                    objApproval.sFormName = "TestPendingSearch";
                }
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

        #region SaveImages

        public bool SaveFilePath(clsStoreEnumeration objStoreEnum)
        {
            try
            {
                //FTP Parameter
                string sFTPLink = string.Empty;
                string sFTPUserName = string.Empty;
                string sFTPPassword = string.Empty;
                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);


                string sTestFileExt = string.Empty;
                string sTestFileName = string.Empty;
                string sDirectory = string.Empty;

                //  Photo Save DTLMSDocs

                clsCommon objComm = new clsCommon();
                DataTable dt = objComm.GetAppSettings();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPLINK")
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

                //     clsFtp objFtp = new clsFtp(sFTPLink, sFTPUserName, sFTPPassword);
                clsSFTP objFtp = new clsSFTP(SFTPPath, sFTPUserName, sFTPPassword);

                bool Isuploaded;

                // Create Directory

                bool IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + objStoreEnum.sEnumDetailsId + "/");
                if (IsExists == false)
                {

                    objFtp.createDirectory(SFTPmainfolder + objStoreEnum.sEnumDetailsId);
                }

                //  Photo Save DTLMSDocs
                string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["NotAllowFileFormat"]);

                if (fupTestDocument.PostedFile.ContentLength != 0)
                {

                    sTestFileExt = System.IO.Path.GetExtension(fupTestDocument.FileName).ToString().ToLower();
                    sTestFileExt = ";" + sTestFileExt.Remove(0, 1) + ";";

                    if (sFileExt.Contains(sTestFileExt))
                    {
                        ShowMsgBox("Invalid File Format");
                        return false;
                    }

                    sTestFileName = System.IO.Path.GetFileName(fupTestDocument.PostedFile.FileName);

                    fupTestDocument.SaveAs(Server.MapPath("~/DTLMSFiles" + "/" + sTestFileName));
                    sDirectory = Server.MapPath("~/DTLMSFiles" + "/" + sTestFileName);

                }


                if (sTestFileName != "")
                {

                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + objStoreEnum.sEnumDetailsId + "/" + "TESTING" + "/");
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(SFTPmainfolder + objStoreEnum.sEnumDetailsId + "/" + "TESTING");
                        }

                        Isuploaded = objFtp.upload(SFTPmainfolder + objStoreEnum.sEnumDetailsId + "/" + "TESTING" + "/" + objSession.UserId, sTestFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objStoreEnum.sNamePlatePhotoPath = SFTPmainfolder + objStoreEnum.sEnumDetailsId + "/" + "TESTING" + "/" + objSession.UserId + "~" + sTestFileName;

                        }
                    }
                }

                bool bResult;
                //if (txtEnumDetailsId.Text.Trim() == "")
                //{
                //    bResult = objStoreEnum.SaveImagePathDetails(objStoreEnum);
                //}
                //else
                //{
                //    bResult = objStoreEnum.UpdateImagePathDetails(objStoreEnum);
                //}

                return true;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                lblMessage.Text = clsException.ErrorMsg();
                return false;
            }
        }

        #endregion
        private void LoadTestingPendingTC()
        {
            try
            {
                clsDTrRepairActivity objTestpending = new clsDTrRepairActivity();

                DataTable dt = new DataTable();
                txtSelectedDetailsId.Text = txtSelectedDetailsId.Text.Replace("~", ",");
                if (!txtSelectedDetailsId.Text.StartsWith(","))
                {
                    txtSelectedDetailsId.Text = "," + txtSelectedDetailsId.Text;
                }
                if (!txtSelectedDetailsId.Text.EndsWith(","))
                {
                    txtSelectedDetailsId.Text = txtSelectedDetailsId.Text + ",";
                }

                if(txtSelectedDetailsId.Text.Length<=1)
                {
                    return;
                }
                txtSelectedDetailsId.Text = txtSelectedDetailsId.Text.Substring(1, txtSelectedDetailsId.Text.Length - 1);
                txtSelectedDetailsId.Text = txtSelectedDetailsId.Text.Substring(0, txtSelectedDetailsId.Text.Length - 1);

                objTestpending.sRepairDetailsId = txtSelectedDetailsId.Text;
                #region this convertion of store code already present in the below (LoadTestOrDeliverPendingDTR)method so commented

                //if (objSession.sRoleType == "1")    
                //{
                //    objTestpending.sOfficeCode = clsStoreOffice.GetStoreID(objSession.OfficeCode);
                //}
                //else
                //{
                //    objTestpending.sOfficeCode = objSession.OfficeCode;
                //}     
                #endregion
                objTestpending.sPurchaseOrderNo = txtPONo.Text;
                objTestpending.sTestingDone = "0";

                dt = objTestpending.LoadTestOrDeliverPendingDTR(objTestpending);
                if (dt.Rows.Count > 10)
                {
                    //div1.Style.Add("overflow", "scroll");
                }
                if (dt.Rows.Count > 0)
                {
                    txtPONo.Text = Convert.ToString(dt.Rows[0]["RSM_PO_NO"]);
                  //  txtIssueDate.Text = Convert.ToString(dt.Rows[0]["RSM_ISSUE_DATE"]);
                  //  txtOldPONo.Text = Convert.ToString(dt.Rows[0]["RSM_OLD_PO_NO"]);
                  //  txtPO_Remarks.Text = Convert.ToString(dt.Rows[0]["RSM_REMARKS"]);
                    DateTime Issuedate = Convert.ToDateTime(Convert.ToString(dt.Rows[0]["RSM_PO_DATE"]));
                    int Isuedate = Convert.ToInt32((DateTime.Now.AddDays(-1) - Issuedate).TotalDays);
                    TestedOnCalender.StartDate = Issuedate;
                    TestedOnCalender.EndDate = System.DateTime.Now;
                   // TestedOnCalender.StartDate = System.DateTime.Now;
                   //// TestedOnCalender.EndDate = System.DateTime.Now.AddDays(0);
                }
                grdDeliverDetails.DataSource = dt;
                grdDeliverDetails.DataBind();
                ViewState["TestDTR"] = dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                lblMessage.Text = clsException.ErrorMsg();
            }

        }



        private void LoadTestingDoneDTR(string sDTrCode, string sInspectId)
        {
            try
            {
                clsDTrRepairActivity objTestpending = new clsDTrRepairActivity();

                DataTable dt = new DataTable();

                objTestpending.sTcCode = sDTrCode;
                objTestpending.sTestInspectionId = sInspectId;

                objTestpending = objTestpending.LoadTestedDTR(objTestpending);

                dt = objTestpending.dtTestDone;

                if (dt.Rows.Count > 10)
                {
                    //div1.Style.Add("overflow", "scroll");
                }
                if (dt.Rows.Count > 0)
                {
                    txtPONo.Text = Convert.ToString(dt.Rows[0]["RSM_PO_NO"]);
                   // txtIssueDate.Text = Convert.ToString(dt.Rows[0]["RSM_ISSUE_DATE"]);

                    cmbTestedBy.SelectedValue = objTestpending.sTestedBy;
                    txtTestedOn.Text = objTestpending.sTestedOn;
                    cmbTestLocation.Text = objTestpending.sTestLocation;

                    hdfRemarks.Value = objTestpending.sInspRemarks;
                    hdfResult.Value = objTestpending.sTestResult;

                }
                grdDeliverDetails.DataSource = dt;
                grdDeliverDetails.DataBind();
                ViewState["TestDTR"] = dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                lblMessage.Text = clsException.ErrorMsg();
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

        public bool ValidateForm()
        {
            bool bValidate = false;
            try
            {
                if (cmbStore.SelectedIndex == 0)
                {
                    ShowMsgBox("Please Select the Store");
                    cmbStore.Focus();
                    return bValidate;
                }

                if (cmbTestedBy.SelectedIndex == 0)
                {
                    ShowMsgBox("Please Enter Tested By");
                    cmbTestedBy.Focus();
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
        void Reset()
        {
            try
            {
                //cmbStore.SelectedIndex = 0;

                cmbTestedBy.SelectedIndex = 0;
                txtTestedOn.Text = string.Empty;
                cmbTestLocation.SelectedIndex = 0;             

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetStoreId()
        {
            try
            {
                clsTcMaster objTcMaster = new clsTcMaster();
                cmbStore.SelectedValue = clsStoreOffice.GetStoreID( objSession.OfficeCode);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdDeliverDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "Remove")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                    int iRowIndex = row.RowIndex;

                    DataTable dt = (DataTable)ViewState["TestDTR"];
                    dt.Rows[iRowIndex].Delete();
                    if (dt.Rows.Count == 0)
                    {
                        ViewState["TestDTR"] = null;
                    }
                    else
                    {
                        ViewState["TestDTR"] = dt;
                    }

                    grdDeliverDetails.DataSource = dt;
                    grdDeliverDetails.DataBind();
                }
                //if (e.CommandName == "Download")
                //{
                //    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                //    int iRowIndex = row.RowIndex;

                //    DataTable dt = (DataTable)ViewState["TestDTR"];
                //    //dt.Rows[iRowIndex].Delete();
                //    //if (dt.Rows.Count == 0)
                //    //{
                //    //    ViewState["TestDTR"] = null;
                //    //}
                //    //else
                //    //{
                //    //    ViewState["TestDTR"] = dt;
                //    //}

                //    //grdDeliverDetails.DataSource = dt;
                //    //grdDeliverDetails.DataBind();
                //}

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdDeliverDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (cmbTestedBy.SelectedIndex > 0)
                    {
                        RadioButton rdbPass = (RadioButton)e.Row.FindControl("rdbPass");
                        RadioButton rdbFail = (RadioButton)e.Row.FindControl("rdbFail");
                        RadioButton rdbScrap = (RadioButton)e.Row.FindControl("rdbScrap");
                        RadioButton rdbSendToStore = (RadioButton)e.Row.FindControl("rdbSendToStore");
                        TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");

                        if (hdfResult.Value == "0")
                        {
                            rdbFail.Checked = false;
                        }
                        if (hdfResult.Value == "1")
                        {
                            rdbPass.Checked = true;
                        }
                        if (hdfResult.Value == "3")
                        {
                            rdbScrap.Checked = true;
                        }
                        if (hdfResult.Value == "4")
                        {
                            rdbSendToStore.Checked = true;
                        }

                        txtRemarks.Text = hdfRemarks.Value;

                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public bool CheckDublicateImage()
        {
            try
            {
                ArrayList TcDetails = new ArrayList();
                bool result = true;
                string filename = string.Empty;
                foreach (GridViewRow grdRow in grdDeliverDetails.Rows)
                {
                    bool IsFile = ((FileUpload)grdRow.FindControl("fupdDoc")).HasFile;

                    if (IsFile)
                    {
                        filename = ((FileUpload)(grdRow.FindControl("fupdDoc"))).FileName;

                        if (TcDetails.Contains(filename))
                            result = false;
                        else
                            result = true;
                        TcDetails.Add(filename);
                    }


                }
                return result;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
            
            
            
            }
        public bool CheckImageAlreaySaved()
        {
            try
            {
                clsDTrRepairActivity objTcActivity = new clsDTrRepairActivity();
                DataTable dbImage = objTcActivity.GetAllImages();
                DataTable UpldingImage = (DataTable)Session["fileupload"];
                Session["fileupload"] = null;
                for (int i = 0; i < dbImage.Rows.Count; i++)
                {
                    for (int j = 0; j < UpldingImage.Rows.Count; j++)
                    {
                        if ((dbImage.Rows[i][0] != DBNull.Value) && (UpldingImage.Rows[j][0] != DBNull.Value))
                        {
                            byte[] dbimg = (byte[])dbImage.Rows[i][0];
                            byte[] upimg = (byte[])UpldingImage.Rows[j][0];

                            if (dbimg.SequenceEqual(upimg))
                                return false;
                        }
                    }
                }
                return true;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }


       

    }
    


        #region Access Rights
        //public bool CheckAccessRights(string sAccessType)
        //{
        //    try
        //    {
        //        // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

        //        clsApproval objApproval = new clsApproval();

        //        objApproval.sFormName = "DeliverPending";
        //        objApproval.sRoleId = objSession.RoleId;
        //        objApproval.sAccessType = "1" + "," + sAccessType;
        //        bool bResult = objApproval.CheckAccessRights(objApproval);
        //        if (bResult == false)
        //        {
        //            if (sAccessType == "4")
        //            {
        //                Response.Redirect("~/UserRestrict.aspx", false);
        //            }
        //            else
        //            {
        //                ShowMsgBox("Sorry , You are not authorized to Access");
        //            }
        //        }
        //        return bResult;

        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CheckAccessRights");
        //        return false;

        //    }
        //}

        #endregion
    }
