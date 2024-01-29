using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;
using IIITS.DTLMS.BL.DataBase;

namespace IIITS.DTLMS.BL
{

    public class clsCRReport : clsRIApproval
    {
        //CustOledbConnection objcon = new CustOledbConnection(Constants.Password);
        PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
        DataBseConnection ObjBasCon = new DataBseConnection(Constants.Password);
        NpgsqlCommand NpgsqlCommand;
        string strFormCode = "clsCRReport";
        public clsRIApproval GetDetailsForCR(clsCRReport objRIApproval)
        {
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();

                NpgsqlCommand cmd = new NpgsqlCommand("sp_getdetailsforcr");
                cmd.Parameters.AddWithValue("sdecommid", sDecommId);
                dt = objcon.FetchDataTable(cmd);

                if (dt.Rows.Count > 0)
                {
                    objRIApproval.sWorkOrderDate = Convert.ToString(dt.Rows[0]["WO_DATE"]);
                    objRIApproval.sComWorkOrder = Convert.ToString(dt.Rows[0]["WO_NO"]);
                    objRIApproval.sDecomWorkOrder = Convert.ToString(dt.Rows[0]["WO_NO_DECOM"]);
                    objRIApproval.sStoreKeeperName = Convert.ToString(dt.Rows[0]["STORE_KEEPER"]);
                    objRIApproval.sStoreOfficerName = Convert.ToString(dt.Rows[0]["STORE_OFFICER"]);
                    objRIApproval.sCommentByStoreKeeper = Convert.ToString(dt.Rows[0]["COMMENT_KEEPER"]);
                    objRIApproval.sCommentByStoreOfficer = Convert.ToString(dt.Rows[0]["COMMENT_OFFICER"]);
                    objRIApproval.sOilQuantity = Convert.ToString(dt.Rows[0]["TR_OIL_QUNTY"]);
                    objRIApproval.sApprovedDate = Convert.ToString(dt.Rows[0]["TR_APPROVED_DATE"]);
                    objRIApproval.sFailureId = Convert.ToString(dt.Rows[0]["TD_DF_ID"]);
                    objRIApproval.sRINo = Convert.ToString(dt.Rows[0]["TR_RI_NO"]);
                    objRIApproval.sRIDate = Convert.ToString(dt.Rows[0]["TR_RI_DATE"]);
                    objRIApproval.sInventoryQty = Convert.ToString(dt.Rows[0]["TR_INVENTORY_QTY"]);
                    objRIApproval.sDecommInventoryQty = Convert.ToString(dt.Rows[0]["TR_DECOM_INV_QTY"]);
                    objRIApproval.sCRDate = Convert.ToString(dt.Rows[0]["TR_CR_DATE"]);

                    objRIApproval.sRVNo = Convert.ToString(dt.Rows[0]["TR_RV_NO"]);
                    objRIApproval.sRVDate = Convert.ToString(dt.Rows[0]["TR_RV_DATE"]);

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

        /// <summary>
        /// Save CompletionReport
        /// </summary>
        /// <param name="objRI"></param>
        /// <returns></returns>
        public string[] SaveCompletionReport(clsRIApproval objRI)
        {
            DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
            string[] Arr = new string[2];
            string strQry = string.Empty;
            NpgsqlCommand = new NpgsqlCommand();
            string QryKey = string.Empty;
            try
            {
                #region Workflow
                objDatabse.BeginTransaction();
                string strQry1 = "UPDATE \"TBLDTCFAILURE\" SET \"DF_REPLACE_FLAG\" =1, \"DF_REP_DATE\" = NOW() WHERE \"DF_ID\" ='" + objRI.sFailureId + "' ";
                strQry1 += " AND \"DF_REPLACE_FLAG\" =0";
                strQry1 = strQry1.Replace("'", "''");
                string strQry2 = "UPDATE \"TBLTCREPLACE\" SET \"TR_INVENTORY_QTY\" ='" + objRI.sInventoryQty + "',\"TR_CR_DATE\" = to_date('" + objRI.sCRDate + "','dd/MM/yyyy'),";
                strQry2 += " \"TR_DECOM_INV_QTY\" ='" + objRI.sDecommInventoryQty + "' WHERE \"TR_WO_SLNO\" ='" + objRI.sDecommId + "'";
                strQry2 = strQry2.Replace("'", "''");
                clsApproval objApproval = new clsApproval();
                objApproval.sFormName = objRI.sFormName;
                objApproval.sRecordId = objRI.sDecommId;
                objApproval.sOfficeCode = objRI.sOfficeCode;
                objApproval.sClientIp = objRI.sClientIP;
                objApproval.sCrby = objRI.sCrby;
                objApproval.sWFObjectId = objRI.sWFObjectId;
                objApproval.sWFAutoId = objRI.sWFAutoId;
                objApproval.sDataReferenceId = objRI.sDTCCode;
                objApproval.sQryValues = strQry1 + ";" + strQry2;
                objApproval.sBOId = "26";
                objApproval.sDescription = "Completion Report For DTC Code " + objRI.sDTCCode;

                #region old inline query
                //strQry = "SELECT \"DF_LOC_CODE\" FROM \"TBLDTCFAILURE\", \"TBLWORKORDER\", \"TBLINDENT\", \"TBLDTCINVOICE\" , \"TBLTCREPLACE\" WHERE \"DF_ID\" = \"WO_DF_ID\" ";
                //strQry += " AND \"WO_SLNO\" = \"TI_WO_SLNO\" AND \"TI_ID\"=\"IN_TI_NO\" AND \"IN_NO\"=\"TR_IN_NO\" AND \"TR_WO_SLNO\" =:sDecommId";
                //NpgsqlCommand.Parameters.AddWithValue("sDecommId", Convert.ToDouble(objRI.sDecommId));
                //objApproval.sRefOfficeCode = objDatabse.get_value(strQry, NpgsqlCommand);
                #endregion

                QryKey = "GET_DF_LOC_CODE";
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clscrreport");
                cmd.Parameters.AddWithValue("p_key", QryKey);
                cmd.Parameters.AddWithValue("p_value_1", Convert.ToString(objRI.sDecommId ?? ""));
                cmd.Parameters.AddWithValue("p_value_2", "");
                objApproval.sRefOfficeCode = objDatabse.StringGetValue(cmd);


                objApproval.sColumnNames = "TR_ID,TR_INVENTORY_QTY,TR_DECOM_INV_QTY,TR_CR_DATE";
                objApproval.sColumnValues = "" + objRI.sDecommId + "," + objRI.sInventoryQty + "," + objRI.sDecommInventoryQty + "," + objRI.sCRDate + "";
                objApproval.sTableNames = "TBLTCREPLACE";
                //bool bResult = objApproval.CheckDuplicateApprove(objApproval);  //Old method Call with out Begin and Commit.
                bool bResult = objApproval.CheckDuplicateApprove_Latest(objApproval, objDatabse);

                if (bResult == false)
                {
                    Arr[0] = "Selected Record Already Approved";
                    Arr[1] = "2";
                    return Arr;
                }
                if (objRI.sActionType == "M")
                {
                    //objApproval.SaveWorkFlowData(objApproval);  //Old method Call with out Begin and Commit.
                    objApproval.SaveWorkFlowData_Latest(objApproval, objDatabse);
                    objRI.sWFDataId = objApproval.sWFDataId;
                }
                else
                {
                    //objApproval.SaveWorkFlowData(objApproval);  //Old method Call with out Begin and Commit.
                    objApproval.SaveWorkFlowData_Latest(objApproval, objDatabse);
                    objRI.sWFDataId = objApproval.sWFDataId;
                    //objApproval.SaveWorkflowObjects(objApproval);   //Old method Call with out Begin and Commit.
                    objApproval.SaveWorkflowObjects_Latest(objApproval, objDatabse);
                }
                Arr[0] = "Approved Successfully";
                Arr[1] = "0";
                objDatabse.CommitTransaction();
                return Arr;
                #endregion
            }
            catch (Exception ex)
            {
                objDatabse.RollBackTrans();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                  System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                throw ex;
            }
        }


        #region WorkFlow XML

        public clsRIApproval GetCRDetailsFromXML(clsRIApproval objRIApproval)
        {
            try
            {

                clsApproval objApproval = new clsApproval();
                DataTable dt = new DataTable();

                dt = objApproval.GetDatatableFromXML(objRIApproval.sWFDataId);
                if (dt.Rows.Count > 0)
                {
                    objRIApproval.sInventoryQty = Convert.ToString(dt.Rows[0]["TR_INVENTORY_QTY"]);
                    objRIApproval.sDecommInventoryQty = Convert.ToString(dt.Rows[0]["TR_DECOM_INV_QTY"]);
                    objRIApproval.sCRDate = Convert.ToString(dt.Rows[0]["TR_CR_DATE"]);
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
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getcrdetails");
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

        public DataTable GetNewDTCDetails(string sDTCId)
        {
            NpgsqlCommand = new NpgsqlCommand();
            DataTable dtNewDTCDetails = new DataTable();
            string sStrQry = string.Empty;
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getnewdtcdetails");
                cmd.Parameters.AddWithValue("sdtcid", sDTCId);
                return objcon.FetchDataTable(cmd);

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtNewDTCDetails;
            }
        }

        public string CheckRIandInvDone(string stcreplaceID)
        {
            string QryKey = string.Empty;            
            string tcreplaceID = string.Empty;
            
            try
            {
                #region Old Inline queary
                //NpgsqlCommand = new NpgsqlCommand();
                //string sStrQry = string.Empty;
                //sStrQry += " SELECT \"TR_IN_NO\"  FROM \"TBLTCREPLACE\" WHERE \"TR_WO_SLNO\" = '" + stcreplaceID + "' ";
                //tcreplaceID = objcon.get_value(sStrQry, NpgsqlCommand);
                #endregion

                QryKey = "GET_TR_IN_NO";
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clscrreport");
                cmd.Parameters.AddWithValue("p_key", QryKey);
                cmd.Parameters.AddWithValue("p_value_1", Convert.ToString(stcreplaceID ?? ""));
                cmd.Parameters.AddWithValue("p_value_2", "");
                tcreplaceID = ObjBasCon.StringGetValue(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return tcreplaceID;
            }
            return tcreplaceID;
        }

    }
}
