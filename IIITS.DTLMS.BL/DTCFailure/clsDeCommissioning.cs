using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;
using System.Configuration;

namespace IIITS.DTLMS.BL
{
    public class clsDeCommissioning
    {

        string strFormCode = "clsDeCommissioning";

        public string sdtrcode { get; set; }

        public string sDecommId { get; set; }
        public string sFailureId { get; set; }
        public string sFailureDate { get; set; }
        public string sTRReading { get; set; }
        public string sRINo { get; set; }
        public string sRIDate { get; set; }
        public string sRVNo { get; set; }
        public string sRVDate { get; set; }
        public string sRemarks { get; set; }
        public string sStoreId { get; set; }
        public string sInvoiceId { get; set; }
        public string sCrby { get; set; }
        public string sTaskType { get; set; }
        public string sOfficeCode { get; set; }
        public string sDTCCode { get; set; }
        public string sOilQuantity { get; set; }
        public string sDecommDate { get; set; }
        public string sManualRINo { get; set; }
        public string sCommDate { get; set; }


        // Workflow
        public string sFormName { get; set; }
        public string sClientIP { get; set; }
        public string sWFOId { get; set; }
        public string sWFDataId { get; set; }
        public string sActionType { get; set; }
        public string sWFAutoId { get; set; }

        public string sDtrWarrentyTime { get; set; }
        public string sWarrentyPeriod { get; set; }
        public string sOiltype { get; set; }
        public string sbarrels { get; set; }

        //CustOledbConnection objcon = new CustOledbConnection(Constants.Password);
        PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
        NpgsqlCommand NpgsqlCommand;
        public DataTable LoadAllDecommission(clsDeCommissioning objDecomm)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {

                //strQry = "SELECT DF_ID,DT_NAME,TO_CHAR(DT_TC_ID) DT_TC_ID,(SELECT TC_SLNO FROM TBLTCMASTER WHERE DT_TC_ID = TC_CODE) TC_SLNO,TI_INDENT_NO,IN_INV_NO,IN_NO,";
                //strQry += " TR_ID,'YES' AS STATUS FROM TBLDTCMAST,TBLDTCFAILURE,TBLWORKORDER,TBLINDENT, TBLDTCINVOICE,TBLTCREPLACE";
                //strQry += " WHERE DT_CODE = DF_DTC_CODE  AND DF_ID = WO_DF_ID AND WO_SLNO = TI_WO_SLNO AND TI_ID = IN_TI_NO AND DF_STATUS_FLAG='" + objDecomm.sTaskType + "' ";
                //strQry += " AND TR_IN_NO=IN_NO AND DF_LOC_CODE LIKE '" + objDecomm.sOfficeCode + "%' ";
                //strQry += " UNION ALL ";
                //strQry += " SELECT DF_ID,DT_NAME,TO_CHAR(DT_TC_ID) DT_TC_ID,(SELECT TC_SLNO FROM TBLTCMASTER WHERE DT_TC_ID = TC_CODE) TC_SLNO,TI_INDENT_NO,IN_INV_NO,";
                //strQry += " IN_NO,0 AS TR_ID,'NO' AS STATUS FROM TBLDTCMAST,TBLTCMASTER,TBLDTCFAILURE,TBLWORKORDER,TBLINDENT,TBLDTCINVOICE";
                //strQry += " WHERE DF_EQUIPMENT_ID = TC_CODE  AND DT_CODE = DF_DTC_CODE  AND DF_ID =  WO_DF_ID AND WO_SLNO = TI_WO_SLNO AND TI_ID = IN_TI_NO AND DF_REPLACE_FLAG = 0";
                //strQry += " AND IN_NO NOT IN (SELECT TR_IN_NO FROM TBLTCREPLACE) AND DF_STATUS_FLAG='" + objDecomm.sTaskType + "'";
                //strQry += " AND DF_LOC_CODE LIKE '" + objDecomm.sOfficeCode + "%' ";

                //dt = objcon.getDataTable(strQry);

                NpgsqlCommand cmd = new NpgsqlCommand("sp_loadalldecommission");
                cmd.Parameters.AddWithValue("stasktype", objDecomm.sTaskType);
                cmd.Parameters.AddWithValue("sofficecode", objDecomm.sOfficeCode);
                dt = objcon.FetchDataTable(cmd);
                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }

        }

