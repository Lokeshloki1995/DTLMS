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

namespace IIITS.DTLMS.TCRepair
{
    public partial class TransformerDetails : System.Web.UI.Page
    {
        clsSession objSession;
        /// <summary>
        /// This function used for default page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }

                lblMessage.Text = string.Empty;
                objSession = (clsSession)Session["clsSession"];
                if (!IsPostBack) 
                {
                    if (Request.QueryString["TcId"] != null && Request.QueryString["TcId"].ToString() != "")
                    {
                        string Qry = "SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\"  WHERE \"MD_TYPE\" = 'G_Type'";
                        Genaral.Load_Combo(Qry, cmbGuaranteetype);

                        hdfdtrid.Value = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TcId"]));
                        if (Request.QueryString["EditOp"] == "1")
                        {
                            cmbGuaranteetype.Enabled = true;
                            btnsubmit.Enabled = true;
                        }
                        else
                        {
                            cmbGuaranteetype.Enabled = false;
                            txtReason.Enabled = false;
                            btnsubmit.Enabled = false;
                        }

                        LoadTransformerdetails(hdfdtrid.Value);

                    }

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        /// <summary>
        /// This function used to call the method LoadTransformerdetails and fetch the values from there and assign to text fields
        /// </summary>
        /// <param name="Dtrid"></param>

        public void LoadTransformerdetails(string Dtrid)
        {
            try
            {
                clsDTrRepairActivity objTcFailure = new clsDTrRepairActivity();
                objTcFailure.sTcId = Dtrid;
                DataTable dt = objTcFailure.LoadTransformerdetails(objTcFailure);


                if (dt.Rows.Count > 0)
                {
                    hdfdtrid.Value = Convert.ToString(dt.Rows[0]["TC_ID"]);
                    txtDtrcode.Text = Convert.ToString(dt.Rows[0]["TC_CODE"]);
                    txtDtrslno.Text = Convert.ToString(dt.Rows[0]["TC_SLNO"]);
                    txtMakename.Text = Convert.ToString(dt.Rows[0]["TM_NAME"]);
                    txtManfdate.Text = Convert.ToString(dt.Rows[0]["TC_MANF_DATE"]);
                    txtCapacity.Text = Convert.ToString(dt.Rows[0]["TC_CAPACITY"]);
                    txtStarrate.Text = Convert.ToString(dt.Rows[0]["TC_STAR_RATE"]);
                    txtRepairedcount.Text = Convert.ToString(dt.Rows[0]["RCOUNT"]);
                    cmbGuaranteetype.SelectedValue = Convert.ToString(dt.Rows[0]["MD_ID"]);
                    HdfOldGuaranteetype.Value = Convert.ToString(dt.Rows[0]["TC_GUARANTY_TYPE"]);
                    txtReason.Text = Convert.ToString(dt.Rows[0]["TC_REASON"]);
                    txtRemarks.Text = Convert.ToString(dt.Rows[0]["TC_REMARKS"]);

                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// This function used for save click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm() == true)
                {

                    clsDTrRepairActivity objtransdetails = new clsDTrRepairActivity();
                    string[] Arr = new string[2];
                    objtransdetails.Remarks = txtRemarks.Text;
                    objtransdetails.Reason = txtReason.Text;
                    objtransdetails.sGuarantyType = cmbGuaranteetype.SelectedItem.Text;
                    objtransdetails.sCrby = objSession.UserId;
                    objtransdetails.sTcId = hdfdtrid.Value;
                    objtransdetails.sTcCode = txtDtrcode.Text;
                    objtransdetails.OldGuaranteetype = HdfOldGuaranteetype.Value;
                    Arr = objtransdetails.Updatetcmaster(objtransdetails);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "Update tc Master");
                    }
                    if (Arr[1].ToString() == "0")
                    {
                        ShowMsgBox(Arr[0]);
                        btnsubmit.Enabled = false;
                        return;
                    }
                    else
                    {
                        ShowMsgBox("Something Went Wrong");
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }
        /// <summary>
        /// This function used to check the validations
        /// </summary>
        /// <returns></returns>
        bool ValidateForm()
        {
            bool bValidate = false;
            try
            {
                if (txtReason.Text.Trim().Length == 0)
                {
                    txtReason.Focus();
                    ShowMsgBox("Please Enter Reason");
                    return bValidate;
                }
                if(cmbGuaranteetype.SelectedItem.Text== HdfOldGuaranteetype.Value)
                {
                    cmbGuaranteetype.Focus();
                    ShowMsgBox("DTr Code already having same Guarantee Type, please select different Guarantee Type.");
                    return bValidate;
                }

                bValidate = true;
                return bValidate;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return bValidate;
            }

        }
        /// <summary>
        /// This function used for close the page and redirect to faulty search form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdClose_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("FaultTCSearch.aspx", false);
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
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
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// This function will work once Guaranteetype selected from the drop down  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbGuaranteetype_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtRemarks.Text = "Guarantee Type Changed From " + HdfOldGuaranteetype.Value
                + " to " + cmbGuaranteetype.SelectedItem.Text;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}