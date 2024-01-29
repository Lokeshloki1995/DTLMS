using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

using System.Data;
using Npgsql;
using System.Reflection;
using NpgsqlTypes;
using IIITS.PGSQL.DAL;
using IIITS.DTLMS.BL.DataBase;

namespace IIITS.DTLMS.BL
{
    public class clsStation
    {
        string strFormCode = "clsStation";
        PGSqlConnection Objcon = new PGSqlConnection(Constants.Password);
        DataBseConnection ObjBseCon = new DataBseConnection(Constants.Password);
        NpgsqlCommand NpgsqlCommand;

        public string sTaluqCode { get; set; }
        public string StationId { get; set; }
        public string OfficeCode { get; set; }
        public string StationCode { get; set; }
        public string StationName { get; set; }
        public string Description { get; set; }
        public string UserLogged { get; set; }
        public string Capacity { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string StationParentID { get; set; }
        public string OfficeName { get; set; }
        public bool IsSave { get; set; }
        public string sSubDivCode { get; set; }
        public string[] SaveStationDetails(clsStation ObjStation)
        {
            string[] Arr = new string[2];
            string strId = string.Empty;
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_save_update_station");
                cmd.Parameters.AddWithValue("station_id", ObjStation.StationId);
                cmd.Parameters.AddWithValue("station_code", ObjStation.StationCode);
                cmd.Parameters.AddWithValue("station_name", ObjStation.StationName.Replace("'", "''"));
                cmd.Parameters.AddWithValue("station_desc", ObjStation.Description.Replace("'", "''"));
                cmd.Parameters.AddWithValue("station_mobileno", ObjStation.MobileNo.Trim());
                cmd.Parameters.AddWithValue("station_user_logged", ObjStation.UserLogged);
                cmd.Parameters.AddWithValue("station_capacity", ObjStation.Capacity);
                cmd.Parameters.AddWithValue("station_officecode", ObjStation.OfficeCode);
                cmd.Parameters.AddWithValue("station_emailid", ObjStation.EmailId.Trim());
                cmd.Parameters.AddWithValue("station_parentid", '0');
                cmd.Parameters.AddWithValue("Taluq_code", ObjStation.sTaluqCode);
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
        public DataTable LoadStationDet(string strStationID = "", string sLocation = "")
        {            
            DataTable DtStationDet = new DataTable();            
            try
            {
                #region Old inline query
                //NpgsqlCommand = new NpgsqlCommand();
                //string strQry = string.Empty;
                //strQry = "SELECT \"ST_ID\",CAST(\"ST_STATION_CODE\" AS TEXT) \"ST_STATION_CODE\",\"ST_NAME\",\"ST_DESCRIPTION\",";
                //strQry += "\"STC_CAP_VALUE\",\"ST_PARENT_STATID\" FROM (SELECT \"ST_ID\",\"ST_STATION_CODE\",\"ST_PARENT_STATID\",";
                //strQry += "\"ST_NAME\",\"ST_DESCRIPTION\",\"STC_CAP_VALUE\" FROM \"TBLSTATION\",\"TBLSTATIONCAPACITY\"";
                //strQry += "WHERE \"ST_STC_CAP_ID\"=\"STC_CAP_ID\"";

                //if (strStationID != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("stationId", Convert.ToInt32(strStationID));
                //    strQry += " AND \"ST_ID\"= :stationId";
                //}
                //if (sLocation.Length > 0)
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("location", sLocation);
                //    strQry += " AND CAST(\"ST_STATION_CODE\" AS TEXT) LIKE :location||'%'";
                //}
                //strQry += ") \"STATION\" GROUP BY \"ST_ID\",\"ST_STATION_CODE\",\"ST_NAME\",\"ST_DESCRIPTION\",\"STC_CAP_VALUE\",\"ST_PARENT_STATID\"";
                //strQry += "  ORDER BY \"ST_STATION_CODE\"";
                //DtStationDet = Objcon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_stationdet_for_clsstation");
                cmd.Parameters.AddWithValue("p_stationid", Convert.ToString(strStationID ?? ""));
                cmd.Parameters.AddWithValue("p_location", Convert.ToString(sLocation ?? ""));
                DtStationDet = Objcon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return DtStationDet;
        }
        public DataTable LoadStationDetail(string strStationID = "")
        {           
            DataTable DtStationDet = new DataTable();            
            try
            {
                #region Old inline query
                //NpgsqlCommand = new NpgsqlCommand();
                //string strQry = string.Empty;
                //strQry = "SELECT \"ST_ID\",\"ST_STATION_CODE\",\"ST_NAME\",\"ST_DESCRIPTION\",\"ST_STC_CAP_ID\",\"ST_MOBILE_NO\",\"ST_EMAILID\",\"ST_OFF_CODE\"";
                //strQry += "FROM(SELECT \"ST_ID\",\"ST_STATION_CODE\",\"ST_NAME\",\"ST_DESCRIPTION\",\"ST_STC_CAP_ID\",\"ST_MOBILE_NO\",\"ST_EMAILID\",\"ST_OFF_CODE\"";
                //strQry += "FROM \"TBLSTATION\"";

                //if (strStationID != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("stationId1", Convert.ToInt32(strStationID));
                //    strQry += " WHERE \"ST_ID\"= :stationId1";
                //}
                //strQry += " )\"STATION\" GROUP BY \"ST_ID\",\"ST_STATION_CODE\",\"ST_NAME\",\"ST_DESCRIPTION\",\"ST_STC_CAP_ID\",\"ST_MOBILE_NO\",\"ST_EMAILID\",\"ST_OFF_CODE\"";
                //strQry += "  ORDER BY \"ST_STATION_CODE\"";
                //DtStationDet = Objcon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_stationdetail_for_clsstation");
                cmd.Parameters.AddWithValue("p_st_id", Convert.ToString(strStationID ?? ""));
                DtStationDet = Objcon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return DtStationDet;
        }
        public DataTable LoadOfficeDet(clsStation objStation)
        {
            DataTable DtStationDet = new DataTable();
            try
            {
                #region Old inline query
                //NpgsqlCommand = new NpgsqlCommand();
                //string strQry = string.Empty;
                //strQry = "SELECT \"OFF_CODE\",\"OFF_NAME\" FROM \"VIEW_ALL_OFFICES\" WHERE  \"OFF_CODE\" IS NOT NULL\"";
                //if (objStation.OfficeCode != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("OfficeCode", objStation.OfficeCode);
                //    strQry += "AND CAST(\"OFF_CODE\" AS TEXT) LIKE :OfficeCode||'%'";
                //    //strQry += "AND CAST(\"OFF_CODE\" AS TEXT) LIKE  '" + objStation.OfficeCode + "'";
                //}
                //if (objStation.OfficeName != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("OfficeName", objStation.OfficeName);
                //    strQry += "\"OFF_NAME\" LIKE :OfficeName||'%'";
                //}
                //strQry += " order by \"OFF_CODE\"";
                //DtStationDet = Objcon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_loadofficedet_for_clsstation");
                cmd.Parameters.AddWithValue("p_offcode", Convert.ToString(objStation.OfficeCode ?? ""));
                cmd.Parameters.AddWithValue("p_offname", Convert.ToString(objStation.OfficeName ?? "").ToUpper());
                DtStationDet = Objcon.FetchDataTable(cmd);

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return DtStationDet;
        }
        public string GenerateSectionCode(clsStation objStation)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string sStaCode = string.Empty;
            string QryKey = string.Empty;
            try
            {
                #region Old inline query
                //string sFinancialYear = string.Empty;
                //NpgsqlCommand.Parameters.AddWithValue("StationCode", objStation.StationCode);
                ////sStaCode = Objcon.get_value("SELECT COALESCE(CAST(MAX(\"ST_STATION_CODE\")AS NUMERIC),0)+1 FROM \"TBLSTATION\"where  substr(\"ST_STATION_CODE\",1,2)='" + objStation.StationCode + "'");
                //sStaCode = Objcon.get_value("SELECT COALESCE(CAST(MAX(\"ST_STATION_CODE\")AS NUMERIC),0)+1 FROM \"TBLSTATION\" where  CAST(\"ST_STATION_CODE\" AS TEXT)  LIKE :StationCode||'%'",NpgsqlCommand);
                #endregion

                QryKey = "GET_MAX_ST_STATION_CODE";
                NpgsqlCommand cmd_TC_SLNO = new NpgsqlCommand("fetch_getvalue_clsstation");
                cmd_TC_SLNO.Parameters.AddWithValue("p_key", QryKey);
                cmd_TC_SLNO.Parameters.AddWithValue("p_value_1", Convert.ToString(objStation.StationCode ?? ""));
                cmd_TC_SLNO.Parameters.AddWithValue("p_value_2", "");
                sStaCode = ObjBseCon.StringGetValue(cmd_TC_SLNO);

                if (Convert.ToInt16(sStaCode) <= 1)
                {
                    sStaCode = objStation.StationCode + "01";
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return sStaCode;
        }

        #region Unused code
        //public string[] SaveStationDetails(clsStation ObjStation)
        //{
        //    string[] Arrmsg = new string[2];
        //    try
        //    {

        //        if (ObjStation.IsSave)
        //        {
        //             //Check For dup;licate Station Code
        //            OleDbDataReader dr = objCon.Fetch("select ST_ID FROM TBLSTATION WHERE ST_STATION_CODE='" + ObjStation.StationCode + "'");
        //            if (dr.Read())
        //            {
        //                Arrmsg[0] = "Station Code  Already Exist";
        //                Arrmsg[1] = "4";
        //                return Arrmsg;
        //            }
        //            dr.Close();

        //            dr = objCon.Fetch("select * FROM TBLSTATION WHERE ST_NAME='" + ObjStation.StationName + "'");
        //            if (dr.Read())
        //            {
        //                Arrmsg[0] = "Station Name  Already Exist";
        //                Arrmsg[1] = "4";
        //                return Arrmsg;
        //            }
        //            dr.Close();

        //            long slno = objCon.Get_max_no("ST_ID", "TBLSTATION");
        //            string strInsqry = "insert into TBLSTATION(ST_ID,ST_STATION_CODE,ST_NAME,ST_DESCRIPTION,ST_MOBILE_NO,ST_EMAILID,ST_OFF_CODE,ST_STC_CAP_ID,ST_ENTRY_AUTH,ST_PARENT_STATID) ";
        //            strInsqry += " values('" + slno + "','" + ObjStation.StationCode + "', '" + ObjStation.StationName.Replace("'", "''") + "',";
        //            strInsqry += " '" + ObjStation.Description.ToUpper().Replace("'", "''") + "' ,'" + ObjStation.MobileNo.Trim() + "','" + ObjStation.EmailId.Trim() + "', ";
        //            strInsqry += " '" + ObjStation.OfficeCode + "','" + ObjStation.Capacity + "','" + ObjStation.UserLogged + "','"+ ObjStation.StationParentID +"')";
        //            objCon.Execute(strInsqry);
        //            Arrmsg[0] = "Station Information has been Saved Sucessfully.";//proc_save_station
        //            Arrmsg[1] = "0";
        //            return Arrmsg;

        //        }
        //        else
        //        {
        //            //Check For dup;licate Station Code
        //            OleDbDataReader dr = objCon.Fetch("select ST_ID FROM TBLSTATION WHERE ST_STATION_CODE='" + ObjStation.StationCode + "' AND ST_ID<>'" + ObjStation.StationId + "'");
        //            if (dr.Read())
        //            {
        //                Arrmsg[0] = "Station Code Already Exist";
        //                Arrmsg[1] = "4";
        //                return Arrmsg;
        //            }
        //            dr.Close();

        //            dr = objCon.Fetch("select * FROM TBLSTATION WHERE ST_NAME='" + ObjStation.StationName + "' AND ST_ID<>'" + ObjStation.StationId + "'");
        //            if (dr.Read())
        //            {
        //                Arrmsg[0] = "Station Name Already Exist";
        //                Arrmsg[1] = "4";
        //                return Arrmsg;
        //            }
        //            dr.Close();

        //            string strUpdqry = "update TBLSTATION set ST_NAME='" + ObjStation.StationName.ToUpper().Replace("'", "''") + "',ST_STATION_CODE='" + ObjStation.StationCode + "',";
        //            strUpdqry += " ST_DESCRIPTION='" + ObjStation.Description.Replace("'", "''") + "',ST_ENTRY_AUTH='" + ObjStation.UserLogged + "',ST_ENTRY_DATE=SYSDATE,";
        //            strUpdqry += " ST_STC_CAP_ID='" + ObjStation.Capacity + "',ST_MOBILE_NO='" + ObjStation.MobileNo.Trim() + "',ST_EMAILID='" + ObjStation.EmailId.Trim() + "',";
        //            strUpdqry += "  ST_PARENT_STATID='" + ObjStation.StationParentID + "'  where ST_ID = '" + ObjStation.StationId + "'";//proc_update_station
        //            objCon.Execute(strUpdqry);

        //            Arrmsg[0] = "Station Information has been Updated Sucessfully.";
        //            Arrmsg[1] = "0";
        //            return Arrmsg;

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "SaveStationDetails");
        //        return Arrmsg;
        //    }
        //}

        //public DataTable LoadStationDet(string strStationID="")
        //{
        //    DataTable DtStationDet = new DataTable();
        //    try
        //    {
        //         string strQry = string.Empty;
        //         strQry = " SELECT ST_ID,TO_CHAR(ST_STATION_CODE) ST_STATION_CODE,ST_NAME,ST_DESCRIPTION,STC_CAP_VALUE,ST_PARENT_STATID ";
        //         strQry += " FROM  (SELECT ST_ID,ST_STATION_CODE,ST_PARENT_STATID,ST_NAME,ST_DESCRIPTION,STC_CAP_VALUE";
        //         strQry += " FROM TBLSTATION,TBLSTATIONCAPACITY  ";
        //         strQry += " WHERE  ST_STC_CAP_ID=STC_CAP_ID";

        //       if (strStationID != "")
        //       {
        //           strQry += " AND ST_ID='" + strStationID + "'";
        //       }
        //       strQry += "  ) GROUP BY ST_ID,ST_STATION_CODE,ST_NAME,ST_DESCRIPTION,STC_CAP_VALUE,ST_PARENT_STATID";
        //       OleDbDataReader drstation = objCon.Fetch(strQry);
        //       DtStationDet.Load(drstation);
        //       return DtStationDet;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadStationDet");
        //        return DtStationDet;
        //    }
        //}
        //public DataTable LoadStationDetail(string strStationID = "")
        //{
        //    DataTable DtStationDet = new DataTable();
        //    try
        //    {
        //        string strQry = string.Empty;

        //       strQry = " SELECT ST_ID,ST_STATION_CODE,ST_NAME,ST_DESCRIPTION,ST_STC_CAP_ID, ST_MOBILE_NO, ST_EMAILID";
        //       strQry += " FROM  (SELECT ST_ID,ST_STATION_CODE,ST_NAME,ST_DESCRIPTION,ST_STC_CAP_ID,";
        //       strQry += "  ST_MOBILE_NO, ST_EMAILID   FROM TBLSTATION";

        //        if (strStationID != "")
        //        {
        //            strQry += " WHERE ST_ID='" + strStationID + "'";
        //        }
        //        strQry += "  ) GROUP BY ST_ID,ST_STATION_CODE,ST_NAME,ST_DESCRIPTION,ST_STC_CAP_ID, ST_MOBILE_NO, ST_EMAILID";
        //        OleDbDataReader drstation = objCon.Fetch(strQry);
        //        DtStationDet.Load(drstation);
        //        return DtStationDet;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadStationDetail");
        //        return DtStationDet;
        //    }
        //}
        //public DataTable LoadOfficeDet(clsStation objStation)
        //{
        //    DataTable DtStationDet = new DataTable();
        //    try
        //    {
        //        string strQry = string.Empty;

        //        strQry = "select OFF_CODE,OFF_NAME FROM VIEW_ALL_OFFICES WHERE  OFF_CODE IS NOT NULL ";
        //        if (objStation.OfficeCode != "")
        //        {
        //            strQry += " AND OFF_CODE LIKE '"+ objStation.OfficeCode +"'";
        //        }
        //        if (objStation.OfficeName  != "")
        //        {
        //            strQry += " AND OFF_NAME LIKE '" + objStation.OfficeName + "'";
        //        }
        //        strQry+= " order by OFF_CODE";
        //        OleDbDataReader drstat = objCon.Fetch(strQry);
        //        DtStationDet.Load(drstat);
        //        return DtStationDet;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadOfficeDet");
        //        return DtStationDet;
        //    }
        //}

        //public DataTable LoadOfficeDet(clsStation objStation)
        //{
        //    DataTable DtStationDet = new DataTable();
        //    string strQry = string.Empty;
        //    try
        //    {
        //        strQry = "SELECT \"OFF_CODE\",\"OFF_NAME\" FROM \"VIEW_ALL_OFFICES\" WHERE  \"OFF_CODE\" IS NOT NULL\"";
        //        if (objStation.OfficeCode != "")
        //        {
        //            strQry += "AND CAST(\"OFF_CODE\" AS TEXT) LIKE  '" + objStation.OfficeCode + "'";
        //        }
        //        if (objStation.OfficeName != "")
        //        {
        //            strQry += "\"OFF_NAME\" LIKE'" + objStation.OfficeName + "'";
        //        }
        //        strQry += " order by \"OFF_CODE\"";
        //        DtStationDet = Objcon.FetchDataTable(strQry);
        //     }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError("QUERY-" + strQry, ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);               
        //    }
        //    return DtStationDet;
        //}

        //public string GenerateSectionCode(clsStation objStation)
        //{
        //    try
        //    {
        //        string sMaxNo = string.Empty;
        //        string sFinancialYear = string.Empty;
        //        string sStaCode = objCon.get_value("SELECT NVL(MAX(ST_STATION_CODE),0)+1 FROM TBLSTATION  where  substr(ST_STATION_CODE,1,2) ='" + objStation.StationCode + "'");
        //        if (sStaCode.Length > 0)
        //        {
        //            StationCode = sStaCode.ToString();
        //        }
        //        return sStaCode;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GenerateOmSecCode");
        //        return "";
        //    }
        //}
        #endregion
    }
}

