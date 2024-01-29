using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using IIITS.PGSQL.DAL;
using Npgsql;
using System.Reflection;
using NpgsqlTypes;
using IIITS.DTLMS.BL.DataBase;

namespace IIITS.DTLMS.BL
{
    public class clsOmSecMast
    {
        PGSqlConnection Objcon = new PGSqlConnection(Constants.Password);
        string strQry = string.Empty;
        string strFormCode = "clsOmSecMast";
        public string sSubDivCode { get; set; }

        public string[] SaveOmSecMastDetails(string strOmCode, string strOmName, string strSubDivCode, string strOmHeadEmp, string strOmMobile, string strUserLogged,string Adress)
        {
            string[] Arr = new string[2];
            string strId = string.Empty;
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_save_update_omSecMast");
                cmd.Parameters.AddWithValue("om_id", strId);
                cmd.Parameters.AddWithValue("om_name", strOmName);
                cmd.Parameters.AddWithValue("om_code", strOmCode);
                cmd.Parameters.AddWithValue("om_heademp", strOmHeadEmp);
                cmd.Parameters.AddWithValue("subdiv_code", strSubDivCode);
                
                cmd.Parameters.AddWithValue("om_mobile", strOmMobile);
                cmd.Parameters.AddWithValue("user_logged", strUserLogged);
                cmd.Parameters.AddWithValue("om_address", Adress);

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
     
        public string[] UpdateOmSecMastDetails(string strOldId, string strOmCode, string strOmName, string strSubDivCode, string strOmHeadEmp, string strOmMobile, string strUserLogged,string adress)
        {
            string[] Arr = new string[2];
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_save_update_omSecMast");
                cmd.Parameters.AddWithValue("om_id", strOldId);
                cmd.Parameters.AddWithValue("om_name", strOmName);
                cmd.Parameters.AddWithValue("om_code", strOmCode);
                cmd.Parameters.AddWithValue("om_heademp", strOmHeadEmp);
                cmd.Parameters.AddWithValue("subdiv_code", strSubDivCode);
                cmd.Parameters.AddWithValue("om_mobile", strOmMobile);
                cmd.Parameters.AddWithValue("user_logged", strUserLogged);
                cmd.Parameters.AddWithValue("om_address", adress);

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
   
        public DataTable LoadOmSecvOffDet(string strOmSecID = "")
        {
            DataTable Dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_omsecoff_details");
                cmd.Parameters.AddWithValue("om_sec_id", strOmSecID);
                Dt = Objcon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return Dt;
        }
        NpgsqlCommand NpgsqlCommand;
        DataBseConnection ObjBasCon = new DataBseConnection(Constants.Password);

        public string GenerateOmSecCode(clsOmSecMast objOmSec)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string sOmSecCode = string.Empty;
            try
            {
                #region inline query
                //NpgsqlCommand.Parameters.AddWithValue("subdivcode", objOmSec.sSubDivCode);
                //sOmSecCode = Objcon.get_value(" SELECT CAST(COALESCE(MAX(\"OM_CODE\"),0)+1 AS TEXT) FROM \"TBLOMSECMAST\"  
                // where CAST(\"OM_SUBDIV_CODE\"AS TEXT) like :subdivcode||'%'", NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_generate_omsec_code");
                cmd.Parameters.AddWithValue("subdiv_code", Convert.ToString(objOmSec.sSubDivCode ?? ""));
                sOmSecCode = ObjBasCon.StringGetValue(cmd);

                if (Convert.ToInt16(sOmSecCode) <= 1)
               {
                   sOmSecCode = objOmSec.sSubDivCode + "1";
               }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return sOmSecCode;
        }
    }
}




