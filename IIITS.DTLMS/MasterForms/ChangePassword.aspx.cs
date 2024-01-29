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


namespace IIITS.DTLMS
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        string strFormCode = "ChangePassword";
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
                    Form.DefaultButton = btnsubmit.UniqueID;
                    objSession = (clsSession)Session["clsSession"];
                    lblMessage.Text = string.Empty;
                }

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        public static class ValidatePwd
        {
            public static bool IsValidPwd(string pwd)
            {
                var r = new System.Text.RegularExpressions.Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,12}$");
                //var r = new System.Text.RegularExpressions.Regex(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,12}$");

                return !string.IsNullOrEmpty(pwd) && r.IsMatch(pwd);
            }
        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string[] Arr = new string[3];
                clsChangePwd objChangepwd = new clsChangePwd();
                clsLogin objLogin = new clsLogin();

                objChangepwd.strOldPwd = txtOldPwd.Text.Replace("'", "");
                objChangepwd.strNewPwd = txtNewPwd.Text.Replace("'", "").Trim();
                objChangepwd.strConfirmPwd = txtConfirmPwd.Text.Replace("'", "").Trim();

                objChangepwd.struserId = objSession.UserId;


                string NewPwd = objChangepwd.strNewPwd;
                //ChangePassword.ValidatePwd a = new ChangePassword.ValidatePwd();
                bool PwdValidate = ChangePassword.ValidatePwd.IsValidPwd(NewPwd);
                if (PwdValidate == true)
                {

                    if (ValidateForm() == true)
                    {
                        //if (objSession.sPassordAcceptance == "1")
                        //{
                        //    bool res = objLogin.GetStatus(Genaral.Encrypt(txtConfirmPwd.Text), objSession.UserId);
                        //    if (res == false)
                        //    {
                        //        ShowMsgBox("you had Already used This Password, Please Use Another Password");
                        //        return;
                        //    }
                        //}

                        if (txtNewPwd.Text == txtConfirmPwd.Text)
                        {

                            Arr = objChangepwd.ChangePwd(objChangepwd);

                            if (Arr[1].ToString() == "1")
                            {
                                Session["clsSession"]= null;
                                Genaral.PasswordChangeLog(objSession.sClientIP, objSession.UserId);
                                string message = "Password Changed Succesfully.Login with New Password";
                                string url = "../Login.aspx";
                                string script = "window.onload = function(){ alert('";
                                script += message;
                                script += "');";
                                script += "window.location = '";
                                script += url;
                                script += "'; }";
                                ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);

                                //ShowMsgBox("Password Change Successfully");
                                //ClientScript.RegisterStartupScript( this.GetType(), "alert", "alert('Password Changed Succesfully.Login with New Password')", true);
                                //Response.Redirect("~/Login.aspx", true);
                                return;
                                //Response.Redirect("~/Login.aspx", false);



                                

                            }
                            if (Arr[1].ToString() == "0")
                            {

                                ShowMsgBox(Arr[0]);

                                // Response.Redirect("~/Login.aspx", false);
                                // return;
                                //  Response.Redirect("~/Login.aspx", false);

                            }
                        }

                    }
                }
                else
                {
                    ShowMsgBox("Password should be greater than or equal to 8 character (Must Contain at least 1 Capital Letter,1 Digit,1 Small Letter, 1 Special Character)");
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

            if (txtOldPwd.Text.Trim().Length == 0)
            {
                txtOldPwd.Focus();
                ShowMsgBox("Enter Old Password");
                return false;
            }


            if (txtNewPwd.Text.Trim().Length == 0)
            {
                txtNewPwd.Focus();
                ShowMsgBox("Enter New Password");
                return false;
            }
            if (txtConfirmPwd.Text.Trim().Length == 0)
            {
                txtConfirmPwd.Focus();
                ShowMsgBox("Enter Confirm Password");
                return false;
            }
            if (txtOldPwd.Text == txtNewPwd.Text)
            {

                ShowMsgBox("New Password and Old password should not be same");
                txtConfirmPwd.Focus();
                return false;
            }
            if (txtNewPwd.Text != txtConfirmPwd.Text)
            {
                ShowMsgBox("New Password and Confirm password should  be same");
                txtConfirmPwd.Focus();
                return false;
            }


            bValidate = true;
            return bValidate;
        }

    }
}