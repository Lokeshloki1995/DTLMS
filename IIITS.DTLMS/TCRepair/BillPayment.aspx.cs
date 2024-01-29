using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;

namespace IIITS.DTLMS.TCRepair
{
    public partial class BillPayment : System.Web.UI.Page
    {
        string strFormCode = "BillPayment";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }

                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;
                txtPOdate.Attributes.Add("readonly", "readonly");
                txtPaymentDate.Attributes.Add("readonly", "readonly");

                CalendarExtender1.EndDate = System.DateTime.Now;
                CalendarExtender1_txtPOdate.EndDate = System.DateTime.Now;

                if (!IsPostBack)
                {

                    string strQry = string.Empty;
                    strQry = "Title=Search and Select Work Order Details&";
                    strQry += "Query=SELECT \"BT_WO_NO\",\"BT_PO_NO\" FROM \"TBLBILLTC\" WHERE \"BT_PAYMENT_FLAG\" =0 and {0} like %{1}% &";
                    strQry += "DBColName=\"BT_WO_NO\"~\"BT_PO_NO\"&";
                    strQry += "ColDisplayName=Work Order No~PO No&";

                    cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtWONo.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + txtWONo.ClientID + ")");
                    txtPOdate.Attributes.Add("onblur", "return ValidateDate(" + txtPOdate.ClientID + ");");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                string[] Arr = new string[2];
                if (ValidateForm() == true)
                {
                    clsBillPayment objPayment = new clsBillPayment();
                    objPayment.sBillId = txtBillId.Text;
                    objPayment.sBRNo = txtBRNo.Text.Trim();
                    objPayment.sPaymentDate = txtPaymentDate.Text;
                    objPayment.sAmount = txtAmount.Text;
                    objPayment.sCrby = objSession.UserId;

                    Arr = objPayment.SaveBillPayment(objPayment);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "Bill payment ");
                    }
                    if (Arr[1].ToString() == "0")
                    {
                        ShowMsgBox(Arr[0].ToString());
                        cmdReset_Click(sender, e);

                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void LoadWOGrid()
        {
            try
            {
                clsBillPayment objBill = new clsBillPayment();
                DataTable dt = new DataTable();

                if (txtWONo.Text != "")
                {
                    objBill.sWONo = txtWONo.Text.Trim();
                    dt = objBill.LoadBillDetails(objBill);
                    grdWo.DataSource = dt;
                    grdWo.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtWONo.Text != "")
                {
                    clsBillPayment objPayment = new clsBillPayment();
                    objPayment.sWONo = txtWONo.Text.Trim();
                    objPayment.GetBillDetails(objPayment);
                    txtBillId.Text = objPayment.sBillId;
                    txtPOdate.Text = objPayment.sPODate;
                    txtPONo.Text = objPayment.sPONo;
                    LoadWOGrid();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public bool ValidateForm()
        {
            bool bValidate = false;
            try
            {
                if (txtWONo.Text.Trim().Length == 0)
                {
                    txtWONo.Focus();
                    ShowMsgBox("Please Search the Work Order No");
                    return bValidate;
                }
                if (txtBRNo .Text.Trim().Length == 0)
                {
                    txtBRNo.Focus();
                    ShowMsgBox("Please Enter the BR No");
                    return bValidate ;
                }
                if (txtAmount.Text.Trim().Length == 0)
                {
                    txtAmount.Focus();
                    ShowMsgBox("Please Enter the Amount");
                    return bValidate;
                }
                if (txtPaymentDate.Text.Trim().Length == 0)
                {
                    txtPaymentDate.Focus();
                    ShowMsgBox("Please Enter Date");
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

        private void ShowMsgBox(string sMsg)
        {
            try
            {
                String sShowMsg = string.Empty;
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
                txtAmount.Text = string.Empty;
                txtBillId.Text = string.Empty;
                txtBRNo.Text = string.Empty;
                txtPaymentDate.Text = string.Empty;
                txtPOdate.Text = string.Empty;
                txtPONo.Text = string.Empty;
                txtWONo.Text = string.Empty;
                grdWo.DataSource = null;
                grdWo.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdWo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdWo.PageIndex = e.NewPageIndex;
                LoadWOGrid();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}