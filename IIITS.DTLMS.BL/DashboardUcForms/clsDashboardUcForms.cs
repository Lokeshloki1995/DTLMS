using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Data.SqlClient;

namespace IIITS.DTLMS.BL
{
    public class clsDashboardUcForms
    {
        string strFormCode = "clsDashboardUcForms";
        PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
        List<clsDashboardUcForms> Total = new List<clsDashboardUcForms>();

        public string Age { get; set; }
        public string Name { get; set; }
        public int? AgeCount { get; set; }
        //public static Nullable<int> AgeCount { get; set; }


       

        public string officecode { get; set; }
        public string roleId { get; set; }
        public string roleType { get; set; }

        public string CurrentDate { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }


        public string lenoffcode { get; set; }


        public DataTable LoadFailureDetailsstackgraph(clsDashboardUcForms objdashbord)
        {
            DataTable dt = new DataTable();
            try
            {
                string[] strArray = new string[3];

                int Length = 0;
                if (objdashbord.officecode == "" || objdashbord.officecode == null)
                {
                    Length = 1;
                }

                else if (objdashbord.officecode.Length > 0 && objdashbord.officecode.Length < 5)
                {
                    Length = objdashbord.officecode.Length + 1;
                }

                else if (objdashbord.officecode.Length == 5)
                {
                    Length = objdashbord.officecode.Length;
                }


                NpgsqlCommand cmd = new NpgsqlCommand("sp_failuredetails_chart");
                cmd.Parameters.AddWithValue("length_off_code", Length);
                cmd.Parameters.AddWithValue("office_Code", Convert.ToString(objdashbord.officecode));

                dt = ObjCon.FetchDataTable(cmd);
                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable LoadFailureDetailsstackgraphpie(clsDashboardUcForms objdashbord)
        {
            DataTable dt = new DataTable();
            try
            {
                string[] strArray = new string[3];

                int Length = 0;
                if (objdashbord.officecode == "" || objdashbord.officecode == null)
                {
                    Length = 1;
                }

                else if (objdashbord.officecode.Length > 0 && objdashbord.officecode.Length < 5)
                {
                    Length = objdashbord.officecode.Length + 1;
                }

                else if (objdashbord.officecode.Length == 5)
                {
                    Length = objdashbord.officecode.Length;
                }


                NpgsqlCommand cmd = new NpgsqlCommand("sp_failuredetails_chartpie");
                cmd.Parameters.AddWithValue("length_off_code", Length);
                cmd.Parameters.AddWithValue("office_Code", Convert.ToString(objdashbord.officecode));

                dt = ObjCon.FetchDataTable(cmd);
                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable LoadFailureDetailsstackgraph1(clsDashboardUcForms objdashbord)
        {
            DataTable dt = new DataTable();
            try
            {
                string[] strArray = new string[3];

                int Length = 0;
                if (objdashbord.officecode == "" || objdashbord.officecode == null)
                {
                    Length = 1;
                }

                else if (objdashbord.officecode.Length > 0 && objdashbord.officecode.Length < 5)
                {
                    Length = objdashbord.officecode.Length + 1;
                }

                else if (objdashbord.officecode.Length == 5)
                {
                    Length = objdashbord.officecode.Length;
                }


                //string qry = "SELECT \"MONTHS\",\"MON\",\"OFF_NAME\",\"DT_CODE\" AS \"DT_COUNT\",\"PRESENTYEAR\",\"PRESENTMONTH\", ";
                //qry += " cast(COALESCE(\"PRESENTCOUNT\",'0')as int)\"PRESENTCOUNT\",\"PREVIOUSYEAR\",\"PREVIOUSMONTH\", ";
                //qry += " cast(COALESCE(\"PREVIOUSCOUNT\",'0') as int)\"PREVIOUSCOUNT\" FROM (SELECT TO_CHAR(generate_series ";
                //qry += " (timestamp without time zone '2018-01-01', timestamp without time zone '2018-12-01', '1 Month'), 'MON') ";
                //qry += " \"MONTHS\",TO_CHAR(generate_series(timestamp without time zone '2018-01-01', timestamp without time ";
                //qry += " zone '2018-12-01','1 Month'),'MM') \"MON\",cast(\"OFF_CODE\" as text) FROM \"VIEW_OFFICES\" WHERE ";
                //qry += " cast(\"OFF_CODE\" as text) LIKE  '1%'  AND length(cast(\"OFF_CODE\" as text))=1)A LEFT JOIN (SELECT ";
                //qry += " cast(\"OFF_CODE\" as text),\"OFF_NAME\" FROM \"VIEW_OFFICES\")B ON A.\"OFF_CODE\"= B.\"OFF_CODE\" LEFT ";
                //qry += " JOIN (SELECT count(\"DT_CODE\")\"DT_CODE\",substr(cast(\"DT_OM_SLNO\" as text),1,1) AS \"OFF_CODE\" FROM ";
                //qry += " \"TBLDTCMAST\" GROUP BY substr(cast(\"DT_OM_SLNO\" as text),1,1))C ON C.\"OFF_CODE\"= A.\"OFF_CODE\" ";
                //qry += " LEFT JOIN (SELECT TO_CHAR(\"DF_DATE\",'YYYY') AS \"PRESENTYEAR\",substr(cast(\"DF_LOC_CODE\" as text),1,1)AS ";
                //qry += "\"OFF_CODE\",TO_CHAR(\"DF_DATE\",'MON') AS \"PRESENTMONTH\", COUNT(\"DF_DTC_CODE\") AS \"PRESENTCOUNT\" FROM ";
                //qry += " \"TBLDTCFAILURE\" WHERE to_char(\"DF_DATE\",'YYYY')=TO_CHAR(NOW(),'YYYY') GROUP BY TO_CHAR(\"DF_DATE\",'MON'), ";
                //qry += " TO_CHAR(\"DF_DATE\",'YYYY'),substr(cast(\"DF_LOC_CODE\" as text),1,1))D ON D.\"PRESENTMONTH\"=A.\"MONTHS\" ";
                //qry += " AND D.\"OFF_CODE\" = A.\"OFF_CODE\" LEFT JOIN (SELECT to_char(date_trunc('YEAR', CURRENT_DATE) - INTERVAL ";
                //qry += " '1 year','yyyy') AS \"PREVIOUSYEAR\",substr(cast(\"DF_LOC_CODE\" as text),1,1)AS \"OFF_CODE\", ";
                //qry += " TO_CHAR(\"DF_DATE\",'MON') AS \"PREVIOUSMONTH\", COUNT(\"DF_DTC_CODE\") AS \"PREVIOUSCOUNT\" FROM ";
                //qry += "\"TBLDTCFAILURE\",\"VIEW_OFFICES\" WHERE to_char(\"DF_DATE\",'YYYY')=to_char(date_trunc('YEAR', CURRENT_DATE) ";
                //qry += " - INTERVAL '1 year','yyyy')  GROUP BY TO_CHAR(\"DF_DATE\",'MON'), TO_CHAR(\"DF_DATE\",'YYYY'), ";
                //qry += " substr(cast(\"DF_LOC_CODE\" as text),1,1))E ON E.\"PREVIOUSMONTH\"=A.\"MONTHS\" and  ";
                //qry += " D.\"OFF_CODE\" = A.\"OFF_CODE\" ORDER BY \"MON\"";
                //dt = ObjCon.FetchDataTable(qry);

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_failure_details_stackgraph1");

                dt = ObjCon.FetchDataTable(cmd);


                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable LoadFailureDetailsview(clsDashboardUcForms objDashBoard)
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                string sOffcodeLength = string.Empty;


                int Length = 1;


                NpgsqlCommand cmd = new NpgsqlCommand("sp_failuredetailsview");
                cmd.Parameters.AddWithValue("length_off_code", Length);
                cmd.Parameters.AddWithValue("office_Code", Convert.ToString(objDashBoard.officecode));
                dt = ObjCon.FetchDataTable(cmd);
                return dt;


            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable LoadFailureDetailsviewLocationWise(clsDashboardUcForms objDashBoard)
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                string sOffcodeLength = string.Empty;

                int Length = 0;
                if (objDashBoard.officecode == "" || objDashBoard.officecode == null)
                {
                    Length = 1;
                }

                else if (objDashBoard.officecode.Length > 0 && objDashBoard.officecode.Length < 5)
                {
                    Length = objDashBoard.officecode.Length + 1;
                }

                else if (objDashBoard.officecode.Length == 5)
                {
                    Length = objDashBoard.officecode.Length;
                }


                NpgsqlCommand cmd = new NpgsqlCommand("sp_failuredetailsviewlocationwise");
                cmd.Parameters.AddWithValue("length_off_code", Length);
                cmd.Parameters.AddWithValue("office_Code", Convert.ToString(objDashBoard.officecode));
                dt = ObjCon.FetchDataTable(cmd);
                return dt;



            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }


        public DataTable Loadexpenditureview(clsDashboardUcForms objDashBoard)
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                string sOffcodeLength = string.Empty;


                int Length = 0;
                if (objDashBoard.officecode == "" || objDashBoard.officecode == null)
                {
                    Length = 1;
                }

                else if (objDashBoard.officecode.Length > 0 && objDashBoard.officecode.Length < 5)
                {
                    Length = objDashBoard.officecode.Length + 1;
                }

                else if (objDashBoard.officecode.Length == 5)
                {
                    Length = objDashBoard.officecode.Length;
                }

                NpgsqlCommand cmd = new NpgsqlCommand("sp_expenditure_view");
                cmd.Parameters.AddWithValue("length_off_code", Length);
                cmd.Parameters.AddWithValue("office_Code", Convert.ToString(objDashBoard.officecode));
                dt = ObjCon.FetchDataTable(cmd);
                return dt;


            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable LoadTransformerDetailsstackgraph(clsDashboardUcForms objdashbord)
        {
            DataTable dt = new DataTable();
            try
            {
                string[] strArray = new string[3];

                int Length = 0;
                if (objdashbord.officecode == "" || objdashbord.officecode == null)
                {
                    Length = 1;
                }

                else if (objdashbord.officecode.Length > 0 && objdashbord.officecode.Length < 5)
                {
                    Length = objdashbord.officecode.Length + 1;
                }

                else if (objdashbord.officecode.Length == 5)
                {
                    Length = objdashbord.officecode.Length;
                }


                if (objdashbord.roleType == "2")
                {
                    Length =3;

                    NpgsqlCommand cmd = new NpgsqlCommand("sp_storetcdetails_chart");
                    cmd.Parameters.AddWithValue("length_off_code", Length);
                    cmd.Parameters.AddWithValue("office_Code", Convert.ToString(objdashbord.officecode));
                    cmd.Parameters.AddWithValue("role_type", Convert.ToString(objdashbord.roleType));
                    dt = ObjCon.FetchDataTable(cmd);
                    return dt;
                }
                else
                {

                    NpgsqlCommand cmd = new NpgsqlCommand("sp_fieldtcdetails_chart");
                    cmd.Parameters.AddWithValue("length_off_code", Length);
                    cmd.Parameters.AddWithValue("office_Code", Convert.ToString(objdashbord.officecode));
                    cmd.Parameters.AddWithValue("role_type", Convert.ToString(objdashbord.roleType));
                    dt = ObjCon.FetchDataTable(cmd);
                    return dt;
                }

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }


        public DataTable LoadTransformerDetailsstackgraphadmin(clsDashboardUcForms objdashbord)
        {
            DataTable dt = new DataTable();
            try
            {
                string[] strArray = new string[3];

                int Length = 0;
               
                    Length = 3;
                   
                        Length = 3;

                        NpgsqlCommand cmd = new NpgsqlCommand("sp_storetcdetails_chart");
                        cmd.Parameters.AddWithValue("length_off_code", Length);
                        cmd.Parameters.AddWithValue("office_Code", Convert.ToString(objdashbord.officecode));
                        cmd.Parameters.AddWithValue("role_type", Convert.ToString(objdashbord.roleType));
                        dt = ObjCon.FetchDataTable(cmd);
                        return dt;
                    
               
                
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }


        public DataTable LoadPoDetails(clsDashboardUcForms objdashbord)
        {
            DataTable dt = new DataTable();
            try
            {

                NpgsqlCommand cmd = new NpgsqlCommand("sp_pograph_chart");
                dt = ObjCon.FetchDataTable(cmd);
                return dt;



            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
       


        public DataTable LoadTcStatusDetails(clsDashboardUcForms objdashbord)
        {
            DataTable dt = new DataTable();
            try
            {
                string[] strArray = new string[3];

                int Length = 0;
                if (objdashbord.officecode == "" || objdashbord.officecode == null)
                {
                    Length = 1;
                }

                else if (objdashbord.officecode.Length > 0 && objdashbord.officecode.Length < 5)
                {
                    Length = objdashbord.officecode.Length + 1;
                }

                else if (objdashbord.officecode.Length == 5)
                {
                    Length = objdashbord.officecode.Length;
                }


                if (objdashbord.roleType == "2")
                {
                    Length = 3;

                    NpgsqlCommand cmd = new NpgsqlCommand("sp_tcstorestatus_chart");
                    cmd.Parameters.AddWithValue("length_off_code", Length);
                    cmd.Parameters.AddWithValue("office_Code", Convert.ToString(objdashbord.officecode));
                    cmd.Parameters.AddWithValue("role_type", Convert.ToString(objdashbord.roleType));
                    dt = ObjCon.FetchDataTable(cmd);
                    return dt;
                }
                else
                {

                    NpgsqlCommand cmdfield = new NpgsqlCommand("sp_tcfieldstatus_chart");
                    cmdfield.Parameters.AddWithValue("length_off_code", Length);
                    cmdfield.Parameters.AddWithValue("office_Code", Convert.ToString(objdashbord.officecode));
                    cmdfield.Parameters.AddWithValue("role_type", Convert.ToString(objdashbord.roleType));
                    dt = ObjCon.FetchDataTable(cmdfield);
                    return dt;
                   
                }

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable LoadTcStatusDetailsadmin(clsDashboardUcForms objdashbord)
        {
            DataTable dt = new DataTable();
            try
            {
                string[] strArray = new string[3];

                int Length = 0;

                Length = 3;

                Length = 3;

                NpgsqlCommand cmd = new NpgsqlCommand("sp_tcstorestatus_chart");
                cmd.Parameters.AddWithValue("length_off_code", Length);
                cmd.Parameters.AddWithValue("office_Code", Convert.ToString(objdashbord.officecode));
                cmd.Parameters.AddWithValue("role_type", Convert.ToString(objdashbord.roleType));
                dt = ObjCon.FetchDataTable(cmd);
                return dt;



            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }



        public DataTable LoadAlternativeDetailsgraph(clsDashboardUcForms objdashbord)
        {
            DataTable dt = new DataTable();
            try
            {
                string[] strArray = new string[3];

                int Length = 0;
                if (objdashbord.officecode == "" || objdashbord.officecode == null)
                {
                    Length = 1;
                }

                else if (objdashbord.officecode.Length > 0 && objdashbord.officecode.Length < 5)
                {
                    Length = objdashbord.officecode.Length + 1;
                }

                else if (objdashbord.officecode.Length == 5)
                {
                    Length = objdashbord.officecode.Length;
                }


                NpgsqlCommand cmd = new NpgsqlCommand("sp_alternativedetails_chart");
                cmd.Parameters.AddWithValue("length_off_code", Length);
                cmd.Parameters.AddWithValue("office_Code", Convert.ToString(objdashbord.officecode));

                dt = ObjCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }


        public DataTable LoadRepairerPerformanceDetails(clsDashboardUcForms objdashbord)
        {
            DataTable dt = new DataTable();
            try
            {
                string[] strArray = new string[3];

                int Length = 0;
                if (objdashbord.officecode == "" || objdashbord.officecode == null)
                {
                    Length = 1;
                }

                else if (objdashbord.officecode.Length > 0 && objdashbord.officecode.Length < 5)
                {
                    Length = objdashbord.officecode.Length + 1;
                }

                else if (objdashbord.officecode.Length == 5)
                {
                    Length = objdashbord.officecode.Length;
                }


                NpgsqlCommand cmd = new NpgsqlCommand("sp_repairerperformence_chart");
                cmd.Parameters.AddWithValue("length_off_code", Length);
                cmd.Parameters.AddWithValue("office_Code", Convert.ToString(objdashbord.officecode));
                cmd.Parameters.AddWithValue("role_type", Convert.ToString(objdashbord.roleType));

                dt = ObjCon.FetchDataTable(cmd);
                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        



        public DataTable LoadPendingReplacementDetails(clsDashboardUcForms objdashbord)
        {
            DataTable dt = new DataTable();
            try
            {
                string[] strArray = new string[3];

                int Length = 0;
                if (objdashbord.officecode == "" || objdashbord.officecode == null)
                {
                    Length = 1;
                }

                else if (objdashbord.officecode.Length > 0 && objdashbord.officecode.Length < 5)
                {
                    Length = objdashbord.officecode.Length + 1;
                }

                else if (objdashbord.officecode.Length == 5)
                {
                    Length = objdashbord.officecode.Length;
                }


                NpgsqlCommand cmd = new NpgsqlCommand("sp_pendingreplacement_chart");
                cmd.Parameters.AddWithValue("length_off_code", Length);
                cmd.Parameters.AddWithValue("office_Code", Convert.ToString(objdashbord.officecode));
                cmd.Parameters.AddWithValue("role_type", Convert.ToString(objdashbord.roleType));

                dt = ObjCon.FetchDataTable(cmd);
                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }


        public DataTable Loadristatusgraph(clsDashboardUcForms objdashbord)
        {
            DataTable dt = new DataTable();
            try
            {
                string[] strArray = new string[3];

                int Length = 0;
                if (objdashbord.officecode == "" || objdashbord.officecode == null)
                {
                    Length = 1;
                }

                else if (objdashbord.officecode.Length > 0 && objdashbord.officecode.Length < 5)
                {
                    Length = objdashbord.officecode.Length + 1;
                }

                else if (objdashbord.officecode.Length == 5)
                {
                    Length = objdashbord.officecode.Length;
                }


                NpgsqlCommand cmd = new NpgsqlCommand("sp_ristatus_chart");
                cmd.Parameters.AddWithValue("length_off_code", Length);
                cmd.Parameters.AddWithValue("office_Code", Convert.ToString(objdashbord.officecode));
                cmd.Parameters.AddWithValue("role_type", Convert.ToString(objdashbord.roleType));

                dt = ObjCon.FetchDataTable(cmd);
                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }



        public DataTable Loadfrequentlyfailedgraph(clsDashboardUcForms objdashbord)
        {
            DataTable dt = new DataTable();
            try
            {
                string[] strArray = new string[3];

                int Length = 0;
                if (objdashbord.officecode == "" || objdashbord.officecode == null)
                {
                    Length = 1;
                }

                else if (objdashbord.officecode.Length > 0 && objdashbord.officecode.Length < 5)
                {
                    Length = objdashbord.officecode.Length + 1;
                }

                else if (objdashbord.officecode.Length == 5)
                {
                    Length = objdashbord.officecode.Length;
                }


                NpgsqlCommand cmd = new NpgsqlCommand("sp_frequentlyfaildtc_chart");
                cmd.Parameters.AddWithValue("length_off_code", Length);
                cmd.Parameters.AddWithValue("office_Code", Convert.ToString(objdashbord.officecode));
               

                dt = ObjCon.FetchDataTable(cmd);
                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }


        public DataTable Loadfrequentlyfaileddtrgraph(clsDashboardUcForms objdashbord)
        {
            DataTable dt = new DataTable();
            try
            {
                string[] strArray = new string[3];

                int Length = 0;
                if (objdashbord.officecode == "" || objdashbord.officecode == null)
                {
                    Length = 1;
                }

                else if (objdashbord.officecode.Length > 0 && objdashbord.officecode.Length < 5)
                {
                    Length = objdashbord.officecode.Length + 1;
                }

                else if (objdashbord.officecode.Length == 5)
                {
                    Length = objdashbord.officecode.Length;
                }


                NpgsqlCommand cmd = new NpgsqlCommand("sp_frequentlyfaileddtr_chart");
                cmd.Parameters.AddWithValue("length_off_code", Length);
                cmd.Parameters.AddWithValue("office_Code", Convert.ToString(objdashbord.officecode));

                dt = ObjCon.FetchDataTable(cmd);
                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }


       

        public DataTable LoadTransformerDetailsstackgridview(clsDashboardUcForms objdashbord)
        {
            DataTable dt = new DataTable();
            try
            {
                string[] strArray = new string[3];

                int Length = 0;
                if (objdashbord.officecode == "" || objdashbord.officecode == null)
                {
                    Length = 1;
                }

                else if (objdashbord.officecode.Length > 0 && objdashbord.officecode.Length < 5)
                {
                    Length = objdashbord.officecode.Length + 1;
                }

                else if (objdashbord.officecode.Length == 5)
                {
                    Length = objdashbord.officecode.Length;
                }


                if (objdashbord.roleType == "2")
                {
                    Length = 3;

                    NpgsqlCommand cmd = new NpgsqlCommand("sp_storetcdetails_gridview");
                    cmd.Parameters.AddWithValue("length_off_code", Length);
                    cmd.Parameters.AddWithValue("office_Code", Convert.ToString(objdashbord.officecode));
                    cmd.Parameters.AddWithValue("role_type", Convert.ToString(objdashbord.roleType));
                    dt = ObjCon.FetchDataTable(cmd);
                    return dt;
                }
                else
                {

                    NpgsqlCommand cmd = new NpgsqlCommand("sp_fieldtcdetails_gridview");
                    cmd.Parameters.AddWithValue("length_off_code", Length);
                    cmd.Parameters.AddWithValue("office_Code", Convert.ToString(objdashbord.officecode));
                    cmd.Parameters.AddWithValue("role_type", Convert.ToString(objdashbord.roleType));
                    dt = ObjCon.FetchDataTable(cmd);
                    return dt;
                }

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable LoadTransformerDetailsstackgraphadmingridview(clsDashboardUcForms objdashbord)
        {
            DataTable dt = new DataTable();
            try
            {
                string[] strArray = new string[3];

                int Length = 0;

                Length = 3;

                Length = 3;

                NpgsqlCommand cmd = new NpgsqlCommand("sp_storetcdetails_gridview");
                cmd.Parameters.AddWithValue("length_off_code", Length);
                cmd.Parameters.AddWithValue("office_Code", Convert.ToString(objdashbord.officecode));
                cmd.Parameters.AddWithValue("role_type", Convert.ToString(objdashbord.roleType));
                dt = ObjCon.FetchDataTable(cmd);
                return dt;



            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }


        public DataTable LoadTcStatusgridview(clsDashboardUcForms objdashbord)
        {
            DataTable dt = new DataTable();
            try
            {
                string[] strArray = new string[3];

                int Length = 0;
                if (objdashbord.officecode == "" || objdashbord.officecode == null)
                {
                    Length = 1;
                }

                else if (objdashbord.officecode.Length > 0 && objdashbord.officecode.Length < 5)
                {
                    Length = objdashbord.officecode.Length + 1;
                }

                else if (objdashbord.officecode.Length == 5)
                {
                    Length = objdashbord.officecode.Length;
                }


                if (objdashbord.roleType == "2")
                {
                    Length = 3;

                    NpgsqlCommand cmd = new NpgsqlCommand("sp_tcstorestatus_gridview");
                    cmd.Parameters.AddWithValue("length_off_code", Length);
                    cmd.Parameters.AddWithValue("office_Code", Convert.ToString(objdashbord.officecode));
                    cmd.Parameters.AddWithValue("role_type", Convert.ToString(objdashbord.roleType));
                    dt = ObjCon.FetchDataTable(cmd);
                    return dt;
                }
                else
                {

                    NpgsqlCommand cmdfield = new NpgsqlCommand("sp_tcfieldstatus_gridview");
                    cmdfield.Parameters.AddWithValue("length_off_code", Length);
                    cmdfield.Parameters.AddWithValue("office_Code", Convert.ToString(objdashbord.officecode));
                    cmdfield.Parameters.AddWithValue("role_type", Convert.ToString(objdashbord.roleType));
                    dt = ObjCon.FetchDataTable(cmdfield);
                    return dt;

                }

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable LoadTcStatusDetailsadmingridview(clsDashboardUcForms objdashbord)
        {
            DataTable dt = new DataTable();
            try
            {
                string[] strArray = new string[3];

                int Length = 0;

                Length = 3;

                Length = 3;

                NpgsqlCommand cmd = new NpgsqlCommand("sp_tcstorestatus_gridview");
                cmd.Parameters.AddWithValue("length_off_code", Length);
                cmd.Parameters.AddWithValue("office_Code", Convert.ToString(objdashbord.officecode));
                cmd.Parameters.AddWithValue("role_type", Convert.ToString(objdashbord.roleType));
                dt = ObjCon.FetchDataTable(cmd);
                return dt;



            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }


        public DataTable LoadAlternativeDetailsgridview(clsDashboardUcForms objdashbord)
        {
            DataTable dt = new DataTable();
            try
            {
                string[] strArray = new string[3];

                int Length = 0;
                if (objdashbord.officecode == "" || objdashbord.officecode == null)
                {
                    Length = 1;
                }

                else if (objdashbord.officecode.Length > 0 && objdashbord.officecode.Length < 5)
                {
                    Length = objdashbord.officecode.Length + 1;
                }

                else if (objdashbord.officecode.Length == 5)
                {
                    Length = objdashbord.officecode.Length;
                }


                NpgsqlCommand cmd = new NpgsqlCommand("sp_alternativedetails_viewgrid");
                cmd.Parameters.AddWithValue("length_off_code", Length);
                cmd.Parameters.AddWithValue("office_Code", Convert.ToString(objdashbord.officecode));

                dt = ObjCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable LoadRepairerPerformanceDetailsgridview(clsDashboardUcForms objdashbord)
        {
            DataTable dt = new DataTable();
            try
            {
                string[] strArray = new string[3];

                int Length = 0;
                if (objdashbord.officecode == "" || objdashbord.officecode == null)
                {
                    Length = 1;
                }

                else if (objdashbord.officecode.Length > 0 && objdashbord.officecode.Length < 5)
                {
                    Length = objdashbord.officecode.Length + 1;
                }

                else if (objdashbord.officecode.Length == 5)
                {
                    Length = objdashbord.officecode.Length;
                }


                NpgsqlCommand cmd = new NpgsqlCommand("sp_repairerperformence_viewgrid");
                cmd.Parameters.AddWithValue("length_off_code", Length);
                cmd.Parameters.AddWithValue("office_Code", Convert.ToString(objdashbord.officecode));
                cmd.Parameters.AddWithValue("role_type", Convert.ToString(objdashbord.roleType));

                dt = ObjCon.FetchDataTable(cmd);
                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable LoadPendingReplacementDetailsgridview(clsDashboardUcForms objdashbord)
        {
            DataTable dt = new DataTable();
            try
            {
                string[] strArray = new string[3];

                int Length = 0;
                if (objdashbord.officecode == "" || objdashbord.officecode == null)
                {
                    Length = 1;
                }

                else if (objdashbord.officecode.Length > 0 && objdashbord.officecode.Length < 5)
                {
                    Length = objdashbord.officecode.Length + 1;
                }

                else if (objdashbord.officecode.Length == 5)
                {
                    Length = objdashbord.officecode.Length;
                }


                NpgsqlCommand cmd = new NpgsqlCommand("sp_pendingreplacement_viewgrid");
                cmd.Parameters.AddWithValue("length_off_code", Length);
                cmd.Parameters.AddWithValue("office_Code", Convert.ToString(objdashbord.officecode));
                cmd.Parameters.AddWithValue("role_type", Convert.ToString(objdashbord.roleType));

                dt = ObjCon.FetchDataTable(cmd);
                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable Loadridetalsgridview(clsDashboardUcForms objdashbord)
        {
            DataTable dt = new DataTable();
            try
            {
                string[] strArray = new string[3];

                int Length = 0;
                if (objdashbord.officecode == "" || objdashbord.officecode == null)
                {
                    Length = 1;
                }

                else if (objdashbord.officecode.Length > 0 && objdashbord.officecode.Length < 5)
                {
                    Length = objdashbord.officecode.Length + 1;
                }

                else if (objdashbord.officecode.Length == 5)
                {
                    Length = objdashbord.officecode.Length;
                }


                NpgsqlCommand cmd = new NpgsqlCommand("sp_ristatus_viewgrid");
                cmd.Parameters.AddWithValue("length_off_code", Length);
                cmd.Parameters.AddWithValue("office_Code", Convert.ToString(objdashbord.officecode));
                cmd.Parameters.AddWithValue("role_type", Convert.ToString(objdashbord.roleType));

                dt = ObjCon.FetchDataTable(cmd);
                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }


        public DataTable Loadfrequentlyfailedgridview(clsDashboardUcForms objdashbord)
        {
            DataTable dt = new DataTable();
            try
            {
                string[] strArray = new string[3];

                int Length = 0;
                if (objdashbord.officecode == "" || objdashbord.officecode == null)
                {
                    Length = 1;
                }

                else if (objdashbord.officecode.Length > 0 && objdashbord.officecode.Length < 5)
                {
                    Length = objdashbord.officecode.Length + 1;
                }

                else if (objdashbord.officecode.Length == 5)
                {
                    Length = objdashbord.officecode.Length;
                }


                NpgsqlCommand cmd = new NpgsqlCommand("sp_frequentlyfaildtc_chart");
                cmd.Parameters.AddWithValue("length_off_code", Length);
                cmd.Parameters.AddWithValue("office_Code", Convert.ToString(objdashbord.officecode));

                dt = ObjCon.FetchDataTable(cmd);
                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable Loadfrequentlyfailedsectiongridview(clsDashboardUcForms objdashbord)
        {
            DataTable dt = new DataTable();
            try
            {
                string[] strArray = new string[3];

                int Length = 0;
                if (objdashbord.officecode == "" || objdashbord.officecode == null)
                {
                    Length = 1;
                }

                else if (objdashbord.officecode.Length > 0 && objdashbord.officecode.Length < 5)
                {
                    Length = objdashbord.officecode.Length + 1;
                }

                else if (objdashbord.officecode.Length == 5)
                {
                    Length = objdashbord.officecode.Length;
                }


                NpgsqlCommand cmd = new NpgsqlCommand("sp_frequentlyfaildtcsection_chart");
                cmd.Parameters.AddWithValue("length_off_code", Length);
                cmd.Parameters.AddWithValue("office_Code", Convert.ToString(objdashbord.officecode));

                dt = ObjCon.FetchDataTable(cmd);
                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable Loadfrequentlyfaileddtrgridview(clsDashboardUcForms objdashbord)
        {
            DataTable dt = new DataTable();
            try
            {
                string[] strArray = new string[3];

                int Length = 0;
                if (objdashbord.officecode == "" || objdashbord.officecode == null)
                {
                    Length = 1;
                }

                else if (objdashbord.officecode.Length > 0 && objdashbord.officecode.Length < 5)
                {
                    Length = objdashbord.officecode.Length + 1;
                }

                else if (objdashbord.officecode.Length == 5)
                {
                    Length = objdashbord.officecode.Length;
                }


                NpgsqlCommand cmd = new NpgsqlCommand("sp_frequentlyfaildtr_gridview");
                cmd.Parameters.AddWithValue("length_off_code", Length);
                cmd.Parameters.AddWithValue("office_Code", Convert.ToString(objdashbord.officecode));

                dt = ObjCon.FetchDataTable(cmd);
                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable LoadExpenditureDetails(clsDashboardUcForms objdashbord)
        {
            DataTable dt = new DataTable();
            try
            {
                string[] strArray = new string[3];

                int Length = 0;
                //if (objdashbord.officecode == "" || objdashbord.officecode == null)
                //{
                //    Length = 1;
                //}

                //else if (objdashbord.officecode.Length > 0 && objdashbord.officecode.Length < 5)
                //{
                //    Length = objdashbord.officecode.Length + 1;
                //}

                //else if (objdashbord.officecode.Length == 5)
                //{
                //    Length = objdashbord.officecode.Length;
                //}
                Length = objdashbord.officecode.Length;

                NpgsqlCommand cmd = new NpgsqlCommand("sp_expenditure_chart");
                cmd.Parameters.AddWithValue("length_off_code", Length);
                cmd.Parameters.AddWithValue("office_Code", Convert.ToString(objdashbord.officecode));

                dt = ObjCon.FetchDataTable(cmd);
                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }


    }
}
