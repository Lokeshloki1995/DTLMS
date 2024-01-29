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
    public partial class Zone : System.Web.UI.Page
    {
        string strFormCode = "Zone";
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
                UpdateZone.Visible = false;

                Form.DefaultButton = cmdSave.UniqueID;
                if (!IsPostBack)
                {
                    GenerateZoneId();
                    if (Request.QueryString["ZoneId"] != null && Convert.ToString(Request.QueryString["ZoneId"]) != "")
                    {

                        CheckAccessRights("4");
                        txtZnId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ZoneId"]));
                        GetZoneDetails(txtZnId.Text);
                        Create.Visible = false;
                        CreateZone.Visible = false;

                        Update.Visible = true;
                        UpdateZone.Visible = true;
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


                 clsZone objZone= new clsZone();
                string[] Arr = new string[3];
                if (ValidateForm() == true)
                {
                    //objCircle.sDepartmentId = Convert.ToString(txtDepId.Text);
                    objZone.sZoneId = txtZoneId.Text.Trim().Replace("'", "");
                    objZone.sZoneName = txtZoneName.Text.Trim().Replace("'", "");
                    objZone.sName = txtFullName.Text.Trim().Replace("'", "");
                    objZone.sMobileNo = txtMobile.Text.Trim().Replace("'", "");
                    objZone.sPhone = txtPhone.Text.Trim().Replace("'", "");
                    objZone.saddr = txtZoneAddress.Text.Trim().Replace("'", "");
                    objZone.sMaxid = txtZnId.Text.Trim().Replace("'", "");
                    objZone.sEmailId = txtEmailId.Text.Trim().Replace("'", "");


                    Arr = objZone.SaveZone(objZone);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " Zone Master ");
                    }

                    if (Arr[1].ToString() == "0")
                    {
                        txtZnId.Text = objZone.sMaxid;
                        cmdSave.Text = "Update";
                        txtZoneId.Enabled = false;
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

            if (txtZoneId.Text.Trim().Length == 0)
            {
                txtZoneId.Focus();
                ShowMsgBox("Please Enter the Zone Id");
                return bValidate;
            }

            if (txtZoneName.Text.Trim().Length == 0)
            {
                txtZoneName.Focus();
                ShowMsgBox("Please Enter the Zone Name");
                return bValidate;
            }

            if (txtZoneName.Text.Trim().StartsWith("."))
            {
                txtZoneName.Focus();
                ShowMsgBox("Enter valid Zone Name");
                return bValidate;
            }


            if (txtFullName.Text.Trim().StartsWith("."))
            {
                txtFullName.Focus();
                ShowMsgBox("Enter valid Name Of Head");
                return bValidate;
            }

            //if (!System.Text.RegularExpressions.Regex.IsMatch(txtZoneName.Text, "^\\s*[a-zA-Z0-9 \\s]+\\s*[a-zA-Z0-9_.+-]\\s*$"))
            //{
            //    ShowMsgBox("Please enter Valid Zone name ");
            //    return bValidate;
            //}
            if (txtFullName.Text.Trim().Length == 0)
            {
                txtFullName.Focus();
                ShowMsgBox("Please Enter Name Of Head");
                return bValidate;
            }



            //if (!System.Text.RegularExpressions.Regex.IsMatch(txtFullName.Text, "^\\s*[a-zA-Z0-9 \\s]+\\s*[a-zA-Z0-9_.+-]\\s*$"))
            //{
            //    ShowMsgBox("Please enter Valid Name Of Head ");
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



            if (txtPhone.Text.Trim().Length == 0 || txtPhone.Text.Length < 10)
            {
                txtPhone.Focus();
                ShowMsgBox("Please Enter the Valid Phone No ");
                return bValidate;
            }
            if (txtZoneAddress.Text == "")
            {
                txtZoneAddress.Focus();
                ShowMsgBox("Please Enter Valid  Address");
                return bValidate;
            }



            if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmailId.Text, "^\\s*[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+[a-zA-Z0-9]\\s*$"))
            {
                txtEmailId.Focus();
                ShowMsgBox("Please enter Valid Email (xyz@aaa.com)");
                return bValidate;
            }

            if (txtPhone.Text != "")
            {
                //if (txtPhone.Text.Length > 12)
                //{
                //    txtPhone.Focus();
                //    ShowMsgBox("Please enter Valid Phone No");
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
                txtZoneId.Text = "";
                txtZoneName.Text = "";
                txtZoneAddress.Text = "";
                txtFullName.Text = "";
                txtMobile.Text = "";
                txtPhone.Text = "";
                txtZoneId.Enabled = true;
                cmdSave.Text = "Save";
                txtZnId.Text = "";
                txtZoneId.Enabled = true;
                txtZoneId.ReadOnly = false;
                txtEmailId.Text = "";

                GenerateZoneId();
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }



        public void GetZoneDetails(string strZoneId)
        {
            try
            {
                clsZone objZone = new clsZone();
                DataTable dtDetails = new DataTable();

                objZone.sMaxid = Convert.ToString(strZoneId);
                objZone.getZoneDetails(objZone);
                txtZoneId.Text = Convert.ToString(objZone.sZoneId);
                txtZoneName.Text = Convert.ToString(objZone.sZoneName);
                txtFullName.Text = Convert.ToString(objZone.sName);
                txtMobile.Text = Convert.ToString(objZone.sMobileNo);
                txtPhone.Text = Convert.ToString(objZone.sPhone);
                txtZoneAddress.Text = Convert.ToString(objZone.saddr);
                txtZnId.Text = Convert.ToString(objZone.sMaxid);
                txtEmailId.Text = Convert.ToString(objZone.sEmailId);
                txtZoneId.Enabled = false;
                //cmdSave.Text = "Update";
                cmdSave.Text = "Update";


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        public void GenerateZoneId()
        {
            try
            {
                clsZone objZone = new clsZone();

                txtZoneId.Text = objZone.GenerateZoneId();
                txtZoneId.ReadOnly = true;
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
