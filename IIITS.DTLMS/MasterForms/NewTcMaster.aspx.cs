using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Collections;
using System.Data.OleDb;
using System.IO;
using System.Text;

namespace IIITS.DTLMS.MasterForms
{
    public partial class NewTcMaster : System.Web.UI.Page
    {
        string strFormCode = "NewTcMaster";     
        clsSession objSession;
        string storeId = "";
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
                Form.DefaultButton = cmdSave.UniqueID;
                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;
                txtAllotmentDate.Attributes.Add("readonly", "readonly");
                txtManufactureDate.Attributes.Add("readonly", "readonly");
                txtDINo.Attributes.Add("readonly", "readonly");

                ManufactureCalender.EndDate = System.DateTime.Now;
                PurchaseCalender.EndDate = System.DateTime.Now;

                //Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='SR' GROUP BY \"MD_ID\",\"MD_NAME\" ", "-Select-", cmbRating);
                //Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='C' GROUP BY \"MD_ID\",\"MD_NAME\"", "--Select--", cmbCapacity);
                //Genaral.Load_Combo("SELECT DISTINCT \"TM_ID\",\"TM_NAME\" FROM \"TBLTRANSMAKES\"  ORDER BY \"TM_NAME\"", "--Select--", cmbMake);
                //Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'OT' ORDER BY \"MD_ORDER_BY\" ", "--Select--", cmbOilType);

