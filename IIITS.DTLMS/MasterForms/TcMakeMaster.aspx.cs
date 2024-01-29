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
    public partial class TcMakeMaster : System.Web.UI.Page
    {
        string strFormCode = "TcMakeMaster";
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
                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;
                if (!IsPostBack)
                {
                    if (Request.QueryString["MakeId"] != null && Request.QueryString["MakeId"].ToString() != "")
                    {
                        txtMakeId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["MakeId"]));
                    }

                    if (txtMakeId.Text != "")
                    {

                        GetMakeDeatils(txtMakeId.Text);
                    }

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }

        }

        public void GetMakeDeatils(string strId)
        {

            try
            {
                clsTcMakeMaster objTcMakeMaster = new clsTcMakeMaster();

                objTcMakeMaster.sMakeId  = Convert.ToString(strId);
                objTcMakeMaster.GetTCMakeMasterDetails(objTcMakeMaster);

                txtMakeName.Text = Convert.ToString(objTcMakeMaster.sMakeName);
                txtCode.Text = Convert.ToString(objTcMakeMaster.sDescription);
                cmdSave.Text = "Update";

                //txtMakeName.Enabled = false;
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

                if (txtMakeName.Text.Trim().Length == 0)
                {
                    txtMakeName.Focus();
                    ShowMsgBox("Enter Valid Make Name");
                    return false;
                }
                if (txtMakeName.Text.Trim().StartsWith("."))
                {
                    txtMakeName.Focus();
                    ShowMsgBox("Enter Valid Make Name");
                    return false;
                }
                //if (!System.Text.RegularExpressions.Regex.IsMatch(txtMakeName.Text, @"^[a-zA-Z]*$"))
                //{
                //    txtMakeName.Focus();
                //    ShowMsgBox("Please enter Valid Make Name");
                //    return bValidate;
                //}
                if (txtCode.Text.Trim().Length == 0)
                {
                    txtCode.Focus();
                    ShowMsgBox("Enter Valid Desciption");
                    return false;
                }
                //if (!System.Text.RegularExpressions.Regex.IsMatch(txtCode.Text, @"^[a-zA-Z0-9 ,.-\\-_]*$"))
                //{
                //    txtCode.Focus();
                //    ShowMsgBox("Please enter Valid Desciption Name");
                //    return bValidate;
                //}


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
                //txtMakeId.Text = string.Empty;
                //txtMakeName.Text = string.Empty;
                //txtDescription.Text = string.Empty;
                //cmdSave.Text = "Save";

                //txtMakeName.Enabled = true;
                Response.Redirect("TcMakeMaster.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            clsTcMakeMaster objTcMakeMaster = new clsTcMakeMaster();
            string[] Arr = new string[2];
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

                if (ValidateForm() == true)
                {
                    objTcMakeMaster.sMakeId = txtMakeId.Text;
                    objTcMakeMaster.sMakeName  = txtMakeName.Text;
                    objTcMakeMaster.sDescription = txtCode.Text;
                    objTcMakeMaster.sCrby = objSession.UserId;
                 
                    Arr = objTcMakeMaster.SaveUpdateTcMakeMaster(objTcMakeMaster);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "Make Master");
                    }

                    if (Arr[1].ToString() == "0")
                    {
                        ShowMsgBox(Arr[0]);
                        //txtMakeId.Text = Arr[0].ToString();
                        //cmdSave.Text = "Update";
                       // Reset();
                        txtMakeId.Text = string.Empty;
                        txtMakeName.Text = string.Empty;
                        txtCode.Text = string.Empty;
                        return;
                    }

                    if (Arr[1].ToString() == "1")
                    {
                        ShowMsgBox(Arr[0]);
                        return;
                    }
                    if (Arr[1].ToString() == "4")
                    {
                        ShowMsgBox(Arr[0]);
                        
                    }
                   
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

                objApproval.sFormName = "TcMakeMaster";
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