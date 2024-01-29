using IIITS.DTLMS.BL;
using IIITS.DTLMS.BL.POFlow;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.POFlow
{
    public partial class PMCPoMasterCreate : System.Web.UI.Page
    {
        string strFormCode = "PoMaster";
        public int gridRowId;
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
                Update.Visible = false;
               // UpdatePO.Visible = false;
                lblMessage.Text = string.Empty;
                objSession = (clsSession)Session["clsSession"];
                Form.DefaultButton = btnSave.UniqueID;
                txtDelivery.Attributes.Add("readonly", "readonly");
                txtDate.Attributes.Add("readonly", "readonly");
                //ManufactureCalender.EndDate = System.DateTime.Now;
                //DeliveryCalendar.StartDate = System.DateTime.Now;
                if (!IsPostBack)
                {
                    LoadComboField();
                    CheckAccessRights(Constants.CheckAccessRights.CheckAccessRightsReadOnly); //"4"
                   //Genaral.Load_Combo("select \"DM_ID\",\"DM_NUMBER\" from \"TBLDWAMASTER\" where \"DM_STATUS\"='A' and to_char(\"DM_EXTENDED_UPTO\",'dd/mm/yyyy')>=to_char(now(),'dd/mm/yyyy') ORDER BY \"DM_ID\" ", "--Select--", cmbDWANo);
                    Genaral.Load_Combo("select \"DM_ID\",\"DM_NUMBER\" from \"TBLDWAMASTER\" where \"DM_STATUS\"='A' and to_char(\"DM_EXTENDED_UPTO\",'yyyy/mm/dd')>=to_char(now(),'yyyy/mm/dd') ORDER BY \"DM_ID\" ", "--Select--", cmbDWANo);

                    if (Request.QueryString["QryPoId"] != null && Request.QueryString["QryPoId"].ToString() != "")
                    {
                        Genaral.Load_Combo("select \"DM_ID\",\"DM_NUMBER\" from \"TBLDWAMASTER\" ORDER BY \"DM_ID\" ",cmbDWANo);

                        txtPoId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryPoId"]));
                        string TotQnty = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryPoQnty"]));
                        txtDMIdView.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryDMId"]));
                        cmbDWANo.SelectedValue =  txtDMIdView.Text;
                        cmbDWANo.Enabled = false;
                        LoadPoDetails(txtPoId.Text);
                        Create.Visible = false;
                       // CreatePO.Visible = false;
                        Update.Visible = true;
                      //  UpdatePO.Visible = true;
                        DivUpload.Visible = true;
                        cmbSupplier.Enabled = false;
                    }
                    txtDate.Attributes.Add("onblur", "return ValidateDate(" + txtDate.ClientID + ");");
                }
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

                strQry = "SELECT \"TM_ID\",\"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE (\"TM_STATUS\"='A' AND ";
                strQry += " TO_CHAR(COALESCE (\"TM_EFFECT_FROM\",NOW()),'YYYYMMDD') <= TO_CHAR(NOW(),'YYYYMMDD'))";
                strQry += " OR (\"TM_STATUS\"='D' AND  TO_CHAR(\"TM_EFFECT_FROM\",'YYYYMMDD') >= TO_CHAR(NOW(),'YYYYMMDD')) ORDER BY \"TM_NAME\"";
                Genaral.Load_Combo(strQry, "-Select-", ddlMake);
                Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='SR' ORDER BY \"MD_ORDER_BY\" ", "-Select-", cmbRating);
                Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='C' ORDER BY \"MD_ORDER_BY\" ", "--Select--", ddlCapacity);
                Genaral.Load_Combo("select \"TS_ID\",\"TS_NAME\" FROM \"TBLTRANSSUPPLIER\" WHERE \"TS_STATUS\"='A' ORDER BY \"TS_NAME\"", "--Select--", cmbSupplier);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void GetDWADetails(object sender, EventArgs e)
        {
            try
            {

                ClsPMCPoMaster objPo = new ClsPMCPoMaster();
                string flag = string.Empty;
                DataTable dt = objPo.GetLECExpiryDate(cmbDWANo.SelectedValue);
                if(dt.Rows.Count>0)
                {
                    flag = Convert.ToString(dt.Rows[0]["lec_flag"]);
                    if(flag!="1")
                    {

                      string LECNo=  objPo.getLECNo(cmbDWANo.SelectedValue);
                        //string msg = "LEC date got expired for DWA No as " + cmbDWANo.SelectedItem + ", Please extend the LEC expiry Date or it is not in Active.";
                        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID, "alert('" + msg + "'); location.href='PMCPoMaterView.aspx';", true);
                        string msg = "LEC date got expired for LEC Name as " + LECNo + ", Please extend the LEC expiry Date or The LEC No is Inactive.";
                        ShowMsgBox(msg);
                        cmbDWANo.SelectedIndex = 0;
                        Reset();
                        return;

                    }
                }
                

                objPo.DWAId = cmbDWANo.SelectedValue;
                objPo.GetDWADetailstoPO(objPo);

                txtworkName.Text = objPo.WorkName;
                txtDWAAmount.Text = objPo.DWAAmount;
                txtLECNo.Text = objPo.LECNo;
                txtDWADate.Text = objPo.DWADate;
                txtDWAExpiryDate.Text = objPo.DWAExpiryDate;
                txtAvlAmount.Text = objPo.AvlAmount;
                txtLECName.Text = objPo.LECName;
                

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();

                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void ValidatePODelShdlDate(object sender, EventArgs e)
        {
            try
            {
                DateTime DWADate = DateTime.Now;
                DateTime DWAExpiryDate = DateTime.Now;
                DateTime PoDate = DateTime.Now;
                if (txtDWADate.Text.Trim() != "")
                {
                    DWADate = DateTime.ParseExact(txtDWADate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (txtDWAExpiryDate.Text.Trim() != "")
                {
                    DWAExpiryDate = DateTime.ParseExact(txtDWAExpiryDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (txtDate.Text.Trim() != "")
                {
                    PoDate = DateTime.ParseExact(txtDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (txtDWADate.Text.Trim() == "")
                {
                    cmbDWANo.Focus();
                    ShowMsgBox("Please Select DWA NO");
                    txtDate.Text = string.Empty;
                    return;
                }
                if (!(PoDate >= DWADate && PoDate <= DWAExpiryDate))
                {
                    txtDate.Focus();
                    ShowMsgBox("PO Date Should Greater Than or equal to DWA Date and Less Than or equal to DWA Expiry Date");
                    txtDate.Text = string.Empty;
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();

                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void ValidateDelShdlDate(object sender, EventArgs e)
        {
            try
            {
                DateTime DWADate=DateTime.Now;
                DateTime DWAExpiryDate= DateTime.Now;
                DateTime PoDate= DateTime.Now;
                DateTime DeliveryDate= DateTime.Now;
                if (txtDWADate.Text.Trim()!="")
                {
                     DWADate = DateTime.ParseExact(txtDWADate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (txtDWAExpiryDate.Text.Trim() != "")
                {
                     DWAExpiryDate = DateTime.ParseExact(txtDWAExpiryDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (txtDate.Text.Trim() != "")
                {
                     PoDate = DateTime.ParseExact(txtDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (txtDelivery.Text.Trim() != "")
                {
                     DeliveryDate = DateTime.ParseExact(txtDelivery.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if(txtDate.Text.Trim()=="")
                {
                    txtDate.Focus();
                    ShowMsgBox("Please Select PO Date");
                    txtDelivery.Text = string.Empty;
                    return;
                }
                if (!(DeliveryDate >= PoDate && DeliveryDate <= DWAExpiryDate))
                {
                    txtDelivery.Focus();
                    ShowMsgBox("Delivery Scheduled Date Should Greater Than or equal to PO Date and Less Than or equal to DWA Expiry Date");
                    txtDelivery.Text = string.Empty;
                    return;
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();

                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void CheckPOAmount(object sender, EventArgs e)
        {
            try
            {
                double AvlAmt = Convert.ToDouble(txtAvlAmount.Text);
                double POAmt = Convert.ToDouble(txtRate.Text);
                if (txtRate.Text.Trim().StartsWith("."))
                {
                    txtRate.Focus();
                    ShowMsgBox("Please Enter valid PO Amount (eg:111111.00)");
                    return ;
                }
                if (AvlAmt==0.00)
                {
                    ShowMsgBox("DWA Available Amount is 0, can not create PO");
                    txtRate.Text = string.Empty;
                    return;
                }
                if (AvlAmt < POAmt)
                {
                    ShowMsgBox("Please Enter valid Amount, PO Amount Should Be Less Than or Equal to DWA Available Amount");
                    txtRate.Text = string.Empty;
                    return;
                }
                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();

                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadPoDetails(string strPoId)
        {
            try
            {
                ClsPMCPoMaster objPoMaster = new ClsPMCPoMaster();
                objPoMaster.DWAId = cmbDWANo.SelectedValue;
                objPoMaster.GetDWADetailstoPO(objPoMaster);
                
                txtworkName.Text = objPoMaster.WorkName;
                txtDWAAmount.Text = objPoMaster.DWAAmount;
                txtLECNo.Text = objPoMaster.LECNo;
                txtDWADate.Text = objPoMaster.DWADate;
                txtDWAExpiryDate.Text = objPoMaster.DWAExpiryDate;
                txtAvlAmount.Text = objPoMaster.AvlAmount;
                txtLECName.Text = objPoMaster.LECName;

                objPoMaster.sPoId = Convert.ToString(strPoId);
                objPoMaster.GetPoDetails(objPoMaster);
                txtPoNumber.Text = objPoMaster.sPoNo;
                txtDate.Text = objPoMaster.sDate;
                txtRate.Text = objPoMaster.sPoRate;
                cmbSupplier.SelectedValue = objPoMaster.sSupplierId;
                txtPoNumber.Enabled = false;
                txtDate.Enabled = false;
                txtRate.Enabled = false;
                txtDelivery.Text = objPoMaster.sDeliveryDate;
                LoadTcCapacity(txtPoId.Text);
                BindgridView(SFTPmainfolder, sUserName, sPassword);

               DataTable dt= objPoMaster.GetDICreateOnPO(objPoMaster.sPoId);
                if(dt.Rows.Count>0)
                {
                    btnSave.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void cmdAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();


                DateTime DWADate = DateTime.ParseExact(txtDWADate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime DWAExpiryDate = DateTime.ParseExact(txtDWAExpiryDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime PoDate = DateTime.ParseExact(txtDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime DeliveryDate = DateTime.ParseExact(txtDelivery.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (!(PoDate >= DWADate && PoDate <= DWAExpiryDate))
                {
                    txtDate.Focus();
                    ShowMsgBox("PO Date Should Greater Than DWA Date and Less Than DWA Expiry Date");
                    return;
                }
                if (!(DeliveryDate >= PoDate && DeliveryDate <= DWAExpiryDate))
                {
                    txtDelivery.Focus();
                    ShowMsgBox("Delivery Scheduled Date Should Greater Than PO Date and Less Than DWA Expiry Date");
                    return;
                }


                ClsPMCPoMaster objPoMaster = new ClsPMCPoMaster();
                objPoMaster.sPoId = Convert.ToString(txtPoId.Text);
                if (objPoMaster.sPoId != "" && objPoMaster.sPoId != null)
                {
                    objPoMaster.sCapacity = ddlCapacity.SelectedItem.Text;
                    objPoMaster.sTcMake = ddlMake.SelectedValue;
                    dt1 = objPoMaster.LoadDeliveredCount(objPoMaster);
                    if (dt1.Rows.Count > 0)
                    {
                        int delivered = Convert.ToInt32(dt1.Rows[0]["DELIVERED"]);
                        int Quantty = Convert.ToInt32(txtQuantity.Text);

                        if (Quantty < delivered)
                        {
                            string Msg = Convert.ToString(delivered);
                            ShowMsgBox("This Capacity Already Delivered To Some Store  Quantity " + Msg + ", So You Can`t Reduce The Count Below  :" + Msg + " For Reference DI Number:" + Convert.ToString(dt1.Rows[0]["DI_NO"]));
                            return;
                        }
                    }
                }

                if (ViewState["dt"] != null)
                {
                    dt = (DataTable)ViewState["dt"];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (ddlCapacity.SelectedItem.Text == Convert.ToString(dt.Rows[i]["PMC_PB_CAPACITY"]) && ddlMake.SelectedItem.Text == Convert.ToString(dt.Rows[i]["PMC_PB_MAKE"])
                            && cmbRating.SelectedValue == Convert.ToString(dt.Rows[i]["PMC_PB_STARRATE"]))
                        {
                            ShowMsgBox("Capacity-MakeName-Star Rate Combination Already Added");
                            return;
                        }
                    }
                }

                if (Session["FileUpload"] == null && fupdDoc.HasFile)
                {
                    Session["FileUpload"] = fupdDoc;
                    lblFilename.Text = fupdDoc.FileName; // get the name 
                    fupdDoc.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + fupdDoc.FileName));

                    fupdDoc = (FileUpload)Session["FileUpload"];
                }
                else if (Session["FileUpload"] != null && (!fupdDoc.HasFile))
                {

                    fupdDoc = (FileUpload)Session["FileUpload"];
                    lblFilename.Text = fupdDoc.FileName;
                }
                else if (fupdDoc.HasFile)
                {
                    fupdDoc.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + fupdDoc.FileName));
                    Session["FileUpload"] = fupdDoc;
                    lblFilename.Text = fupdDoc.FileName;
                }

                if (ViewState["dt"] == null)
                {
                    dt.Columns.Add("PMC_PO_ID");
                    dt.Columns.Add("PMC_PO_NO");
                    dt.Columns.Add("PMC_PB_CAPACITY");
                    dt.Columns.Add("PMC_PB_CAPACITY_ID");
                    dt.Columns.Add("PMC_PB_MAKE");
                    dt.Columns.Add("PMC_MAKE_ID");
                    dt.Columns.Add("PMC_PB_STARRATE");
                    dt.Columns.Add("PMC_PB_STAR_NAME");
                    dt.Columns.Add("PMC_PB_QUANTITY");
                }
                else
                {
                    dt = (DataTable)ViewState["dt"];
                }
                DataRow dRow = dt.NewRow();
                int qnty = Convert.ToInt32(txtQuantity.Text);

                dRow["PMC_PB_QUANTITY"] = qnty;
                if (Convert.ToString(dRow["PMC_PB_QUANTITY"]) == "0")
                {
                    ShowMsgBox("Quantity Should Not be Zero");
                    return;
                }
                dRow["PMC_PB_CAPACITY"] = ddlCapacity.SelectedItem.Text;
                dRow["PMC_PB_CAPACITY_ID"] = ddlCapacity.SelectedValue;
                dRow["PMC_PB_MAKE"] = ddlMake.SelectedItem.Text;
                dRow["PMC_MAKE_ID"] = ddlMake.SelectedValue;
                dRow["PMC_PO_NO"] = txtPoNumber.Text;
                dRow["PMC_PB_STARRATE"] = cmbRating.SelectedValue;
                dRow["PMC_PB_STAR_NAME"] = cmbRating.SelectedItem.Text;

                if (ViewState["gridRowId"] != null)
                {
                    int i = Convert.ToInt32(ViewState["gridRowId"]);
                    dt.Rows[i].SetField("PMC_PB_QUANTITY", qnty);
                    dt.Rows[i].SetField("PMC_PB_CAPACITY", ddlCapacity.SelectedItem.Text);
                    dt.Rows[i].SetField("PMC_PB_CAPACITY_ID", ddlCapacity.SelectedValue);
                    dt.Rows[i].SetField("PMC_MAKE_ID", ddlMake.SelectedValue);
                    dt.Rows[i].SetField("PMC_PB_STARRATE", cmbRating.SelectedValue);
                    dt.Rows[i].SetField("PMC_PB_STAR_NAME", cmbRating.SelectedItem.Text);
                    dt.Rows[i].SetField("PMC_PO_NO", txtPoNumber.Text);

                    ViewState["gridRowId"] = null;
                    ViewState["dt"] = dt;
                    LoadCapacity(dt);
                }
                else
                {
                    dt.Rows.Add(dRow);
                    ViewState["dt"] = dt;
                    LoadCapacity(dt);
                }
                ddlCapacity.Enabled = true;
                ddlMake.Enabled = true;
                cmbRating.Enabled = true;
                cmbSupplier.Enabled = false;
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

        public void LoadTcCapacity(string strIndentId)
        {
            DataTable dtTcCapacity = new DataTable();
            try
            {
                ClsPMCPoMaster objPoMaster = new ClsPMCPoMaster();
                objPoMaster.sPoId = Convert.ToString(strIndentId);
                dtTcCapacity = objPoMaster.LoadCapacityGrid(objPoMaster);
                grdPoMaster.DataSource = dtTcCapacity;
                grdPoMaster.DataBind();
                btnSave.Text = "Update";
                ViewState["dt"] = dtTcCapacity;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void LoadCapacity(DataTable dt)
        {
            try
            {
                grdPoMaster.DataSource = dt;
                grdPoMaster.DataBind();
                grdPoMaster.Visible = true;
                txtQuantity.Text = string.Empty;
                ddlCapacity.SelectedIndex = 0;
                ddlMake.SelectedIndex = 0;
                cmbRating.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }



        protected void btnSave_Click1(object sender, EventArgs e)
        {
            try
            {
                //to check whether capacity and quantity are added
                if (ViewState["dt"] != null)
                {
                    //Check AccessRights
                    bool bAccResult;
                    if (btnSave.Text == "Update")
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
                    if (ddlCapacity.SelectedValue != "0" && txtQuantity.Text != "" && ddlMake.SelectedValue != "0")
                    {
                        ddlCapacity.Focus();
                        txtQuantity.Focus();
                        ddlMake.Focus();
                        ShowMsgBox("Please Add Make, Capacity, Quantity and Rating ");
                        return;
                    }
                    SavePoMaster();
                }
                else
                {
                    ShowMsgBox("Please Add Make, Capacity, Quantity and Rating ");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void DownloadFile(object sender, EventArgs e)
        {

            string fileName = (sender as LinkButton).CommandArgument;

            try
            {
                bool status = false;
                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["VirtualDirectoryPMCDocs"]);

                string PoNo = Regex.Replace(txtPoNumber.Text, @"[^0-9a-zA-Z]+", "");
                clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);

                string path = SFTPmainfolder + "PMC_PO_DOCS/" + PoNo + "/" + fileName;
                RegisterStartupScript("Print", "<script>window.open('" + path + "','_blank')</script>");
            }
            catch (WebException ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void DownloadFiledwnld(object sender, EventArgs e)
        {
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
                    string SFTPmainfolder = Convert.ToString(ConfigurationSettings.AppSettings["VirtualDirectoryPMCDocs"]);

                    string PoNo = Regex.Replace(txtPoNumber.Text, @"[^0-9a-zA-Z]+", "");
                    clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);

                    string url = SFTPmainfolder + "PMC_PO_DOCS/" + PoNo + "/" + fileName1;
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
        private string getFilename(string hreflink)
        {
            Uri uri = new Uri(hreflink);
            string filename = System.IO.Path.GetFileName(uri.LocalPath);
            return filename;
        }
        public DataTable BindgridView(string FtpServer, string username, string password)
        {
            DataTable dtFiles = new DataTable();
            try
            {
                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);

                string PoNo = Regex.Replace(txtPoNumber.Text, @"[^0-9a-zA-Z]+", "");
                FtpServer += "PMC_PO_DOCS/" + PoNo;
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

        public void SavePoMaster()
        {
            ClsPMCPoMaster objPoMaster = new ClsPMCPoMaster();
            DataTable dt;
            string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
            string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);

            clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);
            bool Isuploaded;
            string sMainFolderName = "PMC_PO_DOCS";

            byte[] Buffer = new byte[100];
            try
            {
                if (Session["FileUpload"] != null && (!fupdDoc.HasFile))
                {
                    fupdDoc = (FileUpload)Session["FileUpload"];
                    lblFilename.Text = fupdDoc.FileName;
                }
                else
                {
                    if (fupdDoc.PostedFile.FileName != null && fupdDoc.PostedFile.FileName != "")
                    {
                        fupdDoc.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + fupdDoc.FileName));
                    }
                    else
                    {
                        ShowMsgBox("Please upload the Purchase Order Document.");
                        fupdDoc.Focus();
                        return;
                    }
                }
                if (ValidateForm() == true)
                {
                    string strpath = System.IO.Path.GetExtension(fupdDoc.FileName);
                    string filename = Path.GetFileName(fupdDoc.PostedFile.FileName);
                  string dwaflag=  objPoMaster.getDWAExpiry(Convert.ToInt32( cmbDWANo.SelectedValue));

                    if(dwaflag!="1")
                    {

                        ShowMsgBox("Selected DWA No is Expired or The DWA No is Inactive.");
                        cmbDWANo.SelectedIndex = 0;
                        Reset();
                        return;
                    }
                    string flag = string.Empty;
                    DataTable dt1 = objPoMaster.GetLECExpiryDate(cmbDWANo.SelectedValue);
                    if (dt1.Rows.Count > 0)
                    {
                        flag = Convert.ToString(dt1.Rows[0]["lec_flag"]);
                        if (flag != "1")
                        {

                            string LECNo = objPoMaster.getLECNo(cmbDWANo.SelectedValue);
                            string msg = "LEC date got expired for LEC Name as " + LECNo + ", Please extend the LEC expiry Date or The LEC No is Inactive.";
                            ShowMsgBox(msg);
                            cmbDWANo.SelectedIndex = 0;
                            Reset();
                            return;

                        }
                    }

                    string[] Arr = new string[2];
                    dt = (DataTable)ViewState["dt"];
                    objPoMaster.sCrBy = objSession.UserId;
                    objPoMaster.DWAId = Convert.ToString(cmbDWANo.SelectedValue);
                    objPoMaster.sDate = txtDate.Text;
                    objPoMaster.sPoNo = txtPoNumber.Text;
                    objPoMaster.sSupplierId = cmbSupplier.SelectedValue;
                    objPoMaster.sPoRate = txtRate.Text;
                    objPoMaster.sPoId = txtPoId.Text;
                    objPoMaster.sPoDlvrdate = txtDelivery.Text;
                    objPoMaster.ddtCapacityGrid = dt;
                    objPoMaster.sFileName = filename;
                    string PoNo = Regex.Replace(objPoMaster.sPoNo, @"[^0-9a-zA-Z]+", "");
                    objPoMaster.sFileExtension = SFTPPath + "/" + SFTPmainfolder + sMainFolderName + "/" + PoNo + "/" + filename;
                    Arr = objPoMaster.SavePoMaster(objPoMaster, Buffer);
                    
                    if (Arr[1].ToString() == "0")
                    {
                        if (fupdDoc.PostedFile != null)
                        {
                            if (fupdDoc.PostedFile.ContentLength != 0)
                            {
                                string strExt = filename.Substring(filename.LastIndexOf('.') + 1);
                                
                                string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FileFormat"]);
                                string sAnxFileExt = System.IO.Path.GetExtension(fupdDoc.FileName).ToString().ToLower();
                                sAnxFileExt = ";" + sAnxFileExt.Remove(0, 1) + ";";

                                if (!sFileExt.Contains(sAnxFileExt))
                                {
                                    ShowMsgBox("Invalid File Format");
                                    return;
                                }
                                string sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + fupdDoc.FileName);
                                
                                if (File.Exists(sDirectory))
                                {
                                    bool IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName);
                                    if (IsExists == false)
                                    {
                                        objFtp.createDirectory(SFTPmainfolder + sMainFolderName);
                                    }
                                    IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName + "/" + PoNo);
                                    if (IsExists == false)
                                    {

                                        objFtp.createDirectory(SFTPmainfolder + sMainFolderName + "/" + PoNo);
                                    }
                                    Isuploaded = objFtp.upload(SFTPmainfolder + sMainFolderName + "/" + PoNo + "/", filename, sDirectory);
                                    if (Isuploaded == true & File.Exists(sDirectory))
                                    {
                                        File.Delete(sDirectory);
                                        sDirectory = PoNo + "/" + filename;
                                       // ShowMsgBox(Arr[0]);
                                    }
                                }
                                Session["FileUpload"] = null;
                            }
                        }
                        else
                        {
                            ShowMsgBox("Please upload the File");
                            return;
                        }
                    }
                    
                    if (Arr[1].ToString() == "0")
                    {
                        txtPoId.Text = objPoMaster.sPoId;
                        btnSave.Text = "Update";
                      //  ShowMsgBox(Arr[0]);
                      //  string msg = "LEC date got expired for DWA No as " + cmbDWANo.SelectedItem + ", Please extend the LEC expiry Date or it is not in Active.";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID, "alert('" + Arr[0] + "'); location.href='PMCPoMaterView.aspx';", true);
                        Reset();
                        return;
                    }
                    else
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
        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "ASC"; }
            set { ViewState["SortDirection"] = value; }


        }
        private string GridViewSortExpression
        {
            get { return ViewState["SortExpression"] as string ?? string.Empty; }
            set { ViewState["SortExpression"] = value; }
        }

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

        private void Reset()
        {
            try
            {
                cmbDWANo.SelectedIndex = 0;
                txtworkName.Text = string.Empty;
                txtDWAAmount.Text = string.Empty;
                txtLECNo.Text = string.Empty;
                txtDWADate.Text = string.Empty;
                txtDWAExpiryDate.Text = string.Empty;
                txtAvlAmount.Text = string.Empty;
                txtLECName.Text = string.Empty;

                txtPoId.Text = string.Empty;
                txtPoNumber.Text = string.Empty;
                ddlMake.SelectedIndex = 0;
                txtDate.Text = string.Empty;
                txtDelivery.Text = string.Empty;
                btnSave.Text = "Save";
                txtQuantity.Text = string.Empty;
                ddlCapacity.SelectedIndex = 0;
                txtQuantity.Text = string.Empty;
                grdPoMaster.Visible = false;
                ViewState["dt"] = null;
                lblMessage.Text = string.Empty;
                txtPoNumber.Enabled = true;
                txtDate.Enabled = true;
                txtDelivery.Enabled = true;
                txtRate.Text = string.Empty;
                cmbSupplier.SelectedIndex = 0;
                cmbSupplier.Enabled = true;
                txtRate.Text = string.Empty;
                txtRate.Enabled = true;
                Create.Visible = true;
                btnPoView.Visible = true;
                cmbRating.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
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

        protected void cmdClose_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("PMCPoMaterView.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    
        protected void grdPoMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            ClsPMCPoMaster objPoMaster = new ClsPMCPoMaster();
            try
            {
                if (e.CommandName == "Remove")
                {
                    DataTable dt = (DataTable)ViewState["dt"];
                    DataTable DICount;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                        Label lblCapacity = (Label)row.FindControl("lblCapacity");
                        Label lblRating = (Label)row.FindControl("lblRatingId");
                        Label lblQuantity = (Label)row.FindControl("lblQuantity");
                        Label lblMake = (Label)row.FindControl("lblMake");
                        Label lblMakeId = (Label)row.FindControl("lblMakeId");
                        Label lblPoId = (Label)row.FindControl("lblPOId");

                        //to remove selected Capacity from grid
                        if (lblCapacity.Text == Convert.ToString(dt.Rows[i]["PMC_PB_CAPACITY"]) && 
                            lblRating.Text == Convert.ToString(dt.Rows[i]["PMC_PB_STARRATE"]) && 
                            lblQuantity.Text == Convert.ToString(dt.Rows[i]["PMC_PB_QUANTITY"]) && 
                            lblMake.Text == Convert.ToString(dt.Rows[i]["PMC_PB_MAKE"]))
                        {
                            objPoMaster.sPoId = lblPoId.Text;
                            objPoMaster.sCapacity = lblCapacity.Text;
                            objPoMaster.sTcMake = lblMakeId.Text;
                            DICount = objPoMaster.LoadDeliveredCount(objPoMaster);
                            if (DICount.Rows.Count > 0)
                            {
                                int delivered = Convert.ToInt32(DICount.Rows[0]["DELIVERED"]);

                                string Msg = Convert.ToString(delivered);
                                ShowMsgBox("This Capacity Already Delivered To Some Store  Quantity " + Msg + ", So You Can`t Delete This Record , For Reference DI Number:" + Convert.ToString(DICount.Rows[0]["PMC_DI_NO"]));
                                return;
                            }
                            else
                            {
                                dt.Rows[i].Delete();
                                dt.AcceptChanges();
                            }
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["dt"] = dt;
                    }
                    else
                    {
                        ViewState["dt"] = null;
                    }
                    LoadCapacity(dt);
                }

                if (e.CommandName == "EditQNTY")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    DataTable dt = (DataTable)ViewState["dt"];
                    DataTable DICount;
                    int sRowIndex = row.RowIndex;
                    Label lblCapacity = (Label)row.FindControl("lblCapacity");
                    Label lblCapacityid = (Label)row.FindControl("lblCapacityID");
                    Label lblRating = (Label)row.FindControl("lblRatingId");
                    Label lblQuantity = (Label)row.FindControl("lblQuantity");
                    Label lblMakeid = (Label)row.FindControl("lblMakeId");
                    Label lblMake = (Label)row.FindControl("lblMake");
                    Label lblPoId = (Label)row.FindControl("lblPOId");
                    if (lblCapacity.Text == Convert.ToString(dt.Rows[sRowIndex]["PMC_PB_CAPACITY"]) && lblRating.Text == Convert.ToString(dt.Rows[sRowIndex]["PMC_PB_STARRATE"]) && lblQuantity.Text == Convert.ToString(dt.Rows[sRowIndex]["PMC_PB_QUANTITY"]) && lblMake.Text == Convert.ToString(dt.Rows[sRowIndex]["PMC_PB_MAKE"]))
                    {
                        objPoMaster.sPoId = lblPoId.Text;
                        objPoMaster.sCapacity = lblCapacity.Text;
                        objPoMaster.sTcMake = lblMakeid.Text;
                        DICount = objPoMaster.LoadDeliveredCount(objPoMaster);
                        if (DICount.Rows.Count > 0)
                        {
                            ddlCapacity.Enabled = false;
                            ddlMake.Enabled = false;
                            cmbRating.Enabled = false;
                        }

                    }
                    ddlCapacity.SelectedValue = lblCapacityid.Text;
                    ddlMake.SelectedValue = lblMakeid.Text;
                    cmbRating.SelectedValue = lblRating.Text; ;
                    txtQuantity.Text = lblQuantity.Text;

                    dt.Rows[sRowIndex].Delete();
                    dt.AcceptChanges();
                    if (dt.Rows.Count == 0)
                    {
                        ViewState["dt"] = null;
                    }
                    else
                    {
                        ViewState["dt"] = dt;
                    }
                    grdPoMaster.DataSource = dt;
                    grdPoMaster.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdPoMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtTcCapacity = new DataTable();
                grdPoMaster.PageIndex = e.NewPageIndex;
                dtTcCapacity = (DataTable)ViewState["dt"];
                LoadCapacity(dtTcCapacity);
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
                objApproval.sFormName = "PMCPOMaster";
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

        bool ValidateForm()
        {
            bool bValidate = false;
            string strpath = System.IO.Path.GetExtension(fupdDoc.FileName);

            string filename = Path.GetFileName(fupdDoc.PostedFile.FileName);
            if(cmbDWANo.SelectedIndex==0)
            {
                cmbDWANo.Focus();
                ShowMsgBox("Please Enter select DWA No");
                return false;
            }
            if (txtPoNumber.Text.Trim().StartsWith("."))
            {
                txtPoNumber.Focus();
                ShowMsgBox("Please Enter valid PO Number");
                return false;
            }
            if (txtRate.Text.Trim().StartsWith("."))
            {
                txtRate.Focus();
                ShowMsgBox("Please Enter valid PO Amount (eg:111111.00)");
                return false;
            }
            DateTime DWADate = DateTime.ParseExact(txtDWADate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime PoDate = DateTime.ParseExact(txtDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime DeliveryDate = DateTime.ParseExact(txtDelivery.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (PoDate > DeliveryDate)
            {
                txtDelivery.Focus();
                ShowMsgBox("Delivery Scheduled Date Should Greater Than Po Date");
                return false;
            }
            if (DWADate > PoDate)
            {
                txtDate.Focus();
                ShowMsgBox("PO Date Should Greater Than DWA Date");
                return false;
            }
            if (txtRate.Text.Trim() != "")
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtRate.Text, "^(\\d{1,12})?(\\.\\d{1,2})?$"))
                {
                    txtRate.Focus();
                    ShowMsgBox("Please Enter valid PO Amount (eg:111111.00)");
                    return false;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(txtRate.Text, "[-+]?[0-9]{0,11}\\.?[0-9]{0,2}"))
                {
                    txtRate.Focus();
                    ShowMsgBox("Please Enter valid PO Amount (eg:111111.00)");
                    return false;
                }
                if(txtRate.Text.StartsWith("0"))
                {
                    txtRate.Focus();
                    ShowMsgBox("Please Enter valid PO Amount (eg:111111.00)");
                    return false;
                }
                if(txtRate.Text.Trim()=="0"|| txtRate.Text.Trim() == "0.0"||txtRate.Text.Trim() == "0.00" || txtRate.Text.Trim() == "00")
                {
                    txtRate.Focus();
                    ShowMsgBox("Please Enter valid PO Amount (eg:111111.00)");
                    return false;
                }
            }
            if (fupdDoc.FileName == "")
            {
                ShowMsgBox("Please Upload Edited Purchase Order Note");
                fupdDoc.Focus();
                return false;
            }
            if (fupdDoc.PostedFile != null)
            {
                if (fupdDoc.PostedFile.ContentLength != 0)
                {
                    string strExt = filename.Substring(filename.LastIndexOf('.') + 1);
                    string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FileFormat"]);
                    string sAnxFileExt = System.IO.Path.GetExtension(fupdDoc.FileName).ToString().ToLower();
                    sAnxFileExt = ";" + sAnxFileExt.Remove(0, 1) + ";";
                    if (!sFileExt.Contains(sAnxFileExt))
                    {
                        ShowMsgBox("Invalid File Format");
                        return false;
                    }
                }
            }
            bValidate = true;
            return bValidate;
        }

        protected void lnkDwnld_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable DtPODoc = new DataTable();
                ClsPMCPoMaster objPomaster = new ClsPMCPoMaster();
                DtPODoc = objPomaster.GetPODoc(txtPoId.Text);

                Byte[] bytes = (Byte[])DtPODoc.Rows[0]["PO_DOC"];
                string sExtension = DtPODoc.Rows[0]["PO_DOC_EXT"].ToString();
                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "image/png";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + txtPoNumber.Text + sExtension);
                Response.BinaryWrite(bytes);
                Response.Flush();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}