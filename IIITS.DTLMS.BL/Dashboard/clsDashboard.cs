using System;
using IIITS.PGSQL.DAL;
using System.Data;
using System.Configuration;
using Npgsql;

namespace IIITS.DTLMS.BL
{
    public class clsDashboard
    {
        string strFormCode = "clsDashboard";
        PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
        public string sOfficeCode { get; set; }
        public string sBOId { get; set; }
        public string sRoleId { get; set; }
        public string RoleType { get; set; }
        public string sCapacity { get; set; }
        public string sUserId { get; set; }
        public string sBoType { get; set; }
        public string Manual_Type { get; set; }
        public string File_Name { get; set; }

        //#region "DTC Failure Pending"
        public string GetEstimationPendingCount(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;
                if (objDashoard.sOfficeCode != "" && objDashoard.sOfficeCode != null)
                {
                    strQry = "SELECT COUNT(*) FROM \"WORKFLOWSTATUSDUMMY\" WHERE \"ESTIMATION\" IS NULL AND LENGTH(\"FAILURE\")>= 1 ";
                    strQry += " AND \"WO_BO_ID\" = '9' AND  CAST(\"OFFCODE\" AS TEXT) LIKE '" + objDashoard.sOfficeCode + "%'";
                }
                else
                {
                    strQry = "SELECT COUNT(*) FROM \"WORKFLOWSTATUSDUMMY\" WHERE \"ESTIMATION\" IS NULL AND ";
                    strQry += " LENGTH(\"FAILURE\")>= 1 AND \"WO_BO_ID\" = '9'";
                }
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name,
                    ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }
        public string[] SaveUploadedDocinUserManual(clsDashboard objDashoard)
        {
            string[] Arr = new string[3];
            try
            {
                string strQry = string.Empty;
                strQry = "UPDATE \"TBLUSERMANUALDOC\" SET \"UM_STATUS\" = '0' WHERE \"UM_UMT_ID\" = ";
                strQry += " '" + objDashoard.Manual_Type + "'";
                ObjCon.ExecuteQry(strQry);

                strQry = "INSERT INTO \"TBLUSERMANUALDOC\" (\"UM_ID\",\"UM_UMT_ID\",\"UM_FILENAME\",\"UM_STATUS\") VALUES ";
                strQry += " ((SELECT COALESCE(MAX(CAST(\"UM_ID\" AS INT8)),0)+1  FROM \"TBLUSERMANUALDOC\") ";
                strQry += " ,'" + objDashoard.Manual_Type + "','" + objDashoard.File_Name + "','1')";
                ObjCon.ExecuteQry(strQry);
                Arr[0] = "Uploaded Succesfully...";
                Arr[1] = "0";
                return Arr;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name,
                    ex.Message, ex.StackTrace);
                return Arr;
            }
        }

