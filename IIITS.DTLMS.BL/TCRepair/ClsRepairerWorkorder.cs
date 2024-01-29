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


namespace IIITS.DTLMS.BL.TCRepair
{
  public  class ClsRepairerWorkorder
    {
        string strFormCode = "clsRepairerWorkOrder";
        //CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);

        public string sWOId { get; set; }
        public string sEstId { get; set; }
        public string sFailureDate { get; set; }
        public string sLocationCode { get; set; }
        public string sAccCode { get; set; }

        public string sScrapWoNo { get; set; }
        public string sScrapDate { get; set; }
        public string sScrapAmmount { get; set; }
        public string sScrapAccCode { get; set; }
        public string sCrBy { get; set; }
        public string sCommWoNo { get; set; }
        public string sCommDate { get; set; }
        public string sCommAmmount { get; set; }
        public string sInncuredcost { get; set; }
        public string sRepairercost { get; set; }
       
        public string sCrAccCode { get; set; }
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
      
        public string sGuarentyType { get; set; }

        public string sboid { get; set; }
        public bool scrapCheck { get; set; }

        NpgsqlCommand NpgsqlCommand;
        public string[] SaveUpdateWorkOrder(ClsRepairerWorkorder objWorkOrder)
        {
            string strQry = string.Empty;
            string[] Arr = new string[3];

            try
            {

               

                if (objWorkOrder.sWOId == "")
                {
                    objWorkOrder.sRequestLoc=objWorkOrder.sLocationCode;
                    NpgsqlCommand = new NpgsqlCommand();
                    NpgsqlCommand.Parameters.AddWithValue("sCommWoNo", objWorkOrder.sCommWoNo);
                    NpgsqlCommand.Parameters.AddWithValue("sLocationCode", Convert.ToInt32(objWorkOrder.sLocationCode));

                    if (objWorkOrder.sScrapWoNo != null && objWorkOrder.sScrapWoNo != "")
                    {
                        NpgsqlCommand.Parameters.AddWithValue("sScrapWoNo", objWorkOrder.sScrapWoNo);
                        string sWOId = ObjCon.get_value("SELECT \"RWO_SS_WO_NO\" FROM \"TBLREPAIRERWORKORDER\" WHERE \"RWO_SS_WO_NO\"=:sScrapWoNo  AND \"RWO_OFF_CODE\" =:sLocationCode", NpgsqlCommand);
                        if (sWOId.Length > 0)
                        {
                            Arr[0] = "Sale of Scrap Work Order No. Already Exists";
                            Arr[1] = "2";
                            return Arr;
                        }
                    }
                
                    string sId = ObjCon.get_value("SELECT \"RWO_NO\" FROM \"TBLREPAIRERWORKORDER\" WHERE \"RWO_NO\"=:sCommWoNo  AND \"RWO_OFF_CODE\" =:sLocationCode", NpgsqlCommand);
                    if (sId.Length > 0)
                    {
                        Arr[0] = "Work Order No. Already Exists";
                        Arr[1] = "2";
                        return Arr;
                    }
                    if (objWorkOrder.sActionType != "M")
                    {
                        DataTable dt = new DataTable();
                        NpgsqlCommand cmd = new NpgsqlCommand("sp_check_repairerworkorder_alredyexiest");
                        cmd.Parameters.AddWithValue("offCode", objWorkOrder.sLocationCode);
                        cmd.Parameters.AddWithValue("rwo_no", objWorkOrder.sCommWoNo);
                        dt = ObjCon.FetchDataTable(cmd);
                        if (dt.Rows.Count > 0)
                        {
                            Arr[0] = "Work Order No. Already Exists";
                            Arr[1] = "2";
                            return Arr;
                        }
                    }


                    objWorkOrder.sWOId = Convert.ToString(ObjCon.Get_max_no("RWO_SLNO", "TBLREPAIRERWORKORDER"));
                    strQry = "Insert INTO TBLREPAIRERWORKORDER (RWO_SLNO,RWO_NO,RWO_EST_ID,RWO_DATE,RWO_AMT,";
                    strQry += " RWO_ACC_CODE,RWO_CRBY,RWO_CRON,RWO_ISSUED_BY,RWO_DTC_CAP,RWO_NEW_CAP,RWO_REQUEST_LOC,RWO_INNC_COST,RWO_SS_WO_NO,RWO_SS_DATE,RWO_SS_AMT,RWO_SS_ACCODE) VALUES";
                    strQry += "('" + objWorkOrder.sWOId + "','" + objWorkOrder.sCommWoNo.ToUpper() + "','" + objWorkOrder.sEstId + "',";
                    strQry += " TO_DATE('" + objWorkOrder.sCommDate + "','dd/MM/yyyy'),'" + objWorkOrder.sCommAmmount + "',";
                    strQry += " '" + objWorkOrder.sAccCode + "','" + objWorkOrder.sLocationCode + "','" + objWorkOrder.sCrBy + "',NOW(),";
                    strQry += " '" + objWorkOrder.sIssuedBy + "','" + objWorkOrder.sCapacity + "','" + objWorkOrder.sNewCapacity + "','" + objWorkOrder.sRequestLoc + "',";
                    strQry += "'" + objWorkOrder.sInncuredcost + "','" + objWorkOrder.sScrapWoNo + "','" + objWorkOrder.sScrapDate + "','" + objWorkOrder.sScrapAmmount + "','" + objWorkOrder.sScrapAccCode + "')";


                    #region Workflow
                    if (objWorkOrder.sEstId == "")
                    {
                        if (objWorkOrder.scrapCheck == false)
                        {
                            strQry = "Insert into \"TBLREPAIRERWORKORDER\" (\"RWO_SLNO\",\"RWO_NO\",\"RWO_EST_ID\",\"RWO_DATE\",\"RWO_AMT\",\"RWO_INNC_COST\",";
                            strQry += " \"RWO_ACC_CODE\",\"RWO_OFF_CODE\",\"RWO_CRBY\",\"RWO_CRON\",\"RWO_ISSUED_BY\",\"RWO_DTC_CAP\",\"RWO_NEW_CAP\",\"RWO_REQUEST_LOC\",\"RWO_AUTO_NO\") VALUES";

                            strQry += "('{0}','" + objWorkOrder.sCommWoNo.ToUpper() + "',NULL,";
                            strQry += " TO_DATE('" + objWorkOrder.sCommDate + "','dd/MM/yyyy'),'" + objWorkOrder.sCommAmmount + "','" + objWorkOrder.sInncuredcost + "',";
                            strQry += " '" + objWorkOrder.sAccCode + "','" + objWorkOrder.sLocationCode + "','" + objWorkOrder.sCrBy + "',NOW(),";
                            strQry += " '" + objWorkOrder.sIssuedBy + "','" + objWorkOrder.sCapacity + "',";
                            strQry += " '" + objWorkOrder.sNewCapacity + "','" + objWorkOrder.sRequestLoc + "','{1}')";
                        }
                        else
                        {
                            strQry = "Insert into \"TBLREPAIRERWORKORDER\" (\"RWO_SLNO\",\"RWO_NO\",\"RWO_EST_ID\",\"RWO_DATE\",\"RWO_AMT\",\"RWO_INNC_COST\",";
                            strQry += " \"RWO_ACC_CODE\",\"RWO_OFF_CODE\",\"RWO_CRBY\",\"RWO_CRON\",\"RWO_ISSUED_BY\",\"RWO_DTC_CAP\",\"RWO_NEW_CAP\",\"RWO_REQUEST_LOC\",\"RWO_AUTO_NO\",";
                            strQry += " \"RWO_SS_WO_NO\",\"RWO_SS_DATE\",\"RWO_SS_AMT\",\"RWO_SS_ACCODE\") VALUES";

                            strQry += "('{0}','" + objWorkOrder.sCommWoNo.ToUpper() + "',NULL,";
                            strQry += " TO_DATE('" + objWorkOrder.sCommDate + "','dd/MM/yyyy'),'" + objWorkOrder.sCommAmmount + "','" + objWorkOrder.sInncuredcost + "',";
                            strQry += " '" + objWorkOrder.sAccCode + "','" + objWorkOrder.sLocationCode + "','" + objWorkOrder.sCrBy + "',NOW(),";
                            strQry += " '" + objWorkOrder.sIssuedBy + "','" + objWorkOrder.sCapacity + "',";
                            strQry += " '" + objWorkOrder.sNewCapacity + "','" + objWorkOrder.sRequestLoc + "','{1}','" + objWorkOrder.sScrapWoNo + "'";
                            strQry += " ,TO_DATE('" + objWorkOrder.sScrapDate + "','dd/MM/yyyy'),'" + objWorkOrder.sScrapAmmount + "','" + objWorkOrder.sScrapAccCode + "' )";
                        }

                    }
                    else
                    {

                        if (objWorkOrder.scrapCheck == false)
                        {
                            strQry = "Insert into \"TBLREPAIRERWORKORDER\" (\"RWO_SLNO\",\"RWO_NO\",\"RWO_EST_ID\",\"RWO_DATE\",\"RWO_AMT\",\"RWO_INNC_COST\",";
                            strQry += " \"RWO_ACC_CODE\",\"RWO_OFF_CODE\",\"RWO_CRBY\",\"RWO_CRON\",\"RWO_ISSUED_BY\",\"RWO_DTC_CAP\",\"RWO_NEW_CAP\",\"RWO_REQUEST_LOC\",\"RWO_AUTO_NO\") VALUES";

                            strQry += "('{0}','" + objWorkOrder.sCommWoNo.ToUpper() + "','" + objWorkOrder.sEstId + "',";
                            strQry += " TO_DATE('" + objWorkOrder.sCommDate + "','dd/MM/yyyy'),'" + objWorkOrder.sCommAmmount + "','" + objWorkOrder.sInncuredcost + "',";
                            strQry += " '" + objWorkOrder.sAccCode + "','" + objWorkOrder.sLocationCode + "','" + objWorkOrder.sCrBy + "',NOW(),";
                            strQry += " '" + objWorkOrder.sIssuedBy + "','" + objWorkOrder.sCapacity + "',";
                            strQry += " '" + objWorkOrder.sNewCapacity + "','" + objWorkOrder.sRequestLoc + "','{1}')";
                        }
                        else
                        {
                            strQry = "Insert into \"TBLREPAIRERWORKORDER\" (\"RWO_SLNO\",\"RWO_NO\",\"RWO_EST_ID\",\"RWO_DATE\",\"RWO_AMT\",\"RWO_INNC_COST\",";
                            strQry += " \"RWO_ACC_CODE\",\"RWO_OFF_CODE\",\"RWO_CRBY\",\"RWO_CRON\",\"RWO_ISSUED_BY\",\"RWO_DTC_CAP\",\"RWO_NEW_CAP\",\"RWO_REQUEST_LOC\",\"RWO_AUTO_NO\",";
                            strQry += " \"RWO_SS_WO_NO\",\"RWO_SS_DATE\",\"RWO_SS_AMT\",\"RWO_SS_ACCODE\") VALUES";

                            strQry += "('{0}','" + objWorkOrder.sCommWoNo.ToUpper() + "','" + objWorkOrder.sEstId + "',";
                            strQry += " TO_DATE('" + objWorkOrder.sCommDate + "','dd/MM/yyyy'),'" + objWorkOrder.sCommAmmount + "','" + objWorkOrder.sInncuredcost + "',";
                            strQry += " '" + objWorkOrder.sAccCode + "','" + objWorkOrder.sLocationCode + "','" + objWorkOrder.sCrBy + "',NOW(),";
                            strQry += " '" + objWorkOrder.sIssuedBy + "','" + objWorkOrder.sCapacity + "',";
                            strQry += " '" + objWorkOrder.sNewCapacity + "','" + objWorkOrder.sRequestLoc + "','{1}','" + objWorkOrder.sScrapWoNo + "'";
                            strQry += " ,TO_DATE('" + objWorkOrder.sScrapDate + "','dd/MM/yyyy'),'" + objWorkOrder.sScrapAmmount + "','" + objWorkOrder.sScrapAccCode + "' )";
                        }
                    }



                    strQry = strQry.Replace("'", "''");

                    string sParam = "SELECT COALESCE(MAX(\"RWO_SLNO\"),0)+1 FROM \"TBLREPAIRERWORKORDER\"";
                    string sParam1 = "SELECT RWONUMBER('" + objWorkOrder.sOfficeCode + "')";

                    sParam1 = sParam1.Replace("'", "''");

                    //Workflow / Approval
                    clsApproval objApproval = new clsApproval();
                   
                    


                        objApproval.sFormName = objWorkOrder.sFormName;
                        objApproval.sBOId = "72";
                        objApproval.sOfficeCode = objWorkOrder.sOfficeCode;
                        objApproval.sClientIp = objWorkOrder.sClientIP;
                        objApproval.sCrby = objWorkOrder.sCrBy;
                        objApproval.sWFObjectId = objWorkOrder.sWFOId;
                        objApproval.sWFAutoId = objWorkOrder.sWFAutoId;
                        objApproval.sFailType = objWorkOrder.sFailType;
                        objApproval.sQryValues = strQry;
                        objApproval.sParameterValues = sParam + ";" + sParam1;
                        objApproval.sMainTable = "TBLREPAIRERWORKORDER";
                        objApproval.sGuarentyType = objWorkOrder.sGuarentyType;

                        objApproval.sDataReferenceId = objWorkOrder.sEstId;

                    NpgsqlCommand = new NpgsqlCommand();
                    NpgsqlCommand.Parameters.AddWithValue("sEstId", Convert.ToInt32(objWorkOrder.sEstId));
                    objApproval.sRefOfficeCode = ObjCon.get_value("SELECT \"RESTD_OFF_CODE\" FROM \"TBLREPAIRERESTIMATIONDETAILS\" WHERE \"RESTD_ID\" =:sEstId", NpgsqlCommand);
                    objApproval.sDescription = "Repairer Work Order of TC Code " + objWorkOrder.sTCCode + "  with WO No " + objWorkOrder.sCommWoNo + "";

                  

                    string sPrimaryKey = "{0}";


                    objApproval.sColumnNames = "RWO_SLNO,RWO_NO,RWO_EST_ID,RWO_DATE,RWO_AMT,RWO_INNC_COST,";
                    objApproval.sColumnNames += "RWO_ACC_CODE,RWO_OFF_CODE,RWO_CRBY,RWO_CRON,RWO_ISSUED_BY,RWO_DTC_CAP,";
                    objApproval.sColumnNames += "RWO_NEW_CAP,RWO_REQUEST_LOC,RWO_DTC_SCHEME,RWO_REPAIRER,RWO_SS_WO_NO,RWO_SS_DATE,RWO_SS_AMT,RWO_SS_ACCODE";
                    

                    objApproval.sColumnValues = "" + sPrimaryKey + "," + objWorkOrder.sCommWoNo.ToUpper() + "," + objWorkOrder.sEstId + ",";
                    objApproval.sColumnValues += "" + objWorkOrder.sCommDate + "," + objWorkOrder.sCommAmmount + "," + objWorkOrder.sInncuredcost + ",";
                    objApproval.sColumnValues += "" + objWorkOrder.sAccCode + "," + objWorkOrder.sLocationCode + "," + objWorkOrder.sCrBy + ",NOW(),";
                    objApproval.sColumnValues += "" + objWorkOrder.sIssuedBy + "," + objWorkOrder.sCapacity + ",";
                    objApproval.sColumnValues += "" + objWorkOrder.sNewCapacity + "," + objWorkOrder.sRequestLoc + "," + objWorkOrder.sDtcScheme + "," + objWorkOrder.sRepairer + ",";
                    objApproval.sColumnValues += "" + objWorkOrder.sScrapWoNo + "," + objWorkOrder.sScrapDate + "," + objWorkOrder.sScrapAmmount + "," + objWorkOrder.sScrapAccCode + "";

                    objApproval.sTableNames = "TBLREPAIRERWORKORDER";

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

                        Arr[2] = objApproval.sBOId;
                    }
                    else
                    {

                        objApproval.SaveWorkFlowData(objApproval);
                        objWorkOrder.sWFDataId = objApproval.sWFDataId;    //******new code for workorder report
                        objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow();
                        objApproval.SaveWorkflowObjects(objApproval);
                        objWorkOrder.sWFObjectId = objApproval.sWFObjectId; //new code for workorder report
                    }

                    #endregion


                    Arr[0] = "Work Order Created Successfully";
                    Arr[1] = "0";
                    return Arr;

                }

                else
                {
                   

                    string[] strResult = new string[2];
                    string[] strArray = new string[2];
                    NpgsqlCommand cmd = new NpgsqlCommand("sp_updateworkorderrepairer");


                    cmd.Parameters.AddWithValue("sestid", Convert.ToString(objWorkOrder.sEstId));
                    cmd.Parameters.AddWithValue("scommwono", Convert.ToString(objWorkOrder.sCommWoNo));
                    cmd.Parameters.AddWithValue("slocationcode", Convert.ToString(objWorkOrder.sLocationCode));
                    cmd.Parameters.AddWithValue("swoid", Convert.ToString(objWorkOrder.sWOId));
                    cmd.Parameters.AddWithValue("scommdate", Convert.ToString(objWorkOrder.sCommDate));
                    cmd.Parameters.AddWithValue("scommammount", Convert.ToString(objWorkOrder.sCommAmmount));
                     cmd.Parameters.AddWithValue("sacccode", Convert.ToString(objWorkOrder.sAccCode));
                    cmd.Parameters.AddWithValue("scrby", Convert.ToString(objWorkOrder.sCrBy));    
                    cmd.Parameters.AddWithValue("sissuedby", Convert.ToString(objWorkOrder.sIssuedBy));
                    cmd.Parameters.AddWithValue("scapacity", Convert.ToString(objWorkOrder.sCapacity));
                    cmd.Parameters.AddWithValue("snewcapacity", Convert.ToString(objWorkOrder.sNewCapacity));
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

        public bool SaveWOFilePath(ClsRepairerWorkorder objWO)
        {
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
                NpgsqlCommand = new NpgsqlCommand();

                strQry = "UPDATE \"TBLREPAIRERWORKORDER\" SET \"RWO_FILE_PATH\" =:sWOFilePath WHERE \"RWO_SLNO\" =:sWOId";
                NpgsqlCommand.Parameters.AddWithValue("sWOFilePath", objWO.sWOFilePath);
                NpgsqlCommand.Parameters.AddWithValue("sWOId", Convert.ToInt32(objWO.sWOId));
                ObjCon.ExecuteQry(strQry, NpgsqlCommand);
                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }


        public DataTable LoadAlreadyWorkOrder(ClsRepairerWorkorder objWorkOrder)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
               

                NpgsqlCommand cmd = new NpgsqlCommand("sp_loadalreadyworkorderrepairer");
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



        public DataTable LoadAllWorkOrder(ClsRepairerWorkorder objWorkOrder)
        {

            string strQry = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                

                NpgsqlCommand cmd = new NpgsqlCommand("sp_loadallworkorderrepairer");
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


        public object GetWorkOrderDetails(ClsRepairerWorkorder objWorkOrder)
        {
            try
            {
                string strQry = string.Empty;
                DataTable dtWODetails = new DataTable();

       

                NpgsqlCommand cmd = new NpgsqlCommand("sp_getworkorderdetailsrepairer");
                cmd.Parameters.AddWithValue("sestid", objWorkOrder.sEstId);
                dtWODetails = ObjCon.FetchDataTable(cmd);

                if (dtWODetails.Rows.Count > 0)
                {

                    objWorkOrder.sWOId = Convert.ToString(dtWODetails.Rows[0]["RWO_SLNO"]).Trim();
                    objWorkOrder.sAccCode = Convert.ToString(dtWODetails.Rows[0]["RWO_ACC_CODE"]).Trim();
                    objWorkOrder.sCrBy = Convert.ToString(dtWODetails.Rows[0]["RWO_CRBY"]).Trim();
                    objWorkOrder.sCommWoNo = Convert.ToString(dtWODetails.Rows[0]["RWO_NO"]).Trim();
                    objWorkOrder.sCommDate = Convert.ToString(dtWODetails.Rows[0]["RWO_DATE"]).Trim();
                    objWorkOrder.sCommAmmount = Convert.ToString(dtWODetails.Rows[0]["RWO_AMT"]).Trim();
                   
                  
                    objWorkOrder.sIssuedBy = Convert.ToString(dtWODetails.Rows[0]["RWO_ISSUED_BY"]).Trim();
                    
                    objWorkOrder.sEstId = Convert.ToString(dtWODetails.Rows[0]["RWO_EST_ID"]).Trim();
                    objWorkOrder.sFailureDate = Convert.ToString(dtWODetails.Rows[0]["RESTD_DATE"]).Trim();
                    objWorkOrder.sNewCapacity = Convert.ToString(dtWODetails.Rows[0]["RWO_NEW_CAP"]).Trim();
                    objWorkOrder.sRequestLoc = Convert.ToString(dtWODetails.Rows[0]["RWO_REQUEST_LOC"]).Trim();
                   

                    NpgsqlCommand = new NpgsqlCommand();
                    strQry = "SELECT \"WO_WFO_ID\" FROM (SELECT \"WO_ID\",\"WO_WFO_ID\",row_number() over (PARTITION by \"RWO_SLNO\" ";
                    strQry += " ORDER BY \"WO_ID\" desc) as \"RNUM\" FROM \"TBLREPAIRERWORKORDER\",\"TBLWORKFLOWOBJECTS\" WHERE \"WO_RECORD_ID\"";
                    strQry += " =\"RWO_SLNO\" AND \"WO_BO_ID\"='72' AND \"RWO_SLNO\"=:sWOId)A WHERE \"RNUM\" = 1";
                    NpgsqlCommand.Parameters.AddWithValue("sWOId", Convert.ToInt32(objWorkOrder.sWOId));
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

      

        public ClsRepairerWorkorder GetWOBasicDetails(ClsRepairerWorkorder objWO)
        {
            string strQry = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getwobasicdetailsrepairer");
                cmd.Parameters.AddWithValue("swoid", objWO.sWOId);
                dt = ObjCon.FetchDataTable(cmd);

                if (dt.Rows.Count > 0)
                {
                    objWO.sCommWoNo = dt.Rows[0]["RWO_NO"].ToString();
                    objWO.sCommDate = dt.Rows[0]["RWO_DATE"].ToString();
                   
                    objWO.sTCCode = dt.Rows[0]["TC_CODE"].ToString();
                    objWO.sCrBy = dt.Rows[0]["US_FULL_NAME"].ToString();
                    objWO.sEstId = dt.Rows[0]["RESTD_ID"].ToString();
                    objWO.sFailureDate = dt.Rows[0]["RESTD_DATE"].ToString();
                    objWO.sNewCapacity = dt.Rows[0]["RWO_NEW_CAP"].ToString();
                  
                    objWO.sTCId = dt.Rows[0]["TC_ID"].ToString();
                  
                    objWO.sLocationCode = dt.Rows[0]["RESTD_OFF_CODE"].ToString();
                }

                return objWO;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objWO;
            }
        }


        public ClsRepairerWorkorder GetCommDecommAccCode(ClsRepairerWorkorder objWO)
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
                    objWO.sAccCode = Convert.ToString(dt.Rows[0]["MD_COMM_ACCCODE"]);

                    objWO.sScrapAccCode= Convert.ToString(dt.Rows[0]["MD_SS_ACCCODE"]);

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

        public ClsRepairerWorkorder GetDTCAccCode(ClsRepairerWorkorder objWO)
        {
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();
                NpgsqlCommand = new NpgsqlCommand();
                strQry = "SELECT \"SCHM_ACCCODE\" FROM \"TBLDTCSCHEME\" WHERE \"SCHM_ID\" =:sDtcScheme";
                NpgsqlCommand.Parameters.AddWithValue("sDtcScheme", objWO.sDtcScheme);
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

        public DataTable LoadNewDTCWO(clsWorkOrder objWorkOrder)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                //strQry = "SELECT WO_SLNO,WO_NO,TO_CHAR(WO_DATE,'DD-MON-YYYY') WO_DATE,WO_ACC_CODE FROM TBLWORKORDER WHERE WO_DF_ID IS NULL ";
                //strQry += " AND WO_OFF_CODE LIKE '" + objWorkOrder.sOfficeCode + "%' AND WO_REPLACE_FLG='0'";
                //dt = ObjCon.getDataTable(strQry);

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

        public object GetWODetailsForNewDTC(clsWorkOrder objWorkOrder)
        {
            try
            {
                string strQry = string.Empty;
                DataTable dtWODetails = new DataTable();

                //strQry = "SELECT WO_SLNO,WO_NO,TO_CHAR(WO_DATE,'dd/MM/yyyy') WO_DATE,WO_AMT,WO_ACC_CODE,WO_ISSUED_BY,WO_NEW_CAP,";
                //strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE WO_CRBY=US_ID) US_FULL_NAME,WO_REQUEST_LOC FROM TBLWORKORDER WHERE ";
                //strQry += " WO_SLNO='"+ objWorkOrder.sWOId +"' ";            
                //dtWODetails = ObjCon.getDataTable(strQry);

                NpgsqlCommand cmd = new NpgsqlCommand("sp_getwodetailsfornewdtc");
                cmd.Parameters.AddWithValue("swoid", objWorkOrder.sWOId);
                dtWODetails = ObjCon.FetchDataTable(cmd);

                if (dtWODetails.Rows.Count > 0)
                {

                    objWorkOrder.sWOId = dtWODetails.Rows[0]["WO_SLNO"].ToString();
                    objWorkOrder.sAccCode = dtWODetails.Rows[0]["WO_ACC_CODE"].ToString();
                    objWorkOrder.sCommWoNo = dtWODetails.Rows[0]["WO_NO"].ToString();
                    objWorkOrder.sCommDate = dtWODetails.Rows[0]["WO_DATE"].ToString();
                    objWorkOrder.sCommAmmount = dtWODetails.Rows[0]["WO_AMT"].ToString();
                    objWorkOrder.sIssuedBy = dtWODetails.Rows[0]["WO_ISSUED_BY"].ToString();
                    objWorkOrder.sNewCapacity = dtWODetails.Rows[0]["WO_NEW_CAP"].ToString();
                    objWorkOrder.sCrBy = dtWODetails.Rows[0]["US_FULL_NAME"].ToString();
                    objWorkOrder.sRequestLoc = dtWODetails.Rows[0]["WO_REQUEST_LOC"].ToString();
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
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();
                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Parameters.AddWithValue("sFailureId", sFailureId);
                string sOfficeCode = ObjCon.get_value("SELECT \"DF_LOC_CODE\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\" =:sFailureId", NpgsqlCommand);

                //strQry = "SELECT US_FULL_NAME,US_MOBILE_NO,US_EMAIL FROM TBLUSER WHERE US_ROLE_ID IN (4) AND US_OFFICE_CODE='" + sOfficeCode + "'";
                //dt = ObjCon.getDataTable(strQry);

                NpgsqlCommand cmd = new NpgsqlCommand("sp_getsocontact");
                cmd.Parameters.AddWithValue("sofficecode", sOfficeCode);
                dt = ObjCon.FetchDataTable(cmd);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sFullName = Convert.ToString(dt.Rows[i]["US_FULL_NAME"]);
                    string sMobileNo = Convert.ToString(dt.Rows[i]["US_MOBILE_NO"]);

                    clsCommunication objCommunication = new clsCommunication();
                    objCommunication.sSMSkey = "SMStoWorkOrder";
                    objCommunication = objCommunication.GetsmsTempalte(objCommunication);

                    string sSMSText = String.Format(objCommunication.sSMSTemplate, sDTCCode, sWONo, sDTCName);
                    //objCommunication.sendSMS(sSMSText, sMobileNo, sFullName);

                    if(objCommunication.sSMSTemplateID !=null && objCommunication.sSMSTemplateID!="")
                    {
                        objCommunication.DumpSms(sMobileNo, sSMSText, objCommunication.sSMSTemplateID, "WEB");
                    }
                   
                }

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        #region WorkFlow XML

        public ClsRepairerWorkorder GetWODetailsFromXML(ClsRepairerWorkorder objWorkOrder)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable dtWODetails = new DataTable();

                dtWODetails = objApproval.GetDatatableFromXML(objWorkOrder.sWFDataId);
                if (dtWODetails.Rows.Count > 0)
                {
                    // objWorkOrder.sWOId = Convert.ToString(dtWODetails.Rows[0]["WO_SLNO"]).Trim();
                    objWorkOrder.sAccCode = Convert.ToString(dtWODetails.Rows[0]["RWO_ACC_CODE"]).Trim();
                    objWorkOrder.sCrBy = Convert.ToString(dtWODetails.Rows[0]["RWO_CRBY"]).Trim();
                    objWorkOrder.sCommWoNo = Convert.ToString(dtWODetails.Rows[0]["RWO_NO"]).Trim();
                    objWorkOrder.sCommDate = Convert.ToString(dtWODetails.Rows[0]["RWO_DATE"]).Trim();
                    objWorkOrder.sCommAmmount = Convert.ToString(dtWODetails.Rows[0]["RWO_AMT"]).Trim();
                    if (dtWODetails.Columns.Contains("RWO_INNC_COST") && Convert.ToString(dtWODetails.Rows[0]["RWO_INNC_COST"]).Trim() !="")
                    {
                        objWorkOrder.sInncuredcost = Convert.ToString(dtWODetails.Rows[0]["RWO_INNC_COST"]).Trim();
                    }
                    if (dtWODetails.Columns.Contains("RWO_SS_ACCODE") && Convert.ToString(dtWODetails.Rows[0]["RWO_SS_ACCODE"]).Trim() != ""
                        && dtWODetails.Columns.Contains("RWO_SS_AMT") && Convert.ToString(dtWODetails.Rows[0]["RWO_SS_AMT"]).Trim() != "" 
                        && dtWODetails.Columns.Contains("RWO_SS_DATE") && Convert.ToString(dtWODetails.Rows[0]["RWO_SS_DATE"]).Trim() != ""
                        && dtWODetails.Columns.Contains("RWO_SS_WO_NO") && Convert.ToString(dtWODetails.Rows[0]["RWO_SS_WO_NO"]).Trim() != "")
                    {
                        objWorkOrder.sScrapAccCode = Convert.ToString(dtWODetails.Rows[0]["RWO_SS_ACCODE"]).Trim();
                        objWorkOrder.sScrapAmmount = Convert.ToString(dtWODetails.Rows[0]["RWO_SS_AMT"]).Trim();
                        objWorkOrder.sScrapDate = Convert.ToString(dtWODetails.Rows[0]["RWO_SS_DATE"]).Trim();
                        objWorkOrder.sScrapWoNo = Convert.ToString(dtWODetails.Rows[0]["RWO_SS_WO_NO"]).Trim();

                    }
                    objWorkOrder.sIssuedBy = Convert.ToString(dtWODetails.Rows[0]["RWO_ISSUED_BY"]).Trim();
                   
                    objWorkOrder.sEstId = Convert.ToString(dtWODetails.Rows[0]["RWO_EST_ID"]).Trim();
                  
                    objWorkOrder.sNewCapacity = Convert.ToString(dtWODetails.Rows[0]["RWO_NEW_CAP"]).Trim();
                    objWorkOrder.sRequestLoc = Convert.ToString(dtWODetails.Rows[0]["RWO_REQUEST_LOC"]).Trim();
                   
                    objWorkOrder.sRepairer = Convert.ToString(dtWODetails.Rows[0]["RWO_REPAIRER"]);           
                  
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
            ArrayList strQrylist = new ArrayList();
            string sWoid = string.Empty;
            DataTable dt = new DataTable();
            try
            {

                sWoid = ObjCon.get_value("SELECT MAX(\"WO_ID\") FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\"='72' AND \"WO_DATA_ID\"='" + sDataId + "' AND CAST(\"WO_OFFICE_CODE\" AS TEXT) LIKE SUBSTR('" + sOffCode + "',1,3)");
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
            string sQry = string.Empty;
            string sWoDataId = string.Empty;
            NpgsqlCommand = new NpgsqlCommand();


            sQry = "SELECT MAX(\"WO_WFO_ID\") FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_DATA_ID\"=:sFailureId AND \"WO_BO_ID\"='72'";
            NpgsqlCommand.Parameters.AddWithValue("sFailureId", Convert.ToInt32(sFailureId));
            sWoDataId = ObjCon.get_value(sQry, NpgsqlCommand);
            return sWoDataId;
        }

        public string getofficeName(string offcode)
        {
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

        public string getsubdivName(string est_id)
        {
            try
            {
                string sQry = string.Empty;
                NpgsqlCommand = new NpgsqlCommand();
                sQry = "SELECT \"SD_SUBDIV_NAME\" FROM \"TBLREPAIRERESTIMATIONDETAILS\",\"TBLSUBDIVMAST\" WHERE ";
                sQry += " CAST(\"SD_SUBDIV_CODE\" AS TEXT) = SUBSTR(CAST(\"RESTD_OFF_CODE\" AS TEXT),1,'" + Constants.SubDivision + "') AND \"RESTD_ID\" =:restd_id";
                NpgsqlCommand.Parameters.AddWithValue("restd_id", Convert.ToInt32(est_id));
                return ObjCon.get_value(sQry, NpgsqlCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }

        public DataTable GetofficeName(string restd_id)
        {
            DataTable dt = new DataTable();
            try
            {
                string sQry = string.Empty;
                NpgsqlCommand = new NpgsqlCommand();
                sQry = "SELECT (SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(CAST(\"RESTD_OFF_CODE\" as TEXT), 1,3) )DIV,(SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(CAST(\"RESTD_OFF_CODE\" as TEXT), 1,4) )SUBDIV FROM \"TBLREPAIRERESTIMATIONDETAILS\" WHERE \"RESTD_ID\"=:restd_id";
                NpgsqlCommand.Parameters.AddWithValue("restd_id", Convert.ToInt32(restd_id));
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
            DataTable dt = new DataTable();
            try
            {
                string sQry = string.Empty;
                sQry = "SELECT (SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(CAST('" + sOM_Code + "' as TEXT), 1,3) )DIV,(SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(CAST('" + sOM_Code + "' as TEXT), 1,4) )SUBDIV FROM \"TBLOMSECMAST\" WHERE \"OM_CODE\"='" + sOM_Code + "'";
                dt = ObjCon.FetchDataTable(sQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable FailDetails(string sFailId, string sFailType, string sCoilType, string ActionType, string sGuarenteeType, string sWoRecordId = "")
        {
            DataTable dtWODetails = new DataTable();
            string sQry = string.Empty;
            try
            {
                if (ActionType == "V")
                {
                    if (sWoRecordId.Contains('-'))
                    {
                       
                            NpgsqlCommand = new NpgsqlCommand();
                            sQry = "SELECT \"RESTD_ID\",\"RESTD_TC_CODE\",to_char(\"RESTD_DATE\",'dd-mm-yyyy')RESTD_DATE,to_char(\"RESTD_DATE\",'dd-MON-yyyy')DF_MONTH_DATE";
                            sQry += ",\"RESTD_CAPACITY\",\"RESTD_FAIL_TYPE\",SUBSTR(CAST(\"RESTD_OFF_CODE\" AS TEXT),1,4)RESTD_OFF_CODE FROM \"TBLREPAIRERESTIMATIONDETAILS\"  WHERE \"RESTD_ID\"=:sFailId1";
                            NpgsqlCommand.Parameters.AddWithValue("sFailId1", Convert.ToInt32(sFailId));
                        
                    }
                    else
                    {
                        
                       
                            NpgsqlCommand = new NpgsqlCommand();
                            sQry = "SELECT \"RESTD_ID\",\"RESTD_TC_CODE\",to_char(\"RESTD_DATE\",'dd-mm-yyyy')RESTD_DATE,to_char(\"RESTD_DATE\",'dd-MON-yyyy')DF_MONTH_DATE";
                            sQry += ",\"RESTD_CAPACITY\",\"RESTD_FAIL_TYPE\",SUBSTR(CAST(\"RESTD_OFF_CODE\" AS TEXT),1,4)RESTD_OFF_CODE FROM \"TBLREPAIRERESTIMATIONDETAILS\"   INNER JOIN \"TBLREPAIRERWORKORDER\" ON \"RESTD_ID\"=\"RWO_EST_ID\"  WHERE \"RWO_EST_ID\"=:sFailId3";
                            NpgsqlCommand.Parameters.AddWithValue("sFailId3", Convert.ToInt32(sFailId));
                      
                    }

                }
                else
                {
                    
                        NpgsqlCommand = new NpgsqlCommand();
                        sQry = "SELECT \"RESTD_ID\",\"RESTD_TC_CODE\",to_char(\"RESTD_DATE\",'dd-mm-yyyy')RESTD_DATE,to_char(\"RESTD_DATE\",'dd-MON-yyyy')DF_MONTH_DATE";
                        sQry += ",\"RESTD_CAPACITY\",\"RESTD_FAIL_TYPE\",SUBSTR(CAST(\"RESTD_OFF_CODE\" AS TEXT),1,4)RESTD_OFF_CODE FROM \"TBLREPAIRERESTIMATIONDETAILS\" WHERE \"RESTD_ID\"=:sFailId5";
                        NpgsqlCommand.Parameters.AddWithValue("sFailId5", Convert.ToInt32(sFailId));
                    
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
        public string FailureId(string sWo_Slno, string sWoRecordID = "")
        {
            string sQry = string.Empty;
            try
            {
               
                if (sWo_Slno.Contains('-'))
                {
                    clsFormValues objForm = new clsFormValues();
                    NpgsqlCommand = new NpgsqlCommand();
                    sQry = "SELECT \"WO_RECORD_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\" in (SELECT \"WOA_PREV_APPROVE_ID\" FROM ";
                    sQry += " \"TBLWORKFLOWOBJECTS\",\"TBLWO_OBJECT_AUTO\" WHERE \"WO_INITIAL_ID\"=\"WOA_INITIAL_ACTION_ID\" and ";
                    sQry += " \"WO_BO_ID\" ='72' and \"WO_RECORD_ID\"=:sWo_Slno and \"WOA_BFM_ID\"='33')";
                    NpgsqlCommand.Parameters.AddWithValue("sWo_Slno", Convert.ToInt32(sWo_Slno));
                }
                else
                {
                    NpgsqlCommand = new NpgsqlCommand();
                    if (sWoRecordID.Contains('-'))
                    {
                        return sWo_Slno;
                    }
                    else
                    {
                        sQry = "SELECT \"RESTD_ID\" FROM \"TBLREPAIRERWORKORDER\",\"TBLREPAIRERESTIMATIONDETAILS\" WHERE  ";
                        sQry += " \"RESTD_ID\"=\"RWO_EST_ID\" AND \"RWO_SLNO\" =:sWo_Slno1";
                        NpgsqlCommand.Parameters.AddWithValue("sWo_Slno1", Convert.ToInt32(sWo_Slno));
                    }

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
            string sQry = string.Empty;
            try
            {
                if (sActionType == "V")
                {
                    NpgsqlCommand = new NpgsqlCommand();
                    sQry = "SELECT cast(\"EST_FAIL_TYPE\" as text)|| '~' ||cast(\"WO_SLNO\"as text) FROM \"TBLESTIMATIONDETAILS\",\"TBLWORKORDER\",\"TBLDTCFAILURE\" WHERE \"DF_ID\"=\"WO_DF_ID\" AND \"DF_ID\"=\"EST_FAILUREID\" AND \"WO_SLNO\" =:sWo_Slno";
                    NpgsqlCommand.Parameters.AddWithValue("sWo_Slno", Convert.ToInt32(sWo_Slno));
                }
                else
                {
                    NpgsqlCommand = new NpgsqlCommand();
                    sQry = "SELECT cast(\"EST_FAIL_TYPE\" as text)|| '~' ||cast(\"WO_SLNO\"as text) FROM \"TBLESTIMATIONDETAILS\",\"TBLWORKORDER\",\"TBLDTCFAILURE\" WHERE \"DF_ID\"=\"WO_DF_ID\" AND \"DF_ID\"=\"EST_FAILUREID\" AND \"WO_DF_ID\" =:sWo_Slno1";
                    NpgsqlCommand.Parameters.AddWithValue("sWo_Slno1", Convert.ToInt32(sWo_Slno));
                }

                return ObjCon.get_value(sQry, NpgsqlCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }

        public string getestimatedate(string estid)
        {
            try
            {
                string sQry = string.Empty;
                NpgsqlCommand = new NpgsqlCommand();
                sQry = " SELECT TO_CHAR(\"RESTD_DATE\",'dd/mm/yyyy') from \"TBLREPAIRERESTIMATIONDETAILS\" WHERE \"RWO_EST_ID\"=:estid ";
                NpgsqlCommand.Parameters.AddWithValue("estid", Convert.ToInt32(estid));
                return ObjCon.get_value(sQry, NpgsqlCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }

        public string getfailid(string sestid)
        {
            string sQry = string.Empty;
            string sWoDataId = string.Empty;
            NpgsqlCommand = new NpgsqlCommand();
            sQry = "SELECT DISTINCT \"WO_DATA_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_RECORD_ID\"=:sestid AND \"WO_BO_ID\"='72'";
            NpgsqlCommand.Parameters.AddWithValue("sestid", Convert.ToInt32(sestid));
            sWoDataId = ObjCon.get_value(sQry, NpgsqlCommand);
            return sWoDataId;
        }
       
        public string getFailureId(string swoSlno, string type)
        {
            string sQry = string.Empty;
            if (type == "1")
            {
                NpgsqlCommand = new NpgsqlCommand();
                sQry = "SELECT \"RESTD_ID\" FROM \"TBLREPAIRERESTIMATIONDETAILS\",\"TBLREPAIRERWORKORDER\" WHERE \"RESTD_ID\"=\"RWO_EST_ID\" and \"RWO_SLNO\"=:swoSlno";
                NpgsqlCommand.Parameters.AddWithValue("swoSlno", Convert.ToInt32(swoSlno));
            }
            else
            {
                NpgsqlCommand = new NpgsqlCommand();
                sQry = "SELECT DISTINCT \"WO_DATA_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\"='72' and \"WO_RECORD_ID\"=:swoSlno1";
                NpgsqlCommand.Parameters.AddWithValue("swoSlno1", Convert.ToInt32(swoSlno));
            }

            return ObjCon.get_value(sQry, NpgsqlCommand);
        }


        public DataTable GetWorkorderDetailsrepairer(string swoslno)
        {
            DataTable dt = new DataTable();
            try
            {
                string sQry = string.Empty;
                sQry = " SELECT * from \"TBLREPAIRERWORKORDER\" WHERE \"RWO_SLNO\"='"+ swoslno + "'";
                dt = ObjCon.FetchDataTable(sQry);
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
