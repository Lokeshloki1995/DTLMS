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
    public partial class Frequentlyfaileddtr : System.Web.UI.UserControl
    {
        string strFormCode = "Frequentlyfaileddtr.ascx";
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
                            Genaral.Load_Combo("SELECT \"STO_OFF_CODE\",\"DIV_NAME\" from \"TBLSTOREMAST\",\"TBLSTOREOFFCODE\",\"TBLDIVISION\"  WHERE \"STO_SM_ID\"=\"SM_ID\" AND \"DIV_CODE\"=\"STO_OFF_CODE\" and \"SM_ID\"='" + objSession.OfficeCode + "' order by \"STO_OFF_CODE\" ", "--Select--", cmbDivisions);


                            divdropdwn.Visible = true;
                        }
                        else
                        {

                            divdropdwn.Visible = false;
                        }


                    }

                    LoadFrequentlyfaileddtr();
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        public void LoadFrequentlyfaileddtr()
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
                dt = objdashbord.Loadfrequentlyfaileddtrgraph(objdashbord);

                List<string> capacitytype = (from p in dt.AsEnumerable()
                                           select p.Field<string>("CAPACITY")).Distinct().ToList();


              
                string[] x = new string[] { };
                int?[] y = new int?[] { };

                foreach (string capacity in capacitytype)
                {


                    x = (from p in dt.AsEnumerable()
                         where p.Field<string>("CAPACITY") == capacity
                         select p.Field<string>("OFF_NAME")).ToArray();


                    y = (from p in dt.AsEnumerable()
                         where p.Field<string>("CAPACITY") == capacity
                         select p.Field<int?>("TOTAL_COUNT")).ToArray();

                    Frequentlyfaileddtrchart.Series.Add(new Series(capacity));

                    Frequentlyfaileddtrchart.Series[capacity].IsValueShownAsLabel = true;
                    Frequentlyfaileddtrchart.Series[capacity].Points.DataBindXY(x, y);


                    Frequentlyfaileddtrchart.Series[capacity].ChartType = SeriesChartType.StackedColumn;
                    Frequentlyfaileddtrchart.Series[capacity].ToolTip = "(#VALY)count on (" + capacity + ")";

                   // Frequentlyfaileddtrchart.Series[capacity].Points[0].MapAreaAttributes = "onmouseover=\"showTooltip('#VALX',#VALY,event);\"";


                    Frequentlyfaileddtrchart.ChartAreas[0].AxisX.Title = "Location Name";

                    Frequentlyfaileddtrchart.ChartAreas[0].AxisY.Title = "No. of DTR";

                    Frequentlyfaileddtrchart.Series[capacity].SmartLabelStyle.Enabled = false;


                }



               // Frequentlyfaileddtrchart.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
                //Frequentlyfaileddtrchart.ChartAreas["ChartArea1"].Area3DStyle.IsClustered = true;


                Frequentlyfaileddtrchart.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                Frequentlyfaileddtrchart.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;

                Frequentlyfaileddtrchart.Legends[0].Enabled = true;

                Axis xaxis = Frequentlyfaileddtrchart.ChartAreas[0].AxisX;
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
                LoadFrequentlyfaileddtr();
            }

            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void view_link_click(object sender, EventArgs e)
        {


            string url = "/ViewDetailsgrid.aspx?RefId=true&Gridid=" + "Frequentlyfaileddtr";
            string s = "window.open('" + url + "','_blank');"; // '_newtab'
            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);


        }
    }
}