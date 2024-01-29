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
    public partial class Expenditure : System.Web.UI.UserControl
    {
        string strFormCode = "Expenditure.ascx";
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
                        
                            Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" from \"TBLDIVISION\" WHERE cast(\"DIV_CODE\" as text) like '" + objSession.OfficeCode + "%' order by \"DIV_CODE\" ", "--Select--", cmbDivisions);
                    }
                    LoadExpenditureDetails();
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        public void LoadExpenditureDetails()
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dtadmin = new DataTable();
                clsDashboardUcForms objdashbord = new clsDashboardUcForms();


               
                    if (cmbDivisions.SelectedValue == "--Select--")
                    {
                        if (objdashbord.officecode == "" || objdashbord.officecode == null)
                        {
                            objdashbord.officecode = "123";
                            cmbDivisions.SelectedValue="123";
                        }
                        else
                        {
                            objdashbord.officecode = objSession.OfficeCode;
                        }
                        
                    }
                    else
                    {
                        objdashbord.officecode = cmbDivisions.SelectedValue;

                    }

                objdashbord.roleType = objSession.sRoleType;
                objdashbord.roleId = objSession.RoleId;

                dt = objdashbord.LoadExpenditureDetails(objdashbord);


                string[] XPointMember = new string[dt.Rows.Count];
                string[] XPointMember1 = new string[dt.Rows.Count];
                int[] YPointMember = new int[dt.Rows.Count];
                int[] YPointMember1 = new int[dt.Rows.Count];
                //int[] YPointMember2 = new int[dtBarGraph.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    XPointMember[i] = Convert.ToString(dt.Rows[i]["PRESENTMONTH"]);

                    YPointMember[i] = Convert.ToInt32(dt.Rows[i]["PRESENTCOUNT"]);
                    XPointMember1[i] = Convert.ToString(dt.Rows[i]["PREVIOUSMONTH"]);

                    YPointMember1[i] = Convert.ToInt32(dt.Rows[i]["PREVIOUSCOUNT"]);
                }

                var PresentYear = Convert.ToString(dt.Rows[0]["PRESENTYEAR"]);
                var PreviousYear = Convert.ToString(dt.Rows[0]["PREVIOUSYEAR"]);
                Series series = new Series();
              

                expenditure.Series[1].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
                expenditure.Series[0].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
                expenditure.Series[1].ToolTip = "(#VALY)Failure on (#VALX " + PresentYear + ")";
                expenditure.Series[0].ToolTip = "(#VALY)Failure on (#VALX " + PreviousYear + ")";
                expenditure.Series[1].Points.DataBindXY(XPointMember, YPointMember);
                expenditure.Series[0].Points.DataBindXY(XPointMember1, YPointMember1);


                expenditure.Series[1]["PixelPointWidth"] = "50";
                expenditure.Series[0]["PixelPointWidth"] = "50";

                expenditure.Width = 1000;


                expenditure.Titles["NewTitle"].Text = "";

                expenditure.Series[0].LegendText = "Previous Year";
                expenditure.Series[1].LegendText = "Current Year";


                Axis xaxis = expenditure.ChartAreas[0].AxisX;

                xaxis.Interval = 1;
                Axis yaxis = expenditure.ChartAreas[0].AxisY;

                expenditure.ChartAreas[0].AxisX.Title = "Month";

                expenditure.ChartAreas[0].AxisY.Title = "";
                expenditure.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                expenditure.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
              
                expenditure.Legends["Legend1"].Docking = Docking.Bottom;
                expenditure.Legends["Legend1"].DockedToChartArea = "ChartArea1";
                expenditure.Legends["Legend1"].IsDockedInsideChartArea = false;
                series.IsValueShownAsLabel = true;

                expenditure.Series[1].IsValueShownAsLabel = true;
                expenditure.Series[0].IsValueShownAsLabel = true;
   


            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void cmbDivisions_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadExpenditureDetails();
            }

            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void view_link_click(object sender, EventArgs e)
        {

            string url = "/ViewDetailsgrid.aspx?RefId=true&Gridid=" + "Expenditure";
            string s = "window.open('" + url + "','_blank');";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }
    }
}