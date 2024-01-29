using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIITS.DTLMS.BL.MasterForms
{
    public class ClsUserLoginDetails
    {
        public string strFormCode = "ClsUserLoginDetails";

        PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
        NpgsqlCommand NpgsqlCommand;
        public string storeoffcode { get; set; }
        public string OfficeCode { get; set; }
        public string FromDate { get; set; }
        public string Todate { get; set; }

        // to change current system date and time formate
        public string GetDateTimeSpecificFormat(DateTime dtDateTime)
        {
            string ExactDateTime = string.Empty;
            try
            {
                ExactDateTime = DateTime.ParseExact(DateTime.Now.ToString(), "MM/DD/YYYY hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd hh:mm:ss");
            }
            catch (Exception)
            {
                try
                {
                    ExactDateTime = DateTime.ParseExact(DateTime.Now.ToString(), "DD-MM-YYYY hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd hh:mm:ss");
                }
                catch (Exception)
                {
                    try
                    {
                        ExactDateTime = DateTime.ParseExact(DateTime.Now.ToString(), "MM-DD-YYYY hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd hh:mm:ss");
                    }
                    catch (Exception)
                    {
                        try
                        {
                            ExactDateTime = DateTime.ParseExact(DateTime.Now.ToString(), "DD/MM/YYYY hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd hh:mm:ss");
                        }
                        catch (Exception ex)
                        {
                            ExactDateTime = DateTime.ParseExact(DateTime.Now.ToString(), "DD/MM/YYYY HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd hh:mm:ss");
                            clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                        }
                    }
                }
            }
            return ExactDateTime;
        }
        /// <summary>
        /// to insert login time to table TBLUSERLOGINDETAILS
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="LoginTime"></param>
        public void InsertDIDetails(string UserID, DateTime LoginTime)
        {
            string[] Arr = new string[3];
            string strQry = string.Empty;
            string datetime = string.Empty;
            NpgsqlCommand = new NpgsqlCommand();
            DataTable dt = new DataTable();
            try
            {
                #region inline query
                //strQry = "INSERT INTO \"TBLUSERLOGINDETAILS\" (\"ULD_USER_ID\",\"ULD_LOGIN\") VALUES ";
                //strQry += " ('" + UserID + "',now())";
                //objcon.ExecuteQry(strQry);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_insert_user_login_details");
                cmd.Parameters.AddWithValue("user_id", UserID);
                objcon.Execute(cmd, Arr, 0);
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to update logout timings to tble TBLUSERLOGINDETAILS
        /// </summary>
        /// <param name="UserID"></param>
        public void UpdateDIDetails(string UserID)
        {
            string[] Arr = new string[3];
            string strQry = string.Empty;
            string maxid = string.Empty;
            string TimeDuration = string.Empty;
            NpgsqlCommand = new NpgsqlCommand();
            DataTable dt = new DataTable();
            DateTime time = System.DateTime.Now;
            try
            {
                if (UserID != "")
                {
                    #region inline query
                    //string qry = "select \"ULD_ID\" from \"TBLUSERLOGINDETAILS\" where \"ULD_USER_ID\"='" + UserID + "' ORDER BY \"ULD_ID\" desc limit 1";
                    //maxid = objcon.get_value(qry);

                    //strQry = "UPDATE \"TBLUSERLOGINDETAILS\" SET \"ULD_LOGOUT\" =now() WHERE \"ULD_ID\" = '" + maxid + "'";
                    //objcon.ExecuteQry(strQry);

                    //string qry1 = "select  \"ULD_LOGOUT\"-\"ULD_LOGIN\"   from \"TBLUSERLOGINDETAILS\" where \"ULD_USER_ID\"='" + UserID + "' ORDER BY \"ULD_ID\" desc limit 1";
                    //TimeDuration = objcon.get_value(qry1);

                    //strQry = "UPDATE \"TBLUSERLOGINDETAILS\" SET \"ULD_DURATION\" = '" + TimeDuration + "' WHERE \"ULD_ID\" = '" + maxid + "'";
                    //objcon.ExecuteQry(strQry);
                    #endregion
                    NpgsqlCommand cmd = new NpgsqlCommand("proc_update_user_login_details");
                    cmd.Parameters.AddWithValue("user_id", UserID);
                    objcon.Execute(cmd, Arr, 0);

                }
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to fetch user login details to generate excel.
        /// </summary>
        /// <param name="objuserlogin"></param>
        /// <returns></returns>
        public DataTable GetUserLoginDetails(ClsUserLoginDetails objuserlogin)
        {
            DataTable dt = new DataTable();
            try
            {
                if (objuserlogin.FromDate != null)
                {
                    objuserlogin.FromDate = DateTime.ParseExact(objuserlogin.FromDate, "yyyy/MM/dd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                }
                if (objuserlogin.Todate != null)
                {
                    objuserlogin.Todate = DateTime.ParseExact(objuserlogin.Todate, "yyyy/MM/dd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                }
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_userlogin_details");
                cmd.Parameters.AddWithValue("officecode", objuserlogin.OfficeCode == null ? "" : objuserlogin.OfficeCode);
                cmd.Parameters.AddWithValue("storeoffcode", objuserlogin.storeoffcode == null ? "" : objuserlogin.storeoffcode);
                cmd.Parameters.AddWithValue("fromdate", objuserlogin.FromDate == null ? "" : objuserlogin.FromDate);
                cmd.Parameters.AddWithValue("todate", objuserlogin.Todate == null ? "" : objuserlogin.Todate);

                dt = objcon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public string getLastlogintime(string UserID)
        {
            string strQry = string.Empty;
            string LastLogin = string.Empty;
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                if (UserID != "")
                {
                    #region inline query
                    //string qry1 = "select \"ULD_LOGIN\" from(select distinct \"ULD_ID\",ROW_NUMBER() over (order by \"ULD_ID\" desc) as RowNumber,\"ULD_LOGIN\" from \"TBLUSERLOGINDETAILS\"";
                    //qry1 += " where \"ULD_USER_ID\" ='" + UserID + "' order by \"ULD_ID\" desc)A where rownumber=2";
                    //LastLogin = objcon.get_value(qry1);
                    #endregion
                    DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
                    NpgsqlCommand cmd = new NpgsqlCommand("proc_getvalue_for_clsuserlogindetails");
                    cmd.Parameters.AddWithValue("p_key", "GETLASTLOGINTIME");
                    cmd.Parameters.AddWithValue("p_value", Convert.ToString( UserID));
                    cmd.Parameters.AddWithValue("p_offcode", "");
                    LastLogin = objDatabse.StringGetValue(cmd);
                }
                return LastLogin;
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return LastLogin;
            }
        }
        public string getVersion()
        {
            string strQry = string.Empty;
            string Latestversion = string.Empty;
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                #region inline query
                //strQry = "select \"V_VERSION\" from \"TBLVERSIONS\" where \"V_STATUS\"='1'";
                //return Latestversion = objcon.get_value(strQry);
                #endregion
                DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
                NpgsqlCommand cmd = new NpgsqlCommand("proc_getvalue_for_clsuserlogindetails");
                cmd.Parameters.AddWithValue("p_key", "GETVERSION");
                cmd.Parameters.AddWithValue("p_value", "");
                cmd.Parameters.AddWithValue("p_offcode", "");
                Latestversion = objDatabse.StringGetValue(cmd);
                return Latestversion;
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Latestversion;
            }
        }
    }
}
