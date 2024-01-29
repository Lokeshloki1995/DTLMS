using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using IIITS.PGSQL.DAL;
using System.Configuration;
using Npgsql;

namespace IIITS.DTLMS.BL
{
    public class clsIndent
    {

        string strFormCode = "clsIndent";
        public string sDtcFailId { get; set; }
        public string sIndentId { get; set; }
        public string sFailDate { get; set; }
        public string sIndentNo { get; set; }
        public string sIndentDate { get; set; }
        public string sStoreName { get; set; }
        public string sIndentDescription { get; set; }
        public string sWOSlno { get; set; }
        public string sCrBy { get; set; }
        public string sAlertFlg { get; set; }
        public string sWoNo { get; set; }
        public string sTasktype { get; set; }
        public string sOfficeCode { get; set; }
        public string sRequstedCapacity { get; set; }
        public string sFailureId { get; set; }

        public string sDTCCode { get; set; }

        public string sCrOn { get; set; }
        public string sDTCName { get; set; }

        // Workflow
        public string sClientIP { get; set; }
        public string sWFOId { get; set; }
        public string sWFDataId { get; set; }
        public string sActionType { get; set; }
        public string sWFAutoId { get; set; }
        public string sStoreType { get; set; }
        public string sApprovalDesc { get; set; }
        public string sFormName { get; set; }
        public string sRating { get; set; }
        public string sWoAmt { get; set; }

        /// CustOledbConnection objCon = new CustOledbConnection(Constants.Password);
        NpgsqlCommand NpgsqlCommand;
        public DataTable LoadAlreadyIndent(clsIndent objIndent)
        {

            DataTable dtIndentDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                PGSqlConnection objCon = new PGSQL.DAL.PGSqlConnection(Constants.Password);
                //strQry = "SELECT DT_NAME,TO_CHAR(TC_CODE) TC_CODE,TO_CHAR(DF_ID) DF_ID ,WO_NO,DF_DTC_CODE,'YES' AS STATUS,TI_ID,WO_SLNO,TI_INDENT_NO FROM TBLDTCMAST,TBLTCMASTER, TBLDTCFAILURE,TBLWORKORDER, ";
                //strQry += " TBLINDENT WHERE DF_EQUIPMENT_ID = TC_CODE  AND DT_CODE = DF_DTC_CODE AND DF_REPLACE_FLAG = 0";
                //strQry += "  AND DF_ID =  WO_DF_ID AND WO_SLNO = TI_WO_SLNO AND DF_STATUS_FLAG='" + objIndent.sTasktype + "'";
                //strQry += " AND DF_LOC_CODE LIKE '" + objIndent.sOfficeCode + "%' ";
                //dtIndentDetails = objCon.getDataTable(strQry);

                Npgsql.NpgsqlCommand cmd = new Npgsql.NpgsqlCommand("sp_loadalreadyindent");
                cmd.Parameters.AddWithValue("task_type", objIndent.sTasktype);
                cmd.Parameters.AddWithValue("off_Code", objIndent.sOfficeCode);
                dtIndentDetails = objCon.FetchDataTable(cmd);
                return dtIndentDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtIndentDetails;
            }

        }

