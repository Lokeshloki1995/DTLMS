using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;

namespace IIITS.DTLMS.BL
{
    public class clsStoreIndent
    {
        //CustOledbConnection objCon = new CustOledbConnection(Constants.Password);
        PGSqlConnection objCon = new PGSqlConnection(Constants.Password);

        string strFormCode = "clsTcTransfer";
        public string sQuantity { get; set; }
        public string sTcCapacity { get; set; }
        public string sIndentNo { get; set; }
        public string sIndentDate { get; set; }
        public string sDescription { get; set; }
        public string sFromStoreId { get; set; }
        public string sToStoreId { get; set; }
        public string sSoId { get; set; }
        public string sCrBy { get; set; }
        public string sSiId { get; set; }
        public string sOfficeCode { get; set; }
        public DataTable ddtCapacityGrid { get; set; }
        public string sIndentId { get; set; }
        public string sToStoreName { get; set; }

        public string sIndentObjectid { get; set; }

        // Workflow
        public string sFormName { get; set; }
        public string sClientIP { get; set; }
        public string sWFDataId { get; set; }
        public string sActionType { get; set; }

        NpgsqlCommand NpgsqlCommand;
        public string[] SaveStoreTransfer(clsStoreIndent objTcTransfer)
        {
            
            string strQry = string.Empty;
            string p_key = string.Empty;

            string[] Arr = new string[2];
            DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);

