using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;

namespace IIITS.DTLMS.BL
{
    public class clsBankReceiveDtr
    {
        public string strFormCode = "clsReceiveTrans";
        public string sInvoiceId { get; set; }
        public string sInvoiceNo { get; set; }
        public string sInvoiceDate { get; set; }
        public string sIndentNo { get; set; }
        public string sIndentDate { get; set; }
        public string sIndentId { get; set; }
        public string sRVNo { get; set; }
        public string sRemarks{ get; set; }
        public DataTable dtCapacity { get; set; }
        public DataTable dtDTRList { get; set; }
        public string sOfficeCode { get; set; }
        public string sUserId { get; set; }

        public string sClientIP { get; set; }
        public string sActionType { get; set; }
        public string sWFDataId { get; set; }
        public string sWFAutoId { get; set; }
        public string sWFO_id { get; set; }

        PGSqlConnection objcon = new PGSqlConnection(Constants.Password);


        public string[] SaveRVDetails(clsBankReceiveDtr obj)
        {
            string[] Arr = new string[2];
            try
            {
                string sQry = string.Empty;
                sQry = "UPDATE \"TBLBANKINVOICE\" SET \"BV_RV_NO\" = '" + obj.sRVNo + "', \"BV_RV_DATE\" = NOW(), \"BV_REMARKS\" = '" + obj.sRemarks + "' WHERE \"BV_ID\" = '" + obj.sInvoiceId + "'";
                sQry = sQry.Replace("'", "''");

                StringBuilder sbQuery = new StringBuilder();
                String sQry1 = string.Empty;
                String BM_ID = string.Empty;

                sQry1 = "  SELECT \"BOC_BM_ID\" FROM \"TBLBANKOFFICECODE\" WHERE \"BOC_BM_SUBDIV_CODE\" = '" + obj.sOfficeCode + "' ";
                BM_ID = objcon.get_value(sQry1);

                for (int i = 0; i < obj.dtDTRList.Rows.Count; i++)
                {
                    sQry1 = "UPDATE \"TBLTCMASTER\" SET \"TC_CURRENT_LOCATION\" = '5', \"TC_LOCATION_ID\" = '" + BM_ID + "'  ";
                    sQry1 += "WHERE \"TC_CODE\" = '" + obj.dtDTRList.Rows[i]["TC_CODE"] + "' ";
                    sbQuery.Append(sQry1);
                    sbQuery.Append(";");
                }

                sbQuery = sbQuery.Replace("'", "''");
                string sParam = "SELECT \"BV_ID\" FROM \"TBLBANKINVOICE\" WHERE  \"BV_ID\" = " + obj.sInvoiceId + "";

                clsApproval objApproval = new clsApproval();
                objApproval.sFormName = "ReceiveTransBank";
                objApproval.sOfficeCode = obj.sOfficeCode;
                objApproval.sClientIp = obj.sClientIP;
                objApproval.sCrby = obj.sUserId;
                objApproval.sQryValues = sQry + ";" + sbQuery;
                objApproval.sParameterValues = sParam;
                objApproval.sWFObjectId = obj.sWFO_id;
                objApproval.sMainTable = "TBLBANKINVOICE";
                objApproval.sDataReferenceId = obj.sInvoiceId;
                objApproval.sDescription = "Receive Transformer for Transformer Bank";
                objApproval.sRefOfficeCode = obj.sOfficeCode;

                objApproval.sColumnNames = "BV_RV_NO,BV_REMARKS";
                objApproval.sColumnValues = "" + obj.sRVNo + "," + obj.sRemarks + "";
                objApproval.sTableNames = "TBLBANKINVOICE";

                if (obj.sActionType == "M")
                {
                    objApproval.SaveWorkFlowData(objApproval);
                    obj.sWFDataId = objApproval.sWFDataId;
                }
                else
                {
                    objApproval.SaveWorkFlowData(objApproval);
                    objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow();
                    objApproval.SaveWorkflowObjects(objApproval);
                }

                Arr[0] = "0";
                Arr[1] = "Received Sucessfully";
                return Arr;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                Arr[0] = "1";
                Arr[1] = "Failed to Receive";
                return Arr;
            }
        }

    }
}
