using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IIITS.PGSQL.DAL;
using Npgsql;

namespace IIITS.DTLMS.BL
{
    public class clsAlterPowerSupply
    {
        public string strFormCode = "clsAlterPowerSupply";
        NpgsqlCommand NpgsqlCommand;
        public string GetFailureId(string sDTRCode, string sDTCCode)
        {
            NpgsqlCommand = new NpgsqlCommand();
            PGSqlConnection obj = new PGSqlConnection(Constants.Password);
            string sDFId = string.Empty;
            try
            {
                string sQry = string.Empty;
                sQry = "SELECT \"DF_ID\" FROM \"TBLDTCFAILURE\" WHERE \"DF_REPLACE_FLAG\" = 0 AND (\"DF_EQUIPMENT_ID\" =:sDTRCode OR ";
                sQry += " \"DF_DTC_CODE\" =:sDTCCode)";
                //NpgsqlCommand.Parameters.AddWithValue("sDTRCode",Convert.ToDouble(sDTRCode)); Note : Converstion to Double was causing error log.
                NpgsqlCommand.Parameters.AddWithValue("sDTRCode", Convert.ToString(sDTRCode));
                NpgsqlCommand.Parameters.AddWithValue("sDTCCode", sDTCCode);
                sDFId = obj.get_value(sQry, NpgsqlCommand);
                return sDFId;
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return sDFId;
            }
        }


        public bool validateFailure(string sDFId)
        {
            NpgsqlCommand = new NpgsqlCommand();
            PGSqlConnection obj = new PGSqlConnection(Constants.Password);
            string sEstId = string.Empty;
            try
            {
                string sQry = string.Empty;
                sQry = "SELECT \"EST_ID\" FROM \"TBLESTIMATIONDETAILS\" WHERE  \"EST_FAILUREID\" =:sDFId";
                NpgsqlCommand.Parameters.AddWithValue("sDFId", Convert.ToInt32(sDFId));
                sEstId = obj.get_value(sQry, NpgsqlCommand);
                if(sEstId.Length > 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }

        public bool SaveAlternateSupplydetails(string sAterID, string sFailid)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                PGSqlConnection obj = new PGSqlConnection(Constants.Password);
                string sQry = string.Empty;
                sQry = "UPDATE \"TBLDTCFAILURE\" SET \"DF_ALTERNATE_RPMT\" =:sAterID WHERE \"DF_ID\" =:sFailid";
                NpgsqlCommand.Parameters.AddWithValue("sAterID", sAterID);
                NpgsqlCommand.Parameters.AddWithValue("sFailid", Convert.ToInt32(sFailid));
                obj.ExecuteQry(sQry, NpgsqlCommand);
                return true;
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }
    }
}
