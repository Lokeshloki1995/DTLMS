using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using IIITS.PGSQL.DAL;
using NpgsqlTypes;
using Npgsql;

namespace IIITS.DTLMS.BL
{
   public class clsEnhancement
    {
        //CustOledbConnection objcon = new CustOledbConnection(Constants.Password);
        PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
        public string sDtcId { get; set; }
        public string sDtcCode { get; set; }
        public string sDtcName { get; set; }
        public string sServicedate { get; set; }
        public string sLoadKw { get; set; }
        public string sLoadHp { get; set; }
        public string sCommissionDate { get; set; }
        public string sCapacity { get; set; }
        public string sLocation { get; set; }
        public string sTcSlno { get; set; }
        public string sTcMake { get; set; }
        public string sEnhancementDate { get; set; }
        public string sReason { get; set; }
        public string sDtcReadings { get; set; }
        public string sEnhancementId { get; set; }
        public string sTcCode { get; set; }
        public string sCrby { get; set; }
        public string sOfficeCode { get; set; }
        public string sEnhancedCapacity { get; set; }
        public string sTCId { get; set; }
        public string sDtrcommdate { get; set; }
        public string sFilePath { get; set; }

        // Workflow
        public string sFormName { get; set; }
        public string sClientIP { get; set; }
        public string sWFDataId { get; set; }
        public string sActionType { get; set; }
        public DataTable dtDocuments { get; set; }

        string strFormCode = "clsEnhancement";

