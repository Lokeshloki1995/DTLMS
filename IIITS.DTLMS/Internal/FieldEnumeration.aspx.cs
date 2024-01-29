using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Collections;
using System.Data;
using System.IO;
using System.Net;
using System.Configuration;

namespace IIITS.DTLMS.Internal
{
    public partial class FieldEnumeration : System.Web.UI.Page
    {
        string strFormCode = "FieldEnumeration";
        clsSession objSession;
        int Circle_code = Convert.ToInt32(ConfigurationManager.AppSettings["Circle_code"]);
        int Division_code = Convert.ToInt32(ConfigurationManager.AppSettings["Division_code"]);
        int SubDiv_code = Convert.ToInt32(ConfigurationManager.AppSettings["SubDiv_code"]);
        int Section_code = Convert.ToInt32(ConfigurationManager.AppSettings["Section_code"]);
        int Feeder_code = Convert.ToInt32(ConfigurationManager.AppSettings["Feeder_code"]);
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];
                lblmessage.Text = string.Empty;

                RetainImageinPostbackSession();
                RetainImageOnPostback();

                txtManufactureDate.Attributes.Add("readonly", "readonly");
                txtwelddate.Attributes.Add("readonly", "readonly");

                txtCommisionDate.Attributes.Add("readonly", "readonly");
                txtDtrCommissionDate.Attributes.Add("readonly", "readonly");
                txtServiceDate.Attributes.Add("readonly", "readonly");
                txtManufactureDate_CalendarExtender.EndDate = System.DateTime.Now;

                txtCommisionDate_CalendarExtender1.EndDate = System.DateTime.Now;
                txtServiceDate_CalendarExtender1.EndDate = System.DateTime.Now;
                CalendarExtender1.EndDate = System.DateTime.Now;
                if (CheckAccessRights("4"))

