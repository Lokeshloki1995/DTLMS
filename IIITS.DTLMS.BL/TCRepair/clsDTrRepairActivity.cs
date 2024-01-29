using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;
using System.Data.OleDb;
using System.IO;
using System.Configuration;

namespace IIITS.DTLMS.BL
{

    public class clsDTrRepairActivity
    {
        DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
        string strFormCode = "clsDTrRepairActivity";
        PGSqlConnection objCon = new PGSqlConnection(Constants.Password);

        //Tc Details
        public string newdivcode { get; set; }
        public string sTcCode { get; set; }
        public string sTcSlno { get; set; }
        public string sMakeName { get; set; }
        public string sManfDate { get; set; }
        public string sCapacity { get; set; }
        public string sstarrate { get; set; }
        public string sRSMID { get; set; }
        public string sActionType { get; set; }
        public string sWarrantyPeriod { get; set; }
        public string sSupplierName { get; set; }
        public string sGuarantyType { get; set; }
        public string sMakeId { get; set; }
        public string sStoreId { get; set; }
        public string sSupplierId { get; set; }
        public string sRepairerId { get; set; }
        public string sTcId { get; set; }
        public string sRefString { get; set; }

        //To send to Repairer/Supplier
        public string sSupRepId { get; set; }
        public string sIssueDate { get; set; }
        public string sPurchaseOrderNo { get; set; }
        public string sPurchaseDate { get; set; }
        public string sInvoiceNo { get; set; }
        public string sInvoiceDate { get; set; }
        public string sIndenteNo { get; set; }
        public string sIndentDate { get; set; }
        public string sRINo { get; set; }
        public string sRIDate { get; set; }
        public string sManualInvoiceNo { get; set; }
        public string sCrby { get; set; }
        public string SessionOfcCode { get; set; }

        public string sType { get; set; }

        public string sRepairDetailsId { get; set; }
        public string sRepairMasterId { get; set; }
        public string sQty { get; set; }

        //Testing Activity       
        public string sPass { get; set; }
        public string sFail { get; set; }

        public string sTestingDone { get; set; }
        public string sTestedBy { get; set; }
        public string sTestedOn { get; set; }
        public string sTestLocation { get; set; }
        public string sInspRemarks { get; set; }
        public string sTestResult { get; set; }
        public string sTestInspectionId { get; set; }

        public DataTable dtTestDone { get; set; }

        //Deliver or Recieve DTR
        public string sDeliverDate { get; set; }
        public string sDeliverChallenNo { get; set; }
        public string sVerifiedby { get; set; }
        public string sOfficeCode { get; set; }
        public string sRVNo { get; set; }
        public string sRVDate { get; set; }

        // Workflow
        public string sFormName { get; set; }
        public string sClientIP { get; set; }
        public string sWFDataId { get; set; }
        public string sWFObjectId { get; set; }

        //Document
        public string sFileName { get; set; }
        public string sStatus { get; set; }
        public string sOldPONo { get; set; }
        public string sPORemarks { get; set; }
        public string sroletype { get; set; }
        public string sroleid { get; set; }
        public string UserId { get; set; }

        public string sEstimationNo { get; set; }
        public string sWorkorderNo { get; set; }
        public string sEstimationAmount { get; set; }
        public string sEstimationDate { get; set; }
        public string sWorkorderDate { get; set; }
        public string swoslno { get; set; }
        public string sDivCode { get; set; }

