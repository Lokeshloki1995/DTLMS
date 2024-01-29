using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IIITS.PGSQL.DAL;
using System.Data;
using Npgsql;


namespace IIITS.DTLMS.BL
{
    public class clsReceiveMinorTC
    {
        public string sWONumber { get; set; }
        public string sWODate { get; set; }
        public string sWOCrby { get; set; }
        public string sFailureID { get; set; }
        public string sESTNumber { get; set; }
        public string sESTDate { get; set; }
        public string sDTCName { get; set; }
        public string sOldDTRCode { get; set; }
        public string sOldDTRCapacity { get; set; }
        public string sRepairer { get; set; }
        public string sNewDTRCode { get; set; }
        public string sNewDTRCapacity { get; set; }
        public string sNewDTRMake { get; set; }
        public string sNewDTRSLNo { get; set; }
        public string sReceiveID { get; set; }
        public string sReceivedate { get; set; }
        public string sWoSlno { get; set; }
        public string sOfficeCode { get; set; }
        public string sCrby { get; set; }
        public string sClientIP { get; set; }
        public string sWFO_id { get; set; }
        public string sFormName { get; set; }
        public string sWFOId { get; set; }
        public string sWFAutoId { get; set; }
        public string sActionType { get; set; }
        public string sCrBy { get; set; }
        public string sWFDataId { get; set; }
        public string sApproveComment { get; set; }

        public string strFormCode = "clsReceiveMinorTC";

