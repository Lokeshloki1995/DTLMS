using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Configuration;



namespace IIITS.DTLMS.BasicForms
{
    public partial class Division : System.Web.UI.Page
    {
        string strFormCode = "Division";
        clsSession objSession;
        int Circle_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Circle_code"]);
        int Division_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Division_code"]);
        int SubDiv_code = Convert.ToInt32(ConfigurationSettings.AppSettings["SubDiv_code"]);
        int Section_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Section_code"]);
        int Feeder_code = Convert.ToInt32(ConfigurationSettings.AppSettings["feeder_code"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
            {
                Response.Redirect("~/Login.aspx", false);
            }
            else
            {
                Update.Visible = false;
                UpdateDivision.Visible = false;
                Form.DefaultButton = cmdSave.UniqueID;
                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;
                if (!IsPostBack)
                {

                    Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_CODE\" || '-' || \"CM_CIRCLE_NAME\" AS \"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" ORDER BY \"CM_CIRCLE_CODE\"", "--Select--", cmbCircle);
                    if (Request.QueryString["DivId"] != null && Convert.ToString(Request.QueryString["DivId"]) != "")
                    {
                        CheckAccessRights("4");
                        txtDivid.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["DivId"]));
                        GetDivDetails(txtDivid.Text);
                        Create.Visible = false;
                        CreateDivision.Visible = false;

                        Update.Visible = true;
                        UpdateDivision.Visible = true;
                    }
                }
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

                clsDivision objDivision = new clsDivision();
                string[] Arr = new string[3];
                if (ValidateForm() == true)
                {
                    //objCircle.sDepartmentId = Convert.ToString(txtDepId.Text);
                    objDivision.sCircleCode = cmbCircle.SelectedValue;
                    objDivision.sDivisionCode = txtDivisionCode.Text.Trim().Replace("'", ""); 
                    objDivision.sDivisionName = txtDivisionName.Text.Trim().Replace("'", ""); 
                    objDivision.sName = txtFullName.Text.Trim().Replace("'", ""); 
                    objDivision.sMobileNo = txtMobile.Text.Trim().Replace("'", ""); 
                    objDivision.sPhone = txtPhone.Text.Trim().Replace("'", ""); 
                    objDivision.sEmail = txtEmailId.Text.Trim().Replace("'", ""); 
                    objDivision.sMaxid = txtDivid.Text.Trim().Replace("'", ""); 
                    objDivision.sAddress = txtDivAddress.Text.Trim().Replace("'", "");

                    Arr = objDivision.SaveDivision(objDivision);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " Division Master ");
                    }

                    if (Arr[1].ToString() == "0")
                    {
                        txtDivid.Text = objDivision.sMaxid;
                        cmdSave.Text = "Update";
                        cmbCircle.Enabled = false;
                        txtDivisionCode.Enabled = false;
                        ShowMsgBox(Arr[0].ToString());
                        Reset();
                       
                    }
                    else if (Arr[1].ToString() == "1")
                    {
                      
                        ShowMsgBox(Arr[0].ToString());
                        Reset();
                    }
                    else
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

            if (cmbCircle.SelectedIndex == 0)
            {
                cmbCircle.Focus();
                ShowMsgBox("Please Select the Circle");
                return bValidate;
            }
            if (txtDivisionCode.Text == "" || txtDivisionCode.Text.Length < Division_code)
            {
                txtDivisionCode.Focus();
                ShowMsgBox("Please Enter the Division Code");
                return bValidate;
            }
            if (cmbCircle.SelectedValue != txtDivisionCode.Text.Substring(0, Circle_code))
            {
                txtDivisionCode.Focus();
                ShowMsgBox("Circle Code Doesnot match With Division Code");
                return bValidate;
            }

            if (txtDivisionName.Text.Trim() == "")
            {
                txtDivisionName.Focus();
                ShowMsgBox("Please Enter the Division Name");
                return bValidate;
            }

            if (txtDivisionName.Text.Trim().StartsWith("."))
            {
                txtDivisionName.Focus();
                ShowMsgBox("Enter valid Division Name");
                return bValidate;
            }


