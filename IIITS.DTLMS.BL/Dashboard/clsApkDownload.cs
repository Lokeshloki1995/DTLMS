using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.PGSQL.DAL;
using System.Data;
using System.Data.OleDb;
using IIITS.DTLMS.BL.DataBase;
using Npgsql;

namespace IIITS.DTLMS.BL
{
    public class clsApkDownload
    {
        string strFormCode = "clsApkDownload";
        PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
        DataBseConnection ObjBasCon = new DataBseConnection(Constants.Password);
        public string RetrieveLatestApkDetails()
        {
            string sFolderName = string.Empty;
            clsApkDownload objReturnApk = new clsApkDownload();
            try
            {
                NpgsqlCommand NpgsqlCommand = new NpgsqlCommand();
                //sFolderName = ObjCon.get_value("SELECT MAX(\"AP_FOLDER_PATH\") FROM \"TBLDTLMSAPK\" ");
                NpgsqlCommand cmd = new NpgsqlCommand("proc_retrieve_latest_apkdetails");
                sFolderName = ObjBasCon.StringGetValue(cmd);

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return sFolderName;
            }

            return sFolderName;
        }
    }
}
