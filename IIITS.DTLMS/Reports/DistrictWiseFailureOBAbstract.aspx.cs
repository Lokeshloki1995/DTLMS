using System;
using System.Collections.Generic;

using IIITS.DTLMS.BL;

using System.Data;
using System.Globalization;

namespace IIITS.DTLMS.Reports
{
    public partial class DistrictWiseFailureOBAbstract : System.Web.UI.Page
    {
        string strFormCode = "DistrictWiseFailureOBAbstract";
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
                if (!IsPostBack)
                {
                    txtFromDate.Attributes.Add("readonly", "readonly");
                    CalendarExtender3.EndDate = System.DateTime.Now.AddDays(0);
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
                objrep.sCurrentMonth = txtFromDate.Text;

                if (Convert.ToInt32(txtFromDate.Text.Split('-').GetValue(0)) > 3)
                {

                    objrep.sFromDate = "01-04-" + txtFromDate.Text.Split('-').GetValue(1).ToString();
                    
                }
                else
                {
                    objrep.sFromDate = "01-04-" + Convert.ToString(Convert.ToInt32(txtFromDate.Text.Split('-').GetValue(1)) - 1);
                   
                }

                dt =  objrep.GetDistrcitWiseFailureOb(objrep);

                dt.Columns["DISTRICT_NAME"].ColumnName = " District Name ";
                dt.Columns["DIVISION_NAME"].ColumnName = " Division Name ";
                dt.Columns["OPENINGBALANCECOUNT"].ColumnName = " OB Count ";
                dt.Columns["FAILEDCOUNT"].ColumnName = " Total DTR Failed ";
                dt.Columns["REPLACEDCOOUNT"].ColumnName = " Total DTR Replacaed ";
                dt.Columns["TOBEREPLACEDCOUNT"].ColumnName = " Pending For Replacement ";
                dt.Columns["DTREXISTINGCOUNT"].ColumnName = " Total Number of DTR's ";
                dt.Columns["CUMULATIVEFAILURECOUNT"].ColumnName = " Cumulative Failure ";
                dt.Columns["CUMULATIVE_PRE"].ColumnName = " Cumulative Percentage[%] ";
                dt.Columns["TRFAILEDRATE_PRE"].ColumnName = " Failed Perentage[%] ";

                
                DateTime dateTime11 = DateTime.ParseExact(txtFromDate.Text, "MM-yyyy", CultureInfo.InvariantCulture);
                string month = dateTime11.ToString("MMM-yyyy");

                List<string> listtoRemove = new List<string> { "" };
                string filename = "DitrictWiseFailureAbstract" + DateTime.Now + ".xls";
                string pagetitle = "DISTRICT WISE DISTRIBUTION TRANSFORMERS FAILURE DETAILS OF "+ month + "";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);


            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }

        }
    }
}