using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IIITS.DTLMS.BL;
using IIITS.DTLMS.BL.Query;
using System.Reflection;

namespace IIITS.DTLMS.Query
{
    public partial class FailureDateMissMatch : System.Web.UI.Page
    {
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/InternalLogin.aspx", false);
                }

                objSession = (clsSession)Session["clsSession"];

                if (!IsPostBack)
                {
                    txtDTrFailDate.Attributes.Add("readOnly", "readOnly");
                    CalendarExtender1.EndDate = DateTime.Now;

                    LoadSearchWindow();
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
        /// For loading Search Window
        /// </summary>
        public void LoadSearchWindow()
        {
            try
            {
                string strQry = string.Empty;
                strQry += "Title=Search and Select DTC CODE Details&";
                strQry += "Query=select  \"DF_DTC_CODE\",UPPER(\"DT_NAME\") AS \"DT_NAME\"  FROM \"TBLDTCFAILURE\" INNER JOIN \"TBLDTCMAST\" ON ";
                strQry += "\"DF_REPLACE_FLAG\" = 0  AND CAST(\"DF_DTC_CODE\" AS TEXT) LIKE '" + txtDTCCode.Text.Trim() + "%' ";
                strQry += "AND \"DF_DTC_CODE\"=\"DT_CODE\" AND {0} like %{1}% order by \"DF_DTC_CODE\" limit 500 &";
                strQry += "DBColName=CAST(\"DF_DTC_CODE\" AS TEXT)~upper(CAST(\"DT_NAME\" AS TEXT))&";
                strQry += "ColDisplayName=DF DTC CODE~DTC Name&";
                strQry = strQry.Replace("'", @"\'");
                btnDtcSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry +
                    "tb=" + txtDTCCode.ClientID + "&btn=" + btnDtcSearch.ClientID + "',520,520," + txtDTCCode.ClientID + ")");
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
        /// Navigate to clsFailureDateAndDtrDocMissMatch.cs form for getting  dtc details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDtcSearch_Click(object sender, EventArgs e)
        {
            try
            {
                clsFailureDateAndDtrDocMissMatch objDtrFailDateMissMatch = new clsFailureDateAndDtrDocMissMatch();
                objDtrFailDateMissMatch.DtcCode = txtDTCCode.Text.Trim();
                DataTable dt = new DataTable();
                dt = objDtrFailDateMissMatch.LoadFailDtcDtr(objDtrFailDateMissMatch);
                if (dt.Rows.Count > 0)
                {
                    txtDTCCode.Text = dt.Rows[0]["DF_DTC_CODE"].ToString();
                    txtTcCode.Text = dt.Rows[0]["DF_EQUIPMENT_ID"].ToString();
                    grdDtrCommDateDetails.DataSource = dt;
                    grdDtrCommDateDetails.DataBind();
                    grdTable.Attributes.Add("style", "display:block");
                }
                else
                {
                    ShowMsgBox("DTC Not Failed or CR Completed");
                    grdTable.Attributes.Add("style", "display:none");
                }
                txtDTCCode.Enabled = false;
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
        /// Navigate to clsFailureDateAndDtrDocMissMatch.cs form for update failure date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            string[] Arr = new string[2];
            DataTable dtDeatils = new DataTable();
            try
            {
                if (ValidateForm() == true)
                {
                    clsFailureDateAndDtrDocMissMatch objFailDate = new clsFailureDateAndDtrDocMissMatch();
                    objFailDate.DtrCode = txtTcCode.Text;
                    objFailDate.DtcCode = txtDTCCode.Text.Replace("'", "");
                    objFailDate.DTrFailureDate = txtDTrFailDate.Text.Replace("'", "");
                    objFailDate.TicketNumber = txtTicketNumber.Text;
                    Arr = objFailDate.UpdateFailDate(objFailDate);
                    dtDeatils = objFailDate.LoadFailDtcDtr(objFailDate);
                    if (dtDeatils.Rows.Count > 0)
                    {
                        grdDtrCommDateDetails.DataSource = dtDeatils;
                        grdDtrCommDateDetails.DataBind();
                    }

                    if (Arr[1].ToString() == "1")
                    {
                        ShowMsgBox(Arr[0].ToString());
                    }

                    if (Arr[1].ToString() == "0")
                    {
                        ShowMsgBox(Arr[0].ToString());
                        cmdReset_Click(sender, e);
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
                clsException.LogError(
                             MethodBase.GetCurrentMethod().DeclaringType.Name,
                             MethodBase.GetCurrentMethod().Name,
                             ex.Message,
                             ex.StackTrace);
            }

        }

        /// <summary>
        /// Validating while Updating failure date
        /// </summary>
        /// <returns></returns>
        public bool ValidateForm()
        {
            bool bValidate = false;
            try
            {
                if (txtDTCCode.Text == "")
                {
                    ShowMsgBox("Please Enter DTC Code");
                    txtDTCCode.Focus();
                    return bValidate;
                }
                if (txtTcCode.Text == "")
                {
                    ShowMsgBox("Please Enter DTR Code");
                    txtTcCode.Focus();
                    return bValidate;
                }
                if (txtDTrFailDate.Text == "")
                {
                    ShowMsgBox("Please Enter DTR Failure Date");
                    txtDTrFailDate.Focus();
                    return bValidate;
                }
                if (txtTicketNumber.Text == "")
                {
                    ShowMsgBox("Please Enter Ticket Number");
                    txtTicketNumber.Focus();
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
        /// Reset the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtDTCCode.Text = string.Empty;
                txtTcCode.Text = string.Empty;
                txtDTCCode.Enabled = true;
                hdfDTCcode.Value = string.Empty;
                btnDtcSearch.Visible = true;
                txtDTrFailDate.Text = string.Empty;
                txtTicketNumber.Text = string.Empty;
                grdTable.Attributes.Add("style", "display:none");

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