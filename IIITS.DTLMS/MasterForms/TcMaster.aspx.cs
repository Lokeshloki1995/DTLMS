using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Configuration;
using System.Net;
using System.IO;

namespace IIITS.DTLMS.MasterForms
{
    public partial class TcMaster : System.Web.UI.Page
    {
        string strFormCode = "TcMaster";
        clsTcMaster objTcMaster = new clsTcMaster();
        clsSession objSession;
        string sTcXmlData = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                Form.DefaultButton = cmdSave.UniqueID;
                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;
                txtManufactureDate.Attributes.Add("readonly", "readonly");
                txtPurchaseDate.Attributes.Add("readonly", "readonly");
                //txtWarrentyPeriod.Attributes.Add("readonly", "readonly");
                txtLastServiceDate.Attributes.Add("readonly", "readonly");

                CalendarExtender3.EndDate = System.DateTime.Now;
                PurchaseCalender.EndDate = System.DateTime.Now;
                ManufactureCalender.EndDate = System.DateTime.Now;

                if (!IsPostBack)
                {

                    if (objSession.OfficeCode == "" || objSession.OfficeCode == null)
                    {
                        cmbCapacity.Enabled = true;
                        //cmbCapacity.Attributes.Add("disabled", "disabled");
                    }
                    ManufactureCalender.EndDate = System.DateTime.Now;
                    PurchaseCalender.EndDate = System.DateTime.Now;

                    if (Request.QueryString["TCId"] != null && Request.QueryString["TCId"].ToString() != "")
                    {
                        txtTcID.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TCId"]));
                        Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" from \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='TCL' ORDER BY \"MD_ORDER_BY\" ", "-Select-", cmbTcLocation);
                    }

                    LoadComboField();

                    if (txtTcID.Text != "")
                    {

                        Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" from \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='TCL' ORDER BY \"MD_ORDER_BY\" ", "-Select-", cmbTcLocation);
                        GetTcMAsterDeatils(txtTcID.Text);
                        hdfTcCode.Value = txtTcCode.Text;
                        //if (cmbRating.SelectedValue == "1")
                        //{
                        cmbRating_SelectedIndexChanged(sender, e);
                        //    cmbStarRated.SelectedValue = hdfStarRate.Value;
                        //}



                        objTcMaster.sTcId = Convert.ToString(txtTcID.Text);
                        objTcMaster.GetTCDetails(objTcMaster);
                        hdfCapacity.Value = objTcMaster.sTcCapacity;

                        sTcXmlData = objTcMaster.SaveXmlData(objTcMaster);
                        ViewState["sTcXmlData"] = sTcXmlData;

