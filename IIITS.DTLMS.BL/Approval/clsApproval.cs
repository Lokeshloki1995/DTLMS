//**************************** Work Flow Concept******************************//
//*************************** Logic By : Ramesh Sir **************************//
//*************************** Code By : Priya *********************************//
//*************************** Last Update : 24/06/2016********************** //


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using System.Xml.XPath;
using IIITS.DTLMS.BL;
using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;
using System.Configuration;
using System.Net;
using IIITS.DTLMS.BL.DataBase;

namespace IIITS.DTLMS.BL
{
    public class clsApproval
    {
        string strFormCode = "clsApproval";

        public string sfailid { get; set; }
        public string sbfm_type { get; set; }
        public string sbfm_id { get; set; }

        public string sdtccode { get; set; }
        public string sRoleId { get; set; }
        public string sBOId { get; set; }
        public string sAccessType { get; set; }
        public string sFormName { get; set; }
        public string sRefTypeforInbox { get; set; }
        public string sRecordId { get; set; }
        public string sOfficeCode { get; set; }
        public string sApproveStatus { get; set; }
        public string sPrevApproveId { get; set; }
        public string sClientIp { get; set; }
        public string sApproveComments { get; set; }
        public string sWFObjectId { get; set; }
        public string sQryValues { get; set; }
        public string sDescription { get; set; }
        public string sParameterValues { get; set; }
        public string sXMLdata { get; set; }
        public string sTableNames { get; set; }
        public string sColumnNames { get; set; }
        public string sColumnValues { get; set; }
        public string sCrby { get; set; }
        public string sMainTable { get; set; }
        public string sRefColumnName { get; set; }
        public string sApproveColumnName { get; set; }
        public string sBOFlowMasterId { get; set; }
        public string sPrevWFOId { get; set; }
        public string sWFAutoId { get; set; }
        public string sWFDataId { get; set; }
        public string sNewRecordId { get; set; }
        public string sWFInitialId { get; set; }
        public string sDataReferenceId { get; set; }
        public string sRefOfficeCode { get; set; }
        public string sFromDate { get; set; }
        public string sToDate { get; set; }
        public string sFailType { get; set; }
        public string sRoleType { get; set; }
        public string sGuarentyType { get; set; }
        public string sStoreType { get; set; }
        public string sFilePath { get; set; }

        public string sMaterialfoloi { get; set; }
        public string sStatus { get; set; }
        public string sTTKSComstatus { get; set; }
        int Division;
        int SubDivision;
        int Section;
        //Approve Status
        // 0--->Pending    1---->Approved    2----> Modify and Approve   3--> Reject   4----> Abort

        PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
        DataBseConnection ObjBasCon = new DataBseConnection(Constants.Password);
        /// <summary>
        /// To check Access of Forms Based on Roles
        /// </summary>
        /// <param name="objApproval">Role Id, Form Name</param>
        /// <returns></returns>
        /// 
        NpgsqlCommand NpgsqlCommand;
        public string sTTKStatus { get; set; }

        public bool CheckAccessRights(clsApproval objApproval)
        {

            string strQry = string.Empty;
            DataTable dt = new DataTable();
            sFailType = "0";
            try
            {
                if (Convert.ToString(ConfigurationManager.AppSettings["AccessRights"]).ToUpper().Equals("OFF"))
                {
                    return true;
                }
                #region old inline query
                //NpgsqlCommand = new NpgsqlCommand();
                //// 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY
                //strQry = "SELECT \"UR_ACCESSTYPE\" FROM \"TBLUSERROLEMAPPING\" WHERE \"UR_ROLEID\" ='" + objApproval.sRoleId + "' AND \"UR_BOID\" IN ";
                //strQry += " (SELECT \"BO_ID\" FROM \"TBLBUSINESSOBJECT\" WHERE UPPER(\"BO_FORMNAME\")='" + objApproval.sFormName.Trim().ToUpper() + "') ";
                //strQry += " AND \"UR_ACCESSTYPE\" IN (" + objApproval.sAccessType + ")  ORDER BY \"UR_ACCESSTYPE\"";
                //dt = objcon.FetchDataTable(strQry);
                #endregion 

                #region Converted to sp
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_accessrights_clsapproval");
                cmd.Parameters.AddWithValue("roleid", Convert.ToInt32(objApproval.sRoleId));
                cmd.Parameters.AddWithValue("formname", objApproval.sFormName.Trim().ToUpper());
                cmd.Parameters.AddWithValue("accesstype", Convert.ToString(objApproval.sAccessType).Contains(",") ?
                    Convert.ToString(objApproval.sAccessType.Replace(",", "','")) :
                    Convert.ToString(objApproval.sAccessType));
                dt = objcon.FetchDataTable(cmd);
                #endregion
                if (dt.Rows.Count > 0)
                {
                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }


        /// <summary>
        /// Load Approval Inbox with Pending Details
        /// </summary>
        /// <param name="objApproval">Office Code,Role Id</param>
        /// <returns></returns>
        public DataTable LoadPendingApprovalInbox(clsApproval objApproval)
        {
            DataTable dt = new DataTable();
            try
            {
                #region Old inline query
                //NpgsqlCommand = new NpgsqlCommand();
                //string strQry = string.Empty;
                ////WO_OFFICE_CODE   WO_REF_OFFCODE
                //strQry = "SELECT \"WO_DATA_ID\",\"WO_ID\", \"WO_RECORD_ID\", \"WO_BO_ID\", \"BO_NAME\",\"WO_REF_OFFCODE\", \"USER_NAME\",(SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" ";
                //strQry += " WHERE \"US_ID\"=( SELECT DISTINCT \"first_value\"(B. \"WO_CR_BY\") OVER(ORDER BY B.\"WO_ID\") FROM \"TBLWORKFLOWOBJECTS\" ";
                //strQry += " B WHERE B. \"WO_INITIAL_ID\" = A. \"WO_INITIAL_ID\" )) AS \"CREATOR\", \"CR_ON\", \"STATUS\", \"WO_APPROVE_STATUS\", ";
                //strQry += " \"RO_NAME\", \"CURRENT_STATUS\", \"WO_DESCRIPTION\", \"WOA_ID\", \"WO_WFO_ID\", \"WO_INITIAL_ID\" FROM (SELECT \"WO_DATA_ID\",\"WO_ID\" ";
                ////
                //strQry += " , \"WO_RECORD_ID\", \"WO_BO_ID\",\"WO_REF_OFFCODE\", \"BO_NAME\",(SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"WO_CR_BY\"= \"US_ID\" ) ";
                //strQry += " \"USER_NAME\", TO_CHAR(\"WO_CR_ON\",'DD-MON-YYYY HH24:MI') AS \"CR_ON\",";
                //strQry += " (CASE \"WO_APPROVE_STATUS\" WHEN '0' THEN 'PENDING' WHEN '1' THEN 'APPROVED' WHEN '2' THEN 'MODIFY AND APPROVE' WHEN '3' THEN 'REJECTED' ELSE 'OTHERS' END) \"STATUS\", \"WO_APPROVE_STATUS\" , ";
                //strQry += " (SELECT \"RO_NAME\" FROM \"TBLROLES\" WHERE \"WO_NEXT_ROLE\"=\"RO_ID\") \"RO_NAME\",'' AS \"CURRENT_STATUS\",\"WO_DESCRIPTION\",0 AS \"WOA_ID\",\"WO_WFO_ID\",\"WO_INITIAL_ID\" ";
                //strQry += " FROM \"TBLWORKFLOWOBJECTS\", \"TBLBUSINESSOBJECT\" WHERE \"WO_BO_ID\"= \"BO_ID\"  ";
                //strQry += " AND \"WO_APPROVE_STATUS\"='0'  ";

                //if (objApproval.sRoleId != Convert.ToString(ConfigurationManager.AppSettings["SupAdminRole"]))
                //{
                //    strQry += " AND  \"WO_NEXT_ROLE\" ='" + objApproval.sRoleId + "' ";

                //}

                ////NpgsqlCommand.Parameters.AddWithValue("OfficeCode", OfficeCode);
                //if (objApproval.sRoleType == "2")
                //{
                //    strQry += " AND (";
                //    string sOffCode = clsStoreOffice.GetOfficeCode(objApproval.sOfficeCode, "WO_REF_OFFCODE");
                //    strQry += sOffCode;

                //    //30-03-220 BY MADAN FOR STORE SPLIT 
                //    if (objApproval.sOfficeCode == Convert.ToString(ConfigurationManager.AppSettings["NEWSTORE"]))
                //    {

                //        strQry += " or CAST(\"WO_STATUS\"  AS TEXT) ='1'";

                //    }

                //    if (objApproval.sOfficeCode == Convert.ToString(ConfigurationManager.AppSettings["OLDSTORE"]))
                //    {

                //        strQry += " and (CAST(\"WO_STATUS\"  AS TEXT) is null or  cast(\"WO_STATUS\" as text)='')";

                //    }

                //    strQry += ")";



                //}

                //else
                //{

                //    //strQry += " AND CAST(\"WO_REF_OFFCODE\"  AS TEXT) LIKE :sOfficeCode||'%'";
                //    //NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", objApproval.sOfficeCode);

                //    strQry += " AND CAST(\"WO_REF_OFFCODE\"  AS TEXT) LIKE '" + objApproval.sOfficeCode + "%'";

                //}


                //if (objApproval.sBOId != null)
                //{

                //    strQry += " AND \"WO_BO_ID\" =:sBOId ";
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objApproval.sBOId));
                //}
                //if (objApproval.sCrby != null)
                //{

                //    strQry += " AND \"WO_CR_BY\" =:sCrby ";
                //    NpgsqlCommand.Parameters.AddWithValue("sCrby", Convert.ToInt32(objApproval.sCrby));
                //}
                //if (objApproval.sFromDate != "")
                //{

                //    DateTime dFromDate = DateTime.ParseExact(objApproval.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    strQry += " AND TO_CHAR(\"WO_CR_ON\",'YYYYMMDD')>=:dFromDate";
                //    NpgsqlCommand.Parameters.AddWithValue("dFromDate", dFromDate.ToString("yyyyMMdd"));

                //}
                //if (objApproval.sToDate != "")
                //{

                //    DateTime dToDate = DateTime.ParseExact(objApproval.sToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    strQry += " AND TO_CHAR(\"WO_CR_ON\",'YYYYMMDD')<=:dToDate";
                //    NpgsqlCommand.Parameters.AddWithValue("dToDate", dToDate.ToString("yyyyMMdd"));
                //}

                //if (objApproval.sFormName != "")
                //{

                //    strQry += " AND UPPER(\"BO_NAME\") LIKE :sFormName||'%' ";
                //    NpgsqlCommand.Parameters.AddWithValue("sFormName", objApproval.sFormName.ToUpper());
                //}
                //if (objApproval.sDescription != "")
                //{
                //    strQry += " AND UPPER(\"WO_DESCRIPTION\") LIKE '%'|| :sDescription ||'%' ";
                //    NpgsqlCommand.Parameters.AddWithValue("sDescription", objApproval.sDescription.ToUpper());
                //}

                //strQry += " UNION ALL ";
                //strQry += " SELECT \"WO_DATA_ID\",\"WO_ID\", \"WO_RECORD_ID\",(SELECT \"BFM_NEXT_BO_ID\" FROM \"TBLBO_FLOW_MASTER\" WHERE \"BFM_ID\"= \"WOA_BFM_ID\") AS ";
                //strQry += " \"WO_BO_ID\",\"WO_REF_OFFCODE\", \"BO_NAME\", (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"WOA_CRBY\" = \"US_ID\") \"USER_NAME\",";
                //strQry += "  TO_CHAR(\"WOA_CRON\",'DD-MON-YYYY HH24:MI') \"CR_ON\", 'PENDING' AS \"STATUS\",0 AS \"WO_APPROVE_STATUS\", ";
                //strQry += " (SELECT \"RO_NAME\" FROM \"TBLROLES\" WHERE \"WOA_ROLE_ID\" = \"RO_ID\") \"RO_NAME\",'' AS \"CURRENT_STATUS\", \"WOA_DESCRIPTION\" ";
                //strQry += " AS WO_DESCRIPTION, \"WOA_ID\" ,'0' AS WO_WFO_ID, \"WO_INITIAL_ID\" FROM \"TBLWO_OBJECT_AUTO\", \"TBLBUSINESSOBJECT\", \"TBLWORKFLOWOBJECTS\" WHERE ";
                //strQry += " (SELECT \"BFM_NEXT_BO_ID\" FROM \"TBLBO_FLOW_MASTER\" WHERE \"BFM_ID\" = \"WOA_BFM_ID\") = \"BO_ID\" AND ";
                //strQry += " \"WOA_INITIAL_ACTION_ID\" IS NULL AND \"WOA_PREV_APPROVE_ID\" = \"WO_ID\" AND ";

                //if (objApproval.sRoleId != Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                //{
                //    //strQry += "  \"WOA_ROLE_ID\" =:sRoleId1 and  ";
                //    //NpgsqlCommand.Parameters.AddWithValue("sRoleId1", Convert.ToInt32(objApproval.sRoleId));

                //    strQry += "  \"WOA_ROLE_ID\" ='" + objApproval.sRoleId + "' and  ";
                //    // NpgsqlCommand.Parameters.AddWithValue("sRoleId1", Convert.ToInt32(objApproval.sRoleId));
                //}

                //NpgsqlCommand.Parameters.AddWithValue("sRoleId1", Convert.ToInt32(objApproval.sRoleId));

                //if (objApproval.sRoleType == "2")
                //{
                //    strQry += "(";
                //    string sOffCode = clsStoreOffice.GetOfficeCode(objApproval.sOfficeCode, "WOA_REF_OFFCODE");
                //    strQry += sOffCode;

                //    //30-03-2020 by madan for store split ups
                //    if (objApproval.sOfficeCode == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["NEWSTORE"]))
                //    {

                //        strQry += " or CAST(\"WOA_STATUS\"  AS TEXT) ='1'";

                //    }

                //    if (objApproval.sOfficeCode == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["OLDSTORE"]))
                //    {

                //        strQry += " and (CAST(\"WOA_STATUS\"  AS TEXT) is null or  cast(\"WOA_STATUS\" as text)='')";

                //    }


                //    strQry += ")";

                //}

                ////if (objApproval.sRoleType == "1")
                //else
                //{
                //    //strQry += " CAST(\"WOA_REF_OFFCODE\"  AS TEXT) LIKE :sOfficeCode1||'%'";
                //    //NpgsqlCommand.Parameters.AddWithValue("sOfficeCode1", objApproval.sOfficeCode);

                //    strQry += " CAST(\"WOA_REF_OFFCODE\"  AS TEXT) LIKE '" + objApproval.sOfficeCode + "%'";
                //}


                ////strQry += " WOA_INITIAL_ACTION_ID IS NULL AND WOA_FLAG IS NULL AND WOA_PREV_APPROVE_ID=WO_ID AND WOA_ROLE_ID='" + objApproval.sRoleId + "' AND WOA_REF_OFFCODE LIKE '" + objApproval.sOfficeCode + "%'";


                //if (objApproval.sBOId != null)
                //{
                //    strQry += " AND (SELECT \"BFM_NEXT_BO_ID\" FROM \"TBLBO_FLOW_MASTER\" WHERE \"BFM_ID\"=\"WOA_BFM_ID\")=:sBOId1 ";
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId1", Convert.ToInt32(objApproval.sBOId));
                //}
                //if (objApproval.sCrby != null)
                //{
                //    strQry += " AND \"WO_CR_BY\" =:sCrby1";
                //    NpgsqlCommand.Parameters.AddWithValue("sCrby1", Convert.ToInt32(objApproval.sCrby));
                //}
                //if (objApproval.sFromDate != "")
                //{
                //    DateTime dFromDate = DateTime.ParseExact(objApproval.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    strQry += " AND TO_CHAR(\"WO_CR_ON\",'YYYYMMDD')>=:dFromDate1";
                //    NpgsqlCommand.Parameters.AddWithValue("dFromDate1", dFromDate.ToString("yyyyMMdd"));

                //}
                //if (objApproval.sToDate != "")
                //{
                //    DateTime dToDate = DateTime.ParseExact(objApproval.sToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    strQry += " AND TO_CHAR(\"WO_CR_ON\",'YYYYMMDD')<=:dToDate1";
                //    NpgsqlCommand.Parameters.AddWithValue("dToDate1", dToDate.ToString("yyyyMMdd"));
                //}

                //if (objApproval.sFormName != "")
                //{
                //    strQry += " AND UPPER(\"BO_NAME\") LIKE :sFormName2||'%' ";
                //    NpgsqlCommand.Parameters.AddWithValue("sFormName2", objApproval.sFormName.ToUpper());
                //}
                //if (objApproval.sDescription != "")
                //{
                //    strQry += " AND UPPER(\"WOA_DESCRIPTION\") LIKE '%'|| :sDescription2 ||'%' ";
                //    NpgsqlCommand.Parameters.AddWithValue("sDescription2", objApproval.sDescription.ToUpper());
                //}

                //strQry += " ORDER BY \"WO_ID\" DESC)A LIMIT 300";

                //return objcon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                #region
                string WoRefoffcode = clsStoreOffice.GetOfficeCode(objApproval.sOfficeCode, "WO_REF_OFFCODE");
                string WOA_refcode = clsStoreOffice.GetOfficeCode(objApproval.sOfficeCode, "WOA_REF_OFFCODE");
                DateTime ExactFromDate = new DateTime();
                DateTime ExactToDate = new DateTime();

                if (objApproval.sFromDate != "")
                {
                    ExactFromDate = DateTime.ParseExact(objApproval.sFromDate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture);

                }
                if (objApproval.sToDate != "")
                {
                    ExactToDate = DateTime.ParseExact(objApproval.sToDate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture);
                }
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_loadpendingapprovalinbox_clsapproval");
                cmd.Parameters.AddWithValue("roleid", objApproval.sRoleId);
                cmd.Parameters.AddWithValue("config_superadmin_role", Convert.ToString(ConfigurationManager.AppSettings["SupAdminRole"]));
                cmd.Parameters.AddWithValue("role_type", objApproval.sRoleType);
                cmd.Parameters.AddWithValue("soffcode", WoRefoffcode);
                cmd.Parameters.AddWithValue("office_code", objApproval.sOfficeCode);
                cmd.Parameters.AddWithValue("config_newstore", Convert.ToString(ConfigurationManager.AppSettings["NEWSTORE"]));
                cmd.Parameters.AddWithValue("config_oldstore", Convert.ToString(ConfigurationManager.AppSettings["OLDSTORE"]));
                cmd.Parameters.AddWithValue("bo_id", objApproval.sBOId == null ? "" : Convert.ToString(objApproval.sBOId));
                cmd.Parameters.AddWithValue("cr_by", objApproval.sCrby == null ? "" : Convert.ToString(objApproval.sCrby));
                cmd.Parameters.AddWithValue("from_date", Convert.ToString(objApproval.sFromDate));
                cmd.Parameters.AddWithValue("dfromdate", ExactFromDate.ToString("yyyyMMdd"));
                cmd.Parameters.AddWithValue("to_date", Convert.ToString(objApproval.sToDate));
                cmd.Parameters.AddWithValue("dtodate", ExactToDate.ToString("yyyyMMdd"));
                cmd.Parameters.AddWithValue("form_name", Convert.ToString(objApproval.sFormName));
                cmd.Parameters.AddWithValue("description", Convert.ToString(objApproval.sDescription));
                cmd.Parameters.AddWithValue("swoaoffcode", WOA_refcode);
                dt = objcon.FetchDataTable(cmd);

                return dt;
                #endregion
                // AND WO_OFFICE_CODE LIKE '"+ objApproval.sOfficeCode +"%'
                //AND WOA_OFFICE_CODE LIKE '" + objApproval.sOfficeCode + "%'
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }


        /// <summary>
        /// Load Approval Inbox with Already Approved Details
        /// </summary>
        /// <param name="objApproval">Office Code,Role Id</param>
        /// <returns></returns>
        public DataTable LoadAlreadyApprovedInbox(clsApproval objApproval)
        {
            DataTable dt = new DataTable();
            try
            {
                #region Old inline query  
                //NpgsqlCommand = new NpgsqlCommand();
                //string strQry = string.Empty;
                //strQry = " SELECT \"WO_DATA_ID\",\"WO_ID\",\"WO_RECORD_ID\",\"WO_BO_ID\",\"BO_NAME\",\"WO_REF_OFFCODE\",\"USER_NAME\",(SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE ";
                //strQry += " \"US_ID\"=(SELECT DISTINCT \"first_value\"(B. \"WO_CR_BY\") OVER(ORDER BY B.\"WO_ID\" DESC) FROM \"TBLWORKFLOWOBJECTS\" B ";
                //strQry += " WHERE B.\"WO_INITIAL_ID\" = A.\"WO_INITIAL_ID\")) AS CREATOR,\"CR_ON\",\"STATUS\",\"WO_APPROVE_STATUS\",\"RO_NAME\", ";
                //strQry += " \"NEXT_ROLE\",\"CURRENT_STATUS\",\"WO_DESCRIPTION\",\"WOA_ID\", \"WO_WFO_ID\",\"WO_INITIAL_ID\" FROM ";
                //strQry += " (SELECT \"WO_DATA_ID\",\"WO_ID\",\"WO_RECORD_ID\",\"WO_BO_ID\",\"BO_NAME\",\"WO_REF_OFFCODE\",(SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE ";
                //strQry += " \"WO_CR_BY\"=\"US_ID\") \"USER_NAME\",TO_CHAR(\"WO_CR_ON\",'DD-MON-YYYY HH24:MI') \"CR_ON\",";
                //strQry += " (CASE \"WO_APPROVE_STATUS\" WHEN '0' THEN 'PENDING' WHEN '1' THEN 'APPROVED' WHEN '2' THEN 'MODIFY AND APPROVE' END) \"STATUS\",\"WO_APPROVE_STATUS\",";
                //strQry += " (SELECT \"RO_NAME\" FROM \"TBLROLES\" WHERE \"WO_NEXT_ROLE\"=\"RO_ID\") \"RO_NAME\",";
                //strQry += " (SELECT \"WO_NEXT_ROLE\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\"=(SELECT MAX(\"WO_ID\") FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_RECORD_ID\"= A.\"WO_RECORD_ID\" )) \"NEXT_ROLE\",";
                //strQry += " CASE (SELECT \"WO_NEXT_ROLE\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\"=(SELECT MAX(\"WO_ID\") FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_RECORD_ID\" = A.\"WO_RECORD_ID\" ))";
                //strQry += " WHEN 0 THEN 'APPROVED' ELSE 'PENDING' END \"CURRENT_STATUS\",\"WO_DESCRIPTION\",0 AS \"WOA_ID\",";
                ////strQry += " (SELECT MAX(WO_WFO_ID) FROM TBLWORKFLOWOBJECTS WHERE WO_REF_OFFCODE LIKE '" + objApproval.sOfficeCode + "%' AND WO_RECORD_ID=A.WO_RECORD_ID) ";
                //strQry += " \"WO_WFO_ID\", \"WO_INITIAL_ID\" ";
                //strQry += " FROM \"TBLWORKFLOWOBJECTS\" A,\"TBLBUSINESSOBJECT\" B WHERE \"WO_BO_ID\"=\"BO_ID\"  AND \"WO_PREV_APPROVE_ID\" in (SELECT \"WO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_APPROVE_STATUS\" <> 3 AND \"WO_APPROVED_BY\"=:sCrby) ";
                //// strQry += "  AND \"WO_APPROVE_STATUS\" <> 0  ";
                //NpgsqlCommand.Parameters.AddWithValue("sCrby", Convert.ToInt32(objApproval.sCrby));
                //if (objApproval.sRoleType == "2")
                //{
                //    strQry += "AND (";
                //    string sOffCode = clsStoreOffice.GetOfficeCode(objApproval.sOfficeCode, "WO_REF_OFFCODE"); ////by rudra changed wo to woref on 2704
                //    strQry += sOffCode;


                //    //30-03-220 BY MADAN FOR STORE SPLIT 
                //    if (objApproval.sOfficeCode == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["NEWSTORE"]))
                //    {

                //        strQry += " or CAST(\"WO_STATUS\"  AS TEXT) ='1'";

                //    }

                //    if (objApproval.sOfficeCode == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["OLDSTORE"]))
                //    {

                //        strQry += " and (CAST(\"WO_STATUS\"  AS TEXT) is null or  cast(\"WO_STATUS\" as text)='')";

                //    }

                //    strQry += ")";



                //}

                //if (objApproval.sRoleType == "1")
                //{
                //    strQry += " AND CAST(\"WO_OFFICE_CODE\"  AS TEXT) LIKE :sOfficeCode||'%'"; // before it is WO_REF_OFFCODE with like condition
                //    NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", objApproval.sOfficeCode);
                //}

                //if (objApproval.sBOId != null)
                //{
                //    strQry += " AND \"WO_BO_ID\" =:sBOId ";
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objApproval.sBOId));
                //}
                //if (objApproval.sCrby != null)
                //{
                //    strQry += " AND \"WO_CR_BY\" =:sCrby1 ";
                //    NpgsqlCommand.Parameters.AddWithValue("sCrby1", Convert.ToInt32(objApproval.sCrby));
                //}
                //if (objApproval.sFromDate != "")
                //{
                //    DateTime dFromDate = DateTime.ParseExact(objApproval.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    strQry += " AND TO_CHAR(\"WO_CR_ON\",'YYYYMMDD')>=:dFromDate";
                //    NpgsqlCommand.Parameters.AddWithValue("dFromDate", dFromDate.ToString("yyyyMMdd"));

                //}
                //if (objApproval.sToDate != "")
                //{
                //    DateTime dToDate = DateTime.ParseExact(objApproval.sToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    strQry += " AND TO_CHAR(\"WO_CR_ON\",'YYYYMMDD')<=:dToDate";
                //    NpgsqlCommand.Parameters.AddWithValue("dToDate", dToDate.ToString("yyyyMMdd"));
                //}
                //if (objApproval.sFormName != "")
                //{
                //    strQry += " AND UPPER(\"BO_NAME\") LIKE :sFormName1||'%' ";
                //    NpgsqlCommand.Parameters.AddWithValue("sFormName1", objApproval.sFormName.ToUpper());
                //}
                //if (objApproval.sDescription != "")
                //{
                //    strQry += " AND UPPER(\"WO_DESCRIPTION\") LIKE '%'|| :sDescription1 ||'%' ";
                //    NpgsqlCommand.Parameters.AddWithValue("sDescription1", objApproval.sDescription.ToUpper());
                //}

                //strQry += " UNION ALL  ";
                //strQry += " SELECT \"WO_DATA_ID\",\"WO_ID\",\"WO_RECORD_ID\",(SELECT \"BFM_NEXT_BO_ID\" FROM \"TBLBO_FLOW_MASTER\" WHERE \"BFM_ID\"=\"WOA_BFM_ID\") AS \"WO_BO_ID\",";
                //strQry += " \"BO_NAME\",\"WO_REF_OFFCODE\", (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE  \"US_ID\"=(SELECT DISTINCT \"first_value\"(B. \"WO_CR_BY\") ";
                //strQry += " OVER(ORDER BY B.\"WO_ID\" DESC) FROM \"TBLWORKFLOWOBJECTS\" B  WHERE B.\"WO_INITIAL_ID\" = A.\"WO_INITIAL_ID\")) AS \"USER_NAME\" ,TO_CHAR(\"WOA_CRON\",'DD-MON-YYYY HH24:MI') \"CR_ON\" ,";
                //strQry += " 'INITIATED' \"STATUS\",\"WO_APPROVE_STATUS\",";
                //strQry += "  (SELECT \"RO_NAME\" FROM \"TBLROLES\" WHERE \"WOA_ROLE_ID\"=\"RO_ID\") \"RO_NAME\", ";
                //strQry += " (SELECT \"WO_NEXT_ROLE\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\"=(SELECT MAX(\"WO_ID\") FROM \"TBLWORKFLOWOBJECTS\" WHERE ";
                //strQry += " \"WO_RECORD_ID\"= A.\"WO_RECORD_ID\" )) \"NEXT_ROLE\" ,CASE (SELECT \"WO_NEXT_ROLE\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\" =(SELECT MAX(\"WO_ID\") ";
                //strQry += " FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_RECORD_ID\" = A.\"WO_RECORD_ID\" )) WHEN 0 THEN 'APPROVED' ELSE 'PENDING' END \"CURRENT_STATUS\",\"WOA_DESCRIPTION\" ";
                //strQry += " AS \"WO_DESCRIPTION\",'0' AS \"WOA_ID\",\"WO_WFO_ID\",\"WO_INITIAL_ID\"  FROM \"TBLWO_OBJECT_AUTO\",\"TBLBUSINESSOBJECT\",\"TBLWORKFLOWOBJECTS\" A ";
                //strQry += " WHERE  (SELECT \"BFM_NEXT_BO_ID\" FROM \"TBLBO_FLOW_MASTER\" WHERE \"BFM_ID\"=\"WOA_BFM_ID\") = \"BO_ID\" AND  \"WOA_INITIAL_ACTION_ID\" IS NOT NULL ";
                //strQry += " AND \"WOA_INITIAL_ACTION_ID\" = \"WO_INITIAL_ID\" ";
                ////AND \"WOA_ROLE_ID\" ='" + objApproval.sRoleId + "'  ";


                //if (objApproval.sRoleType == "2")
                //{
                //    strQry += "AND (";
                //    string sOffCode = clsStoreOffice.GetOfficeCode(objApproval.sOfficeCode, "WOA_OFFICE_CODE");
                //    strQry += sOffCode;

                //    //30-03-2020 by madan for store split ups
                //    if (objApproval.sOfficeCode == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["NEWSTORE"]))
                //    {

                //        strQry += " or CAST(\"WOA_STATUS\"  AS TEXT) ='1'";

                //    }

                //    if (objApproval.sOfficeCode == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["OLDSTORE"]))
                //    {

                //        strQry += " and (CAST(\"WOA_STATUS\"  AS TEXT) is null or  cast(\"WO_STATUS\" as text)='')";

                //    }

                //    strQry += ")";
                //}

                //if (objApproval.sRoleType == "1")
                //{
                //    strQry += " AND CAST(\"WOA_OFFICE_CODE\"  AS TEXT) LIKE :sOfficeCode2||'%'";
                //    if (objApproval.sRoleId == "40" || objApproval.sRoleId == "41" || objApproval.sRoleId == "38" || objApproval.sRoleId == "31" || objApproval.sRoleId == "35" || objApproval.sRoleId == "36")
                //    {
                //        NpgsqlCommand.Parameters.AddWithValue("sOfficeCode2", objApproval.sOfficeCode);
                //    }
                //    else
                //    {
                //        NpgsqlCommand.Parameters.AddWithValue("sOfficeCode2", objApproval.sOfficeCode.Substring(0, 3));
                //    }

                //}




                //if (objApproval.sBOId != null)
                //{
                //    strQry += " AND (SELECT \"BFM_NEXT_BO_ID\" FROM \"TBLBO_FLOW_MASTER\" WHERE \"BFM_ID\" = \"WOA_BFM_ID\" )=:sBOId3 ";
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId3", Convert.ToInt32(objApproval.sBOId));
                //}
                //if (objApproval.sCrby != null)
                //{
                //    strQry += " AND \"WO_CR_BY\" =:sCrby3 ";
                //    NpgsqlCommand.Parameters.AddWithValue("sCrby3", Convert.ToInt32(objApproval.sCrby));
                //}
                //if (objApproval.sFromDate != "")
                //{
                //    DateTime dFromDate = DateTime.ParseExact(objApproval.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    strQry += " AND TO_CHAR(\"WOA_CRON\",'YYYYMMDD')>=:dFromDate1";
                //    NpgsqlCommand.Parameters.AddWithValue("dFromDate1", dFromDate.ToString("yyyyMMdd"));

                //}
                //if (objApproval.sToDate != "")
                //{
                //    DateTime dToDate = DateTime.ParseExact(objApproval.sToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    strQry += " AND TO_CHAR(\"WOA_CRON\",'YYYYMMDD')<=:dToDate1";
                //    NpgsqlCommand.Parameters.AddWithValue("dToDate1", dToDate.ToString("yyyyMMdd"));
                //}

                //if (objApproval.sFormName != "")
                //{
                //    strQry += " AND UPPER(\"BO_NAME\") LIKE :sFormName4||'%' ";
                //    NpgsqlCommand.Parameters.AddWithValue("sFormName4", objApproval.sFormName.ToUpper());
                //}
                //if (objApproval.sDescription != "")
                //{
                //    strQry += " AND UPPER(\"WOA_DESCRIPTION\") LIKE '%'||:sDescription4||'%' ";
                //    NpgsqlCommand.Parameters.AddWithValue("sDescription4", objApproval.sDescription.ToUpper());
                //}

                ////strQry += " ORDER BY \"WO_ID\" DESC)A LIMIT 500"; till now is the actual query, if the below code not worked properly then need to delete and uncomment this line 

                //strQry += " UNION ALL SELECT \"WO_DATA_ID\",\"WO_ID\",\"WO_RECORD_ID\",\"WO_BO_ID\",\"BO_NAME\",\"WO_REF_OFFCODE\",(SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE ";
                //strQry += " \"US_ID\" =\"WO_CR_BY\")AS \"USER_NAME\",TO_CHAR(\"WO_CR_ON\",'DD-MON-YYYY HH24:MI') \"CR_ON\" ,'INITIATED' \"STATUS\",";
                //strQry += " \"WO_APPROVE_STATUS\",'' AS \"RO_NAME\",'0' AS \"NEXT_ROLE\",'APPROVED' AS \"CURRENT_STATUS\",\"WO_DESCRIPTION\",";
                //strQry += " '0' AS \"WOA_ID\",\"WO_WFO_ID\",\"WO_INITIAL_ID\"  FROM \"TBLWORKFLOWOBJECTS\",\"TBLBUSINESSOBJECT\" WHERE ";
                //strQry += " \"WO_BO_ID\" =\"BO_ID\" AND \"WO_PREV_APPROVE_ID\" IN (SELECT \"WO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE ";
                //strQry += " \"WO_NEXT_ROLE\" =:sRoleId3 and \"WO_PREV_APPROVE_ID\" IN (SELECT \"WO_ID\" FROM ";
                //strQry += " \"TBLWORKFLOWOBJECTS\" WHERE \"WO_APPROVE_STATUS\"='3')) ";
                //NpgsqlCommand.Parameters.AddWithValue("sRoleId3", Convert.ToInt32(objApproval.sRoleId));
                //if (objApproval.sFormName != "")
                //{
                //    strQry += " AND UPPER(\"BO_NAME\") LIKE :sFormName5||'%' ";
                //    NpgsqlCommand.Parameters.AddWithValue("sFormName5", objApproval.sFormName.ToUpper());
                //}
                //if (objApproval.sDescription != "")
                //{
                //    strQry += " AND UPPER(\"WO_DESCRIPTION\") LIKE '%'||:sDescription5||'%' ";
                //    NpgsqlCommand.Parameters.AddWithValue("sDescription5", objApproval.sDescription.ToUpper());
                //}
                //if (objApproval.sCrby != null)
                //{
                //    strQry += " AND \"WO_CR_BY\" =:sCrby4 ";
                //    NpgsqlCommand.Parameters.AddWithValue("sCrby4", Convert.ToInt32(objApproval.sCrby));
                //}
                //if (objApproval.sFromDate != "")
                //{
                //    DateTime dFromDate = DateTime.ParseExact(objApproval.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    strQry += " AND TO_CHAR(\"WO_CR_ON\",'YYYYMMDD')>=:dFromDate4";
                //    NpgsqlCommand.Parameters.AddWithValue("dFromDate4", dFromDate.ToString("yyyyMMdd"));

                //}
                //if (objApproval.sToDate != "")
                //{
                //    DateTime dToDate = DateTime.ParseExact(objApproval.sToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    strQry += " AND TO_CHAR(\"WO_CR_ON\",'YYYYMMDD')<=:dToDate4";
                //    NpgsqlCommand.Parameters.AddWithValue("dToDate4", dToDate.ToString("yyyyMMdd"));
                //}
                //strQry += " )A  ORDER BY \"WO_ID\" DESC LIMIT 300";
                //return objcon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion



                #region Old inline query  made changes by siddesha
                //NpgsqlCommand = new NpgsqlCommand();
                //string strQry = string.Empty;
                //strQry = " SELECT \"WO_DATA_ID\",\"WO_ID\",\"WO_RECORD_ID\",\"WO_BO_ID\",\"BO_NAME\",\"WO_REF_OFFCODE\",\"USER_NAME\",(SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE ";
                //strQry += " \"US_ID\"=(SELECT DISTINCT \"first_value\"(B. \"WO_CR_BY\") OVER(ORDER BY B.\"WO_ID\" DESC) FROM \"TBLWORKFLOWOBJECTS\" B ";
                //strQry += " WHERE B.\"WO_INITIAL_ID\" = A.\"WO_INITIAL_ID\")) AS CREATOR,\"CR_ON\",\"STATUS\",\"WO_APPROVE_STATUS\",\"RO_NAME\", ";
                //strQry += " \"NEXT_ROLE\",\"CURRENT_STATUS\",\"WO_DESCRIPTION\",\"WOA_ID\", \"WO_WFO_ID\",\"WO_INITIAL_ID\" FROM ";
                //strQry += " (SELECT \"WO_DATA_ID\",\"WO_ID\",\"WO_RECORD_ID\",\"WO_BO_ID\",\"BO_NAME\",\"WO_REF_OFFCODE\",(SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE ";
                //strQry += " \"WO_CR_BY\"=\"US_ID\") \"USER_NAME\",TO_CHAR(\"WO_CR_ON\",'DD-MON-YYYY HH24:MI') \"CR_ON\",";
                //strQry += "  case ( select \"WO_APPROVE_STATUS\" from \"TBLWORKFLOWOBJECTS\"  where \"WO_ID\"=(SELECT MAX(\"WO_ID\") FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_RECORD_ID\" = A.\"WO_RECORD_ID\"  and  \"WO_APPROVED_BY\"=:sCrby  ))   WHEN '0' THEN 'PENDING' WHEN '1' THEN 'APPROVED' WHEN '2' THEN 'MODIFY AND APPROVE' END \"STATUS\" ,\"WO_APPROVE_STATUS\",";
                //strQry += " (SELECT \"RO_NAME\" FROM \"TBLROLES\" WHERE \"WO_NEXT_ROLE\"=\"RO_ID\") \"RO_NAME\",";
                //strQry += " (SELECT \"WO_NEXT_ROLE\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\"=(SELECT MAX(\"WO_ID\") FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_RECORD_ID\"= A.\"WO_RECORD_ID\" )) \"NEXT_ROLE\",";
                //strQry += " CASE (SELECT \"WO_NEXT_ROLE\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\"=(SELECT MAX(\"WO_ID\") FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_RECORD_ID\" = A.\"WO_RECORD_ID\" ))";
                //strQry += " WHEN 0 THEN 'APPROVED' ELSE 'PENDING' END \"CURRENT_STATUS\",\"WO_DESCRIPTION\",0 AS \"WOA_ID\",";
                ////strQry += " (SELECT MAX(WO_WFO_ID) FROM TBLWORKFLOWOBJECTS WHERE WO_REF_OFFCODE LIKE '" + objApproval.sOfficeCode + "%' AND WO_RECORD_ID=A.WO_RECORD_ID) ";
                //strQry += " \"WO_WFO_ID\", \"WO_INITIAL_ID\" ";
                //strQry += " FROM \"TBLWORKFLOWOBJECTS\" A,\"TBLBUSINESSOBJECT\" B WHERE \"WO_BO_ID\"=\"BO_ID\"  AND \"WO_PREV_APPROVE_ID\" in (SELECT \"WO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_APPROVE_STATUS\" <> 3 AND \"WO_APPROVED_BY\"=:sCrby) ";
                //// strQry += "  AND \"WO_APPROVE_STATUS\" <> 0  ";
                //NpgsqlCommand.Parameters.AddWithValue("sCrby", Convert.ToInt32(objApproval.sCrby));
                //if (objApproval.sRoleType == "2")
                //{
                //    strQry += "AND (";
                //    string sOffCode = clsStoreOffice.GetOfficeCode(objApproval.sOfficeCode, "WO_REF_OFFCODE"); ////by rudra changed wo to woref on 2704
                //    strQry += sOffCode;


                //    //30-03-220 BY MADAN FOR STORE SPLIT 
                //    if (objApproval.sOfficeCode == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["NEWSTORE"]))
                //    {

                //        strQry += " or CAST(\"WO_STATUS\"  AS TEXT) ='1'";

                //    }

                //    if (objApproval.sOfficeCode == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["OLDSTORE"]))
                //    {

                //        strQry += " and (CAST(\"WO_STATUS\"  AS TEXT) is null or  cast(\"WO_STATUS\" as text)='')";

                //    }

                //    strQry += ")";



                //}

                //if (objApproval.sRoleType == "1")
                //{
                //    strQry += " AND CAST(\"WO_OFFICE_CODE\"  AS TEXT) LIKE :sOfficeCode||'%'"; // before it is WO_REF_OFFCODE with like condition
                //    NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", objApproval.sOfficeCode);
                //}

                //if (objApproval.sBOId != null)
                //{
                //    strQry += " AND \"WO_BO_ID\" =:sBOId ";
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objApproval.sBOId));
                //}
                //if (objApproval.sCrby != null)
                //{
                //    strQry += " AND \"WO_CR_BY\" =:sCrby1 ";
                //    NpgsqlCommand.Parameters.AddWithValue("sCrby1", Convert.ToInt32(objApproval.sCrby));
                //}
                //if (objApproval.sFromDate != "")
                //{
                //    DateTime dFromDate = DateTime.ParseExact(objApproval.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    strQry += " AND TO_CHAR(\"WO_CR_ON\",'YYYYMMDD')>=:dFromDate";
                //    NpgsqlCommand.Parameters.AddWithValue("dFromDate", dFromDate.ToString("yyyyMMdd"));

                //}
                //if (objApproval.sToDate != "")
                //{
                //    DateTime dToDate = DateTime.ParseExact(objApproval.sToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    strQry += " AND TO_CHAR(\"WO_CR_ON\",'YYYYMMDD')<=:dToDate";
                //    NpgsqlCommand.Parameters.AddWithValue("dToDate", dToDate.ToString("yyyyMMdd"));
                //}
                //if (objApproval.sFormName != "")
                //{
                //    strQry += " AND UPPER(\"BO_NAME\") LIKE :sFormName1||'%' ";
                //    NpgsqlCommand.Parameters.AddWithValue("sFormName1", objApproval.sFormName.ToUpper());
                //}
                //if (objApproval.sDescription != "")
                //{
                //    strQry += " AND UPPER(\"WO_DESCRIPTION\") LIKE '%'|| :sDescription1 ||'%' ";
                //    NpgsqlCommand.Parameters.AddWithValue("sDescription1", objApproval.sDescription.ToUpper());
                //}

                //strQry += " UNION ALL  ";
                //strQry += " SELECT \"WO_DATA_ID\",\"WO_ID\",\"WO_RECORD_ID\",(SELECT \"BFM_NEXT_BO_ID\" FROM \"TBLBO_FLOW_MASTER\" WHERE \"BFM_ID\"=\"WOA_BFM_ID\") AS \"WO_BO_ID\",";
                //strQry += " \"BO_NAME\",\"WO_REF_OFFCODE\", (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE  \"US_ID\"=(SELECT DISTINCT \"first_value\"(B. \"WO_CR_BY\") ";
                //strQry += " OVER(ORDER BY B.\"WO_ID\" DESC) FROM \"TBLWORKFLOWOBJECTS\" B  WHERE B.\"WO_INITIAL_ID\" = A.\"WO_INITIAL_ID\")) AS \"USER_NAME\" ,TO_CHAR(\"WOA_CRON\",'DD-MON-YYYY HH24:MI') \"CR_ON\" ,";
                //strQry += " 'INITIATED' \"STATUS\",\"WO_APPROVE_STATUS\",";
                //strQry += "  (SELECT \"RO_NAME\" FROM \"TBLROLES\" WHERE \"WOA_ROLE_ID\"=\"RO_ID\") \"RO_NAME\", ";
                //strQry += " (SELECT \"WO_NEXT_ROLE\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\"=(SELECT MAX(\"WO_ID\") FROM \"TBLWORKFLOWOBJECTS\" WHERE ";
                //strQry += " \"WO_RECORD_ID\"= A.\"WO_RECORD_ID\" )) \"NEXT_ROLE\" ,CASE (SELECT \"WO_NEXT_ROLE\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\" =(SELECT MAX(\"WO_ID\") ";
                //strQry += " FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_RECORD_ID\" = A.\"WO_RECORD_ID\" )) WHEN 0 THEN 'APPROVED' ELSE 'PENDING' END \"CURRENT_STATUS\",\"WOA_DESCRIPTION\" ";
                //strQry += " AS \"WO_DESCRIPTION\",'0' AS \"WOA_ID\",\"WO_WFO_ID\",\"WO_INITIAL_ID\"  FROM \"TBLWO_OBJECT_AUTO\",\"TBLBUSINESSOBJECT\",\"TBLWORKFLOWOBJECTS\" A ";
                //strQry += " WHERE  (SELECT \"BFM_NEXT_BO_ID\" FROM \"TBLBO_FLOW_MASTER\" WHERE \"BFM_ID\"=\"WOA_BFM_ID\") = \"BO_ID\" AND  \"WOA_INITIAL_ACTION_ID\" IS NOT NULL ";
                //strQry += " AND \"WOA_INITIAL_ACTION_ID\" = \"WO_INITIAL_ID\" ";
                ////AND \"WOA_ROLE_ID\" ='" + objApproval.sRoleId + "'  ";


                //if (objApproval.sRoleType == "2")
                //{
                //    strQry += "AND (";
                //    string sOffCode = clsStoreOffice.GetOfficeCode(objApproval.sOfficeCode, "WOA_OFFICE_CODE");
                //    strQry += sOffCode;

                //    //30-03-2020 by madan for store split ups
                //    if (objApproval.sOfficeCode == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["NEWSTORE"]))
                //    {

                //        strQry += " or CAST(\"WOA_STATUS\"  AS TEXT) ='1'";

                //    }

                //    if (objApproval.sOfficeCode == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["OLDSTORE"]))
                //    {

                //        strQry += " and (CAST(\"WOA_STATUS\"  AS TEXT) is null or  cast(\"WO_STATUS\" as text)='')";

                //    }

                //    strQry += ")";
                //}

                //if (objApproval.sRoleType == "1")
                //{
                //    strQry += " AND CAST(\"WOA_OFFICE_CODE\"  AS TEXT) LIKE :sOfficeCode2||'%'";
                //    if (objApproval.sRoleId == "40" || objApproval.sRoleId == "41" || objApproval.sRoleId == "38" || objApproval.sRoleId == "31" || objApproval.sRoleId == "35" || objApproval.sRoleId == "36")
                //    {
                //        NpgsqlCommand.Parameters.AddWithValue("sOfficeCode2", objApproval.sOfficeCode);
                //    }
                //    else
                //    {
                //        NpgsqlCommand.Parameters.AddWithValue("sOfficeCode2", objApproval.sOfficeCode.Substring(0, 3));
                //    }

                //}




                //if (objApproval.sBOId != null)
                //{
                //    strQry += " AND (SELECT \"BFM_NEXT_BO_ID\" FROM \"TBLBO_FLOW_MASTER\" WHERE \"BFM_ID\" = \"WOA_BFM_ID\" )=:sBOId3 ";
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId3", Convert.ToInt32(objApproval.sBOId));
                //}
                //if (objApproval.sCrby != null)
                //{
                //    strQry += " AND \"WO_CR_BY\" =:sCrby3 ";
                //    NpgsqlCommand.Parameters.AddWithValue("sCrby3", Convert.ToInt32(objApproval.sCrby));
                //}
                //if (objApproval.sFromDate != "")
                //{
                //    DateTime dFromDate = DateTime.ParseExact(objApproval.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    strQry += " AND TO_CHAR(\"WOA_CRON\",'YYYYMMDD')>=:dFromDate1";
                //    NpgsqlCommand.Parameters.AddWithValue("dFromDate1", dFromDate.ToString("yyyyMMdd"));

                //}
                //if (objApproval.sToDate != "")
                //{
                //    DateTime dToDate = DateTime.ParseExact(objApproval.sToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    strQry += " AND TO_CHAR(\"WOA_CRON\",'YYYYMMDD')<=:dToDate1";
                //    NpgsqlCommand.Parameters.AddWithValue("dToDate1", dToDate.ToString("yyyyMMdd"));
                //}

                //if (objApproval.sFormName != "")
                //{
                //    strQry += " AND UPPER(\"BO_NAME\") LIKE :sFormName4||'%' ";
                //    NpgsqlCommand.Parameters.AddWithValue("sFormName4", objApproval.sFormName.ToUpper());
                //}
                //if (objApproval.sDescription != "")
                //{
                //    strQry += " AND UPPER(\"WOA_DESCRIPTION\") LIKE '%'||:sDescription4||'%' ";
                //    NpgsqlCommand.Parameters.AddWithValue("sDescription4", objApproval.sDescription.ToUpper());
                //}

                ////strQry += " ORDER BY \"WO_ID\" DESC)A LIMIT 500"; till now is the actual query, if the below code not worked properly then need to delete and uncomment this line 

                //strQry += " UNION ALL SELECT \"WO_DATA_ID\",\"WO_ID\",\"WO_RECORD_ID\",\"WO_BO_ID\",\"BO_NAME\",\"WO_REF_OFFCODE\",(SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE ";
                //strQry += " \"US_ID\" =\"WO_CR_BY\")AS \"USER_NAME\",TO_CHAR(\"WO_CR_ON\",'DD-MON-YYYY HH24:MI') \"CR_ON\" ,'INITIATED' \"STATUS\",";
                //strQry += " \"WO_APPROVE_STATUS\",'' AS \"RO_NAME\",'0' AS \"NEXT_ROLE\",'APPROVED' AS \"CURRENT_STATUS\",\"WO_DESCRIPTION\",";
                //strQry += " '0' AS \"WOA_ID\",\"WO_WFO_ID\",\"WO_INITIAL_ID\"  FROM \"TBLWORKFLOWOBJECTS\",\"TBLBUSINESSOBJECT\" WHERE ";
                //strQry += " \"WO_BO_ID\" =\"BO_ID\" AND \"WO_PREV_APPROVE_ID\" IN (SELECT \"WO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE ";
                //strQry += " \"WO_NEXT_ROLE\" =:sRoleId3 and \"WO_PREV_APPROVE_ID\" IN (SELECT \"WO_ID\" FROM ";
                //strQry += " \"TBLWORKFLOWOBJECTS\" WHERE \"WO_APPROVE_STATUS\"='3')) ";
                //NpgsqlCommand.Parameters.AddWithValue("sRoleId3", Convert.ToInt32(objApproval.sRoleId));
                //if (objApproval.sFormName != "")
                //{
                //    strQry += " AND UPPER(\"BO_NAME\") LIKE :sFormName5||'%' ";
                //    NpgsqlCommand.Parameters.AddWithValue("sFormName5", objApproval.sFormName.ToUpper());
                //}
                //if (objApproval.sDescription != "")
                //{
                //    strQry += " AND UPPER(\"WO_DESCRIPTION\") LIKE '%'||:sDescription5||'%' ";
                //    NpgsqlCommand.Parameters.AddWithValue("sDescription5", objApproval.sDescription.ToUpper());
                //}
                //if (objApproval.sCrby != null)
                //{
                //    strQry += " AND \"WO_CR_BY\" =:sCrby4 ";
                //    NpgsqlCommand.Parameters.AddWithValue("sCrby4", Convert.ToInt32(objApproval.sCrby));
                //}
                //if (objApproval.sFromDate != "")
                //{
                //    DateTime dFromDate = DateTime.ParseExact(objApproval.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    strQry += " AND TO_CHAR(\"WO_CR_ON\",'YYYYMMDD')>=:dFromDate4";
                //    NpgsqlCommand.Parameters.AddWithValue("dFromDate4", dFromDate.ToString("yyyyMMdd"));

                //}
                //if (objApproval.sToDate != "")
                //{
                //    DateTime dToDate = DateTime.ParseExact(objApproval.sToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    strQry += " AND TO_CHAR(\"WO_CR_ON\",'YYYYMMDD')<=:dToDate4";
                //    NpgsqlCommand.Parameters.AddWithValue("dToDate4", dToDate.ToString("yyyyMMdd"));
                //}
                //strQry += " )A  ORDER BY \"WO_ID\" DESC LIMIT 300";
                //return objcon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                #region Converted to sp
                string WoRefoffcode = clsStoreOffice.GetOfficeCode(objApproval.sOfficeCode, "WO_REF_OFFCODE");
                string WOA_refcode = clsStoreOffice.GetOfficeCode(objApproval.sOfficeCode, "WOA_OFFICE_CODE");
                DateTime ExactFromDate = new DateTime();
                DateTime ExactToDate = new DateTime();

                if (objApproval.sFromDate != "")
                {
                    ExactFromDate = DateTime.ParseExact(objApproval.sFromDate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture);

                }
                if (objApproval.sToDate != "")
                {
                    ExactToDate = DateTime.ParseExact(objApproval.sToDate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture);
                }
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_alreadyapprovedinbox_clsapproval");
                cmd.Parameters.AddWithValue("cr_by", Convert.ToInt32(objApproval.sCrby));
                cmd.Parameters.AddWithValue("role_type", objApproval.sRoleType);
                cmd.Parameters.AddWithValue("soffcode", WoRefoffcode);
                cmd.Parameters.AddWithValue("office_code", objApproval.sOfficeCode);
                cmd.Parameters.AddWithValue("new_store", Convert.ToString(ConfigurationManager.AppSettings["NEWSTORE"]));
                cmd.Parameters.AddWithValue("old_store", Convert.ToString(ConfigurationManager.AppSettings["OLDSTORE"]));
                cmd.Parameters.AddWithValue("bo_id", objApproval.sBOId == null ? "" : Convert.ToString(objApproval.sBOId));
                cmd.Parameters.AddWithValue("from_date", Convert.ToString(objApproval.sFromDate));
                cmd.Parameters.AddWithValue("to_date", Convert.ToString(objApproval.sToDate));
                cmd.Parameters.AddWithValue("form_name", Convert.ToString(objApproval.sFormName.ToUpper()));
                cmd.Parameters.AddWithValue("description", Convert.ToString(objApproval.sDescription.ToUpper()));
                cmd.Parameters.AddWithValue("dfromdate", ExactFromDate.ToString("yyyyMMdd"));
                cmd.Parameters.AddWithValue("dtodate", ExactToDate.ToString("yyyyMMdd"));
                cmd.Parameters.AddWithValue("woa_offcode", WOA_refcode);
                cmd.Parameters.AddWithValue("role_id", objApproval.sRoleId);
                dt = objcon.FetchDataTable(cmd);

                return dt;
                #endregion
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        /// <summary>
        /// Load Approval Inbox with Already Approved Details
        /// </summary>
        /// <param name="objApproval">Office Code,Role Id</param>
        /// <returns></returns>
        public DataTable LoadRejectedApprovedInbox(clsApproval objApproval)
        {
            DataTable dt = new DataTable();
            try
            {
                #region Inline Query
                //NpgsqlCommand = new NpgsqlCommand();
                //string strQry = string.Empty;
                //strQry = " SELECT * FROM (SELECT \"WO_DATA_ID\",\"WO_ID\",\"WO_RECORD_ID\",\"WO_BO_ID\",\"BO_NAME\",(SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"WO_CR_BY\"=\"US_ID\") USER_NAME,TO_CHAR(\"WO_CR_ON\",'DD-MON-YYYY HH24:MI') CR_ON,(SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_ID\" =(SELECT DISTINCT \"first_value\"(B. \"WO_CR_BY\") OVER(ORDER BY B.\"WO_ID\" DESC) FROM \"TBLWORKFLOWOBJECTS\" B WHERE B.\"WO_INITIAL_ID\" = A.\"WO_INITIAL_ID\")) AS CREATOR,";
                //strQry += " (CASE \"WO_APPROVE_STATUS\" WHEN '0' THEN 'PENDING' WHEN '1' THEN 'APPROVED' WHEN '2' THEN 'MODIFY AND APPROVE' WHEN '3' THEN 'REJECTED' ELSE 'OTHERS' END) \"STATUS\", \"WO_APPROVE_STATUS\" ,";
                //strQry += " (SELECT \"RO_NAME\" FROM \"TBLROLES\" WHERE \"WO_NEXT_ROLE\"=\"RO_ID\") RO_NAME,";
                //strQry += " (SELECT \"WO_NEXT_ROLE\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\" =(SELECT MAX(\"WO_ID\") FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_RECORD_ID\" = A.\"WO_RECORD_ID\" )) NEXT_ROLE,";
                //strQry += " CASE (SELECT \"WO_NEXT_ROLE\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\" =(SELECT MAX(\"WO_ID\") FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_RECORD_ID\" = A.\"WO_RECORD_ID\" ))";
                //strQry += " WHEN 0 THEN 'APPROVED' ELSE 'PENDING' END CURRENT_STATUS,\"WO_DESCRIPTION\",0 AS WOA_ID,";
                //strQry += " \"WO_WFO_ID\", \"WO_WFO_ID\" as \"WO_REF_OFFCODE\", \"WO_INITIAL_ID\" ";
                //strQry += " FROM \"TBLWORKFLOWOBJECTS\" A, \"TBLBUSINESSOBJECT\" B WHERE \"WO_BO_ID\" = \"BO_ID\"  AND \"WO_NEXT_ROLE\" =:sRoleId";
                //strQry += "  AND \"WO_APPROVE_STATUS\" ='3'  ";
                //NpgsqlCommand.Parameters.AddWithValue("sRoleId", Convert.ToInt32(objApproval.sRoleId));

                //if (objApproval.sRoleType == "2")
                //{
                //    string sOffCode = clsStoreOffice.GetOfficeCode(objApproval.sOfficeCode, "WO_REF_OFFCODE");
                //    strQry += "AND " + sOffCode;



                //    //30-03-220 BY MADAN FOR STORE SPLIT 
                //    if (objApproval.sOfficeCode == Convert.ToString(ConfigurationManager.AppSettings["NEWSTORE"]))
                //    {

                //        strQry += " or CAST(\"WO_STATUS\"  AS TEXT) ='1')";

                //    }

                //    if (objApproval.sOfficeCode == Convert.ToString(ConfigurationManager.AppSettings["OLDSTORE"]))
                //    {

                //        strQry += " and (CAST(\"WO_STATUS\"  AS TEXT) is null or  cast(\"WO_STATUS\" as text)=''))";

                //    }
                //}

                //if (objApproval.sRoleType == "1")
                //{
                //    strQry += " AND CAST(\"WO_REF_OFFCODE\"  AS TEXT) LIKE :sOfficeCode||'%'";
                //    NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", objApproval.sOfficeCode);
                //}




                //if (objApproval.sBOId != null)
                //{
                //    strQry += " AND \"WO_BO_ID\" =:sBOId1 ";
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId1", Convert.ToInt32(objApproval.sBOId));
                //}
                //if (objApproval.sCrby != null)
                //{
                //    strQry += " AND \"WO_CR_BY\" =:sCrby1 ";
                //    NpgsqlCommand.Parameters.AddWithValue("sCrby1", Convert.ToInt32(objApproval.sCrby));
                //}
                //if (objApproval.sFromDate != "")
                //{
                //    DateTime dFromDate = DateTime.ParseExact(objApproval.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    strQry += " AND TO_CHAR(\"WO_CR_ON\" ,'YYYYMMDD')>=:dFromDate";
                //    NpgsqlCommand.Parameters.AddWithValue("dFromDate", dFromDate.ToString("yyyyMMdd"));

                //}
                //if (objApproval.sToDate != "")
                //{
                //    DateTime dToDate = DateTime.ParseExact(objApproval.sToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    strQry += " AND TO_CHAR(\"WO_CR_ON\" ,'YYYYMMDD')<=:dToDate";
                //    NpgsqlCommand.Parameters.AddWithValue("dToDate", dToDate.ToString("yyyyMMdd"));
                //}
                //if (objApproval.sFormName != "")
                //{
                //    strQry += " AND UPPER(\"BO_NAME\") LIKE :sFormName2||'%' ";
                //    NpgsqlCommand.Parameters.AddWithValue("sFormName2", objApproval.sFormName.ToUpper());
                //}
                //if (objApproval.sDescription != "")
                //{
                //    strQry += " AND UPPER(\"WO_DESCRIPTION\") LIKE '%'||:sDescription2||'%' ";
                //    NpgsqlCommand.Parameters.AddWithValue("sDescription2", objApproval.sDescription.ToUpper());
                //}

                //strQry += " ORDER BY \"WO_ID\" DESC) A  LIMIT 300";
                //return objcon.FetchDataTable(strQry, NpgsqlCommand);

                #endregion
                string WoRefoffcode = clsStoreOffice.GetOfficeCode(objApproval.sOfficeCode, "WO_REF_OFFCODE");
                DateTime ExactFromDate = new DateTime();
                DateTime ExactToDate = new DateTime();

                if (objApproval.sFromDate != "")
                {
                    ExactFromDate = DateTime.ParseExact(objApproval.sFromDate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture);

                }
                if (objApproval.sToDate != "")
                {
                    ExactToDate = DateTime.ParseExact(objApproval.sToDate.Replace('-', '/'), "d/M/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture);
                }
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_loadrejectedapproved_clsapproval");
                cmd.Parameters.AddWithValue("roleid", Convert.ToString(objApproval.sRoleId));
                cmd.Parameters.AddWithValue("role_type", objApproval.sRoleType);
                cmd.Parameters.AddWithValue("soffcode", WoRefoffcode);
                cmd.Parameters.AddWithValue("office_code", objApproval.sOfficeCode);
                cmd.Parameters.AddWithValue("config_newstore", Convert.ToString(ConfigurationManager.AppSettings["NEWSTORE"]));
                cmd.Parameters.AddWithValue("config_oldstore", Convert.ToString(ConfigurationManager.AppSettings["OLDSTORE"]));
                cmd.Parameters.AddWithValue("bo_id", objApproval.sBOId == null ? "" : Convert.ToString(objApproval.sBOId));
                cmd.Parameters.AddWithValue("cr_by", objApproval.sCrby == null ? "" : Convert.ToString(objApproval.sCrby));
                cmd.Parameters.AddWithValue("from_date", Convert.ToString(objApproval.sFromDate));
                cmd.Parameters.AddWithValue("to_date", Convert.ToString(objApproval.sToDate));
                cmd.Parameters.AddWithValue("form_name", Convert.ToString(objApproval.sFormName.ToUpper()));
                cmd.Parameters.AddWithValue("description", Convert.ToString(objApproval.sDescription.ToUpper()));
                cmd.Parameters.AddWithValue("dfromdate", ExactFromDate.ToString("yyyyMMdd"));
                cmd.Parameters.AddWithValue("dtodate", ExactToDate.ToString("yyyyMMdd"));
                dt = objcon.FetchDataTable(cmd);

                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        /// <summary>
        /// Save Workflow Data to TBLWFODATA table like QueryValues,ParameterValues and XML format
        /// </summary>
        /// <param name="objApproval"></param>
        /// <returns></returns>
        public bool SaveWorkFlowData(clsApproval objApproval)
        {
            try
            {
                string strQry = string.Empty;
                string sResult = string.Empty;
                string[] Arr = new string[3];
                if (objApproval.sTableNames != "" && objApproval.sTableNames != null)
                {
                    sResult = CreateXml(objApproval.sColumnNames, objApproval.sColumnValues, objApproval.sTableNames);
                    sResult = sResult.Replace("'", "''");
                }
                if (objApproval.sFormName != null && objApproval.sFormName != "")
                {
                    //To get Business Object Id
                    //NpgsqlCommand = new NpgsqlCommand();
                    //NpgsqlCommand.Parameters.AddWithValue("sFormName", objApproval.sFormName.Trim().ToUpper());
                    //objApproval.sBOId = objcon.get_value("SELECT \"BO_ID\" FROM \"TBLBUSINESSOBJECT\" WHERE UPPER(\"BO_FORMNAME\")=:sFormName", NpgsqlCommand);

                    #region Converted to sp
                    DataBseConnection objDatabse = new DataBseConnection(Constants.Password);
                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_workflow_getvalue_clsapproval");
                    cmd.Parameters.AddWithValue("p_key", "SAVEWORKFLOW_GETBO_ID");
                    cmd.Parameters.AddWithValue("p_value", objApproval.sFormName.Trim().ToUpper());
                    cmd.Parameters.AddWithValue("p_offcode", "");
                    objApproval.sBOId = objDatabse.StringGetValue(cmd);
                    #endregion
                }


                //objApproval.sWFDataId = Convert.ToString(objcon.Get_max_no("WFO_ID", "TBLWFODATA"));
                //strQry = "INSERT INTO \"TBLWFODATA\" (\"WFO_ID\",\"WFO_QUERY_VALUES\",\"WFO_PARAMETER\",\"WFO_DATA\",\"WFO_CR_BY\",\"WFO_BO_ID\") VALUES (";
                //strQry += " '" + objApproval.sWFDataId + "','" + objApproval.sQryValues + "','" + objApproval.sParameterValues + "','" + sResult + "',";
                //strQry += " '" + objApproval.sCrby + "','" + objApproval.sBOId + "')";
                //objcon.ExecuteQry(strQry);

                #region Converted to sp
                NpgsqlCommand cmdinsert = new NpgsqlCommand("proc_insert_tblwfodata_clsapproval");
                cmdinsert.Parameters.AddWithValue("sqryvalues", (objApproval.sQryValues ?? ""));
                cmdinsert.Parameters.AddWithValue("sparametervalues", (objApproval.sParameterValues ?? ""));
                cmdinsert.Parameters.AddWithValue("sresult", sResult);
                cmdinsert.Parameters.AddWithValue("scrby", Convert.ToInt32(objApproval.sCrby));
                cmdinsert.Parameters.AddWithValue("sboid", Convert.ToInt32(objApproval.sBOId));
                cmdinsert.Parameters.Add("msg", NpgsqlDbType.Text);
                cmdinsert.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmdinsert.Parameters.Add("pk_id", NpgsqlDbType.Text);
                cmdinsert.Parameters["msg"].Direction = ParameterDirection.Output;
                cmdinsert.Parameters["op_id"].Direction = ParameterDirection.Output;
                cmdinsert.Parameters["pk_id"].Direction = ParameterDirection.Output;
                Arr[0] = "msg";
                Arr[1] = "op_id";
                Arr[2] = "pk_id";
                Arr = objcon.Execute(cmdinsert, Arr, 3);
                objApproval.sWFDataId = Arr[2].ToString();

                #endregion
                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }


        /// <summary>
        /// Save Workflow Data to TBLWFODATA table like QueryValues,ParameterValues and XML format
        /// </summary>
        /// <param name="objApproval"></param>
        /// <returns></returns>
        public bool SaveWorkFlowData_Latest(clsApproval objApproval, DataBseConnection objDatabse)
        {

            try
            {
                string p_key = string.Empty;
                string strQry = string.Empty;
                string sResult = string.Empty;

                if (objApproval.sTableNames != "" && objApproval.sTableNames != null)
                {
                    sResult = CreateXml(objApproval.sColumnNames, objApproval.sColumnValues, objApproval.sTableNames);
                    sResult = sResult.Replace("'", "''");
                }
                if (objApproval.sFormName != null && objApproval.sFormName != "")
                {
                    //To get Business Object Id
                    //NpgsqlCommand = new NpgsqlCommand();
                    //NpgsqlCommand.Parameters.AddWithValue("sFormName", objApproval.sFormName.Trim().ToUpper());
                    //objApproval.sBOId = objDatabse.get_value("SELECT \"BO_ID\" FROM \"TBLBUSINESSOBJECT\" WHERE UPPER(\"BO_FORMNAME\")=:sFormName", NpgsqlCommand);

                    #region Converted to sp
                    p_key = "SAVEWORKFLOW_GETBO_ID";
                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_workflow_getvalue_clsapproval");
                    cmd.Parameters.AddWithValue("p_key", Convert.ToString(p_key));
                    cmd.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sFormName.Trim().ToUpper()));
                    cmd.Parameters.AddWithValue("p_offcode", "");
                    objApproval.sBOId = objDatabse.StringGetValue(cmd);
                    #endregion
                }
                int NoofTimes = 0;
                LOOP:

                //objApproval.sWFDataId = Convert.ToString(objDatabse.Get_max_no("WFO_ID", "TBLWFODATA"));
                //strQry = "INSERT INTO \"TBLWFODATA\" (\"WFO_ID\",\"WFO_QUERY_VALUES\",\"WFO_PARAMETER\",\"WFO_DATA\",\"WFO_CR_BY\",\"WFO_BO_ID\") VALUES (";
                //strQry += " '" + objApproval.sWFDataId + "','" + objApproval.sQryValues + "','" + objApproval.sParameterValues + "','" + sResult + "',";
                //strQry += " '" + objApproval.sCrby + "','" + objApproval.sBOId + "')";


                //try
                //{
                //    objDatabse.ExecuteQry(strQry);
                //}

                #region Converted to sp
                string[] Arr = new string[3];
                NpgsqlCommand cmdinsert = new NpgsqlCommand("proc_insert_tblwfodata_clsapproval");
                cmdinsert.Parameters.AddWithValue("sqryvalues", objApproval.sQryValues);
                cmdinsert.Parameters.AddWithValue("sparametervalues", Convert.ToString(objApproval.sParameterValues ?? ""));
                cmdinsert.Parameters.AddWithValue("sresult", sResult);
                cmdinsert.Parameters.AddWithValue("scrby", Convert.ToInt32(objApproval.sCrby));
                cmdinsert.Parameters.AddWithValue("sboid", Convert.ToInt32(objApproval.sBOId));
                cmdinsert.Parameters.Add("msg", NpgsqlDbType.Text);
                cmdinsert.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmdinsert.Parameters.Add("pk_id", NpgsqlDbType.Text);

                cmdinsert.Parameters["msg"].Direction = ParameterDirection.Output;
                cmdinsert.Parameters["op_id"].Direction = ParameterDirection.Output;
                cmdinsert.Parameters["pk_id"].Direction = ParameterDirection.Output;

                Arr[0] = "msg";
                Arr[1] = "op_id";
                Arr[2] = "pk_id";

                try
                {
                    Arr = objDatabse.Execute(cmdinsert, Arr, 3);
                    objApproval.sWFDataId = Arr[2].ToString();
                }
                #endregion

                catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(300);
                    NoofTimes++;
                    if (NoofTimes <= 5)
                    {
                        goto LOOP;
                    }
                    else
                    {
                        throw ex;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                throw ex;
                //return false;
            }
        }

        /// <summary>
        /// Generate Temporary Record Id to Insert in TBLWORKFLOWOBJECTS Table. i.e -1,-2 etc
        /// </summary>
        /// <returns></returns>
        public string GetRecordIdForWorkFlow()
        {
            try
            {
                //string strQry = string.Empty;


                //strQry = " SELECT  COALESCE(MIN(\"WO_RECORD_ID\"),0)-1 FROM \"TBLWORKFLOWOBJECTS\"";
                //string sResult = objcon.get_value(strQry);
                //if (Convert.ToInt32(sResult) >= 0)
                //{
                //    sResult = "-1";
                //}
                #region Converted to sp
                DataBseConnection objDatabse = new DataBseConnection(Constants.Password);
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_workflow_getvalue_clsapproval");
                cmd.Parameters.AddWithValue("p_key", "WO_RECORD_ID");
                cmd.Parameters.AddWithValue("p_value", "");
                cmd.Parameters.AddWithValue("p_offcode", "");
                string sResult = objDatabse.StringGetValue(cmd);
                if (Convert.ToInt32(sResult) >= 0)
                {
                    sResult = "-1";
                }
                #endregion 
                return sResult;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        /// <summary>
        /// Generate Temporary Record Id to Insert in TBLWORKFLOWOBJECTS Table. i.e -1,-2 etc
        /// </summary>
        /// <returns></returns>
        public string GetRecordIdForWorkFlow_Latest(DataBseConnection objDatabse)
        {
            try
            {
                //string strQry = string.Empty;
                //strQry = " SELECT  COALESCE(MIN(\"WO_RECORD_ID\"),0)-1 FROM \"TBLWORKFLOWOBJECTS\"";
                //string sResult = objDatabse.get_value(strQry);
                //if (Convert.ToInt32(sResult) >= 0)
                //{
                //    sResult = "-1";
                //}
                #region Converted to sp
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_workflow_getvalue_clsapproval");
                cmd.Parameters.AddWithValue("p_key", "WO_RECORD_ID");
                cmd.Parameters.AddWithValue("p_value", "");
                cmd.Parameters.AddWithValue("p_offcode", "");
                string sResult = objDatabse.StringGetValue(cmd);
                if (Convert.ToInt32(sResult) >= 0)
                {
                    sResult = "-1";
                }
                #endregion 


                return sResult;
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                throw ex;
                // return ex.Message;
            }
        }


        /// <summary>
        /// Save WorkFlow object like Who is Next Role to access the Bussiness Object.
        /// TBLWORKFLOWOBJECTS Table Contains  Transaction of WorkFlow based on Businens Object
        /// </summary>
        /// <param name="objApproval"></param>
        /// <returns></returns>
        public bool SaveWorkflowObjects(clsApproval objApproval)
        {
            try
            {
                string strQry = string.Empty;
                string sApproveResult = string.Empty;
                string[] Arr = new string[2];
                DataBseConnection objDatabse = new DataBseConnection(Constants.Password);

                if (Convert.ToString(ConfigurationManager.AppSettings["Approval"]).ToUpper().Equals("OFF"))
                {
                    return false;
                }

                if (objApproval.sFormName != null && objApproval.sFormName != "")
                {
                    //To get Business Object Id
                    //NpgsqlCommand = new NpgsqlCommand();
                    //NpgsqlCommand.Parameters.AddWithValue("sFormName", objApproval.sFormName.Trim().ToUpper());
                    //objApproval.sBOId = objcon.get_value("SELECT \"BO_ID\" FROM \"TBLBUSINESSOBJECT\" WHERE UPPER(\"BO_FORMNAME\")=:sFormName", NpgsqlCommand);

                    #region Converted to sp
                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_workflow_getvalue_clsapproval");
                    cmd.Parameters.AddWithValue("p_key", "GET_BO_ID");
                    cmd.Parameters.AddWithValue("p_value", objApproval.sFormName.Trim().ToUpper());
                    cmd.Parameters.AddWithValue("p_offcode", "");
                    objApproval.sBOId = objDatabse.StringGetValue(cmd);
                    #endregion
                }
                //fetching bfm_id from bo_id for updating intial id based on previosapproveid  n initialactionid
                if (objApproval.sbfm_type != null && objApproval.sbfm_type != "")
                {
                    //objApproval.sbfm_id = objcon.get_value("SELECT \"BFM_ID\" from \"TBLBO_FLOW_MASTER\" WHERE \"BFM_NEXT_BO_ID\"='" + objApproval.sBOId + "' and \"BFM_TYPE\"='" + objApproval.sbfm_type + "'");
                    #region Converted to sp
                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_workflow_getvalue_clsapproval");
                    cmd.Parameters.AddWithValue("p_key", "GET_BFM_ID");
                    cmd.Parameters.AddWithValue("p_value", objApproval.sBOId);
                    cmd.Parameters.AddWithValue("p_offcode", objApproval.sbfm_type);
                    objApproval.sbfm_id = objDatabse.StringGetValue(cmd);
                    #endregion

                }
                else if (objApproval.sTTKSComstatus == "1")
                {
                    //objApproval.sbfm_id = objcon.get_value("SELECT \"BFM_ID\" from \"TBLBO_FLOW_MASTER\" WHERE \"BFM_NEXT_BO_ID\"='" + objApproval.sBOId + "' and \"BFM_BO_ID\"=11 ");
                    #region Converted to sp
                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_workflow_getvalue_clsapproval");
                    cmd.Parameters.AddWithValue("p_key", "GET_TTKBFM_ID");
                    cmd.Parameters.AddWithValue("p_value", objApproval.sBOId);
                    cmd.Parameters.AddWithValue("p_offcode", "");
                    objApproval.sbfm_id = objDatabse.StringGetValue(cmd);
                    #endregion
                }
                else
                {
                    //objApproval.sbfm_id = objcon.get_value("SELECT \"BFM_ID\" from \"TBLBO_FLOW_MASTER\" WHERE \"BFM_NEXT_BO_ID\"='" + objApproval.sBOId + "' ");
                    #region Converted to sp
                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_workflow_getvalue_clsapproval");
                    cmd.Parameters.AddWithValue("p_key", "GET_ELSE_BFM_ID");
                    cmd.Parameters.AddWithValue("p_value", objApproval.sBOId);
                    cmd.Parameters.AddWithValue("p_offcode", "");
                    objApproval.sbfm_id = objDatabse.StringGetValue(cmd);
                    #endregion

                }
                if (objApproval.sBOId == "62")
                {
                    objApproval.sbfm_id = "22";
                }
                //Check Approval Exists
                bool bResult = CheckFormApprovalExists(objApproval);

                if (bResult == true)
                {
                    //To get Role Id with Priority Level                 
                    objApproval.sRoleId = GetRoleFromApprovePriority(objApproval);
                }
                else
                {
                    objApproval.sRoleId = "";

                }

                if (objApproval.sPrevApproveId == null)
                {
                    objApproval.sPrevApproveId = "0";
                }

                if (objApproval.sRoleId != "" && objApproval.sRoleId != null)
                {

                    objApproval.sPrevWFOId = objApproval.sWFObjectId;
                    #region inline query
                    //string sWFlowId = Convert.ToString(objcon.get_value("SELECT COALESCE (MAX(\"WO_ID\")+1,1) FROM \"TBLWORKFLOWOBJECTS\""));

                    //objApproval.sWFObjectId = sWFlowId;
                    //strQry = "INSERT INTO \"TBLWORKFLOWOBJECTS\" (\"WO_ID\",\"WO_BO_ID\",\"WO_RECORD_ID\",\"WO_PREV_APPROVE_ID\",\"WO_NEXT_ROLE\",\"WO_OFFICE_CODE\",";
                    //strQry += " \"WO_CLIENT_IP\",\"WO_CR_BY\",\"WO_DESCRIPTION\",\"WO_WFO_ID\",\"WO_DATA_ID\",\"WO_REF_OFFCODE\",\"WO_STATUS\")";
                    //strQry += " VALUES (:sWFlowId1,:sBOId2,:sRecordId2,:sPrevApproveId2,";
                    //strQry += " :sRoleId3,:sOfficeCode3,:sClientIp,:sCrby4,";
                    //strQry += " :sDescription4,:sWFDataId4,:sDataReferenceId3,:sRefOfficeCode4,:status)";
                    //NpgsqlCommand = new NpgsqlCommand();
                    //NpgsqlCommand.Parameters.AddWithValue("sWFlowId1", Convert.ToInt32(sWFlowId));
                    //NpgsqlCommand.Parameters.AddWithValue("sBOId2", Convert.ToInt32(objApproval.sBOId));
                    //NpgsqlCommand.Parameters.AddWithValue("sRecordId2", Convert.ToInt32(objApproval.sRecordId));
                    //NpgsqlCommand.Parameters.AddWithValue("sPrevApproveId2", Convert.ToInt32(objApproval.sPrevApproveId));
                    //NpgsqlCommand.Parameters.AddWithValue("sRoleId3", Convert.ToInt32(objApproval.sRoleId));
                    //NpgsqlCommand.Parameters.AddWithValue("sOfficeCode3", Convert.ToInt32(objApproval.sOfficeCode));


                    //if (objApproval.sClientIp == null || objApproval.sClientIp == "")
                    //{
                    //    objApproval.sClientIp = "";
                    //}
                    //NpgsqlCommand.Parameters.AddWithValue("sClientIp", objApproval.sClientIp);
                    //NpgsqlCommand.Parameters.AddWithValue("sCrby4", Convert.ToInt32(objApproval.sCrby));
                    //NpgsqlCommand.Parameters.AddWithValue("sDescription4", objApproval.sDescription);
                    //NpgsqlCommand.Parameters.AddWithValue("sWFDataId4", objApproval.sWFDataId);
                    //if (objApproval.sDataReferenceId == null || objApproval.sDataReferenceId == "")
                    //{
                    //    objApproval.sDataReferenceId = "";
                    //}

                    //NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId3", objApproval.sDataReferenceId);
                    //NpgsqlCommand.Parameters.AddWithValue("sRefOfficeCode4", Convert.ToInt32(objApproval.sRefOfficeCode));

                    //if (objApproval.sStatus == null || objApproval.sStatus == "")
                    //{
                    //    objApproval.sStatus = "";
                    //}
                    //NpgsqlCommand.Parameters.AddWithValue("status", objApproval.sStatus);

                    //objcon.ExecuteQry(strQry, NpgsqlCommand);
                    #endregion

                    #region Converted to sp
                    string[] Arrinsert = new string[3];
                    NpgsqlCommand cmdinsert = new NpgsqlCommand("proc_insert_tblworkflowobjects_clsapproval");
                    cmdinsert.Parameters.AddWithValue("bo_id", Convert.ToInt32(objApproval.sBOId));
                    cmdinsert.Parameters.AddWithValue("recordid", Convert.ToInt32(objApproval.sRecordId));
                    cmdinsert.Parameters.AddWithValue("sprevapproveid", Convert.ToInt32(objApproval.sPrevApproveId));
                    cmdinsert.Parameters.AddWithValue("sroleid", Convert.ToInt32(objApproval.sRoleId));
                    cmdinsert.Parameters.AddWithValue("sofficecode", Convert.ToInt32(objApproval.sOfficeCode));
                    cmdinsert.Parameters.AddWithValue("sclientip", objApproval.sClientIp);
                    cmdinsert.Parameters.AddWithValue("scrby", Convert.ToInt32(objApproval.sCrby));
                    cmdinsert.Parameters.AddWithValue("sdescription", objApproval.sDescription);
                    cmdinsert.Parameters.AddWithValue("swfdataid", objApproval.sWFDataId);
                    cmdinsert.Parameters.AddWithValue("sdatareferenceid", (objApproval.sDataReferenceId ?? ""));
                    cmdinsert.Parameters.AddWithValue("srefofficecode", Convert.ToInt32(objApproval.sRefOfficeCode));
                    cmdinsert.Parameters.AddWithValue("status", (objApproval.sStatus ?? ""));
                    cmdinsert.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmdinsert.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmdinsert.Parameters.Add("pk_id", NpgsqlDbType.Text);
                    cmdinsert.Parameters["msg"].Direction = ParameterDirection.Output;
                    cmdinsert.Parameters["op_id"].Direction = ParameterDirection.Output;
                    cmdinsert.Parameters["pk_id"].Direction = ParameterDirection.Output;
                    Arrinsert[0] = "msg";
                    Arrinsert[1] = "op_id";
                    Arrinsert[2] = "pk_id";

                    Arr = objDatabse.Execute(cmdinsert, Arrinsert, 3);
                    string sWFlowId = Arr[2];
                    objApproval.sWFObjectId = sWFlowId;

                    #endregion

                    //Insert Initial Id for WorkFlow
                    if (objApproval.sWFInitialId == "" || objApproval.sWFInitialId == null)
                    {
                        //NpgsqlCommand = new NpgsqlCommand();
                        //strQry = "UPDATE \"TBLWORKFLOWOBJECTS\" SET \"WO_INITIAL_ID\"=:sWFlowId WHERE CAST(\"WO_ID\" AS TEXT) =:sWFlowId1";
                        //NpgsqlCommand.Parameters.AddWithValue("sWFlowId", Convert.ToInt32(sWFlowId));
                        //NpgsqlCommand.Parameters.AddWithValue("sWFlowId1", sWFlowId);
                        //objcon.ExecuteQry(strQry, NpgsqlCommand);

                        #region Converted to sp
                        NpgsqlCommand cmd = new NpgsqlCommand("proc_update_workflowobjects_clsapproval");
                        cmd.Parameters.AddWithValue("wfinitialid", Convert.ToInt32(sWFlowId));
                        cmd.Parameters.AddWithValue("swflowid", Convert.ToString(sWFlowId));
                        cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                        cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                        cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                        cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                        Arr[0] = "msg";
                        Arr[1] = "op_id";
                        Arr = objcon.Execute(cmd, Arr, 2);
                        #endregion
                    }
                    else
                    {
                        //NpgsqlCommand = new NpgsqlCommand();
                        //strQry = "UPDATE \"TBLWORKFLOWOBJECTS\" SET \"WO_INITIAL_ID\" =:sWFInitialId1 WHERE CAST(\"WO_ID\" AS TEXT) =:sWFlowId2";
                        //NpgsqlCommand.Parameters.AddWithValue("sWFInitialId1", Convert.ToInt32(objApproval.sWFInitialId));
                        //NpgsqlCommand.Parameters.AddWithValue("sWFlowId2", sWFlowId);
                        //objcon.ExecuteQry(strQry, NpgsqlCommand);

                        #region Converted to sp
                        NpgsqlCommand cmd = new NpgsqlCommand("proc_update_workflowobjects_clsapproval");
                        cmd.Parameters.AddWithValue("wfinitialid", Convert.ToInt32(objApproval.sWFInitialId));
                        cmd.Parameters.AddWithValue("swflowid", Convert.ToString(sWFlowId));
                        cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                        cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                        cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                        cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                        Arr[0] = "msg";
                        Arr[1] = "op_id";
                        Arr = objcon.Execute(cmd, Arr, 2);
                        #endregion

                    }

                    UpdateWFOAutoObject(objApproval);

                    //Saving SMS in Table to send to Next Role
                    SendSMStoRole(objApproval, "");
                    return true;
                }
                else
                {
                    //NpgsqlCommand = new NpgsqlCommand();
                    //strQry = "SELECT COUNT(*) FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_BOID\" =:sBOId5";
                    //NpgsqlCommand.Parameters.AddWithValue("sBOId5", Convert.ToInt32(objApproval.sBOId));
                    //string count = objcon.get_value(strQry, NpgsqlCommand);

                    #region Converted to sp
                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_workflow_getvalue_clsapproval");
                    cmd.Parameters.AddWithValue("p_key", "COUNT");
                    cmd.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sBOId));
                    cmd.Parameters.AddWithValue("p_offcode", "");
                    string count = objDatabse.StringGetValue(cmd);
                    #endregion
                    if (Convert.ToInt16(count) > 1 || objApproval.sBOId == "48")
                    {
                        if (objApproval.sPrevApproveId == "0" && objApproval.sRoleId == "")
                        {
                            objApproval.sWFDataId = "";
                            objApproval.sWFInitialId = "";
                        }
                    }
                    #region Inline Query
                    //string sWFlowId = Convert.ToString(objcon.get_value(" SELECT MAX(\"WO_ID\")+1 FROM \"TBLWORKFLOWOBJECTS\""));

                    //strQry = "INSERT INTO \"TBLWORKFLOWOBJECTS\" (\"WO_ID\",\"WO_BO_ID\",\"WO_RECORD_ID\",\"WO_PREV_APPROVE_ID\",\"WO_NEXT_ROLE\",\"WO_OFFICE_CODE\",\"WO_CLIENT_IP\",";
                    //strQry += " \"WO_CR_BY\",\"WO_APPROVE_STATUS\",\"WO_DESCRIPTION\",\"WO_WFO_ID\",\"WO_DATA_ID\",\"WO_REF_OFFCODE\",\"WO_STATUS\")";
                    //strQry += " VALUES (:sWFlowId,:sBOId,:sRecordId,:sPrevApproveId,";
                    //strQry += " '0',:sOfficeCode,:sClientIp,:sCrby,";
                    //strQry += " '1',:sDescription,:sWFDataId,";

                    //if (objApproval.sDataReferenceId != null)
                    //{
                    //    strQry += ":sDataReferenceId,";
                    //}
                    //else
                    //{
                    //    strQry += "NULL,";
                    //}

                    //if (objApproval.sRefOfficeCode != null)
                    //{
                    //    strQry += ":sRefOfficeCode";
                    //}
                    //else
                    //{
                    //    strQry += "NULL";
                    //}

                    //strQry += ",:status)";

                    //NpgsqlCommand = new NpgsqlCommand();
                    //NpgsqlCommand.Parameters.AddWithValue("sWFlowId", Convert.ToInt32(sWFlowId));
                    //NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objApproval.sBOId));

                    //NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objApproval.sRecordId));
                    //NpgsqlCommand.Parameters.AddWithValue("sPrevApproveId", Convert.ToInt32(objApproval.sPrevApproveId));
                    //// NpgsqlCommand.Parameters.AddWithValue("sRoleId3", objApproval.sRoleId);
                    //NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", Convert.ToInt32(objApproval.sOfficeCode));
                    //NpgsqlCommand.Parameters.AddWithValue("sClientIp", objApproval.sClientIp);
                    //NpgsqlCommand.Parameters.AddWithValue("sCrby", Convert.ToInt32(objApproval.sCrby));
                    //NpgsqlCommand.Parameters.AddWithValue("sDescription", objApproval.sDescription);

                    //if (objApproval.sWFDataId == null || objApproval.sWFDataId == "")
                    //{
                    //    objApproval.sWFDataId = "";
                    //}
                    //NpgsqlCommand.Parameters.AddWithValue("sWFDataId", Convert.ToString(objApproval.sWFDataId));
                    //NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", objApproval.sDataReferenceId);
                    //NpgsqlCommand.Parameters.AddWithValue("sRefOfficeCode", Convert.ToInt32(objApproval.sRefOfficeCode));

                    //if (objApproval.sStatus == null || objApproval.sStatus == "")
                    //{
                    //    objApproval.sStatus = "";
                    //}
                    //NpgsqlCommand.Parameters.AddWithValue("status", objApproval.sStatus);

                    //objcon.ExecuteQry(strQry, NpgsqlCommand);
                    #endregion

                    #region Converted to sp

                    string[] Arrinsert = new string[3];
                    NpgsqlCommand cmdinsert = new NpgsqlCommand("proc_insert_createlevel_workflowobjects_clsapproval");
                    cmdinsert.Parameters.AddWithValue("bo_id", Convert.ToInt32(objApproval.sBOId));
                    cmdinsert.Parameters.AddWithValue("recordid", Convert.ToInt32(objApproval.sRecordId));
                    cmdinsert.Parameters.AddWithValue("sprevapproveid", Convert.ToInt32(objApproval.sPrevApproveId));
                    cmdinsert.Parameters.AddWithValue("sofficecode", Convert.ToInt32(objApproval.sOfficeCode));
                    cmdinsert.Parameters.AddWithValue("sclientip", objApproval.sClientIp);
                    cmdinsert.Parameters.AddWithValue("scrby", Convert.ToInt32(objApproval.sCrby));
                    cmdinsert.Parameters.AddWithValue("sdescription", objApproval.sDescription);

                    cmdinsert.Parameters.AddWithValue("swfdataid", (objApproval.sWFDataId ?? ""));
                    cmdinsert.Parameters.AddWithValue("sdatareferenceid", (objApproval.sDataReferenceId ?? ""));
                    cmdinsert.Parameters.AddWithValue("srefofficecode", Convert.ToInt32(objApproval.sRefOfficeCode));
                    cmdinsert.Parameters.AddWithValue("status", (objApproval.sStatus ?? ""));
                    cmdinsert.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmdinsert.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmdinsert.Parameters.Add("pk_id", NpgsqlDbType.Text);
                    cmdinsert.Parameters["msg"].Direction = ParameterDirection.Output;
                    cmdinsert.Parameters["op_id"].Direction = ParameterDirection.Output;
                    cmdinsert.Parameters["pk_id"].Direction = ParameterDirection.Output;
                    Arrinsert[0] = "msg";
                    Arrinsert[1] = "op_id";
                    Arrinsert[2] = "pk_id";

                    Arrinsert = objcon.Execute(cmdinsert, Arrinsert, 3);
                    string sWFlowId = Arrinsert[2];

                    #endregion

                    //Insert Initial Id for WorkFlow
                    if (objApproval.sWFInitialId == "" || objApproval.sWFInitialId == null)
                    {
                        //NpgsqlCommand = new NpgsqlCommand();
                        //strQry = "UPDATE \"TBLWORKFLOWOBJECTS\" SET \"WO_INITIAL_ID\" =:sWFlowId WHERE \"WO_ID\" =:sWFlowId1";
                        //NpgsqlCommand.Parameters.AddWithValue("sWFlowId", Convert.ToInt32(sWFlowId));
                        //NpgsqlCommand.Parameters.AddWithValue("sWFlowId1", Convert.ToInt32(sWFlowId));
                        //objcon.ExecuteQry(strQry, NpgsqlCommand);

                        #region Converted to sp
                        NpgsqlCommand cmdupdate = new NpgsqlCommand("proc_update_workflowobjects_clsapproval");
                        cmdupdate.Parameters.AddWithValue("wfinitialid", Convert.ToInt32(sWFlowId));
                        cmdupdate.Parameters.AddWithValue("swflowid", Convert.ToString(sWFlowId));
                        cmdupdate.Parameters.Add("msg", NpgsqlDbType.Text);
                        cmdupdate.Parameters.Add("op_id", NpgsqlDbType.Text);
                        cmdupdate.Parameters["msg"].Direction = ParameterDirection.Output;
                        cmdupdate.Parameters["op_id"].Direction = ParameterDirection.Output;
                        Arr[0] = "msg";
                        Arr[1] = "op_id";
                        Arr = objcon.Execute(cmdupdate, Arr, 2);
                        #endregion
                    }
                    else
                    {
                        //NpgsqlCommand = new NpgsqlCommand();
                        //strQry = "UPDATE \"TBLWORKFLOWOBJECTS\" SET \"WO_INITIAL_ID\" =:sWFInitialId WHERE \"WO_ID\" =:sWFlowId";
                        //NpgsqlCommand.Parameters.AddWithValue("sWFInitialId", Convert.ToInt32(objApproval.sWFInitialId));
                        //NpgsqlCommand.Parameters.AddWithValue("sWFlowId", Convert.ToInt32(sWFlowId));
                        //objcon.ExecuteQry(strQry, NpgsqlCommand);

                        #region Converted to sp
                        NpgsqlCommand cmdupdate = new NpgsqlCommand("proc_update_workflowobjects_clsapproval");
                        cmdupdate.Parameters.AddWithValue("wfinitialid", Convert.ToInt32(objApproval.sWFInitialId));
                        cmdupdate.Parameters.AddWithValue("swflowid", sWFlowId);
                        cmdupdate.Parameters.Add("msg", NpgsqlDbType.Text);
                        cmdupdate.Parameters.Add("op_id", NpgsqlDbType.Text);
                        cmdupdate.Parameters["msg"].Direction = ParameterDirection.Output;
                        cmdupdate.Parameters["op_id"].Direction = ParameterDirection.Output;
                        Arr[0] = "msg";
                        Arr[1] = "op_id";
                        Arr = objcon.Execute(cmdupdate, Arr, 2);
                        #endregion
                    }

                    if (objApproval.sBOId == "29")
                    {
                        objApproval.sWFDataId = "";
                    }
                    UpdateToMainTable(objApproval);


                    string sPrevBoId = objApproval.sBOId;
                    if (objApproval.sStoreType != null)
                    {
                        objApproval.sFailType = objApproval.sStoreType;
                    }
                    if (objApproval.sTTKStatus == "1")
                    {
                        objApproval.sFailType = "3";
                    }

                    string sResult = GetNextBOId(objApproval.sBOId, objApproval.sFailType, objApproval.sGuarentyType);
                    objApproval.sPrevWFOId = objApproval.sWFObjectId;
                    objApproval.sWFObjectId = sWFlowId;
                    int i = 0;
                    string[] sarray = sResult.Split('|');

                    if (sResult != "")
                    {
                        for (int x = 0; x < sarray.Length; x++)
                        {
                            sResult = sarray[x];
                            objApproval.sBOFlowMasterId = sResult.Split('~').GetValue(1).ToString();
                            objApproval.sBOId = sResult.Split('~').GetValue(0).ToString();

                            //To get Role Id with Priority Level to Create Form
                            objApproval.sRoleId = GetRoleFromApprovePriorityForBOCreate(objApproval);

                            UpdateWFOAutoObject(objApproval);
                            i++;

                            //SaveWFObjectAuto(objApproval);

                            bool resrult = SaveWFObjectAuto(objApproval);
                            if (resrult == false)
                            {
                                return false;
                            }
                        }

                    }
                    if (i == 0)
                    {
                        UpdateWFOAutoObject(objApproval);
                    }

                    //Saving SMS in Table to send to Next Role
                    SendSMStoRole(objApproval, sPrevBoId);

                    return true;

                }

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }


        /// <summary>
        /// Save WorkFlow object like Who is Next Role to access the Bussiness Object.
        /// TBLWORKFLOWOBJECTS Table Contains  Transaction of WorkFlow based on Businens Object
        /// </summary>
        /// <param name="objApproval"></param>
        /// <returns></returns>
        public bool SaveWorkflowObjects_Latest(clsApproval objApproval, DataBseConnection objDatabse)
        {
            string strQry = string.Empty;
            try
            {
                string sApproveResult = string.Empty;
                if (Convert.ToString(ConfigurationManager.AppSettings["Approval"]).ToUpper().Equals("OFF"))
                {
                    return false;
                }
                if ((objApproval.sFormName ?? "").Length > 0)
                {
                    //NpgsqlCommand = new NpgsqlCommand();
                    //NpgsqlCommand.Parameters.AddWithValue("sFormName", objApproval.sFormName.Trim().ToUpper());
                    //objApproval.sBOId = objDatabse.get_value("SELECT \"BO_ID\" FROM \"TBLBUSINESSOBJECT\" WHERE UPPER(\"BO_FORMNAME\")=:sFormName", NpgsqlCommand); //To get Business Object Id

                    #region Converted to sp
                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_workflow_getvalue_clsapproval");
                    cmd.Parameters.AddWithValue("p_key", "GET_BO_ID");
                    cmd.Parameters.AddWithValue("p_value", objApproval.sFormName.Trim().ToUpper());
                    cmd.Parameters.AddWithValue("p_offcode", "");
                    objApproval.sBOId = objDatabse.StringGetValue(cmd);
                    #endregion
                }
                //fetching bfm_id from bo_id for updating intial id based on previosapproveid  n initialactionid
                if ((objApproval.sbfm_type ?? "").Length > 0)
                {
                    //objApproval.sbfm_id = objDatabse.get_value("SELECT \"BFM_ID\" from \"TBLBO_FLOW_MASTER\" WHERE \"BFM_NEXT_BO_ID\"='" + objApproval.sBOId + "' and \"BFM_TYPE\"='" + objApproval.sbfm_type + "'");

                    #region Converted to sp
                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_workflow_getvalue_clsapproval");
                    cmd.Parameters.AddWithValue("p_key", "GET_BFM_ID");
                    cmd.Parameters.AddWithValue("p_value", objApproval.sBOId);
                    cmd.Parameters.AddWithValue("p_offcode", objApproval.sbfm_type);
                    objApproval.sbfm_id = objDatabse.StringGetValue(cmd);
                    #endregion
                }
                else if (objApproval.sTTKSComstatus == "1")
                {
                    //objApproval.sbfm_id = objDatabse.get_value("SELECT \"BFM_ID\" from \"TBLBO_FLOW_MASTER\" WHERE \"BFM_NEXT_BO_ID\"='" + objApproval.sBOId + "' and \"BFM_BO_ID\"=11 ");
                    #region Converted to sp
                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_workflow_getvalue_clsapproval");
                    cmd.Parameters.AddWithValue("p_key", "GET_TTKBFM_ID");
                    cmd.Parameters.AddWithValue("p_value", objApproval.sBOId);
                    cmd.Parameters.AddWithValue("p_offcode", "");
                    objApproval.sbfm_id = objDatabse.StringGetValue(cmd);
                    #endregion
                }
                else
                {
                    //objApproval.sbfm_id = objDatabse.get_value("SELECT \"BFM_ID\" from \"TBLBO_FLOW_MASTER\" WHERE \"BFM_NEXT_BO_ID\"='" + objApproval.sBOId + "' ");
                    #region Converted to sp
                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_workflow_getvalue_clsapproval");
                    cmd.Parameters.AddWithValue("p_key", "GET_ELSE_BFM_ID");
                    cmd.Parameters.AddWithValue("p_value", objApproval.sBOId);
                    cmd.Parameters.AddWithValue("p_offcode", "");
                    objApproval.sbfm_id = objDatabse.StringGetValue(cmd);
                    #endregion
                }
                if (objApproval.sBOId == "62")
                {
                    objApproval.sbfm_id = "22";
                }
                bool bResult = CheckFormApprovalExists_Latest(objApproval, objDatabse);    //Check Approval Exists
                if (bResult == true)
                {
                    objApproval.sRoleId = GetRoleFromApprovePriority_Latest(objApproval, objDatabse);  //To get Role Id with Priority Level     
                }
                else
                {
                    objApproval.sRoleId = "";
                }
                if (objApproval.sPrevApproveId == null)
                {
                    objApproval.sPrevApproveId = "0";
                }
                if ((objApproval.sRoleId ?? "").Length > 0)
                {
                    objApproval.sPrevWFOId = objApproval.sWFObjectId;
                    int NoofTimes = 0;
                    LOOP:
                    #region Inline Query
                    //string sWFlowId = Convert.ToString(objcon.get_value("SELECT COALESCE (MAX(\"WO_ID\")+1,1) FROM \"TBLWORKFLOWOBJECTS\""));
                    //objApproval.sWFObjectId = sWFlowId;
                    //strQry = "INSERT INTO \"TBLWORKFLOWOBJECTS\" (\"WO_ID\",\"WO_BO_ID\",\"WO_RECORD_ID\",\"WO_PREV_APPROVE_ID\",\"WO_NEXT_ROLE\",\"WO_OFFICE_CODE\",";
                    //strQry += " \"WO_CLIENT_IP\",\"WO_CR_BY\",\"WO_DESCRIPTION\",\"WO_WFO_ID\",\"WO_DATA_ID\",\"WO_REF_OFFCODE\",\"WO_STATUS\")";
                    //strQry += " VALUES (:sWFlowId1,:sBOId2,:sRecordId2,:sPrevApproveId2,";
                    //strQry += " :sRoleId3,:sOfficeCode3,:sClientIp,:sCrby4,";
                    //strQry += " :sDescription4,:sWFDataId4,:sDataReferenceId3,:sRefOfficeCode4,:status)";
                    //NpgsqlCommand = new NpgsqlCommand();
                    //NpgsqlCommand.Parameters.AddWithValue("sWFlowId1", Convert.ToInt32(sWFlowId));
                    //NpgsqlCommand.Parameters.AddWithValue("sBOId2", Convert.ToInt32(objApproval.sBOId));
                    //NpgsqlCommand.Parameters.AddWithValue("sRecordId2", Convert.ToInt32(objApproval.sRecordId));
                    //NpgsqlCommand.Parameters.AddWithValue("sPrevApproveId2", Convert.ToInt32(objApproval.sPrevApproveId));
                    //NpgsqlCommand.Parameters.AddWithValue("sRoleId3", Convert.ToInt32(objApproval.sRoleId));
                    //NpgsqlCommand.Parameters.AddWithValue("sOfficeCode3", Convert.ToInt32(objApproval.sOfficeCode));
                    //if ((objApproval.sClientIp ?? "").Length == 0)
                    //{
                    //    objApproval.sClientIp = "";
                    //}
                    //NpgsqlCommand.Parameters.AddWithValue("sClientIp", objApproval.sClientIp);
                    //NpgsqlCommand.Parameters.AddWithValue("sCrby4", Convert.ToInt32(objApproval.sCrby));
                    //NpgsqlCommand.Parameters.AddWithValue("sDescription4", objApproval.sDescription);
                    //NpgsqlCommand.Parameters.AddWithValue("sWFDataId4", objApproval.sWFDataId);
                    //if ((objApproval.sDataReferenceId ?? "").Length == 0)
                    //{
                    //    objApproval.sDataReferenceId = "";
                    //}
                    //NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId3", objApproval.sDataReferenceId);
                    //NpgsqlCommand.Parameters.AddWithValue("sRefOfficeCode4", Convert.ToInt32(objApproval.sRefOfficeCode));
                    //if ((objApproval.sStatus ?? "").Length == 0)
                    //{
                    //    objApproval.sStatus = "";
                    //}
                    //NpgsqlCommand.Parameters.AddWithValue("status", objApproval.sStatus);
                    //try
                    //{
                    //    objDatabse.ExecuteQry(strQry, NpgsqlCommand);
                    //}
                    #endregion


                    #region Converted to sp
                    string sWFlowId = string.Empty;
                    string[] Arr = new string[3];
                    NpgsqlCommand cmdinsert = new NpgsqlCommand("proc_insert_tblworkflowobjects_clsapproval");
                    cmdinsert.Parameters.AddWithValue("bo_id", Convert.ToInt32(objApproval.sBOId));
                    cmdinsert.Parameters.AddWithValue("recordid", Convert.ToInt32(objApproval.sRecordId));
                    cmdinsert.Parameters.AddWithValue("sprevapproveid", Convert.ToInt32(objApproval.sPrevApproveId));
                    cmdinsert.Parameters.AddWithValue("sroleid", Convert.ToInt32(objApproval.sRoleId));
                    cmdinsert.Parameters.AddWithValue("sofficecode", Convert.ToInt32(objApproval.sOfficeCode));
                    cmdinsert.Parameters.AddWithValue("sclientip", objApproval.sClientIp);
                    cmdinsert.Parameters.AddWithValue("scrby", Convert.ToInt32(objApproval.sCrby));
                    cmdinsert.Parameters.AddWithValue("sdescription", objApproval.sDescription);
                    cmdinsert.Parameters.AddWithValue("swfdataid", objApproval.sWFDataId);
                    cmdinsert.Parameters.AddWithValue("sdatareferenceid", objApproval.sDataReferenceId);
                    cmdinsert.Parameters.AddWithValue("srefofficecode", Convert.ToInt32(objApproval.sRefOfficeCode));
                    cmdinsert.Parameters.AddWithValue("status", (objApproval.sStatus ?? ""));
                    cmdinsert.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmdinsert.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmdinsert.Parameters.Add("pk_id", NpgsqlDbType.Text);
                    cmdinsert.Parameters["msg"].Direction = ParameterDirection.Output;
                    cmdinsert.Parameters["op_id"].Direction = ParameterDirection.Output;
                    cmdinsert.Parameters["pk_id"].Direction = ParameterDirection.Output;
                    Arr[0] = "msg";
                    Arr[1] = "op_id";
                    Arr[2] = "pk_id";
                    try
                    {
                        Arr = objDatabse.Execute(cmdinsert, Arr, 3);
                        sWFlowId = Arr[2];
                        objApproval.sWFObjectId = sWFlowId;
                    }
                    #endregion

                    catch (Exception ex)
                    {
                        System.Threading.Thread.Sleep(300);
                        NoofTimes++;
                        if (NoofTimes <= 5)
                        {
                            goto LOOP;
                        }
                        else
                        {
                            throw ex;
                        }
                    }
                    if ((objApproval.sWFInitialId ?? "").Length == 0) //Insert Initial Id for WorkFlow
                    {
                        //NpgsqlCommand = new NpgsqlCommand();
                        //strQry = "UPDATE \"TBLWORKFLOWOBJECTS\" SET \"WO_INITIAL_ID\"=:sWFlowId WHERE CAST(\"WO_ID\" AS TEXT) =:sWFlowId1";
                        //NpgsqlCommand.Parameters.AddWithValue("sWFlowId", Convert.ToInt32(sWFlowId));
                        //NpgsqlCommand.Parameters.AddWithValue("sWFlowId1", sWFlowId);
                        //objDatabse.ExecuteQry(strQry, NpgsqlCommand);

                        #region Converted to sp
                        NpgsqlCommand cmd = new NpgsqlCommand("proc_update_workflowobjects_clsapproval");
                        cmd.Parameters.AddWithValue("wfinitialid", Convert.ToInt32(sWFlowId));
                        cmd.Parameters.AddWithValue("swflowid", sWFlowId);
                        cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                        cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                        cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                        cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                        Arr[0] = "msg";
                        Arr[1] = "op_id";
                        Arr = objDatabse.Execute(cmd, Arr, 2);
                        #endregion
                    }
                    else
                    {
                        //NpgsqlCommand = new NpgsqlCommand();
                        //strQry = "UPDATE \"TBLWORKFLOWOBJECTS\" SET \"WO_INITIAL_ID\" =:sWFInitialId1 WHERE CAST(\"WO_ID\" AS TEXT) =:sWFlowId2";
                        //NpgsqlCommand.Parameters.AddWithValue("sWFInitialId1", Convert.ToInt32(objApproval.sWFInitialId));
                        //NpgsqlCommand.Parameters.AddWithValue("sWFlowId2", sWFlowId);
                        //objDatabse.ExecuteQry(strQry, NpgsqlCommand);

                        #region Converted to sp
                        NpgsqlCommand cmd = new NpgsqlCommand("proc_update_workflowobjects_clsapproval");
                        cmd.Parameters.AddWithValue("wfinitialid", Convert.ToInt32(objApproval.sWFInitialId));
                        cmd.Parameters.AddWithValue("swflowid", sWFlowId);
                        cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                        cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                        cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                        cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                        Arr[0] = "msg";
                        Arr[1] = "op_id";
                        Arr = objDatabse.Execute(cmd, Arr, 2);
                        #endregion
                    }

                    UpdateWFOAutoObject_Latest(objApproval, objDatabse);
                    SendSMStoRole_Latest(objApproval, "", objDatabse);  //Saving SMS in Table to send to Next Role
                    return true;
                }
                else
                {
                    //NpgsqlCommand = new NpgsqlCommand();
                    //strQry = "SELECT COUNT(*) FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_BOID\" =:sBOId5";
                    //NpgsqlCommand.Parameters.AddWithValue("sBOId5", Convert.ToInt32(objApproval.sBOId));
                    //string count = objDatabse.get_value(strQry, NpgsqlCommand);

                    #region Converted to sp
                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_workflow_getvalue_clsapproval");
                    cmd.Parameters.AddWithValue("p_key", "COUNT");
                    cmd.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sBOId));
                    cmd.Parameters.AddWithValue("p_offcode", "");
                    string count = objDatabse.StringGetValue(cmd);
                    #endregion

                    if (Convert.ToInt16(count) > 1 || objApproval.sBOId == "48")
                    {
                        if (objApproval.sPrevApproveId == "0" && objApproval.sRoleId == "")
                        {
                            objApproval.sWFDataId = "";
                            objApproval.sWFInitialId = "";
                        }
                    }
                    // newly added by santhosh
                    if ((objApproval.sBOId ?? "").Length == 0)
                    {
                        return false;
                    }
                    if ((objApproval.sOfficeCode ?? "").Length == 0)
                    {
                        return false;
                    }
                    // end 
                    int NoofTimes = 0;
                    LOOP:
                    #region Inline Query
                    //string sWFlowId = Convert.ToString(objDatabse.get_value(" SELECT MAX(\"WO_ID\")+1 FROM \"TBLWORKFLOWOBJECTS\""));
                    //strQry = "INSERT INTO \"TBLWORKFLOWOBJECTS\" (\"WO_ID\",\"WO_BO_ID\",\"WO_RECORD_ID\",\"WO_PREV_APPROVE_ID\",\"WO_NEXT_ROLE\",\"WO_OFFICE_CODE\",\"WO_CLIENT_IP\",";
                    //strQry += " \"WO_CR_BY\",\"WO_APPROVE_STATUS\",\"WO_DESCRIPTION\",\"WO_WFO_ID\",\"WO_DATA_ID\",\"WO_REF_OFFCODE\",\"WO_STATUS\")";
                    //strQry += " VALUES (:sWFlowId,:sBOId,:sRecordId,:sPrevApproveId,";
                    //strQry += " '0',:sOfficeCode,:sClientIp,:sCrby,";
                    //strQry += " '1',:sDescription,:sWFDataId,";
                    //if (objApproval.sDataReferenceId != null)
                    //{
                    //    strQry += ":sDataReferenceId,";
                    //}
                    //else
                    //{
                    //    strQry += "NULL,";
                    //}
                    //if (objApproval.sRefOfficeCode != null)
                    //{
                    //    strQry += ":sRefOfficeCode";
                    //}
                    //else
                    //{
                    //    strQry += "NULL";
                    //}
                    //strQry += ",:status)";
                    //NpgsqlCommand = new NpgsqlCommand();
                    //NpgsqlCommand.Parameters.AddWithValue("sWFlowId", Convert.ToInt32(sWFlowId));
                    //NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objApproval.sBOId));
                    //NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objApproval.sRecordId));
                    //NpgsqlCommand.Parameters.AddWithValue("sPrevApproveId", Convert.ToInt32(objApproval.sPrevApproveId));
                    //NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", Convert.ToInt32(objApproval.sOfficeCode));
                    //NpgsqlCommand.Parameters.AddWithValue("sClientIp", objApproval.sClientIp);
                    //NpgsqlCommand.Parameters.AddWithValue("sCrby", Convert.ToInt32(objApproval.sCrby));
                    //NpgsqlCommand.Parameters.AddWithValue("sDescription", objApproval.sDescription);
                    //if ((objApproval.sWFDataId ?? "").Length == 0)
                    //{
                    //    objApproval.sWFDataId = "";
                    //}
                    //NpgsqlCommand.Parameters.AddWithValue("sWFDataId", Convert.ToString(objApproval.sWFDataId));
                    //NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", objApproval.sDataReferenceId);
                    //NpgsqlCommand.Parameters.AddWithValue("sRefOfficeCode", Convert.ToInt32(objApproval.sRefOfficeCode));
                    //if ((objApproval.sStatus ?? "").Length == 0)
                    //{
                    //    objApproval.sStatus = "";
                    //}
                    //NpgsqlCommand.Parameters.AddWithValue("status", objApproval.sStatus);
                    //try
                    //{
                    //    objDatabse.ExecuteQry(strQry, NpgsqlCommand);
                    //}
                    #endregion

                    #region Converted to sp
                    string[] Arr = new string[3];
                    string sWFlowId = string.Empty;
                    NpgsqlCommand cmdinsert = new NpgsqlCommand("proc_insert_createlevel_workflowobjects_clsapproval");
                    cmdinsert.Parameters.AddWithValue("bo_id", Convert.ToInt32(objApproval.sBOId));
                    cmdinsert.Parameters.AddWithValue("recordid", Convert.ToInt32(objApproval.sRecordId));
                    cmdinsert.Parameters.AddWithValue("sprevapproveid", Convert.ToInt32(objApproval.sPrevApproveId));
                    cmdinsert.Parameters.AddWithValue("sofficecode", Convert.ToInt32(objApproval.sOfficeCode));
                    cmdinsert.Parameters.AddWithValue("sclientip", objApproval.sClientIp);
                    cmdinsert.Parameters.AddWithValue("scrby", Convert.ToInt32(objApproval.sCrby));
                    cmdinsert.Parameters.AddWithValue("sdescription", objApproval.sDescription);

                    if ((objApproval.sWFDataId ?? "").Length == 0)
                    {
                        objApproval.sWFDataId = "";
                    }

                    cmdinsert.Parameters.AddWithValue("swfdataid", objApproval.sWFDataId);
                    cmdinsert.Parameters.AddWithValue("sdatareferenceid", (objApproval.sDataReferenceId ?? ""));
                    cmdinsert.Parameters.AddWithValue("srefofficecode", Convert.ToInt32(objApproval.sRefOfficeCode));
                    if ((objApproval.sStatus ?? "").Length == 0)
                    {
                        objApproval.sStatus = "";
                    }
                    cmdinsert.Parameters.AddWithValue("status", (objApproval.sStatus ?? ""));
                    cmdinsert.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmdinsert.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmdinsert.Parameters.Add("pk_id", NpgsqlDbType.Text);
                    cmdinsert.Parameters["msg"].Direction = ParameterDirection.Output;
                    cmdinsert.Parameters["op_id"].Direction = ParameterDirection.Output;
                    cmdinsert.Parameters["pk_id"].Direction = ParameterDirection.Output;
                    Arr[0] = "msg";
                    Arr[1] = "op_id";
                    Arr[2] = "pk_id";
                    try
                    {
                        Arr = objDatabse.Execute(cmdinsert, Arr, 3);
                        sWFlowId = Arr[2];
                    }
                    #endregion
                    catch (Exception ex)
                    {
                        System.Threading.Thread.Sleep(300);
                        NoofTimes++;
                        if (NoofTimes <= 5)
                        {
                            goto LOOP;
                        }
                        else
                        {
                            throw ex;
                        }
                    }

                    //Insert Initial Id for WorkFlow

                    if ((objApproval.sWFInitialId ?? "").Length == 0)
                    {
                        //NpgsqlCommand = new NpgsqlCommand();
                        //strQry = "UPDATE \"TBLWORKFLOWOBJECTS\" SET \"WO_INITIAL_ID\" =:sWFlowId WHERE \"WO_ID\" =:sWFlowId1";
                        //NpgsqlCommand.Parameters.AddWithValue("sWFlowId", Convert.ToInt32(sWFlowId));
                        //NpgsqlCommand.Parameters.AddWithValue("sWFlowId1", Convert.ToInt32(sWFlowId));
                        //objDatabse.ExecuteQry(strQry, NpgsqlCommand);

                        #region Converted to sp
                        NpgsqlCommand cmdupdate = new NpgsqlCommand("proc_update_workflowobjects_clsapproval");
                        cmdupdate.Parameters.AddWithValue("wfinitialid", Convert.ToInt32(sWFlowId));
                        cmdupdate.Parameters.AddWithValue("swflowid", sWFlowId);
                        cmdupdate.Parameters.Add("msg", NpgsqlDbType.Text);
                        cmdupdate.Parameters.Add("op_id", NpgsqlDbType.Text);
                        cmdupdate.Parameters["msg"].Direction = ParameterDirection.Output;
                        cmdupdate.Parameters["op_id"].Direction = ParameterDirection.Output;
                        Arr[0] = "msg";
                        Arr[1] = "op_id";
                        Arr = objDatabse.Execute(cmdupdate, Arr, 2);
                        #endregion
                    }
                    else
                    {
                        //NpgsqlCommand = new NpgsqlCommand();
                        //strQry = "UPDATE \"TBLWORKFLOWOBJECTS\" SET \"WO_INITIAL_ID\" =:sWFInitialId WHERE \"WO_ID\" =:sWFlowId";
                        //NpgsqlCommand.Parameters.AddWithValue("sWFInitialId", Convert.ToInt32(objApproval.sWFInitialId));
                        //NpgsqlCommand.Parameters.AddWithValue("sWFlowId", Convert.ToInt32(sWFlowId));
                        //objDatabse.ExecuteQry(strQry, NpgsqlCommand);

                        #region Converted to sp
                        NpgsqlCommand cmdupdate = new NpgsqlCommand("proc_update_workflowobjects_clsapproval");
                        cmdupdate.Parameters.AddWithValue("wfinitialid", Convert.ToInt32(objApproval.sWFInitialId));
                        cmdupdate.Parameters.AddWithValue("swflowid", sWFlowId);
                        cmdupdate.Parameters.Add("msg", NpgsqlDbType.Text);
                        cmdupdate.Parameters.Add("op_id", NpgsqlDbType.Text);
                        cmdupdate.Parameters["msg"].Direction = ParameterDirection.Output;
                        cmdupdate.Parameters["op_id"].Direction = ParameterDirection.Output;
                        Arr[0] = "msg";
                        Arr[1] = "op_id";
                        Arr = objDatabse.Execute(cmdupdate, Arr, 2);
                        #endregion
                    }
                    if (objApproval.sBOId == "29")
                    {
                        objApproval.sWFDataId = "";
                    }

                    UpdateToMainTable_Latest(objApproval, objDatabse);

                    string sPrevBoId = objApproval.sBOId;

                    if (objApproval.sStoreType != null)
                    {
                        objApproval.sFailType = objApproval.sStoreType;
                    }

                    if (objApproval.sTTKStatus == "1")
                    {
                        objApproval.sFailType = "3";
                    }

                    string sResult = GetNextBOId_Latest(objApproval.sBOId, objApproval.sFailType, objApproval.sGuarentyType, objDatabse);
                    objApproval.sPrevWFOId = objApproval.sWFObjectId;
                    objApproval.sWFObjectId = sWFlowId;

                    int i = 0;
                    string[] sarray = sResult.Split('|');

                    if (sResult != "")
                    {
                        for (int x = 0; x < sarray.Length; x++)
                        {
                            sResult = sarray[x];
                            objApproval.sBOFlowMasterId = sResult.Split('~').GetValue(1).ToString();
                            objApproval.sBOId = sResult.Split('~').GetValue(0).ToString();

                            //To get Role Id with Priority Level to Create Form
                            objApproval.sRoleId = GetRoleFromApprovePriorityForBOCreate_Latest(objApproval, objDatabse);
                            UpdateWFOAutoObject_Latest(objApproval, objDatabse);
                            i++;
                            bool resrult = SaveWFObjectAuto_Latest(objApproval, objDatabse);
                            if (resrult == false)
                            {
                                return false;
                            }
                        }
                    }
                    if (i == 0)
                    {
                        UpdateWFOAutoObject_Latest(objApproval, objDatabse);
                    }
                    SendSMStoRole_Latest(objApproval, sPrevBoId, objDatabse);   //Saving SMS in Table to send to Next Role
                    return true;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                // return false;
                throw ex;
            }
        }

        /// <summary>
        /// Get Approval Priority Role From TBLWORKFLOWMASTER Table 
        /// </summary>
        /// <param name="objApproval"></param>
        /// <returns></returns>
        public string GetRoleFromApprovePriority(clsApproval objApproval)
        {
            string WM_ROLEID = string.Empty;
            DataTable Dt = new DataTable();
            try
            {
                #region Old inline query
                //string strQry = string.Empty;
                //string sExistWFObject = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "SELECT MAX(\"WO_ID\") FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" =:sBOId AND \"WO_RECORD_ID\"=:sRecordId";
                //NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objApproval.sBOId));
                //NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objApproval.sRecordId));
                //sExistWFObject = objcon.get_value(strQry, NpgsqlCommand);

                //// Reference
                ////if (objApproval.sApproveStatus != null && objApproval.sApproveStatus !="")
                ////{
                ////    if (objApproval.sApproveStatus=="3")
                ////    {
                ////        sExistWFObject = "";
                ////    }
                ////}

                //if (sExistWFObject == "")
                //{
                //    NpgsqlCommand = new NpgsqlCommand();
                //    strQry = "SELECT \"WM_ROLEID\" FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_BOID\" =:sBOId AND \"WM_LEVEL\" = (";
                //    strQry += " SELECT MIN(\"WM_LEVEL\") FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_BOID\" =:sBOId1 AND \"WM_LEVEL\" <> '1')";
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objApproval.sBOId));
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId1", Convert.ToInt32(objApproval.sBOId));
                //}
                //else if (objApproval.sApproveStatus == "3")
                //{
                //    NpgsqlCommand = new NpgsqlCommand();
                //    strQry = "SELECT \"WM_ROLEID\" FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_BOID\" =:sBOId AND \"WM_LEVEL\" = (";
                //    strQry += " SELECT MIN(\"WM_LEVEL\") FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_BOID\" =:sBOId1 AND \"WM_LEVEL\" = '1')";
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objApproval.sBOId));
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId1", Convert.ToInt32(objApproval.sBOId));
                //}
                //else
                //{
                //    NpgsqlCommand = new NpgsqlCommand();
                //    strQry = "SELECT \"WM_ROLEID\" FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_BOID\" =:sBOId AND \"WM_LEVEL\" = (";
                //    strQry += " SELECT \"WM_LEVEL\"+1 FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_BOID\" =:sBOId1 AND \"WM_ROLEID\" = ";
                //    strQry += " (SELECT \"WO_NEXT_ROLE\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\" = ";
                //    strQry += " (SELECT MAX(\"WO_ID\") FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" =:sBOId2 AND \"WO_RECORD_ID\"=:sRecordId)) LIMIT 1)";
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objApproval.sBOId));
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId1", Convert.ToInt32(objApproval.sBOId));
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId2", Convert.ToInt32(objApproval.sBOId));
                //    NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objApproval.sRecordId));
                //}
                //return objcon.get_value(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_getvalue_rolefrom_approvepriority_for_clsapproval");
                cmd.Parameters.AddWithValue("p_boid", Convert.ToString(objApproval.sBOId ?? ""));
                cmd.Parameters.AddWithValue("p_recordid", Convert.ToString(objApproval.sRecordId ?? ""));
                cmd.Parameters.AddWithValue("p_approvestatus", Convert.ToString(objApproval.sApproveStatus ?? ""));
                Dt = objcon.FetchDataTable(cmd);

                if (Dt.Rows.Count > 0)
                {
                    WM_ROLEID = Convert.ToString(Dt.Rows[0]["WMROLEID"]);
                }

                return WM_ROLEID;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        /// <summary>
        /// Get Approval Priority Role From TBLWORKFLOWMASTER Table 
        /// </summary>
        /// <param name="objApproval"></param>
        /// <returns></returns>
        public string GetRoleFromApprovePriority_Latest(clsApproval objApproval, DataBseConnection objDatabse)
        {
            string WM_ROLEID = string.Empty;
            DataTable Dt = new DataTable();
            try
            {
                #region Old inline queary
                //string strQry = string.Empty;
                //string sExistWFObject = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "SELECT MAX(\"WO_ID\") FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" =:sBOId AND \"WO_RECORD_ID\"=:sRecordId";
                //NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objApproval.sBOId));
                //NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objApproval.sRecordId));
                //sExistWFObject = objDatabse.get_value(strQry, NpgsqlCommand);

                //if (sExistWFObject == "")
                //{
                //    NpgsqlCommand = new NpgsqlCommand();
                //    strQry = "SELECT \"WM_ROLEID\" FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_BOID\" =:sBOId AND \"WM_LEVEL\" = (";
                //    strQry += " SELECT MIN(\"WM_LEVEL\") FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_BOID\" =:sBOId1 AND \"WM_LEVEL\" <> '1')";
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objApproval.sBOId));
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId1", Convert.ToInt32(objApproval.sBOId));
                //}
                //else if (objApproval.sApproveStatus == "3")
                //{
                //    NpgsqlCommand = new NpgsqlCommand();
                //    strQry = "SELECT \"WM_ROLEID\" FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_BOID\" =:sBOId AND \"WM_LEVEL\" = (";
                //    strQry += " SELECT MIN(\"WM_LEVEL\") FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_BOID\" =:sBOId1 AND \"WM_LEVEL\" = '1')";
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objApproval.sBOId));
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId1", Convert.ToInt32(objApproval.sBOId));
                //}
                //else
                //{
                //    NpgsqlCommand = new NpgsqlCommand();
                //    strQry = "SELECT \"WM_ROLEID\" FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_BOID\" =:sBOId AND \"WM_LEVEL\" = (";
                //    strQry += " SELECT \"WM_LEVEL\"+1 FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_BOID\" =:sBOId1 AND \"WM_ROLEID\" = ";
                //    strQry += " (SELECT \"WO_NEXT_ROLE\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\" = ";
                //    strQry += " (SELECT MAX(\"WO_ID\") FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" =:sBOId2 AND \"WO_RECORD_ID\"=:sRecordId)) LIMIT 1)";
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objApproval.sBOId));
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId1", Convert.ToInt32(objApproval.sBOId));
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId2", Convert.ToInt32(objApproval.sBOId));
                //    NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objApproval.sRecordId));
                //}
                //return objDatabse.get_value(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_getvalue_rolefrom_approvepriority_for_clsapproval");
                cmd.Parameters.AddWithValue("p_boid", Convert.ToString(objApproval.sBOId ?? ""));
                cmd.Parameters.AddWithValue("p_recordid", Convert.ToString(objApproval.sRecordId ?? ""));
                cmd.Parameters.AddWithValue("p_approvestatus", Convert.ToString(objApproval.sApproveStatus ?? ""));
                Dt = objDatabse.FetchDataTable(cmd);

                if (Dt.Rows.Count > 0)
                {
                    WM_ROLEID = Convert.ToString(Dt.Rows[0]["WMROLEID"]);
                }
                if(objApproval.sBOId=="1020" && objApproval.sRoleId=="2")
                {
                    string strQry = string.Empty;
                    string sExistWFObject = string.Empty;
                    NpgsqlCommand = new NpgsqlCommand();
                    strQry = "SELECT MAX(\"WO_ID\") FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" =:sBOId AND \"WO_RECORD_ID\"=:sRecordId";
                    NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objApproval.sBOId));
                    NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objApproval.sRecordId));
                    sExistWFObject = objDatabse.get_value(strQry, NpgsqlCommand);

                    if (sExistWFObject == "")
                    {
                        NpgsqlCommand = new NpgsqlCommand();
                        strQry = "SELECT \"WM_ROLEID\" FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_BOID\" =:sBOId AND \"WM_LEVEL\" = (";
                        strQry += " SELECT MIN(\"WM_LEVEL\") FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_BOID\" =:sBOId1 AND \"WM_LEVEL\" <> '1')";
                        NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objApproval.sBOId));
                        NpgsqlCommand.Parameters.AddWithValue("sBOId1", Convert.ToInt32(objApproval.sBOId));
                    }
                    else if (objApproval.sApproveStatus == "3")
                    {
                        NpgsqlCommand = new NpgsqlCommand();
                        strQry = "SELECT \"WM_ROLEID\" FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_BOID\" =:sBOId AND \"WM_LEVEL\" = (";
                        strQry += " SELECT MIN(\"WM_LEVEL\") FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_BOID\" =:sBOId1 AND \"WM_LEVEL\" = '1')";
                        NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objApproval.sBOId));
                        NpgsqlCommand.Parameters.AddWithValue("sBOId1", Convert.ToInt32(objApproval.sBOId));
                    }
                    else
                    {
                        NpgsqlCommand = new NpgsqlCommand();
                        strQry = "SELECT \"WM_ROLEID\" FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_BOID\" =:sBOId AND \"WM_LEVEL\" = (";
                        strQry += " SELECT \"WM_LEVEL\"+1 FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_BOID\" =:sBOId1 AND \"WM_ROLEID\" = ";
                        strQry += " (SELECT \"WO_NEXT_ROLE\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\" = ";
                        strQry += " (SELECT MAX(\"WO_ID\") FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" =:sBOId2 AND \"WO_RECORD_ID\"=:sRecordId)) LIMIT 1)";
                        NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objApproval.sBOId));
                        NpgsqlCommand.Parameters.AddWithValue("sBOId1", Convert.ToInt32(objApproval.sBOId));
                        NpgsqlCommand.Parameters.AddWithValue("sBOId2", Convert.ToInt32(objApproval.sBOId));
                        NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objApproval.sRecordId));
                    }
                    WM_ROLEID= objDatabse.get_value(strQry, NpgsqlCommand);
                }

                return WM_ROLEID;
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        /// <summary>
        /// Get Role Id of Form Creator from Bussiness Object Id
        /// </summary>
        /// <param name="objApproval"></param>
        /// <returns></returns>
        public string GetRoleFromApprovePriorityForBOCreate(clsApproval objApproval)
        {
            string WM_ROLEID = string.Empty;
            DataTable Dt = new DataTable();
            try
            {
                #region Old inline queary
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "SELECT \"WM_ROLEID\" FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_BOID\"=:sBOId AND \"WM_LEVEL\" = (";
                //strQry += " SELECT MIN(\"WM_LEVEL\") FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_BOID\" =:sBOId1 AND \"WM_LEVEL\" = '1')";
                //NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objApproval.sBOId));
                //NpgsqlCommand.Parameters.AddWithValue("sBOId1", Convert.ToInt32(objApproval.sBOId));
                //return objcon.get_value(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_role_from_approvepriority_for_bo_create");
                cmd.Parameters.AddWithValue("p_boid", Convert.ToString(objApproval.sBOId ?? ""));
                Dt = objcon.FetchDataTable(cmd);

                if (Dt.Rows.Count > 0)
                {
                    WM_ROLEID = Convert.ToString(Dt.Rows[0]["WMROLEID"]);
                }

                return WM_ROLEID;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        /// <summary>
        /// Get Role Id of Form Creator from Bussiness Object Id
        /// </summary>
        /// <param name="objApproval"></param>
        /// <returns></returns>
        public string GetRoleFromApprovePriorityForBOCreate_Latest(clsApproval objApproval, DataBseConnection objDatabse)
        {
            string WM_ROLEID = string.Empty;
            DataTable Dt = new DataTable();
            try
            {
                #region Old inline queary
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "SELECT \"WM_ROLEID\" FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_BOID\"=:sBOId AND \"WM_LEVEL\" = (";
                //strQry += " SELECT MIN(\"WM_LEVEL\") FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_BOID\" =:sBOId1 AND \"WM_LEVEL\" = '1')";
                //NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objApproval.sBOId));
                //NpgsqlCommand.Parameters.AddWithValue("sBOId1", Convert.ToInt32(objApproval.sBOId));
                //return objDatabse.get_value(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_role_from_approvepriority_for_bo_create");
                cmd.Parameters.AddWithValue("p_boid", Convert.ToString(objApproval.sBOId ?? ""));
                Dt = objDatabse.FetchDataTable(cmd);

                if (Dt.Rows.Count > 0)
                {
                    WM_ROLEID = Convert.ToString(Dt.Rows[0]["WMROLEID"]);
                }

                return WM_ROLEID;
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        /// <summary>
        /// Check Approval Exists for Given Form/Bussiness Object
        /// </summary>
        /// <param name="objApproval"></param>
        /// <returns></returns>
        public bool CheckFormApprovalExists(clsApproval objApproval)
        {
            string sId = string.Empty;
            string QryKey = string.Empty;
            try
            {
                #region Old inline queary
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "SELECT \"WM_BOID\" FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_BOID\" =:sBOId";
                //NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objApproval.sBOId));
                //sId = objcon.get_value(strQry, NpgsqlCommand);
                #endregion

                QryKey = "GET_FORM_APPROVAL_EXISTS";
                NpgsqlCommand cmd = new NpgsqlCommand("proc_getvalue_for_approval");
                cmd.Parameters.AddWithValue("p_key", QryKey);
                cmd.Parameters.AddWithValue("p_value_1", Convert.ToString(objApproval.sBOId ?? ""));
                cmd.Parameters.AddWithValue("p_value_2", "");
                sId = ObjBasCon.StringGetValue(cmd);

                if (sId.Length > 0)
                {
                    return true;
                }
                return false;

                //OleDbDataReader dr = objcon.Fetch(strQry);
                //if (dr.Read())
                //{
                //    dr.Close();
                //    return true;
                //}
                //dr.Close();
                //return false;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// Check Approval Exists for Given Form/Bussiness Object
        /// </summary>
        /// <param name="objApproval"></param>
        /// <returns></returns>
        public bool CheckFormApprovalExists_Latest(clsApproval objApproval, DataBseConnection objDatabse)
        {
            string sId = string.Empty;
            string QryKey = string.Empty;
            try
            {
                #region Old inline queary
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "SELECT \"WM_BOID\" FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_BOID\" =:sBOId";
                //NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objApproval.sBOId));
                //sId = objDatabse.get_value(strQry, NpgsqlCommand);
                #endregion

                QryKey = "GET_FORM_APPROVAL_EXISTS";
                NpgsqlCommand cmd = new NpgsqlCommand("proc_getvalue_for_approval");
                cmd.Parameters.AddWithValue("p_key", QryKey);
                cmd.Parameters.AddWithValue("p_value_1", Convert.ToString(objApproval.sBOId ?? ""));
                cmd.Parameters.AddWithValue("p_value_2", "");
                sId = objDatabse.StringGetValue(cmd);

                if (sId.Length > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }


        /// <summary>
        /// Approve WorkFlow Object
        /// </summary>
        /// <param name="objApproval"></param>
        /// <returns></returns>
        public bool ApproveWFRequest(clsApproval objApproval)
        {
            try
            {
                string QryKey = string.Empty;
                string strQry = string.Empty;
                DataTable dt = new DataTable();
                string sNextRole = string.Empty;
                string sApproveResult = string.Empty;
                string sResult = string.Empty;


                if ((objApproval.sWFAutoId ?? "").Length != 0)
                {
                    if (objApproval.sWFAutoId == "0")
                    {
                        #region Old inline queary
                        //NpgsqlCommand = new NpgsqlCommand();
                        //strQry = "SELECT \"WO_BO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\" =:sWFObjectId AND \"WO_APPROVE_STATUS\" <> 0";
                        //NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                        //sApproveResult = objcon.get_value(strQry, NpgsqlCommand);
                        #endregion

                        QryKey = "GET_WO_BO_ID_FOR_APPROVEWFREQUEST";
                        NpgsqlCommand cmd_WO_BO_ID = new NpgsqlCommand("proc_getvalue_for_approval");
                        cmd_WO_BO_ID.Parameters.AddWithValue("p_key", QryKey);
                        cmd_WO_BO_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(objApproval.sWFObjectId ?? ""));
                        cmd_WO_BO_ID.Parameters.AddWithValue("p_value_2", "");
                        sApproveResult = ObjBasCon.StringGetValue(cmd_WO_BO_ID);


                        if (sApproveResult != "")
                        {
                            return false;
                        }
                    }
                    else
                    {
                        #region Old inline queary
                        //NpgsqlCommand = new NpgsqlCommand();
                        //strQry = "SELECT \"WOA_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_ID\" =:sWFAutoId AND \"WOA_INITIAL_ACTION_ID\" IS NOT NULL";
                        //NpgsqlCommand.Parameters.AddWithValue("sWFAutoId", Convert.ToInt32(objApproval.sWFAutoId));
                        //sApproveResult = objcon.get_value(strQry, NpgsqlCommand);
                        #endregion

                        QryKey = "GET_WOA_ID_FOR_APPROVEWFREQUEST";
                        NpgsqlCommand cmd_WOA_ID = new NpgsqlCommand("proc_getvalue_for_approval");
                        cmd_WOA_ID.Parameters.AddWithValue("p_key", QryKey);
                        cmd_WOA_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(objApproval.sWFAutoId ?? ""));
                        cmd_WOA_ID.Parameters.AddWithValue("p_value_2", "");
                        sApproveResult = ObjBasCon.StringGetValue(cmd_WOA_ID);


                        if (sApproveResult != "")
                        {
                            return false;
                        }
                    }
                }

                #region Old inline queary
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "UPDATE \"TBLWORKFLOWOBJECTS\" SET \"WO_USER_COMMENT\" =:sApproveComments, \"WO_APPROVE_STATUS\" =:sApproveStatus,";
                //strQry += " \"WO_APPROVED_BY\" =:sCrby WHERE \"WO_ID\" =:sWFObjectId";
                //NpgsqlCommand.Parameters.AddWithValue("sApproveComments", objApproval.sApproveComments.Replace("'", "''"));
                //NpgsqlCommand.Parameters.AddWithValue("sApproveStatus", Convert.ToInt16(objApproval.sApproveStatus));
                //NpgsqlCommand.Parameters.AddWithValue("sCrby", Convert.ToInt32(objApproval.sCrby));
                //NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                //objcon.ExecuteQry(strQry, NpgsqlCommand);
                #endregion

                string[] Arr = new string[2];
                NpgsqlCommand cmd_update = new NpgsqlCommand("proc_update_tblworkflowobjects_for_approval");
                cmd_update.Parameters.AddWithValue("p_wfobjectid", Convert.ToString(objApproval.sWFObjectId ?? ""));
                cmd_update.Parameters.AddWithValue("p_approvecomments", Convert.ToString(objApproval.sApproveComments.Replace("'", "''") ?? ""));
                cmd_update.Parameters.AddWithValue("p_approvestatus", Convert.ToString(objApproval.sApproveStatus ?? ""));
                cmd_update.Parameters.AddWithValue("p_crby", Convert.ToString(objApproval.sCrby ?? ""));
                cmd_update.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd_update.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd_update.Parameters["msg"].Direction = ParameterDirection.Output;
                cmd_update.Parameters["op_id"].Direction = ParameterDirection.Output;
                Arr[0] = "msg";
                Arr[1] = "op_id";
                Arr = objcon.Execute(cmd_update, Arr, 2);


                // //objApproval.sDescription = objcon.get_value("SELECT WO_DESCRIPTION FROM TBLWORKFLOWOBJECTS WHERE WO_ID='" + objApproval.sWFObjectId + "'");

                #region Old inline queary
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "SELECT \"WO_ID\",\"WO_BO_ID\",\"WO_RECORD_ID\",\"WO_PREV_APPROVE_ID\",\"WO_OFFICE_CODE\",\"WO_CLIENT_IP\",\"WO_NEXT_ROLE\",\"WO_DESCRIPTION\",";
                //strQry += " \"WO_WFO_ID\",\"WO_INITIAL_ID\",\"WO_DATA_ID\",\"WO_REF_OFFCODE\" FROM \"TBLWORKFLOWOBJECTS\" ";
                //strQry += " WHERE \"WO_ID\" =:sWFObjectId";
                //NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                //dt = objcon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd_tblworkflowobj = new NpgsqlCommand("proc_get_tblworkflowobjects_for_approval");
                cmd_tblworkflowobj.Parameters.AddWithValue("p_wfobjectid", Convert.ToString(objApproval.sWFObjectId ?? ""));
                dt = objcon.FetchDataTable(cmd_tblworkflowobj);

                if (dt.Rows.Count > 0)
                {
                    objApproval.sBOId = Convert.ToString(dt.Rows[0]["WO_BO_ID"]);

                    if (objApproval.sRecordId == "" || objApproval.sRecordId == null)
                    {
                        objApproval.sRecordId = Convert.ToString(dt.Rows[0]["WO_RECORD_ID"]);
                    }
                    if (objApproval.sWFDataId == "" || objApproval.sWFDataId == null)
                    {
                        objApproval.sWFDataId = Convert.ToString(dt.Rows[0]["WO_WFO_ID"]);
                    }

                    objApproval.sWFInitialId = Convert.ToString(dt.Rows[0]["WO_INITIAL_ID"]);
                    objApproval.sDataReferenceId = Convert.ToString(dt.Rows[0]["WO_DATA_ID"]);

                    if (objApproval.sRefOfficeCode == "" || objApproval.sRefOfficeCode == null)
                    {
                        objApproval.sRefOfficeCode = Convert.ToString(dt.Rows[0]["WO_REF_OFFCODE"]);
                    }

                    if (objApproval.sDescription == "" || objApproval.sDescription == null)
                    {
                        objApproval.sDescription = Convert.ToString(dt.Rows[0]["WO_DESCRIPTION"]);
                    }

                    sNextRole = Convert.ToString(dt.Rows[0]["WO_NEXT_ROLE"]);
                    if (sNextRole == "0")
                    {
                        #region Old inline queary
                        //NpgsqlCommand = new NpgsqlCommand();
                        ////strQry = "SELECT BFM_NEXT_BO_ID || '~' || WOA_ROLE_ID FROM TBLWO_OBJECT_AUTO,TBLBO_FLOW_MASTER WHERE WOA_BFM_ID=BFM_ID AND WOA_PREV_APPROVE_ID='" + objApproval.sWFObjectId + "'";

                        //strQry = "SELECT \"BFM_NEXT_BO_ID\" FROM \"TBLWO_OBJECT_AUTO\",\"TBLBO_FLOW_MASTER\" WHERE \"WOA_BFM_ID\"=\"BFM_ID\" AND CAST(\"WOA_PREV_APPROVE_ID\" AS TEXT) =:sWFObjectId";
                        //NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", objApproval.sWFObjectId);
                        //sResult = objcon.get_value(strQry, NpgsqlCommand);
                        #endregion

                        QryKey = "GET_BFM_NEXT_BO_ID_FOR_APPROVEWFREQUEST";
                        NpgsqlCommand cmd_WOA_ID = new NpgsqlCommand("proc_getvalue_for_approval");
                        cmd_WOA_ID.Parameters.AddWithValue("p_key", QryKey);
                        cmd_WOA_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(objApproval.sWFObjectId ?? ""));
                        cmd_WOA_ID.Parameters.AddWithValue("p_value_2", "");
                        sResult = ObjBasCon.StringGetValue(cmd_WOA_ID);

                        objApproval.sBOId = sResult;
                        //objApproval.sRoleId = sResult.Split('~').GetValue(1).ToString();

                        objApproval.sPrevWFOId = objApproval.sWFObjectId;

                    }
                    else
                    {
                        objApproval.sPrevApproveId = Convert.ToString(dt.Rows[0]["WO_ID"]);
                    }

                }

                SaveWorkflowObjects(objApproval);

                //if (sNextRole == "0")
                //{
                //UpdateWFOAutoObject(objApproval);
                //strQry = "UPDATE TBLWO_OBJECT_AUTO SET WOA_INITIAL_ACTION_ID='" + objApproval.sWFObjectId + "' WHERE WOA_PREV_APPROVE_ID='" + objApproval.sPrevWFOId + "'";
                //objcon.Execute(strQry);
                //}
                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// Approve WorkFlow Object
        /// </summary>
        /// <param name="objApproval"></param>
        /// <returns></returns>
        public bool ApproveWFRequest_Latest(clsApproval objApproval)
        {
            DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
            try
            {
                objDatabse.BeginTransaction();

                string QryKey = string.Empty;
                string strQry = string.Empty;
                DataTable dt = new DataTable();
                string sNextRole = string.Empty;
                string sApproveResult = string.Empty;
                string sResult = string.Empty;

                if (objApproval.sWFAutoId != null)
                {
                    if (objApproval.sWFAutoId == "0")
                    {
                        #region Old inline queary
                        //NpgsqlCommand = new NpgsqlCommand();
                        //strQry = " SELECT \"WO_BO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\" =:sWFObjectId ";
                        //strQry += " AND \"WO_APPROVE_STATUS\" <> 0 ";
                        //NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                        //sApproveResult = objDatabse.get_value(strQry, NpgsqlCommand);
                        #endregion

                        QryKey = "GET_WO_BO_ID_FOR_APPROVEWFREQUEST";
                        NpgsqlCommand cmd_WO_BO_ID = new NpgsqlCommand("proc_getvalue_for_approval");
                        cmd_WO_BO_ID.Parameters.AddWithValue("p_key", QryKey);
                        cmd_WO_BO_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(objApproval.sWFObjectId ?? ""));
                        cmd_WO_BO_ID.Parameters.AddWithValue("p_value_2", "");
                        sApproveResult = objDatabse.StringGetValue(cmd_WO_BO_ID);

                        if (sApproveResult != "")
                        {
                            return false;
                        }
                    }
                    else
                    {
                        #region Old inline queary
                        //NpgsqlCommand = new NpgsqlCommand();
                        //strQry = " SELECT \"WOA_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_ID\" =:sWFAutoId ";
                        //strQry += " AND \"WOA_INITIAL_ACTION_ID\" IS NOT NULL ";
                        //NpgsqlCommand.Parameters.AddWithValue("sWFAutoId", Convert.ToInt32(objApproval.sWFAutoId));
                        //sApproveResult = objDatabse.get_value(strQry, NpgsqlCommand);
                        #endregion

                        QryKey = "GET_WOA_ID_FOR_APPROVEWFREQUEST";
                        NpgsqlCommand cmd_WOA_ID = new NpgsqlCommand("proc_getvalue_for_approval");
                        cmd_WOA_ID.Parameters.AddWithValue("p_key", QryKey);
                        cmd_WOA_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(objApproval.sWFAutoId ?? ""));
                        cmd_WOA_ID.Parameters.AddWithValue("p_value_2", "");
                        sApproveResult = objDatabse.StringGetValue(cmd_WOA_ID);

                        if (sApproveResult != "")
                        {
                            return false;
                        }
                    }
                }

                #region Old inline queary
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "UPDATE \"TBLWORKFLOWOBJECTS\" SET \"WO_USER_COMMENT\" =:sApproveComments, \"WO_APPROVE_STATUS\" =:sApproveStatus,";
                //strQry += " \"WO_APPROVED_BY\" =:sCrby WHERE \"WO_ID\" =:sWFObjectId";
                //NpgsqlCommand.Parameters.AddWithValue("sApproveComments", objApproval.sApproveComments.Replace("'", "''"));
                //NpgsqlCommand.Parameters.AddWithValue("sApproveStatus", Convert.ToInt16(objApproval.sApproveStatus));
                //NpgsqlCommand.Parameters.AddWithValue("sCrby", Convert.ToInt32(objApproval.sCrby));
                //NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                //objDatabse.ExecuteQry(strQry, NpgsqlCommand);
                #endregion

                string[] Arr = new string[2];
                NpgsqlCommand cmd_update = new NpgsqlCommand("proc_update_tblworkflowobjects_for_approval");
                cmd_update.Parameters.AddWithValue("p_wfobjectid", Convert.ToString(objApproval.sWFObjectId ?? ""));
                cmd_update.Parameters.AddWithValue("p_approvecomments", Convert.ToString(objApproval.sApproveComments.Replace("'", "''") ?? ""));
                cmd_update.Parameters.AddWithValue("p_approvestatus", Convert.ToString(objApproval.sApproveStatus ?? ""));
                cmd_update.Parameters.AddWithValue("p_crby", Convert.ToString(objApproval.sCrby ?? ""));
                cmd_update.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd_update.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd_update.Parameters["msg"].Direction = ParameterDirection.Output;
                cmd_update.Parameters["op_id"].Direction = ParameterDirection.Output;
                Arr[0] = "msg";
                Arr[1] = "op_id";
                Arr = objDatabse.Execute(cmd_update, Arr, 2);


                #region Old inline queary
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = " SELECT \"WO_ID\",\"WO_BO_ID\",\"WO_RECORD_ID\",\"WO_PREV_APPROVE_ID\",\"WO_OFFICE_CODE\",\"WO_CLIENT_IP\", ";
                //strQry += " \"WO_NEXT_ROLE\",\"WO_DESCRIPTION\", \"WO_WFO_ID\",\"WO_INITIAL_ID\",\"WO_DATA_ID\", ";
                //strQry += " \"WO_REF_OFFCODE\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\" =:sWFObjectId";
                //NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                //dt = objDatabse.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd_tblworkflowobj = new NpgsqlCommand("proc_get_tblworkflowobjects_for_approval");
                cmd_tblworkflowobj.Parameters.AddWithValue("p_wfobjectid", Convert.ToString(objApproval.sWFObjectId ?? ""));
                dt = objDatabse.FetchDataTable(cmd_tblworkflowobj);

                if (dt.Rows.Count > 0)
                {
                    objApproval.sBOId = Convert.ToString(dt.Rows[0]["WO_BO_ID"]);
                    if ((objApproval.sRecordId ?? "").Length == 0)
                    {
                        objApproval.sRecordId = Convert.ToString(dt.Rows[0]["WO_RECORD_ID"]);
                    }
                    if ((objApproval.sWFDataId ?? "").Length == 0)
                    {
                        objApproval.sWFDataId = Convert.ToString(dt.Rows[0]["WO_WFO_ID"]);
                    }
                    objApproval.sWFInitialId = Convert.ToString(dt.Rows[0]["WO_INITIAL_ID"]);
                    objApproval.sDataReferenceId = Convert.ToString(dt.Rows[0]["WO_DATA_ID"]);
                    if ((objApproval.sRefOfficeCode ?? "").Length == 0)
                    {
                        objApproval.sRefOfficeCode = Convert.ToString(dt.Rows[0]["WO_REF_OFFCODE"]);
                    }
                    if ((objApproval.sDescription ?? "").Length == 0)
                    {
                        objApproval.sDescription = Convert.ToString(dt.Rows[0]["WO_DESCRIPTION"]);
                    }
                    sNextRole = Convert.ToString(dt.Rows[0]["WO_NEXT_ROLE"]);
                    if (sNextRole == "0")
                    {
                        #region Old inline queary
                        //NpgsqlCommand = new NpgsqlCommand();
                        //strQry = " SELECT \"BFM_NEXT_BO_ID\"  FROM \"TBLWO_OBJECT_AUTO\",\"TBLBO_FLOW_MASTER\" ";
                        //strQry += " WHERE \"WOA_BFM_ID\"=\"BFM_ID\" AND CAST(\"WOA_PREV_APPROVE_ID\" AS TEXT) =:sWFObjectId ";
                        //NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", objApproval.sWFObjectId);
                        //sResult = objDatabse.get_value(strQry, NpgsqlCommand);
                        #endregion

                        QryKey = "GET_BFM_NEXT_BO_ID_FOR_APPROVEWFREQUEST";
                        NpgsqlCommand cmd_WOA_ID = new NpgsqlCommand("proc_getvalue_for_approval");
                        cmd_WOA_ID.Parameters.AddWithValue("p_key", QryKey);
                        cmd_WOA_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(objApproval.sWFObjectId ?? ""));
                        cmd_WOA_ID.Parameters.AddWithValue("p_value_2", "");
                        sResult = objDatabse.StringGetValue(cmd_WOA_ID);

                        objApproval.sBOId = sResult;

                        objApproval.sPrevWFOId = objApproval.sWFObjectId;
                    }
                    else
                    {
                        objApproval.sPrevApproveId = Convert.ToString(dt.Rows[0]["WO_ID"]);
                    }
                }
                SaveWorkflowObjects_Latest(objApproval, objDatabse);

                objDatabse.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                objDatabse.RollBackTrans();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                throw ex;
                //   return false;
            }
        }

        /// <summary>
        /// Modify and Approve Workflow Object
        /// </summary>
        /// <param name="objApproval"></param>
        /// <returns></returns>
        public bool ModifyApproveWFRequest(clsApproval objApproval)
        {
            try
            {
                DataBseConnection objDatabse = new DataBseConnection(Constants.Password);
                string[] arr = new string[0];
                string strQry = string.Empty;
                DataTable dt = new DataTable();
                string sNextRole = string.Empty;
                string sApproveResult = string.Empty;
                if (objApproval.sBOId == "73")
                {
                    sApproveResult = "";
                    objApproval.sFormName = "EstimationCreation_sdo";
                }

                else if (objApproval.sBOId == "74")
                {
                    sApproveResult = "";
                    objApproval.sFormName = "WorkOrder_sdo";
                }
                else
                {
                    //NpgsqlCommand = new NpgsqlCommand();
                    //strQry = "SELECT \"WO_BO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\" =:sWFObjectId AND \"WO_APPROVE_STATUS\" <>0";
                    //NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    //sApproveResult = objcon.get_value(strQry, NpgsqlCommand);

                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "WO_BO_ID");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    sApproveResult = objDatabse.StringGetValue(NpgsqlCommand);
                }
                if (sApproveResult != "")
                {
                    return false;
                }
                NpgsqlCommand cmd = new NpgsqlCommand("update_modifyapprovewfrequest_latest");
                cmd.Parameters.AddWithValue("sapprovecomments", objApproval.sApproveComments.Replace("'", "''"));
                cmd.Parameters.AddWithValue("sapprovestatus", Convert.ToString(objApproval.sApproveStatus));
                cmd.Parameters.AddWithValue("scrby", Convert.ToString(objApproval.sCrby));
                cmd.Parameters.AddWithValue("swfobjectid", Convert.ToString(objApproval.sWFObjectId));
                objcon.Execute(cmd, arr, 0);
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "UPDATE \"TBLWORKFLOWOBJECTS\" SET \"WO_USER_COMMENT\" =:sApproveComments, \"WO_APPROVE_STATUS\" =:sApproveStatus,";
                //strQry += " \"WO_APPROVED_BY\" =:sCrby WHERE \"WO_ID\" =:sWFObjectId";
                //NpgsqlCommand.Parameters.AddWithValue("sApproveComments", objApproval.sApproveComments.Replace("'", "''"));
                //NpgsqlCommand.Parameters.AddWithValue("sApproveStatus", Convert.ToInt16(objApproval.sApproveStatus));
                //NpgsqlCommand.Parameters.AddWithValue("sCrby", Convert.ToInt32(objApproval.sCrby));
                //NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                //objcon.ExecuteQry(strQry, NpgsqlCommand);

                //objApproval.sDescription = objcon.get_value("SELECT WO_DESCRIPTION FROM TBLWORKFLOWOBJECTS WHERE WO_ID='" + objApproval.sWFObjectId + "'");

                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "SELECT \"WO_ID\",\"WO_BO_ID\",\"WO_RECORD_ID\",\"WO_PREV_APPROVE_ID\",\"WO_OFFICE_CODE\",\"WO_CLIENT_IP\",\"WO_NEXT_ROLE\",\"WO_DESCRIPTION\",";
                //strQry += " \"WO_WFO_ID\",\"WO_INITIAL_ID\",\"WO_DATA_ID\",\"WO_REF_OFFCODE\" FROM \"TBLWORKFLOWOBJECTS\" ";
                //strQry += " WHERE \"WO_ID\" =:sWFObjectId";
                //NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                //dt = objcon.FetchDataTable(strQry, NpgsqlCommand);

                NpgsqlCommand = new NpgsqlCommand("sp_modifyapprovewfrequest_latest");
                NpgsqlCommand.Parameters.AddWithValue("woid", Convert.ToInt32(objApproval.sWFObjectId));
                dt = objcon.FetchDataTable(NpgsqlCommand);
                if (dt.Rows.Count > 0)
                {
                    objApproval.sBOId = Convert.ToString(dt.Rows[0]["WO_BO_ID"]);
                    objApproval.sRecordId = Convert.ToString(dt.Rows[0]["WO_RECORD_ID"]);
                    // objApproval.sWFDataId = Convert.ToString(dt.Rows[0]["WO_WFO_ID"]);
                    objApproval.sWFInitialId = Convert.ToString(dt.Rows[0]["WO_INITIAL_ID"]);
                    objApproval.sDataReferenceId = Convert.ToString(dt.Rows[0]["WO_DATA_ID"]);


                    if (objApproval.sRefOfficeCode == "" || objApproval.sRefOfficeCode == null)
                    {
                        objApproval.sRefOfficeCode = Convert.ToString(dt.Rows[0]["WO_REF_OFFCODE"]);
                    }

                    if (objApproval.sDescription == "" || objApproval.sDescription == null)
                    {
                        objApproval.sDescription = Convert.ToString(dt.Rows[0]["WO_DESCRIPTION"]);

                    }

                    sNextRole = Convert.ToString(dt.Rows[0]["WO_NEXT_ROLE"]);
                    if (sNextRole == "0")
                    {
                        //NpgsqlCommand = new NpgsqlCommand();
                        ////strQry = "SELECT BFM_NEXT_BO_ID || '~' || WOA_ROLE_ID FROM TBLWO_OBJECT_AUTO,TBLBO_FLOW_MASTER WHERE WOA_BFM_ID=BFM_ID AND WOA_PREV_APPROVE_ID='" + objApproval.sWFObjectId + "'";
                        //strQry = "SELECT \"BFM_NEXT_BO_ID\"  FROM \"TBLWO_OBJECT_AUTO\",\"TBLBO_FLOW_MASTER\" WHERE \"WOA_BFM_ID\"=\"BFM_ID\" AND \"WOA_PREV_APPROVE_ID\" =:sWFObjectId";
                        //NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                        //string sResult = objcon.get_value(strQry, NpgsqlCommand);

                        NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                        NpgsqlCommand.Parameters.AddWithValue("p_key", "BFM_NEXT_BO_ID");
                        NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                        string sResult = objDatabse.StringGetValue(NpgsqlCommand);
                        objApproval.sBOId = sResult;
                        //objApproval.sRoleId = sResult.Split('~').GetValue(1).ToString();

                        objApproval.sPrevWFOId = objApproval.sWFObjectId;

                    }
                    else
                    {
                        objApproval.sPrevApproveId = Convert.ToString(dt.Rows[0]["WO_ID"]);
                    }

                }

                // objApproval.sbfm_id = objcon.get_value("SELECT \"BFM_ID\" from \"TBLBO_FLOW_MASTER\" WHERE \"BFM_NEXT_BO_ID\"='" + objApproval.sBOId + "'");
                NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                NpgsqlCommand.Parameters.AddWithValue("p_key", "BFM_ID");
                NpgsqlCommand.Parameters.AddWithValue("p_value", objApproval.sBOId);
                objApproval.sbfm_id = objDatabse.StringGetValue(NpgsqlCommand);
                objApproval.sFormName = objApproval.sFormName;

                SaveWorkflowObjects(objApproval);

                UpdateWFOAutoObject(objApproval);

                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }


        /// <summary>
        /// Modify and Approve Workflow Object
        /// </summary>
        /// <param name="objApproval"></param>
        /// <returns></returns>
        public bool ModifyApproveWFRequest_Latest(clsApproval objApproval)
        {
            DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
            try
            {
                string[] arr = new string[0];
                objDatabse.BeginTransaction();
                string strQry = string.Empty;
                DataTable dt = new DataTable();
                string sNextRole = string.Empty;
                string sApproveResult = string.Empty;
                if (objApproval.sBOId == "73")
                {
                    sApproveResult = "";
                    objApproval.sFormName = "EstimationCreation_sdo";
                }

                else if (objApproval.sBOId == "74")
                {
                    sApproveResult = "";
                    objApproval.sFormName = "WorkOrder_sdo";
                }
                else
                {
                    // NpgsqlCommand = new NpgsqlCommand();
                    // strQry = "SELECT \"WO_BO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\" =:sWFObjectId AND \"WO_APPROVE_STATUS\" <>0";
                    // NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    //sApproveResult = objDatabse.get_value(strQry, NpgsqlCommand);

                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "WO_BO_ID");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    sApproveResult = objDatabse.StringGetValue(NpgsqlCommand);

                }
                if (sApproveResult != "")
                {
                    return false;
                }
                NpgsqlCommand cmd = new NpgsqlCommand("update_modifyapprovewfrequest_latest");
                cmd.Parameters.AddWithValue("sapprovecomments", objApproval.sApproveComments.Replace("'", "''"));
                cmd.Parameters.AddWithValue("sapprovestatus", Convert.ToString(objApproval.sApproveStatus));
                cmd.Parameters.AddWithValue("scrby", Convert.ToString(objApproval.sCrby));
               cmd.Parameters.AddWithValue("swfobjectid", Convert.ToString(objApproval.sWFObjectId));
                objcon.Execute(cmd, arr, 0);

                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "UPDATE \"TBLWORKFLOWOBJECTS\" SET \"WO_USER_COMMENT\" =:sApproveComments, \"WO_APPROVE_STATUS\" =:sApproveStatus,";
                //strQry += " \"WO_APPROVED_BY\" =:sCrby WHERE \"WO_ID\" =:sWFObjectId";
                //NpgsqlCommand.Parameters.AddWithValue("sApproveComments", objApproval.sApproveComments.Replace("'", "''"));
                //NpgsqlCommand.Parameters.AddWithValue("sApproveStatus", Convert.ToInt16(objApproval.sApproveStatus));
                //NpgsqlCommand.Parameters.AddWithValue("sCrby", Convert.ToInt32(objApproval.sCrby));
                //NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                //objDatabse.ExecuteQry(strQry, NpgsqlCommand);


                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "SELECT \"WO_ID\",\"WO_BO_ID\",\"WO_RECORD_ID\",\"WO_PREV_APPROVE_ID\",\"WO_OFFICE_CODE\",\"WO_CLIENT_IP\",\"WO_NEXT_ROLE\",\"WO_DESCRIPTION\",";
                //strQry += " \"WO_WFO_ID\",\"WO_INITIAL_ID\",\"WO_DATA_ID\",\"WO_REF_OFFCODE\" FROM \"TBLWORKFLOWOBJECTS\" ";
                //strQry += " WHERE \"WO_ID\" =:sWFObjectId";
                //NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                //dt = objDatabse.FetchDataTable(strQry, NpgsqlCommand);

                NpgsqlCommand = new NpgsqlCommand("sp_modifyapprovewfrequest_latest");
                NpgsqlCommand.Parameters.AddWithValue("woid", Convert.ToInt32(objApproval.sWFObjectId));
                dt = objcon.FetchDataTable(NpgsqlCommand);
                if (dt.Rows.Count > 0)
                {
                    objApproval.sBOId = Convert.ToString(dt.Rows[0]["WO_BO_ID"]);
                    objApproval.sRecordId = Convert.ToString(dt.Rows[0]["WO_RECORD_ID"]);
                    objApproval.sWFInitialId = Convert.ToString(dt.Rows[0]["WO_INITIAL_ID"]);
                    objApproval.sDataReferenceId = Convert.ToString(dt.Rows[0]["WO_DATA_ID"]);
                    if (objApproval.sRefOfficeCode == "" || objApproval.sRefOfficeCode == null)
                    {
                        objApproval.sRefOfficeCode = Convert.ToString(dt.Rows[0]["WO_REF_OFFCODE"]);
                    }
                    if (objApproval.sDescription == "" || objApproval.sDescription == null)
                    {
                        objApproval.sDescription = Convert.ToString(dt.Rows[0]["WO_DESCRIPTION"]);
                    }
                    sNextRole = Convert.ToString(dt.Rows[0]["WO_NEXT_ROLE"]);
                    if (sNextRole == "0")
                    {
                        //NpgsqlCommand = new NpgsqlCommand();
                        //strQry = "SELECT \"BFM_NEXT_BO_ID\"  FROM \"TBLWO_OBJECT_AUTO\",\"TBLBO_FLOW_MASTER\" WHERE \"WOA_BFM_ID\"=\"BFM_ID\" AND \"WOA_PREV_APPROVE_ID\" =:sWFObjectId";
                        //NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                        //string sResult = objDatabse.get_value(strQry, NpgsqlCommand);
                        NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                        NpgsqlCommand.Parameters.AddWithValue("p_key", "BFM_NEXT_BO_ID");
                        NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                        string sResult = objDatabse.StringGetValue(NpgsqlCommand);
                        objApproval.sBOId = sResult;
                        objApproval.sPrevWFOId = objApproval.sWFObjectId;
                    }
                    else
                    {
                        objApproval.sPrevApproveId = Convert.ToString(dt.Rows[0]["WO_ID"]);
                    }
                }
                //objApproval.sbfm_id = objDatabse.get_value("SELECT \"BFM_ID\" from \"TBLBO_FLOW_MASTER\" WHERE \"BFM_NEXT_BO_ID\"='" + objApproval.sBOId + "'");
                NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                NpgsqlCommand.Parameters.AddWithValue("p_key", "BFM_ID");
                NpgsqlCommand.Parameters.AddWithValue("p_value", objApproval.sBOId);
                objApproval.sbfm_id = objDatabse.StringGetValue(NpgsqlCommand);
                objApproval.sFormName = objApproval.sFormName;
                SaveWorkflowObjects_Latest(objApproval, objDatabse);
                UpdateWFOAutoObject_Latest(objApproval, objDatabse);
                objDatabse.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                objDatabse.RollBackTrans();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                throw ex;
                //  return false;
            }
        }

        //#region Approval Column Update Concept

        //#region Approval Column Update Concept

        #region 0 references
        public void UpdateApproveStatusinMainTable(clsApproval objApproval)
        {
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "SELECT \"BO_MAIN_TABLE\",\"BO_REF_COLUMN\",\"BO_REF_APPROVE\" FROM \"TBLBUSINESSOBJECT\" WHERE \"BO_ID\" =:sBOId";
                //NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objApproval.sBOId));
                //dt = objcon.FetchDataTable(strQry, NpgsqlCommand);
                NpgsqlCommand = new NpgsqlCommand("sp_approvestatusinmaintable");
                NpgsqlCommand.Parameters.AddWithValue("boid", Convert.ToInt32(objApproval.sBOId));
                dt = objcon.FetchDataTable(NpgsqlCommand);
                if (dt.Rows.Count > 0)
                {
                    objApproval.sMainTable = Convert.ToString(dt.Rows[0]["BO_MAIN_TABLE"]);
                    objApproval.sRefColumnName = Convert.ToString(dt.Rows[0]["BO_REF_COLUMN"]);
                    objApproval.sApproveColumnName = Convert.ToString(dt.Rows[0]["BO_REF_APPROVE"]);
                }

                if (objApproval.sMainTable != "")
                {
                    strQry = " UPDATE " + objApproval.sMainTable + "  SET   " + sApproveColumnName + "='1' WHERE ";
                    strQry += " " + objApproval.sRefColumnName + "='" + objApproval.sRecordId + "'";
                    objcon.ExecuteQry(strQry);
                }


            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        #endregion
        //#endregion
        //#endregion

        /// <summary>
        /// Update to Main Table by Fetching Queries from TBLWFODATA Table
        /// </summary>
        /// <param name="objApproval"></param>
        public void UpdateToMainTable(clsApproval objApproval)
        {
            //DataTable dt2 = new DataTable();
            //bool IsSuccess = false;
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();

                // objcon.BeginTrans();

                if (objApproval.sWFDataId != null)
                {
                    if (objApproval.sWFDataId.Length > 0)
                    {
                        NpgsqlCommand = new NpgsqlCommand();
                        //strQry = "SELECT \"WFO_QUERY_VALUES\",\"WFO_PARAMETER\" FROM \"TBLWFODATA\" WHERE \"WFO_ID\" =:sWFDataId ";
                        //NpgsqlCommand.Parameters.AddWithValue("sWFDataId", Convert.ToInt32(objApproval.sWFDataId));
                        //dt = objcon.FetchDataTable(strQry, NpgsqlCommand);
                        NpgsqlCommand = new NpgsqlCommand("sp_updatetomaintable");
                        NpgsqlCommand.Parameters.AddWithValue("swfdataid", Convert.ToInt32(objApproval.sWFDataId));
                        dt = objcon.FetchDataTable(NpgsqlCommand);
                        if (dt.Rows.Count > 0)
                        {
                            objApproval.sColumnNames = Convert.ToString(dt.Rows[0]["WFO_QUERY_VALUES"]);
                            objApproval.sParameterValues = Convert.ToString(dt.Rows[0]["WFO_PARAMETER"]);

                            if (objApproval.sParameterValues != "")
                            {
                                string[] sParameterQueries = objApproval.sParameterValues.Split(';');
                                string sSecondRecordId = string.Empty;

                                for (int i = 0; i < sParameterQueries.Length; i++)
                                {
                                    if (objApproval.sNewRecordId == null || objApproval.sNewRecordId == "")
                                    {
                                        if (sParameterQueries[i].ToString() != "")
                                        {
                                            objApproval.sNewRecordId = objcon.get_value(sParameterQueries[i]);
                                            objApproval.sColumnNames = objApproval.sColumnNames.Replace("{" + i + "}", objApproval.sNewRecordId);
                                        }
                                    }
                                    else
                                    {
                                        sSecondRecordId = objcon.get_value(sParameterQueries[i]);
                                        objApproval.sColumnNames = objApproval.sColumnNames.Replace("{" + i + "}", sSecondRecordId);
                                    }

                                }
                            }

                            //objApproval.sNewRecordId = objcon.get_value(objApproval.sParameterValues);
                            //objApproval.sColumnNames = objApproval.sColumnNames.Replace("{0}", objApproval.sNewRecordId);
                            string[] sQueries = objApproval.sColumnNames.Split(';');
                            for (int i = 0; i < sQueries.Length; i++)
                            {
                                sQueries[i] = sQueries[i].Replace("''", "'");

                                if (sQueries[i].ToString() != "")
                                {
                                    objcon.ExecuteQry(sQueries[i]);
                                }
                            }


                        }
                    }
                }



                // && objApproval.sBOId != "45" dont know adding
                if (objApproval.sNewRecordId != null && objApproval.sNewRecordId != "")
                {
                    UpdateWorkFlowRecordId(objApproval);
                }
                //objcon.CommitTrans();

            }
            catch (Exception ex)
            {
                //objcon.RollBack();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Update to Main Table by Fetching Queries from TBLWFODATA Table
        /// </summary>
        /// <param name="objApproval"></param>
        public void UpdateToMainTable_Latest(clsApproval objApproval, DataBseConnection objDatabse)
        {
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();
                if (objApproval.sWFDataId != null)
                {
                    if (objApproval.sWFDataId.Length > 0)
                    {
                        NpgsqlCommand = new NpgsqlCommand();
                        //strQry = "SELECT \"WFO_QUERY_VALUES\",\"WFO_PARAMETER\" FROM \"TBLWFODATA\" WHERE \"WFO_ID\" =:sWFDataId ";
                        //NpgsqlCommand.Parameters.AddWithValue("sWFDataId", Convert.ToInt32(objApproval.sWFDataId));
                        //dt = objDatabse.FetchDataTable(strQry, NpgsqlCommand);

                        NpgsqlCommand = new NpgsqlCommand("sp_updatetomaintable");
                        NpgsqlCommand.Parameters.AddWithValue("swfdataid", Convert.ToInt32(objApproval.sWFDataId));
                        dt = objDatabse.FetchDataTable(NpgsqlCommand);




                        if (dt.Rows.Count > 0)
                        {
                            objApproval.sColumnNames = Convert.ToString(dt.Rows[0]["WFO_QUERY_VALUES"]);
                            objApproval.sParameterValues = Convert.ToString(dt.Rows[0]["WFO_PARAMETER"]);
                            if (objApproval.sParameterValues != "")
                            {
                                string[] sParameterQueries = objApproval.sParameterValues.Split(';');
                                string sSecondRecordId = string.Empty;

                                for (int i = 0; i < sParameterQueries.Length; i++)
                                {
                                    if ((objApproval.sNewRecordId ?? "").Length == 0)
                                    {
                                        if (sParameterQueries[i].ToString() != "")
                                        {
                                            objApproval.sNewRecordId = objDatabse.get_value(sParameterQueries[i]);
                                            objApproval.sColumnNames = objApproval.sColumnNames.Replace("{" + i + "}", objApproval.sNewRecordId);
                                        }
                                    }
                                    else
                                    {
                                        sParameterQueries[i] = sParameterQueries[i].Replace("''", "'");
                                        sSecondRecordId = objDatabse.get_value(sParameterQueries[i]);
                                        objApproval.sColumnNames = objApproval.sColumnNames.Replace("{" + i + "}", sSecondRecordId);
                                    }
                                }
                            }
                            //objApproval.sNewRecordId = objcon.get_value(objApproval.sParameterValues);
                            //objApproval.sColumnNames = objApproval.sColumnNames.Replace("{0}", objApproval.sNewRecordId);
                            string[] sQueries = objApproval.sColumnNames.Split(';');
                            for (int i = 0; i < sQueries.Length; i++)
                            {
                                sQueries[i] = sQueries[i].Replace("''", "'");
                                if (sQueries[i].ToString() != "")
                                {
                                    objDatabse.ExecuteQry(sQueries[i]);
                                }
                            }
                        }
                    }
                }
                // && objApproval.sBOId != "45" dont know adding
                if (objApproval.sNewRecordId != null && objApproval.sNewRecordId != "")
                {
                    UpdateWorkFlowRecordId_Latest(objApproval, objDatabse);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                throw ex;
            }
        }
        /// <summary>
        /// Get Form Name Using Bussiness Object Id
        /// </summary>
        /// <param name="sBOId"></param>
        /// <returns></returns>
        public string GetFormName(string sBOId)
        {


            try
            {

                string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "SELECT \"BO_FORMNAME\" FROM \"TBLBUSINESSOBJECT\" WHERE \"BO_ID\" =:sBOId";
                //NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(sBOId));
                //return objcon.get_value(strQry, NpgsqlCommand);
                DataBseConnection objDatabse = new DataBseConnection(Constants.Password);
                NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                NpgsqlCommand.Parameters.AddWithValue("p_key", "GETFORMNAME");
                NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(sBOId));
                return objDatabse.StringGetValue(NpgsqlCommand);
            }

            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        /// <summary>
        /// Update Genrated/Actual Record Id to TBLWORKFLOWOBJECTS Table
        /// </summary>
        /// <param name="objApproval"></param>
        public void UpdateWorkFlowRecordId(clsApproval objApproval)
        {
            DataTable dt = new DataTable();

            try
            {
                string[] arr = new string[0];
                string strQry = string.Empty;
                if (objApproval.sNewRecordId == null || objApproval.sNewRecordId == "")
                {
                    return;
                }
                NpgsqlCommand = new NpgsqlCommand("sp_getworkflowrecordid");
                NpgsqlCommand.Parameters.AddWithValue("boid", objApproval.sBOId);
                NpgsqlCommand.Parameters.AddWithValue("recordid", objApproval.sRecordId);
                dt = objcon.FetchDataTable(NpgsqlCommand);
                //DataTable dt = objcon.FetchDataTable(" SELECT \"WO_BO_ID\",\"WO_RECORD_ID\",\"WO_DATA_ID\",\"WO_REF_OFFCODE\" FROM \"TBLWORKFLOWOBJECTS\"   WHERE  \"WO_BO_ID\" ='" + Convert.ToInt32(objApproval.sBOId) + "' AND \"WO_RECORD_ID\" ='" + Convert.ToInt32(objApproval.sRecordId) + "'");

                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "UPDATE \"TBLWORKFLOWOBJECTS\" SET \"WO_RECORD_ID\" =:sNewRecordId WHERE ";
                //strQry += " \"WO_BO_ID\" =:sBOId AND \"WO_RECORD_ID\" =:sRecordId";
                //NpgsqlCommand.Parameters.AddWithValue("sNewRecordId", Convert.ToInt32(objApproval.sNewRecordId));
                //NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objApproval.sBOId));
                //NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objApproval.sRecordId));
                // objcon.ExecuteQry(strQry, NpgsqlCommand);
                NpgsqlCommand = new NpgsqlCommand("sp_updateworkflowrecordid");
                NpgsqlCommand.Parameters.AddWithValue("boid", objApproval.sBOId);
                NpgsqlCommand.Parameters.AddWithValue("recordid", objApproval.sRecordId);
                NpgsqlCommand.Parameters.AddWithValue("newrecordid", objApproval.sNewRecordId);
                objcon.Execute(NpgsqlCommand, arr, 0);


                // // NpgsqlCommand.Parameters.AddWithValue("sBOId", sBOId);


                //string sFolderPath = Convert.ToString(ConfigurationSettings.AppSettings["FILEWRITELOGS"]) + DateTime.Now.ToString("yyyyMM");

                //if (!Directory.Exists(sFolderPath))
                //{
                //    Directory.CreateDirectory(sFolderPath);
                //}
                //if (Convert.ToString(ConfigurationSettings.AppSettings["filewrite"]) == "ON")
                //{

                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        string sPath = sFolderPath + "//" + "UpdateWorkFlowRecordId " + DateTime.Now.ToString("yyyyMMdd") + "-UpdateWorkFlowRecordId.txt";
                //        File.AppendAllText(sPath, Environment.NewLine + "BO_ID -- " + Convert.ToString(dt.Rows[i]["WO_BO_ID"]) + "   | RECORD_ID -- " + Convert.ToString(dt.Rows[i]["WO_RECORD_ID"]) + "  | DATA_ID -- " + Convert.ToString(dt.Rows[i]["WO_DATA_ID"]) + "| OFFICE_CODE-- " + Convert.ToString(dt.Rows[i]["WO_REF_OFFCODE"]));
                //    }
                //}


            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        /// <summary>
        /// Update Genrated/Actual Record Id to TBLWORKFLOWOBJECTS Table
        /// </summary>
        /// <param name="objApproval"></param>
        public void UpdateWorkFlowRecordId_Latest(clsApproval objApproval, DataBseConnection objDatabse)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                string[] arr = new string[2];
                if (objApproval.sNewRecordId == null || objApproval.sNewRecordId == "")
                {
                    return;
                }
                NpgsqlCommand = new NpgsqlCommand("sp_getworkflowrecordid");
                NpgsqlCommand.Parameters.AddWithValue("boid", Convert.ToString(objApproval.sBOId));
                NpgsqlCommand.Parameters.AddWithValue("recordid", Convert.ToString(objApproval.sRecordId));
                dt = objDatabse.FetchDataTable(NpgsqlCommand);
                //  DataTable dt = objDatabse.FetchDataTable(" SELECT \"WO_BO_ID\",\"WO_RECORD_ID\",\"WO_DATA_ID\",\"WO_REF_OFFCODE\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" ='" + Convert.ToInt32(objApproval.sBOId) + "' AND \"WO_RECORD_ID\" ='" + Convert.ToInt32(objApproval.sRecordId) + "'");
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "UPDATE \"TBLWORKFLOWOBJECTS\" SET \"WO_RECORD_ID\" =:sNewRecordId WHERE ";
                //strQry += " \"WO_BO_ID\" =:sBOId AND \"WO_RECORD_ID\" =:sRecordId";
                //NpgsqlCommand.Parameters.AddWithValue("sNewRecordId", Convert.ToInt32(objApproval.sNewRecordId));
                //NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objApproval.sBOId));
                //NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objApproval.sRecordId));
                //objDatabse.ExecuteQry(strQry, NpgsqlCommand);
                NpgsqlCommand cmd = new NpgsqlCommand("sp_updateworkflowrecordid");
                cmd.Parameters.AddWithValue("boid", Convert.ToString(objApproval.sBOId));
                cmd.Parameters.AddWithValue("recordid", Convert.ToString(objApproval.sRecordId));
                cmd.Parameters.AddWithValue("newrecordid", Convert.ToString(objApproval.sNewRecordId));
                cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                arr[0] = "msg";
                arr[1] = "op_id";
                arr = objDatabse.Execute(cmd, arr, 2);

            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        //#region XML Concepts

        //public string DatatableToXML(DataTable dtforXML, string sTableName)
        //{
        //    string xmlstr = string.Empty;
        //    DataTable dt = new DataTable(sTableName);
        //    try
        //    {
        //        //dt.WriteXml(sPath + "/" + sUserId + "-" +"/XMLFile.xml");
        //        dt = dtforXML;
        //        //MemoryStream str = new MemoryStream();
        //        //dt.WriteXml(str, true);
        //        //str.Seek(0, SeekOrigin.Begin);
        //        //StreamReader sr = new StreamReader(str);
        //        //xmlstr = sr.ReadToEnd();
        //        //return (xmlstr);


        //        //DataTable dtCloned = dt.Clone();
        //        //foreach (DataColumn dc in dtCloned.Columns)
        //        //    dc.DataType = typeof(string);
        //        //foreach (DataRow row in dt.Rows)
        //        //{
        //        //    dtCloned.ImportRow(row);
        //        //}

        //        //foreach (DataRow row in dtCloned.Rows)
        //        //{
        //        //    for (int i = 0; i < dtCloned.Columns.Count; i++)
        //        //    {
        //        //        dtCloned.Columns[i].ReadOnly = false;

        //        //        if (string.IsNullOrEmpty(row[i].ToString()))
        //        //            row[i] = string.Empty;
        //        //    }
        //        //}

        //        //DataSet ds = new DataSet(sTableName);
        //        //ds.Tables.Add(dtCloned);
        //        //return ds.GetXml();


        //        if (dt != null && dt.Rows.Count == 0)
        //        {
        //            foreach (DataColumn dc in dt.Columns)
        //            {
        //                dc.DataType = typeof(String);
        //            }


        //            DataRow dr = dt.NewRow();
        //            for (int i = 0; i < dr.ItemArray.Count(); i++)
        //            {
        //                dr[i] = string.Empty;
        //            }
        //            dt.Rows.Add(dr);


        //        }
        //        DataSet ds = new DataSet(sTableName);
        //        ds.Tables.Add(dt);
        //        return ds.GetXml();

        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return xmlstr;
        //    }

        //}



        //public void CreateXMLFile(clsApproval objApproval, DataTable dt)
        //{
        //    try
        //    {
        //        //If System.IO.Directory.Exists(sDataPath & "\" & "XMLData" & "\" & sNamespace & "-" & iCompanyId) = False Then
        //        //     System.IO.Directory.CreateDirectory(sDataPath & "\" & "XMLData" & "\" & iCompanyId)
        //        // End If
        //        string sDirectory = System.Web.HttpContext.Current.Server.MapPath("XMLData");
        //        string sSubDirectory = System.Web.HttpContext.Current.Server.MapPath("XMLData" + "/" + objApproval.sCrby);
        //        string sPath = System.Web.HttpContext.Current.Server.MapPath("XMLData" + "/" + objApproval.sCrby + "/" + objApproval.sMainTable);

        //        if (!Directory.Exists(sDirectory))
        //        {
        //            Directory.CreateDirectory(sDirectory);
        //        }

        //        if (!Directory.Exists(sSubDirectory))
        //        {
        //            Directory.CreateDirectory(sSubDirectory);
        //        }

        //        if (File.Exists(sPath))
        //        {
        //            File.Delete(sPath);
        //        }

        //        dt.WriteXml(sPath);

        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}

        //public void ReadXMLFile(clsApproval objApproval)
        //{
        //    try
        //    {
        //        XmlReader objXmRdr;
        //        string sPath = System.Web.HttpContext.Current.Server.MapPath("XMLData" + "/" + objApproval.sCrby + "/" + objApproval.sMainTable);
        //        if (File.Exists(sPath))
        //        {
        //            objXmRdr = XmlReader.Create(sPath);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}

        //public string InsertToWFOData()
        //{
        //    try
        //    {
        //        string strQry = string.Empty;
        //        strQry = "INSERT INTO TBLWFODATA (WFO_ID,WFO_WO_ID,WFO_DATA ";
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "InsertToWFOData");
        //        return "";
        //    }
        //}
        //#endregion


        /// <summary>
        /// Get Next Bussiness Object Id from TBLBO_FLOW_MASTER Table
        /// </summary>
        /// <param name="sBOId"></param>
        /// <returns></returns>
        public string GetNextBOId(string sBOId, string type, string sGuarentyType)
        {
            try
            {
                DataBseConnection objDatabse = new DataBseConnection(Constants.Password);
                if (type == null)
                {
                    type = "0";
                }
                else if (type == "")
                {
                    type = "2";
                }
                else if (type == "1")
                {
                    if (sGuarentyType == "WGP" || sGuarentyType == "WRGP")
                    {
                        type = "2";
                    }
                    else
                    {
                        type = "1";
                    }
                }
                else if (type == "3")
                {
                    type = "0";
                }
                if (sBOId == "10")
                {
                    type = "1";
                }
                string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "SELECT string_agg(\"BFM_NEXT_BO_ID\" || '~' || \"BFM_ID\" , '|')   FROM \"TBLBO_FLOW_MASTER\" WHERE \"BFM_BO_ID\" =:sBOId AND \"BFM_TYPE\"=:type";
                //NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(sBOId));
                //NpgsqlCommand.Parameters.AddWithValue("type", Convert.ToInt16(type));
                //return objcon.get_value(strQry, NpgsqlCommand);
                NpgsqlCommand = new NpgsqlCommand("sp_getnextboid");
                NpgsqlCommand.Parameters.AddWithValue("boid", Convert.ToInt32(sBOId));
                NpgsqlCommand.Parameters.AddWithValue("bfmtype", Convert.ToInt16(type));
                return objDatabse.StringGetValue(NpgsqlCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        /// <summary>
        /// Get Next Bussiness Object Id from TBLBO_FLOW_MASTER Table
        /// </summary>
        /// <param name="sBOId"></param>
        /// <returns></returns>
        public string GetNextBOId_Latest(string sBOId, string type, string sGuarentyType, DataBseConnection objDatabse)
        {
            try
            {
                if (type == null)
                {
                    type = "0";
                }
                else if (type == "")
                {
                    type = "2";
                }
                else if (type == "1")
                {
                    if (sGuarentyType == "WGP" || sGuarentyType == "WRGP")
                    {
                        type = "2";
                    }
                    else
                    {
                        type = "1";
                    }
                }
                else if (type == "3")
                {
                    type = "0";
                }
                if (sBOId == "10")
                {
                    type = "1";
                }
                string strQry = string.Empty;

                NpgsqlCommand = new NpgsqlCommand("sp_getnextboid");
                NpgsqlCommand.Parameters.AddWithValue("boid", Convert.ToInt32(sBOId));
                NpgsqlCommand.Parameters.AddWithValue("bfmtype", Convert.ToInt16(type));
                return objDatabse.StringGetValue(NpgsqlCommand);
                //  strQry = " SELECT string_agg(\"BFM_NEXT_BO_ID\" || '~' || \"BFM_ID\" , '|')   FROM \"TBLBO_FLOW_MASTER\" WHERE \"BFM_BO_ID\" ='" + Convert.ToInt32(sBOId) + "' AND \"BFM_TYPE\"='" + Convert.ToInt16(type) + "' ";
                //NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(sBOId));
                //NpgsqlCommand.Parameters.AddWithValue("type", Convert.ToInt16(type));

                //  return objDatabse.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }
        /// <summary>
        /// Save Initiation of Next Form for assigned Role
        /// </summary>
        /// <param name="objApproval"></param>
        /// <returns></returns>
        public bool SaveWFObjectAuto(clsApproval objApproval)
        {
            DataBseConnection objDatabse = new DataBseConnection(Constants.Password);
            try
            {
                Division = Convert.ToInt32(ConfigurationManager.AppSettings["Division_code"]);
                SubDivision = Convert.ToInt32(ConfigurationManager.AppSettings["SubDiv_code"]);
                Section = Convert.ToInt32(ConfigurationManager.AppSettings["Section_code"]);

                string strQry = string.Empty;

                string sMaxNo = Convert.ToString(objcon.Get_max_no("WOA_ID", "TBLWO_OBJECT_AUTO"));
                if (objApproval.sBOId == "25" || objApproval.sBOId == "11")
                {
                    //NpgsqlCommand = new NpgsqlCommand();
                    //NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    //string sEntype = objcon.get_value("SELECT \"DF_STATUS_FLAG\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\",\"TBLESTIMATIONDETAILS\" WHERE \"WO_ID\" =:sWFObjectId AND \"DF_ID\"=CAST(\"WO_DATA_ID\" AS BIGINT) and \"EST_FAILUREID\"=\"DF_ID\"", NpgsqlCommand);
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETDFSTATUSFLAG");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    string sEntype = objDatabse.StringGetValue(NpgsqlCommand);


                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETESTFAILTYPE");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    string sftype = objDatabse.StringGetValue(NpgsqlCommand);
                    //NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    //string sftype = objcon.get_value("SELECT \"EST_FAIL_TYPE\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\",\"TBLESTIMATIONDETAILS\" WHERE \"WO_ID\" =:sWFObjectId AND \"DF_ID\"=CAST(\"WO_DATA_ID\" AS BIGINT) and \"EST_FAILUREID\"=\"DF_ID\"", NpgsqlCommand);

                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETDFDTCCODE");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToInt32(objApproval.sWFObjectId));
                    string sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                    //NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    //string sRecordId = objcon.get_value("SELECT \"DF_DTC_CODE\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\" WHERE \"WO_ID\" =:sWFObjectId AND \"DF_ID\"=CAST(\"WO_DATA_ID\" AS BIGINT)", NpgsqlCommand);
                    if (sftype == "2" || sftype == "")
                    {
                        if (sEntype == "2")
                        {
                            objApproval.sDescription = " Enhacement of  Work Order For DTC Code " + sRecordId;
                        }
                        if (sEntype == "4")
                        {
                            objApproval.sDescription = "Repair and enhancement of Work Order For DTC Code " + sRecordId;
                        }
                        else
                        {
                            objApproval.sDescription = " Repair and replacement of  Major faulty Work Order For DTC Code " + sRecordId;
                        }
                    }
                    else
                    {
                        objApproval.sDescription = " Repair and replacement of  Minor faulty Work Order For DTC Code " + sRecordId;
                    }

                }
                else if (objApproval.sBOId == "12")
                {
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETDFSTATUSFLAGWITHTWO");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    string sEntype = objDatabse.StringGetValue(NpgsqlCommand);
                    //NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    //string sEntype = objcon.get_value("SELECT \"DF_STATUS_FLAG\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLWORKORDER\",\"TBLDTCFAILURE\",\"TBLESTIMATIONDETAILS\" WHERE \"WO_ID\" =:sWFObjectId AND  \"WO_RECORD_ID\"=\"WO_SLNO\" AND \"DF_ID\"=\"WO_DF_ID\" and \"EST_FAILUREID\"=\"DF_ID\"", NpgsqlCommand);
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETESTFAILTYPETWO");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    string sftype = objDatabse.StringGetValue(NpgsqlCommand);
                    // NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    //string sftype = objcon.get_value("SELECT \"EST_FAIL_TYPE\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLWORKORDER\",\"TBLDTCFAILURE\",\"TBLESTIMATIONDETAILS\" WHERE \"WO_ID\" =:sWFObjectId AND  \"WO_RECORD_ID\"=\"WO_SLNO\" AND \"DF_ID\"=\"WO_DF_ID\" and \"EST_FAILUREID\"=\"DF_ID\"", NpgsqlCommand);
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETDFDTCCODETWO");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    string sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                    // NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    //   string sRecordId = objcon.get_value("SELECT \"DF_DTC_CODE\" || '~' || \"WO_NO\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLWORKORDER\",\"TBLDTCFAILURE\" WHERE \"WO_ID\" =:sWFObjectId AND  \"WO_RECORD_ID\"=\"WO_SLNO\" AND \"DF_ID\"=\"WO_DF_ID\"", NpgsqlCommand);
                    if (sRecordId == "")
                    {
                        if (objApproval.sTTKStatus == "1")
                        {
                            NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                            NpgsqlCommand.Parameters.AddWithValue("p_key", "GETWONO_TTKSTATUS1");
                            NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                            sRecordId = objDatabse.StringGetValue(NpgsqlCommand);


                            //NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                            //sRecordId = objcon.get_value("SELECT  \"WO_NO\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLWORKORDER\" WHERE \"WO_ID\"=:sWFObjectId AND  \"WO_RECORD_ID\"=\"WO_SLNO\"", NpgsqlCommand);
                            objApproval.sDescription = "Indent For New DTC Commission TTK Flow WO No " + sRecordId;
                        }
                        else
                        {
                            NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                            NpgsqlCommand.Parameters.AddWithValue("p_key", "GETWONO_TTKSTATUSNOT1");
                            NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                            sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                            // NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                            // sRecordId = objcon.get_value("SELECT  \"WO_NO\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLWORKORDER\" WHERE \"WO_ID\"=:sWFObjectId AND  \"WO_RECORD_ID\"=\"WO_SLNO\"", NpgsqlCommand);
                            objApproval.sDescription = "Indent For New DTC Commission PTK with WO No " + sRecordId;
                        }
                    }
                    else
                    {
                        if (sEntype == "2")
                        {
                            objApproval.sDescription = "Enhacement of Indent For DTC Code " + sRecordId.Split('~').GetValue(0).ToString() + " and WO No " + sRecordId.Split('~').GetValue(1).ToString();

                        }
                        if (sEntype == "4")
                        {
                            objApproval.sDescription = "Repair and enhancement of  Indent For DTC Code " + sRecordId.Split('~').GetValue(0).ToString() + " and WO No " + sRecordId.Split('~').GetValue(1).ToString();
                        }

                        else
                        {
                            objApproval.sDescription = "Repair & replacement of Indent For DTC Code " + sRecordId.Split('~').GetValue(0).ToString() + " and WO No " + sRecordId.Split('~').GetValue(1).ToString();

                        }


                    }
                }
                else if (objApproval.sBOId == "13" || objApproval.sBOId == "56" || objApproval.sBOId == "57")
                {
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETDFSTATUSFLAGWITHINDENTDTTC");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    string sEntype = objDatabse.StringGetValue(NpgsqlCommand);
                    //NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    //string sEntype = objcon.get_value("SELECT \"DF_STATUS_FLAG\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLINDENT\",\"TBLWORKORDER\",\"TBLDTCFAILURE\",\"TBLDTCMAST\" WHERE \"WO_ID\" =:sWFObjectId AND  \"WO_RECORD_ID\"=\"WO_SLNO\" AND \"DF_ID\"=\"WO_DF_ID\" AND \"TI_WO_SLNO\" = \"WO_SLNO\" AND \"DT_CODE\" = \"DF_DTC_CODE\"", NpgsqlCommand);

                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETDFDTCCODEWITHTINO_WONO_DTNAME");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    string sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                    // NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    // string sRecordId = objcon.get_value("SELECT \"DF_DTC_CODE\" || '~' || \"TI_INDENT_NO\" || '~' || \"WO_NO\" || '~' || \"DT_NAME\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLINDENT\",\"TBLWORKORDER\",\"TBLDTCFAILURE\",\"TBLDTCMAST\" WHERE \"WO_ID\" =:sWFObjectId AND  \"WO_RECORD_ID\"=\"WO_SLNO\" AND \"DF_ID\"=\"WO_DF_ID\" AND \"TI_WO_SLNO\" = \"WO_SLNO\" AND \"DT_CODE\" = \"DF_DTC_CODE\"", NpgsqlCommand);
                    if (sRecordId == "")
                    {
                        NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                        NpgsqlCommand.Parameters.AddWithValue("p_key", "GETTIINDENTNO_WONO");
                        NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                        sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                        // NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                        //sRecordId = objcon.get_value("SELECT \"TI_INDENT_NO\" || '~' || \"WO_NO\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLINDENT\",\"TBLWORKORDER\" WHERE \"WO_ID\" =:sWFObjectId AND  \"WO_RECORD_ID\"=\"WO_SLNO\" AND \"TI_WO_SLNO\"=\"WO_SLNO\"", NpgsqlCommand);
                        objApproval.sDescription = "Invoice For New DTC Commission with Auto Indent No " + sRecordId.Split('~').GetValue(0).ToString() + " and WorkOrder No " + sRecordId.Split('~').GetValue(1).ToString();
                        if (objApproval.sBOId == "13")
                        {
                            objApproval.sDescription = "Invoice For New DTC Commission with Auto Indent No " + sRecordId.Split('~').GetValue(0).ToString() + " and WorkOrder No " + sRecordId.Split('~').GetValue(1).ToString() + " , DTC Code " + sdtccode;
                        }
                    }
                    else
                    {
                        if (sEntype == "2")
                        {
                            objApproval.sDescription = "Enhacement of  Invoice For DTC Code " + sRecordId.Split('~').GetValue(0).ToString() + ", DTC Name " + sRecordId.Split('~').GetValue(3).ToString() + " and Auto Indent No " + sRecordId.Split('~').GetValue(1).ToString() + " and WorkOrder No " + sRecordId.Split('~').GetValue(2).ToString();
                        }
                        if (sEntype == "4")
                        {
                            objApproval.sDescription = "Repair and enhancement  of Invoice For DTC Code " + sRecordId.Split('~').GetValue(0).ToString() + ", DTC Name " + sRecordId.Split('~').GetValue(3).ToString() + " and Auto Indent No " + sRecordId.Split('~').GetValue(1).ToString() + " and WorkOrder No " + sRecordId.Split('~').GetValue(2).ToString();
                        }
                        else
                        {
                            objApproval.sDescription = "Repair & replacement of Invoice For DTC Code " + sRecordId.Split('~').GetValue(0).ToString() + ", DTC Name " + sRecordId.Split('~').GetValue(3).ToString() + " and Auto Indent No " + sRecordId.Split('~').GetValue(1).ToString() + " and WorkOrder No " + sRecordId.Split('~').GetValue(2).ToString();

                        }
                    }
                }
                else if (objApproval.sBOId == "14")
                {
                    NpgsqlCommand = new NpgsqlCommand();
                    NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    string sRecordId = objApproval.sdtccode;

                    objApproval.sDescription = "Decommissioning For DTC Code " + sRecordId;
                    if (sRecordId == "")
                    {
                        NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                        NpgsqlCommand.Parameters.AddWithValue("p_key", "GETIN_INVNO");
                        NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                        sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                        //NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                        //sRecordId = objcon.get_value("SELECT \"IN_INV_NO\" FROM \"TBLDTCINVOICE\",\"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\" =:sWFObjectId AND \"WO_RECORD_ID\"=\"IN_NO\" ", NpgsqlCommand);
                        objApproval.sDescription = "Commissioning of DTC for the Invoice NO " + sRecordId;
                    }
                }
                else if (objApproval.sBOId == "15")
                {
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETDF_STATUSFLAGFROMTDTCF");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", objApproval.sDataReferenceId);
                    string sEntype = objDatabse.StringGetValue(NpgsqlCommand);

                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETDF_EQUIPMENTID_WO_NO_DECOM");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", objApproval.sDataReferenceId);
                    string sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                    //  string sEntype = objcon.get_value("SELECT \"DF_STATUS_FLAG\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\" = '" + objApproval.sDataReferenceId + "'");
                    // string sRecordId = objcon.get_value("SELECT \"DF_EQUIPMENT_ID\" || '~' || \"WO_NO_DECOM\" FROM \"TBLDTCFAILURE\" , \"TBLWORKORDER\" WHERE \"WO_DF_ID\" = \"DF_ID\" and \"DF_ID\" = '" + objApproval.sDataReferenceId + "'");

                    if (sEntype == "2")
                    {
                        objApproval.sDescription = "Enhacement of RI Approval For DTr Code " + sRecordId.Split('~').GetValue(0).ToString() + " and Work Order NO " + sRecordId.Split('~').GetValue(1).ToString();
                    }
                    if (sEntype == "4")
                    {
                        objApproval.sDescription = "Repair and enhancement  of RI Approval For DTr Code " + sRecordId.Split('~').GetValue(0).ToString() + " and Work Order NO " + sRecordId.Split('~').GetValue(1).ToString();
                    }
                    else
                    {
                        objApproval.sDescription = "Repair & replacement of  RI Approval For DTr Code " + sRecordId.Split('~').GetValue(0).ToString() + " and Work Order NO " + sRecordId.Split('~').GetValue(1).ToString();

                    }

                }
                else if (objApproval.sBOId == "26")
                {
                    NpgsqlCommand = new NpgsqlCommand();
                    NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));

                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETVALUEFORRECORDID");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", objApproval.sWFObjectId);
                    //string sRecordId = objcon.get_value("SELECT \"WO_DATA_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\"='" + objApproval.sWFObjectId + "'");
                    string sRecordId = objDatabse.StringGetValue(NpgsqlCommand);

                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETVALUEFORDFSTATUSFLAG");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", sRecordId);
                    string sEntype = objDatabse.StringGetValue(NpgsqlCommand);
                    //  string sEntype = objcon.get_value("SELECT \"DF_STATUS_FLAG\" FROM \"TBLDTCFAILURE\" WHERE \"DF_DTC_CODE\" ='" + sRecordId + "'");
                    if (sEntype == "2")
                    {
                        objApproval.sDescription = "Enhacement of Completion Report For DTC Code " + sRecordId;
                    }
                    if (sEntype == "4")
                    {
                        objApproval.sDescription = "Repair and enhancement  of Completion Report For DTC Code " + sRecordId;
                    }
                    else
                    {
                        objApproval.sDescription = "Repair & replacement of Completion Report For DTC Code " + sRecordId;

                    }
                }
                else if (objApproval.sBOId == "29")
                {
                    NpgsqlCommand = new NpgsqlCommand();
                    //NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));

                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETVALUEFORDFSTATUSFLAGONWOID");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", sWFObjectId);
                    string sEntype = objDatabse.StringGetValue(NpgsqlCommand);
                    //string sEntype = objcon.get_value("SELECT \"DF_STATUS_FLAG\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLINDENT\",\"TBLWORKORDER\",\"TBLDTCFAILURE\" WHERE \"WO_ID\" =:sWFObjectId AND  \"WO_RECORD_ID\"=\"TI_ID\" AND \"DF_ID\"=\"WO_DF_ID\" AND \"TI_WO_SLNO\"=\"WO_SLNO\" ", NpgsqlCommand);

                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETDFDTCCODE_TIINDENTNO_WONO");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    string sRecordId = objDatabse.StringGetValue(NpgsqlCommand);

                    //   NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));

                    //   string sRecordId = objcon.get_value("SELECT \"DF_DTC_CODE\" || '~' || \"TI_INDENT_NO\" || '~' || \"WO_NO\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLINDENT\",\"TBLWORKORDER\",\"TBLDTCFAILURE\" WHERE \"WO_ID\" =:sWFObjectId AND  \"WO_RECORD_ID\"=\"TI_ID\" AND \"DF_ID\"=\"WO_DF_ID\" AND \"TI_WO_SLNO\"=\"WO_SLNO\" ", NpgsqlCommand);
                    if (sRecordId == "")
                    {
                        NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                        NpgsqlCommand.Parameters.AddWithValue("p_key", "GETVALUETIINDENTNO_WONO");
                        NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                        sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                        // NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                        //  sRecordId = objcon.get_value("SELECT \"TI_INDENT_NO\" || '~' || \"WO_NO\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLINDENT\",\"TBLWORKORDER\" WHERE \"WO_ID\" =:sWFObjectId AND  \"WO_RECORD_ID\"=\"TI_ID\"  AND \"TI_WO_SLNO\"=\"WO_SLNO\"", NpgsqlCommand);
                        objApproval.sDescription = "Invoice For New DTC Commission with  Indent No " + sRecordId.Split('~').GetValue(0).ToString() + " and WorkOrder No " + sRecordId.Split('~').GetValue(1).ToString();
                    }
                    else
                    {
                        if (sEntype == "2")
                        {
                            objApproval.sDescription = "Enhacement of Invoice For DTC Code " + sRecordId.Split('~').GetValue(0).ToString() + " with WorkOrder No " + sRecordId.Split('~').GetValue(2).ToString() + " and Indent No " + sRecordId.Split('~').GetValue(1).ToString();
                        }
                        if (sEntype == "4")
                        {
                            objApproval.sDescription = "Repair and enhancement  of Invoice For DTC Code " + sRecordId.Split('~').GetValue(0).ToString() + " with WorkOrder No " + sRecordId.Split('~').GetValue(2).ToString() + " and Indent No " + sRecordId.Split('~').GetValue(1).ToString();
                        }
                        else
                        {
                            objApproval.sDescription = "Repair & replacement of  Invoice For DTC Code " + sRecordId.Split('~').GetValue(0).ToString() + " with WorkOrder No " + sRecordId.Split('~').GetValue(2).ToString() + " and Indent No " + sRecordId.Split('~').GetValue(1).ToString();

                        }
                    }

                }

                else if (objApproval.sBOId == "24")
                {
                    string sOfficeCode = string.Empty;
                    if (Convert.ToInt32(objApproval.sRefOfficeCode) > 1)
                    {
                        sOfficeCode = objApproval.sRefOfficeCode.Substring(0, Division);
                    }
                    else
                    {
                        sOfficeCode = objApproval.sRefOfficeCode;
                    }
                    //    NpgsqlCommand = new NpgsqlCommand();
                    // NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", Convert.ToInt32(sOfficeCode));
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETSTORENAME");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", sOfficeCode);
                    string sStoreName = objDatabse.StringGetValue(NpgsqlCommand);
                    // string sStoreName = objcon.get_value("SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\",\"TBLSTOREOFFCODE\" WHERE \"SM_ID\"=\"STO_SM_ID\" AND \"STO_OFF_CODE\" =:sOfficeCode", NpgsqlCommand);

                    //  NpgsqlCommand.Parameters.AddWithValue("si_id", Convert.ToInt32(objApproval.sNewRecordId));
                    //  string indentno = objcon.get_value(" select \"SI_NO\"  from \"TBLSTOREINDENT\"   WHERE \"SI_ID\"  =:si_id", NpgsqlCommand);
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETINDENTNO");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sNewRecordId));
                    string indentno = objDatabse.StringGetValue(NpgsqlCommand);

                    // NpgsqlCommand.Parameters.AddWithValue("store_id", Convert.ToInt32(objApproval.sNewRecordId));
                    // string storeid = objcon.get_value(" select \"SI_FROM_STORE\"  from \"TBLSTOREINDENT\"   WHERE \"SI_ID\"  =:store_id", NpgsqlCommand);
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETSTOREID");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sNewRecordId));
                    string storeid = objDatabse.StringGetValue(NpgsqlCommand);



                    //  string sfromStoreName = objcon.get_value("SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\"  WHERE \"SM_ID\" =:storeid", NpgsqlCommand);
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETFROMSTORE");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", storeid);
                    string sfromStoreName = objDatabse.StringGetValue(NpgsqlCommand);

                    objApproval.sDescription = "Inter Store Indent no " + indentno + " Request for Specified Capacity Transformer From Store Name " + sfromStoreName;
                }
                else if (objApproval.sBOId == "32")
                {
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETRESULT");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sNewRecordId));
                    string sResult = objDatabse.StringGetValue(NpgsqlCommand);
                    //NpgsqlCommand.Parameters.AddWithValue("sNewRecordId", Convert.ToInt32(objApproval.sNewRecordId));
                    //string sResult = objcon.get_value("SELECT \"SI_NO\" ||'~' || \"IS_NO\" FROM \"TBLSTOREINDENT\",\"TBLSTOREINVOICE\" WHERE \"IS_ID\" =:sNewRecordId AND \"SI_ID\"=\"IS_SI_ID\"", NpgsqlCommand);

                    objApproval.sDescription = "Response for Store Indent No " + sResult.Split('~').GetValue(0).ToString() + " with Store Invoice Number " + sResult.Split('~').GetValue(1).ToString();
                }
                else if (objApproval.sBOId == "46")
                {
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETWONO");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                    // NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    //  sRecordId = objcon.get_value("SELECT  \"WO_NO\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLWORKORDER\" WHERE \"WO_ID\"=:sWFObjectId AND  \"WO_RECORD_ID\"=\"WO_SLNO\"", NpgsqlCommand);
                    objApproval.sDescription = " MinorFailure WO No " + sRecordId;
                }

                else if (objApproval.sBOId == "47" || objApproval.sBOId == "48")
                {
                    if (objApproval.sNewRecordId == null)
                    {
                        NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                        NpgsqlCommand.Parameters.AddWithValue("p_key", "GETRECORDID");
                        NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sRecordId));
                        sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                        // NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objApproval.sRecordId));
                        // sRecordId = objcon.get_value("SELECT \"DF_DTC_CODE\" FROM \"TBLRECEIVEDTR\",\"TBLWORKORDER\",\"TBLDTCFAILURE\" WHERE \"RD_WO_SLNO\"=\"WO_SLNO\" AND \"WO_DF_ID\"=\"DF_ID\" AND \"RD_ID\"=:sRecordId", NpgsqlCommand);
                    }
                    else
                    {
                        NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                        NpgsqlCommand.Parameters.AddWithValue("p_key", "GETNEWRECORDID");
                        NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sNewRecordId));
                        sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                        //NpgsqlCommand.Parameters.AddWithValue("sNewRecordId", Convert.ToInt32(objApproval.sNewRecordId));
                        //sRecordId = objcon.get_value("SELECT \"DF_DTC_CODE\" FROM \"TBLRECEIVEDTR\",\"TBLWORKORDER\",\"TBLDTCFAILURE\" WHERE \"RD_WO_SLNO\"=\"WO_SLNO\" AND \"WO_DF_ID\"=\"DF_ID\" AND \"RD_ID\"=:sNewRecordId", NpgsqlCommand);
                    }

                    objApproval.sDescription = "Commissioning of Minor Coil Failure DTC code " + sRecordId;

                }


                else if (objApproval.sBOId == "62")
                {
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETPESTDTCCODE");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                    //  NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    //   sRecordId = objcon.get_value("SELECT distinct \"PEST_DTC_CODE\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLPERMANENTESTIMATIONDETAILS\" WHERE \"WO_ID\" =:sWFObjectId", NpgsqlCommand);
                    objApproval.sDescription = "PermanentEstimation Request for DTC CODE  " + sRecordId;
                }
                else if (objApproval.sBOId == "63")
                {
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETPESTDTCCODE");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                    //NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    //sRecordId = objcon.get_value("SELECT distinct \"PEST_DTC_CODE\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLPERMANENTESTIMATIONDETAILS\" WHERE \"WO_ID\" =:sWFObjectId", NpgsqlCommand);
                    objApproval.sDescription = "PermanentWorkOrder Request for DTC CODE " + sDataReferenceId;
                }


                else if (objApproval.sBOId == "64")
                {
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETPESTDTCCODE");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sDataReferenceId));
                    sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                    //   NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", Convert.ToInt32(objApproval.sDataReferenceId));
                    //   sRecordId = objcon.get_value("SELECT distinct \"PEST_DTC_CODE\" FROM \"TBLPERMANENTESTIMATIONDETAILS\" WHERE \"PEST_ID\" =:sDataReferenceId", NpgsqlCommand);
                    objApproval.sDescription = "PermanentIndent Request for DTC CODE " + sRecordId;
                }
                else if (objApproval.sBOId == "65")
                {
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETPESTDTCCODEONPWO_SLNO");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sDataReferenceId));
                    sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                    // NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", Convert.ToInt32(objApproval.sDataReferenceId));
                    //  sRecordId = objcon.get_value("SELECT  \"PEST_DTC_CODE\" FROM \"TBLPERMANENTESTIMATIONDETAILS\",\"TBLPERMANENTWORKORDER\" WHERE \"PWO_SLNO\"=:sDataReferenceId and \"PEST_ID\"=\"PWO_PEF_ID\"", NpgsqlCommand);
                    objApproval.sDescription = "PermanentDecomm Request for DTC CODE " + sRecordId;
                }

                else if (objApproval.sBOId == "66")
                {
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETPESTDTCCODEONPESTID_PWOSLNO");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                    // NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    // sRecordId = objcon.get_value("SELECT distinct \"PEST_DTC_CODE\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLPERMANENTESTIMATIONDETAILS\",\"TBLPERMANENTWORKORDER\"   WHERE  \"PEST_ID\"=\"PWO_PEF_ID\" and \"PWO_SLNO\" =:sWFObjectId", NpgsqlCommand);
                    objApproval.sDescription = "PermanentRI Request for DTC CODE " + sDataReferenceId;
                }
                else if (objApproval.sBOId == "67")
                {
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETPESTDTCCODEONWOID");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                    //  NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    // sRecordId = objcon.get_value("SELECT  distinct \"PEST_DTC_CODE\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLPERMANENTESTIMATIONDETAILS\" WHERE \"WO_ID\" =:sWFObjectId", NpgsqlCommand);
                    objApproval.sDescription = "PermanentCR Request for DTC CODE " + sDataReferenceId;
                }
                else if (objApproval.sBOId == "58")
                {
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETDTCODE");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    string sResult = objDatabse.StringGetValue(NpgsqlCommand);
                    // NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    // string sResult = objcon.get_value("SELECT \"DT_CODE\" || '~' || \"TI_INDENT_NO\" || '~' || \"WO_NO\" || '~' || \"DT_NAME\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCINVOICE\",\"TBLINDENT\",\"TBLWORKORDER\",\"TBLDTCMAST\" WHERE \"DT_ID\" = \"WO_RECORD_ID\" AND \"WO_DATA_ID\" = CAST(\"IN_NO\" AS TEXT) AND \"WO_ID\" =:sWFObjectId AND \"TI_ID\" = \"IN_TI_NO\" AND \"TI_WO_SLNO\" = \"WO_SLNO\"", NpgsqlCommand);
                    objApproval.sDescription = "New DTC CR Request for DTC CODE " + sResult.Split('~').GetValue(0).ToString() + " with WO NO " + sResult.Split('~').GetValue(2).ToString();
                }

                else if (objApproval.sBOId == "76")
                {

                    ///rudresh 22-04-2020
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETRWOA_NO");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    string sResult = objDatabse.StringGetValue(NpgsqlCommand);
                    // NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    //  string sResult = objcon.get_value("SELECT \"RWOA_NO\" from \"TBLREPAIRERWORKAWARD\"  WHERE \"RWAO_ID\"=:sWFObjectId", NpgsqlCommand);
                    objApproval.sDescription = "TransilOil Replacement " + sResult.ToString() + " ";



                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETWOREFOFFCODE");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    string sResult1 = objDatabse.StringGetValue(NpgsqlCommand);
                    //string sResult1 = objcon.get_value("select \"WO_REF_OFFCODE\" from \"TBLWORKFLOWOBJECTS\" where \"WO_ID\"=:sWFObjectId", NpgsqlCommand);
                    if (sResult1 != null && sResult1 != "")
                    {
                        objApproval.sOfficeCode = sResult1;
                    }


                }
                //string[] Arr = new string[0];
                //NpgsqlCommand = new NpgsqlCommand("proc_savewfobjectauto");

                //// strQry = "INSERT INTO \"TBLWO_OBJECT_AUTO\" (\"WOA_ID\",\"WOA_BFM_ID\",\"WOA_PREV_APPROVE_ID\",\"WOA_ROLE_ID\",\"WOA_OFFICE_CODE\",";
                //// strQry += "\"WOA_CRBY\",\"WOA_DESCRIPTION\",\"WOA_REF_OFFCODE\",\"WOA_STATUS\") VALUES (:sMaxNo,:sBOFlowMasterId,:sWFObjectId,";
                ////  strQry += " :sRoleId,:sOfficeCode,:sCrby,:sDescription,:sRefOfficeCode,:status)";
                //NpgsqlCommand.Parameters.AddWithValue("smaxno", Convert.ToInt32(sMaxNo));
                //NpgsqlCommand.Parameters.AddWithValue("sboflowmasterid", Convert.ToInt32(objApproval.sBOFlowMasterId));
                //NpgsqlCommand.Parameters.AddWithValue("swfobjectid", Convert.ToInt32(objApproval.sWFObjectId));
                //NpgsqlCommand.Parameters.AddWithValue("sroleid", Convert.ToInt32(objApproval.sRoleId));
                //NpgsqlCommand.Parameters.AddWithValue("srefofficecode", Convert.ToInt32(objApproval.sOfficeCode));
                //NpgsqlCommand.Parameters.AddWithValue("scrby", Convert.ToInt32(objApproval.sCrby));
                //NpgsqlCommand.Parameters.AddWithValue("sdescription", objApproval.sDescription);
                //NpgsqlCommand.Parameters.AddWithValue("srefofficecode", Convert.ToInt32(objApproval.sRefOfficeCode));
                //NpgsqlCommand.Parameters.Add("op_id", NpgsqlDbType.Text);
                //NpgsqlCommand.Parameters["op_id"].Direction = ParameterDirection.Output;
                //Arr[0] = "op_id";
                //if (objApproval.sStatus == null || objApproval.sStatus == "")
                //{
                //    objApproval.sStatus = "";
                //}
                //NpgsqlCommand.Parameters.AddWithValue("status", objApproval.sStatus);
                //objcon.Execute(NpgsqlCommand, Arr, 0);



                string[] Arr = new string[3];

                NpgsqlCommand cmd = new NpgsqlCommand("proc_save_wfobject_auto_latest");
                cmd.Parameters.AddWithValue("bo_flow_master_id", (objApproval.sBOFlowMasterId ?? ""));
                cmd.Parameters.AddWithValue("wfo_object_id", (objApproval.sWFObjectId ?? ""));
                cmd.Parameters.AddWithValue("role_id", (objApproval.sRoleId ?? ""));
                cmd.Parameters.AddWithValue("off_code", (objApproval.sOfficeCode ?? ""));
                cmd.Parameters.AddWithValue("crby", (objApproval.sCrby ?? ""));
                cmd.Parameters.AddWithValue("desc", (objApproval.sDescription ?? ""));
                cmd.Parameters.AddWithValue("ref_ofc_code", (objApproval.sRefOfficeCode ?? ""));
                cmd.Parameters.AddWithValue("status", (objApproval.sStatus ?? ""));

                cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd.Parameters.Add("pk_id", NpgsqlDbType.Text);
                cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                cmd.Parameters["pk_id"].Direction = ParameterDirection.Output;
                Arr[0] = "msg";
                Arr[1] = "op_id";
                Arr[2] = "pk_id";
                Arr = objcon.Execute(cmd, Arr, 3);
                sMaxNo = Arr[2].ToString();





                #region WCF Methods
                //if (objApproval.sBOId == "29")
                //{
                //    bool isSuccess;
                //    strQry = string.Empty;
                //    DtmmsWebService.Service1Client objWcf = new DtmmsWebService.Service1Client();

                //    ///////////Update TBLWO_OBJECT_AUTO(WOA_FLAG) so (DTLMS) STO will not Get the PseudoIndent Record To Approve////////////
                //    strQry = "UPDATE TBLWO_OBJECT_AUTO SET WOA_FLAG='1' WHERE WOA_ID='" + sMaxNo + "'";
                //    objcon.Execute(strQry);
                //    ///////////////////////////Update TBLWO_OBJECT_AUTO////////////////////////////////////


                //    strQry = " SELECT * FROM TBLWORKORDER,TBLDTCFAILURE,TBLTCMASTER,TBLINDENT,TBLESTIMATION, ";
                //    strQry += " TBLWORKFLOWOBJECTS,TBLWO_OBJECT_AUTO  WHERE DF_EQUIPMENT_ID=TC_CODE AND WO_DF_ID=DF_ID AND WO_SLNO=TI_WO_SLNO AND EST_DF_ID=DF_ID AND ";
                //    strQry += " WO_ID=WOA_PREV_APPROVE_ID AND TI_ID=WO_RECORD_ID AND WOA_PREV_APPROVE_ID='" + objApproval.sWFObjectId + "'";

                //    DataTable dtIndentDetails = new DataTable("TableIndentDetails");
                //    dtIndentDetails = objcon.getDataTable(strQry);
                //    isSuccess = objWcf.SaveIndentDetails(dtIndentDetails);
                //    //if (isSuccess)
                //    //objcon.CommitTrans();
                //    //else
                //    //objcon.RollBack(); 
                //}

                //if (objApproval.sBOId == "15")
                //{
                //    DataTable dt2 = new DataTable();
                //    DataTable dt = new DataTable();
                //    bool IsSuccess = false;
                //    strQry = " SELECT TR_IN_NO FROM TBLTCREPLACE WHERE TR_ID =(SELECT MAX(TR_ID) FROM TBLTCREPLACE )";
                //    string res = objcon.get_value(strQry);

                //    strQry = "SELECT TR_ID,WO_SLNO,IN_DATE,TR_RI_DATE,TR_CRBY,TR_DESC,TR_STORE_SLNO,TR_OIL_QUNTY,DF_STATUS_FLAG FROM TBLWORKORDER,TBLINDENT,TBLDTCINVOICE,";
                //    strQry += "TBLTCREPLACE,TBLDTCFAILURE WHERE TR_IN_NO=IN_NO AND IN_TI_NO=TI_ID AND TI_WO_SLNO=WO_SLNO AND WO_DF_ID=DF_ID AND TR_IN_NO='" + res + "' ";
                //    dt = objcon.getDataTable(strQry);

                //    strQry = "SELECT * FROM TBLWORKFLOWOBJECTS,TBLWO_OBJECT_AUTO WHERE WO_ID=(SELECT max(WO_ID) FROM TBLWORKFLOWOBJECTS WHERE WO_RECORD_ID='" + res + "') AND WO_ID=WOA_PREV_APPROVE_ID";
                //    dt2 = objcon.getDataTable(strQry);

                //    DtmmsWebService.Service1Client objWcf = new DtmmsWebService.Service1Client();
                //    DataTable RiDetails = new DataTable("Ridetails");

                //    RiDetails.Columns.Add("FORMNAME", typeof(string));

                //    #region RI Details
                //    RiDetails.Columns.Add("TR_ID", typeof(string));
                //    RiDetails.Columns.Add("WO_SLNO", typeof(string));
                //    RiDetails.Columns.Add("TR_IN_NO", typeof(string));
                //    RiDetails.Columns.Add("IN_DATE", typeof(string));
                //    RiDetails.Columns.Add("TR_RI_DATE", typeof(string));
                //    RiDetails.Columns.Add("TR_CRBY", typeof(string));
                //    RiDetails.Columns.Add("TR_DESC", typeof(string));
                //    RiDetails.Columns.Add("TR_STORE_SLNO", typeof(string));
                //    RiDetails.Columns.Add("TR_OIL_QUNTY", typeof(string));
                //    RiDetails.Columns.Add("DF_STATUS_FLAG", typeof(string));

                //    #endregion

                //    #region TBLWORKFLOWOBJECTS details

                //    RiDetails.Columns.Add("WO_BO_ID", typeof(string));
                //    RiDetails.Columns.Add("WO_RECORD_ID", typeof(string));
                //    RiDetails.Columns.Add("WO_PREV_APPROVE_ID", typeof(string));
                //    RiDetails.Columns.Add("WO_NEXT_ROLE", typeof(string));
                //    RiDetails.Columns.Add("WO_OFFICE_CODE", typeof(string));
                //    RiDetails.Columns.Add("WO_USER_COMMENT", typeof(string));
                //    RiDetails.Columns.Add("WO_APPROVED_BY", typeof(string));
                //    RiDetails.Columns.Add("WO_APPROVE_STATUS", typeof(string));
                //    RiDetails.Columns.Add("WO_CR_BY", typeof(string));
                //    RiDetails.Columns.Add("WO_CR_ON", typeof(string));
                //    RiDetails.Columns.Add("WO_RECORD_BY", typeof(string));
                //    RiDetails.Columns.Add("WO_DEVICE_ID", typeof(string));
                //    RiDetails.Columns.Add("WO_DESCRIPTION", typeof(string));
                //    RiDetails.Columns.Add("WO_WFO_ID", typeof(string));
                //    RiDetails.Columns.Add("WO_INITIAL_ID", typeof(string));
                //    RiDetails.Columns.Add("WO_DATA_ID", typeof(string));
                //    RiDetails.Columns.Add("WO_REF_OFFCODE", typeof(string));

                //    #endregion

                //    #region TBLWO_OBJECT_AUTO details

                //    RiDetails.Columns.Add("WOA_ID", typeof(string));
                //    RiDetails.Columns.Add("WOA_BFM_ID", typeof(string));
                //    RiDetails.Columns.Add("WOA_PREV_APPROVE_ID", typeof(string));
                //    RiDetails.Columns.Add("WOA_ROLE_ID", typeof(string));
                //    RiDetails.Columns.Add("WOA_OFFICE_CODE", typeof(string));
                //    RiDetails.Columns.Add("WOA_INITIAL_ACTION_ID", typeof(string));
                //    RiDetails.Columns.Add("WOA_DESCRIPTION", typeof(string));
                //    RiDetails.Columns.Add("WOA_CRBY", typeof(string));
                //    RiDetails.Columns.Add("WOA_CRON", typeof(string));
                //    RiDetails.Columns.Add("WOA_REF_OFFCODE", typeof(string));

                //    #endregion

                //    DataRow dtrow = RiDetails.NewRow();
                //    dtrow["FORMNAME"] = "ReturnInvoice";

                //    dtrow["TR_ID"] = dt.Rows[0]["TR_ID"].ToString();
                //    dtrow["WO_SLNO"] = dt.Rows[0]["WO_SLNO"].ToString();
                //    dtrow["TR_IN_NO"] = res;
                //    dtrow["IN_DATE"] = dt.Rows[0]["IN_DATE"].ToString();
                //    dtrow["TR_RI_DATE"] = dt.Rows[0]["TR_RI_DATE"].ToString();
                //    dtrow["TR_CRBY"] = dt.Rows[0]["TR_CRBY"].ToString();
                //    dtrow["TR_DESC"] = dt.Rows[0]["TR_DESC"].ToString();
                //    dtrow["TR_STORE_SLNO"] = dt.Rows[0]["TR_STORE_SLNO"].ToString();
                //    dtrow["TR_OIL_QUNTY"] = dt.Rows[0]["TR_OIL_QUNTY"].ToString();
                //    dtrow["DF_STATUS_FLAG"] = dt.Rows[0]["DF_STATUS_FLAG"].ToString();

                //    dtrow["WO_BO_ID"] = dt2.Rows[0]["WO_BO_ID"].ToString();
                //    dtrow["WO_RECORD_ID"] = dt2.Rows[0]["WO_RECORD_ID"].ToString();
                //    dtrow["WO_PREV_APPROVE_ID"] = dt2.Rows[0]["WO_PREV_APPROVE_ID"].ToString();
                //    dtrow["WO_NEXT_ROLE"] = dt2.Rows[0]["WO_NEXT_ROLE"].ToString();
                //    dtrow["WO_OFFICE_CODE"] = dt2.Rows[0]["WO_OFFICE_CODE"].ToString();
                //    dtrow["WO_CR_ON"] = dt2.Rows[0]["WO_CR_ON"].ToString();
                //    dtrow["WO_USER_COMMENT"] = dt2.Rows[0]["WO_USER_COMMENT"].ToString();
                //    dtrow["WO_APPROVED_BY"] = dt2.Rows[0]["WO_APPROVED_BY"].ToString();
                //    dtrow["WO_APPROVE_STATUS"] = dt2.Rows[0]["WO_APPROVE_STATUS"].ToString();
                //    dtrow["WO_CR_BY"] = dt2.Rows[0]["WO_CR_BY"].ToString();
                //    dtrow["WO_RECORD_BY"] = dt2.Rows[0]["WO_RECORD_BY"].ToString();
                //    dtrow["WO_DEVICE_ID"] = dt2.Rows[0]["WO_DEVICE_ID"].ToString();
                //    dtrow["WO_DESCRIPTION"] = dt2.Rows[0]["WO_DESCRIPTION"].ToString();
                //    dtrow["WO_WFO_ID"] = dt2.Rows[0]["WO_WFO_ID"].ToString();
                //    dtrow["WO_INITIAL_ID"] = dt2.Rows[0]["WO_INITIAL_ID"].ToString();
                //    dtrow["WO_DATA_ID"] = dt2.Rows[0]["WO_DATA_ID"].ToString();
                //    dtrow["WO_REF_OFFCODE"] = dt2.Rows[0]["WO_REF_OFFCODE"].ToString();

                //    dtrow["WOA_ID"] = dt2.Rows[0]["WOA_ID"].ToString();
                //    dtrow["WOA_BFM_ID"] = dt2.Rows[0]["WOA_BFM_ID"].ToString();
                //    dtrow["WOA_PREV_APPROVE_ID"] = dt2.Rows[0]["WOA_PREV_APPROVE_ID"].ToString();
                //    dtrow["WOA_ROLE_ID"] = dt2.Rows[0]["WOA_ROLE_ID"].ToString();
                //    dtrow["WOA_OFFICE_CODE"] = dt2.Rows[0]["WOA_OFFICE_CODE"].ToString();
                //    dtrow["WOA_INITIAL_ACTION_ID"] = dt2.Rows[0]["WOA_INITIAL_ACTION_ID"].ToString();
                //    dtrow["WOA_DESCRIPTION"] = dt2.Rows[0]["WOA_DESCRIPTION"].ToString();
                //    dtrow["WOA_CRBY"] = dt2.Rows[0]["WOA_CRBY"].ToString();
                //    dtrow["WOA_CRON"] = dt2.Rows[0]["WOA_CRON"].ToString();
                //    dtrow["WOA_REF_OFFCODE"] = dt2.Rows[0]["WOA_REF_OFFCODE"].ToString();


                //    RiDetails.Rows.Add(dtrow);
                //    IsSuccess = objWcf.SaveRVData(RiDetails);
                //}
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }


        /// <summary>
        /// Save Initiation of Next Form for assigned Role
        /// </summary>
        /// <param name="objApproval"></param>
        /// <returns></returns>
        public bool SaveWFObjectAuto_Latest(clsApproval objApproval, DataBseConnection objDatabse)
        {
            try
            {
                Division = Convert.ToInt32(ConfigurationManager.AppSettings["Division_code"]);
                SubDivision = Convert.ToInt32(ConfigurationManager.AppSettings["SubDiv_code"]);
                Section = Convert.ToInt32(ConfigurationManager.AppSettings["Section_code"]);
                string strQry = string.Empty;
                string sMaxNo = Convert.ToString(objDatabse.Get_max_no("WOA_ID", "TBLWO_OBJECT_AUTO"));
                if (objApproval.sBOId == "25" || objApproval.sBOId == "11")
                {
                    //NpgsqlCommand = new NpgsqlCommand();
                    // NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    // string sEntype = objDatabse.get_value("SELECT \"DF_STATUS_FLAG\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\",\"TBLESTIMATIONDETAILS\" WHERE \"WO_ID\" =:sWFObjectId AND \"DF_ID\"=CAST(\"WO_DATA_ID\" AS BIGINT) and \"EST_FAILUREID\"=\"DF_ID\"", NpgsqlCommand);
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETDFSTATUSFLAG");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    string sEntype = objDatabse.StringGetValue(NpgsqlCommand);


                    // NpgsqlCommand = new NpgsqlCommand();
                    //  NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    // string sftype = objDatabse.get_value("SELECT \"EST_FAIL_TYPE\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\",\"TBLESTIMATIONDETAILS\" WHERE \"WO_ID\" =:sWFObjectId AND \"DF_ID\"=CAST(\"WO_DATA_ID\" AS BIGINT) and \"EST_FAILUREID\"=\"DF_ID\"", NpgsqlCommand);
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETESTFAILTYPE");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    string sftype = objDatabse.StringGetValue(NpgsqlCommand);


                    // NpgsqlCommand = new NpgsqlCommand();
                    // NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    //  string sRecordId = objDatabse.get_value("SELECT \"DF_DTC_CODE\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\" WHERE \"WO_ID\" =:sWFObjectId AND \"DF_ID\"=CAST(\"WO_DATA_ID\" AS BIGINT)", NpgsqlCommand);
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETDFDTCCODE");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    string sRecordId = objDatabse.StringGetValue(NpgsqlCommand);

                    if (sftype == "2" || sftype == "")

                    {
                        if (sEntype == "2")
                        {
                            objApproval.sDescription = " Enhacement of  Work Order For DTC Code " + sRecordId;
                        }
                        if (sEntype == "4")
                        {
                            objApproval.sDescription = "Repair and enhancement of Work Order For DTC Code " + sRecordId;
                        }
                        else
                        {
                            objApproval.sDescription = " Repair and replacement of  Major faulty Work Order For DTC Code " + sRecordId;
                        }
                    }
                    else
                    {
                        objApproval.sDescription = " Repair and replacement of  Minor faulty Work Order For DTC Code " + sRecordId;
                    }
                }
                else if (objApproval.sBOId == "12")
                {
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETDFSTATUSFLAGWITHTWO");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    string sEntype = objDatabse.StringGetValue(NpgsqlCommand);
                    //NpgsqlCommand = new NpgsqlCommand();
                    // NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    //  string sEntype = objDatabse.get_value("SELECT \"DF_STATUS_FLAG\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLWORKORDER\",\"TBLDTCFAILURE\",\"TBLESTIMATIONDETAILS\" WHERE \"WO_ID\" =:sWFObjectId AND  \"WO_RECORD_ID\"=\"WO_SLNO\" AND \"DF_ID\"=\"WO_DF_ID\" and \"EST_FAILUREID\"=\"DF_ID\"", NpgsqlCommand);
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETESTFAILTYPETWO");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    string sftype = objDatabse.StringGetValue(NpgsqlCommand);

                    //   NpgsqlCommand = new NpgsqlCommand();
                    //   NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    //  string sftype = objDatabse.get_value("SELECT \"EST_FAIL_TYPE\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLWORKORDER\",\"TBLDTCFAILURE\",\"TBLESTIMATIONDETAILS\" WHERE \"WO_ID\" =:sWFObjectId AND  \"WO_RECORD_ID\"=\"WO_SLNO\" AND \"DF_ID\"=\"WO_DF_ID\" and \"EST_FAILUREID\"=\"DF_ID\"", NpgsqlCommand);
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETDFDTCCODETWO");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    string sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                    // NpgsqlCommand = new NpgsqlCommand();
                    // NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    // string sRecordId = objDatabse.get_value("SELECT \"DF_DTC_CODE\" || '~' || \"WO_NO\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLWORKORDER\",\"TBLDTCFAILURE\" WHERE \"WO_ID\" =:sWFObjectId AND  \"WO_RECORD_ID\"=\"WO_SLNO\" AND \"DF_ID\"=\"WO_DF_ID\"", NpgsqlCommand);
                    if (sRecordId == "")
                    {
                        if (objApproval.sTTKStatus == "1")
                        {
                            NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                            NpgsqlCommand.Parameters.AddWithValue("p_key", "GETWONO_TTKSTATUS1");
                            NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                            sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                            // NpgsqlCommand = new NpgsqlCommand();
                            // NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                            // sRecordId = objDatabse.get_value("SELECT  \"WO_NO\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLWORKORDER\" WHERE \"WO_ID\"=:sWFObjectId AND  \"WO_RECORD_ID\"=\"WO_SLNO\"", NpgsqlCommand);
                            objApproval.sDescription = "Indent For New DTC Commission TTK Flow WO No " + sRecordId;
                        }
                        else
                        {
                            NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                            NpgsqlCommand.Parameters.AddWithValue("p_key", "GETWONO_TTKSTATUSNOT1");
                            NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                            sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                            //  NpgsqlCommand = new NpgsqlCommand();
                            //  NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                            //  sRecordId = objDatabse.get_value("SELECT  \"WO_NO\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLWORKORDER\" WHERE \"WO_ID\"=:sWFObjectId AND  \"WO_RECORD_ID\"=\"WO_SLNO\"", NpgsqlCommand);
                            objApproval.sDescription = "Indent For New DTC Commission PTK with WO No " + sRecordId;
                        }
                    }
                    else
                    {
                        if (sEntype == "2")
                        {
                            objApproval.sDescription = "Enhacement of Indent For DTC Code " + sRecordId.Split('~').GetValue(0).ToString() + " and WO No " + sRecordId.Split('~').GetValue(1).ToString();
                        }
                        if (sEntype == "4")
                        {
                            objApproval.sDescription = "Repair and enhancement of  Indent For DTC Code " + sRecordId.Split('~').GetValue(0).ToString() + " and WO No " + sRecordId.Split('~').GetValue(1).ToString();
                        }
                        else
                        {
                            objApproval.sDescription = "Repair & replacement of Indent For DTC Code " + sRecordId.Split('~').GetValue(0).ToString() + " and WO No " + sRecordId.Split('~').GetValue(1).ToString();
                        }
                    }
                }
                else if (objApproval.sBOId == "13" || objApproval.sBOId == "56" || objApproval.sBOId == "57")
                {
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETDFSTATUSFLAGWITHINDENTDTTC");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    string sEntype = objDatabse.StringGetValue(NpgsqlCommand);
                    // NpgsqlCommand = new NpgsqlCommand();
                    // NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));

                    //string sEntype = objDatabse.get_value("SELECT \"DF_STATUS_FLAG\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLINDENT\",\"TBLWORKORDER\",\"TBLDTCFAILURE\",\"TBLDTCMAST\" WHERE \"WO_ID\" =:sWFObjectId AND  \"WO_RECORD_ID\"=\"WO_SLNO\" AND \"DF_ID\"=\"WO_DF_ID\" AND \"TI_WO_SLNO\" = \"WO_SLNO\" AND \"DT_CODE\" = \"DF_DTC_CODE\"", NpgsqlCommand);

                    //   NpgsqlCommand = new NpgsqlCommand();
                    //   NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    //   string sRecordId = objDatabse.get_value("SELECT \"DF_DTC_CODE\" || '~' || \"TI_INDENT_NO\" || '~' || \"WO_NO\" || '~' || \"DT_NAME\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLINDENT\",\"TBLWORKORDER\",\"TBLDTCFAILURE\",\"TBLDTCMAST\" WHERE \"WO_ID\" =:sWFObjectId AND  \"WO_RECORD_ID\"=\"WO_SLNO\" AND \"DF_ID\"=\"WO_DF_ID\" AND \"TI_WO_SLNO\" = \"WO_SLNO\" AND \"DT_CODE\" = \"DF_DTC_CODE\"", NpgsqlCommand);
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETDFDTCCODEWITHTINO_WONO_DTNAME");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    string sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                    if (sRecordId == "")
                    {
                        NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                        NpgsqlCommand.Parameters.AddWithValue("p_key", "GETTIINDENTNO_WONO");
                        NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                        sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                        // NpgsqlCommand = new NpgsqlCommand();
                        // NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                        // sRecordId = objDatabse.get_value("SELECT \"TI_INDENT_NO\" || '~' || \"WO_NO\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLINDENT\",\"TBLWORKORDER\" WHERE \"WO_ID\" =:sWFObjectId AND  \"WO_RECORD_ID\"=\"WO_SLNO\" AND \"TI_WO_SLNO\"=\"WO_SLNO\"", NpgsqlCommand);
                        objApproval.sDescription = "Invoice For New DTC Commission with Auto Indent No " + sRecordId.Split('~').GetValue(0).ToString() + " and WorkOrder No " + sRecordId.Split('~').GetValue(1).ToString();
                        if (objApproval.sBOId == "13")
                        {
                            objApproval.sDescription = "Invoice For New DTC Commission with Auto Indent No " + sRecordId.Split('~').GetValue(0).ToString() + " and WorkOrder No " + sRecordId.Split('~').GetValue(1).ToString() + " , DTC Code " + sdtccode;
                        }
                    }
                    else
                    {
                        if (sEntype == "2")
                        {
                            objApproval.sDescription = "Enhacement of  Invoice For DTC Code " + sRecordId.Split('~').GetValue(0).ToString() + ", DTC Name " + sRecordId.Split('~').GetValue(3).ToString() + " and Auto Indent No " + sRecordId.Split('~').GetValue(1).ToString() + " and WorkOrder No " + sRecordId.Split('~').GetValue(2).ToString();
                        }
                        if (sEntype == "4")
                        {
                            objApproval.sDescription = "Repair and enhancement  of Invoice For DTC Code " + sRecordId.Split('~').GetValue(0).ToString() + ", DTC Name " + sRecordId.Split('~').GetValue(3).ToString() + " and Auto Indent No " + sRecordId.Split('~').GetValue(1).ToString() + " and WorkOrder No " + sRecordId.Split('~').GetValue(2).ToString();
                        }
                        else
                        {
                            objApproval.sDescription = "Repair & replacement of Invoice For DTC Code " + sRecordId.Split('~').GetValue(0).ToString() + ", DTC Name " + sRecordId.Split('~').GetValue(3).ToString() + " and Auto Indent No " + sRecordId.Split('~').GetValue(1).ToString() + " and WorkOrder No " + sRecordId.Split('~').GetValue(2).ToString();
                        }
                    }
                }
                else if (objApproval.sBOId == "14")
                {
                    // NpgsqlCommand = new NpgsqlCommand();
                    // NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    // string sRecordId = objApproval.sdtccode;
                    NpgsqlCommand = new NpgsqlCommand();
                    NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    string sRecordId = objApproval.sdtccode;
                    objApproval.sDescription = "Decommissioning For DTC Code " + sRecordId;
                    if (sRecordId == "")
                    {
                        NpgsqlCommand = new NpgsqlCommand();
                        NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                        sRecordId = objDatabse.get_value("SELECT \"IN_INV_NO\" FROM \"TBLDTCINVOICE\",\"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\" =:sWFObjectId AND \"WO_RECORD_ID\"=\"IN_NO\" ", NpgsqlCommand);
                        objApproval.sDescription = "Commissioning of DTC for the Invoice NO " + sRecordId;
                    }
                }
                else if (objApproval.sBOId == "15")
                {
                    // string sEntype = objDatabse.get_value("SELECT \"DF_STATUS_FLAG\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\" = '" + objApproval.sDataReferenceId + "'");
                    // string sRecordId = objDatabse.get_value("SELECT \"DF_EQUIPMENT_ID\" || '~' || \"WO_NO_DECOM\" FROM \"TBLDTCFAILURE\" , \"TBLWORKORDER\" WHERE \"WO_DF_ID\" = \"DF_ID\" and \"DF_ID\" = '" + objApproval.sDataReferenceId + "'");

                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETDF_STATUSFLAGFROMTDTCF");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", objApproval.sDataReferenceId);
                    string sEntype = objDatabse.StringGetValue(NpgsqlCommand);

                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETDF_EQUIPMENTID_WO_NO_DECOM");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", objApproval.sDataReferenceId);
                    string sRecordId = objDatabse.StringGetValue(NpgsqlCommand);

                    if (sEntype == "2")
                    {
                        objApproval.sDescription = "Enhacement of RI Approval For DTr Code " + sRecordId.Split('~').GetValue(0).ToString() + " and DTC Code " + sRecordId.Split('~').GetValue(2).ToString() + " and Work Order NO " + sRecordId.Split('~').GetValue(1).ToString();
                    }
                    if (sEntype == "4")
                    {
                        objApproval.sDescription = "Repair and enhancement  of RI Approval For DTr Code " + sRecordId.Split('~').GetValue(0).ToString() + " and DTC Code " + sRecordId.Split('~').GetValue(2).ToString() + " and Work Order NO " + sRecordId.Split('~').GetValue(1).ToString();
                    }
                    else
                    {
                        objApproval.sDescription = "Repair & replacement of  RI Approval For DTr Code " + sRecordId.Split('~').GetValue(0).ToString() + " and DTC Code " + sRecordId.Split('~').GetValue(2).ToString() + " and Work Order NO " + sRecordId.Split('~').GetValue(1).ToString();
                    }
                }
                else if (objApproval.sBOId == "26")
                {
                    NpgsqlCommand = new NpgsqlCommand();
                    NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    //  string sRecordId = objDatabse.get_value("SELECT \"WO_DATA_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\"='" + objApproval.sWFObjectId + "'");
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETVALUEFORRECORDID");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", objApproval.sWFObjectId);
                    string sRecordId = objDatabse.StringGetValue(NpgsqlCommand);

                    // string sEntype = objDatabse.get_value("SELECT \"DF_STATUS_FLAG\" FROM \"TBLDTCFAILURE\" WHERE \"DF_DTC_CODE\" ='" + sRecordId + "'");
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETVALUEFORDFSTATUSFLAG");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", sRecordId);
                    string sEntype = objDatabse.StringGetValue(NpgsqlCommand);
                    if (sEntype == "2")
                    {
                        objApproval.sDescription = "Enhacement of Completion Report For DTC Code " + sRecordId;
                    }
                    if (sEntype == "4")
                    {
                        objApproval.sDescription = "Repair and enhancement  of Completion Report For DTC Code " + sRecordId;
                    }
                    else
                    {
                        objApproval.sDescription = "Repair & replacement of Completion Report For DTC Code " + sRecordId;
                    }
                }
                else if (objApproval.sBOId == "29")
                {
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETVALUEFORDFSTATUSFLAGONWOID");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", sWFObjectId);
                    string sEntype = objDatabse.StringGetValue(NpgsqlCommand);
                    //  NpgsqlCommand = new NpgsqlCommand();
                    //  NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    //  string sEntype = objDatabse.get_value("SELECT \"DF_STATUS_FLAG\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLINDENT\",\"TBLWORKORDER\",\"TBLDTCFAILURE\" WHERE \"WO_ID\" =:sWFObjectId AND  \"WO_RECORD_ID\"=\"TI_ID\" AND \"DF_ID\"=\"WO_DF_ID\" AND \"TI_WO_SLNO\"=\"WO_SLNO\" ", NpgsqlCommand);

                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETDFDTCCODE_TIINDENTNO_WONO");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    string sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                    // NpgsqlCommand = new NpgsqlCommand();
                    // NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    // string sRecordId = objDatabse.get_value("SELECT \"DF_DTC_CODE\" || '~' || \"TI_INDENT_NO\" || '~' || \"WO_NO\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLINDENT\",\"TBLWORKORDER\",\"TBLDTCFAILURE\" WHERE \"WO_ID\" =:sWFObjectId AND  \"WO_RECORD_ID\"=\"TI_ID\" AND \"DF_ID\"=\"WO_DF_ID\" AND \"TI_WO_SLNO\"=\"WO_SLNO\" ", NpgsqlCommand);
                    if (sRecordId == "")
                    {
                        // NpgsqlCommand = new NpgsqlCommand();
                        //  NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                        //  sRecordId = objDatabse.get_value("SELECT \"TI_INDENT_NO\" || '~' || \"WO_NO\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLINDENT\",\"TBLWORKORDER\" WHERE \"WO_ID\" =:sWFObjectId AND  \"WO_RECORD_ID\"=\"TI_ID\"  AND \"TI_WO_SLNO\"=\"WO_SLNO\"", NpgsqlCommand);
                        NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                        NpgsqlCommand.Parameters.AddWithValue("p_key", "GETVALUETIINDENTNO_WONO");
                        NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                        sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                        objApproval.sDescription = "Invoice For New DTC Commission with  Indent No " + sRecordId.Split('~').GetValue(0).ToString() + " and WorkOrder No " + sRecordId.Split('~').GetValue(1).ToString();
                    }
                    else
                    {
                        if (sEntype == "2")
                        {
                            objApproval.sDescription = "Enhacement of Invoice For DTC Code " + sRecordId.Split('~').GetValue(0).ToString() + " with WorkOrder No " + sRecordId.Split('~').GetValue(2).ToString() + " and Indent No " + sRecordId.Split('~').GetValue(1).ToString();
                        }
                        if (sEntype == "4")
                        {
                            objApproval.sDescription = "Repair and enhancement  of Invoice For DTC Code " + sRecordId.Split('~').GetValue(0).ToString() + " with WorkOrder No " + sRecordId.Split('~').GetValue(2).ToString() + " and Indent No " + sRecordId.Split('~').GetValue(1).ToString();
                        }
                        else
                        {
                            objApproval.sDescription = "Repair & replacement of  Invoice For DTC Code " + sRecordId.Split('~').GetValue(0).ToString() + " with WorkOrder No " + sRecordId.Split('~').GetValue(2).ToString() + " and Indent No " + sRecordId.Split('~').GetValue(1).ToString();
                        }
                    }
                }

                else if (objApproval.sBOId == "24")
                {
                    string sOfficeCode = string.Empty;
                    if (Convert.ToInt32(objApproval.sRefOfficeCode) > 1)
                    {
                        sOfficeCode = objApproval.sRefOfficeCode.Substring(0, Division);
                    }
                    else
                    {
                        sOfficeCode = objApproval.sRefOfficeCode;
                    }
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETSTORENAME");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", sOfficeCode);
                    string sStoreName = objDatabse.StringGetValue(NpgsqlCommand);
                    //  NpgsqlCommand = new NpgsqlCommand();
                    //  NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", Convert.ToInt32(sOfficeCode));
                    //  string sStoreName = objDatabse.get_value("SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\",\"TBLSTOREOFFCODE\" WHERE \"SM_ID\"=\"STO_SM_ID\" AND \"STO_OFF_CODE\" =:sOfficeCode", NpgsqlCommand);

                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETINDENTNO");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sNewRecordId));
                    string indentno = objDatabse.StringGetValue(NpgsqlCommand);

                    // NpgsqlCommand.Parameters.AddWithValue("si_id", Convert.ToInt32(objApproval.sNewRecordId));
                    // string indentno = objDatabse.get_value(" select \"SI_NO\"  from \"TBLSTOREINDENT\"   WHERE \"SI_ID\"  =:si_id", NpgsqlCommand);
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETSTOREID");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sNewRecordId));
                    string storeid = objDatabse.StringGetValue(NpgsqlCommand);
                    //   NpgsqlCommand.Parameters.AddWithValue("store_id", Convert.ToInt32(objApproval.sNewRecordId));
                    //  string storeid = objDatabse.get_value(" select \"SI_FROM_STORE\"  from \"TBLSTOREINDENT\"   WHERE \"SI_ID\"  =:store_id", NpgsqlCommand);
                    // NpgsqlCommand.Parameters.AddWithValue("storeid", Convert.ToInt32(storeid));
                    //  string sfromStoreName = objDatabse.get_value("SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\"  WHERE \"SM_ID\" =:storeid", NpgsqlCommand);

                    //  objApproval.sDescription = "Inter Store Indent no " + indentno + " Request for Specified Capacity Transformer From Store Name " + sfromStoreName;
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETFROMSTORE");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sNewRecordId));
                    string sfromStoreName = objDatabse.StringGetValue(NpgsqlCommand);
                }
                else if (objApproval.sBOId == "32")
                {
                    // NpgsqlCommand = new NpgsqlCommand();
                    // NpgsqlCommand.Parameters.AddWithValue("sNewRecordId", Convert.ToInt32(objApproval.sNewRecordId));
                    //string sResult = objDatabse.get_value("SELECT \"SI_NO\" ||'~' || \"IS_NO\" FROM \"TBLSTOREINDENT\",\"TBLSTOREINVOICE\" WHERE \"IS_ID\" =:sNewRecordId AND \"SI_ID\"=\"IS_SI_ID\"", NpgsqlCommand);
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETRESULT");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sNewRecordId));
                    string sResult = objDatabse.StringGetValue(NpgsqlCommand);
                    objApproval.sDescription = "Response for Store Indent No " + sResult.Split('~').GetValue(0).ToString() + " with Store Invoice Number " + sResult.Split('~').GetValue(1).ToString();
                }
                else if (objApproval.sBOId == "46")
                {
                    // NpgsqlCommand = new NpgsqlCommand();
                    // NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    // sRecordId = objDatabse.get_value("SELECT  \"WO_NO\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLWORKORDER\" WHERE \"WO_ID\"=:sWFObjectId AND  \"WO_RECORD_ID\"=\"WO_SLNO\"", NpgsqlCommand);
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETWONO");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                    objApproval.sDescription = " MinorFailure WO No " + sRecordId;
                }
                else if (objApproval.sBOId == "47" || objApproval.sBOId == "48")
                {
                    if (objApproval.sNewRecordId == null)
                    {
                        //  NpgsqlCommand = new NpgsqlCommand();
                        //  NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objApproval.sRecordId));

                        //  sRecordId = objDatabse.get_value("SELECT \"DF_DTC_CODE\" FROM \"TBLRECEIVEDTR\",\"TBLWORKORDER\",\"TBLDTCFAILURE\" WHERE \"RD_WO_SLNO\"=\"WO_SLNO\" AND \"WO_DF_ID\"=\"DF_ID\" AND \"RD_ID\"=:sRecordId", NpgsqlCommand);
                        NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                        NpgsqlCommand.Parameters.AddWithValue("p_key", "GETRECORDID");
                        NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sRecordId));
                        sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                    }
                    else
                    {
                        NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                        NpgsqlCommand.Parameters.AddWithValue("p_key", "GETNEWRECORDID");
                        NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sNewRecordId));
                        sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                        // NpgsqlCommand = new NpgsqlCommand();
                        //NpgsqlCommand.Parameters.AddWithValue("sNewRecordId", Convert.ToInt32(objApproval.sNewRecordId));
                        // sRecordId = objDatabse.get_value("SELECT \"DF_DTC_CODE\" FROM \"TBLRECEIVEDTR\",\"TBLWORKORDER\",\"TBLDTCFAILURE\" WHERE \"RD_WO_SLNO\"=\"WO_SLNO\" AND \"WO_DF_ID\"=\"DF_ID\" AND \"RD_ID\"=:sNewRecordId", NpgsqlCommand);
                    }
                    objApproval.sDescription = "Commissioning of Minor Coil Failure DTC code " + sRecordId;
                }
                else if (objApproval.sBOId == "62")
                {
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETPESTDTCCODE");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                    // NpgsqlCommand = new NpgsqlCommand();
                    //  NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    //  sRecordId = objDatabse.get_value("SELECT distinct \"PEST_DTC_CODE\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLPERMANENTESTIMATIONDETAILS\" WHERE \"WO_ID\" =:sWFObjectId", NpgsqlCommand);
                    objApproval.sDescription = "PermanentEstimation Request for DTC CODE  " + sRecordId;
                }
                else if (objApproval.sBOId == "63")
                {
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETPESTDTCCODE");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                    // NpgsqlCommand = new NpgsqlCommand();
                    // NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    // sRecordId = objDatabse.get_value("SELECT distinct \"PEST_DTC_CODE\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLPERMANENTESTIMATIONDETAILS\" WHERE \"WO_ID\" =:sWFObjectId", NpgsqlCommand);
                    objApproval.sDescription = "PermanentWorkOrder Request for DTC CODE " + sDataReferenceId;
                }
                else if (objApproval.sBOId == "64")
                {
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETPESTDTCCODE");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sDataReferenceId));
                    sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                    // NpgsqlCommand = new NpgsqlCommand();
                    // NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", Convert.ToInt32(objApproval.sDataReferenceId));
                    // sRecordId = objDatabse.get_value("SELECT distinct \"PEST_DTC_CODE\" FROM \"TBLPERMANENTESTIMATIONDETAILS\" WHERE \"PEST_ID\" =:sDataReferenceId", NpgsqlCommand);
                    objApproval.sDescription = "PermanentIndent Request for DTC CODE " + sRecordId;
                }
                else if (objApproval.sBOId == "65")
                {
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETPESTDTCCODEONPWO_SLNO");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sDataReferenceId));
                    sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                    //NpgsqlCommand = new NpgsqlCommand();
                    // NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", Convert.ToInt32(objApproval.sDataReferenceId));
                    // sRecordId = objDatabse.get_value("SELECT  \"PEST_DTC_CODE\" FROM \"TBLPERMANENTESTIMATIONDETAILS\",\"TBLPERMANENTWORKORDER\" WHERE \"PWO_SLNO\"=:sDataReferenceId and \"PEST_ID\"=\"PWO_PEF_ID\"", NpgsqlCommand);
                    objApproval.sDescription = "PermanentDecomm Request for DTC CODE " + sRecordId;
                }
                else if (objApproval.sBOId == "66")
                {
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETPESTDTCCODEONPESTID_PWOSLNO");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                    //NpgsqlCommand = new NpgsqlCommand();
                    //  NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    //  sRecordId = objDatabse.get_value("SELECT distinct \"PEST_DTC_CODE\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLPERMANENTESTIMATIONDETAILS\",\"TBLPERMANENTWORKORDER\"   WHERE  \"PEST_ID\"=\"PWO_PEF_ID\" and \"PWO_SLNO\" =:sWFObjectId", NpgsqlCommand);
                    objApproval.sDescription = "PermanentRI Request for DTC CODE " + sDataReferenceId;
                }
                else if (objApproval.sBOId == "67")
                {
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETPESTDTCCODEONWOID");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    sRecordId = objDatabse.StringGetValue(NpgsqlCommand);
                    //NpgsqlCommand = new NpgsqlCommand();
                    //NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    //sRecordId = objDatabse.get_value("SELECT  distinct \"PEST_DTC_CODE\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLPERMANENTESTIMATIONDETAILS\" WHERE \"WO_ID\" =:sWFObjectId", NpgsqlCommand);
                    objApproval.sDescription = "PermanentCR Request for DTC CODE " + sDataReferenceId;
                }
                else if (objApproval.sBOId == "58")
                {
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETDTCODE");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    string sResult = objDatabse.StringGetValue(NpgsqlCommand);
                    //NpgsqlCommand = new NpgsqlCommand();
                    // NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    //  string sResult = objDatabse.get_value("SELECT \"DT_CODE\" || '~' || \"TI_INDENT_NO\" || '~' || \"WO_NO\" || '~' || \"DT_NAME\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCINVOICE\",\"TBLINDENT\",\"TBLWORKORDER\",\"TBLDTCMAST\" WHERE \"DT_ID\" = \"WO_RECORD_ID\" AND \"WO_DATA_ID\" = CAST(\"IN_NO\" AS TEXT) AND \"WO_ID\" =:sWFObjectId AND \"TI_ID\" = \"IN_TI_NO\" AND \"TI_WO_SLNO\" = \"WO_SLNO\"", NpgsqlCommand);
                    objApproval.sDescription = "New DTC CR Request for DTC CODE " + sResult.Split('~').GetValue(0).ToString() + " with WO NO " + sResult.Split('~').GetValue(2).ToString();
                }
                else if (objApproval.sBOId == "76")
                {
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_approval");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETRWOA_NO");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", Convert.ToString(objApproval.sWFObjectId));
                    string sResult = objDatabse.StringGetValue(NpgsqlCommand);
                    //NpgsqlCommand = new NpgsqlCommand();
                    // NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                    // string sResult = objDatabse.get_value("SELECT \"RWOA_NO\" from \"TBLREPAIRERWORKAWARD\"  WHERE \"RWAO_ID\"=:sWFObjectId", NpgsqlCommand);
                    objApproval.sDescription = "TransilOil Replacement " + sResult.ToString() + " ";

                    //string sResult1 = objDatabse.get_value("select \"WO_REF_OFFCODE\" from \"TBLWORKFLOWOBJECTS\" where \"WO_ID\"=:sWFObjectId", NpgsqlCommand);

                    NpgsqlCommand cmd1 = new NpgsqlCommand("proc_get_value_of_wo_ref_ofc_code");
                    cmd1.Parameters.AddWithValue("wfobject_id", (objApproval.sWFObjectId ?? ""));
                    string sResult1 = objDatabse.StringGetValue(cmd1);
                    if (sResult1 != null && sResult1 != "")
                    {
                        objApproval.sOfficeCode = sResult1;
                    }
                }

                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "INSERT INTO \"TBLWO_OBJECT_AUTO\" (\"WOA_ID\",\"WOA_BFM_ID\",\"WOA_PREV_APPROVE_ID\",\"WOA_ROLE_ID\",\"WOA_OFFICE_CODE\",";
                //strQry += "\"WOA_CRBY\",\"WOA_DESCRIPTION\",\"WOA_REF_OFFCODE\",\"WOA_STATUS\") VALUES (:sMaxNo,:sBOFlowMasterId,:sWFObjectId,";
                //strQry += " :sRoleId,:sOfficeCode,:sCrby,:sDescription,:sRefOfficeCode,:status)";
                //NpgsqlCommand.Parameters.AddWithValue("sMaxNo", Convert.ToInt32(sMaxNo));
                //NpgsqlCommand.Parameters.AddWithValue("sBOFlowMasterId", Convert.ToInt32(objApproval.sBOFlowMasterId));
                //NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                //NpgsqlCommand.Parameters.AddWithValue("sRoleId", Convert.ToInt32(objApproval.sRoleId));
                //NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", Convert.ToInt32(objApproval.sOfficeCode));
                //NpgsqlCommand.Parameters.AddWithValue("sCrby", Convert.ToInt32(objApproval.sCrby));
                //NpgsqlCommand.Parameters.AddWithValue("sDescription", objApproval.sDescription);
                //NpgsqlCommand.Parameters.AddWithValue("sRefOfficeCode", Convert.ToInt32(objApproval.sRefOfficeCode));
                //if (objApproval.sStatus == null || objApproval.sStatus == "")
                //{
                //    objApproval.sStatus = "";
                //}
                //NpgsqlCommand.Parameters.AddWithValue("status", objApproval.sStatus);
                //objDatabse.ExecuteQry(strQry, NpgsqlCommand);

                string[] Arr = new string[3];

                NpgsqlCommand cmd = new NpgsqlCommand("proc_save_wfobject_auto_latest");
                cmd.Parameters.AddWithValue("bo_flow_master_id", (objApproval.sBOFlowMasterId ?? ""));
                cmd.Parameters.AddWithValue("wfo_object_id", (objApproval.sWFObjectId ?? ""));
                cmd.Parameters.AddWithValue("role_id", (objApproval.sRoleId ?? ""));
                cmd.Parameters.AddWithValue("off_code", (objApproval.sOfficeCode ?? ""));
                cmd.Parameters.AddWithValue("crby", (objApproval.sCrby ?? ""));
                cmd.Parameters.AddWithValue("desc", (objApproval.sDescription ?? ""));
                cmd.Parameters.AddWithValue("ref_ofc_code", (objApproval.sRefOfficeCode ?? ""));
                cmd.Parameters.AddWithValue("status", (objApproval.sStatus ?? ""));

                cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd.Parameters.Add("pk_id", NpgsqlDbType.Text);
                cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                cmd.Parameters["pk_id"].Direction = ParameterDirection.Output;
                Arr[0] = "msg";
                Arr[1] = "op_id";
                Arr[2] = "pk_id";
                Arr = objDatabse.Execute(cmd, Arr, 3);

                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                // return false;
                throw ex;
            }
        }

        /// <summary>
        /// Update Initial Action Id from Workflow Object Id in TBLWO_OBJECT_AUTO Table
        /// </summary>
        /// <param name="objApproval"></param>
        /// <returns></returns>
        public bool UpdateWFOAutoObject(clsApproval objApproval)
        {
            string[] Arr = new string[2];
            try
            {
                string strQry = string.Empty;
                string var = string.Empty;
                // NpgsqlCommand = new NpgsqlCommand();
                if (objApproval.sbfm_id == null || objApproval.sbfm_id == string.Empty || objApproval.sbfm_id == "")
                {
                    objApproval.sbfm_id = "0";
                }
                if (objApproval.sWFObjectId == null || objApproval.sWFObjectId == string.Empty || objApproval.sWFObjectId == "")
                {
                    objApproval.sWFObjectId = "0";
                }

                if (objApproval.sPrevWFOId == null || objApproval.sPrevWFOId == string.Empty || objApproval.sPrevWFOId == "")
                {
                    objApproval.sPrevWFOId = "0";
                }
                #region Old inline query
                //strQry = " UPDATE \"TBLWO_OBJECT_AUTO\" SET \"WOA_INITIAL_ACTION_ID\" ='" + Convert.ToInt32(objApproval.sWFObjectId) + "' ";
                //strQry += " WHERE \"WOA_PREV_APPROVE_ID\" ='" + Convert.ToInt32(objApproval.sPrevWFOId) + "' ";
                //strQry += " and \"WOA_BFM_ID\"='" + Convert.ToInt32(objApproval.sbfm_id) + "' ";
                //// NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                //// NpgsqlCommand.Parameters.AddWithValue("sPrevWFOId", Convert.ToInt32(objApproval.sPrevWFOId));
                //// NpgsqlCommand.Parameters.AddWithValue("sbfm_id", Convert.ToInt32(objApproval.sbfm_id));
                //objcon.ExecuteQry(strQry);
                #endregion

                NpgsqlCommand cmd_TBLTCRANGEALLOTMENT = new NpgsqlCommand("proc_update_wfoautoobject_for_clsapproval");
                cmd_TBLTCRANGEALLOTMENT.Parameters.AddWithValue("p_initial_action_id", Convert.ToInt32(objApproval.sWFObjectId));
                cmd_TBLTCRANGEALLOTMENT.Parameters.AddWithValue("p_prev_approve_id", Convert.ToInt32(objApproval.sPrevWFOId));
                cmd_TBLTCRANGEALLOTMENT.Parameters.AddWithValue("p_bfm_id", Convert.ToInt32(objApproval.sbfm_id));
                cmd_TBLTCRANGEALLOTMENT.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd_TBLTCRANGEALLOTMENT.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd_TBLTCRANGEALLOTMENT.Parameters["msg"].Direction = ParameterDirection.Output;
                cmd_TBLTCRANGEALLOTMENT.Parameters["op_id"].Direction = ParameterDirection.Output;
                Arr[0] = "msg";
                Arr[1] = "op_id";
                Arr = ObjBasCon.Execute(cmd_TBLTCRANGEALLOTMENT, Arr, 2);
                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }
        /// <summary>
        /// Update Initial Action Id from Workflow Object Id in TBLWO_OBJECT_AUTO Table
        /// </summary>
        /// <param name="objApproval"></param>
        /// <returns></returns>       
        public bool UpdateWFOAutoObject_Latest(clsApproval objApproval, DataBseConnection objDatabse)
        {
            string[] Arr = new string[2];
            try
            {
                string strQry = string.Empty;
                string var = string.Empty;
                // NpgsqlCommand = new NpgsqlCommand();
                if ((objApproval.sbfm_id ?? "").Length == 0)
                {
                    objApproval.sbfm_id = "0";
                }
                if ((objApproval.sWFObjectId ?? "").Length == 0)
                {
                    objApproval.sWFObjectId = "0";
                }
                if ((objApproval.sPrevWFOId ?? "").Length == 0)
                {
                    objApproval.sPrevWFOId = "0";
                }
                #region Old inline query
                //strQry = " UPDATE \"TBLWO_OBJECT_AUTO\" SET \"WOA_INITIAL_ACTION_ID\" ='" + Convert.ToInt32(objApproval.sWFObjectId) + "' ";
                //strQry += " WHERE \"WOA_PREV_APPROVE_ID\" ='" + Convert.ToInt32(objApproval.sPrevWFOId) + "' and ";
                //strQry += " \"WOA_BFM_ID\"='" + Convert.ToInt32(objApproval.sbfm_id) + "' ";
                //objDatabse.ExecuteQry(strQry);
                #endregion

                NpgsqlCommand cmd_TBLTCRANGEALLOTMENT = new NpgsqlCommand("proc_update_wfoautoobject_for_clsapproval");
                cmd_TBLTCRANGEALLOTMENT.Parameters.AddWithValue("p_initial_action_id", Convert.ToInt32(objApproval.sWFObjectId));
                cmd_TBLTCRANGEALLOTMENT.Parameters.AddWithValue("p_prev_approve_id", Convert.ToInt32(objApproval.sPrevWFOId));
                cmd_TBLTCRANGEALLOTMENT.Parameters.AddWithValue("p_bfm_id", Convert.ToInt32(objApproval.sbfm_id));
                cmd_TBLTCRANGEALLOTMENT.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd_TBLTCRANGEALLOTMENT.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd_TBLTCRANGEALLOTMENT.Parameters["msg"].Direction = ParameterDirection.Output;
                cmd_TBLTCRANGEALLOTMENT.Parameters["op_id"].Direction = ParameterDirection.Output;
                Arr[0] = "msg";
                Arr[1] = "op_id";
                Arr = objDatabse.Execute(cmd_TBLTCRANGEALLOTMENT, Arr, 2);
                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                throw ex;
                // return false;
            }
        }
        /// <summary>
        /// To get Form Creator Access (If priority is 1, Consider as Form Creator)
        /// </summary>
        /// <param name="sBOId"></param>
        /// <param name="sRoleId"></param>
        /// <param name="sFormName"></param>
        /// <returns></returns>
        public string GetFormCreatorLevel(string sBOId, string sRoleId, string sFormName = "", string Woa_id = "")
        {
            try
            {
                #region old inline query
                //string strQry = string.Empty;
                //if (sBOId != "")
                //{
                //    NpgsqlCommand = new NpgsqlCommand();
                //    strQry = "SELECT \"WM_LEVEL\" FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_ROLEID\" =:sRoleId AND \"WM_BOID\" =:sBOId";
                //    NpgsqlCommand.Parameters.AddWithValue("sRoleId", Convert.ToInt32(sRoleId));
                //    NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(sBOId));

                //    if (sRoleId == Convert.ToString(ConfigurationManager.AppSettings["SupAdminRole"]) && Woa_id != "" && Woa_id != null)
                //    {
                //        strQry = " select case when \"WOA_INITIAL_ACTION_ID\" is null then 1 else 0 end from \"TBLWO_OBJECT_AUTO\", ";
                //        strQry += " \"TBLWORKFLOWOBJECTS\" where \"WO_ID\" = \"WOA_PREV_APPROVE_ID\" and \"WOA_ID\"=:sWoa_id ";
                //        NpgsqlCommand.Parameters.AddWithValue("sWoa_id", Convert.ToInt32(Woa_id));
                //    }
                //}
                //else
                //{
                //    NpgsqlCommand = new NpgsqlCommand();
                //    strQry = "SELECT \"WM_LEVEL\" FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_ROLEID\" =:sRoleId AND \"WM_BOID\" = ";
                //    strQry += " (SELECT \"BO_ID\" FROM \"TBLBUSINESSOBJECT\" WHERE UPPER(\"BO_FORMNAME\")=:sFormName)";
                //    NpgsqlCommand.Parameters.AddWithValue("sRoleId", Convert.ToInt32(sRoleId));
                //    NpgsqlCommand.Parameters.AddWithValue("sFormName", sFormName.Trim().ToUpper());
                //}
                //return objcon.get_value(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_for_getformcreatorlevel_in_clsapproval");
                cmd.Parameters.AddWithValue("p_roleid", Convert.ToString(sRoleId));
                cmd.Parameters.AddWithValue("p_boid", Convert.ToString(sBOId));
                cmd.Parameters.AddWithValue("p_supadminrole", Convert.ToString(ConfigurationManager.AppSettings["SupAdminRole"]));
                cmd.Parameters.AddWithValue("p_woa_id", Convert.ToString(Woa_id));
                cmd.Parameters.AddWithValue("p_formname", Convert.ToString(sFormName ?? "").Trim().ToUpper());
                return ObjBasCon.StringGetValue(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        /// <summary>
        /// Get Datatable from XML string
        /// </summary>
        /// <param name="sWFDataId"></param>
        /// <returns></returns>
        public DataTable GetDatatableFromXML(string sWFDataId)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string sXMLResult = string.Empty;
            try
            {
                if (sWFDataId != "")
                {


                    sXMLResult = GetWFOData(sWFDataId);
                    if (sXMLResult != "")
                    {
                        StringReader sReader = new StringReader(sXMLResult);
                        ds.ReadXml(sReader);
                        return ds.Tables[0];
                    }
                }
                if (ds.Tables.Count == 0)
                {
                    return dt;
                }
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ds.Tables[0];
            }
        }

        /// <summary>
        /// Get Datatable from Multiple XML string
        /// </summary>
        /// <param name="sWFDataId"></param>
        /// <returns></returns>
        public DataSet GetDatatableFromMultipleXML(string sWFDataId)
        {
            //  DataSet ds = new DataSet();
            DataSet dsResult = new DataSet();
            try
            {
                if (sWFDataId == "" && sWFDataId == null)
                {
                    return dsResult;
                }
                string sXMLResult = GetWFOData(sWFDataId);


                // string[] sXMLStrings = sXMLResult.Split(';');

                //DataRow dRow = dt.NewRow();
                //for (int i = 0; i < sXMLStrings.Length; i++)
                //{
                //    StringReader sReader = new StringReader(sXMLStrings[i]);
                //    ds.ReadXml(sReader, XmlReadMode.IgnoreSchema);

                //    dsResult.Merge(ds);
                //    ds.Clear();
                //}            
                StringReader sReader = new StringReader(sXMLResult);
                dsResult.ReadXml(sReader);
                return dsResult;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dsResult;
            }
        }


        /// <summary>
        /// Create Xml format using Table Name and Column Name with Values
        /// </summary>
        /// <param name="strColumns"></param>
        /// <param name="strParameters"></param>
        /// <param name="strTableName"></param>
        /// <returns></returns>
        public string CreateXml(string strColumns, string strParameters, string strTableName)
        {
            try
            {
                DataTable dtXmlContent = new DataTable();

                DataTable dtnew = new DataTable();

                DataSet ds;
                if (strTableName.Contains(";"))
                {
                    ds = new DataSet(strTableName.Split(';').GetValue(0).ToString());
                }
                else
                {
                    ds = new DataSet(strTableName);
                }

                string[] strArrColumns = strColumns.Split(';');
                string[] strArrParameters = strParameters.Split(';');
                string[] strTableNames = strTableName.Split(';');

                int k = 0;
                //DataRow dRow = dt.NewRow();
                for (int i = 0; i < strArrColumns.Length; i++)
                {
                    DataTable dt = new DataTable();
                    DataRow dRow = dt.NewRow();
                    string[] strdtColumns = strArrColumns[i].Split(',');
                    string[] strdtParametres = strArrParameters[i].Split(',');
                    dt.TableName = strTableNames[i];
                    //DataRow dRow1 = dtnew.NewRow();
                    for (int j = 0; j < strdtColumns.Length; j++)
                    {
                        dt.Columns.Add(strdtColumns[j]);
                        if (k < strdtParametres.Length)
                        {
                            string strColumnName = strdtParametres[k];
                            dRow[dt.Columns[j]] = strdtParametres[k];
                            if (dt.Rows.Count == 0)
                            {
                                dt.Rows.Add(dRow);
                            }
                            dt.AcceptChanges();
                            //i--;
                        }
                        k++;

                    }

                    k = 0;

                    ds.Merge(dt);
                    dt.Clear();

                }
                return ds.GetXml();
                //dt.TableName = "Failure and Invoice";
                //////////////////////////////////////////////
                //dt.TableName = "TBLDTCFAILURE";

            }

            catch (Exception ex)
            {
                string strfailure = string.Empty;
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return strfailure;
                //return ds;
            }
        }
        ///// <summary>
        ///// To get XML String from TBLWFODATA based on WFODataId corresponding to WorkflowObject Id from TBLWORKFLOWOBJECTS
        ///// </summary>
        ///// <param name="sWFDataId"></param>
        ///// <returns></returns>        
        public string GetWFOData(string sWFDataId)
        {
            DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
            try
            {
                string strQry = string.Empty;

                if (sWFDataId == "" || sWFDataId == string.Empty || sWFDataId == null)
                {
                    sWFDataId = "0";
                }
                //strQry = " SELECT WFO_DATA FROM TBLWFODATA,TBLWORKFLOWOBJECTS WHERE WFO_ID=WO_WFO_ID AND WO_ID='" + sWFOId + "'";
                #region old Query
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "SELECT \"WFO_DATA\" FROM \"TBLWFODATA\" WHERE \"WFO_ID\" =:sWFDataId";
                //NpgsqlCommand.Parameters.AddWithValue("sWFDataId", Convert.ToInt32(sWFDataId));
                //string sXMLResult = objcon.get_value(strQry, NpgsqlCommand);
                //return sXMLResult;
                #endregion
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_value_wfo_data");
                cmd.Parameters.AddWithValue("wfdata_id", (sWFDataId ?? ""));
                string sXMLResult = objDatabse.StringGetValue(cmd);
                return sXMLResult;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        #region Extra Code
        public string CreateXml1(string strColumns, string strParameters)
        {
            try
            {
                DataTable dtXmlContent = new DataTable();
                DataTable dt = new DataTable();
                string[] strdtColumns = new string[10];
                string[] strdtParameterValues = new string[10];
                //dt.Columns.Add("QueryValues");
                //dt.Columns.Add("ParameterValues");
                strdtColumns = strColumns.Split(',');
                strdtParameterValues = strParameters.Split(',');
                int j = 0;
                DataRow dRow = dt.NewRow();
                for (int i = 0; i < strdtColumns.Length; i++)
                {

                    dt.Columns.Add(strdtColumns[i]);
                    if (j < strdtParameterValues.Length)
                    {
                        string strColumnName = strdtColumns[i];
                        dRow[dt.Columns[i]] = strdtParameterValues[j];
                        if (dt.Rows.Count == 0)
                        {
                            dt.Rows.Add(dRow);
                        }
                        dt.AcceptChanges();
                        //i--;
                    }
                    j++;

                }
                dt.TableName = "TBLDTCFAILURE";
                DataSet ds = new DataSet("TBLDTCFAILURE");

                ds.Tables.Add(dt);
                return ds.GetXml();
            }
            catch (Exception ex)
            {
                string strfailure = string.Empty;
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return strfailure;
                //return ds;
            }
        }
        #endregion


        /// <summary>
        /// Check Data already exist and Waiting for Approval to Restrict duplicate Entry
        /// </summary>
        /// <param name="sDataReferenceId"></param>
        /// <param name="sBOId"></param>
        /// <returns></returns>
        public bool CheckAlreadyExistEntry(string sDataReferenceId, string sBOId)
        {
            string sResult = string.Empty;
            try
            {
                #region Old inline query
                //string strQry = string.Empty;                
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "SELECT \"WO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_DATA_ID\" =:sDataReferenceId AND \"WO_APPROVE_STATUS\" ='0' AND \"WO_BO_ID\" =:sBOId";
                //NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", sDataReferenceId);
                //NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(sBOId));
                //sResult = objcon.get_value(strQry, NpgsqlCommand);
                #endregion 

                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_for_checkalreadyexistentry_in_clsapproval");
                cmd.Parameters.AddWithValue("p_datareferenceid", Convert.ToString(sDataReferenceId ?? ""));
                cmd.Parameters.AddWithValue("p_boid", Convert.ToInt32(sBOId));
                sResult = ObjBasCon.StringGetValue(cmd);


                if (sResult != "")
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// changed by rudres 11-08-2020
        /// </summary>
        /// <param name="sDataReferenceId"></param>
        /// <param name="sBOId"></param>
        /// <returns></returns>
        //public bool CheckAlreadyExistEntryfaulty(string sDataReferenceId, string sBOId)
        //{
        //    try
        //    {
        //        string strQry = string.Empty;
        //        string sResult = string.Empty;
        //        NpgsqlCommand = new NpgsqlCommand();
        //        strQry = "SELECT \"WO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_DATA_ID\" =:sDataReferenceId  AND \"WO_BO_ID\" =:sBOId AND  \"WO_RECORD_ID\"<0";
        //        NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", sDataReferenceId);
        //        NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(sBOId));
        //        sResult = objcon.get_value(strQry, NpgsqlCommand);
        //        if (sResult != "")
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return false;
        //    }
        //}


        //public bool CheckAlreadyExistEntryfaulty(string sDataReferenceId, string sBOId)
        //{
        //    try
        //    {
        //        string strQry = string.Empty;
        //        string sResult = string.Empty;
        //        NpgsqlCommand = new NpgsqlCommand();
        //        strQry = "SELECT \"WO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_DATA_ID\" =:sDataReferenceId  AND \"WO_BO_ID\" =:sBOId";
        //        NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", sDataReferenceId);
        //        NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(sBOId));
        //        sResult = objcon.get_value(strQry, NpgsqlCommand);
        //        if (sResult != "")
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return false;
        //    }
        //}

        /// changed by rudres 19-08-2020
        //public bool CheckAlreadyExistEntryfaulty(string sDataReferenceId, string sBOId)
        //{
        //    try
        //    {
        //        string strQry = string.Empty;
        //        string sResult = string.Empty;
        //        NpgsqlCommand = new NpgsqlCommand();
        //        if (sBOId == "71")
        //        {

        //            strQry = "SELECT \"WO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_DATA_ID\" =:sDataReferenceId  AND \"WO_BO_ID\" =:sBOId";
        //            NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", sDataReferenceId);
        //            NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(sBOId));
        //            sResult = objcon.get_value(strQry, NpgsqlCommand);
        //            if (sResult != "")
        //            {

        //                strQry = " SELECT Max(\"RSD_ID\") FROM \"TBLREPAIRSENTDETAILS\" WHERE \"RSD_TC_CODE\" =:sTCcode  AND \"RSD_DELIVARY_DATE\" IS NOT NULL and \"RSD_PROCESS_FLAG\" = 1";
        //                NpgsqlCommand.Parameters.AddWithValue("sTCcode", Convert.ToInt32(sDataReferenceId));
        //                string flag = objcon.get_value(strQry, NpgsqlCommand);
        //                if (flag.Length == 0)
        //                {
        //                    return true;
        //                }
        //                else
        //                {
        //                    strQry = "SELECT MAX(\"WO_ID\") FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_DATA_ID\" =:sDataReferenceId1  AND \"WO_BO_ID\" =:sBOId1 and \"WO_RECORD_ID\" < 0";
        //                    NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId1", sDataReferenceId);
        //                    NpgsqlCommand.Parameters.AddWithValue("sBOId1", Convert.ToInt32(sBOId));
        //                    sResult = objcon.get_value(strQry, NpgsqlCommand);
        //                    if (sResult.Length != 0)
        //                    {
        //                        return true;
        //                    }
        //                    return false;
        //                }

        //            }
        //            return false;
        //        }
        //        else
        //        {
        //            strQry = "SELECT  \"WO_RECORD_ID\"  ||'~'|| MAX(\"WO_ID\")  FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_DATA_ID\" =:sDataReferenceId  AND \"WO_BO_ID\" =71  and \"WO_RECORD_ID\" > 0  GROUP BY \"WO_RECORD_ID\" ";
        //            NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", sDataReferenceId);
        //            NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(sBOId));
        //            string EstID = objcon.get_value(strQry, NpgsqlCommand);
        //            if (EstID != "")
        //            {

        //                strQry = " SELECT Max(\"RSD_ID\") FROM \"TBLREPAIRSENTDETAILS\" WHERE \"RSD_TC_CODE\" =:sTCcode  AND \"RSD_DELIVARY_DATE\" IS NOT NULL and \"RSD_PROCESS_FLAG\" = 1";
        //                NpgsqlCommand.Parameters.AddWithValue("sTCcode", Convert.ToInt32(sDataReferenceId));
        //                string flag = objcon.get_value(strQry, NpgsqlCommand);
        //                if (flag.Length == 0)
        //                {
        //                    return true;
        //                }
        //                else
        //                {
        //                    strQry = "SELECT MAX(\"WO_ID\") FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_DATA_ID\" ='" + EstID.Split('~').GetValue(0).ToString() + "'  AND \"WO_BO_ID\" =:sBOId1 ";
        //                    NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId1", sDataReferenceId);
        //                    NpgsqlCommand.Parameters.AddWithValue("sBOId1", Convert.ToInt32(sBOId));
        //                    sResult = objcon.get_value(strQry, NpgsqlCommand);
        //                    if (sResult.Length != 0)
        //                    {
        //                        return true;
        //                    }
        //                    return false;
        //                }
        //            }
        //            return false;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return false;
        //    }
        //}


        /// changed by rudres 13-10-2020             
        public bool CheckAlreadyExistEntryfaulty(string sDataReferenceId, string sBOId)
        {
            DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
            try
            {
                string PKey = string.Empty;
                string strQry = string.Empty;
                string sResult = string.Empty;
                NpgsqlCommand = new NpgsqlCommand();
                if (sBOId == "71")
                {

                    //strQry = "SELECT \"WO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_DATA_ID\" =:sDataReferenceId  AND \"WO_BO_ID\" =:sBOId";
                    //NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", sDataReferenceId);
                    //NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(sBOId));
                    //sResult = objcon.get_value(strQry, NpgsqlCommand);

                    PKey = "GET_WO_ID";
                    NpgsqlCommand cmd = new NpgsqlCommand("proc_get_value_check_already_exists_entry_faulty");
                    cmd.Parameters.AddWithValue("data_ref_id", (sDataReferenceId ?? ""));
                    cmd.Parameters.AddWithValue("bo_id", (sBOId ?? ""));
                    cmd.Parameters.AddWithValue("p_key", (PKey ?? ""));

                    sResult = objDatabse.StringGetValue(cmd);

                    if (sResult != "")
                    {

                        //strQry = " SELECT Max(\"RSD_ID\") FROM \"TBLREPAIRSENTDETAILS\" WHERE \"RSD_TC_CODE\" =:sTCcode  AND \"RSD_DELIVARY_DATE\" IS NOT NULL and \"RSD_PROCESS_FLAG\" = 1";
                        //NpgsqlCommand.Parameters.AddWithValue("sTCcode", Convert.ToInt32(sDataReferenceId));
                        //string flag = objcon.get_value(strQry, NpgsqlCommand);
                        PKey = "GET_MAX_RSD_ID";

                        NpgsqlCommand cmd1 = new NpgsqlCommand("proc_get_value_check_already_exists_entry_faulty");
                        cmd1.Parameters.AddWithValue("tc_code", (sDataReferenceId ?? ""));
                        cmd1.Parameters.AddWithValue("p_key", (PKey ?? ""));

                        string flag = objDatabse.StringGetValue(cmd1);

                        if (flag.Length == 0)
                        {
                            return true;
                        }
                        else
                        {
                            //strQry = "SELECT MAX(\"WO_ID\") FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_DATA_ID\" =:sDataReferenceId1  AND \"WO_BO_ID\" =:sBOId1 and \"WO_RECORD_ID\" < 0";
                            //NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId1", sDataReferenceId);
                            //NpgsqlCommand.Parameters.AddWithValue("sBOId1", Convert.ToInt32(sBOId));
                            //sResult = objcon.get_value(strQry, NpgsqlCommand);
                            PKey = "GET_MAX_WO_ID";

                            NpgsqlCommand cmd2 = new NpgsqlCommand("proc_get_value_check_already_exists_entry_faulty");
                            cmd2.Parameters.AddWithValue("data_ref_id", (sDataReferenceId ?? ""));
                            cmd2.Parameters.AddWithValue("bo_id", (sBOId ?? ""));
                            cmd2.Parameters.AddWithValue("p_key", (PKey ?? ""));

                            sResult = objDatabse.StringGetValue(cmd2);

                            if (sResult.Length != 0)
                            {
                                return true;
                            }
                            return false;
                        }

                    }
                    return false;
                }
                else
                {
                    //strQry = "SELECT  \"WO_RECORD_ID\"  ||'~'|| MAX(\"WO_ID\")  FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_DATA_ID\" =:sDataReferenceId  AND \"WO_BO_ID\" =71  and \"WO_RECORD_ID\" > 0  GROUP BY \"WO_RECORD_ID\" ";
                    //NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", sDataReferenceId);
                    //NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(sBOId));
                    //string EstID = objcon.get_value(strQry, NpgsqlCommand);
                    PKey = "GET_RECORD_ID_AND_MAX_WO_ID";
                    NpgsqlCommand cmd3 = new NpgsqlCommand("proc_get_value_check_already_exists_entry_faulty");
                    cmd3.Parameters.AddWithValue("data_ref_id", (sDataReferenceId ?? ""));
                    cmd3.Parameters.AddWithValue("bo_id", (sBOId ?? ""));
                    cmd3.Parameters.AddWithValue("p_key", (PKey ?? ""));

                    string EstID = objDatabse.StringGetValue(cmd3);

                    if (EstID != "")
                    {

                        //strQry = " SELECT Max(\"RSD_ID\") FROM \"TBLREPAIRSENTDETAILS\" WHERE \"RSD_TC_CODE\" =:sTCcode  AND \"RSD_DELIVARY_DATE\" IS NOT NULL and \"RSD_PROCESS_FLAG\" = 1";
                        //NpgsqlCommand.Parameters.AddWithValue("sTCcode", Convert.ToInt32(sDataReferenceId));
                        //string flag = objcon.get_value(strQry, NpgsqlCommand);
                        PKey = "GET_MAX_RSD_ID1";

                        NpgsqlCommand cmd4 = new NpgsqlCommand("proc_get_value_check_already_exists_entry_faulty");
                        cmd4.Parameters.AddWithValue("data_ref_id", (sDataReferenceId ?? ""));
                        cmd4.Parameters.AddWithValue("p_key", (PKey ?? ""));

                        string flag = objDatabse.StringGetValue(cmd4);

                        if (flag.Length == 0)
                        {
                            return true;
                        }
                        else
                        {
                            //strQry = "SELECT MAX(\"WO_ID\") FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_DATA_ID\" ='" + EstID.Split('~').GetValue(0).ToString() + "'  AND \"WO_BO_ID\" =:sBOId1 ";
                            //NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId1", sDataReferenceId);
                            //NpgsqlCommand.Parameters.AddWithValue("sBOId1", Convert.ToInt32(sBOId));
                            //sResult = objcon.get_value(strQry, NpgsqlCommand);
                            PKey = "GET_MAX_WO_ID1";

                            NpgsqlCommand cmd5 = new NpgsqlCommand("proc_get_value_check_already_exists_entry_faulty");
                            cmd5.Parameters.AddWithValue("est_id", (EstID.Split('~').GetValue(0).ToString() ?? ""));
                            cmd5.Parameters.AddWithValue("bo_id", (sBOId ?? ""));
                            cmd5.Parameters.AddWithValue("p_key", (PKey ?? ""));

                            sResult = objDatabse.StringGetValue(cmd5);

                            if (sResult.Length != 0)
                            {
                                //strQry = " SELECT Max(\"RSD_ID\") FROM \"TBLREPAIRSENTDETAILS\" WHERE \"RSD_TC_CODE\" =:sTCcode  AND \"RSD_DELIVARY_DATE\" IS NOT NULL and \"RSD_PROCESS_FLAG\" = 1";
                                //NpgsqlCommand.Parameters.AddWithValue("sTCcode", Convert.ToInt32(sDataReferenceId));
                                //flag = objcon.get_value(strQry, NpgsqlCommand);
                                PKey = "GET_MAX_RSD_ID2";

                                NpgsqlCommand cmd6 = new NpgsqlCommand("proc_get_value_check_already_exists_entry_faulty");
                                cmd6.Parameters.AddWithValue("data_ref_id", (sDataReferenceId ?? ""));
                                cmd6.Parameters.AddWithValue("p_key", (PKey ?? ""));

                                flag = objDatabse.StringGetValue(cmd6);

                                if (flag.Length != 0)
                                {
                                    return false;
                                }
                                else
                                    return true;
                            }
                            return false;
                        }
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }

        //public bool CheckAlreadyExistEntryfaultyestimate(string sDataReferenceId, string sBOId)
        //{
        //    try
        //    {
        //        string strQry = string.Empty;
        //        string sResult = string.Empty;
        //        NpgsqlCommand = new NpgsqlCommand();
        //        strQry = "SELECT \"WO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE cast(\"WO_DESCRIPTION\" as text) LIKE :sDataReferenceId||'%'  AND \"WO_BO_ID\" =:sBOId AND  \"WO_RECORD_ID\"<0";
        //        NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", sDataReferenceId);
        //        NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(sBOId));
        //        sResult = objcon.get_value(strQry, NpgsqlCommand);
        //        if (sResult != "")
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return false;
        //    }
        //}
        //public string GetDataReferenceId(string sBOId, string sWFOId = "")
        //{
        //    try
        //    {
        //        string strQry = string.Empty;
        //        string sResult = string.Empty;
        //        DataTable dt = new DataTable();
        //        if (sBOId != "")
        //        {
        //            NpgsqlCommand = new NpgsqlCommand();
        //            strQry = "SELECT DISTINCT \"WO_DATA_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" =:sBOId AND \"WO_APPROVE_STATUS\" ='0' AND \"WO_DATA_ID\" IS NOT NULL";
        //            NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(sBOId));
        //            dt = objcon.FetchDataTable(strQry, NpgsqlCommand);
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                sResult += Convert.ToString(dt.Rows[i]["WO_DATA_ID"]) + ",";
        //            }
        //        }
        //        else if (sWFOId != "")
        //        {
        //            NpgsqlCommand = new NpgsqlCommand();
        //            strQry = "SELECT  \"WO_DATA_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\" =:sWFOId ";
        //            NpgsqlCommand.Parameters.AddWithValue("sWFOId", Convert.ToInt32(sWFOId));
        //            sResult = objcon.get_value(strQry, NpgsqlCommand);
        //        }
        //        return sResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return ex.Message;
        //    }
        //}
        //public string SendSMSNew(string sMobileNo, string sSMS, string sSenderID, string sConfigSenderId, string sTemplateID)
        //{
        //    try
        //    {
        //        string strUsername = Convert.ToString(ConfigurationSettings.AppSettings["VENDOR_USERNAME1"]);
        //        string strPassword = Convert.ToString(ConfigurationSettings.AppSettings["VENDOR_PASS1"]);
        //        string strLink = Convert.ToString(ConfigurationSettings.AppSettings["VENDOR_LINK1"]);
        //        //  string sSenderfromConfig = Convert.ToString(ConfigurationSettings.AppSettings["SenderIDFromConfig1"]);
        //        string sAPIKey = Convert.ToString(ConfigurationSettings.AppSettings["APIKEY1"]);
        //        string IsSenderIdinConfig = Convert.ToString(ConfigurationSettings.AppSettings["SenderIDFromConfig1"]);

        //        if (sSMS.Contains("#"))
        //        {
        //            sSMS = sSMS.Replace("#", "%23");
        //        }
        //        if (sSMS.Contains("&"))
        //        {
        //            sSMS = sSMS.Replace("&", "%26");
        //        }
        //        if (IsSenderIdinConfig == "ON")
        //        {
        //            sSenderID = sConfigSenderId;
        //        }
        //        string strLog = "sendSMS : '~' Mobile No : " + sMobileNo + ", Subject : " + sSMS + "";

        //        string baseurl = strLink + "/v3/api.php?username=" + strUsername + "&apikey=" + sAPIKey + "&senderid=";
        //        baseurl += "" + sSenderID + " &templateid=" + sTemplateID + "&mobile=" + System.Uri.EscapeUriString(sMobileNo) + "";
        //        baseurl += "&message=" + System.Uri.EscapeUriString(sSMS).Replace("+", "%2B") + "";
        //        String result = GetPageContent(baseurl);
        //        Console.WriteLine(sMobileNo + "-" + result);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return "";
        //    }
        //}
        //static string GetPageContent(string FullUri)
        //{
        //    HttpWebRequest Request;
        //    StreamReader ResponseReader;
        //    Request = ((HttpWebRequest)(WebRequest.Create(FullUri)));
        //    ResponseReader = new StreamReader(Request.GetResponse().GetResponseStream());
        //    return ResponseReader.ReadToEnd();
        //}
        public void SendSMStoRole(clsApproval objApproval, string sPreviousBoId)
        {
            string QryKey = string.Empty;
            try
            {
                if (Convert.ToString(ConfigurationManager.AppSettings["SendSMS"]).ToUpper().Equals("ON"))
                {
                    string strQry = string.Empty;
                    DataTable dt = new DataTable();
                    string sOfficeCode = string.Empty;
                    Division = Convert.ToInt32(ConfigurationManager.AppSettings["Division_code"]);
                    SubDivision = Convert.ToInt32(ConfigurationManager.AppSettings["SubDiv_code"]);
                    Section = Convert.ToInt32(ConfigurationManager.AppSettings["Section_code"]);
                    clsCommunication objcomm = new clsCommunication();
                    string sFullName = string.Empty;
                    string sMobileNo = string.Empty;

                    // Create another table to store office code length with respect to the roles , take the length and substring the office code
                    if (objApproval.sRoleId == "1" || objApproval.sRoleId == "26")
                    {
                        sOfficeCode = objApproval.sRefOfficeCode.Substring(0, SubDivision);
                    }
                    if (objApproval.sRoleId == "2" || objApproval.sRoleId == "3" || objApproval.sRoleId == "5" || objApproval.sRoleId == "6" || objApproval.sRoleId == "7" || objApproval.sRoleId == "21" || objApproval.sRoleId == "24")
                    {
                        sOfficeCode = objApproval.sRefOfficeCode.Substring(0, Division);
                    }

                    if (objApproval.sRoleId == "4")
                    {
                        sOfficeCode = objApproval.sRefOfficeCode;
                    }

                    if (sOfficeCode != "")
                    {
                        strQry = "SELECT \"US_FULL_NAME\",\"US_MOBILE_NO\",\"US_EMAIL\" FROM \"TBLUSER\" WHERE \"US_ROLE_ID\" IN (" + objApproval.sRoleId + ") AND \"US_OFFICE_CODE\" ='" + sOfficeCode + "'";
                        dt = objcon.FetchDataTable(strQry);

                        //NpgsqlCommand cmd = new NpgsqlCommand("proc_get_tbluser_details_for_sendsmstorole");
                        //cmd.Parameters.AddWithValue("p_us_role_id", Convert.ToString(objApproval.sRoleId ?? ""));
                        //cmd.Parameters.AddWithValue("p_us_office_code", Convert.ToString(sOfficeCode ?? ""));
                        //dt = objcon.FetchDataTable(cmd);

                        string sSMSText = string.Empty;
                        string sQry = string.Empty;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sFullName = Convert.ToString(dt.Rows[i]["US_FULL_NAME"]);
                            sMobileNo = Convert.ToString(dt.Rows[i]["US_MOBILE_NO"]);

                            //Failure Entry
                            if (objApproval.sBOId == "9")
                            {
                                if (objApproval.sApproveStatus == "3")
                                {
                                    string sResult = string.Empty;
                                    // NpgsqlCommand = new NpgsqlCommand();
                                    strQry = "SELECT \"BO_NAME\" || '~' || \"RO_NAME\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLBUSINESSOBJECT\",\"TBLUSER\",\"TBLROLES\" WHERE \"WO_BO_ID\"=\"BO_ID\" AND \"WO_CR_BY\"=\"US_ID\" AND \"RO_ID\"=\"US_ROLE_ID\" ";
                                    strQry += " AND \"WO_ID\" ='" + Convert.ToInt32(objApproval.sWFObjectId) + "' AND \"WO_BO_ID\" ='" + Convert.ToInt32(objApproval.sBOId) + "'";
                                    //  NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                                    //NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objApproval.sBOId));
                                    sResult = objcon.get_value(strQry);


                                    //QryKey = "GET_BO_NAME_AND_RO_NAME";
                                    //NpgsqlCommand cmd_BO_NAME_AND_RO_NAME = new NpgsqlCommand("fetch_getvalue_for_sendsmstorole_in_clsapproval");
                                    //cmd_BO_NAME_AND_RO_NAME.Parameters.AddWithValue("p_key", QryKey);
                                    //cmd_BO_NAME_AND_RO_NAME.Parameters.AddWithValue("p_value_1", Convert.ToString(objApproval.sWFObjectId ?? ""));
                                    //cmd_BO_NAME_AND_RO_NAME.Parameters.AddWithValue("p_value_2", Convert.ToString(objApproval.sBOId ?? ""));
                                    //cmd_BO_NAME_AND_RO_NAME.Parameters.AddWithValue("p_value_3", "");
                                    //cmd_BO_NAME_AND_RO_NAME.Parameters.AddWithValue("p_value_4", "");
                                    //sResult = ObjBasCon.StringGetValue(cmd_BO_NAME_AND_RO_NAME);

                                    objcomm.sSMSkey = "SMStoReject";
                                    objcomm = objcomm.GetsmsTempalte(objcomm);
                                    if (objcomm.sSMSTemplate != null)
                                    {
                                        sSMSText = String.Format(objcomm.sSMSTemplate, sResult.Split('~').GetValue(0).ToString(),
                                        objApproval.sDataReferenceId, sResult.Split('~').GetValue(1).ToString());
                                    }
                                }
                                else
                                {
                                    objcomm.sSMSkey = "SMStoFailureCreate";
                                    objcomm = objcomm.GetsmsTempalte(objcomm);
                                    if (objcomm.sSMSTemplate != null)
                                    {
                                        sSMSText = String.Format(objcomm.sSMSTemplate,
                                        objApproval.sDataReferenceId);
                                    }
                                }

                            }

                            //Work Order Entry
                            if (objApproval.sBOId == "11")
                            {

                                if (sPreviousBoId == "9")
                                {
                                    objcomm.sSMSkey = "SMStoFailure";
                                    objcomm = objcomm.GetsmsTempalte(objcomm);
                                    if (objcomm.sSMSTemplate != null)
                                    {
                                        sSMSText = String.Format(objcomm.sSMSTemplate,
                                          objApproval.sDataReferenceId);
                                    }
                                }
                                else
                                {
                                    string sResult = string.Empty;


                                    NpgsqlCommand = new NpgsqlCommand();
                                    sQry = "SELECT \"DF_DTC_CODE\" || '~' || \"DT_NAME\" || '~' || \"RO_NAME\" || '~' || \"BO_NAME\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\",\"TBLDTCMAST\",\"TBLUSER\",\"TBLROLES\",\"TBLBUSINESSOBJECT\" WHERE ";
                                    sQry += " \"DF_ID\" =:sDataReferenceId AND \"WO_DATA_ID\"=:sDataReferenceId1 AND \"DF_DTC_CODE\"=\"DT_CODE\" AND ";
                                    //sQry += " \"WO_NEXT_ROLE\" ='" + objApproval.sRoleId + "' AND \"WO_ID\"='" + objApproval.sWFObjectId + "' AND \"WO_CR_BY\"=\"US_ID\" AND \"RO_ID\"=\"US_ROLE_ID\" AND \"WO_BO_ID\"=\"BO_ID\" ";
                                    sQry += " \"WO_ID\"=:sWFObjectId AND \"WO_CR_BY\"=\"US_ID\" AND \"RO_ID\"=\"US_ROLE_ID\" AND \"WO_BO_ID\"=\"BO_ID\" ";
                                    NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", Convert.ToInt32(objApproval.sDataReferenceId));
                                    NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId1", objApproval.sDataReferenceId);
                                    NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                                    // NpgsqlCommand.Parameters.AddWithValue("sBOId", objApproval.sBOId);
                                    sResult = objcon.get_value(sQry, NpgsqlCommand);

                                    //if (sResult != "")
                                    //{
                                    //    sSMSText = String.Format(Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SMStoWorkOrderCreate"]),
                                    //        sResult.Split('~').GetValue(0).ToString(), sResult.Split('~').GetValue(1).ToString());
                                    //}


                                    //QryKey = "GET_DF_DTC_CODE_AND_DT_NAME_AND_RO_NAME_AND_BO_NAME";
                                    //NpgsqlCommand cmd_DF_DTC_CODE_AND_DT_NAME_AND_RO_NAME_AND_BO_NAME = new NpgsqlCommand("fetch_getvalue_for_sendsmstorole_in_clsapproval");
                                    //cmd_DF_DTC_CODE_AND_DT_NAME_AND_RO_NAME_AND_BO_NAME.Parameters.AddWithValue("p_key", QryKey);
                                    //cmd_DF_DTC_CODE_AND_DT_NAME_AND_RO_NAME_AND_BO_NAME.Parameters.AddWithValue("p_value_1", Convert.ToString(objApproval.sDataReferenceId ?? ""));
                                    //cmd_DF_DTC_CODE_AND_DT_NAME_AND_RO_NAME_AND_BO_NAME.Parameters.AddWithValue("p_value_2", Convert.ToString(objApproval.sWFObjectId ?? ""));
                                    //cmd_DF_DTC_CODE_AND_DT_NAME_AND_RO_NAME_AND_BO_NAME.Parameters.AddWithValue("p_value_3", "");
                                    //cmd_DF_DTC_CODE_AND_DT_NAME_AND_RO_NAME_AND_BO_NAME.Parameters.AddWithValue("p_value_4", "");
                                    //sResult = ObjBasCon.StringGetValue(cmd_DF_DTC_CODE_AND_DT_NAME_AND_RO_NAME_AND_BO_NAME);


                                    if (objApproval.sRoleId == "2")
                                    {
                                        objcomm.sSMSkey = "SMStoWorkOrderCreate";
                                        objcomm = objcomm.GetsmsTempalte(objcomm);
                                        if (objcomm.sSMSTemplate != null)
                                        {
                                            sSMSText = String.Format(objcomm.sSMSTemplate,
                                                        sResult.Split('~').GetValue(0).ToString(), sResult.Split('~').GetValue(1).ToString());
                                        }
                                    }
                                    else
                                    {
                                        if (objApproval.sApproveStatus == "3")
                                        {
                                            NpgsqlCommand = new NpgsqlCommand();
                                            sQry = "SELECT \"DF_DTC_CODE\" || '~' || \"DT_NAME\" || '~' || \"RO_NAME\" || '~' || \"BO_NAME\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\",\"TBLDTCMAST\",\"TBLUSER\",\"TBLROLES\",\"TBLBUSINESSOBJECT\" WHERE ";
                                            sQry += " \"DF_ID\" =:sDataReferenceId AND \"WO_DATA_ID\" =:sDataReferenceId12 AND \"DF_DTC_CODE\"=\"DT_CODE\" AND ";
                                            sQry += " \"WO_NEXT_ROLE\" =:sRoleId AND \"WO_CR_BY\"=\"US_ID\" AND \"RO_ID\"=\"US_ROLE_ID\" AND \"WO_BO_ID\"=\"BO_ID\" AND \"WO_ID\"=:sWFObjectId";
                                            NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", Convert.ToInt32(objApproval.sDataReferenceId));
                                            NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId12", objApproval.sDataReferenceId);
                                            NpgsqlCommand.Parameters.AddWithValue("sRoleId", Convert.ToInt32(objApproval.sRoleId));
                                            NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                                            sResult = objcon.get_value(sQry, NpgsqlCommand);

                                            //QryKey = "GET_DF_DTC_CODE_AND_DT_NAME_AND_RO_NAME_AND_BO_NAME_FOR_SENDSMSTOROLE";
                                            //NpgsqlCommand cmd_FOR_SENDSMSTOROLE = new NpgsqlCommand("fetch_getvalue_for_sendsmstorole_in_clsapproval");
                                            //cmd_FOR_SENDSMSTOROLE.Parameters.AddWithValue("p_key", QryKey);
                                            //cmd_FOR_SENDSMSTOROLE.Parameters.AddWithValue("p_value_1", Convert.ToString(objApproval.sDataReferenceId ?? ""));
                                            //cmd_FOR_SENDSMSTOROLE.Parameters.AddWithValue("p_value_2", Convert.ToString(objApproval.sRoleId ?? ""));
                                            //cmd_FOR_SENDSMSTOROLE.Parameters.AddWithValue("p_value_3", Convert.ToString(objApproval.sWFObjectId ?? ""));
                                            //cmd_FOR_SENDSMSTOROLE.Parameters.AddWithValue("p_value_4", "");
                                            //sResult = ObjBasCon.StringGetValue(cmd_FOR_SENDSMSTOROLE);

                                            objcomm.sSMSkey = "SMStoReject";
                                            objcomm = objcomm.GetsmsTempalte(objcomm);
                                            if (objcomm.sSMSTemplate != null)
                                            {
                                                sSMSText = String.Format(objcomm.sSMSTemplate, sResult.Split('~').GetValue(3).ToString(),
                                                        sResult.Split('~').GetValue(0).ToString(), sResult.Split('~').GetValue(2).ToString());
                                            }
                                        }
                                        else
                                        {
                                            objcomm.sSMSkey = "SMStoWorkOrderApprover";
                                            objcomm = objcomm.GetsmsTempalte(objcomm);
                                            if (objcomm.sSMSTemplate != null)
                                            {
                                                sSMSText = String.Format(objcomm.sSMSTemplate,
                                                        sResult.Split('~').GetValue(0).ToString(), sResult.Split('~').GetValue(1).ToString(), sResult.Split('~').GetValue(2).ToString());
                                            }
                                        }

                                    }
                                }

                            }

                            // Indent
                            if (objApproval.sBOId == "12")
                            {
                                if (sPreviousBoId == "11")
                                {
                                    NpgsqlCommand = new NpgsqlCommand();
                                    sQry = "SELECT \"DT_CODE\",\"DT_NAME\",\"WO_NO\" FROM \"TBLWORKORDER\",\"TBLDTCFAILURE\",\"TBLDTCMAST\" WHERE \"WO_SLNO\"=:sNewRecordId AND \"WO_DF_ID\"=\"DF_ID\" AND \"DF_DTC_CODE\"=\"DT_CODE\"";
                                    NpgsqlCommand.Parameters.AddWithValue("sNewRecordId", Convert.ToInt32(objApproval.sNewRecordId));
                                    dt = objcon.FetchDataTable(sQry, NpgsqlCommand);

                                    //NpgsqlCommand cmd_dt_code_and_dt_name_and_wo_no_details = new NpgsqlCommand("proc_get_dt_code_and_dt_name_and_wo_no_for_sendsmstorole");
                                    //cmd_dt_code_and_dt_name_and_wo_no_details.Parameters.AddWithValue("p_newrecordid", Convert.ToString(objApproval.sRoleId ?? ""));
                                    //dt = objcon.FetchDataTable(cmd_dt_code_and_dt_name_and_wo_no_details);


                                    if (dt.Rows.Count > 0)
                                    {
                                        objcomm.sSMSkey = "SMStoWorkOrder";
                                        objcomm = objcomm.GetsmsTempalte(objcomm);
                                        if (objcomm.sSMSTemplate != null)
                                        {
                                            sSMSText = String.Format(objcomm.sSMSTemplate,
                                          Convert.ToString(dt.Rows[0]["DT_CODE"]), Convert.ToString(dt.Rows[0]["WO_NO"]), Convert.ToString(dt.Rows[0]["DT_NAME"]));
                                        }
                                    }

                                }
                                else
                                {
                                    if (objApproval.sApproveStatus == "3")
                                    {
                                        string sResult = string.Empty;

                                        NpgsqlCommand = new NpgsqlCommand();
                                        sQry = "SELECT \"DF_DTC_CODE\" || '~' || \"DT_NAME\" || '~' || \"RO_NAME\" || '~' || \"BO_NAME\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\",\"TBLDTCMAST\",\"TBLUSER\",\"TBLROLES\",\"TBLBUSINESSOBJECT\" WHERE ";
                                        sQry += " \"DF_ID\" =:sDataReferenceId AND \"WO_DATA_ID\"=:sDataReferenceId1 AND \"DF_DTC_CODE\"=\"DT_CODE\" AND ";
                                        sQry += " \"WO_NEXT_ROLE\" =:sRoleId AND \"WO_CR_BY\"=\"US_ID\" AND \"RO_ID\"=\"US_ROLE_ID\" AND \"WO_BO_ID\"=\"BO_ID\" AND \"WO_ID\"=:sWFObjectId";
                                        NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", Convert.ToInt32(objApproval.sDataReferenceId));
                                        NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId1", objApproval.sDataReferenceId);
                                        NpgsqlCommand.Parameters.AddWithValue("sRoleId", Convert.ToInt32(objApproval.sRoleId));
                                        NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                                        sResult = objcon.get_value(sQry, NpgsqlCommand);


                                        //QryKey = "GET_DF_DTC_CODE_AND_DT_NAME_AND_RO_NAME_AND_BO_NAME_FOR_SENDSMSTOROLE";
                                        //NpgsqlCommand cmd_FOR_SENDSMSTOROLE = new NpgsqlCommand("fetch_getvalue_for_sendsmstorole_in_clsapproval");
                                        //cmd_FOR_SENDSMSTOROLE.Parameters.AddWithValue("p_key", QryKey);
                                        //cmd_FOR_SENDSMSTOROLE.Parameters.AddWithValue("p_value_1", Convert.ToString(objApproval.sDataReferenceId ?? ""));
                                        //cmd_FOR_SENDSMSTOROLE.Parameters.AddWithValue("p_value_2", Convert.ToString(objApproval.sRoleId ?? ""));
                                        //cmd_FOR_SENDSMSTOROLE.Parameters.AddWithValue("p_value_3", Convert.ToString(objApproval.sWFObjectId ?? ""));
                                        //cmd_FOR_SENDSMSTOROLE.Parameters.AddWithValue("p_value_4", "");
                                        //sResult = ObjBasCon.StringGetValue(cmd_FOR_SENDSMSTOROLE);


                                        objcomm.sSMSkey = "SMStoReject";
                                        objcomm = objcomm.GetsmsTempalte(objcomm);
                                        if (objcomm.sSMSTemplate != null)
                                        {
                                            sSMSText = String.Format(objcomm.sSMSTemplate, sResult.Split('~').GetValue(3).ToString(),
                                                    sResult.Split('~').GetValue(0).ToString(), sResult.Split('~').GetValue(2).ToString());
                                        }
                                    }
                                    else
                                    {
                                        NpgsqlCommand = new NpgsqlCommand();
                                        sQry = "SELECT \"DT_CODE\",\"DT_NAME\",\"WO_NO\" FROM \"TBLWORKORDER\",\"TBLDTCFAILURE\",\"TBLDTCMAST\" WHERE \"WO_SLNO\" =:sDataReferenceId AND \"WO_DF_ID\"=\"DF_ID\" AND \"DF_DTC_CODE\"=\"DT_CODE\" ";
                                        NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", Convert.ToInt32(objApproval.sDataReferenceId));
                                        dt = objcon.FetchDataTable(sQry, NpgsqlCommand);

                                        //NpgsqlCommand cmd_dt_code_and_dt_name_and_wo_no_details = new NpgsqlCommand("proc_get_dt_code_and_dt_name_and_wo_no_for_sendsmstorole");
                                        //cmd_dt_code_and_dt_name_and_wo_no_details.Parameters.AddWithValue("p_newrecordid", Convert.ToString(objApproval.sDataReferenceId ?? ""));
                                        //dt = objcon.FetchDataTable(cmd_dt_code_and_dt_name_and_wo_no_details);

                                        if (dt.Rows.Count > 0)
                                        {
                                            objcomm.sSMSkey = "SMStoIndentCreate";
                                            if (objcomm.sSMSTemplate != null)
                                            {
                                                objcomm = objcomm.GetsmsTempalte(objcomm);
                                                sSMSText = String.Format(objcomm.sSMSTemplate,
                                                  Convert.ToString(dt.Rows[0]["DT_CODE"]), Convert.ToString(dt.Rows[0]["WO_NO"]), Convert.ToString(dt.Rows[0]["DT_NAME"]));
                                            }
                                        }
                                    }


                                }
                            }

                            // Invoice Creation Approval
                            if (objApproval.sBOId == "29")
                            {
                                if (sPreviousBoId == "12")
                                {
                                    NpgsqlCommand = new NpgsqlCommand();
                                    sQry = "SELECT \"DT_CODE\",\"DT_NAME\",\"WO_NO\",\"TI_INDENT_NO\" FROM \"TBLWORKORDER\",\"TBLDTCFAILURE\",\"TBLDTCMAST\",\"TBLINDENT\" WHERE ";
                                    sQry += " \"TI_ID\"=:sNewRecordId AND \"WO_DF_ID\"=\"DF_ID\" AND \"DF_DTC_CODE\"=\"DT_CODE\" AND \"WO_SLNO\"=\"TI_WO_SLNO\"";
                                    NpgsqlCommand.Parameters.AddWithValue("sNewRecordId", Convert.ToInt32(objApproval.sNewRecordId));
                                    dt = objcon.FetchDataTable(sQry, NpgsqlCommand);


                                    if (dt.Rows.Count > 0)
                                    {
                                        objcomm.sSMSkey = "SMStoIndent";
                                        objcomm = objcomm.GetsmsTempalte(objcomm);
                                        if (objcomm.sSMSTemplate != null)
                                        {
                                            sSMSText = String.Format(objcomm.sSMSTemplate,
                                          Convert.ToString(dt.Rows[0]["DT_CODE"]), Convert.ToString(dt.Rows[0]["WO_NO"]), Convert.ToString(dt.Rows[0]["TI_INDENT_NO"]));
                                        }
                                    }
                                }
                            }

                            // Invoice Creation
                            if (objApproval.sBOId == "13")
                            {
                                if (sPreviousBoId == "29")
                                {
                                    NpgsqlCommand = new NpgsqlCommand();
                                    sQry = "SELECT \"DT_CODE\",\"DT_NAME\",\"WO_NO\",\"TI_INDENT_NO\" FROM \"TBLWORKORDER\",\"TBLDTCFAILURE\",\"TBLDTCMAST\",\"TBLINDENT\" WHERE ";
                                    sQry += " \"TI_ID\"=:sRecordId AND \"WO_DF_ID\"=\"DF_ID\" AND \"DF_DTC_CODE\"=\"DT_CODE\" AND \"WO_SLNO\"=\"TI_WO_SLNO\" ";
                                    NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objApproval.sRecordId));
                                    dt = objcon.FetchDataTable(sQry, NpgsqlCommand);
                                    if (dt.Rows.Count > 0)
                                    {
                                        objcomm.sSMSkey = "SMStoIndent";
                                        objcomm = objcomm.GetsmsTempalte(objcomm);
                                        if (objcomm.sSMSTemplate != null)
                                        {
                                            sSMSText = String.Format(objcomm.sSMSTemplate,
                                          Convert.ToString(dt.Rows[0]["DT_CODE"]), Convert.ToString(dt.Rows[0]["WO_NO"]), Convert.ToString(dt.Rows[0]["TI_INDENT_NO"]));
                                        }
                                    }
                                }

                            }

                            //Decommission
                            if (objApproval.sBOId == "14")
                            {
                                if (sPreviousBoId == "13")
                                {
                                    NpgsqlCommand = new NpgsqlCommand();
                                    sQry = "SELECT \"TI_INDENT_NO\",\"IN_INV_NO\" FROM \"TBLINDENT\",\"TBLDTCINVOICE\" WHERE \"IN_TI_NO\"=\"TI_ID\" AND \"TI_ID\" =:sDataReferenceId";
                                    NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", Convert.ToInt32(objApproval.sDataReferenceId));
                                    dt = objcon.FetchDataTable(sQry, NpgsqlCommand);
                                    if (dt.Rows.Count > 0)
                                    {
                                        objcomm.sSMSkey = "SMStoInvoice";
                                        objcomm = objcomm.GetsmsTempalte(objcomm);
                                        if (objcomm.sSMSTemplate != null)
                                        {
                                            sSMSText = String.Format(objcomm.sSMSTemplate,
                                           // sSMSText = String.Format(Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SMStoInvoice"]),
                                           Convert.ToString(dt.Rows[0]["TI_INDENT_NO"]), Convert.ToString(dt.Rows[0]["IN_INV_NO"]));
                                        }
                                    }

                                }
                                else
                                {
                                    if (objApproval.sApproveStatus == "3")
                                    {
                                        NpgsqlCommand = new NpgsqlCommand();
                                        sQry = "SELECT \"DF_DTC_CODE\" || '~' || \"DT_NAME\" || '~' || \"RO_NAME\" || '~' || \"BO_NAME\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\",\"TBLDTCMAST\",\"TBLUSER\",\"TBLROLES\",\"TBLBUSINESSOBJECT\" WHERE ";
                                        sQry += " \"DF_DTC_CODE\" =:sDataReferenceId AND \"WO_DATA_ID\" =:sDataReferenceId1 AND \"DF_DTC_CODE\"=\"DT_CODE\" AND ";
                                        sQry += " \"WO_NEXT_ROLE\" =:sRoleId AND \"WO_CR_BY\" = \"US_ID\" AND \"RO_ID\"=\"US_ROLE_ID\" AND \"WO_BO_ID\"=\"BO_ID\" AND \"WO_ID\" =:sWFObjectId";
                                        NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", objApproval.sDataReferenceId);
                                        NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId1", objApproval.sDataReferenceId);
                                        NpgsqlCommand.Parameters.AddWithValue("sRoleId", Convert.ToInt32(objApproval.sRoleId));
                                        NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                                        string sResult = objcon.get_value(sQry, NpgsqlCommand);

                                        objcomm.sSMSkey = "SMStoReject";
                                        objcomm = objcomm.GetsmsTempalte(objcomm);
                                        if (objcomm.sSMSTemplate != null)
                                        {
                                            sSMSText = String.Format(objcomm.sSMSTemplate, sResult.Split('~').GetValue(3).ToString(),
                                                    // sSMSText = String.Format(Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SMStoReject"]), sResult.Split('~').GetValue(3).ToString(),
                                                    sResult.Split('~').GetValue(0).ToString(), sResult.Split('~').GetValue(2).ToString());
                                        }
                                    }
                                    else
                                    {
                                        NpgsqlCommand = new NpgsqlCommand();
                                        sQry = "SELECT \"DF_DTC_CODE\",\"DF_EQUIPMENT_ID\" FROM \"TBLDTCFAILURE\" WHERE  \"DF_DTC_CODE\" =:sDataReferenceId ";
                                        NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", objApproval.sDataReferenceId);

                                        dt = objcon.FetchDataTable(sQry, NpgsqlCommand);
                                        if (dt.Rows.Count > 0)
                                        {
                                            objcomm.sSMSkey = "SMStoDecommCreate";
                                            objcomm = objcomm.GetsmsTempalte(objcomm);
                                            if (objcomm.sSMSTemplate != null)
                                            {
                                                sSMSText = String.Format(objcomm.sSMSTemplate,
                                                   //  sSMSText = String.Format(Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SMStoDecommCreate"]),
                                                   Convert.ToString(dt.Rows[0]["DF_DTC_CODE"]), Convert.ToString(dt.Rows[0]["DF_EQUIPMENT_ID"]));
                                            }
                                        }
                                    }
                                }
                            }

                            // RI Acknoldgement
                            if (objApproval.sBOId == "15")
                            {
                                if (sPreviousBoId == "14")
                                {
                                    NpgsqlCommand = new NpgsqlCommand();
                                    sQry = "SELECT \"IN_INV_NO\",\"TR_RI_NO\" FROM \"TBLDTCINVOICE\",\"TBLTCREPLACE\" WHERE \"TR_IN_NO\"=\"IN_NO\" AND \"TR_ID\"=:sNewRecordId";
                                    NpgsqlCommand.Parameters.AddWithValue("sNewRecordId", Convert.ToInt32(objApproval.sNewRecordId));
                                    dt = objcon.FetchDataTable(sQry, NpgsqlCommand);
                                    if (dt.Rows.Count > 0)
                                    {
                                        objcomm.sSMSkey = "SMStoDecomm";
                                        objcomm = objcomm.GetsmsTempalte(objcomm);
                                        if (objcomm.sSMSTemplate != null)
                                        {
                                            sSMSText = String.Format(objcomm.sSMSTemplate,
                                           //  sSMSText = String.Format(Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SMStoDecomm"]),
                                           Convert.ToString(dt.Rows[0]["IN_INV_NO"]), Convert.ToString(dt.Rows[0]["TR_RI_NO"]));
                                        }
                                    }

                                }
                                else
                                {
                                    if (objApproval.sApproveStatus == "3")
                                    {
                                        NpgsqlCommand = new NpgsqlCommand();
                                        sQry = "SELECT \"DF_DTC_CODE\" || '~' || \"DT_NAME\" || '~' || \"RO_NAME\" || '~' || \"BO_NAME\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\",\"TBLDTCMAST\",\"TBLUSER\",\"TBLROLES\",\"TBLBUSINESSOBJECT\" WHERE ";
                                        sQry += " \"DF_DTC_CODE\" =:sDataReferenceId AND \"WO_DATA_ID\" =:sDataReferenceId1 AND \"DF_DTC_CODE\"=\"DT_CODE\" AND ";
                                        sQry += " \"WO_NEXT_ROLE\" =:sRoleId AND \"WO_CR_BY\"=\"US_ID\" AND \"RO_ID\"=\"US_ROLE_ID\" AND \"WO_BO_ID\"=\"BO_ID\" AND \"WO_ID\" =:sWFObjectId";
                                        NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", objApproval.sDataReferenceId);
                                        NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId1", objApproval.sDataReferenceId);
                                        NpgsqlCommand.Parameters.AddWithValue("sRoleId", Convert.ToInt32(objApproval.sRoleId));
                                        NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                                        string sResult = objcon.get_value(sQry, NpgsqlCommand);

                                        objcomm.sSMSkey = "SMStoReject";
                                        objcomm = objcomm.GetsmsTempalte(objcomm);
                                        if (objcomm.sSMSTemplate != null)
                                        {
                                            sSMSText = String.Format(objcomm.sSMSTemplate, sResult.Split('~').GetValue(3).ToString(),
                                                    //   sSMSText = String.Format(Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SMStoReject"]), sResult.Split('~').GetValue(3).ToString(),
                                                    sResult.Split('~').GetValue(0).ToString(), sResult.Split('~').GetValue(2).ToString());
                                        }
                                    }
                                    else
                                    {
                                        NpgsqlCommand = new NpgsqlCommand();
                                        sQry = "SELECT \"TR_RI_NO\",\"DF_EQUIPMENT_ID\" FROM \"TBLTCREPLACE\",\"TBLDTCFAILURE\",\"TBLTCDRAWN\" WHERE \"DF_ID\"=\"TD_DF_ID\" AND ";
                                        sQry += " \"TD_INV_NO\"=\"TR_IN_NO\" AND \"TR_ID\" =:sRecordId";
                                        NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objApproval.sRecordId));
                                        dt = objcon.FetchDataTable(sQry, NpgsqlCommand);
                                        if (dt.Rows.Count > 0)
                                        {
                                            objcomm.sSMSkey = "SMStoRICreate";
                                            objcomm = objcomm.GetsmsTempalte(objcomm);
                                            if (objcomm.sSMSTemplate != null)
                                            {
                                                sSMSText = String.Format(objcomm.sSMSTemplate,
                                               //  sSMSText = String.Format(Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SMStoRICreate"]),
                                               Convert.ToString(dt.Rows[0]["TR_RI_NO"]), Convert.ToString(dt.Rows[0]["DF_EQUIPMENT_ID"]));
                                            }
                                        }
                                    }

                                }
                            }


                            // Completion Report
                            if (objApproval.sBOId == "26")
                            {
                                if (sPreviousBoId == "15")
                                {
                                    NpgsqlCommand = new NpgsqlCommand();
                                    sQry = "SELECT \"TR_RI_NO\",\"DF_EQUIPMENT_ID\" FROM \"TBLTCREPLACE\",\"TBLDTCFAILURE\",\"TBLTCDRAWN\" WHERE \"DF_ID\"=\"TD_DF_ID\" AND ";
                                    sQry += " \"TD_INV_NO\"=\"TR_IN_NO\" AND \"TR_ID\" =:sRecordId";
                                    NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objApproval.sRecordId));
                                    dt = objcon.FetchDataTable(sQry, NpgsqlCommand);
                                    if (dt.Rows.Count > 0)
                                    {
                                        NpgsqlCommand = new NpgsqlCommand();
                                        NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", Convert.ToInt32(objApproval.sOfficeCode.Substring(0, Division)));

                                        string sStoreName = objcon.get_value("SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"SM_OFF_CODE\" =:sOfficeCode", NpgsqlCommand);

                                        objcomm.sSMSkey = "SMStoRI";
                                        objcomm = objcomm.GetsmsTempalte(objcomm);
                                        if (objcomm.sSMSTemplate != null)
                                        {
                                            sSMSText = String.Format(objcomm.sSMSTemplate,
                                           // sSMSText = String.Format(Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SMStoRI"]),
                                           Convert.ToString(dt.Rows[0]["TR_RI_NO"]), Convert.ToString(dt.Rows[0]["DF_EQUIPMENT_ID"]), sStoreName);
                                        }
                                    }
                                }
                                else
                                {
                                    if (objApproval.sApproveStatus == "3")
                                    {
                                        NpgsqlCommand = new NpgsqlCommand();

                                        sQry = "SELECT \"DF_DTC_CODE\" || '~' || \"DT_NAME\" || '~' || \"RO_NAME\" || '~' || \"BO_NAME\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\",\"TBLDTCMAST\",\"TBLUSER\",\"TBLROLES\",\"TBLBUSINESSOBJECT\" WHERE ";
                                        sQry += " \"DF_DTC_CODE\" =:sDataReferenceId AND \"WO_DATA_ID\" =:sDataReferenceId1 AND \"DF_DTC_CODE\"=\"DT_CODE\" AND ";
                                        sQry += " \"WO_NEXT_ROLE\" =:sRoleId AND \"WO_CR_BY\"=\"US_ID\" AND \"RO_ID\"=\"US_ROLE_ID\" AND \"WO_BO_ID\"=\"BO_ID\" AND \"WO_ID\" =:sWFObjectId";
                                        NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", objApproval.sDataReferenceId);
                                        NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId1", objApproval.sDataReferenceId);
                                        NpgsqlCommand.Parameters.AddWithValue("sRoleId", Convert.ToInt32(objApproval.sRoleId));
                                        NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                                        string sResult = objcon.get_value(sQry, NpgsqlCommand);

                                        objcomm.sSMSkey = "SMStoReject";
                                        objcomm = objcomm.GetsmsTempalte(objcomm);
                                        if (objcomm.sSMSTemplate != null)
                                        {
                                            sSMSText = String.Format(objcomm.sSMSTemplate, sResult.Split('~').GetValue(3).ToString(),
                                                    // sSMSText = String.Format(Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SMStoReject"]), sResult.Split('~').GetValue(3).ToString(),
                                                    sResult.Split('~').GetValue(0).ToString(), sResult.Split('~').GetValue(2).ToString());
                                        }
                                    }
                                    else
                                    {
                                        NpgsqlCommand = new NpgsqlCommand();
                                        sQry = "SELECT \"TR_RI_NO\",\"DF_EQUIPMENT_ID\",\"TD_TC_NO\" FROM \"TBLTCREPLACE\",\"TBLDTCFAILURE\",\"TBLTCDRAWN\" WHERE \"DF_ID\"=\"TD_DF_ID\" AND ";
                                        sQry += " \"TD_INV_NO\"=\"TR_IN_NO\" AND \"TR_ID\" =:sRecordId";
                                        NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objApproval.sRecordId));

                                        dt = objcon.FetchDataTable(sQry, NpgsqlCommand);
                                        if (dt.Rows.Count > 0)
                                        {
                                            NpgsqlCommand = new NpgsqlCommand();
                                            NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", Convert.ToInt32(objApproval.sOfficeCode.Substring(0, Division)));
                                            string sStoreName = objcon.get_value("SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"SM_OFF_CODE\" =:sOfficeCode", NpgsqlCommand);

                                            objcomm.sSMSkey = "SMStoCRCreate";
                                            objcomm = objcomm.GetsmsTempalte(objcomm);
                                            if (objcomm.sSMSTemplate != null)
                                            {
                                                sSMSText = String.Format(objcomm.sSMSTemplate,
                                               //sSMSText = String.Format(Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SMStoCRCreate"]),
                                               Convert.ToString(dt.Rows[0]["TR_RI_NO"]), Convert.ToString(dt.Rows[0]["DF_EQUIPMENT_ID"]), Convert.ToString(dt.Rows[0]["TD_TC_NO"]));
                                            }
                                        }
                                    }
                                }
                            }
                            //Estimation
                            if (objApproval.sBOId == "45")
                            {
                                if (sPreviousBoId == "9")
                                {
                                    objcomm.sSMSkey = "SMStoFailure";
                                    objcomm = objcomm.GetsmsTempalte(objcomm);
                                    if (objcomm.sSMSTemplate != null)
                                    {
                                        sSMSText = String.Format(objcomm.sSMSTemplate,
                                          //  sSMSText = String.Format(Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SMStoFailure"]),
                                          objApproval.sDataReferenceId);
                                    }
                                }
                                else
                                {
                                    NpgsqlCommand = new NpgsqlCommand();

                                    sQry = "SELECT \"DF_DTC_CODE\" || '~' || \"DT_NAME\" || '~' || \"RO_NAME\" || '~' || \"BO_NAME\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\",\"TBLDTCMAST\",\"TBLUSER\",\"TBLROLES\",\"TBLBUSINESSOBJECT\" WHERE ";
                                    sQry += " \"DF_ID\" =:sDataReferenceId AND \"WO_DATA_ID\"=:sDataReferenceId1 AND \"DF_DTC_CODE\"=\"DT_CODE\" AND ";
                                    //sQry += " \"WO_NEXT_ROLE\" ='" + objApproval.sRoleId + "' AND \"WO_ID\"='" + objApproval.sWFObjectId + "' AND \"WO_CR_BY\"=\"US_ID\" AND \"RO_ID\"=\"US_ROLE_ID\" AND \"WO_BO_ID\"=\"BO_ID\" ";
                                    sQry += " \"WO_ID\"=:sWFObjectId AND \"WO_CR_BY\"=\"US_ID\" AND \"RO_ID\"=\"US_ROLE_ID\" AND \"WO_BO_ID\"=\"BO_ID\" ";
                                    NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", Convert.ToInt32(objApproval.sDataReferenceId));
                                    NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId1", objApproval.sDataReferenceId);
                                    NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                                    string sResult = objcon.get_value(sQry, NpgsqlCommand);

                                    if (objApproval.sRoleId == "2")
                                    {

                                        objcomm.sSMSkey = "SMStoEstimationCreate";
                                        objcomm = objcomm.GetsmsTempalte(objcomm);
                                        if (objcomm.sSMSTemplate != null)
                                        {
                                            sSMSText = String.Format(objcomm.sSMSTemplate,
                                        // sSMSText = String.Format(Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SMStoEstimationCreate"]),
                                        sResult.Split('~').GetValue(0).ToString(), sResult.Split('~').GetValue(1).ToString());
                                        }
                                    }
                                    else
                                    {
                                        if (objApproval.sApproveStatus == "3")
                                        {
                                            NpgsqlCommand = new NpgsqlCommand();
                                            sQry = "SELECT \"DF_DTC_CODE\" || '~' || \"DT_NAME\" || '~' || \"RO_NAME\" || '~' || \"BO_NAME\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\",\"TBLDTCMAST\",\"TBLUSER\",\"TBLROLES\",\"TBLBUSINESSOBJECT\" WHERE ";
                                            sQry += " \"DF_ID\" =:sDataReferenceId AND \"WO_DATA_ID\" =:sDataReferenceId1 AND \"DF_DTC_CODE\"=\"DT_CODE\" AND ";
                                            sQry += " \"WO_NEXT_ROLE\" =:sRoleId AND \"WO_CR_BY\"=\"US_ID\" AND \"RO_ID\"=\"US_ROLE_ID\" AND \"WO_BO_ID\"=\"BO_ID\" AND \"WO_ID\"=:sWFObjectId";
                                            NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", Convert.ToInt32(objApproval.sDataReferenceId));
                                            NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId1", objApproval.sDataReferenceId);
                                            NpgsqlCommand.Parameters.AddWithValue("sRoleId", Convert.ToInt32(objApproval.sRoleId));
                                            NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                                            sResult = objcon.get_value(sQry, NpgsqlCommand);

                                            objcomm.sSMSkey = "SMStoReject";
                                            objcomm = objcomm.GetsmsTempalte(objcomm);
                                            if (objcomm.sSMSTemplate != null)
                                            {
                                                sSMSText = String.Format(objcomm.sSMSTemplate,
                                                // sSMSText = String.Format(Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SMStoReject"]),
                                                sResult.Split('~').GetValue(3).ToString(),
                                            sResult.Split('~').GetValue(0).ToString(), sResult.Split('~').GetValue(2).ToString());
                                            }
                                        }
                                        else
                                        {
                                            objcomm.sSMSkey = "SMStoEstimateApprover";
                                            objcomm = objcomm.GetsmsTempalte(objcomm);
                                            if (objcomm.sSMSTemplate != null)
                                            {
                                                sSMSText = String.Format(objcomm.sSMSTemplate,
                                            // sSMSText = String.Format(Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SMStoEstimateApprover"]),
                                            sResult.Split('~').GetValue(0).ToString(), sResult.Split('~').GetValue(1).ToString(), sResult.Split('~').GetValue(2).ToString());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (sSMSText == "")
                        {

                        }
                        else
                        {
                            if (objcomm.sSMSTemplateID != null && objcomm.sSMSTemplateID != "")
                            {
                                objcomm.DumpSms(sMobileNo, sSMSText, objcomm.sSMSTemplateID, "WEB");
                            }
                        }
                        //objCommunication.sendSMS(sSMSText, sMobileNo, objApproval.sRoleId);
                    }
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// SendSMS to Role with Begin and Commit 
        /// </summary>
        /// <param name="objApproval"></param>
        /// <param name="sPreviousBoId"></param>
        /// <param name="objDatabse"></param>
        public void SendSMStoRole_Latest(clsApproval objApproval, string sPreviousBoId, DataBseConnection objDatabse)
        {
            try
            {
                if (Convert.ToString(ConfigurationManager.AppSettings["SendSMS"]).ToUpper().Equals("ON"))
                {
                    string strQry = string.Empty;
                    DataTable dt = new DataTable();
                    string sOfficeCode = string.Empty;
                    Division = Convert.ToInt32(ConfigurationManager.AppSettings["Division_code"]);
                    SubDivision = Convert.ToInt32(ConfigurationManager.AppSettings["SubDiv_code"]);
                    Section = Convert.ToInt32(ConfigurationManager.AppSettings["Section_code"]);
                    clsCommunication objcomm = new clsCommunication();
                    string sFullName = string.Empty;
                    string sMobileNo = string.Empty;
                    // Create another table to store office code length with respect to the roles , take the length and substring the office code
                    if (objApproval.sRoleId == "1" || objApproval.sRoleId == "26")
                    {
                        sOfficeCode = objApproval.sRefOfficeCode.Substring(0, SubDivision);
                    }
                    if (objApproval.sRoleId == "2" || objApproval.sRoleId == "3" || objApproval.sRoleId == "5" ||
                        objApproval.sRoleId == "6" || objApproval.sRoleId == "7" || objApproval.sRoleId == "21" ||
                        objApproval.sRoleId == "24")
                    {
                        sOfficeCode = objApproval.sRefOfficeCode.Substring(0, Division);
                    }
                    if (objApproval.sRoleId == "4")
                    {
                        sOfficeCode = objApproval.sRefOfficeCode;
                    }
                    if (sOfficeCode != "")
                    {
                        strQry = " SELECT \"US_FULL_NAME\",\"US_MOBILE_NO\",\"US_EMAIL\" FROM \"TBLUSER\" ";
                        strQry += " WHERE \"US_ROLE_ID\" IN (" + objApproval.sRoleId + ") AND \"US_OFFICE_CODE\" ='" + sOfficeCode + "' ";
                        dt = objDatabse.FetchDataTable(strQry);

                        string sSMSText = string.Empty;
                        string sQry = string.Empty;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sFullName = Convert.ToString(dt.Rows[i]["US_FULL_NAME"]);
                            sMobileNo = Convert.ToString(dt.Rows[i]["US_MOBILE_NO"]);
                            if (objApproval.sBOId == "9")   //Failure Entry
                            {
                                if (objApproval.sApproveStatus == "3")
                                {
                                    // NpgsqlCommand = new NpgsqlCommand();
                                    strQry = " SELECT \"BO_NAME\" || '~' || \"RO_NAME\" FROM \"TBLWORKFLOWOBJECTS\",\"TBLBUSINESSOBJECT\", ";
                                    strQry += " \"TBLUSER\",\"TBLROLES\" WHERE \"WO_BO_ID\"=\"BO_ID\" AND \"WO_CR_BY\"=\"US_ID\" ";
                                    strQry += " AND \"RO_ID\"=\"US_ROLE_ID\" AND \"WO_ID\" ='" + Convert.ToInt32(objApproval.sWFObjectId) + "' ";
                                    strQry += " AND \"WO_BO_ID\" ='" + Convert.ToInt32(objApproval.sBOId) + "' ";
                                    //  NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                                    //NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objApproval.sBOId));
                                    string sResult = objDatabse.get_value(strQry);

                                    objcomm.sSMSkey = "SMStoReject";
                                    objcomm = objcomm.GetsmsTempalte_Latest(objcomm, objDatabse);
                                    if (objcomm.sSMSTemplate != null)
                                    {
                                        sSMSText = String.Format(
                                            objcomm.sSMSTemplate,
                                            sResult.Split('~').GetValue(0).ToString(),
                                            objApproval.sDataReferenceId,
                                            sResult.Split('~').GetValue(1).ToString()
                                            );
                                    }
                                }
                                else
                                {
                                    objcomm.sSMSkey = "SMStoFailureCreate";
                                    objcomm = objcomm.GetsmsTempalte_Latest(objcomm, objDatabse);
                                    if (objcomm.sSMSTemplate != null)
                                    {
                                        sSMSText = String.Format(
                                            objcomm.sSMSTemplate,
                                            objApproval.sDataReferenceId
                                            );
                                    }
                                }
                            }
                            if (objApproval.sBOId == "11")  //Work Order Entry
                            {

                                if (sPreviousBoId == "9")
                                {
                                    objcomm.sSMSkey = "SMStoFailure";
                                    objcomm = objcomm.GetsmsTempalte_Latest(objcomm, objDatabse);
                                    if (objcomm.sSMSTemplate != null)
                                    {
                                        sSMSText = String.Format(objcomm.sSMSTemplate,
                                          objApproval.sDataReferenceId);
                                    }
                                }
                                else
                                {
                                    NpgsqlCommand = new NpgsqlCommand();
                                    sQry = " SELECT \"DF_DTC_CODE\" || '~' || \"DT_NAME\" || '~' || \"RO_NAME\" || '~' || \"BO_NAME\" ";
                                    sQry += " FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\",\"TBLDTCMAST\",\"TBLUSER\",\"TBLROLES\", ";
                                    sQry += " \"TBLBUSINESSOBJECT\" WHERE \"DF_ID\" =:sDataReferenceId AND \"WO_DATA_ID\"=:sDataReferenceId1 ";
                                    sQry += " AND \"DF_DTC_CODE\"=\"DT_CODE\" AND \"WO_ID\"=:sWFObjectId AND \"WO_CR_BY\"=\"US_ID\" ";
                                    //sQry += " \"WO_NEXT_ROLE\" ='" + objApproval.sRoleId + "' AND \"WO_ID\"='" + objApproval.sWFObjectId + "' AND \"WO_CR_BY\"=\"US_ID\" AND \"RO_ID\"=\"US_ROLE_ID\" AND \"WO_BO_ID\"=\"BO_ID\" ";
                                    sQry += " AND \"RO_ID\"=\"US_ROLE_ID\" AND \"WO_BO_ID\"=\"BO_ID\" ";
                                    NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", Convert.ToInt32(objApproval.sDataReferenceId));
                                    NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId1", objApproval.sDataReferenceId);
                                    NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                                    // NpgsqlCommand.Parameters.AddWithValue("sBOId", objApproval.sBOId);
                                    string sResult = objDatabse.get_value(sQry, NpgsqlCommand);

                                    //if (sResult != "")
                                    //{
                                    //    sSMSText = String.Format(Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SMStoWorkOrderCreate"]),
                                    //        sResult.Split('~').GetValue(0).ToString(), sResult.Split('~').GetValue(1).ToString());
                                    //}

                                    if (objApproval.sRoleId == "2")
                                    {
                                        objcomm.sSMSkey = "SMStoWorkOrderCreate";
                                        objcomm = objcomm.GetsmsTempalte_Latest(objcomm, objDatabse);
                                        if (objcomm.sSMSTemplate != null)
                                        {
                                            sSMSText = String.Format(
                                                objcomm.sSMSTemplate,
                                                sResult.Split('~').GetValue(0).ToString(),
                                                sResult.Split('~').GetValue(1).ToString()
                                                );
                                        }
                                    }
                                    else
                                    {
                                        if (objApproval.sApproveStatus == "3")
                                        {
                                            NpgsqlCommand = new NpgsqlCommand();
                                            sQry = " SELECT \"DF_DTC_CODE\" || '~' || \"DT_NAME\" || '~' || \"RO_NAME\" || '~' || \"BO_NAME\" ";
                                            sQry += " FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\",\"TBLDTCMAST\",\"TBLUSER\",\"TBLROLES\", ";
                                            sQry += " \"TBLBUSINESSOBJECT\" WHERE \"DF_ID\" =:sDataReferenceId AND \"WO_DATA_ID\" =:sDataReferenceId12 ";
                                            sQry += " AND \"DF_DTC_CODE\"=\"DT_CODE\" AND \"WO_NEXT_ROLE\" =:sRoleId AND \"WO_CR_BY\"=\"US_ID\" ";
                                            sQry += " AND \"RO_ID\"=\"US_ROLE_ID\" AND \"WO_BO_ID\"=\"BO_ID\" AND \"WO_ID\"=:sWFObjectId ";
                                            NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", Convert.ToInt32(objApproval.sDataReferenceId));
                                            NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId12", objApproval.sDataReferenceId);
                                            NpgsqlCommand.Parameters.AddWithValue("sRoleId", Convert.ToInt32(objApproval.sRoleId));
                                            NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                                            sResult = objDatabse.get_value(sQry, NpgsqlCommand);

                                            objcomm.sSMSkey = "SMStoReject";
                                            objcomm = objcomm.GetsmsTempalte_Latest(objcomm, objDatabse);
                                            if (objcomm.sSMSTemplate != null)
                                            {
                                                sSMSText = String.Format(
                                                    objcomm.sSMSTemplate,
                                                    sResult.Split('~').GetValue(3).ToString(),
                                                    sResult.Split('~').GetValue(0).ToString(),
                                                    sResult.Split('~').GetValue(2).ToString()
                                                    );
                                            }
                                        }
                                        else
                                        {
                                            objcomm.sSMSkey = "SMStoWorkOrderApprover";
                                            objcomm = objcomm.GetsmsTempalte_Latest(objcomm, objDatabse);
                                            if (objcomm.sSMSTemplate != null)
                                            {
                                                sSMSText = String.Format(
                                                    objcomm.sSMSTemplate,
                                                    sResult.Split('~').GetValue(0).ToString(),
                                                    sResult.Split('~').GetValue(1).ToString(),
                                                    sResult.Split('~').GetValue(2).ToString()
                                                    );
                                            }
                                        }

                                    }
                                }
                            }
                            if (objApproval.sBOId == "12")  // Indent
                            {
                                if (sPreviousBoId == "11")
                                {
                                    NpgsqlCommand = new NpgsqlCommand();
                                    sQry = " SELECT \"DT_CODE\",\"DT_NAME\",\"WO_NO\" FROM \"TBLWORKORDER\",\"TBLDTCFAILURE\",\"TBLDTCMAST\" ";
                                    sQry += " WHERE \"WO_SLNO\"=:sNewRecordId AND \"WO_DF_ID\"=\"DF_ID\" AND \"DF_DTC_CODE\"=\"DT_CODE\" ";
                                    NpgsqlCommand.Parameters.AddWithValue("sNewRecordId", Convert.ToInt32(objApproval.sNewRecordId));
                                    dt = objDatabse.FetchDataTable(sQry, NpgsqlCommand);
                                    if (dt.Rows.Count > 0)
                                    {
                                        objcomm.sSMSkey = "SMStoWorkOrder";
                                        objcomm = objcomm.GetsmsTempalte_Latest(objcomm, objDatabse);
                                        if (objcomm.sSMSTemplate != null)
                                        {
                                            sSMSText = String.Format(
                                                objcomm.sSMSTemplate,
                                                Convert.ToString(dt.Rows[0]["DT_CODE"]),
                                                Convert.ToString(dt.Rows[0]["WO_NO"]),
                                                Convert.ToString(dt.Rows[0]["DT_NAME"])
                                                );
                                        }
                                    }
                                }
                                else
                                {
                                    if (objApproval.sApproveStatus == "3")
                                    {
                                        NpgsqlCommand = new NpgsqlCommand();
                                        sQry = " SELECT \"DF_DTC_CODE\" || '~' || \"DT_NAME\" || '~' || \"RO_NAME\" || '~' || \"BO_NAME\" ";
                                        sQry += " FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\",\"TBLDTCMAST\",\"TBLUSER\",\"TBLROLES\", ";
                                        sQry += " \"TBLBUSINESSOBJECT\" WHERE \"DF_ID\" =:sDataReferenceId AND \"WO_DATA_ID\"=:sDataReferenceId1 ";
                                        sQry += " AND \"DF_DTC_CODE\"=\"DT_CODE\" AND \"WO_NEXT_ROLE\" =:sRoleId AND \"WO_CR_BY\"=\"US_ID\" AND ";
                                        sQry += " \"RO_ID\"=\"US_ROLE_ID\" AND \"WO_BO_ID\"=\"BO_ID\" AND \"WO_ID\"=:sWFObjectId ";
                                        NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", Convert.ToInt32(objApproval.sDataReferenceId));
                                        NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId1", objApproval.sDataReferenceId);
                                        NpgsqlCommand.Parameters.AddWithValue("sRoleId", Convert.ToInt32(objApproval.sRoleId));
                                        NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                                        string sResult = objDatabse.get_value(sQry, NpgsqlCommand);

                                        objcomm.sSMSkey = "SMStoReject";
                                        objcomm = objcomm.GetsmsTempalte_Latest(objcomm, objDatabse);
                                        if (objcomm.sSMSTemplate != null)
                                        {
                                            sSMSText = String.Format(
                                                objcomm.sSMSTemplate,
                                                sResult.Split('~').GetValue(3).ToString(),
                                                sResult.Split('~').GetValue(0).ToString(),
                                                sResult.Split('~').GetValue(2).ToString()
                                                );
                                        }
                                    }
                                    else
                                    {
                                        NpgsqlCommand = new NpgsqlCommand();
                                        sQry = " SELECT \"DT_CODE\",\"DT_NAME\",\"WO_NO\" FROM \"TBLWORKORDER\",\"TBLDTCFAILURE\",\"TBLDTCMAST\" ";
                                        sQry += " WHERE \"WO_SLNO\" =:sDataReferenceId AND \"WO_DF_ID\"=\"DF_ID\" AND \"DF_DTC_CODE\"=\"DT_CODE\" ";
                                        NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", Convert.ToInt32(objApproval.sDataReferenceId));
                                        dt = objDatabse.FetchDataTable(sQry, NpgsqlCommand);
                                        if (dt.Rows.Count > 0)
                                        {
                                            objcomm.sSMSkey = "SMStoIndentCreate";
                                            if (objcomm.sSMSTemplate != null)
                                            {
                                                objcomm = objcomm.GetsmsTempalte_Latest(objcomm, objDatabse);
                                                sSMSText = String.Format(
                                                    objcomm.sSMSTemplate,
                                                    Convert.ToString(dt.Rows[0]["DT_CODE"]),
                                                    Convert.ToString(dt.Rows[0]["WO_NO"]),
                                                    Convert.ToString(dt.Rows[0]["DT_NAME"])
                                                    );
                                            }
                                        }
                                    }
                                }
                            }
                            if (objApproval.sBOId == "29")  // Invoice Creation Approval
                            {
                                if (sPreviousBoId == "12")
                                {
                                    NpgsqlCommand = new NpgsqlCommand();
                                    sQry = " SELECT \"DT_CODE\",\"DT_NAME\",\"WO_NO\",\"TI_INDENT_NO\" FROM \"TBLWORKORDER\", ";
                                    sQry += " \"TBLDTCFAILURE\",\"TBLDTCMAST\",\"TBLINDENT\" WHERE \"TI_ID\"=:sNewRecordId AND ";
                                    sQry += " \"WO_DF_ID\"=\"DF_ID\" AND \"DF_DTC_CODE\"=\"DT_CODE\" AND \"WO_SLNO\"=\"TI_WO_SLNO\" ";
                                    NpgsqlCommand.Parameters.AddWithValue("sNewRecordId", Convert.ToInt32(objApproval.sNewRecordId));
                                    dt = objDatabse.FetchDataTable(sQry, NpgsqlCommand);
                                    if (dt.Rows.Count > 0)
                                    {
                                        objcomm.sSMSkey = "SMStoIndent";
                                        objcomm = objcomm.GetsmsTempalte_Latest(objcomm, objDatabse);
                                        if (objcomm.sSMSTemplate != null)
                                        {
                                            sSMSText = String.Format(objcomm.sSMSTemplate,
                                                Convert.ToString(dt.Rows[0]["DT_CODE"]),
                                                Convert.ToString(dt.Rows[0]["WO_NO"]),
                                                Convert.ToString(dt.Rows[0]["TI_INDENT_NO"])
                                                );
                                        }
                                    }
                                }
                            }
                            if (objApproval.sBOId == "13")  // Invoice Creation
                            {
                                if (sPreviousBoId == "29")
                                {
                                    NpgsqlCommand = new NpgsqlCommand();
                                    sQry = " SELECT \"DT_CODE\",\"DT_NAME\",\"WO_NO\",\"TI_INDENT_NO\" FROM \"TBLWORKORDER\", ";
                                    sQry += " \"TBLDTCFAILURE\",\"TBLDTCMAST\",\"TBLINDENT\" WHERE \"TI_ID\"=:sRecordId AND ";
                                    sQry += " \"WO_DF_ID\"=\"DF_ID\" AND \"DF_DTC_CODE\"=\"DT_CODE\" AND \"WO_SLNO\"=\"TI_WO_SLNO\" ";
                                    NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objApproval.sRecordId));
                                    dt = objDatabse.FetchDataTable(sQry, NpgsqlCommand);
                                    if (dt.Rows.Count > 0)
                                    {
                                        objcomm.sSMSkey = "SMStoIndent";
                                        objcomm = objcomm.GetsmsTempalte_Latest(objcomm, objDatabse);
                                        if (objcomm.sSMSTemplate != null)
                                        {
                                            sSMSText = String.Format(
                                                objcomm.sSMSTemplate,
                                                Convert.ToString(dt.Rows[0]["DT_CODE"]),
                                                Convert.ToString(dt.Rows[0]["WO_NO"]),
                                                Convert.ToString(dt.Rows[0]["TI_INDENT_NO"])
                                                );
                                        }
                                    }
                                }
                            }
                            if (objApproval.sBOId == "14")  // Decommission
                            {
                                if (sPreviousBoId == "13")
                                {
                                    NpgsqlCommand = new NpgsqlCommand();
                                    sQry = " SELECT \"TI_INDENT_NO\",\"IN_INV_NO\" FROM \"TBLINDENT\",\"TBLDTCINVOICE\" ";
                                    sQry += " WHERE \"IN_TI_NO\"=\"TI_ID\" AND \"TI_ID\" =:sDataReferenceId ";
                                    NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", Convert.ToInt32(objApproval.sDataReferenceId));
                                    dt = objDatabse.FetchDataTable(sQry, NpgsqlCommand);
                                    if (dt.Rows.Count > 0)
                                    {
                                        objcomm.sSMSkey = "SMStoInvoice";
                                        objcomm = objcomm.GetsmsTempalte_Latest(objcomm, objDatabse);
                                        if (objcomm.sSMSTemplate != null)
                                        {
                                            // sSMSText = String.Format(Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SMStoInvoice"]),
                                            sSMSText = String.Format(
                                                objcomm.sSMSTemplate,
                                                Convert.ToString(dt.Rows[0]["TI_INDENT_NO"]),
                                                Convert.ToString(dt.Rows[0]["IN_INV_NO"])
                                                );
                                        }
                                    }
                                }
                                else
                                {
                                    if (objApproval.sApproveStatus == "3")
                                    {
                                        NpgsqlCommand = new NpgsqlCommand();
                                        sQry = " SELECT \"DF_DTC_CODE\" || '~' || \"DT_NAME\" || '~' || \"RO_NAME\" || '~' || \"BO_NAME\" ";
                                        sQry += " FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\",\"TBLDTCMAST\",\"TBLUSER\",\"TBLROLES\", ";
                                        sQry += " \"TBLBUSINESSOBJECT\" WHERE \"DF_DTC_CODE\" =:sDataReferenceId AND \"WO_DATA_ID\" =:sDataReferenceId1 ";
                                        sQry += " AND \"DF_DTC_CODE\"=\"DT_CODE\" AND \"WO_NEXT_ROLE\" =:sRoleId AND \"WO_CR_BY\" = \"US_ID\" ";
                                        sQry += " AND \"RO_ID\"=\"US_ROLE_ID\" AND \"WO_BO_ID\"=\"BO_ID\" AND \"WO_ID\" =:sWFObjectId ";
                                        NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", objApproval.sDataReferenceId);
                                        NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId1", objApproval.sDataReferenceId);
                                        NpgsqlCommand.Parameters.AddWithValue("sRoleId", Convert.ToInt32(objApproval.sRoleId));
                                        NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                                        string sResult = objDatabse.get_value(sQry, NpgsqlCommand);
                                        objcomm.sSMSkey = "SMStoReject";
                                        objcomm = objcomm.GetsmsTempalte_Latest(objcomm, objDatabse);
                                        if (objcomm.sSMSTemplate != null)
                                        {
                                            // sSMSText = String.Format(Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SMStoReject"]), sResult.Split('~').GetValue(3).ToString(),
                                            sSMSText = String.Format(
                                                objcomm.sSMSTemplate,
                                                sResult.Split('~').GetValue(3).ToString(),
                                                sResult.Split('~').GetValue(0).ToString(),
                                                sResult.Split('~').GetValue(2).ToString()
                                                );
                                        }
                                    }
                                    else
                                    {
                                        NpgsqlCommand = new NpgsqlCommand();
                                        sQry = " SELECT \"DF_DTC_CODE\",\"DF_EQUIPMENT_ID\" FROM \"TBLDTCFAILURE\" WHERE  \"DF_DTC_CODE\" =:sDataReferenceId ";
                                        NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", objApproval.sDataReferenceId);
                                        dt = objDatabse.FetchDataTable(sQry, NpgsqlCommand);
                                        if (dt.Rows.Count > 0)
                                        {
                                            objcomm.sSMSkey = "SMStoDecommCreate";
                                            objcomm = objcomm.GetsmsTempalte_Latest(objcomm, objDatabse);
                                            if (objcomm.sSMSTemplate != null)
                                            {
                                                //  sSMSText = String.Format(Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SMStoDecommCreate"]),
                                                sSMSText = String.Format(
                                                    objcomm.sSMSTemplate,
                                                    Convert.ToString(dt.Rows[0]["DF_DTC_CODE"]),
                                                    Convert.ToString(dt.Rows[0]["DF_EQUIPMENT_ID"])
                                                    );
                                            }
                                        }
                                    }
                                }
                            }
                            if (objApproval.sBOId == "15")  // RI Acknoldgement
                            {
                                if (sPreviousBoId == "14")
                                {
                                    NpgsqlCommand = new NpgsqlCommand();
                                    sQry = " SELECT \"IN_INV_NO\",\"TR_RI_NO\" FROM \"TBLDTCINVOICE\",\"TBLTCREPLACE\" ";
                                    sQry += " WHERE \"TR_IN_NO\"=\"IN_NO\" AND \"TR_ID\"=:sNewRecordId ";
                                    NpgsqlCommand.Parameters.AddWithValue("sNewRecordId", Convert.ToInt32(objApproval.sNewRecordId));
                                    dt = objDatabse.FetchDataTable(sQry, NpgsqlCommand);
                                    if (dt.Rows.Count > 0)
                                    {
                                        objcomm.sSMSkey = "SMStoDecomm";
                                        objcomm = objcomm.GetsmsTempalte_Latest(objcomm, objDatabse);
                                        if (objcomm.sSMSTemplate != null)
                                        {
                                            //  sSMSText = String.Format(Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SMStoDecomm"]),
                                            sSMSText = String.Format(
                                                objcomm.sSMSTemplate,
                                                Convert.ToString(dt.Rows[0]["IN_INV_NO"]),
                                                Convert.ToString(dt.Rows[0]["TR_RI_NO"])
                                                );
                                        }
                                    }
                                }
                                else
                                {
                                    if (objApproval.sApproveStatus == "3")
                                    {
                                        NpgsqlCommand = new NpgsqlCommand();
                                        sQry = " SELECT \"DF_DTC_CODE\" || '~' || \"DT_NAME\" || '~' || \"RO_NAME\" || '~' || \"BO_NAME\" ";
                                        sQry += " FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\",\"TBLDTCMAST\",\"TBLUSER\",\"TBLROLES\", ";
                                        sQry += " \"TBLBUSINESSOBJECT\" WHERE \"DF_DTC_CODE\" =:sDataReferenceId AND \"WO_DATA_ID\" =:sDataReferenceId1 ";
                                        sQry += "  AND \"DF_DTC_CODE\"=\"DT_CODE\" AND \"WO_NEXT_ROLE\" =:sRoleId AND \"WO_CR_BY\"=\"US_ID\" ";
                                        sQry += "  AND \"RO_ID\"=\"US_ROLE_ID\" AND \"WO_BO_ID\"=\"BO_ID\" AND \"WO_ID\" =:sWFObjectId";
                                        NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", objApproval.sDataReferenceId);
                                        NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId1", objApproval.sDataReferenceId);
                                        NpgsqlCommand.Parameters.AddWithValue("sRoleId", Convert.ToInt32(objApproval.sRoleId));
                                        NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                                        string sResult = objDatabse.get_value(sQry, NpgsqlCommand);
                                        objcomm.sSMSkey = "SMStoReject";
                                        objcomm = objcomm.GetsmsTempalte_Latest(objcomm, objDatabse);
                                        if (objcomm.sSMSTemplate != null)
                                        {
                                            // sSMSText = String.Format(Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SMStoReject"]), sResult.Split('~').GetValue(3).ToString(),
                                            sSMSText = String.Format(
                                                objcomm.sSMSTemplate,
                                                sResult.Split('~').GetValue(3).ToString(),
                                                sResult.Split('~').GetValue(0).ToString(),
                                                sResult.Split('~').GetValue(2).ToString()
                                                );
                                        }
                                    }
                                    else
                                    {
                                        NpgsqlCommand = new NpgsqlCommand();
                                        sQry = " SELECT \"TR_RI_NO\",\"DF_EQUIPMENT_ID\" FROM \"TBLTCREPLACE\",\"TBLDTCFAILURE\",\"TBLTCDRAWN\" ";
                                        sQry += " WHERE \"DF_ID\"=\"TD_DF_ID\" AND  \"TD_INV_NO\"=\"TR_IN_NO\" AND \"TR_ID\" =:sRecordId ";
                                        NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objApproval.sRecordId));
                                        dt = objDatabse.FetchDataTable(sQry, NpgsqlCommand);
                                        if (dt.Rows.Count > 0)
                                        {
                                            objcomm.sSMSkey = "SMStoRICreate";
                                            objcomm = objcomm.GetsmsTempalte_Latest(objcomm, objDatabse);
                                            if (objcomm.sSMSTemplate != null)
                                            {
                                                // sSMSText = String.Format(Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SMStoRICreate"]),
                                                sSMSText = String.Format(
                                                    objcomm.sSMSTemplate,
                                                    Convert.ToString(dt.Rows[0]["TR_RI_NO"]),
                                                    Convert.ToString(dt.Rows[0]["DF_EQUIPMENT_ID"])
                                                    );
                                            }
                                        }
                                    }
                                }
                            }
                            if (objApproval.sBOId == "26")  // Completion Report
                            {
                                if (sPreviousBoId == "15")
                                {
                                    NpgsqlCommand = new NpgsqlCommand();
                                    sQry = " SELECT \"TR_RI_NO\",\"DF_EQUIPMENT_ID\" FROM \"TBLTCREPLACE\",\"TBLDTCFAILURE\",\"TBLTCDRAWN\" ";
                                    sQry += " WHERE \"DF_ID\"=\"TD_DF_ID\" AND \"TD_INV_NO\"=\"TR_IN_NO\" AND \"TR_ID\" =:sRecordId ";
                                    NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objApproval.sRecordId));
                                    dt = objDatabse.FetchDataTable(sQry, NpgsqlCommand);
                                    if (dt.Rows.Count > 0)
                                    {
                                        NpgsqlCommand = new NpgsqlCommand();
                                        NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", Convert.ToInt32(objApproval.sOfficeCode.Substring(0, Division)));

                                        string sStoreName = objDatabse.get_value(" SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"SM_OFF_CODE\" =:sOfficeCode ", NpgsqlCommand);

                                        objcomm.sSMSkey = "SMStoRI";
                                        objcomm = objcomm.GetsmsTempalte_Latest(objcomm, objDatabse);
                                        if (objcomm.sSMSTemplate != null)
                                        {
                                            // sSMSText = String.Format(Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SMStoRI"]),
                                            sSMSText = String.Format(
                                                objcomm.sSMSTemplate,
                                                Convert.ToString(dt.Rows[0]["TR_RI_NO"]),
                                                Convert.ToString(dt.Rows[0]["DF_EQUIPMENT_ID"]),
                                                sStoreName
                                                );
                                        }
                                    }
                                }
                                else
                                {
                                    if (objApproval.sApproveStatus == "3")
                                    {
                                        NpgsqlCommand = new NpgsqlCommand();

                                        sQry = " SELECT \"DF_DTC_CODE\" || '~' || \"DT_NAME\" || '~' || \"RO_NAME\" || '~' || \"BO_NAME\" ";
                                        sQry += " FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\",\"TBLDTCMAST\",\"TBLUSER\",\"TBLROLES\", ";
                                        sQry += " \"TBLBUSINESSOBJECT\" WHERE \"DF_DTC_CODE\" =:sDataReferenceId AND \"WO_DATA_ID\" =:sDataReferenceId1 ";
                                        sQry += " AND \"DF_DTC_CODE\"=\"DT_CODE\" AND \"WO_NEXT_ROLE\" =:sRoleId AND \"WO_CR_BY\"=\"US_ID\" ";
                                        sQry += " AND \"RO_ID\"=\"US_ROLE_ID\" AND \"WO_BO_ID\"=\"BO_ID\" AND \"WO_ID\" =:sWFObjectId ";
                                        NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", objApproval.sDataReferenceId);
                                        NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId1", objApproval.sDataReferenceId);
                                        NpgsqlCommand.Parameters.AddWithValue("sRoleId", Convert.ToInt32(objApproval.sRoleId));
                                        NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                                        string sResult = objDatabse.get_value(sQry, NpgsqlCommand);

                                        objcomm.sSMSkey = "SMStoReject";
                                        objcomm = objcomm.GetsmsTempalte_Latest(objcomm, objDatabse);
                                        if (objcomm.sSMSTemplate != null)
                                        {
                                            // sSMSText = String.Format(Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SMStoReject"]), sResult.Split('~').GetValue(3).ToString(),
                                            sSMSText = String.Format(
                                                objcomm.sSMSTemplate,
                                                sResult.Split('~').GetValue(3).ToString(),
                                                sResult.Split('~').GetValue(0).ToString(),
                                                sResult.Split('~').GetValue(2).ToString()
                                                );
                                        }
                                    }
                                    else
                                    {
                                        NpgsqlCommand = new NpgsqlCommand();
                                        sQry = " SELECT \"TR_RI_NO\",\"DF_EQUIPMENT_ID\",\"TD_TC_NO\" FROM \"TBLTCREPLACE\",\"TBLDTCFAILURE\",\"TBLTCDRAWN\" ";
                                        sQry += " WHERE \"DF_ID\"=\"TD_DF_ID\" AND \"TD_INV_NO\"=\"TR_IN_NO\" AND \"TR_ID\" =:sRecordId ";
                                        NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(objApproval.sRecordId));

                                        dt = objDatabse.FetchDataTable(sQry, NpgsqlCommand);
                                        if (dt.Rows.Count > 0)
                                        {
                                            NpgsqlCommand = new NpgsqlCommand();
                                            NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", Convert.ToInt32(objApproval.sOfficeCode.Substring(0, Division)));
                                            string sStoreName = objDatabse.get_value("SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"SM_OFF_CODE\" =:sOfficeCode", NpgsqlCommand);

                                            objcomm.sSMSkey = "SMStoCRCreate";
                                            objcomm = objcomm.GetsmsTempalte_Latest(objcomm, objDatabse);
                                            if (objcomm.sSMSTemplate != null)
                                            {
                                                //sSMSText = String.Format(Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SMStoCRCreate"]),
                                                sSMSText = String.Format(
                                                    objcomm.sSMSTemplate,
                                                    Convert.ToString(dt.Rows[0]["TR_RI_NO"]),
                                                    Convert.ToString(dt.Rows[0]["DF_EQUIPMENT_ID"]),
                                                    Convert.ToString(dt.Rows[0]["TD_TC_NO"])
                                                    );
                                            }
                                        }
                                    }
                                }
                            }
                            if (objApproval.sBOId == "45")  //Estimation
                            {
                                if (sPreviousBoId == "9")
                                {
                                    objcomm.sSMSkey = "SMStoFailure";
                                    objcomm = objcomm.GetsmsTempalte_Latest(objcomm, objDatabse);
                                    if (objcomm.sSMSTemplate != null)
                                    {
                                        //  sSMSText = String.Format(Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SMStoFailure"]),
                                        sSMSText = String.Format(
                                            objcomm.sSMSTemplate,
                                            objApproval.sDataReferenceId
                                          );
                                    }
                                }
                                else
                                {
                                    NpgsqlCommand = new NpgsqlCommand();
                                    sQry = " SELECT \"DF_DTC_CODE\" || '~' || \"DT_NAME\" || '~' || \"RO_NAME\" || '~' || \"BO_NAME\" ";
                                    sQry += " FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\",\"TBLDTCMAST\",\"TBLUSER\",\"TBLROLES\",\"TBLBUSINESSOBJECT\" WHERE ";
                                    sQry += " \"DF_ID\" =:sDataReferenceId AND \"WO_DATA_ID\"=:sDataReferenceId1 AND \"DF_DTC_CODE\"=\"DT_CODE\" AND ";
                                    //sQry += " \"WO_NEXT_ROLE\" ='" + objApproval.sRoleId + "' AND \"WO_ID\"='" + objApproval.sWFObjectId + "' AND \"WO_CR_BY\"=\"US_ID\" AND \"RO_ID\"=\"US_ROLE_ID\" AND \"WO_BO_ID\"=\"BO_ID\" ";
                                    sQry += " \"WO_ID\"=:sWFObjectId AND \"WO_CR_BY\"=\"US_ID\" AND \"RO_ID\"=\"US_ROLE_ID\" AND \"WO_BO_ID\"=\"BO_ID\" ";
                                    NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", Convert.ToInt32(objApproval.sDataReferenceId));
                                    NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId1", objApproval.sDataReferenceId);
                                    NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                                    string sResult = objDatabse.get_value(sQry, NpgsqlCommand);

                                    if (objApproval.sRoleId == "2")
                                    {
                                        objcomm.sSMSkey = "SMStoEstimationCreate";
                                        objcomm = objcomm.GetsmsTempalte_Latest(objcomm, objDatabse);
                                        if (objcomm.sSMSTemplate != null)
                                        {
                                            // sSMSText = String.Format(Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SMStoEstimationCreate"]),
                                            sSMSText = String.Format(
                                                objcomm.sSMSTemplate,
                                                sResult.Split('~').GetValue(0).ToString(),
                                                sResult.Split('~').GetValue(1).ToString()
                                                );
                                        }
                                    }
                                    else
                                    {
                                        if (objApproval.sApproveStatus == "3")
                                        {
                                            NpgsqlCommand = new NpgsqlCommand();
                                            sQry = " SELECT \"DF_DTC_CODE\" || '~' || \"DT_NAME\" || '~' || \"RO_NAME\" || '~' || \"BO_NAME\" ";
                                            sQry += " FROM \"TBLWORKFLOWOBJECTS\",\"TBLDTCFAILURE\",\"TBLDTCMAST\",\"TBLUSER\",\"TBLROLES\",\"TBLBUSINESSOBJECT\" WHERE ";
                                            sQry += " \"DF_ID\" =:sDataReferenceId AND \"WO_DATA_ID\" =:sDataReferenceId1 AND \"DF_DTC_CODE\"=\"DT_CODE\" AND ";
                                            sQry += " \"WO_NEXT_ROLE\" =:sRoleId AND \"WO_CR_BY\"=\"US_ID\" AND \"RO_ID\"=\"US_ROLE_ID\" ";
                                            sQry += " AND \"WO_BO_ID\"=\"BO_ID\" AND \"WO_ID\"=:sWFObjectId ";
                                            NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", Convert.ToInt32(objApproval.sDataReferenceId));
                                            NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId1", objApproval.sDataReferenceId);
                                            NpgsqlCommand.Parameters.AddWithValue("sRoleId", Convert.ToInt32(objApproval.sRoleId));
                                            NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                                            sResult = objDatabse.get_value(sQry, NpgsqlCommand);

                                            objcomm.sSMSkey = "SMStoReject";
                                            objcomm = objcomm.GetsmsTempalte_Latest(objcomm, objDatabse);
                                            if (objcomm.sSMSTemplate != null)
                                            {
                                                // sSMSText = String.Format(Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SMStoReject"]),
                                                sSMSText = String.Format(
                                                    objcomm.sSMSTemplate,
                                                    sResult.Split('~').GetValue(3).ToString(),
                                                    sResult.Split('~').GetValue(0).ToString(),
                                                    sResult.Split('~').GetValue(2).ToString()
                                                    );
                                            }
                                        }
                                        else
                                        {
                                            objcomm.sSMSkey = "SMStoEstimateApprover";
                                            objcomm = objcomm.GetsmsTempalte_Latest(objcomm, objDatabse);
                                            if (objcomm.sSMSTemplate != null)
                                            {
                                                // sSMSText = String.Format(Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SMStoEstimateApprover"]),
                                                sSMSText = String.Format(
                                                    objcomm.sSMSTemplate,
                                                    sResult.Split('~').GetValue(0).ToString(),
                                                    sResult.Split('~').GetValue(1).ToString(),
                                                    sResult.Split('~').GetValue(2).ToString()
                                                    );
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (sSMSText == "")
                        {
                        }
                        else
                        {
                            if (objcomm.sSMSTemplateID != null && objcomm.sSMSTemplateID != "")
                            {
                                objcomm.DumpSms_Latest(sMobileNo, sSMSText, objcomm.sSMSTemplateID, "WEB", objDatabse);
                            }
                        }
                        //objCommunication.sendSMS(sSMSText, sMobileNo, objApproval.sRoleId);
                    }
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// Check for Duplicate Approval
        /// </summary>
        /// <param name="sBOId"></param>
        /// <param name="sRoleId"></param>
        /// <param name="sFormName"></param>
        /// <returns></returns>       
        public bool CheckDuplicateApprove(clsApproval objApproval)
        {
            string QryKey = string.Empty;
            try
            {
                string strQry = string.Empty;
                string sApproveResult = string.Empty;
                if (objApproval.sWFAutoId != null)
                {
                    if (objApproval.sWFAutoId == "0")
                    {
                        #region Old Inline query
                        //NpgsqlCommand = new NpgsqlCommand();
                        //strQry = "SELECT \"WO_BO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\" =:sWFObjectId AND \"WO_APPROVE_STATUS\" <>0";
                        //NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                        //sApproveResult = objcon.get_value(strQry, NpgsqlCommand);
                        #endregion

                        QryKey = "GET_WO_BO_ID";
                        NpgsqlCommand cmd_WO_BO_ID = new NpgsqlCommand("fetch_getvalue_for_checkduplicateapprove_in_clsapproval");
                        cmd_WO_BO_ID.Parameters.AddWithValue("p_key", QryKey);
                        cmd_WO_BO_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(objApproval.sWFObjectId ?? ""));
                        sApproveResult = ObjBasCon.StringGetValue(cmd_WO_BO_ID);

                        if (sApproveResult != "")
                        {
                            return false;
                        }
                    }
                    else
                    {
                        #region Old Inline query
                        //NpgsqlCommand = new NpgsqlCommand();
                        //strQry = "SELECT \"WOA_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE CAST(\"WOA_ID\" AS TEXT) =CAST(:sWFAutoId AS TEXT) AND \"WOA_INITIAL_ACTION_ID\" IS NOT NULL";
                        //NpgsqlCommand.Parameters.AddWithValue("sWFAutoId", objApproval.sWFAutoId);
                        //sApproveResult = objcon.get_value(strQry, NpgsqlCommand);
                        #endregion

                        QryKey = "GET_WOA_ID";
                        NpgsqlCommand cmd_WO_BO_ID = new NpgsqlCommand("fetch_getvalue_for_checkduplicateapprove_in_clsapproval");
                        cmd_WO_BO_ID.Parameters.AddWithValue("p_key", QryKey);
                        cmd_WO_BO_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(objApproval.sWFAutoId ?? ""));
                        sApproveResult = ObjBasCon.StringGetValue(cmd_WO_BO_ID);


                        if (sApproveResult != "")
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return true;
            }
        }
        public string sGetApprovalLevel(string sBoid, string sRoleId)
        {
            try
            {
                #region Old inline query
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "SELECT \"MAX_LEVEL\"|| '~' ||\"LEVEL\"  AS \"LEVELS\" FROM (SELECT MAX(\"WM_LEVEL\") \"MAX_LEVEL\",\"WM_BOID\" FROM \"TBLWORKFLOWMASTER\" WHERE ";
                //strQry += " \"WM_BOID\" =:sBoid GROUP BY \"WM_BOID\")A INNER JOIN (SELECT \"WM_LEVEL\" as \"LEVEL\",\"WM_BOID\"  FROM \"TBLWORKFLOWMASTER\" ";
                //strQry += " WHERE \"WM_BOID\"=:sBoid1 AND \"WM_ROLEID\"=:sRoleId)B ON A.\"WM_BOID\"=B.\"WM_BOID\"";
                //NpgsqlCommand.Parameters.AddWithValue("sBoid", Convert.ToInt32(sBoid));
                //NpgsqlCommand.Parameters.AddWithValue("sBoid1", Convert.ToInt32(sBoid));
                //NpgsqlCommand.Parameters.AddWithValue("sRoleId", Convert.ToInt32(sRoleId));
                //return objcon.get_value(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_for_sgetapprovallevel_in_clsapproval");
                cmd.Parameters.AddWithValue("p_boid", Convert.ToInt32(sBoid));
                cmd.Parameters.AddWithValue("p_roleid", Convert.ToInt32(sRoleId));
                return ObjBasCon.StringGetValue(cmd);

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }
        public string getdataid(string sWFInitialId)
        {
            try
            {
                #region Old inline query
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "SELECT \"WO_DATA_ID\" from \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\"=:sWFInitialId";
                //NpgsqlCommand.Parameters.AddWithValue("sWFInitialId", Convert.ToInt32(sWFInitialId));
                //return objcon.get_value(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_for_getdataid_in_clsapproval");
                cmd.Parameters.AddWithValue("p_wfinitialid", Convert.ToInt32(sWFInitialId));
                return ObjBasCon.StringGetValue(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }
        public bool CheckAlreadyExistEntryId(string sDataReferenceId, string sBOId)
        {
            string sResult = string.Empty;
            try
            {
                #region Old inline query
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "SELECT \"WO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_DATA_ID\" =:sDataReferenceId AND \"WO_APPROVE_STATUS\" ='0' AND \"WO_BO_ID\" =:sBOId";
                //NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", sDataReferenceId);
                //NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(sBOId));
                //sResult = objcon.get_value(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_for_checkalreadyexistentryid_in_clsapproval");
                cmd.Parameters.AddWithValue("p_datareferenceid", Convert.ToString(sDataReferenceId ?? ""));
                cmd.Parameters.AddWithValue("p_boid", Convert.ToInt32(sBOId));
                sResult = ObjBasCon.StringGetValue(cmd);

                if (sResult != "")
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }
        public string[] GetApprovedpreview(string sDataReferenceId, string sBOId)
        {
            string sResult = string.Empty;
            string[] Arr = new string[3];
            try
            {
                #region old inlin query
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "SELECT \"WO_WFO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_DATA_ID\" =:sDataReferenceId  AND \"WO_BO_ID\" =:sBOId ORDER BY \"WO_ID\" DESC";
                //NpgsqlCommand.Parameters.AddWithValue("sDataReferenceId", sDataReferenceId);
                //NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(sBOId));
                //sResult = objcon.get_value(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_for_getapprovedpreview_in_clsapproval");
                cmd.Parameters.AddWithValue("p_datareferenceid", Convert.ToString(sDataReferenceId ?? ""));
                cmd.Parameters.AddWithValue("p_boid", Convert.ToInt32(sBOId));
                sResult = ObjBasCon.StringGetValue(cmd);

                if (sResult != "")
                {
                    Arr[0] = "success";
                    Arr[1] = "0";
                    Arr[2] = sResult;
                    return Arr;
                }
                Arr[0] = "fail";
                Arr[1] = "2";
                Arr[2] = sResult;
                return Arr;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }
        /// <summary>
        /// Check for Duplicate Approval
        /// </summary>
        /// <param name="sBOId"></param>
        /// <param name="sRoleId"></param>
        /// <param name="sFormName"></param>
        /// <returns></returns>
        public bool CheckDuplicateApprove_Latest(clsApproval objApproval, DataBseConnection objDatabse)
        {
            string QryKey = string.Empty;
            try
            {
                string strQry = string.Empty;
                string sApproveResult = string.Empty;
                if (objApproval.sWFAutoId != null)
                {
                    if (objApproval.sWFAutoId == "0")
                    {
                        #region Old inline query
                        //NpgsqlCommand = new NpgsqlCommand();
                        //strQry = "SELECT \"WO_BO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\" =:sWFObjectId AND \"WO_APPROVE_STATUS\" <>0";
                        //NpgsqlCommand.Parameters.AddWithValue("sWFObjectId", Convert.ToInt32(objApproval.sWFObjectId));
                        //sApproveResult = objDatabse.get_value(strQry, NpgsqlCommand);
                        #endregion

                        QryKey = "GET_WO_BO_ID";
                        NpgsqlCommand cmd_WO_BO_ID = new NpgsqlCommand("fetch_getvalue_for_checkduplicateapprove_in_clsapproval");
                        cmd_WO_BO_ID.Parameters.AddWithValue("p_key", QryKey);
                        cmd_WO_BO_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(objApproval.sWFObjectId ?? ""));
                        sApproveResult = objDatabse.StringGetValue(cmd_WO_BO_ID);

                        if (sApproveResult != "")
                        {
                            return false;
                        }
                    }
                    else
                    {
                        #region Old inline query
                        //NpgsqlCommand = new NpgsqlCommand();
                        //strQry = "SELECT \"WOA_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE CAST(\"WOA_ID\" AS TEXT) =CAST(:sWFAutoId AS TEXT) AND \"WOA_INITIAL_ACTION_ID\" IS NOT NULL";
                        //NpgsqlCommand.Parameters.AddWithValue("sWFAutoId", objApproval.sWFAutoId);
                        //sApproveResult = objDatabse.get_value(strQry, NpgsqlCommand);
                        #endregion

                        QryKey = "GET_WOA_ID";
                        NpgsqlCommand cmd_WO_BO_ID = new NpgsqlCommand("fetch_getvalue_for_checkduplicateapprove_in_clsapproval");
                        cmd_WO_BO_ID.Parameters.AddWithValue("p_key", QryKey);
                        cmd_WO_BO_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(objApproval.sWFAutoId ?? ""));
                        sApproveResult = objDatabse.StringGetValue(cmd_WO_BO_ID);

                        if (sApproveResult != "")
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }
    }
}
