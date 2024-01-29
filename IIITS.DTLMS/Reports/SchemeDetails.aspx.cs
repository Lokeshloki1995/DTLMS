using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Configuration;

namespace IIITS.DTLMS.Reports
{
    public partial class SchemeDetails : System.Web.UI.Page
    {
        string Officecode = string.Empty;
        string strFormCode = "SchemeDetails";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];
                txtFromDate.Attributes.Add("readonly", "readonly");
                CalendarExtender3.EndDate = System.DateTime.Now.AddDays(0);
                if (!IsPostBack)
                {
                    Genaral.Load_Combo("SELECT \"FY_YEARS\",\"FY_YEARS\" FROM \"TBLFINANCIALYEAR\"  WHERE \"FY_STATUS\"='1' ORDER BY \"FY_ID\"", "--Select--", cmbFinYear);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void Export_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                clsReports objrep = new clsReports();
                string fnclyear = string.Empty;

                string sResult = string.Empty;
                if (cmbFinYear.Text == "")
                {
                    ShowMsgBox("Please Select The Financial Year");
                    cmbFinYear.Focus();
                    return;
                }

                string previousyear = cmbFinYear.SelectedValue.Split('-').GetValue(0).ToString();
                string presentyear = cmbFinYear.SelectedValue.Split('-').GetValue(1).ToString();
                
                string sParam = "id=SchemeDetails&presentyear=" + presentyear + "&previosyear=" + previousyear + "&financialyear=" + cmbFinYear.SelectedValue + "";
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + sParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void Export_Click1(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                clsReports objrep = new clsReports();
                string fnclyear = string.Empty;

                string sResult = string.Empty;
                if (txtFromDate.Text == "")
                {
                    ShowMsgBox("Please Select The  Month");
                    txtFromDate.Focus();
                    return;
                }
                string month = txtFromDate.Text.Trim();

                DateTime DToDate = DateTime.ParseExact(txtFromDate.Text, "MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                objrep.sFromDate = DToDate.AddDays(28).ToString("yyyy-MM-dd");

                string sParam = "id=SchemeDetailscapacitywise&fromdate=" + objrep.sFromDate + "&month="+ month + "";
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + sParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                
            }
            catch (Exception ex)
            {
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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}