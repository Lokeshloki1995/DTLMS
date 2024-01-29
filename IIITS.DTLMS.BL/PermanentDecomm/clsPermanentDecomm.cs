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
    public class clsPermanentDecomm
    {

        string strFormCode = "clsPermanentDecomm";
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

        public string sIndentid { get; set; }


        // Workflow
        public string sFormName { get; set; }
        public string sClientIP { get; set; }
        public string sWFOId { get; set; }
        public string sWFDataId { get; set; }
        public string sActionType { get; set; }
        public string sWFAutoId { get; set; }

        public string sDtrWarrentyTime { get; set; }
        public string sWarrentyPeriod { get; set; }
        public string sWOSlno { get; set; }

        //CustOledbConnection objcon = new CustOledbConnection(Constants.Password);
        PGSqlConnection objcon = new PGSqlConnection(Constants.Password);

        public DataTable LoadAllDecommission(clsPermanentDecomm objDecomm)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {


                NpgsqlCommand cmd = new NpgsqlCommand("sp_loadall_permanent_decommission");
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

        public DataTable LoadCreateDecommission(clsPermanentDecomm objDecomm)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {

                NpgsqlCommand cmd = new NpgsqlCommand("sp_loadcreate_permanent_decommission");
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

        public DataTable LoadAlreadyDecomm(clsPermanentDecomm objDecomm)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("sp_loadalready_permanent_decomm");
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
        NpgsqlCommand NpgsqlCommand;
        public void updateRecord(string DtcCode)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string strQry = string.Empty;
            string strTemp;
            try
            {
                NpgsqlCommand.Parameters.AddWithValue("DtcCode1",Convert.ToInt32(DtcCode));
                strQry = "SELECT \"PEST_ID\" AS \"PEST_ID\" FROM \"TBLPERMANENTESTIMATIONDETAILS\" WHERE \"PEST_DTC_CODE\"  =:DtcCode1";
                string df_id = objcon.get_value(strQry, NpgsqlCommand);

                strTemp = "0";
                string sReplaceTCCode = strTemp;


                NpgsqlCommand.Parameters.AddWithValue(" ReplaceTCCode",Convert.ToDouble(sReplaceTCCode));
                strQry = "SELECT CASE WHEN NOW() < \"TC_WARANTY_PERIOD\" THEN 1 ELSE 0 END FROM \"TBLTCMASTER\" WHERE \"TC_CODE\" =:ReplaceTCCode";
                string sval = objcon.get_value(strQry, NpgsqlCommand);

                if (sval == "0")
                {
                    NpgsqlCommand.Parameters.AddWithValue(" ReplaceTCCode1",Convert.ToDouble(sReplaceTCCode));
                    strQry = "SELECT TO_CHAR(ADD_MONTHS(\"TM_MAPPING_DATE\",\"WARENTY_MONTH\"),'YYYY-MM-DD') WARENTY_DATE FROM (SELECT \"RSD_GUARRENTY_TYPE\" ,";
                    strQry += " (\"RSD_WARENTY_PERIOD\" * 12) WARENTY_MONTH, \"TC_CODE\", \"TM_MAPPING_DATE\" FROM \"TBLTRANSDTCMAPPING\", \"TBLREPAIRSENTDETAILS\",";
                    strQry += " \"TBLTCMASTER\" WHERE  \"TC_CODE\"=\"RSD_TC_CODE\" AND \"RSD_TC_CODE\"=\"TM_TC_ID\" AND \"TC_CODE\"=:ReplaceTCCode1 AND \"TM_LIVE_FLAG\" =1)";
                    sDtrWarrentyTime = objcon.get_value(strQry, NpgsqlCommand);

                    NpgsqlCommand.Parameters.AddWithValue(" ReplaceTCCode2",Convert.ToDouble( sReplaceTCCode));
                    strQry = "SELECT \"RSD_WARENTY_PERIOD\" FROM \"TBLTRANSDTCMAPPING\", \"TBLREPAIRSENTDETAILS\", \"TBLTCMASTER\" WHERE \"TC_CODE\" = \"RSD_TC_CODE\" AND ";
                    strQry += " \"RSD_TC_CODE\" = \"TM_TC_ID\"  AND \"TC_CODE\" =:ReplaceTCCode2 AND \"TM_LIVE_FLAG\" =1";
                    sWarrentyPeriod = objcon.get_value(strQry, NpgsqlCommand);
                }
      

                if (sval == "0")
                {
                    NpgsqlCommand.Parameters.AddWithValue(" DtrWarrentyTime", sDtrWarrentyTime);
                    NpgsqlCommand.Parameters.AddWithValue(" WarrentyPeriod",Convert.ToInt32( sWarrentyPeriod));
                    NpgsqlCommand.Parameters.AddWithValue(" ReplaceTCCode3",Convert.ToDouble( sReplaceTCCode));
                    strQry = "UPDATE \"TBLTCMASTER\" SET \"TC_WARANTY_PERIOD\" =TO_DATE(:DtrWarrentyTime,'yyyy-MM-dd'), \"TC_WARRENTY\" =:WarrentyPeriod WHERE ";
                    strQry += " \"TC_CODE\" =:ReplaceTCCode3";
                    objcon.ExecuteQry(strQry, NpgsqlCommand);
                }
            }

            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public string[] SaveReplaceDetails(clsPermanentDecomm objReplace)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string[] Arr = new string[3];
            try
            {

                string strQry = string.Empty;
                string strTemp = string.Empty;
                string strQry1 = string.Empty;

                if (objReplace.sDecommId == "")
                {

                    //To get Failure TC ID and DTC Code
                    NpgsqlCommand.Parameters.AddWithValue("FailureId",Convert.ToInt32(objReplace.sFailureId));
                    strTemp = " SELECT CAST( \"PEST_TC_CODE\" AS TEXT) || '~' || CAST( \"PEST_DTC_CODE\" AS TEXT) FROM \"TBLPERMANENTESTIMATIONDETAILS\", \"TBLPERMANENTWORKORDER\" ";
                    strTemp += " WHERE \"PEST_ID\"= \"PWO_PEF_ID\"  AND \"PEST_ID\" =:FailureId";
                    string sTCCode = objcon.get_value(strTemp, NpgsqlCommand);

                    //To get Alloted New TC ID
                    strTemp = "0";
                    string sReplaceTCCode = strTemp;


                    //Workflow / Approval
                    #region Workflow

                    if (objReplace.sTRReading.Trim().Length == 0)
                    {
                        strQry = "INSERT INTO \"TBLPERMANENTTCREPLACE\"(\"PTR_ID\",\"PTR_RDG\",\"PTR_RI_NO\",\"PTR_RI_DATE\",\"PTR_DESC\",\"PTR_STORE_SLNO\",\"PTR_CRBY\",\"PTR_OIL_QUNTY\",\"PTR_DECOMM_DATE\",\"PTR_MANUAL_RINO\",\"PTR_COMM_DATE\",\"PTR_WO_SLNO\")";
                        strQry += "VALUES('{0}', NULL ,";
                        strQry += " '" + objReplace.sRINo + "',TO_DATE('" + objReplace.sRIDate + "','dd/MM/yyyy'),'" + objReplace.sRemarks + "',";
                        strQry += " '" + objReplace.sStoreId + "',";
                        strQry += " '" + objReplace.sCrby + "','" + objReplace.sOilQuantity + "',TO_DATE('" + objReplace.sDecommDate + "','dd/MM/yyyy'),'" + objReplace.sManualRINo.ToUpper() + "',TO_DATE('" + objReplace.sCommDate + "','dd/MM/yyyy'),'" + objReplace.sWOSlno + "')";

                    }
                    else
                    {
                        strQry = "INSERT INTO \"TBLPERMANENTTCREPLACE\"(\"PTR_ID\",\"PTR_RDG\",\"PTR_RI_NO\",\"PTR_RI_DATE\",\"PTR_DESC\",\"PTR_STORE_SLNO\",\"PTR_CRBY\",\"PTR_OIL_QUNTY\",\"PTR_DECOMM_DATE\",\"PTR_MANUAL_RINO\",\"PTR_COMM_DATE\",\"PTR_WO_SLNO\")";
                        strQry += "VALUES('{0}','" + objReplace.sTRReading + "',";
                        strQry += " '" + objReplace.sRINo + "',TO_DATE('" + objReplace.sRIDate + "','dd/MM/yyyy'),'" + objReplace.sRemarks + "',";
                        strQry += " '" + objReplace.sStoreId + "',";
                        strQry += " '" + objReplace.sCrby + "','" + objReplace.sOilQuantity + "',TO_DATE('" + objReplace.sDecommDate + "','dd/MM/yyyy'),'" + objReplace.sManualRINo.ToUpper() + "',TO_DATE('" + objReplace.sCommDate + "','dd/MM/yyyy'),'" + objReplace.sWOSlno + "')";

                    }


                    strQry = strQry.Replace("'", "''");

                    string strQry3 = "UPDATE \"TBLDTCMAST\" SET \"DT_TC_ID\" ='" + sReplaceTCCode + "' WHERE \"DT_CODE\" ='" + objReplace.sDTCCode + "'";

                    strQry3 = strQry3.Replace("'", "''");

                    string strQry5 = "UPDATE \"TBLTRANSDTCMAPPING\" SET \"TM_LIVE_FLAG\"='0' WHERE \"TM_ID\"=(SELECT MAX(\"TM_ID\") FROM \"TBLTRANSDTCMAPPING\" WHERE \"TM_DTC_ID\"='" + objReplace.sDTCCode + "')";
                    strQry5 = strQry5.Replace("'", "''");

                    string strQry4 = "INSERT INTO \"TBLTRANSDTCMAPPING\"(\"TM_ID\",\"TM_MAPPING_DATE\",\"TM_TC_ID\",\"TM_DTC_ID\",\"TM_LIVE_FLAG\",\"TM_CRBY\",\"TM_CRON\")";
                    strQry4 += " VALUES('{1}', NOW() ,'" + sReplaceTCCode + "',";
                    strQry4 += " '" + objReplace.sDTCCode + "','0','" + objReplace.sCrby + "', NOW())";

                    strQry4 = strQry4.Replace("'", "''");


                   




                    string sParam = "SELECT COALESCE(MAX(\"PTR_ID\"),0)+1 FROM \"TBLPERMANENTTCREPLACE\"";

                    string sParam1 = "SELECT COALESCE(MAX(\"TM_ID\"),0)+1 FROM \"TBLTRANSDTCMAPPING\"";

                    clsApproval objApproval = new clsApproval();
                    objApproval.sFormName = objReplace.sFormName;
                    //objApproval.sRecordId = StrGetMaxNo;
                    objApproval.sOfficeCode = objReplace.sOfficeCode;
                    objApproval.sClientIp = objReplace.sClientIP;
                    objApproval.sCrby = objReplace.sCrby;
                    objApproval.sWFObjectId = objReplace.sWFOId;
                    objApproval.sDataReferenceId = objReplace.sDTCCode;
                    objApproval.sWFAutoId = objReplace.sWFAutoId;

                    objApproval.sQryValues = strQry + ";" + strQry1 + ";" + strQry3 + ";" + strQry5 + ";" + strQry4;
                    objApproval.sParameterValues = sParam + ";" + sParam1;
                    objApproval.sMainTable = "TBLPERMANENTTCREPLACE";

                    objApproval.sDescription = "PermanentDecommisioning for DTC Code " + objReplace.sDTCCode;

                    objApproval.sRefOfficeCode = objcon.get_value("SELECT \"PEST_LOC_CODE\" FROM \"TBLPERMANENTESTIMATIONDETAILS\",\"TBLPERMANENTWORKORDER\" WHERE \"PEST_ID\"=\"PWO_PEF_ID\" AND \"PWO_SLNO\"='" + objReplace.sWOSlno + "'");

                    string sPrimaryKey = "{0}";


                    objApproval.sColumnNames = "PTR_ID,PTR_RDG,PTR_RI_NO,PTR_RI_DATE,PTR_DESC,PTR_STORE_SLNO,PTR_CRBY,PTR_OIL_QUNTY,PTR_DECOMM_DATE,OFFICECODE,PTR_MANUAL_RINO,PTR_COMM_DATE,PTR_WO_SLNO";

                    objApproval.sColumnValues = "" + sPrimaryKey + "," + objReplace.sTRReading + "," + objReplace.sRINo + "," + objReplace.sRIDate + "," + objReplace.sRemarks.Replace(",", "") + ",";
                    objApproval.sColumnValues += "" + objReplace.sStoreId + ",";
                    objApproval.sColumnValues += "" + objReplace.sCrby + "," + objReplace.sOilQuantity + "," + objReplace.sDecommDate + "," + objReplace.sOfficeCode + "," + objReplace.sManualRINo + "," + objReplace.sCommDate + "," + objReplace.sWOSlno + " ";


                    objApproval.sTableNames = "TBLPERMANENTTCREPLACE";

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
                    NpgsqlCommand cmd = new NpgsqlCommand("sp_update_replace_permanent_details");
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
                    cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                    cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                    strArray[0] = "op_id";
                    strArray[1] = "msg";
                    Arr = objcon.Execute(cmd, strArray, 2);

                
                    return Arr;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }


        public clsPermanentDecomm GetDecommDetails(clsPermanentDecomm objDecomm)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                //strQry = "SELECT TR_RDG,TR_RI_NO,TO_CHAR(TR_RI_DATE,'DD/MM/YYYY') TR_RI_DATE,TR_DESC,TR_RV_NO,TO_CHAR(TR_RV_DATE,'DD/MM/YYYY') TR_RV_DATE,";
                //strQry += " TR_STORE_SLNO,TR_OIL_QUNTY,TO_CHAR(TR_DECOMM_DATE,'DD/MM/YYYY') TR_DECOMM_DATE,TR_MANUAL_RINO from TBLTCREPLACE WHERE TR_ID='" + objDecomm.sDecommId + "'";
                //dt = objcon.getDataTable(strQry);

                NpgsqlCommand cmd = new NpgsqlCommand("sp_get_permanent_decommdetails");
                cmd.Parameters.AddWithValue("sdecommid", objDecomm.sDecommId);
                dt = objcon.FetchDataTable(cmd);

                if (dt.Rows.Count > 0)
                {
                    objDecomm.sRINo = dt.Rows[0]["PTR_RI_NO"].ToString();
                    objDecomm.sRIDate = dt.Rows[0]["PTR_RI_DATE"].ToString();
                    objDecomm.sRemarks = dt.Rows[0]["PTR_DESC"].ToString();
                    objDecomm.sStoreId = dt.Rows[0]["PTR_STORE_SLNO"].ToString();
                    objDecomm.sRVNo = dt.Rows[0]["PTR_RV_NO"].ToString();
                    objDecomm.sRVDate = dt.Rows[0]["PTR_RV_DATE"].ToString();
                    objDecomm.sOilQuantity = dt.Rows[0]["PTR_OIL_QUNTY"].ToString();
                    objDecomm.sTRReading = dt.Rows[0]["PTR_RDG"].ToString();
                    objDecomm.sDecommDate = dt.Rows[0]["PTR_DECOMM_DATE"].ToString();
                    objDecomm.sManualRINo = dt.Rows[0]["PTR_MANUAL_RINO"].ToString(); 
                     objDecomm.sCommDate = dt.Rows[0]["ptr_comm_date"].ToString();
                }

                return objDecomm;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objDecomm;
            }
        }

        //public string GetInvoiceNo(string sFailureId)
        //{
        //    string strQry = string.Empty;
        //    try
        //    {
        //        strQry = "SELECT \"IN_NO\" FROM \"TBLDTCFAILURE\",\"TBLWORKORDER\",\"TBLINDENT\",\"TBLDTCINVOICE\" WHERE \"DF_ID\"=\"WO_DF_ID\" ";
        //        strQry += " AND \"WO_SLNO\" = \"TI_WO_SLNO\" AND \"TI_ID\"=\"IN_TI_NO\" AND \"DF_ID\"='" + sFailureId + "'";
        //        return objcon.get_value(strQry);
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetInvoiceNo");
        //        return "";
        //    }
        //}

        public string GenerateRINo(string sOfficeCode)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;
                NpgsqlCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                string sRINo = objcon.get_value("SELECT COALESCE(MAX(CAST(\"PTR_RI_NO\" AS INT8)),0)+1   FROM \"TBLPERMANENTTCREPLACE\" WHERE \"PTR_RI_NO\" LIKE :OfficeCode||'%'", NpgsqlCommand);
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
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) < 03)
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



        #region NewDTC

        public DataTable LoadCreateNewDTCDecommission(clsPermanentDecomm objDecomm)
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

        public clsPermanentDecomm GetDecommDetailsFromXML(clsPermanentDecomm objDecomm)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable dt = new DataTable();

                dt = objApproval.GetDatatableFromXML(objDecomm.sWFDataId);
                if (dt.Rows.Count > 0)
                {
                    objDecomm.sRINo = dt.Rows[0]["PTR_RI_NO"].ToString();
                    objDecomm.sRIDate = dt.Rows[0]["PTR_RI_DATE"].ToString();
                    objDecomm.sRemarks = dt.Rows[0]["PTR_DESC"].ToString().Replace("ç", ",");
                    objDecomm.sStoreId = dt.Rows[0]["PTR_STORE_SLNO"].ToString();
                    //objDecomm.sRVNo = dt.Rows[0]["TR_RV_NO"].ToString();
                    //objDecomm.sRVDate = dt.Rows[0]["TR_RV_DATE"].ToString();
                    objDecomm.sOilQuantity = dt.Rows[0]["PTR_OIL_QUNTY"].ToString();
                    objDecomm.sTRReading = dt.Rows[0]["PTR_RDG"].ToString();
                    objDecomm.sDecommDate = dt.Rows[0]["PTR_DECOMM_DATE"].ToString();
                    objDecomm.sCrby = dt.Rows[0]["PTR_CRBY"].ToString();
                    objDecomm.sOfficeCode = dt.Rows[0]["OFFICECODE"].ToString();
                    objDecomm.sCommDate = dt.Rows[0]["PTR_COMM_DATE"].ToString();
                    if (dt.Columns.Contains("PTR_MANUAL_RINO"))
                    {
                        objDecomm.sManualRINo = Convert.ToString(dt.Rows[0]["PTR_MANUAL_RINO"]);
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


        public string GetIndentId(clsPermanentDecomm objindent)
        {
            string Indentid = string.Empty;
            try
            {
               // Indentid = objcon.get_value(" SELECT \"PTI_ID\" FROM \"TBLPERMANENTINDENT\" INNER JOIN \"TBLPERMANENTWORKORDER\" ON \"PWO_SLNO\"=\"PTI_WO_SLNO\" INNER JOIN \"TBLPERMANENTESTIMATIONDETAILS\" ON \"PEST_ID\"=\"PWO_PEF_ID\" WHERE \"PEST_ID\"='" + objindent.sFailureId + "'");
                Indentid = objcon.get_value("SELECT \"PWO_SLNO\" FROM  \"TBLPERMANENTWORKORDER\" INNER JOIN \"TBLPERMANENTESTIMATIONDETAILS\" ON \"PEST_ID\"=\"PWO_PEF_ID\" WHERE \"PEST_ID\"='" + objindent.sFailureId + "'");
                return Indentid;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Indentid;
            }
        }

        #endregion
    }
}