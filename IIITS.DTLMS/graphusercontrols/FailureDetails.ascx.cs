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
    public partial class FailureDetails : System.Web.UI.UserControl
    {
        string strFormCode = "FailureDetails.ascx";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            clsDashboardUcForms objDashboard = new clsDashboardUcForms();
            try
            {
                viewstatus.ServerClick += new EventHandler(view_link_click);
                viewstatuspi.ServerClick += new EventHandler(view_link_click);
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
                        if (objSession.sRoleType=="2")
                        {
                            Genaral.Load_Combo("SELECT \"STO_OFF_CODE\",\"DIV_NAME\" from \"TBLSTOREMAST\",\"TBLSTOREOFFCODE\",\"TBLDIVISION\"  WHERE \"STO_SM_ID\"=\"SM_ID\" AND \"DIV_CODE\"=\"STO_OFF_CODE\" and \"SM_ID\"='" + objSession.OfficeCode + "' order by \"STO_OFF_CODE\" ", "--Select--", cmbDivisions);
                          
                           
                            divdropdwn.Visible = true;
                           // clsStoreOffice.GetOfficeCode(objSession.OfficeCode, "US_OFFICE_CODE");
                        }
                        else
                        {

                            divdropdwn.Visible = false;
                        }
                        
                        
                    }

                    LoadFailureDetails();
                }
            }
            catch (Exception ex)
            {
                // lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        public void LoadFailureDetails()
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dtpiechart = new DataTable();
                clsDashboardUcForms objdashbord = new clsDashboardUcForms();
                if (objSession.sRoleType == "2")
                {
                    if (cmbDivisions.SelectedValue == "--Select--")
                    {
                        objdashbord.officecode = clsStoreOffice.GetZone_Circle_Div_Offcode(objSession.OfficeCode, objSession.RoleId);
                    }
                    else
                    {
                        objdashbord.officecode = cmbDivisions.SelectedValue;

                    }
                    
                }
                else
                {
                    objdashbord.officecode = objSession.OfficeCode;
                }
                objdashbord.roleType = objSession.sRoleType;
                objdashbord.roleId = objSession.RoleId;
                dt = objdashbord.LoadFailureDetailsstackgraph(objdashbord);

                List<string> officename = (from p in dt.AsEnumerable()
                                          select p.Field<string>("OFF_NAME")).Distinct().ToList();

                dtpiechart = objdashbord.LoadFailureDetailsstackgraphpie(objdashbord);

                string[] x1 = new string[dtpiechart.Rows.Count];
                int?[] y1 = new int?[dtpiechart.Rows.Count];

                for (int i = 0; i < dtpiechart.Rows.Count; i++)
                {
                    x1[i] = dtpiechart.Rows[i][1].ToString();
                    y1[i] = Convert.ToInt32(dtpiechart.Rows[i][4]);
                }

                //foreach (DataPoint p in FailureDetailschartpie.Series[0].Points)
                //{
                //    p.AxisLabel = "#PERCENT\n#VALY";
                //}

                //FailureDetailschartpie.Series[0].AxisLabel= "#VALY" + "%";
               // FailureDetailschartpie.ChartAreas[0].AxisY.LabelStyle.Format = "{#VALY}%";
                FailureDetailschartpie.Series[0].IsValueShownAsLabel = true;
               // FailureDetailschartpie.Series[0].AxisLabel = "#PERCENT\n#VALY";

               

                FailureDetailschartpie.Series[0].Points.DataBindXY(x1, y1);
                FailureDetailschartpie.Series[0].ChartType = SeriesChartType.Pie;
                FailureDetailschartpie.Series[0].ToolTip = "(#VALY)count(#VALX)";
                FailureDetailschartpie.Series[0].LabelFormat = "{#}%";
                FailureDetailschartpie.Series[0]["PixelPointWidth"] = "20";

                //FailureDetailschartpie.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;

                FailureDetailschartpie.Legends[0].Enabled = true;

                //Loop through the zones.
                #region single
                string[] x = new string[] { };
                int?[] y = new int?[] { };
 
                foreach (string office in officename)
                {


                    x = (from p in dt.AsEnumerable()
                         where p.Field<string>("OFF_NAME") == office
                         select p.Field<string>("MONTHS")).ToArray();


                    y = (from p in dt.AsEnumerable()
                         where p.Field<string>("OFF_NAME") == office
                         select p.Field<int?>("PRESENTCOUNT")).ToArray();

                    FailureDetailschart.Series.Add(new Series(office));

                    FailureDetailschart.Series[office].IsValueShownAsLabel = true;
                    FailureDetailschart.Series[office].Points.DataBindXY(x, y);


                    FailureDetailschart.Series[office].ChartType = SeriesChartType.StackedColumn;
                    FailureDetailschart.Series[office].ToolTip = "(#VALY)failure on(#VALX)";

                    FailureDetailschart.ChartAreas[0].AxisX.Title = "Month";

                    FailureDetailschart.ChartAreas[0].AxisY.Title = "No. of DTR Failure";

                    FailureDetailschart.Series[office].SmartLabelStyle.Enabled = false;


                }



                //FailureDetailschart.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
                FailureDetailschart.ChartAreas["ChartArea1"].Area3DStyle.IsClustered = true;


                FailureDetailschart.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                FailureDetailschart.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;

                FailureDetailschart.Legends[0].Enabled = true;

                Axis xaxis = FailureDetailschart.ChartAreas[0].AxisX;
                // xaxis.IntervalOffsetType = DateTimeIntervalType.Months;
                xaxis.Interval = 1;
              

                #endregion 

                #region old
              //  string[] XPointMember = new string[dt.Rows.Count];
              //  string[] XPointMember1 = new string[dt.Rows.Count];
              //  int?[] YPointMember = new int?[dt.Rows.Count];
              //  int?[] YPointMember1 = new int?[dt.Rows.Count];
              //  //int[] YPointMember2 = new int[dtBarGraph.Rows.Count];

              //  for (int i = 0; i < dt.Rows.Count; i++)
              //  {

              //      XPointMember[i] = Convert.ToString(dt.Rows[i]["MONTHS"]);

              //      YPointMember[i] = Convert.ToInt32(dt.Rows[i]["PRESENTCOUNT"]);
              //      XPointMember1[i] = Convert.ToString(dt.Rows[i]["MONTHS"]);

              //      YPointMember1[i] = Convert.ToInt32(dt.Rows[i]["PREVIOUSCOUNT"]);
              //  }

              ////  var PresentYear = Convert.ToString(dt.Rows[0]["PRESENTYEAR"]);
              // // var PreviousYear = Convert.ToString(dt.Rows[0]["PREVIOUSYEAR"]);

              //  //series.ChartType = SeriesChartType.Bar;
              //  FailureDetailschart.Series[1].IsValueShownAsLabel = true;
              //  FailureDetailschart.Series[0].IsValueShownAsLabel = true;
              //  FailureDetailschart.Legends[1].Enabled = true;
              //  FailureDetailschart.Legends[0].Enabled = true;
              //  FailureDetailschart.Series[1].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.StackedColumn;
              //  FailureDetailschart.Series[0].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.StackedColumn;
              //  //FailureDetailschart.Series[1].ToolTip = "(#VALY)Failure on (#VALX " + PresentYear + ")";
              // // FailureDetailschart.Series[0].ToolTip = "(#VALY)Failure on (#VALX " + PreviousYear + ")";
              //  FailureDetailschart.Series[1].Points.DataBindXY(XPointMember, YPointMember);
              //  FailureDetailschart.Series[0].Points.DataBindXY(XPointMember1, YPointMember1);
              //  Axis xaxis = FailureDetailschart.ChartAreas[0].AxisX;
              //  //xaxis.IntervalOffsetType = DateTimeIntervalType.Months;
              //  xaxis.Interval = 1;
                #endregion





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
                LoadFailureDetails();
            }
            
                catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            
        }

        protected void view_link_click(object sender, EventArgs e)
        {
           
           // ViewDetailsgrid.ascx?RefId=true&Gridid=FailureDetails




            string url = "/ViewDetailsgrid.aspx?RefId=true&Gridid=" + "FailureDetails";
            string s = "window.open('" + url + "','_blank');"; // '_newtab'
            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);

            //Page.Response.Redirect(url);


            //var uc = (ViewDetailsgrid)Page.LoadControl("~/ViewDetailsgrid.ascx?RefId=true&Gridid=" + "FailureDetails");
            
           
        }
    }
}