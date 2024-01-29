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
    public class clsCircle                      
    {
        PGSqlConnection Objcon = new PGSqlConnection(Constants.Password);
        string strFormCode = "clsCircle";
        public string sCircleCode { get; set; }
        public string sCircleName { get; set; }
        public string sName { get; set; }
        public string sPhone { get; set; }
        public string sMobileNo { get; set; }
        public string sEmail { get; set; }
        public string sMaxid { get; set; }
        public string sZoneCode { get; set; }
        public string sAddress { get; set; }
        
        
        public string[] SaveCircle(clsCircle objCircle)
        {
           
            string[] Arr = new string[3];
            try
            {                
                  NpgsqlCommand cmd = new NpgsqlCommand("proc_save_update_circle");
                    cmd.Parameters.AddWithValue("cir_id", objCircle.sMaxid);
                    cmd.Parameters.AddWithValue("cir_code", objCircle.sCircleCode);
                    cmd.Parameters.AddWithValue("cir_name", objCircle.sCircleName.ToUpper());
                    cmd.Parameters.AddWithValue("cir_head_name", objCircle.sName);                 
                    cmd.Parameters.AddWithValue("cir_mobile_number", objCircle.sMobileNo);
                    cmd.Parameters.AddWithValue("cir_phone", objCircle.sPhone);
                    cmd.Parameters.AddWithValue("cir_email", objCircle.sEmail);
                    cmd.Parameters.AddWithValue("cir_zo_id", objCircle.sZoneCode);
                    cmd.Parameters.AddWithValue("cm_address", objCircle.sAddress);
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
        
        public DataTable LoadAllCircleDetails()
        {
            DataTable dt = new DataTable();
          
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_circle_toload");
                dt = Objcon.FetchDataTable(cmd);
                
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
            return dt;
        }
      
        public object getCircleDetails(clsCircle objCircle)
        {
            DataTable dtDetails = new DataTable();
            try
            {

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_circle_details");
                cmd.Parameters.AddWithValue("cir_id", objCircle.sMaxid);
                dtDetails = Objcon.FetchDataTable(cmd);
               

                if (dtDetails.Rows.Count > 0)
                {
                    objCircle.sCircleCode = Convert.ToString(dtDetails.Rows[0]["CM_CIRCLE_CODE"]);
                    objCircle.sCircleName = Convert.ToString(dtDetails.Rows[0]["CM_CIRCLE_NAME"]);
                    objCircle.sName = Convert.ToString(dtDetails.Rows[0]["CM_HEAD_EMP"]);
                    objCircle.sMobileNo = Convert.ToString(dtDetails.Rows[0]["CM_MOBILE_NO"]);
                    objCircle.sPhone = Convert.ToString(dtDetails.Rows[0]["CM_PHONE"]);
                    objCircle.sEmail = Convert.ToString(dtDetails.Rows[0]["CM_EMAIL"]);
                    objCircle.sZoneCode = Convert.ToString(dtDetails.Rows[0]["CM_ZO_ID"]);
                    objCircle.sAddress = Convert.ToString(dtDetails.Rows[0]["CM_ADDRESS"]);
                }            
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return objCircle;
        }
        public string GenerateCircleCode()
        {
            string sCircleCodeNo = string.Empty;
            
            try
            {
               sCircleCodeNo  = Convert.ToString(Objcon.Get_max_no("CM_CIRCLE_CODE", "TBLCIRCLE"));
                    
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
            return sCircleCodeNo;
        }

        NpgsqlCommand NpgsqlCommand;
        public string GenerateCirCode(clsCircle objCir)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string sZoneCodeNo = string.Empty;
            try
            {

                NpgsqlCommand cmd = new NpgsqlCommand("sp_generatecirclecode");
                cmd.Parameters.AddWithValue("ZoneCode", objCir.sZoneCode);
                cmd.Parameters.Add("sZoneCodeNo", NpgsqlDbType.Text);
                cmd.Parameters["sZoneCodeNo"].Direction = ParameterDirection.Output;
                DataTable dt = Objcon.FetchDataTable(cmd);
                if(dt.Rows.Count>0)
                { 
                sZoneCodeNo = dt.Rows[0]["sZoneCodeNo"].ToString();
                 }
                if (Convert.ToInt16(sZoneCodeNo) <= 1)
                {
                    sZoneCodeNo = objCir.sZoneCode + "1";
                }
                // INLINE QUERY CONVERTED TO SP
                //NpgsqlCommand.Parameters.AddWithValue("ZoneCode", objCir.sZoneCode);
                //string strQry = " SELECT CAST(COALESCE(MAX(\"CM_CIRCLE_CODE\"),0)+1 AS TEXT) FROM \"TBLCIRCLE\"  where CAST(\"CM_ZO_ID\"AS TEXT) like :ZoneCode||'%'";
                //sZoneCodeNo = Objcon.get_value(strQry, NpgsqlCommand);
                //if (Convert.ToInt16(sZoneCodeNo) <= 1)
                //{
                //    sZoneCodeNo = objCir.sZoneCode + "1";
                //}
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return sZoneCodeNo;
        }

    }
}