        NpgsqlCommand NpgsqlCommand;
        public string[] SaveEnhancementDetails(clsEnhancement  objEnhancement)
        {

            string[] Arr = new string[2];
            try
            {
                NpgsqlCommand = new NpgsqlCommand();
                OleDbDataReader dr;
                string strQry = string.Empty;
                DataTable dt = new DataTable();
                StringBuilder sbQuery = new StringBuilder();
                if (objEnhancement.sEnhancementId == "0" || objEnhancement.sEnhancementId == "")
                {
                    NpgsqlCommand.Parameters.AddWithValue("sDtcCode", objEnhancement.sDtcCode);
                    string sCode = objcon.get_value("SELECT \"DT_CODE\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\" =:sDtcCode", NpgsqlCommand);
                    if(sCode.Length == 0)
                    {
                        Arr[0] = "Enter Valid DTC Code";
                        Arr[1] = "2";
                        return Arr;
                    }

                    NpgsqlCommand = new NpgsqlCommand();
                    NpgsqlCommand.Parameters.AddWithValue("sDtcCode1", objEnhancement.sDtcCode);
                    string sFCode = objcon.get_value("SELECT \"DF_DTC_CODE\" FROM \"TBLDTCFAILURE\" WHERE \"DF_DTC_CODE\" =:sDtcCode1 AND \"DF_REPLACE_FLAG\" =0", NpgsqlCommand);
                    if (sFCode.Length > 0)
                    {
                        Arr[0] = "Already Declared Failure or Enhancement for Selected DTC Code " + objEnhancement.sDtcCode;
                        Arr[1] = "2";
                        return Arr;
                    }


                    //dr = objcon.Fetch("SELECT DT_CODE FROM TBLDTCMAST WHERE DT_CODE='" + objEnhancement.sDtcCode + "'");
                    //if (!dr.Read())
                    //{
                    //    dr.Close();
                    //    Arr[0] = "Enter Valid DTC Code";
                    //    Arr[1] = "2";
                    //    return Arr;
                    //}
                    //dr.Close();

                    //Check Already Failure Entry Done but Decomm Pending
                    //dr = objcon.Fetch("SELECT DF_DTC_CODE FROM TBLDTCFAILURE WHERE DF_DTC_CODE='" + objEnhancement.sDtcCode + "' AND DF_REPLACE_FLAG=0");
                    //if (dr.Read())
                    //{
                    //    dr.Close();
                    //    Arr[0] = "Already Declared Failure or Enhancement for Selected DTC Code " + objEnhancement.sDtcCode;
                    //    Arr[1] = "2";
                    //    return Arr;
                    //}
                    //dr.Close();

                  

                    objEnhancement.sEnhancementId = objcon.Get_max_no("DF_ID", "TBLDTCFAILURE").ToString();
                    NpgsqlCommand = new NpgsqlCommand();
                    NpgsqlCommand.Parameters.AddWithValue("sDtcCode2", objEnhancement.sDtcCode);

                    string sOfficeCode = objcon.get_value("SELECT MAX(\"DT_OM_SLNO\") FROM \"TBLDTCMAST\" WHERE \"DT_CODE\" =:sDtcCode2", NpgsqlCommand);

                    if (objEnhancement.sEnhancementDate == "")
                    {
                        objEnhancement.sEnhancementDate = System.DateTime.Now.ToString("dd/MM/yyyy");
                    }

                    //Workflow / Approval
                    #region WorkFlow

                    clsApproval objApproval = new clsApproval();
                    DataTable dtDoc = new DataTable();
                    dtDoc = objEnhancement.dtDocuments;

                    if (objEnhancement.sDtcReadings.Length == 0)
                    {
                        strQry = "INSERT INTO \"TBLDTCFAILURE\"(\"DF_ID\",\"DF_DTC_CODE\",\"DF_EQUIPMENT_ID\",\"DF_REASON\",\"DF_DATE\",\"DF_CRBY\",\"DF_CRON\",\"DF_KWH_READING\",\"DF_STATUS_FLAG\",\"DF_LOC_CODE\",\"DF_ENHANCE_CAPACITY\",\"DF_DTR_COMMISSION_DATE\",\"DF_FILE_PATH\",\"DF_ACC_HEAD\")";
                        strQry += "VALUES('{0}','" + objEnhancement.sDtcCode + "','" + objEnhancement.sTcCode + "','" + objEnhancement.sReason.Replace(",", "") + "',";
                        strQry += " TO_DATE('" + objEnhancement.sEnhancementDate + "','dd/MM/yyyy'),'" + objEnhancement.sCrby + "',NOW(),";
                        strQry += " NULL,'2','" + sOfficeCode + "','" + objEnhancement.sEnhancedCapacity + "',TO_DATE('" + objEnhancement.sDtrcommdate + "','dd/MM/yyyy'),'" + Convert.ToString(dtDoc.Rows[0]["PATH"]) + "','14.1697')";

                    }
                    else
                    {
                        strQry = "INSERT INTO \"TBLDTCFAILURE\"(\"DF_ID\",\"DF_DTC_CODE\",\"DF_EQUIPMENT_ID\",\"DF_REASON\",\"DF_DATE\",\"DF_CRBY\",\"DF_CRON\",\"DF_KWH_READING\",\"DF_STATUS_FLAG\",\"DF_LOC_CODE\",\"DF_ENHANCE_CAPACITY\",\"DF_DTR_COMMISSION_DATE\",\"DF_FILE_PATH\",\"DF_ACC_HEAD\")";
                        strQry += "VALUES('{0}','" + objEnhancement.sDtcCode + "','" + objEnhancement.sTcCode + "','" + objEnhancement.sReason.Replace(",", "") + "',";
                        strQry += " TO_DATE('" + objEnhancement.sEnhancementDate + "','dd/MM/yyyy'),'" + objEnhancement.sCrby + "',NOW(),";
                        strQry += " '" + objEnhancement.sDtcReadings + "','2','" + sOfficeCode + "','" + objEnhancement.sEnhancedCapacity + "',TO_DATE('" + objEnhancement.sDtrcommdate + "','dd/MM/yyyy'),'" + Convert.ToString(dtDoc.Rows[0]["PATH"]) + "','14.1697')";
                    }

                    sbQuery.Append(strQry);
                    sbQuery.Append(";");

                    string sParam = "SELECT COALESCE(MAX(\"DF_ID\"),0)+1 FROM \"TBLDTCFAILURE\"";

                    

                    strQry = "INSERT INTO \"TBLTEMPDOCS\" (\"TD_ID\" ,\"TD_REFERENCE_ID\", \"TD_FAIL_TYPE\" , \"TD_DOC_NAME\", \"TD_INITIALDOC_PATH\" ) ";
                    strQry += " VALUES ((SELECT COALESCE(MAX(\"TD_ID\"),0)+1 FROM \"TBLTEMPDOCS\"), '{0}', 'ENHANCEMENT', ";
                    strQry += " '" + Convert.ToString(dtDoc.Rows[0]["NAME"]) + "', '" + Convert.ToString(dtDoc.Rows[0]["PATH"]) + "') ";

                    sbQuery.Append(strQry);
                    sbQuery.Append(";");
                    sbQuery = sbQuery.Replace("'", "''");

                    if (objEnhancement.sActionType == null )
                    {

                        bool bResult = objApproval.CheckAlreadyExistEntry(objEnhancement.sDtcCode, "10");
                        if (bResult == true)
                        {
                            Arr[0] = "Capacity Enhancement Already done for DTC Code " + objEnhancement.sDtcCode + ", Waiting for Approval";
                            Arr[1] = "2";
                            return Arr;
                        }

                        bResult = objApproval.CheckAlreadyExistEntry(objEnhancement.sDtcCode, "9");
                        if (bResult == true)
                        {
                            Arr[0] = "Failure Declare Already done for DTC Code " + objEnhancement.sDtcCode + ", Waiting for Approval";
                            Arr[1] = "2";
                            return Arr;
                        }

                    }


                    objApproval.sFormName = objEnhancement.sFormName;
                    //objApproval.sRecordId = objEnhancement.sEnhancementId;
                    objApproval.sOfficeCode = objEnhancement.sOfficeCode;
                    objApproval.sClientIp = objEnhancement.sClientIP;
                    objApproval.sCrby = objEnhancement.sCrby;

                    objApproval.sQryValues = sbQuery.ToString();
                    objApproval.sParameterValues = sParam;
                    objApproval.sMainTable = "TBLDTCFAILURE";
                    objApproval.sRefOfficeCode = objEnhancement.sOfficeCode;
                    objApproval.sDataReferenceId = objEnhancement.sDtcCode;
                    objApproval.sbfm_type = "1";

                    objApproval.sDescription = "Capacity Enhancement For DTC Code " + objEnhancement.sDtcCode;

                    string sPrimaryKey = "{0}";


                    objApproval.sColumnNames = "DF_ID,DF_DTC_CODE,DF_EQUIPMENT_ID,DF_REASON,DF_DATE,DF_CRBY,DF_CRON,DF_KWH_READING,DF_STATUS_FLAG,DF_LOC_CODE,DF_ENHANCE_CAPACITY,DTR_COMISSION_DATE,NAME,PATH,DF_ACC_HEAD";

                    objApproval.sColumnValues = "" + sPrimaryKey + "," + objEnhancement.sDtcCode + "," + objEnhancement.sTcCode + "," + objEnhancement.sReason + ",";
                    objApproval.sColumnValues += "" + objEnhancement.sEnhancementDate + "," + objEnhancement.sCrby + ",NOW()," + objEnhancement.sDtcReadings + ",2," + sOfficeCode + ",";
                    objApproval.sColumnValues += "" + objEnhancement.sEnhancedCapacity + "," + objEnhancement.sDtrcommdate + ", " + Convert.ToString(dtDoc.Rows[0]["NAME"]) + ", " + Convert.ToString(dtDoc.Rows[0]["PATH"]) + ",14.1697";


                    objApproval.sTableNames = "TBLDTCFAILURE";


                    //Check for Duplicate Approval
                    bool bApproveResult = objApproval.CheckDuplicateApprove(objApproval);
                    if (bApproveResult == false)
                    {
                        Arr[0] = "Selected Record Already Approved";
                        Arr[1] = "2";
                        return Arr;
                    }

                    if (objEnhancement.sActionType == "M")
                    {
                        objApproval.SaveWorkFlowData(objApproval);
                        objEnhancement.sWFDataId = objApproval.sWFDataId;
                    }
                    else
                    {

                        objApproval.SaveWorkFlowData(objApproval);
                        objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow();
                        objApproval.SaveWorkflowObjects(objApproval);
                    }

                    #endregion
                    Arr[0] = "DTC Enhancement Declared Successfully";
                    Arr[1] = "0";
                    return Arr;
                }
                else
                {
                                       
                    string[] strArray = new string[2];
                    NpgsqlCommand cmd = new NpgsqlCommand("update_enhancementdetails");
                    cmd.Parameters.AddWithValue("sdtccode", objEnhancement.sDtcCode);
                    cmd.Parameters.AddWithValue("senhancementdate", objEnhancement.sEnhancementDate);
                    cmd.Parameters.AddWithValue("sreason", objEnhancement.sReason);
                    cmd.Parameters.AddWithValue("sdtcreadings", objEnhancement.sDtcReadings);
                    cmd.Parameters.AddWithValue("senhancementid", objEnhancement.sEnhancementId);
                    cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                    cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                    strArray[0] = "op_id";
                    strArray[1] = "msg";
                    Arr = objcon.Execute(cmd, strArray, 2);

                    //string sCode = objcon.get_value("SELECT \"DT_CODE\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\" ='" + objEnhancement.sDtcCode + "'");
                    //if (sCode.Length == 0)
                    //{
                    //    Arr[0] = "Enter Valid DTC Code";
                    //    Arr[1] = "2";
                    //    return Arr;
                    //}

                    //dr = objcon.Fetch("SELECT DT_CODE FROM TBLDTCMAST WHERE DT_CODE='" + objEnhancement.sDtcCode + "'");
                    //if (!dr.Read())
                    //{
                    //    dr.Close();
                    //    Arr[0] = "Enter Valid DTC Code";
                    //    Arr[1] = "2";
                    //    return Arr;
                    //}
                    //dr.Close();

                    //strQry = "UPDATE TBLDTCFAILURE SET DF_DATE=TO_DATE('" + objEnhancement.sEnhancementDate + "','dd/MM/yyyy'), DF_REASON='" + objEnhancement.sReason + "',";
                    //strQry += " DF_KWH_READING='" + objEnhancement.sDtcReadings + "' WHERE DF_ID='" + objEnhancement.sEnhancementId + "'";
                    //objcon.Execute(strQry);
                                       

                    //Arr[0] = "DTC Enhancement Updated Successfully";
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

        public string GetTCCapacity (string sDtc_Code)
        {
            string sCap = string.Empty;
            try
            {
                NpgsqlCommand = new NpgsqlCommand();
                string Qry = "SELECT \"TC_CAPACITY\" FROM \"TBLTCMASTER\",\"TBLDTCMAST\" WHERE \"DT_TC_ID\"=\"TC_CODE\" AND \"DT_ID\"=:sDtc_Code ";
                NpgsqlCommand.Parameters.AddWithValue("sDtc_Code", Convert.ToInt32(sDtc_Code));
                return sCap = objcon.get_value(Qry, NpgsqlCommand);
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return sCap;
            }
        }

        public DataTable LoadAllDTCEnhancement(clsEnhancement objEnhancement)
        {
            DataTable dtDetails = new DataTable();
            try
            {
                //string strQry = "SELECT DT_ID,TO_CHAR(TC_CODE) TC_CODE,DT_CODE,DT_NAME, TC_SLNO, (SELECT TM_NAME FROM TBLTRANSMAKES WHERE ";
                //strQry += " TM_ID = TC_MAKE_ID) TM_NAME, TO_CHAR(TC_CAPACITY) TC_CAPACITY,DF_ID, 'YES' AS STATUS,DT_PROJECTTYPE ";
                //strQry += " FROM TBLDTCMAST,TBLTCMASTER,TBLDTCFAILURE WHERE DF_EQUIPMENT_ID = TC_CODE  ";
                //strQry += "  AND DF_DTC_CODE = DT_CODE AND DF_REPLACE_FLAG = 0 AND DF_STATUS_FLAG='2'";
                //strQry += " AND DF_LOC_CODE LIKE '" + objEnhancement.sOfficeCode + "%'";
                //strQry += " UNION ALL";
                //strQry += " SELECT DT_ID, TO_CHAR(TC_CODE) TC_CODE,DT_CODE,DT_NAME, TC_SLNO, (SELECT TM_NAME FROM TBLTRANSMAKES WHERE ";
                //strQry += " TM_ID = TC_MAKE_ID) TM_NAME, TO_CHAR(TC_CAPACITY) TC_CAPACITY, 0 AS DF_ID,'NO' AS STATUS,DT_PROJECTTYPE  FROM TBLDTCMAST,TBLTCMASTER";
                //strQry += " WHERE DT_TC_ID = TC_CODE ";
                //strQry += " AND DT_CODE NOT IN (SELECT DF_DTC_CODE FROM TBLDTCFAILURE WHERE  DF_REPLACE_FLAG = 0 ) ";
                //strQry += " AND DT_OM_SLNO LIKE '" + objEnhancement.sOfficeCode + "%'";
                //dtDetails = objcon.getDataTable(strQry);
                //return dtDetails;

                NpgsqlCommand cmd = new NpgsqlCommand("sp_loadalldtcenhancement");
                cmd.Parameters.AddWithValue("sofficecode", Convert.ToString(objEnhancement.sOfficeCode));
                dtDetails = objcon.FetchDataTable(cmd);
                return dtDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtDetails;

            }

        }

        public DataTable LoadAlreadyEnhanced(clsEnhancement objEnhance)
        {
            DataTable dtDetails = new DataTable();
            try
            {
                //string strQry = "SELECT DT_ID,TO_CHAR(TC_CODE) TC_CODE,DT_CODE,DF_ID,DT_NAME, TC_SLNO, TM_NAME, TO_CHAR(TC_CAPACITY) TC_CAPACITY,";
                //strQry += " DF_ID,'YES' AS STATUS,DT_PROJECTTYPE  FROM TBLDTCMAST,TBLTCMASTER,";
                //strQry += "TBLTRANSMAKES,TBLDTCFAILURE WHERE DF_EQUIPMENT_ID = TC_CODE AND ";
                //strQry += "TM_ID = TC_MAKE_ID AND DF_DTC_CODE = DT_CODE AND DF_REPLACE_FLAG = 0 AND DF_STATUS_FLAG='2'";
                //strQry += " AND DF_LOC_CODE LIKE '" + objEnhance.sOfficeCode + "%'";
                //dtDetails = objcon.getDataTable(strQry);
                //return dtDetails;

                NpgsqlCommand cmd = new NpgsqlCommand("sp_loadalreadyenhanced");
                cmd.Parameters.AddWithValue("sofficecode", Convert.ToString(objEnhance.sOfficeCode));
                dtDetails = objcon.FetchDataTable(cmd);
                return dtDetails;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtDetails;

            }

        }

        public object GetEnhancementDetails(clsEnhancement objEnhancement)
        {

            DataTable dtDetails = new DataTable();
            try
            {
                if (objEnhancement.sEnhancementId  != "0")
                {

                    //string strQry = "SELECT DF_ID,DT_ID, DF_DTC_CODE,DT_NAME,TO_CHAR(DT_LAST_SERVICE_DATE,'DD/MM/YYYY') DT_LAST_SERVICE_DATE,";
                    //strQry += "TO_CHAR(DT_TOTAL_CON_KW)DT_TOTAL_CON_KW,TO_CHAR(DT_TOTAL_CON_HP)DT_TOTAL_CON_HP,TO_CHAR(DT_TRANS_COMMISION_DATE,'DD/MM/YYYY') DT_TRANS_COMMISION_DATE,";
                    //strQry += "(SELECT DISTINCT OM_NAME FROM TBLOMSECMAST WHERE OM_CODE=DT_OM_SLNO  ";
                    //strQry += " )AS TC_LOCATION_ID ,TC_SLNO,(SELECT TM_NAME from TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID) TC_MAKE_ID,TC_CODE,TC_ID, ";
                    //strQry += " TO_CHAR(TC_CAPACITY) TC_CAPACITY,TO_CHAR(DF_DATE,'DD/MM/YYYY') DF_DATE,DF_REASON,TO_CHAR(DF_KWH_READING)DF_KWH_READING,";
                    //strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE DF_CRBY=US_ID) US_FULL_NAME,DF_ENHANCE_CAPACITY,DF_LOC_CODE,DF_DTR_COMMISSION_DATE  ";
                    //strQry += " FROM TBLDTCMAST,TBLTCMASTER,TBLDTCFAILURE WHERE DF_STATUS_FLAG='2' AND DF_DTC_CODE=DT_CODE AND DF_EQUIPMENT_ID=TC_CODE";
                    //strQry += " AND DF_ID='" + objEnhancement.sEnhancementId + "'";
                    //dtDetails = objcon.getDataTable(strQry);

                    NpgsqlCommand cmd = new NpgsqlCommand("sp_getenhancementdetails");
                    cmd.Parameters.AddWithValue("senhancementid", Convert.ToString(objEnhancement.sEnhancementId));
                    dtDetails = objcon.FetchDataTable(cmd);

                    if (dtDetails.Rows.Count > 0)
                    {
                        objEnhancement.sDtcId = dtDetails.Rows[0]["DT_ID"].ToString();
                        objEnhancement.sDtcCode = dtDetails.Rows[0]["DF_DTC_CODE"].ToString();
                        objEnhancement.sDtcName = dtDetails.Rows[0]["DT_NAME"].ToString();
                        objEnhancement.sServicedate  = dtDetails.Rows[0]["DT_LAST_SERVICE_DATE"].ToString();
                        objEnhancement.sLoadKw  = dtDetails.Rows[0]["DT_TOTAL_CON_KW"].ToString();
                        objEnhancement.sLoadHp = dtDetails.Rows[0]["DT_TOTAL_CON_HP"].ToString();
                        objEnhancement.sCommissionDate = dtDetails.Rows[0]["DT_TRANS_COMMISION_DATE"].ToString();
                        objEnhancement.sCapacity = dtDetails.Rows[0]["TC_CAPACITY"].ToString();
                        objEnhancement.sLocation = dtDetails.Rows[0]["TC_LOCATION_ID"].ToString();
                        objEnhancement.sTcSlno = dtDetails.Rows[0]["TC_SLNO"].ToString();
                        objEnhancement.sTcMake = dtDetails.Rows[0]["TC_MAKE_ID"].ToString();
                        objEnhancement.sEnhancementDate = dtDetails.Rows[0]["DF_DATE"].ToString();
                        objEnhancement.sReason = dtDetails.Rows[0]["DF_REASON"].ToString();
                        objEnhancement.sDtcReadings = dtDetails.Rows[0]["DF_KWH_READING"].ToString();
                        objEnhancement.sTcCode = dtDetails.Rows[0]["TC_CODE"].ToString();
                        objEnhancement.sEnhancementId = dtDetails.Rows[0]["DF_ID"].ToString();
                        objEnhancement.sCrby = dtDetails.Rows[0]["US_FULL_NAME"].ToString();
                        objEnhancement.sOfficeCode = dtDetails.Rows[0]["DF_LOC_CODE"].ToString();
                        objEnhancement.sEnhancedCapacity = dtDetails.Rows[0]["DF_ENHANCE_CAPACITY"].ToString();
                        //objEnhancement.sTCId = dtDetails.Rows[0]["TC_ID"].ToString();
                        objEnhancement.sDtrcommdate = dtDetails.Rows[0]["DF_DTR_COMMISSION_DATE"].ToString();
                    }

                    return objEnhancement;
                }

                else
                {
                    //String strQry = "SELECT  DT_ID,DT_NAME,DT_CODE,TO_CHAR(DT_LAST_SERVICE_DATE,'DD/MM/YYYY') DT_LAST_SERVICE_DATE,";
                    //strQry += "TO_CHAR(DT_TOTAL_CON_KW)DT_TOTAL_CON_KW,TO_CHAR(DT_TOTAL_CON_HP)DT_TOTAL_CON_HP,TO_CHAR(DT_TRANS_COMMISION_DATE,'DD/MM/YYYY') DT_TRANS_COMMISION_DATE,";
                    //strQry += " (SELECT OM_NAME FROM TBLOMSECMAST WHERE OM_CODE=DT_OM_SLNO ) AS TC_LOCATION_ID ,TC_ID,";
                    //strQry += " TC_SLNO,(SELECT TM_NAME from TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID) TC_MAKE_ID,TC_CODE,TO_CHAR(TC_CAPACITY) TC_CAPACITY FROM";
                    //strQry+= " TBLDTCMAST,TBLTCMASTER WHERE  DT_TC_ID=TC_CODE AND DT_ID='" + objEnhancement.sDtcId + "'";
                    //dtDetails = objcon.getDataTable(strQry);

                    NpgsqlCommand cmd = new NpgsqlCommand("sp_searchenhancementdetails");
                    cmd.Parameters.AddWithValue("senhancementid", Convert.ToString(objEnhancement.sDtcId));
                    dtDetails = objcon.FetchDataTable(cmd);

                    if (dtDetails.Rows.Count > 0)
                    {
                        objEnhancement.sDtcId = dtDetails.Rows[0]["DT_ID"].ToString();
                        objEnhancement.sDtcCode = dtDetails.Rows[0]["DT_CODE"].ToString();
                        objEnhancement.sDtcName = dtDetails.Rows[0]["DT_NAME"].ToString();
                        objEnhancement.sServicedate = dtDetails.Rows[0]["DT_LAST_SERVICE_DATE"].ToString();
                        objEnhancement.sLoadKw = dtDetails.Rows[0]["DT_TOTAL_CON_KW"].ToString();
                        objEnhancement.sLoadHp = dtDetails.Rows[0]["DT_TOTAL_CON_HP"].ToString();
                        objEnhancement.sCommissionDate = dtDetails.Rows[0]["DT_TRANS_COMMISION_DATE"].ToString();
                        objEnhancement.sCapacity = dtDetails.Rows[0]["TC_CAPACITY"].ToString();
                        objEnhancement.sLocation = dtDetails.Rows[0]["TC_LOCATION_ID"].ToString();
                        objEnhancement.sTcSlno = dtDetails.Rows[0]["TC_SLNO"].ToString();
                        objEnhancement.sTcMake = dtDetails.Rows[0]["TC_MAKE_ID"].ToString();
                        objEnhancement.sTcCode = dtDetails.Rows[0]["TC_CODE"].ToString();
                        objEnhancement.sTCId = dtDetails.Rows[0]["TC_ID"].ToString();

                        NpgsqlCommand = new NpgsqlCommand();
                        string strQry = "SELECT TO_CHAR(\"TM_MAPPING_DATE\",'DD/MM/YYYY') AS DTR_COMM_DATE FROM \"TBLTRANSDTCMAPPING\" WHERE \"TM_DTC_ID\" =:sDtcCode3 AND \"TM_TC_ID\" =:sTcCode3 AND \"TM_LIVE_FLAG\" =1";
                        NpgsqlCommand.Parameters.AddWithValue("sDtcCode3", objEnhancement.sDtcCode);
                        NpgsqlCommand.Parameters.AddWithValue("sTcCode3",Convert.ToDouble(objEnhancement.sTcCode));
                        string sdtrCommDate = objcon.get_value(strQry, NpgsqlCommand);
                        objEnhancement.sDtrcommdate = sdtrCommDate;

                    }

                    return objEnhancement;
                }


            }

            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objEnhancement;
            }

        }

        public bool ValidateEnhancementUpdate(string sEnhanceId)
        {
            try
            {
                NpgsqlCommand = new NpgsqlCommand();
                DataTable dt = new DataTable();
                string strQry = "select \"WO_DF_ID\" from \"TBLWORKORDER\",\"TBLDTCFAILURE\" WHERE \"WO_DF_ID\"=\"DF_ID\" AND  \"DF_STATUS_FLAG\"='2'";
                strQry += " AND \"DF_ID\" =:sEnhanceId";
                NpgsqlCommand.Parameters.AddWithValue("sEnhanceId", Convert.ToInt32(sEnhanceId));
                string sResult = objcon.get_value(strQry, NpgsqlCommand);
                if(sResult.Length > 0)
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

        #region WorkFlow XML

        public clsEnhancement GetEnhancementDetailsFromXML(clsEnhancement objEnhancement)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable dt = new DataTable();

                dt = objApproval.GetDatatableFromXML(objEnhancement.sWFDataId);
                if (dt.Rows.Count > 0)
                {
                    objEnhancement.sEnhancementDate = Convert.ToString(dt.Rows[0]["DF_DATE"]).Trim();
                    objEnhancement.sReason = Convert.ToString(dt.Rows[0]["DF_REASON"]).Trim();
                    objEnhancement.sDtcCode = Convert.ToString(dt.Rows[0]["DF_DTC_CODE"]).Trim();
                    objEnhancement.sOfficeCode = Convert.ToString(dt.Rows[0]["DF_LOC_CODE"]);
                    objEnhancement.sCrby = Convert.ToString(dt.Rows[0]["DF_CRBY"]);
                    objEnhancement.sDtcReadings = Convert.ToString(dt.Rows[0]["DF_KWH_READING"]);
                    if (dt.Columns.Contains("DTR_COMISSION_DATE"))
                    {
                        objEnhancement.sDtrcommdate = Convert.ToString(dt.Rows[0]["DTR_COMISSION_DATE"]);
                    }
                    objEnhancement.sFilePath = Convert.ToString(dt.Rows[0]["PATH"]);
                    if (dt.Columns.Contains("DF_ENHANCE_CAPACITY"))
                    {
                        objEnhancement.sEnhancedCapacity = Convert.ToString(dt.Rows[0]["DF_ENHANCE_CAPACITY"]);
                    }                   
                   
                    objEnhancement.sEnhancementId = "0";
                    NpgsqlCommand = new NpgsqlCommand();
                    NpgsqlCommand.Parameters.AddWithValue("sDtcCode4", objEnhancement.sDtcCode);
                    objEnhancement.sDtcId = objcon.get_value("SELECT \"DT_ID\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\" =:sDtcCode4", NpgsqlCommand);
                    GetEnhancementDetails(objEnhancement);
                }
                return objEnhancement;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objEnhancement;
            }
        }

        #endregion

        public string getWfoIDforEstimationSO(string sWOId)
        {
            string sWFOID = string.Empty;
            try
            {
                NpgsqlCommand = new NpgsqlCommand();
                string sQry = string.Empty;
                sQry = "SELECT \"WO_WFO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\" =:sWOId ";
                NpgsqlCommand.Parameters.AddWithValue("sWOId", Convert.ToInt32(sWOId));
                sWFOID = objcon.get_value(sQry, NpgsqlCommand);
                return sWFOID;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return sWFOID;
            }
        }

        public string getWoIDforEstimation(string sOffCode, string sDtcCode)
        {

            string sWoID = string.Empty;
            try
            {
                NpgsqlCommand = new NpgsqlCommand();
                string sQry = string.Empty;
                sQry = "SELECT MAX(\"WO_ID\") FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_OFFICE_CODE\" =:sOffCode AND \"WO_NEXT_ROLE\"=1 AND \"WO_DATA_ID\"=:sDtcCode ";
                NpgsqlCommand.Parameters.AddWithValue("sOffCode", Convert.ToInt32(sOffCode));
                NpgsqlCommand.Parameters.AddWithValue("sDtcCode", sDtcCode);
                sWoID = objcon.get_value(sQry, NpgsqlCommand);
                return sWoID;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return sWoID;
            }

        }

        public DataTable GetLTVR_FilePath(string sWFO_ID,string sEnhance_Id)
        {
            DataTable dt = new DataTable();
            try
            {
                clsApproval objApproval = new clsApproval();

                if (!sEnhance_Id.Contains("-"))
                {
                    NpgsqlCommand = new NpgsqlCommand();
                    string sStrQry = "SELECT \"DF_FILE_PATH\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\"=:sEnhance_Id";
                    NpgsqlCommand.Parameters.AddWithValue("sEnhance_Id", Convert.ToInt32(sEnhance_Id));
                    string sFile_Path = objcon.get_value(sStrQry, NpgsqlCommand);
                    string sFileName = sFile_Path.Substring(sFile_Path.LastIndexOf('/') + 1);

                    dt.Columns.Add(new DataColumn("PATH"));
                    dt.Columns.Add(new DataColumn("NAME"));

                    DataRow drow = dt.NewRow();

                    drow["PATH"] = sFile_Path;
                    drow["NAME"] = sFileName;
                    dt.Rows.Add(drow);
                }
                else
                {
                    dt = objApproval.GetDatatableFromXML(sWFO_ID);
                }                    
                return dt;
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public void UpdatePath(string sDF_ID, string sLatestPath)
        {
            try
            {
                NpgsqlCommand = new NpgsqlCommand();
                string sStrQry = "UPDATE \"TBLDTCFAILURE\" SET \"DF_FILE_PATH\"=:sLatestPath WHERE \"DF_ID\"=:sDF_ID ";
                NpgsqlCommand.Parameters.AddWithValue("sLatestPath", sLatestPath);
                NpgsqlCommand.Parameters.AddWithValue("sDF_ID",Convert.ToInt32(sDF_ID));
                objcon.ExecuteQry(sStrQry, NpgsqlCommand);
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

    }
}
