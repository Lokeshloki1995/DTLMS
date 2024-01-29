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
   public  class clsPermanentRI
    {

        string strFormCode = "clsPermanentRI";
        public string sDecommId { get; set; }
        public string sFailureId { get; set; }
        public string sFailureDate { get; set; }
        public string sTRReading { get; set; }
        public string sRINo { get; set; }
        public string sRIDate { get; set; }
        public string sRVNo { get; set; }
        public string sRVDate { get; set; }
        public string sRemarks { get; set; }
        public string sStoreId { get; set; }
        public string sInvoiceId { get; set; }
        public string sCrby { get; set; }
        public string sDecommDate { get; set; }
        public string sManualRVACKNo { get; set; }

        public string sTasktype { get; set; }
        public string sOfficeCode { get; set; }
        public string sWorkOrderDate { get; set; }
        public string sComWorkOrder { get; set; }
        public string sDecomWorkOrder { get; set; }



        //RI
        public string sTCCode { get; set; }
        public string sTcSlno { get; set; }
        public string sTcMake { get; set; }
        public string sDate { get; set; }
        public string sOilQuantity { get; set; }
        public string sDTCCode { get; set; }
        public string sDTCId { get; set; }
        public string sFailureTCId { get; set; }
        public string sNewTCId { get; set; }
        public string sNewTCCode { get; set; }
        public string sTCId { get; set; }
        public string sOilQuantitySK { get; set; }

        // Workflow
        public string sFormName { get; set; }
        public string sClientIP { get; set; }
        public string sWFDataId { get; set; }
        public string sWFObjectId { get; set; }
        public string sActionType { get; set; }
        public string sWFAutoId { get; set; }

        //CR
        public string sCommentByStoreKeeper { get; set; }
        public string sCommentByStoreOfficer { get; set; }
        public string sStoreKeeperName { get; set; }
        public string sStoreOfficerName { get; set; }
        public string sApprovedDate { get; set; }
        public string sInventoryQty { get; set; }
        public string sDecommInventoryQty { get; set; }


        //CustOledbConnection objcon = new CustOledbConnection(Constants.Password);
        PGSqlConnection objcon = new PGSqlConnection(Constants.Password);

        public DataTable LoadAlreadyRI(clsPermanentRI objRIApp)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {


                NpgsqlCommand cmd = new NpgsqlCommand("sp_loadalready_permanent_ri");
                cmd.Parameters.AddWithValue("stasktype", objRIApp.sTasktype);
                cmd.Parameters.AddWithValue("sofficecode", objRIApp.sOfficeCode);
                dt = objcon.FetchDataTable(cmd);

                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }

        }

        NpgsqlCommand NpgsqlCommand;
        public string[] UpdateReplaceDetails(clsPermanentRI objReplace)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string[] Arr = new string[3];
            string strQry = string.Empty;
            try
            {
                NpgsqlCommand.Parameters.AddWithValue("FailureId",Convert.ToInt32( objReplace.sFailureId));
                strQry = " SELECT \"PEST_TC_CODE\"  FROM \"TBLPERMANENTESTIMATIONDETAILS\" WHERE \"PEST_ID\" =:FailureId";
                string sTCCode = objcon.get_value(strQry, NpgsqlCommand);

                //Workflow / Approval
                #region Workflow

                strQry = "UPDATE \"TBLPERMANENTTCREPLACE\" SET \"PTR_APPROVE_REMARKS\"='" + objReplace.sRemarks + "' , \"PTR_APPROVE_FLAG\"=1,\"PTR_OIL_QTY_BYSK\"='" + objReplace.sOilQuantitySK + "', ";
                strQry += " \"PTR_RV_NO\"='{0}', \"PTR_RV_DATE\" =TO_DATE('" + objReplace.sRVDate + "','dd/MM/yyyy'),";
                strQry += " \"PTR_APPROVED_DATE\" =NOW(),\"PTR_APPROVED_BY\" ='" + objReplace.sCrby + "', \"PTR_MANUAL_ACKRV_NO\" ='" + objReplace.sManualRVACKNo + "' WHERE \"PTR_ID\" ='" + objReplace.sDecommId + "'";

                strQry = strQry.Replace("'", "''");
                string strQry2 = string.Empty;


                if (objReplace.sTasktype == "1" || objReplace.sTasktype == "4")
                {
                    //Update TC Status in TC Master Table
                    strQry2 = "UPDATE \"TBLTCMASTER\" SET \"TC_STATUS\" =3 , \"TC_CURRENT_LOCATION\" =1, \"TC_UPDATED_EVENT\" ='PERMANENTDECOMM ENTRY',";
                    strQry2 += " \"TC_UPDATED_EVENT_ID\" ='" + objReplace.sFailureId + "', \"TC_LOCATION_ID\" ='" + clsStoreOffice.GetStoreID(objReplace.sOfficeCode) + "'";
                    strQry2 += " WHERE \"TC_CODE\" = '" + sTCCode + "'";

                    strQry2 = strQry2.Replace("'", "''");
                }
                else
                {
                    //Update TC Status in TC Master Table
                    strQry2 = "UPDATE \"TBLTCMASTER\" SET \"TC_STATUS\" =1 ,\"TC_CURRENT_LOCATION\" =1,\"TC_UPDATED_EVENT\" ='ENHANCEMENT ENTRY',";
                    strQry2 += " \"TC_UPDATED_EVENT_ID\" ='" + objReplace.sFailureId + "', \"TC_LOCATION_ID\" ='" + clsStoreOffice.GetStoreID(objReplace.sOfficeCode) + "' ";
                    strQry2 += " WHERE \"TC_CODE\" = '" + sTCCode + "'";

                    strQry2 = strQry2.Replace("'", "''");
                }

                string sParam = "SELECT ACKNUMBERPERMANENT(" + objReplace.sDecommId + ") ";

                //
                clsApproval objApproval = new clsApproval();
                objApproval.sFormName = objReplace.sFormName;
                objApproval.sRecordId = objReplace.sDecommId;
                objApproval.sOfficeCode = objReplace.sOfficeCode;
                objApproval.sClientIp = objReplace.sClientIP;
                objApproval.sCrby = objReplace.sCrby;
                objApproval.sWFObjectId = objReplace.sWFObjectId;
                objApproval.sDataReferenceId = objReplace.sDTCCode;
                objApproval.sWFAutoId = objReplace.sWFAutoId;

                objApproval.sQryValues = strQry + ";" + strQry2;
                objApproval.sParameterValues = sParam;
                objApproval.sMainTable = "TBLPERMANENTTCREPLACE";

                objApproval.sDescription = "RI Approval For DTr Code " + objReplace.sTCCode;

                objApproval.sRefOfficeCode = objcon.get_value("SELECT \"PEST_LOC_CODE\" FROM \"TBLPERMANENTESTIMATIONDETAILS\",\"TBLPERMANENTWORKORDER\",\"TBLPERMANENTTCREPLACE\" WHERE \"PEST_ID\"=\"PWO_PEF_ID\" AND \"PWO_SLNO\"=\"PTR_WO_SLNO\" AND \"PTR_ID\" ='" + objReplace.sDecommId + "'");

                objApproval.sColumnNames = "PTR_ID,PTR_OIL_QUNTY,PTR_APPROVE_REMARKS,PTR_APPROVE_FLAG,PTR_OIL_QTY_BYSK,PTR_RV_NO,PTR_RV_DATE,PTR_APPROVED_DATE,PTR_APPROVED_BY,PTR_MANUAL_ACKRV_NO,PTR_COMM_DATE";

                objApproval.sColumnValues = "" + objReplace.sDecommId + "," + objReplace.sOilQuantity + "," + objReplace.sRemarks + ",1," + objReplace.sOilQuantitySK + "," + objReplace.sRVNo + ",";
                objApproval.sColumnValues += "" + objReplace.sRVDate + ",NOW()," + objReplace.sCrby + "," + objReplace.sManualRVACKNo + "";

                objApproval.sTableNames = "TBLPERMANENTTCREPLACE";


                //Check for Duplicate Approval
                bool bApproveResult = objApproval.CheckDuplicateApprove(objApproval);
                if (bApproveResult == false)
                {
                    Arr[0] = "Selected Record Already Approved";
                    Arr[1] = "2";
                    Arr[2] = objApproval.sWFDataId;
                    return Arr;
                }

                if (objReplace.sActionType == "M")
                {
                    objApproval.SaveWorkFlowData(objApproval);
                    objReplace.sWFDataId = objApproval.sWFDataId;
                }
                else
                {
                    objApproval.SaveWorkFlowData(objApproval);
                    objReplace.sWFDataId = objApproval.sWFDataId;
                    //objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow();
                    objApproval.SaveWorkflowObjects(objApproval);
                }

                #endregion


                Arr[0] = "RI Approved Successfully";
                Arr[1] = "1";
                Arr[2] = objApproval.sWFDataId;
                return Arr;

            }
            catch (Exception ex)
            {
                //objcon.RollBack();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;

            }
        }

        public object GetFailureTCDetails(clsPermanentRI objRIAprrove)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();


                NpgsqlCommand cmd = new NpgsqlCommand("sp_getestimatetcdetails");
                cmd.Parameters.AddWithValue("sestid", objRIAprrove.sFailureId);
                dt = objcon.FetchDataTable(cmd);
                if (dt.Rows.Count > 0)
                {
                    objRIAprrove.sDecomWorkOrder = dt.Rows[0]["PWO_NO_DECOM"].ToString();
                    objRIAprrove.sTCCode = dt.Rows[0]["TC_CODE"].ToString();
                    objRIAprrove.sTcSlno = dt.Rows[0]["TC_SLNO"].ToString();
                    objRIAprrove.sTcMake = dt.Rows[0]["TM_NAME"].ToString();
                    objRIAprrove.sTCId = dt.Rows[0]["TC_ID"].ToString();
                    objRIAprrove.sDTCCode = dt.Rows[0]["PEST_DTC_CODE"].ToString();
                    objRIAprrove.sFailureDate = dt.Rows[0]["PEST_DATE"].ToString();
                    //objDecomm.sTcCapacity = dt.Rows[0]["TC_CAPACITY"].ToString();
                    NpgsqlCommand.Parameters.AddWithValue("DecommId",Convert.ToInt32( objRIAprrove.sDecommId));
                    objRIAprrove.sOilQuantity = objcon.get_value("SELECT \"PTR_OIL_QUNTY\" FROM \"TBLPERMANENTTCREPLACE\" WHERE \"PTR_ID\" =:DecommId", NpgsqlCommand);

                }
                return objRIAprrove;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objRIAprrove;
            }
        }

        #region Completion Report

        #endregion


        public clsPermanentRI GetDTCTCDetailsFromFailure(clsPermanentRI objRI)
        {
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();

          
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getdtctcdetailspermanentfromfailure");
                cmd.Parameters.AddWithValue("sestid", objRI.sFailureId);
                dt = objcon.FetchDataTable(cmd);
                if (dt.Rows.Count > 0)
                {
                    objRI.sDTCCode = Convert.ToString(dt.Rows[0]["PEST_DTC_CODE"]);
                    objRI.sTCCode = Convert.ToString(dt.Rows[0]["PEST_TC_CODE"]).Replace(".00","");
                    objRI.sFailureTCId = Convert.ToString(dt.Rows[0]["TC_ID"]);
                    objRI.sDTCId = Convert.ToString(dt.Rows[0]["DT_ID"]);
                   // objRI.sNewTCCode = Convert.ToString(dt.Rows[0]["TD_TC_NO"]);
                   // objRI.sNewTCId = Convert.ToString(dt.Rows[0]["TD_TC_ID"]);

                }
                return objRI;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objRI;
            }
        }

        public clsPermanentRI GetRIDetails(clsPermanentRI objRIApproval)
        {
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();


                NpgsqlCommand cmd = new NpgsqlCommand("sp_getridetails_permanent");
                cmd.Parameters.AddWithValue("sdecommid", objRIApproval.sDecommId);
                dt = objcon.FetchDataTable(cmd);

                if (dt.Rows.Count > 0)
                {
                    objRIApproval.sOilQuantity = Convert.ToString(dt.Rows[0]["PTR_OIL_QUNTY"]);
                    objRIApproval.sRemarks = Convert.ToString(dt.Rows[0]["PTR_APPROVE_REMARKS"]).Replace("ç", ",");
                    objRIApproval.sRVNo = Convert.ToString(dt.Rows[0]["PTR_RV_NO"]);
                    objRIApproval.sRVDate = Convert.ToString(dt.Rows[0]["PTR_RV_DATE"]);
                    objRIApproval.sOilQuantitySK = Convert.ToString(dt.Rows[0]["PTR_OIL_QTY_BYSK"]);
                    objRIApproval.sDecommDate = Convert.ToString(dt.Rows[0]["PTR_DECOMM_DATE"]);
                    objRIApproval.sManualRVACKNo = Convert.ToString(dt.Rows[0]["PTR_MANUAL_ACKRV_NO"]);
                }

                return objRIApproval;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objRIApproval;
            }
        }

        public void SendSMStoSectionOfficer(string sDTrNo, string sDecommId, string sFailureId)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();

                NpgsqlCommand.Parameters.AddWithValue("FailureId",Convert.ToInt32( sFailureId));
                string sOfficeCode = objcon.get_value("SELECT \"DF_LOC_CODE\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\" =:FailureId", NpgsqlCommand);
                NpgsqlCommand.Parameters.AddWithValue("DecommId",Convert.ToInt32( sDecommId));
                string sResult = objcon.get_value("SELECT \"SM_NAME\" || '~' || \"TR_RI_NO\" FROM \"TBLTCREPLACE\",\"TBLSTOREMAST\" WHERE \"TR_STORE_SLNO\"=\"SM_ID\" AND \"TR_ID\"=:DecommId", NpgsqlCommand);

                //strQry = "SELECT US_FULL_NAME,US_MOBILE_NO,US_EMAIL FROM TBLUSER WHERE US_ROLE_ID IN (4) AND US_OFFICE_CODE='" + sOfficeCode + "'";
                //dt = objcon.getDataTable(strQry);
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getsocontact");
                cmd.Parameters.AddWithValue("sofficecode", sOfficeCode);
                dt = objcon.FetchDataTable(cmd);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sFullName = Convert.ToString(dt.Rows[i]["US_FULL_NAME"]);
                    string sMobileNo = Convert.ToString(dt.Rows[i]["US_MOBILE_NO"]);

                    clsCommunication objcomm = new clsCommunication();
                    objcomm.sSMSkey = "SMStoRI";
                    objcomm = objcomm.GetsmsTempalte(objcomm);

                    string sSMSText = String.Format(objcomm.sSMSTemplate,
                        sResult.Split('~').GetValue(1).ToString(), sDTrNo, sResult.Split('~').GetValue(0).ToString());
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
                //return ex.Message;

            }
        }


        public string GenerateAckNo(string sOfficeCode)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;
                NpgsqlCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                string sAckNo = objcon.get_value("SELECT  COALESCE(MAX(CAST(\"PTR_RV_NO\" AS INT8)),0)+1  FROM \"TBLPERMANENTTCREPLACE\" WHERE \"PTR_RV_NO\" LIKE :OfficeCode||'%' ", NpgsqlCommand);
                if (sAckNo.Length == 1)
                {

                    //2 digit Division Code
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        sFinancialYear = System.DateTime.Now.ToString("yy");
                    }
                    else
                    {
                        sFinancialYear = System.DateTime.Now.AddYears(-1).ToString("yy");
                    }

                    sAckNo = sOfficeCode + sFinancialYear + "00001";
                }
                else
                {
                    //2 digit Division Code
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                        {
                            sFinancialYear = System.DateTime.Now.ToString("yy");
                        }
                        if (sFinancialYear == sAckNo.Substring(3, 2))
                        {
                            return sAckNo;
                        }
                        else
                        {
                            sAckNo = sOfficeCode + sFinancialYear + "00001";
                        }
                    }
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) < 03)
                    {
                        return sAckNo;
                    }


                }

                return sAckNo;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }

        #region WorkFlow XML

        public clsPermanentRI GetRIDetailsFromXML(clsPermanentRI objRIApproval)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable dt = new DataTable();

                dt = objApproval.GetDatatableFromXML(objRIApproval.sWFDataId);
                if (dt.Rows.Count > 0)
                {
                    objRIApproval.sOilQuantity = Convert.ToString(dt.Rows[0]["PTR_OIL_QUNTY"]);
                    objRIApproval.sRemarks = Convert.ToString(dt.Rows[0]["PTR_APPROVE_REMARKS"]);
                    objRIApproval.sRVNo = Convert.ToString(dt.Rows[0]["PTR_RV_NO"]);
                    objRIApproval.sRVDate = Convert.ToString(dt.Rows[0]["PTR_RV_DATE"]);
                    objRIApproval.sOilQuantitySK = Convert.ToString(dt.Rows[0]["PTR_OIL_QTY_BYSK"]);
                    objRIApproval.sCrby = Convert.ToString(dt.Rows[0]["PTR_APPROVED_BY"]);
                    objRIApproval.sManualRVACKNo = Convert.ToString(dt.Rows[0]["PTR_MANUAL_ACKRV_NO"]);
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






    }
}
