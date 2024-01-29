using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Configuration;
using System.Data.SQLite;
using IIITS.PGSQL.DAL;
using Npgsql;
using IIITS.DTLMS.BL.DataBase;
using NpgsqlTypes;

namespace IIITS.DTLMS.BL
{
    public class clsFeederMast
    {
        string strFormCode = "clsFeederMast";
        string strQry = string.Empty;

        PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationManager.AppSettings["pgSQLPassword"]));
        DataBseConnection ObjBseCon = new DataBseConnection(Constants.Password);
        NpgsqlCommand NpgsqlCommand;

        public Int64 FeederID { get; set; }
        public string FeederCode { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public Int64 Stationid { get; set; }
        public Int64 BankId { get; set; }
        public Int64 BusId { get; set; }
        public string FeederName { get; set; }
        public string FeederType { get; set; }
        public string FeederCategory { get; set; }
        public string FeederInterflow { get; set; }
        public string FeederDCC { get; set; }
        public string UserLogged { get; set; }
        public bool IsSave { get; set; }
        public string MDMFeedercode { get; set; }
        public string TotalDtc { get; set; }

        /// <summary>
        /// Feeder Master
        /// </summary>
        /// <param name="objFeederMaster"></param>
        /// <returns></returns>
        public string[] FeederMaster(clsFeederMast objFeederMaster)
        {
            string[] Arrmsg = new string[2];
            string[] Arr = new string[2];
            NpgsqlCommand = new NpgsqlCommand();
            string QryKey = string.Empty;
            string strFeederId = string.Empty;
            string StrOfficeCodeExist = string.Empty;
            
            try
            {
                string[] strQryVallist = null;

                if (objFeederMaster.OfficeCode != "")
                {
                    strQryVallist = objFeederMaster.OfficeCode.Split(',');
                }

                if (objFeederMaster.IsSave)
                {
                    #region Old Inline queary                                        
                    //NpgsqlCommand.Parameters.AddWithValue("FeederCode", objFeederMaster.FeederCode);
                    //strFeederId = ObjCon.get_value("select \"FD_FEEDER_ID\" FROM \"TBLFEEDERMAST\" WHERE CAST(\"FD_FEEDER_CODE\" AS TEXT)=:FeederCode", NpgsqlCommand);
                    #endregion

                    QryKey = "GET_FD_FEEDER_ID";
                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsfeedermast");
                    cmd.Parameters.AddWithValue("p_key", QryKey);
                    cmd.Parameters.AddWithValue("p_value_1", Convert.ToString(objFeederMaster.FeederCode ?? ""));
                    cmd.Parameters.AddWithValue("p_value_2", "");
                    strFeederId = ObjBseCon.StringGetValue(cmd);

                    if (strFeederId == "")
                    {
                        strFeederId = "0";
                    }
                    else
                    {
                        #region Old Inline queary  
                        //NpgsqlCommand.Parameters.AddWithValue("FeederId", Convert.ToInt32(strFeederId));
                        //NpgsqlCommand.Parameters.AddWithValue("OfficeCode", Convert.ToString(OfficeCode));
                        //if (!ObjCon.get_value("SELECT count(*) FROM \"TBLFEEDEROFFCODE\" WHERE \"FDO_FEEDER_ID\" =:FeederId and cast(\"FDO_OFFICE_CODE\" as text) in (:OfficeCode)", NpgsqlCommand).ToString().Equals("0"))
                        //{
                        //    StrOfficeCodeExist += OfficeCode + ",";
                        //}
                        #endregion

                        QryKey = "GET_TBLFEEDEROFFCODE_COUNT";
                        NpgsqlCommand cmd_Count = new NpgsqlCommand("fetch_getvalue_clsfeedermast");
                        cmd_Count.Parameters.AddWithValue("p_key", QryKey);
                        cmd_Count.Parameters.AddWithValue("p_value_1", Convert.ToString(strFeederId ?? ""));
                        cmd_Count.Parameters.AddWithValue("p_value_2", Convert.ToString(OfficeCode ?? ""));

                        if (!ObjBseCon.StringGetValue(cmd_Count).Equals("0"))
                        {
                            StrOfficeCodeExist += OfficeCode + ",";
                        }
                        if (StrOfficeCodeExist != "")
                        {
                            Arrmsg[0] = "Feeder Code  Already Present";
                            Arrmsg[1] = "4";
                            return Arrmsg;
                        }
                    }
                    string strQry = string.Empty;
                    long slno = ObjCon.Get_max_no("FD_FEEDER_ID", "TBLFEEDERMAST");

                    #region Old Inline queary                
                    //NpgsqlCommand.Parameters.AddWithValue("slno", slno);
                    //NpgsqlCommand.Parameters.AddWithValue("FeederCode1", objFeederMaster.FeederCode);
                    //NpgsqlCommand.Parameters.AddWithValue("FeederName", objFeederMaster.FeederName.Replace("'", "''"));
                    //NpgsqlCommand.Parameters.AddWithValue("UserLogged", Convert.ToInt32(objFeederMaster.UserLogged));
                    //NpgsqlCommand.Parameters.AddWithValue("BusId", objFeederMaster.BusId);
                    //NpgsqlCommand.Parameters.AddWithValue("FeederCategory", Convert.ToInt32(objFeederMaster.FeederCategory));
                    //NpgsqlCommand.Parameters.AddWithValue("FeederInterflow", Convert.ToInt32(objFeederMaster.FeederInterflow));
                    //NpgsqlCommand.Parameters.AddWithValue("FeederDCC", Convert.ToInt32(objFeederMaster.FeederDCC));
                    //NpgsqlCommand.Parameters.AddWithValue("Stationid", objFeederMaster.Stationid);
                    //NpgsqlCommand.Parameters.AddWithValue("MDMFeedercode", objFeederMaster.MDMFeedercode);
                    //strQry = "INSERT INTO \"TBLFEEDERMAST\"(\"FD_FEEDER_ID\",\"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\",";
                    //strQry += " \"FD_CREATED_AUTH\",\"FD_BS_ID\",\"FD_FC_ID\",\"FD_IS_INTERFLOW\",\"FD_DTC_CAPACITY\",\"FD_ST_ID\",\"FD_MDM_FEEDERCODE\")";
                    //strQry += " VALUES(:slno,:FeederCode1,:FeederName,";
                    //strQry += " :UserLogged,:BusId,:FeederCategory,";
                    //strQry += " :FeederInterflow,:FeederDCC,:Stationid,:MDMFeedercode)";
                    //ObjCon.ExecuteQry(strQry, NpgsqlCommand);
                    #endregion
                    
                    NpgsqlCommand cmd_insert = new NpgsqlCommand("proc_insert_tblfeedermast_for_clsfeedermast");
                    cmd_insert.Parameters.AddWithValue("p_slno", slno);
                    cmd_insert.Parameters.AddWithValue("p_feedercode", Convert.ToString(objFeederMaster.FeederCode ?? ""));
                    cmd_insert.Parameters.AddWithValue("p_feedername", Convert.ToString(objFeederMaster.FeederName ?? "").Replace("'", "''"));
                    cmd_insert.Parameters.AddWithValue("p_userlogged", Convert.ToInt32(objFeederMaster.UserLogged ?? ""));
                    cmd_insert.Parameters.AddWithValue("p_busid", objFeederMaster.BusId);
                    cmd_insert.Parameters.AddWithValue("p_feedercategory", Convert.ToInt32(objFeederMaster.FeederCategory));
                    cmd_insert.Parameters.AddWithValue("p_feederinterflow", Convert.ToInt32(objFeederMaster.FeederInterflow));
                    cmd_insert.Parameters.AddWithValue("p_feederdcc", Convert.ToInt32(objFeederMaster.FeederDCC));
                    cmd_insert.Parameters.AddWithValue("p_stationid", objFeederMaster.Stationid);
                    cmd_insert.Parameters.AddWithValue("p_mdmfeedercode", Convert.ToString(objFeederMaster.MDMFeedercode ?? ""));
                    cmd_insert.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd_insert.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd_insert.Parameters["msg"].Direction = ParameterDirection.Output;
                    cmd_insert.Parameters["op_id"].Direction = ParameterDirection.Output;
                    Arr[0] = "msg";
                    Arr[1] = "op_id";
                    Arr = ObjCon.Execute(cmd_insert, Arr, 2);


                    foreach (string officeCode in strQryVallist)
                    {
                        long fdo_id = ObjCon.Get_max_no("FDO_ID", "TBLFEEDEROFFCODE");

                        #region Old Inline queary 
                        //strQry = "Insert into \"TBLFEEDEROFFCODE\" (\"FDO_ID\",\"FDO_FEEDER_ID\",\"FDO_OFFICE_CODE\") VALUES ";
                        //strQry += "(" + Convert.ToInt32(fdo_id) + "," + Convert.ToInt32(slno) + " ," + Convert.ToInt32(officeCode) + ") ";
                        //ObjCon.ExecuteQry(strQry);
                        #endregion

                        NpgsqlCommand cmd_TBLFEEDEROFFCODE = new NpgsqlCommand("proc_insert_tblfeederoffcode_for_clsfeedermast");
                        cmd_TBLFEEDEROFFCODE.Parameters.AddWithValue("p_fdo_id", Convert.ToInt32(fdo_id));
                        cmd_TBLFEEDEROFFCODE.Parameters.AddWithValue("p_slno", Convert.ToInt32(slno));
                        cmd_TBLFEEDEROFFCODE.Parameters.AddWithValue("p_officecode", Convert.ToInt32(officeCode));                        
                        cmd_TBLFEEDEROFFCODE.Parameters.Add("msg", NpgsqlDbType.Text);
                        cmd_TBLFEEDEROFFCODE.Parameters.Add("op_id", NpgsqlDbType.Text);
                        cmd_TBLFEEDEROFFCODE.Parameters["msg"].Direction = ParameterDirection.Output;
                        cmd_TBLFEEDEROFFCODE.Parameters["op_id"].Direction = ParameterDirection.Output;
                        Arr[0] = "msg";
                        Arr[1] = "op_id";
                        Arr = ObjCon.Execute(cmd_TBLFEEDEROFFCODE, Arr, 2);
                    }
                    Arrmsg[0] = "Feeder Information Saved Successfully ";
                    Arrmsg[1] = "0";
                    return Arrmsg;
                }
                else
                {
                    StrOfficeCodeExist = string.Empty;

                    #region Old Inline queary 
                    //NpgsqlCommand.Parameters.AddWithValue("FeederCode2", objFeederMaster.FeederCode);
                    //NpgsqlCommand.Parameters.AddWithValue("FeederCategory1", Convert.ToInt32(objFeederMaster.FeederCategory));
                    //NpgsqlCommand.Parameters.AddWithValue("FeederName1", objFeederMaster.FeederName.Replace("'", "''"));
                    //NpgsqlCommand.Parameters.AddWithValue("UserLogged1", Convert.ToInt32(objFeederMaster.UserLogged));

                    //NpgsqlCommand.Parameters.AddWithValue("BusId1", objFeederMaster.BusId);
                    //NpgsqlCommand.Parameters.AddWithValue("FeederDCC1", Convert.ToInt32(objFeederMaster.FeederDCC));
                    //NpgsqlCommand.Parameters.AddWithValue("FeederInterflow1", Convert.ToInt32(objFeederMaster.FeederInterflow));
                    //NpgsqlCommand.Parameters.AddWithValue("Stationid1", objFeederMaster.Stationid);
                    //NpgsqlCommand.Parameters.AddWithValue("MDMFeedercode1", objFeederMaster.MDMFeedercode);
                    //NpgsqlCommand.Parameters.AddWithValue("FeederID1", objFeederMaster.FeederID);
                    //strQry = "UPDATE \"TBLFEEDERMAST\" SET \"FD_FEEDER_CODE\"=:FeederCode2,";
                    //strQry += " \"FD_CREATED_DATE\"=NOW() ,\"FD_FC_ID\"=:FeederCategory1,";
                    //strQry += " \"FD_FEEDER_NAME\"=:FeederName1,";
                    //strQry += " \"FD_CREATED_AUTH\"=:UserLogged1,\"FD_BS_ID\"=:BusId1,\"FD_DTC_CAPACITY\"=:FeederDCC1,";
                    //strQry += " \"FD_IS_INTERFLOW\"=:FeederInterflow1,\"FD_ST_ID\"=:Stationid1,\"FD_MDM_FEEDERCODE\"=:MDMFeedercode1";
                    //strQry += " where \"FD_FEEDER_ID\" =:FeederID1";
                    //ObjCon.ExecuteQry(strQry, NpgsqlCommand);

                    //NpgsqlCommand.Parameters.AddWithValue("FeederID2", Convert.ToInt32(objFeederMaster.FeederID));
                    //strQry = " DELETE FROM \"TBLFEEDEROFFCODE\" WHERE \"FDO_FEEDER_ID\" = :FeederID2";
                    //ObjCon.ExecuteQry(strQry, NpgsqlCommand);
                    #endregion

                    NpgsqlCommand cmd_tblfeedermast = new NpgsqlCommand("proc_update_tblfeedermast_for_clsfeedermast");
                    cmd_tblfeedermast.Parameters.AddWithValue("p_feedercode", objFeederMaster.FeederCode);
                    cmd_tblfeedermast.Parameters.AddWithValue("p_feedercategory", Convert.ToInt32(objFeederMaster.FeederCategory));
                    cmd_tblfeedermast.Parameters.AddWithValue("p_feedername", objFeederMaster.FeederName.Replace("'", "''"));
                    cmd_tblfeedermast.Parameters.AddWithValue("p_userlogged", Convert.ToInt32(objFeederMaster.UserLogged));
                    cmd_tblfeedermast.Parameters.AddWithValue("p_busid", objFeederMaster.BusId);
                    cmd_tblfeedermast.Parameters.AddWithValue("p_feederdcc", Convert.ToInt32(objFeederMaster.FeederDCC));
                    cmd_tblfeedermast.Parameters.AddWithValue("p_feederinterflow", Convert.ToInt32(objFeederMaster.FeederInterflow));
                    cmd_tblfeedermast.Parameters.AddWithValue("p_stationid", objFeederMaster.Stationid);
                    cmd_tblfeedermast.Parameters.AddWithValue("p_mdmfeedercode", objFeederMaster.MDMFeedercode);
                    cmd_tblfeedermast.Parameters.AddWithValue("p_feederid", Convert.ToString(objFeederMaster.FeederID));
                    cmd_tblfeedermast.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd_tblfeedermast.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd_tblfeedermast.Parameters["msg"].Direction = ParameterDirection.Output;
                    cmd_tblfeedermast.Parameters["op_id"].Direction = ParameterDirection.Output;
                    Arr[0] = "msg";
                    Arr[1] = "op_id";
                    Arr = ObjCon.Execute(cmd_tblfeedermast, Arr, 2);

                    foreach (string OfficeCode in strQryVallist)
                    {
                        long fdo_id = ObjCon.Get_max_no("FDO_ID", "TBLFEEDEROFFCODE");

                        #region
                        //strQry = "Insert into \"TBLFEEDEROFFCODE\" (\"FDO_ID\",\"FDO_FEEDER_ID\",\"FDO_OFFICE_CODE\") VALUES ";
                        //strQry += "(" + Convert.ToInt32(fdo_id) + "," + Convert.ToInt32(objFeederMaster.FeederID) + " ," + Convert.ToInt32(OfficeCode) + ") ";
                        //ObjCon.ExecuteQry(strQry);
                        #endregion

                        NpgsqlCommand cmd_TBLFEEDEROFFCODE = new NpgsqlCommand("proc_insert_tblfeederoffcode_for_clsfeedermast");
                        cmd_TBLFEEDEROFFCODE.Parameters.AddWithValue("p_fdo_id", Convert.ToInt32(fdo_id));
                        cmd_TBLFEEDEROFFCODE.Parameters.AddWithValue("p_slno", Convert.ToInt32(objFeederMaster.FeederID));
                        cmd_TBLFEEDEROFFCODE.Parameters.AddWithValue("p_officecode", Convert.ToInt32(OfficeCode));
                        cmd_TBLFEEDEROFFCODE.Parameters.Add("msg", NpgsqlDbType.Text);
                        cmd_TBLFEEDEROFFCODE.Parameters.Add("op_id", NpgsqlDbType.Text);
                        cmd_TBLFEEDEROFFCODE.Parameters["msg"].Direction = ParameterDirection.Output;
                        cmd_TBLFEEDEROFFCODE.Parameters["op_id"].Direction = ParameterDirection.Output;
                        Arr[0] = "msg";
                        Arr[1] = "op_id";
                        Arr = ObjCon.Execute(cmd_TBLFEEDEROFFCODE, Arr, 2);

                    }
                    Arrmsg[0] = "Feeder Information Updated Successfully ";
                    Arrmsg[1] = "0";
                    return Arrmsg;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arrmsg;
            }
        }
        /// <summary>
        /// Load Office Det
        /// </summary>
        /// <param name="objStation"></param>
        /// <returns></returns>
        public DataTable LoadOfficeDet(clsFeederMast objStation)
        {
            DataTable DtStationDet = new DataTable();
            try
            {
                #region Old LoadOfficeDet
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "select \"OFF_CODE\",\"OFF_NAME\" FROM \"VIEW_ALL_OFFICES\" WHERE  \"OFF_NAME\" IS NOT NULL AND LENGTH(cast(\"OFF_CODE\" as text))='4'";
                //if (objStation.OfficeCode != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("officecode1", objStation.OfficeCode);
                //    strQry += " AND CAST(\"OFF_CODE\" AS TEXT) LIKE :officecode1||'%'";
                //}
                //if (objStation.OfficeName != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("officename1", objStation.OfficeName.ToUpper());
                //    strQry += " AND UPPER(CAST(\"OFF_NAME\" AS TEXT)) LIKE '%'|| :officename1||'%'";
                //}
                //strQry += " order by \"OFF_CODE\"";
                //DtStationDet = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_officedet");
                cmd.Parameters.AddWithValue("p_offcode", (objStation.OfficeCode ?? ""));
                cmd.Parameters.AddWithValue("p_offname", (objStation.OfficeName ?? "").ToUpper());
                DtStationDet = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return DtStationDet;
            }
            return DtStationDet;
        }
        /// <summary>
        /// Load Office Dtr Mapping
        /// </summary>
        /// <param name="objStation"></param>
        /// <returns></returns>
        public DataTable LoadOfficeDtrMapping(clsFeederMast objStation)
        {
            DataTable DtStationDet = new DataTable();
            try
            {
                #region old LoadOfficeDtrMapping
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "select \"OFF_CODE\",\"OFF_NAME\" FROM \"VIEW_ALL_OFFICES\" WHERE  \"OFF_NAME\" IS NOT NULL AND LENGTH(cast(\"OFF_CODE\" as text))='3'";
                //if (objStation.OfficeCode != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("officecode1", objStation.OfficeCode);
                //    strQry += " AND CAST(\"OFF_CODE\" AS TEXT) LIKE :officecode1||'%'";
                //}
                //if (objStation.OfficeName != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("officename1", objStation.OfficeName.ToUpper());
                //    strQry += " AND UPPER(CAST(\"OFF_NAME\" AS TEXT)) LIKE '%'|| :officename1||'%'";
                //}
                //strQry += " order by \"OFF_CODE\"";
                //DtStationDet = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_officedtrmapping");
                cmd.Parameters.AddWithValue("p_offcode", (objStation.OfficeCode ?? ""));
                cmd.Parameters.AddWithValue("p_offname", (objStation.OfficeName ?? "").ToUpper());
                DtStationDet = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return DtStationDet;
            }
            return DtStationDet;
        }
        /// <summary>
        /// Load Office Details
        /// </summary>
        /// <param name="objStation"></param>
        /// <returns></returns>
        public DataTable LoadOfficeDetails(clsFeederMast objStation)
        {
            DataTable DtStationDet = new DataTable();
            try
            {
                #region Old LoadOfficeDetails
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "select \"OFF_CODE\",\"OFF_NAME\" FROM \"VIEW_ALL_OFFICES\" WHERE  \"OFF_NAME\" IS NOT NULL  ";
                //if (objStation.OfficeCode != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("officecode1", objStation.OfficeCode);
                //    strQry += " AND CAST(\"OFF_CODE\" AS TEXT) LIKE '%" + objStation.OfficeCode + "%'";
                //}
                //if (objStation.OfficeName != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("officename1", objStation.OfficeName.ToUpper());
                //    strQry += " AND UPPER(CAST(\"OFF_NAME\" AS TEXT)) LIKE '%" + objStation.OfficeName.ToUpper() + "%'";
                //}
                //strQry += " order by \"OFF_CODE\"";
                //DtStationDet = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_officedetails");
                cmd.Parameters.AddWithValue("p_offcode", (objStation.OfficeCode ?? ""));
                cmd.Parameters.AddWithValue("p_offname", (objStation.OfficeName ?? "").ToUpper());
                DtStationDet = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return DtStationDet;
            }
            return DtStationDet;
        }

        /// <summary>
        /// Get Feeder Details
        /// </summary>
        /// <param name="strFeederID"></param>
        /// <returns></returns>
        public DataTable GetFeederDetails(string strFeederID)
        {
            DataTable DtFeederfDet = new DataTable();
            try
            {
                #region Old GetFeederDetails
                //strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = " SELECT \"FD_MDM_FEEDERCODE\",\"FD_FEEDER_NAME\",\"FD_DTC_CAPACITY\",\"FD_IS_INTERFLOW\",\"FD_FEEDER_CODE\" AS \"FD_FEEDER_CODE\",\"FD_FEEDER_ID\",\"ST_ID\",";
                //strQry += " \"BS_ID\",\"FC_ID\",\"FC_FT_ID\",\"BN_ID\",array_to_string(array(select \"FDO_OFFICE_CODE\" ), ', ') \"OFFCODE\",\"DIST_CODE\",\"TALUQ_CODE\",\"STATION_CODE\" FROM  ";
                //strQry += " (SELECT \"FD_MDM_FEEDERCODE\",\"FD_FEEDER_NAME\",\"FD_FEEDER_CODE\" AS \"FD_FEEDER_CODE\",\"FD_DTC_CAPACITY\",\"FD_IS_INTERFLOW\",\"FDO_OFFICE_CODE\",";
                //strQry += " \"FD_FEEDER_ID\",\"FD_ST_ID\" AS \"ST_ID\",\"FD_BS_ID\" AS \"BS_ID\",\"FD_FC_ID\" AS \"FC_ID\",(SELECT DISTINCT \"FT_ID\" FROM \"TBLFDRTYPE\",";
                //strQry += " \"TBLFEEDERCATEGORY\" WHERE CAST(\"FC_FT_ID\" AS TEXT)=CAST(\"FT_ID\" AS TEXT) AND CAST(\"FD_FC_ID\" AS TEXT)=CAST(\"FC_ID\" AS TEXT)) \"FC_FT_ID\", ";
                //strQry += " (SELECT DISTINCT \"BN_ID\" FROM \"TBLBANK\",\"TBLBUS\" WHERE CAST(\"BS_BN_ID\" AS TEXT)=CAST(\"BN_ID\" AS TEXT) AND ";
                //strQry += " CAST(\"BS_ID\" AS TEXT)=CAST(\"FD_BS_ID\" AS TEXT)) \"BN_ID\", (SELECT \"DT_CODE\" FROM \"TBLDIST\" WHERE ";
                //strQry += " cast(\"DT_CODE\" as text)=substr(cast(\"FD_FEEDER_CODE\" as text),1,1))\"DIST_CODE\",(SELECT DISTINCT \"TQ_CODE\" FROM \"TBLTALQ\" WHERE ";
                //strQry += " cast(\"TQ_CODE\" as text)=substr(cast(\"FD_FEEDER_CODE\" as text),1,2))\"TALUQ_CODE\", (SELECT \"ST_ID\" FROM \"TBLSTATION\" WHERE \"ST_STATION_CODE\" = substr(cast(\"FD_FEEDER_CODE\" as text),1,4) )\"STATION_CODE\" ";
                //strQry += " from  \"TBLFEEDERMAST\" left join \"TBLFEEDEROFFCODE\"  on CAST(\"FD_FEEDER_ID\" AS TEXT) = CAST(\"FDO_FEEDER_ID\" AS TEXT) ";
                //if (strFeederID.Length > 0)
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("feederid", strFeederID);
                //    strQry += "  where CAST(\"FD_FEEDER_ID\" AS TEXT)=:feederid";
                //}
                //strQry += " ) A ";
                //DtFeederfDet = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_feeder_details");
                cmd.Parameters.AddWithValue("p_feederid", strFeederID);
                DtFeederfDet = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return DtFeederfDet;
            }
            return DtFeederfDet;
        }
        /// <summary>
        /// Get DTC Details
        /// </summary>
        /// <param name="strFeederID"></param>
        /// <returns></returns>
        public DataTable GetDTCDetails(string strFeederID)
        {
            DataTable DtFeederfDet = new DataTable();
            try
            {
                #region Old GetDTCDetails
                //string strQrycode = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("feederid", Convert.ToInt32(strFeederID));
                //strQrycode = ObjCon.get_value("SELECT \"FD_FEEDER_CODE\" from \"TBLFEEDERMAST\" where \"FD_FEEDER_ID\"=:feederid", NpgsqlCommand);

                //string strQryvalue = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("querycode", strQrycode);
                //strQryvalue = "SELECT count(*) as TOTALDTC from \"TBLDTCMAST\" WHERE \"DT_FDRSLNO\"=:querycode";
                //DtFeederfDet = ObjCon.FetchDataTable(strQryvalue, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmdFeederCode = new NpgsqlCommand("proc_get_totaldtc_count_on_feederid");
                cmdFeederCode.Parameters.AddWithValue("p_feederid", strFeederID);
                DtFeederfDet = ObjCon.FetchDataTable(cmdFeederCode);

                return DtFeederfDet;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return DtFeederfDet;
            }
        }
        /// <summary>
        /// Sync Feeder Details to App
        /// </summary>
        /// <param name="objFeeder"></param>
        /// <returns></returns>
        public bool SyncFeederDetailstoApp(clsFeederMast objFeeder)
        {
            string[] strQryVallist = null;
            string strQry = string.Empty;

            NpgsqlCommand = new NpgsqlCommand();
            DataTable dt = new DataTable();
            try
            {
                if (objFeeder.OfficeCode != "")
                {
                    strQryVallist = objFeeder.OfficeCode.Split(',');

                    foreach (string OfficeCode in strQryVallist)
                    {
                        #region Old SyncFeederDetailstoApp
                        //NpgsqlCommand.Parameters.AddWithValue("offcode", OfficeCode);
                        //strQry = "SELECT \"OM_CODE\" FROM \"TBLOMSECMAST\" WHERE CAST(\"OM_SUBDIV_CODE\" AS TEXT)=:offcode";
                        //dt = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
                        #endregion

                        NpgsqlCommand cmdFeederCode = new NpgsqlCommand("proc_get_om_code_on_offcode");
                        cmdFeederCode.Parameters.AddWithValue("p_offcode", OfficeCode);
                        dt = ObjCon.FetchDataTable(cmdFeederCode);

                        string sSection;
                        if (dt.Rows.Count > 0)
                        {
                            sSection = Convert.ToString(dt.Rows[0]["OM_CODE"]);
                        }
                        else
                        {
                            sSection = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
            return true;
        }
        /// <summary>
        /// Auto Generate Feeder Code
        /// </summary>
        /// <param name="objFeeder"></param>
        /// <returns></returns>
        public string AutoGenerateFeederCode(clsFeederMast objFeeder)
        {
            string strQry = string.Empty;
            string stationcode = string.Empty;
            string feedercode = string.Empty;
            int twodigitFdcode;

            DataTable dt = new DataTable();
            try
            {
                #region Old AutoGenerateFeederCode
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("stid", objFeeder.Stationid);
                //strQry = "SELECT \"ST_STATION_CODE\" FROM \"TBLSTATION\" WHERE \"ST_ID\" = :stid ";
                //stationcode = ObjCon.get_value(strQry, NpgsqlCommand);

                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("stcd", stationcode);
                //strQry = "SELECT \"FD_FEEDER_CODE\" FROM \"TBLFEEDERMAST\" WHERE SUBSTR(\"FD_FEEDER_CODE\",1,4) = :stcd ORDER BY \"FD_FEEDER_CODE\" DESC";
                //dt = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_station_code");
                cmd.Parameters.AddWithValue("p_stid", Convert.ToString(objFeeder.Stationid));
                stationcode = ObjBseCon.StringGetValue(cmd);

                NpgsqlCommand cmdFeederCode = new NpgsqlCommand("proc_get_feeder_code_on_station_code");
                cmdFeederCode.Parameters.AddWithValue("p_stcd", stationcode);
                dt = ObjCon.FetchDataTable(cmdFeederCode);

                if (dt.Rows.Count > 0)
                {
                    feedercode = dt.Rows[0]["FD_FEEDER_CODE"].ToString();
                }
                else
                {
                    feedercode = stationcode + "01";
                    return feedercode;
                }

                if (feedercode.Length > 4)
                {
                    twodigitFdcode = Convert.ToInt16(feedercode.Substring(4, 2));
                    twodigitFdcode = twodigitFdcode + 1;
                    if (twodigitFdcode >= 100)
                    {
                        return "MAX FEEDER CODE HAS REACHED FOR STATION";
                    }

                    if (twodigitFdcode.ToString().Length == 1)
                    {
                        feedercode = feedercode.Substring(0, 4) + "0" + twodigitFdcode;
                    }
                    else
                    {
                        feedercode = feedercode.Substring(0, 4) + twodigitFdcode;
                    }
                }
                return feedercode;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return null;
            }
        }
        /// <summary>
        /// Get Feeder Count
        /// </summary>
        /// <param name="sSubdivCode"></param>
        /// <returns></returns>
        public string sGetFeederCount(string sSubdivCode)
        {
            string FeederCount = string.Empty;
            try
            {
                #region Old sGetFeederCount
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("subdivcd", Convert.ToInt32(sSubdivCode));
                //string sSQry = "SELECT count(*) FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" WHERE \"FD_FEEDER_ID\"=\"FDO_FEEDER_ID\" and \"FDO_OFFICE_CODE\"=:subdivcd";
                //return ObjCon.get_value(sSQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_feeder_count");
                cmd.Parameters.AddWithValue("p_subdivcode", sSubdivCode);
                FeederCount = ObjBseCon.StringGetValue(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
            return FeederCount;
        }
        string charset = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        /// <summary>
        /// Get Digit Value
        /// </summary>
        /// <param name="digit"></param>
        /// <returns></returns>
        public int GetDigitValue(char digit)
        {
            return charset.IndexOf(digit);
        }
        /// <summary>
        /// Check All Char
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool CheckAllChar(string str)
        {
            char[] c = str.ToCharArray();
            for (int i = 0; i < str.Length; i++)
            {
                if (c[i] != 'Z')
                {
                    return false;
                }
            }
            return true;
        }

        #region UnUsed Code.
        //public string FeederMast(string txtFeederCode, string cmbstationvalue, string txtName, string txtChangDate, string txtCTLowRange, string txtCTHighRange, string txtConstant, string txtLineVoltage, string cmbFeederTypevalue, string cmbSectionvalue, string txtCurLimit, string txtKWHReading, string cmbconscatvalue, string strUser)
        //{
        //    try
        //    {

        //        if (!objCon.get_value("SELECT count(*) FROM TBLFEEDERMAST WHERE FD_FEEDER_CODE='1111' and FD_OFFICE_CODE='1112').ToString().Equals("0"))
        //        {
        //            return ("Feeder Code Already Present");
        //        }
        //        OleDbDataReader dr = objCon.Fetch("SELECT * FROM TBLMUSSMAST WHERE MU_MUSS_CODE='" + txtFeederCode.Substring(0, 3) + "'");
        //        if (!dr.Read())
        //        {
        //            return ("Muss Code Not Found " + txtFeederCode.Substring(0, 2));
        //        }
        //        MussSlno = objCon.get_value("Select MU_SLNO from TBLMUSSMAST where MU_MUSS_CODE='" + cmbstationvalue.ToString() + "'");
        //        if (cmbconscatvalue == "10")
        //        {
        //            cmbconscatvalue = "0";
        //        }
        //        string mob = objCon.get_value("SELECT MU_MOBILE_NO FROM TBLMUSSMAST WHERE MU_MUSS_CODE='" + txtFeederCode.Substring(0, 3) + "'");
        //        string qry = "INSERT INTO TBLFEEDERMAST(FD_FEEDER_ID,FD_MUSS_CODE,FD_NAME,FD_CHANGE_DT,FD_CT_LOW_RANGE,FD_CT_HIGH_RANGE,FD_CONST,FD_LINE_VOLTAGE,FD_SLNO,FD_TYPE,FD_MOBILE_NO,FD_OFFICE_CODE,FD_CUR_LIMIT,FD_KWH_READING,FD_CONS_CAT) VALUES('";
        //        qry = qry + txtFeederCode + "','";	//FDR CODE
        //        qry = qry + MussSlno + "','";				//MUSS CODE
        //        qry = qry + txtName + "',";		//FDR NAME
        //        qry = qry + "TO_DATE('" + txtChangDate + "','MM/DD/YYYY'),'";	//CT CHGD DATE3
        //        qry = qry + txtCTLowRange + "','";	//CT LOW RANGE
        //        qry = qry + txtCTHighRange + "','"; //CT HIGH RANGE
        //        qry = qry + txtConstant + "','";	//CONST
        //        qry = qry + txtLineVoltage + "','";	//LINE VOLTAGE
        //        qry = qry + slno + "','";	//FDR SLNO
        //        qry = qry + cmbFeederTypevalue + "','";	//FDR TYPE
        //        qry = qry + mob + "','";	//MOBILE NO
        //        qry = qry + cmbSectionvalue + "','";	//SECTION CODE
        //        qry = qry + txtCurLimit + "','";	//CUR LIMIT
        //        qry = qry + txtKWHReading + "','";	//KWH READNG
        //        qry = qry + cmbconscatvalue + "')";	//CONS CAT
        //        objCon.Execute(qry);
        //        // long bfslno = objCon.Get_max_no("BF_SLNO", "TBLBNKFDRLINK");
        //        //if (cmbFeederTypevalue.Equals("5") || cmbFeederTypevalue.Equals("6"))
        //        //{
        //        //   // objCon.Execute("ALTER TRIGGER  CESCDTC.BNK_FDR_VALIDATE DISABLE");
        //        //    objCon.Execute("INSERT INTO TBLBNKFDRLINK(BF_SLNO,BF_BNK_SLNO,BF_FDR_SLNO,BF_LINK_DATE,BF_CANCEL_FLAG,BF_ENTRY_AUTH) VALUES('" + bfslno + "','" + slno + "','" + slno + "',sysdate,0,'" + strUser + "')");
        //        //  //  objCon.Execute("ALTER TRIGGER  CESCDTC.BNK_FDR_VALIDATE ENABLE");
        //        //}
        //        //else if (!cmbIFPointvalue.Equals(string.Empty))
        //        //{
        //        //    objCon.Execute("INSERT INTO TBLBNKFDRLINK(BF_SLNO,BF_BNK_SLNO,BF_FDR_SLNO,BF_LINK_DATE,BF_CANCEL_FLAG,BF_ENTRY_AUTH) VALUES('" + bfslno + "','" + cmbIFPointvalue + "','" + slno + "',sysdate,0,'" + strUser + "')");
        //        //}
        //        return ("Feeder Information Saved Successfully " + txtFeederCode);
        //    }
        //    catch (Exception ex)
        //    {
        //        return "Error:" + ex.Message;
        //    }
        //}
        //public string UpdateFeederDetails(string FdSlno, string txtFeederCode, string cmbstationvalue, string txtName, string txtChangDate, string txtCTLowRange, string txtCTHighRange, string txtConstant, string txtLineVoltage, string cmbFeederTypevalue, string cmbSectionvalue, string txtCurLimit, string txtKWHReading, string cmbconscatvalue, string strUser)
        //{
        //    try
        //    {
        //        OleDbDataReader dr;
        //        dr = objCon.Fetch("Select * from TBLFEEDERMAST where FD_SLNO='" + FdSlno + "'");
        //        if (!dr.Read())
        //        {
        //            return ("Feeder Does Not Exists");
        //        }
        //        dr.Close();

        //        dr = objCon.Fetch("SELECT * FROM TBLMUSSMAST WHERE MU_MUSS_CODE='" + txtFeederCode.Substring(0, 3) + "'");
        //        if (!dr.Read())
        //        {
        //            return ("Muss Code Not Found " + txtFeederCode.Substring(0, 2));
        //        }
        //        MussSlno = objCon.get_value("Select MU_SLNO from TBLMUSSMAST where MU_MUSS_CODE='" + cmbstationvalue.ToString() + "'");
        //        if (cmbconscatvalue == "10")
        //        {
        //            cmbconscatvalue = "0";
        //        }
        //        string mob = objCon.get_value("SELECT MU_MOBILE_NO FROM TBLMUSSMAST WHERE MU_MUSS_CODE='" + txtFeederCode.Substring(0, 3) + "'");
        //        string qry = "Update TBLFEEDERMAST set FD_FEEDER_ID='" + txtFeederCode + "',";
        //        qry += "FD_MUSS_CODE='" + MussSlno + "',";
        //        qry += "FD_NAME='" + txtName.Trim().ToUpper() + "',";
        //        if (txtChangDate.Trim().Length > 0)
        //        {
        //            qry += "FD_CHANGE_DT=TO_DATE('" + txtChangDate + "','MM/DD/YYYY'),";
        //        }
        //        qry += "FD_CT_LOW_RANGE='" + txtCTLowRange + "',";
        //        qry += "FD_CT_HIGH_RANGE='" + txtCTHighRange + "',";
        //        qry += "FD_CONST='" + txtConstant + "',";
        //        qry += "FD_LINE_VOLTAGE='" + txtLineVoltage + "',";
        //        qry += "FD_TYPE='" + cmbFeederTypevalue + "',";
        //        qry += "FD_MOBILE_NO='" + mob + "',";
        //        qry += "FD_OFFICE_CODE='" + cmbSectionvalue + "',";
        //        qry += "FD_CUR_LIMIT='" + txtCurLimit + "',";
        //        qry += "FD_KWH_READING='" + txtKWHReading + "',";
        //        qry += "FD_CONS_CAT='" + cmbconscatvalue + "' where FD_SLNO='" + FdSlno + "'";
        //        objCon.Execute(qry);
        //        return ("Feeder Information Updated Successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return "Error:" + ex.Message;
        //    }
        //}
        //public DataTable LoadFeederMastDet(string strFeederID = "")
        //{
        //    PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
        //    DataTable DtFeederfDet = new DataTable();
        //    NpgsqlCommand = new NpgsqlCommand();
        //    try
        //    {

        //        strQry = string.Empty;
        //        //strQry = " SELECT FD_FEEDER_NAME,FD_FEEDER_CODE ,FD_FEEDER_ID,ST_NAME,BS_NAME,FC_NAME,LISTAGG(OFFNAME,',') WITHIN GROUP (ORDER BY OFFNAME) OFFNAME FROM";
        //        //strQry += " (SELECT FD_FEEDER_NAME,FD_FEEDER_CODE ,FD_FEEDER_ID,(SELECT ST_NAME FROM TBLSTATION WHERE FD_ST_ID=ST_ID ) ST_NAME,";
        //        //strQry += " (SELECT BS_NAME FROM TBLBUS WHERE FD_BS_ID=BS_ID) BS_NAME,(SELECT FC_NAME FROM TBLFEEDERCATEGORY WHERE  FD_FC_ID=FC_ID) FC_NAME,";
        //        //strQry += " (SELECT OFF_NAME from VIEW_ALL_OFFICES WHERE OFF_CODE=FDO_OFFICE_CODE ) AS OFFNAME";
        //        //strQry += " from TBLFEEDERMAST ,TBLFEEDEROFFCODE WHERE FD_FEEDER_ID=FDO_FEEDER_ID ";
        //        //if (strFeederID != "")
        //        //{
        //        //    strQry += " AND FD_FEEDER_ID='" + strFeederID + "'";
        //        //}
        //        //strQry += " ) GROUP BY FD_FEEDER_NAME,FD_FEEDER_CODE ,FD_FEEDER_ID,ST_NAME,BS_NAME,FC_NAME";

        //        strQry = " SELECT \"FD_FEEDER_NAME\",\"FD_FEEDER_CODE \",\"FD_FEEDER_ID\",\"ST_NAME\",\"BS_NAME\",\"FC_NAME\",LISTAGG(\"OFFNAME\",',') WITHIN GROUP (ORDER BY \"OFFNAME\") OFFNAME FROM";
        //        strQry += " (SELECT \"FD_FEEDER_NAME\",\"FD_FEEDER_CODE \",\"FD_FEEDER_ID\",(SELECT \"ST_NAME\" FROM \"TBLSTATION\" WHERE CAST(\"FD_ST_ID\" AS TEXT)=CAST(\"ST_ID\" AS TEXT) ) ST_NAME,";
        //        strQry += " (SELECT \"BS_NAME\" FROM \"TBLBUS\" WHERE \"FD_BS_ID\"=\"BS_ID\") BS_NAME,(SELECT \"FC_NAME\" FROM \"TBLFEEDERCATEGORY\" WHERE  CAST(\"FD_FC_ID\" AS TEXT)=CAST(\"FC_ID\" AS TEXT))  FC_NAME,";
        //        strQry += " (SELECT \"OFF_NAME\" from \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT)=CAST(\"FDO_OFFICE_CODE\" AS TEXT) ) AS OFFNAME";
        //        strQry += " from \"TBLFEEDERMAST \",\"TBLFEEDEROFFCODE\" WHERE CAST(\"FD_FEEDER_ID\" AS TEXT)= CAST(\"FDO_FEEDER_ID\" AS TEXT) ";
        //        if (strFeederID != "")
        //        {
        //            NpgsqlCommand.Parameters.AddWithValue("feederid12", strFeederID);
        //            strQry += " AND CAST(\"FD_FEEDER_ID\" AS TEXT)=:feederid12";
        //        }
        //        strQry += " ) GROUP BY \"FD_FEEDER_NAME\",\"FD_FEEDER_CODE \",\"FD_FEEDER_ID\",\"ST_NAME\",\"BS_NAME\",\"FC_NAME\"";
        //        DtFeederfDet = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
        //        return DtFeederfDet;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return DtFeederfDet;
        //    }
        //}
        //public DataTable LoadViewAllOffices(string strOfficeCode = "", string strOfficeName = "")
        //{
        //    PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
        //    DataTable DtOfficeDet = new DataTable();
        //    NpgsqlCommand = new NpgsqlCommand();
        //    try
        //    {
        //        strQry = string.Empty;
        //        strQry = "select \"OFF_CODE\" ,\"OFF_NAME\"  FROM \"VIEW_ALL_OFFICES\"   ";
        //        //if (strOfficeCode != "" && strOfficeName != "")
        //        //{
        //        NpgsqlCommand.Parameters.AddWithValue("offcode", strOfficeCode);
        //        NpgsqlCommand.Parameters.AddWithValue("offname", strOfficeName.ToUpper());
        //        strQry += " where (CAST(\"OFF_CODE\" AS TEXT) like  :offcode||'%' and UPPER(CAST(\"OFF_NAME\" AS TEXT)) like  :offname||'%') ";
        //        //}

        //        //else if(strOfficeCode != ""  && strOfficeName == "") 
        //        //{
        //        //    strQry += " where (OFF_CODE like  '%" + strOfficeCode + "%') ";
        //        //}

        //        //else if (strOfficeCode == "" && strOfficeName != "")
        //        //{
        //        //    strQry += " where (UPPER(OFF_NAME) like  '%" + strOfficeName.ToUpper() + "%') ";
        //        //}
        //        strQry += " order by \"OFF_NAME\"";

        //        DtOfficeDet = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
        //        return DtOfficeDet;

        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return DtOfficeDet;
        //    }
        //}
        //public void SyncFeederDetails(string sOfficeCode, string sFeederCode, string sSection)
        //{
        //    //CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        //    PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
        //    NpgsqlCommand = new NpgsqlCommand();
        //    try
        //    {
        //        DataTable ds = new DataTable();

        //        strQry = "DELETE FROM TBLFEEDERDETAILS WHERE FD_FEEDER_CODE='" + sFeederCode + "'";
        //        ExecuteSqliteQuery(strQry, sSection);

        //        NpgsqlCommand.Parameters.AddWithValue("fdcode", sFeederCode);
        //        NpgsqlCommand.Parameters.AddWithValue("offcode", sOfficeCode);
        //        strQry = "SELECT \"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\",\"FDO_OFFICE_CODE\" FROM \"TBLFEEDERMAST\", \"TBLFEEDEROFFCODE\" WHERE ";
        //        strQry += " \"FD_FEEDER_ID\" = \"FDO_FEEDER_ID\" AND \"FD_FEEDER_CODE\"=:fdcode AND (cast(\"FDO_OFFICE_CODE\" as TEXT) LIKE :offcode||'%' OR  LENGTH( CAST (\"FDO_OFFICE_CODE\" AS TEXT)) = 0)";

        //        ds = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
        //        // arrFinal.Add(OperateDBName + " -  " + OperateDivisionCode + "-FEEDER-" + DS.Tables[0].Rows.Count.ToString());
        //        if (ds.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < ds.Rows.Count; i++)
        //            {
        //                strQry = "INSERT INTO TBLFEEDERDETAILS (FD_FEEDER_CODE, FD_FEEDER_NAME,FD_OFFICE_CODE) VALUES ";
        //                strQry += " ('" + Convert.ToString(ds.Rows[i][0]).Replace("'", "") + "','" + Convert.ToString(ds.Rows[i][1]).Replace("'", "") + "',";
        //                strQry += " '" + Convert.ToString(ds.Rows[i][2]).Replace("'", "") + "')";
        //                ExecuteSqliteQuery(strQry, sSection);
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}
        //public void SetSqlLiteConnection(string sDbName)
        //{
        //    String Exists = "FALSE";
        //    try
        //    {

        //        sDbName = sDbName.Substring(0, 4);
        //        //SQLiteConnection sql_con;
        //        string sURL = Convert.ToString(ConfigurationSettings.AppSettings["SQLLiteDB"]);
        //        string relative_path = sURL + sDbName + ".db";


        //        if (System.IO.File.Exists(relative_path))
        //        {
        //            Exists = "TRUE";
        //        }


        //        sql_con = new SQLiteConnection
        //        ("Data Source=" + relative_path + ";Version=3;");
        //        sql_con.Open();
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}
        //public void ExecuteSqliteQuery(string Query, string sDbName)
        //{
        //    String Exists = "INVOKE";
        //    try
        //    {
        //        SQLiteCommand sql_cmd;

        //        SetSqlLiteConnection(sDbName);
        //        sql_cmd = sql_con.CreateCommand();
        //        sql_cmd.CommandText = Query;
        //        sql_cmd.ExecuteNonQuery();
        //        sql_con.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}
        //int ConvertFromBase26(string number)
        //{
        //    int result = 0;
        //    foreach (char digit in number)
        //        result = result * charset.Length + GetDigitValue(digit);

        //    return result;
        //}
        #endregion
    }
}
