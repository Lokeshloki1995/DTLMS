using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using IIITS.PGSQL.DAL;
using Npgsql;
using System.Configuration;

namespace IIITS.DTLMS.BL
{
    public class clsQCApproval
    {

        string strFormCode = "clsQCApproval";


        public string sPendingforQC { get; set; }
        public string sLocationType { get; set; }
        public string sOperator { get; set; }
        public string sSupervisor { get; set; }
        public string sDivision { get; set; }
        public string sSubDivision { get; set; }
        public string sOffcode { get; set; }
        public string sFeeder { get; set; }
        public string sStore { get; set; }
        public string sRepairer { get; set; }
        public string sMakeExist { get; set; }
        public string sRework { get; set; }
        public string sOperator1 { get; set; }
        public string sOperator2 { get; set; }
        public string sSerialNo { get; set; }

        public string sEnumDetailsId { get; set; }
        public string sStatusFlag { get; set; }
        public string sComments { get; set; }
        public string sRoleType { get; set; }
        public string sRoleId { get; set; }
        public bool sMobileEntry { get; set; }
        public string sUserType { get; set; }
        public string sDtrCode { get; set; }
        public string sDtcCode { get; set; }
        public string sQcDoneBy { get; set; }
        public string sDeEnteredBy { get; set; }
        public string sDtcName { get; set; }
        public bool sViewAll { get; set; }

