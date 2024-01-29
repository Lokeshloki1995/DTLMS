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
    public partial class Alternativepowersupply : System.Web.UI.UserControl
    {
        string strFormCode = "Alternativepowersupply.ascx";
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
                        if (objSession.sRoleType == "2")
                        {
                            Genaral.Load_Combo("SELECT \"STO_OFF_CODE\",\"SM_NAME\" from \"TBLSTOREMAST\",\"TBLSTOREOFFCODE\"  WHERE \"STO_SM_ID\"=\"SM_ID\" and \"SM_ID\"='" + objSession.OfficeCode + "' order by \"STO_OFF_CODE\" ", "--Select--", cmbDivisions);


                            divdropdwn.Visible = true;
                        }
                        else
                        {

                            divdropdwn.Visible = false;
                        }


                    }

                    LoadAlternativeDetails();
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        public void LoadAlternativeDetails()
        {
            try
            {
                DataTable dt = new DataTable();
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
                dt = objdashbord.LoadAlternativeDetailsgraph(objdashbord);

                List<string> officename = (from p in dt.AsEnumerable()
                                           select p.Field<string>("STATUS")).Distinct().ToList();

                
                string[] x = new string[] { };
                int?[] y = new int?[] { };
        
                foreach (string office in officename)
                {


                     x = (from p in dt.AsEnumerable()
                                  where p.Field<string>("STATUS") == office
                                  select p.Field<string>("OFF_NAME")).ToArray();

                    y = (from p in dt.AsEnumerable()
                                where p.Field<string>("STATUS") == office
                                select p.Field<int?>("TOTAL_COUNT")).ToArray();

                    Alternativepowersupplies.Series.Add(new Series(office));

                    Alternativepowersupplies.Series[office].IsValueShownAsLabel = true;
                    Alternativepowersupplies.Series[office].Points.DataBindXY(x, y);

                   


                   Alternativepowersupplies.Series[office].ChartType = SeriesChartType.StackedColumn;
                    Alternativepowersupplies.Series[office].ToolTip = "(#VALY)Count on(#VALX)";

                    Alternativepowersupplies.Series[office]["PixelPointWidth"] = "20";

                    Alternativepowersupplies.ChartAreas[0].AxisX.Title = "Office Name";

                    Alternativepowersupplies.ChartAreas[0].AxisY.Title = "Tc Count";
                    Alternativepowersupplies.Series[office].SmartLabelStyle.Enabled = false;



                }

                Alternativepowersupplies.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
                Alternativepowersupplies.ChartAreas["ChartArea1"].Area3DStyle.Inclination = 20;

                Alternativepowersupplies.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                Alternativepowersupplies.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;

                Alternativepowersupplies.Legends[0].Enabled = true;

                Axis xaxis = Alternativepowersupplies.ChartAreas[0].AxisX;
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
                LoadAlternativeDetails();
            }

            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void view_link_click(object sender, EventArgs e)
        {

            string url = "/ViewDetailsgrid.aspx?RefId=true&Gridid=" + "Alternativepowersupply";
            string s = "window.open('" + url + "','_blank');"; // '_newtab'
            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);

        }
    }
}