        public DataTable LoadAllIndent(clsIndent objIndent)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                PGSqlConnection objCon = new PGSQL.DAL.PGSqlConnection(Constants.Password);
                //strQry = "SELECT DT_NAME,TO_CHAR(TC_CODE) TC_CODE,TO_CHAR(DF_ID) DF_ID,WO_NO,DF_DTC_CODE,'YES' AS STATUS,TI_ID,WO_SLNO,TI_INDENT_NO FROM TBLDTCMAST,TBLTCMASTER, TBLDTCFAILURE,";
                //strQry += " TBLWORKORDER, TBLINDENT WHERE DF_EQUIPMENT_ID = TC_CODE  AND DT_CODE = DF_DTC_CODE AND DF_REPLACE_FLAG = 0 AND DF_ID =  WO_DF_ID ";
                //strQry += " AND WO_SLNO = TI_WO_SLNO AND DF_STATUS_FLAG='" + objIndent.sTasktype + "' ";
                //strQry += " AND DF_LOC_CODE LIKE '" + objIndent.sOfficeCode + "%'";
                //strQry += " UNION ALL ";
                //strQry += " SELECT DT_NAME,TO_CHAR(TC_CODE) TC_CODE,TO_CHAR(DF_ID) DF_ID,WO_NO,DF_DTC_CODE,'NO' AS STATUS,0 AS TI_ID,WO_SLNO,'' AS TI_INDENT_NO FROM TBLDTCMAST,TBLTCMASTER, ";
                //strQry += " TBLDTCFAILURE , TBLWORKORDER WHERE DF_EQUIPMENT_ID = TC_CODE  AND DT_CODE = DF_DTC_CODE AND DF_REPLACE_FLAG = 0 ";
                //strQry += " AND DF_ID =  WO_DF_ID AND  DF_STATUS_FLAG='" + objIndent.sTasktype + "' AND WO_SLNO NOT IN (SELECT TI_WO_SLNO FROM TBLINDENT)";
                //strQry += " AND DF_LOC_CODE LIKE '" + objIndent.sOfficeCode + "%' ";
                //dt = objCon.getDataTable(strQry);
                Npgsql.NpgsqlCommand cmd = new Npgsql.NpgsqlCommand("sp_fetchloadallindent");
                cmd.Parameters.AddWithValue("task_type", objIndent.sTasktype);
                cmd.Parameters.AddWithValue("off_Code", objIndent.sOfficeCode);
                dt = objCon.FetchDataTable(cmd);
                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public string[] SaveUpdateIndentDetails(clsIndent objIndent)
        {
            string[] Arr = new string[2];
            //  OleDbDataReader dr;
            Npgsql.NpgsqlDataReader dr;
            string strQry = string.Empty;
            try
            {
                PGSqlConnection objCon = new PGSQL.DAL.PGSqlConnection(Constants.Password);
                NpgsqlCommand = new NpgsqlCommand();
                string sIndentNo = string.Empty;
                //Check Work Order no exists or not 
                NpgsqlCommand.Parameters.AddWithValue("sWoNo", objIndent.sWoNo.ToUpper());
                string sWoNo = objCon.get_value("SELECT \"WO_NO\" FROM \"TBLWORKORDER\" WHERE UPPER(\"WO_NO\")=:sWoNo", NpgsqlCommand);
                if (sWoNo == "")
                {
                    //  dr.Close();
                    Arr[0] = "Enter Valid Work Order No";
                    Arr[1] = "2";
                    return Arr;
                }
                //  dr.Close();

                if (objIndent.sIndentId == "")
                {
                    NpgsqlCommand = new NpgsqlCommand();
                    NpgsqlCommand.Parameters.AddWithValue("sIndentNo", objIndent.sIndentNo.ToUpper());
                    sIndentNo = objCon.get_value("select \"TI_INDENT_NO\"from \"TBLINDENT\" where  UPPER(\"TI_INDENT_NO\")=:sIndentNo ", NpgsqlCommand);
                    if (sIndentNo != "")
                    {
                        //  dr.Close();
                        Arr[0] = "Indent No. Already Exists";
                        Arr[1] = "2";
                        return Arr;
                    }
                    //   dr.Close();

                    objIndent.sIndentId = Convert.ToString(objCon.Get_max_no("TI_ID", "TBLINDENT"));

                    //objCon.BeginTrans();

                    //strQry = "INSERT into TBLINDENT (TI_ID,TI_INDENT_NO,TI_INDENT_DATE,TI_STORE_ID,TI_DESC,TI_WO_SLNO,TI_CRBY,TI_CRON,TI_ALERT_FLAG) ";
                    //strQry += "values ('" + objIndent.sIndentId + "','" + objIndent.sIndentNo.ToUpper() + "',TO_DATE('" + objIndent.sIndentDate+ "','dd/MM/yyyy'),'" + objIndent.sStoreName + "','" + objIndent.sIndentDescription+ "'";
                    //strQry += ",'" + objIndent.sWOSlno + "','" + objIndent.sCrBy + "',SYSDATE,'" + objIndent.sAlertFlg + "')";
                    ////objCon.Execute(strQry);

                    //objCon.CommitTrans();

                    //Workflow / Approval
                    #region Workflow

                    //sStoreType - 1 -> STORE , 2 -> BANK
                    if (objIndent.sFailureId != "")
                    {
                        strQry = "INSERT into \"TBLINDENT\" (\"TI_ID\",\"TI_INDENT_NO\",\"TI_INDENT_DATE\",\"TI_STORE_ID\",\"TI_DESC\",\"TI_WO_SLNO\",\"TI_CRBY\",\"TI_CRON\",\"TI_ALERT_FLAG\",\"TI_STORE_TYPE\") ";
                        strQry += "values ('{0}',(SELECT INDENTNUMBER('" + objIndent.sWOSlno + "','" + objIndent.sFailureId + "')),TO_DATE('" + objIndent.sIndentDate + "','dd/MM/yyyy'),'" + objIndent.sStoreName + "','" + objIndent.sIndentDescription + "'";
                        strQry += ",'" + objIndent.sWOSlno + "','" + objIndent.sCrBy + "',Now(),'" + objIndent.sAlertFlg + "','"+ objIndent.sStoreType +"')";
                    }
                    else
                    {
                        strQry = "INSERT into \"TBLINDENT\" (\"TI_ID\",\"TI_INDENT_NO\",\"TI_INDENT_DATE\",\"TI_STORE_ID\",\"TI_DESC\",\"TI_WO_SLNO\",\"TI_CRBY\",\"TI_CRON\",\"TI_ALERT_FLAG\",\"TI_STORE_TYPE\") ";
                        strQry += "values ('{0}',(SELECT INDENTNUMBER('" + objIndent.sWOSlno + "',null)),TO_DATE('" + objIndent.sIndentDate + "','dd/MM/yyyy'),'" + objIndent.sStoreName + "','" + objIndent.sIndentDescription + "'";
                        strQry += ",'" + objIndent.sWOSlno + "','" + objIndent.sCrBy + "',Now(),'" + objIndent.sAlertFlg + "','" + objIndent.sStoreType + "')";
                    }

                    strQry = strQry.Replace("'", "''");
                    string strQry2 = string.Empty;
                    if (objIndent.sFailureId != "" && objIndent.sFailureId != null)
                    {
                         strQry2 = "UPDATE \"TBLTCMASTER\" SET \"TC_STORE_ID\"='" + objIndent.sStoreName + "' WHERE \"TC_CODE\"=(SELECT \"DF_EQUIPMENT_ID\" from \"TBLDTCFAILURE\" WHERE \"DF_ID\" = '" + objIndent.sFailureId + "')";
                        strQry2 = strQry2.Replace("'", "''");
                    }

                    string sParam = "SELECT COALESCE(MAX(\"TI_ID\"),0)+1 FROM \"TBLINDENT\"";

                    clsApproval objApproval = new clsApproval();
                    objApproval.sFormName = objIndent.sFormName;
                    //objApproval.sRecordId = objIndent.sIndentId;
                    objApproval.sOfficeCode = objIndent.sOfficeCode;
                    objApproval.sClientIp = objIndent.sClientIP;
                    objApproval.sCrby = objIndent.sCrBy;
                    objApproval.sWFObjectId = objIndent.sWFOId;
                    objApproval.sWFAutoId = objIndent.sWFAutoId;

                    objApproval.sQryValues = strQry + ";" + strQry2;
                    objApproval.sParameterValues = sParam;
                    objApproval.sMainTable = "TBLINDENT";
                    objApproval.sDataReferenceId = objIndent.sWOSlno;

                    string presentstoreid = objCon.get_value("SELECT \"STO_SM_ID\" from \"TBLSTOREOFFCODE\" WHERE \"STO_OFF_CODE\"='"+ objIndent.sOfficeCode.Substring(0, 3) + "'");

                    if (presentstoreid != objIndent.sStoreName)
                    {
                        objApproval.sStatus = "1";
                    }

                    if (objIndent.sTasktype != "3")
                    {
                        NpgsqlCommand = new NpgsqlCommand();
                        NpgsqlCommand.Parameters.AddWithValue("sWOSlno",Convert.ToInt32(objIndent.sWOSlno));
                        objApproval.sRefOfficeCode = objCon.get_value("SELECT \"DF_LOC_CODE\" FROM \"TBLDTCFAILURE\",\"TBLWORKORDER\" WHERE \"DF_ID\"=\"WO_DF_ID\" AND \"WO_SLNO\"=:sWOSlno", NpgsqlCommand);
                        objApproval.sDescription = "Indent pertaining to DTC Code - " + objIndent.sDTCCode + ", DTC Name - " + objIndent.sDTCName + " for Work Order No " + objIndent.sWoNo;
                    }
                    else
                    {
                        NpgsqlCommand = new NpgsqlCommand();
                        NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                        string sEntype = objCon.get_value("SELECT \"DF_STATUS_FLAG\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLWORKORDER\",\"TBLDTCFAILURE\",\"TBLESTIMATIONDETAILS\" WHERE \"WO_ID\" =:sWFObjectId AND  \"WO_RECORD_ID\"=\"WO_SLNO\" AND \"DF_ID\"=\"WO_DF_ID\" and \"EST_FAILUREID\"=\"DF_ID\"", NpgsqlCommand);

                        NpgsqlCommand = new NpgsqlCommand();
                        NpgsqlCommand.Parameters.AddWithValue("sWOSlno1", Convert.ToInt32(objIndent.sWOSlno));
                        objApproval.sRefOfficeCode = objCon.get_value("SELECT \"WO_REQUEST_LOC\" FROM \"TBLWORKORDER\" WHERE \"WO_SLNO\"=:sWOSlno1", NpgsqlCommand);
                        if (sEntype == "2")
                        {
                            objApproval.sDescription = "Enhacement of Indent For Indent pertaining to Work Order No " + objIndent.sWoNo;

                        }
                        if (sEntype == "4")
                        {
                            objApproval.sDescription = "Repair and enhancement of  Indent pertaining to Work Order No " + objIndent.sWoNo;
                        }
                        else if (sEntype == "1")
                        {
                            objApproval.sDescription = "Repair & replacement of Indent pertaining to Work Order No " + objIndent.sWoNo;

                        }
                        else
                        {
                            objApproval.sDescription = "New dtc commission  Indent pertaining to Work Order No " + objIndent.sWoNo;

                        }
                    }

                    string sPrimaryKey = "{0}";

                    objApproval.sColumnNames = "TI_ID,TI_INDENT_NO,TI_INDENT_DATE,TI_STORE_ID,TI_DESC,TI_WO_SLNO,TI_CRBY,TI_CRON,TI_ALERT_FLAG,TI_STORE_TYPE";


                    objApproval.sColumnValues = "" + sPrimaryKey + "," + objIndent.sIndentNo.ToUpper() + "," + objIndent.sIndentDate + ",";
                    objApproval.sColumnValues += "" + objIndent.sStoreName + "," + objIndent.sIndentDescription + ",";
                    objApproval.sColumnValues += "" + objIndent.sWOSlno + "," + objIndent.sCrBy + ",NOW()," + objIndent.sAlertFlg + "," + objIndent.sStoreType + "";

                    objApproval.sTableNames = "TBLINDENT";


                    //Check for Duplicate Approval
                    bool bApproveResult = objApproval.CheckDuplicateApprove(objApproval);
                    if (bApproveResult == false)
                    {
                        Arr[0] = "Selected Record Already Approved";
                        Arr[1] = "2";
                        return Arr;
                    }

                    if (objIndent.sActionType == "M")
                    {
                        objApproval.SaveWorkFlowData(objApproval);
                        objIndent.sWFDataId = objApproval.sWFDataId;

                    }
                    else
                    {

                        objApproval.SaveWorkFlowData(objApproval);
                        objIndent.sWFDataId = objApproval.sWFDataId;
                        objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow();
                        objApproval.SaveWorkflowObjects(objApproval);
                    }

                    #endregion

                    Arr[0] = "Saved Successfully";
                    Arr[1] = "0";
                    return Arr;
                }
                else
                {
                    NpgsqlCommand = new NpgsqlCommand();
                    NpgsqlCommand.Parameters.AddWithValue("sIndentNo1", objIndent.sIndentNo.ToUpper());
                    NpgsqlCommand.Parameters.AddWithValue("sIndentId", Convert.ToInt32(objIndent.sIndentId));
                    sIndentNo = objCon.get_value("select \"TI_INDENT_NO\" from \"TBLINDENT\" where  UPPER(\"TI_INDENT_NO\")=:sIndentNo1 and \"TI_ID\"<>:sIndentId", NpgsqlCommand);
                    if (sIndentNo == "")
                    {

                        Arr[0] = "Indent No. Already Exists";
                        Arr[1] = "2";
                        return Arr;
                    }
                    //strQry = "UPDATE TBLINDENT SET TI_INDENT_NO='" + objIndent.sIndentNo + "',TI_INDENT_DATE=TO_DATE('" + objIndent.sIndentDate + "','dd/MM/yyyy'),";
                    //strQry += " TI_DESC='" + objIndent.sIndentDescription + "',TI_STORE_ID='" + objIndent.sStoreName + "',TI_ALERT_FLAG='" + objIndent.sAlertFlg + "' ";
                    //strQry+= " where TI_ID='" + objIndent.sIndentId + "'";
                    Npgsql.NpgsqlCommand cmd = new Npgsql.NpgsqlCommand("sp_update_indent_det");
                    cmd.Parameters.AddWithValue("sIndentNo", objIndent.sIndentNo);
                    cmd.Parameters.AddWithValue("sIndentDate", objIndent.sIndentDate);
                    cmd.Parameters.AddWithValue("sIndentDescription", objIndent.sIndentDescription);
                    cmd.Parameters.AddWithValue("sStoreName", objIndent.sStoreName);
                    cmd.Parameters.AddWithValue("sAlertFlg", objIndent.sAlertFlg);
                    cmd.Parameters.AddWithValue("sIndentId", objIndent.sIndentId);
                    string[] strResult = new string[3];
                    objCon.Execute(cmd, strResult, 0);
                    objCon.ExecuteQry(strQry);
                    Arr[0] = "Updated Successfully";
                    Arr[1] = "1";
                    return Arr;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }

        }



        public object GetIndentDetails(clsIndent objIndent)
        {

            try
            {
                DataTable dtIndentDetails = new DataTable();
                string strQry = string.Empty;
                PGSqlConnection objCon = new PGSQL.DAL.PGSqlConnection(Constants.Password);
                //strQry = "SELECT TI_ID,TI_INDENT_NO,TO_CHAR(TI_INDENT_DATE,'dd/MM/yyyy') TI_INDENT_DATE,";
                //strQry += " (SELECT WO_NEW_CAP FROM TBLWORKORDER WHERE WO_SLNO=TI_WO_SLNO) CAPACITY,TO_CHAR(TI_CRON,'dd/MM/yyyy') TI_CRON,";
                //strQry += " TI_STORE_ID,TI_DESC,(SELECT US_FULL_NAME FROM TBLUSER WHERE TI_CRBY=US_ID) US_FULL_NAME  FROM TBLINDENT WHERE  TI_ID='" + objIndent.sIndentId + "'  ";
                Npgsql.NpgsqlCommand cmd = new Npgsql.NpgsqlCommand("sp_fetc_indentdet");
                cmd.Parameters.AddWithValue("sIndentId", objIndent.sIndentId);
                dtIndentDetails = objCon.FetchDataTable(cmd);
                if (dtIndentDetails.Rows.Count > 0)
                {
                    objIndent.sIndentNo = Convert.ToString(dtIndentDetails.Rows[0]["TI_INDENT_NO"]);
                    objIndent.sIndentDate = Convert.ToString(dtIndentDetails.Rows[0]["TI_INDENT_DATE"]);
                    objIndent.sStoreName = Convert.ToString(dtIndentDetails.Rows[0]["TI_STORE_ID"]);
                    objIndent.sIndentDescription = Convert.ToString(dtIndentDetails.Rows[0]["TI_DESC"]).Replace("ç", ",");
                    objIndent.sIndentId = Convert.ToString(dtIndentDetails.Rows[0]["TI_ID"]);
                    objIndent.sCrBy = Convert.ToString(dtIndentDetails.Rows[0]["US_FULL_NAME"]);
                    objIndent.sRequstedCapacity = Convert.ToString(dtIndentDetails.Rows[0]["CAPACITY"]);
                    objIndent.sCrOn = Convert.ToString(dtIndentDetails.Rows[0]["TI_CRON"]);
                    objIndent.sStoreType = Convert.ToString(dtIndentDetails.Rows[0]["TI_STORE_TYPE"]);
                    objIndent.sRating = Convert.ToString(dtIndentDetails.Rows[0]["WO_RATING"]);
                    objIndent.sWoAmt = Convert.ToString(dtIndentDetails.Rows[0]["WO_AMT"]);
                }


                return objIndent;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objIndent;

            }
        }
        public bool ValidateUpdate(string sIndentId)
        {
            try
            {
                PGSqlConnection objCon = new PGSQL.DAL.PGSqlConnection(Constants.Password);
                DataTable dt = new DataTable();
                //  OleDbDataReader dr;
                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Parameters.AddWithValue("sIndentId", Convert.ToInt32(sIndentId));
                string sValue = objCon.get_value("select \"IN_INV_NO\" from \"TBLINDENT\",\"TBLDTCINVOICE\" WHERE \"IN_TI_NO\"=\"TI_ID\" AND \"TI_ID\"=:sIndentId", NpgsqlCommand);
                //   dt.Load(dr);
                if (sValue != "")
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

        public string GetTransformerCount(string sOfficeCode, string sWOslno, string sStoreType, string sRoleType = "")
        {
            try
            {
                PGSqlConnection objCon = new PGSQL.DAL.PGSqlConnection(Constants.Password);
                string strQry = string.Empty;
                string sStoreId = string.Empty;

                if (sRoleType == "1" || sRoleType == "")
                {
                    //int Division = Convert.ToInt32(ConfigurationSettings.AppSettings["Division_code"]);

                    //if (sOfficeCode.Length > 1)
                    //{
                    //    sOfficeCode = sOfficeCode.Substring(0, Constants.Division);
                    //}
                    //NpgsqlCommand = new NpgsqlCommand();
                    //NpgsqlCommand.Parameters.AddWithValue("sOfficeCode",Convert.ToInt32(sOfficeCode));
                    //strQry = "SELECT \"STO_SM_ID\" FROM \"TBLSTOREOFFCODE\" WHERE \"STO_OFF_CODE\" =:sOfficeCode ";
                    //sStoreId = objCon.get_value(strQry, NpgsqlCommand);

                    sStoreId = sOfficeCode;
                }
                else
                {
                    sStoreId = sOfficeCode;
                }

                //strQry = "SELECT COUNT(*) FROM TBLTCMASTER WHERE TC_STORE_ID IN (SELECT SM_ID FROM TBLSTOREMAST WHERE  SM_OFF_CODE='" + sOfficeCode + "') ";
                //strQry += " AND  TC_CAPACITY IN (SELECT WO_NEW_CAP FROM TBLWORKORDER WHERE WO_SLNO='" + sWOslno + "') AND TC_STATUS IN (1,2) AND TC_CURRENT_LOCATION=1";

                if(sStoreType == "2")
                {
                    sStoreType = "5";
                }
                NpgsqlCommand = new NpgsqlCommand();
              
                strQry = "SELECT COUNT(*) FROM \"TBLTCMASTER\" WHERE \"TC_LOCATION_ID\"=:sStoreId ";
                strQry += " AND  \"TC_CAPACITY\" IN (SELECT \"WO_NEW_CAP\" FROM \"TBLWORKORDER\" WHERE \"WO_SLNO\"=:sWOslno1) AND \"TC_STATUS\" IN (1,2) AND \"TC_CURRENT_LOCATION\"=:sStoreType1";
                NpgsqlCommand.Parameters.AddWithValue("sStoreId", Convert.ToDouble(sStoreId));
                NpgsqlCommand.Parameters.AddWithValue("sWOslno1",Convert.ToInt32( sWOslno));
                NpgsqlCommand.Parameters.AddWithValue("sStoreType1", Convert.ToInt16(sStoreType));
                return objCon.get_value(strQry, NpgsqlCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }


        public string GenerateIndentNo(string sOfficeCode)
        {
            try
             {
                PGSqlConnection objCon = new PGSQL.DAL.PGSqlConnection(Constants.Password);
                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;
                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", sOfficeCode);
                string sIndentNo = objCon.get_value("SELECT  COALESCE(MAX(CAST(\"TI_INDENT_NO\" AS INT8)),0)+1 FROM \"VIEW_INDENTNO\" WHERE CAST(\"TI_INDENT_NO\" AS TEXT) LIKE :sOfficeCode||'%' ", NpgsqlCommand);
                //string sIndentNo = objCon.get_value("select max(maxnum)+1 from (SELECT  COALESCE(MAX(CAST(\"TI_INDENT_NO\" AS INT8)),0) as maxnum FROM \"VIEW_INDENTNO\" WHERE CAST(\"TI_INDENT_NO\" AS TEXT) LIKE :sOfficeCode||'%' union all select COALESCE(max (CAST(\"WO_NO\" AS INT8)),0)  as maxnum  from (select 	replace(cast(unnest(xpath('(./TBLINDENT/TI_INDENT_NO)/text()', \"WFO_DATA\"::XML))::text as text), '''', '') AS \"WO_NO\" from \"TBLWFODATA\"  left join \"TBLWORKFLOWOBJECTS\" on cast(\"WFO_ID\" as text)=\"WO_WFO_ID\"  where \"WO_BO_ID\"=12 and \"WO_RECORD_ID\" <0 and \"WO_APPROVE_STATUS\"=0 )C where \"WO_NO\" not like '%-%' AND \"WO_NO\" !='null' )c ", NpgsqlCommand);
                //System.IO.File.AppendAllText("D:\\ERRORLOG\\indent.txt", sIndentNo);
                if (sIndentNo.Length == 1)
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

                    sIndentNo = sOfficeCode + sFinancialYear + "00001";
                }
                else
                {
                    //4 digit Section Code 13111802336
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                        {
                            sFinancialYear = System.DateTime.Now.ToString("yy");
                        }
                        if (sFinancialYear == sIndentNo.Substring(5, 2))
                          //if (sFinancialYear == sIndentNo.Substring(4, 2))
                            {
                            return sIndentNo;
                        }
                        else
                        {
                            sIndentNo = sOfficeCode + sFinancialYear + "00001";
                        }
                    }
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) < 03)
                    {
                        return sIndentNo;
                    }


                }

                return sIndentNo;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }
        public string GenerateIndentNoForRepairerTC(string sOfficeCode)
        {
            try
            {
                PGSqlConnection objCon = new PGSQL.DAL.PGSqlConnection(Constants.Password);
                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;
                NpgsqlCommand = new NpgsqlCommand();

                string off = clsStoreOffice.Getofficecode(sOfficeCode);

                NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", sOfficeCode);
                string sIndentNo = objCon.get_value("SELECT COALESCE(MAX(CAST(\"RI_INDENT_NO\" AS INT8)),0)+1  FROM \"TBLREPAIRERINVOICE\" WHERE SUBSTR(cast(\"RI_INDENT_NO\" as text),0,4) ='"+ off + "'", NpgsqlCommand);
                if (sIndentNo.Length == 1)
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

                    sIndentNo = off + sFinancialYear + "00001";
                }
                else
                {
                    //4 digit Section Code 13111802336
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                        {
                            sFinancialYear = System.DateTime.Now.ToString("yy");
                        }
                        string a = sIndentNo.Substring(3, 2);
                        if (sFinancialYear == sIndentNo.Substring(3, 2))
                        //if (sFinancialYear == sIndentNo.Substring(4, 2))
                        {
                            return sIndentNo;
                        }
                        else
                        {
                            sIndentNo = off + sFinancialYear + "00001";
                        }
                    }
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) < 03)
                    {
                        return sIndentNo;
                    }


                }

