using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Configuration;
using IIITS.PGSQL.DAL;
using Npgsql;


namespace IIITS.DTLMS.BL
{
    public class clsVendorMaster
    {
        string StrQry = string.Empty;
       // CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);

        public string sVendorName { get; set; }
        public string sVendorNumber { get; set; }
        public string sVendorId { get; set; }

        public string save_Vendor_Details(clsVendorMaster objVendor)
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            try
            {
                StrQry = "INSERT INTO \"TBLVENDORMASTER\" VALUES ((SELECT COALESCE(max(\"VM_ID\"),0)+1 FROM \"TBLVENDORMASTER\"),'" + objVendor.sVendorName+ "','"+objVendor.sVendorNumber+"')";
                ObjCon.ExecuteQry(StrQry);
                return "1";
            }
            catch(Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return "0";
            }
        }

        public string update_Vendor_Details(clsVendorMaster objVendor)
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            try
            {
                StrQry = "UPDATE \"TBLVENDORMASTER\" SET \"VM_NAME\"=UPPER('"+ objVendor.sVendorName + "'),\"VM_MOBILE_NUM\"='"+ objVendor.sVendorNumber +"' WHERE \"VM_ID\"='"+ objVendor.sVendorId + "'";
                ObjCon.ExecuteQry(StrQry);
                return "1";
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return "0";
            }
        }

        public DataTable GetVendorDetails(string sVendorId)
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            DataTable dtVendorDetails = new DataTable();
            try
            {
                StrQry = "SELECT * FROM \"TBLVENDORMASTER\" WHERE \"VM_ID\"='"+ sVendorId + "'";
                dtVendorDetails = ObjCon.FetchDataTable(StrQry);
                return dtVendorDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return dtVendorDetails;
            }
        }

        public DataTable LoadVendorDetails(string sVendorName)
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            DataTable dtVendorDetails = new DataTable();
            try
            {
                StrQry = "SELECT * FROM \"TBLVENDORMASTER\" WHERE \"VM_NAME\" LIKE UPPER('" + sVendorName + "%')";
                dtVendorDetails = ObjCon.FetchDataTable(StrQry);
                return dtVendorDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return dtVendorDetails;
            }
        }

        public DataTable LoadVendorDetails()
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            DataTable dtVendorDetails = new DataTable();
            try
            {
                StrQry = "SELECT \"VM_ID\",\"VM_NAME\",\"VM_MOBILE_NUM\" FROM \"TBLVENDORMASTER\" ";
                dtVendorDetails = ObjCon.FetchDataTable(StrQry);
                return dtVendorDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return dtVendorDetails;
            }
        }
    }
}
