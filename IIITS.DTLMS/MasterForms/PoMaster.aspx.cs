using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.IO;
using System.Collections;
using System.Net;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Configuration;

namespace IIITS.DTLMS.MasterForms
{
    public partial class PoMaster : System.Web.UI.Page
    {
        string strFormCode = "PoMaster";
        public int gridRowId;
        clsSession objSession;
        string sFileServerPath = Convert.ToString(ConfigurationManager.AppSettings["EstimatioinVirtualPath"]);
        string sUserName = Convert.ToString(ConfigurationManager.AppSettings["FTP_USER"]);
        string sPassword = Convert.ToString(ConfigurationManager.AppSettings["FTP_PASS"]);
        string SFTPPath = Convert.ToString(ConfigurationManager.AppSettings["SFTPPath"]);
        string SFTPmainfolder = Convert.ToString(ConfigurationManager.AppSettings["SFTPmainfolder"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                Update.Visible = false;
                UpdatePO.Visible = false;

                lblMessage.Text = string.Empty;
                objSession = (clsSession)Session["clsSession"];
                Form.DefaultButton = btnSave.UniqueID;
                txtDelivery.Attributes.Add("readonly", "readonly");
                txtDate.Attributes.Add("readonly", "readonly");

                ManufactureCalender.EndDate = System.DateTime.Now;
                //DeliveryCalendar.EndDate = System.DateTime.Now;
                //DeliveryCalendar.StartDate = System.DateTime.Now;

                if (!IsPostBack)
                {

                    LoadComboField();
                    CheckAccessRights("4");

                    if (Request.QueryString["QryPoId"] != null && Request.QueryString["QryPoId"].ToString() != "")
                    {
                        txtPoId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryPoId"]));
                        string TotQnty = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryPoQnty"]));
                        LoadPoDetails(txtPoId.Text);
                        Create.Visible = false;
                        CreatePO.Visible = false;
                        //lnkDwnld.Visible = true;
                        Update.Visible = true;
                        UpdatePO.Visible = true;
                        DivUpload.Visible = true;

                        cmbSupplier.Enabled = false;
                        //blNote.Text = "*NOTE: Please Ensure That Before Updating, Check Total Quantity should be Equal to the Old Quantity of " + TotQnty + "";
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

                strQry = "SELECT \"TM_ID\",\"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE (\"TM_STATUS\"='A' AND TO_CHAR(COALESCE (\"TM_EFFECT_FROM\",NOW()),'YYYYMMDD') <= TO_CHAR(NOW(),'YYYYMMDD'))";
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

        public void LoadPoDetails(string strPoId)
        {
            try
            {
                clsPoMaster objPoMaster = new clsPoMaster();
                objPoMaster.sPoId = Convert.ToString(strPoId);
                objPoMaster.GetPoDetails(objPoMaster);

                txtPoNumber.Text = objPoMaster.sPoNo;
                txtDate.Text = objPoMaster.sDate;
                txtRate.Text = objPoMaster.sPoRate;
                cmbSupplier.SelectedValue = objPoMaster.sSupplierId;
                txtPoNumber.Enabled = false;
                txtDate.Enabled = false;
                //cmbSupplier.Enabled = false;
                txtRate.Enabled = false;
                txtDelivery.Text = objPoMaster.sDeliveryDate;

                LoadTcCapacity(txtPoId.Text);
                BindgridView(SFTPmainfolder, sUserName, sPassword);
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

                clsPoMaster objPoMaster = new clsPoMaster();
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
                        if (ddlCapacity.SelectedItem.Text == Convert.ToString(dt.Rows[i]["PB_CAPACITY"]) && ddlMake.SelectedItem.Text == Convert.ToString(dt.Rows[i]["PB_MAKE"])
                            && cmbRating.SelectedValue == Convert.ToString(dt.Rows[i]["PB_STARRATE"]))
                        {
                            //ShowMsgBox("Capacity("+ dt.Rows[i]["PB_CAPACITY"] + ")-MakeName(" + dt.Rows[i]["PB_MAKE"] + ") Combination Already Added");
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
                    //lblFilename.Text = fupdDoc.FileName;
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
                    dt.Columns.Add("PO_ID");
                    dt.Columns.Add("PO_NO");
                    dt.Columns.Add("PB_CAPACITY");
                    dt.Columns.Add("PB_CAPACITY_ID");
                    dt.Columns.Add("PB_MAKE");
                    dt.Columns.Add("MAKE_ID");
                    dt.Columns.Add("PB_STARRATE");
                    dt.Columns.Add("PB_STAR_NAME");
                    dt.Columns.Add("PB_QUANTITY");
                }
                else
                {
                    //load datatble from viewstate
                    dt = (DataTable)ViewState["dt"];
                }
                DataRow dRow = dt.NewRow();
                int qnty = Convert.ToInt32(txtQuantity.Text);

                dRow["PB_QUANTITY"] = qnty;
                if (Convert.ToString(dRow["PB_QUANTITY"]) == "0")
                {
                    ShowMsgBox("Quantity Should Not be Zero");
                    return;
                }
                dRow["PB_CAPACITY"] = ddlCapacity.SelectedItem.Text;
                dRow["PB_CAPACITY_ID"] = ddlCapacity.SelectedValue;
                dRow["PB_MAKE"] = ddlMake.SelectedItem.Text;
                dRow["MAKE_ID"] = ddlMake.SelectedValue;
                dRow["PO_NO"] = txtPoNumber.Text;
                dRow["PB_STARRATE"] = cmbRating.SelectedValue;
                dRow["PB_STAR_NAME"] = cmbRating.SelectedItem.Text;

                if (ViewState["gridRowId"] != null)
                {
                    // gridid.Text = ViewState["gridRowId"].ToString();
                    int i = Convert.ToInt32(ViewState["gridRowId"]);
                    dt.Rows[i].SetField("PB_QUANTITY", qnty);
                    dt.Rows[i].SetField("PB_CAPACITY", ddlCapacity.SelectedItem.Text);
                    dt.Rows[i].SetField("PB_CAPACITY_ID", ddlCapacity.SelectedValue);
                    dt.Rows[i].SetField("MAKE_ID", ddlMake.SelectedValue);
                    dt.Rows[i].SetField("PB_STARRATE", cmbRating.SelectedValue);
                    dt.Rows[i].SetField("PB_STAR_NAME", cmbRating.SelectedItem.Text);
                    dt.Rows[i].SetField("PO_NO", txtPoNumber.Text);

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
                //btnSave.Enabled = true;
                //btnReset.Enabled = true;

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
                clsPoMaster objPoMaster = new clsPoMaster();
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
                        ShowMsgBox("Please Add Capacity and Quantity ");
                        return;
                    }



                    SavePoMaster();
                }
                else
                {
                    ShowMsgBox("Please Add Capacity and Quantity");
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
                string SFTPPath = Convert.ToString(ConfigurationManager.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(ConfigurationManager.AppSettings["VirtualDirectoryDocs"]);

                string PoNo = Regex.Replace(txtPoNumber.Text, @"[^0-9a-zA-Z]+", "");
                clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);

                string path = SFTPmainfolder + "PO_DOCS/" + PoNo + "/" + fileName;
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
                    string SFTPmainfolder = Convert.ToString(ConfigurationManager.AppSettings["VirtualDirectoryDocs"]);
                    string PoNo = Regex.Replace(txtPoNumber.Text, @"[^0-9a-zA-Z]+", "");
                    clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);
                    string url = SFTPmainfolder + "PO_DOCS/" + PoNo + "/" + fileName1;
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
                string SFTPPath = Convert.ToString(ConfigurationManager.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(ConfigurationManager.AppSettings["SFTPmainfolder"]);

                string PoNo = Regex.Replace(txtPoNumber.Text, @"[^0-9a-zA-Z]+", "");
                FtpServer += "PO_DOCS/" + PoNo;
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
            clsPoMaster objPoMaster = new clsPoMaster();
            DataTable dt;
            string SFTPPath = Convert.ToString(ConfigurationManager.AppSettings["SFTPPath"]);
            string SFTPmainfolder = Convert.ToString(ConfigurationManager.AppSettings["SFTPmainfolder"]);
            clsSFTP objFtp = new clsSFTP(SFTPPath, sUserName, sPassword);
            bool Isuploaded;
            string sMainFolderName = "PO_DOCS";

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
                        ShowMsgBox("Please upload the Purchase Order Note ");
                        fupdDoc.Focus();
                        return;
                    }
                }
                if (ValidateForm() == true)
                {
                    string strpath = System.IO.Path.GetExtension(fupdDoc.FileName);
                    string filename = Path.GetFileName(fupdDoc.PostedFile.FileName);
                    string[] Arr = new string[2];
                    dt = (DataTable)ViewState["dt"];
                    objPoMaster.sCrBy = objSession.UserId;
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
                                string sFileExt = Convert.ToString(ConfigurationManager.AppSettings["FileFormat"]);
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
                                    // IsFileExiest = objFtp.IsfileExiest(sMainFolderName + "/" + cmbDivision.Text + "/" + cmbRepairer.Text);
                                    //if (IsFileExiest == false)
                                    //{
                                    Isuploaded = objFtp.upload(SFTPmainfolder + sMainFolderName + "/" + PoNo + "/", filename, sDirectory);
                                    if (Isuploaded == true & File.Exists(sDirectory))
                                    {
                                        File.Delete(sDirectory);
                                        sDirectory = PoNo + "/" + filename;
                                        ShowMsgBox(Arr[0]);
                                    }
                                }

                                Session["FileUpload"] = null;
                            }

                            //else
                            //{
                            //    Stream strm = fupdDoc.PostedFile.InputStream;
                            //    Buffer = new byte[strm.Length];
                            //    strm.Read(Buffer, 0, (int)strm.Length);
                            //}
                        }
                        else
                        {
                            ShowMsgBox("Please upload the File");
                            return;
                        }
                    }



                    //if (objSession.sTransactionLog == "1")
                    //{
                    //    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "PO Master");
                    //}

                    if (Arr[1].ToString() == "0")
                    {
                        txtPoId.Text = objPoMaster.sPoId;
                        btnSave.Text = "Update";
                        ShowMsgBox(Arr[0]);
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
                            // dataView.Sort = string.Format("{0} {1} ", GridViewSortExpression, GridViewSortDirection);
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
                            // dataView.Sort = string.Format("{0} {1} ", GridViewSortExpression, GetSortDirection());
                            ViewState["dt"] = dataView.ToTable(); ;


                        }


                        //dv.Sort = string.Format("{0} {1} ", GridViewSortExpression, GetSortDirection());

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
                Response.Redirect("PoMasterView.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        //protected void GridView1_RowEditing(object sender,  GridViewEditEventArgs e)
        //{
        //    try
        //    {

        //      if (e.CommandName == "Edit")
        //        {                                      
        //                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
        //                Label lblCapacity = (Label)row.FindControl("lblCapacity");
        //                Label lblRating = (Label)row.FindControl("lblRating");
        //                Label lblQuantity = (Label)row.FindControl("lblQuantity");
        //                Label lblMake = (Label)row.FindControl("lblMake");

        //                //objPoMaster.sCapacity=lblCapacity.Text;
        //                //objPoMaster.sTcMake=lblMake.Text;
        //                //objPoMaster.sTcQuantity=lblQuantity.Text;
        //                //objPoMaster.sPoRate=lblRating.Text;
        //                //to remove selected Capacity from grid


        //                    ddlCapacity.SelectedItem.Text = lblCapacity.Text;
        //                    ddlMake.SelectedItem.Text = lblMake.Text;
        //                    cmbRating.SelectedValue = lblRating.Text; ;
        //                    txtQuantity.Text = lblQuantity.Text ;

        //                    return;
        //                }

        //        }



        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}


        protected void grdPoMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            clsPoMaster objPoMaster = new clsPoMaster();
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
                        if (lblCapacity.Text == Convert.ToString(dt.Rows[i]["PB_CAPACITY"]) && lblRating.Text == Convert.ToString(dt.Rows[i]["PB_STARRATE"]) && lblQuantity.Text == Convert.ToString(dt.Rows[i]["PB_QUANTITY"]) && lblMake.Text == Convert.ToString(dt.Rows[i]["PB_MAKE"]))
                        {
                            objPoMaster.sPoId = lblPoId.Text;
                            objPoMaster.sCapacity = lblCapacity.Text;
                            objPoMaster.sTcMake = lblMakeId.Text;
                            DICount = objPoMaster.LoadDeliveredCount(objPoMaster);
                            if (DICount.Rows.Count > 0)
                            {
                                int delivered = Convert.ToInt32(DICount.Rows[0]["DELIVERED"]);

                                string Msg = Convert.ToString(delivered);
                                ShowMsgBox("This Capacity Already Delivered To Some Store  Quantity " + Msg + ", So You Can`t Delete This Record , For Reference DI Number:" + Convert.ToString(DICount.Rows[0]["DI_NO"]));
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
                    //clsPoMaster objPoMast = new clsPoMaster();
                    //objPoMast.sRowIndex = row.RowIndex;
                    int sRowIndex = row.RowIndex;
                    //ViewState["gridRowId"] = row.RowIndex;
                    Label lblCapacity = (Label)row.FindControl("lblCapacity");
                    Label lblCapacityid = (Label)row.FindControl("lblCapacityID");
                    Label lblRating = (Label)row.FindControl("lblRatingId");
                    Label lblQuantity = (Label)row.FindControl("lblQuantity");
                    Label lblMakeid = (Label)row.FindControl("lblMakeId");
                    Label lblMake = (Label)row.FindControl("lblMake");
                    Label lblPoId = (Label)row.FindControl("lblPOId");
                    if (lblCapacity.Text == Convert.ToString(dt.Rows[sRowIndex]["PB_CAPACITY"]) && lblRating.Text == Convert.ToString(dt.Rows[sRowIndex]["PB_STARRATE"]) && lblQuantity.Text == Convert.ToString(dt.Rows[sRowIndex]["PB_QUANTITY"]) && lblMake.Text == Convert.ToString(dt.Rows[sRowIndex]["PB_MAKE"]))
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
                    //objPoMaster.sCapacity=lblCapacity.Text;
                    //objPoMaster.sTcMake=lblMake.Text;
                    //objPoMaster.sTcQuantity=lblQuantity.Text;
                    //objPoMaster.sPoRate=lblRating.Text;
                    //to remove selected Capacity from grid


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

                objApproval.sFormName = "PoMaster";
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
            if (txtPoNumber.Text.Trim().StartsWith("."))
            {
                txtPoNumber.Focus();
                ShowMsgBox("Enter valid PO Number");
                return false;
            }
            DateTime PoDate = DateTime.ParseExact(txtDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime DeliveryDate = DateTime.ParseExact(txtDelivery.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (PoDate > DeliveryDate)
            {
                txtDelivery.Focus();
                ShowMsgBox("Delivery Date Should be Greater Than or Equal to PO Date");
                return false;
            }

            if (txtRate.Text.Trim() != "")
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtRate.Text, "^(\\d{1,12})?(\\.\\d{1,2})?$"))
                {
                    txtRate.Focus();
                    ShowMsgBox("Enter valid price (eg:111111111111.00)");
                    return false;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(txtRate.Text, "[-+]?[0-9]{0,11}\\.?[0-9]{0,2}"))
                {
                    txtRate.Focus();
                    ShowMsgBox("Enter valid price (eg:111111111111.00)");
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
                clsPoMaster objPomaster = new clsPoMaster();
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

        protected void ValidateDeliveryScheduled(object sender, EventArgs e)
        {
            try
            {
                DateTime PoDate = DateTime.Now;
                DateTime DeliveryScheduledDate = DateTime.Now;

                if (txtDate.Text.Trim() != "")
                {
                    PoDate = DateTime.ParseExact(txtDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (txtDelivery.Text.Trim() != "")
                {
                    DeliveryScheduledDate = DateTime.ParseExact(txtDelivery.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                if (txtDate.Text.Trim() == "")
                {
                    txtDate.Focus();
                    ShowMsgBox("Please Select PO Date");
                    txtDelivery.Text = string.Empty;
                    return;
                }

                if (DeliveryScheduledDate < PoDate)
                {
                    txtDelivery.Focus();
                    ShowMsgBox("Delivery Scheduled Date Should Greater Than or Equal to PO Date");
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

    }
}