        public DataTable LoadCreateDecommission(clsDeCommissioning objDecomm)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                //strQry += " SELECT DF_ID,DT_NAME,TO_CHAR(DT_TC_ID) DT_TC_ID,(SELECT TC_SLNO FROM TBLTCMASTER WHERE DT_TC_ID = TC_CODE) TC_SLNO,TI_INDENT_NO,IN_INV_NO,";
                //strQry += " IN_NO,0 AS TR_ID,'NO' AS STATUS FROM TBLDTCMAST,TBLTCMASTER,TBLDTCFAILURE,TBLWORKORDER,TBLINDENT,TBLDTCINVOICE";
                //strQry += " WHERE DF_EQUIPMENT_ID = TC_CODE  AND DT_CODE = DF_DTC_CODE  AND DF_ID =  WO_DF_ID AND WO_SLNO = TI_WO_SLNO AND TI_ID = IN_TI_NO AND DF_REPLACE_FLAG = 0";
                //strQry += " AND IN_NO NOT IN (SELECT TR_IN_NO FROM TBLTCREPLACE) AND DF_STATUS_FLAG='" + objDecomm.sTaskType + "'";
                //strQry += " AND DF_LOC_CODE LIKE '" + objDecomm.sOfficeCode + "%' ";

                //dt = objcon.getDataTable(strQry);

