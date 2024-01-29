using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IIITS.DTLMS.BL;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;

namespace IIITS.DTLMS.DtcMissMatch
{
    public partial class DtcDtrMissMatchEntry : System.Web.UI.Page
    {
        string strFormCode = "DtcDtrSwapping";
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
            if (!IsPostBack)
            {
                checkboxdiv.Style.Add("display", "none");
                divOldTc.Style.Add("display", "none");
                LoadSearchWindow();
                CheckAccessRights("1", "4");
                sRemark = ConfigurationManager.AppSettings["MissMatchRemarks"].ToString();
                txtremarks.Text = sRemark;
            }
        }
        /// <summary>
        /// LoadSearchWindow
        /// </summary>
        public void LoadSearchWindow()
        {
            try
            {
                string strQry = string.Empty;
                strQry = "Title=Search and Select DTC Details&";
                strQry += "Query=SELECT \"DT_CODE\" AS \"DTC Code\",\"DT_NAME\" AS \"DTC Name\" FROM \"TBLDTCMAST\"";
                strQry += " WHERE \"DT_CODE\" LIKE '" + txtDtcCode.Text + "%' ";
                strQry += " AND {0} like %{1}% LIMIT 50 &";
                strQry += "DBColName=\"DT_CODE\"~\"DT_NAME\"&";
                strQry += "ColDisplayName=DTC Code~DTC Name&";
                strQry = strQry.Replace("'", @"\'");
                btnDtcSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry +
                    "tb=" + txtDtcCode.ClientID + "&btn=" + btnDtcSearch.ClientID + "',520,520," + txtDtcCode.ClientID + ")");

                strQry = "Title=Search and Select DTr Details&";
                strQry += "Query=SELECT \"TC_CODE\" AS \"DTr Code\",\"TC_CAPACITY\" AS \"DTr Capacity\" FROM \"TBLTCMASTER\"";
                strQry += " WHERE CAST(\"TC_CODE\" AS TEXT) LIKE '" + txtDtrCode.Text + "%' ";
                strQry += " AND {0} like %{1}% LIMIT 50 &";
                strQry += "DBColName=CAST(\"TC_CODE\" AS TEXT)~CAST(\"TC_CAPACITY\" AS TEXT)&";
                strQry += "ColDisplayName=DTr Code~DTr Capacity&";
                strQry = strQry.Replace("'", @"\'");
                btnDtrSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry +
                    "tb=" + txtDtrCode.ClientID + "&btn=" + btnDtrSearch.ClientID + "',520,520," + txtDtrCode.ClientID + ")");