            try
            {
                //To check whether Entered Indent No Already Exists or Not

                if (objTcTransfer.sSiId == null)
                {
                    //NpgsqlCommand = new NpgsqlCommand();
                    ////To get the Store id of logged in user
                    //NpgsqlCommand.Parameters.AddWithValue("IndentNo", objTcTransfer.sIndentNo);
                    //string sInNo = objCon.get_value("SELECT \"SI_NO\" FROM \"TBLSTOREINDENT\" WHERE \"SI_NO\" =:IndentNo", NpgsqlCommand);

                    p_key = "GET_STORE_IND_NUM";
                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_of_save_store_transfer");
                    cmd.Parameters.AddWithValue("indent_no", (objTcTransfer.sIndentNo ?? ""));
                    cmd.Parameters.AddWithValue("p_key", (p_key ?? ""));
                    cmd.Parameters.AddWithValue("ofc_code","");

                    string sInNo = objDatabse.StringGetValue(cmd);

                    if (sInNo.Length > 0)
                    {
                        Arr[0] = "Entered Indent Number Already Exists";
                        Arr[1] = "2";
                        return Arr;
                    }

                    //dr = objCon.Fetch("SELECT \"SI_NO\" FROM \"TBLSTOREINDENT\" WHERE \"SI_NO\" ='" + objTcTransfer.sIndentNo + "'");
                    //if (dr.Read())
                    //{
                    //    dr.Close();
                    //    Arr[0] = "Entered Indent Number Already Exists";
                    //    Arr[1] = "2";
                    //    return Arr;
                    //}
                    //dr.Close();

                   /// string strFromStoreId = objCon.get_value("SELECT \"SM_ID\" FROM \"TBLSTOREMAST\" WHERE \"SM_OFF_CODE\" ='" + objTcTransfer.sOfficeCode.Substring(0, Constants.Division) + "'");

                    objTcTransfer.sSiId = Convert.ToString(objCon.Get_max_no("SI_ID", "TBLSTOREINDENT"));

                    strQry = "INSERT INTO \"TBLSTOREINDENT\" (\"SI_ID\",\"SI_NO\",\"SI_DATE\",\"SI_DESC\",\"SI_FROM_STORE\",\"SI_TO_STORE\",\"SI_CRBY\",\"SI_CRON\",\"SI_TRANSFER_FLAG\") ";
                    strQry += " VALUES('" + objTcTransfer.sSiId + "','" + objTcTransfer.sIndentNo + "',";
                    strQry += " TO_DATE('" + objTcTransfer.sIndentDate + "','dd/MM/yyyy'),'" + objTcTransfer.sDescription + "',";
                    strQry += " '" + objTcTransfer.sOfficeCode + "','" + objTcTransfer.sToStoreId + "','" + objTcTransfer.sCrBy + "',NOW(),0)";
                   //objCon.Execute(strQry);


                    //for (int i = 0; i < objTcTransfer.ddtCapacityGrid.Rows.Count; i++)
                    //{
                    //    objTcTransfer.sSoId = Convert.ToString(objCon.Get_max_no("SO_ID", "TBLSINDENTOBJECTS"));
                    //    strQry = "INSERT INTO TBLSINDENTOBJECTS(SO_ID,SO_SI_ID,SO_CAPACITY,SO_QNTY,SO_CRBY,SO_CRON) ";
                    //    strQry += " VALUES('" + objTcTransfer.sSoId + "','" + objTcTransfer.sSiId + "','" + objTcTransfer.ddtCapacityGrid.Rows[i]["SO_CAPACITY"] + "',";
                    //    strQry += " '" + objTcTransfer.ddtCapacityGrid.Rows[i]["SO_QNTY"] + "','" + objTcTransfer.sCrBy + "',SYSDATE)";
                    //    //objCon.Execute(strQry);
                     
                    //}
                   
                    #region Workflow

                    strQry = "INSERT INTO \"TBLSTOREINDENT\" (\"SI_ID\",\"SI_NO\",\"SI_DATE\",\"SI_DESC\",\"SI_FROM_STORE\",\"SI_TO_STORE\",\"SI_CRBY\",\"SI_CRON\",\"SI_TRANSFER_FLAG\") ";
                    strQry += " VALUES('{0}','" + objTcTransfer.sIndentNo + "',";  //    (SELECT INVOICENUMBER('" + objTcTransfer.sOfficeCode + "')),";
                    strQry += " TO_DATE('" + objTcTransfer.sIndentDate + "','dd/MM/yyyy'),'" + objTcTransfer.sDescription + "',";
                    strQry += " '" + objTcTransfer.sOfficeCode + "','" + objTcTransfer.sToStoreId + "','" + objTcTransfer.sCrBy + "',NOW(),0)";

                    strQry = strQry.Replace("'", "''");


                    StringBuilder sbQuery = new StringBuilder();
                   // StringBuilder sbQueryParameter = new StringBuilder();
                    string sCapacityValues = string.Empty;
                    string sQuantityValues = string.Empty;


                    int iObject = 1;
                    string strQry1 = string.Empty;
                    for (int i = 0; i < objTcTransfer.ddtCapacityGrid.Rows.Count; i++)
                    {
                        //objTcTransfer.sSoId = Convert.ToString(objCon.Get_max_no("SO_ID", "TBLSINDENTOBJECTS"));

                        strQry1 = "INSERT INTO \"TBLSINDENTOBJECTS\"(\"SO_ID\",\"SO_SI_ID\",\"SO_CAPACITY\",\"SO_QNTY\",\"SO_CRBY\",\"SO_CRON\") ";
                        strQry1 += " VALUES((SELECT COALESCE(MAX(\"SO_ID\"),0)+1 FROM \"TBLSINDENTOBJECTS\"),'{0}','" + objTcTransfer.ddtCapacityGrid.Rows[i]["SO_CAPACITY"] + "',";
                        strQry1 += " '" + objTcTransfer.ddtCapacityGrid.Rows[i]["SO_QNTY"] + "','" + objTcTransfer.sCrBy + "',NOW())";
                        //objCon.Execute(strQry);
                        sbQuery.Append(strQry1);
                        sbQuery.Append(";");

                        //sbQueryParameter.Append("SELECT COALESCE(MAX(SO_ID),0)+1 FROM TBLSINDENTOBJECTS");
                        //sbQueryParameter.Append(";");

                        sCapacityValues += objTcTransfer.ddtCapacityGrid.Rows[i]["SO_CAPACITY"]+"`";
                        sQuantityValues += objTcTransfer.ddtCapacityGrid.Rows[i]["SO_QNTY"] + "`";

                        iObject++;
                    }
                    sbQuery = sbQuery.Replace("'", "''");

                    string sParam = "SELECT COALESCE(MAX(\"SI_ID\"),0)+1 FROM \"TBLSTOREINDENT\"";

                    clsApproval objApproval = new clsApproval();

                    objApproval.sFormName = objTcTransfer.sFormName;
                    // objApproval.sRecordId = objFailureDetails.sFailureId;
                    //objApproval.sOfficeCode = objCon.get_value("SELECT \"STO_OFF_CODE\" FROM \"TBLSTOREOFFCODE\" WHERE \"STO_SM_ID\" ='" + objTcTransfer.sOfficeCode + "'");

                    p_key = "GET_STORE_OFF";
                    NpgsqlCommand cmd1 = new NpgsqlCommand("fetch_getvalue_of_save_store_transfer");
                    cmd1.Parameters.AddWithValue("indent_no", "");
                    cmd1.Parameters.AddWithValue("p_key", (p_key ?? ""));
                    cmd1.Parameters.AddWithValue("ofc_code", (objTcTransfer.sOfficeCode ?? ""));
                    objApproval.sOfficeCode = objDatabse.StringGetValue(cmd1);


                    objApproval.sClientIp = objTcTransfer.sClientIP;
                  
                    objApproval.sCrby = objTcTransfer.sCrBy;
                    objApproval.sQryValues = strQry + ";" + sbQuery;
                    objApproval.sParameterValues = sParam ;
                    objApproval.sMainTable = "TBLSTOREINDENT";
                    objApproval.sDataReferenceId = objTcTransfer.sOfficeCode;
                    objApproval.sDescription = "Inter Store Indent no " + objTcTransfer.sIndentNo + " Request for Specified Capacity Transformer  To Store Name " + objTcTransfer.sToStoreName;
                    //objApproval.sRefOfficeCode = objTcTransfer.sOfficeCode;
                    //objApproval.sRefOfficeCode = objCon.get_value("SELECT \"STO_OFF_CODE\" FROM \"TBLSTOREOFFCODE\" WHERE \"STO_SM_ID\" ='" + objTcTransfer.sOfficeCode + "'");

                    p_key = "GET_STORE_OFF1";
                    NpgsqlCommand cmd2 = new NpgsqlCommand("fetch_getvalue_of_save_store_transfer");
                    cmd2.Parameters.AddWithValue("indent_no", "");
                    cmd2.Parameters.AddWithValue("p_key", (p_key ?? ""));
                    cmd2.Parameters.AddWithValue("ofc_code", (objTcTransfer.sOfficeCode ?? ""));
                    objApproval.sRefOfficeCode = objDatabse.StringGetValue(cmd2);



                    string sPrimaryKey = "{0}";
                    string sSecPrimaryKey = "{1}";

                    objApproval.sColumnNames = "SI_ID,SI_NO,SI_DATE,SI_DESC,SI_FROM_STORE,SI_TO_STORE,SI_CRBY";
                    objApproval.sColumnNames += ";SO_ID,SO_CAPACITY,SO_QNTY";
                    objApproval.sColumnValues = "" + sPrimaryKey + "," + objTcTransfer.sIndentNo + "," + objTcTransfer.sIndentDate + "," + objTcTransfer.sDescription + ",";
                    objApproval.sColumnValues += "" + objTcTransfer.sOfficeCode + "," + objTcTransfer.sToStoreId + "," + objTcTransfer.sCrBy + "";
                    objApproval.sColumnValues += ";" + sSecPrimaryKey + "," + sCapacityValues + "," + sQuantityValues + "";

                    objApproval.sTableNames = "TBLSTOREINDENT;TBLSINDENTOBJECTS";

                    if (objTcTransfer.sActionType == "M")
                    {
                        objApproval.SaveWorkFlowData(objApproval);
                        objTcTransfer.sWFDataId = objApproval.sWFDataId;
                    }
                    else
                    {

                        objApproval.SaveWorkFlowData(objApproval);
                        objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow();
                        objApproval.SaveWorkflowObjects(objApproval);
                    }

                    #endregion



                    Arr[0] = "Saved Successfully";
                    Arr[1] = "0";
                    return Arr;
                }
                #region UnUsed Code
                //else
                //{
                //    NpgsqlCommand = new NpgsqlCommand();
                //    objCon.BeginTransaction();

                //    NpgsqlCommand.Parameters.AddWithValue("IndentNo1", objTcTransfer.sIndentNo);
                //    NpgsqlCommand.Parameters.AddWithValue("SiId", Convert.ToInt32(objTcTransfer.sSiId));
                //    string sInNo = objCon.get_value("SELECT \"SI_NO\" FROM \"TBLSTOREINDENT\" WHERE \"SI_NO\" =:IndentNo1 and \"SI_ID\" <> :SiId", NpgsqlCommand);
                //    if (sInNo.Length > 0)
                //    {
                //        Arr[0] = "Entered Indent Number Already Exists";
                //        Arr[1] = "2";
                //        return Arr;
                //    }

                //    //dr = objCon.Fetch("select SI_NO from TBLSTOREINDENT where SI_NO='" + objTcTransfer.sIndentNo + "' and SI_ID <> '" + objTcTransfer.sSiId + "'");
                //    //if (dr.Read())
                //    //{
                //    //    dr.Close();
                //    //    Arr[0] = "Entered Indent Number Already Exists";
                //    //    Arr[1] = "2";
                //    //    return Arr;
                //    //}
                //    //dr.Close();


                //    NpgsqlCommand.Parameters.AddWithValue("ToStoreId",Convert.ToInt32( objTcTransfer.sToStoreId));
                //    NpgsqlCommand.Parameters.AddWithValue("Description", objTcTransfer.sDescription);
                //    NpgsqlCommand.Parameters.AddWithValue("IndentNo2",  objTcTransfer.sIndentNo );
                //    NpgsqlCommand.Parameters.AddWithValue("IndentDate", objTcTransfer.sIndentDate);
                //    NpgsqlCommand.Parameters.AddWithValue("SiId1",Convert.ToInt32( objTcTransfer.sSiId));

                //    strQry = "UPDATE \"TBLSTOREINDENT\" SET \"SI_TO_STORE\" =:ToStoreId,\"SI_DESC\"=:Description";
                //    strQry += " , \"SI_NO\" =:IndentNo2, \"SI_DATE\" =to_date(:IndentDate,'dd/MM/yyyy') ";
                //    strQry += " WHERE \"SI_ID\" =:SiId1";
                //    objCon.ExecuteQry(strQry, NpgsqlCommand);

                //    //deleting old records
                //    NpgsqlCommand.Parameters.AddWithValue("SiId2",Convert.ToInt32(objTcTransfer.sSiId));
                //    strQry = "DELETE FROM \"TBLSINDENTOBJECTS\" WHERE \"SO_SI_ID\" =:SiId2";
                //    objCon.ExecuteQry(strQry, NpgsqlCommand);

                //    for (int i = 0; i < objTcTransfer.ddtCapacityGrid.Rows.Count; i++)
                //    {
                //        NpgsqlCommand = new NpgsqlCommand();
                //        //inserting updated grid records
                //        objTcTransfer.sSoId = Convert.ToString(objCon.Get_max_no("SO_ID", "TBLSINDENTOBJECTS"));

                //        NpgsqlCommand.Parameters.AddWithValue("SoId",Convert.ToInt32( objTcTransfer.sSoId));
                //        NpgsqlCommand.Parameters.AddWithValue("SiId3", Convert.ToInt32(objTcTransfer.sSiId));
                //        NpgsqlCommand.Parameters.AddWithValue("ddtCapacityGrid",Convert.ToDouble(objTcTransfer.ddtCapacityGrid.Rows[i]["SO_CAPACITY"]));
                //        NpgsqlCommand.Parameters.AddWithValue("ddtCapacityGrid1",Convert.ToDouble(objTcTransfer.ddtCapacityGrid.Rows[i]["SO_QNTY"]));
                //        NpgsqlCommand.Parameters.AddWithValue("CrBy",Convert.ToInt32( objTcTransfer.sCrBy));

                //        strQry = "INSERT INTO \"TBLSINDENTOBJECTS\" (\"SO_ID\",\"SO_SI_ID\",\"SO_CAPACITY\",\"SO_QNTY\",\"SO_CRBY\",\"SO_CRON\") ";
                //        strQry += " VALUES(:SoId,:SiId3,:ddtCapacityGrid,";
                //        strQry += " :ddtCapacityGrid1,:CrBy, NOW())";
                //        objCon.ExecuteQry(strQry, NpgsqlCommand);
                //    }

                //    objCon.CommitTransaction();
                //    Arr[0] = "Updated Successfully";
                //    Arr[1] = "1";
                //    return Arr;
                //}
                #endregion
                return Arr;

            }

