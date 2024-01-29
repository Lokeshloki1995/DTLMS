using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IIITS.PGSQL.DAL;
using Npgsql;
using IIITS.DTLMS.BL.DataBase;

namespace IIITS.DTLMS.BL
{
    public class clsAprovalHistory
    {
        string strFormCode = "clsAprovalHistory";
        PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
        DataBseConnection ObjBseCon = new DataBseConnection(Constants.Password);

        public string sRecordId { get; set; }
        public string sBOId { get; set; }
        public string sDescription { get; set; }
        public string sStatus { get; set; }

        public string sDTCCode { get; set; }
        public string sDTCName { get; set; }
        public string sDTRCode { get; set; }
        public string sroletype { get; set; }
        public String sroleID { get; set; }
        public string OfcCode { get; set; }

        NpgsqlCommand NpgsqlCommand;
        public DataTable LoadApprovalHistory(string sRecordId, string sBOId)
        {
            DataTable dt = new DataTable();
            try
            {
                //NpgsqlCommand = new NpgsqlCommand();
                //string strQry = string.Empty;
                //string OfficeCode = string.Empty;

                //strQry = " SELECT \"WO_ID\", (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"WO_CR_BY\" = \"US_ID\") INITIATOR , TO_CHAR(\"WO_CR_ON\",'DD-MON-YY HH:MI AM') WO_CR_ON, ";
                //strQry += " (SELECT \"WO_USER_COMMENT\"  FROM \"TBLWORKFLOWOBJECTS\" A  WHERE A.\"WO_ID\" = B.\"WO_PREV_APPROVE_ID\") WO_USER_COMMENT,\"WO_REF_OFFCODE\" FROM \"TBLWORKFLOWOBJECTS\" B  ";
                //strQry += "  WHERE \"WO_RECORD_ID\" =:sRecordId AND \"WO_BO_ID\" =:sBOId ORDER BY \"WO_ID\" ";
                //NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(sRecordId));
                //NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(sBOId));
                //dt = objcon.FetchDataTable(strQry, NpgsqlCommand);


                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_approval_history_clsapprovalhistory");
                cmd.Parameters.AddWithValue("record_id", (sRecordId ?? ""));
                cmd.Parameters.AddWithValue("bo_id", (sBOId ?? ""));
                dt = objcon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public clsAprovalHistory GetStatusofApproval(clsAprovalHistory objHistory)
        {
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();
                DataSet dset = new DataSet();
                string p_key = string.Empty;

                if (objHistory.sroletype != "2")
                {
                    if (objHistory.sBOId == "1012")
                    {
                        //NpgsqlCommand = new NpgsqlCommand();
                        //strQry = "SELECT \"STATUS\",\"WO_DESCRIPTION\" FROM (";
                        //strQry += " SELECT CASE WHEN \"WO_APPROVE_STATUS\" = 1  THEN 'APPROVED' ELSE 'PENDING WITH ' || (SELECT \"RO_NAME\" FROM \"TBLROLES\" WHERE \"WO_NEXT_ROLE\" = \"RO_ID\") || ' - ' ||";
                        //strQry += " (SELECT \"SM_NAME\" FROM \"TBLSTOREOFFCODE\",\"TBLSTOREMAST\"  WHERE \"STO_SM_ID\"=\"SM_ID\" and  cast(\"STO_OFF_CODE\" as text)=substr(cast(A.\"WO_REF_OFFCODE\" as text),'1','3')) ";
                        //strQry += " END \"STATUS\",\"WO_DESCRIPTION\",\"WO_REF_OFFCODE\" FROM \"TBLWORKFLOWOBJECTS\" A WHERE \"WO_RECORD_ID\" =:sRecordId AND \"WO_BO_ID\" =:sBOId ORDER BY \"WO_ID\" DESC)A LIMIT 1";
                        //NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objHistory.sRecordId));
                        //NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objHistory.sBOId));
                        //dt = objcon.FetchDataTable(strQry, NpgsqlCommand);

                        p_key = "BOID_1012";
                        NpgsqlCommand cmd = new NpgsqlCommand("proc_get_status_approval_clsapprovalhistory");
                        cmd.Parameters.AddWithValue("record_id", (objHistory.sRecordId ?? ""));
                        cmd.Parameters.AddWithValue("bo_id", (objHistory.sBOId ?? ""));
                        cmd.Parameters.AddWithValue("ofc_code", "");
                        cmd.Parameters.AddWithValue("p_key", p_key);
                        dset = ObjBseCon.FetchDataSetCursor(cmd);
                        dt = dset.Tables[0];

                        if (dt.Rows.Count > 0)
                        {
                            objHistory.sDescription = Convert.ToString(dt.Rows[0]["WO_DESCRIPTION"]);
                            objHistory.sStatus = Convert.ToString(dt.Rows[0]["STATUS"]);
                        }

                        return objHistory;
                    }


                    if ((objHistory.sroleID != "1" && objHistory.sroleID != "24" && objHistory.sroleID != "3" && objHistory.sroleID != "6" && objHistory.sroleID != "4" && objHistory.sroleID != "7"))
                    {
                        //NpgsqlCommand = new NpgsqlCommand();
                        //strQry = "SELECT \"STATUS\",\"WO_DESCRIPTION\" FROM (";
                        //strQry += " SELECT CASE WHEN \"WO_APPROVE_STATUS\" = 1 THEN 'APPROVED' ELSE 'PENDING WITH ' || (SELECT \"RO_NAME\" FROM \"TBLROLES\" WHERE \"WO_NEXT_ROLE\" = \"RO_ID\") || ' - ' ||";
                        //strQry += " (SELECT \"OFF_NAME\" FROM \"VIEW_ALL_OFFICES\" WHERE \"OFF_CODE\" = (SELECT \"WO_OFFICE_CODE\" FROM \"TBLWORKFLOWOBJECTS\" B WHERE A.\"WO_PREV_APPROVE_ID\" = B.\"WO_ID\" )) ";
                        //strQry += " END \"STATUS\",\"WO_DESCRIPTION\" FROM \"TBLWORKFLOWOBJECTS\" A WHERE \"WO_RECORD_ID\" =:sRecordId AND \"WO_BO_ID\" =:sBOId ORDER BY \"WO_ID\" DESC)A LIMIT 1";

                        ////strQry = "SELECT \"STATUS\",\"WO_DESCRIPTION\" FROM (";
                        ////strQry += " SELECT CASE WHEN \"WO_APPROVE_STATUS\" = 1 THEN 'APPROVED' ELSE 'PENDING WITH ' || (SELECT \"RO_NAME\" FROM \"TBLROLES\" WHERE \"WO_NEXT_ROLE\" = \"RO_ID\") || '-' ||";
                        ////strQry += " (SELECT case when \"WO_PREV_APPROVE_ID\" ='0' then (select \"OFF_NAME\" FROM \"VIEW_ALL_OFFICES\" WHERE \"OFF_CODE\" = \"WO_OFFICE_CODE\") else (SELECT \"OFF_NAME\" ";
                        ////strQry += " FROM \"VIEW_ALL_OFFICES\" WHERE \"OFF_CODE\" = (SELECT \"WO_OFFICE_CODE\" FROM \"TBLWORKFLOWOBJECTS\" B WHERE A.\"WO_ID\" = B.\"WO_ID\" )) end ) ";
                        ////strQry += " END \"STATUS\",\"WO_DESCRIPTION\" FROM \"TBLWORKFLOWOBJECTS\" A WHERE \"WO_RECORD_ID\" =:sRecordId AND \"WO_BO_ID\" =:sBOId ORDER BY \"WO_ID\" DESC)A LIMIT 1";
                        //NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objHistory.sRecordId));
                        //NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objHistory.sBOId));
                        //dt = objcon.FetchDataTable(strQry, NpgsqlCommand);

                        p_key = "NOT_OF_ROLE_ID_IS_1_24_3_6";
                        NpgsqlCommand cmd1 = new NpgsqlCommand("proc_get_status_approval_clsapprovalhistory");
                        cmd1.Parameters.AddWithValue("record_id", (objHistory.sRecordId ?? ""));
                        cmd1.Parameters.AddWithValue("bo_id", (objHistory.sBOId ?? ""));
                        cmd1.Parameters.AddWithValue("ofc_code", "");
                        cmd1.Parameters.AddWithValue("p_key", p_key);
                        dset = ObjBseCon.FetchDataSetCursor(cmd1);
                        dt = dset.Tables[0];

                        if (dt.Rows.Count > 0)
                        {
                            objHistory.sDescription = Convert.ToString(dt.Rows[0]["WO_DESCRIPTION"]);
                            objHistory.sStatus = Convert.ToString(dt.Rows[0]["STATUS"]);
                        }
                    }
                    else if (objHistory.sroleID == "1")
                    {
                        //NpgsqlCommand = new NpgsqlCommand();

                        //strQry = "SELECT \"STATUS\",\"WO_DESCRIPTION\" FROM (";
                        //strQry += " SELECT CASE WHEN \"WO_APPROVE_STATUS\" = 1 THEN 'APPROVED' ELSE 'PENDING WITH ' || (SELECT \"RO_NAME\" FROM \"TBLROLES\" WHERE \"WO_NEXT_ROLE\" = \"RO_ID\") || '-' ||";
                        //strQry += " (SELECT case when \"WO_PREV_APPROVE_ID\" ='0' then (select \"OFF_NAME\" FROM \"VIEW_ALL_OFFICES\" WHERE ";
                        //strQry += " \"OFF_CODE\" = (SELECT  \"OM_SUBDIV_CODE\"  FROM \"TBLOMSECMAST\" INNER JOIN \"TBLSUBDIVMAST\"  ON ";
                        //strQry += " \"SD_SUBDIV_CODE\"=\"OM_SUBDIV_CODE\" WHERE \"OM_CODE\"=:OfcCode)) else (SELECT \"OFF_NAME\" FROM \"VIEW_ALL_OFFICES\" ";
                        //strQry += " WHERE \"OFF_CODE\" = (SELECT \"WO_OFFICE_CODE\" FROM \"TBLWORKFLOWOBJECTS\" B WHERE A.\"WO_PREV_APPROVE_ID\" = B.\"WO_ID\"  )) end ) ";
                        //strQry += " END \"STATUS\",\"WO_DESCRIPTION\" FROM \"TBLWORKFLOWOBJECTS\" A WHERE \"WO_RECORD_ID\" =:sRecordId AND \"WO_BO_ID\" =:sBOId ORDER BY \"WO_ID\" DESC)A LIMIT 1";

                        //NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objHistory.sRecordId));
                        //NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objHistory.sBOId));
                        //NpgsqlCommand.Parameters.AddWithValue("OfcCode", Convert.ToInt32(objHistory.OfcCode));

                        p_key = "ROLE_ID_IS_1";
                        NpgsqlCommand cmd2 = new NpgsqlCommand("proc_get_status_approval_clsapprovalhistory");
                        cmd2.Parameters.AddWithValue("record_id", (objHistory.sRecordId ?? ""));
                        cmd2.Parameters.AddWithValue("bo_id", (objHistory.sBOId ?? ""));
                        cmd2.Parameters.AddWithValue("ofc_code", (objHistory.OfcCode ?? ""));
                        cmd2.Parameters.AddWithValue("p_key", p_key);
                        dset = ObjBseCon.FetchDataSetCursor(cmd2);
                        dt = dset.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            objHistory.sDescription = Convert.ToString(dt.Rows[0]["WO_DESCRIPTION"]);
                            objHistory.sStatus = Convert.ToString(dt.Rows[0]["STATUS"]);
                        }
                    }
                    else if (objHistory.sroleID == "24" || objHistory.sroleID == "3" || objHistory.sroleID == "6" || objHistory.sroleID == "7")
                    {
                        //NpgsqlCommand = new NpgsqlCommand();

                        //strQry = "SELECT \"STATUS\",\"WO_DESCRIPTION\" FROM (";
                        //strQry += " SELECT CASE WHEN \"WO_APPROVE_STATUS\" = 1 THEN 'APPROVED' ELSE 'PENDING WITH ' || (SELECT \"RO_NAME\" FROM \"TBLROLES\" WHERE \"WO_NEXT_ROLE\" = \"RO_ID\") || ' - ' ||";
                        //strQry += " (SELECT \"OFF_NAME\" FROM \"VIEW_ALL_OFFICES\" WHERE \"OFF_CODE\" = (SELECT  \"DIV_CODE\"  FROM \"TBLOMSECMAST\" ";
                        //strQry += " INNER JOIN \"TBLSUBDIVMAST\"  ON \"SD_SUBDIV_CODE\"=\"OM_SUBDIV_CODE\"  inner join \"TBLDIVISION\" on ";
                        //strQry += " \"SD_DIV_CODE\"=\"DIV_CODE\" WHERE CAST(\"OM_CODE\" AS TEXT) LIKE '"+objHistory.OfcCode+"%' LIMIT 1)) ";
                        //strQry += " END \"STATUS\",\"WO_DESCRIPTION\" FROM \"TBLWORKFLOWOBJECTS\" A WHERE \"WO_RECORD_ID\" =:sRecordId AND \"WO_BO_ID\" =:sBOId ORDER BY \"WO_ID\" DESC)A LIMIT 1";

                        //NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objHistory.sRecordId));
                        //NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objHistory.sBOId));
                        //dt = objcon.FetchDataTable(strQry, NpgsqlCommand);

                        p_key = "ROLE_ID_24_3_6";
                        NpgsqlCommand cmd3 = new NpgsqlCommand("proc_get_status_approval_clsapprovalhistory");
                        cmd3.Parameters.AddWithValue("record_id", (objHistory.sRecordId ?? ""));
                        cmd3.Parameters.AddWithValue("bo_id", (objHistory.sBOId ?? ""));
                        cmd3.Parameters.AddWithValue("ofc_code", (objHistory.OfcCode ?? ""));
                        cmd3.Parameters.AddWithValue("p_key", p_key);
                        dset = ObjBseCon.FetchDataSetCursor(cmd3);
                        dt = dset.Tables[0];

                        if (dt.Rows.Count > 0)
                        {
                            objHistory.sDescription = Convert.ToString(dt.Rows[0]["WO_DESCRIPTION"]);
                            objHistory.sStatus = Convert.ToString(dt.Rows[0]["STATUS"]);
                        }
                    }
                    else if (objHistory.sroleID == "4")
                    {
                        p_key = "ROLE_ID_4";
                        NpgsqlCommand cmd3 = new NpgsqlCommand("proc_get_status_approval_clsapprovalhistory");
                        cmd3.Parameters.AddWithValue("record_id", (objHistory.sRecordId ?? ""));
                        cmd3.Parameters.AddWithValue("bo_id", (objHistory.sBOId ?? ""));
                        cmd3.Parameters.AddWithValue("ofc_code", (objHistory.OfcCode ?? ""));
                        cmd3.Parameters.AddWithValue("p_key", p_key);
                        dset = ObjBseCon.FetchDataSetCursor(cmd3);
                        dt = dset.Tables[0];

                        if (dt.Rows.Count > 0)
                        {
                            objHistory.sDescription = Convert.ToString(dt.Rows[0]["WO_DESCRIPTION"]);
                            objHistory.sStatus = Convert.ToString(dt.Rows[0]["STATUS"]);
                        }
                    }
                }
                else
                {
                    if (objHistory.sBOId == "23" || objHistory.sBOId == "24")
                    {
                        NpgsqlCommand = new NpgsqlCommand();
                        //strQry = "SELECT \"STATUS\",\"WO_DESCRIPTION\" FROM (";
                        //strQry += " SELECT CASE WHEN \"WO_APPROVE_STATUS\" = 1 THEN 'APPROVED' ELSE 'PENDING WITH ' || (SELECT \"RO_NAME\" FROM \"TBLROLES\" WHERE \"WO_NEXT_ROLE\" = \"RO_ID\") || '-' ||";
                        //strQry += " (SELECT \"SM_NAME\" FROM \"TBLSTOREOFFCODE\",\"TBLSTOREMAST\"  WHERE \"STO_SM_ID\"=\"SM_ID\" and  \"STO_OFF_CODE\"  = (SELECT \"WO_REF_OFFCODE\" FROM \"TBLWORKFLOWOBJECTS\" B WHERE A.\"WO_PREV_APPROVE_ID\" = B.\"WO_ID\" )) ";
                        //strQry += " END \"STATUS\",\"WO_DESCRIPTION\" FROM \"TBLWORKFLOWOBJECTS\" A WHERE \"WO_RECORD_ID\" =:sRecordId AND \"WO_BO_ID\" =:sBOId ORDER BY \"WO_ID\" DESC)A LIMIT 1";

                        if (objHistory.sBOId == "23")
                        {
                            //if(objHistory.sroleID=="2")
                            //{
                            //    strQry = "SELECT \"STATUS\",\"WO_DESCRIPTION\" FROM (";
                            //    strQry += " SELECT 'PENDING WITH STO' AS \"STATUS\",\"WO_DESCRIPTION\" FROM \"TBLWORKFLOWOBJECTS\" A WHERE \"WO_RECORD_ID\" =:sRecordId AND \"WO_BO_ID\" =:sBOId ORDER BY \"WO_ID\" DESC)A LIMIT 1";
                            //    NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objHistory.sRecordId));
                            //    NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objHistory.sBOId));
                            //    dt = objcon.FetchDataTable(strQry, NpgsqlCommand);
                            //    if (dt.Rows.Count > 0)
                            //    {
                            //        objHistory.sDescription = Convert.ToString(dt.Rows[0]["WO_DESCRIPTION"]);
                            //        objHistory.sStatus = Convert.ToString(dt.Rows[0]["STATUS"]);
                            //        return objHistory;
                            //    }
                            //}

                            #region old inline query
                            //strQry = " SELECT \"STATUS\",\"WO_DESCRIPTION\" FROM ( SELECT CASE WHEN \"WO_APPROVE_STATUS\" = 1 ";
                            //strQry += " THEN 'APPROVED' ELSE 'PENDING WITH ' || (SELECT \"RO_NAME\" FROM \"TBLROLES\" WHERE ";
                            //strQry += " \"WO_NEXT_ROLE\" = \"RO_ID\") || ' - ' || (select \"OFF_NAME\" FROM \"VIEW_ALL_OFFICES\" ";
                            //strQry += " WHERE \"OFF_CODE\" = (select \"SM_CODE\" from \"TBLSTOREMAST\" where cast(\"SM_ID\" as text)=\"WO_DATA_ID\")) ";
                            //strQry += " END \"STATUS\",\"WO_DESCRIPTION\" FROM \"TBLWORKFLOWOBJECTS\" A WHERE \"WO_RECORD_ID\" =:sRecordId ";
                            //strQry += " AND \"WO_BO_ID\" =:sBOId ORDER BY \"WO_ID\" DESC)A LIMIT 1 ";

                            //NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objHistory.sRecordId));
                            //NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objHistory.sBOId));
                            //dt = objcon.FetchDataTable(strQry, NpgsqlCommand);
                            #endregion

                            p_key = "BO_ID_IS_23";
                            NpgsqlCommand cmd4 = new NpgsqlCommand("proc_get_status_approval_clsapprovalhistory");
                            cmd4.Parameters.AddWithValue("record_id", (objHistory.sRecordId ?? ""));
                            cmd4.Parameters.AddWithValue("bo_id", (objHistory.sBOId ?? ""));
                            cmd4.Parameters.AddWithValue("ofc_code", "");
                            cmd4.Parameters.AddWithValue("p_key", p_key);
                            dset = ObjBseCon.FetchDataSetCursor(cmd4);
                            dt = dset.Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                objHistory.sDescription = Convert.ToString(dt.Rows[0]["WO_DESCRIPTION"]);
                                objHistory.sStatus = Convert.ToString(dt.Rows[0]["STATUS"]);
                                return objHistory;
                            }
                        }

                        //strQry = "SELECT \"STATUS\",\"WO_DESCRIPTION\" FROM (";
                        //strQry += " SELECT CASE WHEN \"WO_APPROVE_STATUS\" = 1 THEN 'APPROVED' ELSE 'PENDING WITH ' || (SELECT \"RO_NAME\" FROM \"TBLROLES\" WHERE \"WO_NEXT_ROLE\" = \"RO_ID\") || ' - ' ||";
                        //strQry += " (SELECT case when \"WO_PREV_APPROVE_ID\" ='0' then (select \"OFF_NAME\" FROM \"VIEW_ALL_OFFICES\" WHERE \"OFF_CODE\" = (select \"SM_CODE\" from \"TBLSTOREMAST\" where \"SM_ID\"=\"WO_OFFICE_CODE\")) else (SELECT \"OFF_NAME\" FROM \"VIEW_ALL_OFFICES\" WHERE \"OFF_CODE\" = (SELECT \"WO_OFFICE_CODE\" FROM \"TBLWORKFLOWOBJECTS\" B WHERE A.\"WO_PREV_APPROVE_ID\" = B.\"WO_ID\" )) end ) ";
                        //strQry += " END \"STATUS\",\"WO_DESCRIPTION\" FROM \"TBLWORKFLOWOBJECTS\" A WHERE \"WO_RECORD_ID\" =:sRecordId AND \"WO_BO_ID\" =:sBOId ORDER BY \"WO_ID\" DESC)A LIMIT 1";
                        //NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objHistory.sRecordId));
                        //NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objHistory.sBOId));
                        //dt = objcon.FetchDataTable(strQry, NpgsqlCommand);

                        p_key = "BO_ID_IS_24";
                        NpgsqlCommand cmd5 = new NpgsqlCommand("proc_get_status_approval_clsapprovalhistory");
                        cmd5.Parameters.AddWithValue("record_id", (objHistory.sRecordId ?? ""));
                        cmd5.Parameters.AddWithValue("bo_id", (objHistory.sBOId ?? ""));
                        cmd5.Parameters.AddWithValue("ofc_code", "");
                        cmd5.Parameters.AddWithValue("p_key", p_key);
                        dset = ObjBseCon.FetchDataSetCursor(cmd5);
                        dt = dset.Tables[0];

                        if (dt.Rows.Count > 0)
                        {
                            objHistory.sDescription = Convert.ToString(dt.Rows[0]["WO_DESCRIPTION"]);
                            objHistory.sStatus = Convert.ToString(dt.Rows[0]["STATUS"]);
                        }

                    }
                    else
                    {
                        //NpgsqlCommand = new NpgsqlCommand();
                        //strQry = "SELECT \"STATUS\",\"WO_DESCRIPTION\" FROM (";
                        //strQry += " SELECT CASE WHEN \"WO_APPROVE_STATUS\" = 1 THEN 'APPROVED' ELSE 'PENDING WITH ' || (SELECT \"RO_NAME\" FROM \"TBLROLES\" WHERE \"WO_NEXT_ROLE\" = \"RO_ID\") || ' - ' ||";
                        //strQry += " (SELECT \"SM_NAME\" FROM \"TBLSTOREOFFCODE\",\"TBLSTOREMAST\"  WHERE \"STO_SM_ID\"=\"SM_ID\" and  cast(\"STO_OFF_CODE\" as text)=substr(cast(A.\"WO_REF_OFFCODE\" as text),'1','3')) ";
                        //strQry += " END \"STATUS\",\"WO_DESCRIPTION\",\"WO_REF_OFFCODE\" FROM \"TBLWORKFLOWOBJECTS\" A WHERE \"WO_RECORD_ID\" =:sRecordId AND \"WO_BO_ID\" =:sBOId ORDER BY \"WO_ID\" DESC)A LIMIT 1";
                        //NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objHistory.sRecordId));
                        //NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objHistory.sBOId));
                        //dt = objcon.FetchDataTable(strQry, NpgsqlCommand);

                        p_key = "NOT_OF_BO_ID_24_23";
                        NpgsqlCommand cmd6 = new NpgsqlCommand("proc_get_status_approval_clsapprovalhistory");
                        cmd6.Parameters.AddWithValue("record_id", (objHistory.sRecordId ?? ""));
                        cmd6.Parameters.AddWithValue("bo_id", (objHistory.sBOId ?? ""));
                        cmd6.Parameters.AddWithValue("ofc_code", "");
                        cmd6.Parameters.AddWithValue("p_key", p_key);
                        dset = ObjBseCon.FetchDataSetCursor(cmd6);
                        dt = dset.Tables[0];

                        if (dt.Rows.Count > 0)
                        {
                            objHistory.sDescription = Convert.ToString(dt.Rows[0]["WO_DESCRIPTION"]);
                            objHistory.sStatus = Convert.ToString(dt.Rows[0]["STATUS"]);
                        }
                    }

                }

                return objHistory;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objHistory;
            }
        }

