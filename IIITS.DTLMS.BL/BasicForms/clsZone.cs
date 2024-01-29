using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using IIITS.PGSQL.DAL;
using Npgsql;
using System.Reflection;
using NpgsqlTypes;

namespace IIITS.DTLMS.BL
{
    public class clsZone
    {
        string strFormCode = "clsZone";
        PGSqlConnection Objcon = new PGSqlConnection(Constants.Password);
      
        public string sZoneId { get; set; }
        public string sZoneName { get; set; }
        public string sName { get; set; }
        public string sPhone { get; set; }
        public string saddr { get; set; }
        public string sMobileNo { get; set; }
        public string sMaxid { get; set; }
        public string sEmailId { get; set; }

        public string[] SaveZone(clsZone objZone)
        {
            string[] Arr = new string[3];
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_save_update_Zone");
                cmd.Parameters.AddWithValue("zn_id", objZone.sMaxid);
                cmd.Parameters.AddWithValue("zn_name", objZone.sZoneName.ToUpper());
                cmd.Parameters.AddWithValue("zn_phone", objZone.sPhone);
                cmd.Parameters.AddWithValue("zn_addr", objZone.saddr);
                cmd.Parameters.AddWithValue("zn_head_name", objZone.sName);
                cmd.Parameters.AddWithValue("zn_mobile_number", objZone.sMobileNo);
                cmd.Parameters.AddWithValue("zo_email_id", objZone.sEmailId);
                cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;

                Arr[0] = "msg";
                Arr[1] = "op_id";
                Arr = Objcon.Execute(cmd, Arr, 2);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return Arr;
        }
        public DataTable LoadAllZoneDetails()
        {
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_zone_toload");
                dt = Objcon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public object getZoneDetails(clsZone objZone)
        {
            DataTable dtDetails = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_zone_details");
                cmd.Parameters.AddWithValue("zn_id", objZone.sMaxid);
                dtDetails = Objcon.FetchDataTable(cmd);

                if (dtDetails.Rows.Count > 0)
                {
                    objZone.sZoneId = Convert.ToString(dtDetails.Rows[0]["ZO_ID"]);
                    objZone.sZoneName = Convert.ToString(dtDetails.Rows[0]["ZO_NAME"]);
                    objZone.sPhone = Convert.ToString(dtDetails.Rows[0]["ZO_PHONE"]);
                    objZone.saddr = Convert.ToString(dtDetails.Rows[0]["ZO_ADDR"]);
                    objZone.sName = Convert.ToString(dtDetails.Rows[0]["ZO_HEAD_EMP"]);
                    objZone.sMobileNo = Convert.ToString(dtDetails.Rows[0]["ZO_MOBILE_NO"]);
                    objZone.sEmailId = Convert.ToString(dtDetails.Rows[0]["ZO_EMAIL_ID"]);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return objZone;
        }
        public string GenerateZoneId()
        {
            string sCircleCodeNo = string.Empty;
            try
            {
                sCircleCodeNo = Convert.ToString(Objcon.Get_max_no("ZO_ID", "TBLZONE"));
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return sCircleCodeNo;
        }
    }
}