                if (!IsPostBack)
                {
                    clsTcMaster obj = new clsTcMaster();

                    if (objSession.OfficeCode.Length > 4)
                    {
                         storeId = obj.GetStoreId(objSession.OfficeCode);
                         ViewState["StoreID"] = storeId;
                    }
                    else
                    {
                        storeId = objSession.OfficeCode;
                        ViewState["StoreID"] = storeId;
                    }
                    LoadSearchWindow(storeId);

                    Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='SR' GROUP BY \"MD_ID\",\"MD_NAME\" ", "-Select-", cmbRating);
                    Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='C' GROUP BY \"MD_ID\",\"MD_NAME\"", "--Select--", cmbCapacity);
                    Genaral.Load_Combo("SELECT DISTINCT \"TM_ID\",\"TM_NAME\" FROM \"TBLTRANSMAKES\"  ORDER BY \"TM_NAME\"", "--Select--", cmbMake);
                    Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'OT' ORDER BY \"MD_ORDER_BY\" ", "--Select--", cmbOilType);


                    //Genaral.Load_Combo("select \"TS_ID\",\"TS_NAME\"  FROM \"TBLTRANSSUPPLIER\" WHERE \"TS_STATUS\"='A' ORDER BY \"TS_NAME\"", "-Select-", cmbSupplier);

                    //string strQry = string.Empty;
                    //strQry = "Title=Search and Select Po Details&";
                    ////strQry += "Query=SELECT \"PO_NO\",\"PO_ID\",\"TS_NAME\" FROM \"TBLPOMASTER\",\"TBLTRANSSUPPLIER\" WHERE \"PO_SUPPLIER_ID\"=\"TS_ID\" AND  {0} like %{1}% &";
                    //strQry += "Query=SELECT DISTINCT \"DI_NO\", \"TS_NAME\" FROM \"TBLPOMASTER\", \"TBLTRANSSUPPLIER\", \"TBLDELIVERYINSTRUCTION\" WHERE \"PO_ID\" = \"DI_PO_ID\" AND \"PO_SUPPLIER_ID\" = \"TS_ID\" AND  {0} like %{1}% &";
                    //strQry += "DBColName=\"DI_NO\"~\"TS_NAME\"&";
                    //strQry += "ColDisplayName=DI No~Supplier Name&";
                    //btnAltSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtPONo.ClientID + "&btn=" + btnPoSearch.ClientID + "',520,520," + txtPONo.ClientID + ")");

                    //ManufactureCalender.EndDate = System.DateTime.Now;
                    //PurchaseCalender.EndDate = System.DateTime.Now;
                    //txtPurchaseDate.Attributes.Add("onblur", "return ValidateDate(" + txtPurchaseDate.ClientID + ");");
                    //txtManufactureDate.Attributes.Add("onblur", "return ValidateDate(" + txtManufactureDate.ClientID + ");");
                    //CheckAccessRights("2");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

         }
        public void LoadSearchWindow(string storeId)
        {
            try
            {
                string strQry = string.Empty;
                strQry = "Title=Search and Select  Dispatch Instructions Details&";
                //strQry += "Query=SELECT  \"DI_NO\"  FROM \"TBLDELIVERYINSTRUCTION\" ";
                //strQry += "WHERE {0} like %{1}% AND \"DI_STATUS\"=1 AND \"DI_STORE_ID\"=" + storeId + "  group by \"DI_NO\" &";
                //strQry += "DBColName=\"DI_NO\" &";
                strQry += "Query=SELECT distinct \"DIM_DI_NO\"  FROM \"TBLDELIVERYINSTRUCTION\" inner join \"TBLDELIVERYINSTMASTER\" on \"DIM_ID\"=\"DI_DIM_ID\" inner join \"TBLTCRANGEALLOTMENT\" on  \"TCP_DI_ID\"=\"DI_ID\" ";
                //strQry += "WHERE {0} like %{1}% AND \"DI_STATUS\"=1 AND \"DI_STORE_ID\"=" + storeId + "  group by \"DIM_DI_NO\" &";
                strQry += "WHERE {0} like %{1}% AND \"DI_STATUS\"=1  group by \"DIM_DI_NO\" &";

                strQry += "DBColName=\"DIM_DI_NO\" &";
                strQry += "ColDisplayName=DI Number &";
                strQry = strQry.Replace("'", @"\'");
                btnDISearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtDINo.ClientID + "&btn=" + btnDISearch.ClientID + "',520,520," + txtDINo.ClientID + ")");

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        //public void LoadSearchWindow()//OLD CODE FOR ALLOTEMENT BASE INWARD 
        //{
        //    try
        //    {
        //        string strQry = string.Empty;
        //        strQry = "Title=Search and Select ALLOTMENT Details&";
        //        strQry += "Query=SELECT  \"ALT_NO\" FROM \"TBLALLOTEMENT\" ";
        //        strQry += "WHERE {0} like %{1}% AND \"ALT_STORE_ID\"="+objSession.OfficeCode+" GROUP BY \"ALT_NO\" &";
        //        strQry += "DBColName=\"ALT_NO\" &";
        //        strQry += "ColDisplayName=Allotment Number &";
        //        strQry = strQry.Replace("'", @"\'");
        //        btnAltSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtAltNo.ClientID + "&btn=" + btnAltSearch.ClientID + "',520,520," + txtAltNo.ClientID + ")");

        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}

        protected void btnPoSearch_Click(object sender, EventArgs e)
        {

            try
            {
                string sQery = string.Empty;
                storeId =(string) ViewState["StoreID"];

                //sQery = "SELECT \"ALT_NO\",\"ALT_NO\"  from \"TBLSTOREOFFCODE\", \"TBLSTOREMAST\",\"TBLDIVISION\",\"TBLALLOTEMENT\"  WHERE \"STO_SM_ID\"=\"SM_ID\" AND \"STO_OFF_CODE\"=\"DIV_CODE\" AND \"ALT_STATUS\"=1  AND \"ALT_DI_NO\"='" + txtDINo.Text + "' AND \"ALT_DIV_ID\"=\"DIV_ID\" AND  \"SM_ID\"=" + storeId + "  GROUP BY  \"ALT_NO\",\"ALT_NO\"";

                //Genaral.Load_Combo(sQery, "--Select--", cmbALTNo);
                
                
                //sQery = "SELECT DISTINCT \"TS_ID\",\"TS_NAME\" FROM \"TBLTRANSSUPPLIER\",\"TBLALLOTEMENT\",\"TBLDELIVERYINSTRUCTION\",\"TBLPOMASTER\" WHERE \"ALT_DI_NO\"=\"DI_NO\" AND \"DI_PO_ID\"=\"PO_ID\" AND \"TS_ID\"=\"PO_SUPPLIER_ID\" AND \"ALT_NO\"='" + txtAltNo.Text + "'  GROUP BY \"TS_ID\",\"TS_NAME\" ORDER BY \"TS_NAME\"";
                //Genaral.Load_Combo(sQery, "--Select--", cmbSupplier);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbALTNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadPoDetails();
                //storeId = (string)ViewState["StoreID"];

                //if (objSession.OfficeCode.Length > 4)
                //{
                //    Genaral.Load_Combo(" SELECT \"DIV_ID\",\"DIV_NAME\"  from \"TBLSTOREOFFCODE\", \"TBLSTOREMAST\",\"TBLDIVISION\",\"TBLALLOTEMENT\"  WHERE \"STO_SM_ID\"=\"SM_ID\" AND \"STO_OFF_CODE\"=\"DIV_CODE\"  and \"ALT_STATUS\"=1 AND \"ALT_NO\"='" + cmbALTNo.SelectedItem.Text + "' AND \"ALT_DIV_ID\"=\"DIV_ID\" AND  \"SM_ID\"=\"STO_SM_ID\" AND CAST(\"STO_OFF_CODE\" AS TEXT)=substr(CAST('" + objSession.OfficeCode + "' AS TEXT),1,3) GROUP BY  \"DIV_ID\",\"DIV_NAME\"", "--Select--", cmbDiv);
                //}
                //else
                //{
                //    //Genaral.Load_Combo("SELECT \"DIV_ID\",\"DIV_NAME\"  from \"TBLSTOREOFFCODE\", \"TBLSTOREMAST\",\"TBLDIVISION\",\"TBLALLOTEMENT\"  WHERE \"STO_SM_ID\"=\"SM_ID\" AND \"STO_OFF_CODE\"=\"DIV_CODE\"  AND \"ALT_NO\"='" + cmbALTNo.SelectedItem.Text + "' AND \"ALT_DIV_ID\"=\"DIV_ID\" AND  \"SM_ID\"=" + storeId + "  and \"ALT_STATUS\"=1 GROUP BY  \"DIV_ID\",\"DIV_NAME\"", "--Select--", cmbDiv);
                //    Genaral.Load_Combo("SELECT \"DIV_ID\",\"DIV_NAME\"  from \"TBLSTOREOFFCODE\", \"TBLSTOREMAST\",\"TBLDIVISION\",\"TBLALLOTEMENT\"  WHERE \"STO_SM_ID\"=\"SM_ID\" AND \"STO_OFF_CODE\"=\"DIV_CODE\"  AND \"ALT_NO\"='" + cmbALTNo.SelectedItem.Text + "' AND \"ALT_DIV_ID\"=\"DIV_ID\" AND  \"SM_ID\"=" + storeId + "  and \"ALT_STATUS\"=1 GROUP BY  \"DIV_ID\",\"DIV_NAME\"", "--Select--", cmbDiv);

                //}
                loaddtrdetails();


                    //string dinoo = txtDINo.Text;

                    //Genaral.Load_Combo("select \"TCP_ID\" ,\"TCP_TC_CODE\" from \"TBLDELIVERYINSTMASTER\" inner join \"TBLDELIVERYINSTRUCTION\" on \"DIM_ID\" = \"DI_DIM_ID\" inner join \"TBLTCRANGEALLOTMENT\" on \"DI_ID\"=\"TCP_DI_ID\" and \"TCP_STATUS_M\"='0' and \"DIM_DI_NO\" ='" + txtDINo.Text + "'", "--Select--", txtTcCode);
               
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void loaddtrdetails()
        {
            try
            {
                string dinoo = txtDINo.Text;

                Genaral.Load_Combo("select \"TCP_ID\" ,\"TCP_TC_CODE\" from \"TBLDELIVERYINSTMASTER\" inner join \"TBLDELIVERYINSTRUCTION\" on \"DIM_ID\" = \"DI_DIM_ID\" inner join \"TBLTCRANGEALLOTMENT\" on \"DI_ID\"=\"TCP_DI_ID\" and \"TCP_STATUS_M\"='0' and \"DIM_DI_NO\" ='" + txtDINo.Text + "'", "--Select--", txtTcCode);

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                storeId = (string)ViewState["StoreID"];
                Genaral.Load_Combo("SELECT DISTINCT \"TM_ID\",\"TM_NAME\" FROM \"TBLTRANSMAKES\",\"TBLALLOTEMENT\" WHERE \"TM_ID\"=\"ALT_MAKE_ID\" AND  \"ALT_STORE_ID\"=" + storeId + "  AND  \"ALT_NO\"='" + cmbALTNo.SelectedItem.Text + "'  and \"ALT_STATUS\"=1 AND  \"ALT_DIV_ID\"=" + cmbDiv.SelectedValue + "  ORDER BY \"TM_NAME\"", "--Select--", cmbMake);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbtxttccode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string dinoo = txtDINo.Text;
              string tccodeid = txtTcCode.SelectedValue;
                clsTcMaster objTcMaster = new clsTcMaster();
                string tccode = txtTcCode.SelectedItem.Text;
                objTcMaster=objTcMaster.GetdtrDetails(tccode);

                txtSerialNo.Text = objTcMaster.sTcSlNo;
                cmbMake.SelectedValue=objTcMaster.sTcMakeId;
                cmbCapacity.SelectedValue = objTcMaster.sTcCapacity;
                cmbRating.SelectedValue = objTcMaster.sStarRate;
                txtManufactureDate.Text = objTcMaster.sManufacDate;
                txtWarrentyPeriod.Text = objTcMaster.sWarrentyPeriod;
                txtTcLifeSpan.Text = objTcMaster.sTcLifeSpan;
                txtOilCapacity.Text = objTcMaster.sOilCapacity;
               
                txtWeight.Text = objTcMaster.sWeight;
                //  cmbOilType.SelectedValue = objTcMaster.sOilType;

                txtSerialNo.ReadOnly = true;
                cmbMake.Enabled = false;
                cmbCapacity.Enabled = false;
                cmbRating.Enabled = false;
               // txtManufactureDate.ReadOnly = true;
                txtWarrentyPeriod.ReadOnly = true;
                txtTcLifeSpan.ReadOnly = true;
                txtOilCapacity.ReadOnly = true;
                txtWeight.ReadOnly = true;
                cmbOilType.Enabled = false;
                cmbOilType.SelectedValue = objTcMaster.sOilType;
                // Genaral.Load_Combo("select \"TCP_ID\" ,\"TCP_TC_CODE\" from \"TBLDELIVERYINSTMASTER\" inner join \"TBLDELIVERYINSTRUCTION\" on \"DIM_ID\" = \"DI_DIM_ID\" inner join \"TBLTCPLATEALLOCATION\" on \"DI_ID\"=\"TCP_DI_ID\" and \"TCP_STATUS_M\"='0' and \"DIM_DI_NO\" =" + dinoo + "", "--Select--", txtTcCode);
                //  Genaral.Load_Combo("SELECT DISTINCT CAST(\"ALT_CAPACITY\" as TEXT),CAST(\"ALT_CAPACITY\" AS TEXT) PB_CAPACITY FROM \"TBLALLOTEMENT\" WHERE  \"ALT_MAKE_ID\"=" + cmbMake.SelectedValue + "  and \"ALT_STATUS\"=1 AND  \"ALT_STORE_ID\"=" + storeId + " and \"ALT_NO\"='" + cmbALTNo.SelectedItem.Text + "'", "-Select-", cmbCapacity);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbMake_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                storeId = (string)ViewState["StoreID"];
                Genaral.Load_Combo("SELECT DISTINCT CAST(\"ALT_CAPACITY\" as TEXT),CAST(\"ALT_CAPACITY\" AS TEXT) PB_CAPACITY FROM \"TBLALLOTEMENT\" WHERE  \"ALT_MAKE_ID\"=" + cmbMake.SelectedValue + "  and \"ALT_STATUS\"=1 AND  \"ALT_STORE_ID\"=" + storeId + " and \"ALT_NO\"='" + cmbALTNo.SelectedItem.Text + "'", "-Select-", cmbCapacity);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbCapacity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                storeId = (string)ViewState["StoreID"];
                //Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\", \"TBLALLOTEMENT\" WHERE \"MD_TYPE\"='SR' AND \"ALT_DI_NO\"='" + txtDINo.Text + "' AND  \"ALT_STORE_ID\"=" + storeId + "  AND \"MD_ID\"=\"ALT_STAR_TYPE\" AND \"ALT_CAPACITY\"='" + cmbCapacity.SelectedItem.Text + "' GROUP BY \"MD_ID\",\"MD_NAME\" ", "-Select-", cmbRating);
                Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='SR'  GROUP BY \"MD_ID\",\"MD_NAME\" ", "-Select-", cmbRating);

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void LoadPoDetails()
        {
            try
            {
                clsTcMaster objTcMaster = new clsTcMaster();
                DataTable dt = new DataTable();
                //objTcMaster.sPoId = hdfPOId.Value;s
                //txtPOId.Text = hdfPOId.Value;
                //hdfPOId.Value = "";
                //txtPONo.Text = txtPOId.Text;
                objTcMaster.sDINo = txtDINo.Text;
                storeId = (string)ViewState["StoreID"];
              //  objTcMaster.sAltNo = cmbALTNo.SelectedItem.Text;
                objTcMaster.sAltNo = "";
                objTcMaster.sOfficeCode = storeId;
                dt = objTcMaster.GetAllotedDetails(objTcMaster);
                if (dt.Rows.Count > 0)
                 {
                    //txtDIId.Text = dt.Rows[0]["ALT_ID"].ToString();
                    //txtDINo.Text = dt.Rows[0]["ALT_DI_NO"].ToString();
                    //txtAllotmentDate.Text = dt.Rows[0]["ALT_DATE"].ToString();
                    //txtSupplier.Text = dt.Rows[0]["SUPPLIER_NAME"].ToString();
                    txtDIId.Text = dt.Rows[0]["DIM_ID"].ToString();
                    txtDINo.Text = dt.Rows[0]["DIM_DI_NO"].ToString();
                    txtAllotmentDate.Text = dt.Rows[0]["PO_DATE"].ToString();
                    txtSupplier.Text = dt.Rows[0]["SUPPLIER_NAME"].ToString();
                  objTcMaster.sDINo = dt.Rows[0]["DIM_DI_NO"].ToString();
                    txtDINum.Text = dt.Rows[0]["DIM_DI_NO"].ToString();
                }
                ViewState["AllotDetails"] = dt;
                txtQuantity.Text = objTcMaster.loadCountTc(objTcMaster);              
                  LoadTcGrid(txtDINo.Text);
                               
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadTcGrid(string sAltNo)
        {
            DataTable dtTcDetails = new DataTable();
            try
            {
                clsTcMaster objTcMaster = new clsTcMaster();
                storeId = (string)ViewState["StoreID"];

                //objTcMaster.sAltNo = cmbALTNo.SelectedItem.Text;
                objTcMaster.sAltNo = "0";
                objTcMaster.sOfficeCode = storeId;
                objTcMaster.sDINo = txtDINo.Text;
                dtTcDetails = objTcMaster.LoadTcGrid(objTcMaster);
               
                grdAltQuantity.DataSource = dtTcDetails;
                grdAltQuantity.DataBind();
                txtDINum.Text = txtDINo.Text;
                ViewState["TcDetailsGrid"] = dtTcDetails;
                grdAltQuantity.Visible = true;

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

        bool ValidateForm()
        {
            bool bValidate = false;
            //if (txtTcCode.Text.Trim().Length == 0)
            if (txtTcCode.SelectedIndex < 0)
            {
                txtTcCode.Focus();
                ShowMsgBox("Enter DTr Code");
                return false;
            }
                       
            if (txtSerialNo.Text.Trim().Length == 0)
            {
                txtSerialNo.Focus();
                ShowMsgBox("Enter DTr Serial Number");
                 return false;
            }
            if (cmbMake.SelectedIndex < 0)
            {
                cmbMake.Focus();
                ShowMsgBox("Please Select the DTr Make");              
                return false;
            }
            if (cmbCapacity.SelectedIndex < 0)
            {
                cmbCapacity.Focus();
                ShowMsgBox("Select DTr Capacity");
                return false;
            }
            if (cmbRating.SelectedIndex < 0)
            {
                cmbRating.Focus();
                ShowMsgBox("Select The Rating");
                return false;
            }
            if (txtDINum.Text.Trim().Length == 0)
            {
                txtDINum.Focus();
                ShowMsgBox("Enter The Dispatch Instrution Number");
                return false;
            }
            if (txtManufactureDate.Text.Trim().Length == 0)
            {
                txtManufactureDate.Focus();
                ShowMsgBox("Select the Manufacture Date");
                return false;
            }
            string  sResult = Genaral.DateComparision(txtManufactureDate.Text, "", true, false);
            if (sResult == "1")
            {
                ShowMsgBox("Manufacturing Date Should be Less Than or Equal to Current Date");
                return bValidate;
            }
            if (txtTcLifeSpan.Text.Trim().Length == 0)
            {
                txtTcLifeSpan.Focus();
                ShowMsgBox("Enter Valid Life Span");
                return false;
            }
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
            if (txtWarrentyPeriod.Text.Length == 0)
            {
                txtWarrentyPeriod.Focus();
                ShowMsgBox("Select warranty Period up to");
                return false;
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
            if (txtOilCapacity.Text == "0")
            {
                txtOilCapacity.Focus();
                ShowMsgBox("Enter valid Oil Capacity");
                return false;
            }
            if (txtWeight.Text == "0")
            {
                txtWeight.Focus();
                ShowMsgBox("Enter valid Weight of DTr");
                return false;
            }
            if (FileNamePlate.FileName == ""  )
            {
                FileNamePlate.Focus();
                ShowMsgBox(" Select Name Plate Photo");
                return false;
            }
            if (FileSSplate.FileName == "" )
            {
                FileSSplate.Focus();
                ShowMsgBox(" Select DTr Code Photo");
                return false;
            }
            if (FileSSplate.FileName == FileNamePlate.FileName)
            {
                FileSSplate.Focus();
                FileNamePlate.Focus();
                ShowMsgBox("Name Plate And SS Plate Should Not be the Same File");
                return false;
            }

            if (txtAllotmentDate.Text.Trim() != "" && txtManufactureDate.Text.Trim() != "")
            {
                //string sResult = Genaral.DateComparision(txtPurchaseDate.Text, txtManufactureDate.Text, false, false);
                //if (sResult == "2")
                //{
                //    ShowMsgBox("Purchasing Date should be Greater than Manufacturing Date");
                //    return bValidate;
                //}
            }
            //if (cmbCapacity.SelectedItem.Text == Convert.ToString(dtTcDetails.Rows[]["PB_CAPACITY"]) && ddlMake.SelectedItem.Text == Convert.ToString(dt.Rows[i]["PB_MAKE"]))
            //{
            //    //ShowMsgBox("Capacity("+ dt.Rows[i]["PB_CAPACITY"] + ")-MakeName(" + dt.Rows[i]["PB_MAKE"] + ") Combination Already Added");
            //    ShowMsgBox("Check the Capacity-MakeName Combination");
            //    return false;
            //}
              DataTable dt=(DataTable)ViewState["TcDetailsGrid"];
              for (int i = 0; i < dt.Rows.Count; i++)
              {
                  if (cmbCapacity.SelectedItem.Text == Convert.ToString(dt.Rows[i]["CAPACITY"]))
                  {
                      if ((cmbMake.SelectedItem.Text != Convert.ToString(dt.Rows[i]["TM_NAME"])) || cmbRating.SelectedItem.Text != Convert.ToString(dt.Rows[i]["RATING"]))
                      {
                          ShowMsgBox("The Combination of Capacity, Make and Rating not valid");
                          return false;
                      }
                      else
                      {
                          return true;
                      }
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
                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);

                // clsFtp objFtp = new clsFtp(sFileServerPath, sUserName, sPassword);
                clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);
                bool Isuploaded;
                bool IsFileExiest;

                string[] Arr = new string[2];

                if (ValidateSave() == true)
                {
                    DataTable dtAllot = (DataTable)ViewState["AllotDetails"];

                    objTcMaster.sDINo = Convert.ToString(dtAllot.Rows[0]["DIM_DI_NO"]);
                    objTcMaster.sPoNo = Convert.ToString(dtAllot.Rows[0]["PO_NO"]);
                    objTcMaster.sAltId = txtDIId.Text;
               //     objTcMaster.sAltNo = cmbALTNo.SelectedItem.Text;
                    objTcMaster.sAllotementDate = txtAllotmentDate.Text;
                    objTcMaster.sSupplierId = Convert.ToString(dtAllot.Rows[0]["PO_SUPPLIER_ID"]);
                    //objTcMaster.sQuantity = txtQuantity.Text;
                    objTcMaster.sCrBy = objSession.UserId;
                    objTcMaster.sOfficeCode = objSession.OfficeCode;
                    objTcMaster.sroletype = objSession.sRoleType;

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

                    objTcMaster.sClientIP = sClientIP;


                    int i = 0;
                    string[] strQryVallist = new string[grdTCDetails.Rows.Count];
                    foreach (GridViewRow row in grdTCDetails.Rows)
                    {
                        strQryVallist[i] = ((Label)row.FindControl("lblTCCode")).Text.Trim() + "~" + ((Label)row.FindControl("lblTCSlNo")).Text.Trim() + "~" + ((Label)row.FindControl("lblMakeID")).Text.Trim()
                            + "~" + ((Label)row.FindControl("lblCapacity")).Text.Trim() + "~" + ((Label)row.FindControl("lblManfDate")).Text.Trim() + "~" + ((Label)row.FindControl("lblLifeSpan")).Text.Trim()
                            + "~" + ((Label)row.FindControl("lblWarrenty")).Text.Trim() + "~" + ((Label)row.FindControl("lblOilCapacity")).Text.Trim() + "~" + ((Label)row.FindControl("lblWeight")).Text.Trim()
                            + "~" + ((Label)row.FindControl("lblTcstarRate")).Text.Trim() + "~" + ((Label)row.FindControl("lblOilType")).Text.Trim();

                        //+ "~" + ((Label)row.FindControl("lblTcstarRate")).Text.Trim() + "~" + ((Label)row.FindControl("lblAltno")).Text.Trim() + "~" + ((Label)row.FindControl("lblDivID")).Text.Trim() + "~" + ((Label)row.FindControl("lblOilType")).Text.Trim();

                        i++;
                    }
                    objTcMaster.sQuantity = Convert.ToString(grdTCDetails.Rows.Count);
                    Arr = objTcMaster.SaveTCDetails(strQryVallist, objTcMaster);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "New DTR Master");
                    }

                    if (Arr[1].ToString() == "0")
                    {
                        DataTable dtTc = (DataTable)ViewState["TC"];
                        for (int j = 0; j < dtTc.Rows.Count; j++)
                        {
                            string sMainFolder = "TcAllotment";
                            string sNameplate = "NAMEPLATE";
                            string sSSplate = "SSPLATE";
                            objTcMaster.sTcCode = Convert.ToString(dtTc.Rows[j]["TC_CODE"]);
                            string NPlateFname = sNameplate + Convert.ToString(dtTc.Rows[j]["NAME_PLATE_FNAME"]);
                            string SSplateFname = sSSplate + Convert.ToString(dtTc.Rows[j]["SS_PLATE_FNAME"]);
                            byte[] fileNamePlate = null;
                            byte[] fileSSplate = null;
                            fileNamePlate = Encoding.Unicode.GetBytes(dtTc.Rows[j]["NAME_PLATE"].ToString().Trim());
                            fileSSplate = Encoding.Unicode.GetBytes(dtTc.Rows[j]["SS_PLATE"].ToString().Trim());

                            //MemoryStream ms = new MemoryStream(fileNamePlate);

                            //Response.Clear();
                            //Response.Buffer = false;
                            //Response.ContentType = "image/png";
                            //Response.AddHeader("Content-disposition", string.Format("attachment; filename={0};", NPlateFname));
                            //ms.WriteTo(Response.OutputStream);


                            //File.WriteAllBytes(Server.MapPath("~/DTLMSDocs" + "/" + objTcMaster.sTcCode + "~" + fileNamePlate),
                            //Encoding.Unicode.GetBytes(dtTc.Rows[j]["NAME_PLATE"].ToString().Trim()));

                            //File.WriteAllBytes(Server.MapPath("~/DTLMSDocs" + "/" + objTcMaster.sTcCode + "~" + fileSSplate),
                            //Encoding.Unicode.GetBytes(dtTc.Rows[j]["SS_PLATE"].ToString().Trim()));

                            string sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objTcMaster.sTcCode + "~" + NPlateFname);
                            string sDirectory1 = Server.MapPath("~/DTLMSDocs" + "/" + objTcMaster.sTcCode + "~" + SSplateFname);

                            if (File.Exists(sDirectory))
                            {

                                bool IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder  + sMainFolder);
                                if (IsExists == false)
                                {

                                    objFtp.createDirectory(SFTPmainfolder  + sMainFolder);
                                }
                                IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolder + "/" + objTcMaster.sTcCode);
                                if (IsExists == false)
                                {

                                    objFtp.createDirectory(SFTPmainfolder + sMainFolder + "/" + objTcMaster.sTcCode);
                                }
                                IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolder + "/" + objTcMaster.sTcCode + "/" + sNameplate);
                                if (IsExists == false)
                                {

                                    objFtp.createDirectory(SFTPmainfolder  + sMainFolder + "/" + objTcMaster.sTcCode + "/" + sNameplate);
                                }


                                IsFileExiest = objFtp.IsfileExiest(SFTPmainfolder + sMainFolder + "/" + objTcMaster.sTcCode + "/" + sNameplate);
                                if (IsFileExiest == false)
                                {
                                    Isuploaded = objFtp.upload(SFTPmainfolder + sMainFolder + "/" + objTcMaster.sTcCode + "/" + sNameplate + "/", NPlateFname, sDirectory);
                                    if (Isuploaded == true & File.Exists(sDirectory))
                                    {
                                        File.Delete(sDirectory);
                                        sDirectory = objTcMaster.sTcCode + "/" + NPlateFname;

                                    }
                                }
                                else
                                {
                                    objFtp.Delete(SFTPmainfolder + sMainFolder + "/" + objTcMaster.sTcCode + "/" + sNameplate + "/" + NPlateFname);
                                    Isuploaded = objFtp.upload(SFTPmainfolder  + sMainFolder + "/" + objTcMaster.sTcCode + "/" + sNameplate + "/", NPlateFname, sDirectory);
                                    if (Isuploaded == true & File.Exists(sDirectory))
                                    {
                                        File.Delete(sDirectory);
                                        sDirectory = objTcMaster.sTcCode + "/" + NPlateFname;

                                    }

                                }

                            }
                            if (File.Exists(sDirectory1))
                            {

                                bool IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder  + sMainFolder);
                                if (IsExists == false)
                                {

                                    objFtp.createDirectory(SFTPmainfolder  + sMainFolder);
                                }
                                IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolder + "/" + objTcMaster.sTcCode);
                                if (IsExists == false)
                                {

                                    objFtp.createDirectory(SFTPmainfolder + sMainFolder + "/" + objTcMaster.sTcCode);
                                }
                                IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + "/" + sMainFolder + "/" + objTcMaster.sTcCode + "/" + sSSplate);
                                if (IsExists == false)
                                {

                                    objFtp.createDirectory(SFTPmainfolder + "/" + sMainFolder + "/" + objTcMaster.sTcCode + "/" + sSSplate);
                                }


                                IsFileExiest = objFtp.IsfileExiest(SFTPmainfolder + "/" + sMainFolder + "/" + objTcMaster.sTcCode + "/" + sSSplate);
                                if (IsFileExiest == false)
                                {
                                    Isuploaded = objFtp.upload(SFTPmainfolder + "/" + sMainFolder + "/" + objTcMaster.sTcCode + "/" + sSSplate + "/", SSplateFname, sDirectory1);
                                    if (Isuploaded == true & File.Exists(sDirectory1))
                                    {
                                        File.Delete(sDirectory1);
                                        sDirectory1 = objTcMaster.sTcCode + "/" + sSSplate;

                                    }
                                }
                                else
                                {
                                    objFtp.Delete(SFTPmainfolder + "/" + sMainFolder + "/" + objTcMaster.sTcCode + "/" + sSSplate + "/");
                                    Isuploaded = objFtp.upload(SFTPmainfolder + "/" + sMainFolder + "/" + objTcMaster.sTcCode + "/" + sSSplate + "/", SSplateFname, sDirectory1);
                                    if (Isuploaded == true & File.Exists(sDirectory1))
                                    {
                                        File.Delete(sDirectory1);
                                        sDirectory1 = objTcMaster.sTcCode + "/" + SSplateFname;

                                    }

                                }

                            }
                        }
                        ShowMsgBox(Arr[0].ToString());
                        Reset();
                        cmdSave.Visible = false;
                        LoadTcGrid(txtDIId.Text);
                        grdTCDetails.DataSource = null;
                        grdTCDetails.DataBind();
                        ViewState["TC"] = null;
                        loaddtrdetails();

                        return;

                    }
                    if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[2]);
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
                txtTcCode.ClearSelection();
                txtSerialNo.Text = string.Empty;
                cmbDiv.ClearSelection();
                cmbMake.ClearSelection();
                cmbCapacity.ClearSelection();
                cmbRating.ClearSelection();
                txtManufactureDate.Text = string.Empty;
                txtWarrentyPeriod.Text = string.Empty;                
                txtTcLifeSpan.Text = string.Empty;
                txtOilCapacity.Text = string.Empty;
                cmbOilType.ClearSelection();
                txtWeight.Text = string.Empty;
               
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }



        public void AddTCtoGrid(string sTcCode, string sTCSlno)
        {
            try
            {
                if (ValidateGridValue(sTcCode, sTCSlno) == true)
                {
                    if (ViewState["TC"] != null)
                    {

                        DataTable dtTc = (DataTable)ViewState["TC"];
                        DataRow drow;
                        if (dtTc.Rows.Count > 0)
                        {
                            string strpath = System.IO.Path.GetExtension(FileNamePlate.FileName);
                            string filename = Path.GetFileName(FileNamePlate.PostedFile.FileName);


                            string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["PhotoFormat"]);
                            string sAnxFileExt = System.IO.Path.GetExtension(FileNamePlate.FileName).ToString().ToLower();
                            sAnxFileExt = ";" + sAnxFileExt.Remove(0, 1) + ";";

                            if (!sFileExt.Contains(sAnxFileExt))
                            {
                                ShowMsgBox("Invalid File Format");
                                FileNamePlate.Focus();
                                return;
                            }
                           
                            #region//convert file to byte code
                            ////Read the uploaded File as Byte Array from FileUpload control.
                            //Stream fs = FileNamePlate.PostedFile.InputStream;
                            //BinaryReader br = new BinaryReader(fs);
                            //byte[] bytes = br.ReadBytes((Int32)fs.Length);

                            ////Save the Byte Array as File.
                            //string filePath = "~/DTLMSDocs/" + Path.GetFileName(FileNamePlate.FileName);
                            //File.WriteAllBytes(Server.MapPath(filePath), bytes);

                            //Read the uploaded File as Byte Array from FileUpload control.
                            //Stream fs1 = FileSSplate.PostedFile.InputStream;
                            //BinaryReader br1 = new BinaryReader(fs1);
                            //byte[] bytes1 = br1.ReadBytes((Int32)fs.Length);

                            ////Save the Byte Array as File.
                            //string filePath1 = "~/DTLMSDocs/" + Path.GetFileName(FileSSplate.FileName);
                            //File.WriteAllBytes(Server.MapPath(filePath1), bytes1);
                            #endregion
                            string strpath1 = System.IO.Path.GetExtension(FileSSplate.FileName);
                            string filename1 = Path.GetFileName(FileSSplate.PostedFile.FileName);

                            string sAnxFileExt1 = System.IO.Path.GetExtension(FileSSplate.FileName).ToString().ToLower();
                            sAnxFileExt1 = ";" + sAnxFileExt1.Remove(0, 1) + ";";

                            if (!sFileExt.Contains(sAnxFileExt1))
                            {
                                ShowMsgBox("Invalid File Format");
                                FileSSplate.Focus();
                                return;
                            }

                            //FileNamePlate.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + txtTcCode.Text + "~" + "NAMEPLATE" + FileNamePlate.FileName));
                            //FileSSplate.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + txtTcCode.Text + "~" + "SSPLATE" + FileSSplate.FileName));
                            FileNamePlate.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + txtTcCode.SelectedItem.Text + "~" + "NAMEPLATE" + FileNamePlate.FileName));
                            FileSSplate.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + txtTcCode.SelectedItem.Text + "~" + "SSPLATE" + FileSSplate.FileName));


                            bool isCount = isCountCapacity(sTcCode);
                            if (isCount)
                            {
                                drow = dtTc.NewRow();
                                string temp = txtSerialNo.Text;
                                drow["TC_CODE"] = txtTcCode.SelectedItem.Text;
                                drow["TC_SLNO"] = temp.ToUpper();
                              //  drow["DIV_NAME"] = cmbDiv.SelectedItem.Text;
                             //   drow["Div_ID"] = cmbDiv.SelectedValue;
                                drow["TM_NAME"] = cmbMake.SelectedItem.Text;
                                drow["MAKE_ID"] = cmbMake.SelectedValue;
                                drow["TC_CAPACITY"] = cmbCapacity.SelectedItem.Text;
                                drow["TC_MANF_DATE"] = txtManufactureDate.Text;
                                drow["LIFE_SPAN"] = txtTcLifeSpan.Text;
                                drow["TC_WARANTY_PERIOD"] = txtWarrentyPeriod.Text;
                                drow["TC_OIL_CAPACITY"] = txtOilCapacity.Text;
                                drow["TC_OIL_TYPE"] = cmbOilType.SelectedValue;
                                drow["TC_WEIGHT"] = txtWeight.Text;
                                drow["TC_STAR_RATE"] = cmbRating.SelectedValue;
                                drow["TC_RATING"] = cmbRating.SelectedItem.Text;
                              //  drow["TC_ALT_NO"] = cmbALTNo.SelectedItem.Text;
                                drow["NAME_PLATE"] = Convert.ToBase64String(FileNamePlate.FileBytes);
                                drow["SS_PLATE"] = Convert.ToBase64String(FileSSplate.FileBytes);
                                drow["NAME_PLATE_FNAME"] = FileNamePlate.FileName;
                                drow["SS_PLATE_FNAME"] = FileSSplate.FileName;
                                dtTc.Rows.Add(drow);
                                grdTCDetails.DataSource = dtTc;
                                grdTCDetails.DataBind();
                                ViewState["TC"] = dtTc;
                            }
                            else
                            {
                                ShowMsgBox("You Already Allocated Requested number of transformers");
                                // ViewState["TC"] = dtTc;
                                return;
                            }
                        }
                    }

                    else
                    {
                        DataTable dtTC = new DataTable();
                        DataRow drow;
                        ViewState["TC"] = null;
                        bool isCount = CheckQuantityAvailable(sTcCode);
                        if (isCount)
                        {

                            string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["PhotoFormat"]);
                            string sAnxFileExt = System.IO.Path.GetExtension(FileNamePlate.FileName).ToString().ToLower();
                            sAnxFileExt = ";" + sAnxFileExt.Remove(0, 1) + ";";

                            if (!sFileExt.Contains(sAnxFileExt))
                            {
                                ShowMsgBox("Invalid File Format");
                                FileNamePlate.Focus();
                                return;
                            }
                           
                            //Read the uploaded File as Byte Array from FileUpload control.
                            Stream fs = FileNamePlate.PostedFile.InputStream;
                            BinaryReader br = new BinaryReader(fs);
                            byte[] bytes = br.ReadBytes((Int32)fs.Length);

                            ////Save the Byte Array as File.
                            //string filePath = "~/DTLMSDocs/" + Path.GetFileName(FileNamePlate.FileName);
                            //File.WriteAllBytes(Server.MapPath(filePath), bytes);


                            string sAnxFileExt1 = System.IO.Path.GetExtension(FileSSplate.FileName).ToString().ToLower();
                            sAnxFileExt1 = ";" + sAnxFileExt1.Remove(0, 1) + ";";

                            if (!sFileExt.Contains(sAnxFileExt1))
                            {
                                ShowMsgBox("Invalid File Format");
                                FileSSplate.Focus();
                                return;
                            }

                            //FileNamePlate.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + txtTcCode.Text + "~" + "NAMEPLATE" + FileNamePlate.FileName));
                            //FileSSplate.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + txtTcCode.Text + "~" + "SSPLATE" + FileSSplate.FileName));
                            FileNamePlate.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + txtTcCode.SelectedItem.Text + "~" + "NAMEPLATE" + FileNamePlate.FileName));
                            FileSSplate.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + txtTcCode.SelectedItem.Text + "~" + "SSPLATE" + FileSSplate.FileName));

                            //Read the uploaded File as Byte Array from FileUpload control.
                            Stream fs1 = FileSSplate.PostedFile.InputStream;
                            BinaryReader br1 = new BinaryReader(fs1);
                            byte[] bytes1 = br1.ReadBytes((Int32)fs.Length);

                            ////Save the Byte Array as File.
                            //string filePath1 = "~/DTLMSDocs/" + Path.GetFileName(FileSSplate.FileName);
                            //File.WriteAllBytes(Server.MapPath(filePath1), bytes1);

                            //dtFaultTc.Columns.Add(new DataColumn("TC_ID"));
                            dtTC.Columns.Add(new DataColumn("TC_CODE"));
                            dtTC.Columns.Add(new DataColumn("TC_SLNO"));
                         //   dtTC.Columns.Add(new DataColumn("DIV_NAME"));
                         //   dtTC.Columns.Add(new DataColumn("DIV_ID"));
                            dtTC.Columns.Add(new DataColumn("TM_NAME"));
                            dtTC.Columns.Add(new DataColumn("MAKE_ID"));
                            dtTC.Columns.Add(new DataColumn("TC_CAPACITY"));
                            dtTC.Columns.Add(new DataColumn("TC_MANF_DATE"));
                            dtTC.Columns.Add(new DataColumn("LIFE_SPAN"));
                            dtTC.Columns.Add(new DataColumn("TC_WARANTY_PERIOD"));
                            dtTC.Columns.Add(new DataColumn("TC_OIL_CAPACITY"));
                            dtTC.Columns.Add(new DataColumn("TC_OIL_TYPE"));
                            dtTC.Columns.Add(new DataColumn("TC_WEIGHT"));
                            dtTC.Columns.Add(new DataColumn("TC_STAR_RATE"));
                            dtTC.Columns.Add(new DataColumn("TC_RATING"));
                         //   dtTC.Columns.Add(new DataColumn("TC_ALT_NO"));
                            dtTC.Columns.Add(new DataColumn("NAME_PLATE"));
                            dtTC.Columns.Add(new DataColumn("SS_PLATE"));
                            dtTC.Columns.Add(new DataColumn("NAME_PLATE_FNAME"));
                            dtTC.Columns.Add(new DataColumn("SS_PLATE_FNAME"));

                            drow = dtTC.NewRow();
                            string temp = txtSerialNo.Text;
                            drow["TC_CODE"] = txtTcCode.SelectedItem.Text;
                            drow["TC_SLNO"] = temp.ToUpper();
                        //    drow["DIV_NAME"] = cmbDiv.SelectedItem.Text;
                         //   drow["DIV_ID"] = cmbDiv.SelectedValue;
                            drow["TM_NAME"] = cmbMake.SelectedItem.Text;
                            drow["MAKE_ID"] = cmbMake.SelectedValue;
                            drow["TC_CAPACITY"] = cmbCapacity.SelectedItem.Text;
                            drow["TC_MANF_DATE"] = txtManufactureDate.Text;
                            drow["LIFE_SPAN"] = txtTcLifeSpan.Text;
                            drow["TC_WARANTY_PERIOD"] = txtWarrentyPeriod.Text;
                            drow["TC_OIL_CAPACITY"] = txtOilCapacity.Text;
                            drow["TC_OIL_TYPE"] = cmbOilType.SelectedValue;
                            drow["TC_WEIGHT"] = txtWeight.Text;
                            drow["TC_STAR_RATE"] = cmbRating.SelectedValue;
                            drow["TC_RATING"] = cmbRating.SelectedItem.Text;
                          //  drow["TC_ALT_NO"] = cmbALTNo.SelectedItem.Text;
                            drow["NAME_PLATE"] = Convert.ToBase64String(FileNamePlate.FileBytes);
                            drow["SS_PLATE"] = Convert.ToBase64String(FileSSplate.FileBytes);
                            drow["NAME_PLATE_FNAME"] = FileNamePlate.FileName;
                            drow["SS_PLATE_FNAME"] = FileSSplate.FileName;
                            dtTC.Rows.Add(drow);
                            grdTCDetails.DataSource = dtTC;
                            grdTCDetails.DataBind();
                            ViewState["TC"] = dtTC;
                        }

                        else
                        {
                            ShowMsgBox("Check with Available/Pending Quantity");
                            ViewState["TC"] = dtTC;
                            return;
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
        public bool isCountCapacity(string strTcCode)
        {
            bool isCapacity = false;
            try
            {
                DataTable dtTcDetails;
                DataTable dtTcCapacityGrid;
                int Count = 0;
                dtTcDetails = (DataTable)ViewState["TC"];
                dtTcCapacityGrid = (DataTable)ViewState["TcDetailsGrid"];

                for (int i = 0; i < dtTcCapacityGrid.Rows.Count; i++)
                {
                    for (int j = 0; j < dtTcDetails.Rows.Count; j++)
                    {
                        //Taking count of number of transformers selected 
                        //if (Convert.ToString(dtTcDetails.Rows[j]["TC_CAPACITY"]) == cmbCapacity.SelectedItem.Text)
                        //{
                        //    Count++;
                        //}

                        //if (Convert.ToString(dtTcCapacityGrid.Rows[i]["CAPACITY"]) == Convert.ToString(dtTcDetails.Rows[j]["TC_CAPACITY"]) && Convert.ToString(dtTcCapacityGrid.Rows[i]["DIV_NAME"]) == Convert.ToString(dtTcDetails.Rows[j]["DIV_NAME"])
                        if (Convert.ToString(dtTcCapacityGrid.Rows[i]["CAPACITY"]) == Convert.ToString(dtTcDetails.Rows[j]["TC_CAPACITY"]) 
                            && Convert.ToString(dtTcCapacityGrid.Rows[i]["TM_NAME"]) == Convert.ToString(dtTcDetails.Rows[j]["TM_NAME"]) && Convert.ToString(dtTcCapacityGrid.Rows[i]["RATING"]) == Convert.ToString(dtTcDetails.Rows[j]["TC_RATING"]))
                        { 
                            Count++;
                        }
                    }
                    //To check whether selected transformers doesnot exceed requested number of transformers
                    if (Convert.ToString(dtTcCapacityGrid.Rows[i]["CAPACITY"]) == cmbCapacity.SelectedItem.Text && Convert.ToString(dtTcCapacityGrid.Rows[i]["TM_NAME"]) == cmbMake.SelectedItem.Text 
                        && Convert.ToString(dtTcCapacityGrid.Rows[i]["RATING"]) == cmbRating.SelectedItem.Text)

                    //&& Convert.ToString(dtTcCapacityGrid.Rows[i]["MAKE"]) == cmbMake.SelectedItem.Text && Convert.ToString(dtTcCapacityGrid.Rows[i]["RATING"])==cmbRating.SelectedItem.Text)
                    {
                        if (Convert.ToInt32(dtTcCapacityGrid.Rows[i]["PENDINGCOUNT"]) > Count)
                        {
                            isCapacity = true;
                        }

                        Count--;
                    }
                    else
                    {
                        Count = 0;
                    }
                   
                }
                return isCapacity;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return isCapacity;
            }
          
        }

        public bool CheckQuantityAvailable(string strTcCode)
        {
            bool isCapacity = false;
            try
            {
              
                DataTable dtTcCapacityGrid;
                int Count = 0;
               
                dtTcCapacityGrid = (DataTable)ViewState["TcDetailsGrid"];
                string cap = cmbCapacity.SelectedValue;
                string makename = cmbMake.SelectedItem.Text;
                string rating = cmbRating.SelectedItem.Text;

                for (int i = 0; i < dtTcCapacityGrid.Rows.Count; i++)
                {
                    
                    //To check whether selected transformers doesnot exceed requested number of transformers
                    if (Convert.ToString(dtTcCapacityGrid.Rows[i]["CAPACITY"]) == cmbCapacity.SelectedItem.Text && Convert.ToString(dtTcCapacityGrid.Rows[i]["TM_NAME"]) == cmbMake.SelectedItem.Text
                         && Convert.ToString(dtTcCapacityGrid.Rows[i]["RATING"])==cmbRating.SelectedItem.Text)
                    {
                        if (Convert.ToInt32(dtTcCapacityGrid.Rows[i]["PENDINGCOUNT"]) > Count)
                        {
                            isCapacity = true;
                        }
                    }
                }
                return isCapacity;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return isCapacity;
            }

        }

        public bool ValidateGridValue(string sTcCode,string sTCSlno)
        {
            bool bValidate = false;
            try
            {
                ArrayList objArrlist = new ArrayList();
                ArrayList objArrlistSlno = new ArrayList();

                foreach (GridViewRow row in grdTCDetails.Rows)
                {
                    objArrlist.Add(((Label)row.FindControl("lblTCCode")).Text.Trim());
                    objArrlistSlno.Add(((Label)row.FindControl("lblTCSlNo")).Text.Trim());
                }

                if (objArrlist.Contains(sTcCode))
                {
                    ShowMsgBox("Same Transformer Code Already Added");
                    DataTable dtFaultTc = (DataTable)ViewState["TC"];
                    grdTCDetails.DataSource = dtFaultTc;
                    grdTCDetails.DataBind();
                    return bValidate;
                }

                if (objArrlistSlno.Contains(sTCSlno))
                {
                    ShowMsgBox("Same Transformer Serial No Already Added");
                    DataTable dtFaultTc = (DataTable)ViewState["TC"];
                    grdTCDetails.DataSource = dtFaultTc;
                    grdTCDetails.DataBind();
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

        protected void cmdAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm() == true)
                {
                    string[] Arr = new string[3];
                    clsTcMaster objDtrCComm = new clsTcMaster();
                    objDtrCComm.sTcSlNo = txtSerialNo.Text;
                    //  objDtrCComm.sTcCode = txtTcCode.Text;
                    objDtrCComm.sTcCode = txtTcCode.SelectedItem.Text;
                    Arr = objDtrCComm.GetTcAndSrlNumDetails(objDtrCComm);
                  if (Arr[1] == "0")
                  {
                        //AddTCtoGrid(txtTcCode.Text, txtSerialNo.Text);   
                        AddTCtoGrid(objDtrCComm.sTcCode, txtSerialNo.Text);
                        if (ViewState["TC"] != null)
                      {
                          DataTable dtTcDetails = (DataTable)ViewState["TC"];
                          if (dtTcDetails.Rows.Count > 0)
                          {
                              cmdSave.Visible = true;
                              Reset();
                              Refresh();
                          }
                      }
                  }
                  else
                  {
                      ShowMsgBox(Arr[0]);                      
                  }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void Refresh()
        {
            string[] startrange = new string[grdTCDetails.Rows.Count];
            string s = string.Empty;
            try
            {
                foreach (GridViewRow row in grdTCDetails.Rows)
                {
                    for ( int i=0;i< grdTCDetails.Rows.Count;i++)
                    {
                        Label lblTCCode = row.FindControl("lblTCCode") as Label;
                        string tccode = lblTCCode.Text;
                        startrange[i] =  tccode;
                         s= s+tccode + ',';
                    }
                    s = s.Substring(0, s.Length - 1);
                }
                //Genaral.Load_Combo("select \"TCP_ID\" ,\"TCP_TC_CODE\" from \"TBLDELIVERYINSTMASTER\" inner join \"TBLDELIVERYINSTRUCTION\" on \"DIM_ID\" = \"DI_DIM_ID\" inner join \"TBLTCRANGEALLOTMENT\" on \"DI_ID\"=\"TCP_DI_ID\" and \"TCP_STATUS_M\"='0' and \"DIM_DI_NO\" ='" + txtDINo.Text + "' and \"TCP_TC_CODE\" NOT IN ("+s+")", "--Select--", txtTcCode);
                Genaral.Load_Combo("select \"TCP_ID\" ,\"TCP_TC_CODE\" from \"TBLDELIVERYINSTMASTER\" inner join \"TBLDELIVERYINSTRUCTION\" on \"DIM_ID\" = \"DI_DIM_ID\" inner join \"TBLTCRANGEALLOTMENT\" on \"DI_ID\"=\"TCP_DI_ID\" and \"TCP_STATUS_M\"='0' and \"DIM_DI_NO\" ='" + txtDINo.Text + "' ", "--Select--", txtTcCode);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public bool ValidateSave()
        {

            bool bValidate = false;

            try
            {
                if (txtDINo.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Enter the PO Number");
                    txtDINo.Focus();
                    return false;
                }
                if (txtAllotmentDate.Text.Trim() == "")
                {
                    txtAllotmentDate.Focus();
                    ShowMsgBox("Enter Purchasing Date");
                    return false;
                }
                if (txtQuantity.Text.Trim() == "")
                {
                    txtQuantity.Focus();
                    ShowMsgBox("Enter Quantity");
                    return false;
                }
                if (txtSupplier.Text.Trim() == "")
                {
                    ShowMsgBox("Select the Supplier");
                    txtSupplier.Focus();
                    return false;
                }
                if (txtQuantity.Text.Trim() == "0")
                {
                    ShowMsgBox("Enter Valid Quantity");
                    txtQuantity.Focus();
                    return false;
                }

                //if (ViewState["TC"] != null)
                //{

                //    DataTable dt = (DataTable)ViewState["TC"];
                //     DataTable dtTcDetails=(DataTable)ViewState["TcDetailsGrid"];
                //    {
                //        ShowMsgBox("Mentioned Quantity not Matching with added TC Quantity");
                //        return false;
                //    }
                //}



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

        protected void grdTCDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "remove")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    int iRowIndex = row.RowIndex;
                    Label lblTCCode = (Label)row.FindControl("lblTCCode");
                    DataTable dt = (DataTable)ViewState["TC"];
                    File.Delete(Server.MapPath("~/DTLMSDocs" + "/" + lblTCCode.Text + "~" + Convert.ToString(dt.Rows[iRowIndex]["NAME_PLATE_FNAME"])));
                    File.Delete(Server.MapPath("~/DTLMSDocs" + "/" + lblTCCode.Text + "~" + Convert.ToString(dt.Rows[iRowIndex]["SS_PLATE_FNAME"])));
                    dt.Rows[iRowIndex].Delete();
                    if (dt.Rows.Count == 0)
                    {
                        ViewState["TC"] = null;
                    }
                    else
                    {
                        ViewState["TC"] = dt;
                    }

                    grdTCDetails.DataSource = dt;
                    grdTCDetails.DataBind();
                    
                }
                if (e.CommandName == "editT")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    int iRowIndex = row.RowIndex;

                    Label lblTCCode = (Label)row.FindControl("lblTCCode");
                    Label lblTCSlNo = (Label)row.FindControl("lblTCSlNo");
                    Label lblMakeID = (Label)row.FindControl("lblMakeID");
                    Label lblDivID = (Label)row.FindControl("lblDivID");
                    Label lblCapacity = (Label)row.FindControl("lblCapacity");
                    Label lblManfDate = (Label)row.FindControl("lblManfDate");
                    Label lblLifeSpan = (Label)row.FindControl("lblLifeSpan");
                    Label lblWarrenty = (Label)row.FindControl("lblWarrenty");
                    Label lblServiceDate = (Label)row.FindControl("lblServiceDate");
                    Label lblOilCapacity = (Label)row.FindControl("lblOilCapacity");
                    Label lblOilType = (Label)row.FindControl("lblOilType");
                    Label lblWeight = (Label)row.FindControl("lblWeight");
                    Label lblTcstarRate = (Label)row.FindControl("lblTcstarRate");
                    Label lblDIno = (Label)row.FindControl("lblAltno");

                    //txtTcCode.Text = lblTCCode.Text;
                    txtTcCode.SelectedItem.Text = lblTCCode.Text;
                    txtSerialNo.Text = lblTCSlNo.Text;
                  //  cmbDiv.SelectedValue = lblDivID.Text;
                    cmbMake.SelectedValue = lblMakeID.Text;
                    cmbCapacity.SelectedItem.Text = lblCapacity.Text;
                    txtManufactureDate.Text = lblManfDate.Text;
                    txtTcLifeSpan.Text = lblLifeSpan.Text;
                    txtWarrentyPeriod.Text = lblWarrenty.Text;
                    txtOilCapacity.Text = lblOilCapacity.Text;
                    cmbOilType.SelectedValue = lblOilType.Text;
                    txtWeight.Text = lblWeight.Text;
                    cmbRating.Text = lblTcstarRate.Text;
                  //  txtDINum.Text = lblDIno.Text;

                    DataTable dt = (DataTable)ViewState["TC"];

                    File.Delete(Server.MapPath("~/DTLMSDocs" + "/" + lblTCCode.Text + "~" + Convert.ToString(dt.Rows[iRowIndex]["NAME_PLATE_FNAME"])));
                    File.Delete(Server.MapPath("~/DTLMSDocs" + "/" + lblTCCode.Text + "~" + Convert.ToString(dt.Rows[iRowIndex]["SS_PLATE_FNAME"])));
                    dt.Rows[iRowIndex].Delete();
                    if (dt.Rows.Count == 0)
                    {
                        ViewState["TC"] = null;
                    }
                    else
                    {
                        ViewState["TC"] = dt;
                    }

                    grdTCDetails.DataSource = dt;
                    grdTCDetails.DataBind();

                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdTCDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdTCDetails.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["TC"];
                grdTCDetails.DataSource = dt;
                grdTCDetails.DataBind();

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

                    Response.Redirect("~/UserRestrict.aspx", false);

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

        protected void cmdResetPO_Click(object sender, EventArgs e)
        {
            try
            {
                txtDINo.Text = string.Empty;
                hdfPOId.Value = "";
                txtAllotmentDate.Text = string.Empty;
                txtQuantity.Text = string.Empty;
                txtSupplier.Text = string.Empty;
                cmbALTNo.ClearSelection(); 
                grdAltQuantity.DataSource = null;
                grdAltQuantity.DataBind();
                txtDIId.Text = string.Empty;
                grdAltQuantity.Visible = false;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdPOQuantity_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdAltQuantity.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["TcDetailsGrid"];
                grdAltQuantity.DataSource = dt;
                grdAltQuantity.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void lnkPoDownload_Click(object sender, EventArgs e)
        {
            try
            {
                Byte[] POImage = (Byte[])ViewState["POIMAGE"];
                string sExt = (string)ViewState["POFILEXT"];
                DownloadFile(POImage, sExt,"PurchaseOrder");
            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void DownloadFile(byte[] File, string FileExt, string sFileName)
        {
            try
            {
                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "image/png";

                Response.AppendHeader("Content-Disposition", "attachment; filename=" + sFileName + FileExt);

                Response.BinaryWrite(File);
                Response.Flush();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void lnkDI_Click(object sender, EventArgs e)
        {
            try
            {
                Byte[] DIImage = (Byte[])ViewState["DIIMAGE"];
                string sExt = (string)ViewState["DIFILEXT"];
                DownloadFile(DIImage, sExt,"Delivery");
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}