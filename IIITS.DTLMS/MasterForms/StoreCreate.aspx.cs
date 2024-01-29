using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Collections;

namespace IIITS.DTLMS.MasterForms
{
    public partial class StoreCreate : System.Web.UI.Page
    {
        string strFormCode = "StoreCreate";
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
                CheckAccessRights("4");
                if (!IsPostBack)
                {                    
                    if (Request.QueryString["QryStoreId"] != null && Request.QueryString["QryStoreId"].ToString() != "")
                    {
                        txtStoreId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryStoreId"]));
                    }

                    Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_CODE\" || '-' || \"DIV_NAME\" FROM \"TBLDIVISION\" ORDER BY \"DIV_CODE\"", "-Select-", cmbDivision);

                    if (txtStoreId.Text != "")
                    {
                        
                        LoadStoreDetails(txtStoreId.Text);
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

        bool ValidateForm()
        {
            bool bValidate = false;

            try
            {

                if (txtStoreName.Text.Trim().Length == 0)
                {
                    txtStoreName.Focus();
                    ShowMsgBox("Enter valid Store Name");
                    return bValidate;
                }

                if (txtStoreName.Text.Trim().StartsWith("."))
                {
                    txtStoreName.Focus();
                    ShowMsgBox("Enter valid Store Name");
                    return bValidate;
                }

                if (txtStoreDescription.Text.Trim().Length == 0)
                {
                    txtStoreDescription.Focus();
                    ShowMsgBox("Enter valid Store Description");
                    return bValidate;
                }


                if (txtInchargeName.Text.Trim().Length == 0)
                {
                    txtInchargeName.Focus();
                    ShowMsgBox("Enter Store Incharge name");
                    return bValidate;
                }
                if (txtInchargeName.Text.Trim().StartsWith("."))
                {
                    txtInchargeName.Focus();
                    ShowMsgBox("Enter Store Incharge name");
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

                if (txtAddress.Text.Trim().Length == 0)
                {
                    txtAddress.Focus();
                    ShowMsgBox("Enter address");
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
                if (txtMobile.Text[0] == '0' || txtMobile.Text[0] == '1' || txtMobile.Text[0] == '2' )
                {
                    ShowMsgBox("Please Enter Valid Mobile No");
                    txtMobile.Focus();
                    return false;
                }
                if (txtOfficeCode.Text.Trim().Length == 0)
                {
                    txtOfficeCode.Focus();
                    ShowMsgBox("Enter select Division");
                    return bValidate;
                }

                //if (objSession.OfficeCode.Length > 2)
                //{
                //    objSession.OfficeCode = objSession.OfficeCode.Substring(0, 2);
                //}

                //if (cmbDivision.SelectedValue != objSession.OfficeCode)
                //{
                //    txtMobile.Focus();
                //    ShowMsgBox("Enter Valid 10 digit Mobile number");
                //    return bValidate;
                //}

                if (txtPhone.Text != "")
                {

                    if (txtPhone.Text.Length < 10)
                    {
                        ShowMsgBox("Enter valid  Phone no");
                        txtPhone.Focus();
                        return bValidate;
                    }
                    if ((txtPhone.Text.Length - txtPhone.Text.Replace("-", "").Length) >= 2)
                    {
                        txtPhone.Focus();
                        ShowMsgBox("You cannot use more than one hyphen (-)");
                        return bValidate;
                    }

                    if (txtPhone.Text.Contains(".") == true)
                    {
                        txtPhone.Focus();
                        ShowMsgBox("You cannot enter (.) in Phone Number");
                        return bValidate;
                    }

                    if (txtPhone.Text.Replace("0", "").Length == 0)
                    {
                        txtPhone.Focus();

                        ShowMsgBox("Please Enter Valid Phone Number");

                        return bValidate;
                    }
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
            clsStore ObjStore = new clsStore();
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
                    ObjStore.sSlNo = txtStoreId.Text;
                    ObjStore.sStoreCode = txtStoreCode.Text;
                    ObjStore.sStoreName = txtStoreName.Text;
                    ObjStore.sStoreDescription = txtStoreDescription.Text;
                    ObjStore.sOfficeCode = txtOfficeCode.Text;
                    ObjStore.sCrby = objSession.UserId;

                    ObjStore.sEmailId = txtEmailId.Text;

                    ObjStore.sMobile = txtMobile.Text;
                    ObjStore.sPhoneNo = txtPhone.Text;
                    ObjStore.sStoreIncharge = txtInchargeName.Text;
                    ObjStore.sAddress = txtAddress.Text;

                    Arr = ObjStore.SaveUpdateStoreDetails(ObjStore);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "Store Master");
                    }

                    if (Arr[1].ToString() == "0")
                    {

                        ShowMsgBox("Saved Successfully");
                        txtStoreId.Text = Arr[2].ToString();
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
                txtStoreId.Text = string.Empty;
                cmbDivision.SelectedIndex = 0;
                txtStoreCode.Text = string.Empty;
                txtStoreName.Text = string.Empty;
                txtStoreDescription.Text = string.Empty;
                txtOfficeCode.Text = string.Empty;
                txtEmailId.Text = string.Empty;
                txtMobile.Text = string.Empty;
                txtPhone.Text = string.Empty;
                txtInchargeName.Text = string.Empty;
                txtAddress.Text = string.Empty;
                cmdSave.Text = "Save";

                txtStoreCode.Enabled = true;
                this.mdlPopup.Hide();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

   
        public void LoadStoreDetails(string strId)
        {
            ArrayList arrOffCode = new ArrayList();
            ArrayList arrOffCodeValue = new ArrayList();

            try
            {
                clsStore objStore = new clsStore();
                objStore.sSlNo = strId;
                objStore.GetStoreDetails(objStore);
                txtStoreId.Text = objStore.sSlNo;
                //txtStoreCode.Text = objStore.sStoreCode;
                txtStoreName.Text = objStore.sStoreName;
                txtStoreDescription.Text = objStore.sStoreDescription;
                //cmbDivision.SelectedValue = objStore.sOfficeCode;
                txtEmailId.Text = objStore.sEmailId;
                txtMobile.Text = objStore.sMobile;
                txtPhone.Text = objStore.sPhoneNo;
                txtInchargeName.Text = objStore.sStoreIncharge;
                txtAddress.Text = objStore.sAddress;
                txtOfficeCode.Text = objStore.sOfficeCode;
                txtStoreCode.Enabled = false;

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

        protected void cmdClose_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("StoreView.aspx", false);
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

                objApproval.sFormName = "StoreCreate";
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


        public void LoadOffice(string sOfficeCode = "", string sOffName = "")
        {
            try
            {
                DataTable dtPageDetaiils = new DataTable();
                clsStore Objstore = new clsStore();
                Objstore.sOfficeCode = sOfficeCode;
                Objstore.sOfficeName = sOffName;
                dtPageDetaiils = Objstore.LoadOfficeDet(Objstore);
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
    }
}