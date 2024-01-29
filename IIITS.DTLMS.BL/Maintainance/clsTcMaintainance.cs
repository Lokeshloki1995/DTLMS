using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data;
using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;
using IIITS.DTLMS.BL.DataBase;

namespace IIITS.DTLMS.BL
{
    public class clsTcMaintainance
    {
        string strFormCode = "clsTcMaintainance";

        PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
        DataBseConnection ObjBseCon = new DataBseConnection(Constants.Password);
        NpgsqlCommand NpgsqlCommand;

        public string sMaintainanceId { get; set; }
        public string sMaintainanceDetailsId { get; set; }
        public string sTcCode { get; set; }
        public string sDTCCode { get; set; }
        public string sTmDate { get; set; }
        public string sDescription { get; set; }
        public string sDTCName { get; set; }
        public string sRadiator { get; set; }
        public string sCrBy { get; set; }
        public string sCrOn { get; set; }
        public string sFeederId { get; set; }
        public string sOfficeCode { get; set; }
        //New
        public string sSupports { get; set; }
        public string sBreather { get; set; }
        public string sEarthing { get; set; }
        public string sDangerPlate { get; set; }
        public string sAntiClimbing { get; set; }
        public string sExplosion { get; set; }
        public string sConditionNuts { get; set; }
        public string sLTWsitch { get; set; }
        public string sArrestor { get; set; }
        public string sGOSwitches { get; set; }
        public string sConnections { get; set; }
        public string sFuses { get; set; }
        public string sOilLeakage { get; set; }
        public string sBushing { get; set; }
        public string sArcing { get; set; }
        public string sMaintainType { get; set; }
        public string sMaintainDate { get; set; }
        public string sMaintainBy { get; set; }
        public string sVoltage { get; set; }
        public string sLoadBalancing { get; set; }
        public string sEarthTesting { get; set; }

