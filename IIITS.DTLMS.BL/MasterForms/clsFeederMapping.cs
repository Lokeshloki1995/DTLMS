using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIITS.DTLMS.BL.MasterForms
{
    public  class clsFeederMapping
    {
        PGSqlConnection Objcon = new PGSqlConnection(Convert.ToString(ConfigurationManager.AppSettings["pgSQLPassword"]));
        public DataTable ddtCapacityGrid { get; set; }
        string strFormCode = "clsFeederMapping";
        public Int64 FeederID { get; set; }
        public string FeederCode { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public Int64 Stationid { get; set; }
        public Int64 FDO_Feederid { get; set; }


        SQLiteConnection sql_con;
        NpgsqlCommand NpgsqlCommand;
        public DataTable LoadFeederDet(string station)
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationManager.AppSettings["pgSQLPassword"]));
            DataTable DtStationDet = new DataTable();
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                NpgsqlCommand = new NpgsqlCommand("sp_loadfeederdet");
                NpgsqlCommand.Parameters.AddWithValue("fd_st_id", station);
                DtStationDet = ObjCon.FetchDataTable(NpgsqlCommand);
                //string strQry = string.Empty;
                //strQry = "SELECT \"FD_FEEDER_ID\", \"FD_FEEDER_CODE\"||'~'||  \"FD_FEEDER_NAME\"  as \"FD_FEEDER_NAME\" FROM  \"TBLFEEDERMAST\"   WHERE \"FD_ST_ID\"='" + station + "'";

                //DtStationDet = ObjCon.FetchDataTable(strQry, NpgsqlCommand);

                return DtStationDet;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return DtStationDet;
            }
        }


        public int DeleteFeederOffCode(string feeder_id, string OfficeCode)
        {
            //PGSqlConnection ObjConn = new PGSqlConnection(Convert.ToString(ConfigurationManager.AppSettings["pgSQLPassword"]));
            //DataTable DtStationDet = new DataTable();
            //NpgsqlCommand = new NpgsqlCommand();
            string[] Arr = new string[3];
            int n = 0;
            try
            {
                //string strQry = string.Empty;
                NpgsqlCommand cmd = new NpgsqlCommand("sp_deletefeederoffcode");
                cmd.Parameters.AddWithValue("fdrid", feeder_id);
                cmd.Parameters.AddWithValue("frdoffcode", OfficeCode);
                cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                Arr[0] = "msg";
                Arr[1] = "op_id";
                Arr = Objcon.Execute(cmd, Arr, 2);

                if (Arr[1] == "0")
                {
                    n = 1;
                }


              //  strQry = "delete  from \"TBLFEEDEROFFCODE\" where \"FDO_FEEDER_ID\"=" + feeder_id + " and \"FDO_OFFICE_CODE\"=" + OfficeCode + "";
                //int n = ObjConn.ExecuteQry(strQry,cmd);
                //return n;
              //  NpgsqlCommand cmd = new NpgsqlCommand();
             //   NpgsqlCommand.Parameters.AddWithValue("fdo_feederid", feeder_id);
             //   NpgsqlCommand.Parameters.AddWithValue("fdo_offcode", OfficeCode);

             //   cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
             //   cmd.Parameters.Add("msg", NpgsqlDbType.Text);


             //   cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
             //   cmd.Parameters["msg"].Direction = ParameterDirection.Output;

             //   Arr[0] = "op_id";
             //   Arr[1] = "msg";

             //Arr= Objcon.Execute(cmd, Arr, 2);
             }              
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return n;
            }
            return n;
        }




        //DtStationDet = ObjCon.FetchDataTable(NpgsqlCommand);
        // return DtStationDet;
        //string strQry = string.Empty;
        //strQry = "delete  from \"TBLFEEDEROFFCODE\" where \"FDO_FEEDER_ID\"=" + feeder_id + " and \"FDO_OFFICE_CODE\"=" + OfficeCode + "";

        //int n = int.Parse(DtStationDet.Rows[0][0].ToString());
        public DataTable LoadFeederMastDet(string sOfficeCode, string strFeederName = "", string strFeederCode = "", string sStationName = "")
        {
            NpgsqlCommand = new NpgsqlCommand();
            DataTable DtFeederfDet = new DataTable();
            string strQry = string.Empty;
            try
            {
                if (sOfficeCode.Length >= 5)
                {
                    sOfficeCode = sOfficeCode.Substring(0, Constants.Section);
                }
                NpgsqlCommand NpgsqlCommand = new NpgsqlCommand("sp_loadfeedermastdet");
                NpgsqlCommand.Parameters.AddWithValue("offcode", sOfficeCode);
                NpgsqlCommand.Parameters.AddWithValue("offcode1", sOfficeCode);
                NpgsqlCommand.Parameters.AddWithValue("feedname", strFeederName);
                NpgsqlCommand.Parameters.AddWithValue("feedcode", strFeederCode);
                NpgsqlCommand.Parameters.AddWithValue("stationname", sStationName);

                //strQry = "SELECT distinct \"FD_FEEDER_NAME\",\"FD_FEEDER_CODE\",\"FD_FEEDER_ID\", \"OFF_CODE\",\"ST_NAME\",\"OFF_NAME\",  case when  \"FD_FEEDER_CODE\"=\"DT_FDRSLNO\"  then 1 else 0 end as status  FROM (SELECT \"FD_FEEDER_NAME\",\"FD_FEEDER_CODE\",\"FD_FEEDER_ID\",\"FDO_OFFICE_CODE\"  as \"OFF_CODE\",";
                //strQry += " (SELECT \"ST_NAME\" FROM \"TBLSTATION\" WHERE \"ST_ID\"=\"FD_ST_ID\" ) \"ST_NAME\",(SELECT \"OFF_NAME\" from \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT)=CAST(\"FDO_OFFICE_CODE\" AS TEXT) ) AS \"OFF_NAME\" ";
                //strQry += " FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\",\"VIEW_ALL_OFFICES\" WHERE \"FD_FEEDER_CODE\" IS NOT NULL";
                //strQry += " AND \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" AND \"OFF_CODE\"=\"FDO_OFFICE_CODE\" AND cast(\"FDO_OFFICE_CODE\" as TEXT) LIKE :offcode||'%' Order BY \"FD_FEEDER_ID\")A  left join     \"TBLDTCMAST\" on \"FD_FEEDER_CODE\"=\"DT_FDRSLNO\"  and cast(\"DT_OM_SLNO\" as TEXT) LIKE  :offcode1||'%'  WHERE \"FD_FEEDER_ID\" IS NOT NULL AND \"ST_NAME\" IS NOT NULL";
                //if (strFeederName != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("feedname", strFeederName.ToUpper());
                //    strQry += " AND UPPER(CAST(\"FD_FEEDER_NAME\"AS TEXT)) like :feedname||'%'";
                //}
                //if (strFeederCode != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("feedcode", strFeederCode.ToUpper());
                //    strQry += " AND CAST(\"FD_FEEDER_CODE\"AS TEXT) like :feedcode||'%'";
                //}
                //if (sStationName != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("stationname", sStationName.ToUpper());
                //    strQry += " AND UPPER(CAST(\"ST_NAME\"AS TEXT)) like :stationname||'%'";
                //}

                //strQry += "  ORDER BY \"FD_FEEDER_CODE\" ";

                ///   OleDbDataReader drcorp = Objcon.Fetch(strQry);
                /////     DtFeederfDet.Load(drcorp);
                DtFeederfDet = Objcon.FetchDataTable(NpgsqlCommand);
                return DtFeederfDet;
            }

            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return DtFeederfDet;
            }
        }
        public string[] SaveFeederOffCode(clsFeederMapping objfeeder)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string strQry = string.Empty;
            string[] Arr = new string[3];
            string[] Arr2 = new string[0];

            string str;
            byte[] imageData = null;
            NpgsqlParameter docPhoto = new NpgsqlParameter();
            NpgsqlCommand comd = new NpgsqlCommand();
            try
            {

                if (objfeeder.ddtCapacityGrid.Rows.Count > 0)
                {

                    NpgsqlCommand cmd = new NpgsqlCommand("sp_saveddeletefeederoffcode");
                    cmd.Parameters.AddWithValue("frdoffcode", Convert.ToString(objfeeder.ddtCapacityGrid.Rows[0]["OFF_CODE"]));
                    // strQry = "delete  from \"TBLFEEDEROFFCODE\" where   \"FDO_OFFICE_CODE\"=" + Convert.ToString(objfeeder.ddtCapacityGrid.Rows[0]["OFF_CODE"]) + "";

                    //  int n = Objcon.ExecuteQry(strQry);
                    //int n = Objcon.ExecuteQry(strQry,cmd);
                    Arr2 = Objcon.Execute(cmd, Arr2, 0);


                    for (int i = 0; i < objfeeder.ddtCapacityGrid.Rows.Count; i++)
                    {

                        string[] Arr1 = new string[2];
                        NpgsqlCommand cmd1 = new NpgsqlCommand("proc_save_feeder_offcode");
                     //   objfeeder.FDO_Feederid = Convert.ToInt32(Objcon.Get_max_no("FDO_ID", "TBLFEEDEROFFCODE"));

                       // cmd1.Parameters.AddWithValue("fdo_id", Convert.ToString( objfeeder.FDO_Feederid));
                        cmd1.Parameters.AddWithValue("feeder_id", Convert.ToString(objfeeder.ddtCapacityGrid.Rows[i]["FD_FEEDER_ID"]));
                        cmd1.Parameters.AddWithValue("offcode", Convert.ToString(objfeeder.ddtCapacityGrid.Rows[i]["OFF_CODE"]));

                        cmd1.Parameters.Add("status", NpgsqlDbType.Text);
                        cmd1.Parameters.Add("msg", NpgsqlDbType.Text);
                      

                        cmd1.Parameters["status"].Direction = ParameterDirection.Output;
                        cmd1.Parameters["msg"].Direction = ParameterDirection.Output;
                       
                        Arr[0] = "status";
                        Arr[1] = "msg";
                        
                        Arr = Objcon.Execute(cmd1, Arr, 2);      
                    }

                     Objcon.CommitTransaction();
                  
                    return Arr;
                }
                else
                {                  
                    Arr[0] = "Please Select Feeder Code For save Details";
                    Arr[1] = "1";
                    return Arr;
                }             
            }

            catch (Exception ex)
            {
                // Objcon.RollBackTrans();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }



    }
}
