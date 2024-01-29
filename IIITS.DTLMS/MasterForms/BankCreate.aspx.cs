using IIITS.DTLMS.BL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.MasterForms
{
    public partial class BankCreate : System.Web.UI.Page
    {
        string strFormCode = "BankCreate";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                Form.DefaultButton = cmdSave.UniqueID;
                Update.Visible = false;
                txtOfficeCode.Attributes.Add("readonly", "readonly");
                UpdateStore.Visible = false;
                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;
                // DivmdlPoppup.Style.Add("display", "none");
                if (!IsPostBack)
                {
                    if (Request.QueryString["QryBankId"] != null && Request.QueryString["QryBankId"].ToString() != "")
                    {
                        txtBankId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryBankId"]));
                    }

                    Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_CODE\" || '-' || \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" ORDER BY \"SD_SUBDIV_CODE\"", "--Select--", cmbsubDivision);
                    Genaral.Load_Combo("SELECT \"SM_ID\",\"SM_NAME\" FROM \"TBLSTOREMAST\" ORDER BY \"SM_ID\"", "--Select--", cmbStore);

                    if (txtBankId.Text != "")
                    {
                        CheckAccessRights("4");
                        LoadBankDetails(txtBankId.Text);
                        Create.Visible = false;
                        CreateStore.Visible = false;

                        Update.Visible = true;
                        UpdateStore.Visible = true;
                    }


                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        
        }

        public void LoadBankDetails(string strId)
        {
            ArrayList arrOffCode = new ArrayList();
            ArrayList arrOffCodeValue = new ArrayList();

            try
            {
                clsBank objBank = new clsBank();
                objBank.sSlNo = strId;
                objBank.GetBankDetails(objBank);
                txtBankId.Text = objBank.sSlNo;
                //txtStoreCode.Text = objStore.sStoreCode;
                txtBankName.Text = objBank.sBankName;
                //txtBankName.ReadOnly = true;
                txtBankDescription.Text = objBank.sBankDescription;
                //cmbDivision.SelectedValue = objStore.sOfficeCode;
                txtEmailId.Text = objBank.sEmailId;
                txtMobile.Text = objBank.sMobile;
                //txtPhone.Text = objStore.sPhoneNo;
                txtInchargeName.Text = objBank.sBankIncharge;
                txtAddress.Text = objBank.sAddress;
                txtOfficeCode.Text = objBank.sOfficeCode;
                txtStoreCode.Enabled = false;
                cmbStore.SelectedIndex = Convert.ToInt32(objBank.sStoreId);

                if (txtOfficeCode.Text.StartsWith(";"))
                {
                    txtOfficeCode.Text = txtOfficeCode.Text.Substring(1, txtOfficeCode.Text.Length - 1);
                }
                if (txtOfficeCode.Text.EndsWith(";"))
                {
                    txtOfficeCode.Text = txtOfficeCode.Text.Substring(0, txtOfficeCode.Text.Length - 1);
                }
                txtOfficeCode.Text = txtOfficeCode.Text.Replace(";", ",");

                arrOffCode.AddRange(txtOfficeCode.Text.Split(','));


                for (int i = 0; i < arrOffCode.Count; i++)
                {
                    arrOffCodeValue.Add(Convert.ToInt32(arrOffCode[i]));
                }

                ViewState["CHECKED_ITEMS"] = arrOffCodeValue;

                LoadOffice();
                PopulateCheckedValues();

                cmdSave.Text = "Update";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }


        }
        private void PopulateCheckedValues()
        {
            try
            {
                ArrayList userdetails = (ArrayList)ViewState["CHECKED_ITEMS"];

                if (userdetails != null && userdetails.Count > 0)
                {
                    foreach (GridViewRow gvrow in GrdOffices.Rows)
                    {
                        int index = Convert.ToInt32(GrdOffices.DataKeys[gvrow.RowIndex].Value);
                        if (userdetails.Contains(index))
                        {
                            CheckBox myCheckBox = (CheckBox)gvrow.FindControl("cbSelect");
                            myCheckBox.Checked = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void btnOK_Click1(object sender, EventArgs e)
        {
            try
            {
                ArrayList arrChecked = new ArrayList();

                GrdOffices.AllowPaging = false;
                SaveCheckedValues();
                LoadOffice(objSession.OfficeCode);
                PopulateCheckedValues();

                foreach (GridViewRow Row in GrdOffices.Rows)
                {
                    bool result = ((CheckBox)Row.FindControl("cbSelect")).Checked;

                    if (result == true)
                    {
                        arrChecked.Add(((Label)Row.FindControl("lblOffCode")).Text);
                    }
                }

                GrdOffices.AllowPaging = true;
                SaveCheckedValues();
                LoadOffice(objSession.OfficeCode);
                PopulateCheckedValues();


                string sOfficeCode = string.Empty;

                for (int i = 0; i < arrChecked.Count; i++)
                {
                    sOfficeCode += arrChecked[i];
                    if (sOfficeCode.StartsWith(",") == false)
                    {
                        //sOfficeCode =  sOfficeCode;
                    }
                    if (sOfficeCode.EndsWith(",") == false)
                    {
                        sOfficeCode = sOfficeCode + ",";
                    }
                }

                //txtOfficeCode.Text = strOk;
                if (sOfficeCode.EndsWith(",") == true)
                {
                    sOfficeCode = sOfficeCode.Remove(sOfficeCode.Length - 1);
                }

                txtOfficeCode.Text = sOfficeCode;
                txtOfficeCode.Enabled = false;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }
        protected void GrdOffices_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                SaveCheckedValues();
                GrdOffices.PageIndex = 0;
                GrdOffices.PageIndex = e.NewPageIndex;
                LoadOffice(objSession.OfficeCode);
                PopulateCheckedValues();
                this.mdlPopup.Show();
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void GrdOffices_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                foreach (GridViewRow Row in GrdOffices.Rows)
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)//except header and footer
                    {
                        TextBox txtOff = new TextBox();
                        CheckBox cbSelect = new CheckBox();
                        ArrayList arroffcode = new ArrayList();

                        cbSelect = (CheckBox)e.Row.FindControl("cbSelect");
                        Label lblOff = new Label();

                        lblOff = (Label)Row.FindControl("lblOffCode");
                    }
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.mdlPopup.Show();
                //  DivmdlPoppup.Style.Add("display","block");
                //userdetails.Clear();     

                if (ViewState["CHECKED_ITEMS"] != null)
                {
                    SaveCheckedValues();
                    LoadOffice(objSession.OfficeCode);
                    PopulateCheckedValues();
                }
                else
                {
                    LoadOffice(objSession.OfficeCode);
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            clsBank ObjBank = new clsBank();
            try
            {

                //Check AccessRights
                bool bAccResult;
                if (cmdSave.Text == "UPDATE")
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
                    string[] Arr = new string[2];
                    ObjBank.sSlNo = txtBankId.Text;
                    ObjBank.sBankCode = txtStoreCode.Text;
                    ObjBank.sBankName = txtBankName.Text;
                    ObjBank.sBankDescription = txtBankDescription.Text;
                    ObjBank.sOfficeCode = txtOfficeCode.Text;
                    ObjBank.sCrby = objSession.UserId;
                    ObjBank.sEmailId = txtEmailId.Text;
                    ObjBank.sStoreId = cmbStore.Text;
                    ObjBank.sMobile = txtMobile.Text;
                   // ObjStore.sPhoneNo = txtPhone.Text;
                    ObjBank.sBankIncharge = txtInchargeName.Text;
                    ObjBank.sAddress = txtAddress.Text;
                   // ObjBank.sStatus = "A";
                    Arr = ObjBank.SaveUpdateBankDetails(ObjBank);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "Bank Master");
                    }

                    if (Arr[1].ToString() == "0")
                    {

                        ShowMsgBox("Saved Successfully");
                        txtBankId.Text = Arr[2].ToString();
                        cmdSave.Text = "Update";

                        return;
                    }
                    if (Arr[1].ToString() == "1")
                    {
                        ShowMsgBox(Arr[0]);
                        cmdSave.Text = "Update";
                        return;
                    }
                    if (Arr[1].ToString() == "4")
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
        bool ValidateForm()
        {
            bool bValidate = false;

            try
            {

                if (txtBankName.Text.Trim().Length == 0)
                {
                    txtBankName.Focus();
                    ShowMsgBox("Enter valid Bank Name");
                    return bValidate;
                }

                if (txtBankName.Text.Trim().StartsWith("."))
                {
                    txtBankName.Focus();
                    ShowMsgBox("Enter valid Bank Name");
                    return bValidate;
                }



                if (txtOfficeCode.Text.Trim().Length == 0)
                {
                    txtOfficeCode.Focus();
                    ShowMsgBox("Please Select Sub Division");
                    return bValidate;
                }


                if (txtAddress.Text.Trim().Length == 0)
                {
                    txtAddress.Focus();
                    ShowMsgBox("Enter address");
                    return bValidate;
                }

                if (txtBankDescription.Text.Trim().Length == 0)
                {
                    txtBankDescription.Focus();
                    ShowMsgBox("Enter valid Bank Description");
                    return bValidate;
                }


                if (cmbStore.Text == "--Select--")
                {
                    ShowMsgBox("select store name");
                    return bValidate;
                }

                if (txtInchargeName.Text.Trim().Length == 0)
                {
                    txtInchargeName.Focus();
                    ShowMsgBox("Enter Bnak Incharge name");
                    return bValidate;
                }
                if (txtInchargeName.Text.Trim().StartsWith("."))
                {
                    txtInchargeName.Focus();
                    ShowMsgBox("Enter Bank Incharge name");
                    return bValidate;
                }

                if (txtEmailId.Text.Trim().Length == 0)
                {
                    txtEmailId.Focus();
                    ShowMsgBox("Enter valid Email Id");
                    return bValidate;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmailId.Text, "^\\s*[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+[a-zA-Z0-9]\\s*$"))
                {
                    ShowMsgBox("Please enter Valid Email (xyz@aaa.com)");
                    return bValidate;
                }


                if (txtMobile.Text.Trim().Length == 0)
                {
                    txtMobile.Focus();
                    ShowMsgBox("Enter Valid Mobile number");
                    return bValidate;
                }
                if (txtMobile.Text.Trim().Length < 10)
                {
                    txtMobile.Focus();
                    ShowMsgBox("Enter Valid 10 digit Mobile number");
                    return bValidate;
                }
                int len = txtAddress.Text.Trim().Length;
                if (txtAddress.Text.Trim().Length > 200)
                {
                    txtAddress.Focus();
                    ShowMsgBox("Please Enter a maximum of 200 characters for Address");
                    return bValidate;
                }
                int leng = txtBankDescription.Text.Trim().Length;
                if (txtBankDescription.Text.Trim().Length > 200)
                {
                    txtBankDescription.Focus();
                    ShowMsgBox("Please Enter a maximum of 200 characters for Description");
                    return bValidate;
                }

                if (txtMobile.Text[0] == '0' || txtMobile.Text[0] == '1' || txtMobile.Text[0] == '2')
                {
                    ShowMsgBox("Please Enter Valid Mobile No");
                    txtMobile.Focus();
                    return false;
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


        private void SaveCheckedValues()
        {
            try
            {
                ArrayList userdetails = new ArrayList();
                ArrayList tmpArrayList = new ArrayList();

                int index = -1;
                string strIndex = string.Empty;
                string strOk1 = string.Empty;
                foreach (GridViewRow gvrow in GrdOffices.Rows)
                {
                    index = Convert.ToInt32(GrdOffices.DataKeys[gvrow.RowIndex].Value);
                    CheckBox result = ((CheckBox)gvrow.FindControl("cbSelect"));
                    // Check in the Session
                    if ((ArrayList)ViewState["CHECKED_ITEMS"] != null)
                        userdetails = (ArrayList)ViewState["CHECKED_ITEMS"];


                    Label lblOff = (Label)gvrow.FindControl("lblOffCode");

                    if (result.Checked == true)
                    {
                        if (!userdetails.Contains(index))
                        {
                            userdetails.Add(index);
                        }
                    }

                    else
                    {
                        if (userdetails.Contains(index))
                        {
                            userdetails.Remove(index);
                        }
                    }
                }
                if (userdetails != null && userdetails.Count > 0)
                    ViewState["CHECKED_ITEMS"] = userdetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }
        protected void GrdOffices_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtOffCode = (TextBox)row.FindControl("txtOffCode");
                    TextBox txtOffName = (TextBox)row.FindControl("txtOffName");
                    LoadOffice(txtOffCode.Text.Trim().Replace("'", "''"), txtOffName.Text.Trim().Replace("'", "''"));
                    this.mdlPopup.Show();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }        
        public void LoadOffice(string sOfficeCode = "", string sOffName = "")
        {
            try
            {
                DataTable dtPageDetaiils = new DataTable();
                clsBank ObjBank = new clsBank();
                ObjBank.sOfficeCode = sOfficeCode;
                ObjBank.sOfficeName = sOffName;
                dtPageDetaiils = ObjBank.LoadOfficeDet(ObjBank);
                //if (dtPageDetaiils.Rows.Count > 0)
                //{
                GrdOffices.DataSource = dtPageDetaiils;
                GrdOffices.DataBind();
            }

            catch (Exception ex)
            {
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
                txtBankId.Text = string.Empty;
                cmbsubDivision.SelectedIndex = 0;
                txtStoreCode.Text = string.Empty;
                txtBankName.Text = string.Empty;
                txtBankDescription.Text = string.Empty;
                txtOfficeCode.Text = string.Empty;
                txtEmailId.Text = string.Empty;
                txtMobile.Text = string.Empty;
                //txtPhone.Text = string.Empty;
                txtInchargeName.Text = string.Empty;
                txtAddress.Text = string.Empty;
                cmdSave.Text = "Save";
                cmbStore.SelectedIndex =0;

                txtStoreCode.Enabled = true;
                this.mdlPopup.Hide();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "BankCreate";
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
        private void ShowMsgBox(string sMsg)
        {
            try
            {
                string sShowMsg = string.Empty;
                sShowMsg = "<script language=javascript> alert ('" + sMsg + "')</script>";
                this.Page.RegisterStartupScript("Msg", sShowMsg);
                this.mdlPopup.Hide();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}