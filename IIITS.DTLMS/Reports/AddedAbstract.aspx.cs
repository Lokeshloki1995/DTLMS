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
    public partial class AddedAbstract : System.Web.UI.Page
    {
        string Officecode = string.Empty;
        string strFormCode = "AddedAbstract";
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
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
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

                DateTime previosmonthdt = Convert.ToDateTime(month).AddMonths(-1);


               
                string previousmonth = Convert.ToDateTime(previosmonthdt).ToString("MM-yyyy");


                dt = objrep.Getabstractfprevadded(previousmonth);

                dt1 = objrep.Getabstractfadded(month);

                dt2 = objrep.Getabstractfpresentadded(month);


                dt1.Columns.Add("No.of DTs existing as at the end of previous month");
                dt1.Columns.Add("Total No. of DTs as at the end of Present Month");

                dt1.Rows[0]["No.of DTs existing as at the end of previous month"] = dt.Rows[0]["No.of DTs existing as at the end of previous month"];
                dt1.Rows[0]["Total No. of DTs as at the end of Present Month"] = dt2.Rows[0]["Total No. of DTs as at the end of Present Month"];
                //dt1.Rows.Add(dt.Rows[0]["No.of DTs existing as at the end of previous month"]);

                //dt1.Rows.Add(dt2.Rows[0]["Total No. of DTs as at the end of Present Month"]);


                dt1.Columns["No.of DTs existing as at the end of previous month"].SetOrdinal(1);

                List<string> listtoRemove = new List<string> { "" };
                string filename = "Added.xls";
                string pagetitle = "Details of Distribution Transformers Added for the month of '"+ month + "'";

                Genaral.getexcel(dt1, listtoRemove, filename, pagetitle);


            }
            catch (Exception ex)
            {

                //clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

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
                if (cmbFinYear.Text == "")
                {
                    ShowMsgBox("Please Select The Financial Year");
                    cmbFinYear.Focus();
                    return;

                }

                string previousyear = cmbFinYear.SelectedValue.Split('-').GetValue(0).ToString();
                string presentyear = cmbFinYear.SelectedValue.Split('-').GetValue(1).ToString();

                dt = objrep.Getabstractadded(previousyear, presentyear);


                List<string> listtoRemove = new List<string> { "" };
                string filename = "AddedFY.xls";
                string pagetitle = "Details of Distribution  Transformers added during FY   '" + cmbFinYear.SelectedValue + "'";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);



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