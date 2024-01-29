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
using System.Reflection;

namespace IIITS.DTLMS.MasterForms
{
    public partial class UserCreate : System.Web.UI.Page
    {
        clsSession objSession;
        int Zone_code = Convert.ToInt32(ConfigurationManager.AppSettings["Zone_code"]);
        int Circle_code = Convert.ToInt32(ConfigurationManager.AppSettings["Circle_code"]);
        int Division_code = Convert.ToInt32(ConfigurationManager.AppSettings["Division_code"]);
        int SubDiv_code = Convert.ToInt32(ConfigurationManager.AppSettings["SubDiv_code"]);
        int Section_code = Convert.ToInt32(ConfigurationManager.AppSettings["Section_code"]);
        int Feeder_code = Convert.ToInt32(ConfigurationManager.AppSettings["feeder_code"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToString(Session["clsSession"] ?? "").Length == 0)
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                Update.Visible = false;
                UpdateUser.Visible = false;
                Form.DefaultButton = cmdSave.UniqueID;
                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;
                string Orginal_OffCode = string.Empty;
                bool AccessResult = CheckAccessRights(Constants.CheckAccessRights.CheckAccessRightsReadOnly);
                if (!IsPostBack)
                {
                    if (Request.QueryString["QryUserId"] != null && Request.QueryString["QryUserId"].ToString() != "")
                    {
                        txtuserID.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryUserId"]));
                    }

                    if (AccessResult == true)
                    {
                        if (objSession.sRoleType == Constants.Roles.OfficeLevel) // logic for the EE_DO
                        {
                            Orginal_OffCode = objSession.OfficeCode;
                        }
                        LoadDropDownDetailsOnOffCode(Orginal_OffCode);
                    }

                    divstore.Visible = false;
                    if (txtuserID.Text != "")
                    {
                        LoadUserDetails(txtuserID.Text);
                        //if (cmbSubDiv.SelectedValue.Length == 4)
                        //{
                        //    cmbSubDiv_SelectedIndexChanged(sender, e);
                        //}
                        //else if (cmbDiv.SelectedValue.Length == 3)
                        //{
                        //    cmbDiv_SelectedIndexChanged(sender, e);
                        //}
                        //else if (cmbCircle.SelectedValue.Length == 2)
                        //{
                        //    cmbCircle_SelectedIndexChanged(sender, e);
                        //}
                        //else if (cmbZone.SelectedValue.Length == 1)
                        //{
                        //    cmbZone_SelectedIndexChanged(sender, e);
                        //}
                        Create.Visible = false;
                        CreateUser.Visible = false;
                        if (cmbRole.SelectedValue == "2" || cmbRole.SelectedValue == "5")
                        {
                            divcircle.Visible = false;
                            divstore.Visible = true;
                        }
                        else if (cmbRole.SelectedValue == "22")
                        {
                            divcircle.Visible = false;
                            divstore.Visible = false;
                        }
                        else
                        {
                            divcircle.Visible = true;
                            divstore.Visible = false;
                        }
                        Update.Visible = true;
                        UpdateUser.Visible = true;
                    }
                    string strQry = string.Empty;
                    strQry = "Title=Search and Select Office Details&";
                    strQry += " Query= select * from(select COALESCE(\"OFF_CODE\",-1) \"OFF_CODE\", ";
                    strQry += " LTRIM(SUBSTR(\"OFF_NAME\",POSITION(':' IN \"OFF_NAME\")+1,LENGTH(\"OFF_NAME\"))) AS \"OFF_NAME\" ";
                    strQry += " FROM \"VIEW_ALL_OFFICES\")A  where  {0} like %{1}% order by \"OFF_NAME\"&";
                    strQry += "DBColName=CAST(\"OFF_CODE\" AS TEXT)~CAST(\"OFF_NAME\" AS TEXT)&";
                    strQry += "ColDisplayName=OFF_CODE~OFF_NAME&";
                    strQry = strQry.Replace("'", @"\'");
                    strQry = strQry.Replace("+", @"8TT8");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   MethodBase.GetCurrentMethod().DeclaringType.Name,
                   MethodBase.GetCurrentMethod().Name,
                   ex.Message,
                   ex.StackTrace);
            }
        }

