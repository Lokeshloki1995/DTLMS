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
    public partial class DTRFailureAbstract : System.Web.UI.Page
    {
        string Officecode = string.Empty;
        string strFormCode = "DTRFailureAbstract";
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
                    CalendarExtender1.EndDate = System.DateTime.Now;

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
                if (txtFromDate.Text == "")
                {
                    ShowMsgBox("Please Select the Month");
                    txtFromDate.Focus();
                    return;

                }
                
                string strParam = "id=DTRFailureAbstract&Month=" + txtFromDate.Text;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

            }
            catch (Exception ex)
            {

                //clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

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