                NpgsqlCommand cmd = new NpgsqlCommand("sp_loadcreatedecommission");
                cmd.Parameters.AddWithValue("stasktype", objDecomm.sTaskType);
                cmd.Parameters.AddWithValue("sofficecode", objDecomm.sOfficeCode);
                dt = objcon.FetchDataTable(cmd);

                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }

        }

        public DataTable LoadAlreadyDecomm(clsDeCommissioning objDecomm)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                //strQry = "SELECT DF_ID,DT_NAME,TO_CHAR(DT_TC_ID)DT_TC_ID,(SELECT TC_SLNO FROM TBLTCMASTER WHERE DT_TC_ID = TC_CODE) TC_SLNO,TI_INDENT_NO,IN_INV_NO,IN_NO,";
                //strQry += " TR_ID,'YES' AS STATUS FROM TBLDTCMAST,TBLDTCFAILURE,TBLWORKORDER,TBLINDENT, TBLDTCINVOICE,TBLTCREPLACE";
                //strQry += " WHERE DT_CODE = DF_DTC_CODE  AND DF_ID = WO_DF_ID AND WO_SLNO = TI_WO_SLNO AND TI_ID = IN_TI_NO AND DF_STATUS_FLAG='" + objDecomm.sTaskType + "' AND TR_IN_NO=IN_NO";
                //strQry += " AND DF_LOC_CODE LIKE '" + objDecomm.sOfficeCode + "%' AND DF_REPLACE_FLAG='0'";
                //dt = objcon.getDataTable(strQry);

                NpgsqlCommand cmd = new NpgsqlCommand("sp_loadalreadydecomm");
                cmd.Parameters.AddWithValue("stasktype", objDecomm.sTaskType);
                cmd.Parameters.AddWithValue("sofficecode", objDecomm.sOfficeCode);
                dt = objcon.FetchDataTable(cmd);

                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }

        }

        public void updateRecord(string DtcCode)
        {
          
            string strQry = string.Empty;
            string strTemp;
            try
            {
                //MOVED TO INVOICE 

               // NpgsqlCommand = new NpgsqlCommand();
               // strQry = "SELECT \"DF_ID\" FROM \"TBLDTCFAILURE\" WHERE \"DF_DTC_CODE\" =:DtcCode AND \"DF_REPLACE_FLAG\" ='0'";
               // NpgsqlCommand.Parameters.AddWithValue("DtcCode", DtcCode);
               // string df_id = objcon.get_value(strQry, NpgsqlCommand);

               // NpgsqlCommand = new NpgsqlCommand();
               // strTemp = "SELECT \"TD_TC_NO\" FROM \"TBLTCDRAWN\" WHERE \"TD_DF_ID\" =:df_id";
               //NpgsqlCommand.Parameters.AddWithValue("df_id", Convert.ToInt32(df_id));
               // string sReplaceTCCode = objcon.get_value(strTemp, NpgsqlCommand);


               // NpgsqlCommand = new NpgsqlCommand();
               // strQry = "SELECT CASE WHEN NOW() < \"TC_WARANTY_PERIOD\" THEN 1 ELSE 0 END FROM \"TBLTCMASTER\" WHERE \"TC_CODE\" =:sReplaceTCCode";
               // NpgsqlCommand.Parameters.AddWithValue("sReplaceTCCode", Convert.ToDouble(sReplaceTCCode));
               // string sval = objcon.get_value(strQry, NpgsqlCommand);

               // if (sval == "0")
               // {
               //     NpgsqlCommand = new NpgsqlCommand();
               //     strQry = "SELECT TO_CHAR(ADD_MONTHS(\"TM_MAPPING_DATE\",\"WARENTY_MONTH\"),'YYYY-MM-DD') WARENTY_DATE FROM (SELECT \"RSD_GUARRENTY_TYPE\" ,";
               //     strQry += " (\"RSD_WARENTY_PERIOD\" * 12) \"WARENTY_MONTH\", \"TC_CODE\", \"TM_MAPPING_DATE\" FROM \"TBLTRANSDTCMAPPING\", \"TBLREPAIRSENTDETAILS\",";
               //     strQry += " \"TBLTCMASTER\" WHERE  \"TC_CODE\"=\"RSD_TC_CODE\" AND \"RSD_TC_CODE\"=\"TM_TC_ID\" AND \"TC_CODE\"=:sReplaceTCCode1 AND \"TM_LIVE_FLAG\" =1)A";
               //     NpgsqlCommand.Parameters.AddWithValue("sReplaceTCCode1", Convert.ToDouble(sReplaceTCCode));
               //     sDtrWarrentyTime = objcon.get_value(strQry, NpgsqlCommand);

               //     NpgsqlCommand = new NpgsqlCommand();
               //     strQry = "SELECT \"RSD_WARENTY_PERIOD\" FROM \"TBLTRANSDTCMAPPING\", \"TBLREPAIRSENTDETAILS\", \"TBLTCMASTER\" WHERE \"TC_CODE\" = \"RSD_TC_CODE\" AND ";
               //     strQry += " \"RSD_TC_CODE\" = \"TM_TC_ID\"  AND \"TC_CODE\" =:sReplaceTCCode2 AND \"TM_LIVE_FLAG\" =1";
               //     NpgsqlCommand.Parameters.AddWithValue("sReplaceTCCode2", Convert.ToDouble(sReplaceTCCode));
               //     sWarrentyPeriod = objcon.get_value(strQry, NpgsqlCommand);
               // }
               // //IFormatProvider culture = new CultureInfo("en-US", true);
               // //DateTime dateVal = DateTime.ParseExact(sDtrWarrentyTime, "yyyy-MM-dd", culture);

               // if (sval == "0")
               // {
               //     NpgsqlCommand = new NpgsqlCommand();
               //     strQry = "UPDATE \"TBLTCMASTER\" SET \"TC_WARANTY_PERIOD\" =TO_DATE(:sDtrWarrentyTime,'yyyy-MM-dd'), \"TC_WARRENTY\" =:sWarrentyPeriod1 WHERE ";
               //     strQry += " \"TC_CODE\" =:sReplaceTCCode3";
               //     NpgsqlCommand.Parameters.AddWithValue("sDtrWarrentyTime", sDtrWarrentyTime);
               //     NpgsqlCommand.Parameters.AddWithValue("sWarrentyPeriod1",Convert.ToInt32(sWarrentyPeriod));
               //     NpgsqlCommand.Parameters.AddWithValue("sReplaceTCCode3", Convert.ToDouble(sReplaceTCCode));
               //     objcon.ExecuteQry(strQry, NpgsqlCommand);
               // }
            }

            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public string[] SaveReplaceDetails(clsDeCommissioning objReplace)
        {

            string[] Arr = new string[3];
            try
            {
                NpgsqlCommand = new NpgsqlCommand();
                string strQry = string.Empty;
                string strTemp = string.Empty;
               // string strQry1 = string.Empty;

                //Check Failure ID is exists or not
                NpgsqlCommand.Parameters.AddWithValue("sFailureId", Convert.ToInt32(objReplace.sFailureId));
                String sId = objcon.get_value("SELECT \"DF_ID\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\" =:sFailureId", NpgsqlCommand);
                if(sId.Length == 0)
                {
                    Arr[0] = "Enter Valid Failure ID";
                    Arr[1] = "2";
                    return Arr;
                }

                string workorderid = objcon.get_value("SELECT \"WO_SLNO\" FROM \"TBLWORKORDER\" WHERE \"WO_DF_ID\"='"+ objReplace.sFailureId + "'");
                //OleDbDataReader dr = objcon.Fetch("SELECT DF_ID FROM TBLDTCFAILURE WHERE DF_ID='" + objReplace.sFailureId + "'");
                //if (!dr.Read())
                //{
                //    Arr[0] = "Enter Valid Failure ID";
                //    Arr[1] = "2";
                //    dr.Close();
                //    return Arr;
                //}
                //dr.Close();

                if (objReplace.sDecommId == "")
                {
                    NpgsqlCommand = new NpgsqlCommand();
                    //To get Failure TC ID and DTC Code
                    string sTCCode = objReplace.sdtrcode;

                   // strTemp = " SELECT CAST( \"DF_EQUIPMENT_ID\" AS TEXT) || '~' || CAST( \"DF_DTC_CODE\" AS TEXT) FROM \"TBLDTCFAILURE\", \"TBLWORKORDER\", \"TBLINDENT\" ,";
                   // strTemp += " \"TBLDTCINVOICE\" WHERE \"DF_ID\"= \"WO_DF_ID\" AND \"WO_SLNO\"=\"TI_WO_SLNO\" AND \"TI_ID\"=\"IN_TI_NO\" AND \"IN_NO\" =:sInvoiceId";
                   // NpgsqlCommand.Parameters.AddWithValue("sInvoiceId", Convert.ToInt32(objReplace.sInvoiceId));
                   // string sTCCode = objcon.get_value(strTemp, NpgsqlCommand);

                    //To get Alloted New TC ID
                   // NpgsqlCommand = new NpgsqlCommand();
                   // strTemp = "SELECT \"TD_TC_NO\" FROM \"TBLTCDRAWN\" WHERE \"TD_DF_ID\" =:sFailureId1";
                  //  NpgsqlCommand.Parameters.AddWithValue("sFailureId1", Convert.ToInt32(objReplace.sFailureId));
                   // string sReplaceTCCode = objcon.get_value(strTemp, NpgsqlCommand);

                    //if (objReplace.sOfficeCode.Length > 1)
                    //{
                    //    objReplace.sOfficeCode = objReplace.sOfficeCode.Substring(0, 2);
                    //}
                    
                    //Workflow / Approval
                    #region Workflow

                    if(objReplace.sTRReading.Trim().Length == 0)
                    {
                        strQry = "INSERT INTO \"TBLTCREPLACE\"(\"TR_ID\",\"TR_RDG\",\"TR_RI_NO\",\"TR_RI_DATE\",\"TR_DESC\",\"TR_IN_NO\",\"TR_STORE_SLNO\",\"TR_CRBY\",\"TR_OIL_QUNTY\",\"TR_DECOMM_DATE\",\"TR_MANUAL_RINO\",\"TR_COMM_DATE\",\"TR_WO_SLNO\",\"TR_OIL_TYPE\",\"TR_NO_OF_BARRELS\")";
                        strQry += "VALUES('{0}', NULL ,";
                        strQry += " (SELECT RINUMBEROFFCODE('" + objReplace.sOfficeCode + "')),TO_DATE('" + objReplace.sRIDate + "','dd/MM/yyyy'),'" + objReplace.sRemarks + "',";
                        strQry += " '" + objReplace.sInvoiceId + "','" + objReplace.sStoreId + "',";
                        strQry += " '" + objReplace.sCrby + "','" + objReplace.sOilQuantity + "',TO_DATE('" + objReplace.sDecommDate + "','dd/MM/yyyy'),'" + objReplace.sManualRINo.ToUpper() + "',TO_DATE('" + objReplace.sCommDate + "','dd/MM/yyyy'),'" + workorderid + "','" + objReplace.sOiltype + "','" + objReplace.sbarrels + "')";

                    }
                    else
                    {
                        strQry = "INSERT INTO \"TBLTCREPLACE\"(\"TR_ID\",\"TR_RDG\",\"TR_RI_NO\",\"TR_RI_DATE\",\"TR_DESC\",\"TR_IN_NO\",\"TR_STORE_SLNO\",\"TR_CRBY\",\"TR_OIL_QUNTY\",\"TR_DECOMM_DATE\",\"TR_MANUAL_RINO\",\"TR_COMM_DATE\",\"TR_WO_SLNO\",\"TR_OIL_TYPE\",\"TR_NO_OF_BARRELS\")";
                        strQry += "VALUES('{0}','" + objReplace.sTRReading + "',";
                        strQry += " (SELECT RINUMBEROFFCODE('" + objReplace.sOfficeCode + "')),TO_DATE('" + objReplace.sRIDate + "','dd/MM/yyyy'),'" + objReplace.sRemarks + "',";
                        strQry += " '" + objReplace.sInvoiceId + "','" + objReplace.sStoreId + "',";
                        strQry += " '" + objReplace.sCrby + "','" + objReplace.sOilQuantity + "',TO_DATE('" + objReplace.sDecommDate + "','dd/MM/yyyy'),'" + objReplace.sManualRINo.ToUpper() + "',TO_DATE('" + objReplace.sCommDate + "','dd/MM/yyyy'),'" + workorderid + "','" + objReplace.sOiltype + "','" + objReplace.sbarrels + "')";

                    }


                    strQry = strQry.Replace("'", "''");


                    //strQry1 = "UPDATE TBLTCMASTER SET TC_UPDATED_EVENT='REPLACE',TC_UPDATED_EVENT_ID='{0}',";
                    //strQry1 += " TC_CURRENT_LOCATION=2,TC_LOCATION_ID='" + objReplace.sOfficeCode + "' WHERE ";
                    //strQry1 += " TC_ID=(select TD_TC_NO from TBLTCDRAWN where TD_ID='" + objReplace.sInvoiceId + "')";
                    //strQry1 = strQry1.Replace("'", "''");

                    //comented
                    //strQry1 = "UPDATE \"TBLTCMASTER\" SET \"TC_UPDATED_EVENT\"='REPLACE',\"TC_UPDATED_EVENT_ID\"='{0}',";
                    //strQry1 += " \"TC_CURRENT_LOCATION\"=2,\"TC_LOCATION_ID\"='" + objReplace.sOfficeCode + "' WHERE ";
                    //strQry1 += " \"TC_CODE\" =(select \"TD_TC_NO\" from \"TBLTCDRAWN\" where \"TD_ID\" ='" + objReplace.sInvoiceId + "')";
                    //strQry1 = strQry1.Replace("'", "''");



                    // unmap failed dtr 
                    string strQry2 = "UPDATE \"TBLTRANSDTCMAPPING\" SET \"TM_LIVE_FLAG\" = '0', \"TM_UNMAP_CRON\" = NOW() ,\"TM_UNMAP_CRBY\" ='" + objReplace.sCrby + "',";
                    strQry2 += " \"TM_UNMAP_REASON\" ='FROM FAILURE ENTRY' WHERE \"TM_TC_ID\" ='" + sTCCode + "'";
                    strQry2 += " AND \"TM_LIVE_FLAG\"='1' AND \"TM_DTC_ID\" ='" + objReplace.sDTCCode + "'";

                    //string strQry2 = "UPDATE TBLTRANSDTCMAPPING SET TM_LIVE_FLAG= '0',TM_UNMAP_CRON=SYSDATE,TM_UNMAP_CRBY='" + objReplace.sCrby + "',";
                    //strQry2 += " TM_UNMAP_REASON='FROM FAILURE ENTRY',TM_MAPPING_DATE=TO_DATE('" + objReplace.sCommDate + "','dd/MM/yyyy') WHERE TM_TC_ID='" + sTCCode.Split('~').GetValue(0).ToString() + "'";
                    //strQry2 += " AND TM_LIVE_FLAG='1' AND TM_DTC_ID='" + objReplace.sDTCCode + "'";

                    strQry2 = strQry2.Replace("'", "''");

                    //query moved to invoice

                    //string strQry3 = "UPDATE \"TBLDTCMAST\" SET \"DT_TC_ID\" ='" + sReplaceTCCode + "' WHERE \"DT_CODE\" ='" + objReplace.sDTCCode + "'";

                    //strQry3 = strQry3.Replace("'", "''");

                    //string strQry4 = "INSERT INTO \"TBLTRANSDTCMAPPING\"(\"TM_ID\",\"TM_MAPPING_DATE\",\"TM_TC_ID\",\"TM_DTC_ID\",\"TM_LIVE_FLAG\",\"TM_CRBY\",\"TM_CRON\")";
                    //strQry4 += " VALUES('{1}', NOW() ,'" + sReplaceTCCode + "',";
                    //strQry4 += " '" + objReplace.sDTCCode + "','1','" + objReplace.sCrby + "', NOW())";

                    //strQry4 = strQry4.Replace("'", "''");



                    string sParam = "SELECT COALESCE(MAX(\"TR_ID\"),0)+1 FROM \"TBLTCREPLACE\"";

                    string sParam1 = "SELECT COALESCE(MAX(\"TM_ID\"),0)+1 FROM \"TBLTRANSDTCMAPPING\"";

                    clsApproval objApproval = new clsApproval();
                    objApproval.sfailid = objReplace.sFailureId;
                    objApproval.sFormName = objReplace.sFormName;
                    //objApproval.sRecordId = StrGetMaxNo;
                    objApproval.sOfficeCode = objReplace.sOfficeCode;
                    objApproval.sClientIp = objReplace.sClientIP;
                    objApproval.sCrby = objReplace.sCrby;
                    objApproval.sWFObjectId = objReplace.sWFOId;
                    objApproval.sDataReferenceId = objReplace.sDTCCode;
                    objApproval.sWFAutoId = objReplace.sWFAutoId;

                    objApproval.sQryValues = strQry + ";" + strQry2 ;
                    objApproval.sParameterValues = sParam + ";" + sParam1;
                    objApproval.sMainTable = "TBLTCREPLACE";

                    objApproval.sDescription = "Decommisioning for DTC Code " + objReplace.sDTCCode;

                    objApproval.sbfm_type = "2";

                    string presentstoreid = objcon.get_value("SELECT \"STO_SM_ID\" from \"TBLSTOREOFFCODE\" WHERE \"STO_OFF_CODE\"='" + objReplace.sOfficeCode.Substring(0, 3) + "'");

                    if (presentstoreid != objReplace.sStoreId)
                    {
                        objApproval.sStatus = "1";
                    }

                    NpgsqlCommand = new NpgsqlCommand();
                    NpgsqlCommand.Parameters.AddWithValue("sFailureId", Convert.ToInt32(objReplace.sFailureId));
                    objApproval.sRefOfficeCode = objcon.get_value("SELECT \"DF_LOC_CODE\" FROM \"TBLDTCFAILURE\",\"TBLWORKORDER\" WHERE \"DF_ID\"=\"WO_DF_ID\" AND \"DF_ID\"=:sFailureId", NpgsqlCommand);

                    string sPrimaryKey = "{0}";


                    objApproval.sColumnNames = "TR_ID,TR_RDG,TR_RI_NO,TR_RI_DATE,TR_DESC,TR_IN_NO,TR_STORE_SLNO,TR_CRBY,TR_OIL_QUNTY,TR_DECOMM_DATE,OFFICECODE,TR_MANUAL_RINO,TR_COMM_DATE,TR_WO_SLNO,TR_OIL_TYPE,TR_NO_OF_BARRELS";

                    objApproval.sColumnValues = "" + sPrimaryKey + "," + objReplace.sTRReading + "," + objReplace.sRINo + "," + objReplace.sRIDate + "," + objReplace.sRemarks.Replace(",", "") + ",";
                    objApproval.sColumnValues += "" + objReplace.sInvoiceId + "," + objReplace.sStoreId + ",";
                    objApproval.sColumnValues += "" + objReplace.sCrby + "," + objReplace.sOilQuantity + "," + objReplace.sDecommDate + "," + objReplace.sOfficeCode + "," + objReplace.sManualRINo + "," + objReplace.sCommDate + "," + workorderid + "," + objReplace.sOiltype + "," + objReplace.sbarrels + "";


                    objApproval.sTableNames = "TBLTCREPLACE";

                    //Check for Duplicate Approval
                    bool bApproveResult = objApproval.CheckDuplicateApprove(objApproval);
                    if (bApproveResult == false)
                    {
                        Arr[0] = "Selected Record Already Approved";
                        Arr[1] = "2";
                        Arr[2] = objApproval.sWFDataId;
                        return Arr;
                    }

                    if (objReplace.sActionType == "M")
                    {
                        objApproval.SaveWorkFlowData(objApproval);
                        objReplace.sWFDataId = objApproval.sWFDataId;
                    }
                    else
                    {

                        objApproval.SaveWorkFlowData(objApproval);
                        objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow();
                        objApproval.SaveWorkflowObjects(objApproval);
                    }

                    #endregion
                    Arr[0] = "Decommissioning Done Successfully";
                    Arr[1] = "0";
                    Arr[2] = objApproval.sWFDataId;
                    return Arr;
                }
                else
                {

                    //string[] strResult = new string[2];
                    string[] strArray = new string[2];
                    NpgsqlCommand cmd = new NpgsqlCommand("sp_update_replacedetails");
                    cmd.Parameters.AddWithValue("strreading", objReplace.sTRReading);
                    cmd.Parameters.AddWithValue("srino", objReplace.sRINo);
                    cmd.Parameters.AddWithValue("sridate", objReplace.sRIDate);
                    cmd.Parameters.AddWithValue("sremarks", objReplace.sRemarks);
                    cmd.Parameters.AddWithValue("sstoreid", objReplace.sStoreId);
                    cmd.Parameters.AddWithValue("srvno", objReplace.sRVNo);
                    cmd.Parameters.AddWithValue("srvdate", objReplace.sRVDate);
                    cmd.Parameters.AddWithValue("soilquantity", objReplace.sOilQuantity);
                    cmd.Parameters.AddWithValue("scommdate", objReplace.sCommDate);
                    cmd.Parameters.AddWithValue("sdecommid", objReplace.sDecommId);
                    cmd.Parameters.AddWithValue("sbarrels", objReplace.sbarrels);

                    cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                    cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                    strArray[0] = "op_id";
                    strArray[1] = "msg";
                    Arr = objcon.Execute(cmd, strArray, 2);

                    //strQry = "UPDATE TBLTCREPLACE SET TR_RDG='" + objReplace.sTRReading + "',TR_RI_NO='" + objReplace.sRINo + "',TR_RI_DATE=TO_DATE('" + objReplace.sRIDate + "','dd/MM/yyyy'),";
                    //strQry += " TR_DESC='" + objReplace.sRemarks + "',TR_STORE_SLNO='" + objReplace.sStoreId + "',TR_RV_NO='" + objReplace.sRVNo + "',";
                    //strQry += " TR_RV_DATE=TO_DATE('" + objReplace.sRVDate + "','dd/MM/yyyy'),TR_OIL_QUNTY='" + objReplace.sOilQuantity + "',TR_COMM_DATE='" + objReplace.sCommDate + "' WHERE TR_ID='" + objReplace.sDecommId + "'";
                    //objcon.Execute(strQry);

                    //Arr[0] = "Decommission Details Updated Successfully";
                    //Arr[1] = "1";
                    return Arr;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }


        public string GetRecordIdForinvoiceno()
        {
            try
            {
                string strQry = string.Empty;


                strQry = " SELECT  COALESCE(MIN(\"TR_ID\"),0)-1 FROM \"TBLTCREPLACE\"";
                string sResult = objcon.get_value(strQry);
                if (Convert.ToInt32(sResult) >= 0)
                {
                    sResult = "-1";
                }

                return sResult;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        public clsDeCommissioning GetDecommDetails(clsDeCommissioning objDecomm)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                //strQry = "SELECT TR_RDG,TR_RI_NO,TO_CHAR(TR_RI_DATE,'DD/MM/YYYY') TR_RI_DATE,TR_DESC,TR_RV_NO,TO_CHAR(TR_RV_DATE,'DD/MM/YYYY') TR_RV_DATE,";
                //strQry += " TR_STORE_SLNO,TR_OIL_QUNTY,TO_CHAR(TR_DECOMM_DATE,'DD/MM/YYYY') TR_DECOMM_DATE,TR_MANUAL_RINO from TBLTCREPLACE WHERE TR_ID='" + objDecomm.sDecommId + "'";
                //dt = objcon.getDataTable(strQry);

                NpgsqlCommand cmd = new NpgsqlCommand("sp_getdecommdetails");
                cmd.Parameters.AddWithValue("sdecommid", objDecomm.sDecommId);
                dt = objcon.FetchDataTable(cmd);

                if (dt.Rows.Count > 0)
                {
                    objDecomm.sRINo = dt.Rows[0]["TR_RI_NO"].ToString();
                    objDecomm.sRIDate = dt.Rows[0]["TR_RI_DATE"].ToString();
                    objDecomm.sRemarks = dt.Rows[0]["TR_DESC"].ToString();
                    objDecomm.sStoreId = dt.Rows[0]["TR_STORE_SLNO"].ToString();
                    objDecomm.sRVNo = dt.Rows[0]["TR_RV_NO"].ToString();
                    objDecomm.sRVDate = dt.Rows[0]["TR_RV_DATE"].ToString();
                    objDecomm.sOilQuantity = dt.Rows[0]["TR_OIL_QUNTY"].ToString();
                    objDecomm.sTRReading = dt.Rows[0]["TR_RDG"].ToString();
                    objDecomm.sDecommDate = dt.Rows[0]["TR_DECOMM_DATE"].ToString();
                    objDecomm.sManualRINo = dt.Rows[0]["TR_MANUAL_RINO"].ToString();
                }

                return objDecomm;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objDecomm;
            }
        }

        public string GetInvoiceNo(string sFailureId)
        {
            string strQry = string.Empty;
            try
            {
                NpgsqlCommand = new NpgsqlCommand();
                strQry = "SELECT \"IN_NO\" FROM \"TBLDTCFAILURE\",\"TBLWORKORDER\",\"TBLINDENT\",\"TBLDTCINVOICE\" WHERE \"DF_ID\"=\"WO_DF_ID\" ";
                strQry += " AND \"WO_SLNO\" = \"TI_WO_SLNO\" AND \"TI_ID\"=\"IN_TI_NO\" AND \"DF_ID\"=:sFailureId";
                NpgsqlCommand.Parameters.AddWithValue("sFailureId", Convert.ToInt32(sFailureId));
                return objcon.get_value(strQry, NpgsqlCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }

        public string GenerateRINo(string sOfficeCode)
        {
            try
            {
                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;
                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", sOfficeCode);
                string sRINo = objcon.get_value("SELECT COALESCE(MAX(CAST(\"TR_RI_NO\" AS INT8)),0)+1   FROM \"TBLTCREPLACE\" WHERE \"TR_RI_NO\" LIKE :sOfficeCode||'%' ", NpgsqlCommand);
                if (sRINo.Length == 1)
                {

                    //4 digit Section Code
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        sFinancialYear = System.DateTime.Now.ToString("yy");
                    }
                    else
                    {
                        sFinancialYear = System.DateTime.Now.AddYears(-1).ToString("yy");
                    }

                    sRINo = sOfficeCode + sFinancialYear + "00001";
                }
                else
                {
                    //4 digit Section Code
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                        {
                            sFinancialYear = System.DateTime.Now.ToString("yy");
                        }
                        if (sFinancialYear == sRINo.Substring(5, 2))
                        {
                            return sRINo;
                        }
                        else
                        {
                            sRINo = sOfficeCode + sFinancialYear + "00001";
                        }
                    }
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) <= 03)
                    {
                        return sRINo;
                    }


                }

                return sRINo;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }


        public void updateinvnotbltcreplace(string decommid)
        {

            string strQry = string.Empty;
            string strTemp;
            try
            {
                string workorderid = objcon.get_value("SELECT \"TR_WO_SLNO\" from \"TBLTCREPLACE\" WHERE cast(\"TR_ID\" as text)='" + decommid + "'");

                string indentid= objcon.get_value("SELECT \"TI_ID\" from \"TBLINDENT\" WHERE cast(\"TI_WO_SLNO\" as text)='" + workorderid + "'");

                string invoiceid= objcon.get_value("SELECT \"IN_NO\" from \"TBLDTCINVOICE\" WHERE cast(\"IN_TI_NO\" as text)='" + indentid + "'");

                // if invoice is done then  update the invoice number in the tcreplace  else
                // update the dtc mast's tc_code to zero bcz if invoice is done then dtc mast would have updated dtc 
                // else dtc mast will have failed dtr , so update the dtc_tc_id  with 0 
                if (invoiceid != null && invoiceid != "")
                {
                    string updateqry = "UPDATE \"TBLTCREPLACE\" set \"TR_IN_NO\"='" + invoiceid + "' WHERE cast(\"TR_WO_SLNO\" as text)='" + workorderid + "'";
                    objcon.ExecuteQry(updateqry);

                }
                else
                {
                    string failid = objcon.get_value("SELECT \"WO_DF_ID\" from \"TBLWORKORDER\" WHERE cast(\"WO_SLNO\" as text)='" + workorderid + "'");
                    string df_dtc_code = objcon.get_value("SELECT \"DF_DTC_CODE\" from \"TBLDTCFAILURE\" WHERE cast(\"DF_ID\" as text)='" + failid + "'");
                    string updateqry1 = "UPDATE \"TBLDTCMAST\" set \"DT_TC_ID\"=0 WHERE \"DT_CODE\"='"+ df_dtc_code + "'";
                        objcon.ExecuteQry(updateqry1);

                }


            }

            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }



        #region NewDTC

        public DataTable LoadCreateNewDTCDecommission(clsDeCommissioning objDecomm)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                int SubDivision = Convert.ToInt32(ConfigurationSettings.AppSettings["SubDiv_code"]);
                if (objDecomm.sOfficeCode.Length > 3)
                {
                    objDecomm.sOfficeCode = objDecomm.sOfficeCode.Substring(0, SubDivision);
                }

                //strQry = "SELECT WO_SLNO,WO_NO,TO_CHAR(WO_DATE,'DD-MON-YYYY') WO_DATE,TI_ID,TI_INDENT_NO,'NO' AS STATUS,IN_INV_NO,";
                //strQry += " TO_CHAR(IN_DATE,'DD-MON-YYYY') IN_DATE,(SELECT TD_TC_NO FROM TBLTCDRAWN WHERE TD_INV_NO=IN_NO) TD_TC_NO,IN_NO ";
                //strQry += "  FROM TBLWORKORDER,TBLINDENT,TBLDTCINVOICE WHERE WO_SLNO=TI_WO_SLNO AND WO_REPLACE_FLG='0' AND ";
                //strQry += " WO_DF_ID IS NULL AND WO_REQUEST_LOC LIKE '" + objDecomm.sOfficeCode + "%' AND TI_ID=IN_TI_NO AND IN_NO NOT IN ";
                //strQry += " (SELECT TR_IN_NO FROM TBLTCREPLACE)";

                //dt = objcon.getDataTable(strQry);

                NpgsqlCommand cmd = new NpgsqlCommand("sp_loadcreatenewdtcdecommission");
                cmd.Parameters.AddWithValue("sofficecode", objDecomm.sOfficeCode);
                dt = objcon.FetchDataTable(cmd);

                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }

        }


        #endregion

        #region WorkFlow XML

        public clsDeCommissioning GetDecommDetailsFromXML(clsDeCommissioning objDecomm)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable dt = new DataTable();

                dt = objApproval.GetDatatableFromXML(objDecomm.sWFDataId);
                if (dt.Rows.Count > 0)
                {
                    objDecomm.sRINo = dt.Rows[0]["TR_RI_NO"].ToString();
                    objDecomm.sRIDate = dt.Rows[0]["TR_RI_DATE"].ToString();
                    objDecomm.sRemarks = dt.Rows[0]["TR_DESC"].ToString().Replace("ç", ",");
                    objDecomm.sStoreId = dt.Rows[0]["TR_STORE_SLNO"].ToString();
                    //objDecomm.sRVNo = dt.Rows[0]["TR_RV_NO"].ToString();
                    //objDecomm.sRVDate = dt.Rows[0]["TR_RV_DATE"].ToString();
                    objDecomm.sOilQuantity = dt.Rows[0]["TR_OIL_QUNTY"].ToString();
                    objDecomm.sTRReading = dt.Rows[0]["TR_RDG"].ToString();
                    objDecomm.sDecommDate = dt.Rows[0]["TR_DECOMM_DATE"].ToString();
                    objDecomm.sCrby = dt.Rows[0]["TR_CRBY"].ToString();
                    objDecomm.sOfficeCode = dt.Rows[0]["OFFICECODE"].ToString();
                    objDecomm.sCommDate = dt.Rows[0]["TR_COMM_DATE"].ToString();
                    objDecomm.sbarrels = dt.Rows[0]["TR_NO_OF_BARRELS"].ToString();
                    if (dt.Columns.Contains("TR_MANUAL_RINO"))
                    {
                        objDecomm.sManualRINo = Convert.ToString(dt.Rows[0]["TR_MANUAL_RINO"]);
                    }
                   
                    if (dt.Columns.Contains("TR_OIL_TYPE"))
                    {
                        objDecomm.sOiltype = Convert.ToString(dt.Rows[0]["TR_OIL_TYPE"]);
                    }
                }
                return objDecomm;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objDecomm;
            }
        }

        #endregion
    }
}
