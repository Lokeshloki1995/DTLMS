using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;
using System.Collections;
using IIITS.DTLMS.BL.DataBase;

namespace IIITS.DTLMS.BL
{
    public class clsDtcMaster
    {
        string strFormCode = "clsDtcMaster";
        PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
        DataBseConnection ObjBseCon = new DataBseConnection(Constants.Password);
        NpgsqlCommand NpgsqlCommand;

        public long lGetMaxMap { get; set; }
        public string lDtcId { get; set; }
        public string sDtcCode { get; set; }
        public string sDtcName { get; set; }
        public string sOMSectionName { get; set; }
        public string iConnectedKW { get; set; }
        public string iConnectedHP { get; set; }
        public string sInternalCode { get; set; }
        public string sPlatformType { get; set; }
        public string sConnectionDate { get; set; }
        public string sInspectionDate { get; set; }
        public string sServiceDate { get; set; }
        public string sCommisionDate { get; set; }
        public string sFeederChangeDate { get; set; }
        public string iKWHReading { get; set; }
        public string sTCMakeName { get; set; }
        public string sTCCapacity { get; set; }
        public string sTcCode { get; set; }
        public string sTcSlno { get; set; }
        public string sOldTcCode { get; set; }
        public string sCrBy { get; set; }
        public string sHtlinelength { get; set; }
        public string sLtlinelength { get; set; }
        public string sArresters { get; set; }
        public string sGrounding { get; set; }
        public string sHTProtect { get; set; }
        public string sLTProtect { get; set; }
        public string sDTCMeters { get; set; }
        public string sBreakertype { get; set; }
        public string sProjectType { get; set; }
        public string sOfficeCode { get; set; }
        public string sFeederCode { get; set; }
        public string sStation { get; set; }
        public string sDate { get; set; }
        public string sFeederName { get; set; }
        internal string sManufactureDate { get; set; }
        /// <summary>
        /// SaveUpdateDtcDetails
        /// </summary>
        /// <param name="objDtcMaster"></param>
        /// <returns></returns>
        public string[] SaveUpdateDtcDetails(clsDtcMaster objDtcMaster)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string[] Arr = new string[2];
            try
            {
                string sQryVal = string.Empty;
                string strQry = string.Empty;
                string QryKey = string.Empty;

                #region Old inline query
                //NpgsqlCommand.Parameters.AddWithValue("tcslno", objDtcMaster.sTcSlno);
                //sQryVal = ObjCon.get_value("select * from \"TBLTCMASTER\" where \"TC_SLNO\"=:tcslno ", NpgsqlCommand);
                #endregion

                QryKey = "GET_TC_ID";
                NpgsqlCommand cmd_TC_ID = new NpgsqlCommand("fetch_getvalue_clsdtcmaster");
                cmd_TC_ID.Parameters.AddWithValue("p_key", QryKey);
                cmd_TC_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(objDtcMaster.sTcSlno ?? ""));
                cmd_TC_ID.Parameters.AddWithValue("p_value_2", "");
                sQryVal = ObjBseCon.StringGetValue(cmd_TC_ID);

                if (sQryVal == "" || sQryVal == null)
                {
                    Arr[0] = "Enter Valid TC SlNo ";
                    Arr[1] = "4";
                    return Arr;
                }

                if (objDtcMaster.lDtcId == "")
                {
                    #region Old inline query
                    //NpgsqlCommand.Parameters.AddWithValue("dtcode", objDtcMaster.sDtcCode);
                    //sQryVal = ObjCon.get_value("select \"DT_CODE\" from \"TBLDTCMAST\" where \"DT_CODE\"=:dtcode", NpgsqlCommand);
                    #endregion

                    QryKey = "GET_DT_CODE";
                    NpgsqlCommand cmd_DT_COD = new NpgsqlCommand("fetch_getvalue_clsdtcmaster");
                    cmd_DT_COD.Parameters.AddWithValue("p_key", QryKey);
                    cmd_DT_COD.Parameters.AddWithValue("p_value_1", Convert.ToString(objDtcMaster.sDtcCode ?? ""));
                    cmd_DT_COD.Parameters.AddWithValue("p_value_2", "");
                    sQryVal = ObjBseCon.StringGetValue(cmd_DT_COD);

                    if (sQryVal != "")
                    {
                        Arr[0] = "DTC Code Already Exists";
                        Arr[1] = "4";
                        return Arr;
                    }

                    #region Old inline query
                    //NpgsqlCommand.Parameters.AddWithValue("feedercode", objDtcMaster.sDtcCode.ToString().Substring(0, Constants.Feeder));
                    //sQryVal = ObjCon.get_value("select * from \"TBLFEEDERMAST\" where \"FD_FEEDER_CODE\"=:feedercode", NpgsqlCommand);
                    #endregion

                    QryKey = "GET_FD_FEEDER_ID";
                    NpgsqlCommand cmd_FD_FEEDER_ID = new NpgsqlCommand("fetch_getvalue_clsdtcmaster");
                    cmd_FD_FEEDER_ID.Parameters.AddWithValue("p_key", QryKey);
                    cmd_FD_FEEDER_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(objDtcMaster.sDtcCode ?? "").Substring(0, Constants.Feeder));
                    cmd_FD_FEEDER_ID.Parameters.AddWithValue("p_value_2", "");
                    sQryVal = ObjBseCon.StringGetValue(cmd_FD_FEEDER_ID);

                    if (sQryVal == "" || sQryVal == null)
                    {
                        Arr[0] = "Code Does Not Match With The Feeder Code";
                        Arr[1] = "4";
                        return Arr;
                    }

                    #region Old inline query
                    //NpgsqlCommand.Parameters.AddWithValue("omcode", objDtcMaster.sOMSectionName);
                    //sQryVal = ObjCon.get_value("select * from \"TBLOMSECMAST\" where \"OM_CODE\"=:omcode", NpgsqlCommand);
                    #endregion

                    QryKey = "GET_OM_SLNO";
                    NpgsqlCommand cmd_OM_SLNO = new NpgsqlCommand("fetch_getvalue_clsdtcmaster");
                    cmd_OM_SLNO.Parameters.AddWithValue("p_key", QryKey);
                    cmd_OM_SLNO.Parameters.AddWithValue("p_value_1", Convert.ToString(objDtcMaster.sOMSectionName ?? ""));
                    cmd_OM_SLNO.Parameters.AddWithValue("p_value_2", "");
                    sQryVal = ObjBseCon.StringGetValue(cmd_OM_SLNO);

                    if (sQryVal == "" || sQryVal == null)
                    {
                        Arr[0] = "Enter Valid O&m Sec ";
                        Arr[1] = "4";
                        return Arr;
                    }

                    #region Old inline query
                    //NpgsqlCommand.Parameters.AddWithValue("tccode", Convert.ToDouble(objDtcMaster.sTcCode));
                    //sQryVal = ObjCon.get_value("select * from \"TBLTCMASTER\" where \"TC_CODE\"=:tccode", NpgsqlCommand);
                    #endregion

                    QryKey = "GET_TC_ID_ON_TC_CODE";
                    NpgsqlCommand cmd_ON_TC_CODE = new NpgsqlCommand("fetch_getvalue_clsdtcmaster");
                    cmd_ON_TC_CODE.Parameters.AddWithValue("p_key", QryKey);
                    cmd_ON_TC_CODE.Parameters.AddWithValue("p_value_1", Convert.ToString(objDtcMaster.sTcCode ?? ""));
                    cmd_ON_TC_CODE.Parameters.AddWithValue("p_value_2", "");
                    sQryVal = ObjBseCon.StringGetValue(cmd_ON_TC_CODE);

                    if (sQryVal == "" || sQryVal == null)
                    {
                        Arr[0] = "Enter Valid TC SlNo ";
                        Arr[1] = "4";
                        return Arr;
                    }

                    objDtcMaster.lDtcId = Convert.ToString(ObjCon.Get_max_no("DT_ID", "TBLDTCMAST"));

                    string strFeederSlno = string.Empty;
                    #region Old inline query
                    //NpgsqlCommand.Parameters.AddWithValue("feedercode1", objDtcMaster.sDtcCode.ToString().Substring(0, Constants.Feeder));
                    //strFeederSlno = ObjCon.get_value("SELECT \"FD_FEEDER_ID\" FROM \"TBLFEEDERMAST\" WHERE \"FD_FEEDER_CODE\"=:feedercode1", NpgsqlCommand);
                    #endregion

                    QryKey = "GET_FD_FEEDER_ID";
                    NpgsqlCommand cmd_FEEDER_ID = new NpgsqlCommand("fetch_getvalue_clsdtcmaster");
                    cmd_FEEDER_ID.Parameters.AddWithValue("p_key", QryKey);
                    cmd_FEEDER_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(objDtcMaster.sDtcCode ?? "").Substring(0, Constants.Feeder));
                    cmd_FEEDER_ID.Parameters.AddWithValue("p_value_2", "");
                    strFeederSlno = ObjBseCon.StringGetValue(cmd_FEEDER_ID);


                    objDtcMaster.lGetMaxMap = ObjCon.Get_max_no("TM_ID", "TBLTRANSDTCMAPPING");
                    string strDTCupdated = "DTC MASTER ENTRY";
                    string strCurrLoc = "2";