            catch (Exception ex)
            {
                objCon.RollBackTrans();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }

        public DataTable LoadIndentGrid(string sOfficeCode)
        {
            string strQry = string.Empty;
            DataTable dtIndentDetails = new DataTable();
            try
            {
                //if (sOfficeCode.Length >= 3)
                //{
                //    sOfficeCode = sOfficeCode.Substring(0, Constants.Division);
                //}
              
                NpgsqlCommand cmd = new NpgsqlCommand("sp_loadindentgrid");
                cmd.Parameters.AddWithValue("soffcode", sOfficeCode);
                dtIndentDetails = objCon.FetchDataTable(cmd);

                //strQry = "SELECT SI_ID,TO_CHAR(SI_DATE,'dd-MON-yyyy')SI_DATE,SI_NO,SUM(SO_QNTY)SO_QNTY,";
                //strQry += " (SELECT SM_NAME FROM TBLSTOREMAST WHERE SM_ID=SI_TO_STORE)SI_TO_STORE FROM TBLSTOREINDENT,TBLSINDENTOBJECTS ";
                //strQry += " WHERE SI_ID=SO_SI_ID and SI_TRANSFER_FLAG=0 AND (SELECT SM_OFF_CODE FROM TBLSTOREMAST WHERE SM_ID=SI_FROM_STORE) LIKE '" + sOfficeCode + "%'";
                //strQry += " GROUP BY SI_NO,SI_ID,SI_DATE,SI_TO_STORE ORDER BY SI_NO DESC";


                ////strQry += " AND  (SELECT SM_OFF_CODE FROM TBLSTOREMAST WHERE SM_ID=SI_FROM_STORE)";
                ////LIKE '" + sOfficeCode + "%'
                //dtIndentDetails = objCon.getDataTable(strQry);
                return dtIndentDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtIndentDetails;
            }
        }
        