        ///// <summary>
        ///// Get count of pending Work Order, Indent,Invoice(it includes Failure Entry,Enhancement, New DTC counts)
        ///// </summary>
        ///// <param name="objDashoard"></param>
        ///// <returns></returns>
        public string GetFailurePendingCount(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT WFOTABLE+AUTOTABLE AS FAILURE_PENDING FROM ";
                strQry += " (SELECT COUNT(*) \"WFOTABLE\"  FROM \"TBLWORKFLOWOBJECTS\" WHERE ";
                strQry += " \"WO_REF_OFFCODE\" LIKE '" + objDashoard.sOfficeCode + "%' ";
                strQry += " AND \"WO_BO_ID\" IN (" + objDashoard.sBOId + ") AND \"WO_APPROVE_STATUS\" ='0') A,";
                strQry += " (SELECT COUNT(*) \"AUTOTABLE\" FROM \"TBLWO_OBJECT_AUTO\",\"TBLBO_FLOW_MASTER\" WHERE ";
                strQry += " \"WOA_REF_OFFCODE\" LIKE '" + objDashoard.sOfficeCode + "%' AND ";
                strQry += " \"WOA_INITIAL_ACTION_ID\" IS NULL AND \"BFM_ID\"=\"WOA_BFM_ID\" AND \"BFM_NEXT_BO_ID\" ";
                strQry += " IN (" + objDashoard.sBOId + ")) B ";
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name,
                    ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        ///// <summary>
        ///// Get count of pending Work Order
        ///// </summary>
        ///// <param name="objDashoard"></param>
        ///// <returns></returns>
        public string GetWOPendingCount(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT COUNT(*) FROM \"WORKFLOWSTATUSDUMMY\",\"TBLDTCFAILURE\",\"TBLESTIMATIONDETAILS\" ";
                strQry += " WHERE \"DF_ID\"=\"EST_FAILUREID\"  AND \"WO_DATA_ID\"=\"DF_DTC_CODE\" AND  ";
                strQry += "\"EST_FAIL_TYPE\"='2' AND \"DF_REPLACE_FLAG\"='0' AND \"WORKORDER\"  IS NULL AND LENGTH(\"ESTIMATION\")>= 0 ";
                strQry += " AND \"WO_BO_ID\"<>10 AND  CAST(\"OFFCODE\" AS TEXT) LIKE '" + objDashoard.sOfficeCode + "%'";
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        public string GetSingleWOPendingCount(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT COUNT(*) FROM \"WORKFLOWSTATUSDUMMY\",\"TBLDTCFAILURE\",\"TBLESTIMATIONDETAILS\"  ";
                strQry += " WHERE \"DF_ID\"=\"EST_FAILUREID\" AND \"WO_DATA_ID\"=\"DF_DTC_CODE\" AND \"EST_FAIL_TYPE\"='1'";
                strQry += " AND \"DF_REPLACE_FLAG\"='0' AND \"WORKORDER\"  IS NULL AND LENGTH(\"ESTIMATION\")>= 0  ";
                strQry += " AND \"WO_BO_ID\"<>10 AND  CAST(\"OFFCODE\" AS TEXT) LIKE '" + objDashoard.sOfficeCode + "%'";
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name,
                    ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }
        public string GetReceiveTCPendingCount(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT COUNT(*) FROM \"TBLDTCFAILURE\" A,\"TBLESTIMATIONDETAILS\",\"TBLWORKORDER\", ";
                strQry += " \"VIEW_MINORFAILURE_PENDING\" B WHERE A.\"DF_ID\"=\"EST_FAILUREID\" and A.\"DF_ID\"= ";
                strQry += " \"WO_DF_ID\" AND \"DT_CODE\"=\"DF_DTC_CODE\" AND \"EST_FAIL_TYPE\"='1' ";
                strQry += " AND \"RD_ID\" IS NULL AND \"DF_REPLACE_FLAG\"='0' AND LENGTH(CAST(B.\"WO_NO\" AS TEXT)) ";
                strQry += " >=1 AND CAST(\"OM_CODE\" AS TEXT) LIKE '" + objDashoard.sOfficeCode + "%'";
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name,
                    ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }
        public string GetComissionPendingCount(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT COUNT(*) FROM \"TBLDTCFAILURE\" A,\"TBLESTIMATIONDETAILS\",\"TBLWORKORDER\",\"TBLRECEIVEDTR\", ";
                strQry += " \"VIEW_MINORFAILURE_PENDING\" B WHERE A.\"DF_ID\"=\"EST_FAILUREID\" and A.\"DF_ID\"=\"WO_DF_ID\" AND ";
                strQry += " \"DT_CODE\" =\"DF_DTC_CODE\" AND \"RD_WO_SLNO\"=\"WO_SLNO\" AND \"EST_FAIL_TYPE\"='1' AND \"TMC_ID\" IS NULL ";
                strQry += " AND LENGTH(CAST(B.\"RD_ID\" AS TEXT)) >=1 AND \"DF_REPLACE_FLAG\"='0' AND CAST(\"OM_CODE\" AS TEXT) ";
                strQry += " LIKE '" + objDashoard.sOfficeCode + "%'";
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        ///// <summary>
        ///// Get count of pending Indent
        ///// </summary>
        ///// <param name="objDashoard"></param>
        ///// <returns></returns>
        public string GetIndentPendingCount(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT COUNT(*) FROM \"WORKFLOWSTATUSDUMMY\" A,\"TBLDTCFAILURE\",\"TBLWORKFLOWOBJECTS\" B,\"TBLESTIMATIONDETAILS\" ";
                strQry += " WHERE \"FAILURE\" = CAST(\"WO_ID\" AS TEXT) AND \"WO_RECORD_ID\"=\"DF_ID\" AND \"DF_ID\"=\"EST_FAILUREID\" ";
                strQry += " AND \"EST_FAIL_TYPE\"='2' AND B.\"WO_BO_ID\"='9'  AND \"INDENT\"  IS NULL AND \"WORKORDER\" IS NOT NULL ";
                strQry += " AND A.\"WO_BO_ID\"<>10 AND CAST(\"OFFCODE\" AS TEXT) LIKE '" + objDashoard.sOfficeCode + "%'";

                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }


        ///// <summary>
        ///// Get count of pending Invoice
        ///// </summary>
        ///// <param name="objDashoard"></param>
        ///// <returns></returns>
        public string GetInvoicePendingCount(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT COUNT(*) FROM \"WORKFLOWSTATUSDUMMY\" WHERE \"INVOICE\" IS NULL AND \"INDENT\" IS NOT NULL AND ";
                strQry += " \"WO_BO_ID\"<>10 AND CAST(\"OFFCODE\" AS TEXT) LIKE '" + objDashoard.sOfficeCode + "%'";
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name,
                    ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        public string GetDecommissionPendingCount(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT COUNT(*) FROM \"WORKFLOWSTATUSDUMMY\" WHERE \"INVOICE\" IS NOT NULL AND \"DECOMMISION\" ";
                strQry += " IS NULL AND \"WO_BO_ID\"<>10 AND CAST(\"OFFCODE\" AS TEXT) LIKE '" + objDashoard.sOfficeCode + "%'";
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name,
                    ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        ///// <summary>
        ///// Get count of pending RI
        ///// </summary>
        ///// <param name="objDashoard"></param>
        ///// <returns></returns>
        public string GetRIPendingCount(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT COUNT(*) FROM \"WORKFLOWSTATUSDUMMY\" WHERE (\"CRREPORT\" IS NULL)  AND (\"INVOICE\" ";
                strQry += " IS NOT NULL AND \"DECOMMISION\" IS NOT NULL) AND \"WO_BO_ID\"<>10 AND CAST(\"OFFCODE\" AS TEXT)";
                strQry += " LIKE '" + objDashoard.sOfficeCode + "%'";
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name,
                    ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        public string GetTotalPendingCount(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;
                strQry = "WITH WORKFLOWSTATUS as (SELECT * FROM \"WORKFLOWSTATUSDUMMY\" ) SELECT sum(COALESCE(\"TOTAL_PENDING\" ";
                strQry += " ,0)-COALESCE(\"CR_PENDING\",0) -COALESCE(\"DECOMM_PENDING\",0)) FROM ";
                strQry += " (SELECT COUNT(*)AS \"TOTAL_PENDING\" , \"OFFCODE\" FROM ";
                strQry += " WORKFLOWSTATUS,\"VIEW_MINORFAILURE_PENDING\"  WHERE  \"DT_CODE\" = \"WO_DATA_ID\" AND \"CRREPORT\" ";
                strQry += " IS NULL  AND CAST(\"OFFCODE\" AS TEXT) LIKE '" + objDashoard.sOfficeCode + "%' ";
                strQry += " AND \"WO_BO_ID\"<>10 GROUP BY \"OFFCODE\")A ";
                strQry += "  left JOIN (SELECT COUNT(*) AS \"CR_PENDING\",  \"OFFCODE\" FROM WORKFLOWSTATUS ";
                strQry += " WHERE(\"CRREPORT\" IS NULL)  AND (\"INVOICE\" IS NOT NULL AND \"DECOMMISION\" IS NOT NULL) AND ";
                strQry += " CAST(\"OFFCODE\" AS TEXT) LIKE '" + objDashoard.sOfficeCode + "%' AND \"WO_BO_ID\"<>10 GROUP BY \"OFFCODE\")B ";
                strQry += " ON A.\"OFFCODE\" = B.\"OFFCODE\" LEFT JOIN (SELECT COUNT(*)AS \"DECOMM_PENDING\" , \"OFFCODE\" FROM ";
                strQry += " WORKFLOWSTATUS WHERE(\"CRREPORT\" IS NULL AND \"DECOMMISION\" IS NULL ) AND(\"INVOICE\" IS NOT NULL ) ";
                strQry += " AND CAST(\"OFFCODE\" AS TEXT) LIKE '" + objDashoard.sOfficeCode + "%' AND \"WO_BO_ID\"<>10 GROUP BY ";
                strQry += " \"OFFCODE\")C ON A.\"OFFCODE\"= C.\"OFFCODE\"";
                string result = ObjCon.get_value(strQry);
                return result;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        public string getFailureApprovalPendingCount(string sOfficeCode)
        {
            string strQry = "SELECT COUNT(\"WO_DATA_ID\")AS \"TOTAL_FAILUREAPPROVE_PENDING\" FROM  \"WORKFLOWSTATUSDUMMY\",\"VIEW_MINORFAILURE_PENDING\" ";
            strQry += " WHERE  \"DT_CODE\" = \"WO_DATA_ID\" AND \"CRREPORT\" IS NULL  AND CAST(\"OFFCODE\" AS TEXT) LIKE '" + sOfficeCode + "%' ";
            strQry += " AND \"WO_BO_ID\" <> 10 AND \"WO_DF_ID\" IS NULL";
            return ObjCon.get_value(strQry);
        }
        //#endregion
        //#region "Faulty DTR"

        public string GetFaultyTCField(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;

                #region
                //old query for getting count (included CR Count as well)
                //strQry = "SELECT SUM( tc_failedbutnotreturned + tc_failedbutnotmapped)A FROM  (SELECT md_name,off_name,off_code,COALESCE(tc_failedbutnotreturned,0) tc_failedbutnotreturned ";
                //strQry += " from (SELECT tc_capacity,substr  (tc_location_id,0,2)tc_location_id ,count(tc_code) as tc_failedbutnotreturned from TBLTCMASTER inner join  TBLDTCFAILURE  ";
                //strQry += "on TC_CODE=DF_EQUIPMENT_ID INNER JOIN TBLWORKORDER on df_id=wo_df_id INNER JOIN TBLINDENT on wo_slno=TI_WO_SLNO INNER JOIN  TBLDTCINVOICE on in_ti_no=ti_id ";
                //strQry += "LEFT JOIN TBLTCREPLACE on IN_NO= TR_IN_NO WHERE tr_ri_no is NULL or tr_rv_no is null and tc_status='3'  GROUP BY tc_capacity,substr(tc_location_id,0,2))a ";
                //strQry += "RIGHT JOIN (SELECT MD_NAME,OFF_NAME,OFF_CODE  FROM TBLMASTERDATA,VIEW_ALL_OFFICES WHERE UPPER(MD_TYPE)='C' AND  LENGTH  (OFF_CODE)=2)b on MD_NAME=tc_capacity ";
                //strQry += "and tc_location_id=OFF_CODE)a INNER JOIN  (SELECT md_name,off_name,off_code,COALESCE(tc_failedbutnotmapped,0) tc_failedbutnotmapped from (SELECT tc_capacity,";
                //strQry += "substr   (tc_location_id,0,2)tc_location_id ,count(tc_code) as tc_failedbutnotmapped from TBLTCMASTER inner join  TBLDTCFAILURE  on   TC_CODE=DF_EQUIPMENT_ID ";
                //strQry += "LEFT JOIN TBLWORKORDER on df_id=wo_df_id LEFT JOIN TBLINDENT on wo_slno=TI_WO_SLNO  left JOIN   TBLDTCINVOICE on in_ti_no=ti_id  WHERE in_ti_no is NULL ";
                //strQry += "and tc_status='3' GROUP BY tc_capacity,substr(tc_location_id,0,2))a RIGHT JOIN (SELECT MD_NAME,OFF_NAME,OFF_CODE  FROM TBLMASTERDATA,VIEW_ALL_OFFICES WHERE ";
                //strQry += "UPPER(MD_TYPE)='C' AND  LENGTH(OFF_CODE)=2)b on MD_NAME=tc_capacity and tc_location_id=OFF_CODE)b on a.md_name=b.md_name and a.off_name=b.off_name and ";
                //strQry += "a.off_code=b.off_code  AND a.OFF_CODE like '" + objDashoard.sOfficeCode + "%'";
                #endregion

                #region
                //new query for getting count (excluded CR/RI Count )
                //strQry = "SELECT sum(COALESCE(TOTAL_PENDING,0)-COALESCE(CR_PENDING,0)) FROM (SELECT COUNT(*)AS TOTAL_PENDING , OFFCODE FROM WORKFLOWSTATUSDUMMY ";
                //strQry += " WHERE CRREPORT IS NULL AND OFFCODE LIKE '" + objDashoard.sOfficeCode + "%' AND WO_BO_ID<>10 GROUP BY OFFCODE)A left JOIN (SELECT COUNT(*) AS CR_PENDING, ";
                //strQry += "OFFCODE FROM WORKFLOWSTATUSDUMMY WHERE(CRREPORT IS NULL)  AND(INVOICE IS NOT NULL AND DECOMMISION IS NOT NULL AND RIAPPROVE IS NOT NULL) AND ";
                //strQry += "OFFCODE LIKE '" + objDashoard.sOfficeCode + "%' AND WO_BO_ID<>10 GROUP BY OFFCODE)B ON A.OFFCODE = B.OFFCODE";
                #endregion

                #region new query for getting count (Considering only Decomm Count)
                //if (objDashoard.sOfficeCode != "" && objDashoard.sOfficeCode != null)
                //{
                //    strQry = "SELECT COUNT(*) AS \"DECOMM_PENDING\"  FROM \"WORKFLOWSTATUSDUMMY\" WHERE (LENGTH(\"INVOICE\") >0) AND (LENGTH(\"CRREPORT\")  = 0 AND LENGTH(\"DECOMMISION\") = 0 ";
                //    strQry += " AND \"WO_BO_ID\"<> 10) AND CAST(\"OFFCODE\" AS TEXT) LIKE '" + objDashoard.sOfficeCode + "%'";
                //}
                //else
                //{
                //    strQry = "SELECT COUNT(*) AS \"DECOMM_PENDING\"  FROM \"WORKFLOWSTATUSDUMMY\" WHERE (LENGTH(\"INVOICE\") >0) AND (LENGTH(\"CRREPORT\")  = 0 AND LENGTH(\"DECOMMISION\") = 0 ";
                //    strQry += " AND \"WO_BO_ID\"<> 10) ";
                //}

                #endregion

                strQry = "SELECT count(\"TC_ID\") FROM \"TBLTCMASTER\" WHERE \"TC_CURRENT_LOCATION\"='2' and \"TC_STATUS\"='3' ";
                strQry += " AND cast(\"TC_LOCATION_ID\" as text) like '" + objDashoard.sOfficeCode + "%'";
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        public string GetFaultyTCStore(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;

                #region count for only failure transaction(\"
                //strQry = "SELECT \"TC_CODE\", \"TC_CAPACITY\",\"TC_SLNO\",TO_CHARTC_MANF_DATE\",'DD-MON-YYYY') AS \"TC_MANF_DATE\" FROM ";
                //strQry += " \"TBLTCMASTER\" WHERE \"TC_CURRENT_LOCATION\" ='1' AND \"TC_STATUS\"='3' AND \"TC_CODE\" in ";
                //strQry += " (SELECT \"DF_EQUIPMENT_ID\" FROM \"TBLDTCFAILURE\" WHERE cast(\"DF_LOC_CODE\" as text) like '" + sOfficeCode + "%')";
                #endregion

                if (objDashoard.sOfficeCode != "" && objDashoard.sOfficeCode != null)
                {
                    strQry = "SELECT COUNT(\"TC_ID\") FROM \"TBLTCMASTER\" WHERE \"TC_CURRENT_LOCATION\"='1' AND \"TC_STATUS\"='3' ";
                    strQry += " AND CAST(\"TC_LOCATION_ID\" AS TEXT) = '" + clsStoreOffice.GetStoreID(objDashoard.sOfficeCode) + "'";
                }
                else
                {
                    strQry = "SELECT COUNT(\"TC_ID\") FROM \"TBLTCMASTER\" WHERE \"TC_CURRENT_LOCATION\"='1' AND \"TC_STATUS\"='3' ";
                }

                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }
        public string GetFaultyTCRepair(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;

                if (objDashoard.sOfficeCode != "" && objDashoard.sOfficeCode != null)
                {
                    strQry = "SELECT COUNT(\"TC_ID\") FROM \"TBLTCMASTER\" WHERE \"TC_CURRENT_LOCATION\"= '3' AND \"TC_STATUS\"='3' ";
                    strQry += " AND CAST(\"TC_LOCATION_ID\" AS TEXT) = '" + clsStoreOffice.GetStoreID(objDashoard.sOfficeCode) + "'";
                }
                else
                {
                    strQry = "SELECT COUNT(\"TC_ID\") FROM \"TBLTCMASTER\" WHERE \"TC_CURRENT_LOCATION\"= '3' AND \"TC_STATUS\"='3' ";
                }
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        public string GetTotalFaultyTC(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;
                if (objDashoard.sOfficeCode == "" || objDashoard.sOfficeCode == null)
                {
                    strQry = "SELECT \"FIELDCOUNT\"+\"STORECOUNT\"+\"REPAIRCOUNT\" FROM ";
                    strQry += " (SELECT COUNT(\"TC_ID\") \"FIELDCOUNT\" FROM \"TBLTCMASTER\" WHERE \"TC_CURRENT_LOCATION\"='2' AND ";
                    strQry += " \"TC_STATUS\"='3' AND \"TC_CODE\"<>0  ";
                    strQry += " AND CAST(\"TC_LOCATION_ID\" AS TEXT) LIKE '" + objDashoard.sOfficeCode + "%') A,";
                    strQry += " (SELECT COUNT(\"TC_ID\") \"STORECOUNT\" FROM \"TBLTCMASTER\" WHERE \"TC_CURRENT_LOCATION\"='1' AND ";
                    strQry += " \"TC_STATUS\"='3' AND \"TC_CODE\"<>0 ) B,";
                    strQry += " (SELECT COUNT(\"TC_ID\") \"REPAIRCOUNT\" FROM \"TBLTCMASTER\" WHERE \"TC_CURRENT_LOCATION\"='3' AND ";
                    strQry += " \"TC_STATUS\"='3' AND \"TC_CODE\"<>0 ) C";
                }
                else
                {
                    strQry = "SELECT \"FIELDCOUNT\"+\"STORECOUNT\"+\"REPAIRCOUNT\" FROM ";
                    strQry += " (SELECT COUNT(*) \"FIELDCOUNT\" FROM \"TBLTCMASTER\" WHERE \"TC_CURRENT_LOCATION\"='2' ";
                    strQry += " AND \"TC_STATUS\"='3' AND \"TC_CODE\"<>0  ";
                    strQry += " AND CAST(\"TC_LOCATION_ID\" AS TEXT) LIKE '" + objDashoard.sOfficeCode + "%') A,";
                    strQry += " (SELECT COUNT(*) \"STORECOUNT\" FROM \"TBLTCMASTER\" WHERE \"TC_CURRENT_LOCATION\"='1' ";
                    strQry += " AND \"TC_STATUS\"='3' AND \"TC_CODE\"<>0 ";
                    strQry += " AND CAST(\"TC_LOCATION_ID\" AS TEXT) LIKE '" + clsStoreOffice.GetStoreID(objDashoard.sOfficeCode) + "%') B,";
                    strQry += " (SELECT COUNT(*) \"REPAIRCOUNT\" FROM \"TBLTCMASTER\" WHERE \"TC_CURRENT_LOCATION\"='3' AND ";
                    strQry += " \"TC_STATUS\"='3' AND \"TC_CODE\"<>0 ";
                    strQry += " AND CAST(\"TC_LOCATION_ID\" AS TEXT) LIKE '" + clsStoreOffice.GetStoreID(objDashoard.sOfficeCode) + "%') C";
                }
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }
        //#endregion
        //#region Inbox status
        public string GetPendingWorkflow(clsDashboard objDashoard)
        {
            DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
            try
            {
                //string strQry = string.Empty;
                //strQry = "SELECT \"WFOTABLE\"+\"AUTOTABLE\" AS TOTAL_PENDING FROM ";
                //strQry += " (SELECT COUNT(*) \"WFOTABLE\"  FROM \"TBLWORKFLOWOBJECTS\" WHERE CAST(\"WO_REF_OFFCODE\" AS TEXT) ";
                //strQry += " LIKE '" + objDashoard.sOfficeCode + "%'  ";
                //strQry += " AND \"WO_APPROVE_STATUS\" ='0' AND \"WO_NEXT_ROLE\" ='" + objDashoard.sRoleId + "') A, ";
                //strQry += "  (SELECT COUNT(*) \"AUTOTABLE\" FROM \"TBLWO_OBJECT_AUTO\" WHERE CAST(\"WOA_REF_OFFCODE\" AS TEXT) ";
                //strQry += " LIKE '" + objDashoard.sOfficeCode + "%' ";
                //strQry += " AND \"WOA_INITIAL_ACTION_ID\" IS NULL AND \"WOA_ROLE_ID\" ='" + objDashoard.sRoleId + "' ) B ";
                //return ObjCon.get_value(strQry);

                NpgsqlCommand cmd = new NpgsqlCommand("dashboard_get_pend_count_work_flow");
                cmd.Parameters.AddWithValue("ofc_code", (objDashoard.sOfficeCode ?? ""));
                cmd.Parameters.AddWithValue("role_id", (objDashoard.sRoleId ?? ""));
                return objDatabse.StringGetValue(cmd);

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        public string GetApprovedWorkflow(clsDashboard objDashoard)
        {
            DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
            try
            {
                //string strQry = string.Empty;
                //strQry = " SELECT \"APPROVED\"+\"APPROVED_AUTO\" FROM ";
                //strQry += "(SELECT COUNT(*) \"APPROVED\"  FROM \"TBLWORKFLOWOBJECTS\" WHERE CAST(\"WO_REF_OFFCODE\" AS TEXT) LIKE ";
                //strQry += " '" + objDashoard.sOfficeCode + "%'  ";
                //strQry += " AND \"WO_APPROVE_STATUS\" IN ('1','2') AND \"WO_NEXT_ROLE\" ='" + objDashoard.sRoleId + "') A,";
                //strQry += " (SELECT COUNT(*) \"APPROVED_AUTO\"  FROM \"TBLWO_OBJECT_AUTO\" WHERE CAST(\"WOA_REF_OFFCODE\" AS TEXT) LIKE ";
                //strQry += " '" + objDashoard.sOfficeCode + "%'";
                //strQry += " AND \"WOA_INITIAL_ACTION_ID\" IS NOT NULL AND \"WOA_ROLE_ID\" ='" + objDashoard.sRoleId + "') B";
                //return ObjCon.get_value(strQry);

                NpgsqlCommand cmd = new NpgsqlCommand("dashboard_get_approved_count_work_flow");
                cmd.Parameters.AddWithValue("ofc_code", (objDashoard.sOfficeCode ?? ""));
                cmd.Parameters.AddWithValue("role_id", (objDashoard.sRoleId ?? ""));
                return objDatabse.StringGetValue(cmd);

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        public string GetRejectedWorkflow(clsDashboard objDashoard)
        {
            DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
            try
            {
                //string strQry = string.Empty;
                //strQry = "SELECT COUNT(*) \"REJECTED\"  FROM \"TBLWORKFLOWOBJECTS\" WHERE CAST(\"WO_REF_OFFCODE\" AS TEXT) LIKE ";
                //strQry += " '" + objDashoard.sOfficeCode + "%'  ";
                //strQry += " AND \"WO_APPROVE_STATUS\" ='3' AND \"WO_NEXT_ROLE\" ='" + objDashoard.sRoleId + "'";
                //return ObjCon.get_value(strQry);

                NpgsqlCommand cmd = new NpgsqlCommand("dashboard_get_rejected_count_work_flow");
                cmd.Parameters.AddWithValue("ofc_code", (objDashoard.sOfficeCode ?? ""));
                cmd.Parameters.AddWithValue("role_id", (objDashoard.sRoleId ?? ""));
                return objDatabse.StringGetValue(cmd);

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        public string GetTotalWorkflow(clsDashboard objDashoard)
        {
            DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
            try
            {
                //string strQry = string.Empty;
                //strQry = "SELECT \"TOTAL_PENDING\"+\"APPROVED\"+\"REJECTED\" FROM ";
                //strQry += " (SELECT A.\"WFOTABLE\"+B.\"AUTOTABLE\" AS \"TOTAL_PENDING\" FROM ";
                //strQry += " (SELECT COUNT(*) \"WFOTABLE\"  FROM \"TBLWORKFLOWOBJECTS\" WHERE CAST(\"WO_REF_OFFCODE\" AS TEXT) ";
                //strQry += " LIKE '" + objDashoard.sOfficeCode + "%'  ";
                //strQry += " AND \"WO_APPROVE_STATUS\" ='0' AND \"WO_NEXT_ROLE\" ='" + objDashoard.sRoleId + "') A,";
                //strQry += " (SELECT COUNT(*) \"AUTOTABLE\" FROM \"TBLWO_OBJECT_AUTO\" WHERE CAST(\"WOA_REF_OFFCODE\" AS TEXT) ";
                //strQry += " LIKE '" + objDashoard.sOfficeCode + "%' ";
                //strQry += " AND \"WOA_INITIAL_ACTION_ID\" IS NULL AND \"WOA_ROLE_ID\" ='" + objDashoard.sRoleId + "' ) B ) X,";
                //strQry += " (SELECT \"APPROVED\"+\"APPROVED_AUTO\" AS \"APPROVED\" FROM ";
                //strQry += "(SELECT COUNT(*) \"APPROVED\"  FROM \"TBLWORKFLOWOBJECTS\" WHERE CAST(\"WO_REF_OFFCODE\" AS TEXT) LIKE ";
                //strQry += " '" + objDashoard.sOfficeCode + "%'  ";
                //strQry += " AND \"WO_APPROVE_STATUS\" IN ('1','2') AND \"WO_NEXT_ROLE\" ='" + objDashoard.sRoleId + "') A,";
                //strQry += " (SELECT COUNT(*) \"APPROVED_AUTO\"  FROM \"TBLWO_OBJECT_AUTO\" WHERE CAST(\"WOA_REF_OFFCODE\" AS TEXT) LIKE ";
                //strQry += " '" + objDashoard.sOfficeCode + "%'";
                //strQry += " AND \"WOA_INITIAL_ACTION_ID\" IS NOT NULL AND \"WOA_ROLE_ID\"='" + objDashoard.sRoleId + "') B) Y,";
                //strQry += " (SELECT COUNT(*) AS \"REJECTED\"  FROM \"TBLWORKFLOWOBJECTS\" WHERE CAST(\"WO_REF_OFFCODE\" AS TEXT) LIKE ";
                //strQry += " '" + objDashoard.sOfficeCode + "%' ";
                //strQry += " AND \"WO_APPROVE_STATUS\" ='3' AND \"WO_NEXT_ROLE\" ='" + objDashoard.sRoleId + "') Z";
                //return ObjCon.get_value(strQry);

                NpgsqlCommand cmd = new NpgsqlCommand("dashboard_get_total_count_work_flow");
                cmd.Parameters.AddWithValue("ofc_code", (objDashoard.sOfficeCode ?? ""));
                cmd.Parameters.AddWithValue("role_id", (objDashoard.sRoleId ?? ""));
                return objDatabse.StringGetValue(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        //#endregion
        public DataTable LoadBarGraph(string sOfficeCode)
        {
            DataTable dtBarGraph = new DataTable();
            try
            {
                string strQry = string.Empty;
                string previousYear = ObjCon.get_value("SELECT to_char(date_trunc('YEAR', now() - '1 year'::interval), 'yyyy')");
                string presentYear = ObjCon.get_value("SELECT to_char(date_trunc('YEAR', now()- '0 year' ::interval), 'yyyy')");
                strQry = " SELECT COALESCE(\"PRESENTYEAR\",TO_CHAR(NOW(),'YYYY')) \"PRESENTYEAR\",\"MONTHS\" AS \"PRESENTMONTH\", ";
                strQry += " COALESCE(\"PRESENTCOUNT\",0) \"PRESENTCOUNT\",COALESCE(\"PREVIOUSYEAR\", to_char(date_trunc('YEAR', CURRENT_DATE) - INTERVAL '1 year','yyyy')) ";
                strQry += " \"PREVIOUSYEAR\",  \"MONTHS\" AS \"PREVIOUSMONTH\", COALESCE(\"PREVIOUSCOUNT\",0) \"PREVIOUSCOUNT\",'' as \"TESTYEAR\", ";
                strQry += " '' as \"TESTMONTH\",'0' as \"TESTCOUNT\" FROM (SELECT TO_CHAR(generate_series(timestamp without time zone '" + previousYear + "-04-01', ";
                strQry += " timestamp without time zone '" + presentYear + "-03-01', '1 Month'), 'MON') \"MONTHS\",TO_CHAR(generate_series(timestamp without time ";
                strQry += " zone '" + previousYear + "-04-01', timestamp without time zone '" + presentYear + "-03-01','1 Month'),'MM') \"MON\")C LEFT JOIN  (SELECT ";
                strQry += " TO_CHAR(\"DF_DATE\",'YYYY') AS \"PRESENTYEAR\",TO_CHAR(\"DF_DATE\",'MON') AS \"PRESENTMONTH\", COUNT(\"DF_DTC_CODE\") ";
                strQry += " AS \"PRESENTCOUNT\" FROM \"TBLDTCFAILURE\" WHERE TO_CHAR(\"DF_DATE\",'YYYY') = TO_CHAR(NOW(),'YYYY') AND ";
                strQry += " CAST(\"DF_LOC_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%' AND \"DF_STATUS_FLAG\" IN (1,4) GROUP BY TO_CHAR(\"DF_DATE\",'MON'), ";
                strQry += " TO_CHAR(\"DF_DATE\",'YYYY'))A ON C.\"MONTHS\" = A.\"PRESENTMONTH\" LEFT JOIN (SELECT TO_CHAR(\"DF_DATE\",'YYYY') ";
                strQry += " AS \"PREVIOUSYEAR\",TO_CHAR(\"DF_DATE\",'MON') AS \"PREVIOUSMONTH\", COUNT(\"DF_DTC_CODE\")AS \"PREVIOUSCOUNT\" ";
                strQry += " FROM \"TBLDTCFAILURE\" WHERE TO_CHAR(\"DF_DATE\",'YYYY') = to_char(date_trunc('YEAR', CURRENT_DATE) - INTERVAL '1 year','yyyy') AND CAST(\"DF_LOC_CODE\" ";
                strQry += " AS TEXT) LIKE '" + sOfficeCode + "%' AND \"DF_STATUS_FLAG\" IN (1,4) GROUP BY TO_CHAR(\"DF_DATE\",'MON'),TO_CHAR(\"DF_DATE\",'YYYY'))B ";
                strQry += " ON C.\"MONTHS\" = B.\"PREVIOUSMONTH\"";
                dtBarGraph = ObjCon.FetchDataTable(strQry);
                return dtBarGraph;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtBarGraph;
            }
        }
        //if added this one hear for Failure Reson Chart GRAF
        public DataTable LoadBarGraphforFailureResonChart(string sOfficeCode)
        {
            DataTable dtBarGraphReson = new DataTable();
            try
            {
                string strQry = string.Empty;
                strQry += " SELECT  C.\"REASON\" AS \"REASON\" , COALESCE(a.\"PRESENTCOUNT\", 0) \"PRESENTCOUNT\" FROM (SELECT  \"MD_NAME\" AS \"REASON\", 0 AS \"PRESENTCOUNT\" ";
                strQry += " FROM \"TBLMASTERDATA\" WHERE   \"MD_TYPE\" = 'FT'  ORDER BY \"MD_NAME\")C LEFT JOIN (SELECT \"MD_NAME\" AS \"REASON\", COUNT(\"DF_DTC_CODE\")  AS \"PRESENTCOUNT\" ";
                strQry += " FROM \"TBLDTCFAILURE\"  INNER JOIN \"TBLMASTERDATA\" ON \"DF_FAILURE_TYPE\" = \"MD_ID\" AND \"MD_TYPE\" = 'FT' ";
                strQry += " WHERE CAST(\"DF_LOC_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%' AND \"DF_STATUS_FLAG\" IN(1, 4)  GROUP BY  \"MD_NAME\" ORDER BY \"MD_NAME\")A ON C.\"REASON\" = A.\"REASON\" ";
                dtBarGraphReson = ObjCon.FetchDataTable(strQry);
                return dtBarGraphReson;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtBarGraphReson;
            }
        }
        //#region View Failure Pending Details
        public DataTable LoadFailurePendingDetails(string sOfficeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                //string strQry = string.Empty;
                //strQry = "SELECT distinct \"DT_CODE\",\"DT_NAME\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" ";
                //strQry += " where \"OFF_CODE\"=\"TRANS_REF_OFF_CODE\") as \"OMSECTION\",\"TRANS_REF_OFF_CODE\" AS \"OM_CODE\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,";
                //strQry += " length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" where cast(\"OFF_CODE\" as text)=SUBSTRING(cast(\"TRANS_REF_OFF_CODE\" as text),1,4)) as \"SUBDIVSION\",";
                //strQry += "(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" where cast(\"OFF_CODE\" as text)=SUBSTRING(cast(\"TRANS_REF_OFF_CODE\" as text),1,3)) as \"DIVSION\",\"DF_ID\",";
                //strQry += " TO_CHAR(\"DF_DATE\",'dd-MON-yyyy')\"DF_DATE\",'PENDING WITH '||\"RO_NAME\" ||' SINCE '||date_part('day',age(CURRENT_DATE,CAST(\"TRANS_UPDATE_DATE\" AS DATE)))||' Days' as  \"FL_STATUS\"   from \"TBLPENDINGTRANSACTION\" inner join \"TBLROLES\" on \"RO_ID\"=\"TRANS_NEXT_ROLE_ID\" ";
                //strQry += " inner join \"TBLDTCMAST\" on \"DT_CODE\"=\"TRANS_DTC_CODE\" left join  \"TBLDTCFAILURE\" ";
                //strQry += " on \"DF_DTC_CODE\"=\"TRANS_DTC_CODE\" and  \"DF_REPLACE_FLAG\"=0 where \"TRANS_BO_ID\" NOT IN (15,26,10,71,72,75,76,77,78,79) AND CAST(\"TRANS_REF_OFF_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%' and \"TRANS_NEXT_ROLE_ID\"<>0 ";

                //dt = ObjCon.FetchDataTable(strQry);
                //return dt;
                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_failure_pending_details");
                cmd.Parameters.AddWithValue("ofc_code", sOfficeCode);
                dt = ObjCon.FetchDataTable(cmd);
                return dt;


            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable LoadFailureApprovalPendingDetails(string sOfficeCode)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            strQry = "SELECT \"DT_CODE\",\"DT_NAME\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\"";
            strQry += " where \"OFF_CODE\"=\"TRANS_REF_OFF_CODE\") as \"OMSECTION\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) ";
            strQry += "  from \"VIEW_ALL_OFFICES\" where cast(\"OFF_CODE\" as text)=SUBSTRING(cast(\"TRANS_REF_OFF_CODE\" as text),1,4)) ";
            strQry += " as \"SUBDIVSION\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from ";
            strQry += " \"VIEW_ALL_OFFICES\" where cast(\"OFF_CODE\" as text)=SUBSTRING(cast(\"TRANS_REF_OFF_CODE\" as text),1,3)) as \"DIVSION\"";
            strQry += ",'PENDING WITH ' ||\"RO_NAME\" ||' SINCE '||date_part('day',age(CURRENT_DATE,CAST(\"TRANS_UPDATE_DATE\" AS DATE)))||' Days' as \"FL_STATUS\" from \"TBLPENDINGTRANSACTION\"";
            strQry += "  INNER JOIN \"TBLDTCMAST\" ON \"DT_CODE\"=\"TRANS_DTC_CODE\" INNER JOIN \"TBLROLES\" ON \"RO_ID\"=\"TRANS_NEXT_ROLE_ID\" WHERE \"TRANS_BO_ID\"=9  AND CAST(\"TRANS_REF_OFF_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%'";
            dt = ObjCon.FetchDataTable(strQry);
            return dt;
        }

        //#endregion
        //#region DTC Failure Abstract
        public DataTable LoadDTCFailureAbstract(clsDashboard objDashboard)
        {
            string strQry = string.Empty;
            DataTable dt = new DataTable();

            try
            {
                int Length = 0;
                Length = objDashboard.sOfficeCode.Length + 1;

                strQry = " SELECT \"TC_CAPACITY\", \"DF_LOC_CODE\", \"SECTION\", SUM(\"FAILURECOUNTOFYEAR\") \"FAILURECOUNTOFYEAR\", ";
                strQry += " SUM(\"CURRENTQUARTER\") \"CURRENTQUARTER\", SUM(\"CURRENTMONTH\") \"CURRENTMONTH\", SUM(\"PREVIOUSMONTH\") \"PREVIOUSMONTH\" FROM ";
                strQry += " (SELECT \"TC_CAPACITY\", SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Length + ") \"DF_LOC_CODE\", (SELECT \"OFF_NAME\" AS \"OFFICENAME\" ";
                strQry += " FROM \"VIEW_OFFICES\" WHERE SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Length + ") = CAST(\"OFF_CODE\" AS TEXT)) AS ";
                strQry += " \"SECTION\", COUNT(*) AS \"FAILURECOUNTOFYEAR\", ";
                strQry += " SUM(CASE WHEN TO_CHAR(\"DF_DATE\",'MONTH') IN  (SELECT \"MONTH\" FROM (SELECT TO_CHAR(\"MONTH\",'MONTH') \"MONTH\" ";
                strQry += " , CAST(DATE_PART('QUARTER', \"MONTH\")AS TEXT) \"QUARTER\" FROM (SELECT CURRENT_DATE + (INTERVAL '1' MONTH * ";
                strQry += " GENERATE_SERIES(0,11)) \"MONTH\")A)B WHERE \"QUARTER\" = (SELECT  TO_CHAR(CURRENT_DATE,'Q') \"CURRENTQUARTER\")) ";
                strQry += " THEN 1 ELSE 0 END ) \"CURRENTQUARTER\",  SUM(CASE WHEN TO_CHAR(\"DF_DATE\",'MONTH') = TO_CHAR(CURRENT_DATE,'MONTH') ";
                strQry += " THEN 1 ELSE 0 END) \"CURRENTMONTH\",SUM(CASE WHEN TO_CHAR(\"DF_DATE\",'MMYYYY') = (SELECT to_char(\"MONTH\",'MMYYYY')";
                strQry += " FROM(SELECT DATE_TRUNC('month', current_date - interval '1' month) \"MONTH\")A) THEN 1 ELSE 0 END) \"PREVIOUSMONTH\" ";
                strQry += " FROM \"TBLDTCFAILURE\", \"TBLTCMASTER\" WHERE \"DF_EQUIPMENT_ID\" = \"TC_CODE\" AND TO_CHAR(\"DF_DATE\",'YYYY') = ";
                strQry += " (SELECT TO_CHAR(\"MONTH\",'YYYY') FROM(SELECT DATE_TRUNC('month', current_date ) \"MONTH\")A)  AND ";
                strQry += " CAST(\"DF_LOC_CODE\" AS TEXT) LIKE '" + objDashboard.sOfficeCode + "%' AND \"DF_STATUS_FLAG\" IN (1,4) ";
                if (objDashboard.sCapacity != null)
                {
                    strQry += " AND \"TC_CAPACITY\" = '" + objDashboard.sCapacity + "' ";
                }
                strQry += " GROUP BY \"TC_CAPACITY\", \"DF_LOC_CODE\")Z GROUP BY \"TC_CAPACITY\", \"DF_LOC_CODE\", \"SECTION\"  ORDER BY \"TC_CAPACITY\"";

                dt = ObjCon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        //#endregion

        public DataTable LoadFaultyDTRDetails(string sOfficeCode)
        {

            DataTable dtCompleteDetails = new DataTable();
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT \"TC_CODE\",\"TM_NAME\",\"TC_SLNO\",\"TC_CAPACITY\",\"DT_CODE\",\"DT_NAME\",\"TR_RI_NO\",\"TR_RI_DATE\", ";
                strQry += " \"SM_NAME\",\"SUP_REPNAME\",\"RSM_ISSUE_DATE\", \"SUP_INSP_DATE\",\"INSP_BY\" FROM ";
                strQry += " (SELECT DISTINCT \"TC_CODE\",\"TC_LOCATION_ID\",\"TM_NAME\",\"TC_SLNO\",CAST(\"TC_CAPACITY\" AS TEXT)\"TC_CAPACITY\" FROM ";
                strQry += " \"TBLTCMASTER\",\"TBLTRANSMAKES\" TM WHERE \"TC_MAKE_ID\"=\"TM_ID\" AND \"TC_STATUS\"=3 )A LEFT JOIN ";
                strQry += " (SELECT \"DT_CODE\",\"DT_NAME\",\"DT_TC_ID\" FROM \"TBLDTCMAST\", \"TBLTCMASTER\" WHERE \"TC_CURRENT_LOCATION\"=2  ";
                strQry += " AND \"TC_CODE\"=\"DT_TC_ID\" AND \"TC_CODE\"<>0)B ON B.\"DT_TC_ID\"=A.\"TC_CODE\" LEFT JOIN ";
                strQry += " (SELECT \"SM_NAME\",\"TR_RI_NO\",\"FAIL_TC_CODE\",\"TR_RI_DATE\",\"TR_ID\" FROM (SELECT DISTINCT SM.\"SM_NAME\" AS ";
                strQry += " \"SM_NAME\",\"TR_RI_NO\",\"FAIL_TC_CODE\",TO_CHAR(\"TR_RI_DATE\",'DD-MON-YYYY') \"TR_RI_DATE\",\"TR_ID\" FROM ";
                strQry += " \"VIEWFAILTCCODE\" FT,\"TBLTCMASTER\",\"TBLSTOREMAST\" SM  WHERE \"TC_CURRENT_LOCATION\"<>2 AND  \"TC_CODE\" = ";
                strQry += " \"FAIL_TC_CODE\" AND \"TC_STORE_ID\"=\"SM_ID\" )A INNER JOIN (SELECT  MAX(\"TR_ID\") \"TR_IDD\",\"FAIL_TC_CODE\" AS ";
                strQry += " \"FAIL_TC_CODE1\"  FROM \"VIEWFAILTCCODE\" FT,\"TBLTCMASTER\",\"TBLSTOREMAST\" SM  WHERE \"TC_CURRENT_LOCATION\"<>2 ";
                strQry += " AND \"TC_CODE\"=\"FAIL_TC_CODE\" AND  \"TC_STORE_ID\" =\"SM_ID\" GROUP BY \"FAIL_TC_CODE\" )B ON \"TR_IDD\"=\"TR_ID\" )C ";
                strQry += " ON A.\"TC_CODE\"=C.\"FAIL_TC_CODE\"  LEFT JOIN  (SELECT DISTINCT(CASE WHEN \"RSM_SUPREP_TYPE\"='2' THEN ";
                strQry += " (SELECT \"TR_NAME\" FROM \"TBLTRANSREPAIRER\" TR WHERE TR.\"TR_ID\"=\"RSM_SUPREP_ID\") WHEN \"RSM_SUPREP_TYPE\"='1' ";
                strQry += " THEN (SELECT \"TS_NAME\" FROM \"TBLTRANSSUPPLIER\"  WHERE \"TS_ID\"=\"RSM_SUPREP_ID\") END ) \"SUP_REPNAME\", ";
                strQry += " \"RSD_TC_CODE\" ,TO_CHAR(\"RSM_ISSUE_DATE\",'DD-MON-YYYY') \"RSM_ISSUE_DATE\", \"RSM_ID\",\"RSD_RSM_ID\" FROM ";
                strQry += " \"TBLREPAIRSENTDETAILS\",\"TBLREPAIRSENTMASTER\",\"TBLTCMASTER\" WHERE \"TC_CURRENT_LOCATION\"=3 AND \"RSM_ID\"= ";
                strQry += " \"RSD_RSM_ID\" AND \"TC_CODE\"=\"RSD_TC_CODE\" AND \"RSD_ID\" IN (SELECT \"RSD_ID\" FROM (SELECT DISTINCT ";
                strQry += " \"RSD_TC_CODE\", MAX(\"RSD_ID\") \"RSD_ID\" FROM \"TBLREPAIRSENTDETAILS\",\"TBLREPAIRSENTMASTER\",\"TBLTCMASTER\" ";
                strQry += " WHERE \"TC_CURRENT_LOCATION\"=3 AND \"RSM_ID\" =\"RSD_RSM_ID\" AND \"TC_CODE\"=\"RSD_TC_CODE\" GROUP BY ";
                strQry += " \"RSD_TC_CODE\")Z))D ON A.\"TC_CODE\"=D.\"RSD_TC_CODE\" LEFT JOIN  (SELECT TO_CHAR(\"IND_INSP_DATE\",'DD-MON-YYYY') ";
                strQry += " AS \"SUP_INSP_DATE\",\"US_FULL_NAME\" AS \"INSP_BY\",\"RSD_TC_CODE\" FROM \"TBLREPAIRSENTDETAILS\", ";
                strQry += " \"TBLINSPECTIONDETAILS\",\"TBLUSER\", \"TBLTCMASTER\" WHERE \"TC_CURRENT_LOCATION\"=3 AND \"IND_INSP_BY\"=\"US_ID\" ";
                strQry += " AND \"IND_RSD_ID\"=\"RSD_ID\" AND \"TC_CODE\"=\"RSD_TC_CODE\")E ON E.\"RSD_TC_CODE\"=A.\"TC_CODE\" WHERE  ";
                strQry += " CAST(\"TC_LOCATION_ID\" AS TEXT) LIKE '" + sOfficeCode + "%' ORDER BY \"TC_CODE\"";

                dtCompleteDetails = ObjCon.FetchDataTable(strQry);
                return dtCompleteDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtCompleteDetails;
            }
        }


        //#region DTC Count

        public string GetTotalDTCCount(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT COUNT(\"DT_CODE\") FROM \"TBLDTCMAST\" WHERE CAST(\"DT_OM_SLNO\" AS TEXT) LIKE '" + objDashoard.sOfficeCode + "%' ";
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        //#endregion
        //#region Approval Pending

        public DataTable LoadApprovalPendingDetails(string sOfficeCode, string sBOType, string sRoleType)
        {

            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                strQry = " SELECT  \"TRANS_DTC_CODE\" as \"DT_CODE\",(SELECT \"DT_NAME\" from \"TBLDTCMAST\" where \"DT_CODE\" = \"TRANS_DTC_CODE\")as \"DT_NAME\",(select substring(\"OFF_NAME\", position(':' in \"OFF_NAME\") + 2, length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" where \"OFF_CODE\" = \"TRANS_REF_OFF_CODE\" limit 1) as \"OMSECTION\",";
                strQry += "  \"TRANS_REF_OFF_CODE\" AS \"OM_CODE\",(SELECT \"DF_ID\" from \"TBLDTCFAILURE\" WHERE \"DF_DTC_CODE\" = \"TRANS_DTC_CODE\" and \"DF_STATUS_FLAG\" <> '0' limit 1)\"DF_ID\", 'PENDING WITH ' || \"RO_NAME\" || '(AT  ' || \"BO_NAME\" || ')' as  \"STATUS\" from \"TBLPENDINGTRANSACTION\",\"TBLROLES\",\"TBLBUSINESSOBJECT\" WHERE ";
                strQry += " \"RO_ID\" = \"TRANS_NEXT_ROLE_ID\"  and \"BO_ID\" = \"TRANS_BO_ID\"";

                //FAILURE ENTRY
                if (sBOType == "9")
                {
                    strQry += " AND \"TRANS_BO_ID\"='9'";
                }

                //ENHANCEMENT ENTRY
                else if (sBOType == "10")
                {
                    strQry += " AND \"TRANS_BO_ID\"='10'";
                }
                //ENHANCEMENT ENTRY
                else if (sBOType == "45")
                {
                    strQry += " AND \"TRANS_BO_ID\"='45'";
                }
                //WORK ORDER
                else if (sBOType == "11")
                {
                    strQry += " AND \"TRANS_BO_ID\"='11'";
                }

                else if (sBOType == "74")
                {
                    strQry += " AND \"TRANS_BO_ID\"='74'";
                }

                else if (sBOType == "73")
                {
                    strQry += " AND \"TRANS_BO_ID\"='73'";
                }

                //INDENT
                else if (sBOType == "12")
                {
                    strQry += " AND \"TRANS_BO_ID\"='12'";
                }

                //INVOICE
                else if (sBOType == "13")
                {
                    strQry += " AND \"TRANS_BO_ID\"='13'";
                }

                //DECOMMISSION
                else if (sBOType == "14")
                {
                    strQry += " AND \"TRANS_BO_ID\"='14'";
                }

                //RI
                else if (sBOType == "15")
                {
                    strQry += " AND \"TRANS_BO_ID\"='15'";
                }

                //CR REPORT
                else if (sBOType == "26")
                {
                    strQry += " AND \"TRANS_BO_ID\"='26'";
                }
                //if(sOfficeCode != null && sOfficeCode != "")
                //{
                //    strQry += " AND cast(\"OM_CODE\" as text) LIKE ";
                //}

                if (sRoleType == "2")
                {
                    strQry += "AND ";
                    string sOffCode = clsStoreOffice.GetOfficeCode(sOfficeCode, "TRANS_REF_OFF_CODE");
                    strQry += sOffCode;
                }
                else if (sRoleType == "1")
                {
                    strQry += " AND CAST(\"TRANS_REF_OFF_CODE\"  AS TEXT) LIKE '" + sOfficeCode + "%'"; // before it is WO_REF_OFFCODE with like condition
                }
                else
                {
                    strQry += " AND cast(\"TRANS_REF_OFF_CODE\" as text) LIKE '%'";
                }

                strQry += " and \"TRANS_NEXT_ROLE_ID\"<>0 ORDER BY \"DF_ID\"";

                //strQry += " SELECT * FROM  (SELECT \"DT_CODE\",\"DT_NAME\",\"OMSECTION\",CASE ";
                //strQry += " WHEN \"FL_STATUS\" IS NULL THEN 'FAILURE/ENHANCEMENT : ' || \"FL_STATUS\" ";
                //strQry += " WHEN \"WO_STATUS\" IS NULL THEN 'WORK ORDER : ' || \"WO_STATUS\"    ";
                //strQry += " WHEN \"INDT_STATUS\" IS NULL THEN 'INDENT : ' || \"INDT_STATUS\" ";
                //strQry += " WHEN \"INV_STATUS\" IS NULL THEN 'INVOICE : ' || \"INV_STATUS\" ";
                //strQry += " WHEN \"DECOMM_STATUS\" IS NULL THEN 'DECOMMISSION : ' || \"DECOMM_STATUS\" ";
                //strQry += " WHEN \"RI_STATUS\" IS NULL THEN 'RI APPROVE : ' || \"RI_STATUS\" ";
                //strQry += " WHEN \"CR_STATUS\" IS NULL THEN 'CR REPORT : ' || \"CR_STATUS\" ";
                //strQry += " ELSE '' END \"STATUS\" FROM \"VIEWPENDINGAPPROVAL\" ";
                //strQry += "  WHERE CAST(\"OM_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%')A ";

                ////FAILURE ENTRY
                //if (sBOType == "9")
                //{
                //    strQry += " WHERE STATUS LIKE 'FAILURE%'";
                //}

                ////ENHANCEMENT ENTRY
                //if (sBOType == "10")
                //{
                //    strQry += " WHERE STATUS LIKE 'FAILURE%' ";
                //}

                ////WORK ORDER
                //if (sBOType == "11")
                //{
                //    strQry += " WHERE STATUS LIKE 'WORK ORDER%' ";
                //}

                ////INDENT
                //if (sBOType == "12")
                //{
                //    strQry += " WHERE STATUS LIKE 'INDENT%'  ";
                //}

                ////INVOICE
                //if (sBOType == "13")
                //{
                //    strQry += " WHERE STATUS LIKE 'INVOICE%' ";
                //}

                ////DECOMMISSION
                //if (sBOType == "14")
                //{
                //    strQry += " WHERE STATUS LIKE 'DECOMMISSION%' ";
                //}

                ////RI
                //if (sBOType == "15")
                //{
                //    strQry += " WHERE STATUS LIKE 'RI APPROVE%'  ";
                //}

                ////CR REPORT
                //if (sBOType == "26")
                //{
                //    strQry += " WHERE STATUS LIKE 'CR%' ";
                //}

                dt = ObjCon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        //#endregion


        public DataTable LoadEstimationPendingDetails(string sOfficeCode)
        {

            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;

                ////strQry = "SELECT \"DF_EQUIPMENT_ID\",\"DT_CODE\",\"DT_NAME\",\"SUBDIVSION\",\"OMSECTION\",A.\"DF_DATE\",\"EST_STATUS\",case when ";
                ////strQry += " \"DAYS_FROM_PENDING\" is null then (\"EST_STATUS\"|| ' SINCE ' || CURRENT_DATE - (SELECT to_date(to_char(\"WO_CR_ON\",'yyyy-mm-dd'),'yyyy-mm-dd') ";
                ////strQry += " FROM \"WORKFLOWSTATUSDUMMY\" WHERE \"WO_DATA_ID\"=\"DT_CODE\") ||' Days') else \"DAYS_FROM_PENDING\" END \"DAYS_FROM_PENDING\" ";
                ////strQry += " FROM (SELECT \"DF_EQUIPMENT_ID\",\"DT_CODE\",\"DT_NAME\",\"SUBDIVSION\",\"OMSECTION\",A.\"DF_DATE\",\"EST_STATUS\",";
                ////strQry += " \"EST_STATUS\"|| ' SINCE ' || CURRENT_DATE - (SELECT to_date(to_char(\"EST_WO_CR_ON\",'yyyy-mm-dd'),'yyyy-mm-dd') FROM ";
                ////strQry += " \"WORKFLOWSTATUSDUMMY\" WHERE \"WO_DATA_ID\"=\"DT_CODE\") ||' Days' AS \"DAYS_FROM_PENDING\" FROM \"VIEWPENDINGFAILURE\" A,";
                ////strQry += " \"WORKFLOWSTATUSDUMMY\",  \"TBLDTCFAILURE\" C WHERE \"OM_CODE\"=\"OFFCODE\" AND CAST(\"OFFCODE\" AS TEXT) ";
                ////strQry += " LIKE '" + sOfficeCode + "%' AND \"WO_BO_ID\" ='9' AND \"WO_DATA_ID\"=\"DT_CODE\" AND \"EST_STATUS\" <>'' AND ";
                ////strQry += " \"WO_DF_ID\" = C.\"DF_ID\")A";
                //strQry = "SELECT \"DT_CODE\",\"DT_NAME\",\"DF_EQUIPMENT_ID\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" ";
                //strQry += "where \"OFF_CODE\"=\"TRANS_REF_OFF_CODE\") as \"OMSECTION\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" ";
                //strQry += "where cast(\"OFF_CODE\" as text)=SUBSTRING(cast(\"TRANS_REF_OFF_CODE\" as text),1,4)) as \"SUBDIVSION\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" where cast(\"OFF_CODE\" as text)=SUBSTRING(cast(\"TRANS_REF_OFF_CODE\" as text),1,3)) as \"DIVSION\",TO_CHAR(\"DF_DATE\",'dd-MON-yyyy')\"DF_DATE\",'PENDING WITH '||\"RO_NAME\" ||";
                //strQry += "' SINCE ' ||date_part('day',age(CURRENT_DATE,CAST(\"TRANS_UPDATE_DATE\" AS DATE)))||' Days' as \"DAYS_FROM_PENDING\" from \"TBLPENDINGTRANSACTION\" inner join \"TBLDTCMAST\" on \"DT_CODE\"=\"TRANS_DTC_CODE\" inner join";
                //strQry += " \"TBLDTCFAILURE\" on \"DF_DTC_CODE\"=\"TRANS_DTC_CODE\" and  \"DF_REPLACE_FLAG\"=0  INNER JOIN \"TBLROLES\" ON \"RO_ID\"=\"TRANS_NEXT_ROLE_ID\" where \"TRANS_BO_ID\"  in(45) AND CAST(\"TRANS_REF_OFF_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%' order by CAST(\"DF_DATE\" AS TIMESTAMP) desc";
                //dt = ObjCon.FetchDataTable(strQry);
                //return dt;

                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_estimation_pend_details");
                cmd.Parameters.AddWithValue("ofc_code", sOfficeCode);
                dt = ObjCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable LoadWorkorderPendingDetails(string sOfficeCode)
        {

            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;

                ////strQry = "SELECT \"DF_EQUIPMENT_ID\",\"DT_CODE\",\"DT_NAME\",\"SUBDIVSION\",\"OMSECTION\",\"DF_DATE\", \"WO_STATUS\",";
                ////strQry += " case when \"DAYS_FROM_PENDING\" is null then (\"WO_STATUS\"|| ' SINCE ' || CURRENT_DATE - (SELECT to_date(to_char(\"EST_WO_CR_ON\",'yyyy-mm-dd'),'yyyy-mm-dd') ";
                ////strQry += " FROM \"WORKFLOWSTATUSDUMMY\" WHERE \"WO_DATA_ID\"=\"DT_CODE\") ||' Days') else \"DAYS_FROM_PENDING\" END \"DAYS_FROM_PENDING\" ";
                ////strQry += " FROM (SELECT \"DF_EQUIPMENT_ID\",\"DT_CODE\",\"DT_NAME\",\"SUBDIVSION\",\"OMSECTION\",B.\"DF_DATE\", \"WO_STATUS\",\"WO_NO\" ";
                ////strQry += " ,\"WO_STATUS\" || ' SINCE ' || CURRENT_DATE - (SELECT to_date(to_char(\"WO_WO_CR_ON\",'yyyy-mm-dd'),'yyyy-mm-dd') ";
                ////strQry += " FROM \"WORKFLOWSTATUSDUMMY\" WHERE \"WO_DATA_ID\"=\"DT_CODE\") ||' Days' AS \"DAYS_FROM_PENDING\" FROM \"WORKFLOWSTATUSDUMMY\" ";
                ////strQry += " ,\"VIEWPENDINGFAILURE\" B,\"TBLDTCFAILURE\" C,\"TBLESTIMATIONDETAILS\"  WHERE \"WORKORDER\" IS NULL AND C.\"DF_ID\"=\"EST_FAILUREID\" ";
                ////strQry += " AND \"EST_FAIL_TYPE\"='2' AND  LENGTH(\"FAILURE\") >0 AND \"OFFCODE\"=\"OM_CODE\" AND \"WO_BO_ID\"<>10 AND ";
                ////strQry += " CAST(\"OFFCODE\"  AS TEXT) LIKE '" + sOfficeCode + "%' AND \"DT_CODE\"=\"WO_DATA_ID\"AND  \"WO_DF_ID\" = C.\"DF_ID\")A";
                //strQry = "SELECT \"DT_CODE\",\"DT_NAME\",\"DF_EQUIPMENT_ID\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" ";
                //strQry += "  where \"OFF_CODE\"=\"TRANS_REF_OFF_CODE\") as \"OMSECTION\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" ";
                //strQry += " where cast(\"OFF_CODE\" as text)=SUBSTRING(cast(\"TRANS_REF_OFF_CODE\" as text),1,4)) as \"SUBDIVSION\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" where cast(\"OFF_CODE\" as text)=SUBSTRING(cast(\"TRANS_REF_OFF_CODE\" as text),1,3)) as \"DIVSION\",\"DF_ID\",TO_CHAR(\"DF_DATE\",'dd-MON-yyyy')\"DF_DATE\",'PENDING WITH '";
                //strQry += "  ||\"RO_NAME\" ||' SINCE '||date_part('day',age(CURRENT_DATE,CAST(\"TRANS_UPDATE_DATE\" AS DATE)))||' Days' as \"DAYS_FROM_PENDING\" from \"TBLPENDINGTRANSACTION\" inner join \"TBLDTCMAST\" ";
                //strQry += "  on \"DT_CODE\"=\"TRANS_DTC_CODE\" left join  \"TBLDTCFAILURE\" on \"DF_DTC_CODE\"=\"TRANS_DTC_CODE\" and  \"DF_REPLACE_FLAG\"=0 left JOIN \"TBLESTIMATIONDETAILS\" on \"DF_ID\"=\"EST_FAILUREID\" and \"EST_FAIL_TYPE\"='2'";
                //strQry += "  INNER JOIN \"TBLROLES\" ON \"RO_ID\"=\"TRANS_NEXT_ROLE_ID\" where \"TRANS_BO_ID\" in(11,74) and CAST(\"TRANS_REF_OFF_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%' order by CAST(\"DF_DATE\" AS TIMESTAMP) desc";
                //dt = ObjCon.FetchDataTable(strQry);
                //return dt;

                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_work_order_pend_details");
                cmd.Parameters.AddWithValue("ofc_code", sOfficeCode);
                dt = ObjCon.FetchDataTable(cmd);

                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable LoadSingleWorkorderPendingDetails(string sOfficeCode)
        {

            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;

                //strQry = "SELECT \"DF_EQUIPMENT_ID\",\"DT_CODE\",\"DT_NAME\",\"SUBDIVSION\",\"OMSECTION\",\"DF_DATE\", \"WO_STATUS\", case when ";
                //strQry += " \"DAYS_FROM_PENDING\" is null then (\"WO_STATUS\"|| ' SINCE ' || CURRENT_DATE - (SELECT to_date(to_char(\"EST_WO_CR_ON\",'yyyy-mm-dd'),'yyyy-mm-dd') ";
                //strQry += " FROM \"WORKFLOWSTATUSDUMMY\" WHERE \"WO_DATA_ID\"=\"DT_CODE\") ||' Days') else \"DAYS_FROM_PENDING\" END \"DAYS_FROM_PENDING\" ";
                //strQry += " FROM (SELECT \"DF_EQUIPMENT_ID\",\"DT_CODE\",\"DT_NAME\",\"SUBDIVSION\",\"OMSECTION\",A.\"DF_DATE\", \"WO_STATUS\",\"WO_NO\",";
                //strQry += " \"WO_STATUS\" || ' SINCE ' || CURRENT_DATE - (SELECT to_date(to_char(\"WO_WO_CR_ON\",'yyyy-mm-dd'),'yyyy-mm-dd')  FROM ";
                //strQry += " \"WORKFLOWSTATUSDUMMY\" WHERE \"WO_DATA_ID\"=\"DT_CODE\") ||' Days' AS \"DAYS_FROM_PENDING\" FROM  \"VIEW_MINORFAILURE_PENDING\"A,";
                //strQry += " \"TBLDTCFAILURE\" B,\"TBLESTIMATIONDETAILS\" WHERE A.\"DF_ID\"=B.\"DF_ID\" AND  B.\"DF_ID\"=\"EST_FAILUREID\" AND \"EST_FAIL_TYPE\"='1'";
                //strQry += " AND \"WO_NO\" ='' AND LENGTH(CAST(A.\"DF_ID\" AS TEXT)) >=1 AND CAST(\"OM_CODE\"  AS TEXT) LIKE '" + sOfficeCode + "%')A";

                strQry = "SELECT \"DT_CODE\",\"DT_NAME\",\"DF_EQUIPMENT_ID\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\"";
                strQry += " where \"OFF_CODE\"=\"TRANS_REF_OFF_CODE\") as \"OMSECTION\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\"";
                strQry += "  where cast(\"OFF_CODE\" as text)=SUBSTRING(cast(\"TRANS_REF_OFF_CODE\" as text),1,4)) as \"SUBDIVSION\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" where cast(\"OFF_CODE\" as text)=SUBSTRING(cast(\"TRANS_REF_OFF_CODE\" as text),1,3)) as \"DIVSION\",\"DF_ID\",TO_CHAR(\"DF_DATE\",'dd-MON-yyyy')\"DF_DATE\",'PENDING WITH '||\"RO_NAME\" ||";
                strQry += " ' SINCE ' ||date_part('day',age(CURRENT_DATE,CAST(\"TRANS_UPDATE_DATE\" AS DATE)))||' Days' as \"DAYS_FROM_PENDING\" from \"TBLPENDINGTRANSACTION\" inner join \"TBLDTCMAST\" on \"DT_CODE\"=\"TRANS_DTC_CODE\"";
                strQry += "  inner join  \"TBLDTCFAILURE\" on \"DF_DTC_CODE\"=\"TRANS_DTC_CODE\" and  \"DF_REPLACE_FLAG\"=0 INNER JOIN \"TBLESTIMATIONDETAILS\" on \"DF_ID\"=\"EST_FAILUREID\" and \"EST_FAIL_TYPE\"='1' INNER JOIN \"TBLROLES\" ";
                strQry += " ON \"RO_ID\"=\"TRANS_NEXT_ROLE_ID\" where \"TRANS_BO_ID\"  in(11,74) and CAST(\"TRANS_REF_OFF_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%' order by CAST(\"DF_DATE\" AS TIMESTAMP) desc";

                dt = ObjCon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable LoadReceiveTCPendingDetails(string sOfficeCode)
        {

            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                //strQry = "SELECT \"DT_CODE\",\"DT_NAME\",\"SUBDIVSION\",\"OMSECTION\",\"WO_DATE\",\"RECEIVE_STATUS\",\"WO_NO\",case when ";
                //strQry += " \"DAYS_FROM_PENDING\" is null then (\"RECEIVE_STATUS\"|| ' SINCE ' || CURRENT_DATE - (SELECT to_date(to_char(\"WO_WO_CR_ON\",'yyyy-mm-dd'),'yyyy-mm-dd') ";
                //strQry += " FROM \"WORKFLOWSTATUSDUMMY\" WHERE \"WO_DATA_ID\"=\"DT_CODE\") ||' Days') else \"DAYS_FROM_PENDING\" END \"DAYS_FROM_PENDING\" ";
                //strQry += " FROM (SELECT \"DT_CODE\",\"DT_NAME\",\"SUBDIVSION\",\"OMSECTION\",\"WO_DATE\",\"RECEIVE_STATUS\",\"WO_NO\",";
                //strQry += " \"RECEIVE_STATUS\"|| ' SINCE ' || CURRENT_DATE - (SELECT to_date(to_char(\"RECEIVE_WO_CR_ON\",'yyyy-mm-dd'),'yyyy-mm-dd') ";
                //strQry += " FROM \"WORKFLOWSTATUSDUMMY\" WHERE \"WO_DATA_ID\"=\"DT_CODE\") ||' Days' AS \"DAYS_FROM_PENDING\" FROM ";
                //strQry += " \"VIEW_MINORFAILURE_PENDING\"A,\"TBLDTCFAILURE\" B,\"TBLESTIMATIONDETAILS\",\"WORKFLOWSTATUSDUMMY\" WHERE ";
                //strQry += " A.\"DF_ID\"=B.\"DF_ID\" AND  B.\"DF_ID\"=\"EST_FAILUREID\" AND \"DT_CODE\"=\"WO_DATA_ID\" AND \"EST_FAIL_TYPE\"='1' ";
                //strQry += " AND \"RD_ID\" IS NULL AND LENGTH(cast(\"WO_NO\" as text)) >=1 AND CAST(\"OM_CODE\"  AS TEXT) LIKE '" + sOfficeCode + "%')A";
                strQry = "SELECT \"DT_CODE\",\"DT_NAME\",\"DF_EQUIPMENT_ID\",\"WO_NO\",to_char(\"WO_DATE\",'dd-MON-yyyy')\"WO_DATE\",";
                strQry += " (select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" where \"OFF_CODE\"=\"TRANS_REF_OFF_CODE\") as \"OMSECTION\",";
                strQry += " (select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" where cast(\"OFF_CODE\" as text)=";
                strQry += " SUBSTRING(cast(\"TRANS_REF_OFF_CODE\" as text),1,4)) as \"SUBDIVSION\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" where cast(\"OFF_CODE\" as text)=SUBSTRING(cast(\"TRANS_REF_OFF_CODE\" as text),1,3)) as \"DIVSION\",\"DF_ID\",TO_CHAR(\"DF_DATE\",'dd-MON-yyyy')\"DF_DATE\",'PENDING WITH '||\"RO_NAME\" ||' SINCE '||";
                strQry += "  date_part('day',age(CURRENT_DATE,CAST(\"TRANS_UPDATE_DATE\" AS DATE)))||' Days' as \"DAYS_FROM_PENDING\" from \"TBLPENDINGTRANSACTION\" inner join \"TBLDTCMAST\" on ";
                strQry += " \"DT_CODE\"=\"TRANS_DTC_CODE\" inner join  \"TBLDTCFAILURE\" on \"DF_DTC_CODE\"=\"TRANS_DTC_CODE\" and  \"DF_REPLACE_FLAG\"=0 INNER JOIN \"TBLWORKORDER\" on \"DF_ID\"=\"WO_DF_ID\"";
                strQry += " INNER JOIN \"TBLROLES\" ON \"RO_ID\"=\"TRANS_NEXT_ROLE_ID\" where \"TRANS_BO_ID\" =46 and CAST(\"TRANS_REF_OFF_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%' order by CAST(\"DF_DATE\" AS TIMESTAMP) desc";
                dt = ObjCon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable LoadSingleComissionPendingDetails(string sOfficeCode)
        {

            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;

                //strQry = "SELECT \"DT_CODE\",\"DT_NAME\",\"SUBDIVSION\",\"OMSECTION\",\"WO_DATE\",\"COMMISSION_STATUS\",\"WO_NO\",case when ";
                //strQry += " \"DAYS_FROM_PENDING\" is null then (\"COMMISSION_STATUS\"|| ' SINCE ' || CURRENT_DATE - (SELECT to_date(to_char(\"RECEIVE_WO_CR_ON\",'yyyy-mm-dd'),'yyyy-mm-dd') ";
                //strQry += " FROM \"WORKFLOWSTATUSDUMMY\" WHERE \"WO_DATA_ID\"=\"DT_CODE\") ||' Days') else \"DAYS_FROM_PENDING\" END \"DAYS_FROM_PENDING\" ";
                //strQry += " FROM(SELECT \"DT_CODE\",\"DT_NAME\",\"SUBDIVSION\",\"OMSECTION\",\"WO_DATE\",\"COMMISSION_STATUS\",\"WO_NO\",";
                //strQry += " \"COMMISSION_STATUS\"|| ' SINCE ' || CURRENT_DATE - (SELECT to_date(to_char(\"COMISSION_WO_CR_ON\",'yyyy-mm-dd'),'yyyy-mm-dd') ";
                //strQry += " FROM \"WORKFLOWSTATUSDUMMY\" WHERE \"WO_DATA_ID\"=\"DT_CODE\") ||' Days' AS \"DAYS_FROM_PENDING\"  FROM ";
                //strQry += " \"VIEW_MINORFAILURE_PENDING\"A,\"TBLDTCFAILURE\" B,\"TBLESTIMATIONDETAILS\",\"WORKFLOWSTATUSDUMMY\" WHERE ";
                //strQry += " A.\"DF_ID\"=B.\"DF_ID\" AND  B.\"DF_ID\"=\"EST_FAILUREID\" AND \"DT_CODE\"=\"WO_DATA_ID\" AND \"EST_FAIL_TYPE\"='1' ";
                //strQry += " AND \"TMC_ID\" is null AND LENGTH(cast(\"RD_ID\" as text)) >=1 AND  CAST(\"OM_CODE\"  AS TEXT) LIKE '" + sOfficeCode + "%')A";
                strQry = "SELECT \"DT_CODE\",\"DT_NAME\",\"DF_EQUIPMENT_ID\",\"WO_NO\",to_char(\"WO_DATE\",'dd-MON-yyyy')\"WO_DATE\",";
                strQry += " (select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\"";
                strQry += " where \"OFF_CODE\"=\"TRANS_REF_OFF_CODE\") as \"OMSECTION\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) ";
                strQry += " from \"VIEW_ALL_OFFICES\" where cast(\"OFF_CODE\" as text)=SUBSTRING(cast(\"TRANS_REF_OFF_CODE\" as text),1,4)) as \"SUBDIVSION\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" where cast(\"OFF_CODE\" as text)=SUBSTRING(cast(\"TRANS_REF_OFF_CODE\" as text),1,3)) as \"DIVSION\",\"DF_ID\",TO_CHAR(\"DF_DATE\",'dd-MON-yyyy')";
                strQry += " \"DF_DATE\",'PENDING WITH '||\"RO_NAME\" ||' SINCE '||date_part('day',age(CURRENT_DATE,CAST(\"TRANS_UPDATE_DATE\" AS DATE)))||' Days' as \"DAYS_FROM_PENDING\" from \"TBLPENDINGTRANSACTION\"";
                strQry += "  inner join \"TBLDTCMAST\" on \"DT_CODE\"=\"TRANS_DTC_CODE\" inner join  \"TBLDTCFAILURE\" on \"DF_DTC_CODE\"=\"TRANS_DTC_CODE\" and  \"DF_REPLACE_FLAG\"=0 INNER JOIN \"TBLWORKORDER\" ON ";
                strQry += "  \"DF_ID\"=\"WO_DF_ID\" INNER JOIN \"TBLROLES\" ON \"RO_ID\"=\"TRANS_NEXT_ROLE_ID\" where \"TRANS_BO_ID\" =47 and CAST(\"TRANS_REF_OFF_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%' order by CAST(\"DF_DATE\" AS TIMESTAMP) desc";
                dt = ObjCon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable LoadIndentPendingDetails(string sOfficeCode)
        {

            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;

                //strQry = "SELECT \"DF_EQUIPMENT_ID\", \"DT_CODE\",\"DT_NAME\",\"SUBDIVSION\",\"OMSECTION\",\"DF_DATE\",\"INDT_STATUS\",";
                //strQry += " case when \"DAYS_FROM_PENDING\" is null then (\"INDT_STATUS\"|| ' SINCE ' || CURRENT_DATE - (SELECT to_date(to_char(\"WO_WO_CR_ON\",'yyyy-mm-dd'),'yyyy-mm-dd') ";
                //strQry += " FROM \"WORKFLOWSTATUSDUMMY\" WHERE \"WO_DATA_ID\"=\"DT_CODE\") ||' Days') else \"DAYS_FROM_PENDING\" END \"DAYS_FROM_PENDING\" ";
                //strQry += " FROM(SELECT  \"DF_EQUIPMENT_ID\", \"DT_CODE\",\"DT_NAME\",\"SUBDIVSION\",\"OMSECTION\",B.\"DF_DATE\",\"INDT_STATUS\",";
                //strQry += " \"INDT_STATUS\" || ' SINCE ' || CURRENT_DATE - (SELECT to_date(to_char(\"IND_WO_CR_ON\",'yyyy-mm-dd'),'yyyy-mm-dd')  FROM ";
                //strQry += " \"WORKFLOWSTATUSDUMMY\" WHERE \"WO_DATA_ID\"=\"DT_CODE\") ||' Days' AS \"DAYS_FROM_PENDING\" FROM \"WORKFLOWSTATUSDUMMY\", ";
                //strQry += " \"VIEWPENDINGFAILURE\" B, \"TBLDTCFAILURE\" C WHERE \"INDENT\"  IS NULL AND \"WORKORDER\" <> ''  AND \"OFFCODE\"=\"OM_CODE\" ";
                //strQry += " AND \"WO_BO_ID\"<>10 AND CAST(\"OFFCODE\" AS TEXT) LIKE '" + sOfficeCode + "%' AND \"DT_CODE\"=\"WO_DATA_ID\" AND \"WO_DF_ID\" = C.\"DF_ID\")A";
                strQry = "SELECT \"DT_CODE\",\"DT_NAME\",\"DF_EQUIPMENT_ID\",\"WO_NO\",to_char(\"WO_DATE\",'dd-MON-yyyy')\"WO_DATE\",";
                strQry += " (select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" where \"OFF_CODE\"=\"TRANS_REF_OFF_CODE\")";
                strQry += " as \"OMSECTION\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" where cast(\"OFF_CODE\" as text)=SUBSTRING(cast(\"TRANS_REF_OFF_CODE\" as text),1,4)) as \"SUBDIVSION\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" where cast(\"OFF_CODE\" as text)=SUBSTRING(cast(\"TRANS_REF_OFF_CODE\" as text),1,3)) as \"DIVSION\",";
                strQry += "  \"DF_ID\",TO_CHAR(\"DF_DATE\",'dd-MON-yyyy')\"DF_DATE\",'PENDING WITH '||\"RO_NAME\" ||' SINCE '||date_part('day',age(CURRENT_DATE,CAST(\"TRANS_UPDATE_DATE\" AS DATE)))||' Days' as \"DAYS_FROM_PENDING\" from \"TBLPENDINGTRANSACTION\" ";
                strQry += "  inner join \"TBLDTCMAST\" on \"DT_CODE\"=\"TRANS_DTC_CODE\" inner join  \"TBLDTCFAILURE\" on \"DF_DTC_CODE\"=\"TRANS_DTC_CODE\" and  \"DF_REPLACE_FLAG\"=0 left join \"TBLWORKORDER\" ON \"DF_ID\"=\"WO_DF_ID\" INNER JOIN \"TBLROLES\" ON \"RO_ID\"=\"TRANS_NEXT_ROLE_ID\"";
                strQry += "  where \"TRANS_BO_ID\" in (12,29) and CAST(\"TRANS_REF_OFF_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%' order by CAST(\"DF_DATE\" AS TIMESTAMP) desc";
                dt = ObjCon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable LoadInvoicePendingDetails(string sOfficeCode)
        {

            DataTable dt = new DataTable();
            try
            {
                //string strQry = string.Empty;

                ////strQry = "SELECT \"DF_EQUIPMENT_ID\", \"DT_CODE\",\"DT_NAME\",\"SUBDIVSION\",\"OMSECTION\",\"DF_DATE\",\"INV_STATUS\",case when ";
                ////strQry += " \"DAYS_FROM_PENDING\" is null then (\"INV_STATUS\"|| ' SINCE ' || CURRENT_DATE - (SELECT to_date(to_char(\"IND_WO_CR_ON\",'yyyy-mm-dd'),'yyyy-mm-dd') ";
                ////strQry += " FROM \"WORKFLOWSTATUSDUMMY\" WHERE \"WO_DATA_ID\"=\"DT_CODE\") ||' Days') else \"DAYS_FROM_PENDING\" END \"DAYS_FROM_PENDING\" ";
                ////strQry += " FROM (SELECT \"DF_EQUIPMENT_ID\", \"DT_CODE\",\"DT_NAME\",\"SUBDIVSION\",\"OMSECTION\",B.\"DF_DATE\",\"INV_STATUS\", ";
                ////strQry += " \"INV_STATUS\"|| ' SINCE ' || CURRENT_DATE - (SELECT to_date(to_char(\"IN_WO_CR_ON\",'yyyy-mm-dd'),'yyyy-mm-dd') ";
                ////strQry += " FROM \"WORKFLOWSTATUSDUMMY\" WHERE \"WO_DATA_ID\"=\"DT_CODE\") ||' Days' AS \"DAYS_FROM_PENDING\" FROM \"WORKFLOWSTATUSDUMMY\", ";
                ////strQry += " \"VIEWPENDINGFAILURE\" B, \"TBLDTCFAILURE\" C WHERE \"OFFCODE\"=\"OM_CODE\" AND \"INVOICE\" IS NULL AND \"INDENT\" <> '' AND ";
                ////strQry += " \"WO_BO_ID\" <>10 AND CAST(\"OFFCODE\" AS TEXT) LIKE '" + sOfficeCode + "%' AND \"DT_CODE\"=\"WO_DATA_ID\" AND \"WO_DF_ID\" = C.\"DF_ID\")A";
                //strQry = "SELECT \"DT_CODE\",\"DT_NAME\",\"DF_EQUIPMENT_ID\",\"WO_NO\",to_char(\"WO_DATE\",'dd-MON-yyyy')\"WO_DATE\",";
                //strQry += " (select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" where \"OFF_CODE\"=\"TRANS_REF_OFF_CODE\") as \"OMSECTION\",";
                //strQry += " (select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" where cast(\"OFF_CODE\" as text)=SUBSTRING(cast(\"TRANS_REF_OFF_CODE\" as text),1,4)) as \"SUBDIVSION\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" where cast(\"OFF_CODE\" as text)=SUBSTRING(cast(\"TRANS_REF_OFF_CODE\" as text),1,3)) as \"DIVSION\",";
                //strQry += "  \"DF_ID\",TO_CHAR(\"DF_DATE\",'dd-MON-yyyy')\"DF_DATE\",'PENDING WITH '||\"RO_NAME\" ||' SINCE '||date_part('day',age(CURRENT_DATE,CAST(\"TRANS_UPDATE_DATE\" AS DATE)))||' Days' as \"DAYS_FROM_PENDING\" ";
                //strQry += "  from \"TBLPENDINGTRANSACTION\" inner join \"TBLDTCMAST\" on \"DT_CODE\"=\"TRANS_DTC_CODE\" inner join  \"TBLDTCFAILURE\" on \"DF_DTC_CODE\"=\"TRANS_DTC_CODE\" and  \"DF_REPLACE_FLAG\"=0 INNER JOIN \"TBLWORKORDER\" ";
                //strQry += "  ON \"DF_ID\"=\"WO_DF_ID\" INNER JOIN \"TBLROLES\" ON \"RO_ID\"=\"TRANS_NEXT_ROLE_ID\" where \"TRANS_BO_ID\" ='13' and CAST(\"TRANS_REF_OFF_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%' and \"TRANS_NEXT_ROLE_ID\"<>0 order by CAST(\"DF_DATE\" AS TIMESTAMP) desc";
                //dt = ObjCon.FetchDataTable(strQry);
                //return dt;

                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_invoice_pend_details");
                cmd.Parameters.AddWithValue("ofc_code", sOfficeCode);
                dt = ObjCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable LoadDeCommissionPendingDetails(string sOfficeCode)
        {

            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;

                //strQry = "SELECT \"DF_EQUIPMENT_ID\", \"DT_CODE\",\"DT_NAME\",\"SUBDIVSION\",\"OMSECTION\",\"DECOMM_STATUS\",case when \"DAYS_FROM_PENDING\" ";
                //strQry += " is null then (\"DECOMM_STATUS\"|| ' SINCE ' || CURRENT_DATE - (SELECT to_date(to_char(\"IN_WO_CR_ON\",'yyyy-mm-dd'),'yyyy-mm-dd') ";
                //strQry += " FROM \"WORKFLOWSTATUSDUMMY\" WHERE \"WO_DATA_ID\"=\"DT_CODE\") ||' Days') else \"DAYS_FROM_PENDING\" END \"DAYS_FROM_PENDING\" ";
                //strQry += " FROM (SELECT  \"DF_EQUIPMENT_ID\", \"DT_CODE\",\"DT_NAME\",\"SUBDIVSION\",\"OMSECTION\",\"DECOMM_STATUS\",\"DECOMM_STATUS\"|| ";
                //strQry += " ' SINCE ' || CURRENT_DATE - (SELECT to_date(to_char(\"DECOM_WO_CR_ON\",'yyyy-mm-dd'),'yyyy-mm-dd')  FROM \"WORKFLOWSTATUSDUMMY\" ";
                //strQry += " WHERE \"WO_DATA_ID\"=\"DT_CODE\") ||' Days' AS \"DAYS_FROM_PENDING\" FROM \"WORKFLOWSTATUSDUMMY\",\"VIEWPENDINGFAILURE\" B,";
                //strQry += " \"TBLDTCFAILURE\" C WHERE \"OFFCODE\"=\"OM_CODE\" AND \"INVOICE\" <> '' AND \"DECOMMISION\" IS NULL AND \"WO_BO_ID\"<>10 ";
                //strQry += " AND CAST(\"OFFCODE\" AS TEXT) LIKE '" + sOfficeCode + "%' AND \"DT_CODE\"=\"WO_DATA_ID\" AND \"WO_DF_ID\" = C.\"DF_ID\")A";
                strQry = "SELECT \"DT_CODE\",\"DT_NAME\",\"DF_EQUIPMENT_ID\",\"WO_NO\",to_char(\"WO_DATE\",'dd-MON-yyyy')\"WO_DATE\",";
                strQry += " (select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" where \"OFF_CODE\"=\"TRANS_REF_OFF_CODE\")";
                strQry += "  as \"OMSECTION\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" where cast(\"OFF_CODE\" as text)";
                strQry += " = SUBSTRING(cast(\"TRANS_REF_OFF_CODE\" as text),1,4)) as \"SUBDIVSION\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" where cast(\"OFF_CODE\" as text)=SUBSTRING(cast(\"TRANS_REF_OFF_CODE\" as text),1,3)) as \"DIVSION\",\"DF_ID\",TO_CHAR(\"DF_DATE\",'dd-MON-yyyy')\"DF_DATE\",'PENDING WITH '||\"RO_NAME\" ||' SINCE '||";
                strQry += "  date_part('day',age(CURRENT_DATE,CAST(\"TRANS_UPDATE_DATE\" AS DATE)))||' Days' as \"DAYS_FROM_PENDING\" from \"TBLPENDINGTRANSACTION\" inner join \"TBLDTCMAST\" on \"DT_CODE\"=\"TRANS_DTC_CODE\"";
                strQry += " inner join  \"TBLDTCFAILURE\" on \"DF_DTC_CODE\"=\"TRANS_DTC_CODE\" and  \"DF_REPLACE_FLAG\"=0 INNER JOIN \"TBLWORKORDER\" ON \"DF_ID\"=\"WO_DF_ID\" INNER JOIN \"TBLROLES\" ON \"RO_ID\"=\"TRANS_NEXT_ROLE_ID\"";
                strQry += "  where \"TRANS_BO_ID\" ='14' and CAST(\"TRANS_REF_OFF_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%' order by CAST(\"DF_DATE\" AS TIMESTAMP) desc";
                dt = ObjCon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable LoadRIPendingDetails(string sOfficeCode)
        {

            DataTable dt = new DataTable();
            try
            {
                //string strQry = string.Empty;
                ////query changes done for invoced dtr issue in grid
                ////strQry = "SELECT \"DT_CODE\",\"DT_NAME\",\"DT_TC_ID\",\"DF_EQUIPMENT_ID\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) ";
                ////   strQry += " from \"VIEW_ALL_OFFICES\" where \"OFF_CODE\"=\"TRANS_REF_OFF_CODE\") as \"OMSECTION\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" ";
                ////   strQry += " where cast(\"OFF_CODE\" as text)=SUBSTRING(cast(\"TRANS_REF_OFF_CODE\" as text),1,4)) as \"SUBDIVSION\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" where cast(\"OFF_CODE\" as text)=SUBSTRING(cast(\"TRANS_REF_OFF_CODE\" as text),1,3)) as \"DIVSION\",CASE WHEN \"TRANS_BO_ID\"=15 THEN 'PENDING WITH '||\"RO_NAME\" ||";
                ////   strQry += " ' SINCE ' ||date_part('day',age(CURRENT_DATE,CAST(\"TRANS_UPDATE_DATE\" AS DATE)))||' Days'  END AS \"DAYS_FROM_PENDING\"";
                ////   strQry += "  from \"TBLPENDINGTRANSACTION\" inner join \"TBLDTCMAST\" on \"DT_CODE\"=\"TRANS_DTC_CODE\"";
                ////     strQry += "  inner join \"TBLDTCFAILURE\" on \"TRANS_DTC_CODE\"=\"DF_DTC_CODE\"  INNER JOIN \"TBLROLES\" ON \"TRANS_NEXT_ROLE_ID\"=\"RO_ID\" where \"TRANS_BO_ID\" in (15) and CAST(\"TRANS_REF_OFF_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%' order by CAST(\"DF_DATE\" AS TIMESTAMP) desc";

                //strQry = "SELECT \"DT_CODE\",\"DT_NAME\", \"TD_TC_NO\" AS \"DT_TC_ID\",\"DF_EQUIPMENT_ID\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) ";
                //strQry += " from \"VIEW_ALL_OFFICES\" where \"OFF_CODE\"=\"TRANS_REF_OFF_CODE\") as \"OMSECTION\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" ";
                //strQry += " where cast(\"OFF_CODE\" as text)=SUBSTRING(cast(\"TRANS_REF_OFF_CODE\" as text),1,4)) as \"SUBDIVSION\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" where cast(\"OFF_CODE\" as text)=SUBSTRING(cast(\"TRANS_REF_OFF_CODE\" as text),1,3)) as \"DIVSION\",CASE WHEN \"TRANS_BO_ID\"=15 THEN 'PENDING WITH '||\"RO_NAME\" ||";
                //strQry += " ' SINCE ' ||date_part('day',age(CURRENT_DATE,CAST(\"TRANS_UPDATE_DATE\" AS DATE)))||' Days'  END AS \"DAYS_FROM_PENDING\"";
                //strQry += "  from \"TBLPENDINGTRANSACTION\" inner join \"TBLDTCMAST\" on \"DT_CODE\"=\"TRANS_DTC_CODE\"";
                //strQry += "  inner join \"TBLDTCFAILURE\" on \"TRANS_DTC_CODE\"=\"DF_DTC_CODE\"  INNER JOIN \"TBLROLES\" ON \"TRANS_NEXT_ROLE_ID\"=\"RO_ID\"  LEFT JOIN \"TBLTCDRAWN\" ON \"DF_ID\"=\"TD_DF_ID\" where \"TRANS_BO_ID\" in (15) and CAST(\"TRANS_REF_OFF_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%' order by CAST(\"DF_DATE\" AS TIMESTAMP) desc";
                //dt = ObjCon.FetchDataTable(strQry);
                //return dt;

                NpgsqlCommand cmd = new NpgsqlCommand("proc_ri_pend_details");
                cmd.Parameters.AddWithValue("ofc_code", sOfficeCode);
                dt = ObjCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }


        public DataTable LoadCRPendingDetails(string sOfficeCode)
        {

            DataTable dt = new DataTable();
            try
            {
                //string strQry = string.Empty;
                ////query changes done for invoced dtr issue in grid
                ////strQry = "SELECT \"DT_CODE\",\"DT_NAME\",\"DT_TC_ID\",\"DF_EQUIPMENT_ID\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) ";
                ////strQry += " from \"VIEW_ALL_OFFICES\" where \"OFF_CODE\"=\"TRANS_REF_OFF_CODE\") as \"OMSECTION\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" ";
                ////strQry += " where cast(\"OFF_CODE\" as text)=SUBSTRING(cast(\"TRANS_REF_OFF_CODE\" as text),1,4)) as \"SUBDIVSION\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" where cast(\"OFF_CODE\" as text)=SUBSTRING(cast(\"TRANS_REF_OFF_CODE\" as text),1,3)) as \"DIVSION\",";
                ////strQry += " CASE WHEN \"TRANS_BO_ID\"=26 THEN 'PENDING WITH '||\"RO_NAME\" ||";
                ////strQry += "   ' SINCE ' ||date_part('day',age(CURRENT_DATE,CAST(\"TRANS_UPDATE_DATE\" AS DATE)))||' Days'  END AS \"CR_DAYS_FROM_PENDING\"  from \"TBLPENDINGTRANSACTION\" inner join \"TBLDTCMAST\" on \"DT_CODE\"=\"TRANS_DTC_CODE\"";
                ////strQry += "  inner join \"TBLDTCFAILURE\" on \"TRANS_DTC_CODE\"=\"DF_DTC_CODE\"  INNER JOIN \"TBLROLES\" ON \"TRANS_NEXT_ROLE_ID\"=\"RO_ID\" where \"TRANS_BO_ID\" in (26) and CAST(\"TRANS_REF_OFF_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%'and \"DF_REPLACE_FLAG\"<>1 order by CAST(\"DF_DATE\" AS TIMESTAMP) desc";


                //strQry = "SELECT \"DT_CODE\",\"DT_NAME\",\"TD_TC_NO\" AS \"DT_TC_ID\",\"DF_EQUIPMENT_ID\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) ";
                //strQry += " from \"VIEW_ALL_OFFICES\" where \"OFF_CODE\"=\"TRANS_REF_OFF_CODE\") as \"OMSECTION\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" ";
                //strQry += " where cast(\"OFF_CODE\" as text)=SUBSTRING(cast(\"TRANS_REF_OFF_CODE\" as text),1,4)) as \"SUBDIVSION\",(select substring(\"OFF_NAME\",position(':' in \"OFF_NAME\")+2,length(\"OFF_NAME\")) from \"VIEW_ALL_OFFICES\" where cast(\"OFF_CODE\" as text)=SUBSTRING(cast(\"TRANS_REF_OFF_CODE\" as text),1,3)) as \"DIVSION\",";
                //strQry += " CASE WHEN \"TRANS_BO_ID\"=26 THEN 'PENDING WITH '||\"RO_NAME\" ||";
                //strQry += "   ' SINCE ' ||date_part('day',age(CURRENT_DATE,CAST(\"TRANS_UPDATE_DATE\" AS DATE)))||' Days'  END AS \"CR_DAYS_FROM_PENDING\"  from \"TBLPENDINGTRANSACTION\" inner join \"TBLDTCMAST\" on \"DT_CODE\"=\"TRANS_DTC_CODE\"";
                //strQry += "  inner join \"TBLDTCFAILURE\" on \"TRANS_DTC_CODE\"=\"DF_DTC_CODE\"  INNER JOIN \"TBLROLES\" ON \"TRANS_NEXT_ROLE_ID\"=\"RO_ID\" LEFT JOIN \"TBLTCDRAWN\" ON \"DF_ID\"=\"TD_DF_ID\" where \"TRANS_BO_ID\" in (26) and CAST(\"TRANS_REF_OFF_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%'and \"DF_REPLACE_FLAG\"<>1 order by CAST(\"DF_DATE\" AS TIMESTAMP) desc";
                //dt = ObjCon.FetchDataTable(strQry);
                //return dt;

                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_cr_pend_details");
                cmd.Parameters.AddWithValue("ofc_code", sOfficeCode);
                dt = ObjCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        public DataTable LoadFailureDtrDetails(string sOfficeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;

                if (sOfficeCode == "" || sOfficeCode == null)
                {
                    strQry = " SELECT CAST(\"TC_CODE\") AS \"TC_CODE\",\"TC_SLNO\",CAST(\"TC_CAPACITY\" AS TEXT) \"TC_CAPACITY\",(SELECT SUBSTR(\"OFF_NAME\",INSTR(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE \"OFF_CODE\"=\"TC_LOCATION_ID\") AS \"OFFNAME\"  FROM \"TBLTCMASTER\" WHERE \"TC_CURRENT_LOCATION\" ='2' AND \"TC_STATUS\" ='3'  AND \"TC_LOCATION_ID\" LIKE '" + sOfficeCode + "%' ";
                    strQry += " UNION SELECT CAST(\"TC_CODE\") AS \"TC_CODE\",\"TC_SLNO\",CAST(\"TC_CAPACITY\" AS TEXT) \"TC_CAPACITY\",(SELECT SUBSTR(\"OFF_NAME\",INSTR(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE \"OFF_CODE\"=\"TC_LOCATION_ID\") \"OFFNAME\"  FROM \"TBLTCMASTER\" WHERE \"TC_CURRENT_LOCATION\" ='1' AND \"TC_STATUS\" ='3' ";
                    strQry += " UNION SELECT CAST(\"TC_CODE\" AS TEXT),\"TC_SLNO\",\"TC_CAPACITY\" ,(SELECT SUBSTR(\"OFF_NAME\",INSTR(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE \"OFF_CODE\"=\"TC_LOCATION_ID\") \"OFFNAME\"   FROM \"TBLTCMASTER\" WHERE \"TC_CURRENT_LOCATION\" ='3' AND \"TC_STATUS\" ='3' ORDER BY \"TC_CAPACITY\"";
                }
                else
                {
                    strQry = " SELECT CAST(\"TC_CODE\") AS \"TC_CODE\",\"TC_SLNO\",CAST(\"TC_CAPACITY\" AS TEXT) \"TC_CAPACITY\",(SELECT SUBSTR(\"OFF_NAME\",INSTR(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE \"OFF_CODE\"=\"TC_LOCATION_ID\") AS \"OFFNAME\"  FROM \"TBLTCMASTER\" WHERE \"TC_CURRENT_LOCATION\" ='2' AND \"TC_STATUS\" ='3'  AND \"TC_LOCATION_ID\" LIKE '" + sOfficeCode + "%' ";
                    strQry += " UNION SELECT CAST(\"TC_CODE\") AS \"TC_CODE\",\"TC_SLNO\",CAST(\"TC_CAPACITY\" AS TEXT) \"TC_CAPACITY\",(SELECT SUBSTR(\"OFF_NAME\",INSTR(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE \"OFF_CODE\"=\"TC_LOCATION_ID\") \"OFFNAME\"  FROM \"TBLTCMASTER\" WHERE \"TC_CURRENT_LOCATION\" ='1' AND \"TC_STATUS\" ='3'  AND \"TC_LOCATION_ID\" = '" + clsStoreOffice.GetStoreID(sOfficeCode) + " ";
                    strQry += " UNION SELECT CAST(\"TC_CODE\" AS TEXT),\"TC_SLNO\",\"TC_CAPACITY\" ,(SELECT SUBSTR(\"OFF_NAME\",INSTR(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE \"OFF_CODE\"=\"TC_LOCATION_ID\") \"OFFNAME\"   FROM \"TBLTCMASTER\" WHERE \"TC_CURRENT_LOCATION\" ='3' AND \"TC_STATUS\" ='3'  AND \"TC_LOCATION_ID\" = '" + clsStoreOffice.GetStoreID(sOfficeCode) + "' ORDER BY \"TC_CAPACITY\"";
                }

                dt = ObjCon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        public DataTable LoadConditionOfTCDetails(string sOfficeCode)
        {

            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;


                strQry = " SELECT \"TC_CODE\",\"TC_SLNO\",\"TC_CAPACITY\",\"TM_NAME\",(\"MD_NAME\")\"TC_RATING\" FROM \"TBLTCMASTER\",\"TBLMASTERDATA\",\"TBLTRANSMAKES\" WHERE \"TC_RATING\"=\"MD_ID\" AND \"TM_ID\"=\"TC_MAKE_ID\" AND \"TC_STATUS\" in(1,2,3) AND \"TC_CURRENT_LOCATION\" = 1  AND \"TC_LOCATION_ID\"=" + sOfficeCode;
                dt = ObjCon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        public DataTable LoadConditionOfNewTCDetails(string sOfficeCode)
        {

            DataTable dt = new DataTable();
            try
            {
                #region Old Inline query
                //string strQry = string.Empty;
                //strQry = " SELECT distinct \"TC_CODE\",\"TC_SLNO\",\"TC_CAPACITY\",\"TM_NAME\",(select \"MD_NAME\" from \"TBLMASTERDATA\" ";
                //strQry += " where \"MD_TYPE\"='SR' AND \"MD_ID\"=\"TC_RATING\")\"TC_RATING\" from  \"TBLTCMASTER\",\"TBLTRANSMAKES\" ";
                //strQry += " WHERE  \"TC_STATUS\"='1' AND \"TM_ID\"=\"TC_MAKE_ID\" AND \"TC_CURRENT_LOCATION\" = 1 AND \"TC_LOCATION_ID\"= " + sOfficeCode;
                //dt = ObjCon.FetchDataTable(strQry);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_condition_of_newtc_details");
                cmd.Parameters.AddWithValue("p_officecode", Convert.ToString(sOfficeCode ?? ""));
                dt = ObjCon.FetchDataTable(cmd);




                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        public DataTable LoadConditionOfREGoodTCDetails(string sOfficeCode)
        {

            DataTable dt = new DataTable();
            try
            {
                #region Old inline query
                //string strQry = string.Empty;
                ////dummy query
                //strQry = " SELECT \"TC_CODE\",\"TC_SLNO\",\"TC_CAPACITY\",\"TM_NAME\",(\"MD_NAME\")\"TC_RATING\" ";
                //strQry += " FROM \"TBLTCMASTER\",\"TBLMASTERDATA\",\"TBLTRANSMAKES\" WHERE \"TC_RATING\"=\"MD_ID\" ";
                //strQry += " AND \"TM_ID\"=\"TC_MAKE_ID\"  AND \"MD_TYPE\"='SR' AND \"TC_STATUS\"='11' AND \"TC_CURRENT_LOCATION\" = 1  AND \"TC_LOCATION_ID\"= " + sOfficeCode;
                //dt = ObjCon.FetchDataTable(strQry);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_condition_of_regoodtc_details");
                cmd.Parameters.AddWithValue("p_officecode", Convert.ToString(sOfficeCode ?? ""));
                dt = ObjCon.FetchDataTable(cmd);

                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        public DataTable LoadConditionOfRPGoodTCDetails(string sOfficeCode)
        {

            DataTable dt = new DataTable();
            try
            {
                #region Old inline query
                //string strQry = string.Empty;
                ////dummy query
                //strQry = " SELECT \"TC_CODE\",\"TC_SLNO\",\"TC_CAPACITY\",\"TM_NAME\",(\"MD_NAME\")\"TC_RATING\" ";
                //strQry += " FROM \"TBLTCMASTER\",\"TBLMASTERDATA\",\"TBLTRANSMAKES\" WHERE \"TC_RATING\"=\"MD_ID\" ";
                //strQry += " AND \"TM_ID\"=\"TC_MAKE_ID\" AND \"MD_TYPE\"='SR' AND \"TC_STATUS\"='2' AND \"TC_CURRENT_LOCATION\" = 1  AND \"TC_LOCATION_ID\"=" + sOfficeCode;
                //dt = ObjCon.FetchDataTable(strQry);
                #endregion


                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_condition_of_rpgoodtc_details");
                cmd.Parameters.AddWithValue("p_officecode", Convert.ToString(sOfficeCode ?? ""));
                dt = ObjCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        public DataTable LoadConditionOfFaultyTCDetails(string sOfficeCode)
        {

            DataTable dt = new DataTable();
            try
            {
                #region Old Inline query
                //string strQry = string.Empty;
                //strQry = " SELECT \"TC_CODE\",\"TC_SLNO\",\"TC_CAPACITY\",\"TM_NAME\",(\"MD_NAME\")\"TC_RATING\" ";
                //strQry += " FROM \"TBLTCMASTER\",\"TBLMASTERDATA\",\"TBLTRANSMAKES\" WHERE \"TC_RATING\"=\"MD_ID\" ";
                //strQry += " AND \"TM_ID\"=\"TC_MAKE_ID\" AND \"MD_TYPE\"='SR' AND \"TC_STATUS\"='3' AND \"TC_CURRENT_LOCATION\" = 1  AND \"TC_LOCATION_ID\"=" + sOfficeCode;
                //dt = ObjCon.FetchDataTable(strQry);
                #endregion 

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_condition_of_faultytc_details");
                cmd.Parameters.AddWithValue("p_officecode", Convert.ToString(sOfficeCode ?? ""));
                dt = ObjCon.FetchDataTable(cmd);

                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        public DataTable LoadConditionOfMobileTCDetails(string sOfficeCode)
        {

            DataTable dt = new DataTable();
            try
            {
                #region Old Inline query
                //string strQry = string.Empty;
                ////dummy query
                ////strQry = " SELECT \"TC_CODE\",\"TC_SLNO\",\"TC_CAPACITY\",\"TM_NAME\",(\"MD_NAME\")\"TC_RATING\" FROM \"TBLTCMASTER\",\"TBLMASTERDATA\",\"TBLTRANSMAKES\" WHERE \"TC_RATING\"=\"MD_ID\" AND \"TM_ID\"=\"TC_MAKE_ID\" AND \"TC_CONDITION\"='MOBILE TRANSFORMER' AND \"MD_TYPE\"='SR' AND \"TC_STATUS\"='4' AND \"TC_CURRENT_LOCATION\" = 1  AND \"TC_LOCATION_ID\"=" + sOfficeCode;
                //strQry = " SELECT \"TC_CODE\",\"TC_SLNO\",\"TC_CAPACITY\",\"TM_NAME\",(\"MD_NAME\")\"TC_RATING\" ";
                //strQry += " FROM \"TBLTCMASTER\",\"TBLMASTERDATA\",\"TBLTRANSMAKES\" WHERE \"TC_RATING\" = \"MD_ID\" ";
                //strQry += " AND \"TM_ID\" = \"TC_MAKE_ID\"  AND \"MD_TYPE\" = 'SR' AND \"TC_STATUS\" = '4' AND \"TC_CURRENT_LOCATION\" = 1  AND \"TC_LOCATION_ID\"=" + sOfficeCode;
                //dt = ObjCon.FetchDataTable(strQry);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_condition_of_mobiletc_details");
                cmd.Parameters.AddWithValue("p_officecode", Convert.ToString(sOfficeCode ?? ""));
                dt = ObjCon.FetchDataTable(cmd);

                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public string TotalRepairGoodTc(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;

                if (objDashoard.sOfficeCode == "" || objDashoard.sOfficeCode == null)
                {
                    strQry = "SELECT COUNT(\"TC_ID\") from \"TBLTCMASTER\" WHERE \"TC_STATUS\"=2 AND \"TC_CURRENT_LOCATION\" =1 ";
                }
                else
                {
                    strQry = "SELECT COUNT(\"TC_ID\") from \"TBLTCMASTER\" WHERE \"TC_STATUS\"=2 AND \"TC_CURRENT_LOCATION\" =1 AND CAST(\"TC_LOCATION_ID\" AS TEXT) = '" + clsStoreOffice.GetStoreID(objDashoard.sOfficeCode) + "'";
                }


                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        public DataTable TotalTcfailedview(string sOfficeCode)
        {
            DataTable dt = new DataTable();

            try
            {
                string strQry = string.Empty;

                //if (sOfficeCode == "" || sOfficeCode == null)
                //{
                //    NpgsqlCommand cmd = new NpgsqlCommand("sp_totaltcfailedview");
                //    dt = ObjCon.FetchDataTable(cmd);
                //    //strQry = "select \"TC_CODE\",CAST(\"TC_CAPACITY\" AS TEXT) \"TC_CAPACITY\", \"TC_SLNO\",TO_CHAR(\"TC_MANF_DATE\",'DD-MON-YYYY') \"TC_MANF_DATE\" FROM ";
                //    //strQry += " \"TBLTCMASTER\" WHERE \"TC_STATUS\"=2 AND \"TC_CURRENT_LOCATION\"=1 ";
                //}
                //else
                //{
                //    NpgsqlCommand cmd = new NpgsqlCommand("sp_totaltcfailedviewbasedonoffcode");
                //    cmd.Parameters.AddWithValue("offcode", clsStoreOffice.GetStoreID(sOfficeCode));
                //    dt = ObjCon.FetchDataTable(cmd);
                //    //  strQry = "select \"TC_CODE\",CAST(\"TC_CAPACITY\" AS TEXT) \"TC_CAPACITY\", \"TC_SLNO\",TO_CHAR(\"TC_MANF_DATE\",'DD-MON-YYYY') \"TC_MANF_DATE\" FROM ";
                //    // strQry += " \"TBLTCMASTER\" WHERE \"TC_STATUS\"=2 AND \"TC_CURRENT_LOCATION\"=1 AND \"TC_LOCATION_ID\" = '" + clsStoreOffice.GetStoreID(sOfficeCode) + "'";
                //}
                NpgsqlCommand cmd = new NpgsqlCommand("sp_totaltcfailedview");
                cmd.Parameters.AddWithValue("offcode", Convert.ToString(clsStoreOffice.GetStoreID(sOfficeCode)) == null ? "" : Convert.ToString(clsStoreOffice.GetStoreID(sOfficeCode)));
                dt = ObjCon.FetchDataTable(cmd);

                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }


        public DataTable GetTotalFaultyTCview(string sOfficeCode)
        {
            DataTable dt = new DataTable();

            try
            {
                string strQry = string.Empty;
                string storeid = clsStoreOffice.GetStoreID(sOfficeCode);
                //  if (storeid != null)
                // {
                //     strQry = "SELECT \"TC_CODE\",\"TC_CAPACITY\",\"TC_SLNO\",TO_CHAR(\"TC_MANF_DATE\",'DD-MON-YYYY') \"TC_MANF_DATE\" FROM \"TBLTCMASTER\" WHERE   \"TC_STATUS\"= '3'  AND (CAST(\"TC_LOCATION_ID\" AS TEXT) LIKE '" + sOfficeCode + "%' or  CAST(\"TC_LOCATION_ID\" AS TEXT)='" + clsStoreOffice.GetStoreID(sOfficeCode) + "'  ) and \"TC_CODE\"<>'0' and \"TC_CURRENT_LOCATION\" in(1,2,3)  ";
                // }
                // else
                //  {
                //     strQry = "SELECT \"TC_CODE\",\"TC_CAPACITY\",\"TC_SLNO\",TO_CHAR(\"TC_MANF_DATE\",'DD-MON-YYYY') \"TC_MANF_DATE\" FROM \"TBLTCMASTER\" WHERE   \"TC_STATUS\"= '3'  AND CAST(\"TC_LOCATION_ID\" AS TEXT) LIKE '" + sOfficeCode + "%' ";

                // }

                //dt = ObjCon.FetchDataTable(strQry);
                // string storeid = clsStoreOffice.GetStoreID(sOfficeCode);
                NpgsqlCommand cmd = new NpgsqlCommand("sp_gettotalfaultytcview");
                cmd.Parameters.AddWithValue("offcode", Convert.ToString(sOfficeCode == null ? "" : sOfficeCode));
                cmd.Parameters.AddWithValue("storeoffcode", Convert.ToString(storeid) == null ? "" : Convert.ToString(storeid));
                dt = ObjCon.FetchDataTable(cmd);

                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }



        public DataTable GetFaultyTCFieldview(string sOfficeCode)
        {
            DataTable dt = new DataTable();

            try
            {

                string strQry = string.Empty;
                //strQry = "SELECT \"TC_CODE\",CAST(\"TC_CAPACITY\") AS \"TC_CAPACITY\", \"TC_SLNO\",TO_CHAR(\"TC_MANF_DATE\",'DD-MON-YYYY') AS \"TC_MANF_DATE\" FROM \"TBLTCMASTER\" WHERE ";
                //strQry += " \"TC_CODE\" IN (SELECT \"DF_EQUIPMENT_ID\" FROM \"TBLDTCFAILURE\" WHERE \"DF_DTC_CODE\" IN(SELECT \"WO_DATA_ID\" FROM \"WORKFLOWSTATUSDUMMY\" WHERE ";
                //strQry += "(LENGTH(\"INVOICE\") > 0) AND (LENGTH(\"CRREPORT\") = 0 AND LENGTH(\"DECOMMISION\" ) = 0 AND \"WO_BO_ID\"<>10) AND CAST(\"OFFCODE\" AS TEXT) LIKE '" + sOfficeCode + "%'))";

                //strQry = "SELECT \"TC_CODE\",CAST(\"TC_CAPACITY\" as text) AS \"TC_CAPACITY\", \"TC_SLNO\",TO_CHAR(\"TC_MANF_DATE\",'DD-MON-YYYY') AS \"TC_MANF_DATE\" FROM \"TBLTCMASTER\" WHERE ";
                //strQry += " \"TC_CODE\" IN (SELECT \"DF_EQUIPMENT_ID\" FROM \"TBLDTCFAILURE\" WHERE \"DF_DTC_CODE\" IN(SELECT \"WO_DATA_ID\" FROM \"WORKFLOWSTATUSDUMMY\" WHERE ";
                //strQry += "(\"CRREPORT\" IS NULL)  AND \"WO_BO_ID\"<>10) AND CAST(\"DF_LOC_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%')";


                NpgsqlCommand Cmd = new NpgsqlCommand("sp_getfaultytcfieldview");
                Cmd.Parameters.AddWithValue("offcode", sOfficeCode);
                dt = ObjCon.FetchDataTable(Cmd);
                //  strQry = "SELECT \"TC_CODE\",CAST(\"TC_CAPACITY\" as text) AS \"TC_CAPACITY\", \"TC_SLNO\", ";
                //  strQry += " TO_CHAR(\"TC_MANF_DATE\",'DD-MON-YYYY') AS \"TC_MANF_DATE\" FROM \"TBLTCMASTER\" ";
                //  strQry += " WHERE  \"TC_CURRENT_LOCATION\"='2'  and   \"TC_STATUS\"='3' AND cast(\"TC_LOCATION_ID\" as text) ";
                //  strQry += " LIKE '" + sOfficeCode + "%'";

                // dt = ObjCon.FetchDataTable(strQry);

                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }


        public DataTable GetFaultyTCStoreview(string sOfficeCode)
        {
            DataTable dt = new DataTable();

            try
            {
                string strQry = string.Empty;
                //   if (sOfficeCode == "" || sOfficeCode == null)
                //  {
                //strQry = "SELECT \"TC_CODE\", \"TC_CAPACITY\",\"TC_SLNO\",TO_CHAR(\"TC_MANF_DATE\",'DD-MON-YYYY') AS \"TC_MANF_DATE\" FROM \"TBLTCMASTER\" WHERE \"TC_CURRENT_LOCATION\" ='1' AND \"TC_STATUS\"='3' AND \"TC_CODE\"<>'0' ";
                //  NpgsqlCommand cmd = new NpgsqlCommand("sp_getfaultytcstoreview");
                //  dt = ObjCon.FetchDataTable(cmd);
                //  }
                //  else
                //  {
                //  NpgsqlCommand cmd = new NpgsqlCommand("sp_getfaultytcstoreviewbasedonoffcode");
                //  cmd.Parameters.AddWithValue("offcode", clsStoreOffice.GetStoreID(sOfficeCode));
                //  dt = ObjCon.FetchDataTable(cmd);
                // strQry = "SELECT \"TC_CODE\", \"TC_CAPACITY\",\"TC_SLNO\",TO_CHAR(\"TC_MANF_DATE\",'DD-MON-YYYY') AS \"TC_MANF_DATE\" FROM \"TBLTCMASTER\" WHERE \"TC_CURRENT_LOCATION\" ='1' AND \"TC_STATUS\"='3' AND CAST(\"TC_LOCATION_ID\" AS TEXT) = '" + clsStoreOffice.GetStoreID(sOfficeCode) + "'";
                // }
                //  dt = ObjCon.FetchDataTable(strQry);
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getfaultytcstoreview");
                cmd.Parameters.AddWithValue("offcode", Convert.ToString(clsStoreOffice.GetStoreID(sOfficeCode)) == null ? "" : Convert.ToString(clsStoreOffice.GetStoreID(sOfficeCode)));
                dt = ObjCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable GetFaultyTCRepairview(string sOfficeCode)
        {
            DataTable dt = new DataTable();

            try
            {
                string strQry = string.Empty;
                // if (sOfficeCode == "" || sOfficeCode == null)
                //{
                // strQry = "SELECT \"TC_CODE\", \"TC_CAPACITY\",\"TC_SLNO\",TO_CHAR(\"TC_MANF_DATE\",'DD-MON-YYYY') \"TC_MANF_DATE\" FROM \"TBLTCMASTER\" WHERE \"TC_CURRENT_LOCATION\"='3' AND \"TC_STATUS\"='3' ";
                //  NpgsqlCommand cmd = new NpgsqlCommand("sp_getfaultytcrepairview");
                //  dt = ObjCon.FetchDataTable(cmd);
                //}
                //  else
                // {
                //  NpgsqlCommand cmd = new NpgsqlCommand("sp_getfaultytcrepairviewbasedonoffcode");
                //  cmd.Parameters.AddWithValue("offcode", clsStoreOffice.GetStoreID(sOfficeCode));
                //  dt = ObjCon.FetchDataTable(cmd);
                // strQry = "SELECT \"TC_CODE\", \"TC_CAPACITY\",\"TC_SLNO\",TO_CHAR(\"TC_MANF_DATE\",'DD-MON-YYYY') \"TC_MANF_DATE\" FROM \"TBLTCMASTER\" WHERE \"TC_CURRENT_LOCATION\"='3' AND \"TC_STATUS\"='3' AND CAST(\"TC_LOCATION_ID\" AS TEXT) = '" + clsStoreOffice.GetStoreID(sOfficeCode) + "'";
                //}

                // dt = ObjCon.FetchDataTable(strQry);
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getfaultytcrepairview");
                cmd.Parameters.AddWithValue("offcode", Convert.ToString(clsStoreOffice.GetStoreID(sOfficeCode)) == null ? "" : Convert.ToString(clsStoreOffice.GetStoreID(sOfficeCode)));
                dt = ObjCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }


        public DataTable GetStore_TcDetails(string sOfficeCode, string sWOslno, string sRoleType)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                if (sRoleType != "2")
                {
                    if (sOfficeCode.Length > 2)
                    {
                        sOfficeCode = sOfficeCode.Substring(0, Constants.Division);
                        sOfficeCode = clsStoreOffice.GetStoreID(sOfficeCode);
                    }
                }

                strQry = "SELECT \"TC_CODE\",\"TC_SLNO\",\"TC_CAPACITY\",CASE WHEN \"TC_STATUS\"=1 THEN 'BRAND NEW' WHEN \"TC_STATUS\"=2 THEN 'REPAIR GOOD' END ";
                strQry += " \"STATUS\", \"SM_NAME\" FROM \"TBLTCMASTER\",\"TBLSTOREMAST\" WHERE \"TC_LOCATION_ID\"=\"SM_ID\" AND \"TC_LOCATION_ID\"='" + sOfficeCode + "' ";
                strQry += " AND  \"TC_CAPACITY\" IN (SELECT \"WO_NEW_CAP\" FROM \"TBLWORKORDER\" WHERE \"WO_SLNO\"='" + sWOslno + "') AND \"TC_STATUS\" IN (1,2) AND \"TC_CURRENT_LOCATION\"=1";
                dt = ObjCon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable Loadless25CapacityTCDetails(string sOfficeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                #region old inline query
                //string strQry = string.Empty;
                //strQry = " SELECT\"TC_CODE\",\"TC_SLNO\",\"TC_CAPACITY\",\"TM_NAME\",(\"MD_NAME\")\"TC_RATING\" ";
                //strQry += " FROM \"TBLTCMASTER\",\"TBLMASTERDATA\",\"TBLTRANSMAKES\" WHERE \"TC_RATING\"=\"MD_ID\" ";
                //strQry += " AND \"TC_MAKE_ID\"=\"TM_ID\"  AND \"MD_TYPE\"='SR' and \"TC_CURRENT_LOCATION\" =1 AND \"TC_CAPACITY\" < 25 AND \"TC_LOCATION_ID\" = '" + sOfficeCode + "'";
                //dt = ObjCon.FetchDataTable(strQry);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_less_25capacitytc_details");
                cmd.Parameters.AddWithValue("p_officecode", Convert.ToString(sOfficeCode ?? ""));
                dt = ObjCon.FetchDataTable(cmd);

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
            return dt;
        }
        public DataTable Loadbtw25_100CapacityTCDetails(string sOfficeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                #region old inline query
                //string strQry = string.Empty;
                //strQry = " SELECT\"TC_CODE\",\"TC_SLNO\",\"TC_CAPACITY\",\"TM_NAME\",(\"MD_NAME\")\"TC_RATING\" ";
                //strQry += " FROM \"TBLTCMASTER\",\"TBLMASTERDATA\",\"TBLTRANSMAKES\" WHERE \"TC_RATING\"=\"MD_ID\" ";
                //strQry += " AND \"TC_MAKE_ID\"=\"TM_ID\"  AND \"MD_TYPE\"='SR' and \"TC_CURRENT_LOCATION\" =1 ";
                //strQry += " AND \"TC_CAPACITY\" BETWEEN 25 AND 100 AND \"TC_LOCATION_ID\" = '" + sOfficeCode + "' ";
                //dt = ObjCon.FetchDataTable(strQry);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_btw25_100_capacitytc_details");
                cmd.Parameters.AddWithValue("p_officecode", Convert.ToString(sOfficeCode ?? ""));
                dt = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return dt;
        }
        public DataTable Loadbtw125_250CapacityTCDetails(string sOfficeCode)
        {
            DataTable Dt = new DataTable();
            try
            {
                #region Old inline query 
                //string strQry = string.Empty;
                //strQry = " SELECT\"TC_CODE\",\"TC_SLNO\",\"TC_CAPACITY\",\"TM_NAME\",(\"MD_NAME\")\"TC_RATING\" ";
                //strQry += " FROM \"TBLTCMASTER\",\"TBLMASTERDATA\",\"TBLTRANSMAKES\" WHERE \"TC_RATING\"=\"MD_ID\" ";
                //strQry += " AND \"TC_MAKE_ID\"=\"TM_ID\"  AND \"MD_TYPE\"='SR' and \"TC_CURRENT_LOCATION\" =1 ";
                //strQry += " AND \"TC_CAPACITY\" BETWEEN 125 AND 250 AND \"TC_LOCATION_ID\" = '" + sOfficeCode + "' ";
                //Dt = ObjCon.FetchDataTable(strQry);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_btw125_250_capacitytc_details");
                cmd.Parameters.AddWithValue("p_officecode", Convert.ToString(sOfficeCode ?? ""));
                Dt = ObjCon.FetchDataTable(cmd);

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return Dt;
        }
        public DataTable Loadgreater250CapacityTCDetails(string sOfficeCode)
        {
            DataTable Dt = new DataTable();
            try
            {
                #region old inline query
                //string strQry = string.Empty;
                //strQry = " SELECT\"TC_CODE\",\"TC_SLNO\",\"TC_CAPACITY\",\"TM_NAME\",(\"MD_NAME\")\"TC_RATING\" ";
                //strQry += " FROM \"TBLTCMASTER\",\"TBLMASTERDATA\",\"TBLTRANSMAKES\" WHERE \"TC_RATING\"=\"MD_ID\" ";
                //strQry += " AND \"TC_MAKE_ID\"=\"TM_ID\"  AND \"MD_TYPE\"='SR' and \"TC_CURRENT_LOCATION\" =1 ";
                //strQry += " AND \"TC_CAPACITY\" > 250 AND \"TC_LOCATION_ID\" = '" + sOfficeCode + "' ";
                //Dt = ObjCon.FetchDataTable(strQry);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_greater250_capacitytc_details");
                cmd.Parameters.AddWithValue("p_officecode", Convert.ToString(sOfficeCode ?? ""));
                Dt = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return Dt;
        }

        public DataTable LoadTCpending_issue_countDetails(string sOfficeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                string sOffCode = clsStoreOffice.GetOfficeCode(sOfficeCode, "TRANS_REF_OFF_CODE");

                #region old inline query
                //string strQry = string.Empty;
                //strQry = " SELECT \"TC_CODE\",\"TC_SLNO\",\"TC_CAPACITY\",\"TM_NAME\",(\"MD_NAME\")\"TC_RATING\" ";
                //strQry += " FROM \"TBLPENDINGTRANSACTION\",\"TBLTCMASTER\",\"TBLDTCFAILURE\",\"TBLMASTERDATA\",\"TBLTRANSMAKES\" ";
                //strQry += " WHERE \"TC_MAKE_ID\"=\"TM_ID\" AND \"TRANS_BO_ID\" in(29,13)  AND \"TRANS_BO_ID\"<>10 ";
                //strQry += " AND \"TC_RATING\"=\"MD_ID\" AND \"MD_TYPE\"='SR'AND \"TRANS_DTC_CODE\"=\"DF_DTC_CODE\" ";
                //strQry += " and \"DF_EQUIPMENT_ID\"=\"TC_CODE\"  and \"DF_REPLACE_FLAG\"<>1 and \"TRANS_NEXT_ROLE_ID\"<>0 and";                
                //strQry += sOffCode;
                //dt = ObjCon.FetchDataTable(strQry);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_tcpending_issue_count_details");
                cmd.Parameters.AddWithValue("p_value", Convert.ToString(sOffCode ?? ""));
                dt = ObjCon.FetchDataTable(cmd);               
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
            return dt;
        }

        public DataTable LoadTCpending_repair_countDetails(string sOfficeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                #region old inline query
                //string strQry = string.Empty;
                //strQry = " SELECT \"TC_CODE\",\"TC_SLNO\",\"TC_CAPACITY\",\"TM_NAME\",(\"MD_NAME\")\"TC_RATING\" ";
                //strQry += " FROM \"TBLTCMASTER\" ,\"TBLMASTERDATA\",\"TBLTRANSMAKES\" WHERE \"TC_CURRENT_LOCATION\" =3 ";
                //strQry += " AND \"TC_MAKE_ID\"=\"TM_ID\" AND  \"TC_STATUS\" = 3 AND \"TC_RATING\"=\"MD_ID\" ";
                //strQry += " AND \"MD_TYPE\"='SR'AND \"TC_LOCATION_ID\"='" + sOfficeCode + "' ";
                //strQry += " and \"TC_CODE\" in ( SELECT \"RSD_TC_CODE\"  FROM \"TBLREPAIRSENTMASTER\",\"TBLREPAIRSENTDETAILS\" WHERE ";
                //strQry += " \"RSM_ID\"=\"RSD_RSM_ID\" and \"RSD_RV_NO\" is null and  \"RSM_DIV_CODE\" = (select \"SM_CODE\" from ";
                //strQry += " \"TBLSTOREMAST\" where \"SM_ID\"= '" + sOfficeCode + "')) ";
                //dt = ObjCon.FetchDataTable(strQry);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_tcpending_repair_count_details");
                cmd.Parameters.AddWithValue("p_offcode", Convert.ToString(sOfficeCode ?? ""));
                dt = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
            return dt;
        }

        public DataTable LoadTCpending_release_countDetails(string sOfficeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                #region Old inline query
                //string strQry = string.Empty;
                //strQry = "	SELECT \"TC_CODE\",\"TC_SLNO\",\"TC_CAPACITY\",\"TM_NAME\",(\"MD_NAME\")\"TC_RATING\" FROM \"TBLTCMASTER\" INNER JOIN ";
                //strQry += " \"TBLMASTERDATA\" ON \"TC_RATING\"=\"MD_ID\" AND \"MD_TYPE\"='SR' INNER JOIN \"TBLTRANSMAKES\" ON \"TC_MAKE_ID\"=\"TM_ID\" ";
                //strQry += " INNER JOIN \"TBLDTCFAILURE\" ON \"TC_CODE\"=\"DF_EQUIPMENT_ID\" INNER JOIN \"TBLWORKORDER\" on \"DF_ID\"=\"WO_DF_ID\" ";
                //strQry += " INNER JOIN \"TBLTCREPLACE\" on  \"WO_SLNO\"=\"TR_WO_SLNO\" and \"TR_RV_NO\" is null and \"TR_RI_NO\" is not null and \"WO_SLNO\" is not null  and ";
                //strQry += " \"TR_STORE_SLNO\"='" + sOfficeCode + "' ";
                //dt = ObjCon.FetchDataTable(strQry);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_tcpending_release_count_details");
                cmd.Parameters.AddWithValue("p_offcode", Convert.ToString(sOfficeCode ?? ""));
                dt = ObjCon.FetchDataTable(cmd);                
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
            return dt;
        }
        public DataTable Load_Tc_Good(string sOfficeCode)
        {
            DataTable dt = new DataTable();           
            try
            {
                #region old inline query
                //string strQry = string.Empty;
                //strQry = " SELECT \"TC_CODE\",\"TC_SLNO\",\"TC_CAPACITY\",\"TM_NAME\",(\"MD_NAME\")\"TC_RATING\" ";
                //strQry += " FROM \"TBLTCMASTER\",\"TBLMASTERDATA\",\"TBLTRANSMAKES\" WHERE \"TC_RATING\" = \"MD_ID\" AND \"TM_ID\" = \"TC_MAKE_ID\" ";
                //strQry += " AND \"MD_TYPE\" = 'SR' AND \"TC_STATUS\" = '1' AND \"TC_CURRENT_LOCATION\" = 5  AND \"TC_LOCATION_ID\"='" + sOfficeCode + "' ";
                //dt = ObjCon.FetchDataTable(strQry);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_tc_good_details");
                cmd.Parameters.AddWithValue("p_officecode", Convert.ToString(sOfficeCode ?? ""));
                dt = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
            return dt;
        }
        public DataTable Load_Tc_Release_Good(string sOfficeCode)
        {
            DataTable dt = new DataTable();            
            try
            {
                #region Old inline query
                //string strQry = string.Empty;
                //strQry = " SELECT \"TC_CODE\",\"TC_SLNO\",\"TC_CAPACITY\",\"TM_NAME\",(\"MD_NAME\")\"TC_RATING\" ";
                //strQry += " FROM \"TBLTCMASTER\",\"TBLMASTERDATA\",\"TBLTRANSMAKES\" WHERE \"TC_RATING\" = \"MD_ID\" AND \"TM_ID\" = \"TC_MAKE_ID\" ";
                //strQry += " AND \"MD_TYPE\" = 'SR' AND \"TC_STATUS\" = '11' AND \"TC_CURRENT_LOCATION\" = 5  AND \"TC_LOCATION_ID\"='" + sOfficeCode + "' ";
                //dt = ObjCon.FetchDataTable(strQry);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_tc_release_good_details");
                cmd.Parameters.AddWithValue("p_officecode", Convert.ToString(sOfficeCode ?? ""));
                dt = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return dt;
        }
        public DataTable Load_Tc_Repair_Good(string sOfficeCode)
        {
            DataTable dt = new DataTable();            
            try
            {
                #region Old Inline query
                //string strQry = string.Empty;
                //strQry = " SELECT \"TC_CODE\",\"TC_SLNO\",\"TC_CAPACITY\",\"TM_NAME\",(\"MD_NAME\")\"TC_RATING\" ";
                //strQry += " FROM \"TBLTCMASTER\",\"TBLMASTERDATA\",\"TBLTRANSMAKES\" WHERE \"TC_RATING\" = \"MD_ID\" AND \"TM_ID\" = \"TC_MAKE_ID\" ";
                //strQry += " AND \"MD_TYPE\" = 'SR' AND \"TC_STATUS\" = '2' AND \"TC_CURRENT_LOCATION\" = 5  AND \"TC_LOCATION_ID\"='" + sOfficeCode + "' ";
                //dt = ObjCon.FetchDataTable(strQry);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_tc_repair_good_details");
                cmd.Parameters.AddWithValue("p_officecode", Convert.ToString(sOfficeCode ?? ""));
                dt = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public DataTable GetDtrDetails(string off_code, string Dtrtype)
        {
            DataTable dtDTRDetails = new DataTable();
            string strQry = string.Empty;
            // if (Dtrtype == "0")
            //{
            #region Inline query

            //strQry = "SELECT \"TC_ID\",cast(\"TC_CODE\" as text),\"TC_SLNO\",(SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='SR' AND \"MD_ID\"=\"TC_RATING\")\"TC_RATING\",\"TC_CAPACITY\",\"TM_NAME\" FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\" WHERE \"TC_MAKE_ID\"=\"TM_ID\" AND cast(\"TC_LOCATION_ID\" as text) LIKE '" + off_code + "%' AND  \"TC_CODE\" <> '0' and \"TC_CAPACITY\" <>0";
            //if (off_code == "" || off_code == null)
            //{
            //    strQry += " ORDER BY \"TC_ID\" DESC  LIMIT 300";
            //}
            //else
            //{
            //    strQry += " ORDER BY \"TC_ID\" DESC ";
            //}
            //dtDTRDetails = ObjCon.FetchDataTable(strQry);
            //#endregion



            // }
            // else if (Dtrtype == "1")
            //{

            // strQry = "SELECT \"TC_ID\",cast(\"TC_CODE\" as text),\"TC_SLNO\",\"TM_NAME\",\"TC_CAPACITY\",(SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='SR' AND \"MD_ID\"=\"TC_RATING\")\"TC_RATING\" FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\" WHERE \"TC_MAKE_ID\"=\"TM_ID\" AND cast(\"TC_LOCATION_ID\" as text) LIKE '" + off_code + "%' AND \"TC_CURRENT_LOCATION\" in (1,4,6,7) AND  \"TC_CODE\" <> '0' and \"TC_CAPACITY\" <>0";
            // if (off_code == "" || off_code == null)
            // {
            //  strQry += " ORDER BY \"TC_ID\" DESC  LIMIT 300";
            // }
            //  else
            // {
            //     strQry += " ORDER BY \"TC_ID\" DESC ";
            // }
            // dtDTRDetails = ObjCon.FetchDataTable(strQry);



            //  }
            // else
            // {

            //strQry = "SELECT \"TC_ID\",cast(\"TC_CODE\" as text),\"TC_SLNO\",\"TM_NAME\",\"TC_CAPACITY\",(SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='SR' AND \"MD_ID\"=\"TC_RATING\")\"TC_RATING\" FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\" WHERE \"TC_MAKE_ID\"=\"TM_ID\" AND cast(\"TC_LOCATION_ID\" as text) LIKE '" + off_code + "%' AND \"TC_CURRENT_LOCATION\"='" + Dtrtype + "' AND  \"TC_CODE\" <> '0' and \"TC_CAPACITY\" <>0";
            // if (off_code == "" || off_code == null)
            //    {
            //   strQry += " ORDER BY \"TC_ID\" DESC  LIMIT 300";
            // }
            // else
            //  {
            //     strQry += " ORDER BY \"TC_ID\" DESC ";
            // }
            // dtDTRDetails = ObjCon.FetchDataTable(strQry);


            // }
            //else if (Dtrtype == "1")
            //{
            //    strQry = "SELECT * FROM \"TBLTCMASTER\" WHERE cast(\"TC_LOCATION_ID\" as text) LIKE '" + off_code + "%' AND \"TC_CURRENT_LOCATION\"='"+Dtrtype+"'";
            //    dtDTRDetails = ObjCon.FetchDataTable(strQry);
            //}
            //else if (Dtrtype == "2")
            //{
            //    strQry = "SELECT * FROM \"TBLTCMASTER\" WHERE cast(\"TC_LOCATION_ID\" as text) LIKE '" + off_code + "%' AND \"TC_CURRENT_LOCATION\"='" + Dtrtype + "'";
            //    dtDTRDetails = ObjCon.FetchDataTable(strQry);
            //}
            //else if (Dtrtype == "3")
            //{
            //    strQry = "SELECT * FROM \"TBLTCMASTER\" WHERE cast(\"TC_LOCATION_ID\" as text) LIKE '" + off_code + "%' AND \"TC_CURRENT_LOCATION\"='" + Dtrtype + "'";
            //    dtDTRDetails = ObjCon.FetchDataTable(strQry);
            //}
            //else if (Dtrtype == "5")
            //{
            //    strQry = "SELECT * FROM \"TBLTCMASTER\" WHERE cast(\"TC_LOCATION_ID\" as text) LIKE '" + off_code + "%' AND \"TC_CURRENT_LOCATION\"='" + Dtrtype + "'";
            //    dtDTRDetails = ObjCon.FetchDataTable(strQry);
            //}
            #endregion
            NpgsqlCommand cmd = new NpgsqlCommand("sp_getdtrdetailsforstoreandfield");
            cmd.Parameters.AddWithValue("dtrtype", Convert.ToString(Dtrtype == null ? "" : Dtrtype));
            cmd.Parameters.AddWithValue("offcode", Convert.ToString(off_code == null ? "" : off_code));

            dtDTRDetails = ObjCon.FetchDataTable(cmd);
            return dtDTRDetails;
        }
        public DataTable GetDtrDetailsdt(string off_code, string Dtrtype)
        {
            DataTable dtDTRDetails = new DataTable();
            string strQry = string.Empty;
            if (Dtrtype == "0")
            {
                strQry = "SELECT \"TC_ID\",cast(\"TC_CODE\" as text),\"TC_SLNO\",\"TM_NAME\",\"TC_CAPACITY\",(SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='SR' AND \"MD_ID\"=\"TC_RATING\")\"TC_RATING\" FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\" WHERE \"TC_MAKE_ID\"=\"TM_ID\" AND cast(\"TC_LOCATION_ID\" as text) LIKE '" + off_code + "%'";

                strQry += " ORDER BY \"TC_ID\" DESC ";

                dtDTRDetails = ObjCon.FetchDataTable(strQry);
            }
            else if (Dtrtype == "1")
            {
                strQry = "SELECT \"TC_ID\",cast(\"TC_CODE\" as text),\"TC_SLNO\",\"TM_NAME\",\"TC_CAPACITY\",(SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='SR' AND \"MD_ID\"=\"TC_RATING\")\"TC_RATING\" FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\" WHERE \"TC_MAKE_ID\"=\"TM_ID\" AND cast(\"TC_LOCATION_ID\" as text) LIKE '" + off_code + "%' AND \"TC_CURRENT_LOCATION\" in (1,4,6,7)";

                strQry += " ORDER BY \"TC_ID\" DESC ";

                dtDTRDetails = ObjCon.FetchDataTable(strQry);
            }
            else
            {
                strQry = "SELECT \"TC_ID\",cast(\"TC_CODE\" as text),\"TC_SLNO\",\"TM_NAME\",\"TC_CAPACITY\",(SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='SR' AND \"MD_ID\"=\"TC_RATING\")\"TC_RATING\" FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\" WHERE \"TC_MAKE_ID\"=\"TM_ID\" AND cast(\"TC_LOCATION_ID\" as text) LIKE '" + off_code + "%' AND \"TC_CURRENT_LOCATION\"='" + Dtrtype + "'";


                strQry += " ORDER BY \"TC_ID\" DESC ";

                dtDTRDetails = ObjCon.FetchDataTable(strQry);
            }

            //}
            return dtDTRDetails;
        }

        public string GetDtrCount(string off_code, string Dtrtype)
        {
            string strQry = string.Empty;
            string DtrCount = string.Empty;
            if (Dtrtype == "0")
            {
                strQry = "SELECT COUNT(\"TC_ID\") FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\" WHERE \"TC_MAKE_ID\"=\"TM_ID\" AND cast(\"TC_LOCATION_ID\" as text) LIKE '" + off_code + "%' AND \"TC_CODE\" <> '0'";
                DtrCount = ObjCon.get_value(strQry);
            }
            else if (Dtrtype == "1")
            {
                strQry = "SELECT COUNT(\"TC_ID\") FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\" WHERE \"TC_MAKE_ID\"=\"TM_ID\" AND cast(\"TC_LOCATION_ID\" as text) LIKE '" + off_code + "%' AND \"TC_CURRENT_LOCATION\" in (1,4,6,7)";
                DtrCount = ObjCon.get_value(strQry);
            }
            else
            {
                strQry = "SELECT COUNT(\"TC_ID\") FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\" WHERE \"TC_MAKE_ID\"=\"TM_ID\" AND cast(\"TC_LOCATION_ID\" as text) LIKE '" + off_code + "%' AND \"TC_CURRENT_LOCATION\"='" + Dtrtype + "'";
                DtrCount = ObjCon.get_value(strQry);
            }
            return DtrCount;
        }

        public DataTable getFailurePendingCounts(string sOfficeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                //string strQry = string.Empty;
                //strQry = "SELECT COALESCE(SUM(CASE WHEN \"TRANS_BO_ID\" = 9 THEN 1 ELSE 0 END ),0)AS \"FAILURE_APPROVE\",COALESCE(SUM(CASE WHEN \"TRANS_BO_ID\" in(45)  and \"TRANS_NEXT_ROLE_ID\"<>0 THEN 1 ELSE 0 END ),0) AS \"PENDING_ESTIMATION\",COALESCE(SUM(CASE WHEN \"TRANS_BO_ID\" IN(15) and \"TRANS_NEXT_ROLE_ID\"<>0  THEN 1 ELSE 0 END ),0) AS \"PENDING_RI\",COALESCE(SUM(CASE WHEN \"TRANS_BO_ID\" IN(26) and \"TRANS_NEXT_ROLE_ID\"<>0  THEN 1 ELSE 0 END ),0) AS \"PENDING_CR\",COALESCE(SUM(CASE WHEN \"TRANS_BO_ID\"in(13) and \"TRANS_NEXT_ROLE_ID\"<>0 THEN 1 ELSE 0 END ),0) AS \"PENDING_MAJOR_INV\", COALESCE(SUM(CASE WHEN  \"TRANS_BO_ID\" in(11) and \"TRANS_NEXT_ROLE_ID\"<>0 THEN 1 ELSE 0 END ),0)AS \"PEN_MULTI_COIL_WOR\" FROM \"TBLPENDINGTRANSACTION\" WHERE CAST(\"TRANS_REF_OFF_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%' and  \"TRANS_NEXT_ROLE_ID\"<>0";
                //dt = ObjCon.FetchDataTable(strQry);
                //return dt;

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_failure_pend_counts");
                cmd.Parameters.AddWithValue("ofc_code", sOfficeCode);
                dt = ObjCon.FetchDataTable(cmd);
                return dt;


            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable getFailureCounts(string sOfficeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                //string strQry = string.Empty;
                //strQry = "SELECT  count(distinct \"TRANS_DTC_CODE\") AS \"TOTA_DTC_FAILURE\" from \"TBLPENDINGTRANSACTION\" WHERE \"TRANS_BO_ID\"  in(45,11,13)  and CAST(\"TRANS_REF_OFF_CODE\" AS TEXT) like '" + sOfficeCode + "%' and \"TRANS_NEXT_ROLE_ID\"<>0";
                //dt = ObjCon.FetchDataTable(strQry);
                //return dt;

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_failure_count");
                cmd.Parameters.AddWithValue("ofc_code", sOfficeCode);
                dt = ObjCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable GetFaultyTCFields(clsDashboard objDashboard)
        {
            string sFolderPath = Convert.ToString(ConfigurationSettings.AppSettings["LOGFILEPATH"]) + DateTime.Now.ToString("yyyyMM");

            DataTable dt = new DataTable();
            try
            {

                // clsException.WriteLogFile("", "", "start", "");
                string strQry = string.Empty;
                //strQry = "select \"Field_Count\"+\"Store_count\"+\"Repair_Count\" AS \"total_count\",* from (SELECT sum(case when \"TC_CURRENT_LOCATION\"='2'  and   \"TC_STATUS\"='3' AND cast(\"TC_LOCATION_ID\" as text) like '" + objDashboard.sOfficeCode + "%' then 1 else 0 end) as \"Field_Count\",sum(case when \"TC_CURRENT_LOCATION\"='1' AND  \"TC_STATUS\"='3'";
                //if (objDashboard.sOfficeCode != "")
                //{
                //    strQry += " and CAST(\"TC_LOCATION_ID\" AS TEXT) = '" + clsStoreOffice.GetStoreID(objDashboard.sOfficeCode) + "'";
                //}
                //strQry += " then 1 else 0 end) as \"Store_count\",sum(case when \"TC_CURRENT_LOCATION\"='3' and   \"TC_STATUS\"='3'";
                //if (objDashboard.sOfficeCode != "")
                //{
                //    strQry += " and CAST(\"TC_LOCATION_ID\" AS TEXT) = '" + clsStoreOffice.GetStoreID(objDashboard.sOfficeCode) + "'";
                //}
                //strQry += " then 1 else 0 end) as \"Repair_Count\",sum(case when \"TC_CURRENT_LOCATION\"='1' and   \"TC_STATUS\"='2'";
                //if (objDashboard.sOfficeCode != "")
                //{
                //    strQry += " and CAST(\"TC_LOCATION_ID\" AS TEXT) = '" + clsStoreOffice.GetStoreID(objDashboard.sOfficeCode) + "'";
                //}

                //strQry += "then 1 else 0 end) as \"Repair_Good\" from \"TBLTCMASTER\" where \"TC_CODE\"<>'0' )A ";
                //dt = ObjCon.FetchDataTable(strQry);
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getfaultytcfieldsfordashboard");
                cmd.Parameters.AddWithValue("offcode", objDashboard.sOfficeCode);
                cmd.Parameters.AddWithValue("storeoffcode", clsStoreOffice.GetStoreID(objDashboard.sOfficeCode));
                dt = ObjCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        public DataTable GetTcDetails(clsDashboard objDashboard)
        {

            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                string sRoleId = string.Empty;
                // strQry = "SELECT COUNT(\"TC_ID\") as \"total_tc\",SUM(case when \"TC_CURRENT_LOCATION\" in (1,4,6,7) then 1 else 0 end) as \"TotalStoreDtr\" , SUM(case when \"TC_CURRENT_LOCATION\"=2 then 1 else 0 end)  AS \"field_count\",SUM(case when \"TC_CURRENT_LOCATION\"=3 then 1 else 0 end) as \"repairer_count\",SUM(case when \"TC_CURRENT_LOCATION\"=5 then 1 else 0 end) as \"bank_count\"   FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\" WHERE \"TC_MAKE_ID\"=\"TM_ID\" AND cast(\"TC_LOCATION_ID\" as text) LIKE '" + objDashboard.sOfficeCode + "%' AND \"TC_CODE\" <> '0'  and \"TC_CAPACITY\" <>0";
                // dt = ObjCon.FetchDataTable(strQry);

                if (objDashboard.sRoleId == "8" || objDashboard.sRoleId == "11")
                    {
                        NpgsqlCommand command = new NpgsqlCommand("sp_gettcdetailsfordashboard");
                        command.Parameters.AddWithValue("offcode", objDashboard.sOfficeCode);
                        dt = ObjCon.FetchDataTable(command);
                    }
                    else
                    {
                        string store_id = clsStoreOffice.GetStoreID(objDashboard.sOfficeCode);
                        NpgsqlCommand command = new NpgsqlCommand("dashboard_totaldtrcount_web");
                        command.Parameters.AddWithValue("officecode", Convert.ToString(objDashboard.sOfficeCode));
                        command.Parameters.AddWithValue("storeid", Convert.ToString(store_id));
                        dt = ObjCon.FetchDataTable(command);
                        return dt;
                    }
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }

        }

        public DataTable GetDtrDetailssearch(string off_code, string Dtrtype, string tccode, string tcslno)
        {
            DataTable dtDTRDetails = new DataTable();
            string strQry = string.Empty;
            if (Dtrtype == "0")
            {
                strQry = "SELECT \"TC_ID\",cast(\"TC_CODE\" as text),\"TC_SLNO\",\"TM_NAME\",\"TC_CAPACITY\",(SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='SR' AND \"MD_ID\"=\"TC_RATING\")\"TC_RATING\" FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\" WHERE \"TC_MAKE_ID\"=\"TM_ID\" AND cast(\"TC_LOCATION_ID\" as text) LIKE '" + off_code + "%' and cast(\"TC_CODE\" AS TEXT) LIKE '" + tccode + "%' and cast(\"TC_SLNO\" as text) LIKE '" + tcslno + "%'";
                if (off_code == "" || off_code == null)
                {
                    strQry += " ORDER BY \"TC_ID\" DESC  LIMIT 100";
                }
                else
                {
                    strQry += " ORDER BY \"TC_ID\" DESC ";
                }
                // 
                dtDTRDetails = ObjCon.FetchDataTable(strQry);
            }
            else if (Dtrtype == "1")
            {
                strQry = "SELECT \"TC_ID\",cast(\"TC_CODE\" as text),\"TC_SLNO\",\"TM_NAME\",\"TC_CAPACITY\",(SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='SR' AND \"MD_ID\"=\"TC_RATING\")\"TC_RATING\" FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\" WHERE \"TC_MAKE_ID\"=\"TM_ID\" AND cast(\"TC_LOCATION_ID\" as text) LIKE '" + off_code + "%' AND \"TC_CURRENT_LOCATION\" in (1,4,6,7) and cast(\"TC_CODE\" AS TEXT) LIKE '" + tccode + "%' and cast(\"TC_SLNO\" as text) LIKE '" + tcslno + "%'";
                if (off_code == "" || off_code == null)
                {
                    strQry += " ORDER BY \"TC_ID\" DESC  LIMIT 100";
                }
                else
                {
                    strQry += " ORDER BY \"TC_ID\" DESC ";
                }
                dtDTRDetails = ObjCon.FetchDataTable(strQry);
            }
            else
            {
                strQry = "SELECT \"TC_ID\",cast(\"TC_CODE\" as text),\"TC_SLNO\",\"TM_NAME\",\"TC_CAPACITY\",(SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='SR' AND \"MD_ID\"=\"TC_RATING\")\"TC_RATING\" FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\" WHERE \"TC_MAKE_ID\"=\"TM_ID\" AND cast(\"TC_LOCATION_ID\" as text) LIKE '" + off_code + "%' AND \"TC_CURRENT_LOCATION\"='" + Dtrtype + "' and cast(\"TC_CODE\" AS TEXT) LIKE '" + tccode + "%' and cast(\"TC_SLNO\" as text) LIKE '" + tcslno + "%'";
                if (off_code == "" || off_code == null)
                {
                    strQry += " ORDER BY \"TC_ID\" DESC  LIMIT 100";
                }
                else
                {
                    strQry += " ORDER BY \"TC_ID\" DESC ";
                }
                dtDTRDetails = ObjCon.FetchDataTable(strQry);
            }


            return dtDTRDetails;
        }

        public DataTable LoadDtrDetailsexcel(string off_code, string Dtrtype)
        {
            DataTable dtDTRDetails = new DataTable();
            string strQry = string.Empty;
            if (Dtrtype == "0")
            {
                strQry = "SELECT \"TC_ID\",cast(\"TC_CODE\" as text),\"TC_SLNO\",(SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='SR' AND \"MD_ID\"=\"TC_RATING\")\"TC_RATING\",\"TC_CAPACITY\",\"TM_NAME\" FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\" WHERE \"TC_MAKE_ID\"=\"TM_ID\" AND cast(\"TC_LOCATION_ID\" as text) LIKE '" + off_code + "%' AND \"TC_CODE\" <> '0'";
                if (off_code == "" || off_code == null)
                {
                    strQry += " ORDER BY \"TC_ID\" DESC  ";
                }
                else
                {
                    strQry += " ORDER BY \"TC_ID\" DESC ";
                }
                // 
                dtDTRDetails = ObjCon.FetchDataTable(strQry);
            }
            else if (Dtrtype == "1")
            {
                strQry = "SELECT \"TC_ID\",cast(\"TC_CODE\" as text),\"TC_SLNO\",(SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='SR' AND \"MD_ID\"=\"TC_RATING\")\"TC_RATING\",\"TC_CAPACITY\",\"TM_NAME\" FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\" WHERE \"TC_MAKE_ID\"=\"TM_ID\" AND cast(\"TC_LOCATION_ID\" as text) LIKE '" + off_code + "%' AND \"TC_CURRENT_LOCATION\" in (1,4,6,7) AND \"TC_CODE\" <> '0'";
                if (off_code == "" || off_code == null)
                {
                    strQry += " ORDER BY \"TC_ID\" DESC  ";
                }
                else
                {
                    strQry += " ORDER BY \"TC_ID\" DESC ";
                }
                dtDTRDetails = ObjCon.FetchDataTable(strQry);
            }
            else
            {
                strQry = "SELECT \"TC_ID\",cast(\"TC_CODE\" as text),\"TC_SLNO\",(SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='SR' AND \"MD_ID\"=\"TC_RATING\")\"TC_RATING\",\"TC_CAPACITY\",\"TM_NAME\" FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\" WHERE \"TC_MAKE_ID\"=\"TM_ID\" AND cast(\"TC_LOCATION_ID\" as text) LIKE '" + off_code + "%' AND \"TC_CURRENT_LOCATION\"='" + Dtrtype + "' AND \"TC_CODE\" <> '0'";
                if (off_code == "" || off_code == null)
                {
                    strQry += " ORDER BY \"TC_ID\" DESC  ";
                }
                else
                {
                    strQry += " ORDER BY \"TC_ID\" DESC ";
                }
                dtDTRDetails = ObjCon.FetchDataTable(strQry);
            }

            return dtDTRDetails;
        }

        public DataTable GetDtrDetailssearchtcslno(string off_code, string Dtrtype, string tccode, string tcslno)
        {
            DataTable dtDTRDetails = new DataTable();
            string strQry = string.Empty;
            if (Dtrtype == "0")
            {
                strQry = "SELECT \"TC_ID\",cast(\"TC_CODE\" as text),\"TC_SLNO\",\"TM_NAME\",\"TC_CAPACITY\",(SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='SR' AND \"MD_ID\"=\"TC_RATING\")\"TC_RATING\" FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\" WHERE \"TC_MAKE_ID\"=\"TM_ID\" AND cast(\"TC_LOCATION_ID\" as text) LIKE '" + off_code + "%' and cast(\"TC_CODE\" AS TEXT) LIKE '" + tccode + "%' and cast(\"TC_SLNO\" AS TEXT) LIKE '" + tcslno + "%' AND \"TC_CODE\" <> '0'";
                if (off_code == "" || off_code == null)
                {
                    strQry += " ORDER BY \"TC_ID\" DESC  LIMIT 100";
                }
                else
                {
                    strQry += " ORDER BY \"TC_ID\" DESC ";
                }
                // 
                dtDTRDetails = ObjCon.FetchDataTable(strQry);
            }
            else if (Dtrtype == "1")
            {
                strQry = "SELECT \"TC_ID\",cast(\"TC_CODE\" as text),\"TC_SLNO\",\"TM_NAME\",\"TC_CAPACITY\",(SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='SR' AND \"MD_ID\"=\"TC_RATING\")\"TC_RATING\" FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\" WHERE \"TC_MAKE_ID\"=\"TM_ID\" AND cast(\"TC_LOCATION_ID\" as text) LIKE '" + off_code + "%' AND \"TC_CURRENT_LOCATION\" in (1,4,6,7) and cast(\"TC_CODE\" AS TEXT) LIKE '" + tccode + "%'  and cast(\"TC_SLNO\" AS TEXT) LIKE '" + tcslno + "%' AND \"TC_CODE\" <> '0'";
                if (off_code == "" || off_code == null)
                {
                    strQry += " ORDER BY \"TC_ID\" DESC  LIMIT 100";
                }
                else
                {
                    strQry += " ORDER BY \"TC_ID\" DESC ";
                }
                dtDTRDetails = ObjCon.FetchDataTable(strQry);
            }
            else
            {

                strQry = "SELECT \"TC_ID\",cast(\"TC_CODE\" as text),\"TC_SLNO\",\"TM_NAME\",\"TC_CAPACITY\",(SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='SR' AND \"MD_ID\"=\"TC_RATING\")\"TC_RATING\" FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\" WHERE \"TC_MAKE_ID\"=\"TM_ID\" AND cast(\"TC_LOCATION_ID\" as text) LIKE '" + off_code + "%' AND \"TC_CURRENT_LOCATION\"='" + Dtrtype + "' and cast(\"TC_CODE\" AS TEXT) LIKE '" + tccode + "%'  and cast(\"TC_SLNO\" AS TEXT) LIKE '" + tcslno + "%' AND  \"TC_CODE\" <> '0'";
                if (off_code == "" || off_code == null)
                {
                    strQry += " ORDER BY \"TC_ID\" DESC  LIMIT 100";
                }
                else
                {
                    strQry += " ORDER BY \"TC_ID\" DESC ";
                }
                dtDTRDetails = ObjCon.FetchDataTable(strQry);
            }


            return dtDTRDetails;
        }


        public DataTable LoadDTCFailureAbstractofficewise(clsDashboard objDashboard)
        {
            DataTable dt = new DataTable();
            try
            {
                int Length = 0;
                Length = objDashboard.sOfficeCode.Length + 1;

                #region Old inline query
                //string strQry = string.Empty;
                //strQry = " SELECT *,\"PRESENTCOUNT\"+\"PREVIOUSCOUNT\" AS \"TOTAL_DTCCOUNT\" FROM (SELECT \"OFF_CODE\",SUM(CASE WHEN ";
                //strQry += " ((\"DF_DATE\" IS NULL AND TO_CHAR(\"DF_DATE\",'YYYY')=TO_CHAR(CURRENT_DATE,'YYYY')) OR (\"DF_DATE\" IS NOT";
                //strQry += "  NULL AND TO_CHAR(\"DF_DATE\",'YYYY')=TO_CHAR(CURRENT_DATE,'YYYY'))) THEN 1 ELSE 0 END) AS \"PRESENTCOUNT\",SUM(CASE ";
                //strQry += "  WHEN ((\"DF_DATE\" IS NULL AND TO_CHAR(\"DF_DATE\",'YYYY')=TO_CHAR(NOW()::TIMESTAMP - interval '1 YEAR','YYYY'))";
                //strQry += "  OR (\"DF_DATE\" IS NOT NULL AND TO_CHAR(\"DF_DATE\",'YYYY')=TO_CHAR(NOW()::TIMESTAMP - interval '1 YEAR','YYYY'))) THEN 1 ELSE 0 END) as \"PREVIOUSCOUNT\",";
                //strQry += "  \"OFF_NAME\" from \"TBLDTCFAILURE\"  INNER JOIN \"VIEW_ALL_OFFICES\" ON cast(\"OFF_CODE\" as text)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Length + ")  ";
                //strQry += "   AND \"DF_STATUS_FLAG\" IN (1,4) WHERE  CAST(\"OFF_CODE\" AS text) LIKE '" + objDashboard.sOfficeCode + "%' GROUP BY \"OFF_NAME\",\"OFF_CODE\")A ";
                //dt = ObjCon.FetchDataTable(strQry);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_dtcfailure_abstract_officewise");
                cmd.Parameters.AddWithValue("p_officecode", Convert.ToString(objDashboard.sOfficeCode ?? ""));
                cmd.Parameters.AddWithValue("p_length", Convert.ToString(Length));
                dt = ObjCon.FetchDataTable(cmd);

                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

    }
}