        PGSqlConnection objCon = new PGSqlConnection(Constants.Password);
        NpgsqlCommand NpgsqlCommand;
        public clsReceiveMinorTC LoadWODetails(clsReceiveMinorTC objReceive, string sView="")
        {
            DataTable dt = new DataTable();
            try
            {
                string sQry = string.Empty;
                NpgsqlCommand = new NpgsqlCommand();
                if(sView != "")
                {
                    //sQry = " SELECT \"WO_SLNO\", \"WO_NO\", \"WO_DATE\", \"US_FULL_NAME\", \"DF_ID\", \"EST_NO\", \"EST_CRON\", \"DT_NAME\", ";
                    //sQry += " \"DF_EQUIPMENT_ID\", \"TC_CAPACITY\", \"EST_REPAIRER\",\"RD_NEW_TCCODE\",TO_CHAR(\"RD_RECEIVE_DATE\",'dd-mm-yyyy') AS \"RD_RECEIVE_DATE\" FROM \"TBLWORKORDER\", \"TBLDTCFAILURE\", \"TBLESTIMATIONDETAILS\", ";
                    //sQry += " \"TBLUSER\", \"TBLDTCMAST\", \"TBLTCMASTER\",\"TBLRECEIVEDTR\" WHERE \"WO_DF_ID\" = \"DF_ID\" AND \"DF_ID\" = \"EST_FAILUREID\" AND ";
                    //sQry += " \"WO_CRBY\" = \"US_ID\" AND \"DF_DTC_CODE\" = \"DT_CODE\" AND \"DF_EQUIPMENT_ID\" = \"TC_CODE\" AND \"RD_WO_SLNO\"=\"WO_SLNO\" AND \"WO_SLNO\" =:sWoSlno ";
                    //NpgsqlCommand.Parameters.AddWithValue("sWoSlno",Convert.ToInt32(objReceive.sWoSlno));

                    sQry = " SELECT  DISTINCT \"WO_SLNO\", \"WO_NO\", \"WO_DATE\", \"US_FULL_NAME\", \"DF_ID\", \"EST_NO\", \"EST_CRON\", \"DT_NAME\",  \"DF_EQUIPMENT_ID\", \"TC_CAPACITY\", \"EST_REPAIRER\",";
                    sQry += "  \"RD_NEW_TCCODE\",TO_CHAR(\"RD_RECEIVE_DATE\",'dd-mm-yyyy') AS \"RD_RECEIVE_DATE\" FROM \"TBLWORKORDER\" inner join  \"TBLDTCFAILURE\" on \"WO_DF_ID\" = \"DF_ID\" inner join   \"TBLESTIMATIONDETAILS\"";
                    sQry += "   on   \"DF_ID\" = \"EST_FAILUREID\"  inner join  \"TBLUSER\" on  \"WO_CRBY\" = \"US_ID\" inner join  \"TBLDTCMAST\" on   \"DF_DTC_CODE\" = \"DT_CODE\"  inner  join \"TBLTCMASTER\" on \"DF_EQUIPMENT_ID\" = \"TC_CODE\" ";
                    sQry += "  left join  \"TBLRECEIVEDTR\" on \"RD_WO_SLNO\"=\"WO_SLNO\" WHERE   \"WO_SLNO\" =:sWoSlno ";
                    NpgsqlCommand.Parameters.AddWithValue("sWoSlno", Convert.ToInt32(objReceive.sWoSlno));

                    dt = objCon.FetchDataTable(sQry, NpgsqlCommand);
                    if (dt.Rows.Count > 0)
                    {
                        objReceive.sWONumber = Convert.ToString(dt.Rows[0]["WO_NO"]);
                        objReceive.sWoSlno = Convert.ToString(dt.Rows[0]["WO_SLNO"]);
                        objReceive.sWODate = Convert.ToDateTime(dt.Rows[0]["WO_DATE"]).ToString("dd/MM/yyyy");
                        objReceive.sWOCrby = Convert.ToString(dt.Rows[0]["US_FULL_NAME"]);
                        objReceive.sFailureID = Convert.ToString(dt.Rows[0]["DF_ID"]);
                        objReceive.sESTNumber = Convert.ToString(dt.Rows[0]["EST_NO"]);
                        objReceive.sESTDate = Convert.ToString(dt.Rows[0]["EST_CRON"]);
                        objReceive.sDTCName = Convert.ToString(dt.Rows[0]["DT_NAME"]);
                        objReceive.sOldDTRCode = Convert.ToString(dt.Rows[0]["DF_EQUIPMENT_ID"]);
                        objReceive.sOldDTRCapacity = Convert.ToString(dt.Rows[0]["TC_CAPACITY"]);
                        objReceive.sRepairer = Convert.ToString(dt.Rows[0]["EST_REPAIRER"]);
                        objReceive.sNewDTRCode = Convert.ToString(dt.Rows[0]["RD_NEW_TCCODE"]);
                        objReceive.sReceivedate = Convert.ToString(dt.Rows[0]["RD_RECEIVE_DATE"]);
                    }
                }
                else
                {
                    NpgsqlCommand = new NpgsqlCommand();
                    sQry = " SELECT \"WO_SLNO\", \"WO_NO\", \"WO_DATE\", \"US_FULL_NAME\", \"DF_ID\", \"EST_NO\", \"EST_CRON\", \"DT_NAME\", ";
                    sQry += " \"DF_EQUIPMENT_ID\", \"TC_CAPACITY\", \"EST_REPAIRER\" FROM \"TBLWORKORDER\", \"TBLDTCFAILURE\", \"TBLESTIMATIONDETAILS\", ";
                    sQry += " \"TBLUSER\", \"TBLDTCMAST\", \"TBLTCMASTER\" WHERE \"WO_DF_ID\" = \"DF_ID\" AND \"DF_ID\" = \"EST_FAILUREID\" AND ";
                    sQry += " \"WO_CRBY\" = \"US_ID\" AND \"DF_DTC_CODE\" = \"DT_CODE\" AND \"DF_EQUIPMENT_ID\" = \"TC_CODE\" AND \"WO_SLNO\" = :sWoSlno1";
                    NpgsqlCommand.Parameters.AddWithValue("sWoSlno1", Convert.ToInt32(objReceive.sWoSlno));
                    dt = objCon.FetchDataTable(sQry, NpgsqlCommand);
                    if (dt.Rows.Count > 0)
                    {
                        objReceive.sWONumber = Convert.ToString(dt.Rows[0]["WO_NO"]);
                        objReceive.sWoSlno = Convert.ToString(dt.Rows[0]["WO_SLNO"]);
                        objReceive.sWODate = Convert.ToDateTime(dt.Rows[0]["WO_DATE"]).ToString("dd/MM/yyyy"); 
                        objReceive.sWOCrby = Convert.ToString(dt.Rows[0]["US_FULL_NAME"]);
                        objReceive.sFailureID = Convert.ToString(dt.Rows[0]["DF_ID"]);
                        objReceive.sESTNumber = Convert.ToString(dt.Rows[0]["EST_NO"]);
                        objReceive.sESTDate = Convert.ToString(dt.Rows[0]["EST_CRON"]);
                        objReceive.sDTCName = Convert.ToString(dt.Rows[0]["DT_NAME"]);
                        objReceive.sOldDTRCode = Convert.ToString(dt.Rows[0]["DF_EQUIPMENT_ID"]);
                        objReceive.sOldDTRCapacity = Convert.ToString(dt.Rows[0]["TC_CAPACITY"]);
                        objReceive.sRepairer = Convert.ToString(dt.Rows[0]["EST_REPAIRER"]);
                    }
                }
                
                return objReceive;
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objReceive;
            }
        }

