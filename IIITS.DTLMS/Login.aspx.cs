using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.IO;
using System.Data;
using System.Configuration;
using System.Net.Mail;
using IIITS.DTLMS.BL.MasterForms;

namespace IIITS.DTLMS
{
    public partial class Login : System.Web.UI.Page
    {
        //string strFormCode = "Login";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Text = string.Empty;
                Form.DefaultButton = cmdLogin.UniqueID;
                if ((Session["clsSession"] != null))
                {
                    Response.Redirect("Dashboard.aspx", false);
                }
                if (!IsPostBack)
                {
                    ViewState["OTP"] = null;
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// Login Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdLogin_Click(object sender, EventArgs e)
        {
            try
            {
                clsLogin objLogin = new clsLogin();
                clsSession objSession = new clsSession();
                DataTable dtConfiguration = new DataTable();
                ClsUserLoginDetails objUserLogin = new ClsUserLoginDetails();
                objLogin.sLoginName = txtUsername.Text.Trim().ToUpper();
                objLogin.sPassword = txtPassword.Text.Trim();
                if (ValidateForm() == true)
                {
                    objLogin.UserLogin(objLogin);
                    if (objLogin.sMessage == null)
                    {
                        Session["FullName"] = objLogin.sFullName;
                        Session["ChangPwd"] = objLogin.sChangePwd;
                        Session["sOfficeCode"] = objLogin.sOfficeCode;
                        if (objLogin.sOfficeCode == "0")
                        {
                            objLogin.sOfficeCode = "";
                        }
                        objSession.UserId = objLogin.sUserId;
                        objSession.FullName = objLogin.sFullName;
                        objSession.RoleId = objLogin.sRoleId;
                        objSession.OfficeCode = objLogin.sOfficeCode;
                        objSession.OfficeName = objLogin.sOfficeNamewithType;
                        objSession.Designation = objLogin.sDesignation;
                        objSession.OfficeNameWithType = objLogin.sOfficeNamewithType;
                        objSession.sRoleType = objLogin.sRoleType;
                        objLogin.sEmail = objLogin.sEmail;
                        objSession.sClientIP = Genaral.WorkFlowObjects();
                        String sStoreId = objLogin.sOfficeCode;
                        objUserLogin.InsertDIDetails(objSession.UserId, System.DateTime.Now);
                        if (objSession.sRoleType == "1")
                        {
                            objSession.sStoreID = sStoreId;
                        }
                        else
                        {
                            objSession.sStoreID = objLogin.sOfficeCode;
                        }
                        dtConfiguration = objLogin.GetConfiguration();
                        if (dtConfiguration.Rows.Count > 0)
                        {
                            objSession.sGeneralLog = Convert.ToString(dtConfiguration.Rows[0]["CG_GEN_LOG"]);
                            objSession.sTransactionLog = Convert.ToString(dtConfiguration.Rows[0]["CG_TRANS_LOG"]);
                            objSession.sPasswordChangeRequest = Convert.ToString(dtConfiguration.Rows[0]["CG_PASS_CHANGE_REQ"]);
                            objSession.sPasswordChangeInDays = Convert.ToString(dtConfiguration.Rows[0]["CG_PASS_CHANGE_DAYS"]);
                            objSession.sPassordAcceptance = Convert.ToString(dtConfiguration.Rows[0]["CG_PRE_PASS_ACCEPTANCE"]);
                        }
                        if (dtConfiguration.Rows.Count > 0)
                        {
                            if (objSession.sGeneralLog == "1") // Login Logout Log
                            {
                                Genaral.GeneralLog(objSession.sClientIP, objSession.UserId, "LOGIN");
                            }
                            if (objSession.sPasswordChangeRequest == "1") // check password by days
                            {
                                string numberOfDays = objLogin.GetPasswordDetails(objSession.UserId);
                                if (numberOfDays != null && numberOfDays != "")
                                {
                                    if (Convert.ToInt32(numberOfDays) > Convert.ToInt32(objSession.sPasswordChangeInDays))
                                    {
                                        Session["ChangPwd"] = "";
                                        Response.Redirect("~/MasterForms/ChangePassword.aspx", false);
                                    }
                                }
                            }
                        }
                        Session["clsSession"] = objSession;
                        if (Session["ChangPwd"] == null || Session["ChangPwd"].ToString() == "")
                        {
                            Response.Redirect("~/MasterForms/ChangePassword.aspx", false);
                        }
                        else if (objSession.sRoleType == "1" || objSession.sRoleType == "3")
                        {
                            Response.Redirect("Dashboard.aspx", false);
                        }
                        else
                        {
                            Response.Redirect("StoreDashboard.aspx", false);
                        }
                    }
                    else
                    {
                        lblMsg.Text = objLogin.sMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                Genaral.GeneralLog("Error in login :" + ex.Message, ex.StackTrace, "LOGIN");
                lblMsg.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// Send Mail.
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public string SendMail(clsLogin login)
        {
            List<string> uniques = login.sEmail.Split(',').Reverse().Take(100).Distinct().Reverse().ToList();
            login.sEmail = string.Join(",", uniques);
            Console.WriteLine(login.sEmail);
            double MailExpiryTime = Convert.ToDouble(ConfigurationManager.AppSettings["MAILSESSIONTIME"]);
            string DomainIP = Convert.ToString(ConfigurationManager.AppSettings["DomainIP"]);

            string strbody = string.Empty;
            DateTime dtTodayDate = DateTime.Now;
            string strTodayFormat = dtTodayDate.ToString("dd-MMM-yyyy HH:mm");

            DateTime dtExpireDate = DateTime.Now.AddMinutes(MailExpiryTime);
            string strExpireFormat = dtExpireDate.ToString("dd-MMM-yyyy HH:mm");
            strbody = " <html> ";
            strbody += " <head > </head> ";
            strbody += " <body> Dear " + login.sFullName + ",<br>";
            strbody += " &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; As per organization's ";
            strbody += " password policy and guidelines, your password for the application DTLMS has ";
            strbody += " expired on " + strTodayFormat + ". Please Click on <b> ";
            strbody += " <a style='font-weight:bold;color:blue ";
            strbody += " 'href='" + DomainIP + "/MasterForms/ChangePwdInternal.aspx?pkid=" + 
                HttpUtility.UrlEncode(Genaral.UrlEncrypt(Convert.ToString(login.SubmitClick))) 
                + "'> Link </a></b> to reset your password.";
            strbody += "<p> <span style='font-weight:bold'>Note:</span> This link will expire by <span>" + strExpireFormat + " </span></p>";
            strbody += "</p> <p> Regards, <br> Team IT-Admin </p> </body> </html>";
            try
            {
                if (Convert.ToString(ConfigurationManager.AppSettings["SENDEMAIL"]).ToUpper().Equals("ON"))
                {
                    string sendpwd = Convert.ToString(ConfigurationManager.AppSettings["SENDPWD"]);
                    string sendmailid = Convert.ToString(ConfigurationManager.AppSettings["SENDMAILID"]);
                    MailMessage mail = new MailMessage();
                    mail.To.Add(login.sEmail);
                    mail.From = new MailAddress(sendmailid, "Idea Infinity");
                    mail.Subject = "Change Password - DTLMS - " + login.sFullName + "";
                    mail.IsBodyHtml = true;
                    string Body = strbody;
                    mail.BodyEncoding = System.Text.Encoding.UTF8;
                    var varbody = AlternateView.CreateAlternateViewFromString(
                        Body, new System.Net.Mime.ContentType("text/html")
                        );
                    mail.AlternateViews.Add(varbody);

                    //   SmtpClient smtp = new SmtpClient("smtp.bizmail.yahoo.com", 587);
                    SmtpClient smtp = new SmtpClient(
                        Convert.ToString(ConfigurationManager.AppSettings["SENDSMTP"]),
                        Convert.ToInt32(ConfigurationManager.AppSettings["SENDSMTPPORT"])
                        );
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    //smtp.Credentials = new System.Net.NetworkCredential("bescomdtlms@ideainfinityit.com", "ctwubdqrhpphfclz");
                    smtp.Credentials = new System.Net.NetworkCredential(
                        Convert.ToString(ConfigurationManager.AppSettings["SENDMAILID"]),
                        Convert.ToString(ConfigurationManager.AppSettings["SENDPWD"])
                        );
                    smtp.EnableSsl = true;
                    smtp.Timeout = 500000;
                    smtp.Send(mail);
                    mail.Dispose();
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return "";
        }
        /// <summary>
        /// Validate Form.
        /// </summary>
        /// <returns></returns>
        public bool ValidateForm()
        {
            bool bValidate = true;
            try
            {
                if (txtUsername.Text == "" || txtUsername.Text == null)
                {
                    if (!txtEmail.Text.Contains("@"))
                    {
                        if (txtEmail.Text.Length == 10)
                        {
                            int Mob_First_Digit = Convert.ToInt32(txtEmail.Text.Substring(0, 1));
                            if (Mob_First_Digit <= 6)
                            {
                                txtEmail.Focus();
                                ShowMsgBox("Please Enter Valid Mobile Number");
                                return bValidate;
                            }
                        }
                        else
                        {
                            txtEmail.Focus();
                            ShowMsgBox("Please Enter Valid 10 Digit Mobile Number");
                            return bValidate;
                        }
                    }
                    else
                    {
                        if (!System.Text.RegularExpressions.Regex.IsMatch(
                            txtEmail.Text, "^\\s*[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+[a-zA-Z0-9]\\s*$"
                            )
                            )
                        {
                            txtEmail.Focus();
                            ShowMsgBox("Please enter Valid Email (xyz@aaa.com)");
                            return bValidate;
                        }
                    }
                    if (txtEmail.Text == "")
                    {
                        ShowMsgBox("Please Enter Register Mail Id / Mobile number to get OTP");
                        txtEmail.Focus();
                    }
                }
                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                lblMsg.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return bValidate;
            }
        }
        /// <summary>
        /// Show Msg Box.
        /// </summary>
        /// <param name="sMsg"></param>
        private void ShowMsgBox(string sMsg)
        {
            string sShowMsg = string.Empty;
            try
            {
                sShowMsg = "<script language=javascript> alert ('" + sMsg + "')</script>";
                this.Page.RegisterStartupScript("Msg", sShowMsg);
            }
            catch (Exception ex)
            {
                lblMsg.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// Save Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdFSave_Click(object sender, EventArgs e)
        {
            try
            {
                clsLogin objLogin = new clsLogin();
                objLogin.sEmail = txtEmail.Text;
                if (ValidateForm() == true)
                {
                    objLogin.sLoginName = txtUsername.Text;
                    objLogin = objLogin.CheckEmailOrMobialNoDetails(objLogin); //new implementation  
                    if (objLogin.VLDEmail_Or_Mob == false)
                    {
                        if (objLogin.sMessage == "Please enter valid user name.")
                        {
                            ResetPwd.Style.Add("display", "none");
                            Form2.Style.Add("display", "block");
                            logoblock.Style.Add("display", "block");
                        }
                        else
                        {
                            ResetPwd.Style.Add("display", "block");
                            Form2.Style.Add("display", "none");
                            logoblock.Style.Add("display", "none");
                        }
                        ShowMsgBox(objLogin.sMessage);
                        return;
                    }
                    objLogin.ForgtPassword(objLogin); //new implementation 
                    if (objLogin.sResult == "1")
                    {
                        ShowMsgBox(objLogin.sMessage);
                    }
                    else
                    {
                        if (objLogin.sMessage != null)
                        {
                            string sPattern = @"^\d{10}$";
                            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(sPattern);
                            if (r.IsMatch(objLogin.sEmail))
                            {
                                ShowMsgBox("OTP has been sent to your Mobile Number");
                                ResetPwd.Style.Add("display", "block");
                                Form2.Style.Add("display", "none");
                                logoblock.Style.Add("display", "none");
                            }
                            else
                            {
                                ShowMsgBox("OPT has been sent to your Registered Email ID");
                                ResetPwd.Style.Add("display", "block");
                                Form2.Style.Add("display", "none");
                                logoblock.Style.Add("display", "none");
                            }
                            cmdFSave.Enabled = true;
                            Form2.Visible = true;
                            ResetPwd.Visible = true;
                        }
                        else
                        {
                            ShowMsgBox(objLogin.sMessage);
                            Form2.Visible = true;
                            ResetPwd.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                      System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// ResetPwd_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnResetPwd_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtOtpDetails = new DataTable();
                DataTable dtConfiguration = new DataTable();
                string[] Arr = new string[2];
                if (txtNewpwd.Text == txtCnfrmPwd.Text)
                {
                    clsLogin objOTPDetails = new clsLogin();
                    dtOtpDetails = objOTPDetails.GetOTPDetails(txtOTP.Text, txtUsername.Text);
                    dtConfiguration = objOTPDetails.GetConfiguration();
                    if (Convert.ToString(dtConfiguration.Rows[0]["CG_PRE_PASS_ACCEPTANCE"]) == "1")
                    {
                        //bool res = objOTPDetails.GetStatus(Genaral.Encrypt(txtCnfrmPwd.Text),
                        //Convert.ToString(dtOtpDetails.Rows[0]["otp_us_id"]));
                        //if (res == false)
                        //{
                        //    ShowMsgBox("Password Should not be same as old");
                        //    return;
                        //}
                    }
                    else
                    {
                        if (dtOtpDetails.Rows.Count > 0)
                        {
                            if (dtOtpDetails.Rows[0]["otp_no"].ToString() == txtOTP.Text)
                            {
                                clsUser objUser = new clsUser();
                                objUser.sPassword = txtCnfrmPwd.Text;
                                objUser.lSlNo = dtOtpDetails.Rows[0]["otp_us_id"].ToString();
                                objUser.sOTP = dtOtpDetails.Rows[0]["otp_no"].ToString();
                                string sClientIP = Genaral.WorkFlowObjects();
                                Genaral.PasswordChangeLog(sClientIP, Convert.ToString(dtOtpDetails.Rows[0]["otp_us_id"]));
                                Arr = objUser.UpdatePwd(objUser);
                                ShowMsgBox(Arr[1]);
                                if (Arr[0] == "1")
                                {
                                    ResetPwd.Style.Add("display", "none");
                                    Form2.Style.Add("display", "block");
                                    logoblock.Style.Add("display", "block");
                                }
                            }
                            else
                            {
                                ShowMsgBox("Your OTP Expired Please Generate New OTP");
                                txtOTP.Text = string.Empty;
                                txtNewpwd.Text = string.Empty;
                                txtCnfrmPwd.Text = string.Empty;
                                ResetPwd.Style.Add("display", "block");
                                Form2.Style.Add("display", "none");
                                logoblock.Style.Add("display", "none");
                            }
                        }
                        else
                        {
                            ShowMsgBox("Please Enter Correct OTP");
                            txtOTP.Text = string.Empty;
                            txtNewpwd.Text = string.Empty;
                            txtCnfrmPwd.Text = string.Empty;
                            ResetPwd.Style.Add("display", "block");
                            Form2.Style.Add("display", "none");
                            logoblock.Style.Add("display", "none");
                        }

                    }
                }
                else
                {
                    ShowMsgBox("New Password and Confirm Password Should Be Same");
                    ResetPwd.Style.Add("display", "block");
                    Form2.Style.Add("display", "none");
                    logoblock.Style.Add("display", "none");
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

    }
}