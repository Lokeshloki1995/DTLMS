using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;
using IIITS.DTLMS;
using System.Configuration;
using IIITS.DTLMS.BL.DataBase;

namespace IIITS.DTLMS.BL
{
    public class clsRIApproval
    {
        DataBseConnection ObjBasCon = new DataBseConnection(Constants.Password);

        string strFormCode = "clsRIApproval";
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
        public string sBarrels { get; set; }

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
        public string sCRDate { get; set; }
        public string sOilQntyTank { get; set; }
        public string sOilQnty { get; set; }
        public string sBo_id { get; set; }

        public string sCreditWorkOrder { get; set; }


        //CustOledbConnection objcon = new CustOledbConnection(Constants.Password);
        PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
        NpgsqlCommand NpgsqlCommand;
        public DataTable LoadAlreadyRI(clsRIApproval objRIApp)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                //strQry = "SELECT DF_ID,DT_NAME,TR_ID,DT_TC_ID,(SELECT TC_SLNO FROM TBLTCMASTER WHERE DT_TC_ID = TC_CODE) TC_SLNO,TI_INDENT_NO,IN_INV_NO,IN_NO,TR_ID,'YES' AS STATUS,DT_TC_ID FROM TBLDTCMAST,TBLDTCFAILURE,";
                //strQry += " TBLWORKORDER,TBLINDENT, TBLDTCINVOICE,TBLTCREPLACE";
                //strQry += " WHERE DT_CODE = DF_DTC_CODE  AND DF_ID = WO_DF_ID AND WO_SLNO = TI_WO_SLNO AND TI_ID = IN_TI_NO AND ";
                //strQry += " DF_STATUS_FLAG='" + objRIApp.sTasktype + "' AND TR_IN_NO=IN_NO AND TR_APPROVE_FLAG=1";
                //strQry += " AND DF_LOC_CODE LIKE '" + objRIApp.sOfficeCode + "%'";
                //dt = objcon.getDataTable(strQry);

                NpgsqlCommand cmd = new NpgsqlCommand("sp_loadalreadyri");
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

        //public int checkapprovallevel (clsRIApproval objRIApproval)
        //{
        //    string level= objcon.get_value("select * from \"TBLWORKFLOWOBJECTS\" where \"WO_ID\"='objRIApproval.sWFObjectId' and \"WO_NEXT_ROLE\"!='5'");
        //    NpgsqlCommand.Parameters.AddWithValue("sFailureId", Convert.ToInt32(objRIApproval.sWFObjectId));

        //    //bjRIApproval.sWFObjectId
        //    if (level == "")
        //        return 1;
        //}
        public string checkapprovallevel(string val)
        {
            string QryKey = string.Empty;
            string v = string.Empty;
            try
            {
                #region Old Inline queary 
                ////  return sbfm_id = objcon.get_value("SELECT \"BFM_ID\" from \"TBLBO_FLOW_MASTER\" WHERE \"BFM_NEXT_BO_ID\"='" + val + "' ");
                //v = objcon.get_value("select * from \"TBLWORKFLOWOBJECTS\" where \"WO_ID\"='" + val + "' and \"WO_NEXT_ROLE\"='5' and \"WO_BO_ID\"='15'");
                #endregion

                QryKey = "GET_CHECKAPPROVALLEVEL";
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsriapproval");
                cmd.Parameters.AddWithValue("p_key", QryKey);
                cmd.Parameters.AddWithValue("p_value_1", Convert.ToString(val ?? ""));
                cmd.Parameters.AddWithValue("p_value_2", "");
                v = ObjBasCon.StringGetValue(cmd);

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }

            if (v == "")
                return "0";
            else
                return "5";
        }

