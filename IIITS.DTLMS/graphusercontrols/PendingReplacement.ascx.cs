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
    public partial class PendingReplacement : System.Web.UI.UserControl
    {
        string strFormCode = "PendingReplacement.ascx";
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
                            Genaral.Load_Combo("SELECT \"SM_ID\",\"SM_NAME\" from \"TBLSTOREMAST\",\"TBLSTOREOFFCODE\"  WHERE \"STO_SM_ID\"=\"SM_ID\" and \"SM_ID\"='" + objSession.OfficeCode + "' order by \"STO_OFF_CODE\" ", "--Select--", cmbDivisions);


                            // divdropdwn.Visible = true;
                        }
                        else
                        {
                            Genaral.Load_Combo("SELECT DISTINCT \"SM_ID\",\"SM_NAME\" from \"TBLSTOREMAST\",\"TBLSTOREOFFCODE\"  WHERE \"STO_SM_ID\"=\"SM_ID\" and cast(\"SM_ID\" as text) like '%' order by \"SM_ID\" ", "--Select--", cmbDivisions);

                            // divdropdwn.Visible = false;
                        }

                    }
                    LoadPendingReplacementDetails();
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        public void LoadPendingReplacementDetails()
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dtadmin = new DataTable();
                clsDashboardUcForms objdashbord = new clsDashboardUcForms();

                if (objSession.sRoleType == "2")
                {
                    if (cmbDivisions.SelectedValue == "--Select--")
                    {
                        objdashbord.officecode = objSession.OfficeCode;
                        //clsStoreOffice.GetZone_Circle_Div_Offcode(objSession.OfficeCode, objSession.RoleId);
                    }
                    else
                    {
                        objdashbord.officecode = cmbDivisions.SelectedValue;

                    }

                }
                else
                {
                    if (cmbDivisions.SelectedValue == "--Select--")
                    {
                        objdashbord.officecode = "2";
                    }
                    else
                    {
                        objdashbord.officecode = cmbDivisions.SelectedValue;

                    }
                }

                objdashbord.roleType = objSession.sRoleType;
                objdashbord.roleId = objSession.RoleId;

                dt = objdashbord.LoadPendingReplacementDetails(objdashbord);

                List<string> officename = (from p in dt.AsEnumerable()
                                           select p.Field<string>("OFFICE_NAME")).Distinct().ToList();

                List<string> status = (from p in dt.AsEnumerable()
                                       select p.Field<string>("STATUS")).Distinct().ToList();

                string[] x = new string[dt.Rows.Count];
                int?[] y = new int?[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    x[i] = dt.Rows[i][2].ToString();
                    y[i] = Convert.ToInt32(dt.Rows[i][4]);
                }


                //foreach (string office in status)
                //{

                //    string[] x = (from p in dt.AsEnumerable()
                //                  where p.Field<string>("STATUS") == office
                //                  select p.Field<string>("OFFICE_NAME")).ToArray();

                //    int?[] y = (from p in dt.AsEnumerable()
                //                where p.Field<string>("STATUS") == office
                //                select p.Field<int?>("TOTAL_COUNT")).ToArray();


                //    PendingReplacements.Series.Add(new Series(office));

                    PendingReplacements.Series[0].IsValueShownAsLabel = true;
                    PendingReplacements.Series[0].Points.DataBindXY(x, y);
                    PendingReplacements.Series[0].ChartType = SeriesChartType.Pie;
                    PendingReplacements.Series[0].ToolTip = "(#VALY)count(#VALX)";

                    PendingReplacements.Series[0]["PixelPointWidth"] = "20";

                    PendingReplacements.ChartAreas[0].AxisX.Title = "Office Name";

                    PendingReplacements.ChartAreas[0].AxisY.Title = "Tc Count";
                    PendingReplacements.Series[0].SmartLabelStyle.Enabled = false;


                //}

                PendingReplacements.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
               // PendingReplacements.ChartAreas["ChartArea1"].Area3DStyle.Inclination = 20;

                PendingReplacements.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                PendingReplacements.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;

                PendingReplacements.Legends[0].Enabled = true;

                Axis xaxis = PendingReplacements.ChartAreas[0].AxisX;
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
                LoadPendingReplacementDetails();
            }

            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void view_link_click(object sender, EventArgs e)
        {

            string url = "/ViewDetailsgrid.aspx?RefId=true&Gridid=" + "PendingReplacement";
            string s = "window.open('" + url + "','_blank');";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }
    }
}