using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;


namespace IIITS.DTLMS.BL
{
    public class clsPermanentCR : clsPermanentRI
    {

        //CustOledbConnection objcon = new CustOledbConnection(Constants.Password);
        PGSqlConnection objcon = new PGSqlConnection(Constants.Password);

        string strFormCode = "clsPermanentRIApproval";
        public clsPermanentRI GetDetailsForCR(clsPermanentCR objRIApproval)
        {
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();


                NpgsqlCommand cmd = new NpgsqlCommand("sp_getdetailsforcr_permanent");
                cmd.Parameters.AddWithValue("sdecommid", sDecommId);
                dt = objcon.FetchDataTable(cmd);

                if (dt.Rows.Count > 0)
                {
                    objRIApproval.sWorkOrderDate = Convert.ToString(dt.Rows[0]["PWO_DATE"]);
                    objRIApproval.sComWorkOrder = Convert.ToString(dt.Rows[0]["PWO_NO"]);
                    objRIApproval.sDecomWorkOrder = Convert.ToString(dt.Rows[0]["PWO_NO_DECOM"]);
                    objRIApproval.sStoreKeeperName = Convert.ToString(dt.Rows[0]["STORE_KEEPER"]);
                    objRIApproval.sStoreOfficerName = Convert.ToString(dt.Rows[0]["STORE_OFFICER"]);
                    objRIApproval.sCommentByStoreKeeper = Convert.ToString(dt.Rows[0]["COMMENT_KEEPER"]);
                    objRIApproval.sCommentByStoreOfficer = Convert.ToString(dt.Rows[0]["COMMENT_OFFICER"]);
                    objRIApproval.sOilQuantity = Convert.ToString(dt.Rows[0]["PTR_OIL_QUNTY"]);
                    objRIApproval.sApprovedDate = Convert.ToString(dt.Rows[0]["PTR_APPROVED_DATE"]);
                    objRIApproval.sFailureId = Convert.ToString(dt.Rows[0]["PEST_ID"]);
                    objRIApproval.sRINo = Convert.ToString(dt.Rows[0]["PTR_RI_NO"]);
                    objRIApproval.sRIDate = Convert.ToString(dt.Rows[0]["PTR_RI_DATE"]);
                    objRIApproval.sInventoryQty = Convert.ToString(dt.Rows[0]["PTR_INVENTORY_QTY"]);
                    objRIApproval.sDecommInventoryQty = Convert.ToString(dt.Rows[0]["PTR_DECOM_INV_QTY"]);

                    GetDTCTCDetailsFromFailure(objRIApproval);
                }

                return objRIApproval;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objRIApproval;
            }
        }

        public string[] SaveCompletionReport(clsPermanentCR objRI)
        {
            string[] Arr = new string[2];
            string strQry = string.Empty;
            try
            {
                #region Workflow


                //string strQry1 = "UPDATE \"TBLDTCFAILURE\" SET \"DF_REPLACE_FLAG\" =1, \"DF_REP_DATE\" = NOW() WHERE \"DF_ID\" ='" + objRI.sFailureId + "' ";
                //strQry1 += " AND \"DF_REPLACE_FLAG\" =0";

               // strQry1 = strQry1.Replace("'", "''");

                string strQry2 = "UPDATE \"TBLPERMANENTTCREPLACE\" SET \"PTR_INVENTORY_QTY\" ='" + objRI.sInventoryQty + "',";
                strQry2 += " \"PTR_DECOM_INV_QTY\" ='" + objRI.sDecommInventoryQty + "' WHERE \"PTR_ID\" ='" + objRI.sDecommId + "'";

                strQry2 = strQry2.Replace("'", "''");

                //string strQry3 = "UPDATE \"TBLDTCMAST\" SET \"DT_TC_ID\" ='0' WHERE \"DT_CODE\" ='" + objRI.sDTCCode + "'";

                // strQry3 = strQry3.Replace("'", "''");

                clsApproval objApproval = new clsApproval();
                objApproval.sFormName = objRI.sFormName;
                objApproval.sRecordId = objRI.sDecommId;
                objApproval.sOfficeCode = objRI.sOfficeCode;
                objApproval.sClientIp = objRI.sClientIP;
                objApproval.sCrby = objRI.sCrby;
                objApproval.sWFObjectId = objRI.sWFObjectId;
                objApproval.sWFAutoId = objRI.sWFAutoId;
                objApproval.sDataReferenceId = objRI.sDTCCode;
                objApproval.sQryValues = strQry2;

                objApproval.sDescription = "PermanentCompletion Report For DTC Code " + objRI.sDTCCode;

                strQry = "SELECT \"PEST_LOC_CODE\" FROM \"TBLPERMANENTESTIMATIONDETAILS\", \"TBLPERMANENTWORKORDER\",\"TBLPERMANENTTCREPLACE\" WHERE \"PEST_ID\" = \"PWO_PEF_ID\" AND \"PWO_SLNO\" = \"PTR_WO_SLNO\" AND \"PTR_ID\"  ='" + objRI.sDecommId + "'";

                objApproval.sRefOfficeCode = objcon.get_value(strQry);
                //objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow();


                objApproval.sColumnNames = "PTR_ID,PTR_INVENTORY_QTY,PTR_DECOM_INV_QTY";
                objApproval.sColumnValues = "" + objRI.sDecommId + "," + objRI.sInventoryQty + "," + objRI.sDecommInventoryQty + "";
                objApproval.sTableNames = "TBLPERMANENTTCREPLACE";

                bool bResult = objApproval.CheckDuplicateApprove(objApproval);
                if (bResult == false)
                {
                    Arr[0] = "Selected Record Already Approved";
                    Arr[1] = "2";
                    return Arr;
                }

                //objApproval.SaveWorkflowObjects(objApproval);

                if (objRI.sActionType == "M")
                {
                    objApproval.SaveWorkFlowData(objApproval);
                    objRI.sWFDataId = objApproval.sWFDataId;
                }
                else
                {
                    objApproval.SaveWorkFlowData(objApproval);
                    objRI.sWFDataId = objApproval.sWFDataId;
                    //objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow();
                    objApproval.SaveWorkflowObjects(objApproval);
                }


                Arr[0] = "Approved Successfully";
                Arr[1] = "0";
                return Arr;
                #endregion
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }


        #region WorkFlow XML

        public clsPermanentCR GetCRDetailsFromXML(clsPermanentCR objRIApproval)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable dt = new DataTable();

                dt = objApproval.GetDatatableFromXML(objRIApproval.sWFDataId);
                if (dt.Rows.Count > 0)
                {
                    objRIApproval.sInventoryQty = Convert.ToString(dt.Rows[0]["PTR_INVENTORY_QTY"]);
                    objRIApproval.sDecommInventoryQty = Convert.ToString(dt.Rows[0]["PTR_DECOM_INV_QTY"]);

                }
                return objRIApproval;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objRIApproval;
            }
        }

        #endregion

        public DataTable GetCRDetails(string DtcCode)
        {
            DataTable dtCRDetails = new DataTable();
            string strQry = string.Empty;
            try
            {


                NpgsqlCommand cmd = new NpgsqlCommand("sp_getcrdetails_permanent");
                cmd.Parameters.AddWithValue("dtccode", DtcCode);
                dtCRDetails = objcon.FetchDataTable(cmd);

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtCRDetails;
            }
            return dtCRDetails;
        }

    }
}