        private void LoadDropDownDetailsOnOffCode(string Orginal_OffCode = "")
        {
            string Extracted_OffCode = string.Empty;
            try
            {
                if ((Orginal_OffCode ?? "").Length == 0) // for Zone and Admin level Users
                {
                    Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone);
                }
                if (Orginal_OffCode.Length >= 1) // for Zone
                {
                    Extracted_OffCode = "";
                    Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone);
                    Extracted_OffCode = Orginal_OffCode.Substring(0, Zone_code);
                    cmbZone.Items.FindByValue(Extracted_OffCode).Selected = true;
                    cmbZone.Enabled = false;
                }
                if (Orginal_OffCode.Length >= 1)    // for Circle
                {
                    Extracted_OffCode = "";
                    Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_ZO_ID\"='"
                        + cmbZone.SelectedValue + "'", "--Select--", cmbCircle);
                    if (Orginal_OffCode.Length >= 2)
                    {
                        Extracted_OffCode = Orginal_OffCode.Substring(0, Circle_code);
                        cmbCircle.Items.FindByValue(Extracted_OffCode).Selected = true;
                        cmbCircle.Enabled = false;
                    }
                }
                if (Orginal_OffCode.Length >= 2)    // for Division
                {
                    Extracted_OffCode = "";
                    Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CICLE_CODE\" AS TEXT)='"
                        + cmbCircle.SelectedValue + "'", "--Select--", cmbDiv);
                    if (Orginal_OffCode.Length >= 3)
                    {
                        Extracted_OffCode = Orginal_OffCode.Substring(0, Division_code);
                        cmbDiv.Items.FindByValue(Extracted_OffCode).Selected = true;
                        cmbDiv.Enabled = false;
                    }
                    //Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE \"SD_DIV_CODE\"='"
                    //    + Orginal_OffCode + "'", "--Select--", cmbSubDiv);
                }
                if (Orginal_OffCode.Length >= 3)    // for SubDivision
                {
                    Extracted_OffCode = "";
                    Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE CAST(\"SD_DIV_CODE\" AS TEXT)='"
                        + cmbDiv.SelectedValue + "'", "--Select--", cmbSubDiv);
                    if (Orginal_OffCode.Length >= 4)
                    {
                        Extracted_OffCode = Orginal_OffCode.Substring(0, SubDiv_code);
                        cmbSubDiv.Items.FindByValue(Extracted_OffCode).Selected = true;
                        cmbSubDiv.Enabled = false;
                    }


                }
                if (Orginal_OffCode.Length >= 4)    // for Section
                {
                    Extracted_OffCode = "";
                    Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_SUBDIV_CODE\"='"
                        + cmbSubDiv.SelectedValue + "'", "--Select--", cmbOMSection);
                    if (Orginal_OffCode.Length >= 5)
                    {
                        Extracted_OffCode = Orginal_OffCode.Substring(0, Section_code);
                        cmbOMSection.Items.FindByValue(Extracted_OffCode).Selected = true;
                        cmbOMSection.Enabled = false;
                    }
                }
                else
                {
                    Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_SUBDIV_CODE\"='"
                     + cmbSubDiv.SelectedValue + "'", "--Select--", cmbOMSection);
                }
                Genaral.Load_Combo("SELECT \"SM_ID\",\"SM_NAME\" FROM \"TBLSTOREMAST\" ORDER BY \"SM_ID\"", "--Select--", cmbStore);

                if ((Orginal_OffCode ?? "").Length > 0)
                {
                    Genaral.Load_Combo(" SELECT \"RO_ID\",\"RO_NAME\" FROM \"TBLROLES\" WHERE	\"RO_TYPE\" != '3' ORDER BY \"RO_ID\" ", "--Select--", cmbRole);
                    Genaral.Load_Combo("SELECT DISTINCT \"DM_DESGN_ID\",\"DM_NAME\" FROM \"TBLDESIGNMAST\" inner join \"TBLROLES\" on cast(\"RO_DESIGNATION\" as int4)=\"DM_DESGN_ID\" where \"RO_TYPE\"!='3' ORDER BY \"DM_DESGN_ID\"", "--Select--", cmbDesignation);
                }
                else
                {
                    Genaral.Load_Combo("SELECT \"RO_ID\",\"RO_NAME\" FROM \"TBLROLES\" ORDER BY \"RO_ID\"", "--Select--", cmbRole);
                    Genaral.Load_Combo("SELECT \"DM_DESGN_ID\",\"DM_NAME\" FROM \"TBLDESIGNMAST\" ORDER BY \"DM_DESGN_ID\"", "--Select--", cmbDesignation);
                }

                //Genaral.Load_Combo("SELECT \"DM_DESGN_ID\",\"DM_NAME\" FROM \"TBLDESIGNMAST\" ORDER BY \"DM_DESGN_ID\"", "--Select--", cmbDesignation);
                //Genaral.Load_Combo("SELECT \"DM_DESGN_ID\",\"DM_NAME\" FROM \"TBLDESIGNMAST\" inner join \"TBLROLES\" on cast(\"RO_DESIGNATION\" as int4)=\"DM_DESGN_ID\" where \"RO_TYPE\"!='3' ORDER BY \"DM_DESGN_ID\"", "--Select--", cmbDesignation);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   MethodBase.GetCurrentMethod().DeclaringType.Name,
                   MethodBase.GetCurrentMethod().Name,
                   ex.Message,
                   ex.StackTrace);
            }
        }
        /// <summary>
        /// cmbZone_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbZone.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_ZO_ID\"='"
                        + cmbZone.SelectedValue + "'", "--Select--", cmbCircle);
                    //cmbDiv.Items.Clear();
                    //cmbSubDiv.Items.Clear();
                    //cmbOMSection.Items.Clear();
                }
                else
                {
                    cmbCircle.Items.Clear();
                    //cmbDiv.Items.Clear();
                    //cmbSubDiv.Items.Clear();
                    //cmbOMSection.Items.Clear();
                }
                cmbDiv.Items.Clear();
                cmbSubDiv.Items.Clear();
                cmbOMSection.Items.Clear();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   MethodBase.GetCurrentMethod().DeclaringType.Name,
                   MethodBase.GetCurrentMethod().Name,
                   ex.Message,
                   ex.StackTrace);
            }
        }
        /// <summary>
        /// cmbCircle_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE \"DIV_CICLE_CODE\"='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDiv);
                    //cmbSubDiv.Items.Clear();
                    //cmbOMSection.Items.Clear();
                }
                else
                {
                    cmbDiv.Items.Clear();
                    //cmbSubDiv.Items.Clear();
                    //cmbOMSection.Items.Clear();
                }
                cmbSubDiv.Items.Clear();
                cmbOMSection.Items.Clear();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   MethodBase.GetCurrentMethod().DeclaringType.Name,
                   MethodBase.GetCurrentMethod().Name,
                   ex.Message,
                   ex.StackTrace);
            }
        }
        /// <summary>
        /// cmbDiv_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDiv.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE \"SD_DIV_CODE\"='"
                        + cmbDiv.SelectedValue + "'", "--Select--", cmbSubDiv);
                    //cmbOMSection.Items.Clear();
                }
                else
                {
                    cmbSubDiv.Items.Clear();
                    //cmbOMSection.Items.Clear();
                }
                cmbOMSection.Items.Clear();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   MethodBase.GetCurrentMethod().DeclaringType.Name,
                   MethodBase.GetCurrentMethod().Name,
                   ex.Message,
                   ex.StackTrace);
            }
        }
        /// <summary>
        /// cmbSubDiv_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbSubDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubDiv.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_SUBDIV_CODE\"='"
                        + cmbSubDiv.SelectedValue + "'", "--Select--", cmbOMSection);
                }
                else
                {
                    cmbOMSection.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   MethodBase.GetCurrentMethod().DeclaringType.Name,
                   MethodBase.GetCurrentMethod().Name,
                   ex.Message,
                   ex.StackTrace);
            }
        }
        /// <summary>
        /// ValidateForm
        /// </summary>
        /// <returns></returns>
        bool ValidateForm()
        {
            bool bValidate = false;
            try
            {
                clsUser objuser = new clsUser();

                string selected_RoleType = objuser.GetRoleType(cmbRole.SelectedValue);

                if (txtFullName.Text.Trim().Length == 0)
                {
                    txtFullName.Focus();
                    ShowMsgBox("Please Enter Full Name");
                    return bValidate;
                }
                if (txtFullName.Text.Trim().StartsWith("."))
                {
                    txtFullName.Focus();
                    ShowMsgBox("Full Name not Start with DOT(.)");
                    return bValidate;
                }
                if (txtLoginName.Text.Trim().Length == 0)
                {
                    txtLoginName.Focus();
                    ShowMsgBox("Please Enter Login Name");
                    return bValidate;
                }
                if (txtLoginName.Text.Trim().StartsWith("."))
                {
                    txtLoginName.Focus();
                    ShowMsgBox("Login Name not Start with DOT(.)");
                    return bValidate;
                }
                if (cmbRole.SelectedValue == "2" || cmbRole.SelectedValue == "5")
                {
                    if (cmbStore.SelectedIndex == 0)
                    {
                        ShowMsgBox("Please Select Store");
                        return bValidate;
                    }
                }
                else if (selected_RoleType == Constants.Roles.OfficeLevel)
                {
                    if (cmbZone.SelectedIndex == 0 && cmbCircle.SelectedIndex == -1 && cmbDiv.SelectedIndex == -1
                        && cmbSubDiv.SelectedIndex == -1 && cmbOMSection.SelectedIndex == -1)
                    {
                        if (selected_RoleType != Constants.Roles.AdminLevel)
                        {
                            ShowMsgBox("Please Select Zone");
                            return bValidate;
                        }
                    }
                }
                if (txtEmailId.Text.Trim() == "")
                {
                    txtEmailId.Focus();
                    ShowMsgBox("Please Enter Email Id.");
                    return bValidate;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmailId.Text,
                    "^\\s*[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+[a-zA-Z0-9]\\s*$"))
                {
                    txtEmailId.Focus();
                    ShowMsgBox("Please Enter Valid Email (xyz@aaa.com)");
                    return bValidate;
                }
                if (txtMobile.Text.Trim().Length == 0)
                {
                    txtMobile.Focus();
                    ShowMsgBox("Please Enter Mobile Number");
                    return bValidate;
                }
                if (txtMobile.Text.Length < 10)
                {
                    ShowMsgBox("Please Enter Valid Mobile Number");
                    txtMobile.Focus();
                    return bValidate;
                }
                if (txtMobile.Text[0] == '0' || txtMobile.Text[0] == '1' || txtMobile.Text[0] == '2')
                {
                    ShowMsgBox("Please Enter Valid Mobile Number");
                    txtMobile.Focus();
                    return false;
                }
                if (txtPhone.Text.Trim().Length != 0)
                {
                    if ((txtPhone.Text.Length - txtPhone.Text.Replace("-", "").Length) >= 2)
                    {
                        txtPhone.Focus();
                        ShowMsgBox("Please Enter Valid Phone Number");
                        return bValidate;
                    }
                    if (txtPhone.Text.Contains(".") == true)
                    {
                        txtPhone.Focus();
                        ShowMsgBox("Please Enter Valid Phone Number");
                        return bValidate;
                    }
                }
                if (cmbRole.SelectedIndex == 0)
                {
                    cmbRole.Focus();
                    ShowMsgBox("Please Select Role");
                    return bValidate;
                }
                if (cmbDesignation.SelectedIndex == 0)
                {
                    cmbDesignation.Focus();
                    ShowMsgBox("Please Select Designation.");
                    return bValidate;
                }
                if (txtuserID.Text != "") { }
                else if (txtuserID.Text == "")
                {
                    if (txtPassword.Text.Length == 0)
                    {
                        txtPassword.Focus();
                        ShowMsgBox("Please Enter Password.");
                        return bValidate;
                    }
                }
                if (txtAddress.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Please Enter Address");
                    txtAddress.Focus();
                    return bValidate;
                }
                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
                return bValidate;
            }
        }
        /// <summary>
        /// cmdClose_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdClose_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("UserView.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }
        /// <summary>
        /// cmdSave_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool bAccResult;
                if (cmdSave.Text == Constants.ButtonClass.ButtenUpdate)
                {
                    bAccResult = CheckAccessRights(Constants.CheckAccessRights.CheckAccessRightsModify);    //Check AccessRights
                }
                else
                {
                    bAccResult = CheckAccessRights(Constants.CheckAccessRights.CheckAccessRightsCreate);    //Check AccessRights
                }
                if (bAccResult == false)
                {
                    return;
                }
                if (ValidateForm() == true)
                {
                    clsUser objUser = new clsUser();
                    string[] Arr = new string[2];
                    Byte[] Buffer;
                    string strofficecode = GetOfficeID();
                    objUser.lSlNo = txtuserID.Text;
                    objUser.sOfficeCode = strofficecode;
                    objUser.sFullName = txtFullName.Text;
                    objUser.sLoginName = txtLoginName.Text;
                    objUser.sPassword = txtPassword.Text;
                    objUser.sRole = cmbRole.SelectedValue;
                    if (objUser.sRole == "2" || objUser.sRole == "5")
                    {
                        objUser.sOfficeCode = cmbStore.SelectedValue;
                    }
                    if (objUser.sRole == "22")
                    {
                        objUser.sOfficeCode = "";
                    }
                    objUser.sEmail = txtEmailId.Text.ToLower();
                    objUser.sMobileNo = txtMobile.Text;
                    objUser.sPhoneNo = txtPhone.Text;
                    objUser.sAddress = txtAddress.Text;
                    objUser.sCrby = objSession.UserId;
                    objUser.sDesignation = cmbDesignation.SelectedValue;
                    if (fupSign.PostedFile.ContentLength != 0)
                    {
                        string filename = Path.GetFileName(fupSign.PostedFile.FileName);
                        string strExt = filename.Substring(filename.LastIndexOf('.') + 1);
                        if (strExt.ToLower().Equals("jpg") || strExt.ToLower().Equals("jpeg")
                            || strExt.ToLower().Equals("png") || strExt.ToLower().Equals("gif"))
                        {
                            Stream strm = fupSign.PostedFile.InputStream;
                            Buffer = new byte[strm.Length];
                            strm.Read(Buffer, 0, (int)strm.Length);
                            objUser.sSignImage = Buffer;
                        }
                        else
                        {
                            lblMessage.Text = "Invalid Image File";
                            return;
                        }
                    }
                    else
                    {
                        Stream strm = fupSign.PostedFile.InputStream;
                        Buffer = new byte[strm.Length];
                        strm.Read(Buffer, 0, (int)strm.Length);
                        objUser.sSignImage = Buffer;
                    }
                    Arr = objUser.SaveUpdateUserDetails(objUser);
                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "User Create Master");
                    }
                    if (Arr[1].ToString() == "0")
                    {
                        txtuserID.Text = Arr[0].ToString();
                        cmdSave.Text = Constants.ButtonClass.ButtenUpdate; //"Update"
                        string message = "Saved Successfully";
                        string url = "../MasterForms/UserView.aspx";
                        string script = "window.onload = function(){ alert('";
                        script += message;
                        script += "');";
                        script += "window.location = '";
                        script += url;
                        script += "'; }";
                        ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
                        return;
                    }
                    if (Arr[1].ToString() == "1")
                    {
                        cmdSave.Text = Constants.ButtonClass.ButtenUpdate; //"Update"
                        txtuserID.Text = Convert.ToString(objUser.lSlNo);
                        string message = Arr[0];
                        string url = "../MasterForms/UserView.aspx";
                        string script = "window.onload = function(){ alert('";
                        script += message;
                        script += "');";
                        script += "window.location = '";
                        script += url;
                        script += "'; }";
                        ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
                        return;
                    }
                    if (Arr[1].ToString() == "4")
                    {
                        ShowMsgBox(Arr[0]);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   MethodBase.GetCurrentMethod().DeclaringType.Name,
                   MethodBase.GetCurrentMethod().Name,
                   ex.Message,
                   ex.StackTrace);
            }
        }
        /// <summary>
        /// GetOfficeID
        /// </summary>
        /// <returns></returns>
        public string GetOfficeID()
        {
            string strOfficeId = string.Empty;
            if (cmbZone.SelectedIndex > 0)
            {
                strOfficeId = cmbZone.SelectedValue.ToString();
            }
            if (cmbCircle.SelectedIndex > 0)
            {
                strOfficeId = cmbCircle.SelectedValue.ToString();
            }
            if (cmbDiv.SelectedIndex > 0)
            {
                strOfficeId = cmbDiv.SelectedValue.ToString();
            }
            if (cmbSubDiv.SelectedIndex > 0)
            {
                strOfficeId = cmbSubDiv.SelectedValue.ToString();
            }
            if (cmbOMSection.SelectedIndex > 0)
            {
                strOfficeId = cmbOMSection.SelectedValue.ToString();
            }
            return (strOfficeId);
        }
        /// <summary>
        /// cmdReset_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdReset_Click(object sender, EventArgs e)
        {
            Reset();
        }
        /// <summary>
        /// ShowMsgBox
        /// </summary>
        /// <param name="sMsg"></param>
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
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }
        /// <summary>
        /// Reset funtion
        /// </summary>
        public void Reset()
        {
            objSession = (clsSession)Session["clsSession"];
            try
            {
                txtFullName.Text = string.Empty;
                txtLoginName.Text = string.Empty;
                txtPassword.Text = string.Empty;
                cmbRole.SelectedIndex = 0;
                txtEmailId.Text = string.Empty;
                txtMobile.Text = string.Empty;
                txtPhone.Text = string.Empty;
                txtAddress.Text = string.Empty;
                txtuserID.Text = string.Empty;
                cmdSave.Text = Constants.ButtonClass.ButtenSave;
                cmbDesignation.SelectedIndex = 0;
                txtPassword.Attributes.Add("Value", "");
                txtPassword.Attributes.Remove("readonly");
                txtLoginName.Enabled = true;
                if (objSession.sRoleType == Constants.Roles.OfficeLevel)
                {
                    cmbSubDiv.SelectedIndex = 0;
                }
                else if (objSession.sRoleType == Constants.Roles.AdminLevel)
                {
                    cmbZone.SelectedIndex = 0;
                    cmbCircle.Items.Clear();
                    cmbDiv.Items.Clear();
                    cmbSubDiv.Items.Clear();
                }
                cmbOMSection.Items.Clear();
                cmbStore.SelectedIndex = 0;
                divstore.Visible = false;
                divcircle.Visible = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   MethodBase.GetCurrentMethod().DeclaringType.Name,
                   MethodBase.GetCurrentMethod().Name,
                   ex.Message,
                   ex.StackTrace);
            }
        }
        /// <summary>
        /// LoadUserDetails
        /// </summary>
        /// <param name="strId"></param>
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
                txtLoginName.Text = objUser.sLoginName;
                txtPassword.Text = objUser.sPassword;
                txtPassword.Visible = false;
                lblpwd.Visible = false;
                cnttxt.Visible = false;
                cmbRole.SelectedValue = objUser.sRole;
                txtEmailId.Text = objUser.sEmail;
                txtMobile.Text = objUser.sMobileNo;
                txtPhone.Text = objUser.sPhoneNo;
                txtAddress.Text = objUser.sAddress;
                cmbDesignation.SelectedValue = objUser.sDesignation;
                cmdSave.Text = Constants.ButtonClass.ButtenUpdate;
                if (objSession.OfficeCode != "")
                {
                    txtPassword.Attributes.Add("readonly", "readonly");
                    // txtLoginName.Enabled = false;
                }
                if (objUser.sRoleType == Constants.Roles.StoreLevel) //"2"
                {
                    Genaral.Load_Combo("SELECT \"SM_ID\",\"SM_NAME\" FROM \"TBLSTOREMAST\" ORDER BY \"SM_ID\"", "-Select-", cmbStore);
                    cmbStore.Items.FindByValue(objUser.sOfficeCode).Selected = true;
                }
                else if (objUser.sRoleType == Constants.Roles.OfficeLevel) //"1"
                {
                    if (objUser.sOfficeCode.Length >= 1)
                    {
                        Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone);
                        stroffCode = objUser.sOfficeCode.Substring(0, Zone_code);
                        cmbZone.Items.FindByValue(stroffCode).Selected = true;
                        stroffCode = string.Empty;
                        stroffCode = objUser.sOfficeCode;
                    }
                    if (stroffCode.Length > 1)
                    {
                        Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_ZO_ID\"='"
                            + cmbZone.SelectedValue + "'", "--Select--", cmbCircle);
                        if (stroffCode.Length >= 2)
                        {
                            stroffCode = stroffCode.Substring(0, Circle_code);
                            cmbCircle.Items.FindByValue(stroffCode).Selected = true;
                            stroffCode = string.Empty;
                            stroffCode = objUser.sOfficeCode;
                        }
                    }

                    if (stroffCode.Length > 2)
                    {
                        Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE \"DIV_CICLE_CODE\"='"
                            + cmbCircle.SelectedValue + "'", "--Select--", cmbDiv);
                        if (stroffCode.Length >= 3)
                        {
                            stroffCode = stroffCode.Substring(0, Division_code);
                            cmbDiv.Items.FindByValue(stroffCode).Selected = true;
                            stroffCode = string.Empty;
                            stroffCode = objUser.sOfficeCode;
                        }
                    }
                    if (stroffCode.Length > 3)
                    {
                        Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE \"SD_DIV_CODE\"='"
                            + cmbDiv.SelectedValue + "'", "--Select--", cmbSubDiv);
                        if (stroffCode.Length >= 4)
                        {
                            stroffCode = stroffCode.Substring(0, SubDiv_code);
                            cmbSubDiv.Items.FindByValue(stroffCode).Selected = true;
                            stroffCode = string.Empty;
                            stroffCode = objUser.sOfficeCode;
                        }
                    }
                    if (stroffCode.Length > 4)
                    {
                        Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_SUBDIV_CODE\"='"
                            + cmbSubDiv.SelectedValue + "'", "--Select--", cmbOMSection);
                        if (stroffCode.Length >= 5)
                        {
                            stroffCode = stroffCode.Substring(0, Section_code);
                            cmbOMSection.Items.FindByValue(stroffCode).Selected = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }
        /// <summary>
        /// RetainImageOnPostback
        /// </summary>
        public void RetainImageOnPostback()
        {
            try
            {
                string sDirectory = string.Empty;
                string sSSPlateFileName = string.Empty;
                string sNamePlateFileName = string.Empty;
                if (fupSign.HasFile)
                {
                    sSSPlateFileName = Path.GetFileName(fupSign.PostedFile.FileName);
                    fupSign.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sSSPlateFileName));
                    txtSignImagePath.Text = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sSSPlateFileName);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }

        #region Access Rights
        /// <summary>
        /// CheckAccessRights
        /// </summary>
        /// <param name="sAccessType"></param>
        /// <returns></returns>
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY
                clsApproval objApproval = new clsApproval();
                objApproval.sFormName = "UserCreate";
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sAccessType = Constants.CheckAccessRights.CheckAccessRightsAll + "," + sAccessType;
                bool bResult = objApproval.CheckAccessRights(objApproval);
                if (bResult == false)
                {
                    if (sAccessType == Constants.CheckAccessRights.CheckAccessRightsReadOnly)
                    {
                        Response.Redirect("~/UserRestrict.aspx", false);
                    }
                    else
                    {
                        ShowMsgBox(Convert.ToString(ConfigurationManager.AppSettings["AccessRightsIfDenied"]));
                    }
                }
                return bResult;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   MethodBase.GetCurrentMethod().DeclaringType.Name,
                   MethodBase.GetCurrentMethod().Name,
                   ex.Message,
                   ex.StackTrace);
                return false;
            }
        }
        #endregion
        /// <summary>
        /// cmbRole_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                clsUser objuser = new clsUser();
                objSession = (clsSession)Session["clsSession"];

                string sRoleType = objuser.GetRoleType(cmbRole.SelectedValue);
                //STO OR SK
                if (cmbRole.SelectedValue == "2" || cmbRole.SelectedValue == "5")
                {
                    if (objSession.sRoleType == Constants.Roles.OfficeLevel) //"1"
                    {
                        string StoreID = clsStoreOffice.GetStoreIDs(cmbDiv.SelectedValue); // to get div code based on store id ;
                        Genaral.Load_Combo("SELECT \"SM_ID\",\"SM_NAME\" FROM \"TBLSTOREMAST\" where  \"SM_CODE\" = '"
                            + cmbDiv.SelectedValue + "'ORDER BY \"SM_ID\"", "-Select-", cmbStore);
                        cmbStore.Items.FindByValue(StoreID).Selected = true;
                        cmbStore.Enabled = false;
                    }
                    divcircle.Visible = false;
                    divstore.Visible = true;
                }
                else if (sRoleType == "3")
                {
                    divcircle.Visible = false;
                    divstore.Visible = false;
                }
                else
                {
                    divcircle.Visible = true;
                    divstore.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }
    }
}