        public string Remarks { get; set; }
        public string Reason { get; set; }
        public string OldGuaranteetype { get; set; }
        public string sCron { get; set; }
        #region Fault TC Search and Send to Repair
        NpgsqlCommand NpgsqlCommand;
        #region
        public DataTable LoadFaultTC(clsDTrRepairActivity objTcRepair)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                // below 2 lines is also act as list agg 
                //SELECT rtrim(xmlagg(XMLELEMENT(e,DF_DTC_CODE,',').EXTRACT('//text()')).GetClobVal(),',') very_long_text FROM TBLDTCFAILURE 
                //WHERE DF_REPLACE_FLAG='0'
                if (objTcRepair.sroletype != "2" && objTcRepair.sroleid == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]) || objTcRepair.sOfficeCode == "")
                {
                    objTcRepair.sStoreId = objCon.get_value(" SELECT \"STO_SM_ID\" from \"TBLSTOREOFFCODE\" WHERE cast(\"STO_OFF_CODE\" as text)=(SELECT substr(\"US_OFFICE_CODE\",1,3) from \"TBLUSER\" WHERE \"US_ID\"='" + objTcRepair.UserId + "')");
                }


                strQry = "SELECT* FROM (SELECT \"RESTD_ID\",\"TC_ID\", \"TC_CODE\", \"TC_SLNO\",\"TC_PREV_OFFCODE\", \"TM_NAME\", TO_CHAR(\"TC_MANF_DATE\", 'DD-MON-YYYY') \"TC_MANF_DATE\", ";
                strQry += "  CAST(\"TC_CAPACITY\" AS TEXT) \"TC_CAPACITY\", (SELECT COUNT(\"RSD_TC_CODE\") FROM \"TBLREPAIRSENTDETAILS\" WHERE \"RSD_TC_CODE\" =  \"TC_CODE\" AND \"RSD_DELIVARY_DATE\" IS NOT NULL) \"RCOUNT\",";
                strQry += " TO_CHAR(\"TC_PURCHASE_DATE\", 'DD-MON-YYYY') \"TC_PURCHASE_DATE\",  TO_CHAR(\"TC_WARANTY_PERIOD\", 'DD-MON-YYYY') \"TC_WARANTY_PERIOD\",  (SELECT \"TS_NAME\" FROM \"TBLTRANSSUPPLIER\" WHERE  \"TC_SUPPLIER_ID\" = \"TS_ID\")";
                strQry += " \"TS_NAME\", CASE WHEN \"TC_WARANTY_PERIOD\" IS NOT NULL THEN ((SELECT CASE WHEN NOW() <  \"TC_WARANTY_PERIOD\" THEN \"RSD_GUARRENTY_TYPE\" ELSE 'AGP'  END FROM  \"TBLREPAIRSENTDETAILS\" WHERE \"TC_CODE\" = \"RSD_TC_CODE\"  AND \"RSD_GUARRENTY_TYPE\" IS NOT NULL AND";
                strQry += "\"RSD_WARENTY_PERIOD\" IS NOT NULL  AND \"RSD_ID\" IN(SELECT FIRST_VALUE(\"RSD_ID\")  OVER(ORDER BY \"RSD_ID\" DESC) AS \"RSD_ID\" FROM \"TBLREPAIRSENTDETAILS\" WHERE \"RSD_TC_CODE\" = \"TC_CODE\")) )  ELSE(SELECT \"RESTD_GUARANTEETYPE\" FROM \"TBLREPAIRERESTIMATIONDETAILS\"  ";
                strQry += "WHERE \"RESTD_ID\" IN (SELECT  MAX(\"RESTD_ID\") FROM \"TBLREPAIRERESTIMATIONDETAILS\" WHERE  \"RESTD_TC_CODE\" = \"TC_CODE\")) END \"TC_GUARANTY_TYPE\",  (CASE WHEN CAST(\"TC_CODE\" AS TEXT) IN  (SELECT UNNEST(STRING_TO_ARRAY(STRING_AGG(\"WO_DATA_ID\", ','), ',')) FROM \"TBLWORKFLOWOBJECTS\",\"TBLWO_OBJECT_AUTO\"";
                strQry += "WHERE \"WO_ID\"=\"WOA_PREV_APPROVE_ID\"  AND \"WOA_BFM_ID\"=33 AND (\"WO_BO_ID\" ='71' or \"WO_BO_ID\" ='72')   AND \"WO_DATA_ID\" IS NOT NULL  and \"RESTD_ID\" is not null) THEN 'ALREADY SENT' ELSE 'PENDING' END) \"STATUS\" FROM  \"TBLTCMASTER\" inner join  \"TBLTRANSMAKES\"  on \"TC_MAKE_ID\" = \"TM_ID\"  inner join  \"TBLSTOREMAST\" on \"SM_ID\" = \"TC_STORE_ID\" left join ";
                strQry += " \"TBLREPAIRERESTIMATIONDETAILS\" on  \"TC_CODE\"=\"RESTD_TC_CODE\" where  \"TC_STATUS\" = 3 and \"TC_CURRENT_LOCATION\"=1   ";

                if (objTcRepair.sroleid == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]) || objTcRepair.sOfficeCode == "")
                {
                    strQry += " and \"TC_PREV_OFFCODE\" like  '" + objTcRepair.sOfficeCode + "%'";
                }
                else
                {
                    strQry += "  and \"TC_PREV_OFFCODE\"= '" + objTcRepair.sOfficeCode + "'";
                }

                if (objTcRepair.sTcId != null)
                {
                    strQry += " AND \"TC_ID\" IN (" + objTcRepair.sTcId + ")";
                }
                else
                {
                    strQry += " AND \"TC_CURRENT_LOCATION\" =1";
                }

                if (objTcRepair.sCapacity != null)
                {
                    strQry += " AND \"TC_CAPACITY\" ='" + objTcRepair.sCapacity + "' ";
                }

                if (objTcRepair.sTcCode != null)
                {
                    strQry += " AND cast(\"TC_CODE\" as text) LIKE '%" + objTcRepair.sTcCode.Trim() + "%'";
                }

                if (objTcRepair.sTcSlno != null)
                {
                    strQry += "AND \"TC_SLNO\" LIKE '%" + objTcRepair.sTcSlno.Trim() + "%'";
                }

                if (objTcRepair.sMakeId != null)
                {
                    strQry += " AND \"TC_MAKE_ID\" ='" + objTcRepair.sMakeId + "'";
                }
                if (objTcRepair.sStoreId != null)
                {
                    //strQry += " AND \"SM_ID\" ='" + objTcRepair.sStoreId + "'";
                }
                if (objTcRepair.sSupplierId != null)
                {
                    strQry += " AND ((\"TS_ID\" ='" + objTcRepair.sSupplierId + "' AND \"TC_LAST_REPAIRER_ID\" is null) or \"TC_LAST_REPAIRER_ID\" ='" + objTcRepair.sSupplierId + "')";
                    // strQry += " AND ((TS_ID= (select TS_ID from TBLTRANSSUPPLIER where TS_NAME='" + txtSupplier.Text.Trim().ToUpper() + "') and RE_LAST_REPAIRER_ID is null) or  RE_LAST_REPAIRER_ID= (select TS_ID from TBLTRANSSUPPLIER where TS_NAME='" + txtSupplier.Text.Trim().ToUpper() + "'))";

                }
                strQry += " ORDER BY \"TC_CODE\" )A limit 50";

                if (objTcRepair.sGuarantyType != null)
                {
                    if (objTcRepair.sGuarantyType == "AGP")
                        strQry += " WHERE \"TC_GUARANTY_TYPE\" ='AGP'";
                    else if (objTcRepair.sGuarantyType == "WGP")
                        strQry += " WHERE \"TC_GUARANTY_TYPE\" ='WGP'";
                    else if (objTcRepair.sGuarantyType == "WRGP")
                        strQry += " WHERE \"TC_GUARANTY_TYPE\" ='WRGP'";
                }
                dt = objCon.FetchDataTable(strQry);
                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        #endregion
        public DataTable LoadFaultTCsearch(clsDTrRepairActivity objTcRepair)
        {
            DataTable dt = new DataTable();
            try
            {
                if (objTcRepair.sroleid == "43")
                {

                    // converted inline query to sp on (13-07-2022)
                    // NpgsqlCommand cmd = new NpgsqlCommand("proc_load_faultytc_search");
                    NpgsqlCommand cmd = new NpgsqlCommand("proc_load_faultytc_search_for_sa");
                    cmd.Parameters.AddWithValue("soffice_code", objTcRepair.sStoreId == null ? "" : objTcRepair.sStoreId);
                    cmd.Parameters.AddWithValue("stc_id", objTcRepair.sTcId == null ? "" : objTcRepair.sTcId);
                    cmd.Parameters.AddWithValue("scapacity", objTcRepair.sCapacity == null ? "" : objTcRepair.sCapacity);
                    cmd.Parameters.AddWithValue("stccode", objTcRepair.sTcCode == null ? "" : objTcRepair.sTcCode);
                    cmd.Parameters.AddWithValue("stcslno", objTcRepair.sTcSlno == null ? "" : objTcRepair.sTcSlno);
                    cmd.Parameters.AddWithValue("sstore_id", objTcRepair.sStoreId == null ? "" : objTcRepair.sStoreId);
                    cmd.Parameters.AddWithValue("sstarrate", objTcRepair.sstarrate == null ? "" : objTcRepair.sstarrate);
                    cmd.Parameters.AddWithValue("ssupplier_id", objTcRepair.sSupplierId == null ? "" : objTcRepair.sSupplierId);
                    cmd.Parameters.AddWithValue("sguaranty_type", objTcRepair.sGuarantyType == null ? "" : objTcRepair.sGuarantyType);
                    dt = objCon.FetchDataTable(cmd);

                    return dt;
                }
                else
                {
                    // converted inline query to sp on (13-07-2022)

                    NpgsqlCommand cmd = new NpgsqlCommand("proc_load_faultytc_search");
                    cmd.Parameters.AddWithValue("soffice_code", Convert.ToInt32(objTcRepair.sStoreId));
                    cmd.Parameters.AddWithValue("stc_id", objTcRepair.sTcId == null ? "" : objTcRepair.sTcId);
                    cmd.Parameters.AddWithValue("scapacity", objTcRepair.sCapacity == null ? "" : objTcRepair.sCapacity);
                    cmd.Parameters.AddWithValue("stccode", objTcRepair.sTcCode == null ? "" : objTcRepair.sTcCode);
                    cmd.Parameters.AddWithValue("stcslno", objTcRepair.sTcSlno == null ? "" : objTcRepair.sTcSlno);
                    cmd.Parameters.AddWithValue("sstore_id", objTcRepair.sStoreId == null ? "" : objTcRepair.sStoreId);
                    cmd.Parameters.AddWithValue("sstarrate", objTcRepair.sstarrate == null ? "" : objTcRepair.sstarrate);
                    cmd.Parameters.AddWithValue("ssupplier_id", objTcRepair.sSupplierId == null ? "" : objTcRepair.sSupplierId);
                    cmd.Parameters.AddWithValue("sguaranty_type", objTcRepair.sGuarantyType == null ? "" : objTcRepair.sGuarantyType);
                    dt = objCon.FetchDataTable(cmd);

                    return dt;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }


        public clsDTrRepairActivity GetFaultTCDetails(clsDTrRepairActivity objTcRepair)
        {
            DataTable dt = new DataTable();
            try
            {
                //string strQry = string.Empty;
                //strQry = "SELECT \"TC_ID\",\"TC_CODE\",\"TC_SLNO\",\"TM_NAME\", TO_CHAR(\"TC_MANF_DATE\",'DD-MON-YYYY') TC_MANF_DATE,CAST(\"TC_CAPACITY\" AS TEXT) TC_CAPACITY,";
                //strQry += " TO_CHAR(\"TC_PURCHASE_DATE\",'DD-MON-YYYY') TC_PURCHASE_DATE,TO_CHAR(\"TC_WARANTY_PERIOD\",'DD-MON-YYYY') TC_WARANTY_PERIOD,(SELECT \"TS_NAME\" FROM \"TBLTRANSSUPPLIER\" WHERE \"TC_SUPPLIER_ID\" = \"TS_ID\") TS_NAME , ";
                //strQry += " (CASE WHEN TO_CHAR(\"TC_WARANTY_PERIOD\",'YYYYMMDD')<TO_CHAR(NOW(),'YYYYMMDD') THEN 'AGP' ";
                //strQry += " WHEN TO_CHAR(\"TC_WARANTY_PERIOD\",'YYYYMMDD') > TO_CHAR(NOW(),'YYYYMMDD') THEN 'WGP' END )TC_GUARANTY_TYPE";
                //strQry += " FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\" WHERE \"TC_MAKE_ID\"=\"TM_ID\"  ";
                //strQry += " AND \"TC_CURRENT_LOCATION\" <>3 AND \"TC_CODE\" =:sTcCode";

                //if (objTcRepair.sRefString != null)
                //{
                //    strQry += " AND \"TC_STATUS\" =4 ";
                //}
                //else
                //{
                //    strQry += " AND \"TC_STATUS\" =7 ";
                //}

                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sTcCode", Convert.ToDouble(objTcRepair.sTcCode));
                //dt = objCon.FetchDataTable(strQry, NpgsqlCommand);
                NpgsqlCommand = new NpgsqlCommand("sp_getfault_tcdetails");
                NpgsqlCommand.Parameters.AddWithValue("stccode", sTcCode);
                NpgsqlCommand.Parameters.AddWithValue("sref", objTcRepair.sRefString);
                dt = objCon.FetchDataTable(NpgsqlCommand);
                if (dt.Rows.Count > 0)
                {
                    objTcRepair.sTcId = dt.Rows[0]["TC_ID"].ToString();
                    objTcRepair.sTcCode = dt.Rows[0]["TC_CODE"].ToString();
                    objTcRepair.sTcSlno = dt.Rows[0]["TC_SLNO"].ToString();
                    objTcRepair.sMakeName = dt.Rows[0]["TM_NAME"].ToString();
                    objTcRepair.sManfDate = dt.Rows[0]["TC_MANF_DATE"].ToString();
                    objTcRepair.sCapacity = dt.Rows[0]["TC_CAPACITY"].ToString();
                    objTcRepair.sPurchaseDate = dt.Rows[0]["TC_PURCHASE_DATE"].ToString();
                    objTcRepair.sWarrantyPeriod = dt.Rows[0]["TC_WARANTY_PERIOD"].ToString();
                    objTcRepair.sSupplierName = dt.Rows[0]["TS_NAME"].ToString();
                    objTcRepair.sGuarantyType = dt.Rows[0]["TC_GUARANTY_TYPE"].ToString();

                }
                return objTcRepair;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objTcRepair;
            }
        }
        public clsDTrRepairActivity GetRepairerTCDetails(clsDTrRepairActivity objTcRepair)
        {
            DataTable dt = new DataTable();
            try
            {
                //string strQry = string.Empty;
                //strQry = "select \"RESTD_EST_NO\",TO_CHAR(\"RESTD_DATE\", 'DD-MON-YYYY') \"RESTD_DATE\",\"RWO_NO\",TO_CHAR(\"RWO_DATE\", 'DD-MON-YYYY') \"RWO_DATE\",\"RSM_PO_NO\", TO_CHAR(\"RSM_PO_DATE\", 'DD-MON-YYYY') \"RSM_PO_DATE\", ";
                //strQry += " \"RESTD_TOTAL_AMOUNT\",\"RSM_PO_QNTY\",\"RESTD_REPAIRER\" ,TO_CHAR(\"RI_INDENT_DATE\", 'DD-MON-YYYY') \"INDENT_DATE\" ,TO_CHAR(\"RI_INVOICE_DATE\", 'DD-MON-YYYY') \"INVOICE_DATE\",\"RI_INDENT_NO\",\"RI_INVOICE_NO\" from \"TBLREPAIRSENTMASTER\" inner join \"TBLREPAIRSENTDETAILS\" on \"RSM_ID\" = \"RSD_RSM_ID\" inner join \"TBLREPAIRERESTIMATIONDETAILS\" on \"RESTD_ID\" = \"RSM_RESTD_ID\" ";
                //strQry += " inner join \"TBLREPAIRERWORKORDER\" on \"RWO_ID\" = \"RSM_RWO_ID\"  LEFT JOIN \"TBLREPAIRERINVOICE\" ON \"RI_RSM_ID\"=\"RSM_ID\" where \"RSM_ID\" = '" + objTcRepair.sRSMID + "' group BY \"RESTD_EST_NO\",\"RESTD_DATE\",\"RWO_NO\",\"RWO_DATE\",\"RSM_PO_NO\",\"RSM_PO_DATE\",\"RESTD_TOTAL_AMOUNT\",\"RSM_PO_QNTY\",\"RESTD_REPAIRER\"  ,\"INDENT_DATE\" ,\"INVOICE_DATE\",\"RI_INDENT_NO\",\"RI_INVOICE_NO\" ";

                NpgsqlCommand cmd = new NpgsqlCommand("sp_getrepairertcdetails");
                cmd.Parameters.AddWithValue("rsm_id", objTcRepair.sRSMID);
                dt = objCon.FetchDataTable(cmd);
                if (dt.Rows.Count > 0)
                {
                    objTcRepair.sEstimationNo = dt.Rows[0]["RESTD_EST_NO"].ToString();
                    objTcRepair.sEstimationDate = dt.Rows[0]["RESTD_DATE"].ToString();
                    objTcRepair.sWorkorderNo = dt.Rows[0]["RWO_NO"].ToString();
                    objTcRepair.sWorkorderDate = dt.Rows[0]["RWO_DATE"].ToString();
                    objTcRepair.sPurchaseOrderNo = dt.Rows[0]["RSM_PO_NO"].ToString();
                    objTcRepair.sPurchaseDate = dt.Rows[0]["RSM_PO_DATE"].ToString();
                    objTcRepair.sEstimationAmount = dt.Rows[0]["RESTD_TOTAL_AMOUNT"].ToString();
                    objTcRepair.sQty = dt.Rows[0]["RSM_PO_QNTY"].ToString();
                    objTcRepair.sRepairerId = dt.Rows[0]["RESTD_REPAIRER"].ToString();

                    objTcRepair.sIndentDate = dt.Rows[0]["INDENT_DATE"].ToString();
                    objTcRepair.sInvoiceDate = dt.Rows[0]["INVOICE_DATE"].ToString();
                    objTcRepair.sIndenteNo = dt.Rows[0]["RI_INDENT_NO"].ToString();
                    objTcRepair.sInvoiceNo = dt.Rows[0]["RI_INVOICE_NO"].ToString();
                }
                return objTcRepair;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objTcRepair;
            }
        }

        public clsDTrRepairActivity AddFaultTCDetails(clsDTrRepairActivity objTcRepair)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                //strQry = "SELECT \"TC_ID\",\"TC_CODE\",\"TC_SLNO\",\"TM_NAME\", TO_CHAR(\"TC_MANF_DATE\",'DD-MON-YYYY') TC_MANF_DATE,CAST(\"TC_CAPACITY\" AS TEXT) TC_CAPACITY,";
                //strQry += " TO_CHAR(\"TC_PURCHASE_DATE\",'DD-MON-YYYY') TC_PURCHASE_DATE,TO_CHAR(\"TC_WARANTY_PERIOD\",'DD-MON-YYYY') TC_WARANTY_PERIOD,(SELECT \"TS_NAME\" FROM \"TBLTRANSSUPPLIER\" WHERE \"TC_SUPPLIER_ID\"=\"TS_ID\") TS_NAME , ";
                //strQry += " (CASE WHEN TO_CHAR(\"TC_WARANTY_PERIOD\",'YYYYMMDD')<TO_CHAR(NOW(),'YYYYMMDD') THEN 'AGP' ";
                //strQry += " WHEN TO_CHAR(\"TC_WARANTY_PERIOD\",'YYYYMMDD') > TO_CHAR(NOW(),'YYYYMMDD') THEN 'WGP' END )TC_GUARANTY_TYPE";
                //strQry += " FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\" WHERE \"TC_MAKE_ID\"=\"TM_ID\"  ";
                //strQry += " AND \"TC_CURRENT_LOCATION\" <>3 AND \"TC_CODE\" =:sTcCode";

                //if (objTcRepair.sRefString != null)
                //{
                //    strQry += " AND \"TC_STATUS\" =4 ";
                //}
                //else
                //{
                //    strQry += " AND \"TC_STATUS\" =3 ";
                //}

                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sTcCode", Convert.ToDouble(objTcRepair.sTcCode));
                //dt = objCon.FetchDataTable(strQry, NpgsqlCommand);

                NpgsqlCommand cmd = new NpgsqlCommand("sp_addfaulttcdetails");
                cmd.Parameters.AddWithValue("stccode", sTcCode);
                cmd.Parameters.AddWithValue("sref", sRefString);
                dt = objCon.FetchDataTable(cmd);
                if (dt.Rows.Count > 0)
                {
                    objTcRepair.sTcId = dt.Rows[0]["TC_ID"].ToString();
                    objTcRepair.sTcCode = dt.Rows[0]["TC_CODE"].ToString();
                    objTcRepair.sTcSlno = dt.Rows[0]["TC_SLNO"].ToString();
                    objTcRepair.sMakeName = dt.Rows[0]["TM_NAME"].ToString();
                    objTcRepair.sManfDate = dt.Rows[0]["TC_MANF_DATE"].ToString();
                    objTcRepair.sCapacity = dt.Rows[0]["TC_CAPACITY"].ToString();
                    objTcRepair.sPurchaseDate = dt.Rows[0]["TC_PURCHASE_DATE"].ToString();
                    objTcRepair.sWarrantyPeriod = dt.Rows[0]["TC_WARANTY_PERIOD"].ToString();
                    objTcRepair.sSupplierName = dt.Rows[0]["TS_NAME"].ToString();
                    objTcRepair.sGuarantyType = dt.Rows[0]["TC_GUARANTY_TYPE"].ToString();
                }
                return objTcRepair;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objTcRepair;
            }
        }

        public DataTable LoadRepairSentDTR(string sRepairMasterId)
        {
            DataTable dt = new DataTable();
            try
            {
                //string strQry = string.Empty;

                //strQry = "select \"TC_ID\",\"TC_CODE\",\"TC_SLNO\",\"TM_NAME\",TO_CHAR(\"TC_MANF_DATE\",'DD-MON-YYYY') TC_MANF_DATE,CAST(\"TC_CAPACITY\" AS TEXT) TC_CAPACITY,  TO_CHAR(\"TC_PURCHASE_DATE\",'DD-MON-YYYY') TC_PURCHASE_DATE, ";
                //strQry += " TO_CHAR(\"TC_WARANTY_PERIOD\",'DD-MON-YYYY') TC_WARANTY_PERIOD,  (SELECT \"TS_NAME\" FROM  \"TBLTRANSSUPPLIER\" WHERE \"TC_SUPPLIER_ID\"=\"TS_ID\") TS_NAME,(SELECT \"DF_GUARANTY_TYPE\" FROM \"TBLDTCFAILURE\" WHERE \"DF_EQUIPMENT_ID\"=\"TC_CODE\" ORDER BY \"DF_ID\" desc limit 1) \"TC_GUARANTY_TYPE\" ";
                //strQry += ",(select \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_ID\"=\"TC_RATING\" AND \"MD_TYPE\"='SR') AS TC_STAR_RATE ,  \"TC_REMARKS\" AS \"REMARKS\", \"TC_REASON\" AS \"REASON\" ,cast(round(\"RSD_REP_COST\",2) as text)  AS ESTIMATION_AMOUNT  from \"TBLREPAIRSENTMASTER\" inner join \"TBLREPAIRSENTDETAILS\" ";
                //strQry += " on \"RSM_ID\"=\"RSD_RSM_ID\" inner join \"TBLREPAIRERESTIMATIONDETAILS\" on \"RSM_RESTD_ID\"=\"RESTD_ID\" inner join \"TBLREPAIRERWORKORDER\" on \"RSM_RWO_ID\" =\"RWO_ID\" inner join \"TBLTCMASTER\" on \"TC_CODE\"=\"RSD_TC_CODE\" ";
                //strQry += " inner join \"TBLTRANSMAKES\" on \"TC_MAKE_ID\"=\"TM_ID\"  WHERE \"RSM_ID\"=:sRepairMasterId ";

                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sRepairMasterId", Convert.ToInt32(sRepairMasterId));
                // dt = objCon.FetchDataTable(strQry, NpgsqlCommand);
                NpgsqlCommand cmd = new NpgsqlCommand("sp_loadrepairsentdtr");
                cmd.Parameters.AddWithValue("rsm_id", sRepairMasterId);
                dt = objCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        # region form commented
        public string[] SaveRepairIssueDetails(string[] sTcCodes, clsDTrRepairActivity objTcRepair)
        {
            string[] Arr = new string[3];
            string strQry0 = string.Empty;
            string strQry = string.Empty;
            string strQry1 = string.Empty;
            string strQry2 = string.Empty;
            string strQry3 = string.Empty;
            string DTrCodes = string.Empty;
            string RsdTcCode = string.Empty;
            string RsmPoNum = string.Empty;


            clsApproval objApproval = new clsApproval();
            StringBuilder sbQuery = new StringBuilder();
            DataTable dt = new DataTable();


            string sRepairsentMasterId = string.Empty;

            string[] strDetailVal = sTcCodes.ToArray();
            string sDTrCode = string.Empty;
            string sRepairMasterDetailsId = string.Empty;

            bool bResult = false;
            try
            {
                NpgsqlCommand = new NpgsqlCommand();
                if (objTcRepair.sRSMID == "")
                {
                    //NpgsqlCommand.Parameters.AddWithValue("sPurchaseOrderNo", objTcRepair.sPurchaseOrderNo.ToUpper());
                    //NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", Convert.ToInt32(objTcRepair.sOfficeCode == "" ? objTcRepair.sStoreId : objTcRepair.sOfficeCode));
                    //string sPONo = objCon.get_value("SELECT \"RSM_ID\" FROM \"TBLREPAIRSENTMASTER\" WHERE UPPER(\"RSM_PO_NO\")=:sPurchaseOrderNo AND \"RSM_DIV_CODE\" =:sOfficeCode", NpgsqlCommand);
                    NpgsqlCommand = new NpgsqlCommand("sp_getpono");
                    NpgsqlCommand.Parameters.AddWithValue("pono", objTcRepair.sPurchaseOrderNo.ToUpper());
                    NpgsqlCommand.Parameters.AddWithValue("rsmdivcode", Convert.ToInt32(objTcRepair.sOfficeCode == "" ? objTcRepair.sStoreId : objTcRepair.sOfficeCode));
                    string sPONo = objDatabse.StringGetValue(NpgsqlCommand);
                    if (sPONo.Length > 0)
                    {
                        Arr[0] = "Purchase Order Number " + objTcRepair.sPurchaseOrderNo.ToUpper() + " Already Exists";
                        Arr[1] = "2";
                        return Arr;
                    }

                    string[] divcode;
                    string divcodenew;
                    if (objTcRepair.sSupRepId == "2")
                    {
                        divcode = objTcRepair.newdivcode.Split('~');

                        NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_clsdtrrepairactivity");
                        NpgsqlCommand.Parameters.AddWithValue("p_key", "GETDIVCODENEW");
                        NpgsqlCommand.Parameters.AddWithValue("p_value", divcode[1]);
                        divcodenew = objDatabse.StringGetValue(NpgsqlCommand);
                        //  divcodenew = objCon.get_value("SELECT \"DIV_CODE\" FROM \"TBLDIVISION\" WHERE \"DIV_NAME\"='" + divcode[1] + "'");
                    }
                    else
                    {
                        string officecode;

                        NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_clsdtrrepairactivity");
                        NpgsqlCommand.Parameters.AddWithValue("p_key", "GETOFFCODE");
                        NpgsqlCommand.Parameters.AddWithValue("p_value", objTcRepair.sOfficeCode);
                        officecode = objDatabse.StringGetValue(NpgsqlCommand);
                        // officecode = objCon.get_value("SELECT \"STO_OFF_CODE\" from \"TBLSTOREOFFCODE\" WHERE \"STO_SM_ID\"='" + objTcRepair.sOfficeCode + "' limit 1");
                        if (officecode == "")
                        {
                            divcodenew = objTcRepair.sOfficeCode;
                        }
                        else
                        {
                            divcodenew = officecode;
                        }
                    }


                    //  NpgsqlCommand.Parameters.AddWithValue("sEstimationNo", Convert.ToInt64(objTcRepair.sEstimationNo));
                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_clsdtrrepairactivity");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETESTNO");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", sEstimationNo);
                    string sEstNo = objDatabse.StringGetValue(NpgsqlCommand);
                    //  string sEstNo = objCon.get_value("SELECT \"RESTD_ID\" FROM \"TBLREPAIRERESTIMATIONDETAILS\" WHERE \"RESTD_EST_NO\"=:sEstimationNo", NpgsqlCommand);
                    if (sEstNo.Length > 0)
                    {
                        Arr[0] = "Estimation Number " + objTcRepair.sEstimationNo + " Already Exists";
                        Arr[1] = "2";
                        return Arr;
                    }

                    NpgsqlCommand = new NpgsqlCommand("fetch_getvalue_clsdtrrepairactivity");
                    NpgsqlCommand.Parameters.AddWithValue("p_key", "GETWONO");
                    NpgsqlCommand.Parameters.AddWithValue("p_value", sWorkorderNo);
                    string sWONo = objDatabse.StringGetValue(NpgsqlCommand);
                    //  NpgsqlCommand.Parameters.AddWithValue("sWorkorderNo", objTcRepair.sWorkorderNo);
                    //  string sWONo = objCon.get_value("SELECT \"RWO_ID\" FROM \"TBLREPAIRERWORKORDER\" WHERE \"RWO_NO\"=:sWorkorderNo", NpgsqlCommand);
                    if (sWONo.Length > 0)
                    {
                        Arr[0] = "Work Order Number " + objTcRepair.sWorkorderNo + " Already Exists";
                        Arr[1] = "2";
                        return Arr;
                    }

                    for (int i = 0; i < strDetailVal.Length; i++)
                    {
                        string RepairDtr = Convert.ToString(strDetailVal[i].Split('~').GetValue(0).ToString().Replace("'", ""));
                        NpgsqlCommand cmd = new NpgsqlCommand("proc_get_tccode_pend_in_repairer");
                        cmd.Parameters.AddWithValue("rsd_tc_code", RepairDtr);
                        dt = objCon.FetchDataTable(cmd);
                        if (dt.Rows.Count > 0)
                        {
                            RsmPoNum = Convert.ToString(dt.Rows[0]["po_no"]);
                            RsdTcCode = Convert.ToString(dt.Rows[0]["tc_code"]);
                            Arr[0] = "Tc Already Invoiced to Repairer with PO No : " + RsmPoNum + " ";
                            Arr[1] = "2";
                            return Arr;
                        }
                    }
                    string[] starr = new string[2];

                    string sRepairEstimationId = objCon.Get_max_no("RESTD_ID", "TBLREPAIRERESTIMATIONDETAILS").ToString();

                    NpgsqlCommand = new NpgsqlCommand("proc_saverepairissuedetails");
                    NpgsqlCommand.Parameters.AddWithValue("srepairestimationid", Convert.ToInt32(sRepairEstimationId));
                    NpgsqlCommand.Parameters.AddWithValue("sestimationno", Convert.ToInt64(objTcRepair.sEstimationNo));
                    NpgsqlCommand.Parameters.AddWithValue("ssuprepid", Convert.ToInt32(objTcRepair.sSupRepId));
                    NpgsqlCommand.Parameters.AddWithValue("stccodes", Convert.ToInt32(sTcCodes.Length));
                    NpgsqlCommand.Parameters.AddWithValue("sestimationamount", Convert.ToString(objTcRepair.sEstimationAmount));
                    NpgsqlCommand.Parameters.AddWithValue("scrby", Convert.ToInt32(objTcRepair.sCrby));
                    NpgsqlCommand.Parameters.AddWithValue("scron", objTcRepair.sCron == null ? "" : objTcRepair.sCron);
                    //NpgsqlCommand.Parameters.AddWithValue("scron",DateTime.Now);
                    NpgsqlCommand.Parameters.AddWithValue("sestimationdate", objTcRepair.sEstimationDate);
                    NpgsqlCommand.Parameters.AddWithValue("sofficecode", objTcRepair.sOfficeCode);
                    NpgsqlCommand.Parameters.AddWithValue("sstarrate", objTcRepair.sstarrate);

                    NpgsqlCommand.Parameters.Add("msg", NpgsqlDbType.Text);
                    NpgsqlCommand.Parameters.Add("op_id", NpgsqlDbType.Text);
                    NpgsqlCommand.Parameters["msg"].Direction = ParameterDirection.Output;
                    NpgsqlCommand.Parameters["op_id"].Direction = ParameterDirection.Output;
                    Arr[0] = "msg";
                    Arr[1] = "op_id";
                    objCon.Execute(NpgsqlCommand, Arr, 0);
                    //  strQry0 = "INSERT INTO \"TBLREPAIRERESTIMATIONDETAILS\"(\"RESTD_ID\", \"RESTD_EST_NO\", \"RESTD_REPAIRER\", \"RESTD_ITEM_QNTY\", \"RESTD_TOTAL_AMOUNT\", \"RESTD_CRBY\", \"RESTD_CRON\", \"RESTD_DATE\", \"RESTD_OFF_CODE\",\"RESTD_STARRATE\") VALUES";
                    //    strQry0 += "( '" + sRepairEstimationId + "', '" + objTcRepair.sEstimationNo + "', '" + objTcRepair.sSupRepId + "', '" + sTcCodes.Length + "','" + objTcRepair.sEstimationAmount + "','" + objTcRepair.sCrby + "',now(), TO_DATE( '" + objTcRepair.sEstimationDate + "','dd/MM/yyyy'), '" + objTcRepair.sOfficeCode + "', '" + objTcRepair.sstarrate + "')";
                    //    objCon.ExecuteQry(strQry0, NpgsqlCommand);

                    string sRepairWorkOrderId = objCon.Get_max_no("RWO_ID", "TBLREPAIRERWORKORDER").ToString();

                    //strQry = "INSERT INTO \"TBLREPAIRERWORKORDER\"(\"RWO_ID\", \"RWO_NO\", \"RWO_EST_ID\", \"RWO_DATE\", \"RWO_ACC_CODE\", \"RWO_OFF_CODE\", \"RWO_CRBY\", \"RWO_CRON\", \"RWO_AUTO_SLNO\") VALUES";
                    // strQry += "( '" + sRepairWorkOrderId + "', '" + objTcRepair.sWorkorderNo + "',  '" + objTcRepair.sWorkorderNo + "', TO_DATE('" + objTcRepair.sWorkorderDate + "' ,'dd/MM/yyyy'), '74.1177',  '" + objTcRepair.sOfficeCode + "',  '" + objTcRepair.sCrby + "', now(), '" + objTcRepair.swoslno + "')";
                    // objCon.ExecuteQry(strQry, NpgsqlCommand);
                    NpgsqlCommand = new NpgsqlCommand("proc_saverepairissuedetailsfortblwo");
                    NpgsqlCommand.Parameters.AddWithValue("srepairworkorderid", Convert.ToInt32(sRepairWorkOrderId));
                    NpgsqlCommand.Parameters.AddWithValue("sworkorderno", Convert.ToString(objTcRepair.sWorkorderNo));
                    NpgsqlCommand.Parameters.AddWithValue("srepairestimationid", sRepairEstimationId);
                    NpgsqlCommand.Parameters.AddWithValue("sworkorderdate", objTcRepair.sWorkorderDate);
                    NpgsqlCommand.Parameters.AddWithValue("acccode", "74.1177");
                    NpgsqlCommand.Parameters.AddWithValue("sofficecode", Convert.ToInt32(objTcRepair.sOfficeCode));
                    NpgsqlCommand.Parameters.AddWithValue("scrby", Convert.ToInt32(objTcRepair.sCrby));
                    //  cmd.Parameters.AddWithValue("scron",);
                    NpgsqlCommand.Parameters.AddWithValue("scron", objTcRepair.sCron == null ? "" : objTcRepair.sCron);
                    NpgsqlCommand.Parameters.AddWithValue("swoslno", Convert.ToInt16(objTcRepair.swoslno));

                    NpgsqlCommand.Parameters.Add("op_id", NpgsqlDbType.Text);
                    NpgsqlCommand.Parameters.Add("msg", NpgsqlDbType.Text);

                    NpgsqlCommand.Parameters["op_id"].Direction = ParameterDirection.Output;
                    NpgsqlCommand.Parameters["msg"].Direction = ParameterDirection.Output;
                    Arr[0] = "msg";
                    Arr[1] = "op_id";
                    objCon.Execute(NpgsqlCommand, Arr, 0);

                    if (sTcCodes.Length > 0)
                    {




                        sRepairsentMasterId = objCon.Get_max_no("RSM_ID", "TBLREPAIRSENTMASTER").ToString();
                        NpgsqlCommand = new NpgsqlCommand("proc_saverepairissuedetailsfortblrpsm");
                        NpgsqlCommand.Parameters.AddWithValue("srepairsentmasterid", Convert.ToInt32(sRepairsentMasterId));
                        NpgsqlCommand.Parameters.AddWithValue("rsmissuedate", objTcRepair.sIssueDate);
                        NpgsqlCommand.Parameters.AddWithValue("spurchaseorderno", Convert.ToString(objTcRepair.sPurchaseOrderNo.ToUpper()));
                        NpgsqlCommand.Parameters.AddWithValue("spurchasedate", objTcRepair.sPurchaseDate);
                        NpgsqlCommand.Parameters.AddWithValue("sinvoiceno", objTcRepair.sInvoiceNo);
                        NpgsqlCommand.Parameters.AddWithValue("sguarantytype", objTcRepair.sGuarantyType);
                        NpgsqlCommand.Parameters.AddWithValue("stype", Convert.ToInt32(objTcRepair.sType));
                        NpgsqlCommand.Parameters.AddWithValue("ssuprepid", Convert.ToInt32(objTcRepair.sSupRepId));
                        NpgsqlCommand.Parameters.AddWithValue("sofficecode", Convert.ToInt32(objTcRepair.sOfficeCode));
                        NpgsqlCommand.Parameters.AddWithValue("scrby", Convert.ToInt32(objTcRepair.sCrby));
                        NpgsqlCommand.Parameters.AddWithValue("sqty", Convert.ToInt32(objTcRepair.sQty));
                        NpgsqlCommand.Parameters.AddWithValue("smanualinvoiceno", objTcRepair.sManualInvoiceNo);
                        NpgsqlCommand.Parameters.AddWithValue("soldpono", objTcRepair.sOldPONo);
                        NpgsqlCommand.Parameters.AddWithValue("sporemarks", objTcRepair.sPORemarks);
                        NpgsqlCommand.Parameters.AddWithValue("divcodenew", Convert.ToInt32(divcodenew));
                        NpgsqlCommand.Parameters.AddWithValue("srepairestimationid", Convert.ToInt32(sRepairEstimationId));
                        NpgsqlCommand.Parameters.AddWithValue("srepairworkorderid", Convert.ToInt32(sRepairWorkOrderId));
                        NpgsqlCommand.Parameters.Add("msg", NpgsqlDbType.Text);
                        NpgsqlCommand.Parameters.Add("op_id", NpgsqlDbType.Text);
                        NpgsqlCommand.Parameters["msg"].Direction = ParameterDirection.Output;
                        NpgsqlCommand.Parameters["op_id"].Direction = ParameterDirection.Output;
                        Arr[0] = "msg";
                        Arr[1] = "op_id";
                        objCon.Execute(NpgsqlCommand, Arr, 0);

                        //strQry1 = "INSERT INTO \"TBLREPAIRSENTMASTER\" (\"RSM_ID\",\"RSM_ISSUE_DATE\",\"RSM_PO_NO\",\"RSM_PO_DATE\",\"RSM_INV_NO\",\"RSM_INV_DATE\",\"RSM_GUARANTY_TYPE\",\"RSM_SUPREP_TYPE\",";
                        //strQry1 += " \"RSM_SUPREP_ID\",\"RSM_DIV_CODE\",\"RSM_CRBY\",\"RSM_PO_QNTY\",\"RSM_MANUAL_INV_NO\",\"RSM_OLD_PO_NO\",\"RSM_REMARKS\",\"RSM_NEW_DIV_CODE\",\"RSM_RESTD_ID\",\"RSM_RWO_ID\") VALUES (";
                        //strQry1 += "  '" + sRepairsentMasterId + "',null,'" + objTcRepair.sPurchaseOrderNo.ToUpper() + "',TO_DATE('" + objTcRepair.sPurchaseDate + "','DD/MM/YYYY'),'" + objTcRepair.sInvoiceNo + "',null, ";
                        //strQry1 += " '" + objTcRepair.sGuarantyType + "','" + Convert.ToDouble(objTcRepair.sType) + "','" + Convert.ToInt32(objTcRepair.sSupRepId) + "', '" + Convert.ToInt32(objTcRepair.sOfficeCode) + "','" + Convert.ToInt32(objTcRepair.sCrby) + "','" + Convert.ToDouble(objTcRepair.sQty) + "', ";
                        //strQry1 += " '" + objTcRepair.sManualInvoiceNo + "','" + objTcRepair.sOldPONo + "','" + objTcRepair.sPORemarks + "','" + Convert.ToInt32(divcodenew) + "','" + sRepairEstimationId + "', '" + sRepairWorkOrderId + "')";
                        //objCon.ExecuteQry(strQry1, NpgsqlCommand);
                    }

                    for (int i = 0; i < strDetailVal.Length; i++)
                    {

                        sRepairMasterDetailsId = objCon.Get_max_no("RSD_ID", "TBLREPAIRSENTDETAILS").ToString();
                        string key1 = strDetailVal[i].Split('~').GetValue(0).ToString().Replace("'", "");
                        string key2 = strDetailVal[i].Split('~').GetValue(2).ToString();
                        string key3 = strDetailVal[i].Split('~').GetValue(3).ToString();
                        string key4 = strDetailVal[i].Split('~').GetValue(4).ToString();
                        string key5 = strDetailVal[i].Split('~').GetValue(5).ToString();

                        NpgsqlCommand = new NpgsqlCommand("proc_saverepairissuedetailsfortblrpsmrsdid");
                        NpgsqlCommand.Parameters.AddWithValue("srepairmasterdetailsid", Convert.ToString(sRepairMasterDetailsId));
                        NpgsqlCommand.Parameters.AddWithValue("srepairsentmasterid", Convert.ToString(sRepairsentMasterId));
                        NpgsqlCommand.Parameters.AddWithValue("rsd_tccode", Convert.ToString(key1));
                        NpgsqlCommand.Parameters.AddWithValue("scrby", Convert.ToString(objTcRepair.sCrby));
                        NpgsqlCommand.Parameters.AddWithValue("rsd_repcost", Convert.ToString(key2));
                        NpgsqlCommand.Parameters.AddWithValue("rsd_invoiceguarrenty_type", Convert.ToString(key3));
                        NpgsqlCommand.Parameters.AddWithValue("rsd_remarkscgp", Convert.ToString(key4));
                        //  strQry2 += " '" + Convert.ToString(strDetailVal[i].Split('~').GetValue(3).ToString()) + "','" + Convert.ToString(strDetailVal[i].Split('~').GetValue(4).ToString());
                        NpgsqlCommand.Parameters.AddWithValue("rsd_reasoncgp", Convert.ToString(key5));
                        NpgsqlCommand.Parameters.Add("msg", NpgsqlDbType.Text);
                        NpgsqlCommand.Parameters.Add("op_id", NpgsqlDbType.Text);
                        NpgsqlCommand.Parameters["msg"].Direction = ParameterDirection.Output;
                        NpgsqlCommand.Parameters["op_id"].Direction = ParameterDirection.Output;
                        Arr[0] = "msg";
                        Arr[1] = "op_id";
                        objCon.Execute(NpgsqlCommand, Arr, 0);



                        // strQry2 = " INSERT INTO \"TBLREPAIRSENTDETAILS\" (\"RSD_ID\",\"RSD_RSM_ID\",\"RSD_TC_CODE\",\"RSD_CRBY\",\"RSD_REP_COST\",\"RSD_INVOICE_GUARRENTY_TYPE\",\"RSD_REMARKS_CGP\",\"RSD_REASON_CGP\") VALUES ('" + sRepairMasterDetailsId + "','" + sRepairsentMasterId + "',";
                        // strQry2 += " " + Convert.ToString(strDetailVal[i].Split('~').GetValue(0).ToString()) + ",'" + Convert.ToInt32(objTcRepair.sCrby) + "','" + Convert.ToString(strDetailVal[i].Split('~').GetValue(2).ToString()) + "', ";
                        //   strQry2 += " '" + Convert.ToString(strDetailVal[i].Split('~').GetValue(3).ToString()) + "','" + Convert.ToString(strDetailVal[i].Split('~').GetValue(4).ToString()) + "','" + Convert.ToString(strDetailVal[i].Split('~').GetValue(5).ToString()) + "')";

                        // objCon.ExecuteQry(strQry2, NpgsqlCommand);

                        //strQry = "UPDATE \"TBLTCMASTER\" SET \"TC_UPDATED_EVENT_ID\" ='" + Convert.ToInt32(sRepairMasterDetailsId) + "',";
                        ////strQry += "\"TC_LAST_REPAIRER_ID\" =:sSupRepId where \"TC_CODE\" =:strDetailVal";
                        //strQry += "\"TC_LAST_REPAIRER_ID\" ='" + Convert.ToInt32(objTcRepair.sSupRepId) + "' where \"TC_CODE\" =" + Convert.ToString(strDetailVal[i].Split('~').GetValue(0).ToString()) + "";

                        //NpgsqlCommand.Parameters.AddWithValue("sSupRepId", Convert.ToInt32(objTcRepair.sSupRepId));
                        //NpgsqlCommand.Parameters.AddWithValue("strDetailVal", Convert.ToString(strDetailVal[i].Split('~').GetValue(0).ToString()));
                        //objCon.ExecuteQry(strQry, NpgsqlCommand);

                        string strdetailval = strDetailVal[i].Split('~').GetValue(0).ToString().Replace("'", "");
                        NpgsqlCommand = new NpgsqlCommand("proc_saveupdaterepairissuedetailss");
                        NpgsqlCommand.Parameters.AddWithValue("p_srepairmasterdetailsid", Convert.ToString(sRepairMasterDetailsId));
                        NpgsqlCommand.Parameters.AddWithValue("ssuprepid", sSupRepId);
                        NpgsqlCommand.Parameters.AddWithValue("strdetailval", Convert.ToString(strdetailval));
                        NpgsqlCommand.Parameters.Add("op_id", NpgsqlDbType.Text);
                        NpgsqlCommand.Parameters.Add("msg", NpgsqlDbType.Text);
                        NpgsqlCommand.Parameters["op_id"].Direction = ParameterDirection.Output;
                        NpgsqlCommand.Parameters["msg"].Direction = ParameterDirection.Output;


                        Arr[0] = "msg";
                        Arr[1] = "op_id";
                        objCon.Execute(NpgsqlCommand, Arr, 0);
                        sDTrCode += strDetailVal[i].Split('~').GetValue(0).ToString() + ",";
                        bResult = true;
                    }
                }
                else
                {
                    for (int i = 0; i < strDetailVal.Length; i++)
                    {
                        sDTrCode += strDetailVal[i].Split('~').GetValue(0).ToString() + ",";
                        bResult = true;
                    }
                }

                if (bResult == true)
                {
                    if (objTcRepair.sRSMID == "")
                    {

                        #region WorkFlow

                        if (!sDTrCode.StartsWith(","))
                        {
                            sDTrCode = "," + sDTrCode;
                        }
                        if (!sDTrCode.EndsWith(","))
                        {
                            sDTrCode = sDTrCode + ",";
                        }

                        sDTrCode = sDTrCode.Substring(1, sDTrCode.Length - 1);
                        sDTrCode = sDTrCode.Substring(0, sDTrCode.Length - 1);

                        strQry3 = "UPDATE \"TBLTCMASTER\" SET \"TC_CURRENT_LOCATION\" =3, \"TC_UPDATED_EVENT_ID\" ='" + sRepairMasterDetailsId + "', \"TC_UPDATED_EVENT\" ='REPAIRER ISSUE',";
                        strQry3 += " \"TC_LAST_REPAIRER_ID\" ='" + objTcRepair.sSupRepId + "' WHERE \"TC_CODE\" IN (" + sDTrCode + ")";

                        strQry3 = strQry3.Replace("'", "''");
                        DTrCodes = sDTrCode.Replace("'", "");

                        objApproval.sFormName = objTcRepair.sFormName;
                        objApproval.sRecordId = sRepairsentMasterId;
                    }
                    else
                    {
                        objApproval.sFormName = objTcRepair.sFormName;
                        objApproval.sRecordId = objTcRepair.sRSMID;
                    }
                    objApproval.sFormName = objTcRepair.sFormName;
                    //  objApproval.sRecordId = objTcRepair.sRSMID;
                    // objApproval.sRecordId = sRepairsentMasterId;
                    objApproval.sOfficeCode = objTcRepair.sOfficeCode;
                    objApproval.sClientIp = objTcRepair.sClientIP;
                    objApproval.sCrby = objTcRepair.sCrby;
                    objApproval.sWFObjectId = objTcRepair.sWFObjectId;
                    //objApproval.sDataReferenceId = sDTrCode;
                    objApproval.sDataReferenceId = DTrCodes;
                    objApproval.sBOId = "20";
                    NpgsqlCommand = new NpgsqlCommand();
                    objApproval.sRefOfficeCode = objTcRepair.sOfficeCode;

                    objApproval.sQryValues = strQry3;
                    objApproval.sMainTable = "TBLREPAIRSENTMASTER";
                    objApproval.sDescription = "Faulty Transformer issue to Supplier / Repairer with Estimation No " + objTcRepair.sEstimationNo + " and Work Order No " + objTcRepair.sWorkorderNo;


                    if (objTcRepair.sActionType == "M")
                    {
                        //strQry = "UPDATE \"TBLREPAIRSENTMASTER\" SET \"RSM_ISSUE_DATE\" = to_date('" + objTcRepair.sIssueDate + "','DD/MM/YYYY'), \"RSM_PO_DATE\" = to_date('" + objTcRepair.sPurchaseDate + "','DD/MM/YYYY'), ";
                        //strQry += " \"RSM_MANUAL_INV_NO\" = '" + objTcRepair.sManualInvoiceNo + "', \"RSM_OLD_PO_NO\" = '" + objTcRepair.sOldPONo + "', \"RSM_REMARKS\" = '" + objTcRepair.sPORemarks + "' WHERE \"RSM_ID\" = '" + objTcRepair.sRSMID + "';";
                        //objCon.ExecuteQry(strQry, NpgsqlCommand);


                        updatemodifyeddata(objTcRepair.sIssueDate, objTcRepair.sPurchaseDate, objTcRepair.sManualInvoiceNo, objTcRepair.sOldPONo, objTcRepair.sPORemarks, objTcRepair.sRSMID);


                        objApproval.sWFDataId = objTcRepair.sWFDataId;
                        objApproval.SaveWorkflowObjects(objApproval);
                        objApproval.sOfficeCode = objTcRepair.sOfficeCode;

                    }
                    else
                    {
                        objApproval.SaveWorkFlowData(objApproval);
                        //  objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow();
                        objApproval.SaveWorkflowObjects(objApproval);
                        objApproval.sOfficeCode = objTcRepair.sOfficeCode;
                    }
                    #endregion

                    //Arr[0] = "Transformers Issued Sucessfully to Repairer/Supplier";
                    Arr[0] = "Details Saved Successfully";
                    Arr[1] = "0";
                    Arr[2] = sRepairsentMasterId;
                }
                else
                {
                    Arr[0] = "No Transformer Exists to Issue for Repairer/Supplier";
                    Arr[1] = "2";
                }
                return Arr;
            }
            catch (Exception ex)
            {
                objCon.RollBackTrans();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }
        #endregion
        public string[] SaveRepairIvoiceDetails(clsDTrRepairActivity objTcRepair)
        {
            string[] Arr = new string[2];
            string strQry0 = string.Empty;

            clsApproval objApproval = new clsApproval();

            string sRepairsentMasterId = string.Empty;

            try
            {
                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getestno");
                cmd.Parameters.AddWithValue("sinvoiceno", Convert.ToInt64(objTcRepair.sInvoiceNo));
                string sEstNo = objDatabse.StringGetValue(cmd);
                //string sEstNo = objCon.get_value("SELECT \"RI_ID\" FROM \"TBLREPAIRERINVOICE\" WHERE \"RI_INVOICE_NO\"='" + objTcRepair.sInvoiceNo + "'", NpgsqlCommand);
                if (sEstNo.Length > 0)
                {
                    Arr[0] = "Invoice Number " + objTcRepair.sInvoiceNo + " Already Exists";
                    Arr[1] = "2";
                    return Arr;
                }

                NpgsqlCommand cmmd = new NpgsqlCommand("sp_getwono");
                cmmd.Parameters.AddWithValue("sinvoiceno", Convert.ToInt64(objTcRepair.sInvoiceNo));
                string sWONo = objDatabse.StringGetValue(cmmd);
                // string sWONo = objCon.get_value("SELECT \"RI_ID\" FROM \"TBLREPAIRERINVOICE\" WHERE cast(\"RI_INDENT_NO\" as int8)='" + objTcRepair.sInvoiceNo + "'", NpgsqlCommand);
                if (sWONo.Length > 0)
                {
                    Arr[0] = "Invoice Number " + objTcRepair.sInvoiceNo + " Already Exists";
                    Arr[1] = "2";
                    return Arr;
                }


                //NpgsqlCommand = new NpgsqlCommand();
                //string sRepairInvoiceId = objCon.Get_max_no("RI_ID", "TBLREPAIRERINVOICE").ToString();

                //strQry0 = "UPDATE \"TBLREPAIRSENTMASTER\" SET \"RSM_INV_NO\"='" + objTcRepair.sInvoiceNo + "' , \"RSM_INV_DATE\"=TO_DATE('" + objTcRepair.sInvoiceDate + "','DD/MM/YYYY'), \"RSM_INDENT_NO\"='" + objTcRepair.sIndenteNo + "' , \"RSM_INDENT_DATE\"=TO_DATE('" + objTcRepair.sIndentDate + "','DD/MM/YYYY') WHERE \"RSM_ID\"='" + objTcRepair.sRSMID + "'";
                //objCon.ExecuteQry(strQry0, NpgsqlCommand);


                //strQry0 = "INSERT INTO \"TBLREPAIRERINVOICE\"(\"RI_ID\",\"RI_RSM_ID\",\"RI_INVOICE_NO\",\"RI_INVOICE_DATE\",\"RI_INDENT_NO\",\"RI_INDENT_DATE\",\"RI_CRON\",\"RI_CRBY\") VALUES ";
                //strQry0 += " ('" + sRepairInvoiceId + "','" + objTcRepair.sRSMID + "','" + objTcRepair.sInvoiceNo + "',TO_DATE('" + objTcRepair.sInvoiceDate + "','DD/MM/YYYY'),'" + objTcRepair.sIndenteNo + "',TO_DATE('" + objTcRepair.sIndentDate + "','DD/MM/YYYY'),NOW(),'" + objTcRepair.sCrby + "' )";
                //objCon.ExecuteQry(strQry0, NpgsqlCommand);
                // command.Parameters.AddWithValue("rsm_invno", objTcRepair.sInvoiceNo);
                //  command.Parameters.AddWithValue("rsm_invdate", objTcRepair.sInvoiceDate);
                // command.Parameters.AddWithValue("rsm_indentno", objTcRepair.sIndenteNo);
                //  command.Parameters.AddWithValue("rsm_indentdate", objTcRepair.sIndentDate);
                //objCon.ExecuteQry(strQry0, command);

                string[] ArrInvoicedetails = new string[3];
                NpgsqlCommand command = new NpgsqlCommand("sp_saverepairinvoicedetails");
                command.Parameters.AddWithValue("rsmid", Convert.ToString(objTcRepair.sRSMID ?? ""));
                command.Parameters.AddWithValue("srepairinvoiceid", Convert.ToString(objTcRepair.sRSMID ?? ""));
                command.Parameters.AddWithValue("ri_invoice_no", Convert.ToString(objTcRepair.sInvoiceNo ?? ""));
                command.Parameters.AddWithValue("ri_invoice_date", Convert.ToString(objTcRepair.sInvoiceDate ?? ""));
                command.Parameters.AddWithValue("ri_indent_no", Convert.ToString(objTcRepair.sIndenteNo ?? ""));
                command.Parameters.AddWithValue("ri_indent_date", Convert.ToString(objTcRepair.sIndentDate ?? ""));
                command.Parameters.AddWithValue("cron", Convert.ToString(objTcRepair.sCron ?? ""));
                command.Parameters.AddWithValue("scrby", Convert.ToString(objTcRepair.sCrby ?? ""));
                command.Parameters.Add("op_id", NpgsqlDbType.Text);
                command.Parameters.Add("msg", NpgsqlDbType.Text);
                command.Parameters.Add("pk_id", NpgsqlDbType.Text);
                command.Parameters["op_id"].Direction = ParameterDirection.Output;
                command.Parameters["msg"].Direction = ParameterDirection.Output;
                command.Parameters["pk_id"].Direction = ParameterDirection.Output;
                ArrInvoicedetails[0] = "op_id";
                ArrInvoicedetails[1] = "msg";
                ArrInvoicedetails[2] = "pk_id";
                ArrInvoicedetails = objCon.Execute(command, ArrInvoicedetails, 3);


                objApproval.sFormName = "RepairerInvoiceCreation";
                objApproval.sRecordId = objTcRepair.sRSMID;// sRepairInvoiceId;
                objApproval.sOfficeCode = objTcRepair.sOfficeCode;
                objApproval.sClientIp = objTcRepair.sClientIP;
                objApproval.sCrby = objTcRepair.sCrby;
                objApproval.sWFObjectId = objTcRepair.sWFObjectId;
                objApproval.sDataReferenceId = objTcRepair.sRSMID;
                objApproval.sBOId = "1009";
                NpgsqlCommand = new NpgsqlCommand();
                objApproval.sRefOfficeCode = objTcRepair.sOfficeCode;


                objApproval.sQryValues = "";
                objApproval.sMainTable = "TBLREPAIRERINVOICE";
                objApproval.sDescription = "Repairer Invoice creation of Invoice No " + objTcRepair.sInvoiceNo + " and Indent No " + objTcRepair.sIndenteNo;


                objApproval.SaveWorkFlowData(objApproval);
                //  objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow();
                objApproval.SaveWorkflowObjects(objApproval);
                objApproval.sOfficeCode = objTcRepair.sOfficeCode;

                Arr[0] = "Details Saved Successfully";
                Arr[1] = "0";



                return Arr;
            }
            catch (Exception ex)
            {
                objCon.RollBackTrans();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }


        #endregion

        #region Testing Activity

        /// <summary>
        /// for load the detals of pending to test tc details
        /// </summary>
        /// <param name="objTestPending"></param>
        /// <returns></returns>
        public DataTable LoadTestOrDeliverPendingDTR(clsDTrRepairActivity objTestPending)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                #region
                //strQry = " SELECT \"RSM_PO_NO\",TO_CHAR(\"RSM_ISSUE_DATE\",'DD-MON-YYYY') RSM_ISSUE_DATE,TO_CHAR(\"RSM_PO_DATE\",'DD-MON-YYYY') RSM_PO_DATE , \"RSD_ID\", \"TC_CODE\",\"TC_SLNO\", (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\" =  \"TC_MAKE_ID\") TM_NAME, ";
                //strQry += " CAST(\"TC_CAPACITY\" AS TEXT) AS CAPACITY, TO_CHAR(\"TC_MANF_DATE\",'DD-MON-YYYY') TC_MANF_DATE,";
                //strQry += " (CASE WHEN \"RSM_SUPREP_TYPE\" ='2' THEN (SELECT \"TR_NAME\" FROM \"TBLTRANSREPAIRER\" TR WHERE TR.\"TR_ID\"= \"RSM_SUPREP_ID\") WHEN ";
                //strQry += " \"RSM_SUPREP_TYPE\" ='1' THEN (SELECT \"TS_NAME\" FROM \"TBLTRANSSUPPLIER\"  WHERE \"TS_ID\" = \"RSM_SUPREP_ID\" ) END ) SUP_REPNAME,\"RSM_OLD_PO_NO\", \"RSM_REMARKS\"  ";
                //strQry += " FROM \"TBLREPAIRSENTMASTER\", \"TBLREPAIRSENTDETAILS\",\"TBLREPAIRERINVOICE\", \"TBLTCMASTER\" WHERE  \"RSM_ID\" = \"RSD_RSM_ID\" AND \"RSD_TC_CODE\" = \"TC_CODE\" AND \"TC_CURRENT_LOCATION\" ='3' AND \"RSM_ID\" = \"RI_RSM_ID\" ";
                //strQry += " AND \"RSD_DELIVARY_DATE\" IS NULL AND \"RSD_ID\" NOT IN (SELECT \"IND_RSD_ID\" FROM \"TBLINSPECTIONDETAILS\" WHERE \"IND_INSP_RESULT\" IN (1,3,4))";
                //strQry += " AND CAST(\"RSM_DIV_CODE\" AS TEXT) LIKE '" + objTestPending.sOfficeCode + "%'";

                //if (objTestPending.sRepairDetailsId != null)
                //{
                //    strQry += " AND \"RSD_ID\" IN (" + objTestPending.sRepairDetailsId + ") ";
                //}
                //if (objTestPending.sRepairerId != null)
                //{
                //    strQry += " AND \"RSM_SUPREP_ID\" ='" + objTestPending.sRepairerId.ToString().ToUpper() + "' AND \"RSM_SUPREP_TYPE\" ='2'";
                //}

                //if (objTestPending.sSupplierId != null)
                //{
                //    strQry += " AND \"RSM_SUPREP_ID\" ='" + objTestPending.sSupplierId.ToString().ToUpper() + "' AND \"RSM_SUPREP_TYPE\" ='1'";
                //}
                //if (objTestPending.sPurchaseOrderNo.Trim() != "")
                //{
                //    strQry += " AND UPPER(\"RSM_PO_NO\")='" + objTestPending.sPurchaseOrderNo.ToString().ToUpper() + "'";
                //}
                //if (objTestPending.sCapacity != null)
                //{
                //    strQry += " AND \"TC_CAPACITY\" ='" + objTestPending.sCapacity.ToString().ToUpper() + "'";
                //}
                //if (objTestPending.sMakeId != null)
                //{
                //    strQry += " AND \"TC_MAKE_ID\" ='" + objTestPending.sMakeId.ToString() + "'";
                //}
                ////if (objPending.sPendingDays.Trim() != "")
                ////{
                ////    strQry += " AND ROUND(SYSDATE-TR_ISSUE_DATE) >" + objPending.sPendingDays.ToString() + "";
                ////}
                //strQry += "UNION";
                //strQry += " SELECT \"RSM_PO_NO\",TO_CHAR(\"RSM_ISSUE_DATE\",'DD-MON-YYYY') RSM_ISSUE_DATE,TO_CHAR(\"RSM_PO_DATE\",'DD-MON-YYYY') RSM_PO_DATE , \"RSD_ID\", \"TC_CODE\",\"TC_SLNO\", (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\" =  \"TC_MAKE_ID\") TM_NAME, ";
                //strQry += " CAST(\"TC_CAPACITY\" AS TEXT) AS CAPACITY, TO_CHAR(\"TC_MANF_DATE\",'DD-MON-YYYY') TC_MANF_DATE,";
                //strQry += " (CASE WHEN \"RSM_SUPREP_TYPE\" ='2' THEN (SELECT \"TR_NAME\" FROM \"TBLTRANSREPAIRER\" TR WHERE TR.\"TR_ID\"= \"RSM_SUPREP_ID\") WHEN ";
                //strQry += " \"RSM_SUPREP_TYPE\" ='1' THEN (SELECT \"TS_NAME\" FROM \"TBLTRANSSUPPLIER\"  WHERE \"TS_ID\" = \"RSM_SUPREP_ID\" ) END ) SUP_REPNAME,\"RSM_OLD_PO_NO\", \"RSM_REMARKS\"  ";
                //strQry += " FROM \"TBLREPAIRSENTMASTER\", \"TBLREPAIRSENTDETAILS\",\"TBLREPAIRERINVOICE\", \"TBLTCMASTER\",\"TBLTRANSREPAIRER\" WHERE  \"RSM_ID\" = \"RSD_RSM_ID\" AND \"RSD_TC_CODE\" = \"TC_CODE\" AND \"TC_CURRENT_LOCATION\" ='3' AND \"RSM_ID\" = \"RI_RSM_ID\" ";
                //strQry += " AND  \"TR_ID\"=\"RSM_SUPREP_ID\" and \"TR_MEGA_R_FLAG\"=1 AND  \"RSD_DELIVARY_DATE\" IS NULL AND \"RSD_ID\" NOT IN (SELECT \"IND_RSD_ID\" FROM \"TBLINSPECTIONDETAILS\" WHERE \"IND_INSP_RESULT\" IN (1,3,4))";

                //if (objTestPending.sRepairDetailsId != null)
                //{
                //    strQry += " AND \"RSD_ID\" IN (" + objTestPending.sRepairDetailsId + ") ";
                //}
                //if (objTestPending.sRepairerId != null)
                //{
                //    strQry += " AND \"RSM_SUPREP_ID\" ='" + objTestPending.sRepairerId.ToString().ToUpper() + "' AND \"RSM_SUPREP_TYPE\" ='2'";
                //}

                //if (objTestPending.sSupplierId != null)
                //{
                //    strQry += " AND \"RSM_SUPREP_ID\" ='" + objTestPending.sSupplierId.ToString().ToUpper() + "' AND \"RSM_SUPREP_TYPE\" ='1'";
                //}
                //if (objTestPending.sPurchaseOrderNo.Trim() != "")
                //{
                //    strQry += " AND UPPER(\"RSM_PO_NO\")='" + objTestPending.sPurchaseOrderNo.ToString().ToUpper() + "'";
                //}
                //if (objTestPending.sCapacity != null)
                //{
                //    strQry += " AND \"TC_CAPACITY\" ='" + objTestPending.sCapacity.ToString().ToUpper() + "'";
                //}
                //if (objTestPending.sMakeId != null)
                //{
                //    strQry += " AND \"TC_MAKE_ID\" ='" + objTestPending.sMakeId.ToString() + "'";
                //}

                //dt = objCon.FetchDataTable(strQry);
                //return dt;
                #endregion
                NpgsqlCommand cmd = new NpgsqlCommand("loadtestordeliverpendingdtr");
                cmd.Parameters.AddWithValue("rsm_div_code", Convert.ToString(objTestPending.sOfficeCode ?? ""));
                cmd.Parameters.AddWithValue("srepairdetailsid", Convert.ToString(objTestPending.sRepairDetailsId ?? ""));
                cmd.Parameters.AddWithValue("srepairerid", Convert.ToString(objTestPending.sRepairerId ?? ""));
                cmd.Parameters.AddWithValue("ssupplierid", Convert.ToString(objTestPending.sSupplierId ?? ""));
                cmd.Parameters.AddWithValue("spurchaseorderno", Convert.ToString(objTestPending.sPurchaseOrderNo ?? ""));
                cmd.Parameters.AddWithValue("scapacity", Convert.ToString(objTestPending.sCapacity ?? ""));
                cmd.Parameters.AddWithValue("smakeid", Convert.ToString(objTestPending.sMakeId ?? ""));
                dt = objCon.FetchDataTable(cmd);
                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        #region
        public string[] SaveTestingTCDetails(string[] strRepairDetailsIds, clsDTrRepairActivity objpending, DataTable dt)
        {
            string[] Arr = new string[2];
            string sFilePath = string.Empty;

            try
            {
                string[] strDetailVal = strRepairDetailsIds.ToArray();
                for (int i = 0; i < strDetailVal.Length; i++)
                {
                    if (strDetailVal[i] != null)
                    {
                        byte[] imageData = null;
                        string strQry = string.Empty;

                        NpgsqlParameter docPhoto = new NpgsqlParameter();
                        NpgsqlCommand comd = new NpgsqlCommand();
                        docPhoto.DbType = DbType.Binary;

                        for (int k = 0; k < dt.Rows.Count; k++)
                        {
                            if (dt.Rows[k][1].ToString() == strDetailVal[i].Split('~').GetValue(0).ToString())
                            {
                                imageData = (Byte[])dt.Rows[k][0];
                                if (imageData != null)
                                {
                                    Arr = new string[2];
                                    string indrsd_id = strDetailVal[i].Split('~').GetValue(0).ToString().ToUpper();
                                    string ind_inspresult = strDetailVal[i].Split('~').GetValue(1).ToString().ToUpper();
                                    string indremarks = strDetailVal[i].Split('~').GetValue(2).ToString().ToUpper();
                                    string FilePath = strDetailVal[i].Split('~').GetValue(3).ToString().ToUpper();
                                    docPhoto.ParameterName = "Document";
                                    docPhoto.Value = imageData;
                                    string sInspectionId = Convert.ToString(objCon.Get_max_no("IND_ID", "TBLINSPECTIONDETAILS"));
                                    NpgsqlCommand = new NpgsqlCommand("proc_savetestingtcdetailswhendocnotnull");
                                    NpgsqlCommand.Parameters.AddWithValue("sinspectionid", Convert.ToInt32(sInspectionId));
                                    NpgsqlCommand.Parameters.AddWithValue("indrsd_id", indrsd_id);
                                    NpgsqlCommand.Parameters.AddWithValue("stestedby", Convert.ToInt32(objpending.sTestedBy));
                                    NpgsqlCommand.Parameters.AddWithValue("stestedon", objpending.sTestedOn);
                                    NpgsqlCommand.Parameters.AddWithValue("stestlocation", Convert.ToString(objpending.sTestLocation));
                                    NpgsqlCommand.Parameters.AddWithValue("ind_inspresult", ind_inspresult);
                                    NpgsqlCommand.Parameters.AddWithValue("indremarks", indremarks);
                                    NpgsqlCommand.Parameters.AddWithValue("scrby", Convert.ToInt32(objpending.sCrby));
                                    //comd.Parameters.Add(docPhoto);
                                    //NpgsqlCommand.Parameters.AddWithValue("document", docPhoto.Value);
                                    NpgsqlCommand.Parameters.AddWithValue("document", Convert.ToString(FilePath ?? ""));
                                    NpgsqlCommand.Parameters.AddWithValue("sessionofccode", Convert.ToInt32(objpending.SessionOfcCode));

                                    NpgsqlCommand.Parameters.Add("op_id", NpgsqlDbType.Text);
                                    NpgsqlCommand.Parameters.Add("msg", NpgsqlDbType.Text);

                                    NpgsqlCommand.Parameters["op_id"].Direction = ParameterDirection.Output;
                                    NpgsqlCommand.Parameters["msg"].Direction = ParameterDirection.Output;

                                    Arr[0] = "op_id";
                                    Arr[1] = "msg";

                                    Arr = objCon.Execute(NpgsqlCommand, Arr, 0);

                                    // strQry = "INSERT INTO \"TBLINSPECTIONDETAILS\" (\"IND_ID\",\"IND_RSD_ID\",\"IND_INSP_BY\",\"IND_INSP_DATE\",\"IND_TEST_LOCATION\",\"IND_INSP_RESULT\",\"IND_REMARKS\",\"IND_CRBY\",\"IND_DOC\",\"IND_DIV_CODE\")";
                                    // strQry += " VALUES ('" + sInspectionId + "','" + strDetailVal[i].Split('~').GetValue(0).ToString().ToUpper() + "','" + objpending.sTestedBy + "',TO_DATE('" + objpending.sTestedOn + "','DD/MM/YYYY'),";
                                    // strQry += " '" + objpending.sTestLocation + "','" + strDetailVal[i].Split('~').GetValue(1).ToString().ToUpper() + "','" + strDetailVal[i].Split('~').GetValue(2).ToString().ToUpper() + "','" + objpending.sCrby + "',:Document,'" + objpending.SessionOfcCode + "')";

                                }
                                else
                                {
                                    Arr = new string[2];
                                    docPhoto.ParameterName = "Document";
                                    docPhoto.Value = null;
                                    string indrsd_id = strDetailVal[i].Split('~').GetValue(0).ToString().ToUpper();
                                    string ind_inspresult = strDetailVal[i].Split('~').GetValue(1).ToString().ToUpper();
                                    string indremarks = strDetailVal[i].Split('~').GetValue(2).ToString().ToUpper();
                                    string sInspectionId = Convert.ToString(objCon.Get_max_no("IND_ID", "TBLINSPECTIONDETAILS"));
                                    //  strQry = "INSERT INTO \"TBLINSPECTIONDETAILS\" (\"IND_ID\",\"IND_RSD_ID\",\"IND_INSP_BY\",\"IND_INSP_DATE\",\"IND_TEST_LOCATION\",\"IND_INSP_RESULT\",\"IND_REMARKS\",\"IND_CRBY\",\"IND_DIV_CODE\")";
                                    //  strQry += " VALUES ('" + sInspectionId + "','" + strDetailVal[i].Split('~').GetValue(0).ToString().ToUpper() + "','" + objpending.sTestedBy + "',TO_DATE('" + objpending.sTestedOn + "','DD/MM/YYYY'),";
                                    //  strQry += " '" + objpending.sTestLocation + "','" + strDetailVal[i].Split('~').GetValue(1).ToString().ToUpper() + "','" + strDetailVal[i].Split('~').GetValue(2).ToString().ToUpper() + "','" + objpending.sCrby + "','" + objpending.SessionOfcCode + "')";
                                    NpgsqlCommand = new NpgsqlCommand("proc_savetestingtcdetailswhendocnotnullnodocs");

                                    NpgsqlCommand.Parameters.AddWithValue("sinspectionid", Convert.ToInt32(sInspectionId));
                                    NpgsqlCommand.Parameters.AddWithValue("indrsd_id", indrsd_id);
                                    NpgsqlCommand.Parameters.AddWithValue("stestedby", Convert.ToInt32(objpending.sTestedBy));
                                    NpgsqlCommand.Parameters.AddWithValue("stestedon", objpending.sTestedOn);
                                    NpgsqlCommand.Parameters.AddWithValue("stestlocation", Convert.ToString(objpending.sTestLocation));

                                    NpgsqlCommand.Parameters.AddWithValue("ind_inspresult", ind_inspresult);
                                    NpgsqlCommand.Parameters.AddWithValue("indremarks", indremarks);
                                    NpgsqlCommand.Parameters.AddWithValue("scrby", Convert.ToInt32(objpending.sCrby));
                                    NpgsqlCommand.Parameters.AddWithValue("sessionofccode", Convert.ToInt32(objpending.SessionOfcCode));
                                    NpgsqlCommand.Parameters.Add("op_id", NpgsqlDbType.Text);
                                    NpgsqlCommand.Parameters.Add("msg", NpgsqlDbType.Text);

                                    NpgsqlCommand.Parameters["op_id"].Direction = ParameterDirection.Output;
                                    NpgsqlCommand.Parameters["msg"].Direction = ParameterDirection.Output;

                                    Arr[0] = "op_id";
                                    Arr[1] = "msg";

                                    Arr = objCon.Execute(NpgsqlCommand, Arr, 0);
                                }
                            }
                        }

                        if (docPhoto.Value == null)
                        {
                            Arr = new string[2];
                            docPhoto.ParameterName = "Document";
                            docPhoto.Value = null;
                            string indrsd_id = strDetailVal[i].Split('~').GetValue(0).ToString().ToUpper();
                            string ind_inspresult = strDetailVal[i].Split('~').GetValue(1).ToString().ToUpper();
                            string indremarks = strDetailVal[i].Split('~').GetValue(2).ToString().ToUpper();
                            string sInspectionId = Convert.ToString(objCon.Get_max_no("IND_ID", "TBLINSPECTIONDETAILS"));
                            NpgsqlCommand = new NpgsqlCommand("proc_savetestingtcdetailswhendocisnull");
                            NpgsqlCommand.Parameters.AddWithValue("sinspectionid", Convert.ToString(sInspectionId));
                            NpgsqlCommand.Parameters.AddWithValue("indrsd_id", indrsd_id);
                            NpgsqlCommand.Parameters.AddWithValue("stestedby", Convert.ToString(objpending.sTestedBy));
                            NpgsqlCommand.Parameters.AddWithValue("stestedon", objpending.sTestedOn);
                            NpgsqlCommand.Parameters.AddWithValue("stestlocation", Convert.ToString(objpending.sTestLocation));
                            NpgsqlCommand.Parameters.AddWithValue("ind_inspresult", ind_inspresult);
                            NpgsqlCommand.Parameters.AddWithValue("indremarks", indremarks);
                            NpgsqlCommand.Parameters.AddWithValue("scrby", Convert.ToInt32(objpending.sCrby));
                            NpgsqlCommand.Parameters.AddWithValue("sessionofccode", Convert.ToString(objpending.SessionOfcCode));
                            // strQry = "INSERT INTO \"TBLINSPECTIONDETAILS\" (\"IND_ID\",\"IND_RSD_ID\",\"IND_INSP_BY\",\"IND_INSP_DATE\",\"IND_TEST_LOCATION\",\"IND_INSP_RESULT\",\"IND_REMARKS\",\"IND_CRBY\",\"IND_DIV_CODE\")";
                            //  strQry += " VALUES ('" + sInspectionId + "','" + strDetailVal[i].Split('~').GetValue(0).ToString().ToUpper() + "','" + objpending.sTestedBy + "',TO_DATE('" + objpending.sTestedOn + "','DD/MM/YYYY'),";
                            // strQry += " '" + objpending.sTestLocation + "','" + strDetailVal[i].Split('~').GetValue(1).ToString().ToUpper() + "','" + strDetailVal[i].Split('~').GetValue(2).ToString().ToUpper() + "','" + objpending.sCrby + "','" + objpending.SessionOfcCode + "')";

                            NpgsqlCommand.Parameters.Add("op_id", NpgsqlDbType.Text);
                            NpgsqlCommand.Parameters.Add("msg", NpgsqlDbType.Text);

                            NpgsqlCommand.Parameters["op_id"].Direction = ParameterDirection.Output;
                            NpgsqlCommand.Parameters["msg"].Direction = ParameterDirection.Output;

                            Arr[0] = "op_id";
                            Arr[1] = "msg";

                            Arr = objCon.Execute(NpgsqlCommand, Arr, 0);
                        }

                        //NpgsqlConnection objconn = new NpgsqlConnection();
                        //string strConnectionString = ConfigurationManager.ConnectionStrings["pgSQL"].ConnectionString + "Idea@12345+";
                        //objconn.ConnectionString = strConnectionString; // "Server=localhost;Port=5432;Database=TestTRM;User Id=postgres;Password =Idea123";  
                        //objconn.Open();
                        //comd = new NpgsqlCommand(strQry,objconn);
                        //if (docPhoto.Value != null)
                        //{
                        //    comd.Parameters.Add(docPhoto);
                        //}

                        //comd.ExecuteNonQuery();
                        //objconn.Close();
                        Arr = new string[2];
                        Arr[0] = "Testing Done Successfully";
                        Arr[1] = "0";

                    }
                }


                return Arr;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }
        #endregion

        public clsDTrRepairActivity LoadTestedDTR(clsDTrRepairActivity objTestPending)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = " SELECT \"RSM_PO_NO\",TO_CHAR(\"RSM_ISSUE_DATE\",'DD-MON-YYYY') RSM_ISSUE_DATE, \"RSD_ID\", \"TC_CODE\",\"TC_SLNO\",";
                //strQry += " (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\" =  \"TC_MAKE_ID\") TM_NAME, ";
                //strQry += " CAST(\"TC_CAPACITY\" AS TEXT) AS CAPACITY, TO_CHAR(\"TC_MANF_DATE\",'DD-MON-YYYY') TC_MANF_DATE,";
                //strQry += " (CASE WHEN \"RSM_SUPREP_TYPE\"='2' THEN (SELECT \"TR_NAME\" FROM \"TBLTRANSREPAIRER\" TR WHERE TR.\"TR_ID\"=\"RSM_SUPREP_ID\") WHEN ";
                //strQry += " \"RSM_SUPREP_TYPE\"='1' THEN (SELECT \"TS_NAME\" FROM \"TBLTRANSSUPPLIER\"  WHERE \"TS_ID\"=\"RSM_SUPREP_ID\") END ) SUP_REPNAME, ";
                //strQry += " \"IND_INSP_BY\",TO_CHAR(\"IND_INSP_DATE\",'DD-MON-YYYY') IND_INSP_DATE,\"IND_TEST_LOCATION\",\"IND_INSP_RESULT\",\"IND_REMARKS\"";
                //strQry += " FROM \"TBLREPAIRSENTMASTER\", \"TBLREPAIRSENTDETAILS\",\"TBLTCMASTER\",\"TBLINSPECTIONDETAILS\" WHERE  \"RSM_ID\" = \"RSD_RSM_ID\" AND ";
                //strQry += " \"RSD_TC_CODE\" = \"TC_CODE\" AND \"IND_INSP_RESULT\" = 1 AND \"IND_RSD_ID\"=\"RSD_ID\" AND \"TC_CODE\"=:sTcCode";
                //strQry += " AND \"IND_ID\" =:sTestInspectionId";
                //NpgsqlCommand.Parameters.AddWithValue("sTcCode", Convert.ToDouble(objTestPending.sTcCode));
                //NpgsqlCommand.Parameters.AddWithValue("sTestInspectionId", Convert.ToInt32(objTestPending.sTestInspectionId));
                //dt = objCon.FetchDataTable(strQry, NpgsqlCommand);
                NpgsqlCommand cmd = new NpgsqlCommand("proc_loadtesteddtr");
                cmd.Parameters.AddWithValue("stccode", Convert.ToDouble(objTestPending.sTcCode));
                cmd.Parameters.AddWithValue("stestinspectionid", Convert.ToInt32(objTestPending.sTestInspectionId));
                dt = objCon.FetchDataTable(cmd);
                if (dt.Rows.Count > 0)
                {
                    objTestPending.dtTestDone = dt;
                    objTestPending.sInspRemarks = Convert.ToString(dt.Rows[0]["IND_REMARKS"]);
                    objTestPending.sTestedBy = Convert.ToString(dt.Rows[0]["IND_INSP_BY"]);
                    objTestPending.sTestedOn = Convert.ToString(dt.Rows[0]["IND_INSP_DATE"]);
                    objTestPending.sTestLocation = Convert.ToString(dt.Rows[0]["IND_TEST_LOCATION"]);
                    objTestPending.sTestResult = Convert.ToString(dt.Rows[0]["IND_INSP_RESULT"]);
                }
                return objTestPending;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objTestPending;
            }
        }

        #endregion

        #region Deliver DTR / Recieve DTR


        public DataTable LoadTestingPassedDetails(clsDTrRepairActivity objTestPending)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                string id = Convert.ToString(ConfigurationManager.AppSettings["SupAdminUserId"]);
                #region Old inline query
                //strQry = " SELECT \"RSM_PO_NO\", \"RSM_ID\", TO_CHAR(\"RSM_PO_DATE\",'DD-MON-YY') AS PODATE, TO_CHAR(\"RSM_ISSUE_DATE\",'DD-MON-YY') AS ISSUEDATE,";
                //strQry += " (CASE WHEN \"RSM_SUPREP_TYPE\"='2' THEN (SELECT \"TR_NAME\" FROM \"TBLTRANSREPAIRER\" TR WHERE TR.\"TR_ID\"=\"RSM_SUPREP_ID\") WHEN ";
                //strQry += " \"RSM_SUPREP_TYPE\"='1' THEN (SELECT \"TS_NAME\" FROM \"TBLTRANSSUPPLIER\" WHERE \"TS_ID\"=\"RSM_SUPREP_ID\") END ) SUP_REPNAME,";
                //strQry += " \"RSM_PO_QNTY\" PO_QUANTITY, SUM(CASE WHEN \"RSD_DELIVARY_DATE\" IS NULL THEN 1 ELSE 0 END) PENDING_QNTY,";
                //strQry += " SUM(CASE WHEN \"RSD_DELIVARY_DATE\" IS NOT NULL THEN 1 ELSE 0 END) DELIVERED_QNTY";
                //strQry += " FROM \"TBLREPAIRSENTMASTER\", \"TBLREPAIRSENTDETAILS\" WHERE \"RSM_ID\" = \"RSD_RSM_ID\" AND \"RSD_ID\" IN ";  //LIKE :sOfficeCode||'%'

                //if (objTestPending.UserId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminUserId"]))
                //{
                //    strQry += " (SELECT \"IND_RSD_ID\" FROM \"TBLINSPECTIONDETAILS\" WHERE \"IND_INSP_RESULT\" IN(1,3,4)) AND CAST(\"RSM_DIV_CODE\" AS TEXT) LIKE '" + clsStoreOffice.GetStoreID(objTestPending.sOfficeCode) + "%'";
                //}
                //else
                //{
                //    strQry += " (SELECT \"IND_RSD_ID\" FROM \"TBLINSPECTIONDETAILS\" WHERE \"IND_INSP_RESULT\" IN(1,3,4)) AND CAST(\"RSM_DIV_CODE\" AS TEXT) LIKE '" + objTestPending.sOfficeCode + "%'";
                //}
                //// NpgsqlCommand = new NpgsqlCommand();
                ////  NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", objTestPending.sOfficeCode);

                //if (objTestPending.sRepairerId != null)
                //{
                //    strQry += " AND \"RSM_SUPREP_ID\" ='" + objTestPending.sRepairerId.ToString().ToUpper() + "' AND \"RSM_SUPREP_TYPE\" ='2'";
                //    // NpgsqlCommand = new NpgsqlCommand();
                //    // NpgsqlCommand.Parameters.AddWithValue("sRepairerId", objTestPending.sRepairerId.ToString().ToUpper());
                //}

                //if (objTestPending.sSupplierId != null)
                //{
                //    strQry += " AND \"RSM_SUPREP_ID\" ='" + objTestPending.sSupplierId.ToString().ToUpper() + "' AND \"RSM_SUPREP_TYPE\" ='1'";
                //    // NpgsqlCommand = new NpgsqlCommand();
                //    //NpgsqlCommand.Parameters.AddWithValue("sSupplierId", objTestPending.sSupplierId.ToString().ToUpper());

                //}
                //if (objTestPending.sPurchaseOrderNo.Trim() != "")
                //{
                //    strQry += " AND  UPPER(\"RSM_PO_NO\") ='" + objTestPending.sPurchaseOrderNo.ToString().ToUpper() + "' ";
                //    //NpgsqlCommand = new NpgsqlCommand();
                //    // NpgsqlCommand.Parameters.AddWithValue("sPurchaseOrderNo", objTestPending.sPurchaseOrderNo.ToString().ToUpper());

                //}

                //strQry += " GROUP BY \"RSM_PO_NO\", \"RSM_PO_QNTY\", TO_CHAR(\"RSM_PO_DATE\",'DD-MON-YY'),";
                //strQry += " TO_CHAR(\"RSM_ISSUE_DATE\",'DD-MON-YY'),\"RSM_SUPREP_TYPE\",\"RSM_SUPREP_ID\",\"RSM_ID\"";
                //dt = objCon.FetchDataTable(strQry);
                //return dt;
                #endregion
                NpgsqlCommand = new NpgsqlCommand("proc_loadtestingpasseddetails");
                NpgsqlCommand.Parameters.AddWithValue("offcode", Convert.ToString(objTestPending.sOfficeCode ?? ""));
                NpgsqlCommand.Parameters.AddWithValue("userid", Convert.ToString(objTestPending.UserId ?? ""));
                NpgsqlCommand.Parameters.AddWithValue("srepairerid", Convert.ToString(objTestPending.sRepairerId ?? ""));
                NpgsqlCommand.Parameters.AddWithValue("ssupplierid", Convert.ToString(objTestPending.sSupplierId ?? ""));
                NpgsqlCommand.Parameters.AddWithValue("spurchaseorderno", Convert.ToString(objTestPending.sPurchaseOrderNo ?? ""));
                dt = objCon.FetchDataTable(NpgsqlCommand);
                return dt;
            }
            catch (Exception ex)
            {
                objCon.RollBackTrans();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable LoadPendingForTestingDetails(clsDTrRepairActivity objTestPending)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                #region  old qury

                // strQry = "SELECT \"RSM_PO_NO\",  TO_CHAR(\"RSM_PO_DATE\",'DD-MON-YY') AS PODATE, TO_CHAR(\"RSM_ISSUE_DATE\",'DD-MON-YY') AS ISSUEDATE,";
                // strQry += " (CASE WHEN \"RSM_SUPREP_TYPE\"='2' THEN (SELECT \"TR_NAME\" FROM \"TBLTRANSREPAIRER\" TR WHERE TR.\"TR_ID\"=\"RSM_SUPREP_ID\") WHEN ";
                // strQry += " \"RSM_SUPREP_TYPE\"='1' THEN (SELECT \"TS_NAME\" FROM \"TBLTRANSSUPPLIER\"  WHERE \"TS_ID\"=\"RSM_SUPREP_ID\") END ) SUP_REPNAME,";
                // strQry += " \"RSM_PO_QNTY\" PO_QUANTITY, SUM(CASE WHEN \"RSD_DELIVARY_DATE\" IS NULL THEN 1 ELSE 0 END) PENDING_QNTY,";
                // strQry += " SUM(CASE WHEN \"RSD_DELIVARY_DATE\" IS NOT NULL THEN 1 ELSE 0 END) DELIVERED_QNTY";
                // strQry += " FROM \"TBLREPAIRSENTMASTER\", \"TBLREPAIRSENTDETAILS\" WHERE \"RSM_ID\"= \"RSD_RSM_ID\" AND \"RSD_ID\" NOT IN ";
                // strQry += " (SELECT \"IND_RSD_ID\" FROM \"TBLINSPECTIONDETAILS\" WHERE \"IND_INSP_RESULT\" IN (1,3)) AND CAST(\"RSM_DIV_CODE\" AS TEXT) LIKE '" + clsStoreOffice.GetStoreID(objTestPending.sOfficeCode) +"%'";
                //// NpgsqlCommand = new NpgsqlCommand();
                //// NpgsqlCommand.Parameters.AddWithValue("sStoreId", clsStoreOffice.GetStoreID(objTestPending.sOfficeCode));

                // if (objTestPending.sRepairerId != null)
                // {
                //     strQry += " AND \"RSM_SUPREP_ID\" ='"+ objTestPending.sRepairerId.ToString().ToUpper() + "' AND \"RSM_SUPREP_TYPE\" ='2'";
                //   //  NpgsqlCommand = new NpgsqlCommand();
                //    // NpgsqlCommand.Parameters.AddWithValue("sRepairerId", objTestPending.sRepairerId.ToString().ToUpper());

                // }

                // if (objTestPending.sSupplierId != null)
                // {
                //     strQry += " AND \"RSM_SUPREP_ID\" ='"+ objTestPending.sSupplierId.ToString().ToUpper() + "' AND \"RSM_SUPREP_TYPE\" ='1'";
                //    // NpgsqlCommand = new NpgsqlCommand();
                //    // NpgsqlCommand.Parameters.AddWithValue("sSupplierId", objTestPending.sSupplierId.ToString().ToUpper());


                // }
                // if (objTestPending.sPurchaseOrderNo.Trim() != "")
                // {
                //     strQry += " AND UPPER(\"RSM_PO_NO\")='" + objTestPending.sPurchaseOrderNo.ToString().ToUpper() + "'";
                //     //NpgsqlCommand = new NpgsqlCommand();
                //    // NpgsqlCommand.Parameters.AddWithValue("sPurchaseOrderNo", objTestPending.sPurchaseOrderNo.ToString().ToUpper());

                // }

                // strQry += " GROUP BY \"RSM_PO_NO\", \"RSM_PO_QNTY\", TO_CHAR(\"RSM_PO_DATE\",'DD-MON-YY'),";
                // strQry += " TO_CHAR(\"RSM_ISSUE_DATE\",'DD-MON-YY'),\"RSM_SUPREP_TYPE\",\"RSM_SUPREP_ID\"";
                #endregion
                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_faultytc_pending_totest");
                cmd.Parameters.AddWithValue("soffice_code", objTestPending.sOfficeCode.Trim() == null ? "" : objTestPending.sOfficeCode.Trim());
                cmd.Parameters.AddWithValue("rep_id", objTestPending.sRepairerId == null ? "" : objTestPending.sRepairerId);
                cmd.Parameters.AddWithValue("supplier_id", objTestPending.sSupplierId == null ? "" : objTestPending.sSupplierId);
                cmd.Parameters.AddWithValue("po_no", objTestPending.sPurchaseOrderNo == null ? "" : objTestPending.sPurchaseOrderNo);

                dt = objCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                objCon.RollBackTrans();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }


        public DataTable LoadPendingForRecieve(string sRepairMasterId)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;

                //strQry = " SELECT CASE WHEN NOW() < \"TC_WARANTY_PERIOD\" THEN '1' ELSE '' END STATUS, CASE WHEN NOW() < \"TC_WARANTY_PERIOD\" ";
                //strQry += "THEN (SELECT \"RSD_GUARRENTY_TYPE\" FROM \"TBLREPAIRSENTDETAILS\",\"TBLREPAIRSENTMASTER\" WHERE \"RSD_TC_CODE\"=\"TC_CODE\" and \"RSD_RSM_ID\"=\"RSM_ID\" and \"RSM_ID\" ='" + sRepairMasterId + "' AND \"RSD_GUARRENTY_TYPE\" IS NOT NULL) ";
                //strQry += " ELSE '' END WARRENTY_TYPE ,TO_CHAR(\"TC_WARANTY_PERIOD\",'DD-MON-YYYY')TC_WARANTY_PERIOD,\"TC_WARRENTY\", ";
                //strQry += " \"RSM_PO_NO\",TO_CHAR(\"RSM_ISSUE_DATE\",'DD-MON-YYYY') \"RSM_ISSUE_DATE\",TO_CHAR(\"RSM_PO_DATE\",'DD-MON-YYYY') RSM_PO_DATE , \"RSD_ID\", CAST(\"TC_CODE\" AS TEXT) TC_CODE,\"TC_SLNO\", ";
                //strQry += "(SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\" =  \"TC_MAKE_ID\") MAKE, (SELECT \"IND_DOC\" FROM \"TBLINSPECTIONDETAILS\" ";
                //strQry += " WHERE \"RSD_TC_CODE\"= \"TC_CODE\" and \"RSD_ID\"=\"IND_RSD_ID\" AND \"IND_INSP_RESULT\" IN(1,3))IND_DOC, CAST(\"TC_CAPACITY\" AS TEXT) AS ";
                //strQry += " CAPACITY, TO_CHAR(\"TC_MANF_DATE\",'DD-MON-YYYY') TC_MANF_DATE,(CASE WHEN \"RSM_SUPREP_TYPE\" ='2' THEN (SELECT \"TR_NAME\" ";
                //strQry += "  FROM \"TBLTRANSREPAIRER\" TR WHERE TR.\"TR_ID\"=\"RSM_SUPREP_ID\") WHEN \"RSM_SUPREP_TYPE\"='1' THEN (SELECT \"TS_NAME\" FROM ";
                //strQry += "  \"TBLTRANSSUPPLIER\"  WHERE \"TS_ID\"=\"RSM_SUPREP_ID\") END ) SUP_REPNAME, (SELECT  CASE \"IND_INSP_RESULT\" WHEN 1 THEN 'PASS'";
                //strQry += " WHEN 3 THEN 'SCRAP' WHEN 4 THEN 'NONE' END AS STATE  FROM \"TBLINSPECTIONDETAILS\" WHERE \"IND_RSD_ID\"=\"RSD_ID\" AND \"IND_INSP_RESULT\" IN(1,3,4) )STATE ";
                //strQry += " FROM \"TBLREPAIRSENTMASTER\", \"TBLREPAIRSENTDETAILS\",\"TBLTCMASTER\" WHERE \"RSM_ID\" ='" + sRepairMasterId + "' AND \"RSM_ID\" = \"RSD_RSM_ID\" AND \"RSD_TC_CODE\" = \"TC_CODE\"";
                //strQry += " AND \"RSD_DELIVARY_DATE\" IS NULL AND \"RSD_ID\"  IN ";
                //strQry += " (SELECT \"IND_RSD_ID\" FROM \"TBLINSPECTIONDETAILS\" WHERE \"IND_INSP_RESULT\" IN(1,3,4))";
                //// NpgsqlCommand = new NpgsqlCommand();
                //// NpgsqlCommand.Parameters.AddWithValue("sRepairMasterId", Convert.ToInt32(sRepairMasterId));
                //dt = objCon.FetchDataTable(strQry);
                NpgsqlCommand cmd = new NpgsqlCommand("proc_loadpendingforrecievers");
                cmd.Parameters.AddWithValue("rsmid", Convert.ToInt32(sRepairMasterId));
                dt = objCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                objCon.RollBackTrans();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        //public DataTable LoadPendingForRecieve(string sRepairMasterId)
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        string strQry = string.Empty;

        //        strQry = " SELECT CASE WHEN NOW() < \"TC_WARANTY_PERIOD\" THEN '1' ELSE '' END STATUS, CASE WHEN NOW() < \"TC_WARANTY_PERIOD\" ";
        //        strQry += "THEN (SELECT \"RSD_GUARRENTY_TYPE\" FROM \"TBLREPAIRSENTDETAILS\" WHERE \"RSD_TC_CODE\"=\"TC_CODE\" AND \"RSD_GUARRENTY_TYPE\" IS NOT NULL) ";
        //        strQry += " ELSE '' END WARRENTY_TYPE ,TO_CHAR(\"TC_WARANTY_PERIOD\",'DD-MON-YYYY')TC_WARANTY_PERIOD,\"TC_WARRENTY\", ";
        //        strQry += " \"RSM_PO_NO\",TO_CHAR(\"RSM_ISSUE_DATE\",'DD-MON-YYYY') \"RSM_ISSUE_DATE\", \"RSD_ID\", CAST(\"TC_CODE\" AS TEXT) TC_CODE,\"TC_SLNO\", ";
        //        strQry += "(SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\" =  \"TC_MAKE_ID\") MAKE, (SELECT \"IND_DOC\" FROM \"TBLINSPECTIONDETAILS\" ";
        //        strQry += " WHERE \"RSD_TC_CODE\"= \"TC_CODE\" and \"RSD_ID\"=\"IND_RSD_ID\" AND \"IND_INSP_RESULT\" IN(1,3))IND_DOC, CAST(\"TC_CAPACITY\" AS TEXT) AS ";
        //        strQry += " CAPACITY, TO_CHAR(\"TC_MANF_DATE\",'DD-MON-YYYY') TC_MANF_DATE,(CASE WHEN \"RSM_SUPREP_TYPE\" ='2' THEN (SELECT \"TR_NAME\" ";
        //        strQry += "  FROM \"TBLTRANSREPAIRER\" TR WHERE TR.\"TR_ID\"=\"RSM_SUPREP_ID\") WHEN \"RSM_SUPREP_TYPE\"='1' THEN (SELECT \"TS_NAME\" FROM ";
        //        strQry += "  \"TBLTRANSSUPPLIER\"  WHERE \"TS_ID\"=\"RSM_SUPREP_ID\") END ) SUP_REPNAME, (SELECT  CASE \"IND_INSP_RESULT\" WHEN 1 THEN 'PASS'";
        //        strQry += " WHEN 3 THEN 'SCRAP' WHEN 4 THEN 'NONE' END AS STATE  FROM \"TBLINSPECTIONDETAILS\" WHERE \"IND_RSD_ID\"=\"RSD_ID\" AND \"IND_INSP_RESULT\" IN(1,3,4) )STATE ";
        //        strQry += " FROM \"TBLREPAIRSENTMASTER\", \"TBLREPAIRSENTDETAILS\",\"TBLTCMASTER\" WHERE \"RSM_ID\" ='"+ sRepairMasterId + "' AND \"RSM_ID\" = \"RSD_RSM_ID\" AND \"RSD_TC_CODE\" = \"TC_CODE\"";
        //        strQry += " AND \"RSD_DELIVARY_DATE\" IS NULL AND \"RSD_ID\"  IN ";
        //        strQry += " (SELECT \"IND_RSD_ID\" FROM \"TBLINSPECTIONDETAILS\" WHERE \"IND_INSP_RESULT\" IN(1,3,4))";
        //       // NpgsqlCommand = new NpgsqlCommand();
        //       // NpgsqlCommand.Parameters.AddWithValue("sRepairMasterId", Convert.ToInt32(sRepairMasterId));
        //        dt = objCon.FetchDataTable(strQry);

        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        objCon.RollBackTrans();
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return dt;
        //    }
        //}

        //public string getWarentyStatus(string SDtrCode)
        //{
        //    string sWarentyStatus = string.Empty;
        //    string strQry = string.Empty;
        //    try
        //    {
        //        strQry = "SELECT DISTINCT STATUS FROM (SELECT CASE WHEN SYSDATE < TC_WARANTY_PERIOD THEN (SELECT DISTINCT RSD_WARENTY_PERIOD";
        //        strQry += " FROM TBLREPAIRSENTDETAILS WHERE RSD_WARENTY_PERIOD IS NOT NULL AND RSD_TC_CODE='"+ SDtrCode + "') ELSE '' END ";
        //        strQry += "STATUS FROM TBLTCMASTER,TBLREPAIRSENTDETAILS WHERE TC_CODE=RSD_TC_CODE AND TC_CODE='" + SDtrCode + "')";
        //        sWarentyStatus = objCon.get_value(strQry);
        //        return sWarentyStatus;
        //    }
        //    catch(Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "getWarentyStatus");
        //        return sWarentyStatus;
        //    }
        //}

        public DataTable LoadRecievedTransformers(string sRepairMasterId)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;

                //strQry = " SELECT \"RSM_PO_NO\", TO_CHAR(\"RSM_ISSUE_DATE\",'DD-MON-YYYY') RSM_ISSUE_DATE, \"RSD_ID\", \"TC_CODE\",\"TC_SLNO\", (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\" =  \"TC_MAKE_ID\") MAKE, ";
                //strQry += " CAST(\"TC_CAPACITY\" AS TEXT) AS CAPACITY, TO_CHAR(\"TC_MANF_DATE\",'DD-MON-YYYY') TC_MANF_DATE,";
                //strQry += " (CASE WHEN \"RSM_SUPREP_TYPE\"='2' THEN (SELECT \"TR_NAME\" FROM \"TBLTRANSREPAIRER\" TR WHERE TR.\"TR_ID\"=\"RSM_SUPREP_ID\") WHEN ";
                //strQry += " \"RSM_SUPREP_TYPE\"='1' THEN (SELECT \"TS_NAME\" FROM \"TBLTRANSSUPPLIER\"  WHERE \"TS_ID\"=\"RSM_SUPREP_ID\") END ) SUP_REPNAME, ";
                //strQry += " CASE \"IND_INSP_RESULT\" WHEN 1 THEN 'PASS' WHEN 3 THEN 'SCRAP' END AS STATE,\"IND_INSP_RESULT\" ";
                //strQry += " FROM \"TBLREPAIRSENTMASTER\", \"TBLREPAIRSENTDETAILS\",\"TBLINSPECTIONDETAILS\", \"TBLTCMASTER\" WHERE \"RSM_ID\" =:sRepairMasterId AND \"RSM_ID\" = \"RSD_RSM_ID\" AND \"RSD_TC_CODE\" = \"TC_CODE\"";
                //strQry += " AND \"RSD_DELIVARY_DATE\" IS NOT NULL AND \"RSD_ID\"=\"IND_RSD_ID\"  and \"IND_INSP_RESULT\"='1' ";
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sRepairMasterId", Convert.ToInt32(sRepairMasterId));
                //dt = objCon.FetchDataTable(strQry, NpgsqlCommand);
                //return dt;
                NpgsqlCommand = new NpgsqlCommand("proc_loadrecievedtransformers");
                NpgsqlCommand.Parameters.AddWithValue("rsmid", Convert.ToInt32(sRepairMasterId));
                dt = objCon.FetchDataTable(NpgsqlCommand);
                return dt;
            }
            catch (Exception ex)
            {
                objCon.RollBackTrans();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }


        //public string[] SaveDeliverTCDetails(string[] sRepairDetailsId, clsDTrRepairActivity objDeliverpending)
        //{
        //    string[] Arr = new string[2];
        //    try
        //    {
        //        objCon.BeginTransaction();

        //        string[] strDetailVal = sRepairDetailsId.ToArray();
        //        for (int i = 0; i < strDetailVal.Length; i++)
        //        {
        //            if (strDetailVal[i] != null)
        //            {
        //                string strQry = string.Empty;

        //                strQry = "SELECT DISTINCT STATUS FROM (SELECT CASE WHEN NOW() < \"TC_WARANTY_PERIOD\" THEN (SELECT DISTINCT ";
        //                strQry += " CAST(\"RSD_WARENTY_PERIOD\" AS TEXT) FROM \"TBLREPAIRSENTDETAILS\" WHERE \"RSD_WARENTY_PERIOD\" IS NOT NULL AND ";
        //                strQry += " \"RSD_TC_CODE\" ='" + strDetailVal[i].Split('~').GetValue(1).ToString() + "') ELSE '' END STATUS FROM \"TBLTCMASTER\",";
        //                strQry += " \"TBLREPAIRSENTDETAILS\" WHERE \"TC_CODE\"=\"RSD_TC_CODE\" AND \"TC_CODE\" ='" + strDetailVal[i].Split('~').GetValue(1).ToString() + "') A";
        //                string res = objCon.get_value(strQry);
        //                if (res == "" || res == null)
        //                {
        //                    strQry = "Update \"TBLREPAIRSENTDETAILS\" SET ";
        //                    strQry += "\"RSD_DELIVARY_DATE\"=to_date('" + objDeliverpending.sDeliverDate + "','DD/MM/YYYY'),";
        //                    strQry += "\"RSD_DELIVER_VER_BY\"='" + objDeliverpending.sVerifiedby.Trim().ToUpper() + "',";
        //                    strQry += "\"RSD_DELIVER_LOCATION\"='" + objDeliverpending.sStoreId + "',";
        //                    strQry += "\"RSD_DELIVER_CHALLEN_NO\"='" + objDeliverpending.sDeliverChallenNo.Trim().ToUpper() + "', ";
        //                    strQry += " \"RSD_RV_NO\"='" + objDeliverpending.sRVNo + "', \"RSD_RV_DATE\" =to_date('" + objDeliverpending.sRVDate + "','DD/MM/YYYY')";
        //                    strQry += ", \"RSD_WARENTY_PERIOD\" ='" + strDetailVal[i].Split('~').GetValue(3).ToString() + "', \"RSD_GUARRENTY_TYPE\" ='" + strDetailVal[i].Split('~').GetValue(4).ToString() + "'";
        //                    strQry += "  WHERE \"RSD_ID\" ='" + strDetailVal[i].Split('~').GetValue(0).ToString() + "'";
        //                    objCon.ExecuteQry(strQry);
        //                }
        //                else
        //                {
        //                    strQry = "Update \"TBLREPAIRSENTDETAILS\" SET ";
        //                    strQry += "\"RSD_DELIVARY_DATE\" =to_date('" + objDeliverpending.sDeliverDate + "','DD/MM/YYYY'),";
        //                    strQry += "\"RSD_DELIVER_VER_BY\" ='" + objDeliverpending.sVerifiedby.Trim().ToUpper() + "',";
        //                    strQry += "\"RSD_DELIVER_LOCATION\" ='" + objDeliverpending.sStoreId + "',";
        //                    strQry += "\"RSD_DELIVER_CHALLEN_NO\" ='" + objDeliverpending.sDeliverChallenNo.Trim().ToUpper() + "', ";
        //                    strQry += " \"RSD_RV_NO\" ='" + objDeliverpending.sRVNo + "', \"RSD_RV_DATE\" =to_date('" + objDeliverpending.sRVDate + "','DD/MM/YYYY')";
        //                    //strQry += ",RSD_WARENTY_PERIOD='"+ objDeliverpending.sWarrantyPeriod +"',RSD_GUARRENTY_TYPE='"+ objDeliverpending.sGuarantyType +"'";
        //                    strQry += " where \"RSD_ID\" ='" + strDetailVal[i].Split('~').GetValue(0).ToString() + "'";
        //                    objCon.ExecuteQry(strQry);
        //                }

        //                if (strDetailVal[i].Split('~').GetValue(2).ToString() == "1")
        //                {
        //                    strQry = "UPDATE \"TBLTCMASTER\" SET \"TC_CURRENT_LOCATION\"=1,\"TC_STORE_ID\"='" + objDeliverpending.sStoreId + "', ";
        //                    strQry += " \"TC_STATUS\"=2,\"TC_UPDATED_EVENT\"='DELIVER TC',\"TC_UPDATED_EVENT_ID\" ='" + strDetailVal[i].Split('~').GetValue(0).ToString() + "' ";
        //                    strQry += " WHERE \"TC_CODE\"='" + strDetailVal[i].Split('~').GetValue(1).ToString() + "'";
        //                    objCon.ExecuteQry(strQry);
        //                }
        //                else if (strDetailVal[i].Split('~').GetValue(2).ToString() == "3")
        //                {
        //                    strQry = "UPDATE \"TBLTCMASTER\" SET \"TC_CURRENT_LOCATION\"=1, \"TC_STORE_ID\" ='" + objDeliverpending.sStoreId + "', ";
        //                    strQry += " \"TC_STATUS\" =6, \"TC_UPDATED_EVENT\" ='DELIVER TC', \"TC_UPDATED_EVENT_ID\" ='" + strDetailVal[i].Split('~').GetValue(0).ToString() + "' ";
        //                    strQry += " WHERE \"TC_CODE\" ='" + strDetailVal[i].Split('~').GetValue(1).ToString() + "'";
        //                    objCon.ExecuteQry(strQry);
        //                }
        //                else if (strDetailVal[i].Split('~').GetValue(2).ToString() == "4")
        //                {
        //                    strQry = "UPDATE \"TBLTCMASTER\" SET \"TC_CURRENT_LOCATION\" =1, \"TC_STORE_ID\" ='" + Convert.ToInt32(objDeliverpending.sStoreId) + "', ";
        //                    strQry += " \"TC_STATUS\"=3,\"TC_UPDATED_EVENT\"='DELIVER TC',\"TC_UPDATED_EVENT_ID\"='" + strDetailVal[i].Split('~').GetValue(0).ToString() + "'";
        //                    strQry += " WHERE \"TC_CODE\"='" + strDetailVal[i].Split('~').GetValue(1).ToString() + "'";
        //                   // NpgsqlCommand = new NpgsqlCommand();
        //                   // NpgsqlCommand.Parameters.AddWithValue("sStoreId", Convert.ToInt32(objDeliverpending.sStoreId));
        //                   // NpgsqlCommand.Parameters.AddWithValue("strDetailVal", strDetailVal[i].Split('~').GetValue(0).ToString());
        //                   // NpgsqlCommand.Parameters.AddWithValue("strDetailVal1", strDetailVal[i].Split('~').GetValue(1).ToString());

        //                    objCon.ExecuteQry(strQry);
        //                }


        //                Arr[0] = "Repaired Transformer Successfully Recieved in Store";
        //                Arr[1] = "0";
        //            }

        //        }

        //        objCon.CommitTransaction();
        //        return Arr;
        //    }
        //    catch (Exception ex)
        //    {
        //        objCon.RollBackTrans();
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return Arr;
        //    }
        //}

        #endregion
        //11-09-2020

        public string[] SaveDeliverTCDetails(string[] sRepairDetailsId, clsDTrRepairActivity objDeliverpending)
        {


            string[] Arr = new string[2];
            string[] Arr1 = new string[2];

            try
            {
                objCon.BeginTransaction();

                string[] strDetailVal = sRepairDetailsId.ToArray();
                for (int i = 0; i < strDetailVal.Length; i++)
                {
                    if (strDetailVal[i] != null)
                    {
                        //string strQry = string.Empty;

                        //strQry = "SELECT DISTINCT STATUS FROM (SELECT CASE WHEN NOW() < \"TC_WARANTY_PERIOD\" THEN (SELECT DISTINCT ";
                        //strQry += " CAST(\"RSD_WARENTY_PERIOD\" AS TEXT) FROM \"TBLREPAIRSENTDETAILS\" WHERE \"RSD_WARENTY_PERIOD\" IS NOT NULL AND ";
                        //strQry += " \"RSD_TC_CODE\" ='" + strDetailVal[i].Split('~').GetValue(1).ToString() + "' limit 1 ) ELSE '' END STATUS FROM \"TBLTCMASTER\",";
                        //// strQry += " \"RSD_TC_CODE\" ='" + strDetailVal[i].Split('~').GetValue(1).ToString() + "') ELSE '' END STATUS FROM \"TBLTCMASTER\",";
                        //strQry += " \"TBLREPAIRSENTDETAILS\" WHERE \"TC_CODE\"=\"RSD_TC_CODE\" AND \"TC_CODE\" ='" + strDetailVal[i].Split('~').GetValue(1).ToString() + "') A";
                        //string res = objCon.get_value(strQry);

                        //if (res == "" || res == null)
                        //{
                        //    strQry = "Update \"TBLREPAIRSENTDETAILS\" SET ";
                        //    strQry += "\"RSD_DELIVARY_DATE\"=to_date('" + objDeliverpending.sDeliverDate + "','DD/MM/YYYY'),";
                        //    strQry += "\"RSD_DELIVER_VER_BY\"='" + objDeliverpending.sVerifiedby.Trim().ToUpper() + "',";
                        //    strQry += "\"RSD_DELIVER_LOCATION\"='" + objDeliverpending.sStoreId + "',";
                        //    strQry += "\"RSD_DELIVER_CHALLEN_NO\"='" + objDeliverpending.sDeliverChallenNo.Trim().ToUpper() + "', ";
                        //    strQry += " \"RSD_RV_NO\"='" + objDeliverpending.sRVNo + "', \"RSD_RV_DATE\" =to_date('" + objDeliverpending.sRVDate + "','DD/MM/YYYY'),";
                        //    strQry += " \"RSD_RI_NO\" ='" + objDeliverpending.sRINo + "', \"RSD_RI_DATE\" =to_date('" + objDeliverpending.sRIDate + "','DD/MM/YYYY'),";
                        //    strQry += " \"RSD_WARENTY_PERIOD\" ='" + strDetailVal[i].Split('~').GetValue(3).ToString() + "', \"RSD_GUARRENTY_TYPE\" ='" + strDetailVal[i].Split('~').GetValue(4).ToString() + "',";
                        //    strQry += "\"RSD_PROCESS_FLAG\"=1";
                        //    strQry += "  WHERE \"RSD_ID\" ='" + strDetailVal[i].Split('~').GetValue(0).ToString() + "'";
                        //    objCon.ExecuteQry(strQry);
                        //}
                        //else
                        //{
                        //    strQry = "Update \"TBLREPAIRSENTDETAILS\" SET ";
                        //    strQry += "\"RSD_DELIVARY_DATE\" =to_date('" + objDeliverpending.sDeliverDate + "','DD/MM/YYYY'),";
                        //    strQry += "\"RSD_DELIVER_VER_BY\" ='" + objDeliverpending.sVerifiedby.Trim().ToUpper() + "',";
                        //    strQry += "\"RSD_DELIVER_LOCATION\" ='" + objDeliverpending.sStoreId + "',";
                        //    strQry += "\"RSD_DELIVER_CHALLEN_NO\" ='" + objDeliverpending.sDeliverChallenNo.Trim().ToUpper() + "', ";
                        //    strQry += " \"RSD_RV_NO\" ='" + objDeliverpending.sRVNo + "', \"RSD_RV_DATE\" =to_date('" + objDeliverpending.sRVDate + "','DD/MM/YYYY'),";
                        //    strQry += " \"RSD_RI_NO\" ='" + objDeliverpending.sRINo + "', \"RSD_RI_DATE\" =to_date('" + objDeliverpending.sRIDate + "','DD/MM/YYYY'),";
                        //    //strQry += ",RSD_WARENTY_PERIOD='"+ objDeliverpending.sWarrantyPeriod +"',RSD_GUARRENTY_TYPE='"+ objDeliverpending.sGuarantyType +"'";
                        //    strQry += "\"RSD_PROCESS_FLAG\"=1";
                        //    strQry += " where \"RSD_ID\" ='" + strDetailVal[i].Split('~').GetValue(0).ToString() + "'";
                        //    objCon.ExecuteQry(strQry);
                        //}

                        //if (strDetailVal[i].Split('~').GetValue(2).ToString() == "1")
                        //{
                        //    strQry = "UPDATE \"TBLTCMASTER\" SET \"TC_CURRENT_LOCATION\"=1,\"TC_STORE_ID\"='" + objDeliverpending.sStoreId + "', ";
                        //    strQry += " \"TC_STATUS\"=2,\"TC_UPDATED_EVENT\"='DELIVER TC',\"TC_UPDATED_EVENT_ID\" ='" + strDetailVal[i].Split('~').GetValue(0).ToString() + "' ";
                        //    strQry += " WHERE \"TC_CODE\"='" + strDetailVal[i].Split('~').GetValue(1).ToString() + "'";
                        //    objCon.ExecuteQry(strQry);
                        //}
                        //else if (strDetailVal[i].Split('~').GetValue(2).ToString() == "3")
                        //{
                        //    strQry = "UPDATE \"TBLTCMASTER\" SET \"TC_CURRENT_LOCATION\"=1, \"TC_STORE_ID\" ='" + objDeliverpending.sStoreId + "', ";
                        //    strQry += " \"TC_STATUS\" =6, \"TC_UPDATED_EVENT\" ='DELIVER TC', \"TC_UPDATED_EVENT_ID\" ='" + strDetailVal[i].Split('~').GetValue(0).ToString() + "' ";
                        //    strQry += " WHERE \"TC_CODE\" ='" + strDetailVal[i].Split('~').GetValue(1).ToString() + "'";
                        //    objCon.ExecuteQry(strQry);
                        //}
                        //else if (strDetailVal[i].Split('~').GetValue(2).ToString() == "4")
                        //{
                        //    strQry = "UPDATE \"TBLTCMASTER\" SET \"TC_CURRENT_LOCATION\" =1, \"TC_STORE_ID\" ='" + Convert.ToInt32(objDeliverpending.sStoreId) + "', ";
                        //    strQry += " \"TC_STATUS\"=3,\"TC_UPDATED_EVENT\"='DELIVER TC',\"TC_UPDATED_EVENT_ID\"='" + strDetailVal[i].Split('~').GetValue(0).ToString() + "'";
                        //    strQry += " WHERE \"TC_CODE\"='" + strDetailVal[i].Split('~').GetValue(1).ToString() + "'";
                        //    // NpgsqlCommand = new NpgsqlCommand();
                        //    // NpgsqlCommand.Parameters.AddWithValue("sStoreId", Convert.ToInt32(objDeliverpending.sStoreId));
                        //    // NpgsqlCommand.Parameters.AddWithValue("strDetailVal", strDetailVal[i].Split('~').GetValue(0).ToString());
                        //    // NpgsqlCommand.Parameters.AddWithValue("strDetailVal1", strDetailVal[i].Split('~').GetValue(1).ToString());

                        //    objCon.ExecuteQry(strQry);
                        //}

                        string TcCode = strDetailVal[i].Split('~').GetValue(1).ToString();
                        string RsdDelivaryDate = objDeliverpending.sDeliverDate;
                        string RsdDelivarBy = objDeliverpending.sVerifiedby.Trim().ToUpper();
                        string RsdDelivarLoc = objDeliverpending.sStoreId;
                        string RsdDelivaryChallenNo = objDeliverpending.sDeliverChallenNo.Trim().ToUpper();
                        string RsdRvNum = objDeliverpending.sRVNo;
                        string RsdRvDate = objDeliverpending.sRVDate;
                        string RsdRiNo = objDeliverpending.sRINo;
                        string RsdRiDate = objDeliverpending.sRIDate;
                        string RsdWarentyPeriod = strDetailVal[i].Split('~').GetValue(3).ToString();
                        string RsdGuarentyType = strDetailVal[i].Split('~').GetValue(4).ToString();
                        string RsdId = strDetailVal[i].Split('~').GetValue(0).ToString();
                        string GetVal2 = strDetailVal[i].Split('~').GetValue(2).ToString();
                        string StoreId = objDeliverpending.sStoreId;
                        string TcUpdateEventId = strDetailVal[i].Split('~').GetValue(0).ToString();


                        NpgsqlCommand cmd1 = new NpgsqlCommand("proc_save_deliver_tc_details");
                        cmd1.Parameters.AddWithValue("tc_code", (TcCode ?? ""));
                        cmd1.Parameters.AddWithValue("rsd_delivary_date", (RsdDelivaryDate ?? ""));
                        cmd1.Parameters.AddWithValue("rsd_delivar_by", (RsdDelivarBy ?? ""));
                        cmd1.Parameters.AddWithValue("rsd_delivar_loc", (RsdDelivarLoc ?? ""));
                        cmd1.Parameters.AddWithValue("rsd_delivar_challen_no", (RsdDelivaryChallenNo ?? ""));
                        cmd1.Parameters.AddWithValue("rsd_rv_num", (RsdRvNum ?? ""));
                        cmd1.Parameters.AddWithValue("rsd_rv_date", (RsdRvDate ?? ""));
                        cmd1.Parameters.AddWithValue("rsd_ri_no", (RsdRiNo ?? ""));
                        cmd1.Parameters.AddWithValue("rsd_ri_date", (RsdRiDate ?? ""));
                        cmd1.Parameters.AddWithValue("rsd_wanrenty_period", (RsdWarentyPeriod ?? ""));
                        cmd1.Parameters.AddWithValue("rsd_guarenty_type", (RsdGuarentyType ?? ""));
                        cmd1.Parameters.AddWithValue("rsd_id", (RsdId ?? ""));
                        cmd1.Parameters.AddWithValue("get_val_2", (GetVal2 ?? ""));
                        cmd1.Parameters.AddWithValue("store_id", (StoreId ?? ""));
                        cmd1.Parameters.AddWithValue("tc_update_event_id", (TcUpdateEventId ?? ""));
                        cmd1.Parameters.Add("msg", NpgsqlDbType.Text);
                        cmd1.Parameters.Add("op_id", NpgsqlDbType.Text);
                        cmd1.Parameters["msg"].Direction = ParameterDirection.Output;
                        cmd1.Parameters["op_id"].Direction = ParameterDirection.Output;
                        Arr1[0] = "msg";
                        Arr1[1] = "op_id";
                        Arr1 = objCon.Execute(cmd1, Arr1, 2);


                        Arr[0] = "Repaired Transformer Successfully Recieved in Store";
                        Arr[1] = "0";
                    }

                }

                objCon.CommitTransaction();
                return Arr;
            }
            catch (Exception ex)
            {
                objCon.RollBackTrans();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }
        public clsDTrRepairActivity GetRepairSentDetails(clsDTrRepairActivity objRepair)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                //strQry = "SELECT \"RSM_ID\",TO_CHAR(\"RSM_ISSUE_DATE\",'DD/MM/YYYY') RSM_ISSUE_DATE,\"RSM_PO_NO\",\"RSM_DIV_CODE\",TO_CHAR(\"RSM_PO_DATE\",'DD/MM/YYYY') RSM_PO_DATE, \"RSM_INV_NO\" ,";
                //strQry += " TO_CHAR(\"RSM_INV_DATE\",'DD/MM/YYYY') RSM_INV_DATE,\"RSM_GUARANTY_TYPE\",\"RSM_SUPREP_TYPE\",\"RSM_SUPREP_ID\",\"RSM_MANUAL_INV_NO\",\"RSM_OLD_PO_NO\",\"RSM_REMARKS\" FROM \"TBLREPAIRSENTMASTER\" WHERE \"RSM_ID\" =:sRepairMasterId";
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "SELECT \"RSM_ID\",TO_CHAR(\"RSM_ISSUE_DATE\",'DD/MM/YYYY') RSM_ISSUE_DATE,\"RSM_PO_NO\",\"RSM_DIV_CODE\",TO_CHAR(\"RSM_PO_DATE\",'DD/MM/YYYY') RSM_PO_DATE, \"RSM_INV_NO\" ,";
                //strQry += " TO_CHAR(\"RSM_INV_DATE\",'DD/MM/YYYY') RSM_INV_DATE,\"RSM_GUARANTY_TYPE\",\"RSM_SUPREP_TYPE\",\"RSM_SUPREP_ID\",\"RSM_MANUAL_INV_NO\",\"RSM_OLD_PO_NO\",\"RSM_REMARKS\" , ";
                //strQry += " \"RESTD_EST_NO\",\"RESTD_REPAIRER\", cast(round(\"RESTD_TOTAL_AMOUNT\",2) as text) \"RESTD_TOTAL_AMOUNT\",TO_CHAR(\"RESTD_DATE\",'DD/MM/YYYY') RESTD_DATE,\"RESTD_STARRATE\",\"RWO_NO\",TO_CHAR(\"RWO_DATE\",'DD/MM/YYYY') RWO_DATE ,\"RSM_PO_QNTY\" FROM \"TBLREPAIRSENTMASTER\" ";
                //strQry += " INNER JOIN \"TBLREPAIRERESTIMATIONDETAILS\" ON \"RSM_RESTD_ID\"=\"RESTD_ID\" INNER JOIN \"TBLREPAIRERWORKORDER\" ON \"RSM_RWO_ID\"=\"RWO_ID\" WHERE \"RSM_ID\" ='" + Convert.ToInt32(sRepairMasterId) + "'";

                ////   NpgsqlCommand.Parameters.AddWithValue("sRepairMasterId", Convert.ToInt32(sRepairMasterId));
                //dt = objCon.FetchDataTable(strQry);
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getrepairsentdetails");
                cmd.Parameters.AddWithValue("rsmid", Convert.ToString(sRepairMasterId));
                dt = objCon.FetchDataTable(cmd);
                if (dt.Rows.Count > 0)
                {
                    objRepair.sStoreId = Convert.ToString(dt.Rows[0]["RSM_DIV_CODE"]);
                    objRepair.sIssueDate = Convert.ToString(dt.Rows[0]["RSM_ISSUE_DATE"]);
                    objRepair.sPurchaseOrderNo = Convert.ToString(dt.Rows[0]["RSM_PO_NO"]);
                    objRepair.sPurchaseDate = Convert.ToString(dt.Rows[0]["RSM_PO_DATE"]);
                    objRepair.sInvoiceNo = Convert.ToString(dt.Rows[0]["RSM_INV_NO"]);
                    objRepair.sInvoiceDate = Convert.ToString(dt.Rows[0]["RSM_INV_DATE"]);
                    objRepair.sGuarantyType = Convert.ToString(dt.Rows[0]["RSM_GUARANTY_TYPE"]);
                    objRepair.sType = Convert.ToString(dt.Rows[0]["RSM_SUPREP_TYPE"]);
                    objRepair.sSupRepId = Convert.ToString(dt.Rows[0]["RSM_SUPREP_ID"]);
                    objRepair.sOldPONo = Convert.ToString(dt.Rows[0]["RSM_OLD_PO_NO"]);
                    objRepair.sPORemarks = Convert.ToString(dt.Rows[0]["RSM_REMARKS"]);

                    objRepair.sManualInvoiceNo = Convert.ToString(dt.Rows[0]["RSM_MANUAL_INV_NO"]);

                    objRepair.sEstimationNo = Convert.ToString(dt.Rows[0]["RESTD_EST_NO"]);
                    objRepair.sRepairerId = Convert.ToString(dt.Rows[0]["RESTD_REPAIRER"]);
                    objRepair.sEstimationAmount = Convert.ToString(dt.Rows[0]["RESTD_TOTAL_AMOUNT"]);
                    objRepair.sEstimationDate = Convert.ToString(dt.Rows[0]["RESTD_DATE"]);
                    objRepair.sstarrate = Convert.ToString(dt.Rows[0]["RESTD_STARRATE"]);
                    objRepair.sWorkorderNo = Convert.ToString(dt.Rows[0]["RWO_NO"]);
                    objRepair.sWorkorderDate = Convert.ToString(dt.Rows[0]["RWO_DATE"]);
                    objRepair.sQty = Convert.ToString(dt.Rows[0]["RSM_PO_QNTY"]);
                }

                return objRepair;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objRepair;
            }
        }

        public clsDTrRepairActivity GetRepairRecieveDetails(clsDTrRepairActivity objRepair)
        {
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();

                //strQry = "SELECT TO_CHAR(\"RSD_DELIVARY_DATE\",'DD/MM/YYYY') RSD_DELIVARY_DATE ,\"RSD_DELIVER_VER_BY\",";
                //strQry += " \"RSD_DELIVER_LOCATION\",\"RSD_DELIVER_CHALLEN_NO\",\"RSD_RV_NO\",TO_CHAR(\"RSD_RV_DATE\",'DD/MM/YYYY') RSD_RV_DATE FROM ";
                //strQry += " \"TBLREPAIRSENTDETAILS\" WHERE \"RSD_ID\" =:sRepairDetailsId";
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sRepairDetailsId", Convert.ToInt32(objRepair.sRepairDetailsId));
                //dt = objCon.FetchDataTable(strQry, NpgsqlCommand);
                NpgsqlCommand = new NpgsqlCommand("sp_getrepairrecievedetails");
                NpgsqlCommand.Parameters.AddWithValue("rsdid", Convert.ToInt32(objRepair.sRepairDetailsId));
                dt = objCon.FetchDataTable(NpgsqlCommand);
                if (dt.Rows.Count > 0)
                {
                    objRepair.sDeliverDate = Convert.ToString(dt.Rows[0]["RSD_DELIVARY_DATE"]);
                    objRepair.sVerifiedby = Convert.ToString(dt.Rows[0]["RSD_DELIVER_VER_BY"]);
                    objRepair.sDeliverChallenNo = Convert.ToString(dt.Rows[0]["RSD_DELIVER_CHALLEN_NO"]);
                    objRepair.sRVNo = Convert.ToString(dt.Rows[0]["RSD_RV_NO"]);
                    objRepair.sRVDate = Convert.ToString(dt.Rows[0]["RSD_RV_DATE"]);

                }

                return objRepair;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objRepair;
            }
        }



        public string GetRepairDetailsId(string sRepairMasterId)
        {
            DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
            try
            {
                string strQry = string.Empty;

                //strQry = "SELECT \"RSD_RSM_ID\" FROM \"TBLREPAIRSENTDETAILS\" WHERE \"RSD_ID\" =:sRepairMasterId";
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sRepairMasterId", Convert.ToInt32(sRepairMasterId));
                //return objCon.get_value(strQry, NpgsqlCommand);

                NpgsqlCommand cmd = new NpgsqlCommand("sp_getrepairdetailsid");
                cmd.Parameters.AddWithValue("rsd_id", sRepairMasterId);
                return objDatabse.StringGetValue(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;

            }
        }
        public double GetEstAmt(string store, string Rep, string starrate, string cap)
        {
            double amt = 0;
            try
            {
                string strQry = string.Empty;
                NpgsqlCommand = new NpgsqlCommand();
                //strQry = "select \"MD_ID\" from \"TBLMASTERDATA\" where upper(\"MD_NAME\") =upper('" + starrate + "')";
                //string rate = objCon.get_value(strQry, NpgsqlCommand);
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getestimationamt");
                cmd.Parameters.AddWithValue("starrate", starrate);
                string rate = objDatabse.StringGetValue(cmd);

                if (rate == "1" || rate == "2")
                {
                    rate = "1";
                }
                else
                {
                    rate = "2";
                }
                //strQry = "select coalesce(\"MRI_BASE_RATE\",0) from \"TBLMINORREPAIRITEMRATEMASTER\" where \"MRI_TR_ID\"='" + Rep + "' and \"MRI_DIV_ID\"='"+ store + "' and \"MRI_RATETYPE\"='"+ rate + "' and \"MRI_CAPACITY\"=(select \"DCR_ID\" from \"TBLDTRCAPACITYRANGE\" WHERE \"DCR_CAPACITY\"='"+ cap + "' limit 1)";
                #region working
                //strQry = "select cast(coalesce(\"RR_AMOUNT\",0) as text) from \"TBLREPAIRERRATES\" where cast(\"RR_REP_ID\"  as text)='" + Rep + "' and  ";
                //strQry += " cast(\"RR_DIV_ID\" as text)='" + store + "'  and cast(\"RR_RATING_ID\" as text) =cast('" + rate + "' as text) and \"RR_CAP_ID\"=(select \"MD_ID\" ";
                //strQry += " from \"TBLMASTERDATA\" where  \"MD_TYPE\"='C' and \"MD_NAME\"='" + cap + "' limit 1)";
                //string amount = objCon.get_value(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmmd = new NpgsqlCommand("sp_getestamt");
                cmmd.Parameters.AddWithValue("rep", Rep);
                cmmd.Parameters.AddWithValue("store", store);
                cmmd.Parameters.AddWithValue("rate", rate);
                cmmd.Parameters.AddWithValue("cap", cap);
                string amount = objDatabse.StringGetValue(cmmd);
                if (amount != "")
                {
                    amt = Convert.ToDouble(amount);
                }
                else
                {
                    amt = 0;
                }



                // amt = Convert.ToDouble(amount);
                //  amt = Convert.ToDouble(objCon.get_value(strQry, NpgsqlCommand));

                return amt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return amt;
            }
        }

        public bool RepairDateIsValidnew(string sRepair_Id, string offcode)
        {
            string StrQry = string.Empty;
            try
            {
                NpgsqlCommand = new NpgsqlCommand();
                StrQry = "select count(*) from \"TBLREPAIRERRATES\" inner join \"TBLTRANSREPAIRER\" on \"TR_ID\"=\"RR_REP_ID\" where \"RR_REP_ID\"=:sRepair_Id and \"RR_DIV_ID\"=(select \"DIV_ID\" from \"TBLDIVISION\" where \"DIV_CODE\"=:offcode) and NOW() BETWEEN \"RR_EFFECTIVE_FROM\" AND \"RR_EFFECTIVE_TO\"";
                NpgsqlCommand.Parameters.AddWithValue("sRepair_Id", Convert.ToInt32(sRepair_Id));
                NpgsqlCommand.Parameters.AddWithValue("offcode", Convert.ToInt32(offcode));
                int count = Convert.ToInt16(objCon.get_value(StrQry, NpgsqlCommand));
                if (count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }
        public DataTable GetAllImages()
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                //   strQry = "SELECT \"IND_DOC\" FROM \"TBLINSPECTIONDETAILS\" WHERE \"IND_DOC\" IS NOT NULL";
                //  dt = objCon.FetchDataTable(strQry);
                //   return dt;
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getallindimages");
                dt = objCon.FetchDataTable(cmd);
                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }
        }

        public string getdivid(string DIV_CODE)
        {
            string val = string.Empty;
            try
            {
                //string strQry = string.Empty;
                //strQry = "SELECT \"DIV_ID\" FROM \"TBLDIVISION\" WHERE \"DIV_CODE\"='" + DIV_CODE + "'";
                //val = objCon.get_value(strQry);
                //return val;
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getdivid");
                cmd.Parameters.AddWithValue("div_code", DIV_CODE);
                val = objDatabse.StringGetValue(cmd);
                return val;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return val;
            }
        }

        public string getdivcode(string div_id)
        {
            string val = string.Empty;
            try
            {
                //string strQry = string.Empty;
                //strQry = "SELECT \"DIV_CODE\" FROM \"TBLDIVISION\" WHERE \"DIV_ID\"='" + div_id + "'";
                //val = objCon.get_value(strQry);
                //return val;
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getdivcode");
                cmd.Parameters.AddWithValue("divid", div_id);
                val = objDatabse.StringGetValue(cmd);
                return val;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return val;
            }
        }

        public string getstrcode(string STR_ID)
        {
            string val = string.Empty;
            try
            {
                //string strQry = string.Empty;
                //strQry = "SELECT \"STO_OFF_CODE\" FROM \"TBLSTOREOFFCODE\" WHERE \"STO_SM_ID\"='" + STR_ID + "'";
                //val = objCon.get_value(strQry);
                //return val;
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getstrcode");
                cmd.Parameters.AddWithValue("strid", STR_ID);
                val = objDatabse.StringGetValue(cmd);
                return val;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return val;
            }
        }

        #region Convert Image to Byte Array
        public static byte[] ConvertImg(string imageLocation)
        {
            byte[] imageData = null;
            FileInfo fileInfo = new FileInfo(imageLocation);
            long imageFileLength = fileInfo.Length;
            FileStream fs = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            imageData = br.ReadBytes((int)imageFileLength);
            return imageData;
        }
        #endregion

        public DataTable GetRepairPoDetails(clsDTrRepairActivity objRepair)
        {
            DataTable dtPoDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                if (objRepair.sroletype == "2")
                {
                    //strQry = "SELECT \"TC_CODE\",\"TC_SLNO\",(SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\")TM_NAME,";
                    //strQry += " CAST(\"TC_CAPACITY\" AS TEXT)TC_CAPACITY,to_char(\"TC_MANF_DATE\",'DD-MON-YYYY')TC_MANF_DATE,\"RSD_INVOICE_GUARRENTY_TYPE\" as RSM_GUARANTY_TYPE ,CASE WHEN ";
                    //strQry += " \"RSD_DELIVARY_DATE\" IS NULL THEN 'Repair Pending' WHEN \"RSD_DELIVARY_DATE\" IS NOT NULL THEN 'Repair Completed' END ";
                    //strQry += "STATUS FROM \"TBLREPAIRSENTMASTER\" INNER JOIN \"TBLREPAIRSENTDETAILS\" ON \"RSM_ID\"=\"RSD_RSM_ID\" INNER JOIN \"TBLTCMASTER\" ON ";
                    ////strQry += " \"TC_CODE\"=\"RSD_TC_CODE\" AND \"RSM_DIV_CODE\" =:sOfficeCode AND \"RSM_PO_NO\" =:sPurchaseOrderNo";
                    ////strQry += " \"TC_CODE\"=\"RSD_TC_CODE\" AND \"RSM_DIV_CODE\" =(select \"DIV_CODE\" from \"TBLDIVISION\" where \"DIV_ID\"=:sOfficeCode) AND \"RSM_PO_NO\" =:sPurchaseOrderNo";
                    //strQry += " \"TC_CODE\"=\"RSD_TC_CODE\" AND \"RSM_DIV_CODE\" =(select \"SM_CODE\" from \"TBLSTOREMAST\" where \"SM_ID\"=:sOfficeCode) AND \"RSM_PO_NO\" =:sPurchaseOrderNo";
                    //NpgsqlCommand = new NpgsqlCommand();
                    //NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", Convert.ToInt32(objRepair.sOfficeCode));
                    //NpgsqlCommand.Parameters.AddWithValue("sPurchaseOrderNo", objRepair.sPurchaseOrderNo);
                    //dtPoDetails = objCon.FetchDataTable(strQry, NpgsqlCommand);

                    NpgsqlCommand cmd = new NpgsqlCommand("sp_getrepairpodetails");
                    cmd.Parameters.AddWithValue("p_rsmpo_no", sPurchaseOrderNo);
                    cmd.Parameters.AddWithValue("p_officecode", Convert.ToInt32(objRepair.sOfficeCode));
                    dtPoDetails = objCon.FetchDataTable(cmd);
                }
                else
                {
                    //strQry = "SELECT \"TC_CODE\",\"TC_SLNO\",(SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\")TM_NAME,";
                    //strQry += " CAST(\"TC_CAPACITY\" AS TEXT)TC_CAPACITY,to_char(\"TC_MANF_DATE\",'DD-MON-YYYY')TC_MANF_DATE,\"RSD_INVOICE_GUARRENTY_TYPE\" as RSM_GUARANTY_TYPE ,CASE WHEN ";
                    //strQry += " \"RSD_DELIVARY_DATE\" IS NULL THEN 'Repair Pending' WHEN \"RSD_DELIVARY_DATE\" IS NOT NULL THEN 'Repair Completed' END ";
                    //strQry += "STATUS FROM \"TBLREPAIRSENTMASTER\" INNER JOIN \"TBLREPAIRSENTDETAILS\" ON \"RSM_ID\"=\"RSD_RSM_ID\" INNER JOIN \"TBLTCMASTER\" ON ";
                    //if (objRepair.sroleid == "43")
                    //{
                    //    strQry += " \"TC_CODE\"=\"RSD_TC_CODE\" AND \"RSM_PO_NO\" =:sPurchaseOrderNo";
                    //}
                    //else
                    //{
                    //    if (objRepair.sOfficeCode == "")
                    //        strQry += " \"TC_CODE\"=\"RSD_TC_CODE\"  AND \"RSM_PO_NO\" =:sPurchaseOrderNo";
                    //    else
                    //    strQry += " \"TC_CODE\"=\"RSD_TC_CODE\" AND \"RSM_DIV_CODE\" =:sOfficeCode AND \"RSM_PO_NO\" =:sPurchaseOrderNo";
                    //}

                    NpgsqlCommand cmd1 = new NpgsqlCommand("sp_getrepairpodetailswhenroleid43");
                    if (objRepair.sroleid != "43" && objRepair.sOfficeCode != "")
                    {
                        cmd1.Parameters.AddWithValue("sofficecode", (objRepair.sOfficeCode ?? ""));
                    }
                    cmd1.Parameters.AddWithValue("spurchaseorderno", (objRepair.sPurchaseOrderNo ?? ""));

                    dtPoDetails = objCon.FetchDataTable(cmd1);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return dtPoDetails;
        }

        public void GetEstimateDetailsFromXML(string sWFDataId, clsDTrRepairActivity objRepair)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable dt = new DataTable();

                DataSet ds = new DataSet();

                ds = objApproval.GetDatatableFromMultipleXML(sWFDataId);

                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    if (ds.Tables[i].Rows.Count > 0)
                    {
                        dt = ds.Tables[i];
                        if (i == 0)
                        {
                            objRepair.sEstimationNo = Convert.ToString(dt.Rows[0]["RESTD_EST_NO"]);
                            objRepair.sRepairerId = Convert.ToString(dt.Rows[0]["RESTD_REPAIRER"]);
                            objRepair.sEstimationAmount = Convert.ToString(dt.Rows[0]["RESTD_TOTAL_AMOUNT"]);
                            objRepair.sEstimationDate = Convert.ToString(dt.Rows[0]["RESTD_DATE"]);
                            objRepair.sstarrate = Convert.ToString(dt.Rows[0]["RESTD_STARRATE"]);
                        }
                        else if (i == 1)
                        {
                            objRepair.sWorkorderNo = Convert.ToString(dt.Rows[0]["RWO_NO"]);
                            objRepair.sWorkorderDate = Convert.ToString(dt.Rows[0]["RWO_DATE"]);
                        }
                        else if (i == 2)
                        {
                            objRepair.sIssueDate = Convert.ToString(dt.Rows[0]["RSM_ISSUE_DATE"]);
                            objRepair.sPurchaseOrderNo = Convert.ToString(dt.Rows[0]["RSM_PO_NO"]);
                            objRepair.sPurchaseDate = Convert.ToString(dt.Rows[0]["RSM_PO_DATE"]);
                            objRepair.sInvoiceNo = Convert.ToString(dt.Rows[0]["RSM_INV_NO"]);
                            objRepair.sInvoiceDate = Convert.ToString(dt.Rows[0]["RSM_INV_DATE"]);
                            objRepair.sType = Convert.ToString(dt.Rows[0]["RSM_SUPREP_TYPE"]);
                            objRepair.sSupRepId = Convert.ToString(dt.Rows[0]["RSM_SUPREP_ID"]);
                            objRepair.sManualInvoiceNo = Convert.ToString(dt.Rows[0]["RSM_MANUAL_INV_NO"]);
                            objRepair.sOldPONo = Convert.ToString(dt.Rows[0]["RSM_OLD_PO_NO"]);
                            objRepair.sPORemarks = Convert.ToString(dt.Rows[0]["RSM_REMARKS"]);
                        }
                        else if (i == 3)
                        {
                            objRepair.sTcCode = Convert.ToString(dt.Rows[0]["RSD_TC_CODE"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void updatemodifyeddata(string sIssueDate, string sPurchaseDate, string sManualInvoiceNo, string sOldPONo, string sPORemarks, string sRSMID)
        {
            try
            {
                string[] arr = new string[0];
                string strQry = string.Empty;
                NpgsqlCommand = new NpgsqlCommand();
                //strQry = "UPDATE \"TBLREPAIRSENTMASTER\" SET \"RSM_ISSUE_DATE\" = to_date('" + sIssueDate + "','DD/MM/YYYY'), \"RSM_PO_DATE\" = to_date('" + sPurchaseDate + "','DD/MM/YYYY'), ";
                //strQry += " \"RSM_MANUAL_INV_NO\" = '" + sManualInvoiceNo + "', \"RSM_OLD_PO_NO\" = '" + sOldPONo + "', \"RSM_REMARKS\" = '" + sPORemarks + "' WHERE \"RSM_ID\" = '" + sRSMID + "';";
                //objCon.ExecuteQry(strQry, NpgsqlCommand);
                #region working query
                //strQry = "UPDATE \"TBLREPAIRSENTMASTER\" SET \"RSM_PO_DATE\" = to_date('" + sPurchaseDate + "','DD/MM/YYYY'), ";
                //strQry += " \"RSM_MANUAL_INV_NO\" = '" + sManualInvoiceNo + "', \"RSM_OLD_PO_NO\" = '" + sOldPONo + "', \"RSM_REMARKS\" = '" + sPORemarks + "' WHERE \"RSM_ID\" = '" + sRSMID + "';";
                //objCon.ExecuteQry(strQry, NpgsqlCommand);
                #endregion


                NpgsqlCommand cmd = new NpgsqlCommand("proc_updatemodifyeddata");
                cmd.Parameters.AddWithValue("spurchasedate", sPurchaseDate);
                cmd.Parameters.AddWithValue("smanualinvoiceno", sManualInvoiceNo);
                cmd.Parameters.AddWithValue("soldpono", sOldPONo);
                cmd.Parameters.AddWithValue("sporemarks", sPORemarks);
                cmd.Parameters.AddWithValue("srsmid", sRSMID);
                objCon.Execute(cmd, arr, 0);
                //NpgsqlCommand.Parameters.AddWithValue("sRepairMasterId", Convert.ToInt32(sRepairMasterId));
                //return objCon.get_value(strQry, NpgsqlCommand);



            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);


            }
        }
        public void updatefilepath(string sbudgetfile, string spofile, string sRSMID)
        {
            try
            {
                string[] arr = new string[0];
                string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "UPDATE \"TBLREPAIRSENTMASTER\" SET \"RSM_REP_BUDGET_DOC\" = '" + sbudgetfile + "',\"RSM_PURCHASE_ORDER_DOC\" = '" + spofile + "' WHERE \"RSM_ID\" = '" + sRSMID + "';";
                //objCon.ExecuteQry(strQry, NpgsqlCommand);
                NpgsqlCommand cmd = new NpgsqlCommand("proc_updatefilepath");
                cmd.Parameters.AddWithValue("sbudgetfile", sbudgetfile);
                cmd.Parameters.AddWithValue("spofile", spofile);
                cmd.Parameters.AddWithValue("srsmid", sRSMID);
                objCon.Execute(cmd, arr, 0);

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);


            }
        }

        /// <summary>
        /// Coded by sandeep on 25-07-2023
        /// This method used to fetch the transformer details while click on edit button in faulty tc search
        /// </summary>
        /// <param name="objTcRepair"></param>
        /// <returns></returns>
        public DataTable LoadTransformerdetails(clsDTrRepairActivity objTcRepair)
        {
            DataTable dt = new DataTable();
            try
            {

                NpgsqlCommand cmd = new NpgsqlCommand("sp_get_transformerdetails_repairer");
                cmd.Parameters.AddWithValue("stc_id", Convert.ToInt32(objTcRepair.sTcId));

                dt = objCon.FetchDataTable(cmd);

                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        /// <summary>
        /// Coded by sandeep on 25-07-2023
        /// This method used to update the guarantee type details in tcmaster
        /// </summary>-
        /// <param name="objTcRepair"></param>
        /// <returns></returns>
        public string[] Updatetcmaster(clsDTrRepairActivity objTcRepair)
        {
            string[] Arr = new string[3];

            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_update_tcmaster");
                cmd.Parameters.AddWithValue("stc_id", Convert.ToString(objTcRepair.sTcId));
                cmd.Parameters.AddWithValue("oldguarantee", Convert.ToString(objTcRepair.OldGuaranteetype));
                cmd.Parameters.AddWithValue("newgurarantee", Convert.ToString(objTcRepair.sGuarantyType));
                cmd.Parameters.AddWithValue("reason", Convert.ToString(objTcRepair.Reason));
                cmd.Parameters.AddWithValue("remarks", Convert.ToString(objTcRepair.Remarks));
                cmd.Parameters.AddWithValue("type", "FAULTYDTR");
                cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                Arr[0] = "msg";
                Arr[1] = "op_id";

                Arr = objCon.Execute(cmd, Arr, 2);
                return Arr;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;

            }
        }
    }
}
