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
    public partial class TcStatus : System.Web.UI.UserControl
    {


        string strFormCode = "TcStatus.ascx";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            clsDashboardUcForms objDashboard = new clsDashboardUcForms();
            try
            {
                viewstatus.ServerClick += new EventHandler(view_link_click);
                viewstatus1.ServerClick += new EventHandler(view_link_click);
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
                    LoadTcStatusDetails();
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        public void LoadTcStatusDetails()
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dtadmin = new DataTable();
                clsDashboardUcForms objdashbord = new clsDashboardUcForms();

                objdashbord.officecode = objSession.OfficeCode;

                objdashbord.roleType = objSession.sRoleType;
                objdashbord.roleId = objSession.RoleId;

                if (objdashbord.roleType == "3") 
                {
                    dtadmin = objdashbord.LoadTcStatusDetailsadmin(objdashbord);
                    List<string> statusadmin = (from p in dtadmin.AsEnumerable()
                                                select p.Field<string>("STATUS")).Distinct().ToList();

                   
                    foreach (string office in statusadmin)
                    {

                        string[] x = (from p in dtadmin.AsEnumerable()
                                      where p.Field<string>("STATUS") == office
                                      select p.Field<string>("OFF_NAME")).ToArray();

                        int?[] y = (from p in dtadmin.AsEnumerable()
                                    where p.Field<string>("STATUS") == office
                                    select p.Field<int?>("TOTAL_COUNT")).ToArray();


                        TcStatusStore.Series.Add(new Series(office));
                       
                        TcStatusStore.Series[office].IsValueShownAsLabel = true;
                        TcStatusStore.Series[office].Points.DataBindXY(x, y);
                        TcStatusStore.Series[office].ChartType = SeriesChartType.StackedColumn;
                        TcStatusStore.Series[office].ToolTip = "(#VALY)count(" + office + ")";

                        TcStatusStore.Series[office]["PixelPointWidth"] = "15";

                        TcStatusStore.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = true;
                        TcStatusStore.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = true;

                        TcStatusStore.Series[office].SmartLabelStyle.Enabled = false;
        
                        TcStatusStore.Legends["Legend1"].Docking = Docking.Bottom;
                        TcStatusStore.Legends["Legend1"].DockedToChartArea = "ChartArea1";
                        TcStatusStore.Legends["Legend1"].IsDockedInsideChartArea = false;

                        TcStatusStore.ChartAreas[0].AxisX.Title = "Office Name";

                        TcStatusStore.ChartAreas[0].AxisY.Title = "Tc Count";

                        TcStatusStore.Titles["NewTitle"].Text = "Store Wise";
                       

                    }


                   // TcStatusStore.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
                    //TcStatusStore.ChartAreas["ChartArea1"].Area3DStyle.IsClustered = true;

                    TcStatusStore.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                    TcStatusStore.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;

                    TcStatusStore.Legends[0].Enabled = true;

                    Axis xaxisadmin = TcStatusStore.ChartAreas[0].AxisX;
                    xaxisadmin.Interval = 1;
                }


                dt = objdashbord.LoadTcStatusDetails(objdashbord);

                List<string> officename = (from p in dt.AsEnumerable()
                                           select p.Field<string>("OFF_NAME")).Distinct().ToList();

                List<string> status = (from p in dt.AsEnumerable()
                                       select p.Field<string>("STATUS")).Distinct().ToList();

               

                foreach (string office in status)
                {

                    string[] x = (from p in dt.AsEnumerable()
                                  where p.Field<string>("STATUS") == office
                                  select p.Field<string>("OFF_NAME")).ToArray();

                    int?[] y = (from p in dt.AsEnumerable()
                                where p.Field<string>("STATUS") == office
                                select p.Field<int?>("TOTAL_COUNT")).ToArray();


                    TcStatusfield.Series.Add(new Series(office));

                    TcStatusfield.Series[office].IsValueShownAsLabel = true;
                    TcStatusfield.Series[office].Points.DataBindXY(x, y);
                    TcStatusfield.Series[office].ChartType = SeriesChartType.StackedColumn;
                    TcStatusfield.Series[office].ToolTip = "(#VALY)count(" + office + ")";

                    TcStatusfield.Series[office]["PixelPointWidth"] = "15";

                    TcStatusfield.ChartAreas[0].AxisX.Title = "Office Name";

                    TcStatusfield.ChartAreas[0].AxisY.Title = "Tc Count";

                    TcStatusfield.Series[office].SmartLabelStyle.Enabled = false;

                    TcStatusfield.Series[office].CustomProperties = "DrawSideBySide=True";
                    TcStatusfield.Series[office]["StackedGroupName"] = office;

                }

                if (objSession.sRoleType == "2")
                {
                    TcStatusfield.Titles["NewTitle"].Text = "Store Wise";

                }
                else
                {

                    TcStatusfield.Titles["NewTitle"].Text = "Field Wise";
                }

                if (objSession.sRoleType == "1" || objSession.sRoleType == "2")
                {

                    TcStatusStores.Visible = false;
                }

                TcStatusfield.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                TcStatusfield.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;

                TcStatusfield.Legends[0].Enabled = true;

                Axis xaxis = TcStatusfield.ChartAreas[0].AxisX;
                xaxis.Interval = 1;



               


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
                LoadTcStatusDetails();
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void view_link_click(object sender, EventArgs e)
        {

            string url = "/ViewDetailsgrid.aspx?RefId=true&Gridid=" + "TcStatus";
            string s = "window.open('" + url + "','_blank');"; 
            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }
    }
}