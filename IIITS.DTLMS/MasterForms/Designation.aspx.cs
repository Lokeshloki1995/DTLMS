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
    public partial class Designation : System.Web.UI.Page
    {

        string strFormCode = "Designation.aspx";      
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
                    Update.Visible = false;
                    UpdateDesignation.Visible = false;

                    Form.DefaultButton = cmdSave.UniqueID;
                    objSession = (clsSession)Session["clsSession"];
                    lblMessage.Text = string.Empty;
                     CheckAccessRights("4");
                    //AdminAccess();
                    if (!IsPostBack)
                    {

                        if (Request.QueryString["StrQryId"] != null && Convert.ToString(Request.QueryString["StrQryId"]) != "")
                        {
                            txtDesignationId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["StrQryId"]));
                        }

                        if (txtDesignationId.Text != "")
                        {
                           
                            GetDesigantionDetails(txtDesignationId.Text);
                            Create.Visible = false;
                            CreateDesignation.Visible = false;

                            Update.Visible = true;
                            UpdateDesignation.Visible = true;
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        public void AdminAccess()
        {
            try
            {
                if (objSession.RoleId != "11")
                {
                    Response.Redirect("~/UserRestrict.aspx", false);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetDesigantionDetails(string strDesignationId)
        {
            try
            {
                clsDesignation objDesignation = new clsDesignation();
                DataTable dtDetails = new DataTable();

                objDesignation.sDesignationId = Convert.ToString(strDesignationId);
                objDesignation.getDesignationDetails(objDesignation);
                txtDesignationId.Text = Convert.ToString(objDesignation.sDesignationId);
                txtDesignation.Text = Convert.ToString(objDesignation.sDesignationName);
                txtDescription.Text = Convert.ToString(objDesignation.sDesignationDesc); 
                cmdSave.Text = "Update";

                //txtDesignation.Enabled = false;
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
                clsDesignation objDesignation = new clsDesignation();
                string[] Arr = new string[3];

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
                 
                    objDesignation.sDesignationId = Convert.ToString(txtDesignationId.Text);
                    objDesignation.sDesignationName = txtDesignation.Text.Replace("'", "");
                    objDesignation.sDesignationDesc = txtDescription.Text.Replace("'", "''");
                    objDesignation.sCrby = objSession.UserId;
                    
                    Arr = objDesignation.SaveDetails(objDesignation);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " Designation Master ");
                    }

                    if (Arr[1].ToString() == "0")
                    {
                        txtDesignationId.Text = Arr[0].ToString();
                        cmdSave.Text = "Update";
                        ShowMsgBox(Arr[0].ToString());
                        cmdReset_Click(sender, e);
                        
                    }
                    if (Arr[1].ToString() == "1")
                    {
                        ShowMsgBox(Arr[0]);
                        return;
                    }

                    if (Arr[1].ToString() == "2")
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


        private void ShowMsgBox(string sMsg)
        {
            try
            {
                string sShowMsg = string.Empty;
                sShowMsg = "<script language=javascript> alert ('" + sMsg + "')</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "Msg", sShowMsg);
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
          
            if (txtDesignation.Text == "")
            {
                txtDesignation.Focus();
                ShowMsgBox("Please Enter the Desgnation");
                return bValidate;
            }
            if (txtDescription.Text == "")
            {
                txtDescription.Focus();
                ShowMsgBox("Please Enter the Description");
                return bValidate;
            }


            if (txtDesignation.Text != "")
            {
                if ((txtDesignation.Text.Length - txtDesignation.Text.Replace(" ", "").Length) >= 3)
                {
                    txtDesignation.Focus();
                    ShowMsgBox("You cannot use more than two space");
                    return bValidate;
                }

            }

            bValidate = true;
            return bValidate;
        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            txtDescription.Text = string.Empty;
            txtDesignation.Text = string.Empty;
            txtDesignationId.Text = string.Empty;
            cmdSave.Text = "Save";

            txtDesignation.Enabled = true;

        }

        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "Designation";
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