            if (txtFullName.Text.Trim() == "")
            {
                txtFullName.Focus();
                ShowMsgBox("Please Enter Name Of Head");
                return bValidate;
            }

            if (txtFullName.Text.Trim().StartsWith("."))
            {
                txtFullName.Focus();
                ShowMsgBox("Enter valid name Of the Head");
                return bValidate;
            }


            //if (!System.Text.RegularExpressions.Regex.IsMatch(txtFullName.Text, "^\\s*[a-zA-Z0-9]+\\s*[a-zA-Z0-9_.+-]\\s*$"))
            //{
            //    ShowMsgBox("Please enter Valid Division Head Name ");
            //    return bValidate;
            //}
            if (txtMobile.Text == "" || txtMobile.Text.Length <10)
            {
                txtMobile.Focus();
                ShowMsgBox("Please Enter Valid Mobile No");
                return bValidate;
            }
            if (txtMobile.Text[0] == '0' || txtMobile.Text[0] == '1' || txtMobile.Text[0] == '2')
            {
                ShowMsgBox("Please Enter Valid Mobile No");
                txtMobile.Focus();
                return false;
            }

            if (txtEmailId.Text.Trim() == "")
            {
                txtEmailId.Focus();
                ShowMsgBox("Please Enter Email Id");
                return bValidate;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmailId.Text, "^\\s*[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+[a-zA-Z0-9]\\s*$"))
            {
                ShowMsgBox("Please enter Valid Email (xyz@aaa.com)");
                return bValidate;
            }
            if (txtPhone.Text != "")
            {
                //if (txtPhone.Text.Length < 12)
                //{
                //    txtPhone.Focus();
                //    ShowMsgBox("Please enter Valid Phone No");
                //    return bValidate;
                //}
                if ((txtPhone.Text.Length - txtPhone.Text.Replace("-", "").Length) >= 2)
                {
                    txtPhone.Focus();
                    ShowMsgBox("Please enter Valid Phone No");
                    return bValidate;
                }
                if (txtPhone.Text.Contains(".") == true)
                {
                    txtPhone.Focus();
                    ShowMsgBox("Please enter Valid Phone No");
                    return bValidate;
                }

                if (txtPhone.Text.Replace("0", "").Length == 0)
                {
                    txtPhone.Focus();

                    ShowMsgBox("Please enter Valid Phone No");

                    return false;
                }

            }

            bValidate = true;
            return bValidate;
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
                cmbCircle.SelectedIndex = 0;
                txtDivisionCode.Text = "";
                txtEmailId.Text = "";
                txtFullName.Text = "";
                txtMobile.Text = "";
                txtPhone.Text = "";
                txtDivisionName.Text = "";
                cmdSave.Text = "Save";
                txtDivid.Text = "";
                txtDivAddress.Text = "";
                cmbCircle.Enabled = true;
                txtDivisionCode.Enabled = true;
                txtDivisionCode.ReadOnly = false;
                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void GetDivDetails(string strCircleId)
        {
            try
            {
                clsDivision objDivision = new clsDivision();
                DataTable dtDetails = new DataTable();

                objDivision.sMaxid = Convert.ToString(strCircleId);
                objDivision.getDivisionDetails(objDivision);
                txtDivisionCode.Text = Convert.ToString(objDivision.sDivisionCode);
                txtDivisionName.Text = Convert.ToString(objDivision.sDivisionName);
                txtFullName.Text = Convert.ToString(objDivision.sName);
                txtMobile.Text = Convert.ToString(objDivision.sMobileNo);
                txtPhone.Text = Convert.ToString(objDivision.sPhone);
                txtEmailId.Text = Convert.ToString(objDivision.sEmail);
                txtDivAddress.Text = Convert.ToString(objDivision.sAddress);
                cmbCircle.SelectedValue = objDivision.sCircleCode;
                cmbCircle.Enabled = false;
                txtDivisionCode.Enabled = false;
                cmdSave.Text = "Update";


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

                objApproval.sFormName = "Division";
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


   

        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                clsDivision objdiv = new clsDivision();
                objdiv.sCircleCode = cmbCircle.SelectedValue;
                txtDivisionCode.Text = objdiv.GenerateDivCode(objdiv);
                txtDivisionCode.ReadOnly = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}