                    NpgsqlCommand cmd = new NpgsqlCommand("sp_save_DTCdetails");
                    cmd.Parameters.AddWithValue("dt_id", objDtcMaster.lDtcId);
                    cmd.Parameters.AddWithValue("dt_code", objDtcMaster.sDtcCode);
                    cmd.Parameters.AddWithValue("dt_name", objDtcMaster.sDtcName);
                    cmd.Parameters.AddWithValue("dt_om_slno", objDtcMaster.sOMSectionName);
                    cmd.Parameters.AddWithValue("dt_total_con_kw", objDtcMaster.iConnectedKW);
                    cmd.Parameters.AddWithValue("dt_total_con_hp", objDtcMaster.iConnectedHP);
                    cmd.Parameters.AddWithValue("dt_kwh_reading", objDtcMaster.iKWHReading);
                    cmd.Parameters.AddWithValue("dt_platform", objDtcMaster.sPlatformType);
                    cmd.Parameters.AddWithValue("dt_internal_code", objDtcMaster.sInternalCode);
                    cmd.Parameters.AddWithValue("dt_tc_id", objDtcMaster.sTcCode);
                    cmd.Parameters.AddWithValue("dt_con_date", objDtcMaster.sConnectionDate);
                    cmd.Parameters.AddWithValue("dt_last_insp_date", objDtcMaster.sInspectionDate);
                    cmd.Parameters.AddWithValue("dt_last_service_date", objDtcMaster.sServiceDate);
                    cmd.Parameters.AddWithValue("dt_trans_commision_date", objDtcMaster.sCommisionDate);
                    cmd.Parameters.AddWithValue("dt_fdrchange_date", objDtcMaster.sFeederChangeDate);
                    cmd.Parameters.AddWithValue("dt_fdrslno", objDtcMaster.sDtcCode.ToString().Substring(0, Constants.Feeder));
                    cmd.Parameters.AddWithValue("dt_crby", objDtcMaster.sCrBy);
                    cmd.Parameters.AddWithValue("dt_cron", DateTime.Now.ToString("dd/MM/yyyy"));
                    cmd.Parameters.AddWithValue("dt_breaker_type", objDtcMaster.sBreakertype);
                    cmd.Parameters.AddWithValue("dt_dtcmeters", objDtcMaster.sDTCMeters);
                    cmd.Parameters.AddWithValue("dt_ht_protect", objDtcMaster.sHTProtect);
                    cmd.Parameters.AddWithValue("dt_lt_protect", objDtcMaster.sHTProtect);
                    cmd.Parameters.AddWithValue("dt_grounding", objDtcMaster.sGrounding);
                    cmd.Parameters.AddWithValue("dt_arresters", objDtcMaster.sArresters);
                    cmd.Parameters.AddWithValue("dt_lt_line", objDtcMaster.sLtlinelength);
                    cmd.Parameters.AddWithValue("dt_ht_line", objDtcMaster.sHtlinelength);
                    cmd.Parameters.AddWithValue("tm_id", objDtcMaster.lGetMaxMap);
                    cmd.Parameters.AddWithValue("tm_tc_id", objDtcMaster.sTcCode.ToUpper());
                    cmd.Parameters.AddWithValue("tm_dtc_id", objDtcMaster.sDtcCode);
                    cmd.Parameters.AddWithValue("tm_mapping_date", objDtcMaster.sConnectionDate);
                    cmd.Parameters.AddWithValue("tm_crby", objDtcMaster.sCrBy);
                    cmd.Parameters.AddWithValue("tc_updated_event", strDTCupdated);
                    cmd.Parameters.AddWithValue("tc_updated_event_id", lGetMaxMap);
                    cmd.Parameters.AddWithValue("tc_current_location", strCurrLoc);
                    cmd.Parameters.AddWithValue("tc_location_id", objDtcMaster.sOMSectionName);
                    cmd.Parameters.AddWithValue("tc_code", objDtcMaster.sTcCode.ToUpper());
                    cmd.Parameters["pk_id"].Direction = ParameterDirection.Output;
                    cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                    cmd.Parameters["msg"].Direction = ParameterDirection.Output;

