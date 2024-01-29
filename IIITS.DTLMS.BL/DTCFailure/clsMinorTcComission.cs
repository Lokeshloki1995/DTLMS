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
    public class clsMinorTcComission
    {
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
        public string sWONumber { get; set; }
        public string sFailureID { get; set; }
        public string sDTRCode { get; set; }
        public string sCapacity { get; set; }
        public string sReceivedate { get; set; }
        public string sRsd_id { get; set; }
        public string sDtcCode { get; set; }
        public string sOldDtcCode { get; set; }

        public string strFormCode = "clsMinorTcComission";

        PGSqlConnection objCon = new PGSqlConnection(Constants.Password);

        NpgsqlCommand NpgsqlCommand;
        public DataTable GetFailDetails(string sWO_SLNO)
        {
            DataTable dtFailDetails = new DataTable();
            string sQry = string.Empty;
            try
            {
                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Parameters.AddWithValue("sWO_SLNO", Convert.ToInt32(sWO_SLNO));
                sQry = " SELECT \"WO_SLNO\", \"WO_NO\",\"TC_SLNO\", \"WO_DATE\", \"US_FULL_NAME\", \"DF_ID\", \"EST_NO\", \"EST_CRON\", \"DT_NAME\", ";
                sQry += " \"DF_EQUIPMENT_ID\", \"TC_CAPACITY\", \"EST_REPAIRER\", \"RD_NEW_TCCODE\",\"DF_DTC_CODE\",to_char(\"RD_RECEIVE_DATE\",'dd/mm/yyyy')\"RD_RECEIVE_DATE\" FROM \"TBLWORKORDER\", \"TBLDTCFAILURE\", \"TBLESTIMATIONDETAILS\", ";
                sQry += " \"TBLUSER\", \"TBLDTCMAST\", \"TBLTCMASTER\", \"TBLRECEIVEDTR\" WHERE \"WO_DF_ID\" = \"DF_ID\" AND \"DF_ID\" = \"EST_FAILUREID\" AND \"WO_SLNO\"=\"RD_WO_SLNO\" AND ";
                sQry += " \"WO_CRBY\" = \"US_ID\" AND \"DF_DTC_CODE\" = \"DT_CODE\" AND \"RD_NEW_TCCODE\" = \"TC_CODE\" AND \"RD_ID\" = :sWO_SLNO";
                dtFailDetails = objCon.FetchDataTable(sQry, NpgsqlCommand);
                return dtFailDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtFailDetails;
            }
        }

        public clsMinorTcComission GetFailDetailsFromXML(clsMinorTcComission objComission)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable dtRepairDetails = new DataTable();

                dtRepairDetails = objApproval.GetDatatableFromXML(objComission.sWFO_id);
                if (dtRepairDetails.Rows.Count > 0)
                {
                    // objWorkOrder.sWOId = Convert.ToString(dtWODetails.Rows[0]["WO_SLNO"]).Trim();
                    objComission.sApproveComment = Convert.ToString(dtRepairDetails.Rows[0]["TMC_DESC"]).Trim();
                    objComission.sReceivedate = Convert.ToString(dtRepairDetails.Rows[0]["TMC_COM_DATE"]).Trim();
                }
                return objComission;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objComission;
            }
        }

        public string[] SaveDetails(clsMinorTcComission objComission) 
        {
            string[] Arr = new string[2];
            StringBuilder sbQuery = new StringBuilder();
            try
            {
                string sQry = string.Empty;
                sQry = "INSERT INTO \"TBLMNORCOMISSION\"(\"TMC_ID\", \"TMC_RD_ID\", \"TMC_CRBY\", \"TMC_DESC\", \"TMC_COM_DATE\")";
                sQry += " VALUES ('{0}','" + objComission.sRsd_id + "','" + objComission.sCrBy + "','" + objComission.sApproveComment.Trim() + "', ";
                sQry += " TO_DATE('" + objComission.sReceivedate + "','dd/MM/yyyy'))";               

                sbQuery.Append(sQry);
                sbQuery.Append(";");

                sQry = "UPDATE \"TBLDTCFAILURE\" SET \"DF_REPLACE_FLAG\"='1' WHERE \"DF_ID\"='"+ objComission.sFailureID + "' ";
                sbQuery.Append(sQry);
                sbQuery.Append(";");

                sQry = "UPDATE \"TBLDTCMAST\" SET \"DT_TC_ID\"='0' WHERE \"DT_CODE\"='"+ objComission.sOldDtcCode + "'";
                sbQuery.Append(sQry);
                sbQuery.Append(";");

                sQry = "UPDATE \"TBLDTCMAST\" SET \"DT_TC_ID\"='"+ objComission.sDTRCode + "' WHERE \"DT_CODE\"='" + objComission.sDtcCode + "'";
                sbQuery.Append(sQry);
                sbQuery.Append(";");

                sQry = "UPDATE \"TBLTCMASTER\" SET \"TC_STATUS\"='2', \"TC_LOCATION_ID\"='" + objComission.sOfficeCode + "',\"TC_CURRENT_LOCATION\"='2' WHERE \"TC_CODE\"='" + objComission.sDTRCode + "'";

                sbQuery.Append(sQry);
                sbQuery.Append(";");

                sbQuery = sbQuery.Replace("'", "''");

                string sParam = "SELECT COALESCE(MAX(\"TMC_ID\"),0)+1 FROM \"TBLMNORCOMISSION\"";

                clsApproval objApproval = new clsApproval();
                objApproval.sFormName = "MinorTCInvoice";
                // objApproval.sRecordId = objFailureDetails.sFailureId;
                objApproval.sOfficeCode = objComission.sOfficeCode;
                objApproval.sClientIp = objComission.sClientIP;
                objApproval.sCrby = objComission.sCrBy;
                objApproval.sQryValues =  sbQuery.ToString();
                objApproval.sParameterValues = sParam;
                objApproval.sMainTable = "TBLMNORCOMISSION";
                objApproval.sDataReferenceId = objComission.sRsd_id;
                objApproval.sDescription = objComission.sApproveComment;
                objApproval.sRefOfficeCode = objComission.sOfficeCode;
                objApproval.sWFObjectId = objComission.sWFOId;

                string sPrimaryKey = "{0}";

                objApproval.sColumnNames = "TMC_DESC,TMC_COM_DATE";
                objApproval.sColumnValues = "" + objComission.sApproveComment.Trim() + "," + objComission.sReceivedate + "";
                objApproval.sTableNames = "TBLMNORCOMISSION";


                objApproval.SaveWorkFlowData(objApproval);
                objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow();
                objApproval.SaveWorkflowObjects(objApproval);

                Arr[0] = "Saved Successfully";
                Arr[1] = "0";
                return Arr;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }

        public string GetOldDTCCode(string TCCode)
        {
            try
            {
                NpgsqlCommand = new NpgsqlCommand();
                string sQry = string.Empty;
                sQry = "SELECT \"DT_CODE\" FROM \"TBLDTCMAST\" WHERE \"DT_TC_ID\"=:TCCode";
                NpgsqlCommand.Parameters.AddWithValue("TCCode",Convert.ToDouble(TCCode));
                return objCon.get_value(sQry, NpgsqlCommand);
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }

    }
}
