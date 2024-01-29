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
    public class clsPermanentIndent
    {
        string strFormCode = "clsPermanentIndent";
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

        /// CustOledbConnection objCon = new CustOledbConnection(Constants.Password);

        public DataTable LoadAlreadyIndent(clsPermanentIndent objIndent)
        {

            DataTable dtIndentDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                PGSqlConnection objCon = new PGSQL.DAL.PGSqlConnection(Constants.Password);


                Npgsql.NpgsqlCommand cmd = new Npgsql.NpgsqlCommand("sp_loadalreadypermanentindent");
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

        public DataTable LoadAllIndent(clsPermanentIndent objIndent)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                PGSqlConnection objCon = new PGSQL.DAL.PGSqlConnection(Constants.Password);
         
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
        NpgsqlCommand NpgsqlCommand;
        public string[] SaveUpdateIndentDetails(clsPermanentIndent objIndent)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string[] Arr = new string[2];
            //  OleDbDataReader dr;
            Npgsql.NpgsqlDataReader dr;
            string strQry = string.Empty;
            try
            {
                PGSqlConnection objCon = new PGSQL.DAL.PGSqlConnection(Constants.Password);
                string sIndentNo = string.Empty;
                //Check Work Order no exists or not
                NpgsqlCommand.Parameters.AddWithValue("WoNo", objIndent.sWoNo.ToUpper());
                string sWoNo = objCon.get_value("SELECT \"PWO_NO_DECOM\" FROM \"TBLPERMANENTWORKORDER\" WHERE UPPER(\"PWO_NO_DECOM\")=:WoNo", NpgsqlCommand);
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
                    NpgsqlCommand.Parameters.AddWithValue("IndentNo",  objIndent.sIndentNo.ToUpper() );
                    sIndentNo = objCon.get_value("select \"PTI_INDENT_NO\"from \"TBLPERMANENTINDENT\" where  UPPER(\"PTI_INDENT_NO\")=:IndentNo", NpgsqlCommand);
                    if (sIndentNo != "")
                    {
                        //  dr.Close();
                        Arr[0] = "Indent No. Already Exists";
                        Arr[1] = "2";
                        return Arr;
                    }
                    //   dr.Close();

                    objIndent.sIndentId = Convert.ToString(objCon.Get_max_no("PTI_ID", "TBLPERMANENTINDENT"));

                   
                    #region Workflow

                    //sStoreType - 1 -> STORE , 2 -> BANK
                    if (objIndent.sFailureId != "")
                    {

                        //commented by sandeep on 07/04/2022 bcz we passing only woslno to permanentindentnumber
                        //strQry = "INSERT into \"TBLPERMANENTINDENT\" (\"PTI_ID\",\"PTI_INDENT_NO\",\"PTI_INDENT_DATE\",\"PTI_STORE_ID\",\"PTI_DESC\",\"PTI_WO_SLNO\",\"PTI_CRBY\",\"PTI_CRON\",\"PTI_ALERT_FLAG\",\"PTI_STORE_TYPE\") ";
                        //strQry += "values ('{0}',(SELECT permanentindentnumber('" + objIndent.sWOSlno + "','" + objIndent.sFailureId + "')),TO_DATE('" + objIndent.sIndentDate + "','dd/MM/yyyy'),'" + objIndent.sStoreName + "','" + objIndent.sIndentDescription + "'";
                        //strQry += ",'" + objIndent.sWOSlno + "','" + objIndent.sCrBy + "',Now(),'" + objIndent.sAlertFlg + "','" + objIndent.sStoreType + "')";

                        strQry = "INSERT into \"TBLPERMANENTINDENT\" (\"PTI_ID\",\"PTI_INDENT_NO\",\"PTI_INDENT_DATE\",\"PTI_STORE_ID\",\"PTI_DESC\",\"PTI_WO_SLNO\",\"PTI_CRBY\",\"PTI_CRON\",\"PTI_ALERT_FLAG\",\"PTI_STORE_TYPE\") ";
                        strQry += "values ('{0}',(SELECT permanentindentnumber('" + objIndent.sWOSlno + "')),TO_DATE('" + objIndent.sIndentDate + "','dd/MM/yyyy'),'" + objIndent.sStoreName + "','" + objIndent.sIndentDescription + "'";
                        strQry += ",'" + objIndent.sWOSlno + "','" + objIndent.sCrBy + "',Now(),'" + objIndent.sAlertFlg + "','" + objIndent.sStoreType + "')";
                    }
                    else
                    {
                        //strQry = "INSERT into \"TBLPERMANENTINDENT\" (\"PTI_ID\",\"PTI_INDENT_NO\",\"PTI_INDENT_DATE\",\"PTI_STORE_ID\",\"PTI_DESC\",\"PTI_WO_SLNO\",\"PTI_CRBY\",\"PTI_CRON\",\"PTI_ALERT_FLAG\",\"PTI_STORE_TYPE\") ";
                        //strQry += "values ('{0}',(SELECT permanentindentnumber('" + objIndent.sWOSlno + "',null)),TO_DATE('" + objIndent.sIndentDate + "','dd/MM/yyyy'),'" + objIndent.sStoreName + "','" + objIndent.sIndentDescription + "'";
                        //strQry += ",'" + objIndent.sWOSlno + "','" + objIndent.sCrBy + "',Now(),'" + objIndent.sAlertFlg + "','" + objIndent.sStoreType + "')";

                        strQry = "INSERT into \"TBLPERMANENTINDENT\" (\"PTI_ID\",\"PTI_INDENT_NO\",\"PTI_INDENT_DATE\",\"PTI_STORE_ID\",\"PTI_DESC\",\"PTI_WO_SLNO\",\"PTI_CRBY\",\"PTI_CRON\",\"PTI_ALERT_FLAG\",\"PTI_STORE_TYPE\") ";
                        strQry += "values ('{0}',(SELECT permanentindentnumber('" + objIndent.sWOSlno + "')),TO_DATE('" + objIndent.sIndentDate + "','dd/MM/yyyy'),'" + objIndent.sStoreName + "','" + objIndent.sIndentDescription + "'";
                        strQry += ",'" + objIndent.sWOSlno + "','" + objIndent.sCrBy + "',Now(),'" + objIndent.sAlertFlg + "','" + objIndent.sStoreType + "')";
                    }


                    strQry = strQry.Replace("'", "''");

                    string sParam = "SELECT COALESCE(MAX(\"PTI_ID\"),0)+1 FROM \"TBLPERMANENTINDENT\"";

                    clsApproval objApproval = new clsApproval();
                    objApproval.sFormName = objIndent.sFormName;
                    //objApproval.sRecordId = objIndent.sIndentId;
                    objApproval.sOfficeCode = objIndent.sOfficeCode;
                    objApproval.sClientIp = objIndent.sClientIP;
                    objApproval.sCrby = objIndent.sCrBy;
                    objApproval.sWFObjectId = objIndent.sWFOId;
                    objApproval.sWFAutoId = objIndent.sWFAutoId;

                    objApproval.sQryValues = strQry;
                    objApproval.sParameterValues = sParam;
                    objApproval.sMainTable = "TBLPERMANENTINDENT";
                    objApproval.sDataReferenceId = objIndent.sWOSlno;

                    if (objIndent.sTasktype != "3")
                    {
                        objApproval.sRefOfficeCode = objCon.get_value("SELECT \"PEST_LOC_CODE\" FROM \"TBLPERMANENTESTIMATIONDETAILS\",\"TBLPERMANENTWORKORDER\" WHERE \"PEST_ID\"=\"PWO_PEF_ID\" AND \"PWO_SLNO\"='" + objIndent.sWOSlno + "'");
                        objApproval.sDescription = "PermanentIndent pertaining to DTC Code - " + objIndent.sDTCCode + ", DTC Name - " + objIndent.sDTCName + " for Work Order No " + objIndent.sWoNo;
                    }
                
                  
                    string sPrimaryKey = "{0}";

                    objApproval.sColumnNames = "PTI_ID,PTI_INDENT_NO,PTI_INDENT_DATE,PTI_STORE_ID,PTI_DESC,PTI_WO_SLNO,PTI_CRBY,PTI_CRON,PTI_ALERT_FLAG,PTI_STORE_TYPE";


                    objApproval.sColumnValues = "" + sPrimaryKey + "," + objIndent.sIndentNo.ToUpper() + "," + objIndent.sIndentDate + ",";
                    objApproval.sColumnValues += "" + objIndent.sStoreName + "," + objIndent.sIndentDescription + ",";
                    objApproval.sColumnValues += "" + objIndent.sWOSlno + "," + objIndent.sCrBy + ",NOW()," + objIndent.sAlertFlg + "," + objIndent.sStoreType + "";

                    objApproval.sTableNames = "TBLPERMANENTINDENT";


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
                    NpgsqlCommand.Parameters.AddWithValue("IndentNo1", objIndent.sIndentNo.ToUpper());
                    NpgsqlCommand.Parameters.AddWithValue("IndentId",Convert.ToInt32( objIndent.sIndentId));
                    sIndentNo = objCon.get_value("select \"PTI_INDENT_NO\" from \"TBLPERMANENTINDENT\" where  UPPER(\"PTI_INDENT_NO\")=:IndentNo1 and \"PTI_ID\"<>:IndentId", NpgsqlCommand);
                    if (sIndentNo == "")
                    {

                        Arr[0] = "PermanentIndent No. Already Exists";
                        Arr[1] = "2";
                        return Arr;
                    }

                    Npgsql.NpgsqlCommand cmd = new Npgsql.NpgsqlCommand("sp_update_permanent_indent_det");
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



        public object GetIndentDetails(clsPermanentIndent objIndent)
        {

            try
            {
                DataTable dtIndentDetails = new DataTable();
                string strQry = string.Empty;
                PGSqlConnection objCon = new PGSQL.DAL.PGSqlConnection(Constants.Password);
                Npgsql.NpgsqlCommand cmd = new Npgsql.NpgsqlCommand("sp_fetc_permanent_indentdet");
                cmd.Parameters.AddWithValue("sIndentId", objIndent.sIndentId);
                dtIndentDetails = objCon.FetchDataTable(cmd);
                if (dtIndentDetails.Rows.Count > 0)
                {
                    objIndent.sIndentNo = Convert.ToString(dtIndentDetails.Rows[0]["PTI_INDENT_NO"]);
                    objIndent.sIndentDate = Convert.ToString(dtIndentDetails.Rows[0]["PTI_INDENT_DATE"]);
                    objIndent.sStoreName = Convert.ToString(dtIndentDetails.Rows[0]["PTI_STORE_ID"]);
                    objIndent.sIndentDescription = Convert.ToString(dtIndentDetails.Rows[0]["PTI_DESC"]).Replace("ç", ",");
                    objIndent.sIndentId = Convert.ToString(dtIndentDetails.Rows[0]["PTI_ID"]);
                    objIndent.sCrBy = Convert.ToString(dtIndentDetails.Rows[0]["US_FULL_NAME"]);
                    objIndent.sRequstedCapacity = Convert.ToString(dtIndentDetails.Rows[0]["CAPACITY"]);
                    objIndent.sCrOn = Convert.ToString(dtIndentDetails.Rows[0]["PTI_CRON"]);
                    objIndent.sStoreType = Convert.ToString(dtIndentDetails.Rows[0]["PTI_STORE_TYPE"]);
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
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                PGSqlConnection objCon = new PGSQL.DAL.PGSqlConnection(Constants.Password);
                DataTable dt = new DataTable();
                //  OleDbDataReader dr;
                NpgsqlCommand.Parameters.AddWithValue("IndentId",Convert.ToInt32( sIndentId));
                string sValue = objCon.get_value("select \"PIN_INV_NO\" from \"TBLPERMANENTINDENT\",\"TBLDTCINVOICE\" WHERE \"IN_TI_NO\"=\"TI_ID\" AND \"TI_ID\"=:IndentId", NpgsqlCommand);
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
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                PGSqlConnection objCon = new PGSQL.DAL.PGSqlConnection(Constants.Password);
                string strQry = string.Empty;
                string sStoreId = string.Empty;

                if (sRoleType == "1" || sRoleType == "")
                {
                    int Division = Convert.ToInt32(ConfigurationSettings.AppSettings["Division_code"]);

                    if (sOfficeCode.Length > 1)
                    {
                        sOfficeCode = sOfficeCode.Substring(0, Constants.Division);
                    }
                    NpgsqlCommand.Parameters.AddWithValue("OfficeCode",Convert.ToInt32( sOfficeCode));
                    strQry = "SELECT \"STO_SM_ID\" FROM \"TBLSTOREOFFCODE\" WHERE \"STO_OFF_CODE\" = :OfficeCode ";
                    sStoreId = objCon.get_value(strQry, NpgsqlCommand);
                }
                else
                {
                    sStoreId = sOfficeCode;
                }

   
                if (sStoreType == "2")
                {
                    sStoreType = "5";
                }

                NpgsqlCommand.Parameters.AddWithValue("StoreId",Convert.ToDouble( sStoreId));
                NpgsqlCommand.Parameters.AddWithValue("WOslno",Convert.ToInt32( sWOslno));
                NpgsqlCommand.Parameters.AddWithValue("StoreType",Convert.ToInt16( sStoreType));
                strQry = "SELECT COUNT(*) FROM \"TBLTCMASTER\" WHERE \"TC_LOCATION_ID\"=:StoreId";
                strQry += " AND  \"TC_CAPACITY\" IN (SELECT \"PWO_DTC_CAP\" FROM \"TBLPERMANENTWORKORDER\" WHERE \"PWO_SLNO\"=:WOslno) AND \"TC_STATUS\" IN (1,2) AND \"TC_CURRENT_LOCATION\"=:StoreType";
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
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                PGSqlConnection objCon = new PGSQL.DAL.PGSqlConnection(Constants.Password);
                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;
                NpgsqlCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                string sIndentNo = objCon.get_value("SELECT  COALESCE(MAX(CAST(\"PTI_INDENT_NO\" AS INT8)),0)+1 FROM \"TBLPERMANENTINDENT\" WHERE CAST(\"PTI_INDENT_NO\" AS TEXT) LIKE :OfficeCode||'%'", NpgsqlCommand);
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
        public DataTable LoadAllNewDTCIndent(clsPermanentIndent objIndent)
        {
            DataTable dt = new DataTable();
            try
            {
                PGSqlConnection objCon = new PGSQL.DAL.PGSqlConnection(Constants.Password);
                string strQry = string.Empty;

                Npgsql.NpgsqlCommand cmd = new Npgsql.NpgsqlCommand("sp_fetchallnewdtc_permanent_indent");
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

        public DataTable LoadAlreadyNewDTCIndent(clsPermanentIndent objIndent)
        {
            DataTable dt = new DataTable();
            try
            {

                string strQry = string.Empty;

                PGSqlConnection objCon = new PGSQL.DAL.PGSqlConnection(Constants.Password);
                Npgsql.NpgsqlCommand cmd = new Npgsql.NpgsqlCommand("sp_fetchalreadynewdtc_permanent_indent");
                cmd.Parameters.AddWithValue("off_Code", objIndent.sOfficeCode);  //objIndent.sOfficeCode.Substring(0,2)
             
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

        public clsPermanentIndent GetIndentDetailsFromXML(clsPermanentIndent objIndent)
        {
            try
            {
                PGSqlConnection objCon = new PGSQL.DAL.PGSqlConnection(Constants.Password);
                clsApproval objApproval = new clsApproval();
                DataTable dtIndentDetails = new DataTable();

                dtIndentDetails = objApproval.GetDatatableFromXML(objIndent.sWFDataId);
                if (dtIndentDetails.Rows.Count > 0)
                {
                    objIndent.sIndentNo = Convert.ToString(dtIndentDetails.Rows[0]["PTI_INDENT_NO"]);
                    objIndent.sIndentDate = Convert.ToString(dtIndentDetails.Rows[0]["PTI_INDENT_DATE"]);
                    objIndent.sStoreName = Convert.ToString(dtIndentDetails.Rows[0]["PTI_STORE_ID"]);
                    objIndent.sIndentDescription = Convert.ToString(dtIndentDetails.Rows[0]["PTI_DESC"]).Replace("ç", ",");
                    objIndent.sIndentId = Convert.ToString(dtIndentDetails.Rows[0]["PTI_ID"]);
                    objIndent.sWOSlno = Convert.ToString(dtIndentDetails.Rows[0]["PTI_WO_SLNO"]);
                    objIndent.sCrOn = objCon.get_value("SELECT TO_CHAR(" + dtIndentDetails.Rows[0]["PTI_CRON"] + ",'DD/MM/YYYY')");
                    //objIndent.sCrOn =Convert.ToDateTime(Convert.ToString(dtIndentDetails.Rows[0]["TI_CRON"])).ToString("dd/MM/yyyy");
                    objIndent.sCrBy = Convert.ToString(dtIndentDetails.Rows[0]["PTI_CRBY"]);
                   // objIndent.sStoreType = Convert.ToString(dtIndentDetails.Rows[0]["PTI_STORE_TYPE"]);
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
            NpgsqlCommand = new NpgsqlCommand();
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
                NpgsqlCommand.Parameters.AddWithValue("dIndentDate", dIndentDate.ToString("yyyyMMdd"));
                strQry = "SELECT ((TO_CHAR(now(),'YYYYMMDD') - :dIndentDate))";
                sResult = objCon.get_value(strQry, NpgsqlCommand);
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
