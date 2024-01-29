using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Collections;
using IIITS.PGSQL.DAL;
using NpgsqlTypes;
using Npgsql;


namespace IIITS.DTLMS.BL
{
    public class clsPermanentWO
    {
        string strFormCode = "clsPermanentWO";

        PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);

        public string sUsername { get; set; }
        public string sWOId { get; set; }
        public string sFailureId { get; set; }
        public string sFailureDate { get; set; }
        public string sLocationCode { get; set; }
        public string sAccCode { get; set; }
        public string sCrBy { get; set; }
        public string sCommWoNo { get; set; }
        public string sCommDate { get; set; }
        public string sCommAmmount { get; set; }
        public string sDeWoNo { get; set; }
        public string sDeCommDate { get; set; }
        public string sDeCommAmmount { get; set; }

        public string sDecomAccCode { get; set; }
        public string sIssuedBy { get; set; }
        public string sCapacity { get; set; }
        public string sNewCapacity { get; set; }
        public string sEnhanceAccCode { get; set; }
        public string sEnhancedCapacity { get; set; }

        public string sTaskType { get; set; }
        public string sOfficeCode { get; set; }

        public string sDTCName { get; set; }
        public string sTCSlno { get; set; }
        public string sTCCode { get; set; }
        public string sRequestLoc { get; set; }
        public string sDTCCode { get; set; }
        public string sDTCId { get; set; }
        public string sTCId { get; set; }

        public string sWOFilePath { get; set; }

        // Workflow
        public string sFormName { get; set; }
        public string sClientIP { get; set; }
        public string sWFOId { get; set; }
        public string sWFDataId { get; set; }
        public string sActionType { get; set; }
        public string sWFAutoId { get; set; }
        public string sWFObjectId { get; set; }

        public string sApprovalDesc { get; set; }
        public int sDtcScheme { get; set; }
        public string sFailType { get; set; }
        public string sRepairer { get; set; }
        public string sOFCommWoNo { get; set; }
        public string sOFCommDate { get; set; }
        public string sOFCommAmmount { get; set; }
        public string sOFAccCode { get; set; }
        public string sGuarentyType { get; set; }

        public string sEstid { get; set; }

        NpgsqlCommand NpgsqlCommand;
        public string[] SaveUpdateWorkOrder(clsPermanentWO objWorkOrder)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string strQry = string.Empty;
            string[] Arr = new string[2];

