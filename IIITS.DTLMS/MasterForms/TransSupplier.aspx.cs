using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;



namespace IIITS.DTLMS.MasterForms
{
    public partial class TransSupplier : System.Web.UI.Page
    {

        string strFormCode = "TransSupplier.aspx";
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
                UpdateSupplier.Visible = false;
                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;
                txtfromdateExtender.StartDate = System.DateTime.Now.AddDays(0);
                txtDateUpto.Attributes.Add("readonly", "readonly");                

                if (!IsPostBack)
                {

                    if (Request.QueryString["StrQryId"] != null && Request.QueryString["StrQryId"].ToString() != "")
                    {
                        txtSuppId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["StrQryId"]));
                    }

                    cmbIsBlack.SelectedIndex = 2;
                    txtDateUpto.Enabled = false;
                    txtDateUpto.Text = string.Empty;
                    if (txtSuppId.Text != "")
                    {
                        GetSupplierDetails(txtSuppId.Text);

                        cmbIsBlack_SelectedIndexChanged(sender, e);
                        Create.Visible = false;
                        CreateSupplier.Visible = false;

                        Update.Visible = true;
                        UpdateSupplier.Visible = true;


                    }

                    txtDateUpto.Attributes.Add("onblur", "return ValidateDate(" + txtDateUpto.ClientID + ");");

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }


        public void GetSupplierDetails(string strSupplierId)
        {
            try
            {
                DataTable dtBillDetails = new DataTable();
                clsTransSupplier objSupplier = new clsTransSupplier();

                objSupplier.SupplierId = strSupplierId;
                objSupplier.GetSupplierDetails(objSupplier);
                txtSuppId.Text = objSupplier.SupplierId;
                txtSupplierName.Text = objSupplier.SupplierName.Replace("'", "");
                txtSupplierAddress.Text = objSupplier.RegisterAddress.Replace("'", "");
                txtSupplierPhnNo.Text = objSupplier.SupplierPhoneNo;
                txtSupplierEmailId.Text = objSupplier.SupplierEmail;
                txtCommAddress.Text = objSupplier.CommAddress.Replace("'", "");

                cmbIsBlack.SelectedValue = objSupplier.SupplierBlacklisted;
                txtDateUpto.Text = objSupplier.SupplierBlackedupto;

                txtContactPerson.Text = objSupplier.sContactPerson.Replace("'", "");
                txtFaxNo.Text = objSupplier.sFax;
                txtMobileNo.Text = objSupplier.sMobileNo;
                cmdSave.Text = "Update";
                txtSupplierName.Enabled = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }


        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                clsTransSupplier objSupplier = new clsTransSupplier();
                string[] Arr = new string[2];

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

                    objSupplier.SupplierId = txtSuppId.Text;
                    objSupplier.SupplierName = txtSupplierName.Text.ToUpper().Replace("'", "");
                    objSupplier.RegisterAddress = txtSupplierAddress.Text.Replace("'", "''");
                    objSupplier.SupplierPhoneNo = txtSupplierPhnNo.Text.Replace("'", "");
                    objSupplier.SupplierEmail = txtSupplierEmailId.Text.Replace("'", "").ToLower();
                    if (cmbIsBlack.SelectedIndex > 0)
                    {
                        objSupplier.SupplierBlacklisted = cmbIsBlack.SelectedValue;
                    }

                    objSupplier.SupplierBlackedupto = txtDateUpto.Text.Replace("'", "");
                    objSupplier.sCrby = objSession.UserId;
                    objSupplier.CommAddress = txtCommAddress.Text.Replace("'", "''");
                    objSupplier.sContactPerson = txtContactPerson.Text.Trim();
                    objSupplier.sFax = txtFaxNo.Text.Trim();
                    objSupplier.sMobileNo = txtMobileNo.Text.Trim();

                    Arr = objSupplier.SaveDetails(objSupplier);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "Supplier Master");
                    }

                    if (Arr[1].ToString() == "0")
                    {
                        txtSuppId.Text = objSupplier.SupplierId;
                        cmdSave.Text = "Update";
                        ShowMsgBox(Arr[0]);
                        Reset();
                    }

                    if (Arr[1].ToString() == "1")
                    {
                        ShowMsgBox(Arr[0]);
                        Reset();
                    }

                    if (Arr[1].ToString() == "4")
                    {
                        ShowMsgBox(Arr[0]);
                        Reset();
                    }

                }
                return;
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
                txtSuppId.Text = string.Empty;
                txtSupplierName.Text = string.Empty;
                txtSupplierPhnNo.Text = string.Empty;
                txtSupplierEmailId.Text = string.Empty;
                txtSupplierAddress.Text = string.Empty;
                txtDateUpto.Text = string.Empty;
                cmbIsBlack.SelectedIndex = 2;
                txtDateUpto.Enabled = false;
                cmdSave.Text = "Save";
                txtCommAddress.Text = string.Empty;
                txtContactPerson.Text = string.Empty;
                txtFaxNo.Text = string.Empty;
                txtMobileNo.Text = string.Empty;
                txtSupplierName.Enabled = true;

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
                // Response.Redirect("TransSupplier.aspx", false);

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


        bool ValidateForm()
        {


            bool bValidate = false;

            if (txtSupplierName.Text == "")
            {
                txtSupplierName.Focus();
                ShowMsgBox("Please Enter the Name of Supplier");
                return bValidate;
            }

            if (txtSupplierName.Text.Trim().StartsWith("."))
            {
                txtSupplierName.Focus();
                ShowMsgBox("Please Enter the  Valid Name of Supplier");
                return bValidate;
            }

            

            if (txtSupplierPhnNo.Text == "")
            {
                txtSupplierPhnNo.Focus();
                ShowMsgBox("Please Enter Valid Phone No");
                return bValidate;
            }
            if (txtSupplierPhnNo.Text.Contains(".") == true)
            {
                txtSupplierPhnNo.Focus();
                ShowMsgBox("You cannot enter (.) in Phone Number");
                return bValidate;
            }
            if (txtSupplierPhnNo.Text != "")
            {
                if ((txtSupplierPhnNo.Text.Length - txtSupplierPhnNo.Text.Replace("-", "").Length) >= 2)
                {
                    txtSupplierPhnNo.Focus();
                    ShowMsgBox("You cannot use more than one hyphen (-)");
                    return bValidate;
                }

                if (txtSupplierPhnNo.Text.Contains(".") == true)
                {
                    txtSupplierPhnNo.Focus();
                    ShowMsgBox("You cannot enter (.) in Phone Number");
                    return bValidate;
                }

            }
            if (txtMobileNo.Text == "")
            {
                txtMobileNo.Focus();
                ShowMsgBox("Please Enter Valid Mobile No");
                return bValidate;
            }

            if (txtSupplierPhnNo.Text.Length < 10)
            {
                ShowMsgBox("Enter valid 11 digit Phone no");
                txtSupplierPhnNo.Focus();
                return bValidate;
            }


            if (txtSupplierEmailId.Text == "")
            {
                txtSupplierEmailId.Focus();
                ShowMsgBox("Please Enter the EmailId");
                return bValidate;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtSupplierEmailId.Text, "^\\s*[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+[a-zA-Z0-9]\\s*$"))
            {
                ShowMsgBox("Please enter Valid Email (xyz@aaa.com)");
                return bValidate;
            }

            if (txtMobileNo.Text != "")
            {
                if (txtMobileNo.Text.Length < 10)
                {
                    ShowMsgBox("Enter valid  Mobile No");
                    txtMobileNo.Focus();
                    return bValidate;
                }
            }
            if (txtMobileNo.Text[0] == '0' || txtMobileNo.Text[0] == '1' || txtMobileNo.Text[0] == '2' )
            {
                ShowMsgBox("Please Enter Valid Mobile No");
                txtMobileNo.Focus();
                return false;
            }

            //if (cmbIsBlack.SelectedIndex == 0)
            //{
            //    ShowMsgBox("Please Select the Blacklisted condition");
            //    cmbIsBlack.Focus();
            //    return bValidate;
            //}
            if (cmbIsBlack.SelectedIndex == 0)
            {
                ShowMsgBox("Please select the BlockList");
                cmbIsBlack.Focus();
            }
            else
            {
                if (cmbIsBlack.SelectedValue == "1")
                {
                    //blocklist.Visible = true;
                    if (txtDateUpto.Text == "")
                    {
                        ShowMsgBox("Please select the BlockListed Upto Date");
                        txtDateUpto.Focus();

                        return bValidate;
                    }
                    if (txtDateUpto.Text != "")
                    {
                        string sRet = Genaral.DateValidation(txtDateUpto.Text);
                        if (sRet != "")
                        {
                            ShowMsgBox(sRet);
                            return bValidate;
                        }
                    }
                }
            }
            

            if (txtSupplierAddress.Text == "")
            {
                txtSupplierAddress.Focus();
                ShowMsgBox("Please Enter Valid Register Address");
                return bValidate;
            }
            if (txtSupplierAddress.Text.Trim().Length == 0)
            {
                txtSupplierAddress.Focus();
                ShowMsgBox("Please Enter Valid Register Address");
                return bValidate;
            }
            //if (txtSupplierAddress.Text != "")
            //{
            //    if (!System.Text.RegularExpressions.Regex.IsMatch(txtSupplierAddress.Text, "^[0-9 a-z A-Z ]+$"))
            //    {

            //        txtSupplierAddress.Focus();
            //        ShowMsgBox("Please Enter Valid Address ");
            //        return bValidate;
            //    }
            //}

            if (txtFaxNo.Text.Trim() != "")
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtFaxNo.Text, "^[0-9 \\-\\s \\( \\)]*$"))
                {
                    txtFaxNo.Focus();
                    ShowMsgBox("Please Enter Valid Fax No (Eg: 865-934-1234)");
                    return bValidate;
                }
            }


            bValidate = true;
            return bValidate;
        }

        protected void cmbIsBlack_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                if (cmbIsBlack.SelectedIndex == 1)
                {
                    txtDateUpto.Enabled = true;
                    blocklist.Visible = true;

                }
                else
                {
                    blocklist.Visible = false;
                    txtDateUpto.Enabled = false;
                    txtDateUpto.Text = string.Empty;
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

                objApproval.sFormName = "TransSupplier";
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


    }
}