        public clsAprovalHistory GetDTCDetails(clsAprovalHistory objHistory)
        {
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();
                DataSet dset = new DataSet();

                //if (objHistory.sBOId == "9" || objHistory.sBOId == "10")
                //{
                //    NpgsqlCommand = new NpgsqlCommand();
                //    strQry = "SELECT DISTINCT \"DT_NAME\",\"DT_CODE\",(SELECT \"TC_CODE\" FROM \"TBLTCMASTER\" WHERE \"TC_CODE\"=\"DT_TC_ID\") TC_CODE ";
                //    strQry += " FROM \"TBLDTCMAST\",\"TBLWORKFLOWOBJECTS\" WHERE \"WO_DATA_ID\"=\"DT_CODE\" AND \"WO_RECORD_ID\" =:sRecordId and \"WO_BO_ID\" =:sBOId";
                //    NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objHistory.sRecordId));
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objHistory.sBOId));
                //}
                //if (objHistory.sBOId == "11")
                //{
                //    NpgsqlCommand = new NpgsqlCommand();
                //    //strQry = "SELECT DISTINCT \"DT_NAME\",\"DT_CODE\",\"DF_EQUIPMENT_ID\" TC_CODE FROM \"TBLDTCMAST\",\"TBLWORKFLOWOBJECTS\",";
                //    //strQry += " \"TBLDTCFAILURE\" WHERE \"WO_DATA_ID\"=\"DF_ID\" AND \"WO_RECORD_ID\" ='" + objHistory.sRecordId + "' and \"WO_BO_ID\" ='" + objHistory.sBOId + "' AND \"DF_DTC_CODE\"=\"DT_CODE\"";
                //    strQry = " SELECT DISTINCT \"DT_NAME\",\"DT_CODE\",\"DF_EQUIPMENT_ID\" TC_CODE FROM \"TBLDTCMAST\",\"TBLWORKFLOWOBJECTS\",";
                //    strQry += " \"TBLDTCFAILURE\",\"TBLESTIMATIONDETAILS\" WHERE \"WO_DATA_ID\"=CAST(\"EST_FAILUREID\" AS TEXT) AND \"DF_ID\"=\"EST_FAILUREID\" ";
                //    strQry += " AND \"WO_RECORD_ID\" =:sRecordId and \"WO_BO_ID\" =:sBOId AND \"DF_DTC_CODE\"=\"DT_CODE\"";
                //    NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objHistory.sRecordId));
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objHistory.sBOId));
                //}
                //if (objHistory.sBOId == "12")
                //{
                //    NpgsqlCommand = new NpgsqlCommand();
                //    strQry = "SELECT DISTINCT \"DT_NAME\",\"DT_CODE\",\"DF_EQUIPMENT_ID\" TC_CODE FROM \"TBLDTCMAST\",\"TBLWORKFLOWOBJECTS\",";
                //    strQry += " \"TBLDTCFAILURE\",\"TBLWORKORDER\" WHERE \"WO_DATA_ID\"=CAST(\"WO_SLNO\" as text) AND \"WO_RECORD_ID\" =:sRecordId ";
                //    strQry += " and \"WO_BO_ID\" =:sBOId AND \"DF_DTC_CODE\"=\"DT_CODE\" AND \"DF_ID\"=\"WO_DF_ID\"";
                //    NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objHistory.sRecordId));
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objHistory.sBOId));
                //}
                //if (objHistory.sBOId == "13")
                //{
                //    NpgsqlCommand = new NpgsqlCommand();
                //    strQry = "SELECT DISTINCT \"DT_NAME\",\"DT_CODE\",DF_EQUIPMENT_ID\" TC_CODE FROM \"TBLDTCMAST\",\"TBLWORKFLOWOBJECTS\",";
                //    strQry += " \"TBLDTCFAILURE\",\"TBLWORKORDER\",\"TBLINDENT\" WHERE \"WO_DATA_ID\"=CAST(\"TI_ID\" as text) AND \"WO_RECORD_ID\" =:sRecordId ";
                //    strQry += " and \"WO_BO_ID\" =:sBOId AND \"DF_DTC_CODE\"=\"DT_CODE\" AND \"DF_ID\"=\"WO_DF_ID\" AND \"WO_SLNO\"=\"TI_WO_SLNO\"";
                //    NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objHistory.sRecordId));
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objHistory.sBOId));
                //}
                //if (objHistory.sBOId == "14")
                //{
                //    NpgsqlCommand = new NpgsqlCommand();
                //    strQry = "SELECT DISTINCT \"DT_NAME\",\"DT_CODE\",(SELECT \"TC_CODE\" FROM \"TBLTCMASTER\" WHERE \"TC_CODE\"=\"DT_TC_ID\") TC_CODE ";
                //    strQry += " FROM \"TBLDTCMAST\",\"TBLWORKFLOWOBJECTS\" WHERE \"WO_DATA_ID\"=\"DT_CODE\" AND \"WO_RECORD_ID\"=:sRecordId and \"WO_BO_ID\" =:sBOId";
                //    NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objHistory.sRecordId));
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objHistory.sBOId));
                //}
                //if (objHistory.sBOId == "15")
                //{
                //    NpgsqlCommand = new NpgsqlCommand();
                //    strQry = "SELECT DISTINCT \"DT_NAME\",\"DT_CODE\",(SELECT \"TC_CODE\" FROM \"TBLTCMASTER\" WHERE \"TC_CODE\"=\"DT_TC_ID\") \"TC_CODE\"  ";
                //    strQry += " FROM \"TBLDTCMAST\",\"TBLWORKFLOWOBJECTS\" WHERE \"WO_DATA_ID\"=\"DT_CODE\" AND \"WO_RECORD_ID\" =:sRecordId and \"WO_BO_ID\" =:sBOId";
                //    NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objHistory.sRecordId));
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objHistory.sBOId));
                //}
                //if (objHistory.sBOId == "26")
                //{
                //    NpgsqlCommand = new NpgsqlCommand();
                //    strQry = " SELECT DISTINCT \"DT_NAME\",\"DT_CODE\",\"DF_EQUIPMENT_ID\" TC_CODE FROM \"TBLDTCMAST\",\"TBLWORKFLOWOBJECTS\", ";
                //    strQry += " \"TBLDTCFAILURE\",\"TBLDTCINVOICE\",\"TBLTCREPLACE\",\"TBLTCDRAWN\" WHERE \"WO_RECORD_ID\"=\"TR_WO_SLNO\" AND \"WO_RECORD_ID\"=:sRecordId ";
                //    strQry += " AND \"WO_BO_ID\" =:sBOId AND \"DF_DTC_CODE\"=\"DT_CODE\" AND \"DF_ID\"=\"TD_DF_ID\" AND \"IN_NO\"=\"TD_INV_NO\" AND \"TR_IN_NO\"=\"IN_NO\"";
                //    NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objHistory.sRecordId));
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objHistory.sBOId));
                //}
                //if (objHistory.sBOId == "45")
                //{
                //    NpgsqlCommand = new NpgsqlCommand();
                //    strQry = "SELECT DISTINCT \"DT_NAME\",\"DT_CODE\",\"DF_EQUIPMENT_ID\" AS \"TC_CODE\" FROM \"TBLDTCFAILURE\",\"TBLWORKFLOWOBJECTS\",";
                //    strQry += " \"TBLDTCMAST\" WHERE \"WO_DATA_ID\"=CAST(\"DF_ID\" AS TEXT) AND \"DF_DTC_CODE\"=\"DT_CODE\" AND \"WO_BO_ID\"=:sBOId AND \"WO_RECORD_ID\"=:sRecordId";

                //    NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objHistory.sBOId));
                //    NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objHistory.sRecordId));
                //}
                //if (objHistory.sBOId == "46" || objHistory.sBOId == "48")
                //{
                //    NpgsqlCommand = new NpgsqlCommand();
                //    strQry = "SELECT DISTINCT \"DT_NAME\",\"DT_CODE\",\"DF_EQUIPMENT_ID\" AS \"TC_CODE\" FROM \"TBLDTCFAILURE\",\"TBLWORKORDER\",";
                //    strQry += " \"TBLWORKFLOWOBJECTS\",\"TBLESTIMATIONDETAILS\",\"TBLDTCMAST\" WHERE \"DF_ID\"=\"EST_FAILUREID\" ";
                //    strQry += " AND \"WO_DATA_ID\"=CAST(\"WO_SLNO\" AS TEXT) AND \"DF_ID\"=\"WO_DF_ID\" AND  \"DF_DTC_CODE\"=\"DT_CODE\" ";
                //    strQry += " AND \"WO_BO_ID\"=:sBOId AND \"WO_RECORD_ID\"=:sRecordId";
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objHistory.sBOId));
                //    NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objHistory.sRecordId));
                //}
                //if (objHistory.sBOId == "47")
                //{
                //    NpgsqlCommand = new NpgsqlCommand();
                //    strQry = "SELECT DISTINCT \"DT_NAME\",\"DT_CODE\",\"DF_EQUIPMENT_ID\" AS \"TC_CODE\" FROM \"TBLDTCFAILURE\",\"TBLWORKORDER\",";
                //    strQry += " \"TBLWORKFLOWOBJECTS\",\"TBLESTIMATIONDETAILS\",\"TBLRECEIVEDTR\",\"TBLDTCMAST\" WHERE \"DF_ID\"=\"EST_FAILUREID\" ";
                //    strQry += " AND \"WO_DATA_ID\"=CAST(\"RD_ID\" AS TEXT) AND \"DF_ID\"=\"WO_DF_ID\" AND  \"WO_SLNO\"=\"RD_WO_SLNO\" AND ";
                //    strQry += " \"DF_DTC_CODE\" =\"DT_CODE\" AND \"WO_BO_ID\"=:sBOId AND \"WO_RECORD_ID\"=:sRecordId";
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objHistory.sBOId));
                //    NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objHistory.sRecordId));
                //}

                //if (objHistory.sBOId == "62")
                //{
                //    NpgsqlCommand = new NpgsqlCommand();
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objHistory.sBOId));
                //    NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objHistory.sRecordId));
                //    string stc_id = objcon.get_value("SELECT \"WO_DATA_ID\" from \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\"=:sBOId AND \"WO_RECORD_ID\"=:sRecordId", NpgsqlCommand);
                //    NpgsqlCommand = new NpgsqlCommand();
                //    NpgsqlCommand.Parameters.AddWithValue("stc_id", Convert.ToInt32(stc_id));
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objHistory.sBOId));
                //    NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objHistory.sRecordId));
                //    strQry = " SELECT DISTINCT \"DT_NAME\",\"DT_CODE\",\"TC_CODE\" FROM \"TBLWORKFLOWOBJECTS\", \"TBLDTCMAST\",\"TBLTCMASTER\" WHERE \"TC_ID\"=:stc_id  AND \"WO_BO_ID\"=:sBOId AND \"WO_RECORD_ID\"=:sRecordId ";
                //}
                //if (objHistory.sBOId == "47")
                //{
                //    NpgsqlCommand = new NpgsqlCommand();
                //    strQry = "SELECT DISTINCT \"DT_NAME\",\"DT_CODE\",\"DF_EQUIPMENT_ID\" AS \"TC_CODE\" FROM \"TBLDTCFAILURE\",\"TBLWORKORDER\",";
                //    strQry += " \"TBLWORKFLOWOBJECTS\",\"TBLESTIMATIONDETAILS\",\"TBLRECEIVEDTR\",\"TBLDTCMAST\" WHERE \"DF_ID\"=\"EST_FAILUREID\" ";
                //    strQry += " AND \"WO_DATA_ID\"=CAST(\"RD_ID\" AS TEXT) AND \"DF_ID\"=\"WO_DF_ID\" AND  \"WO_SLNO\"=\"RD_WO_SLNO\" AND ";
                //    strQry += " \"DF_DTC_CODE\" =\"DT_CODE\" AND \"WO_BO_ID\"=:sBOId AND \"WO_RECORD_ID\"=:sRecordId";
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objHistory.sBOId));
                //    NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objHistory.sRecordId));
                //}
                //if (objHistory.sBOId == "1020")
                //{
                //    NpgsqlCommand = new NpgsqlCommand();
                //    strQry = "SELECT DISTINCT \"PI_DTC_NAME\" AS \"DT_NAME\",\"PI_DTCCODE\" AS \"DT_CODE\",\"PI_TC_CODE\" AS \"TC_CODE\" from ";
                //    strQry += " \"TBLTEMP_PMC_INDENT_ENUMDETAILS\" INNER JOIN \"TBLPMC_INDENT\" ON \"PI_TPIE_ID\"=\"TPIE_ID\" ";
                //    strQry += " INNER JOIN \"TBLPMC_INVOICE\" ON \"PI_ID\"=\"PMC_PI_ID\" WHERE \"PMC_ID\"=:sRecordId";
                //    // NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objHistory.sBOId));
                //    NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objHistory.sRecordId));
                //}

                //if(strQry != "" && strQry != null)
                //{
                //    dt = objcon.FetchDataTable(strQry, NpgsqlCommand);
                //}

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_dtc_details_clsapprovalhistory");
                cmd.Parameters.AddWithValue("record_id", (objHistory.sRecordId ?? ""));
                cmd.Parameters.AddWithValue("bo_id", (objHistory.sBOId ?? ""));
                dset = ObjBseCon.FetchDataSetCursor(cmd);
                dt = dset.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    objHistory.sDTCCode = Convert.ToString(dt.Rows[0]["DT_CODE"]);
                    objHistory.sDTCName = Convert.ToString(dt.Rows[0]["DT_NAME"]);
                    objHistory.sDTRCode = Convert.ToString(dt.Rows[0]["TC_CODE"]);
                }
                return objHistory;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objHistory;
            }
        }

        public DataTable LoadApprovalFullHistory(string sRecordId, string sBOId, string sDataId, string sAutoID)
        {
            DataTable dt = new DataTable();
            DataTable dtFinalDetails = new DataTable();
            DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);

            try
            {
                string sQry = string.Empty;
                string sFetch = string.Empty;
                string p_key = string.Empty;

                if (sDataId == "0")
                {
                    ////  NpgsqlCommand = new NpgsqlCommand();
                    //  sQry = "SELECT \"WO_RECORD_ID\"||'~' ||\"WO_BO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\" ='"+ Convert.ToInt32(sAutoID) + "'";
                    ////  NpgsqlCommand.Parameters.AddWithValue("sAutoID", Convert.ToInt32(sAutoID));
                    //  string sValue = objcon.get_value(sQry);

                    p_key = "DATA_ID_IS_0";
                    NpgsqlCommand cmd = new NpgsqlCommand("proc_get_val_of_load_approval_full_history");
                    cmd.Parameters.AddWithValue("wo_id", (sAutoID ?? ""));
                    cmd.Parameters.AddWithValue("p_key", p_key);
                    cmd.Parameters.AddWithValue("record_id", "");
                    cmd.Parameters.AddWithValue("bo_id", "");

                    string sValue = objDatabse.StringGetValue(cmd);

                    if (sValue.Length > 0)
                    {
                        sRecordId = sValue.Split('~').GetValue(0).ToString();
                        sBOId = sValue.Split('~').GetValue(1).ToString();
                    }
                }

                if (sBOId == "1020" && (sRecordId ?? "") == "")
                {
                    ////  NpgsqlCommand = new NpgsqlCommand();
                    //sQry = "SELECT \"WO_RECORD_ID\"||'~' ||\"WO_BO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\" ='" + Convert.ToInt32(sAutoID) + "'";
                    ////  NpgsqlCommand.Parameters.AddWithValue("sAutoID", Convert.ToInt32(sAutoID));
                    //string sValue = objcon.get_value(sQry);


                    p_key = "BO_ID_IS_1020_AND_RECORD_ID_IS_NULL";
                    NpgsqlCommand cmd1 = new NpgsqlCommand("proc_get_val_of_load_approval_full_history");
                    cmd1.Parameters.AddWithValue("wo_id", (sAutoID ?? ""));
                    cmd1.Parameters.AddWithValue("p_key", p_key);
                    cmd1.Parameters.AddWithValue("record_id", "");
                    cmd1.Parameters.AddWithValue("bo_id", "");

                    string sValue = objDatabse.StringGetValue(cmd1);

                    if (sValue.Length > 0)
                    {
                        sRecordId = sValue.Split('~').GetValue(0).ToString();
                        sBOId = sValue.Split('~').GetValue(1).ToString();
                    }
                }

                if (sRecordId == "" || sRecordId == null || sRecordId == string.Empty)
                {
                    sRecordId = "0";
                }
                if (sBOId == "" || sBOId == null || sBOId == string.Empty)
                {
                    sBOId = "0";
                }
                LOOP:
                ////  NpgsqlCommand = new NpgsqlCommand();
                //sQry = " SELECT \"WO_ID\", (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"WO_CR_BY\" = \"US_ID\") \"INITIATOR\" , ";
                //sQry += " (SELECT \"BO_NAME\" FROM \"TBLBUSINESSOBJECT\" WHERE \"BO_ID\" = \"WO_BO_ID\" ) \"BONAME\" ,  ";
                //sQry += " TO_CHAR(\"WO_CR_ON\",'DD-MON-YYYY HH:MI AM') WO_CR_ON, (SELECT \"WO_USER_COMMENT\"  FROM \"TBLWORKFLOWOBJECTS\" A  ";
                //sQry += " WHERE A.\"WO_ID\" = B.\"WO_PREV_APPROVE_ID\") WO_USER_COMMENT FROM \"TBLWORKFLOWOBJECTS\" B  ";
                //sQry += " WHERE \"WO_RECORD_ID\" ='" + Convert.ToInt32(sRecordId) + "' AND \"WO_BO_ID\" ='" + Convert.ToInt32(sBOId) + "' ORDER BY \"WO_ID\" ";

                ////  NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(sRecordId));
                ////  NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(sBOId));
                //dt = objcon.FetchDataTable(sQry);

                NpgsqlCommand cmd4 = new NpgsqlCommand("proc_load_approval_full_history");
                cmd4.Parameters.AddWithValue("record_id", (sRecordId ?? ""));
                cmd4.Parameters.AddWithValue("bo_id", (sBOId ?? ""));
                dt = objcon.FetchDataTable(cmd4);

                if (dtFinalDetails.Rows.Count == 0)
                {
                    dtFinalDetails = objcon.FetchDataTable(cmd4);
                }
                else
                {
                    dtFinalDetails.Merge(dt);
                }

                //  NpgsqlCommand = new NpgsqlCommand();
                if (sRecordId == "" || sRecordId == null || sRecordId == string.Empty)
                {
                    sRecordId = "0";
                }

                if (sBOId == "" || sBOId == null || sBOId == string.Empty)
                {
                    sBOId = "0";
                }
                sFetch = "SELECT MIN(\"WO_ID\")  FROM \"TBLWORKFLOWOBJECTS\" WHERE  \"WO_RECORD_ID\" ='" + Convert.ToInt32(sRecordId) + "' AND \"WO_BO_ID\" ='" + Convert.ToInt32(sBOId) + "'";

                // NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(sRecordId));
                // NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(sBOId));
                string sMinWoId = objcon.get_value(sFetch);
                if (sMinWoId == "")
                {

                    sFetch = "SELECT \"WOA_PREV_APPROVE_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_INITIAL_ACTION_ID\" = '0' ";
                }
                else
                {
                    //NpgsqlCommand = new NpgsqlCommand();
                    sFetch = "SELECT \"WOA_PREV_APPROVE_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_INITIAL_ACTION_ID\" ='" + Convert.ToInt32(sMinWoId) + "' ";
                    //  NpgsqlCommand.Parameters.AddWithValue("sMinWoId", Convert.ToInt32(sMinWoId));

                }

                string sPrevAppId = objcon.get_value(sFetch);

                //p_key = "MIN_WO_ID";
                //NpgsqlCommand cmd2 = new NpgsqlCommand("proc_get_val_of_load_approval_full_history");
                //cmd2.Parameters.AddWithValue("wo_id", "");
                //cmd2.Parameters.AddWithValue("p_key", p_key);
                //cmd2.Parameters.AddWithValue("record_id", (sRecordId ?? ""));
                //cmd2.Parameters.AddWithValue("bo_id", (sBOId ?? ""));

                //string sPrevAppId = objDatabse.StringGetValue(cmd2);

                if (sPrevAppId.Length == 0)
                {
                    DataView dv = dtFinalDetails.DefaultView;

                    if (dv.Count > 0)
                    {
                        dv.Sort = "WO_ID ASC";
                        dtFinalDetails = dv.ToTable();
                    }


                    return dtFinalDetails;
                }
                ////  NpgsqlCommand = new NpgsqlCommand();
                //  sFetch = "SELECT \"WO_BO_ID\" || '~' || \"WO_RECORD_ID\"  FROM \"TBLWORKFLOWOBJECTS\" WHERE  \"WO_ID\" ='"+ Convert.ToInt32(sPrevAppId) + "'";
                ////  NpgsqlCommand.Parameters.AddWithValue("sPrevAppId", Convert.ToInt32(sPrevAppId));
                //  string sdetails = objcon.get_value(sFetch);

                p_key = "BO_ID_AND_RECORD_ID";
                NpgsqlCommand cmd3 = new NpgsqlCommand("proc_get_val_of_load_approval_full_history");
                cmd3.Parameters.AddWithValue("wo_id", (sPrevAppId ?? ""));
                cmd3.Parameters.AddWithValue("p_key", p_key);
                cmd3.Parameters.AddWithValue("record_id", "");
                cmd3.Parameters.AddWithValue("bo_id", "");
                string sdetails = objDatabse.StringGetValue(cmd3);

                if (sdetails.Length > 0)
                {
                    sBOId = sdetails.Split('~').GetValue(0).ToString();
                    sRecordId = sdetails.Split('~').GetValue(1).ToString();
                }

                goto LOOP;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
    }
}
