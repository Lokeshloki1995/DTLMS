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
    public partial class TransformerAbstarct : System.Web.UI.UserControl
    {

        string strFormCode = "TransformerAbstarct.ascx";
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
                    LoadTransformerDetails();
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        public void LoadTransformerDetails()
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
                    dtadmin = objdashbord.LoadTransformerDetailsstackgraphadmin(objdashbord);
                    int count = dtadmin.Rows.Count;
                    List<string> statusadmin = (from p in dtadmin.AsEnumerable()
                                                select p.Field<string>("STATUS")).Distinct().ToList();

                   
                    foreach (string office in statusadmin)
                    {

                        string[] x = (from p in dtadmin.AsEnumerable()
                                      where p.Field<string>("STATUS") == office
                                      select p.Field<string>("OFF_NAME")).ToArray();

                        int?[] y = (from p in dtadmin.AsEnumerable()
                                    where p.Field<string>("STATUS") == office
                                    select p.Field<int?>("COUNT")).ToArray();


                        TransformerAbstarctchartadmin.Series.Add(new Series(office));

                        TransformerAbstarctchartadmin.Series[office].IsValueShownAsLabel = true;
                      
                       
                       
                        //Palette="Berry"
                        
                        TransformerAbstarctchartadmin.Series[office].Points.DataBindXY(x, y);
                        TransformerAbstarctchartadmin.Series[office].ChartType = SeriesChartType.StackedColumn;
                        TransformerAbstarctchartadmin.Series[office].ToolTip = "(#VALY)Count(" + office + ")";

                        TransformerAbstarctchartadmin.Series[office]["PixelPointWidth"] = "15";

                        TransformerAbstarctchartadmin.Series[office].SmartLabelStyle.Enabled = false;

                        //TransformerAbstarctchartadmin.Series[office].LabelAngle = -90;
                       // TransformerAbstarctchartadmin.Series[office].SmartLabelStyle.IsMarkerOverlappingAllowed = true;



                    }


                   // TransformerAbstarctchartadmin.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
                    //TransformerAbstarctchartadmin.ChartAreas["ChartArea1"].Area3DStyle.IsClustered = true;

                    TransformerAbstarctchartadmin.ChartAreas[0].AxisX.Title = "Office Name";

                    TransformerAbstarctchartadmin.ChartAreas[0].AxisY.Title = "Tc Count";


                    TransformerAbstarctchartadmin.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                    TransformerAbstarctchartadmin.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;


                    TransformerAbstarctchartadmin.Titles["NewTitle"].Text = "Store Wise";
                  
                    TransformerAbstarctchartadmin.Legends[0].Enabled = true;

                   
                    Axis xaxisadmin = TransformerAbstarctchartadmin.ChartAreas[0].AxisX;
                    xaxisadmin.Interval = 1;

                    Axis yaxis = TransformerAbstarctchartadmin.ChartAreas[0].AxisY;
                  
                }


                dt = objdashbord.LoadTransformerDetailsstackgraph(objdashbord);


                List<string> status = (from p in dt.AsEnumerable()
                                       select p.Field<string>("STATUS")).Distinct().ToList();

               

                foreach (string office in status)
                {

                    string[] x = (from p in dt.AsEnumerable()
                                  where p.Field<string>("STATUS") == office
                                  select p.Field<string>("OFF_NAME")).ToArray();

                    int?[] y = (from p in dt.AsEnumerable()
                                where p.Field<string>("STATUS") == office
                                select p.Field<int?>("COUNT")).ToArray();


                    TransformerAbstarctchart.Series.Add(new Series(office));

                    TransformerAbstarctchart.Series[office].IsValueShownAsLabel = true;
                   
                    TransformerAbstarctchart.Series[office].Points.DataBindXY(x, y);
                    TransformerAbstarctchart.Series[office].ChartType = SeriesChartType.StackedColumn;

                    
                   

                    TransformerAbstarctchart.Series[office].ToolTip = "(#VALY)Count(" + office + ")";

                    TransformerAbstarctchart.Series[office]["PixelPointWidth"] = "15";

                   TransformerAbstarctchart.Series[office].SmartLabelStyle.Enabled = false;

                   TransformerAbstarctchart.Series[office].SmartLabelStyle.IsOverlappedHidden = false;
                   TransformerAbstarctchart.Series[office].SmartLabelStyle.IsMarkerOverlappingAllowed = false;

                  // TransformerAbstarctchart.Series[office].CustomProperties = "BarLabelStyle = Bottom";// Auto, Top, Bottom, Right, Left, TopLeft, TopRight, BottomLeft, BottomRight, Center

                  // TransformerAbstarctchart.Series[office].IsValueShownAsLabel = true;

                   TransformerAbstarctchart.Series[office].SmartLabelStyle.Enabled = false;
                 //  TransformerAbstarctchart.Series[office].CustomProperties = "LabelStyle=left";
                   //TransformerAbstarctchart.Series[office].Points[0].CustomProperties = "LabelStyle=Bottom";


                  // TransformerAbstarctchart.Series[office].SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.No;

                   TransformerAbstarctchart.Series[office].SmartLabelStyle.IsMarkerOverlappingAllowed = false;

                 //  TransformerAbstarctchart.Series[office].SmartLabelStyle.MovingDirection = LabelAlignmentStyles.Right; //

                  // TransformerAbstarctchart.Series[office].LabelAngle = -30;

                  // TransformerAbstarctchart.Series[office].Font = new Font("Arial", 6, FontStyle.Regular);

                   //TransformerAbstarctchart.Series[office].CustomProperties = "LabelStyle= TopLeft";

                   TransformerAbstarctchart.Series[office].CustomProperties = "DrawSideBySide=True";
                   TransformerAbstarctchart.Series[office]["StackedGroupName"] = office;

                }
               
                

                TransformerAbstarctchart.ChartAreas[0].AxisX.Title = "Office Name";

                TransformerAbstarctchart.ChartAreas[0].AxisY.Title = "Tc Count";

                TransformerAbstarctchart.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                TransformerAbstarctchart.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;

                if (objSession.sRoleType == "2")
                {
                    TransformerAbstarctchart.Titles["NewTitle"].Text = "Store Wise";
                 
                }
                else
                {
                    TransformerAbstarctchart.Titles["NewTitle"].Text = "field Wise";
                   
                }
                if (objSession.sRoleType == "1" || objSession.sRoleType == "2")
                {

                    TransformerAbstarctchartad.Visible = false;
                }

               


                TransformerAbstarctchart.Legends[1].Enabled = true;


                
                Axis xaxis = TransformerAbstarctchart.ChartAreas[0].AxisX;
                xaxis.Interval = 1;
                Axis yaxiss = TransformerAbstarctchart.ChartAreas[0].AxisY;
              

               


            }
            catch (Exception ex)
            {
                // lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbDivisions_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadTransformerDetails();
            }
            catch (Exception ex)
            {
                // lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void view_link_click(object sender, EventArgs e)
        {

            string url = "/ViewDetailsgrid.aspx?RefId=true&Gridid=" + "TCDetails";
            string s = "window.open('" + url + "','_blank');"; // '_newtab'
            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }
    }
}