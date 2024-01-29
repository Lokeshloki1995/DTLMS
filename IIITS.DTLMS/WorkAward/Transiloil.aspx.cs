using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL.WorkAward;
using IIITS.DTLMS.BL;
using System.Data;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.Drawing;
using System.IO;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Drawing;
using System.Threading;

namespace IIITS.DTLMS.WorkAward
{
    public partial class Transiloil : System.Web.UI.Page
    {
        public string strFormCode = "Transiloil";
        clsSession objSession;
        string sWoaid = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                //lblMessage.Text = string.Empty;
                objSession = (clsSession)Session["clsSession"];
                txtWOADate.Attributes.Add("readonly", "readonly");
                txtissuedate.Attributes.Add("readonly", "readonly");
                txtissuedateid.EndDate = System.DateTime.Now;
                
               // WOACalender.EndDate = System.DateTime.Now;

                if (!IsPostBack)
                {
                    txtWOADate.Attributes.Add("readonly", "readonly");

                    txtissuedate.Attributes.Add("readonly", "readonly");
                    txtissuedateid.EndDate = System.DateTime.Now;

                    // WOACalender.EndDate = System.DateTime.Now;

                    //if (!(Convert.ToString(Request.QueryString["woaid"]) == null))
                    //{
                    //    sWoaid = Genaral.UrlDecrypt(Convert.ToString(Request.QueryString["woaid"]));


                    //}


                    if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
                    {
                        txtActionType.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));
                    }