        /// <summary>
        /// Save/Update Tc Maintainance
        /// </summary>
        /// <param name="objTcMaintainance"></param>
        /// <returns></returns>
        public string[] SaveUpdateTcMaintainance(clsTcMaintainance objTcMaintainance)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string strQry = string.Empty;
            string DT_CODE = string.Empty;
            string QryKey = string.Empty;
            string[] Arr = new string[3];
            try
            {
                #region old inline queary
                ////ObjCon.BeginTransaction();
                //NpgsqlCommand.Parameters.AddWithValue("DTCCode", objTcMaintainance.sDTCCode);
                //DT_CODE = ObjCon.get_value("SELECT \"DT_CODE\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\" =:DTCCode", NpgsqlCommand);
                #endregion

                QryKey = "GET_DT_CODE";
                NpgsqlCommand cmd_TBLDTCMAST = new NpgsqlCommand("proc_getvalue_for_clstcmaintainance");
                cmd_TBLDTCMAST.Parameters.AddWithValue("p_key", QryKey);
                cmd_TBLDTCMAST.Parameters.AddWithValue("p_value_1", Convert.ToString(objTcMaintainance.sDTCCode ?? ""));
                cmd_TBLDTCMAST.Parameters.AddWithValue("p_value_2", "");
                DT_CODE = ObjBseCon.StringGetValue(cmd_TBLDTCMAST);

                if ((DT_CODE ?? "").Length == 0)
                {
                    Arr[0] = "Enter Valid DTC Code";
                    Arr[1] = "2";
                    return Arr;
                }
                if (objTcMaintainance.sMaintainanceId == "")
                {

                    //Check Maintainance Entry
                    bool bResult = CheckMaintainanceEntry(objTcMaintainance);
                    if (bResult == false)
                    {
                        if (objTcMaintainance.sMaintainType == "1")
                        {
                            Arr[0] = "Maintainance has been done for DTC Code " + objTcMaintainance.sDTCCode + " Quarterly";
                            Arr[1] = "2";
                            return Arr;
                        }
                        if (objTcMaintainance.sMaintainType == "2")
                        {
                            Arr[0] = "Maintainance has been done for DTC Code " + objTcMaintainance.sDTCCode + " Half Yearly";
                            Arr[1] = "2";
                            return Arr;
                        }

                    }

                    string strDate = string.Empty;
                    string sMaintType = objTcMaintainance.sMaintainType;
                    if (sMaintType == "1")
                    {
                        string strtime = objTcMaintainance.sMaintainDate;
                        DateTime dtManufacturingDate = DateTime.ParseExact(strtime, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        DateTime dt = dtManufacturingDate.AddMonths(3);
                        strDate = dt.ToString("dd/MM/yyyy");
                        Arr[0] = "Maintenance Details Saved Successfully and Next Maintenance Date will be " + strDate;
                    }
                    if (sMaintType == "2")
                    {
                        string strtime = objTcMaintainance.sMaintainDate;
                        DateTime dtManufacturingDate = DateTime.ParseExact(strtime, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        DateTime dt = dtManufacturingDate.AddMonths(6);
                        strDate = dt.ToString("dd/MM/yyyy");
                        Arr[0] = "Maintenance Details Saved Successfully and Next Maintenance Date will be " + strDate;
                    }

                    if (objTcMaintainance.sMaintainanceDetailsId == "")
                        objTcMaintainance.sMaintainanceDetailsId = "0";

                    if (objTcMaintainance.sMaintainanceId == "")
                        objTcMaintainance.sMaintainanceId = "0";
                    string strTeeest = DateTime.Now.ToString("dd/MM/yyyy");
                    NpgsqlCommand cmd = new NpgsqlCommand("sp_save_tcmaintain");
                    cmd.Parameters.AddWithValue("tm_id", objTcMaintainance.sMaintainanceId);
                    cmd.Parameters.AddWithValue("tm_dt_code", objTcMaintainance.sDTCCode);
                    cmd.Parameters.AddWithValue("tm_tc_code", objTcMaintainance.sTcCode);
                    cmd.Parameters.AddWithValue("tm_date", objTcMaintainance.sMaintainDate);
                    cmd.Parameters.AddWithValue("tm_maintain_type", objTcMaintainance.sMaintainType);
                    cmd.Parameters.AddWithValue("tm_maintain_by", objTcMaintainance.sMaintainBy);
                    cmd.Parameters.AddWithValue("tm_crby", objTcMaintainance.sCrBy);
                    cmd.Parameters.AddWithValue("tm_cron", DateTime.Now.ToString("dd/MM/yyyy"));
                    cmd.Parameters.AddWithValue("dmd_id", objTcMaintainance.sMaintainanceDetailsId);
                    cmd.Parameters.AddWithValue("dmd_tm_id", objTcMaintainance.sMaintainanceId);
                    cmd.Parameters.AddWithValue("dmd_supports", objTcMaintainance.sSupports);
                    cmd.Parameters.AddWithValue("dmd_connections", objTcMaintainance.sConnections);
                    cmd.Parameters.AddWithValue("dmd_fuses", objTcMaintainance.sFuses);
                    cmd.Parameters.AddWithValue("dmd_oil", objTcMaintainance.sOilLeakage);
                    cmd.Parameters.AddWithValue("dmd_bushing", objTcMaintainance.sBushing);
                    cmd.Parameters.AddWithValue("dmd_arcing", objTcMaintainance.sArcing);
                    cmd.Parameters.AddWithValue("dmd_breather", objTcMaintainance.sBreather);
                    cmd.Parameters.AddWithValue("dmd_earthing", objTcMaintainance.sEarthing);
                    cmd.Parameters.AddWithValue("dmd_danger", objTcMaintainance.sDangerPlate);
                    cmd.Parameters.AddWithValue("dmd_anti_climb", objTcMaintainance.sAntiClimbing);
                    cmd.Parameters.AddWithValue("dmd_explosion", objTcMaintainance.sExplosion);
                    cmd.Parameters.AddWithValue("dmd_condition_nuts", objTcMaintainance.sConditionNuts);
                    cmd.Parameters.AddWithValue("dmd_lt_switch", objTcMaintainance.sLTWsitch);
                    cmd.Parameters.AddWithValue("dmd_light_arrester", objTcMaintainance.sArrestor);
                    cmd.Parameters.AddWithValue("dmd_go_switches", objTcMaintainance.sGOSwitches);
                    cmd.Parameters.AddWithValue("dmd_votage", objTcMaintainance.sVoltage);
                    cmd.Parameters.AddWithValue("dmd_load_balance", objTcMaintainance.sLoadBalancing);
                    cmd.Parameters.AddWithValue("dmd_earth_testing", objTcMaintainance.sEarthTesting);
                    cmd.Parameters.AddWithValue("dt_last_service_date", strDate);
                    cmd.Parameters.AddWithValue("dt_code", objTcMaintainance.sDTCCode);
                    cmd.Parameters.Add("pk_id", NpgsqlDbType.Text);
                    cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd.Parameters["pk_id"].Direction = ParameterDirection.Output;
                    cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                    cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                    Arr[2] = "pk_id";
                    Arr[1] = "op_id";
                    Arr[0] = "msg";
                    Arr = ObjCon.Execute(cmd, Arr, 3);
                    //ObjCon.CommitTransaction();
                    if (sMaintType == "1")
                    {
                        Arr[0] = "Maintenance Details Saved Successfully and Next Maintenance Date will be " + strDate;
                    }
                    if (sMaintType == "2")
                    {
                        Arr[0] = "Maintenance Details Saved Successfully and Next Maintenance Date will be " + strDate;
                    }
                    Arr[1] = "0";
                    return Arr;
                }
                else
                {
                    NpgsqlCommand cmd = new NpgsqlCommand("sp_update_TCmaintain");
                    cmd.Parameters.AddWithValue("tm_maintain_by", objTcMaintainance.sMaintainBy);
                    cmd.Parameters.AddWithValue("tm_date", objTcMaintainance.sMaintainDate);
                    cmd.Parameters.AddWithValue("tm_id", objTcMaintainance.sMaintainanceId);
                    cmd.Parameters.AddWithValue("dmd_supports", objTcMaintainance.sSupports);
                    cmd.Parameters.AddWithValue("dmd_connections", objTcMaintainance.sConnections);
                    cmd.Parameters.AddWithValue("dmd_fuses", objTcMaintainance.sFuses);
                    cmd.Parameters.AddWithValue("dmd_oil", objTcMaintainance.sOilLeakage);
                    cmd.Parameters.AddWithValue("dmd_bushing", objTcMaintainance.sBushing);
                    cmd.Parameters.AddWithValue("dmd_arcing", objTcMaintainance.sArcing);
                    cmd.Parameters.AddWithValue("dmd_breather", objTcMaintainance.sBreather);
                    cmd.Parameters.AddWithValue("dmd_earthing", objTcMaintainance.sEarthing);
                    cmd.Parameters.AddWithValue("dmd_danger", objTcMaintainance.sDangerPlate);
                    cmd.Parameters.AddWithValue("dmd_anti_climb", objTcMaintainance.sAntiClimbing);
                    cmd.Parameters.AddWithValue("dmd_explosion", objTcMaintainance.sExplosion);
                    cmd.Parameters.AddWithValue("dmd_condition_nuts", objTcMaintainance.sConditionNuts);
                    cmd.Parameters.AddWithValue("dmd_lt_switch", objTcMaintainance.sLTWsitch);
                    cmd.Parameters.AddWithValue("dmd_light_arrester", objTcMaintainance.sArrestor);
                    cmd.Parameters.AddWithValue("dmd_go_switches", objTcMaintainance.sGOSwitches);
                    cmd.Parameters.AddWithValue("dmd_votage", objTcMaintainance.sVoltage);
                    cmd.Parameters.AddWithValue("dmd_load_balance", objTcMaintainance.sLoadBalancing);
                    cmd.Parameters.AddWithValue("dmd_earth_testing", objTcMaintainance.sEarthTesting);
                    cmd.Parameters.AddWithValue("dmd_id", objTcMaintainance.sMaintainanceDetailsId);
                    cmd.Parameters.Add("pk_id", NpgsqlDbType.Text);
                    cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd.Parameters["pk_id"].Direction = ParameterDirection.Output;
                    cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                    cmd.Parameters["msg"].Direction = ParameterDirection.Output;

                    Arr[2] = "pk_id";
                    Arr[1] = "op_id";
                    Arr[0] = "msg";
                    Arr = ObjCon.Execute(cmd, Arr, 3);

                    string strDate = string.Empty;
                    string sMaintType = objTcMaintainance.sMaintainType;

                    if (objTcMaintainance.sMaintainDate.Contains("-"))
                    {
                        objTcMaintainance.sMaintainDate = objTcMaintainance.sMaintainDate.Replace("-", "/");
                    }

                    if (sMaintType == "1")
                    {
                        string strtime = objTcMaintainance.sMaintainDate;

                        DateTime dtManufacturingDate = DateTime.ParseExact(strtime, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        DateTime dt = dtManufacturingDate.AddMonths(3);
                        strDate = dt.ToString("dd/MM/yyyy");
                        Arr[0] = "Maintenance Details Updated Successfully and Next Maintenance Date will be " + strDate;
                    }
                    if (sMaintType == "2")
                    {
                        string strtime = objTcMaintainance.sMaintainDate;
                        DateTime dtManufacturingDate = DateTime.ParseExact(strtime, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        DateTime dt = dtManufacturingDate.AddMonths(6);
                        strDate = dt.ToString("dd/MM/yyyy");
                        Arr[0] = "Maintenance Details Updated Successfully and Next Maintenance Date will be " + strDate;
                    }

                    Arr[1] = "1";
                    return Arr;
                }
            }

            catch (Exception ex)
            {
                ObjCon.RollBackTrans();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }
        /// <summary>
        /// Load DTC Maintainance
        /// </summary>
        /// <param name="sOfficeCode"></param>
        /// <returns></returns>
        public DataTable LoadDTCMaintainance(string sOfficeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                #region Old Query
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                //strQry = "SELECT \"TM_ID\",\"TM_TC_CODE\",\"TM_DT_CODE\",CASE \"TM_MAINTAIN_TYPE\" WHEN 1 THEN 'QUARTERLY' WHEN 2 THEN 'HALF YEARLY' END \"TM_MAINTAIN_TYPE\",";
                //strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_ID\"=\"TM_MAINTAIN_BY\") AS  \"TM_MAINTAIN_BY\", \"DT_NAME\" AS \"DTCNAME\",TO_CHAR(\"TM_DATE\",'DD-MON-YYYY') \"TM_DATE\" FROM \"TBLDTCMAINTENANCE\" M,\"TBLDTCMAST\"";
                //strQry += " WHERE M.CTID IN (SELECT MAX(CTID) FROM \"TBLDTCMAINTENANCE\" GROUP BY \"TM_DT_CODE\", \"TM_TC_CODE\" ) AND ";
                //strQry += " \"DT_CODE\" = \"TM_DT_CODE\" AND \"TM_MAINTAIN_TYPE\"=1 AND CAST(\"DT_OM_SLNO\" AS TEXT) LIKE :OfficeCode||'%'";
                //dt = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_dtc_maintainance");
                cmd.Parameters.AddWithValue("p_officecode", (sOfficeCode ?? ""));
                dt = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
            return dt;
        }
        /// <summary>
        /// Get Maintainace Details
        /// </summary>
        /// <param name="objTcMaintainance"></param>
        /// <returns></returns>
        public object GetMaintainaceDetails(clsTcMaintainance objTcMaintainance)
        {
            DataTable dt = new DataTable();
            try
            {
                #region Old Query
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("MaintainanceId", Convert.ToInt32(objTcMaintainance.sMaintainanceId));
                //strQry = "SELECT * FROM \"TBLDTCMAINTENANCE\",\"TBLDTCMAINTAINDETAILS\" WHERE \"TM_ID\"=\"DMD_TM_ID\" AND \"TM_ID\" =:MaintainanceId";
                //dt = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_maintainace_details");
                cmd.Parameters.AddWithValue("p_maintainanceid", (objTcMaintainance.sMaintainanceId ?? ""));
                dt = ObjCon.FetchDataTable(cmd);

                if (dt.Rows.Count > 0)
                {
                    objTcMaintainance.sMaintainanceId = Convert.ToString(dt.Rows[0]["TM_ID"]);
                    objTcMaintainance.sTcCode = Convert.ToString(dt.Rows[0]["TM_TC_CODE"]);
                    objTcMaintainance.sDTCCode = Convert.ToString(dt.Rows[0]["TM_DT_CODE"]);
                    objTcMaintainance.sTmDate = Convert.ToDateTime(dt.Rows[0]["TM_DATE"]).ToString("dd/MM/yyyy");
                    objTcMaintainance.sDescription = Convert.ToString(dt.Rows[0]["TM_DESC"]);
                    objTcMaintainance.sMaintainBy = Convert.ToString(dt.Rows[0]["TM_MAINTAIN_BY"]);
                    objTcMaintainance.sMaintainType = Convert.ToString(dt.Rows[0]["TM_MAINTAIN_TYPE"]);
                    objTcMaintainance.sSupports = Convert.ToString(dt.Rows[0]["DMD_SUPPORTS"]);
                    objTcMaintainance.sConditionNuts = Convert.ToString(dt.Rows[0]["DMD_CONDITION_NUTS"]);
                    objTcMaintainance.sConnections = Convert.ToString(dt.Rows[0]["DMD_CONNECTIONS"]);
                    objTcMaintainance.sFuses = Convert.ToString(dt.Rows[0]["DMD_FUSES"]);
                    objTcMaintainance.sBushing = Convert.ToString(dt.Rows[0]["DMD_BUSHING"]);
                    objTcMaintainance.sArcing = Convert.ToString(dt.Rows[0]["DMD_ARCING"]);
                    objTcMaintainance.sBreather = Convert.ToString(dt.Rows[0]["DMD_BREATHER"]);
                    objTcMaintainance.sEarthing = Convert.ToString(dt.Rows[0]["DMD_EARTHING"]);
                    objTcMaintainance.sDangerPlate = Convert.ToString(dt.Rows[0]["DMD_DANGER"]);
                    objTcMaintainance.sAntiClimbing = Convert.ToString(dt.Rows[0]["DMD_ANTI_CLIMB"]);
                    objTcMaintainance.sExplosion = Convert.ToString(dt.Rows[0]["DMD_EXPLOSION"]);
                    objTcMaintainance.sLTWsitch = Convert.ToString(dt.Rows[0]["DMD_LT_SWITCH"]);
                    objTcMaintainance.sArrestor = Convert.ToString(dt.Rows[0]["DMD_LIGHT_ARRESTER"]);
                    objTcMaintainance.sGOSwitches = Convert.ToString(dt.Rows[0]["DMD_GO_SWITCHES"]);
                    objTcMaintainance.sVoltage = Convert.ToString(dt.Rows[0]["DMD_VOTAGE"]);
                    objTcMaintainance.sLoadBalancing = Convert.ToString(dt.Rows[0]["DMD_LOAD_BALANCE"]);
                    objTcMaintainance.sEarthTesting = Convert.ToString(dt.Rows[0]["DMD_EARTH_TESTING"]);
                    objTcMaintainance.sOilLeakage = Convert.ToString(dt.Rows[0]["DMD_OIL"]);
                    objTcMaintainance.sMaintainanceDetailsId = Convert.ToString(dt.Rows[0]["DMD_ID"]);
                    objTcMaintainance.sCrOn = Convert.ToString(dt.Rows[0]["TM_CRON"]);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objTcMaintainance;
            }
            return objTcMaintainance;
        }
        /// <summary>
        /// Get TC Details
        /// </summary>
        /// <param name="objTcMaintainance"></param>
        /// <returns></returns>
        public string GetTCDetails(clsTcMaintainance objTcMaintainance)
        {
            string StringValue = string.Empty;
            DataTable dtSearchDetails = new DataTable();
            try
            {
                #region Old Query
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("DTCCode", objTcMaintainance.sDTCCode);
                //strQry = "SELECT \"TC_SLNO\"  || '~' || \"TC_CODE\" || '~' || TO_CHAR(\"DT_LAST_SERVICE_DATE\",'DD/MM/YYYY') AS \"DT_LAST_SERVICE_DATE\" ";
                //strQry += "FROM \"TBLDTCMAST\", \"TBLTCMASTER\"  WHERE \"DT_TC_ID\"=\"TC_CODE\" AND CAST(\"DT_CODE\" AS TEXT)=:DTCCode";
                //StringValue = ObjCon.get_value(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_tc_details_by_dtccode");
                cmd.Parameters.AddWithValue("p_dtccode", (objTcMaintainance.sDTCCode ?? ""));
                StringValue = ObjBseCon.StringGetValue(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
            return StringValue;
        }
        /// <summary>
        /// Load Prev Maintainance
        /// </summary>
        /// <param name="objMaintain"></param>
        /// <returns></returns>
        public DataTable LoadPrevMaintainance(clsTcMaintainance objMaintain)
        {
            DataTable dt = new DataTable();
            try
            {
                #region Old Query
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("FeederId", objMaintain.sFeederId);
                //NpgsqlCommand.Parameters.AddWithValue("OfficeCode", objMaintain.sOfficeCode);
                //strQry = " SELECT \"DT_CODE\",\"DT_NAME\", CAST(\"TC_CAPACITY\" AS TEXT) \"CAPACITY\", TO_CHAR(\"DT_LAST_SERVICE_DATE\",'DD-MON-YYYY') AS  \"LAST_SERVICE_DATE\" ,";
                //strQry += " TO_CHAR((\"DT_LAST_SERVICE_DATE\" + INTERVAL '1 MONTH'),'DD-MON-YYYY') \"EXPECTED_SERVICEDATE\",\"TC_CODE\" FROM \"TBLDTCMAST\",\"TBLTCMASTER\",";
                //strQry += " \"TBLMAINTAINANCEPERIOD\" WHERE \"DT_TC_ID\" = \"TC_CODE\"  AND \"TC_CAPACITY\" BETWEEN \"MP_FROM\" AND \"MP_TO\" AND (CURRENT_DATE - \"DT_LAST_SERVICE_DATE\") / 30 > \"MP_PERIOD\" ";
                //strQry += " AND SUBSTR (\"DT_CODE\", 1, 4) =:FeederId";
                //strQry += " AND CAST(\"DT_OM_SLNO\" AS TEXT) LIKE :OfficeCode||'%'";
                //dt = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_prev_maintainance");
                cmd.Parameters.AddWithValue("p_feederid", (objMaintain.sFeederId ?? ""));
                cmd.Parameters.AddWithValue("p_officecode", (objMaintain.sOfficeCode ?? ""));
                dt = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
            return dt;
        }
        /// <summary>
        /// Get Dtc Basic Details
        /// </summary>
        /// <param name="objMaintain"></param>
        /// <param name="OfficeCode"></param>
        /// <returns></returns>
        public clsTcMaintainance GetDtcBasicDetails(clsTcMaintainance objMaintain, string OfficeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                #region Old Query
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("DTCCode", objMaintain.sDTCCode);
                //NpgsqlCommand.Parameters.AddWithValue("OfficeCode", OfficeCode);
                //strQry = "SELECT \"DT_TC_ID\",\"DT_CODE\",\"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=:DTCCode and CAST(\"DT_OM_SLNO\" AS TEXT) LIKE :OfficeCode||'%'";
                //dt = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_dtc_basic_details");
                cmd.Parameters.AddWithValue("p_dtccode", (objMaintain.sDTCCode ?? ""));
                cmd.Parameters.AddWithValue("p_officecode", (OfficeCode ?? ""));
                dt = ObjCon.FetchDataTable(cmd);

                if (dt.Rows.Count > 0)
                {
                    objMaintain.sTcCode = Convert.ToString(dt.Rows[0]["DT_TC_ID"]);
                    objMaintain.sDTCCode = Convert.ToString(dt.Rows[0]["DT_CODE"]);
                    objMaintain.sDTCName = Convert.ToString(dt.Rows[0]["DT_NAME"]);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objMaintain;
            }
            return objMaintain;
        }
        /// <summary>
        /// Load Dtc Maintain Grid
        /// </summary>
        /// <param name="sDTCCode"></param>
        /// <returns></returns>
        public DataTable LoadDtcMaintainGrid(string sDTCCode)
        {
            DataTable dt = new DataTable();
            try
            {
                #region Old Query
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("DTCCode", sDTCCode);
                //strQry = "select CAST(\"TM_TC_CODE\" AS TEXT)TM_TC_CODE,CAST(\"TM_DT_CODE\" AS TEXT)TM_DT_CODE,TO_CHAR(\"TM_DATE\",'dd/MM/yyyy')TM_DATE,";
                //strQry += "CASE \"TM_MAINTAIN_TYPE\" WHEN 1 THEN 'QUARTERLY' WHEN 2 THEN 'HALF YEARLY' END \"MD_NAME\",\"US_FULL_NAME\" ";
                //strQry += " FROM \"TBLDTCMAINTENANCE\",\"TBLUSER\" WHERE \"US_ID\"=\"TM_MAINTAIN_BY\" AND \"TM_DT_CODE\"=:DTCCode";
                //dt = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_dtc_maintain_grid");
                cmd.Parameters.AddWithValue("p_dtccode", (sDTCCode ?? ""));
                dt = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
            return dt;
        }
        /// <summary>
        /// Check Maintainance Entry
        /// </summary>
        /// <param name="objMaintain"></param>
        /// <returns></returns>
        public bool CheckMaintainanceEntry(clsTcMaintainance objMaintain)
        {
            string sResult = string.Empty;
            try
            {
                #region Old Query
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("DTCCode", objMaintain.sDTCCode);
                //NpgsqlCommand.Parameters.AddWithValue("MaintainType", objMaintain.sMaintainType);
                //strQry = "SELECT A.\"DATEDIFF\" FROM (SELECT FLOOR(CURRENT_DATE-\"DT_LAST_SERVICE_DATE\") \"DATEDIFF\" FROM ";
                //strQry += " \"TBLDTCMAINTENANCE\",\"TBLDTCMAST\" WHERE \"DT_CODE\"=\"TM_DT_CODE\" AND \"TM_DT_CODE\"=:DTCCode  AND ";
                //strQry += " CAST(\"TM_MAINTAIN_TYPE\" AS TEXT)=:MaintainType ORDER BY \"TM_ID\" DESC) A  limit 1";
                //sResult = ObjCon.get_value(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_check_maintainance_entry");
                cmd.Parameters.AddWithValue("p_dtccode", (objMaintain.sDTCCode ?? ""));
                cmd.Parameters.AddWithValue("p_maintaintype", (objMaintain.sMaintainType ?? ""));
                sResult = ObjBseCon.StringGetValue(cmd);

                if (sResult != "")
                {
                    //Quarterly
                    if (objMaintain.sMaintainType == "1")
                    {
                        if (Convert.ToInt32(sResult) > 90)
                        {
                            return true;
                        }
                    }
                    //Half Yearly
                    if (objMaintain.sMaintainType == "2")
                    {
                        if (Convert.ToInt32(sResult) > 180)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }
        /// <summary>
        /// Load QuarPending DTC Maintainance
        /// </summary>
        /// <param name="sOfficeCode"></param>
        /// <returns></returns>
        public DataTable LoadQuarPendingDTCMaintainance(string sOfficeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                #region Old Query
                //string strQry = string.Empty;
                //string sMainType = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "SELECT MIN(AA.\"TM_MAINTAIN_TYPE\") FROM (SELECT \"TM_MAINTAIN_TYPE\", DENSE_RANK() over(ORDER BY \"TM_DATE\") from \"TBLDTCMAINTENANCE\") AA";
                //sMainType = ObjCon.get_value(strQry);

                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("MainType", sMainType);
                //NpgsqlCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                //strQry = " SELECT \"DT_CODE\" , \"DT_NAME\", \"DT_TC_ID\", \"DT_OM_SLNO\",TO_CHAR(\"DT_LAST_SERVICE_DATE\",'dd/MM/yyyy') AS \"DT_LAST_SERVICE_DATE\",";
                //strQry += " 'QUARTERLY' AS \"TM_MAINTAIN_TYPE\" FROM (SELECT \"TM_DT_CODE\", MAX(\"TM_DATE\") \"LAST_SERVICE_DATE\",";
                //strQry += " :MainType AS  \"TM_MAINTAIN_TYPE\"  FROM \"TBLDTCMAINTENANCE\" GROUP BY \"TM_DT_CODE\")B ";
                //strQry += " INNER  JOIN (SELECT \"DT_CODE\", \"DT_NAME\", \"DT_TC_ID\", \"DT_OM_SLNO\", ";
                //strQry += " COALESCE(\"DT_LAST_SERVICE_DATE\",NOW() - INTERVAL '91 DAY') AS  \"DT_LAST_SERVICE_DATE\" FROM ";
                //strQry += " \"TBLDTCMAST\") A ON A.\"DT_CODE\" = B.\"TM_DT_CODE\" AND (\"DT_LAST_SERVICE_DATE\" - NOW() )  > CAST('90' AS INTERVAL) AND";
                //strQry += " CAST(\"DT_OM_SLNO\" AS TEXT) LIKE :OfficeCode||'%' AND (CAST(\"TM_MAINTAIN_TYPE\" AS TEXT) = '1' OR \"TM_MAINTAIN_TYPE\" IS NULL)";
                //dt = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_quar_pending_dtc_maintainance");
                cmd.Parameters.AddWithValue("p_officecode", (sOfficeCode ?? ""));
                dt = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
            return dt;
        }
        /// <summary>
        /// Load Half Yearly Pending DTC Maintainance
        /// </summary>
        /// <param name="sOfficeCode"></param>
        /// <returns></returns>
        public DataTable LoadHalfYearlyPendingDTCMaintainance(string sOfficeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                #region Old Query
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                //strQry = "   SELECT \"DT_CODE\" , \"DT_NAME\", \"DT_TC_ID\", \"DT_OM_SLNO\", TO_CHAR(\"DT_LAST_SERVICE_DATE\",'dd/MM/yyyy')DT_LAST_SERVICE_DATE, 'HALFYEARLY' AS \"TM_MAINTAIN_TYPE\" FROM ";
                //strQry += " (SELECT \"DT_CODE\", \"DT_NAME\", \"DT_TC_ID\", \"DT_OM_SLNO\", \"DT_LAST_SERVICE_DATE\" FROM \"TBLDTCMAST\") A,";
                //strQry += " (SELECT \"TM_DT_CODE\", MAX(\"TM_DATE\") LAST_SERVICE_DATE, MIN(\"TM_MAINTAIN_TYPE\")  ";
                //strQry += " TM_MAINTAIN_TYPE ";
                //strQry += " FROM \"TBLDTCMAINTENANCE\" GROUP BY \"TM_DT_CODE\")B WHERE A.\"DT_CODE\" = B.\"TM_DT_CODE\" ";
                //strQry += " AND CAST(\"DT_OM_SLNO\" AS TEXT) LIKE :OfficeCode||'%' ";
                //dt = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_halfyearly_pending_dtc_maintainance");
                cmd.Parameters.AddWithValue("p_officecode", (sOfficeCode ?? ""));
                dt = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
            return dt;
        }
        /// <summary>
        /// Load Half Year DTC Maintainance
        /// </summary>
        /// <param name="sOfficeCode"></param>
        /// <returns></returns>
        public DataTable LoadHalfYearDTCMaintainance(string sOfficeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                #region Old Query
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                //strQry = "SELECT \"TM_ID\",\"TM_TC_CODE\",\"TM_DT_CODE\",CASE \"TM_MAINTAIN_TYPE\" WHEN 1 THEN 'QUARTERLY' WHEN 2 THEN 'HALF YEARLY' END \"TM_MAINTAIN_TYPE\",";
                //strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_ID\"=\"TM_MAINTAIN_BY\") \"TM_MAINTAIN_BY\", \"DT_NAME\" AS \"DTCNAME\",TO_CHAR(\"TM_DATE\",'DD-MON-YYYY') TM_DATE FROM \"TBLDTCMAINTENANCE\" M,\"TBLDTCMAST\"";
                //strQry += " WHERE M.CTID IN (SELECT MAX(CTID) FROM \"TBLDTCMAINTENANCE\" GROUP BY \"TM_DT_CODE\", \"TM_TC_CODE\" ) AND ";
                //strQry += " \"DT_CODE\" = \"TM_DT_CODE\"  AND \"TM_MAINTAIN_TYPE\"=2 AND CAST(\"DT_OM_SLNO\" AS TEXT) LIKE :OfficeCode||'%'";
                //dt = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_halfyear_dtc_maintainance");
                cmd.Parameters.AddWithValue("p_officecode", (sOfficeCode ?? ""));
                dt = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
            return dt;
        }
    }
}

