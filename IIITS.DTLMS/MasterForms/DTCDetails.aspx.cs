using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;

namespace IIITS.DTLMS.MasterForms
{
    public partial class DTCDetails : System.Web.UI.Page
    {
        string strFormCode = "DTCDetails";
        clsSession objSession;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
            {
                Response.Redirect("~/Login.aspx", false);
            }

            objSession = (clsSession)Session["clsSession"];
            lblMessage.Text = string.Empty;
            if (!IsPostBack)
            {
                Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'PT' ORDER BY \"MD_ORDER_BY\" ", "--Select--", cmbprojecttype);
                Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'LT' ORDER BY \"MD_ORDER_BY\" ", "--Select--", cmbLoadtype);
                Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'PLT' ORDER BY \"MD_ORDER_BY\" ", "--Select--", cmbPlatformType);
                //  Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'LTP' ORDER BY \"MD_ORDER_BY\" ", "--Select--", ddlLTProtection);
                // Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'HTP' ORDER BY \"MD_ORDER_BY\" ", "--Select--", ddlhtprotection);

                if (Request.QueryString["QryDtcId"] != null && Request.QueryString["QryDtcId"].ToString() != "")
                {
                    txtDTCId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryDtcId"]));
                }

                if (txtDTCId.Text != "")
                {
                    LoadDtcDetails(txtDTCId.Text);
                }
            }
        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                cmbPlatformType.SelectedIndex = 0;
                cmdSave.Text = "Update";
                ddlArresters.SelectedIndex = 0;
                ddldtcmeters.SelectedIndex = 0;
                ddlgrounding.SelectedIndex = 0;
                ddlhtprotection.SelectedIndex = 0;
                ddlLTProtection.SelectedIndex = 0;
                txthtLine.Text = string.Empty;
                txtltLine.Text = string.Empty;
                cmbprojecttype.SelectedIndex = 0;
                cmbPlatformType.SelectedIndex = 0;
                txtLatitude.Text = string.Empty;
                txtlongitude.Text = string.Empty;
                cmbLoadtype.SelectedIndex = 0;
                txtDepreciation.Text = string.Empty;
                cmbGOS.SelectedIndex = 0;
                cmbRAPDRP.SelectedIndex = 0;

                txtCircute1.Text = string.Empty;
                txtCircute2.Text = string.Empty;
                txtCircute3.Text = string.Empty;
                txtCircute4.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            clsDTCCommision objDtcCommision = new clsDTCCommision();
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
                if (txtltLine.Text != "")
                {
                    string Val = txtltLine.Text.Trim();
                    string afterDotVal = Val.Contains('.') ? Val.Split('.')[1] : Val;
                    bool ReturnValue = false;
                    if (Val.Length > 3 && Val.Length <= 7 && !Val.Contains('.'))
                    {
                        txtltLine.Focus();
                        ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                        return;
                    }
                    else if (Val.IndexOf('.') == 0 || afterDotVal.Length == 0)
                    {
                        txtltLine.Focus();
                        ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                        return;
                    }
                    else if (!(Val.IndexOf('.') <= 3))
                    {
                        txtltLine.Focus();
                        ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                        return;
                    }
                    else if (Val.Contains('.') && afterDotVal.Length > 3)
                    {
                        txtltLine.Focus();
                        ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                        return;
                    }
                    else if (Val.Length > 0 && Val.Length <= 3 && !(Val.Contains('.')))
                    {
                        switch (Val)
                        {
                            case "00":
                                ReturnValue = true;
                                break;
                            case "000":
                                ReturnValue = true;
                                break;
                        }
                        if (ReturnValue)
                        {
                            txtltLine.Focus();
                            ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                            return;
                        }
                    }
                    else if (Val.Length > 0 && Val.Contains('.') && Val.Length <= 7)
                    {
                        switch (Val)
                        {
                            case "0.":
                                ReturnValue = true;
                                break;
                            case "00.":
                                ReturnValue = true;
                                break;
                            case "000.":
                                ReturnValue = true;
                                break;
                            case ".":
                                ReturnValue = true;
                                break;
                            case ".0":
                                ReturnValue = true;
                                break;
                            case ".00":
                                ReturnValue = true;
                                break;
                            case ".000":
                                ReturnValue = true;
                                break;
                            case "0.00":
                                ReturnValue = true;
                                break;
                            case "0.000":
                                ReturnValue = true;
                                break;
                            case "00.0":
                                ReturnValue = true;
                                break;
                            case "00.00":
                                ReturnValue = true;
                                break;
                            case "00.000":
                                ReturnValue = true;
                                break;
                            case "000.0":
                                ReturnValue = true;
                                break;
                            case "000.00":
                                ReturnValue = true;
                                break;
                            case "000.000":
                                ReturnValue = true;
                                break;
                        }
                        if (ReturnValue)
                        {
                            txtltLine.Focus();
                            ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                            return;
                        }
                    }
                    //if (txtltLine.Text.Trim().Length != 0)
                    //{
                    //    double val = Convert.ToDouble(txtltLine.Text);
                    //    if (val < 0.0 || val > 999.999)
                    //    {
                    //        txtltLine.Focus();
                    //        ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                    //        return;
                    //    }
                    //    if (txtltLine.Text == ".")
                    //    {
                    //        ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                    //        return;
                    //    }
                    //}

                    //if (txtltLine.Text != "0")
                    //{
                    //    if (txtltLine.Text.Contains('.'))
                    //    {
                    //        int ltindex = txtltLine.Text.IndexOf('.');
                    //        string ltstr = txtltLine.Text.Substring(ltindex + 1);
                    //        int val = Convert.ToInt16(ltstr);
                    //        if (val == 0)
                    //        {
                    //            string lltstr = txtltLine.Text.Substring(0, ltindex);

                    //            if (Convert.ToInt16(lltstr) == 0)
                    //            {
                    //                ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                    //                return;
                    //            }
                    //        }
                    //        string lltstr1 = txtltLine.Text.Substring(0, ltindex);
                    //        if (lltstr1.Length > 3)
                    //        {
                    //            ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                    //            return;
                    //        }
                    //    }
                    //}
                    //if (txtltLine.Text.Contains('.'))
                    //{
                    //    string LTPattern = @"[0-9]{0,3}^\d+\.\d{0,3}$";
                    //    System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(LTPattern);
                    //    if (!r.IsMatch(txtltLine.Text))
                    //    {
                    //        ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                    //        return;
                    //    }
                    //}
                }
                if (txthtLine.Text != "")
                {
                    string Val = txthtLine.Text.Trim();
                    string afterDotVal = Val.Contains('.') ? Val.Split('.')[1] : Val;
                    bool ReturnValue = false;
                    if (Val.Length > 3 && Val.Length <= 7 && !Val.Contains('.'))
                    {
                        txthtLine.Focus();
                        ShowMsgBox("Please Enter a valid HT line Length (Eg: 111.111)");
                        return;
                    }
                    else if (!(Val.IndexOf('.') <= 3))
                    {
                        txthtLine.Focus();
                        ShowMsgBox("Please Enter a valid HT line Length (Eg: 111.111)");
                        return;
                    }
                    else if (Val.IndexOf('.') == 0 || afterDotVal.Length == 0)
                    {
                        txthtLine.Focus();
                        ShowMsgBox("Please Enter a valid HT line Length (Eg: 111.111)");
                        return;
                    }
                    else if (Val.Contains('.') && afterDotVal.Length > 3)
                    {
                        txthtLine.Focus();
                        ShowMsgBox("Please Enter a valid HT line Length (Eg: 111.111)");
                        return;
                    }
                    else if (Val.Length > 0 && Val.Length <= 3 && !(Val.Contains('.')))
                    {
                        switch (Val)
                        {
                            case "00":
                                ReturnValue = true;
                                break;
                            case "000":
                                ReturnValue = true;
                                break;
                        }
                        if (ReturnValue)
                        {
                            txthtLine.Focus();
                            ShowMsgBox("Please Enter a valid HT line Length (Eg: 111.111)");
                            return;
                        }
                    }
                    else if (Val.Length > 0 && Val.Contains('.') && Val.Length <= 7)
                    {
                        switch (Val)
                        {
                            case "0.":
                                ReturnValue = true;
                                break;
                            case "00.":
                                ReturnValue = true;
                                break;
                            case "000.":
                                ReturnValue = true;
                                break;
                            case ".":
                                ReturnValue = true;
                                break;
                            case ".0":
                                ReturnValue = true;
                                break;
                            case ".00":
                                ReturnValue = true;
                                break;
                            case ".000":
                                ReturnValue = true;
                                break;
                            case "0.00":
                                ReturnValue = true;
                                break;
                            case "0.000":
                                ReturnValue = true;
                                break;
                            case "00.0":
                                ReturnValue = true;
                                break;
                            case "00.00":
                                ReturnValue = true;
                                break;
                            case "00.000":
                                ReturnValue = true;
                                break;
                            case "000.0":
                                ReturnValue = true;
                                break;
                            case "000.00":
                                ReturnValue = true;
                                break;
                            case "000.000":
                                ReturnValue = true;
                                break;
                        }
                        if (ReturnValue)
                        {
                            txthtLine.Focus();
                            ShowMsgBox("Please Enter a valid HT line Length (Eg: 111.111)");
                            return;
                        }
                    }
                    //if (txthtLine.Text.Trim().Length != 0)
                    //{
                    //    double val = Convert.ToDouble(txthtLine.Text);
                    //    if (val < 0.0 || val > 999.999)
                    //    {
                    //        txthtLine.Focus();
                    //        ShowMsgBox("Please Enter a valid HT line Length (Eg: 111.111)");
                    //        return;
                    //    }
                    //    if (txthtLine.Text == ".")
                    //    {
                    //        ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                    //        return;
                    //    }
                    //}
                    //if (txthtLine.Text != "0")
                    //{
                    //    if (txthtLine.Text.Contains('.'))
                    //    {
                    //        int ltindex = txthtLine.Text.IndexOf('.');
                    //        string ltstr = txthtLine.Text.Substring(ltindex + 1);
                    //        int val = Convert.ToInt16(ltstr);
                    //        if (val == 0)
                    //        {
                    //            string lltstr = txthtLine.Text.Substring(0, ltindex);

                    //            if (Convert.ToInt16(lltstr) == 0)
                    //            {
                    //                ShowMsgBox("Please Enter a valid HT line Length (Eg: 111.111)");
                    //                return;
                    //            }
                    //        }
                    //        string lltstr1 = txthtLine.Text.Substring(0, ltindex);
                    //        if (lltstr1.Length > 3)
                    //        {
                    //            ShowMsgBox("Please Enter a valid LT line Length (Eg: 111.111)");
                    //            return;
                    //        }
                    //    }
                    //}
                    //if (txthtLine.Text.Contains('.'))
                    //{
                    //    string HTPattern = @"[0-9]{0,3}^\d+\.\d{0,3}$";
                    //    System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(HTPattern);
                    //    if (!r.IsMatch(txthtLine.Text))
                    //    {
                    //        ShowMsgBox("Please Enter a valid HT line Length (Eg: 111.111)");
                    //        return;
                    //    }
                    //}
                }
                string[] Arr = new string[3];
                objDtcCommision.lDtcId = Convert.ToString(txtDTCId.Text);
                objDtcCommision.sCrBy = objSession.UserId;
                objDtcCommision.sHtlinelength = txthtLine.Text;
                objDtcCommision.sLtlinelength = txtltLine.Text;

                if (cmbPlatformType.SelectedIndex > 0)
                {
                    objDtcCommision.sPlatformType = cmbPlatformType.SelectedValue;
                }
                else
                {
                    objDtcCommision.sPlatformType = "0";
                }
                if (ddlArresters.SelectedIndex > 0)
                {
                    objDtcCommision.sArresters = ddlArresters.SelectedValue;
                }
                else
                {
                    objDtcCommision.sArresters = "0";
                }
                if (ddlgrounding.SelectedIndex > 0)
                {
                    objDtcCommision.sGrounding = ddlgrounding.SelectedValue;
                }
                else
                {
                    objDtcCommision.sGrounding = "0";
                }
                if (ddlLTProtection.SelectedIndex > 0)
                {
                    objDtcCommision.sLTProtect = ddlLTProtection.SelectedValue;
                }
                else
                {
                    objDtcCommision.sLTProtect = "0";
                }
                if (ddlhtprotection.SelectedIndex > 0)
                {
                    objDtcCommision.sHTProtect = ddlhtprotection.SelectedValue;
                }
                else
                {
                    objDtcCommision.sHTProtect = "0";
                }
                if (ddldtcmeters.SelectedIndex > 0)
                {
                    objDtcCommision.sDTCMeters = ddldtcmeters.SelectedValue;
                }
                else
                {
                    objDtcCommision.sDTCMeters = "0";
                }
                if (cmbLoadtype.SelectedIndex > 0)
                {
                    objDtcCommision.sLoadtype = cmbLoadtype.SelectedValue;
                }
                else
                {
                    objDtcCommision.sLoadtype = "0";
                }
                if (cmbGOS.SelectedIndex > 0)
                {
                    objDtcCommision.sGOS = cmbGOS.SelectedValue;
                }
                else
                {
                    objDtcCommision.sGOS = "0";
                }
                if (cmbRAPDRP.SelectedIndex > 0)
                {
                    objDtcCommision.sRAPDRP = cmbRAPDRP.SelectedValue;
                }
                else
                {
                    objDtcCommision.sRAPDRP = "0";
                }
                if (txthtLine.Text == "")
                {
                    objDtcCommision.sHtlinelength = "0";
                }
                else
                {
                    objDtcCommision.sHtlinelength = txthtLine.Text;
                }
                if (txtDepreciation.Text == "")
                {
                    objDtcCommision.sDepreciation = "0";
                }
                else
                {
                    objDtcCommision.sDepreciation = txtDepreciation.Text.Trim();
                }
                if (txtCircute1.Text == "")
                {
                    objDtcCommision.sCircuit1 = "0";
                }
                else
                {
                    objDtcCommision.sCircuit1 = txtCircute1.Text;
                }
                if (txtCircute2.Text == "")
                {
                    objDtcCommision.Circuit2 = "0";
                }
                else
                {
                    objDtcCommision.Circuit2 = txtCircute2.Text;
                }
                if (txtCircute3.Text == "")
                {
                    objDtcCommision.Circuit3 = "0";
                }
                else
                {
                    objDtcCommision.Circuit3 = txtCircute3.Text;
                }
                if (txtCircute4.Text == "")
                {
                    objDtcCommision.Circuit4 = "0";
                }
                else
                {
                    objDtcCommision.Circuit4 = txtCircute4.Text;
                }
                objDtcCommision.sLongitude = txtlongitude.Text;
                objDtcCommision.sLatitude = txtLatitude.Text;

                Arr = objDtcCommision.SaveUpdateDtcSpecification(objDtcCommision);

                if (objSession.sTransactionLog == "1")
                {
                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " Dtc Details Master");
                }

                if (Arr[1].ToString() == "0")
                {
                    ShowMsgBox(Arr[0]);
                    txtDTCId.Text = objDtcCommision.lDtcId;
                    cmdSave.Text = "Update";
                    return;
                }

                if (Arr[1].ToString() == "1")
                {
                    ShowMsgBox(Arr[0]);
                    cmdSave.Text = "Update";
                    GenerateCommissionReport(objDtcCommision);
                    return;
                }

                if (Arr[1].ToString() == "2")
                {
                    ShowMsgBox(Arr[0]);
                    return;
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void GenerateCommissionReport(clsDTCCommision objcommision)
        {
            try
            {
                string sDTCId = txtDTCCode.Text.ToString();
                string strParam = string.Empty;
                strParam = "id=NewDtcCR_CommReport&DtcID=" + sDTCId + "&OffCode=" + objSession.OfficeCode;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                return;


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

        public void LoadDtcDetails(string strId)
        {
            try
            {
                clsDTCCommision objDtcCommision = new clsDTCCommision();
                objDtcCommision.lDtcId = Convert.ToString(strId);

                objDtcCommision.GetDTCDetails(objDtcCommision);

                txtDTCCode.Text = Convert.ToString(objDtcCommision.sDtcCode);
                txtDTCName.Text = objDtcCommision.sDtcName;
                txtDTCId.Text = Convert.ToString(objDtcCommision.lDtcId);
                cmbPlatformType.SelectedValue = objDtcCommision.sPlatformType;
                ddlArresters.SelectedValue = objDtcCommision.sArresters;
                ddldtcmeters.SelectedValue = objDtcCommision.sDTCMeters;
                ddlgrounding.SelectedValue = objDtcCommision.sGrounding;
                ddlhtprotection.SelectedValue = objDtcCommision.sHTProtect;
                ddlLTProtection.SelectedValue = objDtcCommision.sLTProtect;
                txtltLine.Text = objDtcCommision.sLtlinelength;
                txthtLine.Text = objDtcCommision.sHtlinelength;
                cmbprojecttype.SelectedValue = objDtcCommision.sProjecttype;
                cmbLoadtype.SelectedValue = objDtcCommision.sLoadtype;
                txtLatitude.Text = objDtcCommision.sLatitude;
                txtlongitude.Text = objDtcCommision.sLongitude;
                txtDepreciation.Text = objDtcCommision.sDepreciation;
                cmbGOS.SelectedValue = objDtcCommision.sGOS;
                cmbRAPDRP.SelectedValue = objDtcCommision.sRAPDRP;
                txtCircute1.Text = objDtcCommision.sCircuit1;
                txtCircute2.Text = objDtcCommision.Circuit2;
                txtCircute3.Text = objDtcCommision.Circuit3;
                txtCircute4.Text = objDtcCommision.Circuit4;

                if (Request.QueryString["Ref"] != null && Request.QueryString["Ref"].ToString() != "")
                {
                    cmdSave.Text = "Update";
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbBack_Click(object sender, EventArgs e)
        {
            try
            {
                string strDtcId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtDTCId.Text));
                Response.Redirect("DTCCommision.aspx?QryDtcId=" + strDtcId + "", false);
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

                objApproval.sFormName = "DTCCommision";
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