        /// <summary>
        /// Update Replace Details
        /// </summary>
        /// <param name="objReplace"></param>
        /// <returns></returns>
        public string[] UpdateReplaceDetails(clsRIApproval objReplace)
        {
            DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
            string[] Arr = new string[3];
            string strQry = string.Empty;
            string sTCCode = string.Empty;
            string QryKey = string.Empty;
            try
            {
                objDatabse.BeginTransaction();
                if (objReplace.sOfficeCode.Length > 3)
                {
                    objReplace.sOfficeCode = objReplace.sOfficeCode.Substring(0, 3);
                }
                #region old inline query
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = " SELECT \"DF_EQUIPMENT_ID\"  FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\" =:sFailureId";
                //NpgsqlCommand.Parameters.AddWithValue("sFailureId", Convert.ToInt32(objReplace.sFailureId));
                //sTCCode = objDatabse.get_value(strQry, NpgsqlCommand);
                #endregion

                QryKey = "GET_DF_EQUIPMENT_ID";
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsriapproval");
                cmd.Parameters.AddWithValue("p_key", QryKey);
                cmd.Parameters.AddWithValue("p_value_1", Convert.ToString(objReplace.sFailureId ?? ""));
                cmd.Parameters.AddWithValue("p_value_2", "");
                sTCCode = ObjBasCon.StringGetValue(cmd);

                //Workflow / Approval
                #region Workflow
                strQry = "UPDATE \"TBLTCREPLACE\" SET \"TR_APPROVE_REMARKS\"='" + objReplace.sRemarks + "' , \"TR_APPROVE_FLAG\"=1,\"TR_OIL_QTY_BYSK\"='" + objReplace.sOilQuantitySK + "', ";
                strQry += " \"TR_RV_NO\"='" + objReplace.sRVNo + "', \"TR_RV_DATE\" =TO_DATE('" + objReplace.sRVDate + "','dd/MM/yyyy'),";
                strQry += " \"TR_OIL_TANK_QTY\"='" + objReplace.sOilQntyTank + "', \"TR_QUANTITY_OIL\" ='" + objReplace.sOilQnty + "',";
                strQry += " \"TR_APPROVED_DATE\" =NOW(),\"TR_APPROVED_BY\" ='" + objReplace.sCrby + "', \"TR_MANUAL_ACKRV_NO\" ='" + objReplace.sManualRVACKNo + "' WHERE \"TR_WO_SLNO\" ='" + objReplace.sDecommId + "'";
                strQry = strQry.Replace("'", "''");
                string strQry2 = string.Empty;
                if (objReplace.sTasktype == "1" || objReplace.sTasktype == "4")
                {
                    if (objReplace.sOfficeCode == Convert.ToString(ConfigurationManager.AppSettings["NEWDIV"]))
                    {
                        string storeids = objDatabse.get_value("SELECT \"STO_SM_ID\" from \"TBLSTOREOFFCODE\" WHERE \"STO_OFF_CODE\"='" + objReplace.sRVNo.Substring(0, 3) + "'");
                        string officecode = objDatabse.get_value("SELECT \"STO_OFF_CODE\" from \"TBLSTOREOFFCODE\" WHERE \"STO_SM_ID\"='" + System.Web.HttpContext.Current.Session["sOfficeCode"].ToString() + "'");
                        //Update TC Status in TC Master Table
                        if (storeids != System.Web.HttpContext.Current.Session["sOfficeCode"].ToString())
                        {
                            strQry2 = "UPDATE \"TBLTCMASTER\" SET \"TC_STATUS\" =3 , \"TC_CURRENT_LOCATION\" =1, \"TC_UPDATED_EVENT\" ='FAILURE ENTRY',";
                            strQry2 += " \"TC_UPDATED_EVENT_ID\" ='" + objReplace.sFailureId + "', \"TC_LOCATION_ID\" ='" + clsStoreOffice.GetStoreID(officecode) + "',\"TC_STORE_ID\" ='" + clsStoreOffice.GetStoreID(officecode) + "'";
                            strQry2 += " WHERE \"TC_CODE\" = '" + sTCCode + "'";
                            strQry2 = strQry2.Replace("'", "''");
                        }
                        else
                        {
                            strQry2 = "UPDATE \"TBLTCMASTER\" SET \"TC_STATUS\" =3 , \"TC_CURRENT_LOCATION\" =1, \"TC_UPDATED_EVENT\" ='FAILURE ENTRY',";
                            strQry2 += " \"TC_UPDATED_EVENT_ID\" ='" + objReplace.sFailureId + "', \"TC_LOCATION_ID\" ='" + clsStoreOffice.GetStoreID(objReplace.sOfficeCode) + "',\"TC_STORE_ID\" ='" + clsStoreOffice.GetStoreID(objReplace.sOfficeCode) + "'";
                            strQry2 += " WHERE \"TC_CODE\" = '" + sTCCode + "'";
                            strQry2 = strQry2.Replace("'", "''");
                        }
                    }
                    else
                    {
                        strQry2 = "UPDATE \"TBLTCMASTER\" SET \"TC_STATUS\" =3 , \"TC_CURRENT_LOCATION\" =1, \"TC_UPDATED_EVENT\" ='FAILURE ENTRY',";
                        strQry2 += " \"TC_UPDATED_EVENT_ID\" ='" + objReplace.sFailureId + "', \"TC_LOCATION_ID\" ='" + clsStoreOffice.GetStoreID(objReplace.sOfficeCode) + "',\"TC_STORE_ID\" ='" + clsStoreOffice.GetStoreID(objReplace.sOfficeCode) + "'";
                        strQry2 += " WHERE \"TC_CODE\" = '" + sTCCode + "'";
                        strQry2 = strQry2.Replace("'", "''");
                    }
                }
                else
                {
                    //Update TC Status in TC Master Table
                    strQry2 = "UPDATE \"TBLTCMASTER\" SET \"TC_STATUS\" =1 ,\"TC_CURRENT_LOCATION\" =1,\"TC_UPDATED_EVENT\" ='ENHANCEMENT ENTRY',";
                    strQry2 += " \"TC_UPDATED_EVENT_ID\" ='" + objReplace.sFailureId + "', \"TC_LOCATION_ID\" ='" + clsStoreOffice.GetStoreID(objReplace.sOfficeCode) + "',\"TC_STORE_ID\" ='" + clsStoreOffice.GetStoreID(objReplace.sOfficeCode) + "' ";
                    strQry2 += " WHERE \"TC_CODE\" = '" + sTCCode + "'";
                    strQry2 = strQry2.Replace("'", "''");
                }
                string sParam = "SELECT ACKNUMBER(" + objReplace.sDecommId + ") ";
                clsApproval objApproval = new clsApproval();
                objApproval.sFormName = objReplace.sFormName;
                objApproval.sRecordId = objReplace.sDecommId;
                objApproval.sOfficeCode = objReplace.sOfficeCode;
                objApproval.sClientIp = objReplace.sClientIP;
                objApproval.sCrby = objReplace.sCrby;
                objApproval.sWFObjectId = objReplace.sWFObjectId;
                objApproval.sDataReferenceId = objReplace.sDTCCode;
                objApproval.sWFAutoId = objReplace.sWFAutoId;
                objApproval.sBOId = "15";
                objApproval.sQryValues = strQry + ";" + strQry2;
                objApproval.sParameterValues = sParam;
                objApproval.sMainTable = "TBLTCREPLACE";
                string storeid = objDatabse.get_value("SELECT \"STO_SM_ID\" from \"TBLSTOREOFFCODE\" WHERE \"STO_OFF_CODE\"='" + objReplace.sRVNo.Substring(0, 3) + "'");
                if (storeid != System.Web.HttpContext.Current.Session["sOfficeCode"].ToString())
                {
                    objApproval.sStatus = "1";
                }
                objApproval.sDescription = "RI Approval For DTr Code " + objReplace.sTCCode;
                objApproval.sRefOfficeCode = objDatabse.get_value("SELECT \"DF_LOC_CODE\" FROM \"TBLDTCFAILURE\",\"TBLWORKORDER\" WHERE \"DF_ID\"=\"WO_DF_ID\" AND \"DF_ID\"='" + objReplace.sFailureId + "'");
                objApproval.sColumnNames = "TR_ID,TR_OIL_QUNTY,TR_APPROVE_REMARKS,TR_APPROVE_FLAG,TR_OIL_QTY_BYSK,TR_RV_NO,TR_RV_DATE,TR_APPROVED_DATE,TR_APPROVED_BY,TR_MANUAL_ACKRV_NO,TR_COMM_DATE,TR_NO_OF_BARRELS,TR_OIL_TANK_QTY,TR_QUANTITY_OIL";
                objApproval.sColumnValues = "" + objReplace.sDecommId + "," + objReplace.sOilQuantity + "," + objReplace.sRemarks + ",1," + objReplace.sOilQuantitySK + "," + objReplace.sRVNo + ",";
                objApproval.sColumnValues += "" + objReplace.sRVDate + ",NOW()," + objReplace.sCrby + "," + objReplace.sManualRVACKNo + "," + objReplace.sBarrels + "," + objReplace.sOilQntyTank + "," + objReplace.sOilQnty + "";
                objApproval.sTableNames = "TBLTCREPLACE";

                //bool bApproveResult = objApproval.CheckDuplicateApprove(objApproval);   //Check for Duplicate Approval
                bool bApproveResult = objApproval.CheckDuplicateApprove_Latest(objApproval, objDatabse);   //Check for Duplicate Approval
                if (bApproveResult == false)
                {
                    Arr[0] = "Selected Record Already Approved";
                    Arr[1] = "2";
                    Arr[2] = objApproval.sWFDataId;
                    return Arr;
                }
                if (objReplace.sActionType == "M")
                {
                    //objApproval.SaveWorkFlowData(objApproval);   //Old method Call with out Begin and Commit.
                    objApproval.SaveWorkFlowData_Latest(objApproval, objDatabse);
                    objReplace.sWFDataId = objApproval.sWFDataId;
                }
                else
                {
                    //objApproval.SaveWorkFlowData(objApproval);    //Old method Call with out Begin and Commit.
                    objApproval.SaveWorkFlowData_Latest(objApproval, objDatabse);
                    objReplace.sWFDataId = objApproval.sWFDataId;
                    //objApproval.SaveWorkflowObjects(objApproval); //Old method Call with out Begin and Commit.
                    objApproval.SaveWorkflowObjects_Latest(objApproval, objDatabse);
                }
                #endregion             
                Arr[0] = "RI Approved Successfully";
                Arr[1] = "1";
                Arr[2] = objApproval.sWFDataId;
                objDatabse.CommitTransaction();
                return Arr;
            }
            catch (Exception ex)
            {
                objDatabse.RollBackTrans();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                  System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public object GetFailureTCDetails(clsRIApproval objRIAprrove)
        {
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();

                //strQry = "SELECT TC_ID,TC_SLNO,TC_CODE,WO_NO_DECOM,TO_CHAR(TC_CAPACITY) TC_CAPACITY,(SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID)  TM_NAME,DF_DTC_CODE, ";
                //strQry += " TO_CHAR(DF_DATE,'DD/MM/YYYY') AS DF_DATE FROM TBLTCMASTER,TBLDTCFAILURE,TBLWORKORDER WHERE DF_EQUIPMENT_ID=TC_CODE AND DF_ID=WO_DF_ID AND ";
                //strQry += " DF_ID ='" + objRIAprrove.sFailureId + "'";
                //dt = objcon.getDataTable(strQry);

                NpgsqlCommand cmd = new NpgsqlCommand("sp_getfailuretcdetails");
                cmd.Parameters.AddWithValue("sfailureid", objRIAprrove.sFailureId);
                dt = objcon.FetchDataTable(cmd);
                if (dt.Rows.Count > 0)
                {
                    objRIAprrove.sDecomWorkOrder = dt.Rows[0]["WO_NO_DECOM"].ToString();
                    objRIAprrove.sCreditWorkOrder = dt.Rows[0]["WO_NO_CREDIT"].ToString();
                    objRIAprrove.sTCCode = dt.Rows[0]["TC_CODE"].ToString();
                    objRIAprrove.sTcSlno = dt.Rows[0]["TC_SLNO"].ToString();
                    objRIAprrove.sTcMake = dt.Rows[0]["TM_NAME"].ToString();
                    objRIAprrove.sTCId = dt.Rows[0]["TC_ID"].ToString();
                    objRIAprrove.sDTCCode = dt.Rows[0]["DF_DTC_CODE"].ToString();
                    objRIAprrove.sFailureDate = dt.Rows[0]["DF_DATE"].ToString();
                    objRIAprrove.sOilQntyTank = dt.Rows[0]["DF_OIL_QUANTITY_TANK"].ToString();
                    objRIAprrove.sOilQnty = dt.Rows[0]["DF_OIL_QNTY"].ToString();
                    objRIAprrove.sRINo = dt.Rows[0]["ri_no"].ToString();
                    objRIAprrove.sRIDate = dt.Rows[0]["ri_date"].ToString();
                    //objDecomm.sTcCapacity = dt.Rows[0]["TC_CAPACITY"].ToString();
                    //             //objRIAprrove.sOilQuantity = objcon.get_value("SELECT \"TR_OIL_QUNTY\" FROM \"TBLTCREPLACE\" WHERE \"TR_ID\" ='" + objRIAprrove.sDecommId + "'");

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


        public clsRIApproval GetDTCTCDetailsFromFailure(clsRIApproval objRI)
        {
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();

                //strQry = "SELECT DF_DTC_CODE,DF_EQUIPMENT_ID,(SELECT DT_ID FROM TBLDTCMAST WHERE DF_DTC_CODE=DT_CODE) DT_ID,";
                //strQry += " (SELECT TC_ID FROM TBLTCMASTER WHERE TC_CODE=DF_EQUIPMENT_ID) TC_ID,TD_TC_NO,(SELECT TC_ID FROM TBLTCMASTER WHERE TC_CODE=TD_TC_NO) TD_TC_ID";
                //strQry += " FROM TBLDTCFAILURE,TBLTCDRAWN WHERE DF_ID=TD_DF_ID AND DF_ID='"+ objRI.sFailureId +"'";
                // dt = objcon.getDataTable(strQry); 

                NpgsqlCommand cmd = new NpgsqlCommand("sp_getdtctcdetailsfromfailure");
                cmd.Parameters.AddWithValue("sfailureid", objRI.sFailureId);
                dt = objcon.FetchDataTable(cmd);
                if (dt.Rows.Count > 0)
                {
                    objRI.sDTCCode = Convert.ToString(dt.Rows[0]["DF_DTC_CODE"]);
                    objRI.sTCCode = Convert.ToString(dt.Rows[0]["DF_EQUIPMENT_ID"]);
                    objRI.sFailureTCId = Convert.ToString(dt.Rows[0]["TC_ID"]);
                    objRI.sDTCId = Convert.ToString(dt.Rows[0]["DT_ID"]);
                    objRI.sNewTCCode = Convert.ToString(dt.Rows[0]["TD_TC_NO"]);
                    objRI.sNewTCId = Convert.ToString(dt.Rows[0]["TD_TC_ID"]);

                }
                return objRI;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objRI;
            }
        }

        public clsRIApproval GetRIDetails(clsRIApproval objRIApproval)
        {
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();

                //strQry = "SELECT TR_OIL_QUNTY,TR_APPROVE_REMARKS,TR_RV_NO,TO_CHAR(TR_RV_DATE,'DD/MM/YYYY') TR_RV_DATE,TR_MANUAL_ACKRV_NO,";
                //strQry += " TR_OIL_QTY_BYSK,TO_CHAR(TR_DECOMM_DATE,'DD/MM/YYYY') TR_DECOMM_DATE FROM TBLTCREPLACE WHERE TR_ID='" + objRIApproval.sDecommId + "'";
                //dt = objcon.getDataTable(strQry);

                NpgsqlCommand cmd = new NpgsqlCommand("sp_getridetails");
                cmd.Parameters.AddWithValue("sdecommid", objRIApproval.sDecommId);
                dt = objcon.FetchDataTable(cmd);

                if (dt.Rows.Count > 0)
                {
                    objRIApproval.sOilQuantity = Convert.ToString(dt.Rows[0]["TR_OIL_QUNTY"]);
                    objRIApproval.sRemarks = Convert.ToString(dt.Rows[0]["TR_APPROVE_REMARKS"]).Replace("ç", ",");
                    objRIApproval.sRVNo = Convert.ToString(dt.Rows[0]["TR_RV_NO"]);
                    objRIApproval.sRVDate = Convert.ToString(dt.Rows[0]["TR_RV_DATE"]);
                    objRIApproval.sOilQuantitySK = Convert.ToString(dt.Rows[0]["TR_OIL_QTY_BYSK"]);
                    objRIApproval.sDecommDate = Convert.ToString(dt.Rows[0]["TR_DECOMM_DATE"]);
                    objRIApproval.sManualRVACKNo = Convert.ToString(dt.Rows[0]["TR_MANUAL_ACKRV_NO"]);
                    // objRIApproval.sBarrels = Convert.ToString(dt.Rows[0]["TR_NO_OF_BARRELS"]);

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
            string QryKey = string.Empty;
            string sOfficeCode = string.Empty;
            string sResult = string.Empty;
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();

                #region old inline queary
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sFailureId", Convert.ToInt32(sFailureId));
                //sOfficeCode = objcon.get_value("SELECT \"DF_LOC_CODE\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\" =:sFailureId", NpgsqlCommand);
                #endregion

                QryKey = "GET_DF_LOC_CODE";
                NpgsqlCommand cmd_DF_LOC_CODE = new NpgsqlCommand("fetch_getvalue_clsriapproval");
                cmd_DF_LOC_CODE.Parameters.AddWithValue("p_key", QryKey);
                cmd_DF_LOC_CODE.Parameters.AddWithValue("p_value_1", Convert.ToString(sFailureId ?? ""));
                cmd_DF_LOC_CODE.Parameters.AddWithValue("p_value_2", "");
                sOfficeCode = ObjBasCon.StringGetValue(cmd_DF_LOC_CODE);


                #region old inline queary
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sDecommId", Convert.ToInt32(sDecommId));
                //sResult = objcon.get_value("SELECT \"SM_NAME\" || '~' || \"TR_RI_NO\" FROM \"TBLTCREPLACE\",\"TBLSTOREMAST\" WHERE \"TR_STORE_SLNO\"=\"SM_ID\" AND \"TR_ID\"=:sDecommId", NpgsqlCommand);
                #endregion

                QryKey = "GET_SM_NAME_AND_TR_RI_NO";
                NpgsqlCommand cmd_SM_NAME_AND_TR_RI_NO = new NpgsqlCommand("fetch_getvalue_clsriapproval");
                cmd_SM_NAME_AND_TR_RI_NO.Parameters.AddWithValue("p_key", QryKey);
                cmd_SM_NAME_AND_TR_RI_NO.Parameters.AddWithValue("p_value_1", Convert.ToString(sDecommId ?? ""));
                cmd_SM_NAME_AND_TR_RI_NO.Parameters.AddWithValue("p_value_2", "");
                sOfficeCode = ObjBasCon.StringGetValue(cmd_SM_NAME_AND_TR_RI_NO);


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
            }
        }


        public string GenerateAckNo(Int32 wo_id)
        {
            try
            {
                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;
                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Parameters.AddWithValue("woid", wo_id);
                string sAckNo = objcon.get_value("SELECT  acknumber(:woid) ", NpgsqlCommand);


                return sAckNo;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }

        #region WorkFlow XML

        public clsRIApproval GetRIDetailsFromXML(clsRIApproval objRIApproval)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable dt = new DataTable();

                dt = objApproval.GetDatatableFromXML(objRIApproval.sWFDataId);
                if (dt.Rows.Count > 0)
                {
                    objRIApproval.sOilQuantity = Convert.ToString(dt.Rows[0]["TR_OIL_QUNTY"]);
                    objRIApproval.sRemarks = Convert.ToString(dt.Rows[0]["TR_APPROVE_REMARKS"]);
                    objRIApproval.sRVNo = Convert.ToString(dt.Rows[0]["TR_RV_NO"]);
                    objRIApproval.sRVDate = Convert.ToString(dt.Rows[0]["TR_RV_DATE"]);
                    objRIApproval.sOilQuantitySK = Convert.ToString(dt.Rows[0]["TR_OIL_QTY_BYSK"]);
                    objRIApproval.sCrby = Convert.ToString(dt.Rows[0]["TR_APPROVED_BY"]);
                    objRIApproval.sManualRVACKNo = Convert.ToString(dt.Rows[0]["TR_MANUAL_ACKRV_NO"]);
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
