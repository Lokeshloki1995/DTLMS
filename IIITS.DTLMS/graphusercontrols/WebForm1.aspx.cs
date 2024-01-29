using IIITS.DTLMS.BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.graphusercontrols
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        string strFormCode = "FailureDetails.ascx";
        clsSession objSession;
        List<clsDashboardUcForms> Total = new List<clsDashboardUcForms>();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static List<clsDashboardUcForms> GetChartData()
        {
            DataTable dt = new DataTable();
            clsDashboardUcForms objdashbord = new clsDashboardUcForms();

            // objSession = (clsSession)Session["clsSession"];

            objdashbord.officecode = "2";

            objdashbord.roleType = "3";
            objdashbord.roleId = "8";
            dt = objdashbord.LoadRepairerPerformanceDetails(objdashbord);


            List<string> officename = (from p in dt.AsEnumerable()
                                       select p.Field<string>("OFFICE_NAME")).Distinct().ToList();

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    WebUserControl1 student = new WebUserControl1();
            //    student.CountryId = Convert.ToInt32(dt.Rows[i]["PRESENTCOUNT"]);
            //    student.CountryName = dt.Rows[i]["MONTHS"].ToString();

            //    CountryInformation.Add(student);
            //}  

            List<clsDashboardUcForms> vehicles = new List<clsDashboardUcForms>();
            foreach (DataRow dr in dt.Rows)
            {
                vehicles.Add(new clsDashboardUcForms
                {
                    Age = dr["STATUS"].ToString(),
                    AgeCount = Convert.ToInt32(dr["TOTAL_COUNT"]),

                });
            }

             return vehicles;

           // return new JavaScriptSerializer().Serialize(vehicles);

        }
        [WebMethod]
        public List<clsDashboardUcForms> LoadFailureDetails()
        {
            try
            {
                DataTable dt = new DataTable();
                clsDashboardUcForms objdashbord = new clsDashboardUcForms();

                objSession = (clsSession)Session["clsSession"];

                objdashbord.officecode = objSession.OfficeCode;

                objdashbord.roleType = objSession.sRoleType;
                objdashbord.roleId = objSession.RoleId;
                dt = objdashbord.LoadFailureDetailsstackgraph1(objdashbord);


                List<string> officename = (from p in dt.AsEnumerable()
                                           select p.Field<string>("OFF_NAME")).Distinct().ToList();

              


                List<clsDashboardUcForms> vehicles = new List<clsDashboardUcForms>();
                foreach (DataRow dr in dt.Rows)
                {
                    vehicles.Add(new clsDashboardUcForms
                    {
                        Age = dr["MONTHS"].ToString(),
                        AgeCount = Convert.ToInt32(dr["PRESENTCOUNT"]),

                    });
                }

                return vehicles;

                //return new JavaScriptSerializer().Serialize(vehicles);

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Total;
            }
        }
       
    }
}