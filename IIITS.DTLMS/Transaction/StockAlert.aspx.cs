using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using IIITS.DTLMS.BL;

namespace IIITS.DTLMS.Transaction
{
    public partial class StockAlert : System.Web.UI.Page
    {
        string strFormCode = "StockAlert";
        clsSession objSession;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                string strQry = string.Empty;
                objSession = (clsSession)Session["clsSession"];
                if (!IsPostBack)
                {
                    strQry += "Title=Search and Select Indent No&";
                    strQry += "Query=select \"TI_INDENT_NO\",\"US_FULL_NAME\" FROM \"TBLINDENT\",\"TBLDTCFAILURE\",\"TBLWORKORDER\",\"TBLUSER\" WHERE \"WO_DF_ID\"=\"DF_ID\" AND ";
                    strQry += " \"WO_SLNO\"=\"TI_WO_SLNO\" AND \"US_ID\"=\"TI_CRBY\" AND CAST(\"DF_LOC_CODE\" AS TEXT) LIKE '" + objSession.OfficeCode + "%'  AND {0} like %{1}% order by \"TI_INDENT_NO\" &";
                    strQry += "DBColName= CAST(\"TI_CRBY\" AS TEXT)~CAST(\"TI_INDENT_NO\" AS TEXT)&";
                    strQry += "ColDisplayName=Indent Created By~Indent Number&";

                    strQry = strQry.Replace("'", @"\'");

                    btnSearchId.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtIndentNoSearch.ClientID + "&btn=" + btnSearchId.ClientID + "',520,520," + txtIndentNoSearch.ClientID + ")");
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void btnSearchId_Click(object sender, EventArgs e)
        {
            clsStockAlert objStockAlert = new clsStockAlert();
            string strQry = string.Empty;
            try
            {

                GetTcDetails();
                //lblTcSlNo.Text = objInvoice.sTcSlNo;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetTcDetails()
        {
            clsStockAlert objStockAlert = new clsStockAlert();

            try
            {
                objStockAlert.sIndentNo = txtIndentNoSearch.Text.Replace("'", "");
                objStockAlert.GetTcDetails(objStockAlert);
                if (objStockAlert.sIndentid != "")
                {
                    txtindentid.Text = objStockAlert.sIndentid;
                    txtFailureId.Text = objStockAlert.sFailureId;
                    txtTcCapacity.Text = objStockAlert.sTcCapacity;
                    txtindentid.Enabled = false;
                    txtIndentNoSearch.Enabled = false;
                    btnSearchId.Enabled = false;
                    cmdSave.Enabled = true;
                }
                else
                {
                    btnSearchId.Enabled = true;
                    txtIndentNoSearch.Enabled = true;
                    ShowMsgBox("Enter Valid Indent Number");

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }//lblTcSlNo.Text = objInvoice.sTcSlNo;
        }



        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {

                GetTcDetails();
                if(txtIndentNoSearch.Text.Replace("'", "").Length > 0)
                {
                    AlertStock();
                }
                

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void AlertStock()
        {
            try
            {
                string[] Arr = new string[2];
                clsStockAlert objStockAlert = new clsStockAlert();
                objStockAlert.sIndentNo = txtIndentNoSearch.Text.Replace("'", "");
                objStockAlert.sFailureId = txtFailureId.Text;
                objStockAlert.sTcCapacity = txtTcCapacity.Text;
                objStockAlert.sCreatedBy = objSession.UserId;
                objStockAlert.sIndentid = txtindentid.Text;
                Arr = objStockAlert.SaveStockAlert(objStockAlert);
                if (Arr[1].ToString() == "0")
                {
                    ShowMsgBox(Arr[0]);
                    btnSearchId.Enabled = true;
                    txtIndentNoSearch.Enabled = true;
                    return;
                }
                if (Arr[1].ToString() == "2")
                {
                    ShowMsgBox(Arr[0]);
                    btnSearchId.Enabled = true;
                    txtIndentNoSearch.Enabled = true;
                    return;
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
                this.Page.RegisterStartupScript("Msg", sShowMsg);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtFailureId.Text = string.Empty;
                txtIndentNoSearch.Text = string.Empty;
                txtTcCapacity.Text = string.Empty;

                btnSearchId.Enabled = true;
                txtIndentNoSearch.Enabled = true;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}