                return sIndentNo;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }

        public string GenerateRINoForRepairerTCReceive(string sOfficeCode)
        {
            try
            {
                PGSqlConnection objCon = new PGSQL.DAL.PGSqlConnection(Constants.Password);
                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;
                NpgsqlCommand = new NpgsqlCommand();

                string off = clsStoreOffice.Getofficecode(sOfficeCode);

                NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", sOfficeCode);
                string sRINum = objCon.get_value("SELECT COALESCE(MAX(CAST(\"RSD_RI_NO\" AS INT8)),0)+1  FROM \"TBLREPAIRSENTDETAILS\" WHERE SUBSTR(cast(\"RSD_RI_NO\" as text),0,4) ='" + off + "'", NpgsqlCommand);
                if (sRINum.Length == 1)
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

                    sRINum = off + sFinancialYear + "00001";
                }
                else
                {
                    //4 digit Section Code 13111802336
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                        {
                            sFinancialYear = System.DateTime.Now.ToString("yy");
                        }
                        string a = sRINum.Substring(3, 2);
                        if (sFinancialYear == sRINum.Substring(3, 2))
                        //if (sFinancialYear == sIndentNo.Substring(4, 2))
                        {
                            return sRINum;
                        }
                        else
                        {
                            sRINum = off + sFinancialYear + "00001";
                        }
                    }
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) < 03)
                    {
                        return sRINum;
                    }


                }

                return sRINum;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }



        public string GenerateBankIndentNo(string sOfficeCode)
        {
            try
            {
                PGSqlConnection objCon = new PGSQL.DAL.PGSqlConnection(Constants.Password);
                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;
                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", sOfficeCode);
                string sIndentNo = objCon.get_value("SELECT  COALESCE(MAX(CAST(\"TI_INDENT_NO\" AS INT8)),0)+1 FROM \"VIEW_INDENTNO\" WHERE CAST(\"TI_INDENT_NO\" AS TEXT) LIKE :sOfficeCode||'%' ", NpgsqlCommand);
                //System.IO.File.AppendAllText("D:\\ERRORLOG\\indent.txt", sIndentNo);
                if (sIndentNo.Length == 1)
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

                    sIndentNo = sOfficeCode + sFinancialYear + "00001";
                }
                else
                {
                    //4 digit Section Code 13111802336
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                        {
                            sFinancialYear = System.DateTime.Now.ToString("yy");
                        }
                        if (sFinancialYear == sIndentNo.Substring(4, 2))
                        {
                            return sIndentNo;
                        }
                        else
                        {
                            sIndentNo = sOfficeCode + sFinancialYear + "00001";
                        }
                    }
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) < 03)
                    {
                        return sIndentNo;
                    }


                }

                return sIndentNo;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }

        #region NewDTC
        public DataTable LoadAllNewDTCIndent(clsIndent objIndent)
        {
            DataTable dt = new DataTable();
            try
            {
                PGSqlConnection objCon = new PGSQL.DAL.PGSqlConnection(Constants.Password);
                string strQry = string.Empty;
                //strQry = "SELECT WO_SLNO,WO_NO,TO_CHAR(WO_DATE,'DD-MON-YYYY') WO_DATE,WO_ACC_CODE,TI_ID,TI_INDENT_NO,TO_CHAR(TI_INDENT_DATE,'DD-MON-YYYY') TI_INDENT_DATE,'YES' AS STATUS ";
                //strQry += " FROM TBLWORKORDER,TBLINDENT WHERE WO_SLNO=TI_WO_SLNO AND WO_REPLACE_FLG='0' AND WO_DF_ID IS NULL AND WO_REQUEST_LOC LIKE '" + objIndent.sOfficeCode + "%'";
                //strQry += " UNION ALL ";
                //strQry += " SELECT WO_SLNO,WO_NO,TO_CHAR(WO_DATE,'DD-MON-YYYY') WO_DATE,WO_ACC_CODE,0 AS TI_ID,'' AS TI_INDENT_NO,'' AS TI_INDENT_DATE,'NO' AS STATUS FROM TBLWORKORDER ";
                //strQry += " WHERE WO_REPLACE_FLG='0'  AND WO_DF_ID IS NULL AND WO_REQUEST_LOC LIKE '" + objIndent.sOfficeCode + "%'";
                //strQry += " AND  WO_SLNO NOT IN (SELECT TI_WO_SLNO FROM TBLINDENT)";
                Npgsql.NpgsqlCommand cmd = new Npgsql.NpgsqlCommand("sp_fetchallnewdtcindent");
                cmd.Parameters.AddWithValue("off_Code", objIndent.sOfficeCode);
                dt = objCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable LoadAlreadyNewDTCIndent(clsIndent objIndent)
        {
            DataTable dt = new DataTable();
            try
            {

                string strQry = string.Empty;

                PGSqlConnection objCon = new PGSQL.DAL.PGSqlConnection(Constants.Password);
                Npgsql.NpgsqlCommand cmd = new Npgsql.NpgsqlCommand("sp_fetchalreadynewdtcindent");
                cmd.Parameters.AddWithValue("off_Code", objIndent.sOfficeCode);  //objIndent.sOfficeCode.Substring(0,2)
                //strQry = " SELECT WO_SLNO,WO_NO,TO_CHAR(WO_DATE,'DD-MON-YYYY') WO_DATE,WO_ACC_CODE,TI_ID,TI_INDENT_NO,TO_CHAR(TI_INDENT_DATE,'DD-MON-YYYY') TI_INDENT_DATE,'YES' AS STATUS ";
                //strQry += " FROM TBLWORKORDER,TBLINDENT WHERE WO_SLNO=TI_WO_SLNO AND WO_REPLACE_FLG='0' AND WO_DF_ID IS NULL ";            
                //strQry += "  AND WO_OFF_CODE LIKE '" + objIndent.sOfficeCode.Substring(0,2) + "%'";
                dt = objCon.FetchDataTable(strQry);
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

        public clsIndent GetIndentDetailsFromXML(clsIndent objIndent)
        {
            try
            {
                PGSqlConnection objCon = new PGSQL.DAL.PGSqlConnection(Constants.Password);
                clsApproval objApproval = new clsApproval();
                DataTable dtIndentDetails = new DataTable();

                dtIndentDetails = objApproval.GetDatatableFromXML(objIndent.sWFDataId);
                if (dtIndentDetails.Rows.Count > 0)
                {
                    objIndent.sIndentNo = Convert.ToString(dtIndentDetails.Rows[0]["TI_INDENT_NO"]);
                    objIndent.sIndentDate = Convert.ToString(dtIndentDetails.Rows[0]["TI_INDENT_DATE"]);
                    objIndent.sStoreName = Convert.ToString(dtIndentDetails.Rows[0]["TI_STORE_ID"]);
                    objIndent.sIndentDescription = Convert.ToString(dtIndentDetails.Rows[0]["TI_DESC"]).Replace("ç", ",");
                    objIndent.sIndentId = Convert.ToString(dtIndentDetails.Rows[0]["TI_ID"]);
                    objIndent.sWOSlno = Convert.ToString(dtIndentDetails.Rows[0]["TI_WO_SLNO"]);
                    objIndent.sCrOn = objCon.get_value("SELECT TO_CHAR(" + dtIndentDetails.Rows[0]["TI_CRON"] + ",'DD/MM/YYYY')");
                    //objIndent.sCrOn =Convert.ToDateTime(Convert.ToString(dtIndentDetails.Rows[0]["TI_CRON"])).ToString("dd/MM/yyyy");
                    objIndent.sCrBy = Convert.ToString(dtIndentDetails.Rows[0]["TI_CRBY"]);
                    objIndent.sStoreType = Convert.ToString(dtIndentDetails.Rows[0]["TI_STORE_TYPE"]);
                    //objIndent.sCrBy = objCon.get_value("SELECT US_FULL_NAME FROM TBLUSER WHERE US_ID='"+ sCrby  +"'");
                    //objIndent.sRequstedCapacity = objCon.get_value("SELECT WO_NEW_CAP FROM TBLWORKORDER WHERE WO_SLNO='" + objIndent.sWOSlno + "'"); 
                }
                return objIndent;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objIndent;
            }
        }

        #endregion


        public bool CheckIndentCreation3DaysExceeds(string sIndentCreatedDate)
        {
            try
            {
                PGSqlConnection objCon = new PGSQL.DAL.PGSqlConnection(Constants.Password);
                string strQry = string.Empty;
                string sResult = string.Empty;
                if (sIndentCreatedDate == "")
                {
                    return false;
                }
                DateTime dIndentDate = DateTime.ParseExact(sIndentCreatedDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                strQry = "SELECT (cast(TO_CHAR(now(),'YYYYMMDD') as date) -  cast('" + dIndentDate.ToString("yyyyMMdd") + "' as date))";
                sResult = objCon.get_value(strQry);
                if (Convert.ToInt32(sResult) > 3)
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
    }
}
