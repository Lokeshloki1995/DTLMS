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
    public partial class Detailsview : System.Web.UI.Page
    {
        string Officecode = string.Empty;
        string strFormCode = "Detailsview";
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
                if (!IsPostBack)
                {
                    txtFromDate.Attributes.Add("readonly", "readonly");
                  

                    CalendarExtender3.EndDate = System.DateTime.Now;
                    Genaral.Load_Combo("SELECT \"FY_YEARS\",\"FY_YEARS\" FROM \"TBLFINANCIALYEAR\"  WHERE \"FY_STATUS\"='1'", "--Select--", cmbFinYear);

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



                string sParam = "id=Detailsview&presentyear=" + presentyear + "&previosyear=" + previousyear + "&financialyear=" + cmbFinYear.SelectedValue + "";
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + sParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");






            }
            catch (Exception ex)
            {

                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }

        }



        protected void Export_Click2(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                clsReports objrep = new clsReports();
                string fnclyear = string.Empty;

                string sResult = string.Empty;
                if (txtFromDate.Text == "")
                {
                    ShowMsgBox("Please Select The Month");
                    txtFromDate.Focus();
                    return;

                }

                string month = txtFromDate.Text.Trim();

                DateTime  previosmonthdt =Convert.ToDateTime(month).AddMonths(-1);


                //string myString = txtDTrCommDate.Text;
                //DateTime myDateTime = DateTime.ParseExact(myString.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //string myString_new = Convert.ToDateTime(myDateTime).ToString("yyyy-MM-dd");

                //DateTime dtime = DateTime.ParseExact(previosmonthdt, "MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                string previousmonth =Convert.ToDateTime(previosmonthdt).ToString("MM-yyyy");

                string presentyear = string.Empty;
                string previousyear = string.Empty;

                if (Convert.ToInt32(txtFromDate.Text.Split('-').GetValue(0)) > 3)
                {

                    previousyear = txtFromDate.Text.Split('-').GetValue(1).ToString();
                    presentyear = Convert.ToString(Convert.ToInt32(txtFromDate.Text.Split('-').GetValue(1)) + 1);
                }
                else
                {
                    previousyear = Convert.ToString(Convert.ToInt32(txtFromDate.Text.Split('-').GetValue(1)) - 1);
                    presentyear = txtFromDate.Text.Split('-').GetValue(1).ToString();
                }

                string financialyear = previousyear + "-" + presentyear;

                string sParam = "id=Detailsviewaddedabstract&presentyear=" + presentyear + "&previosyear=" + previousyear + "&month=" + txtFromDate.Text.Trim() + "&previousmonth=" + previousmonth + "&financialyear=" + financialyear + "";
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