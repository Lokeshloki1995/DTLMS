using System;
using IIITS.DTLMS.BL;
using System.Web;
using System.Configuration;

namespace IIITS.DTLMS
{
    public partial class ChangePwdInternal : System.Web.UI.Page
    {
        string strFormCode = "ChangePwdInternal";
        clsSession objSession;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                clsLogin objLogin = new clsLogin();
                clsChangePwd objChangepwd = new clsChangePwd();
                Form.DefaultButton = btnsubmit.UniqueID;
                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;
                if (Request.QueryString["pkid"] != null && Convert.ToString(Request.QueryString["pkid"]) != "")
                {

                    hdnpkid.Value = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["pkid"]));
                    if (Convert.ToInt32(hdnpkid.Value) > 0)
                    {
                        objLogin.SubmitClick = Convert.ToInt32(hdnpkid.Value);
                        double MailExpiryTime = Convert.ToDouble(ConfigurationSettings.AppSettings["MAILSESSIONTIME"]);
                        string MailLogDetails = objLogin.FetchMailHistory(objLogin);
                        if (MailLogDetails != "" && MailLogDetails != null)
                        {
                            string LastChangedOn = objLogin.FetchLastChangedPassowrdCrOn(objLogin);
                            string[] MailLogDetailsplit = MailLogDetails.Split('~');
                            DateTime MailLoggedOn = Convert.ToDateTime(MailLogDetailsplit[0]).AddMinutes(MailExpiryTime);
                            
                            objLogin.sFullName = Convert.ToString(MailLogDetailsplit[1]);
                            objChangepwd.struserId = Convert.ToString(MailLogDetailsplit[2]);
                            DateTime dtExpireDate = DateTime.Now;
                            int res = DateTime.Compare(MailLoggedOn, DateTime.Now);
                            int res1 = DateTime.Compare(Convert.ToDateTime(LastChangedOn), MailLoggedOn);
                           
                            if ( MailLoggedOn >= Convert.ToDateTime(LastChangedOn).AddMinutes(30))
                            {
                                string message = "Your password is already changed.Please login using the valid credentials";
                                string url = "../Login.aspx";
                                string script = "window.onload = function(){ alert('";
                                script += message;
                                script += "');";
                                script += "window.location = '";
                                script += url;
                                script += "'; }";
                                ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);

                                return;
                            }
                            else if (res < 0)
                            {
                                lblMessage.Text = "Session Expired";
                                Response.Redirect("error_page.html", false);
                                //return RedirectToAction("MailSessionExpiry");

                            }


                            else
                            {
                                
                                

                            }
                        }

                        else
                        {
                            lblMessage.Text = "Session Expired";
                            Response.Redirect("error_page.html", false);
                        }

                    }
                    else
                    {

                    }

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

                // objChangepwd.struserId = objSession.UserId;


                string NewPwd = objChangepwd.strNewPwd;
                //ChangePassword.ValidatePwd a = new ChangePassword.ValidatePwd();
                bool PwdValidate = ChangePwdInternal.ValidatePwd.IsValidPwd(NewPwd);
                if (PwdValidate == true)
                {

                    if (ValidateForm() == true)
                    {
                        if (Convert.ToInt32(hdnpkid.Value) > 0)
                        {

                            objLogin.SubmitClick = Convert.ToInt32(hdnpkid.Value);

                            string MailLogDetails = objLogin.FetchMailHistory(objLogin);
                            if (MailLogDetails != "" && MailLogDetails != null)
                            {
                                string[] MailLogDetailsplit = MailLogDetails.Split('~');
                                objLogin.sFullName = Convert.ToString(MailLogDetailsplit[1]);
                                objChangepwd.struserId = Convert.ToString(MailLogDetailsplit[2]);
                            }
                        }
                        objLogin.sFullName = (objLogin.sFullName == null || objLogin.sFullName == "") ? Convert.ToString(Session["usname"]) : objLogin.sFullName;

                        if (txtNewPwd.Text == txtConfirmPwd.Text)
                        {

                            Arr = objChangepwd.ChangePwd(objChangepwd);

                            string clientip = Genaral.WorkFlowObjects();
                            if (Arr[1].ToString() == "1")
                            {
                                Session["clsSession"] = null;
                                Genaral.PasswordChangeLog(clientip, objChangepwd.struserId);
                                string message = "Password Changed Succesfully.Login with New Password";
                                string url = "../Login.aspx";
                                string script = "window.onload = function(){ alert('";
                                script += message;
                                script += "');";
                                script += "window.location = '";
                                script += url;
                                script += "'; }";
                                ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);

                                return;


                            }
                            if (Arr[1].ToString() == "0")
                            {

                                ShowMsgBox(Arr[0]);

                            }
                        }

                    }
                }
                else
                {
                    ShowMsgBox("Password Length Should 8 Character and  contains at least 1 Capital Letter or 1 Small Letter,1 Digit, 1 Special Character");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Something went wrong please try again after sometime!!!!";
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