        public DataTable LoadCompletedIndentGrid(string sOfficeCode)
        {
            string strQry = string.Empty;
            DataTable dtIndentDetails = new DataTable();
            try
            {
                //if (sOfficeCode.Length >= 3)
                //{
                //    sOfficeCode = sOfficeCode.Substring(0, Constants.Division);
                //}


                NpgsqlCommand cmd = new NpgsqlCommand("sp_loadcompletedindentgrid");
                cmd.Parameters.AddWithValue("soffcode", sOfficeCode);
                dtIndentDetails = objCon.FetchDataTable(cmd);

                //strQry = "SELECT SI_ID,TO_CHAR(SI_DATE,'dd-MON-yyyy')SI_DATE,SI_NO,SUM(SO_QNTY)SO_QNTY,";
                //strQry += " (SELECT SM_NAME FROM TBLSTOREMAST WHERE SM_ID=SI_TO_STORE)SI_TO_STORE FROM TBLSTOREINDENT,TBLSINDENTOBJECTS ";
                //strQry += " WHERE SI_ID=SO_SI_ID and SI_TRANSFER_FLAG=1 AND (SELECT SM_OFF_CODE FROM TBLSTOREMAST WHERE SM_ID=SI_FROM_STORE) LIKE '" + sOfficeCode + "%'";
                //strQry += " GROUP BY SI_NO,SI_ID,SI_DATE,SI_TO_STORE ORDER BY SI_NO DESC";
                ////strQry += " AND  (SELECT SM_OFF_CODE FROM TBLSTOREMAST WHERE SM_ID=SI_FROM_STORE)";
                ////LIKE '" + sOfficeCode + "%'
                //dtIndentDetails = objCon.getDataTable(strQry);
                return dtIndentDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtIndentDetails;
            }
        }

