using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IIITS.DTLMS.BL;
using System.Globalization;
using System.Data.OleDb;
using IIITS.PGSQL.DAL;

namespace IIITS.DTLMS.DTCFailure
{
    public partial class PseudoIndent : System.Web.UI.Page
    {
        string strFormCode = "PseudoIndent";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {                    
            try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                else
                {
                    objSession = (clsSession)Session["clsSession"];
                    lblMessage.Text = string.Empty;
                    Form.DefaultButton = cmdSave.UniqueID;
                    txtIndentDate.Attributes.Add("readonly", "readonly");

                    txtIndentDate_CalendarExtender1.EndDate = System.DateTime.Now;

                    if (!IsPostBack)
                    {
                        Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='SR' ORDER BY \"MD_ORDER_BY\" ", "-Select-", cmbRating);
                        Genaral.Load_Combo("SELECT \"SM_ID\",\"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"SM_STATUS\" ='A' ORDER BY \"SM_NAME\" ", "Select", cmbStoreName);

                        if (Request.QueryString["TypeValue"] != null && Convert.ToString(Request.QueryString["TypeValue"]) != "")
                        {
                            txtType.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TypeValue"]));
                            if (Request.QueryString["ReferID"] != null && Convert.ToString(Request.QueryString["ReferID"]) != "")
                            {
                                txtssOfficeCode.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["OfficeCode"]));
                            }
                          
                            ChangeLabelText();
                            GenerateIndentNo();
                            txtIndentDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                        }

                        if (Request.QueryString["ReferID"] != null && Convert.ToString(Request.QueryString["ReferID"]) != "")
                        {
                            txtWOSlno.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ReferID"]));
                            txtssOfficeCode.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["OfficeCode"]));