                        cmbTcLocation_SelectedIndexChanged(sender, e);
                    }
                    txtPurchaseDate.Attributes.Add("onblur", "return ValidateDate(" + txtPurchaseDate.ClientID + ");");
                    txtManufactureDate.Attributes.Add("onblur", "return ValidateDate(" + txtManufactureDate.ClientID + ");");
                    txtLastServiceDate.Attributes.Add("onblur", "return ValidateDate(" + txtLastServiceDate.ClientID + ");");
                    if (cmbMake.SelectedValue != "1")
                    {
                        //cmbMake_SelectedIndexChanged(sender, e);
                        txtManufactureDate.Text = objTcMaster.sManufacDate;
                        Div1.Style.Add("display", "block");


                    }
                }
                string strQry = string.Empty;
                strQry = "Title=Search and Select Transformer Centre Details&";
                strQry += "Query=select \"DT_CODE\",\"DT_NAME\" FROM \"TBLDTCMAST\" where \"DT_OM_SLNO\" like '" + objSession.OfficeCode + "%' and  {0} like %{1}% order by \"DT_NAME\"&";
                strQry += "DBColName=\"DT_CODE\"~\"DT_NAME\"&";
                strQry += "ColDisplayName=Transformer Centre Code~Transformer Centre Name&";
                strQry = strQry.Replace("'", @"\'");
                btnDtcSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtDtcCode.ClientID + "&btn=" + btnDtcSearch.ClientID + "',520,520," + txtDtcCode.ClientID + ")");

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        public void LoadComboField()
        {
            try
            {
                string strQry = string.Empty;


                strQry = "SELECT \"TM_ID\",\"TM_NAME\" FROM \"TBLTRANSMAKES\" ORDER BY \"TM_NAME\"";

                Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" from \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='TCL' and \"MD_ID\" <> '2' ORDER BY \"MD_ORDER_BY\" ", "-Select-", cmbTcLocation);
                Genaral.Load_Combo(strQry, "-Select-", cmbMake);
                Genaral.Load_Combo("SELECT \"MD_NAME\",\"MD_NAME\" from \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='C' ORDER BY \"MD_ORDER_BY\"", "-Select-", cmbCapacity);
                Genaral.Load_Combo("select \"TS_ID\",\"TS_NAME\"  FROM \"TBLTRANSSUPPLIER\" WHERE \"TS_STATUS\"='A' ORDER BY \"TS_NAME\"", "-Select-", cmbSupplier);
                Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" from \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='COTC' ORDER BY \"MD_ORDER_BY\"", "-Select-", cmbConditionOfTC);

                Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'SR' ORDER BY \"MD_ORDER_BY\"", "--Select--", cmbRating);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetTcMAsterDeatils(string strId)
        {
            try
            {
                int iCommission_Year;
                clsTcMaster objTcMaster = new clsTcMaster();
                objTcMaster.sTcId = Convert.ToString(strId);
                objTcMaster.GetTCDetails(objTcMaster);
                txtTcID.Text = objTcMaster.sTcId;
                txtTcCode.Text = objTcMaster.sTcCode;
                txtSerialNo.Text = objTcMaster.sTcSlNo;
                cmbMake.SelectedValue = objTcMaster.sTcMakeId;
                cmbCapacity.SelectedValue = objTcMaster.sTcCapacity;
                txtManufactureDate.Text = objTcMaster.sManufacDate;
                txtPurchaseDate.Text = objTcMaster.sAllotementDate;
                //uncommented by sandeep bcz DTr Life Span value miss match while binding to text field
                txtTcLifeSpan.Text = objTcMaster.sTcLifeSpan;
                cmbSupplier.SelectedValue = objTcMaster.sSupplierId;
                if (objTcMaster.sPoNo == "")
                {
                    txtPoNo.Text = "Data Not Available";
                }
                else
                {
                    txtPoNo.Text = objTcMaster.sPoNo;
                    txtPoNo.Enabled = false;
                }

                if (objTcMaster.sDTRcommissionYear == "" || objTcMaster.sDTRcommissionYear == null)
                {
                    iCommission_Year = 25;
                }
                else
                {
                    int Current_year = Convert.ToInt16(DateTime.Now.Year.ToString());
                    iCommission_Year = Convert.ToInt16(objTcMaster.sDTRcommissionYear.Split('-').GetValue(2));
                    iCommission_Year = 25 - (Current_year - iCommission_Year);
                }
                //commented by sandeep bcz DTr Life Span value miss match while binding to text field
                //txtTcLifeSpan.Text = Convert.ToString(iCommission_Year);

                txtPrice.Text = objTcMaster.sPrice;
                txtWarrentyPeriod.Text = objTcMaster.sWarrentyPeriod;
                txtLastServiceDate.Text = objTcMaster.sLastServiceDate;
                txtLastFailureDate.Text = objTcMaster.sLastFaildate;
                txtLastRepairCost.Text = objTcMaster.sLastRepaircost;
                txtLastRepairCount.Text = objTcMaster.sLastRepaircount;
                txtLastrFailureType.Text = objTcMaster.sLastFailuretype;
                cmbTcLocation.SelectedValue = objTcMaster.sCurrentLocation;
                cmbConditionOfTC.SelectedValue = objTcMaster.sConditiontc;
                cmbCooling.SelectedValue = objTcMaster.sCooling;
                cmbCoreType.SelectedValue = objTcMaster.sCore;
                cmbTapCharger.SelectedValue = objTcMaster.sTapeCharger;
                cmbTCtype.SelectedValue = objTcMaster.sType;
                hdfTcLocation.Value = objTcMaster.sLocationId;
                txtTcCode.Enabled = false;
                txtLastrFailureType.Enabled = false;
                txtLastRepairCount.Enabled = false;
                txtLastRepairCost.Enabled = false;
                txtLastFailureDate.Enabled = false;
                cmbRating.SelectedValue = objTcMaster.sRating;
                hdfStarRate.Value = objTcMaster.sStarRate;
                txtRAPDRP.Text = objTcMaster.sInfosysId;
                if (objTcMaster.sComponentId == "")
                {
                    txtComponentID.Text = objTcMaster.sInfosysId; // said by ramesh sir on 05-06-2018 discussion
                }
                else
                {
                    txtComponentID.Text = objTcMaster.sComponentId;
                }

                txtOriginalCost.Text = objTcMaster.sOriginalCost;
                if (objTcMaster.sInsurance == "")
                {
                    txtInsurance.Text = "No Insurance";
                }
                else
                {
                    txtInsurance.Text = objTcMaster.sInsurance;
                }

                txtdepreciation.Text = objTcMaster.sDepreciation;
                //txtRAPDRP.Text = objTcMaster.sInfosysId;
                //txtComponentID.Text = objTcMaster.sComponentId;
                //txtOriginalCost.Text = objTcMaster.sOriginalCost;
                //txtInsurance.Text = objTcMaster.sInsurance;
                //txtdepreciation.Text = objTcMaster.sDepreciation;
                txtFailCount.Text = objTcMaster.sFailCount;

                txtWeight.Text = objTcMaster.sWeight;
                txtOilCapacity.Text = objTcMaster.sOilCapacity;
                if (objTcMaster.sOilType != "" || objTcMaster.sOilType != null)
                {
                    cmbOilType.SelectedValue = objTcMaster.sOilType;
                }
                cmbTcLocation.Enabled = false;
                //cmbCapacity.Enabled = false;
                if (cmbTcLocation.SelectedValue == "2")
                {
                    cmbTcLocation.Enabled = false;
                    txtDtcCode.Enabled = false;
                }

                cmdSave.Text = "Update";
                cmdReset.Enabled = false;
                //  objTcMaster.sTcCode = txtTcCode.Text;
                if (objTcMaster.sDINo == ""  || objTcMaster.sDINo == null)
                {
                    if (objTcMaster.sTcCode.Length>7)
                    {
                        objTcMaster.GetImagePathForNew(objTcMaster);
                    }
                    else
                    {
                        objTcMaster.GetImagePath(objTcMaster);
                    }
                    hdfDTRImagePath.Value = objTcMaster.sDTrImagePath;
                    hdfDTRNameplateImagePath.Value = objTcMaster.sNamePlateImagePath;
                }
                ShowUploadedImages(objTcMaster.sDINo, objTcMaster.sTcCode);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }
        public void ShowUploadedImages(string Alt_no, string Tc_code)
        {
            try
            {
                clsCommon objComm = new clsCommon();

                //FTP Parameter
                string sFTPLink = string.Empty;
                string sFTPUserName = string.Empty;
                string sFTPPassword = string.Empty;

                string sFTPLinkpath = string.Empty;

                // To bind the Images from Ftp Path to Image Control
                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);
                System.Data.DataTable dt = objComm.GetAppSettings();
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
                if (txtTcCode.Text.Length > 7)
                {
                    sFTPLink = ConfigurationSettings.AppSettings["VirtualDirectoryPathPMCInvoice"].ToString();
                }
                else
                {
                    sFTPLink = ConfigurationSettings.AppSettings["VirtualDirectoryPath"].ToString();
                }
                //clsFtp objFtp1 = new clsFtp(sFTPLink, sFTPUserName, sFTPPassword);
                clsSFTP objFtp = new clsSFTP(SFTPPath, sFTPUserName, sFTPPassword);
                if (Alt_no != "" && Alt_no != null && Tc_code != "")
                {
                    string sMainFolder = "TcAllotment";
                    string sNameplate = "NAMEPLATE";
                    string sSSplate = "SSPLATE";
                    bool IsFileExiest = false;
                    bool IsExists = false;
                    string FileName = string.Empty;

                    IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolder + "/" + Tc_code);
                    if (IsExists == true)
                    {
                        FileName = objFtp.GetFileName(SFTPmainfolder + sMainFolder + "/" + Tc_code + "/" + sNameplate);
                        if (FileName != "" && FileName != null)
                        {
                            hdfDTRImagePath.Value = SFTPmainfolder + sMainFolder + "/" + Tc_code + "/" + sNameplate + "/" + FileName;
                        }
                        FileName = objFtp.GetFileName(SFTPmainfolder + sMainFolder + "/" + Tc_code + "/" + sSSplate);
                        if (FileName != "" && FileName != null)
                        {
                            hdfDTRNameplateImagePath.Value = SFTPmainfolder + sMainFolder + "/" + Tc_code + "/" + sSSplate + "/" + FileName;

                        }
                        sFTPLink = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["InwardedTCPath"]);
                    }
                }
                //sFTPLinkpath="http://192.168.6.18:1133/SFTP_FILES/";

                if (hdfDTRImagePath.Value != "")
                {
                    dvDTrCode.Style.Add("display", "block");
                    //imgDTrCode.ImageUrl = "ftp://" + sFTPUserName + ":" + sFTPPassword + "@" + sFTPLink.Remove(0, 6) + hdfDTRImagePath.Value;
                    //imgDTrCode.ImageUrl = sFTPLink + SFTPmainfolder + hdfDTRImagePath.Value;


                    imgDTrCode.ImageUrl = sFTPLink + hdfDTRImagePath.Value;
                    // imgDTrCode.ImageUrl = sFTPLinkpath + hdfDTRImagePath.Value;

                }
                if (hdfDTRNameplateImagePath.Value != "")
                {
                    dvNamePlate.Style.Add("display", "block");
                    //imgDTrCode.ImageUrl = "ftp://" + sFTPUserName + ":" + sFTPPassword + "@" + sFTPLink.Remove(0, 6) + hdfDTRImagePath.Value;
                    //imgNamePlate.ImageUrl = sFTPLink + SFTPmainfolder + hdfDTRNameplateImagePath.Value;


                    imgNamePlate.ImageUrl = sFTPLink + hdfDTRNameplateImagePath.Value;
                    //  imgNamePlate.ImageUrl = sFTPLinkpath + hdfDTRNameplateImagePath.Value;

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }
        public string GetFileName(string sFTPLink, string sFTPUserName, string sFTPPassword)
        {
            string fileName = string.Empty;
            try
            {
                WebRequest request = (WebRequest)WebRequest.Create(sFTPLink);
                request.Credentials = new NetworkCredential(sFTPUserName, sFTPPassword);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                try
                {
                    using (request.GetResponse())
                    {
                        StreamReader streamReader = new StreamReader(request.GetResponse().GetResponseStream());
                        fileName = streamReader.ReadLine();
                        streamReader.Close();
                        return fileName;
                    }
                }
                catch (WebException)
                {
                    return fileName;
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return fileName;
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

        bool ValidateForm()
        {
            bool bValidate = false;

            if (cmbRating.SelectedIndex == 0)
            {
                cmbRating.Focus();
                ShowMsgBox("Select Star Rating");
                return false;

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
                ShowMsgBox("Please Select the TC Make");
                return false;
            }
            if (cmbCapacity.SelectedIndex == 0)
            {
                cmbCapacity.Focus();
                ShowMsgBox("Select TC Capacity");
                return false;
            }
            if (txtTcLifeSpan.Text.Trim().Length > 0)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtTcLifeSpan.Text, "^(\\d{1,6})?(\\.\\d{1,2})?$"))
                {
                    ShowMsgBox("Enter valid Life Span");
                    txtTcLifeSpan.Focus();
                    return false;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(txtTcLifeSpan.Text, "[-+]?[0-9]{0,5}\\.?[0-9]{0,2}"))
                {
                    ShowMsgBox("Enter valid Life Span");
                    txtTcLifeSpan.Focus();
                    return false;
                }
                if (txtTcLifeSpan.Text.Contains("-"))
                {
                    ShowMsgBox("Enter valid Life Span");
                    txtTcLifeSpan.Focus();
                    return false;
                }
            }

            if (txtOilCapacity.Text.Length == 0)
            {
                txtOilCapacity.Focus();
                ShowMsgBox("Enter Oil Capacity");
                return false;
            }

            if (txtWeight.Text.Length == 0)
            {
                txtWeight.Focus();
                ShowMsgBox("Enter Weight of DTr");
                return false;
            }


            if (txtPrice.Text.Trim() != "")
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtPrice.Text, "^(\\d{1,6})?(\\.\\d{1,2})?$"))
                {
                    txtPrice.Focus();
                    ShowMsgBox("Enter valid price (eg:111111.00)");
                    return false;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(txtPrice.Text, "[-+]?[0-9]{0,5}\\.?[0-9]{0,2}"))
                {
                    txtPrice.Focus();
                    ShowMsgBox("Enter valid price (eg:111111.00)");
                    return false;
                }
            }

            if (txtLastServiceDate.Text.Trim() != "")
            {
                string sResult = Genaral.DateComparision(txtLastServiceDate.Text, "", true, false);
                if (sResult == "1")
                {
                    txtLastServiceDate.Focus();
                    ShowMsgBox("Last Service Date should be Less than Current Date");
                    return bValidate;
                }
                sResult = Genaral.DateComparision(txtLastServiceDate.Text, txtManufactureDate.Text, false, false);
                if (sResult == "2")
                {
                    ShowMsgBox("Last Service Date should be Greater than Manufacturing Date");
                    return bValidate;
                }
            }
            if (cmbTcLocation.SelectedIndex == 0)
            {
                cmbTcLocation.Focus();
                ShowMsgBox("Select TC Current Location");
                return false;
            }
            if (txtPurchaseDate.Text.Trim() != "")
            {
                //string sResult = Genaral.DateComparision(txtPurchaseDate.Text, txtManufactureDate.Text, false, false);
                //if (sResult == "2")
                //{
                //    ShowMsgBox("Purchasing Date should be Greater than Manufacturing Date");
                //    return bValidate;
                //}
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
            if (cmbTcLocation.SelectedValue == "2")
            {
                if (txtDtcCode.Text.Trim() == "")
                {
                    txtDtcCode.Focus();
                    ShowMsgBox("Enter DTC Code ");
                    return false;
                }
            }
            if (cmbMake.SelectedValue != "1")
            {
                if (txtManufactureDate.Text.Trim().Length == 0)
                {
                    txtManufactureDate.Focus();
                    ShowMsgBox("Enter Manufacture Date");
                    return false;
                }
            }


            if (txtWarrentyPeriod.Text != "")
            {
                if (txtWarrentyPeriod.Text.Contains('%') ||
                    txtWarrentyPeriod.Text.Contains('@') ||
                    txtWarrentyPeriod.Text.Contains('!') ||
                    txtWarrentyPeriod.Text.Contains('#') ||
                    txtWarrentyPeriod.Text.Contains('&') ||
                    txtWarrentyPeriod.Text.Contains('$') ||
                     txtWarrentyPeriod.Text.Contains('*'))
                {
                    txtWarrentyPeriod.Focus();
                    ShowMsgBox("Enter Valid Period");
                    return false;
                }
            }


            bValidate = true;
            return bValidate;
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                clsTcMaster objTcMaster = new clsTcMaster();
                string[] Arr = new string[2];

                if (txtManufactureDate.Text != "" && txtPurchaseDate.Text != "")
                {
                    string sResult = string.Empty;
                    sResult = Genaral.DateComparision(txtPurchaseDate.Text, txtManufactureDate.Text, false, false);
                    if (sResult == "2")
                    {
                        ShowMsgBox("Purchasing date should be greater than manufacturing Date");
                        return;
                    }
                }

                //Check AccessRights
                bool bAccResult;
                if (cmdSave.Text == "Update")
                {
                    bAccResult = CheckAccessRights("3");
                }
                else
                {
                    bAccResult = CheckAccessRights("2");
                }

                if (bAccResult == false)
                {
                    return;
                }

                if (ValidateForm() == true)
                {
                    objTcMaster.sTcId = txtTcID.Text;
                    objTcMaster.sTcSlNo = txtSerialNo.Text;
                    if (cmbMake.SelectedIndex > 0)
                    {
                        objTcMaster.sTcMakeId = cmbMake.SelectedValue;
                    }
                    objTcMaster.sTcCode = txtTcCode.Text;
                    objTcMaster.sTcCapacity = cmbCapacity.SelectedValue;
                    if (cmbMake.SelectedValue != "1")
                    {
                        objTcMaster.sManufacDate = txtManufactureDate.Text;
                    }
                    else
                    {
                        objTcMaster.sManufacDate = "";
                    }
                    objTcMaster.sAllotementDate = txtPurchaseDate.Text;
                    objTcMaster.sPoNo = txtPoNo.Text;
                    if (cmbSupplier.SelectedIndex > 0)
                    {
                        objTcMaster.sSupplierId = cmbSupplier.SelectedValue;
                    }
                    else
                    {
                        objTcMaster.sSupplierId = "0";
                    }
                    objTcMaster.sAltNo = txtPoNo.Text;
                    if (txtPrice.Text == "")
                    {
                        txtPrice.Text = "0";
                    }
                    objTcMaster.sPrice = txtPrice.Text;
                    objTcMaster.sWarrentyPeriod = txtWarrentyPeriod.Text;
                    objTcMaster.sLastServiceDate = txtLastServiceDate.Text;
                    objTcMaster.sCurrentLocation = cmbTcLocation.SelectedValue;
                    objTcMaster.sTcLifeSpan = txtTcLifeSpan.Text;
                    objTcMaster.sCrBy = objSession.UserId;
                    objTcMaster.sOfficeCode = objSession.OfficeCode;
                    objTcMaster.sLocationId = hdfTcLocation.Value;
                    if (cmbRating.SelectedIndex > 0)
                    {
                        objTcMaster.sRating = cmbRating.SelectedValue;
                    }
                    else
                    {
                        objTcMaster.sRating = "0";
                    }
                    if (cmbStarRated.SelectedIndex > 0)
                    {
                        objTcMaster.sStarRate = cmbStarRated.SelectedValue;
                    }
                    else
                    {
                        objTcMaster.sStarRate = "0";
                    }
                    //if (txtDtcCode.Text.Trim() != "")
                    {
                        objTcMaster.sDtcCodes = txtDtcCode.Text;
                    }
                    if (txtdepreciation.Text == "")
                    {
                        objTcMaster.sDepreciation = "0";
                    }
                    else
                    {
                        objTcMaster.sDepreciation = txtdepreciation.Text;
                    }
                    if (txtInsurance.Text == "")
                    {
                        objTcMaster.sInsurance = "0";
                    }
                    else
                    {
                        objTcMaster.sInsurance = txtInsurance.Text;
                    }
                    if (txtOriginalCost.Text == "")
                    {
                        objTcMaster.sOriginalCost = "0";
                    }
                    else
                    {
                        objTcMaster.sOriginalCost = txtOriginalCost.Text;
                    }
                    if (txtComponentID.Text == "")
                    {
                        objTcMaster.sComponentId = "0";
                    }
                    else
                    {
                        objTcMaster.sComponentId = txtComponentID.Text;
                    }
                    if (txtRAPDRP.Text == "")
                    {
                        objTcMaster.sInfosysId = "0";
                    }
                    else
                    {
                        objTcMaster.sInfosysId = txtRAPDRP.Text;
                    }

                    //if (cmdSave.Text == "Update" && cmbTcLocation.SelectedValue == "1")
                    //{
                    //    objTcMaster.sLocationId = "0";
                    //}

                    objTcMaster.sOilCapacity = txtOilCapacity.Text;
                    objTcMaster.sOilType = cmbOilType.SelectedValue;
                    objTcMaster.sWeight = txtWeight.Text;

                    if (cmbConditionOfTC.SelectedValue == "-Select-")
                    {
                        objTcMaster.sConditiontc = "0";
                    }
                    else
                    {
                        objTcMaster.sConditiontc = cmbConditionOfTC.SelectedValue;
                    }

                    objTcMaster.sCooling = cmbCooling.SelectedValue;
                    objTcMaster.sCore = cmbCoreType.SelectedValue;
                    objTcMaster.sType = cmbTCtype.SelectedValue;
                    objTcMaster.sTapeCharger = cmbTapCharger.SelectedValue;
                    objTcMaster.sroletype = objSession.sRoleType;
                    objTcMaster.locationtype = cmbTcLocation.SelectedValue;

                    objTcMaster.sFormName = strFormCode;
                    objTcMaster.sDescription = "DATA MODIFIED BY " + Session["FullName"];
                    objTcMaster.sXmlData = Convert.ToString(ViewState["sTcXmlData"]);

                    Arr = objTcMaster.SaveUpdateTransformerDetails(objTcMaster);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "TC Master");
                    }

                    if (Arr[1].ToString() == "0")
                    {
                        ShowMsgBox(Arr[0]);
                        //txtTcID.Text = objTcMaster.sTcId;
                        //cmdSave.Text = "Update";
                        //txtTcCode.Enabled = false;
                        Reset();
                        return;
                    }

                    if (Arr[1].ToString() == "1")
                    {



                        objTcMaster.GetTCDetails(objTcMaster);
                        sTcXmlData = objTcMaster.SaveXmlData(objTcMaster);
                        bool sResult;
                        sResult = objTcMaster.SaveWorkFlowData(objTcMaster);

                        objTcMaster.GetTCDetails(objTcMaster);
                        sTcXmlData = objTcMaster.SaveXmlData(objTcMaster);
                        //To get last data
                        ViewState["sTcXmlData"] = sTcXmlData;


                        ShowMsgBox(Arr[0]);
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



        public void Reset()
        {
            try
            {
                txtTcCode.Enabled = true;
                txtTcID.Text = string.Empty;
                txtTcCode.Text = string.Empty;
                txtSerialNo.Text = string.Empty;
                cmbMake.SelectedIndex = 0;
                cmbCapacity.SelectedIndex = 0;
                txtManufactureDate.Text = string.Empty;
                txtPurchaseDate.Text = string.Empty;
                cmbSupplier.SelectedIndex = 0;
                txtPoNo.Text = string.Empty;
                txtPrice.Text = string.Empty;
                txtWarrentyPeriod.Text = string.Empty;
                txtLastServiceDate.Text = string.Empty;
                cmbTcLocation.SelectedIndex = 0;
                cmdSave.Text = "Save";
                txtTcLifeSpan.Text = string.Empty;

                cmbRating.SelectedIndex = 0;
                cmbStarRated.Items.Clear();
                dvStar.Style.Add("display", "none");
                cmbTcLocation.Enabled = true;
                txtDtcCode.Enabled = false;
                txtOilCapacity.Text = string.Empty;
                txtWeight.Text = string.Empty;
                divRepairer.Style.Add("display", "none");
                divStore.Style.Add("display", "none");
                divField.Style.Add("display", "none");
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void txtTcCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                clsTcMaster objTcMaster = new clsTcMaster();
                objTcMaster.sTcCode = txtTcCode.Text;
                bool bResult = objTcMaster.CheckTransformerCodeExist(objTcMaster);
                if (bResult)
                {
                    txtTcCode.Focus();
                    ShowMsgBox("Transformer Code Already Exist");
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

                objApproval.sFormName = "TcMaster";
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

        protected void cmbRating_SelectedIndexChanged(object sender, EventArgs e)
        {
            //    try
            //    {
            //        if (cmbRating.SelectedValue == "1")
            //        {
            //            dvStar.Style.Add("display", "block");
            //            Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'SRT'", "--Select--", cmbStarRated);
            //        }
            //        else
            //        {
            //            dvStar.Style.Add("display", "none");
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        lblMessage.Text = clsException.ErrorMsg();
            //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbRating_SelectedIndexChanged");
            //    }
        }

        protected void cmbTcLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtTc = new DataTable();
            clsTcMaster objTcMaster = new clsTcMaster();
            try
            {

                if (cmbTcLocation.SelectedValue == "2")
                {
                    divField.Style.Add("display", "block");

                    if (objSession.OfficeCode.Length > Constants.Division)
                    {
                        objTcMaster.srepairOffCode = objSession.OfficeCode.Substring(0, Constants.Division);
                    }
                    else
                    {
                        objTcMaster.srepairOffCode = objSession.OfficeCode;
                    }
                    objTcMaster.sTcCode = hdfTcCode.Value;
                    dtTc = objTcMaster.GetFieldDetails(objTcMaster);
                    if (dtTc.Rows.Count > 0)
                    {
                        txtDtcCode.Text = dtTc.Rows[0]["DT_CODE"].ToString();
                        txtDtcName.Text = dtTc.Rows[0]["DT_NAME"].ToString();
                        divField.Style.Add("display", "block");
                    }

                    divRepairer.Style.Add("display", "none");
                    divStore.Style.Add("display", "none");

                }

                else if (cmbTcLocation.SelectedValue == "3")
                {
                    if (objSession.OfficeCode.Length > 2)
                    {
                        objTcMaster.srepairOffCode = objSession.OfficeCode.Substring(0, 2);
                    }
                    else
                    {
                        objTcMaster.srepairOffCode = objSession.OfficeCode;
                    }
                    objTcMaster.sLocationId = hdfTcLocation.Value;
                    objTcMaster.sTcCode = txtTcCode.Text;
                    dtTc = objTcMaster.GetRepairerDetails(objTcMaster);
                    if (dtTc.Rows.Count > 0)
                    {
                        txtRepairerName.Text = dtTc.Rows[0]["TR_NAME"].ToString();
                        txtReAddress.Text = dtTc.Rows[0]["TR_ADDRESS"].ToString();
                        txtReMobileNo.Text = dtTc.Rows[0]["TR_MOBILE_NO"].ToString();
                        txtReEmailId.Text = dtTc.Rows[0]["TR_EMAIL"].ToString();
                        divRepairer.Style.Add("display", "block");
                    }

                    divField.Style.Add("display", "none");
                    divStore.Style.Add("display", "none");
                }
                else if (cmbTcLocation.SelectedValue == "1")
                {
                    if (objSession.OfficeCode.Length > 2)
                    {
                        objTcMaster.srepairOffCode = objSession.OfficeCode.Substring(0, 2);
                    }
                    else
                    {
                        objTcMaster.srepairOffCode = objSession.OfficeCode;
                    }

                    objTcMaster.sTcCode = txtTcCode.Text;
                    dtTc = objTcMaster.GetStoreDetails(objTcMaster);
                    if (dtTc.Rows.Count > 0)
                    {
                        txtStoreName.Text = dtTc.Rows[0]["SM_NAME"].ToString();
                        txtStoreincharge.Text = dtTc.Rows[0]["SM_STORE_INCHARGE"].ToString();
                        txtStoreMobile.Text = dtTc.Rows[0]["SM_MOBILENO"].ToString();
                        txtStoreAddress.Text = dtTc.Rows[0]["SM_ADDRESS"].ToString();
                        divStore.Style.Add("display", "block");
                    }
                    divField.Style.Add("display", "none");
                    divRepairer.Style.Add("display", "none");
                }
                else
                {
                    if (objSession.OfficeCode.Length > 2)
                    {
                        objTcMaster.srepairOffCode = objSession.OfficeCode.Substring(0, 2);
                    }
                    else
                    {
                        objTcMaster.srepairOffCode = objSession.OfficeCode;
                    }
                    objTcMaster.sTcCode = txtTcCode.Text;
                    dtTc = objTcMaster.GetStoreDetails(objTcMaster);
                    if (dtTc.Rows.Count > 0)
                    {
                        txtStoreName.Text = dtTc.Rows[0]["SM_NAME"].ToString();
                        txtStoreincharge.Text = dtTc.Rows[0]["SM_STORE_INCHARGE"].ToString();
                        txtStoreMobile.Text = dtTc.Rows[0]["SM_MOBILENO"].ToString();
                        txtStoreAddress.Text = dtTc.Rows[0]["SM_ADDRESS"].ToString();
                        divStore.Style.Add("display", "block");
                    }

                    divField.Style.Add("display", "none");
                    divRepairer.Style.Add("display", "none");

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void btnDtcSearch_Click(object sender, EventArgs e)
        {
            try
            {
                clsTcMaster objTcMaster = new clsTcMaster();
                DataTable dtTc = new DataTable();
                objTcMaster.sDtcCodes = txtDtcCode.Text;
                dtTc = objTcMaster.GetDtcDetails(objTcMaster);
                if (dtTc.Rows.Count > 0)
                {
                    txtDtcCode.Text = dtTc.Rows[0]["DT_CODE"].ToString();
                    txtDtcName.Text = dtTc.Rows[0]["DT_NAME"].ToString();
                    divField.Style.Add("display", "block");
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

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
                    txtSerialNo.Enabled = false;
                    txtSerialNo.Text = "NNP" + txtTcCode.Text;
                    Div1.Style.Add("display", "none");
                }
                else
                {
                    txtSerialNo.Enabled = true;
                    txtSerialNo.Text = "";
                    Div1.Style.Add("display", "block");
                    txtManufactureDate.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbMake_SelectedIndexChanged");
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtDtcCode.Text = string.Empty;
                txtDtcName.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void lnkDTRHistory_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTcID.Text.Trim() != "")
                {
                    string sTCCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtTcCode.Text));
                    Response.Redirect("/Transaction/TcTracker.aspx?TCCode=" + sTCCode, false);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }



    }
}