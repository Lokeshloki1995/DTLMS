using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;
using IIITS.DTLMS.BL.DataBase;

namespace IIITS.DTLMS.BL
{
    public class clsFormValues
    {
        string strFormCode = "clsFormValues";
        PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
        DataBseConnection ObjBseCon = new DataBseConnection(Constants.Password);
        NpgsqlCommand NpgsqlCommand;

        public string sFailureId { get; set; }
        public string sWorkOrderId { get; set; }
        public string sWOTTKstatus { get; set; }
        public string sIndentId { get; set; }
        public string sInvoiceId { get; set; }
        public string sDecommisionId { get; set; }
        public string sWFInitialId { get; set; }
        public string sTaskType { get; set; }
        public string sTCcode { get; set; }
        public string sFailType { get; set; }
        public string Worangeallocationid { get; set; }
        public string QryKey { get; set; }

        /// <summary>
        /// Get DTC Id
        /// </summary>
        /// <param name="objForm"></param>
        /// <returns></returns>
        public string GetDTCId(clsFormValues objForm)
        {
            string DTCID = string.Empty;
            QryKey = string.Empty;
            try
            {
                #region Old inline query
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "SELECT (SELECT \"DT_ID\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=\"DF_DTC_CODE\") DT_ID FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\"=:sFailureId";
                //NpgsqlCommand.Parameters.AddWithValue("sFailureId", Convert.ToInt32(objForm.sFailureId));
                //DTCID = objcon.get_value(strQry, NpgsqlCommand);
                #endregion

                QryKey = "GET_DT_ID";
                NpgsqlCommand cmd_DT_ID = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                cmd_DT_ID.Parameters.AddWithValue("p_key", QryKey);
                cmd_DT_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(objForm.sFailureId ?? ""));
                cmd_DT_ID.Parameters.AddWithValue("p_value_2", "");
                DTCID = ObjBseCon.StringGetValue(cmd_DT_ID);

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
            return DTCID;
        }
        #region WorkOrder
        /// <summary>
        /// Get StatusFlag For WO
        /// </summary>
        /// <param name="objForm"></param>
        /// <returns></returns>
        public string GetStatusFlagForWO(clsFormValues objForm)
        {
            string StringGetValue = string.Empty;
            try
            {
                #region Old inline query
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "SELECT \"DF_STATUS_FLAG\"|| '~' ||COALESCE(\"EST_GUARANTEETYPE\",'') FROM \"TBLDTCFAILURE\",\"TBLESTIMATIONDETAILS\" WHERE \"DF_ID\"=\"EST_FAILUREID\" AND \"EST_ID\"=:sFailureId";
                //NpgsqlCommand.Parameters.AddWithValue("sFailureId", Convert.ToInt32(objForm.sFailureId));
                ////strQry = "SELECT \"DF_STATUS_FLAG\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\" ='" + objForm.sFailureId + "'";
                //StringGetValue = objcon.get_value(strQry, NpgsqlCommand);
                #endregion

                QryKey = "GET_DF_STATUS_FLAG_AND_EST_GUARANTEETYPE";
                NpgsqlCommand cmd_DT_ID = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                cmd_DT_ID.Parameters.AddWithValue("p_key", QryKey);
                cmd_DT_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(objForm.sFailureId ?? ""));
                cmd_DT_ID.Parameters.AddWithValue("p_value_2", "");
                StringGetValue = ObjBseCon.StringGetValue(cmd_DT_ID);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
            return StringGetValue;
        }
        /// <summary>
        /// Get FailType
        /// </summary>
        /// <param name="objForm"></param>
        /// <returns></returns>
        public string GetFailType(clsFormValues objForm)
        {
            string StringGetValue = string.Empty;
            try
            {
                #region Old inline query
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "SELECT \"EST_FAIL_TYPE\" || '~' || \"DF_ID\" FROM \"TBLDTCFAILURE\",\"TBLESTIMATIONDETAILS\" WHERE \"DF_ID\"=\"EST_FAILUREID\" AND \"EST_ID\"=:sFailureId";
                //NpgsqlCommand.Parameters.AddWithValue("sFailureId", Convert.ToInt32(objForm.sFailureId));
                ////strQry = "SELECT \"DF_STATUS_FLAG\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\" ='" + objForm.sFailureId + "'";
                //StringGetValue = objcon.get_value(strQry, NpgsqlCommand);
                #endregion

                QryKey = "GET_EST_FAIL_TYPE_AND_DF_ID";
                NpgsqlCommand cmd_DT_ID = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                cmd_DT_ID.Parameters.AddWithValue("p_key", QryKey);
                cmd_DT_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(objForm.sFailureId ?? ""));
                cmd_DT_ID.Parameters.AddWithValue("p_value_2", "");
                StringGetValue = ObjBseCon.StringGetValue(cmd_DT_ID);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
            return StringGetValue;
        }
        #endregion
        /// <summary>
        /// Get Status Flag For WO From WF
        /// </summary>
        /// <param name="objForm"></param>
        /// <returns></returns>
        public clsFormValues GetStatusFlagForWOFromWF(clsFormValues objForm)
        {
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();
                #region old code when work order had only 1 level
                //if (objForm.sFailureId.Contains("-"))
                //{
                //    strQry = "SELECT \"DF_STATUS_FLAG\",\"EST_ID\" AS WO_RECORD_ID,\"EST_FAIL_TYPE\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\",\"TBLESTIMATIONDETAILS\" WHERE ";
                //    strQry += " (SELECT \"WOA_PREV_APPROVE_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_INITIAL_ACTION_ID\" ='" + objForm.sWFInitialId + "')= \"WO_ID\" ";
                //    strQry += " AND \"DF_ID\"=\"EST_FAILUREID\" AND  CAST(\"DF_ID\" AS TEXT)=\"WO_DATA_ID\"";
                //}
                //else
                //{
                //    strQry = "SELECT \"DF_STATUS_FLAG\",\"WO_RECORD_ID\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\" WHERE ";
                //    strQry += " (SELECT \"WOA_PREV_APPROVE_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_INITIAL_ACTION_ID\" ='" + objForm.sWFInitialId + "')= \"WO_ID\" ";
                //    strQry += " AND  \"DF_ID\"=\"WO_RECORD_ID\"";
                //}
                #endregion

                if (objForm.sFailureId.Contains("-"))
                {
                    #region Old inline query
                    //strQry = "SELECT \"DF_STATUS_FLAG\"|| '~' ||COALESCE(\"EST_GUARANTEETYPE\",'') AS \"DF_STATUS_FLAG\",\"WO_RECORD_ID\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\",\"TBLESTIMATIONDETAILS\" WHERE ";
                    //strQry += " (SELECT \"WOA_PREV_APPROVE_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_INITIAL_ACTION_ID\" =:sWFInitialId)= \"WO_ID\" ";
                    //strQry += " AND cast(\"DF_ID\" as text)=cast(\"WO_DATA_ID\"as text) AND cast(\"EST_ID\" as text)=cast(\"WO_RECORD_ID\"as text)";
                    //NpgsqlCommand = new NpgsqlCommand();
                    //NpgsqlCommand.Parameters.AddWithValue("sWFInitialId", Convert.ToInt32(objForm.sWFInitialId));
                    //dt = objcon.FetchDataTable(strQry, NpgsqlCommand);
                    #endregion

                    NpgsqlCommand cmd = new NpgsqlCommand("proc_get_dfstatusflagandworecordid_dls");
                    cmd.Parameters.AddWithValue("p_wfinitialid", Convert.ToString(objForm.sWFInitialId ?? ""));
                    dt = objcon.FetchDataTable(cmd);


                    if (dt.Rows.Count > 0)
                    {
                        objForm.sFailureId = Convert.ToString(dt.Rows[0]["WO_RECORD_ID"]);
                        objForm.sTaskType = Convert.ToString(dt.Rows[0]["DF_STATUS_FLAG"]);

                        if (dt.Columns.Contains("EST_FAIL_TYPE"))
                        {
                            objForm.sFailType = Convert.ToString(dt.Rows[0]["EST_FAIL_TYPE"]);
                        }
                        else
                        {
                            #region Old inline query
                            //strQry = "SELECT \"DF_STATUS_FLAG\"|| '~' ||COALESCE(\"EST_GUARANTEETYPE\",'') AS \"DF_STATUS_FLAG\",\"EST_ID\" AS WO_RECORD_ID,\"EST_FAIL_TYPE\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\",\"TBLESTIMATIONDETAILS\" WHERE ";
                            //strQry += " (SELECT \"WOA_PREV_APPROVE_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_INITIAL_ACTION_ID\" =:sWFInitialId)= \"WO_ID\" ";
                            //strQry += " AND \"DF_ID\"=\"EST_FAILUREID\" AND  CAST(\"DF_ID\" AS TEXT)=\"WO_DATA_ID\"";
                            //NpgsqlCommand = new NpgsqlCommand();
                            //NpgsqlCommand.Parameters.AddWithValue("sWFInitialId", Convert.ToInt32(objForm.sWFInitialId));
                            //dt = objcon.FetchDataTable(strQry, NpgsqlCommand);
                            #endregion

                            NpgsqlCommand cmd_WO_ID = new NpgsqlCommand("proc_get_not_of_est_fail_type_details");
                            cmd_WO_ID.Parameters.AddWithValue("p_wfinitialid", Convert.ToString(objForm.sWFInitialId ?? ""));
                            dt = objcon.FetchDataTable(cmd_WO_ID);


                            if (dt.Rows.Count > 0)
                            {
                                objForm.sFailureId = Convert.ToString(dt.Rows[0]["WO_RECORD_ID"]);
                                objForm.sTaskType = Convert.ToString(dt.Rows[0]["DF_STATUS_FLAG"]);
                                objForm.sFailType = Convert.ToString(dt.Rows[0]["EST_FAIL_TYPE"]);
                            }
                        }
                    }
                    else
                    {
                        #region Old Inline query
                        //strQry = "SELECT \"WO_RECORD_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE ";
                        //strQry += " (SELECT \"WOA_PREV_APPROVE_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_INITIAL_ACTION_ID\" =:sWFInitialId)= \"WO_ID\" ";
                        //NpgsqlCommand = new NpgsqlCommand();
                        //NpgsqlCommand.Parameters.AddWithValue("sWFInitialId", Convert.ToInt32(objForm.sWFInitialId));
                        //objForm.sFailureId = objcon.get_value(strQry, NpgsqlCommand);
                        #endregion

                        QryKey = "GET_WO_RECORD_ID_ON_WO_ID";
                        NpgsqlCommand cmd_WO_ID = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                        cmd_WO_ID.Parameters.AddWithValue("p_key", QryKey);
                        cmd_WO_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(objForm.sWFInitialId ?? ""));
                        cmd_WO_ID.Parameters.AddWithValue("p_value_2", "");
                        objForm.sFailureId = ObjBseCon.StringGetValue(cmd_WO_ID);

                        objForm.sTaskType = "3";
                    }
                }
                else /*(dt.Rows.Count == 0)*/
                {
                    #region Old inline query
                    //strQry = "SELECT \"EST_FAIL_TYPE\",\"EST_ID\",\"DF_STATUS_FLAG\"|| '~' ||COALESCE(\"EST_GUARANTEETYPE\",'') AS \"DF_STATUS_FLAG\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLWORKORDER\",\"TBLESTIMATIONDETAILS\",";
                    //strQry += " \"TBLWO_OBJECT_AUTO\",\"TBLDTCFAILURE\" WHERE \"WO_RECORD_ID\"=\"WO_SLNO\" AND \"WO_DF_ID\"=\"EST_FAILUREID\" AND ";
                    //strQry += " \"WOA_INITIAL_ACTION_ID\" =\"WO_ID\" AND \"WO_DF_ID\"=\"EST_FAILUREID\" AND \"DF_ID\"=\"WO_DF_ID\" AND \"WO_ID\"=:sWFInitialId";
                    //NpgsqlCommand = new NpgsqlCommand();
                    //NpgsqlCommand.Parameters.AddWithValue("sWFInitialId", Convert.ToInt32(objForm.sWFInitialId));
                    //dt = objcon.FetchDataTable(strQry, NpgsqlCommand);
                    #endregion

                    NpgsqlCommand cmd_WO_ID = new NpgsqlCommand("proc_get_estfailtypeanddfstatusflag_dls");
                    cmd_WO_ID.Parameters.AddWithValue("p_wfinitialid", Convert.ToString(objForm.sWFInitialId ?? ""));
                    dt = objcon.FetchDataTable(cmd_WO_ID);

                    if (dt.Rows.Count > 0)
                    {
                        objForm.sFailureId = Convert.ToString(dt.Rows[0]["EST_ID"]);
                        objForm.sTaskType = Convert.ToString(dt.Rows[0]["DF_STATUS_FLAG"]);
                        objForm.sFailType = Convert.ToString(dt.Rows[0]["EST_FAIL_TYPE"]);
                    }
                    else
                    {
                        #region Old Inline query
                        //strQry = "SELECT \"WO_RECORD_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE ";
                        //strQry += "  \"WO_ID\"=:sWFInitialId ";
                        //NpgsqlCommand = new NpgsqlCommand();
                        //NpgsqlCommand.Parameters.AddWithValue("sWFInitialId", Convert.ToInt32(objForm.sWFInitialId));
                        //objForm.sWorkOrderId = objcon.get_value(strQry, NpgsqlCommand);
                        #endregion

                        QryKey = "GET_WO_RECORD_ID_IN_TBLWORKFLOWOBJECTS";
                        NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                        cmd.Parameters.AddWithValue("p_key", QryKey);
                        cmd.Parameters.AddWithValue("p_value_1", Convert.ToString(objForm.sWFInitialId ?? ""));
                        cmd.Parameters.AddWithValue("p_value_2", "");
                        objForm.sWorkOrderId = ObjBseCon.StringGetValue(cmd);

                        objForm.sTaskType = "3";
                    }
                }
                return objForm;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objForm;
            }
        }
        #region Indent
        /// <summary>
        /// Get StatusFlag For Indent
        /// </summary>
        /// <param name="objForm"></param>
        /// <returns></returns>
        public string GetStatusFlagForIndent(clsFormValues objForm)
        {
            string sResult = string.Empty;
            try
            {
                #region Old inline query
                //string strQry = string.Empty;
                //strQry = "SELECT \"DF_STATUS_FLAG\"|| '~' ||COALESCE(\"EST_GUARANTEETYPE\",'') AS \"DF_STATUS_FLAG\" FROM \"TBLWORKORDER\",\"TBLDTCFAILURE\",\"TBLESTIMATIONDETAILS\" WHERE \"DF_ID\"=\"WO_DF_ID\" ";
                //strQry += " AND \"DF_ID\"=\"EST_FAILUREID\" AND \"WO_SLNO\" =:sWorkOrderId ";
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sWorkOrderId", Convert.ToInt32(objForm.sWorkOrderId));
                //sResult = objcon.get_value(strQry, NpgsqlCommand);
                #endregion

                QryKey = "GET_DF_STATUS_FLAG_AND_EST_GUARANTEETYPE_ON_WO_DF_ID";
                NpgsqlCommand cmd_DT_ID = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                cmd_DT_ID.Parameters.AddWithValue("p_key", QryKey);
                cmd_DT_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(objForm.sWorkOrderId ?? ""));
                cmd_DT_ID.Parameters.AddWithValue("p_value_2", "");
                sResult = ObjBseCon.StringGetValue(cmd_DT_ID);

                if (sResult == "")
                {
                    #region Old inline query
                    //strQry = "SELECT \"WO_NO\" FROM \"TBLWORKORDER\" WHERE  \"WO_DF_ID\" IS NULL AND \"WO_SLNO\" =:sWorkOrderId";
                    //NpgsqlCommand = new NpgsqlCommand();
                    //NpgsqlCommand.Parameters.AddWithValue("sWorkOrderId", Convert.ToInt32(objForm.sWorkOrderId));
                    //sResult = objcon.get_value(strQry, NpgsqlCommand);
                    #endregion

                    QryKey = "GET_WO_NO";
                    NpgsqlCommand cmd_WO_NO = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                    cmd_WO_NO.Parameters.AddWithValue("p_key", QryKey);
                    cmd_WO_NO.Parameters.AddWithValue("p_value_1", Convert.ToString(objForm.sWorkOrderId ?? ""));
                    cmd_WO_NO.Parameters.AddWithValue("p_value_2", "");
                    sResult = ObjBseCon.StringGetValue(cmd_WO_NO);

                    if (sResult != "")
                    {
                        sResult = "3";
                    }
                }
                else
                {
                    return sResult;
                }

                return sResult;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }
        /// <summary>
        /// Get Status Flag For Indent From WF
        /// </summary>
        /// <param name="objForm"></param>
        /// <returns></returns>
        public clsFormValues GetStatusFlagForIndentFromWF(clsFormValues objForm)
        {
            DataTable dt = new DataTable();
            try
            {
                #region Old inline query
                //string strQry = string.Empty;
                //strQry = "SELECT \"DF_STATUS_FLAG\",\"WO_RECORD_ID\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\",\"TBLWORKORDER\" WHERE ";
                //strQry += " (SELECT \"WOA_PREV_APPROVE_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_INITIAL_ACTION_ID\" =:sWFInitialId)= \"WO_ID\"";
                //strQry += " AND  \"WO_SLNO\"=\"WO_RECORD_ID\" AND \"DF_ID\"=\"WO_DF_ID\"";
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sWFInitialId", Convert.ToInt32(objForm.sWFInitialId));
                //dt = objcon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_st_flag_for_indentfromwf");
                cmd.Parameters.AddWithValue("p_wfinitialid", Convert.ToString(objForm.sWFInitialId));
                dt = objcon.FetchDataTable(cmd);

                if (dt.Rows.Count > 0)
                {
                    objForm.sWorkOrderId = Convert.ToString(dt.Rows[0]["WO_RECORD_ID"]);
                    objForm.sTaskType = Convert.ToString(dt.Rows[0]["DF_STATUS_FLAG"]);
                }
                else
                {
                    #region Old inline query
                    //strQry = "SELECT \"WO_RECORD_ID\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLWORKORDER\" WHERE ";
                    //strQry += " (SELECT \"WOA_PREV_APPROVE_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_INITIAL_ACTION_ID\" =:sWFInitialId)= \"WO_ID\" ";
                    //strQry += " AND  \"WO_SLNO\"=\"WO_RECORD_ID\" AND \"WO_DF_ID\" IS NULL";
                    //NpgsqlCommand = new NpgsqlCommand();
                    //NpgsqlCommand.Parameters.AddWithValue("sWFInitialId", Convert.ToInt32(objForm.sWFInitialId));
                    //objForm.sWorkOrderId = objcon.get_value(strQry, NpgsqlCommand);
                    #endregion

                    QryKey = "GET_WO_RECORD_ID";
                    NpgsqlCommand cmd_WO_NO = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                    cmd_WO_NO.Parameters.AddWithValue("p_key", QryKey);
                    cmd_WO_NO.Parameters.AddWithValue("p_value_1", Convert.ToString(objForm.sWFInitialId ?? ""));
                    cmd_WO_NO.Parameters.AddWithValue("p_value_2", "");
                    objForm.sWorkOrderId = ObjBseCon.StringGetValue(cmd_WO_NO);
                    objForm.sTaskType = "3";
                }
                return objForm;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objForm;
            }
        }
        #endregion
        #region Invoice
        /// <summary>
        /// Get StatusFlag For Invoice From Indent
        /// </summary>
        /// <param name="objForm"></param>
        /// <returns></returns>
        public string GetStatusFlagForInvoiceFromIndent(clsFormValues objForm)
        {
            string sResult = string.Empty;
            try
            {
                #region Old inline query
                //string strQry = string.Empty;                
                //strQry = "SELECT \"DF_STATUS_FLAG\" FROM \"TBLWORKORDER\",\"TBLDTCFAILURE\",\"TBLINDENT\" WHERE \"DF_ID\"=\"WO_DF_ID\"  ";
                //strQry += " AND \"WO_SLNO\"=\"TI_WO_SLNO\"  AND \"WO_SLNO\" =:sIndentId";
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sIndentId", Convert.ToInt32(objForm.sIndentId));
                //sResult = objcon.get_value(strQry, NpgsqlCommand);
                #endregion

                QryKey = "GET_DF_STATUS_FLAG";
                NpgsqlCommand cmd_DF_STATUS_FLAG = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                cmd_DF_STATUS_FLAG.Parameters.AddWithValue("p_key", QryKey);
                cmd_DF_STATUS_FLAG.Parameters.AddWithValue("p_value_1", Convert.ToString(objForm.sIndentId ?? ""));
                cmd_DF_STATUS_FLAG.Parameters.AddWithValue("p_value_2", "");
                sResult = ObjBseCon.StringGetValue(cmd_DF_STATUS_FLAG);

                if (sResult == "")
                {
                    #region Old inline query
                    //strQry = "SELECT \"WO_NO\" FROM \"TBLWORKORDER\",\"TBLINDENT\" WHERE \"WO_SLNO\"=\"TI_WO_SLNO\" AND \"WO_DF_ID\" IS NULL ";
                    //strQry += " AND \"TI_ID\" =:sIndentId";
                    //NpgsqlCommand = new NpgsqlCommand();
                    //NpgsqlCommand.Parameters.AddWithValue("sIndentId", Convert.ToInt32(objForm.sIndentId));
                    //sResult = objcon.get_value(strQry, NpgsqlCommand);
                    #endregion

                    QryKey = "GET_WO_NO_ON_TI_ID";
                    NpgsqlCommand cmd_WO_NO = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                    cmd_WO_NO.Parameters.AddWithValue("p_key", QryKey);
                    cmd_WO_NO.Parameters.AddWithValue("p_value_1", Convert.ToString(objForm.sIndentId ?? ""));
                    cmd_WO_NO.Parameters.AddWithValue("p_value_2", "");
                    sResult = ObjBseCon.StringGetValue(cmd_WO_NO);

                    if (sResult != "")
                    {
                        sResult = "3";
                    }
                }
                else
                {
                    return sResult;
                }

                return sResult;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }
        /// <summary>
        /// Get Status Flag For Invoice From WF
        /// </summary>
        /// <param name="objForm"></param>
        /// <returns></returns>
        public clsFormValues GetStatusFlagForInvoiceFromWF(clsFormValues objForm)
        {
            DataTable dt = new DataTable();
            try
            {
                #region Old inline query
                //string strQry = string.Empty;
                //strQry = "SELECT \"DF_STATUS_FLAG\",\"WO_RECORD_ID\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\",\"TBLWORKORDER\",\"TBLINDENT\" WHERE ";
                //strQry += " (SELECT \"WOA_PREV_APPROVE_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_INITIAL_ACTION_ID\" =:sWFInitialId)= \"WO_ID\" ";
                //strQry += " AND  \"WO_SLNO\"=\"WO_RECORD_ID\" AND \"DF_ID\" = \"WO_DF_ID\" AND \"WO_SLNO\"=\"TI_WO_SLNO\" ";
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sWFInitialId", Convert.ToInt32(objForm.sWFInitialId));
                //dt = objcon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_st_flag_for_invoicefromwf");
                cmd.Parameters.AddWithValue("p_wfinitialid", Convert.ToString(objForm.sWFInitialId));
                dt = objcon.FetchDataTable(cmd);

                if (dt.Rows.Count > 0)
                {
                    objForm.sIndentId = Convert.ToString(dt.Rows[0]["WO_RECORD_ID"]);
                    objForm.sTaskType = Convert.ToString(dt.Rows[0]["DF_STATUS_FLAG"]);
                }
                else
                {
                    #region Old inline query
                    //strQry = "SELECT \"WO_RECORD_ID\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLWORKORDER\",\"TBLINDENT\" WHERE ";
                    //strQry += " (SELECT \"WOA_PREV_APPROVE_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_INITIAL_ACTION_ID\" =:sWFInitialId)= \"WO_ID\"";
                    //strQry += " AND  \"TI_ID\"=\"WO_RECORD_ID\" AND \"WO_DF_ID\" IS NULL AND \"WO_SLNO\"=\"TI_WO_SLNO\" ";
                    //NpgsqlCommand = new NpgsqlCommand();
                    //NpgsqlCommand.Parameters.AddWithValue("sWFInitialId", Convert.ToInt32(objForm.sWFInitialId));
                    //objForm.sIndentId = objcon.get_value(strQry, NpgsqlCommand);
                    #endregion

                    QryKey = "GET_WO_RECORD_ID_ON_WOA_PREV_APPROVE_ID";
                    NpgsqlCommand cmd_WO_NO = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                    cmd_WO_NO.Parameters.AddWithValue("p_key", QryKey);
                    cmd_WO_NO.Parameters.AddWithValue("p_value_1", Convert.ToString(objForm.sWFInitialId ?? ""));
                    cmd_WO_NO.Parameters.AddWithValue("p_value_2", "");
                    objForm.sIndentId = ObjBseCon.StringGetValue(cmd_WO_NO);

                    objForm.sTaskType = "3";
                }
                return objForm;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objForm;
            }
        }
        #endregion
        #region Decommission  
        /// <summary>
        /// Get StatusFlag For Decommission From Invoice
        /// </summary>
        /// <param name="objForm"></param>
        /// <returns></returns>     
        public string GetStatusFlagForDecommissionFromInvoice(clsFormValues objForm)
        {
            string sResult = string.Empty;
            try
            {
                #region Old inline query
                string strQry = string.Empty;
                //strQry = "SELECT \"DF_STATUS_FLAG\" from \"TBLWORKORDER\",\"TBLDTCFAILURE\" WHERE \"DF_ID\"=\"WO_DF_ID\" and \"WO_SLNO\"=:sWorkOrderId";
                //// strQry = "SELECT \"DF_STATUS_FLAG\" FROM \"TBLWORKORDER\",\"TBLDTCFAILURE\",\"TBLINDENT\",\"TBLDTCINVOICE\" WHERE \"DF_ID\"=\"WO_DF_ID\"  ";
                //// strQry += " AND \"WO_SLNO\"=\"TI_WO_SLNO\" AND \"TI_ID\"=\"IN_TI_NO\"  AND \"IN_NO\" =:sInvoiceId";
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sWorkOrderId", Convert.ToInt32(objForm.sWorkOrderId));
                //sResult = objcon.get_value(strQry, NpgsqlCommand);
                #endregion

                QryKey = "GET_DF_STATUS_FLAG_ON_WO_SLNO";
                NpgsqlCommand cmd_DF_STATUS_FLAG = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                cmd_DF_STATUS_FLAG.Parameters.AddWithValue("p_key", QryKey);
                cmd_DF_STATUS_FLAG.Parameters.AddWithValue("p_value_1", Convert.ToString(objForm.sWorkOrderId ?? ""));
                cmd_DF_STATUS_FLAG.Parameters.AddWithValue("p_value_2", "");
                sResult = ObjBseCon.StringGetValue(cmd_DF_STATUS_FLAG);

                if (sResult == "")
                {
                    #region Old inline query
                    ////string sInvoiceId = GetInvoiceId(objForm.sDecommisionId);
                    //strQry = "SELECT \"WO_NO\" FROM \"TBLWORKORDER\",\"TBLINDENT\",\"TBLDTCINVOICE\" WHERE \"WO_SLNO\"=\"TI_WO_SLNO\" AND \"WO_DF_ID\" IS NULL ";
                    //strQry += " AND \"IN_NO\" =:sInvoiceId AND \"TI_ID\"=\"IN_TI_NO\" ";
                    //NpgsqlCommand = new NpgsqlCommand();
                    //NpgsqlCommand.Parameters.AddWithValue("sInvoiceId", Convert.ToInt32(objForm.sInvoiceId));
                    //sResult = objcon.get_value(strQry, NpgsqlCommand);
                    #endregion

                    QryKey = "GET_WO_NO_ON_IN_NO";
                    NpgsqlCommand cmd_WO_NO = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                    cmd_WO_NO.Parameters.AddWithValue("p_key", QryKey);
                    cmd_WO_NO.Parameters.AddWithValue("p_value_1", Convert.ToString(objForm.sInvoiceId ?? ""));
                    cmd_WO_NO.Parameters.AddWithValue("p_value_2", "");
                    sResult = ObjBseCon.StringGetValue(cmd_WO_NO);

                    if (sResult != "")
                    {
                        sResult = "3";
                    }
                    else
                    {
                        #region Old inline query
                        //strQry = "SELECT \"WO_TTK_STATUS\" FROM \"TBLWORKORDER\" WHERE \"WO_SLNO\"=:sInvoiceId";
                        //NpgsqlCommand = new NpgsqlCommand();
                        //NpgsqlCommand.Parameters.AddWithValue("sInvoiceId", Convert.ToInt32(objForm.sInvoiceId));
                        //sResult = objcon.get_value(strQry, NpgsqlCommand);
                        #endregion

                        QryKey = "GET_WO_TTK_STATUS";
                        NpgsqlCommand cmd_WO_SLNO = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                        cmd_WO_SLNO.Parameters.AddWithValue("p_key", QryKey);
                        cmd_WO_SLNO.Parameters.AddWithValue("p_value_1", Convert.ToString(objForm.sInvoiceId ?? ""));
                        cmd_WO_SLNO.Parameters.AddWithValue("p_value_2", "");
                        sResult = ObjBseCon.StringGetValue(cmd_WO_SLNO);

                        if (sResult == "1")
                        {
                            objForm.sWOTTKstatus = sResult;
                            sResult = "3";
                        }
                    }
                }
                else
                {
                    return sResult;
                }

                return sResult;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }
        /// <summary>
        /// Get StatusFlag For Decomm From WF
        /// </summary>
        /// <param name="objForm"></param>
        /// <returns></returns>
        public clsFormValues GetStatusFlagForDecommFromWF(clsFormValues objForm)
        {
            DataTable dt = new DataTable();
            try
            {
                #region old inline query
                //string strQry = string.Empty;
                //strQry = " SELECT \"DF_STATUS_FLAG\" ,\"DF_ID\" from \"TBLDTCFAILURE\" WHERE  cast (\"DF_ID\" as varchar)in ";
                //strQry += " (SELECT \"WO_DATA_ID\"  FROM \"TBLWORKFLOWOBJECTS\" , \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_PREV_APPROVE_ID\" = \"WO_ID\" ";
                //strQry += " and \"WOA_INITIAL_ACTION_ID\" =:sWFInitialId ) ";
                //// strQry = "SELECT \"DF_STATUS_FLAG\",\"WO_RECORD_ID\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\",\"TBLTCDRAWN\" WHERE ";
                //// strQry += " (SELECT \"WOA_PREV_APPROVE_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_INITIAL_ACTION_ID\" =:sWFInitialId)= \"WO_ID\"";
                ////strQry += " AND  \"TD_INV_NO\"=\"WO_RECORD_ID\" AND \"DF_ID\"=\"TD_DF_ID\"";
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sWFInitialId", Convert.ToInt32(objForm.sWFInitialId));
                //dt = objcon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_st_flag_for_decommfromwf");
                cmd.Parameters.AddWithValue("p_wfinitialid", Convert.ToString(objForm.sWFInitialId));
                dt = objcon.FetchDataTable(cmd);

                if (dt.Rows.Count > 0)
                {
                    objForm.sInvoiceId = Convert.ToString(dt.Rows[0]["DF_ID"]);
                    objForm.sTaskType = Convert.ToString(dt.Rows[0]["DF_STATUS_FLAG"]);
                }
                else
                {
                    #region Old inline query
                    //strQry = "SELECT \"WO_RECORD_ID\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLWORKORDER\",\"TBLINDENT\",\"TBLDTCINVOICE\" WHERE ";
                    //strQry += " (SELECT \"WOA_PREV_APPROVE_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_INITIAL_ACTION_ID\" =:sWFInitialId)= \"WO_ID\"";
                    //strQry += " AND  \"IN_NO\"=\"WO_RECORD_ID\" AND \"WO_DF_ID\" IS NULL AND \"WO_SLNO\"=\"TI_WO_SLNO\" AND \"TI_ID\"=\"IN_TI_NO\" ";
                    //NpgsqlCommand = new NpgsqlCommand();
                    //NpgsqlCommand.Parameters.AddWithValue("sWFInitialId", Convert.ToInt32(objForm.sWFInitialId));
                    //objForm.sInvoiceId = objcon.get_value(strQry, NpgsqlCommand);
                    #endregion

                    QryKey = "GET_WO_RECORD_ID_ON_WOA_INITIAL_ACTION_ID";
                    NpgsqlCommand cmd_WOA_INITIAL_ACTION_ID = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                    cmd_WOA_INITIAL_ACTION_ID.Parameters.AddWithValue("p_key", QryKey);
                    cmd_WOA_INITIAL_ACTION_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(objForm.sWFInitialId ?? ""));
                    cmd_WOA_INITIAL_ACTION_ID.Parameters.AddWithValue("p_value_2", "");
                    objForm.sInvoiceId = ObjBseCon.StringGetValue(cmd_WOA_INITIAL_ACTION_ID);

                    objForm.sTaskType = "3";
                }
                return objForm;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objForm;
            }
        }
        /// <summary>
        /// Get StatusFlag For Decommission
        /// </summary>
        /// <param name="objForm"></param>
        /// <returns></returns>
        public string GetStatusFlagForDecommission(clsFormValues objForm)
        {
            string sResult = string.Empty;
            string failid = string.Empty;
            try
            {
                #region Old inline query
                ////  string woslno = objcon.get_value("SELECT \"TR_WO_SLNO\" from \"TBLTCREPLACE\" WHERE cast(\"TR_ID\" as text) ='"+objForm.sDecommisionId + "'");
                //failid = objcon.get_value("SELECT \"WO_DF_ID\" from \"TBLWORKORDER\" WHERE cast(\"WO_SLNO\" as text) = '" + objForm.sDecommisionId + "'");
                #endregion

                QryKey = "GET_WO_DF_ID";
                NpgsqlCommand cmd_WO_SLNO = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                cmd_WO_SLNO.Parameters.AddWithValue("p_key", QryKey);
                cmd_WO_SLNO.Parameters.AddWithValue("p_value_1", Convert.ToString(objForm.sDecommisionId ?? ""));
                cmd_WO_SLNO.Parameters.AddWithValue("p_value_2", "");
                failid = ObjBseCon.StringGetValue(cmd_WO_SLNO);

                #region Old inline query
                //string strQry = string.Empty;
                //strQry = "SELECT DISTINCT \"DF_STATUS_FLAG\" FROM \"TBLDTCFAILURE\" WHERE cast(\"DF_ID\" as text)='" + failid + "'  ";
                //// strQry += " AND \"TR_ID\" =:sDecommisionId";
                ////NpgsqlCommand = new NpgsqlCommand();
                //// NpgsqlCommand.Parameters.AddWithValue("sDecommisionId", Convert.ToInt32(objForm.sDecommisionId));
                //sResult = objcon.get_value(strQry);
                #endregion

                QryKey = "GET_DF_STATUS_FLAG_ON_DF_ID";
                NpgsqlCommand cmd_DF_ID = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                cmd_DF_ID.Parameters.AddWithValue("p_key", QryKey);
                cmd_DF_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(failid ?? ""));
                cmd_DF_ID.Parameters.AddWithValue("p_value_2", "");
                sResult = ObjBseCon.StringGetValue(cmd_DF_ID);

                if (sResult == "")
                {
                    string sInvoiceId = GetInvoiceId(objForm.sDecommisionId);

                    #region Old inline query
                    //strQry = "SELECT \"WO_NO\" FROM \"TBLWORKORDER\",\"TBLINDENT\",\"TBLDTCINVOICE\" WHERE \"WO_SLNO\"=\"TI_WO_SLNO\" AND \"WO_DF_ID\" IS NULL ";
                    //strQry += " AND \"IN_NO\" =:sInvoiceId AND  \"TI_ID\"=\"IN_TI_NO\" ";
                    //NpgsqlCommand = new NpgsqlCommand();
                    //NpgsqlCommand.Parameters.AddWithValue("sInvoiceId", Convert.ToInt32(sInvoiceId));
                    //sResult = objcon.get_value(strQry, NpgsqlCommand);
                    #endregion

                    QryKey = "GET_WO_NO_ON_IN_NO";
                    NpgsqlCommand cmd_IN_NO = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                    cmd_IN_NO.Parameters.AddWithValue("p_key", QryKey);
                    cmd_IN_NO.Parameters.AddWithValue("p_value_1", Convert.ToString(sInvoiceId ?? ""));
                    cmd_IN_NO.Parameters.AddWithValue("p_value_2", "");
                    sResult = ObjBseCon.StringGetValue(cmd_IN_NO);

                    if (sResult != "")
                    {
                        sResult = "3";
                    }
                }
                else
                {
                    return sResult;
                }

                return sResult;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }
        public clsFormValues GetWOnoForDTCCommission(clsFormValues objForm)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;

                //string sInvoiceId = GetInvoiceId(objForm.sDecommisionId);
                if (objForm.sWOTTKstatus != "1")
                {
                    #region Old inline query
                    //strQry = "SELECT \"TD_TC_NO\",\"WO_SLNO\" FROM \"TBLWORKORDER\",\"TBLINDENT\",\"TBLDTCINVOICE\",\"TBLTCDRAWN\" WHERE ";
                    //strQry += " \"WO_SLNO\"=\"TI_WO_SLNO\" AND \"TI_ID\"=\"IN_TI_NO\" AND \"IN_NO\"=\"TD_INV_NO\" AND \"IN_NO\" =:sInvoiceId";
                    //NpgsqlCommand = new NpgsqlCommand();
                    //NpgsqlCommand.Parameters.AddWithValue("sInvoiceId", Convert.ToInt32(objForm.sInvoiceId));
                    //dt = objcon.FetchDataTable(strQry, NpgsqlCommand);
                    #endregion

                    NpgsqlCommand cmd = new NpgsqlCommand("proc_get_wono_fordtccommission");
                    cmd.Parameters.AddWithValue("p_invoiceid", Convert.ToString(objForm.sInvoiceId));
                    dt = objcon.FetchDataTable(cmd);

                    if (dt.Rows.Count > 0)
                    {
                        objForm.sWorkOrderId = Convert.ToString(dt.Rows[0]["WO_SLNO"]);
                        objForm.sTCcode = Convert.ToString(dt.Rows[0]["TD_TC_NO"]);
                    }
                }
                else
                {
                    #region Old inline query
                    //strQry = "SELECT \"WO_SLNO\" FROM \"TBLWORKORDER\" WHERE ";
                    //strQry += " \"WO_SLNO\"=:sInvoiceId";
                    //NpgsqlCommand = new NpgsqlCommand();
                    //NpgsqlCommand.Parameters.AddWithValue("sInvoiceId", Convert.ToInt32(objForm.sInvoiceId));
                    //dt = objcon.FetchDataTable(strQry, NpgsqlCommand);
                    #endregion

                    NpgsqlCommand cmd = new NpgsqlCommand("proc_get_wo_slno_for_clsformvalues");
                    cmd.Parameters.AddWithValue("p_invoiceid", Convert.ToString(objForm.sInvoiceId));
                    dt = objcon.FetchDataTable(cmd);

                    if (dt.Rows.Count > 0)
                    {
                        objForm.sWorkOrderId = Convert.ToString(dt.Rows[0]["WO_SLNO"]);
                    }
                }
                return objForm;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objForm;
            }
        }
        /// <summary>
        /// Get DTCId From WO
        /// </summary>
        /// <param name="sWOSlno"></param>
        /// <returns></returns>
        public string GetDTCIdFromWO(string sWOSlno)
        {
            string DT_ID = string.Empty;
            try
            {
                #region Old inline query
                //string strQry = string.Empty;
                //strQry = "SELECT \"DT_ID\" FROM \"TBLDTCMAST\" WHERE \"DT_WO_ID\" =:sWOSlno";
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sWOSlno", Convert.ToInt32(sWOSlno));
                //DT_ID = objcon.get_value(strQry, NpgsqlCommand);
                #endregion

                QryKey = "GET_DT_ID_ON_DT_WO_ID";
                NpgsqlCommand cmd_DT_WO_ID = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                cmd_DT_WO_ID.Parameters.AddWithValue("p_key", QryKey);
                cmd_DT_WO_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(sWOSlno ?? ""));
                cmd_DT_WO_ID.Parameters.AddWithValue("p_value_2", "");
                DT_ID = ObjBseCon.StringGetValue(cmd_DT_WO_ID);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
            return DT_ID;
        }
        #endregion
        /// <summary>
        /// Get Invoice Id
        /// </summary>
        /// <param name="sDecommId"></param>
        /// <returns></returns>
        public string GetInvoiceId(string sDecommId)
        {
            string IN_NO = string.Empty;
            try
            {
                #region Old inline query
                //string strQry = string.Empty;
                //strQry = "SELECT \"IN_NO\" FROM \"TBLTCREPLACE\",\"TBLDTCINVOICE\" WHERE \"TR_IN_NO\"=\"IN_NO\" AND \"TR_ID\" =:sDecommId";
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sDecommId", Convert.ToInt32(sDecommId));
                //IN_NO = objcon.get_value(strQry, NpgsqlCommand);
                #endregion

                QryKey = "GET_IN_NO";
                NpgsqlCommand cmd_TR_ID = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                cmd_TR_ID.Parameters.AddWithValue("p_key", QryKey);
                cmd_TR_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(sDecommId ?? ""));
                cmd_TR_ID.Parameters.AddWithValue("p_value_2", "");
                IN_NO = ObjBseCon.StringGetValue(cmd_TR_ID);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
            return IN_NO;
        }
        /// <summary>
        /// Get WorkOrder Id
        /// </summary>
        /// <param name="sIndentId"></param>
        /// <returns></returns>
        public string GetWorkOrderId(string sIndentId)
        {
            string TI_WO_SLNO = string.Empty;
            try
            {
                #region Old inline query
                //string strQry = string.Empty;
                //strQry = "SELECT \"TI_WO_SLNO\" FROM \"TBLINDENT\" WHERE \"TI_ID\" =:sIndentId";
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sIndentId", Convert.ToInt32(sIndentId));
                //TI_WO_SLNO = objcon.get_value(strQry, NpgsqlCommand);
                #endregion

                QryKey = "GET_TI_WO_SLNO";
                NpgsqlCommand cmd_TI_ID = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                cmd_TI_ID.Parameters.AddWithValue("p_key", QryKey);
                cmd_TI_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(sIndentId ?? ""));
                cmd_TI_ID.Parameters.AddWithValue("p_value_2", "");
                TI_WO_SLNO = ObjBseCon.StringGetValue(cmd_TI_ID);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
            return TI_WO_SLNO;
        }
        /// <summary>
        /// Get Indent Id
        /// </summary>
        /// <param name="sInvoiceId"></param>
        /// <returns></returns>
        public string GetIndentId(string sInvoiceId)
        {
            string IN_TI_NO = string.Empty;
            try
            {
                #region Old inline query
                //string strQry = string.Empty;
                //strQry = "SELECT \"IN_TI_NO\" FROM \"TBLDTCINVOICE\" WHERE  \"IN_NO\" =:sInvoiceId";
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sInvoiceId", Convert.ToInt32(sInvoiceId));
                //IN_TI_NO = objcon.get_value(strQry, NpgsqlCommand);
                #endregion

                QryKey = "GET_IN_TI_NO";
                NpgsqlCommand cmd_IN_NO = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                cmd_IN_NO.Parameters.AddWithValue("p_key", QryKey);
                cmd_IN_NO.Parameters.AddWithValue("p_value_1", Convert.ToString(sInvoiceId ?? ""));
                cmd_IN_NO.Parameters.AddWithValue("p_value_2", "");
                IN_TI_NO = ObjBseCon.StringGetValue(cmd_IN_NO);

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
            return IN_TI_NO;
        }
        /// <summary>
        /// Get FailureId From Invoice
        /// </summary>
        /// <param name="sWorkOrderId"></param>
        /// <returns></returns>
        public string GetFailureIdFromInvoice(string sWorkOrderId)
        {
            string DF_ID = string.Empty;
            try
            {
                #region Old inline query
                //string strQry = string.Empty;
                //strQry = "SELECT \"DF_ID\" from \"TBLWORKORDER\",\"TBLDTCFAILURE\" WHERE \"DF_ID\"=\"WO_DF_ID\" and \"WO_SLNO\"=:sWorkOrderId";
                //// strQry = "SELECT \"DF_ID\" FROM \"TBLWORKORDER\",\"TBLDTCFAILURE\",\"TBLINDENT\",\"TBLDTCINVOICE\" WHERE \"DF_ID\"=\"WO_DF_ID\"  ";
                //// strQry += " AND \"WO_SLNO\"=\"TI_WO_SLNO\" AND \"TI_ID\"=\"IN_TI_NO\" AND \"IN_NO\" =:sInvoiceId";
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sWorkOrderId", Convert.ToInt32(sWorkOrderId));
                //DF_ID = objcon.get_value(strQry, NpgsqlCommand);
                #endregion

                QryKey = "GET_DF_ID";
                NpgsqlCommand cmd_WO_SLNO = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                cmd_WO_SLNO.Parameters.AddWithValue("p_key", QryKey);
                cmd_WO_SLNO.Parameters.AddWithValue("p_value_1", Convert.ToString(sWorkOrderId ?? ""));
                cmd_WO_SLNO.Parameters.AddWithValue("p_value_2", "");
                DF_ID = ObjBseCon.StringGetValue(cmd_WO_SLNO);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
            return DF_ID;
        }
        /// <summary>
        /// Get FailureId From DecommId
        /// </summary>
        /// <param name="sDecomm"></param>
        /// <returns></returns>
        public string GetFailureIdFromDecommId(string sDecomm)
        {
            string WO_DF_ID = string.Empty;
            try
            {
                #region Old inline query
                //string strQry = string.Empty;
                //// string woslno = objcon.get_value("SELECT \"TR_WO_SLNO\" from \"TBLTCREPLACE\" WHERE cast(\"TR_ID\" as text) ='" + sDecomm + "'");
                //strQry = "SELECT \"WO_DF_ID\" from \"TBLWORKORDER\" WHERE \"WO_SLNO\" = '" + sDecomm + "'";
                //// strQry = "SELECT DISTINCT \"DF_ID\" FROM \"TBLDTCFAILURE\",\"TBLTCDRAWN\",\"TBLTCREPLACE\" WHERE \"DF_ID\"=\"TD_DF_ID\" AND ";
                //// strQry += " \"TD_INV_NO\"=\"TR_IN_NO\" AND \"TR_ID\" =:sDecomm";
                //// NpgsqlCommand = new NpgsqlCommand();
                //// NpgsqlCommand.Parameters.AddWithValue("sDecomm", Convert.ToInt32(sDecomm));
                //WO_DF_ID = objcon.get_value(strQry);
                #endregion

                QryKey = "GET_WO_DF_ID";
                NpgsqlCommand cmd_WO_SLNO = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                cmd_WO_SLNO.Parameters.AddWithValue("p_key", QryKey);
                cmd_WO_SLNO.Parameters.AddWithValue("p_value_1", Convert.ToString(sDecomm ?? ""));
                cmd_WO_SLNO.Parameters.AddWithValue("p_value_2", "");
                WO_DF_ID = ObjBseCon.StringGetValue(cmd_WO_SLNO);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
            return WO_DF_ID;
        }
        #region Store Invoice
        /// <summary>
        /// Get StoreIndentId From WF
        /// </summary>
        /// <param name="sWFInitialId"></param>
        /// <param name="sWFObjectId"></param>
        /// <returns></returns>
        public string GetStoreIndentIdFromWF(string sWFInitialId, string sWFObjectId)
        {
            string sResult = string.Empty;
            try
            {
                #region Old inline query
                string strQry = string.Empty;
                //strQry = "SELECT \"WO_RECORD_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE ";
                //strQry += " (SELECT \"WOA_PREV_APPROVE_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_INITIAL_ACTION_ID\" =:sWFInitialId)= \"WO_ID\"";
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sWFInitialId", Convert.ToInt32(sWFInitialId));
                //sResult = objcon.get_value(strQry, NpgsqlCommand);
                #endregion

                QryKey = "GET_WO_RECORD_ID_ON_WO_ID";
                NpgsqlCommand cmd_WO_ID = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                cmd_WO_ID.Parameters.AddWithValue("p_key", QryKey);
                cmd_WO_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(sWFInitialId ?? ""));
                cmd_WO_ID.Parameters.AddWithValue("p_value_2", "");
                sResult = ObjBseCon.StringGetValue(cmd_WO_ID);

                if (sResult == "")
                {
                    #region Old inline query
                    //strQry = "SELECT \"WO_RECORD_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\" =:sWFObjectId";
                    //NpgsqlCommand = new NpgsqlCommand();
                    //NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(sWFObjectId));
                    //sResult = objcon.get_value(strQry, NpgsqlCommand);
                    #endregion

                    QryKey = "GET_WO_RECORD_ID_IN_TBLWORKFLOWOBJECTS";
                    NpgsqlCommand cmd_WO_RECORD_ID = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                    cmd_WO_RECORD_ID.Parameters.AddWithValue("p_key", QryKey);
                    cmd_WO_RECORD_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(sWFObjectId ?? ""));
                    cmd_WO_RECORD_ID.Parameters.AddWithValue("p_value_2", "");
                    sResult = ObjBseCon.StringGetValue(cmd_WO_RECORD_ID);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
            return sResult;
        }
        /// <summary>
        /// Get StoreInvoiceId From WF
        /// </summary>
        /// <param name="sWFObjectId"></param>
        /// <returns></returns>
        public string GetStoreInvoiceIdFromWF(string sWFObjectId)
        {
            string sResult = string.Empty;
            try
            {
                #region Old inline query
                //string strQry = string.Empty;
                //strQry = "SELECT \"WO_RECORD_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\" =:sWFObjectId";
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(sWFObjectId));
                //sResult = objcon.get_value(strQry, NpgsqlCommand);
                #endregion

                QryKey = "GET_WO_RECORD_ID_IN_TBLWORKFLOWOBJECTS";
                NpgsqlCommand cmd_WO_ID = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                cmd_WO_ID.Parameters.AddWithValue("p_key", QryKey);
                cmd_WO_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(sWFObjectId ?? ""));
                cmd_WO_ID.Parameters.AddWithValue("p_value_2", "");
                sResult = ObjBseCon.StringGetValue(cmd_WO_ID);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
            return sResult;
        }
        #endregion
        /// <summary>
        /// Get Status Flag For WO From WF perdecomm
        /// </summary>
        /// <param name="objForm"></param>
        /// <returns></returns>
        public clsFormValues GetStatusFlagForWOFromWFperdecomm(clsFormValues objForm)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                if (objForm.sFailureId.Contains("-"))
                {
                    #region Old inline query
                    //NpgsqlCommand = new NpgsqlCommand();
                    //NpgsqlCommand.Parameters.AddWithValue("sWFInitialId", Convert.ToInt32(objForm.sWFInitialId));
                    //strQry = "SELECT \"PEST_GUARANTEETYPE\" AS \"PEST_GUARANTEETYPE\",\"WO_RECORD_ID\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLPERMANENTESTIMATIONDETAILS\" WHERE ";
                    //strQry += " (SELECT \"WOA_PREV_APPROVE_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_INITIAL_ACTION_ID\" =:sWFInitialId)= \"WO_ID\" ";
                    //strQry += "  AND cast(\"PEST_ID\" as text)=cast(\"WO_RECORD_ID\"as text)";
                    //dt = objcon.FetchDataTable(strQry, NpgsqlCommand);
                    #endregion

                    NpgsqlCommand cmd = new NpgsqlCommand("proc_get_wofromwfperdecomm_st_flag");
                    cmd.Parameters.AddWithValue("p_wfinitialid", Convert.ToString(objForm.sWFInitialId ?? ""));
                    dt = objcon.FetchDataTable(cmd);

                    if (dt.Rows.Count > 0)
                    {
                        objForm.sFailureId = Convert.ToString(dt.Rows[0]["WO_RECORD_ID"]);
                        objForm.sTaskType = Convert.ToString(dt.Rows[0]["PEST_GUARANTEETYPE"]);
                        if (dt.Columns.Contains("PEST_FAIL_TYPE"))
                        {
                            objForm.sFailType = Convert.ToString(dt.Rows[0]["PEST_FAIL_TYPE"]);
                        }
                        else
                        {
                            #region Old inline query
                            //strQry = "SELECT \"PEST_GUARANTEETYPE\" AS \"PEST_GUARANTEETYPE\",\"PEST_ID\" AS WO_RECORD_ID,\"PEST_FAIL_TYPE\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLPERMANENTESTIMATIONDETAILS\" WHERE ";
                            //strQry += " (SELECT \"WOA_PREV_APPROVE_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_INITIAL_ACTION_ID\" =:sWFInitialId)= \"WO_ID\" ";
                            //strQry += " AND   CAST(\"PEST_ID\" AS TEXT)=cast(\"WO_RECORD_ID\"as text)";
                            //NpgsqlCommand = new NpgsqlCommand();
                            //NpgsqlCommand.Parameters.AddWithValue("sWFInitialId", Convert.ToInt32(objForm.sWFInitialId));
                            //dt = objcon.FetchDataTable(strQry, NpgsqlCommand);
                            #endregion

                            NpgsqlCommand cmd_elseblock = new NpgsqlCommand("proc_get_st_flag_forwofromwfperdecomm");
                            cmd_elseblock.Parameters.AddWithValue("p_wfinitialid", Convert.ToString(objForm.sWFInitialId ?? ""));
                            dt = objcon.FetchDataTable(cmd_elseblock);

                            if (dt.Rows.Count > 0)
                            {
                                objForm.sFailureId = Convert.ToString(dt.Rows[0]["WO_RECORD_ID"]);
                                objForm.sTaskType = Convert.ToString(dt.Rows[0]["PEST_GUARANTEETYPE"]);
                                objForm.sFailType = Convert.ToString(dt.Rows[0]["PEST_FAIL_TYPE"]);
                            }
                        }
                    }
                    else
                    {
                        #region Old inline query
                        //strQry = "SELECT \"WO_RECORD_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE ";
                        //strQry += " (SELECT \"WOA_PREV_APPROVE_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_INITIAL_ACTION_ID\" =:sWFInitialId)= \"WO_ID\" ";
                        //NpgsqlCommand = new NpgsqlCommand();
                        //NpgsqlCommand.Parameters.AddWithValue("sWFInitialId", Convert.ToInt32(objForm.sWFInitialId));
                        //objForm.sFailureId = objcon.get_value(strQry, NpgsqlCommand);
                        #endregion

                        QryKey = "GET_WO_RECORD_ID_ON_WO_ID";
                        NpgsqlCommand cmd_WO_ID = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                        cmd_WO_ID.Parameters.AddWithValue("p_key", QryKey);
                        cmd_WO_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(objForm.sWFInitialId ?? ""));
                        cmd_WO_ID.Parameters.AddWithValue("p_value_2", "");
                        objForm.sFailureId = ObjBseCon.StringGetValue(cmd_WO_ID);

                        objForm.sTaskType = "3";
                    }
                }
                else /*(dt.Rows.Count == 0)*/
                {
                    #region Old Inline Query
                    //strQry = "SELECT \"PEST_FAIL_TYPE\",\"PEST_ID\",\"PEST_GUARANTEETYPE\" AS \"PEST_GUARANTEETYPE\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLPERMANENTWORKORDER\",\"TBLPERMANENTESTIMATIONDETAILS\",";
                    //strQry += " \"TBLWO_OBJECT_AUTO\" WHERE \"WO_RECORD_ID\"=\"PWO_SLNO\" AND \"PWO_PEF_ID\"=\"PEST_ID\" AND ";
                    //strQry += " \"WOA_INITIAL_ACTION_ID\" =\"WO_ID\"  AND \"WO_ID\"=:sWFInitialId";
                    //NpgsqlCommand = new NpgsqlCommand();
                    //NpgsqlCommand.Parameters.AddWithValue("sWFInitialId", Convert.ToInt32(objForm.sWFInitialId));
                    //dt = objcon.FetchDataTable(strQry, NpgsqlCommand);
                    #endregion

                    NpgsqlCommand cmd_elseblock = new NpgsqlCommand("proc_get_st_flag_for_perdecomm");
                    cmd_elseblock.Parameters.AddWithValue("p_wfinitialid", Convert.ToString(objForm.sWFInitialId ?? ""));
                    dt = objcon.FetchDataTable(cmd_elseblock);

                    if (dt.Rows.Count > 0)
                    {
                        objForm.sFailureId = Convert.ToString(dt.Rows[0]["PEST_ID"]);
                        objForm.sTaskType = Convert.ToString(dt.Rows[0]["PEST_GUARANTEETYPE"]);
                        objForm.sFailType = Convert.ToString(dt.Rows[0]["PEST_FAIL_TYPE"]);
                    }
                }
                return objForm;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objForm;
            }
        }
        /// <summary>
        /// Get EstId From DecommId
        /// </summary>
        /// <param name="sDecomm"></param>
        /// <returns></returns>
        public string GetEstIdFromDecommId(string sDecomm)
        {
            string PEST_ID = string.Empty;
            try
            {
                #region Old inline query
                //string strQry = string.Empty;
                //strQry = "SELECT DISTINCT \"PEST_ID\" FROM \"TBLPERMANENTESTIMATIONDETAILS\",\"TBLPERMANENTTCREPLACE\",\"TBLPERMANENTWORKORDER\" WHERE \"PEST_ID\"=\"PWO_PEF_ID\"";
                //strQry += " AND \"PTR_WO_SLNO\" = \"PWO_SLNO\" AND \"PTR_ID\" =:sDecomm";
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sDecomm", Convert.ToInt32(sDecomm));
                //PEST_ID = objcon.get_value(strQry, NpgsqlCommand);
                #endregion

                QryKey = "GET_PEST_ID";
                NpgsqlCommand cmd_PTR_ID = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                cmd_PTR_ID.Parameters.AddWithValue("p_key", QryKey);
                cmd_PTR_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(sDecomm ?? ""));
                cmd_PTR_ID.Parameters.AddWithValue("p_value_2", "");
                PEST_ID = ObjBseCon.StringGetValue(cmd_PTR_ID);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
            return PEST_ID;
        }
        /// <summary>
        /// Get EstId From Indent
        /// </summary>
        /// <param name="sInvoiceId"></param>
        /// <returns></returns>
        public string GetEstIdFromIndent(string sInvoiceId)
        {
            string PEST_ID = string.Empty;
            try
            {
                #region Old inline query
                //string strQry = string.Empty;
                //strQry = "SELECT \"PEST_ID\" FROM \"TBLPERMANENTWORKORDER\",\"TBLPERMANENTESTIMATIONDETAILS\" WHERE \"PEST_ID\"=\"PWO_PEF_ID\" AND \"PWO_SLNO\"=:sInvoiceId";
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sInvoiceId", Convert.ToInt32(sInvoiceId));
                //PEST_ID = objcon.get_value(strQry, NpgsqlCommand);
                #endregion

                QryKey = "GET_PEST_ID_ON_PWO_SLNO";
                NpgsqlCommand cmd_PWO_SLNO = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                cmd_PWO_SLNO.Parameters.AddWithValue("p_key", QryKey);
                cmd_PWO_SLNO.Parameters.AddWithValue("p_value_1", Convert.ToString(sInvoiceId ?? ""));
                cmd_PWO_SLNO.Parameters.AddWithValue("p_value_2", "");
                PEST_ID = ObjBseCon.StringGetValue(cmd_PWO_SLNO);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
            return PEST_ID;
        }
        /// <summary>
        /// Get StatusFlag For WOper decomm
        /// </summary>
        /// <param name="objForm"></param>
        /// <returns></returns>
        public string GetStatusFlagForWOperdecomm(clsFormValues objForm)
        {
            string PEST_GUARANTEETYPE = string.Empty;
            try
            {
                #region Old inline query
                //string strQry = string.Empty;
                //strQry = "SELECT \"PEST_GUARANTEETYPE\" FROM \"TBLPERMANENTESTIMATIONDETAILS\" WHERE \"PEST_ID\"=:sFailureId";
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sFailureId", Convert.ToInt32(objForm.sFailureId));
                ////strQry = "SELECT \"DF_STATUS_FLAG\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\" ='" + objForm.sFailureId + "'";
                //PEST_GUARANTEETYPE = objcon.get_value(strQry, NpgsqlCommand);
                #endregion

                QryKey = "GET_PEST_GUARANTEETYPE";
                NpgsqlCommand cmd_PEST_ID = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                cmd_PEST_ID.Parameters.AddWithValue("p_key", QryKey);
                cmd_PEST_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(objForm.sFailureId ?? ""));
                cmd_PEST_ID.Parameters.AddWithValue("p_value_2", "");
                PEST_GUARANTEETYPE = ObjBseCon.StringGetValue(cmd_PEST_ID);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
            return PEST_GUARANTEETYPE;
        }
        /// <summary>
        /// Get StatusFlag For Decommission From Invoiceper
        /// </summary>
        /// <param name="objForm"></param>
        /// <returns></returns>
        public string GetStatusFlagForDecommissionFromInvoiceper(clsFormValues objForm)
        {
            string sResult = string.Empty;
            try
            {
                #region Old inline query
                //string strQry = string.Empty;
                ////string sInvoiceId = GetInvoiceId(objForm.sDecommisionId);
                //strQry = "SELECT \"PWO_SLNO\" FROM \"TBLPERMANENTWORKORDER\" WHERE \"PWO_SLNO\"=:sInvoiceId ";
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sInvoiceId", Convert.ToInt32(objForm.sInvoiceId));
                //sResult = objcon.get_value(strQry, NpgsqlCommand);
                #endregion

                QryKey = "GET_PWO_SLNO";
                NpgsqlCommand cmd_PWO_SLNO = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                cmd_PWO_SLNO.Parameters.AddWithValue("p_key", QryKey);
                cmd_PWO_SLNO.Parameters.AddWithValue("p_value_1", Convert.ToString(objForm.sInvoiceId ?? ""));
                cmd_PWO_SLNO.Parameters.AddWithValue("p_value_2", "");
                sResult = ObjBseCon.StringGetValue(cmd_PWO_SLNO);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
            return sResult;
        }
        /// <summary>
        /// Get StatusFlag For Decomm From WFper
        /// </summary>
        /// <param name="objForm"></param>
        /// <returns></returns>
        public clsFormValues GetStatusFlagForDecommFromWFper(clsFormValues objForm)
        {
            try
            {
                #region Old inline query
                //string strQry = string.Empty;
                //strQry = "SELECT \"WO_RECORD_ID\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLPERMANENTWORKORDER\" WHERE ";
                //strQry += " (SELECT \"WOA_PREV_APPROVE_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_INITIAL_ACTION_ID\" =:sWFInitialId)= \"WO_ID\"";
                //strQry += " AND  \"PWO_SLNO\"=\"WO_RECORD_ID\"";
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sWFInitialId", Convert.ToInt32(objForm.sWFInitialId));
                //objForm.sIndentId = objcon.get_value(strQry, NpgsqlCommand);
                #endregion

                QryKey = "GET_WO_RECORD_ID_ON_WO_ID_AND_WO_RECORD_ID";
                NpgsqlCommand cmd_WO_RECORD_ID = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                cmd_WO_RECORD_ID.Parameters.AddWithValue("p_key", QryKey);
                cmd_WO_RECORD_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(objForm.sWFInitialId ?? ""));
                cmd_WO_RECORD_ID.Parameters.AddWithValue("p_value_2", "");
                objForm.sIndentId = ObjBseCon.StringGetValue(cmd_WO_RECORD_ID);

                objForm.sTaskType = "3";
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objForm;
            }
            return objForm;
        }
        /// <summary>
        /// Get Status Flag For Decommission per
        /// </summary>
        /// <param name="objForm"></param>
        /// <returns></returns>
        public string GetStatusFlagForDecommissionper(clsFormValues objForm)
        {
            string sResult = string.Empty;
            try
            {
                string sIndentid = getIndentid(objForm.sDecommisionId);

                #region Old inline query
                //string strQry = string.Empty;                
                //strQry = "SELECT \"PWO_NO\" FROM \"TBLPERMANENTWORKORDER\" WHERE \"PWO_SLNO\"=:sIndentid";
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sIndentid", Convert.ToInt32(sIndentid));
                //sResult = objcon.get_value(strQry, NpgsqlCommand);
                #endregion

                QryKey = "GET_PWO_NO";
                NpgsqlCommand cmd_PWO_SLNO = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                cmd_PWO_SLNO.Parameters.AddWithValue("p_key", QryKey);
                cmd_PWO_SLNO.Parameters.AddWithValue("p_value_1", Convert.ToString(sIndentid ?? ""));
                cmd_PWO_SLNO.Parameters.AddWithValue("p_value_2", "");
                sResult = ObjBseCon.StringGetValue(cmd_PWO_SLNO);

                sResult = "3";
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
            return sResult;
        }
        /// <summary>
        /// Get StatusFlag For Indent From WFper
        /// </summary>
        /// <param name="objForm"></param>
        /// <returns></returns>
        public clsFormValues GetStatusFlagForIndentFromWFper(clsFormValues objForm)
        {
            try
            {
                #region Old inline query
                //string strQry = string.Empty;               
                //strQry = "SELECT \"WO_RECORD_ID\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLPERMANENTWORKORDER\" WHERE ";
                //strQry += " (SELECT \"WOA_PREV_APPROVE_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_INITIAL_ACTION_ID\" =:sWFInitialId)= \"WO_ID\" ";
                //strQry += " AND  \"PWO_SLNO\"=\"WO_RECORD_ID\" ";
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sWFInitialId", Convert.ToInt32(objForm.sWFInitialId));
                //objForm.sWorkOrderId = objcon.get_value(strQry, NpgsqlCommand);
                #endregion

                QryKey = "GET_WO_RECORD_ID_ON_WO_ID_AND_WO_RECORD_ID";
                NpgsqlCommand cmd_WO_RECORD_ID = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                cmd_WO_RECORD_ID.Parameters.AddWithValue("p_key", QryKey);
                cmd_WO_RECORD_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(objForm.sWFInitialId ?? ""));
                cmd_WO_RECORD_ID.Parameters.AddWithValue("p_value_2", "");
                objForm.sWorkOrderId = ObjBseCon.StringGetValue(cmd_WO_RECORD_ID);

                objForm.sTaskType = "3";
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objForm;
            }
            return objForm;
        }
        /// <summary>
        /// get Indent id
        /// </summary>
        /// <param name="sDecommId"></param>
        /// <returns></returns>
        public string getIndentid(string sDecommId)
        {
            string PWO_SLNO = string.Empty;
            try
            {
                #region Old inline query
                //string strQry = string.Empty;
                //strQry = "SELECT \"PWO_SLNO\" FROM \"TBLPERMANENTTCREPLACE\",\"TBLPERMANENTWORKORDER\" WHERE \"PTR_WO_SLNO\"=\"PWO_SLNO\" AND \"PTR_ID\" =:sDecommId";
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sDecommId", Convert.ToInt32(sDecommId));
                //PWO_SLNO = objcon.get_value(strQry, NpgsqlCommand);
                #endregion

                QryKey = "GET_PWO_SLNO_ON_PTR_ID";
                NpgsqlCommand cmd_WO_RECORD_ID = new NpgsqlCommand("fetch_getvalue_for_clsformvalues");
                cmd_WO_RECORD_ID.Parameters.AddWithValue("p_key", QryKey);
                cmd_WO_RECORD_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(sDecommId ?? ""));
                cmd_WO_RECORD_ID.Parameters.AddWithValue("p_value_2", "");
                PWO_SLNO = ObjBseCon.StringGetValue(cmd_WO_RECORD_ID);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
            return PWO_SLNO;
        }

        #region Unused Code
        #region WorkOrder
        //public clsFormValues GetStatusFlagForWOFromWF(clsFormValues objForm)
        //{
        //    try
        //    {
        //        string strQry = string.Empty;
        //        DataTable dt = new DataTable();
        //        #region old code when work order had only 1 level
        //        //if (objForm.sFailureId.Contains("-"))
        //        //{
        //        //    strQry = "SELECT \"DF_STATUS_FLAG\",\"EST_ID\" AS WO_RECORD_ID,\"EST_FAIL_TYPE\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\",\"TBLESTIMATIONDETAILS\" WHERE ";
        //        //    strQry += " (SELECT \"WOA_PREV_APPROVE_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_INITIAL_ACTION_ID\" ='" + objForm.sWFInitialId + "')= \"WO_ID\" ";
        //        //    strQry += " AND \"DF_ID\"=\"EST_FAILUREID\" AND  CAST(\"DF_ID\" AS TEXT)=\"WO_DATA_ID\"";
        //        //}
        //        //else
        //        //{
        //        //    strQry = "SELECT \"DF_STATUS_FLAG\",\"WO_RECORD_ID\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\" WHERE ";
        //        //    strQry += " (SELECT \"WOA_PREV_APPROVE_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_INITIAL_ACTION_ID\" ='" + objForm.sWFInitialId + "')= \"WO_ID\" ";
        //        //    strQry += " AND  \"DF_ID\"=\"WO_RECORD_ID\"";
        //        //}
        //        #endregion

        //        if (objForm.sFailureId.Contains("-"))
        //        {
        //            strQry = "SELECT \"DF_STATUS_FLAG\"|| '~' ||COALESCE(\"EST_GUARANTEETYPE\",'') AS \"DF_STATUS_FLAG\",\"WO_RECORD_ID\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\",\"TBLESTIMATIONDETAILS\" WHERE ";
        //            strQry += " (SELECT \"WOA_PREV_APPROVE_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_INITIAL_ACTION_ID\" =:sWFInitialId)= \"WO_ID\" ";
        //            strQry += " AND cast(\"DF_ID\" as text)=cast(\"WO_DATA_ID\"as text) AND cast(\"EST_ID\" as text)=cast(\"WO_RECORD_ID\"as text)";

        //            //else
        //            //{
        //            //    strQry = "SELECT \"DF_STATUS_FLAG\",\"DF_ID\" FROM \"TBLDTCFAILURE\",\"TBLWORKORDER\" WHERE \"DF_ID\"=\"WO_DF_ID\" and \"WO_SLNO\"='"+ objForm.sFailureId + "'";
        //            //}
        //            NpgsqlCommand = new NpgsqlCommand();
        //            NpgsqlCommand.Parameters.AddWithValue("sWFInitialId", Convert.ToInt32(objForm.sWFInitialId));
        //            dt = objcon.FetchDataTable(strQry, NpgsqlCommand);

        //            if (dt.Rows.Count > 0)
        //            {
        //                objForm.sFailureId = Convert.ToString(dt.Rows[0]["WO_RECORD_ID"]);
        //                objForm.sTaskType = Convert.ToString(dt.Rows[0]["DF_STATUS_FLAG"]);
        //                if (dt.Columns.Contains("EST_FAIL_TYPE"))
        //                {
        //                    objForm.sFailType = Convert.ToString(dt.Rows[0]["EST_FAIL_TYPE"]);
        //                }
        //                else
        //                {
        //                    strQry = "SELECT \"DF_STATUS_FLAG\"|| '~' ||COALESCE(\"EST_GUARANTEETYPE\",'') AS \"DF_STATUS_FLAG\",\"EST_ID\" AS WO_RECORD_ID,\"EST_FAIL_TYPE\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\",\"TBLESTIMATIONDETAILS\" WHERE ";
        //                    strQry += " (SELECT \"WOA_PREV_APPROVE_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_INITIAL_ACTION_ID\" =:sWFInitialId)= \"WO_ID\" ";
        //                    strQry += " AND \"DF_ID\"=\"EST_FAILUREID\" AND  CAST(\"DF_ID\" AS TEXT)=\"WO_DATA_ID\"";
        //                    NpgsqlCommand = new NpgsqlCommand();
        //                    NpgsqlCommand.Parameters.AddWithValue("sWFInitialId", Convert.ToInt32(objForm.sWFInitialId));
        //                    dt = objcon.FetchDataTable(strQry, NpgsqlCommand);
        //                    if (dt.Rows.Count > 0)
        //                    {
        //                        objForm.sFailureId = Convert.ToString(dt.Rows[0]["WO_RECORD_ID"]);
        //                        objForm.sTaskType = Convert.ToString(dt.Rows[0]["DF_STATUS_FLAG"]);
        //                        objForm.sFailType = Convert.ToString(dt.Rows[0]["EST_FAIL_TYPE"]);
        //                    }
        //                }

        //            }
        //            else
        //            {
        //                strQry = "SELECT \"WO_RECORD_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE ";
        //                strQry += " (SELECT \"WOA_PREV_APPROVE_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_INITIAL_ACTION_ID\" =:sWFInitialId)= \"WO_ID\" ";
        //                NpgsqlCommand = new NpgsqlCommand();
        //                NpgsqlCommand.Parameters.AddWithValue("sWFInitialId", Convert.ToInt32(objForm.sWFInitialId));
        //                objForm.sFailureId = objcon.get_value(strQry, NpgsqlCommand);
        //                objForm.sTaskType = "3";
        //            }
        //        }

        //        else /*(dt.Rows.Count == 0)*/
        //        {
        //            strQry = "SELECT \"EST_FAIL_TYPE\",\"EST_ID\",\"DF_STATUS_FLAG\"|| '~' ||COALESCE(\"EST_GUARANTEETYPE\",'') AS \"DF_STATUS_FLAG\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLWORKORDER\",\"TBLESTIMATIONDETAILS\",";
        //            strQry += " \"TBLWO_OBJECT_AUTO\",\"TBLDTCFAILURE\" WHERE \"WO_RECORD_ID\"=\"WO_SLNO\" AND \"WO_DF_ID\"=\"EST_FAILUREID\" AND ";
        //            strQry += " \"WOA_INITIAL_ACTION_ID\" =\"WO_ID\" AND \"WO_DF_ID\"=\"EST_FAILUREID\" AND \"DF_ID\"=\"WO_DF_ID\" AND \"WO_ID\"=:sWFInitialId";
        //            NpgsqlCommand = new NpgsqlCommand();
        //            NpgsqlCommand.Parameters.AddWithValue("sWFInitialId", Convert.ToInt32(objForm.sWFInitialId));
        //            dt = objcon.FetchDataTable(strQry, NpgsqlCommand);

        //            if (dt.Rows.Count > 0)
        //            {
        //                objForm.sFailureId = Convert.ToString(dt.Rows[0]["EST_ID"]);
        //                objForm.sTaskType = Convert.ToString(dt.Rows[0]["DF_STATUS_FLAG"]);
        //                objForm.sFailType = Convert.ToString(dt.Rows[0]["EST_FAIL_TYPE"]);
        //            }
        //        }



        //        return objForm;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return objForm;
        //    }
        //}
        #endregion
        #region Decommission
        //public string GetStatusFlagForDecommissionFromInvoice(clsFormValues objForm)
        //{
        //    try
        //    {
        //        string strQry = string.Empty;
        //        string sResult = string.Empty;


        //        strQry = "SELECT \"DF_STATUS_FLAG\" from \"TBLWORKORDER\",\"TBLDTCFAILURE\" WHERE \"DF_ID\"=\"WO_DF_ID\" and \"WO_SLNO\"=:sWorkOrderId";
        //       // strQry = "SELECT \"DF_STATUS_FLAG\" FROM \"TBLWORKORDER\",\"TBLDTCFAILURE\",\"TBLINDENT\",\"TBLDTCINVOICE\" WHERE \"DF_ID\"=\"WO_DF_ID\"  ";
        //       // strQry += " AND \"WO_SLNO\"=\"TI_WO_SLNO\" AND \"TI_ID\"=\"IN_TI_NO\"  AND \"IN_NO\" =:sInvoiceId";
        //        NpgsqlCommand = new NpgsqlCommand();
        //        NpgsqlCommand.Parameters.AddWithValue("sWorkOrderId", Convert.ToInt32(objForm.sWorkOrderId));
        //        sResult = objcon.get_value(strQry, NpgsqlCommand);
        //        if (sResult == "")
        //        {
        //            //string sInvoiceId = GetInvoiceId(objForm.sDecommisionId);
        //            strQry = "SELECT \"WO_NO\" FROM \"TBLWORKORDER\",\"TBLINDENT\",\"TBLDTCINVOICE\" WHERE \"WO_SLNO\"=\"TI_WO_SLNO\" AND \"WO_DF_ID\" IS NULL ";
        //            strQry += " AND \"IN_NO\" =:sInvoiceId AND \"TI_ID\"=\"IN_TI_NO\" ";
        //            NpgsqlCommand = new NpgsqlCommand();
        //            NpgsqlCommand.Parameters.AddWithValue("sInvoiceId", Convert.ToInt32(objForm.sInvoiceId));
        //            sResult = objcon.get_value(strQry, NpgsqlCommand);
        //            if (sResult != "")
        //            {
        //                sResult = "3";
        //            }
        //        }
        //        else
        //        {
        //            return sResult;
        //        }

        //        return sResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return ex.Message;
        //    }
        //}
        //public clsFormValues GetWOnoForDTCCommission(clsFormValues objForm)
        //{
        //    try
        //    {
        //        string strQry = string.Empty;
        //        DataTable dt = new DataTable();

        //        //string sInvoiceId = GetInvoiceId(objForm.sDecommisionId);

        //        strQry = "SELECT \"TD_TC_NO\",\"WO_SLNO\" FROM \"TBLWORKORDER\",\"TBLINDENT\",\"TBLDTCINVOICE\",\"TBLTCDRAWN\" WHERE ";
        //        strQry += " \"WO_SLNO\"=\"TI_WO_SLNO\" AND \"TI_ID\"=\"IN_TI_NO\" AND \"IN_NO\"=\"TD_INV_NO\" AND \"IN_NO\" =:sInvoiceId";
        //        NpgsqlCommand = new NpgsqlCommand();
        //        NpgsqlCommand.Parameters.AddWithValue("sInvoiceId", Convert.ToInt32(objForm.sInvoiceId));
        //        dt = objcon.FetchDataTable(strQry, NpgsqlCommand);
        //        if (dt.Rows.Count > 0)
        //        {
        //            objForm.sWorkOrderId = Convert.ToString(dt.Rows[0]["WO_SLNO"]);
        //            objForm.sTCcode = Convert.ToString(dt.Rows[0]["TD_TC_NO"]);
        //        }
        //        return objForm;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return objForm;
        //    }
        //}
        #endregion

        #region Invoice
        //public clsFormValues GetIDForRepairerinviceCreation(clsFormValues objForm)
        //{
        //    try
        //    {
        //        string strQry = string.Empty;
        //        DataTable dt = new DataTable();


        //        strQry = "SELECT \"DF_STATUS_FLAG\",\"WO_RECORD_ID\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\",\"TBLWORKORDER\" WHERE ";
        //        strQry += " (SELECT \"WOA_PREV_APPROVE_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_INITIAL_ACTION_ID\" =:sWFInitialId)= \"WO_ID\"";
        //        strQry += " AND  \"WO_SLNO\"=\"WO_RECORD_ID\" AND \"DF_ID\"=\"WO_DF_ID\"";
        //        NpgsqlCommand = new NpgsqlCommand();
        //        NpgsqlCommand.Parameters.AddWithValue("sWFInitialId", Convert.ToInt32(objForm.sWFInitialId));
        //        dt = objcon.FetchDataTable(strQry, NpgsqlCommand);
        //        if (dt.Rows.Count > 0)
        //        {
        //            objForm.sWorkOrderId = Convert.ToString(dt.Rows[0]["WO_RECORD_ID"]);
        //            objForm.sTaskType = Convert.ToString(dt.Rows[0]["DF_STATUS_FLAG"]);
        //        }
        //        else
        //        {

        //            strQry = "SELECT \"WO_RECORD_ID\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLWORKORDER\" WHERE ";
        //            strQry += " (SELECT \"WOA_PREV_APPROVE_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_INITIAL_ACTION_ID\" =:sWFInitialId)= \"WO_ID\" ";
        //            strQry += " AND  \"WO_SLNO\"=\"WO_RECORD_ID\" AND \"WO_DF_ID\" IS NULL";
        //            NpgsqlCommand = new NpgsqlCommand();
        //            NpgsqlCommand.Parameters.AddWithValue("sWFInitialId", Convert.ToInt32(objForm.sWFInitialId));
        //            objForm.sWorkOrderId = objcon.get_value(strQry, NpgsqlCommand);

        //            objForm.sTaskType = "3";

        //        }
        //        return objForm;

        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return objForm;
        //    }
        //}
        #endregion
        //public string GetFailureIdFromWO(string sWOId)
        //{
        //    try
        //    {
        //        string strQry = string.Empty;
        //        strQry = "SELECT \"WO_DF_ID\" FROM \"TBLWORKORDER\" WHERE \"WO_SLNO\" =:sWOId";
        //        NpgsqlCommand = new NpgsqlCommand();
        //        NpgsqlCommand.Parameters.AddWithValue("sWOId", Convert.ToInt32(sWOId));
        //        return objcon.get_value(strQry, NpgsqlCommand);
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return ex.Message;
        //    }
        //}
        //public string Getwoid(string sIndentId)
        //{
        //    try
        //    {
        //        string strQry = string.Empty;
        //        strQry = "SELECT \"PTI_WO_SLNO\" FROM \"TBLPERMANENTINDENT\" WHERE  \"PTI_ID\" =:sIndentId";
        //        NpgsqlCommand = new NpgsqlCommand();
        //        NpgsqlCommand.Parameters.AddWithValue("sIndentId", Convert.ToInt32(sIndentId));
        //        return objcon.get_value(strQry, NpgsqlCommand);
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return ex.Message;
        //    }
        //}
        //public string GetIndentIdbyestimateid(string sEstId)
        //{
        //    try
        //    {
        //        string strQry = string.Empty;
        //        strQry = "SELECT \"PTI_ID\" from \"TBLPERMANENTINDENT\" INNER join \"TBLPERMANENTWORKORDER\" on \"PTI_WO_SLNO\"=\"PWO_SLNO\" INNER JOIN \"TBLPERMANENTESTIMATIONDETAILS\" on \"PEST_ID\"=\"PWO_PEF_ID\" WHERE \"PEST_ID\"=:sEstId";
        //        NpgsqlCommand = new NpgsqlCommand();
        //        NpgsqlCommand.Parameters.AddWithValue("sEstId", Convert.ToInt32(sEstId));
        //        return objcon.get_value(strQry, NpgsqlCommand);
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return ex.Message;
        //    }
        //}
        #endregion
    }
}
