using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.UI.DataVisualization.Charting;
using System.Data;
using IIITS.DTLMS.BL;
using System.Drawing;


namespace IIITS.DTLMS.graphusercontrols
{
    public partial class Pograph : System.Web.UI.UserControl
    {
        string strFormCode = "Pograph.ascx";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            clsDashboardUcForms objDashboard = new clsDashboardUcForms();
            try
            {
                viewstatus.ServerClick += new EventHandler(view_link_click);
                string stroffCode = string.Empty;
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }

                else
                {
                    objSession = (clsSession)Session["clsSession"];
                    if (!IsPostBack)
                    {

                    }
                    LoadPoDetails();
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        public void LoadPoDetails()
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dtadmin = new DataTable();
                clsDashboardUcForms objdashbord = new clsDashboardUcForms();

                objdashbord.roleType = objSession.sRoleType;
                objdashbord.roleId = objSession.RoleId;

                dt = objdashbord.LoadPoDetails(objdashbord);

                string[] XPointMember = new string[dt.Rows.Count];
                string[] XPointMember1 = new string[dt.Rows.Count];
                int[] YPointMember = new int[dt.Rows.Count];
                int[] YPointMember1 = new int[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    XPointMember[i] = Convert.ToString(dt.Rows[i]["PREVIOUSYEAR"]);

                    YPointMember[i] = Convert.ToInt32(dt.Rows[i]["PREVIOUS_PO_AMOUNT"]);
                    XPointMember1[i] = Convert.ToString(dt.Rows[i]["PRESENTYEAR"]);

                    YPointMember1[i] = Convert.ToInt32(dt.Rows[i]["PRESENT_PO_AMOUNT"]);
                }


                var PresentYear = Convert.ToString(dt.Rows[0]["PRESENTYEAR"]);
                var PreviousYear = Convert.ToString(dt.Rows[0]["PREVIOUSYEAR"]);
                Series series = new Series();


                pograph.Series[0].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
                pograph.Series[1].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
                pograph.Series[0].ToolTip = "(#VALY)Failure on (#VALX " + PresentYear + ")";
                pograph.Series[1].ToolTip = "(#VALY)Failure on (#VALX " + PreviousYear + ")";
                pograph.Series[0].Points.DataBindXY(XPointMember, YPointMember);
                pograph.Series[1].Points.DataBindXY(XPointMember1, YPointMember1);


                pograph.Series[0]["PixelPointWidth"] = "50";
                pograph.Series[1]["PixelPointWidth"] = "50";

                pograph.Titles["NewTitle"].Text = "";

                pograph.Series[0].LegendText = "Previous Year";
                pograph.Series[1].LegendText = "Current Year";

                pograph.Width = 1000;
                Axis xaxis = pograph.ChartAreas[0].AxisX;

                xaxis.Interval = 1;
                Axis yaxis = pograph.ChartAreas[0].AxisY;

                pograph.ChartAreas[0].AxisX.Title = "Years";

                pograph.ChartAreas[0].AxisY.Title = "";
                pograph.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                pograph.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;

                pograph.Legends["Legend1"].Docking = Docking.Bottom;
                pograph.Legends["Legend1"].DockedToChartArea = "ChartArea1";
                pograph.Legends["Legend1"].IsDockedInsideChartArea = false;
                series.IsValueShownAsLabel = true;

                pograph.Series[0].IsValueShownAsLabel = true;
                pograph.Series[1].IsValueShownAsLabel = true;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        

        protected void view_link_click(object sender, EventArgs e)
        {

            string url = "/ViewDetailsgrid.aspx?RefId=true&Gridid=" + "Po";
            string s = "window.open('" + url + "','_blank');";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }
    }
}