        public clsReceiveMinorTC LoadReceiveMinorTCDetails(clsReceiveMinorTC objReceive)
        {
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand = new NpgsqlCommand();
                string sQry = string.Empty;
                sQry = " SELECT \"WO_SLNO\", \"WO_NO\", TO_CHAR(\"WO_DATE\",'DD-MM-YYYY')\"WO_DATE\", \"US_FULL_NAME\", \"DF_ID\", \"EST_NO\", TO_CHAR(\"EST_CRON\",'DD-MM-YYYY')\"EST_CRON\", \"DT_NAME\",TO_CHAR(\"RD_RECEIVE_DATE\",'DD-MM-YYYY')\"RD_RECEIVE_DATE\" ,";
                sQry += " \"DF_EQUIPMENT_ID\", \"TC_CAPACITY\", \"EST_REPAIRER\", \"RD_NEW_TCCODE\" FROM \"TBLWORKORDER\", \"TBLDTCFAILURE\", \"TBLESTIMATIONDETAILS\",\"TBLRECEIVEDTR\", ";
                sQry += " \"TBLUSER\", \"TBLDTCMAST\", \"TBLTCMASTER\" WHERE \"WO_DF_ID\" = \"DF_ID\" AND \"DF_ID\" = \"EST_FAILUREID\" AND ";
                sQry += " \"WO_CRBY\" = \"US_ID\" AND \"DF_DTC_CODE\" = \"DT_CODE\" AND \"DF_EQUIPMENT_ID\" = \"TC_CODE\" AND \"RD_WO_SLNO\"=\"WO_SLNO\" AND \"RD_ID\" =:sWoSlno ";
                NpgsqlCommand.Parameters.AddWithValue("sWoSlno", Convert.ToInt32(objReceive.sWoSlno));
                dt = objCon.FetchDataTable(sQry, NpgsqlCommand);
                if (dt.Rows.Count > 0)
                {
                    objReceive.sWONumber = Convert.ToString(dt.Rows[0]["WO_NO"]);
                    objReceive.sWoSlno = Convert.ToString(dt.Rows[0]["WO_SLNO"]);
                    objReceive.sWODate = Convert.ToString(dt.Rows[0]["WO_DATE"]);
                    objReceive.sWOCrby = Convert.ToString(dt.Rows[0]["US_FULL_NAME"]);
                    objReceive.sFailureID = Convert.ToString(dt.Rows[0]["DF_ID"]);
                    objReceive.sESTNumber = Convert.ToString(dt.Rows[0]["EST_NO"]);
                    objReceive.sESTDate = Convert.ToString(dt.Rows[0]["EST_CRON"]);
                    objReceive.sDTCName = Convert.ToString(dt.Rows[0]["DT_NAME"]);
                    objReceive.sOldDTRCode = Convert.ToString(dt.Rows[0]["DF_EQUIPMENT_ID"]);
                    objReceive.sOldDTRCapacity = Convert.ToString(dt.Rows[0]["TC_CAPACITY"]);
                    objReceive.sRepairer = Convert.ToString(dt.Rows[0]["EST_REPAIRER"]);
                    objReceive.sNewDTRCode = Convert.ToString(dt.Rows[0]["RD_NEW_TCCODE"]);
                    objReceive.sReceivedate = Convert.ToString(dt.Rows[0]["RD_RECEIVE_DATE"]);
                }
                return objReceive;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objReceive;
            }
        }

        public clsReceiveMinorTC GetTCDetails(clsReceiveMinorTC objReceive)
        {
            DataTable dt = new DataTable();
            try
            {
                string sQry = string.Empty;
                NpgsqlCommand = new NpgsqlCommand();
                sQry = "SELECT \"TC_CODE\",\"TC_CAPACITY\", \"TM_NAME\", \"TC_SLNO\" FROM \"TBLTCMASTER\", \"TBLTRANSMAKES\" WHERE \"TM_ID\" = \"TC_MAKE_ID\" AND \"TC_CODE\" =:sNewDTRCode";
                NpgsqlCommand.Parameters.AddWithValue("sNewDTRCode",Convert.ToDouble(objReceive.sNewDTRCode));
                dt = objCon.FetchDataTable(sQry, NpgsqlCommand);
                if(dt.Rows.Count>0)
                {
                    objReceive.sNewDTRCode = Convert.ToString(dt.Rows[0]["TC_CODE"]);
                    objReceive.sNewDTRCapacity = Convert.ToString(dt.Rows[0]["TC_CAPACITY"]);
                    objReceive.sNewDTRMake = Convert.ToString(dt.Rows[0]["TM_NAME"]);
                    objReceive.sNewDTRSLNo = Convert.ToString(dt.Rows[0]["TC_SLNO"]);
                }
                return objReceive;
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objReceive;
            }
        }


