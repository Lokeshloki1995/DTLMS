using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IIITS.DTLMS.BL;
using System.Configuration;

namespace IIITS.DTLMS.DtcMissMatch
{
    public partial class DTRMissmatchEntry : System.Web.UI.Page
    {
        string strFormCode = "DTRMissmatchEntry";
        clsSession objSession;
        string sRemark;
        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["clsSession"] ?? "").Length == 0)
            {
                Response.Redirect("~/Login.aspx", false);
            }
            objSession = (clsSession)Session["clsSession"];
            try
            {
                if (!IsPostBack)
                {
                    CheckAccessRights("1");
                    string qry = "SELECT \"STO_OFF_CODE\",\"STO_OFF_CODE\" || '-' || \"SM_NAME\" FROM \"TBLSTOREMAST\" ";
                    qry += " ,\"TBLSTOREOFFCODE\" WHERE \"SM_ID\" = \"STO_SM_ID\" ORDER BY \"STO_OFF_CODE\"";
                    Genaral.Load_Combo(qry, "--Select--", cmbStore);
                    LoadSearchWindow();
                    sRemark = ConfigurationManager.AppSettings["MissMatchRemarks"].ToString();
                    txtremarks.Text = sRemark;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
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
                #region Note: // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY
                clsApproval objApproval = new clsApproval();
                objApproval.sFormName = "DTRUnAllocate";
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sAccessType = Constants.CheckAccessRights.CheckAccessRightsAll + "," + sAccessType;
                bool bResult = objApproval.CheckAccessRights(objApproval);
                if (UserValid() == false)
                {
                    if (bResult == true)
                    {
                        Response.Redirect("~/UserRestrict.aspx", false);
                        bResult = false;
                    }
                }
                #endregion
                return bResult;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }
        /// <summary>
        /// UserValid
        /// </summary>
        /// <returns></returns>
        public bool UserValid()
        {
            bool res = true;
            try
            {
                string Roleid = Convert.ToString(ConfigurationManager.AppSettings["SELECTEDUSER1"]);
                string[] sRoleid = Roleid.Split(',');
                for (int i = 0; i < sRoleid.Length; i++)
                {
                    if (objSession.RoleId != sRoleid[i])
                    {
                        res = false;
                    }
                    else
                    {
                        res = true;
                        return res;
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }

        #endregion
        /// <summary>
        /// LoadSearchWindow
        /// </summary>
        public void LoadSearchWindow()
        {
            try
            {
                string strQry = string.Empty;
                strQry = "Title=Search and Select DTr Details&";
                strQry += "Query=SELECT \"TC_CODE\" as \"DTr Code\",\"TC_CAPACITY\" as \"DTr Capacity\" FROM \"TBLTCMASTER\"";
                strQry += " WHERE  CAST(\"TC_CODE\" AS TEXT) LIKE '" + txtDtrCode.Text + "%' ";
                strQry += " AND {0} like %{1}% LIMIT 50 &";
                strQry += "DBColName=CAST(\"TC_CODE\" AS TEXT)~CAST(\"TC_CAPACITY\" AS TEXT)&";
                strQry += "ColDisplayName=DTr Code~DTr Capacity&";
                strQry = strQry.Replace("'", @"\'");
                btnDtrSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry +
                    "tb=" + txtDtrCode.ClientID + "&btn=" + btnDtrSearch.ClientID + "',520,520," + txtDtrCode.ClientID + ")");
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// Navigate to clsDtcMissMatchEntry.cs form for getting dtr details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDtrSearch_Click(object sender, EventArgs e)
        {
            try
            {
                clsDtcMissMatchEntry objDtcMissMatch = new clsDtcMissMatchEntry();
                objDtcMissMatch.sDtrCode = txtDtrCode.Text;
                DataTable dt = new DataTable();
                dt = objDtcMissMatch.LoadDtrDetails1(objDtcMissMatch);
                if (dt.Rows.Count > 0)
                {
                    hdfNewLocType.Value = dt.Rows[0]["TC_CURRENT_LOCATION"].ToString();
                    hdfNewOffCode.Value = dt.Rows[0]["TC_LOCATION_ID"].ToString();
                    if ((hdfNewOffCode.Value).Length == 2)
                    {
                        if (cmbStore.SelectedValue == hdfNewOffCode.Value)
                        {
                            ShowMsgBox("DTr Already in Store");
                            cmdReset_Click(sender, e);
                            return;
                        }
                    }
                    else
                    {
                        if (dt.Rows[0]["TC_UPDATED_EVENT"].ToString() == "Drawn" && dt.Rows[0]["DF_REPLACE_FLAG"].ToString() == "0")
                        {
                            ShowMsgBox("DTr Already Given for Invoice and Waiting to Decommission");
                            cmdReset_Click(sender, e);
                        }
                        if (dt.Rows[0]["TC_UPDATED_EVENT"].ToString() == "Drawn" && dt.Rows[0]["DF_REPLACE_FLAG"].ToString() == "1")
                        {
                            ShowMsgBox("DTr Invoiced it's in Field Now");
                        }
                    }
                }
                else
                {
                    ShowMsgBox("For Entered DTr Code not Have Records");
                    txtDtrCode.Focus();
                    return;
                }
                grdDtrDetails.DataSource = dt;
                grdDtrDetails.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Navigate to clsDtcMissMatchEntry.cs form for dtr moved to store
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (validation() == true)
                {
                    string[] Arr = new string[2];
                    clsDtcMissMatchEntry objDtcMissMatch = new clsDtcMissMatchEntry();
                    string sRemark = ConfigurationManager.AppSettings["MissMatchRemarks"].ToString();
                    objDtcMissMatch.sDtrCode = txtDtrCode.Text;
                    objDtcMissMatch.sStoreId = cmbStore.SelectedValue;
                    objDtcMissMatch.sCrBy = objSession.UserId;
                    objDtcMissMatch.sRemarks = txtremarks.Text;
                    objDtcMissMatch.sDtrStatus = cmbStatus.SelectedValue;
                    Arr = objDtcMissMatch.SendTOStore(objDtcMissMatch);
                    if (Arr[0] == "1")
                    {
                        ShowMsgBox(Arr[1]);
                        txtremarks.Text = ConfigurationManager.AppSettings["MissMatchRemarks"].ToString();
                    }
                    else
                        ShowMsgBox(Arr[1]);

                    cmdReset_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// validation
        /// </summary>
        /// <returns></returns>
        public bool validation()
        {
            bool bValidate = false;
            try
            {
                if (txtDtrCode.Text.Trim() == "")
                {
                    txtDtrCode.Focus();
                    ShowMsgBox("Please Enter the DTr Code");
                    return bValidate;
                }
                if (cmbStore.Text == "--Select--")
                {
                    cmbStore.Focus();
                    ShowMsgBox("Select Store");
                    return bValidate;
                }
                if (cmbStatus.SelectedValue == "0")
                {
                    cmbStatus.Focus();
                    ShowMsgBox("Select Condition of the DTr ");
                    return bValidate;
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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// cmdReset_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtDtrCode.Text = string.Empty;
                txtremarks.Text = ConfigurationManager.AppSettings["MissMatchRemarks"].ToString();
                cmbStore.SelectedIndex = 0;
                cmbStatus.SelectedValue = "0";
                grdDtrDetails.DataSource = null;
                grdDtrDetails.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}