        public DataTable LoadEnumearionDetails(clsQCApproval objQC)
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            DataTable dt = new DataTable();
            try
            {


                string strQry = string.Empty;

                //strQry = " SELECT * FROM (SELECT \"ED_ID\", \"ED_LOCTYPE\", (CASE  WHEN  \"ED_IS_FEEDER_BIFURCATION\"=1 and  \"ED_LOCTYPE\"=2 THEN 'FIELD ~ FEEDER BIFURCATION'  WHEN \"ED_LOCTYPE\"=1 THEN 'STORE' WHEN \"ED_LOCTYPE\"=2 THEN 'FIELD' WHEN \"ED_LOCTYPE\"=3 THEN 'REPAIRER' WHEN \"ED_LOCTYPE\"=5 THEN 'TRANSFORMER BANK' end ) AS \"TYPE\",";
                //strQry += " \"DTE_DTCCODE\",UPPER(\"DTE_NAME\") \"DTE_NAME\",\"DTE_TC_CODE\" \"DTE_TC_CODE\", ";
                //strQry += " (SELECT UPPER(\"TM_NAME\")  FROM \"TBLTRANSMAKES\" WHERE CAST(\"TM_ID\" AS TEXT)=CAST(\"DTE_MAKE\" AS TEXT)) AS \"MAKE\", ";
                //strQry += " (SELECT UPPER(\"IU_FULLNAME\")  FROM \"TBLINTERNALUSERS\" WHERE \"IU_ID\" = (SELECT \"IU_SUPERVISORID\" FROM \"TBLINTERNALUSERS\" WHERE  CAST(\"IU_ID\" AS TEXT) = CAST(\"ED_OPERATOR1\" AS TEXT))) AS \"SUPERVISOR\",\"ED_LOCTYPE\",\"ED_ENUM_TYPE\",\"ED_STATUS_FLAG\",\"DTE_TC_SLNO\",\"ED_IS_FEEDER_BIFURCATION\" ";
                //strQry += " FROM \"TBLENUMERATIONDETAILS\", \"TBLDTCENUMERATION\" WHERE CAST(\"ED_ID\" AS TEXT) = CAST(\"DTE_ED_ID\" AS TEXT) AND \"ED_STATUS_FLAG\" = '" + objQC.sPendingforQC + "' ";

                //if (objQC.sFeeder != null)
                //{
                //    strQry += " AND CAST(\"ED_FEEDERCODE\" AS TEXT) LIKE '" + objQC.sFeeder + "'";
                //}

                //if (objQC.sOperator != null)
                //{
                //    strQry += " AND (\"ED_OPERATOR1\" = '" + objQC.sOperator + "' OR \"ED_OPERATOR2\" = '" + objQC.sOperator + "')";
                //}

                //if (objQC.sLocationType != null && objQC.sLocationType != "")
                //{
                //    strQry += " AND \"ED_LOCTYPE\" IN (" + objQC.sLocationType + ")";
                //}

                //if (objQC.sRoleType == "2")
                //{
                //    // string sOffCode = clsStoreOffice.GetOfficeCode(objQC.sOffcode, "ED_OFFICECODE");
                //    // strQry += "AND (" + sOffCode;
                //    //strQry += ")";
                //    strQry += " AND CAST(\"ED_OFFICECODE\"  AS TEXT)LIKE '" + objQC.sOffcode + "%'";
                //    strQry += " AND \"ED_LOCTYPE\" IN (1,3,5)";
                //}
                //else
                //{


                //    if (objQC.sRoleId == "8")
                //    {
                //        if (objQC.sOffcode != null)
                //        {
                //            strQry += " AND CAST(\"ED_OFFICECODE\"  AS TEXT)LIKE '" + objQC.sOffcode + "%'";
                //        }
                //        else
                //        {
                //            strQry += " AND CAST(\"ED_OFFICECODE\"  AS TEXT)LIKE '%'";
                //        }
                //    }
                //    else
                //    {
                //        strQry += " AND CAST(\"ED_OFFICECODE\"  AS TEXT)LIKE '" + objQC.sOffcode + "%'";
                //        strQry += " AND \"ED_LOCTYPE\" IN (2)";
                //    }
                //}

                //if (objQC.sRepairer != null)
                //{
                //    strQry += " AND CAST(\"ED_LOCNAME\" AS TEXT) LIKE '" + objQC.sRepairer + "'";
                //}

                //if (objQC.sStore != null)
                //{
                //    strQry += " AND CAST(\"ED_LOCNAME\" AS TEXT) LIKE '" + objQC.sStore + "'";
                //}
                //if (objQC.sSupervisor != null)
                //{
                //    strQry += " AND (\"ED_OPERATOR2\" IN (SELECT \"IU_ID\" FROM \"TBLINTERNALUSERS\" WHERE \"IU_SUPERVISORID\"='" + objQC.sSupervisor + "'))";
                //    strQry += " OR (\"ED_OPERATOR1\" IN (SELECT \"IU_ID\" FROM \"TBLINTERNALUSERS\" WHERE \"IU_SUPERVISORID\"='" + objQC.sSupervisor + "'))";
                //}

                //if (objQC.sMobileEntry == true)
                //{
                //    strQry += " AND CAST(\"ED_RECORD_BY\"  AS TEXT)LIKE 'MOBILE%'";
                //}

                //strQry += " ORDER BY \"DTE_DTCCODE\")A ";
                //strQry += " WHERE  ";/* ROWNUM < 20 AND*/
                //strQry += "  CAST(\"DTE_TC_CODE\" AS TEXT) LIKE '" + objQC.sDtrCode + "%' ";
                //if (objQC.sDtcCode != "")
                //{
                //    strQry += " AND CAST(\"DTE_DTCCODE\" AS TEXT) LIKE '" + objQC.sDtcCode + "%'";
                //}
                //if (objQC.sDtcName != "")
                //{
                //    strQry += " AND CAST(\"DTE_NAME\" AS TEXT) LIKE '" + objQC.sDtcName + "%'";
                //}

                //if (objQC.sSerialNo != "")
                //{
                //    strQry += " AND UPPER(CAST(\"DTE_TC_SLNO\" AS TEXT))  LIKE '" + objQC.sSerialNo + "%'";
                //}

                //dt = ObjCon.FetchDataTable(strQry);
                //return dt;


                //sp
                string boolval = Convert.ToString(objQC.sMobileEntry);
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_enumeration_details");
                cmd.Parameters.AddWithValue("pendforqc", Convert.ToString(objQC.sPendingforQC ?? ""));
                cmd.Parameters.AddWithValue("feeder", Convert.ToString(objQC.sFeeder ?? ""));
                cmd.Parameters.AddWithValue("operator", Convert.ToString(objQC.sOperator ?? ""));
                cmd.Parameters.AddWithValue("locationtype", Convert.ToString(objQC.sLocationType ?? ""));
                cmd.Parameters.AddWithValue("roletype", Convert.ToString(objQC.sRoleType ?? ""));
                cmd.Parameters.AddWithValue("roleid", Convert.ToString(objQC.sRoleId ?? ""));
                cmd.Parameters.AddWithValue("ofccode", Convert.ToString(objQC.sOffcode ?? ""));
                cmd.Parameters.AddWithValue("repairer", Convert.ToString(objQC.sRepairer ?? ""));
                cmd.Parameters.AddWithValue("store", Convert.ToString(objQC.sStore ?? ""));
                cmd.Parameters.AddWithValue("supervisor", Convert.ToString(objQC.sSupervisor ?? ""));
                cmd.Parameters.AddWithValue("mobentry", Convert.ToString(boolval));
                cmd.Parameters.AddWithValue("dtrcode", Convert.ToString(objQC.sDtrCode ?? ""));
                cmd.Parameters.AddWithValue("dtccode", Convert.ToString(objQC.sDtcCode ?? ""));
                cmd.Parameters.AddWithValue("dtcname", Convert.ToString(objQC.sDtcName ?? ""));
                cmd.Parameters.AddWithValue("serialno", Convert.ToString(objQC.sSerialNo ?? ""));
                dt = ObjCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }

        }