        public string[] SaveDetails(clsReceiveMinorTC objReceive)
        {
            string[] Arr = new string[2];
            try
            {
                string sQry = string.Empty;
                sQry = "INSERT INTO \"TBLRECEIVEDTR\"(\"RD_ID\", \"RD_WO_SLNO\", \"RD_OLD_TCCODE\", \"RD_NEW_TCCODE\", \"RD_RECEIVE_DATE\", \"RD_CRBY\")";
                sQry += " VALUES ('{0}','" + objReceive.sWoSlno + "','" + objReceive.sOldDTRCode + "','" + objReceive.sNewDTRCode + "', ";
                sQry += " TO_DATE('" + objReceive.sReceivedate + "','dd/MM/yyyy'), '" + objReceive.sCrby + "' )";

                sQry = sQry.Replace("'", "''");

                string sParam = "SELECT COALESCE(MAX(\"RD_ID\"),0)+1 FROM \"TBLRECEIVEDTR\"";

                clsApproval objApproval = new clsApproval();
                //objApproval.sFormName = "ReceiveMinorTC";
                // objApproval.sRecordId = objFailureDetails.sFailureId;

                objApproval.sFormName = objReceive.sFormName;
                
                objApproval.sOfficeCode = objReceive.sOfficeCode;
                objApproval.sClientIp = objReceive.sClientIP;
                objApproval.sCrby = objReceive.sCrby;
                objApproval.sQryValues = sQry ;
                objApproval.sParameterValues = sParam;
                objApproval.sMainTable = "TBLRECEIVEDTR";
                objApproval.sDataReferenceId = objReceive.sWoSlno;
                objApproval.sDescription = "Receive TC from the Repairer " + objReceive.sRepairer;
                objApproval.sRefOfficeCode = objReceive.sOfficeCode;
                objApproval.sWFObjectId = objReceive.sWFOId;

                string sPrimaryKey = "{0}";

                objApproval.sColumnNames = "RD_NEW_TCCODE,RD_RECEIVE_DATE,RD_MAKE,RD_CAPACITY,RD_SLNO";
                objApproval.sColumnValues = "" + objReceive.sNewDTRCode + "," + objReceive.sReceivedate + "," + objReceive.sNewDTRMake + "," + objReceive.sNewDTRCapacity + "," + objReceive.sNewDTRSLNo + "";
                objApproval.sTableNames = "TBLRECEIVEDTR";

                if (objReceive.sActionType == "M")
                {
                    objApproval.SaveWorkFlowData(objApproval);
                    objReceive.sWFDataId = objApproval.sWFDataId;
                }
                else
                {
                    objApproval.SaveWorkFlowData(objApproval);
                    objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow();
                    objApproval.SaveWorkflowObjects(objApproval);
                    objReceive.sWFDataId = objApproval.sWFDataId;
                }
                //objApproval.SaveWorkFlowData(objApproval);
                //objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow();
                //objApproval.SaveWorkflowObjects(objApproval);

                Arr[0] = "Saved Successfully";
                Arr[1] = "0";
                return Arr;
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }        

        public clsReceiveMinorTC GetWODetailsFromXML(clsReceiveMinorTC objReceive)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable dtRepairDetails = new DataTable();
                if(objReceive.sWFO_id != "")
                {
                    dtRepairDetails = objApproval.GetDatatableFromXML(objReceive.sWFO_id);
                    if (dtRepairDetails.Rows.Count > 0)
                    {
                        // objWorkOrder.sWOId = Convert.ToString(dtWODetails.Rows[0]["WO_SLNO"]).Trim();
                        objReceive.sNewDTRCode = Convert.ToString(dtRepairDetails.Rows[0]["RD_NEW_TCCODE"]).Trim();
                        objReceive.sReceivedate = Convert.ToString(dtRepairDetails.Rows[0]["RD_RECEIVE_DATE"]).Trim();
                        objReceive.sNewDTRMake = Convert.ToString(dtRepairDetails.Rows[0]["RD_MAKE"]).Trim();
                        objReceive.sNewDTRCapacity = Convert.ToString(dtRepairDetails.Rows[0]["RD_CAPACITY"]).Trim();
                        objReceive.sNewDTRSLNo = Convert.ToString(dtRepairDetails.Rows[0]["RD_SLNO"]).Trim();
                    }
                }
                
                return objReceive;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objReceive;
            }
        }
        public string GetWOSLID(string sWO_ID)
        {
            string StrQry = string.Empty;
            try
            {
                NpgsqlCommand = new NpgsqlCommand();
                StrQry = "SELECT \"WO_DATA_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\"=:sWO_ID";
                NpgsqlCommand.Parameters.AddWithValue("sWO_ID", Convert.ToInt32(sWO_ID));
                return objCon.get_value(StrQry, NpgsqlCommand);
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }

        public string GetWOSLNO(string sID)
        {
            string StrQry = string.Empty;
            try
            {
                NpgsqlCommand = new NpgsqlCommand();
                StrQry = "SELECT \"RD_WO_SLNO\" FROM \"TBLRECEIVEDTR\" WHERE \"RD_ID\"=:sID";
                NpgsqlCommand.Parameters.AddWithValue("sID", Convert.ToInt32(sID));
                return objCon.get_value(StrQry, NpgsqlCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }
    }
}
