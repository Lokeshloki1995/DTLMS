using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IIITS.PGSQL.DAL;
using System.Configuration;
using System.IO;
using IIITS.DTLMS.BL.DataBase;
using Npgsql;
using NpgsqlTypes;

namespace IIITS.DTLMS.BL
{
    public class clsDtcMissMatchEntry
    {
        string strFormCode = "clsDtcMissMatchEntry";
        PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
        DataBseConnection ObjBseCon = new DataBseConnection(Constants.Password);

        public string sDtcCode { get; set; }
        public string sDtrCode { get; set; }
        public string sNewDTCCode { get; set; }
        public string sOfficeCode { get; set; }
        public string sLocType { get; set; }
        public string sNewDtrCode { get; set; }
        public string sNewOfficeCode { get; set; }
        public string sNewLocType { get; set; }
        public string sRemarks { get; set; }
        public string sCrBy { get; set; }
        public string sOldDtrCode { get; set; }
        public string sStoreId { get; set; }
        public string sDtrStatus { get; set; }
        public string QryKey { get; set; }

        string validationstatus = ConfigurationManager.AppSettings["VaildationOfDtcDtr"].ToString();
        string validateCRforDTR = ConfigurationManager.AppSettings["ValidateCRCompletion"].ToString();

        /// <summary>
        /// for getting Dtc details
        /// </summary>
        /// <param name="objDtcMissEntry"></param>
        /// <returns></returns>
        public DataTable LoadDtcDetails(clsDtcMissMatchEntry objDtcMissEntry)
        {
            DataTable dt = new DataTable();

            try
            {
                #region Old Inline query
                //string strQry = string.Empty;
                //strQry = " SELECT \"DT_CODE\",\"DT_NAME\",\"DT_OM_SLNO\",\"DT_TC_ID\",\"TC_SLNO\",\"TC_CAPACITY\", ";
                //strQry += " \"TC_CURRENT_LOCATION\" FROM \"TBLDTCMAST\",\"TBLTCMASTER\" ";
                //strQry += " WHERE \"TC_CODE\"=\"DT_TC_ID\" AND \"DT_CODE\"= '" + objDtcMissEntry.sDtcCode + "' ";
                //dt = ObjCon.FetchDataTable(strQry);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_dtcdetails_for_clsdtcmissmatchentry");
                cmd.Parameters.AddWithValue("p_dtccode", Convert.ToString(objDtcMissEntry.sDtcCode ?? ""));
                dt = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError("clsDtcMissMatchEntry", "LoadDtcDetails", ex.Message, ex.StackTrace);
                return dt;
            }
            return dt;
        }
        /// <summary>
        /// for getting dtr details
        /// </summary>
        /// <param name="objDtcMissEntry"></param>
        /// <returns></returns>
        public DataTable LoadDtrDetails(clsDtcMissMatchEntry objDtcMissEntry)
        {
            DataTable dt = new DataTable();

            try
            {
                #region old inline query
                //string strQry = string.Empty;
                //strQry = "SELECT \"TC_CURRENT_LOCATION\" from \"TBLTCMASTER\" WHERE \"TC_CODE\"='" + objDtcMissEntry.sDtrCode + "'";
                //string sqry1 = ObjCon.get_value(strQry);

                //if (sqry1 != "1")
                //{

                //    strQry = "SELECT \"TC_CODE\", cast(\"TC_SLNO\" as text)\"TC_SLNO\",\"TC_CAPACITY\",\"TC_LOCATION_ID\",";
                //    strQry += "\"TC_CURRENT_LOCATION\", ";
                //    strQry += "CASE \"TC_CURRENT_LOCATION\" WHEN  1 THEN 'STORE' WHEN 2 THEN  'FIELD' WHEN 3 THEN 'REPAIRER' ";
                //    strQry += " WHEN 5 THEN 'BANK'   END AS \"CURRENTLOCATION\", ";
                //    strQry += "CASE \"TC_STATUS\" WHEN 1 THEN 'GOOD CONDITION' WHEN 2 THEN 'REPAIRED THEN GOOD' WHEN 3 THEN 'FAILED' ";
                //    strQry += "WHEN 11 THEN 'RELEASED GOOD' WHEN 6 THEN 'SCRAP' END AS \"STATUS\",(SELECT \"OFF_NAME\" ";
                //    strQry += "FROM \"VIEW_OFFICES\" WHERE \"OFF_CODE\"=\"TC_LOCATION_ID\")\"OFFNAME\"";
                //    strQry += " FROM \"TBLTCMASTER\" WHERE \"TC_CODE\"='" + objDtcMissEntry.sDtrCode + "'";
                //    dt = ObjCon.FetchDataTable(strQry);

                //}
                //else
                //{
                //    strQry = "SELECT \"TC_CODE\", cast(\"TC_SLNO\" as text)\"TC_SLNO\",\"TC_CAPACITY\",\"TC_LOCATION_ID\",";
                //    strQry += "\"TC_CURRENT_LOCATION\", ";
                //    strQry += "CASE \"TC_CURRENT_LOCATION\" WHEN  1 THEN 'STORE' WHEN 2 THEN  'FIELD' WHEN 3 THEN 'REPAIRER' ";
                //    strQry += " WHEN 5 THEN 'BANK'  END AS \"CURRENTLOCATION\", ";
                //    strQry += "CASE \"TC_STATUS\" WHEN 1 THEN 'GOOD CONDITION' WHEN 2 THEN 'REPAIRED THEN GOOD' ";
                //    strQry += "WHEN 3 THEN 'FAILED' ";
                //    strQry += "WHEN 11 THEN 'RELEASED GOOD' WHEN 6 THEN 'SCRAP' END AS \"STATUS\",(SELECT \"SM_NAME\"||'~'||'STORE' ";
                //    strQry += "FROM \"TBLSTOREMAST\" WHERE \"SM_ID\"=\"TC_LOCATION_ID\")\"OFFNAME\"";
                //    strQry += " FROM \"TBLTCMASTER\" WHERE \"TC_CODE\"='" + objDtcMissEntry.sDtrCode + "'";
                //    dt = ObjCon.FetchDataTable(strQry);
                //}
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_dtrdetails_for_clsdtcmissmatchentry");
                cmd.Parameters.AddWithValue("p_dtrcode", Convert.ToString(objDtcMissEntry.sDtrCode ?? ""));
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
        ///  getting dtr detils for moved to store
        /// </summary>
        /// <param name="objDtcMissEntry"></param>
        /// <returns></returns>
        public DataTable LoadDtrDetails1(clsDtcMissMatchEntry objDtcMissEntry)
        {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            string DFId = string.Empty;
            string strQry = string.Empty;
            try
            {
                #region old inline query
                //strQry = "SELECT \"TD_DF_ID\" FROM \"TBLTCDRAWN\" WHERE \"TD_TC_NO\"='" + objDtcMissEntry.sDtrCode + "'";
                //DFId = ObjCon.get_value(strQry);
                #endregion

                QryKey = "GET_TD_DF_ID";
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_dtrdetails1");
                cmd.Parameters.AddWithValue("p_key", QryKey);
                cmd.Parameters.AddWithValue("p_value_1", Convert.ToString(objDtcMissEntry.sDtrCode ?? ""));
                DFId = ObjBseCon.StringGetValue(cmd);

                if (DFId == null || DFId == "")
                {
                    #region old inline query
                    //strQry = "SELECT \"TC_CODE\",cast(\"TC_SLNO\" as text)\"TC_SLNO\",\"DT_CODE\",\"TC_CAPACITY\",";
                    //strQry += "'' AS \"DF_REPLACE_FLAG\",";
                    //strQry += "\"TC_UPDATED_EVENT\",\"TC_STATUS\",   ";
                    //strQry += "\"TC_LOCATION_ID\",\"TC_STORE_ID\",  CASE \"TC_CURRENT_LOCATION\" WHEN  1 THEN 'STORE' ";
                    //strQry += "WHEN 2 THEN  'FIELD' ";
                    //strQry += "WHEN 3 THEN 'REPAIRER' WHEN 5 THEN 'BANK'  END AS  ";
                    //strQry += " \"TC_CURRENT_LOCATION\", CASE \"TC_STATUS\" WHEN 1 THEN 'GOOD CONDITION' ";
                    //strQry += "WHEN 2 THEN 'REPAIRED THEN GOOD' WHEN 3 ";
                    //strQry += "THEN 'FAILED' ";
                    //strQry += " WHEN 11 THEN 'RELEASED GOOD' WHEN 6 THEN 'SCRAP'END AS \"STATUS\",(SELECT \"OFF_NAME\"";
                    //strQry += "FROM \"VIEW_OFFICES\" ";
                    //strQry += "WHERE \"OFF_CODE\"=\"TC_LOCATION_ID\")";
                    //strQry += "\"OFFNAME\" FROM \"TBLTCMASTER\",\"TBLDTCMAST\" WHERE \"TC_CODE\"=\"DT_TC_ID\" AND ";
                    //strQry += "\"TC_CODE\"='" + objDtcMissEntry.sDtrCode + "'";
                    //dt = ObjCon.FetchDataTable(strQry);
                    #endregion

                    NpgsqlCommand cmd_dfid_isnull = new NpgsqlCommand("proc_get_dtrdetails1_when_dfid_isnull");
                    cmd_dfid_isnull.Parameters.AddWithValue("p_dtrcode", Convert.ToString(objDtcMissEntry.sDtrCode ?? ""));
                    dt = ObjCon.FetchDataTable(cmd_dfid_isnull);

                    if (dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                    else
                    {
                        string sqry1 = string.Empty;

                        #region Old inline query
                        //strQry = " SELECT \"TC_CURRENT_LOCATION\" from \"TBLTCMASTER\" WHERE ";
                        //strQry += " \"TC_CODE\" = '" + objDtcMissEntry.sDtrCode + "' ";
                        //sqry1 = ObjCon.get_value(strQry);
                        #endregion

                        QryKey = "GET_TC_CURRENT_LOCATION";
                        NpgsqlCommand cmd_TBLTCMASTER = new NpgsqlCommand("fetch_getvalue_dtrdetails1");
                        cmd_TBLTCMASTER.Parameters.AddWithValue("p_key", QryKey);
                        cmd_TBLTCMASTER.Parameters.AddWithValue("p_value_1", Convert.ToString(objDtcMissEntry.sDtrCode ?? ""));
                        sqry1 = ObjBseCon.StringGetValue(cmd_TBLTCMASTER);


                        #region Old inline query
                        //if (sqry1 != "1")
                        //{
                        //    strQry = "SELECT \"TC_CODE\", cast(\"TC_SLNO\" as text)\"TC_SLNO\",'' AS \"DT_CODE\",";
                        //    strQry += "\"TC_CAPACITY\",";
                        //    strQry += "'' AS \"DF_REPLACE_FLAG\",";
                        //    strQry += "\"TC_UPDATED_EVENT\",";
                        //    strQry += "\"TC_LOCATION_ID\", CASE \"TC_CURRENT_LOCATION\" WHEN  1 THEN 'STORE' ";
                        //    strQry += "WHEN 2 THEN  'FIELD' ";
                        //    strQry += "WHEN 3 THEN 'REPAIRER' WHEN 5 THEN 'BANK' END AS ";
                        //    strQry += "\"TC_CURRENT_LOCATION\", CASE \"TC_STATUS\" WHEN 1 THEN 'GOOD CONDITION' ";
                        //    strQry += "WHEN 2 THEN 'REPAIRED THEN GOOD' WHEN 3 THEN 'FAILED' ";
                        //    strQry += " WHEN 11 THEN 'RELEASED GOOD' WHEN 6 THEN 'SCRAP' END AS \"STATUS\",";
                        //    strQry += "(SELECT \"OFF_NAME\" FROM \"VIEW_OFFICES\" WHERE ";
                        //    strQry += "\"OFF_CODE\"=\"TC_LOCATION_ID\")";
                        //    strQry += "\"OFFNAME\" FROM \"TBLTCMASTER\" WHERE \"TC_CODE\"='" + objDtcMissEntry.sDtrCode + "'";
                        //    dt = ObjCon.FetchDataTable(strQry);
                        //}
                        //else
                        //{
                        //    strQry = "SELECT \"TC_CODE\", cast(\"TC_SLNO\" as text)\"TC_SLNO\",'' AS \"DT_CODE\",\"TC_CAPACITY\",";
                        //    strQry += "'' AS \"DF_REPLACE_FLAG\",";
                        //    strQry += "\"TC_UPDATED_EVENT\",";
                        //    strQry += " \"TC_STATUS\",\"TC_LOCATION_ID\",\"TC_STORE_ID\",CASE \"TC_CURRENT_LOCATION\" ";
                        //    strQry += "WHEN  1 THEN 'STORE' WHEN 2 THEN  'FIELD' ";
                        //    strQry += "WHEN 3 THEN 'REPAIRER' WHEN 5 THEN 'BANK' ";
                        //    strQry += " END AS \"TC_CURRENT_LOCATION\" , CASE \"TC_STATUS\" WHEN 1 THEN 'GOOD CONDITION' ";
                        //    strQry += "WHEN 2 THEN 'REPAIRED THEN GOOD' WHEN 3 ";
                        //    strQry += "THEN 'FAILED' ";
                        //    strQry += " WHEN 11 THEN 'RELEASED GOOD' WHEN 6 THEN 'SCRAP'  END AS \"STATUS\",";
                        //    strQry += "(SELECT \"SM_NAME\"||'~'||'STORE' ";
                        //    strQry += "FROM \"TBLSTOREMAST\" WHERE \"SM_ID\"=\"TC_LOCATION_ID\")";
                        //    strQry += "\"OFFNAME\" FROM \"TBLTCMASTER\" WHERE \"TC_CODE\"='" + objDtcMissEntry.sDtrCode + "'";
                        //    dt = ObjCon.FetchDataTable(strQry);
                        //}
                        #endregion

                        NpgsqlCommand cmd_on_TC_CURRENT_LOCATION = new NpgsqlCommand("proc_get_dtrdetails1_on_tc_curr_loc");
                        cmd_on_TC_CURRENT_LOCATION.Parameters.AddWithValue("p_dtrcurr_loc", Convert.ToString(sqry1 ?? ""));
                        cmd_on_TC_CURRENT_LOCATION.Parameters.AddWithValue("p_dtrcode", Convert.ToString(objDtcMissEntry.sDtrCode ?? ""));
                        dt = ObjCon.FetchDataTable(cmd_on_TC_CURRENT_LOCATION);

                        return dt;
                    }
                }
                else
                {
                    #region Old inline query
                    //strQry = "SELECT cast(\"TC_SLNO\" as text)\"TC_SLNO\",\"TC_CAPACITY\",\"TC_LOCATION_ID\",\"DT_CODE\",";
                    //strQry += "\"TC_UPDATED_EVENT\",\"DF_REPLACE_FLAG\",";
                    //strQry += "CASE \"TC_CURRENT_LOCATION\" WHEN  ";
                    //strQry += " 1 THEN 'STORE' WHEN 2 THEN  'FIELD' WHEN 3 THEN 'REPAIRER' WHEN 5 THEN 'BANK' END AS \"TC_CURRENT_LOCATION\",";
                    //strQry += "CASE \"TC_STATUS\" WHEN 1 THEN ";
                    //strQry += "'GOOD CONDITION' WHEN 2 THEN ";
                    //strQry += "'REPAIRED THEN GOOD' WHEN 3 THEN 'FAILED' WHEN 11 THEN 'RELEASED GOOD' WHEN 6 THEN 'SCRAP'  ";
                    //strQry += "END AS \"STATUS\",\"OFF_NAME\" ";
                    //strQry += "as \"OFFNAME\"    FROM \"TBLDTCMAST\",\"TBLTCMASTER\",\"VIEW_OFFICES\",\"TBLDTCFAILURE\" ";
                    //strQry += " WHERE \"TC_CODE\"=\"DT_TC_ID\" AND \"TC_CODE\"='" + objDtcMissEntry.sDtrCode + "' AND ";
                    //strQry += "\"DT_CODE\"=\"DF_DTC_CODE\" ";
                    //strQry += "and \"TC_LOCATION_ID\"=\"OFF_CODE\"";
                    //dt = ObjCon.FetchDataTable(strQry);
                    #endregion

                    NpgsqlCommand cmd_dfid_isnull = new NpgsqlCommand("proc_get_dtrdetails1_when_dfid_isnotnull");
                    cmd_dfid_isnull.Parameters.AddWithValue("p_dtrcode", Convert.ToString(objDtcMissEntry.sDtrCode ?? ""));
                    dt = ObjCon.FetchDataTable(cmd_dfid_isnull);

                    if (dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                    else
                    {
                        string sqry1 = string.Empty;

                        #region Old inline query
                        //strQry = "SELECT \"TC_CURRENT_LOCATION\" from \"TBLTCMASTER\" WHERE \"TC_CODE\"=";
                        //strQry += "'" + objDtcMissEntry.sDtrCode + "'";
                        //sqry1 = ObjCon.get_value(strQry);
                        #endregion

                        QryKey = "GET_TC_CURRENT_LOCATION";
                        NpgsqlCommand cmd_TBLTCMASTER = new NpgsqlCommand("fetch_getvalue_dtrdetails1");
                        cmd_TBLTCMASTER.Parameters.AddWithValue("p_key", QryKey);
                        cmd_TBLTCMASTER.Parameters.AddWithValue("p_value_1", Convert.ToString(objDtcMissEntry.sDtrCode ?? ""));
                        sqry1 = ObjBseCon.StringGetValue(cmd_TBLTCMASTER);

                        #region old inline query
                        //if (sqry1 != "1")
                        //{
                        //    strQry = "SELECT \"TC_CODE\", cast(\"TC_SLNO\" as text)\"TC_SLNO\",'' as \"DT_CODE\",";
                        //    strQry += "\"TC_CAPACITY\",";
                        //    strQry += "\"DF_REPLACE_FLAG\",";
                        //    strQry += "\"TC_UPDATED_EVENT\",\"TC_LOCATION_ID\", ";
                        //    strQry += "CASE \"TC_CURRENT_LOCATION\" WHEN  1 THEN 'STORE' WHEN 2 THEN  'FIELD' ";
                        //    strQry += "WHEN 3 THEN 'REPAIRER' WHEN 5 THEN 'BANK' ";
                        //    strQry += "END AS \"TC_CURRENT_LOCATION\", ";
                        //    strQry += "CASE \"TC_STATUS\" WHEN 1 THEN 'GOOD CONDITION' WHEN 2 THEN 'REPAIRED ";
                        //    strQry += "THEN GOOD' WHEN 3 THEN ";
                        //    strQry += "'FAILED' WHEN 11 THEN ";
                        //    strQry += "'RELEASED GOOD' WHEN 6 THEN 'SCRAP' END AS \"STATUS\",(SELECT \"OFF_NAME\" ";
                        //    strQry += "FROM \"VIEW_OFFICES\" WHERE ";
                        //    strQry += "\"OFF_CODE\"=\"TC_LOCATION_ID\")\"OFFNAME\"";
                        //    strQry += " FROM \"TBLTCMASTER\",\"TBLTCDRAWN\",\"TBLDTCFAILURE\" WHERE \"TC_CODE\"=";
                        //    strQry += "\"TD_TC_NO\" AND ";
                        //    strQry += "\"DF_ID\"=\"TD_DF_ID\" ";
                        //    strQry += "AND \"TC_CODE\"='" + objDtcMissEntry.sDtrCode + "'";
                        //    dt1 = ObjCon.FetchDataTable(strQry);
                        //}
                        //else
                        //{
                        //    strQry = "SELECT \"TC_CODE\", cast(\"TC_SLNO\" as text)\"TC_SLNO\",'' as \"DT_CODE\",";
                        //    strQry += "\"TC_CAPACITY\",\"DF_REPLACE_FLAG\",";
                        //    strQry += "\"TC_UPDATED_EVENT\",\"TC_LOCATION_ID\", ";
                        //    strQry += "CASE \"TC_CURRENT_LOCATION\" WHEN  1 THEN 'STORE' WHEN 2 THEN  'FIELD' ";
                        //    strQry += "WHEN 3 THEN 'REPAIRER' WHEN 5 THEN 'BANK' END AS ";
                        //    strQry += "\"TC_CURRENT_LOCATION\", ";
                        //    strQry += "CASE \"TC_STATUS\" WHEN 1 THEN 'GOOD CONDITION' WHEN 2 THEN 'REPAIRED ";
                        //    strQry += "THEN GOOD' WHEN 3 THEN 'FAILED'  ";
                        //    strQry += "WHEN 11 THEN 'RELEASED GOOD' WHEN 6 THEN 'SCRAP' END AS \"STATUS\",";
                        //    strQry += "(SELECT \"SM_NAME\"||'~'||'STORE' FROM ";
                        //    strQry += "\"TBLSTOREMAST\" WHERE \"SM_ID\"=\"TC_LOCATION_ID\")\"OFFNAME\"";
                        //    strQry += " FROM \"TBLTCMASTER\",\"TBLTCDRAWN\",\"TBLDTCFAILURE\" WHERE ";
                        //    strQry += "\"TC_CODE\"=\"TD_TC_NO\" AND ";
                        //    strQry += "\"DF_ID\"=\"TD_DF_ID\" AND \"TC_CODE\"='" + objDtcMissEntry.sDtrCode + "'";
                        //    dt1 = ObjCon.FetchDataTable(strQry);
                        //}
                        #endregion

                        NpgsqlCommand cmd_on_TC_CURRENT_LOCATION = new NpgsqlCommand("proc_get_dtrdtls1_on_tc_loc_dfid_notnull");
                        cmd_on_TC_CURRENT_LOCATION.Parameters.AddWithValue("p_dtrcurr_loc", Convert.ToString(sqry1 ?? ""));
                        cmd_on_TC_CURRENT_LOCATION.Parameters.AddWithValue("p_dtrcode", Convert.ToString(objDtcMissEntry.sDtrCode ?? ""));
                        dt1 = ObjCon.FetchDataTable(cmd_on_TC_CURRENT_LOCATION);

                        return dt1;
                    }
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
            finally
            {

            }
        }
        /// <summary>
        /// for dtr mapping to store 
        /// </summary>
        /// <param name="objsend"></param>
        /// <returns></returns>
        public string[] SendTOStore(clsDtcMissMatchEntry objsend)
        {
            string[] Arr = new string[2];
            string strQry = string.Empty;
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            string Desc = string.Empty;
            try
            {

                #region Validation of DTR codes 

                #region old inline query
                //strQry = "SELECT \"TC_CAPACITY\",\"TC_STATUS\",\"TC_CURRENT_LOCATION\",\"TC_LOCATION_ID\",\"TC_STORE_ID\" ";
                //strQry += "FROM \"TBLTCMASTER\" WHERE \"TC_CODE\"='" + objsend.sDtrCode + "'";
                //dt = ObjCon.FetchDataTable(strQry);
                #endregion

                NpgsqlCommand cmd_overall_validation = new NpgsqlCommand("proc_get_sendtostore_overall_validation");
                cmd_overall_validation.Parameters.AddWithValue("p_dtrcode", Convert.ToString(objsend.sDtrCode ?? ""));
                dt = ObjCon.FetchDataTable(cmd_overall_validation);

                if (dt.Rows.Count > 0)
                {
                    string tcstatus = dt.Rows[0]["TC_STATUS"].ToString();
                    string tcCurentLocation = dt.Rows[0]["TC_CURRENT_LOCATION"].ToString();
                    string tcLocationId = dt.Rows[0]["TC_LOCATION_ID"].ToString();
                    string storeId = dt.Rows[0]["TC_STORE_ID"].ToString();

                    // check whether tc is in interstore transfer 
                    if (tcCurentLocation == "4")
                    {
                        Arr[0] = "2";
                        Arr[1] = "DTr is in Interstore transfer so cant allocate  ";
                        return Arr;
                    }
                    //tc shpuld be in the scrap 
                    if (tcstatus == "6" || tcstatus == "7")
                    {
                        Arr[0] = "2";
                        Arr[1] = "DTr has been sent to scrap";
                        return Arr;
                    }

                    #region old inline query
                    //strQry = "select  \"SM_ID\" from \"TBLSTOREMAST\" where  \"SM_CODE\"='" + objsend.sStoreId + "'";
                    //dt = ObjCon.FetchDataTable(strQry);
                    //if (dt.Rows.Count > 0)
                    #endregion

                    QryKey = "GET_VALIDAT_TBLSTOREMAST";
                    NpgsqlCommand cmd_VALIDAT_TBLSTOREMAST = new NpgsqlCommand("fetch_getvalue_for_sendtostore");
                    cmd_VALIDAT_TBLSTOREMAST.Parameters.AddWithValue("p_key", QryKey);
                    cmd_VALIDAT_TBLSTOREMAST.Parameters.AddWithValue("p_value_1", Convert.ToString(objsend.sStoreId ?? ""));
                    string SM_ID = ObjBseCon.StringGetValue(cmd_VALIDAT_TBLSTOREMAST);

                    if ((SM_ID ?? "").Length > 0)
                    {
                        //string smId = dt.Rows[0]["SM_ID"].ToString();
                        string smId = SM_ID;

                        if (tcLocationId == smId && storeId == smId && objsend.sDtrStatus == tcstatus && tcCurentLocation == "1")
                        {
                            Arr[0] = "2";
                            Arr[1] = "DTr Already Present in Store with Selected Condition";
                            return Arr;
                        }
                    }

                    //DTR shouldnt be failed 
                    #region old inline query
                    //strQry = "SELECT \"DF_ID\" from \"TBLDTCFAILURE\" WHERE CAST(\"DF_EQUIPMENT_ID\" AS TEXT) ";
                    //strQry += "= '" + objsend.sDtrCode + "' AND CAST(\"DF_REPLACE_FLAG\" AS INT) = 0";
                    //dt1 = ObjCon.FetchDataTable(strQry);
                    //if (dt1.Rows.Count > 0)
                    #endregion

                    QryKey = "GET_VALIDAT_TBLDTCFAILURE";
                    NpgsqlCommand cmd_VALIDAT_TBLDTCFAILURE = new NpgsqlCommand("fetch_getvalue_for_sendtostore");
                    cmd_VALIDAT_TBLDTCFAILURE.Parameters.AddWithValue("p_key", QryKey);
                    cmd_VALIDAT_TBLDTCFAILURE.Parameters.AddWithValue("p_value_1", Convert.ToString(objsend.sDtrCode ?? ""));
                    string DF_ID = ObjBseCon.StringGetValue(cmd_VALIDAT_TBLDTCFAILURE);

                    if ((DF_ID ?? "").Length > 0)
                    {
                        string sWoNo = string.Empty;
                        if (validateCRforDTR == "TRUE")
                        {
                            #region old inline query
                            //strQry = "SELECT \"WO_NO\" FROM \"TBLDTCFAILURE\",\"TBLWORKORDER\" WHERE \"DF_ID\" ";
                            //strQry += "= \"WO_DF_ID\" and \"DF_EQUIPMENT_ID\" ";
                            //strQry += "= '" + objsend.sDtrCode + "'";
                            //sWoNo = ObjCon.get_value(strQry);
                            #endregion

                            QryKey = "GET_WO_NO";
                            NpgsqlCommand cmd_WO_NO = new NpgsqlCommand("fetch_getvalue_for_sendtostore");
                            cmd_WO_NO.Parameters.AddWithValue("p_key", QryKey);
                            cmd_WO_NO.Parameters.AddWithValue("p_value_1", Convert.ToString(objsend.sDtrCode ?? ""));
                            sWoNo = ObjBseCon.StringGetValue(cmd_WO_NO);

                            Arr[0] = "2";
                            Arr[1] = "DTr Code / DTr PLATE / Unique Id has been failed and needs to complete the cycle  ";
                            Arr[1] += "with workorder number " + sWoNo + " so cant allocate";
                            return Arr;
                        }

                        if (tcLocationId.Length == 5)
                        {
                            #region old inline query
                            //strQry = "SELECT \"WO_NO\" FROM \"TBLDTCFAILURE\",\"TBLWORKORDER\" WHERE \"DF_ID\" = ";
                            //strQry += "\"WO_DF_ID\" and \"DF_EQUIPMENT_ID\" ";
                            //strQry += "= '" + objsend.sDtrCode + "'";
                            //sWoNo = ObjCon.get_value(strQry);
                            #endregion

                            QryKey = "GET_WO_NO";
                            NpgsqlCommand cmd_WO_NO = new NpgsqlCommand("fetch_getvalue_for_sendtostore");
                            cmd_WO_NO.Parameters.AddWithValue("p_key", QryKey);
                            cmd_WO_NO.Parameters.AddWithValue("p_value_1", Convert.ToString(objsend.sDtrCode ?? ""));
                            sWoNo = ObjBseCon.StringGetValue(cmd_WO_NO);

                            Arr[0] = "2";
                            Arr[1] = "DTr CODE / DTr PLATE / Unique Id has been failed and needs to complete the cycle  ";
                            Arr[1] += "with workorder number " + sWoNo + " so cant allocate";
                            return Arr;
                        }

                    }

                    if (!(validationstatus == "FALSE"))
                    {

                        //DTR should  or moved from backend 
                        #region old inline query
                        //strQry = "SELECT \"DM_REMARKS\" FROM \"TBLDTRMISMATCHENTRY\" WHERE \"DM_DTR_CODE\" ";
                        //strQry += "='" + objsend.sDtrCode + "' ";
                        //dt = ObjCon.FetchDataTable(strQry);
                        //if (dt.Rows.Count > 0)
                        #endregion

                        QryKey = "GET_VALIDAT_TBLDTRMISMATCHENTRY";
                        NpgsqlCommand cmd_VALIDAT_TBLDTRMISMATCHENTRY = new NpgsqlCommand("fetch_getvalue_for_sendtostore");
                        cmd_VALIDAT_TBLDTRMISMATCHENTRY.Parameters.AddWithValue("p_key", QryKey);
                        cmd_VALIDAT_TBLDTRMISMATCHENTRY.Parameters.AddWithValue("p_value_1", Convert.ToString(objsend.sDtrCode ?? ""));
                        string DM_REMARKS = ObjBseCon.StringGetValue(cmd_VALIDAT_TBLDTRMISMATCHENTRY);

                        if ((DM_REMARKS ?? "").Length > 0)
                        {
                            //string remarks = dt.Rows[0]["DM_REMARKS"].ToString();
                            string remarks = DM_REMARKS;

                            Arr[0] = "2";
                            Arr[1] = " DTr has been moved from  Backend .. Remarks as follows " + remarks + "";
                            return Arr;
                        }
                    }

                    //DTR should be sent to repair centre 
                    #region old inline query
                    //strQry = "SELECT \"RSM_PO_NO\" FROM \"TBLREPAIRSENTMASTER\",\"TBLREPAIRSENTDETAILS\" WHERE ";
                    //strQry += "\"RSM_ID\" = \"RSD_RSM_ID\" ";
                    //strQry += "AND \"RSD_DELIVARY_DATE\" IS NULL  AND \"RSD_TC_CODE\" ='" + objsend.sDtrCode + "' ";
                    //dt = ObjCon.FetchDataTable(strQry);
                    //if (dt.Rows.Count > 0)
                    #endregion

                    QryKey = "GET_VALIDAT_TBLREPAIRSENTMASTER";
                    NpgsqlCommand cmd_VALIDAT_TBLREPAIRSENTMASTER = new NpgsqlCommand("fetch_getvalue_for_sendtostore");
                    cmd_VALIDAT_TBLREPAIRSENTMASTER.Parameters.AddWithValue("p_key", QryKey);
                    cmd_VALIDAT_TBLREPAIRSENTMASTER.Parameters.AddWithValue("p_value_1", Convert.ToString(objsend.sDtrCode ?? ""));
                    string RSM_PO_NO = ObjBseCon.StringGetValue(cmd_VALIDAT_TBLREPAIRSENTMASTER);

                    if ((RSM_PO_NO ?? "").Length > 0)
                    {
                        //string pono = dt.Rows[0]["RSM_PO_NO"].ToString();
                        string pono = RSM_PO_NO;
                        Arr[0] = "2";
                        Arr[1] = " DTr has been sent to repair centre  with Purchase  order number  " + pono + "";
                        return Arr;
                    }


                    if (!(validationstatus == "FALSE"))
                    {
                        //DTR should not be allocated from backend 
                        #region old inline query
                        //strQry = "SELECT \"DME_REMARKS\" FROM \"TBLDTCMISMATCHENTRY\" WHERE \"DME_NEW_DTR_CODE\" ";
                        //strQry += "= '" + objsend.sDtrCode + "' ";
                        //dt = ObjCon.FetchDataTable(strQry);
                        //if (dt.Rows.Count > 0)
                        #endregion

                        QryKey = "GET_VALIDAT_TBLDTCMISMATCHENTRY";
                        NpgsqlCommand cmd_VALIDAT_TBLDTCMISMATCHENTRY = new NpgsqlCommand("fetch_getvalue_for_sendtostore");
                        cmd_VALIDAT_TBLDTCMISMATCHENTRY.Parameters.AddWithValue("p_key", QryKey);
                        cmd_VALIDAT_TBLDTCMISMATCHENTRY.Parameters.AddWithValue("p_value_1", Convert.ToString(objsend.sDtrCode ?? ""));
                        string DME_REMARKS = ObjBseCon.StringGetValue(cmd_VALIDAT_TBLDTCMISMATCHENTRY);

                        if ((DME_REMARKS ?? "").Length > 0)
                        {
                            //string remarks = dt.Rows[0]["DME_REMARKS"].ToString();
                            string remarks = DME_REMARKS;

                            Arr[0] = "2";
                            Arr[1] = " DTr has been mapped from  Backend .. Remarks as follows " + remarks + "";
                            return Arr;
                        }
                    }
                }


                #endregion

                string Store_id = string.Empty;

                #region old inline query
                //strQry = "SELECT \"SM_ID\" FROM \"TBLSTOREMAST\",\"TBLSTOREOFFCODE\" WHERE \"SM_ID\"=\"STO_SM_ID\" AND ";
                //strQry += "\"STO_OFF_CODE\"='" + objsend.sStoreId + "'";
                //Store_id = ObjCon.get_value(strQry);
                #endregion

                QryKey = "GET_SM_ID";
                NpgsqlCommand cmd_SM_ID = new NpgsqlCommand("fetch_getvalue_for_sendtostore");
                cmd_SM_ID.Parameters.AddWithValue("p_key", QryKey);
                cmd_SM_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(objsend.sStoreId ?? ""));
                Store_id = ObjBseCon.StringGetValue(cmd_SM_ID);

                long DTRSL_NO = ObjCon.Get_max_no("DM_SL_NO", "TBLDTRMISMATCHENTRY");

                #region old inline query
                //strQry = " SELECT \"DT_CODE\",\"DT_OM_SLNO\",\"TC_CURRENT_LOCATION\",\"TC_LOCATION_ID\" FROM ";
                //strQry += "\"TBLDTCMAST\",\"TBLTCMASTER\" ";
                //strQry += "WHERE \"TC_CODE\"=\"DT_TC_ID\" AND \"TC_CODE\"='" + objsend.sDtrCode + "'";
                //dt = ObjCon.FetchDataTable(strQry);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_sendtostore_overall_details");
                cmd.Parameters.AddWithValue("p_dtrcode", Convert.ToString(objsend.sDtrCode ?? ""));
                dt = ObjCon.FetchDataTable(cmd);

                #region dt based opration
                if (dt.Rows.Count > 0)
                {
                    string tc_id = string.Empty;
                    string From_Office_Name = string.Empty;
                    string To_Office_Name = string.Empty;

                    if (dt.Rows.Count > 0)
                    {
                        tc_id = dt.Rows[0]["TC_LOCATION_ID"].ToString();
                    }

                    #region old inline query
                    //strQry = " SELECT \"OFF_NAME\" FROM \"VIEW_OFFICES\" WHERE \"OFF_CODE\"= '" + tc_id + "' ";
                    //From_Office_Name = ObjCon.get_value(strQry);
                    #endregion

                    QryKey = "GET_OFF_NAME";
                    NpgsqlCommand cmd_OFF_NAME = new NpgsqlCommand("fetch_getvalue_for_sendtostore");
                    cmd_OFF_NAME.Parameters.AddWithValue("p_key", QryKey);
                    cmd_OFF_NAME.Parameters.AddWithValue("p_value_1", Convert.ToString(tc_id ?? ""));
                    From_Office_Name = ObjBseCon.StringGetValue(cmd_OFF_NAME);

                    #region old inline query
                    //strQry = "SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\",\"TBLSTOREOFFCODE\" WHERE \"SM_ID\"=\"STO_SM_ID\" ";
                    //strQry += "AND \"STO_OFF_CODE\"='" + objsend.sStoreId + "'";
                    //To_Office_Name = ObjCon.get_value(strQry);
                    #endregion

                    QryKey = "GET_SM_NAME";
                    NpgsqlCommand cmd_SM_NAME = new NpgsqlCommand("fetch_getvalue_for_sendtostore");
                    cmd_SM_NAME.Parameters.AddWithValue("p_key", QryKey);
                    cmd_SM_NAME.Parameters.AddWithValue("p_value_1", Convert.ToString(objsend.sStoreId ?? ""));
                    To_Office_Name = ObjBseCon.StringGetValue(cmd_SM_NAME);

                    if (tc_id.Length != 5)
                    {
                        Desc = "MOVED FROM " + From_Office_Name + " STORE TO " + To_Office_Name + " STORE (FROM BACKEND)";
                    }
                    else
                    {
                        Desc = "MOVED FROM " + From_Office_Name + " FIELD TO " + To_Office_Name + " STORE (FROM BACKEND)";
                    }

                    ObjCon.BeginTransaction();

                    #region Old inline query
                    //strQry = " INSERT INTO \"TBLDTRMISMATCHENTRY\" (\"DM_SL_NO\",\"DM_ENTRY_DATE\",\"DM_DTC_CODE\",";
                    //strQry += "\"DM_DTR_CODE\",";
                    //strQry += "\"DM_DTRCODE_OLD_LOCTYPE\",";
                    //strQry += "\"DM_DTRCODE_OLD_REFCODE\",\"DM_DTRCODE_NEW_LOCTYPE\",\"DM_DTRCODE_NEW_REFCODE\",";
                    //strQry += "\"DM_REMARKS\",\"DM_CREATED_BY\")";
                    //strQry += " VALUES ('" + DTRSL_NO + "',now(),'" + dt.Rows[0]["DT_CODE"].ToString() + "',";
                    //strQry += "'" + objsend.sDtrCode + "',";
                    //strQry += "'" + dt.Rows[0]["TC_CURRENT_LOCATION"] + "',";
                    //strQry += " '" + dt.Rows[0]["DT_OM_SLNO"].ToString() + "','1','" + Store_id + "',";
                    //strQry += "'" + objsend.sRemarks + "','" + objsend.sCrBy + "')";
                    //ObjCon.ExecuteQry(strQry);


                    //strQry = " UPDATE \"TBLDTCMAST\" SET \"DT_TC_ID\"='0' WHERE \"DT_CODE\"=";
                    //strQry += "'" + dt.Rows[0]["DT_CODE"].ToString() + "'";
                    //ObjCon.ExecuteQry(strQry);

                    //strQry = "UPDATE \"TBLTRANSDTCMAPPING\" SET \"TM_LIVE_FLAG\"='0',\"TM_UNMAP_CRON\"=now(),";
                    //strQry += "\"TM_UNMAP_CRBY\"";
                    //strQry += "= '" + objsend.sCrBy + "',\"TM_UNMAP_REASON\"='" + objsend.sRemarks + "' WHERE \"TM_DTC_ID\"=";
                    //strQry += "'" + dt.Rows[0]["DT_CODE"].ToString() + "' AND \"TM_LIVE_FLAG\"=1";
                    //ObjCon.ExecuteQry(strQry);

                    //strQry = "SELECT \"DF_ID\" FROM \"TBLDTCFAILURE\" WHERE \"DF_DTC_CODE\"=";
                    //strQry += "'" + dt.Rows[0]["DT_CODE"].ToString() + "'";
                    //string val = ObjCon.get_value(strQry);

                    //long SL_NO = ObjCon.Get_max_no("UNA_SL_NO", "TBLUNALLOCATEDDTCS");

                    //if (val == null || val == "")
                    //{
                    //    strQry = "INSERT INTO \"TBLUNALLOCATEDDTCS\" (\"UNA_SL_NO\",\"UNA_MISMATCHENTRY_SLNO\",";
                    //    strQry += "\"UNA_DTCCODE\",\"UNA_IS_DTC_FAILURE\",";
                    //    strQry += "\"UNA_ENTRY_FROM\") VALUES (";
                    //    strQry += " '" + SL_NO + "','" + DTRSL_NO + "','" + dt.Rows[0]["DT_CODE"].ToString() + "',";
                    //    strQry += "'NO','TBLDTRMISMATCHENTRY')";

                    //    ObjCon.ExecuteQry(strQry);

                    //}
                    //else
                    //{
                    //    strQry = "INSERT INTO \"TBLUNALLOCATEDDTCS\" (\"UNA_SL_NO\",\"UNA_MISMATCHENTRY_SLNO\",";
                    //    strQry += "\"UNA_DTCCODE\",\"UNA_IS_DTC_FAILURE\",";
                    //    strQry += "\"UNA_FAILURE_ID\",\"UNA_ENTRY_FROM\") VALUES (";
                    //    strQry += " '" + SL_NO + "','" + DTRSL_NO + "','" + dt.Rows[0]["DT_CODE"].ToString() + "',";
                    //    strQry += "'YES','" + val + "','TBLDTRMISMATCHENTRY')";
                    //    ObjCon.ExecuteQry(strQry);
                    //}

                    //strQry = " UPDATE \"TBLTCMASTER\" SET \"TC_CURRENT_LOCATION\"='1',\"TC_LOCATION_ID\"=";
                    //strQry += "'" + Store_id + "',\"TC_STATUS\"=";
                    //strQry += "'" + objsend.sDtrStatus + "',\"TC_STORE_ID\"='" + Store_id + "' ";
                    //strQry += "WHERE \"TC_CODE\"='" + objsend.sDtrCode + "'";
                    //ObjCon.ExecuteQry(strQry);

                    //strQry = "INSERT INTO \"TBLDTRTRANSACTION\" (\"DRT_ID\",\"DRT_DTR_CODE\",\"DRT_LOC_ID\",";
                    //strQry += "\"DRT_LOC_TYPE\",\"DRT_TRANS_DATE\",";
                    //strQry += "\"DRT_ACT_REFNO\",\"DRT_ACT_REFTYPE\",\"DRT_DESC\",\"DRT_DTR_STATUS\",";
                    //strQry += "\"DRT_ENTRYDATE\",\"DRT_CANCEL_FLAG\") VALUES ";
                    //strQry += "((SELECT COALESCE(max(\"DRT_ID\"),0)+1 FROM \"TBLDTRTRANSACTION\"),";
                    //strQry += "'" + objsend.sDtrCode + "',";
                    //strQry += "'" + Store_id + "','1',now(),'','2','" + Desc + "','" + objsend.sDtrStatus + "'";
                    //strQry += ",now(),'0')";
                    //ObjCon.ExecuteQry(strQry);
                    #endregion

                    NpgsqlCommand cmdSendtostore = new NpgsqlCommand("proc_saveupdate_sendtostore_on_overall_dt");
                    cmdSendtostore.Parameters.AddWithValue("p_dtrsl_no", Convert.ToInt64(DTRSL_NO));
                    cmdSendtostore.Parameters.AddWithValue("p_dt_code", Convert.ToString(dt.Rows[0]["DT_CODE"] ?? ""));
                    cmdSendtostore.Parameters.AddWithValue("p_dtrcode", Convert.ToString(objsend.sDtrCode ?? ""));
                    cmdSendtostore.Parameters.AddWithValue("p_tc_current_location", Convert.ToString(dt.Rows[0]["TC_CURRENT_LOCATION"] ?? ""));
                    cmdSendtostore.Parameters.AddWithValue("p_dt_om_slno", Convert.ToString(dt.Rows[0]["DT_OM_SLNO"] ?? ""));
                    cmdSendtostore.Parameters.AddWithValue("p_store_id", Convert.ToString(Store_id ?? ""));
                    cmdSendtostore.Parameters.AddWithValue("p_remarks", Convert.ToString(objsend.sRemarks ?? ""));
                    cmdSendtostore.Parameters.AddWithValue("p_crby", Convert.ToString(objsend.sCrBy ?? ""));
                    cmdSendtostore.Parameters.AddWithValue("p_dtrstatus", Convert.ToString(objsend.sDtrStatus ?? ""));
                    cmdSendtostore.Parameters.AddWithValue("p_desc", Convert.ToString(Desc ?? ""));
                    cmdSendtostore.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmdSendtostore.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmdSendtostore.Parameters["op_id"].Direction = ParameterDirection.Output;
                    cmdSendtostore.Parameters["msg"].Direction = ParameterDirection.Output;
                    Arr[0] = "op_id";
                    Arr[1] = "msg";
                    Arr = ObjCon.Execute(cmdSendtostore, Arr, 2);

                }
                else
                {
                    #region old inline query
                    //strQry = "SELECT \"TC_CAPACITY\",\"TC_STATUS\",\"TC_CURRENT_LOCATION\",\"TC_LOCATION_ID\" FROM ";
                    //strQry += "\"TBLTCMASTER\" WHERE \"TC_CODE\"='" + objsend.sDtrCode + "'";
                    //dt = ObjCon.FetchDataTable(strQry);
                    #endregion

                    NpgsqlCommand cmd_sendtostore = new NpgsqlCommand("proc_get_sendtostore_details_on_case");
                    cmd_sendtostore.Parameters.AddWithValue("p_dtrcode", Convert.ToString(objsend.sDtrCode ?? ""));
                    dt = ObjCon.FetchDataTable(cmd_sendtostore);

                    string From_Office_Name = string.Empty;
                    string tc_id = string.Empty;
                    string To_Office_Name = string.Empty;
                    string StoreId = string.Empty;

                    if (dt.Rows.Count > 0)
                    {
                        tc_id = dt.Rows[0]["TC_LOCATION_ID"].ToString();
                    }

                    if (dt.Rows[0]["TC_CURRENT_LOCATION"].ToString() == "2")
                    {
                        #region old inline query
                        //strQry = " SELECT \"OFF_NAME\" FROM \"VIEW_OFFICES\" WHERE \"OFF_CODE\"= '" + tc_id + "' ";
                        //From_Office_Name = ObjCon.get_value(strQry);
                        #endregion

                        QryKey = "GET_OFF_NAME";
                        NpgsqlCommand cmd_OFF_NAME = new NpgsqlCommand("fetch_getvalue_for_sendtostore");
                        cmd_OFF_NAME.Parameters.AddWithValue("p_key", QryKey);
                        cmd_OFF_NAME.Parameters.AddWithValue("p_value_1", Convert.ToString(tc_id ?? ""));
                        From_Office_Name = ObjBseCon.StringGetValue(cmd_OFF_NAME);
                    }
                    else
                    {
                        #region old inline query
                        //strQry = "SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\",\"TBLSTOREOFFCODE\" WHERE \"SM_ID\"=\"STO_SM_ID\" ";
                        //strQry += "AND \"STO_OFF_CODE\"='" + objsend.sStoreId + "'";
                        //From_Office_Name = ObjCon.get_value(strQry);
                        #endregion

                        QryKey = "GET_SM_NAME";
                        NpgsqlCommand cmd_SM_NAME = new NpgsqlCommand("fetch_getvalue_for_sendtostore");
                        cmd_SM_NAME.Parameters.AddWithValue("p_key", QryKey);
                        cmd_SM_NAME.Parameters.AddWithValue("p_value_1", Convert.ToString(objsend.sStoreId ?? ""));
                        From_Office_Name = ObjBseCon.StringGetValue(cmd_SM_NAME);
                    }

                    #region Old inline query
                    //strQry = "SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\",\"TBLSTOREOFFCODE\" WHERE \"SM_ID\"=\"STO_SM_ID\" AND ";
                    //strQry += "\"STO_OFF_CODE\"='" + objsend.sStoreId + "'";
                    //To_Office_Name = ObjCon.get_value(strQry);
                    #endregion

                    QryKey = "GET_SM_NAME";
                    NpgsqlCommand cmd_TBLSTOREMAST = new NpgsqlCommand("fetch_getvalue_for_sendtostore");
                    cmd_TBLSTOREMAST.Parameters.AddWithValue("p_key", QryKey);
                    cmd_TBLSTOREMAST.Parameters.AddWithValue("p_value_1", Convert.ToString(objsend.sStoreId ?? ""));
                    To_Office_Name = ObjBseCon.StringGetValue(cmd_TBLSTOREMAST);

                    #region old inline query
                    //strQry = "SELECT \"SM_ID\" FROM \"TBLSTOREMAST\",\"TBLSTOREOFFCODE\" WHERE \"SM_ID\"=\"STO_SM_ID\" AND ";
                    //strQry += "\"STO_OFF_CODE\"='" + objsend.sStoreId + "'";
                    //StoreId = ObjCon.get_value(strQry);
                    #endregion

                    QryKey = "GET_SM_ID";
                    NpgsqlCommand cmd_SM_ID_TBLSTOREMAST = new NpgsqlCommand("fetch_getvalue_for_sendtostore");
                    cmd_SM_ID_TBLSTOREMAST.Parameters.AddWithValue("p_key", QryKey);
                    cmd_SM_ID_TBLSTOREMAST.Parameters.AddWithValue("p_value_1", Convert.ToString(objsend.sStoreId ?? ""));
                    StoreId = ObjBseCon.StringGetValue(cmd_SM_ID_TBLSTOREMAST);

                    //strQry = "SELECT SM_MMS_STORE_ID FROM TBLSTOREMAST WHERE SM_ID='" + StoreId + "'";
                    //string sMmsStoreId = ObjCon.get_value(strQry);

                    if (tc_id.Length != 5)
                    {
                        Desc = "MOVED FROM " + From_Office_Name + " STORE TO " + To_Office_Name + " STORE (FROM BACKEND)";
                    }
                    else
                    {
                        Desc = "MOVED FROM " + From_Office_Name + " FIELD TO " + To_Office_Name + " STORE (FROM BACKEND)";
                    }

                    ObjCon.BeginTransaction();

                    #region old inline query
                    //strQry = " INSERT INTO \"TBLDTRMISMATCHENTRY\" (\"DM_SL_NO\",\"DM_ENTRY_DATE\",\"DM_DTC_CODE\",";
                    //strQry += "\"DM_DTR_CODE\",";
                    //strQry += "\"DM_DTRCODE_OLD_LOCTYPE\",\"DM_DTRCODE_OLD_REFCODE\",\"DM_DTRCODE_NEW_LOCTYPE\",";
                    //strQry += "\"DM_DTRCODE_NEW_REFCODE\",";
                    //strQry += "\"DM_REMARKS\",\"DM_CREATED_BY\")";
                    //strQry += " VALUES ('" + DTRSL_NO + "',now(),'','" + objsend.sDtrCode + "',";
                    //strQry += "'" + dt.Rows[0]["TC_CURRENT_LOCATION"] + "',";
                    //strQry += " '" + dt.Rows[0]["TC_LOCATION_ID"].ToString() + "','1','" + StoreId + "',";
                    //strQry += "'" + objsend.sRemarks + "','" + objsend.sCrBy + "')";
                    //ObjCon.ExecuteQry(strQry);

                    //strQry = " UPDATE \"TBLTCMASTER\" SET \"TC_CURRENT_LOCATION\"='1',\"TC_LOCATION_ID\"=";
                    //strQry += "'" + StoreId + "',\"TC_STATUS\"=";
                    //strQry += "'" + objsend.sDtrStatus + "',\"TC_STORE_ID\"='" + StoreId + "' WHERE \"TC_CODE\"=";
                    //strQry += "'" + objsend.sDtrCode + "'";
                    //ObjCon.ExecuteQry(strQry);

                    //strQry = "INSERT INTO \"TBLDTRTRANSACTION\" (\"DRT_ID\",\"DRT_DTR_CODE\",\"DRT_LOC_ID\",";
                    //strQry += "\"DRT_LOC_TYPE\",\"DRT_TRANS_DATE\",";
                    //strQry += "\"DRT_ACT_REFNO\",\"DRT_ACT_REFTYPE\",\"DRT_DESC\",\"DRT_DTR_STATUS\",\"DRT_ENTRYDATE\",";
                    //strQry += "\"DRT_CANCEL_FLAG\") VALUES ";
                    //strQry += "((SELECT COALESCE(max(\"DRT_ID\"),0)+1 FROM \"TBLDTRTRANSACTION\"),'" + objsend.sDtrCode + "',";
                    //strQry += "'" + StoreId + "','1',now(),'','2','" + Desc + "','" + objsend.sDtrStatus + "',now(),'0')";
                    //ObjCon.ExecuteQry(strQry);
                    #endregion

                    NpgsqlCommand cmdSendtostore_if_dtisnull = new NpgsqlCommand("proc_saveupdate_sendtostore_if_dtisnull");
                    cmdSendtostore_if_dtisnull.Parameters.AddWithValue("p_dtrsl_no", Convert.ToInt64(DTRSL_NO));
                    cmdSendtostore_if_dtisnull.Parameters.AddWithValue("p_dtrcode", Convert.ToString(objsend.sDtrCode ?? ""));
                    cmdSendtostore_if_dtisnull.Parameters.AddWithValue("p_tc_current_location", Convert.ToString(dt.Rows[0]["TC_CURRENT_LOCATION"] ?? ""));
                    cmdSendtostore_if_dtisnull.Parameters.AddWithValue("p_tc_location_id", Convert.ToString(dt.Rows[0]["TC_LOCATION_ID"] ?? ""));
                    cmdSendtostore_if_dtisnull.Parameters.AddWithValue("p_store_id", Convert.ToString(StoreId ?? ""));
                    cmdSendtostore_if_dtisnull.Parameters.AddWithValue("p_remarks", Convert.ToString(objsend.sRemarks ?? ""));
                    cmdSendtostore_if_dtisnull.Parameters.AddWithValue("p_crby", Convert.ToString(objsend.sCrBy ?? ""));
                    cmdSendtostore_if_dtisnull.Parameters.AddWithValue("p_dtrstatus", Convert.ToString(objsend.sDtrStatus ?? ""));
                    cmdSendtostore_if_dtisnull.Parameters.AddWithValue("p_desc", Convert.ToString(Desc ?? ""));
                    cmdSendtostore_if_dtisnull.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmdSendtostore_if_dtisnull.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmdSendtostore_if_dtisnull.Parameters["op_id"].Direction = ParameterDirection.Output;
                    cmdSendtostore_if_dtisnull.Parameters["msg"].Direction = ParameterDirection.Output;
                    Arr[0] = "op_id";
                    Arr[1] = "msg";
                    Arr = ObjCon.Execute(cmdSendtostore_if_dtisnull, Arr, 2);

                }
                #endregion

                ObjCon.CommitTransaction();

                //Arr[0] = "1";
                //Arr[1] = "Tc Send To Store Successfully";
                return Arr;
            }
            catch (Exception ex)
            {
                ObjCon.RollBackTrans();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                Arr[0] = "2";
                Arr[1] = "Error Occured";
                return Arr;
            }
            finally
            {

            }
        }
        /// <summary>
        /// for allocating dtr to dtc
        /// </summary>
        /// <param name="objswap"></param>
        /// <returns></returns>
        public string[] swapDetails(clsDtcMissMatchEntry objswap)
        {
            string[] Arr = new string[2];
            string strQry = string.Empty;
            string strQry1 = string.Empty;
            string strQry2 = string.Empty;
            string strQry3 = string.Empty;

            DataTable dt = new DataTable();
            DataTable dtDtrDetails = new DataTable();
            DataTable dtDtcDetails = new DataTable();
            DataTable dt1 = new DataTable();
            try
            {
                long sDTC_SL_NO = ObjCon.Get_max_no("DME_SL_NO", "TBLDTCMISMATCHENTRY");

                #region Validation of  DTC and DTR codes

                if (!(validationstatus == "FALSE"))
                {
                    //check whether the DTC or DTR has been mapped previsously in backend
                    #region old inline query
                    //strQry1 = "SELECT \"DME_DTC_CODE\",\"DME_NEW_DTR_CODE\",\"DME_REMARKS\" FROM \"TBLDTCMISMATCHENTRY\" ";
                    //strQry1 += "WHERE \"DME_DTC_CODE\" = '" + objswap.sDtcCode + "' OR \"DME_NEW_DTR_CODE\" = '" + objswap.sNewDtrCode + "'";
                    //dt = ObjCon.FetchDataTable(strQry1);
                    //if (dt.Rows.Count > 0)
                    #endregion

                    QryKey = "GET_VALIDATE_TBLDTCMISMATCHENTRY";
                    NpgsqlCommand cmd_VALIDATE_TBLDTCMISMATCHENTRY = new NpgsqlCommand("fetch_getvalue_for_swapdetails");
                    cmd_VALIDATE_TBLDTCMISMATCHENTRY.Parameters.AddWithValue("p_key", QryKey);
                    cmd_VALIDATE_TBLDTCMISMATCHENTRY.Parameters.AddWithValue("p_value_1", Convert.ToString(objswap.sDtcCode ?? ""));
                    cmd_VALIDATE_TBLDTCMISMATCHENTRY.Parameters.AddWithValue("p_value_2", Convert.ToString(objswap.sNewDtrCode ?? ""));
                    string DME_REMARKS = ObjBseCon.StringGetValue(cmd_VALIDATE_TBLDTCMISMATCHENTRY);

                    if ((DME_REMARKS ?? "").Length > 0)
                    {
                        ////string remarks = dt.Rows[0]["DME_REMARKS"].ToString();
                        string remarks = DME_REMARKS;
                        Arr[0] = "2";
                        Arr[1] = "DTC or DTr has been allocated using Backend .. Remarks as follows " + remarks + "";
                        return Arr;
                    }
                }

                //validate if is in workflowobject
                #region Old inline query
                //strQry2 = "SELECT * from \"TBLWORKFLOWOBJECTS\" WHERE \"WO_DATA_ID\"='" + objswap.sDtcCode + "'  ";
                //strQry2 += "and \"WO_RECORD_ID\"<0";
                //dt = ObjCon.FetchDataTable(strQry2);
                //if (dt.Rows.Count > 0)
                #endregion
                QryKey = "GET_VALIDATE_TBLWORKFLOWOBJECTS";
                NpgsqlCommand cmd_VALIDATE_TBLWORKFLOWOBJECTS = new NpgsqlCommand("fetch_getvalue_for_swapdetails");
                cmd_VALIDATE_TBLWORKFLOWOBJECTS.Parameters.AddWithValue("p_key", QryKey);
                cmd_VALIDATE_TBLWORKFLOWOBJECTS.Parameters.AddWithValue("p_value_1", Convert.ToString(objswap.sDtcCode ?? ""));
                cmd_VALIDATE_TBLWORKFLOWOBJECTS.Parameters.AddWithValue("p_value_2", "");
                string WO_ID = ObjBseCon.StringGetValue(cmd_VALIDATE_TBLWORKFLOWOBJECTS);

                if ((WO_ID ?? "").Length > 0)
                {
                    Arr[0] = "2";
                    Arr[1] = "DTC has been failed and needs to complete the cycle .. so cant allocate";
                    return Arr;
                }

                #region old inline query
                //strQry3 = "SELECT \"DT_TC_ID\" from \"TBLDTCMAST\" WHERE \"DT_CODE\"='" + objswap.sDtcCode + "' ";
                //dt = ObjCon.FetchDataTable(strQry3);
                //if (dt.Rows.Count > 0)
                #endregion
                QryKey = "GET_VALIDATE_TBLDTCMAST";
                NpgsqlCommand cmd_VALIDATE_TBLDTCMAST = new NpgsqlCommand("fetch_getvalue_for_swapdetails");
                cmd_VALIDATE_TBLDTCMAST.Parameters.AddWithValue("p_key", QryKey);
                cmd_VALIDATE_TBLDTCMAST.Parameters.AddWithValue("p_value_1", Convert.ToString(objswap.sDtcCode ?? ""));
                cmd_VALIDATE_TBLDTCMAST.Parameters.AddWithValue("p_value_2", "");
                string DT_TC_ID = ObjBseCon.StringGetValue(cmd_VALIDATE_TBLDTCMAST);

                if ((DT_TC_ID ?? "").Length > 0)
                {
                    //string tcCode = Convert.ToString(dt.Rows[0]["DT_TC_ID"]);
                    string tcCode = DT_TC_ID;

                    if (tcCode == objswap.sNewDtrCode)
                    {
                        Arr[0] = "2";
                        Arr[1] = "DTr Already Allocated with the Same DTC";
                        return Arr;
                    }
                }

                // validate if DTC has not  completed the cycle
                #region old inline query
                //strQry1 = " SELECT * from \"TBLDTCFAILURE\" WHERE \"DF_DTC_CODE\" = '" + objswap.sDtcCode + "' ";
                //strQry1 += " AND \"DF_REPLACE_FLAG\" = 0 ";
                //dt = ObjCon.FetchDataTable(strQry1);
                //if (dt.Rows.Count > 0)
                #endregion
                QryKey = "GET_VALIDATE_TBLDTCFAILURE";
                NpgsqlCommand cmd_VALIDATE_TBLDTCFAILURE = new NpgsqlCommand("fetch_getvalue_for_swapdetails");
                cmd_VALIDATE_TBLDTCFAILURE.Parameters.AddWithValue("p_key", QryKey);
                cmd_VALIDATE_TBLDTCFAILURE.Parameters.AddWithValue("p_value_1", Convert.ToString(objswap.sDtcCode ?? ""));
                cmd_VALIDATE_TBLDTCFAILURE.Parameters.AddWithValue("p_value_2", "");
                string DF_ID = ObjBseCon.StringGetValue(cmd_VALIDATE_TBLDTCFAILURE);

                if ((DF_ID ?? "").Length > 0)
                {
                    string sWoNo = string.Empty;

                    #region old inline query
                    //strQry1 = " SELECT \"WO_NO\" FROM \"TBLDTCFAILURE\",\"TBLWORKORDER\" WHERE \"DF_ID\" = \"WO_DF_ID\" ";
                    //strQry1 += " and \"DF_DTC_CODE\" = '" + objswap.sDtcCode + "' ";
                    //sWoNo = ObjCon.get_value(strQry1);
                    #endregion

                    QryKey = "GET_WO_NO";
                    NpgsqlCommand cmd_WO_NO = new NpgsqlCommand("fetch_getvalue_for_swapdetails");
                    cmd_WO_NO.Parameters.AddWithValue("p_key", QryKey);
                    cmd_WO_NO.Parameters.AddWithValue("p_value_1", Convert.ToString(objswap.sDtcCode ?? ""));
                    cmd_WO_NO.Parameters.AddWithValue("p_value_2", "");
                    sWoNo = ObjBseCon.StringGetValue(cmd_WO_NO);

                    Arr[0] = "2";
                    Arr[1] = "DTC has been failed and needs to complete the cycle  with workorder ";
                    Arr[1] += "number " + sWoNo + " so cant allocate";
                    return Arr;
                }


                //validate if DTR has failed and not completed the cycle
                #region old inline query
                //strQry1 = " SELECT * from \"TBLDTCFAILURE\" WHERE \"DF_EQUIPMENT_ID\" = '" + objswap.sNewDtrCode + "' ";
                //strQry1 += " AND \"DF_REPLACE_FLAG\" = 0 ";
                //dt = ObjCon.FetchDataTable(strQry1);
                //if (dt.Rows.Count > 0)
                #endregion
                QryKey = "GET_VALIDATE_TBLDTCFAILURE_ON_DF_EQUIPMENT_ID";
                NpgsqlCommand cmd_VALIDATE_TBLDTCFAILURE_ON_DF_EQUIPMENT_ID = new NpgsqlCommand("fetch_getvalue_for_swapdetails");
                cmd_VALIDATE_TBLDTCFAILURE_ON_DF_EQUIPMENT_ID.Parameters.AddWithValue("p_key", QryKey);
                cmd_VALIDATE_TBLDTCFAILURE_ON_DF_EQUIPMENT_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(objswap.sNewDtrCode ?? ""));
                cmd_VALIDATE_TBLDTCFAILURE_ON_DF_EQUIPMENT_ID.Parameters.AddWithValue("p_value_2", "");
                DF_ID = ObjBseCon.StringGetValue(cmd_VALIDATE_TBLDTCFAILURE_ON_DF_EQUIPMENT_ID);

                if ((DF_ID ?? "").Length > 0)
                {
                    string sWoNo = string.Empty;

                    #region old inline query
                    //strQry1 = " SELECT \"WO_NO\" FROM \"TBLDTCFAILURE\",\"TBLWORKORDER\" WHERE \"DF_ID\" = \"WO_DF_ID\" ";
                    //strQry1 += " and \"DF_EQUIPMENT_ID\" = '" + objswap.sNewDtrCode + "' ";
                    //sWoNo = ObjCon.get_value(strQry1);
                    #endregion

                    QryKey = "GET_WO_NO_ON_DF_EQUIPMENT_ID";
                    NpgsqlCommand cmd_WO_NO = new NpgsqlCommand("fetch_getvalue_for_swapdetails");
                    cmd_WO_NO.Parameters.AddWithValue("p_key", QryKey);
                    cmd_WO_NO.Parameters.AddWithValue("p_value_1", Convert.ToString(objswap.sNewDtrCode ?? ""));
                    cmd_WO_NO.Parameters.AddWithValue("p_value_2", "");
                    sWoNo = ObjBseCon.StringGetValue(cmd_WO_NO);

                    Arr[0] = "2";
                    Arr[1] = "DTr CODE / DTr PLATE / Unique Id has been failed and needs to complete the cycle with workorder ";
                    Arr[1] += "number " + sWoNo + " so can not allocate";
                    return Arr;
                }

                //check if the TC is in Store or  Field or repairer with good condition
                #region old inline query
                //strQry1 = " SELECT \"TC_STATUS\" ,\"TC_CURRENT_LOCATION\" ,\"TC_LOCATION_ID\",\"TC_STORE_ID\" ";
                //strQry1 += " FROM \"TBLTCMASTER\" WHERE \"TC_CODE\" = '" + objswap.sNewDtrCode + "' ";
                //dt = ObjCon.FetchDataTable(strQry1);
                #endregion

                NpgsqlCommand cmd_tbltcmaster_details_for_validate = new NpgsqlCommand("proc_get_tbltcmaster_details_for_validate");
                cmd_tbltcmaster_details_for_validate.Parameters.AddWithValue("p_newdtrcode", Convert.ToString(objswap.sNewDtrCode ?? ""));
                dt = ObjCon.FetchDataTable(cmd_tbltcmaster_details_for_validate);

                if (dt.Rows.Count > 0)
                {
                    string tcstatus = dt.Rows[0]["TC_STATUS"].ToString();
                    string tcCurentLocation = dt.Rows[0]["TC_CURRENT_LOCATION"].ToString();
                    string tcLocationId = dt.Rows[0]["TC_LOCATION_ID"].ToString();
                    string storeId = dt.Rows[0]["TC_STORE_ID"].ToString();

                    // if DTR has been failed (maybe if the status is empty )
                    if (tcstatus == "3")
                    {
                        Arr[0] = "2";
                        Arr[1] = "DTr has been failed";
                        return Arr;
                    }

                    //if tc has been scrapped 
                    if (tcstatus == "4")
                    {
                        Arr[0] = "2";
                        Arr[1] = "DTr has been scraped";
                        return Arr;
                    }

                    //if the DTR is in interstore transaction 
                    if (tcCurentLocation == "4")
                    {
                        Arr[0] = "2";
                        Arr[1] = "DTr is in Interstore transfer so cant allocate  ";
                        return Arr;
                    }

                    //if any of the field has null value 
                    if (tcCurentLocation.Length == 0 || tcLocationId.Length == 0 || storeId.Length == 0)
                    {
                        Arr[0] = "2";
                        Arr[1] = "Store Id  Location Id / Current Location  is empty please contact backend team ";
                        return Arr;
                    }
                    //if location code and current location doesnt match 
                    if ((tcCurentLocation == "1" && tcLocationId.Length == 5) ||
                        (tcCurentLocation == "2" && tcLocationId.Length != 5))
                    {
                        Arr[0] = "2";
                        Arr[1] = "There may be some problem with current location ";
                        Arr[1] += "and location id please contact backend team ";
                        return Arr;
                    }
                    // if tc has been send to repaircentre  
                    if (tcCurentLocation == "3")
                    {
                        string sPoNo = string.Empty;

                        #region Old inline query
                        //strQry1 = " SELECT \"RSM_PO_NO\" FROM \"TBLREPAIRSENTDETAILS\",\"TBLREPAIRSENTMASTER\" ";
                        //strQry1 += " WHERE \"RSD_DELIVARY_DATE\" IS NULL and \"RSD_RSM_ID\" = \"RSM_ID\" ";
                        //strQry1 += " AND \"RSD_TC_CODE\" = '" + objswap.sNewDtrCode + "' ";
                        //sPoNo = ObjCon.get_value(strQry1);
                        #endregion

                        QryKey = "GET_RSM_PO_NO";
                        NpgsqlCommand cmd_WO_NO = new NpgsqlCommand("fetch_getvalue_for_swapdetails");
                        cmd_WO_NO.Parameters.AddWithValue("p_key", QryKey);
                        cmd_WO_NO.Parameters.AddWithValue("p_value_1", Convert.ToString(objswap.sNewDtrCode ?? ""));
                        cmd_WO_NO.Parameters.AddWithValue("p_value_2", "");
                        sPoNo = ObjBseCon.StringGetValue(cmd_WO_NO);

                        Arr[0] = "2";
                        Arr[1] = "DTr CODE / DTr PLATE / Unique Id has been sent to repair centre with PO NO " + sPoNo + "";
                        return Arr;
                    }
                }

                #endregion

                #region NewDtc Notthere
                if (objswap.sNewDTCCode == null || objswap.sNewDTCCode == "")
                {

                    #region Old inline query
                    //strQry1 = " SELECT \"DT_CODE\",\"DT_NAME\",\"DT_OM_SLNO\",\"DT_TC_ID\",\"TC_SLNO\",\"TC_CAPACITY\", ";
                    //strQry1 += " \"TC_CURRENT_LOCATION\" FROM \"TBLDTCMAST\",\"TBLTCMASTER\" ";
                    //strQry1 += " WHERE \"TC_CODE\"=\"DT_TC_ID\" AND \"TC_CODE\"='" + objswap.sNewDtrCode + "' ";
                    //dt = ObjCon.FetchDataTable(strQry1);
                    #endregion

                    NpgsqlCommand cmd_dtc_notthere = new NpgsqlCommand("proc_get_dtr_dtc_details_for_swap");
                    cmd_dtc_notthere.Parameters.AddWithValue("p_newdtrcode", Convert.ToString(objswap.sNewDtrCode ?? ""));
                    dt = ObjCon.FetchDataTable(cmd_dtc_notthere);

                    if (dt.Rows.Count > 0)
                    {
                        string[] ArrNewDtc_Notthere = new string[2];
                        long UNA_SL_NO = ObjCon.Get_max_no("UNA_SL_NO", "TBLUNALLOCATEDDTCS");
                        string sDTC_Code = dt.Rows[0]["DT_CODE"].ToString();

                        ObjCon.BeginTransaction();
                        #region old Inine query
                        //string sDfId = string.Empty;
                        //strQry = "select \"DF_ID\" FROM \"TBLDTCFAILURE\" WHERE \"DF_DTC_CODE\"='" + sDTC_Code + "'";
                        //sDfId = ObjCon.get_value(strQry);

                        ////ObjCon.BeginTransaction();

                        //if (sDfId == "")
                        //{
                        //    strQry = " INSERT INTO \"TBLUNALLOCATEDDTCS\" (\"UNA_SL_NO\",\"UNA_MISMATCHENTRY_SLNO\", ";
                        //    strQry += " \"UNA_DTCCODE\",\"UNA_ENTRY_FROM\",\"UNA_IS_DTC_FAILURE\") ";
                        //    strQry += " VALUES ('" + UNA_SL_NO + "','" + sDTC_SL_NO + "','" + sDTC_Code + "','DTCMISMATCHENTRY','NO') ";
                        //    ObjCon.ExecuteQry(strQry);
                        //}
                        //else
                        //{
                        //    strQry = " INSERT INTO \"TBLUNALLOCATEDDTCS\" (\"UNA_SL_NO\",\"UNA_MISMATCHENTRY_SLNO\", ";
                        //    strQry += " \"UNA_DTCCODE\",\"UNA_ENTRY_FROM\",\"UNA_IS_DTC_FAILURE\",\"UNA_FAILURE_ID\") ";
                        //    strQry += " VALUES ('" + UNA_SL_NO + "','" + sDTC_SL_NO + "',";
                        //    strQry += " '" + sDTC_Code + "','DTCMISMATCHENTRY','YES','" + sDfId + "')";
                        //    ObjCon.ExecuteQry(strQry);
                        //}

                        //strQry = " UPDATE \"TBLTRANSDTCMAPPING\" SET \"TM_LIVE_FLAG\"='0',\"TM_UNMAP_CRON\"=now(), ";
                        //strQry += " \"TM_UNMAP_CRBY\" = '" + objswap.sCrBy + "', ";
                        //strQry += " \"TM_UNMAP_REASON\"='CORRECTION OF Dtc CODE BY DTCMISMATCHENTRY' WHERE ";
                        //strQry += " \"TM_DTC_ID\"='" + sDTC_Code + "' ";
                        //strQry += " AND \"TM_LIVE_FLAG\"=1 ";
                        //ObjCon.ExecuteQry(strQry);

                        //strQry = "UPDATE \"TBLDTCMAST\" SET \"DT_TC_ID\"='0' WHERE \"DT_CODE\"='" + sDTC_Code + "'";

                        //ObjCon.ExecuteQry(strQry);
                        #endregion

                        NpgsqlCommand cmd_newdtc_notthere = new NpgsqlCommand("proc_saveupdate_newdtc_notthere_swap");
                        cmd_newdtc_notthere.Parameters.AddWithValue("p_dtccode", Convert.ToString(sDTC_Code ?? ""));
                        cmd_newdtc_notthere.Parameters.AddWithValue("p_una_sl_no", Convert.ToInt64(UNA_SL_NO));
                        cmd_newdtc_notthere.Parameters.AddWithValue("p_dtc_sl_no", Convert.ToInt64(sDTC_SL_NO));
                        cmd_newdtc_notthere.Parameters.AddWithValue("p_crby", Convert.ToString(objswap.sCrBy ?? ""));
                        cmd_newdtc_notthere.Parameters.Add("op_id", NpgsqlDbType.Text);
                        cmd_newdtc_notthere.Parameters.Add("msg", NpgsqlDbType.Text);
                        cmd_newdtc_notthere.Parameters["op_id"].Direction = ParameterDirection.Output;
                        cmd_newdtc_notthere.Parameters["msg"].Direction = ParameterDirection.Output;
                        ArrNewDtc_Notthere[1] = "op_id";
                        ArrNewDtc_Notthere[0] = "msg";
                        ArrNewDtc_Notthere = ObjCon.Execute(cmd_newdtc_notthere, ArrNewDtc_Notthere, 2);
                        ObjCon.CommitTransaction();
                    }

                    ObjCon.BeginTransaction();

                    #region old inline query
                    //strQry = "INSERT INTO \"TBLDTCMISMATCHENTRY\" (\"DME_SL_NO\",\"DME_DTC_CODE\",";
                    //strQry += "\"DME_EXISTING_DTR_CODE\",\"DME_NEW_DTR_CODE\",";
                    //strQry += "\"DME_NEWDTRCODE_OLDLOC_TYPE\", ";
                    //strQry += " \"DME_NEWDTRCODE_OLDLOC_REFNO\",\"DME_ENTRY_DATE\",\"DME_REMARKS\",";
                    //strQry += "\"DME_CREATED_BY\" ) VALUES ";
                    //strQry += "('" + sDTC_SL_NO + "','" + objswap.sDtcCode + "',";
                    //strQry += " '" + objswap.sDtrCode + "','" + objswap.sNewDtrCode + "','" + objswap.sLocType + "'";
                    //strQry += ",'" + objswap.sOfficeCode + "',now(),";
                    //strQry += "'" + objswap.sRemarks + "','" + objswap.sCrBy + "')";
                    //ObjCon.ExecuteQry(strQry);

                    //// long sMaxNo = ObjCon.Get_max_no("UAD_SL_NO", "TBLUNALLOCATEDDTRS");
                    //string qry = "SELECT COALESCE(MAX(\"UAD_SL_NO\"),0)+1 FROM \"TBLUNALLOCATEDDTRS\"";
                    //string sMaxNo = ObjCon.get_value(qry);

                    //strQry = "INSERT INTO \"TBLUNALLOCATEDDTRS\" (\"UAD_SL_NO\",\"UAD_SL_MISMATCHENTRY_SLNO\",";
                    //strQry += "\"UAD_DTRCODE\",\"UAD_ENTRY_FROM\")";
                    //strQry += " VALUES ('" + sMaxNo + "','" + sDTC_SL_NO + "',";
                    //strQry += " '" + objswap.sDtrCode + "','DTCMISMATCHENTRY')";
                    //ObjCon.ExecuteQry(strQry);

                    //strQry = "UPDATE \"TBLDTCMAST\" SET \"DT_TC_ID\"='" + objswap.sNewDtrCode + "' ";
                    //strQry += "WHERE \"DT_CODE\"='" + objswap.sDtcCode + "'";
                    //ObjCon.ExecuteQry(strQry);

                    //strQry = "UPDATE \"TBLTCMASTER\" SET \"TC_LOCATION_ID\"='" + objswap.sOfficeCode + "',";
                    //strQry += "\"TC_CURRENT_LOCATION\"='2' WHERE ";
                    //strQry += "\"TC_CODE\"='" + objswap.sNewDtrCode + "'";
                    //ObjCon.ExecuteQry(strQry);

                    ////DtmmsWebService.Service1Client objWCF = new DtmmsWebService.Service1Client();
                    ////objWCF.SaveTcDetails(strQry);

                    //strQry = " UPDATE \"TBLTRANSDTCMAPPING\" SET \"TM_LIVE_FLAG\"='0',\"TM_UNMAP_CRON\"=now(),";
                    //strQry += "\"TM_UNMAP_CRBY\"='" + objswap.sCrBy + "',";
                    //strQry += "\"TM_UNMAP_REASON\"='CORRECTION OF DTR CODE BY DTCMISMATCHENTRY' WHERE ";
                    //strQry += "\"TM_DTC_ID\"='" + objswap.sDtcCode + "'";
                    //strQry += "AND \"TM_LIVE_FLAG\"=1";
                    //ObjCon.ExecuteQry(strQry);

                    //long sMaxmapping = ObjCon.Get_max_no("TM_ID", "TBLTRANSDTCMAPPING");

                    //strQry = "INSERT INTO \"TBLTRANSDTCMAPPING\" (\"TM_ID\",\"TM_MAPPING_DATE\",\"TM_TC_ID\",";
                    //strQry += "\"TM_DTC_ID\",\"TM_LIVE_FLAG\",\"TM_CRBY\",";
                    //strQry += "\"TM_CRON\") VALUES ('" + sMaxmapping + "',";
                    //strQry += "now(),'" + objswap.sNewDtrCode + "','" + objswap.sDtcCode + "','1',";
                    //strQry += "'" + objswap.sCrBy + "',now())";
                    //ObjCon.ExecuteQry(strQry);

                    //strQry = " UPDATE \"TBLTRANSDTCMAPPING\" SET \"TM_LIVE_FLAG\"='0',\"TM_UNMAP_CRON\"=now(),";
                    //strQry += "\"TM_UNMAP_CRBY\"='" + objswap.sCrBy + "',";
                    //strQry += "\"TM_UNMAP_REASON\"='CORRECTION OF DTR CODE BY DTCMISMATCHENTRY' WHERE \"TM_TC_ID\"=";
                    //strQry += "'" + objswap.sDtrCode + "' AND ";
                    //strQry += "\"TM_LIVE_FLAG\"=1";
                    //ObjCon.ExecuteQry(strQry);

                    //strQry = "INSERT INTO \"TBLDTCTRANSACTION\" (\"DCT_ID\",\"DCT_DTC_CODE\",\"DCT_DTR_CODE\",";
                    //strQry += "\"DCT_DTR_STATUS\",\"DCT_TRANS_DATE\",";
                    //strQry += "\"DCT_ACT_REFNO\",\"DCT_ACT_REFTYPE\",\"DCT_DESC\",\"DCT_ENTRYDATE\",";
                    //strQry += "\"DCT_CANCEL_FLAG\") ";
                    //strQry += "VALUES((SELECT COALESCE(max(\"DCT_ID\"),0)+1 FROM \"TBLDTCTRANSACTION\"),";
                    //strQry += "'" + objswap.sDtcCode + "',";
                    //strQry += "'" + objswap.sNewDtrCode + "','1',now(),'','1','NEW DTC COMMISSIONED ";
                    //strQry += "(FROM BACKEND)',now(),'0')";
                    //ObjCon.ExecuteQry(strQry);

                    //strQry = "INSERT INTO \"TBLDTRTRANSACTION\" (\"DRT_ID\",\"DRT_DTR_CODE\",\"DRT_LOC_ID\",";
                    //strQry += "\"DRT_LOC_TYPE\",\"DRT_TRANS_DATE\",";
                    //strQry += "\"DRT_ACT_REFNO\",\"DRT_ACT_REFTYPE\",\"DRT_DESC\",\"DRT_DTR_STATUS\",";
                    //strQry += "\"DRT_ENTRYDATE\",\"DRT_CANCEL_FLAG\") VALUES ";
                    //strQry += "((SELECT COALESCE(max(\"DRT_ID\"),0)+1 FROM \"TBLDTRTRANSACTION\"),";
                    //strQry += "'" + objswap.sNewDtrCode + "',";
                    //strQry += "'" + objswap.sOfficeCode + "','2',now(),'','2','COMMISSIONED TO DTCCODE :";
                    //strQry += "" + objswap.sDtcCode + " (FROM BACKEND)','1',now(),'0')";
                    //ObjCon.ExecuteQry(strQry);
                    //Arr[0] = "1";
                    //Arr[1] = "Allocate Succesfully";
                    #endregion

                    NpgsqlCommand cmd = new NpgsqlCommand("proc_saveupdate_swap_newdtc_notthere");
                    cmd.Parameters.AddWithValue("p_dtcsl_no", Convert.ToInt64(sDTC_SL_NO));
                    cmd.Parameters.AddWithValue("p_dtccode", Convert.ToString(objswap.sDtcCode ?? ""));
                    cmd.Parameters.AddWithValue("p_dtrcode", Convert.ToString(objswap.sDtrCode ?? ""));
                    cmd.Parameters.AddWithValue("p_newdtrcode", Convert.ToString(objswap.sNewDtrCode ?? ""));
                    cmd.Parameters.AddWithValue("p_loctype", Convert.ToString(objswap.sLocType ?? ""));
                    cmd.Parameters.AddWithValue("p_officecode", Convert.ToString(objswap.sOfficeCode ?? ""));
                    cmd.Parameters.AddWithValue("p_remarks", Convert.ToString(objswap.sRemarks ?? ""));
                    cmd.Parameters.AddWithValue("p_crby", Convert.ToString(objswap.sCrBy ?? ""));
                    cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                    cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                    Arr[0] = "op_id";
                    Arr[1] = "msg";
                    Arr = ObjCon.Execute(cmd, Arr, 2);
                    ObjCon.CommitTransaction();
                    return Arr;
                }
                #endregion                
                #region NewDTc Given
                else
                {

                    if (objswap.sDtrCode == "0")
                    {
                        Arr[0] = "2";
                        Arr[1] = objswap.sDtcCode + " DTC code Not Mapped with Any DTr code.";
                        return Arr;
                    }

                    #region old inline query
                    //strQry = " SELECT \"DT_CODE\",\"DT_NAME\",\"DT_OM_SLNO\",\"DT_TC_ID\",\"TC_SLNO\",\"TC_CAPACITY\", ";
                    //strQry += " \"TC_CURRENT_LOCATION\" FROM \"TBLDTCMAST\",\"TBLTCMASTER\" WHERE \"TC_CODE\"=\"DT_TC_ID\" ";
                    //strQry += " AND \"TC_CODE\"='" + objswap.sNewDtrCode + "' ";
                    //dtDtcDetails = ObjCon.FetchDataTable(strQry);
                    #endregion

                    NpgsqlCommand cmdnewdtcnotthere = new NpgsqlCommand("proc_get_newdtcnotthere_overall_details");
                    cmdnewdtcnotthere.Parameters.AddWithValue("p_newdtrcode", Convert.ToString(objswap.sNewDtrCode ?? ""));
                    dtDtcDetails = ObjCon.FetchDataTable(cmdnewdtcnotthere);

                    if (dtDtcDetails.Rows.Count > 0)
                    {
                        #region old inline query
                        //strQry = "INSERT INTO \"TBLDTCMISMATCHENTRY\" (\"DME_SL_NO\",\"DME_DTC_CODE\",";
                        //strQry += "\"DME_EXISTING_DTR_CODE\",";
                        //strQry += "\"DME_NEW_DTR_CODE\",";
                        //strQry += "\"DME_NEWDTRCODE_OLDLOC_TYPE\", ";
                        //strQry += " \"DME_NEWDTRCODE_OLDLOC_REFNO\",\"DME_ENTRY_DATE\",\"DME_REMARKS\",\"DME_CREATED_BY\",";
                        //strQry += "\"DME_NEW_DTC_CODE\",";
                        //strQry += "\"DME_EXIST_DTRCODE_NEWLOCTYPE\",\"DME_EXIST_DTRCODE_NEWLOC_REFNO\" ) VALUES ";
                        //strQry += "('" + sDTC_SL_NO + "','" + objswap.sDtcCode + "',";
                        //strQry += " '" + objswap.sDtrCode + "','" + objswap.sNewDtrCode + "','" + objswap.sLocType + "',";
                        //strQry += "'" + objswap.sOfficeCode + "',now(),";
                        //strQry += "'" + objswap.sRemarks + "','" + objswap.sCrBy + "','" + objswap.sNewDTCCode + "',";
                        //strQry += " '" + dtDtcDetails.Rows[0]["TC_CURRENT_LOCATION"] + "',";
                        //strQry += "'" + dtDtcDetails.Rows[0]["DT_OM_SLNO"] + "' )";
                        //ObjCon.ExecuteQry(strQry);
                        #endregion

                        NpgsqlCommand cmdnewdtcnotthere_overall = new NpgsqlCommand("proc_saveupdate_newdtcnotthere_overall");
                        cmdnewdtcnotthere_overall.Parameters.AddWithValue("p_dtc_sl_no", Convert.ToInt64(sDTC_SL_NO));
                        cmdnewdtcnotthere_overall.Parameters.AddWithValue("p_dtccode", Convert.ToString(objswap.sDtcCode ?? ""));
                        cmdnewdtcnotthere_overall.Parameters.AddWithValue("p_dtrcode", Convert.ToString(objswap.sDtrCode ?? ""));
                        cmdnewdtcnotthere_overall.Parameters.AddWithValue("p_newdtrcode", Convert.ToString(objswap.sNewDtrCode ?? ""));
                        cmdnewdtcnotthere_overall.Parameters.AddWithValue("p_loctype", Convert.ToString(objswap.sLocType ?? ""));
                        cmdnewdtcnotthere_overall.Parameters.AddWithValue("p_officecode", Convert.ToString(objswap.sOfficeCode ?? ""));
                        cmdnewdtcnotthere_overall.Parameters.AddWithValue("p_remarks", Convert.ToString(objswap.sRemarks ?? ""));
                        cmdnewdtcnotthere_overall.Parameters.AddWithValue("p_crby", Convert.ToString(objswap.sCrBy ?? ""));
                        cmdnewdtcnotthere_overall.Parameters.AddWithValue("p_newdtccode", Convert.ToString(objswap.sNewDTCCode ?? ""));
                        cmdnewdtcnotthere_overall.Parameters.AddWithValue("p_tc_current_location", Convert.ToString(dtDtcDetails.Rows[0]["TC_CURRENT_LOCATION"] ?? ""));
                        cmdnewdtcnotthere_overall.Parameters.AddWithValue("p_dt_om_slno", Convert.ToString(dtDtcDetails.Rows[0]["DT_OM_SLNO"] ?? ""));
                        cmdnewdtcnotthere_overall.Parameters.Add("op_id", NpgsqlDbType.Text);
                        cmdnewdtcnotthere_overall.Parameters.Add("msg", NpgsqlDbType.Text);
                        cmdnewdtcnotthere_overall.Parameters["op_id"].Direction = ParameterDirection.Output;
                        cmdnewdtcnotthere_overall.Parameters["msg"].Direction = ParameterDirection.Output;
                        Arr[0] = "op_id";
                        Arr[1] = "msg";
                        Arr = ObjCon.Execute(cmdnewdtcnotthere_overall, Arr, 2);

                    }
                    else
                    {
                        #region old inline query
                        //strQry = "SELECT \"TC_SLNO\",\"TC_CAPACITY\",\"TC_CURRENT_LOCATION\" FROM \"TBLTCMASTER\" WHERE  ";
                        //strQry += "\"TC_CODE\"='" + objswap.sNewDtrCode + "'";
                        //dt = ObjCon.FetchDataTable(strQry);
                        #endregion

                        NpgsqlCommand cmdtbltcmaster = new NpgsqlCommand("proc_get_tcmaster_dtls_for_swapdetails");
                        cmdtbltcmaster.Parameters.AddWithValue("p_newdtrcode", Convert.ToString(objswap.sNewDtrCode ?? ""));
                        dt = ObjCon.FetchDataTable(cmdtbltcmaster);

                    }

                    #region old inline query
                    //strQry = "SELECT * FROM \"TBLDTCMISMATCHENTRY\" WHERE \"DME_DTC_CODE\"='" + objswap.sDtcCode + "' ";
                    //strQry += "AND  \"DME_NEW_DTR_CODE\"=";
                    //strQry += "'" + objswap.sNewDtrCode + "'";
                    //strQry += " AND \"DME_NEW_DTC_CODE\"='" + objswap.sNewDTCCode + "' AND \"DME_EXISTING_DTR_CODE\"";
                    //strQry += "='" + objswap.sDtrCode + "' ";
                    //strQry += "AND \"DME_SL_NO\"='" + sDTC_SL_NO + "'";
                    //dt1 = ObjCon.FetchDataTable(strQry);
                    #endregion

                    NpgsqlCommand cmd = new NpgsqlCommand("proc_get_dtcmismatchentry_for_swapdetails");
                    cmd.Parameters.AddWithValue("p_dtc_sl_no", Convert.ToInt64(sDTC_SL_NO));
                    cmd.Parameters.AddWithValue("p_dtccode", Convert.ToString(objswap.sDtcCode ?? ""));
                    cmd.Parameters.AddWithValue("p_dtrcode", Convert.ToString(objswap.sDtrCode ?? ""));
                    cmd.Parameters.AddWithValue("p_newdtrcode", Convert.ToString(objswap.sNewDtrCode ?? ""));
                    cmd.Parameters.AddWithValue("p_newdtccode", Convert.ToString(objswap.sNewDTCCode ?? ""));
                    dt1 = ObjCon.FetchDataTable(cmd);


                    string res = string.Empty;
                    #region old inline query
                    //strQry = "SELECT \"DT_TC_ID\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"='" + objswap.sNewDTCCode + "'";
                    //res = ObjCon.get_value(strQry);
                    #endregion

                    QryKey = "GET_DT_TC_ID";
                    NpgsqlCommand cmd_DT_TC_ID = new NpgsqlCommand("fetch_getvalue_for_swapdetails");
                    cmd_DT_TC_ID.Parameters.AddWithValue("p_key", QryKey);
                    cmd_DT_TC_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(objswap.sNewDTCCode ?? ""));
                    cmd_DT_TC_ID.Parameters.AddWithValue("p_value_2", "");
                    res = ObjBseCon.StringGetValue(cmd_DT_TC_ID);


                    if ((dt1.Rows.Count > 0 && res == objswap.sNewDtrCode) || (dt1.Rows.Count > 0 && res == objswap.sDtrCode))
                    {
                        string sNewDTC_TC = string.Empty;
                        #region old inline query

                        //strQry = "SELECT \"DT_TC_ID\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"='" + objswap.sNewDTCCode + "'";
                        //sNewDTC_TC = ObjCon.get_value(strQry);

                        //strQry = "UPDATE \"TBLDTCMAST\" SET \"DT_TC_ID\"='" + objswap.sNewDtrCode + "' WHERE \"DT_CODE\"=";
                        //strQry += "'" + objswap.sDtcCode + "'";
                        //ObjCon.ExecuteQry(strQry);

                        //strQry = "UPDATE \"TBLTCMASTER\" SET \"TC_LOCATION_ID\"='" + objswap.sOfficeCode + "',";
                        //strQry += "\"TC_CURRENT_LOCATION\" ='2' WHERE \"TC_CODE\"=";
                        //strQry += "'" + objswap.sNewDtrCode + "'";
                        //ObjCon.ExecuteQry(strQry);

                        ////DtmmsWebService.Service1Client objWCF = new DtmmsWebService.Service1Client();
                        ////objWCF.SaveTcDetails(strQry);

                        //strQry = "UPDATE \"TBLDTCMAST\" SET \"DT_TC_ID\"='" + objswap.sDtrCode + "' WHERE ";
                        //strQry += "\"DT_CODE\"='" + objswap.sNewDTCCode + "'";
                        //ObjCon.ExecuteQry(strQry);

                        //strQry = "UPDATE \"TBLTCMASTER\" SET \"TC_LOCATION_ID\"='" + objswap.sOfficeCode + "',";
                        //strQry += "\"TC_CURRENT_LOCATION\"='2' WHERE ";
                        //strQry += "\"TC_CODE\"='" + objswap.sDtrCode + "'";
                        //ObjCon.ExecuteQry(strQry);

                        ////DtmmsWebService.Service1Client objWCF = new DtmmsWebService.Service1Client();
                        ////objWCF.SaveTcDetails(strQry);

                        //strQry = "UPDATE \"TBLTRANSDTCMAPPING\" SET \"TM_LIVE_FLAG\"='0',\"TM_UNMAP_CRON\"=now(),";
                        //strQry += "\"TM_UNMAP_CRBY\"='" + objswap.sCrBy + "',";
                        //strQry += "\"TM_UNMAP_REASON\"='CORRECTION OF DTR CODE BY DTCMISMATCHENTRY' WHERE ";
                        //strQry += "\"TM_TC_ID\"='" + sNewDTC_TC + "' ";
                        //strQry += "AND \"TM_LIVE_FLAG\"=1 ";
                        //ObjCon.ExecuteQry(strQry);

                        //strQry = " UPDATE \"TBLTRANSDTCMAPPING\" SET \"TM_LIVE_FLAG\"='0',\"TM_UNMAP_CRON\"=now(),";
                        //strQry += "\"TM_UNMAP_CRBY\"='" + objswap.sCrBy + "',";
                        //strQry += "\"TM_UNMAP_REASON\"='CORRECTION OF DTR CODE BY DTCMISMATCHENTRY' WHERE \"TM_DTC_ID\"=";
                        //strQry += "'" + objswap.sDtcCode + "' AND ";
                        //strQry += "\"TM_LIVE_FLAG\"=1";
                        //ObjCon.ExecuteQry(strQry);

                        //long sMaxmapping = ObjCon.Get_max_no("TM_ID", "TBLTRANSDTCMAPPING");

                        //strQry = "INSERT INTO \"TBLTRANSDTCMAPPING\" (\"TM_ID\",\"TM_MAPPING_DATE\",\"TM_TC_ID\",";
                        //strQry += "\"TM_DTC_ID\",\"TM_LIVE_FLAG\",\"TM_CRBY\",";
                        //strQry += "\"TM_CRON\") VALUES ('" + sMaxmapping + "',";
                        //strQry += "now(),'" + objswap.sNewDtrCode + "','" + objswap.sDtcCode + "','1',";
                        //strQry += "'" + objswap.sCrBy + "',now())";
                        //ObjCon.ExecuteQry(strQry);

                        //strQry = " UPDATE \"TBLTRANSDTCMAPPING\" SET \"TM_LIVE_FLAG\"='0',\"TM_UNMAP_CRON\"=now(),";
                        //strQry += "\"TM_UNMAP_CRBY\"='" + objswap.sCrBy + "',";
                        //strQry += "\"TM_UNMAP_REASON\"='CORRECTION OF DTR CODE BY DTCMISMATCHENTRY' WHERE \"TM_TC_ID\"=";
                        //strQry += "'" + objswap.sDtrCode + "' AND ";
                        //strQry += "\"TM_LIVE_FLAG\"=1";
                        //ObjCon.ExecuteQry(strQry);

                        //long sMaxDtcmapping = ObjCon.Get_max_no("TM_ID", "TBLTRANSDTCMAPPING");

                        //strQry = "INSERT INTO \"TBLTRANSDTCMAPPING\" (\"TM_ID\",\"TM_MAPPING_DATE\",\"TM_TC_ID\",\"TM_DTC_ID\",";
                        //strQry += "\"TM_LIVE_FLAG\",\"TM_CRBY\",";
                        //strQry += "\"TM_CRON\") VALUES ('" + sMaxDtcmapping + "',";
                        //strQry += "now(),'" + objswap.sDtrCode + "','" + objswap.sNewDTCCode + "','1','" + objswap.sCrBy + "',now())";
                        //ObjCon.ExecuteQry(strQry);

                        //strQry = "INSERT INTO \"TBLDTCTRANSACTION\" (\"DCT_ID\",\"DCT_DTC_CODE\",\"DCT_DTR_CODE\",";
                        //strQry += "\"DCT_DTR_STATUS\",\"DCT_TRANS_DATE\",";
                        //strQry += "\"DCT_ACT_REFNO\",\"DCT_ACT_REFTYPE\",\"DCT_DESC\",\"DCT_ENTRYDATE\",\"DCT_CANCEL_FLAG\") VALUES ";
                        //strQry += "((SELECT COALESCE(max(\"DCT_ID\"),0)+1 FROM \"TBLDTCTRANSACTION\"),'" + objswap.sDtcCode + "',";
                        //strQry += "'" + objswap.sNewDtrCode + "','1',now(),'','1','NEW DTC COMMISSIONED (FROM BACKEND)',now(),'0')";
                        //ObjCon.ExecuteQry(strQry);

                        //strQry = "INSERT INTO \"TBLDTRTRANSACTION\" (\"DRT_ID\",\"DRT_DTR_CODE\",\"DRT_LOC_ID\",\"DRT_LOC_TYPE\",";
                        //strQry += "\"DRT_TRANS_DATE\",";
                        //strQry += "\"DRT_ACT_REFNO\",\"DRT_ACT_REFTYPE\",\"DRT_DESC\",\"DRT_DTR_STATUS\",\"DRT_ENTRYDATE\",";
                        //strQry += "\"DRT_CANCEL_FLAG\") VALUES ";
                        //strQry += "((SELECT COALESCE(max(\"DRT_ID\"),0)+1 FROM \"TBLDTRTRANSACTION\"),'" + objswap.sNewDtrCode + "',";
                        //strQry += "'" + objswap.sOfficeCode + "','2',now(),'','2','COMMISSIONED TO DTCCODE :" + objswap.sDtcCode + " ";
                        //strQry += "(FROM BACKEND)','1',now(),'0')";
                        //ObjCon.ExecuteQry(strQry);

                        //Arr[0] = "1";
                        //Arr[1] = "Allocate Succesfully";
                        #endregion

                        NpgsqlCommand cmdon_second_dt = new NpgsqlCommand("proc_saveupdate_newdtcnotthere_on_second_dt");
                        cmdon_second_dt.Parameters.AddWithValue("p_newdtccode", Convert.ToString(objswap.sNewDTCCode ?? ""));
                        cmdon_second_dt.Parameters.AddWithValue("p_newdtrcode", Convert.ToString(objswap.sNewDtrCode ?? ""));
                        cmdon_second_dt.Parameters.AddWithValue("p_dtccode", Convert.ToString(objswap.sDtcCode ?? ""));
                        cmdon_second_dt.Parameters.AddWithValue("p_officecode", Convert.ToString(objswap.sOfficeCode ?? ""));
                        cmdon_second_dt.Parameters.AddWithValue("p_dtrcode", Convert.ToString(objswap.sDtrCode ?? ""));
                        cmdon_second_dt.Parameters.AddWithValue("p_crby", Convert.ToString(objswap.sCrBy ?? ""));
                        cmdon_second_dt.Parameters.Add("op_id", NpgsqlDbType.Text);
                        cmdon_second_dt.Parameters.Add("msg", NpgsqlDbType.Text);
                        cmdon_second_dt.Parameters["op_id"].Direction = ParameterDirection.Output;
                        cmdon_second_dt.Parameters["msg"].Direction = ParameterDirection.Output;
                        Arr[0] = "op_id";
                        Arr[1] = "msg";
                        Arr = ObjCon.Execute(cmdon_second_dt, Arr, 2);

                        return Arr;
                    }
                    else
                    {

                        if (dtDtcDetails.Rows.Count > 0)
                        {
                            #region old inline query

                            //long UNA_SL_NO = ObjCon.Get_max_no("UNA_SL_NO", "TBLUNALLOCATEDDTCS");
                            //string sDTC_Code = dtDtcDetails.Rows[0]["DT_CODE"].ToString();


                            //strQry = "select \"DF_ID\" FROM \"TBLDTCFAILURE\" WHERE \"DF_DTC_CODE\"='" + sDTC_Code + "'";
                            //string sDfId = ObjCon.get_value(strQry);

                            //if (sDfId == "")
                            //{
                            //    strQry = "INSERT INTO \"TBLUNALLOCATEDDTCS\" (\"UNA_SL_NO\",\"UNA_MISMATCHENTRY_SLNO\",\"UNA_DTCCODE\",";
                            //    strQry += "\"UNA_ENTRY_FROM\",\"UNA_IS_DTC_FAILURE\")";
                            //    strQry += " VALUES ('" + UNA_SL_NO + "','" + sDTC_SL_NO + "',";
                            //    strQry += " '" + sDTC_Code + "','DTCMISMATCHENTRY','NO')";
                            //    ObjCon.ExecuteQry(strQry);
                            //}
                            //else
                            //{
                            //    strQry = "INSERT INTO \"TBLUNALLOCATEDDTCS\" (\"UNA_SL_NO\",\"UNA_MISMATCHENTRY_SLNO\",";
                            //    strQry += "\"UNA_DTCCODE\",\"UNA_ENTRY_FROM\",";
                            //    strQry += "\"UNA_IS_DTC_FAILURE\",\"UNA_FAILURE_ID\")";
                            //    strQry += " VALUES ('" + UNA_SL_NO + "','" + sDTC_SL_NO + "',";
                            //    strQry += " '" + sDTC_Code + "','DTCMISMATCHENTRY','YES','" + sDfId + "')";
                            //    ObjCon.ExecuteQry(strQry);
                            //}

                            //strQry = " UPDATE \"TBLTRANSDTCMAPPING\" SET \"TM_LIVE_FLAG\"='0',\"TM_UNMAP_CRON\"=now(),";
                            //strQry += "\"TM_UNMAP_CRBY\"='" + objswap.sCrBy + "',";
                            //strQry += "\"TM_UNMAP_REASON\"='CORRECTION OF Dtc CODE BY DTCMISMATCHENTRY' WHERE ";
                            //strQry += "\"TM_DTC_ID\"='" + sDTC_Code + "' AND ";
                            //strQry += "\"TM_LIVE_FLAG\"=1";
                            //ObjCon.ExecuteQry(strQry);

                            //strQry = "UPDATE \"TBLDTCMAST\" SET \"DT_TC_ID\"='0' WHERE \"DT_CODE\"='" + sDTC_Code + "'";
                            //ObjCon.ExecuteQry(strQry);

                            #endregion

                            NpgsqlCommand cmdon_dtdtcdetails = new NpgsqlCommand("proc_saveupdate_newdtcnotthere_on_dtdtcdetails");
                            cmdon_dtdtcdetails.Parameters.AddWithValue("p_dtc_code", Convert.ToString(dtDtcDetails.Rows[0]["DT_CODE"] ?? ""));
                            cmdon_dtdtcdetails.Parameters.AddWithValue("p_dtc_sl_no", Convert.ToInt64(sDTC_SL_NO));
                            cmdon_dtdtcdetails.Parameters.AddWithValue("p_crby", Convert.ToString(objswap.sCrBy ?? ""));
                            cmdon_dtdtcdetails.Parameters.Add("op_id", NpgsqlDbType.Text);
                            cmdon_dtdtcdetails.Parameters.Add("msg", NpgsqlDbType.Text);
                            cmdon_dtdtcdetails.Parameters["op_id"].Direction = ParameterDirection.Output;
                            cmdon_dtdtcdetails.Parameters["msg"].Direction = ParameterDirection.Output;
                            Arr[0] = "op_id";
                            Arr[1] = "msg";
                            Arr = ObjCon.Execute(cmdon_dtdtcdetails, Arr, 2);
                        }

                        #region old inline query

                        //strQry = "SELECT \"DT_TC_ID\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"='" + objswap.sNewDTCCode + "'";
                        //string sNewDTC_TC = ObjCon.get_value(strQry);

                        //long sMaxNo = ObjCon.Get_max_no("UAD_SL_NO", "TBLUNALLOCATEDDTRS");

                        //strQry = "INSERT INTO \"TBLUNALLOCATEDDTRS\" (\"UAD_SL_NO\",\"UAD_SL_MISMATCHENTRY_SLNO\",";
                        //strQry += "\"UAD_DTRCODE\",\"UAD_ENTRY_FROM\")";
                        //strQry += " VALUES ('" + sMaxNo + "','" + sDTC_SL_NO + "',";
                        //strQry += " '" + sNewDTC_TC + "','DTCMISMATCHENTRY')";
                        //ObjCon.ExecuteQry(strQry);

                        //strQry = "UPDATE \"TBLDTCMAST\" SET \"DT_TC_ID\"='" + objswap.sNewDtrCode + "' WHERE \"DT_CODE\"=";
                        //strQry += "'" + objswap.sDtcCode + "'";
                        //ObjCon.ExecuteQry(strQry);

                        //strQry = "UPDATE \"TBLTCMASTER\" SET \"TC_LOCATION_ID\"='" + objswap.sOfficeCode + "',";
                        //strQry += "\"TC_CURRENT_LOCATION\"='2' WHERE ";
                        //strQry += "\"TC_CODE\"='" + objswap.sNewDtrCode + "'";
                        //ObjCon.ExecuteQry(strQry);

                        ////DtmmsWebService.Service1Client objWCF = new DtmmsWebService.Service1Client();
                        ////objWCF.SaveTcDetails(strQry);

                        //strQry = "UPDATE \"TBLDTCMAST\" SET \"DT_TC_ID\"='" + objswap.sDtrCode + "' WHERE ";
                        //strQry += "\"DT_CODE\"='" + objswap.sNewDTCCode + "'";
                        //ObjCon.ExecuteQry(strQry);

                        //strQry = "UPDATE \"TBLTCMASTER\" SET \"TC_LOCATION_ID\"='" + objswap.sOfficeCode + "',";
                        //strQry += "\"TC_CURRENT_LOCATION\"='2' WHERE ";
                        //strQry += "\"TC_CODE\"='" + objswap.sDtrCode + "'";
                        //ObjCon.ExecuteQry(strQry);

                        ////DtmmsWebService.Service1Client objWCF = new DtmmsWebService.Service1Client();
                        ////objWCF.SaveTcDetails(strQry);

                        //strQry = "UPDATE \"TBLTRANSDTCMAPPING\" SET \"TM_LIVE_FLAG\"='0',\"TM_UNMAP_CRON\"=now(),\"TM_UNMAP_CRBY\"";
                        //strQry += "='" + objswap.sCrBy + "',";
                        //strQry += "\"TM_UNMAP_REASON\"='CORRECTION OF DTR CODE BY DTCMISMATCHENTRY' WHERE \"TM_TC_ID\"='" + sNewDTC_TC + "' ";
                        //strQry += "AND \"TM_LIVE_FLAG\"=1 ";
                        //ObjCon.ExecuteQry(strQry);

                        //strQry = " UPDATE \"TBLTRANSDTCMAPPING\" SET \"TM_LIVE_FLAG\"='0',\"TM_UNMAP_CRON\"=now(),\"TM_UNMAP_CRBY\"";
                        //strQry += "='" + objswap.sCrBy + "',";
                        //strQry += "\"TM_UNMAP_REASON\"='CORRECTION OF DTR CODE BY DTCMISMATCHENTRY' WHERE \"TM_DTC_ID\"=";
                        //strQry += "'" + objswap.sDtcCode + "' AND ";
                        //strQry += "\"TM_LIVE_FLAG\"=1";
                        //ObjCon.ExecuteQry(strQry);

                        //long sMaxmapping = ObjCon.Get_max_no("TM_ID", "TBLTRANSDTCMAPPING");

                        //strQry = "INSERT INTO \"TBLTRANSDTCMAPPING\" (\"TM_ID\",\"TM_MAPPING_DATE\",\"TM_TC_ID\",\"TM_DTC_ID\",";
                        //strQry += "\"TM_LIVE_FLAG\",";
                        //strQry += "\"TM_CRBY\",\"TM_CRON\") VALUES ('" + sMaxmapping + "',";
                        //strQry += "now(),'" + objswap.sNewDtrCode + "','" + objswap.sDtcCode + "','1','" + objswap.sCrBy + "',now())";
                        //ObjCon.ExecuteQry(strQry);

                        //strQry = " UPDATE \"TBLTRANSDTCMAPPING\" SET \"TM_LIVE_FLAG\"='0',\"TM_UNMAP_CRON\"=now(),";
                        //strQry += "\"TM_UNMAP_CRBY\"='" + objswap.sCrBy + "',";
                        //strQry += "\"TM_UNMAP_REASON\"='CORRECTION OF DTR CODE BY DTCMISMATCHENTRY' WHERE \"TM_TC_ID\"=";
                        //strQry += "'" + objswap.sDtrCode + "' AND ";
                        //strQry += "\"TM_LIVE_FLAG\"=1";
                        //ObjCon.ExecuteQry(strQry);

                        //long sMaxDtcmapping = ObjCon.Get_max_no("TM_ID", "TBLTRANSDTCMAPPING");

                        //strQry = "INSERT INTO \"TBLTRANSDTCMAPPING\" (\"TM_ID\",\"TM_MAPPING_DATE\",\"TM_TC_ID\",\"TM_DTC_ID\",";
                        //strQry += "\"TM_LIVE_FLAG\",\"TM_CRBY\",";
                        //strQry += "\"TM_CRON\") VALUES ('" + sMaxDtcmapping + "',";
                        //strQry += "now(),'" + objswap.sDtrCode + "','" + objswap.sNewDTCCode + "','1','" + objswap.sCrBy + "',now())";
                        //ObjCon.ExecuteQry(strQry);

                        //strQry = "INSERT INTO \"TBLDTCTRANSACTION\" (\"DCT_ID\",\"DCT_DTC_CODE\",\"DCT_DTR_CODE\",";
                        //strQry += "\"DCT_DTR_STATUS\",\"DCT_TRANS_DATE\",";
                        //strQry += "\"DCT_ACT_REFNO\",\"DCT_ACT_REFTYPE\",\"DCT_DESC\",\"DCT_ENTRYDATE\",\"DCT_CANCEL_FLAG\") ";
                        //strQry += "VALUES((SELECT COALESCE(max(\"DCT_ID\"),0)+1 FROM \"TBLDTCTRANSACTION\"),'" + objswap.sDtcCode + "',";
                        //strQry += "'" + objswap.sNewDtrCode + "','1',now(),'','1','NEW DTC COMMISSIONED (FROM BACKEND)',now(),'0')";
                        //ObjCon.ExecuteQry(strQry);

                        //strQry = "INSERT INTO \"TBLDTRTRANSACTION\" VALUES ((SELECT COALESCE(max(\"DRT_ID\"),0)+1 FROM ";
                        //strQry += "\"TBLDTRTRANSACTION\"),";
                        //strQry += "'" + objswap.sNewDtrCode + "',";
                        //strQry += "'" + objswap.sOfficeCode + "','2',now(),'','2','COMMISSIONED TO DTCCODE :";
                        //strQry += "" + objswap.sDtcCode + " (FROM BACKEND)',";
                        //strQry += "'1',now(),'0')";
                        //ObjCon.ExecuteQry(strQry);

                        //Arr[0] = "1";
                        //Arr[1] = "Allocate Succesfully";
                        #endregion

                        NpgsqlCommand cmdon_second_dt_isnull = new NpgsqlCommand("proc_saveupdate_newdtcnotthere_on_second_dt_isnull");
                        cmdon_second_dt_isnull.Parameters.AddWithValue("p_newdtccode", Convert.ToString(objswap.sNewDTCCode ?? ""));
                        cmdon_second_dt_isnull.Parameters.AddWithValue("p_dtc_sl_no", Convert.ToInt64(sDTC_SL_NO));
                        cmdon_second_dt_isnull.Parameters.AddWithValue("p_newdtrcode", Convert.ToString(objswap.sNewDtrCode ?? ""));
                        cmdon_second_dt_isnull.Parameters.AddWithValue("p_dtccode", Convert.ToString(objswap.sDtcCode ?? ""));
                        cmdon_second_dt_isnull.Parameters.AddWithValue("p_officecode", Convert.ToString(objswap.sOfficeCode ?? ""));
                        cmdon_second_dt_isnull.Parameters.AddWithValue("p_dtrcode", Convert.ToString(objswap.sDtrCode ?? ""));
                        cmdon_second_dt_isnull.Parameters.AddWithValue("p_crby", Convert.ToString(objswap.sCrBy ?? ""));
                        cmdon_second_dt_isnull.Parameters.Add("op_id", NpgsqlDbType.Text);
                        cmdon_second_dt_isnull.Parameters.Add("msg", NpgsqlDbType.Text);
                        cmdon_second_dt_isnull.Parameters["op_id"].Direction = ParameterDirection.Output;
                        cmdon_second_dt_isnull.Parameters["msg"].Direction = ParameterDirection.Output;
                        Arr[0] = "op_id";
                        Arr[1] = "msg";
                        Arr = ObjCon.Execute(cmdon_second_dt_isnull, Arr, 2);

                        return Arr;
                    }
                }
            }
            #endregion
            catch (Exception ex)
            {
                ObjCon.RollBackTrans();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                Arr[0] = "2";
                Arr[1] = "Error Occured";
                return Arr;
            }
            finally
            {

            }
        }
        /// <summary>
        /// Load UnAllocate Details
        /// </summary>
        /// <param name="OffCode"></param>
        /// <returns></returns>
        public DataTable LoadUnAllocateDetails(string OffCode)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                #region Old inline query
                //strQry = "SELECT \"DT_CODE\",\"DT_NAME\",0 AS \"TC_CODE\",TO_CHAR(\"DM_ENTRY_DATE\",'dd-mon-YYYY')";
                //strQry += "\"DM_ENTRY_DATE\",\"DT_OM_SLNO\",";
                //strQry += "(SELECT \"OFF_NAME\" AS SECTION FROM \"VIEW_OFFICES\" WHERE \"OFF_CODE\"";
                //strQry += "=SUBSTR(DT_OM_SLNO,0,5))SECTION FROM ";
                //strQry += "\"TBLUNALLOCATEDDTCS\",";
                //strQry += "\"TBLDTRMISMATCHENTRY\",\"TBLDTCMAST\" ";
                //strQry += " WHERE \"DT_CODE\"=\"UNA_DTCCODE\" AND \"DM_SL_NO\"=\"UNA_MISMATCHENTRY_SLNO\" ";
                //strQry += "AND \"DT_OM_SLNO\" LIKE '" + OffCode + "%'";
                //dt = ObjCon.FetchDataTable(strQry);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_unallocate_details");
                cmd.Parameters.AddWithValue("p_offcode", Convert.ToString(OffCode ?? ""));
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
        /// Load UnAllocate DTR Details
        /// </summary>
        /// <param name="OffCode"></param>
        /// <returns></returns>
        public DataTable LoadUnAllocateDTRDetails(string OffCode)
        {
            DataTable dt = new DataTable();
            try
            {
                #region Old inline query
                //string strQry = string.Empty;
                //strQry = "SELECT 0 AS \"DT_CODE\",\"DME_EXISTING_DTR_CODE\",TO_CHAR(\"DME_ENTRY_DATE\",'dd-mon-YYYY')";
                //strQry += "\"DME_ENTRY_DATE\",\"TC_LOCATION_ID\",";
                //strQry += "(SELECT \"OFF_NAME\" AS \"SECTION\" FROM \"VIEW_OFFICES\" WHERE \"OFF_CODE\"=";
                //strQry += "SUBSTR(TC_LOCATION_ID,0,5))\"SECTION\" FROM ";
                //strQry += "\"TBLUNALLOCATEDDTRS\",\"TBLDTCMISMATCHENTRY\", ";
                //strQry += " \"TBLTCMASTER\" WHERE \"DME_SL_NO\"=\"UAD_SL_MISMATCHENTRY_SLNO\" AND ";
                //strQry += "\"DME_EXISTING_DTR_CODE\"=\"TC_CODE\" AND ";
                //strQry += "\"TC_LOCATION_ID\" LIKE '" + OffCode + "%'";
                //dt = ObjCon.FetchDataTable(strQry);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_unallocate_dtr_details");
                cmd.Parameters.AddWithValue("p_offcode", Convert.ToString(OffCode ?? ""));
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
        /// Get Unmap Details
        /// </summary>
        /// <param name="objDtcMissMatch"></param>
        /// <returns></returns>
        public DataTable GetUnmapDetails(clsDtcMissMatchEntry objDtcMissMatch)
        {
            DataTable dt = new DataTable();
            try
            {
                #region Old inline query
                //string strQry = string.Empty;
                //strQry = "SELECT \"DT_CODE\",\"DT_NAME\",\"DT_OM_SLNO\",0 as \"DT_TC_ID\" FROM \"TBLUNALLOCATEDDTCS\",";
                //strQry += "\"TBLDTCMISMATCHENTRY\",";
                //strQry += "\"TBLDTCMAST\" WHERE \"DT_CODE\"=\"UNA_DTCCODE\" AND \"DME_SL_NO\"=\"UNA_MISMATCHENTRY_SLNO\" AND ";
                //strQry += "\"UNA_REALLOCATED_DTR_SLNO\" IS NULL ";
                //strQry += "AND \"DT_CODE\"='" + objDtcMissMatch.sDtcCode + "'";
                //dt = ObjCon.FetchDataTable(strQry);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_unmap_details");
                cmd.Parameters.AddWithValue("p_dtccode", Convert.ToString(objDtcMissMatch.sDtcCode ?? ""));
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
        /// Get Unmap DTR Details
        /// </summary>
        /// <param name="objDtcMissMatch"></param>
        /// <returns></returns>
        public DataTable GetUnmapDTRDetails(clsDtcMissMatchEntry objDtcMissMatch)
        {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();

            try
            {
                #region Old inline query
                //string strQry = string.Empty;
                //strQry = "SELECT * FROM \"TBLDTCMAST\",\"TBLTCMASTER\" WHERE \"TC_CODE\"=\"DT_TC_ID\" AND cast(\"TC_CODE\" as text)=";
                //strQry += "'" + objDtcMissMatch.sDtrCode + "'";
                //dt1 = ObjCon.FetchDataTable(strQry);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_unmap_dtr_overall_details");
                cmd.Parameters.AddWithValue("p_dtrcode", Convert.ToString(objDtcMissMatch.sDtrCode ?? ""));
                dt1 = ObjCon.FetchDataTable(cmd);

                if (dt1.Rows.Count > 0)
                {
                    return dt1;
                }
                else
                {
                    #region Old inline query
                    //strQry = "SELECT 0 AS \"DTCODE\",\"DME_EXISTING_DTR_CODE\",TO_CHAR(\"DME_ENTRY_DATE\",'dd-mon-YYYY')";
                    //strQry += "\"DME_ENTRY_DATE\",\"TC_CAPACITY\",";
                    //strQry += "CASE WHEN ";
                    //strQry += " \"TC_CURRENT_LOCATION\"='1' THEN 'STORE' WHEN \"TC_CURRENT_LOCATION\"='2' THEN 'FIELD' ";
                    //strQry += "END \"TC_CURRENT_LOCATION\" FROM ";
                    //strQry += "\"TBLUNALLOCATEDDTRS\",\"TBLDTCMISMATCHENTRY\",";
                    //strQry += " \"TBLTCMASTER\" WHERE \"DME_SL_NO\"=\"UAD_SL_MISMATCHENTRY_SLNO\" AND ";
                    //strQry += "\"DME_EXISTING_DTR_CODE\"=\"TC_CODE\" AND ";
                    //strQry += "\"DME_EXISTING_DTR_CODE\"='" + objDtcMissMatch.sDtrCode + "'";
                    //dt = ObjCon.FetchDataTable(strQry);

                    //if (dt.Rows.Count == 0)
                    //{                   
                    //    strQry = "SELECT 0 as \"DTCODE\",\"TC_CODE\" AS \"DME_EXISTING_DTR_CODE\",'' as \"DME_ENTRY_DATE\",";
                    //    strQry += "\"TC_CAPACITY\",CASE WHEN  ";
                    //    strQry += "\"TC_CURRENT_LOCATION\"='1' THEN 'STORE' WHEN ";
                    //    strQry += " \"TC_CURRENT_LOCATION\"='2' THEN 'FIELD' END \"TC_CURRENT_LOCATION\" FROM \"TBLTCMASTER\" WHERE ";
                    //    strQry += "\"TC_CURRENT_LOCATION\"='1' ";
                    //    strQry += "AND cast(\"TC_CODE\" as text)='" + objDtcMissMatch.sDtrCode + "'";
                    //    dt = ObjCon.FetchDataTable(strQry);
                    //}
                    #endregion

                    NpgsqlCommand cmd_else = new NpgsqlCommand("proc_get_unmap_dtr_details_on_case");
                    cmd_else.Parameters.AddWithValue("p_dtrcode", Convert.ToString(objDtcMissMatch.sDtrCode ?? ""));
                    dt = ObjCon.FetchDataTable(cmd_else);
                }
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        /// <summary>
        /// Allocate UnMapp DTC
        /// </summary>
        /// <param name="objMissMatch"></param>
        /// <returns></returns>
        public string[] AllocateUnMappDTC(clsDtcMissMatchEntry objMissMatch)
        {
            string[] Arr = new string[2];
            string strQry = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                ObjCon.BeginTransaction();

                #region old inline query
                //strQry = "UPDATE \"TBLUNALLOCATEDDTCS\" SET \"UNA_REALLOCATION_DATE\"=now(),\"UNA_REALLOCATION_BY\"=";
                //strQry += "'" + objMissMatch.sCrBy + "',";
                //strQry += "\"UNA_REALLOCATED_DTR_SLNO\"=";
                //strQry += "'" + objMissMatch.sDtrCode + "',\"UNA_REMARKS\"='" + objMissMatch.sRemarks + "' WHERE ";
                //strQry += "\"UNA_DTCCODE\"='" + objMissMatch.sDtcCode + "'";
                //ObjCon.ExecuteQry(strQry);

                //strQry = "SELECT \"UAD_SL_NO\" FROM \"TBLUNALLOCATEDDTRS\" WHERE \"UAD_DTRCODE\"='" + objMissMatch.sDtrCode + "'";
                //string res = ObjCon.get_value(strQry);
                ////if (res == null || res == "")
                ////{

                ////}
                //if (Convert.ToString(res ?? "").Length > 0)
                //{
                //    strQry = "UPDATE \"TBLUNALLOCATEDDTRS\" SET \"UAD_REALLOCATION_DATE\"=now(),\"UAD_DTCCODE\"=";
                //    strQry += "'" + objMissMatch.sDtcCode + "',";
                //    strQry += "\"UAD_REALLOCATION_BY\"='" + objMissMatch.sCrBy + "',";
                //    strQry += "\"UAD_REALLOCATED_LOC_TYPE\"='2',\"UAD_REALLOCATED_LOC_REFNO\"=";
                //    strQry += "'" + objMissMatch.sOfficeCode + "',\"UAD_REMARKS\"=";
                //    strQry += "'" + objMissMatch.sRemarks + "' WHERE \"UAD_SL_NO\"='" + res + "'";
                //    ObjCon.ExecuteQry(strQry);
                //}

                //strQry = "UPDATE \"TBLDTCMAST\" SET \"DT_TC_ID\"='" + objMissMatch.sDtrCode + "' ";
                //strQry += "WHERE \"DT_CODE\"='" + objMissMatch.sDtcCode + "'";
                //ObjCon.ExecuteQry(strQry);

                //strQry = "UPDATE \"TBLTCMASTER\" SET \"TC_LOCATION_ID\"='" + objMissMatch.sOfficeCode + "',";
                //strQry += "\"TC_CURRENT_LOCATION\"='2' WHERE ";
                //strQry += "\"TC_CODE\"='" + objMissMatch.sDtrCode + "'";
                //ObjCon.ExecuteQry(strQry);

                ////DtmmsWebService.Service1Client objWCF = new DtmmsWebService.Service1Client();
                ////objWCF.SaveTcDetails(strQry);

                //long sMaxmapping = ObjCon.Get_max_no("TM_ID", "TBLTRANSDTCMAPPING");

                //strQry = "INSERT INTO \"TBLTRANSDTCMAPPING\" (\"TM_ID\",\"TM_MAPPING_DATE\",\"TM_TC_ID\",";
                //strQry += "\"TM_DTC_ID\",\"TM_LIVE_FLAG\",";
                //strQry += "\"TM_CRBY\",\"TM_CRON\") ";
                //strQry += "VALUES ('" + sMaxmapping + "',";
                //strQry += "now(),'" + objMissMatch.sDtrCode + "','" + objMissMatch.sDtcCode + "','1',";
                //strQry += "'" + objMissMatch.sCrBy + "',now())";
                //ObjCon.ExecuteQry(strQry);

                //strQry = "INSERT INTO \"TBLDTCTRANSACTION\" (\"DCT_ID\",\"DCT_DTC_CODE\",\"DCT_DTR_CODE\",";
                //strQry += "\"DCT_DTR_STATUS\",\"DCT_TRANS_DATE\",";
                //strQry += "\"DCT_ACT_REFNO\",\"DCT_ACT_REFTYPE\",\"DCT_DESC\",\"DCT_ENTRYDATE\",\"DCT_CANCEL_FLAG\") ";
                //strQry += "VALUES((SELECT COALESCE(max(\"DCT_ID\"),0)+1 FROM \"TBLDTCTRANSACTION\"),";
                //strQry += "'" + objMissMatch.sDtcCode + "',";
                //strQry += "'" + objMissMatch.sDtrCode + "','1',now(),'','1','NEW DTC COMMISSIONED (FROM BACKEND)',now(),'0')";
                //ObjCon.ExecuteQry(strQry);

                //strQry = " INSERT INTO \"TBLDTRTRANSACTION\" (\"DRT_ID\",\"DRT_DTR_CODE\",\"DRT_LOC_ID\",\"DRT_LOC_TYPE\", ";
                //strQry += " \"DRT_TRANS_DATE\", \"DRT_ACT_REFNO\", \"DRT_ACT_REFTYPE\",\"DRT_DESC\",\"DRT_DTR_STATUS\",\"DRT_ENTRYDATE\",\"DRT_CANCEL_FLAG\") ";
                //strQry += " VALUES ((SELECT COALESCE(max(\"DRT_ID\"),0)+1 FROM \"TBLDTRTRANSACTION\"),";
                //strQry += " '" + objMissMatch.sDtrCode + "','" + objMissMatch.sOfficeCode + "','2',now(),'','2',"; 
                //strQry += " 'COMMISSIONED TO DTCCODE :" + objMissMatch.sDtcCode + " (FROM BACKEND)','1',now(),'0')";                
                //ObjCon.ExecuteQry(strQry);
                //Arr[0] = "1";
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_saveupdate_allocate_unmapp_dtc");
                cmd.Parameters.AddWithValue("p_dtccode", Convert.ToString(objMissMatch.sDtcCode));
                cmd.Parameters.AddWithValue("p_dtrcode", Convert.ToString(objMissMatch.sDtrCode ?? ""));
                cmd.Parameters.AddWithValue("p_remarks", Convert.ToString(objMissMatch.sRemarks ?? ""));
                cmd.Parameters.AddWithValue("p_crby", Convert.ToString(objMissMatch.sCrBy ?? ""));
                cmd.Parameters.AddWithValue("p_officecode", Convert.ToString(objMissMatch.sOfficeCode ?? ""));
                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                Arr[0] = "op_id";
                Arr[1] = "msg";
                Arr = ObjCon.Execute(cmd, Arr, 2);

                ObjCon.CommitTransaction();

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                Arr[0] = "2";
                return Arr;
            }
            return Arr;
        }
    }
}