        public Boolean CheckForInvoice(string strIndentId)
        {
            DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
            NpgsqlCommand = new NpgsqlCommand();
            string strQry = string.Empty;
            bool istrue = false;
            try
            {
                //NpgsqlCommand.Parameters.AddWithValue("IndentId",Convert.ToInt32( strIndentId));
                //strQry = "SELECT \"IS_SI_ID\" FROM \"TBLSTOREINVOICE\" WHERE \"IS_SI_ID\"=:IndentId";
                //string sId = objCon.get_value(strQry, NpgsqlCommand);

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_value_of_inv");
                cmd.Parameters.AddWithValue("indent_id", (strIndentId ?? ""));
                string sId = objDatabse.StringGetValue(cmd);


                if (sId.Length > 0)
                {
                    istrue = true;
                }
                //dr = objCon.Fetch(strQry);
                //if (dr.Read())
                //{
                //    istrue = true;
                //}
                return istrue;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return istrue;
            }
        }

        public object GetIndentDetails(clsStoreIndent objTcTransfer)
        {
            string strQry = string.Empty;
            try
            {
                DataTable dtIndentDetails = new DataTable();
                //strQry = "SELECT SI_ID,TO_CHAR(SI_DATE,'dd/MM/yyyy')SI_DATE,SI_DESC,SI_TO_STORE,SI_NO,SO_CAPACITY,";
                //strQry += " SO_QNTY FROM TBLSTOREINDENT,TBLSINDENTOBJECTS WHERE SI_ID=SO_SI_ID AND SI_ID='" + objTcTransfer.sIndentId + "'";

                //dtIndentDetails = objCon.getDataTable(strQry);

                NpgsqlCommand cmd = new NpgsqlCommand("sp_getindentdetails");
                cmd.Parameters.AddWithValue("sindentid", objTcTransfer.sIndentId);
                dtIndentDetails = objCon.FetchDataTable(cmd);

                objTcTransfer.sSiId = Convert.ToString(dtIndentDetails.Rows[0]["SI_ID"]);
                objTcTransfer.sIndentNo = Convert.ToString(dtIndentDetails.Rows[0]["SI_NO"]);
                objTcTransfer.sIndentDate = Convert.ToString(dtIndentDetails.Rows[0]["SI_DATE"]);
                objTcTransfer.sDescription = Convert.ToString(dtIndentDetails.Rows[0]["SI_DESC"]);
                objTcTransfer.sToStoreId = Convert.ToString(dtIndentDetails.Rows[0]["SI_TO_STORE"]);
                return objTcTransfer;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objTcTransfer;

            }
        }

