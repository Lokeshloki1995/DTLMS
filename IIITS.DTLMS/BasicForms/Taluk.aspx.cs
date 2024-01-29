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
    public partial class Taluk : System.Web.UI.Page
    {
        string strFormCode = "Taluk";
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
                    Form.DefaultButton = cmdSave.UniqueID;
                    Update.Visible = false;
                    UpdateTaluk.Visible = false;
                    objSession = (clsSession)Session["clsSession"];

                    if (!IsPostBack)
                    {
                        LoadDropDown();
                        if (Request.QueryString["TalukId"] != null && Convert.ToString(Request.QueryString["TalukId"]) != "")
                        {
                            CheckAccessRights("4");
                            txtTlkId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TalukId"]));
                            GetTalukDetails(txtTlkId.Text);
                            Create.Visible = false;
                            CreateTaluk.Visible = false;

                            Update.Visible = true;
                            UpdateTaluk.Visible = true;
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadDropDown()
        {
            try
            {
                Genaral.Load_Combo("SELECT \"DT_CODE\",\"DT_NAME\" FROM \"TBLDIST\" ORDER BY \"DT_CODE\" ", "---Select---", cmbDistName);
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

                if (cmbDistName.SelectedValue == "")
                {
                    ShowMsgBox("Select District Name");
                    cmbDistName.Focus();
                    return;
                }

                if (txtTalukCode.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Enter Taluk Code");
                    txtTalukCode.Focus();
                    return;
                }

                if (txtTalukName.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Enter Taluk Name");
                    txtTalukName.Focus();
                    return;
                }

                if (txtTalukName.Text.Trim().StartsWith("."))
                {
                    ShowMsgBox("Enter  valid Taluk Name");
                    txtTalukName.Focus();
                    return;
                }

    

                //if (!System.Text.RegularExpressions.Regex.IsMatch(txtTalukName.Text, "^\\s*[a-zA-Z0-9 \\s]+\\s*[a-zA-Z0-9_.+-]\\s*$"))
                //{
                //    ShowMsgBox("Please enter Valid Taluk Name ");
                //    txtTalukName.Focus();
                //    return;
                //}

                string txtTalukCoderesult = txtTalukCode.Text.Remove(txtTalukCode.Text.Length - 1);


                if (Convert.ToString(cmbDistName.SelectedValue) != txtTalukCoderesult)
                {
                    ShowMsgBox("District Code and TaluK Code Does not Match");
                    txtTalukCode.Focus();
                    return;
                }

                clsTaluk objTlk = new clsTaluk();
                string[] Arr = new string[2];
                string sname = string.Empty;
                objTlk.sTalukId = txtTlkId.Text;
                objTlk.sDistrictName = Convert.ToString(cmbDistName.SelectedValue);
                objTlk.sTalukCode = txtTalukCode.Text;
                objTlk.sButtonName = cmdSave.Text;
                sname = txtTalukName.Text.Replace("'", "''");
                objTlk.sTalukName = sname;

                Arr = objTlk.SaveDetails(objTlk);

                if (objSession.sTransactionLog == "1")
                {
                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " Taluk Master ");
                }

                if (Arr[1] == "2")
                {
                    txtTlkId.Text = Arr[0].ToString();
                    cmdSave.Text = "Update";
                    ShowMsgBox(Arr[0]);


                }
                else
                {
                    ShowMsgBox(Arr[0]);
                }
                cmbDistName.SelectedIndex = 0;
                txtTalukCode.Text = string.Empty;
                txtTalukName.Text = string.Empty;
                cmbDistName.Enabled = true;
                txtTalukCode.Enabled = true;
                txtTalukCode.ReadOnly = false;
                cmdSave.Text = "Save";
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

        protected void cmdClear_Click(object sender, EventArgs e)
        {
            try
            {
                cmbDistName.SelectedIndex = 0;
                txtTalukCode.Text = string.Empty;
                txtTalukName.Text = string.Empty;
                cmbDistName.Enabled = true;
                txtTalukCode.Enabled = true;
                txtTalukCode.ReadOnly = false;
                cmdSave.Text = "Save";
                //Response.Redirect("Taluk.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }



        public void GetTalukDetails(string strTalukId)
        {
            try
            {
                clsTaluk objTaluk = new clsTaluk();
                DataTable dtDetails = new DataTable();
                objTaluk.sTalukId = Convert.ToString(strTalukId);
                objTaluk.GetTlkDetails(objTaluk);
                txtTalukCode.Text = Convert.ToString(objTaluk.sTalukCode);
                txtTalukName.Text = Convert.ToString(objTaluk.sTalukName);
                cmbDistName.SelectedValue = objTaluk.sDistrictName;
                txtTlkId.Text = objTaluk.sTalukId;
                txtTalukCode.Enabled = false;
                cmbDistName.Enabled = false;
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

                objApproval.sFormName = "Taluk";
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
                //lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;

            }
        }

        #endregion

        protected void cmbDistName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                clsTaluk objTaluk = new clsTaluk();
                objTaluk.sDistrictName = cmbDistName.SelectedValue;
                txtTalukCode.Text = objTaluk.GenerateTalukCode(objTaluk);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }



    }
}