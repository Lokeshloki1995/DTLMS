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
    public partial class Frequentlyfailed : System.Web.UI.UserControl
    {
        string strFormCode = "Frequentlyfailed.ascx";
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

                    LoadFrequentlyfailed();
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        public void LoadFrequentlyfailed()
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
                dt = objdashbord.Loadfrequentlyfailedgraph(objdashbord);

                List<string> officename = (from p in dt.AsEnumerable()
                                           select p.Field<string>("OFF_NAME")).Distinct().ToList();


                string[] x = new string[dt.Rows.Count];
                int?[] y = new int?[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    x[i] = dt.Rows[i][1].ToString();
                    y[i] = Convert.ToInt32(dt.Rows[i][2]);
                }

                Frequentlyfailedchart.Series[0].IsValueShownAsLabel = true;


                Frequentlyfailedchart.Series[0].Points.DataBindXY(x, y);
                Frequentlyfailedchart.Series[0].ChartType = SeriesChartType.Pie;
                Frequentlyfailedchart.Series[0].ToolTip = "(#VALY)count(#VALX)";
                //Frequentlyfailedchart.Series[0].LabelFormat = "{#}%";
                Frequentlyfailedchart.Series[0]["PixelPointWidth"] = "20";


                Frequentlyfailedchart.Legends[0].Enabled = true;




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
                LoadFrequentlyfailed();
            }

            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void view_link_click(object sender, EventArgs e)
        {

            string url = "/ViewDetailsgrid.aspx?RefId=true&Gridid=" + "Frequentlyfailed";
            string s = "window.open('" + url + "','_blank');"; 
            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);

        }
    }
}