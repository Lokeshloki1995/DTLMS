using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIITS.DTLMS.BL.MasterForms
{
  public class clsRepairerRates
    {
        string strFormCode = "clsRepairerRates";
        NpgsqlCommand cmd;

        public string pkid { get; set; }
        public string div_id { get; set; }
        public string rep_id { get; set; }
        public string rep_name { get; set; }
        public string capacity_id { get; set; }
        public string capacity { get; set; }
        public string StarRate { get; set; }
        public string StarRate_id { get; set; }
        public string EffectiveFrom { get; set; }
        public string EffectiveTo { get; set; }
        public string Po_No { get; set; }
        public string PoDate { get; set; }
        public string sCrby { get; set; }
        public Double Cost { get; set; }

        public DataTable GetRepairerRateDetails(clsRepairerRates obj)
        {
            PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
            DataTable dt = new DataTable();
            try
            {
                cmd = new  NpgsqlCommand("proc_get_repaireratedetails");
                cmd.Parameters.AddWithValue("div_id",Convert.ToString( obj.div_id == null ?"" : obj.div_id));
                cmd.Parameters.AddWithValue("rep_id", Convert.ToString(obj.rep_id == null ? "" : obj.rep_id));
                cmd.Parameters.AddWithValue("cap_id", Convert.ToString(obj.capacity == null ? "" : obj.capacity));
                cmd.Parameters.AddWithValue("star_id", Convert.ToString(obj.StarRate == null ? "" : obj.StarRate));
                dt = objcon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public string[] SaveRepairerRates(clsRepairerRates obj)
        {

            string[] strResult = new string[2];
            PGSqlConnection objcon = new PGSqlConnection(Constants.Password);          
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("sp_saveupdate_repairer_rates");
                cmd.Parameters.AddWithValue("rr_id", Convert.ToString(obj.pkid==null ?"" : obj.pkid));
                cmd.Parameters.AddWithValue("rr_divi_id", Convert.ToString(obj.div_id));
                cmd.Parameters.AddWithValue("rep_id", Convert.ToString(obj.rep_id));
                cmd.Parameters.AddWithValue("repairer", Convert.ToString(obj.rep_name));
                cmd.Parameters.AddWithValue("po_no", Convert.ToString(obj.Po_No== null ? "" : obj.Po_No));
                cmd.Parameters.AddWithValue("po_date", Convert.ToString(obj.PoDate==null?"":obj.PoDate));
                cmd.Parameters.AddWithValue("effective_from", Convert.ToString(obj.EffectiveFrom));

                cmd.Parameters.AddWithValue("effective_to", Convert.ToString(obj.EffectiveTo));
                cmd.Parameters.AddWithValue("cap_id", Convert.ToString(obj.capacity_id));
                cmd.Parameters.AddWithValue("capacity", Convert.ToString(obj.capacity));
                cmd.Parameters.AddWithValue("rating_id", Convert.ToString(obj.StarRate_id));

                cmd.Parameters.AddWithValue("rating", Convert.ToString(obj.StarRate));
                cmd.Parameters.AddWithValue("amount",Convert.ToDouble( obj.Cost));
                cmd.Parameters.AddWithValue("crby", Convert.ToString(obj.sCrby));

                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd.Parameters.Add("msg", NpgsqlDbType.Text);

                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                cmd.Parameters["msg"].Direction = ParameterDirection.Output;

                strResult[0] = "op_id";
                strResult[1] = "msg";
            
            
                strResult = objcon.Execute(cmd, strResult, 2);

                return strResult;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return strResult;
            }
        }
        public string[] DeleteRepairerRates(string pkid)
        {

            string[] strResult = new string[2];
            PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
            try
            {
                cmd = new NpgsqlCommand("sp_delete_repairercost");
                cmd.Parameters.AddWithValue("rr_id", Convert.ToString( pkid == null ? "" :  pkid));

                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd.Parameters.Add("msg", NpgsqlDbType.Text);
               
                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                cmd.Parameters["msg"].Direction = ParameterDirection.Output;

                strResult[0] = "op_id";
                strResult[1] = "msg";

                strResult = objcon.Execute(cmd, strResult, 2);
                return strResult;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return strResult;
            }
        }

        public DataTable GetRepairRatesDetails(string repid)
        {
            DataTable dt = new DataTable();
            try
            {
                PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
                string strQry = string.Empty;


                cmd = new NpgsqlCommand("proc_get_repairer_rates_details");
                cmd.Parameters.AddWithValue("p_rep_id", Convert.ToString(repid == null ? "" : repid));
                dt = objcon.FetchDataTable(cmd);

                //strQry = "select case when \"TR_DWA_NO\" = '0' then null else \"TR_DWA_NO\" end TR_DWA_NO ,case when \"TR_DWA_DATE\" = '01/01/0001' then null else to_char(\"TR_DWA_DATE\",'DD/MM/YYYY') end TR_DWA_DATE ,to_char(\"TR_CON_STR_DATE\",'DD/MM/YYYY') TR_CON_STR_DATE, to_char(\"TR_CON_END_DATE\",'DD/MM/YYYY') TR_CON_END_DATE from \"TBLTRANSREPAIRER\" where \"TR_ID\"='" + repid + "'";
                //dt = objcon.FetchDataTable(strQry);

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