        //Will Be Called at the time of FiledEnumeraton Data Update to check the Access.
        public bool CheckEnumerationUpdateAuthority(string sUserId)
        {
            DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            try
            {
                string strQry = string.Empty;
                if (sUserId != "")
                {

                    //strQry = "SELECT \"ED_ID\" FROM \"TBLENUMERATIONDETAILS\" WHERE (\"ED_OPERATOR1\"='" + sUserId + "' OR \"ED_OPERATOR2\"='" + sUserId + "' OR \"ED_CRBY\"='" + sUserId + "') OR ";
                    //strQry += " (CAST (\"ED_CRBY\" as int) IN (SELECT \"IU_ID\" FROM \"TBLINTERNALUSERS\" WHERE \"IU_SUPERVISORID\"='" + sUserId + "')) ";
                    //strQry += "  OR   ( CAST (\"ED_OPERATOR2\" as text) IN (SELECT cast(\"IU_ID\" as text) FROM \"TBLINTERNALUSERS\" WHERE \"IU_SUPERVISORID\"='" + sUserId + "'))";
                    //strQry += " OR (CAST (\"ED_OPERATOR1\" as int) IN (SELECT \"IU_ID\" FROM \"TBLINTERNALUSERS\" WHERE \"IU_SUPERVISORID\"='" + sUserId + "'))";
                    //strQry += "  UNION ALL ";
                    //strQry += " SELECT \"IU_ID\" FROM \"TBLINTERNALUSERS\" WHERE (\"IU_ID\"='" + sUserId + "' OR \"IU_SUPERVISORID\"='" + sUserId + "') AND \"IU_USERTYPE\"='5'";


                    //string sResult = ObjCon.get_value(strQry);


                    NpgsqlCommand cmd = new NpgsqlCommand("sp_get_enumeration_update_authority");
                    cmd.Parameters.AddWithValue("user_id", (sUserId ?? ""));
                    string sResult = objDatabse.StringGetValue(cmd);

                    if (sResult == "")
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }

        public string GetPendingRejectRemarks(clsQCApproval objQC)
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);

            try
            {
                //string strQry = string.Empty;
                //if (objQC.sStatusFlag == "3")
                //{
                //    strQry = "SELECT \"QR_REMARKS\" FROM \"TBLQCREJECT\" WHERE \"QR_ED_ID\"='" + objQC.sEnumDetailsId + "'";

                //}
                //else if (objQC.sStatusFlag == "2")
                //{
                //    strQry = "SELECT \"QP_REMARKS\" FROM \"TBLQCPENDING\" WHERE \"QP_ED_ID\"='" + objQC.sEnumDetailsId + "'";
                //}
                //return ObjCon.get_value(strQry);

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_pend_reject_remarks");

                cmd.Parameters.AddWithValue("status_flag", (objQC.sStatusFlag ?? ""));
                cmd.Parameters.AddWithValue("enum_details_id", (objQC.sEnumDetailsId ?? ""));
                return objDatabse.StringGetValue(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        public DataTable LoadQcDoneBy(clsQCApproval objQC)
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationManager.AppSettings["pgSQLPassword"]));
            DataTable dt = new DataTable();

            try
            {
                string strQry = string.Empty;
                string sOffCode = string.Empty;

                if (objQC.sRoleType == "2")
                {
                    sOffCode = clsStoreOffice.GetOfficeCode(objQC.sOffcode, "ED_OFFICECODE");
                }
                #region old inline query
                //strQry = " SELECT \"ED_ID\", CASE WHEN \"ED_LOCTYPE\"=1 THEN 'STORE' WHEN \"ED_LOCTYPE\"=2 THEN 'FIELD' WHEN \"ED_LOCTYPE\"=3 THEN 'REPAIRER' WHEN \"ED_LOCTYPE\"=5 THEN 'TRANSFORMER BANK' END AS \"TYPE\",\"DTE_DTCCODE\", CAST(\"DTE_NAME\" AS TEXT), CAST(\"DTE_TC_CODE\" AS TEXT) \"DTE_TC_CODE\", ";
                //strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"ED_OPERATOR2\" AS TEXT) = CAST(\"US_ID\" AS TEXT)) AS \"OPERATOR2\", ";
                //strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_ID\" AS TEXT) =CAST(\"ED_CRBY\" AS TEXT)) AS \"ED_CRBY\",(SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_ID\" AS TEXT) =CAST(\"ED_APPROVED_BY\" AS TEXT)) AS \"ED_APPROVED_BY\"";
                //strQry += " FROM \"TBLENUMERATIONDETAILS\", \"TBLDTCENUMERATION\" WHERE CAST(\"ED_ID\" AS TEXT) = CAST(\"DTE_ED_ID\" AS TEXT) and \"ED_STATUS_FLAG\"=1";

                //if (objQC.sDtcCode != null && objQC.sDtcCode != "")
                //{
                //    strQry += " AND CAST(\"DTE_DTCCODE\" AS TEXT) LIKE '" + objQC.sDtcCode + "%'";
                //}

                //if (objQC.sDtrCode != "" && objQC.sDtrCode != null)
                //{
                //    strQry += " AND CAST(\"DTE_TC_CODE\" AS TEXT) LIKE '" + objQC.sDtrCode + "%'";
                //}
                //if (objQC.sRoleType == "2")
                //{
                //     sOffCode = clsStoreOffice.GetOfficeCode(objQC.sOffcode, "ED_OFFICECODE");
                //    strQry += "AND (" + sOffCode;
                //    strQry += ")";
                //    strQry += " AND \"ED_LOCTYPE\" IN (1,3,5)";
                //}
                //else
                //{
                //    strQry += " AND CAST(\"ED_OFFICECODE\"  AS TEXT)LIKE '" + objQC.sOffcode + "%'";
                //    strQry += " AND \"ED_LOCTYPE\" IN (2)";
                //}
                //if (objQC.sDeEnteredBy != null && objQC.sDeEnteredBy != "")
                //{
                //    strQry += " AND \"ED_CRBY\"='" + objQC.sDeEnteredBy + "'";
                //}

                //if (objQC.sQcDoneBy != null && objQC.sQcDoneBy != "")
                //{
                //    strQry += " AND \"ED_APPROVED_BY\" ='" + objQC.sQcDoneBy + "'";
                //}

                //dt = ObjCon.FetchDataTable(strQry);
                #endregion
                //sp
                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_qc_done_by");
                cmd.Parameters.AddWithValue("dtc_code", Convert.ToString(objQC.sDtcCode ?? ""));
                cmd.Parameters.AddWithValue("dtr_code", Convert.ToString(objQC.sDtrCode ?? ""));
                cmd.Parameters.AddWithValue("role_type", Convert.ToString(objQC.sRoleType ?? ""));
                cmd.Parameters.AddWithValue("off_code", Convert.ToString(sOffCode ?? ""));
                cmd.Parameters.AddWithValue("off_code1", Convert.ToString(objQC.sOffcode ?? ""));
                cmd.Parameters.AddWithValue("de_entered_by", Convert.ToString(objQC.sDeEnteredBy ?? ""));
                cmd.Parameters.AddWithValue("qc_done_by", Convert.ToString(objQC.sQcDoneBy ?? ""));

                dt = ObjCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
    }
}