            try
            {

                if (objWorkOrder.sWOId == "")
                {
                    NpgsqlCommand.Parameters.AddWithValue("CommWoNo",  objWorkOrder.sCommWoNo );
                    NpgsqlCommand.Parameters.AddWithValue("LocationCode",Convert.ToInt32( objWorkOrder.sLocationCode));
                    string sId = ObjCon.get_value("SELECT \"PWO_NO\" FROM \"TBLPERMANENTWORKORDER\" WHERE \"PWO_NO\"=:CommWoNo AND \"PWO_OFF_CODE\" =:LocationCode", NpgsqlCommand);
                    if (sId.Length > 0)
                    {
                        Arr[0] = "Work Order No. Already Exists";
                        Arr[1] = "2";
                        return Arr;
                    }

                    objWorkOrder.sWOId = Convert.ToString(ObjCon.Get_max_no("PWO_SLNO", "TBLPERMANENTWORKORDER"));

                    strQry = "INSERT INTO TBLPERMANENTWORKORDER (PWO_SLNO,PWO_NO_DECOM,PWO_DATE_DECOM,PWO_AMT_DECOM,";
                    strQry += "PWO_OFF_CODE,PWO_CRBY,PWO_CRON,PWO_ACCCODE_DECOM,PWO_ISSUED_BY,PWO_DTC_CAP,PWO_REQUEST_LOC,PWO_PEF_ID) VALUES";
                    strQry += "('" + objWorkOrder.sWOId + "',";
                    strQry += " '" + objWorkOrder.sDeWoNo.ToUpper() + "',";
                    strQry += " TO_DATE('" + objWorkOrder.sDeCommDate + "','dd/MM/yyyy'),'" + objWorkOrder.sDeCommAmmount + "',";
                    strQry += "'" + objWorkOrder.sLocationCode + "','" + objWorkOrder.sCrBy + "',NOW(),";
                    strQry += " '" + objWorkOrder.sDecomAccCode + "','" + objWorkOrder.sIssuedBy + "','" + objWorkOrder.sCapacity + "','" + objWorkOrder.sRequestLoc + "','" + objWorkOrder.sFailureId + "') ";



                    //************************ WORK FLOW *************************************

                    #region Workflow
                    if (objWorkOrder.sFailureId == "")
                    {
                        strQry = "INSERT INTO \"TBLPERMANENTWORKORDER\" (\"PWO_SLNO\",\"PWO_NO_DECOM\",\"PWO_DATE_DECOM\",\"PWO_AMT_DECOM\",";
                        strQry += " \"PWO_OFF_CODE\",\"PWO_CRBY\",\"PWO_CRON\",\"PWO_ACCCODE_DECOM\",\"PWO_ISSUED_BY\",\"PWO_DTC_CAP\",\"PWO_NEW_CAP\",\"PWO_REQUEST_LOC\",\"PWO_AUTO_NO\",\"PWO_PEF_ID\") VALUES";
                       


                        strQry += "('{0}',";
                        strQry += " '" + objWorkOrder.sDeWoNo.ToUpper() + "',";
                        strQry += " TO_DATE('" + objWorkOrder.sDeCommDate + "','dd/MM/yyyy'),'" + objWorkOrder.sDeCommAmmount + "',";
                        strQry += " '" + objWorkOrder.sLocationCode + "','" + objWorkOrder.sCrBy + "',NOW(),";
                        strQry += " '" + objWorkOrder.sDecomAccCode + "','" + objWorkOrder.sIssuedBy + "','" + objWorkOrder.sCapacity + "',";
                        strQry += " '" + objWorkOrder.sRequestLoc + "','{1}','" + objWorkOrder.sFailureId + "')";

                        

                    }
                    else
                    {
                        strQry = "INSERT INTO \"TBLPERMANENTWORKORDER\" (\"PWO_SLNO\",\"PWO_NO_DECOM\",\"PWO_DATE_DECOM\",\"PWO_AMT_DECOM\",";
                        strQry += " \"PWO_OFF_CODE\",\"PWO_CRBY\",\"PWO_CRON\",\"PWO_ACCCODE_DECOM\",\"PWO_ISSUED_BY\",\"PWO_DTC_CAP\",\"PWO_REQUEST_LOC\",\"PWO_AUTO_NO\",\"PWO_PEF_ID\")VALUES";

                        strQry += "('{0}',";
                        strQry += " '" + objWorkOrder.sDeWoNo.ToUpper() + "',";
                        strQry += " TO_DATE('" + objWorkOrder.sDeCommDate + "','dd/MM/yyyy'),'" + objWorkOrder.sDeCommAmmount + "',";
                        strQry += " '" + objWorkOrder.sLocationCode + "','" + objWorkOrder.sCrBy + "',NOW(),";
                        strQry += " '" + objWorkOrder.sDecomAccCode + "','" + objWorkOrder.sIssuedBy + "','" + objWorkOrder.sCapacity + "',";
                        strQry += " '" + objWorkOrder.sRequestLoc + "','{1}','" + objWorkOrder.sFailureId + "')";

                       
                    }



                    strQry = strQry.Replace("'", "''");

                    string sParam = "SELECT COALESCE(MAX(\"PWO_SLNO\"),0)+1 FROM \"TBLPERMANENTWORKORDER\"";
                    string sParam1 = "SELECT wonumberpermanent('" + objWorkOrder.sOfficeCode + "')";

                    sParam1 = sParam1.Replace("'", "''");

                    //Workflow / Approval
                    clsApproval objApproval = new clsApproval();
                    objApproval.sFormName = objWorkOrder.sFormName;
                    //objApproval.sRecordId = objWorkOrder.sWOId;
                    objApproval.sOfficeCode = objWorkOrder.sOfficeCode;
                    objApproval.sClientIp = objWorkOrder.sClientIP;
                    objApproval.sCrby = objWorkOrder.sCrBy;
                    objApproval.sWFObjectId = objWorkOrder.sWFOId;
                    objApproval.sWFAutoId = objWorkOrder.sWFAutoId;
                    objApproval.sFailType = objWorkOrder.sFailType;
                    objApproval.sQryValues = strQry;
                    objApproval.sParameterValues = sParam + ";" + sParam1;
                    objApproval.sMainTable = "TBLPERMANENTWORKORDER";
                    objApproval.sGuarentyType = objWorkOrder.sGuarentyType;

                    objApproval.sDataReferenceId = objWorkOrder.sFailureId;

                    objApproval.sRefOfficeCode = ObjCon.get_value("SELECT \"PEST_LOC_CODE\" FROM \"TBLPERMANENTESTIMATIONDETAILS\" WHERE \"PEST_ID\" ='" + objWorkOrder.sFailureId + "'");
                    objApproval.sDescription = "Work Order For PermanentDecommision Entry of DTC Code " + objWorkOrder.sDTCCode + " and DTC Name " + objWorkOrder.sDTCName + " with WO No " + objWorkOrder.sDeWoNo + "";

              

                    string sPrimaryKey = "{0}";


                    objApproval.sColumnNames = "PWO_SLNO,PWO_NO_DECOM,PWO_DATE_DECOM,PWO_AMT_DECOM,";
                    objApproval.sColumnNames += "PWO_OFF_CODE,PWO_CRBY,PWO_CRON,PWO_ACCCODE_DECOM,PWO_ISSUED_BY,PWO_DTC_CAP,";
                    objApproval.sColumnNames += "PWO_REQUEST_LOC,PWO_DTC_SCHEME,PWO_REPAIRER,PWO_PEF_ID";

                    objApproval.sColumnValues = "" + sPrimaryKey + ",";
                    objApproval.sColumnValues += "" + objWorkOrder.sDeWoNo.ToUpper() + ",";
                    objApproval.sColumnValues += "" + objWorkOrder.sDeCommDate + "," + objWorkOrder.sDeCommAmmount + ",";
                    objApproval.sColumnValues += "" + objWorkOrder.sLocationCode + "," + objWorkOrder.sCrBy + ",NOW(),";
                    objApproval.sColumnValues += "" + objWorkOrder.sDecomAccCode + "," + objWorkOrder.sIssuedBy + "," + objWorkOrder.sCapacity + ",";
                    objApproval.sColumnValues += "" + objWorkOrder.sRequestLoc + "," + objWorkOrder.sDtcScheme + "," + objWorkOrder.sRepairer + ",'" + objWorkOrder.sFailureId + "'";
                    //objApproval.sColumnValues += "" + objWorkOrder.sOFCommWoNo + ", ";




                    objApproval.sTableNames = "TBLPERMANENTWORKORDER";


                    //Check for Duplicate Approval
                    bool bApproveResult = objApproval.CheckDuplicateApprove(objApproval);
                    if (bApproveResult == false)
                    {
                        Arr[0] = "Selected Record Already Approved";
                        Arr[1] = "2";
                        return Arr;
                    }


                    if (objWorkOrder.sActionType == "M")
                    {
                        objApproval.SaveWorkFlowData(objApproval);
                        objWorkOrder.sWFDataId = objApproval.sWFDataId;
                        objWorkOrder.sApprovalDesc = objApproval.sDescription;
                    }
                    else
                    {
                        objApproval.sGuarentyType = "0";
                        objApproval.SaveWorkFlowData(objApproval);
                        objWorkOrder.sWFDataId = objApproval.sWFDataId;    //******new code for workorder report
                        objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow();
                        objApproval.SaveWorkflowObjects(objApproval);
                        objWorkOrder.sWFObjectId = objApproval.sWFObjectId; //new code for workorder report
                    }

                    #endregion

                    //string sOfficeCode = ObjCon.get_value("SELECT DF_LOC_CODE FROM TBLDTCFAILURE WHERE DF_ID='"+ objWorkOrder.sFailureId +"'");
                    //SendSMStoSectionOfficer(sOfficeCode, objWorkOrder.sDTCCode, objWorkOrder.sCommWoNo,objWorkOrder.sDTCName);

                    Arr[0] = "Work Order Created Successfully";
                    Arr[1] = "0";
                    return Arr;

                }

                else
                {
                    

                    string[] strResult = new string[2];
                    string[] strArray = new string[2];
                    NpgsqlCommand cmd = new NpgsqlCommand("sp_updatepermanentworkorder");


                    
                    cmd.Parameters.AddWithValue("slocationcode", Convert.ToString(objWorkOrder.sLocationCode));
                    cmd.Parameters.AddWithValue("swoid", Convert.ToString(objWorkOrder.sWOId));
                    cmd.Parameters.AddWithValue("sdewono", Convert.ToString(objWorkOrder.sDeWoNo));
                    cmd.Parameters.AddWithValue("sdecommdate", Convert.ToString(objWorkOrder.sDeCommDate));
                    cmd.Parameters.AddWithValue("sdecommammount", Convert.ToString(objWorkOrder.sDeCommAmmount));
                    cmd.Parameters.AddWithValue("scrby", Convert.ToString(objWorkOrder.sCrBy));
                    cmd.Parameters.AddWithValue("sdecomacccode", Convert.ToString(objWorkOrder.sDecomAccCode));
                    cmd.Parameters.AddWithValue("sissuedby", Convert.ToString(objWorkOrder.sIssuedBy));
                    cmd.Parameters.AddWithValue("scapacity", Convert.ToString(objWorkOrder.sCapacity));
                    cmd.Parameters.AddWithValue("srequestloc", Convert.ToString(objWorkOrder.sRequestLoc));
                    cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                    cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                    strArray[0] = "op_id";
                    strArray[1] = "msg";
                    strResult = ObjCon.Execute(cmd, strArray, 2);
                    return strResult;
                }
            }
            catch (Exception ex)
            {
                // ObjCon.RollBack();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }

        }

