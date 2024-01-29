using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using IIITS.DTLMS.BL;

namespace IIITS.DTLMS.DtcMissMatch
{
    public partial class Allocate : System.Web.UI.Page
    {
        string strFormCode = "Allocate";
        clsSession objSession;
        string sRemark;
        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
            {
                Response.Redirect("~/Login.aspx", false);
            }
            objSession = (clsSession)Session["clsSession"];
            if (!IsPostBack)
            {
                LoadSearchWindow();
                CheckAccessRights("1", "1");
                sRemark = ConfigurationManager.AppSettings["AllocatedRemarks"].ToString();
                txtremarks.Text = sRemark;                
            }
        }
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
                #region NOTe: // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY
                clsApproval objApproval = new clsApproval();
                objApproval.sFormName = "AllocateUnmapp";
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sAccessType = "1" + "," + sAccessType;
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
                string Userid = Convert.ToString(ConfigurationManager.AppSettings["SELECTEDUSER"]);
                string[] sUserid = Userid.Split(',');
                for (int i = 0; i < sUserid.Length; i++)
                {
                    if (objSession.UserId != sUserid[i])
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
        /// <summary>
        /// LoadSearchWindow
        /// </summary>
        public void LoadSearchWindow()
        {
            try
            {
                string strQry = string.Empty;
                strQry = "Title=Search and Select DTC Details&";
                strQry += "Query=SELECT DISTINCT \"UNA_DTCCODE\" as \"DTC Code\",\"DT_NAME\" as \"DTC Name\" ";
                strQry += "FROM \"TBLUNALLOCATEDDTCS\",\"TBLDTCMAST\" WHERE \"UNA_DTCCODE\"=\"DT_CODE\" ";
                strQry += "AND \"UNA_REALLOCATED_DTR_SLNO\" IS NULL AND \"UNA_DTCCODE\" LIKE '" + txtDtcCode.Text + "%'";
                strQry += " AND {0} like %{1}% &";
                strQry += "DBColName=\"UNA_DTCCODE\"~\"DT_NAME\"&";
                strQry += "ColDisplayName=DTC Code~DTC Name&";
                strQry = strQry.Replace("'", @"\'");

                btnDtcSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + 
                    txtDtcCode.ClientID + "&btn=" + btnDtcSearch.ClientID + "',520,520," + txtDtcCode.ClientID + ")");

                strQry = "Title=Search and Select DTr Details&";
                strQry += "Query=SELECT DISTINCT \"TC_CODE\" as \"DTr Code\" ,\"TC_CAPACITY\" as \"DTr Capacity\" ";
                strQry += "FROM (SELECT \"TC_CODE\",\"TC_CAPACITY\" FROM \"TBLUNALLOCATEDDTRS\",\"TBLDTCMISMATCHENTRY\",";
                strQry += "\"TBLTCMASTER\" WHERE \"DME_SL_NO\"=\"UAD_SL_MISMATCHENTRY_SLNO\" ";
                strQry += "AND \"DME_EXISTING_DTR_CODE\"=\"TC_CODE\" UNION ALL SELECT \"TC_CODE\",";
                strQry += "\"TC_CAPACITY\" FROM \"TBLTCMASTER\" WHERE \"TC_CURRENT_LOCATION\"='1' AND \"TC_STATUS\" IN (1,2))a WHERE  ";
                strQry += "CAST(\"TC_CODE\" AS TEXT) LIKE '" + txtDtrCode.Text + "%'";
                strQry += " AND cast({0} as text) like %{1}% LIMIT 50 &";
                strQry += "DBColName=\"TC_CODE\"~\"TC_CAPACITY\"&";
                strQry += "ColDisplayName=DTr Code~DTr Capacity&";

                strQry = strQry.Replace("'", @"\'");

                btnDtrSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" +
                    txtDtrCode.ClientID + "&btn=" + btnDtrSearch.ClientID + "',520,520," + txtDtrCode.ClientID + ")");

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// btnDtcSearch_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDtcSearch_Click(object sender, EventArgs e)
        {
            try
            {
                clsDtcMissMatchEntry objDtcMissMatch = new clsDtcMissMatchEntry();
                DataTable dt = new DataTable();
                objDtcMissMatch.sDtcCode = txtDtcCode.Text;
                dt = objDtcMissMatch.GetUnmapDetails(objDtcMissMatch);
                if (dt.Rows.Count > 0)
                {
                    hdfOffCode.Value = dt.Rows[0]["DT_OM_SLNO"].ToString();
                    grdDtcDetails.DataSource = dt;
                    grdDtcDetails.DataBind();
                }
                else
                {
                    ShowMsgBox("DTC Already got Allocated");
                    txtDtcCode.Text = string.Empty;
                }                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// btnDtrSearch_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDtrSearch_Click(object sender, EventArgs e)
        {
            clsDtcMissMatchEntry objDtcMissMatch = new clsDtcMissMatchEntry();
            DataTable dt = new DataTable();
            string dtccode = string.Empty;
            objDtcMissMatch.sDtrCode = txtDtrCode.Text;
            dt = objDtcMissMatch.GetUnmapDTRDetails(objDtcMissMatch);

            if (dt.Columns.Contains("DT_CODE"))
            {
               dtccode = dt.Rows[0]["DT_CODE"].ToString();                
            }
            if (dtccode == null || dtccode == "")
            {
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            else
            {
                ShowMsgBox("DTr Already Allocated");
                txtDtrCode.Text = string.Empty;               
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
        /// GridView1_RowCreated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    GridView HeaderGrid = (GridView)sender;
                    GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                    TableCell HeaderCell = new TableCell();
                    HeaderCell.Text = "DTr DETAILS";
                    HeaderCell.ColumnSpan = 4;
                    HeaderGridRow.Cells.Add(HeaderCell);
                    GridView1.Controls[0].Controls.AddAt(0, HeaderGridRow);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
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
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// cmdAllocate_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdAllocate_Click(object sender, EventArgs e)
        {
            try
            {
                bool bResult = CheckAccessRights("1", "1");
                if (bResult == true)
                {
                    clsDtcMissMatchEntry objMissMatch = new clsDtcMissMatchEntry();
                    string[] sArr = new string[2];
                    objMissMatch.sDtrCode = txtDtrCode.Text;
                    objMissMatch.sDtcCode = txtDtcCode.Text;
                    objMissMatch.sRemarks = txtremarks.Text;
                    objMissMatch.sCrBy = objSession.UserId;
                    objMissMatch.sRemarks = txtremarks.Text;                   
                    objMissMatch.sOfficeCode = hdfOffCode.Value;                  

                    if (ValidateForm(objMissMatch))
                    {
                        sArr = objMissMatch.AllocateUnMappDTC(objMissMatch);

                        if (sArr[0] == "1")
                        {
                            ShowMsgBox("DTC Allocated Successfully");
                            cmdReset_Click(sender, e);

                        }
                        else if (sArr[0] == "2")
                        {
                            ShowMsgBox("Something Went Wrong......");
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// ValidateForm
        /// </summary>
        /// <param name="objDtcMisEntry"></param>
        /// <returns></returns>
        public bool ValidateForm(clsDtcMissMatchEntry objDtcMisEntry)
        {
            if (txtDtcCode.Text == "" || txtDtcCode.Text == null)
            {
                ShowMsgBox("Please Enter DTC Code");
                txtDtcCode.Focus();
                return false;
            }
            if (txtDtrCode.Text == "" || txtDtrCode.Text == null)
            {
                ShowMsgBox("Please Enter DTr Code");
                txtDtrCode.Focus();
                return false;
            }
            return true;

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
            grdDtcDetails.DataSource = null;
            grdDtcDetails.DataBind();
            GridView1.DataSource = null;
            GridView1.DataBind();
            txtDtcCode.Text = string.Empty;
            txtDtrCode.Text = string.Empty;
            txtremarks.Text = ConfigurationManager.AppSettings["AllocatedRemarks"].ToString();
        }
    }
}