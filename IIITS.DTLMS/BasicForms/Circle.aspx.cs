using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;

namespace IIITS.DTLMS.BasicForms
{
    public partial class Circle : System.Web.UI.Page
    {
        string strFormCode = "Circle";
        clsSession objSession;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
            {
                Response.Redirect("~/Login.aspx", false);
            }
            else
            {
                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;
                Update.Visible = false;
                UpdateCircle.Visible = false;
                Form.DefaultButton = cmdSave.UniqueID;
                if (!IsPostBack)
                {

                    Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone);
                    // GenerateCircleCode();
                    if (Request.QueryString["CircleId"] != null && Convert.ToString(Request.QueryString["CircleId"]) != "")
                    {

                        CheckAccessRights("4");
                        txtCrID.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["CircleId"]));
                        GetCircleDetails(txtCrID.Text);
                        Create.Visible = false;
                        CreateCircle.Visible = false;

                        Update.Visible = true;
                        UpdateCircle.Visible = true;
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


                clsCircle objCircle = new clsCircle();
                string[] Arr = new string[3];
                if (ValidateForm() == true)
                {
                    //objCircle.sDepartmentId = Convert.ToString(txtDepId.Text);
                    objCircle.sCircleCode = txtCircleCode.Text.Trim().Replace("'", "");
                    objCircle.sZoneCode = cmbZone.SelectedValue;
                    objCircle.sCircleName = txtCircleName.Text.Trim().Replace("'", "");
                    objCircle.sName = txtFullName.Text.Trim().Replace("'", "");
                    objCircle.sMobileNo = txtMobile.Text.Trim().Replace("'", "");
                    objCircle.sPhone = txtPhone.Text.Trim().Replace("'", "");
                    objCircle.sEmail = txtEmailId.Text.Trim().Replace("'", "");
                    objCircle.sMaxid = txtCrID.Text.Trim().Replace("'", "");
                    objCircle.sAddress = txtAddress.Text.Trim().Replace("'", "");


                    Arr = objCircle.SaveCircle(objCircle);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " Circle Master ");
                    }
                    if (Convert.ToString(Arr[1]) == "0")
                    {
                        txtCrID.Text = objCircle.sMaxid;
                        cmdSave.Text = "Update";
                        txtCircleCode.Enabled = false;
                        ShowMsgBox(Convert.ToString(Arr[0]));

                        Reset();
                    }
                    else if (Convert.ToString(Arr[1]) == "1")
                    {

                        ShowMsgBox(Convert.ToString(Arr[0]));
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

        protected void cmbZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                clsCircle objcir = new clsCircle();
                objcir.sZoneCode = cmbZone.SelectedValue;
                txtCircleCode.Text = objcir.GenerateCirCode(objcir);
                txtCircleCode.ReadOnly = true;
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

            if (cmbZone.SelectedIndex == 0)
            {
                ShowMsgBox("Select The Zone");
                return bValidate;
            }
            if (txtCircleCode.Text.Trim().Length == 0)
            {
                txtCircleCode.Focus();
                ShowMsgBox("Please Enter the Circle Code");
                return bValidate;
            }



            if (txtCircleName.Text.Trim().Length == 0)
            {
                txtCircleCode.Focus();
                ShowMsgBox("Please Enter the Circle Name");
                return bValidate;
            }

            //if (!System.Text.RegularExpressions.Regex.IsMatch(txtCircleName.Text, @"^[a-zA-Z)]+$"))
            //{
            //    txtCircleCode.Focus();
            //    ShowMsgBox("Please Enter the Circle Name");
            //    return bValidate;

            //}


                if (txtCircleName.Text.Trim().StartsWith("."))
            {
                txtCircleName.Focus();
                ShowMsgBox("Enter valid Circle Name");
                return bValidate;
            }



            if (txtFullName.Text.Trim().Length == 0)
            {
                txtFullName.Focus();
                ShowMsgBox("Please Enter Name Of Head");
                return bValidate;
            }

            if (txtFullName.Text.Trim().StartsWith("."))
            {
                txtFullName.Focus();
                ShowMsgBox("Enter valid Name Of Head");
                return bValidate;
            }

            //if (!System.Text.RegularExpressions.Regex.IsMatch(txtFullName.Text, "^\\s*[a-zA-Z0-9]+\\s*[a-zA-Z0-9_.+-]\\s*$"))
            //{
            //    ShowMsgBox("Please enter Valid Superintending Engineer(ele) ");
            //    return bValidate;
            //}
            if (txtMobile.Text.Trim().Length == 0 || txtMobile.Text.Length < 10)
            {
                txtMobile.Focus();
                ShowMsgBox("Please Enter the Valid Mobile No ");
                return bValidate;
            }

            if (txtMobile.Text[0] == '0' || txtMobile.Text[0] == '1' || txtMobile.Text[0] == '2')
            {
                ShowMsgBox("Please Enter Valid Mobile No");
                txtMobile.Focus();
                return false;
            }
            if (txtEmailId.Text.Trim().Length == 0)
            {
                txtEmailId.Focus();
                ShowMsgBox("Please Enter tEmail Id");
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
                //    ShowMsgBox("Please enter Valid 12 Digit Phone No");
                //    return bValidate;
                //}
                //if (txtPhone.Text.indexOf("-") > 1)
                //{
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
                cmbZone.SelectedIndex = 0;
                txtCircleCode.Text = "";
                txtCircleName.Text = "";
                txtEmailId.Text = "";
                txtFullName.Text = "";
                txtMobile.Text = "";
                txtPhone.Text = "";
                txtAddress.Text = "";
                txtCircleCode.Enabled = true;
                cmdSave.Text = "Save";
                txtCrID.Text = "";
                txtCircleCode.Enabled = true;
                cmbZone.Enabled = true;
                txtCircleCode.ReadOnly = false;

                //GenerateCircleCode();
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }



        public void GetCircleDetails(string strCircleId)
        {
            try
            {
                clsCircle objCircle = new clsCircle();
                DataTable dtDetails = new DataTable();

                objCircle.sMaxid = Convert.ToString(strCircleId);
                objCircle.getCircleDetails(objCircle);
                txtCircleCode.Text = Convert.ToString(objCircle.sCircleCode);
                txtCircleName.Text = Convert.ToString(objCircle.sCircleName);
                txtFullName.Text = Convert.ToString(objCircle.sName);
                txtMobile.Text = Convert.ToString(objCircle.sMobileNo);
                txtPhone.Text = Convert.ToString(objCircle.sPhone);
                txtEmailId.Text = Convert.ToString(objCircle.sEmail);
                txtCrID.Text = Convert.ToString(objCircle.sMaxid);
                cmbZone.SelectedValue = objCircle.sZoneCode;
                txtAddress.Text = objCircle.sAddress;
                txtCircleCode.Enabled = false;
                cmbZone.Enabled = false;
                cmdSave.Text = "Update";


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        //public void GenerateCircleCode()
        //{
        //    try
        //    {
        //        clsCircle objCircleCode = new clsCircle();

        //        txtCircleCode.Text = objCircleCode.GenerateCircleCode();
        //        txtCircleCode.ReadOnly = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GenerateCircleCode");
        //    }
        //}

        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "Circle";
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