        public DataTable LoadCapacityGrid(clsStoreIndent objTcTransfer)
        {
            string strQry = string.Empty;
            DataTable dtCapacityDetails = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("sp_loadcapgrid");
                cmd.Parameters.AddWithValue("sindentid", objTcTransfer.sIndentId);
                dtCapacityDetails = objCon.FetchDataTable(cmd);

                //strQry = "SELECT SO_ID,SO_CAPACITY,SO_QNTY FROM TBLSINDENTOBJECTS,TBLSTOREINDENT WHERE SO_SI_ID=SI_ID AND SI_ID='" + objTcTransfer.sIndentId + "'";
                //dtCapacityDetails = objCon.getDataTable(strQry);

                return dtCapacityDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtCapacityDetails;

            }
        }

        public string GetTransformerCount(string sStoreId)
        {
            DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                //string strQry = string.Empty;
                //NpgsqlCommand.Parameters.AddWithValue("StoreId",Convert.ToInt32( sStoreId));
                //strQry = "SELECT COUNT(*) FROM \"TBLTCMASTER\" WHERE \"TC_STORE_ID\" =:StoreId AND ";
                //strQry += " \"TC_STATUS\" IN (1,2) AND \"TC_CURRENT_LOCATION\" =1";
                //return objCon.get_value(strQry, NpgsqlCommand);

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_transformer_count");
                cmd.Parameters.AddWithValue("store_id", (sStoreId ?? ""));
                return objDatabse.StringGetValue(cmd);



            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }

        public DataTable LoadStoreCapacityGrid(string sStoreId)
        {
            string strQry = string.Empty;
            DataTable dtCapacityDetails = new DataTable();
            try
            {
                //strQry = " SELECT COUNT(*) STOCKCOUNT,TO_CHAR(TC_CAPACITY)TC_CAPACITY,(SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':')+2) ";
                //strQry += " FROM VIEW_ALL_OFFICES WHERE OFF_CODE=SM_OFF_CODE) SM_OFF_CODE FROM TBLTCMASTER,TBLSTOREMAST WHERE TC_STORE_ID ='" + sStoreId + "' AND TC_STORE_ID=SM_ID ";
                //strQry += " AND TC_STATUS IN (1,2) AND TC_CURRENT_LOCATION=1 AND TC_CAPACITY IS NOT NULL GROUP BY TC_CAPACITY,SM_OFF_CODE";
                //dtCapacityDetails = objCon.getDataTable(strQry);

                NpgsqlCommand cmd = new NpgsqlCommand("sp_loadstorecapacitygrid");
                cmd.Parameters.AddWithValue("sstoreid", sStoreId);
                dtCapacityDetails = objCon.FetchDataTable(cmd);

                return dtCapacityDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtCapacityDetails;

            }
        }