        public bool SaveWOFilePath(clsPermanentWO objWO)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {

                //FTP Parameter
                string sFTPLink = string.Empty;
                string sFTPUserName = string.Empty;
                string sFTPPassword = string.Empty;
                string sFolderName = "WORKORDER";
                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);

                string sWOFileName = string.Empty;
                string sDirectory = string.Empty;

                //  Photo Save DTLMSDocs

                clsCommon objComm = new clsCommon();
                DataTable dt = objComm.GetAppSettings();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPMAINLINK")
                    {
                        sFTPLink = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
                    }
                    else if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPUSERNAME")
                    {
                        sFTPUserName = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
                    }
                    else if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPPASSWORD")
                    {
                        sFTPPassword = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
                    }
                }

                //clsFtp objFtp = new clsFtp(sFTPLink, sFTPUserName, sFTPPassword);
                clsSFTP objFtp = new clsSFTP(SFTPPath, sFTPUserName, sFTPPassword);

                bool Isuploaded;
                sDirectory = objWO.sWOFilePath;

                // Create Directory

                bool IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sFolderName + "/");
                if (IsExists == false)
                {

                    objFtp.createDirectory(objWO.sWOId);
                }

                sWOFileName = Path.GetFileName(objWO.sWOFilePath);

                if (sWOFileName != "")
                {

                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sFolderName + "/" + objWO.sWOId + "/");
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(SFTPmainfolder + sFolderName + "/" + objWO.sWOId);
                        }

                        Isuploaded = objFtp.upload(SFTPmainfolder + sFolderName + "/" + objWO.sWOId , sWOFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objWO.sWOFilePath = SFTPmainfolder + sFolderName + "/" + objWO.sWOId + "/" + sWOFileName;

                        }
                    }
                }

                string strQry = string.Empty;
                NpgsqlCommand.Parameters.AddWithValue("WOFilePath", objWO.sWOFilePath);
                NpgsqlCommand.Parameters.AddWithValue("WOId",Convert.ToInt32( objWO.sWOId));
                strQry = "UPDATE \"TBLWORKORDER\" SET \"WO_FILE_PATH\" =:WOFilePath WHERE \"WO_SLNO\" =:WOId";
                ObjCon.ExecuteQry(strQry, NpgsqlCommand);
                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }


        public DataTable LoadAlreadyWorkOrder(clsPermanentWO objWorkOrder)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
        
                NpgsqlCommand cmd = new NpgsqlCommand("sp_loadalpermanentreadyworkorder");
                cmd.Parameters.AddWithValue("stasktype", objWorkOrder.sTaskType);
                cmd.Parameters.AddWithValue("sofficecode", objWorkOrder.sOfficeCode);
                dt = ObjCon.FetchDataTable(cmd);
                //AND DF_APPROVE_STATUS='1'
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }
        }



        public DataTable LoadAllWorkOrder(clsPermanentWO objWorkOrder)
        {

            string strQry = string.Empty;
            DataTable dt = new DataTable();
            try
            {


                NpgsqlCommand cmd = new NpgsqlCommand("sp_loadallpermanentworkorder");
                cmd.Parameters.AddWithValue("stasktype", objWorkOrder.sTaskType);
                cmd.Parameters.AddWithValue("sofficecode", objWorkOrder.sOfficeCode);
                dt = ObjCon.FetchDataTable(cmd);

                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }
        }


        public object GetWorkOrderDetails(clsPermanentWO objWorkOrder)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                string strQry = string.Empty;
                DataTable dtWODetails = new DataTable();



                NpgsqlCommand cmd = new NpgsqlCommand("sp_getpermanentworkorderdetails");
                cmd.Parameters.AddWithValue("sestid", objWorkOrder.sFailureId);
                dtWODetails = ObjCon.FetchDataTable(cmd);

                if (dtWODetails.Rows.Count > 0)
                {

                    objWorkOrder.sWOId = Convert.ToString(dtWODetails.Rows[0]["PWO_SLNO"]).Trim();
                    objWorkOrder.sCrBy = Convert.ToString(dtWODetails.Rows[0]["PWO_CRBY"]).Trim();
                    objWorkOrder.sDeWoNo = Convert.ToString(dtWODetails.Rows[0]["PWO_NO_DECOM"]).Trim();
                    objWorkOrder.sDeCommDate = Convert.ToString(dtWODetails.Rows[0]["PWO_DATE_DECOM"]).Trim();
                    objWorkOrder.sDeCommAmmount = Convert.ToString(dtWODetails.Rows[0]["PWO_AMT_DECOM"]).Trim();
                    objWorkOrder.sIssuedBy = Convert.ToString(dtWODetails.Rows[0]["PWO_ISSUED_BY"]).Trim();
                    objWorkOrder.sDecomAccCode = Convert.ToString(dtWODetails.Rows[0]["PWO_ACCCODE_DECOM"]).Trim();
                    objWorkOrder.sNewCapacity = Convert.ToString(dtWODetails.Rows[0]["PWO_NEW_CAP"]).Trim();
                    objWorkOrder.sRequestLoc = Convert.ToString(dtWODetails.Rows[0]["PWO_REQUEST_LOC"]).Trim();

                    NpgsqlCommand.Parameters.AddWithValue("WOId",Convert.ToInt32( objWorkOrder.sWOId ));

                    strQry = "SELECT \"WO_WFO_ID\" FROM (SELECT \"WO_ID\",\"WO_WFO_ID\",row_number() over (PARTITION by \"PWO_SLNO\" ";
                    strQry += " ORDER BY \"WO_ID\" desc) as \"RNUM\" FROM \"TBLPERMANENTWORKORDER\",\"TBLWORKFLOWOBJECTS\" WHERE \"WO_RECORD_ID\"";
                    strQry += " =\"PWO_SLNO\" AND \"WO_BO_ID\"='63' AND \"PWO_SLNO\"=:WOId)A WHERE \"RNUM\" = 1";
                    objWorkOrder.sWFDataId = ObjCon.get_value(strQry, NpgsqlCommand);
                    GetWODetailsFromXML(objWorkOrder);
                }
                return objWorkOrder;
            }

            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objWorkOrder;

            }

        }

        public bool ValidateUpdate(string stEstid, string sWOSlno, string sType)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                DataTable dt = new DataTable();
               
                string strQry = string.Empty;
                if (sType != "3")
                {
                    NpgsqlCommand.Parameters.AddWithValue("Estid",Convert.ToInt32( stEstid));
                    strQry = " SELECT \"PTI_WO_SLNO\" FROM \"TBLPERMANENTWORKORDER\",\"TBLPERMANENTINDENT\",\"TBLPERMANENTESTIMATIONDETAILS\" WHERE \"PWO_SLNO\"=\"PTI_WO_SLNO\" AND ";
                    strQry += " \"PEST_ID\"=\"PWO_PEF_ID\" AND \"PWO_PEF_ID\" =:Estid";
                }
                else
                {
                    NpgsqlCommand.Parameters.AddWithValue("WOSlno",Convert.ToInt32( sWOSlno));
                    strQry = "select \"PTI_WO_SLNO\" from \"TBLPERMANENTWORKORDER\",\"TBLPERMANENTINDENT\" WHERE \"PWO_SLNO\"=\"PTI_WO_SLNO\" AND ";
                    strQry += "  \"PWO_SLNO\" =:WOSlno";
                }

                string sSLNo = ObjCon.get_value(strQry, NpgsqlCommand);
                if (sSLNo.Length > 0)
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

        public clsPermanentWO GetWOBasicDetails(clsPermanentWO objWO)
        {
            string strQry = string.Empty;
            DataTable dt = new DataTable();
            try
            {


                NpgsqlCommand cmd = new NpgsqlCommand("sp_getpermanentwobasicdetails");
                cmd.Parameters.AddWithValue("swoid", objWO.sWOId);
                dt = ObjCon.FetchDataTable(cmd);

                if (dt.Rows.Count > 0)
                {
                    objWO.sCommWoNo = dt.Rows[0]["PWO_NO_DECOM"].ToString();
                    objWO.sCommDate = dt.Rows[0]["PWO_DATE_DECOM"].ToString();
                    objWO.sDTCName = dt.Rows[0]["DT_NAME"].ToString();
                    objWO.sTCCode = dt.Rows[0]["TC_CODE"].ToString();
                    objWO.sCrBy = dt.Rows[0]["US_FULL_NAME"].ToString();
                    objWO.sFailureId = dt.Rows[0]["PEST_ID"].ToString();
                  //  objWO.sFailureDate = dt.Rows[0]["DF_FAILED_DATE"].ToString();
                    objWO.sNewCapacity = dt.Rows[0]["PWO_NEW_CAP"].ToString();
                    objWO.sDTCCode = dt.Rows[0]["DT_CODE"].ToString();
                    objWO.sTCId = dt.Rows[0]["TC_ID"].ToString();
                    objWO.sDTCId = dt.Rows[0]["DT_ID"].ToString();
                    objWO.sLocationCode = dt.Rows[0]["PEST_LOC_CODE"].ToString();
                }

                return objWO;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objWO;
            }
        }


        public clsPermanentWO GetCommDecommAccCode(clsPermanentWO objWO)
        {
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();

                NpgsqlCommand cmd = new NpgsqlCommand("sp_getcommdecommacccode");
                cmd.Parameters.AddWithValue("scapacity", objWO.sCapacity);
                dt = ObjCon.FetchDataTable(cmd);

                if (dt.Rows.Count > 0)
                {
                   
                    objWO.sDecomAccCode = Convert.ToString(dt.Rows[0]["MD_DECOMM_ACCCODE"]);
                    objWO.sEnhanceAccCode = Convert.ToString(dt.Rows[0]["MD_ENHANCE_ACCCODE"]);
                }
                return objWO;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objWO;
            }
        }

        public clsPermanentWO GetDTCAccCode(clsPermanentWO objWO)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();

                NpgsqlCommand.Parameters.AddWithValue("DtcScheme",Convert.ToInt32( objWO.sDtcScheme));
                strQry = "SELECT \"SCHM_ACCCODE\" FROM \"TBLDTCSCHEME\" WHERE \"SCHM_ID\" =:DtcScheme";
                objWO.sAccCode = ObjCon.get_value(strQry, NpgsqlCommand);

                return objWO;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objWO;
            }
        }

        #region NewDTC

        public DataTable LoadNewDTCWO(clsPermanentWO objWorkOrder)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {

                NpgsqlCommand cmd = new NpgsqlCommand("sp_getcommdecommacccode");
                cmd.Parameters.AddWithValue("sofficecode", objWorkOrder.sOfficeCode);
                dt = ObjCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public object GetWODetailsForNewDTC(clsPermanentWO objWorkOrder)
        {
            try
            {
                string strQry = string.Empty;
                DataTable dtWODetails = new DataTable();


                NpgsqlCommand cmd = new NpgsqlCommand("sp_getpermanentwodetailsfornewdtc");
                cmd.Parameters.AddWithValue("swoid", objWorkOrder.sWOId);
                dtWODetails = ObjCon.FetchDataTable(cmd);

                if (dtWODetails.Rows.Count > 0)
                {

                    objWorkOrder.sWOId = dtWODetails.Rows[0]["PWO_SLNO"].ToString();
                    objWorkOrder.sAccCode = dtWODetails.Rows[0]["PWO_ACC_CODE"].ToString();
                    objWorkOrder.sCommWoNo = dtWODetails.Rows[0]["PWO_NO"].ToString();
                    objWorkOrder.sCommDate = dtWODetails.Rows[0]["PWO_DATE"].ToString();
                    objWorkOrder.sCommAmmount = dtWODetails.Rows[0]["PWO_AMT"].ToString();
                    objWorkOrder.sIssuedBy = dtWODetails.Rows[0]["PWO_ISSUED_BY"].ToString();
                    objWorkOrder.sNewCapacity = dtWODetails.Rows[0]["PWO_NEW_CAP"].ToString();
                    objWorkOrder.sCrBy = dtWODetails.Rows[0]["US_FULL_NAME"].ToString();
                    objWorkOrder.sRequestLoc = dtWODetails.Rows[0]["PWO_REQUEST_LOC"].ToString();
                }
                return objWorkOrder;
            }

            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objWorkOrder;

            }

        }

        #endregion

        public void SendSMStoSectionOfficer(string sFailureId, string sDTCCode, string sWONo, string sDTCName)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();
                NpgsqlCommand.Parameters.AddWithValue("FailureId", Convert.ToInt32(sFailureId));
                string sOfficeCode = ObjCon.get_value("SELECT \"DF_LOC_CODE\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\" =:FailureId", NpgsqlCommand);

                //strQry = "SELECT US_FULL_NAME,US_MOBILE_NO,US_EMAIL FROM TBLUSER WHERE US_ROLE_ID IN (4) AND US_OFFICE_CODE='" + sOfficeCode + "'";
                //dt = ObjCon.getDataTable(strQry);

                NpgsqlCommand cmd = new NpgsqlCommand("sp_getsocontact");
                cmd.Parameters.AddWithValue("sofficecode", sOfficeCode);
                dt = ObjCon.FetchDataTable(cmd);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sFullName = Convert.ToString(dt.Rows[i]["US_FULL_NAME"]);
                    string sMobileNo = Convert.ToString(dt.Rows[i]["US_MOBILE_NO"]);

                    clsCommunication objcomm = new clsCommunication();
                    objcomm.sSMSkey = "SMStoWorkOrder";
                    objcomm = objcomm.GetsmsTempalte(objcomm);


                    string sSMSText = String.Format(objcomm.sSMSTemplate, sDTCCode, sWONo, sDTCName);
                    //objCommunication.sendSMS(sSMSText, sMobileNo, sFullName);
                    if (objcomm.sSMSTemplateID != null && objcomm.sSMSTemplateID != "")
                    {
                        objcomm.DumpSms(sMobileNo, sSMSText, objcomm.sSMSTemplateID, "WEB");
                    }
                  
                }

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        #region WorkFlow XML

        public clsPermanentWO GetWODetailsFromXML(clsPermanentWO objWorkOrder)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable dtWODetails = new DataTable();

                dtWODetails = objApproval.GetDatatableFromXML(objWorkOrder.sWFDataId);
                if (dtWODetails.Rows.Count > 0)
                {
                     objWorkOrder.sWOId = Convert.ToString(dtWODetails.Rows[0]["PWO_SLNO"]).Trim();
                    //objWorkOrder.sAccCode = Convert.ToString(dtWODetails.Rows[0]["PWO_ACC_CODE"]).Trim();
                    objWorkOrder.sCrBy = Convert.ToString(dtWODetails.Rows[0]["PWO_CRBY"]).Trim();
                    //objWorkOrder.sCommWoNo = Convert.ToString(dtWODetails.Rows[0]["PWO_NO"]).Trim();
                   // objWorkOrder.sCommDate = Convert.ToString(dtWODetails.Rows[0]["PWO_DATE"]).Trim();
                    //objWorkOrder.sCommAmmount = Convert.ToString(dtWODetails.Rows[0]["PWO_AMT"]).Trim();
                    objWorkOrder.sDeWoNo = Convert.ToString(dtWODetails.Rows[0]["PWO_NO_DECOM"]).Trim();
                    objWorkOrder.sDeCommDate = Convert.ToString(dtWODetails.Rows[0]["PWO_DATE_DECOM"]).Trim();
                    objWorkOrder.sDeCommAmmount = Convert.ToString(dtWODetails.Rows[0]["PWO_AMT_DECOM"]).Trim();
                    objWorkOrder.sIssuedBy = Convert.ToString(dtWODetails.Rows[0]["PWO_ISSUED_BY"]).Trim();
                    objWorkOrder.sDecomAccCode = Convert.ToString(dtWODetails.Rows[0]["PWO_ACCCODE_DECOM"]).Trim();
                   // objWorkOrder.sFailureId = Convert.ToString(dtWODetails.Rows[0]["WO_DF_ID"]).Trim();
                    //objWorkOrder.sFailureDate = Convert.ToString(dtWODetails.Rows[0]["DF_FAILED_DATE"]).Trim();
                    objWorkOrder.sNewCapacity = Convert.ToString(dtWODetails.Rows[0]["PWO_DTC_CAP"]).Trim();
                    //objWorkOrder.sRequestLoc = Convert.ToString(dtWODetails.Rows[0]["PWO_REQUEST_LOC"]).Trim();
                    objWorkOrder.sDtcScheme = Convert.ToInt32(dtWODetails.Rows[0]["PWO_DTC_SCHEME"]);
                    objWorkOrder.sRepairer = Convert.ToString(dtWODetails.Rows[0]["PWO_REPAIRER"]);

                    objWorkOrder.sUsername = ObjCon.get_value("SELECT \"US_LG_NAME\" from \"TBLUSER\" WHERE \"US_ID\"='"+objWorkOrder.sCrBy+"'");
                   
                }
                return objWorkOrder;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objWorkOrder;
            }
        }

        #endregion


        public ArrayList getCreatedByUserName(string sDataId, string sOffCode)
        {
            NpgsqlCommand = new NpgsqlCommand();
            ArrayList strQrylist = new ArrayList();
            string sWoid = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand.Parameters.AddWithValue("DataId", sDataId);
                //NpgsqlCommand.Parameters.AddWithValue("OffCode", sOffCode);
                sWoid = ObjCon.get_value("SELECT MAX(\"WO_ID\") FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\"='63' AND \"WO_DATA_ID\"=:DataId AND CAST(\"WO_OFFICE_CODE\" AS TEXT) LIKE SUBSTR('"+sOffCode+"',1,3)", NpgsqlCommand);
                //dt = ObjCon.getDataTable("SELECT (SELECT US_FULL_NAME  FROM TBLUSER WHERE US_ID=WO_CR_BY) FROM TBLWORKFLOWOBJECTS WHERE  WO_INITIAL_ID ='" + sWoid + "' ORDER BY WO_ID");
                //dt = ObjCon.getDataTable("SELECT US_FULL_NAME,(CASE WHEN US_ROLE_ID='7' THEN 1 WHEN US_ROLE_ID ='2' THEN 2 WHEN US_ROLE_ID ='6' THEN 3 ELSE 4 END )SLEVEL  FROM TBLUSER WHERE US_ROLE_ID IN(7,2,6,3) AND US_OFFICE_CODE LIKE '" + sOffCode + "%' AND US_MMS_ID IS NULL AND US_STATUS='A' ORDER BY SLEVEL");
                if (sOffCode.Length < 3)
                {
                    sOffCode = clsStoreOffice.Getofficecode(sOffCode);

                }
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getcreatedbyusername");
                cmd.Parameters.AddWithValue("sofficecode", sOffCode);
                dt = ObjCon.FetchDataTable(cmd);

                for (int i = 0; i < 4; i++)
                {
                    if (dt.Rows.Count > i)
                    {
                        if (dt.Rows[i][0].ToString() != "" || dt.Rows[i][0].ToString() != null)
                            strQrylist.Add(dt.Rows[i][0].ToString());
                    }
                    else
                        strQrylist.Add("");

                }
                return strQrylist;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return strQrylist;
            }


        }

        public string getWoDataId(string sFailureId)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string sQry = string.Empty;
            string sWoDataId = string.Empty;
            NpgsqlCommand.Parameters.AddWithValue("FailureId", sFailureId);
            sQry = "SELECT MAX(\"WO_WFO_ID\") FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_DATA_ID\"=:FailureId AND \"WO_BO_ID\"='63'";
            sWoDataId = ObjCon.get_value(sQry, NpgsqlCommand);
            return sWoDataId;
        }

        public string getofficeName(string offcode)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                string sQry = string.Empty;

                sQry = "SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT)=SUBSTR(CAST('" + offcode + "' AS TEXT),1,'" + Constants.Division + "')";
                return ObjCon.get_value(sQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }

        public string getsubdivName(string Ef_id)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                string sQry = string.Empty;
                NpgsqlCommand.Parameters.AddWithValue("Efid",Convert.ToInt32(Ef_id));
                sQry = "SELECT \"SD_SUBDIV_NAME\" FROM \"TBLPERMANENTESTIMATIONDETAILS\",\"TBLSUBDIVMAST\" WHERE ";
                sQry += " CAST(\"SD_SUBDIV_CODE\" AS TEXT) = SUBSTR(CAST(\"PEST_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "') AND \"PEST_ID\" = :Efid";
                return ObjCon.get_value(sQry, NpgsqlCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }

        public DataTable GetofficeName(string Ef_id)
        {
            NpgsqlCommand = new NpgsqlCommand();
            DataTable dt = new DataTable();
            try
            {
                string sQry = string.Empty;
                NpgsqlCommand.Parameters.AddWithValue("Efid",Convert.ToInt32( Ef_id));
                sQry = "SELECT (SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(CAST(\"PEST_LOC_CODE\" as TEXT), 1,3) )DIV,(SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(CAST(\"PEST_LOC_CODE\" as TEXT), 1,4) )SUBDIV FROM \"TBLPERMANENTESTIMATIONDETAILS\" WHERE \"PEST_ID\"=:Efid";
                dt = ObjCon.FetchDataTable(sQry, NpgsqlCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        public DataTable GetofficeNameBySectionCode(string sOM_Code)
        {
            NpgsqlCommand = new NpgsqlCommand();
            DataTable dt = new DataTable();
            try
            {
                string sQry = string.Empty;
                NpgsqlCommand.Parameters.AddWithValue("OMCode",Convert.ToInt32(sOM_Code));
              
                sQry = "SELECT (SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(CAST('" + sOM_Code + "' as TEXT), 1,3) )DIV,(SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(CAST('" + sOM_Code + "' as TEXT), 1,4) )SUBDIV FROM \"TBLOMSECMAST\" WHERE \"OM_CODE\"=:OMCode";
                dt = ObjCon.FetchDataTable(sQry, NpgsqlCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable FailDetails(string sFailId, string sFailType, string ActionType, string sGuarenteeType)
        {
            NpgsqlCommand = new NpgsqlCommand();
            DataTable dtWODetails = new DataTable();
            string sQry = string.Empty;
            try
            {
                if (ActionType == "V")
                {
                    if (sFailType != "2" && sGuarenteeType != "WGP")
                    {
                        NpgsqlCommand.Parameters.AddWithValue("FailId",Convert.ToInt32(sFailId));
                        sQry = " SELECT \"PEST_ID\",\"PEST_DTC_CODE\",(SELECT \"US_FULL_NAME\" from \"TBLUSER\" WHERE \"US_ID\"=\"PEST_CRBY\")\"PEST_CRBY\",\"PEST_TC_CODE\",to_char(\"PEST_DATE\",'dd-mm-yyyy')PEST_DATE,to_char(\"PEST_DATE\",";
                        sQry += "'dd-MON-yyyy')PEST_MONTH_DATE,\"TR_ID\",\"PEST_CAPACITY\",\"PEST_FAIL_TYPE\",\"TR_NAME\",UPPER(\"DT_NAME\")DT_NAME,SUBSTR(CAST(\"PEST_LOC_CODE\" AS TEXT),1,4)PEST_LOC_CODE FROM  \"TBLPERMANENTESTIMATIONDETAILS\"  INNER JOIN ";
                        sQry += " \"TBLPERMANENTWORKORDER\" ON \"PEST_ID\"=\"PWO_PEF_ID\" INNER JOIN \"TBLTRANSREPAIRER\" ON \"PEST_REPAIRER\" = \"TR_ID\" INNER JOIN \"TBLDTCMAST\" ON CAST(\"PEST_DTC_CODE\" AS TEXT)=\"DT_CODE\" and \"PWO_SLNO\"=:FailId";
                    }
                    else
                    {
                        NpgsqlCommand.Parameters.AddWithValue("FailId1",Convert.ToInt32(sFailId));
                        sQry = "SELECT \"PEST_ID\",\"PEST_DTC_CODE\",(SELECT \"US_FULL_NAME\" from \"TBLUSER\" WHERE \"US_ID\"=\"PEST_CRBY\")\"PEST_CRBY\",\"PEST_TC_CODE\",to_char(\"PEST_DATE\",'dd-mm-yyyy')PEST_DATE,to_char(\"PEST_DATE\",'dd-MON-yyyy')PEST_MONTH_DATE";
                        sQry += ",\"PEST_CAPACITY\",\"PEST_FAIL_TYPE\",UPPER(\"DT_NAME\")DT_NAME,SUBSTR(CAST(\"PEST_LOC_CODE\" AS TEXT),1,4)PEST_LOC_CODE FROM \"TBLPERMANENTESTIMATIONDETAILS\" INNER JOIN";
                        sQry += " \"TBLPERMANENTWORKORDER\" ON \"PEST_ID\"=\"PWO_PEF_ID\"   INNER JOIN \"TBLDTCMAST\" ON CAST(\"PEST_DTC_CODE\" AS TEXT)=\"DT_CODE\" WHERE \"WO_SLNO\"=:FailId1";
                    }
                }
                else
                {
                    if (sFailType != "2" && ActionType=="")
                    {
                        NpgsqlCommand.Parameters.AddWithValue("FailId2",Convert.ToInt32(sFailId));
                        sQry = "SELECT \"PWO_ACCCODE_DECOM\",\"PWO_AMT_DECOM\",\"PWO_DATE_DECOM\",\"PWO_NO_DECOM\",\"PWO_DTC_CAP\",\"PWO_ISSUED_BY\",\"PEST_ID\",\"PEST_DTC_CODE\",(SELECT \"US_FULL_NAME\" from \"TBLUSER\" WHERE \"US_ID\"=\"PEST_CRBY\")\"PEST_CRBY\",\"PEST_TC_CODE\",to_char(\"PEST_DATE\",'dd-mm-yyyy')PEST_DATE,to_char(\"PEST_DATE\",'dd-MON-yyyy')PEST_MONTH_DATE";
                        sQry += " ,\"TR_ID\",\"PEST_CAPACITY\",\"PEST_FAIL_TYPE\",\"TR_NAME\",UPPER(\"DT_NAME\")DT_NAME,SUBSTR(CAST(\"PEST_LOC_CODE\" AS TEXT),1,4)PEST_LOC_CODE FROM \"TBLPERMANENTESTIMATIONDETAILS\" INNER JOIN \"TBLPERMANENTWORKORDER\" ON \"PEST_ID\"=\"PWO_PEF_ID\" ";
                        sQry += "  INNER JOIN \"TBLTRANSREPAIRER\" ON \"PEST_REPAIRER\" = \"TR_ID\" INNER JOIN \"TBLDTCMAST\" ON CAST(\"PEST_DTC_CODE\" AS TEXT)=\"DT_CODE\" WHERE \"PWO_SLNO\"=:FailId2";
                    }
                    else
                    {
                        NpgsqlCommand.Parameters.AddWithValue("FailId3",Convert.ToInt32(sFailId));
                        sQry = "SELECT \"TR_ID\",\"PEST_ID\",\"PEST_DTC_CODE\",(SELECT \"US_FULL_NAME\" from \"TBLUSER\" WHERE \"US_ID\"=\"PEST_CRBY\")\"PEST_CRBY\",\"PEST_TC_CODE\",to_char(\"PEST_DATE\",'dd-mm-yyyy')PEST_DATE,to_char(\"PEST_DATE\",'dd-MON-yyyy')PEST_MONTH_DATE";
                        sQry += ",\"PEST_CAPACITY\",\"PEST_FAIL_TYPE\",UPPER(\"DT_NAME\")DT_NAME,SUBSTR(CAST(\"PEST_LOC_CODE\" AS TEXT),1,4)PEST_LOC_CODE FROM  \"TBLPERMANENTESTIMATIONDETAILS\"";
                        sQry += "   INNER JOIN \"TBLTRANSREPAIRER\" ON \"PEST_REPAIRER\" = \"TR_ID\" INNER JOIN  \"TBLDTCMAST\" ON CAST(\"PEST_DTC_CODE\" AS TEXT)=\"DT_CODE\" WHERE \"PEST_ID\"=:FailId3";
                    }
                }


                dtWODetails = ObjCon.FetchDataTable(sQry, NpgsqlCommand);
                return dtWODetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtWODetails;
            }
        }


        public string FailureId(string sWo_Slno)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string sQry = string.Empty;
            try
            {
                #region Old Code when work order at only one level
                //sQry = "SELECT \"EST_ID\" FROM \"TBLWORKORDER\",\"TBLESTIMATIONDETAILS\",\"TBLDTCFAILURE\" WHERE \"DF_ID\"=\"EST_FAILUREID\" AND ";
                //sQry += " \"DF_ID\"=\"WO_DF_ID\" AND \"WO_SLNO\" ='" + sWo_Slno + "'";
                #endregion

                if (sWo_Slno.Contains('-'))
                {
                    clsFormValues objForm = new clsFormValues();
                    NpgsqlCommand.Parameters.AddWithValue("sWoSlno",Convert.ToInt32(sWo_Slno));
                    sQry = "SELECT \"WO_RECORD_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\" in (SELECT \"WOA_PREV_APPROVE_ID\" FROM ";
                    sQry += " \"TBLWORKFLOWOBJECTS\",\"TBLWO_OBJECT_AUTO\" WHERE \"WO_INITIAL_ID\"=\"WOA_INITIAL_ACTION_ID\" and ";
                    sQry += " \"WO_BO_ID\" ='63' and \"WO_RECORD_ID\"=:sWoSlno and \"WOA_BFM_ID\"='22')";
                }
                else
                {
                    NpgsqlCommand.Parameters.AddWithValue("sWoSlno1", Convert.ToInt32(sWo_Slno));
                    sQry = "SELECT \"PEST_ID\" FROM \"TBLPERMANENTWORKORDER\",\"TBLPERMANENTESTIMATIONDETAILS\" WHERE \"PEST_ID\"=\"PWO_PEF_ID\" AND ";
                    sQry += "  AND \"WO_SLNO\" =:sWoSlno1";
                }


                return ObjCon.get_value(sQry, NpgsqlCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }

        public string GetFailCoilType(string sWo_Slno, string sActionType)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string sQry = string.Empty;
            try
            {
                if (sActionType == "V")
                {
                    NpgsqlCommand.Parameters.AddWithValue("WoSlno", Convert.ToInt32(sWo_Slno));
                    sQry = "SELECT cast(\"PEST_FAIL_TYPE\" as text)|| '~' ||cast(\"PWO_SLNO\"as text) FROM \"TBLPERMANENTESTIMATIONDETAILS\",\"TBLPERMANENTWORKORDER\" WHERE \"PEST_ID\"=\"PWO_PEF_ID\"  AND \"PWO_SLNO\" =:WoSlno";
                }
                else
                {
                    NpgsqlCommand.Parameters.AddWithValue("WoSlno1", Convert.ToInt32( sWo_Slno));
                    sQry = "SELECT cast(\"PEST_FAIL_TYPE\" as text)|| '~' ||cast(\"PWO_SLNO\"as text) FROM \"TBLPERMANENTESTIMATIONDETAILS\",\"TBLPERMANENTWORKORDER\" WHERE \"PEST_ID\"=\"PWO_PEF_ID\" AND \"PWO_PEF_ID\" =:WoSlno1";
                }

                return ObjCon.get_value(sQry, NpgsqlCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }

        public string getestimatedate(string failid)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                string sQry = string.Empty;
                NpgsqlCommand.Parameters.AddWithValue("failid",Convert.ToInt32(failid));
                sQry = " SELECT TO_CHAR(\"PEST_DATE\",'dd/mm/yyyy') from \"TBLPERMANENTESTIMATIONDETAILS\" WHERE \"PEST_ID\"=:failid ";
                return ObjCon.get_value(sQry, NpgsqlCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }

        public clsPermanentWO  GetFailureDetails(clsPermanentWO objwo)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable dttcdetails = new DataTable();
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getcommdecommacccode");
                cmd.Parameters.AddWithValue("scapacity", objwo.sCapacity);
                dttcdetails = ObjCon.FetchDataTable(cmd);

                if (dttcdetails.Rows.Count > 0)
                {

                    objwo.sDecomAccCode = Convert.ToString(dttcdetails.Rows[0]["MD_DECOMM_ACCCODE"]);
                    objwo.sEnhanceAccCode = Convert.ToString(dttcdetails.Rows[0]["MD_ENHANCE_ACCCODE"]);
                }
                return objwo;
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objwo;
            }
        }
      
    }
}