                    if (!IsPostBack)
                    {
                        DateTime dt = System.DateTime.Now;
                        objSession = (clsSession)Session["clsSession"];
                        string stroffCode = string.Empty;
                        string stroffCode1 = string.Empty;
                        stroffCode = objSession.OfficeCode;
                        if (objSession.OfficeCode != "")
                        {
                            stroffCode1 = stroffCode;
                        }
                        if (objSession.OfficeCode != "")
                        {
                            Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" Where \"CM_CIRCLE_CODE\"= '" + objSession.OfficeCode.Substring(0, Circle_code) + "'  ORDER BY \"CM_CIRCLE_CODE\"", "--Select--", cmbCircle);
                            stroffCode = stroffCode.Substring(0, Circle_code);
                            cmbCircle.Items.FindByValue(stroffCode).Selected = true;
                            cmbCircle.Enabled = false;
                        }
                        else
                        {
                            Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\"  ORDER BY \"CM_CIRCLE_CODE\"", "--Select--", cmbCircle);

                        }
                        if (cmbCircle.SelectedIndex > 0)
                        {
                            if (objSession.OfficeCode != "")
                            {
                                Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\"  WHERE \"DIV_CODE\"='" + objSession.OfficeCode.Substring(0, Division_code) + "' AND \"DIV_CICLE_CODE\"='" + cmbCircle.SelectedValue + "' ORDER BY \"DIV_CODE\"", "--Select--", cmbDivision);
                            }
                            else
                            {
                                Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\"  WHERE  \"DIV_CICLE_CODE\"='" + cmbCircle.SelectedValue + "' ORDER BY \"DIV_CODE\"", "--Select--", cmbDivision);
                            }
                        }
                        else
                        {
                            cmbDivision.Items.Clear();
                            cmbsubdivision.Items.Clear();
                            cmbSection.Items.Clear();
                            cmbFeeder.Items.Clear();
                        }
                        string strQry = string.Empty;

                        objSession = (clsSession)Session["clsSession"];

                        if (objSession.OfficeCode.Length <= 2 && objSession.OfficeCode.Length != 0)
                        {
                            stroffCode = clsStoreOffice.GetZone_Circle_Div_Offcode(objSession.OfficeCode, objSession.RoleId).Substring(0, Constants.Circle);
                        }
                        else
                        {
                            stroffCode = objSession.OfficeCode;
                        }

                        if (stroffCode.Length >= 2)
                        {
                            if (stroffCode.Length >= 3)
                            {
                                stroffCode = stroffCode.Substring(0, Division_code);
                                cmbDivision.Items.FindByValue(stroffCode).Selected = true;
                                cmbDivision.Enabled = false;

                                stroffCode = stroffCode1;
                            }
                        }

                        if (stroffCode.Length >= 2)
                        {

                            if (objSession.OfficeCode != "")
                            {
                                Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\", \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\"  WHERE \"SD_SUBDIV_CODE\"='" + objSession.OfficeCode.Substring(0, SubDiv_code) + "' AND \"SD_DIV_CODE\"='" + cmbDivision.SelectedValue + "' ORDER BY \"SD_SUBDIV_CODE\"", "--Select--", cmbsubdivision);
                            }
                            else
                            {
                                Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\", \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\"  WHERE \"SD_DIV_CODE\"='" + cmbDivision.SelectedValue + "' ORDER BY \"SD_SUBDIV_CODE\"", "--Select--", cmbsubdivision);
                            }
                            if (stroffCode.Length >= 3)
                            {
                                stroffCode = stroffCode.Substring(0, SubDiv_code);
                                cmbsubdivision.Items.FindByValue(stroffCode).Selected = true;
                                cmbsubdivision.Enabled = false;

                                stroffCode = stroffCode1;
                            }
                            if (stroffCode.Length >= 4)
                            {
                                if (objSession.OfficeCode != "")
                                {
                                    Genaral.Load_Combo("SELECT \"OM_CODE\", \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_CODE\"='" + objSession.OfficeCode.Substring(0, Section_code) + "' AND \"OM_SUBDIV_CODE\" = '" + cmbsubdivision.SelectedValue + "' ORDER BY \"OM_CODE\"", "--Select--", cmbSection);
                                }
                                else
                                {
                                    Genaral.Load_Combo("SELECT \"OM_CODE\", \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE  \"OM_SUBDIV_CODE\" = '" + cmbsubdivision.SelectedValue + "' ORDER BY \"OM_CODE\"", "--Select--", cmbSection);
                                }
                                if (stroffCode.Length >= 4)
                                {
                                    string stroffCode2 = string.Empty;
                                    stroffCode = stroffCode.Substring(0, Section_code);
                                    cmbSection.Items.FindByValue(stroffCode).Selected = true;
                                    cmbSection.Enabled = false;
                                    stroffCode = stroffCode1;
                                    stroffCode2 = hdfFeeder.Value;
                                    cmbSection_SelectedIndexChanged(sender, e);

                                }
                            }
                        }
                        Genaral.Load_Combo("SELECT \"TM_ID\",\"TM_NAME\" FROM \"TBLTRANSMAKES\" ORDER BY \"TM_NAME\"", "--Select--", cmbMake);
                        Genaral.Load_Combo("SELECT \"MD_NAME\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'C'", "--Select--", cmbCapacity);

                        Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'PT'", "--Select--", cmbProjecttype);
                        Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'LT'", "--Select--", cmbLoadtype);
                        Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'PLT'", "--Select--", cmbPlatformType);
                        Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'BT'", "--Select--", cmbBreakerType);
                        Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'SR'", "--Select--", cmbRating);
                        Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'LC_TYPE'", "--Select--", cmdloctype);

                        cmbRework.SelectedValue = "2";
                        if (Request.QueryString["QryEnumId"] != null && Request.QueryString["QryEnumId"].ToString() != "")
                        {
                            txtEnumDetailsId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryEnumId"]));
                            txtStatus.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["Status"]));
                            if (Request.QueryString["FeederBifurcation"] != null && Request.QueryString["FeederBifurcation"].ToString() != "")
                            {
                                txtfeederbifurcation.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["FeederBifurcation"]));
                            }
                            GetEnumerationDetails(txtEnumDetailsId.Text);
                            RestrictUpdate(txtStatus.Text);

                            if (objSession.RoleId != "8")
                            {
                                cmbCircle_SelectedIndexChanged(sender, e);
                                cmbDivision.SelectedValue = hdfDivision.Value;
                                cmbDivision_SelectedIndexChanged(sender, e);
                                cmbsubdivision.SelectedValue = hdfSubdivision.Value;
                                cmbsubdivision_SelectedIndexChanged(sender, e);
                                cmbSection.SelectedValue = hdfSection.Value;
                                cmbSection_SelectedIndexChanged(sender, e);
                                cmbFeeder.SelectedValue = hdfFeeder.Value;
                            }

                            if (CheckAccessRights("4"))

                                if (cmbMake.SelectedValue == "1")
                                {
                                    string sTCSlno = txtTcslno.Text;
                                    cmbMake_SelectedIndexChanged(sender, e);
                                    txtTcslno.Text = sTCSlno;
                                }

                            if (cmbLightArrester.SelectedIndex == 0)
                            {
                                cmbLightArrester.SelectedValue = "1";
                            }

                            if (cmbHTProtection.SelectedIndex == 0)
                            {
                                cmbHTProtection.SelectedValue = "1";
                            }
                            if (cmbLTProtection.SelectedIndex == 0)
                            {
                                cmbLTProtection.SelectedValue = "1";
                            }
                            if (cmbPlatformType.SelectedIndex == 0)
                            {
                                cmbPlatformType.SelectedValue = "5";
                            }
                            if (cmbProjecttype.SelectedIndex == 0)
                            {
                                cmbProjecttype.SelectedValue = "7";
                            }
                            if (cmbGrounding.SelectedIndex == 0)
                            {
                                cmbGrounding.SelectedValue = "1";
                            }
                        }

                        string edfeeder = txtfeederbifurcation.Text;

                        if (txtwelddate.Text == null || txtwelddate.Text == "")
                        {
                            DateTime currentdate = DateTime.Now;
                            txtwelddate.Text = currentdate.ToString("dd/MM/yyyy");
                        }

                        double Time = (System.DateTime.Now - dt).TotalSeconds;

                    }

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to check access to the form
        /// </summary>
        /// <param name="sAccessType"></param>
        /// <returns></returns>
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY
                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "Feild Enumeration";
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
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

                return false;

            }
        }

        //UnUsed Method

        /// <summary>
        /// to load next enumeration details
        /// </summary>
        //private void GetNextEnumerationDetails()
        //{
        //    try
        //    {
        //        objSession = (clsSession)Session["clsSession"];
        //        clsFieldEnumeration objEnum = new clsFieldEnumeration();
        //        objEnum.sEnumDTCID = txtEnumDetailsId.Text;
        //        objEnum.sStatus = txtStatus.Text;
        //        objEnum.sUserType = objSession.UserType;
        //        objEnum.sFeederCode = cmbFeeder.SelectedValue.ToString();
        //        objEnum.sDTCCode = txtDTCCode.Text;
        //        objEnum.GetNextEnumerationDetails(objEnum);

        //        if (objEnum.sEnumDTCID.Length > 0)
        //        {
        //            string sEnumerationId = objEnum.sEnumDTCID;
        //            string sStatus = objEnum.sStatus;

        //            string sEnumDetailsId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sEnumerationId));
        //            string sStatusFlag = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sStatus));

        //            Response.Redirect("FieldEnumeration.aspx?QryEnumId=" + sEnumDetailsId + "&Status=" + sStatusFlag, false);
        //        }
        //        else
        //        {
        //            ShowMsgBox("No Details Exists for the Selected Feeder Code");
        //            Response.Redirect("EnumerationView.aspx", false);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}

        /// <summary>
        /// to get enumeration details
        /// </summary>
        /// <param name="strRecordId"></param>
        protected void GetEnumerationDetails(string strRecordId)
        {
            try
            {
                clsFieldEnumeration objfield = new clsFieldEnumeration();


                objfield.sEnumDetailsID = strRecordId;

                objfield.GetEnumerationDetails(objfield);


                if (objfield.sDTCWithoutDTR != "" && objfield.sDTCWithoutDTR != null)
                {
                    Mdtrmake.Visible = false;
                    Mplatenum.Visible = false;
                    Mrating.Visible = false;
                    lblNote.Text = "Note: DTC Without DTR Enumaration";
                    lblNote.Visible = true;
                    hdfDTCWithoutDTrFlag.Value = objfield.sDTCWithoutDTR;
                }

                cmbCircle.SelectedValue = objfield.sOfficeCode.Substring(0, Circle_code);
                hdfDivision.Value = objfield.sOfficeCode.Substring(0, Division_code);
                hdfSubdivision.Value = objfield.sOfficeCode.Substring(0, SubDiv_code);
                hdfSection.Value = objfield.sOfficeCode.Substring(0, Section_code);
                hdfFeeder.Value = objfield.sFeederCode.Substring(0, Feeder_code);
                cmbTaggedDTR.SelectedValue = objfield.staggedDTR;

                if (objSession.RoleId == "8")
                {
                    Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" ORDER BY \"DIV_CODE\"", "--Select--", cmbDivision);
                    Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\", \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\"  ORDER BY \"SD_SUBDIV_CODE\"", "--Select--", cmbsubdivision);
                    Genaral.Load_Combo("SELECT \"OM_CODE\", \"OM_NAME\" FROM \"TBLOMSECMAST\"  ORDER BY \"OM_CODE\"", "--Select--", cmbSection);
                    Genaral.Load_Combo("SELECT DISTINCT \"FD_FEEDER_CODE\",\"FD_FEEDER_CODE\" || '-' || \"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\", \"TBLFEEDEROFFCODE\" WHERE  \"FD_FEEDER_ID\" = \"FDO_FEEDER_ID\" ORDER BY \"FD_FEEDER_CODE\"", "--Select--", cmbFeeder);

                    cmbDivision.SelectedValue = hdfDivision.Value;
                    cmbsubdivision.SelectedValue = hdfSubdivision.Value;
                    cmbSection.SelectedValue = hdfSection.Value;
                    cmbFeeder.SelectedValue = hdfFeeder.Value;
                }
                txtDTCName.Text = objfield.sDTCName;
                txtwelddate.Text = objfield.sWeldDate;
                txtTcCode.Text = objfield.sTcCode;
                txtTcslno.Text = objfield.sTCSlno;

                txtDTCCode.Text = objfield.sDTCCode;
                txtoldDTCCode.Text = objfield.sOldDTCCode;
                txtIPDTCCode.Text = objfield.sIPDTCCode;
                if (objfield.sTCCapacity != "")
                {
                    cmbCapacity.SelectedValue = objfield.sTCCapacity;

                }
                if (Convert.ToDouble(objfield.sConnectedKW) == 0 && cmbCapacity.SelectedIndex > 0)
                {
                    cmbCapacity_SelectedIndexChanged(this, new EventArgs());
                }
                else
                {
                    txtConnectedKW.Text = objfield.sConnectedKW;
                    txtConnectedHP.Text = objfield.sConnectedHP;
                }
                cmbMake.SelectedValue = objfield.sTCMake;
                if (cmbMake.SelectedValue == "1")
                {
                    Div1.Style.Add("display", "none");
                }
                else
                {
                    Div1.Style.Add("display", "block");
                }
                txtIPDTCCode.Text = objfield.sIPDTCCode;
                txtInternalCode.Text = objfield.sInternalCode;
                if (cmbMake.SelectedValue == "1")
                {
                    txtManufactureDate.Text = "";
                }
                else
                {
                    txtManufactureDate.Text = objfield.sTCManfDate;
                }
                txtKWHReading.Text = objfield.sKWHReading;
                txtCommisionDate.Text = objfield.sCommisionDate;
                txtDtrCommissionDate.Text = objfield.sDtrcommissiondate;
                txtServiceDate.Text = objfield.sLastServiceDate;

                cmbPlatformType.SelectedValue = objfield.sPlatformType;
                cmbBreakerType.SelectedValue = objfield.sBreakertype;
                cmbDTCMetered.SelectedValue = objfield.sDTCMeters;
                cmbHTProtection.SelectedValue = objfield.sHTProtect;
                cmbLTProtection.SelectedValue = objfield.sLTProtect;
                cmbGrounding.SelectedValue = objfield.sGrounding;
                cmbLightArrester.SelectedValue = objfield.sArresters;
                cmbLoadtype.SelectedValue = objfield.sLoadtype;
                cmbProjecttype.SelectedValue = objfield.sProjecttype;
                txtltLine.Text = objfield.sLTlinelength;
                txtHtLine.Text = objfield.sHTlinelength;
                txtDepreciation.Text = objfield.sDepreciation;
                txtLatitude.Text = objfield.sLatitude;
                txtLongitude.Text = objfield.sLongitude;
                txtfeederbifurcation.Text = objfield.sEDFeederBifurcation;
                cmbFeeder.SelectedValue = hdfFeeder.Value;
                txtEnumDetailsId.Text = objfield.sEnumDetailsID;
                cmdloctype.SelectedValue = objfield.sLocation;
                string sApproveStatus = objfield.sApproveStatus;

                if (objfield.sIPCESCValue == "1")
                {
                    rdbDTLMS.Checked = true;

                    //rdbOldDtc.Enabled = false;
                    //rdbIPEnum.Enabled = false;
                }
                else if (objfield.sIPCESCValue == "2")
                {
                    //rdbOldDtc.Checked = true;

                    rdbDTLMS.Enabled = false;
                    //rdbIPEnum.Enabled = false;
                }
                else if (objfield.sIPCESCValue == "3")
                {
                    //rdbIPEnum.Checked = true;

                    //rdbOldDtc.Enabled = false;
                    rdbDTLMS.Enabled = false;
                }

                txtTankCapacity.Text = objfield.sTankCapacity;
                txtInfosysAsset.Text = objfield.sInfosysAsset;

                cmbRating.SelectedValue = objfield.sRating;
                hdfStarRate.Value = objfield.sStarRate;

                txtDTLMSDTCPath.Text = objfield.sDTLMSCodePhotoPath;
                txtOLDDTCPath.Text = objfield.sOldCodePhotoPath;
                txtIPDTCPath.Text = objfield.sIPEnumCodePhotoPath;
                txtInfosysPath.Text = objfield.sInfosysCodePhotoPath;
                txtDTCPath.Text = objfield.sDTCPhotoPath;

                txtSSPlatePath.Text = objfield.sSSPlatePhotoPath;
                txtNamePlatePhotoPath.Text = objfield.sNamePlatePhotoPath;

                //string sDetails = objfield.GetReworkdetails(txtEnumDetailsId.Text);
                //if (sDetails.Length > 0)
                //{
                //    string sRework = sDetails.Split('~').GetValue(0).ToString();
                //    string sRemarks = sDetails.Split('~').GetValue(1).ToString();
                //    cmbRework.SelectedValue = sRework;
                //}

                ShowUploadedImages();
                cmdSave.Text = "Update";

                if (cmbMake.SelectedIndex > 0)
                {
                    cmdSave.Text = "Modify and Approve";
                }

                RestrictUpdatebasedonHeirarchy(objfield.sPriorityLevel, objSession.UserType);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to reset the fields
        /// </summary>
        public void Reset()
        {
            try
            {

                txtTcCode.Text = string.Empty;
                txtTcslno.Text = string.Empty;
                cmbCapacity.SelectedIndex = 0;
                cmbMake.SelectedIndex = 0;
                txtManufactureDate.Text = string.Empty;

                txtDTCName.Text = string.Empty;
                txtDTCCode.Text = string.Empty;
                txtoldDTCCode.Text = string.Empty;
                txtIPDTCCode.Text = string.Empty;

                txtInternalCode.Text = string.Empty;
                txtConnectedHP.Text = string.Empty;
                txtConnectedKW.Text = string.Empty;
                txtKWHReading.Text = string.Empty;
                txtCommisionDate.Text = string.Empty;
                txtDtrCommissionDate.Text = string.Empty;
                txtServiceDate.Text = string.Empty;
                cmbPlatformType.SelectedIndex = 0;
                cmbBreakerType.SelectedIndex = 0;
                cmbDTCMetered.SelectedIndex = 0;
                cmbHTProtection.SelectedIndex = 0;
                cmbLTProtection.SelectedIndex = 0;
                cmbGrounding.SelectedIndex = 0;
                cmbLightArrester.SelectedIndex = 0;
                cmbLoadtype.SelectedIndex = 0;
                cmbProjecttype.SelectedIndex = 0;
                txtltLine.Text = string.Empty;
                txtDepreciation.Text = string.Empty;
                txtLatitude.Text = string.Empty;
                txtLongitude.Text = string.Empty;

                txtEnumDetailsId.Text = string.Empty;

                txtTankCapacity.Text = string.Empty;
                txtInfosysAsset.Text = string.Empty;
                cmbRating.SelectedIndex = 0;
                cmbStarRated.Items.Clear();
                dvStar.Style.Add("display", "none");

                txtTcslno.Enabled = true;

                cmdSave.Text = "Save";

                dvSSPlate.Style.Add("display", "none");
                dvNamePlate.Style.Add("display", "none");
                dvDTLMSPhoto.Style.Add("display", "none");
                dvOldDTCBESCOM.Style.Add("display", "none");
                dvIPEnumPhoto.Style.Add("display", "none");
                dvInfosys.Style.Add("display", "none");
                dvDTCPhoto.Style.Add("display", "none");
                dvDTCPhoto1.Style.Add("display", "none");

                txtSSPlatePath.Text = string.Empty;
                txtNamePlatePhotoPath.Text = string.Empty;
                txtDTLMSDTCPath.Text = string.Empty;
                txtDTCPath.Text = string.Empty;
                txtOLDDTCPath.Text = string.Empty;
                txtIPDTCPath.Text = string.Empty;
                txtInfosysPath.Text = string.Empty;

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to display images based on conditions
        /// </summary>
        public void RetainImageinPostbackSession()
        {
            string[] arrSave_ImageSession_String = { "fupNamePlate", "fupSSPlate", "fupDTLMSCodePhoto", "fupIPEnum", "fupOldCodePhoto", "fupInfosys", "fupDTCPhoto" };
            Session["arrSave_ImageSession_String"] = arrSave_ImageSession_String;
            try
            {
                //Name Plate Photo

                //Case: 1 When the page is submitted for the first time(First PostBack) and there is file 
                // in FileUpload control but session is Null then Store the values to Session Object as:
                if (Session["fupNamePlate"] == null && fupNamePlate.HasFile)
                {
                    Session["fupNamePlate"] = fupNamePlate;
                }
                if (Session["fupSSPlate"] == null && fupSSPlate.HasFile)
                {
                    Session["fupSSPlate"] = fupSSPlate;
                }
                if (Session["fupDTLMSCodePhoto"] == null && fupDTLMSCodePhoto.HasFile)
                {
                    Session["fupDTLMSCodePhoto"] = fupDTLMSCodePhoto;
                }
                if (Session["fupIPEnum"] == null && fupIPEnum.HasFile)
                {
                    Session["fupIPEnum"] = fupIPEnum;
                }
                if (Session["fupOldCodePhoto"] == null && fupOldCodePhoto.HasFile)
                {
                    Session["fupOldCodePhoto"] = fupOldCodePhoto;
                }
                if (Session["fupDTCPhoto"] == null && fupDTCPhoto.HasFile)
                {
                    Session["fupDTCPhoto"] = fupDTCPhoto;
                }

                // Case 2: On Next PostBack Session has value but FileUpload control is
                // Blank due to PostBack then return the values from session to FileUpload as:

                if (Session["fupNamePlate"] != null && (!fupNamePlate.HasFile))
                {
                    fupNamePlate = (FileUpload)Session["fupNamePlate"];
                }
                if (Session["fupSSPlate"] != null && (!fupSSPlate.HasFile))
                {
                    fupSSPlate = (FileUpload)Session["fupSSPlate"];
                }
                if (Session["fupDTLMSCodePhoto"] != null && (!fupDTLMSCodePhoto.HasFile))
                {
                    fupDTLMSCodePhoto = (FileUpload)Session["fupDTLMSCodePhoto"];
                }
                if (Session["fupIPEnum"] != null && (!fupIPEnum.HasFile))
                {
                    fupIPEnum = (FileUpload)Session["fupIPEnum"];
                }
                if (Session["fupOldCodePhoto"] != null && (!fupOldCodePhoto.HasFile))
                {
                    fupOldCodePhoto = (FileUpload)Session["fupOldCodePhoto"];
                }
                if (Session["fupDTCPhoto"] != null && (!fupDTCPhoto.HasFile))
                {
                    fupDTCPhoto = (FileUpload)Session["fupDTCPhoto"];
                }

                // Case 3: When there is value in Session but user want to change the file then
                // In this case we need to change the file in session object also as:
                if (fupNamePlate.HasFile)
                {
                    Session["fupNamePlate"] = fupNamePlate;
                }
                if (fupSSPlate.HasFile)
                {
                    Session["fupSSPlate"] = fupSSPlate;
                }
                if (fupDTLMSCodePhoto.HasFile)
                {
                    Session["fupDTLMSCodePhoto"] = fupDTLMSCodePhoto;
                }
                if (fupIPEnum.HasFile)
                {
                    Session["fupIPEnum"] = fupIPEnum;
                }
                if (fupOldCodePhoto.HasFile)
                {
                    Session["fupOldCodePhoto"] = fupOldCodePhoto;
                }
                if (fupDTCPhoto.HasFile)
                {
                    Session["fupDTCPhoto"] = fupDTCPhoto;
                }

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to save enumeration details
        /// </summary>
        public void SaveEnumerationDetails()
        {

            try
            {
                clsFieldEnumeration objFieldEnum = new clsFieldEnumeration();
                string[] Arr = new string[3];

                objFieldEnum.sEnumDetailsID = txtEnumDetailsId.Text;
                objFieldEnum.stempdtccode = txtDTCCode.Text;
                objFieldEnum.sOfficeCode = cmbSection.SelectedValue;
                objFieldEnum.sFeederCode = cmbFeeder.SelectedValue;
                objFieldEnum.sWeldDate = txtwelddate.Text;
                objFieldEnum.staggedDTR = cmbTaggedDTR.SelectedValue;
                objFieldEnum.locationtype = cmdloctype.SelectedValue;

                //TC Details
                objFieldEnum.sTcCode = txtTcCode.Text;

                if (cmbCapacity.SelectedIndex > 0)
                {
                    objFieldEnum.sTCCapacity = cmbCapacity.SelectedValue;
                }
                else
                {
                    objFieldEnum.sTCCapacity = "0";
                }
                if (cmbMake.SelectedIndex > 0)
                {
                    objFieldEnum.sTCMake = cmbMake.SelectedValue;
                }
                else
                {
                    objFieldEnum.sTCMake = "0";
                }
                objFieldEnum.sTCSlno = txtTcslno.Text;
                objFieldEnum.sTCManfDate = txtManufactureDate.Text;
                objFieldEnum.sMakeName = cmbMake.SelectedItem.ToString();

                // DTC Details
                objFieldEnum.sDTCName = txtDTCName.Text;
                objFieldEnum.sDTCCode = txtDTCCode.Text;
                objFieldEnum.sOldDTCCode = txtoldDTCCode.Text;
                objFieldEnum.sIPDTCCode = txtIPDTCCode.Text;
                //objFieldEnum.sEnumDate = null;
                objFieldEnum.sEnumDate = txtwelddate.Text;
                objFieldEnum.sInfosysAsset = txtInfosysAsset.Text;

                // DTC Other Details
                objFieldEnum.sInternalCode = txtInternalCode.Text;
                objFieldEnum.sConnectedKW = txtConnectedKW.Text;
                objFieldEnum.sConnectedHP = txtConnectedHP.Text;
                objFieldEnum.sKWHReading = txtKWHReading.Text;
                objFieldEnum.sCommisionDate = txtCommisionDate.Text;
                objFieldEnum.sDtrcommissiondate = txtDtrCommissionDate.Text;
                objFieldEnum.sLastServiceDate = txtServiceDate.Text;

                if (objFieldEnum.sConnectedKW == "")
                {
                    objFieldEnum.sConnectedKW = "0";
                }
                if (objFieldEnum.sConnectedHP == "")
                {
                    objFieldEnum.sConnectedHP = "0";
                }
                if (objFieldEnum.sKWHReading == "")
                {
                    objFieldEnum.sKWHReading = "0";
                }
                if (cmbPlatformType.SelectedIndex > 0)
                {
                    objFieldEnum.sPlatformType = cmbPlatformType.SelectedValue;
                }
                else
                {
                    objFieldEnum.sPlatformType = "0";
                }
                if (cmbBreakerType.SelectedIndex > 0)
                {
                    objFieldEnum.sBreakertype = cmbBreakerType.SelectedValue;
                }
                else
                {
                    objFieldEnum.sBreakertype = "0";
                }
                if (cmbDTCMetered.SelectedIndex > 0)
                {
                    objFieldEnum.sDTCMeters = cmbDTCMetered.SelectedValue;
                }
                else
                {
                    objFieldEnum.sDTCMeters = "0";
                }
                if (cmbHTProtection.SelectedIndex > 0)
                {
                    objFieldEnum.sHTProtect = cmbHTProtection.SelectedValue;
                }
                else
                {
                    objFieldEnum.sHTProtect = "0";
                }
                if (cmbLTProtection.SelectedIndex > 0)
                {
                    objFieldEnum.sLTProtect = cmbLTProtection.SelectedValue;
                }
                else
                {
                    objFieldEnum.sLTProtect = "0";
                }
                if (cmbGrounding.SelectedIndex > 0)
                {
                    objFieldEnum.sGrounding = cmbGrounding.SelectedValue;
                }
                else
                {
                    objFieldEnum.sGrounding = "0";
                }
                if (cmbLightArrester.SelectedIndex > 0)
                {
                    objFieldEnum.sArresters = cmbLightArrester.SelectedValue;
                }
                else
                {
                    objFieldEnum.sArresters = "0";
                }
                if (cmbLoadtype.SelectedIndex > 0)
                {
                    objFieldEnum.sLoadtype = cmbLoadtype.SelectedValue;
                }
                else
                {
                    objFieldEnum.sLoadtype = "0";
                }
                if (cmbProjecttype.SelectedIndex > 0)
                {
                    objFieldEnum.sProjecttype = cmbProjecttype.SelectedValue;
                }
                else
                {
                    objFieldEnum.sProjecttype = "0";
                }
                objFieldEnum.sLTlinelength = txtltLine.Text;
                objFieldEnum.sHTlinelength = txtHtLine.Text;
                objFieldEnum.sDepreciation = txtDepreciation.Text;
                objFieldEnum.sLatitude = txtLatitude.Text;
                objFieldEnum.sLongitude = txtLongitude.Text;

                if (rdbDTLMS.Checked == true)
                {
                    objFieldEnum.bIsDTLMSDetails = true;
                }
                //if (rdbIPEnum.Checked == true)
                //{
                //    objFieldEnum.bIsIPDetails = true;
                //}
                //if (rdbOldDtc.Checked == true)
                //{
                //    objFieldEnum.bIsCESCDetails = true;
                //}

                objFieldEnum.sCrBy = objSession.UserId;
                objFieldEnum.sStatus = txtStatus.Text;

                objFieldEnum.sTankCapacity = txtTankCapacity.Text;
                objFieldEnum.sTCWeight = "";

                if (cmbRating.SelectedIndex > 0)
                {
                    objFieldEnum.sRating = cmbRating.SelectedValue;
                }
                else
                {
                    objFieldEnum.sRating = "0";
                }
                if (cmbStarRated.SelectedIndex > 0)
                {
                    objFieldEnum.sStarRate = cmbStarRated.SelectedValue;
                }
                else
                {
                    objFieldEnum.sStarRate = "0";
                }

                objFieldEnum.sUserType = objSession.UserType;
                objFieldEnum.sRemark = null;
                objFieldEnum.sEnumType = "2";
                string stempstatus = objFieldEnum.Gettempstatus(objFieldEnum);
                objFieldEnum.stempstatus = stempstatus;


                // checking make and serial No in main table.
                string[] Array = new string[2];
                Array = objFieldEnum.CheckMakeSerialNoOfTC(cmbMake.SelectedValue, txtTcslno.Text, txtTcCode.Text);
                if (Array[1].ToString() == "2")
                {
                    ShowMsgBox(Array[0].ToString());
                    return;
                }
                Arr = objFieldEnum.SaveFieldEnumerationDetails(objFieldEnum);

                objFieldEnum.UpdateApproverpriority(objSession.UserType, objFieldEnum.sEnumDetailsID, objSession.UserId, objFieldEnum.sRemark, cmbRework.SelectedValue);

                if (Arr[1].ToString() == "0")
                {

                    bool bResult = SaveImagesPath(objFieldEnum);
                    txtEnumDetailsId.Text = objFieldEnum.sEnumDetailsID;
                    cmdSave.Text = "Update";
                    ShowMsgBox(Arr[0].ToString());
                    if (bResult == true)
                    {
                        ShowMsgBox(Arr[0].ToString());
                    }
                    else
                    {
                        ShowMsgBox("Error Occured While Uploading Image");
                    }
                    return;
                }
                if (Arr[1].ToString() == "1")
                {

                    bool bResult = SaveImagesPath(objFieldEnum);
                    if (bResult == true)
                    {
                        ShowMsgBox(Arr[0]);
                    }
                    else
                    {
                        ShowMsgBox("Error Occured While Uploading Image");
                    }
                    return;
                }
                if (Arr[1].ToString() == "2")
                {
                    ShowMsgBox(Arr[0]);
                    return;
                }

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }
        /// <summary>
        /// method to validate fields before save
        /// </summary>
        /// <returns></returns>
        bool ValidateForm()
        {
            bool bValidate = false;
            if (cmbCircle.SelectedIndex == 0)
            {
                cmbCircle.Focus();
                ShowMsgBox("Please Select the Circle");
                return false;
            }
            if (cmbDivision.SelectedIndex == 0)
            {
                cmbDivision.Focus();
                ShowMsgBox("Please Select the Division");
                return false;
            }
            if (cmbsubdivision.SelectedIndex == 0)
            {
                cmbsubdivision.Focus();
                ShowMsgBox("Please Select the Subdivision");
                return false;
            }
            if (cmbSection.SelectedIndex == 0)
            {
                cmbSection.Focus();
                ShowMsgBox("Please Select the Section");
                return false;
            }
            if (cmbFeeder.SelectedIndex == 0)
            {
                cmbFeeder.Focus();
                ShowMsgBox("Please Select the Feeder");
                return false;
            }
            if (cmbCapacity.SelectedIndex == 0)
            {
                cmbCapacity.Focus();
                ShowMsgBox("Please Enter the Capacity");
                return false;
            }
            if (txtwelddate.Text.Trim().Length == 0)
            {
                txtwelddate.Focus();
                ShowMsgBox("Enter Date of Fixing");
                return false;
            }
            if (txtTcslno.Text.Trim().Length == 0)
            {
                txtTcslno.Focus();
                ShowMsgBox("Enter DTR Slno");
                return false;
            }
            string sResult = Genaral.DateValidation(txtwelddate.Text);
            if (sResult != "")
            {
                ShowMsgBox(sResult);
                return bValidate;
            }
            sResult = Genaral.DateComparision(txtwelddate.Text, "", true, false);
            if (sResult == "1")
            {
                ShowMsgBox("Date of Fixing should be Less than Current Date");
                return bValidate;
            }

            if (cmdloctype.SelectedIndex == 0)
            {
                cmbCircle.Focus();
                ShowMsgBox("Please Select the Location Type");
                return false;
            }

            if (cmbMake.SelectedValue != "1")
            {
                if (txtManufactureDate.Text.Trim().Length == 0)
                {
                    txtwelddate.Focus();
                    ShowMsgBox("Enter Manufacture Date");
                    return false;
                }
            }
            if (hdfDTCWithoutDTrFlag.Value != "1")
            {
                if (txtManufactureDate.Text.Length > 0 && (txtManufactureDate.Text.Length < 10))
                {
                    ShowMsgBox("Enter Proper Manufacturer date");
                    return bValidate;
                }

                if (txtTcCode.Text.Trim().Length == 0)
                {
                    txtTcCode.Focus();
                    ShowMsgBox("Enter DTR Code");
                    return false;
                }

                if (cmbMake.SelectedIndex == 0)
                {
                    cmbMake.Focus();
                    ShowMsgBox("Please Select the DTr Make");
                    return false;
                }

                if (cmbRating.SelectedIndex == 0)
                {
                    cmbRating.Focus();
                    ShowMsgBox("Please Select the Rating");
                    return false;
                }
            }
            if (txtConnectedKW.Text.Length == 0)
            {
                txtConnectedKW.Focus();
                ShowMsgBox("Please Enter the Connected KW");
                return false;
            }

            if (txtConnectedKW.Text.Length > 0)
            {
                //if (Convert.ToDouble(txtConnectedKW.Text) == 0)
                //{
                //    txtConnectedKW.Text = "";
                //    txtConnectedKW.Focus();
                //    ShowMsgBox("Please Enter Valid Connected Kw Length (Eg: 111.11)");
                //    return false;
                //}

                if (txtConnectedKW.Text.Contains("."))
                {
                    string decimalString = txtConnectedKW.Text.ToString();
                    int index = decimalString.IndexOf('.');

                    if (index > 3)
                    {
                        //txtConnectedKW.Text = "";
                        txtConnectedKW.Focus();
                        ShowMsgBox("Please Enter Valid Connected KW Length (Eg: 111.11)");
                        return false;
                    }

                    if ((decimalString.Length - (index + 1)) != 2 && (decimalString.Length - (index + 1)) != 1)
                    {
                        //txtConnectedKW.Text = "";
                        txtConnectedKW.Focus();
                        ShowMsgBox("Please Enter Valid Connected KW Length (Eg: 111.11)");
                        return false;
                    }
                }
                else
                {
                    if (txtConnectedKW.Text.Length > 3)
                    {
                        //txtConnectedKW.Text = "";
                        txtConnectedKW.Focus();
                        ShowMsgBox("Please Enter Valid Connected KW Length (Eg: 111.11)");
                        return false;
                    }
                }
            }

            if (txtConnectedHP.Text.Length == 0)
            {
                txtConnectedHP.Focus();
                ShowMsgBox("Please Enter the Connected HP");
                return false;
            }

            if (txtConnectedHP.Text.Length > 0)
            {
                //if (Convert.ToDouble(txtConnectedHP.Text) == 0)
                //{
                //    txtConnectedHP.Text = "";
                //    txtConnectedHP.Focus();
                //    ShowMsgBox("Please Enter Valid Connected HP Length (Eg: 111.11)");
                //    return false;
                //}

                if (txtConnectedHP.Text.Contains("."))
                {
                    string decimalString = txtConnectedHP.Text.ToString();
                    int index = decimalString.IndexOf('.');

                    if (index > 3)
                    {
                        //txtConnectedHP.Text = "";
                        txtConnectedHP.Focus();
                        ShowMsgBox("Please Enter Valid Connected HP Length (Eg: 111.11)");
                        return false;
                    }

                    if ((decimalString.Length - (index + 1)) != 2 && (decimalString.Length - (index + 1)) != 1)
                    {
                        //txtConnectedHP.Text = "";
                        txtConnectedHP.Focus();
                        ShowMsgBox("Please Enter Valid Connected HP Length (Eg: 111.11)");
                        return false;
                    }
                }
                else
                {
                    if (txtConnectedHP.Text.Length > 3)
                    {
                        //txtConnectedHP.Text = "";
                        txtConnectedHP.Focus();
                        ShowMsgBox("Please Enter Valid Connected HP Length (Eg: 111.11)");
                        return false;
                    }
                }
            }

            if (txtCommisionDate.Text.Trim().Length == 0)
            {
                txtCommisionDate.Focus();
                ShowMsgBox("Please Select DTC Commision Date");
                return false;
            }

            if (txtDtrCommissionDate.Text.Trim().Length == 0)
            {
                txtDtrCommissionDate.Focus();
                ShowMsgBox("Please Select DTr Commision Date");
                return false;
            }

            sResult = Genaral.DateComparision(txtDtrCommissionDate.Text, txtCommisionDate.Text, false, false);

            if (sResult == "2")
            {
                ShowMsgBox("DTr Commission Date should be Greater than or equal to DTC Commission Date");
                return false;
            }

            // for manufacture date validation
            string sResult1 = Genaral.DateComparision(txtDtrCommissionDate.Text, txtManufactureDate.Text, false, false);

            if (sResult1 == "2")
            {
                ShowMsgBox("DTr Commission Date should be Greater than or equal to Manufacture Date");
                return false;
            }

            if (cmbPlatformType.SelectedIndex == 0)
            {
                cmbPlatformType.Focus();
                ShowMsgBox("Please Select the Platform Type");
                return false;
            }

            if (cmbBreakerType.SelectedIndex == 0)
            {
                cmbBreakerType.Focus();
                ShowMsgBox("Please Select the Breaker Type");
                return false;
            }

            if (cmbDTCMetered.SelectedIndex == 0)
            {
                cmbDTCMetered.Focus();
                ShowMsgBox("Please Select the DTC Meter");
                return false;
            }

            if (cmbDTCMetered.SelectedValue == "1" && txtKWHReading.Text.Length == 0)
            {
                txtKWHReading.Focus();
                ShowMsgBox("Please Enter the KWH Reading");
                return false;
            }
            if (txtKWHReading.Text.Trim() == ".")
            {
                txtKWHReading.Focus();
                ShowMsgBox("Enter vaild KWH Reading");
                return false;
            }
            if (cmbHTProtection.SelectedIndex == 0)
            {
                cmbHTProtection.Focus();
                ShowMsgBox("Please Select the HT Protection");
                return false;
            }
            if (cmbLTProtection.SelectedIndex == 0)
            {
                cmbLTProtection.Focus();
                ShowMsgBox("Please Select the LT Protection");
                return false;
            }

            if (cmbGrounding.SelectedIndex == 0)
            {
                cmbGrounding.Focus();
                ShowMsgBox("Please Select the Grounding");
                return false;
            }

            if (cmbLightArrester.SelectedIndex == 0)
            {
                cmbLightArrester.Focus();
                ShowMsgBox("Please Select the Lightning Arresters");
                return false;
            }
            if (cmbLoadtype.SelectedIndex == 0)
            {
                cmbLoadtype.Focus();
                ShowMsgBox("Please Select the Load Type");
                return false;
            }
            if (cmbProjecttype.SelectedIndex == 0)
            {
                cmbProjecttype.Focus();
                ShowMsgBox("Please Select the Project Type");
                return false;
            }

            if (txtltLine.Text != "")
            {
                string Val = txtltLine.Text.Trim();
                string afterDotVal = Val.Contains('.') ? Val.Split('.')[1] : Val;
                bool ReturnValue = false;
                if (Val.Length > 3 && Val.Length <= 7 && !Val.Contains('.'))
                {
                    txtltLine.Focus();
                    ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                    return bValidate;
                }
                else if (Val.IndexOf('.') == 0 || afterDotVal.Length == 0)
                {
                    txtltLine.Focus();
                    ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                    return bValidate;
                }
                else if (!(Val.IndexOf('.') <= 3))
                {
                    txtltLine.Focus();
                    ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                    return bValidate;
                }
                else if (Val.Contains('.') && afterDotVal.Length > 3)
                {
                    txtltLine.Focus();
                    ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                    return bValidate;
                }
                else if (Val.Length > 0 && Val.Length <= 3 && !(Val.Contains('.')))
                {
                    switch (Val)
                    {
                        case "00":
                            ReturnValue = true;
                            break;
                        case "000":
                            ReturnValue = true;
                            break;
                    }
                    if (ReturnValue)
                    {
                        txtltLine.Focus();
                        ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                        return bValidate;
                    }
                }
                else if (Val.Length > 0 && Val.Contains('.') && Val.Length <= 7)
                {
                    switch (Val)
                    {
                        case "0.":
                            ReturnValue = true;
                            break;
                        case "00.":
                            ReturnValue = true;
                            break;
                        case "000.":
                            ReturnValue = true;
                            break;
                        case ".":
                            ReturnValue = true;
                            break;
                        case ".0":
                            ReturnValue = true;
                            break;
                        case ".00":
                            ReturnValue = true;
                            break;
                        case ".000":
                            ReturnValue = true;
                            break;
                        case "0.00":
                            ReturnValue = true;
                            break;
                        case "0.000":
                            ReturnValue = true;
                            break;
                        case "00.0":
                            ReturnValue = true;
                            break;
                        case "00.00":
                            ReturnValue = true;
                            break;
                        case "00.000":
                            ReturnValue = true;
                            break;
                        case "000.0":
                            ReturnValue = true;
                            break;
                        case "000.00":
                            ReturnValue = true;
                            break;
                        case "000.000":
                            ReturnValue = true;
                            break;
                    }
                    if (ReturnValue)
                    {
                        txtltLine.Focus();
                        ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                        return bValidate;
                    }
                }

                //if (txtltLine.Text.Trim().Length != 0)
                //{
                //    string LTLine = txtltLine.Text == "." ? "0" : txtltLine.Text;
                //    double val = Convert.ToDouble(LTLine);
                //    if (val < 0.0 || val > 999.999)
                //    {
                //        txtltLine.Focus();
                //        ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                //        return bValidate;
                //    }
                //    if (txtltLine.Text == ".")
                //    {
                //        ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                //        return bValidate;
                //    }
                //}

                //if (txtltLine.Text != "0")
                //{
                //    if (txtltLine.Text.Contains('.'))
                //    {
                //        int ltindex = txtltLine.Text.IndexOf('.');
                //        string ltstr = txtltLine.Text.Substring(ltindex + 1);
                //        int val = Convert.ToInt16(ltstr);
                //        if (val == 0)
                //        {
                //            string lltstr = txtltLine.Text.Substring(0, ltindex);

                //            if (Convert.ToInt16(lltstr) == 0)
                //            {
                //                ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                //                return bValidate;
                //            }

                //        }

                //        string lltstr1 = txtltLine.Text.Substring(0, ltindex);
                //        if (lltstr1.Length > 3)
                //        {
                //            ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                //            return bValidate;
                //        }
                //    }
                //}

                //if (txtltLine.Text.Contains('.'))
                //{
                //    string LTPattern = @"[0-9]{0,3}^\d+\.\d{0,3}$";
                //    System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(LTPattern);
                //    if (!r.IsMatch(txtltLine.Text))
                //    {
                //        ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                //        return bValidate;
                //    }
                //}

            }

            if (txtHtLine.Text != "")
            {
                string Val = txtHtLine.Text.Trim();
                string afterDotVal = Val.Contains('.') ? Val.Split('.')[1] : Val;
                bool ReturnValue = false;
                if (Val.Length > 3 && Val.Length <= 7 && !Val.Contains('.'))
                {
                    txtHtLine.Focus();
                    ShowMsgBox("Please Enter a valid HT line Length (Eg: 111.111)");
                    return bValidate;
                }
                else if (!(Val.IndexOf('.') <= 3))
                {
                    txtHtLine.Focus();
                    ShowMsgBox("Please Enter a valid HT line Length (Eg: 111.111)");
                    return bValidate;
                }
                else if (Val.IndexOf('.') == 0 || afterDotVal.Length == 0)
                {
                    txtHtLine.Focus();
                    ShowMsgBox("Please Enter a valid HT line Length (Eg: 111.111)");
                    return bValidate;
                }
                else if (Val.Contains('.') && afterDotVal.Length > 3)
                {
                    txtHtLine.Focus();
                    ShowMsgBox("Please Enter a valid HT line Length (Eg: 111.111)");
                    return bValidate;
                }
                else if (Val.Length > 0 && Val.Length <= 3 && !(Val.Contains('.')))
                {
                    switch (Val)
                    {
                        case "00":
                            ReturnValue = true;
                            break;
                        case "000":
                            ReturnValue = true;
                            break;
                    }
                    if (ReturnValue)
                    {
                        txtHtLine.Focus();
                        ShowMsgBox("Please Enter a valid HT line Length (Eg: 111.111)");
                        return bValidate;
                    }
                }
                else if (Val.Length > 0 && Val.Contains('.') && Val.Length <= 7)
                {
                    switch (Val)
                    {
                        case "0.":
                            ReturnValue = true;
                            break;
                        case "00.":
                            ReturnValue = true;
                            break;
                        case "000.":
                            ReturnValue = true;
                            break;
                        case ".":
                            ReturnValue = true;
                            break;
                        case ".0":
                            ReturnValue = true;
                            break;
                        case ".00":
                            ReturnValue = true;
                            break;
                        case ".000":
                            ReturnValue = true;
                            break;
                        case "0.00":
                            ReturnValue = true;
                            break;
                        case "0.000":
                            ReturnValue = true;
                            break;
                        case "00.0":
                            ReturnValue = true;
                            break;
                        case "00.00":
                            ReturnValue = true;
                            break;
                        case "00.000":
                            ReturnValue = true;
                            break;
                        case "000.0":
                            ReturnValue = true;
                            break;
                        case "000.00":
                            ReturnValue = true;
                            break;
                        case "000.000":
                            ReturnValue = true;
                            break;
                    }
                    if (ReturnValue)
                    {
                        txtHtLine.Focus();
                        ShowMsgBox("Please Enter a valid HT line Length (Eg: 111.111)");
                        return bValidate;
                    }
                }

                //if (txtHtLine.Text.Trim().Length != 0)
                //{
                //    double val = Convert.ToDouble(txtHtLine.Text);
                //    if (val < 0.0 || val > 999.999)
                //    {
                //        txtHtLine.Focus();
                //        ShowMsgBox("Please Enter a valid HT line Length (Eg: 111.111)");
                //        return bValidate;
                //    }
                //    if (txtHtLine.Text == ".")
                //    {
                //        ShowMsgBox("Please Enter a valid HT line Length (Eg: 111.111)");
                //        return bValidate;
                //    }
                //}

                //if (txtHtLine.Text != "0")
                //{
                //    if (txtltLine.Text.Contains('.'))
                //    {
                //        int ltindex = txtHtLine.Text.IndexOf('.');
                //        string ltstr = txtHtLine.Text.Substring(ltindex + 1);
                //        int val = Convert.ToInt16(ltstr);
                //        if (val == 0)
                //        {
                //            string lltstr = txtHtLine.Text.Substring(0, ltindex);

                //            if (Convert.ToInt16(lltstr) == 0)
                //            {
                //                ShowMsgBox("Please Enter a valid HT line Length (Eg: 111.111)");
                //                return bValidate;
                //            }
                //        }
                //        string lltstr1 = txtHtLine.Text.Substring(0, ltindex);
                //        if (lltstr1.Length > 3)
                //        {
                //            ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                //            return bValidate;
                //        }
                //    }
                //}
                //if (txtHtLine.Text.Contains('.'))
                //{
                //    string HTPattern = @"[0-9]{0,3}^\d+\.\d{0,3}$";
                //    System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(HTPattern);
                //    if (!r.IsMatch(txtHtLine.Text))
                //    {
                //        ShowMsgBox("Please Enter a valid HT line Length (Eg: 111.111)");
                //        return bValidate;
                //    }
                //}


            }
            if (txtManufactureDate.Text.Trim() != "")
            {
                string DatePattern = @"^[a-zA-Z0-9 !@#$%^&*)(]{1,20}$";
                System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(DatePattern);
                if (r.IsMatch(txtManufactureDate.Text))
                {
                    ShowMsgBox("Please Enter a valid Maufacture date in the format (dd/mm/yyyy)");
                    return bValidate;
                }
            }
            if (txtEnumDetailsId.Text == "")
            {
                if (fupNamePlate.PostedFile.ContentLength == 0 && txtNamePlatePhotoPath.Text.Trim() == "")
                {
                    fupNamePlate.Focus();
                    ShowMsgBox("Select Name Plate Image to Upload");
                    return false;
                }

                if (fupSSPlate.PostedFile.ContentLength == 0 && txtSSPlatePath.Text.Trim() == "")
                {
                    fupSSPlate.Focus();
                    ShowMsgBox("Select DTr Code Image to Upload");
                    return false;
                }
            }

            if (cmbRating.SelectedValue == "1")
            {
                if (cmbStarRated.SelectedIndex == 0)
                {
                    cmbStarRated.Focus();
                    ShowMsgBox("Select Star Rating");
                    return false;
                }
            }


            if (txtDTCCode.Text.Trim().Length == 0)
            {
                txtDTCCode.Focus();
                ShowMsgBox("Enter 9 digit DTC Code");
                return false;
            }
            if (txtDTCCode.Text.Trim().Length < 6)
            {
                txtDTCCode.Focus();
                ShowMsgBox("Enter 6 digit DTC Code");
                return false;
            }

            if (txtDTCName.Text.Trim().Length == 0)
            {
                txtDTCName.Focus();
                ShowMsgBox("Enter DTC Name");
                DTCDetails.Attributes.Add("class", "tab-pane fade in active");
                TCDetails.Attributes.Add("class", "tab-pane fade");
                liDTCDetails.Attributes.Add("class", "active");
                liTCDetails.Attributes.Add("class", "");

                return false;
            }
            if (txtEnumDetailsId.Text == "")
            {
                if (fupDTLMSCodePhoto.PostedFile.ContentLength == 0 && txtDTLMSDTCPath.Text.Trim() == "")
                {
                    fupDTLMSCodePhoto.Focus();
                    ShowMsgBox("Select DTLMS Code Photo to Upload");

                    DTCDetails.Attributes.Add("class", "tab-pane fade in active");
                    TCDetails.Attributes.Add("class", "tab-pane fade");
                    liDTCDetails.Attributes.Add("class", "active");
                    liTCDetails.Attributes.Add("class", "");
                    return false;
                }
                if (fupDTCPhoto.PostedFile.ContentLength == 0 && txtDTCPath.Text.Trim() == "")
                {
                    fupDTCPhoto.Focus();
                    ShowMsgBox("Select DTC Photo to Upload");

                    DTCDetails.Attributes.Add("class", "tab-pane fade in active");
                    TCDetails.Attributes.Add("class", "tab-pane fade");
                    liDTCDetails.Attributes.Add("class", "active");
                    liTCDetails.Attributes.Add("class", "");
                    return false;
                }
            }

            string sValidateResult = ValidateImages();
            if (sValidateResult != "")
            {
                ShowMsgBox(sValidateResult);
                return false;
            }
            bValidate = true;
            return bValidate;
        }
        /// <summary>
        /// to display popup aleart message
        /// </summary>
        /// <param name="sMsg"></param>
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
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowMsgBox");
            }
        }
        /// <summary>
        /// method to excecute based on command names
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdFieldEnumDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                //if (e.CommandName == "remove")
                //{
                //    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                //    Label lblEnumDetailsId = (Label)row.FindControl("lblEnumId");

                //    clsFieldEnumeration objFieldEnum = new clsFieldEnumeration();
                //    objFieldEnum.sEnumDetailsID = lblEnumDetailsId.Text;
                //    bool bResult = objFieldEnum.DeleteEnumerationDetails(objFieldEnum);
                //    if (bResult == true)
                //    {
                //        ShowMsgBox("Removed Successfully");
                //        //LoadFieldEnumeration();
                //        return;
                //    }

                //}
                #region edit

                if (e.CommandName == "Modify")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    int iRowIndex = row.RowIndex;

                    Label lblOffcode = (Label)row.FindControl("lblOffcode");
                    Label lblfeedercode = (Label)row.FindControl("lblfeedercode");
                    Label lblDTCName = (Label)row.FindControl("lblDTCName");
                    Label lblWelddate = (Label)row.FindControl("lblWelddate");
                    Label lbloper1 = (Label)row.FindControl("lbloper1");
                    Label lbloper2 = (Label)row.FindControl("lbloper2");
                    Label lblTcCode = (Label)row.FindControl("lblTcCode");
                    Label lblmake = (Label)row.FindControl("lblmake");
                    Label lblTcslno = (Label)row.FindControl("lblTcslno");
                    Label lblManfDate = (Label)row.FindControl("lblManfDate");
                    Label lblCapacity = (Label)row.FindControl("lblCapacity");
                    Label lblPhotopath = (Label)row.FindControl("lblPhotopath");
                    Label lblDTCCode = (Label)row.FindControl("lblDTCCode");
                    Label lblCescDTCCode = (Label)row.FindControl("lblCescDTCCode");
                    Label lblIpDTCCode = (Label)row.FindControl("lblIpDTCCode");
                    Label lblEnumerationDate = (Label)row.FindControl("lblEnumerationDate");
                    Label lblOldPhoto = (Label)row.FindControl("lblOldPhoto");
                    Label lblNewPhoto = (Label)row.FindControl("lblNewPhoto");


                    cmbsubdivision.SelectedValue = lblOffcode.Text.Substring(1, 3);
                    cmbSection.SelectedValue = lblOffcode.Text;
                    cmbFeeder.SelectedValue = lblfeedercode.Text;
                    txtDTCName.Text = lblDTCName.Text;
                    txtwelddate.Text = lblWelddate.Text;
                    txtTcCode.Text = lblTcCode.Text;
                    txtTcslno.Text = lblTcslno.Text;
                    txtManufactureDate.Text = lblManfDate.Text;
                    //txtplatephoto.Text = lblPhotopath.Text;
                    txtDTCCode.Text = lblDTCCode.Text;
                    txtoldDTCCode.Text = lblCescDTCCode.Text;
                    txtIPDTCCode.Text = lblIpDTCCode.Text;
                    // txtEnumerationdate.Text = lblEnumerationDate.Text;
                    //txtoldCoding.Text = lblOldPhoto.Text;
                    // txtAfterCoding.Text = lblNewPhoto.Text;
                    cmbCapacity.SelectedValue = lblCapacity.Text;
                    cmbMake.SelectedValue = lblmake.Text;
                    //cmboperator1.SelectedValue = lbloper1.Text;
                    //cmboperator2.SelectedValue = lbloper2.Text;

                    DataTable dt = (DataTable)ViewState["Enum"];
                    dt.Rows[iRowIndex].Delete();
                    if (dt.Rows.Count == 0)
                    {
                        ViewState["Enum"] = null;
                    }
                    else
                    {
                        ViewState["Enum"] = dt;
                    }

                    grdFieldEnumDetails.DataSource = dt;
                    grdFieldEnumDetails.DataBind();
                }

                #endregion
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// method for page indexing to grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdFieldEnumDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdFieldEnumDetails.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Enum"];
                grdFieldEnumDetails.DataSource = dt;
                grdFieldEnumDetails.DataBind();

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to save image path through sftp path
        /// </summary>
        /// <param name="objFieldEnum"></param>
        /// <returns></returns>
        public bool SaveImagesPath(clsFieldEnumeration objFieldEnum)
        {
            try
            {

                //FTP Parameter
                string sFTPLink = string.Empty;
                string sFTPUserName = string.Empty;
                string sFTPPassword = string.Empty;

                // File Name Parameter
                string sDirectory = string.Empty;
                string sPlateFileName = string.Empty;
                string sSSPlateFileName = string.Empty;
                string sOldCodeFileName = string.Empty;
                string sIPEnumCodeFileName = string.Empty;
                string sDTLMSCodeFileName = string.Empty;
                string sDTCFileName = string.Empty;
                string sInfosysCodeFileName = string.Empty;

                // File Path Parameter
                string sSavePlateFilePath = string.Empty;
                string sSaveSSPlateFilePath = string.Empty;
                string sSaveOldCodeFilePath = string.Empty;
                string sSaveIPEnumCodeFilePath = string.Empty;
                string sSaveDTLMSCodeFilePath = string.Empty;
                string sSaveDTCFilePath = string.Empty;
                string sSaveInfosysCodeFilePath = string.Empty;

                //FileType Parameter
                string sPlatePhotoExtension = string.Empty;
                string sSSPlatePhotoExtension = string.Empty;
                string sOldCodePhotoExtension = string.Empty;
                string sIPEnumCodePhotoExtension = string.Empty;
                string sDTLMSCodePhotoExtension = string.Empty;
                string sDTCPhotoExtension = string.Empty;
                string sInfosysCodePhotoExtension = string.Empty;

                //  Photo Save DTLMSDocs
                string sFileExt = Convert.ToString(ConfigurationSettings.AppSettings["FileFormat"]);

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

                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder1"]);
                clsSFTP objFtp = new clsSFTP(SFTPPath, sFTPUserName, sFTPPassword);
                bool Isuploaded;

                string sNamePlateFolderName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["NamePlateFolder"].ToUpper());
                string sSSPlateFolderName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SSPlateFolder"].ToUpper());
                string sOldCodeFolderName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["OldCodeFolder"].ToUpper());
                string sIPEnumFolderName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["IPEnumCodeFolder"].ToUpper());
                string sDTLMSFolderName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["DTLMSCodeFolder"].ToUpper());
                string sDTCFolderName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["DTCPhoto"].ToUpper());
                string sInfosysFolderName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["InfosysCodeFolder"].ToUpper());

                // Create Directory

                bool IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + objFieldEnum.sEnumDetailsID + "/");
                if (IsExists == false)
                {
                    objFtp.createDirectory(SFTPmainfolder + objFieldEnum.sEnumDetailsID + "/");
                }

                // Name Plate Photo Save
                if (txtNamePlatePhotoPath.Text.Trim() != "")
                {
                    sPlatePhotoExtension = System.IO.Path.GetExtension(txtNamePlatePhotoPath.Text).ToString().ToLower();
                    sPlatePhotoExtension = ";" + sPlatePhotoExtension.Remove(0, 1) + ";";
                    if (!sFileExt.Contains(sPlatePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }
                    sPlateFileName = Path.GetFileName(txtNamePlatePhotoPath.Text);
                    sDirectory = txtNamePlatePhotoPath.Text;
                    objFieldEnum.sNamePlatePhotoPath = txtNamePlatePhotoPath.Text;
                }
                else if (fupNamePlate.PostedFile.ContentLength != 0)
                {
                    sPlatePhotoExtension = System.IO.Path.GetExtension(fupNamePlate.FileName).ToString().ToLower();
                    sPlatePhotoExtension = ";" + sPlatePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sPlatePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }
                    sPlateFileName = Path.GetFileName(fupNamePlate.PostedFile.FileName);
                    fupNamePlate.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sPlateFileName));
                    sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sPlateFileName);
                }
                if (sPlateFileName != "")
                {
                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + objFieldEnum.sEnumDetailsID + "/" + sNamePlateFolderName + "/");
                        if (IsExists == false)
                        {
                            objFtp.createDirectory(SFTPmainfolder + objFieldEnum.sEnumDetailsID + "/" + sNamePlateFolderName);
                        }

                        Isuploaded = objFtp.upload(SFTPmainfolder + objFieldEnum.sEnumDetailsID + "/" + sNamePlateFolderName + "/", sPlateFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objFieldEnum.sNamePlatePhotoPath = objFieldEnum.sEnumDetailsID + "/" + sNamePlateFolderName + "/" + sPlateFileName;
                            txtNamePlatePhotoPath.Text = objFieldEnum.sNamePlatePhotoPath;
                            Session["fupNamePlate"] = null;
                        }
                    }
                }


                // SS Plate Photo Save
                if (txtSSPlatePath.Text.Trim() != "")
                {
                    sSSPlatePhotoExtension = System.IO.Path.GetExtension(txtSSPlatePath.Text).ToString().ToLower();
                    sSSPlatePhotoExtension = ";" + sSSPlatePhotoExtension.Remove(0, 1) + ";";
                    if (!sFileExt.Contains(sSSPlatePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }
                    sSSPlateFileName = Path.GetFileName(txtSSPlatePath.Text);
                    sDirectory = txtSSPlatePath.Text;
                    objFieldEnum.sSSPlatePhotoPath = txtSSPlatePath.Text;
                }
                else if (fupSSPlate.PostedFile.ContentLength != 0)
                {

                    sSSPlatePhotoExtension = System.IO.Path.GetExtension(fupSSPlate.FileName).ToString().ToLower();
                    sSSPlatePhotoExtension = ";" + sSSPlatePhotoExtension.Remove(0, 1) + ";";
                    if (!sFileExt.Contains(sSSPlatePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }
                    sSSPlateFileName = Path.GetFileName(fupSSPlate.PostedFile.FileName);
                    fupSSPlate.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sSSPlateFileName));
                    sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sSSPlateFileName);
                }
                if (sSSPlateFileName != "")
                {
                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + objFieldEnum.sEnumDetailsID + "/" + sSSPlateFolderName + "/");
                        if (IsExists == false)
                        {
                            objFtp.createDirectory(SFTPmainfolder + objFieldEnum.sEnumDetailsID + "/" + sSSPlateFolderName);
                        }
                        Isuploaded = objFtp.upload(SFTPmainfolder + objFieldEnum.sEnumDetailsID + "/" + sSSPlateFolderName + "/", sSSPlateFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objFieldEnum.sSSPlatePhotoPath = objFieldEnum.sEnumDetailsID + "/" + sSSPlateFolderName + "/" + sSSPlateFileName;
                            txtSSPlatePath.Text = objFieldEnum.sSSPlatePhotoPath;
                            Session["fupSSPlate"] = null;
                        }
                    }
                }

                // Old Code Photo Save

                if (txtOLDDTCPath.Text.Trim() != "")
                {
                    sOldCodePhotoExtension = System.IO.Path.GetExtension(txtOLDDTCPath.Text).ToString().ToLower();
                    sOldCodePhotoExtension = ";" + sOldCodePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sOldCodePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sOldCodeFileName = Path.GetFileName(txtOLDDTCPath.Text);
                    sDirectory = txtOLDDTCPath.Text;
                    objFieldEnum.sOldCodePhotoPath = txtOLDDTCPath.Text;
                }

                else if (fupOldCodePhoto.PostedFile.ContentLength != 0)
                {

                    sOldCodePhotoExtension = System.IO.Path.GetExtension(fupOldCodePhoto.FileName).ToString().ToLower();
                    sOldCodePhotoExtension = ";" + sOldCodePhotoExtension.Remove(0, 1) + ";";


                    if (!sFileExt.Contains(sOldCodePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sOldCodeFileName = Path.GetFileName(fupOldCodePhoto.PostedFile.FileName);

                    fupOldCodePhoto.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sOldCodeFileName));
                    sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sOldCodeFileName);
                }
                if (sOldCodeFileName != "")
                {
                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + objFieldEnum.sEnumDetailsID + "/" + sOldCodeFolderName + "/");
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(SFTPmainfolder + objFieldEnum.sEnumDetailsID + "/" + sOldCodeFolderName);
                        }

                        Isuploaded = objFtp.upload(SFTPmainfolder + objFieldEnum.sEnumDetailsID + "/" + sOldCodeFolderName + "/", sOldCodeFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objFieldEnum.sOldCodePhotoPath = objFieldEnum.sEnumDetailsID + "/" + sOldCodeFolderName + "/" + sOldCodeFileName;
                            txtOLDDTCPath.Text = objFieldEnum.sOldCodePhotoPath;
                            Session["fupOldCodePhoto"] = null;
                        }
                    }
                }

                // IP Enum Code Photo Save
                if (txtIPDTCPath.Text.Trim() != "")
                {
                    sIPEnumCodePhotoExtension = System.IO.Path.GetExtension(txtIPDTCPath.Text).ToString().ToLower();
                    sIPEnumCodePhotoExtension = ";" + sIPEnumCodePhotoExtension.Remove(0, 1) + ";";


                    if (!sFileExt.Contains(sIPEnumCodePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sIPEnumCodeFileName = Path.GetFileName(txtIPDTCPath.Text);
                    sDirectory = txtIPDTCPath.Text;
                    objFieldEnum.sIPEnumCodePhotoPath = txtIPDTCPath.Text;
                }
                else if (fupIPEnum.PostedFile.ContentLength != 0)
                {

                    sIPEnumCodePhotoExtension = System.IO.Path.GetExtension(fupIPEnum.FileName).ToString().ToLower();
                    sIPEnumCodePhotoExtension = ";" + sIPEnumCodePhotoExtension.Remove(0, 1) + ";";


                    if (!sFileExt.Contains(sIPEnumCodePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sIPEnumCodeFileName = Path.GetFileName(fupIPEnum.PostedFile.FileName);

                    fupIPEnum.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sIPEnumCodeFileName));
                    sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sIPEnumCodeFileName);
                }
                if (sIPEnumCodeFileName != "")
                {
                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + objFieldEnum.sEnumDetailsID + "/" + sIPEnumFolderName + "/");
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(SFTPmainfolder + objFieldEnum.sEnumDetailsID + "/" + sIPEnumFolderName);
                        }

                        Isuploaded = objFtp.upload(SFTPmainfolder + objFieldEnum.sEnumDetailsID + "/" + sIPEnumFolderName + "/", sIPEnumCodeFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objFieldEnum.sIPEnumCodePhotoPath = objFieldEnum.sEnumDetailsID + "/" + sIPEnumFolderName + "/" + sIPEnumCodeFileName;
                            txtIPDTCPath.Text = objFieldEnum.sIPEnumCodePhotoPath;
                            Session["fupIPEnum"] = null;
                        }
                    }
                }

                // DTLMS Code Photo Save

                if (txtDTLMSDTCPath.Text.Trim() != "")
                {
                    sDTLMSCodePhotoExtension = System.IO.Path.GetExtension(txtDTLMSDTCPath.Text).ToString().ToLower();
                    sDTLMSCodePhotoExtension = ";" + sDTLMSCodePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sDTLMSCodePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sDTLMSCodeFileName = Path.GetFileName(txtDTLMSDTCPath.Text);
                    sDirectory = txtDTLMSDTCPath.Text;
                    objFieldEnum.sDTLMSCodePhotoPath = txtDTLMSDTCPath.Text;
                }

                else if (fupDTLMSCodePhoto.PostedFile.ContentLength != 0)
                {

                    sDTLMSCodePhotoExtension = System.IO.Path.GetExtension(fupDTLMSCodePhoto.FileName).ToString().ToLower();
                    sDTLMSCodePhotoExtension = ";" + sDTLMSCodePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sDTLMSCodePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sDTLMSCodeFileName = Path.GetFileName(fupDTLMSCodePhoto.PostedFile.FileName);

                    fupDTLMSCodePhoto.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTLMSCodeFileName));
                    sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTLMSCodeFileName);

                }
                if (sDTLMSCodeFileName != "")
                {
                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + objFieldEnum.sEnumDetailsID + "/" + sDTLMSFolderName + "/");
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(SFTPmainfolder + objFieldEnum.sEnumDetailsID + "/" + sDTLMSFolderName);
                        }

                        Isuploaded = objFtp.upload(SFTPmainfolder + objFieldEnum.sEnumDetailsID + "/" + sDTLMSFolderName + "/", sDTLMSCodeFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objFieldEnum.sDTLMSCodePhotoPath = objFieldEnum.sEnumDetailsID + "/" + sDTLMSFolderName + "/" + sDTLMSCodeFileName;
                            txtDTLMSDTCPath.Text = objFieldEnum.sDTLMSCodePhotoPath;
                            Session["fupDTLMSCodePhoto"] = null;
                        }
                    }
                }


                // DTC Photo Save
                if (txtDTCPath.Text.Trim() != "")
                {
                    sDTCPhotoExtension = System.IO.Path.GetExtension(txtDTCPath.Text).ToString().ToLower();
                    sDTCPhotoExtension = ";" + sDTCPhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sDTCPhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sDTCFileName = Path.GetFileName(txtDTCPath.Text);
                    sDirectory = txtDTCPath.Text;
                    objFieldEnum.sDTCPhotoPath = txtDTCPath.Text;
                }
                else if (fupDTCPhoto.PostedFile.ContentLength != 0)
                {

                    sDTCPhotoExtension = System.IO.Path.GetExtension(fupDTCPhoto.FileName).ToString().ToLower();
                    sDTCPhotoExtension = ";" + sDTCPhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sDTCPhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sDTCFileName = Path.GetFileName(fupDTCPhoto.PostedFile.FileName);


                    fupDTCPhoto.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTCFileName));
                    sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTCFileName);
                }
                if (sDTCFileName != "")
                {
                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + objFieldEnum.sEnumDetailsID + "/" + sDTCFolderName + "/");
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(SFTPmainfolder + objFieldEnum.sEnumDetailsID + "/" + sDTCFolderName);
                        }

                        Isuploaded = objFtp.upload(SFTPmainfolder + objFieldEnum.sEnumDetailsID + "/" + sDTCFolderName + "/", sDTCFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objFieldEnum.sDTCPhotoPath = objFieldEnum.sEnumDetailsID + "/" + sDTCFolderName + "/" + sDTCFileName;
                            txtDTCPath.Text = objFieldEnum.sDTCPhotoPath;
                            Session["fupDTCPhoto"] = null;
                        }
                    }
                }

                bool bResult;

                //if (txtEnumDetailsId.Text.Trim() == "")
                //{
                //    bResult = objFieldEnum.SaveImagePathDetails(objFieldEnum);
                //}
                //else
                //{
                //    bResult = objFieldEnum.UpdateImagePathDetails(objFieldEnum);
                //}

                bResult = objFieldEnum.UpdateImagePathDetails(objFieldEnum);

                return bResult;

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }
        /// <summary>
        /// to save the enumeration details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                //if (cmbRework.SelectedValue == "1")
                //{
                //    clsFieldEnumeration ObjField = new clsFieldEnumeration();
                //    ObjField.UpdateReworkStatus(objSession.UserType, txtEnumDetailsId.Text, objSession.UserId, cmbRework.SelectedValue);
                //    ShowMsgBox("Rework Status Updated Successfully");
                //    return;
                //}
                DateTime dt = System.DateTime.Now;
                if (ValidateForm() == true)
                {
                    SaveEnumerationDetails();
                    ShowUploadedImages();
                }
                double Time = (System.DateTime.Now - dt).TotalSeconds;
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// on change method for circle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle.SelectedIndex > 0)
                {
                    if (objSession.RoleId == "8" || objSession.sRoleType == "3")
                    {
                        Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\"  WHERE  \"DIV_CICLE_CODE\"='" + cmbCircle.SelectedValue + "' ORDER BY \"DIV_CODE\"", "--Select--", cmbDivision);
                    }
                    else
                    {
                        Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\"  WHERE \"DIV_CODE\"='" + objSession.OfficeCode.Substring(0, Division_code) + "' AND \"DIV_CICLE_CODE\"='" + cmbCircle.SelectedValue + "' ORDER BY \"DIV_CODE\"", "--Select--", cmbDivision);
                    }
                }
                else
                {
                    cmbDivision.Items.Clear();
                    cmbsubdivision.Items.Clear();
                    cmbSection.Items.Clear();
                    cmbFeeder.Items.Clear();
                }
                string strQry = string.Empty;

                objSession = (clsSession)Session["clsSession"];
                string stroffCode = string.Empty;
                if (objSession.OfficeCode.Length <= 2 && objSession.OfficeCode.Length != 0)
                {
                    stroffCode = clsStoreOffice.GetZone_Circle_Div_Offcode(objSession.OfficeCode, objSession.RoleId).Substring(0, Constants.Circle);
                }
                else
                {
                    stroffCode = objSession.OfficeCode;
                }
                string stroffCode1 = stroffCode;
                if (stroffCode.Length >= 2)
                {
                    if (stroffCode.Length >= 3)
                    {
                        stroffCode = stroffCode.Substring(0, Division_code);
                        cmbDivision.Items.FindByValue(stroffCode).Selected = true;
                        cmbDivision.Enabled = false;

                        stroffCode = stroffCode1;
                    }
                }

                if (stroffCode.Length >= 2)
                {


                    Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\", \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\"  WHERE \"SD_SUBDIV_CODE\"='" + objSession.OfficeCode.Substring(0, SubDiv_code) + "' AND \"SD_DIV_CODE\"='" + cmbDivision.SelectedValue + "' ORDER BY \"SD_SUBDIV_CODE\"", "--Select--", cmbsubdivision);



                    if (stroffCode.Length >= 3)
                    {
                        stroffCode = stroffCode.Substring(0, SubDiv_code);
                        cmbsubdivision.Items.FindByValue(stroffCode).Selected = true;
                        cmbsubdivision.Enabled = false;



                        stroffCode = stroffCode1;
                    }
                }
                if (stroffCode.Length >= 4)
                {
                    Genaral.Load_Combo("SELECT \"OM_CODE\", \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_CODE\"='" + objSession.OfficeCode.Substring(0, Section_code) + "' AND \"OM_SUBDIV_CODE\" = '" + cmbsubdivision.SelectedValue + "' ORDER BY \"OM_CODE\"", "--Select--", cmbSection);
                    if (stroffCode.Length >= 4)
                    {
                        stroffCode = stroffCode.Substring(0, Section_code);
                        cmbSection.Items.FindByValue(stroffCode).Selected = true;
                        cmbSection.Enabled = false;
                        stroffCode = stroffCode1;
                        strQry = "SELECT \"FD_FEEDER_CODE\",\"FD_FEEDER_CODE\" || '-' || \"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\", \"TBLFEEDEROFFCODE\" WHERE  \"FD_FEEDER_ID\" = \"FDO_FEEDER_ID\" AND";
                        strQry += " cast(\"FDO_OFFICE_CODE\" as text) LIKE '" + cmbsubdivision.SelectedValue + "%' ORDER BY \"FD_FEEDER_CODE\"";
                        Genaral.Load_Combo(strQry, "--Select--", cmbFeeder);
                    }
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// onchange method for division
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDivision.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\", \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\"  WHERE \"SD_DIV_CODE\"='" + cmbDivision.SelectedValue + "' ORDER BY \"SD_SUBDIV_CODE\"", "--Select--", cmbsubdivision);
                }
                else
                {
                    cmbsubdivision.Items.Clear();
                    cmbSection.Items.Clear();
                    cmbFeeder.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// on change method for subdivision
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbsubdivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbsubdivision.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"OM_CODE\", \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_SUBDIV_CODE\" = '" + cmbsubdivision.SelectedValue + "' ORDER BY \"OM_CODE\"", "--Select--", cmbSection);
                }
                else
                {
                    cmbSection.Items.Clear();
                    cmbFeeder.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// on change method for section
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbsubdivision.SelectedIndex > 0)
                {
                    string strQry = "SELECT DISTINCT \"FD_FEEDER_CODE\",\"FD_FEEDER_CODE\" || '-' || \"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\", \"TBLFEEDEROFFCODE\" WHERE  \"FD_FEEDER_ID\" = \"FDO_FEEDER_ID\" AND";
                    strQry += " cast(\"FDO_OFFICE_CODE\" as text) LIKE '" + cmbSection.SelectedValue + "%' ORDER BY \"FD_FEEDER_CODE\"";
                    Genaral.Load_Combo(strQry, "--Select--", cmbFeeder);
                }
                else
                {
                    cmbSection.Items.Clear();
                    cmbFeeder.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        //UnUsed Method

        /// <summary>
        /// on change method for feeder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void cmbFeeder_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (txtTcCode.Text == null || txtTcCode.Text == "")
        //        {

        //            if (cmbFeeder.SelectedIndex > 0)
        //            {
        //                clsFieldEnumeration ObjField = new clsFieldEnumeration();
        //                ObjField.sFeederCode = cmbFeeder.SelectedValue;

        //                string sClientIP = string.Empty;

        //                string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        //                if (!string.IsNullOrEmpty(ip))
        //                {
        //                    string[] ipRange = ip.Split(',');
        //                    int le = ipRange.Length - 1;
        //                    sClientIP = ipRange[0];
        //                }
        //                else
        //                {
        //                    sClientIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        //                }

        //                ObjField.sClientIP = sClientIP;

        //                ObjField.sCrBy = objSession.UserId;
        //                ObjField.sOfficeCode = objSession.OfficeCode;
        //                txtDTCCode.Text = ObjField.GeneratefeederCode(ObjField);
        //                ObjField.stempdtccode = txtDTCCode.Text;
        //                txtDTCCode.ReadOnly = true;
        //                txtTcCode.Text = ObjField.Generatetccode(ObjField);
        //                txtTcCode.ReadOnly = true;

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblmessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}


        /// <summary>
        ///  to load field enumeration details
        /// </summary>
        /// <param name="sOperator"></param>
        //public void LoadFieldEnumeration(string sOperator = "")
        //{
        //    try
        //    {
        //        clsFieldEnumeration objFieldEnum = new clsFieldEnumeration();
        //        DataTable dt = new DataTable();
        //        dt = objFieldEnum.LoadFieldEnumeration(sOperator);
        //        grdFieldEnumDetails.DataSource = dt;
        //        grdFieldEnumDetails.DataBind();
        //        ViewState["Enum"] = dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        lblmessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}


        // UnUsed Method

        // on change method for old dtc change
        //protected void rdbOldDtc_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        otherDetails.Attributes.Add("class", "tab-pane fade in active");
        //        TCDetails.Attributes.Add("class", "tab-pane fade");
        //        DTCDetails.Attributes.Add("class", "tab-pane fade");

        //        liTCDetails.Attributes.Add("class", "");
        //        liDTCDetails.Attributes.Add("class", "");
        //        liOtherDetails.Attributes.Add("class", "active");

        //        if (txtoldDTCCode.Text.Trim() == "")
        //        {
        //            ShowMsgBox("Enter Old DTC Code(HESCOM)");
        //            return;
        //        }
        //        clsFieldEnumeration objFieldEnum = new clsFieldEnumeration();

        //        objFieldEnum.sOldDTCCode = txtoldDTCCode.Text;

        //        objFieldEnum.GetCESCOldData(objFieldEnum);

        //        txtLatitude.Text = objFieldEnum.sLatitude;
        //        txtLongitude.Text = objFieldEnum.sLongitude;
        //        cmbLightArrester.SelectedValue = objFieldEnum.sArresters;
        //        cmbBreakerType.SelectedValue = objFieldEnum.sBreakertype;
        //        cmbHTProtection.SelectedValue = objFieldEnum.sHTProtect;
        //        cmbLTProtection.SelectedValue = objFieldEnum.sLTProtect;
        //        cmbDTCMetered.SelectedValue = objFieldEnum.sDTCMeters;
        //        cmbGrounding.SelectedValue = objFieldEnum.sGrounding;
        //        txtConnectedHP.Text = objFieldEnum.sConnectedHP;
        //        txtConnectedKW.Text = objFieldEnum.sConnectedKW;
        //        txtInternalCode.Text = objFieldEnum.sInternalCode;
        //    }
        //    catch (Exception ex)
        //    {
        //        lblmessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}
        // UnUsed Method

        /// <summary>
        /// on change method for ip enum image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void rdbIPEnum_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        otherDetails.Attributes.Add("class", "tab-pane fade in active");
        //        TCDetails.Attributes.Add("class", "tab-pane fade");
        //        DTCDetails.Attributes.Add("class", "tab-pane fade");

        //        liTCDetails.Attributes.Add("class", "");
        //        liDTCDetails.Attributes.Add("class", "");
        //        liOtherDetails.Attributes.Add("class", "active");

        //        if (txtIPDTCCode.Text.Trim() == "")
        //        {
        //            ShowMsgBox("Enter IP Enumeration DTC Code");
        //            return;
        //        }


        //        clsFieldEnumeration objFieldEnum = new clsFieldEnumeration();

        //        objFieldEnum.sIPDTCCode = txtIPDTCCode.Text;

        //        objFieldEnum.GetIPEnumerationData(objFieldEnum);

        //        txtLatitude.Text = objFieldEnum.sLatitude;
        //        txtLongitude.Text = objFieldEnum.sLongitude;
        //        cmbLightArrester.SelectedValue = objFieldEnum.sArresters;
        //        cmbBreakerType.SelectedValue = objFieldEnum.sBreakertype;
        //        cmbHTProtection.SelectedValue = objFieldEnum.sHTProtect;
        //        cmbLTProtection.SelectedValue = objFieldEnum.sLTProtect;
        //        cmbDTCMetered.SelectedValue = objFieldEnum.sDTCMeters;
        //        cmbGrounding.SelectedValue = objFieldEnum.sGrounding;
        //        txtConnectedHP.Text = objFieldEnum.sConnectedHP;
        //        txtConnectedKW.Text = objFieldEnum.sConnectedKW;

        //    }
        //    catch (Exception ex)
        //    {
        //        lblmessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}



        /// <summary>
        ///  method to reset the fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                Reset();
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to check the changes and to display labels
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdbDTLMS_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                otherDetails.Attributes.Add("class", "tab-pane fade in active");
                TCDetails.Attributes.Add("class", "tab-pane fade");

                liTCDetails.Attributes.Add("class", "");
                liOtherDetails.Attributes.Add("class", "active");


                ResetOtherDetails();

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        ///  to update based on conditions
        /// </summary>
        /// <param name="sApprovestatus"></param>
        /// <param name="sUsertype"></param>
        public void RestrictUpdatebasedonHeirarchy(string sApprovestatus, string sUsertype)
        {
            try
            {
                clsPreQCApproval ObjQc = new clsPreQCApproval();
                string sApprover = ObjQc.GetCurrentApprover(sApprovestatus);
                if (objSession.RoleId != "8")
                {
                    if (!sApprover.Contains(objSession.RoleId))
                    {
                        cmdSave.Enabled = false;
                        cmdReset.Enabled = false;
                    }

                }
                if (objSession.RoleId == "4")
                {
                    cmdSave.Enabled = true;
                }
                if (sApprovestatus == "1" && (sUsertype == "1" || sUsertype == "3"))
                {
                    cmdSave.Text = "Approve";
                }
                else if (sApprovestatus == "1" && sUsertype == "4")
                {
                    cmdSave.Text = "Approve";
                }
                else if (sApprovestatus == "2" && sUsertype == "6")
                {
                    cmdReject.Visible = true;
                    cmdSave.Text = "Approve";
                }
                else if (sApprovestatus == "3" && sUsertype == "7")
                {
                    cmdReject.Visible = true;
                    cmdSave.Text = "Approve";
                }
                else if (sApprovestatus == "4" && sUsertype == "7")
                {
                    cmdReject.Visible = true;
                    cmdSave.Text = "Approve";
                }
                else
                {
                    cmdSave.Text = "Update";
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }
        /// <summary>
        ///  to update based on conditions
        /// </summary>
        /// <param name="sStatusFlag"></param>
        public void RestrictUpdate(string sStatusFlag)
        {
            try
            {
                if (sStatusFlag == "1" || sStatusFlag == "3")
                {
                    cmdSave.Enabled = false;
                    cmdReset.Enabled = false;
                    grdFieldEnumDetails.Columns[10].Visible = false;
                }
                clsQCApproval objQC = new clsQCApproval();

                bool bResult = objQC.CheckEnumerationUpdateAuthority(objSession.UserId);
                if (bResult == false)
                {
                    cmdSave.Enabled = true;
                    cmdReset.Enabled = false;
                    grdFieldEnumDetails.Columns[10].Visible = false;
                }

                if (objSession.UserType == "4")
                {
                    if (txtEnumDetailsId.Text != "")
                    {
                        //cmdDelete.Visible = true;
                    }
                }


            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        ///  to reset the fields based on other details
        /// </summary>
        public void ResetOtherDetails()
        {
            try
            {
                txtInternalCode.Text = string.Empty;
                txtConnectedHP.Text = string.Empty;
                txtConnectedKW.Text = string.Empty;
                txtKWHReading.Text = string.Empty;
                txtCommisionDate.Text = string.Empty;
                txtServiceDate.Text = string.Empty;
                cmbPlatformType.SelectedIndex = 0;
                cmbBreakerType.SelectedIndex = 0;
                cmbDTCMetered.SelectedIndex = 0;
                cmbHTProtection.SelectedIndex = 0;
                cmbLTProtection.SelectedIndex = 0;
                cmbGrounding.SelectedIndex = 0;
                cmbLightArrester.SelectedIndex = 0;
                cmbLoadtype.SelectedIndex = 0;
                cmbProjecttype.SelectedIndex = 0;
                txtltLine.Text = string.Empty;
                txtDepreciation.Text = string.Empty;
                txtLatitude.Text = string.Empty;
                txtLongitude.Text = string.Empty;

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// to display images
        /// </summary>
        public void RetainImageinPostback()
        {
            try
            {
                //Name Plate Photo

                //Case: 1 When the page is submitted for the first time(First PostBack) and there is file 
                // in FileUpload control but session is Null then Store the values to Session Object as:
                if (Session["fupNamePlate"] == null && fupNamePlate.HasFile)
                {
                    Session["fupNamePlate"] = fupNamePlate;
                }
                // Case 2: On Next PostBack Session has value but FileUpload control is
                // Blank due to PostBack then return the values from session to FileUpload as:
                else if (Session["fupNamePlate"] != null && (!fupNamePlate.HasFile))
                {
                    fupNamePlate = (FileUpload)Session["fupNamePlate"];
                }
                // Case 3: When there is value in Session but user want to change the file then
                // In this case we need to change the file in session object also as:
                else if (fupNamePlate.HasFile)
                {
                    Session["fupNamePlate"] = fupNamePlate;
                }
                //SS Plate Photo

                //Case: 1 When the page is submitted for the first time(First PostBack) and there is file 
                // in FileUpload control but session is Null then Store the values to Session Object as:
                if (Session["fupSSPlate"] == null && fupSSPlate.HasFile)
                {
                    Session["fupSSPlate"] = fupSSPlate;
                }
                // Case 2: On Next PostBack Session has value but FileUpload control is
                // Blank due to PostBack then return the values from session to FileUpload as:
                else if (Session["fupSSPlate"] != null && (!fupSSPlate.HasFile))
                {
                    fupSSPlate = (FileUpload)Session["fupSSPlate"];
                }
                // Case 3: When there is value in Session but user want to change the file then
                // In this case we need to change the file in session object also as:
                else if (fupSSPlate.HasFile)
                {
                    Session["fupSSPlate"] = fupSSPlate;
                }

                // Old Code Photo

                //Case: 1 When the page is submitted for the first time(First PostBack) and there is file 
                // in FileUpload control but session is Null then Store the values to Session Object as:
                if (Session["fupOldCodePhoto"] == null && fupOldCodePhoto.HasFile)
                {
                    Session["fupOldCodePhoto"] = fupOldCodePhoto;
                }
                // Case 2: On Next PostBack Session has value but FileUpload control is
                // Blank due to PostBack then return the values from session to FileUpload as:
                else if (Session["fupOldCodePhoto"] != null && (!fupOldCodePhoto.HasFile))
                {
                    fupOldCodePhoto = (FileUpload)Session["fupOldCodePhoto"];
                }
                // Case 3: When there is value in Session but user want to change the file then
                // In this case we need to change the file in session object also as:
                else if (fupOldCodePhoto.HasFile)
                {
                    Session["fupOldCodePhoto"] = fupOldCodePhoto;
                }

                // IP Enumeration Code Photo

                //Case: 1 When the page is submitted for the first time(First PostBack) and there is file 
                // in FileUpload control but session is Null then Store the values to Session Object as:
                if (Session["fupIPEnum"] == null && fupIPEnum.HasFile)
                {
                    Session["fupIPEnum"] = fupIPEnum;
                }
                // Case 2: On Next PostBack Session has value but FileUpload control is
                // Blank due to PostBack then return the values from session to FileUpload as:
                else if (Session["fupIPEnum"] != null && (!fupIPEnum.HasFile))
                {
                    fupIPEnum = (FileUpload)Session["fupIPEnum"];
                }
                // Case 3: When there is value in Session but user want to change the file then
                // In this case we need to change the file in session object also as:
                else if (fupIPEnum.HasFile)
                {
                    Session["fupIPEnum"] = fupIPEnum;
                }

                // DTLMS Code Photo

                //Case: 1 When the page is submitted for the first time(First PostBack) and there is file 
                // in FileUpload control but session is Null then Store the values to Session Object as:
                if (Session["fupDTLMSCodePhoto"] == null && fupDTLMSCodePhoto.HasFile)
                {
                    Session["fupDTLMSCodePhoto"] = fupDTLMSCodePhoto;
                }
                // Case 2: On Next PostBack Session has value but FileUpload control is
                // Blank due to PostBack then return the values from session to FileUpload as:
                else if (Session["fupDTLMSCodePhoto"] != null && (!fupDTLMSCodePhoto.HasFile))
                {
                    fupDTLMSCodePhoto = (FileUpload)Session["fupDTLMSCodePhoto"];
                }
                // Case 3: When there is value in Session but user want to change the file then
                // In this case we need to change the file in session object also as:
                else if (fupDTLMSCodePhoto.HasFile)
                {
                    Session["fupDTLMSCodePhoto"] = fupDTLMSCodePhoto;
                }


                // DTC Photo

                //Case: 1 When the page is submitted for the first time(First PostBack) and there is file 
                // in FileUpload control but session is Null then Store the values to Session Object as:
                if (Session["fupDTCPhoto"] == null && fupDTCPhoto.HasFile)
                {
                    Session["fupDTCPhoto"] = fupDTCPhoto;
                }
                // Case 2: On Next PostBack Session has value but FileUpload control is
                // Blank due to PostBack then return the values from session to FileUpload as:
                else if (Session["fupDTCPhoto"] != null && (!fupDTCPhoto.HasFile))
                {
                    fupDTCPhoto = (FileUpload)Session["fupDTCPhoto"];
                }
                // Case 3: When there is value in Session but user want to change the file then
                // In this case we need to change the file in session object also as:
                else if (fupDTCPhoto.HasFile)
                {
                    Session["fupDTCPhoto"] = fupDTCPhoto;
                }

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        //UnUsed Method

        /// <summary>
        /// on test change method for dtc code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void txtIPDTCCode_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        clsFieldEnumeration objFieldEnum = new clsFieldEnumeration();
        //        objFieldEnum.sIPDTCCode = txtIPDTCCode.Text;
        //        objFieldEnum.GetIPEnumerationData(objFieldEnum);
        //        if (objFieldEnum.sDTCName != null)
        //        {
        //            txtDTCName.Text = objFieldEnum.sDTCName;
        //        }

        //        DTCDetails.Attributes.Add("class", "tab-pane fade in active");
        //        liDTCDetails.Attributes.Add("class", "active");
        //        liTCDetails.Attributes.Add("class", "");
        //        liOtherDetails.Attributes.Add("class", "");
        //        TCDetails.Attributes.Add("class", "tab-pane fade");
        //    }
        //    catch (Exception ex)
        //    {
        //        lblmessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}

            //UnUsed Method

        /// <summary>
        /// on test change method for old dtc code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void txtoldDTCCode_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        clsFieldEnumeration objFieldEnum = new clsFieldEnumeration();
        //        objFieldEnum.sOldDTCCode = txtoldDTCCode.Text;
        //        objFieldEnum.GetCESCOldData(objFieldEnum);

        //        if (txtDTCName.Text == "")
        //        {
        //            txtDTCName.Text = objFieldEnum.sDTCName;
        //        }


        //        DTCDetails.Attributes.Add("class", "tab-pane fade in active");
        //        liDTCDetails.Attributes.Add("class", "active");
        //        liTCDetails.Attributes.Add("class", "");
        //        liOtherDetails.Attributes.Add("class", "");
        //        TCDetails.Attributes.Add("class", "tab-pane fade");
        //    }
        //    catch (Exception ex)
        //    {
        //        lblmessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}

        /// <summary>
        /// on  change method for make
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbMake_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbMake.SelectedValue == "1")
                {

                    if (txtTcCode.Text.Trim() == "")
                    {
                        ShowMsgBox("Enter DTR Code");
                        cmbMake.SelectedIndex = 0;
                        return;
                    }
                    txtTcslno.Enabled = false;
                    txtTcslno.Text = "NNP" + txtTcCode.Text;
                    Div1.Style.Add("display", "none");
                }
                else
                {
                    txtTcslno.Enabled = true;
                    txtTcslno.Text = "";
                    Div1.Style.Add("display", "block");
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// on  change method for rating
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbRating_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbRating.SelectedValue == "1")
                {
                    dvStar.Style.Add("display", "block");
                    Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'SRT'", "--Select--", cmbStarRated);
                }
                else
                {
                    dvStar.Style.Add("display", "none");
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// method to display images
        /// </summary>
        public void RetainImageOnPostback()
        {
            try
            {
                string sDirectory = string.Empty;

                string sNamePlateFileName = string.Empty;
                string sSSPlateFileName = string.Empty;
                string sOldCodeFileName = string.Empty;
                string sIPEnumCodeFileName = string.Empty;
                string sDTLMSCodeFileName = string.Empty;
                string sDTCFileName = string.Empty;
                string sInfosysCodeFileName = string.Empty;

                if (fupSSPlate.HasFile)
                {
                    sSSPlateFileName = Path.GetFileName(fupSSPlate.PostedFile.FileName);
                    txtSSPlatePath.Text = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sSSPlateFileName);
                    if (!File.Exists(txtSSPlatePath.Text))
                    {
                        fupSSPlate.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sSSPlateFileName));
                    }
                }

                if (fupNamePlate.HasFile)
                {
                    sNamePlateFileName = Path.GetFileName(fupNamePlate.PostedFile.FileName);
                    txtNamePlatePhotoPath.Text = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sNamePlateFileName);
                    if (!File.Exists(txtNamePlatePhotoPath.Text))
                    {
                        fupNamePlate.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sNamePlateFileName));
                    }
                }

                if (fupDTLMSCodePhoto.HasFile)
                {
                    sDTLMSCodeFileName = Path.GetFileName(fupDTLMSCodePhoto.PostedFile.FileName);
                    txtDTLMSDTCPath.Text = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTLMSCodeFileName);
                    if (!File.Exists(txtDTLMSDTCPath.Text))
                    {
                        fupDTLMSCodePhoto.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTLMSCodeFileName));
                    }
                }

                if (fupOldCodePhoto.HasFile)
                {
                    sOldCodeFileName = Path.GetFileName(fupOldCodePhoto.PostedFile.FileName);
                    txtOLDDTCPath.Text = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sOldCodeFileName);
                    if (!File.Exists(txtOLDDTCPath.Text))
                    {
                        fupOldCodePhoto.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sOldCodeFileName));
                    }
                }

                if (fupIPEnum.HasFile)
                {
                    sIPEnumCodeFileName = Path.GetFileName(fupIPEnum.PostedFile.FileName);
                    txtIPDTCPath.Text = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sIPEnumCodeFileName);
                    if (!File.Exists(txtIPDTCPath.Text))
                    {
                        fupIPEnum.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sIPEnumCodeFileName));
                    }
                }

                if (fupDTCPhoto.HasFile)
                {
                    sDTCFileName = Path.GetFileName(fupDTCPhoto.PostedFile.FileName);
                    txtDTCPath.Text = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTCFileName);
                    if (!File.Exists(txtDTCPath.Text))
                    {
                        fupDTCPhoto.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTCFileName));
                    }
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        ///  to validate the image formate
        /// </summary>
        /// <returns></returns>
        public string ValidateImages()
        {
            string svalidate = string.Empty;
            try
            {
                //FileType Parameter
                string sPlatePhotoExtension = string.Empty;
                string sSSPlatePhotoExtension = string.Empty;
                string sOldCodePhotoExtension = string.Empty;
                string sIPEnumCodePhotoExtension = string.Empty;
                string sDTLMSCodePhotoExtension = string.Empty;
                string sDTCPhotoExtension = string.Empty;
                string sInfosysCodePhotoExtension = string.Empty;

                //  Photo Save DTLMSDocs
                string sFileExt = Convert.ToString(ConfigurationManager.AppSettings["FileFormat"]);

                //Name Plate Photo
                if (txtNamePlatePhotoPath.Text.Trim() != "")
                {
                    sPlatePhotoExtension = System.IO.Path.GetExtension(txtNamePlatePhotoPath.Text).ToString().ToLower();
                    sPlatePhotoExtension = ";" + sPlatePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sPlatePhotoExtension))
                    {
                        return "Invalid Image Format in Name Plate Photo";
                    }

                }
                else if (fupNamePlate.PostedFile.ContentLength != 0)
                {
                    sPlatePhotoExtension = System.IO.Path.GetExtension(fupNamePlate.FileName).ToString().ToLower();
                    sPlatePhotoExtension = ";" + sPlatePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sPlatePhotoExtension))
                    {
                        return "Invalid Image Format in Name Plate Photo";
                    }
                }

                if (txtSSPlatePath.Text.Trim() != "")
                {
                    sSSPlatePhotoExtension = System.IO.Path.GetExtension(txtSSPlatePath.Text).ToString().ToLower();
                    sSSPlatePhotoExtension = ";" + sSSPlatePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sSSPlatePhotoExtension))
                    {
                        return "Invalid Image Format in DTr photo";
                    }
                }

                else if (fupSSPlate.PostedFile.ContentLength != 0)
                {

                    sSSPlatePhotoExtension = System.IO.Path.GetExtension(fupSSPlate.FileName).ToString().ToLower();
                    sSSPlatePhotoExtension = ";" + sSSPlatePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sSSPlatePhotoExtension))
                    {
                        return "Invalid Image Format in DTr photo";
                    }
                }
                // Old Code Photo Save

                if (txtOLDDTCPath.Text.Trim() != "")
                {
                    sOldCodePhotoExtension = System.IO.Path.GetExtension(txtOLDDTCPath.Text).ToString().ToLower();
                    sOldCodePhotoExtension = ";" + sOldCodePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sOldCodePhotoExtension))
                    {
                        return "Invalid Image Format in OLD DTC Code(HESCOM) Photo";
                    }

                }

                else if (fupOldCodePhoto.PostedFile.ContentLength != 0)
                {
                    sOldCodePhotoExtension = System.IO.Path.GetExtension(fupOldCodePhoto.FileName).ToString().ToLower();
                    sOldCodePhotoExtension = ";" + sOldCodePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sOldCodePhotoExtension))
                    {
                        return "Invalid Image Format in OLD DTC Code(HESCOM) Photo";
                    }
                }

                // IP Enum Code Photo Save
                if (txtIPDTCPath.Text.Trim() != "")
                {
                    sIPEnumCodePhotoExtension = System.IO.Path.GetExtension(txtIPDTCPath.Text).ToString().ToLower();
                    sIPEnumCodePhotoExtension = ";" + sIPEnumCodePhotoExtension.Remove(0, 1) + ";";


                    if (!sFileExt.Contains(sIPEnumCodePhotoExtension))
                    {
                        return "Invalid Image Format in  DTC Code(IP Enum) Photo";
                    }

                }
                else if (fupIPEnum.PostedFile.ContentLength != 0)
                {

                    sIPEnumCodePhotoExtension = System.IO.Path.GetExtension(fupIPEnum.FileName).ToString().ToLower();
                    sIPEnumCodePhotoExtension = ";" + sIPEnumCodePhotoExtension.Remove(0, 1) + ";";


                    if (!sFileExt.Contains(sIPEnumCodePhotoExtension))
                    {
                        return "Invalid Image Format in DTC Code(IP Enum) Photo";
                    }

                }

                // DTLMS Code Photo Save
                if (txtDTLMSDTCPath.Text.Trim() != "")
                {
                    sDTLMSCodePhotoExtension = System.IO.Path.GetExtension(txtDTLMSDTCPath.Text).ToString().ToLower();
                    sDTLMSCodePhotoExtension = ";" + sDTLMSCodePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sDTLMSCodePhotoExtension))
                    {
                        return "Invalid Image Format  in DTC Code(DTLMS) Photo";
                    }
                }


                else if (fupDTLMSCodePhoto.PostedFile.ContentLength != 0)
                {

                    sDTLMSCodePhotoExtension = System.IO.Path.GetExtension(fupDTLMSCodePhoto.FileName).ToString().ToLower();
                    sDTLMSCodePhotoExtension = ";" + sDTLMSCodePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sDTLMSCodePhotoExtension))
                    {
                        return "Invalid Image Format in DTC Code(DTLMS) Photo";
                    }

                }

                // DTC Photo Save
                if (txtDTCPath.Text.Trim() != "")
                {
                    sDTCPhotoExtension = System.IO.Path.GetExtension(txtDTCPath.Text).ToString().ToLower();
                    sDTCPhotoExtension = ";" + sDTCPhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sDTCPhotoExtension))
                    {
                        return "Invalid Image Format in DTC Photo";
                    }

                }
                else if (fupDTCPhoto.PostedFile.ContentLength != 0)
                {

                    sDTCPhotoExtension = System.IO.Path.GetExtension(fupDTCPhoto.FileName).ToString().ToLower();
                    sDTCPhotoExtension = ";" + sDTCPhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sDTCPhotoExtension))
                    {
                        return "Invalid Image Format in DTC Photo";
                    }

                }


                // Infosys Photo Save
                if (txtInfosysPath.Text.Trim() != "")
                {
                    sInfosysCodePhotoExtension = System.IO.Path.GetExtension(txtInfosysPath.Text).ToString().ToLower();
                    sInfosysCodePhotoExtension = ";" + sInfosysCodePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sInfosysCodePhotoExtension))
                    {
                        return "Invalid Image Format in Infosys Asset ID photo";
                    }

                }
                return "";
            }

            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ValidateImages");
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }
        /// <summary>
        ///  to display uploaded images
        /// </summary>
        public void ShowUploadedImages()
        {
            try
            {

                clsCommon objComm = new clsCommon();

                //FTP Parameter
                string sFTPLink = string.Empty;
                string sFTPUserName = string.Empty;
                string sFTPPassword = string.Empty;


                // To bind the Images from Ftp Path to Image Control
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

                sFTPLink = ConfigurationManager.AppSettings["VirtualDirectoryPath"].ToString();
                clsFtp objFtp = new clsFtp(sFTPLink, sFTPUserName, sFTPPassword);


                if (txtNamePlatePhotoPath.Text != "")
                {
                    dvNamePlate.Style.Add("display", "block");
                    imgNamePlate.ImageUrl = sFTPLink + txtNamePlatePhotoPath.Text;
                }
                if (txtSSPlatePath.Text != "")
                {
                    dvSSPlate.Style.Add("display", "block");
                    imgSSPlate.ImageUrl = sFTPLink + txtSSPlatePath.Text;
                }

                if (txtOLDDTCPath.Text != "")
                {
                    dvOldDTCBESCOM.Style.Add("display", "block");
                    imgOldCode.ImageUrl = sFTPLink + txtOLDDTCPath.Text;
                }
                if (txtDTLMSDTCPath.Text != "")
                {
                    dvDTLMSPhoto.Style.Add("display", "block");
                    imgDTLMS.ImageUrl = sFTPLink + txtDTLMSDTCPath.Text;
                }
                if (txtIPDTCPath.Text != "")
                {
                    dvIPEnumPhoto.Style.Add("display", "block");
                    imgIPEnum.ImageUrl = sFTPLink + txtIPDTCPath.Text;
                }
                if (txtInfosysPath.Text != "")
                {
                    dvInfosys.Style.Add("display", "block");
                    imgInfosys.ImageUrl = sFTPLink + txtInfosysPath.Text;
                }
                if (txtDTCPath.Text != "")
                {
                    dvDTCPhoto.Style.Add("display", "block");

                    if (txtDTCPath.Text.Contains(";"))
                    {
                        string sDTCPhotoPath = txtDTCPath.Text;
                        txtDTCPath.Text = txtDTCPath.Text.Split(';').GetValue(0).ToString();
                        dvDTCPhoto1.Style.Add("display", "block");

                        string sDTCPhoto2 = sDTCPhotoPath.Split(';').GetValue(1).ToString();
                        imgDTCPhoto1.ImageUrl = sFTPLink + sDTCPhoto2;
                    }
                    imgDTCPhoto.ImageUrl = sFTPLink + txtDTCPath.Text;
                }

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }
        /// <summary>
        ///  to load images
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdLoadImage_Click(object sender, EventArgs e)
        {
            try
            {
                ShowUploadedImages();
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }

        //UnUsed Method

        /// <summary>
        ///  method for delete case
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void cmdDelete_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        clsFieldEnumeration objField = new clsFieldEnumeration();

        //        objField.sEnumDetailsID = txtEnumDetailsId.Text;
        //        bool bResult = objField.DeleteEnumerationDetails(objField);
        //        if (bResult == true)
        //        {
        //            ShowMsgBox("Deleted Successfully");

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblmessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

        //    }
        //}

            //UnUsed Method

        /// <summary>
        ///  to load next enumeration details 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void cmdNext_Click(object sender, EventArgs e)
        //{
        //    GetNextEnumerationDetails();
        //}


        /// <summary>
        ///  to close the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdClose_Click(object sender, EventArgs e)
        {
            objSession = (clsSession)Session["clsSession"];
            Response.Redirect("EnumerationView.aspx", false);

        }
        /// <summary>
        ///  method to perform on reject case
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdReject_Click(object sender, EventArgs e)
        {
            string[] Arr = new string[3];
            try
            {
                clsFieldEnumeration objFieldEnum = new clsFieldEnumeration();

                objFieldEnum.sRemark = "REJECTED IN PRE APPROVER";
                objFieldEnum.sEnumDetailsID = txtEnumDetailsId.Text;
                objFieldEnum.sCrBy = objSession.UserId;

                Arr = objFieldEnum.RejectEnumerationDetails(objFieldEnum);

                if (Arr[0] == "1")
                {
                    ShowMsgBox("Enumeration Details Rejected Successfully");

                    cmdSave.Enabled = false;
                    cmdReject.Enabled = false;
                }
                else if (Arr[0] == "0")
                {
                    ShowMsgBox(Arr[1].ToString());
                    return;
                }
                else
                {
                    ShowMsgBox("Reject Failed");
                    return;
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        ///  on change method for capacity
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbCapacity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Random random = new Random();
                var next = random.NextDouble();
                double d = -20 + (next * (20 - (-20)));

                string sKVA = cmbCapacity.SelectedValue;
                double TotalHP = Convert.ToDouble(sKVA) / 0.878;
                double TotalKw = TotalHP * 0.746;
                txtConnectedKW.Text = Convert.ToString(Math.Round(TotalKw + ((TotalKw / 100) * d)));
                txtConnectedHP.Text = Convert.ToString(Math.Round(TotalHP + ((TotalHP / 100) * d)));
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// on change method for dtcmeters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbDTCMeters_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDTCMetered.SelectedValue == "2")
                {
                    // txtKWHReading.Enabled = false;
                    idtxtKWHReading.Visible = false;
                    txtKWHReading.Text = "0";
                }
                else
                {
                    //txtKWHReading.Enabled = true;
                    idtxtKWHReading.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

    }
}