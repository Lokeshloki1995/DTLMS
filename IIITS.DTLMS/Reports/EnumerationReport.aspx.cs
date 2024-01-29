using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;


namespace IIITS.DTLMS.Reports
{
    public partial class EnumerationReport : System.Web.UI.Page
    {
        string strFormCode = "EnumerationReport";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/InternalLogin.aspx", false);
                }

                objSession = (clsSession)Session["clsSession"];
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");

                CalendarExtender2.EndDate = System.DateTime.Now;
                CalendarExtender1.EndDate = System.DateTime.Now;

                if (!IsPostBack)
                {

                    txtFromDate.Attributes.Add("onblur", "return ValidateDate(" + txtFromDate.ClientID + ");");
                    txtToDate.Attributes.Add("onblur", "return ValidateDate(" + txtToDate.ClientID + ");");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdReport_Click(object sender, EventArgs e)
        {
            try
            {
                clsReports objEnumReport = new clsReports();
                string sResult = string.Empty;

                if (txtFromDate.Text.Trim() == "")
                {
                    ShowMsgBox("Enter From Date");
                    return;

                }
                if (txtFromDate.Text != "")
                {
                    sResult = Genaral.DateValidation(txtFromDate.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtFromDate.Focus();
                        return;
                    }
                }
                if (txtToDate.Text != "")
                {
                    sResult = Genaral.DateValidation(txtToDate.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtToDate.Focus();
                        return;
                    }
                }

                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    sResult = Genaral.DateComparision(txtToDate.Text, txtFromDate.Text, false, false);
                    if (sResult == "2")
                    {
                        ShowMsgBox("To Date should be Greater than From Date");
                        txtToDate.Focus();
                        return;

                    }
                }

                if (txtToDate.Text.Trim() == "")
                {
                    txtToDate.Text = txtFromDate.Text;
                }

                objEnumReport.sFromDate = txtFromDate.Text;
                objEnumReport.sTodate = txtToDate.Text;
                objEnumReport.sType = cmbType.SelectedValue;
                //objEnumReport.EnumerationReport(objEnumReport);
                if (cmbType.SelectedIndex <= 0)
                {
                    ShowMsgBox("Please Select report type");
                    cmbType.Focus();
                    return;
                }
                if (cmbType.SelectedValue == "1")
                {
                    string strParam = string.Empty;
                    strParam = "id=EnumReport&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text + "";
                    RegisterStartupScript("Print", "<script>window.open('ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

                }
                if (cmbType.SelectedValue == "2")
                {
                    string strParam = string.Empty;
                    strParam = "id=LocOperator&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text + "";
                    RegisterStartupScript("Print", "<script>window.open('ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

                }
                if (cmbType.SelectedValue == "3")
                {
                    string strParam = string.Empty;
                    strParam = "id=DetailedField&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text + "";
                    RegisterStartupScript("Print", "<script>window.open('ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

                }
                if (cmbType.SelectedValue == "4")
                {
                    string strParam = string.Empty;
                    strParam = "id=DetailedStore&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text + "";
                    RegisterStartupScript("Print", "<script>window.open('ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

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

    }
}