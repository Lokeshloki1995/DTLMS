using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IIITS.PGSQL.DAL;
using Npgsql;
using System.Reflection;


namespace IIITS.DTLMS.BL
{
    public class clsCommon
    {   
        string strFormCode = "clsCommon";
       // CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        PGSqlConnection Objcon = new PGSqlConnection(Constants.Password);
        public DataTable  GetAppSettings()
        {
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_app_details");
                dt = Objcon.FetchDataTable(cmd);
         
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return dt;
        }
    }
}
