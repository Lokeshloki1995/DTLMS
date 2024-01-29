using IIITS.DTLMS.BL.DataBase;
using IIITS.PGSQL.DAL;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIITS.DTLMS.BL.Dashboard
{
    public  class ClsDownload
    {
        PGSqlConnection Objcon = new PGSqlConnection(Constants.Password);
        NpgsqlCommand NpgsqlCommand;

        DataBseConnection ObjBasCon = new DataBseConnection(Constants.Password);

        public string GetEnumUserManualDoc(string type)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string path = string.Empty;
            try
            {
                string strQry = string.Empty;

                #region inline query
                //strQry = "SELECT \"UM_FILENAME\" from \"TBLUSERMANUALDOC\" where \"UM_UMT_ID\"='"+ type + "' AND \"UM_STATUS\"= '1'";
                //path = Objcon.get_value(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_enum_usermanual_doc");
                cmd.Parameters.AddWithValue("type", Convert.ToString(type ?? ""));
                path = ObjBasCon.StringGetValue(cmd);

                return path;
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return path;
            }
        }
    }
}
