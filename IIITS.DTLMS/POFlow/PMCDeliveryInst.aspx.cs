
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Net;
using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;
using Renci.SshNet;
using System.Configuration;
using IIITS.DTLMS.BL;
using IIITS.DTLMS.BL.POFlow;
using System.Reflection;

namespace IIITS.DTLMS.POFlow
{
    public partial class PMCDeliveryInst : System.Web.UI.Page
    {
        public string strFormCode = "DeliveryInst";
        clsSession objSession;
        string FileServerPath = Convert.ToString(ConfigurationManager.AppSettings["EstimatioinVirtualPath"]);
        string FileUploadloadPath = Convert.ToString(ConfigurationManager.AppSettings["CircularsVirtualPath"]);

        string UserName = Convert.ToString(ConfigurationManager.AppSettings["FTP_USER"]);
        string Password = Convert.ToString(ConfigurationManager.AppSettings["FTP_PASS"]);

        string SFTPPath = Convert.ToString(ConfigurationManager.AppSettings["SFTPPath"]);

        string SFTPmainfolder = Convert.ToString(ConfigurationManager.AppSettings["SFTPmainfolder"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
            {
                Response.Redirect("~/Login.aspx", false);
            }
            else
            {
                objSession = (clsSession)Session["clsSession"];

                if (CheckAccessRights(Constants.CheckAccessRights.CheckAccessRightsAll))
                {
                    txtDueDate.Attributes.Add("readonly", "readonly");
                    txtDIDate.Attributes.Add("readonly", "readonly");

                    if (!IsPostBack)
                    {
                        LoadSearchWindow();

                        if (Request.QueryString["QryPMCDIid"] != null && Convert.ToString(Request.QueryString["QryPMCDIid"]) != "")
                        {
                            string DI_id = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryPMCDIid"]));
                            string DI_No = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryPMCDiNo"]));
                            LoadDeliveredIst(DI_No);
                            Create.Visible = true;
                            CreateDI.Visible = true;
                            fupFile.Enabled = false;
                            btnSave.Visible = false;
                            //btnUpdate.Visible = false;
                            btnReset.Visible = false;
                            cmdAdd.Enabled = false;
                            txtDINumber.Enabled = false;
                            txtDueDate.Enabled = false;
                            cmbMake.Enabled = false;
                            cmbStore.Enabled = false;
                            txtDIDate.Enabled = false;
                            cmbCapacity.Enabled = false;
                            txtQuantity.Enabled = false;
                            cmdAdd.Enabled = false;
                            cmbRating.Enabled = false;
                            txtPoNumber.Enabled = false;
                            //btnUpdate.Visible = true;
                            hdfviewstatusflag.Value = "1";
                            grdDelivery.HeaderRow.Cells[20].Enabled = false;
                            grdDelivery.Columns[20].Visible = false;

                        }
                        if (Request.QueryString["QryPMCPoId"] != null && Convert.ToString(Request.QueryString["QryPMCPoId"]) != "")
                        {
                            txtPoId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryPMCPoId"]));

                            DataTable dt = new DataTable();
                            clsPMCDelivery obj = new clsPMCDelivery();
                            obj.PMCPoid = txtPoId.Text;
                            dt = obj.GetDeliveryDetails(obj);
                            txtDWANo.Text = obj.PMCDINumber;
                            txtLECName.Text = obj.PMCLecNAme;

                            if (dt.Rows.Count > 0)
                            {
                                ViewState["TOTALTC"] = dt;
                                cmdAdd.Enabled = true;
                            }
                            txtPoNumber.Text = Convert.ToString(dt.Rows[0]["sPMC_PO_NO"]);
                            obj.PMCPoNumber = txtPoNumber.Text;
                            obj.GetPurchaseCount(obj);
                            txtTotalQuantity.Text = obj.TotalTC;
                            BindgridView(SFTPmainfolder, UserName, Password);
                            BindDIdocs(SFTPmainfolder, UserName, Password);
                            grdPendingTC.DataSource = dt;
                            grdPendingTC.DataBind();
                            LoadComboField();
                            cmdAdd.Enabled = false;

                        }
                        txtDIDate.Attributes.Add("onblur", "return ValidateDate(" + txtDIDate.ClientID + ");");
                    }
                }
            }

        }

        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = MethodBase.GetCurrentMethod().DeclaringType.Name;
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
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;

            }
        }
        #endregion



