using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Configuration;

namespace IIITS.DTLMS.MasterForms
{
    public partial class UpdateProfile : System.Web.UI.Page
    {
        string strFormCode = "UpdateProfile";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }

                lblMessage.Text = string.Empty;
                objSession = (clsSession)Session["clsSession"];
                if (!IsPostBack)
                {

                    LoadUserDetails(objSession.UserId);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
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

        public void LoadUserDetails(string strId)
        {
            try
            {
                clsUser objUser = new clsUser();
                string stroffCode = string.Empty;
                objUser.lSlNo = strId;

                objUser.GetUserDetails(objUser);

                txtuserID.Text = objUser.lSlNo;
                txtFullName.Text = objUser.sFullName;
                txtEmailId.Text = objUser.sEmail;
                txtMobile.Text = objUser.sMobileNo;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm() == true)
                {

                    clsUser objUser = new clsUser();
                    string[] Arr = new string[2];
                    objUser.sFullName = txtFullName.Text;
                    objUser.sEmail = txtEmailId.Text.ToLower();
                    objUser.sMobileNo = txtMobile.Text;
                    objUser.sCrby = objSession.UserId;
                    objUser.struserId = objSession.UserId;

                    Arr = objUser.UpdateProfile(objUser);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "Update Profile Master");
                    }
                    if (Arr[1].ToString() == "1")
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


                if (txtFullName.Text.Trim().Length == 0)
                {
                    txtFullName.Focus();
                    ShowMsgBox("Enter Full Name");
                    return bValidate;
                }
                if (txtFullName.Text.Trim().StartsWith("."))
                {
                    txtFullName.Focus();
                    ShowMsgBox("Full Name not Start with DOT(.)");
                    return bValidate;
                }

                //if (!System.Text.RegularExpressions.Regex.IsMatch(txtFullName.Text, @"^[a-zA-Z ]*$"))
                //{
                //    txtFullName.Focus();
                //    ShowMsgBox("Please enter Valid Full Name");
                //    return bValidate;
                //}


                if (txtEmailId.Text.Trim() == "")
                {
                    txtEmailId.Focus();
                    ShowMsgBox("Enter Email Id");
                    return bValidate;
                }

                //if ((txtEmailId.Text.Replace(" ", "").Length) >= 1)
                //{
                //    txtEmailId.Focus();
                //    ShowMsgBox("Full Name Should Not Be More 1 Spaces");
                //    return bValidate;
                //}

                //if ((txtEmailId.Text.Replace(".","").Length) >= 2)
                //{
                //    txtEmailId.Focus();
                //    ShowMsgBox("Full Name Should Not Be More 1 Spaces");
                //    return bValidate;
                //}
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmailId.Text, "^\\s*[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+[a-zA-Z0-9]\\s*$"))
                {
                    txtEmailId.Focus();
                    ShowMsgBox("Please enter Valid Email (xyz@aaa.com)");
                    return bValidate;
                }

            
                if (txtMobile.Text.Trim().Length == 0)
                {
                    txtMobile.Focus();
                    ShowMsgBox("Enter Mobile Number");
                    return bValidate;
                }
                if (txtMobile.Text.Length < 10)
                {
                    ShowMsgBox("Enter Valid Mobile Number");
                    txtMobile.Focus();
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


        protected void cmdClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (objSession.sRoleType == "2")
                {
                    Response.Redirect("~/StoreDashboard.aspx", false);
                }
                else
                {
                    Response.Redirect("~/Dashboard.aspx", false);
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