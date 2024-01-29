using IIITS.DTLMS.BL;
using IIITS.DTLMS.BL.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.Query
{
    public partial class FailureTransactionDelete : System.Web.UI.Page
    {
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToString(Session["clsSession"] ?? "").Length == 0)
                {
                    Response.Redirect("~/InternalLogin.aspx", true);
                }
                objSession = (clsSession)Session["clsSession"];

                if (!IsPostBack)
                {
                    string DTCCode = Request.QueryString["DTCCOde"] ?? "-1";
                    string TicketNum = Request.QueryString["TicketNumber"] ?? "-1";
                    if ((DTCCode ?? "").Length > 0 && DTCCode != "-1")
                    {
                        txtDTCCode.Text = DTCCode;
                        txtTicketNumber.Text = TicketNum;
                        if (Convert.ToString(Request.QueryString["DTRCODE"] ?? "").Length > 0)
                        {
                            hdfDtrCode.Value = Convert.ToString(Request.QueryString["DTRCODE"]);
                        }
                        DeleteFailureTransaction_Click(this, EventArgs.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            lblMessage.Text = string.Empty;
        }
        protected void LoadFailureDtc_Click(object sender, EventArgs e)
        {
            try
            {
                if ((txtDTCCode.Text ?? "").Length == 0)
                {
                    ShowMsgBox("Please Enter Valid DTC Code ");
                    txtDTCCode.Focus();
                    return;
                }
                if (txtDTCCode.Text.Length < 6)
                {
                    ShowMsgBox("Please Enter Valid 6 Digit DTC Code ");
                    txtDTCCode.Focus();
                    return;
                }

                clsFailureTransactionDelete objLoadFailureDtc = new clsFailureTransactionDelete();
                objLoadFailureDtc.DtcCode = txtDTCCode.Text.Trim();
                DataTable dt = new DataTable();

                dt = objLoadFailureDtc.LoadFailureDtcDetails(objLoadFailureDtc);
                if (dt.Rows.Count > 0)
                {
                    txtDTCCode.Text = dt.Rows[0]["DTC_CODE"].ToString();
                    hdfDtc.Value = dt.Rows[0]["DTC_CODE"].ToString();
                    hdfDtrCode.Value = dt.Rows[0]["TC_CODE"].ToString();
                    grdDtcFailDetails.DataSource = dt;
                    grdDtcFailDetails.DataBind();

                    grdTable.Attributes.Add("style", "display:block");
                    txtDTCCode.Enabled = false;

                    dt = objLoadFailureDtc.ValidationMajorFailDelete(objLoadFailureDtc);
                    if (dt.Rows.Count > 0)
                    {
                        hdfinIno.Value = dt.Rows[0]["IN_NO"].ToString();
                        hdftrRvNo.Value = dt.Rows[0]["TR_RV_NO"].ToString();
                    }
                }
                else
                {
                    ShowMsgBox("DTC Not Failed or CR Completed");
                    grdTable.Attributes.Add("style", "display:none");
                }                                                              
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void ShowMsgBox(string sMsg)
        {
            try
            {
                string sShowMsg = string.Empty;
                sShowMsg = "<script language=javascript> swal('" + sMsg + "')</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "Msg", sShowMsg);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        public void DeleteFailureTransaction_Click(object sender, EventArgs e)
        {
            string[] Arr = new string[2];
            DataTable dtDeatils = new DataTable();
            DataTable dt = new DataTable();

            try
            {
                clsFailureTransactionDelete Obj = new clsFailureTransactionDelete();
                Obj.DtcCode = txtDTCCode.Text.Replace("'", "");
                Obj.TicketNumber = txtTicketNumber.Text;

                if (Convert.ToString(hdfDtrCode.Value ?? "").Length > 0)
                {
                    Obj.DtrCode = hdfDtrCode.Value;
                }

                if (ValidateForm(Obj) == true)
                {
                    Obj = Obj.GetBoid(Obj);

                    Arr = Obj.DeleteFailureTransactionDetails(Obj);

                    if (Arr[1].ToString() == "0")
                    {
                        ShowMsgBox(Arr[0].ToString());
                        
                    }
                    else
                    {
                        ShowMsgBox(Arr[0]);
                        return;
                    }

                    txtDTCCode.Text = "";
                    txtTicketNumber.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        public bool ValidateForm(clsFailureTransactionDelete Obj)
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

                if (txtTicketNumber.Text.Trim() == "")
                {
                    ShowMsgBox("Please Enter Ticket no");
                    txtTicketNumber.Focus();
                    return bValidate;
                }
                else
                {
                    if (!txtTicketNumber.Text.ToUpper().Contains("HDT") || !txtTicketNumber.Text.ToUpper().StartsWith("HDT"))
                    {
                        ShowMsgBox("Please Enter Valid Ticket No");
                        txtTicketNumber.Focus();
                        return bValidate;
                    }
                }

                if (CheckReparerCenterRecord(Obj))
                {
                    ShowMsgBox("This Faild DTr is Pushed to Repare Center, Failure Transation Can not be Deleted");
                    return bValidate;
                }
                bValidate = true;
                return bValidate;
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                return bValidate;
            }
        }
        public bool CheckReparerCenterRecord(clsFailureTransactionDelete Obj)
        {
            try
            {
                Obj.isReparerCenterd = Obj.GetisReparerCenterdRecord(Obj);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                Obj.isReparerCenterd = false;
                return Obj.isReparerCenterd;
            }
            return Obj.isReparerCenterd;
        }
        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Query/FailureTransactionDelete.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
    }
}