        public void LoadComboField()
        {
            try
            {
                string strQry = string.Empty;

                strQry = "SELECT \"TM_ID\",\"TM_NAME\" FROM \"TBLTRANSMAKES\",\"TBLPMC_POMASTER\",\"TBLPMC_POOBJECTS\" ";
                strQry += " WHERE \"TM_ID\"=\"PMC_PB_MAKE\" AND ";
                strQry += " \"PMC_PO_ID\"=\"PMC_PB_PO_ID\" AND \"PMC_PO_NO\"='" + txtPoNumber.Text + "' and ";
                strQry += " \"PMC_PB_PO_STATUS\"=1 GROUP BY \"TM_ID\",\"TM_NAME\"";

                Genaral.Load_Combo(strQry, "-Select-", cmbMake);

                Genaral.Load_Combo("SELECT \"SM_ID\", \"SM_NAME\" FROM \"TBLSTOREMAST\" ORDER BY \"SM_NAME\"", "-Select-", cmbStore);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// load the rating Drop Down Details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbCapacity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string qry = "SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\",\"TBLPMC_POMASTER\",\"TBLPMC_POOBJECTS\"  ";
                qry += " WHERE \"MD_TYPE\"='SR' ";
                qry += " AND \"PMC_PO_NO\"='" + txtPoNumber.Text + "' AND \"PMC_PO_ID\"=\"PMC_PB_PO_ID\" and ";
                qry += " \"PMC_PB_PO_STATUS\"=1  AND CAST(\"MD_ID\" AS TEXT)=\"PMC_PO_RATING\" ";
                qry += " AND \"PMC_PB_MAKE\"=" + cmbMake.SelectedValue + "  AND \"PMC_PB_CAPACITY\" ";
                qry += " =" + cmbCapacity.SelectedItem.Text + " GROUP BY \"MD_ID\",\"MD_NAME\" ";

                Genaral.Load_Combo(qry, "--Select--", cmbRating);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        ///  load the rating Drop Down Details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbCapacity_SelectedIndexChangedonupdate(object sender, EventArgs e)
        {
            try
            {
                string qry = "SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\",\"TBLPMC_POMASTER\",\"TBLPMC_POOBJECTS\" ";
                qry += " WHERE \"MD_TYPE\"='SR' AND \"PMC_PO_NO\"='" + txtPoNumber.Text + "' ";
                qry += " AND \"PMC_PO_ID\"=\"PMC_PB_PO_ID\" and \"PMC_PB_PO_STATUS\"=1  AND CAST(\"MD_ID\" AS TEXT)=";
                qry += " \"PMC_PO_RATING\" AND \"PMC_PB_MAKE\"=" + cmbMake.SelectedValue + "  AND \"PMC_PB_CAPACITY\" ";
                qry += " = " + cmbCapacity.SelectedItem.Text + " GROUP BY \"MD_ID\",\"MD_NAME\"";

                Genaral.Load_Combo(qry, cmbRating);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// load the capacity Details in  Drop Down  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbMake_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string qry = "SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\",\"TBLPMC_POMASTER\",\"TBLPMC_POOBJECTS\" ";
                qry += " WHERE \"MD_TYPE\"='C' AND ";
                qry += "\"PMC_PO_NO\"='" + txtPoNumber.Text + "'  ";
                qry += " AND \"PMC_PO_ID\"=\"PMC_PB_PO_ID\"  and \"PMC_PB_MAKE\"=" + cmbMake.SelectedValue + " ";
                qry += " AND \"MD_NAME\"=cast(\"PMC_PB_CAPACITY\" as text) and ";
                qry += "\"PMC_PB_PO_STATUS\"=1  GROUP BY \"MD_ID\",\"MD_NAME\"";


                Genaral.Load_Combo(qry, "--Select--", cmbCapacity);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// load the capacity Details in Drop Down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbMake_SelectedIndexChangedonupdate(object sender, EventArgs e)
        {
            try
            {
                string qry = "SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\",\"TBLPMC_POMASTER\",\"TBLPMC_POOBJECTS\" ";
                qry += " WHERE \"MD_TYPE\"='C' AND \"PMC_PO_NO\"='" + txtPoNumber.Text + "' ";
                qry += " AND \"PMC_PO_ID\"=\"PMC_PB_PO_ID\"  and \"PMC_PB_MAKE\"=" + cmbMake.SelectedValue + " AND ";
                qry += " \"MD_ID\"=\"PMC_PB_CAPACITY_ID\" and \"PMC_PB_PO_STATUS\"=1  GROUP BY \"MD_ID\",\"MD_NAME\"";
                Genaral.Load_Combo(qry, cmbCapacity);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        // to load DI details
        public void LoadDeliveredIst(string DI_No)
        {
            try
            {

                clsPMCDelivery objDelivery = new clsPMCDelivery();
                DataTable dt = new DataTable();
                objDelivery.DINo = DI_No;
                dt = objDelivery.GetDeliveredDetails(DI_No);
                ViewState["DiDetails"] = dt;
                txtDIId.Text = Convert.ToString(dt.Rows[0]["DI_ID"]);
                txtDIId.Visible = false;
                txtDINumber.Text = Convert.ToString(dt.Rows[0]["DIM_DI_NO"]);
                txtDINumber.Enabled = false;
                grdDelivery.DataSource = dt;
                grdDelivery.DataBind();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// for serach po num in search window
        /// </summary>
        public void LoadSearchWindow()
        {
            try
            {
                string strQry = string.Empty;
                strQry = "Title=Search and Select Purchase Order Details&";
                strQry += "Query=SELECT \"PMC_PO_NO\" as \"PO Number\", \"DM_NUMBER\" as \"DWA Number\" ";
                strQry += "FROM \"TBLPMC_POMASTER\",\"TBLDWAMASTER\" ";
                strQry += "WHERE \"PMC_DM_ID\" = \"DM_ID\" AND {0} like %{1}% ORDER BY \"PMC_PO_ID\" &";
                strQry += "DBColName=\"PMC_PO_NO\"~\"DM_NUMBER\"&";
                strQry += "ColDisplayName=PO Number~DWA Number&";
                strQry = strQry.Replace("'", @"\'");
                cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?"
                    + strQry + "tb=" + txtPoNumber.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + txtPoNumber.ClientID + ")");

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// For dowlowding or view po document 
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
                clsSFTP objFtp = new clsSFTP(SFTPPath, UserName, Password);

                string PoNo = Regex.Replace(txtPoNumber.Text, @"[^0-9a-zA-Z]+", "");
                //FtpServer += "PO_DOCS/" + PoNo;
                FtpServer += "PMC_PO_DOCS/" + PoNo;

                //  taking ftp server path for get file
                bool IsExists = objFtp.FtpDirectoryExists(FtpServer);
                // checking related ponumber directory is there are not!
                if (IsExists == false)
                {
                    gvFiles.Visible = false;
                    return dtFiles;
                }

                dtFiles = objFtp.GetListOfFiles(FtpServer);


                if (dtFiles.Rows.Count > 0)
                {
                    gvFiles.DataSource = dtFiles;
                    gvFiles.DataBind();
                }

                ViewState["PoDocs"] = dtFiles;
                gvFiles.DataBind();

                return dtFiles;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtFiles;
            }
        }

        /// <summary>
        /// For Bind Di Docs
        /// </summary>
        /// <param name="FtpServer"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>

        public DataTable BindDIdocs(string FtpServer, string username, string password)
        {
            DataTable dtFiles = new DataTable();
            try
            {
                clsSFTP objFtp = new clsSFTP(SFTPPath, UserName, Password);
                // clsFtp objFtp = new clsFtp(sFileServerPath, sUserName, sPassword);
                string DiNo = Regex.Replace(txtDINumber.Text, @"[^0-9a-zA-Z]+", "");
                FtpServer += "PMC_DI_DOCS/" + DiNo;
                //path for get files from ftp
                bool IsExists = objFtp.FtpDirectoryExists(FtpServer);
                // checking related ponumber directory is there are not!

                dtFiles = objFtp.GetListOfFiles(FtpServer);
                if (dtFiles.Rows.Count > 0)
                {
                    grdDIdocs.DataSource = dtFiles;
                    grdDIdocs.DataBind();
                }


                if (IsExists == false)
                {
                    grdDIdocs.Visible = false;
                    return dtFiles;
                }


                ViewState["DiDocs"] = dtFiles;

                grdDIdocs.DataBind();
                //}
                return dtFiles;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtFiles;
            }
        }

        /// <summary>
        /// Displaying PO Details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                clsPMCDelivery obj = new clsPMCDelivery();
                string temp = txtPoNumber.Text.ToUpper().Trim();              
                obj.PMCPoNumber = temp;                
                //txtPoNumber.Text = temp;
                txtPoId.Text = obj.GetPOId(obj);
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                dt1 = obj.GetDwaExpAndPoDate(obj);

                txtDWAExp.Text = obj.PmcDwaExp;
                txtPODate.Text = obj.PmcPoDate;


                if ((obj.PmcDwaValUpTO ?? "").Length > 0 && (obj.PmcLecUpTo ?? "").Length > 0)
                {
                    string sResult = Genaral.DateComparision(obj.PmcDwaValUpTO, "", true, false);
                    if (sResult == "2" || obj.PmcDwaStatus == "D")
                    {
                        string msg = "DWA date got expired for DWA N0-" + obj.PMCDINumber + ", Please extend the DWA expiry Date.";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID, "alert('" + msg + "');", true);
                        return;
                    }
                    string LecResult = Genaral.DateComparision(obj.PmcLecUpTo, "", true, false);
                    if (LecResult == "2" || obj.PmcLecStatus == "D")
                    {
                        string msg = "LEC date got expired for LEC Name -" + obj.PMCLecNAme + ", ";
                        msg += "Please extend the LEC expiry Date.or the LEC is Inactive !";

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID, "alert('" + msg + "');", true);
                        return;
                    }

                }

                dt = obj.GetDeliveryDetails(obj);                
                if (dt.Rows.Count > 0)
                {
                    ViewState["TOTALTC"] = dt;
                    cmdAdd.Enabled = true;

                    txtDWANo.Text = obj.PMCDINumber;
                    txtLECName.Text = obj.PMCLecNAme;
                    LoadComboField();
                    obj.GetPurchaseCount(obj);
                    txtTotalQuantity.Text = obj.TotalTC;
                    BindgridView(SFTPmainfolder, UserName, Password);
                    
                }
                else
                {
                    ShowMsgBox("No Records Found");
                    return;
                }
                grdPendingTC.DataSource = dt;
                grdPendingTC.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// validating DI date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void ValidateDIDate(object sender, EventArgs e)
        {
            try
            {                
                DateTime DWAExpiryDate = DateTime.Now;
                DateTime PoDate = DateTime.Now;
                DateTime DIDate = DateTime.Now;

                if (txtDWAExp.Text.Trim() != "")
                {
                    DWAExpiryDate = DateTime.ParseExact(txtDWAExp.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (txtPODate.Text.Trim() != "")
                {
                    PoDate = DateTime.ParseExact(txtPODate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (txtDIDate.Text.Trim() != "")
                {
                    DIDate = DateTime.ParseExact(txtDIDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }


                if (txtPODate.Text.Trim() == "")
                {
                    txtPoNumber.Focus();
                    ShowMsgBox("Please Search PO Number");
                    txtDIDate.Text = string.Empty;
                    return;
                }

                

                if (DIDate < PoDate)
                {
                    txtDIDate.Focus();
                    ShowMsgBox("DI Date Should Greater Than or equal to PO Date");
                    txtDIDate.Text = string.Empty;
                    return;
                }
                if (DIDate > DWAExpiryDate)
                {
                    txtDIDate.Focus();
                    ShowMsgBox("DI Date Should Less Than or equal to DWA Expiry Date");
                    txtDIDate.Text = string.Empty;
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

        /// <summary>
        /// For validate Due date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ValidateDueDate(object sender, EventArgs e)
        {
            try
            {
                DateTime DWAExpiryDate = DateTime.Now;
                DateTime DIDate = DateTime.Now;
                DateTime DueDate = DateTime.Now;
                if (txtDWAExp.Text.Trim() != "")
                {
                    DWAExpiryDate = DateTime.ParseExact(txtDWAExp.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (txtDIDate.Text.Trim() != "")
                {
                    DIDate = DateTime.ParseExact(txtDIDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (txtDueDate.Text.Trim() != "")
                {
                    DueDate = DateTime.ParseExact(txtDueDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                
                if (txtDIDate.Text.Trim() == "")
                {
                    txtDIDate.Focus();
                    ShowMsgBox("Please Select DI Date");
                    txtDueDate.Text = string.Empty;
                    return;
                }
                

                if (DueDate < DIDate)
                {
                    txtDueDate.Focus();
                    ShowMsgBox("Due Date Should Greater Than or equal to DI Date");
                    txtDueDate.Text = string.Empty;
                    return;
                }
                if (DueDate > DWAExpiryDate)
                {
                    txtDueDate.Focus();
                    ShowMsgBox("Due Date Should Less Than or equal to DWA Expiry Date");
                    txtDueDate.Text = string.Empty;
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

        // for reseting Text Field
        private void Reset()
        {
            try
            {
                txtDIDate.Text = "";
                txtDINumber.Text = "";
                cmbStore.SelectedIndex = 0;
                txtDueDate.Text = "";
                cmbMake.ClearSelection();
                cmbCapacity.ClearSelection();
                txtQuantity.Text = "";
                cmbRating.ClearSelection();
                //cmbRating.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        // for reseting Text Field
        protected void btnReset_Click1(object sender, EventArgs e)
        {
            txtDIDate.Text = "";
            txtDINumber.Text = "";
            cmbStore.SelectedIndex = 0;
            txtDueDate.Text = "";
            cmbMake.ClearSelection();
            cmbCapacity.ClearSelection();
            txtQuantity.Text = "";
            cmbRating.ClearSelection();
            cmbRating.SelectedIndex = 0;
            txtPoNumber.Text = "";
            txtTotalQuantity.Text = "";
            txtDWANo.Text = "";
            txtLECName.Text = "";
            grdPendingTC.DataSource = null;
            grdPendingTC.DataBind();
            gvFiles.DataSource = null;
            gvFiles.DataBind();
            grdDelivery.DataSource = null;
            grdDelivery.DataBind();
            ViewState["DiDetails"] = null;

        }

        public string[] isCountCapacity(string strCapacity, string strmake)
        {
            string[] Arr = new string[2];
            try
            {
                DataTable dtPoDetails;
                DataTable DiDetailsGrid;
                int Count = 0;
                int pending = 0;
                dtPoDetails = (DataTable)ViewState["TOTALTC"];
                DiDetailsGrid = (DataTable)ViewState["DiDetails"];
                string po_num, po_make, po_capacity, po_rating, po_total, Po_pending;

                if (DiDetailsGrid != null && DiDetailsGrid.Rows.Count > 0)
                {

                    for (int i = 0; i < DiDetailsGrid.Rows.Count;)
                    {
                        for (int j = 0; j < dtPoDetails.Rows.Count; j++)
                        {
                            if (Convert.ToString(DiDetailsGrid.Rows[i]["DI_CAPACITY"]) == Convert.ToString(dtPoDetails.Rows[j]["CAPACITY"]) &&
                                Convert.ToString(DiDetailsGrid.Rows[i]["DI_MAKE"]) == Convert.ToString(dtPoDetails.Rows[j]["MAKE"])
                                && strmake == Convert.ToString(dtPoDetails.Rows[j]["MAKE"]) && 
                                strCapacity == Convert.ToString(dtPoDetails.Rows[j]["CAPACITY"]))
                            {

                                po_num = Convert.ToString(dtPoDetails.Rows[j]["PO_NO"]);
                                po_make = Convert.ToString(dtPoDetails.Rows[j]["MAKE"]);
                                po_capacity = Convert.ToString(dtPoDetails.Rows[j]["CAPACITY"]);
                                po_rating = Convert.ToString(dtPoDetails.Rows[j]["RATING"]);
                                po_total = Convert.ToString(dtPoDetails.Rows[j]["TOTAL"]);
                                Po_pending = Convert.ToString(dtPoDetails.Rows[j]["PENDING"]);
                            }
                        }
                        Count = pending + Convert.ToInt32(txtQuantity.Text);
                        //To check whether selected transformers doesnot exceed requested number of transformers
                        if (Convert.ToInt32(dtPoDetails.Rows[i]["PENDING"]) > Count)
                        {
                            Arr[0] = "Accept";
                            Arr[1] = "0";
                            return Arr;
                        }
                        else
                        {
                            Arr[0] = "Please Select Available Quantity of " + (Convert.ToInt32(dtPoDetails.Rows[i]["PENDING"]) - pending) + " ";
                            Arr[1] = "1";
                            return Arr;
                        }
                        

                    }
                }
                else
                {
                    for (int j = 0; j < dtPoDetails.Rows.Count; j++)
                    {
                        if (strCapacity == Convert.ToString(dtPoDetails.Rows[j]["CAPACITY"]) && strmake == Convert.ToString(dtPoDetails.Rows[j]["MAKE"]))
                        {
                            po_num = Convert.ToString(dtPoDetails.Rows[j]["PO_NO"]);
                            po_make = Convert.ToString(dtPoDetails.Rows[j]["MAKE"]);
                            po_capacity = Convert.ToString(dtPoDetails.Rows[j]["CAPACITY"]);
                            po_rating = Convert.ToString(dtPoDetails.Rows[j]["RATING"]);
                            po_total = Convert.ToString(dtPoDetails.Rows[j]["TOTAL"]);
                            Po_pending = Convert.ToString(dtPoDetails.Rows[j]["PENDING"]);

                            Count += Convert.ToInt32(txtQuantity.Text);
                        }
                    }
                    Arr[0] = "Accept";
                    Arr[1] = "0";
                    return Arr;
                }
                return Arr;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }

        }

        /// <summary>
        /// For add di details to grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string[] Arr = new string[2];
                DataTable dt = new DataTable();
                clsPMCDelivery objDely = new clsPMCDelivery();
                string strResult = string.Empty;

                if (validate() == false)
                {
                    return;
                }
                string ponumber = txtPoNumber.Text;
                string dinumber = txtDINumber.Text.ToUpper().Trim();

                string[] res = objDely.CheckduplicateDI(ponumber, dinumber);
                if (res[1].ToString() == "2")
                {
                    ShowMsgBox(res[0]);
                    return;
                }

                string sDIid = txtDIId.Text;
                string sDINo = txtDINumber.Text.ToUpper().Trim();
                string sDIDate = txtDIDate.Text;
                string sPoId = txtPoId.Text;
                int startrange = 0;
                int endrange = 0;
                var TcStartrange = string.Empty;
                var TcEndrange = string.Empty;

                string sStoreId = cmbStore.SelectedValue;
                string sStoreName = Convert.ToString(cmbStore.SelectedItem);
                string sDueDate = txtDueDate.Text;
                string sMakeId = cmbMake.SelectedValue;
                string sMakeName = Convert.ToString(cmbMake.SelectedItem);
                string sCapacity = Convert.ToString(cmbCapacity.SelectedItem);
                string sCapacityID = cmbCapacity.SelectedValue;
                string sQuantity = txtQuantity.Text;
                string sRatingID = cmbRating.SelectedValue;
                string sRating = Convert.ToString(cmbRating.SelectedItem);
                objDely.PMCDIDate = txtDIDate.Text;
                objDely.PMCDueDate = txtDueDate.Text;


                int pending = 0;
                int oldQuantity = 0;
                int AvailQuantity = 0;
                int totalQunty = 0;
                objDely.PMCPoid = txtPoId.Text;

                DataTable dtpending = new DataTable();
                DataTable dttcrange = new DataTable();
                
                if (Session["FileUpload"] == null && fupFile.HasFile)
                {
                    Session["FileUpload"] = fupFile;
                    lblFilename.Text = fupFile.FileName; // get the name 
                    fupFile.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + fupFile.FileName));
                    fupFile = (FileUpload)Session["FileUpload"];
                }
                else if (Session["FileUpload"] != null && (!fupFile.HasFile))
                {
                    fupFile = (FileUpload)Session["FileUpload"];
                    lblFilename.Text = fupFile.FileName;
                }
                else if (fupFile.HasFile)
                {
                    fupFile.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + fupFile.FileName));
                    Session["FileUpload"] = fupFile;
                    lblFilename.Text = fupFile.FileName;
                }
                clsPMCDelivery objDeli = new clsPMCDelivery();
                objDeli.DIid = sDIid;
                objDeli.DINo = sDINo.ToUpper();
                if (objDeli.DIid != "" && objDeli.DIid != null)
                {
                    objDeli.Capacity = sCapacity;
                    objDeli.TcMake = sMakeId;
                    objDeli.Store = sStoreId;
                    dt = objDeli.GetAllotmentCount(objDeli);
                    if (dt.Rows.Count > 0)
                    {
                        int delivered = Convert.ToInt32(dt.Rows[0]["ALLOTED"]);
                        int Quantty = Convert.ToInt32(txtQuantity.Text);

                        if (Quantty < delivered)
                        {
                            //string Status = Convert.ToString(delivered);
                            string Msg = "This Capacity Already Alloted To Some Division  Quantity " + delivered + ", So ";
                            Msg += " You Can`t Reduce The Count Below  :" + delivered + " For Reference Allotment Number:";
                            Msg += Convert.ToString(dt.Rows[0]["ALT_NO"]);
                            ShowMsgBox(Msg);
                            dt = null;
                            return;
                        }
                    }
                }

                if (grdDelivery.Rows.Count <= 0)
                {
                    dttcrange = objDeli.GetTCRange(objDeli);
                    if (dttcrange.Rows.Count > 0)
                    {
                        startrange = Convert.ToInt32(dttcrange.Rows[0]["proc_pmc_max_dtrrange"]);
                        endrange = startrange + Convert.ToInt32(txtQuantity.Text) - 1;
                    }

                    if ((Convert.ToString(startrange) ?? "").Length > 0 && (Convert.ToString(endrange) ?? "").Length > 0)
                    {

                        TcStartrange = objDeli.addprefix(Convert.ToString(startrange).Length, Convert.ToString(startrange));
                        TcEndrange = objDeli.addprefix(Convert.ToString(endrange).Length, Convert.ToString(endrange));
                        
                    }

                }
                else
                {
                    foreach (GridViewRow row in grdDelivery.Rows)
                    {

                        Label lblstartRange = row.FindControl("lblendRange") as Label;
                        lblstartRange.Text = lblstartRange.Text.Replace("HP", " ");
                        startrange = Convert.ToInt32(lblstartRange.Text) + 1;
                        endrange = startrange + Convert.ToInt32(txtQuantity.Text) - 1;

                        if ((Convert.ToString(startrange) ?? "").Length > 0 && (Convert.ToString(endrange) ?? "").Length > 0)
                        {

                            TcStartrange = objDeli.addprefix(Convert.ToString(startrange).Length, Convert.ToString(startrange));
                            TcEndrange = objDeli.addprefix(Convert.ToString(endrange).Length, Convert.ToString(endrange));

                        }
                    }
                    //if ((Convert.ToString(startrange) ?? "").Length > 0 && (Convert.ToString(endrange) ?? "").Length > 0)
                    //{

                    //    TcStartrange = objDeli.addprefix(Convert.ToString(startrange).Length, Convert.ToString(startrange));
                    //    TcEndrange = objDeli.addprefix(Convert.ToString(endrange).Length, Convert.ToString(endrange));

                    //}
                }
                objDeli.Capacity = sCapacity;
                objDeli.PMCPoid = sPoId;
                dtpending = objDeli.GetDeliveryDetails(objDeli);
                for (int i = 0; i < dtpending.Rows.Count; i++)
                {
                    if (Convert.ToString(dtpending.Rows[i]["CAPACITY"]) == cmbCapacity.SelectedItem.Text &&
                        cmbMake.SelectedItem.Text == Convert.ToString(dtpending.Rows[i]["MAKE"]) &&
                        cmbRating.SelectedValue == Convert.ToString(dtpending.Rows[i]["sPMC_PO_RATING"]))
                    {
                        pending = Convert.ToInt32(dtpending.Rows[i]["PENDING"]);
                        totalQunty = Convert.ToInt32(dtpending.Rows[i]["TOTAL"]);

                    }
                    else
                    {
                        continue;
                    }
                    if (sDIid == "")
                    {
                        if (Convert.ToInt32(sQuantity) > Convert.ToInt32(pending))
                        {
                            ShowMsgBox(" Quantity Should not greater than the Available Quantity of " + pending + " ");
                            return;
                        }
                    }
                    else
                    {
                        if (ViewState["DiDetails"] != null)
                        {
                            DataTable dtQunty = (DataTable)ViewState["DiDetails"];

                            for (int j = 0; j < dtQunty.Rows.Count; j++)
                            {
                                if (cmbMake.SelectedItem.Text == Convert.ToString(dtQunty.Rows[j]["DI_MAKE"]) &&
                                    cmbCapacity.SelectedItem.Text == Convert.ToString(dtQunty.Rows[j]["DI_CAPACITY"])
                                    && cmbRating.SelectedValue == Convert.ToString(dtQunty.Rows[j]["DI_STARRATE"]))
                                {
                                    oldQuantity += Convert.ToInt32(dtQunty.Rows[j]["DI_QUANTITY"]);
                                }
                            }
                            AvailQuantity = (Convert.ToInt32(totalQunty) - Convert.ToInt32(oldQuantity));

                            if (Convert.ToInt32(sQuantity) > AvailQuantity)
                            {
                                ShowMsgBox(" Quantity Should not greater than the Available Quantity of " + AvailQuantity + " ");
                                return;
                            }

                        }
                        break;
                    }
                    if (ViewState["DiDetails"] != null)
                    {
                        DataTable dtQunty = (DataTable)ViewState["DiDetails"];
                        for (int j = 0; j < dtQunty.Rows.Count; j++)
                        {
                            if (cmbMake.SelectedItem.Text == Convert.ToString(dtQunty.Rows[j]["DI_MAKE"]) && 
                                cmbCapacity.SelectedItem.Text == Convert.ToString(dtQunty.Rows[j]["DI_CAPACITY"])
                                && cmbRating.SelectedValue == Convert.ToString(dtQunty.Rows[j]["DI_STARRATE"]))
                            {
                                oldQuantity += Convert.ToInt32(dtQunty.Rows[j]["DI_QUANTITY"]);
                            }
                        }
                        AvailQuantity = (Convert.ToInt32(pending) - Convert.ToInt32(oldQuantity));

                        if (Convert.ToInt32(sQuantity) > AvailQuantity)
                        {
                            ShowMsgBox(" Capacity Quantity Should not greater than the Available Quantity of " + AvailQuantity + " ");
                            return;
                        }

                    }
                }
                if (ViewState["DiDetails"] != null)
                {
                    DataTable dtCap = (DataTable)ViewState["DiDetails"];
                    for (int i = 0; i < dtCap.Rows.Count; i++)
                    {
                        if (sDINo == Convert.ToString(dtCap.Rows[i]["DIM_DI_NO"])
                            && sStoreId == Convert.ToString(dtCap.Rows[i]["DI_STORE_ID"]) && sMakeId == Convert.ToString(dtCap.Rows[i]["DI_MAKE_ID"])
                            && sRatingID == Convert.ToString(dtCap.Rows[i]["DI_STARRATE"])
                            && sCapacity == Convert.ToString(dtCap.Rows[i]["DI_CAPACITY"]))
                        {
                            ShowMsgBox(" make - Store- Star Rate - Capacity - Start Rate Combination Already Added");
                            return;
                        }
                    }
                }

                if (ViewState["DiDetails"] == null)
                {


                    dt.Columns.Add("DI_ID");
                    dt.Columns.Add("DI_PO_ID");
                    dt.Columns.Add("DIM_DI_NO");
                    dt.Columns.Add("DI_DATE");
                    dt.Columns.Add("DI_CONSIGNEE");
                    dt.Columns.Add("DI_STORE_ID");
                    dt.Columns.Add("DI_STORE");
                    dt.Columns.Add("DI_DUEDATE");
                    dt.Columns.Add("DI_MAKE_ID");
                    dt.Columns.Add("DI_MAKE");
                    dt.Columns.Add("DI_CAPACITY");
                    dt.Columns.Add("DI_CAPACITY_ID");
                    dt.Columns.Add("DI_STARRATE");
                    dt.Columns.Add("DI_STARRATENAME");
                    dt.Columns.Add("DI_QUANTITY");
                    dt.Columns.Add("DI_START_RANGE");
                    dt.Columns.Add("DI_END_RANGE");
                    dt.Columns.Add("PMC_DI_TC_START_RANGE_VAR");
                    dt.Columns.Add("PMC_DI_TC_END_RANGE_VAR");
                    dt.Columns.Add(new DataColumn("DI_FILE", typeof(byte[])));
                    dt.Columns.Add("DI_FILE_EXT");

                }
                else
                {
                    //load datatble from viewstate
                    dt = (DataTable)ViewState["DiDetails"];
                }
                string Id = "";
                DataRow dRow = dt.NewRow();


                string strExt = string.Empty;
                if (fupFile.PostedFile.ContentLength != 0)
                {
                    string filename = Path.GetFileName(fupFile.PostedFile.FileName);
                    strExt = filename.Substring(filename.LastIndexOf('.') + 1);
                    strExt = Path.GetExtension(fupFile.PostedFile.FileName);
                    string sFileExt = Convert.ToString(ConfigurationManager.AppSettings["FileFormat"]);
                    string sAnxFileExt = System.IO.Path.GetExtension(fupFile.FileName).ToString().ToLower();
                    sAnxFileExt = ";" + sAnxFileExt.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sAnxFileExt))
                    {
                        ShowMsgBox("Invalid File Format");
                        return;
                    }
                    strExt = Path.GetExtension(fupFile.PostedFile.FileName);

                }

                dRow["DI_PO_ID"] = sPoId;
                dRow["DIM_DI_NO"] = sDINo.ToUpper();
                dRow["DI_DATE"] = sDIDate;
                dRow["DI_STORE_ID"] = sStoreId;
                dRow["DI_STORE"] = sStoreName;
                dRow["DI_DUEDATE"] = sDueDate;
                dRow["DI_MAKE_ID"] = sMakeId;
                dRow["DI_MAKE"] = sMakeName;
                dRow["DI_CAPACITY"] = sCapacity;
                dRow["DI_CAPACITY_ID"] = sCapacityID;
                dRow["DI_STARRATE"] = sRatingID;
                dRow["DI_STARRATENAME"] = sRating;
                dRow["DI_QUANTITY"] = sQuantity;
                dRow["DI_START_RANGE"] = startrange;
                dRow["DI_END_RANGE"] = endrange;
                dRow["PMC_DI_TC_START_RANGE_VAR"] = TcStartrange;
                dRow["PMC_DI_TC_END_RANGE_VAR"] = TcEndrange;
                dRow["DI_FILE_EXT"] = strExt;

                if (sDIid == "")
                {

                    dRow["DI_ID"] = Id;
                    dRow["DI_PO_ID"] = sPoId;
                    dRow["DIM_DI_NO"] = sDINo.ToUpper();
                    dRow["DI_DATE"] = sDIDate;
                    dRow["DI_STORE_ID"] = sStoreId;
                    dRow["DI_STORE"] = sStoreName;
                    dRow["DI_DUEDATE"] = sDueDate;
                    dRow["DI_MAKE_ID"] = sMakeId;
                    dRow["DI_MAKE"] = sMakeName;
                    dRow["DI_CAPACITY"] = sCapacity;
                    dRow["DI_CAPACITY_ID"] = sCapacityID;
                    dRow["DI_STARRATE"] = sRatingID;
                    dRow["DI_STARRATENAME"] = sRating;
                    dRow["DI_QUANTITY"] = sQuantity;
                    dRow["DI_START_RANGE"] = startrange;
                    dRow["DI_END_RANGE"] = endrange;
                    dRow["PMC_DI_TC_START_RANGE_VAR"] = TcStartrange;
                    dRow["PMC_DI_TC_END_RANGE_VAR"] = TcEndrange;
                    dRow["DI_FILE_EXT"] = strExt;
                    dt.Rows.Add(dRow);


                    if (dt.Rows.Count > 0)
                    {
                        dt = objDeli.ArrangedtRange(dt);
                    }
                    dt.AcceptChanges();
                    ViewState["DiDetails"] = dt;
                    LoadCapacity(dt);
                    txtDINumber.Enabled = true;
                    txtDIDate.Enabled = true;
                    txtDueDate.Enabled = true;

                    return;
                }
                #region//grd row edit

                #endregion
                else
                {


                    dRow["DI_ID"] = sDIid;
                    dRow["DI_PO_ID"] = sPoId;
                    dRow["DIM_DI_NO"] = sDINo.ToUpper();
                    dRow["DI_DATE"] = sDIDate;
                    dRow["DI_STORE_ID"] = sStoreId;
                    dRow["DI_STORE"] = sStoreName;
                    dRow["DI_DUEDATE"] = sDueDate;
                    dRow["DI_MAKE_ID"] = sMakeId;
                    dRow["DI_MAKE"] = sMakeName;
                    dRow["DI_CAPACITY"] = sCapacity;
                    dRow["DI_CAPACITY_ID"] = sCapacityID;
                    dRow["DI_STARRATE"] = sRatingID;
                    dRow["DI_STARRATENAME"] = sRating;
                    dRow["DI_QUANTITY"] = sQuantity;
                    dRow["DI_START_RANGE"] = startrange;
                    dRow["DI_END_RANGE"] = endrange;
                    dRow["PMC_DI_TC_START_RANGE_VAR"] = TcStartrange;
                    dRow["PMC_DI_TC_END_RANGE_VAR"] = TcEndrange;

                    dt.Rows.Add(dRow);
                    dt.AcceptChanges();

                    dt = objDeli.ArrangedtRange(dt);

                    ViewState["DiDetails"] = dt;
                    LoadCapacity(dt);
                    txtDINumber.Enabled = true;
                    txtDIDate.Enabled = true;
                    txtDueDate.Enabled = true;

                }
                if (dt.Rows.Count > 0)
                {
                    dt = objDeli.ArrangedtRange(dt);
                }

                ViewState["DiDetails"] = dt;
                LoadCapacity(dt);
                txtDINumber.Enabled = true;
                txtDIDate.Enabled = true;
                txtDueDate.Enabled = true;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Bind the data to grid
        /// </summary>
        /// <param name="dt"></param>
        public void LoadCapacity(DataTable dt)
        {
            try
            {
                grdDelivery.DataSource = dt;
                grdDelivery.DataBind();
                grdDelivery.Visible = true;
                Reset();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// For Displaying the pop up msg
        /// </summary>
        /// <param name="sMsg"></param>
        private void ShowMsgBox(string sMsg)
        {
            try
            {
                string sShowMsg = string.Empty;
                sShowMsg = "<script language=javascript> alert ('" + sMsg + "')</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "Msg", sShowMsg);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// validating while add click
        /// </summary>
        /// <returns></returns>
        protected bool validate()
        {
            bool Status = false;
            try
            {
                if (txtPoNumber.Text.Trim() == string.Empty)
                {
                    ShowMsgBox("Please Enter PO Number");
                    txtPoNumber.Focus();
                    return Status;
                }

                if (txtDWANo.Text.Trim() == string.Empty)
                {
                    ShowMsgBox("Please Enter DWA Number");
                    txtDWANo.Focus();
                    return Status;
                }

                if (txtTotalQuantity.Text.Trim() == string.Empty)
                {
                    ShowMsgBox("Please Enter  Quantity");
                    txtTotalQuantity.Focus();
                    return Status;
                }

                if (txtLECName.Text.Trim() == string.Empty)
                {
                    ShowMsgBox("Please Enter LEC Name");
                    txtLECName.Focus();
                    return Status;
                }

                if (txtDINumber.Text.Trim() == string.Empty)
                {
                    ShowMsgBox("Please Enter DI Number");
                    txtDINumber.Focus();
                    return Status;
                }


                if (txtDIDate.Text.Trim() == string.Empty)
                {
                    ShowMsgBox("Please Select DI Date");
                    txtDIDate.Focus();
                    return Status;
                }
                //if (txtDueDate.Text.Trim() == string.Empty)
                //{
                //    ShowMsgBox("Please Select Due Date");
                //    txtDueDate.Focus();
                //    return Status;
                //}
                if (cmbMake.SelectedIndex == 0)
                {
                    ShowMsgBox("Please Select Make");
                    cmbMake.Focus();
                    return Status;
                }
                if (cmbCapacity.SelectedItem.Text == "--Select--")
                {
                    ShowMsgBox("Please Select Capacity");
                    cmbCapacity.Focus();
                    return Status;
                }
                if (cmbStore.SelectedIndex == 0)
                {
                    ShowMsgBox("Please Select Store");
                    cmbStore.Focus();
                    return Status;
                }
                
                //    if (cmbCapacity.SelectedIndex == 0)
                
                if (txtQuantity.Text.Trim() == string.Empty)
                {
                    ShowMsgBox("Please Enter Quantity");
                    txtQuantity.Focus();
                    return Status;
                }

                if ((txtQuantity.Text ?? "").Length > 0)
                {
                    var count = 0;

                    foreach (var item in txtQuantity.Text)
                    {
                        if (item != '0')
                        {
                            count++;
                        }
                    }

                    if (count == 0)
                    {
                        txtQuantity.Focus();
                        ShowMsgBox("please Enter Valid Quantity.");
                        txtQuantity.Text = "";
                        cmdAdd.Enabled = true;
                        return Status;
                    }
                }

                if (txtQuantity.Text.Trim() == "0")
                {
                    ShowMsgBox("Quantity Should be Greater than Zero ");
                    txtQuantity.Focus();
                    return Status;
                }
                if (cmbRating.SelectedItem.Text == "--Select--")
                {
                    ShowMsgBox("Please Select Rating");
                    cmbRating.Focus();
                    return Status;
                }
                Status = true;
                return Status;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Status;
            }
        }

        /// <summary>
        /// For reseting the Di details grid 
        /// </summary>
        protected void ResetGrid()
        {
            DataTable dt = (DataTable)ViewState["DiDetails"];
            if (dt.Rows.Count > 0)
            {
                int counter = 0;
                foreach (DataRow row1 in dt.Rows)
                {
                    counter++;
                    row1["ID"] = counter;
                }
                ViewState["DiDetails"] = dt;
            }
            else
            {
                ViewState["DiDetails"] = null;
            }
            dt = (DataTable)ViewState["DiDetails"];
            grdDelivery.DataSource = dt;
            grdDelivery.DataBind();
        }

        /// <summary>
        /// Editing DI add Details and Deleting DI Add details After ADD Button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDelivery_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            clsPMCDelivery objDeli = new clsPMCDelivery();
            try
            {
                if (e.CommandName == "Remove")
                {
                    DataTable dt = (DataTable)ViewState["DiDetails"];
                    DataTable ALtCount;
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    int sRowIndex = row.RowIndex;

                    Label lblDiId = (Label)row.FindControl("lblDiId");
                    Label lblpoId = (Label)row.FindControl("lblpoId");
                    Label lblDiNo = (Label)row.FindControl("lblDiNo");
                    Label lblDIDate = (Label)row.FindControl("lblDIDate");
                    Label lblConsignee = (Label)row.FindControl("lblConsignee");
                    Label lblStoreId = (Label)row.FindControl("lblStoreId");
                    Label lblDuedate = (Label)row.FindControl("lblDuedate");
                    Label lblMakeId = (Label)row.FindControl("lblMakeId");
                    Label lblCapacity = (Label)row.FindControl("lblCapacity");
                    Label lblRating = (Label)row.FindControl("lblRating");
                    Label lblQuantity = (Label)row.FindControl("lblQuantity");

                    objDeli.DINo = lblDiNo.Text;
                    objDeli.Capacity = lblCapacity.Text;
                    objDeli.TcMake = lblMakeId.Text;
                    objDeli.Store = lblStoreId.Text;
                    ALtCount = objDeli.GetAllotmentCount(objDeli);
                    // TO Check Before Deleting Whether Dispatch No Alloted or not 
                    if (ALtCount.Rows.Count > 0)
                    {
                        int delivered = Convert.ToInt32(ALtCount.Rows[0]["ALLOTED"]);

                        string status = Convert.ToString(delivered);
                        string msg = "This Capacity Already Alloted To Some Division  Quantity " + status + ", ";
                        msg += " So You Can`t Delete This Record , For Reference Allotment Number:";
                        msg += Convert.ToString(ALtCount.Rows[0]["ALT_NO"]);
                        ShowMsgBox(msg);
                        ALtCount = null;
                        return;
                    }
                    else
                    {
                        //to remove selected Capacity from grid
                        dt.Rows[sRowIndex].Delete();
                        dt.AcceptChanges();
                    }
                    if (dt.Rows.Count == 0)
                    {
                        ViewState["DiDetails"] = null;
                    }
                    else
                    {
                        ViewState["DiDetails"] = dt;
                    }
                    grdDelivery.DataSource = dt;
                    grdDelivery.DataBind();

                    LoadCapacity(dt);
                }
                else
                {
                    DataTable dt = (DataTable)ViewState["DiDetails"];
                    LoadCapacity(dt);
                }
                if (e.CommandName == "EditQNTY")
                {

                    DataTable dt = (DataTable)ViewState["DiDetails"];
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    //clsPoMaster objPoMast = new clsPoMaster();
                    int sRowIndex = row.RowIndex;

                    Label lblDiId = (Label)row.FindControl("lblDiId");
                    Label lblpoId = (Label)row.FindControl("lblpoId");
                    Label lblDiNo = (Label)row.FindControl("lblDiNo");
                    Label lblDIDate = (Label)row.FindControl("lblDIDate");
                    Label lblConsignee = (Label)row.FindControl("lblConsignee");
                    Label lblStoreId = (Label)row.FindControl("lblStoreId");
                    Label lblDuedate = (Label)row.FindControl("lblDuedate");
                    Label lblMakeId = (Label)row.FindControl("lblMakeId");
                    Label lblCapacityID = (Label)row.FindControl("lblCapacityID");
                    Label lblCapacity = (Label)row.FindControl("lblCapacity");

                    //Label lblCapacityValue = (Label)row.FindControl("lblCapacity");
                    Label lblRating = (Label)row.FindControl("lblRating");
                    Label lblQuantity = (Label)row.FindControl("lblQuantity");

                    Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='SR' ", "--Select--", cmbRating);

                    txtDINumber.Text = lblDiNo.Text;
                    txtDIId.Visible = false;
                    txtDIDate.Text = lblDIDate.Text;
                    txtDueDate.Text = lblDuedate.Text;
                    txtDINumber.Enabled = false;
                    cmbMake.SelectedValue = lblMakeId.Text;
                    //cmbCapacity.SelectedValue = lblCapacityValue.Text;
                    cmbRating.SelectedValue = lblRating.Text;
                    txtQuantity.Text = lblQuantity.Text;
                    cmbStore.SelectedValue = lblStoreId.Text;
                    cmbMake_SelectedIndexChanged(sender, e);
                    //cmbMake_SelectedIndexChangedonupdate(sender, e); // jira id : DTLMS-1896 on 30-09-2023.
                    cmbCapacity.SelectedValue = lblCapacityID.Text;
                    cmbCapacity_SelectedIndexChangedonupdate(sender, e);

                    cmbRating.SelectedValue = lblRating.Text;
                    if (hdfviewstatusflag.Value != "1")
                    {

                        dt.Rows[sRowIndex].Delete();
                        dt.AcceptChanges();
                        if (dt.Rows.Count == 0)
                        {
                            ViewState["DiDetails"] = null;
                        }
                        else
                        {
                            ViewState["DiDetails"] = dt;
                        }
                        grdDelivery.DataSource = dt;
                        grdDelivery.DataBind();
                    }
                    else
                    {
                        cmbMake.Enabled = false;
                        cmbRating.Enabled = false;
                        txtQuantity.ReadOnly = true;
                        cmbStore.Enabled = false;
                        cmbCapacity.Enabled = false;
                        fupFile.Enabled = false;
                        txtDIDate.Enabled = false;
                    }

                }
                DataTable dt1 = (DataTable)ViewState["DiDetails"];
                DataTable dtrange = objDeli.ArrangedtRange(dt1);
                grdDelivery.DataSource = dtrange;
                grdDelivery.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdDelivery_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtTcCapacity = new DataTable();
                grdDelivery.PageIndex = e.NewPageIndex;
                dtTcCapacity = (DataTable)ViewState["DiDetails"];
                LoadCapacity(dtTcCapacity);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmdClose_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("PMCDeliveryInstView.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// For save and update Delivery  details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveUpdate_Click(object sender, EventArgs e)
        {
            try
            {

                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SFTPPath"] ?? "");
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SFTPmainfolder"]);

                if (ViewState["DiDetails"] != null)
                {
                    clsPMCDelivery obj = new clsPMCDelivery();
                    clsSFTP objFtp = new clsSFTP(SFTPPath, UserName, Password);

                    obj.Crby = objSession.UserId;
                    obj.dtDelivery = (DataTable)ViewState["DiDetails"];

                    if (obj.dtDelivery.Rows.Count > 0)
                    {
                        obj.PMCPoid = Convert.ToString(obj.dtDelivery.Rows[0]["DI_PO_ID"]);

                    }
                    DataTable dt1 = new DataTable();
                    dt1 = obj.GetDwaExpAndPoDate(obj);

                    if ((obj.PmcDwaValUpTO ?? "").Length > 0 && (obj.PmcLecUpTo ?? "").Length > 0)
                    {
                        string sResult = Genaral.DateComparision(obj.PmcDwaValUpTO, "", true, false);
                        if (sResult == "2" || obj.PmcDwaStatus == "D")
                        {
                            string msg = "DWA date got expired for DWA N0-" + obj.PMCDINumber + ", Please extend the DWA expiry Date.";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID, "alert('" + msg + "');", true);
                            return;
                        }
                        string LecResult = Genaral.DateComparision(obj.PmcLecUpTo, "", true, false);
                        if (LecResult == "2" || obj.PmcLecStatus == "D")
                        {
                            string msg = "LEC date got expired for LEC Name -" + obj.PMCLecNAme + ", Please extend the LEC expiry Date.or the LEC is Inactive !";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID, "alert('" + msg + "');", true);
                            return;
                        }

                    }

                    obj.DINo = Convert.ToString(obj.dtDelivery.Rows[0]["DIM_DI_NO"]);
                    bool Isuploaded;

                    //string sMainFolderName = "DI_DOCS";
                    string sMainFolderName = "PMC_DI_DOCS";

                    string[] sArr = new string[2];
                    if (Session["FileUpload"] != null && (!fupFile.HasFile))
                    {
                        fupFile = (FileUpload)Session["FileUpload"];
                        lblFilename.Text = fupFile.FileName;
                    }
                    else
                    {
                        if (fupFile.PostedFile.FileName != null && fupFile.PostedFile.FileName != "")
                        {
                            fupFile.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + fupFile.FileName));
                        }
                        else
                        {
                            ShowMsgBox("Please upload the Delivery Note ");
                            fupFile.Focus();
                            return;
                        }

                    }
                    string strpath = System.IO.Path.GetExtension(fupFile.FileName);

                    string filename = Path.GetFileName(fupFile.PostedFile.FileName);
                    obj.DINo = Regex.Replace(obj.DINo, @"[^0-9a-zA-Z]+", "");

                    obj.FileExt = SFTPmainfolder + sMainFolderName + "/" + obj.DINo + "/" + filename;

                    if (ValidateForm() == true)
                    {
                        if (fupFile.PostedFile.FileName != null && fupFile.PostedFile.FileName != "")
                        {
                            sArr = obj.SaveDeliveryDetails(obj);
                        }
                        else
                        {
                            ShowMsgBox("Please upload the File");
                            return;
                        }
                    }

                    if (Convert.ToString(sArr[1]) == "0")
                    {
                        if (fupFile.PostedFile.ContentLength != 0)
                        {

                            string strExt = filename.Substring(filename.LastIndexOf('.') + 1);


                            string sFileExt = Convert.ToString(ConfigurationManager.AppSettings["FileFormat"]);
                            string sAnxFileExt = Convert.ToString(System.IO.Path.GetExtension(fupFile.FileName)).ToLower();
                            sAnxFileExt = ";" + sAnxFileExt.Remove(0, 1) + ";";

                            if (!sFileExt.Contains(sAnxFileExt))
                            {
                                ShowMsgBox("Invalid File Format");
                                return;
                            }
                            string sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + filename);



                            if (File.Exists(sDirectory))
                            {

                                bool IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName);
                                if (IsExists == false)
                                {

                                    objFtp.createDirectory(SFTPmainfolder + sMainFolderName);
                                }
                                IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName + "/" + obj.DINo);
                                if (IsExists == false)
                                {

                                    objFtp.createDirectory(SFTPmainfolder + sMainFolderName + "/" + obj.DINo);
                                }
                                //Isuploaded = objFtp.upload(SFTPmainfolder + sMainFolderName + "/" + obj.DINo + "/", filename,
                                //    sDirectory, SFTPmainfolder + sMainFolderName, SFTPmainfolder + sMainFolderName + "/" + obj.DINo);

                                Isuploaded = objFtp.upload(SFTPmainfolder + sMainFolderName + "/" + obj.DINo + "/", filename, sDirectory);

                                if (Isuploaded == true & File.Exists(sDirectory))
                                {
                                    File.Delete(sDirectory);
                                    sDirectory = obj.DINo + "/" + filename;

                                    ShowMsgBox(sArr[0]);


                                }
                            }


                        }
                        Session["FileUpload"] = null;
                        btnSave.Enabled = false;
                        ShowMsgBox(sArr[0]);
                        Reset();
                        grdDelivery.DataSource = null;
                        grdDelivery.DataBind();
                        cmdSearch_Click(sender, e);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID, " location.href='PMCDeliveryInstView.aspx';", true);
                        return;

                    }
                    else
                    {
                        ShowMsgBox(sArr[0]);
                        return;
                    }

                }
                else
                {
                    if (validate() == false)
                    {

                    }

                    ShowMsgBox("Please ADD Delivery Details!");
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
        #region Requirement got 5th Jan 2022
        /*
        giving provision to update Due date 
        */
        #endregion
        // update button is newly added to update due date
        //protected void btnUpdate_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string[] Arr = new string[3];
        //        clsPMCDelivery objdelivery = new clsPMCDelivery();
        //        if (txtDueDate.Text.Trim() == string.Empty)
        //        {
        //            ShowMsgBox("Please Enter the Due Date");
        //            txtDueDate.Focus();
        //            return;
        //        }

        //        string sResult = Genaral.DateComparision(txtDueDate.Text, txtDIDate.Text, false, false);

        //        if (sResult == "2")
        //        {
        //            ShowMsgBox("DI Date should be Greater than or equal to Due Date");
        //            txtDueDate.Focus();
        //            return;
        //        }
        //        string DueDate = txtDueDate.Text;
        //        string DIno = txtDINumber.Text;
        //        int PoNo = Convert.ToInt32(txtPoId.Text);

        //        Arr = objdelivery.UpdateDIDetails(DueDate, DIno, PoNo);
        //        if (Convert.ToString(Arr[1]) == "0")
        //        {
        //            ShowMsgBox(Arr[0]);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
        //         System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}

        protected void gvFiles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvFiles.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["PoDocs"];
                gvFiles.DataSource = SortDataTable(dt as DataTable, true);
                gvFiles.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdPendingTC_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdPendingTC.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["TOTALTC"];
                grdPendingTC.DataSource = SortDataTable(dt as DataTable, true);
                grdPendingTC.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdDIdocs_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdDIdocs.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["DiDocs"];
                grdDIdocs.DataSource = SortDataTable(dt as DataTable, true);
                grdDIdocs.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void gvFiles_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
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
            try
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

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return GridViewSortDirection;
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
        bool ValidateForm()
        {
            string strpath = System.IO.Path.GetExtension(fupFile.FileName);

            string filename = Path.GetFileName(fupFile.PostedFile.FileName);
            bool bValidate = false;
            if (ViewState["DiDetails"] == null)
            {
                if (txtDINumber.Text.Trim() == string.Empty)
                {
                    ShowMsgBox("Please Enter DI Number");
                    txtDINumber.Focus();
                    return bValidate;
                }
                if (txtDIDate.Text.Trim() == string.Empty)
                {
                    ShowMsgBox("Please select DI Date");
                    txtDIDate.Focus();
                    return bValidate;
                }
                
                if (cmbStore.SelectedIndex == 0)
                {
                    ShowMsgBox("Please Select Store");
                    cmbStore.Focus();
                    return bValidate;
                }
                if (cmbMake.SelectedIndex == 0)
                {
                    ShowMsgBox("Please Select Make");
                    cmbMake.Focus();
                    return bValidate;
                }
                if (cmbCapacity.SelectedIndex == 0)
                {
                    ShowMsgBox("Please Select Capacity");
                    cmbCapacity.Focus();
                    return bValidate;
                }
                if (txtQuantity.Text.Trim() == string.Empty)
                {
                    ShowMsgBox("Please Enter Quantity");
                    txtQuantity.Focus();
                    return bValidate;
                }

                if (cmbRating.SelectedIndex == 0)
                {
                    ShowMsgBox("Please Select the Rating");
                    cmbRating.Focus();
                    return bValidate;
                }
                if (fupFile.PostedFile != null)
                {
                    if (fupFile.PostedFile.ContentLength != 0)
                    {

                        string strExt = filename.Substring(filename.LastIndexOf('.') + 1);


                        string sFileExt = Convert.ToString(ConfigurationManager.AppSettings["FileFormat"]);
                        string sAnxFileExt = System.IO.Path.GetExtension(fupFile.FileName).ToString().ToLower();
                        sAnxFileExt = ";" + sAnxFileExt.Remove(0, 1) + ";";

                        if (!sFileExt.Contains(sAnxFileExt))
                        {
                            ShowMsgBox("Invalid File Format");
                            return false;
                        }
                    }
                }


            }
            bValidate = true;
            return bValidate;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        // to download the file
        protected void DownloadFile(object sender, EventArgs e)
        {

            string fileName = (sender as LinkButton).CommandArgument;

            try
            {
                string SFTPmainfolderpath = Convert.ToString(ConfigurationManager.AppSettings["VirtualDirectoryPMCDocs"]);
                string PoNo = Regex.Replace(txtPoNumber.Text, @"[^0-9a-zA-Z]+", "");
                clsSFTP objFtp = new clsSFTP(SFTPPath, UserName, Password);

                string path = SFTPmainfolderpath + "PMC_PO_DOCS/" + PoNo + "/" + fileName;
                ClientScript.RegisterStartupScript(this.GetType(), "Print", "<script>window.open('" + path + "','_blank')</script>");
            }
            catch (WebException ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                 MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

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
                    string SFTPmainfolder = Convert.ToString(ConfigurationManager.AppSettings["VirtualDirectoryPMCDocs"]);

                    string PoNo = Regex.Replace(txtPoNumber.Text, @"[^0-9a-zA-Z]+", "");
                    clsSFTP objFtp = new clsSFTP(SFTPPath, UserName, Password);

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
                    resp.AddHeader("Content-Length", Convert.ToString(fileResp.ContentLength));

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
                    clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                     System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                }

            }

        }

        protected void DownloadFiledwnld1(object sender, EventArgs e)
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
                    string SFTPmainfolderpath = Convert.ToString(ConfigurationManager.AppSettings["VirtualDirectoryPMCDocs"]);

                    string DINo = Regex.Replace(txtDINumber.Text, @"[^0-9a-zA-Z]+", "");
                    clsSFTP objFtp = new clsSFTP(SFTPPath, UserName, Password);

                    string url = SFTPmainfolderpath + "PMC_DI_DOCS/" + DINo + "/" + fileName1;
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
                    resp.AddHeader("Content-Length", Convert.ToString(fileResp.ContentLength));

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
                    clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                     System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                }

            }

        }


        private string getFilename(string hreflink)
        {
            Uri uri = new Uri(hreflink);

            string filename = System.IO.Path.GetFileName(uri.LocalPath);

            return filename;
        }
        protected void DownloadFile1(object sender, EventArgs e)
        {

            string fileName = (sender as LinkButton).CommandArgument;

            try
            {
                string SFTPmainfolderpath = Convert.ToString(ConfigurationManager.AppSettings["VirtualDirectoryPMCDocs"]);
                string DINo = Regex.Replace(txtDINumber.Text, @"[^0-9a-zA-Z]+", "");

                clsSFTP objFtp = new clsSFTP(SFTPPath, UserName, Password);

                string path = SFTPmainfolderpath + "PMC_DI_DOCS/" + DINo + "/" + fileName;
                ClientScript.RegisterStartupScript(this.GetType(), "Print", "<script>window.open('" + path + "','_blank')</script>");

            }
            catch (WebException ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

    }
}