                strQry = "Title=Search and Select DTC Details&";
                strQry += "Query=SELECT \"DT_CODE\" AS \"DTC Code\",\"DT_NAME\" AS \"DTC Name\" FROM \"TBLDTCMAST\"";
                strQry += " WHERE \"DT_CODE\" LIKE '" + txtOldDtc.Text + "%'";
                strQry += " AND {0} like %{1}% LIMIT 50 &";
                strQry += "DBColName=\"DT_CODE\"~\"DT_NAME\"&";
                strQry += "ColDisplayName=DTC Code~DTC Name&";
                strQry = strQry.Replace("'", @"\'");
                btnSearchdtc2.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry +
                    "tb=" + txtOldDtc.ClientID + "&btn=" + btnSearchdtc2.ClientID + "',520,520," + txtOldDtc.ClientID + ")");
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        #region Access Rights
        /// <summary>
        /// CheckAccessRights
        /// </summary>
        /// <param name="sAccessType"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public bool CheckAccessRights(string sAccessType, string flag)
        {
            try
            {
                #region Note:  // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY
                clsApproval objApproval = new clsApproval();
                objApproval.sFormName = "DTCMissMatch";
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sAccessType = Constants.CheckAccessRights.CheckAccessRightsAll + "," + sAccessType;
                bool bResult = objApproval.CheckAccessRights(objApproval);
                if (flag == "2")
                {
                    if (UserValid() == false)
                    {
                        if (bResult == true)
                        {
                            Response.Redirect("~/UserRestrict.aspx", false);
                            bResult = false;
                        }
                    }
                }
                else if (flag == "1")
                {
                    if (UserValid() == false)
                    {
                        if (bResult == false)
                        {
                            Response.Redirect("~/UserRestrict.aspx", false);
                        }
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
        /// Navigate to clsDtcMissMatchEntry.cs form for getting dtc details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDtcSearch_Click(object sender, EventArgs e)
        {
            try
            {
                clsDtcMissMatchEntry objDtcMissMatch = new clsDtcMissMatchEntry();
                objDtcMissMatch.sDtcCode = txtDtcCode.Text;
                DataTable dt = new DataTable();
                dt = objDtcMissMatch.LoadDtcDetails(objDtcMissMatch);
                if (dt.Rows.Count > 0)
                {
                    ViewState["OldtcCode"] = dt.Rows[0]["DT_TC_ID"].ToString();
                    lblDtrCode.Text = "TC: " + dt.Rows[0]["DT_TC_ID"].ToString();
                    checkboxdiv.Style.Add("display", "block");
                    hdfDtrCode.Value = dt.Rows[0]["DT_TC_ID"].ToString();
                    hdfLocType.Value = dt.Rows[0]["TC_CURRENT_LOCATION"].ToString();
                    hdfOffCode.Value = dt.Rows[0]["DT_OM_SLNO"].ToString();
                }
                else
                {
                    ShowMsgBox("For Entered DTC Code Not Have Records");
                    txtDtcCode.Focus();
                    return;
                }
                grdDtcDetails.DataSource = dt;
                grdDtcDetails.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError("dtcsearch", "btnDtcSearch_Click", ex.Message, ex.StackTrace);
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
                dt = objDtcMissMatch.LoadDtrDetails(objDtcMissMatch);
                if (dt.Rows.Count > 0)
                {
                    hdfNewLocType.Value = dt.Rows[0]["TC_CURRENT_LOCATION"].ToString();
                    hdfNewOffCode.Value = dt.Rows[0]["TC_LOCATION_ID"].ToString();
                }
                else
                {
                    ShowMsgBox("For Entered DTr Code Not Have Records");
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
        /// CheckBox1_CheckedChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox1.Checked == true)
            {
                divOldTc.Style.Add("display", "block");
            }
            else
            {
                divOldTc.Style.Add("display", "none");
            }
        }
        /// <summary>
        /// cmdReset_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdReset_Click(object sender, EventArgs e)
        {
            checkboxdiv.Style.Add("display", "none");
            divOldTc.Style.Add("display", "none");
            grdDtcDetails.DataSource = null;
            grdDtcDetails.DataBind();
            grdDtrDetails.DataSource = null;
            grdDtrDetails.DataBind();
            grdDtrDetails2.DataSource = null;
            grdDtrDetails2.DataBind();
            grdSecondDtcDetails.DataSource = null;
            grdSecondDtcDetails.DataBind();
            txtDtcCode.Text = string.Empty;
            txtDtrCode.Text = string.Empty;
            txtOldDtc.Text = string.Empty;
            CheckBox1.Checked = false;
            txtremarks.Text = ConfigurationManager.AppSettings["MissMatchRemarks"].ToString();
        }
        /// <summary>
        /// Navigate to clsDtcMissMatchEntry.cs form for getting 2nd dtc details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearchdtc2_Click(object sender, EventArgs e)
        {
            try
            {
                clsDtcMissMatchEntry objDtcMissMatch = new clsDtcMissMatchEntry();
                DataTable dt = new DataTable();
                objDtcMissMatch.sDtcCode = txtOldDtc.Text;
                dt = objDtcMissMatch.LoadDtcDetails(objDtcMissMatch);
                grdSecondDtcDetails.DataSource = dt;
                grdSecondDtcDetails.DataBind();
                objDtcMissMatch.sDtrCode = ViewState["OldtcCode"].ToString();
                dt = objDtcMissMatch.LoadDtrDetails(objDtcMissMatch);

                grdDtrDetails2.DataSource = dt;
                grdDtrDetails2.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// Navigate to clsDtcMissMatchEntry.cs form for dtc and dtr allocation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdAllocate_Click(object sender, EventArgs e)
        {
            DataTable DtDtcCheck = new DataTable();
            DataTable DtDtrCheck = new DataTable();
            clsDtcMissMatchEntry objMissMatch = new clsDtcMissMatchEntry();
            objMissMatch.sDtcCode = txtDtcCode.Text;
            objMissMatch.sDtrCode = txtDtrCode.Text;
            try
            {
                if (txtDtcCode.Text == "" || txtDtcCode.Text == null)
                {
                    ShowMsgBox("Please Enter DTC Code");
                    txtDtcCode.Focus();
                    return;
                }

                DtDtcCheck = objMissMatch.LoadDtcDetails(objMissMatch);
                if (DtDtcCheck.Rows.Count == 0)
                {
                    ShowMsgBox("For Entered DTC Code Not Have Records");
                    txtDtcCode.Focus();
                    return;
                }

                if (txtDtrCode.Text == "")
                {
                    ShowMsgBox("Please Enter DTC Code");
                    txtDtrCode.Focus();
                    return;
                }

                DtDtrCheck = objMissMatch.LoadDtrDetails(objMissMatch);
                if (DtDtrCheck.Rows.Count == 0)
                {
                    ShowMsgBox("For Entered DTr Code Not Have Records");
                    txtDtrCode.Focus();
                    return;
                }
                bool bResult = CheckAccessRights("1", "2");
                if (bResult == true)
                {

                    string[] sArr = new string[2];
                    objMissMatch.sDtrCode = hdfDtrCode.Value;
                    objMissMatch.sDtcCode = txtDtcCode.Text.Trim();
                    objMissMatch.sLocType = hdfLocType.Value;
                    objMissMatch.sNewDTCCode = txtOldDtc.Text;
                    objMissMatch.sNewLocType = hdfNewLocType.Value;
                    objMissMatch.sOfficeCode = hdfOffCode.Value;
                    objMissMatch.sNewOfficeCode = hdfNewOffCode.Value;
                    objMissMatch.sNewDtrCode = txtDtrCode.Text.Trim();

                    if (CheckBox1.Checked == true)
                    {
                        if (txtOldDtc.Text == "")
                        {
                            ShowMsgBox("Please Enter DTC Code");
                            txtOldDtc.Focus();
                            return;
                        }
                    }
                    if (objMissMatch.sDtrCode == "" || objMissMatch.sNewDtrCode == "")
                    {

                        ShowMsgBox("Load DTr and DTC Details Before Proceedding");
                        return;
                    }

                    objMissMatch.sRemarks = txtremarks.Text;
                    objMissMatch.sCrBy = objSession.UserId;
                    if (ValidateForm(objMissMatch))
                    {
                        sArr = objMissMatch.swapDetails(objMissMatch);
                    }
                    if (sArr[0] == "1")
                    {
                        ShowMsgBox("DTC Allocated Successfully");
                        cmdReset_Click(sender, e);
                        txtDtcCode.Text = string.Empty;
                        txtDtrCode.Text = string.Empty;
                        txtremarks.Text = ConfigurationManager.AppSettings["MissMatchRemarks"].ToString();
                    }
                    else if (sArr[0] == "2")
                    {
                        ShowMsgBox(sArr[1]);
                    }
                    else
                    {
                        ShowMsgBox("Something went wrong .....");
                    }
                    cmdAllocate.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
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
        /// grdDtcDetails_RowCreated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDtcDetails_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    GridView HeaderGrid = (GridView)sender;
                    GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                    TableCell HeaderCell = new TableCell();
                    HeaderCell.Text = "DTC DETAILS";
                    HeaderCell.ColumnSpan = 4;
                    HeaderGridRow.Cells.Add(HeaderCell);
                    grdDtcDetails.Controls[0].Controls.AddAt(0, HeaderGridRow);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// grdNewMappingDetails_RowCreated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdNewMappingDetails_RowCreated(object sender, GridViewRowEventArgs e)
        {

        }
        /// <summary>
        /// grdDtrDetails_RowCreated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDtrDetails_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    GridView HeaderGrid = (GridView)sender;
                    GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                    TableCell HeaderCell = new TableCell();
                    HeaderCell.Text = "DTr DETAILS";
                    HeaderCell.ColumnSpan = 5;
                    HeaderGridRow.Cells.Add(HeaderCell);
                    grdDtrDetails.Controls[0].Controls.AddAt(0, HeaderGridRow);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// grdDtrDetails2_RowCreated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDtrDetails2_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    GridView HeaderGrid = (GridView)sender;
                    GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                    TableCell HeaderCell = new TableCell();
                    HeaderCell.Text = "DTr DETAILS";
                    HeaderCell.ColumnSpan = 5;
                    HeaderGridRow.Cells.Add(HeaderCell);
                    grdDtrDetails2.Controls[0].Controls.AddAt(0, HeaderGridRow);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// grdSecondDtcDetails_RowCreated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSecondDtcDetails_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    GridView HeaderGrid = (GridView)sender;
                    GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                    TableCell HeaderCell = new TableCell();
                    HeaderCell.Text = "DTC DETAILS";
                    HeaderCell.ColumnSpan = 4;
                    HeaderGridRow.Cells.Add(HeaderCell);
                    grdSecondDtcDetails.Controls[0].Controls.AddAt(0, HeaderGridRow);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// Before allocation check dtc and dtr textbox dtr allocated or not 
        /// </summary>
        /// <param name="objDtcMisEntry"></param>
        /// <returns></returns>
        public bool ValidateForm(clsDtcMissMatchEntry objDtcMisEntry)
        {
            try
            {
                if (objDtcMisEntry.sDtrCode == objDtcMisEntry.sNewDtrCode)
                {
                    ShowMsgBox("DTr Already Allocated with the Same DTC");
                    txtDtrCode.Focus();
                    return false;
                }
                if (objDtcMisEntry.sDtcCode == objDtcMisEntry.sNewDTCCode)
                {
                    ShowMsgBox("DTr Already Allocated with the Same DTC");
                    txtOldDtc.Focus();
                    return false;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
            return true;
        }
    }
}