                    if (Request.QueryString["WOId"] != null && Request.QueryString["WOId"].ToString() != "")
                    {
                        txtWOId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["WOId"]));

                        if (txtWOId.Text.Contains('-') || txtActionType.Text=="V")
                        {
                            txtWOId.Text =Convert.ToString(Session["dataId"]);
                        }

                        LoadDetails(txtWOId.Text);

                        WorkFlowConfig();

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
                        dvComments.Style.Add("display", "none");
                    }
                }

                }
            catch (Exception ex)
            {
               
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }


        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateForm())
                {
                    return;
                }

                //if (txtActionType.Text == "A" || txtActionType.Text == "R" || txtActionType.Text == "D"|| txtActionType.Text == "M")
                if (txtActionType.Text == "A" || txtActionType.Text == "R" || txtActionType.Text == "D")

                {
                    if (hdfWFDataId.Value != "0")
                    {
                        ApproveRejectAction();
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "(MajorWorkAward)");
                        }
                        return;
                    }
                }

                ClsMajorWorkAward obj = new ClsMajorWorkAward();
                obj.actualoil = txtactualoil.Text.Trim();

                obj.sWOAId= txtWOId.Text;

                obj.soil = txtoil.Text.Trim();
                obj.sbarrel = txtbarrel.Text.Trim();

                obj.oiltype = cmboiltype.SelectedValue;

                obj.issuedate = txtissuedate.Text.Trim();
                obj.matrialfolio = txtmtrialf0lio.Text.Trim();

                obj.sOfficeCode = objSession.OfficeCode;
                obj.sUserId = objSession.UserId;
                obj.dtWODetails = (DataTable)ViewState["WODETAILS"];
                WorkFlowObjects(obj);
                string[] Arr = new string[3];

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

                    Arr = obj.SaveWorkAwardoilDetails(obj);
                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "(MajorWorkAward)");
                    }
                    if (txtActionType.Text == "M")
                    {
                        txtoil.Text = obj.soil; // = txtoil.Text.Trim();
                        txtbarrel.Text = obj.sbarrel; // = txtbarrel.Text.Trim();

                        cmboiltype.SelectedValue = obj.oiltype; // = cmboiltype.SelectedValue;

                        txtissuedate.Text = obj.issuedate; // = txtissuedate.Text.Trim();
                        txtmtrialf0lio.Text = obj.matrialfolio; // = txtmtrialf0lio.Text.Trim();
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


                Arr = obj.SaveWorkAwardoilDetails(obj);
                if (objSession.sTransactionLog == "1")
                {
                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "(Transiloil)");
                }
                if (cmdSave.Text != "View")
                {
                    ShowMsgBox(Arr[0]);
                }
                cmdSave.Enabled = false;

            }
            catch (Exception ex)
            {
               
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void imgBtnEdit_Click(object sender, EventArgs e)
        {

        }

        public void WorkFlowObjects(ClsMajorWorkAward objAwrd)
        {
            try
            {
                if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
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


                    objAwrd.strFormCode = "Transiloil";
                    objAwrd.sOfficeCode = objSession.OfficeCode;
                    objAwrd.sClientIp = sClientIP;
                    objAwrd.sWFO_id = hdfWFOId.Value;

                    objAwrd.sWFAutoId = hdfWFOAutoId.Value;

                    if (hdfWFDataId.Value != "0")
                    {
                        GetDataFromXML(hdfWFDataId.Value);
                    }

                    dvComments.Style.Add("display", "block");

                    if (hdfWFOAutoId.Value != "0")
                    {
                        dvComments.Style.Add("display", "none");
                    }
                    if (txtActionType.Text == "V")
                    {
                        cmdSave.Text = "View";
                        
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
                
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public bool ValidateForm()
        {
            bool Verify = false;
            try
            {
                if (txtbarrel.Text == string.Empty)
                {
                    ShowMsgBox("Please Enter the Num Of barrels");
                    txtbarrel.Focus();
                    return Verify;
                }

                if (txtissuedate.Text == string.Empty)
                {
                    ShowMsgBox("Please Select the issue Date");
                    txtissuedate.Focus();
                    return Verify;
                }

                if (txtoil.Text == string.Empty)
                {
                    ShowMsgBox("Please Enter the oil");
                    txtoil.Focus();
                    return Verify;
                }

                if (cmboiltype.SelectedIndex == 0)
                {
                    ShowMsgBox("Please Select  Oil Type");
                    return Verify;
                }
                if (txtmtrialf0lio.Text == string.Empty)
                {
                    ShowMsgBox("Please Enter Material folio");
                    txtmtrialf0lio.Focus();
                    return Verify;
                }


                string sResult = Genaral.DateComparisionTransaction(txtissuedate.Text, txtWOADate.Text, false, false);
                if (sResult == "1")
                {
                    ShowMsgBox("Oil  Date should be Greater than Work Award Date");
                    return Verify;
                }

                Verify = true;
                return Verify;
            }
            catch (Exception ex)
            {
               
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Verify;
            }
        }

        public void PDF_onclick(object sender, EventArgs e)
        {
            GenerateFormReport();
        }  

        public void GenerateFormReport()
        {
            try
            {
              
                    string strParam = string.Empty;
                strParam = "id=FormNewReport&WOANo=" + txtWOANo.Text + "&WOADate=" + txtWOADate.Text + "&actualoil=" + txtactualoil.Text + "&oil=" + txtoil.Text + "&barrel=" + txtbarrel.Text +"&oiltype=" + cmboiltype.SelectedItem + "&issuedate=" + txtissuedate.Text + "&mtrialf0lio=" + txtmtrialf0lio.Text;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
            }
            catch (Exception ex)
            {
               // lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadDetails(string sWoid)
        {
            try
            {
                ClsMajorWorkAward objmaj = new ClsMajorWorkAward();
                objmaj.sWoSlno = sWoid;


                objmaj.GetawrdDetails(objmaj);

                txtWOANo.Text=objmaj.sWOANo;
                txtWOADate.Text= objmaj.sWOADate;

                txtactualoil.Text =objmaj.actualoil;


              ViewState["BOID"] = "76";
            }
            catch (Exception ex)
            {
              
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
                    }

                    dvComments.Style.Add("display", "block");

                    if (hdfWFOAutoId.Value != "0")
                    {
                        dvComments.Style.Add("display", "none");
                    }
                    if (txtActionType.Text == "V")
                    {
                        cmdSave.Text = "View";
                       
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
               
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetDataFromXML(string sWFDataId)
        {
            try
            {
                ClsMajorWorkAward obj = new ClsMajorWorkAward();
                obj.sWFDataId = sWFDataId;
                obj.GetoilDetailsFromXML(obj);
                //  txtWOANo.Text = obj.sWOANo;
                //txtWOADate.Text = obj.sWOADate;

                txtbarrel.Text = obj.sbarrel;
                txtoil.Text = obj.soil;

                txtissuedate.Text = obj.issuedate;

                cmboiltype.SelectedValue = obj.oiltype.Trim();
                txtmtrialf0lio.Text = obj.matrialfolio;

                if (txtActionType.Text == "A")
                {
                    
                    pnlApproval.Enabled = false;
                }
                //Reject
                if (txtActionType.Text == "R")
                {
                    
                    pnlApproval.Enabled = false;
                }





                //ViewState["WODETAILS"] = obj.dtWODetails;
            }
            catch (Exception ex)
            {
               
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
                    pnlApproval.Enabled = false;
                }
                //Reject
                if (txtActionType.Text == "R")
                {
                    objApproval.sApproveStatus = "3";
                    pnlApproval.Enabled = false;
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
                        txtWOAId.Text = objApproval.sNewRecordId;
                       // GenerateIndentReport();
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
               
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }



        public void GenerateWorkawardReport()
        {
            try
            {
                if (!txtWOId.Text.Contains("-") && txtWOId.Text.Length > 0)
                {
                    string strParam = string.Empty;
                    string sOffcName = string.Empty;
                    /* strParam = "id=WorkAwardReportmajor&WorkAwardId=" + txtWOId.Text;*/
                    

                    strParam = "id=WorkAwardReportmajor&WorkAwardId=" + txtWOId.Text + "&OffCode=" + sOffcName;
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                }
            }
            catch (Exception ex)
            {
                
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdWorkaward_Click(object sender, EventArgs e)
        {
            GenerateWorkawardReport();
        }
    }
}