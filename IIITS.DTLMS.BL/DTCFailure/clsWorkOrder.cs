using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Collections;
using IIITS.PGSQL.DAL;
using NpgsqlTypes;
using Npgsql;
using IIITS.DTLMS.BL.DataBase;
using System.Configuration;

namespace IIITS.DTLMS.BL
{
    public class clsWorkOrder
    {
        string strFormCode = "clsWorkOrder";
        PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);

        DataBseConnection ObjBasCon = new DataBseConnection(Constants.Password);

        public string sdtccode { get; set; }
        public string sWOId { get; set; }
        public string sFailureId { get; set; }
        public string sFailureDate { get; set; }
        public string sLocationCode { get; set; }
        public string sAccCode { get; set; }
        public string sCrBy { get; set; }
        public string sCommWoNo { get; set; }
        public string sCommDate { get; set; }
        public string sCommAmmount { get; set; }
        public string sDeWoNo { get; set; }
        public string sDeCommDate { get; set; }
        public string sDeCommAmmount { get; set; }
        public string WoNoLocCode { get; set; }
        public string sDecomAccCode { get; set; }
        public string sCrAccCode { get; set; }
        public string sIssuedBy { get; set; }
        public string sCapacity { get; set; }
        public string sNewCapacity { get; set; }
        public string sEnhanceAccCode { get; set; }
        public string sEnhancedCapacity { get; set; }

        public string sTaskType { get; set; }
        public string sOfficeCode { get; set; }

        public string sDTCName { get; set; }
        public string sTCSlno { get; set; }
        public string sTCCode { get; set; }
        public string sRequestLoc { get; set; }
        public string sDTCCode { get; set; }
        public string sDTCId { get; set; }
        public string sTCId { get; set; }

        public string sWOFilePath { get; set; }

        // Workflow
        public string sFormName { get; set; }
        public string sClientIP { get; set; }
        public string sWFOId { get; set; }
        public string sWFDataId { get; set; }
        public string sActionType { get; set; }
        public string sWFAutoId { get; set; }
        public string sWFObjectId { get; set; }
        public string sApprovalDesc { get; set; }
        public int sDtcScheme { get; set; }
        public string sFailType { get; set; }
        public string sRepairer { get; set; }
        public string sOFCommWoNo { get; set; }
        public string sOFCommDate { get; set; }
        public string sOFCommAmmount { get; set; }
        public string sOFAccCode { get; set; }
        public string sGuarentyType { get; set; }
        public string sCreditWO { get; set; }
        public string sCreditDate { get; set; }
        public string sCreditAmount { get; set; }
        public string sCreditAccCode { get; set; }

        public string sDeCreditWO { get; set; }
        public string sDeCreditDate { get; set; }
        public string sDeCreditAmount { get; set; }
        public string sDeCreditAccCode { get; set; }
        public string sTtkStatus { get; set; }

        public string sboid { get; set; }

        NpgsqlCommand NpgsqlCommand;
        public string sTtkAutoNo { get; set; }
        public string sTtkVendorName { get; set; }
        public string sTtkManual { get; set; }