        #region WorkFlow XML

        public clsStoreIndent GetStoreIndentDetailsFromXML(clsStoreIndent objStoreIndent)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable dtIndentDetails = new DataTable();
                DataSet ds = new DataSet();

                ds = objApproval.GetDatatableFromMultipleXML(objStoreIndent.sWFDataId);
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    if (ds.Tables[i].Rows.Count > 0)
                    {
                        dtIndentDetails = ds.Tables[i];
                        if (i == 0)
                        {
                            //objStoreIndent.sSiId = Convert.ToString(dtIndentDetails.Rows[0]["SI_ID"]);
                            objStoreIndent.sIndentNo = Convert.ToString(dtIndentDetails.Rows[0]["SI_NO"]);
                            objStoreIndent.sIndentDate = Convert.ToString(dtIndentDetails.Rows[0]["SI_DATE"]);
                            objStoreIndent.sDescription = Convert.ToString(dtIndentDetails.Rows[0]["SI_DESC"]);
                            objStoreIndent.sToStoreId = Convert.ToString(dtIndentDetails.Rows[0]["SI_TO_STORE"]);
                            objStoreIndent.sCrBy = Convert.ToString(dtIndentDetails.Rows[0]["SI_CRBY"]);
                        }
                        else
                        {
                            objStoreIndent.sIndentObjectid = Convert.ToString(dtIndentDetails.Rows[0]["SO_ID"]);
                            objStoreIndent.sTcCapacity = Convert.ToString(dtIndentDetails.Rows[0]["SO_CAPACITY"]);
                            objStoreIndent.sQuantity = Convert.ToString(dtIndentDetails.Rows[0]["SO_QNTY"]);

                            if (objStoreIndent.sTcCapacity.EndsWith("`"))
                            {
                                objStoreIndent.sTcCapacity = objStoreIndent.sTcCapacity.Remove(objStoreIndent.sTcCapacity.Length - 1);
                            }

                            if (objStoreIndent.sQuantity.EndsWith("`"))
                            {
                                objStoreIndent.sQuantity = objStoreIndent.sQuantity.Remove(objStoreIndent.sQuantity.Length - 1);
                            }


                            objStoreIndent.ddtCapacityGrid = CreateDatatableFromString(objStoreIndent);
                            //objStoreIndent.sIndentObjectid = "{1}";
                            //objStoreIndent.sTcCapacity = "750`125`";
                            //objStoreIndent.sQuantity = "2`1`";
                        }
                    }
                }
               
                return objStoreIndent;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objStoreIndent;
            }
        }


        public DataTable CreateDatatableFromString(clsStoreIndent objStoreIndent)
        {
          
            DataTable dt = new DataTable();

            dt.Columns.Add("SO_ID");
            dt.Columns.Add("SO_CAPACITY");
            dt.Columns.Add("SO_QNTY");

            string[] strdtColumns = objStoreIndent.sTcCapacity.Split('`');
            string[] strdtParametres = objStoreIndent.sQuantity.Split('`');
            for (int i = 0; i < strdtColumns.Length; i++)
            {

                for (int j = 0; j < strdtParametres.Length; j++)
                {
                    if (strdtColumns[j] != "")
                    {
                        DataRow dRow = dt.NewRow();
                        dRow["SO_ID"] = i;
                        dRow["SO_CAPACITY"] = strdtColumns[i];
                        dRow["SO_QNTY"] = strdtParametres[j];
                        dt.Rows.Add(dRow);
                        dt.AcceptChanges();
                    }
                    i++;
                }
            }
            return dt;
        }

        #endregion

        public string GetOfficeCodeFromStore(string sStoreId)
        {
            DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);

            NpgsqlCommand = new NpgsqlCommand();
            try
            {
               
                //string strQry = string.Empty;
                //NpgsqlCommand.Parameters.AddWithValue("StoreId",Convert.ToInt32( sStoreId));
                //strQry = "SELECT \"STO_OFF_CODE\" FROM \"TBLSTOREOFFCODE\" WHERE \"STO_SM_ID\" =:StoreId";
                //return objCon.get_value(strQry, NpgsqlCommand);

                NpgsqlCommand cmd2 = new NpgsqlCommand("proc_get_office_code_from_store");
                cmd2.Parameters.AddWithValue("store_id", (sStoreId ?? ""));
                return objDatabse.StringGetValue(cmd2);


            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        public string GenerateInvoiceNo(string sOfficeCode, string sRoletype)
        {
            try
            {
                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;
                NpgsqlCommand = new NpgsqlCommand();
                int stempLength;
                if (sRoletype == "1")
                {
                    sOfficeCode = sOfficeCode.Substring(0, Constants.Division);
                    sOfficeCode = clsStoreOffice.GetStoreID(sOfficeCode);
                }
                else if (sRoletype == "2")
                {
                    if (sOfficeCode.Length > 2)
                    {
                        sOfficeCode = clsStoreOffice.GetStoreID(sOfficeCode);
                    }
                }

                stempLength = sOfficeCode.Length;
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", sOfficeCode);
                //string sInvoiceNo = objCon.get_value("SELECT COALESCE(MAX(CAST(\"INV_NO\" AS INT8)),0)+1  FROM \"VIEWINVOICE\" WHERE \"LOCCODE\" =:sOfficeCode", NpgsqlCommand);

                #region Converted to sp
                DataBase.DataBseConnection objdatabase = new DataBase.DataBseConnection(Constants.Password);
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_value_of_store_indent_num");
                cmd.Parameters.AddWithValue("ofc_code", sOfficeCode);
                string sInvoiceNo = objdatabase.StringGetValue(cmd);

                #endregion

                if (stempLength == 1)
                {
                    if (sInvoiceNo == "1")
                    {
                        sInvoiceNo = sOfficeCode;
                    }
                }

                if (sInvoiceNo.Length == 1)
                {
                    //2 digit Office Code
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        sFinancialYear = System.DateTime.Now.ToString("yy") + "" + System.DateTime.Now.AddYears(1).ToString("yy");
                    }
                    else
                    {
                        sFinancialYear = System.DateTime.Now.AddYears(-1).ToString("yy") + "" + System.DateTime.Now.ToString("yy");
                    }
                    if (sRoletype == "2")
                    {
                        sInvoiceNo = sOfficeCode + sFinancialYear + "00001";
                    }
                    else
                    {
                        sInvoiceNo = sOfficeCode + sFinancialYear + "00001";
                    }
                }
                else
                {
                    //2 digit Office Code
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                        {
                            sFinancialYear = System.DateTime.Now.ToString("yy") + "" + System.DateTime.Now.AddYears(1).ToString("yy");
                        }
                        if (sRoletype == "2")
                        {
                            if (sInvoiceNo.Length > 2)
                            {
                                if (sFinancialYear == sInvoiceNo.Substring(2, 4))
                                {
                                    return sInvoiceNo;
                                }
                            }
                            else
                            {
                                sInvoiceNo = sOfficeCode + sFinancialYear + "00001";
                            }
                        }
                        else
                        {
                            if (sFinancialYear == sInvoiceNo.Substring(2, 4))
                            {
                                return sInvoiceNo;
                            }
                            else
                            {
                                sInvoiceNo = sOfficeCode + sFinancialYear + "00001";
                            }
                        }

                    }
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) <= 03)
                    {
                        if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                        {
                            sFinancialYear = System.DateTime.Now.ToString("yy") + "" + System.DateTime.Now.AddYears(1).ToString("yy");
                        }
                        else
                        {
                            sFinancialYear = System.DateTime.Now.AddYears(-1).ToString("yy") + "" + System.DateTime.Now.ToString("yy");
                        }
                        if (sRoletype == "2")
                        {
                            if (sInvoiceNo.Length > 2)
                            {
                                if (sFinancialYear == sInvoiceNo.Substring(2, 4))
                                {
                                    return sInvoiceNo;
                                }
                            }
                            else
                            {
                                sInvoiceNo = sOfficeCode + sFinancialYear + "00001";
                            }

                            return sInvoiceNo;
                        }


                    }
                }

                return sInvoiceNo;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }




    }
}