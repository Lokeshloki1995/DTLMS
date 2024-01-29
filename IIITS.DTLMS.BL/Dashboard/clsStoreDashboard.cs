using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;
using System.Data;
using System.Data.OleDb;

namespace IIITS.DTLMS.BL.Dashboard
{
    public class clsStoreDashboard
    {
        String strFormCode = "clsStoreDashboard";
        PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
        
        public string sOfficeCode { get; set; }
        public string sroletype { get; set; }

        DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);

        public string GetNewTCCount(clsStoreDashboard objSDashboard)
        {
            string count = string.Empty;
            try
            {
                string strQry = string.Empty;
                if (objSDashboard.sOfficeCode != null || objSDashboard.sOfficeCode != string.Empty)
                {
                    //strQry = "SELECT COUNT(*) FROM \"TBLTCMASTER\" WHERE \"TC_STATUS\"='1' AND \"TC_CURRENT_LOCATION\" = 1  AND cast(\"TC_LOCATION_ID\" as text) = '" + objSDashboard.sOfficeCode + "'";
                    //return ObjCon.get_value(strQry);

                   // DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsstoredashboard");
                    cmd.Parameters.AddWithValue("p_key", "GETNEWTCCOUNT");
                    cmd.Parameters.AddWithValue("p_value", objSDashboard.sOfficeCode);
                    cmd.Parameters.AddWithValue("p_offcode", "");
                    count = objDatabse.StringGetValue(cmd);
                    return count;
                }
                else
                {
                    return "0";
                }
                
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }
        public string GetRepaireGoodCount(clsStoreDashboard objSDashboard)
        {
            string count = string.Empty;
            try
            {
                string strQry = string.Empty;
                if (objSDashboard.sOfficeCode != null || objSDashboard.sOfficeCode != string.Empty)
                {
                    //strQry = "SELECT COUNT(*) FROM \"TBLTCMASTER\" WHERE \"TC_STATUS\"='2'AND \"TC_CURRENT_LOCATION\" =1 AND cast(\"TC_LOCATION_ID\" as text) = '" + objSDashboard.sOfficeCode + "'";
                    //return ObjCon.get_value(strQry);

                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsstoredashboard");
                    cmd.Parameters.AddWithValue("p_key", "GETREPAIREGOODCOUNT");
                    cmd.Parameters.AddWithValue("p_value", objSDashboard.sOfficeCode);
                    cmd.Parameters.AddWithValue("p_offcode", "");
                    count = objDatabse.StringGetValue(cmd);
                    return count;
                }
                else
                {
                    return "0";
                }
                
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }
        public string GetReleaseGoodCount(clsStoreDashboard objSDashboard)
        {
            string count = string.Empty;
            try
            {
                string strQry = string.Empty;
                if (objSDashboard.sOfficeCode != null || objSDashboard.sOfficeCode != string.Empty)
                {

                    //strQry = "SELECT COUNT(*) FROM \"TBLTCMASTER\" WHERE \"TC_STATUS\"='11'AND \"TC_CURRENT_LOCATION\" =1 AND cast(\"TC_LOCATION_ID\" as text) = '" + objSDashboard.sOfficeCode + "' ";
                    //return ObjCon.get_value(strQry);

                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsstoredashboard");
                    cmd.Parameters.AddWithValue("p_key", "GETREALEASEGOODCOUNT");
                    cmd.Parameters.AddWithValue("p_value", objSDashboard.sOfficeCode);
                    cmd.Parameters.AddWithValue("p_offcode", "");
                    count = objDatabse.StringGetValue(cmd);
                    return count;
                }

                else
                {
                    return "0";
                }

                

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }
        public string GetFaultyCount(clsStoreDashboard objSDashboard)
        {
            string count = string.Empty;
            try
            {
                string strQry = string.Empty;
                if (objSDashboard.sOfficeCode != null || objSDashboard.sOfficeCode != string.Empty)
                {
                    //strQry = "SELECT COUNT(*) FROM \"TBLTCMASTER\" WHERE \"TC_STATUS\"='3' AND \"TC_CURRENT_LOCATION\" =1 AND cast(\"TC_LOCATION_ID\" as text) = '" + objSDashboard.sOfficeCode + "'";
                    //return ObjCon.get_value(strQry);

                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsstoredashboard");
                    cmd.Parameters.AddWithValue("p_key", "GETFAULTYCOUNT");
                    cmd.Parameters.AddWithValue("p_value", objSDashboard.sOfficeCode);
                    cmd.Parameters.AddWithValue("p_offcode", "");
                    count = objDatabse.StringGetValue(cmd);
                    return count;
                }
                else
                {
                    return "0";
                }
                
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }
        public string GetMobileTCCount(clsStoreDashboard objSDashboard)
        {
            string count = string.Empty;

            try
            {
                string strQry = string.Empty;
                if (objSDashboard.sOfficeCode != null || objSDashboard.sOfficeCode != string.Empty)
                {
                    //strQry = "SELECT COUNT(*) FROM \"TBLTCMASTER\" WHERE \"TC_STATUS\"='4' AND \"TC_CURRENT_LOCATION\" =1 AND cast(\"TC_LOCATION_ID\" as text) = '" + objSDashboard.sOfficeCode + "'";
                    //return ObjCon.get_value(strQry);

                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsstoredashboard");
                    cmd.Parameters.AddWithValue("p_key", "GETMOBILETCCOUNT");
                    cmd.Parameters.AddWithValue("p_value", objSDashboard.sOfficeCode);
                    cmd.Parameters.AddWithValue("p_offcode", "");
                    count = objDatabse.StringGetValue(cmd);
                    return count;
                }
                else
                {
                    return "0";
                }
                
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }
        public string GetCapacityless25(clsStoreDashboard objSDashboard)
        {
            string count = string.Empty;
            try
            {
                string strQry = string.Empty;
                if (objSDashboard.sOfficeCode != null || objSDashboard.sOfficeCode != string.Empty)
                {
                    //strQry = "SELECT COUNT(*) FROM \"TBLTCMASTER\" WHERE \"TC_CURRENT_LOCATION\" =1 AND \"TC_CAPACITY\" < 25 AND cast(\"TC_LOCATION_ID\" as text) = '" + objSDashboard.sOfficeCode + "'";
                    //return ObjCon.get_value(strQry);

                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsstoredashboard");
                    cmd.Parameters.AddWithValue("p_key", "GETCAPACITYLESS25");
                    cmd.Parameters.AddWithValue("p_value", objSDashboard.sOfficeCode);
                    cmd.Parameters.AddWithValue("p_offcode", "");
                    count = objDatabse.StringGetValue(cmd);
                    return count;
                }

                else
                {
                    return "0";
                }
                
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }
        public string GetCapacity25_100(clsStoreDashboard objSDashboard)
        {
            string count = string.Empty;
            try
            {  
                string strQry = string.Empty;
                if (objSDashboard.sOfficeCode != null || objSDashboard.sOfficeCode != string.Empty)
                {
                    //strQry = "SELECT COUNT(*) FROM \"TBLTCMASTER\" WHERE \"TC_CURRENT_LOCATION\" =1 AND \"TC_CAPACITY\" BETWEEN 25 AND 100 AND cast(\"TC_LOCATION_ID\" as text) = '" + objSDashboard.sOfficeCode + "' ";
                    //return ObjCon.get_value(strQry);

                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsstoredashboard");
                    cmd.Parameters.AddWithValue("p_key", "GETCAPACITY25_100");
                    cmd.Parameters.AddWithValue("p_value", objSDashboard.sOfficeCode);
                    cmd.Parameters.AddWithValue("p_offcode", "");
                    count = objDatabse.StringGetValue(cmd);
                    return count;
                }
                else
                {
                    return "0";
                }

               
            }
            catch (Exception ex)      
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }
        public string GetCapacity125_250(clsStoreDashboard objSDashboard)
        {
            string count = string.Empty;
            try
            {
                string strQry = string.Empty;
                if (objSDashboard.sOfficeCode != null || objSDashboard.sOfficeCode != string.Empty)
                {
                    //strQry = "SELECT COUNT(*) FROM \"TBLTCMASTER\" WHERE \"TC_CURRENT_LOCATION\" =1 AND \"TC_CAPACITY\" BETWEEN 125 AND 250 AND cast(\"TC_LOCATION_ID\" as text)= '" + objSDashboard.sOfficeCode + "' ";
                    //return ObjCon.get_value(strQry);

                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsstoredashboard");
                    cmd.Parameters.AddWithValue("p_key", "GETCAPACITY125_250");
                    cmd.Parameters.AddWithValue("p_value", objSDashboard.sOfficeCode);
                    cmd.Parameters.AddWithValue("p_offcode", "");
                    count = objDatabse.StringGetValue(cmd);
                    return count;
                }
                else
                {
                    return "0";
                }

               
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }
        public string GetCapacitygreater250(clsStoreDashboard objSDashboard)
        {
            string count = string.Empty;
            try
            {
                string strQry = string.Empty;
                if (objSDashboard.sOfficeCode != null || objSDashboard.sOfficeCode != string.Empty)
                {
                    //strQry = "SELECT COUNT(*) FROM \"TBLTCMASTER\" WHERE \"TC_CURRENT_LOCATION\" =1 AND \"TC_CAPACITY\" > 250 AND cast(\"TC_LOCATION_ID\" as text) = '" + objSDashboard.sOfficeCode + "'";
                    //return ObjCon.get_value(strQry);

                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsstoredashboard");
                    cmd.Parameters.AddWithValue("p_key", "GETCAPACITYGREATER250");
                    cmd.Parameters.AddWithValue("p_value", objSDashboard.sOfficeCode);
                    cmd.Parameters.AddWithValue("p_offcode", "");
                    count = objDatabse.StringGetValue(cmd);
                    return count;
                }
                else
                {
                    return "0";
                }
                
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        public string GetIssuePendingCount(clsStoreDashboard objSDashboard)
        {
            try
            {
                // invoice pending
                string strQry = string.Empty;
                strQry = "SELECT COUNT(*) FROM \"TBLPENDINGTRANSACTION\" WHERE  \"TRANS_BO_ID\" in(29,13) and  \"TRANS_BO_ID\"<>10 and \"TRANS_NEXT_ROLE_ID\"<>0 AND ";
                if (objSDashboard.sroletype == "2")
                {
                    string sOffCode = clsStoreOffice.GetOfficeCode(objSDashboard.sOfficeCode, "TRANS_REF_OFF_CODE");
                    strQry += sOffCode;
                }
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        public string GetRepairPendingCount(clsStoreDashboard objSDashboard)
        {
            string count = string.Empty;
            try
            {
                //string strQry = string.Empty;
                //strQry = " SELECT count(\"RSD_TC_CODE\") ";
                //strQry += " FROM \"TBLREPAIRSENTMASTER\",\"TBLREPAIRSENTDETAILS\" WHERE \"RSM_ID\"=\"RSD_RSM_ID\" and \"RSD_RV_NO\" is null and ";
                //strQry += " \"RSM_DIV_CODE\" = '" + objSDashboard.sOfficeCode + "'";
                //return ObjCon.get_value(strQry);

                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsstoredashboard");
                cmd.Parameters.AddWithValue("p_key", "GETREPAIRPENDINGCOUNT");
                cmd.Parameters.AddWithValue("p_value", objSDashboard.sOfficeCode);
                cmd.Parameters.AddWithValue("p_offcode", "");
                count = objDatabse.StringGetValue(cmd);
                return count;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }
        public string GetRecivePendingCount(clsStoreDashboard objSDashboard)
        {
            string count = string.Empty;
            try
            {
                //string strQry = string.Empty;
                //strQry = "SELECT count(\"TR_ID\") FROM \"TBLDTCFAILURE\" LEFT JOIN \"TBLWORKORDER\" on \"DF_ID\"=\"WO_DF_ID\" LEFT JOIN \"TBLINDENT\" ";
                //strQry += " on \"WO_SLNO\"=\"TI_WO_SLNO\" LEFT JOIN \"TBLDTCINVOICE\" on \"TI_ID\"=\"IN_TI_NO\" LEFT JOIN \"TBLTCREPLACE\" on ";
                //strQry += " \"WO_SLNO\"=\"TR_WO_SLNO\" and \"TR_RV_NO\" is null and \"TR_RI_NO\" is not null and \"WO_SLNO\" is not null and \"TR_STORE_SLNO\"='" + objSDashboard.sOfficeCode + "'";
                //return ObjCon.get_value(strQry);

                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsstoredashboard");
                cmd.Parameters.AddWithValue("p_key", "GETRECIEVEPENDINGCOUNT");
                cmd.Parameters.AddWithValue("p_value", objSDashboard.sOfficeCode);
                cmd.Parameters.AddWithValue("p_offcode", "");
                count = objDatabse.StringGetValue(cmd);
                return count;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        public string GetbankGoodCount(clsStoreDashboard objSDashboard)
        {
            string count = string.Empty;
            try
            {
                //string strQry = string.Empty;
                //strQry =  "select count(*) from \"TBLTCMASTER\" where \"TC_STORE_ID\" = '"+ objSDashboard.sOfficeCode + "' and \"TC_STATUS\" = '1' and \"TC_CODE\" <> '0'  and \"TC_CURRENT_LOCATION\" = '5'";
                //return ObjCon.get_value(strQry);

                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsstoredashboard");
                cmd.Parameters.AddWithValue("p_key", "GETBANKGOODCOUNT");
                cmd.Parameters.AddWithValue("p_value", objSDashboard.sOfficeCode);
                cmd.Parameters.AddWithValue("p_offcode", "");
                count = objDatabse.StringGetValue(cmd);
                return count;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        public string GetReleasegoodCount(clsStoreDashboard objSDashboard)
        {
            string count = string.Empty;
            try
            {
                //string strQry = string.Empty;
                //strQry = "select count(*) from \"TBLTCMASTER\" where \"TC_STORE_ID\" = '" + objSDashboard.sOfficeCode + "' and \"TC_STATUS\" = '11' and \"TC_CODE\" <> '0'  and \"TC_CURRENT_LOCATION\" = '5'";
                //return ObjCon.get_value(strQry);

                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsstoredashboard");
                cmd.Parameters.AddWithValue("p_key", "GETBANKREALEASEGOODCOUNT");
                cmd.Parameters.AddWithValue("p_value", objSDashboard.sOfficeCode);
                cmd.Parameters.AddWithValue("p_offcode", "");
                count = objDatabse.StringGetValue(cmd);
                return count;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        public string GetRepairGoodCount(clsStoreDashboard objSDashboard)
        {
            string count = string.Empty;
            try
            {
                //string strQry = string.Empty;
                //strQry = "select count(*) from \"TBLTCMASTER\" where \"TC_STORE_ID\" = '" + objSDashboard.sOfficeCode + "' and \"TC_STATUS\" = '2' and \"TC_CODE\" <> '0'  and \"TC_CURRENT_LOCATION\" = '5'";
                //return ObjCon.get_value(strQry);

                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsstoredashboard");
                cmd.Parameters.AddWithValue("p_key", "GETREPAIRGOODCOUNT");
                cmd.Parameters.AddWithValue("p_value", objSDashboard.sOfficeCode);
                cmd.Parameters.AddWithValue("p_offcode", "");
                count = objDatabse.StringGetValue(cmd);
                return count;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }
    }
    }

