using IIITS.DTLMS.BL;
using System;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data;
using System.IO;
using System.Net;
using System.Data;
using System.IO;
using System.Configuration;
using System.Web.UI.WebControls;
using static IIITS.DTLMS.BL.Constants;
using IIITS.DTLMS.BL.POFlow;
using System.Reflection;

namespace IIITS.DTLMS.POFlow
{
    public partial class PMCInvoiceCreate : System.Web.UI.Page
    {

        clsSession objSession;
        int Circle_code = Convert.ToInt32(ConfigurationManager.AppSettings["Circle_code"]);
        int Division_code = Convert.ToInt32(ConfigurationManager.AppSettings["Division_code"]);
        int SubDiv_code = Convert.ToInt32(ConfigurationManager.AppSettings["SubDiv_code"]);
        int Section_code = Convert.ToInt32(ConfigurationManager.AppSettings["Section_code"]);
        int Feeder_code = Convert.ToInt32(ConfigurationManager.AppSettings["Feeder_code"]);
        string sFileServerPath = Convert.ToString(ConfigurationManager.AppSettings["EstimatioinVirtualPath"]);
        string sUserName = Convert.ToString(ConfigurationManager.AppSettings["FTP_USER"]);
        string sPassword = Convert.ToString(ConfigurationManager.AppSettings["FTP_PASS"]);
        string SFTPPath = Convert.ToString(ConfigurationManager.AppSettings["SFTPPath"]);
        string SFTPmainfolder = Convert.ToString(ConfigurationManager.AppSettings["SFTPmainfolder"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToString(Session["clsSession"] ?? "").Length == 0)
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                else
                {
                    string stroffCode = string.Empty;
                    string stroffCode1 = string.Empty;
                    string strQry = string.Empty;
                    DateTime dt = System.DateTime.Now;
                    txtInvoiceDate.Attributes.Add("readonly", "readonly");
                    txtInvoiceDate_CalendarExtender1.StartDate = System.DateTime.Now;
                    objSession = (clsSession)Session["clsSession"];
                    stroffCode = objSession.OfficeCode;

                    cmbCircle.Enabled = false;
                    cmbDivision.Enabled = false;
                    cmbsubdivision.Enabled = false;
                    cmbSection.Enabled = false;
                    cmbFeeder.Enabled = false;
                    txtwelddate.Enabled = false;
                    txtInternalCode.Enabled = false;
                    txtConnectedKW.Enabled = false;
                    txtConnectedHP.Enabled = false;
                    txtCommisionDate.Enabled = false;
                    txtDtrCommissionDate.Enabled = false;
                    txtServiceDate.Enabled = false;
                    cmbPlatformType.Enabled = false;
                    cmbBreakerType.Enabled = false;
                    cmbDTCMetered.Enabled = false;
                    txtKWHReading.Enabled = false;
                    cmbHTProtection.Enabled = false;
                    cmbLTProtection.Enabled = false;

                    cmbGrounding.Enabled = false;
                    cmbLightArrester.Enabled = false;
                    cmbLoadtype.Enabled = false;
                    cmbProjecttype.Enabled = false;
                    txtltLine.Enabled = false;
                    txtHtLine.Enabled = false;
                    txtDepreciation.Enabled = false;
                    txtLatitude.Enabled = false;
                    txtLongitude.Enabled = false;
                    cmbMake.Enabled = false;
                    cmbCapacity.Enabled = false;
                    txtManufactureDate.Enabled = false;
                    cmdloctype.Enabled = false;
                    cmbRating.Enabled = false;
                    txtIndentdate.Enabled = false;
                    txtpodate.Enabled = false;

                    RetainImageinPostbackSession(); // Retain Image in Postback Session
                    RetainImageOnPostback();
                    if (!IsPostBack)
                    {
                        Genaral.Load_Combo("SELECT \"TM_ID\",\"TM_NAME\" FROM \"TBLTRANSMAKES\" ORDER BY \"TM_NAME\"", "--Select--", cmbMake);
                        Genaral.Load_Combo("SELECT \"MD_NAME\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'C'", "--Select--", cmbCapacity);
                        Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'PT'", "--Select--", cmbProjecttype);
                        Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'LT'", "--Select--", cmbLoadtype);
                        Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'PLT'", "--Select--", cmbPlatformType);
                        Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'BT'", "--Select--", cmbBreakerType);
                        Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'SR'", "--Select--", cmbRating);
                        Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'LC_TYPE'", "--Select--", cmdloctype);

                        Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" ORDER BY \"CM_CIRCLE_CODE\"", "--Select--", cmbCircle);
                        Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" ORDER BY \"DIV_CODE\"", "--Select--", cmbDivision);
                        Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\", \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" ORDER BY \"SD_SUBDIV_CODE\"", "--Select--", cmbsubdivision);
                        Genaral.Load_Combo("SELECT \"OM_CODE\", \"OM_NAME\" FROM \"TBLOMSECMAST\" ORDER BY \"OM_CODE\"", "--Select--", cmbSection);
                        Genaral.Load_Combo(" SELECT  DISTINCT \"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\" ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeeder);

                        string RecordId = string.Empty;
                        if (Convert.ToString(Request.QueryString["WOId"] ?? "").Length > 0)
                        {
                            RecordId = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["WOId"]));
                            txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));
                            if (txtActiontype.Text == "V")
                            {
                                GetInvoiceDetails(RecordId, txtActiontype.Text);
                                GetInvoiceFromTable(RecordId);
                            }
                            else
                            {
                                // get Invoice Details.
                                GetInvoiceDetails(RecordId);
                            }
                        }
                        if (Convert.ToString(Request.QueryString["Indent"] ?? "").Length > 0)
                        {
                            hdfinvoiceId.Value = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["Indent"]));

                            GetInvoiceDetails(hdfinvoiceId.Value);
                            GetInvoiceFromTable(RecordId);
                        }
                        if ((txtActiontype.Text == "A" || txtActiontype.Text == "R") && objSession.RoleId == "2")
                        {
                            GetInvoiceFromTable(RecordId);

                            cmdReset.Visible = false;
                        }
                        if (txtActiontype.Text == "V")
                        {
                            cmdSave.Enabled = false;
                            cmdReset.Enabled = false;
                        }
                        //WorkFlow / Approval
                        WorkFlowConfig();
                        if (Request.QueryString["InvoiceId"] != null && Convert.ToString(Request.QueryString["IndentId"]) != "")
                        {
                            // get Invoice Details.
                            string InvoiceId = string.Empty;

                            InvoiceId = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["InvoiceId"]));
                            RecordId = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["IndentId"]));
                            GetInvoiceDetails(RecordId);
                            GetInvoiceCompletedDetails(InvoiceId);
                            txtRemarkComments.Enabled = false;
                            txtInvoiceDate.Enabled = false;
                            txtInvoiceno.Enabled = false;
                            cmdSave.Visible = false;
                            cmdReset.Visible = false;
                            Session["BOID"] = "1020";
                            HdnInvoiceid.Value = InvoiceId;
                        }
                        SetControlText();

                    }
                    ApprovalHistoryView.BOID = Convert.ToString(Session["BOID"]);
                    if((HdnInvoiceid.Value ?? "") != "")
                    {
                        ApprovalHistoryView.sRecordId = HdnInvoiceid.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                       MethodBase.GetCurrentMethod().DeclaringType.Name,
                       MethodBase.GetCurrentMethod().Name,
                       ex.Message,
                       ex.StackTrace);
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
                    if (Convert.ToString(Session["WFOId"] ?? "").Length > 0)
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
                    if ((txtActiontype.Text == "A" && objSession.RoleId == "5") && hdfWFDataId.Value != "0")
                    {
                        string Rid = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["WOId"]));
                        GetInvoiceFromTable(Rid);
                    }
                    if (txtActiontype.Text == "V")
                    {
                        dvComments.Style.Add("display", "none");
                    }
                    else if ((txtActiontype.Text == "A" || txtActiontype.Text == "R") && objSession.RoleId == "2")
                    {
                        dvComments.Style.Add("display", "block");

                    }



                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                      MethodBase.GetCurrentMethod().DeclaringType.Name,
                      MethodBase.GetCurrentMethod().Name,
                      ex.Message,
                      ex.StackTrace);
            }
        }
        /// <summary>
        /// get invoice details from table
        /// </summary>
        /// <param name="RecordId"></param>
        public void GetInvoiceFromTable(string RecordId)
        {
            try
            {
                clsPmcInvoice objInvoice = new clsPmcInvoice();
                DataTable dt = new DataTable();
                dt = objInvoice.getinvoiceDetails(RecordId);
                if (dt.Rows.Count > 0)
                {
                    txtInvoiceno.Text = dt.Rows[0]["PMC_INVOICE_NO"].ToString();
                    txtInvoiceDate.Text = dt.Rows[0]["PMC_INVOICE_DATE"].ToString();
                    txtRemarkComments.Text = dt.Rows[0]["PMC_REMARKS"].ToString();
                }
                txtInvoiceno.Enabled = false;
                txtInvoiceDate.Enabled = false;
                txtRemarkComments.Enabled = false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                                  MethodBase.GetCurrentMethod().DeclaringType.Name,
                                  MethodBase.GetCurrentMethod().Name,
                                  ex.Message,
                                  ex.StackTrace);
            }
        }
        /// <summary>
        /// to get invoicedetails
        /// </summary>
        /// <param name="RecordId"></param>
        /// <param name="actionType"></param>
        public void GetInvoiceDetails(string RecordId, string actionType = "")
        {
            try
            {
                clsPmcInvoice objInvoice = new clsPmcInvoice();

                if (((objSession.RoleId == "2" && actionType != "") || actionType == "V") ||
                        (txtActiontype.Text == "A" && objSession.RoleId == "5" && Convert.ToString(Session["WFDataId"]) != "0") ||
                        (objSession.RoleId == "2" && txtActiontype.Text != ""))
                {
                    string InvoiceId = RecordId;
                    objInvoice.IndentId = objInvoice.getIndentID(InvoiceId);
                }
                else
                {
                    objInvoice.IndentId = RecordId;
                }

                objInvoice.GetInvoiceDetails(objInvoice);

                txtwelddate.Text = objInvoice.PmcEnumerationDate;
                txtTcCode.Text = objInvoice.PmcDTrCode;
                cmbMake.SelectedValue = objInvoice.PmcDTrMake;
                cmbCapacity.SelectedValue = objInvoice.PmcCapacity;
                txtTcslno.Text = objInvoice.PmcSlno;
                txtlifespan.Text = objInvoice.PmcLifespan;
                txttcweight.Text = objInvoice.PmcTcWeight;
                txtOiltype.Text = objInvoice.PmcOilType;
                txtManufactureDate.Text = objInvoice.PmcManfdate;
                txtTankCapacity.Text = objInvoice.PmcTankCapacity;
                cmdloctype.SelectedValue = objInvoice.PmcLocType;
                cmbRating.Text = objInvoice.PmcRating;
                hdfTcOilTypeId.Value = objInvoice.PmcOilType_ID;
                txtWarrantyPeriod.Text = objInvoice.TcWarrantyPeriod;
                txtTCOilCapacity.Text = objInvoice.TcOilCap;
                TxtDTrAmount.Text = objInvoice.TcAmt;
                txtIndentno.Text = objInvoice.PmcIndentno;
                txtIndentdate.Text = objInvoice.PmcIndentDate;
                txtPOno.Text = objInvoice.PmcPONo;
                txtpodate.Text = objInvoice.PmcPODate;
                txtDTCCode.Text = objInvoice.PmcDTCCode;
                txtIPDTCCode.Text = objInvoice.PmcDTCCodeIp;
                txtoldDTCCode.Text = objInvoice.PmcTIMSCode;
                txtDTCName.Text = objInvoice.PmcDTCName;
                txtInternalCode.Text = objInvoice.PmcInternalCode;
                txtConnectedKW.Text = objInvoice.PmcConnectedKW;
                txtConnectedHP.Text = objInvoice.PmcConnectedHP;
                txtCommisionDate.Text = objInvoice.PmcDTCCommisiondate;
                txtDtrCommissionDate.Text = objInvoice.PmcDTrCommisiondate;
                txtServiceDate.Text = objInvoice.PmcLastServiceDate;
                cmbPlatformType.SelectedValue = objInvoice.PmcPlatformType;
                cmbBreakerType.SelectedValue = objInvoice.PmcBreakerType;
                cmbDTCMetered.SelectedValue = objInvoice.PmcDTCMetersAvail;
                txtKWHReading.Text = objInvoice.PmcKWHReading;
                cmbHTProtection.SelectedValue = objInvoice.PmcHTProtection;
                cmbLTProtection.SelectedValue = objInvoice.PmcLTProtection;
                cmbGrounding.SelectedValue = objInvoice.PmcGrounding;
                cmbLightArrester.SelectedValue = objInvoice.PmcLightningArrears;
                cmbLoadtype.SelectedValue = objInvoice.PmcLoadType;
                cmbProjecttype.SelectedValue = objInvoice.PmcProjType;
                txtltLine.Text = objInvoice.PmcLTLine;
                txtHtLine.Text = objInvoice.PmcHTLine;
                txtDepreciation.Text = objInvoice.PmcDepreciation;
                txtLatitude.Text = objInvoice.PmcLatitude;
                txtLongitude.Text = objInvoice.PmcLongitude;


                hdffdid.Value = objInvoice.fdId;
                hdfindentoffcode.Value = objInvoice.sOfficeCode;
                string circle = objInvoice.sOfficeCode.Substring(0, 2);
                cmbCircle.SelectedValue = objInvoice.sOfficeCode.Substring(0, 2);
                cmbDivision.SelectedValue = objInvoice.sOfficeCode.Substring(0, 3);
                cmbsubdivision.SelectedValue = objInvoice.sOfficeCode.Substring(0, 4);
                cmbSection.SelectedValue = objInvoice.sOfficeCode.Substring(0, 5);
                cmbFeeder.SelectedValue = objInvoice.sFeederCode;

                txtDTLMSDTCPath.Text = objInvoice.PmcDTCPhoto;
                txtOLDDTCPath.Text = objInvoice.PmcTIMSCodePhoto;
                txtIPDTCPath.Text = objInvoice.PmcDTCCodePhotoIP;
                txtDTCPath.Text = objInvoice.PmcDTCCodePhoto;

                txtSSPlatePath.Text = objInvoice.PmcDtrCodePhoto;
                txtNamePlatePhotoPath.Text = objInvoice.PmcNamePlatePhoto;
                ShowUploadedImages();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                                  MethodBase.GetCurrentMethod().DeclaringType.Name,
                                  MethodBase.GetCurrentMethod().Name,
                                  ex.Message,
                                  ex.StackTrace);
            }
        }
        /// <summary>
        /// method to get invoicecompleted details
        /// </summary>
        /// <param name="InvoiceId"></param>
        public void GetInvoiceCompletedDetails(string InvoiceId)
        {
            try
            {
                clsPmcInvoice objInvoiced = new clsPmcInvoice();
                objInvoiced.PmcId = InvoiceId;
                objInvoiced.GetInvoiceCompletedDetails(objInvoiced);

                txtInvoiceno.Text = objInvoiced.PmcInvoiceno;
                txtInvoiceDate.Text = objInvoiced.PmcInvoiceDate;
                txtRemarkComments.Text = objInvoiced.PmcComments;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                                  MethodBase.GetCurrentMethod().DeclaringType.Name,
                                  MethodBase.GetCurrentMethod().Name,
                                  ex.Message,
                                  ex.StackTrace);
            }
        }

        /// <summary>
        /// method to show uploaded images
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

                sFTPLink = ConfigurationManager.AppSettings["VirtualDirectoryPathPMCInvoice"].ToString();
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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                       MethodBase.GetCurrentMethod().DeclaringType.Name,
                       MethodBase.GetCurrentMethod().Name,
                       ex.Message,
                       ex.StackTrace);
            }
        }
        /// save methos to declare failure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdSave_Click(object sender, EventArgs e)
        {
            clsPmcInvoice Obj = new clsPmcInvoice();
            string[] Arr = new string[2];
            try
            {
                cmdReset.Enabled = false;

                //Check AccessRights
                bool bAccResult = true;

                if (cmdSave.Text == "Approve")
                {
                    bAccResult = CheckAccessRights(Constants.CheckAccessRights.CheckAccessRightsCreate); // 2
                }
                if (bAccResult == false)
                {
                    return;
                }

                if (ValidateForm() == true)
                {
                    Obj.sCrBy = objSession.UserId;
                    Obj.fdId = hdffdid.Value;

                    //invoice variabls
                    Obj.InvoiceNo = txtInvoiceno.Text;
                    Obj.InvoiceDate = txtInvoiceDate.Text;
                    Obj.sRemark = txtRemarkComments.Text.Trim();

                    //Tc Master variabls
                    Obj.sTcCode = txtTcCode.Text;
                    Obj.PmcSlno = txtTcslno.Text;
                    Obj.PmcDTrMake = cmbMake.SelectedValue;
                    Obj.PmcCapacity = cmbCapacity.SelectedValue;
                    Obj.PmcManfdate = txtManufactureDate.Text;
                    Obj.PmcPurchasedate = txtpodate.Text;
                    Obj.PmcLifespan = txtlifespan.Text;
                    Obj.PmcSupplierID = "0";
                    Obj.sOfficeCode = clsStoreOffice.Getofficecode(objSession.OfficeCode);
                    Obj.StoreId = objSession.OfficeCode;
                    Obj.PmcPONo = txtPOno.Text;
                    Obj.PmcRating = cmbRating.SelectedValue;

                    Obj.TcWarrantyPeriod = txtWarrantyPeriod.Text;
                    Obj.TcOilCap = txtTCOilCapacity.Text;
                    Obj.TcAmt = TxtDTrAmount.Text;

                    if (cmbStarRated.SelectedValue != "")
                    {
                        Obj.sStarRate = cmbStarRated.SelectedValue;
                    }
                    else
                    {
                        Obj.sStarRate = "0";
                    }
                    Obj.OilCapacity = txtTankCapacity.Text;
                    Obj.PmcTcWeight = txttcweight.Text;
                    Obj.tcCore = "0";
                    Obj.sTCType = "0";
                    Obj.PmcOilType = hdfTcOilTypeId.Value; //oiltype Id.
                    Obj.tcStatus = "1";

                    //DTC variabls
                    Obj.PmcDTCCode = txtDTCCode.Text;
                    Obj.PmcDTCName = txtDTCName.Text;
                    Obj.PmcConnectedKW = txtConnectedKW.Text;
                    Obj.PmcConnectedHP = txtConnectedHP.Text;
                    Obj.PmcKWHReading = txtKWHReading.Text;
                    Obj.sConnectionDate = "null";
                    Obj.sFeederCode = cmbFeeder.SelectedValue;
                    Obj.sProjecttype = cmbProjecttype.SelectedValue;
                    Obj.PmcTIMSCode = txtoldDTCCode.Text;
                    Obj.IndentOffCode = hdfindentoffcode.Value;
                    Obj.PmcDTCCommisiondate = txtCommisionDate.Text;
                    Obj.PmcDTrCommisiondate = txtDtrCommissionDate.Text;
                    Obj.PmcLastServiceDate = txtServiceDate.Text;
                    Obj.sDTCMeters = cmbDTCMetered.SelectedValue;
                    Obj.PmcHTProtection = cmbHTProtection.SelectedValue;
                    Obj.PmcLTProtection = cmbLTProtection.SelectedValue;
                    Obj.PmcGrounding = cmbGrounding.SelectedValue;
                    Obj.sArresters = cmbLightArrester.SelectedValue;
                    Obj.PmcLoadType = cmbLoadtype.SelectedValue;
                    Obj.sProjecttype = cmbProjecttype.SelectedValue;
                    Obj.PmcBreakerType = cmbBreakerType.SelectedValue;
                    Obj.PmcDepreciation = txtDepreciation.Text;
                    Obj.PmcLatitude = txtLatitude.Text;
                    Obj.PmcLongitude = txtLongitude.Text;
                    Obj.PmcHTLine = txtHtLine.Text;
                    Obj.PmcLTLine = txtltLine.Text;
                    Obj.PmcPlatformType = cmbPlatformType.SelectedValue;
                    Obj.PmcLoadType = cmbLoadtype.SelectedValue;
                    Obj.PmcLocType = cmdloctype.SelectedValue;
                    Obj.PmcInternalCode = txtInternalCode.Text;

                    if (Obj.PmcInternalCode == null || Obj.PmcInternalCode == "")
                    {
                        Obj.PmcInternalCode = "0";
                    }
                    if (Obj.PmcLocType == null || Obj.PmcLocType == "")
                    {
                        Obj.PmcLocType = "0";
                    }
                    if (Obj.PmcPlatformType == null || Obj.PmcPlatformType == "")
                    {
                        Obj.PmcPlatformType = "0";
                    }
                    if (Obj.PmcLoadType == null || Obj.PmcLoadType == "")
                    {
                        Obj.PmcLoadType = "0";
                    }
                    if (Obj.sDTCMeters == null || Obj.sDTCMeters == "")
                    {
                        Obj.sDTCMeters = "0";
                    }
                    if (Obj.PmcHTProtection == null || Obj.PmcHTProtection == "")
                    {
                        Obj.PmcHTProtection = "0";
                    }
                    if (Obj.PmcLTProtection == null || Obj.PmcLTProtection == "")
                    {
                        Obj.PmcLTProtection = "0";
                    }
                    if (Obj.PmcGrounding == null || Obj.PmcGrounding == "")
                    {
                        Obj.PmcGrounding = "0";
                    }
                    if (Obj.sArresters == null || Obj.sArresters == "")
                    {
                        Obj.sArresters = "0";
                    }
                    if (Obj.PmcLoadType == null || Obj.PmcLoadType == "")
                    {
                        Obj.PmcLoadType = "0";
                    }
                    if (Obj.sProjecttype == null || Obj.sProjecttype == "")
                    {
                        Obj.sProjecttype = "0";
                    }
                    if (Obj.PmcBreakerType == null || Obj.PmcBreakerType == "")
                    {
                        Obj.PmcBreakerType = "0";
                    }
                    if (Obj.PmcDepreciation == null || Obj.PmcDepreciation == "")
                    {
                        Obj.PmcDepreciation = "0";
                    }
                    Obj.sRoleID = objSession.RoleId;

                    Obj.hdfWFOId = hdfWFOId.Value; // assing the hidden field from the qrey string 

                    if (((txtActiontype.Text == "A" || txtActiontype.Text == "R") && objSession.RoleId == "2")
                        || (txtActiontype.Text == "A" && objSession.RoleId == "5") && (hdfWFDataId.Value != "0"))
                    {
                        ApproveRejectAction(Obj);
                        return;
                    }

                    //if ()
                    //{
                    //    ApproveRejectAction(Obj);
                    //    return;
                    //}
                    //Workflow
                    WorkFlowObjects(Obj);
                    Arr = Obj.SavePMCInvoiceDetails(Obj);

                    if (Arr[1].ToString() == "0")
                    {
                        ShowMsgBox(Arr[0].ToString());
                        cmdSave.Enabled = false;
                        return;
                    }
                    else
                    {
                        ShowMsgBox(Arr[0]);
                        cmdSave.Enabled = true;
                    }

                }
                cmdReset.Enabled = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                       MethodBase.GetCurrentMethod().DeclaringType.Name,
                       MethodBase.GetCurrentMethod().Name,
                       ex.Message,
                       ex.StackTrace);
            }
        }
        /// <summary>
        /// method to validate form
        /// </summary>
        /// <returns></returns>
        public bool ValidateForm()
        {
            bool bValidate = false;
            string sResultPO = string.Empty;
            try
            {
                if ((txtInvoiceno.Text ?? "").Length == 0)
                {
                    txtInvoiceno.Focus();
                    ShowMsgBox("Please Enter Invoice No");
                    cmdSave.Enabled = true;
                    return bValidate;
                }
                // added by santhosh on 09-10-2023.
                if ((txtInvoiceno.Text ?? "").Length > 0)
                {
                    var count = 0;

                    foreach (var item in txtInvoiceno.Text)
                    {
                        if (item != '0' && item != '/' && item != '-' && item != '_')
                        {
                            count++;
                        }
                    }

                    if (count == 0)
                    {
                        txtInvoiceno.Focus();
                        ShowMsgBox("Please Enter Valid Invoice No.");
                        txtInvoiceno.Text = "";
                        cmdSave.Enabled = true;
                        return bValidate;
                    }
                }

                if ((txtInvoiceDate.Text ?? "").Length == 0)
                {
                    txtIndentno.Focus();
                    ShowMsgBox("Please select Invoice Date");
                    cmdSave.Enabled = true;
                    return bValidate;
                }
                if (txtRemarkComments.Text.Trim() == "")
                {
                    txtRemarkComments.Focus();
                    ShowMsgBox("Enter Comments/Remarks");
                    cmdSave.Enabled = true;
                    return bValidate;
                }
                bValidate = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                       MethodBase.GetCurrentMethod().DeclaringType.Name,
                       MethodBase.GetCurrentMethod().Name,
                       ex.Message,
                       ex.StackTrace);
            }
            return bValidate;
        }

        /// <summary>
        /// to get the basic parametere details for save
        /// </summary>
        /// <param name="objFailure"></param>
        public void WorkFlowObjects(clsPmcInvoice Obj)
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

                Obj.sFormName = Convert.ToString(MethodBase.GetCurrentMethod().DeclaringType.Name);
                Obj.sClientIP = sClientIP;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                       MethodBase.GetCurrentMethod().DeclaringType.Name,
                       MethodBase.GetCurrentMethod().Name,
                       ex.Message,
                       ex.StackTrace);
            }
        }
        /// <summary>
        /// method to save the details
        /// </summary>
        public void ApproveRejectAction(clsPmcInvoice Obj)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                if (txtComment.Text.Trim() == "" && objSession.RoleId == "2")
                {
                    ShowMsgBox("Enter Comments/Remarks");
                    txtComment.Focus();
                    return;
                }

                objApproval.sCrby = objSession.UserId;
                objApproval.sOfficeCode = clsStoreOffice.Getofficecode(objSession.OfficeCode);
                objApproval.sApproveComments = txtComment.Text.Trim();
                objApproval.sWFObjectId = hdfWFOId.Value;
                objApproval.sWFAutoId = hdfWFOAutoId.Value;
                objApproval.sRoleId = Obj.sRoleID;

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
                    bResult = objApproval.ModifyApproveWFRequest_Latest(objApproval);
                }
                else
                {
                    bResult = objApproval.ApproveWFRequest_Latest(objApproval);

                    if (objApproval.sApproveStatus == "3")
                    {
                        Obj.ApproveStatus = objApproval.sApproveStatus;
                        Obj.Updatestatusafterreject(Obj);
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
                    else
                    {
                        ShowMsgBox("Something went wrong PGRS Docket no not updated");
                        cmdSave.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(
                       MethodBase.GetCurrentMethod().DeclaringType.Name,
                       MethodBase.GetCurrentMethod().Name,
                       ex.Message,
                       ex.StackTrace);
                throw ex;
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
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY
                clsApproval objApproval = new clsApproval();
                objApproval.sFormName = MethodBase.GetCurrentMethod().DeclaringType.Name;
                objApproval.sRoleId = objSession.RoleId; // for Admin there is no role.
                objApproval.sAccessType = Constants.CheckAccessRights.CheckAccessRightsAll + "," + sAccessType; //  "1"
                bool bResult = objApproval.CheckAccessRights(objApproval);
                if (bResult == false)
                {
                    Response.Redirect("~/UserRestrict.aspx", false);
                }
                return bResult;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
                return false;

            }
        }
        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strQry = string.Empty;
            string stroffCode = string.Empty;
            try
            {
                objSession = (clsSession)Session["clsSession"];

                if (cmbCircle.SelectedIndex > 0)
                {
                    if (objSession.RoleId == "8" || objSession.sRoleType == "3")
                    {
                        Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" " +
                            " WHERE  \"DIV_CICLE_CODE\"='" + cmbCircle.SelectedValue +
                            "' ORDER BY \"DIV_CODE\"", "--Select--", cmbDivision);
                    }
                    else
                    {
                        Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" " +
                            " WHERE \"DIV_CODE\"='" + objSession.OfficeCode.Substring(0, Division_code) +
                            "' AND \"DIV_CICLE_CODE\"='" + cmbCircle.SelectedValue +
                            "' ORDER BY \"DIV_CODE\"", "--Select--", cmbDivision);
                    }
                }
                else
                {
                    cmbDivision.Items.Clear();
                    cmbsubdivision.Items.Clear();
                    cmbSection.Items.Clear();
                    cmbFeeder.Items.Clear();
                }
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
                    Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\", \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" " +
                        " WHERE \"SD_SUBDIV_CODE\"='" + objSession.OfficeCode.Substring(0, SubDiv_code) +
                        "' AND \"SD_DIV_CODE\"='" + cmbDivision.SelectedValue +
                        "' ORDER BY \"SD_SUBDIV_CODE\"", "--Select--", cmbsubdivision);
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
                    Genaral.Load_Combo("SELECT \"OM_CODE\", \"OM_NAME\" FROM \"TBLOMSECMAST\" " +
                        " WHERE \"OM_CODE\"='" + objSession.OfficeCode.Substring(0, Section_code) +
                        "' AND \"OM_SUBDIV_CODE\" = '" + cmbsubdivision.SelectedValue +
                        "' ORDER BY \"OM_CODE\"", "--Select--", cmbSection);
                    if (stroffCode.Length >= 4)
                    {
                        stroffCode = stroffCode.Substring(0, Section_code);
                        cmbSection.Items.FindByValue(stroffCode).Selected = true;
                        cmbSection.Enabled = false;
                        stroffCode = stroffCode1;
                        strQry = " SELECT \"FD_FEEDER_CODE\",\"FD_FEEDER_CODE\" || '-' || \"FD_FEEDER_NAME\" ";
                        strQry += " FROM \"TBLFEEDERMAST\", \"TBLFEEDEROFFCODE\" WHERE  \"FD_FEEDER_ID\" = \"FDO_FEEDER_ID\" AND ";
                        strQry += " cast(\"FDO_OFFICE_CODE\" as text) LIKE '" + cmbsubdivision.SelectedValue + "%' ORDER BY \"FD_FEEDER_CODE\" ";
                        Genaral.Load_Combo(strQry, "--Select--", cmbFeeder);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }
        protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDivision.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\", \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" " +
                        " WHERE \"SD_DIV_CODE\"='" + cmbDivision.SelectedValue +
                        "' ORDER BY \"SD_SUBDIV_CODE\"", "--Select--", cmbsubdivision);
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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }
        protected void cmbsubdivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbsubdivision.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"OM_CODE\", \"OM_NAME\" FROM \"TBLOMSECMAST\" " +
                        " WHERE \"OM_SUBDIV_CODE\" = '" + cmbsubdivision.SelectedValue +
                        "' ORDER BY \"OM_CODE\"", "--Select--", cmbSection);
                }
                else
                {
                    cmbSection.Items.Clear();
                    cmbFeeder.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }
        protected void cmbSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strQry = string.Empty;
            try
            {
                if (cmbsubdivision.SelectedIndex > 0)
                {
                    strQry = " SELECT \"FD_FEEDER_CODE\",\"FD_FEEDER_CODE\" || '-' || \"FD_FEEDER_NAME\" ";
                    strQry += " FROM \"TBLFEEDERMAST\", \"TBLFEEDEROFFCODE\" WHERE  \"FD_FEEDER_ID\" = \"FDO_FEEDER_ID\" AND";
                    strQry += " cast(\"FDO_OFFICE_CODE\" as text) LIKE '" + cmbSection.SelectedValue + "%' ORDER BY \"FD_FEEDER_CODE\" ";
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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                     MethodBase.GetCurrentMethod().DeclaringType.Name,
                     MethodBase.GetCurrentMethod().Name,
                     ex.Message,
                     ex.StackTrace);
            }
        }

        // UnUsed Method

        //protected void cmbFeeder_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string sClientIP = string.Empty;
        //    string ip = string.Empty;
        //    try
        //    {
        //        if (txtTcCode.Text == null || txtTcCode.Text == "")
        //        {
        //            if (cmbFeeder.SelectedIndex > 0)
        //            {
        //                clsFieldEnumeration ObjField = new clsFieldEnumeration();
        //                ObjField.sFeederCode = cmbFeeder.SelectedValue;
        //                ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
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
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(
        //                MethodBase.GetCurrentMethod().DeclaringType.Name,
        //                MethodBase.GetCurrentMethod().Name,
        //                ex.Message,
        //                ex.StackTrace);
        //    }
        //}
        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtInvoiceno.Text = string.Empty;
                txtInvoiceDate.Text = string.Empty;
                txtComment.Text = string.Empty;
                txtRemarkComments.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                        MethodBase.GetCurrentMethod().DeclaringType.Name,
                        MethodBase.GetCurrentMethod().Name,
                        ex.Message,
                        ex.StackTrace);
            }
        }
        protected void cmdClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["InvoiceId"] != null && Convert.ToString(Request.QueryString["IndentId"]) != "")
                {
                    Response.Redirect("PmcInvoiceView.aspx", false);
                }
                else
                {
                    Response.Redirect("/Approval/ApprovalInbox.aspx", false);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }/// <summary>
         /// method to retain images
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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }
        /// <summary>
        /// method to retainimage on postback
        /// </summary>
        public void RetainImageOnPostback()
        {
            string sDirectory = string.Empty;
            string sNamePlateFileName = string.Empty;
            string sSSPlateFileName = string.Empty;
            string sOldCodeFileName = string.Empty;
            string sIPEnumCodeFileName = string.Empty;
            string sDTLMSCodeFileName = string.Empty;
            string sDTCFileName = string.Empty;
            string sInfosysCodeFileName = string.Empty;
            try
            {
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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                       MethodBase.GetCurrentMethod().DeclaringType.Name,
                       MethodBase.GetCurrentMethod().Name,
                       ex.Message,
                       ex.StackTrace);
            }
        }

        public void SetControlText()
        {
            try
            {
                //if (txtActiontype.Text == "")
                //{
                //    txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));
                //}

                if ((txtActiontype.Text == "A" && objSession.RoleId == "5") && hdfWFDataId.Value != "0")
                {
                    cmdSave.Text = "Approve";
                    cmdReset.Visible = false;
                }
                if (txtActiontype.Text == "R")
                {
                    cmdSave.Text = "Reject";
                    cmdReset.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                       MethodBase.GetCurrentMethod().DeclaringType.Name,
                       MethodBase.GetCurrentMethod().Name,
                       ex.Message,
                       ex.StackTrace);
            }
        }

    }
}