                    Arr[2] = "pk_id";
                    Arr[1] = "op_id";
                    Arr[0] = "msg";
                    Arr = ObjCon.Execute(cmd, Arr, 3);
                    return Arr;
                }
                else
                {
                    string strCurrLoc = "2";

                    #region Old inline query
                    //NpgsqlCommand.Parameters.AddWithValue("dtcd", objDtcMaster.sDtcCode);
                    //NpgsqlCommand.Parameters.AddWithValue("dtid", Convert.ToInt32(objDtcMaster.lDtcId));
                    //sQryVal = ObjCon.get_value("select * from \"TBLDTCMAST\" where \"DT_CODE\"=:dtcd AND \"DT_ID\"<>:dtid", NpgsqlCommand);
                    #endregion

                    QryKey = "GET_DT_CODE_ON_DT_CODE_AND_DT_ID";
                    NpgsqlCommand cmd_FEEDER_ID = new NpgsqlCommand("fetch_getvalue_clsdtcmaster");
                    cmd_FEEDER_ID.Parameters.AddWithValue("p_key", QryKey);
                    cmd_FEEDER_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(objDtcMaster.sDtcCode ?? ""));
                    cmd_FEEDER_ID.Parameters.AddWithValue("p_value_2", Convert.ToString(objDtcMaster.lDtcId ?? ""));
                    sQryVal = ObjBseCon.StringGetValue(cmd_FEEDER_ID);

                    if (sQryVal != "")
                    {
                        Arr[0] = "DTC With This Id  Already Exists";
                        Arr[1] = "4";
                        return Arr;
                    }

                    #region Old inline query
                    //NpgsqlCommand.Parameters.AddWithValue("fdcd", objDtcMaster.sDtcCode.ToString().Substring(0, Constants.Feeder));
                    //sQryVal = ObjCon.get_value("select * from \"TBLFEEDERMAST\" where \"FD_FEEDER_CODE\"=:fdcd ", NpgsqlCommand);
                    #endregion

                    QryKey = "GET_FD_FEEDER_ID";
                    NpgsqlCommand cmd_TBLFEEDERMAST = new NpgsqlCommand("fetch_getvalue_clsdtcmaster");
                    cmd_TBLFEEDERMAST.Parameters.AddWithValue("p_key", QryKey);
                    cmd_TBLFEEDERMAST.Parameters.AddWithValue("p_value_1", Convert.ToString(objDtcMaster.sDtcCode ?? "").Substring(0, Constants.Feeder));
                    cmd_TBLFEEDERMAST.Parameters.AddWithValue("p_value_2", "");
                    sQryVal = ObjBseCon.StringGetValue(cmd_TBLFEEDERMAST);

                    if (sQryVal == "" || sQryVal == null)
                    {
                        Arr[0] = "Code Does Not Match With The Feeder Code";
                        Arr[1] = "4";
                        return Arr;
                    }

                    #region Old inline query
                    //NpgsqlCommand.Parameters.AddWithValue("omcd", objDtcMaster.sOMSectionName);
                    //sQryVal = ObjCon.get_value("select * from \"TBLOMSECMAST\" where \"OM_CODE\"=:omcd ", NpgsqlCommand);
                    #endregion

                    QryKey = "GET_OM_SLNO";
                    NpgsqlCommand cmd_OM_SLNO = new NpgsqlCommand("fetch_getvalue_clsdtcmaster");
                    cmd_OM_SLNO.Parameters.AddWithValue("p_key", QryKey);
                    cmd_OM_SLNO.Parameters.AddWithValue("p_value_1", Convert.ToString(objDtcMaster.sOMSectionName ?? ""));
                    cmd_OM_SLNO.Parameters.AddWithValue("p_value_2", "");
                    sQryVal = ObjBseCon.StringGetValue(cmd_OM_SLNO);

                    if (sQryVal == "" || sQryVal == null)
                    {
                        Arr[0] = "Enter Valid O&m Sec ";
                        Arr[1] = "4";
                        return Arr;
                    }

                    #region Old inline query
                    //NpgsqlCommand.Parameters.AddWithValue("tcdc", Convert.ToDouble(objDtcMaster.sTcCode));
                    //sQryVal = ObjCon.get_value("select * from \"TBLTCMASTER\" where \"TC_CODE\"=:tcdc ", NpgsqlCommand);
                    #endregion

                    QryKey = "GET_TC_ID_ON_TC_CODE";
                    NpgsqlCommand cmd_ON_TC_CODE = new NpgsqlCommand("fetch_getvalue_clsdtcmaster");
                    cmd_ON_TC_CODE.Parameters.AddWithValue("p_key", QryKey);
                    cmd_ON_TC_CODE.Parameters.AddWithValue("p_value_1", Convert.ToString(objDtcMaster.sTcCode ?? ""));
                    cmd_ON_TC_CODE.Parameters.AddWithValue("p_value_2", "");
                    sQryVal = ObjBseCon.StringGetValue(cmd_ON_TC_CODE);

                    if (sQryVal == "" || sQryVal == null)
                    {
                        Arr[0] = "Enter Valid TC SlNo ";
                        Arr[1] = "4";
                        return Arr;
                    }

                    string sQryValMap = string.Empty;

                    #region Old inline query
                    //NpgsqlCommand.Parameters.AddWithValue("dtcid", objDtcMaster.sDtcCode);
                    //sQryValMap = ObjCon.get_value("SELECT COUNT(*) FROM \"TBLTRANSDTCMAPPING\"  WHERE \"TM_DTC_ID\"=:dtcid", NpgsqlCommand);
                    #endregion

                    QryKey = "GET_COUNT_ON_TM_DTC_ID";
                    NpgsqlCommand cmd_TM_DTC_ID = new NpgsqlCommand("fetch_getvalue_clsdtcmaster");
                    cmd_TM_DTC_ID.Parameters.AddWithValue("p_key", QryKey);
                    cmd_TM_DTC_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(objDtcMaster.sDtcCode ?? ""));
                    cmd_TM_DTC_ID.Parameters.AddWithValue("p_value_2", "");
                    sQryVal = ObjBseCon.StringGetValue(cmd_ON_TC_CODE);


                    if (sQryValMap != "")
                    {
                        string strFeederSlno = string.Empty;

                        #region Old inline query
                        //NpgsqlCommand.Parameters.AddWithValue("feedCode1", objDtcMaster.sDtcCode.ToString().Substring(0, Constants.Feeder));
                        //strFeederSlno = ObjCon.get_value("SELECT \"FD_FEEDER_ID\" FROM \"TBLFEEDERMAST\" WHERE \"FD_FEEDER_CODE\"=:feedCode1", NpgsqlCommand);
                        #endregion

                        QryKey = "GET_FD_FEEDER_ID";
                        NpgsqlCommand cmd_FEEDERMAST = new NpgsqlCommand("fetch_getvalue_clsdtcmaster");
                        cmd_FEEDERMAST.Parameters.AddWithValue("p_key", QryKey);
                        cmd_FEEDERMAST.Parameters.AddWithValue("p_value_1", Convert.ToString(objDtcMaster.sDtcCode ?? "").Substring(0, Constants.Feeder));
                        cmd_FEEDERMAST.Parameters.AddWithValue("p_value_2", "");
                        strFeederSlno = ObjBseCon.StringGetValue(cmd_FEEDERMAST);


                        string strCount = string.Empty;

                        #region Old inline query
                        //NpgsqlCommand.Parameters.AddWithValue("TcCode1", Convert.ToDouble(objDtcMaster.sTcCode));
                        //strCount = ObjCon.get_value("select count(*) from \"TBLTRANSDTCMAPPING\" where \"TM_TC_ID\"=:TcCode1 and \"TM_LIVE_FLAG\"=1", NpgsqlCommand);
                        #endregion

                        QryKey = "GET_COUNT_ON_TM_TC_ID";
                        NpgsqlCommand cmd_TM_TC_ID = new NpgsqlCommand("fetch_getvalue_clsdtcmaster");
                        cmd_TM_TC_ID.Parameters.AddWithValue("p_key", QryKey);
                        cmd_TM_TC_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(objDtcMaster.sTcCode ?? ""));
                        cmd_TM_TC_ID.Parameters.AddWithValue("p_value_2", "");
                        strCount = ObjBseCon.StringGetValue(cmd_TM_TC_ID);

                        if (Convert.ToInt32(strCount) <= 1)
                        {
                            NpgsqlCommand cmdupdate = new NpgsqlCommand();
                            cmdupdate.Parameters.AddWithValue("dt_code", objDtcMaster.sDtcCode);
                            cmdupdate.Parameters.AddWithValue("dt_name", objDtcMaster.sDtcName);
                            cmdupdate.Parameters.AddWithValue("dt_om_slno", objDtcMaster.sOMSectionName);
                            cmdupdate.Parameters.AddWithValue("dt_tc_id", objDtcMaster.sTcCode);
                            cmdupdate.Parameters.AddWithValue("dt_internal_code", objDtcMaster.sInternalCode);
                            cmdupdate.Parameters.AddWithValue("dt_kwh_reading", objDtcMaster.iKWHReading);
                            cmdupdate.Parameters.AddWithValue("dt_platform", objDtcMaster.sPlatformType);
                            cmdupdate.Parameters.AddWithValue("dt_total_con_hp", objDtcMaster.iConnectedHP);
                            cmdupdate.Parameters.AddWithValue("dt_total_con_kw", objDtcMaster.iConnectedKW);
                            cmdupdate.Parameters.AddWithValue("dt_last_insp_date", objDtcMaster.sInspectionDate);
                            cmdupdate.Parameters.AddWithValue("dt_last_service_date", objDtcMaster.sServiceDate);
                            cmdupdate.Parameters.AddWithValue("dt_trans_commision_date", objDtcMaster.sCommisionDate);
                            cmdupdate.Parameters.AddWithValue("dt_fdrchange_date", objDtcMaster.sFeederChangeDate);
                            cmdupdate.Parameters.AddWithValue("dt_fdrslno", objDtcMaster.sDtcCode.ToString().Substring(0, 4));
                            cmdupdate.Parameters.AddWithValue("dt_breaker_type", objDtcMaster.sBreakertype);
                            cmdupdate.Parameters.AddWithValue("dt_dtcmeters", objDtcMaster.sDTCMeters);
                            cmdupdate.Parameters.AddWithValue("dt_ht_protect", objDtcMaster.sHTProtect);
                            cmdupdate.Parameters.AddWithValue("dt_lt_protect", objDtcMaster.sLTProtect);
                            cmdupdate.Parameters.AddWithValue("dt_grounding", objDtcMaster.sGrounding);
                            cmdupdate.Parameters.AddWithValue("dt_arresters", objDtcMaster.sArresters);
                            cmdupdate.Parameters.AddWithValue("dt_lt_line", objDtcMaster.sLtlinelength);
                            cmdupdate.Parameters.AddWithValue("dt_ht_line", objDtcMaster.sHtlinelength);
                            cmdupdate.Parameters.AddWithValue("dt_con_date", objDtcMaster.sConnectionDate);
                            cmdupdate.Parameters.AddWithValue("dt_id", objDtcMaster.lDtcId);
                            cmdupdate.Parameters.AddWithValue("tm_tc_id", objDtcMaster.sTcCode.ToUpper());
                            cmdupdate.Parameters.AddWithValue("tm_crby", objDtcMaster.sCrBy);
                            cmdupdate.Parameters.AddWithValue("tm_mapping_date", objDtcMaster.sConnectionDate);
                            cmdupdate.Parameters.AddWithValue("tm_dtc_id", objDtcMaster.sDtcCode);
                            cmdupdate.Parameters.AddWithValue("tc_current_location", strCurrLoc);
                            cmdupdate.Parameters.AddWithValue("tc_location_id", objDtcMaster.sOMSectionName);
                            cmdupdate.Parameters.AddWithValue("tc_code", objDtcMaster.sTcCode);
                            cmdupdate.Parameters["pk_id"].Direction = ParameterDirection.Output;
                            cmdupdate.Parameters["op_id"].Direction = ParameterDirection.Output;
                            cmdupdate.Parameters["msg"].Direction = ParameterDirection.Output;

                            Arr[2] = "pk_id";
                            Arr[1] = "op_id";
                            Arr[0] = "msg";
                            Arr = ObjCon.Execute(cmdupdate, Arr, 3);

                            if (objDtcMaster.sTcCode != objDtcMaster.sOldTcCode && objDtcMaster.sOldTcCode != "")
                            {
                                #region Old inline query
                                //NpgsqlCommand.Parameters.AddWithValue("OldTcCode9", Convert.ToDouble(objDtcMaster.sOldTcCode));
                                //ObjCon.ExecuteQry("update \"TBLTCMASTER\" set \"TC_CURRENT_LOCATION\"=1 where \"TC_CODE\"=:OldTcCode9", NpgsqlCommand);
                                #endregion

                                string[] Arr_TCMaster = new string[2];
                                NpgsqlCommand cmd_Update = new NpgsqlCommand("proc_update_tbltcmaster_for_clsdtcmaster");
                                cmd_Update.Parameters.AddWithValue("p_oldtccode", Convert.ToString(objDtcMaster.sOldTcCode ?? ""));
                                cmd_Update.Parameters.Add("msg", NpgsqlDbType.Text);
                                cmd_Update.Parameters.Add("op_id", NpgsqlDbType.Text);
                                cmd_Update.Parameters["msg"].Direction = ParameterDirection.Output;
                                cmd_Update.Parameters["op_id"].Direction = ParameterDirection.Output;
                                Arr_TCMaster = ObjCon.Execute(cmd_Update, Arr, 2);
                            }
                            return Arr;
                        }
                        else
                        {
                            Arr[0] = "DTC Cannot be updated as it is not in work, due to failure";
                            Arr[1] = "4";
                            return Arr;
                        }
                    }

                }


                return Arr;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }
        /// <summary>
        /// LoadDtcGrid Used by DTCView.aspx
        /// </summary>
        /// <param name="objDTC"></param>
        /// <returns></returns>
        public DataTable LoadDtcGrid(clsDtcMaster objDTC)
        {
            NpgsqlCommand = new NpgsqlCommand();
            DataTable dtDtcDetails = new DataTable();
            try
            {
                string strQry = string.Empty;
                if (objDTC.sOfficeCode == "" || objDTC.sOfficeCode == null)
                {
                    objDTC.sOfficeCode = "";

                }

                #region old logic
                //strQry = "SELECT \"DT_ID\",cast(\"DT_CODE\" as TEXT) DT_CODE,\"DT_NAME\",CAST(\"DT_TOTAL_CON_KW\" AS TEXT) DT_TOTAL_CON_KW,";
                //strQry += " CAST(\"DT_TOTAL_CON_HP\" AS TEXT) DT_TOTAL_CON_HP,CAST(\"TC_CODE\" AS TEXT) TC_CODE, CAST(\"TC_CAPACITY\" AS TEXT) TC_CAPACITY, ";
                //strQry += " TO_CHAR(\"DT_LAST_SERVICE_DATE\",'DD-MON-YYYY') DT_LAST_SERVICE_DATE,\"DT_PROJECTTYPE\", ";
                //strQry += " \"FD_FEEDER_NAME\" AS \"FEEDER_NAME\" FROM \"TBLDTCMAST\", \"TBLTCMASTER\",\"TBLFEEDERMAST\",\"TBLSTATION\" WHERE \"TC_CODE\" = \"DT_TC_ID\" ";
                //strQry += " AND CAST(\"DT_FDRSLNO\" AS TEXT)=\"FD_FEEDER_CODE\" AND \"ST_ID\"=\"FD_ST_ID\" ";
                //strQry += "  AND cast(\"DT_OM_SLNO\" as TEXT) LIKE :offcode||'%' and  \"DT_TC_ID\" <>0";

                //strQry = "SELECT \"DT_ID\",cast(\"DT_CODE\" as TEXT) DT_CODE,\"DT_NAME\",CAST(\"DT_TOTAL_CON_KW\" AS TEXT) DT_TOTAL_CON_KW,";
                //strQry += " CAST(\"DT_TOTAL_CON_HP\" AS TEXT) DT_TOTAL_CON_HP,CAST(\"TC_CODE\" AS TEXT) TC_CODE, CAST(\"TC_CAPACITY\" AS TEXT) TC_CAPACITY, ";
                //strQry += " TO_CHAR(\"DT_LAST_SERVICE_DATE\",'DD-MON-YYYY') DT_LAST_SERVICE_DATE,\"DT_PROJECTTYPE\", ";
                //strQry += " \"FD_FEEDER_NAME\" AS \"FEEDER_NAME\" FROM \"TBLDTCMAST\", \"TBLTCMASTER\",\"TBLFEEDERMAST\",\"TBLSTATION\" WHERE \"TC_CODE\" = \"DT_TC_ID\" ";
                //strQry += " AND CAST(\"DT_FDRSLNO\" AS TEXT)=\"FD_FEEDER_CODE\" AND \"ST_ID\"=\"FD_ST_ID\" ";
                //strQry += "  AND cast(\"DT_OM_SLNO\" as TEXT) LIKE :offcode||'%'  and \"TC_CAPACITY\"<>0";


                //strQry = "SELECT \"DT_ID\",cast(\"DT_CODE\" as TEXT) DT_CODE,\"DT_NAME\",CAST(\"DT_TOTAL_CON_KW\" AS TEXT) DT_TOTAL_CON_KW,";
                //strQry += "CAST(\"DT_TOTAL_CON_HP\" AS TEXT) DT_TOTAL_CON_HP,CAST(\"TC_CODE\" AS TEXT) TC_CODE, case when \"TC_CAPACITY\" = '500' then '0' else \"TC_CAPACITY\" end TC_CAPACITY,";
                //strQry += "TO_CHAR(\"DT_LAST_SERVICE_DATE\",'DD-MON-YYYY') DT_LAST_SERVICE_DATE,\"DT_PROJECTTYPE\", ";
                //strQry += "\"FD_FEEDER_NAME\" AS \"FEEDER_NAME\" FROM \"TBLDTCMAST\", \"TBLTCMASTER\",\"TBLFEEDERMAST\",\"TBLSTATION\" WHERE \"TC_CODE\" = \"DT_TC_ID\"";
                //strQry += "AND CAST(\"DT_FDRSLNO\" AS TEXT)=\"FD_FEEDER_CODE\" AND \"ST_ID\"=\"FD_ST_ID\"   AND cast(\"DT_OM_SLNO\" as TEXT) LIKE :offcode||'%' ";
                #endregion

                #region old Inline query
                //NpgsqlCommand.Parameters.AddWithValue("offcode", objDTC.sOfficeCode);
                //strQry = "SELECT \"DT_ID\",cast(\"DT_CODE\" as TEXT) DT_CODE,\"DT_NAME\",CAST(\"DT_TOTAL_CON_KW\" AS TEXT) DT_TOTAL_CON_KW,";
                //strQry += "CAST(\"DT_TOTAL_CON_HP\" AS TEXT) DT_TOTAL_CON_HP,CAST(\"TC_CODE\" AS TEXT) TC_CODE,\"TC_CAPACITY\",";
                //strQry += "TO_CHAR(\"DT_LAST_SERVICE_DATE\",'DD-MON-YYYY') DT_LAST_SERVICE_DATE,\"DT_PROJECTTYPE\", ";
                //strQry += "\"FD_FEEDER_NAME\" AS \"FEEDER_NAME\" FROM \"TBLDTCMAST\", \"TBLTCMASTER\",\"TBLFEEDERMAST\",\"TBLSTATION\" WHERE \"TC_CODE\" = \"DT_TC_ID\"";
                //strQry += "AND CAST(\"DT_FDRSLNO\" AS TEXT)=\"FD_FEEDER_CODE\" AND \"ST_ID\"=\"FD_ST_ID\"   AND cast(\"DT_OM_SLNO\" as TEXT) LIKE :offcode||'%' ";

                //if (objDTC.sFeederCode != null && objDTC.sFeederCode != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("feedcode", objDTC.sFeederCode);
                //    strQry += " AND \"DT_FDRSLNO\" LIKE :feedcode||'%'";
                //}
                //if (objDTC.sFeederName != null && objDTC.sFeederName != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("feedname", objDTC.sFeederName.Trim().Replace("'", "''").ToUpper());
                //    strQry += " AND \"FD_FEEDER_NAME\" LIKE '%'||:feedname||'%'";
                //}

                //if (objDTC.sProjectType != null && objDTC.sProjectType != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("projecttype", Convert.ToDouble(objDTC.sProjectType));
                //    strQry += " AND \"DT_PROJECTTYPE\"=:projecttype";
                //}
                //if (objDTC.sStation != null && objDTC.sStation != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("station", Convert.ToInt32(objDTC.sStation));
                //    strQry += " AND \"ST_ID\"=:station";
                //}
                //if (objDTC.sDtcCode != null && objDTC.sDtcCode != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("dtcCode", objDTC.sDtcCode.Trim().Replace("'", "''").ToUpper());
                //    strQry += " AND \"DT_CODE\" like '%'||:dtcCode||'%' ";
                //}
                //if (objDTC.sTcCode != null && objDTC.sTcCode != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("tcCode", Convert.ToString(objDTC.sTcCode.Trim().Replace("'", "''").ToUpper()));
                //    strQry += " AND \"DT_TC_ID\" like '%'||:tcCode|| '%'";
                //}
                //if (objDTC.sDtcName != null && objDTC.sDtcName != "")
                //{
                //    //NpgsqlCommand.Parameters.AddWithValue("dtcname", Convert.ToString(objDTC.sDtcName.Trim().Replace("'", "''").ToUpper()));
                //    //strQry += " AND \"DT_NAME\" like '%'||:dtcname||'%'";
                //    //NpgsqlCommand.Parameters.AddWithValue("dtcname", Convert.ToString(objDTC.sDtcName.Trim().Replace("'", "''").ToUpper()));
                //    strQry += " AND UPPER(\"DT_NAME\") LIKE '%" + objDTC.sDtcName.Trim().Replace("'", "''").ToUpper() + "%'";
                //}

                //if (objDTC.sOfficeCode == "" || objDTC.sOfficeCode == null)
                //{
                //    strQry += " ORDER BY \"DT_ID\" DESC  LIMIT 300";
                //}
                //else
                //{
                //    strQry += " ORDER BY \"DT_ID\" DESC ";
                //}
                //dtDtcDetails = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_dtc_grid_for_clsdtcmaster");
                cmd.Parameters.AddWithValue("p_offcode", Convert.ToString(objDTC.sOfficeCode ?? ""));
                cmd.Parameters.AddWithValue("p_feedercode", Convert.ToString(objDTC.sFeederCode ?? ""));
                cmd.Parameters.AddWithValue("p_feedername", Convert.ToString(objDTC.sFeederName ?? "").Trim().Replace("'", "''").ToUpper());
                cmd.Parameters.AddWithValue("p_projecttype", Convert.ToString(objDTC.sProjectType ?? ""));
                cmd.Parameters.AddWithValue("p_station", Convert.ToString(objDTC.sStation ?? ""));
                cmd.Parameters.AddWithValue("p_dtccode", Convert.ToString(objDTC.sDtcCode ?? "").Trim().Replace("'", "''").ToUpper());
                cmd.Parameters.AddWithValue("p_tccode", Convert.ToString(objDTC.sTcCode ?? "").Trim().Replace("'", "''").ToUpper());
                cmd.Parameters.AddWithValue("p_dtcname", Convert.ToString(objDTC.sDtcName ?? "").Trim().Replace("'", "''").ToUpper());
                dtDtcDetails = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtDtcDetails;
            }
            return dtDtcDetails;
        }
        /// <summary>
        /// GetDtcDetails
        /// </summary>
        /// <param name="objDtcMaster"></param>
        /// <returns></returns>
        public object GetDtcDetails(clsDtcMaster objDtcMaster)
        {
            string sResult = string.Empty;
            string QryKey = string.Empty;
            DataTable dtDcDetails = new DataTable();
            try
            {
                #region old inline query                
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("dtcId", Convert.ToInt32(objDtcMaster.lDtcId));
                //strQry = " SELECT \"DT_ID\",\"DT_CODE\",\"DT_NAME\",\"DT_TC_ID\",\"DT_OM_SLNO\",TO_CHAR(\"DT_TOTAL_CON_KW\") DT_TOTAL_CON_KW,TO_CHAR(\"DT_TOTAL_CON_HP\") DT_TOTAL_CON_HP,TO_CHAR(\"DT_KWH_READING\") DT_KWH_READING,\"DT_PLATFORM\",";
                //strQry += " \"DT_INTERNAL_CODE\",\"DT_TC_ID\",to_char(\"DT_CON_DATE\",'dd/MM/yyyy')DT_CON_DATE,to_char(\"DT_LAST_INSP_DATE\",'dd/MM/yyyy')DT_LAST_INSP_DATE,";
                //strQry += " to_char(\"DT_LAST_SERVICE_DATE\",'dd/MM/yyyy')DT_LAST_SERVICE_DATE,to_char(\"DT_TRANS_COMMISION_DATE\",'dd/MM/yyyy')DT_TRANS_COMMISION_DATE,";
                //strQry += " to_char(\"DT_FDRCHANGE_DATE\",'dd/MM/yyyy')DT_FDRCHANGE_DATE,to_char(\"DT_CON_DATE\",'dd/MM/yyyy') DT_CON_DATE, COALESCE(\"DT_BREAKER_TYPE\",0) DT_BREAKER_TYPE,  ";
                //strQry += "  COALESCE(\"DT_DTCMETERS\",0) DT_DTCMETERS,  COALESCE(\"DT_HT_PROTECT\",0) DT_HT_PROTECT, COALESCE(\"DT_LT_PROTECT\",0) DT_LT_PROTECT, COALESCE( \"DT_GROUNDING\",0) DT_GROUNDING, ";
                //strQry += " COALESCE(\"DT_ARRESTERS\",0) DT_ARRESTERS, \"DT_LT_LINE\", \"DT_HT_LINE\" FROM ";
                //strQry += " \"TBLDTCMAST\" WHERE \"DT_ID\"=:dtcId";
                //dtDcDetails = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_dtc_details_for_clsdtcmaster");
                cmd.Parameters.AddWithValue("p_dtcid", Convert.ToString(objDtcMaster.lDtcId ?? ""));
                dtDcDetails = ObjCon.FetchDataTable(cmd);

                if (dtDcDetails.Rows.Count > 0)
                {
                    objDtcMaster.lDtcId = Convert.ToString(dtDcDetails.Rows[0]["DT_ID"]);
                    objDtcMaster.sDtcCode = Convert.ToString(dtDcDetails.Rows[0]["DT_CODE"]);
                    objDtcMaster.sDtcName = Convert.ToString(dtDcDetails.Rows[0]["DT_NAME"]);
                    objDtcMaster.sOMSectionName = Convert.ToString(dtDcDetails.Rows[0]["DT_OM_SLNO"]);
                    objDtcMaster.iConnectedKW = Convert.ToString(dtDcDetails.Rows[0]["DT_TOTAL_CON_KW"]);
                    objDtcMaster.iConnectedHP = Convert.ToString(dtDcDetails.Rows[0]["DT_TOTAL_CON_HP"]);
                    objDtcMaster.iKWHReading = Convert.ToString(dtDcDetails.Rows[0]["DT_KWH_READING"]);
                    objDtcMaster.sPlatformType = Convert.ToString(dtDcDetails.Rows[0]["DT_PLATFORM"]);
                    objDtcMaster.sTcCode = Convert.ToString(dtDcDetails.Rows[0]["DT_TC_ID"]);
                    objDtcMaster.sConnectionDate = Convert.ToString(dtDcDetails.Rows[0]["DT_CON_DATE"]);
                    objDtcMaster.sInspectionDate = Convert.ToString(dtDcDetails.Rows[0]["DT_LAST_INSP_DATE"]);
                    objDtcMaster.sServiceDate = Convert.ToString(dtDcDetails.Rows[0]["DT_LAST_SERVICE_DATE"]);
                    objDtcMaster.sCommisionDate = Convert.ToString(dtDcDetails.Rows[0]["DT_TRANS_COMMISION_DATE"]);
                    objDtcMaster.sFeederChangeDate = Convert.ToString(dtDcDetails.Rows[0]["DT_FDRCHANGE_DATE"]);
                    objDtcMaster.sInternalCode = Convert.ToString(dtDcDetails.Rows[0]["DT_INTERNAL_CODE"]);
                    objDtcMaster.sBreakertype = Convert.ToString(dtDcDetails.Rows[0]["DT_BREAKER_TYPE"]);
                    objDtcMaster.sDTCMeters = Convert.ToString(dtDcDetails.Rows[0]["DT_DTCMETERS"]);
                    objDtcMaster.sHTProtect = Convert.ToString(dtDcDetails.Rows[0]["DT_HT_PROTECT"]);
                    objDtcMaster.sLTProtect = Convert.ToString(dtDcDetails.Rows[0]["DT_LT_PROTECT"]);
                    objDtcMaster.sGrounding = Convert.ToString(dtDcDetails.Rows[0]["DT_GROUNDING"]);
                    objDtcMaster.sArresters = Convert.ToString(dtDcDetails.Rows[0]["DT_ARRESTERS"]);
                    objDtcMaster.sLtlinelength = Convert.ToString(dtDcDetails.Rows[0]["DT_LT_LINE"]);
                    objDtcMaster.sHtlinelength = Convert.ToString(dtDcDetails.Rows[0]["DT_HT_LINE"]);
                }

                #region old inline query
                //NpgsqlCommand.Parameters.AddWithValue("tccd", Convert.ToDouble(objDtcMaster.sTcCode));
                //strQry = "SELECT \"TC_SLNO\" ||  '~' ||  \"TM_NAME\" || '~' || TO_CHAR(\"TC_CAPACITY\") TC_CAPACITY FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\"  WHERE \"TC_MAKE_ID\"= \"TM_ID\" and \"TC_CODE\"=:tccd";
                //sResult = ObjCon.get_value(strQry, NpgsqlCommand);
                #endregion

                QryKey = "GET_TC_SLNO_AND_TM_NAME_AND_TC_CAPACITY";
                NpgsqlCommand cmd_TC_CAPACITY = new NpgsqlCommand("fetch_getvalue_clsdtcmaster");
                cmd_TC_CAPACITY.Parameters.AddWithValue("p_key", QryKey);
                cmd_TC_CAPACITY.Parameters.AddWithValue("p_value_1", Convert.ToString(objDtcMaster.sTcCode ?? ""));
                cmd_TC_CAPACITY.Parameters.AddWithValue("p_value_2", "");
                sResult = ObjBseCon.StringGetValue(cmd_TC_CAPACITY);

                if (sResult != "")
                {
                    objDtcMaster.sTcSlno = sResult.Split('~').GetValue(0).ToString();
                    objDtcMaster.sTCMakeName = sResult.Split('~').GetValue(1).ToString();
                    objDtcMaster.sTCCapacity = sResult.Split('~').GetValue(2).ToString();
                }

                return objDtcMaster;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objDtcMaster;
            }

        }
        /// <summary>
        /// To get TC Details Used in DTCMaster Form
        /// </summary>
        /// <param name="objTCMaster"></param>
        /// <returns></returns>
        public object GetTCDetails(clsDtcMaster objDTCMaster)
        {
            DataTable dt = new DataTable();
            try
            {
                #region Old inline query
                //string sQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("tccd", Convert.ToDouble(objDTCMaster.sTcCode));
                //sQry = "SELECT \"TC_SLNO\",\"TC_CODE\",\"TM_NAME\",CAST(\"TC_CAPACITY\" AS TEXT) TC_CAPACITY FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\" WHERE \"TC_MAKE_ID\"= \"TM_ID\" and ";
                //sQry += "\"TC_CODE\" NOT IN (SELECT \"RSD_TC_CODE\" from \"TBLREPAIRSENTDETAILS\" where \"RSD_DELIVARY_DATE\" is NULL ) AND \"TC_STATUS\"=3 AND  ";
                //sQry += "\"TC_CURRENT_LOCATION\" =1 AND \"TC_CODE\"=:tccd";
                //dt = ObjCon.FetchDataTable(sQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_tcdetails_for_clsdtcmaster");
                cmd.Parameters.AddWithValue("p_tccode", Convert.ToString(objDTCMaster.sTcCode ?? ""));
                dt = ObjCon.FetchDataTable(cmd);

                if (dt.Rows.Count > 0)
                {
                    objDTCMaster.sTcSlno = dt.Rows[0]["TC_SLNO"].ToString();
                    objDTCMaster.sTCMakeName = dt.Rows[0]["TM_NAME"].ToString();
                    objDTCMaster.sTCCapacity = dt.Rows[0]["TC_CAPACITY"].ToString();
                    objDTCMaster.sTcCode = dt.Rows[0]["TC_CODE"].ToString();
                }
                return objDTCMaster;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objDTCMaster;
            }
        }

        #region Unused code
        //public DataTable GetDTCDetailsUsingFeederCode(clsDtcMaster objDTC)
        //{
        //    string strQry = string.Empty;
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        strQry = "SELECT \"DT_ID\" , \"DT_CODE\" , \"DT_NAME\" ,\"DT_TC_ID\"  FROM \"TBLDTCMAST\" WHERE \"DT_FDRSLNO\" = '" + objDTC.sFeederCode + "' ORDER BY \"DT_CODE\"";
        //        dt = ObjCon.FetchDataTable(strQry);

        //        return dt;

        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetPONo");
        //        return dt;
        //    }

        //}

        //public string[] GetPONo(String sPoNO)
        //{
        //    NpgsqlCommand = new NpgsqlCommand();
        //    string[] Arr = new string[2];
        //    try
        //    {
        //        string sQry = string.Empty;
        //        DataTable dt = new DataTable();
        //        NpgsqlCommand.Parameters.AddWithValue("pono", sPoNO.Trim());
        //        sQry = "SELECT \"RSM_PO_NO\" FROM \"TBLREPAIRSENTMASTER\" WHERE \"RSM_PO_NO\"=:pono";
        //        string PO_NO = ObjCon.get_value(sQry, NpgsqlCommand);
        //        if (PO_NO != "")
        //        {
        //            Arr[0] = PO_NO;
        //            Arr[1] = "1";
        //        }
        //        else
        //        {
        //            Arr[0] = "Entered Purchase Order Number Not Exist";
        //            Arr[1] = "2";
        //        }

        //        return Arr;

        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        Arr[0] = "Somthing went Wrong";
        //        Arr[1] = "3";
        //        return Arr;
        //    }
        //}

        //    public string[] ApproveFbcnRecords(clsDtcMaster obj)
        //    {
        //        string[] resultArray = new string[3];
        //        string strQry = string.Empty;
        //        try
        //        {
        //            //strQry = " SELECT LISTAGG( \"DTE_DTCCODE\"  , ',') WITHIN GROUP (ORDER BY \"DTE_DTCCODE\" )   \"DTE_DTCCODE\"  FROM  " +
        //            //     " \"TBLDTCENUMERATION\"  , \"TBLENUMERATIONDETAILS\" WHERE \"ED_ID\" = \"DTE_ED_ID\" and  \"DTE_DTCCODE\" in (  SELECT  " +
        //            //    " \"FBDS_NEW_DTC_CODE\"  FROM \"TBLFEEDER_BFCN_DETAILS_SO\"  WHERE  \"FBDS_FB_ID\" in (1) ) and  \"ED_STATUS_FLAG\"  in ('" + obj.lDtcId + "')  ";

        //            strQry = " SELECT STRING_AGG( cast(\"DTE_DTCCODE\" as text) , ',')   \"DTE_DTCCODE\"  FROM  " +
        // " \"TBLDTCENUMERATION\"  , \"TBLENUMERATIONDETAILS\" WHERE \"ED_ID\" = \"DTE_ED_ID\" and  \"DTE_DTCCODE\" in (  SELECT  " +
        //" \"FBDS_NEW_DTC_CODE\"  FROM \"TBLFEEDER_BFCN_DETAILS_SO\"  WHERE  \"FBDS_FB_ID\" in (1) ) and  \"ED_STATUS_FLAG\"  in ('" + obj.lDtcId + "')  ";
        //            string status = ObjCon.get_value(strQry);

        //            if (status != "")
        //            {
        //                resultArray[0] = status + " DTC Codes were already Enumerated,Please contact support team";
        //                resultArray[1] = "0";
        //                return resultArray;
        //            }

        //            strQry = "	UPDATE \"TBLFEEDERBIFURCATION_SO\" set \"FBS_APP_BY\" = '" + obj.sCrBy + "'  ,  \"FBS_APP_ON\" = now() ,\"FBS_STATUS\" = 1   WHERE \"FBS_ID\" = '" + obj.lDtcId + "'  ";
        //            ObjCon.ExecuteQry(strQry);

        //            strQry = "	UPDATE \"TBLFEEDER_BFCN_DETAILS_SO\" set \"FBDS_STATUS\" = 1   WHERE \"FBDS_FB_ID\" = '" + obj.lDtcId + "'  ";
        //            ObjCon.ExecuteQry(strQry);

        //            strQry = "SELECT  \"US_MOBILE_NO\"  FROM \"TBLUSER\" , \"TBLFEEDERBIFURCATION_SO\"  WHERE \"US_ID\" = \"FBS_CR_BY\" and  \"FBS_ID\" = '" + obj.lDtcId + "' ";

        //            resultArray[0] = "Approved Successfully";
        //            resultArray[1] = "1";


        //            SendSMSToSectionOfficer(ObjCon.get_value(strQry));

        //            return resultArray;
        //        }
        //        catch (Exception ex)
        //        {
        //            clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetNewDTCCode");
        //            resultArray[0] = "Exception Occurred ";
        //            resultArray[1] = "-1";
        //            return resultArray;
        //        }
        //    }

        //public void SendSMSToSectionOfficer(string sMobileNo)
        //{
        //    try
        //    {
        //        clsCommunication objComm = new clsCommunication();
        //        objComm.sSMSkey = "SMS_FB_SO";
        //        objComm = objComm.GetsmsTempalte(objComm);
        //        objComm.DumpSms(sMobileNo, objComm.sSMSTemplate, objComm.sSMSTemplateID);
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SendSMSToSectionOfficer");
        //    }

        //}

        //public string CheckFeederCodeStatus(string selectedValue)
        //{
        //    string strQry = string.Empty;
        //    try
        //    {
        //        //strQry = "SELECT case FBS_NEW_FEEDER_CODE when null then 'SUCCESS' else 'Feeder '||FBS_NEW_FEEDER_CODE|| ' pending  with '|| ( " +
        //        //    " SELECT OM_NAME  FROM TBLOMSECMAST WHERE US_OFFICE_CODE = OM_CODE )  ||' Section Officer for Feeder Bifurcation, Please contact through '||US_MOBILE_NO ||' or " +
        //        //    "  Contact Support Team'   end as STATUS    FROM TBLFEEDERBIFURCATION_SO , TBLUSER  WHERE US_ID = FBS_CR_BY and  FBS_STATUS in (0,1) and " +
        //        //    " FBS_NEW_FEEDER_CODE = '" + selectedValue + "'";
        //        strQry= "SELECT case \"FBS_NEW_FEEDER_CODE\" when null then 'SUCCESS' else 'Feeder '||\"FBS_NEW_FEEDER_CODE\"|| ' pending  with '|| (  SELECT \"OM_NAME\"  FROM \"TBLOMSECMAST\" WHERE \"US_OFFICE_CODE\" = 'OM_CODE' )  ||' Section Officer for Feeder Bifurcation, Please contact through '||\"US_MOBILE_NO\" ||' or   Contact Support Team'   end as STATUS    FROM \"TBLFEEDERBIFURCATION_SO\" , \"TBLUSER\"  WHERE \"US_ID\" = \"FBS_CR_BY\" and  \"FBS_STATUS\" in (0,1) and  \"FBS_NEW_FEEDER_CODE\" = '" + selectedValue + "'";
        //        return ObjCon.get_value(strQry);
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveUpdateDtcDetails");
        //        throw ex;
        //    }
        //}

        //public DataTable GetDTCDetailsUsingFeederCodeSectionOfficer(clsDtcMaster objDTC)
        //{
        //    string strQry = string.Empty;
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        //strQry = "SELECT DT_ID , DT_CODE , DT_NAME ,DT_TC_ID , CASE WHEN DT_CODE =(SELECT DF_DTC_CODE FROM TBLDTCFAILURE  WHERE DF_DTC_CODE=DT_CODE AND  DF_REPLACE_FLAG=0) THEN 'FAILURE' ELSE 'GOOD' END AS STATUS" +
        //        //    " FROM TBLDTCMAST WHERE DT_FDRSLNO = '" + objDTC.sFeederCode + "' " +
        //        //    "  and  DT_CODE not  in   (SELECT FBDS_OLD_DTC_CODE  FROM TBLFEEDERBIFURCATION_SO , TBLFEEDER_BFCN_DETAILS_SO WHERE FBS_ID = FBDS_FB_ID and FBDS_STATUS in (0,1)) " +
        //        //    " ORDER BY DT_CODE ";
        //        strQry = "SELECT \"DT_ID\" , \"DT_CODE\" , \"DT_NAME\",\"DT_TC_ID\" , CASE WHEN \"DT_CODE\" =(SELECT \"DF_DTC_CODE\" FROM \"TBLDTCFAILURE\"  WHERE \"DF_DTC_CODE\"=\"DT_CODE\" AND  \"DF_REPLACE_FLAG\"=0) THEN 'FAILURE' ELSE 'GOOD' END AS STATUS FROM \"TBLDTCMAST\" WHERE \"DT_FDRSLNO\" = '" + objDTC.sFeederCode + "'   and  \"DT_CODE\" not  in   (SELECT \"FBDS_OLD_DTC_CODE\"  FROM \"TBLFEEDERBIFURCATION_SO\" , \"TBLFEEDER_BFCN_DETAILS_SO\" WHERE \"FBS_ID\" = \"FBDS_FB_ID\" and \"FBDS_STATUS\" in (0,1))  ORDER BY \"DT_CODE\" ";

        //        dt = ObjCon.FetchDataTable(strQry);

        //        return dt;

        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetDTCDetailsUsingFeederCodeSectionOfficer");
        //        return dt;
        //    }

        //}

        //public DataTable GetDTCDetailsUsingIdFeederCode(clsDtcMaster objDTC, String dtcIDs = "")
        //{
        //    string strQry = string.Empty;
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        if (objDTC.sFeederCode.Length == 0 || dtcIDs.Length == 0)
        //        {
        //            return dt;
        //        }


        //        // if (((objDTC.sFeederCode != "") || (objDTC.sFeederCode != null) ) && ((dtcIDs == null) || (dtcIDs =="")))

        //        //                // first query to get the Data based on DTC ID for the checked values
        //        //                strQry = " SELECT * FROM ( SELECT DT_ID ,DT_CODE ,DT_TC_ID, '' AS  DT_SERIAL_NUMBER ,DT_NAME ,DT_FDRSLNO AS OLD_FEEDER_CODE , '" + objDTC.sFeederCode + "' AS NEW_FEEDER_CODE, DT_OM_SLNO AS OFFICECODE , 2 AS ORDER_STATUS ";
        //        //                strQry += " FROM TBLDTCMAST WHERE DT_ID IN (" + dtcIDs + ") ";
        //        //                strQry += "UNION ALL";
        //        //                // second one for the DTC's under the NEW  FEEDR CODE
        //        //                strQry += " SELECT DT_ID , DT_CODE, DT_TC_ID, SUBSTR(DT_CODE,5,2) AS  DT_SERIAL_NUMBER ,DT_NAME ,DT_FDRSLNO AS OLD_FEEDER_CODE , '" + objDTC.sFeederCode + "' AS NEW_FEEDER_CODE , DT_OM_SLNO AS OFFICECODE , 1 AS ORDER_STATUS  FROM TBLDTCMAST WHERE DT_FDRSLNO IN ";
        //        //                strQry += " ('" + objDTC.sFeederCode + "') )A ORDER BY ORDER_STATUS,DT_CODE ";
        //        ////

        //        strQry = " SELECT * FROM ( SELECT \"DT_ID\" ,\"DT_CODE\" ,\"DT_TC_ID\", '' AS  \"DT_SERIAL_NUMBER\" ,\"DT_NAME\" ,\"DT_FDRSLNO\" AS \"OLD_FEEDER_CODE\" ,'" + objDTC.sFeederCode + "' AS \"NEW_FEEDER_CODE\", \"DT_OM_SLNO\" AS \"OFFICECODE\" , 2 AS \"ORDER_STATUS\"";
        //         strQry+= "FROM \"TBLDTCMAST\" WHERE \"DT_ID\" IN (" + dtcIDs + ") UNION ALL SELECT \"DT_ID\" , \"DT_CODE\", \"DT_TC_ID\", SUBSTR(\"DT_CODE\",7,3) AS  \"DT_SERIAL_NUMBER\" ,\"DT_NAME\" ,\"DT_FDRSLNO\" AS \"OLD_FEEDER_CODE\" , '" + objDTC.sFeederCode + "' AS \"NEW_FEEDER_CODE\" , \"DT_OM_SLNO\" AS \"OFFICECODE\" , 1 AS \"ORDER_STATUS\"  FROM \"TBLDTCMAST\" WHERE \"DT_FDRSLNO\" IN  ('" + objDTC.sFeederCode + "') )A ORDER BY \"ORDER_STATUS\",\"DT_CODE\" ";

        //        if (strQry.Length != 0)
        //        {
        //            dt = ObjCon.FetchDataTable(strQry);
        //        }

        //        return dt;

        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetDTCDetailsUsingIdFeederCode");
        //        return dt;
        //    }

        //}

        //public DataTable GetFeederBfcnRecordsID(string strFbsId)
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        string strQry = string.Empty;
        //        //strQry = "SELECT case  \"FBS_STATUS\" when 1 then 'APPROVED' when 2 then 'BIFURCATED' WHEN 0 THEN 'PENDING' ELSE 'PENDING' END AS \"FBS_STATUS\" ," +
        //        //    " \"FBDS_NEW_FEEDER_CODE\" ,  LISTAGG( \"FBDS_OLD_DTC_ID\"  , ',') WITHIN GROUP (ORDER BY \"FBDS_OLD_DTC_ID\" ) as" +
        //        //    "  \"FBDS_OLD_DTC_ID\"  FROM  \"TBLFEEDER_BFCN_DETAILS_SO\" , \"TBLFEEDERBIFURCATION_SO\"  WHERE \"FBS_ID\"  =\"FBDS_FB_ID\" and  \"FBDS_FB_ID\" = '" + strFbsId + "' GROUP BY \"FBDS_NEW_FEEDER_CODE\" , \"FBS_STATUS\"  ";


        //        strQry = " SELECT 	case  \"FBS_STATUS\" when 1 then 'APPROVED' when 2 then 'BIFURCATED' WHEN 0 THEN 'PENDING' ELSE 'PENDING' END AS \"FBS_STATUS\" , \"FBDS_NEW_FEEDER_CODE\" ," +
        //            "STRING_AGG(cast(\"FBDS_OLD_DTC_ID\" as text)  , ',') as  \"FBDS_OLD_DTC_ID\"  FROM  \"TBLFEEDER_BFCN_DETAILS_SO\" , \"TBLFEEDERBIFURCATION_SO\"  WHERE \"FBS_ID\"  =\"FBDS_FB_ID\" and  \"FBDS_FB_ID\" = '" + strFbsId + "' " +
        //            "GROUP BY   \"FBDS_NEW_FEEDER_CODE\" , \"FBS_STATUS\"  ";



        //        dt = ObjCon.FetchDataTable(strQry);
        //        return dt;

        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetNewDTCCode");
        //        return dt;
        //    }
        //}

        //public DataTable GetDTCDetailsUsingIdFeederCodeApproval(clsDtcMaster objDTC, String dtcIDs = "")
        //{
        //    string strQry = string.Empty;
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        if (objDTC.sFeederCode.Length == 0 || dtcIDs.Length == 0)
        //        {
        //            return dt;
        //        }


        //        // if (((objDTC.sFeederCode != "") || (objDTC.sFeederCode != null) ) && ((dtcIDs == null) || (dtcIDs =="")))

        //        // first query to get the Data based on DTC ID for the checked values
        //        strQry = " SELECT * FROM ( SELECT \"DT_ID\" ,\"DT_CODE\" ,\"DT_TC_ID\", (SELECT substr(\"FBDS_NEW_DTC_CODE\" , 7 ,3 ) FROM  \"TBLFEEDER_BFCN_DETAILS_SO\" WHERE \"DT_CODE\" = \"FBDS_OLD_DTC_CODE\") AS  \"DT_SERIAL_NUMBER\" ,\"DT_NAME\" ,\"DT_FDRSLNO\" AS \"OLD_FEEDER_CODE\" , '" + objDTC.sFeederCode + "' AS \"NEW_FEEDER_CODE\", \"DT_OM_SLNO\" AS \"OFFICECODE\" , 2 AS \"ORDER_STATUS\" ";
        //        strQry += " FROM \"TBLDTCMAST\" WHERE \"DT_ID\" IN (" + dtcIDs + ") ";
        //        strQry += "UNION ALL";
        //        // second one for the DTC's under the NEW  FEEDR CODE
        //        strQry += " SELECT \"DT_ID\" , \"DT_CODE\", \"DT_TC_ID\", SUBSTR(\"DT_CODE\",7,3) AS  \"DT_SERIAL_NUMBER\" ,\"DT_NAME\" ,\"DT_FDRSLNO\" AS \"OLD_FEEDER_CODE\" , '" + objDTC.sFeederCode + "' AS \"NEW_FEEDER_CODE\" , \"DT_OM_SLNO\" AS \"OFFICECODE\" , 1 AS \"ORDER_STATUS\"  FROM \"TBLDTCMAST\" WHERE \"DT_FDRSLNO\" IN ";
        //        strQry += " ('" + objDTC.sFeederCode + "') )A  ORDER BY \"ORDER_STATUS\",\"DT_CODE\" ";

        //        //  strQry = "SELECT DT_ID , '" + newFeederCode + "' AS FEEDER_CODE ,DT_FDRSLNO AS OLD_FEEDER_CODE ,  DT_CODE , DT_NAME  FROM TBLDTCMAST WHERE DT_ID IN (" + dtcID + ") ORDER BY DT_CODE";

        //        if (strQry.Length != 0)
        //        {
        //            dt = ObjCon.FetchDataTable(strQry);
        //        }

        //        return dt;

        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetDTCDetailsUsingIdFeederCodeApproval");
        //        return dt;
        //    }

        //}

        //public DataTable GetFdrBfcnDetails(clsDtcMaster obj)
        //{
        //    try
        //    {
        //        string StrQry = string.Empty;
        //        StrQry = "  SELECT \"FBS_OLD_FEEDER_CODE\"  ,  \"FBS_NEW_FEEDER_CODE\" ,  to_char(\"FBS_OM_DATE\" ,'dd-MM-yyyy')  as \"FBS_OM_DATE\" , \"FDO_OFFICE_CODE\"   from " +
        //            " \"TBLFEEDERBIFURCATION_SO\"   left join  \"TBLFEEDERMAST\"  on \"FBS_OLD_FEEDER_CODE\" = \"FD_FEEDER_CODE\" left join " +
        //            " \"TBLFEEDEROFFCODE\"  on  \"FDO_FEEDER_ID\" = \"FD_FEEDER_ID\"  WHERE \"FBS_ID\" = '" + obj.lDtcId + "' ";

        //        return ObjCon.FetchDataTable(StrQry);
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetFdrBfcnDetails");
        //        return null;
        //    }
        //}
        //public DataTable GetFeederBfcnRecords(clsDtcMaster obj)
        //{

        //    DataTable dtFeederDetails = new DataTable();
        //    string strQry = string.Empty;
        //    try
        //    {
        //        //strQry = " SELECT FBS_ID ,  FBS_OLD_FEEDER_CODE as OLD_FEEDER_CODE , FBS_NEW_FEEDER_CODE  as NEW_FEEDER_CODE , " +
        //        //    "  case FBS_status WHEN 1 THEN 'APPROVED' WHEN 0 THEN 'PENDING' WHEN  2  THEN 'FEEDER BIFRUCATED' ELSE 'PENDING'  END  AS STATUS  " +
        //        //    " , (SELECT count(*) FROM TBLFEEDER_BFCN_DETAILS_SO WHERE FBS_ID = FBDS_FB_ID GROUP BY  FBDS_FB_ID  ) as COUNT_DTC  , " +
        //        //    " (SELECT US_FULL_NAME  FROM TBLUSER WHERE US_ID = FBS_CR_BY  ) as SECTION_OFFICER ," +
        //        //    " to_char(FBS_CRON ,  'dd-MON-yyyy') as CREATED_ON ,  (SELECT US_FULL_NAME  FROM TBLUSER WHERE US_ID = FBS_APP_BY  ) as APPROVED_BY, " +
        //        //    " to_char(FBS_APP_ON ,  'dd-MON-yyyy') as APPROVED_ON FROM TBLFEEDERBIFURCATION_SO WHERE FBS_SECTION_CODE like '" + obj.sOfficeCode + "%' ORDER BY FBS_ID desc  ";


        //        strQry = "SELECT \"FBS_ID\" ,  \"FBS_OLD_FEEDER_CODE\" as \"OLD_FEEDER_CODE\" , \"FBS_NEW_FEEDER_CODE\" as \"NEW_FEEDER_CODE\" ,   case \"FBS_STATUS\" WHEN 1 THEN 'APPROVED' WHEN 0 THEN 'PENDING' WHEN  2  THEN 'FEEDER BIFRUCATED' ELSE 'PENDING'  END  AS STATUS   , (SELECT count(*) FROM \"TBLFEEDER_BFCN_DETAILS_SO\" WHERE \"FBS_ID\" = \"FBDS_FB_ID\" GROUP BY  \"FBDS_FB_ID\"  ) as \"COUNT_DTC\"  ,  (SELECT \"US_FULL_NAME\"  FROM \"TBLUSER\" WHERE \"US_ID\" = \"FBS_CR_BY\"  ) as \"SECTION_OFFICER\" , to_char(\"FBS_CRON\" ,  'dd-MON-yyyy') as \"CREATED_ON\" ,  (SELECT \"US_FULL_NAME\"  FROM \"TBLUSER\" WHERE \"US_ID\" = \"FBS_APP_BY\"  ) as \"APPROVED_BY\",  to_char(\"FBS_APP_ON\" ,  'dd-MON-yyyy') as \"APPROVED_ON\" FROM \"TBLFEEDERBIFURCATION_SO\" WHERE CAST(\"FBS_SECTION_CODE\" AS TEXT) like '" + obj.sOfficeCode + "%' ORDER BY \"FBS_ID\" desc";
        //        dtFeederDetails = ObjCon.FetchDataTable(strQry);

        //        return dtFeederDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveUpdateDtcDetails");
        //        return dtFeederDetails;
        //    }
        //}
        //public string GetStationCode(string sNewFeederCode)
        //{
        //    try
        //    {
        //        string strQry = " SELECT \"FD_ST_ID\"  FROM \"TBLFEEDERMAST\" WHERE \"FD_FEEDER_CODE\" = '" + sNewFeederCode + "'";
        //        return ObjCon.get_value(strQry);
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetStationCode");
        //        throw ex;
        //    }
        //}

        //public Tuple<string[], List<string>> UpdateFeederBifurcation(ArrayList lst, StringBuilder oldDTCIDs, StringBuilder newDTCCodes, HashSet<string> selectedFeederCodes, clsDtcMaster objDTCMast, string clientIP, string through)
        //{

        //    // 0-DTCID  // 1-OldDTCCode  // 2-OldFeederCode  //  3-NewFeederCode  //  4-NewDTCCode
        //    string[] Arr = new string[3];
        //    string strQry = string.Empty;
        //    DataTable dt = new DataTable();
        //    List<string> lstfailedDTC = new List<string>();
        //    List<string> lstDuplicateDTC = new List<string>();
        //    string failedDTC = string.Empty;
        //    string duplicateDTC = string.Empty;
        //    string description = string.Empty;
        //    string[] arrFeederCodes = selectedFeederCodes.ToArray();
        //    string sFeederCodes = string.Join(",", arrFeederCodes);
        //    string scrby = objDTCMast.sCrBy;
        //    string sOmDate = objDTCMast.sDate;
        //    string fbs_id = string.Empty;

        //    //string sFolderPath = Convert.ToString(ConfigurationSettings.AppSettings["LOGFILEPATH"]) + DateTime.Now.ToString("yyyyMM");

        //    //if (!Directory.Exists(sFolderPath))
        //    //{
        //    //    Directory.CreateDirectory(sFolderPath);

        //    //}
        //    //string sFileName = "CESCMAIN";
        //    //string sPath = sFolderPath + "//" + sFileName + DateTime.Now.ToString("yyyyMMdd") + "-ErrorLog.txt";


        //    try
        //    {
        //        #region Check for DTC Failure  if failed  than add it to  the list "lstfailedDTC" 

        //        //strQry = "SELECT LISTAGG(DT_ID , ',') WITHIN GROUP (ORDER BY DT_ID DESC)DT_ID , LISTAGG(DT_CODE , ',') WITHIN GROUP ";
        //        //strQry += " (ORDER BY DT_CODE DESC)DT_CODE    FROM TBLDTCMAST   WHERE  DT_ID in ("+ oldDTCIDs + ") AND  DF_REPLACE_FLAG = 0 ";

        //        strQry = "SELECT \"DT_ID\" ,  ";
        //        strQry += " \"DT_CODE\" FROM \"TBLDTCMAST\",\"TBLDTCFAILURE\"    WHERE  \"DT_ID\" in (" + oldDTCIDs + ") AND \"DF_DTC_CODE\" = \"DT_CODE\" AND  \"DF_REPLACE_FLAG\" = 0 ";

        //        dt = ObjCon.FetchDataTable(strQry);
        //        if (dt.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                lstfailedDTC.Add(Convert.ToString(dt.Rows[i]["DT_CODE"]));
        //                failedDTC = failedDTC + Convert.ToString(dt.Rows[i]["DT_CODE"]) + ",";
        //            }
        //            //string[] str =  Convert.ToString(dt.Rows[0]["DT_CODE"]).Split(',');
        //            //lstfailedDTC.Add(str);
        //            //failedDTC = Convert.ToString( dt.Rows[0]["DT_CODE"]);

        //            if (failedDTC.Length > 0)
        //            {
        //                failedDTC = failedDTC.Substring(0, failedDTC.Length - 1);
        //            }
        //            Arr[0] = failedDTC + " DTCs has been declared failure .Please complete the Transaction  ";
        //            Arr[1] = "0";
        //            Arr[2] = "";
        //            //       return Arr;

        //        }
        //        #endregion

        //        #region check for DTC Exists if exists than add it to  the list "lstDuplicateDTC" 

        //        strQry = "SELECT  \"DT_CODE\"  FROM \"TBLDTCMAST\" WHERE \"DT_CODE\" IN (" + newDTCCodes + ")";
        //        dt = ObjCon.FetchDataTable(strQry);

        //        if (dt.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                lstDuplicateDTC.Add(Convert.ToString(dt.Rows[i]["DT_CODE"]));
        //                duplicateDTC = duplicateDTC + Convert.ToString(dt.Rows[i]["DT_CODE"]) + ",";
        //            }
        //            if (duplicateDTC.Length > 0)
        //            {
        //                duplicateDTC = duplicateDTC.Substring(0, duplicateDTC.Length - 1);
        //            }
        //            //add duplicate and failed DTC to the single list 
        //            lstDuplicateDTC.AddRange(lstfailedDTC);
        //            Arr[0] = duplicateDTC + "DTCs Code already exists";
        //            Arr[1] = "0";
        //            Arr[2] = "";
        //            // return Arr;
        //        }
        //        #endregion

        //        if (through == "SECTION OFFICER")
        //        {
        //            #region Section Officer 
        //            // insert into the TBLFEEDERBIFURCATION 
        //            description = "Feeder Bifurcation ";
        //            string feederBifurcationID = ObjCon.get_value("SELECT COALESCE(max(\"FBS_ID\"), 0) + 1 FROM \"TBLFEEDERBIFURCATION_SO\"");
        //            ObjCon.BeginTransaction();

        //            strQry = "INSERT into \"TBLFEEDERBIFURCATION_SO\" (\"FBS_ID\" ,  \"FBS_CRON\",\"FBS_SECTION_CODE\", \"FBS_CR_BY\",\"FBS_OM_DATE\", \"FBS_DESC\",\"FBS_CLIENTIP\", \"FBS_OLD_FEEDER_CODE\" ,\"FBS_NEW_FEEDER_CODE\" , \"FBS_STATUS\"     )" +
        //                "VALUES (" + feederBifurcationID + " , now() , '" + objDTCMast.sOfficeCode + "' ," + scrby + ", '" + objDTCMast.sDate + "','" + description + " " +
        //                " " + sFeederCodes + " Feeder Code To " + Convert.ToString(lst[0]).Split('~')[4] + " ' ,'" + clientIP + "' ,  '" + Convert.ToString(lst[0]).Split('~')[3] + "' ,  '" + Convert.ToString(lst[0]).Split('~')[4] + "' ,  0 )";
        //            ObjCon.ExecuteQry(strQry);


        //            for (int i = 0; i < lst.Count; i++)
        //            {

        //                // 0-DTCID  // 1-OldDTCCode  // 2-sTCCode  //  3-OldFeederCode  //  4-NewFeederCode  // 5- NewDTCCode
        //                string sDTCId = Convert.ToString(lst[i]).Split('~')[0];
        //                string sOldDTCCode = Convert.ToString(lst[i]).Split('~')[1];
        //                string sDTRCode = Convert.ToString(lst[i]).Split('~')[2];
        //                string sOldFeederCode = Convert.ToString(lst[i]).Split('~')[3];
        //                string sNewFeederCode = Convert.ToString(lst[i]).Split('~')[4];
        //                string sNewDTCCode = Convert.ToString(lst[i]).Split('~')[5];

        //                if (!(lstDuplicateDTC.Contains(sOldDTCCode) || lstfailedDTC.Contains(sOldDTCCode)) && !(lstDuplicateDTC.Contains(sNewDTCCode) || lstfailedDTC.Contains(sNewDTCCode)))
        //                {
        //                    try
        //                    {
        //                        strQry = "INSERT INTO \"TBLFEEDER_BFCN_DETAILS_SO\" (\"FBDS_ID\",\"FBDS_FB_ID\",\"FBDS_OLD_DTC_CODE\",\"FBDS_DTR_CODE\",\"FBDS_NEW_DTC_CODE\",\"FBDS_CRON\",\"FBDS_CR_BY\"," +
        //                            "\"FBDS_OLD_FEEDER_CODE\",\"FBDS_NEW_FEEDER_CODE\" , \"FBDS_OLD_DTC_ID\" ,\"FBDS_STATUS\") " +
        //                       " VALUES ((SELECT COALESCE( max(\"FBDS_ID\" ),0)+1 FROM \"TBLFEEDER_BFCN_DETAILS_SO\")," + feederBifurcationID + ",'" + sOldDTCCode + "','" + sDTRCode + "','" + sNewDTCCode + "',now()," + scrby + " , '" + sOldFeederCode + "' ,'" + sNewFeederCode + "' , '" + Convert.ToString(lst[i]).Split('~')[0] + "' ,  0)";
        //                        ObjCon.ExecuteQry(strQry);
        //                    }
        //                    catch (Exception x)
        //                    {
        //                        clsException.LogError(x.StackTrace, x.Message, strFormCode, "UPDATINGFEEDERBIFURCATION");
        //                    }
        //                }

        //            }

        //            strQry = " SELECT * FROM \"TBLFEEDER_BFCN_DETAILS_SO\" WHERE \"FBDS_FB_ID\" = '" + feederBifurcationID + "'  ";
        //            if (ObjCon.get_value(strQry) == "")
        //            {
        //                strQry = " DELETE FROM \"TBLFEEDERBIFURCATION_SO\" WHERE \"FBS_ID\" = '" + feederBifurcationID + "'  ";
        //                ObjCon.ExecuteQry(strQry);

        //            }

        //            if (!(lstDuplicateDTC.Count == 0 && lstfailedDTC.Count == 0))
        //            {
        //                Arr[0] = "Some DTC has been decalred Failure or Duplicate exists";
        //                Arr[1] = "0";
        //                if (feederBifurcationID.Length > 0)
        //                {
        //                    Arr[2] = feederBifurcationID;
        //                }
        //                else
        //                {
        //                    Arr[2] = "";
        //                }

        //            }
        //            else
        //            {

        //                Arr[0] = "Details have been updated Successfully";
        //                Arr[1] = "1";
        //                Arr[2] = feederBifurcationID;
        //            }

        //            #endregion

        //        }
        //        else
        //        {

        //            #region check in both the lists and update  the DTC Code along with Feeder Code and save in the Tables .

        //            // insert into the TBLFEEDERBIFURCATION 
        //            description = "Feeder Bifurcation ";
        //            string feederBifurcationID = ObjCon.get_value("SELECT COALESCE(max(\"FB_ID\"), 0) + 1 FROM \"TBLFEEDERBIFURCATION\"");
        //            ObjCon.BeginTransaction();

        //            strQry = "INSERT into \"TBLFEEDERBIFURCATION\" (\"FB_ID\" , \"FB_CRON\" ,\"FB_OM_DATE\", \"FB_CR_BY\" , \"FB_DESC\",\"FB_CLIENTIP\"  ) VALUES (" + feederBifurcationID + " , now() ,TO_DATE('" + objDTCMast.sDate + "','dd/MM/yyyy') ," + scrby + " ,'" + description + " " + sFeederCodes + " Feeder Code To " + Convert.ToString(lst[0]).Split('~')[4] + " ' ,'" + clientIP + "')";
        //            ObjCon.ExecuteQry(strQry);

        //            for (int i = 0; i < lst.Count; i++)
        //            {

        //                // 0-DTCID  // 1-OldDTCCode  // 2-sTCCode  //  3-OldFeederCode  //  4-NewFeederCode  // 5- NewDTCCode
        //                string sDTCId = Convert.ToString(lst[i]).Split('~')[0];
        //                string sOldDTCCode = Convert.ToString(lst[i]).Split('~')[1];
        //                string sDTRCode = Convert.ToString(lst[i]).Split('~')[2];
        //                string sOldFeederCode = Convert.ToString(lst[i]).Split('~')[3];
        //                string sNewFeederCode = Convert.ToString(lst[i]).Split('~')[4];
        //                string sNewDTCCode = Convert.ToString(lst[i]).Split('~')[5];

        //                if (!(lstDuplicateDTC.Contains(sOldDTCCode) || lstfailedDTC.Contains(sOldDTCCode)) && !(lstDuplicateDTC.Contains(sNewDTCCode) || lstfailedDTC.Contains(sNewDTCCode)))
        //                {
        //                    try
        //                    {
        //                        strQry = "INSERT INTO \"TBLFEEDER_BIFURCATION_DETAILS\" (\"FBD_ID\",\"FBD_FB_ID\",\"FBD_OLD_DTC_CODE\",\"FBD_DTR_CODE\",\"FBD_NEW_DTC_CODE\",\"FBD_CRON\",\"FBD_CR_BY\",\"FBD_OLD_FEEDER_CODE\",\"FBD_NEW_FEEDER_CODE\") ";
        //                        strQry += " VALUES ((SELECT COALESCE( max(\"FBD_ID\" ),0)+1 FROM \"TBLFEEDER_BIFURCATION_DETAILS\")," + feederBifurcationID + ",'" + sOldDTCCode + "','" + sDTRCode + "','" + sNewDTCCode + "',now()," + scrby + " , '" + sOldFeederCode + "' ,'" + sNewFeederCode + "')";
        //                        ObjCon.ExecuteQry(strQry);

        //                        strQry = "UPDATE \"TBLDTCMAST\" SET \"DT_CODE\" = '" + sNewDTCCode + "' , \"DT_FDRSLNO\" = '" + sNewFeederCode + "',\"DT_OLD_DTC_CODE\"='"+ sOldDTCCode + "'  WHERE \"DT_ID\" = " + sDTCId + "";
        //                        ObjCon.ExecuteQry(strQry);


        //                        strQry = "UPDATE \"TBLTRANSDTCMAPPING\" SET \"TM_DTC_ID\" = '" + sNewDTCCode + "' WHERE \"TM_DTC_ID\" = '" + sOldDTCCode + "' and \"TM_LIVE_FLAG\" = 1";
        //                        ObjCon.ExecuteQry(strQry);


        //                        //strQry = "UPDATE \"TBLDTCENUMERATION\" SET \"DTE_DTC_OLDCODE\" = '" + sOldDTCCode + "'   WHERE \"DTE_DTCCODE\" = " + sNewDTCCode + "";
        //                        //ObjCon.ExecuteQry(strQry);


        //                        strQry = " UPDATE \"TBLFEEDER_BFCN_DETAILS_SO\" set \"FBDS_STATUS\" = 2  WHERE \"FBDS_OLD_DTC_CODE\" = '" + sOldDTCCode + "' ";
        //                        ObjCon.ExecuteQry(strQry);


        //                        strQry = "INSERT into \"TBLDTCTRANSACTION\" (\"DCT_ID\" , \"DCT_DTC_CODE\" , \"DCT_DTR_STATUS\",  \"DCT_DTR_CODE\"  , \"DCT_TRANS_DATE\"   , \"DCT_DESC\" ,\"DCT_ENTRYDATE\" , \"DCT_CANCEL_FLAG\"  )";
        //                        strQry += " VALUES ((SELECT COALESCE( max(\"DCT_ID\"),0)+1 FROM \"TBLDTCTRANSACTION\"),'" + sOldDTCCode + "' ,1 ,'" + sDTRCode + "' ,now() ,' " + description + " OLD DTC CODE : " + sOldDTCCode + " ; NEW DTC CODE : " + sNewDTCCode + " ', now() , '0' )";
        //                        ObjCon.ExecuteQry(strQry);

        //                        strQry = "SELECT \"FBDS_FB_ID\"  FROM \"TBLFEEDER_BFCN_DETAILS_SO\"  WHERE \"FBDS_OLD_DTC_CODE\"  ='" + sOldDTCCode + "' ";
        //                        fbs_id = ObjCon.get_value(strQry);

        //                        strQry = "SELECT \"FBDS_ID\"  FROM \"TBLFEEDER_BFCN_DETAILS_SO\" WHERE \"FBDS_FB_ID\" in   ( '" + fbs_id + "' ) and  \"FBDS_STATUS\" = 1 ";
        //                        if (ObjCon.FetchDataTable(strQry).Rows.Count == 0)
        //                        {
        //                            strQry = " UPDATE \"TBLFEEDER_BFCN_DETAILS_SO\" set \"FBDS_STATUS\" = 2  WHERE \"FBDS_FB_ID\" = '" + fbs_id + "'  ";
        //                            ObjCon.ExecuteQry(strQry);

        //                            strQry = " UPDATE \"TBLFEEDERBIFURCATION_SO\"  set \"FBS_STATUS\"  = 2  WHERE \"FBS_ID\"  = '" + fbs_id + "' ";
        //                            ObjCon.ExecuteQry(strQry);
        //                        }

        //                    }
        //                    catch (Exception x)
        //                    {
        //                        clsException.LogError(x.StackTrace, x.Message, strFormCode, "btnbifurcate_click_inside");
        //                    }
        //                }

        //            }

        //            if (!(lstDuplicateDTC.Count == 0 && lstfailedDTC.Count == 0))
        //            {
        //                Arr[0] = "Some DTC has been decalred Failure or Duplicate exists";
        //                Arr[1] = "0";
        //                if (feederBifurcationID.Length > 0)
        //                {
        //                    Arr[2] = feederBifurcationID;
        //                }
        //                else
        //                {
        //                    Arr[2] = "";
        //                }

        //            }

        //            #endregion
        //            else
        //            {

        //                Arr[0] = "Details have been updated Successfully";
        //                Arr[1] = "1";
        //                Arr[2] = feederBifurcationID;
        //            }


        //        }
        //        // return Arr;
        //        ObjCon.CommitTransaction();
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjCon.RollBackTrans();
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "btnbifurcate_click");
        //        Arr[0] = "Exception Occurred";
        //        Arr[1] = "-1";
        //        Arr[2] = "";
        //    }
        //    return new Tuple<string[], List<string>>(Arr, lstDuplicateDTC);
        //}
        #endregion
    }
}

