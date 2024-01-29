using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using IIITS.DTLMS.BL.Billing;
using System.IO;
using System.Configuration;
using System.Net;

namespace IIITS.DTLMS.Billing
{
    public partial class MajorDTRBilling : System.Web.UI.Page
    {
        string strFormCode = "MajorDTRBilling";
        clsSession objSession;
        string sEstNo = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];
                txtInvoiceDate.Attributes.Add("readonly", "readonly");


                CalendarExtender1_txtInvoiceDate.EndDate = System.DateTime.Now;

                if (!IsPostBack)
                {
                    LoadSearchWindow();

                    if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
                    {
                        txtActionType.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));
                    }
                    if (Request.QueryString["WOId"] != null && Request.QueryString["WOId"].ToString() != "")
                    {
                        txtWoId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["WOId"]));
                    }

                    if (txtActionType.Text == "V")
                    {
                        cmdSave.Text = "View";
                        cmdSave.Enabled = false;
                    }

                    if (Request.QueryString["EstId"] != null && Request.QueryString["EstId"].ToString() != "")
                    {
                        txtEstNo.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["EstId"]));
                        txtInvId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["InvId"]));
                        txtActionType.Text = "V";
                        cmdCalc.Enabled = false;
                        cmdSave.Enabled = false;
                        GetDatafromMainTable();
                    }
                    else
                    {
                        WorkFlowConfig();
                    }
                }

                if (txtActionType.Text == "M")
                {
                    cmdSave.Text = "Modify And Approve";
                }
                else if (txtActionType.Text == "R")
                {
                    cmdSave.Text = "Reject";
                }
                else if (txtActionType.Text == "V")
                {
                    cmdSave.Text = "View";
                    cmdReset.Enabled = false;
                    dvComments.Style.Add("display", "none");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void WorkFlowConfig()
        {
            try
            {
                if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
                {
                    if (Session["WFOId"] != null && Session["WFOId"].ToString() != "")
                    {
                        hdfWFDataId.Value = Session["WFDataId"].ToString();
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
                        GetDataFromXML(hdfWFDataId.Value);

                        if (txtActionType.Text == "M")
                        {
                            cmdCalc.Visible = true;
                        }
                        else
                        {
                            cmdCalc.Visible = false;
                        }
                    }

                    dvComments.Style.Add("display", "block");

                    if (hdfWFOAutoId.Value != "0")
                    {
                        dvComments.Style.Add("display", "none");
                    }
                    if (txtActionType.Text == "V")
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
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetDatafromMainTable()
        {
            try
            {
                clsMajorBilling obj = new clsMajorBilling();
                obj.sInvID = txtInvId.Text;
                obj.sEstNo = txtEstNo.Text;
                obj.GetEstimationDetails(obj);
                txtEstId.Text = obj.sEstID;
                txtEstDate.Text = obj.sEstDate;
                txtEstAmount.Text = obj.sEstAmount;
                txtWoNo.Text = obj.sWoNo;
                txtWoDate.Text = obj.sWODate;
                txtWoAmount.Text = obj.sWOAmount;
                txtFailtype.Text = obj.sFailtype;
                txtGuranteetype.Text = obj.sGuaranteetype;
                txtWorkOrderID.Text = obj.sWOId;
                obj.GetInvoiceDetails(obj);
                txtInvoiceNo.Text = obj.sInvNo;
                txtInvoiceDate.Text = obj.sInvDate;
                txtInvAmount.Text = obj.sInvAmount;
                txtBrNo.Text = obj.sBRNo;
                txtbooked.Text = obj.bookamnt;
                txtFilepath.Text = obj.sFilePath;
                lnkDownloadFile.Enabled = true;
                obj.GetActualMaterialDetails(obj);
                grdMaterialMast.DataSource = obj.dtEstMatDetails;
                obj.GetActualLabourDetails(obj);
                grdLabourMast.DataSource = obj.dtEstLabDetails;
                obj.GetActualSalvageDetails(obj);
                grdSalvageMast.DataSource = obj.dtEstSalDetails;
                grdMaterialMast.DataBind();
                grdLabourMast.DataBind();
                grdSalvageMast.DataBind();
                grdMaterialMast.Columns[2].Visible = false;
                grdLabourMast.Columns[2].Visible = false;
                grdSalvageMast.Columns[2].Visible = false;
                CalculateTotal();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetDataFromXML(string sWFDataId)
        {
            try
            {
                clsMajorBilling obj = new clsMajorBilling();
                obj.sWFDataId = sWFDataId;
                obj.GetEstimateDetailsFromXML(obj);
                txtEstId.Text = obj.sEstID;
                txtEstNo.Text = obj.sEstNo;
                txtEstDate.Text = obj.sEstDate;
                txtEstAmount.Text = obj.sEstAmount;
                txtWoNo.Text = obj.sWoNo;
                txtWoDate.Text = obj.sWODate;
                txtWoAmount.Text = obj.sWOAmount;
                txtFailtype.Text = obj.sFailtype;
                txtGuranteetype.Text = obj.sGuaranteetype;
                txtWorkOrderID.Text = obj.sWOId;
                txtInvoiceNo.Text = obj.sInvNo;
                txtInvoiceDate.Text = obj.sInvDate;
                txtInvAmount.Text = obj.sInvAmount;
                txtFilepath.Text = obj.sFilePath;
                txtBrNo.Text = obj.sBRNo;
                txtbooked.Text = obj.bookamnt;
                lnkDownloadFile.Visible = true;
                grdCharges.DataSource = obj.dtEstDetails;
                grdCharges.DataBind();
                grdMaterialMast.DataSource = obj.dtEstMatDetails;
                grdMaterialMast.DataBind();
                grdLabourMast.DataSource = obj.dtEstLabDetails;
                grdLabourMast.DataBind();
                grdSalvageMast.DataSource = obj.dtEstSalDetails;
                grdSalvageMast.DataBind();
                CalculateTotal();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        private string getFilename(string hreflink)
        {
            Uri uri = new Uri(hreflink);

            string filename = System.IO.Path.GetFileName(uri.LocalPath);

            return filename;
        }

        private void DownloadFile(string sFilePath)
        {
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
                    string sVirtualDirpath = Convert.ToString(ConfigurationSettings.AppSettings["VirtualDirectoryDocs"]);
                    string url = sVirtualDirpath + sFilePath;
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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadSearchWindow()
        {
            try
            {
                clsStoreIndent obj = new clsStoreIndent();
                if (objSession.sRoleType == "2")
                {
                    string officecode =clsStoreOffice.GetOfficeCode(objSession.OfficeCode, "RESTD_OFF_CODE");

                    string strQry = string.Empty;
                    strQry = "Title=Search and Select Purchase Order Details&";
                    strQry += "Query=SELECT \"RESTD_NO\", \"TR_NAME\" FROM \"TBLREPAIRERESTIMATIONDETAILS\", \"TBLTRANSREPAIRER\", \"TBLREPAIRERWORKORDER\" WHERE \"RWO_EST_ID\" = \"RESTD_ID\"  AND \"RESTD_REPAIRER\" = \"TR_ID\" AND  ";
                     strQry +=  " \"RESTD_ID\" NOT IN (SELECT \"MJB_EST_ID\" FROM \"TBLMAJORBILLING\") and " +officecode+" ";
                    strQry += "and  cast(\"RESTD_NO\" as varchar) not in(SELECT \"WO_DATA_ID\" from \"TBLWORKFLOWOBJECTS\" WHERE \"WO_DATA_ID\"=cast(\"RESTD_NO\" as varchar) and  \"WO_APPROVE_STATUS\"=0 and  \"WO_BO_ID\"=78 )";
                   strQry += "AND CAST({0} AS TEXT) like %{1}% order by \"RESTD_NO\" &";
                    strQry += "DBColName=\"RESTD_NO\"~\"TR_NAME\"&";
                    strQry += "ColDisplayName=Est Number~Repairer&";
                    strQry = strQry.Replace("'", @"\'");
                    cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtEstNo.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + txtEstNo.ClientID + ")");

                }
                else
                {
                    string officecode = objSession.OfficeCode;

                    string strQry = string.Empty;
                    strQry = "Title=Search and Select Purchase Order Details&";
                    strQry += "Query=SELECT \"RESTD_NO\", \"TR_NAME\" FROM \"TBLREPAIRERESTIMATIONDETAILS\", \"TBLTRANSREPAIRER\", \"TBLREPAIRERWORKORDER\" WHERE \"RWO_EST_ID\" = \"RESTD_ID\"  AND ";
                       strQry += " \"RESTD_REPAIRER\" = \"TR_ID\" AND  \"RESTD_ID\" NOT IN (SELECT \"MJB_EST_ID\" FROM \"TBLMAJORBILLING\") and cast(\"RESTD_OFF_CODE\" as text) like '" + objSession.OfficeCode + "%'";
                    strQry += "and  cast(\"RESTD_NO\" as varchar) not in(SELECT \"WO_DATA_ID\" from \"TBLWORKFLOWOBJECTS\" WHERE \"WO_DATA_ID\"=cast(\"RESTD_NO\" as varchar) and  \"WO_APPROVE_STATUS\"=0 and  \"WO_BO_ID\"=78 )";
                    strQry += "AND CAST({0} AS TEXT) like %{1}% order by \"RESTD_NO\" &";
                    strQry += "DBColName=\"RESTD_NO\"~\"TR_NAME\"&";
                    strQry += "ColDisplayName=Est Number~Repairer&";
                    strQry = strQry.Replace("'", @"\'");
                    cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtEstNo.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + txtEstNo.ClientID + ")");

                }

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
                string sEstNo = txtEstNo.Text;
                clsMajorBilling obj = new clsMajorBilling();
                obj.sEstNo = txtEstNo.Text;

                obj.sOfficeCode = objSession.OfficeCode;
                obj.sUserId = objSession.UserId;

                obj.GetEstimationDetails(obj);
                txtEstId.Text = obj.sEstID;
                txtEstDate.Text = obj.sEstDate;
                txtEstAmount.Text = obj.sEstAmount;
                txtWorkOrderID.Text = obj.sWOId;
                txtWoNo.Text = obj.sWoNo;
                txtWoDate.Text = obj.sWODate;
                txtWoAmount.Text = obj.sWOAmount;
                txtFailtype.Text = obj.sFailtype;
                txtGuranteetype.Text = obj.sGuaranteetype;
                txtWorkOrderID.Text = obj.sWOId;
                grdCharges.DataSource = obj.dtEstDetails;
                grdCharges.DataBind();
                grdMaterialMast.DataSource = obj.dtEstMatDetails;
                grdMaterialMast.DataBind();
                grdLabourMast.DataSource = obj.dtEstLabDetails;
                grdLabourMast.DataBind();
                grdSalvageMast.DataSource = obj.dtEstSalDetails;
                grdSalvageMast.DataBind();
                CalculateTotal();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtInvoiceNo.Text.Trim() == string.Empty)
                {
                    ShowMsgBox("Please Enter the Invoice Number");
                    txtInvoiceNo.Focus();
                    return;
                }

                if (txtInvoiceDate.Text.Trim() == string.Empty)
                {
                    ShowMsgBox("Please Enter the Invoice Date");
                    txtInvoiceDate.Focus();
                    return;
                }

                if (txtInvAmount.Text.Trim() == string.Empty)
                {
                    ShowMsgBox("Please Enter the Invoice Amount");
                    txtInvAmount.Focus();
                    return;
                }
                if (fupInvoice.HasFile == false && txtActionType.Text == "")
                {
                    ShowMsgBox("Please uplaod the Invoice");
                    fupInvoice.Focus();
                    return;
                }

                if (objSession.RoleId == "24")
                {
                    if (txtbooked.Text.Trim() == string.Empty)
                    {
                        ShowMsgBox("Please Enter the Expenditure booked  Amount");
                        txtbooked.Focus();
                        return;
                    }
                }
                if (objSession.RoleId == "27")
                {
                    if (txtBrNo.Text.Trim() == string.Empty)
                    {
                        ShowMsgBox("Please Enter the BR Number");
                        txtBrNo.Focus();
                        return;
                    }
                }

                if (txtActionType.Text == "A" || txtActionType.Text == "R" || txtActionType.Text == "D")
                {
                    if (hdfWFDataId.Value != "0")
                    {
                        ApproveRejectAction();
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "Billing");
                        }
                        return;
                    }
                }

                CalculateTotal();

                string sDirectory = String.Empty;
                string sFileName = string.Empty;

                if (fupInvoice.HasFile == true)
                {
                    string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["MaterialallotmentFormat"]);
                    string sAnxFileExt = System.IO.Path.GetExtension(fupInvoice.FileName).ToString().ToLower();
                    sAnxFileExt = ";" + sAnxFileExt.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sAnxFileExt))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return;
                    }

                    sFileName = Path.GetFileName(fupInvoice.PostedFile.FileName);
                    sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sFileName);
                    fupInvoice.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sFileName));
                    Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sFileName);
                }

                clsMajorBilling obj = new clsMajorBilling();
                obj.sInvNo = txtInvoiceNo.Text.Replace("'", "").Trim();
                obj.sInvDate = txtInvoiceDate.Text.Replace("'", "").Trim();
                obj.sInvAmount = txtInvAmount.Text.Replace("'", "").Trim();
                obj.sFilePath = sDirectory;
                obj.sFileName = sFileName;
                obj.sEstFinalRate = txtRate.Text;
                obj.sEstFinalTax = txtTax.Text;
                obj.sEstFinalAmount = txtAmount.Text;
                obj.sEstID = txtEstId.Text;
                obj.sEstNo = txtEstNo.Text;
                obj.sUserId = objSession.UserId;
                obj.sOfficeCode = objSession.OfficeCode;
                obj.sActualFilePath = sDirectory;
                obj.sBRNo = txtBrNo.Text;
                obj.bookamnt = txtbooked.Text;

                int i = 0;
                string[] sMateriallist = new string[grdMaterialMast.Rows.Count];
                foreach (GridViewRow row in grdMaterialMast.Rows)
                {
                    if (((TextBox)row.FindControl("txtMqty")).Text.Length > 0)
                    {
                        sMateriallist[i] = ((Label)row.FindControl("lblMaterialId")).Text.Trim() + "~" + ((Label)row.FindControl("lblMaterialName")).Text.Trim() + "~" +
                            ((TextBox)row.FindControl("txtMqty")).Text.Trim() + "~" + ((Label)row.FindControl("lblMatunit")).Text.Trim() + "~" +
                            ((Label)row.FindControl("lblMatunitName")).Text.Trim() + "~" + ((Label)row.FindControl("lblBaserate")).Text.Trim() + "~" +
                            ((Label)row.FindControl("lbltax")).Text.Trim() + "~" + row.Cells[8].Text + "~" + row.Cells[9].Text + "~" + row.Cells[10].Text;
                        //((Label)row.FindControl("lblMatAmount")).Text.Trim() + "~" +  ((Label)row.FindControl("lblMatTax")).Text.Trim() + "~" + ((Label)row.FindControl("lblTotal")).Text.Trim() ;
                    }
                    i++;
                }



                int j = 0;
                string[] sLabourlist = new string[grdLabourMast.Rows.Count];
                foreach (GridViewRow row in grdLabourMast.Rows)
                {
                    if (((TextBox)row.FindControl("txtLqty")).Text.Length > 0)
                    {
                        sLabourlist[j] = ((Label)row.FindControl("lblLabourId")).Text.Trim() + "~" + ((Label)row.FindControl("lblLabourName")).Text.Trim() + "~" +
                            ((TextBox)row.FindControl("txtLqty")).Text.Trim() + "~" + ((Label)row.FindControl("lblLabunit")).Text.Trim() + "~" +
                            ((Label)row.FindControl("lblLabunitName")).Text.Trim() + "~" + ((Label)row.FindControl("lbllabrate")).Text.Trim() + "~" +
                            ((Label)row.FindControl("lbllabtax")).Text.Trim() + "~" + row.Cells[8].Text + "~" + row.Cells[9].Text + "~" + row.Cells[10].Text;
                        //((Label)row.FindControl("lblLabAmount")).Text.Trim() + "~" + ((Label)row.FindControl("lblFinalLabTax")).Text.Trim() + "~" + ((Label)row.FindControl("lbllabtotal")).Text.Trim() ;
                    }
                    j++;
                }


                int k = 0;
                string[] sSalvageslist = new string[grdSalvageMast.Rows.Count];
                foreach (GridViewRow row in grdSalvageMast.Rows)
                {
                    if (((TextBox)row.FindControl("txtSqty")).Text.Length > 0)
                    {
                        sSalvageslist[k] = ((Label)row.FindControl("lblSalvageId")).Text.Trim() + "~" + ((Label)row.FindControl("lblSalvageName")).Text.Trim() + "~" +
                             ((TextBox)row.FindControl("txtSqty")).Text.Trim() + "~" + ((Label)row.FindControl("lblSalunit")).Text.Trim() + "~" +
                             ((Label)row.FindControl("lblSalunitName")).Text.Trim() + "~" + ((Label)row.FindControl("lblsalrate")).Text.Trim() + "~" +
                            ((Label)row.FindControl("lblsaltax")).Text.Trim() + "~" + row.Cells[8].Text + "~" + row.Cells[9].Text + "~" + row.Cells[10].Text;
                        //((Label)row.FindControl("lblSalAmount")).Text.Trim() + "~" + ((Label)row.FindControl("lblSalFinalTax")).Text.Trim() + "~" + ((Label)row.FindControl("lblsaltotal")).Text.Trim() ;
                    }
                    k++;
                }

                obj.MaterialList = sMateriallist;
                obj.LabourList = sLabourlist;
                obj.SalavageList = sSalvageslist;
                string[] Arr = new string[3];

                WorkFlowObjects(obj);

                #region Modify and Approve

                // For Modify and Approve
                if (txtActionType.Text == "M")
                {
                    if (txtComment.Text.Trim() == "")
                    {
                        ShowMsgBox("Enter Comments/Remarks");
                        txtComment.Focus();
                        return;

                    }
                    obj.sActionType = txtActionType.Text;
                    obj.sUserId = objSession.UserId;

                    Arr = obj.SaveInvoiceDetails(obj);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "Billing");
                    }
                    if (Arr[1].ToString() == "0")
                    {
                        hdfWFDataId.Value = obj.sWFDataId;
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
                Arr = obj.SaveInvoiceDetails(obj);

                if (objSession.sTransactionLog == "1")
                {
                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "Billing");
                }
                ShowMsgBox(Arr[0]);
                cmdSave.Enabled = false;
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
                objApproval.sWFObjectId = hdfWFOId.Value;
                objApproval.sWFAutoId = hdfWFOAutoId.Value;

                //Approve
                if (txtActionType.Text == "A")
                {
                    objApproval.sApproveStatus = "1";
                }
                //Reject
                if (txtActionType.Text == "R")
                {
                    objApproval.sApproveStatus = "3";
                }
                //Abort
                if (txtActionType.Text == "D")
                {
                    objApproval.sApproveStatus = "4";
                }
                //Modify and Approve
                if (txtActionType.Text == "M")
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
                if (txtActionType.Text == "M")
                {
                    objApproval.sWFDataId = hdfWFDataId.Value;
                    bResult = objApproval.ModifyApproveWFRequest(objApproval);
                }
                else
                {
                    bResult = objApproval.ApproveWFRequest(objApproval);
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
                    else if (objApproval.sApproveStatus == "4")
                    {

                        ShowMsgBox("Aborted Successfully");
                        cmdSave.Enabled = false;
                    }
                    else if (objApproval.sApproveStatus == "2")
                    {
                        ShowMsgBox("Modified and Approved Successfully");
                        cmdSave.Enabled = false;
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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void WorkFlowObjects(clsMajorBilling objBilling)
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


                objBilling.strFormCode = "MajorDTRBilling";
                objBilling.sOfficeCode = objSession.OfficeCode;
                objBilling.sClientIp = sClientIP;
                objBilling.sWFO_id = hdfWFOId.Value;
                objBilling.sWFAutoId = hdfWFOAutoId.Value;

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

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtInvoiceNo.Text = string.Empty;
                txtInvoiceDate.Text = string.Empty;
                txtInvAmount.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void CalculateTotal()
        {
            try
            {
                double MaterialTotal = 0;
                double MatAmount = 0;
                double MatTax = 0;
                double MatTotal = 0;
                double LabAmount = 0;
                double LabTax = 0;
                double LabTotal = 0;
                double SalAmount = 0;
                double SalTax = 0;
                double SalTotal = 0;

                foreach (GridViewRow row in grdMaterialMast.Rows)
                {
                    Label lbltotal = (Label)row.FindControl("lblTotal");
                    Label lblMTax = (Label)row.FindControl("lbltax");
                    Label lblMRate = (Label)row.FindControl("lblBaserate");
                    Label lblMatTax = (Label)row.FindControl("lblMatTax");
                    Label lblMatAmount = (Label)row.FindControl("lblMatAmount");
                    TextBox txtMqty = (TextBox)row.FindControl("txtMqty");
                    Label lblQuantity = (Label)row.FindControl("lblQuantity");
                    MatTotal = 0;


                    if (txtMqty.Text.Length > 0 && Convert.ToDouble(txtMqty.Text) > 0)
                    {
                        MatAmount = Convert.ToDouble(lblMRate.Text) * Convert.ToDouble(txtMqty.Text);
                        MatTax = (MatAmount / 100) * Convert.ToDouble(lblMTax.Text);
                        MatTotal = MatAmount + MatTax;

                        row.Cells[2].Text = txtMqty.Text;
                        row.Cells[8].Text = Convert.ToString(Math.Round(MatAmount, 2));
                        row.Cells[9].Text = Convert.ToString(Math.Round(MatTax, 2));
                        row.Cells[10].Text = Convert.ToString(Math.Round(MatTotal, 2));
                    }

                    if (lblQuantity.Text.Length > 0 && (txtMqty.Text.Length == 0 || Convert.ToDouble(txtMqty.Text) == 0))
                    {
                        row.Cells[2].Text = "";
                        row.Cells[8].Text = "";
                        row.Cells[9].Text = "";
                        row.Cells[10].Text = "";
                    }

                    MaterialTotal = MaterialTotal + MatTotal;

                }
                if (grdMaterialMast.Rows.Count > 0)
                {
                    grdMaterialMast.FooterRow.Cells[10].Text = Convert.ToString(Math.Round(MaterialTotal, 2));
                }


                double LabourTotal = 0;
                foreach (GridViewRow row in grdLabourMast.Rows)
                {
                    Label lbltotal = (Label)row.FindControl("lbllabtotal");
                    Label lblFinalLabTax = (Label)row.FindControl("lblFinalLabTax");
                    Label lbllabrate = (Label)row.FindControl("lbllabrate");
                    Label lbllabtax = (Label)row.FindControl("lbllabtax");
                    Label lblSalAmount = (Label)row.FindControl("lblLabAmount");
                    Label lbllabQuantity = (Label)row.FindControl("lbllabQuantity");

                    TextBox txtLqty = (TextBox)row.FindControl("txtLqty");
                    LabTotal = 0;



                    if (txtLqty.Text.Length > 0 && Convert.ToDouble(txtLqty.Text) > 0)
                    {
                        LabAmount = Convert.ToDouble(lbllabrate.Text) * Convert.ToDouble(txtLqty.Text);
                        LabTax = (LabAmount / 100) * Convert.ToDouble(lbllabtax.Text);
                        LabTotal = LabAmount + LabTax;

                        row.Cells[2].Text = txtLqty.Text;
                        row.Cells[8].Text = Convert.ToString(Math.Round(LabAmount, 2));
                        row.Cells[9].Text = Convert.ToString(Math.Round(LabTax, 2));
                        row.Cells[10].Text = Convert.ToString(Math.Round(LabTotal, 2));
                    }

                    if (lbllabQuantity.Text.Length > 0 && (txtLqty.Text.Length == 0 || Convert.ToDouble(txtLqty.Text) == 0))
                    {
                        row.Cells[2].Text = "";
                        row.Cells[8].Text = "";
                        row.Cells[9].Text = "";
                        row.Cells[10].Text = "";
                    }


                    LabourTotal = LabourTotal + LabTotal;

                }

                if (grdLabourMast.Rows.Count > 0)
                {
                    grdLabourMast.FooterRow.Cells[10].Text = Convert.ToString(Math.Round(LabourTotal, 2));
                }


                double SalvageTotal = 0;
                foreach (GridViewRow row in grdSalvageMast.Rows)
                {
                    Label lbltotal = (Label)row.FindControl("lblsaltotal");

                    Label lblSalFinalTax = (Label)row.FindControl("lblSalFinalTax");
                    Label lblsalrate = (Label)row.FindControl("lblsalrate");
                    Label lblsaltax = (Label)row.FindControl("lblsaltax");
                    Label lblSalAmount = (Label)row.FindControl("lblSalAmount");
                    Label lblSalQuantity = (Label)row.FindControl("lblSalQuantity");

                    TextBox txtSqty = (TextBox)row.FindControl("txtSqty");
                    SalTotal = 0;



                    if (txtSqty.Text.Length > 0 && Convert.ToDouble(txtSqty.Text) > 0)
                    {
                        SalAmount = Convert.ToDouble(lblsalrate.Text) * Convert.ToDouble(txtSqty.Text);
                        SalTax = (SalAmount / 100) * Convert.ToDouble(lblsaltax.Text);
                        SalTotal = SalAmount + SalTax;

                        row.Cells[2].Text = txtSqty.Text;
                        row.Cells[8].Text = Convert.ToString(Math.Round(SalAmount, 2));
                        row.Cells[9].Text = Convert.ToString(Math.Round(SalTax, 2));
                        row.Cells[10].Text = Convert.ToString(Math.Round(SalTotal, 2));
                    }

                    if (lblSalQuantity.Text.Length > 0 && (txtSqty.Text.Length == 0 || Convert.ToDouble(txtSqty.Text) == 0))
                    {
                        row.Cells[2].Text = "";
                        row.Cells[8].Text = "";
                        row.Cells[9].Text = "";
                        row.Cells[10].Text = "";
                    }


                    SalvageTotal = SalvageTotal + SalTotal;

                }
                if (grdSalvageMast.Rows.Count > 0)
                {
                    grdSalvageMast.FooterRow.Cells[10].Text = Convert.ToString(Math.Round(SalvageTotal, 2));
                }

                lblTotalCharges.Text = Convert.ToString(MaterialTotal + LabourTotal - SalvageTotal);
                txtRate.Text = Convert.ToString(MatAmount + LabAmount - SalAmount);
                txtTax.Text = Convert.ToString(MatTax + LabTax - SalTax);
                txtAmount.Text = Convert.ToString(Math.Round(MatTotal + LabTotal - SalTotal, 2));

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdCalc_Click(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        protected void lnkDownloadFile_Click(object sender, EventArgs e)
        {
            DownloadFile(txtFilepath.Text);
        }

        protected void lnkWoDetails_Click(object sender, EventArgs e)
        {
            try
            {
                string sReferId = string.Empty;
                sReferId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtWorkOrderID.Text));

                string sType = txtFailtype.Text + "~" + txtGuranteetype.Text;
                string sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sType));
                string sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt("V"));

                string url = "/TCRepair/RepairerWorkOrder.aspx?TypeValue=" + sTaskType + "&ReferID=" + sReferId + "&ActionType=" + sActionType;
                string s = "window.open('" + url + "', '_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}