                            if (Request.QueryString["IndentId"] != null && Convert.ToString(Request.QueryString["IndentId"]) != "")
                            {
                                txtIndentId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["IndentId"]));
                            }
                            GetBasicDetails();

                            if (txtIndentId.Text != "0" && txtIndentId.Text != "")
                            {
                                if (!txtIndentId.Text.Contains("-"))
                                {
                                    GetIndentDetails();
                                }

                                cmdSave.Text = "View";

                            }
                            else
                            {
                                txtIndentId.Text = "";
                            }

                            ValidateFormUpdate();

                        }

                        //Search Window Call
                        LoadSearchWindow();

                        //WorkFlow / Approval
                        WorkFlowConfig();
                        ViewState["BOID"] = Convert.ToString(Session["BOID"]);

                    }
                    ApprovalHistoryView.BOID = Convert.ToString(ViewState["BOID"]);
                    ApprovalHistoryView.sRecordId = txtIndentId.Text;
                    //if (objSession.RoleId == "12")
                    //{
                    //    txtActiontype.Text = "M";
                    //}
                    if (txtActiontype.Text == "M")
                    {
                        clsApproval objLevel = new clsApproval();
                        string sLevel = objLevel.sGetApprovalLevel(Convert.ToString(ViewState["BOID"]), objSession.RoleId);
                        if (sLevel != "" && sLevel != null)
                        {
                            if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                            {
                                cmdSave.Text = " Modify and Submit";
                            }
                            else if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                            {
                                cmdSave.Text = " Modify and Approve";
                            }
                        }
                    }
                    else if (txtActiontype.Text == "A")
                    {
                        clsApproval objLevel = new clsApproval();
                        string sLevel = objLevel.sGetApprovalLevel(Convert.ToString(ViewState["BOID"]), objSession.RoleId);
                        if (sLevel != "" && sLevel != null)
                        {
                            if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                            {
                                cmdSave.Text = "Submit";
                            }
                            else if (Convert.ToInt32(sLevel.Split('~').GetValue(1).ToString()) < Convert.ToInt32(sLevel.Split('~').GetValue(0).ToString()))
                            {
                                cmdSave.Text = "Approve";
                            }
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void GetIndentDetails()
        {
            try
            {

                clsIndent objIndent = new clsIndent();
                objIndent.sIndentId = txtIndentId.Text;

                objIndent.GetIndentDetails(objIndent);

                txtIndentNo.Text = objIndent.sIndentNo;
                txtIndentDesc.Text = objIndent.sIndentDescription;
                txtIndentDate.Text = objIndent.sIndentDate;
                cmbStoreName.SelectedValue = objIndent.sStoreName;
                cmbStoreType.SelectedValue = objIndent.sStoreType;
                cmdSave.Text = "Update";
                if(cmbStoreType.SelectedValue != "0")
                {
                    ShowTCQuantity(cmbStoreType.SelectedValue);
                }
               
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }

        }

        public void GetBasicDetails()
        {
            try
            {
                clsWorkOrder objWO = new clsWorkOrder();
                
                objWO.sWOId = txtWOSlno.Text;

                if (txtType.Text != "3")
                {
                    objWO.GetWOBasicDetails(objWO);
                    txtDTCName.Text = objWO.sDTCName;
                    txtTcCode.Text = objWO.sTCCode;
                    txtFailureID.Text = objWO.sFailureId;
                    txtFailureDate.Text = objWO.sFailureDate;
                    txtDTCCode.Text = objWO.sDTCCode;
                    txtDTCId.Text = objWO.sDTCId;
                    txtTCId.Text = objWO.sTCId;
                }
                else
                {

                    objWO.GetWODetailsForNewDTC(objWO);
                    cmbRating.SelectedValue = objWO.sRating;
                    cmbRating.Enabled = false;

                }

                txtWONo.Text = objWO.sCommWoNo;
                txtWODate.Text = objWO.sCommDate;
                txtIssuedBy.Text = objWO.sCrBy;
                txtNewCapacity.Text = objWO.sNewCapacity;

                //ShowTCQuantity();

                txtWONo.Enabled = false;
                cmdSearch.Visible = false;
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

                if (txtIndentNo.Text.Length == 0)
                {
                    txtIndentNo.Focus();
                    ShowMsgBox("Enter Indent No");
                    return bValidate;
                }
                if (txtIndentDate.Text.Length == 0)
                {
                    txtIndentDate.Focus();
                    ShowMsgBox("Enter Indent Date");
                    return bValidate;
                }
                string sResult = Genaral.DateComparision(txtIndentDate.Text, txtWODate.Text, false, false);
                if (sResult == "2")
                {
                    ShowMsgBox("Commisioning Indent Date should be Greater than Work Order Date");
                    return bValidate;
                }
                //if (txtIndentDesc.Text.Length == 0)
                //{
                //    txtIndentDesc.Focus();
                //    ShowMsgBox("Enter Indent Description");
                //    return bValidate;
                //}

                if (cmbStoreName.SelectedIndex == 0)
                {
                    cmbStoreName.Focus();
                    ShowMsgBox("Select Store Name");
                    return bValidate;
                }
                if (cmbStoreType.SelectedIndex == 0)
                {
                    cmbStoreType.Focus();
                    ShowMsgBox("Select Store Type");
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

          protected void cmdSave_Click(object sender, EventArgs e)
          {
             
              try
              {

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

                  if (cmdSave.Text == "View")
                  {
                      GenerateIndentReport();
                      return;
                  }

                  if (ValidateForm() == true)
                  {
                      string[] Arr = new string[2];
                      clsIndent ObjIndent = new clsIndent();

                      ObjIndent.sIndentNo = txtIndentNo.Text;
                      ObjIndent.sIndentDate = txtIndentDate.Text;
                      ObjIndent.sIndentDescription = txtIndentDesc.Text.Replace("'", "");
                      ObjIndent.sStoreName = cmbStoreName.SelectedValue;
                      ObjIndent.sIndentId = txtIndentId.Text;
                      ObjIndent.sWOSlno = txtWOSlno.Text;
                      ObjIndent.sCrBy = objSession.UserId;
                      ObjIndent.sWoNo = txtWONo.Text.Trim();

                      ObjIndent.sDTCCode = txtDTCCode.Text;
                      ObjIndent.sDTCName = txtDTCName.Text;

                      if (hdfAvailQuantity.Value == "0")
                      {
                          ObjIndent.sAlertFlg = "1";
                      }

                      if (txtActiontype.Text == "A" || txtActiontype.Text == "R")
                      {
                            ApproveRejectAction();
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " (Pseudent)Failure ");
                        }
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

          protected void cmdClose_Click(object sender, EventArgs e)
          {

              try
              {
                  if (Request.QueryString["ActionType"] != null && Convert.ToString(Request.QueryString["ActionType"]) != "")
                  {
                      Response.Redirect("/Approval/ApprovalInbox.aspx", false);
                  }
                  else
                  {
                      Response.Redirect("IndentView.aspx", false);
                  }

              }
              catch (Exception ex)
              {
                  lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
             
          }

          public void ValidateFormUpdate()
          {
              try
              {
                  clsIndent objIndent = new clsIndent();
                  if (objIndent.ValidateUpdate(txtIndentId.Text) == true)
                  {

                      //cmdSave.Enabled = false;
                  }
                  else
                  {

                      cmdSave.Enabled = true;
                  }

              }
              catch (Exception ex)
              {
                  lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
          }

          public void ChangeLabelText()
          {
              try
              {
                  if (txtType.Text == "1" || txtType.Text == "4")
                  {
                      lblIDText.Text = "Failure ID";
                      
                  }
                  else if (txtType.Text == "2")
                  {
                      lblIDText.Text = "Enhancement ID";

                  }
                  else
                  {
                      //txtWONo.Enabled = false;
                      //cmdSearch.Enabled = false;
                      dvFailure.Style.Add("display", "none");
                      dvrating.Visible = true;
                      lnkDTCDetails.Visible = false;
                      lnkDTrDetails.Visible = false;
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
                  txtWOSlno.Text = hdfWOslno.Value;
                  GetBasicDetails();
              }
              catch (Exception ex)
              {
                  lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
          }

          public void  ShowTCQuantity(string sStoreType)
          {
              try
              {
                  clsIndent objIndent = new clsIndent();
                  hdfAvailQuantity.Value  = objIndent.GetTransformerCount(objSession.OfficeCode, txtWOSlno.Text, sStoreType, objSession.sRoleType);
                if (sStoreType != "0")
                {
                    lnkQuantityMsg.Visible = true;
                }
                else
                {
                    lnkQuantityMsg.Visible = false;
                }
                if (sStoreType == "1")
                {
                    lnkQuantityMsg.Text = hdfAvailQuantity.Value + " Number of  " + txtNewCapacity.Text + " KVA Capacity Transformers Available in the Store";
                }
                else
                {
                    lnkQuantityMsg.Text = hdfAvailQuantity.Value + " Number of  " + txtNewCapacity.Text + " KVA Capacity Transformers Available in the Bank";
                }
                  spanQuant.Visible = true;
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
                if (objSession.sRoleType == "1")
                {
                    if(objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                    {

                        cmbStoreName.SelectedValue = objTcMaster.GetStoreId(txtssOfficeCode.Text);
                    }
                    else
                    {
                        cmbStoreName.SelectedValue = objTcMaster.GetStoreId(objSession.OfficeCode);
                    }
                 //   cmbStoreName.SelectedValue = objTcMaster.GetStoreId(objSession.OfficeCode);
                }
                else
                {
                    if(objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                    {
                        cmbStoreName.SelectedValue = txtssOfficeCode.Text;
                    }
                    else
                    {
                        cmbStoreName.SelectedValue = objSession.OfficeCode;
                    }
                  //  cmbStoreName.SelectedValue = objSession.OfficeCode;
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

                // y permanentIndent name comes here need to check ?
                //  if (cmbStoreType.SelectedValue == "1")
                //{
                //    objApproval.sFormName = "PermanentIndent";
                //    //objApproval.sFormName = "InvoiceCreation";
                //}
                //else
                //{
                //    objApproval.sFormName = "PermanentIndent";
                //    //objApproval.sFormName = "BankInvoice";
                //}
                objApproval.sFormName = "PseudoIndent";               

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

          public void WorkFlowObjects(clsIndent objIndent)
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


                  objIndent.sFormName = "IndentCreation";
                if(objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                {
                  string OfficeCode = txtssOfficeCode.Text;
                    objIndent.sOfficeCode = OfficeCode;
                }
                else
                {
                    objIndent.sOfficeCode = objSession.OfficeCode;
                }
              //  objIndent.sOfficeCode = objSession.OfficeCode;
                  objIndent.sClientIP = sClientIP;
                  objIndent.sWFOId = hdfWFOId.Value ;

              }
              catch (Exception ex)
              {
                  lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
          }

          public void GenerateIndentNo()
          {
              try
              {
                  clsIndent objIndent = new clsIndent();
                if(objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                {
                    string OfficeCode = txtssOfficeCode.Text;
                    txtIndentNo.Text = objIndent.GenerateIndentNo(objSession.OfficeCode);
                }
                else
                {
                    txtIndentNo.Text = objIndent.GenerateIndentNo(objSession.OfficeCode);
                }
                //  txtIndentNo.Text = objIndent.GenerateIndentNo(objSession.OfficeCode);
                  txtIndentNo.ReadOnly = true;
              }
              catch (Exception ex)
              {
                  lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
          }

          #region Workflow/Approval
          public void SetControlText()
          {
              try
              {
                  txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));
                if(objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                {
                    txtActiontype.Text = "M";
                }
                  if (txtActiontype.Text == "A")
                  {
                      cmdSave.Text = "Approve";
                      pnlApproval.Enabled = false;
                  }
                  if (txtActiontype.Text == "R")
                  {
                      cmdSave.Text = "Reject";
                      pnlApproval.Enabled = false;
                  }
                  if (txtActiontype.Text == "M")
                  {
                    cmdSave.Text = "Modify and Approve";
                    
                }

                  dvComments.Style.Add("display", "block");
                //cmdReset.Enabled = false;


                //if (hdfWFOAutoId.Value   != "0")
                //{
                //    cmdSave.Text = "Save";
                //    dvComments.Style.Add("display", "none");
                //}
                if (objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                {
                    cmdSave.Text = "Approve";
                    pnlApproval.Enabled = true;
                    txtActiontype.Text = "A";
                }
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

                PGSqlConnection objCon = new PGSQL.DAL.PGSqlConnection(Constants.Password);
                string presentstoreid = objCon.get_value("SELECT \"STO_SM_ID\" from \"TBLSTOREOFFCODE\" WHERE \"STO_OFF_CODE\"='" + txtIndentNo.Text.Substring(0, 3) + "'");

                if (presentstoreid != cmbStoreName.SelectedValue)
                {
                    objApproval.sStatus = "1";
                }

                objApproval.sCrby = objSession.UserId;
                  if(objSession.sRoleType =="1")
                  {
                    if(objSession.RoleId== Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                    {
                        string OfficeCode = txtssOfficeCode.Text;
                        objApproval.sOfficeCode = objSession.OfficeCode;

                    }
                    else
                    {
                        objApproval.sOfficeCode = objSession.OfficeCode;

                    }
                  //  objApproval.sOfficeCode = objSession.OfficeCode;
                  }
                  else
                  {
                    if(objSession.RoleId== Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                    {
                        objApproval.sOfficeCode = txtssOfficeCode.Text;
                    }
                    else
                    {
                        objApproval.sOfficeCode = Convert.ToString(ViewState["LOCCODE"]);
                    }
                  //  objApproval.sOfficeCode = Convert.ToString(ViewState["LOCCODE"]);
                  }
                  
                  objApproval.sApproveComments = txtComment.Text.Trim();
                  objApproval.sWFObjectId = hdfWFOId.Value;
                  objApproval.sWFAutoId = hdfWFOAutoId.Value;
                  if(cmbStoreType.SelectedValue == "1")
                  {
                      objApproval.sStoreType = "1";
                  }
                      else if(cmbStoreType.SelectedValue == "2")
                  {
                      objApproval.sStoreType = "2";
                  }

                  if (txtActiontype.Text == "A")
                  {
                      objApproval.sApproveStatus = "1";
                  }
                  if (txtActiontype.Text == "R")
                  {
                      objApproval.sApproveStatus = "3";
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

                  bool bResult = objApproval.ApproveWFRequest(objApproval);
                  if (bResult == true)
                  {

                      if (objApproval.sApproveStatus == "1")
                      {
                          ShowMsgBox("Approved Successfully");
                          cmdSave.Enabled = false;
                          //if (txtType.Text == "1")
                          //{
                          //      string strParam = string.Empty;
                          //      strParam = "id=IndentReport&IndentId=" + txtIndentId.Text;
                          //      RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                             
                          //}
                      }
                      else if (objApproval.sApproveStatus == "3")
                      {
                          ShowMsgBox("Rejected Successfully");
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

          public void WorkFlowConfig()
          {
            string sLocCode = string.Empty;
              try
              { 
                  if (Request.QueryString["ActionType"] != null && Convert.ToString(Request.QueryString["ActionType"]) != "")
                  {
                     
                      if (Session["WFOId"] != null && Convert.ToString(Session["WFOId"]) != "")
                      {
                          hdfWFDataId.Value  = Convert.ToString(Session["WFDataId"]);
                          hdfWFOId.Value  = Convert.ToString(Session["WFOId"]);
                          hdfWFOAutoId.Value = Convert.ToString(Session["WFOAutoId"]);

                          Session["WFDataId"] = null;
                          Session["WFOId"] = null;
                          Session["WFOAutoId"] = null;
                      }


                      SetControlText();
                      ControlEnableDisable();
                      if (txtActiontype.Text == "V")
                      {
                          cmdSave.Text = "View";

                          dvComments.Style.Add("display", "none");
                      }

                    if (objSession.sRoleType == "2")
                    {
                        if(objSession.RoleId == "2")
                        {
                            sLocCode = clsStoreOffice.GetCurrentOfficeCode(hdfWFOAutoId.Value, "2");
                        }
                        else
                        {
                            sLocCode = clsStoreOffice.GetCurrentOfficeCode(hdfWFOId.Value, "2");
                        }
                        
                        if(sLocCode.Length > 0)
                        {
                            sLocCode = sLocCode.Substring(0, Constants.Division);
                        }
                        ViewState["LOCCODE"] = sLocCode;
                    }
                }
              }
              catch (Exception ex)
              {
                  lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
          }

          #endregion

          public void GenerateIndentReport()
          {
              try
              {
                  string strParam = string.Empty;
                  strParam = "id=IndentReport&IndentId=" + txtIndentId.Text;
                  RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
              }
              catch (Exception ex)
              {
                  lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
          }

          protected void lnkDTrDetails_Click(object sender, EventArgs e)
          {
              try
              {
                  string sTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtTCId.Text));

                  string url = "/MasterForms/TcMaster.aspx?TCId=" + sTCId;
                  string s = "window.open('" + url + "', '_blank');";
                  ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
              }
              catch (Exception ex)
              {
                  lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
          }

          protected void lnkDTCDetails_Click(object sender, EventArgs e)
          {
              try
              {
                  string sDTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtDTCId.Text));

                  string url = "/MasterForms/DTCCommision.aspx?QryDtcId=" + sDTCId;
                  string s = "window.open('" + url + "', '_blank');";
                  ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
              }
              catch (Exception ex)
              {
                  lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
          }

          public void ControlEnableDisable()
          {
              try
              {
                  txtWONo.Enabled = false;
                  cmdSearch.Visible = false;
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
                string strQry = string.Empty;
                if (txtType.Text != "3")
                {
                   
                    string sOfficecode = clsStoreOffice.GetOfficeCode(objSession.OfficeCode, "DF_LOC_CODE");

                    strQry = "Title=Search and Select Work Order Details&";
                    strQry += "Query= SELECT \"WO_SLNO\",\"DT_NAME\",\"WO_NO\",\"DF_DTC_CODE\" FROM \"TBLDTCMAST\",\"TBLDTCFAILURE\",\"TBLWORKORDER\" WHERE  \"DT_CODE\" = \"DF_DTC_CODE\" AND \"DF_REPLACE_FLAG\" = 0 ";
                    strQry += " AND \"WO_SLNO\" NOT IN (SELECT \"TI_WO_SLNO\" FROM TBLINDENT) AND \"DF_ID\" = \"WO_DF_ID\" AND  \"DF_STATUS_FLAG\" = " + txtType.Text + "  AND ";
                      
                    if(objSession.sRoleType == "1")
                    {
                        strQry += "  \"DF_LOC_CODE\" LIKE '" + objSession.OfficeCode + "%' AND {0} like %{1}% &";
                    }
                    else
                    {
                        strQry += " " + sOfficecode + " AND {0} like %{1}% &";
                    }
                    

                    strQry += "DBColName=\"WO_NO\"~\"DT_NAME\"~\"DF_DTC_CODE\"&";
                    strQry += "ColDisplayName=WO NO~DTC_NAME~DTC_CODE&";
                }
                else
                {
                    
                    string sOfficecode = clsStoreOffice.GetOfficeCode("objSession.OfficeCode", "WO_OFF_CODE");

                    strQry = "Title=Search and Select Work Order Details&";
                    strQry += "Query= SELECT \"WO_SLNO\",\"WO_NO\",TO_CHAR(\"WO_DATE\",'DD-MON-YYYY') WO_DATE FROM \"TBLWORKORDER\" ";
                    strQry += " WHERE \"WO_REPLACE_FLG\" ='0'  AND \"WO_DF_ID\" IS NULL AND ";

                    if (objSession.sRoleType == "1")
                    {
                        strQry += "  \"WO_OFF_CODE\" LIKE '" + objSession.OfficeCode + "%'";
                    }
                    else
                    {
                        strQry += " " + sOfficecode + " AND {0} like %{1}% &";
                    }
                    
                    strQry += " AND  \"WO_SLNO\" NOT IN (SELECT \"TI_WO_SLNO\" FROM \"TBLINDENT\") AND {0} like %{1}% &";
                    strQry += "DBColName=\"WO_NO\"&";
                    strQry += "ColDisplayName=WO NO&";
                }

                strQry = strQry.Replace("'", @"\'");
                cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + hdfWOslno.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + hdfWOslno.ClientID + ")");

                txtIndentDate.Attributes.Add("onblur", "return ValidateDate(" + txtIndentDate.ClientID + ");");

                GetStoreId();
              }
              catch (Exception ex)
              {
                  lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
          }

          protected void cmdViewWO_Click(object sender, EventArgs e)
          {
              try
              {
                  string sReferId = string.Empty;
                  string sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtType.Text));

                  if (txtType.Text == "3")
                  {
                      sReferId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtWOSlno.Text));
                  }
                  else
                  {
                      sReferId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtFailureID.Text));
                  }

                 // string sFailureId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtFailureID.Text));

                  string url = "/DTCFailure/WorkOrder.aspx?TypeValue=" + sTaskType + "&ReferID=" + sReferId + "&ActionType=" + HttpUtility.UrlEncode(Genaral.UrlEncrypt("V")); ;
                  string s = "window.open('" + url + "', '_blank');";
                  ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
              }
              catch (Exception ex)
              {
                  lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
          }

        protected void lnkQuantityMsg_Click(object sender, EventArgs e)
        {
            try
            {
                string sOfficeCode = String.Empty;

                if (objSession.sRoleType == "1")
                {
                    sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(objSession.OfficeCode)); 
                }
                else
                {
                    sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(Convert.ToString(ViewState["LOCCODE"])));
                }
               
                string WOSLNO = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtWOSlno.Text));
                string url = "/DashboardForm/FailurePendingOverview.aspx?value=InvoiceTCDetails&OfficeCode=" + sOfficeCode + "&WOSLNO=" + WOSLNO;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);

                //clsIndent objIndent = new clsIndent();
                //DataTable dtTcDeatils = objIndent.GetStore_TcDetails(objSession.OfficeCode, txtWOSlno.Text);
                //HyperQuantityMsg.Text = hdfAvailQuantity.Value + " Number of  " + txtNewCapacity.Text + " KVA Capacity Transformers Available in the Store";
                //spanQuant.Visible = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}