        public string sRating { get; set; }
        public string sDWAname { get; set; }
        public string sDWAdate { get; set; }
        public string sDiv_Locaton_Code { get; set; }
        public string sFinancialYear { get; set; }
        public string sTempslno { get; set; }
        public string sWoSlno { get; set; }
        public string sDecSlno { get; set; }
        public string sWofinyear { get; set; }
        public string sdecfinyear { get; set; }
        public string sWoDecomSlno { get; set; }
        public string Decommisionid { get; set; }
        public string Commissionid { get; set; }
        public string RoleType { get; set; }
        public string Officecodeforworange { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        /// <summary>
        /// Save Update WorkOrder
        /// </summary>
        /// <param name="objWorkOrder"></param>
        /// <returns></returns>
        public string[] SaveUpdateWorkOrder(clsWorkOrder objWorkOrder)
        {
            string strQry = string.Empty;
            string[] Arr = new string[3];
            string[] Arrrange = new string[3];
            string DecComQry = string.Empty;
            string ComQry = string.Empty;
            string QryKey = string.Empty;
            DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
            try
            {
                objDatabse.BeginTransaction();
                if (objWorkOrder.sTaskType != "3")
                {
                    #region Old Inline query
                    //NpgsqlCommand = new NpgsqlCommand();
                    //NpgsqlCommand.Parameters.AddWithValue("sFailureId", Convert.ToInt32(objWorkOrder.sFailureId));
                    ////Check Failure ID is exists or not                    
                    //string sId = objDatabse.get_value("SELECT \"DF_ID\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\"=:sFailureId", NpgsqlCommand);
                    #endregion

                    QryKey = "GET_DF_ID";
                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsworkorder");
                    cmd.Parameters.AddWithValue("p_key", QryKey);
                    cmd.Parameters.AddWithValue("p_value_1", objWorkOrder.sFailureId);
                    cmd.Parameters.AddWithValue("p_value_2", "");
                    string sId = objDatabse.StringGetValue(cmd);

                    if (sId.Length == 0)
                    {
                        Arr[0] = "Enter Valid Failure ID";
                        Arr[1] = "2";
                        return Arr;
                    }
                }
                if (objWorkOrder.sWOId == "")
                {
                    if (objWorkOrder.sTtkStatus != "1")
                    {
                        #region Old Inlline query
                        //NpgsqlCommand = new NpgsqlCommand();
                        //NpgsqlCommand.Parameters.AddWithValue("sCommWoNo", objWorkOrder.sCommWoNo);
                        //NpgsqlCommand.Parameters.AddWithValue("sLocationCode", Convert.ToInt32(objWorkOrder.sLocationCode));
                        //string sId = objDatabse.get_value("SELECT \"WO_NO\" FROM \"TBLWORKORDER\" WHERE \"WO_NO\"=:sCommWoNo AND \"WO_OFF_CODE\" =:sLocationCode", NpgsqlCommand);
                        #endregion

                        QryKey = "GET_WO_NO";
                        NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsworkorder");
                        cmd.Parameters.AddWithValue("p_key", QryKey);
                        cmd.Parameters.AddWithValue("p_value_1", objWorkOrder.sCommWoNo);
                        cmd.Parameters.AddWithValue("p_value_2", objWorkOrder.sLocationCode);
                        string sId = objDatabse.StringGetValue(cmd);

                        if (sId.Length > 0)
                        {
                            Arr[0] = "Work Order No. Already Exists";
                            Arr[1] = "2";
                            return Arr;
                        }
                    }
                    if (objWorkOrder.sTtkStatus != "1")
                    {
                        #region Old Inline query
                        //NpgsqlCommand = new NpgsqlCommand();
                        //NpgsqlCommand.Parameters.AddWithValue("sDeWoNo", objWorkOrder.sDeWoNo);
                        //NpgsqlCommand.Parameters.AddWithValue("sLocationCode", Convert.ToInt32(objWorkOrder.sLocationCode));
                        //string sId = objDatabse.get_value("SELECT \"WO_NO_DECOM\" FROM \"TBLWORKORDER\" WHERE \"WO_NO_DECOM\"=:sDeWoNo AND \"WO_OFF_CODE\" =:sLocationCode", NpgsqlCommand);
                        #endregion

                        QryKey = "GET_WO_NO_DECOM";
                        NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsworkorder");
                        cmd.Parameters.AddWithValue("p_key", QryKey);
                        cmd.Parameters.AddWithValue("p_value_1", objWorkOrder.sDeWoNo);
                        cmd.Parameters.AddWithValue("p_value_2", objWorkOrder.sLocationCode);
                        string sId = objDatabse.StringGetValue(cmd);

                        if (sId.Length > 0)
                        {
                            Arr[0] = "Decommission Work Order No. Already Exists";
                            Arr[1] = "2";
                            return Arr;
                        }
                    }
                    if (objWorkOrder.sActionType != "M")
                    {
                        DataTable dt = new DataTable();
                        NpgsqlCommand cmd = new NpgsqlCommand("sp_check_workorder_alredyexiest");
                        cmd.Parameters.AddWithValue("offCode", objWorkOrder.sLocationCode);
                        cmd.Parameters.AddWithValue("wo_no", objWorkOrder.sCommWoNo);
                        dt = objDatabse.FetchDataTable(cmd);
                        if (dt.Rows.Count > 0)
                        {
                            Arr[0] = "Work Order No. Already Exists";
                            Arr[1] = "2";
                            return Arr;
                        }
                    }
                    if (objWorkOrder.sActionType != "M")
                    {
                        DataTable dt = new DataTable();
                        NpgsqlCommand cmd = new NpgsqlCommand("sp_check_decommworkorder_alredyexiest");
                        cmd.Parameters.AddWithValue("offCode", objWorkOrder.sLocationCode);
                        cmd.Parameters.AddWithValue("wo_no", objWorkOrder.sDeWoNo);
                        dt = objDatabse.FetchDataTable(cmd);
                        if (dt.Rows.Count > 0)
                        {
                            Arr[0] = "Decommission Work Order No. Already Exists";
                            Arr[1] = "2";
                            return Arr;
                        }
                    }

                    NpgsqlCommand cmdwono = new NpgsqlCommand("workorder_proc_check_workorder_exists_or_not");
                    cmdwono.Parameters.AddWithValue("wo_no", objWorkOrder.sCommWoNo);
                    cmdwono.Parameters.AddWithValue("failure_id", objWorkOrder.sFailureId);
                    cmdwono.Parameters.AddWithValue("decom_wo_no", objWorkOrder.sDeWoNo);
                    cmdwono.Parameters.Add("id", NpgsqlDbType.Text);
                    cmdwono.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmdwono.Parameters["id"].Direction = ParameterDirection.Output;
                    cmdwono.Parameters["msg"].Direction = ParameterDirection.Output;
                    Arr[0] = "id";
                    Arr[1] = "msg";
                    string[] stResult = objDatabse.Execute(cmdwono, Arr, 2);

                    if (stResult[0] == "2")
                    {
                        Arr[0] = stResult[1];
                        Arr[1] = stResult[0];
                        return Arr;
                    }


                    if (objWorkOrder.sRequestLoc == "")
                        objWorkOrder.sRequestLoc = null;

                    // DF_STATUS_FLAG--->1 Failure Entry ;  DF_STATUS_FLAG--->2 Enhancement Entry
                    objWorkOrder.sWOId = Convert.ToString(ObjCon.Get_max_no("WO_SLNO", "TBLWORKORDER"));
                    strQry = "Insert INTO TBLWORKORDER (WO_SLNO,WO_AUTO_SL_ACCNO,WO_AUTO_SL_DECOMM,WO_FIN_YEAR,WO_NO,WO_DF_ID,WO_DATE,WO_AMT,WO_NO_DECOM,WO_DATE_DECOM,WO_AMT_DECOM,";
                    strQry += " WO_ACC_CODE,WO_OFF_CODE,WO_CRBY,WO_CRON,WO_ACCCODE_DECOM,WO_ISSUED_BY,WO_DTC_CAP,WO_NEW_CAP,WO_REQUEST_LOC,WO_NO_CREDIT,WO_AMT_CREDIT,WO_ACC_CRERDIT,WO_DATE_CREDIT,WO_LOCCODE) VALUES";
                    strQry += "('" + objWorkOrder.sWOId + "','" + objWorkOrder.sWoSlno + "','" + objWorkOrder.sWoDecomSlno + "','" + objWorkOrder.sWofinyear + "','" + objWorkOrder.sCommWoNo.ToUpper() + "','" + objWorkOrder.sFailureId + "',";
                    strQry += " TO_DATE('" + objWorkOrder.sCommDate + "','dd/MM/yyyy'),'" + objWorkOrder.sCommAmmount + "', '" + objWorkOrder.sDeWoNo.ToUpper() + "',";
                    strQry += " TO_DATE('" + objWorkOrder.sDeCommDate + "','dd/MM/yyyy'),'" + objWorkOrder.sDeCommAmmount + "',";
                    strQry += " '" + objWorkOrder.sAccCode + "','" + objWorkOrder.sLocationCode + "','" + objWorkOrder.sCrBy + "',NOW(),";
                    strQry += " '" + objWorkOrder.sDecomAccCode + "','" + objWorkOrder.sIssuedBy + "','" + objWorkOrder.sCapacity + "','" + objWorkOrder.sNewCapacity + "','" + objWorkOrder.sRequestLoc + "',";
                    if (objWorkOrder.sDeCreditDate != "0" && objWorkOrder.sDeCreditDate != null)
                    {
                        strQry += " '" + objWorkOrder.sDeCreditWO.ToUpper() + "','" + objWorkOrder.sDeCreditAmount + "','" + objWorkOrder.sDeCreditAccCode + "',TO_DATE('" + objWorkOrder.sDeCreditDate + "','dd/MM/yyyy') ";
                    }
                    else
                    {
                        strQry += " '" + objWorkOrder.sCreditWO.ToUpper() + "','" + objWorkOrder.sCreditAmount + "','" + objWorkOrder.sCreditAccCode + "',TO_DATE('" + objWorkOrder.sCreditDate + "','dd/MM/yyyy') ";

                    }
                    if (objWorkOrder.sTtkStatus == "1")
                    {
                        strQry += " '" + objWorkOrder.sTtkStatus + "','" + objWorkOrder.sTtkAutoNo + "','" + objWorkOrder.sTtkVendorName + "','" + objWorkOrder.sTtkManual + "'";
                    }
                    else
                    {
                        strQry += ",null,null,'',null,'','','','" + objWorkOrder.WoNoLocCode + "')";
                    }
                    #region Workflow
                    if (objWorkOrder.sFailureId == "")
                    {
                        strQry = "Insert into \"TBLWORKORDER\" (\"WO_SLNO\",\"WO_NO\",\"WO_DF_ID\",\"WO_DATE\",\"WO_AMT\",\"WO_NO_DECOM\",\"WO_DATE_DECOM\",\"WO_AMT_DECOM\",";
                        strQry += " \"WO_ACC_CODE\",\"WO_OFF_CODE\",\"WO_CRBY\",\"WO_CRON\",\"WO_ACCCODE_DECOM\",\"WO_ISSUED_BY\",\"WO_DTC_CAP\",\"WO_NEW_CAP\",\"WO_REQUEST_LOC\",\"WO_AUTO_NO\",";
                        strQry += " \"WO_TTK_STATUS\",\"WO_TTK_AUTO_NO\",\"WO_TTK_VENDOR_NAME\",\"WO_TTK_MANUAL_NO\",\"WO_DWA_NAME\",\"WO_DWA_DATE\",\"WO_RATING\",\"WO_LOCCODE\" ) VALUES";
                        strQry += "('{0}','" + objWorkOrder.sCommWoNo.ToUpper() + "',NULL,";
                        strQry += " TO_DATE('" + objWorkOrder.sCommDate + "','dd/MM/yyyy'),'" + objWorkOrder.sCommAmmount + "', '" + objWorkOrder.sDeWoNo.ToUpper() + "',";
                        strQry += " TO_DATE('" + objWorkOrder.sDeCommDate + "','dd/MM/yyyy'),'" + objWorkOrder.sDeCommAmmount + "',";
                        strQry += " '" + objWorkOrder.sAccCode + "','" + objWorkOrder.sLocationCode + "','" + objWorkOrder.sCrBy + "',NOW(),";
                        strQry += " '" + objWorkOrder.sDecomAccCode + "','" + objWorkOrder.sIssuedBy + "','" + objWorkOrder.sCapacity + "',";
                        strQry += " '" + objWorkOrder.sNewCapacity + "','" + objWorkOrder.sRequestLoc + "','{1}','" + objWorkOrder.sOFCommWoNo + "',";
                        // coded by rudra for new TTk concept
                        if (objWorkOrder.sTtkStatus == "1" || objWorkOrder.sTtkStatus == "0")
                        {
                            strQry += ",'" + objWorkOrder.sTtkStatus + "','" + objWorkOrder.sTtkAutoNo + "','" + objWorkOrder.sTtkVendorName + "','" + objWorkOrder.sTtkManual + "','" + objWorkOrder.sDWAname + "',TO_DATE('" + objWorkOrder.sDWAdate + "','dd/MM/yyyy'),'" + objWorkOrder.sRating + "')";
                        }
                        else
                        {
                            strQry += ",null,null,'',null,'',null,'','" + objWorkOrder.WoNoLocCode + "')";
                        }
                    }
                    else
                    {
                        strQry = "Insert into \"TBLWORKORDER\" (\"WO_SLNO\",\"WO_AUTO_SL_ACCNO\",\"WO_AUTO_SL_DECOMM\",\"WO_FIN_YEAR\",\"WO_NO\",\"WO_DF_ID\",\"WO_DATE\",\"WO_AMT\",\"WO_NO_DECOM\",\"WO_DATE_DECOM\",\"WO_AMT_DECOM\",";
                        strQry += " \"WO_ACC_CODE\",\"WO_OFF_CODE\",\"WO_CRBY\",\"WO_CRON\",\"WO_ACCCODE_DECOM\",\"WO_ISSUED_BY\",\"WO_DTC_CAP\",\"WO_NEW_CAP\",\"WO_REQUEST_LOC\",\"WO_AUTO_NO\",\"WO_LOCCODE\") VALUES";
                        strQry += "('{0}','" + objWorkOrder.sWoSlno + "','" + objWorkOrder.sWoDecomSlno + "','" + objWorkOrder.sWofinyear + "','" + objWorkOrder.sCommWoNo.ToUpper() + "','" + objWorkOrder.sFailureId + "',";
                        strQry += " TO_DATE('" + objWorkOrder.sCommDate + "','dd/MM/yyyy'),'" + objWorkOrder.sCommAmmount + "', '" + objWorkOrder.sDeWoNo.ToUpper() + "',";
                        strQry += " TO_DATE('" + objWorkOrder.sDeCommDate + "','dd/MM/yyyy'),'" + objWorkOrder.sDeCommAmmount + "',";
                        strQry += " '" + objWorkOrder.sAccCode + "','" + objWorkOrder.sLocationCode + "','" + objWorkOrder.sCrBy + "',NOW(),";
                        strQry += " '" + objWorkOrder.sDecomAccCode + "','" + objWorkOrder.sIssuedBy + "','" + objWorkOrder.sCapacity + "',";
                        strQry += " '" + objWorkOrder.sNewCapacity + "','" + objWorkOrder.sRequestLoc + "','{1}','" + objWorkOrder.WoNoLocCode + "')";
                    }
                    strQry = strQry.Replace("'", "''");
                    string sParam = "SELECT COALESCE(MAX(\"WO_SLNO\"),0)+1 FROM \"TBLWORKORDER\"";
                    string sParam1 = "SELECT WONUMBER('" + objWorkOrder.sOfficeCode + "')";
                    sParam1 = sParam1.Replace("'", "''");



                    //Workflow / Approval
                    clsApproval objApproval = new clsApproval();
                    if (objWorkOrder.sboid == "74")
                    {
                        objApproval.sFormName = "WorkOrder_sdo";
                        objApproval.sBOId = "74";
                        objApproval.sOfficeCode = objWorkOrder.sOfficeCode;
                        objApproval.sClientIp = objWorkOrder.sClientIP;
                        objApproval.sCrby = objWorkOrder.sCrBy;
                        objApproval.sWFObjectId = objWorkOrder.sWFOId;
                        objApproval.sWFAutoId = objWorkOrder.sWFAutoId;
                        objApproval.sFailType = objWorkOrder.sFailType;
                        objApproval.sQryValues = strQry;
                        objApproval.sParameterValues = sParam + ";" + sParam1;
                        objApproval.sMainTable = "TBLWORKORDER";
                        objApproval.sGuarentyType = objWorkOrder.sGuarentyType;
                        objApproval.sdtccode = objWorkOrder.sdtccode;
                        objApproval.sDataReferenceId = objWorkOrder.sFailureId;
                    }
                    else
                    {

                        NpgsqlCommand cmdcommworkorderupdate = new NpgsqlCommand("update_wo_rangeallocated_wocreating");
                        cmdcommworkorderupdate.Parameters.AddWithValue("financialyear", objWorkOrder.sWofinyear);
                        cmdcommworkorderupdate.Parameters.AddWithValue("accounthead", objWorkOrder.sAccCode);
                        cmdcommworkorderupdate.Parameters.AddWithValue("divcode", Convert.ToInt32(objWorkOrder.Officecodeforworange));
                        cmdcommworkorderupdate.Parameters.AddWithValue("com_decom_wono", objWorkOrder.sWoSlno);

                        cmdcommworkorderupdate.Parameters.Add("status", NpgsqlDbType.Text);
                        cmdcommworkorderupdate.Parameters["status"].Direction = ParameterDirection.Output;

                        Arrrange[0] = "status";
                        Arrrange = ObjCon.Execute(cmdcommworkorderupdate, Arrrange, 1);
                        if (Arrrange[0].Length > 0)
                        {
                            ComQry = "UPDATE \"TBLWORKORDERALLOTMENTDETAILS\" SET \"WAD_STATUS\" = '2' WHERE \"WAD_WO_NO\" ='"
                            + objWorkOrder.sWoSlno + "' and \"WAD_WRA_ID\"='" + Arrrange[0] + "'";
                            ComQry = ComQry.Replace("'", "''");
                        }

                        NpgsqlCommand cmddecommworkorderupdate = new NpgsqlCommand("update_wo_rangeallocated_wocreating");
                        cmddecommworkorderupdate.Parameters.AddWithValue("financialyear", objWorkOrder.sWofinyear);
                        cmddecommworkorderupdate.Parameters.AddWithValue("accounthead", objWorkOrder.sDecomAccCode);
                        cmddecommworkorderupdate.Parameters.AddWithValue("divcode", Convert.ToInt32(objWorkOrder.Officecodeforworange));
                        cmddecommworkorderupdate.Parameters.AddWithValue("com_decom_wono", objWorkOrder.sWoDecomSlno);

                        cmddecommworkorderupdate.Parameters.Add("status", NpgsqlDbType.Text);
                        cmddecommworkorderupdate.Parameters["status"].Direction = ParameterDirection.Output;

                        Arrrange[0] = "status";
                        Arrrange = ObjCon.Execute(cmddecommworkorderupdate, Arrrange, 1);
                        if (Arrrange[0].Length > 0)
                        {
                            DecComQry = "UPDATE \"TBLWORKORDERALLOTMENTDETAILS\" SET \"WAD_STATUS\" = '2' WHERE \"WAD_WO_NO\" ='"
                                    + objWorkOrder.sWoDecomSlno + "' and \"WAD_WRA_ID\"='" + Arrrange[0] + "'";
                            DecComQry = DecComQry.Replace("'", "''");
                        }


                        objApproval.sFormName = objWorkOrder.sFormName;
                        objApproval.sBOId = "11";
                        objApproval.sOfficeCode = objWorkOrder.sOfficeCode;
                        objApproval.sClientIp = objWorkOrder.sClientIP;
                        objApproval.sCrby = objWorkOrder.sCrBy;
                        objApproval.sWFObjectId = objWorkOrder.sWFOId;
                        objApproval.sWFAutoId = objWorkOrder.sWFAutoId;
                        objApproval.sFailType = objWorkOrder.sFailType;
                        objApproval.sQryValues = strQry + ";" + ComQry + ";" + DecComQry;
                        objApproval.sParameterValues = sParam + ";" + sParam1;
                        objApproval.sMainTable = "TBLWORKORDER";
                        objApproval.sGuarentyType = objWorkOrder.sGuarentyType;
                        objApproval.sdtccode = objWorkOrder.sdtccode;
                        objApproval.sDataReferenceId = objWorkOrder.sFailureId;
                    }
                    if ((objWorkOrder.sTaskType == "1" || objWorkOrder.sTaskType == "4") && objWorkOrder.sFailType != "1")
                    {
                        NpgsqlCommand = new NpgsqlCommand();
                        NpgsqlCommand.Parameters.AddWithValue("sFailureId1", Convert.ToInt32(objWorkOrder.sFailureId));
                        objApproval.sRefOfficeCode = objDatabse.get_value("SELECT \"DF_LOC_CODE\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\" =:sFailureId1", NpgsqlCommand);
                        objApproval.sDescription = "Work Order For Failure Entry of DTC Code " + objWorkOrder.sDTCCode + " and DTC Name " + objWorkOrder.sDTCName + " with WO No " + objWorkOrder.sCommWoNo + "";
                    }
                    else if (objWorkOrder.sTaskType == "1" && objWorkOrder.sFailType == "1")
                    {
                        NpgsqlCommand = new NpgsqlCommand();
                        NpgsqlCommand.Parameters.AddWithValue("sFailureId2", Convert.ToInt32(objWorkOrder.sFailureId));
                        objApproval.sRefOfficeCode = objDatabse.get_value("SELECT \"DF_LOC_CODE\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\" =:sFailureId2", NpgsqlCommand);
                        objApproval.sDescription = "Work Order For Failure Entry of DTC Code " + objWorkOrder.sDTCCode + " and DTC Name " + objWorkOrder.sDTCName + " with WO No " + objWorkOrder.sCommWoNo + "";
                    }
                    else if (objWorkOrder.sTaskType == "2")
                    {
                        NpgsqlCommand = new NpgsqlCommand();
                        NpgsqlCommand.Parameters.AddWithValue("sFailureId3", Convert.ToInt32(objWorkOrder.sFailureId));
                        objApproval.sRefOfficeCode = objDatabse.get_value("SELECT \"DF_LOC_CODE\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\" =:sFailureId3", NpgsqlCommand);
                        objApproval.sDescription = "Work Order For Capacity Enhancement of DTC Code " + objWorkOrder.sDTCCode + " and DTC Name " + objWorkOrder.sDTCName + " with WO No " + objWorkOrder.sCommWoNo + "";
                    }
                    else if (objWorkOrder.sTaskType == "3")
                    {
                        if (objWorkOrder.sTtkStatus == "1")
                        {
                            objApproval.sRefOfficeCode = objWorkOrder.sRequestLoc;
                            objApproval.sDescription = "Work Order For New DTC Commission with TTK FLOW Work Order No: " + objWorkOrder.sCommWoNo;
                        }
                        else
                        {
                            objApproval.sRefOfficeCode = objWorkOrder.sRequestLoc;
                            objApproval.sDescription = "Work Order For New DTC Commission with PTK FLOW Work Order NO : " + objWorkOrder.sCommWoNo;
                        }
                    }
                    string sPrimaryKey = "{0}";
                    objApproval.sColumnNames = "WO_SLNO,WO_AUTO_SL_ACCNO,WO_AUTO_SL_DECOMM,WO_FIN_YEAR,WO_NO,WO_DF_ID,WO_DATE,WO_AMT,WO_NO_DECOM,WO_DATE_DECOM,WO_AMT_DECOM,";
                    objApproval.sColumnNames += "WO_ACC_CODE,WO_OFF_CODE,WO_CRBY,WO_CRON,WO_ACCCODE_DECOM,WO_ISSUED_BY,WO_DTC_CAP,";
                    objApproval.sColumnNames += "WO_NEW_CAP,WO_REQUEST_LOC,WO_DTC_SCHEME,WO_REPAIRER,WO_LOCCODE,";
                    objApproval.sColumnNames += "WO_TTK_STATUS,WO_TTK_AUTO_NO,WO_TTK_VENDOR_NAME,WO_TTK_MANUAL_NO,WO_DWA_NAME,WO_DWA_DATE,WO_RATING";
                    objApproval.sColumnValues = "" + sPrimaryKey + ",'" + objWorkOrder.sWoSlno + "','" + objWorkOrder.sWoDecomSlno + "','" + objWorkOrder.sWofinyear + "'," + objWorkOrder.sCommWoNo.ToUpper() + "," + objWorkOrder.sFailureId + ",";
                    objApproval.sColumnValues += "" + objWorkOrder.sCommDate + "," + objWorkOrder.sCommAmmount + ", " + objWorkOrder.sDeWoNo.ToUpper() + ",";
                    objApproval.sColumnValues += "" + objWorkOrder.sDeCommDate + "," + objWorkOrder.sDeCommAmmount + ",";
                    objApproval.sColumnValues += "" + objWorkOrder.sAccCode + "," + objWorkOrder.sLocationCode + "," + objWorkOrder.sCrBy + ",NOW(),";
                    objApproval.sColumnValues += "" + objWorkOrder.sDecomAccCode + "," + objWorkOrder.sIssuedBy + "," + objWorkOrder.sCapacity + ",";
                    objApproval.sColumnValues += "" + objWorkOrder.sNewCapacity + "," + objWorkOrder.sRequestLoc + "," + objWorkOrder.sDtcScheme + "," + objWorkOrder.sRepairer + ",'" + objWorkOrder.WoNoLocCode + "',";
                    objApproval.sColumnValues += "" + objWorkOrder.sOFCommWoNo + "";
                    // coded by rudra for new TTk concept
                    if (objWorkOrder.sTtkStatus == "1" || objWorkOrder.sTtkStatus == "0")
                    {
                        objApproval.sColumnValues += "" + objWorkOrder.sTtkStatus + "," + objWorkOrder.sTtkAutoNo + "," + objWorkOrder.sTtkVendorName + "," + objWorkOrder.sTtkManual + "," + objWorkOrder.sDWAname + "," + objWorkOrder.sDWAdate + "," + objWorkOrder.sRating + "";
                    }
                    else
                    {
                        objApproval.sColumnValues += ",null,null,'',null,'','',''";
                    }
                    objApproval.sTableNames = "TBLWORKORDER";
                    //Check for Duplicate Approval
                    //bool bApproveResult = objApproval.CheckDuplicateApprove(objApproval);
                    bool bApproveResult = objApproval.CheckDuplicateApprove_Latest(objApproval, objDatabse);
                    if (bApproveResult == false)
                    {
                        Arr[0] = "Selected Record Already Approved";
                        Arr[1] = "2";
                        return Arr;
                    }
                    if (objWorkOrder.sActionType == "M")
                    {
                        //objApproval.SaveWorkFlowData(objApproval);
                        objApproval.SaveWorkFlowData_Latest(objApproval, objDatabse);
                        objWorkOrder.sWFDataId = objApproval.sWFDataId;
                        objWorkOrder.sApprovalDesc = objApproval.sDescription;
                        Arr[2] = objApproval.sBOId;
                    }
                    else
                    {
                        //objApproval.SaveWorkFlowData(objApproval);
                        objApproval.SaveWorkFlowData_Latest(objApproval, objDatabse);
                        objWorkOrder.sWFDataId = objApproval.sWFDataId;    //******new code for workorder report
                        //objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow();
                        objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow_Latest(objDatabse);
                        //objApproval.SaveWorkflowObjects(objApproval);
                        objApproval.SaveWorkflowObjects_Latest(objApproval, objDatabse);
                        objWorkOrder.sWFObjectId = objApproval.sWFObjectId; //new code for workorder report
                    }

                    #endregion
                    objDatabse.CommitTransaction();
                    Arr[0] = "Work Order Created Successfully";
                    Arr[1] = "0";
                    return Arr;
                }
                else
                {
                    ObjCon.BeginTransaction();
                    string[] strResult = new string[2];
                    string[] strArray = new string[2];
                    NpgsqlCommand cmd = new NpgsqlCommand("sp_updateworkorder");
                    cmd.Parameters.AddWithValue("sfailureid", Convert.ToString(objWorkOrder.sFailureId));
                    cmd.Parameters.AddWithValue("scommwono", Convert.ToString(objWorkOrder.sCommWoNo));
                    cmd.Parameters.AddWithValue("slocationcode", Convert.ToString(objWorkOrder.sLocationCode));
                    cmd.Parameters.AddWithValue("swoid", Convert.ToString(objWorkOrder.sWOId));
                    cmd.Parameters.AddWithValue("scommdate", Convert.ToString(objWorkOrder.sCommDate));
                    cmd.Parameters.AddWithValue("scommammount", Convert.ToString(objWorkOrder.sCommAmmount));
                    cmd.Parameters.AddWithValue("sdewono", Convert.ToString(objWorkOrder.sDeWoNo));
                    cmd.Parameters.AddWithValue("sdecommdate", Convert.ToString(objWorkOrder.sDeCommDate));
                    cmd.Parameters.AddWithValue("sdecommammount", Convert.ToString(objWorkOrder.sDeCommAmmount));
                    cmd.Parameters.AddWithValue("sacccode", Convert.ToString(objWorkOrder.sAccCode));
                    cmd.Parameters.AddWithValue("scrby", Convert.ToString(objWorkOrder.sCrBy));
                    cmd.Parameters.AddWithValue("sdecomacccode", Convert.ToString(objWorkOrder.sDecomAccCode));
                    cmd.Parameters.AddWithValue("sissuedby", Convert.ToString(objWorkOrder.sIssuedBy));
                    cmd.Parameters.AddWithValue("scapacity", Convert.ToString(objWorkOrder.sCapacity));
                    cmd.Parameters.AddWithValue("snewcapacity", Convert.ToString(objWorkOrder.sNewCapacity));
                    cmd.Parameters.AddWithValue("srequestloc", Convert.ToString(objWorkOrder.sRequestLoc));
                    if (objWorkOrder.sDeCreditDate != "0")
                    {
                        cmd.Parameters.AddWithValue("sdecreditwO", Convert.ToString(objWorkOrder.sDeCreditWO));
                        cmd.Parameters.AddWithValue("sdecreditamount", Convert.ToString(objWorkOrder.sDeCreditAmount));
                        cmd.Parameters.AddWithValue("sdecreditaccCode", Convert.ToString(objWorkOrder.sDeCreditAccCode));
                        cmd.Parameters.AddWithValue("sdecreditdate", Convert.ToString(objWorkOrder.sDeCreditDate));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("sdecreditwO", Convert.ToString(objWorkOrder.sCreditWO));
                        cmd.Parameters.AddWithValue("sdecreditamount", Convert.ToString(objWorkOrder.sCreditAmount));
                        cmd.Parameters.AddWithValue("sdecreditaccCode", Convert.ToString(objWorkOrder.sCreditAccCode));
                        cmd.Parameters.AddWithValue("sdecreditdate", Convert.ToString(objWorkOrder.sCreditDate));
                    }
                    cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                    cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                    strArray[0] = "op_id";
                    strArray[1] = "msg";
                    strResult = ObjCon.Execute(cmd, strArray, 2);
                    ObjCon.CommitTransaction();
                    return strResult;
                }
            }
            catch (Exception ex)
            {
                objDatabse.RollBackTrans();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                //  return Arr;
                throw ex;
            }
        }
        /// <summary>
        /// to generate work order number
        /// </summary>
        /// <param name="sOfficeCode"></param>
        /// <returns></returns>
        public string GenerateWOautoNo(string sOfficeCode)
        {
            try
            {
                PGSqlConnection objCon = new PGSQL.DAL.PGSqlConnection(Constants.Password);

                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;
                string QryKey = string.Empty;
                string sWOAutoNo = string.Empty;
                #region Old Inline query
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", sOfficeCode);
                //sWOAutoNo = objCon.get_value("select max(maxnum)+1 from (SELECT  COALESCE(MAX(CAST(\"WO_TTK_AUTO_NO\" AS INT8)),0) as maxnum FROM \"VIEW_WORKORDER\" WHERE CAST(\"WO_TTK_AUTO_NO\" AS TEXT) LIKE :sOfficeCode||'%' union all select COALESCE(max (CAST(\"WO_NO\" AS INT8)),0)  as maxnum  from (select 	replace(cast(unnest(xpath('(./TBLWORKORDER/WO_TTK_AUTO_NO)/text()', \"WFO_DATA\"::XML))::text as text), '''', '') AS \"WO_NO\" from \"TBLWFODATA\"  left join \"TBLWORKFLOWOBJECTS\" on cast(\"WFO_ID\" as text)=\"WO_WFO_ID\"  where \"WO_BO_ID\"=11 and \"WO_RECORD_ID\" <0 and \"WO_APPROVE_STATUS\"=0 )C where \"WO_NO\" not like '%-%' AND \"WO_NO\" !='null' )c ", NpgsqlCommand);
                #endregion

                QryKey = "GET_GENERATEWOAUTONO";
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsworkorder");
                cmd.Parameters.AddWithValue("p_key", QryKey);
                cmd.Parameters.AddWithValue("p_value_1", sOfficeCode);
                cmd.Parameters.AddWithValue("p_value_2", "");
                sWOAutoNo = ObjBasCon.StringGetValue(cmd);

                if (sWOAutoNo.Length == 1)
                {
                    //4 digit Section Code
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        sFinancialYear = System.DateTime.Now.ToString("yy");
                    }
                    else
                    {
                        sFinancialYear = System.DateTime.Now.AddYears(-1).ToString("yy");
                    }

                    sWOAutoNo = sOfficeCode + sFinancialYear + "001";
                }
                else
                {
                    //4 digit Section Code 13111802336
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                        {
                            sFinancialYear = System.DateTime.Now.ToString("yy");
                        }
                        if (sFinancialYear == sWOAutoNo.Substring(5, 2))
                        {
                            return sWOAutoNo;
                        }
                        else
                        {
                            sWOAutoNo = sOfficeCode + sFinancialYear + "001";
                        }
                    }
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) < 03)
                    {
                        return sWOAutoNo;
                    }


                }

                return sWOAutoNo;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }
        /// <summary>
        /// to save the file
        /// </summary>
        /// <param name="objWO"></param>
        /// <returns></returns>
        public bool SaveWOFilePath(clsWorkOrder objWO)
        {
            try
            {

                //FTP Parameter
                string sFTPLink = string.Empty;
                string sFTPUserName = string.Empty;
                string sFTPPassword = string.Empty;
                string sFolderName = "WORKORDER";
                string SFTPPath = Convert.ToString(ConfigurationManager.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(ConfigurationManager.AppSettings["SFTPmainfolder"]);

                string sWOFileName = string.Empty;
                string sDirectory = string.Empty;

                //  Photo Save DTLMSDocs
                clsCommon objComm = new clsCommon();
                DataTable dt = objComm.GetAppSettings();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPMAINLINK")
                    {
                        sFTPLink = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
                    }
                    else if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPUSERNAME")
                    {
                        sFTPUserName = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
                    }
                    else if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPPASSWORD")
                    {
                        sFTPPassword = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
                    }
                }
                clsSFTP objFtp = new clsSFTP(SFTPPath, sFTPUserName, sFTPPassword);

                bool Isuploaded;
                sDirectory = objWO.sWOFilePath;

                // Create Directory
                bool IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sFolderName + "/");
                if (IsExists == false)
                {

                    objFtp.createDirectory(objWO.sWOId);
                }

                sWOFileName = Path.GetFileName(objWO.sWOFilePath);

                if (sWOFileName != "")
                {

                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sFolderName + "/" + objWO.sWOId + "/");
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(SFTPmainfolder + sFolderName + "/" + objWO.sWOId);
                        }

                        Isuploaded = objFtp.upload(SFTPmainfolder + sFolderName + "/" + objWO.sWOId, sWOFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objWO.sWOFilePath = SFTPmainfolder + sFolderName + "/" + objWO.sWOId + "/" + sWOFileName;

                        }
                    }
                }
                #region old Qry
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "UPDATE \"TBLWORKORDER\" SET \"WO_FILE_PATH\" =:sWOFilePath WHERE \"WO_SLNO\" =:sWOId";
                //NpgsqlCommand.Parameters.AddWithValue("sWOFilePath", objWO.sWOFilePath);
                //NpgsqlCommand.Parameters.AddWithValue("sWOId", Convert.ToInt32(objWO.sWOId));
                //ObjCon.ExecuteQry(strQry, NpgsqlCommand);
                #endregion

                string[] Arr = new string[1];
                NpgsqlCommand cmd = new NpgsqlCommand("proc_save_wo_filepath");
                cmd.Parameters.AddWithValue("p_wofilepath", objWO.sWOFilePath);
                cmd.Parameters.AddWithValue("p_woid", Convert.ToInt32(objWO.sWOId));
                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                Arr[0] = "op_id";
                Arr = ObjCon.Execute(cmd, Arr, 1);

                if (Arr[0] == "0")
                {
                    return true;
                }
                else
                {
                    return false;
                }



            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }
        /// <summary>
        /// to load all details of work order
        /// </summary>
        /// <param name="objWorkOrder"></param>
        /// <returns></returns>

        public DataTable LoadAlreadyWorkOrder(clsWorkOrder objWorkOrder)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                //Type=1----> Failure ;  Type=2-----------> Enhancement
                NpgsqlCommand cmd = new NpgsqlCommand("sp_loadalreadyapprovedworkorder");
                //  NpgsqlCommand cmd = new NpgsqlCommand("sp_loadalreadyapprovedworkorder_dummy");
                cmd.Parameters.AddWithValue("stasktype", Convert.ToString(objWorkOrder.sTaskType));
                cmd.Parameters.AddWithValue("sofficecode", Convert.ToString(objWorkOrder.sOfficeCode));
                cmd.Parameters.AddWithValue("fromdate", objWorkOrder.FromDate == null ? "" : objWorkOrder.FromDate);
                cmd.Parameters.AddWithValue("todate", objWorkOrder.ToDate == null ? "" : objWorkOrder.ToDate);
                dt = ObjCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }
        }

        /// <summary>
        ///  to get load all  work order details
        /// </summary>
        /// <param name="objWorkOrder"></param>
        /// <returns></returns>
        public DataTable LoadAllWorkOrder(clsWorkOrder objWorkOrder)
        {

            string strQry = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                //Type=1----> Failure ;  Type=2-----------> Enhancement
                NpgsqlCommand cmd = new NpgsqlCommand("sp_loadallworkorder");
                cmd.Parameters.AddWithValue("stasktype", objWorkOrder.sTaskType);
                cmd.Parameters.AddWithValue("sofficecode", objWorkOrder.sOfficeCode);
                dt = ObjCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }
        }

        /// <summary>
        /// get work order details
        /// </summary>
        /// <param name="objWorkOrder"></param>
        /// <returns></returns>
        public object GetWorkOrderDetails(clsWorkOrder objWorkOrder)
        {
            try
            {
                string strQry = string.Empty;
                DataTable dtWODetails = new DataTable();

                NpgsqlCommand cmd = new NpgsqlCommand("sp_getworkorderdetails");
                cmd.Parameters.AddWithValue("sfailureid", objWorkOrder.sFailureId);
                dtWODetails = ObjCon.FetchDataTable(cmd);

                if (dtWODetails.Rows.Count > 0)
                {

                    objWorkOrder.sWOId = Convert.ToString(dtWODetails.Rows[0]["WO_SLNO"]).Trim();
                    objWorkOrder.sAccCode = Convert.ToString(dtWODetails.Rows[0]["WO_ACC_CODE"]).Trim();
                    objWorkOrder.sCrBy = Convert.ToString(dtWODetails.Rows[0]["WO_CRBY"]).Trim();
                    objWorkOrder.sCommWoNo = Convert.ToString(dtWODetails.Rows[0]["WO_NO"]).Trim();
                    objWorkOrder.sCommDate = Convert.ToString(dtWODetails.Rows[0]["WO_DATE"]).Trim();
                    objWorkOrder.sCommAmmount = Convert.ToString(dtWODetails.Rows[0]["WO_AMT"]).Trim();
                    objWorkOrder.sDeWoNo = Convert.ToString(dtWODetails.Rows[0]["WO_NO_DECOM"]).Trim();
                    objWorkOrder.sDeCommDate = Convert.ToString(dtWODetails.Rows[0]["WO_DATE_DECOM"]).Trim();
                    objWorkOrder.sDeCommAmmount = Convert.ToString(dtWODetails.Rows[0]["WO_AMT_DECOM"]).Trim();
                    objWorkOrder.sIssuedBy = Convert.ToString(dtWODetails.Rows[0]["WO_ISSUED_BY"]).Trim();
                    objWorkOrder.sDecomAccCode = Convert.ToString(dtWODetails.Rows[0]["WO_ACCCODE_DECOM"]).Trim();
                    objWorkOrder.sFailureId = Convert.ToString(dtWODetails.Rows[0]["WO_DF_ID"]).Trim();
                    objWorkOrder.sFailureDate = Convert.ToString(dtWODetails.Rows[0]["DF_FAILED_DATE"]).Trim();
                    objWorkOrder.sNewCapacity = Convert.ToString(dtWODetails.Rows[0]["WO_NEW_CAP"]).Trim();
                    objWorkOrder.sRequestLoc = Convert.ToString(dtWODetails.Rows[0]["WO_REQUEST_LOC"]).Trim();
                    objWorkOrder.sEnhancedCapacity = Convert.ToString(dtWODetails.Rows[0]["DF_ENHANCE_CAPACITY"]).Trim();
                    if (Convert.ToString(dtWODetails.Rows[0]["WO_NO_CREDIT"]).Trim() == "2")
                    {
                        objWorkOrder.sDeCreditWO = Convert.ToString(dtWODetails.Rows[0]["WO_NO_CREDIT"]).Trim();
                        objWorkOrder.sDeCreditAmount = Convert.ToString(dtWODetails.Rows[0]["WO_AMT_CREDIT"]).Trim();
                        objWorkOrder.sDeCreditAccCode = Convert.ToString(dtWODetails.Rows[0]["WO_ACC_CRERDIT"]).Trim();
                        objWorkOrder.sDeCreditDate = Convert.ToString(dtWODetails.Rows[0]["WO_DATE_CREDIT"]).Trim();
                    }
                    else
                    {
                        objWorkOrder.sCreditWO = Convert.ToString(dtWODetails.Rows[0]["WO_NO_CREDIT"]).Trim();
                        objWorkOrder.sCreditAmount = Convert.ToString(dtWODetails.Rows[0]["WO_AMT_CREDIT"]).Trim();
                        objWorkOrder.sCreditAccCode = Convert.ToString(dtWODetails.Rows[0]["WO_ACC_CRERDIT"]).Trim();
                        objWorkOrder.sCreditDate = Convert.ToString(dtWODetails.Rows[0]["WO_DATE_CREDIT"]).Trim();

                    }
                    #region Old inline query
                    //NpgsqlCommand = new NpgsqlCommand();
                    //strQry = "SELECT \"WO_WFO_ID\" FROM (SELECT \"WO_ID\",\"WO_WFO_ID\",row_number() over (PARTITION by \"WO_SLNO\" ";
                    //strQry += " ORDER BY \"WO_ID\" desc) as \"RNUM\" FROM \"TBLWORKORDER\",\"TBLWORKFLOWOBJECTS\" WHERE \"WO_RECORD_ID\"";
                    //strQry += " =\"WO_SLNO\" AND \"WO_BO_ID\"='11' AND \"WO_SLNO\"=:sWOId)A WHERE \"RNUM\" = 1";
                    //NpgsqlCommand.Parameters.AddWithValue("sWOId", Convert.ToInt32(objWorkOrder.sWOId));
                    //objWorkOrder.sWFDataId = ObjCon.get_value(strQry, NpgsqlCommand);
                    #endregion

                    string QryKey = string.Empty;
                    QryKey = "GET_WO_WFO_ID";
                    NpgsqlCommand NpgsqlCom = new NpgsqlCommand("fetch_getvalue_clsworkorder");
                    NpgsqlCom.Parameters.AddWithValue("p_key", QryKey);
                    NpgsqlCom.Parameters.AddWithValue("p_value_1", objWorkOrder.sWOId);
                    NpgsqlCom.Parameters.AddWithValue("p_value_2", "");
                    objWorkOrder.sWFDataId = ObjBasCon.StringGetValue(NpgsqlCom);

                    GetWODetailsFromXML(objWorkOrder);
                }
                return objWorkOrder;
            }

            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objWorkOrder;

            }

        }
        /// <summary>
        /// to validate update data
        /// </summary>
        /// <param name="strFailureId"></param>
        /// <param name="sWOSlno"></param>
        /// <param name="sType"></param>
        /// <returns></returns>
        public bool ValidateUpdate(string strFailureId, string sWOSlno, string sType)
        {
            try
            {
                #region Old ValidateUpdate inline query
                //DataTable dt = new DataTable();
                ////OleDbDataReader dr;
                //string strQry = string.Empty;

                //if (sType != "3")
                //{
                //    NpgsqlCommand = new NpgsqlCommand();
                //    strQry = " SELECT \"TI_WO_SLNO\" FROM \"TBLWORKORDER\",\"TBLINDENT\",\"TBLDTCFAILURE\" WHERE \"WO_SLNO\"=\"TI_WO_SLNO\" AND ";
                //    strQry += " \"DF_ID\"=\"WO_DF_ID\" AND \"WO_DF_ID\" =:strFailureId";
                //    NpgsqlCommand.Parameters.AddWithValue("strFailureId", Convert.ToInt32(strFailureId));
                //}
                //else
                //{
                //    strQry = "select \"TI_WO_SLNO\" from \"TBLWORKORDER\",\"TBLINDENT\" WHERE \"WO_SLNO\"=\"TI_WO_SLNO\" AND ";
                //    strQry += "  \"WO_SLNO\" =:sWOSlno";
                //    NpgsqlCommand.Parameters.AddWithValue("sWOSlno", Convert.ToInt32(sWOSlno));
                //}
                //string sSLNo = ObjCon.get_value(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_validateupdate_for_clsworkorder");
                cmd.Parameters.AddWithValue("p_failureid", (strFailureId ?? ""));
                cmd.Parameters.AddWithValue("p_woslno", (sWOSlno ?? ""));
                cmd.Parameters.AddWithValue("p_type", (sType ?? ""));
                string sSLNo = ObjBasCon.StringGetValue(cmd);

                if (sSLNo.Length > 0)
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
        ///  to get basic details of work order
        /// </summary>
        /// <param name="objWO"></param>
        /// <returns></returns>
        public clsWorkOrder GetWOBasicDetails(clsWorkOrder objWO)
        {
            string strQry = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getwobasicdetails");
                cmd.Parameters.AddWithValue("swoid", objWO.sWOId);
                dt = ObjCon.FetchDataTable(cmd);

                if (dt.Rows.Count > 0)
                {
                    objWO.sCommWoNo = dt.Rows[0]["WO_NO"].ToString();
                    objWO.sCommDate = dt.Rows[0]["WO_DATE"].ToString();
                    objWO.sDTCName = dt.Rows[0]["DT_NAME"].ToString();
                    objWO.sTCCode = dt.Rows[0]["TC_CODE"].ToString();
                    objWO.sCrBy = dt.Rows[0]["US_FULL_NAME"].ToString();
                    objWO.sFailureId = dt.Rows[0]["DF_ID"].ToString();
                    objWO.sFailureDate = dt.Rows[0]["DF_FAILED_DATE"].ToString();
                    objWO.sNewCapacity = dt.Rows[0]["WO_NEW_CAP"].ToString();
                    objWO.sDTCCode = dt.Rows[0]["DT_CODE"].ToString();
                    objWO.sTCId = dt.Rows[0]["TC_ID"].ToString();
                    objWO.sDTCId = dt.Rows[0]["DT_ID"].ToString();
                    objWO.sLocationCode = dt.Rows[0]["DF_LOC_CODE"].ToString();
                }

                return objWO;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objWO;
            }
        }

        /// <summary>
        ///  to get commission and decommision account code
        /// </summary>
        /// <param name="objWO"></param>
        /// <returns></returns>
        public clsWorkOrder GetCommDecommAccCode(clsWorkOrder objWO)
        {
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();

                NpgsqlCommand cmd = new NpgsqlCommand("sp_getcommdecommacccode");
                cmd.Parameters.AddWithValue("scapacity", objWO.sCapacity);
                dt = ObjCon.FetchDataTable(cmd);

                if (dt.Rows.Count > 0)
                {
                    objWO.sAccCode = Convert.ToString(dt.Rows[0]["MD_COMM_ACCCODE"]);
                    objWO.sDecomAccCode = Convert.ToString(dt.Rows[0]["MD_DECOMM_ACCCODE"]);
                    objWO.sDeCreditAccCode = Convert.ToString(dt.Rows[0]["MD_CREDIT_ACCCODE"]);
                    objWO.sEnhanceAccCode = Convert.ToString(dt.Rows[0]["MD_ENHANCE_ACCCODE"]);
                }
                return objWO;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objWO;
            }
        }
        /// <summary>
        /// to Get Work order number
        /// </summary>
        /// <param name="objWO"></param>
        /// <returns></returns>
        public clsWorkOrder GetWorkordernumber(clsWorkOrder objWO)
        {
            try
            {
                ObjCon.BeginTransaction();
                string strQry = string.Empty;
                DataTable dt = new DataTable();
                DataTable dtrangeallocated = new DataTable();
                string tempslno = string.Empty;
                string sQry = string.Empty;
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getautoworkorder_commission");
                cmd.Parameters.AddWithValue("sfailureid", objWO.sFailureId);
                cmd.Parameters.AddWithValue("sofficecode", objWO.sOfficeCode);
                dt = ObjCon.FetchDataTable(cmd);

                if (dt.Rows.Count > 0)
                {
                    string[] tokens = dt.Rows[0]["sp_getautoworkorder_commission"].ToString().Split('/');

                    if (tokens[0].Length > 0)
                    {
                        objWO.sDiv_Locaton_Code = tokens[0];
                        objWO.sAccCode = tokens[1];
                        objWO.sFinancialYear = tokens[2];
                    }

                    NpgsqlCommand cmdrangeallocated = new NpgsqlCommand("get_workorder_rangeallocated");
                    cmdrangeallocated.Parameters.AddWithValue("financialyear", objWO.sFinancialYear);
                    cmdrangeallocated.Parameters.AddWithValue("accounthead", objWO.sAccCode);
                    cmdrangeallocated.Parameters.AddWithValue("divcode", Convert.ToInt32(objWO.sOfficeCode));
                    cmdrangeallocated.Parameters.AddWithValue("com_decomflag", Convert.ToInt32(objWO.Commissionid));

                    dtrangeallocated = ObjCon.FetchDataTable(cmdrangeallocated);
                    if (dtrangeallocated.Rows.Count > 0)
                    {
                        objWO.sTempslno = Convert.ToString(dtrangeallocated.Rows[0]["WAD_WO_NO"]);
                    }
                }
                ObjCon.CommitTransaction();
                return objWO;

            }
            catch (Exception ex)
            {
                ObjCon.RollBackTrans();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objWO;
            }
        }
        /// <summary>
        /// to Get decommission Work order number
        /// </summary>
        /// <param name="objWO"></param>
        /// <returns></returns>
        public clsWorkOrder GetDecommnumber(clsWorkOrder objWO)
        {
            try
            {
                ObjCon.BeginTransaction();
                string strQry = string.Empty;
                DataTable dt = new DataTable();
                DataTable dtrangeallocated = new DataTable();
                string tempslno = string.Empty;
                string sQry = string.Empty;
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getautocommworkorder_decommission");
                cmd.Parameters.AddWithValue("sofficecode", objWO.sOfficeCode);

                dt = ObjCon.FetchDataTable(cmd);

                if (dt.Rows.Count > 0)
                {
                    string[] tokens = dt.Rows[0]["sp_getautocommworkorder_decommission"].ToString().Split('/');

                    if (tokens[0].Length > 0)
                    {
                        objWO.sDiv_Locaton_Code = tokens[0];
                        objWO.sDecomAccCode = tokens[1];
                        objWO.sFinancialYear = tokens[2];
                    }
                }
                NpgsqlCommand cmdrangeallocated = new NpgsqlCommand("get_workorder_rangeallocated");
                cmdrangeallocated.Parameters.AddWithValue("financialyear", objWO.sFinancialYear);
                cmdrangeallocated.Parameters.AddWithValue("accounthead", objWO.sDecomAccCode);
                cmdrangeallocated.Parameters.AddWithValue("divcode", Convert.ToInt32(objWO.sOfficeCode));
                cmdrangeallocated.Parameters.AddWithValue("com_decomflag", Convert.ToInt32(objWO.Decommisionid));
                dtrangeallocated = ObjCon.FetchDataTable(cmdrangeallocated);
                if (dtrangeallocated.Rows.Count > 0)
                {
                    objWO.sTempslno = Convert.ToString(dtrangeallocated.Rows[0]["WAD_WO_NO"]);
                }
                ObjCon.CommitTransaction();
                return objWO;
            }
            catch (Exception ex)
            {
                ObjCon.RollBackTrans();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objWO;
            }
        }
        /// <summary>
        ///  to get dtcaccount code
        /// </summary>
        /// <param name="objWO"></param>
        /// <returns></returns>
        public clsWorkOrder GetDTCAccCode(clsWorkOrder objWO)
        {
            try
            {
                #region Old Inline query GetDTCAccCode
                //string strQry = string.Empty;
                //DataTable dt = new DataTable();
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "SELECT \"SCHM_ACCCODE\" FROM \"TBLDTCSCHEME\" WHERE \"SCHM_ID\" =:sDtcScheme";
                //NpgsqlCommand.Parameters.AddWithValue("sDtcScheme", objWO.sDtcScheme);
                //objWO.sAccCode = ObjCon.get_value(strQry, NpgsqlCommand);
                #endregion

                string QryKey = string.Empty;
                QryKey = "GET_DTCACCCODE";
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsworkorder");
                cmd.Parameters.AddWithValue("p_key", QryKey);
                cmd.Parameters.AddWithValue("p_value_1", Convert.ToString(objWO.sDtcScheme));
                cmd.Parameters.AddWithValue("p_value_2", "");
                objWO.sAccCode = ObjBasCon.StringGetValue(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objWO;
            }
            return objWO;
        }

        #region NewDTC
        /// <summary>
        ///  to load new dtc work order
        /// </summary>
        /// <param name="objWorkOrder"></param>
        /// <returns></returns>
        public DataTable LoadNewDTCWO(clsWorkOrder objWorkOrder)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getcommdecommacccode");
                cmd.Parameters.AddWithValue("sofficecode", objWorkOrder.sOfficeCode);
                dt = ObjCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        /// <summary>
        ///  to Get WO Details Fo rNew DTC
        /// </summary>
        /// <param name="objWorkOrder"></param>
        /// <returns></returns>
        public object GetWODetailsForNewDTC(clsWorkOrder objWorkOrder)
        {
            try
            {
                string strQry = string.Empty;
                DataTable dtWODetails = new DataTable();

                NpgsqlCommand cmd = new NpgsqlCommand("sp_getwodetailsfornewdtc");
                cmd.Parameters.AddWithValue("swoid", objWorkOrder.sWOId);
                dtWODetails = ObjCon.FetchDataTable(cmd);

                if (dtWODetails.Rows.Count > 0)
                {

                    objWorkOrder.sWOId = dtWODetails.Rows[0]["WO_SLNO"].ToString();
                    objWorkOrder.sAccCode = dtWODetails.Rows[0]["WO_ACC_CODE"].ToString();
                    objWorkOrder.sCommWoNo = dtWODetails.Rows[0]["WO_NO"].ToString();
                    objWorkOrder.sCommDate = dtWODetails.Rows[0]["WO_DATE"].ToString();
                    objWorkOrder.sCommAmmount = dtWODetails.Rows[0]["WO_AMT"].ToString();
                    objWorkOrder.sIssuedBy = dtWODetails.Rows[0]["WO_ISSUED_BY"].ToString();
                    objWorkOrder.sNewCapacity = dtWODetails.Rows[0]["WO_NEW_CAP"].ToString();
                    objWorkOrder.sCrBy = dtWODetails.Rows[0]["US_FULL_NAME"].ToString();
                    objWorkOrder.sRequestLoc = dtWODetails.Rows[0]["WO_REQUEST_LOC"].ToString();
                    objWorkOrder.sRating = dtWODetails.Rows[0]["WO_RATING"].ToString();
                }
                return objWorkOrder;
            }

            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objWorkOrder;

            }

        }

        #endregion
        /// <summary>
        ///  to Send SMS to SectionOfficer
        /// </summary>
        /// <param name="sFailureId"></param>
        /// <param name="sDTCCode"></param>
        /// <param name="sWONo"></param>
        /// <param name="sDTCName"></param>
        public void SendSMStoSectionOfficer(string sFailureId, string sDTCCode, string sWONo, string sDTCName)
        {
            string sOfficeCode = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                #region Old Inline query SendSMStoSectionOfficer
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sFailureId", sFailureId);
                //sOfficeCode = ObjCon.get_value("SELECT \"DF_LOC_CODE\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\" =:sFailureId", NpgsqlCommand);
                #endregion

                string QryKey = string.Empty;
                QryKey = "GET_DF_LOC_CODE";
                NpgsqlCommand cmd_DF_LOC_CODE = new NpgsqlCommand("fetch_getvalue_clsworkorder");
                cmd_DF_LOC_CODE.Parameters.AddWithValue("p_key", QryKey);
                cmd_DF_LOC_CODE.Parameters.AddWithValue("p_value_1", Convert.ToString(sFailureId ?? ""));
                cmd_DF_LOC_CODE.Parameters.AddWithValue("p_value_2", "");
                sOfficeCode = ObjBasCon.StringGetValue(cmd_DF_LOC_CODE);



                NpgsqlCommand cmd = new NpgsqlCommand("sp_getsocontact");
                cmd.Parameters.AddWithValue("sofficecode", sOfficeCode);
                dt = ObjCon.FetchDataTable(cmd);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sFullName = Convert.ToString(dt.Rows[i]["US_FULL_NAME"]);
                    string sMobileNo = Convert.ToString(dt.Rows[i]["US_MOBILE_NO"]);

                    clsCommunication objcomm = new clsCommunication();
                    objcomm.sSMSkey = "SMStoWorkOrder";
                    objcomm = objcomm.GetsmsTempalte(objcomm);

                    string sSMSText = String.Format(objcomm.sSMSTemplate, sDTCCode, sWONo, sDTCName);
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


        #region WorkFlow XML
        /// <summary>
        ///  to Get WO Details From XM
        /// </summary>
        /// <param name="objWorkOrder"></param>
        /// <returns></returns>
        public clsWorkOrder GetWODetailsFromXML(clsWorkOrder objWorkOrder)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable dtWODetails = new DataTable();

                dtWODetails = objApproval.GetDatatableFromXML(objWorkOrder.sWFDataId);
                if (dtWODetails.Rows.Count > 0)
                {
                    objWorkOrder.sAccCode = Convert.ToString(dtWODetails.Rows[0]["WO_ACC_CODE"]).Trim();
                    objWorkOrder.sCrBy = Convert.ToString(dtWODetails.Rows[0]["WO_CRBY"]).Trim();
                    objWorkOrder.sCommWoNo = Convert.ToString(dtWODetails.Rows[0]["WO_NO"]).Trim();
                    objWorkOrder.sCommDate = Convert.ToString(dtWODetails.Rows[0]["WO_DATE"]).Trim();
                    objWorkOrder.sCommAmmount = Convert.ToString(dtWODetails.Rows[0]["WO_AMT"]).Trim();
                    objWorkOrder.sDeWoNo = Convert.ToString(dtWODetails.Rows[0]["WO_NO_DECOM"]).Trim();
                    objWorkOrder.sDeCommDate = Convert.ToString(dtWODetails.Rows[0]["WO_DATE_DECOM"]).Trim();
                    objWorkOrder.sDeCommAmmount = Convert.ToString(dtWODetails.Rows[0]["WO_AMT_DECOM"]).Trim();
                    objWorkOrder.sIssuedBy = Convert.ToString(dtWODetails.Rows[0]["WO_ISSUED_BY"]).Trim();
                    objWorkOrder.sDecomAccCode = Convert.ToString(dtWODetails.Rows[0]["WO_ACCCODE_DECOM"]).Trim();
                    objWorkOrder.sFailureId = Convert.ToString(dtWODetails.Rows[0]["WO_DF_ID"]).Trim();
                    objWorkOrder.sNewCapacity = Convert.ToString(dtWODetails.Rows[0]["WO_NEW_CAP"]).Trim();
                    objWorkOrder.sRequestLoc = Convert.ToString(dtWODetails.Rows[0]["WO_REQUEST_LOC"]).Trim();
                    objWorkOrder.sDtcScheme = Convert.ToInt32(dtWODetails.Rows[0]["WO_DTC_SCHEME"]);
                    objWorkOrder.sRepairer = Convert.ToString(dtWODetails.Rows[0]["WO_REPAIRER"]);

                    if (dtWODetails.Columns.Contains("WO_NO_OF"))
                    {
                        if (Convert.ToString(dtWODetails.Rows[0]["WO_NO_OF"]).Trim() != "0")
                        {
                            objWorkOrder.sOFCommWoNo = Convert.ToString(dtWODetails.Rows[0]["WO_NO_OF"]).Trim();
                        }
                    }
                    if (dtWODetails.Columns.Contains("WO_DATE_OF"))
                    {
                        if (Convert.ToString(dtWODetails.Rows[0]["WO_DATE_OF"]).Trim() != "0")
                        {
                            objWorkOrder.sOFCommDate = Convert.ToString(dtWODetails.Rows[0]["WO_DATE_OF"]).Trim();
                        }
                    }
                    if (dtWODetails.Columns.Contains("WO_AMT_OF"))
                    {
                        objWorkOrder.sOFCommAmmount = Convert.ToString(dtWODetails.Rows[0]["WO_AMT_OF"]).Trim();
                    }
                    if (dtWODetails.Columns.Contains("WO_TTK_STATUS"))
                    {
                        objWorkOrder.sTtkStatus = Convert.ToString(dtWODetails.Rows[0]["WO_TTK_STATUS"]);
                    }
                    if (dtWODetails.Columns.Contains("WO_TTK_VENDOR_NAME"))
                    {
                        objWorkOrder.sTtkVendorName = Convert.ToString(dtWODetails.Rows[0]["WO_TTK_VENDOR_NAME"]);
                    }
                    if (dtWODetails.Columns.Contains("WO_TTK_MANUAL_NO"))
                    {
                        objWorkOrder.sTtkManual = Convert.ToString(dtWODetails.Rows[0]["WO_TTK_MANUAL_NO"]);
                    }
                    if (dtWODetails.Columns.Contains("WO_TTK_AUTO_NO"))
                    {
                        objWorkOrder.sTtkAutoNo = Convert.ToString(dtWODetails.Rows[0]["WO_TTK_AUTO_NO"]);

                    }
                    if (dtWODetails.Columns.Contains("WO_DWA_NAME"))
                    {
                        objWorkOrder.sDWAname = Convert.ToString(dtWODetails.Rows[0]["WO_DWA_NAME"]);
                    }
                    if (dtWODetails.Columns.Contains("WO_DWA_DATE"))
                    {
                        objWorkOrder.sDWAdate = Convert.ToString(dtWODetails.Rows[0]["WO_DWA_DATE"]);
                    }
                    if (dtWODetails.Columns.Contains("WO_RATING"))
                    {
                        objWorkOrder.sRating = Convert.ToString(dtWODetails.Rows[0]["WO_RATING"]);
                    }
                    if (dtWODetails.Columns.Contains("WO_ACC_OF"))
                    {
                        objWorkOrder.sOFAccCode = Convert.ToString(dtWODetails.Rows[0]["WO_ACC_OF"]).Trim();
                    }
                    string failtype = string.Empty;
                    string guaranteetype = string.Empty;
                    if (objWorkOrder.sFailureId != null && objWorkOrder.sFailureId != "")
                    {
                        #region old inline query 
                        //failtype = ObjCon.get_value("SELECT \"EST_FAIL_TYPE\" from \"TBLESTIMATIONDETAILS\" WHERE \"EST_FAILUREID\"='" + objWorkOrder.sFailureId + "'");
                        #endregion

                        string QryKey = string.Empty;
                        QryKey = "GET_EST_FAIL_TYPE";
                        NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsworkorder");
                        cmd.Parameters.AddWithValue("p_key", QryKey);
                        cmd.Parameters.AddWithValue("p_value_1", Convert.ToString(objWorkOrder.sFailureId ?? ""));
                        cmd.Parameters.AddWithValue("p_value_2", "");
                        failtype =  ObjBasCon.StringGetValue(cmd);

                        #region Old inline query 
                        //guaranteetype = ObjCon.get_value("SELECT \"EST_GUARANTEETYPE\" from \"TBLESTIMATIONDETAILS\" WHERE \"EST_FAILUREID\"='" + objWorkOrder.sFailureId + "'");
                        #endregion

                        QryKey = string.Empty;
                        QryKey = "GET_EST_GUARANTEETYPE";
                        NpgsqlCommand cmd_guaranteetype = new NpgsqlCommand("fetch_getvalue_clsworkorder");
                        cmd_guaranteetype.Parameters.AddWithValue("p_key", QryKey);
                        cmd_guaranteetype.Parameters.AddWithValue("p_value_1", Convert.ToString(objWorkOrder.sFailureId ?? ""));
                        cmd_guaranteetype.Parameters.AddWithValue("p_value_2", "");
                        guaranteetype = ObjBasCon.StringGetValue(cmd_guaranteetype);
                    }

                }
                return objWorkOrder;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objWorkOrder;
            }
        }

        #endregion

        /// <summary>
        ///  to get Created By User Name
        /// </summary>
        /// <param name="sDataId"></param>
        /// <param name="sOffCode"></param>
        /// <returns></returns>
        public ArrayList getCreatedByUserName(string sDataId, string sOffCode)
        {
            ArrayList strQrylist = new ArrayList();
            string sWoid = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                // this sWoid is not used aney ware in the method.
               // sWoid = ObjCon.get_value("SELECT MAX(\"WO_ID\") FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\"='11' AND \"WO_DATA_ID\"='" + sDataId + "' AND CAST(\"WO_OFFICE_CODE\" AS TEXT) LIKE SUBSTR('" + sOffCode + "',1,3)");

                if (sOffCode.Length < 3)
                {
                    sOffCode = clsStoreOffice.Getofficecode(sOffCode);

                }
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getcreatedbyusername");
                cmd.Parameters.AddWithValue("sofficecode", sOffCode == "" ? "0" : sOffCode);
                dt = ObjCon.FetchDataTable(cmd);

                for (int i = 0; i < 4; i++)
                {
                    if (dt.Rows.Count > i)
                    {
                        if (dt.Rows[i][0].ToString() != "" || dt.Rows[i][0].ToString() != null)
                            strQrylist.Add(dt.Rows[i][0].ToString());
                    }
                    else
                        strQrylist.Add("");

                }
                return strQrylist;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return strQrylist;
            }
        }
        /// <summary>
        ///  to get wo data ID
        /// </summary>
        /// <param name="sFailureId"></param>
        /// <param name="sWOSLno"></param>
        /// <returns></returns>
        public string getWoDataId(string sFailureId, string sWOSLno)
        {
            string sWoDataId = string.Empty;
            try
            {
                #region Old inline query getWoDataId
                //string sQry = string.Empty;                
                //NpgsqlCommand = new NpgsqlCommand();
                //if (sFailureId != null && sFailureId != "")
                //{
                //    sQry = "SELECT MAX(\"WO_WFO_ID\") FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_DATA_ID\"='" + Convert.ToInt32(sFailureId) + "' AND \"WO_BO_ID\"='11'";
                //    sWoDataId = ObjCon.get_value(sQry);
                //    return sWoDataId;
                //}
                //else if (sWOSLno != null && sWOSLno != "")
                //{
                //    sQry = "SELECT MAX(\"WO_WFO_ID\") FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_RECORD_ID\"='" + Convert.ToInt32(sWOSLno) + "' AND \"WO_BO_ID\"='11'";
                //    sWoDataId = ObjCon.get_value(sQry);
                //    return sWoDataId;
                //}
                #endregion 

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_max_wodataid_for_clsworkorder");
                cmd.Parameters.AddWithValue("p_failureid", Convert.ToString(sFailureId ?? ""));
                cmd.Parameters.AddWithValue("p_woslno", Convert.ToString(sWOSLno ?? ""));
                sWoDataId = ObjBasCon.StringGetValue(cmd);

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return sWoDataId;
            }
            return sWoDataId;
        }
        /// <summary>
        ///  to get office Name
        /// </summary>
        /// <param name="offcode"></param>
        /// <returns></returns>
        public string getofficeName(string offcode)
        {
            try
            {
                #region Old inline query 
                //string sQry = string.Empty;
                //sQry = "SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT)=SUBSTR(CAST('" + offcode + "' AS TEXT),1,'" + Constants.Division + "')";
                //return ObjCon.get_value(sQry);
                #endregion

                string QryKey = string.Empty;
                QryKey = "GET_OFFICENAME";
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsworkorder");
                cmd.Parameters.AddWithValue("p_key", QryKey);
                cmd.Parameters.AddWithValue("p_value_1", Convert.ToString(offcode ?? ""));
                cmd.Parameters.AddWithValue("p_value_2", "");
                return ObjBasCon.StringGetValue(cmd);

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }
        /// <summary>
        ///  to ge star rating
        /// </summary>
        /// <param name="rating"></param>
        /// <returns></returns>
        public string GetRating(string rating)
        {
            string Rating = string.Empty;
            string QryKey = string.Empty;
            try
            {
                #region Old Inline query GetRating
                //string strQry = string.Empty;               
                //strQry += " SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='SR' and \"MD_ID\"=" + rating + " ORDER BY \"MD_ORDER_BY\"";
                //Rating = ObjCon.get_value(strQry);
                #endregion

                QryKey = "GET_RATING";
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsworkorder");
                cmd.Parameters.AddWithValue("p_key", QryKey);
                cmd.Parameters.AddWithValue("p_value_1", Convert.ToString(rating ?? ""));
                cmd.Parameters.AddWithValue("p_value_2", "");
                Rating = ObjBasCon.StringGetValue(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
            return Rating;
        }
        /// <summary>
        /// to  get sub division name
        /// </summary>
        /// <param name="df_id"></param>
        /// <returns></returns>
        public string getsubdivName(string df_id)
        {
            string QryKey = string.Empty;
            try
            {
                #region Old Inline query getsubdivName
                //string sQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //sQry = "SELECT \"SD_SUBDIV_NAME\" FROM \"TBLDTCFAILURE\",\"TBLSUBDIVMAST\" WHERE ";
                //sQry += " CAST(\"SD_SUBDIV_CODE\" AS TEXT) = SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "') AND \"DF_ID\" =:df_id";
                //NpgsqlCommand.Parameters.AddWithValue("df_id", Convert.ToInt32(df_id));
                //return ObjCon.get_value(sQry, NpgsqlCommand);
                #endregion

                QryKey = "GET_SD_SUBDIV_NAME";
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsworkorder");
                cmd.Parameters.AddWithValue("p_key", QryKey);
                cmd.Parameters.AddWithValue("p_value_1", Convert.ToString(df_id ?? ""));
                cmd.Parameters.AddWithValue("p_value_2", "");
                return ObjBasCon.StringGetValue(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }
        /// <summary>
        ///  to get office name
        /// </summary>
        /// <param name="df_id"></param>
        /// <returns></returns>
        public DataTable GetofficeName(string df_id)
        {
            DataTable dt = new DataTable();
            try
            {
                #region Old Inline query GetofficeName
                //string sQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //sQry = "SELECT (SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(CAST(\"DF_LOC_CODE\" as TEXT), 1,3) )DIV,(SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(CAST(\"DF_LOC_CODE\" as TEXT), 1,4) )SUBDIV FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\"=:df_id";
                //NpgsqlCommand.Parameters.AddWithValue("df_id", Convert.ToInt32(df_id));
                //dt = ObjCon.FetchDataTable(sQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_officenam_for_workorder");
                cmd.Parameters.AddWithValue("p_df_id", df_id);
                dt = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
            return dt;
        }
        /// <summary>
        ///  to get office name by section code
        /// </summary>
        /// <param name="sOM_Code"></param>
        /// <returns></returns>
        public DataTable GetofficeNameBySectionCode(string sOM_Code)
        {
            DataTable dt = new DataTable();
            try
            {
                #region Old Query GetofficeNameBySectionCode
                //string sQry = string.Empty;
                //sQry = "SELECT (SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(CAST('" + sOM_Code + "' as TEXT), 1,3) )DIV,(SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(CAST('" + sOM_Code + "' as TEXT), 1,4) )SUBDIV FROM \"TBLOMSECMAST\" WHERE \"OM_CODE\"='" + sOM_Code + "'";
                //dt = ObjCon.FetchDataTable(sQry);
                #endregion 

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_officename_by_sectioncode_for_workorder");
                cmd.Parameters.AddWithValue("p_om_code", sOM_Code);
                dt = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
            return dt;
        }
        #region before pgrs Add code changed by rudra 
        //public DataTable FailDetails(string sFailId, string sFailType,string sCoilType,string ActionType,string sGuarenteeType,string sWoRecordId="")
        //{
        //    DataTable dtWODetails = new DataTable();
        //    string sQry = string.Empty;
        //    try
        //    {
        //        if(ActionType == "V")
        //        {
        //            if(sWoRecordId.Contains('-'))
        //            {
        //                if (sFailType != "2")
        //                {
        //                    NpgsqlCommand = new NpgsqlCommand();
        //                    sQry = "SELECT \"DF_ID\",\"DF_DTC_CODE\",\"DF_EQUIPMENT_ID\",to_char(\"DF_DATE\",'dd-mm-yyyy')DF_DATE,to_char(\"DF_DATE\",'dd-MON-yyyy')DF_MONTH_DATE";
        //                    sQry += " ,\"TR_ID\",\"EST_CAPACITY\",\"EST_FAIL_TYPE\",\"TR_NAME\",UPPER(\"DT_NAME\")DT_NAME,SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,4)DF_LOC_CODE FROM \"TBLDTCFAILURE\" INNER JOIN \"TBLESTIMATIONDETAILS\" ON \"DF_ID\"";
        //                    sQry += " =\"EST_FAILUREID\" INNER JOIN \"TBLTRANSREPAIRER\" ON \"EST_REPAIRER\" = \"TR_ID\" INNER JOIN \"TBLDTCMAST\" ON \"DF_DTC_CODE\"=\"DT_CODE\" WHERE \"EST_ID\"=:sFailId";
        //                    NpgsqlCommand.Parameters.AddWithValue("sFailId", Convert.ToInt32(sFailId));
        //                }
        //                else
        //                {
        //                    NpgsqlCommand = new NpgsqlCommand();
        //                    sQry = "SELECT \"DF_ID\",\"DF_DTC_CODE\",\"DF_EQUIPMENT_ID\",to_char(\"DF_DATE\",'dd-mm-yyyy')DF_DATE,to_char(\"DF_DATE\",'dd-MON-yyyy')DF_MONTH_DATE";
        //                    sQry += ",\"EST_CAPACITY\",\"EST_FAIL_TYPE\",UPPER(\"DT_NAME\")DT_NAME,SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,4)DF_LOC_CODE FROM \"TBLDTCFAILURE\" INNER JOIN \"TBLESTIMATIONDETAILS\" ON \"DF_ID\"";
        //                    sQry += " =\"EST_FAILUREID\" INNER JOIN \"TBLDTCMAST\" ON \"DF_DTC_CODE\"=\"DT_CODE\" WHERE \"EST_ID\"=:sFailId1";
        //                    NpgsqlCommand.Parameters.AddWithValue("sFailId1", Convert.ToInt32(sFailId));
        //                }
        //            }
        //            else
        //            {
        //                if (sFailType != "2" && sCoilType!="2")
        //                {
        //                    if(sFailType=="1")
        //                    { 
        //                    NpgsqlCommand = new NpgsqlCommand();
        //                    sQry = "SELECT \"DF_ID\",\"DF_DTC_CODE\",\"DF_EQUIPMENT_ID\",to_char(\"DF_DATE\",'dd-mm-yyyy')DF_DATE,to_char(\"DF_DATE\",'dd-MON-yyyy')DF_MONTH_DATE";
        //                    sQry += ",\"EST_CAPACITY\",\"EST_FAIL_TYPE\",UPPER(\"DT_NAME\")DT_NAME,SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,4)DF_LOC_CODE FROM \"TBLDTCFAILURE\" INNER JOIN \"TBLESTIMATIONDETAILS\" ON \"DF_ID\"";
        //                    sQry += " =\"EST_FAILUREID\" INNER JOIN \"TBLDTCMAST\" ON \"DF_DTC_CODE\"=\"DT_CODE\" WHERE \"DF_ID\"=:sFailId";
        //                    NpgsqlCommand.Parameters.AddWithValue("sFailId", Convert.ToInt32(sFailId));
        //                    }
        //                    else
        //                    {
        //                        NpgsqlCommand = new NpgsqlCommand();
        //                    sQry = "SELECT \"DF_ID\",\"DF_DTC_CODE\",\"DF_EQUIPMENT_ID\",to_char(\"DF_DATE\",'dd-mm-yyyy')DF_DATE,to_char(\"DF_DATE\",'dd-MON-yyyy')DF_MONTH_DATE";
        //                    sQry += " ,\"TR_ID\",\"EST_CAPACITY\",\"EST_FAIL_TYPE\",\"TR_NAME\",UPPER(\"DT_NAME\")DT_NAME,SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,4)DF_LOC_CODE FROM \"TBLDTCFAILURE\" INNER JOIN \"TBLESTIMATIONDETAILS\" ON \"DF_ID\"";
        //                    sQry += " =\"EST_FAILUREID\" INNER JOIN \"TBLTRANSREPAIRER\" ON \"EST_REPAIRER\" = \"TR_ID\" INNER JOIN \"TBLDTCMAST\" ON \"DF_DTC_CODE\"=\"DT_CODE\" WHERE \"EST_ID\"=:sFailId";
        //                    NpgsqlCommand.Parameters.AddWithValue("sFailId", Convert.ToInt32(sFailId));
        //                    }

        //                    //NpgsqlCommand = new NpgsqlCommand();
        //                    //sQry = "SELECT \"DF_ID\",\"DF_DTC_CODE\",\"DF_EQUIPMENT_ID\",to_char(\"DF_DATE\",'dd-mm-yyyy')DF_DATE,to_char(\"DF_DATE\",'dd-MON-yyyy')DF_MONTH_DATE";
        //                    //sQry += " ,\"TR_ID\",\"EST_CAPACITY\",\"EST_FAIL_TYPE\",\"TR_NAME\",UPPER(\"DT_NAME\")DT_NAME,SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,4)DF_LOC_CODE FROM \"TBLDTCFAILURE\" INNER JOIN \"TBLESTIMATIONDETAILS\" ON \"DF_ID\"";
        //                    //sQry += " =\"EST_FAILUREID\" INNER JOIN \"TBLWORKORDER\" ON \"DF_ID\"=\"WO_DF_ID\" INNER JOIN \"TBLTRANSREPAIRER\" ON \"EST_REPAIRER\" = \"TR_ID\" INNER JOIN \"TBLDTCMAST\" ON \"DF_DTC_CODE\"=\"DT_CODE\" WHERE \"WO_SLNO\"=:sFailId2";
        //                    //NpgsqlCommand.Parameters.AddWithValue("sFailId2", Convert.ToInt32(sFailId));
        //                }
        //                else
        //                {
        //                    NpgsqlCommand = new NpgsqlCommand();
        //                    sQry = "SELECT \"DF_ID\",\"DF_DTC_CODE\",\"DF_EQUIPMENT_ID\",to_char(\"DF_DATE\",'dd-mm-yyyy')DF_DATE,to_char(\"DF_DATE\",'dd-MON-yyyy')DF_MONTH_DATE";
        //                    sQry += ",\"EST_CAPACITY\",\"EST_FAIL_TYPE\",UPPER(\"DT_NAME\")DT_NAME,SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,4)DF_LOC_CODE FROM \"TBLDTCFAILURE\" INNER JOIN \"TBLESTIMATIONDETAILS\" ON \"DF_ID\"";
        //                    sQry += " =\"EST_FAILUREID\"  INNER JOIN \"TBLWORKORDER\" ON \"DF_ID\"=\"WO_DF_ID\" INNER JOIN \"TBLDTCMAST\" ON \"DF_DTC_CODE\"=\"DT_CODE\" WHERE \"WO_SLNO\"=:sFailId3";
        //                    NpgsqlCommand.Parameters.AddWithValue("sFailId3", Convert.ToInt32(sFailId));
        //                }
        //            }

        //        }
        //        else
        //        {
        //            if (sFailType != "2" && sCoilType != "2")
        //            {
        //                NpgsqlCommand = new NpgsqlCommand();
        //                sQry = "SELECT \"DF_ID\",\"DF_DTC_CODE\",\"DF_EQUIPMENT_ID\",to_char(\"DF_DATE\",'dd-mm-yyyy')DF_DATE,to_char(\"DF_DATE\",'dd-MON-yyyy')DF_MONTH_DATE";
        //                sQry += " ,\"TR_ID\",\"EST_CAPACITY\",\"EST_FAIL_TYPE\",\"TR_NAME\",UPPER(\"DT_NAME\")DT_NAME,SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,4)DF_LOC_CODE FROM \"TBLDTCFAILURE\" INNER JOIN \"TBLESTIMATIONDETAILS\" ON \"DF_ID\"";
        //                sQry += " =\"EST_FAILUREID\" INNER JOIN \"TBLTRANSREPAIRER\" ON \"EST_REPAIRER\" = \"TR_ID\" INNER JOIN \"TBLDTCMAST\" ON \"DF_DTC_CODE\"=\"DT_CODE\" WHERE \"EST_ID\"=:sFailId4";
        //                NpgsqlCommand.Parameters.AddWithValue("sFailId4", Convert.ToInt32(sFailId));
        //            }
        //            else
        //            {
        //                NpgsqlCommand = new NpgsqlCommand();
        //                sQry = "SELECT \"DF_ID\",\"DF_DTC_CODE\",\"DF_EQUIPMENT_ID\",to_char(\"DF_DATE\",'dd-mm-yyyy')DF_DATE,to_char(\"DF_DATE\",'dd-MON-yyyy')DF_MONTH_DATE";
        //                sQry += ",\"EST_CAPACITY\",\"EST_FAIL_TYPE\",UPPER(\"DT_NAME\")DT_NAME,SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,4)DF_LOC_CODE FROM \"TBLDTCFAILURE\" INNER JOIN \"TBLESTIMATIONDETAILS\" ON \"DF_ID\"";
        //                sQry += " =\"EST_FAILUREID\" INNER JOIN \"TBLDTCMAST\" ON \"DF_DTC_CODE\"=\"DT_CODE\" WHERE \"EST_ID\"=:sFailId5";
        //                NpgsqlCommand.Parameters.AddWithValue("sFailId5", Convert.ToInt32(sFailId));
        //            }
        //        }


        //        dtWODetails = ObjCon.FetchDataTable(sQry, NpgsqlCommand);
        //        return dtWODetails;
        //    }
        //    catch(Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return dtWODetails;
        //    }
        //}
        #endregion
        // changed by rudra on 09-06-2020 for display pgrs  Docket number in work order
        public DataTable FailDetails(string sFailId, string sFailType, string sCoilType, string ActionType, string sGuarenteeType, string sWoRecordId = "")
        {
            DataTable dtWODetails = new DataTable();
            try
            {
                #region Old FailDetails 
                //string sQry = string.Empty;
                //if (ActionType == "V")
                //{
                //    if (sWoRecordId.Contains('-'))
                //    {
                //        if (sFailType != "2")
                //        {
                //            NpgsqlCommand = new NpgsqlCommand();
                //            sQry = "SELECT \"DF_ID\",\"DF_DTC_CODE\",\"DF_EQUIPMENT_ID\",\"DF_OIL_QUANTITY_TANK\", \"DF_CONSUMER_COMPLAINT_NUM\" as \"DF_PGRS_DOCKET\",to_char(\"DF_DATE\",'dd-mm-yyyy')DF_DATE,to_char(\"DF_DATE\",'dd-MON-yyyy')DF_MONTH_DATE";
                //            sQry += " ,\"TR_ID\",\"EST_CAPACITY\",\"EST_FAIL_TYPE\",\"TR_NAME\",UPPER(\"DT_NAME\")DT_NAME,SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,4)DF_LOC_CODE FROM \"TBLDTCFAILURE\" INNER JOIN \"TBLESTIMATIONDETAILS\" ON \"DF_ID\"";
                //            sQry += " =\"EST_FAILUREID\" INNER JOIN \"TBLTRANSREPAIRER\" ON \"EST_REPAIRER\" = \"TR_ID\" INNER JOIN \"TBLDTCMAST\" ON \"DF_DTC_CODE\"=\"DT_CODE\" WHERE \"EST_ID\"=:sFailId";
                //            NpgsqlCommand.Parameters.AddWithValue("sFailId", Convert.ToInt32(sFailId));
                //        }
                //        else
                //        {
                //            NpgsqlCommand = new NpgsqlCommand();
                //            sQry = "SELECT \"DF_ID\",\"DF_DTC_CODE\",\"DF_EQUIPMENT_ID\",\"DF_OIL_QUANTITY_TANK\", \"DF_CONSUMER_COMPLAINT_NUM\" as \"DF_PGRS_DOCKET\",to_char(\"DF_DATE\",'dd-mm-yyyy')DF_DATE,to_char(\"DF_DATE\",'dd-MON-yyyy')DF_MONTH_DATE";
                //            sQry += ",\"EST_CAPACITY\",\"EST_FAIL_TYPE\",UPPER(\"DT_NAME\")DT_NAME,SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,4)DF_LOC_CODE FROM \"TBLDTCFAILURE\" INNER JOIN \"TBLESTIMATIONDETAILS\" ON \"DF_ID\"";
                //            sQry += " =\"EST_FAILUREID\" INNER JOIN \"TBLDTCMAST\" ON \"DF_DTC_CODE\"=\"DT_CODE\" WHERE \"EST_ID\"=:sFailId1";
                //            NpgsqlCommand.Parameters.AddWithValue("sFailId1", Convert.ToInt32(sFailId));
                //        }
                //    }
                //    else
                //    {
                //        if (sFailType != "2" && sCoilType != "2")
                //        {
                //            if (sFailType == "1")
                //            {
                //                NpgsqlCommand = new NpgsqlCommand();
                //                sQry = "SELECT \"DF_ID\",\"DF_DTC_CODE\",\"DF_EQUIPMENT_ID\",\"DF_OIL_QUANTITY_TANK\", \"DF_CONSUMER_COMPLAINT_NUM\" as \"DF_PGRS_DOCKET\",to_char(\"DF_DATE\",'dd-mm-yyyy')DF_DATE,to_char(\"DF_DATE\",'dd-MON-yyyy')DF_MONTH_DATE";
                //                sQry += ",\"EST_CAPACITY\",\"EST_FAIL_TYPE\",UPPER(\"DT_NAME\")DT_NAME,SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,4)DF_LOC_CODE FROM \"TBLDTCFAILURE\" INNER JOIN \"TBLESTIMATIONDETAILS\" ON \"DF_ID\"";
                //                sQry += " =\"EST_FAILUREID\" INNER JOIN \"TBLDTCMAST\" ON \"DF_DTC_CODE\"=\"DT_CODE\" WHERE \"DF_ID\"=:sFailId";
                //                NpgsqlCommand.Parameters.AddWithValue("sFailId", Convert.ToInt32(sFailId));
                //            }
                //            else
                //            {
                //                NpgsqlCommand = new NpgsqlCommand();
                //                sQry = "SELECT \"DF_ID\",\"DF_DTC_CODE\",\"DF_EQUIPMENT_ID\",\"DF_OIL_QUANTITY_TANK\", \"DF_CONSUMER_COMPLAINT_NUM\" as \"DF_PGRS_DOCKET\",to_char(\"DF_DATE\",'dd-mm-yyyy')DF_DATE,to_char(\"DF_DATE\",'dd-MON-yyyy')DF_MONTH_DATE";
                //                sQry += " ,\"TR_ID\",\"EST_CAPACITY\",\"EST_FAIL_TYPE\",\"TR_NAME\",UPPER(\"DT_NAME\")DT_NAME,SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,4)DF_LOC_CODE FROM \"TBLDTCFAILURE\" INNER JOIN \"TBLESTIMATIONDETAILS\" ON \"DF_ID\"";
                //                sQry += " =\"EST_FAILUREID\" INNER JOIN \"TBLTRANSREPAIRER\" ON \"EST_REPAIRER\" = \"TR_ID\" INNER JOIN \"TBLDTCMAST\" ON \"DF_DTC_CODE\"=\"DT_CODE\" WHERE \"EST_ID\"=:sFailId";
                //                NpgsqlCommand.Parameters.AddWithValue("sFailId", Convert.ToInt32(sFailId));
                //            }
                //        }
                //        else
                //        {
                //            NpgsqlCommand = new NpgsqlCommand();
                //            sQry = "SELECT \"DF_ID\",\"DF_DTC_CODE\",\"DF_EQUIPMENT_ID\",\"DF_OIL_QUANTITY_TANK\", \"DF_CONSUMER_COMPLAINT_NUM\" as \"DF_PGRS_DOCKET\",to_char(\"DF_DATE\",'dd-mm-yyyy')DF_DATE,to_char(\"DF_DATE\",'dd-MON-yyyy')DF_MONTH_DATE";
                //            sQry += ",\"EST_CAPACITY\",\"EST_FAIL_TYPE\",UPPER(\"DT_NAME\")DT_NAME,SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,4)DF_LOC_CODE FROM \"TBLDTCFAILURE\" INNER JOIN \"TBLESTIMATIONDETAILS\" ON \"DF_ID\"";
                //            sQry += " =\"EST_FAILUREID\"  INNER JOIN \"TBLWORKORDER\" ON \"DF_ID\"=\"WO_DF_ID\" INNER JOIN \"TBLDTCMAST\" ON \"DF_DTC_CODE\"=\"DT_CODE\" WHERE \"WO_SLNO\"=:sFailId3";
                //            NpgsqlCommand.Parameters.AddWithValue("sFailId3", Convert.ToInt32(sFailId));
                //        }
                //    }

                //}
                //else
                //{
                //    if (sFailType != "2" && sCoilType != "2")
                //    {
                //        NpgsqlCommand = new NpgsqlCommand();
                //        sQry = "SELECT \"DF_ID\",\"DF_DTC_CODE\",\"DF_EQUIPMENT_ID\",\"DF_OIL_QUANTITY_TANK\",\"DF_CONSUMER_COMPLAINT_NUM\" as \"DF_PGRS_DOCKET\",to_char(\"DF_DATE\",'dd-mm-yyyy')DF_DATE,to_char(\"DF_DATE\",'dd-MON-yyyy')DF_MONTH_DATE";
                //        sQry += " ,\"TR_ID\",\"EST_CAPACITY\",\"EST_FAIL_TYPE\",\"TR_NAME\",UPPER(\"DT_NAME\")DT_NAME,SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,4)DF_LOC_CODE FROM \"TBLDTCFAILURE\" INNER JOIN \"TBLESTIMATIONDETAILS\" ON \"DF_ID\"";
                //        sQry += " =\"EST_FAILUREID\" INNER JOIN \"TBLTRANSREPAIRER\" ON \"EST_REPAIRER\" = \"TR_ID\" INNER JOIN \"TBLDTCMAST\" ON \"DF_DTC_CODE\"=\"DT_CODE\" WHERE \"EST_ID\"=:sFailId4";
                //        NpgsqlCommand.Parameters.AddWithValue("sFailId4", Convert.ToInt32(sFailId));
                //    }
                //    else
                //    {
                //        NpgsqlCommand = new NpgsqlCommand();
                //        sQry = "SELECT \"DF_ID\",\"DF_DTC_CODE\",\"DF_EQUIPMENT_ID\",\"DF_OIL_QUANTITY_TANK\",\"DF_CONSUMER_COMPLAINT_NUM\" as \"DF_PGRS_DOCKET\",to_char(\"DF_DATE\",'dd-mm-yyyy')DF_DATE,to_char(\"DF_DATE\",'dd-MON-yyyy')DF_MONTH_DATE";
                //        sQry += ",\"EST_CAPACITY\",\"EST_FAIL_TYPE\",UPPER(\"DT_NAME\")DT_NAME,SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,4)DF_LOC_CODE FROM \"TBLDTCFAILURE\" INNER JOIN \"TBLESTIMATIONDETAILS\" ON \"DF_ID\"";
                //        sQry += " =\"EST_FAILUREID\" INNER JOIN \"TBLDTCMAST\" ON \"DF_DTC_CODE\"=\"DT_CODE\" WHERE \"EST_ID\"=:sFailId5";
                //        NpgsqlCommand.Parameters.AddWithValue("sFailId5", Convert.ToInt32(sFailId));
                //    }
                //}
                //dtWODetails = ObjCon.FetchDataTable(sQry, NpgsqlCommand);
                #endregion                

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_fail_details_for_workorder");
                cmd.Parameters.AddWithValue("p_actiontype", (ActionType ?? ""));
                cmd.Parameters.AddWithValue("p_worecordid", (sWoRecordId ?? ""));
                cmd.Parameters.AddWithValue("p_failtype", (sFailType ?? ""));
                cmd.Parameters.AddWithValue("p_coiltype", (sCoilType ?? ""));
                cmd.Parameters.AddWithValue("p_failid", (sFailId ?? ""));
                dtWODetails = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtWODetails;
            }
            return dtWODetails;
        }
        /// <summary>
        ///  to get failure id 
        /// </summary>
        /// <param name="sWo_Slno"></param>
        /// <param name="sWoRecordID"></param>
        /// <returns></returns>
        public string FailureId(string sWo_Slno, string sWoRecordID = "")
        {
            string QryKey = string.Empty;
            string sQry = string.Empty;
            try
            {
                #region Old Code when work order at only one level
                //sQry = "SELECT \"EST_ID\" FROM \"TBLWORKORDER\",\"TBLESTIMATIONDETAILS\",\"TBLDTCFAILURE\" WHERE \"DF_ID\"=\"EST_FAILUREID\" AND ";
                //sQry += " \"DF_ID\"=\"WO_DF_ID\" AND \"WO_SLNO\" ='" + sWo_Slno + "'";
                #endregion


                if (sWo_Slno.Contains('-'))
                {
                    #region Old inline query 
                    //clsFormValues objForm = new clsFormValues();
                    //NpgsqlCommand = new NpgsqlCommand();
                    //sQry = "SELECT \"WO_RECORD_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\" in (SELECT \"WOA_PREV_APPROVE_ID\" FROM ";
                    //sQry += " \"TBLWORKFLOWOBJECTS\",\"TBLWO_OBJECT_AUTO\" WHERE \"WO_INITIAL_ID\"=\"WOA_INITIAL_ACTION_ID\" and ";
                    //sQry += " \"WO_BO_ID\" ='11' and \"WO_RECORD_ID\"=:sWo_Slno and \"WOA_BFM_ID\"='3')";
                    //NpgsqlCommand.Parameters.AddWithValue("sWo_Slno", Convert.ToInt32(sWo_Slno));
                    #endregion

                    QryKey = "GET_WO_RECORD_ID";
                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsworkorder");
                    cmd.Parameters.AddWithValue("p_key", QryKey);
                    cmd.Parameters.AddWithValue("p_value_1", Convert.ToString(sWo_Slno ?? ""));
                    cmd.Parameters.AddWithValue("p_value_2", "");
                    return ObjBasCon.StringGetValue(cmd);

                }
                else
                {
                    NpgsqlCommand = new NpgsqlCommand();
                    if (sWoRecordID.Contains('-'))
                    {
                        return sWo_Slno;
                    }
                    else
                    {
                        #region old inline query
                        //sQry = "SELECT \"EST_ID\" FROM \"TBLWORKORDER\",\"TBLESTIMATIONDETAILS\",\"TBLDTCFAILURE\" WHERE \"DF_ID\"=\"EST_FAILUREID\" AND ";
                        //sQry += " \"DF_ID\"=\"WO_DF_ID\" AND \"WO_SLNO\" =:sWo_Slno1";
                        //NpgsqlCommand.Parameters.AddWithValue("sWo_Slno1", Convert.ToInt32(sWo_Slno));
                        #endregion

                        QryKey = "GET_EST_ID";
                        NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsworkorder");
                        cmd.Parameters.AddWithValue("p_key", QryKey);
                        cmd.Parameters.AddWithValue("p_value_1", Convert.ToString(sWo_Slno ?? ""));
                        cmd.Parameters.AddWithValue("p_value_2", "");
                        return ObjBasCon.StringGetValue(cmd);
                    }

                }

                //return ObjCon.get_value(sQry, NpgsqlCommand);      
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }
        /// <summary>
        ///  to get fail coil type
        /// </summary>
        /// <param name="sWo_Slno"></param>
        /// <param name="sActionType"></param>
        /// <returns></returns>
        public string GetFailCoilType(string sWo_Slno, string sActionType)
        {

            try
            {
                #region Old inline query GetFailCoilType
                //string sQry = string.Empty;
                //if (sActionType == "V")
                //{
                //    NpgsqlCommand = new NpgsqlCommand();
                //    sQry = "SELECT cast(\"EST_FAIL_TYPE\" as text)|| '~' ||cast(\"WO_SLNO\"as text) FROM \"TBLESTIMATIONDETAILS\",\"TBLWORKORDER\",\"TBLDTCFAILURE\" WHERE \"DF_ID\"=\"WO_DF_ID\" AND \"DF_ID\"=\"EST_FAILUREID\" AND \"WO_DF_ID\" =:sWo_Slno";
                //    NpgsqlCommand.Parameters.AddWithValue("sWo_Slno", Convert.ToInt32(sWo_Slno));
                //}
                //else
                //{
                //    NpgsqlCommand = new NpgsqlCommand();
                //    sQry = "SELECT cast(\"EST_FAIL_TYPE\" as text)|| '~' ||cast(\"WO_SLNO\"as text) FROM \"TBLESTIMATIONDETAILS\",\"TBLWORKORDER\",\"TBLDTCFAILURE\" WHERE \"DF_ID\"=\"WO_DF_ID\" AND \"DF_ID\"=\"EST_FAILUREID\" AND \"WO_DF_ID\" =:sWo_Slno1";
                //    NpgsqlCommand.Parameters.AddWithValue("sWo_Slno1", Convert.ToInt32(sWo_Slno));
                //}
                //return ObjCon.get_value(sQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_failcoil_type_for_workorder");
                cmd.Parameters.AddWithValue("p_actiontype", Convert.ToString(sActionType ?? ""));
                cmd.Parameters.AddWithValue("p_wo_slno", Convert.ToString(sWo_Slno ?? ""));
                return ObjBasCon.StringGetValue(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }
        /// <summary>
        ///  to get unit or empty
        /// </summary>
        /// <param name="text"></param>
        /// <param name="stopAt"></param>
        /// <returns></returns>
        public string GetUntilOrEmpty(string text, string stopAt = "~")
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(text))
                {
                    int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

                    if (charLocation > 0)
                    {
                        return text.Substring(0, charLocation);
                    }
                }
                return String.Empty;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }
        /// <summary>
        ///  to get estimation date
        /// </summary>
        /// <param name="failid"></param>
        /// <returns></returns>
        public string getestimatedate(string failid)
        {
            string QryKey = string.Empty;
            try
            {
                #region OLD Inlinw query getestimatedate
                //string sQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //sQry = " SELECT TO_CHAR(\"EST_DATE\",'dd/mm/yyyy') from \"TBLESTIMATIONDETAILS\" WHERE \"EST_FAILUREID\"=:failid ";
                //NpgsqlCommand.Parameters.AddWithValue("failid", Convert.ToInt32(failid));
                //return ObjCon.get_value(sQry, NpgsqlCommand);
                #endregion

                QryKey = "GET_ESTIMATEDATE";
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsworkorder");
                cmd.Parameters.AddWithValue("p_key", QryKey);
                cmd.Parameters.AddWithValue("p_value_1", Convert.ToString(failid ?? ""));
                cmd.Parameters.AddWithValue("p_value_2", "");
                return ObjBasCon.StringGetValue(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }
        /// <summary>
        ///  to et estimation amout
        /// </summary>
        /// <param name="failid"></param>
        /// <returns></returns>
        public DataTable getestimateAmount(string failid)
        {
            DataTable dt = new DataTable();
            try
            {
                #region Old inline query getestimateAmount
                //string sQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //sQry = " select cast(round(\"EST_TOTAL\",2)as text) EST_TOTAL,cast(round(\"EST_DECOM_TOTAL\",2)as text)  EST_DECOM_TOTAL from \"TBLESTIMATION\" where \"EST_ESTD_ID\"=:failid ";
                //NpgsqlCommand.Parameters.AddWithValue("failid", Convert.ToInt32(failid));
                //dt = ObjCon.FetchDataTable(sQry, NpgsqlCommand);
                #endregion 

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_estimateamount_for_workorder");
                //cmd.Parameters.AddWithValue("p_actiontype", Convert.ToString(failid ?? ""));
                cmd.Parameters.AddWithValue("p_failid", Convert.ToString(failid ?? ""));
                dt = ObjCon.FetchDataTable(cmd);

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
            return dt;
        }
        /// <summary>
        ///  to get failed ID
        /// </summary>
        /// <param name="sFailureId"></param>
        /// <returns></returns>
        public string getfailid(string sFailureId)
        {
            string QryKey = string.Empty;
            string sWoDataId = string.Empty;
            try
            {
                #region Old Inline query getfailid
                //string sQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //sQry = "SELECT DISTINCT \"WO_DATA_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_RECORD_ID\"=:sFailureId AND \"WO_BO_ID\"='11'";
                //NpgsqlCommand.Parameters.AddWithValue("sFailureId", Convert.ToInt32(sFailureId));
                //sWoDataId = ObjCon.get_value(sQry, NpgsqlCommand);
                #endregion

                QryKey = "GET_FAILID";
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsworkorder");
                cmd.Parameters.AddWithValue("p_key", QryKey);
                cmd.Parameters.AddWithValue("p_value_1", Convert.ToString(sFailureId ?? ""));
                cmd.Parameters.AddWithValue("p_value_2", "");
                sWoDataId = ObjBasCon.StringGetValue(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return sWoDataId;
            }

            return sWoDataId;
        }
        /// <summary>
        /// to get failure id
        /// </summary>
        /// <param name="swoSlno"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string getFailureId(string swoSlno, string type)
        {
            string QryKey = string.Empty;
            try
            {
                #region Old inline query getFailureId
                //string sQry = string.Empty;
                //if (type == "1")
                //{
                //    NpgsqlCommand = new NpgsqlCommand();
                //    sQry = "SELECT \"DF_ID\" FROM \"TBLDTCFAILURE\",\"TBLWORKORDER\" WHERE \"DF_ID\"=\"WO_DF_ID\" and \"WO_SLNO\"=:swoSlno";
                //    NpgsqlCommand.Parameters.AddWithValue("swoSlno", Convert.ToInt32(swoSlno));
                //}
                //else
                //{
                //    NpgsqlCommand = new NpgsqlCommand();
                //    sQry = "SELECT DISTINCT \"WO_DATA_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\"='11' and \"WO_RECORD_ID\"=:swoSlno1";
                //    NpgsqlCommand.Parameters.AddWithValue("swoSlno1", Convert.ToInt32(swoSlno));
                //}
                //return ObjCon.get_value(sQry, NpgsqlCommand);
                #endregion

                QryKey = "GET_FAILUREID_FOR_CLSWORKORDER";
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsworkorder");
                cmd.Parameters.AddWithValue("p_key", QryKey);
                cmd.Parameters.AddWithValue("p_value_1", Convert.ToString(swoSlno ?? ""));
                cmd.Parameters.AddWithValue("p_value_2", Convert.ToString(type ?? ""));
                return ObjBasCon.StringGetValue(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return string.Empty;
            }

        }
    }
}
