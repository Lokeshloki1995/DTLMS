using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using IIITS.PGSQL.DAL;
using Npgsql;
using System.IO;
using System.Drawing;
using System.Configuration;

namespace IIITS.DTLMS.BL
{
    public class clsReports
    {
        string strFormCode = "clsReports";
        public string offcodeDT { get; set; }
        public string UserStatusFlag { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public string sStarType { get; set; }
        public string sWoundType { get; set; }
        public string sGuarantyTypes { get; set; }
        public string sFromDate { get; set; }
        public string sTodate { get; set; }
        public string sTempFromDate { get; set; }
        public string sTempTodate { get; set; }
        public string sType { get; set; }
        public string SelectStage { get; set; } // added on 07-08-2023
        public string sFailureType { get; set; }
        public string sGreaterVal { get; set; }
        public string sRepriername { get; set; }
        public string sFailType { get; set; }
        public string sOldFeederCode { get; set; }
        public string sNewFeederCode { get; set; }
        public string sCapacity { get; set; }
        public string sMake { get; set; }
        public string sOfficeCode { get; set; }

        public string creportflag { get; set; }
        public string sFeeder { get; set; }
        public string sSchemeType { get; set; }
        public string sFeederType { get; set; }
        public string sDtcCode { get; set; }
        public string sDtrCode { get; set; }
        public string sFailId { get; set; }

        public string sFeederBifurcationID { get; set; }
        public string sMonth { get; set; }
        public string sCurrentMonth { get; set; }
        public string sEmployeeCost { get; set; }
        public string sESI { get; set; }
        public string ServiceTax { get; set; }
        public string DecomLabourCost { get; set; }
        public string sReportType { get; set; }
        public string sGuranteeType { get; set; }
        public string sRoleID { get; set; }
        public string sLocType { get; set; }
        public string sCoilType { get; set; }
        public string sFailureStatus { get; set; }
        public string StoreId { get; set; }
        public string FinancialYearId { get; set; }
       

        PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);

        int Circle_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Circle_code"]);
        int Division_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Division_code"]);
        int SubDiv_code = Convert.ToInt32(ConfigurationSettings.AppSettings["SubDiv_code"]);
        int Section_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Section_code"]);
        int Feeder_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Feeder_code"]);

        #region Internal Application
        public DataTable EnumerationReport(string strFromdate, string strToDate)
        {

            DataTable dtStoreDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                DateTime dFromDate = DateTime.ParseExact(strFromdate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                DateTime dToDate = DateTime.ParseExact(strToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);


                strQry = "   SELECT (SELECT \"IU_FULLNAME\" from \"TBLINTERNALUSERS\" WHERE \"IU_ID\"=\"ED_OPERATOR1\")\"ED_OPERATOR1\",";
                strQry += " (SELECT \"IU_FULLNAME\" from \"TBLINTERNALUSERS\" WHERE CAST(\"IU_ID\" AS TEXT)=\"ED_OPERATOR2\")\"ED_OPERATOR2\" , ";
                strQry += " COALESCE( SUM( CASE WHEN \"ED_ENUM_TYPE\" = 2 THEN 1 ELSE 0 END ),0) \"FIELD\",";
                strQry += " COALESCE( SUM( CASE WHEN \"ED_ENUM_TYPE\" = 1 THEN 1 ELSE 0 END ),0) \"STORES\",";
                strQry += " COALESCE( SUM( CASE WHEN \"ED_ENUM_TYPE\" = 3 THEN 1 ELSE 0 END ),0) \"REPAIRER\", ";
                strQry += " (COALESCE( sum(CASE WHEN \"ED_STATUS_FLAG\"=0 THEN 1 ELSE 0 END ),0 ))\"PENDING_QC\", ";
                strQry += " (COALESCE( sum(CASE WHEN \"ED_STATUS_FLAG\"=1 THEN 1 ELSE 0 END ),0 ))\"QC_DONE\",";
                strQry += " COUNT(\"ED_ENUM_TYPE\") \"TOTAL\" ";
                strQry += " FROM \"TBLENUMERATIONDETAILS\" where TO_CHAR(\"ED_CRON\",'yyyyMMdd')";
                strQry += " BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "' GROUP BY \"ED_OPERATOR1\",\"ED_OPERATOR2\"";
                dtStoreDetails = ObjCon.FetchDataTable(strQry);

                return dtStoreDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtStoreDetails;

            }
        }


        public DataTable PrintDetailedFieldReport(string strFromDate, string strToDate)
        {

            DataTable dtDetailedReport = new DataTable();
            string strQry = string.Empty;
            try
            {
                DateTime dFromDate = DateTime.ParseExact(strFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                DateTime dToDate = DateTime.ParseExact(strToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                strQry = "SELECT CAST(\"DTE_DTCCODE\" AS TEXT) \"DTE_DTCCODE\",CAST(\"DTE_TC_CODE\" AS TEXT)\"DTE_TC_CODE\",CAST(\"DTE_CAPACITY\" AS TEXT)\"DTE_CAPACITY\",\"DTE_NAME\",\"TM_NAME\",(select \"CM_CIRCLE_NAME\" from \"TBLCIRCLE\" WHERE \"CM_CIRCLE_CODE\"=SUBSTR(CAST(\"ED_OFFICECODE\" AS TEXT),1,'" + Constants.Circle + "')) AS \"Circle\",";
                strQry += "(SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE \"DIV_CODE\"=SUBSTR(CAST(\"ED_OFFICECODE\" AS TEXT),1,'" + Constants.Division + "')) as \"Division\",";
                strQry += "(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE \"SD_SUBDIV_CODE\"=SUBSTR(CAST(\"ED_OFFICECODE\" AS TEXT),1,'" + Constants.SubDivision + "')) as \"SubDivision\"";
                strQry += ",(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where \"OM_CODE\"=SUBSTR(CAST(\"ED_OFFICECODE\" AS TEXT),1," + Constants.Section + ")) as \"Location\"   ";
                strQry += " FROM \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\",\"TBLTRANSMAKES\" WHERE \"DTE_MAKE\"=\"TM_ID\" AND \"ED_ID\"=\"DTE_ED_ID\" AND  \"ED_ENUM_TYPE\" ='2' AND ";
                strQry += " TO_CHAR(\"DTE_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                dtDetailedReport = ObjCon.FetchDataTable(strQry);

                return dtDetailedReport;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtDetailedReport;

            }
        }


        public DataTable PrintDetailedStoreReport(string strFromDate, string strToDate)
        {

            DataTable dtDetailedReport = new DataTable();
            string strQry = string.Empty;
            try
            {


                DateTime dFromDate = DateTime.ParseExact(strFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                DateTime dToDate = DateTime.ParseExact(strToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                strQry = "SELECT CAST(\"DTE_TC_CODE\" AS TEXT)\"DTE_TC_CODE\",CAST(\"DTE_CAPACITY\" AS TEXT)\"DTE_CAPACITY\",";
                strQry += " TO_CHAR(\"DTE_TC_MANFDATE\",'dd-MON-yyyy') \"DTE_TC_MANFDATE\",\"TM_NAME\",(select \"CM_CIRCLE_NAME\" from \"TBLCIRCLE\" WHERE \"CM_CIRCLE_CODE\"=SUBSTR(CAST(\"ED_OFFICECODE\" AS TEXT),1,'" + Constants.Circle + "')) AS \"Circle\",";
                strQry += "(SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE \"DIV_CODE\"=SUBSTR(CAST(\"ED_OFFICECODE\" AS TEXT),1,'" + Constants.Division + "')) as \"Division\"";
                strQry += " FROM \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\",\"TBLTRANSMAKES\" WHERE \"DTE_MAKE\"=\"TM_ID\" AND \"ED_ID\"=\"DTE_ED_ID\" and  \"ED_ENUM_TYPE\" IN (1,3)";
                strQry += "  and TO_CHAR(\"DTE_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                dtDetailedReport = ObjCon.FetchDataTable(strQry);
                return dtDetailedReport;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtDetailedReport;

            }
        }

        public DataTable EnumReportLocationWise(string strFromdate, string strToDate)
        {

            DataTable dtStoreDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                DateTime dFromDate = DateTime.ParseExact(strFromdate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                DateTime dToDate = DateTime.ParseExact(strToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                strQry = " SELECT (SELECT \"IU_FULLNAME\" from \"TBLINTERNALUSERS\" WHERE cast(\"IU_ID\" as Text)= cast(\"ED_OPERATOR1\" as Text))\"ED_OPERATOR1\",";
                strQry += " (SELECT \"IU_FULLNAME\" from \"TBLINTERNALUSERS\" WHERE cast(\"IU_ID\" as Text)= cast(\"ED_OPERATOR2\" as Text))\"ED_OPERATOR2\" , ";
                strQry += " COALESCE( SUM( CASE WHEN \"ED_ENUM_TYPE\" = 2 THEN 1 ELSE 0 END ),0) \"FIELD\",";
                strQry += " COALESCE( SUM( CASE WHEN \"ED_ENUM_TYPE\" = 1 THEN 1 ELSE 0 END ),0) \"STORES\",";
                strQry += "COALESCE( SUM( CASE WHEN \"ED_ENUM_TYPE\" = 3 THEN 1 ELSE 0 END ),0) \"REPAIRER\", ";
                strQry += "  (COALESCE( sum(CASE WHEN \"ED_STATUS_FLAG\"=0 THEN 1 ELSE 0 END ),0 ))\"PENDING_QC\", ";
                strQry += "  (COALESCE( sum(CASE WHEN \"ED_STATUS_FLAG\"=1 THEN 1 ELSE 0 END ),0 ))\"QC_DONE\",  ";
                strQry += " COUNT(\"ED_ENUM_TYPE\") \"TOTAL\",";

                strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE cast(\"DIV_CODE\" as Varchar)=SUBSTR(CAST(\"ED_OFFICECODE\" AS TEXT),1,'" + Constants.Division + "')) as \"Division\",";
                strQry += " (select \"CM_CIRCLE_NAME\" from \"TBLCIRCLE\" WHERE cast(\"CM_CIRCLE_CODE\"as Varchar)=SUBSTR(CAST(\"ED_OFFICECODE\" AS TEXT),1,'" + Constants.Circle + "')) AS \"Circle\",";
                strQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE cast(\"SD_SUBDIV_CODE\" as Varchar)=SUBSTR(CAST(\"ED_OFFICECODE\" AS TEXT),1,'" + Constants.SubDivision + "')) as \"SubDivision\",";
                strQry += " (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where cast(\"OM_CODE\" as Varchar)=SUBSTR(CAST(\"ED_OFFICECODE\" AS TEXT),1,'" + Constants.Section + "')) as \"Location\"";
                strQry += " FROM \"TBLENUMERATIONDETAILS\" where TO_CHAR(\"ED_CRON\",'yyyyMMdd')";
                strQry += " BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "' GROUP BY \"ED_OPERATOR1\",\"ED_OPERATOR2\",\"ED_OFFICECODE\"";

                dtStoreDetails = ObjCon.FetchDataTable(strQry);
                return dtStoreDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtStoreDetails;

            }
        }


        //public DataTable PrintFieldDetails(string sFeederCode, string sOfficeCode, string sFromEnumDate, string sToEnumDate, string sdatewise)
        //{
        //    PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
        //    DataTable dtStoreDetails = new DataTable("A");
        //    string strQry = string.Empty;
        //    try
        //    {
        //        // QCWise pending
        //        if (sdatewise == "1")
        //        {
        //            strQry = "select DISTINCT \"DTE_DTCCODE\",\"DTE_CESCCODE\",\"DTE_IPCODE\",\"DTE_NAME\", (select \"TM_NAME\" from \"TBLTRANSMAKES\" where \"TM_ID\"=\"DTE_MAKE\") \"DTE_MAKE\",\"DTE_TC_CODE\",\"DTE_TC_SLNO\",\"DTE_CAPACITY\",";
        //            strQry += "(SELECT \"ZO_NAME\" FROM \"TBLZONE\" WHERE cast(\"ZO_ID\" as text)=SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'1')) as \"ZONE\" ,";
        //            strQry += "(SELECT \"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE cast(\"CM_CIRCLE_CODE\" as text)=SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'2')) as \"CIRCLE\" ,";
        //            strQry += "(SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE cast(\"DIV_CODE\" as text)=SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + Division_code + "')) as \"DIVISION\" ,";
        //            strQry += " \"FD_FEEDER_NAME\" as \"FEEDER\",";
        //            strQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE cast(\"SD_SUBDIV_CODE\" as text)=SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + SubDiv_code + "')) as \"SUBDIVISION\",";
        //            strQry += " (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where cast(\"OM_CODE\" as text)=SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + Section_code + "')) as \"SECTION\" ,TO_CHAR(\"DTE_TC_MANFDATE\",'MM/YYYY') \"DTE_TC_MANFDATE\",";
        //            strQry += " \"DTE_TANK_CAPACITY\",\"DTE_TC_WEIGHT\",TO_CHAR(\"ED_WELD_DATE\",'DD/MM/YYYY') \"ED_WELD_DATE\",\"ED_FEEDERCODE\" FROM ";
        //            strQry += " \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\",\"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"ED_ID\"= \"DTE_ED_ID\" ";
        //            strQry += " and \"ED_LOCTYPE\"='2' AND \"ED_STATUS_FLAG\" IN (0,2) AND  \"FD_FEEDER_CODE\"=\"ED_FEEDERCODE\" AND \"FD_FEEDER_ID\"=\"FDO_FEEDER_ID\" ";
        //            if (sFeederCode != "")
        //            {
        //                strQry += " AND \"ED_FEEDERCODE\" Like '" + sFeederCode + "%' ";
        //            }
        //            strQry += "AND cast(\"ED_OFFICECODE\" as text) LIKE '" + sOfficeCode + "%'";
        //            if (sFromEnumDate != "" && sToEnumDate != "")
        //            {
        //                DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //                DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //                strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
        //            }
        //            strQry += " ORDER BY \"DTE_DTCCODE\"";
        //        }
        //        // QCWise Done
        //        else if (sdatewise == "2")
        //        {
        //            strQry = "select DISTINCT \"DTE_DTCCODE\",\"DTE_CESCCODE\",\"DTE_IPCODE\",\"DTE_NAME\", (select \"TM_NAME\" from \"TBLTRANSMAKES\" where \"TM_ID\"=\"DTE_MAKE\")\"DTE_MAKE\",\"DTE_TC_CODE\",\"DTE_TC_SLNO\",\"DTE_CAPACITY\",";
        //            strQry += "(SELECT \"ZO_NAME\" FROM \"TBLZONE\" WHERE cast(\"ZO_ID\" as text)=SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'1')) as \"ZONE\" ,";
        //            strQry += "(SELECT \"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE cast(\"CM_CIRCLE_CODE\" as text)=SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'2')) as \"CIRCLE\" ,";
        //            strQry += "(SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE cast(\"DIV_CODE\" as text)=SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + Division_code + "')) as \"DIVISION\" ,";
        //            strQry += " \"FD_FEEDER_NAME\" as \"FEEDER\",";
        //            strQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE cast(\"SD_SUBDIV_CODE\" as text)=SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + SubDiv_code + "')) as \"SUBDIVISION\",";
        //            strQry += " (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where cast(\"OM_CODE\" as text)=SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + Section_code + "')) as \"SECTION\" ,TO_CHAR(\"DTE_TC_MANFDATE\",'MM/YYYY') \"DTE_TC_MANFDATE\",";
        //            strQry += " \"DTE_TANK_CAPACITY\",\"DTE_TC_WEIGHT\",TO_CHAR(\"ED_WELD_DATE\",'DD/MM/YYYY') \"ED_WELD_DATE\",\"ED_FEEDERCODE\" FROM ";
        //            strQry += " \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\",\"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"ED_ID\"= \"DTE_ED_ID\" ";
        //            strQry += " and \"ED_LOCTYPE\"='2' AND \"ED_STATUS_FLAG\" IN (1) AND  \"FD_FEEDER_CODE\"=\"ED_FEEDERCODE\" AND \"FD_FEEDER_ID\"=\"FDO_FEEDER_ID\" ";
        //            if (sFeederCode != "")
        //            {
        //                strQry += " AND \"ED_FEEDERCODE\" Like '" + sFeederCode + "%' ";
        //            }
        //            strQry += "AND cast(\"ED_OFFICECODE\" as text) LIKE '" + sOfficeCode + "%'";

        //            if (sFromEnumDate != "" && sToEnumDate != "")
        //            {
        //                DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //                DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //                strQry += " AND TO_CHAR(\"ED_WELD_DATE\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
        //            }
        //            strQry += " ORDER BY \"DTE_DTCCODE\"";
        //        }
        //        // QCWise Reject
        //        else if (sdatewise == "4")
        //        {
        //            strQry = "select DISTINCT \"DTE_DTCCODE\",\"DTE_CESCCODE\",\"DTE_IPCODE\",\"DTE_NAME\", (select \"TM_NAME\" from \"TBLTRANSMAKES\" where \"TM_ID\"=\"DTE_MAKE\")\"DTE_MAKE\",\"DTE_TC_CODE\",\"DTE_TC_SLNO\",\"DTE_CAPACITY\",";
        //            strQry += "(SELECT \"ZO_NAME\" FROM \"TBLZONE\" WHERE cast(\"ZO_ID\" as text)=SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'1')) as \"ZONE\" ,";
        //            strQry += "(SELECT \"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE cast(\"CM_CIRCLE_CODE\" as text)=SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'2')) as \"CIRCLE\" ,";
        //            strQry += "(SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE cast(\"DIV_CODE\" as text)=SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + Division_code + "')) as \"DIVISION\" ,";
        //            strQry += " \"FD_FEEDER_NAME\" as \"FEEDER\",";
        //            strQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE cast(\"SD_SUBDIV_CODE\" as text)=SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + SubDiv_code + "')) as \"SUBDIVISION\",";
        //            strQry += " (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where cast(\"OM_CODE\" as text)=SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + Section_code + "')) as \"SECTION\" ,TO_CHAR(\"DTE_TC_MANFDATE\",'MM/YYYY') \"DTE_TC_MANFDATE\",";
        //            strQry += " \"DTE_TANK_CAPACITY\",\"DTE_TC_WEIGHT\",TO_CHAR(\"ED_WELD_DATE\",'DD/MM/YYYY') \"ED_WELD_DATE\",\"ED_FEEDERCODE\" FROM ";
        //            strQry += " \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\",\"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"ED_ID\"= \"DTE_ED_ID\" ";
        //            strQry += " and \"ED_LOCTYPE\"='2' AND \"ED_STATUS_FLAG\" IN (3) AND  \"FD_FEEDER_CODE\"=\"ED_FEEDERCODE\" AND \"FD_FEEDER_ID\"=\"FDO_FEEDER_ID\" ";
        //            if (sFeederCode != "")
        //            {
        //                strQry += " AND \"ED_FEEDERCODE\" Like '" + sFeederCode + "%' ";
        //            }
        //            strQry += "AND cast(\"ED_OFFICECODE\" as text) LIKE '" + sOfficeCode + "%'";

        //            if (sFromEnumDate != "" && sToEnumDate != "")
        //            {
        //                DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //                DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //                strQry += " AND TO_CHAR(\"ED_WELD_DATE\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
        //            }
        //            strQry += " ORDER BY \"DTE_DTCCODE\"";
        //        }
        //        else if (sdatewise == "5")
        //        {
        //            strQry = "select DISTINCT \"DTE_DTCCODE\",\"DTE_CESCCODE\",\"DTE_IPCODE\",\"DTE_NAME\", (select \"TM_NAME\" from \"TBLTRANSMAKES\" where \"TM_ID\"=\"DTE_MAKE\")\"DTE_MAKE\",\"DTE_TC_CODE\",\"DTE_TC_SLNO\",\"DTE_CAPACITY\",";
        //            strQry += "(SELECT \"ZO_NAME\" FROM \"TBLZONE\" WHERE cast(\"ZO_ID\" as text)=SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'1')) as \"ZONE\" ,";
        //            strQry += "(SELECT \"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE cast(\"CM_CIRCLE_CODE\" as text)=SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'2')) as \"CIRCLE\" ,";
        //            strQry += "(SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE cast(\"DIV_CODE\" as text)=SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + Division_code + "')) as \"DIVISION\" ,";
        //            strQry += " \"FD_FEEDER_NAME\" as \"FEEDER\",";
        //            strQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE cast(\"SD_SUBDIV_CODE\" as text)=SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + SubDiv_code + "')) as \"SUBDIVISION\",";
        //            strQry += " (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where cast(\"OM_CODE\" as text)=SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + Section_code + "')) as \"SECTION\" ,TO_CHAR(\"DTE_TC_MANFDATE\",'MM/YYYY') \"DTE_TC_MANFDATE\",";
        //            strQry += " \"DTE_TANK_CAPACITY\",\"DTE_TC_WEIGHT\",TO_CHAR(\"ED_WELD_DATE\",'DD/MM/YYYY') \"ED_WELD_DATE\",\"ED_FEEDERCODE\" ";
        //            strQry += " ,case  when \"ED_STATUS_FLAG\"=0 then 'QCWise pending' when \"ED_STATUS_FLAG\"=2 then 'QCWise pending' when \"ED_STATUS_FLAG\"=1 then 'QCWise Done' when \"ED_STATUS_FLAG\"=3 then 'QCWise Reject' end as \"STATUS\" FROM ";
        //            strQry += " \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\",\"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"ED_ID\"= \"DTE_ED_ID\" ";
        //            strQry += " and \"ED_LOCTYPE\"='2' AND \"ED_STATUS_FLAG\" IN (0,1,2,3) AND  \"FD_FEEDER_CODE\"=\"ED_FEEDERCODE\" AND \"FD_FEEDER_ID\"=\"FDO_FEEDER_ID\" ";
        //            if (sFeederCode != "")
        //            {
        //                strQry += " AND \"ED_FEEDERCODE\" Like '" + sFeederCode + "%' ";
        //            }
        //            strQry += "AND cast(\"ED_OFFICECODE\" as text) LIKE '" + sOfficeCode + "%'";

        //            if (sFromEnumDate != "" && sToEnumDate != "")
        //            {
        //                DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //                DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //                strQry += " AND TO_CHAR(\"ED_WELD_DATE\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
        //            }
        //            strQry += " ORDER BY \"DTE_DTCCODE\"";
        //        }
        //        // else others
        //        else if (sdatewise == "3")
        //        {
        //            strQry = "SELECT \"DT_CODE\" AS \"DTE_DTCCODE\",'' AS \"DTE_CESCCODE\",'' AS \"DTE_IPCODE\", \"DT_NAME\" AS \"DTE_NAME\",'' AS \"DTE_MAKE\",";
        //            strQry += "'0' AS \"DTE_TC_CODE\",'' AS \"DTE_TC_SLNO\",'' AS \"DTE_CAPACITY\", ";
        //            strQry += "(SELECT \"ZO_NAME\" FROM \"TBLZONE\" WHERE cast(\"ZO_ID\" as text)=SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'1')) as \"ZONE\" ,";
        //            strQry += "(SELECT \"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE cast(\"CM_CIRCLE_CODE\" as text)=SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'2')) as \"CIRCLE\" ,";
        //            strQry += "(SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE cast(\"DIV_CODE\" as text)=SUBSTR(cast(\"DT_OM_SLNO\" as text),1,'" + Division_code + "')) as \"DIVISION\",\"FD_FEEDER_NAME\" as \"FEEDER\",(SELECT \"SD_SUBDIV_NAME\" FROM ";
        //            strQry += "\"TBLSUBDIVMAST\" WHERE cast(\"SD_SUBDIV_CODE\" as text)=SUBSTR(cast(\"DT_OM_SLNO\" as text),1,'" + SubDiv_code + "')) as \"SUBDIVISION\", (SELECT \"OM_NAME\" FROM ";
        //            strQry += "\"TBLOMSECMAST\" where cast(\"OM_CODE\" as text)=SUBSTR(cast(\"DT_OM_SLNO\" as text),1,'" + Section_code + "')) as \"SECTION\",'' AS \"DTE_TC_MANFDATE\",'' AS ";
        //            strQry += "\"DTE_TANK_CAPACITY\",'' AS \"DTE_TC_WEIGHT\",\"DT_CRON\" AS \"ED_WELD_DATE\", \"DT_FDRSLNO\" AS \"ED_FEEDERCODE\"  FROM ";
        //            strQry += "\"TBLDTCMAST\",\"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" WHERE \"DT_FDRSLNO\"=\"FD_FEEDER_CODE\" AND ";
        //            strQry += "\"FD_FEEDER_ID\"=\"FDO_FEEDER_ID\" AND \"DT_STATUS\"='1' AND cast(\"DT_OM_SLNO\" as text) LIKE '" + sOfficeCode + "%' ";
        //            if (sFeederCode != "")
        //            {
        //                strQry += " AND  CAST(\"DT_FDRSLNO\" AS TEXT) Like '" + sFeederCode + "%' ";
        //            }
        //            if (sFromEnumDate != "" && sToEnumDate != "")
        //            {
        //                DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //                DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //                strQry += " AND TO_CHAR(\"DT_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
        //            }
        //            strQry += " ORDER BY \"DT_CODE\"";
        //        }
        //        dtStoreDetails = ObjCon.FetchDataTable(strQry);
        //        return dtStoreDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return dtStoreDetails;

        //    }
        //}

        public DataTable PrintFieldDetails(string sFeederCode, string sOfficeCode, string sFromEnumDate, string sToEnumDate, string sdatewise)
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            DataTable dtStoreDetails = new DataTable("A");
            string strQry = string.Empty;
            try
            {
                //    if (sdatewise != "3")
                //    {

                //        strQry = "select DISTINCT \"DTE_DTCCODE\",\"DTE_CESCCODE\",\"DTE_IPCODE\",\"DTE_NAME\", (select \"TM_NAME\" from \"TBLTRANSMAKES\" where \"TM_ID\"=\"DTE_MAKE\") \"DTE_MAKE\",\"DTE_TC_CODE\",\"DTE_TC_SLNO\",\"DTE_CAPACITY\",";
                //        strQry += "(SELECT \"ZO_NAME\" FROM \"TBLZONE\" WHERE cast(\"ZO_ID\" as text)=SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'1')) as \"ZONE\" ,";
                //        strQry += "(SELECT \"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE cast(\"CM_CIRCLE_CODE\" as text)=SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'2')) as \"CIRCLE\" ,";
                //        strQry += "(SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE cast(\"DIV_CODE\" as text)=SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + Division_code + "')) as \"DIVISION\" ,";
                //        strQry += " \"FD_FEEDER_NAME\" as \"FEEDER\",";
                //        strQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE cast(\"SD_SUBDIV_CODE\" as text)=SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + SubDiv_code + "')) as \"SUBDIVISION\",";
                //        strQry += " (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where cast(\"OM_CODE\" as text)=SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + Section_code + "')) as \"SECTION\" ,TO_CHAR(\"DTE_TC_MANFDATE\",'MM/YYYY') \"DTE_TC_MANFDATE\",";
                //        strQry += " \"DTE_TANK_CAPACITY\",\"DTE_TC_WEIGHT\",TO_CHAR(\"ED_WELD_DATE\",'DD/MM/YYYY') \"ED_WELD_DATE\",\"ED_FEEDERCODE\" ";
                //        strQry += " ,case  when \"ED_STATUS_FLAG\"=0 then 'QCWise pending' when \"ED_STATUS_FLAG\"=2 then 'QCWise pending' when \"ED_STATUS_FLAG\"=1 then 'QCWise Done' when \"ED_STATUS_FLAG\"=3 then 'QCWise Reject' end as \"STATUS\" FROM ";
                //        strQry += " \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\",\"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"ED_ID\"= \"DTE_ED_ID\" and \"ED_LOCTYPE\"='2' ";

                //        switch (sdatewise)
                //        {
                //            case "1":
                //                strQry += " AND \"ED_STATUS_FLAG\" IN (0,2) ";
                //                break;
                //            case "2":
                //                strQry += " AND \"ED_STATUS_FLAG\" IN (1) ";
                //                break;
                //            case "4":
                //                strQry += " AND \"ED_STATUS_FLAG\" IN (3) ";
                //                break;
                //            case "5":
                //                strQry += " AND \"ED_STATUS_FLAG\" IN (0,1,2,3) ";
                //                break;

                //        }
                //        strQry += " AND  \"FD_FEEDER_CODE\"=\"ED_FEEDERCODE\" AND \"FD_FEEDER_ID\"=\"FDO_FEEDER_ID\" ";
                //        if (sFeederCode != "")
                //        {
                //            strQry += " AND \"ED_FEEDERCODE\" Like '" + sFeederCode + "%' ";
                //        }
                //        strQry += "AND cast(\"ED_OFFICECODE\" as text) LIKE '" + sOfficeCode + "%'";
                //        if (sFromEnumDate != "" && sToEnumDate != "")
                //        {
                //            DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //            DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //            strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //        }
                //        strQry += " ORDER BY \"DTE_DTCCODE\"";
                //    }
                //    else
                //    {
                //        strQry = "SELECT \"DT_CODE\" AS \"DTE_DTCCODE\",'' AS \"DTE_CESCCODE\",'' AS \"DTE_IPCODE\", \"DT_NAME\" AS \"DTE_NAME\",'' AS \"DTE_MAKE\",";
                //        strQry += "'0' AS \"DTE_TC_CODE\",'' AS \"DTE_TC_SLNO\",'' AS \"DTE_CAPACITY\", ";
                //        strQry += "(SELECT \"ZO_NAME\" FROM \"TBLZONE\" WHERE cast(\"ZO_ID\" as text)=SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'1')) as \"ZONE\" ,";
                //        strQry += "(SELECT \"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE cast(\"CM_CIRCLE_CODE\" as text)=SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'2')) as \"CIRCLE\" ,";
                //        strQry += "(SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE cast(\"DIV_CODE\" as text)=SUBSTR(cast(\"DT_OM_SLNO\" as text),1,'" + Division_code + "')) as \"DIVISION\",\"FD_FEEDER_NAME\" as \"FEEDER\",(SELECT \"SD_SUBDIV_NAME\" FROM ";
                //        strQry += "\"TBLSUBDIVMAST\" WHERE cast(\"SD_SUBDIV_CODE\" as text)=SUBSTR(cast(\"DT_OM_SLNO\" as text),1,'" + SubDiv_code + "')) as \"SUBDIVISION\", (SELECT \"OM_NAME\" FROM ";
                //        strQry += "\"TBLOMSECMAST\" where cast(\"OM_CODE\" as text)=SUBSTR(cast(\"DT_OM_SLNO\" as text),1,'" + Section_code + "')) as \"SECTION\",'' AS \"DTE_TC_MANFDATE\",'' AS ";
                //        strQry += "\"DTE_TANK_CAPACITY\",'' AS \"DTE_TC_WEIGHT\",\"DT_CRON\" AS \"ED_WELD_DATE\", \"DT_FDRSLNO\" AS \"ED_FEEDERCODE\" ";
                //        strQry += " ,case  when \"ED_STATUS_FLAG\"=0 then 'QCWise pending' when \"ED_STATUS_FLAG\"=2 then 'QCWise pending' when \"ED_STATUS_FLAG\"=1 then 'QCWise Done' when \"ED_STATUS_FLAG\"=3 then 'QCWise Reject' end as \"STATUS\" FROM ";
                //        strQry += "\"TBLDTCMAST\",\"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" WHERE \"DT_FDRSLNO\"=\"FD_FEEDER_CODE\" AND ";
                //        strQry += "\"FD_FEEDER_ID\"=\"FDO_FEEDER_ID\" AND \"DT_STATUS\"='1' AND cast(\"DT_OM_SLNO\" as text) LIKE '" + sOfficeCode + "%' ";
                //        if (sFeederCode != "")
                //        {
                //            strQry += " AND  CAST(\"DT_FDRSLNO\" AS TEXT) Like '" + sFeederCode + "%' ";
                //        }
                //        if (sFromEnumDate != "" && sToEnumDate != "")
                //        {
                //            DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //            DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //            strQry += " AND TO_CHAR(\"DT_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //        }
                //        strQry += " ORDER BY \"DT_CODE\"";
                //    }
                //    dtStoreDetails = ObjCon.FetchDataTable(strQry);
                //    return dtStoreDetails;
                //}



                string fromdate = string.Empty;
                string todate = string.Empty;

                if (sFromEnumDate != "" && sToEnumDate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    fromdate = dFromDate.ToString("yyyyMMdd");
                    todate = dToDate.ToString("yyyyMMdd");

                }

                NpgsqlCommand cmd = new NpgsqlCommand("sp_printfielddetails_clsreports");
                cmd.Parameters.AddWithValue("sdatewise", Convert.ToString(sdatewise == null ? "" : sdatewise));
                cmd.Parameters.AddWithValue("offcode", Convert.ToString(sOfficeCode == null ? "" : sOfficeCode));
                cmd.Parameters.AddWithValue("divcode", Convert.ToString(Division_code));
                cmd.Parameters.AddWithValue("subdivcode", Convert.ToString(SubDiv_code));
                cmd.Parameters.AddWithValue("sectioncode", Convert.ToString(Section_code));
                cmd.Parameters.AddWithValue("sfromenumdate", Convert.ToString(sFromEnumDate == null ? "" : sFromEnumDate));
                cmd.Parameters.AddWithValue("stoenumdate", Convert.ToString(sToEnumDate == null ? "" : sToEnumDate));
                cmd.Parameters.AddWithValue("sfeedercode", Convert.ToString(sFeederCode == null ? "" : sFeederCode));
                dtStoreDetails = ObjCon.FetchDataTable(cmd);
                return dtStoreDetails;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtStoreDetails;

            }
        }

        //public DataTable PrintStoreDetails(string sOfficeCode, string sFromEnumDate, string sToEnumDate, string sdatewise)
        //{
        //    PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));

        //    DataTable dtStoreDetails = new DataTable();
        //    string strQry = string.Empty;
        //    try
        //    {
        //        // QCWise pending
        //        if (sdatewise == "1")
        //        {
        //            strQry = " select (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"ED_LOCNAME\"=cast(\"SM_ID\" as text)) \"SM_NAME\",CASE WHEN \"ED_LOCTYPE\"=1 THEN 'STORE'  WHEN \"ED_LOCTYPE\"=3 THEN 'REPAIRER' WHEN \"ED_LOCTYPE\"=5 THEN 'TRANSFORMER BANK' END AS \"ENUM_TYPE\",TO_CHAR(\"DTE_TC_MANFDATE\",'MM/YYYY') \"DTE_TC_MANFDATE\",";
        //            strQry += " \"DTE_TANK_CAPACITY\",\"DTE_TC_WEIGHT\",\"DTE_TC_CODE\" as \"DTE_TC_CODE\",\"DTE_TC_SLNO\",\"DTE_CAPACITY\" as \"DTE_CAPACITY\",";
        //            strQry += " (select \"TM_NAME\" from \"TBLTRANSMAKES\" where CAST(\"TM_ID\" AS TEXT)=CAST(\"DTE_MAKE\" AS TEXT))\"DTE_MAKE\",";
        //            strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT)=SUBSTRING(CAST(\"ED_OFFICECODE\" AS TEXT),1,'" + Division_code + "')) as \"DIVISION\" ,";
        //            strQry += "(select \"FD_FEEDER_NAME\" from \"TBLFEEDERMAST\" where CAST(\"FD_FEEDER_CODE\" AS TEXT)=CAST(\"ED_FEEDERCODE\" AS TEXT))  as \"FEEDER\",";
        //            strQry += "(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT)=SUBSTRING(CAST(\"ED_OFFICECODE\" AS TEXT),1,'" + SubDiv_code + "')) as \"SUBDIVISION\", ";
        //            strQry += "(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where CAST(\"OM_CODE\" AS TEXT)=SUBSTRING(CAST(\"ED_OFFICECODE\" AS TEXT),1,'" + Section_code + "')) as \"SECTION\",TO_CHAR(\"ED_WELD_DATE\",'DD/MM/YYYY') \"ED_WELD_DATE\",";
        //            strQry += " CASE \"ED_LOCTYPE\" WHEN 1 THEN (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE CAST(\"ED_LOCNAME\" AS TEXT)=CAST(\"SM_ID\" AS TEXT)) WHEN 3 THEN(SELECT \"TR_NAME\" FROM \"TBLTRANSREPAIRER\" WHERE CAST(\"ED_LOCNAME\" AS TEXT)=CAST(\"TR_ID\" AS TEXT)) WHEN 5 THEN (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE CAST(\"ED_LOCNAME\" AS TEXT)=CAST(\"SM_ID\" AS TEXT)) END  \"LOCNAME\",";
        //            strQry += " (SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'DTR_TYPE' AND CAST(\"MD_ID\"  AS TEXT)= CAST(\"DTE_TC_TYPE\" AS TEXT)) AS \"TC_TYPE\" ";
        //            strQry += " ,case  when \"ED_STATUS_FLAG\"=0 then 'QCWise pending' when \"ED_STATUS_FLAG\"=2 then 'QCWise pending' when \"ED_STATUS_FLAG\"=1 then 'QCWise Done' when \"ED_STATUS_FLAG\"=3 then 'QCWise Reject' end as \"STATUS\" FROM ";
        //            strQry += " \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\" ,\"TBLSTOREMAST\"  where \"ED_LOCNAME\"=cast(\"SM_ID\" as text) and CAST(\"ED_ID\" AS TEXT)= CAST(\"DTE_ED_ID\" AS TEXT)  AND CAST(\"SM_CODE\" AS TEXT) Like '" + sOfficeCode + "%' and  \"ED_LOCTYPE\" IN ('1','3','5') AND  \"ED_STATUS_FLAG\" IN (0,2) ";

        //            if (sFromEnumDate != "" && sToEnumDate != "")
        //            {
        //                DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //                DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //                strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
        //            }
        //            strQry += " ORDER BY \"DTE_TC_CODE\"";
        //        }
        //        // QCWise Done
        //        else if (sdatewise == "2")
        //        {
        //            strQry = " select (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"ED_LOCNAME\"=cast(\"SM_ID\" as text)) \"SM_NAME\",CASE WHEN \"ED_LOCTYPE\"=1 THEN 'STORE'  WHEN \"ED_LOCTYPE\"=3 THEN 'REPAIRER' WHEN \"ED_LOCTYPE\"=5 THEN 'TRANSFORMER BANK' END AS \"ENUM_TYPE\",TO_CHAR(\"DTE_TC_MANFDATE\",'MM/YYYY') \"DTE_TC_MANFDATE\",";
        //            strQry += " \"DTE_TANK_CAPACITY\",\"DTE_TC_WEIGHT\",\"DTE_TC_CODE\" as \"DTE_TC_CODE\",\"DTE_TC_SLNO\",\"DTE_CAPACITY\" as \"DTE_CAPACITY\",";
        //            strQry += " (select \"TM_NAME\" from \"TBLTRANSMAKES\" where CAST(\"TM_ID\" AS TEXT)=CAST(\"DTE_MAKE\" AS TEXT))\"DTE_MAKE\",";
        //            strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT)=SUBSTRING(CAST(\"ED_OFFICECODE\" AS TEXT),1,'" + Division_code + "')) as \"DIVISION\" ,";
        //            strQry += "(select \"FD_FEEDER_NAME\" from \"TBLFEEDERMAST\" where CAST(\"FD_FEEDER_CODE\" AS TEXT)=CAST(\"ED_FEEDERCODE\" AS TEXT))  as \"FEEDER\",";
        //            strQry += "(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT)=SUBSTRING(CAST(\"ED_OFFICECODE\" AS TEXT),1,'" + SubDiv_code + "')) as \"SUBDIVISION\", ";
        //            strQry += "(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where CAST(\"OM_CODE\" AS TEXT)=SUBSTRING(CAST(\"ED_OFFICECODE\" AS TEXT),1,'" + Section_code + "')) as \"SECTION\",TO_CHAR(\"ED_WELD_DATE\",'DD/MM/YYYY') \"ED_WELD_DATE\",";
        //            strQry += " CASE \"ED_LOCTYPE\" WHEN 1 THEN (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE CAST(\"ED_LOCNAME\" AS TEXT)=CAST(\"SM_ID\" AS TEXT)) WHEN 3 THEN(SELECT \"TR_NAME\" FROM \"TBLTRANSREPAIRER\" WHERE CAST(\"ED_LOCNAME\" AS TEXT)=CAST(\"TR_ID\" AS TEXT)) WHEN 5 THEN (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE CAST(\"ED_LOCNAME\" AS TEXT)=CAST(\"SM_ID\" AS TEXT)) END  \"LOCNAME\",";
        //            strQry += " (SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'DTR_TYPE' AND CAST(\"MD_ID\"  AS TEXT)= CAST(\"DTE_TC_TYPE\" AS TEXT)) AS \"TC_TYPE\" ";
        //            strQry += " ,case  when \"ED_STATUS_FLAG\"=0 then 'QCWise pending' when \"ED_STATUS_FLAG\"=2 then 'QCWise pending' when \"ED_STATUS_FLAG\"=1 then 'QCWise Done' when \"ED_STATUS_FLAG\"=3 then 'QCWise Reject' end as \"STATUS\" FROM ";
        //            strQry += " \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\",\"TBLSTOREMAST\" where  \"ED_LOCNAME\"=cast(\"SM_ID\" as text) and CAST(\"ED_ID\" AS TEXT)= CAST(\"DTE_ED_ID\" AS TEXT)  AND CAST(\"SM_CODE\" AS TEXT) Like '" + sOfficeCode + "%' and  \"ED_LOCTYPE\" IN ('1','3','5') AND  \"ED_STATUS_FLAG\" IN (1) ";

        //            if (sFromEnumDate != "" && sToEnumDate != "")
        //            {
        //                DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //                DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //                strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
        //            }
        //            strQry += " ORDER BY \"DTE_TC_CODE\"";
        //        }
        //        // QCWise Reject
        //        else if (sdatewise == "4")
        //        {
        //            strQry = " select (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"ED_LOCNAME\"=cast(\"SM_ID\" as text)) \"SM_NAME\",CASE WHEN \"ED_LOCTYPE\"=1 THEN 'STORE'  WHEN \"ED_LOCTYPE\"=3 THEN 'REPAIRER' WHEN \"ED_LOCTYPE\"=5 THEN 'TRANSFORMER BANK' END AS \"ENUM_TYPE\",TO_CHAR(\"DTE_TC_MANFDATE\",'MM/YYYY') \"DTE_TC_MANFDATE\",";
        //            strQry += " \"DTE_TANK_CAPACITY\",\"DTE_TC_WEIGHT\",\"DTE_TC_CODE\" as \"DTE_TC_CODE\",\"DTE_TC_SLNO\",\"DTE_CAPACITY\" as \"DTE_CAPACITY\",";
        //            strQry += " (select \"TM_NAME\" from \"TBLTRANSMAKES\" where CAST(\"TM_ID\" AS TEXT)=CAST(\"DTE_MAKE\" AS TEXT))\"DTE_MAKE\",";
        //            strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT)=SUBSTRING(CAST(\"ED_OFFICECODE\" AS TEXT),1,'" + Division_code + "')) as \"DIVISION\" ,";
        //            strQry += "(select \"FD_FEEDER_NAME\" from \"TBLFEEDERMAST\" where CAST(\"FD_FEEDER_CODE\" AS TEXT)=CAST(\"ED_FEEDERCODE\" AS TEXT))  as \"FEEDER\",";
        //            strQry += "(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT)=SUBSTRING(CAST(\"ED_OFFICECODE\" AS TEXT),1,'" + SubDiv_code + "')) as \"SUBDIVISION\", ";
        //            strQry += "(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where CAST(\"OM_CODE\" AS TEXT)=SUBSTRING(CAST(\"ED_OFFICECODE\" AS TEXT),1,'" + Section_code + "')) as \"SECTION\",TO_CHAR(\"ED_WELD_DATE\",'DD/MM/YYYY') \"ED_WELD_DATE\",";
        //            strQry += " CASE \"ED_LOCTYPE\" WHEN 1 THEN (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE CAST(\"ED_LOCNAME\" AS TEXT)=CAST(\"SM_ID\" AS TEXT)) WHEN 3 THEN(SELECT \"TR_NAME\" FROM \"TBLTRANSREPAIRER\" WHERE CAST(\"ED_LOCNAME\" AS TEXT)=CAST(\"TR_ID\" AS TEXT)) WHEN 5 THEN (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE CAST(\"ED_LOCNAME\" AS TEXT)=CAST(\"SM_ID\" AS TEXT)) END  \"LOCNAME\",";
        //            strQry += " (SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'DTR_TYPE' AND CAST(\"MD_ID\"  AS TEXT)= CAST(\"DTE_TC_TYPE\" AS TEXT)) AS \"TC_TYPE\" ";
        //            strQry += " ,case  when \"ED_STATUS_FLAG\"=0 then 'QCWise pending' when \"ED_STATUS_FLAG\"=2 then 'QCWise pending' when \"ED_STATUS_FLAG\"=1 then 'QCWise Done' when \"ED_STATUS_FLAG\"=3 then 'QCWise Reject' end as \"STATUS\" FROM ";
        //            strQry += " \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\",\"TBLSTOREMAST\" where  \"ED_LOCNAME\"=cast(\"SM_ID\" as text) and  CAST(\"ED_ID\" AS TEXT)= CAST(\"DTE_ED_ID\" AS TEXT)  AND CAST(\"SM_CODE\" AS TEXT) Like '" + sOfficeCode + "%' and  \"ED_LOCTYPE\" IN ('1','3','5') AND  \"ED_STATUS_FLAG\" IN (3) ";

        //            if (sFromEnumDate != "" && sToEnumDate != "")
        //            {
        //                DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //                DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //                strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
        //            }
        //            strQry += " ORDER BY \"DTE_TC_CODE\"";
        //        }
        //        // for all details
        //        else if (sdatewise == "5")
        //        {
        //            strQry = " select (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"ED_LOCNAME\"=cast(\"SM_ID\" as text)) \"SM_NAME\",CASE WHEN \"ED_LOCTYPE\"=1 THEN 'STORE'  WHEN \"ED_LOCTYPE\"=3 THEN 'REPAIRER' WHEN \"ED_LOCTYPE\"=5 THEN 'TRANSFORMER BANK' END AS \"ENUM_TYPE\",TO_CHAR(\"DTE_TC_MANFDATE\",'MM/YYYY') \"DTE_TC_MANFDATE\",";
        //            strQry += " \"DTE_TANK_CAPACITY\",\"DTE_TC_WEIGHT\",\"DTE_TC_CODE\" as \"DTE_TC_CODE\",\"DTE_TC_SLNO\",\"DTE_CAPACITY\" as \"DTE_CAPACITY\",";
        //            strQry += " (select \"TM_NAME\" from \"TBLTRANSMAKES\" where CAST(\"TM_ID\" AS TEXT)=CAST(\"DTE_MAKE\" AS TEXT))\"DTE_MAKE\",";
        //            strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT)=SUBSTRING(CAST(\"ED_OFFICECODE\" AS TEXT),1,'" + Division_code + "')) as \"DIVISION\" ,";
        //            strQry += "(select \"FD_FEEDER_NAME\" from \"TBLFEEDERMAST\" where CAST(\"FD_FEEDER_CODE\" AS TEXT)=CAST(\"ED_FEEDERCODE\" AS TEXT))  as \"FEEDER\",";
        //            strQry += "(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT)=SUBSTRING(CAST(\"ED_OFFICECODE\" AS TEXT),1,'" + SubDiv_code + "')) as \"SUBDIVISION\", ";
        //            strQry += "(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where CAST(\"OM_CODE\" AS TEXT)=SUBSTRING(CAST(\"ED_OFFICECODE\" AS TEXT),1,'" + Section_code + "')) as \"SECTION\",TO_CHAR(\"ED_WELD_DATE\",'DD/MM/YYYY') \"ED_WELD_DATE\",";
        //            strQry += " CASE \"ED_LOCTYPE\" WHEN 1 THEN (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE CAST(\"ED_LOCNAME\" AS TEXT)=CAST(\"SM_ID\" AS TEXT)) WHEN 3 THEN(SELECT \"TR_NAME\" FROM \"TBLTRANSREPAIRER\" WHERE CAST(\"ED_LOCNAME\" AS TEXT)=CAST(\"TR_ID\" AS TEXT)) WHEN 5 THEN (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE CAST(\"ED_LOCNAME\" AS TEXT)=CAST(\"SM_ID\" AS TEXT)) END  \"LOCNAME\",";
        //            strQry += " (SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'DTR_TYPE' AND CAST(\"MD_ID\"  AS TEXT)= CAST(\"DTE_TC_TYPE\" AS TEXT)) AS \"TC_TYPE\" ";
        //            strQry += " ,case  when \"ED_STATUS_FLAG\"=0 then 'QCWise pending' when \"ED_STATUS_FLAG\"=2 then 'QCWise pending' when \"ED_STATUS_FLAG\"=1 then 'QCWise Done' when \"ED_STATUS_FLAG\"=3 then 'QCWise Reject' end as \"STATUS\" FROM ";
        //            strQry += " \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\",\"TBLSTOREMAST\" where  \"ED_LOCNAME\"=cast(\"SM_ID\" as text) and  CAST(\"ED_ID\" AS TEXT)= CAST(\"DTE_ED_ID\" AS TEXT)  AND CAST(\"SM_CODE\" AS TEXT) Like '" + sOfficeCode + "%' and  \"ED_LOCTYPE\" IN ('1','3','5') AND  \"ED_STATUS_FLAG\" IN (0,1,2,3) ";

        //            if (sFromEnumDate != "" && sToEnumDate != "")
        //            {
        //                DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //                DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //                strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
        //            }
        //            strQry += " ORDER BY \"DTE_TC_CODE\"";
        //        }
        //        else
        //        {
        //            // others DTC Without TC Added Report details
        //            strQry = " select (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"ED_LOCNAME\"=cast(\"SM_ID\" as text)) \"SM_NAME\",CASE WHEN \"ED_LOCTYPE\"=1 THEN 'STORE'  WHEN \"ED_LOCTYPE\"=3 THEN 'REPAIRER' WHEN \"ED_LOCTYPE\"=5 THEN 'TRANSFORMER BANK' END AS \"ENUM_TYPE\",TO_CHAR(\"DTE_TC_MANFDATE\",'MM/YYYY') \"DTE_TC_MANFDATE\",";
        //            strQry += " \"DTE_TANK_CAPACITY\",\"DTE_TC_WEIGHT\",\"DTE_TC_CODE\" \"DTE_TC_CODE\",\"DTE_TC_SLNO\",\"DTE_CAPACITY\" \"DTE_CAPACITY\",";
        //            strQry += " (select \"TM_NAME\" from \"TBLTRANSMAKES\" where \"TM_ID\"=\"DTE_MAKE\")\"DTE_MAKE\",";
        //            strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT)=SUBSTRING(CAST(\"ED_OFFICECODE\" AS TEXT),1,'" + Division_code + "')) as \"DIVISION\" ,";
        //            strQry += " (select \"FD_FEEDER_NAME\" from \"TBLFEEDERMAST\" where cast(\"FD_FEEDER_CODE\" as text)=cast(\"ED_FEEDERCODE\" as text))  as \"FEEDER\",";
        //            strQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE cast(\"SD_SUBDIV_CODE\" as text)=SUBSTRING(CAST(\"ED_OFFICECODE\" AS TEXT),1,'" + SubDiv_code + "')) as \"SUBDIVISION\",";
        //            strQry += " (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where CAST(\"OM_CODE\" AS TEXT)=SUBSTRING(CAST(\"ED_OFFICECODE\" AS TEXT),1,'" + Section_code + "')) as \"SECTION\" ,";
        //            strQry += " TO_CHAR(\"ED_WELD_DATE\",'DD/MM/YYYY') \"ED_WELD_DATE\", ";
        //            strQry += " CASE \"ED_LOCTYPE\" WHEN 1 THEN (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"ED_LOCNAME\"=cast(\"SM_ID\" as text)) WHEN 3 THEN(SELECT \"TR_NAME\" FROM \"TBLTRANSREPAIRER\" WHERE \"ED_LOCNAME\"=cast(\"TR_ID\" as text)) WHEN 5 THEN (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"ED_LOCNAME\"=cast(\"SM_ID\" as text)) END  \"LOCNAME\",";
        //            strQry += " (SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'DTR_TYPE' AND \"MD_ID\" = \"DTE_TC_TYPE\") AS \"TC_TYPE\" ";
        //            strQry += " ,case  when \"ED_STATUS_FLAG\"=0 then 'QCWise pending' when \"ED_STATUS_FLAG\"=2 then 'QCWise pending' when \"ED_STATUS_FLAG\"=1 then 'QCWise Done' when \"ED_STATUS_FLAG\"=3 then 'QCWise Reject' end as \"STATUS\" FROM ";
        //            strQry += " \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\" where \"ED_ID\"= \"DTE_ED_ID\"  AND CAST(\"ED_OFFICECODE\" AS TEXT) Like '" + sOfficeCode + "' and  \"ED_LOCTYPE\" IN ('1','3','5') AND  \"ED_STATUS_FLAG\" IN (0,2) ";
        //            if (sFromEnumDate != "" && sToEnumDate != "")
        //            {
        //                DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //                DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //                strQry += " AND TO_CHAR(\"ED_WELD_DATE\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
        //            }
        //            strQry += " UNION ALL";
        //            strQry += " SELECT (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"QA_LOCNAME\"=cast(\"SM_ID\" as text)) \"SM_NAME\",CASE WHEN \"QA_LOCTYPE\"=1 THEN 'STORE'  WHEN \"QA_LOCTYPE\"=3 THEN 'REPAIRER' WHEN \"QA_LOCTYPE\"=5 THEN 'TRANSFORMER BANK' END AS \"ENUM_TYPE\",TO_CHAR(\"QAO_TC_MANFDATE\",'MM/YYYY') \"DTE_TC_MANFDATE\", \"QAO_TANK_CAPACITY\",";
        //            strQry += " \"QAO_TC_WEIGHT\",\"QAO_TC_CODE\" \"QAO_TC_CODE\", \"QAO_TC_SLNO\",\"QAO_CAPACITY\" \"DTE_CAPACITY\",";
        //            strQry += " (select \"TM_NAME\" from \"TBLTRANSMAKES\" where \"TM_ID\"=\"QAO_MAKE\")\"DTE_MAKE\",";
        //            strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT)=SUBSTRING(CAST(\"QA_OFFICECODE\" AS TEXT),1,'" + Division_code + "')) as \"DIVISION\" ,";
        //            strQry += " (select \"FD_FEEDER_NAME\" from \"TBLFEEDERMAST\" where cast(\"FD_FEEDER_CODE\" as text)=cast(\"QA_FEEDERCODE\" as text))  as \"FEEDER\",";
        //            strQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE cast(\"SD_SUBDIV_CODE\" as text)=SUBSTRING(CAST(\"QA_OFFICECODE\" AS TEXT),1,'" + SubDiv_code + "')) as \"SUBDIVISION\",";
        //            strQry += " (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where CAST(\"OM_CODE\" AS TEXT)=SUBSTRING(CAST(\"QA_OFFICECODE\" AS TEXT),1,'" + Section_code + "')) as \"SECTION\" ,";
        //            strQry += " TO_CHAR(\"QA_WELD_DATE\",'DD/MM/YYYY') \"ED_WELD_DATE\", ";
        //            strQry += " CASE \"QA_LOCTYPE\" WHEN 1 THEN (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"QA_LOCNAME\"=cast(\"SM_ID\" as text)) WHEN 3 THEN(SELECT \"TR_NAME\" FROM \"TBLTRANSREPAIRER\" WHERE \"QA_LOCNAME\"=cast(\"TR_ID\" as text)) WHEN 5 THEN (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"QA_LOCNAME\"=CAST(\"SM_ID\" AS text)) END  \"LOCNAME\",";
        //            strQry += " (SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'DTR_TYPE' AND \"MD_ID\" = \"QAO_TC_TYPE\") AS \"TC_TYPE\" ";
        //            strQry += " FROM \"TBLQCAPPROVED\",\"TBLQCAPPROVEDOBJECTS\" WHERE \"QA_ID\"=\"QAO_QA_ID\" and cast(\"QA_OFFICECODE\" as text) Like '" + sOfficeCode + "%' and \"QA_LOCTYPE\" IN ('1','3','5')  ";
        //            if (sFromEnumDate != "" && sToEnumDate != "")
        //            {
        //                DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //                DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //                strQry += " AND TO_CHAR(\"QA_WELD_DATE\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
        //            }
        //            strQry += " ORDER BY \"DTE_TC_CODE\"";
        //        }
        //        dtStoreDetails = ObjCon.FetchDataTable(strQry);
        //        return dtStoreDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return dtStoreDetails;

        //    }
        //}
        NpgsqlCommand NpgsqlCommand;
        public DataTable LoadOfficeDet(clsReports obj)
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            DataTable DtStationDet = new DataTable();
            string[] strQryVallist = null;
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                string strQry = string.Empty;
                string Qry = string.Empty;
                if (obj.offcodeDT != "" && obj.offcodeDT != null)
                {
                    strQryVallist = obj.offcodeDT.Split(',');

                }
                strQry = "select \"OFF_CODE\",\"OFF_NAME\" FROM \"VIEW_ALL_OFFICES\" WHERE  \"OFF_NAME\" IS NOT NULL  ";
                if (obj.UserStatusFlag != "")
                {
                    strQry += " AND LENGTH(cast(\"OFF_CODE\" as text))='" + obj.UserStatusFlag + "'";
                }
                // strQry += " LENGTH(cast(\"OFF_CODE\" as text))='1'";
                if (obj.OfficeCode != "")
                {
                    NpgsqlCommand.Parameters.AddWithValue("officecode1", obj.OfficeCode);
                    strQry += " AND CAST(\"OFF_CODE\" AS TEXT) LIKE :officecode1||'%'";
                }
                else
                {
                    if (obj.offcodeDT != "" && obj.offcodeDT != null)
                    {
                        Qry = "AND (";
                        foreach (string officeCode in strQryVallist)
                        {
                            // NpgsqlCommand.Parameters.AddWithValue("officecode2", officeCode);
                            Qry += "  CAST(\"OFF_CODE\" AS TEXT) LIKE '" + officeCode + "%' OR";
                        }

                        Qry = Qry.Substring(0, (Qry.Length - 2));
                        Qry += " ) ";

                    }
                    strQry += Qry;
                }
                if (obj.OfficeName != "")
                {
                    NpgsqlCommand.Parameters.AddWithValue("officename1", obj.OfficeName.ToUpper());
                    strQry += " AND UPPER(CAST(\"OFF_NAME\" AS TEXT)) LIKE '%'|| :officename1||'%'";
                }
                strQry += " order by \"OFF_CODE\"";
                DtStationDet = ObjCon.FetchDataTable(strQry, NpgsqlCommand);

                return DtStationDet;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return DtStationDet;
            }
        }

        public DataTable PrintStoreDetails(string sOfficeCode, string sFromEnumDate, string sToEnumDate, string sdatewise)
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));

            DataTable dtStoreDetails = new DataTable();
            // DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            // DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string strQry = string.Empty;
            try
            {
                // QCWise pending
                //if (sdatewise != "3")
                //{
                //    strQry = " select (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"ED_LOCNAME\"=cast(\"SM_ID\" as text)) \"SM_NAME\",CASE WHEN \"ED_LOCTYPE\"=1 THEN 'STORE'  WHEN \"ED_LOCTYPE\"=3 THEN 'REPAIRER' WHEN \"ED_LOCTYPE\"=5 THEN 'TRANSFORMER BANK' END AS \"ENUM_TYPE\",TO_CHAR(\"DTE_TC_MANFDATE\",'MM/YYYY') \"DTE_TC_MANFDATE\",";
                //    strQry += " \"DTE_TANK_CAPACITY\",\"DTE_TC_WEIGHT\",\"DTE_TC_CODE\" as \"DTE_TC_CODE\",\"DTE_TC_SLNO\",\"DTE_CAPACITY\" as \"DTE_CAPACITY\",";
                //    strQry += " (select \"TM_NAME\" from \"TBLTRANSMAKES\" where CAST(\"TM_ID\" AS TEXT)=CAST(\"DTE_MAKE\" AS TEXT))\"DTE_MAKE\",";
                //    strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT)=SUBSTRING(CAST(\"ED_OFFICECODE\" AS TEXT),1,'" + Division_code + "')) as \"DIVISION\" ,";
                //    strQry += "(select \"FD_FEEDER_NAME\" from \"TBLFEEDERMAST\" where CAST(\"FD_FEEDER_CODE\" AS TEXT)=CAST(\"ED_FEEDERCODE\" AS TEXT))  as \"FEEDER\",";
                //    strQry += "(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT)=SUBSTRING(CAST(\"ED_OFFICECODE\" AS TEXT),1,'" + SubDiv_code + "')) as \"SUBDIVISION\", ";
                //    strQry += "(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where CAST(\"OM_CODE\" AS TEXT)=SUBSTRING(CAST(\"ED_OFFICECODE\" AS TEXT),1,'" + Section_code + "')) as \"SECTION\",TO_CHAR(\"ED_WELD_DATE\",'DD/MM/YYYY') \"ED_WELD_DATE\",";
                //    strQry += " CASE \"ED_LOCTYPE\" WHEN 1 THEN (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE CAST(\"ED_LOCNAME\" AS TEXT)=CAST(\"SM_ID\" AS TEXT)) WHEN 3 THEN(SELECT \"TR_NAME\" FROM \"TBLTRANSREPAIRER\" WHERE CAST(\"ED_LOCNAME\" AS TEXT)=CAST(\"TR_ID\" AS TEXT)) WHEN 5 THEN (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE CAST(\"ED_LOCNAME\" AS TEXT)=CAST(\"SM_ID\" AS TEXT)) END  \"LOCNAME\",";
                //    strQry += " (SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'DTR_TYPE' AND CAST(\"MD_ID\"  AS TEXT)= CAST(\"DTE_TC_TYPE\" AS TEXT)) AS \"TC_TYPE\" ";
                //    strQry += " ,case  when \"ED_STATUS_FLAG\"=0 then 'QCWise pending' when \"ED_STATUS_FLAG\"=2 then 'QCWise pending' when \"ED_STATUS_FLAG\"=1 then 'QCWise Done' when \"ED_STATUS_FLAG\"=3 then 'QCWise Reject' end as \"STATUS\" FROM ";
                //    strQry += " \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\" ,\"TBLSTOREMAST\"  where \"ED_LOCNAME\"=cast(\"SM_ID\" as text) and CAST(\"ED_ID\" AS TEXT)= CAST(\"DTE_ED_ID\" AS TEXT)  AND CAST(\"SM_CODE\" AS TEXT) Like '" + sOfficeCode + "%' and  \"ED_LOCTYPE\" IN ('1','3','5') ";
                //    // strQry+="  AND  \"ED_STATUS_FLAG\" IN (0,2) ";


                //    switch (sdatewise)
                //    {
                //        case "1":
                //            strQry += " AND \"ED_STATUS_FLAG\" IN (0,2) ";
                //            break;
                //        case "2":
                //            strQry += " AND \"ED_STATUS_FLAG\" IN (1) ";
                //            break;
                //        case "4":
                //            strQry += " AND \"ED_STATUS_FLAG\" IN (3) ";
                //            break;
                //        case "5":
                //            strQry += " AND \"ED_STATUS_FLAG\" IN (0,1,2,3) ";
                //            break;

                //    }

                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //        strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }
                //    strQry += " ORDER BY \"DTE_TC_CODE\"";
                //}
                //else
                //{
                //    // others DTC Without TC Added Report details
                //    strQry = " select (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"ED_LOCNAME\"=cast(\"SM_ID\" as text)) \"SM_NAME\",CASE WHEN \"ED_LOCTYPE\"=1 THEN 'STORE'  WHEN \"ED_LOCTYPE\"=3 THEN 'REPAIRER' WHEN \"ED_LOCTYPE\"=5 THEN 'TRANSFORMER BANK' END AS \"ENUM_TYPE\",TO_CHAR(\"DTE_TC_MANFDATE\",'MM/YYYY') \"DTE_TC_MANFDATE\",";
                //    strQry += " \"DTE_TANK_CAPACITY\",\"DTE_TC_WEIGHT\",\"DTE_TC_CODE\" \"DTE_TC_CODE\",\"DTE_TC_SLNO\",\"DTE_CAPACITY\" \"DTE_CAPACITY\",";
                //    strQry += " (select \"TM_NAME\" from \"TBLTRANSMAKES\" where \"TM_ID\"=\"DTE_MAKE\")\"DTE_MAKE\",";
                //    strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT)=SUBSTRING(CAST(\"ED_OFFICECODE\" AS TEXT),1,'" + Division_code + "')) as \"DIVISION\" ,";
                //    strQry += " (select \"FD_FEEDER_NAME\" from \"TBLFEEDERMAST\" where cast(\"FD_FEEDER_CODE\" as text)=cast(\"ED_FEEDERCODE\" as text))  as \"FEEDER\",";
                //    strQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE cast(\"SD_SUBDIV_CODE\" as text)=SUBSTRING(CAST(\"ED_OFFICECODE\" AS TEXT),1,'" + SubDiv_code + "')) as \"SUBDIVISION\",";
                //    strQry += " (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where CAST(\"OM_CODE\" AS TEXT)=SUBSTRING(CAST(\"ED_OFFICECODE\" AS TEXT),1,'" + Section_code + "')) as \"SECTION\" ,";
                //    strQry += " TO_CHAR(\"ED_WELD_DATE\",'DD/MM/YYYY') \"ED_WELD_DATE\", ";
                //    strQry += " CASE \"ED_LOCTYPE\" WHEN 1 THEN (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"ED_LOCNAME\"=cast(\"SM_ID\" as text)) WHEN 3 THEN(SELECT \"TR_NAME\" FROM \"TBLTRANSREPAIRER\" WHERE \"ED_LOCNAME\"=cast(\"TR_ID\" as text)) WHEN 5 THEN (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"ED_LOCNAME\"=cast(\"SM_ID\" as text)) END  \"LOCNAME\",";
                //    strQry += " (SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'DTR_TYPE' AND \"MD_ID\" = \"DTE_TC_TYPE\") AS \"TC_TYPE\" ";
                //    strQry += " ,case  when \"ED_STATUS_FLAG\"=0 then 'QCWise pending' when \"ED_STATUS_FLAG\"=2 then 'QCWise pending' when \"ED_STATUS_FLAG\"=1 then 'QCWise Done' when \"ED_STATUS_FLAG\"=3 then 'QCWise Reject' end as \"STATUS\" FROM ";
                //    strQry += " \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\" where \"ED_ID\"= \"DTE_ED_ID\"  AND CAST(\"ED_OFFICECODE\" AS TEXT) Like '" + sOfficeCode + "' and  \"ED_LOCTYPE\" IN ('1','3','5') AND  \"ED_STATUS_FLAG\" IN (0,2) ";
                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //        strQry += " AND TO_CHAR(\"ED_WELD_DATE\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }
                //    strQry += " UNION ALL";
                //    strQry += " SELECT (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"QA_LOCNAME\"=cast(\"SM_ID\" as text)) \"SM_NAME\",CASE WHEN \"QA_LOCTYPE\"=1 THEN 'STORE'  WHEN \"QA_LOCTYPE\"=3 THEN 'REPAIRER' WHEN \"QA_LOCTYPE\"=5 THEN 'TRANSFORMER BANK' END AS \"ENUM_TYPE\",TO_CHAR(\"QAO_TC_MANFDATE\",'MM/YYYY') \"DTE_TC_MANFDATE\", \"QAO_TANK_CAPACITY\",";
                //    strQry += " \"QAO_TC_WEIGHT\",\"QAO_TC_CODE\" \"QAO_TC_CODE\", \"QAO_TC_SLNO\",\"QAO_CAPACITY\" \"DTE_CAPACITY\",";
                //    strQry += " (select \"TM_NAME\" from \"TBLTRANSMAKES\" where \"TM_ID\"=\"QAO_MAKE\")\"DTE_MAKE\",";
                //    strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT)=SUBSTRING(CAST(\"QA_OFFICECODE\" AS TEXT),1,'" + Division_code + "')) as \"DIVISION\" ,";
                //    strQry += " (select \"FD_FEEDER_NAME\" from \"TBLFEEDERMAST\" where cast(\"FD_FEEDER_CODE\" as text)=cast(\"QA_FEEDERCODE\" as text))  as \"FEEDER\",";
                //    strQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE cast(\"SD_SUBDIV_CODE\" as text)=SUBSTRING(CAST(\"QA_OFFICECODE\" AS TEXT),1,'" + SubDiv_code + "')) as \"SUBDIVISION\",";
                //    strQry += " (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where CAST(\"OM_CODE\" AS TEXT)=SUBSTRING(CAST(\"QA_OFFICECODE\" AS TEXT),1,'" + Section_code + "')) as \"SECTION\" ,";
                //    strQry += " TO_CHAR(\"QA_WELD_DATE\",'DD/MM/YYYY') \"ED_WELD_DATE\", ";
                //    strQry += " CASE \"QA_LOCTYPE\" WHEN 1 THEN (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"QA_LOCNAME\"=cast(\"SM_ID\" as text)) WHEN 3 THEN(SELECT \"TR_NAME\" FROM \"TBLTRANSREPAIRER\" WHERE \"QA_LOCNAME\"=cast(\"TR_ID\" as text)) WHEN 5 THEN (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"QA_LOCNAME\"=CAST(\"SM_ID\" AS text)) END  \"LOCNAME\",";
                //    strQry += " (SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'DTR_TYPE' AND \"MD_ID\" = \"QAO_TC_TYPE\") AS \"TC_TYPE\" ";
                //    strQry += " FROM \"TBLQCAPPROVED\",\"TBLQCAPPROVEDOBJECTS\" WHERE \"QA_ID\"=\"QAO_QA_ID\" and cast(\"QA_OFFICECODE\" as text) Like '" + sOfficeCode + "%' and \"QA_LOCTYPE\" IN ('1','3','5')  ";
                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //        strQry += " AND TO_CHAR(\"QA_WELD_DATE\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }
                //    strQry += " ORDER BY \"DTE_TC_CODE\"";

                //    dtStoreDetails = ObjCon.FetchDataTable(strQry);
                //    return dtStoreDetails;
                string fromdate = string.Empty;
                string todate = string.Empty;

                if (sFromEnumDate != "" && sToEnumDate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    fromdate = dFromDate.ToString("yyyyMMdd");
                    todate = dToDate.ToString("yyyyMMdd");

                }
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getprint_storedetails_clsreports");
                cmd.Parameters.AddWithValue("sdatewise", Convert.ToString(sdatewise == null ? "" : sdatewise));
                cmd.Parameters.AddWithValue("offcode", Convert.ToString(sOfficeCode == null ? "" : sOfficeCode));
                cmd.Parameters.AddWithValue("divcode", Convert.ToString(Division_code));
                cmd.Parameters.AddWithValue("subdivcode", Convert.ToString(SubDiv_code));
                cmd.Parameters.AddWithValue("sectioncode", Convert.ToString(Section_code));
                cmd.Parameters.AddWithValue("sfromenumdate", Convert.ToString(sFromEnumDate == null ? "" : sFromEnumDate));
                cmd.Parameters.AddWithValue("stoenumdate", Convert.ToString(sToEnumDate == null ? "" : sToEnumDate));
                dtStoreDetails = ObjCon.FetchDataTable(cmd);
                return dtStoreDetails;


            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtStoreDetails;

            }
            // return dtStoreDetails;
        }

        #endregion



        public DataTable GetCircleWiseDTrRepairerReplacement(clsReports objReport)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_timetakenfordtrreplacement_report");
                cmd.Parameters.AddWithValue("from_date",objReport.sFromDate);
                cmd.Parameters.AddWithValue("to_date", objReport.sTodate);
                cmd.Parameters.AddWithValue("offcode", Convert.ToString(objReport.sOfficeCode ?? ""));
                dt = ObjCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }
        }

        public DataTable GetEstimationDetails(string EstId)
        {
            DataTable dtest = new DataTable();
            string sQry = string.Empty;
            try
            {
                sQry = " SELECT DISTINCT \"EST_ID\", CAST(\"EST_NO\" AS TEXT), CAST( \"EST_FAILUREID\" AS TEXT), \"TM_NAME\", \"OM_NAME\", \"EST_CRON\", CAST( \"EST_CAPACITY\" AS TEXT), \"TR_NAME\", ";
                sQry += " \"MRIM_REMARKS\", \"MRIM_ITEM_NAME\",\"MRIM_ITEM_ID\",\"MD_NAME\", \"ESTM_ITEM_QNTY\", \"ESTM_ITEM_RATE\", ";
                sQry += " \"ESTM_ITEM_TOTAL\" - \"ESTM_ITEM_TAX\" \"AMOUNT\", \"ESTM_ITEM_TAX\" , \"ESTM_ITEM_TOTAL\"  FROM \"TBLESTIMATIONDETAILS\" ";
                sQry += " JOIN \"TBLESTIMATIONMATERIAL\" ON \"EST_ID\" = \"ESTM_EST_ID\" JOIN \"TBLTRANSREPAIRER\" ON\"EST_REPAIRER\" = \"TR_ID\" ";
                sQry += " JOIN \"TBLDTCFAILURE\" ON \"DF_ID\" = \"EST_FAILUREID\" JOIN \"TBLTCMASTER\" ON \"DF_EQUIPMENT_ID\" = \"TC_CODE\" JOIN ";
                sQry += " \"TBLTRANSMAKES\" ON \"TC_MAKE_ID\" =  \"TM_ID\" JOIN \"TBLOMSECMAST\" ON \"DF_LOC_CODE\" = \"OM_CODE\" JOIN ";
                sQry += " (SELECT \"MRIM_ID\", \"MRIM_ITEM_NAME\",\"MRIM_ITEM_ID\", \"MD_NAME\",  \"MRI_TR_ID\", \"MRIM_REMARKS\" , \"MRI_CAPACITY\" FROM \"TBLMINORREPAIRERITEMMASTER\", ";
                sQry += " \"TBLMINORREPAIRITEMRATEMASTER\", \"TBLMASTERDATA\",\"TBLTRANSREPAIRER\" WHERE \"MD_TYPE\"='MSR' AND \"MRIM_ID\" = \"MRI_MRIM_ID\" AND ";
                sQry += " CAST(\"MRI_MEASUREMENT\" AS TEXT) = CAST(\"MD_ID\" AS TEXT)and \"TR_ID\"=\"MRI_TR_ID\")A ON \"ESTM_ITEM_ID\" = \"MRIM_ID\" WHERE \"EST_ID\" = '" + EstId + "' ";
                sQry += "AND CAST(\"EST_CAPACITY\" AS TEXT) = (SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'C' AND \"MD_ID\" = A.\"MRI_CAPACITY\") and \"TR_ID\"=\"MRI_TR_ID\"";
                dtest = ObjCon.FetchDataTable(sQry);
                return dtest;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtest;
            }
        }

        public DataTable PrintFeederBifurcationReportSO(clsReports objReport)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;

                strQry = "SELECT  (SELECT  \"FD_FEEDER_CODE\" ||'-'|| \"FD_FEEDER_NAME\"  FROM \"TBLFEEDERMAST\" WHERE \"FBDS_OLD_FEEDER_CODE\" = \"FD_FEEDER_CODE\" ) as \"OLDFEEDERCODE\"  ," +
                 " (SELECT  \"ST_STATION_CODE\" || '-' || \"ST_NAME\"  FROM \"TBLSTATION\", \"TBLFEEDERMAST\" WHERE \"FD_ST_ID\" = \"ST_ID\" AND \"FD_FEEDER_CODE\" = \"FBDS_OLD_FEEDER_CODE\") \"OLD_STATION\" , " +
                "(SELECT  \"ST_STATION_CODE\" || '-' || \"ST_NAME\"  FROM \"TBLSTATION\", \"TBLFEEDERMAST\" WHERE \"FD_ST_ID\" = \"ST_ID\" AND \"FD_FEEDER_CODE\" = \"FBDS_NEW_FEEDER_CODE\") \"NEW_STATION\" , " +
                  "(SELECT  \"FD_FEEDER_CODE\" ||'-'|| \"FD_FEEDER_NAME\"  FROM \"TBLFEEDERMAST\" WHERE \"FBDS_NEW_FEEDER_CODE\"  = \"FD_FEEDER_CODE\" ) as \"NEWFEEDERCODE\" ," +
                  "(SELECT SUBSTR(CAST(\"OFF_NAME\" AS TEXT), POSITION(':' in cast(\"OFF_NAME\" as TEXT))+1)  from \"VIEW_ALL_OFFICES\" WHERE  cast(\"OFF_CODE\" as text) =  SUBSTR(CAST(\"FBS_SECTION_CODE\" AS text), 0, 1)) as \"CIRCLE\" , " +
                   "(SELECT SUBSTR(CAST(\"OFF_NAME\" AS TEXT), POSITION(':' in cast(\"OFF_NAME\" as TEXT))+1)  from \"VIEW_ALL_OFFICES\" WHERE cast(\"OFF_CODE\" as text)=  SUBSTR(CAST(\"FBS_SECTION_CODE\" AS text), 0, 2)) as \"DIVISION\" ," +
                    "(SELECT SUBSTR(CAST(\"OFF_NAME\" AS TEXT), POSITION(':' in cast(\"OFF_NAME\" as TEXT))+1)  from \"VIEW_ALL_OFFICES\" WHERE  cast(\"OFF_CODE\" as text) =  SUBSTR(CAST(\"FBS_SECTION_CODE\" AS text), 0, 3)) as \"SUBDIVISON\" ," +
                    "(SELECT SUBSTR(CAST(\"OFF_NAME\" AS TEXT), POSITION(':' in cast(\"OFF_NAME\" as TEXT))+1)  from \"VIEW_ALL_OFFICES\" WHERE  cast(\"OFF_CODE\" as text) = CAST(\"FBS_SECTION_CODE\" AS text)) as \"Section\"," +
                     " to_char(\"FBDS_CRON\"  ,  'dd-MON-yyyy') as \"CREATEDON\" , \"FBDS_OLD_DTC_CODE\"  , \"FBDS_NEW_DTC_CODE\"  ,  \"FBDS_DTR_CODE\" as \"DTRCODE\" , \"DT_NAME\" , \"FBS_SECTION_CODE\"," +
                     "(select \"US_FULL_NAME\" ||', \"Role\": '||\"RO_NAME\"   FROM \"TBLUSER\" ,\"TBLROLES\"  WHERE \"US_ID\" = \"FBS_CR_BY\" AND \"US_ROLE_ID\" = \"RO_ID\" ) as \"CREATEDBY\" ," +
                     "(select \"US_FULL_NAME\" ||', \"Role\": '||\"RO_NAME\"   FROM \"TBLUSER\" ,\"TBLROLES\"  WHERE \"US_ID\" = \"FBS_APP_BY\" AND \"US_ROLE_ID\" = \"RO_ID\" ) as \"APPROVEDBY\" ," +
                  "CASE \"FBS_STATUS\" WHEN 0 THEN  'PENDING' WHEN 1 THEN 'APPROVED' WHEN 2 THEN 'FEEDER BIFURCATED' END AS \"STATUS\" " +
                  "FROM \"TBLFEEDER_BFCN_DETAILS_SO\"  INNER join \"TBLFEEDERBIFURCATION_SO\" on  \"FBDS_FB_ID\"  = \"FBS_ID\" " +
              "inner join   \"TBLDTCMAST\" on  ( \"FBDS_OLD_DTC_CODE\"  = \"DT_CODE\"  ) or (\"FBDS_NEW_DTC_CODE\"   = \"DT_CODE\")   WHERE \"FBS_ID\" = '" + objReport.sFeederBifurcationID + "'";

                dt = ObjCon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "PrintFeederBifurcationReportSO");
                return dt;
            }

        }


        public DataTable GetEstimationDetailsrepairer(string EstId)
        {
            DataTable dtest = new DataTable();
            string sQry = string.Empty;
            try
            {
                sQry = " SELECT DISTINCT \"RESTD_ID\",CAST(\"RESTD_NO\" AS TEXT), \"TM_NAME\", \"OM_NAME\", \"RESTD_CRON\", CAST( \"RESTD_CAPACITY\" AS TEXT),  ";
                sQry += " (case when \"RESTD_COIL_TYPE\" = '1' then 'SINGLE COIL' WHEN \"RESTD_COIL_TYPE\" = '2' then 'MULTI COIL' END ) as \"RESTD_COIL_TYPE\", ";
                sQry += " (CASE \"RESTD_PHASE\" WHEN  '1' THEN   'R-PHASE'WHEN  '2' THEN    'Y-PHASE' WHEN  '3' THEN    'B-PHASE' WHEN  '1,2' THEN   'R-PHASE,Y-PHASE' ";
                sQry += " WHEN  '1,3' THEN    'R-PHASE,B-PHASE' WHEN  '2,3' THEN   'Y-PHASE,B-PHASE' WHEN  '1,2,3' THEN   'R-PHASE,Y-PHASE,B-PHASE' END) as \"RESTD_PHASE\",";
                sQry += " \"TR_NAME\",\"MRIM_REMARKS\", \"MRIM_ITEM_NAME\",\"MRIM_ITEM_ID\",\"MD_NAME\", \"RESTM_ITEM_QNTY\", \"RESTM_ITEM_RATE\", ";
                sQry += " \"RESTM_ITEM_TOTAL\" - \"RESTM_ITEM_TAX\" \"AMOUNT\", \"RESTM_ITEM_TAX\" , \"RESTM_ITEM_TOTAL\"  FROM \"TBLREPAIRERESTIMATIONDETAILS\" ";
                sQry += " JOIN \"TBLREPAIRERESTIMATIONMATERIAL\" ON \"RESTD_ID\" = \"RESTM_EST_ID\" JOIN \"TBLTRANSREPAIRER\" ON\"RESTD_REPAIRER\" = \"TR_ID\" ";
                sQry += "  JOIN \"TBLTCMASTER\" ON \"RESTD_TC_CODE\" = \"TC_CODE\" JOIN ";
                sQry += " \"TBLTRANSMAKES\" ON \"TC_MAKE_ID\" =  \"TM_ID\" JOIN \"TBLOMSECMAST\" ON \"RESTD_OFF_CODE\" =cast(\"OM_CODE\" as text) JOIN ";
                sQry += " (SELECT \"MRIM_ID\", \"MRIM_ITEM_NAME\",\"MRIM_ITEM_ID\", \"MD_NAME\",  \"MRI_TR_ID\", \"MRIM_REMARKS\" , \"MRI_CAPACITY\" FROM \"TBLMINORREPAIRERITEMMASTER\", ";
                sQry += " \"TBLMINORREPAIRITEMRATEMASTER\", \"TBLMASTERDATA\",\"TBLTRANSREPAIRER\" WHERE \"MD_TYPE\"='MSR' AND \"MRIM_ID\" = \"MRI_MRIM_ID\" AND ";
                sQry += " CAST(\"MRI_MEASUREMENT\" AS TEXT) = CAST(\"MD_ID\" AS TEXT)and \"TR_ID\"=\"MRI_TR_ID\")A ON \"RESTM_ITEM_ID\" = \"MRIM_ID\" WHERE \"RESTD_ID\" = '" + EstId + "' ";
                sQry += "AND CAST(\"RESTD_CAPACITY\" AS TEXT) = (SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'C' AND \"MD_ID\" = A.\"MRI_CAPACITY\") and \"TR_ID\"=\"MRI_TR_ID\"";
                dtest = ObjCon.FetchDataTable(sQry);
                return dtest;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtest;
            }
        }


        public DataTable GetEstimationAmount(string EstId)
        {
            string StrQry = string.Empty;
            DataTable dtest = new DataTable();
            try
            {
                StrQry = "SELECT \"DF_EQUIPMENT_ID\" AS \"DTR_CODE\",SUM(CASE \"MRIM_ITEM_TYPE\" WHEN  '1'  THEN \"ESTM_ITEM_TOTAL\" - \"ESTM_ITEM_TAX\" ELSE 0 END + ";
                StrQry += " CASE \"MRIM_ITEM_TYPE\" WHEN '2' THEN \"ESTM_ITEM_TOTAL\" - \"ESTM_ITEM_TAX\" ELSE 0 END - ";
                StrQry += " CASE \"MRIM_ITEM_TYPE\" WHEN '3' THEN \"ESTM_ITEM_TOTAL\" - \"ESTM_ITEM_TAX\" ELSE 0 END) \"AMOUNT\",";
                StrQry += " SUM(  CASE \"MRIM_ITEM_TYPE\" WHEN  '1'  THEN  \"ESTM_ITEM_TAX\" ELSE 0 END + ";
                StrQry += " CASE \"MRIM_ITEM_TYPE\" WHEN '2' THEN  \"ESTM_ITEM_TAX\" ELSE 0 END - ";
                StrQry += " CASE \"MRIM_ITEM_TYPE\" WHEN '3' THEN  \"ESTM_ITEM_TAX\" ELSE 0 END)  \"TAX\", ";
                StrQry += " SUM( CASE \"MRIM_ITEM_TYPE\" WHEN  '1'  THEN  \"ESTM_ITEM_TOTAL\" ELSE 0 END + ";
                StrQry += " CASE \"MRIM_ITEM_TYPE\" WHEN '2' THEN  \"ESTM_ITEM_TOTAL\" ELSE 0 END - ";
                StrQry += " CASE \"MRIM_ITEM_TYPE\" WHEN '3' THEN  \"ESTM_ITEM_TOTAL\" ELSE 0 END)  \"TOTALAMOUNT\" ";
                StrQry += " FROM (SELECT DISTINCT \"EST_ID\", \"EST_NO\", \"EST_FAILUREID\", \"TM_NAME\", \"OM_NAME\", \"EST_CRON\",  \"EST_CAPACITY\", ";
                StrQry += " \"TR_NAME\", \"MRIM_ITEM_TYPE\", \"MRIM_REMARKS\", \"MRIM_ITEM_NAME\",\"MRIM_ITEM_ID\",\"MD_NAME\", \"ESTM_ITEM_QNTY\", \"ESTM_ITEM_RATE\",  ";
                StrQry += " \"ESTM_ITEM_TOTAL\" - \"ESTM_ITEM_TAX\" \"AMOUNT\", \"ESTM_ITEM_TAX\" , \"ESTM_ITEM_TOTAL\",\"DF_EQUIPMENT_ID\"  FROM \"TBLESTIMATIONDETAILS\"  ";
                StrQry += " JOIN \"TBLESTIMATIONMATERIAL\" ON \"EST_ID\" = \"ESTM_EST_ID\" JOIN \"TBLTRANSREPAIRER\" ON\"EST_REPAIRER\" = \"TR_ID\"  ";
                StrQry += " JOIN \"TBLDTCFAILURE\" ON \"DF_ID\" = \"EST_FAILUREID\" JOIN \"TBLTCMASTER\" ON \"DF_EQUIPMENT_ID\" = \"TC_CODE\" JOIN ";
                StrQry += " \"TBLTRANSMAKES\" ON \"TC_MAKE_ID\" =  \"TM_ID\" JOIN \"TBLOMSECMAST\" ON \"DF_LOC_CODE\" = \"OM_CODE\" JOIN ";
                StrQry += " (SELECT DISTINCT \"MRIM_ID\", \"MRIM_ITEM_TYPE\",\"MRIM_ITEM_NAME\",\"MRIM_ITEM_ID\", \"MD_NAME\",  \"MRI_TR_ID\", \"MRIM_REMARKS\",\"MRI_CAPACITY\" FROM  ";
                StrQry += " \"TBLMINORREPAIRERITEMMASTER\",  \"TBLMINORREPAIRITEMRATEMASTER\", \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='MSR' AND ";
                StrQry += " \"MRIM_ID\" = \"MRI_MRIM_ID\" AND  CAST(\"MRI_MEASUREMENT\" AS TEXT) = CAST(\"MD_ID\" AS TEXT))A ON \"ESTM_ITEM_ID\" = ";
                StrQry += " \"MRIM_ID\"    WHERE \"EST_ID\" = '" + EstId + "' AND CAST(\"EST_CAPACITY\" AS TEXT) = (SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" ";
                StrQry += " WHERE \"MD_TYPE\" = 'C' AND \"MD_ID\" = A.\"MRI_CAPACITY\")AND \"EST_REPAIRER\" = \"MRI_TR_ID\")Z  GROUP BY \"DF_EQUIPMENT_ID\"";
                dtest = ObjCon.FetchDataTable(StrQry);
                return dtest;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtest;
            }
        }

        public DataTable PrintFeederBifurcationReport(clsReports objReport)
        {
            DataTable dt = new DataTable();

            try
            {
                string strQry = string.Empty;

                strQry = "SELECT";
                strQry += " (SELECT \"CM_CIRCLE_NAME\"  FROM \"TBLCIRCLE\" WHERE cast(\"CM_CIRCLE_CODE\" as text) = SUBSTR(cast(\"DT_OM_SLNO\" as text), 0,1 )) \"CIRCLE\" , ";
                strQry += " (SELECT \"DIV_NAME\"  			FROM \"TBLDIVISION\" WHERE cast(\"DIV_CODE\" as text) = SUBSTR(cast(\"DT_OM_SLNO\" as text), 0,2 )) \"DIVISION\" , ";
                strQry += " (SELECT \"SD_SUBDIV_NAME\"	FROM \"TBLSUBDIVMAST\" WHERE cast(\"SD_SUBDIV_CODE\" as text) =SUBSTR(cast(\"DT_OM_SLNO\"as text), 0,3 )) \"SUBDIVISION\" , ";
                strQry += " (SELECT \"OM_NAME\"					FROM \"TBLOMSECMAST\" WHERE \"OM_CODE\" = \"DT_OM_SLNO\") \"SECTION\", ";
                strQry += "(SELECT \"FD_FEEDER_NAME\"  FROM \"TBLFEEDERMAST\" WHERE \"FD_FEEDER_CODE\" = \"FBD_OLD_FEEDER_CODE\" ) \"FBD_OLD_FEEDER_NAME\" ,";
                strQry += " (SELECT \"FD_FEEDER_NAME\"  FROM \"TBLFEEDERMAST\" WHERE \"FD_FEEDER_CODE\" = \"FBD_NEW_FEEDER_CODE\" ) \"FBD_NEW_FEEDER_NAME\" , ";
                strQry += " (SELECT \"DT_NAME\"  FROM \"TBLDTCMAST\" WHERE \"DT_CODE\" = \"FBD_NEW_DTC_CODE\" )\"DTC_NAME\" , ";
                strQry += " \"DT_OM_SLNO\", TO_CHAR(\"FB_OM_DATE\",'DD-MON-YYYY')\"FB_OM_DATE\"  ,\"FBD_OLD_FEEDER_CODE\" , \"FBD_NEW_FEEDER_CODE\",\"FBD_OLD_DTC_CODE\" , \"FBD_NEW_DTC_CODE\" ,\"FBD_DTR_CODE\" AS \"DTR_CODE\", \"ED_ID\", ";
                strQry += " (CASE WHEN \"ED_IS_INTERNAL_ENUM\" = 0 THEN 'PENDING' ELSE 'COMPLETED' end ) \"ENUMSTATUS\" ";
                strQry += " FROM \"TBLFEEDER_BIFURCATION_DETAILS\" left join \"TBLFEEDERBIFURCATION\" ON \"FB_ID\" = \"FBD_FB_ID\"  left join  \"TBLDTCMAST\" on \"FBD_NEW_DTC_CODE\" = \"DT_CODE\" ";
                strQry += " LEFT join \"TBLDTCENUMERATION\"  on \"DTE_DTCCODE\" = \"FBD_NEW_DTC_CODE\" LEFT JOIN \"TBLENUMERATIONDETAILS\" ON  \"ED_ID\" = \"DTE_ED_ID\"  WHERE \"ED_IS_FEEDER_BIFURCATION\"  = 1  ";

                if (!(objReport.sReportType == null || objReport.sReportType == ""))
                {
                    strQry += " AND \"ED_IS_INTERNAL_ENUM\" IN (" + objReport.sReportType + ") ";
                }

                if (!(objReport.sOfficeCode == null || objReport.sOfficeCode == ""))
                {
                    strQry += " AND \"DT_OM_SLNO\" LIKE '" + objReport.sOfficeCode + "%' ";
                }
                if (!(objReport.sOldFeederCode == null || objReport.sOldFeederCode == ""))
                {
                    strQry += " AND \"FBD_OLD_FEEDER_CODE\" = '" + objReport.sOldFeederCode + "'";
                }
                if (!(objReport.sNewFeederCode == null || objReport.sNewFeederCode == ""))
                {
                    strQry += " AND \"FBD_NEW_FEEDER_CODE\" = '" + objReport.sNewFeederCode + "'";
                }
                if (!(objReport.sFeederBifurcationID == null || objReport.sFeederBifurcationID == ""))
                {
                    strQry += " AND \"FB_ID\" = " + objReport.sFeederBifurcationID + "";
                }
                //date selection
                if (objReport.sTodate == null && (objReport.sFromDate != null))
                {
                    strQry += " AND TO_CHAR(\"FB_OM_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(\"FB_OM_DATE\",'YYYY/MM/DD')<=TO_CHAR(NOW(),'YYYY/MM/DD')";

                }
                if (objReport.sFromDate == null && (objReport.sTodate != null))
                {
                    strQry += " AND TO_CHAR(\"FB_OM_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                }
                if (objReport.sFromDate == null && objReport.sTodate == null)
                {
                    strQry += " AND TO_CHAR(\"FB_OM_DATE\",'YYYY/MM/DD')<=TO_CHAR(NOW(),'YYYY/MM/DD') ";
                }
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " AND TO_CHAR(\"FB_OM_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"FB_OM_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                }
                strQry += " ORDER BY \"DT_OM_SLNO\",  \"FBD_OLD_DTC_CODE\", \"FBD_NEW_DTC_CODE\" ";
                dt = ObjCon.FetchDataTable(strQry);
                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "PrintFeederBifurcationReport");
                return dt;
            }
        }


        public DataTable GetEstimationAmountrepairer(string EstId)
        {
            string StrQry = string.Empty;
            DataTable dtest = new DataTable();
            try
            {
                StrQry = "SELECT \"RESTD_TC_CODE\" AS \"DTR_CODE\",SUM(CASE \"MRIM_ITEM_TYPE\" WHEN  '1'  THEN \"RESTM_ITEM_TOTAL\" - \"RESTM_ITEM_TAX\" ELSE 0 END + ";
                StrQry += " CASE \"MRIM_ITEM_TYPE\" WHEN '2' THEN \"RESTM_ITEM_TOTAL\" - \"RESTM_ITEM_TAX\" ELSE 0 END - ";
                StrQry += " CASE \"MRIM_ITEM_TYPE\" WHEN '3' THEN \"RESTM_ITEM_TOTAL\" - \"RESTM_ITEM_TAX\" ELSE 0 END) \"AMOUNT\",";
                StrQry += " SUM(  CASE \"MRIM_ITEM_TYPE\" WHEN  '1'  THEN  \"RESTM_ITEM_TAX\" ELSE 0 END + ";
                StrQry += " CASE \"MRIM_ITEM_TYPE\" WHEN '2' THEN  \"RESTM_ITEM_TAX\" ELSE 0 END - ";
                StrQry += " CASE \"MRIM_ITEM_TYPE\" WHEN '3' THEN  \"RESTM_ITEM_TAX\" ELSE 0 END)  \"TAX\", ";
                StrQry += " SUM( CASE \"MRIM_ITEM_TYPE\" WHEN  '1'  THEN  \"RESTM_ITEM_TOTAL\" ELSE 0 END + ";
                StrQry += " CASE \"MRIM_ITEM_TYPE\" WHEN '2' THEN  \"RESTM_ITEM_TOTAL\" ELSE 0 END - ";
                StrQry += " CASE \"MRIM_ITEM_TYPE\" WHEN '3' THEN  \"RESTM_ITEM_TOTAL\" ELSE 0 END)  \"TOTALAMOUNT\" ";
                StrQry += " FROM (SELECT DISTINCT \"RESTD_ID\", \"RESTD_NO\", \"TM_NAME\", \"OM_NAME\", \"RESTD_CRON\",  \"RESTD_CAPACITY\", ";
                StrQry += " \"TR_NAME\", \"MRIM_ITEM_TYPE\", \"MRIM_REMARKS\", \"MRIM_ITEM_NAME\",\"MRIM_ITEM_ID\",\"MD_NAME\", \"RESTM_ITEM_QNTY\", \"RESTM_ITEM_RATE\",  ";
                StrQry += " \"RESTM_ITEM_TOTAL\" - \"RESTM_ITEM_TAX\" \"AMOUNT\", \"RESTM_ITEM_TAX\" , \"RESTM_ITEM_TOTAL\",\"RESTD_TC_CODE\"  FROM \"TBLREPAIRERESTIMATIONDETAILS\"  ";
                StrQry += " JOIN \"TBLREPAIRERESTIMATIONMATERIAL\" ON \"RESTD_ID\" = \"RESTM_EST_ID\" JOIN \"TBLTRANSREPAIRER\" ON\"RESTD_REPAIRER\" = \"TR_ID\"  ";
                StrQry += "  JOIN \"TBLTCMASTER\" ON \"RESTD_TC_CODE\" = \"TC_CODE\" JOIN ";
                StrQry += " \"TBLTRANSMAKES\" ON \"TC_MAKE_ID\" =  \"TM_ID\" JOIN \"TBLOMSECMAST\" ON \"RESTD_OFF_CODE\" =cast(\"OM_CODE\" as text)JOIN ";
                StrQry += " (SELECT DISTINCT \"MRIM_ID\", \"MRIM_ITEM_TYPE\",\"MRIM_ITEM_NAME\",\"MRIM_ITEM_ID\", \"MD_NAME\",  \"MRI_TR_ID\", \"MRIM_REMARKS\",\"MRI_CAPACITY\" FROM  ";
                StrQry += " \"TBLMINORREPAIRERITEMMASTER\",  \"TBLMINORREPAIRITEMRATEMASTER\", \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='MSR' AND ";
                StrQry += " \"MRIM_ID\" = \"MRI_MRIM_ID\" AND  CAST(\"MRI_MEASUREMENT\" AS TEXT) = CAST(\"MD_ID\" AS TEXT))A ON \"RESTM_ITEM_ID\" = ";
                StrQry += " \"MRIM_ID\"    WHERE \"RESTD_ID\" = '" + EstId + "' AND CAST(\"RESTD_CAPACITY\" AS TEXT) = (SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" ";
                StrQry += " WHERE \"MD_TYPE\" = 'C' AND \"MD_ID\" = A.\"MRI_CAPACITY\")AND \"RESTD_REPAIRER\" = \"MRI_TR_ID\")Z  GROUP BY \"RESTD_TC_CODE\"";
                dtest = ObjCon.FetchDataTable(StrQry);
                return dtest;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtest;
            }
        }
        public DataTable PrintEstimatedReport(string sFailureId, string sStatus, string sFailType, string sInsulationType, string starrating, string StatusFalg)
        {
            // sFailType 1-> single coil, 2-> multi coil
            DataTable dtDetailedReport = new DataTable();
            string strQry = string.Empty;
            sEmployeeCost = ConfigurationSettings.AppSettings["EmployeeCost"];
            sESI = ConfigurationSettings.AppSettings["ESI"];
            ServiceTax = ConfigurationSettings.AppSettings["ServiceTax"];
            DecomLabourCost = ConfigurationSettings.AppSettings["DecomLabourCost"];
            ObjCon.BeginTransaction();
            try
            {

                int tcrating;
                if (starrating != "0")
                {
                    tcrating = Convert.ToInt32(starrating);
                }
                else
                {
                    tcrating = Convert.ToInt32(ObjCon.get_value("SELECT \"TC_RATING\" from \"TBLTCMASTER\",\"TBLDTCFAILURE\"  WHERE  cast(\"DF_EQUIPMENT_ID\" as varchar)=\"TC_CODE\" AND CAST(\"DF_ID\" AS TEXT) ='" + sFailureId + "'"));

                }


                strQry = "SELECT \"DF_ENHANCE_CAPACITY\" FROM \"TBLDTCFAILURE\" WHERE CAST(\"DF_ID\" AS TEXT) ='" + sFailureId + "'";
                string sEnhanceCapacity = ObjCon.get_value(strQry);

                if ((sEnhanceCapacity == "" || sEnhanceCapacity == null) && sFailType != "2")
                {
                    // changed query for calculate amount
                    //strQry = "select \"DF_DTC_CODE\",CAST(\"DF_EQUIPMENT_ID\" AS TEXT) \"DF_EQUIPMENT_ID\",TO_CHAR(\"DF_DATE\",'dd/MM/yyyy') AS \"DF_DATE\",\"DF_LOC_CODE\",CAST(\"TC_CODE\" AS TEXT)\"DTR_CODE\",";
                    //strQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\"AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\"AS TEXT),1,'" + Constants.SubDivision + "')) as \"SUBDIVISION\",";
                    //strQry += " (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where CAST(\"OM_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Section + ")) as \"LOCATION\",";
                    //strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Division + ")) as \"DIVISION\",";
                    //strQry += " (SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=\"DF_DTC_CODE\") AS \"DT_NAME\",";
                    //strQry += " 'No' AS \"UNIT\",'1' as \"QUANTITY\",(select CAST(\"TC_CAPACITY\" AS TEXT) from \"TBLTCMASTER\" where \"TC_CODE\"=\"DF_EQUIPMENT_ID\") AS \"CAPACITY\",";
                    //strQry += " \"TE_RATE\" AS \"PRICE\",1  \"TE_RATE\" AS \"TOTALAMOUNT\",\"TE_COMMLABOUR\" as \"LABOURCHARGE\",(((\"TE_COMMLABOUR\"  '" + ServiceTax + "') / 100 + \"TE_COMMLABOUR\") *  '" + sEmployeeCost + "')/100 AS \"EMPLOYEECOST\",(((\"TE_COMMLABOUR\"  '" + ServiceTax + "') / 100 + \"TE_COMMLABOUR\") * '" + sESI + "')/100 as ESI,(\"TE_COMMLABOUR\" * '" + ServiceTax + "') / 100 + \"TE_COMMLABOUR\" as \"SERVICETAX\",";
                    //strQry += " ((\"TE_RATE\"+((\"TE_COMMLABOUR\"  '" + sEmployeeCost + "')/100)+ \"TE_COMMLABOUR\" )/100)*2 AS \"CONTINGENCYCOST\",((((\"TE_COMMLABOUR\"  '" + ServiceTax + "') / 100 + \"TE_COMMLABOUR\") * '" + sESI + "')/100) +\"TE_RATE\" +  ((\"TE_COMMLABOUR\"  '" + ServiceTax + "') / 100 + \"TE_COMMLABOUR\")  + ((((\"TE_COMMLABOUR\"  '" + ServiceTax + "') / 100 + \"TE_COMMLABOUR\") *  '" + sEmployeeCost + "')/100)) AS \"FINALTOTAL\",\"EST_NO\",CAST(\"DF_ENHANCE_CAPACITY\" AS TEXT) \"DF_ENHANCE_CAPACITY\", ";
                    //strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\"AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\"AS TEXT),1," + Constants.SubDivision + ") AND \"US_ROLE_ID\"='1' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\" ='A' LIMIT 1) \"SDO_USERNAME\",";
                    //strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=CAST(\"DF_LOC_CODE\" AS TEXT) AND \"US_ROLE_ID\"='4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A'  AND \"US_ID\" =(SELECT \"DF_CRBY\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\"='" + sFailureId + "') LIMIT 1) \"SO_USERNAME\" ";
                    //strQry += " FROM \"TBLDTCFAILURE\",\"TBLITEMMASTER\",\"TBLESTIMATION\",\"TBLTCMASTER\" WHERE \"DF_ID\"='" + sFailureId + "' AND \"EST_DF_ID\"=\"DF_ID\" ";
                    //strQry += " AND \"TC_CODE\"=\"DF_EQUIPMENT_ID\" AND \"TC_CAPACITY\"=\"TE_CAPACITY\" ";

                    strQry = "select \"DF_DTC_CODE\",CAST(\"DF_EQUIPMENT_ID\" AS TEXT) \"DF_EQUIPMENT_ID\",TO_CHAR(\"DF_DATE\",'dd/MM/yyyy') AS \"DF_DATE\",\"DF_LOC_CODE\",CAST(\"TC_CODE\" AS TEXT)\"DTR_CODE\",";
                    strQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\"AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\"AS TEXT),1,'" + Constants.SubDivision + "')) as \"SUBDIVISION\",";
                    strQry += " (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where CAST(\"OM_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Section + ")) as \"LOCATION\",";
                    strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Division + ")) as \"DIVISION\",";
                    strQry += " (SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=\"DF_DTC_CODE\") AS \"DT_NAME\",";
                    strQry += " 'No' AS \"UNIT\",'1' as \"QUANTITY\",(select CAST(\"TC_CAPACITY\" AS TEXT) from \"TBLTCMASTER\" where \"TC_CODE\"=\"DF_EQUIPMENT_ID\") AS \"CAPACITY\",";
                    strQry += " \"TE_RATE\" AS \"PRICE\",1  \"TE_RATE\" AS \"TOTALAMOUNT\",\"TE_COMMLABOUR\" as \"LABOURCHARGE\",(((\"TE_COMMLABOUR\"  '" + sEmployeeCost + "') / 100 + \"TE_COMMLABOUR\") *  '" + sEmployeeCost + "')/100 AS \"EMPLOYEECOST\",(((\"TE_COMMLABOUR\"  '" + ServiceTax + "') / 100 + \"TE_COMMLABOUR\") * '" + sESI + "')/100 as ESI,(\"TE_COMMLABOUR\" * '" + ServiceTax + "') / 100 + \"TE_COMMLABOUR\" as \"SERVICETAX\",";
                    strQry += " ((\"TE_RATE\"+((\"TE_COMMLABOUR\"  '" + sEmployeeCost + "')/100)+ \"TE_COMMLABOUR\" )/100)*2 AS \"CONTINGENCYCOST\",((((\"TE_COMMLABOUR\"  '" + sEmployeeCost + "') / 100 + \"TE_COMMLABOUR\") * '" + sESI + "')/100) +\"TE_RATE\" +  ((\"TE_COMMLABOUR\"  '" + ServiceTax + "') / 100 + \"TE_COMMLABOUR\")  + ((((\"TE_COMMLABOUR\"  '" + ServiceTax + "') / 100 + \"TE_COMMLABOUR\") *  '" + sEmployeeCost + "')/100)) AS \"FINALTOTAL\",\"EST_NO\",CAST(\"DF_ENHANCE_CAPACITY\" AS TEXT) \"DF_ENHANCE_CAPACITY\", ";
                    strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\"AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\"AS TEXT),1," + Constants.SubDivision + ") AND \"US_ROLE_ID\"='1' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\" ='A' LIMIT 1) \"SDO_USERNAME\",";
                    strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=CAST(\"DF_LOC_CODE\" AS TEXT) AND \"US_ROLE_ID\"='4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A'  AND \"US_ID\" =(SELECT \"DF_CRBY\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\"='" + sFailureId + "') LIMIT 1) \"SO_USERNAME\" ";
                    strQry += " FROM \"TBLDTCFAILURE\",\"TBLITEMMASTER\",\"TBLESTIMATION\",\"TBLTCMASTER\" WHERE \"DF_ID\"='" + sFailureId + "' AND \"EST_DF_ID\"=\"DF_ID\" ";
                    strQry += " AND \"TC_CODE\"=\"DF_EQUIPMENT_ID\" AND \"TC_CAPACITY\"=\"TE_CAPACITY\" ";
                    if (tcrating > 3)
                    {

                        strQry += " AND '0'=COALESCE(\"TE_STAR_RATE\",0)";
                    }
                    else
                    {
                        if (starrating != "0")
                        {
                            strQry += " AND '" + starrating + "'=COALESCE(\"TE_STAR_RATE\",0) ";
                        }
                        else
                        {
                            strQry += " AND COALESCE(\"TC_RATING\",0)=COALESCE(\"TE_STAR_RATE\",0)";
                        }
                    }
                }
                else
                {
                    if (sStatus != "")
                    {
                        if (sStatus.Contains("-"))
                        {
                            if (sFailType == "2")
                            {
                                strQry = "SELECT * FROM (SELECT \"DF_DTC_CODE\",CAST(\"DF_EQUIPMENT_ID\" AS TEXT) AS \"DF_EQUIPMENT_ID\",TO_CHAR(\"DF_DATE\",'dd/MM/yyyy') AS \"DF_DATE\",\"DF_LOC_CODE\",";
                                strQry += "(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\"AS TEXT) = SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "')) AS \"SUBDIVISION\", ";
                                strQry += "(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where CAST(\"OM_CODE\" AS TEXT) = SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Section + "')) as \"LOCATION\",(select CAST(\"TC_CAPACITY\" AS TEXT) from \"TBLTCMASTER\" where \"TC_CODE\"=\"DF_EQUIPMENT_ID\")\"Capacity\", ";
                                strQry += "(SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "')) as \"Division\", ";
                                strQry += "(SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=\"DF_DTC_CODE\") \"DT_NAME\", 'No' AS \"UNIT\",'1' as \"QUANTITY\",";
                                strQry += "(SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "') AND \"US_ROLE_ID\"='1' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1)";
                                strQry += "\"SDO_USERNAME\", (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=CAST(\"DF_LOC_CODE\" AS TEXT) AND \"US_ROLE_ID\"='4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' ";
                                strQry += "AND \"US_ID\"=(SELECT \"DF_CRBY\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\"='" + sFailureId + "')) \"SO_USERNAME\"  FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\"='" + sFailureId + "')A ";

                                strQry += "RIGHT JOIN (SELECT \"TE_RATE\" as \"Price\",1*\"TE_RATE\" AS \"TotalAmount\", \"TE_COMMLABOUR\" as \"labourcharge\",(((\"TE_COMMLABOUR\"*'" + ServiceTax + "')/100+\"TE_COMMLABOUR\")*'" + sEmployeeCost + "')/100 as ";
                                strQry += "\"EmployeeCost\",(((\"TE_COMMLABOUR\"*'" + ServiceTax + "')/100+\"TE_COMMLABOUR\")*'" + sESI + "')/100 as \"ESI\",(\"TE_COMMLABOUR\"*'" + ServiceTax + "')/100+\"TE_COMMLABOUR\" as \"ServiceTax\", (\"TE_RATE\"*2)/100 as \"ContingencyCost\", ";
                                strQry += "  \"TE_RATE\" + ((((\"TE_COMMLABOUR\"*'" + ServiceTax + "')/100+\"TE_COMMLABOUR\")*'" + sESI + "')/100 )+ ((((\"TE_COMMLABOUR\"*'" + ServiceTax + "')/100+\"TE_COMMLABOUR\")*'" + sEmployeeCost + "')/100 )+ ((\"TE_COMMLABOUR\"*'" + ServiceTax + "')/100+\"TE_COMMLABOUR\")    as \"FinalTotal\",NULL AS \"EST_NO\", \"TC_CAPACITY\" , \"DF_ENHANCE_CAPACITY\" ,\"TIT_INSULATION_NAME\", \"TT_NAME\"";


                                strQry += "   FROM \"TBLITEMMASTER\",\"TBLTRANSINSULATIONTYPE\",\"TBLTRANSCORETYPE\"";
                                strQry += ",\"TBLDTCFAILURE\",\"TBLTCMASTER\" WHERE \"DF_EQUIPMENT_ID\"=\"TC_CODE\" ";

                                if (StatusFalg == "2" || StatusFalg == "4")
                                {
                                    strQry += "AND \"TE_CAPACITY\"=" + sEnhanceCapacity + " AND \"TE_TIT_ID\"=\"TIT_ID\" AND \"TT_ID\"= \"TIT_TT_ID\"   AND CAST(\"DF_ID\" AS TEXT)='" + sFailureId + "' ";
                                }
                                else
                                {
                                    strQry += "AND \"TE_CAPACITY\"=\"TC_CAPACITY\" AND \"TE_TIT_ID\"=\"TIT_ID\" AND \"TT_ID\"= \"TIT_TT_ID\"   AND CAST(\"DF_ID\" AS TEXT)='" + sFailureId + "' ";
                                }

                                if (sInsulationType == null || sInsulationType == "")
                                {
                                    // strQry += " AND CAST( \"TE_TIT_ID\" AS TEXT)='" + sInsulationType + "'";
                                }
                                else
                                {
                                    strQry += " AND CAST( \"TE_TIT_ID\" AS TEXT)='" + sInsulationType + "'";
                                }
                                //if (tcrating > 3)
                                //{

                                //    strQry += " AND '0'=COALESCE(\"TE_STAR_RATE\",0))B ON ";
                                //}
                                //else
                                //{
                                if (starrating != "0")
                                {
                                    strQry += " AND '" + starrating + "'=COALESCE(\"TE_STAR_RATE\",0))B ON ";
                                }
                                else
                                {
                                    strQry += " AND COALESCE(\"TC_RATING\",0)=COALESCE(\"TE_STAR_RATE\",0))B ON ";
                                }

                                // }
                                // AND COALESCE(\"TC_RATING\",0)=COALESCE(\"TE_STAR_RATE\",0))B ON ";
                                strQry += " A.\"Capacity\"=CAST(B.\"TC_CAPACITY\" AS TEXT)";
                            }
                            else
                            {
                                strQry = "SELECT * FROM (SELECT \"DF_DTC_CODE\",CAST(\"DF_EQUIPMENT_ID\" AS TEXT) AS \"DF_EQUIPMENT_ID\",TO_CHAR(\"DF_DATE\",'dd/MM/yyyy') AS \"DF_DATE\",\"DF_LOC_CODE\",";
                                strQry += "(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\"AS TEXT) = SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "')) AS \"SUBDIVISION\", ";
                                strQry += "(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where CAST(\"OM_CODE\" AS TEXT) = SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Section + "')) as \"LOCATION\",(select CAST(\"TC_CAPACITY\" AS TEXT) from \"TBLTCMASTER\" where \"TC_CODE\"=\"DF_EQUIPMENT_ID\")\"Capacity\", ";
                                strQry += "(SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "')) as \"Division\", ";
                                strQry += "(SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=\"DF_DTC_CODE\") \"DT_NAME\", 'No' AS \"UNIT\",'1' as \"QUANTITY\",CAST(\"DF_ENHANCE_CAPACITY\" AS TEXT)\"DF_ENHANCE_CAPACITY\",";
                                strQry += "(SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "') AND \"US_ROLE_ID\"='1' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1)";
                                strQry += "\"SDO_USERNAME\", (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=CAST(\"DF_LOC_CODE\" AS TEXT) AND \"US_ROLE_ID\"='4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' ";
                                strQry += "AND \"US_ID\"=(SELECT \"DF_CRBY\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\"='" + sFailureId + "')) \"SO_USERNAME\"  FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\"='" + sFailureId + "')A ";

                                strQry += "RIGHT JOIN (SELECT \"TE_RATE\" as \"Price\",1*\"TE_RATE\" AS \"TotalAmount\", \"TE_COMMLABOUR\" as \"labourcharge\",(((\"TE_COMMLABOUR\"*'" + ServiceTax + "')/100+\"TE_COMMLABOUR\")*'" + sEmployeeCost + "')/100 as ";
                                strQry += "\"EmployeeCost\",(((\"TE_COMMLABOUR\"*'" + ServiceTax + "')/100+\"TE_COMMLABOUR\")*'" + sESI + "')/100 as \"ESI\",(\"TE_COMMLABOUR\"*'" + ServiceTax + "')/100+\"TE_COMMLABOUR\" as \"ServiceTax\", (\"TE_RATE\"*2)/100 as \"ContingencyCost\", ";
                                strQry += "  \"TE_RATE\" + ((((\"TE_COMMLABOUR\"*'" + ServiceTax + "')/100+\"TE_COMMLABOUR\")*'" + sESI + "')/100 )+ ((((\"TE_COMMLABOUR\"*'" + ServiceTax + "')/100+\"TE_COMMLABOUR\")*'" + sEmployeeCost + "')/100 )+ ((\"TE_COMMLABOUR\"*'" + ServiceTax + "')/100+\"TE_COMMLABOUR\")    as \"FinalTotal\",NULL AS  \"EST_NO\", \"TC_CAPACITY\" , \"DF_ENHANCE_CAPACITY\" ,\"TIT_INSULATION_NAME\", \"TT_NAME\"";

                                strQry += "   FROM \"TBLITEMMASTER\",\"TBLTRANSINSULATIONTYPE\",\"TBLTRANSCORETYPE\"";
                                strQry += ",\"TBLDTCFAILURE\" WHERE  \"TE_CAPACITY\"=\"DF_ENHANCE_CAPACITY\" AND \"TE_TIT_ID\"=\"TIT_ID\" AND \"TT_ID\"= \"TIT_TT_ID\"  AND CAST(\"DF_ID\" AS TEXT)='" + sFailureId + "' ";

                                if (sInsulationType == null || sInsulationType == "")
                                {
                                    // strQry += " AND CAST( \"TE_TIT_ID\" AS TEXT)='" + sInsulationType + "'";


                                }
                                else
                                {
                                    strQry += " AND CAST( \"TE_TIT_ID\" AS TEXT)='" + sInsulationType + "'";
                                }

                                //if (tcrating > 3)
                                //{

                                //    strQry += " AND '0'=COALESCE(\"TE_STAR_RATE\",0))B ON ";
                                //}
                                //else
                                //{

                                if (starrating != "0")
                                {
                                    strQry += " AND '" + starrating + "'=COALESCE(\"TE_STAR_RATE\",0))B ON  ";
                                }
                                else
                                {
                                    strQry += " AND COALESCE(\"TC_RATING\",0)=COALESCE(\"TE_STAR_RATE\",0))B ON ";
                                }
                                //}
                                //AND COALESCE(\"TC_RATING\",0)=COALESCE(\"TE_STAR_RATE\",0))B ON ";
                                strQry += " A.\"DF_ENHANCE_CAPACITY\"=CAST(B.\"DF_ENHANCE_CAPACITY\" AS TEXT)";
                            }
                        }
                        else
                        {
                            if (sFailType == "2")
                            {
                                strQry = "SELECT * FROM (SELECT \"DF_DTC_CODE\",CAST(\"DF_EQUIPMENT_ID\" AS TEXT) AS \"DF_EQUIPMENT_ID\",TO_CHAR(\"DF_DATE\",'dd/MM/yyyy') AS \"DF_DATE\",\"DF_LOC_CODE\",";
                                strQry += "(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\"AS TEXT) = SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "')) AS \"SUBDIVISION\", ";
                                strQry += "(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where CAST(\"OM_CODE\" AS TEXT) = SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Section + "')) as \"LOCATION\",(select CAST(\"TC_CAPACITY\" AS TEXT) from \"TBLTCMASTER\" where \"TC_CODE\"=\"DF_EQUIPMENT_ID\")\"Capacity\", ";
                                strQry += "(SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "')) as \"Division\", ";
                                strQry += "(SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=\"DF_DTC_CODE\") \"DT_NAME\", 'No' AS \"UNIT\",'1' as \"QUANTITY\",";
                                strQry += "(SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "') AND \"US_ROLE_ID\"='1' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1)";
                                strQry += "\"SDO_USERNAME\", (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=CAST(\"DF_LOC_CODE\" AS TEXT) AND \"US_ROLE_ID\"='4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' ";
                                strQry += "AND \"US_ID\"=(SELECT \"DF_CRBY\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\"='" + sFailureId + "')) \"SO_USERNAME\"  FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\"='" + sFailureId + "')A ";
                                strQry += "RIGHT JOIN (SELECT \"TE_RATE\" as \"Price\",1*\"TE_RATE\" AS \"TotalAmount\", \"TE_COMMLABOUR\" as \"labourcharge\",(((\"TE_COMMLABOUR\"*'" + ServiceTax + "')/100+\"TE_COMMLABOUR\")*'" + sEmployeeCost + "')/100 as ";
                                strQry += "\"EmployeeCost\",(((\"TE_COMMLABOUR\"*'" + ServiceTax + "')/100+\"TE_COMMLABOUR\")*'" + sESI + "')/100 as \"ESI\",(\"TE_COMMLABOUR\"*'" + ServiceTax + "')/100+\"TE_COMMLABOUR\" as \"ServiceTax\", (\"TE_RATE\"*2)/100 as \"ContingencyCost\", ";
                                strQry += "  \"TE_RATE\" + ((((\"TE_COMMLABOUR\"*'" + ServiceTax + "')/100+\"TE_COMMLABOUR\")*'" + sESI + "')/100 )+ ((((\"TE_COMMLABOUR\"*'" + ServiceTax + "')/100+\"TE_COMMLABOUR\")*'" + sEmployeeCost + "')/100 )+ ((\"TE_COMMLABOUR\"*'" + ServiceTax + "')/100+\"TE_COMMLABOUR\")    as \"FinalTotal\", NULL AS \"EST_NO\", \"TC_CAPACITY\" , \"DF_ENHANCE_CAPACITY\" ,\"TIT_INSULATION_NAME\", \"TT_NAME\"";
                                strQry += "   FROM \"TBLITEMMASTER\",\"TBLTRANSINSULATIONTYPE\",\"TBLTRANSCORETYPE\"";
                                strQry += ",\"TBLDTCFAILURE\",\"TBLTCMASTER\",\"TBLESTIMATION\" WHERE \"DF_EQUIPMENT_ID\"=\"TC_CODE\"   AND \"EST_DF_ID\"=\"DF_ID\" ";

                                if (StatusFalg == "2" || StatusFalg == "4")
                                {
                                    strQry += "AND \"TE_CAPACITY\"=" + sEnhanceCapacity + " AND \"TE_TIT_ID\"=\"TIT_ID\" AND \"TT_ID\"= \"TIT_TT_ID\"   AND CAST(\"DF_ID\" AS TEXT)='" + sFailureId + "' ";
                                }
                                else
                                {
                                    strQry += "AND \"TE_CAPACITY\"=\"TC_CAPACITY\" AND \"TE_TIT_ID\"=\"TIT_ID\" AND \"TT_ID\"= \"TIT_TT_ID\"   AND CAST(\"DF_ID\" AS TEXT)='" + sFailureId + "' ";
                                }
                                if (sInsulationType == null || sInsulationType == "" || sInsulationType == "0")
                                {
                                    // strQry += " AND CAST( \"TE_TIT_ID\" AS TEXT)='" + sInsulationType + "'";


                                }
                                else
                                {
                                    strQry += " AND CAST( \"TE_TIT_ID\" AS TEXT)='" + sInsulationType + "'";
                                }


                                //if (tcrating > 3)
                                //{

                                //    strQry += " AND '0'=COALESCE(\"TE_STAR_RATE\",0))B ON ";
                                //}
                                //else
                                //{
                                if (starrating != "0")
                                {
                                    strQry += " AND '" + starrating + "'=COALESCE(\"TE_STAR_RATE\",0))B ON  ";
                                }
                                else
                                {
                                    strQry += " AND COALESCE(\"TC_RATING\",0)=COALESCE(\"TE_STAR_RATE\",0))B ON ";
                                }
                                // }
                                //AND COALESCE(\"TC_RATING\",0)=COALESCE(\"TE_STAR_RATE\",0))B ON ";
                                strQry += " A.\"Capacity\"=CAST(B.\"TC_CAPACITY\" AS TEXT)";
                            }
                            else
                            {
                                strQry = "SELECT * FROM (SELECT \"DF_DTC_CODE\",CAST(\"DF_EQUIPMENT_ID\" AS TEXT) AS \"DF_EQUIPMENT_ID\",TO_CHAR(\"DF_DATE\",'dd/MM/yyyy') AS \"DF_DATE\",\"DF_LOC_CODE\",";
                                strQry += "(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\"AS TEXT) = SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "')) AS \"SUBDIVISION\", ";
                                strQry += "(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where CAST(\"OM_CODE\" AS TEXT) = SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Section + "')) as \"LOCATION\",(select CAST(\"TC_CAPACITY\" AS TEXT) from \"TBLTCMASTER\" where \"TC_CODE\"=\"DF_EQUIPMENT_ID\")\"Capacity\", ";
                                strQry += "(SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "')) as \"Division\", ";
                                strQry += "(SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=\"DF_DTC_CODE\") \"DT_NAME\", 'No' AS \"UNIT\",'1' as \"QUANTITY\",CAST(\"DF_ENHANCE_CAPACITY\" AS TEXT)\"DF_ENHANCE_CAPACITY\",";
                                strQry += "(SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "') AND \"US_ROLE_ID\"='1' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1)";
                                strQry += "\"SDO_USERNAME\", (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=CAST(\"DF_LOC_CODE\" AS TEXT) AND \"US_ROLE_ID\"='4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' ";
                                strQry += "AND \"US_ID\"=(SELECT \"DF_CRBY\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\"='" + sFailureId + "')) \"SO_USERNAME\"  FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\"='" + sFailureId + "')A ";
                                strQry += "RIGHT JOIN (SELECT \"TE_RATE\" as \"Price\",1*\"TE_RATE\" AS \"TotalAmount\", \"TE_COMMLABOUR\" as \"labourcharge\",(\"TE_COMMLABOUR\"*'" + sEmployeeCost + "')/100 as ";
                                strQry += "\"EmployeeCost\",(\"TE_COMMLABOUR\"*'" + sESI + "')/100 as \"ESI\",(\"TE_COMMLABOUR\"*'" + ServiceTax + "')/100+\"TE_COMMLABOUR\" as \"ServiceTax\", (\"TE_RATE\"*2)/100 as \"ContingencyCost\", ";
                                strQry += "(\"TE_RATE\"+\"TE_COMMLABOUR\"+(\"TE_RATE\"*2)/100+(\"TE_COMMLABOUR\"*10)/100) as \"FinalTotal\",\"EST_NO\", \"DF_ENHANCE_CAPACITY\",\"TIT_INSULATION_NAME\", \"TT_NAME\" FROM \"TBLITEMMASTER\",\"TBLTRANSINSULATIONTYPE\",\"TBLTRANSCORETYPE\",";
                                strQry += "\"TBLDTCFAILURE\",\"TBLESTIMATION\" WHERE  \"TE_CAPACITY\"=\"DF_ENHANCE_CAPACITY\" AND \"TE_TIT_ID\"=\"TIT_ID\" AND \"TT_ID\"= \"TIT_TT_ID\"  AND CAST(\"DF_ID\" AS TEXT)='" + sFailureId + "' AND \"DF_ID\"=\"EST_DF_ID\" ";



                                if (sInsulationType == null || sInsulationType == "")
                                {
                                    // strQry += " AND CAST( \"TE_TIT_ID\" AS TEXT)='" + sInsulationType + "'";


                                }
                                else
                                {
                                    strQry += " AND CAST( \"TE_TIT_ID\" AS TEXT)='" + sInsulationType + "'";
                                }


                                //if (tcrating > 3)
                                //{

                                //    strQry += " AND '0'=COALESCE(\"TE_STAR_RATE\",0))B ON ";
                                //}
                                //else
                                //{

                                if (starrating != "0")
                                {
                                    strQry += " AND '" + starrating + "'=COALESCE(\"TE_STAR_RATE\",0))B ON ";
                                }
                                else
                                {
                                    strQry += " AND COALESCE(\"TC_RATING\",0)=COALESCE(\"TE_STAR_RATE\",0))B ON ";
                                }

                                //  }
                                //AND COALESCE(\"TC_RATING\",0)=COALESCE(\"TE_STAR_RATE\",0))B ON ";
                                strQry += " A.\"DF_ENHANCE_CAPACITY\"=CAST(B.\"DF_ENHANCE_CAPACITY\" AS TEXT)";
                            }

                        }
                    }
                }

                dtDetailedReport = ObjCon.FetchDataTable(strQry);
                ObjCon.CommitTransaction();
                return dtDetailedReport;
            }
            catch (Exception ex)
            {
                ObjCon.RollBackTrans();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtDetailedReport;

            }
        }

        public DataTable FailureAbstract(clsReports objReport)
        {
            string strQry = string.Empty;
            DataTable dtFailureAbstract = new DataTable();
            try
            {
                //strQry = "SELECT to_char(SYSDATE,'MONTH YYYY')as CURRENT_MONTH, to_char(to_date('" + objReport.sMonth + "','YYYY-MM'),'Month yyyy') ";
                //strQry += "as SELECTED_MONTH , to_number(Invoice_Pending+Decommission_Pending)Invoice_Pending, to_number(nvl(WO_PENDING,0)+";
                //strQry += "nvl(INDENT_PENDING,0)+nvl(EST_PEND,0)+nvl(INVOICE_PENDING,0)+nvl(Decommission_Pending,0)) as  PENDING_FOR_REPLACEMENT, DIV_NAME,";
                //strQry += "to_number(nvl(Total_Fail_Pending,0))Total_Fail_Pending,to_number(nvl(est_pend,0))est_pend,to_number(WO_Pending)WO_Pending, ";
                //strQry += "to_number(Indent_Pending)Indent_Pending,to_number(Decommission_Pending)Decommission_Pending,to_number(CR_RI_Pending)";
                //strQry += "CR_RI_Pending,wo_office_code,to_number(TOTAL_DTC)TOTAL_DTC FROM (SELECT WO_Pending,Indent_Pending,Invoice_Pending,";
                //strQry += "Decommission_Pending, CR_RI_Pending,est_pend,wo_office_code,Total_Fail_Pending from (SELECT WO_Pending,Indent_Pending,";
                //strQry += "est_pend,Invoice_Pending,Decommission_Pending, CR_RI_Pending,a.OFFCODE as wo_office_code from (SELECT substr(OFFCODE,0,2)";
                //strQry += "OFFCODE, sum(CASE WHEN WORKORDER IS NULL AND FAILURE IS NOT NULL  THEN 1 ELSE 0 END)WO_Pending, sum(CASE WHEN INDENT IS NULL ";
                //strQry += "AND WORKORDER IS NOT NULL THEN 1 ELSE 0 END)Indent_Pending, sum(CASE WHEN INVOICE IS NULL AND INDENT IS NOT NULL THEN 1 ELSE ";
                //strQry += "0 END)Invoice_Pending,sum(CASE WHEN DECOMMISION IS NULL AND INVOICE IS NOT NULL THEN 1 ELSE 0 END)Decommission_Pending, ";
                //strQry += "sum(CASE WHEN CRREPORT IS NULL AND DECOMMISION IS NOT NULL THEN 1 ELSE 0 END)CR_RI_Pending FROM WORKFLOWSTATUS WHERE ";
                //strQry += "OFFCODE like '%' GROUP BY substr(OFFCODE, 0, 2) ORDER BY substr(OFFCODE, 0, 2))a LEFT JOIN (SELECT substr";
                //strQry += "(wo_office_code, 0, 2) wo_office_code, count(*) est_pend FROM TBLWORKFLOWOBJECTS WHERE WO_REF_OFFCODE LIKE '%'  AND ";
                //strQry += "WO_BO_ID = '9'  AND WO_APPROVE_STATUS = '0' GROUP BY substr(wo_office_code, 0, 2) ORDER BY substr(wo_office_code, 0, 2))b  ";
                //strQry += "ON a.OFFCODE = b.wo_office_code)a LEFT JOIN (SELECT sum(CASE WHEN  DF_STATUS_FLAG IN (1,4) THEN 1 ELSE 0 END)Total_Fail_Pending, ";
                //strQry += "substr(DF_LOC_CODE, 0, 2)OFFCODE FROM TBLDTCFAILURE WHERE  DF_STATUS_FLAG IN (1,4) AND TO_CHAR(DF_DATE, 'YYYY-MM')= '" + objReport.sMonth + "'";
                //strQry += "GROUP BY substr(DF_LOC_CODE, 0, 2) ORDER BY  substr(DF_LOC_CODE, 0, 2))b on a.wo_office_code = b.OFFCODE)A RIGHT JOIN ";
                //strQry += "(SELECT to_number(TOTAL_DTC)TOTAL_DTC,DT_DIV FROM(SELECT COUNT(DT_CODE)TOTAL_DTC,SUBSTR(DT_OM_SLNO,0,2)DT_DIV FROM ";
                //strQry += "TBLDTCMAST GROUP BY SUBSTR(DT_OM_SLNO,0,2) ORDER BY  SUBSTR(DT_OM_SLNO,0,2)))B ON B.DT_DIV=WO_OFFICE_CODE RIGHT JOIN  ";
                //strQry += "TBLDIVISION ON wo_office_code = DIV_CODE";

                #region second query
                //strQry = "SELECT to_char(SYSDATE,'MONTH YYYY')as CURRENT_MONTH, to_char(to_date('" + objReport.sMonth + "','YYYY-MM'),'Month yyyy') ";
                //strQry += "as SELECTED_MONTH , to_number(Invoice_Pending+Decommission_Pending)Invoice_Pending, to_number(nvl(WO_PENDING,0)+nvl";
                //strQry += "(INDENT_PENDING,0)+nvl(EST_PEND,0)+nvl(INVOICE_PENDING,0)+nvl(Decommission_Pending,0)) as  PENDING_FOR_REPLACEMENT, ";
                //strQry += "DIV_NAME,to_number(nvl(Total_Fail_Pending,0))Total_Fail_Pending,to_number(nvl(est_pend,0))est_pend,";
                //strQry += "to_number(WO_Pending)WO_Pending, to_number(Indent_Pending)Indent_Pending,to_number(Decommission_Pending)";
                //strQry += "Decommission_Pending,to_number(CR_RI_Pending)CR_RI_Pending,wo_office_code,to_number(TOTAL_DTC)TOTAL_DTC FROM ";
                //strQry += "(SELECT WO_Pending,Indent_Pending,Invoice_Pending,Decommission_Pending, CR_RI_Pending,est_pend,wo_office_code,";
                //strQry += "Total_Fail_Pending from (SELECT WO_Pending,Indent_Pending,est_pend,Invoice_Pending,Decommission_Pending, ";
                //strQry += "CR_RI_Pending,a.OFFCODE as wo_office_code from (SELECT substr(OFFCODE,0,2)OFFCODE, sum(CASE WHEN WORKORDER IS NULL ";
                //strQry += "AND FAILURE IS NOT NULL AND to_char(TO_DATE(to_char(DF_DATE),'DD-MM-YYYY'),'YYYY-MM')='" + objReport.sMonth + "'  ";
                //strQry += "THEN 1 ELSE 0 END)WO_Pending, sum(CASE WHEN INDENT IS NULL AND WORKORDER IS NOT NULL AND to_char(TO_DATE(to_char";
                //strQry += "(DF_DATE),'DD-MM-YYYY'),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END)Indent_Pending, sum(CASE WHEN ";
                //strQry += "to_char(TO_DATE(to_char(DF_DATE),'DD-MM-YYYY'),'YYYY-MM')='" + objReport.sMonth + "' AND INVOICE IS NULL AND INDENT IS NOT NULL THEN 1 ";
                //strQry += "ELSE 0 END)Invoice_Pending,sum(CASE WHEN DECOMMISION IS NULL AND INVOICE IS NOT NULL AND to_char(TO_DATE";
                //strQry += "(to_char(DF_DATE),'DD-MM-YYYY'),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END)Decommission_Pending, sum(CASE WHEN ";
                //strQry += "CRREPORT IS NULL AND DECOMMISION IS NOT NULL AND to_char(TO_DATE(to_char(DF_DATE),'DD-MM-YYYY'),'YYYY-MM')";
                //strQry += "='" + objReport.sMonth + "' THEN 1 ELSE 0 END)CR_RI_Pending FROM WORKFLOWSTATUS,VIEWPENDINGFAILURE WHERE DT_CODE=";
                //strQry += "WO_DATA_ID AND OFFCODE like '%' GROUP BY substr(OFFCODE, 0, 2) ORDER BY substr(OFFCODE, 0, 2))a LEFT JOIN ";
                //strQry += "(SELECT substr(wo_office_code, 0, 2) wo_office_code, count(*) est_pend FROM TBLWORKFLOWOBJECTS WHERE ";
                //strQry += "WO_REF_OFFCODE LIKE '%'  AND WO_BO_ID = '9' AND TO_CHAR(WO_CR_ON, 'YYYY-MM')= '" + objReport.sMonth + "'  AND ";
                //strQry += "WO_APPROVE_STATUS = '0' GROUP BY substr(wo_office_code, 0, 2) ORDER BY substr(wo_office_code, 0, 2))b  ";
                //strQry += "ON a.OFFCODE = b.wo_office_code)a LEFT JOIN (SELECT sum(CASE WHEN  DF_STATUS_FLAG IN (1,4) THEN 1 ELSE 0 END)";
                //strQry += "Total_Fail_Pending, substr(DF_LOC_CODE, 0, 2)OFFCODE FROM TBLDTCFAILURE WHERE  DF_STATUS_FLAG IN (1,4) AND ";
                //strQry += "TO_CHAR(DF_DATE, 'YYYY-MM')= '" + objReport.sMonth + "' GROUP BY substr(DF_LOC_CODE, 0, 2) ORDER BY  ";
                //strQry += "substr(DF_LOC_CODE, 0, 2))b on a.wo_office_code = b.OFFCODE)A RIGHT JOIN (SELECT to_number(TOTAL_DTC)TOTAL_DTC,";
                //strQry += "DT_DIV FROM(SELECT COUNT(DT_CODE)TOTAL_DTC,SUBSTR(DT_OM_SLNO,0,2)DT_DIV FROM TBLDTCMAST GROUP BY SUBSTR(DT_OM_SLNO,0,2)";
                //strQry += "ORDER BY  SUBSTR(DT_OM_SLNO,0,2)))B ON B.DT_DIV=WO_OFFICE_CODE RIGHT JOIN  TBLDIVISION ON wo_office_code = DIV_CODE";
                #endregion

                //strQry = "SELECT \"CURRENT_MONTH\",\"SELECTED_MONTH\",\"PENDING_FOR_REPLACEMENT\",\"DIV_NAME\",\"Total_Fail_Pending\",\"EST_PEND\",WO_Pending,Indent_Pending,";
                //strQry += "\"Invoice_Pending\",\"Decommission_Pending\",\"CR_RI_Pending\",\"wo_office_code\",\"CR_COMPLETED\",CAST(\"TOTAL_DTC\" AS INT)\"TOTAL_DTC\",'" + objReport.sReportType + "' as \"ReportType\" FROM ";
                //strQry += "(SELECT to_char(CURRENT_DATE,'MONTH YYYY')as \"CURRENT_MONTH\", to_char(to_date('" + objReport.sMonth + "','YYYY-MM'),'Month yyyy') as ";
                //strQry += "\"SELECTED_MONTH\" , CAST(COALESCE(\"WO_PENDING\",0)+COALESCE(\"INDENT_PENDING\",0)+COALESCE(\"INVOICE_PENDING\",0)+COALESCE";
                //strQry += "(\"Decommission_Pending\",0) AS INT) as  \"PENDING_FOR_REPLACEMENT\",CAST(COALESCE(\"Total_Fail_Pending\",0) AS INT)\"Total_Fail_Pending\",";
                //strQry += "CAST(COALESCE(\"est_pend\",0) AS INT) \"est_pend\",CAST(\"WO_Pending\" AS INT) \"WO_Pending\",CAST(\"Indent_Pending\" AS INT) \"Indent_Pending\",";
                //strQry += "CAST(COALESCE(\"Invoice_Pending\",0)+COALESCE(\"Decommission_Pending\",0) AS INT)\"Invoice_Pending\",CAST(\"Decommission_Pending\" AS INT)\"Decommission_Pending\",";
                //strQry += "CAST(\"CR_RI_Pending\" AS INT)\"CR_RI_Pending\",\"wo_office_code\",COALESCE(\"CR_COMPLETED\",0)\"CR_COMPLETED\" FROM ";
                //strQry += "(SELECT \"WO_Pending\",\"Indent_Pending\",\"Invoice_Pending\",\"Decommission_Pending\", \"CR_RI_Pending\",\"est_pend\",\"wo_office_code\",";
                //strQry += "\"Total_Fail_Pending\" from (SELECT \"WO_Pending\",\"Indent_Pending\",\"est_pend\",\"Invoice_Pending\",\"Decommission_Pending\", ";
                //strQry += "\"CR_RI_Pending\",a.\"OFFCODE\" as \"wo_office_code\" from (SELECT substr(CAST(\"OFFCODE\"AS TEXT),0,2)\"OFFCODE\", sum(CASE WHEN \"WORKORDER\" IS NULL ";
                //strQry += "AND \"FAILURE\" IS NOT NULL AND to_char(TO_DATE(CAST(\"DF_DATE\" AS TEXT),'DD-MM-YYYY'),'YYYY-MM')='" + objReport.sMonth + "'  ";
                //strQry += "THEN 1 ELSE 0 END)\"WO_Pending\", sum(CASE WHEN \"INDENT\" IS NULL AND \"WORKORDER\" IS NOT NULL AND ";
                //strQry += "to_char(TO_DATE(CAST(\"DF_DATE\" AS TEXT),'DD-MM-YYYY'),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END)\"Indent_Pending\", ";
                //strQry += "sum(CASE WHEN to_char(TO_DATE(CAST(\"DF_DATE\" AS TEXT),'DD-MM-YYYY'),'YYYY-MM')='" + objReport.sMonth + "' AND \"INVOICE\" IS NULL AND ";
                //strQry += "\"INDENT\" IS NOT NULL THEN 1 ELSE 0 END)\"Invoice_Pending\",sum(CASE WHEN \"DECOMMISION\" IS NULL AND \"INVOICE\" IS NOT NULL AND ";
                //strQry += "to_char(TO_DATE(CAST(\"DF_DATE\" AS TEXT),'DD-MM-YYYY'),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END)\"Decommission_Pending\", ";
                //strQry += "sum(CASE WHEN \"CRREPORT\" IS NULL AND \"DECOMMISION\" IS NOT NULL AND ";
                //strQry += "to_char(TO_DATE(CAST(\"DF_DATE\" AS TEXT),'DD-MM-YYYY'),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END)\"CR_RI_Pending\" FROM ";
                //strQry += "\"WORKFLOWSTATUSDUMMY\",\"VIEWPENDINGFAILURE\" WHERE \"DT_CODE\"=\"WO_DATA_ID\" AND CAST(\"OFFCODE\" AS TEXT) like '%' AND \"WO_BO_ID\" <>10 GROUP BY substr(CAST(\"OFFCODE\" AS TEXT), 0, 2) ";
                //strQry += "ORDER BY substr(CAST(\"OFFCODE\" AS TEXT), 0, 2))a LEFT JOIN (SELECT substr(CAST(\"wo_office_code\" AS TEXT), 0, 2) \"wo_office_code\", count(*) \"est_pend\" ";
                //strQry += "FROM \"TBLWORKFLOWOBJECTS\" WHERE CAST(\"WO_REF_OFFCODE\" AS TEXT) LIKE '%'  AND CAST(\"WO_BO_ID\" AS TEXT) = '9' AND ";
                //strQry += "TO_CHAR(\"WO_CR_ON\", 'YYYY-MM')= '" + objReport.sMonth + "'  AND CAST(\"WO_APPROVE_STATUS\" AS TEXT)= '0' GROUP BY substr(CAST(\"wo_office_code\" AS TEXT), 0, 2) ";
                //strQry += "ORDER BY SUBSTR(CAST(\"WO_OFFICE_CODE\" AS TEXT), 0, " + Constants.Division + "))b  ON a.\"OFFCODE\" = b.\"wo_office_code\")a LEFT JOIN (SELECT sum(CASE WHEN  ";
                //strQry += "\"DF_STATUS_FLAG\" IN (1,4) THEN 1 ELSE 0 END)\"Total_Fail_Pending\", substr(CAST(\"DF_LOC_CODE\" AS TEXT), 0, 2) \"OFFCODE\" FROM \"TBLDTCFAILURE\" ";
                //strQry += "WHERE  \"DF_STATUS_FLAG\" IN (1,4) AND TO_CHAR(\"DF_DATE\", 'YYYY-MM')= '" + objReport.sMonth + "' GROUP BY substr(CAST(\"DF_LOC_CODE\" AS TEXT), 0, 2) ";
                //strQry += "ORDER BY  substr(CAST(\"DF_LOC_CODE\" AS TEXT), 0, 2))b on a.\"wo_office_code\" = b.\"OFFCODE\")A LEFT JOIN ";
                //strQry += "(SELECT substr(CAST(\"DF_LOC_CODE\" AS TEXT), 0, 2) \"OFFCODE\", count(*) \"CR_COMPLETED\" FROM \"TBLDTCFAILURE\" WHERE  \"DF_REPLACE_FLAG\"='1' AND ";
                //strQry += "\"DF_STATUS_FLAG\" IN (1,4) AND TO_CHAR(\"DF_DATE\", 'YYYY-MM')= '" + objReport.sMonth + "' GROUP BY substr(CAST(\"DF_LOC_CODE\" AS TEXT), 0, 2) )B ON A.\"WO_OFFICE_CODE\"=B.\"OFFCODE\")A RIGHT ";
                //strQry += "JOIN (SELECT CAST(\"TOTAL_DTC\" AS INT)\"TOTAL_DTC\",\"DT_DIV\" FROM(SELECT COUNT(\"DT_CODE\")\"TOTAL_DTC\",SUBSTR(CAST(\"DT_OM_SLNO\" AS TEXT),0,2)\"DT_DIV\" ";
                //strQry += "FROM \"TBLDTCMAST\" GROUP BY SUBSTR(CAST(\"DT_OM_SLNO\" AS TEXT),0,2)ORDER BY SUBSTR(CAST(\"DT_OM_SLNO\" AS TEXT),0,2)))B ON B.\"DT_DIV\"=\"WO_OFFICE_CODE\" RIGHT ";
                //strQry += "JOIN \"TBLDIVISION\" ON \"wo_office_code\" = \"DIV_CODE\"";TO_CHAR(CAST("DF_DATE" AS DATE),'YYYY-MM')

                //strQry = "SELECT \"CURRENT_MONTH\",\"SELECTED_MONTH\",\"PENDING_FOR_REPLACEMENT\",\"DIV_NAME\",\"Total_Fail_Pending\",\"est_pend\",\"WO_Pending\"";
                //strQry += ",\"Indent_Pending\",\"Invoice_Pending\",\"Decommission_Pending\",\"CR_RI_Pending\",\"wo_office_code\",\"CR_COMPLETED\",CAST(\"TOTAL_DTC\" AS INT)";
                //strQry += "\"TOTAL_DTC\",'" + objReport.sReportType + "' as \"ReportType\" FROM (SELECT to_char(CURRENT_DATE,'MONTH YYYY')as \"CURRENT_MONTH\", to_char(to_date('" + objReport.sMonth + "','YYYY-MM'),'Month yyyy') as \"SELECTED_MONTH\"";
                //strQry += ", CAST(COALESCE(\"WO_Pending\",0)+COALESCE(\"Indent_Pending\",0)+COALESCE(\"Invoice_Pending\",0)+COALESCE(\"Decommission_Pending\",0) AS INT) as  ";
                //strQry += "\"PENDING_FOR_REPLACEMENT\",CAST(COALESCE(\"Total_Fail_Pending\",0) AS INT)\"Total_Fail_Pending\",CAST(COALESCE(\"est_pend\",0) AS INT) \"est_pend\",";
                //strQry += "CAST(\"WO_Pending\" AS INT) \"WO_Pending\",CAST(\"Indent_Pending\" AS INT) \"Indent_Pending\",CAST(COALESCE(\"Invoice_Pending\",0)+COALESCE";
                //strQry += "(\"Decommission_Pending\",0) AS INT)\"Invoice_Pending\",CAST(\"Decommission_Pending\" AS INT)\"Decommission_Pending\",CAST(\"CR_RI_Pending\" AS INT)\"CR_RI_Pending\"";
                //strQry += ",\"wo_office_code\",COALESCE(\"CR_COMPLETED\",0)\"CR_COMPLETED\" FROM (SELECT \"WO_Pending\",\"Indent_Pending\",\"Invoice_Pending\",";
                //strQry += "\"Decommission_Pending\",\"CR_RI_Pending\",\"est_pend\",\"wo_office_code\",\"Total_Fail_Pending\" from (SELECT \"WO_Pending\",\"Indent_Pending\",";
                //strQry += "\"est_pend\",\"Invoice_Pending\",\"Decommission_Pending\", \"CR_RI_Pending\",a.\"OFFCODE\" as \"wo_office_code\" from (SELECT substr(CAST(\"OFFCODE\"";
                //strQry += "AS TEXT),1," + Constants.Division + ")\"OFFCODE\", sum(CASE WHEN \"WORKORDER\" IS NULL AND \"FAILURE\" IS NOT NULL AND TO_CHAR(CAST(\"DF_DATE\" AS DATE),'YYYY-MM')='" + objReport.sMonth + "'";
                //strQry += "THEN 1 ELSE 0 END)\"WO_Pending\", sum(CASE WHEN \"INDENT\" IS NULL AND \"WORKORDER\" IS NOT NULL AND TO_CHAR(CAST(\"DF_DATE\" AS DATE),'YYYY-MM')";
                //strQry += "='" + objReport.sMonth + "'THEN 1 ELSE 0 END)\"Indent_Pending\", sum(CASE WHEN TO_CHAR(CAST(\"DF_DATE\" AS DATE),'YYYY-MM')='" + objReport.sMonth + "'";
                //strQry += "AND \"INVOICE\" IS NULL AND \"INDENT\" IS NOT NULL THEN 1 ELSE 0 END)\"Invoice_Pending\",sum(CASE WHEN \"DECOMMISION\" ";
                //strQry += "IS NULL AND \"INVOICE\" IS NOT NULL AND TO_CHAR(CAST(\"DF_DATE\" AS DATE),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END)\"Decommission_Pending\",";
                //strQry += "sum(CASE WHEN \"CRREPORT\" IS NULL AND \"DECOMMISION\" IS NOT NULL AND TO_CHAR(CAST(\"DF_DATE\" AS DATE),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END)";
                //strQry += "\"CR_RI_Pending\" FROM \"WORKFLOWSTATUSDUMMY\",\"VIEWPENDINGFAILURE\" WHERE \"DT_CODE\"=\"WO_DATA_ID\" AND CAST(\"OFFCODE\" AS TEXT)";
                //strQry += "like '%' AND \"WO_BO_ID\" <>10 GROUP BY substr(CAST(\"OFFCODE\" AS TEXT), 1, " + Constants.Division + ") ORDER BY substr(CAST(\"OFFCODE\" AS TEXT), 1, " + Constants.Division + "))a LEFT JOIN";
                //strQry += "(SELECT substr(CAST(\"WO_OFFICE_CODE\" AS TEXT), 1, " + Constants.Division + ") \"wo_office_code\", count(*) \"est_pend\" FROM \"TBLWORKFLOWOBJECTS\" WHERE ";
                //strQry += "CAST(\"WO_REF_OFFCODE\" AS TEXT) LIKE '%'  AND CAST(\"WO_BO_ID\" AS TEXT) = '9' AND TO_CHAR(\"WO_CR_ON\", 'YYYY-MM')= '" + objReport.sMonth + "'  AND ";
                //strQry += "CAST(\"WO_APPROVE_STATUS\" AS TEXT)= '1' GROUP BY substr(CAST(\"WO_OFFICE_CODE\" AS TEXT), 1, " + Constants.Division + ") ORDER BY \"wo_office_code\")b  ";
                //strQry += "ON a.\"OFFCODE\" = b.\"wo_office_code\")a LEFT JOIN (SELECT sum(CASE WHEN  \"DF_STATUS_FLAG\" IN (1,4) THEN 1 ELSE 0 END)\"Total_Fail_Pending\",";
                //strQry += "substr(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.Division + ") \"OFFCODE\" FROM \"TBLDTCFAILURE\" WHERE  \"DF_STATUS_FLAG\" IN (1,4) AND TO_CHAR(CAST(\"DF_DATE\" AS DATE),'YYYY-MM')=";
                //strQry += "'" + objReport.sMonth + "' GROUP BY substr(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.Division + ") ORDER BY  substr(CAST(\"DF_LOC_CODE\" AS TEXT), 1," + Constants.Division + "))b on a.\"wo_office_code\" =";
                //strQry += "b.\"OFFCODE\")A LEFT JOIN (SELECT substr(CAST(\"DF_LOC_CODE\" AS TEXT), 1," + Constants.Division + ") \"OFFCODE\", count(*) \"CR_COMPLETED\" FROM \"TBLDTCFAILURE\" WHERE";
                //strQry += "\"DF_REPLACE_FLAG\"='1' AND \"DF_STATUS_FLAG\" IN (1,4) AND TO_CHAR(CAST(\"DF_DATE\" AS DATE),'YYYY-MM')= '" + objReport.sMonth + "' GROUP BY substr";
                //strQry += "(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.Division + ") )B ON A.\"wo_office_code\"=B.\"OFFCODE\")A RIGHT JOIN (SELECT CAST(\"TOTAL_DTC\" AS INT)\"TOTAL_DTC\",\"DT_DIV\"";
                //strQry += "FROM(SELECT COUNT(\"DT_CODE\")\"TOTAL_DTC\",SUBSTR(CAST(\"DT_OM_SLNO\" AS TEXT),1," + Constants.Division + ")\"DT_DIV\" FROM \"TBLDTCMAST\" GROUP BY ";
                //strQry += "SUBSTR(CAST(\"DT_OM_SLNO\" AS TEXT),1," + Constants.Division + ")ORDER BY SUBSTR(CAST(\"DT_OM_SLNO\" AS TEXT),1," + Constants.Division + "))f)B ON B.\"DT_DIV\"=A.\"wo_office_code\" RIGHT JOIN";
                //strQry += "\"TBLDIVISION\" ON \"wo_office_code\" = cast(\"DIV_CODE\" as text)";

                if (objReport.sReportType == "1")
                {
                    strQry = " SELECT  '" + objReport.sReportType + "' as \"ReportType\",to_char(CURRENT_DATE,'MONTH YYYY')as \"CURRENT_MONTH\",to_char(to_date('" + objReport.sMonth + "','YYYY-MM'),'Month yyyy') as \"SELECTED_MONTH\",*,COALESCE((SELECT  count(*) \"CR_COMPLETED\" FROM \"TBLDTCFAILURE\" WHERE\"DF_REPLACE_FLAG\"='1' ";
                    strQry += "  AND \"DF_STATUS_FLAG\" IN (1,4) AND TO_CHAR(CAST(\"DF_DATE\" AS DATE),'YYYY-MM')<'" + objReport.sMonth + "' and ";
                    strQry += " substr(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.Division + ") = substr(CAST(\"TRANS_REF_OFF_CODE\" AS TEXT), 1,  " + Constants.Division + ")  GROUP BY substr(CAST(\"DF_LOC_CODE\" AS TEXT), 1,  " + Constants.Division + ")) ,0) ";
                    strQry += "  as \"CR_COMPLETED\",(SELECT COUNT(\"DT_CODE\")\"TOTAL_DTC\" FROM \"TBLDTCMAST\" WHERE SUBSTR(CAST(\"DT_OM_SLNO\" AS TEXT),1, " + Constants.Division + ")= substr(CAST(\"TRANS_REF_OFF_CODE\" AS TEXT), 1,  " + Constants.Division + ")";
                    strQry += "   GROUP BY SUBSTR(CAST(\"DT_OM_SLNO\" AS TEXT),1, " + Constants.Division + ") ORDER BY SUBSTR(CAST(\"DT_OM_SLNO\" AS TEXT),1,3)) AS \"TOTAL_DTC\"  FROM (SELECT COALESCE(SUM(CASE WHEN \"TRANS_BO_ID\" NOT IN (15,26,10) ";
                    strQry += "   and TO_CHAR(CAST(\"TRANS_UPDATE_DATE\" AS DATE),'YYYY-MM')<'" + objReport.sMonth + "' THEN 1 ELSE 0 END ),0) AS \"Total_Fail_Pending\",COALESCE(SUM(CASE WHEN \"TRANS_BO_ID\" in(45,73) and TO_CHAR(CAST(\"TRANS_UPDATE_DATE\" AS DATE),'YYYY-MM')<'" + objReport.sMonth + "'";
                    strQry += "  THEN 1 ELSE 0 END ),0) AS \"est_pend\",COALESCE(SUM(CASE WHEN \"TRANS_BO_ID\"=46 and TO_CHAR(CAST(\"TRANS_UPDATE_DATE\" AS DATE),'YYYY-MM')<'" + objReport.sMonth + "' THEN 1 ELSE 0 END ),0) AS \"PENDING_FOR_REPLACEMENT\",";
                    strQry += "  COALESCE(SUM(CASE WHEN \"TRANS_BO_ID\"IN (12,29) and TO_CHAR(CAST(\"TRANS_UPDATE_DATE\" AS DATE),'YYYY-MM')<'" + objReport.sMonth + "' THEN 1 ELSE 0 END ),0) AS \"Indent_Pending\" , COALESCE(SUM(CASE WHEN \"TRANS_BO_ID\" in(14,47)";
                    strQry += "  and TO_CHAR(CAST(\"TRANS_UPDATE_DATE\" AS DATE),'YYYY-MM')<'" + objReport.sMonth + "'  THEN 1 ELSE 0 END ) ,0) AS \"Decommission_Pending\",COALESCE(SUM(CASE WHEN \"TRANS_BO_ID\" IN(15,26) and TO_CHAR(CAST(\"TRANS_UPDATE_DATE\" AS DATE),'YYYY-MM')<'" + objReport.sMonth + "' THEN 1 ELSE 0 END ),0) ";
                    strQry += "   AS \"CR_RI_Pending\",COALESCE(SUM(CASE WHEN \"TRANS_BO_ID\"=13 and TO_CHAR(CAST(\"TRANS_UPDATE_DATE\" AS DATE),'YYYY-MM')<'" + objReport.sMonth + "' THEN 1 ELSE 0 END ),0) AS \"Invoice_Pending\", COALESCE(SUM(CASE WHEN \"TRANS_BO_ID\" in(11,74) and";
                    strQry += " TO_CHAR(CAST(\"TRANS_UPDATE_DATE\" AS DATE),'YYYY-MM')<'" + objReport.sMonth + "' THEN 1 ELSE 0 END ),0)AS \"WO_Pending\",substr(CAST(\"TRANS_REF_OFF_CODE\" AS TEXT), 1, 3) AS \"TRANS_REF_OFF_CODE\" FROM \"TBLPENDINGTRANSACTION\"  ";
                    strQry += "  WHERE CAST(\"TRANS_REF_OFF_CODE\" AS TEXT) LIKE '%' GROUP BY substr(CAST(\"TRANS_REF_OFF_CODE\" AS TEXT), 1,  " + Constants.Division + ") )A RIGHT JOIN  (SELECT \"DIV_CODE\" AS \"wo_office_code\",\"DIV_NAME\" FROM \"TBLDIVISION\")B ON CAST(\"wo_office_code\" AS text)=substr(CAST(\"TRANS_REF_OFF_CODE\" AS TEXT), 1,  " + Constants.Division + ")";
                }
                else
                {
                    strQry = " SELECT  '" + objReport.sReportType + "' as \"ReportType\",to_char(CURRENT_DATE,'MONTH YYYY')as \"CURRENT_MONTH\",to_char(to_date('" + objReport.sMonth + "','YYYY-MM'),'Month yyyy') as \"SELECTED_MONTH\",*,COALESCE((SELECT  count(*) \"CR_COMPLETED\" FROM \"TBLDTCFAILURE\" WHERE\"DF_REPLACE_FLAG\"='1' ";
                    strQry += "  AND \"DF_STATUS_FLAG\" IN (1,4) AND TO_CHAR(CAST(\"DF_DATE\" AS DATE),'YYYY-MM')='" + objReport.sMonth + "' and ";
                    strQry += " substr(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.Division + ") = substr(CAST(\"TRANS_REF_OFF_CODE\" AS TEXT), 1,  " + Constants.Division + ")  GROUP BY substr(CAST(\"DF_LOC_CODE\" AS TEXT), 1,  " + Constants.Division + ")) ,0) ";
                    strQry += "  as \"CR_COMPLETED\",(SELECT COUNT(\"DT_CODE\")\"TOTAL_DTC\" FROM \"TBLDTCMAST\" WHERE SUBSTR(CAST(\"DT_OM_SLNO\" AS TEXT),1, " + Constants.Division + ")= substr(CAST(\"TRANS_REF_OFF_CODE\" AS TEXT), 1,  " + Constants.Division + ")";
                    strQry += "   GROUP BY SUBSTR(CAST(\"DT_OM_SLNO\" AS TEXT),1, " + Constants.Division + ") ORDER BY SUBSTR(CAST(\"DT_OM_SLNO\" AS TEXT),1,3)) AS \"TOTAL_DTC\"  FROM (SELECT COALESCE(SUM(CASE WHEN \"TRANS_BO_ID\" NOT IN (15,26,10) ";
                    strQry += "   and TO_CHAR(CAST(\"TRANS_UPDATE_DATE\" AS DATE),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END ),0) AS \"Total_Fail_Pending\",COALESCE(SUM(CASE WHEN \"TRANS_BO_ID\" in(45,73) and TO_CHAR(CAST(\"TRANS_UPDATE_DATE\" AS DATE),'YYYY-MM')='" + objReport.sMonth + "'";
                    strQry += "  THEN 1 ELSE 0 END ),0) AS \"est_pend\",COALESCE(SUM(CASE WHEN \"TRANS_BO_ID\"=46 and TO_CHAR(CAST(\"TRANS_UPDATE_DATE\" AS DATE),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END ),0) AS \"PENDING_FOR_REPLACEMENT\",";
                    strQry += "  COALESCE(SUM(CASE WHEN \"TRANS_BO_ID\"IN (12,29) and TO_CHAR(CAST(\"TRANS_UPDATE_DATE\" AS DATE),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END ),0) AS \"Indent_Pending\" , COALESCE(SUM(CASE WHEN \"TRANS_BO_ID\" in(14,47)";
                    strQry += "  and TO_CHAR(CAST(\"TRANS_UPDATE_DATE\" AS DATE),'YYYY-MM')='" + objReport.sMonth + "'  THEN 1 ELSE 0 END ) ,0) AS \"Decommission_Pending\",COALESCE(SUM(CASE WHEN \"TRANS_BO_ID\" IN(15,26) and TO_CHAR(CAST(\"TRANS_UPDATE_DATE\" AS DATE),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END ),0) ";
                    strQry += "   AS \"CR_RI_Pending\",COALESCE(SUM(CASE WHEN \"TRANS_BO_ID\"=13 and TO_CHAR(CAST(\"TRANS_UPDATE_DATE\" AS DATE),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END ),0) AS \"Invoice_Pending\", COALESCE(SUM(CASE WHEN \"TRANS_BO_ID\" in(11,74) and";
                    strQry += " TO_CHAR(CAST(\"TRANS_UPDATE_DATE\" AS DATE),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END ),0)AS \"WO_Pending\",substr(CAST(\"TRANS_REF_OFF_CODE\" AS TEXT), 1, 3) AS \"TRANS_REF_OFF_CODE\" FROM \"TBLPENDINGTRANSACTION\"  ";
                    strQry += "  WHERE CAST(\"TRANS_REF_OFF_CODE\" AS TEXT) LIKE '%' GROUP BY substr(CAST(\"TRANS_REF_OFF_CODE\" AS TEXT), 1,  " + Constants.Division + ") )A RIGHT JOIN  (SELECT \"DIV_CODE\" AS \"wo_office_code\",\"DIV_NAME\" FROM \"TBLDIVISION\")B ON CAST(\"wo_office_code\" AS text)=substr(CAST(\"TRANS_REF_OFF_CODE\" AS TEXT), 1,  " + Constants.Division + ")";


                }

                dtFailureAbstract = ObjCon.FetchDataTable(strQry);
                return dtFailureAbstract;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtFailureAbstract;
            }

        }

        public DataTable FailMonthWiseAbstract(clsReports objReport)
        {
            string strQry = string.Empty;
            DataTable dtFailureMonthWiseAbstract = new DataTable();
            try
            {

                if (objReport.sReportType == "1")
                {
                    #region After adding OB+Current_month
                    //strQry = " SELECT \"SD_SUBDIV_CODE\",(SELECT SUBSTR(CAST(\"OFF_NAME\" AS TEXT),STRPOS(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT) =SUBSTR(CAST(\"SD_SUBDIV_CODE\" AS TEXT),1," + Constants.Circle + "))";
                    //strQry += "\"CIRCLE\",(SELECT SUBSTR(CAST(\"OFF_NAME\" AS TEXT),STRPOS(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT) =SUBSTR(CAST(\"SD_SUBDIV_CODE\" AS TEXT),1," + Constants.Division + "))";
                    //strQry += "\"DIVISION\",(SELECT SUBSTR(CAST(\"OFF_NAME\" AS TEXT),STRPOS(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE \"OFF_CODE\" =\"SD_SUBDIV_CODE\") ";
                    //strQry += "\"SUBDIVISION\",\"CAPACITY\",\"OB\",\"CURRENT_MONTH\",\"REPLACED_DURING_THIS_MONTH\",\"TO_BE_REPLACE\",\"CR_COMPLETED\",(\"TO_BE_REPLACE\"+ COALESCE(\"TO_BE_REPLACE2\",0))\"TO_BE_REPLACE_ALL\",";
                    //strQry += "(\"REPLACED_DURING_THIS_MONTH\"+ COALESCE(\"REPLACED_DURING_MONTH2\",0))\"REPLACED_TILL_MONTH\",'1' as \"ReportType\" FROM (SELECT \"ISUBDIV\" AS \"SD_SUBDIV_CODE\",";
                    //strQry += "\"WO_PENDING\",\"INDENT_PENDING\",\"INVOICE_PENDING\",\"CR_RI_PENDING\",\"CAPACITY\",\"EST_PEND\",\"OB\",\"REPLACED_CURR_MONTH\" AS \"REPLACED_DURING_THIS_MONTH\",\"TO_BE_REPLACE\",";
                    //strQry += "\"CR_COMPLETE\" AS \"CR_COMPLETED\",COALESCE(\"TOTAL_FAIL_PENDING\",0)\"CURRENT_MONTH\" FROM (SELECT \"ISUBDIV\",\"WO_PENDING\",\"INDENT_PENDING\",\"INVOICE_PENDING\",\"CR_RI_PENDING\",";
                    //strQry += "\"CAPACITY\",\"EST_PEND\",\"OB\",\"REPLACED_CURR_MONTH\",\"TO_BE_REPLACE\",COALESCE(\"CR_COMPLETE\",0)\"CR_COMPLETE\" FROM (SELECT \"ISUBDIV\",\"WO_PENDING\",\"INDENT_PENDING\",";
                    //strQry += "\"INVOICE_PENDING\",\"CR_RI_PENDING\",\"CAPACITY\",\"EST_PEND\",\"OB\",\"REPLACED_CURR_MONTH\",COALESCE(\"TO_BE_REPLACE\",0)\"TO_BE_REPLACE\" FROM (SELECT DISTINCT \"ISUBDIV\",";
                    //strQry += "\"WO_PENDING\",\"INDENT_PENDING\",\"INVOICE_PENDING\",\"CR_RI_PENDING\",\"CAPACITY\",\"EST_PEND\",\"OB\",COALESCE(\"REPLACED_CURR_MONTH\",0)\"REPLACED_CURR_MONTH\" FROM";
                    //strQry += "(SELECT \"ISUBDIV\",\"WO_PENDING\",\"INDENT_PENDING\",\"INVOICE_PENDING\",\"CR_RI_PENDING\",\"CAPACITY\",\"EST_PEND\",COALESCE(\"OB\",0)\"OB\" FROM (SELECT \"ISUBDIV\",";
                    //strQry += "\"WO_PENDING\",\"INDENT_PENDING\",\"INVOICE_PENDING\",\"CR_RI_PENDING\",\"CAPACITY\",COALESCE(\"EST_PEND\",0)\"EST_PEND\" FROM(SELECT \"SUBDIV_CODE\" AS \"ISUBDIV\"";
                    //strQry += ",COALESCE(\"WO_PENDING\",0)\"WO_PENDING\",COALESCE(\"INDENT_PENDING\",0)\"INDENT_PENDING\",COALESCE(\"INVOICE_PENDING\",0)\"INVOICE_PENDING\",COALESCE(\"CR_RI_PENDING\",0)";
                    //strQry += "\"CR_RI_PENDING\",\"CAPACITY\" FROM (SELECT SUBSTR(CAST(\"OFFCODE\" AS TEXT),1," + Constants.SubDivision + ") \"ISUBDIV\",\"TC_CAPACITY\", SUM(CASE WHEN \"WORKORDER\" IS NULL AND";
                    //strQry += "\"FAILURE\" IS NOT NULL AND to_char(CAST(C.\"DF_DATE\" AS DATE),'YYYY-MM')='2018-03' THEN 1 ELSE 0 END)\"WO_PENDING\", SUM(CASE WHEN \"INDENT\" IS NULL AND \"WORKORDER\" IS NOT NULL AND to_char(CAST(C.\"DF_DATE\" AS DATE),'YYYY-MM')='2018-03' THEN 1 ELSE 0 END)\"INDENT_PENDING\", ";
                    //strQry += "SUM(CASE WHEN to_char(CAST(C.\"DF_DATE\" AS DATE),'YYYY-MM')='2018-03' AND \"INVOICE\" IS NULL AND \"INDENT\" IS NOT NULL THEN 1 ELSE 0 END)";
                    //strQry += "\"INVOICE_PENDING\",sum(CASE WHEN \"DECOMMISION\" IS NULL AND \"INVOICE\" IS NOT NULL AND to_char(CAST(C.\"DF_DATE\" AS DATE),'YYYY-MM')='2018-03' ";
                    //strQry += "THEN 1 ELSE 0 END)\"Decommission_Pending\",SUM(CASE WHEN \"CRREPORT\" IS NULL AND \"DECOMMISION\" IS NOT NULL AND to_char(CAST(C.\"DF_DATE\" AS DATE),'YYYY-MM')= '2018-03'";
                    //strQry += "THEN 1 ELSE 0 END)\"CR_RI_PENDING\" FROM \"WORKFLOWSTATUSDUMMY\",\"VIEWPENDINGFAILURE\" C,\"TBLDTCFAILURE\" A,\"TBLTCMASTER\" B WHERE ";
                    //strQry += "\"WO_DATA_ID\"=A.\"DF_DTC_CODE\" AND A.\"DF_EQUIPMENT_ID\"=B.\"TC_CODE\" AND \"WO_DATA_ID\"=\"DT_CODE\" AND \"WO_BO_ID\" <>10 AND ";
                    //strQry += "CAST(\"OFFCODE\" AS TEXT) LIKE '%' GROUP BY SUBSTR(CAST(\"OFFCODE\" AS TEXT),1," + Constants.SubDivision + "),\"TC_CAPACITY\" ORDER BY ";
                    //strQry += "SUBSTR(CAST(\"OFFCODE\" AS TEXT),1," + Constants.SubDivision + "))A RIGHT JOIN (SELECT \"SD_SUBDIV_CODE\" AS \"SUBDIV_CODE\",\"MD_NAME\" ";
                    //strQry += "AS \"CAPACITY\" FROM \"TBLSUBDIVMAST\",\"TBLMASTERDATA\" WHERE \"MD_TYPE\"='C'  AND CAST(\"MD_NAME\" AS INTEGER) <=500 ORDER BY ";
                    //strQry += "CAST(\"MD_NAME\" AS INTEGER))B ON \"ISUBDIV\"=CAST(\"SUBDIV_CODE\" AS TEXT) AND \"CAPACITY\"=CAST(\"TC_CAPACITY\" AS TEXT) ORDER BY ";
                    //strQry += "\"ISUBDIV\",\"CAPACITY\")A LEFT JOIN (SELECT SUBSTR(CAST(\"WO_OFFICE_CODE\" AS TEXT) ,1, " + Constants.SubDivision + ") \"SUBDIV\", COUNT(*)";
                    //strQry += "\"EST_PEND\" FROM \"TBLWORKFLOWOBJECTS\" WHERE CAST(\"WO_REF_OFFCODE\" AS TEXT) LIKE '%'  AND \"WO_BO_ID\" = '9' AND ";
                    //strQry += "TO_CHAR(\"WO_CR_ON\", 'YYYY-MM')= '2018-03' AND \"WO_APPROVE_STATUS\" = '0' GROUP BY SUBSTR(CAST(\"WO_OFFICE_CODE\" AS TEXT),1," + Constants.SubDivision + ") ";
                    //strQry += "ORDER BY SUBSTR(CAST(\"WO_OFFICE_CODE\" AS TEXT),1," + Constants.SubDivision + "))B ON CAST(A.\"ISUBDIV\" AS TEXT) = B.\"SUBDIV\" )A  LEFT JOIN";
                    //strQry += " (SELECT COUNT(*) AS \"OB\",SUBSTR(CAST(\"OFFCODE\" AS TEXT), 1, " + Constants.SubDivision + ") AS \"SUBDIV\",\"TC_CAPACITY\" FROM ";
                    //strQry += "\"WORKFLOWSTATUSDUMMY\" LEFT JOIN \"TBLDTCFAILURE\" ON \"WO_DATA_ID\"=\"DF_DTC_CODE\" LEFT JOIN \"TBLDTCMAST\" ON \"WO_DATA_ID\" =\"DT_CODE\" ";
                    //strQry += "INNER JOIN \"TBLTCMASTER\" ON \"TC_CODE\"=\"DT_TC_ID\" WHERE  \"DECOMMISION\" IS NULL  AND CAST(\"OFFCODE\" AS TEXT) LIKE '%' AND \"WO_BO_ID\" <>10 ";
                    //strQry += "AND \"DF_REPLACE_FLAG\"='0' AND \"DT_TC_ID\"<>0 AND TO_CHAR(\"DF_DATE\", 'YYYY-MM')< '2018-03' GROUP BY SUBSTR(CAST(\"OFFCODE\" AS TEXT),1," + Constants.SubDivision + ")";
                    //strQry += ",\"TC_CAPACITY\")B ON CAST(A.\"ISUBDIV\" AS TEXT)=B.\"SUBDIV\" AND A.\"CAPACITY\"=CAST(B.\"TC_CAPACITY\" AS TEXT))A LEFT JOIN (SELECT count(*)\"REPLACED_CURR_MONTH\",";
                    //strQry += "SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1," + Constants.SubDivision + ")\"SUBDIV\",\"TC_CAPACITY\" FROM \"TBLDTCFAILURE\",\"TBLWORKORDER\",\"TBLINDENT\"";
                    //strQry += ",\"TBLDTCINVOICE\",\"TBLTCREPLACE\",\"TBLTCMASTER\" WHERE \"DF_EQUIPMENT_ID\"=\"TC_CODE\" AND \"DF_ID\"=\"WO_DF_ID\" AND \"WO_SLNO\" =\"TI_WO_SLNO\" AND";
                    //strQry += "\"TI_ID\"=\"IN_TI_NO\" AND \"IN_NO\" =\"TR_IN_NO\" AND  \"DF_STATUS_FLAG\" IN (1," + Constants.SubDivision + ") AND \"TR_RI_DATE\" IS NOT NULL ";
                    //strQry += "AND to_char(CAST(\"DF_DATE\" AS DATE),'YYYY-MM')= '2018-03' AND \"DF_REPLACE_FLAG\"='0'  GROUP BY SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ")";
                    //strQry += ",\"TC_CAPACITY\" ORDER BY SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + "))B ON CAST(A.\"ISUBDIV\" AS TEXT)=B.\"SUBDIV\"  AND ";
                    //strQry += "A.\"CAPACITY\"=CAST(\"TC_CAPACITY\" AS TEXT))A LEFT JOIN (SELECT COUNT(*) AS \"TO_BE_REPLACE\",SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.SubDivision + ")";
                    //strQry += "AS \"SUBDIV\",\"TC_CAPACITY\" FROM \"TBLDTCFAILURE\" LEFT JOIN \"TBLWORKORDER\" ON \"DF_ID\"=\"WO_DF_ID\" LEFT JOIN \"TBLINDENT\" ON \"WO_SLNO\" =\"TI_WO_SLNO\" LEFT JOIN";
                    //strQry += "\"TBLDTCINVOICE\" ON \"TI_ID\"=\"IN_TI_NO\" LEFT JOIN \"TBLTCREPLACE\" ON \"IN_NO\" =\"TR_IN_NO\" ,\"TBLTCMASTER\" WHERE \"DF_EQUIPMENT_ID\" =\"TC_CODE\" AND  ";
                    //strQry += "\"DF_STATUS_FLAG\" IN (1," + Constants.SubDivision + ") AND \"TR_RI_DATE\" IS NULL AND to_char(CAST(\"DF_DATE\" AS DATE),'YYYY-MM')= '2018-03' AND \"DF_REPLACE_FLAG\" ='0'";
                    //strQry += "GROUP BY SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1," + Constants.SubDivision + "),\"TC_CAPACITY\" ORDER BY \"SUBDIV\")B ON CAST(A.\"ISUBDIV\" AS TEXT)=B.\"SUBDIV\"";
                    //strQry += "AND A.\"CAPACITY\"=CAST(B.\"TC_CAPACITY\" AS TEXT))A LEFT JOIN (SELECT COUNT(\"DF_DTC_CODE\") AS \"CR_COMPLETE\",SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.SubDivision + ")";
                    //strQry += "\"SUBDIV\",\"TC_CAPACITY\" FROM \"TBLDTCFAILURE\",\"TBLTCMASTER\" WHERE \"TC_CODE\" =\"DF_EQUIPMENT_ID\" AND TO_CHAR(CAST(\"DF_DATE\" AS DATE),'YYYY-MM')= '2018-03' AND";
                    //strQry += "\"DF_STATUS_FLAG\" IN (1," + Constants.SubDivision + ") AND \"DF_REPLACE_FLAG\" ='1' GROUP BY SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + "),";
                    //strQry += "\"TC_CAPACITY\" ORDER BY SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + "),\"TC_CAPACITY\")B ON CAST(A.\"ISUBDIV\" AS TEXT)=";
                    //strQry += "B.\"SUBDIV\" AND A.\"CAPACITY\" = CAST(B.\"TC_CAPACITY\" AS TEXT))A LEFT JOIN (SELECT SUM(CASE WHEN  \"DF_STATUS_FLAG\" IN (1," + Constants.SubDivision + ")";
                    //strQry += " THEN 1 ELSE 0 END)\"TOTAL_FAIL_PENDING\",SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1," + Constants.SubDivision + ")\"SUBDIV\",\"TC_CAPACITY\" FROM \"TBLDTCFAILURE\",";
                    //strQry += "\"TBLTCMASTER\" WHERE \"TC_CODE\"=\"DF_EQUIPMENT_ID\" AND \"DF_STATUS_FLAG\" IN (1,4) AND TO_CHAR(CAST(\"DF_DATE\" AS DATE),'YYYY-MM')= '2018-03' ";
                    //strQry += "GROUP BY SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1," + Constants.SubDivision + "),\"TC_CAPACITY\" ORDER BY  SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1," + Constants.SubDivision + "),";
                    //strQry += "\"TC_CAPACITY\")B ON CAST(A.\"ISUBDIV\" AS TEXT)=B.\"SUBDIV\" AND A.\"CAPACITY\"=CAST(B.\"TC_CAPACITY\" AS TEXT))A LEFT JOIN (SELECT sum(CASE WHEN ";
                    //strQry += "\"TR_RI_NO\" IS NULL THEN 1 ELSE 0 END) \"TO_BE_REPLACE2\",sum(CASE WHEN \"TR_RI_NO\" IS NOT NULL THEN 1 ELSE 0 END) \"REPLACED_DURING_MONTH2\",";
                    //strQry += "SUBSTR(CAST(\"OM_CODE\" AS TEXT), 1," + Constants.SubDivision + ")\"SUBDIV\",\"TC_CAPACITY\" FROM \"VIEWPENDINGFAILURE\",\"TBLDTCFAILURE\",\"TBLTCMASTER\"";
                    //strQry += " WHERE \"DT_CODE\" =\"DF_DTC_CODE\" AND \"DF_EQUIPMENT_ID\"=\"TC_CODE\" AND \"DF_STATUS_FLAG\" IN (1," + Constants.SubDivision + ") AND \"DF_REPLACE_FLAG\"='0'";
                    //strQry += "  AND  TO_CHAR(CAST(\"TBLDTCFAILURE\".\"DF_DATE\" AS DATE), 'YYYY-MM')< '2018-03' AND \"DT_CODE\" IN (SELECT \"DT_CODE\" FROM \"WORKFLOWSTATUSDUMMY\"";
                    //strQry += " LEFT JOIN \"TBLDTCFAILURE\" ON \"WO_DATA_ID\"=\"DF_DTC_CODE\" LEFT JOIN \"TBLDTCMAST\" ON \"WO_DATA_ID\" =\"DT_CODE\" INNER JOIN \"TBLTCMASTER\" ON  ";
                    //strQry += "\"TC_CODE\"=\"DT_TC_ID\" WHERE \"DT_TC_ID\" <>0 AND  \"DECOMMISION\" IS NULL  AND CAST(\"OFFCODE\" AS TEXT) LIKE '%' AND \"WO_BO_ID\" <>10 AND";
                    //strQry += " \"DF_REPLACE_FLAG\"='0' AND  TO_CHAR(CAST(\"DF_DATE\" AS DATE),'YYYY-MM')< '2018-03'  LIMIT (SELECT COUNT(*) FROM \"TBLDTCMAST\")) GROUP BY SUBSTR(CAST(\"OM_CODE\"";
                    //strQry += " AS TEXT), 1," + Constants.SubDivision + "),\"TC_CAPACITY\")B ON CAST(A.\"SD_SUBDIV_CODE\" AS TEXT)=B.\"SUBDIV\" AND A.\"CAPACITY\"=CAST(B.\"TC_CAPACITY\" AS TEXT) ORDER BY \"SD_SUBDIV_CODE\"";


                    strQry = " SELECT \"SD_SUBDIV_CODE\",(SELECT SUBSTR(CAST(\"OFF_NAME\" AS TEXT),STRPOS(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT) =SUBSTR(CAST(\"SD_SUBDIV_CODE\" AS TEXT),1,2))\"CIRCLE\",(SELECT SUBSTR(CAST(\"OFF_NAME\" AS TEXT),";
                    strQry += "  STRPOS(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT) =SUBSTR(CAST(\"SD_SUBDIV_CODE\" AS TEXT),1,3))\"DIVISION\",(SELECT SUBSTR(CAST(\"OFF_NAME\" AS TEXT),STRPOS(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE \"OFF_CODE\" =\"SD_SUBDIV_CODE\") \"SUBDIVISION\",\"CAPACITY\",\"OB\",\"CURRENT_MONTH\",\"REPLACED_DURING_THIS_MONTH\",\"TO_BE_REPLACE\",\"CR_COMPLETED\",";
                    strQry += "  (\"TO_BE_REPLACE\"+ COALESCE(\"TO_BE_REPLACE2\",0))\"TO_BE_REPLACE_ALL\",(\"REPLACED_DURING_THIS_MONTH\"+ COALESCE(\"REPLACED_DURING_MONTH2\",0))\"REPLACED_TILL_MONTH\",'1' as \"ReportType\" FROM (SELECT \"ISUBDIV\" AS \"SD_SUBDIV_CODE\",\"WO_PENDING\",\"INDENT_PENDING\",\"INVOICE_PENDING\",\"CR_RI_PENDING\",\"CAPACITY\",\"EST_PEND\",\"OB\",\"REPLACED_CURR_MONTH\" AS \"REPLACED_DURING_THIS_MONTH\",\"TO_BE_REPLACE\",\"CR_COMPLETE\" AS \"CR_COMPLETED\",COALESCE(\"TOTAL_FAIL_PENDING\",0)\"CURRENT_MONTH\" FROM (SELECT \"ISUBDIV\",\"WO_PENDING\",\"INDENT_PENDING\",\"INVOICE_PENDING\",\"CR_RI_PENDING\",\"CAPACITY\",\"EST_PEND\",\"OB\",\"REPLACED_CURR_MONTH\",\"TO_BE_REPLACE\",COALESCE(\"CR_COMPLETE\",0)\"CR_COMPLETE\" FROM (SELECT \"ISUBDIV\",\"WO_PENDING\",\"INDENT_PENDING\",\"INVOICE_PENDING\",\"CR_RI_PENDING\",\"CAPACITY\",\"EST_PEND\",\"OB\",";
                    strQry += "    \"REPLACED_CURR_MONTH\",COALESCE(\"TO_BE_REPLACE\",0)\"TO_BE_REPLACE\" FROM (SELECT DISTINCT \"ISUBDIV\",\"WO_PENDING\",\"INDENT_PENDING\",\"INVOICE_PENDING\",\"CR_RI_PENDING\",\"CAPACITY\",\"EST_PEND\",\"OB\",COALESCE(\"REPLACED_CURR_MONTH\",0)\"REPLACED_CURR_MONTH\" FROM(SELECT \"ISUBDIV\",\"WO_PENDING\",\"INDENT_PENDING\",\"INVOICE_PENDING\",\"CR_RI_PENDING\",\"CAPACITY\",\"EST_PEND\",";
                    strQry += "   COALESCE(\"OB\",0)\"OB\" FROM (SELECT \"ISUBDIV\",\"WO_PENDING\",\"INDENT_PENDING\",\"INVOICE_PENDING\",\"CR_RI_PENDING\",\"CAPACITY\",COALESCE(\"EST_PEND\",0)\"EST_PEND\" FROM(SELECT \"SUBDIV_CODE\" AS \"ISUBDIV\",COALESCE(\"WO_PENDING\",0)\"WO_PENDING\",COALESCE(\"INDENT_PENDING\",0)\"INDENT_PENDING\",COALESCE(\"INVOICE_PENDING\",0)\"INVOICE_PENDING\",COALESCE(\"CR_RI_PENDING\",0)\"CR_RI_PENDING\",";
                    strQry += "   \"CAPACITY\" FROM (SELECT SUBSTR(CAST(\"TRANS_REF_OFF_CODE\" AS TEXT),1,4) \"ISUBDIV\",\"TC_CAPACITY\", SUM(CASE WHEN \"TRANS_BO_ID\" in (11,74)  AND to_char(CAST(C.\"DF_DATE\" AS DATE),'YYYY-MM')<'" + objReport.sMonth + "' THEN 1 ELSE 0 END)\"WO_PENDING\", SUM(CASE WHEN \"TRANS_BO_ID\"in(12,29) AND to_char(CAST(C.\"DF_DATE\" AS DATE),'YYYY-MM')<'" + objReport.sMonth + "'THEN 1 ELSE 0 END)\"INDENT_PENDING\", SUM(CASE WHEN to_char(CAST(C.\"DF_DATE\" AS DATE),'YYYY-MM')<'" + objReport.sMonth + "'";
                    strQry += "   AND \"TRANS_BO_ID\"=13 THEN 1 ELSE 0 END)\"INVOICE_PENDING\",sum(CASE WHEN \"TRANS_BO_ID\"=14 AND to_char(CAST(C.\"DF_DATE\" AS DATE),'YYYY-MM')<'" + objReport.sMonth + "' THEN 1 ELSE 0 END)\"Decommission_Pending\",SUM(CASE WHEN \"TRANS_BO_ID\"=26 AND to_char(CAST(C.\"DF_DATE\" AS DATE),'YYYY-MM')< '" + objReport.sMonth + "' THEN 1 ELSE 0 END)\"CR_RI_PENDING\" FROM \"TBLPENDINGTRANSACTION\",\"VIEWPENDINGFAILURE\" C,\"TBLDTCFAILURE\" A,\"TBLTCMASTER\" B";
                    strQry += "    WHERE \"TRANS_DTC_CODE\"=A.\"DF_DTC_CODE\" AND A.\"DF_EQUIPMENT_ID\"=B.\"TC_CODE\" AND \"TRANS_DTC_CODE\"=\"DT_CODE\" AND \"TRANS_BO_ID\" <>10 AND CAST(\"TRANS_REF_OFF_CODE\" AS TEXT) LIKE '%' GROUP BY SUBSTR(CAST(\"TRANS_REF_OFF_CODE\" AS TEXT),1,4),\"TC_CAPACITY\" ORDER BY SUBSTR(CAST(\"TRANS_REF_OFF_CODE\" AS TEXT),1,4))A RIGHT JOIN (SELECT \"SD_SUBDIV_CODE\" AS \"SUBDIV_CODE\",\"MD_NAME\" AS \"CAPACITY\" FROM \"TBLSUBDIVMAST\",\"TBLMASTERDATA\" WHERE \"MD_TYPE\"='C'  AND CAST(\"MD_NAME\" AS INTEGER) <=500 ORDER BY CAST(\"MD_NAME\" AS INTEGER))B ON \"ISUBDIV\"=CAST(\"SUBDIV_CODE\" AS TEXT) AND \"CAPACITY\"=CAST(\"TC_CAPACITY\" AS TEXT) ";
                    strQry += "   ORDER BY \"ISUBDIV\",\"CAPACITY\")A LEFT JOIN (SELECT SUBSTR(CAST(\"WO_OFFICE_CODE\" AS TEXT) ,1, 4) \"SUBDIV\", COUNT(*)\"EST_PEND\" FROM \"TBLWORKFLOWOBJECTS\" WHERE CAST(\"WO_REF_OFFCODE\" AS TEXT) LIKE '%'  AND \"WO_BO_ID\" = '9' AND TO_CHAR(\"WO_CR_ON\", 'YYYY-MM')< '" + objReport.sMonth + "' AND \"WO_APPROVE_STATUS\" = '0' GROUP BY SUBSTR(CAST(\"WO_OFFICE_CODE\" AS TEXT),1,4) ORDER BY SUBSTR(CAST(\"WO_OFFICE_CODE\" AS TEXT),1,4))B ON CAST(A.\"ISUBDIV\" AS TEXT) = B.\"SUBDIV\" )A  LEFT JOIN";
                    strQry += "   (SELECT COUNT(*) AS \"OB\",SUBSTR(CAST(\"TRANS_REF_OFF_CODE\" AS TEXT), 1, 4) AS \"SUBDIV\",\"TC_CAPACITY\" FROM \"TBLPENDINGTRANSACTION\" LEFT JOIN \"TBLDTCFAILURE\" ON \"TRANS_DTC_CODE\"=\"DF_DTC_CODE\" LEFT JOIN \"TBLDTCMAST\" ON \"TRANS_DTC_CODE\" =\"DT_CODE\" INNER JOIN \"TBLTCMASTER\" ON \"TC_CODE\"=\"DT_TC_ID\" WHERE  \"TRANS_BO_ID\"=14  AND CAST(\"TRANS_REF_OFF_CODE\" AS TEXT)";
                    strQry += "   LIKE '%' AND \"TRANS_BO_ID\" <>10 AND \"DF_REPLACE_FLAG\"='0' AND \"DT_TC_ID\"<>0 AND TO_CHAR(\"DF_DATE\", 'YYYY-MM')< '" + objReport.sMonth + "' GROUP BY SUBSTR(CAST(\"TRANS_REF_OFF_CODE\" AS TEXT),1,4),\"TC_CAPACITY\")B ON CAST(A.\"ISUBDIV\" AS TEXT)=B.\"SUBDIV\" AND A.\"CAPACITY\"=CAST(B.\"TC_CAPACITY\" AS TEXT))A LEFT JOIN (SELECT count(*)\"REPLACED_CURR_MONTH\",SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1,4)\"SUBDIV\",";
                    strQry += "   \"TC_CAPACITY\" FROM \"TBLDTCFAILURE\",\"TBLWORKORDER\",\"TBLINDENT\",\"TBLDTCINVOICE\",\"TBLTCREPLACE\",\"TBLTCMASTER\" WHERE \"DF_EQUIPMENT_ID\"=\"TC_CODE\" AND \"DF_ID\"=\"WO_DF_ID\" AND \"WO_SLNO\" =\"TI_WO_SLNO\" AND\"TI_ID\"=\"IN_TI_NO\" AND \"IN_NO\" =\"TR_IN_NO\" AND  \"DF_STATUS_FLAG\" IN (1,4) AND \"TR_RI_DATE\" IS NOT NULL AND to_char(CAST(\"DF_DATE\" AS DATE),'YYYY-MM')< '" + objReport.sMonth + "' AND \"DF_REPLACE_FLAG\"='0' ";
                    strQry += "   GROUP BY SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,4),\"TC_CAPACITY\" ORDER BY SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,4))B ON CAST(A.\"ISUBDIV\" AS TEXT)=B.\"SUBDIV\"  AND A.\"CAPACITY\"=CAST(\"TC_CAPACITY\" AS TEXT))A LEFT JOIN (SELECT COUNT(*) AS \"TO_BE_REPLACE\",SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, 4)AS \"SUBDIV\",\"TC_CAPACITY\" FROM \"TBLDTCFAILURE\" LEFT JOIN \"TBLWORKORDER\" ON \"DF_ID\"=\"WO_DF_ID\" LEFT JOIN \"TBLINDENT\"";
                    strQry += "   ON \"WO_SLNO\" =\"TI_WO_SLNO\" LEFT JOIN\"TBLDTCINVOICE\" ON \"TI_ID\"=\"IN_TI_NO\" LEFT JOIN \"TBLTCREPLACE\" ON \"IN_NO\" =\"TR_IN_NO\" ,\"TBLTCMASTER\" WHERE \"DF_EQUIPMENT_ID\" =\"TC_CODE\" AND  \"DF_STATUS_FLAG\" IN (1,4) AND \"TR_RI_DATE\" IS NULL AND to_char(CAST(\"DF_DATE\" AS DATE),'YYYY-MM')< '" + objReport.sMonth + "' AND \"DF_REPLACE_FLAG\" ='0'GROUP BY SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1,4),\"TC_CAPACITY\" ORDER BY \"SUBDIV\")B ";
                    strQry += "  ON CAST(A.\"ISUBDIV\" AS TEXT)=B.\"SUBDIV\"AND A.\"CAPACITY\"=CAST(B.\"TC_CAPACITY\" AS TEXT))A LEFT JOIN (SELECT COUNT(\"DF_DTC_CODE\") AS \"CR_COMPLETE\",SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, 4)\"SUBDIV\",\"TC_CAPACITY\" FROM \"TBLDTCFAILURE\",\"TBLTCMASTER\" WHERE \"TC_CODE\" =\"DF_EQUIPMENT_ID\" AND TO_CHAR(CAST(\"DF_DATE\" AS DATE),'YYYY-MM')< '" + objReport.sMonth + "' AND\"DF_STATUS_FLAG\" IN (1,4) AND \"DF_REPLACE_FLAG\" ='1' ";
                    strQry += "  GROUP BY SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,4),\"TC_CAPACITY\" ORDER BY SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,4),\"TC_CAPACITY\")B ON CAST(A.\"ISUBDIV\" AS TEXT)=B.\"SUBDIV\" AND A.\"CAPACITY\" = CAST(B.\"TC_CAPACITY\" AS TEXT))A LEFT JOIN (SELECT SUM(CASE WHEN  \"DF_STATUS_FLAG\" IN (1,4) THEN 1 ELSE 0 END)\"TOTAL_FAIL_PENDING\",SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1,4)\"SUBDIV\",\"TC_CAPACITY\" FROM \"TBLDTCFAILURE\",\"TBLTCMASTER\"";
                    strQry += "  WHERE \"TC_CODE\"=\"DF_EQUIPMENT_ID\" AND \"DF_STATUS_FLAG\" IN (1,4) AND TO_CHAR(CAST(\"DF_DATE\" AS DATE),'YYYY-MM')< '" + objReport.sMonth + "' GROUP BY SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1,4),\"TC_CAPACITY\" ORDER BY  SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1,4),\"TC_CAPACITY\")B ON CAST(A.\"ISUBDIV\" AS TEXT)=B.\"SUBDIV\" AND A.\"CAPACITY\"=CAST(B.\"TC_CAPACITY\" AS TEXT))A LEFT JOIN (SELECT sum(CASE WHEN \"TR_RI_NO\" IS NULL THEN 1 ELSE 0 END)";
                    strQry += "  \"TO_BE_REPLACE2\",sum(CASE WHEN \"TR_RI_NO\" IS NOT NULL THEN 1 ELSE 0 END) \"REPLACED_DURING_MONTH2\",SUBSTR(CAST(\"OM_CODE\" AS TEXT), 1,4)\"SUBDIV\",\"TC_CAPACITY\" FROM \"VIEWPENDINGFAILURE\",\"TBLDTCFAILURE\",\"TBLTCMASTER\" WHERE \"DT_CODE\" =\"DF_DTC_CODE\" AND \"DF_EQUIPMENT_ID\"=\"TC_CODE\" AND \"DF_STATUS_FLAG\" IN (1,4) AND \"DF_REPLACE_FLAG\"='0'  AND  TO_CHAR(CAST(\"TBLDTCFAILURE\".\"DF_DATE\" AS DATE), 'YYYY-MM')<'" + objReport.sMonth + "' AND ";
                    strQry += " \"DT_CODE\" IN (SELECT \"DT_CODE\" FROM \"TBLPENDINGTRANSACTION\" LEFT JOIN \"TBLDTCFAILURE\" ON \"TRANS_DTC_CODE\"=\"DF_DTC_CODE\" LEFT JOIN \"TBLDTCMAST\" ON \"TRANS_DTC_CODE\" =\"DT_CODE\" INNER JOIN \"TBLTCMASTER\" ON  \"TC_CODE\"=\"DT_TC_ID\" WHERE \"DT_TC_ID\" <>0 AND  \"TRANS_BO_ID\"=13  AND CAST(\"TRANS_REF_OFF_CODE\" AS TEXT) LIKE '%' AND \"TRANS_BO_ID\" <>10 AND \"DF_REPLACE_FLAG\"='0' AND  TO_CHAR(CAST(\"DF_DATE\" AS DATE),'YYYY-MM')< '" + objReport.sMonth + "'  ";
                    strQry += "  LIMIT (SELECT COUNT(*) FROM \"TBLDTCMAST\")) GROUP BY SUBSTR(CAST(\"OM_CODE\" AS TEXT), 1,4),\"TC_CAPACITY\")B ON CAST(A.\"SD_SUBDIV_CODE\" AS TEXT)=B.\"SUBDIV\" AND A.\"CAPACITY\"=CAST(B.\"TC_CAPACITY\" AS TEXT) ORDER BY \"SD_SUBDIV_CODE\"";


                    #endregion

                }
                else
                {
                    #region without adding OB+Current_month
                    //strQry = "SELECT \"SD_SUBDIV_CODE\",(SELECT SUBSTR(CAST(\"OFF_NAME\" AS TEXT),STRPOS(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE ";
                    //strQry += "CAST(\"OFF_CODE\"  AS TEXT)=SUBSTR(CAST(\"SD_SUBDIV_CODE\" AS TEXT),1," + Constants.Circle + "))\"CIRCLE\",(SELECT SUBSTR(CAST(\"OFF_NAME\" AS TEXT),";
                    //strQry += "STRPOS(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST (\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"SD_SUBDIV_CODE\" AS TEXT),1," + Constants.Division + "))";
                    //strQry += "\"DIVISION\",(SELECT SUBSTR(CAST(\"OFF_NAME\" AS TEXT),STRPOS(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE \"OFF_CODE\" =\"SD_SUBDIV_CODE\")";
                    //strQry += "\"SUBDIVISION\",\"CAPACITY\",\"OB\",\"CURRENT_MONTH\",\"REPLACED_DURING_THIS_MONTH\",\"TO_BE_REPLACE\",\"CR_COMPLETED\",'" + objReport.sReportType + "' as \"ReportType\"";
                    //strQry += "FROM (SELECT \"ISUBDIV\" AS \"SD_SUBDIV_CODE\",\"WO_PENDING\",\"INDENT_PENDING\",\"INVOICE_PENDING\",\"CR_RI_PENDING\",\"CAPACITY\",\"EST_PEND\",\"OB\"";
                    //strQry += ",\"REPLACED_CURR_MONTH\" AS \"REPLACED_DURING_THIS_MONTH\",\"TO_BE_REPLACE\",\"CR_COMPLETE\" AS \"CR_COMPLETED\",COALESCE(\"TOTAL_FAIL_PENDING\",0)";
                    //strQry += "\"CURRENT_MONTH\" FROM (SELECT \"ISUBDIV\",\"WO_PENDING\",\"INDENT_PENDING\",\"INVOICE_PENDING\",\"CR_RI_PENDING\",\"CAPACITY\",\"EST_PEND\",\"OB\",";
                    //strQry += "\"REPLACED_CURR_MONTH\",\"TO_BE_REPLACE\",COALESCE(\"CR_COMPLETE\",0)\"CR_COMPLETE\" FROM (SELECT \"ISUBDIV\",\"WO_PENDING\",\"INDENT_PENDING\",\"INVOICE_PENDING\"";
                    //strQry += ",\"CR_RI_PENDING\",\"CAPACITY\",\"EST_PEND\",\"OB\",\"REPLACED_CURR_MONTH\",COALESCE(\"TO_BE_REPLACE\",0)\"TO_BE_REPLACE\" FROM (SELECT DISTINCT \"ISUBDIV\"";
                    //strQry += ",\"WO_PENDING\",\"INDENT_PENDING\",\"INVOICE_PENDING\",\"CR_RI_PENDING\",\"CAPACITY\",\"EST_PEND\",\"OB\",COALESCE(\"REPLACED_CURR_MONTH\",0)\"REPLACED_CURR_MONTH\"";
                    //strQry += "FROM (SELECT \"ISUBDIV\",\"WO_PENDING\",\"INDENT_PENDING\",\"INVOICE_PENDING\",\"CR_RI_PENDING\",\"CAPACITY\",\"EST_PEND\",COALESCE(\"OB\",0)\"OB\" FROM ";
                    //strQry += "(SELECT \"ISUBDIV\",\"WO_PENDING\",\"INDENT_PENDING\",\"INVOICE_PENDING\",\"CR_RI_PENDING\",\"CAPACITY\",COALESCE(\"EST_PEND\",0)\"EST_PEND\" FROM";
                    //strQry += "(SELECT \"SUBDIV_CODE\" AS \"ISUBDIV\",COALESCE(\"WO_PENDING\",0)\"WO_PENDING\",COALESCE(\"INDENT_PENDING\",0)\"INDENT_PENDING\",COALESCE(\"INVOICE_PENDING\",0)";
                    //strQry += "\"INVOICE_PENDING\",COALESCE(\"CR_RI_PENDING\",0)\"CR_RI_PENDING\",\"CAPACITY\" FROM (SELECT SUBSTR(CAST(\"OFFCODE\" AS TEXT),1," + Constants.SubDivision + ") ";
                    //strQry += "\"ISUBDIV\",\"TC_CAPACITY\", SUM(CASE WHEN \"WORKORDER\" IS NULL AND \"FAILURE\" IS NOT NULL AND to_char(CAST(C.\"DF_DATE\" AS DATE),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END)";
                    //strQry += "\"WO_PENDING\", SUM(CASE WHEN \"INDENT\" IS NULL AND \"WORKORDER\" IS NOT NULL AND to_char(CAST(C.\"DF_DATE\" AS DATE),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END)\"INDENT_PENDING\"";
                    //strQry += ", SUM(CASE WHEN to_char(CAST(C.\"DF_DATE\" AS DATE),'YYYY-MM')='" + objReport.sMonth + "' AND \"INVOICE\" IS NULL AND \"INDENT\" IS NOT NULL THEN 1 ELSE 0 END)\"INVOICE_PENDING\"";
                    //strQry += ",sum(CASE WHEN \"DECOMMISION\" IS NULL AND \"INVOICE\" IS NOT NULL AND to_char(CAST(C.\"DF_DATE\" AS DATE),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END)\"Decommission_Pending\"";
                    //strQry += ",SUM(CASE WHEN \"CRREPORT\" IS NULL AND \"DECOMMISION\" IS NOT NULL AND to_char(CAST(C.\"DF_DATE\" AS DATE),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END)\"CR_RI_PENDING\" ";
                    //strQry += "FROM \"WORKFLOWSTATUSDUMMY\",\"VIEWPENDINGFAILURE\" C,\"TBLDTCFAILURE\" A,\"TBLTCMASTER\" B WHERE \"WO_DATA_ID\"=A.\"DF_DTC_CODE\" AND A.\"DF_EQUIPMENT_ID\"=B.\"TC_CODE\"";
                    //strQry += "AND \"WO_DATA_ID\"=\"DT_CODE\" AND \"WO_BO_ID\" <>10 AND CAST(\"OFFCODE\" AS TEXT)LIKE '%' GROUP BY SUBSTR(CAST(\"OFFCODE\" AS TEXT),1," + Constants.SubDivision + "),\"TC_CAPACITY\" ";
                    //strQry += "ORDER BY SUBSTR(CAST(\"OFFCODE\" AS TEXT),1," + Constants.SubDivision + "))A RIGHT JOIN (SELECT \"SD_SUBDIV_CODE\" AS \"SUBDIV_CODE\",\"MD_NAME\" AS \"CAPACITY\" FROM ";
                    //strQry += "\"TBLSUBDIVMAST\",\"TBLMASTERDATA\" WHERE \"MD_TYPE\"='C'  AND CAST(\"MD_NAME\" AS INTEGER) <=500 ORDER BY CAST(\"MD_NAME\" AS INTEGER))B ON \"ISUBDIV\"=CAST(\"SUBDIV_CODE\" AS TEXT)";
                    //strQry += "AND \"CAPACITY\"=CAST(\"TC_CAPACITY\" AS TEXT) ORDER BY \"ISUBDIV\",CAST(\"CAPACITY\" AS INTEGER))A LEFT JOIN (SELECT SUBSTR(CAST(\"WO_OFFICE_CODE\" AS TEXT), 1, " + Constants.SubDivision + ")";
                    //strQry += "\"SUBDIV\", COUNT(*) \"EST_PEND\" FROM \"TBLWORKFLOWOBJECTS\" WHERE CAST(\"WO_REF_OFFCODE\" AS TEXT) LIKE '%'  AND \"WO_BO_ID\" = '9' AND TO_CHAR(\"WO_CR_ON\", 'YYYY-MM')= '" + objReport.sMonth + "' ";
                    //strQry += "AND \"WO_APPROVE_STATUS\" = '0' GROUP BY SUBSTR(CAST(\"WO_OFFICE_CODE\" AS TEXT),1," + Constants.SubDivision + ") ORDER BY SUBSTR(CAST (\"WO_OFFICE_CODE\" AS TEXT),1," + Constants.SubDivision + "))B ";
                    //strQry += "ON CAST(A.\"ISUBDIV\" AS TEXT)= B.\"SUBDIV\"  )A LEFT JOIN (SELECT COUNT(*) AS \"OB\",SUBSTR(cast(\"OFFCODE\" as text), 1, " + Constants.SubDivision + ") AS \"SUBDIV\",";
                    //strQry += "\"TC_CAPACITY\" FROM \"WORKFLOWSTATUSDUMMY\" LEFT JOIN  \"TBLDTCFAILURE\" ON \"WO_DATA_ID\"=\"DF_DTC_CODE\"LEFT JOIN \"TBLDTCMAST\" ON \"WO_DATA_ID\" =\"DT_CODE\"";
                    //strQry += ",\"TBLTCMASTER\" WHERE \"TC_CODE\"=\"DT_TC_ID\" AND \"DECOMMISION\" IS NULL  AND cast(\"OFFCODE\" as text) LIKE '%' AND \"WO_BO_ID\" <>10 AND \"DF_REPLACE_FLAG\"='0'";
                    //strQry += "AND to_char(\"DF_DATE\",'YYYY-MM') <= '" + objReport.sMonth + "' GROUP BY SUBSTR(cast(\"OFFCODE\" as text), 1, " + Constants.SubDivision + "),\"TC_CAPACITY\")B ON CAST(A.\"ISUBDIV\" AS TEXT)=B.\"SUBDIV\"";
                    //strQry += "AND A.\"CAPACITY\"=CAST(B.\"TC_CAPACITY\" AS TEXT))A LEFT JOIN (SELECT count(*)\"REPLACED_CURR_MONTH\", SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.SubDivision + ")\"SUBDIV\",\"TC_CAPACITY\" ";
                    //strQry += "FROM \"TBLDTCFAILURE\",\"TBLWORKORDER\",\"TBLINDENT\",\"TBLDTCINVOICE\",\"TBLTCREPLACE\",\"TBLTCMASTER\" WHERE \"DF_EQUIPMENT_ID\"=\"TC_CODE\" AND";
                    //strQry += "\"DF_ID\"=\"WO_DF_ID\" AND \"WO_SLNO\" =\"TI_WO_SLNO\" AND \"TI_ID\"=\"IN_TI_NO\" AND \"IN_NO\" =\"TR_IN_NO\" AND \"DF_STATUS_FLAG\" IN (1,4) AND \"TR_RI_DATE\" ";
                    //strQry += "IS NOT NULL AND to_char(CAST(\"DF_DATE\" AS DATE ),'YYYY-MM')='" + objReport.sMonth + "' AND \"DF_REPLACE_FLAG\"='0'  GROUP BY SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ")";
                    //strQry += ",\"TC_CAPACITY\" ORDER BY SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + "))B ON CAST(A.\"ISUBDIV\" AS TEXT)=B.\"SUBDIV\" AND A.\"CAPACITY\"=CAST(\"TC_CAPACITY\" AS TEXT))A";
                    //strQry += " LEFT JOIN (SELECT COUNT(*) AS \"TO_BE_REPLACE\",SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.SubDivision + ") AS \"SUBDIV\",\"TC_CAPACITY\" FROM \"TBLDTCFAILURE\" LEFT JOIN \"TBLWORKORDER\" ON \"DF_ID\"=\"WO_DF_ID\" LEFT JOIN  ";
                    //strQry += "\"TBLINDENT\" ON \"WO_SLNO\" =\"TI_WO_SLNO\" LEFT JOIN \"TBLDTCINVOICE\" ON \"TI_ID\"=\"IN_TI_NO\" LEFT JOIN \"TBLTCREPLACE\" ON \"IN_NO\" =\"TR_IN_NO\",\"TBLTCMASTER\" ";
                    //strQry += "WHERE \"DF_EQUIPMENT_ID\" =\"TC_CODE\" AND \"DF_STATUS_FLAG\" IN (1,4) AND \"TR_RI_DATE\" IS NULL AND to_char(\"DF_DATE\" ,'YYYY-MM')='" + objReport.sMonth + "' AND ";
                    //strQry += "\"DF_REPLACE_FLAG\" ='0' GROUP BY SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.SubDivision + "),\"TC_CAPACITY\" ORDER BY \"SUBDIV\")B ON";
                    //strQry += " CAST(A.\"ISUBDIV\" AS TEXT)=B.\"SUBDIV\" AND A.\"CAPACITY\"=CAST(B.\"TC_CAPACITY\" AS TEXT))A LEFT JOIN (SELECT COUNT(\"DF_DTC_CODE\") AS \"CR_COMPLETE\",";
                    //strQry += "SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.SubDivision + ")\"SUBDIV\",\"TC_CAPACITY\" FROM \"TBLDTCFAILURE\",\"TBLTCMASTER\" WHERE ";
                    //strQry += "\"TC_CODE\"=\"DF_EQUIPMENT_ID\" AND to_char(CAST(\"DF_DATE\"AS DATE),'YYYY-MM')= '" + objReport.sMonth + "' AND \"DF_STATUS_FLAG\" IN (1,4) AND \"DF_REPLACE_FLAG\" ='1'";
                    //strQry += "GROUP BY SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + "),\"TC_CAPACITY\" ORDER BY SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ")";
                    //strQry += ",\"TC_CAPACITY\")B ON CAST(A.\"ISUBDIV\" AS TEXT)=B.\"SUBDIV\" AND A.\"CAPACITY\" = CAST(B.\"TC_CAPACITY\" AS TEXT))A LEFT JOIN ";
                    //strQry += "(SELECT SUM(CASE WHEN  \"DF_STATUS_FLAG\" IN (1,4) THEN 1 ELSE 0 END)\"TOTAL_FAIL_PENDING\",SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.SubDivision + ")\"SUBDIV\"";
                    //strQry += ",\"TC_CAPACITY\" FROM \"TBLDTCFAILURE\",\"TBLTCMASTER\" WHERE \"TC_CODE\"=\"DF_EQUIPMENT_ID\" AND \"DF_STATUS_FLAG\" IN (1,4) AND ";
                    //strQry += "to_char(\"DF_DATE\",'YYYY-MM')='" + objReport.sMonth + "' GROUP BY SUBSTR(CAST(\"DF_LOC_CODE\"AS TEXT), 1, " + Constants.SubDivision + "),\"TC_CAPACITY\" ORDER BY ";
                    //strQry += "SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.SubDivision + "),\"TC_CAPACITY\")B ON CAST(A.\"ISUBDIV\" AS TEXT)=B.\"SUBDIV\" AND A.\"CAPACITY\"=CAST(B.\"TC_CAPACITY\" AS TEXT)) A";

                    strQry = "SELECT \"SD_SUBDIV_CODE\",(SELECT SUBSTR(CAST(\"OFF_NAME\" AS TEXT),STRPOS(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\"  AS TEXT)=SUBSTR(CAST(\"SD_SUBDIV_CODE\" AS TEXT),1,2))\"CIRCLE\",";
                    strQry += " (SELECT SUBSTR(CAST(\"OFF_NAME\" AS TEXT),STRPOS(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST (\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"SD_SUBDIV_CODE\" AS TEXT),1,3))\"DIVISION\",(SELECT SUBSTR(CAST(\"OFF_NAME\" AS TEXT),STRPOS(\"OFF_NAME\",':')+1) ";
                    strQry += "   FROM \"VIEW_ALL_OFFICES\" WHERE \"OFF_CODE\" =\"SD_SUBDIV_CODE\")\"SUBDIVISION\",\"CAPACITY\",\"OB\",\"CURRENT_MONTH\",\"REPLACED_DURING_THIS_MONTH\",\"TO_BE_REPLACE\",\"CR_COMPLETED\",'2' as \"ReportType\"FROM (SELECT \"ISUBDIV\" AS \"SD_SUBDIV_CODE\",\"WO_PENDING\",";
                    strQry += "  \"INDENT_PENDING\",\"INVOICE_PENDING\",\"CR_RI_PENDING\",\"CAPACITY\",\"EST_PEND\",\"OB\",\"REPLACED_CURR_MONTH\" AS \"REPLACED_DURING_THIS_MONTH\",\"TO_BE_REPLACE\",\"CR_COMPLETE\" AS \"CR_COMPLETED\",COALESCE(\"TOTAL_FAIL_PENDING\",0)\"CURRENT_MONTH\" FROM (SELECT \"ISUBDIV\",\"WO_PENDING\",";
                    strQry += " \"INDENT_PENDING\",\"INVOICE_PENDING\",\"CR_RI_PENDING\",\"CAPACITY\",\"EST_PEND\",\"OB\",\"REPLACED_CURR_MONTH\",\"TO_BE_REPLACE\",COALESCE(\"CR_COMPLETE\",0)\"CR_COMPLETE\" FROM (SELECT \"ISUBDIV\",\"WO_PENDING\",\"INDENT_PENDING\",\"INVOICE_PENDING\",\"CR_RI_PENDING\",\"CAPACITY\",\"EST_PEND\",\"OB\",";
                    strQry += "  \"REPLACED_CURR_MONTH\",COALESCE(\"TO_BE_REPLACE\",0)\"TO_BE_REPLACE\" FROM (SELECT DISTINCT \"ISUBDIV\"";
                    strQry += "  ,\"WO_PENDING\",\"INDENT_PENDING\",\"INVOICE_PENDING\",\"CR_RI_PENDING\",\"CAPACITY\",\"EST_PEND\",\"OB\",COALESCE(\"REPLACED_CURR_MONTH\",0)\"REPLACED_CURR_MONTH\"FROM (SELECT \"ISUBDIV\",\"WO_PENDING\",\"INDENT_PENDING\",\"INVOICE_PENDING\",\"CR_RI_PENDING\",\"CAPACITY\",\"EST_PEND\",";
                    strQry += "  COALESCE(\"OB\",0)\"OB\" FROM (SELECT \"ISUBDIV\",\"WO_PENDING\",\"INDENT_PENDING\",\"INVOICE_PENDING\",\"CR_RI_PENDING\",\"CAPACITY\",COALESCE(\"EST_PEND\",0)\"EST_PEND\" FROM(SELECT \"SUBDIV_CODE\" AS \"ISUBDIV\",COALESCE(\"WO_PENDING\",0)\"WO_PENDING\",COALESCE(\"INDENT_PENDING\",0)\"INDENT_PENDING\",";
                    strQry += "  COALESCE(\"INVOICE_PENDING\",0)\"INVOICE_PENDING\",COALESCE(\"CR_RI_PENDING\",0)\"CR_RI_PENDING\",\"CAPACITY\" FROM (SELECT SUBSTR(CAST(\"TRANS_REF_OFF_CODE\" AS TEXT),1,4) \"ISUBDIV\",\"TC_CAPACITY\", SUM(CASE WHEN \"TRANS_BO_ID\" in(11,73) AND to_char(CAST(C.\"DF_DATE\" AS DATE),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END)\"WO_PENDING\", ";
                    strQry += "  SUM(CASE WHEN \"TRANS_BO_ID\"in(12,29) AND to_char(CAST(C.\"DF_DATE\" AS DATE),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END)\"INDENT_PENDING\", SUM(CASE WHEN to_char(CAST(C.\"DF_DATE\" AS DATE),'YYYY-MM')='" + objReport.sMonth + "' AND \"TRANS_BO_ID\"=13 THEN 1 ELSE 0 END)\"INVOICE_PENDING\",sum(CASE WHEN \"TRANS_BO_ID\"=14 AND ";
                    strQry += " to_char(CAST(C.\"DF_DATE\" AS DATE),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END)\"Decommission_Pending\",SUM(CASE WHEN \"TRANS_BO_ID\"=26 AND to_char(CAST(C.\"DF_DATE\" AS DATE),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END)\"CR_RI_PENDING\" FROM \"TBLPENDINGTRANSACTION\",\"VIEWPENDINGFAILURE\" C,\"TBLDTCFAILURE\" A,";
                    strQry += "  \"TBLTCMASTER\" B WHERE \"TRANS_DTC_CODE\"=A.\"DF_DTC_CODE\" AND A.\"DF_EQUIPMENT_ID\"=B.\"TC_CODE\"AND \"TRANS_DTC_CODE\"=\"DT_CODE\" AND \"TRANS_BO_ID\" <>10 AND CAST(\"TRANS_REF_OFF_CODE\" AS TEXT)LIKE '%' GROUP BY SUBSTR(CAST(\"TRANS_REF_OFF_CODE\" AS TEXT),1,4),\"TC_CAPACITY\" ORDER BY SUBSTR(CAST(\"TRANS_REF_OFF_CODE\" AS TEXT),1,4)";
                    strQry += "  )A RIGHT JOIN(SELECT \"SD_SUBDIV_CODE\" AS \"SUBDIV_CODE\",\"MD_NAME\" AS \"CAPACITY\" FROM \"TBLSUBDIVMAST\",\"TBLMASTERDATA\" WHERE \"MD_TYPE\"='C'  AND CAST(\"MD_NAME\" AS INTEGER) <=500 ORDER BY CAST(\"MD_NAME\" AS INTEGER))B ON \"ISUBDIV\"=CAST(\"SUBDIV_CODE\" AS TEXT)AND \"CAPACITY\"=CAST(\"TC_CAPACITY\" AS TEXT) ORDER BY \"ISUBDIV\",CAST(\"CAPACITY\" AS INTEGER))A LEFT JOIN (SELECT SUBSTR(CAST(\"WO_OFFICE_CODE\" AS TEXT), 1, 4)";
                    strQry += " \"SUBDIV\", COUNT(*) \"EST_PEND\" FROM \"TBLWORKFLOWOBJECTS\" WHERE CAST(\"WO_REF_OFFCODE\" AS TEXT) LIKE '%'  AND \"WO_BO_ID\" = '9' AND TO_CHAR(\"WO_CR_ON\", 'YYYY-MM')= '" + objReport.sMonth + "' AND \"WO_APPROVE_STATUS\" = '0' GROUP BY SUBSTR(CAST(\"WO_OFFICE_CODE\" AS TEXT),1,4) ORDER BY SUBSTR(CAST (\"WO_OFFICE_CODE\" AS TEXT),1,4))B ON CAST(A.\"ISUBDIV\" AS TEXT)= B.\"SUBDIV\"  )A";
                    strQry += " LEFT JOIN (SELECT COUNT(*) AS \"OB\",SUBSTR(cast(\"TRANS_REF_OFF_CODE\" as text), 1, 4) AS \"SUBDIV\",";
                    strQry += "  \"TC_CAPACITY\" FROM \"TBLPENDINGTRANSACTION\" LEFT JOIN  \"TBLDTCFAILURE\" ON \"TRANS_DTC_CODE\"=\"DF_DTC_CODE\"LEFT JOIN \"TBLDTCMAST\" ON \"TRANS_DTC_CODE\" =\"DT_CODE\",\"TBLTCMASTER\" WHERE \"TC_CODE\"=\"DT_TC_ID\" AND \"TRANS_BO_ID\"=14  AND cast(\"TRANS_REF_OFF_CODE\" as text) LIKE '%' AND \"TRANS_BO_ID\" <>10 AND \"DF_REPLACE_FLAG\"='0'AND to_char(\"DF_DATE\",'YYYY-MM') <= '" + objReport.sMonth + "'";
                    strQry += "  GROUP BY SUBSTR(cast(\"TRANS_REF_OFF_CODE\" as text), 1, 4),\"TC_CAPACITY\")B ON CAST(A.\"ISUBDIV\" AS TEXT)=B.\"SUBDIV\"AND A.\"CAPACITY\"=CAST(B.\"TC_CAPACITY\" AS TEXT))A LEFT JOIN (SELECT count(*)\"REPLACED_CURR_MONTH\", SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, 4)\"SUBDIV\",\"TC_CAPACITY\" FROM \"TBLDTCFAILURE\",\"TBLWORKORDER\",\"TBLINDENT\",";
                    strQry += " \"TBLDTCINVOICE\",\"TBLTCREPLACE\",\"TBLTCMASTER\" WHERE \"DF_EQUIPMENT_ID\"=\"TC_CODE\" AND\"DF_ID\"=\"WO_DF_ID\" AND \"WO_SLNO\" =\"TI_WO_SLNO\" AND \"TI_ID\"=\"IN_TI_NO\" AND \"IN_NO\" =\"TR_IN_NO\" AND \"DF_STATUS_FLAG\" IN (1,4) AND \"TR_RI_DATE\" IS NOT NULL AND to_char(CAST(\"DF_DATE\" AS DATE ),'YYYY-MM')='" + objReport.sMonth + "' AND ";
                    strQry += "  \"DF_REPLACE_FLAG\"='0'  GROUP BY SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,4),\"TC_CAPACITY\" ORDER BY SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,4))B ON CAST(A.\"ISUBDIV\" AS TEXT)=B.\"SUBDIV\" AND A.\"CAPACITY\"=CAST(\"TC_CAPACITY\" AS TEXT))A LEFT JOIN (SELECT COUNT(*) AS \"TO_BE_REPLACE\",SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, 4) AS \"SUBDIV\",\"TC_CAPACITY\" FROM ";
                    strQry += "  \"TBLDTCFAILURE\" LEFT JOIN \"TBLWORKORDER\" ON \"DF_ID\"=\"WO_DF_ID\" LEFT JOIN  \"TBLINDENT\" ON \"WO_SLNO\" =\"TI_WO_SLNO\" LEFT JOIN \"TBLDTCINVOICE\" ON \"TI_ID\"=\"IN_TI_NO\" LEFT JOIN \"TBLTCREPLACE\" ON \"IN_NO\" =\"TR_IN_NO\",\"TBLTCMASTER\" WHERE \"DF_EQUIPMENT_ID\" =\"TC_CODE\" AND \"DF_STATUS_FLAG\" IN (1,4) AND \"TR_RI_DATE\" IS NULL AND to_char(\"DF_DATE\" ,'YYYY-MM')='" + objReport.sMonth + "' ";
                    strQry += " AND \"DF_REPLACE_FLAG\" ='0' GROUP BY SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, 4),\"TC_CAPACITY\" ORDER BY \"SUBDIV\")B ON CAST(A.\"ISUBDIV\" AS TEXT)=B.\"SUBDIV\" AND A.\"CAPACITY\"=CAST(B.\"TC_CAPACITY\" AS TEXT))A LEFT JOIN (SELECT COUNT(\"DF_DTC_CODE\") AS \"CR_COMPLETE\",SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, 4)\"SUBDIV\",\"TC_CAPACITY\" FROM \"TBLDTCFAILURE\",\"TBLTCMASTER\" WHERE";
                    strQry += "  \"TC_CODE\"=\"DF_EQUIPMENT_ID\" AND to_char(CAST(\"DF_DATE\"AS DATE),'YYYY-MM')= '" + objReport.sMonth + "' AND \"DF_STATUS_FLAG\" IN (1,4) AND \"DF_REPLACE_FLAG\" ='1' GROUP BY SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,4),\"TC_CAPACITY\" ORDER BY SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,4),\"TC_CAPACITY\")B ON CAST(A.\"ISUBDIV\" AS TEXT)=B.\"SUBDIV\" AND A.\"CAPACITY\" = CAST(B.\"TC_CAPACITY\" AS TEXT))A ";
                    strQry += " LEFT JOIN (SELECT SUM(CASE WHEN  \"DF_STATUS_FLAG\" IN (1,4) THEN 1 ELSE 0 END)\"TOTAL_FAIL_PENDING\",SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, 4)\"SUBDIV\",\"TC_CAPACITY\" FROM \"TBLDTCFAILURE\",\"TBLTCMASTER\" WHERE \"TC_CODE\"=\"DF_EQUIPMENT_ID\" AND \"DF_STATUS_FLAG\" IN (1,4) AND to_char(\"DF_DATE\",'YYYY-MM')='" + objReport.sMonth + "' GROUP BY SUBSTR(CAST(\"DF_LOC_CODE\"AS TEXT), 1, 4),\"TC_CAPACITY\"";
                    strQry += "  ORDER BY SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, 4),\"TC_CAPACITY\")B ON CAST(A.\"ISUBDIV\" AS TEXT)=B.\"SUBDIV\" AND A.\"CAPACITY\"=CAST(B.\"TC_CAPACITY\" AS TEXT)) A";



                    #endregion without adding OB+Current_month
                }

                dtFailureMonthWiseAbstract = ObjCon.FetchDataTable(strQry);
                return dtFailureMonthWiseAbstract;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtFailureMonthWiseAbstract;
            }

        }

        #region EstComPrevSO
        public DataTable PrintEstimatedReportSO(string sDtrCode, string sWFObject, string sNewCap)
        {

            DataTable dtDetailedReport = new DataTable();
            string strQry = string.Empty;
            sEmployeeCost = ConfigurationSettings.AppSettings["EmployeeCost"];
            sESI = ConfigurationSettings.AppSettings["ESI"];
            ServiceTax = ConfigurationSettings.AppSettings["ServiceTax"];
            DecomLabourCost = ConfigurationSettings.AppSettings["DecomLabourCost"];

            try
            {

                if (sNewCap == "")
                {
                    //strQry = "select DISTINCT \"WO_DATA_ID\" as \"DF_DTC_CODE\",('" + sDtrCode + "') as \"DF_EQUIPMENT_ID\",TO_CHAR(\"WO_CR_ON\",'dd/MM/yyyy')\"DF_DATE\",\"WO_OFFICE_CODE\",'' AS \"EST_NO\",";
                    //strQry += " (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where \"OM_CODE\"=SUBSTR(WO_OFFICE_CODE,1,4)) as \"LOCATION\",";
                    //strQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE \"SD_SUBDIV_CODE\"=SUBSTR(\"WO_OFFICE_CODE\",1,3)) as \"SUBDIVISION\", ";
                    //strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE \"DIV_CODE\"=SUBSTR(\"WO_OFFICE_CODE\",1,2)) as \"DIVISION\",";
                    //strQry += " (SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=\"WO_DATA_ID\") \"DT_NAME\", 'No' AS \"UNIT\",'1' as \"QUANTITY\",";
                    //strQry += " (select CAST(\"TC_CAPACITY\" AS TEXT) from \"TBLTCMASTER\" where \"TC_CODE\"='" + sDtrCode + "') AS \"CAPACITY\", TE_RATE as \"PRICE\",1* \"TE_RATE\" AS \"TOTALAMOUNT\",";
                    //strQry += " \"TE_COMMLABOUR\" as \"LABOURCHARGE\",(\"TE_COMMLABOUR\" * '" + sEmployeeCost + "')/100 as \"EMPLOYEECOST\",(\"TE_COMMLABOUR\" * '" + sESI + "')/100 as \"ESI\",(\"TE_COMMLABOUR\" * '" + ServiceTax + "')/100 as \"SERVICETAX\", (\"TE_RATE\" * 2)/100 as \"CONTINGENCYCOST\",";
                    //strQry += " (\"TE_RATE\"+\"TE_COMMLABOUR\"+(\"TE_RATE\" * 2)/100+(\"TE_COMMLABOUR\" * 10)/100) as \"FINALTOTAL\",('" + sNewCap + "') as \"DF_ENHANCE_CAPACITY\",";
                    //strQry += "(SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=\"WO_OFFICE_CODE\" AND \"US_ROLE_ID\"='4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' AND LIMIT 1) \"SO_USERNAME\"";
                    //strQry += " FROM \"TBLDTCFAILURE\",\"TBLITEMMASTER\",\"TBLWORKFLOWOBJECTS\",\"TBLTCMASTER\" WHERE ";
                    //strQry += " \"TC_CODE\" ='" + sDtrCode + "' AND \"TC_CODE\"=\"DF_EQUIPMENT_ID\" (+) AND \"TC_CAPACITY\"=\"TE_CAPACITY\" AND COALESCE(\"TC_RATING\",0)=NVL(\"TE_STAR_RATE\",0) AND \"WO_ID\"='" + sWFObject + "'";

                    strQry = "SELECT DISTINCT \"WO_DATA_ID\" AS \"DF_DTC_CODE\",('" + sDtrCode + "') AS \"DF_EQUIPMENT_ID\",TO_CHAR(\"WO_CR_ON\",'DD/MM/YYYY')\"DF_DATE\", ";
                    strQry += " \"WO_OFFICE_CODE\",NULL AS \"EST_NO\", (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE CAST(\"OM_CODE\" AS TEXT)= ";
                    strQry += " SUBSTR(CAST(\"WO_OFFICE_CODE\" AS TEXT),1," + Constants.Section + ")) AS \"LOCATION\", (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE ";
                    strQry += " CAST(\"SD_SUBDIV_CODE\" AS TEXT)=SUBSTR(CAST(\"WO_OFFICE_CODE\" AS TEXT),1," + Constants.SubDivision + ")) AS \"SUBDIVISION\",  (SELECT \"DIV_NAME\" ";
                    strQry += " FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT)=SUBSTR(CAST(\"WO_OFFICE_CODE\" AS TEXT),1," + Constants.Division + ")) AS \"DIVISION\", ";
                    strQry += " \"DT_NAME\", 'NO' AS \"UNIT\",'1' AS \"QUANTITY\", (SELECT CAST(\"TC_CAPACITY\" AS TEXT) FROM \"TBLTCMASTER\" WHERE ";
                    strQry += " \"TC_CODE\"='" + sDtrCode + "') AS \"CAPACITY\", \"TE_RATE\" AS \"PRICE\",1* \"TE_RATE\" AS \"TOTALAMOUNT\", \"TE_COMMLABOUR\" AS ";
                    strQry += " \"LABOURCHARGE\",(\"TE_COMMLABOUR\" * '" + sEmployeeCost + "')/100 AS \"EMPLOYEECOST\",(\"TE_COMMLABOUR\" * '" + sESI + "')/100 AS \"ESI\", ";
                    strQry += " (\"TE_COMMLABOUR\" * '" + ServiceTax + "')/100 AS \"SERVICETAX\", (\"TE_RATE\" * 2)/100 AS \"CONTINGENCYCOST\", ";
                    strQry += " (\"TE_RATE\"+\"TE_COMMLABOUR\"+(\"TE_RATE\" * 2)/100+(\"TE_COMMLABOUR\" * 10)/100) AS \"FINALTOTAL\", ";
                    strQry += " ('" + sNewCap + "') AS \"DF_ENHANCE_CAPACITY\",(SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"= ";
                    strQry += " CAST(\"WO_OFFICE_CODE\" AS TEXT) AND \"US_ROLE_ID\"='4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) ";
                    strQry += " \"SO_USERNAME\" FROM \"TBLTCMASTER\" INNER JOIN \"TBLITEMMASTER\" ON \"TC_CAPACITY\"=\"TE_CAPACITY\" AND ";
                    strQry += " COALESCE(\"TC_RATING\",0)=COALESCE(\"TE_STAR_RATE\",0)  LEFT JOIN \"TBLDTCFAILURE\" ON \"TC_CODE\"=\"DF_EQUIPMENT_ID\" ";
                    strQry += " INNER JOIN \"TBLDTCMAST\" ON \"TC_CODE\" = \"DT_TC_ID\" INNER JOIN \"TBLWORKFLOWOBJECTS\" ON \"DT_CODE\"=\"WO_DATA_ID\" ";
                    strQry += " WHERE \"WO_ID\"='" + sWFObject + "' AND \"TC_CODE\" ='" + sDtrCode + "' ";
                }
                else
                {
                    strQry = "SELECT * FROM (SELECT DISTINCT \"WO_DATA_ID\" as \"DF_DTC_CODE\",'" + sDtrCode + "' as \"DF_EQUIPMENT_ID\",TO_CHAR(\"WO_CR_ON\",'dd/MM/yyyy') AS \"DF_DATE\"";
                    strQry += ",\"WO_OFFICE_CODE\",'' AS \"EST_NO\",(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where \"OM_CODE\"=SUBSTR(\"WO_OFFICE_CODE\",1,4)) as \"LOCATION\", ";
                    strQry += "(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE \"SD_SUBDIV_CODE\"=SUBSTR(\"WO_OFFICE_CODE\",1,3)) as \"SUBDIVISION\",  ";
                    strQry += "(SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE \"DIV_CODE\"=SUBSTR(\"WO_OFFICE_CODE\",1,2)) as \"DIVISION\",(select CAST(\"TC_CAPACITY\" AS TEXT) from \"TBLTCMASTER\" where \"TC_CODE\"='" + sDtrCode + "') AS \"CAPACITY\",";
                    strQry += "(SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=\"WO_DATA_ID\") \"DT_NAME\", 'No' AS \"UNIT\",'1' as \"QUANTITY\",'" + sNewCap + "' AS ";
                    strQry += " \"DF_ENHANCE_CAPACITY\",(SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=\"WO_OFFICE_CODE\" AND \"US_ROLE_ID\"='4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' ";
                    strQry += " AND LIMIT 1) \"SO_USERNAME\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\"='" + sWFObject + "')A RIGHT JOIN ";
                    strQry += "(SELECT \"TE_RATE\" as Price,1* \"TE_RATE\" AS TotalAmount, \"TE_COMMLABOUR\" as \"labourcharge\",(\"TE_COMMLABOUR\" * '" + sEmployeeCost + "')/100 as \"EmployeeCost\",";
                    strQry += "(\"TE_COMMLABOUR\" * '" + sESI + "')/100 as ESI,(\"TE_COMMLABOUR\" * '" + ServiceTax + "')/100 as \"ServiceTax\", (\"TE_RATE\"*2)/100 as ContingencyCost, ";
                    strQry += "(\"TE_RATE\"+\"TE_COMMLABOUR\"+(\"TE_RATE\" *  2)/100+(\"TE_COMMLABOUR\" * 10)/100) as FinalTotal,'" + sNewCap + "' as \"DF_ENHANCE_CAPACITY\" FROM ";
                    strQry += " \"TBLITEMMASTER\" WHERE  \"TE_CAPACITY\" ='" + sNewCap + "' AND AND COALESCE(\"TC_RATING\",0)=COALESCE(\"TE_STAR_RATE\",0))B ON A.\"DF_ENHANCE_CAPACITY\"=B.\"DF_ENHANCE_CAPACITY\"";
                }
                //OleDbDataReader dr = ObjCon.Fetch(strQry);
                dtDetailedReport = ObjCon.FetchDataTable(strQry);
                return dtDetailedReport;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtDetailedReport;

            }
        }
        #endregion

        #region EstDecPrevSO
        public DataTable PrintDecomEstimationReportSO(string sDtrCode, string sWFObject, string sRes, string sNewCap)
        {
            DataTable dtDetailedReport = new DataTable();
            string strQry = string.Empty;
            sEmployeeCost = ConfigurationSettings.AppSettings["EmployeeCost"];
            sESI = ConfigurationSettings.AppSettings["ESI"];
            ServiceTax = ConfigurationSettings.AppSettings["ServiceTax"];
            DecomLabourCost = ConfigurationSettings.AppSettings["DecomLabourCost"];

            try
            {

                strQry = " SELECT \"WO_DATA_ID\" as \"DF_DTC_CODE\",\"DT_NAME\",('" + sRes.Replace("ç", ",") + "') AS \"DF_REASON\", \"FD_FEEDER_NAME\", \"TE_COMMLABOUR\",(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\"  WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT)=SUBSTR(CAST(\"WO_OFFICE_CODE\" AS TEXT),1,4)) as SubDivision,(SELECT \"DIV_NAME\" FROM \"TBLDIVISION\"  WHERE CAST(\"DIV_CODE\" AS TEXT)=SUBSTR(CAST(\"WO_OFFICE_CODE\" AS TEXT),1,3)) as Division,";
                strQry += " ('" + sDtrCode + "') AS \"DF_EQUIPMENT_ID\",('" + sNewCap + "') as \"DF_ENHANCE_CAPACITY\",to_char(\"WO_CR_ON\",'dd/MM/yyyy')\"DF_DATE\",\"WO_OFFICE_CODE\",'' AS \"EST_NO\", ";
                strQry += " (SELECT  to_char(\"TM_MAPPING_DATE\",'dd/MON/yyyy') \"TM_MAPPING_DATE\" FROM \"TBLTRANSDTCMAPPING\" WHERE \"TM_TC_ID\"=\"TC_CODE\" AND \"TM_LIVE_FLAG\"='1') \"DTR_COMMISSION_DATE\",to_char(\"DT_TRANS_COMMISION_DATE\",'dd/MON/yyyy')\"DTC_COMMISSION_DATE\", \"TE_RATE\" as Price , ";
                strQry += " \"TE_COMMLABOUR\" \"TE_DECOMLABOUR\",CAST(\"TC_CAPACITY\" AS TEXT)\"TC_CAPACITY\",\"TC_CODE\",\"TC_SLNO\",'OLD' AS Rep, (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TC_MAKE_ID\"=\"TM_ID\")\"TM_NAME\", ";
                strQry += "  \"DT_TOTAL_CON_KW\",(1*\"TE_COMMLABOUR\"*'" + DecomLabourCost + "') \"LABOUR_COST\",  (\"TE_COMMLABOUR\"*'" + sEmployeeCost + "')/100 as EmployeeCost,(\"TE_COMMLABOUR\"*'" + sESI + "')/100 as ESI,(\"TE_COMMLABOUR\"*'" + ServiceTax + "')/100 as ServiceTax,((((\"TE_COMMLABOUR\"*'" + sEmployeeCost + "')/100)+(\"TE_COMMLABOUR\"*'" + DecomLabourCost + "'))/100)*2 as ContingencyCost, ((\"TE_COMMLABOUR\"*'" + DecomLabourCost + "')+((\"TE_COMMLABOUR\"*'" + sEmployeeCost + "')/100)+((\"TE_COMMLABOUR\"*'" + sESI + "')/100)+((\"TE_COMMLABOUR\"*'" + ServiceTax + "')/100)+((((\"TE_COMMLABOUR\"*'" + sEmployeeCost + "')/100)+(\"TE_COMMLABOUR\"*'" + DecomLabourCost + "'))/100)*2) as FinalTotal, ";
                strQry += " 'No' as Unit,'1' as Quantity,(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\"  WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT)=SUBSTR(CAST(\"WO_OFFICE_CODE\" AS TEXT),1,4)) as SubDivision, (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where  CAST(\"OM_CODE\" AS TEXT)=SUBSTR(CAST(\"WO_OFFICE_CODE\" AS TEXT),1,5)) as Location, ";
                strQry += " (SELECT \"RSD_GUARRENTY_TYPE\" FROM \"TBLREPAIRSENTMASTER\",\"TBLREPAIRSENTDETAILS\" WHERE \"RSM_ID\"=\"RSD_RSM_ID\" AND \"RSD_ID\" = (SELECT MAX(\"RSD_ID\") FROM \"TBLREPAIRSENTDETAILS\"  WHERE \"RSD_TC_CODE\"= \"TC_CODE\" )) \"TR_GUARANTY\",(1*\"TE_COMMLABOUR\"*'" + DecomLabourCost + "') \"LABOUR_COST\",  ";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=CAST(\"WO_OFFICE_CODE\" AS TEXT)  AND \"US_ROLE_ID\"='4'  AND \"US_STATUS\"='A') \"SO_USERNAME\"  from  \"TBLWORKFLOWOBJECTS\", ";
                strQry += " \"TBLITEMMASTER\",\"TBLTCMASTER\",\"TBLDTCMAST\",\"TBLFEEDERMAST\" WHERE \"WO_DATA_ID\"=\"DT_CODE\" AND  \"TC_CODE\"='" + sDtrCode + "' AND \"WO_ID\"='" + sWFObject + "' ";
                strQry += "  AND \"TC_CAPACITY\"=\"TE_CAPACITY\" AND COALESCE(\"TC_RATING\",0)=COALESCE(\"TE_STAR_RATE\",0) AND \"FD_FEEDER_CODE\"=SUBSTR(\"WO_DATA_ID\",0," + Constants.Feeder + ") ";

                dtDetailedReport = ObjCon.FetchDataTable(strQry);
                return dtDetailedReport;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtDetailedReport;
            }
        }
        #endregion

        #region EstSurvyPrevSO
        public DataTable PrintSurveyReportSO(DataTable dt, string sWoID, string sTCcode, string sNewCap)
        {
            DataTable dtDetailedReport = new DataTable();
            string strQry = string.Empty;
            try
            {


                string sDfType = Convert.ToString(dt.Rows[0]["DF_FAILURE_TYPE"]).Trim();
                string sDfHtBus = Convert.ToString(dt.Rows[0]["DF_HT_BUSING"]).Trim();
                string sDfLTBus = Convert.ToString(dt.Rows[0]["DF_LT_BUSING"]).Trim();
                string sDFHTRoad = Convert.ToString(dt.Rows[0]["DF_HT_BUSING_ROD"]).Trim();
                string sDFLTBusRoad = Convert.ToString(dt.Rows[0]["DF_LT_BUSING_ROD"]).Trim();
                string sDfBrea = Convert.ToString(dt.Rows[0]["DF_BREATHER"]).Trim();
                string sDFOilL = Convert.ToString(dt.Rows[0]["DF_OIL_LEVEL"]).Trim();
                string DFdrainV = Convert.ToString(dt.Rows[0]["DF_DRAIN_VALVE"]).Trim();
                string sDFOilQty = Convert.ToString(dt.Rows[0]["DF_OIL_QNTY"]).Trim();
                string DFTankCon = Convert.ToString(dt.Rows[0]["DF_TANK_CONDITION"]).Trim();
                string sDFExp = Convert.ToString(dt.Rows[0]["DF_EXPLOSION"]).Trim();
                string sDFKWHRead = Convert.ToString(dt.Rows[0]["DF_KWH_READING"]);

                strQry = " SELECT DISTINCT ('" + sTCcode + "') AS DF_EQUIPMENT_ID,TO_CHAR(WO_CR_ON,'DD/MM/YYYY')DF_DATE,TO_CHAR(TC_CAPACITY)TC_CAPACITY,('" + sNewCap + "') as DF_ENHANCE_CAPACITY, ";
                strQry += " TO_CHAR(TC_MANF_DATE,'DD/MM/YYYY')TC_MANF_DATE,SM_NAME,WO_OFFICE_CODE,TM_NAME,TC_SLNO,(SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST WHERE ";
                strQry += " SD_SUBDIV_CODE=SUBSTR(WO_OFFICE_CODE,1,3)) AS SUBDIVISION,(SELECT OM_NAME FROM TBLOMSECMAST WHERE OM_CODE=SUBSTR(WO_OFFICE_CODE,1,4)) ";
                strQry += " AS LOCATION,(SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE=SUBSTR(WO_OFFICE_CODE,1,2)) AS DIVISION,";
                strQry += " DECODE('" + sDfHtBus + "' , 1 , 'GOOD',  2 , 'BAD' , '')DF_HT_BUSING, ";
                strQry += " DECODE('" + sDfLTBus + "' ,1 , 'GOOD' , 2 , 'BAD' , '')DF_LT_BUSING, ";
                strQry += " DECODE('" + sDFHTRoad + "', 1 , 'GOOD' , 2 , 'BAD' , '')DF_HT_BUSING_ROD, ";
                strQry += " DECODE('" + sDFLTBusRoad + "',1 , 'GOOD' , 2 , 'BAD' , '')DF_LT_BUSING_ROD, ";
                strQry += " DECODE('" + sDfBrea + "',1,'YES',2,'NO','')DF_BREATHER, ";
                strQry += " DECODE('" + sDFOilL + "',1,'YES',2,'NO','')DF_OIL_LEVEL, ";
                strQry += " DECODE('" + DFdrainV + "',1,'YES',2,'NO','')DF_DRAIN_VALVE, ";
                strQry += "'" + sDFOilQty + "' AS DF_OIL_QNTY, ";
                strQry += " DECODE('" + DFTankCon + "' , 1 , 'GOOD' , 2 , 'BAD' , '')DF_TANK_CONDITION, ";
                strQry += " DECODE('" + sDFExp + "',1,'YES',2,'NO','')DF_EXAMPLING,'' AS EST_NO, ";

                strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=WO_OFFICE_CODE AND US_ROLE_ID='4' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A') SO_USERNAME FROM ";
                strQry += " TBLTCMASTER,TBLTRANSMAKES,TBLSTOREMAST,TBLWORKFLOWOBJECTS WHERE TC_CODE='" + sTCcode + "' AND TM_ID=TC_MAKE_ID AND ";
                strQry += "TC_STORE_ID=SM_ID AND wo_id='" + sWoID + "'";

                dtDetailedReport = ObjCon.FetchDataTable(strQry);
                return dtDetailedReport;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtDetailedReport;

            }
        }
        #endregion
        public DataTable PrintWorkOrderReport(string sFailureId)
        {
            DataTable dtWorkOrderDetails = new DataTable();
            string strQry = string.Empty;

            strQry = " SELECT  \"WO_SLNO\",\"WO_NO\",\"WO_DF_ID\",TO_CHAR(\"WO_DATE\",'dd/MM/yyyy')\"WO_DATE\",CAST(\"WO_AMT\" AS TEXT)\"WO_AMT\",\"WO_NO_DECOM\",\"WO_ACC_CODE\",\"WO_ACCCODE_DECOM\",";
            strQry += "  \"WO_OFF_CODE\",\"WO_CRBY\",TO_CHAR(\"WO_CRON\",'dd/MM/yyyy')\"WO_CRON,\"TO_CHAR(\"WO_DATE_DECOM\",'dd/MM/yyyy')\"WO_DATE_DECOM\",CAST(\"WO_AMT_DECOM\" AS TEXT)\"WO_AMT_DECOM\",";
            strQry += " \"WO_NO_OF\",TO_CHAR(\"WO_DATE_OF\",'dd/MM/yyyy')\"WO_DATE_OF\",\"WO_AMT_OF\",\"WO_ACC_OF\",\"WO_ISSUED_BY\",";
            strQry += " \"WO_NO_CREDIT\",\"WO_AMT_CREDIT\",\"WO_ACC_CRERDIT\",\"WO_DATE_CREDIT\",";
            strQry += " TO_CHAR(\"DF_DATE\",'DD/MM/YYYY')\"DF_FAILED_DATE\",\"WO_NEW_CAP\",\"WO_REQUEST_LOC\",\"DF_ENHANCE_CAPACITY\",(SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"= \"WO_OFF_CODE\" AND \"US_ROLE_ID\"=7 AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A')\"AET_USERNAME\",";
            strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"= \"WO_OFF_CODE\" AND \"US_ROLE_ID\"=2 AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A')\"STO_USERNAME\",";
            strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"= \"WO_OFF_CODE\" AND \"US_ROLE_ID\"=6 AND \"US_MMS_ID\" IS NULL AND CAST(\"US_STATUS\" AS TEXT)='A')\"AO_USERNAME\",";
            strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"= \"WO_OFF_CODE\" AND \"US_ROLE_ID\"=3 AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A')\"DO_USERNAME\"";
            strQry += " FROM \"TBLWORKORDER\",\"TBLDTCFAILURE\" WHERE \"DF_ID\"=\"WO_DF_ID\" AND \"DF_ID\"='" + sFailureId + "'";

            dtWorkOrderDetails = ObjCon.FetchDataTable(strQry);
            return dtWorkOrderDetails;
        }

        public DataTable PrintDecomEstimatedReport(string sFailureId, string sStatus, string FailType, string sInsulationType, string starrating, string StatusFalg)
        {
            DataTable dtDetailedReport = new DataTable();
            string strQry = string.Empty;
            sEmployeeCost = ConfigurationSettings.AppSettings["EmployeeCost"];
            sESI = ConfigurationSettings.AppSettings["ESI"];
            ServiceTax = ConfigurationSettings.AppSettings["ServiceTax"];
            DecomLabourCost = ConfigurationSettings.AppSettings["DecomLabourCost"];
            ObjCon.BeginTransaction();

            try
            {

                int tcrating;
                if (starrating != "0")
                {
                    tcrating = Convert.ToInt32(starrating);

                }
                else
                {
                    tcrating = Convert.ToInt32(ObjCon.get_value("SELECT \"TC_RATING\" from \"TBLTCMASTER\",\"TBLDTCFAILURE\"  WHERE  cast(\"DF_EQUIPMENT_ID\" as varchar)=\"TC_CODE\" AND CAST(\"DF_ID\" AS TEXT) ='" + sFailureId + "'"));

                }

                strQry = "SELECT \"DF_EQUIPMENT_ID\" FROM \"TBLDTCFAILURE\" WHERE CAST(\"DF_ID\" AS TEXT)='" + sFailureId + "'";
                string tc_code = ObjCon.get_value(strQry);

                strQry = "SELECT \"RSM_GUARANTY_TYPE\" FROM \"TBLREPAIRSENTMASTER\",\"TBLREPAIRSENTDETAILS\" WHERE \"RSM_ID\"=\"RSD_RSM_ID\" AND \"RSD_ID\" = (SELECT MAX(\"RSD_ID\") FROM \"TBLREPAIRSENTDETAILS\",\"TBLTCMASTER\"  WHERE \"RSD_TC_CODE\"= \"TC_CODE\" AND CAST(\"TC_CODE\" AS TEXT)='" + tc_code + "')";
                string res = ObjCon.get_value(strQry);


                strQry = "SELECT \"DF_ENHANCE_CAPACITY\" FROM \"TBLDTCFAILURE\" WHERE CAST(\"DF_ID\" AS TEXT) ='" + sFailureId + "'";
                string sEnhanceCapacity = ObjCon.get_value(strQry);

                if (res == null || res == "")
                {

                    if (sStatus != "")
                    {
                        if (sStatus.Contains("-"))
                        {
                            if (FailType == "2")
                            {
                                strQry = " SELECT \"DF_ID\",\"DF_DTC_CODE\", \"FD_FEEDER_NAME\",\"DT_NAME\",CAST(\"TC_CODE\" AS TEXT) \"DTR_CODE\",(SELECT \"DIV_NAME\" FROM \"TBLDIVISION\"  WHERE CAST(\"DIV_CODE\" AS TEXT)= SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Division + ")) as \"DIVISION\",";
                                strQry += " CAST(\"DF_EQUIPMENT_ID\" AS TEXT) \"DF_EQUIPMENT_ID\",TO_CHAR(\"DF_DATE\",'dd/MM/yyyy') AS \"DF_DATE\",Replace(\"DF_REASON\",'ç',',') \"DF_REASON\",\"DF_LOC_CODE\",TO_CHAR(\"DF_DTR_COMMISSION_DATE\",'DD/MON/YYYY') AS \"DTR_COMMISSION_DATE\",TO_CHAR(\"DT_TRANS_COMMISION_DATE\",'dd/MON/yyyy') \"DTC_COMMISSION_DATE\",";
                                strQry += " \"TE_RATE\" as Price ,(\"TE_COMMLABOUR\" *'" + DecomLabourCost + "') as \"TE_DECOMLABOUR\",CAST(\"TC_CAPACITY\" AS TEXT) \"TC_CAPACITY\",\"TC_CODE\",\"TC_SLNO\",'OLD' AS \"REP\", \"TE_COMMLABOUR\",";
                                strQry += " (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TC_MAKE_ID\"=\"TM_ID\") \"TM_NAME\",\"DT_TOTAL_CON_KW\",";

                                // ADDED BY RUDRA FOR NEW RATE CHANGES 
                                strQry += "   (((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "'))  *'" + sEmployeeCost + "')/100 as \"EMPLOYEECOST\",";

                                strQry += " (((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "') ) * '" + sESI + "')/100 as \"ESI\",(\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "') as \"SERVICETAX\",((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "')/100)*2 as CONTINGENCYCOST,((((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "'))  *'" + sEmployeeCost + "')/100) + ((((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "') ) * '" + sESI + "')/100 ) + ((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "'))  as \"FINALTOTAL\",";


                                strQry += " 'No' as \"UNIT\",'1' as \"QUANTITY\",(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\"  WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT)= SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ")) as SubDivision,";
                                strQry += " (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where CAST(\"OM_CODE\" AS TEXT) = SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ")) as \"LOCATION\",";
                                strQry += " '' AS \"TR_NAME\",\"DF_GUARANTY_TYPE\" AS \"TR_GUARANTY\" ";
                                strQry += " ,null AS \"EST_NO\",(1 * \"TE_COMMLABOUR\" * '" + DecomLabourCost + "') \"LABOUR_COST\", \"DF_ENHANCE_CAPACITY\", \"TIT_INSULATION_NAME\", \"TT_NAME\", ";
                                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ") AND \"US_ROLE_ID\"='1' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SDO_USERNAME\",";
                                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=CAST(\"DF_LOC_CODE\" AS TEXT) AND \"US_ROLE_ID\"='4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' AND \"US_ID\" =(SELECT \"DF_CRBY\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\" ='" + sFailureId + "' LIMIT 1)) \"SO_USERNAME\" ";
                                strQry += " from  \"TBLDTCFAILURE\",\"TBLITEMMASTER\",\"TBLTCMASTER\",\"TBLDTCMAST\",\"TBLFEEDERMAST\",\"TBLTRANSINSULATIONTYPE\",\"TBLTRANSCORETYPE\" WHERE \"DF_DTC_CODE\"=\"DT_CODE\" AND ";
                                //strQry += " from  \"TBLDTCFAILURE\",\"TBLITEMMASTER\",\"TBLTCMASTER\",\"TBLDTCMAST\",\"TBLFEEDERMAST\",\"TBLTRANSINSULATIONTYPE\",\"TBLTRANSCORETYPE\",\"TBLTRANSREPAIRER\" WHERE \"DF_DTC_CODE\"=\"DT_CODE\" AND  \"DF_REPAIRER_ID\"=\"TR_ID\" and ";
                                strQry += " \"DF_EQUIPMENT_ID\"=\"TC_CODE\" AND \"DF_ID\"='" + sFailureId + "'";

                                if (StatusFalg == "2" || StatusFalg == "4")
                                {
                                    strQry += " AND \"TE_CAPACITY\"=" + sEnhanceCapacity + " AND \"TE_TIT_ID\"=\"TIT_ID\" AND \"TT_ID\"= \"TIT_TT_ID\"   ";
                                }
                                else
                                {
                                    strQry += " AND \"TE_CAPACITY\"=\"TC_CAPACITY\"  AND \"TE_TIT_ID\"=\"TIT_ID\" AND \"TT_ID\"= \"TIT_TT_ID\"   ";
                                }
                                if (sInsulationType == null || sInsulationType == "")
                                {
                                    // strQry += " AND CAST( \"TE_TIT_ID\" AS TEXT)='" + sInsulationType + "'";


                                }
                                else
                                {
                                    strQry += " AND CAST( \"TE_TIT_ID\" AS TEXT)='" + sInsulationType + "'";
                                }
                                //if (tcrating > 3)
                                //{

                                //    strQry += " AND '0'=COALESCE(\"TE_STAR_RATE\",0) ";
                                //}
                                //else
                                //{
                                if (starrating != "0")
                                {
                                    strQry += " AND '" + starrating + "'=COALESCE(\"TE_STAR_RATE\",0)";
                                }
                                else
                                {
                                    strQry += " AND COALESCE(\"TC_RATING\",0)=COALESCE(\"TE_STAR_RATE\",0) ";
                                }
                                // }

                                //AND COALESCE(\"TC_RATING\",0)=COALESCE(\"TE_STAR_RATE\",0)";
                                //strQry += " AND \"FD_FEEDER_CODE\"=SUBSTR(\"DF_DTC_CODE\",1," + Constants.Feeder + ")";
                                strQry += " AND \"FD_FEEDER_CODE\"=\"DT_FDRSLNO\" ";
                                strQry += "GROUP BY  \"DF_ID\",\"DF_DTC_CODE\", \"FD_FEEDER_NAME\",\"DT_NAME\" ,\"DTR_CODE\" ,\"DIVISION\",\"DF_EQUIPMENT_ID\",\"DF_DATE\",\"DF_REASON\",\"DF_LOC_CODE\",\"DF_DTR_COMMISSION_DATE\" ,\"DTC_COMMISSION_DATE\" ,Price ,\"TE_DECOMLABOUR\" ,\"TC_CAPACITY\",\"TC_CODE\",\"TC_SLNO\",\"REP\", \"TE_COMMLABOUR\",\"DT_TOTAL_CON_KW\",\"EMPLOYEECOST\",\"ESI\",\"SERVICETAX\",CONTINGENCYCOST,\"FINALTOTAL\",\"UNIT\",\"QUANTITY\",SubDivision,\"LOCATION\",\"TR_GUARANTY\",\"EST_NO\",\"LABOUR_COST\", \"DF_ENHANCE_CAPACITY\", \"TIT_INSULATION_NAME\", \"TT_NAME\",\"SDO_USERNAME\",\"SO_USERNAME\"";
                            }
                            else
                            {
                                strQry = " SELECT \"DF_ID\",\"DF_DTC_CODE\", \"FD_FEEDER_NAME\",\"DT_NAME\",CAST(\"TC_CODE\" AS TEXT) \"DTR_CODE\",(SELECT \"DIV_NAME\" FROM \"TBLDIVISION\"  WHERE CAST(\"DIV_CODE\" AS TEXT)= SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Division + ")) as \"DIVISION\",";
                                strQry += " CAST(\"DF_EQUIPMENT_ID\" AS TEXT) \"DF_EQUIPMENT_ID\",TO_CHAR(\"DF_DATE\",'dd/MM/yyyy') AS \"DF_DATE\",Replace(\"DF_REASON\",'ç',',') \"DF_REASON\",\"DF_LOC_CODE\",TO_CHAR(\"DF_DTR_COMMISSION_DATE\",'DD/MON/YYYY') AS \"DTR_COMMISSION_DATE\",TO_CHAR(\"DT_TRANS_COMMISION_DATE\",'dd/MON/yyyy') \"DTC_COMMISSION_DATE\",";
                                strQry += " \"TE_RATE\" as Price ,(\"TE_COMMLABOUR\" *'" + DecomLabourCost + "') as \"TE_DECOMLABOUR\",CAST(\"TC_CAPACITY\" AS TEXT) \"TC_CAPACITY\",\"TC_CODE\",\"TC_SLNO\",'OLD' AS \"REP\", \"TE_COMMLABOUR\",";
                                strQry += " (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TC_MAKE_ID\"=\"TM_ID\") \"TM_NAME\",\"DT_TOTAL_CON_KW\",";

                                // ADDED BY RUDRA FOR NEW RATE CHANGES 
                                strQry += "   (((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "'))  *'" + sEmployeeCost + "')/100 as \"EMPLOYEECOST\",";

                                strQry += " (((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "') ) * '" + sESI + "')/100 as \"ESI\",(\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "') as \"SERVICETAX\",((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "')/100)*2 as CONTINGENCYCOST,((((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "'))  *'" + sEmployeeCost + "')/100) + ((((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "') ) * '" + sESI + "')/100 ) + ((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "'))  as \"FINALTOTAL\",";



                                strQry += " 'No' as \"UNIT\",'1' as \"QUANTITY\",(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\"  WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT)= SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ")) as SubDivision,";
                                strQry += " (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where CAST(\"OM_CODE\" AS TEXT) = SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ")) as \"LOCATION\",";
                                strQry += " '' AS \"TR_NAME\",\"DF_GUARANTY_TYPE\" AS \"TR_GUARANTY\" ";
                                strQry += " ,null AS \"EST_NO\",(1 * \"TE_COMMLABOUR\" * '" + DecomLabourCost + "') \"LABOUR_COST\",CAST(\"DF_ENHANCE_CAPACITY\" AS TEXT) \"DF_ENHANCE_CAPACITY\",\"TIT_INSULATION_NAME\", \"TT_NAME\" , ";
                                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ") AND \"US_ROLE_ID\"='1' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SDO_USERNAME\",";
                                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=CAST(\"DF_LOC_CODE\" AS TEXT) AND \"US_ROLE_ID\"='4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' AND \"US_ID\" =(SELECT \"DF_CRBY\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\" ='" + sFailureId + "' LIMIT 1)) \"SO_USERNAME\" ";
                                strQry += " from  \"TBLDTCFAILURE\",\"TBLITEMMASTER\",\"TBLTCMASTER\",\"TBLDTCMAST\",\"TBLFEEDERMAST\" ,\"TBLTRANSINSULATIONTYPE\",\"TBLTRANSCORETYPE\" WHERE \"DF_DTC_CODE\"=\"DT_CODE\" AND ";
                                //strQry += " from  \"TBLDTCFAILURE\",\"TBLITEMMASTER\",\"TBLTCMASTER\",\"TBLDTCMAST\",\"TBLFEEDERMAST\" ,\"TBLTRANSINSULATIONTYPE\",\"TBLTRANSCORETYPE\",\"TBLTRANSREPAIRER\" WHERE \"DF_DTC_CODE\"=\"DT_CODE\"  AND  \"DF_REPAIRER_ID\"=\"TR_ID\" AND ";

                                strQry += " \"DF_EQUIPMENT_ID\"=\"TC_CODE\" AND \"DF_ID\"='" + sFailureId + "' AND \"TC_CAPACITY\"=\"TE_CAPACITY\" AND \"TE_TIT_ID\"=\"TIT_ID\" AND \"TT_ID\"= \"TIT_TT_ID\"  ";
                                if (sInsulationType == null || sInsulationType == "")
                                {
                                    // strQry += " AND CAST( \"TE_TIT_ID\" AS TEXT)='" + sInsulationType + "'";


                                }
                                else
                                {
                                    strQry += " AND CAST( \"TE_TIT_ID\" AS TEXT)='" + sInsulationType + "'";
                                }
                                //if (tcrating > 3)
                                //{

                                //    strQry += " AND '0'=COALESCE(\"TE_STAR_RATE\",0) ";
                                //}
                                //else
                                //{
                                if (starrating != "0")
                                {
                                    strQry += " AND '" + starrating + "'=COALESCE(\"TE_STAR_RATE\",0) ";
                                }
                                else
                                {
                                    strQry += " AND COALESCE(\"TC_RATING\",0)=COALESCE(\"TE_STAR_RATE\",0) ";
                                }
                                //   }

                                // AND COALESCE(\"TC_RATING\",0)=COALESCE(\"TE_STAR_RATE\",0)";
                                //strQry += " AND \"FD_FEEDER_CODE\"=SUBSTR(\"DF_DTC_CODE\",1," + Constants.Feeder + ")";
                                strQry += " AND \"FD_FEEDER_CODE\"=\"DF_DTC_CODE\"";
                                strQry += " GROUP BY  \"DF_ID\",\"DF_DTC_CODE\", \"FD_FEEDER_NAME\",\"DT_NAME\" ,\"DTR_CODE\" ,\"DIVISION\",\"DF_EQUIPMENT_ID\",\"DF_DATE\",\"DF_REASON\",\"DF_LOC_CODE\",\"DF_DTR_COMMISSION_DATE\" ,\"DTC_COMMISSION_DATE\" ,Price ,\"TE_DECOMLABOUR\" ,\"TC_CAPACITY\",\"TC_CODE\",\"TC_SLNO\",\"REP\", \"TE_COMMLABOUR\",\"DT_TOTAL_CON_KW\",\"EMPLOYEECOST\",\"ESI\",\"SERVICETAX\",CONTINGENCYCOST,\"FINALTOTAL\",\"UNIT\",\"QUANTITY\",SubDivision,\"LOCATION\",\"TR_GUARANTY\",\"EST_NO\",\"LABOUR_COST\", \"DF_ENHANCE_CAPACITY\", \"TIT_INSULATION_NAME\", \"TT_NAME\",\"SDO_USERNAME\",\"SO_USERNAME\"";

                            }

                        }
                        else
                        {
                            if (FailType == "2")
                            {
                                strQry = " SELECT \"DF_ID\",\"DF_DTC_CODE\", \"FD_FEEDER_NAME\",\"DT_NAME\",CAST(\"TC_CODE\" AS TEXT) \"DTR_CODE\",(SELECT \"DIV_NAME\" FROM \"TBLDIVISION\"  WHERE CAST(\"DIV_CODE\" AS TEXT)= SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Division + ")) as \"DIVISION\",";
                                strQry += " CAST(\"DF_EQUIPMENT_ID\" AS TEXT) \"DF_EQUIPMENT_ID\",TO_CHAR(\"DF_DATE\",'dd/MM/yyyy') AS \"DF_DATE\",Replace(\"DF_REASON\",'ç',',') \"DF_REASON\",\"DF_LOC_CODE\",TO_CHAR(\"DF_DTR_COMMISSION_DATE\",'DD/MON/YYYY') AS \"DTR_COMMISSION_DATE\",TO_CHAR(\"DT_TRANS_COMMISION_DATE\",'dd/MON/yyyy') \"DTC_COMMISSION_DATE\",";
                                strQry += " \"TE_RATE\" as Price ,(\"TE_COMMLABOUR\" *'" + DecomLabourCost + "') as \"TE_DECOMLABOUR\",CAST(\"TC_CAPACITY\" AS TEXT) \"TC_CAPACITY\",\"TC_CODE\",\"TC_SLNO\",'OLD' AS \"REP\", \"TE_COMMLABOUR\",";
                                strQry += " (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TC_MAKE_ID\"=\"TM_ID\") \"TM_NAME\",\"DT_TOTAL_CON_KW\",";


                                // ADDED BY RUDRA FOR NEW RATE CHANGES 
                                strQry += "   (((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "'))  *'" + sEmployeeCost + "')/100 as \"EMPLOYEECOST\",";

                                strQry += " (((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "') ) * '" + sESI + "')/100 as \"ESI\",(\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "') as \"SERVICETAX\",((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "')/100)*2 as CONTINGENCYCOST,((((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "'))  *'" + sEmployeeCost + "')/100) + ((((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "') ) * '" + sESI + "')/100 ) + ((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "'))  as \"FINALTOTAL\",";



                                strQry += " 'No' as \"UNIT\",'1' as \"QUANTITY\",(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\"  WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT)= SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ")) as SubDivision,";
                                strQry += " (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where CAST(\"OM_CODE\" AS TEXT) = SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ")) as \"LOCATION\",";
                                //strQry += " \"EST_REPAIR\" AS \"TR_NAME\",\"DF_GUARANTY_TYPE\" AS \"TR_GUARANTY\" ";
                                strQry += " '' AS \"TR_NAME\",\"DF_GUARANTY_TYPE\" AS \"TR_GUARANTY\" ";
                                strQry += " ,\"EST_NO\",(1 * \"TE_COMMLABOUR\" * '" + DecomLabourCost + "') \"LABOUR_COST\", \"DF_ENHANCE_CAPACITY\",\"TIT_INSULATION_NAME\", \"TT_NAME\" , ";
                                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ") AND \"US_ROLE_ID\"='1' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SDO_USERNAME\",";
                                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=CAST(\"DF_LOC_CODE\" AS TEXT) AND \"US_ROLE_ID\"='4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' AND \"US_ID\" =(SELECT \"DF_CRBY\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\" ='" + sFailureId + "' LIMIT 1)) \"SO_USERNAME\" ";
                                strQry += " from  \"TBLDTCFAILURE\",\"TBLITEMMASTER\",\"TBLTCMASTER\",\"TBLDTCMAST\",\"TBLESTIMATION\",\"TBLFEEDERMAST\",\"TBLTRANSINSULATIONTYPE\",\"TBLTRANSCORETYPE\" WHERE \"DF_DTC_CODE\"=\"DT_CODE\" AND ";
                                //strQry += " from  \"TBLDTCFAILURE\",\"TBLITEMMASTER\",\"TBLTCMASTER\",\"TBLDTCMAST\",\"TBLESTIMATION\",\"TBLFEEDERMAST\",\"TBLTRANSINSULATIONTYPE\",\"TBLTRANSCORETYPE\",\"TBLTRANSREPAIRER\" WHERE \"DF_DTC_CODE\"=\"DT_CODE\" AND  \"DF_REPAIRER_ID\"=\"TR_ID\" AND ";

                                strQry += " \"DF_EQUIPMENT_ID\"=\"TC_CODE\" AND \"DF_ID\"='" + sFailureId + "' AND \"EST_DF_ID\"=\"DF_ID\" ";

                                if (StatusFalg == "2" || StatusFalg == "4")
                                {
                                    strQry += "AND  \"TE_CAPACITY\"=" + sEnhanceCapacity + " AND \"TE_TIT_ID\"=\"TIT_ID\" AND \"TT_ID\"= \"TIT_TT_ID\" ";
                                }
                                else
                                {
                                    strQry += "AND  \"TE_CAPACITY\"=\"TC_CAPACITY\" AND \"TE_TIT_ID\"=\"TIT_ID\" AND \"TT_ID\"= \"TIT_TT_ID\" ";
                                }
                                if (sInsulationType == null || sInsulationType == "" || sInsulationType == "0")
                                {
                                    // strQry += " AND CAST( \"TE_TIT_ID\" AS TEXT)='" + sInsulationType + "'";


                                }
                                else
                                {
                                    strQry += " AND CAST( \"TE_TIT_ID\" AS TEXT)='" + sInsulationType + "'";
                                }

                                //if (tcrating > 3)
                                //{

                                //    strQry += " AND '0'=COALESCE(\"TE_STAR_RATE\",0) ";
                                //}
                                //else
                                //{
                                if (starrating != "0")
                                {
                                    strQry += " AND '" + starrating + "'=COALESCE(\"TE_STAR_RATE\",0) ";
                                }
                                else
                                {
                                    strQry += " AND COALESCE(\"TC_RATING\",0)=COALESCE(\"TE_STAR_RATE\",0) ";
                                }
                                //    }

                                //AND COALESCE(\"TC_RATING\",0)=COALESCE(\"TE_STAR_RATE\",0)";
                                //strQry += " AND \"FD_FEEDER_CODE\"=SUBSTR(\"DF_DTC_CODE\",1," + Constants.Feeder + ")";
                                strQry += " AND \"FD_FEEDER_CODE\"=\"DT_FDRSLNO\" ";
                                strQry += "GROUP BY  \"DF_ID\",\"DF_DTC_CODE\", \"FD_FEEDER_NAME\",\"DT_NAME\" ,\"DTR_CODE\" ,\"DIVISION\",\"DF_EQUIPMENT_ID\",\"DF_DATE\",\"DF_REASON\",\"DF_LOC_CODE\",\"DF_DTR_COMMISSION_DATE\" ,\"DTC_COMMISSION_DATE\" ,Price ,\"TE_DECOMLABOUR\" ,\"TC_CAPACITY\",\"TC_CODE\",\"TC_SLNO\",\"REP\", \"TE_COMMLABOUR\",\"DT_TOTAL_CON_KW\",\"EMPLOYEECOST\",\"ESI\",\"SERVICETAX\",CONTINGENCYCOST,\"FINALTOTAL\",\"UNIT\",\"QUANTITY\",SubDivision,\"LOCATION\",\"TR_GUARANTY\",\"EST_NO\",\"LABOUR_COST\", \"DF_ENHANCE_CAPACITY\", \"TIT_INSULATION_NAME\", \"TT_NAME\",\"SDO_USERNAME\",\"SO_USERNAME\"";

                            }
                            else
                            {
                                strQry = " SELECT \"DF_ID\",\"DF_DTC_CODE\", \"FD_FEEDER_NAME\",\"DT_NAME\",CAST(\"TC_CODE\" AS TEXT) \"DTR_CODE\",(SELECT \"DIV_NAME\" FROM \"TBLDIVISION\"  WHERE CAST(\"DIV_CODE\" AS TEXT)= SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Division + ")) as \"DIVISION\",";
                                strQry += " CAST(\"DF_EQUIPMENT_ID\" AS TEXT) \"DF_EQUIPMENT_ID\",TO_CHAR(\"DF_DATE\",'dd/MM/yyyy') AS \"DF_DATE\",Replace(\"DF_REASON\",'ç',',') \"DF_REASON\",\"DF_LOC_CODE\",TO_CHAR(\"DF_DTR_COMMISSION_DATE\",'DD/MON/YYYY') AS \"DTR_COMMISSION_DATE\",TO_CHAR(\"DT_TRANS_COMMISION_DATE\",'dd/MON/yyyy') \"DTC_COMMISSION_DATE\",";
                                strQry += " \"TE_RATE\" as Price ,(\"TE_COMMLABOUR\" *'" + DecomLabourCost + "') as \"TE_DECOMLABOUR\",CAST(\"TC_CAPACITY\" AS TEXT) \"TC_CAPACITY\",\"TC_CODE\",\"TC_SLNO\",'OLD' AS \"REP\", \"TE_COMMLABOUR\",";
                                strQry += " (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TC_MAKE_ID\"=\"TM_ID\") \"TM_NAME\",\"DT_TOTAL_CON_KW\",";


                                // ADDED BY RUDRA FOR NEW RATE CHANGES 
                                strQry += "   (((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "'))  *'" + sEmployeeCost + "')/100 as \"EMPLOYEECOST\",";

                                strQry += " (((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "') ) * '" + sESI + "')/100 as \"ESI\",(\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "') as \"SERVICETAX\",((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "')/100)*2 as CONTINGENCYCOST,((((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "'))  *'" + sEmployeeCost + "')/100) + ((((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "') ) * '" + sESI + "')/100 ) + ((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "'))  as \"FINALTOTAL\",";



                                strQry += " 'No' as \"UNIT\",'1' as \"QUANTITY\",(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\"  WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT)= SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ")) as SubDivision,";
                                strQry += " (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where CAST(\"OM_CODE\" AS TEXT) = SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ")) as \"LOCATION\",";
                                strQry += " \"EST_REPAIR\" AS \"TR_NAME\",\"DF_GUARANTY_TYPE\" AS \"TR_GUARANTY\" ";
                                //strQry += " \"TR_NAME\" AS \"TR_NAME\",\"DF_GUARANTY_TYPE\" AS \"TR_GUARANTY\" ";
                                strQry += " ,\"EST_NO\",(1 * \"TE_COMMLABOUR\" * '" + DecomLabourCost + "') \"LABOUR_COST\",CAST(\"DF_ENHANCE_CAPACITY\" AS TEXT) \"DF_ENHANCE_CAPACITY\",\"TIT_INSULATION_NAME\", \"TT_NAME\" , ";
                                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ") AND \"US_ROLE_ID\"='1' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SDO_USERNAME\",";
                                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=CAST(\"DF_LOC_CODE\" AS TEXT) AND \"US_ROLE_ID\"='4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' AND \"US_ID\" =(SELECT \"DF_CRBY\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\" ='" + sFailureId + "' LIMIT 1)) \"SO_USERNAME\" ";
                                strQry += " from  \"TBLDTCFAILURE\",\"TBLITEMMASTER\",\"TBLTCMASTER\",\"TBLDTCMAST\",\"TBLESTIMATION\",\"TBLFEEDERMAST\",\"TBLTRANSINSULATIONTYPE\",\"TBLTRANSCORETYPE\" WHERE \"DF_DTC_CODE\"=\"DT_CODE\" AND ";
                                //strQry += " from  \"TBLDTCFAILURE\",\"TBLITEMMASTER\",\"TBLTCMASTER\",\"TBLDTCMAST\",\"TBLESTIMATION\",\"TBLFEEDERMAST\",\"TBLTRANSINSULATIONTYPE\",\"TBLTRANSCORETYPE\",\"TBLTRANSREPAIRER\" WHERE \"DF_DTC_CODE\"=\"DT_CODE\" AND  \"DF_REPAIRER_ID\"=\"TR_ID\" AND ";
                                strQry += " \"DF_EQUIPMENT_ID\"=\"TC_CODE\" AND \"DF_ID\"='" + sFailureId + "' AND \"EST_DF_ID\"=\"DF_ID\" AND \"TC_CAPACITY\"=\"TE_CAPACITY\" AND \"TE_TIT_ID\"=\"TIT_ID\" AND \"TT_ID\"= \"TIT_TT_ID\" ";

                                if (sInsulationType == null || sInsulationType == "")
                                {
                                    // strQry += " AND CAST( \"TE_TIT_ID\" AS TEXT)='" + sInsulationType + "'";


                                }
                                else
                                {
                                    strQry += " AND CAST( \"TE_TIT_ID\" AS TEXT)='" + sInsulationType + "'";
                                }

                                //if (tcrating > 3)
                                //{

                                //    strQry += " AND '0'=COALESCE(\"TE_STAR_RATE\",0) ";
                                //}
                                //else
                                //{
                                if (starrating != "0")
                                {
                                    strQry += " AND '" + starrating + "'=COALESCE(\"TE_STAR_RATE\",0)";
                                }
                                else
                                {
                                    strQry += " AND COALESCE(\"TC_RATING\",0)=COALESCE(\"TE_STAR_RATE\",0) ";
                                }
                                //  }

                                //AND COALESCE(\"TC_RATING\",0)=COALESCE(\"TE_STAR_RATE\",0)";
                                //strQry += " AND \"FD_FEEDER_CODE\"=SUBSTR(\"DF_DTC_CODE\",1," + Constants.Feeder + ")";
                                strQry += " AND \"FD_FEEDER_CODE\"= \"DT_FDRSLNO\" ";
                                strQry += "GROUP BY  \"DF_ID\",\"DF_DTC_CODE\", \"FD_FEEDER_NAME\",\"DT_NAME\" ,\"DTR_CODE\" ,\"DIVISION\",\"DF_EQUIPMENT_ID\",\"DF_DATE\",\"DF_REASON\",\"DF_LOC_CODE\",\"DF_DTR_COMMISSION_DATE\" ,\"DTC_COMMISSION_DATE\" ,Price ,\"TE_DECOMLABOUR\" ,\"TC_CAPACITY\",\"TC_CODE\",\"TC_SLNO\",\"REP\", \"TE_COMMLABOUR\",\"DT_TOTAL_CON_KW\",\"EMPLOYEECOST\",\"ESI\",\"SERVICETAX\",CONTINGENCYCOST,\"FINALTOTAL\",\"UNIT\",\"QUANTITY\",SubDivision,\"LOCATION\",\"TR_GUARANTY\",\"EST_NO\",\"LABOUR_COST\", \"DF_ENHANCE_CAPACITY\", \"TIT_INSULATION_NAME\", \"TT_NAME\",\"SDO_USERNAME\",\"SO_USERNAME\"";

                            }

                        }
                    }



                }
                else
                {

                    if (sStatus != "")
                    {
                        if (sStatus.Contains("-"))
                        {
                            strQry = " SELECT \"DF_ID\",\"DF_DTC_CODE\", \"FD_FEEDER_NAME\",\"DT_NAME\",CAST(\"TC_CODE\" AS TEXT) \"DTR_CODE\",(SELECT \"DIV_NAME\" FROM \"TBLDIVISION\"  WHERE CAST(\"DIV_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Division + ")) as \"DIVISION\",";
                            strQry += " CAST(\"DF_EQUIPMENT_ID\" AS TEXT) AS \"DF_EQUIPMENT_ID\",TO_CHAR(\"DF_DATE\",'dd/MM/yyyy') AS \"DF_DATE\",Replace(\"DF_REASON\",'ç',',') AS \"DF_REASON\",\"DF_LOC_CODE\",TO_CHAR(\"DF_DTR_COMMISSION_DATE\",'DD/MON/YYYY') AS \"DTR_COMMISSION_DATE\",to_char(\"DT_TRANS_COMMISION_DATE\",'dd/MON/yyyy') AS \"DTC_COMMISSION_DATE\",";
                            strQry += " \"TE_RATE\" as \"PRICE\" ,(\"TE_COMMLABOUR\" * '" + DecomLabourCost + "') AS \"TE_DECOMLABOUR\",CAST(\"TC_CAPACITY\" AS TEXT) \"TC_CAPACITY\",\"TC_CODE\",\"TC_SLNO\",'OLD' AS \"REP\",\"TE_COMMLABOUR\",";
                            strQry += " (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TC_MAKE_ID\"=\"TM_ID\") \"TM_NAME\",\"DT_TOTAL_CON_KW\",";


                            // ADDED BY RUDRA FOR NEW RATE CHANGES 
                            strQry += "   (((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "'))  *'" + sEmployeeCost + "')/100 as \"EMPLOYEECOST\",";

                            strQry += " (((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "') ) * '" + sESI + "')/100 as \"ESI\",(\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "') as \"SERVICETAX\",((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "')/100)*2 as CONTINGENCYCOST,((((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "'))  *'" + sEmployeeCost + "')/100) + ((((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "') ) * '" + sESI + "')/100 ) + ((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "'))  as \"FINALTOTAL\",";



                            strQry += " 'No' as \"UNIT\",'1' as \"QUANTITY\",(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\"  WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ")) as \"SUBDIVISION\",";
                            strQry += " (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where CAST(\"OM_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Section + ")) as \"LOCATION\",";
                            strQry += " \"TR_NAME\" AS \"TR_NAME\",(SELECT \"RSD_GUARRENTY_TYPE\" FROM \"TBLREPAIRSENTMASTER\",\"TBLREPAIRSENTDETAILS\" WHERE \"RSM_ID\"=\"RSD_RSM_ID\" AND \"RSD_ID\" = (SELECT MAX(\"RSD_ID\") FROM \"TBLREPAIRSENTDETAILS\"  WHERE \"RSD_TC_CODE\"= \"TC_CODE\" )) \"TR_GUARANTY\"";
                            strQry += " ,null AS \"EST_NO\" ,(1* \"TE_COMMLABOUR\" *'" + DecomLabourCost + "') \"LABOUR_COST\",CAST(\"DF_ENHANCE_CAPACITY\" AS TEXT) AS  \"DF_ENHANCE_CAPACITY\",\"TIT_INSULATION_NAME\", \"TT_NAME\" , ";
                            strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ") AND \"US_ROLE_ID\"='1' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SDO_USERNAME\",";
                            strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS BIGINT)=\"DF_LOC_CODE\" AND \"US_ROLE_ID\"='4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A'  AND \"US_ID\"=(SELECT \"DF_CRBY\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\"='" + sFailureId + "') LIMIT 1) \"SO_USERNAME\" ";
                            //strQry += " from  \"TBLDTCFAILURE\",\"TBLITEMMASTER\",\"TBLTCMASTER\",\"TBLDTCMAST\",\"TBLFEEDERMAST\",\"TBLTRANSINSULATIONTYPE\",\"TBLTRANSCORETYPE\" WHERE \"DF_DTC_CODE\"=\"DT_CODE\" AND ";
                            strQry += " from  \"TBLDTCFAILURE\",\"TBLITEMMASTER\",\"TBLTCMASTER\",\"TBLDTCMAST\",\"TBLFEEDERMAST\",\"TBLTRANSINSULATIONTYPE\",\"TBLTRANSCORETYPE\",\"TBLTRANSREPAIRER\" WHERE \"DF_DTC_CODE\"=\"DT_CODE\"  AND  \"DF_REPAIRER_ID\"=\"TR_ID\" AND ";

                            strQry += " \"DF_EQUIPMENT_ID\"=\"TC_CODE\" AND \"DF_ID\" ='" + sFailureId + "' AND \"TC_CAPACITY\"=\"TE_CAPACITY\" AND \"TE_TIT_ID\"=\"TIT_ID\" AND \"TT_ID\"= \"TIT_TT_ID\" ";

                            if (sInsulationType == null || sInsulationType == "")
                            {
                                // strQry += " AND CAST( \"TE_TIT_ID\" AS TEXT)='" + sInsulationType + "'";


                            }
                            else
                            {
                                strQry += " AND CAST( \"TE_TIT_ID\" AS TEXT)='" + sInsulationType + "'";
                            }


                            //if (tcrating > 3)
                            //{

                            //    strQry += " AND '0'=COALESCE(\"TE_STAR_RATE\",0) ";
                            //}
                            //else
                            //{
                            if (starrating != "0")
                            {
                                strQry += " AND '" + starrating + "'=COALESCE(\"TE_STAR_RATE\",0) ";
                            }
                            else
                            {
                                strQry += " AND COALESCE(\"TC_RATING\",0)=COALESCE(\"TE_STAR_RATE\",0) ";
                            }
                            //   }

                            //AND COALESCE(\"TC_RATING\",0)=COALESCE(\"TE_STAR_RATE\",0)";
                            strQry += " AND \"FD_FEEDER_CODE\"=SUBSTR(\"DF_DTC_CODE\",1," + Constants.Feeder + ")";
                            strQry += "GROUP BY  \"DF_ID\",\"DF_DTC_CODE\", \"FD_FEEDER_NAME\",\"DT_NAME\" ,\"DTR_CODE\" ,\"DIVISION\",\"DF_EQUIPMENT_ID\",\"DF_DATE\",\"DF_REASON\",\"DF_LOC_CODE\",\"DF_DTR_COMMISSION_DATE\" ,\"DTC_COMMISSION_DATE\" ,Price ,\"TE_DECOMLABOUR\" ,\"TC_CAPACITY\",\"TC_CODE\",\"TC_SLNO\",\"REP\", \"TE_COMMLABOUR\",\"DT_TOTAL_CON_KW\",\"EMPLOYEECOST\",\"ESI\",\"SERVICETAX\",CONTINGENCYCOST,\"FINALTOTAL\",\"UNIT\",\"QUANTITY\",SubDivision,\"LOCATION\",\"TR_GUARANTY\",\"EST_NO\",\"LABOUR_COST\", \"DF_ENHANCE_CAPACITY\", \"TIT_INSULATION_NAME\", \"TT_NAME\",\"SDO_USERNAME\",\"SO_USERNAME\"";

                        }
                        else
                        {
                            strQry = " SELECT \"DF_ID\",\"DF_DTC_CODE\", \"FD_FEEDER_NAME\",\"DT_NAME\",CAST(\"TC_CODE\" AS TEXT) \"DTR_CODE\",(SELECT \"DIV_NAME\" FROM \"TBLDIVISION\"  WHERE CAST(\"DIV_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Division + ")) as \"DIVISION\",";
                            strQry += " CAST(\"DF_EQUIPMENT_ID\" AS TEXT) AS \"DF_EQUIPMENT_ID\",TO_CHAR(\"DF_DATE\",'dd/MM/yyyy') AS \"DF_DATE\",Replace(\"DF_REASON\",'ç',',') AS \"DF_REASON\",\"DF_LOC_CODE\",TO_CHAR(\"DF_DTR_COMMISSION_DATE\",'DD/MON/YYYY') AS \"DTR_COMMISSION_DATE\",to_char(\"DT_TRANS_COMMISION_DATE\",'dd/MON/yyyy') AS \"DTC_COMMISSION_DATE\",";
                            strQry += " \"TE_RATE\" as \"PRICE\" ,(\"TE_COMMLABOUR\" * '" + DecomLabourCost + "') AS \"TE_DECOMLABOUR\",CAST(\"TC_CAPACITY\" AS TEXT) \"TC_CAPACITY\",\"TC_CODE\",\"TC_SLNO\",'OLD' AS \"REP\",\"TE_COMMLABOUR\",";
                            strQry += " (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TC_MAKE_ID\"=\"TM_ID\") \"TM_NAME\",\"DT_TOTAL_CON_KW\", ";


                            // ADDED BY RUDRA FOR NEW RATE CHANGES 
                            strQry += "   (((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "'))  *'" + sEmployeeCost + "')/100 as \"EMPLOYEECOST\",";

                            strQry += " (((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "') ) * '" + sESI + "')/100 as \"ESI\",(\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "') as \"SERVICETAX\",((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "')/100)*2 as CONTINGENCYCOST,((((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "'))  *'" + sEmployeeCost + "')/100) + ((((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "') ) * '" + sESI + "')/100 ) + ((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 + (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "'))  as \"FINALTOTAL\",";



                            strQry += " 'No' as \"UNIT\",'1' as \"QUANTITY\",(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\"  WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ")) as \"SUBDIVISION\",";
                            strQry += " (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where CAST(\"OM_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Section + ")) as \"LOCATION\",";
                            //strQry += " \"EST_REPAIR\" AS \"TR_NAME\",(SELECT \"RSD_GUARRENTY_TYPE\" FROM \"TBLREPAIRSENTMASTER\",\"TBLREPAIRSENTDETAILS\" WHERE \"RSM_ID\"=\"RSD_RSM_ID\" AND \"RSD_ID\" = (SELECT MAX(\"RSD_ID\") FROM \"TBLREPAIRSENTDETAILS\"  WHERE \"RSD_TC_CODE\"= \"TC_CODE\" )) \"TR_GUARANTY\"";
                            strQry += " \"TR_NAME\" AS \"TR_NAME\",(SELECT \"RSD_GUARRENTY_TYPE\" FROM \"TBLREPAIRSENTMASTER\",\"TBLREPAIRSENTDETAILS\" WHERE \"RSM_ID\"=\"RSD_RSM_ID\" AND \"RSD_ID\" = (SELECT MAX(\"RSD_ID\") FROM \"TBLREPAIRSENTDETAILS\"  WHERE \"RSD_TC_CODE\"= \"TC_CODE\" )) \"TR_GUARANTY\"";
                            strQry += " ,\"EST_NO\" ,(1* \"TE_COMMLABOUR\" *'" + DecomLabourCost + "') \"LABOUR_COST\",CAST(\"DF_ENHANCE_CAPACITY\" AS TEXT) AS  \"DF_ENHANCE_CAPACITY\",\"TIT_INSULATION_NAME\", \"TT_NAME\" , ";
                            strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ") AND \"US_ROLE_ID\"='1' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SDO_USERNAME\",";
                            strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS BIGINT)=\"DF_LOC_CODE\" AND \"US_ROLE_ID\"='4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A'  AND \"US_ID\"=(SELECT \"DF_CRBY\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\"='" + sFailureId + "') LIMIT 1) \"SO_USERNAME\" ";
                            //strQry += " from  \"TBLDTCFAILURE\",\"TBLITEMMASTER\",\"TBLTCMASTER\",\"TBLDTCMAST\",\"TBLESTIMATION\",\"TBLFEEDERMAST\",\"TBLTRANSINSULATIONTYPE\",\"TBLTRANSCORETYPE\" WHERE \"DF_DTC_CODE\"=\"DT_CODE\" AND ";
                            strQry += " from  \"TBLDTCFAILURE\",\"TBLITEMMASTER\",\"TBLTCMASTER\",\"TBLDTCMAST\",\"TBLESTIMATION\",\"TBLFEEDERMAST\",\"TBLTRANSINSULATIONTYPE\",\"TBLTRANSCORETYPE\",\"TBLTRANSREPAIRER\" WHERE \"DF_DTC_CODE\"=\"DT_CODE\" AND  \"DF_REPAIRER_ID\"=\"TR_ID\" AND ";
                            strQry += " \"DF_EQUIPMENT_ID\"=\"TC_CODE\" AND \"DF_ID\" ='" + sFailureId + "' AND \"EST_DF_ID\"=\"DF_ID\" AND \"TC_CAPACITY\"=\"TE_CAPACITY\" AND \"TE_TIT_ID\"=\"TIT_ID\" AND \"TT_ID\"= \"TIT_TT_ID\" ";


                            if (sInsulationType == null || sInsulationType == "")
                            {
                                // strQry += " AND CAST( \"TE_TIT_ID\" AS TEXT)='" + sInsulationType + "'";


                            }
                            else
                            {
                                strQry += " AND CAST( \"TE_TIT_ID\" AS TEXT)='" + sInsulationType + "'";
                            }

                            //if (tcrating > 3)
                            //{

                            //    strQry += " AND '0'=COALESCE(\"TE_STAR_RATE\",0) ";
                            //}
                            //else
                            //{
                            if (starrating != "0")
                            {
                                strQry += " AND '" + starrating + "'=COALESCE(\"TE_STAR_RATE\",0) ";
                            }
                            else
                            {
                                strQry += " AND COALESCE(\"TC_RATING\",0)=COALESCE(\"TE_STAR_RATE\",0) ";
                            }
                            //  }

                            //AND COALESCE(\"TC_RATING\",0)=COALESCE(\"TE_STAR_RATE\",0)";
                            //strQry += " AND \"FD_FEEDER_CODE\"=SUBSTR(\"DF_DTC_CODE\",1," + Constants.Feeder + ")";
                            strQry += " AND \"FD_FEEDER_CODE\"=\"DT_FDRSLNO\" ";
                            strQry += "GROUP BY  \"DF_ID\",\"DF_DTC_CODE\", \"FD_FEEDER_NAME\",\"DT_NAME\" ,\"DTR_CODE\" ,\"DIVISION\",\"DF_EQUIPMENT_ID\",\"DF_DATE\",\"DF_REASON\",\"DF_LOC_CODE\",\"DF_DTR_COMMISSION_DATE\" ,\"DTC_COMMISSION_DATE\" ,Price ,\"TE_DECOMLABOUR\" ,\"TC_CAPACITY\",\"TC_CODE\",\"TC_SLNO\",\"REP\", \"TE_COMMLABOUR\",\"DT_TOTAL_CON_KW\",\"EMPLOYEECOST\",\"ESI\",\"SERVICETAX\",CONTINGENCYCOST,\"FINALTOTAL\",\"UNIT\",\"QUANTITY\",SubDivision,\"LOCATION\",\"TR_GUARANTY\",\"EST_NO\",\"LABOUR_COST\", \"DF_ENHANCE_CAPACITY\", \"TIT_INSULATION_NAME\", \"TT_NAME\",\"SDO_USERNAME\",\"SO_USERNAME\"";

                        }
                    }

                }


                dtDetailedReport = ObjCon.FetchDataTable(strQry);

                if (dtDetailedReport.Rows.Count > 0)
                {
                    if (String.IsNullOrEmpty(dtDetailedReport.Rows[0]["DTR_COMMISSION_DATE"].ToString()))
                    {
                        string df_id = dtDetailedReport.Rows[0]["DF_ID"].ToString();
                        string dtc_code = dtDetailedReport.Rows[0]["DF_DTC_CODE"].ToString();

                        strQry = "SELECT MAX(A.\"RANKID\") FROM (SELECT \"WO_WFO_ID\" \"RANKID\", \"dense_rank\"() over(ORDER BY \"WO_CR_ON\") from  \"TBLWORKFLOWOBJECTS\" WHERE \"WO_DATA_ID\"='" + dtc_code + "' AND \"WO_RECORD_ID\"='" + df_id + "' AND \"WO_BO_ID\"='9') \"A\" ";
                        string WFO_ID = ObjCon.get_value(strQry);

                        clsApproval objApproval = new clsApproval();
                        DataTable dtFailureDetails = new DataTable();
                        dtFailureDetails = objApproval.GetDatatableFromXML(WFO_ID);
                        if (dtFailureDetails.Rows.Count > 0)
                        {
                            //dtFailureDetails.Columns.RemoveAt(0);
                            if (dtFailureDetails.Columns.Contains("DTR_COMISSION_DATE"))
                            {
                                dtDetailedReport.Columns["DTR_COMMISSION_DATE"].ReadOnly = false;
                                dtDetailedReport.Rows[0]["DTR_COMMISSION_DATE"] = dtFailureDetails.Rows[0]["DTR_COMISSION_DATE"].ToString();
                                //dtDetailedReport.Columns.RemoveAt(10);
                                //dtDetailedReport.Columns.Add("DTR_COMISSION_DATE", typeof(string));
                                //DataRow dtrow = dtDetailedReport.NewRow();
                                //dtrow["DTR_COMISSION_DATE"] = dtFailureDetails.Rows[0]["DTR_COMISSION_DATE"].ToString();
                                //dtDetailedReport.Rows.Add(dtrow);
                            }
                            else
                            {
                                dtDetailedReport.Columns["DTR_COMMISSION_DATE"].ReadOnly = false;
                                dtDetailedReport.Rows[0]["DTR_COMMISSION_DATE"] = "";
                            }
                        }
                    }
                }
                ObjCon.CommitTransaction();
                return dtDetailedReport;
            }
            catch (Exception ex)
            {
                ObjCon.RollBackTrans();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtDetailedReport;

            }
        }





        public DataTable GetPGRSReport(string sFailureId)
        {
            DataTable dt = new DataTable();
            try
            {
                String sQry = String.Empty;
                sQry = "SELECT 'TRANSFORMER FAILURE' AS \"COMPLAINT_NAME\", \"ZO_NAME\" \"ZONE\", \"CM_CIRCLE_NAME\" \"CIRCLE\", \"DIV_NAME\" ";
                sQry += " \"DIVISION\", \"SD_SUBDIV_NAME\" \"SUBDIV\", \"TQ_NAME\" \"TALUK\",\"OM_NAME\" \"SECTION\",\"DT_NAME\" \"DTCNAME\", ";
                sQry += " \"TC_CAPACITY\" \"CAPACITY\", \"TM_NAME\" \"MAKE\", \"TC_SLNO\" \"SLNO\", \"DT_CODE\" \"DTCCODE\",";
                sQry += " (SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'POU' and \"MD_ID\"=\"DF_PURPOSE\") AS \"PURPOSE\", \"FD_FEEDER_CODE\" \"FEEDERCODE\", ";
                sQry += " TO_CHAR(\"DT_TRANS_COMMISION_DATE\",'DD-MM-YYYY') \"DOC\", TO_CHAR(\"DF_DATE\",'DD-MM-YYYY') \"DOF\", '' AS \"MAJOR\",  \"DF_GUARANTY_TYPE\" AS \"GUARANTY_TYPE\", \"DT_TOTAL_CON_HP\" \"CON_LOAD\", ";
                sQry += " \"DF_CUSTOMER_NAME\" AS \"CONS_NAME\", \"DF_CUSTOMER_NUMBER\" AS \"PHONENO\" , \"TC_CODE\" \"DTR_CODE\", \"DF_ID\" \"FAILURE_ID\", \"DF_CONSUMER_COMPLAINT_NUM\" AS \"PGRS_DOCNO\",TO_CHAR(\"DF_CONSUMER_COMPLAINT_DATE\",'DD-MM-YYYY') ";
                sQry += " \"PGRS_DOCDATE\" FROM \"TBLDTCFAILURE\", \"TBLZONE\", \"TBLCIRCLE\", \"TBLDIVISION\", \"TBLSUBDIVMAST\", \"TBLOMSECMAST\", ";
                sQry += " \"TBLTALQ\", \"TBLDTCMAST\", \"TBLTCMASTER\", \"TBLTRANSMAKES\", \"TBLFEEDERMAST\", \"TBLSTATION\" WHERE \"DF_EQUIPMENT_ID\" = \"TC_CODE\" ";
                sQry += " AND \"DF_DTC_CODE\" = \"DT_CODE\" AND \"DF_LOC_CODE\" = \"OM_CODE\" AND \"OM_SUBDIV_CODE\" = \"SD_SUBDIV_CODE\" AND ";
                sQry += " \"SD_DIV_CODE\" = \"DIV_CODE\" AND \"DIV_CICLE_CODE\" = \"CM_CIRCLE_CODE\" AND \"CM_ZO_ID\" = \"ZO_ID\" AND ";
                sQry += " \"DT_FDRSLNO\" = \"FD_FEEDER_CODE\" AND \"FD_ST_ID\" = \"ST_ID\" AND CAST(\"TQ_CODE\" AS TEXT) = SUBSTR(CAST(\"ST_STATION_CODE\" AS TEXT),1,2) ";
                sQry += " AND \"TC_MAKE_ID\" = \"TM_ID\" AND \"DF_ID\" = '" + sFailureId + "' ORDER BY \"TC_CODE\"";
                dt = ObjCon.FetchDataTable(sQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        public DataTable GetFailLocation(string sFailDTC_CODE)
        {
            DataTable dt = new DataTable();
            try
            {
                String sQry = String.Empty;
                sQry = "SELECT 'TRANSFORMER FAILURE' AS \"COMPLAINT_NAME\", \"ZO_NAME\" \"ZONE\", \"CM_CIRCLE_NAME\" \"CIRCLE\", \"DIV_NAME\" ";
                sQry += " \"DIVISION\", \"SD_SUBDIV_NAME\" \"SUBDIV\", \"TQ_NAME\" \"TALUK\",\"OM_NAME\" \"SECTION\",\"DT_NAME\" \"DTCNAME\", ";
                sQry += " \"TC_CAPACITY\" \"CAPACITY\", \"TM_NAME\" \"MAKE\", \"TC_SLNO\" \"SLNO\", \"DT_CODE\" \"DTCCODE\", ";
                sQry += " (SELECT \"SCHM_NAME\"  FROM \"TBLDTCSCHEME\" WHERE \"SCHM_ID\" = \"DT_SCHEME_TYPE\") \"PURPOSE\", \"FD_FEEDER_CODE\" \"FEEDERCODE\", ";
                sQry += " TO_CHAR(\"DT_TRANS_COMMISION_DATE\",'DD-MM-YYYY') \"DOC\", '' AS \"MAJOR\", '' AS \"AGP\", \"DT_TOTAL_CON_HP\" \"CON_LOAD\",";
                sQry += " \"TC_CODE\" \"DTR_CODE\" FROM \"TBLZONE\", \"TBLCIRCLE\", \"TBLDIVISION\", \"TBLSUBDIVMAST\", \"TBLOMSECMAST\", ";
                sQry += " \"TBLTALQ\", \"TBLDTCMAST\", \"TBLTCMASTER\", \"TBLTRANSMAKES\", \"TBLFEEDERMAST\", \"TBLSTATION\" WHERE ";
                sQry += " \"OM_SUBDIV_CODE\" = \"SD_SUBDIV_CODE\" AND  \"SD_DIV_CODE\" = \"DIV_CODE\" AND \"DIV_CICLE_CODE\" = \"CM_CIRCLE_CODE\" ";
                sQry += " AND \"CM_ZO_ID\" = \"ZO_ID\" AND  \"DT_FDRSLNO\" = \"FD_FEEDER_CODE\" AND \"FD_ST_ID\" = \"ST_ID\" AND ";
                sQry += " CAST(\"TQ_CODE\" AS TEXT) = SUBSTR(CAST(\"ST_STATION_CODE\" AS TEXT),1,2)  AND \"TC_MAKE_ID\" = \"TM_ID\" ";
                sQry += " AND \"DT_TC_ID\"=\"TC_CODE\" AND \"DT_OM_SLNO\"=\"OM_CODE\" AND \"DT_CODE\"='" + sFailDTC_CODE + "' ORDER BY \"TC_CODE\";";
                dt = ObjCon.FetchDataTable(sQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        public DataTable NewDTC_WO_IndentDetails(string sWOSlno, string sstoreId)
        {
            DataTable dtIndentDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                //strQry = "select TI_INDENT_NO,TO_CHAR(TI_INDENT_DATE,'dd/MM/yyyy')TI_INDENT_DATE,TO_CHAR(TC_CAPACITY) TC_CAPACITY,TO_CHAR(WO_DATE,'dd/MM/yyyy') WO_DATE,";
                //strQry += " (select SM_NAME from TBLSTOREMAST WHERE SM_ID=TI_STORE_ID) SM_NAME,EST_UNIT_PRICE,";
                //strQry += " (SELECT WO_NO from TBLWORKORDER WHERE WO_SLNO=TI_WO_SLNO) WO_NO,TI_DEVICE_ID from TBLTCMASTER,TBLWORKORDER,TBLINDENT,tblestimation where est_DF_ID=WO_DF_ID";
                //strQry += " AND TI_DEVICE_ID=TC_CODE AND TI_WO_SLNO=WO_SLNO and TI_ID='" + strIndentId + "'";

                strQry = "SELECT null as \"TI_INDENT_NO\",TO_CHAR(CURRENT_DATE,'dd/MM/yyyy') \"TI_INDENT_DATE\",\"WO_NEW_CAP\",";
                strQry += " TO_CHAR(\"WO_DATE\",'dd/MM/yyyy') \"WO_DATE\", (select \"SM_NAME\" from \"TBLSTOREMAST\" WHERE \"SM_ID\"=" + sstoreId + ") \"SM_NAME\",";
                strQry += " \"WO_NO\",\"WO_ACC_CODE\",\"WO_AMT\",";
                strQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT)=SUBSTR(CAST(\"WO_REQUEST_LOC\" AS TEXT),1,'" + Constants.SubDivision + "')) AS \"SUBDIVISION\",";
                strQry += "(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE CAST(\"OM_CODE\" AS TEXT)=SUBSTR(CAST(\"WO_REQUEST_LOC\" AS TEXT),1,'" + Constants.Section + "')) AS \"SECTION\",";
                strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\"  AS TEXT)=SUBSTR(CAST(\"WO_REQUEST_LOC\" AS TEXT),1,'" + Constants.Division + "')) AS \"DIVISION\",";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=SUBSTR(CAST(\"WO_REQUEST_LOC\" AS TEXT),1,'" + Constants.SubDivision + "') AND CAST(\"US_ROLE_ID\" AS TEXT)='1' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"STO_USERNAME\" ";
                strQry += " ,(SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=CAST(\"WO_REQUEST_LOC\" AS TEXT) AND \"US_ROLE_ID\"='4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SO_USERNAME\" FROM ";
                strQry += " \"TBLWORKORDER\" where ";
                strQry += "   CAST(\"WO_SLNO\" AS TEXT)='" + sWOSlno + "'";

                dtIndentDetails = ObjCon.FetchDataTable(strQry);
                return dtIndentDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtIndentDetails;

            }

        }
        public DataTable NewDTC_CR_CommiDetails(string sDTid)
        {
            DataTable dtIndentDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                //strQry = "select TI_INDENT_NO,TO_CHAR(TI_INDENT_DATE,'dd/MM/yyyy')TI_INDENT_DATE,TO_CHAR(TC_CAPACITY) TC_CAPACITY,TO_CHAR(WO_DATE,'dd/MM/yyyy') WO_DATE,";
                //strQry += " (select SM_NAME from TBLSTOREMAST WHERE SM_ID=TI_STORE_ID) SM_NAME,EST_UNIT_PRICE,";
                //strQry += " (SELECT WO_NO from TBLWORKORDER WHERE WO_SLNO=TI_WO_SLNO) WO_NO,TI_DEVICE_ID from TBLTCMASTER,TBLWORKORDER,TBLINDENT,tblestimation where est_DF_ID=WO_DF_ID";
                //strQry += " AND TI_DEVICE_ID=TC_CODE AND TI_WO_SLNO=WO_SLNO and TI_ID='" + strIndentId + "'";

                //strQry = "SELECT null as \"TI_INDENT_NO\",TO_CHAR(CURRENT_DATE,'dd/MM/yyyy') \"TI_INDENT_DATE\",\"WO_NEW_CAP\",";
                //strQry += " TO_CHAR(\"WO_DATE\",'dd/MM/yyyy') \"WO_DATE\", (select \"SM_NAME\" from \"TBLSTOREMAST\" WHERE \"SM_ID\"=\"TC_STORE_ID\") \"SM_NAME\",";
                //strQry += " \"WO_NO\",\"WO_ACC_CODE\",\"WO_AMT\",";
                //strQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT)=SUBSTR(CAST(\"WO_REQUEST_LOC\" AS TEXT),1,'" + Constants.SubDivision + "')) AS \"SUBDIVISION\",";
                //strQry += "(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE CAST(\"OM_CODE\" AS TEXT)=SUBSTR(CAST(\"WO_REQUEST_LOC\" AS TEXT),1,'" + Constants.Section + "')) AS \"SECTION\",";
                //strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\"  AS TEXT)=SUBSTR(CAST(\"WO_REQUEST_LOC\" AS TEXT),1,'" + Constants.Division + "')) AS \"DIVISION\",";
                //strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=SUBSTR(CAST(\"WO_REQUEST_LOC\" AS TEXT),1,'" + Constants.SubDivision + "') AND CAST(\"US_ROLE_ID\" AS TEXT)='1' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"STO_USERNAME\" ";
                //strQry += " ,(SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=CAST(\"WO_REQUEST_LOC\" AS TEXT) AND \"US_ROLE_ID\"='4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SO_USERNAME\" FROM ";
                //strQry += " \"TBLWORKORDER\" where ";
                //strQry += "   CAST(\"WO_SLNO\" AS TEXT)='" + sWOSlno + "'";

                strQry = "SELECT \"DT_CODE\",\"DT_NAME\",cast(\"TC_CODE\" as text)\"TC_CODE\",\"TC_SLNO\",TO_CHAR(\"DT_TRANS_COMMISION_DATE\",'dd/MM/yyyy') \"DT_TRANS_COMMISION_DATE\" ,\"WO_NEW_CAP\",\"DT_NEWDTC_TTK_FLOW\" as\"WO_TTK_STATUS\",(CASE WHEN \"DT_NEWDTC_TTK_FLOW\" = 1 THEN 'TTK FLOW' ELSE 'PTK FLOW' END )AS \"PROJECT_TYPE\",cast(\"WO_TTK_AUTO_NO\" as text)\"WO_TTK_AUTO_NO\",\"WO_TTK_MANUAL_NO\"";
                strQry += " ,\"WO_TTK_VENDOR_NAME\",cast(\"TC_CAPACITY\" as text)\"TC_CAPACITY\", TO_CHAR(\"WO_DATE\", 'dd/MM/yyyy') \"WO_DATE\",(SELECT \"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\" WHERE CAST(\"FD_FEEDER_CODE\" AS TEXT)= ";
                strQry += " SUBSTR(CAST(\"DT_CODE\" AS TEXT), 1, '6')) AS \"FEEDER\", (select \"SM_NAME\" from \"TBLSTOREMAST\" WHERE \"SM_ID\" = \"TC_STORE_ID\") \"SM_NAME\",(select \"TM_NAME\" from \"TBLTRANSMAKES\"";
                strQry += " WHERE \"TM_ID\" = \"TC_MAKE_ID\") \"MAKE_NAME\",\"WO_NO\",\"WO_ACC_CODE\",\"WO_AMT\", (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT)= SUBSTR(CAST(\"WO_REQUEST_LOC\" AS TEXT), 1, '4'))";
                strQry += " AS \"SUBDIVISION\",(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE CAST(\"OM_CODE\" AS TEXT)= SUBSTR(CAST(\"WO_REQUEST_LOC\" AS TEXT), 1, '5')) AS \"SECTION\", (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\"  AS TEXT)= ";
                strQry += " SUBSTR(CAST(\"WO_REQUEST_LOC\" AS TEXT), 1, '3')) AS \"DIVISION\", (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\" = SUBSTR(CAST(\"WO_REQUEST_LOC\" AS TEXT), 1, '4') AND CAST(\"US_ROLE_ID\" AS TEXT)= '1' AND \"US_MMS_ID\"";
                strQry += " IS NULL AND \"US_STATUS\" = 'A' LIMIT 1) \"STO_USERNAME\"  ,(SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)= CAST(\"WO_REQUEST_LOC\" AS TEXT) AND \"US_ROLE_ID\" = '4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\" =";
                strQry += " 'A' LIMIT 1) \"SO_USERNAME\" FROM  \"TBLWORKORDER\",\"TBLDTCMAST\",\"TBLTCMASTER\" where   ";
                strQry += "  \"WO_SLNO\" = \"DT_WO_ID\" and \"TC_CODE\" = \"DT_TC_ID\" AND  CAST(\"DT_CODE\" AS TEXT)='" + sDTid + "'";

                dtIndentDetails = ObjCon.FetchDataTable(strQry);
                return dtIndentDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtIndentDetails;

            }

        }
        public DataTable NewDtcInvoiceReport(string sInvoiceId, string sOfficeCode, string sCapacity)
        {
            DataTable dtIndentDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                //strQry = "select TI_INDENT_NO,TO_CHAR(TI_INDENT_DATE,'dd/MM/yyyy')TI_INDENT_DATE,TO_CHAR(TC_CAPACITY) TC_CAPACITY,TO_CHAR(WO_DATE,'dd/MM/yyyy') WO_DATE,";
                //strQry += " (select SM_NAME from TBLSTOREMAST WHERE SM_ID=TI_STORE_ID) SM_NAME,EST_UNIT_PRICE,";
                //strQry += " (SELECT WO_NO from TBLWORKORDER WHERE WO_SLNO=TI_WO_SLNO) WO_NO,TI_DEVICE_ID from TBLTCMASTER,TBLWORKORDER,TBLINDENT,tblestimation where est_DF_ID=WO_DF_ID";
                //strQry += " AND TI_DEVICE_ID=TC_CODE AND TI_WO_SLNO=WO_SLNO and TI_ID='" + strIndentId + "'";
                if (sOfficeCode.Length > 2)
                {
                    sOfficeCode = sOfficeCode.Substring(0, Constants.Division);
                }
                strQry = "SELECT \"TI_INDENT_NO\",TO_CHAR(\"TI_INDENT_DATE\",'dd/MM/yyyy') \"TI_INDENT_DATE\",CAST(\"WO_DTC_CAP\" AS TEXT) \"TC_CAPACITY\",\"WO_NEW_CAP\",";
                strQry += " TO_CHAR(\"WO_DATE\",'dd/MM/yyyy') \"WO_DATE\", (select \"SM_NAME\" from \"TBLSTOREMAST\" WHERE \"SM_ID\"=\"TI_STORE_ID\") \"SM_NAME\",";
                strQry += "\"WO_NO\",\"WO_ACC_CODE\",\"WO_AMT\",\"IN_MANUAL_INVNO\",null as \"EST_UNIT_PRICE\",null as \"EST_TOTALOILVAL\",'0' as \"EST_OIL_QNTY\",";
                strQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT)=SUBSTR(CAST(\"WO_REQUEST_LOC\" AS TEXT),1,'" + Constants.SubDivision + "')) AS \"SUBDIVISION\",";
                strQry += "(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE CAST(\"OM_CODE\" AS TEXT)=SUBSTR(CAST(\"WO_REQUEST_LOC\" AS TEXT),1,'" + Constants.Section + "')) AS \"SECTION\",";
                strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT)=SUBSTR(CAST(\"WO_REQUEST_LOC\" AS TEXT),1,'" + Constants.Division + "')) AS \"DIVISION\",";
                strQry += " \"IN_INV_NO\",TO_CHAR(\"IN_DATE\",'DD/MM/YYYY') \"IN_DATE\",\"IN_AMT\", ";
                strQry += " (SELECT COUNT(*) FROM \"TBLTCMASTER\" WHERE \"TC_STORE_ID\" IN (SELECT \"SM_ID\" FROM \"TBLSTOREMAST\" WHERE  CAST(\"SM_ID\" AS TEXT)='" + sOfficeCode + "') AND \"TC_STATUS\" IN (1,2) ";
                strQry += " AND \"TC_CURRENT_LOCATION\"=1 AND \"TC_CAPACITY\"='" + sCapacity + "') \"STOCK_COUNT\",";
                strQry += " CAST(\"TC_CODE\" AS TEXT) \"TC_CODE\",CAST(\"TC_SLNO\" AS TEXT) \"TC_SLNO\",";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=SUBSTR(CAST(\"WO_REQUEST_LOC\" AS TEXT),1,'" + Constants.Division + "') AND CAST(\"US_ROLE_ID\" AS TEXT)='2'  AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"STO_USERNAME\"";
                strQry += " FROM \"TBLTCMASTER\",";
                strQry += " \"TBLWORKORDER\",\"TBLINDENT\",\"TBLDTCINVOICE\",\"TBLTCDRAWN\" where  \"TI_WO_SLNO\"=\"WO_SLNO\" and \"TI_ID\"=\"IN_TI_NO\"";
                strQry += " AND CAST(\"IN_NO\" AS TEXT)='" + sInvoiceId + "'  AND \"TD_TC_NO\"=\"TC_CODE\" AND \"TD_INV_NO\"=\"IN_NO\" ";
                dtIndentDetails = ObjCon.FetchDataTable(strQry);
                return dtIndentDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtIndentDetails;
            }

        }
        public DataTable PrintWorkOrderDetailsForNewDTC(string sWoId)
        {
            DataTable dtWODetails = new DataTable();
            try
            {
                string strQry = string.Empty;

                strQry = "SELECT \"WO_SLNO\",\"WO_NO\",TO_CHAR(\"WO_DATE\",'dd/MM/yyyy') \"WO_DATE\",CAST(\"WO_AMT\" AS INT)\"WO_AMT\",\"WO_ACC_CODE\",\"WO_ISSUED_BY\",cast(\"WO_NEW_CAP\" AS INT)\"WO_NEW_CAP\",\"WO_DWA_NAME\",TO_CHAR(\"WO_DWA_DATE\",'dd/MM/yyyy')\"WO_DWA_DATE\",\"WO_RATING\"";
                strQry += " ,(SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"WO_CRBY\"=\"US_ID\" AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A') \"US_FULL_NAME\",\"WO_REQUEST_LOC\" FROM \"TBLWORKORDER\" WHERE ";
                strQry += " \"WO_SLNO\"='" + sWoId + "' ";
                dtWODetails = ObjCon.FetchDataTable(strQry);

                return dtWODetails;
            }

            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtWODetails;

            }

        }
        public DataTable PrintSurveyReport(string sFailureId)
        {
            DataTable dtDetailedReport = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "SELECT DISTINCT \"DF_EQUIPMENT_ID\",TO_CHAR(\"DF_DATE\",'DD/MM/YYYY') AS \"DF_DATE\",CAST(\"TC_CAPACITY\" AS TEXT) \"TC_CAPACITY\",\"DF_ENHANCE_CAPACITY\",TO_CHAR(\"TC_MANF_DATE\",'DD/MM/YYYY') AS \"TC_MANF_DATE\",\"SM_NAME\",\"DF_LOC_CODE\",\"TM_NAME\",\"TC_SLNO\",";
                strQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE \"SD_SUBDIV_CODE\" =SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Section + "')) AS \"SUBDIVISION\",";
                strQry += "(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_CODE\"=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Section + "')) AS \"LOCATION\",";
                strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE \"DIV_CODE\" =SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "')) AS \"DIVISION\",\"EST_NO\", ";
                strQry += " CASE \"DF_HT_BUSING\"  WHEN 1 THEN 'GOOD' WHEN 2 THEN 'BAD' ELSE '' END AS \"DF_HT_BUSING\",CASE \"DF_LT_BUSING\"  WHEN 1 THEN 'GOOD' WHEN 2 THEN 'BAD' ELSE '' END AS \"DF_LT_BUSING\",";
                strQry += "CASE \"DF_HT_BUSING_ROD\"  WHEN 1 THEN 'GOOD' WHEN 2 THEN 'BAD' ELSE '' END AS \"DF_HT_BUSING_ROD\",CASE \"DF_LT_BUSING_ROD\"  WHEN 1 THEN 'GOOD' WHEN 2 THEN 'BAD' ELSE '' END AS \"DF_LT_BUSING_ROD\",CASE \"DF_OIL_LEVEL\"  WHEN 1 THEN 'YES' WHEN 2 THEN 'NO' ELSE '' END AS \"DF_OIL_LEVEL\",";
                strQry += " CASE \"DF_BREATHER\"  WHEN '1' THEN 'YES' WHEN '2' THEN 'NO' ELSE '' END AS \"DF_BREATHER\",";
                strQry += "CASE \"DF_DRAIN_VALVE\"  WHEN 1 THEN 'YES' WHEN 2 THEN 'NO' ELSE '' END AS \"DF_DRAIN_VALVE\",DF_OIL_QNTY\",CASE \"DF_TANK_CONDITION\" WHEN 1 THEN 'GOOD' WHEN 2 THEN 'BAD' ELSE '' END AS \"DF_TANK_CONDITION\",";
                strQry += "CASE \"DF_WHEEL\" WHEN 1 THEN 'YES' WHEN 2 THEN 'NO' ELSE '' END AS \"DF_WHEEL\",CASE \"DF_EXPLOSION\" WHEN 1 THEN 'YES' WHEN 2 THEN 'NO' ELSE '' END AS \"DF_EXAMPLING\", ";
                strQry += " \"EST_REPAIR\" AS \"TR_NAME\", ";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\" =SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Section + "') AND \"US_ROLE_ID\"='1' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SDO_USERNAME\",";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=\"DF_LOC_CODE\" AND \"US_ROLE_ID\"='4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A'  AND \"US_ID\"=(SELECT \"DF_CRBY\" FROM \"TBLDTCFAILURE\" WHERE CAST(\"DF_ID\" AS TEXT)='" + sFailureId + "') LIMIT 1)  \"SO_USERNAME\" ";
                strQry += " FROM \"TBLTCMASTER\",\"TBLDTCFAILURE\",\"TBLTRANSMAKES\",\"TBLSTOREMAST\",\"TBLESTIMATION\" WHERE \"TC_CODE\"=\"DF_EQUIPMENT_ID\" AND \"TM_ID\"=\"TC_MAKE_ID\" AND";
                strQry += "  \"TC_STORE_ID\"=\"SM_ID\" AND \"DF_EQUIPMENT_ID\"=\"TC_CODE\" AND CAST(\"DF_ID\" AS TEXT) ='" + sFailureId + "' AND \"EST_DF_ID\"=\"DF_ID\" ";
                dtDetailedReport = ObjCon.FetchDataTable(strQry);
                return dtDetailedReport;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtDetailedReport;
            }
        }



        #region old CR CODE
        //public DataTable CompletionReport(string sDecommId)
        //{
        //    DataTable dtCompleteReport = new DataTable();
        //    string strQry = string.Empty;
        //    try
        //    {
        //        //strQry = "select DISTINCT WO_NO,TO_CHAR(wo_date,'dd/MM/yyyy') wo_date,to_char(EST_FAULT_CAPACITY)EST_FAULT_CAPACITY,to_char(EST_REPLACE_CAPACITY)EST_REPLACE_CAPACITY,WO_DEVICE_ID,";
        //        //strQry += " (SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST WHERE SD_SUBDIV_CODE=SUBSTR(WO_OFF_CODE,1,3)) as SUBDIVISION,";
        //        //strQry += " (SELECT OM_NAME FROM TBLOMSECMAST where OM_CODE=SUBSTR(WO_OFF_CODE,1,4)) as SECTION,";
        //        //strQry += " EST_UNIT_PRICE,TR_RI_NO,IN_INV_NO,WO_AMT from TBLWORKORDER,tbltcreplace,TBLDTCINVOICE,TBLINDENT,TBLTCMASTER,TBLESTIMATION where";
        //        //strQry += " TR_IN_NO=IN_NO AND IN_TI_NO=TI_ID AND TI_WO_SLNO=WO_SLNO AND EST_DF_ID=WO_DF_ID AND tr_id='" + sDecommId + "'";

        //        strQry = "  SELECT DISTINCT WO_NO,WO_NO_DECOM,TO_CHAR(WO_DATE,'DD/MM/YYYY') WO_DATE,TO_CHAR(WO_DTC_CAP)EST_FAULT_CAPACITY,DT_NAME,";
        //        strQry += " TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY') AS TC_MANF_DATE,  TO_CHAR(IN_DATE,'DD-MON-YYYY') AS IN_DATE,TC_CODE,TC_SLNO, ";
        //        strQry += " TR_RV_NO ACK_NO, TO_CHAR(TR_RV_DATE,'DD-MON-YYYY') AS ACK_DATE,";
        //        strQry += " TO_CHAR(WO_NEW_CAP)EST_REPLACE_CAPACITY,WO_DEVICE_ID,";
        //        strQry += " (SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE = SUBSTR(DF_LOC_CODE,1,2)) DIVISION,";
        //        strQry += " (SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST WHERE SD_SUBDIV_CODE=SUBSTR(DF_LOC_CODE,1,3)) AS SUBDIVISION,";
        //        strQry += " (SELECT OM_NAME FROM TBLOMSECMAST WHERE OM_CODE=SUBSTR(DF_LOC_CODE,1,4)) AS SECTION,";
        //        strQry += " (SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID) MAKE,";
        //        strQry += "  WO_AMT AS EST_UNIT_PRICE,TR_RI_NO,IN_INV_NO,WO_AMT,TR_INVENTORY_QTY,TR_DECOM_INV_QTY, ";
        //        strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=SUBSTR(DF_LOC_CODE,1,3) AND US_ROLE_ID='1' AND ROWNUM='1') SDO_USERNAME,";
        //        strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=DF_LOC_CODE AND US_ROLE_ID='4' AND ROWNUM='1') SO_USERNAME ";
        //        strQry += " FROM TBLTCMASTER,TBLWORKORDER,TBLINDENT,TBLDTCINVOICE,TBLTCREPLACE,TBLDTCFAILURE,TBLDTCMAST WHERE  ";
        //        strQry += " TR_IN_NO=IN_NO AND IN_TI_NO=TI_ID AND TI_WO_SLNO=WO_SLNO AND DF_ID=WO_DF_ID  AND DF_EQUIPMENT_ID = TC_CODE AND DT_CODE = DF_DTC_CODE AND TR_ID='" + sDecommId + "'";

        //        OleDbDataReader dr = ObjCon.Fetch(strQry);
        //        dtCompleteReport.Load(dr);
        //        return dtCompleteReport;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CompleteReport");
        //        return dtCompleteReport;

        //    }

        //}
        #endregion

        #region New CR CODE

        public DataTable CompletionReport(string sDecommId)
        {
            DataTable dtCompleteReport = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "  SELECT DISTINCT \"WO_NO\",'' as \"LEVEL\",\"WO_NO_DECOM\",TO_CHAR(\"WO_DATE\",'DD-MM-YYYY') \"WO_DATE\",CAST(\"WO_DTC_CAP\" AS TEXT)\"EST_FAULT_CAPACITY\",\"DT_NAME\",";
                strQry += " TO_CHAR(\"TC_MANF_DATE\",'DD-MON-YYYY') AS \"TC_MANF_DATE\",  TO_CHAR(\"IN_DATE\",'DD-MON-YYYY') AS \"IN_DATE\",CAST(\"TC_CODE\" AS TEXT)\"TC_CODE\",\"TC_SLNO\", ";
                strQry += " \"TR_RV_NO\" \"ACK_NO\", TO_CHAR(\"TR_RV_DATE\",'DD-MON-YYYY') AS \"ACK_DATE\",TO_CHAR(\"DF_DATE\",'DD-MON-YYYY')\"DF_DATE\",\"EST_NO\",to_char(\"EST_CRON\",'DD-MON-YYYY')\"EST_CRON\",\"DF_DTC_CODE\",";
                strQry += " CAST(\"WO_NEW_CAP\" AS TEXT)\"EST_REPLACE_CAPACITY\",\"WO_DEVICE_ID\",\"TI_INDENT_NO\",TO_CHAR(\"TI_INDENT_DATE\",'DD-MON-YYYY')\"TI_INDENT_DATE\",";
                strQry += " \"IN_INV_NO\",TO_CHAR(\"IN_DATE\",'DD-MON-YYYY')\"IN_DATE\",\"TR_RI_NO\",TO_CHAR(\"TR_RI_DATE\",'DD-MON-YYYY')\"TR_RI_DATE\",";
                strQry += " \"TR_RV_NO\",TO_CHAR(\"TR_RV_DATE\",'DD-MON-YYYY')\"TR_RV_DATE\",(with Grp as( SELECT rank() over (partition by \"WO_RECORD_ID\" order by \"WO_ID\" desc),\"WO_CR_ON\" FROM \"TBLWORKFLOWOBJECTS\" WHERE ";
                strQry += " \"WO_RECORD_ID\"=\"TR_WO_SLNO\" and \"WO_BO_ID\"='26') 	SELECT TO_CHAR(\"WO_CR_ON\",'DD-MON-YYYY') \"TR_COMM_DATE\" FROM Grp WHERE rank =1),";
                strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT) = SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "')) \"DIVISION\",";
                strQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "')) AS \"SUBDIVISION\",";
                strQry += " (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE CAST(\"OM_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Section + "')) AS \"SECTION\",";
                strQry += " (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\") \"MAKE\",TO_CHAR(\"DF_DTR_COMMISSION_DATE\",'DD-MON-YYYY') AS \"DTR_COMMISSION_DATE\",";
                strQry += "  \"WO_AMT\" AS \"EST_UNIT_PRICE\",\"WO_AMT\",\"TR_INVENTORY_QTY\",\"TR_DECOM_INV_QTY\",TO_CHAR(\"DT_TRANS_COMMISION_DATE\",'DD-MON-YYYY')\"DTC_COMMISSION_DATE\", ";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "') AND \"US_ROLE_ID\"='1' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A') SDO_USERNAME,";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "') AND \"US_ROLE_ID\"='24' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A') AO_USERNAME,";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "') AND \"US_ROLE_ID\"='3' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A') EE_USERNAME,";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=CAST(\"DF_LOC_CODE\" AS TEXT) AND CAST(\"US_ROLE_ID\" AS TEXT)='4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A') \"SO_USERNAME\" ";
                strQry += " FROM \"TBLTCMASTER\",\"TBLWORKORDER\",\"TBLINDENT\",\"TBLDTCINVOICE\",\"TBLTCREPLACE\",\"TBLDTCFAILURE\",\"TBLDTCMAST\",\"TBLTRANSDTCMAPPING\",\"TBLESTIMATIONDETAILS\" WHERE  ";
                strQry += " \"TR_IN_NO\"=\"IN_NO\" AND \"IN_TI_NO\"=\"TI_ID\" AND \"TI_WO_SLNO\"=\"WO_SLNO\" AND \"DF_ID\"=\"WO_DF_ID\"  AND \"DF_EQUIPMENT_ID\" = \"TC_CODE\" AND \"DT_CODE\" = \"DF_DTC_CODE\" ";
                strQry += " AND \"TM_DTC_ID\"=\"DF_DTC_CODE\" and  \"EST_FAILUREID\"=\"DF_ID\" AND \"TM_LIVE_FLAG\"='0' AND \"TR_WO_SLNO\"='" + sDecommId + "'";

                dtCompleteReport = ObjCon.FetchDataTable(strQry);
                return dtCompleteReport;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtCompleteReport;

            }

        }

        #endregion 

        public DataTable CRDetails(clsReports objReport)
        {
            DataTable dtCompleteReport = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "  SELECT DISTINCT \"WO_NO\",\"WO_NO\" as \"LEVEL\",\"WO_NO_DECOM\",\"DF_DTC_CODE\",TO_CHAR(\"WO_DATE\",'DD/MM/YYYY') \"WO_DATE\",CAST(\"WO_DTC_CAP\" AS TEXT)\"EST_FAULT_CAPACITY\",\"DT_NAME\",";
                strQry += " TO_CHAR(\"TC_MANF_DATE\",'DD-MON-YYYY') AS \"TC_MANF_DATE\",  TO_CHAR(\"IN_DATE\",'DD-MON-YYYY') AS \"IN_DATE\",CAST(\"TC_CODE\" AS TEXT)\"TC_CODE\",\"TC_SLNO\", ";
                strQry += " \"TR_RV_NO\" \"ACK_NO\", TO_CHAR(\"TR_RV_DATE\",'DD-MON-YYYY') AS \"ACK_DATE\",TO_CHAR(\"DF_DATE\",'DD/MON/YYYY')\"DF_DATE\",TO_CHAR(\"DF_DTR_COMMISSION_DATE\",'DD/MON/YYYY') AS \"DTR_COMMISSION_DATE\",";
                strQry += " CAST(\"WO_NEW_CAP\" AS TEXT)\"EST_REPLACE_CAPACITY\",\"WO_DEVICE_ID\",\"TI_INDENT_NO\",TO_CHAR(\"TI_INDENT_DATE\",'DD/MON/YYYY')\"TI_INDENT_DATE\",TO_CHAR(\"DT_TRANS_COMMISION_DATE\",'DD/MON/YYYY')\"DTC_COMMISSION_DATE\",";
                strQry += " \"IN_INV_NO\",TO_CHAR(\"IN_DATE\",'DD/MON/YYYY')\"IN_DATE\",\"TR_RI_NO\",TO_CHAR(\"TR_RI_DATE\",'DD/MON/YYYY')\"TR_RI_DATE\",";
                strQry += " \"TR_RV_NO\",TO_CHAR(\"TR_RV_DATE\",'DD/MON/YYYY')\"TR_RV_DATE\",(SELECT max(to_char(\"WO_CR_ON\",'DD/MON/YYYY')) FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_RECORD_ID\"=\"TR_WO_SLNO\" AND \"WO_BO_ID\"='26')\"TR_COMM_DATE\",";
                strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE cast(\"DIV_CODE\" as text)= SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "')) \"DIVISION\",";
                strQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE cast(\"SD_SUBDIV_CODE\" as text)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "')) AS \"SUBDIVISION\",";
                strQry += " (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE cast(\"OM_CODE\" as text)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Section + "')) AS \"SECTION\",";
                strQry += " (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\") \"MAKE\",TO_CHAR( \"EST_CRON\",'DD-MON-YYYY')\"EST_CRON\",";
                strQry += "  \"WO_AMT\" AS \"EST_UNIT_PRICE\",\"WO_AMT\",\"TR_INVENTORY_QTY\",\"TR_DECOM_INV_QTY\",CAST(\"EST_NO\" AS TEXT)\"EST_NO\", ";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE cast(\"US_OFFICE_CODE\" as text)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "') AND \"US_ROLE_ID\"='1' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SDO_USERNAME\",";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=cast(\"DF_LOC_CODE\" as text) AND \"US_ROLE_ID\"='4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SO_USERNAME\" ";
                strQry += " FROM \"TBLTCMASTER\",\"TBLWORKORDER\",\"TBLINDENT\",\"TBLDTCINVOICE\",\"TBLTCREPLACE\",\"TBLDTCFAILURE\",\"TBLDTCMAST\",\"TBLESTIMATIONDETAILS\" WHERE \"DF_ID\"=\"EST_FAILUREID\" AND  ";
                strQry += " \"TR_IN_NO\"=\"IN_NO\" AND \"IN_TI_NO\"=\"TI_ID\" AND \"TI_WO_SLNO\"=\"WO_SLNO\" AND \"DF_ID\"=\"WO_DF_ID\"  AND \"DF_EQUIPMENT_ID\" = \"TC_CODE\" AND \"DT_CODE\" = \"DF_DTC_CODE\" AND \"DF_ID\"='" + objReport.sFailId + "'";

                dtCompleteReport = ObjCon.FetchDataTable(strQry);
                return dtCompleteReport;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtCompleteReport;
            }
        }





        public DataTable IndentDetails(string strIndentId)
        {
            DataTable dtIndentDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                //strQry = "select TI_INDENT_NO,TO_CHAR(TI_INDENT_DATE,'dd/MM/yyyy')TI_INDENT_DATE,TO_CHAR(TC_CAPACITY) TC_CAPACITY,TO_CHAR(WO_DATE,'dd/MM/yyyy') WO_DATE,";
                //strQry += " (select SM_NAME from TBLSTOREMAST WHERE SM_ID=TI_STORE_ID) SM_NAME,EST_UNIT_PRICE,";
                //strQry += " (SELECT WO_NO from TBLWORKORDER WHERE WO_SLNO=TI_WO_SLNO) WO_NO,TI_DEVICE_ID from TBLTCMASTER,TBLWORKORDER,TBLINDENT,tblestimation where est_DF_ID=WO_DF_ID";
                //strQry += " AND TI_DEVICE_ID=TC_CODE AND TI_WO_SLNO=WO_SLNO and TI_ID='" + strIndentId + "'";

                strQry = "SELECT \"TI_INDENT_NO\",TO_CHAR(\"TI_INDENT_DATE\",'dd/MM/yyyy') \"TI_INDENT_DATE\",CAST(\"TC_CAPACITY\" AS TEXT) \"TC_CAPACITY\",\"WO_NEW_CAP\",";
                strQry += " TO_CHAR(\"WO_DATE\",'dd/MM/yyyy') \"WO_DATE\", (select \"SM_NAME\" from \"TBLSTOREMAST\" WHERE \"SM_ID\"=\"TI_STORE_ID\") \"SM_NAME\",";
                strQry += " (SELECT \"EST_UNIT_PRICE\" FROM \"TBLESTIMATION\" WHERE \"EST_DF_ID\"=\"WO_DF_ID\") \"EST_UNIT_PRICE\",  (SELECT COALESCE(\"EST_TOTALOILVAL\",0) from \"TBLESTIMATIONDETAILS\" WHERE \"EST_FAILUREID\"=\"WO_DF_ID\" )\"EST_TOTALOILVAL\"";
                strQry += " ,(SELECT COALESCE(\"EST_OIL_QNTY\", 0) from \"TBLESTIMATIONDETAILS\" WHERE \"EST_FAILUREID\" = \"WO_DF_ID\"  )\"EST_OIL_QNTY\",\"WO_NO\",\"WO_ACC_CODE\",\"WO_AMT\",";
                strQry += "  \"DF_DTC_CODE\", (SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=\"DF_DTC_CODE\") \"DT_NAME\",";
                strQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "')) AS \"SUBDIVISION\",";
                strQry += "(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE CAST(\"OM_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Section + "')) AS \"SECTION\",";
                strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\"  AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "')) AS \"DIVISION\",";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "') AND CAST(\"US_ROLE_ID\" AS TEXT)='1' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"STO_USERNAME\" ";
                strQry += " ,(SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=CAST(\"DF_LOC_CODE\" AS TEXT) AND \"US_ROLE_ID\"='4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SO_USERNAME\" FROM \"TBLTCMASTER\",";
                strQry += " \"TBLWORKORDER\",\"TBLINDENT\",\"TBLDTCFAILURE\" where  \"TI_WO_SLNO\"=\"WO_SLNO\" and CAST(\"TI_ID\" AS TEXT)='" + strIndentId + "' AND \"DF_ID\"=\"WO_DF_ID\" AND \"TC_CODE\"=\"DF_EQUIPMENT_ID\" ";
                dtIndentDetails = ObjCon.FetchDataTable(strQry);
                return dtIndentDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtIndentDetails;

            }

        }

        public DataTable WO_IndentDetails(string sWOSlno, string sstoreId)
        {
            DataTable dtIndentDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                //strQry = "select TI_INDENT_NO,TO_CHAR(TI_INDENT_DATE,'dd/MM/yyyy')TI_INDENT_DATE,TO_CHAR(TC_CAPACITY) TC_CAPACITY,TO_CHAR(WO_DATE,'dd/MM/yyyy') WO_DATE,";
                //strQry += " (select SM_NAME from TBLSTOREMAST WHERE SM_ID=TI_STORE_ID) SM_NAME,EST_UNIT_PRICE,";
                //strQry += " (SELECT WO_NO from TBLWORKORDER WHERE WO_SLNO=TI_WO_SLNO) WO_NO,TI_DEVICE_ID from TBLTCMASTER,TBLWORKORDER,TBLINDENT,tblestimation where est_DF_ID=WO_DF_ID";
                //strQry += " AND TI_DEVICE_ID=TC_CODE AND TI_WO_SLNO=WO_SLNO and TI_ID='" + strIndentId + "'";

                strQry = "SELECT null as \"TI_INDENT_NO\",TO_CHAR(CURRENT_DATE,'dd/MM/yyyy') \"TI_INDENT_DATE\",CAST(\"TC_CAPACITY\" AS TEXT) \"TC_CAPACITY\",\"WO_NEW_CAP\",";
                strQry += " TO_CHAR(\"WO_DATE\",'dd/MM/yyyy') \"WO_DATE\", (select \"SM_NAME\" from \"TBLSTOREMAST\" WHERE \"SM_ID\"=" + sstoreId + ") \"SM_NAME\",";
                strQry += " (SELECT \"EST_UNIT_PRICE\" FROM \"TBLESTIMATION\" WHERE  \"EST_DF_ID\"=\"WO_DF_ID\") \"EST_UNIT_PRICE\", (SELECT COALESCE(\"EST_TOTALOILVAL\",0) from \"TBLESTIMATIONDETAILS\" WHERE \"EST_FAILUREID\"=\"WO_DF_ID\" )\"EST_TOTALOILVAL\"";
                strQry += " ,(SELECT COALESCE(\"EST_OIL_QNTY\", 0) from \"TBLESTIMATIONDETAILS\" WHERE \"EST_FAILUREID\" = \"WO_DF_ID\"  )\"EST_OIL_QNTY\",\"WO_NO\",\"WO_ACC_CODE\",\"WO_AMT\",";
                strQry += "  \"DF_DTC_CODE\", (SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=\"DF_DTC_CODE\") \"DT_NAME\",";
                strQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "')) AS \"SUBDIVISION\",";
                strQry += "(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE CAST(\"OM_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Section + "')) AS \"SECTION\",";
                strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\"  AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "')) AS \"DIVISION\",";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "') AND CAST(\"US_ROLE_ID\" AS TEXT)='1' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"STO_USERNAME\" ";
                strQry += " ,(SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=CAST(\"DF_LOC_CODE\" AS TEXT) AND \"US_ROLE_ID\"='4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SO_USERNAME\" FROM \"TBLTCMASTER\",";
                strQry += " \"TBLWORKORDER\",\"TBLDTCFAILURE\" where ";
                strQry += "   CAST(\"WO_SLNO\" AS TEXT)='" + sWOSlno + "'";
                strQry += " AND \"DF_ID\"=\"WO_DF_ID\" AND \"TC_CODE\"=\"DF_EQUIPMENT_ID\" ";
                dtIndentDetails = ObjCon.FetchDataTable(strQry);
                return dtIndentDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtIndentDetails;

            }

        }


        public DataTable InvoiceReport(string sInvoiceId, string sOfficeCode, string sCapacity)
        {
            DataTable dtIndentDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                //strQry = "select TI_INDENT_NO,TO_CHAR(TI_INDENT_DATE,'dd/MM/yyyy')TI_INDENT_DATE,TO_CHAR(TC_CAPACITY) TC_CAPACITY,TO_CHAR(WO_DATE,'dd/MM/yyyy') WO_DATE,";
                //strQry += " (select SM_NAME from TBLSTOREMAST WHERE SM_ID=TI_STORE_ID) SM_NAME,EST_UNIT_PRICE,";
                //strQry += " (SELECT WO_NO from TBLWORKORDER WHERE WO_SLNO=TI_WO_SLNO) WO_NO,TI_DEVICE_ID from TBLTCMASTER,TBLWORKORDER,TBLINDENT,tblestimation where est_DF_ID=WO_DF_ID";
                //strQry += " AND TI_DEVICE_ID=TC_CODE AND TI_WO_SLNO=WO_SLNO and TI_ID='" + strIndentId + "'";
                if (sOfficeCode.Length > 2)
                {
                    sOfficeCode = sOfficeCode.Substring(0, Constants.Division);
                }
                strQry = "SELECT \"TI_INDENT_NO\",TO_CHAR(\"TI_INDENT_DATE\",'dd/MM/yyyy') \"TI_INDENT_DATE\",CAST(\"WO_DTC_CAP\" AS TEXT) \"TC_CAPACITY\",\"WO_NEW_CAP\",";
                strQry += " TO_CHAR(\"WO_DATE\",'dd/MM/yyyy') \"WO_DATE\", (select \"SM_NAME\" from \"TBLSTOREMAST\" WHERE \"SM_ID\"=\"TI_STORE_ID\") \"SM_NAME\",";
                strQry += " (SELECT \"EST_UNIT_PRICE\" FROM \"TBLESTIMATION\" WHERE \"EST_DF_ID\"=\"WO_DF_ID\") \"EST_UNIT_PRICE\",";
                strQry += "(SELECT COALESCE(\"EST_TOTALOILVAL\",0) from \"TBLESTIMATIONDETAILS\" WHERE \"EST_FAILUREID\"=\"WO_DF_ID\"  )\"EST_TOTALOILVAL\",(SELECT COALESCE(\"EST_OIL_QNTY\",0) from \"TBLESTIMATIONDETAILS\" WHERE \"EST_FAILUREID\"=\"WO_DF_ID\"  )\"EST_OIL_QNTY\",\"WO_NO\",\"WO_ACC_CODE\",\"WO_AMT\",";
                strQry += "  \"DF_DTC_CODE\", (SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=\"DF_DTC_CODE\") \"DT_NAME\",\"IN_MANUAL_INVNO\",";
                strQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "')) AS \"SUBDIVISION\",";
                strQry += "(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE CAST(\"OM_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Section + "')) AS \"SECTION\",";
                strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "')) AS \"DIVISION\",";
                strQry += " \"IN_INV_NO\",TO_CHAR(\"IN_DATE\",'DD/MM/YYYY') \"IN_DATE\",\"IN_AMT\", ";
                strQry += " (SELECT COUNT(*) FROM \"TBLTCMASTER\" WHERE \"TC_STORE_ID\" IN (SELECT \"SM_ID\" FROM \"TBLSTOREMAST\" WHERE  CAST(\"SM_ID\" AS TEXT)='" + sOfficeCode + "') AND \"TC_STATUS\" IN (1,2) ";
                strQry += " AND \"TC_CURRENT_LOCATION\"=1 AND \"TC_CAPACITY\"='" + sCapacity + "') \"STOCK_COUNT\",";
                strQry += " CAST(\"TC_CODE\" AS TEXT) \"TC_CODE\",CAST(\"TC_SLNO\" AS TEXT) \"TC_SLNO\",";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "') AND CAST(\"US_ROLE_ID\" AS TEXT)='2'  AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"STO_USERNAME\"";
                strQry += " FROM \"TBLTCMASTER\",";
                strQry += " \"TBLWORKORDER\",\"TBLINDENT\",\"TBLDTCFAILURE\",\"TBLDTCINVOICE\",\"TBLTCDRAWN\" where  \"TI_WO_SLNO\"=\"WO_SLNO\" and \"TI_ID\"=\"IN_TI_NO\"";
                strQry += " AND CAST(\"IN_NO\" AS TEXT)='" + sInvoiceId + "' AND \"DF_ID\"=\"WO_DF_ID\" AND \"TD_TC_NO\"=\"TC_CODE\" AND \"TD_INV_NO\"=\"IN_NO\" ";
                dtIndentDetails = ObjCon.FetchDataTable(strQry);
                return dtIndentDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtIndentDetails;
            }

        }

        // WOANo, WOADate, actualoil, oil, barrel, oiltype, issuedate, mtrialf0lio
        public DataTable NewFormReport(string WOANo, string WOADate, string actualoil, string oil, string barrel, string oiltype, string issuedate, string mtrialf0lio)
        {
            DataTable dtReportDetails = new DataTable();
            try
            {
                dtReportDetails.Columns.Add("R_Work_Award_No");
                dtReportDetails.Columns.Add("R_Work_Award_Date");
                dtReportDetails.Columns.Add("R_Actual_Tank");
                dtReportDetails.Columns.Add("R_Num_of_barrel");
                dtReportDetails.Columns.Add("R_Oil_Issued_Date");
                dtReportDetails.Columns.Add("R_Total_Oil");
                dtReportDetails.Columns.Add("R_Oil_Type");
                dtReportDetails.Columns.Add("R_Material_Folio");

                DataRow dRow = dtReportDetails.NewRow();
                dRow["R_Work_Award_No"] = WOANo;
                dRow["R_Work_Award_Date"] = WOADate;
                dRow["R_Actual_Tank"] = actualoil;
                dRow["R_Num_of_barrel"] = barrel;
                dRow["R_Oil_Issued_Date"] = issuedate;
                dRow["R_Total_Oil"] = oil;
                dRow["R_Oil_Type"] = oiltype;
                dRow["R_Material_Folio"] = mtrialf0lio;

                dtReportDetails.Rows.Add(dRow);
                dtReportDetails.AcceptChanges();

                return dtReportDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtReportDetails;
            }

        }
        public DataTable RIReport(string sDecommId)
        {
            DataTable dtRiDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                //strQry = " select DISTINCT TR_RI_NO,TO_CHAR(TR_RI_DATE,'dd/MM/yyyy')TR_RI_DATE, ";
                //strQry+= " TO_CHAR(TC_CAPACITY)TC_CAPACITY,(select SM_NAME from TBLSTOREMAST where SM_ID=TR_STORE_SLNO)SM_NAME,";
                //strQry += " EST_UNIT_PRICE,(SELECT WO_NO FROM TBLWORKORDER WHERE WO_DF_ID=TD_DF_ID) WO_NO,EST_NO,";
                //strQry += "  (SELECT DF_DTC_CODE FROM TBLDTCFAILURE WHERE DF_ID=EST_DF_ID) DF_DTC_CODE,";
                //strQry += " (SELECT DT_NAME FROM TBLDTCMAST,TBLDTCFAILURE WHERE DT_CODE=DF_DTC_CODE AND DF_ID=EST_DF_ID) DTC_NAME ";
                //strQry+= " FROM TBLTCMASTER,";
                //strQry+= " TBLTCREPLACE,TBLTCDRAWN, TBLESTIMATION where TD_TC_NO=TC_CODE AND TR_IN_NO=TD_INV_NO AND ";
                //strQry += " TR_ID='" + sDecommId + "' AND  EST_DF_ID=TD_DF_ID";

                //strQry = "select  (SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='OT' AND \"MD_ID\"=\"DF_OIL_TYPE\")\"DF_OIL_TYPE\",\"TR_RI_NO\",\"TR_NO_OF_BARRELS\",TO_CHAR(\"TR_RI_DATE\",'DD-MON-YYYY')\"TR_RI_DATE\",TO_CHAR(\"DF_DATE\",'DD-MON-YYYY')\"DF_DATE\", ";



                //strQry = "select  (SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='OT' AND \"MD_ID\"=\"DF_OIL_TYPE\")\"DF_OIL_TYPE\",\"TR_RI_NO\",CASE WHEN \"TR_NO_OF_BARRELS\" is null then (cast(\"TR_OIL_QUNTY\" as int)/210)+1 else \"TR_NO_OF_BARRELS\" end,TO_CHAR(\"TR_RI_DATE\",'DD-MON-YYYY')\"TR_RI_DATE\",TO_CHAR(\"DF_DATE\",'DD-MON-YYYY')\"DF_DATE\",  ";
                strQry = "select  (SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='OT' AND \"MD_ID\"=\"DF_OIL_TYPE\")\"DF_OIL_TYPE\",\"TR_RI_NO\",TO_CHAR(\"TR_RI_DATE\",'DD-MON-YYYY')\"TR_RI_DATE\",TO_CHAR(\"DF_DATE\",'DD-MON-YYYY')\"DF_DATE\",  ";
                strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT) = SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "')) \"DIVISION\", ";
                strQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT) =  SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "')) \"SUBDIVISION\",";
                strQry += " (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_CODE\" = \"DF_LOC_CODE\") \"SECTION\",\"TR_MANUAL_ACKRV_NO\",";
                strQry += " (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\" = \"TC_MAKE_ID\") \"MAKE\", \"TC_SLNO\",CAST(\"TC_OIL_CAPACITY\" AS TEXT)\"TC_OIL_CAPACITY\", TO_CHAR(\"TC_MANF_DATE\",'DD-MON-YYYY') AS \"TC_MANF_DATE\",\"TC_CODE\",\"TR_OIL_QUNTY\",";
                strQry += " (SELECT TO_CHAR(\"TM_MAPPING_DATE\",'DD-MON-YYYY') FROM \"TBLTRANSDTCMAPPING\" WHERE \"TM_ID\" = ";
                strQry += " (SELECT MAX(\"TM_ID\") FROM \"TBLTRANSDTCMAPPING\" WHERE \"TM_TC_ID\" = \"TC_CODE\")) AS \"DTRCOMMISIONDATE\",";
                strQry += " CAST(\"TC_CAPACITY\" AS TEXT)\"TC_CAPACITY\", (select \"SM_NAME\" from \"TBLSTOREMAST\" where \"SM_ID\"=\"TR_STORE_SLNO\")\"SM_NAME\",";
                strQry += " (SELECT \"WO_NO_DECOM\" FROM \"TBLWORKORDER\" WHERE \"WO_DF_ID\"=\"DF_ID\") \"WO_NO_DECOM\",(SELECT \"WO_NO_CREDIT\" FROM \"TBLWORKORDER\" WHERE \"WO_DF_ID\"=\"DF_ID\") \"WO_NO_CREDIT\",\"DF_DTC_CODE\",";
                strQry += " (SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=\"DF_DTC_CODE\") \"DTC_NAME\",\"TR_OIL_QTY_BYSK\",\"TR_RV_NO\" \"ACK_NO\", TO_CHAR(\"TR_RV_DATE\",'DD-MON-YYYY') AS \"ACK_DATE\", ";
                strQry += " (SELECT \"EST_ITEM_TOTAL\" FROM \"TBLESTIMATIONDETAILS\" WHERE \"EST_FAILUREID\"=\"DF_ID\") \"EST_UNIT_PRICE\",";
                strQry += " (SELECT \"EST_NO\" FROM \"TBLESTIMATION\" WHERE \"EST_DF_ID\"=\"DF_ID\") \"EST_NO\", ";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "') AND cast(\"US_ROLE_ID\" AS TEXT)='1' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SDO_USERNAME\",";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=CAST(\"DF_LOC_CODE\" AS TEXT) AND CAST(\"US_ROLE_ID\" AS TEXT)='4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SO_USERNAME\", ";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "') AND CAST(\"US_ROLE_ID\" AS TEXT)='2' AND \"US_MMS_ID\" IS NULL AND CAST(\"US_STATUS\" AS TEXT)='A' LIMIT 1) \"STO_USERNAME\",";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "') AND CAST(\"US_ROLE_ID\" AS TEXT)='5' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SK_USERNAME\"";
                strQry += " FROM \"TBLTCMASTER\",\"TBLTCREPLACE\",\"TBLWORKORDER\",\"TBLDTCFAILURE\" WHERE ";
                // strQry += " TD_TC_NO=TC_CODE AND TR_IN_NO=TD_INV_NO AND  TR_ID='"+ sDecommId +"' AND  DF_ID=TD_DF_ID";
                strQry += " \"DF_EQUIPMENT_ID\"=\"TC_CODE\" AND \"WO_SLNO\"=\"TR_WO_SLNO\" AND  CAST(\"TR_WO_SLNO\" AS TEXT)='" + sDecommId + "' AND  \"DF_ID\"=\"WO_DF_ID\"";

                dtRiDetails = ObjCon.FetchDataTable(strQry);
                return dtRiDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtRiDetails;
            }
        }


        public DataTable RIReportso(string sDtrcode, string sFailurId)
        {
            DataTable dtRiDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "select  TO_CHAR(\"DF_DATE\",'DD-MON-YYYY')\"DF_DATE\",'' AS \"TR_RI_NO\", '' AS \"ACK_NO\", ";
                strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT) = SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "')) \"DIVISION\", ";
                strQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT) =  SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "')) \"SUBDIVISION\",";
                strQry += " (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_CODE\" = \"DF_LOC_CODE\") \"SECTION\",";
                strQry += " (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\" = \"TC_MAKE_ID\") \"MAKE\", \"TC_SLNO\", TO_CHAR(\"TC_MANF_DATE\",'DD-MON-YYYY') AS \"TC_MANF_DATE\",\"TC_CODE\",";
                strQry += " (SELECT TO_CHAR(\"TM_MAPPING_DATE\",'DD-MON-YY') FROM \"TBLTRANSDTCMAPPING\" WHERE \"TM_ID\" = ";
                strQry += " (SELECT MAX(\"TM_ID\") FROM \"TBLTRANSDTCMAPPING\" WHERE \"TM_TC_ID\" = \"TC_CODE\")) AS \"DTRCOMMISIONDATE\",";
                strQry += " CAST(\"TC_CAPACITY\" AS TEXT)\"TC_CAPACITY\",CAST(\"TC_OIL_CAPACITY\" AS TEXT)\"TC_OIL_CAPACITY\", (select \"SM_NAME\" from \"TBLSTOREMAST\" where \"SM_ID\"=\"TC_STORE_ID\")\"SM_NAME\",";
                strQry += " (SELECT \"WO_NO_DECOM\" FROM \"TBLWORKORDER\" WHERE \"WO_DF_ID\"=\"DF_ID\") \"WO_NO_DECOM\",(SELECT \"WO_NO_CREDIT\" FROM \"TBLWORKORDER\" WHERE \"WO_DF_ID\"=\"DF_ID\") \"WO_NO_CREDIT\",\"DF_DTC_CODE\",";
                strQry += " (SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=\"DF_DTC_CODE\") \"DTC_NAME\", ";
                strQry += " (SELECT \"EST_ITEM_TOTAL\" FROM \"TBLESTIMATIONDETAILS\" WHERE \"EST_FAILUREID\"=\"DF_ID\") \"EST_UNIT_PRICE\",";
                strQry += " (SELECT \"EST_NO\" FROM \"TBLESTIMATIONDETAILS\" WHERE \"EST_FAILUREID\"=\"DF_ID\") \"EST_NO\", ";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "') AND cast(\"US_ROLE_ID\" AS TEXT)='1' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SDO_USERNAME\",";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=CAST(\"DF_LOC_CODE\" AS TEXT) AND CAST(\"US_ROLE_ID\" AS TEXT)='4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SO_USERNAME\", ";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "') AND CAST(\"US_ROLE_ID\" AS TEXT)='2' AND \"US_MMS_ID\" IS NULL AND CAST(\"US_STATUS\" AS TEXT)='A' LIMIT 1) \"STO_USERNAME\",";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "') AND CAST(\"US_ROLE_ID\" AS TEXT)='5' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SK_USERNAME\"";
                strQry += " FROM \"TBLTCMASTER\",\"TBLWORKORDER\",\"TBLDTCFAILURE\" WHERE ";
                // strQry += " TD_TC_NO=TC_CODE AND TR_IN_NO=TD_INV_NO AND  TR_ID='"+ sDecommId +"' AND  DF_ID=TD_DF_ID";
                strQry += " \"DF_EQUIPMENT_ID\"=\"TC_CODE\"  AND  \"DF_ID\"=\"WO_DF_ID\" and  \"TC_CODE\"='" + sDtrcode + "' and \"DF_ID\"='" + sFailurId + "'";

                dtRiDetails = ObjCon.FetchDataTable(strQry);
                return dtRiDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtRiDetails;
            }
        }
        public DataTable LoadGatePass(string sInvoiceId)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {

                strQry = "SELECT \"GP_ID\", \"GP_VEHICLE_NO\",\"GP_RECIEPIENT_NAME\",\"GP_CHALLEN_NO\",\"IN_INV_NO\",\"TC_SLNO\", TO_CHAR(\"IN_DATE\",'DD-MON-YYYY') \"IN_DATE\" ,";
                strQry += " (select \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\" )\"TM_NAME\",\"TC_CODE\",CAST(\"TC_CAPACITY\" AS TEXT) \"TC_CAPACITY\", ";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1,'" + Constants.Division + "') AND \"US_ROLE_ID\"='2' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' limit 1) \"STO_USERNAME\" ";
                strQry += " FROM ";
                strQry += " \"TBLDTCINVOICE\",\"TBLGATEPASS\",\"TBLTCDRAWN\",\"TBLTCMASTER\" WHERE \"IN_INV_NO\"=\"GP_IN_NO\" AND \"IN_NO\"=\"TD_INV_NO\" AND \"TC_CODE\"=\"TD_TC_NO\" AND \"GP_IN_NO\"='" + sInvoiceId + "'   GROUP BY \"GP_VEHICLE_NO\",\"GP_RECIEPIENT_NAME\",\"GP_CHALLEN_NO\",\"IN_INV_NO\",\"TC_SLNO\",\"IN_DATE\",\"TM_NAME\",\"TC_CODE\",\"TC_CAPACITY\",\"STO_USERNAME\" ,\"GP_ID\"  ORDER BY \"GP_ID\" desc limit 1 ";
                dt = ObjCon.FetchDataTable(strQry);

                //(SELECT SUBSTR(DF_LOC_CODE,1,2) FROM TBLDTCFAILURE WHERE DF_ID=TD_DF_ID) LOC_CODE 

                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        #region cregisterabstractreport
        public DataTable PrintRegAbstact(clsReports objreports)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;

            try
            {
                #region Old CR Report
                //strQry = "SELECT (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) FROM VIEW_ALL_OFFICES ";
                //strQry += " WHERE OFF_CODE=SUBSTR (DF_LOC_CODE,0,1))CIRCLE,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)";
                //strQry += " FROM VIEW_ALL_OFFICES WHERE OFF_CODE=SUBSTR(DF_LOC_CODE,0,2))DIVISION,";
                //strQry += " (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) FROM VIEW_ALL_OFFICES WHERE ";
                //strQry += " OFF_CODE=SUBSTR(DF_LOC_CODE,0,3))SUBDIVISION, '" + objreports.sFromDate + "' as FROMDATE,'" + objreports.sTodate + "' as TODATE,TO_CHAR(SYSDATE,'DD-MON-YYYY') as currentdate, ";
                //strQry += " (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)FROM VIEW_ALL_OFFICES WHERE";
                //strQry += " OFF_CODE=SUBSTR(DF_LOC_CODE,0,4))SECTION,(CASE WHEN TR_INVENTORY_QTY IS NULL THEN 'Crpending' WHEN TR_INVENTORY_QTY IS NOT NULL THEN 'Crcompleted' END)STATUS";
                //strQry += ",EST_NO,TO_CHAR( EST_CRON,'DD-MON-YYYY')EST_CRON ,WO_NO,WO_NO_DECOM ,";
                //strQry += " TO_CHAR( WO_DATE,'DD-MON-YYYY')WO_DATE,TO_CHAR( WO_DATE_DECOM,'DD-MON-YYYY')WO_DATE_DECOM,";
                //strQry += "  TO_CHAR(TC_CAPACITY)TC_CAPACITY,DF_REASON,IN_INV_NO,TO_CHAR( IN_DATE,'DD-MON-YYYY')IN_DATE,CASE WHEN DF_ENHANCE_CAPACITY IS NULL THEN TC_CAPACITY WHEN DF_ENHANCE_CAPACITY IS NOT NULL THEN DF_ENHANCE_CAPACITY END AS REPLACE_CAPACITY  ";
                //strQry += " FROM TBLTCMASTER INNER JOIN TBLDTCFAILURE ON DF_EQUIPMENT_ID=TC_CODE INNER  JOIN  TBLMASTERDATA  on DF_FAILURE_TYPE=MD_ID LEFT JOIN ";
                //strQry += " TBLESTIMATION ON DF_ID=EST_DF_ID LEFT JOIN TBLWORKORDER ON DF_ID=WO_DF_ID LEFT JOIN TBLINDENT";
                //strQry += " ON WO_SLNO=TI_WO_SLNO LEFT JOIN TBLDTCINVOICE ON IN_TI_NO=TI_ID LEFT JOIN ";
                //strQry += " TBLTCREPLACE ON TR_IN_NO=IN_NO WHERE  DF_LOC_CODE LIKE '" + objreports.sOfficeCode + "%' ";
                //strQry += " AND DF_STATUS_FLAG IN (1,4)  AND MD_TYPE='FT' AND  TR_RI_NO IS NOT NULL  AND ";
                //if (objreports.sTodate == null && (objreports.sFromDate != null))
                //{
                //    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objreports.sFromDate + "' and TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";

                //}
                //if (objreports.sFromDate == null && (objreports.sTodate != null))
                //{
                //    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objreports.sTodate + "' ";
                //}
                //if (objreports.sFromDate == null && objreports.sTodate == null)
                //{
                //    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                //}
                //if (objreports.sFromDate != null && objreports.sTodate != null)
                //{
                //    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objreports.sFromDate + "' AND TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objreports.sTodate + "'  ";
                //}

                //strQry += "ORDER BY est_no";
                #endregion

                strQry = "SELECT (SELECT SUBSTR(CAST(\"OFF_NAME\" AS TEXT),POSITION(':' in cast(\"OFF_NAME\" as TEXT))+1) FROM \"VIEW_ALL_OFFICES\"  WHERE CAST(\"OFF_CODE\" AS TEXT)=SUBSTR (CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Circle + "'))\"CIRCLE\",";
                strQry += "(SELECT SUBSTR(CAST(\"OFF_NAME\" AS TEXT),POSITION(':' IN CAST(\"OFF_NAME\" AS TEXT))+1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "'))\"DIVISION\", ";
                strQry += "(SELECT SUBSTR(CAST(\"OFF_NAME\" AS TEXT),POSITION(':' IN CAST(\"OFF_NAME\" AS TEXT))+1) FROM \"VIEW_ALL_OFFICES\" WHERE  CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "'))\"SUBDIVISION\", ";
                strQry += "'" + objreports.sFromDate + "' as \"FROMDATE\",'" + objreports.sTodate + "' as \"TODATE\",TO_CHAR(CURRENT_DATE,'dd-MM-yyyy') as \"currentdate\",";
                strQry += "(SELECT SUBSTR(CAST(\"OFF_NAME\" AS TEXT),POSITION(':' IN CAST(\"OFF_NAME\" AS TEXT))+1)FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Section + "'))\"SECTION\",substr(CAST(\"DF_DTC_CODE\" AS TEXT),1," + Constants.Feeder + ")\"FD_FEEDER_CODE\",(SELECT DISTINCT \"FD_FEEDER_NAME\" FROM ";
                strQry += " \"TBLFEEDERMAST\" WHERE \"FD_FEEDER_CODE\"=\"DT_FDRSLNO\")\"FD_FEEDER_NAME\",(SELECT SUBSTR(CAST(\"OFF_NAME\" AS TEXT),POSITION(':' IN CAST(\"OFF_NAME\" AS TEXT))+1)FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Zone + "'))\"ZONE\",";
                strQry += "(CASE WHEN \"TR_INVENTORY_QTY\" IS NULL THEN 'Crpending' WHEN \"TR_INVENTORY_QTY\" IS NOT NULL THEN 'Crcompleted' END)\"STATUS\",";
                strQry += "\"EST_NO\",TO_CHAR( \"EST_CRON\",'DD-MON-YYYY')\"EST_CRON\" ,\"WO_NO\",\"WO_NO_DECOM\",TO_CHAR( \"WO_DATE\",'DD-MON-YYYY')\"WO_DATE\",TO_CHAR";
                strQry += "( \"WO_DATE_DECOM\",'DD-MON-YYYY')\"WO_DATE_DECOM\",  CAST(\"TC_CAPACITY\" AS TEXT)\"TC_CAPACITY\",\"DF_REASON\",\"IN_INV_NO\",";
                strQry += "TO_CHAR(\"IN_DATE\",'DD-MON-YYYY')\"IN_DATE\",CASE WHEN \"DF_ENHANCE_CAPACITY\" IS NULL THEN \"TC_CAPACITY\" WHEN \"DF_ENHANCE_CAPACITY\" ";
                strQry += "IS NOT NULL THEN \"DF_ENHANCE_CAPACITY\" END AS \"REPLACE_CAPACITY\" FROM \"TBLTCMASTER\",\"TBLDTCFAILURE\",\"TBLMASTERDATA\",\"TBLWORKORDER\",";
                strQry += "\"TBLINDENT\",\"TBLDTCINVOICE\",\"TBLTCREPLACE\",\"TBLDTCMAST\",\"TBLESTIMATIONDETAILS\" where \"DF_ID\"=\"EST_FAILUREID\" and \"DF_EQUIPMENT_ID\"=\"TC_CODE\" and ";
                strQry += "\"DF_DTC_CODE\" =\"DT_CODE\" and \"DF_REPLACE_FLAG\"=1 AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') AND CAST(\"DF_LOC_CODE\" AS TEXT) ";
                strQry += "LIKE '" + objreports.sOfficeCode + "%' AND \"DF_FAILURE_TYPE\"=\"MD_ID\" AND \"DF_ID\"=\"WO_DF_ID\" AND \"WO_SLNO\"=\"TI_WO_SLNO\" AND \"TI_ID\"=\"IN_TI_NO\" ";
                strQry += "AND \"WO_SLNO\"=\"TR_WO_SLNO\" AND \"MD_TYPE\"='FT' AND \"DF_STATUS_FLAG\" IN (1,4) and \"EST_FAIL_TYPE\"=2 and \"WO_SLNO\" is not null";
                if (objreports.sFeeder != null)
                {
                    strQry += "  and  \"DT_FDRSLNO\" ='" + objreports.sFeeder + "'  ";
                }
                if (objreports.sTodate == null && (objreports.sFromDate != null))
                {
                    strQry += " AND TO_CHAR(\"DF_DATE\",'dd-MM-yyyy')>= '" + objreports.sFromDate + "' and TO_CHAR(\"DF_DATE\",'dd-MM-yyyy')<=TO_CHAR(CURRENT_DATE,'dd-MM-yyyy')";

                }
                if (objreports.sFromDate == null && (objreports.sTodate != null))
                {
                    strQry += " AND TO_CHAR(\"DF_DATE\",'dd-MM-yyyy')<='" + objreports.sTodate + "' ";
                }
                if (objreports.sFromDate == null && objreports.sTodate == null)
                {
                    strQry += " AND TO_CHAR(\"DF_DATE\",'dd-MM-yyyy')<=TO_CHAR(CURRENT_DATE,'dd-MM-yyyy') ";
                }
                if (objreports.sFromDate != null && objreports.sTodate != null)
                {
                    strQry += " AND TO_CHAR(\"DF_DATE\",'dd-MM-yyyy')>= '" + objreports.sFromDate + "' AND TO_CHAR(\"DF_DATE\",'dd-MM-yyyy')<='" + objreports.sTodate + "'  ";
                }
                strQry += " UNION ALL ";

                strQry += "SELECT (SELECT SUBSTR(CAST(\"OFF_NAME\" AS TEXT),POSITION(':' IN CAST(\"OFF_NAME\" AS TEXT))+1) FROM \"VIEW_ALL_OFFICES\"  WHERE CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Circle + "'))\"CIRCLE\",";
                strQry += "(SELECT SUBSTR(CAST(\"OFF_NAME\" AS TEXT),POSITION(':' IN CAST(\"OFF_NAME\" AS TEXT))+1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "'))\"DIVISION\", ";
                strQry += "(SELECT SUBSTR(CAST(\"OFF_NAME\" AS TEXT),POSITION(':' IN CAST(\"OFF_NAME\" AS TEXT))+1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "'))\"SUBDIVISION\", ";
                strQry += "'" + objreports.sFromDate + "' as \"FROMDATE\",'" + objreports.sTodate + "' as \"TODATE\",TO_CHAR(CURRENT_DATE,'dd-MM-yyyy') as \"currentdate\",";
                strQry += "(SELECT SUBSTR(CAST(\"OFF_NAME\" AS TEXT),POSITION(':' IN CAST(\"OFF_NAME\" AS TEXT))+1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Section + "'))\"SECTION\",substr(CAST(\"DF_DTC_CODE\" AS TEXT),1," + Constants.Feeder + ")\"FD_FEEDER_CODE\",(SELECT DISTINCT \"FD_FEEDER_NAME\" FROM ";
                strQry += " \"TBLFEEDERMAST\" WHERE \"FD_FEEDER_CODE\"=\"DT_FDRSLNO\")\"FD_FEEDER_NAME\",(SELECT SUBSTR(CAST(\"OFF_NAME\" AS TEXT),POSITION(':' IN CAST(\"OFF_NAME\" AS TEXT))+1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Zone + "'))\"ZONE\",";
                strQry += "(CASE WHEN \"TR_INVENTORY_QTY\" IS NULL THEN 'Crpending' WHEN \"TR_INVENTORY_QTY\" IS NOT NULL THEN 'Crcompleted' END)\"STATUS\",";
                strQry += "\"EST_NO\",TO_CHAR(\"EST_CRON\",'DD-MON-YYYY')\"EST_CRON\",\"WO_NO\",\"WO_NO_DECOM\",TO_CHAR(\"WO_DATE\",'DD-MON-YYYY')\"WO_DATE\",TO_CHAR";
                strQry += "(\"WO_DATE_DECOM\",'DD-MON-YYYY')\"WO_DATE_DECOM\",CAST(\"TC_CAPACITY\" AS TEXT)\"TC_CAPACITY\",\"DF_REASON\",\"IN_INV_NO\",";
                strQry += "TO_CHAR(\"IN_DATE\",'DD-MON-YYYY')\"IN_DATE\",CASE WHEN \"DF_ENHANCE_CAPACITY\" IS NULL THEN \"TC_CAPACITY\" WHEN \"DF_ENHANCE_CAPACITY\" ";
                strQry += "IS NOT NULL THEN \"DF_ENHANCE_CAPACITY\" END AS \"REPLACE_CAPACITY\"  FROM \"TBLDTCMAST\",\"TBLESTIMATIONDETAILS\" ,\"TBLMASTERDATA\",\"TBLTCMASTER\",\"TBLDTCFAILURE\" LEFT JOIN \"TBLWORKORDER\" ON \"DF_ID\"=\"WO_DF_ID\" LEFT JOIN \"TBLINDENT\" ON \"WO_SLNO\"=\"TI_WO_SLNO\" LEFT JOIN \"TBLDTCINVOICE\" ON  \"TI_ID\"=\"IN_TI_NO\" LEFT JOIN \"TBLTCREPLACE\" ON \"WO_SLNO\"=\"TR_WO_SLNO\"";
                strQry += "where \"DF_ID\"=\"EST_FAILUREID\" and \"DF_EQUIPMENT_ID\"=\"TC_CODE\" and ";
                strQry += "\"DF_DTC_CODE\" =\"DT_CODE\" and \"DF_REPLACE_FLAG\"=0 AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') AND CAST(\"DF_LOC_CODE\" AS TEXT) ";
                strQry += "LIKE '" + objreports.sOfficeCode + "%' AND \"DF_FAILURE_TYPE\"=\"MD_ID\"  ";
                strQry += " AND \"MD_TYPE\"='FT' AND \"DF_STATUS_FLAG\" IN (1,4)";
                if (objreports.sFeeder != null)
                {
                    strQry += "  and  \"DT_FDRSLNO\" ='" + objreports.sFeeder + "'  ";
                }
                if (objreports.sTodate == null && (objreports.sFromDate != null))
                {
                    strQry += " AND TO_CHAR(\"DF_DATE\",'dd-MM-yyyy')>= '" + objreports.sFromDate + "' and TO_CHAR(\"DF_DATE\",'dd-MM-yyyy')<=TO_CHAR(CURRENT_DATE,'dd-MM-yyyy')";

                }
                if (objreports.sFromDate == null && (objreports.sTodate != null))
                {
                    strQry += " AND TO_CHAR(\"DF_DATE\",'dd-MM-yyyy')<='" + objreports.sTodate + "' ";
                }
                if (objreports.sFromDate == null && objreports.sTodate == null)
                {
                    strQry += " AND TO_CHAR(\"DF_DATE\",'dd-MM-yyyy')<=TO_CHAR(CURRENT_DATE,'dd-MM-yyyy') ";
                }
                if (objreports.sFromDate != null && objreports.sTodate != null)
                {
                    strQry += " AND TO_CHAR(\"DF_DATE\",'dd-MM-yyyy')>= '" + objreports.sFromDate + "' AND TO_CHAR(\"DF_DATE\",'dd-MM-yyyy')<='" + objreports.sTodate + "'  ";
                }

                dt = ObjCon.FetchDataTable(strQry);
                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }

        }
        #endregion



        //public DataTable PrintRepairGatePassReport(string sInvoiceNo)
        //{

        //    DataTable dt = new DataTable();
        //    string strQry = string.Empty;
        //    try
        //    {
        //        strQry = "Select CAST(\"RSD_TC_CODE\" AS TEXT) AS \"RSD_TC_CODE\",\"RSM_INV_NO\",TO_CHAR(\"RSM_INV_DATE\",'dd/MM/yyyy') AS \"RSM_INV_DATE\",\"RSD_DELIVARY_DATE\",\"GP_VEHICLE_NO\",\"GP_RECIEPIENT_NAME\",\"GP_CHALLEN_NO\",";
        //        strQry += "\"TC_SLNO\",CAST(\"TC_CAPACITY\" AS TEXT) AS \"TC_CAPACITY\",\"TM_NAME\",\"DIV_NAME\",TO_CHAR(\"TC_MANF_DATE\",'dd/MM/yyyy') \"TC_MANF_DATE\",";
        //        strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=\"RSM_DIV_CODE\" AND \"US_ROLE_ID\"='2' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"STO_USERNAME\" ";
        //        strQry += " FROM \"TBLGATEPASS\",\"TBLREPAIRSENTMASTER\",\"TBLREPAIRSENTDETAILS\",\"TBLTCMASTER\",\"TBLTRANSMAKES\",\"TBLDIVISION\"";
        //        strQry += " WHERE \"RSM_DIV_CODE\"=\"DIV_CODE\" and \"TM_ID\"=\"TC_MAKE_ID\" AND \"TC_CODE\"=\"RSD_TC_CODE\" AND \"RSD_RSM_ID\"=\"RSM_ID\" and \"RSM_INV_NO\"=\"GP_IN_NO\" and \"GP_IN_NO\"='" + sInvoiceNo + "' ";
        //        dt = ObjCon.FetchDataTable(strQry);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "PrintRepairGatePassReport");
        //        return dt;

        //    }
        //}

        public DataTable PrintRepairGatePassReport(string sInvoiceNo)
        {

            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                string Gatepass = ObjCon.get_value("select \"GP_ID\" from \"TBLGATEPASS\" where \"GP_IN_NO\"='" + sInvoiceNo + "'");
                if (Gatepass != null && Gatepass != "")
                {
                    //Commented by sandeep on 14-07-2023 bcz in faultytcinvoice creation page gate pass report not getting receipient name
                    //strQry = "Select CAST(\"RSD_TC_CODE\" AS TEXT) AS \"RSD_TC_CODE\",\"RSM_PO_NO\",\"RSM_SUPREP_TYPE\",CASE WHEN \"RSM_SUPREP_TYPE\"=2 THEN (SELECT \"TR_NAME\" FROM \"TBLTRANSREPAIRER\"WHERE \"TR_ID\"=\"RSM_SUPREP_ID\") ELSE  ";
                    //strQry += " (SELECT \"TS_NAME\" FROM \"TBLTRANSSUPPLIER\" WHERE \"TS_ID\" = \"RSM_SUPREP_ID\")END AS \"REPAIRER\" ,(SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"SM_ID\" =\"RSM_DIV_CODE\") as \"STORE\"";
                    //strQry += " ,TO_CHAR(\"RSM_PO_DATE\",'dd/MM/yyyy')\"RSM_PO_DATE\",\"RSM_INV_NO\",TO_CHAR(\"RSM_INV_DATE\",'dd/MM/yyyy') AS \"RSM_INV_DATE\",\"RSD_DELIVARY_DATE\",\"GP_VEHICLE_NO\",\"GP_RECIEPIENT_NAME\",\"GP_CHALLEN_NO\" ,\"RSM_PO_QNTY\" AS \"ISSUEQTY\",";
                    //strQry += "\"TC_SLNO\",CAST(\"TC_CAPACITY\" AS TEXT) AS \"TC_CAPACITY\",\"TM_NAME\",\"DIV_NAME\",TO_CHAR(\"TC_MANF_DATE\",'dd/MM/yyyy') \"TC_MANF_DATE\",";
                    //strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=CAST(\"RSM_DIV_CODE\" AS TEXT) AND \"US_ROLE_ID\"='2' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"STO_USERNAME\" ";
                    //strQry += ",(SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=CAST(\"RSM_DIV_CODE\" AS TEXT) AND \"US_ROLE_ID\"='5' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SK_USERNAME\", \"RWOA_NO\" as \"WORK_AWARD\"";
                    //strQry += " FROM \"TBLGATEPASS\",\"TBLREPAIRSENTMASTER\",\"TBLREPAIRSENTDETAILS\",\"TBLTCMASTER\",\"TBLTRANSMAKES\",\"TBLDIVISION\",\"TBLREPAIRWORKAWARDDETAILS\",\"TBLREPAIRERWORKAWARD\" ,\"TBLREPAIRERESTIMATIONDETAILS\",\"TBLREPAIRERWORKORDER\" ";
                    //strQry += " WHERE \"RSM_NEW_DIV_CODE\"=\"DIV_CODE\" and \"TM_ID\"=\"TC_MAKE_ID\" AND \"TC_CODE\"=\"RSD_TC_CODE\" AND \"RSD_RSM_ID\"=\"RSM_ID\" and \"RSM_INV_NO\"=\"GP_IN_NO\" and \"RWO_SLNO\"=\"RWAD_WO_SLNO\" and \"RWAO_ID\"=\"RWAD_WA_ID\" and  \"RESTD_ID\"=\"RWO_EST_ID\"  and  \"TC_CODE\"=\"RESTD_TC_CODE\" and \"GP_IN_NO\"='" + sInvoiceNo + "' ";
                    //strQry += " GROUP BY \"RSD_TC_CODE\",\"RSM_INV_NO\",\"RSM_INV_DATE\",\"RSM_SUPREP_TYPE\",\"RSD_DELIVARY_DATE\",\"GP_VEHICLE_NO\",\"GP_RECIEPIENT_NAME\",\"GP_CHALLEN_NO\",\"TC_SLNO\", \"TC_CAPACITY\",\"TM_NAME\",\"DIV_NAME\", \"TC_MANF_DATE\", \"STO_USERNAME\",\"WORK_AWARD\",\"SK_USERNAME\" ,\"ISSUEQTY\",\"RSM_PO_NO\",\"RSM_PO_DATE\",\"REPAIRER\" , \"STORE\" ";
                    #region 
                    //strQry = " Select CAST(\"RSD_TC_CODE\" AS TEXT) AS \"RSD_TC_CODE\",\"RI_INVOICE_NO\" as \"RSM_INV_NO\",";
                    //strQry += "    \"RSM_PO_NO\",\"RSM_SUPREP_TYPE\",CASE WHEN \"RSM_SUPREP_TYPE\"=2 THEN (SELECT \"TR_NAME\" ";
                    //strQry += "    FROM \"TBLTRANSREPAIRER\" WHERE \"TR_ID\"=\"RSM_SUPREP_ID\") ELSE (SELECT \"TS_NAME\" FROM ";
                    //strQry += "    \"TBLTRANSSUPPLIER\" WHERE \"TS_ID\" = \"RSM_SUPREP_ID\")END AS \"REPAIRER\" ,";
                    //strQry += "    (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\",\"TBLSTOREOFFCODE\" WHERE \"STO_SM_ID\"=\"SM_ID\" ";
                    //strQry += "   and  \"STO_OFF_CODE\" =\"RSM_DIV_CODE\") as \"STORE\" ,(SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" ";
                    //strQry += "  WHERE \"TM_ID\"=\"TC_MAKE_ID\") AS \"TM_NAME\" ,TO_CHAR(\"RSM_PO_DATE\",'dd/MM/yyyy')\"RSM_PO_DATE\", ";
                    //strQry += "  TO_CHAR(\"RI_INVOICE_DATE\",'dd/MM/yyyy') AS \"RSM_INV_DATE\",\"RSD_DELIVARY_DATE\",";
                    //strQry += "    null as \"GP_VEHICLE_NO\", \"TC_SLNO\",CAST(\"TC_CAPACITY\" AS TEXT) AS \"TC_CAPACITY\",";
                    //strQry += "   \"DIV_NAME\",TO_CHAR(\"TC_MANF_DATE\",'dd/MM/yyyy') \"TC_MANF_DATE\",\"RSM_PO_QNTY\" as \"ISSUEQTY\",";
                    //strQry += "   (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=(select cast(\"DIV_ID\" as text) ";
                    //strQry += "  from \"TBLDIVISION\" where \"DIV_CODE\"=\"RSM_DIV_CODE\") AND \"US_ROLE_ID\"='2' AND \"US_MMS_ID\" IS NULL";
                    //strQry += "   AND \"US_STATUS\"='A' LIMIT 1) \"STO_USERNAME\" , (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\"";
                    //strQry += "   WHERE \"US_OFFICE_CODE\"=(select cast(\"DIV_ID\" as text) from \"TBLDIVISION\" where ";
                    //strQry += "   \"DIV_CODE\"=\"RSM_DIV_CODE\") AND \"US_ROLE_ID\"='5' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) ";
                    //strQry += "  \"SK_USERNAME\",\"GP_RECIEPIENT_NAME\" FROM \"TBLREPAIRSENTMASTER\",\"TBLREPAIRSENTDETAILS\",\"TBLTCMASTER\",";
                    //strQry += "  \"TBLDIVISION\",\"TBLREPAIRERINVOICE\",\"TBLGATEPASS\"  WHERE \"RSM_NEW_DIV_CODE\"=\"DIV_CODE\" ";
                    //strQry += "  AND \"TC_CODE\"=\"RSD_TC_CODE\" and \"RI_RSM_ID\"=\"RSM_ID\" and  \"RSD_RSM_ID\"=\"RSM_ID\"  ";
                    //strQry += "  and \"RI_INVOICE_NO\"='" + sInvoiceNo + "'AND \"GP_IN_NO\"='" + sInvoiceNo + "'   GROUP BY \"RSD_TC_CODE\",";
                    //strQry += "   \"RI_INVOICE_NO\",\"RI_INVOICE_DATE\",\"RSD_DELIVARY_DATE\",\"TC_SLNO\",\"RSM_SUPREP_TYPE\", ";
                    //strQry += "   \"TC_CAPACITY\",\"DIV_NAME\", \"TC_MANF_DATE\", \"STO_USERNAME\",\"SK_USERNAME\" ,\"ISSUEQTY\",";
                    //strQry += "  \"GP_VEHICLE_NO\",\"RSM_PO_NO\",\"RSM_PO_DATE\",\"REPAIRER\" , \"STORE\",\"TM_NAME\",\"GP_RECIEPIENT_NAME\" ";
                    #endregion




                    //made changes by vaishnavi
                    strQry = " Select CAST(\"RSD_TC_CODE\" AS TEXT) AS \"RSD_TC_CODE\",\"RI_INVOICE_NO\" as \"RSM_INV_NO\",";
                    strQry += "    \"RSM_PO_NO\",\"RSM_SUPREP_TYPE\",CASE WHEN \"RSM_SUPREP_TYPE\"=2 THEN (SELECT \"TR_NAME\" ";
                    strQry += "    FROM \"TBLTRANSREPAIRER\" WHERE \"TR_ID\"=\"RSM_SUPREP_ID\") ELSE (SELECT \"TS_NAME\" FROM ";
                    strQry += "    \"TBLTRANSSUPPLIER\" WHERE \"TS_ID\" = \"RSM_SUPREP_ID\")END AS \"REPAIRER\" ,";
                    strQry += "    (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\",\"TBLSTOREOFFCODE\" WHERE \"STO_SM_ID\"=\"SM_ID\" ";
                    strQry += "   and  \"STO_OFF_CODE\" =\"DIV_CODE\") as \"STORE\" ,(SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" ";
                    strQry += "  WHERE \"TM_ID\"=\"TC_MAKE_ID\") AS \"TM_NAME\" ,TO_CHAR(\"RSM_PO_DATE\",'dd/MM/yyyy')\"RSM_PO_DATE\", ";
                    strQry += "  TO_CHAR(\"RI_INVOICE_DATE\",'dd/MM/yyyy') AS \"RSM_INV_DATE\",\"RSD_DELIVARY_DATE\",";
                    strQry += "    null as \"GP_VEHICLE_NO\", \"TC_SLNO\",CAST(\"TC_CAPACITY\" AS TEXT) AS \"TC_CAPACITY\",";
                    strQry += "   \"DIV_NAME\",TO_CHAR(\"TC_MANF_DATE\",'dd/MM/yyyy') \"TC_MANF_DATE\",\"RSM_PO_QNTY\" as \"ISSUEQTY\",";
                    strQry += "   (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\",\"TBLSTOREOFFCODE\" WHERE \"US_OFFICE_CODE\"=(select cast(\"SM_ID\" as text) ";
                    strQry += "  from \"TBLSTOREMAST\" where \"DIV_CODE\"=\"SM_CODE\") AND \"US_ROLE_ID\"='2' AND \"US_MMS_ID\" IS NULL";
                    strQry += "   AND \"US_STATUS\"='A' LIMIT 1) \"STO_USERNAME\" , (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\",\"TBLSTOREOFFCODE\" ";
                    strQry += "   WHERE \"US_OFFICE_CODE\"=(select cast(\"SM_ID\" as text) from \"TBLSTOREMAST\" where ";
                    strQry += "   \"DIV_CODE\"=\"SM_CODE\") AND \"US_ROLE_ID\"='5' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) ";
                    strQry += "  \"SK_USERNAME\",\"GP_RECIEPIENT_NAME\" FROM \"TBLREPAIRSENTMASTER\",\"TBLREPAIRSENTDETAILS\",\"TBLTCMASTER\",";
                    strQry += "  \"TBLDIVISION\",\"TBLREPAIRERINVOICE\",\"TBLGATEPASS\"  WHERE \"RSM_NEW_DIV_CODE\"=\"DIV_CODE\" ";
                    strQry += "  AND \"TC_CODE\"=\"RSD_TC_CODE\" and \"RI_RSM_ID\"=\"RSM_ID\" and  \"RSD_RSM_ID\"=\"RSM_ID\"  ";
                    strQry += "  and \"RI_INVOICE_NO\"='" + sInvoiceNo + "'AND \"GP_IN_NO\"='" + sInvoiceNo + "'   GROUP BY \"RSD_TC_CODE\",";
                    strQry += "   \"RI_INVOICE_NO\",\"RI_INVOICE_DATE\",\"RSD_DELIVARY_DATE\",\"TC_SLNO\",\"RSM_SUPREP_TYPE\", ";
                    strQry += "   \"TC_CAPACITY\",\"DIV_NAME\", \"TC_MANF_DATE\", \"STO_USERNAME\",\"SK_USERNAME\" ,\"ISSUEQTY\",";
                    strQry += "  \"GP_VEHICLE_NO\",\"RSM_PO_NO\",\"RSM_PO_DATE\",\"REPAIRER\" , \"STORE\",\"TM_NAME\",\"GP_RECIEPIENT_NAME\" ";


                    dt = ObjCon.FetchDataTable(strQry);
                }
                else
                {
                    dt = PrintRepairInvoiceReport(sInvoiceNo);
                }

                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }
        }
        public DataTable printRolewisependingCount(string officecode)
        {

            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            string strQry = string.Empty;
            try
            {

                //NpgsqlCommand cmd = new NpgsqlCommand("proc_get_office_codes");
                //cmd.Parameters.AddWithValue("officecode", officecode);
                //dt = ObjCon.FetchDataTable(cmd);



                NpgsqlCommand cmd1 = new NpgsqlCommand("sp_getrolewisependingcount");
                cmd1.Parameters.AddWithValue("sofficecode", officecode);
                cmd1.CommandTimeout = 500;
                dt1 = ObjCon.FetchDataTable(cmd1);

                // dt = ObjCon.FetchDataTable(strQry);

                return dt1;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt1;

            }
        }
        public DataTable PrintRepairInvoiceReport(string sInvoiceNo)
        {

            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                //strQry = "Select CAST(\"RSD_TC_CODE\" AS TEXT) AS \"RSD_TC_CODE\",\"RSM_INV_NO\",\"RSM_PO_NO\",\"RSM_SUPREP_TYPE\",CASE WHEN \"RSM_SUPREP_TYPE\"=2 THEN (SELECT \"TR_NAME\" FROM \"TBLTRANSREPAIRER\"WHERE \"TR_ID\"=\"RSM_SUPREP_ID\") ELSE ";
                //strQry += " (SELECT \"TS_NAME\" FROM \"TBLTRANSSUPPLIER\" WHERE \"TS_ID\" = \"RSM_SUPREP_ID\")END AS \"REPAIRER\" ,(SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"SM_ID\" =\"RSM_DIV_CODE\") as \"STORE\" ";
                //strQry += " ,TO_CHAR(\"RSM_PO_DATE\",'dd/MM/yyyy')\"RSM_PO_DATE\",TO_CHAR(\"RSM_INV_DATE\",'dd/MM/yyyy') AS \"RSM_INV_DATE\",\"RSD_DELIVARY_DATE\", null as \"GP_VEHICLE_NO\", ";
                //strQry += "\"TC_SLNO\",CAST(\"TC_CAPACITY\" AS TEXT) AS \"TC_CAPACITY\",\"TM_NAME\",\"DIV_NAME\",TO_CHAR(\"TC_MANF_DATE\",'dd/MM/yyyy') \"TC_MANF_DATE\",\"RSM_PO_QNTY\" as \"ISSUEQTY\",";
                //strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=CAST(\"RSM_DIV_CODE\" AS TEXT) AND \"US_ROLE_ID\"='2' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"STO_USERNAME\" ";
                //strQry += ",(SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=CAST(\"RSM_DIV_CODE\" AS TEXT) AND \"US_ROLE_ID\"='5' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SK_USERNAME\", \"RWOA_NO\" as \"WORK_AWARD\"";
                //strQry += " FROM \"TBLREPAIRSENTMASTER\",\"TBLREPAIRSENTDETAILS\",\"TBLTCMASTER\",\"TBLTRANSMAKES\",\"TBLDIVISION\",\"TBLREPAIRWORKAWARDDETAILS\",\"TBLREPAIRERWORKAWARD\" ,\"TBLREPAIRERESTIMATIONDETAILS\",\"TBLREPAIRERWORKORDER\" ";
                //strQry += " WHERE \"RSM_NEW_DIV_CODE\"=\"DIV_CODE\" and \"TM_ID\"=\"TC_MAKE_ID\" AND \"TC_CODE\"=\"RSD_TC_CODE\" AND \"RSD_RSM_ID\"=\"RSM_ID\"  and \"RWO_SLNO\"=\"RWAD_WO_SLNO\" and \"RWAO_ID\"=\"RWAD_WA_ID\" and  \"RESTD_ID\"=\"RWO_EST_ID\"  and  \"TC_CODE\"=\"RESTD_TC_CODE\" and \"RSM_INV_NO\"='" + sInvoiceNo + "' ";
                //strQry += " GROUP BY \"RSD_TC_CODE\",\"RSM_INV_NO\",\"RSM_INV_DATE\",\"RSD_DELIVARY_DATE\",\"TC_SLNO\",\"RSM_SUPREP_TYPE\", \"TC_CAPACITY\",\"TM_NAME\",\"DIV_NAME\", \"TC_MANF_DATE\", \"STO_USERNAME\",\"WORK_AWARD\",\"SK_USERNAME\" ,\"ISSUEQTY\",\"GP_VEHICLE_NO\",\"RSM_PO_NO\",\"RSM_PO_DATE\",\"REPAIRER\" , \"STORE\" ";


                //strQry = "Select CAST(\"RSD_TC_CODE\" AS TEXT) AS \"RSD_TC_CODE\",\"RSM_INV_NO\",\"RSM_PO_NO\",\"RSM_SUPREP_TYPE\",CASE WHEN \"RSM_SUPREP_TYPE\"=2 THEN (SELECT \"TR_NAME\" FROM \"TBLTRANSREPAIRER\" WHERE \"TR_ID\"=\"RSM_SUPREP_ID\") ELSE  ";
                //strQry += " (SELECT \"TS_NAME\" FROM \"TBLTRANSSUPPLIER\" WHERE \"TS_ID\" = \"RSM_SUPREP_ID\")END AS \"REPAIRER\" ,(SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"SM_ID\" =\"RSM_DIV_CODE\") as \"STORE\" ,(SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\") AS \"TM_NAME\" ,TO_CHAR(\"RSM_PO_DATE\",'dd/MM/yyyy')\"RSM_PO_DATE\", ";
                //strQry += " TO_CHAR(\"RSM_INV_DATE\",'dd/MM/yyyy') AS \"RSM_INV_DATE\",\"RSD_DELIVARY_DATE\", null as \"GP_VEHICLE_NO\", \"TC_SLNO\",CAST(\"TC_CAPACITY\" AS TEXT) AS \"TC_CAPACITY\",\"DIV_NAME\",TO_CHAR(\"TC_MANF_DATE\",'dd/MM/yyyy') \"TC_MANF_DATE\", ";
                //strQry += " \"RSM_PO_QNTY\" as \"ISSUEQTY\", (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=CAST(\"RSM_DIV_CODE\" AS TEXT) AND \"US_ROLE_ID\"='2' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"STO_USERNAME\" ,";
                //strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=CAST(\"RSM_DIV_CODE\" AS TEXT) AND \"US_ROLE_ID\"='5' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SK_USERNAME\" FROM \"TBLREPAIRSENTMASTER\",\"TBLREPAIRSENTDETAILS\", ";
                //strQry += " \"TBLTCMASTER\",\"TBLDIVISION\"  WHERE \"RSM_NEW_DIV_CODE\"=\"DIV_CODE\" AND \"TC_CODE\"=\"RSD_TC_CODE\" AND \"RSD_RSM_ID\"=\"RSM_ID\"  and \"RSM_INV_NO\"='" + sInvoiceNo + "' ";
                //strQry += " GROUP BY \"RSD_TC_CODE\",\"RSM_INV_NO\",\"RSM_INV_DATE\",\"RSD_DELIVARY_DATE\",\"TC_SLNO\",\"RSM_SUPREP_TYPE\", \"TC_CAPACITY\",\"DIV_NAME\", \"TC_MANF_DATE\", \"STO_USERNAME\",\"SK_USERNAME\" ,\"ISSUEQTY\",\"GP_VEHICLE_NO\",\"RSM_PO_NO\",\"RSM_PO_DATE\",\"REPAIRER\" , \"STORE\",\"TM_NAME\" ";
                strQry = "Select CAST(\"RSD_TC_CODE\" AS TEXT) AS \"RSD_TC_CODE\",\"RI_INVOICE_NO\" as \"RSM_INV_NO\",\"RSM_PO_NO\",\"RSM_SUPREP_TYPE\",CASE WHEN \"RSM_SUPREP_TYPE\"=2 THEN (SELECT \"TR_NAME\" FROM \"TBLTRANSREPAIRER\" WHERE \"TR_ID\"=\"RSM_SUPREP_ID\") ELSE (SELECT \"TS_NAME\" FROM \"TBLTRANSSUPPLIER\" WHERE \"TS_ID\" = \"RSM_SUPREP_ID\")END AS \"REPAIRER\" ,(SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\",\"TBLSTOREOFFCODE\" WHERE \"STO_SM_ID\"=\"SM_ID\" and  \"STO_OFF_CODE\" =\"RSM_DIV_CODE\") as \"STORE\" ,(SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\") AS \"TM_NAME\" ,TO_CHAR(\"RSM_PO_DATE\",'dd/MM/yyyy')\"RSM_PO_DATE\",  TO_CHAR(\"RI_INVOICE_DATE\",'dd/MM/yyyy') AS \"RSM_INV_DATE\",\"RSD_DELIVARY_DATE\", null as \"GP_VEHICLE_NO\", \"TC_SLNO\",CAST(\"TC_CAPACITY\" AS TEXT) AS \"TC_CAPACITY\",\"DIV_NAME\",TO_CHAR(\"TC_MANF_DATE\",'dd/MM/yyyy') \"TC_MANF_DATE\",\"RSM_PO_QNTY\" as \"ISSUEQTY\", (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=(select cast(\"DIV_ID\" as text) from \"TBLDIVISION\" where \"DIV_CODE\"=\"RSM_DIV_CODE\") AND \"US_ROLE_ID\"='2' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"STO_USERNAME\" , (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=(select cast(\"DIV_ID\" as text) from \"TBLDIVISION\" where \"DIV_CODE\"=\"RSM_DIV_CODE\") AND \"US_ROLE_ID\"='5' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SK_USERNAME\" FROM \"TBLREPAIRSENTMASTER\",\"TBLREPAIRSENTDETAILS\",\"TBLTCMASTER\",\"TBLDIVISION\",\"TBLREPAIRERINVOICE\"  WHERE \"RSM_NEW_DIV_CODE\"=\"DIV_CODE\" AND \"TC_CODE\"=\"RSD_TC_CODE\" and \"RI_RSM_ID\"=\"RSM_ID\" and  \"RSD_RSM_ID\"=\"RSM_ID\"  and \"RI_INVOICE_NO\"='" + sInvoiceNo + "'  GROUP BY \"RSD_TC_CODE\",\"RI_INVOICE_NO\",\"RI_INVOICE_DATE\",\"RSD_DELIVARY_DATE\",\"TC_SLNO\",\"RSM_SUPREP_TYPE\", \"TC_CAPACITY\",\"DIV_NAME\", \"TC_MANF_DATE\", \"STO_USERNAME\",\"SK_USERNAME\" ,\"ISSUEQTY\",\"GP_VEHICLE_NO\",\"RSM_PO_NO\",\"RSM_PO_DATE\",\"REPAIRER\" , \"STORE\",\"TM_NAME\" ";



                dt = ObjCon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }
        }


        public DataTable PrintScrapGatePass(string sInvoiceNo)
        {

            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {

                strQry = " select \"TC_SLNO\",(select \"TM_NAME\" from \"TBLTRANSMAKES\" where \"TM_ID\"=\"TC_MAKE_ID\") \"TM_NAME\",CAST(\"TC_CODE\") as \"RSD_TC_CODE\",\"DIV_NAME\",";
                strQry += " CAST(\"TC_CAPACITY\" AS TEXT) \"TC_CAPACITY\",TO_CHAR(\"TC_MANF_DATE\",'dd/MM/yyyy') \"TC_MANF_DATE\",\"SO_INV_NO\" as \"RSM_INV_NO\",\"GP_VEHICLE_NO\",";
                strQry += " \"GP_CHALLEN_NO\",\"GP_RECIEPIENT_NAME\",To_char(\"SO_INV_DATE\",'dd/MM/yyyy') as \"RSM_INV_DATE\",";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=\"ST_DIV_CODE\" AND \"US_ROLE_ID\"='2' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"STO_USERNAME\" ";
                strQry += " FROM \"TBLSCRAPTC\",\"TBLSCRAPOBJECT\",\"TBLTCMASTER\",\"TBLGATEPASS\",\"TBLDIVISION\"";
                strQry += " where \"SO_ST_ID\"=\"ST_ID\" and \"TC_CODE\"=\"SO_TC_CODE\" and \"SO_INV_NO\"=\"GP_IN_NO\" and \"ST_DIV_CODE\"=\"DIV_CODE\" and \"GP_IN_NO\"='" + sInvoiceNo + "' ";

                dt = ObjCon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }
        }


        public DataTable PrintStoreInvoiceGatePass(string sInvoiceNo)
        {

            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                //strQry = " SELECT DISTINCT \"TC_SLNO\",(SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\") AS \"TM_NAME\",CAST(\"TC_CODE\" AS TEXT) AS \"RSD_TC_CODE\", ";
                //strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\",\"TBLSTOREMAST\" WHERE \"DIV_CODE\"=\"SM_OFF_CODE\" AND \"SM_ID\"=\"SI_TO_STORE\") \"DIV_NAME\",";
                //strQry += " CAST(\"TC_CAPACITY\" AS TEXT) \"TC_CAPACITY\",TO_CHAR(\"TC_MANF_DATE\",'DD/MM/YYYY') AS \"TC_MANF_DATE\",\"IS_NO\" AS \"RSM_INV_NO\",\"GP_VEHICLE_NO\",\"GP_CHALLEN_NO\",\"GP_ISSUE_QTY\" as \"ISSUEQTY\" ,";
                //strQry += " \"GP_RECIEPIENT_NAME\",TO_CHAR(\"IS_DATE\",'DD/MM/YYYY') AS \"RSM_INV_DATE\",\"IS_NO\",";
                //strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=CAST(\"ST_DIV_CODE\" as TEXT) AND \"US_ROLE_ID\"='5' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"STO_USERNAME\",";
                //strQry += " \"GP_IN_NO\" FROM \"TBLSTOREINVOICE\",\"TBLSINVOICEOBJECTS\",\"TBLTCMASTER\",\"TBLGATEPASS\",\"TBLSTOREINDENT\",\"TBLSCRAPTC\" WHERE ";
                //strQry += " \"IO_IS_ID\"=\"IS_ID\" AND \"TC_CODE\"=\"IO_TCCODE\" AND \"IS_NO\"=\"GP_IN_NO\" AND \"IS_SI_ID\"=\"SI_ID\" AND \"GP_IN_NO\"='" + sInvoiceNo + "'";

                strQry = " SELECT DISTINCT \"TC_SLNO\",(SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\" = \"TC_MAKE_ID\") AS \"TM_NAME\",CAST(\"TC_CODE\" AS TEXT) AS \"TC_CODE\",  (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\",\"TBLSTOREMAST\" WHERE \"DIV_CODE\" = \"SM_CODE\" ";
                strQry += " AND \"SM_ID\" = \"SI_TO_STORE\") \"DIV_NAME\", CAST(\"TC_CAPACITY\" AS TEXT) \"TC_CAPACITY\",TO_CHAR(\"TC_MANF_DATE\", 'DD/MM/YYYY') AS \"TC_MANF_DATE\",\"IS_NO\" AS \"IN_INV_NO\",\"GP_VEHICLE_NO\",\"GP_CHALLEN_NO\",\"GP_ISSUE_QTY\" as \"ISSUEQTY\" , ";
                strQry += " \"GP_RECIEPIENT_NAME\",TO_CHAR(\"IS_DATE\", 'DD/MM/YYYY') AS \"IN_DATE\",\"IS_NO\",(SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE  \"US_ROLE_ID\" = '5' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\" = 'A' LIMIT 1) \"STO_USERNAME\",   \"GP_IN_NO\" FROM ";
                strQry += " \"TBLSTOREINVOICE\"  INNER JOIN \"TBLSINVOICEOBJECTS\" ON \"IO_IS_ID\" = \"IS_ID\"  INNER JOIN \"TBLGATEPASS\" ON  \"IS_NO\" = \"GP_IN_NO\" INNER JOIN \"TBLTCMASTER\"  ON \"TC_CODE\" = \"IO_TCCODE\" INNER JOIN \"TBLSTOREINDENT\" ON \"IS_SI_ID\" = \"SI_ID\" ";
                strQry += "  WHERE \"GP_IN_NO\" = '" + sInvoiceNo + "' ";


                dt = ObjCon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }


        public DataTable PrintScrapInvoicereport(string sScrapId)
        {

            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "select \"ST_OM_NO\" \"ST_WO_NO\",To_CHAR(\"ST_OM_DATE\",'dd/MM/yyyy') AS \"ST_WO_DATE\",CAST(\"TC_CODE\" AS TEXT) \"TC_CODE\",\"SO_INV_NO\",To_char(\"SO_INV_DATE\",'dd/MM/yyyy') AS \"SO_INV_DATE\",";
                strQry += " (select \"SM_NAME\" from \"TBLSTOREMAST\" where \"SM_ID\"=\"TC_STORE_ID\") AS \"SM_NAME\",";
                strQry += " (select \"TM_NAME\" from \"TBLTRANSMAKES\" where \"TC_MAKE_ID\"=\"TM_ID\") \"TC_MAKE\",\"TC_SLNO\",CAST(\"TC_CAPACITY\" AS TEXT) AS \"TC_CAPACITY\",";
                strQry += " TO_CHAR(\"TC_MANF_DATE\",'dd/MM/yyyy') \"TC_MANF_DATE\", ";
                strQry += " (SELECT COUNT(*) FROM \"TBLTCMASTER\" WHERE \"TC_STORE_ID\" = \"ST_DIV_CODE\" AND \"TC_STATUS\" IN (1,2) ";
                strQry += "  AND \"TC_CURRENT_LOCATION\"=1 ) \"STOCK_COUNT\",";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=CAST(\"ST_DIV_CODE\" AS TEXT) AND \"US_ROLE_ID\"='2' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A') \"STO_USERNAME\" ";
                strQry += " from \"TBLSCRAPOBJECT\",\"TBLSCRAPTC\",\"TBLTCMASTER\" where \"ST_ID\"=\"SO_ST_ID\" and \"TC_CODE\"=\"SO_TC_CODE\" ";
                strQry += " AND \"ST_ID\"='" + sScrapId + "'";
                dt = ObjCon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }
        }


        public DataTable PrintInterStoreInvoicereport(string sInvoiceNo)
        {

            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                //strQry = "SELECT CAST(\"TC_CODE\" AS TEXT) AS \"TC_CODE\",\"TC_SLNO\",(SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\") AS \"TM_NAME\",";
                //strQry += " CAST(\"TC_CAPACITY\" AS TEXT) \"TC_CAPACITY\", ";
                //strQry += " TO_CHAR(\"TC_MANF_DATE\",'DD/MM/YYYY') \"MANF_DATE\",TO_CHAR(\"IS_DATE\",'DD/MM/YYYY') AS \"INV_DATE\",\"IS_NO\" AS \"INV_NO\",\"SI_NO\" AS \"INDENT_NO\",TO_CHAR(\"SI_DATE\",'DD/MM/YYYY') \"INDENT_DATE\",";
                //strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\",\"TBLSTOREMAST\" WHERE \"DIV_CODE\"=\"SM_OFF_CODE\" AND \"SM_ID\"=\"SI_TO_STORE\") \"DIV_NAME\",";
                //strQry += " (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"SM_ID\"=\"SI_TO_STORE\") \"SM_NAME\",";
                //strQry += " (SELECT COUNT(*) FROM \"TBLTCMASTER\" WHERE \"TC_STORE_ID\" IN ";
                //strQry += " (SELECT \"SM_ID\" FROM \"TBLSTOREMAST\" WHERE  \"SM_ID\"=\"SI_TO_STORE\") AND \"TC_STATUS\" IN (1,2) AND \"TC_CURRENT_LOCATION\"=1 ) \"STOCK_COUNT\",";
                //strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1,2) AND \"US_ROLE_ID\"='2' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' ) \"STO_USERNAME\" ";
                //strQry += " FROM \"TBLSTOREINVOICE\",\"TBLSINVOICEOBJECTS\",\"TBLTCMASTER\",\"TBLSTOREINDENT\" WHERE ";
                //strQry += " \"IO_IS_ID\"=\"IS_ID\" AND \"TC_CODE\"=\"IO_TCCODE\" AND \"IS_SI_ID\"=\"SI_ID\" AND \"IS_NO\"='" + sInvoiceNo + "'";

                strQry = "SELECT distinct CAST(\"TC_CODE\" AS TEXT) AS \"TC_CODE\",\"TC_SLNO\",(SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\") AS \"TM_NAME\", CAST(\"TC_CAPACITY\" AS TEXT) \"TC_CAPACITY\",";
                strQry += " TO_CHAR(\"TC_MANF_DATE\",'DD/MM/YYYY') \"MANF_DATE\",TO_CHAR(\"IS_DATE\",'DD/MM/YYYY') AS \"INV_DATE\",\"IS_NO\" AS \"INV_NO\",\"SI_NO\" AS \"INDENT_NO\",TO_CHAR(\"SI_DATE\",'DD/MM/YYYY') \"INDENT_DATE\",";
                strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\",\"TBLSTOREMAST\" WHERE \"DIV_CODE\"=\"SM_OFF_CODE\" AND \"SM_ID\"=\"SI_TO_STORE\") \"DIV_NAME\", (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"SM_ID\"=\"SI_TO_STORE\") \"SM_NAME\",";
                strQry += " (SELECT COUNT(*) FROM \"TBLTCMASTER\" WHERE \"TC_STORE_ID\" IN  (SELECT \"SM_ID\" FROM \"TBLSTOREMAST\" WHERE  \"SM_ID\"=\"SI_TO_STORE\") AND \"TC_STATUS\" IN (1,2,11) AND \"TC_CURRENT_LOCATION\"=1 ) \"STOCK_COUNT\",";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1,2) AND \"US_ROLE_ID\"='2' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' limit 1) \"STO_USERNAME\", SUM(\"SO_QNTY\") AS \"REQ_QNTY\" ";
                strQry += " FROM \"TBLSTOREINVOICE\",\"TBLSINVOICEOBJECTS\",\"TBLTCMASTER\",\"TBLSTOREINDENT\",\"TBLSINDENTOBJECTS\" WHERE \"SI_ID\"=\"SO_SI_ID\" and  \"IO_IS_ID\"=\"IS_ID\" AND \"TC_CODE\"=\"IO_TCCODE\" AND \"IS_SI_ID\"=\"SI_ID\"";
                strQry += " AND \"IS_NO\"='" + sInvoiceNo + "' ";
                strQry += "GROUP BY \"TC_CODE\",\"IS_DATE\",\"IS_NO\",\"SI_NO\",\"SI_DATE\",\"SI_TO_STORE\" ";
                dt = ObjCon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }
        }

        public DataTable PrintReceiveDTrReport(string sInvoiceId)
        {

            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = " SELECT distinct \"IS_NO\",TO_CHAR(\"IS_DATE\",'DD-MON-YYYY') AS \"IS_DATE\",TO_CHAR(\"SI_DATE\",'DD-MON-YYYY') \"SI_DATE\",";
                strQry += " (select \"SM_NAME\" from \"TBLSTOREMAST\" where \"SM_ID\"=\"SI_FROM_STORE\") AS \"SI_FROM_STORE\",";
                strQry += " (select \"SM_NAME\" from \"TBLSTOREMAST\" where \"SM_ID\"= \"SI_TO_STORE\") AS \"SI_TO_STORE\",";
                strQry += " \"SI_NO\",CAST(\"TC_CODE\" AS TEXT) as \"TC_CODE\",CAST(\"TC_SLNO\" AS TEXT ) as \"TC_SLNO\",CAST(\"TC_CAPACITY\" AS TEXT) AS \"TC_CAPACITY\",(SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\"";
                strQry += " WHERE \"TM_ID\"=\"TC_MAKE_ID\") as \"TM_NAME\",\"TC_MANF_DATE\", ";
                strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\",\"TBLSTOREMAST\" WHERE \"DIV_CODE\"=\"SM_OFF_CODE\" AND \"SM_ID\"=\"SI_TO_STORE\") \"DIV_NAME\",\"IS_RV_NO\", ";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=CAST(\"TC_LOCATION_ID\" AS TEXT) AND \"US_ROLE_ID\"='2' ";
                strQry += " AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' limit 1) AS \"STO_USERNAME\",CASE WHEN \"TC_STATUS\"='1' THEN 'BRAND NEW' WHEN \"TC_STATUS\"='2' THEN 'REPAIR GOOD' WHEN \"TC_STATUS\"='3' THEN 'FAULTY' WHEN \"TC_STATUS\"='11' THEN 'RELEASE GOOD' END \"STATUS\"  ";
                strQry += " FROM \"TBLSTOREINVOICE\",\"TBLSTOREINDENT\",\"TBLSINVOICEOBJECTS\",\"TBLSTOREMAST\",\"TBLTCMASTER\" WHERE \"IS_SI_ID\"=\"SI_ID\"  AND ";
                strQry += " \"IO_IS_ID\"=\"IS_ID\" and \"SM_ID\"=\"SI_FROM_STORE\" AND \"IO_TCCODE\"=\"TC_CODE\" and \"IS_ID\" = '" + sInvoiceId + "'";
                dt = ObjCon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }
        }

        public DataTable PrintStoreIndentReport(string sIndentId)
        {

            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "select \"SI_NO\",To_char(\"SI_DATE\",'dd/MM/yyyy') AS \"SI_DATE\",( select \"SM_NAME\" from \"TBLSTOREMAST\" where \"SM_ID\"=\"SI_FROM_STORE\") AS \"SI_FROM_STORE\",CAST(\"SO_CAPACITY\" AS TEXT) AS \"SO_CAPACITY\",CAST(\"SO_QNTY\" as TEXT) AS \"SO_QNTY\",";
                strQry += " ( select \"SM_NAME\" from \"TBLSTOREMAST\" where \"SM_ID\"=\"SI_TO_STORE\") AS \"SI_TO_STORE\",";
                strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\",\"TBLSTOREMAST\" WHERE \"DIV_CODE\"=\"SM_OFF_CODE\" AND \"SM_ID\"=\"SI_FROM_STORE\") \"DIV_NAME\",";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\",\"TBLSTOREMAST\" WHERE \"US_OFFICE_CODE\"=cast(\"SM_OFF_CODE\" as text) AND \"US_ROLE_ID\"='2' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' AND \"SM_ID\"=\"SI_FROM_STORE\"  LIMIT 1) \"STO_USERNAME\" ";
                strQry += "  from \"TBLSINDENTOBJECTS\",\"TBLSTOREINDENT\" where \"SI_ID\"=\"SO_SI_ID\" and \"SI_ID\"='" + sIndentId + "'";
                dt = ObjCon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }
        }

        public DataTable PrintDTrReport(clsReports objValue)
        {

            DataTable dt = new DataTable();
            string strcode = string.Empty;
            string strQry = string.Empty;
            try
            {

                strQry = "select CAST(\"TC_CODE\" AS TEXT) \"TC_CODE\",\"TC_SLNO\",(select \"TM_NAME\" from \"TBLTRANSMAKES\" where \"TC_MAKE_ID\"=\"TM_ID\") AS \"TC_MAKE_ID\",";
                strQry += " CAST(\"TC_CAPACITY\" AS TEXT) AS \"TC_CAPACITY\",To_char(\"TC_MANF_DATE\",'dd-MON-yyyy') AS \"TC_MANF_DATE\",";
                strQry += " (select \"MD_NAME\" from \"TBLMASTERDATA\" where \"MD_ID\"=\"TC_CURRENT_LOCATION\" and \"MD_TYPE\"='TCL') \"LOCATIONNAME\" , ";
                if (objValue.sLocType == "2")
                {
                    if (objValue.sOfficeCode.Length >= Constants.Circle)
                    {
                        strcode = objValue.sOfficeCode.Substring(0, Constants.Circle);
                        strQry += "  (select \"CM_CIRCLE_NAME\" from \"TBLCIRCLE\" WHERE CAST(\"CM_CIRCLE_CODE\" AS TEXT)=SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1," + Constants.Circle + ")) AS \"CIRCLE\",";
                    }
                    else
                    {
                        strQry += " '' AS \"CIRCLE\",";
                    }
                    if (objValue.sOfficeCode.Length >= Constants.Division)
                    {
                        strcode = objValue.sOfficeCode.Substring(0, Constants.Division);
                        strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\"AS TEXT)=SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1," + Constants.Division + ")) as \"DIVISION\" ,";
                    }
                    else
                    {
                        strQry += " '' AS \"DIVISION\",";
                    }
                    if (objValue.sOfficeCode.Length >= Constants.SubDivision)
                    {
                        strcode = objValue.sOfficeCode.Substring(0, Constants.SubDivision);
                        strQry += "(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT)=SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1," + Constants.SubDivision + ")) as \"SUBDIVISION\", ";
                    }
                    else
                    {
                        strQry += " '' AS \"SUBDIVISION\",";
                    }
                    if (objValue.sOfficeCode.Length >= Constants.Section)
                    {
                        strcode = objValue.sOfficeCode.Substring(0, Constants.Section);
                        strQry += "(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where CAST(\"OM_CODE\" AS TEXT)=SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1," + Constants.Section + ")) as \"SECTION\",";
                    }
                    else
                    {
                        strQry += " '' AS \"SECTION\",";
                    }
                    strQry = "select CAST(\"TC_CODE\" AS TEXT) \"TC_CODE\",\"TC_SLNO\",(select \"TM_NAME\" from \"TBLTRANSMAKES\" where \"TC_MAKE_ID\"=\"TM_ID\") AS \"TC_MAKE_ID\",";
                    strQry += " CAST(\"TC_CAPACITY\" AS TEXT) AS \"TC_CAPACITY\",To_char(\"TC_MANF_DATE\",'dd-MON-yyyy') AS \"TC_MANF_DATE\",";
                    strQry += " (select \"MD_NAME\" from \"TBLMASTERDATA\" where \"MD_ID\"=\"TC_CURRENT_LOCATION\" and \"MD_TYPE\"='TCL') \"LOCATIONNAME\" , ";
                    if (objValue.sLocType == "2")
                    {
                        if (objValue.sOfficeCode.Length >= Constants.Circle)
                        {
                            strcode = objValue.sOfficeCode.Substring(0, Constants.Circle);
                            strQry += "  (select \"CM_CIRCLE_NAME\" from \"TBLCIRCLE\" WHERE CAST(\"CM_CIRCLE_CODE\" AS TEXT)=SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1," + Constants.Circle + ")) AS \"CIRCLE\",";
                        }
                        else
                        {
                            //strQry += " '' AS \"CIRCLE\",";
                            strQry += "  (select \"CM_CIRCLE_NAME\" from \"TBLCIRCLE\" WHERE CAST(\"CM_CIRCLE_CODE\" AS TEXT)=SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1," + Constants.Circle + ")) AS \"CIRCLE\",";

                        }
                        if (objValue.sOfficeCode.Length >= Constants.Division)
                        {
                            strcode = objValue.sOfficeCode.Substring(0, Constants.Division);
                            strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\"AS TEXT)=SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1," + Constants.Division + ")) as \"DIVISION\" ,";
                        }
                        else
                        {
                            // strQry += " '' AS \"DIVISION\",";
                            strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\"AS TEXT)=SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1," + Constants.Division + ")) as \"DIVISION\" ,";

                        }
                        if (objValue.sOfficeCode.Length >= Constants.SubDivision)
                        {
                            strcode = objValue.sOfficeCode.Substring(0, Constants.SubDivision);
                            strQry += "(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT)=SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1," + Constants.SubDivision + ")) as \"SUBDIVISION\", ";
                        }
                        else
                        {
                            //strQry += " '' AS \"SUBDIVISION\",";
                            strQry += "(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT)=SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1," + Constants.SubDivision + ")) as \"SUBDIVISION\", ";

                        }
                        if (objValue.sOfficeCode.Length >= Constants.Section)
                        {
                            strcode = objValue.sOfficeCode.Substring(0, Constants.Section);
                            strQry += "(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where CAST(\"OM_CODE\" AS TEXT)=SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1," + Constants.Section + ")) as \"SECTION\",";
                        }
                        else
                        {
                            //strQry += " '' AS \"SECTION\",";
                            strQry += "(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where CAST(\"OM_CODE\" AS TEXT)=SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1," + Constants.Section + ")) as \"SECTION\",";

                        }


                        //strQry = "select CAST(\"TC_CODE\" AS TEXT) \"TC_CODE\",\"TC_SLNO\",(select \"TM_NAME\" from \"TBLTRANSMAKES\" where \"TC_MAKE_ID\"=\"TM_ID\") AS \"TC_MAKE_ID\",";
                        //strQry += " CAST(\"TC_CAPACITY\" AS TEXT) AS \"TC_CAPACITY\",To_char(\"TC_MANF_DATE\",'dd-MON-yyyy') AS \"TC_MANF_DATE\",";
                        //strQry += " (select \"MD_NAME\" from \"TBLMASTERDATA\" where \"MD_ID\"=\"TC_CURRENT_LOCATION\" and \"MD_TYPE\"='TCL') \"LOCATIONNAME\" , ";
                        //if (objValue.sLocType == "2")
                        //{
                        //    if (objValue.sOfficeCode.Length >= Constants.Circle)
                        //    {
                        //        strcode = objValue.sOfficeCode.Substring(0, Constants.Circle);
                        //        strQry += "  (select \"CM_CIRCLE_NAME\" from \"TBLCIRCLE\" WHERE CAST(\"CM_CIRCLE_CODE\" AS TEXT)=SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1," + Constants.Circle + ")) AS \"CIRCLE\",";
                        //    }
                        //    else
                        //    {
                        //        strQry += " '' AS \"CIRCLE\",";
                        //    }
                        //    if (objValue.sOfficeCode.Length >= Constants.Division)
                        //    {
                        //        strcode = objValue.sOfficeCode.Substring(0, Constants.Division);
                        //        strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\"AS TEXT)=SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1," + Constants.Division + ")) as \"DIVISION\" ,";
                        //    }
                        //    else
                        //    {
                        //        strQry += " '' AS \"DIVISION\",";
                        //    }
                        //    if (objValue.sOfficeCode.Length >= Constants.SubDivision)
                        //    {
                        //        strcode = objValue.sOfficeCode.Substring(0, Constants.SubDivision);
                        //        strQry += "(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT)=SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1," + Constants.SubDivision + ")) as \"SUBDIVISION\", ";
                        //    }
                        //    else
                        //    {
                        //        strQry += " '' AS \"SUBDIVISION\",";
                        //    }
                        //    if (objValue.sOfficeCode.Length >= Constants.Section)
                        //    {
                        //        strcode = objValue.sOfficeCode.Substring(0, Constants.Section);
                        //        strQry += "(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where CAST(\"OM_CODE\" AS TEXT)=SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1," + Constants.Section + ")) as \"SECTION\",";
                        //    }
                        //    else
                        //    {
                        //        strQry += " '' AS \"SECTION\",";
                        //    }


                        if (objValue.sOfficeCode.Length >= Constants.Zone)
                        {
                            strcode = objValue.sOfficeCode.Substring(0, Constants.Zone);
                            strQry += " (SELECT \"ZO_NAME\" FROM \"TBLZONE\" WHERE CAST(\"ZO_ID\"AS TEXT)=SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1," + Constants.Zone + ")) as \"ZONE\",";
                        }
                        else
                        {
                            strQry += " '' AS \"ZONE\",";
                        }

                        if (objValue.sOfficeCode.Length >= Constants.Zone)
                        {
                            strcode = objValue.sOfficeCode.Substring(0, Constants.Zone);
                            strQry += " (SELECT \"ZO_NAME\" FROM \"TBLZONE\" WHERE CAST(\"ZO_ID\"AS TEXT)=SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1," + Constants.Zone + ")) as \"ZONE\",";
                        }
                        else
                        {
                            //strQry += " '' AS \"ZONE\",";
                            strQry += " (SELECT \"ZO_NAME\" FROM \"TBLZONE\" WHERE CAST(\"ZO_ID\"AS TEXT)=SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1," + Constants.Zone + ")) as \"ZONE\",";

                        }

                        //    if (objValue.sFeeder != null)
                        //    {
                        //        // strQry += "( SELECT DISTINCT \"FD_FEEDER_NAME\"  AS \"FEEDER\" FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" and CAST(\"FD_FEEDER_CODE\"AS TEXT) LIKE '" + objValue.sFeeder + "%')  as \"FEEDER\",";
                        //        strQry += "( SELECT DISTINCT \"FD_FEEDER_NAME\"  AS \"FEEDER\" FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\",\"TBLDTCMAST\" where \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" AND \"DT_FDRSLNO\"=\"FD_FEEDER_CODE\" and CAST(\"FD_FEEDER_CODE\"AS TEXT) LIKE '" + objValue.sFeeder + "%' LIMIT 1)  as \"FEEDER\",";
                        //    }
                        //    else
                        //    {
                        //        if (objValue.creportflag != "0")
                        //        {
                        //            strQry += " '' AS \"FEEDER\",";
                        //        }
                        //        else
                        //            strQry += "( SELECT DISTINCT \"FD_FEEDER_NAME\"  AS \"FEEDER\" FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\",\"TBLDTCMAST\" where \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" AND \"DT_FDRSLNO\"=\"FD_FEEDER_CODE\" and CAST(\"FD_FEEDER_CODE\"AS TEXT) LIKE '" + objValue.sFeeder + "%' LIMIT 1)  as \"FEEDER\",";
                        //    }
                        strQry += "'' AS \"STORE_NAME\" ";
                    }
                    else
                    {
                        if (objValue.sOfficeCode == "" || objValue.sOfficeCode == null)
                        {
                            strQry += " '' AS \"CIRCLE\",'' AS \"DIVISION\",'' AS \"SUBDIVISION\",'' AS \"SECTION\",'' AS \"ZONE\",'' AS \"FEEDER\",";
                            if (objValue.creportflag == "0")
                            {
                                strQry += " (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"SM_ID\"=\"TC_LOCATION_ID\") AS \"STORE_NAME\" ";
                            }
                            else
                                strQry += " '' AS \"STORE_NAME\" ";
                        }
                        else
                        {
                            if (objValue.sOfficeCode.Length >= Constants.Circle)
                            {
                                strcode = objValue.sOfficeCode.Substring(0, Constants.Circle);
                                strQry += " (SELECT \"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_CIRCLE_CODE\"=" + strcode + ") AS \"CIRCLE\",";
                            }
                            else
                            {
                                strQry += " '' AS \"CIRCLE\",";
                            }
                            if (objValue.sOfficeCode.Length >= Constants.Zone)
                            {
                                strcode = objValue.sOfficeCode.Substring(0, Constants.Zone);
                                strQry += " (SELECT \"ZO_NAME\" FROM \"TBLZONE\" WHERE \"ZO_ID\"=" + strcode + ") AS \"ZONE\",";
                            }
                            else
                            {
                                strQry += " '' AS \"ZONE\",";
                            }
                            if (objValue.sOfficeCode.Length >= Constants.Division)
                            {
                                strcode = objValue.sOfficeCode.Substring(0, Constants.Division);
                                strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE \"DIV_CODE\"=" + strcode + ") AS \"DIVISION\",";
                            }
                            else
                            {
                                strQry += " '' AS \"DIVISION\",";
                            }
                            strQry += " '' AS \"SUBDIVISION\",'' AS \"SECTION\",'' AS \"FEEDER\",";
                            strQry += " (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE ";
                            //if (objValue.sOfficeCode.Length > 2)//IF NOT DIV
                            //{
                            strQry += " cast(\"SM_ID\" as text)='" + clsStoreOffice.GetStoreID(objValue.sOfficeCode) + "') AS \"STORE_NAME\" ";
                            //}
                            //else
                            //{
                            //    strQry += " cast(\"SM_CODE\" as text) LIKE '" + objValue.sOfficeCode + "%') AS \"STORE_NAME\" ";
                            //}
                        }
                    }
                    if (objValue.sLocType == "1")
                    {
                        if (objValue.sOfficeCode != "")
                        {
                            strQry += " FROM \"TBLTCMASTER\" WHERE  CAST(\"TC_LOCATION_ID\" AS TEXT) = '" + clsStoreOffice.GetStoreIDs(objValue.sOfficeCode) + "' AND \"TC_CODE\" <> '0' and \"TC_CAPACITY\"<>0 ";
                        }
                        else
                        {
                            strQry += " FROM \"TBLTCMASTER\" WHERE  \"TC_CODE\" <> '0' and \"TC_CAPACITY\"<>0 ";
                        }
                    }
                    else if (objValue.sLocType == "5" || objValue.sLocType == "3")
                    {
                        if (objValue.sOfficeCode != "")
                        {
                            strQry += "'' AS \"STORE_NAME\" ";
                        }
                        else
                        {
                            if (objValue.sOfficeCode == "" || objValue.sOfficeCode == null)
                            {
                                //strQry += " '' AS \"CIRCLE\",'' AS \"DIVISION\",'' AS \"SUBDIVISION\",'' AS \"SECTION\",'' AS \"ZONE\",'' AS \"FEEDER\",";


                                if (objValue.sOfficeCode.Length >= Constants.Circle)
                                {
                                    strcode = objValue.sOfficeCode.Substring(0, Constants.Circle);
                                    strQry += "  (select \"CM_CIRCLE_NAME\" from \"TBLCIRCLE\" WHERE CAST(\"CM_CIRCLE_CODE\" AS TEXT)=SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1," + Constants.Circle + ")) AS \"CIRCLE\",";
                                }
                                else
                                {
                                    //strQry += " '' AS \"CIRCLE\",";
                                    strQry += "  (select \"CM_CIRCLE_NAME\" from \"TBLCIRCLE\" WHERE CAST(\"CM_CIRCLE_CODE\" AS TEXT)=(select SUBSTR(CAST(\"SM_CODE\" AS TEXT),1," + Constants.Circle + ") FROM \"TBLSTOREMAST\" WHERE CAST(\"SM_ID\" AS TEXT) =CAST(\"TC_LOCATION_ID\" AS TEXT))) AS \"CIRCLE\",";

                                }
                                if (objValue.sOfficeCode.Length >= Constants.Division)
                                {
                                    strcode = objValue.sOfficeCode.Substring(0, Constants.Division);
                                    strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\"AS TEXT)=SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1," + Constants.Division + ")) as \"DIVISION\" ,";
                                }
                                else
                                {
                                    // strQry += " '' AS \"DIVISION\",";
                                    strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\"AS TEXT)=(select SUBSTR(CAST(\"SM_CODE\" AS TEXT),1," + Constants.Division + ") FROM \"TBLSTOREMAST\" WHERE CAST(\"SM_ID\" AS TEXT) =CAST(\"TC_LOCATION_ID\" AS TEXT))) as \"DIVISION\" ,";

                                }

                                // strQry += " '' AS \"SUBDIVISION\",'' AS \"SECTION\",'' AS \"ZONE\",'' AS \"FEEDER\",";

                                strQry += " '' AS \"SUBDIVISION\",'' AS \"SECTION\",";

                                if (objValue.sOfficeCode.Length >= Constants.Zone)
                                {
                                    strcode = objValue.sOfficeCode.Substring(0, Constants.Zone);
                                    strQry += " (SELECT \"ZO_NAME\" FROM \"TBLZONE\" WHERE CAST(\"ZO_ID\"AS TEXT)=SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1," + Constants.Zone + ")) as \"ZONE\",";
                                }
                                else
                                {
                                    //strQry += " '' AS \"ZONE\",";
                                    strQry += " (SELECT \"ZO_NAME\" FROM \"TBLZONE\" WHERE CAST(\"ZO_ID\" AS TEXT)= (select SUBSTR(CAST(\"SM_CODE\" AS TEXT),1," + Constants.Zone + ") FROM \"TBLSTOREMAST\" WHERE CAST(\"SM_ID\" AS TEXT) =CAST(\"TC_LOCATION_ID\" AS TEXT)) ) as \"ZONE\",";

                                }

                                strQry += " '' AS \"FEEDER\",";
                                if (objValue.creportflag == "0")
                                {
                                    strQry += " (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"SM_ID\"=\"TC_LOCATION_ID\") AS \"STORE_NAME\" ";
                                }
                                else
                                    strQry += " '' AS \"STORE_NAME\" ";
                            }
                            else
                            {
                                if (objValue.sOfficeCode.Length >= Constants.Circle)
                                {
                                    strcode = objValue.sOfficeCode.Substring(0, Constants.Circle);
                                    strQry += " (SELECT \"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_CIRCLE_CODE\"=" + strcode + ") AS \"CIRCLE\",";
                                }
                                else
                                {
                                    //strQry += " '' AS \"CIRCLE\",";
                                    strQry += " (SELECT \"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_CIRCLE_CODE\"=SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1," + Constants.Circle + ")) AS \"CIRCLE\",";

                                }
                                if (objValue.sOfficeCode.Length >= Constants.Zone)
                                {
                                    strcode = objValue.sOfficeCode.Substring(0, Constants.Zone);
                                    strQry += " (SELECT \"ZO_NAME\" FROM \"TBLZONE\" WHERE \"ZO_ID\"=" + strcode + ") AS \"ZONE\",";
                                }
                                else
                                {
                                    //strQry += " '' AS \"ZONE\",";
                                    strQry += " (SELECT \"ZO_NAME\" FROM \"TBLZONE\" WHERE CAST(\"ZO_ID\"AS TEXT)=SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1," + Constants.Zone + ")) as \"ZONE\",";

                                }
                                if (objValue.sOfficeCode.Length >= Constants.Division)
                                {
                                    strcode = objValue.sOfficeCode.Substring(0, Constants.Division);
                                    strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE \"DIV_CODE\"=" + strcode + ") AS \"DIVISION\",";
                                }
                                else
                                {
                                    //strQry += " '' AS \"DIVISION\",";
                                    strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE \"DIV_CODE\"=SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1," + Constants.Division + ")) AS \"DIVISION\",";

                                }
                                strQry += " '' AS \"SUBDIVISION\",'' AS \"SECTION\",'' AS \"FEEDER\",";
                                strQry += " (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE ";
                                //if (objValue.sOfficeCode.Length > 2)//IF NOT DIV
                                //{
                                strQry += " cast(\"SM_ID\" as text)='" + clsStoreOffice.GetStoreID(objValue.sOfficeCode) + "') AS \"STORE_NAME\" ";
                                //}
                                //else
                                //{
                                //    strQry += " cast(\"SM_CODE\" as text) LIKE '" + objValue.sOfficeCode + "%') AS \"STORE_NAME\" ";
                                //}
                            }
                        }
                        if (objValue.sLocType == "1")
                        {
                            if (objValue.sOfficeCode != "")
                            {
                                strQry += " FROM \"TBLTCMASTER\" WHERE  CAST(\"TC_LOCATION_ID\" AS TEXT) = '" + clsStoreOffice.GetStoreIDs(objValue.sOfficeCode) + "' AND \"TC_CODE\" <> '0' and \"TC_CAPACITY\"<>0 ";
                            }
                            else
                            {
                                strQry += " FROM \"TBLTCMASTER\" WHERE  \"TC_CODE\" <> '0' and \"TC_CAPACITY\"<>0 ";
                            }
                        }
                        else if (objValue.sLocType == "5" || objValue.sLocType == "3")
                        {
                            if (objValue.sOfficeCode != "")
                            {





                                //    if (objValue.sOfficeCode.Length >= Constants.Zone)
                                //    {
                                //        strcode = objValue.sOfficeCode.Substring(0, Constants.Zone);
                                //        strQry += " (SELECT \"ZO_NAME\" FROM \"TBLZONE\" WHERE CAST(\"ZO_ID\"AS TEXT)=SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1," + Constants.Zone + ")) as \"ZONE\",";
                                //    }
                                //    else
                                //    {
                                //        strQry += " '' AS \"ZONE\",";
                                //    }

                                //    strQry += "'' AS \"STORE_NAME\" ";
                                //}
                                //else
                                //{
                                //    if (objValue.sOfficeCode == "" || objValue.sOfficeCode == null)
                                //    {
                                //        strQry += " '' AS \"CIRCLE\",'' AS \"DIVISION\",'' AS \"SUBDIVISION\",'' AS \"SECTION\",'' AS \"ZONE\",'' AS \"FEEDER\",";
                                //        if (objValue.creportflag == "0")
                                //        {
                                //            strQry += " (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"SM_ID\"=\"TC_LOCATION_ID\") AS \"STORE_NAME\" ";
                                //        }
                                //        else
                                //            strQry += " '' AS \"STORE_NAME\" ";
                                //    }
                                //    else
                                //    {
                                //        if (objValue.sOfficeCode.Length >= Constants.Circle)
                                //        {
                                //            strcode = objValue.sOfficeCode.Substring(0, Constants.Circle);
                                //            strQry += " (SELECT \"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_CIRCLE_CODE\"=" + strcode + ") AS \"CIRCLE\",";
                                //        }
                                //        else
                                //        {
                                //            strQry += " '' AS \"CIRCLE\",";
                                //        }
                                //        if (objValue.sOfficeCode.Length >= Constants.Zone)
                                //        {
                                //            strcode = objValue.sOfficeCode.Substring(0, Constants.Zone);
                                //            strQry += " (SELECT \"ZO_NAME\" FROM \"TBLZONE\" WHERE \"ZO_ID\"=" + strcode + ") AS \"ZONE\",";
                                //        }
                                //        else
                                //        {
                                //            strQry += " '' AS \"ZONE\",";
                                //        }
                                //        if (objValue.sOfficeCode.Length >= Constants.Division)
                                //        {
                                //            strcode = objValue.sOfficeCode.Substring(0, Constants.Division);
                                //            strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE \"DIV_CODE\"=" + strcode + ") AS \"DIVISION\",";
                                //        }
                                //        else
                                //        {
                                //            strQry += " '' AS \"DIVISION\",";
                                //        }
                                //        strQry += " '' AS \"SUBDIVISION\",'' AS \"SECTION\",'' AS \"FEEDER\",";
                                //        strQry += " (SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE ";
                                //        //if (objValue.sOfficeCode.Length > 2)//IF NOT DIV
                                //        //{
                                //        strQry += " cast(\"SM_ID\" as text)='" + clsStoreOffice.GetStoreID(objValue.sOfficeCode) + "') AS \"STORE_NAME\" ";
                                //        //}
                                //        //else
                                //        //{
                                //        //    strQry += " cast(\"SM_CODE\" as text) LIKE '" + objValue.sOfficeCode + "%') AS \"STORE_NAME\" ";
                                //        //}
                                //    }
                                //}
                                //if (objValue.sLocType == "1")
                                //{
                                //    if (objValue.sOfficeCode != "")
                                //    {
                                //        strQry += " FROM \"TBLTCMASTER\" WHERE  CAST(\"TC_LOCATION_ID\" AS TEXT) = '" + clsStoreOffice.GetStoreIDs(objValue.sOfficeCode) + "' AND \"TC_CODE\" <> '0' and \"TC_CAPACITY\"<>0 ";
                                //    }
                                //    else
                                //    {
                                //        strQry += " FROM \"TBLTCMASTER\" WHERE  \"TC_CODE\" <> '0' and \"TC_CAPACITY\"<>0 ";
                                //    }
                                //}
                                //else if (objValue.sLocType == "5" || objValue.sLocType == "3")
                                //{
                                //    if (objValue.sOfficeCode != "")
                                //    {

                                //        //if (objValue.sOfficeCode.Length > 2)// IF  NOT DIV
                                //        //{
                                //        strQry += " FROM \"TBLTCMASTER\" WHERE  CAST(\"TC_LOCATION_ID\" AS TEXT) = '" + clsStoreOffice.GetStoreIDs(objValue.sOfficeCode) + "' AND \"TC_CODE\" <> '0' and \"TC_CAPACITY\"<>0 ";
                                //        //}
                                //        //else
                                //        //{
                                //        //    strQry += " FROM \"TBLTCMASTER\" WHERE  CAST(\"TC_LOCATION_ID\" AS TEXT) LIKE '" + objValue.sOfficeCode + "%' AND \"TC_CODE\" <> '0' and \"TC_CAPACITY\"<>0 ";
                                //        //}
                                //    }
                                //    else
                                //    {
                                //        strQry += " FROM \"TBLTCMASTER\" WHERE \"TC_CODE\" <> '0' and \"TC_CAPACITY\"<>0 ";
                                //    }
                                //}
                                //else
                                //{
                                //    if (objValue.sFeeder == null)
                                //    {
                                //        strQry += " FROM \"TBLTCMASTER\" WHERE  CAST(\"TC_LOCATION_ID\" AS TEXT) like '" + objValue.sOfficeCode + "%' AND \"TC_CODE\" <> '0' and \"TC_CAPACITY\"<>0 ";
                                //    }
                                //    else
                                //    {
                                //        strQry += " FROM \"TBLTCMASTER\",\"TBLDTCMAST\" WHERE  \"TC_CODE\"=\"DT_TC_ID\"  ";
                                //        if (objValue.sFeeder != "")
                                //        {
                                //            strQry += " AND \"DT_FDRSLNO\" = '" + objValue.sFeeder + "'  ";
                                //        }

                                //        strQry += " AND  CAST(\"TC_LOCATION_ID\" AS TEXT) like '" + objValue.sOfficeCode + "%' AND \"TC_CODE\" <> '0' and \"TC_CAPACITY\"<>0 ";
                                //    }
                                //}
                                //if (objValue.sCapacity != null)
                                //{
                                //    string capacity = ObjCon.get_value("SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='C' AND \"MD_ORDER_BY\"='" + objValue.sCapacity + "'");
                                //    strQry += "  AND  \"TC_CAPACITY\"='" + capacity + "'";
                                //}
                                //if (objValue.sMake != null)
                                //{
                                //    strQry += " AND  \"TC_MAKE_ID\"='" + objValue.sMake + "' ";
                                //}
                                //if (objValue.sLocType != null)
                                //{
                                //    strQry += " AND \"TC_CURRENT_LOCATION\"='" + objValue.sLocType + "' ";
                                //}
                                //strQry += " ORDER BY \"TC_CODE\"";

                                //dt = ObjCon.FetchDataTable(strQry);
                                //return dt;
                                #region  Converted to sp
                                string store_id = clsStoreOffice.GetStoreIDs(objValue.sOfficeCode);
                                NpgsqlCommand cmd = new NpgsqlCommand("fetch_dtr_details_report_clsreport");
                                cmd.Parameters.AddWithValue("p_offcode", Convert.ToString(objValue.sOfficeCode));
                                cmd.Parameters.AddWithValue("p_circle_length", Convert.ToInt32(Constants.Circle));
                                cmd.Parameters.AddWithValue("p_division_length", Convert.ToInt32(Constants.Division));
                                cmd.Parameters.AddWithValue("p_subdivision_length", Convert.ToInt32(Constants.SubDivision));
                                cmd.Parameters.AddWithValue("p_section_length", Convert.ToInt32(Constants.Section));
                                cmd.Parameters.AddWithValue("p_feeder_code", Convert.ToString(objValue.sFeeder) ?? "");
                                cmd.Parameters.AddWithValue("p_cr_reportflag", Convert.ToString(objValue.creportflag) ?? "");
                                cmd.Parameters.AddWithValue("p_zone_length", Convert.ToInt32(Constants.Zone));
                                cmd.Parameters.AddWithValue("p_store_id", store_id);
                                cmd.Parameters.AddWithValue("p_location_type", Convert.ToString(objValue.sLocType));
                                cmd.Parameters.AddWithValue("p_capacity", Convert.ToString(objValue.sCapacity) ?? "");
                                cmd.Parameters.AddWithValue("p_makeid", Convert.ToString(objValue.sMake) ?? "");

                                dt = ObjCon.FetchDataTable(cmd);
                                return dt;
                                #endregion
                            }
                        }
                    }
                }
            }
            



            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }
            return dt;
        }

        public DataTable PrintDTCCReport(clsReports objValue)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                //strQry = "select \"DT_CODE\",\"DT_NAME\", (select \"TM_NAME\"  from \"TBLTRANSMAKES\"  where \"TC_MAKE_ID\"=\"TM_ID\") AS \"TC_MAKE_ID\",";
                //strQry += " CAST(\"TC_CODE\" AS TEXT) AS \"TC_CODE\", CAST(\"TC_CAPACITY\" AS TEXT) \"TC_CAPACITY\",";
                //strQry += " (select \"CM_CIRCLE_NAME\" from \"TBLCIRCLE\" WHERE CAST(\"CM_CIRCLE_CODE\" AS TEXT)=SUBSTR(CAST(\"DT_OM_SLNO\" AS TEXT),1," + Constants.Circle + ")) AS \"CIRCLE\",";
                //strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT)=SUBSTR(CAST(\"DT_OM_SLNO\" AS TEXT),1," + Constants.Division + ")) as \"DIVISION\" ,";
                //strQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT)=SUBSTR(CAST(\"DT_OM_SLNO\" AS TEXT),1," + Constants.SubDivision + ")) as \"SUBDIVISION\", ";
                //strQry += " (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where CAST(\"OM_CODE\" AS TEXT)=SUBSTR(CAST(\"DT_OM_SLNO\" AS TEXT),1," + Constants.Section + ")) as \"SECTION\",";
                ////strQry += " (SELECT \"ZO_NAME\" FROM \"TBLZONE\" WHERE CAST(\"ZO_ID\"AS TEXT)=SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1," + Constants.Zone + ")) as \"ZONE\",\"FD_FEEDER_NAME\",\"FD_FEEDER_CODE\",\"TC_SLNO\" ";
                //strQry += " (SELECT \"ZO_NAME\" FROM \"TBLZONE\" WHERE CAST(\"ZO_ID\"AS TEXT)=SUBSTR(CAST(\"DT_OM_SLNO\" AS TEXT),1," + Constants.Zone + ")) as \"ZONE\",\"FD_FEEDER_NAME\",\"FD_FEEDER_CODE\",\"TC_SLNO\" ";
                //strQry += " FROM  \"TBLDTCMAST\",\"TBLTCMASTER\",\"TBLFEEDERMAST\" WHERE \"TC_CODE\"=\"DT_TC_ID\" AND \"FD_FEEDER_CODE\"=\"DT_FDRSLNO\" ";
                //strQry += " and CAST(\"DT_OM_SLNO\"  AS TEXT) like '" + objValue.sOfficeCode + "%' ";
                ////strQry += "  AND \"DT_TC_ID\" <>'0' ";
                ////strQry += " and \"TC_CAPACITY\"<>0 ";
                //if (objValue.sFeeder != null && objValue.sFeeder != "")
                //{
                //    strQry += "  and  \"DT_FDRSLNO\" ='" + objValue.sFeeder + "'  ";
                //}
                //if (objValue.sSchemeType != null)
                //{
                //    strQry += " and  \"DT_PROJECTTYPE\" ='" + objValue.sSchemeType + "' ";
                //}
                //if (objValue.sCapacity != null)
                //{
                //    string capacity = ObjCon.get_value("SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='C' and \"MD_ORDER_BY\"='" + objValue.sCapacity + "'");
                //    strQry += " AND \"TC_CAPACITY\" ='" + capacity + "'";
                //}
                //strQry += "ORDER BY \"DT_CODE\" ";

                //dt = ObjCon.FetchDataTable(strQry);
                //return dt;
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_dtc_details_clsreport");
                cmd.Parameters.AddWithValue("p_offcode", Convert.ToString(objValue.sOfficeCode ?? ""));
                cmd.Parameters.AddWithValue("p_feeder_code", Convert.ToString(objValue.sFeeder ?? ""));
                cmd.Parameters.AddWithValue("p_schematype", Convert.ToString(objValue.sSchemeType ?? ""));
                cmd.Parameters.AddWithValue("p_capacity", Convert.ToString(objValue.sCapacity ?? ""));

                dt = ObjCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }
        }


        public DataTable PrintAbstractReport(clsReports objReport)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                #region Commented Inline query
                //strQry = "  SELECT A.\"TC_CAPACITY\",A.\"ZONE\",A.\"CIRCLE\",A.\"DIV\",A.\"SUBDIV\",A.\"SECTION\",A.\"TC_COUNT\" AS \"ONEWEEKCOUNT\",B.\"TC_COUNT\" ";
                //strQry += " AS \"FORTNIGHTCOUNT\",C.\"TC_COUNT\" AS \"MONTHCOUNT\" , D.\"TC_COUNT\" AS \"MORETHENMONTHCOUNT\" ,'" + objReport.sFromDate + "' AS \"FROMDATE\" ,";
                //strQry += " '" + objReport.sTodate + "' AS \"TODATE\" ,TO_CHAR(CURRENT_DATE,'DD-MON-YYYY') AS CURRENTDATE ";
                //strQry += " FROM( SELECT B.\"OFF_CODE\", (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)AS \"OFC_NAME\"  FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(B.\"OFF_CODE\" AS TEXT),1," + Constants.Division + ")) ";
                //strQry += "  \"DIV\", (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)= substr(CAST(b.\"OFF_CODE\" AS TEXT),1," + Constants.Zone + "))\"ZONE\" , (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)= substr(CAST(b.\"OFF_CODE\" AS TEXT),1," + Constants.Circle + "))";
                //strQry += "  \"CIRCLE\",(SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)= substr(cast(b.\"OFF_CODE\" AS TEXT),1," + Constants.Section + "))";
                //strQry += "  \"SECTION\",  COALESCE(\"TC_COUNT\",0) \"TC_COUNT\",\"MD_NAME\" AS \"TC_CAPACITY\",SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) \"SUBDIV\" FROM  ";
                //strQry += "  (SELECT SUM(CASE COALESCE(\"DF_ID\",0) WHEN 0 THEN 0 ELSE 1 END) AS \"TC_COUNT\",\"TC_CAPACITY\", SUBSTR(CAST(\"OFF_CODE\" AS TEXT),1," + Constants.Section + ") \"OFF_CODE\"";
                //strQry += "  FROM \"VIEW_ALL_OFFICES\" LEFT JOIN \"TBLDTCFAILURE\" ON SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Section + ")=CAST(\"OFF_CODE\" AS TEXT) LEFT JOIN \"TBLTCMASTER\" ON ";
                //strQry += "  \"TC_CODE\"=\"DF_EQUIPMENT_ID\" WHERE LENGTH(CAST(\"OFF_CODE\" AS TEXT))=" + Constants.Section + " AND \"DF_DATE\">=CURRENT_DATE-7 AND \"DF_STATUS_FLAG\"<>2 AND \"DF_REPLACE_FLAG\"='0'  ";
                //strQry += "  GROUP BY SUBSTR(CAST(\"OFF_CODE\" AS TEXT),1," + Constants.Section + "),\"TC_CAPACITY\")A RIGHT JOIN (SELECT \"MD_NAME\",\"OFF_NAME\",\"OFF_CODE\" FROM \"TBLMASTERDATA\",";
                //strQry += "  \"VIEW_ALL_OFFICES\" WHERE UPPER(\"MD_TYPE\")='C' AND  LENGTH(CAST(\"OFF_CODE\" AS TEXT))=" + Constants.Section + " AND CAST(\"MD_NAME\" AS INTEGER)<=500)B ON CAST(B.\"OFF_CODE\" AS TEXT)=A.\"OFF_CODE\" ";
                //strQry += "  AND A.\"TC_CAPACITY\"=CAST(B.\"MD_NAME\" AS INTEGER) ORDER BY \"MD_NAME\")A  INNER JOIN (SELECT (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)AS \"OFC_NAME\"";
                //strQry += "   FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(B.\"OFF_CODE\" AS TEXT),1," + Constants.Division + ")) \"DIV\",  COALESCE(\"TC_COUNT\",0) \"TC_COUNT\",\"MD_NAME\" AS \"TC_CAPACITY\",";
                //strQry += "   SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) \"SUBDIV\" FROM (SELECT SUM(CASE COALESCE(\"DF_ID\",0) WHEN 0 THEN 0 ELSE 1 END) AS \"TC_COUNT\",";
                //strQry += "  \"TC_CAPACITY\", SUBSTR(CAST(\"OFF_CODE\" AS TEXT),1," + Constants.Section + ") \"OFF_CODE\" FROM \"VIEW_ALL_OFFICES\" LEFT JOIN \"TBLDTCFAILURE\" ON SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Section + ")=CAST(\"OFF_CODE\" AS TEXT) ";
                //strQry += "  LEFT JOIN \"TBLTCMASTER\" ON \"TC_CODE\"=\"DF_EQUIPMENT_ID\" WHERE LENGTH(CAST(\"OFF_CODE\" AS TEXT))=" + Constants.Section + " AND \"DF_DATE\">=(CURRENT_DATE-14) AND \"DF_DATE\"<=CURRENT_DATE-7";
                //strQry += "  AND \"DF_STATUS_FLAG\"<>2 AND \"DF_REPLACE_FLAG\"='0' GROUP BY SUBSTR(CAST(\"OFF_CODE\" AS TEXT),1," + Constants.Section + "),\"TC_CAPACITY\")A RIGHT JOIN (SELECT \"MD_NAME\",\"OFF_NAME\",\"OFF_CODE\"  ";
                //strQry += "   FROM \"TBLMASTERDATA\",\"VIEW_ALL_OFFICES\" WHERE UPPER(\"MD_TYPE\")='C' AND  LENGTH(CAST(\"OFF_CODE\" AS TEXT))=" + Constants.Section + " AND CAST(\"MD_NAME\" AS INTEGER)<=500)B ON";
                //strQry += "   CAST(B.\"OFF_CODE\" AS TEXT)=A.\"OFF_CODE\" AND A.\"TC_CAPACITY\"=CAST(B.\"MD_NAME\" AS INTEGER) ORDER BY \"MD_NAME\")B ON A.\"DIV\"=B.\"DIV\" AND A.\"TC_CAPACITY\"=B.\"TC_CAPACITY\" AND";
                //strQry += "  A.\"SUBDIV\"=B.\"SUBDIV\" INNER JOIN  (SELECT (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)AS \"OFC_NAME\"  FROM \"VIEW_ALL_OFFICES\"";
                //strQry += "   WHERE CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(B.\"OFF_CODE\" AS TEXT),1," + Constants.Division + ")) \"DIV\",  COALESCE(\"TC_COUNT\",0) \"TC_COUNT\",\"MD_NAME\" AS \"TC_CAPACITY\",SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)";
                //strQry += "  \"SUBDIV\" FROM (SELECT SUM(CASE COALESCE(\"DF_ID\",0) WHEN 0 THEN 0 ELSE 1 END) AS \"TC_COUNT\",\"TC_CAPACITY\", SUBSTR(CAST(\"OFF_CODE\" AS TEXT),1," + Constants.Section + ") ";
                //strQry += "  \"OFF_CODE\" FROM \"VIEW_ALL_OFFICES\" LEFT JOIN \"TBLDTCFAILURE\" ON SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Section + ")=CAST(\"OFF_CODE\" AS TEXT) LEFT JOIN \"TBLTCMASTER\" ON ";
                //strQry += "  \"TC_CODE\"=\"DF_EQUIPMENT_ID\" WHERE LENGTH(CAST(\"OFF_CODE\" AS TEXT))=" + Constants.Section + " AND \"DF_DATE\">=(CURRENT_DATE-30) AND \"DF_DATE\"<=(CURRENT_DATE-14) AND \"DF_STATUS_FLAG\"<>2 AND \"DF_REPLACE_FLAG\"='0' ";
                //strQry += "  GROUP BY SUBSTR(CAST(\"OFF_CODE\" AS TEXT),1," + Constants.Section + "),\"TC_CAPACITY\")A RIGHT JOIN (SELECT \"MD_NAME\",\"OFF_NAME\",\"OFF_CODE\"  FROM \"TBLMASTERDATA\",\"VIEW_ALL_OFFICES\"";
                //strQry += "  WHERE UPPER(\"MD_TYPE\")='C' AND  LENGTH(CAST(\"OFF_CODE\" AS TEXT))=" + Constants.Section + " AND CAST(\"MD_NAME\" AS INTEGER)<=500)B ON CAST(B.\"OFF_CODE\" AS TEXT)=A.\"OFF_CODE\" AND A.\"TC_CAPACITY\"=CAST(B.\"MD_NAME\" AS INTEGER) ";
                //strQry += "  ORDER BY \"MD_NAME\")C ON C.\"DIV\"=B.\"DIV\" AND C.\"TC_CAPACITY\"=C.\"TC_CAPACITY\" AND C.\"SUBDIV\"=B.\"SUBDIV\" AND A.\"DIV\"=C.\"DIV\" AND A.\"TC_CAPACITY\"=C.\"TC_CAPACITY\"";
                //strQry += "  AND A.\"SUBDIV\"=C.\"SUBDIV\" INNER JOIN  (SELECT (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)AS \"OFC_NAME\"  FROM \"VIEW_ALL_OFFICES\" ";
                //strQry += "  WHERE CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(B.\"OFF_CODE\" AS TEXT),1," + Constants.Division + ")) \"DIV\",  COALESCE(\"TC_COUNT\",0) \"TC_COUNT\",\"MD_NAME\" AS \"TC_CAPACITY\",SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)";
                //strQry += "  \"SUBDIV\" FROM (SELECT SUM(CASE COALESCE(\"DF_ID\",0) WHEN 0 THEN 0 ELSE 1 END) AS \"TC_COUNT\",\"TC_CAPACITY\", SUBSTR(CAST(\"OFF_CODE\" AS TEXT),1," + Constants.Section + ") \"OFF_CODE\"";
                //strQry += "  FROM \"VIEW_ALL_OFFICES\" LEFT JOIN \"TBLDTCFAILURE\" ON SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Section + ")=CAST(\"OFF_CODE\" AS TEXT) AND \"DF_REPLACE_FLAG\"='0' LEFT JOIN \"TBLTCMASTER\" ON \"TC_CODE\"=\"DF_EQUIPMENT_ID\"";
                //strQry += "  WHERE LENGTH(CAST(\"OFF_CODE\" AS TEXT))=" + Constants.Section + " AND \"DF_DATE\"<=(CURRENT_DATE-30) AND \"DF_STATUS_FLAG\"<>2 AND \"DF_REPLACE_FLAG\"='0' AND ";

                //if (objReport.sTodate == null && (objReport.sFromDate != null))
                //{
                //    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD')";
                //    //strQry += " DF_DATE BETWEEN to_date('" + objReport.sFromDate + "','DD/MM/YYYY')";
                //}
                //if (objReport.sFromDate == null && (objReport.sTodate != null))
                //{
                //    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                //}
                //if (objReport.sFromDate == null && objReport.sTodate == null)
                //{
                //    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                //}
                //if (objReport.sFromDate != null && objReport.sTodate != null)
                //{
                //    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                //}

                //strQry += " GROUP BY SUBSTR(CAST(\"OFF_CODE\" AS TEXT),1," + Constants.Section + "),\"TC_CAPACITY\")A RIGHT JOIN ";
                //strQry += "   (SELECT \"MD_NAME\",\"OFF_NAME\",\"OFF_CODE\"  FROM \"TBLMASTERDATA\",\"VIEW_ALL_OFFICES\" WHERE UPPER(\"MD_TYPE\")='C' AND  LENGTH(CAST(\"OFF_CODE\" AS TEXT))=" + Constants.Section + " AND ";
                //strQry += "  CAST(\"MD_NAME\" AS INTEGER)<=500)B ON CAST(B.\"OFF_CODE\" AS TEXT)=A.\"OFF_CODE\" AND A.\"TC_CAPACITY\"=CAST(B.\"MD_NAME\" AS INTEGER) ORDER BY \"MD_NAME\")D ON C.\"DIV\"=D.\"DIV\" AND ";
                //strQry += "  C.\"TC_CAPACITY\"=D.\"TC_CAPACITY\" AND C.\"SUBDIV\"=D.\"SUBDIV\" AND A.\"DIV\"=D.\"DIV\" AND A.\"TC_CAPACITY\"=D.\"TC_CAPACITY\" AND A.\"SUBDIV\"=D.\"SUBDIV\" ";
                //strQry += "   AND B.\"DIV\"=D.\"DIV\" AND B.\"TC_CAPACITY\"=D.\"TC_CAPACITY\" AND B.\"SUBDIV\"=D.\"SUBDIV\" AND CAST(\"OFF_CODE\" AS TEXT) LIKE '" + objReport.sOfficeCode + "%' ORDER BY \"CIRCLE\",\"DIV\",\"ZONE\" ";
                //dt = ObjCon.FetchDataTable(strQry);
                #endregion
                NpgsqlCommand cmd = new NpgsqlCommand("sp_printabstractreport");
                cmd.Parameters.AddWithValue("stodate", Convert.ToString(objReport.sTodate == null ? "" : objReport.sTodate));
                cmd.Parameters.AddWithValue("sfromdate", Convert.ToString(objReport.sFromDate == null ? "" : objReport.sFromDate));
                cmd.Parameters.AddWithValue("zone", Convert.ToString(Constants.Zone));
                cmd.Parameters.AddWithValue("circle", Convert.ToString(Constants.Circle));
                cmd.Parameters.AddWithValue("division", Convert.ToString(Constants.Division));
                cmd.Parameters.AddWithValue("section", Convert.ToString(Constants.Section));
                cmd.Parameters.AddWithValue("offcode", Convert.ToString(sOfficeCode == null ? "" : sOfficeCode));
                dt = ObjCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }
        }


        public DataTable PrintAbstractReportTcFailedAtFSR(clsReports objReport)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                //strQry = " SELECT A.MD_NAME AS CAPACITY,SUBSTR(A.OFF_NAME,STRPOS(A.OFF_NAME,':')+1)DIV,A.OFF_CODE,TC_FAILEDBUTNOTRETURNED AS FIELD_COUNT,TC_FAILEDBUTNOTMAPPED AS FIELD_COUNT_TOBEREPLACED,TCFAILEDBUTINSTORE AS STORE_COUNT,TCFAILEDBUTINREPAIRCENTER AS REPAIRER_COUNT,  ";
                //strQry += " (SELECT SUBSTR(OFF_NAME,STRPOS(OFF_NAME,':')+1)  FROM VIEW_ALL_OFFICES WHERE OFF_CODE=SUBSTR(B.OFF_CODE,0,1))CIRCLE FROM ";
                //strQry += " (SELECT MD_NAME,OFF_NAME,OFF_CODE,COALESCE(TC_FAILEDBUTNOTRETURNED,0) TC_FAILEDBUTNOTRETURNED FROM (SELECT TC_CAPACITY,SUBSTR";
                //strQry += "  (TC_LOCATION_ID,0,2)TC_LOCATION_ID ,COUNT(TC_CODE) AS TC_FAILEDBUTNOTRETURNED FROM TBLTCMASTER INNER JOIN  TBLDTCFAILURE";
                //strQry += "  ON TC_CODE=DF_EQUIPMENT_ID INNER JOIN TBLWORKORDER ON DF_ID=WO_DF_ID INNER JOIN TBLINDENT ON WO_SLNO=TI_WO_SLNO INNER JOIN ";
                //strQry += " TBLDTCINVOICE ON IN_TI_NO=TI_ID LEFT JOIN TBLTCREPLACE ON IN_NO= TR_IN_NO WHERE TR_RI_NO IS NULL OR TR_RV_NO IS NULL AND TC_STATUS='3'  GROUP BY";
                //strQry += " TC_CAPACITY,SUBSTR(TC_LOCATION_ID,0,2))A RIGHT JOIN (SELECT MD_NAME,OFF_NAME,OFF_CODE  FROM TBLMASTERDATA,VIEW_ALL_OFFICES WHERE UPPER(MD_TYPE)='C' AND TO_NUMBER(MD_NAME)<=500 AND  LENGTH ";
                //strQry += " (OFF_CODE)=2)B ON MD_NAME=TC_CAPACITY AND TC_LOCATION_ID=OFF_CODE)A INNER JOIN ";
                //strQry += " (SELECT MD_NAME,OFF_NAME,OFF_CODE,COALESCE(TC_FAILEDBUTNOTMAPPED,0) TC_FAILEDBUTNOTMAPPED FROM (SELECT TC_CAPACITY,SUBSTR  ";
                //strQry += " (TC_LOCATION_ID,0,2)TC_LOCATION_ID ,COUNT(TC_CODE) AS TC_FAILEDBUTNOTMAPPED FROM TBLTCMASTER INNER JOIN  TBLDTCFAILURE  ON  ";
                //strQry += " TC_CODE=DF_EQUIPMENT_ID LEFT JOIN TBLWORKORDER ON DF_ID=WO_DF_ID LEFT JOIN TBLINDENT ON WO_SLNO=TI_WO_SLNO  LEFT JOIN  ";
                //strQry += " TBLDTCINVOICE ON IN_TI_NO=TI_ID  WHERE IN_TI_NO IS NULL AND TC_STATUS='3' AND TC_CAPACITY<=500 GROUP BY TC_CAPACITY,SUBSTR(TC_LOCATION_ID,0,2))A RIGHT JOIN";
                //strQry += " (SELECT MD_NAME,OFF_NAME,OFF_CODE  FROM TBLMASTERDATA,VIEW_ALL_OFFICES WHERE UPPER(MD_TYPE)='C' AND  LENGTH(OFF_CODE)=2)B";
                //strQry += " ON MD_NAME=TC_CAPACITY AND TC_LOCATION_ID=OFF_CODE)B ON A.MD_NAME=B.MD_NAME AND A.OFF_NAME=B.OFF_NAME AND A.OFF_CODE=B.OFF_CODE INNER JOIN ";
                //strQry += " (SELECT MD_NAME,OFF_NAME,OFF_CODE,COALESCE(TCFAILEDBUTINSTORE,0) TCFAILEDBUTINSTORE FROM (SELECT COUNT(TC_CODE) ";
                //strQry += " TCFAILEDBUTINSTORE,SUBSTR(TC_LOCATION_ID,0,2)TC_LOCATION_ID,TC_CAPACITY FROM TBLTCMASTER WHERE TC_STATUS=3 AND";
                //strQry += " TC_CURRENT_LOCATION=1 GROUP BY SUBSTR(TC_LOCATION_ID,0,2),TC_CAPACITY )A RIGHT JOIN ";
                //strQry += " (SELECT MD_NAME,OFF_NAME,OFF_CODE  FROM TBLMASTERDATA,VIEW_ALL_OFFICES WHERE UPPER(MD_TYPE)='C' AND  LENGTH(OFF_CODE)=2)B ";
                //strQry += " ON MD_NAME=TC_CAPACITY AND TC_LOCATION_ID=OFF_CODE AND TO_NUMBER(MD_NAME)<=500)C ON A.MD_NAME=C.MD_NAME AND A.OFF_NAME=C.OFF_NAME AND A.OFF_CODE=C.OFF_CODE  INNER JOIN ";
                //strQry += " (SELECT MD_NAME,OFF_NAME,OFF_CODE,COALESCE(TCFAILEDBUTINREPAIRCENTER,0) TCFAILEDBUTINREPAIRCENTER FROM (SELECT COUNT(TC_CODE)";
                //strQry += " TCFAILEDBUTINREPAIRCENTER,SUBSTR(TC_LOCATION_ID,0,2)TC_LOCATION_ID,TC_CAPACITY FROM TBLTCMASTER WHERE TC_STATUS=3 AND ";
                //strQry += " TC_CURRENT_LOCATION=3 GROUP BY SUBSTR(TC_LOCATION_ID,0,2),TC_CAPACITY )A RIGHT JOIN ";
                //strQry += " (SELECT MD_NAME,OFF_NAME,OFF_CODE  FROM TBLMASTERDATA,VIEW_ALL_OFFICES WHERE UPPER(MD_TYPE)='C' AND  LENGTH(OFF_CODE)=2)B ";
                //strQry += " ON MD_NAME=TC_CAPACITY AND TC_LOCATION_ID=OFF_CODE AND TO_NUMBER(MD_NAME)<=500)D ON A.MD_NAME=D.MD_NAME AND A.OFF_NAME=D.OFF_NAME AND A.OFF_CODE=D.OFF_CODE AND A.OFF_CODE LIKE '" + objReport.sOfficeCode + "%'  ";
                #region commented Inline query
                //strQry = " SELECT A.\"MD_NAME\" AS \"CAPACITY\",SUBSTR(A.\"OFF_NAME\",STRPOS(A.\"OFF_NAME\",':')+1) \"DIV\",A.\"OFF_CODE\",\"TC_FAILEDBUTNOTRETURNED\" AS ";
                //strQry += "\"FIELD_COUNT\",\"TC_FAILEDBUTNOTMAPPED\" AS \"FIELD_COUNT_TOBEREPLACED\",\"TCFAILEDBUTINSTORE\" AS \"STORE_COUNT\",\"TCFAILEDBUTINREPAIRCENTER\" AS ";
                //strQry += "\"REPAIRER_COUNT\",(SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE cast(\"OFF_CODE\" as text)=SUBSTR(cast(B.\"OFF_CODE\" as text),1," + Constants.Zone + "))\"ZONE\",(SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE cast(\"OFF_CODE\" as text)=SUBSTR(cast(B.\"OFF_CODE\" as text),1," + Constants.Circle + ")) ";
                //strQry += " \"CIRCLE\" FROM (SELECT \"MD_NAME\",\"OFF_NAME\",\"OFF_CODE\",COALESCE(\"TC_FAILEDBUTNOTRETURNED\",0) \"TC_FAILEDBUTNOTRETURNED\" FROM ";
                //strQry += " (SELECT \"TC_CAPACITY\",SUBSTR (cast(\"TC_LOCATION_ID\" as text),1," + Constants.Division + ")\"TC_LOCATION_ID\" ,COUNT(\"TC_CODE\") AS \"TC_FAILEDBUTNOTRETURNED\" ";
                //strQry += " FROM \"TBLTCMASTER\" INNER JOIN \"TBLDTCFAILURE\" ON \"TC_CODE\"=\"DF_EQUIPMENT_ID\" INNER JOIN \"TBLWORKORDER\" ON \"DF_ID\"=\"WO_DF_ID\" ";
                //strQry += " INNER JOIN \"TBLINDENT\" ON \"WO_SLNO\"=\"TI_WO_SLNO\" INNER JOIN \"TBLDTCINVOICE\" ON  \"IN_TI_NO\"=\"TI_ID\" LEFT JOIN \"TBLTCREPLACE\" ON ";
                //strQry += "\"IN_NO\"= \"TR_IN_NO\" WHERE \"TR_RI_NO\" IS NULL OR \"TR_RV_NO\" IS NULL AND \"TC_STATUS\"='3' GROUP BY \"TC_CAPACITY\",SUBSTR (cast(\"TC_LOCATION_ID\" AS TEXT),1," + Constants.Division + "))A";
                //strQry += " RIGHT JOIN (SELECT \"MD_NAME\",\"OFF_NAME\",\"OFF_CODE\" FROM \"TBLMASTERDATA\",\"VIEW_ALL_OFFICES\" WHERE UPPER(\"MD_TYPE\")='C' AND cast(\"MD_NAME\" AS INTEGER)<=500";
                //strQry += "AND LENGTH(cast(\"OFF_CODE\" AS TEXT))='3')B ON CAST(\"MD_NAME\" as numeric)=\"TC_CAPACITY\" AND \"TC_LOCATION_ID\"=cast(\"OFF_CODE\" ";
                //strQry += " AS TEXT))A INNER JOIN (SELECT \"MD_NAME\",\"OFF_NAME\",\"OFF_CODE\",COALESCE(\"TC_FAILEDBUTNOTMAPPED\",0) \"TC_FAILEDBUTNOTMAPPED\" FROM (SELECT ";
                //strQry += " \"TC_CAPACITY\",SUBSTR (cast(\"TC_LOCATION_ID\" as text),1," + Constants.Division + ") \"TC_LOCATION_ID\" ,COUNT(\"TC_CODE\") AS \"TC_FAILEDBUTNOTMAPPED\" FROM \"TBLTCMASTER\"";
                //strQry += "INNER JOIN \"TBLDTCFAILURE\" ON \"TC_CODE\"=\"DF_EQUIPMENT_ID\" ";

                //if (objReport.sTodate == null && (objReport.sFromDate != null))
                //{
                //    strQry += "AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD')";
                //    //strQry += " DF_DATE BETWEEN to_date('" + objReport.sFromDate + "','DD/MM/YYYY')";
                //}
                //if (objReport.sFromDate == null && (objReport.sTodate != null))
                //{
                //    strQry += "AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                //}
                //if (objReport.sFromDate == null && objReport.sTodate == null)
                //{
                //    strQry += "AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                //}
                //if (objReport.sFromDate != null && objReport.sTodate != null)
                //{
                //    strQry += "AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                //}


                //strQry += " LEFT JOIN \"TBLWORKORDER\" ON \"DF_ID\"=\"WO_DF_ID\" LEFT JOIN \"TBLINDENT\" ON \"WO_SLNO\"=\"TI_WO_SLNO\"  LEFT JOIN ";
                //strQry += " \"TBLDTCINVOICE\" ON \"IN_TI_NO\"=\"TI_ID\" WHERE \"IN_TI_NO\" IS NULL AND \"TC_STATUS\"='3' AND \"TC_CAPACITY\"<=500 GROUP BY \"TC_CAPACITY\",";
                //strQry += "SUBSTR (cast(\"TC_LOCATION_ID\" as text),1," + Constants.Division + "))A RIGHT JOIN (SELECT \"MD_NAME\",\"OFF_NAME\",\"OFF_CODE\" FROM \"TBLMASTERDATA\",\"VIEW_ALL_OFFICES\"";
                //strQry += "WHERE UPPER(\"MD_TYPE\")='C' AND  LENGTH(cast(\"OFF_CODE\" as text))='3')B ON cast(\"MD_NAME\" as numeric)=\"TC_CAPACITY\" AND ";
                //strQry += "\"TC_LOCATION_ID\"=cast(\"OFF_CODE\" as text))B ON A.\"MD_NAME\"=B.\"MD_NAME\" AND A.\"OFF_NAME\"=B.\"OFF_NAME\" AND A.\"OFF_CODE\"=B.\"OFF_CODE\"";
                //strQry += "INNER JOIN (SELECT \"MD_NAME\",\"OFF_NAME\",\"OFF_CODE\",COALESCE(\"TCFAILEDBUTINSTORE\",0) \"TCFAILEDBUTINSTORE\" FROM (SELECT COUNT(\"TC_CODE\") ";
                //strQry += "\"TCFAILEDBUTINSTORE\",SUBSTR (cast(\"TC_LOCATION_ID\" as text),1," + Constants.Division + ")\"TC_LOCATION_ID\",\"TC_CAPACITY\" FROM \"TBLTCMASTER\" WHERE \"TC_STATUS\"=3 AND ";
                //strQry += " \"TC_CURRENT_LOCATION\"=1 GROUP BY SUBSTR (cast(\"TC_LOCATION_ID\" as text),1," + Constants.Division + "),\"TC_CAPACITY\")A RIGHT JOIN (SELECT \"MD_NAME\",\"OFF_NAME\",";
                //strQry += "\"OFF_CODE\" FROM \"TBLMASTERDATA\",\"VIEW_ALL_OFFICES\" WHERE UPPER(\"MD_TYPE\")='C' AND LENGTH(cast(\"OFF_CODE\" as text))='3')B ON cast(\"MD_NAME\"";
                //strQry += "as numeric)=\"TC_CAPACITY\" AND \"TC_LOCATION_ID\"=cast(\"OFF_CODE\" as text) AND cast(\"MD_NAME\" as integer)<=500)C ON A.\"MD_NAME\"=C.\"MD_NAME\" AND A.\"OFF_NAME\"=C.\"OFF_NAME\" AND ";
                //strQry += " A.\"OFF_CODE\"=C.\"OFF_CODE\" INNER JOIN (SELECT \"MD_NAME\",\"OFF_NAME\",\"OFF_CODE\",COALESCE(\"TCFAILEDBUTINREPAIRCENTER\",0) \"TCFAILEDBUTINREPAIRCENTER\"";
                //strQry += "FROM (SELECT COUNT(\"TC_CODE\") \"TCFAILEDBUTINREPAIRCENTER\",SUBSTR (cast(\"TC_LOCATION_ID\" as text),1," + Constants.Division + ")\"TC_LOCATION_ID\",\"TC_CAPACITY\" FROM";
                //strQry += "\"TBLTCMASTER\" WHERE \"TC_STATUS\"=3 AND "; strQry += " \"TC_CURRENT_LOCATION\"=3 GROUP BY SUBSTR (cast(\"TC_LOCATION_ID\" as text),1," + Constants.Division + "),\"TC_CAPACITY\"";
                //strQry += ")A RIGHT JOIN (SELECT \"MD_NAME\",\"OFF_NAME\",\"OFF_CODE\" FROM \"TBLMASTERDATA\",\"VIEW_ALL_OFFICES\" WHERE UPPER(\"MD_TYPE\")='C' AND LENGTH(cast(\"OFF_CODE\" as text))='3')B ON cast(\"MD_NAME\" as numeric)=\"TC_CAPACITY\" AND ";
                //strQry += " \"TC_LOCATION_ID\"=cast(\"OFF_CODE\" as text) AND cast(\"MD_NAME\" as integer)<=500)D ON A.\"MD_NAME\"=D.\"MD_NAME\" AND A.\"OFF_NAME\"=D.\"OFF_NAME\" AND A.\"OFF_CODE\"=D.\"OFF_CODE\" AND cast(A.\"OFF_CODE\" as text) LIKE '" + objReport.sOfficeCode + "%'";
                //dt = ObjCon.FetchDataTable(strQry);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("sp_printabstractreporttcfailedatfsr");
                cmd.Parameters.AddWithValue("zone", Convert.ToString(Constants.Zone));
                cmd.Parameters.AddWithValue("circle", Convert.ToString(Constants.Circle));
                cmd.Parameters.AddWithValue("division", Convert.ToString(Constants.Division));
                cmd.Parameters.AddWithValue("subdivision", Convert.ToString(Constants.SubDivision));
                cmd.Parameters.AddWithValue("offcode", Convert.ToString(sOfficeCode == null ? "" : sOfficeCode));
                cmd.Parameters.AddWithValue("stodate", Convert.ToString(objReport.sTodate == null ? "" : objReport.sTodate));
                cmd.Parameters.AddWithValue("sfromdate", Convert.ToString(objReport.sFromDate == null ? "" : objReport.sFromDate));
                dt = ObjCon.FetchDataTable(cmd);

                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }
        }


        public DataTable PrintRepairerTcCount(clsReports objReport)
        {
            string strQry = string.Empty;
            DataTable dt = new DataTable();


            //strQry = "SELECT NVL(TC_COUNT,0)TC_COUNT,MD_NAME AS TC_CAPACITY,REPAIRER FROM(SELECT TC_CAPACITY,NANVL( COUNT(TC_CODE),0) TC_COUNT,RSM_SUPREP_ID,TR_NAME AS";
            //strQry+= " REPAIRER FROM TBLTCMASTER INNER JOIN TBLREPAIRSENTDETAILS ON RSD_TC_CODE=TC_CODE INNER JOIN TBLREPAIRSENTMASTER ON RSM_ID=RSD_RSM_ID ";
            //strQry +=" INNER JOIN TBLTRANSREPAIRER ON RSM_SUPREP_ID=TR_ID WHERE  RSD_DELIVARY_DATE IS NULL GROUP BY TC_CAPACITY,RSM_SUPREP_ID,TR_NAME ORDER BY RSM_SUPREP_ID)A";
            //strQry +=" RIGHT JOIN (SELECT MD_NAME  FROM TBLMASTERDATA WHERE UPPER(MD_TYPE)='C' AND MD_NAME<=500)B ON MD_NAME=TC_CAPACITY";


            //strQry = "SELECT \"MD_NAME\" AS \"TC_CAPACITY\",\"TR_NAME\" AS \"REPAIRER\",COALESCE(\"TC_COUNT\",0) AS \"TC_COUNT\" FROM (SELECT \"TC_LOCATION_ID\",\"TC_CAPACITY\",COALESCE(COUNT(\"TC_CODE\"),0) \"TC_COUNT\",\"RSM_SUPREP_ID\" FROM \"TBLTCMASTER\" INNER JOIN ";
            //strQry += " \"TBLREPAIRSENTDETAILS\" ON \"RSD_TC_CODE\"=\"TC_CODE\" INNER JOIN \"TBLREPAIRSENTMASTER\" ON \"RSM_ID\"=\"RSD_RSM_ID\" WHERE  \"RSD_DELIVARY_DATE\" IS NULL AND \"RSM_SUPREP_TYPE\"=2 AND CAST(\"TC_LOCATION_ID\" AS TEXT)  LIKE '" + objReport.sOfficeCode + "%' AND \"TC_STATUS\"='3' AND \"TC_CURRENT_LOCATION\"='3'";
            //strQry += " GROUP BY \"TC_CAPACITY\",\"RSM_SUPREP_ID\",\"TC_LOCATION_ID\" ORDER BY \"RSM_SUPREP_ID\")A RIGHT JOIN (SELECT \"TR_NAME\",\"TR_ID\",\"MD_NAME\" FROM \"TBLTRANSREPAIRER\",\"TBLMASTERDATA\" WHERE UPPER(\"MD_TYPE\")='C' ";
            //strQry += " AND \"MD_NAME\"<=500)C ON  \"MD_NAME\"=\"TC_CAPACITY\" AND \"RSM_SUPREP_ID\"=\"TR_ID\" AND CAST(\"TC_LOCATION_ID\" AS TEXT) LIKE '" + objReport.sOfficeCode + "%' ";
            #region Commented Inline query
            //strQry = " SELECT \"MD_NAME\" AS \"TC_CAPACITY\",\"TR_NAME\" AS \"REPAIRER\",COALESCE(\"TC_COUNT\",0) AS \"TC_COUNT\" FROM (SELECT ";
            //strQry += "\"TC_LOCATION_ID\",\"TC_CAPACITY\",COALESCE(COUNT(\"TC_CODE\"),0) \"TC_COUNT\",\"RSM_SUPREP_ID\" FROM \"TBLTCMASTER\" INNER JOIN ";
            //strQry += " \"TBLREPAIRSENTDETAILS\" ON \"RSD_TC_CODE\"=\"TC_CODE\" INNER JOIN \"TBLREPAIRSENTMASTER\" ON \"RSM_ID\"=\"RSD_RSM_ID\" WHERE ";
            //strQry += "\"RSD_DELIVARY_DATE\" IS NULL AND \"RSM_SUPREP_TYPE\"=2 AND CAST(\"TC_LOCATION_ID\" AS TEXT) LIKE '%' AND \"TC_STATUS\"='3' AND ";
            //strQry += "\"TC_CURRENT_LOCATION\"='3'";
            //strQry += " GROUP BY \"TC_CAPACITY\",\"RSM_SUPREP_ID\",\"TC_LOCATION_ID\" ORDER BY \"RSM_SUPREP_ID\")A";
            //strQry += " RIGHT JOIN (SELECT \"TR_NAME\",\"TR_ID\",\"MD_NAME\" FROM \"TBLTRANSREPAIRER\",\"TBLMASTERDATA\" WHERE UPPER(\"MD_TYPE\")='C' ";
            //strQry += " AND cast(\"MD_NAME\" as integer)<=500)C ON \"MD_NAME\"=cast(\"TC_CAPACITY\" as VARCHAR) AND \"RSM_SUPREP_ID\"=\"TR_ID\" AND CAST(\"TC_LOCATION_ID\" AS TEXT)";
            //strQry += " LIKE '" + objReport.sOfficeCode + "%'";
            //dt = ObjCon.FetchDataTable(strQry);
            #endregion
            NpgsqlCommand cmd = new NpgsqlCommand("sp_printrepairertccount");
            cmd.Parameters.AddWithValue("offcode", objReport.sOfficeCode);
            dt = ObjCon.FetchDataTable(cmd);
            return dt;
        }


        public DataTable PrintCompletedRepairerTcCount(clsReports objReport)
        {
            string strQry = string.Empty;
            DataTable dt = new DataTable();
            #region Commented Inline query
            //strQry = "SELECT \"MD_NAME\" AS \"TC_CAPACITY\",\"TR_NAME\" AS \"REPAIRER\",COALESCE(\"TC_COUNT\",0) AS \"TC_COUNT\" FROM (SELECT \"TC_LOCATION_ID\",\"TC_CAPACITY\",COALESCE(COUNT(\"TC_CODE\"),0) \"TC_COUNT\",\"RSM_SUPREP_ID\" FROM \"TBLTCMASTER\" INNER JOIN";
            //strQry += "\"TBLREPAIRSENTDETAILS\" ON \"RSD_TC_CODE\"=\"TC_CODE\" INNER JOIN \"TBLREPAIRSENTMASTER\" ON \"RSM_ID\"=\"RSD_RSM_ID\" WHERE  \"RSD_DELIVARY_DATE\" IS NOT NULL AND \"RSM_SUPREP_TYPE\"=2 ";
            //strQry += " GROUP BY \"TC_CAPACITY\",\"RSM_SUPREP_ID\",\"TC_LOCATION_ID\" ORDER BY \"RSM_SUPREP_ID\")A RIGHT JOIN (SELECT \"TR_NAME\",\"TR_ID\",\"MD_NAME\" FROM \"TBLTRANSREPAIRER\",\"TBLMASTERDATA\" WHERE UPPER(\"MD_TYPE\")='C'";
            //strQry += " AND cast(\"MD_NAME\" as integer)<=500)C ON \"MD_NAME\"=cast(\"TC_CAPACITY\" as VARCHAR) AND \"RSM_SUPREP_ID\"=\"TR_ID\" AND CAST(\"TC_LOCATION_ID\" AS TEXT) LIKE '" + objReport.sOfficeCode + "%' ";
            //dt = ObjCon.FetchDataTable(strQry);
            #endregion
            NpgsqlCommand cmd = new NpgsqlCommand("sp_printcompletedrepairertccount");
            cmd.Parameters.AddWithValue("offcode", objReport.sOfficeCode);
            dt = ObjCon.FetchDataTable(cmd);
            return dt;
        }


        public DataTable PrintAbstractReportWeekWise()
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "SELECT (SELECT SUBSTR(\"OFF_NAME\",INSTR(OFF_NAME,':')+1)  from \"VIEW_ALL_OFFICES\" where \"OFF_CODE\"=substr(b.\"OFF_CODE\",0,2)) div,  COALESCE(\"TC_COUNT\",0) \"TC_COUNT\",\"MD_NAME\" AS ";
                strQry += " \"TC_CAPACITY\",SUBSTR(\"OFF_NAME\",INSTR(\"OFF_NAME\",':')+1) \"SUBDIV\" FROM (SELECT sum(CASE COALESCE(\"DF_ID\",0) WHEN 0 THEN 0 ELSE 1 END) AS \"TC_COUNT\",\"TC_CAPACITY\", SUBSTR(\"OFF_CODE\",0,3) \"OFF_CODE\" FROM \"VIEW_ALL_OFFICES\" LEFT JOIN \"TBLDTCFAILURE\" ";
                strQry += "  ON SUBSTR(\"DF_LOC_CODE\",0,3)=\"OFF_CODE\" LEFT JOIN \"TBLTCMASTER\" on \"TC_CODE\"=\"DF_EQUIPMENT_ID\" WHERE LENGTH(\"OFF_CODE\")=3 AND \"DF_DATE\">=NOW()-7   GROUP BY SUBSTR(\"OFF_CODE\",0,3),\"TC_CAPACITY\")a right JOIN ";
                strQry += " (SELECT \"MD_NAME\",\"OFF_NAME\",\"OFF_CODE\"  FROM \"TBLMASTERDATA\",\"VIEW_ALL_OFFICES\" where upper(\"MD_TYPE\")='C' ";
                strQry += "  AND  LENGTH(\"OFF_CODE\")=3)b on b.\"OFF_CODE\"=a.\"OFF_CODE\" and a.\"TC_CAPACITY\"=b.\"MD_NAME\" ORDER BY \"TC_COUNT\",\"OFF_NAME\" ";
                dt = ObjCon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }

        }
        public DataTable PrintAbstractReportMonth()
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "SELECT \"TC_CAPACITY\",COUNT(*) AS \"TOTALFAILUIRE\",(SELECT SUBSTR(\"OFF_NAME\",INSTR(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE \"OFF_CODE\"=\"DF_LOC_CODE\") \"OFF_NAME\"  FROM \"TBLTCMASTER\",\"TBLDTCFAILURE\" WHERE \"TC_CODE\"=\"DF_EQUIPMENT_ID\" AND TO_CHAR(\"DF_DATE\",'YYYY/MM')>=to_char(add_months(trunc(NOW()),-8),'YYYY/MM') AND TO_CHAR(\"DF_DATE\",'YYYY/MM')<=TO_CHAR(NOW(),'YYYY/MM')  GROUP BY \"TC_CAPACITY\",\"DF_LOC_CODE\" ORDER BY \"TOTALFAILUIRE\" DESC";
                dt = ObjCon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }
        }



        public List<string> PrintBlob(string sOfficeCode)
        {
            List<string> sImagePaths = new List<string>();
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();
                DataTable dtRoles = new DataTable();
                strQry = "Select \"US_ID\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"='" + sOfficeCode + "' OR \"US_OFFICE_CODE\"=SUBSTR('" + sOfficeCode + "',1,2) OR \"US_OFFICE_CODE\"=SUBSTR('" + sOfficeCode + "',1,3)";
                dt = ObjCon.FetchDataTable(strQry);
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    strQry = "select DISTINCT \"US_SIGN_IMAGE\",\"RO_NAME\",\"RO_ID\" FROM \"TBLUSER\",\"TBLROLES\" where \"US_ID\"='" + dt.Rows[j]["US_ID"] + "' and \"US_ROLE_ID\"=\"RO_ID\"";

                    dtRoles = ObjCon.FetchDataTable(strQry);
                    if (dtRoles.Rows[0]["US_SIGN_IMAGE"] != DBNull.Value)
                    {
                        byte[] myByteArray = (byte[])dtRoles.Rows[0]["US_SIGN_IMAGE"];
                        MemoryStream memStream = new MemoryStream(myByteArray);
                        Image img = System.Drawing.Image.FromStream(memStream);
                        img.Save(System.Web.HttpContext.Current.Server.MapPath("~/DTLMSDocs/") + "\\" + dt.Rows[j]["US_ID"] + ".Jpeg");
                        sImagePaths.Add(System.Web.HttpContext.Current.Server.MapPath("~/DTLMSDocs/") + "\\" + dt.Rows[j]["US_ID"] + ".Jpeg" + "~" + dtRoles.Rows[0]["RO_NAME"] + "~" + dtRoles.Rows[0]["RO_ID"]);
                    }
                    //img.Save(System.IO.Path.GetTempPath() + "\\myImage.Jpeg");
                }

                return sImagePaths;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return sImagePaths;

            }

        }
        public DataTable GetRoles()
        {
            DataTable dtRoles = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "Select \"RO_ID\",\"RO_NAME\" from \"TBLROLES\"";
                dtRoles = ObjCon.FetchDataTable(strQry);
                return dtRoles;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtRoles;

            }
        }

        public DataTable TCFailReport(clsReports objReport)
        {
            DataTable dtFailureDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                #region 1 : Total Failed Tc

                if (objReport.sType == "1")
                {
                    #region commentedinline query
                    //strQry = "	SELECT CAST(\"TRANS_DTC_CODE\" AS TEXT) AS \"DT_CODE\", ";
                    //strQry += " CAST(\"DT_TC_ID\"AS TEXT) \"TC_CODE\",TO_CHAR(\"TRANS_UPDATE_DATE\",'dd-MON-yyyy') \"DF_DATE\", ";
                    //strQry += " \"DF_CONSUMER_COMPLAINT_NUM\" AS \"ComplainNumber\", ";
                    //strQry += " TO_CHAR(\"DF_CONSUMER_COMPLAINT_DATE\",'dd-MON-yy') as \"ComplainDate\", ";
                    //strQry += " COALESCE(trunc(\"WO_AMT\"),0) AS \"COMMISSION\",COALESCE(\"WO_AMT_DECOM\",0) AS \"DECOMMISSION\",\"TRANS_REF_OFF_CODE\" AS \"DF_LOC_CODE\", ";
                    //strQry += " CAST(\"TC_CAPACITY\" AS TEXT) \"TC_CAPACITY\",  \"MD_NAME\" AS \"MD_NAME\",\"EST_NO\" as \"ESTIMATION_NO\", ";
                    //strQry += " \"WO_NO\" as \"WO_NO\",\"TI_INDENT_NO\" as \"TI_INDENT_NO\",\"IN_INV_NO\" as \"IN_INV_NO\", ";
                    //strQry += " \"TR_RI_NO\" as \"TR_RI_NO\", '' AS \"FROMDATE\",'' AS \"TODATE\", TO_CHAR(CURRENT_DATE,'DD/MM/YYYY')\"TODAY\", ";
                    //strQry += " \"DT_FDRSLNO\" as \"FD_FEEDER_CODE\", ";
                    //strQry += " (SELECT \"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\" WHERE \"FD_FEEDER_CODE\"= \"DT_FDRSLNO\") as \"FD_FEEDER_NAME\", ";
                    //strQry += " (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\") \"TC_MAKE_ID\", ";
                    //strQry += " \"WO_DATE\" AS \"WO_DATE\",\"TI_INDENT_DATE\" AS \"TI_INDENT_DATE\",\"IN_DATE\" AS \"IN_DATE\", ";
                    //strQry += " \"TR_RV_DATE\" AS \"TR_RV_DATE\", \"TR_RI_DATE\" AS \"TR_RI_DATE\", ";
                    //strQry += " '" + objReport.sReportType + "' AS \"REPORT_TYPE\",'" + objReport.SelectStage + "' AS \"REPORT_CATEGORY\", ";
                    //strQry += " \"DT_NAME\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"TRANS_REF_OFF_CODE\" AS TEXT),1,1)) \"ZONE\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"TRANS_REF_OFF_CODE\" AS TEXT),1,2)) \"CIRCLE\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"TRANS_REF_OFF_CODE\" AS TEXT),1,3))  \"DIVISION\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"TRANS_REF_OFF_CODE\" AS TEXT),1,4)) \"SUBDIVISION\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"TRANS_REF_OFF_CODE\" AS TEXT),1,5)) \"SECTION\" ";
                    //strQry += " FROM \"TBLPENDINGTRANSACTION\" INNER JOIN \"TBLDTCMAST\" On \"TRANS_DTC_CODE\" = \"DT_CODE\" ";
                    //strQry += " INNER JOIN \"TBLTCMASTER\"  On \"DT_TC_ID\" = \"TC_CODE\"  INNER JOIN \"TBLDTCFAILURE\" on \"DT_CODE\" = \"DF_DTC_CODE\" ";
                    //strQry += " INNER JOIN \"TBLMASTERDATA\" ON \"DF_FAILURE_TYPE\"=\"MD_ID\" AND \"MD_TYPE\"='FT' ";
                    //strQry += " LEFT JOIN \"TBLESTIMATIONDETAILS\"  ON \"DF_ID\"=\"EST_FAILUREID\" LEFT JOIN \"TBLWORKORDER\" On \"DF_ID\"=\"WO_DF_ID\" ";
                    //strQry += " LEFT JOIN \"TBLINDENT\" On  \"WO_SLNO\" = \"TI_WO_SLNO\" LEFT JOIN \"TBLDTCINVOICE\" ON  \"TI_ID\" = \"IN_TI_NO\" ";
                    //strQry += " LEFT JOIN \"TBLTCREPLACE\" On \"WO_SLNO\" = \"TR_WO_SLNO\" ";
                    //strQry += " WHERE \"TRANS_NEXT_ROLE_ID\"<> 0 AND \"TRANS_BO_ID\"<>9 AND \"TRANS_BO_ID\"<>10 AND \"TRANS_BO_ID\"<>14 ";
                    //strQry += " AND \"DF_STATUS_FLAG\" IN (1,4) and \"DF_REPLACE_FLAG\"=0 ";

                    //if (objReport.sFeeder != null)
                    //{
                    //    strQry += " AND  \"DT_FDRSLNO\" ='" + objReport.sFeeder + "' ";
                    //}
                    //if (objReport.sMake != null)
                    //{
                    //    strQry += " AND \"TC_MAKE_ID\"='" + objReport.sMake + "' ";
                    //}
                    //if (objReport.sCapacity != null)
                    //{
                    //    strQry += " AND \"TC_CAPACITY\"='" + objReport.sCapacity + "' ";
                    //}
                    //if (objReport.sGuranteeType != null)
                    //{
                    //    strQry += " AND \"DF_GUARANTY_TYPE\"='" + objReport.sGuranteeType + "' ";
                    //}
                    //if (objReport.sFailType != null)
                    //{
                    //    strQry += " AND \"EST_FAIL_TYPE\"=" + objReport.sFailType + " "; // Note if we hide this from the Front end this condition will not get exicuted.
                    //}
                    //if (objReport.sFailureType != null)
                    //{
                    //    strQry += " AND \"DF_FAILURE_TYPE\"='" + objReport.sFailureType + "' ";
                    //}

                    //if (objReport.sTodate == null && (objReport.sFromDate != null))
                    //{
                    //    strQry += " AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    //    strQry += " and TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                    //}
                    //if (objReport.sFromDate == null && (objReport.sTodate != null))
                    //{
                    //    strQry += " AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    //}
                    //if (objReport.sFromDate == null && objReport.sTodate == null)
                    //{
                    //    strQry += " AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                    //}
                    //if (objReport.sFromDate != null && objReport.sTodate != null)
                    //{
                    //    strQry += " AND \"DF_DATE\" BETWEEN('" + objReport.sFromDate + "') AND('" + objReport.sTodate + "') ";
                    //}
                    //strQry += " AND CAST(\"DF_LOC_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%' ";
                    #endregion
                    NpgsqlCommand command = new NpgsqlCommand("sp_gettcfailreportfortotalfailedtc");
                    command.Parameters.AddWithValue("reporttype", objReport.sReportType);
                    command.Parameters.AddWithValue("selectedstage", objReport.SelectStage);
                    command.Parameters.AddWithValue("sfeeder", Convert.ToString(sFeeder == null ? "" : sFeeder));
                    command.Parameters.AddWithValue("smake", Convert.ToString(sMake == null ? "" : sMake));
                    command.Parameters.AddWithValue("scapacity", Convert.ToString(sCapacity == null ? "" : sCapacity));
                    command.Parameters.AddWithValue("sguranteetype", Convert.ToString(sGuranteeType == null ? "" : sGuranteeType));
                    command.Parameters.AddWithValue("sfailtype", Convert.ToString(sFailType == null ? "" : sFailType));
                    command.Parameters.AddWithValue("sfailuretype", Convert.ToString(sFailureType == null ? "" : sFailureType));
                    command.Parameters.AddWithValue("stodate", Convert.ToString(sTodate == null ? "" : sTodate));
                    command.Parameters.AddWithValue("sfromdate", Convert.ToString(sFromDate == null ? "" : sFromDate));
                    command.Parameters.AddWithValue("sofficecode", Convert.ToString(sOfficeCode == null ? "" : sOfficeCode));
                    command.Parameters.AddWithValue("zone", Convert.ToString(Constants.Zone));
                    command.Parameters.AddWithValue("circle", Convert.ToString(Constants.Circle));
                    command.Parameters.AddWithValue("division", Convert.ToString(Constants.Division));
                    command.Parameters.AddWithValue("subdivision", Convert.ToString(Constants.SubDivision));
                    command.Parameters.AddWithValue("section", Convert.ToString(Constants.Section));
                    dtFailureDetails = ObjCon.FetchDataTable(command);
                    return dtFailureDetails;

                }



                #region 2 : Estimation Pending
                if (objReport.sType == "2")
                {

                    #region
                    //strQry = " SELECT CAST(\"TC_CODE\" AS TEXT)\"TC_CODE\",\"DF_DTC_CODE\" \"DT_CODE\", ";
                    //strQry += " CAST(\"EST_NO\" AS TEXT) AS \"ESTIMATION_NO\",TO_CHAR(\"DF_DATE\",'dd-MON-yy')\"DF_DATE\", ";
                    //strQry += "  \"DF_CONSUMER_COMPLAINT_NUM\" AS \"ComplainNumber\", TO_CHAR(\"DF_CONSUMER_COMPLAINT_DATE\",'dd-MON-yy') as \"ComplainDate\", ";
                    //strQry += " '' AS \"COMMISSION\",'' AS \"DECOMMISSION\", ";
                    //strQry += " \"DF_LOC_CODE\",CAST(\"TC_CAPACITY\" AS TEXT)\"TC_CAPACITY\",\"MD_NAME\", ";
                    //strQry += " '' AS \"WO_NO\",'' as \"TI_INDENT_NO\",'' as \"IN_INV_NO\",'' as \"TR_RI_NO\", ";
                    //strQry += " '" + objReport.sFromDate + "' AS \"FROMDATE\",'" + objReport.sTodate + "' AS \"TODATE\", ";
                    //strQry += " TO_CHAR(CURRENT_DATE,'DD/MM/YYYY')\"TODAY\", ";
                    ////strQry += " substr(CAST(\"DF_DTC_CODE\" AS TEXT),1," + Constants.Feeder + ")\"FD_FEEDER_CODE\",";
                    //strQry += " \"DT_FDRSLNO\" as \"FD_FEEDER_CODE\", ";
                    //strQry += " (SELECT DISTINCT \"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\" WHERE \"FD_FEEDER_CODE\"=\"DT_FDRSLNO\")\"FD_FEEDER_NAME\", ";
                    //strQry += " (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\")\"TC_MAKE_ID\", ";
                    //strQry += " '' AS \"WO_DATE\",'' AS \"TI_INDENT_DATE\",'' AS \"IN_DATE\",'' AS \"TR_RV_DATE\", '' AS \"TR_RI_DATE\", ";
                    //strQry += " '" + objReport.sReportType + "' AS \"REPORT_TYPE\",'" + objReport.SelectStage + "' AS \"REPORT_CATEGORY\", ";
                    //strQry += " (SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=\"DF_DTC_CODE\")\"DT_NAME\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Zone + ")) \"ZONE\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Circle + ")) \"CIRCLE\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Division + ")) \"DIVISION\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ")) \"SUBDIVISION\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Section + ")) \"SECTION\" ";
                    ////strQry += " ,'' AS \"COILTYPE\" "; // Removed as mentioned in Jira Id : DTLMS-1598
                    //strQry += " from \"TBLTCMASTER\",\"TBLMASTERDATA\",\"TBLDTCMAST\", \"TBLDTCFAILURE\" ";
                    //strQry += " LEFT JOIN \"TBLESTIMATIONDETAILS\" ON \"EST_FAILUREID\"=\"DF_ID\" ";
                    //strQry += " where \"DF_EQUIPMENT_ID\"=\"TC_CODE\" and \"DF_DTC_CODE\"=\"DT_CODE\" and \"DF_REPLACE_FLAG\"=0 AND ";
                    //if (objReport.sFeeder != null)
                    //{
                    //    strQry += " \"DT_FDRSLNO\" ='" + objReport.sFeeder + "' AND ";
                    //}
                    //if (objReport.sTodate == null && (objReport.sFromDate != null))
                    //{
                    //    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    //    strQry += " AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                    //}
                    //if (objReport.sFromDate == null && (objReport.sTodate != null))
                    //{
                    //    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    //}
                    //if (objReport.sFromDate == null && objReport.sTodate == null)
                    //{
                    //    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                    //}
                    //if (objReport.sFromDate != null && objReport.sTodate != null)
                    //{
                    //    //strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    //    //strQry += " AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    //    strQry += " \"DF_DATE\" BETWEEN('" + objReport.sFromDate + "') AND('" + objReport.sTodate + "') ";
                    //}

                    //strQry += " AND CAST(\"DF_LOC_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%' ";

                    //if (objReport.sMake != null)
                    //{
                    //    strQry += " AND \"TC_MAKE_ID\"='" + objReport.sMake + "' ";
                    //}
                    //if (objReport.sFailureType != null)
                    //{
                    //    strQry += " AND \"DF_FAILURE_TYPE\"='" + objReport.sFailureType + "' ";
                    //}
                    //if (objReport.sCapacity != null)
                    //{
                    //    strQry += " AND \"TC_CAPACITY\"='" + objReport.sCapacity + "' ";
                    //}
                    //if (objReport.sGuranteeType != null)
                    //{
                    //    strQry += " AND \"DF_GUARANTY_TYPE\"='" + objReport.sGuranteeType + "' ";
                    //}
                    //strQry += " AND \"DF_FAILURE_TYPE\"=\"MD_ID\" AND \"MD_TYPE\"='FT' AND \"DF_STATUS_FLAG\" IN (1,4) ";
                    //strQry += " AND \"DF_ID\" IS NOT NULL AND \"EST_ID\" IS NULL ";
                    //dtFailureDetails = ObjCon.FetchDataTable(strQry);
                    #endregion
                    NpgsqlCommand command = new NpgsqlCommand("sp_gettcfailreportforestimationpending");
                    command.Parameters.AddWithValue("reporttype", objReport.sReportType);
                    command.Parameters.AddWithValue("selectedstage", objReport.SelectStage);
                    command.Parameters.AddWithValue("sfeeder", Convert.ToString(sFeeder == null ? "" : sFeeder));
                    command.Parameters.AddWithValue("smake", Convert.ToString(sMake == null ? "" : sMake));
                    command.Parameters.AddWithValue("scapacity", Convert.ToString(sCapacity == null ? "" : sCapacity));
                    command.Parameters.AddWithValue("sguranteetype", Convert.ToString(sGuranteeType == null ? "" : sGuranteeType));
                    command.Parameters.AddWithValue("sfailtype", Convert.ToString(sFailType == null ? "" : sFailType));
                    command.Parameters.AddWithValue("sfailuretype", Convert.ToString(sFailureType == null ? "" : sFailureType));
                    command.Parameters.AddWithValue("stodate", Convert.ToString(sTodate == null ? "" : sTodate));
                    command.Parameters.AddWithValue("sfromdate", Convert.ToString(sFromDate == null ? "" : sFromDate));
                    command.Parameters.AddWithValue("sofficecode", Convert.ToString(sOfficeCode == null ? "" : sOfficeCode));
                    command.Parameters.AddWithValue("zone", Convert.ToString(Constants.Zone));
                    command.Parameters.AddWithValue("circle", Convert.ToString(Constants.Circle));
                    command.Parameters.AddWithValue("division", Convert.ToString(Constants.Division));
                    command.Parameters.AddWithValue("subdivision", Convert.ToString(Constants.SubDivision));
                    command.Parameters.AddWithValue("section", Convert.ToString(Constants.Section));
                    dtFailureDetails = ObjCon.FetchDataTable(command);
                    return dtFailureDetails;
                }
                #endregion

                #region 3 : Workorder Pending
                if (objReport.sType == "3")
                {
                    #region
                    //strQry = " SELECT CAST(\"TC_CODE\" AS TEXT)\"TC_CODE\",\"DF_DTC_CODE\" \"DT_CODE\", ";
                    //strQry += " \"DF_CONSUMER_COMPLAINT_NUM\" AS \"ComplainNumber\", TO_CHAR(\"DF_CONSUMER_COMPLAINT_DATE\",'dd-MON-yy') as \"ComplainDate\", ";
                    //strQry += " '' AS \"COMMISSION\",'' AS \"DECOMMISSION\", ";
                    //strQry += " TO_CHAR(\"DF_DATE\",'dd-MON-yy')\"DF_DATE\",\"DF_LOC_CODE\",CAST(\"TC_CAPACITY\" AS TEXT)\"TC_CAPACITY\",\"MD_NAME\",\"WO_NO\", ";
                    //strQry += " '' as \"TI_INDENT_NO\",'' as \"IN_INV_NO\",'' as \"TR_RI_NO\", ";
                    //strQry += " '" + objReport.sFromDate + "' AS \"FROMDATE\",'" + objReport.sTodate + "' AS \"TODATE\", ";
                    //strQry += " TO_CHAR(CURRENT_DATE,'DD/MM/YYYY')\"TODAY\", ";
                    ////strQry += " substr(CAST(\"DF_DTC_CODE\" AS TEXT),1," + Constants.Feeder + ")\"FD_FEEDER_CODE\",";
                    //strQry += " \"DT_FDRSLNO\" as \"FD_FEEDER_CODE\", ";
                    //strQry += " (SELECT DISTINCT \"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\" WHERE \"FD_FEEDER_CODE\"=\"DT_FDRSLNO\")\"FD_FEEDER_NAME\", ";
                    //strQry += " (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\")\"TC_MAKE_ID\",cast(\"EST_NO\" as text)\"ESTIMATION_NO\", ";
                    //strQry += " '' AS \"WO_DATE\",'' AS \"TI_INDENT_DATE\",'' AS \"IN_DATE\",'' AS \"TR_RV_DATE\", '' AS \"TR_RI_DATE\", ";
                    //strQry += " '" + objReport.sReportType + "' AS \"REPORT_TYPE\",'" + objReport.SelectStage + "' AS \"REPORT_CATEGORY\", ";
                    //strQry += " (SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=\"DF_DTC_CODE\")\"DT_NAME\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Zone + ")) \"ZONE\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Circle + ")) \"CIRCLE\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Division + ")) \"DIVISION\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ")) \"SUBDIVISION\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Section + ")) \"SECTION\" ";
                    ////strQry += " , (case when \"EST_FAIL_TYPE\" = 1 then 'SINGLECOIL' WHEN \"EST_FAIL_TYPE\" = 2 then 'MULTICOIL' END ) as \"COILTYPE\" "; // Removed as mentioned in Jira Id : DTLMS-1598
                    //strQry += " from \"TBLTCMASTER\",\"TBLMASTERDATA\",\"TBLDTCMAST\",\"TBLESTIMATIONDETAILS\",\"TBLDTCFAILURE\" ";
                    //strQry += " LEFT JOIN  \"TBLWORKORDER\" ON \"DF_ID\"=\"WO_DF_ID\" where  \"DF_EQUIPMENT_ID\"=\"TC_CODE\" AND \"DF_ID\"=\"EST_FAILUREID\" ";
                    //strQry += " and \"DF_DTC_CODE\"=\"DT_CODE\" and \"DF_REPLACE_FLAG\"=0 AND ";
                    //if (objReport.sFeeder != null)
                    //{
                    //    strQry += "   \"DT_FDRSLNO\" ='" + objReport.sFeeder + "'  AND";
                    //}
                    //if (objReport.sFailType != null)
                    //{
                    //    strQry += " \"EST_FAIL_TYPE\"=" + objReport.sFailType + " AND ";
                    //}
                    //if (objReport.sTodate == null && (objReport.sFromDate != null))
                    //{
                    //    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    //    strQry += " AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                    //}
                    //if (objReport.sFromDate == null && (objReport.sTodate != null))
                    //{
                    //    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    //}
                    //if (objReport.sFromDate == null && objReport.sTodate == null)
                    //{
                    //    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                    //}
                    //if (objReport.sFromDate != null && objReport.sTodate != null)
                    //{
                    //    //strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    //    //strQry += " AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    //    strQry += " \"DF_DATE\" BETWEEN('" + objReport.sFromDate + "') AND('" + objReport.sTodate + "') ";
                    //}

                    //strQry += " AND CAST(\"DF_LOC_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%' ";

                    //if (objReport.sMake != null)
                    //{
                    //    strQry += " AND \"TC_MAKE_ID\"='" + objReport.sMake + "' ";
                    //}
                    //if (objReport.sFailureType != null)
                    //{
                    //    strQry += " AND \"DF_FAILURE_TYPE\"='" + objReport.sFailureType + "' ";
                    //}
                    //if (objReport.sCapacity != null)
                    //{
                    //    strQry += " AND \"TC_CAPACITY\"='" + objReport.sCapacity + "' ";
                    //}
                    //if (objReport.sGuranteeType != null)
                    //{
                    //    strQry += " AND \"DF_GUARANTY_TYPE\"='" + objReport.sGuranteeType + "' ";
                    //}
                    //strQry += " AND \"DF_FAILURE_TYPE\"=\"MD_ID\" AND \"MD_TYPE\"='FT' AND \"DF_STATUS_FLAG\" IN (1,4) ";
                    //strQry += " AND \"DF_ID\" IS NOT NULL AND \"WO_SLNO\" IS NULL ";
                    //dtFailureDetails = ObjCon.FetchDataTable(strQry);
                    #endregion
                    //strQry += " order by \"COILTYPE\" ";


                    NpgsqlCommand command = new NpgsqlCommand("sp_gettcfailreport_workorderpending");
                    command.Parameters.AddWithValue("reporttype", objReport.sReportType);
                    command.Parameters.AddWithValue("selectedstage", objReport.SelectStage);
                    command.Parameters.AddWithValue("sfeeder", Convert.ToString(sFeeder == null ? "" : sFeeder));
                    command.Parameters.AddWithValue("smake", Convert.ToString(sMake == null ? "" : sMake));
                    command.Parameters.AddWithValue("scapacity", Convert.ToString(sCapacity == null ? "" : sCapacity));
                    command.Parameters.AddWithValue("sguranteetype", Convert.ToString(sGuranteeType == null ? "" : sGuranteeType));
                    command.Parameters.AddWithValue("sfailtype", Convert.ToString(sFailType == null ? "" : sFailType));
                    command.Parameters.AddWithValue("sfailuretype", Convert.ToString(sFailureType == null ? "" : sFailureType));
                    command.Parameters.AddWithValue("stodate", Convert.ToString(sTodate == null ? "" : sTodate));
                    command.Parameters.AddWithValue("sfromdate", Convert.ToString(sFromDate == null ? "" : sFromDate));
                    command.Parameters.AddWithValue("sofficecode", Convert.ToString(sOfficeCode == null ? "" : sOfficeCode));
                    command.Parameters.AddWithValue("zone", Convert.ToString(Constants.Zone));
                    command.Parameters.AddWithValue("circle", Convert.ToString(Constants.Circle));
                    command.Parameters.AddWithValue("division", Convert.ToString(Constants.Division));
                    command.Parameters.AddWithValue("subdivision", Convert.ToString(Constants.SubDivision));
                    command.Parameters.AddWithValue("section", Convert.ToString(Constants.Section));
                    dtFailureDetails = ObjCon.FetchDataTable(command);
                    return dtFailureDetails;
                }
                #endregion
                #region Not used Logic 4 : Indent Pending
                //if (objReport.sType == "4")
                //{
                //    strQry = " SELECT CAST(\"TC_CODE\" AS TEXT)\"TC_CODE\",\"DF_DTC_CODE\" \"DT_CODE\",\"DF_LOC_CODE\", ";
                //    strQry += " cast(\"WO_AMT\" as text) \"COMMISSION\",cast(\"WO_NO\" as text) \"DECOMMISSION\", ";
                //    strQry += " TO_CHAR(\"DF_DATE\",'dd-MON-yy')\"DF_DATE\",\"TC_CAPACITY\",\"MD_NAME\", ";
                //    strQry += " '' AS \"WO_NO\" ,'' AS \"TI_INDENT_NO\" ,'' as \"IN_INV_NO\",'' as \"TR_RI_NO\", ";
                //    strQry += " '" + objReport.sFromDate + "' AS \"FROMDATE\",'" + objReport.sTodate + "' AS \"TODATE\", ";
                //    strQry += " TO_CHAR(CURRENT_DATE,'DD/MM/YYYY')\"TODAY\",substr(CAST(\"DF_DTC_CODE\" AS TEXT),1," + Constants.Feeder + ")\"FD_FEEDER_CODE\", ";
                //    strQry += " (SELECT DISTINCT \"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\" WHERE \"FD_FEEDER_CODE\"=\"DT_FDRSLNO\")\"FD_FEEDER_NAME\", ";
                //    strQry += " (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\")\"TC_MAKE_ID\", ";
                //    strQry += " '' AS \"TI_INDENT_DATE\",'' AS \"ESTIMATION_NO\",'' AS \"IN_DATE\",'' AS \"TR_RV_DATE\", '' AS \"TR_RI_DATE\", ";
                //    strQry += " '" + objReport.sReportType + "' AS \"REPORT_TYPE\",'" + objReport.sType + "' AS \"REPORT_CATEGORY\", ";
                //    if (objReport.sReportType == "3")
                //    {
                //        strQry += " TO_CHAR(\"WO_DATE\",'DD-MON-YYYY')\"WO_DATE\", ";
                //    }
                //    else
                //    {
                //        strQry += " '' AS \"WO_DATE\", ";
                //    }
                //    strQry += " (SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=\"DF_DTC_CODE\")\"DT_NAME\", ";
                //    strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Zone + ")) \"ZONE\", ";
                //    strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Circle + ")) \"CIRCLE\", ";
                //    strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Division + ")) \"DIVISION\", ";
                //    strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ")) \"SUBDIVISION\", ";
                //    strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Section + ")) \"SECTION\" ";
                //    //strQry += " ,(case when \"EST_FAIL_TYPE\" = 1 then 'SINGLECOIL' WHEN \"EST_FAIL_TYPE\" = 2 then 'MULTICOIL' END ) as \"COILTYPE\" "; // Removed as mentioned in Jira Id : DTLMS-1598
                //    strQry += " from \"TBLTCMASTER\",\"TBLDTCMAST\",\"TBLMASTERDATA\",\"TBLESTIMATIONDETAILS\" , \"TBLDTCFAILURE\" ";
                //    strQry += " LEFT JOIN \"TBLWORKORDER\"  ON \"DF_ID\"=\"WO_DF_ID\" LEFT JOIN \"TBLINDENT\" ON \"WO_SLNO\"=\"TI_WO_SLNO\" ";
                //    strQry += " where \"DF_EQUIPMENT_ID\"=\"TC_CODE\" AND \"DF_ID\"=\"EST_FAILUREID\" and \"DF_DTC_CODE\"=\"DT_CODE\" and \"DF_REPLACE_FLAG\"=0 AND ";
                //    if (objReport.sFeeder != null)
                //    {
                //        strQry += " \"DT_FDRSLNO\" ='" + objReport.sFeeder + "' AND ";
                //    }
                //    if (objReport.sFailType != null)
                //    {
                //        strQry += " \"EST_FAIL_TYPE\"=" + objReport.sFailType + " AND ";

                //    }
                //    if (objReport.sReportType == "3")
                //    {
                //        if (objReport.sTodate == null && (objReport.sFromDate != null))
                //        {
                //            strQry += " TO_CHAR(\"WO_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                //            strQry += " AND TO_CHAR(\"WO_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";

                //        }
                //        if (objReport.sFromDate == null && (objReport.sTodate != null))
                //        {
                //            strQry += " TO_CHAR(\"WO_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                //        }
                //        if (objReport.sFromDate == null && objReport.sTodate == null)
                //        {
                //            strQry += " TO_CHAR(\"WO_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                //        }
                //        if (objReport.sFromDate != null && objReport.sTodate != null)
                //        {
                //            strQry += " TO_CHAR(\"WO_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                //            strQry += " AND TO_CHAR(\"WO_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                //        }
                //    }
                //    else
                //    {
                //        if (objReport.sTodate == null && (objReport.sFromDate != null))
                //        {
                //            strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                //        }
                //        if (objReport.sFromDate == null && (objReport.sTodate != null))
                //        {
                //            strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                //        }
                //        if (objReport.sFromDate == null && objReport.sTodate == null)
                //        {
                //            strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                //        }
                //        if (objReport.sFromDate != null && objReport.sTodate != null)
                //        {
                //            strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                //        }
                //    }
                //    strQry += " AND CAST(\"DF_LOC_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%' ";
                //    if (objReport.sMake != null)
                //    {
                //        strQry += " AND \"TC_MAKE_ID\"='" + objReport.sMake + "' ";
                //    }
                //    if (objReport.sFailureType != null)
                //    {
                //        strQry += " AND \"DF_FAILURE_TYPE\"='" + objReport.sFailureType + "' ";
                //    }
                //    if (objReport.sCapacity != null)
                //    {
                //        strQry += " AND \"TC_CAPACITY\"='" + objReport.sCapacity + "' ";
                //    }
                //    if (objReport.sGuranteeType != null)
                //    {
                //        strQry += " AND \"DF_GUARANTY_TYPE\"='" + objReport.sGuranteeType + "' ";
                //    }

                //    strQry += " AND \"DF_FAILURE_TYPE\"=\"MD_ID\" AND \"MD_TYPE\"='FT' AND \"DF_STATUS_FLAG\" IN (1,4) ";
                //    strQry += " AND \"WO_SLNO\" IS NOT NULL AND \"TI_ID\" IS NULL ";
                //    strQry += " order by \"COILTYPE\" ";
                //}
                #endregion
                #region 5 : invoice Pending
                if (objReport.sType == "5")
                {
                    #region
                    //strQry = " SELECT CAST(\"TC_CODE\" AS TEXT)\"TC_CODE\",\"DF_DTC_CODE\" \"DT_CODE\",TO_CHAR(\"DF_DATE\",'dd-MON-yy')\"DF_DATE\", ";
                    //strQry += " \"DF_CONSUMER_COMPLAINT_NUM\" AS \"ComplainNumber\", TO_CHAR(\"DF_CONSUMER_COMPLAINT_DATE\",'dd-MON-yy') as \"ComplainDate\", ";
                    //strQry += " cast(\"WO_AMT\" as text) \"COMMISSION\",cast(\"WO_AMT_DECOM\" as text) \"DECOMMISSION\",\"DF_LOC_CODE\",\"TC_CAPACITY\",\"MD_NAME\", ";
                    //strQry += " \"WO_NO\" as \"WO_NO\",CAST(\"TI_INDENT_NO\" AS TEXT) ,\"IN_INV_NO\", '' as \"TR_RI_NO\", ";
                    //strQry += " '" + objReport.sFromDate + "' AS \"FROMDATE\",'" + objReport.sTodate + "' AS \"TODATE\", ";
                    //strQry += " TO_CHAR(CURRENT_DATE,'DD/MM/YYYY')\"TODAY\", ";
                    ////strQry += " substr(CAST(\"DF_DTC_CODE\" AS TEXT),1," + Constants.Feeder + ")\"FD_FEEDER_CODE\",";
                    //strQry += " \"DT_FDRSLNO\" as \"FD_FEEDER_CODE\", ";
                    //strQry += " (SELECT DISTINCT \"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\" WHERE \"FD_FEEDER_CODE\"=\"DT_FDRSLNO\")\"FD_FEEDER_NAME\", ";
                    //strQry += " (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\")\"TC_MAKE_ID\", ";
                    //strQry += " CAST(\"EST_NO\" AS TEXT) AS \"ESTIMATION_NO\",\"WO_DATE\" AS \"WO_DATE\",\"IN_DATE\" AS \"IN_DATE\", ";
                    //strQry += " '' AS \"TR_RV_DATE\", '' AS \"TR_RI_DATE\", ";
                    //strQry += " '" + objReport.sReportType + "' AS \"REPORT_TYPE\",'" + objReport.SelectStage + "' AS \"REPORT_CATEGORY\",";
                    //if (objReport.sReportType == "3")
                    //{
                    //    strQry += " TO_CHAR(\"TI_INDENT_DATE\",'DD-MON-YYYY'), ";
                    //}
                    //else
                    //{
                    //    strQry += " '' AS \"TI_INDENT_DATE\", ";
                    //}
                    //strQry += " (SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=\"DF_DTC_CODE\")DT_NAME, ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Zone + ")) \"ZONE\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Circle + ")) \"CIRCLE\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Division + ")) \"DIVISION\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ")) \"SUBDIVISION\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Section + ")) \"SECTION\" ";
                    ////strQry += " ,(case when \"EST_FAIL_TYPE\" = 1 then 'SINGLECOIL' WHEN \"EST_FAIL_TYPE\" = 2 then 'MULTICOIL' END ) as \"COILTYPE\" "; // Removed as mentioned in Jira Id : DTLMS-1598
                    //strQry += " from \"TBLTCMASTER\",\"TBLDTCMAST\",\"TBLMASTERDATA\",\"TBLESTIMATIONDETAILS\",\"TBLDTCFAILURE\" ";
                    //strQry += " LEFT JOIN \"TBLWORKORDER\" ON \"DF_ID\"=\"WO_DF_ID\"  LEFT JOIN \"TBLINDENT\" ON \"WO_SLNO\"=\"TI_WO_SLNO\" ";
                    //strQry += " LEFT JOIN \"TBLDTCINVOICE\" ON \"TI_ID\"=\"IN_TI_NO\" ";
                    //strQry += " where  \"DF_EQUIPMENT_ID\"=\"TC_CODE\" AND \"DF_ID\"=\"EST_FAILUREID\" ";
                    //strQry += " and \"DF_DTC_CODE\"=\"DT_CODE\" and \"DF_REPLACE_FLAG\"=0 AND  ";
                    //if (objReport.sFeeder != null)
                    //{
                    //    strQry += "   \"DT_FDRSLNO\" ='" + objReport.sFeeder + "'  AND";
                    //}
                    //if (objReport.sFailType != null)
                    //{
                    //    strQry += " \"EST_FAIL_TYPE\"=" + objReport.sFailType + " AND";

                    //}
                    //if (objReport.sReportType == "3")
                    //{
                    //    if (objReport.sTodate == null && (objReport.sFromDate != null))
                    //    {
                    //        strQry += " TO_CHAR(\"TI_INDENT_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    //        strQry += " AND TO_CHAR(\"TI_INDENT_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";

                    //    }
                    //    if (objReport.sFromDate == null && (objReport.sTodate != null))
                    //    {
                    //        strQry += " TO_CHAR(\"TI_INDENT_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    //    }
                    //    if (objReport.sFromDate == null && objReport.sTodate == null)
                    //    {
                    //        strQry += " TO_CHAR(\"TI_INDENT_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                    //    }
                    //    if (objReport.sFromDate != null && objReport.sTodate != null)
                    //    {
                    //        //strQry += " TO_CHAR(\"TI_INDENT_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    //        //strQry += " AND TO_CHAR(\"TI_INDENT_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    //        strQry += " \"DF_DATE\" BETWEEN('" + objReport.sFromDate + "') AND('" + objReport.sTodate + "') ";
                    //    }
                    //}
                    //else
                    //{
                    //    if (objReport.sTodate == null && (objReport.sFromDate != null))
                    //    {
                    //        strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    //        strQry += " AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";

                    //    }
                    //    if (objReport.sFromDate == null && (objReport.sTodate != null))
                    //    {
                    //        strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    //    }
                    //    if (objReport.sFromDate == null && objReport.sTodate == null)
                    //    {
                    //        strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                    //    }
                    //    if (objReport.sFromDate != null && objReport.sTodate != null)
                    //    {
                    //        //strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    //        //strQry += " AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    //        strQry += " \"DF_DATE\" BETWEEN('" + objReport.sFromDate + "') AND('" + objReport.sTodate + "') ";
                    //    }
                    //}

                    //strQry += " AND CAST(\"DF_LOC_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%' ";
                    //if (objReport.sMake != null)
                    //{
                    //    strQry += " AND \"TC_MAKE_ID\"='" + objReport.sMake + "' ";
                    //}
                    //if (objReport.sFailureType != null)
                    //{
                    //    strQry += " AND \"DF_FAILURE_TYPE\"='" + objReport.sFailureType + "' ";
                    //}
                    //if (objReport.sCapacity != null)
                    //{
                    //    strQry += " AND \"TC_CAPACITY\"='" + objReport.sCapacity + "' ";
                    //}
                    //if (objReport.sGuranteeType != null)
                    //{
                    //    strQry += " AND \"DF_GUARANTY_TYPE\"='" + objReport.sGuranteeType + "' ";
                    //}
                    //strQry += " AND \"DF_FAILURE_TYPE\"=\"MD_ID\" AND \"MD_TYPE\"='FT' AND \"DF_STATUS_FLAG\" IN (1,4) ";
                    //strQry += " AND \"TI_ID\" IS NOT NULL AND \"IN_NO\" IS NULL ";
                    //dtFailureDetails = ObjCon.FetchDataTable(strQry);
                    #endregion
                    NpgsqlCommand command = new NpgsqlCommand("sp_gettcfailreport_invoicepending");
                    command.Parameters.AddWithValue("reporttype", objReport.sReportType);
                    command.Parameters.AddWithValue("selectedstage", objReport.SelectStage);
                    command.Parameters.AddWithValue("sfeeder", Convert.ToString(sFeeder == null ? "" : sFeeder));
                    command.Parameters.AddWithValue("smake", Convert.ToString(sMake == null ? "" : sMake));
                    command.Parameters.AddWithValue("scapacity", Convert.ToString(sCapacity == null ? "" : sCapacity));
                    command.Parameters.AddWithValue("sguranteetype", Convert.ToString(sGuranteeType == null ? "" : sGuranteeType));
                    command.Parameters.AddWithValue("sfailtype", Convert.ToString(sFailType == null ? "" : sFailType));
                    command.Parameters.AddWithValue("sfailuretype", Convert.ToString(sFailureType == null ? "" : sFailureType));
                    command.Parameters.AddWithValue("stodate", Convert.ToString(sTodate == null ? "" : sTodate));
                    command.Parameters.AddWithValue("sfromdate", Convert.ToString(sFromDate == null ? "" : sFromDate));
                    command.Parameters.AddWithValue("sofficecode", Convert.ToString(sOfficeCode == null ? "" : sOfficeCode));
                    command.Parameters.AddWithValue("zone", Convert.ToString(Constants.Zone));
                    command.Parameters.AddWithValue("circle", Convert.ToString(Constants.Circle));
                    command.Parameters.AddWithValue("division", Convert.ToString(Constants.Division));
                    command.Parameters.AddWithValue("subdivision", Convert.ToString(Constants.SubDivision));
                    command.Parameters.AddWithValue("section", Convert.ToString(Constants.Section));
                    dtFailureDetails = ObjCon.FetchDataTable(command);
                    return dtFailureDetails;
                }
                #endregion

                #region Not used Logic 6 : Decommission Pending
                //if (objReport.sType == "6")
                //{
                //    strQry = " SELECT CAST(\"TC_CODE\" AS TEXT)\"TC_CODE\",\"DF_DTC_CODE\" \"DT_CODE\",\"DF_LOC_CODE\", ";
                //    strQry += " TO_CHAR(\"DF_DATE\",'dd-MON-yy')\"DF_DATE\",cast(\"WO_AMT\" AS text)\"COMMISSION\", ";
                //    strQry += " cast(\"WO_NO_DECOM\" AS text)\"DECOMMISSION\",\"TC_CAPACITY\",\"MD_NAME\", ";
                //    strQry += " '' as \"WO_NO\",'' as \"TI_INDENT_NO\",cast( \"IN_INV_NO\" as text),\"TR_RI_NO\", ";
                //    strQry += " '" + objReport.sFromDate + "' AS \"FROMDATE\",'" + objReport.sTodate + "' AS \"TODATE\", ";
                //    strQry += " TO_CHAR(CURRENT_DATE,'DD/MM/YYYY')\"TODAY\",substr(CAST(\"DF_DTC_CODE\" AS TEXT),1," + Constants.Feeder + ")\"FD_FEEDER_CODE\", ";
                //    strQry += " (SELECT DISTINCT \"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\" WHERE \"FD_FEEDER_CODE\"=\"DT_FDRSLNO\")\"FD_FEEDER_NAME\", ";
                //    strQry += " (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\")\"TC_MAKE_ID\", ";
                //    strQry += " '' AS \"ESTIMATION_NO\",'' AS \"WO_DATE\",'' AS \"TI_INDENT_DATE\",'' AS \"TR_RV_DATE\", '' AS \"TR_RI_DATE\", ";
                //    strQry += " '" + objReport.sReportType + "' AS \"REPORT_TYPE\",'" + objReport.sType + "' AS \"REPORT_CATEGORY\", ";
                //    if (objReport.sReportType == "3")
                //    {
                //        strQry += "TO_CHAR(\"IN_DATE\",'DD-MON-YYYY')IN_DATE,";
                //    }
                //    else
                //    {
                //        strQry += "'' AS \"IN_DATE\",";
                //    }
                //    strQry += " (SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=\"DF_DTC_CODE\")\"DT_NAME\", ";
                //    strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Zone + ")) \"ZONE\", ";
                //    strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\"as TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Circle + ")) \"CIRCLE\", ";
                //    strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\"as TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Division + ")) \"DIVISION\", ";
                //    strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\"as TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ")) \"SUBDIVISION\", ";
                //    strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\"as TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Section + ")) \"SECTION\" ";
                //    //strQry += " ,(case when \"EST_FAIL_TYPE\" = 1 then 'SINGLECOIL' WHEN \"EST_FAIL_TYPE\" = 2 then 'MULTICOIL' END ) as \"COILTYPE\" "; // Removed as mentioned in Jira Id : DTLMS-1598
                //    strQry += " from \"TBLTCMASTER\",\"TBLDTCMAST\",\"TBLMASTERDATA\",\"TBLESTIMATIONDETAILS\",\"TBLDTCFAILURE\" ";
                //    strQry += " LEFT JOIN \"TBLWORKORDER\" ON \"DF_ID\"=\"WO_DF_ID\" LEFT JOIN \"TBLINDENT\" ON \"WO_SLNO\"=\"TI_WO_SLNO\" ";
                //    strQry += " LEFT JOIN \"TBLDTCINVOICE\" ON \"TI_ID\"=\"IN_TI_NO\" LEFT JOIN \"TBLTCREPLACE\" ON \"WO_SLNO\"=\"TR_WO_SLNO\" ";
                //    strQry += " where \"DF_EQUIPMENT_ID\"=\"TC_CODE\"  AND \"DF_ID\"=\"EST_FAILUREID\" and \"DF_DTC_CODE\"=\"DT_CODE\" and \"DF_REPLACE_FLAG\"=0 AND ";
                //    if (objReport.sFeeder != null)
                //    {
                //        strQry += "   \"DT_FDRSLNO\" ='" + objReport.sFeeder + "'  AND";
                //    }
                //    if (objReport.sReportType == "3")
                //    {
                //        if (objReport.sTodate == null && (objReport.sFromDate != null))
                //        {
                //            strQry += " TO_CHAR(\"IN_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                //            strQry += " AND TO_CHAR(\"IN_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";

                //        }
                //        if (objReport.sFromDate == null && (objReport.sTodate != null))
                //        {
                //            strQry += " TO_CHAR(\"IN_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                //        }
                //        if (objReport.sFromDate == null && objReport.sTodate == null)
                //        {
                //            strQry += " TO_CHAR(\"IN_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                //        }
                //        if (objReport.sFromDate != null && objReport.sTodate != null)
                //        {
                //            strQry += " TO_CHAR(\"IN_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                //            strQry += " AND TO_CHAR(\"IN_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                //        }
                //    }
                //    else
                //    {
                //        if (objReport.sTodate == null && (objReport.sFromDate != null))
                //        {
                //            strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                //        }
                //        if (objReport.sFromDate == null && (objReport.sTodate != null))
                //        {
                //            strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                //        }
                //        if (objReport.sFromDate == null && objReport.sTodate == null)
                //        {
                //            strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                //        }
                //        if (objReport.sFromDate != null && objReport.sTodate != null)
                //        {
                //            strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                //        }
                //    }
                //    strQry += " AND CAST(\"DF_LOC_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%' ";
                //    if (objReport.sMake != null)
                //    {
                //        strQry += " AND \"TC_MAKE_ID\"='" + objReport.sMake + "' ";
                //    }
                //    if (objReport.sFailureType != null)
                //    {
                //        strQry += " AND \"DF_FAILURE_TYPE\"='" + objReport.sFailureType + "' ";
                //    }
                //    if (objReport.sCapacity != null)
                //    {
                //        strQry += " AND \"TC_CAPACITY\"='" + objReport.sCapacity + "' ";
                //    }
                //    if (objReport.sGuranteeType != null)
                //    {
                //        strQry += " AND \"DF_GUARANTY_TYPE\"='" + objReport.sGuranteeType + "' ";
                //    }
                //    strQry += " AND \"DF_FAILURE_TYPE\"=\"MD_ID\" AND \"MD_TYPE\"='FT' AND \"DF_STATUS_FLAG\" IN (1,4) ";
                //    strQry += " AND \"EST_FAIL_TYPE\"=2 AND \"TR_ID\" IS NULL and \"WO_SLNO\" is not null ";
                //    strQry += " order by \"COILTYPE\" ";
                //}
                #endregion
                if (objReport.sType == "7")
                {
                    #region
                    //strQry = " SELECT CAST(\"TC_CODE\" AS TEXT)\"TC_CODE\",\"DF_DTC_CODE\" \"DT_CODE\",\"DF_LOC_CODE\", ";
                    //strQry += " TO_CHAR(\"DF_DATE\",'dd-MON-yy')\"DF_DATE\", ";
                    //strQry += " \"DF_CONSUMER_COMPLAINT_NUM\" AS \"ComplainNumber\", TO_CHAR(\"DF_CONSUMER_COMPLAINT_DATE\",'dd-MON-yy') as \"ComplainDate\",";
                    //strQry += " cast(\"WO_AMT\" AS text)\"COMMISSION\",cast(\"WO_AMT_DECOM\" AS text) \"DECOMMISSION\",\"TC_CAPACITY\",\"MD_NAME\", ";
                    //strQry += " \"WO_NO\" as \"WO_NO\",\"TI_INDENT_NO\" as \"TI_INDENT_NO\" ,\"IN_INV_NO\" as \"IN_INV_NO\", ";
                    //strQry += " \"TR_RI_NO\",'" + objReport.sFromDate + "' AS \"FROMDATE\",'" + objReport.sTodate + "' AS \"TODATE\", ";
                    //strQry += " TO_CHAR(CURRENT_DATE,'DD/MM/YYYY')\"TODAY\", ";
                    ////strQry += " substr(CAST(\"DF_DTC_CODE\" AS TEXT),1," + Constants.Feeder + ")\"FD_FEEDER_CODE\",";
                    //strQry += " \"DT_FDRSLNO\" as \"FD_FEEDER_CODE\", ";
                    //strQry += " (SELECT DISTINCT \"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\" WHERE \"FD_FEEDER_CODE\"=\"DT_FDRSLNO\")\"FD_FEEDER_NAME\", ";
                    //strQry += " (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\")\"TC_MAKE_ID\", ";
                    //strQry += " CAST(\"EST_NO\" AS TEXT) AS \"ESTIMATION_NO\",\"WO_DATE\" AS \"WO_DATE\",\"TI_INDENT_DATE\" AS \"TI_INDENT_DATE\", ";
                    //strQry += " \"TR_RV_DATE\" AS \"TR_RV_DATE\", \"IN_DATE\" AS \"IN_DATE\", ";
                    //strQry += " '" + objReport.sReportType + "' AS \"REPORT_TYPE\",'" + objReport.SelectStage + "' AS \"REPORT_CATEGORY\",";

                    //if (objReport.sReportType == "3")
                    //{
                    //    strQry += "TO_CHAR(\"TR_RI_DATE\",'DD-MON-YYYY')\"TR_RI_DATE\",";
                    //}
                    //else
                    //{
                    //    strQry += "'' AS \"TR_RI_DATE\",";
                    //}
                    //strQry += " (SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=\"DF_DTC_CODE\")\"DT_NAME\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Zone + ")) \"ZONE\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" as TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Circle + ")) \"CIRCLE\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" as TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Division + ")) \"DIVISION\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" as TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ")) \"SUBDIVISION\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" as TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Section + ")) \"SECTION\" ";
                    ////strQry += " ,(case when \"EST_FAIL_TYPE\" = 1 then 'SINGLECOIL' WHEN \"EST_FAIL_TYPE\" = 2 then 'MULTICOIL' END ) as \"COILTYPE\" "; // Removed as mentioned in Jira Id : DTLMS-1598
                    //strQry += " from \"TBLTCMASTER\",\"TBLDTCMAST\",\"TBLMASTERDATA\",\"TBLESTIMATIONDETAILS\",\"TBLDTCFAILURE\" ";
                    //strQry += " LEFT JOIN \"TBLWORKORDER\" ON\"DF_ID\"=\"WO_DF_ID\"LEFT JOIN \"TBLINDENT\" ON \"WO_SLNO\"=\"TI_WO_SLNO\" ";
                    //strQry += " LEFT JOIN \"TBLDTCINVOICE\"  ON \"TI_ID\"=\"IN_TI_NO\" LEFT JOIN  \"TBLTCREPLACE\" ON \"WO_SLNO\"=\"TR_WO_SLNO\"";
                    //strQry += " where  \"DF_EQUIPMENT_ID\"=\"TC_CODE\"  AND \"DF_ID\"=\"EST_FAILUREID\" and \"DF_DTC_CODE\"=\"DT_CODE\" and \"DF_REPLACE_FLAG\"=0 AND ";
                    //if (objReport.sFeeder != null)
                    //{
                    //    strQry += "   \"DT_FDRSLNO\" ='" + objReport.sFeeder + "'  AND";
                    //}
                    //if (objReport.sReportType == "3")
                    //{
                    //    if (objReport.sTodate == null && (objReport.sFromDate != null))
                    //    {
                    //        strQry += " TO_CHAR(\"TR_RI_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    //        strQry += " and TO_CHAR(\"TR_RI_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";

                    //    }
                    //    if (objReport.sFromDate == null && (objReport.sTodate != null))
                    //    {
                    //        strQry += " TO_CHAR(\"TR_RI_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    //    }
                    //    if (objReport.sFromDate == null && objReport.sTodate == null)
                    //    {
                    //        strQry += " TO_CHAR(\"TR_RI_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                    //    }
                    //    if (objReport.sFromDate != null && objReport.sTodate != null)
                    //    {
                    //        //strQry += " TO_CHAR(\"TR_RI_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    //        //strQry += " AND TO_CHAR(\"TR_RI_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    //        strQry += " \"DF_DATE\" BETWEEN('" + objReport.sFromDate + "') AND('" + objReport.sTodate + "') ";
                    //    }
                    //}
                    //else
                    //{
                    //    if (objReport.sTodate == null && (objReport.sFromDate != null))
                    //    {
                    //        strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    //        strQry += " and TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";

                    //    }
                    //    if (objReport.sFromDate == null && (objReport.sTodate != null))
                    //    {
                    //        strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    //    }
                    //    if (objReport.sFromDate == null && objReport.sTodate == null)
                    //    {
                    //        strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                    //    }
                    //    if (objReport.sFromDate != null && objReport.sTodate != null)
                    //    {
                    //        //strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    //        //strQry += " AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                    //        strQry += " \"DF_DATE\" BETWEEN('" + objReport.sFromDate + "') AND('" + objReport.sTodate + "') ";
                    //    }
                    //}

                    //strQry += "AND CAST(\"DF_LOC_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%'";

                    //if (objReport.sMake != null)
                    //{
                    //    strQry += "AND \"TC_MAKE_ID\"='" + objReport.sMake + "'";
                    //}
                    //if (objReport.sFailureType != null)
                    //{
                    //    strQry += "AND \"DF_FAILURE_TYPE\"='" + objReport.sFailureType + "'";
                    //}
                    //if (objReport.sCapacity != null)
                    //{
                    //    strQry += "AND \"TC_CAPACITY\"='" + objReport.sCapacity + "' ";
                    //}
                    //if (objReport.sGuranteeType != null)
                    //{
                    //    strQry += "AND \"DF_GUARANTY_TYPE\"='" + objReport.sGuranteeType + "' ";
                    //}
                    //strQry += " AND \"DF_FAILURE_TYPE\"=\"MD_ID\" AND \"MD_TYPE\"='FT' AND \"DF_STATUS_FLAG\" IN (1,4) ";
                    //strQry += " AND \"EST_FAIL_TYPE\"=2 AND \"TR_RV_DATE\" IS NULL AND \"WO_SLNO\" is not null ";
                    //dtFailureDetails = ObjCon.FetchDataTable(strQry);
                    //strQry += " order by \"COILTYPE\" ";
                    #endregion
                    NpgsqlCommand command = new NpgsqlCommand("sp_gettcfailreport_returninvoicepending");
                    command.Parameters.AddWithValue("reporttype", objReport.sReportType);
                    command.Parameters.AddWithValue("selectedstage", objReport.SelectStage);
                    command.Parameters.AddWithValue("sfeeder", Convert.ToString(sFeeder == null ? "" : sFeeder));
                    command.Parameters.AddWithValue("smake", Convert.ToString(sMake == null ? "" : sMake));
                    command.Parameters.AddWithValue("scapacity", Convert.ToString(sCapacity == null ? "" : sCapacity));
                    command.Parameters.AddWithValue("sguranteetype", Convert.ToString(sGuranteeType == null ? "" : sGuranteeType));
                    command.Parameters.AddWithValue("sfailtype", Convert.ToString(sFailType == null ? "" : sFailType));
                    command.Parameters.AddWithValue("sfailuretype", Convert.ToString(sFailureType == null ? "" : sFailureType));
                    command.Parameters.AddWithValue("stodate", Convert.ToString(sTodate == null ? "" : sTodate));
                    command.Parameters.AddWithValue("sfromdate", Convert.ToString(sFromDate == null ? "" : sFromDate));
                    command.Parameters.AddWithValue("sofficecode", Convert.ToString(sOfficeCode == null ? "" : sOfficeCode));
                    command.Parameters.AddWithValue("zone", Convert.ToString(Constants.Zone));
                    command.Parameters.AddWithValue("circle", Convert.ToString(Constants.Circle));
                    command.Parameters.AddWithValue("division", Convert.ToString(Constants.Division));
                    command.Parameters.AddWithValue("subdivision", Convert.ToString(Constants.SubDivision));
                    command.Parameters.AddWithValue("section", Convert.ToString(Constants.Section));
                    dtFailureDetails = ObjCon.FetchDataTable(command);
                    return dtFailureDetails;
                }

                if (objReport.sType == "8")
                {
                    #region
                    //strQry = " SELECT CAST(\"TC_CODE\" AS TEXT)\"TC_CODE\",\"DF_DTC_CODE\" \"DT_CODE\",\"DF_LOC_CODE\", ";
                    //strQry += " TO_CHAR(\"DF_DATE\",'dd-MON-yy')\"DF_DATE\", ";
                    //strQry += " \"DF_CONSUMER_COMPLAINT_NUM\" AS \"ComplainNumber\", TO_CHAR(\"DF_CONSUMER_COMPLAINT_DATE\",'dd-MON-yy') as \"ComplainDate\", ";
                    //strQry += " cast(\"WO_AMT\" AS text)\"COMMISSION\",cast(\"WO_AMT_DECOM\" AS text) \"DECOMMISSION\",\"TC_CAPACITY\",\"MD_NAME\", ";
                    //strQry += " \"WO_NO\" as \"WO_NO\",\"TI_INDENT_NO\" as \"TI_INDENT_NO\" ,\"IN_INV_NO\" as \"IN_INV_NO\", ";
                    //strQry += " \"TR_RI_NO\",'" + objReport.sFromDate + "' AS \"FROMDATE\",'" + objReport.sTodate + "' AS \"TODATE\", ";

                    //strQry += " TO_CHAR(CURRENT_DATE,'DD/MM/YYYY')\"TODAY\", ";
                    ////strQry += " substr(CAST(\"DF_DTC_CODE\" AS TEXT),1," + Constants.Feeder + ")\"FD_FEEDER_CODE\",";
                    //strQry += " \"DT_FDRSLNO\" as \"FD_FEEDER_CODE\", ";
                    //strQry += " (SELECT DISTINCT \"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\" WHERE \"FD_FEEDER_CODE\"=\"DT_FDRSLNO\")\"FD_FEEDER_NAME\", ";
                    //strQry += " (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\")\"TC_MAKE_ID\", ";
                    //strQry += " CAST(\"EST_NO\" AS TEXT) AS \"ESTIMATION_NO\",\"WO_DATE\" AS \"WO_DATE\", ";
                    //strQry += " \"TI_INDENT_DATE\" AS \"TI_INDENT_DATE\",\"IN_DATE\" AS \"IN_DATE\", \"TR_RI_DATE\" AS \"TR_RI_DATE\", ";
                    //strQry += " '" + objReport.sReportType + "' AS \"REPORT_TYPE\",'" + objReport.SelectStage + "' AS \"REPORT_CATEGORY\", ";

                    //if (objReport.sReportType == "3")
                    //{
                    //    strQry += " TO_CHAR(\"TR_RV_DATE\",'DD-MON-YYYY')\"TR_RV_DATE\", ";
                    //}
                    //else
                    //{
                    //    strQry += " \"TR_RV_DATE\" AS \"TR_RV_DATE\", ";
                    //}
                    //strQry += " (SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=\"DF_DTC_CODE\")\"DT_NAME\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Zone + ")) \"ZONE\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\"as TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Circle + ")) \"CIRCLE\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\"as TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Division + ")) \"DIVISION\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\"as TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ")) \"SUBDIVISION\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\"as TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Section + ")) \"SECTION\" ";
                    ////strQry += " ,(case when \"EST_FAIL_TYPE\" = 1 then 'SINGLECOIL' WHEN \"EST_FAIL_TYPE\" = 2 then 'MULTICOIL' END ) as \"COILTYPE\" "; // Removed as mentioned in Jira Id : DTLMS-1598
                    //strQry += " from \"TBLTCMASTER\",\"TBLDTCMAST\",\"TBLMASTERDATA\",\"TBLESTIMATIONDETAILS\" , \"TBLDTCFAILURE\" ";
                    //strQry += " LEFT JOIN \"TBLWORKORDER\" ON\"DF_ID\"=\"WO_DF_ID\"LEFT JOIN \"TBLINDENT\" ON \"WO_SLNO\"=\"TI_WO_SLNO\" ";
                    //strQry += " LEFT JOIN \"TBLDTCINVOICE\"  ON \"TI_ID\"=\"IN_TI_NO\" LEFT JOIN  \"TBLTCREPLACE\" ON \"WO_SLNO\"=\"TR_WO_SLNO\"";
                    //strQry += " where \"DF_EQUIPMENT_ID\"=\"TC_CODE\"  AND \"DF_ID\"=\"EST_FAILUREID\" and \"DF_DTC_CODE\"=\"DT_CODE\" and \"DF_REPLACE_FLAG\"=0 AND ";
                    //if (objReport.sFeeder != null)
                    //{
                    //    strQry += "   \"DT_FDRSLNO\" ='" + objReport.sFeeder + "'  AND";
                    //}
                    //if (objReport.sFailType != null)
                    //{
                    //    strQry += " \"EST_FAIL_TYPE\"=" + objReport.sFailType + " AND";
                    //}
                    //if (objReport.sReportType == "3")
                    //{
                    //    if (objReport.sTodate == null && (objReport.sFromDate != null))
                    //    {
                    //        strQry += " TO_CHAR(\"TR_RV_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    //        strQry += " and TO_CHAR(\"TR_RV_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                    //    }
                    //    if (objReport.sFromDate == null && (objReport.sTodate != null))
                    //    {
                    //        strQry += " TO_CHAR(\"TR_RV_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    //    }
                    //    if (objReport.sFromDate == null && objReport.sTodate == null)
                    //    {
                    //        strQry += " TO_CHAR(\"TR_RV_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                    //    }
                    //    if (objReport.sFromDate != null && objReport.sTodate != null)
                    //    {
                    //        //strQry += " TO_CHAR(\"TR_RV_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    //        //strQry += " AND TO_CHAR(\"TR_RV_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    //        strQry += " \"DF_DATE\" BETWEEN('" + objReport.sFromDate + "') AND('" + objReport.sTodate + "') ";
                    //    }
                    //}
                    //else
                    //{
                    //    if (objReport.sTodate == null && (objReport.sFromDate != null))
                    //    {
                    //        strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    //        strQry += " and TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                    //    }
                    //    if (objReport.sFromDate == null && (objReport.sTodate != null))
                    //    {
                    //        strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    //    }
                    //    if (objReport.sFromDate == null && objReport.sTodate == null)
                    //    {
                    //        strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                    //    }
                    //    if (objReport.sFromDate != null && objReport.sTodate != null)
                    //    {
                    //        //strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    //        //strQry += " AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    //        strQry += " \"DF_DATE\" BETWEEN('" + objReport.sFromDate + "') AND('" + objReport.sTodate + "') ";
                    //    }
                    //}
                    //strQry += "AND CAST(\"DF_LOC_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%'";
                    //if (objReport.sMake != null)
                    //{
                    //    strQry += "AND \"TC_MAKE_ID\"='" + objReport.sMake + "'";
                    //}
                    //if (objReport.sFailureType != null)
                    //{
                    //    strQry += "AND \"DF_FAILURE_TYPE\"='" + objReport.sFailureType + "'";
                    //}
                    //if (objReport.sCapacity != null)
                    //{
                    //    strQry += "AND \"TC_CAPACITY\"='" + objReport.sCapacity + "' ";
                    //}
                    //if (objReport.sGuranteeType != null)
                    //{
                    //    strQry += "AND \"DF_GUARANTY_TYPE\"='" + objReport.sGuranteeType + "' ";
                    //}
                    //strQry += " AND \"DF_FAILURE_TYPE\"=\"MD_ID\" AND \"MD_TYPE\"='FT' AND \"DF_STATUS_FLAG\" IN (1,4) ";
                    //strQry += " AND \"WO_SLNO\" IS NOT NULL  AND \"TR_RI_NO\" IS NOT NULL AND \"TR_RV_NO\" IS NOT NULL ";
                    //strQry += " AND \"TR_INVENTORY_QTY\" IS NULL and \"EST_FAIL_TYPE\"=2 ";
                    ////strQry += " order by \"COILTYPE\" ";
                    //dtFailureDetails = ObjCon.FetchDataTable(strQry);
                    #endregion
                    NpgsqlCommand command = new NpgsqlCommand("sp_gettcfailreport_crpending");
                    command.Parameters.AddWithValue("reporttype", objReport.sReportType);
                    command.Parameters.AddWithValue("selectedstage", objReport.SelectStage);
                    command.Parameters.AddWithValue("sfeeder", Convert.ToString(sFeeder == null ? "" : sFeeder));
                    command.Parameters.AddWithValue("smake", Convert.ToString(sMake == null ? "" : sMake));
                    command.Parameters.AddWithValue("scapacity", Convert.ToString(sCapacity == null ? "" : sCapacity));
                    command.Parameters.AddWithValue("sguranteetype", Convert.ToString(sGuranteeType == null ? "" : sGuranteeType));
                    command.Parameters.AddWithValue("sfailtype", Convert.ToString(sFailType == null ? "" : sFailType));
                    command.Parameters.AddWithValue("sfailuretype", Convert.ToString(sFailureType == null ? "" : sFailureType));
                    command.Parameters.AddWithValue("stodate", Convert.ToString(sTodate == null ? "" : sTodate));
                    command.Parameters.AddWithValue("sfromdate", Convert.ToString(sFromDate == null ? "" : sFromDate));
                    command.Parameters.AddWithValue("sofficecode", Convert.ToString(sOfficeCode == null ? "" : sOfficeCode));
                    command.Parameters.AddWithValue("zone", Convert.ToString(Constants.Zone));
                    command.Parameters.AddWithValue("circle", Convert.ToString(Constants.Circle));
                    command.Parameters.AddWithValue("division", Convert.ToString(Constants.Division));
                    command.Parameters.AddWithValue("subdivision", Convert.ToString(Constants.SubDivision));
                    command.Parameters.AddWithValue("section", Convert.ToString(Constants.Section));
                    dtFailureDetails = ObjCon.FetchDataTable(command);
                    return dtFailureDetails;
                }

                if (objReport.sType == "9")
                {
                    #region
                    //strQry = " SELECT CAST(\"TC_CODE\" AS TEXT)\"TC_CODE\",\"DF_DTC_CODE\" \"DT_CODE\",\"DF_LOC_CODE\", ";
                    //strQry += " TO_CHAR(\"DF_DATE\",'dd-MON-yy')\"DF_DATE\", ";
                    //strQry += " \"DF_CONSUMER_COMPLAINT_NUM\" AS \"ComplainNumber\", TO_CHAR(\"DF_CONSUMER_COMPLAINT_DATE\",'dd-MON-yy') as \"ComplainDate\",";
                    //strQry += " cast(\"WO_AMT\" AS text)\"COMMISSION\",cast(\"WO_AMT_DECOM\" AS text)\"DECOMMISSION\",\"TC_CAPACITY\",\"MD_NAME\",\"WO_NO\", ";
                    //strQry += " \"TI_INDENT_NO\" as \"TI_INDENT_NO\" ,\"IN_INV_NO\" as \"IN_INV_NO\",\"TR_RI_NO\" as \"TR_RI_NO\", ";
                    //strQry += " '" + objReport.sFromDate + "' AS \"FROMDATE\",'" + objReport.sTodate + "' AS \"TODATE\", ";
                    //strQry += " TO_CHAR(CURRENT_DATE,'DD/MM/YYYY')\"TODAY\", ";
                    ////strQry += " substr(CAST(\"DF_DTC_CODE\" AS TEXT),1," + Constants.Feeder + ")\"FD_FEEDER_CODE\", ";
                    //strQry += " \"DT_FDRSLNO\" as \"FD_FEEDER_CODE\", ";
                    //strQry += " (SELECT DISTINCT \"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\" WHERE \"FD_FEEDER_CODE\"=\"DT_FDRSLNO\")\"FD_FEEDER_NAME\", ";
                    //strQry += " (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\")\"TC_MAKE_ID\", ";
                    //strQry += " CAST(\"EST_NO\" AS TEXT) AS \"ESTIMATION_NO\",\"WO_DATE\" AS \"WO_DATE\",\"TI_INDENT_DATE\" AS \"TI_INDENT_DATE\",\"IN_DATE\" AS \"IN_DATE\",\"TR_RV_DATE\" AS \"TR_RV_DATE\", \"TR_RI_DATE\" AS \"TR_RI_DATE\", ";
                    //strQry += " '" + objReport.sReportType + "' AS \"REPORT_TYPE\",'" + objReport.SelectStage + "' AS \"REPORT_CATEGORY\", ";
                    //strQry += " (SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=\"DF_DTC_CODE\")\"DT_NAME\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Zone + ")) \"ZONE\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\"as TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Circle + ")) \"CIRCLE\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" as TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Division + ")) \"DIVISION\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\"as TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ")) \"SUBDIVISION\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\"as TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Section + ")) \"SECTION\" ";
                    ////strQry += " ,(case when \"EST_FAIL_TYPE\" = 1 then 'SINGLECOIL' WHEN \"EST_FAIL_TYPE\" = 2 then 'MULTICOIL' END ) as \"COILTYPE\" "; // Removed as mentioned in Jira Id : DTLMS-1598
                    //strQry += " from \"TBLTCMASTER\",\"TBLESTIMATIONDETAILS\" , \"TBLDTCFAILURE\",\"TBLDTCMAST\",\"TBLMASTERDATA\",\"TBLWORKORDER\", ";
                    //strQry += " \"TBLTCREPLACE\",\"TBLINDENT\",\"TBLDTCINVOICE\",\"TBLTRANSDTCMAPPING\" ";
                    //strQry += " where \"TM_DTC_ID\"=\"DF_DTC_CODE\" and \"DF_EQUIPMENT_ID\"=\"TC_CODE\"  AND \"DF_ID\"=\"EST_FAILUREID\" ";
                    //strQry += " and \"DF_DTC_CODE\"=\"DT_CODE\" and \"DF_REPLACE_FLAG\"=1 AND ";
                    //if (objReport.sFeeder != null)
                    //{
                    //    strQry += "   \"DT_FDRSLNO\" ='" + objReport.sFeeder + "'  AND";
                    //}
                    //if (objReport.sFailType != null)
                    //{
                    //    strQry += " \"EST_FAIL_TYPE\"=" + objReport.sFailType + " AND";

                    //}
                    //if (objReport.sReportType == "3")
                    //{
                    //    if (objReport.sTodate == null && (objReport.sFromDate != null))
                    //    {
                    //        strQry += " TO_CHAR(\"DF_REP_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    //        strQry += " and TO_CHAR(\"DF_REP_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";

                    //    }
                    //    if (objReport.sFromDate == null && (objReport.sTodate != null))
                    //    {
                    //        strQry += " TO_CHAR(\"DF_REP_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    //    }
                    //    if (objReport.sFromDate == null && objReport.sTodate == null)
                    //    {
                    //        strQry += " TO_CHAR(\"DF_REP_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                    //    }
                    //    if (objReport.sFromDate != null && objReport.sTodate != null)
                    //    {
                    //        //strQry += " TO_CHAR(\"DF_REP_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    //        //strQry += " AND TO_CHAR(\"DF_REP_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    //        strQry += " \"DF_DATE\" BETWEEN('" + objReport.sFromDate + "') AND('" + objReport.sTodate + "') ";
                    //    }
                    //}
                    //else
                    //{
                    //    if (objReport.sTodate == null && (objReport.sFromDate != null))
                    //    {
                    //        strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    //        strQry += " and TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";

                    //    }
                    //    if (objReport.sFromDate == null && (objReport.sTodate != null))
                    //    {
                    //        strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    //    }
                    //    if (objReport.sFromDate == null && objReport.sTodate == null)
                    //    {
                    //        strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                    //    }
                    //    if (objReport.sFromDate != null && objReport.sTodate != null)
                    //    {
                    //        //strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    //        //strQry += " AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    //        strQry += " \"DF_DATE\" BETWEEN('" + objReport.sFromDate + "') AND('" + objReport.sTodate + "') ";
                    //    }
                    //}
                    //strQry += "AND CAST(\"DF_LOC_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%'";
                    //if (objReport.sMake != null)
                    //{
                    //    strQry += "AND \"TC_MAKE_ID\"='" + objReport.sMake + "'";
                    //}
                    //if (objReport.sFailureType != null)
                    //{
                    //    strQry += "AND \"DF_FAILURE_TYPE\"='" + objReport.sFailureType + "'";
                    //}
                    //if (objReport.sCapacity != null)
                    //{
                    //    strQry += "AND \"TC_CAPACITY\"='" + objReport.sCapacity + "' ";
                    //}
                    //if (objReport.sGuranteeType != null)
                    //{
                    //    strQry += "AND \"DF_GUARANTY_TYPE\"='" + objReport.sGuranteeType + "' ";
                    //}
                    //strQry += " AND \"DF_FAILURE_TYPE\"=\"MD_ID\" AND \"DF_ID\"=\"TBLWORKORDER\".\"WO_DF_ID\" ";
                    //strQry += " AND \"WO_SLNO\"=\"TI_WO_SLNO\" AND \"TI_ID\"=\"IN_TI_NO\" AND \"WO_SLNO\"=\"TR_WO_SLNO\" ";
                    //strQry += " AND \"MD_TYPE\"='FT' AND \"DF_STATUS_FLAG\" IN (1,4) and \"TM_LIVE_FLAG\"=1 ";
                    ////strQry += " order by \"COILTYPE\" ";
                    //dtFailureDetails = ObjCon.FetchDataTable(strQry);
                    #endregion

                    NpgsqlCommand command = new NpgsqlCommand("sp_gettcfailreport_crcompleted");
                    command.Parameters.AddWithValue("reporttype", objReport.sReportType);
                    command.Parameters.AddWithValue("selectedstage", objReport.SelectStage);
                    command.Parameters.AddWithValue("sfeeder", Convert.ToString(sFeeder == null ? "" : sFeeder));
                    command.Parameters.AddWithValue("smake", Convert.ToString(sMake == null ? "" : sMake));
                    command.Parameters.AddWithValue("scapacity", Convert.ToString(sCapacity == null ? "" : sCapacity));
                    command.Parameters.AddWithValue("sguranteetype", Convert.ToString(sGuranteeType == null ? "" : sGuranteeType));
                    command.Parameters.AddWithValue("sfailtype", Convert.ToString(sFailType == null ? "" : sFailType));
                    command.Parameters.AddWithValue("sfailuretype", Convert.ToString(sFailureType == null ? "" : sFailureType));
                    command.Parameters.AddWithValue("stodate", Convert.ToString(sTodate == null ? "" : sTodate));
                    command.Parameters.AddWithValue("sfromdate", Convert.ToString(sFromDate == null ? "" : sFromDate));
                    command.Parameters.AddWithValue("sofficecode", Convert.ToString(sOfficeCode == null ? "" : sOfficeCode));
                    command.Parameters.AddWithValue("zone", Convert.ToString(Constants.Zone));
                    command.Parameters.AddWithValue("circle", Convert.ToString(Constants.Circle));
                    command.Parameters.AddWithValue("division", Convert.ToString(Constants.Division));
                    command.Parameters.AddWithValue("subdivision", Convert.ToString(Constants.SubDivision));
                    command.Parameters.AddWithValue("section", Convert.ToString(Constants.Section));
                    dtFailureDetails = ObjCon.FetchDataTable(command);
                    return dtFailureDetails;
                }

                if (objReport.sType == "10")
                {
                    #region
                    //strQry = " SELECT CASE WHEN \"DF_REPLACE_FLAG\"='0' THEN 'Pending' ELSE 'Completed' END \"STATUS\", ";
                    //strQry += " CAST(\"TC_CODE\" AS TEXT)\"TC_CODE\",\"DF_DTC_CODE\" \"DT_CODE\",\"DF_LOC_CODE\", ";
                    //strQry += " TO_CHAR(\"DF_DATE\",'dd-MON-yy')\"DF_DATE\", ";
                    //strQry += " \"DF_CONSUMER_COMPLAINT_NUM\" AS \"ComplainNumber\", TO_CHAR(\"DF_CONSUMER_COMPLAINT_DATE\",'dd-MON-yy') as \"ComplainDate\",";
                    //strQry += " cast(\"WO_AMT\" AS text) \"COMMISSION\",cast(\"WO_AMT_DECOM\" AS text) \"DECOMMISSION\",\"TC_CAPACITY\",\"MD_NAME\",\"WO_NO\" as \"WO_NO\", ";
                    //strQry += " \"TI_INDENT_NO\" as \"TI_INDENT_NO\" ,\"IN_INV_NO\" as \"IN_INV_NO\",\"TR_RI_NO\" AS \"TR_RI_NO\",'" + objReport.sFromDate + "' AS \"FROMDATE\", ";
                    //strQry += " '" + objReport.sTodate + "' AS \"TODATE\",TO_CHAR(CURRENT_DATE,'DD/MM/YYYY')\"TODAY\", ";
                    ////strQry += " substr(CAST(\"DF_DTC_CODE\" AS TEXT),1," + Constants.Feeder + ")\"FD_FEEDER_CODE\", ";
                    //strQry += " \"DT_FDRSLNO\" as \"FD_FEEDER_CODE\", ";
                    //strQry += " (SELECT DISTINCT \"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\" WHERE \"FD_FEEDER_CODE\"=\"DT_FDRSLNO\")\"FD_FEEDER_NAME\", ";
                    //strQry += " (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\")\"TC_MAKE_ID\", ";
                    //strQry += " \"EST_NO\" AS \"ESTIMATION_NO\",\"WO_DATE\" AS \"WO_DATE\",\"TI_INDENT_DATE\" AS \"TI_INDENT_DATE\",\"IN_DATE\" AS \"IN_DATE\", \"TR_RI_DATE\" AS \"TR_RI_DATE\",\"TR_RV_DATE\" AS \"TR_RV_DATE\", ";
                    //strQry += " '" + objReport.sReportType + "' AS \"REPORT_TYPE\",'" + objReport.SelectStage + "' AS \"REPORT_CATEGORY\",";
                    //strQry += " (SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=\"DF_DTC_CODE\")\"DT_NAME\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Zone + ")) \"ZONE\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" as TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Circle + ")) \"CIRCLE\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" as TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Division + ")) \"DIVISION\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" as TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ")) \"SUBDIVISION\", ";
                    //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" as TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Section + ")) \"SECTION\" ";
                    ////strQry += " ,(case when \"EST_FAIL_TYPE\" = 1 then 'SINGLECOIL' WHEN \"EST_FAIL_TYPE\" = 2 then 'MULTICOIL' END ) as \"COILTYPE\" "; // Removed as mentioned in Jira Id : DTLMS-1598
                    //strQry += " from \"TBLTCMASTER\",\"TBLDTCMAST\",\"TBLMASTERDATA\",\"TBLDTCFAILURE\" LEFT JOIN \"TBLESTIMATIONDETAILS\" ON \"DF_ID\"=\"EST_FAILUREID\" ";
                    //strQry += " LEFT JOIN \"TBLWORKORDER\" ON\"DF_ID\"=\"WO_DF_ID\"LEFT JOIN \"TBLINDENT\" ON \"WO_SLNO\"=\"TI_WO_SLNO\" ";
                    //strQry += " LEFT JOIN \"TBLDTCINVOICE\"  ON \"TI_ID\"=\"IN_TI_NO\" LEFT JOIN  \"TBLTCREPLACE\" ON \"WO_SLNO\"=\"TR_WO_SLNO\" ";
                    //strQry += "where  \"DF_EQUIPMENT_ID\"=\"TC_CODE\" and \"DF_DTC_CODE\"=\"DT_CODE\" AND ";
                    //if (objReport.sFeeder != null)
                    //{
                    //    strQry += "   \"DT_FDRSLNO\" ='" + objReport.sFeeder + "'  AND";
                    //}
                    //if (objReport.sFailType != null)
                    //{
                    //    strQry += " \"EST_FAIL_TYPE\"=" + objReport.sFailType + " AND";
                    //}
                    //if (objReport.sTodate == null && (objReport.sFromDate != null))
                    //{
                    //    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    //    strQry += " and TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                    //}
                    //if (objReport.sFromDate == null && (objReport.sTodate != null))
                    //{
                    //    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    //}
                    //if (objReport.sFromDate == null && objReport.sTodate == null)
                    //{
                    //    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                    //}
                    //if (objReport.sFromDate != null && objReport.sTodate != null)
                    //{
                    //    //strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    //    //strQry += " AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    //    strQry += " \"DF_DATE\" BETWEEN('" + objReport.sFromDate + "') AND('" + objReport.sTodate + "') ";
                    //}
                    //strQry += "AND CAST(\"DF_LOC_CODE\"AS TEXT) LIKE '" + sOfficeCode + "%'";
                    //if (objReport.sMake != null)
                    //{
                    //    strQry += "AND \"TC_MAKE_ID\"='" + objReport.sMake + "'";
                    //}
                    //if (objReport.sFailureType != null)
                    //{
                    //    strQry += "AND \"DF_FAILURE_TYPE\"='" + objReport.sFailureType + "'";
                    //}
                    //if (objReport.sCapacity != null)
                    //{
                    //    strQry += "AND \"TC_CAPACITY\"='" + objReport.sCapacity + "' ";
                    //}
                    //if (objReport.sGuranteeType != null)
                    //{
                    //    strQry += "AND \"DF_GUARANTY_TYPE\"='" + objReport.sGuranteeType + "' ";
                    //}
                    //strQry += " AND \"DF_FAILURE_TYPE\"=\"MD_ID\" AND \"MD_TYPE\"='FT' ";
                    //strQry += " AND \"DF_STATUS_FLAG\" IN (1,4) ";
                    ////strQry += " order by \"COILTYPE\" ";
                    //dtFailureDetails = ObjCon.FetchDataTable(strQry);
                    #endregion
                    NpgsqlCommand command = new NpgsqlCommand("sp_gettcfailreport_forallstages");
                    command.Parameters.AddWithValue("reporttype", objReport.sReportType);
                    command.Parameters.AddWithValue("selectedstage", objReport.SelectStage);
                    command.Parameters.AddWithValue("sfeeder", Convert.ToString(sFeeder == null ? "" : sFeeder));
                    command.Parameters.AddWithValue("smake", Convert.ToString(sMake == null ? "" : sMake));
                    command.Parameters.AddWithValue("scapacity", Convert.ToString(sCapacity == null ? "" : sCapacity));
                    command.Parameters.AddWithValue("sguranteetype", Convert.ToString(sGuranteeType == null ? "" : sGuranteeType));
                    command.Parameters.AddWithValue("sfailtype", Convert.ToString(sFailType == null ? "" : sFailType));
                    command.Parameters.AddWithValue("sfailuretype", Convert.ToString(sFailureType == null ? "" : sFailureType));
                    command.Parameters.AddWithValue("stodate", Convert.ToString(sTodate == null ? "" : sTodate));
                    command.Parameters.AddWithValue("sfromdate", Convert.ToString(sFromDate == null ? "" : sFromDate));
                    command.Parameters.AddWithValue("sofficecode", Convert.ToString(sOfficeCode == null ? "" : sOfficeCode));
                    command.Parameters.AddWithValue("zone", Convert.ToString(Constants.Zone));
                    command.Parameters.AddWithValue("circle", Convert.ToString(Constants.Circle));
                    command.Parameters.AddWithValue("division", Convert.ToString(Constants.Division));
                    command.Parameters.AddWithValue("subdivision", Convert.ToString(Constants.SubDivision));
                    command.Parameters.AddWithValue("section", Convert.ToString(Constants.Section));
                    dtFailureDetails = ObjCon.FetchDataTable(command);
                    return dtFailureDetails;
                }

                #region Not used Logic 11 , 12 , 13 : Receive DTR Pending,commission Pending,commission Completed.
                //if (objReport.sType == "11")
                //{
                //    strQry = "SELECT CAST(\"TC_CODE\" AS TEXT)\"TC_CODE\",\"DF_DTC_CODE\" \"DT_CODE\",\"DF_LOC_CODE\",cast(\"WO_AMT\" as text) \"COMMISSION\",cast(\"WO_NO\" as text) \"DECOMMISSION\",TO_CHAR(\"DF_DATE\",'dd-MON-yy')\"DF_DATE\",\"TC_CAPACITY\",\"MD_NAME\",'' AS \"WO_NO\" ,'' AS \"TI_INDENT_NO\" ,'' as \"IN_INV_NO\",'' as \"TR_RI_NO\",'" + objReport.sFromDate + "' AS \"FROMDATE\",'" + objReport.sTodate + "' AS \"TODATE\",TO_CHAR(CURRENT_DATE,'DD/MM/YYYY')\"TODAY\",substr(CAST(\"DF_DTC_CODE\" AS TEXT),1," + Constants.Feeder + ")\"FD_FEEDER_CODE\",(SELECT DISTINCT \"FD_FEEDER_NAME\" FROM ";
                //    strQry += " \"TBLFEEDERMAST\" WHERE \"FD_FEEDER_CODE\"=\"DT_FDRSLNO\")\"FD_FEEDER_NAME\",(SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\")\"TC_MAKE_ID\",'' AS \"TI_INDENT_DATE\",'' AS \"ESTIMATION_NO\",'' AS \"IN_DATE\",'' AS \"TR_RV_DATE\",'' AS \"TR_RI_DATE\",'' AS \"RD_RECEIVE_DATE\",'' AS \"TMC_COM_DATE\",'" + objReport.sReportType + "' AS \"REPORT_TYPE\",'" + objReport.sType + "' AS \"REPORT_CATEGORY\",";
                //    if (objReport.sReportType == "3")
                //    {
                //        strQry += "TO_CHAR(\"WO_DATE\",'DD-MON-YYYY')\"WO_DATE\",";
                //    }
                //    else
                //    {
                //        strQry += "'' AS \"WO_DATE\",";
                //    }
                //    strQry += " (SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=\"DF_DTC_CODE\")\"DT_NAME\",(SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr";
                //    strQry += "(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Zone + ")) \"ZONE\",(SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr";
                //    strQry += "(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Circle + ")) \"CIRCLE\",(SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Division + ")) ";
                //    strQry += " \"DIVISION\",(SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ")) \"SUBDIVISION\",";
                //    strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Section + ")) \"SECTION\" ,(case when \"EST_FAIL_TYPE\" = 1 then 'SINGLECOIL' END ) as \"COILTYPE\" from \"TBLTCMASTER\",\"TBLDTCMAST\",\"TBLMASTERDATA\",\"TBLESTIMATIONDETAILS\" , \"TBLDTCFAILURE\" LEFT JOIN \"TBLWORKORDER\"  ON \"DF_ID\"=\"WO_DF_ID\" LEFT JOIN \"TBLRECEIVEDTR\" ON \"WO_SLNO\"=\"RD_WO_SLNO\" where  \"DF_EQUIPMENT_ID\"=\"TC_CODE\" AND \"DF_ID\"=\"EST_FAILUREID\" and \"DF_DTC_CODE\"=\"DT_CODE\" and \"DF_REPLACE_FLAG\"=0 AND ";
                //    //strQry += " AND TO_CHAR(DF_DATE,'YYYY/MM/DD') BETWEEN '" + sFromDate + "' AND '" + sTodate + "'
                //    if (objReport.sFeeder != null)
                //    {
                //        strQry += "   \"DT_FDRSLNO\" ='" + objReport.sFeeder + "'  AND";
                //    }
                //    if (objReport.sFailType != null)
                //    {
                //        strQry += " \"EST_FAIL_TYPE\"=" + objReport.sFailType + " AND";

                //    }
                //    if (objReport.sReportType == "3")
                //    {
                //        if (objReport.sTodate == null && (objReport.sFromDate != null))
                //        {
                //            strQry += " TO_CHAR(\"WO_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(\"WO_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD')";

                //        }
                //        if (objReport.sFromDate == null && (objReport.sTodate != null))
                //        {
                //            strQry += " TO_CHAR(\"WO_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                //        }
                //        if (objReport.sFromDate == null && objReport.sTodate == null)
                //        {
                //            strQry += " TO_CHAR(\"WO_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                //        }
                //        if (objReport.sFromDate != null && objReport.sTodate != null)
                //        {
                //            strQry += " TO_CHAR(\"WO_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"WO_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                //        }
                //    }
                //    else
                //    {
                //        if (objReport.sTodate == null && (objReport.sFromDate != null))
                //        {
                //            strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD')";

                //        }
                //        if (objReport.sFromDate == null && (objReport.sTodate != null))
                //        {
                //            strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                //        }
                //        if (objReport.sFromDate == null && objReport.sTodate == null)
                //        {
                //            strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                //        }
                //        if (objReport.sFromDate != null && objReport.sTodate != null)
                //        {
                //            strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                //        }
                //    }

                //    strQry += " AND CAST(\"DF_LOC_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%'";
                //    if (objReport.sMake != null)
                //    {
                //        strQry += "AND \"TC_MAKE_ID\"='" + objReport.sMake + "'";
                //    }
                //    if (objReport.sFailureType != null)
                //    {
                //        strQry += "AND \"DF_FAILURE_TYPE\"='" + objReport.sFailureType + "'";
                //    }
                //    if (objReport.sCapacity != null)
                //    {
                //        strQry += "AND \"TC_CAPACITY\"='" + objReport.sCapacity + "' ";
                //    }
                //    if (objReport.sGuranteeType != null)
                //    {
                //        strQry += "AND \"DF_GUARANTY_TYPE\"='" + objReport.sGuranteeType + "' ";
                //    }

                //    strQry += "AND \"DF_FAILURE_TYPE\"=\"MD_ID\" AND \"MD_TYPE\"='FT' AND \"DF_STATUS_FLAG\" IN (1,4) AND \"EST_FAIL_TYPE\" =1 AND \"WO_SLNO\" IS NOT NULL AND \"RD_ID\" IS NULL order by \"COILTYPE\" ";
                //}

                //if (objReport.sType == "12")
                //{

                //    strQry = "SELECT CAST(\"TC_CODE\" AS TEXT)\"TC_CODE\",\"DF_DTC_CODE\" \"DT_CODE\",TO_CHAR(\"DF_DATE\",'dd-MON-yy')\"DF_DATE\",cast(\"WO_AMT\" as text) \"COMMISSION\",cast(\"WO_NO\" as text) \"DECOMMISSION\",\"DF_LOC_CODE\",\"TC_CAPACITY\",\"MD_NAME\",'' as \"WO_NO\",'' AS \"TI_INDENT_NO\",'' as \"IN_INV_NO\",'' as \"TR_RI_NO\",'" + objReport.sFromDate + "' AS \"FROMDATE\",'" + objReport.sTodate + "' AS \"TODATE\",TO_CHAR(CURRENT_DATE,'DD/MM/YYYY')\"TODAY\",substr(CAST(\"DF_DTC_CODE\" AS TEXT),1," + Constants.Feeder + ")\"FD_FEEDER_CODE\",(SELECT DISTINCT \"FD_FEEDER_NAME\" FROM ";
                //    strQry += " \"TBLFEEDERMAST\" WHERE \"FD_FEEDER_CODE\"=\"DT_FDRSLNO\")\"FD_FEEDER_NAME\",(SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\")\"TC_MAKE_ID\",'' AS \"ESTIMATION_NO\",'' AS \"WO_DATE\",'' AS \"IN_DATE\",'' AS \"TR_RV_DATE\", '' AS \"TR_RI_DATE\",'' AS \"TMC_COM_DATE\",'" + objReport.sReportType + "' AS \"REPORT_TYPE\",'" + objReport.sType + "' AS \"REPORT_CATEGORY\",";
                //    if (objReport.sReportType == "3")
                //    {
                //        strQry += "TO_CHAR(\"RD_RECEIVE_DATE\",'DD-MON-YYYY'),";
                //    }
                //    else
                //    {
                //        strQry += "'' AS \"RD_RECEIVE_DATE\",";
                //    }
                //    strQry += " (SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=\"DF_DTC_CODE\")DT_NAME,(SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr";
                //    strQry += "(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Zone + ")) \"ZONE\",(SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr";
                //    strQry += "(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Circle + ")) \"CIRCLE\",(SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Division + ")) ";
                //    strQry += " \"DIVISION\",(SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ")) \"SUBDIVISION\",";
                //    strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Section + ")) \"SECTION\" ,(case when \"EST_FAIL_TYPE\" = 1 then 'SINGLECOIL' END ) as \"COILTYPE\" from ";
                //    strQry += "\"TBLTCMASTER\",\"TBLDTCMAST\",\"TBLMASTERDATA\",\"TBLESTIMATIONDETAILS\" , \"TBLDTCFAILURE\" LEFT JOIN \"TBLWORKORDER\" ON \"DF_ID\"=\"WO_DF_ID\"  LEFT JOIN \"TBLRECEIVEDTR\" ON \"WO_SLNO\"=\"RD_WO_SLNO\" LEFT JOIN \"TBLMNORCOMISSION\" ON \"RD_ID\"=\"TMC_RD_ID\" where  \"DF_EQUIPMENT_ID\"=\"TC_CODE\" AND \"DF_ID\"=\"EST_FAILUREID\" and \"DF_DTC_CODE\"=\"DT_CODE\" and \"DF_REPLACE_FLAG\"=0 AND  ";
                //    if (objReport.sFeeder != null)
                //    {
                //        strQry += "   \"DT_FDRSLNO\" ='" + objReport.sFeeder + "'  AND";
                //    }
                //    if (objReport.sFailType != null)
                //    {
                //        strQry += " \"EST_FAIL_TYPE\"=" + objReport.sFailType + " AND";

                //    }
                //    if (objReport.sReportType == "3")
                //    {
                //        if (objReport.sTodate == null && (objReport.sFromDate != null))
                //        {
                //            strQry += " TO_CHAR(\"RD_RECEIVE_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(\"RD_RECEIVE_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD')";

                //        }
                //        if (objReport.sFromDate == null && (objReport.sTodate != null))
                //        {
                //            strQry += " TO_CHAR(\"RD_RECEIVE_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                //        }
                //        if (objReport.sFromDate == null && objReport.sTodate == null)
                //        {
                //            strQry += " TO_CHAR(\"RD_RECEIVE_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                //        }
                //        if (objReport.sFromDate != null && objReport.sTodate != null)
                //        {
                //            strQry += " TO_CHAR(\"RD_RECEIVE_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"RD_RECEIVE_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                //        }
                //    }
                //    else
                //    {
                //        if (objReport.sTodate == null && (objReport.sFromDate != null))
                //        {
                //            strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD')";

                //        }
                //        if (objReport.sFromDate == null && (objReport.sTodate != null))
                //        {
                //            strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                //        }
                //        if (objReport.sFromDate == null && objReport.sTodate == null)
                //        {
                //            strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                //        }
                //        if (objReport.sFromDate != null && objReport.sTodate != null)
                //        {
                //            strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                //        }
                //    }

                //    strQry += " AND CAST(\"DF_LOC_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%'";
                //    if (objReport.sMake != null)
                //    {
                //        strQry += "AND \"TC_MAKE_ID\"='" + objReport.sMake + "'";
                //    }
                //    if (objReport.sFailureType != null)
                //    {
                //        strQry += "AND \"DF_FAILURE_TYPE\"='" + objReport.sFailureType + "'";
                //    }
                //    if (objReport.sCapacity != null)
                //    {
                //        strQry += "AND \"TC_CAPACITY\"='" + objReport.sCapacity + "' ";
                //    }
                //    if (objReport.sGuranteeType != null)
                //    {
                //        strQry += "AND \"DF_GUARANTY_TYPE\"='" + objReport.sGuranteeType + "' ";
                //    }

                //    strQry += "AND \"DF_FAILURE_TYPE\"=\"MD_ID\" AND \"MD_TYPE\"='FT' AND \"DF_STATUS_FLAG\" IN (1,4) AND  \"EST_FAIL_TYPE\" =1 AND \"RD_ID\" IS NOT NULL AND \"TMC_ID\" IS NULL";
                //}

                //if (objReport.sType == "13")
                //{

                //    strQry = "SELECT CAST(\"TC_CODE\" AS TEXT)\"TC_CODE\",\"DF_DTC_CODE\" \"DT_CODE\",TO_CHAR(\"DF_DATE\",'dd-MON-yy')\"DF_DATE\",cast(\"WO_AMT\" as text) \"COMMISSION\",cast(\"WO_NO\" as text) \"DECOMMISSION\",\"DF_LOC_CODE\",\"TC_CAPACITY\",\"MD_NAME\",'' as \"WO_NO\",'' AS \"TI_INDENT_NO\",'' as \"IN_INV_NO\",'' as \"TR_RI_NO\",'" + objReport.sFromDate + "' AS \"FROMDATE\",'" + objReport.sTodate + "' AS \"TODATE\",TO_CHAR(CURRENT_DATE,'DD/MM/YYYY')\"TODAY\",substr(CAST(\"DF_DTC_CODE\" AS TEXT),1," + Constants.Feeder + ")\"FD_FEEDER_CODE\",(SELECT DISTINCT \"FD_FEEDER_NAME\" FROM ";
                //    strQry += " \"TBLFEEDERMAST\" WHERE \"FD_FEEDER_CODE\"=\"DT_FDRSLNO\")\"FD_FEEDER_NAME\",(SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\")\"TC_MAKE_ID\",'' AS \"ESTIMATION_NO\",'' AS \"WO_DATE\",'' AS \"IN_DATE\",'' AS \"TR_RV_DATE\", '' AS \"TR_RI_DATE\",'' AS \"RD_RECEIVE_DATE\", '" + objReport.sReportType + "' AS \"REPORT_TYPE\",'" + objReport.sType + "' AS \"REPORT_CATEGORY\",";
                //    if (objReport.sReportType == "3")
                //    {
                //        strQry += "TO_CHAR(\"TMC_COM_DATE\",'DD-MON-YYYY'),";
                //    }
                //    else
                //    {
                //        strQry += "'' AS \"TMC_COM_DATE\",";
                //    }
                //    strQry += " (SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=\"DF_DTC_CODE\")DT_NAME,(SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr";
                //    strQry += "(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Zone + ")) \"ZONE\",(SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr";
                //    strQry += "(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Circle + ")) \"CIRCLE\",(SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Division + ")) ";
                //    strQry += " \"DIVISION\",(SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ")) \"SUBDIVISION\",";
                //    strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  from \"VIEW_ALL_OFFICES\" where CAST(\"OFF_CODE\" AS TEXT)=substr(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Section + ")) \"SECTION\" ,(case when \"EST_FAIL_TYPE\" = 1 then 'SINGLECOIL' END ) as \"COILTYPE\" from ";
                //    strQry += "\"TBLTCMASTER\",\"TBLDTCMAST\",\"TBLMASTERDATA\",\"TBLESTIMATIONDETAILS\" , \"TBLDTCFAILURE\" LEFT JOIN \"TBLWORKORDER\" ON \"DF_ID\"=\"WO_DF_ID\"  LEFT JOIN \"TBLRECEIVEDTR\" ON \"WO_SLNO\"=\"RD_WO_SLNO\" LEFT JOIN \"TBLMNORCOMISSION\" ON \"RD_ID\"=\"TMC_RD_ID\" where  \"DF_EQUIPMENT_ID\"=\"TC_CODE\" AND \"DF_ID\"=\"EST_FAILUREID\" and \"DF_DTC_CODE\"=\"DT_CODE\" and \"DF_REPLACE_FLAG\"=1 AND  ";
                //    if (objReport.sFeeder != null)
                //    {
                //        strQry += "   \"DT_FDRSLNO\" ='" + objReport.sFeeder + "'  AND";
                //    }
                //    if (objReport.sFailType != null)
                //    {
                //        strQry += " \"EST_FAIL_TYPE\"=" + objReport.sFailType + " AND";

                //    }
                //    if (objReport.sReportType == "3")
                //    {
                //        if (objReport.sTodate == null && (objReport.sFromDate != null))
                //        {
                //            strQry += " TO_CHAR(\"TMC_COM_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(\"TMC_COM_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD')";

                //        }
                //        if (objReport.sFromDate == null && (objReport.sTodate != null))
                //        {
                //            strQry += " TO_CHAR(\"TMC_COM_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                //        }
                //        if (objReport.sFromDate == null && objReport.sTodate == null)
                //        {
                //            strQry += " TO_CHAR(\"TMC_COM_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                //        }
                //        if (objReport.sFromDate != null && objReport.sTodate != null)
                //        {
                //            strQry += " TO_CHAR(\"TMC_COM_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"TMC_COM_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                //        }
                //    }
                //    else
                //    {
                //        if (objReport.sTodate == null && (objReport.sFromDate != null))
                //        {
                //            strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD')";

                //        }
                //        if (objReport.sFromDate == null && (objReport.sTodate != null))
                //        {
                //            strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                //        }
                //        if (objReport.sFromDate == null && objReport.sTodate == null)
                //        {
                //            strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                //        }
                //        if (objReport.sFromDate != null && objReport.sTodate != null)
                //        {
                //            strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                //        }
                //    }

                //    strQry += " AND CAST(\"DF_LOC_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%'";
                //    if (objReport.sMake != null)
                //    {
                //        strQry += "AND \"TC_MAKE_ID\"='" + objReport.sMake + "'";
                //    }
                //    if (objReport.sFailureType != null)
                //    {
                //        strQry += "AND \"DF_FAILURE_TYPE\"='" + objReport.sFailureType + "'";
                //    }
                //    if (objReport.sCapacity != null)
                //    {
                //        strQry += "AND \"TC_CAPACITY\"='" + objReport.sCapacity + "' ";
                //    }
                //    if (objReport.sGuranteeType != null)
                //    {
                //        strQry += "AND \"DF_GUARANTY_TYPE\"='" + objReport.sGuranteeType + "' ";
                //    }

                //    strQry += "AND \"DF_FAILURE_TYPE\"=\"MD_ID\"  AND \"MD_TYPE\"='FT' AND \"DF_STATUS_FLAG\" IN (1,4) AND  \"EST_FAIL_TYPE\" =1 AND \"TMC_ID\" IS NOT NULL";
                //}
                #endregion

                return dtFailureDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtFailureDetails;

            }
        }

        public DataTable CRAbstract(clsReports objReport)
        {
            string strQry = string.Empty;

            DataTable CRAbstract = new DataTable();
            try
            {
                #region commented Inline query
                //strQry = "SELECT TO_CHAR(\"TM_MAPPING_DATE\",'DD-MON-YYYY')\"TM_MAPPING_DATE\",(SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Zone + "))\"ZONE\",(SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE";
                //strQry += " CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Circle + "))\"CIRCLE\",(SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)";
                //strQry += "FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Division + "))\"DIVISION\",";
                //strQry += "(SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE ";
                //strQry += " CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + "))\"SUBDIVISION\",(SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)";
                //strQry += "FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Section + "))\"SECTION\",substr(CAST(\"DF_DTC_CODE\" AS TEXT),1," + Constants.Feeder + ")\"FD_FEEDER_CODE\",(SELECT DISTINCT \"FD_FEEDER_NAME\" FROM ";
                //strQry += " \"TBLFEEDERMAST\" WHERE \"FD_FEEDER_CODE\"=\"DT_FDRSLNO\")\"FD_FEEDER_NAME\", '" + objReport.sFromDate + "' AS \"FROMDATE\",";
                //strQry += "(CASE WHEN \"DF_ENHANCE_CAPACITY\" IS NULL THEN 'FAILURE' WHEN \"DF_ENHANCE_CAPACITY\" IS ";
                //strQry += "NOT NULL THEN 'ENHANCEMENT' END)\"NOMENCLATURE\",\"DF_DTC_CODE\",CAST(\"DF_EQUIPMENT_ID\"  AS TEXT)\"DF_EQUIPMENT_ID\",\"WO_NO\",";
                //strQry += "\"WO_NO_DECOM\" ,TO_CHAR(\"WO_DATE\",'DD-MON-YYYY')\"WO_DATE\",\"EST_NO\",'" + objReport.sTodate + "' AS \"TODATE\",TO_CHAR(CURRENT_DATE,'DD-MON-YYYY') AS CURRENTDATE,";
                //strQry += "TO_CHAR( \"EST_CRON\",'DD-MON-YYYY')\"EST_CRON\", \"TI_INDENT_NO\",";
                //strQry += "TO_CHAR( \"TI_INDENT_DATE\",'DD-MON-YYYY')\"TI_INDENT_DATE\", \"IN_INV_NO\",";
                //strQry += "TO_CHAR( \"IN_DATE\",'DD-MON-YYYY')\"IN_DATE\",\"TR_RI_NO\", TO_CHAR( \"TR_RI_DATE\",'DD-MON-YYYY')\"TR_RI_DATE\",";
                //strQry += "\"TR_RV_NO\",TO_CHAR( \"TR_RV_DATE\",'DD-MON-YYYY')\"TR_RV_DATE\",TO_CHAR( \"TR_CR_DATE\",'DD-MON-YYYY')\"TR_CR_DATE\" ";
                //strQry += "from \"TBLDTCFAILURE\" INNER JOIN \"TBLESTIMATIONDETAILS\" ON \"DF_ID\"=\"EST_FAILUREID\" INNER JOIN \"TBLWORKORDER\" ON \"DF_ID\"=\"WO_DF_ID\" ";
                //strQry += "INNER JOIN \"TBLINDENT\" ON \"WO_SLNO\"=\"TI_WO_SLNO\" INNER JOIN \"TBLDTCINVOICE\" ON \"IN_TI_NO\"=\"TI_ID\" ";
                //strQry += "INNER JOIN \"TBLTCREPLACE\" ON \"TR_IN_NO\"=\"IN_NO\" INNER JOIN \"TBLTCMASTER\" ON \"DF_EQUIPMENT_ID\"=\"TC_CODE\" INNER JOIN \"TBLDTCMAST\" ON ";
                //strQry += "\"DF_DTC_CODE\" =\"DT_CODE\" INNER JOIN \"TBLTRANSDTCMAPPING\" ON \"TM_DTC_ID\"=\"DF_DTC_CODE\" AND \"TM_LIVE_FLAG\"='1' WHERE ";
                //if (objReport.sFeeder != null)
                //{
                //    strQry += "    \"DT_FDRSLNO\" ='" + objReport.sFeeder + "' and ";
                //}
                //if (objReport.sTodate == null && (objReport.sFromDate != null))
                //{
                //    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD')";
                //}
                //if (objReport.sFromDate == null && (objReport.sTodate != null))
                //{
                //    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                //}
                //if (objReport.sFromDate == null && objReport.sTodate == null)
                //{
                //    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                //}
                //if (objReport.sFromDate != null && objReport.sTodate != null)
                //{
                //    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                //}
                //strQry += "AND CAST(\"DF_LOC_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%' AND \"DF_REPLACE_FLAG\"=1 AND \"DF_STATUS_FLAG\" IN (1,4) ORDER BY \"CIRCLE\",\"DIVISION\",\"ZONE\"  ";
                //CRAbstract = ObjCon.FetchDataTable(strQry);
                #endregion
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getcrabstractreport");
                cmd.Parameters.AddWithValue("stodate", Convert.ToString(objReport.sTodate == null ? "" : objReport.sTodate));
                cmd.Parameters.AddWithValue("sfromdate", Convert.ToString(objReport.sFromDate == null ? "" : objReport.sFromDate));
                cmd.Parameters.AddWithValue("zone", Convert.ToString(Constants.Zone));
                cmd.Parameters.AddWithValue("circle", Convert.ToString(Constants.Circle));
                cmd.Parameters.AddWithValue("division", Convert.ToString(Constants.Division));
                cmd.Parameters.AddWithValue("subdivision", Convert.ToString(Constants.SubDivision));
                cmd.Parameters.AddWithValue("section", Convert.ToString(Constants.Section));
                cmd.Parameters.AddWithValue("sfeeder", Convert.ToString(objReport.sFeeder == null ? "" : objReport.sFeeder));
                cmd.Parameters.AddWithValue("offcode", Convert.ToString(sOfficeCode == null ? "" : sOfficeCode));
                CRAbstract = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return CRAbstract;
            }
            return CRAbstract;
        }

        public DataTable PENDINGWTREPARIER(clsReports objReport)
        {
            string strQry = string.Empty;
            DataTable dtPendingwtreparier = new DataTable();
            try
            {

                strQry += "SELECT \"ZONE\",\"ZONE_CODE\",\"CIRCLE\",\"CIRCLE_CODE\",\"DURATION\",\"UPTO_25\",\"FROM26_63\",\"FROM64_100\",\"FROM101_200\",\"FROM201_250\",";
                strQry += "\"ABOVE_250\",\"FROMDATE\",\"TODATE\",\"CURRENTDATE\",SUM(\"UPTO_25\" + \"FROM26_63\"  + \"FROM64_100\" +";
                strQry += "\"FROM101_200\" + \"FROM201_250\" + \"ABOVE_250\") \"TOTAL\" FROM(";
                strQry += "SELECT DISTINCT \"CM_CIRCLE_CODE\",D.\"SM_ID\",\"DURATION\" ,D.\"DIV_NAME\" AS \"DIVISION_NAME\",\"UPTO25\" AS \"UPTO_25\",\"A2663\" ";
                strQry += "AS \"FROM26_63\",\"B64100\"  AS \"FROM64_100\",\"C101200\" AS \"FROM101_200\",\"D201250\" AS \"FROM201_250\"";
                strQry += ",\"ABOVE250\" AS \"ABOVE_250\", '" + objReport.sTempFromDate + "' AS \"FROMDATE\", '" + objReport.sTempTodate + "' ";
                strQry += "AS \"TODATE\",  TO_CHAR(NOW(),'DD-MON-YYYY') AS \"CURRENTDATE\",(SELECT \"ZO_NAME\" FROM \"TBLZONE\" WHERE";
                strQry += " CAST(\"ZO_ID\" AS TEXT)=SUBSTR(CAST(E.\"CM_CIRCLE_CODE\" AS TEXT),1," + Constants.Zone + ")) AS \"ZONE\",\"CM_CIRCLE_NAME\" AS";
                strQry += " \"CIRCLE\",E.\"CM_CIRCLE_CODE\" AS \"CIRCLE_CODE\",SUBSTR(CAST(E.\"CM_CIRCLE_CODE\" AS TEXT),1," + Constants.Zone + ")AS \"ZONE_CODE\"  FROM (SELECT  \"SM_ID\", \"MD_NAME\" AS \"DURATION\", \"SM_NAME\",\"UPTO25\",\"A2663\",";
                strQry += " \"B64100\",\"C101200\",\"D201250\",\"ABOVE250\",\"MD_ID\",\"DIV_NAME\" FROM (SELECT \"MD_ID\",\"SM_ID\", \"MD_NAME\",";
                strQry += " \"SM_NAME\",\"DIV_CODE\",\"DIV_NAME\" FROM  \"TBLMASTERDATA\" ,\"TBLSTOREMAST\",\"TBLSTOREOFFCODE\",\"TBLDIVISION\"  WHERE \"SM_ID\" = \"STO_SM_ID\" AND \"MD_TYPE\"='RD'";
                strQry += " ORDER BY \"MD_ID\")A  LEFT JOIN ( SELECT CAST(CASE  WHEN ROUND(date_part('day', age(NOW(),\"RSM_ISSUE_DATE\")))";
                strQry += " BETWEEN 0 AND 30 THEN  'WITHIN 30 DAYS'  WHEN  ROUND(  date_part('day',age( NOW(),\"RSM_ISSUE_DATE\")))  BETWEEN";
                strQry += " 31 AND 60 THEN '30-60 DAYS'  WHEN ROUND(  date_part('day',age( NOW(),\"RSM_ISSUE_DATE\")))  BETWEEN 61 AND 90 THEN";
                strQry += " '60-90 DAYS'  WHEN   ROUND(  date_part('day',age( NOW(),\"RSM_ISSUE_DATE\"))) > 90 THEN 'MORE THAN 90'";
                strQry += " END AS TEXT)  AS   \"CONDITION1\",  CAST(SUM(CASE WHEN \"TC_CAPACITY\" BETWEEN 0 AND 25 THEN 1 ELSE 0 END)";
                strQry += " AS INT) \"UPTO25\",CAST(SUM(CASE WHEN \"TC_CAPACITY\"  BETWEEN 26 AND 63 THEN 1 ELSE 0 END) AS INT)  ";
                strQry += " \"A2663\",CAST(SUM(CASE WHEN \"TC_CAPACITY\" BETWEEN 64   AND 100 THEN 1 ELSE 0 END)  AS INT)  \"B64100\",";
                strQry += " CAST(SUM(CASE WHEN \"TC_CAPACITY\" BETWEEN 101 AND 200 THEN 1 ELSE 0 END)  AS INT)  \"C101200\", ";
                strQry += " CAST(SUM(CASE WHEN \"TC_CAPACITY\" BETWEEN 201 AND 250 THEN 1 ELSE 0 END) AS INT)  \"D201250\", ";
                strQry += "CAST(SUM(CASE WHEN  \"TC_CAPACITY\" > 250   THEN 1 ELSE 0 END)  AS INT) \"ABOVE250\",  \"RSM_DIV_CODE\",\"RSM_NEW_DIV_CODE\" FROM ";
                strQry += "\"TBLREPAIRSENTMASTER\",  \"TBLREPAIRSENTDETAILS\",\"TBLTCMASTER\" WHERE  \"RSM_ID\" = \"RSD_RSM_ID\" AND   ";
                strQry += " \"TC_CODE\" = \"RSD_TC_CODE\" AND  \"RSD_DELIVARY_DATE\" IS  NULL ";
                if (objReport.sRepriername != null)
                {
                    strQry += "  AND \"RSM_SUPREP_ID\"='" + objReport.sRepriername + "'  ";
                }
                if (objReport.sTodate == null && (objReport.sFromDate != null))
                {
                    strQry += "  AND  TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    strQry += " AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                }

                if (objReport.sFromDate == null && (objReport.sTodate != null))
                {
                    strQry += " AND  TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                }
                if (objReport.sFromDate == null && objReport.sTodate == null)
                {
                    strQry += " AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<=TO_CHAR(NOW(),'YYYY/MM/DD') ";
                }
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " AND  TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                }

                strQry += " GROUP BY CASE  WHEN ROUND (date_part('day',age( NOW(),\"RSM_ISSUE_DATE\")))  BETWEEN 0 AND 30 THEN 'WITHIN 30 DAYS'  WHEN  ";
                strQry += " ROUND(  date_part('day',age( NOW(),\"RSM_ISSUE_DATE\")))  BETWEEN 31 AND 60 THEN '30-60 DAYS'  WHEN ";
                strQry += " ROUND(  date_part('day',age( NOW(),\"RSM_ISSUE_DATE\")))  BETWEEN 61 AND 90 THEN '60-90 DAYS'  WHEN  ";
                strQry += " ROUND(  date_part('day',age( NOW(),\"RSM_ISSUE_DATE\"))) > 90 THEN 'MORE THAN 90' END ,\"RSM_DIV_CODE\",\"RSM_NEW_DIV_CODE\")B ";
                strQry += " ON \"DIV_CODE\"=\"RSM_NEW_DIV_CODE\" AND \"CONDITION1\" =A.\"MD_NAME\")D RIGHT JOIN (SELECT * FROM \"TBLCIRCLE\",\"TBLSTOREMAST\",\"TBLSTOREOFFCODE\"";
                strQry += "WHERE \"SM_ID\"=\"STO_SM_ID\" AND substr(cast(\"STO_OFF_CODE\" as text),1," + Constants.Circle + ")=cast(\"CM_CIRCLE_CODE\" as text) )E ON D.\"SM_ID\"=E.\"SM_ID\")A ";
                strQry += " GROUP BY \"ZONE\",\"ZONE_CODE\",\"CIRCLE\",\"CIRCLE_CODE\",\"DURATION\",\"UPTO_25\",\"FROM26_63\",\"FROM64_100\", ";
                strQry += " \"FROM101_200\",\"FROM201_250\",\"ABOVE_250\",\"FROMDATE\",\"TODATE\",\"CURRENTDATE\" ";

                dtPendingwtreparier = ObjCon.FetchDataTable(strQry);
                //To Remove the section which doesn't have a single values in that section
                int globalindex = -1;
                int rowcount = dtPendingwtreparier.Rows.Count / 4;
                for (int j = 0; j < rowcount; j++)
                {

                    bool delete = true;
                    for (int i = 0; i < 4; i++)
                    {
                        globalindex++;

                        if (Convert.ToString(dtPendingwtreparier.Rows[globalindex][5]).Length > 0)
                        {
                            delete = false;

                        }

                    }

                    if (delete)
                    {
                        dtPendingwtreparier.Rows.RemoveAt(globalindex - 3);
                        globalindex--;
                        dtPendingwtreparier.Rows.RemoveAt(globalindex - 2);
                        globalindex--;
                        dtPendingwtreparier.Rows.RemoveAt(globalindex - 1);
                        globalindex--;
                        dtPendingwtreparier.Rows.RemoveAt(globalindex);
                        globalindex--;
                    }

                }

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtPendingwtreparier;
            }
            return dtPendingwtreparier;
        }
        //public DataTable TransformerWiseDetails(clsReports objReport)
        //{
        //    string strQry = string.Empty;
        //    DataTable dtTransformerWise = new DataTable();
        //    try
        //    {

        //        strQry = "SELECT  (SELECT \"ZO_NAME\" FROM \"TBLZONE\" WHERE  CAST(\"ZO_ID\" AS TEXT)=SUBSTR(CAST(A.\"OFFCODE\" ";
        //        strQry += "AS TEXT),1," + Constants.Zone + "))\"ZONE\",\"CM_CIRCLE_NAME\" AS \"CIRCLE\"";
        //        strQry += ",CASE WHEN \"IND_INSP_DATE\" IS NOT NULL THEN 'YES' ELSE 'NO'  END \"TESTING_COMPLETE\",  (SELECT \"TR_NAME\"";
        //        strQry += "FROM \"TBLTRANSREPAIRER\" WHERE \"TR_ID\"=\"RSM_SUPREP_ID\")\"REPAIRER_NAME\",(SELECT \"TR_ADDRESS\" FROM \"TBLTRANSREPAIRER\" ";
        //        strQry += "WHERE \"TR_ID\"=\"RSM_SUPREP_ID\")  \"REPAIRER_ADDRESS\" ,TO_CHAR(\"RSM_ISSUE_DATE\",'DD-MON-YYYY')  AS \"ISSUED_ON\",CAST(\"TC_CODE\" AS TEXT) \"TC_CODE\"";
        //        strQry += ",(SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\")\"TM_NAME\",CAST(\"TC_CAPACITY\" AS TEXT)\"TC_CAPACITY\"";
        //        strQry += ",\"RSM_GUARANTY_TYPE\",\"RSM_PO_NO\",TO_CHAR(\"RSM_PO_DATE\",'DD-MON-YYYY') AS \"RSM_PO_DATE\" ,'' AS \"FROMDATE\", '' AS \"TODATE\" ";
        //        strQry += ",TO_CHAR(NOW(),'DD-MON-YYYY') AS \"CURRENTDATE\",ROUND(DATE_PART('DAY',AGE( NOW(),\"RSM_ISSUE_DATE\"))) AS \"PENDING_DAYS\"  FROM  ";
        //        strQry += "(SELECT DISTINCT * FROM \"TBLREPAIRSENTDETAILS\",\"TBLREPAIRSENTMASTER\",\"TBLTCMASTER\",\"TBLSTOREMAST\",(SELECT DISTINCT \"CM_CIRCLE_NAME\",\"CM_CIRCLE_CODE\" AS \"OFFCODE\" FROM \"TBLCIRCLE\",\"TBLSTOREMAST\",\"TBLSTOREOFFCODE\" WHERE";
        //        strQry += "\"SM_ID\"=\"STO_SM_ID\" AND  CAST(\"STO_OFF_CODE\" AS TEXT) LIKE '" + objReport.sOfficeCode + "%' AND substr(cast(\"STO_OFF_CODE\" as text),1," + Constants.Circle + ")=cast(\"CM_CIRCLE_CODE\" as text))B WHERE ";
        //        strQry += "\"RSM_ID\"= \"RSD_RSM_ID\"  AND \"TC_CODE\"= \"RSD_TC_CODE\" AND \"RSM_DIV_CODE\"= \"SM_ID\" AND \"RSD_DELIVARY_DATE\" IS NULL  AND";
        //        strQry += "\"RSD_DELIVARY_DATE\" IS NULL";

        //        //strQry = " SELECT (SELECT \"OFF_NAME\" FROM \"VIEW_OFFICES\" WHERE  CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"RSM_DIV_CODE\" AS TEXT),1," + Constants.Zone + "))\"ZONE\",(SELECT \"OFF_NAME\" FROM \"VIEW_OFFICES\" WHERE  CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"RSM_DIV_CODE\" AS TEXT),0," + Constants.Circle + ")) ";
        //        //strQry += " \"CIRCLE\",\"OFF_NAME\" AS \"DIVISION\",CASE WHEN  \"IND_INSP_DATE\" IS NOT NULL THEN 'YES' ELSE 'NO'  END \"TESTING_COMPLETE\", ";
        //        //strQry += " (SELECT \"TR_NAME\" FROM \"TBLTRANSREPAIRER\"   WHERE \"TR_ID\"=\"RSM_SUPREP_ID\")\"REPAIRER_NAME\",(SELECT \"TR_ADDRESS\"FROM ";
        //        //strQry += " \"TBLTRANSREPAIRER\" WHERE \"TR_ID\"=\"RSM_SUPREP_ID\")  \"REPAIRER_ADDRESS\" ,TO_CHAR(\"RSM_ISSUE_DATE\",'DD-MON-YYYY') ";
        //        //strQry += " AS \"ISSUED_ON\",CAST(\"TC_CODE\" AS TEXT) \"TC_CODE\",(SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\") ";
        //        //strQry += " \"TM_NAME\",CAST(\"TC_CAPACITY\" AS TEXT)\"TC_CAPACITY\",\"RSM_GUARANTY_TYPE\",\"RSM_PO_NO\", ";
        //        //strQry += " TO_CHAR(\"RSM_PO_DATE\",'DD-MON-YYYY') AS \"RSM_PO_DATE\" ,'" + objReport.sTempFromDate + "' AS \"FROMDATE\", '" + objReport.sTempTodate + "' AS \"TODATE\" , ";
        //        //strQry += " TO_CHAR(NOW(),'DD-MON-YYYY') AS \"CURRENTDATE\",ROUND(DATE_PART('DAY',AGE( NOW(),\"RSM_ISSUE_DATE\"))) AS \"PENDING_DAYS\" ";
        //        //strQry += " FROM  (SELECT * FROM \"TBLREPAIRSENTDETAILS\",\"TBLREPAIRSENTMASTER\",\"TBLTCMASTER\",\"VIEW_ALL_OFFICES\" WHERE ";
        //        //strQry += " \"RSM_ID\"= \"RSD_RSM_ID\"  AND \"TC_CODE\"= \"RSD_TC_CODE\" AND \"RSM_DIV_CODE\"= \"OFF_CODE\" AND \"RSD_DELIVARY_DATE\"IS NULL ";
        //        //strQry += " AND  \"RSM_DIV_CODE\"= \"OFF_CODE\"  AND \"RSD_DELIVARY_DATE\" IS NULL AND  CAST(\"OFF_CODE\" AS TEXT) LIKE '" + objReport.sOfficeCode + "%'   ";

        //        if (objReport.sRepriername != null)
        //        {
        //            strQry += "  AND \"RSM_SUPREP_ID\"='" + objReport.sRepriername + "'  ";
        //        }

        //        if (objReport.sTodate == null && (objReport.sFromDate != null))
        //        {
        //            strQry += " AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD') >= '" + objReport.sFromDate + "' AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<=TO_CHAR(NOW(),'YYYY/MM/DD')";
        //        }

        //        if (objReport.sFromDate == null && (objReport.sTodate != null))
        //        {
        //            strQry += " AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD') <='" + objReport.sTodate + "' ";
        //        }
        //        if (objReport.sFromDate == null && objReport.sTodate == null)
        //        {
        //            strQry += " AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<=TO_CHAR(NOW(),'YYYY/MM/DD') ";
        //        }
        //        if (objReport.sFromDate != null && objReport.sTodate != null)
        //        {
        //            strQry += "AND  TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
        //        }

        //        strQry += " ) A LEFT JOIN \"TBLINSPECTIONDETAILS\" ON \"RSD_ID\"=\"IND_RSD_ID\" AND COALESCE(\"IND_INSP_RESULT\",0)=1 ORDER BY \"SM_NAME\",DATE_PART('DAY',AGE( NOW(),\"RSM_ISSUE_DATE\")) DESC";
        //        dtTransformerWise = ObjCon.FetchDataTable(strQry);

        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "TRANSFORMERWISEDETAILS");
        //        return dtTransformerWise;
        //    }
        //    return dtTransformerWise;
        //}

        //completed

        public DataTable TransformerWiseDetails(clsReports objReport)
        {
            string strQry = string.Empty;
            DataTable dtTransformerWise = new DataTable();
            try
            {

                strQry = "SELECT  (SELECT \"ZO_NAME\" FROM \"TBLZONE\" WHERE  CAST(\"ZO_ID\" AS TEXT)=SUBSTR(CAST(A.\"OFFCODE\" ";
                strQry += "AS TEXT),1," + Constants.Zone + "))\"ZONE\",\"CM_CIRCLE_NAME\" AS \"CIRCLE\"";
                strQry += ",CASE WHEN \"IND_INSP_DATE\" IS NOT NULL THEN 'YES' ELSE 'NO'  END \"TESTING_COMPLETE\",  (SELECT \"TR_NAME\"";
                strQry += "FROM \"TBLTRANSREPAIRER\" WHERE \"TR_ID\"=\"RSM_SUPREP_ID\")\"REPAIRER_NAME\",(SELECT \"TR_ADDRESS\" FROM \"TBLTRANSREPAIRER\" ";
                strQry += "WHERE \"TR_ID\"=\"RSM_SUPREP_ID\")  \"REPAIRER_ADDRESS\" ,TO_CHAR(\"RSM_ISSUE_DATE\",'DD-MON-YYYY')  AS \"ISSUED_ON\",CAST(\"TC_CODE\" AS TEXT) \"TC_CODE\"";
                strQry += ",(SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\")\"TM_NAME\",CAST(\"TC_CAPACITY\" AS TEXT)\"TC_CAPACITY\"";
                strQry += ",\"RSM_GUARANTY_TYPE\",\"RSM_PO_NO\",TO_CHAR(\"RSM_PO_DATE\",'DD-MON-YYYY') AS \"RSM_PO_DATE\" ,'' AS \"FROMDATE\", '' AS \"TODATE\" ";
                strQry += ",TO_CHAR(NOW(),'DD-MON-YYYY') AS \"CURRENTDATE\",ROUND(DATE_PART('DAY',AGE( NOW(),\"RSM_ISSUE_DATE\"))) AS \"PENDING_DAYS\"  FROM  ";
                strQry += "(SELECT DISTINCT * FROM \"TBLREPAIRSENTDETAILS\",\"TBLREPAIRSENTMASTER\",\"TBLTCMASTER\",\"TBLSTOREMAST\",\"TBLDIVISION\",(SELECT DISTINCT \"CM_CIRCLE_NAME\",\"CM_CIRCLE_CODE\" AS \"OFFCODE\" FROM \"TBLCIRCLE\",\"TBLSTOREMAST\",\"TBLSTOREOFFCODE\" WHERE";
                strQry += "\"SM_ID\"=\"STO_SM_ID\" AND  CAST(\"STO_OFF_CODE\" AS TEXT) LIKE '" + objReport.sOfficeCode + "%' AND substr(cast(\"STO_OFF_CODE\" as text),1," + Constants.Circle + ")=cast(\"CM_CIRCLE_CODE\" as text))B WHERE ";
                strQry += "\"RSM_ID\"= \"RSD_RSM_ID\"  AND \"TC_CODE\"= \"RSD_TC_CODE\" AND \"RSM_NEW_DIV_CODE\"= \"DIV_CODE\" AND \"RSD_DELIVARY_DATE\" IS NULL  AND";
                strQry += "\"RSD_DELIVARY_DATE\" IS NULL ";

                //strQry = " SELECT (SELECT \"OFF_NAME\" FROM \"VIEW_OFFICES\" WHERE  CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"RSM_DIV_CODE\" AS TEXT),1," + Constants.Zone + "))\"ZONE\",(SELECT \"OFF_NAME\" FROM \"VIEW_OFFICES\" WHERE  CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"RSM_DIV_CODE\" AS TEXT),0," + Constants.Circle + ")) ";
                //strQry += " \"CIRCLE\",\"OFF_NAME\" AS \"DIVISION\",CASE WHEN  \"IND_INSP_DATE\" IS NOT NULL THEN 'YES' ELSE 'NO'  END \"TESTING_COMPLETE\", ";
                //strQry += " (SELECT \"TR_NAME\" FROM \"TBLTRANSREPAIRER\"   WHERE \"TR_ID\"=\"RSM_SUPREP_ID\")\"REPAIRER_NAME\",(SELECT \"TR_ADDRESS\"FROM ";
                //strQry += " \"TBLTRANSREPAIRER\" WHERE \"TR_ID\"=\"RSM_SUPREP_ID\")  \"REPAIRER_ADDRESS\" ,TO_CHAR(\"RSM_ISSUE_DATE\",'DD-MON-YYYY') ";
                //strQry += " AS \"ISSUED_ON\",CAST(\"TC_CODE\" AS TEXT) \"TC_CODE\",(SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\") ";
                //strQry += " \"TM_NAME\",CAST(\"TC_CAPACITY\" AS TEXT)\"TC_CAPACITY\",\"RSM_GUARANTY_TYPE\",\"RSM_PO_NO\", ";
                //strQry += " TO_CHAR(\"RSM_PO_DATE\",'DD-MON-YYYY') AS \"RSM_PO_DATE\" ,'" + objReport.sTempFromDate + "' AS \"FROMDATE\", '" + objReport.sTempTodate + "' AS \"TODATE\" , ";
                //strQry += " TO_CHAR(NOW(),'DD-MON-YYYY') AS \"CURRENTDATE\",ROUND(DATE_PART('DAY',AGE( NOW(),\"RSM_ISSUE_DATE\"))) AS \"PENDING_DAYS\" ";
                //strQry += " FROM  (SELECT * FROM \"TBLREPAIRSENTDETAILS\",\"TBLREPAIRSENTMASTER\",\"TBLTCMASTER\",\"VIEW_ALL_OFFICES\" WHERE ";
                //strQry += " \"RSM_ID\"= \"RSD_RSM_ID\"  AND \"TC_CODE\"= \"RSD_TC_CODE\" AND \"RSM_DIV_CODE\"= \"OFF_CODE\" AND \"RSD_DELIVARY_DATE\"IS NULL ";
                //strQry += " AND  \"RSM_DIV_CODE\"= \"OFF_CODE\"  AND \"RSD_DELIVARY_DATE\" IS NULL AND  CAST(\"OFF_CODE\" AS TEXT) LIKE '" + objReport.sOfficeCode + "%'   ";

                if (objReport.sRepriername != null)
                {
                    strQry += "  AND \"RSM_SUPREP_ID\"='" + objReport.sRepriername + "'  ";
                }

                if (objReport.sTodate == null && (objReport.sFromDate != null))
                {
                    strQry += " AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD') >= '" + objReport.sFromDate + "' AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<=TO_CHAR(NOW(),'YYYY/MM/DD')";
                }

                if (objReport.sFromDate == null && (objReport.sTodate != null))
                {
                    strQry += " AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD') <='" + objReport.sTodate + "' ";
                }
                if (objReport.sFromDate == null && objReport.sTodate == null)
                {
                    strQry += " AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<=TO_CHAR(NOW(),'YYYY/MM/DD') ";
                }
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += "AND  TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                }

                strQry += " ) A LEFT JOIN \"TBLINSPECTIONDETAILS\" ON \"RSD_ID\"=\"IND_RSD_ID\" AND COALESCE(\"IND_INSP_RESULT\",0)=1 ORDER BY \"SM_NAME\",DATE_PART('DAY',AGE( NOW(),\"RSM_ISSUE_DATE\")) DESC";
                dtTransformerWise = ObjCon.FetchDataTable(strQry);

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtTransformerWise;
            }
            return dtTransformerWise;
        }
        //public DataTable ReperierCompleted(clsReports objReport)
        //{
        //    string strQry = string.Empty;
        //    DataTable dtreparierComplete = new DataTable();
        //    try
        //    {



        //        strQry = "SELECT \"ZONE\",\"ZONE_CODE\",\"CIRCLE\",\"CIRCLE_CODE\",\"DURATION\",\"DIVISION_NAME\",\"UPTO_25\",\"FROM26_63\",\"FROM64_100\", ";
        //        strQry += " \"FROM101_200\",\"FROM201_250\",\"ABOVE_250\",\"FROMDATE\",\"TODATE\",\"CURRENTDATE\",SUM(\"UPTO_25\" + \"FROM26_63\" ";
        //        strQry += " + \"FROM64_100\" + \"FROM101_200\" + \"FROM201_250\" + \"ABOVE_250\") \"TOTAL\"FROM(SELECT DISTINCT \"CM_CIRCLE_CODE\",\"DURATION\" ,D.\"SM_ID\" ,D.\"SM_NAME\" AS \"DIVISION_NAME\",  \"UPTO25\" AS ";
        //        strQry += "  \"UPTO_25\",\"A2663\"  AS \"FROM26_63\",\"B64100\"  AS \"FROM64_100\",\"C101200\" AS   \"FROM101_200\", ";
        //        strQry += " \"D201250\" AS \"FROM201_250\",\"ABOVE250\" AS \"ABOVE_250\", '" + objReport.sTempFromDate + "' AS \"FROMDATE\", '" + objReport.sTempTodate + "' AS \"TODATE\", ";
        //        strQry += " TO_CHAR(NOW(),'DD-MON-YYYY') AS \"CURRENTDATE\",(SELECT \"ZO_NAME\" FROM \"TBLZONE\" WHERE CAST(\"ZO_ID\" AS TEXT)=";
        //        strQry += "SUBSTR(CAST(E.\"CM_CIRCLE_CODE\" AS TEXT),1,1)) AS \"ZONE\",\"CM_CIRCLE_NAME\" AS \"CIRCLE\" ,SUBSTR(CAST(E.\"CM_CIRCLE_CODE\" AS TEXT),1,2) AS \"CIRCLE_CODE\",SUBSTR(CAST(E.\"CM_CIRCLE_CODE\" AS TEXT),1,1)AS \"ZONE_CODE\"FROM (SELECT \"MD_NAME\" AS \"DURATION\",\"SM_ID\",  \"SM_NAME\"";
        //        strQry += ",\"UPTO25\",\"A2663\",\"B64100\",\"C101200\",\"D201250\",\"ABOVE250\",\"MD_ID\" FROM (SELECT *  FROM ";
        //        strQry += " \"TBLMASTERDATA\" , \"TBLSTOREMAST\" WHERE \"MD_TYPE\"='RD' ORDER BY \"MD_ID\")A ";
        //        strQry += " LEFT JOIN ( SELECT CAST(CASE  WHEN ROUND(date_part('day', age(NOW(),\"RSM_ISSUE_DATE\")))  BETWEEN 0 AND 30 THEN ";
        //        strQry += " 'WITHIN 30 DAYS'  WHEN  ROUND(  date_part('day',age( NOW(),\"RSM_ISSUE_DATE\")))  BETWEEN 31 AND 60 THEN '30-60 DAYS' ";
        //        strQry += " WHEN ROUND(  date_part('day',age( NOW(),\"RSM_ISSUE_DATE\")))  BETWEEN 61 AND 90 THEN '60-90 DAYS'  WHEN  ";
        //        strQry += " ROUND(  date_part('day',age( NOW(),\"RSM_ISSUE_DATE\"))) > 90 THEN 'MORE THAN 90' END AS TEXT)  AS   \"CONDITION1\", ";
        //        strQry += " CAST(SUM(CASE WHEN \"TC_CAPACITY\" BETWEEN 0 AND 25 THEN 1 ELSE 0 END)  AS INT) \"UPTO25\",CAST(SUM(CASE WHEN \"TC_CAPACITY\" ";
        //        strQry += " BETWEEN 26 AND 63 THEN 1 ELSE 0 END) AS INT)  \"A2663\",CAST(SUM(CASE WHEN \"TC_CAPACITY\" BETWEEN 64   AND 100 THEN 1 ELSE 0 END) ";
        //        strQry += " AS INT)  \"B64100\", CAST(SUM(CASE WHEN \"TC_CAPACITY\" BETWEEN 101 AND 200 THEN 1 ELSE 0 END)  AS INT)  \"C101200\", ";
        //        strQry += " CAST(SUM(CASE WHEN \"TC_CAPACITY\" BETWEEN 201 AND 250 THEN 1 ELSE 0 END) AS INT)  \"D201250\", CAST(SUM(CASE WHEN ";
        //        strQry += " \"TC_CAPACITY\" > 250   THEN 1 ELSE 0 END)  AS INT) \"ABOVE250\", \"RSM_DIV_CODE\" FROM \"TBLREPAIRSENTMASTER\", ";
        //        strQry += " \"TBLREPAIRSENTDETAILS\",\"TBLTCMASTER\" WHERE  \"RSM_ID\" = \"RSD_RSM_ID\" AND   \"TC_CODE\" = \"RSD_TC_CODE\" AND ";
        //        strQry += " \"RSD_DELIVARY_DATE\" IS NOT NULL ";

        //        if (objReport.sRepriername != null)
        //        {
        //            strQry += "  AND \"RSM_SUPREP_ID\"='" + objReport.sRepriername + "'  ";
        //        }
        //        if (objReport.sTodate == null && (objReport.sFromDate != null))
        //        {
        //            strQry += "  AND  TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
        //            strQry += " AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
        //        }

        //        if (objReport.sFromDate == null && (objReport.sTodate != null))
        //        {
        //            strQry += " AND  TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
        //        }
        //        if (objReport.sFromDate == null && objReport.sTodate == null)
        //        {
        //            strQry += " AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<=TO_CHAR(NOW(),'YYYY/MM/DD') ";
        //        }
        //        if (objReport.sFromDate != null && objReport.sTodate != null)
        //        {
        //            strQry += " AND  TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
        //        }


        //        strQry += " GROUP BY CASE  WHEN ROUND (date_part('day',age( NOW(),\"RSM_ISSUE_DATE\")))  BETWEEN 0 AND 30 THEN 'WITHIN 30 DAYS'  WHEN  ";
        //        strQry += " ROUND(  date_part('day',age( NOW(),\"RSM_ISSUE_DATE\")))  BETWEEN 31 AND 60 THEN '30-60 DAYS'  WHEN ";
        //        strQry += " ROUND(  date_part('day',age( NOW(),\"RSM_ISSUE_DATE\")))  BETWEEN 61 AND 90 THEN '60-90 DAYS'  WHEN  ";
        //        strQry += " ROUND(  date_part('day',age( NOW(),\"RSM_ISSUE_DATE\"))) > 90 THEN 'MORE THAN 90' END ,\"RSM_DIV_CODE\")B ";
        //        strQry += " ON CAST(\"CONDITION1\" AS TEXT) =CAST(\"MD_NAME\" AS TEXT) AND \"SM_ID\"=\"RSM_DIV_CODE\")d  ";
        //        strQry += "RIGHT JOIN (SELECT * FROM \"TBLCIRCLE\",\"TBLSTOREMAST\",\"TBLSTOREOFFCODE\" WHERE \"SM_ID\"=\"STO_SM_ID\" AND substr(cast(\"STO_OFF_CODE\" as text),1," + Constants.Circle + ")=cast(\"CM_CIRCLE_CODE\" as text) )E ON D.\"SM_ID\"=E.\"SM_ID\" )A ";
        //        strQry += " GROUP BY \"ZONE\",\"ZONE_CODE\",\"CIRCLE\",\"CIRCLE_CODE\",\"DURATION\",\"DIVISION_NAME\",\"UPTO_25\",\"FROM26_63\",\"FROM64_100\", ";
        //        strQry += " \"FROM101_200\",\"FROM201_250\",\"ABOVE_250\",\"FROMDATE\",\"TODATE\",\"CURRENTDATE\"";


        //        dtreparierComplete = ObjCon.FetchDataTable(strQry);
        //        //To Remove the section which doesn't have a single values in that section
        //        int globalindex = -1;
        //        int rowcount = dtreparierComplete.Rows.Count / 4;
        //        for (int j = 0; j < rowcount; j++)
        //        {

        //            bool delete = true;
        //            for (int i = 0; i < 4; i++)
        //            {
        //                globalindex++;

        //                if (Convert.ToString(dtreparierComplete.Rows[globalindex][5]).Length > 0)
        //                {
        //                    delete = false;

        //                }

        //            }

        //            if (delete)
        //            {
        //                dtreparierComplete.Rows.RemoveAt(globalindex - 3);
        //                globalindex--;
        //                dtreparierComplete.Rows.RemoveAt(globalindex - 2);
        //                globalindex--;
        //                dtreparierComplete.Rows.RemoveAt(globalindex - 1);
        //                globalindex--;
        //                dtreparierComplete.Rows.RemoveAt(globalindex);
        //                globalindex--;
        //            }

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "DTREPARIERCOMPLETE");
        //        return dtreparierComplete;
        //    }
        //    return dtreparierComplete;
        //}


        //DtrMakeWise

        public DataTable ReperierCompleted(clsReports objReport)
        {
            string strQry = string.Empty;
            DataTable dtreparierComplete = new DataTable();
            try
            {



                strQry = "SELECT \"ZONE\",\"ZONE_CODE\",\"CIRCLE\",\"CIRCLE_CODE\",\"DURATION\",\"DIVISION_NAME\",\"UPTO_25\",\"FROM26_63\",\"FROM64_100\", ";
                strQry += " \"FROM101_200\",\"FROM201_250\",\"ABOVE_250\",\"FROMDATE\",\"TODATE\",\"CURRENTDATE\",SUM(\"UPTO_25\" + \"FROM26_63\" ";
                strQry += " + \"FROM64_100\" + \"FROM101_200\" + \"FROM201_250\" + \"ABOVE_250\") \"TOTAL\"FROM(SELECT DISTINCT \"CM_CIRCLE_CODE\",\"DURATION\" ,D.\"SM_ID\" ,D.\"DIV_NAME\" AS \"DIVISION_NAME\",  \"UPTO25\" AS ";
                strQry += "  \"UPTO_25\",\"A2663\"  AS \"FROM26_63\",\"B64100\"  AS \"FROM64_100\",\"C101200\" AS   \"FROM101_200\", ";
                strQry += " \"D201250\" AS \"FROM201_250\",\"ABOVE250\" AS \"ABOVE_250\", '" + objReport.sTempFromDate + "' AS \"FROMDATE\", '" + objReport.sTempTodate + "' AS \"TODATE\", ";
                strQry += " TO_CHAR(NOW(),'DD-MON-YYYY') AS \"CURRENTDATE\",(SELECT \"ZO_NAME\" FROM \"TBLZONE\" WHERE CAST(\"ZO_ID\" AS TEXT)=";
                strQry += "SUBSTR(CAST(E.\"CM_CIRCLE_CODE\" AS TEXT),1,1)) AS \"ZONE\",\"CM_CIRCLE_NAME\" AS \"CIRCLE\" ,SUBSTR(CAST(E.\"CM_CIRCLE_CODE\" AS TEXT),1,2) AS \"CIRCLE_CODE\",SUBSTR(CAST(E.\"CM_CIRCLE_CODE\" AS TEXT),1,1)AS \"ZONE_CODE\"FROM (SELECT \"MD_NAME\" AS \"DURATION\",\"SM_ID\",  \"SM_NAME\",\"DIV_NAME\"";
                strQry += ",\"UPTO25\",\"A2663\",\"B64100\",\"C101200\",\"D201250\",\"ABOVE250\",\"MD_ID\" FROM (SELECT *  FROM ";
                strQry += " \"TBLMASTERDATA\" , \"TBLSTOREMAST\",\"TBLDIVISION\" WHERE \"MD_TYPE\"='RD' ORDER BY \"MD_ID\")A ";
                strQry += " LEFT JOIN ( SELECT CAST(CASE  WHEN ROUND(date_part('day', age(NOW(),\"RSM_ISSUE_DATE\")))  BETWEEN 0 AND 30 THEN ";
                strQry += " 'WITHIN 30 DAYS'  WHEN  ROUND(  date_part('day',age( NOW(),\"RSM_ISSUE_DATE\")))  BETWEEN 31 AND 60 THEN '30-60 DAYS' ";
                strQry += " WHEN ROUND(  date_part('day',age( NOW(),\"RSM_ISSUE_DATE\")))  BETWEEN 61 AND 90 THEN '60-90 DAYS'  WHEN  ";
                strQry += " ROUND(  date_part('day',age( NOW(),\"RSM_ISSUE_DATE\"))) > 90 THEN 'MORE THAN 90' END AS TEXT)  AS   \"CONDITION1\", ";
                strQry += " CAST(SUM(CASE WHEN \"TC_CAPACITY\" BETWEEN 0 AND 25 THEN 1 ELSE 0 END)  AS INT) \"UPTO25\",CAST(SUM(CASE WHEN \"TC_CAPACITY\" ";
                strQry += " BETWEEN 26 AND 63 THEN 1 ELSE 0 END) AS INT)  \"A2663\",CAST(SUM(CASE WHEN \"TC_CAPACITY\" BETWEEN 64   AND 100 THEN 1 ELSE 0 END) ";
                strQry += " AS INT)  \"B64100\", CAST(SUM(CASE WHEN \"TC_CAPACITY\" BETWEEN 101 AND 200 THEN 1 ELSE 0 END)  AS INT)  \"C101200\", ";
                strQry += " CAST(SUM(CASE WHEN \"TC_CAPACITY\" BETWEEN 201 AND 250 THEN 1 ELSE 0 END) AS INT)  \"D201250\", CAST(SUM(CASE WHEN ";
                strQry += " \"TC_CAPACITY\" > 250   THEN 1 ELSE 0 END)  AS INT) \"ABOVE250\", \"RSM_NEW_DIV_CODE\" FROM \"TBLREPAIRSENTMASTER\", ";
                strQry += " \"TBLREPAIRSENTDETAILS\",\"TBLTCMASTER\" WHERE  \"RSM_ID\" = \"RSD_RSM_ID\" AND   \"TC_CODE\" = \"RSD_TC_CODE\" AND ";
                strQry += " \"RSD_DELIVARY_DATE\" IS NOT NULL ";

                if (objReport.sRepriername != null)
                {
                    strQry += "  AND \"RSM_SUPREP_ID\"='" + objReport.sRepriername + "'  ";
                }
                if (objReport.sTodate == null && (objReport.sFromDate != null))
                {
                    strQry += "  AND  TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    strQry += " AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                }

                if (objReport.sFromDate == null && (objReport.sTodate != null))
                {
                    strQry += " AND  TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                }
                if (objReport.sFromDate == null && objReport.sTodate == null)
                {
                    strQry += " AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<=TO_CHAR(NOW(),'YYYY/MM/DD') ";
                }
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " AND  TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                }


                strQry += " GROUP BY CASE  WHEN ROUND (date_part('day',age( NOW(),\"RSM_ISSUE_DATE\")))  BETWEEN 0 AND 30 THEN 'WITHIN 30 DAYS'  WHEN  ";
                strQry += " ROUND(  date_part('day',age( NOW(),\"RSM_ISSUE_DATE\")))  BETWEEN 31 AND 60 THEN '30-60 DAYS'  WHEN ";
                strQry += " ROUND(  date_part('day',age( NOW(),\"RSM_ISSUE_DATE\")))  BETWEEN 61 AND 90 THEN '60-90 DAYS'  WHEN  ";
                strQry += " ROUND(  date_part('day',age( NOW(),\"RSM_ISSUE_DATE\"))) > 90 THEN 'MORE THAN 90' END ,\"RSM_NEW_DIV_CODE\")B ";
                strQry += " ON CAST(\"CONDITION1\" AS TEXT) =CAST(\"MD_NAME\" AS TEXT) AND \"DIV_CODE\"=\"RSM_NEW_DIV_CODE\")d  ";
                strQry += "RIGHT JOIN (SELECT * FROM \"TBLCIRCLE\",\"TBLSTOREMAST\",\"TBLSTOREOFFCODE\" WHERE \"SM_ID\"=\"STO_SM_ID\" AND substr(cast(\"STO_OFF_CODE\" as text),1," + Constants.Circle + ")=cast(\"CM_CIRCLE_CODE\" as text) )E ON D.\"SM_ID\"=E.\"SM_ID\" )A ";
                strQry += " GROUP BY \"ZONE\",\"ZONE_CODE\",\"CIRCLE\",\"CIRCLE_CODE\",\"DURATION\",\"DIVISION_NAME\",\"UPTO_25\",\"FROM26_63\",\"FROM64_100\", ";
                strQry += " \"FROM101_200\",\"FROM201_250\",\"ABOVE_250\",\"FROMDATE\",\"TODATE\",\"CURRENTDATE\"";


                dtreparierComplete = ObjCon.FetchDataTable(strQry);
                //To Remove the section which doesn't have a single values in that section
                int globalindex = -1;
                int rowcount = dtreparierComplete.Rows.Count / 4;
                for (int j = 0; j < rowcount; j++)
                {

                    bool delete = true;
                    for (int i = 0; i < 4; i++)
                    {
                        globalindex++;

                        if (Convert.ToString(dtreparierComplete.Rows[globalindex][5]).Length > 0)
                        {
                            delete = false;

                        }

                    }

                    if (delete)
                    {
                        dtreparierComplete.Rows.RemoveAt(globalindex - 3);
                        globalindex--;
                        dtreparierComplete.Rows.RemoveAt(globalindex - 2);
                        globalindex--;
                        dtreparierComplete.Rows.RemoveAt(globalindex - 1);
                        globalindex--;
                        dtreparierComplete.Rows.RemoveAt(globalindex);
                        globalindex--;
                    }

                }

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtreparierComplete;
            }
            return dtreparierComplete;
        }

        public DataTable Printdtrwise(clsReports objReport)
        {
            DataTable dtRDetailedReport = new DataTable();
            string strQry = string.Empty;

            try
            {

                strQry = " SELECT CASE  WHEN row_number() OVER (ORDER BY \"TM_ID\")<" + objReport.sMake + " THEN \"TM_NAME\" ELSE 'OTHERS' END \"TM_NAME\",\"TCCOUNT\",\"FCOUNT\",\"FAILUREPERCENTAGE\",";
                strQry += " '" + objReport.sFromDate + "' AS \"FROMDATE\",'" + objReport.sTodate + "' AS \"TODATE\",TO_CHAR(CURRENT_DATE,'DD-MON-YYYY') AS \"CURRENTDATE\",ROW_NUMBER() OVER (ORDER BY \"TM_ID\") ";
                strQry += " ,\"COILTYPE\" FROM \"TBLTRANSMAKES\" A,";
                strQry += " (SELECT \"TC_MAKE_ID\", COUNT(DISTINCT(\"TC_CODE\")) \"TCCOUNT\",COUNT(\"DF_EQUIPMENT_ID\") \"FCOUNT\" ";
                strQry += "  ,ROUND ((COUNT(\"DF_EQUIPMENT_ID\")/COUNT(DISTINCT(\"TC_CODE\"))) * 100) \"FAILUREPERCENTAGE\" ";
                strQry += " ,(case when \"EST_FAIL_TYPE\" = 1 then 'SINGLECOIL' WHEN \"EST_FAIL_TYPE\" = 2 then 'MULTICOIL' END ) as \"COILTYPE\" FROM \"TBLTCMASTER\" LEFT OUTER JOIN \"TBLDTCFAILURE\" ON \"DF_EQUIPMENT_ID\"= \"TC_CODE\" AND  CAST(\"DF_LOC_CODE\" AS TEXT) LIKE '" + objReport.sOfficeCode + "%' AND ";
                if (objReport.sFeeder != null)
                {
                    strQry += "  SUBSTR(CAST(\"DF_DTC_CODE\"AS TEXT),1," + Constants.Feeder + ")='" + objReport.sFeeder + "'  AND";
                }
                if (objReport.sTodate == null && (objReport.sFromDate != null))
                {
                    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD')";
                    //strQry += " DF_DATE BETWEEN to_date('" + objReport.sFromDate + "','DD/MM/YYYY')";
                }
                if (objReport.sFromDate == null && (objReport.sTodate != null))
                {
                    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                }
                if (objReport.sFromDate == null && objReport.sTodate == null)
                {
                    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                }
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                }

                //if (objReport.sFromDate != null || objReport.sTodate != null)
                //{
                //    DateTime DFromDate = DateTime.ParseExact(objReport.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    objReport.sFromDate = DFromDate.ToString("yyyyMMdd");
                //    DateTime DToDate = DateTime.ParseExact(objReport.sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    objReport.sTodate = DToDate.ToString("yyyyMMdd");

                //    strQry += " WHERE TO_CHAR(DF_DATE,'YYYYMMDD')>='" + objReport.sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + objReport.sTodate + "'  ";
                //}
                strQry += " LEFT JOIN \"TBLESTIMATIONDETAILS\" on \"DF_ID\"=\"EST_FAILUREID\" WHERE COALESCE(\"TC_CODE\",0)!=0 and \"DF_EQUIPMENT_ID\"!=0  AND \"EST_FAIL_TYPE\"=" + objReport.sFailType + " GROUP BY \"TC_MAKE_ID\",\"EST_FAIL_TYPE\" ORDER BY COUNT(\"DF_EQUIPMENT_ID\") DESC) B WHERE A.\"TM_ID\"= B.\"TC_MAKE_ID\"";
                strQry += "   ORDER BY \"FCOUNT\" DESC LIMIT " + objReport.sMake + " ";


                dtRDetailedReport = ObjCon.FetchDataTable(strQry);
                return dtRDetailedReport;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtRDetailedReport;

            }
        }

        //MakeWise Details
        public DataTable PrintMakeWiseDetails(clsReports objReport)
        {
            DataTable dtRDetailedReport = new DataTable();
            string strQry = string.Empty;

            try
            {

                //strQry = " SELECT TO_CHAR(UPTO25)UPTO25,TO_CHAR(A2663)A2663,TO_CHAR(B64100)B64100, TO_CHAR(C101200)C101200,TO_CHAR(D201250)D201250,";
                //strQry += " TO_CHAR(ABOVE250)ABOVE250,TO_CHAR(CONT_FAIL_ID) AS FCOUNT,TO_CHAR(TOTAL_TC_COUNT) AS TCOUNT,";
                //strQry += " TO_CHAR(ROUND((NVL(CONT_FAIL_ID,0)/NVL(TOTAL_TC_COUNT,0))*100)) AS PER,TM_NAME as MakeName FROM (SELECT SUM(CASE WHEN TC_CAPACITY";
                //strQry += " BETWEEN 0 AND 25 THEN 1 ELSE 0 END)  UPTO25, ";
                //strQry += " SUM(CASE WHEN TC_CAPACITY BETWEEN 26 AND 63 THEN 1 ELSE 0 END)  A2663,";
                //strQry += " SUM(CASE WHEN TC_CAPACITY BETWEEN 64 AND 100 THEN 1 ELSE 0 END)  B64100,";
                //strQry += " SUM(CASE WHEN TC_CAPACITY BETWEEN 101 AND 200 THEN 1 ELSE 0 END)  C101200,";
                //strQry += " SUM(CASE WHEN TC_CAPACITY BETWEEN 201 AND 250 THEN 1 ELSE 0 END)  D201250,";
                //strQry += " SUM(CASE WHEN TC_CAPACITY>250 THEN 1 ELSE 0 END ) ABOVE250,TM_NAME,COUNT(DF_EQUIPMENT_ID) CONT_FAIL_ID,(SELECT COUNT(*) FROM ";
                //strQry += " TBLTCMASTER  A WHERE A.TC_MAKE_ID=TM_ID) TOTAL_TC_COUNT  FROM  TBLDTCFAILURE INNER JOIN TBLTCMASTER ON DF_EQUIPMENT_ID=TC_CODE ";
                //strQry += " RIGHT JOIN TBLTRANSMAKES ON TM_ID= TC_MAKE_ID ";
                //if (objReport.sFromDate != null || objReport.sTodate != null)
                //{
                //    strQry += "  WHERE TO_CHAR(DF_DATE,'YYYYMMDD')>='" + objReport.sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + objReport.sTodate + "'";
                //}

                //strQry += " GROUP BY TM_NAME,TM_ID)A";
                //strQry += " WHERE NVL(TOTAL_TC_COUNT,0)!=0  ORDER BY CONT_FAIL_ID DESC";


                strQry = " SELECT \"UPTO25\",\"A2663\",\"B64100\",\"C101200\", \"D201250\", \"ABOVE250\",(\"CONT_FAIL_ID\") AS \"FCOUNT\",(\"TOTAL_TC_COUNT\") AS \"TCOUNT\",'' AS \"FROMDATE\",'' AS \"TODATE\",";
                strQry += "TO_CHAR(CURRENT_DATE,'DD-MON-YYYY') AS \"CURRENTDATE\", (ROUND(CAST(COALESCE(\"CONT_FAIL_ID\",0) AS INT),CAST(COALESCE(\"TOTAL_TC_COUNT\",0)AS INT))*100) AS \"PER\",\"TM_NAME\"AS \"MAKENAME\",";
                strQry += "\"CIRCLE\",\"DIVISION\",  \"SUBDIVISION\",\"SECTION\",\"FEEDER\",\"COILTYPE\" FROM \"TBLTRANSMAKES\" A, (SELECT SUM(CASE WHEN \"TC_CAPACITY\" BETWEEN 0 AND 25 THEN 1 ELSE 0 END)  \"UPTO25\",";
                strQry += "SUM(CASE WHEN \"TC_CAPACITY\" BETWEEN 26 AND 63 THEN 1 ELSE 0 END)  \"A2663\",  SUM(CASE WHEN \"TC_CAPACITY\" BETWEEN 64 AND 100 THEN 1 ELSE 0 END) \"B64100\",";
                strQry += "SUM(CASE WHEN \"TC_CAPACITY\" BETWEEN 101 AND 200  THEN 1 ELSE 0 END)  \"C101200\",  SUM(CASE WHEN \"TC_CAPACITY\" BETWEEN 201 AND 250 THEN 1 ELSE 0 END)";
                strQry += "\"D201250\", SUM(CASE WHEN  \"TC_CAPACITY\">250 THEN 1 ELSE 0 END ) \"ABOVE250\",\"TC_MAKE_ID\",COUNT(\"DF_EQUIPMENT_ID\") \"CONT_FAIL_ID\",";
                strQry += "(SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE  CAST( \"OFF_CODE\"AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Circle + "))\"CIRCLE\", ";
                strQry += "(SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)   FROM \"VIEW_ALL_OFFICES\" WHERE CAST( \"OFF_CODE\"AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Division + "))\"DIVISION\",";
                strQry += "(SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST( \"OFF_CODE\"AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\"AS TEXT),1," + Constants.SubDivision + "))\"SUBDIVISION\",";
                strQry += "(SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)FROM \"VIEW_ALL_OFFICES\" WHERE CAST( \"OFF_CODE\"AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Section + "))\"SECTION\", ";
                if (objReport.sFeeder != null)
                {
                    strQry += "(SELECT \"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\" WHERE \"FD_FEEDER_CODE\"='" + objReport.sFeeder + "')\"FEEDER\", ";

                }
                else
                {
                    strQry += "'' as \"FEEDER\" ,";
                }
                strQry += "COUNT(DISTINCT(\"TC_CODE\")) \"TOTAL_TC_COUNT\" , (case when \"EST_FAIL_TYPE\" = 1 then 'SINGLECOIL' WHEN \"EST_FAIL_TYPE\" = 2 then 'MULTICOIL' END ) as \"COILTYPE\" FROM \"TBLTCMASTER\"  LEFT JOIN   \"TBLDTCFAILURE\" ON \"DF_EQUIPMENT_ID\"=\"TC_CODE\" LEFT JOIN \"TBLESTIMATIONDETAILS\"  on \"DF_ID\"=\"EST_FAILUREID\" INNER JOIN \"TBLTRANSMAKES\" ON ";
                strQry += "\"TM_ID\"=\"TC_MAKE_ID\"AND  CAST(\"DF_LOC_CODE\" AS TEXT) LIKE '" + objReport.sOfficeCode + "%'  AND ";

                if (objReport.sTodate == null && (objReport.sFromDate != null))
                {
                    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD')";

                }
                if (objReport.sFromDate == null && (objReport.sTodate != null))
                {
                    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                }
                if (objReport.sFromDate == null && objReport.sTodate == null)
                {
                    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                }
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                }

                //if (objReport.sFromDate != null || objReport.sTodate != null)
                //{
                //    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + objReport.sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + objReport.sTodate + "'";
                //}
                strQry += "  WHERE COALESCE(\"DF_EQUIPMENT_ID\",0)!=0 AND COALESCE(\"TC_CODE\",0)!=0 and \"EST_FAIL_TYPE\"=" + objReport.sFailType + " GROUP BY \"TM_NAME\",\"TC_MAKE_ID\",\"DF_LOC_CODE\",\"COILTYPE\")B WHERE A.\"TM_ID\"= B.\"TC_MAKE_ID\"ORDER BY \"CONT_FAIL_ID\"DESC";

                dtRDetailedReport = ObjCon.FetchDataTable(strQry);
                return dtRDetailedReport;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtRDetailedReport;
            }
        }

        //DtrRepairerWise Details
        //public DataTable PrintDtrRepairerWise(clsReports objReport)
        //{
        //    DataTable dtRDetailedReport = new DataTable();
        //    string strQry = string.Empty;
        //    try
        //    {
        //        strQry = " SELECT \"TR_OFFICECODE\",(SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE cast(\"OFF_CODE\" as text)=SUBSTR(CAST(\"TR_OFFICECODE\" AS TEXT),1," + Constants.Zone + "))\"ZONE\",(SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE cast(\"OFF_CODE\" as text)=SUBSTR(CAST(\"TR_OFFICECODE\" AS TEXT),1," + Constants.Circle + "))CIRCLE,'" + objReport.sFromDate + "' AS FROMDATE,'" + objReport.sTodate + "' AS TODATE,TO_CHAR(CURRENT_DATE,'DD-MON-YYYY') AS CURRENTDATE, ";
        //        strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  FROM \"VIEW_ALL_OFFICES\" WHERE cast(\"OFF_CODE\" as text)=SUBSTR(CAST(\"TR_OFFICECODE\" AS TEXT),1," + Constants.Division + "))DIVISION,\"TR_NAME\",";
        //        strQry += " SUM(CASE WHEN \"TC_CAPACITY\" BETWEEN 0 AND 25 THEN 1 ELSE 0 END)  UPTO25, SUM(CASE WHEN \"TC_CAPACITY\" BETWEEN 26 AND 63 THEN 1 ELSE 0 END)  ";
        //        strQry += " A2663,  SUM(CASE WHEN \"TC_CAPACITY\" BETWEEN 64 AND 100 THEN 1 ELSE 0 END)  \"B64100\", SUM(CASE WHEN \"TC_CAPACITY\" BETWEEN 101 AND 200  THEN 1 ";
        //        strQry += " ELSE 0 END)  C101200, SUM(CASE WHEN \"TC_CAPACITY\" BETWEEN 201 AND 250 THEN 1 ELSE 0 END)  D201250, SUM(CASE WHEN \"TC_CAPACITY\">250 THEN 1 ";
        //        strQry += " ELSE 0 END ) ABOVE250,";
        //        strQry += " SUM(CASE WHEN (ROUND(CURRENT_DATE -  \"RSM_ISSUE_DATE\"))  BETWEEN 0 AND 30 THEN  1 ELSE 0 END) WITHIN30,";
        //        strQry += " SUM(CASE WHEN (ROUND(CURRENT_DATE -  \"RSM_ISSUE_DATE\"))  BETWEEN 31 AND 60 THEN  1 ELSE 0 END) WITHIN60,";
        //        strQry += " SUM(CASE WHEN (ROUND(CURRENT_DATE -  \"RSM_ISSUE_DATE\"))  BETWEEN 61 AND 90 THEN  1 ELSE 0 END) WITHING90,";
        //        strQry += " SUM(CASE WHEN (ROUND(CURRENT_DATE -  \"RSM_ISSUE_DATE\"))  > 91 THEN  1 ELSE 0 END) ABOVE90,COUNT(*) TOTAL  ";
        //        strQry += " FROM  \"TBLREPAIRSENTMASTER\",\"TBLREPAIRSENTDETAILS\", \"TBLTRANSREPAIRER\",\"TBLTCMASTER\" WHERE \"RSM_ID\"=\"RSD_RSM_ID\" AND \"TR_ID\"=\"RSM_SUPREP_ID\" AND cast(\"TR_OFFICECODE\" as text) like'" + objReport.sOfficeCode + "%' AND ";

        //        if (objReport.sTodate == null && (objReport.sFromDate != null))
        //        {
        //            strQry += " TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD')";
        //            //strQry += " DF_DATE BETWEEN to_date('" + objReport.sFromDate + "','DD/MM/YYYY')";
        //        }
        //        if (objReport.sFromDate == null && (objReport.sTodate != null))
        //        {
        //            strQry += " TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
        //        }
        //        if (objReport.sFromDate == null && objReport.sTodate == null)
        //        {
        //            strQry += " TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
        //        }
        //        if (objReport.sFromDate != null && objReport.sTodate != null)
        //        {
        //            strQry += " TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
        //        }
        //        //if (objReport.sFromDate != null || objReport.sTodate != null)
        //        //{

        //        //    strQry += "  AND TO_CHAR(RSM_ISSUE_DATE,'YYYYMMDD')>='" + objReport.sFromDate + "' AND TO_CHAR(RSM_ISSUE_DATE,'YYYYMMDD')<='" + objReport.sTodate + "'";
        //        //}
        //        strQry += " AND \"RSD_TC_CODE\"=\"TC_CODE\"GROUP BY \"TR_OFFICECODE\",\"TR_NAME\" ORDER BY \"TR_OFFICECODE\"";

        //        dtRDetailedReport = ObjCon.FetchDataTable(strQry);
        //        return dtRDetailedReport;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "PRINTDTRREPAIRERWISE");
        //        return dtRDetailedReport;

        //    }
        //}
        //RepairerWIse details

        public DataTable PrintDtrRepairerWise(clsReports objReport)
        {
            DataTable dtRDetailedReport = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = " SELECT \"TRO_OFF_CODE\",(SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE cast(\"OFF_CODE\" as text)=SUBSTR(CAST(\"TRO_OFF_CODE\" AS TEXT),1," + Constants.Zone + "))\"ZONE\",(SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE cast(\"OFF_CODE\" as text)=SUBSTR(CAST(\"TRO_OFF_CODE\" AS TEXT),1," + Constants.Circle + "))CIRCLE,'" + objReport.sFromDate + "' AS FROMDATE,'" + objReport.sTodate + "' AS TODATE,TO_CHAR(CURRENT_DATE,'DD-MON-YYYY') AS CURRENTDATE, ";
                strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)  FROM \"VIEW_ALL_OFFICES\" WHERE cast(\"OFF_CODE\" as text)=SUBSTR(CAST(\"TRO_OFF_CODE\" AS TEXT),1," + Constants.Division + "))DIVISION,\"TR_NAME\",";
                strQry += " SUM(CASE WHEN \"TC_CAPACITY\" BETWEEN 0 AND 25 THEN 1 ELSE 0 END)  UPTO25, SUM(CASE WHEN \"TC_CAPACITY\" BETWEEN 26 AND 63 THEN 1 ELSE 0 END)  ";
                strQry += " A2663,  SUM(CASE WHEN \"TC_CAPACITY\" BETWEEN 64 AND 100 THEN 1 ELSE 0 END)  \"B64100\", SUM(CASE WHEN \"TC_CAPACITY\" BETWEEN 101 AND 200  THEN 1 ";
                strQry += " ELSE 0 END)  C101200, SUM(CASE WHEN \"TC_CAPACITY\" BETWEEN 201 AND 250 THEN 1 ELSE 0 END)  D201250, SUM(CASE WHEN \"TC_CAPACITY\">250 THEN 1 ";
                strQry += " ELSE 0 END ) ABOVE250,";
                strQry += " SUM(CASE WHEN (ROUND(CURRENT_DATE -  \"RSM_ISSUE_DATE\"))  BETWEEN 0 AND 30 THEN  1 ELSE 0 END) WITHIN30,";
                strQry += " SUM(CASE WHEN (ROUND(CURRENT_DATE -  \"RSM_ISSUE_DATE\"))  BETWEEN 31 AND 60 THEN  1 ELSE 0 END) WITHIN60,";
                strQry += " SUM(CASE WHEN (ROUND(CURRENT_DATE -  \"RSM_ISSUE_DATE\"))  BETWEEN 61 AND 90 THEN  1 ELSE 0 END) WITHING90,";
                strQry += " SUM(CASE WHEN (ROUND(CURRENT_DATE -  \"RSM_ISSUE_DATE\"))  > 91 THEN  1 ELSE 0 END) ABOVE90,COUNT(*) TOTAL  ";
                strQry += " FROM  \"TBLREPAIRSENTMASTER\",\"TBLREPAIRSENTDETAILS\", \"TBLTRANSREPAIRER\",\"TBLTCMASTER\",\"TBLTRANSREPAIREROFFCODE\" WHERE  \"TR_ID\"=\"TRO_TR_ID\"  AND\"RSM_ID\"=\"RSD_RSM_ID\" AND \"TR_ID\"=\"RSM_SUPREP_ID\" AND cast(\"TRO_OFF_CODE\" as text) like'" + objReport.sOfficeCode + "%' AND ";

                if (objReport.sTodate == null && (objReport.sFromDate != null))
                {
                    strQry += " TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD')";
                    //strQry += " DF_DATE BETWEEN to_date('" + objReport.sFromDate + "','DD/MM/YYYY')";
                }
                if (objReport.sFromDate == null && (objReport.sTodate != null))
                {
                    strQry += " TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                }
                if (objReport.sFromDate == null && objReport.sTodate == null)
                {
                    strQry += " TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                }
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                }
                //if (objReport.sFromDate != null || objReport.sTodate != null)
                //{

                //    strQry += "  AND TO_CHAR(RSM_ISSUE_DATE,'YYYYMMDD')>='" + objReport.sFromDate + "' AND TO_CHAR(RSM_ISSUE_DATE,'YYYYMMDD')<='" + objReport.sTodate + "'";
                //}
                strQry += " AND \"RSD_TC_CODE\"=\"TC_CODE\"GROUP BY \"TRO_OFF_CODE\",\"TR_NAME\" ORDER BY \"TRO_OFF_CODE\"";

                dtRDetailedReport = ObjCon.FetchDataTable(strQry);
                return dtRDetailedReport;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtRDetailedReport;

            }
        }

        //public DataTable PrintRePairerwise(clsReports objReport)
        //{
        //    DataTable dtRDetailedReport = new DataTable();
        //    string strQry = string.Empty;
        //    //WHERE MORETHANONCE!=0
        //    try
        //    {

        //        strQry = " SELECT \"TR_NAME\" ,SUM(\"MORETHANONCE\") AS \"MORETHANONCE\",'" + objReport.sFromDate + "' AS \"FROMDATE\",'" + objReport.sTodate + "' AS \"TODATE\", ";
        //        strQry += " TO_CHAR(NOW(),'DD-MON-YYYY') AS \"CURRENTDATE\"  FROM  (SELECT \"TR_ID\",\"TR_NAME\",\"RSD_TC_CODE\",COUNT(*), CASE WHEN ";
        //        strQry += " COUNT(*) > 1 THEN COUNT(*) ELSE 0 END \"MORETHANONCE\",  CASE WHEN COUNT(*) = 1 THEN COUNT(*)  ELSE 0 END \"ONLYONCE\", ";
        //        strQry += " (SELECT \"OFF_NAME\" FROM \"VIEW_OFFICES\"WHERE  CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"TR_OFFICECODE\" AS TEXT),0," + Constants.Circle + ")) ";
        //        strQry += " \"CIRCLE\",  (SELECT \"OFF_NAME\"  FROM \"VIEW_OFFICES\"  WHERE CAST(\"OFF_CODE\" AS TEXT)= ";
        //        strQry += " SUBSTR(CAST(\"TR_OFFICECODE\" AS TEXT),0," + Constants.Division + ")) \"DIVISION\",(SELECT \"ZO_NAME\" FROM \"TBLZONE\" WHERE CAST(\"ZO_ID\"AS TEXT)=SUBSTR(CAST(\"TR_OFFICECODE\" AS TEXT),1," + Constants.Zone + ")) as \"ZONE\"  FROM \"TBLTRANSREPAIRER\",\"TBLREPAIRSENTMASTER\", ";
        //        strQry += " \"TBLREPAIRSENTDETAILS\"    WHERE \"RSM_ID\"=\"RSD_RSM_ID\" AND \"TR_ID\"=\"RSM_SUPREP_ID\" AND CAST(\"TR_OFFICECODE\" AS TEXT) ";
        //        strQry += " LIKE '" + objReport.sOfficeCode + "%'  AND ";

        //        if (objReport.sTodate == null && (objReport.sFromDate != null))
        //        {
        //            strQry += " TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<=TO_CHAR(NOW(),'YYYY/MM/DD')";

        //        }
        //        if (objReport.sFromDate == null && (objReport.sTodate != null))
        //        {
        //            strQry += " TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
        //        }
        //        if (objReport.sFromDate == null && objReport.sTodate == null)
        //        {
        //            strQry += " TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<=TO_CHAR(NOW(),'YYYY/MM/DD') ";
        //        }
        //        if (objReport.sFromDate != null && objReport.sTodate != null)
        //        {
        //            strQry += " TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
        //        }

        //        strQry += " GROUP BY \"TR_NAME\",\"RSD_TC_CODE\",\"TR_ID\",\"TR_OFFICECODE\"ORDER BY \"TR_NAME\",\"TR_ID\") A WHERE \"MORETHANONCE\" !=0 GROUP BY \"TR_NAME\"";

        //        dtRDetailedReport = ObjCon.FetchDataTable(strQry);
        //        return dtRDetailedReport;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "PRINTREPAIRERWISE");
        //        return dtRDetailedReport;
        //    }
        //}

        //RepairerWise Others Details

        public DataTable PrintRePairerwise(clsReports objReport)
        {
            DataTable dtRDetailedReport = new DataTable();
            string strQry = string.Empty;
            //WHERE MORETHANONCE!=0
            try
            {

                strQry = " SELECT \"TR_NAME\" ,SUM(\"MORETHANONCE\") AS \"MORETHANONCE\",'" + objReport.sFromDate + "' AS \"FROMDATE\",'" + objReport.sTodate + "' AS \"TODATE\", ";
                strQry += " TO_CHAR(NOW(),'DD-MON-YYYY') AS \"CURRENTDATE\"  FROM  (SELECT \"TR_ID\",\"TR_NAME\",\"RSD_TC_CODE\",COUNT(*), CASE WHEN ";
                strQry += " COUNT(*) > 1 THEN COUNT(*) ELSE 0 END \"MORETHANONCE\",  CASE WHEN COUNT(*) = 1 THEN COUNT(*)  ELSE 0 END \"ONLYONCE\", ";
                strQry += " (SELECT \"OFF_NAME\" FROM \"VIEW_OFFICES\"WHERE  CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"TRO_OFF_CODE\" AS TEXT),0," + Constants.Circle + ")) ";
                strQry += " \"CIRCLE\",  (SELECT \"OFF_NAME\"  FROM \"VIEW_OFFICES\"  WHERE CAST(\"OFF_CODE\" AS TEXT)= ";
                strQry += " SUBSTR(CAST(\"TRO_OFF_CODE\" AS TEXT),0," + Constants.Division + ")) \"DIVISION\",(SELECT \"ZO_NAME\" FROM \"TBLZONE\" WHERE CAST(\"ZO_ID\"AS TEXT)=SUBSTR(CAST(\"TRO_OFF_CODE\" AS TEXT),1," + Constants.Zone + ")) as \"ZONE\"  FROM \"TBLTRANSREPAIRER\",\"TBLREPAIRSENTMASTER\", ";
                strQry += " \"TBLREPAIRSENTDETAILS\",\"TBLTRANSREPAIREROFFCODE\"    WHERE \"TR_ID\"=\"TRO_TR_ID\" AND \"RSM_ID\"=\"RSD_RSM_ID\" AND \"TR_ID\"=\"RSM_SUPREP_ID\" AND CAST(\"TRO_OFF_CODE\" AS TEXT) ";
                strQry += " LIKE '" + objReport.sOfficeCode + "%'  AND ";

                if (objReport.sTodate == null && (objReport.sFromDate != null))
                {
                    strQry += " TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<=TO_CHAR(NOW(),'YYYY/MM/DD')";

                }
                if (objReport.sFromDate == null && (objReport.sTodate != null))
                {
                    strQry += " TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                }
                if (objReport.sFromDate == null && objReport.sTodate == null)
                {
                    strQry += " TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<=TO_CHAR(NOW(),'YYYY/MM/DD') ";
                }
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                }

                strQry += " GROUP BY \"TR_NAME\",\"RSD_TC_CODE\",\"TR_ID\",\"TRO_OFF_CODE\"ORDER BY \"TR_NAME\",\"TR_ID\") A WHERE \"MORETHANONCE\" !=0 GROUP BY \"TR_NAME\"";

                dtRDetailedReport = ObjCon.FetchDataTable(strQry);
                return dtRDetailedReport;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtRDetailedReport;
            }
        }

        //public DataTable PrintRePairerOtherswise(clsReports objReport)
        //{
        //    DataTable dtRDetailedReport = new DataTable();
        //    string strQry = string.Empty;

        //    try
        //    {

        //        strQry = " SELECT '" + objReport.sFromDate + "' AS \"FROMDATE\",'" + objReport.sTodate + "' AS \"TODATE\",TO_CHAR(NOW(),'DD-MON-YYYY') AS ";
        //        strQry += " \"CURRENTDATE\",SUM(\"ONLYONCE\")\"FAILURE\" FROM  (SELECT \"TR_NAME\",\"RSD_TC_CODE\",COUNT(*) AS \"COUNT\", CASE WHEN ";
        //        strQry += " COUNT(*) > 1  THEN COUNT(*) ELSE 0 END \"MORETHANONCE\",  CASE WHEN COUNT(*) = 1 THEN COUNT(*) ELSE 0 END \"ONLYONCE\", ";
        //        strQry += " (SELECT \"OFF_NAME\" FROM \"VIEW_OFFICES\" WHERE  CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"TR_OFFICECODE\" AS TEXT),0," + Constants.Circle + ")) ";
        //        strQry += " \"CIRCLE\", (SELECT \"OFF_NAME\" FROM \"VIEW_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"TR_OFFICECODE\" AS TEXT),0," + Constants.Division + ")) ";
        //        strQry += " \"DIVISION\", (SELECT \"OFF_NAME\" FROM \"VIEW_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"TR_OFFICECODE\" AS TEXT),0," + Constants.Zone + ")) ";
        //        strQry += " \"ZONE\"   FROM \"TBLTRANSREPAIRER\",\"TBLREPAIRSENTMASTER\",\"TBLREPAIRSENTDETAILS\"  WHERE \"RSM_ID\"=\"RSD_RSM_ID\" ";
        //        strQry += " AND \"TR_ID\"=\"RSM_SUPREP_ID\"  AND CAST(\"TR_OFFICECODE\" AS TEXT) LIKE '" + objReport.sOfficeCode + "%' AND ";

        //        if (objReport.sTodate == null && (objReport.sFromDate != null))
        //        {
        //            strQry += " TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<=TO_CHAR(NOW(),'YYYY/MM/DD')";

        //        }
        //        if (objReport.sFromDate == null && (objReport.sTodate != null))
        //        {
        //            strQry += " TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
        //        }
        //        if (objReport.sFromDate == null && objReport.sTodate == null)
        //        {
        //            strQry += " TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<=TO_CHAR(NOW(),'YYYY/MM/DD') ";
        //        }
        //        if (objReport.sFromDate != null && objReport.sTodate != null)
        //        {
        //            strQry += " TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
        //        }

        //        strQry += " GROUP BY \"TR_NAME\",\"RSD_TC_CODE\",\"TR_OFFICECODE\" ORDER BY \"TR_NAME\") A   GROUP BY \"TR_NAME\" ";


        //        dtRDetailedReport = ObjCon.FetchDataTable(strQry);
        //        return dtRDetailedReport;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "PRINTREPAIRERWISE");
        //        return dtRDetailedReport;

        //    }
        //}

        public DataTable PrintRePairerOtherswise(clsReports objReport)
        {
            DataTable dtRDetailedReport = new DataTable();
            string strQry = string.Empty;

            try
            {

                strQry = " SELECT '" + objReport.sFromDate + "' AS \"FROMDATE\",'" + objReport.sTodate + "' AS \"TODATE\",TO_CHAR(NOW(),'DD-MON-YYYY') AS ";
                strQry += " \"CURRENTDATE\",SUM(\"ONLYONCE\")\"FAILURE\" FROM  (SELECT \"TR_NAME\",\"RSD_TC_CODE\",COUNT(*) AS \"COUNT\", CASE WHEN ";
                strQry += " COUNT(*) > 1  THEN COUNT(*) ELSE 0 END \"MORETHANONCE\",  CASE WHEN COUNT(*) = 1 THEN COUNT(*) ELSE 0 END \"ONLYONCE\", ";
                strQry += " (SELECT \"OFF_NAME\" FROM \"VIEW_OFFICES\" WHERE  CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"TRO_OFF_CODE\" AS TEXT),0," + Constants.Circle + ")) ";
                strQry += " \"CIRCLE\", (SELECT \"OFF_NAME\" FROM \"VIEW_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"TRO_OFF_CODE\" AS TEXT),0," + Constants.Division + ")) ";
                strQry += " \"DIVISION\", (SELECT \"OFF_NAME\" FROM \"VIEW_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"TRO_OFF_CODE\" AS TEXT),0," + Constants.Zone + ")) ";
                strQry += " \"ZONE\"   FROM \"TBLTRANSREPAIRER\",\"TBLREPAIRSENTMASTER\",\"TBLREPAIRSENTDETAILS\",\"TBLTRANSREPAIREROFFCODE\"  WHERE \"RSM_ID\"=\"RSD_RSM_ID\" AND \"TR_ID\"=\"TRO_TR_ID\" ";
                strQry += " AND \"TR_ID\"=\"RSM_SUPREP_ID\"  AND CAST(\"TRO_OFF_CODE\" AS TEXT) LIKE '" + objReport.sOfficeCode + "%' AND ";

                if (objReport.sTodate == null && (objReport.sFromDate != null))
                {
                    strQry += " TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<=TO_CHAR(NOW(),'YYYY/MM/DD')";

                }
                if (objReport.sFromDate == null && (objReport.sTodate != null))
                {
                    strQry += " TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                }
                if (objReport.sFromDate == null && objReport.sTodate == null)
                {
                    strQry += " TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<=TO_CHAR(NOW(),'YYYY/MM/DD') ";
                }
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                }

                strQry += " GROUP BY \"TR_NAME\",\"RSD_TC_CODE\",\"TRO_OFF_CODE\" ORDER BY \"TR_NAME\") A   GROUP BY \"TR_NAME\" ";


                dtRDetailedReport = ObjCon.FetchDataTable(strQry);
                return dtRDetailedReport;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtRDetailedReport;

            }
        }

        //public DataTable TransformerWiseDetailsCompleted(clsReports objReport)
        //{
        //    string strQry = string.Empty;
        //    DataTable dtTransWisecompleted = new DataTable();
        //    try
        //    {

        //        strQry = "SELECT  (SELECT \"ZO_NAME\" FROM \"TBLZONE\" WHERE  CAST(\"ZO_ID\" AS TEXT)=SUBSTR(CAST(A.\"OFFCODE\" ";
        //        strQry += "AS TEXT),1," + Constants.Zone + "))\"ZONE\",\"CM_CIRCLE_NAME\" AS \"CIRCLE\"";
        //        strQry += ",CASE WHEN \"IND_INSP_DATE\" IS NOT NULL THEN 'YES' ELSE 'NO'  END \"TESTING_COMPLETE\",  (SELECT \"TR_NAME\"";
        //        strQry += "FROM \"TBLTRANSREPAIRER\" WHERE \"TR_ID\"=\"RSM_SUPREP_ID\")\"REPAIRER_NAME\",(SELECT \"TR_ADDRESS\" FROM \"TBLTRANSREPAIRER\" ";
        //        strQry += "WHERE \"TR_ID\"=\"RSM_SUPREP_ID\")  \"REPAIRER_ADDRESS\" ,TO_CHAR(\"RSM_ISSUE_DATE\",'DD-MON-YYYY')  AS \"ISSUED_ON\",CAST(\"TC_CODE\" AS TEXT) \"TC_CODE\"";
        //        strQry += ",(SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\")\"TM_NAME\",CAST(\"TC_CAPACITY\" AS TEXT)\"TC_CAPACITY\"";
        //        strQry += ",\"RSM_GUARANTY_TYPE\",\"RSM_PO_NO\",TO_CHAR(\"RSM_PO_DATE\",'DD-MON-YYYY') AS \"RSM_PO_DATE\" ,'' AS \"FROMDATE\", '' AS \"TODATE\" ";
        //        strQry += ",TO_CHAR(NOW(),'DD-MON-YYYY') AS \"CURRENTDATE\",ROUND(DATE_PART('DAY',AGE( NOW(),\"RSM_ISSUE_DATE\"))) AS \"PENDING_DAYS\"  FROM  ";
        //        strQry += "(SELECT DISTINCT * FROM \"TBLREPAIRSENTDETAILS\",\"TBLREPAIRSENTMASTER\",\"TBLTCMASTER\",\"TBLSTOREMAST\",(SELECT DISTINCT \"CM_CIRCLE_NAME\",\"CM_CIRCLE_CODE\" AS \"OFFCODE\" FROM \"TBLCIRCLE\",\"TBLSTOREMAST\",\"TBLSTOREOFFCODE\" WHERE";
        //        strQry += "\"SM_ID\"=\"STO_SM_ID\" AND  CAST(\"STO_OFF_CODE\" AS TEXT) LIKE '" + objReport.sOfficeCode + "%' AND substr(cast(\"STO_OFF_CODE\" as text),1," + Constants.Circle + ")=cast(\"CM_CIRCLE_CODE\" as text))B WHERE ";
        //        strQry += "\"RSM_ID\"= \"RSD_RSM_ID\"  AND \"TC_CODE\"= \"RSD_TC_CODE\" AND \"RSM_DIV_CODE\"= \"SM_ID\" AND \"RSD_DELIVARY_DATE\" IS NOT NULL ";


        //        //strQry = " SELECT (SELECT \"OFF_NAME\" FROM \"VIEW_OFFICES\" WHERE  CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"RSM_DIV_CODE\" AS TEXT),1," + Constants.Zone + "))\"ZONE\",(SELECT \"OFF_NAME\" FROM \"VIEW_OFFICES\" WHERE  CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"RSM_DIV_CODE\" AS TEXT),0," + Constants.Circle + ")) ";
        //        //strQry += " \"CIRCLE\",\"OFF_NAME\" AS \"DIVISION\",CASE WHEN  \"IND_INSP_DATE\" IS NOT NULL THEN 'YES' ELSE 'NO'  END \"TESTING_COMPLETE\", ";
        //        //strQry += " (SELECT \"TR_NAME\" FROM \"TBLTRANSREPAIRER\"   WHERE \"TR_ID\"=\"RSM_SUPREP_ID\")\"REPAIRER_NAME\",(SELECT \"TR_ADDRESS\"FROM ";
        //        //strQry += " \"TBLTRANSREPAIRER\" WHERE \"TR_ID\"=\"RSM_SUPREP_ID\")  \"REPAIRER_ADDRESS\" ,TO_CHAR(\"RSM_ISSUE_DATE\",'DD-MON-YYYY') ";
        //        //strQry += " AS \"ISSUED_ON\",CAST(\"TC_CODE\" AS TEXT) \"TC_CODE\",(SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\") ";
        //        //strQry += " \"TM_NAME\",CAST(\"TC_CAPACITY\" AS TEXT)\"TC_CAPACITY\",\"RSM_GUARANTY_TYPE\",\"RSM_PO_NO\", ";
        //        //strQry += " TO_CHAR(\"RSM_PO_DATE\",'DD-MON-YYYY') AS \"RSM_PO_DATE\" ,'" + objReport.sTempFromDate + "' AS \"FROMDATE\", '" + objReport.sTempTodate + "' AS \"TODATE\" , ";
        //        //strQry += " TO_CHAR(NOW(),'DD-MON-YYYY') AS \"CURRENTDATE\",ROUND(DATE_PART('DAY',AGE( NOW(),\"RSM_ISSUE_DATE\"))) AS \"PENDING_DAYS\" ";
        //        //strQry += " FROM  (SELECT * FROM \"TBLREPAIRSENTDETAILS\",\"TBLREPAIRSENTMASTER\",\"TBLTCMASTER\",\"VIEW_ALL_OFFICES\" WHERE ";
        //        //strQry += " \"RSM_ID\"= \"RSD_RSM_ID\"  AND \"TC_CODE\"= \"RSD_TC_CODE\" AND \"RSM_DIV_CODE\"= \"OFF_CODE\" AND \"RSD_DELIVARY_DATE\"IS NULL ";
        //        //strQry += " AND  \"RSM_DIV_CODE\"= \"OFF_CODE\"  AND \"RSD_DELIVARY_DATE\" IS NULL AND  CAST(\"OFF_CODE\" AS TEXT) LIKE '" + objReport.sOfficeCode + "%'   ";

        //        if (objReport.sRepriername != null)
        //        {
        //            strQry += "  AND \"RSM_SUPREP_ID\"='" + objReport.sRepriername + "'  ";
        //        }

        //        if (objReport.sTodate == null && (objReport.sFromDate != null))
        //        {
        //            strQry += " AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD') >= '" + objReport.sFromDate + "' AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<=TO_CHAR(NOW(),'YYYY/MM/DD')";
        //        }

        //        if (objReport.sFromDate == null && (objReport.sTodate != null))
        //        {
        //            strQry += " AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD') <='" + objReport.sTodate + "' ";
        //        }
        //        if (objReport.sFromDate == null && objReport.sTodate == null)
        //        {
        //            strQry += " AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<=TO_CHAR(NOW(),'YYYY/MM/DD') ";
        //        }
        //        if (objReport.sFromDate != null && objReport.sTodate != null)
        //        {
        //            strQry += "AND  TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
        //        }

        //        strQry += " ) A LEFT JOIN \"TBLINSPECTIONDETAILS\" ON \"RSD_ID\"=\"IND_RSD_ID\" AND COALESCE(\"IND_INSP_RESULT\",0)=1 ORDER BY \"SM_NAME\",DATE_PART('DAY',AGE( NOW(),\"RSM_ISSUE_DATE\")) DESC";

        //        dtTransWisecompleted = ObjCon.FetchDataTable(strQry);
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "TransformerWiseDetails");
        //        return dtTransWisecompleted;
        //    }
        //    return dtTransWisecompleted;
        //}

        public DataTable TransformerWiseDetailsCompleted(clsReports objReport)
        {
            string strQry = string.Empty;
            DataTable dtTransWisecompleted = new DataTable();
            try
            {

                strQry = "SELECT  (SELECT \"ZO_NAME\" FROM \"TBLZONE\" WHERE  CAST(\"ZO_ID\" AS TEXT)=SUBSTR(CAST(A.\"OFFCODE\" ";
                strQry += "AS TEXT),1," + Constants.Zone + "))\"ZONE\",\"CM_CIRCLE_NAME\" AS \"CIRCLE\"";
                strQry += ",CASE WHEN \"IND_INSP_DATE\" IS NOT NULL THEN 'YES' ELSE 'NO'  END \"TESTING_COMPLETE\",  (SELECT \"TR_NAME\"";
                strQry += "FROM \"TBLTRANSREPAIRER\" WHERE \"TR_ID\"=\"RSM_SUPREP_ID\")\"REPAIRER_NAME\",(SELECT \"TR_ADDRESS\" FROM \"TBLTRANSREPAIRER\" ";
                strQry += "WHERE \"TR_ID\"=\"RSM_SUPREP_ID\")  \"REPAIRER_ADDRESS\" ,TO_CHAR(\"RSM_ISSUE_DATE\",'DD-MON-YYYY')  AS \"ISSUED_ON\",CAST(\"TC_CODE\" AS TEXT) \"TC_CODE\"";
                strQry += ",(SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\")\"TM_NAME\",CAST(\"TC_CAPACITY\" AS TEXT)\"TC_CAPACITY\"";
                strQry += ",\"RSM_GUARANTY_TYPE\",\"RSM_PO_NO\",TO_CHAR(\"RSM_PO_DATE\",'DD-MON-YYYY') AS \"RSM_PO_DATE\" ,'' AS \"FROMDATE\", '' AS \"TODATE\" ";
                strQry += ",TO_CHAR(NOW(),'DD-MON-YYYY') AS \"CURRENTDATE\",ROUND(DATE_PART('DAY',AGE( NOW(),\"RSM_ISSUE_DATE\"))) AS \"PENDING_DAYS\"  FROM  ";
                strQry += "(SELECT DISTINCT * FROM \"TBLREPAIRSENTDETAILS\",\"TBLREPAIRSENTMASTER\",\"TBLTCMASTER\",\"TBLDIVISION\",(SELECT DISTINCT \"CM_CIRCLE_NAME\",\"CM_CIRCLE_CODE\" AS \"OFFCODE\" FROM \"TBLCIRCLE\",\"TBLDIVISION\",\"TBLREPAIRSENTDETAILS\",\"TBLREPAIRSENTMASTER\" WHERE";
                strQry += "  \"RSM_NEW_DIV_CODE\"= \"DIV_CODE\" and CAST(\"RSM_NEW_DIV_CODE\" AS TEXT) LIKE '" + objReport.sOfficeCode + "%' AND substr(cast(\"RSM_NEW_DIV_CODE\" as text),1," + Constants.Circle + ")=cast(\"CM_CIRCLE_CODE\" as text) and  \"RSM_ID\"= \"RSD_RSM_ID\" AND \"RSD_DELIVARY_DATE\" IS NOT NULL  AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<=TO_CHAR(NOW(),'YYYY/MM/DD'))B WHERE ";
                strQry += "\"RSM_ID\"= \"RSD_RSM_ID\"  AND \"TC_CODE\"= \"RSD_TC_CODE\" and \"RSM_NEW_DIV_CODE\"= \"DIV_CODE\"  AND \"RSD_DELIVARY_DATE\" IS NOT NULL ";


                //strQry = " SELECT (SELECT \"OFF_NAME\" FROM \"VIEW_OFFICES\" WHERE  CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"RSM_DIV_CODE\" AS TEXT),1," + Constants.Zone + "))\"ZONE\",(SELECT \"OFF_NAME\" FROM \"VIEW_OFFICES\" WHERE  CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"RSM_DIV_CODE\" AS TEXT),0," + Constants.Circle + ")) ";
                //strQry += " \"CIRCLE\",\"OFF_NAME\" AS \"DIVISION\",CASE WHEN  \"IND_INSP_DATE\" IS NOT NULL THEN 'YES' ELSE 'NO'  END \"TESTING_COMPLETE\", ";
                //strQry += " (SELECT \"TR_NAME\" FROM \"TBLTRANSREPAIRER\"   WHERE \"TR_ID\"=\"RSM_SUPREP_ID\")\"REPAIRER_NAME\",(SELECT \"TR_ADDRESS\"FROM ";
                //strQry += " \"TBLTRANSREPAIRER\" WHERE \"TR_ID\"=\"RSM_SUPREP_ID\")  \"REPAIRER_ADDRESS\" ,TO_CHAR(\"RSM_ISSUE_DATE\",'DD-MON-YYYY') ";
                //strQry += " AS \"ISSUED_ON\",CAST(\"TC_CODE\" AS TEXT) \"TC_CODE\",(SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\") ";
                //strQry += " \"TM_NAME\",CAST(\"TC_CAPACITY\" AS TEXT)\"TC_CAPACITY\",\"RSM_GUARANTY_TYPE\",\"RSM_PO_NO\", ";
                //strQry += " TO_CHAR(\"RSM_PO_DATE\",'DD-MON-YYYY') AS \"RSM_PO_DATE\" ,'" + objReport.sTempFromDate + "' AS \"FROMDATE\", '" + objReport.sTempTodate + "' AS \"TODATE\" , ";
                //strQry += " TO_CHAR(NOW(),'DD-MON-YYYY') AS \"CURRENTDATE\",ROUND(DATE_PART('DAY',AGE( NOW(),\"RSM_ISSUE_DATE\"))) AS \"PENDING_DAYS\" ";
                //strQry += " FROM  (SELECT * FROM \"TBLREPAIRSENTDETAILS\",\"TBLREPAIRSENTMASTER\",\"TBLTCMASTER\",\"VIEW_ALL_OFFICES\" WHERE ";
                //strQry += " \"RSM_ID\"= \"RSD_RSM_ID\"  AND \"TC_CODE\"= \"RSD_TC_CODE\" AND \"RSM_DIV_CODE\"= \"OFF_CODE\" AND \"RSD_DELIVARY_DATE\"IS NULL ";
                //strQry += " AND  \"RSM_DIV_CODE\"= \"OFF_CODE\"  AND \"RSD_DELIVARY_DATE\" IS NULL AND  CAST(\"OFF_CODE\" AS TEXT) LIKE '" + objReport.sOfficeCode + "%'   ";

                if (objReport.sRepriername != null)
                {
                    strQry += "  AND \"RSM_SUPREP_ID\"='" + objReport.sRepriername + "'  ";
                }

                if (objReport.sTodate == null && (objReport.sFromDate != null))
                {
                    strQry += " AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD') >= '" + objReport.sFromDate + "' AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<=TO_CHAR(NOW(),'YYYY/MM/DD')";
                }

                if (objReport.sFromDate == null && (objReport.sTodate != null))
                {
                    strQry += " AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD') <='" + objReport.sTodate + "' ";
                }
                if (objReport.sFromDate == null && objReport.sTodate == null)
                {
                    strQry += " AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<=TO_CHAR(NOW(),'YYYY/MM/DD') ";
                }
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += "AND  TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"RSM_ISSUE_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                }

                strQry += " ) A LEFT JOIN \"TBLINSPECTIONDETAILS\" ON \"RSD_ID\"=\"IND_RSD_ID\" AND COALESCE(\"IND_INSP_RESULT\",0)=1 ORDER BY \"DIV_NAME\",DATE_PART('DAY',AGE( NOW(),\"RSM_ISSUE_DATE\")) DESC";

                dtTransWisecompleted = ObjCon.FetchDataTable(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtTransWisecompleted;
            }
            return dtTransWisecompleted;
        }

        public DataTable WoRegDetails(clsReports objReport)
        {
            string strQry = string.Empty;
            DataTable DtWoRegDetail = new DataTable();
            try
            {

                //strQry  ="select (SELECT substr(off_name,INSTR(off_name,':')+1) FROM VIEW_ALL_OFFICES WHERE  OFF_CODE=substr (df_loc_code,0,1))CIRCLE,";
                // strQry += "  (SELECT substr(off_name,INSTR(off_name,':')+1)FROM  VIEW_ALL_OFFICES WHERE OFF_CODE=substr(df_loc_code,0,2))DIVISION,";
                // strQry += " (SELECT substr(off_name,INSTR(off_name,':')+1) FROM VIEW_ALL_OFFICES WHERE OFF_CODE=substr(df_loc_code,0,3))SUBDIVISION,";
                // strQry += "  '" + objReport.sFromDate + "' as FROMDATE,(SELECT substr(off_name,INSTR(off_name,':')+1)FROM VIEW_ALL_OFFICES WHERE OFF_CODE=substr(df_loc_code,0,4))";
                //strQry += "  SECTION,(CASE WHEN DF_ENHANCE_CAPACITY IS NULL THEN 'FAILURE' WHEN DF_ENHANCE_CAPACITY IS NOT NULL THEN 'ENHANCEMENT' END)NOMENCLATURE";
                //strQry += "  ,DF_DTC_CODE,TO_CHAR(DF_EQUIPMENT_ID)DF_EQUIPMENT_ID,TO_CHAR(EST_NO)EST_NO,'" + objReport.sTodate + "' AS TODATE,TO_CHAR(CURRENT_DATE,'DD-MON-YYYY') as today,TO_CHAR( EST_CRON,'DD-MON-YYYY')";
                //strQry += "  EST_CRON ,RSM_GUARANTY_TYPE,WO_NO,WO_NO_DECOM ,TO_CHAR( WO_DATE,'DD-MON-YYYY')WO_DATE,WO_AMT,WO_AMT_DECOM,TO_CHAR( WO_DATE_DECOM,";
                //strQry += "  'DD-MON-YYYY')WO_DATE_DECOM,CASE WHEN DF_ENHANCE_CAPACITY IS NULL THEN TC_CAPACITY WHEN DF_ENHANCE_CAPACITY IS NOT NULL THEN";
                //strQry += "  TO_NUMBER(DF_ENHANCE_CAPACITY) END AS REPLACE_CAPACITY ,TO_CHAR(TC_CAPACITY)TC_CAPACITY,DF_REASON from TBLREPAIRSENTMASTER ";
                //strQry += "  RIGHT JOIN TBLREPAIRSENTDETAILS ON RSM_ID=RSD_RSM_ID RIGHT JOIN TBLTCMASTER ON TC_CODE=RSD_TC_CODE INNER JOIN TBLDTCFAILURE";
                //strQry += "  ON DF_EQUIPMENT_ID=TC_CODE LEFT JOIN TBLESTIMATION ON DF_ID=EST_DF_ID LEFT JOIN TBLWORKORDER on df_id=WO_DF_ID LEFT JOIN ";
                //strQry += "  TBLINDENT on wo_slno=ti_wo_slno LEFT JOIN TBLDTCINVOICE on in_ti_no=ti_id LEFT JOIN TBLTCREPLACE ON tr_in_no=in_no where  ";
                #region commented inline query
                //strQry = " SELECT (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT) =SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Circle + "))CIRCLE,  ";
                //strQry += "  (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)FROM  \"VIEW_ALL_OFFICES\" WHERE CAST( \"OFF_CODE\"AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Division + "))DIVISION,";
                //strQry += "  (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST( \"OFF_CODE\"AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + "))SUBDIVISION,";
                //strQry += "   '' AS FROMDATE,(SELECT substr(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1)FROM \"VIEW_ALL_OFFICES\" WHERE CAST( \"OFF_CODE\"AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Section + "))";
                //strQry += "   \"SECTION\",(SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST( \"OFF_CODE\"AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Zone + "))\"ZONE\", ";
                ////strQry += " substr(CAST(\"DF_DTC_CODE\" AS TEXT),1," + Constants.Feeder + ")\"FD_FEEDER_CODE\",(SELECT DISTINCT \"FD_FEEDER_NAME\" FROM ";
                ////strQry += " \"TBLFEEDERMAST\" WHERE \"FD_FEEDER_CODE\"=substr(CAST(\"DF_DTC_CODE\" AS TEXT),1," + Constants.Feeder + "))\"FD_FEEDER_NAME\",";
                //strQry += " \"DT_FDRSLNO\" as \"FD_FEEDER_CODE\",(SELECT DISTINCT \"FD_FEEDER_NAME\" FROM ";
                //strQry += " \"TBLFEEDERMAST\" WHERE \"FD_FEEDER_CODE\"= \"DT_FDRSLNO\" )\"FD_FEEDER_NAME\",";
                //strQry += "\"DF_DTC_CODE\",CAST(\"DF_EQUIPMENT_ID\" AS TEXT)\"DF_EQUIPMENT_ID\",CAST(\"EST_NO\" AS TEXT)\"EST_NO\",'' AS TODATE,";
                //strQry += "   TO_CHAR(CURRENT_DATE,'DD-MON-YYYY') AS TODAY,TO_CHAR(\"EST_CRON\",'DD-MON-YYYY')  \"EST_CRON\", ";
                ////strQry+=" ( CASE WHEN \"TC_WARANTY_PERIOD\" IS NOT NULL THEN(SELECT CASE WHEN CURRENT_DATE < \"TC_WARANTY_PERIOD\" THEN \"RSD_GUARRENTY_TYPE\" ELSE 'AGP' END FROM \"TBLREPAIRSENTDETAILS\" WHERE \"RSD_WARENTY_PERIOD\" IS NOT NULL AND \"RSD_WARENTY_PERIOD\" IS NOT NULL AND \"RSD_TC_CODE\"=\"TC_CODE\") ELSE '' END) \"RSM_GUARANTY_TYPE\",\"WO_NO\",\"WO_NO_DECOM\",";
                //strQry += " \"DF_GUARANTY_TYPE\" as \"RSM_GUARANTY_TYPE\",\"WO_NO\",\"WO_NO_DECOM\",";
                //strQry += "   TO_CHAR( \"WO_DATE\",'DD-MON-YYYY')\"WO_DATE\",\"WO_AMT\",\"WO_AMT_DECOM\",TO_CHAR( \"WO_DATE_DECOM\",'DD-MON-YYYY')\"WO_DATE_DECOM\",CASE WHEN ";
                //strQry += "   \"DF_ENHANCE_CAPACITY\"IS NULL THEN \"TC_CAPACITY\" WHEN \"DF_ENHANCE_CAPACITY\" IS NOT NULL THEN  CAST(\"DF_ENHANCE_CAPACITY\" AS INTEGER) END";
                //strQry += "   AS \"REPLACE_CAPACITY\" ,CAST(\"TC_CAPACITY\" AS TEXT)\"TC_CAPACITY\",\"DF_REASON\" ,(case when \"EST_FAIL_TYPE\" = 1 then 'SINGLECOIL' WHEN \"EST_FAIL_TYPE\" = 2 then 'MULTICOIL' END ) as \"COILTYPE\" FROM ";
                //strQry += "   \"TBLTCMASTER\" INNER JOIN \"TBLDTCFAILURE\"  ON \"DF_EQUIPMENT_ID\"=\"TC_CODE\"";
                //strQry += "   INNER JOIN \"TBLWORKORDER\" ON \"DF_ID\"=\"WO_DF_ID\" INNER JOIN \"TBLESTIMATIONDETAILS\" ON \"DF_ID\"=\"EST_FAILUREID\" ";


                //if (objReport.sFailType != null && objReport.sFailType != "")
                //{
                //    strQry += "AND \"EST_FAIL_TYPE\"=" + objReport.sFailType;

                //}

                //strQry += "  LEFT JOIN \"TBLINDENT\" ON \"WO_SLNO\"=\"TI_WO_SLNO\" LEFT JOIN \"TBLDTCINVOICE\" on \"IN_TI_NO\"=\"TI_ID\" LEFT JOIN \"TBLTCREPLACE\" ON \"TR_IN_NO\"=\"IN_NO\" INNER JOIN \"TBLDTCMAST\" ON \"DT_CODE\"=\"DF_DTC_CODE\" WHERE";

                //if (objReport.sTodate == null && (objReport.sFromDate != null))
                //{
                //    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(NOW(),'YYYY/MM/DD')";
                //    //strQry += " DF_DATE BETWEEN to_date('" + objReport.sFromDate + "','DD/MM/YYYY')";
                //}
                //if (objReport.sFromDate == null && (objReport.sTodate != null))
                //{
                //    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                //}
                //if (objReport.sFromDate == null && objReport.sTodate == null)
                //{
                //    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(NOW(),'YYYY/MM/DD') ";
                //}
                //if (objReport.sFromDate != null && objReport.sTodate != null)
                //{
                //    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                //}
                //if (objReport.sFeeder != null && objReport.sFeeder != "")
                //{
                //    strQry += "  and  SUBSTR(CAST(\"DF_DTC_CODE\" AS TEXT),1," + Constants.Feeder + ")='" + objReport.sFeeder + "'  ";
                //}

                //// strQry += " to_date('" + objReport.sFromDate + "','DD/MM/YYYY') AND to_date('" + objReport.sTodate + "','DD/MM/YYYY')";
                //strQry += "  AND CAST(\"DF_LOC_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%' AND \"DF_REPLACE_FLAG\"<>1  AND \"DF_STATUS_FLAG\"<>2  ";
                DtWoRegDetail = ObjCon.FetchDataTable(strQry);
                #endregion
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getworkorderregdetail");
                cmd.Parameters.AddWithValue("zone", Convert.ToString(Constants.Zone));
                cmd.Parameters.AddWithValue("circle", Convert.ToString(Constants.Circle));
                cmd.Parameters.AddWithValue("division", Convert.ToString(Constants.Division));
                cmd.Parameters.AddWithValue("subdivision", Convert.ToString(Constants.SubDivision));
                cmd.Parameters.AddWithValue("section", Convert.ToString(Constants.Section));
                cmd.Parameters.AddWithValue("offcode", Convert.ToString(sOfficeCode == null ? "" : sOfficeCode));
                cmd.Parameters.AddWithValue("sfailtype", Convert.ToString(objReport.sFailType == null ? "" : objReport.sFailType));
                cmd.Parameters.AddWithValue("stodate", Convert.ToString(objReport.sTodate == null ? "" : objReport.sTodate));
                cmd.Parameters.AddWithValue("sfromdate", Convert.ToString(objReport.sFromDate == null ? "" : objReport.sFromDate));
                cmd.Parameters.AddWithValue("sfeeder", Convert.ToString(objReport.sFeeder == null ? "" : objReport.sFeeder));
                DtWoRegDetail = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return DtWoRegDetail;
            }
            return DtWoRegDetail;
        }




        public DataTable DTCAddedReport(clsReports objReport)
        {
            DataTable dtDtcAddedDetails = new DataTable();
            string strQry = string.Empty;
            //DateTime fromdate = Convert.ToDateTime(objReport.sFromDate);
            //string formatted = fromdate.ToString("dd-MMM -yyyy");
            //DateTime todate = Convert.ToDateTime(objReport.sTodate);
            //string formattedto = todate.ToString("dd-MMM -yyyy");
            try
            {
                #region Query to get data Based on Feeder Capacity
                //if (objReport.sType == "1")
                //{// 
                //    // strQry = "SELECT COALESCE (COUNT,0) COUNT,COALESCE(\"FD_DTC_CAPACITY\",0)\"FD_DTC_CAPACITY\",SUBDIVOFF,\"OFF_NAME \"\"AS DIVISION\",'' AS \"SUBDIVISION\",\"FT_NAME\",(SELECT \"OFF_NAME \"FROM \"VIEW_ALL_OFFICES \"WHERE \"OFF_CODE\"=SUBSTR(SUBDIVOFF,0,1)) AS CIRCLE FROM (SELECT \"OFF_CODE \"\"AS SUBDIVOFF\",\"OFF_NAME\",\"FT_ID,\"FT_NAME\" FROM TBLFDRTYPE,\"VIEW_ALL_OFFICES \"WHERE LENGTH(\"OFF_CODE\")=2)A left JOIN (SELECT COUNT(\"DT_CODE\") COUNT,COALESCE(\"FD_DTC_CAPACITY\",0)\"FD_DTC_CAPACITY\",\"FC_FT_ID\",SUBSTR(\"DT_OM_SLNO,0, 2)\"DT_OM_SLNO\" FROM TBLFEEDERMAST INNER JOIN TBLFEEDERCATEGORY ON \"FD_FC_ID\"=\"FC_ID\" INNER JOIN TBLDTCMAST ON \"DT_FDRSLNO\"=\"FD_FEEDER_CODE \"WHERE \"DT_CRON \">=TO_DATE('01/02/2017','dd/mm/yyyy') AND \"DT_CRON\"<=TO_DATE('28/02/2017','dd/mm/yyyy')  GROUP BY \"FD_DTC_CAPACITY\",\"FC_FT_ID\",SUBSTR(\"DT_OM_SLNO\",0, 2))B ON \"FT_ID\"=\"FC_FT_ID \"AND SUBDIVOFF=\"DT_OM_SLNO\" ORDER BY SUBDIVOFF";
                //    strQry = "SELECT NVL(COUNT,0) COUNT,\"DTC_CAPACITY \"AS \"FD_DTC_CAPACITY\",SUBDIVOFF,SUBSTR(\"OFF_NAME\",INSTR(\"OFF_NAME\",':')+1) AS DIVISION,'' AS SUBDIVISION,\"FT_ID\",\"FT_NAME \"AS FT_NAME\",'" + OBJREPORT.SFROMDATE + "' AS FROMDATE,'" + OBJREPORT.STODATE + "' AS TODATE,TO_CHAR(NOW(),'DD/MM/YYYY')TODAY,";
                //    strQry += "(SELECT \"OFF_NAME \"FROM \"VIEW_ALL_OFFICES \"WHERE \"OFF_CODE\"=SUBSTR(SUBDIVOFF,0,1)) AS CIRCLE FROM (SELECT \"OFF_CODE \"AS SUBDIVOFF,\"OFF_NAME\",";
                //    strQry += "\"FT_ID\",\"FT_NAME\",\"FD_DTC_CAPACITY \"AS \"DTC_CAPACITY \"FROM (SELECT DISTINCT \"FD_DTC_CAPACITY \"FROM TBLFEEDERMAST),TBLFDRTYPE,\"VIEW_ALL_OFFICES \"";
                //    strQry += "WHERE LENGTH(\"OFF_CODE\")=2  )A LEFT JOIN  (SELECT COUNT(\"DT_CODE\") COUNT,\"DT_CRON\",COALESCE(\"FD_DTC_CAPACITY\",0)\"FD_DTC_CAPACITY\",\"FC_FT_ID\",";
                //    strQry += "SUBSTR(\"DT_OM_SLNO\",0, 2)\"DT_OM_SLNO \"FROM TBLFEEDERMAST INNER JOIN TBLFEEDERCATEGORY ON \"FD_FC_ID\"=\"FC_ID \"INNER JOIN TBLDTCMAST ON ";
                //    strQry += "\"DT_FDRSLNO\"=\"FD_FEEDER_CODE  \"GROUP BY \"FD_DTC_CAPACITY\",\"FC_FT_ID\",SUBSTR(\"DT_OM_SLNO\",0, 2),\"DT_CRON\")B ON \"FT_ID\"=\"FC_FT_ID \"AND SUBDIVOFF=\"DT_OM_SLNO \"";
                //    if (OBJREPORT.SFEEDERTYPE != NULL)
                //    {
                //        strQry += " AND FT_NAME='" + OBJREPORT.SFEEDERTYPE + "'";
                //    }
                //    if (OBJREPORT.SFROMDATE != NULL)
                //    {
                //        strQry += " AND \"DT_CRON \">=\"TO_DATE\"('" + OBJREPORT.SFROMDATE + "','DD/MM/YYYY') AND \"DT_CRON\"<=\"TO_DATE\"('" + OBJREPORT.STODATE + "','DD/MM/YYYY') ";
                //    }

                //    strQry += "AND \"DTC_CAPACITY\"=\"FD_DTC_CAPACITY \"ORDER BY SUBDIVOFF";
                //    //strQry = "SELECT (SELECT SUBSTR(\"OFF_NAME\",INSTR(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES \"WHERE \"OFF_CODE\"=SUBSTR(\"DT_OM_SLNO\",0,2))DIVISION,'' AS SUBDIVISION,";
                //    //strQry += "\"FT_NAME\",\"TO_CHAR\"(\"FD_DTC_CAPACITY\")\"FD_DTC_CAPACITY\",COUNT(\"FD_DTC_CAPACITY\")COUNT,'" + OBJREPORT.SFROMDATE + "' AS FROMDATE,'" + OBJREPORT.STODATE + "' AS TODATE FROM TBLDTCMAST,TBLFEEDERMAST,TBLFDRTYPE,TBLFEEDERCATEGORY WHERE ";
                //    //strQry += " \"DT_FDRSLNO\"=\"FD_FEEDER_CODE \"AND  \"FD_FC_ID\"=\"FC_ID \"AND \"FC_FT_ID\"=\"FT_ID\"";

                //    //strQry += " AND \"DT_OM_SLNO \"LIKE '%' GROUP BY \"FT_NAME\",SUBSTR(\"DT_OM_SLNO\",0,2),\"FD_DTC_CAPACITY\"";
                //}
                //else
                //{
                //    strQry = "SELECT NVL(COUNT,0) COUNT,\"DTC_CAPACITY \"AS \"FD_DTC_CAPACITY\",SUBDIVOFF,SUBSTR(\"OFF_NAME\",INSTR(\"OFF_NAME\",':')+1) AS SUBDIVISION,'' AS TEMP,";
                //    strQry += "(SELECT SUBSTR(\"OFF_NAME\",INSTR(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES \"WHERE \" OFF_CODE\"=SUBSTR(\"SUBDIVOFF\",0,2))DIVISION,\"FT_ID\",'" + OBJREPORT.SFROMDATE + "' AS FROMDATE,'" + OBJREPORT.STODATE + "' AS TODATE,TO_CHAR(NOW(),'DD/MM/YYYY')TODAY,";
                //    strQry += "(SELECT SUBSTR(\"OFF_NAME\",INSTR(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES \"WHERE \"OFF_CODE\"=SUBSTR(SUBDIVOFF,0,2))DIVISION,\"FT_ID,'" + OBJREPORT.SFROMDATE + "' AS FROMDATE,'" + OBJREPORT.STODATE + "' AS TODATE,\"TO_CHAR\"(NOW(),'DD/MM/YYYY')TODAY,";
                //    strQry += "\"FT_NAME \"AS \"FT_NAME\",(SELECT \"OFF_NAME \"FROM \"VIEW_ALL_OFFICES \"WHERE \"OFF_CODE\"=SUBSTR(SUBDIVOFF,0,1)) AS CIRCLE FROM (SELECT \"OFF_CODE \"AS ";
                //    strQry += "SUBDIVOFF,\"OFF_NAME\",\"FT_ID\",\"FT_NAME\",\"FD_DTC_CAPACITY \"AS \"DTC_CAPACITY \"FROM (SELECT DISTINCT \"FD_DTC_CAPACITY \"FROM TBLFEEDERMAST),";
                //    strQry += "TBLFDRTYPE,\"VIEW_ALL_OFFICES \"WHERE LENGTH(\"OFF_CODE\")=3  )A LEFT JOIN  (SELECT COUNT(\"DT_CODE\") COUNT,\"DT_CRON\",COALESCE(\"FD_DTC_CAPACITY\",0)";
                //    strQry += "\"FD_DTC_CAPACITY\",\"FC_FT_ID\",SUBSTR(\"DT_OM_SLNO\",0, 3)\"DT_OM_SLNO \"FROM TBLFEEDERMAST INNER JOIN TBLFEEDERCATEGORY ON \"FD_FC_ID\"=\"FC_ID \"";
                //    strQry += "INNER JOIN TBLDTCMAST ON \"DT_FDRSLNO\"=\"FD_FEEDER_CODE  \"GROUP BY \"FD_DTC_CAPACITY\",\"FC_FT_ID\",SUBSTR(\"DT_OM_SLNO\",0, 3),\"DT_CRON\")B ON \"FT_ID\"=\"FC_FT_ID \"";
                //    strQry += "AND SUBDIVOFF=\"DT_OM_SLNO \"";
                //    if (OBJREPORT.SFEEDERTYPE != NULL)
                //    {
                //        strQry += " AND FT_NAME='" + objReport.sFeederType + "'";
                //    }
                //    if (OBJREPORT.SFROMDATE != NULL)
                //    {
                //        STRQRY += " AND \"DT_CRON \">=TO_DATE('" + OBJREPORT.SFROMDATE + "','DD/MM/YYYY') AND \"DT_CRON\"<=TO_DATE('" + OBJREPORT.STODATE + "','DD/MM/YYYY') ";
                //    }
                //    strQry += " AND \"DTC_CAPACITY\"=\"FD_DTC_CAPACITY\" ORDER BY SUBDIVOFF ";
                //}
                #endregion

                #region query to get data based on TC Capacity with  feeder Type
                if (objReport.sType == "1")
                {
                    #region code to print capacity in form of range
                    //if (objReport.sGreaterVal == null)
                    //{
                    //    strQry = "SELECT  UPTO25 ,A2663 ,B64100,C101200,D201250 ,ABOVE250 , ";
                    //    strQry += "SUBDIVOFF,SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) AS DIVISION,'' AS ";
                    //    strQry += "SUBDIVISION,FT_ID,FT_NAME AS FT_NAME,'" + objReport.sFromDate + "' AS FROMDATE,'" + objReport.sTodate + "' AS TODATE,";
                    //    strQry += "TO_CHAR(SYSDATE,'DD/MM/YYYY')TODAY,(SELECT OFF_NAME FROM VIEW_ALL_OFFICES WHERE OFF_CODE=SUBSTR(SUBDIVOFF,0,1)) ";
                    //    strQry += "AS CIRCLE FROM(SELECT OFF_CODE AS SUBDIVOFF,TO_NUMBER(MD_NAME)MD_NAME,OFF_NAME,FT_ID,FT_NAME FROM TBLFDRTYPE,";
                    //    strQry += "VIEW_ALL_OFFICES,TBLMASTERDATA  WHERE LENGTH(OFF_CODE)=2 AND MD_TYPE='C' AND TO_NUMBER(MD_NAME)<=500 ";
                    //    strQry += "ORDER BY MD_NAME)A LEFT JOIN (SELECT SUM(CASE WHEN TC_CAPACITY BETWEEN 0 AND 25 THEN 1 ELSE 0 END)  \"UPTO25\",";
                    //    strQry += "SUM(CASE WHEN TC_CAPACITY BETWEEN 26 AND 63 THEN 1 ELSE 0 END)  A2663,";
                    //    strQry += "SUM(CASE WHEN TC_CAPACITY BETWEEN 64 AND 100 THEN 1 ELSE 0 END)  B64100,";
                    //    strQry += "SUM(CASE WHEN TC_CAPACITY BETWEEN 101 AND 200 THEN 1 ELSE 0 END)  C101200,";
                    //    strQry += "SUM(CASE WHEN TC_CAPACITY BETWEEN 201 AND 250 THEN 1 ELSE 0 END)  D201250,";
                    //    strQry += "SUM(CASE WHEN TC_CAPACITY > 250 THEN 1 ELSE 0 END)  ABOVE250, FC_FT_ID,MD_NAME,SUBSTR(DT_OM_SLNO, 0, 2)";
                    //    strQry += "DT_OM_SLNO FROM TBLDTCMAST INNER JOIN TBLFEEDERMAST ON DT_FDRSLNO = FD_FEEDER_CODE JOIN TBLFEEDERCATEGORY ";
                    //    strQry += "ON FD_FC_ID = FC_ID JOIN TBLTCMASTER ON TC_CODE = DT_TC_ID JOIN VIEW_ALL_OFFICES ON OFF_CODE = ";
                    //    strQry += "SUBSTR(DT_OM_SLNO, 0, 2) RIGHT JOIN TBLMASTERDATA ON TC_CAPACITY = MD_NAME WHERE MD_TYPE = 'C'  AND";

                    //    if (objReport.sTodate == null && (objReport.sFromDate != null))
                    //    {
                    //        strQry += " TO_CHAR(DT_CRON,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(DT_CRON,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";

                    //    }
                    //    if (objReport.sFromDate == null && (objReport.sTodate != null))
                    //    {
                    //        strQry += " TO_CHAR(DT_CRON,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    //    }
                    //    if (objReport.sFromDate == null && objReport.sTodate == null)
                    //    {
                    //        strQry += " TO_CHAR(DT_CRON,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                    //    }
                    //    if (objReport.sFromDate != null && objReport.sTodate != null)
                    //    {
                    //        strQry += " TO_CHAR(DT_CRON,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(DT_CRON,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                    //    }
                    //    strQry += " GROUP BY FC_FT_ID,MD_NAME,";
                    //    strQry += "SUBSTR(DT_OM_SLNO,0,2)ORDER BY SUBSTR(DT_OM_SLNO,0,2))B ON FT_ID=FC_FT_ID   AND SUBDIVOFF=DT_OM_SLNO AND A.MD_NAME=B.MD_NAME ";
                    //    strQry += "ORDER BY SUBDIVOFF,A.MD_NAME";
                    //}
                    #endregion

                    strQry = "SELECT  COALESCE(\"COUNT\",0)\"COUNT\", CAST(A.\"MD_NAME\" AS INTEGER) \"FD_DTC_CAPACITY\",\"SUBDIVOFF\",SUBSTR(CAST(\"OFF_NAME\" AS TEXT),STRPOS(\"OFF_NAME\",':')+1) AS \"DIVISION\",null AS \"SUBDIVISION\",\"FT_ID\",\"FT_NAME\" AS \"FT_NAME\",'" + objReport.sFromDate + "' AS \"FROMDATE\",'" + objReport.sTodate + "' AS \"TODATE\",TO_CHAR(CURRENT_DATE,'DD/MM/YYYY')\"TODAY\",";
                    strQry += "(SELECT \"OFF_NAME\" FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"SUBDIVOFF\" AS TEXT),1," + Constants.Circle + ")) AS \"CIRCLE\" FROM(SELECT CAST(\"OFF_CODE\" AS TEXT) AS \"SUBDIVOFF\",CAST(\"MD_NAME\" AS INTEGER)\"MD_NAME\",\"OFF_NAME\"";
                    strQry += ",\"FT_ID\",\"FT_NAME\" FROM \"TBLFDRTYPE\",\"VIEW_ALL_OFFICES\",\"TBLMASTERDATA\"  WHERE LENGTH(CAST(\"OFF_CODE\" AS TEXT))=" + Constants.Division + " AND";
                    if (objReport.sGreaterVal == null)
                    {
                        strQry += " \"MD_TYPE\"='C' AND CAST(\"MD_NAME\" AS INTEGER)<=500  ";
                    }
                    else
                    {
                        strQry += " \"MD_NAME\"='" + objReport.sCapacity + "' ";
                    }

                    if (objReport.sFeederType != null)
                    {
                        strQry += " AND \"FT_NAME\"='" + objReport.sFeederType + "'";
                    }
                    strQry += " ORDER BY \"MD_NAME\")A LEFT JOIN (SELECT COUNT";
                    strQry += "(\"DT_CODE\")\"COUNT\",\"FC_FT_ID\",\"MD_NAME\",SUBSTR(CAST(\"DT_OM_SLNO\" AS TEXT),1," + Constants.Division + ")\"DT_OM_SLNO\" FROM \"TBLDTCMAST\" INNER JOIN \"TBLFEEDERMAST\" ON \"DT_FDRSLNO\"=";
                    strQry += "\"FD_FEEDER_CODE\" JOIN \"TBLFEEDERCATEGORY\" ON \"FD_FC_ID\"=\"FC_ID\" JOIN \"TBLTCMASTER\" ON \"TC_CODE\"=\"DT_TC_ID\"JOIN \"VIEW_ALL_OFFICES\"ON CAST(\"OFF_CODE\" AS TEXT)=";
                    strQry += "SUBSTR(CAST(\"DT_OM_SLNO\" AS TEXT),1," + Constants.Division + ") RIGHT JOIN \"TBLMASTERDATA\" ON CAST(\"TC_CAPACITY\" AS TEXT)=\"MD_NAME\" WHERE \"MD_TYPE\"='C' AND";

                    if (objReport.sTodate == null && (objReport.sFromDate != null))
                    {
                        strQry += " TO_CHAR(\"DT_CRON\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(\"DT_CRON\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD')";

                    }
                    if (objReport.sFromDate == null && (objReport.sTodate != null))
                    {
                        strQry += " TO_CHAR(\"DT_CRON\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    }
                    if (objReport.sFromDate == null && objReport.sTodate == null)
                    {
                        strQry += " TO_CHAR(\"DT_CRON\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                    }
                    if (objReport.sFromDate != null && objReport.sTodate != null)
                    {
                        strQry += " TO_CHAR(\"DT_CRON\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"DT_CRON\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                    }
                    //if(objReport.sFromDate !=null)
                    //{
                    //    strQry += " AND DT_CRON >=TO_DATE('"+objReport.sFromDate+"','DD/MM/YYYY') AND DT_CRON<=TO_DATE('"+objReport.sTodate+"','DD/MM/YYYY') ";
                    //}
                    strQry += " GROUP BY \"FC_FT_ID\",\"MD_NAME\",";
                    strQry += "SUBSTR(CAST(\"DT_OM_SLNO\" AS TEXT),1," + Constants.Division + ")ORDER BY SUBSTR(CAST(\"DT_OM_SLNO\" AS TEXT),1," + Constants.Division + "))B ON \"FT_ID\"=\"FC_FT_ID\"AND CAST(\"SUBDIVOFF\" AS TEXT)=\"DT_OM_SLNO\"AND CAST(A.\"MD_NAME\" AS TEXT)=B.\"MD_NAME\"";
                    strQry += "ORDER BY \"SUBDIVOFF\",A.\"MD_NAME\"";
                }
                else
                {
                    strQry = "SELECT \"COUNT\",\"FD_DTC_CAPACITY\",\"SUBDIVOFF\",\"SUBDIVISION\",\"FT_ID\",\"FT_NAME\",\"FROMDATE\",\"TODATE\",\"TODAY\",\"CIRCLE\",\"DIV_CODE\",\"DIV_NAME\" AS \"DIVISION\",\"temp\" FROM(SELECT  COALESCE(\"COUNT\",0)\"COUNT\", CAST(A.\"MD_NAME\" AS INTEGER) \"FD_DTC_CAPACITY\",\"SUBDIVOFF\",SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) AS \"SUBDIVISION\",(SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\"WHERE CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"SUBDIVOFF\" AS TEXT),1," + Constants.Division + "))AS \"DIVISION\",null AS \"temp\",\"FT_ID\",\"FT_NAME\"AS \"FT_NAME\",'" + objReport.sFromDate + "' AS \"FROMDATE\",'" + objReport.sTodate + "' AS \"TODATE\",TO_CHAR(CURRENT_DATE,'DD/MM/YYYY')\"TODAY\",";
                    strQry += "(SELECT \"OFF_NAME\" FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"SUBDIVOFF\" AS TEXT),1," + Constants.Circle + ")) AS \"CIRCLE\" FROM(SELECT CAST(\"OFF_CODE\"AS TEXT) AS \"SUBDIVOFF\",CAST(\"MD_NAME\" AS INTEGER)\"MD_NAME\",\"OFF_NAME\"";
                    strQry += ",\"FT_ID\",\"FT_NAME\" FROM \"TBLFDRTYPE\",\"VIEW_ALL_OFFICES\",\"TBLMASTERDATA\"  WHERE LENGTH(CAST(\"OFF_CODE\" AS TEXT))=" + Constants.SubDivision + " AND ";
                    if (objReport.sGreaterVal == null)
                    {
                        strQry += " \"MD_TYPE\"='C' AND CAST(\"MD_NAME\" AS INTEGER)<=500  ";
                    }
                    else
                    {
                        strQry += " \"MD_NAME\"='" + objReport.sCapacity + "' ";
                    }
                    if (objReport.sFeederType != null)
                    {
                        strQry += " AND \"FT_NAME\"='" + objReport.sFeederType + "'";
                    }

                    strQry += " ORDER BY \"MD_NAME\")A LEFT JOIN (SELECT COUNT";
                    strQry += "(\"DT_CODE\")\"COUNT\",\"FC_FT_ID\",\"MD_NAME\",SUBSTR(CAST(\"DT_OM_SLNO\" AS TEXT),1," + Constants.SubDivision + ")\"DT_OM_SLNO\" FROM \"TBLDTCMAST\" INNER JOIN \"TBLFEEDERMAST\" ON \"DT_FDRSLNO\"=";
                    strQry += "\"FD_FEEDER_CODE\" JOIN \"TBLFEEDERCATEGORY\" ON \"FD_FC_ID\"=\"FC_ID\" JOIN \"TBLTCMASTER\" ON \"TC_CODE\"=\"DT_TC_ID\" JOIN \"VIEW_ALL_OFFICES\"ON CAST(\"OFF_CODE\" AS TEXT)=";
                    strQry += "SUBSTR(CAST(\"DT_OM_SLNO\" AS TEXT),1," + Constants.SubDivision + ") RIGHT JOIN \"TBLMASTERDATA\" ON CAST(\"TC_CAPACITY\" AS TEXT)=\"MD_NAME\" WHERE \"MD_TYPE\"='C' AND";
                    if (objReport.sTodate == null && (objReport.sFromDate != null))
                    {
                        strQry += " TO_CHAR(\"DT_CRON\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(\"DT_CRON\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD')";

                    }
                    if (objReport.sFromDate == null && (objReport.sTodate != null))
                    {
                        strQry += " TO_CHAR(\"DT_CRON\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    }
                    if (objReport.sFromDate == null && objReport.sTodate == null)
                    {
                        strQry += " TO_CHAR(\"DT_CRON\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                    }
                    if (objReport.sFromDate != null && objReport.sTodate != null)
                    {
                        strQry += " TO_CHAR(\"DT_CRON\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"DT_CRON\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                    }
                    //if (objReport.sFromDate != null)
                    //{
                    //    strQry += " AND DT_CRON >=TO_DATE('" + objReport.sFromDate + "','DD/MM/YYYY') AND DT_CRON<=TO_DATE('" + objReport.sTodate + "','DD/MM/YYYY') ";
                    //}

                    strQry += " GROUP BY \"FC_FT_ID\",\"MD_NAME\",";
                    strQry += "SUBSTR(CAST(\"DT_OM_SLNO\" AS TEXT),1," + Constants.SubDivision + ")ORDER BY SUBSTR(CAST(\"DT_OM_SLNO\" AS TEXT),1," + Constants.SubDivision + "))B ON \"FT_ID\"=\"FC_FT_ID\" AND  CAST(\"SUBDIVOFF\" AS TEXT)=\"DT_OM_SLNO\" AND CAST(A.\"MD_NAME\" AS TEXT)=B.\"MD_NAME\"";
                    strQry += ")A RIGHT JOIN (SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\")B ON CAST(\"DIV_CODE\" AS TEXT)=SUBSTR(CAST(\"SUBDIVOFF\" AS TEXT),1," + Constants.Division + ")";
                    strQry += " ORDER BY \"SUBDIVOFF\",\"FD_DTC_CAPACITY\" ";
                }
                #endregion
                dtDtcAddedDetails = ObjCon.FetchDataTable(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtDtcAddedDetails;
            }
            return dtDtcAddedDetails;
        }
        /// <summary>
        /// PMC Stock Details
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable PMCStockDetails(clsReports objReport)
        {
            DataTable DtPMCStockDetails = new DataTable();
            string formattedfromdate = null;
            string formattedtodate = null;
            try
            {
                if (objReport.sFromDate != null)
                {
                    DateTime fromdate = Convert.ToDateTime(objReport.sFromDate);
                    formattedfromdate = fromdate.ToString("dd-MMM-yyyy");
                }
                if (objReport.sTodate != null)
                {
                    DateTime todate = Convert.ToDateTime(objReport.sTodate);
                    formattedtodate = todate.ToString("dd-MMM-yyyy");
                }

                NpgsqlCommand cmd = new NpgsqlCommand("public.sp_get_pmc_get_storestock_details");
                cmd.Parameters.AddWithValue("store_id", (objReport.StoreId ?? ""));
                cmd.Parameters.AddWithValue("off_code", (objReport.sOfficeCode ?? ""));
                cmd.Parameters.AddWithValue("from_date", (objReport.sFromDate ?? ""));
                cmd.Parameters.AddWithValue("to_date", (objReport.sTodate ?? ""));
                DtPMCStockDetails = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return DtPMCStockDetails;
            }
            return DtPMCStockDetails;
        }
        public DataTable FrequentTcFail(clsReports objReport)
        {
            DataTable dtFrequentTcFailureDetails = new DataTable();
            string strQry = string.Empty;
            string formattedfromdate = null;
            string formattedtodate = null;
            try
            {



                if (objReport.sFromDate != null)
                {
                    DateTime fromdate = Convert.ToDateTime(objReport.sFromDate);
                    formattedfromdate = fromdate.ToString("dd-MM-yyyy");
                }
                if (objReport.sTodate != null)
                {
                    DateTime todate = Convert.ToDateTime(objReport.sTodate);
                    formattedtodate = todate.ToString("dd-MM-yyyy");
                }
                #region Commented inline Query
                //strQry = " SELECT DISTINCT CAST(\"TC_CODE\" AS TEXT)\"TC_CODE\", ";
                //strQry += " (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\")\"TC_MAKE_ID\",\"TC_SLNO\",";
                //strQry += " CAST(\"TC_CAPACITY\" AS TEXT)\"TC_CAPACITY\",\"DF_DTC_CODE\",\"DT_CODE\",";
                //strQry += " (SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=\"DF_DTC_CODE\")\"DT_NAME\",\"DF_GUARANTY_TYPE\", ";
                //strQry += " TO_CHAR(\"DF_DATE\",'dd-MON-yy')\"DF_DATE\",\"DF_LOC_CODE\",";
                //strQry += " (SELECT DISTINCT \"FD_FEEDER_CODE\" FROM \"TBLFEEDERMAST\" WHERE \"FD_FEEDER_CODE\"=\"DT_FDRSLNO\") \"FD_FEEDER_CODE\", ";
                //strQry += " (SELECT DISTINCT \"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\" WHERE \"FD_FEEDER_CODE\"=\"DT_FDRSLNO\")\"FD_FEEDER_NAME\", ";
                //strQry += " \"MD_NAME\",\"DF_CONSUMER_COMPLAINT_NUM\" AS \"ComplainNumber\",";
                //strQry += " TO_CHAR(\"DF_CONSUMER_COMPLAINT_DATE\",'dd-MON-yy') as \"ComplainDate\",";
                //strQry += " '" + formattedfromdate + "' AS \"FROMDATE\",'" + formattedtodate + "' AS \"TODATE\", ";
                //strQry += " TO_CHAR(NOW(),'DD-MON-YYYY') AS \"TODAY\", ";
                //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Zone + ")) \"ZONE\", ";
                //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Circle + ")) \"CIRCLE\", ";
                //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Division + ")) \"DIVISION\", ";
                //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ")) \"SUBDIVISION\", ";
                //strQry += " (SELECT SUBSTR(\"OFF_NAME\",STRPOS(\"OFF_NAME\",':')+1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Section + ")) \"SECTION\" ";
                ////strQry += " ,(case when \"EST_FAIL_TYPE\" = 1 then 'SINGLECOIL' WHEN \"EST_FAIL_TYPE\" = 2 then 'MULTICOIL' END ) as \"COILTYPE\" ";
                //strQry += " FROM \"TBLTCMASTER\",\"TBLDTCFAILURE\",\"TBLMASTERDATA\",\"TBLDTCMAST\",\"TBLTRANSMAKES\" ";
                //strQry += " WHERE  \"DF_EQUIPMENT_ID\"=\"TC_CODE\"AND \"DF_DTC_CODE\"=\"DT_CODE\"";
                //strQry += " AND CAST(\"DF_LOC_CODE\" AS TEXT) LIKE '" + objReport.sOfficeCode + "%' AND \"DF_FAILURE_TYPE\"=\"MD_ID\" ";
                //strQry += " AND \"MD_TYPE\"='FT' AND \"DF_STATUS_FLAG\" IN (1,4) ";

                //if (objReport.sFeeder != null)
                //{
                //    strQry += " AND  \"DT_FDRSLNO\" ='" + objReport.sFeeder + "' ";
                //}
                ////if (objReport.sFailType != null && objReport.sFailType != "") // Removed as mentioned in Jira Id : DTLMS-1598
                ////{
                ////    strQry += " AND \"EST_FAIL_TYPE\"=" + objReport.sFailType;
                ////}
                //if (objReport.sGuranteeType != null)
                //{
                //    strQry += " AND \"DF_GUARANTY_TYPE\"='" + objReport.sGuranteeType + "' ";
                //}
                //if (objReport.sFailureType != null)
                //{
                //    strQry += " AND \"DF_FAILURE_TYPE\"='" + objReport.sFailureType + "' ";
                //}
                //if (objReport.sDtrCode != null)
                //{
                //    strQry += " AND \"DF_EQUIPMENT_ID\"='" + objReport.sDtrCode + "' ";
                //}
                //strQry += " AND \"DF_DTC_CODE\" IN (SELECT \"DF_DTC_CODE\" FROM(SELECT \"DF_DTC_CODE\",COUNT(\"DF_DTC_CODE\") ";
                //strQry += " FROM \"TBLDTCFAILURE\" WHERE \"DF_STATUS_FLAG\" IN (1,4) AND CAST(\"DF_LOC_CODE\" AS TEXT) LIKE '" + objReport.sOfficeCode + "%' AND  ";

                //if (objReport.sTodate == null && (objReport.sFromDate != null))
                //{
                //   strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                //    strQry += " AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(NOW(),'YYYY/MM/DD') ";

                //}
                //if (objReport.sFromDate == null && (objReport.sTodate != null))
                //{
                //    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                //}
                //if (objReport.sFromDate == null && objReport.sTodate == null)
                //{
                //    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(NOW(),'YYYY/MM/DD')  ";
                //}
                //if (objReport.sFromDate != null && objReport.sTodate != null)
                //{
                //    strQry += " TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                //    strQry += " AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                //}
                ////if (objReport.sGuranteeType != null)
                ////{
                ////    strQry += " AND \"DF_GUARANTY_TYPE\"='" + objReport.sGuranteeType + "'  ";
                ////}

                //if (objReport.sDtcCode != null)
                //{
                //    strQry += " AND \"DF_DTC_CODE\"='" + objReport.sDtcCode + "'";
                //}
                //if (objReport.sDtrCode != null)
                //{
                //    strQry += " AND \"DF_DTC_CODE\"=(SELECT \"DF_DTC_CODE\" FROM \"TBLDTCFAILURE\" ";
                //    strQry += " WHERE \"DF_EQUIPMENT_ID\"='" + objReport.sDtrCode + "')";
                //}

                //strQry += " GROUP BY \"DF_DTC_CODE\" HAVING COUNT(\"DF_DTC_CODE\")>=2 )A) ORDER BY \"DF_DTC_CODE\"";
                //dtFrequentTcFailureDetails = ObjCon.FetchDataTable(strQry);
                #endregion


                NpgsqlCommand cmd = new NpgsqlCommand("sp_getfrequenttcfail");
                cmd.Parameters.AddWithValue("formattedfromdate", Convert.ToString(formattedfromdate == null ? "" : formattedfromdate));
                cmd.Parameters.AddWithValue("formattedtodate", Convert.ToString(formattedtodate == null ? "" : formattedtodate));
                cmd.Parameters.AddWithValue("zone", Convert.ToString(Constants.Zone));
                cmd.Parameters.AddWithValue("circle", Convert.ToString(Constants.Circle));
                cmd.Parameters.AddWithValue("division", Convert.ToString(Constants.Division));
                cmd.Parameters.AddWithValue("subdivision", Convert.ToString(Constants.SubDivision));
                cmd.Parameters.AddWithValue("section", Convert.ToString(Constants.Section));
                cmd.Parameters.AddWithValue("offcode", Convert.ToString(sOfficeCode == null ? "" : sOfficeCode));
                cmd.Parameters.AddWithValue("sfeeder", Convert.ToString(objReport.sFeeder == null ? "" : objReport.sFeeder));
                cmd.Parameters.AddWithValue("sguranteetype", objReport.sGuranteeType == null ? "" : objReport.sGuranteeType);
                cmd.Parameters.AddWithValue("sfailuretype", objReport.sFailureType == null ? "" : objReport.sFailureType);
                cmd.Parameters.AddWithValue("dtccode", Convert.ToString(objReport.sDtcCode == null ? "" : objReport.sDtcCode));
                cmd.Parameters.AddWithValue("dtrcode", Convert.ToString(objReport.sDtrCode == null ? "" : objReport.sDtrCode));

                dtFrequentTcFailureDetails = ObjCon.FetchDataTable(cmd);



            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtFrequentTcFailureDetails;
            }
            return dtFrequentTcFailureDetails;
        }

        public string GetPurpose(string Purposeid)
        {
            string purpose = string.Empty;
            try
            {
                purpose = ObjCon.get_value("SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'POU' and \"MD_ID\"='" + Purposeid + "' ORDER BY \"MD_ORDER_BY\"");
                return purpose;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return purpose;
            }
        }

        public DataTable GetDetiailedReport(string sOffCode, string sReportType, string sRecordType, string sFeedercode = "")
        {
            DataTable dt = new DataTable();
            try
            {
                string sQry = string.Empty;

                if (sReportType == "2" && sRecordType == "1")
                {
                    //sQry = "SELECT (SELECT A.\"DT_NAME\" FROM \"TBLDIST\" A WHERE CAST(A.\"DT_CODE\" AS TEXT) = SUBSTR(CAST(\"DT_FDRSLNO\" AS TEXT),1,1)) \"DISTRICT\", ";
                    //sQry += " (SELECT \"ZO_NAME\" FROM \"TBLZONE\" WHERE CAST(\"ZO_ID\" AS TEXT) = SUBSTR(CAST(\"DT_OM_SLNO\" AS TEXT),1,1))  \"ZONE_NAME\",(SELECT \"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" ";
                    //sQry += " WHERE CAST(\"CM_CIRCLE_CODE\" AS TEXT) = SUBSTR(CAST(\"DT_OM_SLNO\" AS TEXT),1,2))  \"CIRCLE_NAME\",(SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT) = ";
                    //sQry += " SUBSTR(CAST(\"DT_OM_SLNO\" AS TEXT),1,3))  \"DIV_NAME\", ";
                    //sQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT) = SUBSTR(CAST(\"DT_OM_SLNO\" AS TEXT),1,4)) \"SD_NAME\", ";
                    //sQry += " \"OM_NAME\", \"DT_FDRSLNO\", \"DT_CODE\", \"DT_NAME\", \"DT_INTERNAL_CODE\", \"DT_TIMS_CODE\", TO_CHAR(\"DT_TRANS_COMMISION_DATE\",'DD-MON-YYYY') \"DT_TRANS_COMMISION_DATE\",\"DT_TOTAL_CON_KW\", ";
                    //sQry += " \"DT_TOTAL_CON_HP\", (SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_ID\" = \"DT_BREAKER_TYPE\" AND \"MD_TYPE\" = 'BT')  \"DT_BREAKER_TYPE\", ";
                    //sQry += " (CASE \"DT_ARRESTERS\" WHEN '1' THEN 'YES' ELSE 'NO' END)  \"DT_ARRESTERS\", (CASE \"DT_DTCMETERS\"  WHEN '1' THEN 'YES' ELSE 'NO' END) \"DT_DTCMETERS\", ";
                    //sQry += " (CASE \"DT_LT_PROTECT\" WHEN '1' THEN 'YES' ELSE 'NO' END) \"DT_LT_PROTECT\", (CASE \"DT_HT_PROTECT\" WHEN '1' THEN 'YES' ELSE 'NO' END) \"DT_HT_PROTECT\", ";
                    //sQry += " (CASE \"DT_GROUNDING\" WHEN '1' THEN 'YES' ELSE 'NO' END) \"DT_GROUNDING\",\"DT_HT_LINE\", \"DT_LT_LINE\", ";
                    //sQry += " (SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_ID\" = \"DT_PLATFORM\" AND \"MD_TYPE\" = 'PLT')\"DT_PLATFORM\",  \"DT_LATITUDE\", ";
                    //sQry += " \"DT_LONGITUDE\",(CASE \"DT_GOS\" WHEN '1' THEN 'YES' ELSE 'NO' END) \"DT_GOS\" FROM \"TBLDTCMAST\" , \"TBLOMSECMAST\" ";
                    //sQry += " WHERE \"OM_CODE\" = \"DT_OM_SLNO\" AND  CAST(\"DT_OM_SLNO\" AS TEXT) LIKE '" + sOffCode + "%'";

                    //if (sFeedercode != "")
                    //{
                    //    sQry += " AND  CAST(\"DT_FDRSLNO\" AS TEXT) Like '" + sFeedercode + "%' ";
                    //}

                    //sQry += "ORDER BY \"ZONE_NAME\",\"CIRCLE_NAME\",\"DIV_NAME\",\"SD_NAME\",\"OM_NAME\", \"DT_CODE\"";
                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_dtrreport_store_repairer_bank_clsreport");
                    cmd.Parameters.AddWithValue("p_offcode", Convert.ToString(sOffCode ?? ""));
                    cmd.Parameters.AddWithValue("p_feedercode", Convert.ToString(sFeedercode ?? ""));

                    dt = ObjCon.FetchDataTable(cmd);

                    return dt;
                }
                else if (sReportType == "2" && sRecordType == "2")
                {
                    //sQry = " SELECT (SELECT \"ZO_NAME\" FROM \"TBLZONE\" WHERE CAST(\"ZO_ID\" AS TEXT) = SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1,1))  \"ZONE_NAME\",(SELECT \"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" ";
                    //sQry += " WHERE CAST(\"CM_CIRCLE_CODE\" AS TEXT) = SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1,2))  \"CIRCLE_NAME\",(SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT) = ";
                    //sQry += " SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1,3))  \"DIV_NAME\", (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT) = SUBSTR(CAST(\"TC_LOCATION_ID\" AS TEXT),1,4)) ";
                    //sQry += " \"SD_NAME\", \"OM_NAME\",  \"TC_CODE\", \"TC_SLNO\", \"TM_NAME\",( SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'SR' AND \"MD_ID\" = \"TC_RATING\") \"TC_RATING\",";
                    //sQry += " TO_CHAR(\"TC_MANF_DATE\",'DD-MON-YYYY') \"TC_MANF_DATE\", \"TC_CAPACITY\", \"TC_WEIGHT\" FROM \"TBLTCMASTER\",";
                    //sQry += " \"TBLOMSECMAST\", \"TBLTRANSMAKES\" WHERE \"TM_ID\" = \"TC_MAKE_ID\" AND  \"OM_CODE\" = \"TC_LOCATION_ID\" AND  ";
                    //sQry += " CAST(\"TC_LOCATION_ID\" AS TEXT) LIKE '" + sOffCode + "%' AND \"TC_CURRENT_LOCATION\" = 2  AND \"TC_CODE\" <> '0' and \"TC_CAPACITY\"<>0  ORDER BY \"TC_LOCATION_ID\",\"TC_CODE\"";
                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_dtrreport_field_clsreport");
                    cmd.Parameters.AddWithValue("p_offcode", Convert.ToString(sOffCode ?? ""));

                    dt = ObjCon.FetchDataTable(cmd);

                    return dt;
                }
                else if (sReportType == "1" && sRecordType == "2")
                {
                    //sQry = " SELECT \"SM_NAME\", \"TC_CODE\", \"TC_SLNO\", \"TM_NAME\",( SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'SR' AND \"MD_ID\" = \"TC_RATING\") \"TC_RATING\",";
                    //sQry += " TO_CHAR(\"TC_MANF_DATE\",'DD-MON-YYYY')  \"TC_MANF_DATE\", \"TC_CAPACITY\", \"TC_WEIGHT\" ";
                    //sQry += ",case when \"TC_CURRENT_LOCATION\"='1' then 'STORE' when \"TC_CURRENT_LOCATION\"='3' then 'REPAIRER' WHEN \"TC_CURRENT_LOCATION\"='5' THEN 'TRANSFORMER BANK' END \"CURRENT LOCATION\" ";
                    //sQry += "  FROM \"TBLTCMASTER\", \"TBLSTOREMAST\", \"TBLTRANSMAKES\" WHERE \"TM_ID\" = \"TC_MAKE_ID\" AND  \"SM_ID\" = \"TC_LOCATION_ID\" AND  ";

                    ////  sQry += " CAST(\"TC_LOCATION_ID\" AS TEXT) LIKE '" + sOffCode + "%' AND \"TC_CURRENT_LOCATION\" IN (1,3,5) AND \"TC_CODE\" <> '0' and \"TC_CAPACITY\"<>0 ORDER BY \"TC_LOCATION_ID\",\"TC_CODE\"";
                    //if ((sOffCode ?? "").Length > 0)
                    //{

                    //    sQry += " CAST(\"TC_LOCATION_ID\" AS TEXT) LIKE '" + sOffCode + "' ";
                    //}
                    //else
                    //{
                    //    sQry += " CAST(\"TC_LOCATION_ID\" AS TEXT) LIKE '" + sOffCode + "%' ";
                    //}
                    //sQry += " AND \"TC_CURRENT_LOCATION\" IN (1,3,5) AND \"TC_CODE\" <> '0' and \"TC_CAPACITY\"<>0 ORDER BY \"TC_LOCATION_ID\",\"TC_CODE\"";
                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_dtcreport_store_repairer_bank_clsreport");
                    cmd.Parameters.AddWithValue("p_offcode", Convert.ToString(sOffCode ?? ""));

                    dt = ObjCon.FetchDataTable(cmd);

                    return dt;
                }

                dt = ObjCon.FetchDataTable(sQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable GetpermanentEstimationDetails(string EstId)
        {
            DataTable dtest = new DataTable();
            string sQry = string.Empty;
            try
            {
                sQry = " SELECT DISTINCT \"PEST_ID\", CAST(\"PEST_NO\" AS TEXT),  \"TM_NAME\", \"OM_NAME\", \"PEST_CRON\", CAST( \"PEST_CAPACITY\" AS TEXT), \"TR_NAME\", ";
                sQry += " \"MRIM_REMARKS\", \"MRIM_ITEM_NAME\",\"MRIM_ITEM_ID\",\"MD_NAME\", \"PESTM_ITEM_QNTY\", \"PESTM_ITEM_RATE\", ";
                sQry += " \"PESTM_ITEM_TOTAL\" - \"PESTM_ITEM_TAX\" \"AMOUNT\", \"PESTM_ITEM_TAX\" , \"PESTM_ITEM_TOTAL\"  FROM \"TBLPERMANENTESTIMATIONDETAILS\" ";
                sQry += " JOIN \"TBLPERMANENTESTIMATIONMATERIAL\" ON \"PEST_ID\" = \"PESTM_EST_ID\" JOIN \"TBLTRANSREPAIRER\" ON\"PEST_REPAIRER\" = \"TR_ID\" ";
                sQry += "  JOIN \"TBLTCMASTER\" ON \"PEST_TC_CODE\" = \"TC_CODE\" JOIN ";
                sQry += " \"TBLTRANSMAKES\" ON \"TC_MAKE_ID\" =  \"TM_ID\" JOIN \"TBLOMSECMAST\" ON \"PEST_LOC_CODE\" = \"OM_CODE\" JOIN ";
                sQry += " (SELECT \"MRIM_ID\", \"MRIM_ITEM_NAME\",\"MRIM_ITEM_ID\", \"MD_NAME\",  \"MRI_TR_ID\", \"MRIM_REMARKS\" , \"MRI_CAPACITY\" FROM \"TBLMINORREPAIRERITEMMASTER\", ";
                sQry += " \"TBLMINORREPAIRITEMRATEMASTER\", \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='MSR' AND \"MRIM_ID\" = \"MRI_MRIM_ID\" AND ";
                sQry += " CAST(\"MRI_MEASUREMENT\" AS TEXT) = CAST(\"MD_ID\" AS TEXT))A ON \"PESTM_ITEM_ID\" = \"MRIM_ID\" WHERE \"PEST_ID\" = '" + EstId + "' ";
                sQry += "AND CAST(\"PEST_CAPACITY\" AS TEXT) = (SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'C' AND \"MD_ID\" = A.\"MRI_CAPACITY\")";
                dtest = ObjCon.FetchDataTable(sQry);
                return dtest;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtest;
            }
        }



        public DataTable GetPermanentEstimationAmount(string EstId)
        {
            string StrQry = string.Empty;
            DataTable dtest = new DataTable();
            try
            {
                StrQry = "SELECT \"PEST_TC_CODE\" AS \"DTR_CODE\",SUM(CASE \"MRIM_ITEM_TYPE\" WHEN  '1'  THEN \"PESTM_ITEM_TOTAL\" - \"PESTM_ITEM_TAX\" ELSE 0 END + ";
                StrQry += " CASE \"MRIM_ITEM_TYPE\" WHEN '2' THEN \"PESTM_ITEM_TOTAL\" - \"PESTM_ITEM_TAX\" ELSE 0 END - ";
                StrQry += " CASE \"MRIM_ITEM_TYPE\" WHEN '3' THEN \"PESTM_ITEM_TOTAL\" - \"PESTM_ITEM_TAX\" ELSE 0 END) \"AMOUNT\",";
                StrQry += " SUM(  CASE \"MRIM_ITEM_TYPE\" WHEN  '1'  THEN  \"PESTM_ITEM_TAX\" ELSE 0 END + ";
                StrQry += " CASE \"MRIM_ITEM_TYPE\" WHEN '2' THEN  \"PESTM_ITEM_TAX\" ELSE 0 END - ";
                StrQry += " CASE \"MRIM_ITEM_TYPE\" WHEN '3' THEN  \"PESTM_ITEM_TAX\" ELSE 0 END)  \"TAX\", ";
                StrQry += " SUM( CASE \"MRIM_ITEM_TYPE\" WHEN  '1'  THEN  \"PESTM_ITEM_TOTAL\" ELSE 0 END + ";
                StrQry += " CASE \"MRIM_ITEM_TYPE\" WHEN '2' THEN  \"PESTM_ITEM_TOTAL\" ELSE 0 END - ";
                StrQry += " CASE \"MRIM_ITEM_TYPE\" WHEN '3' THEN  \"PESTM_ITEM_TOTAL\" ELSE 0 END)  \"TOTALAMOUNT\" ";
                StrQry += " FROM (SELECT \"PEST_ID\", \"PEST_NO\", \"TM_NAME\", \"OM_NAME\", \"PEST_CRON\",  \"PEST_CAPACITY\", ";
                StrQry += " \"TR_NAME\", \"MRIM_ITEM_TYPE\", \"MRIM_REMARKS\", \"MRIM_ITEM_NAME\",\"MRIM_ITEM_ID\",\"MD_NAME\", \"PESTM_ITEM_QNTY\", \"PESTM_ITEM_RATE\",  ";
                StrQry += " \"PESTM_ITEM_TOTAL\" - \"PESTM_ITEM_TAX\" \"AMOUNT\", \"PESTM_ITEM_TAX\" , \"PESTM_ITEM_TOTAL\",\"PEST_TC_CODE\"  FROM \"TBLPERMANENTESTIMATIONDETAILS\"  ";
                StrQry += " JOIN \"TBLPERMANENTESTIMATIONMATERIAL\" ON \"PEST_ID\" = \"PESTM_EST_ID\" JOIN \"TBLTRANSREPAIRER\" ON\"PEST_REPAIRER\" = \"TR_ID\"  ";
                StrQry += "  JOIN \"TBLTCMASTER\" ON \"PEST_TC_CODE\" = \"TC_CODE\" JOIN ";
                StrQry += " \"TBLTRANSMAKES\" ON \"TC_MAKE_ID\" =  \"TM_ID\" JOIN \"TBLOMSECMAST\" ON \"PEST_LOC_CODE\" = \"OM_CODE\" JOIN ";
                StrQry += " (SELECT DISTINCT \"MRIM_ID\", \"MRIM_ITEM_TYPE\",\"MRIM_ITEM_NAME\",\"MRIM_ITEM_ID\", \"MD_NAME\",  \"MRI_TR_ID\", \"MRIM_REMARKS\",\"MRI_CAPACITY\" FROM  ";
                StrQry += " \"TBLMINORREPAIRERITEMMASTER\",  \"TBLMINORREPAIRITEMRATEMASTER\", \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='MSR' AND ";
                StrQry += " \"MRIM_ID\" = \"MRI_MRIM_ID\" AND  CAST(\"MRI_MEASUREMENT\" AS TEXT) = CAST(\"MD_ID\" AS TEXT))A ON \"PESTM_ITEM_ID\" = ";
                StrQry += " \"MRIM_ID\"    WHERE \"PEST_ID\" = '" + EstId + "' AND CAST(\"PEST_CAPACITY\" AS TEXT) = (SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" ";
                StrQry += " WHERE \"MD_TYPE\" = 'C' AND \"MD_ID\" = A.\"MRI_CAPACITY\"))Z  GROUP BY \"PEST_TC_CODE\"";
                dtest = ObjCon.FetchDataTable(StrQry);
                return dtest;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtest;
            }
        }


        public DataTable PrintWorkOrderDetailsForNewpermanentdtc(string sWoId)
        {
            DataTable dtWODetails = new DataTable();
            try
            {
                string strQry = string.Empty;


                strQry = "SELECT \"PWO_SLNO\",\"PWO_NO_DECOM\",TO_CHAR(\"PWO_DATE_DECOM\",'dd/MM/yyyy') \"PWO_DATE_DECOM\",CAST(\"PWO_AMT_DECOM\" AS INT)\"PWO_AMT_DECOM\",\"PWO_ACCCODE_DECOM\",\"PWO_ISSUED_BY\",\"PWO_NEW_CAP\",";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"PWO_CRBY\"=\"US_ID\" AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A') \"US_FULL_NAME\",\"PWO_REQUEST_LOC\" FROM \"TBLPERMANENTWORKORDER\" WHERE ";
                strQry += " \"PWO_SLNO\"='" + sWoId + "' ";
                dtWODetails = ObjCon.FetchDataTable(strQry);


                return dtWODetails;
            }

            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtWODetails;

            }

        }

        public DataTable IndentDetailspermanent(string strIndentId)
        {
            DataTable dtIndentDetails = new DataTable();
            string strQry = string.Empty;
            try
            {

                strQry = "SELECT \"PTI_INDENT_NO\",TO_CHAR(\"PTI_INDENT_DATE\",'dd/MM/yyyy') \"PTI_INDENT_DATE\",CAST(\"TC_CAPACITY\" AS TEXT) \"TC_CAPACITY\",\"PWO_NEW_CAP\",";
                strQry += " TO_CHAR(\"PWO_DATE_DECOM\",'dd/MM/yyyy') \"PWO_DATE_DECOM\", (select \"SM_NAME\" from \"TBLSTOREMAST\" WHERE \"SM_ID\"=\"PTI_STORE_ID\") \"SM_NAME\",";
                strQry += "  \"PWO_NO_DECOM\",\"PWO_ACCCODE_DECOM\",\"PWO_AMT_DECOM\",";
                strQry += "  CAST(\"PEST_DTC_CODE\" AS TEXT), (SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=CAST(\"PEST_DTC_CODE\" AS TEXT)) \"DT_NAME\",";
                strQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT)=SUBSTR(CAST(\"PEST_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "')) AS \"SUBDIVISION\",";
                strQry += "(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE CAST(\"OM_CODE\" AS TEXT)=SUBSTR(CAST(\"PEST_LOC_CODE\" AS TEXT),1,'" + Constants.Section + "')) AS \"SECTION\",";
                strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\"  AS TEXT)=SUBSTR(CAST(\"PEST_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "')) AS \"DIVISION\",";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=SUBSTR(CAST(\"PEST_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "') AND CAST(\"US_ROLE_ID\" AS TEXT)='1' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"STO_USERNAME\" ";
                strQry += " ,(SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=CAST(\"PEST_LOC_CODE\" AS TEXT) AND \"US_ROLE_ID\"='4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SO_USERNAME\" FROM \"TBLTCMASTER\",";
                strQry += " \"TBLPERMANENTWORKORDER\",\"TBLPERMANENTINDENT\",\"TBLPERMANENTESTIMATIONDETAILS\" where  \"PTI_WO_SLNO\"=\"PWO_SLNO\" and CAST(\"PTI_ID\" AS TEXT)='" + strIndentId + "' AND \"PEST_ID\"=\"PWO_PEF_ID\" AND \"TC_CODE\"=\"PEST_TC_CODE\" ";
                dtIndentDetails = ObjCon.FetchDataTable(strQry);
                return dtIndentDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtIndentDetails;

            }

        }



        public DataTable WO_IndentDetailspermanent(string sWOSlno, string sstoreId)
        {
            DataTable dtIndentDetails = new DataTable();
            string strQry = string.Empty;
            try
            {

                strQry = "SELECT null as \"PTI_INDENT_NO\",TO_CHAR(CURRENT_DATE,'dd/MM/yyyy') \"PTI_INDENT_DATE\",CAST(\"TC_CAPACITY\" AS TEXT) \"TC_CAPACITY\",\"PWO_NEW_CAP\",";
                strQry += " TO_CHAR(\"PWO_DATE_DECOM\",'dd/MM/yyyy') \"PWO_DATE_DECOM\", (select \"SM_NAME\" from \"TBLSTOREMAST\" WHERE \"SM_ID\"=" + sstoreId + ") \"SM_NAME\",";
                strQry += "  \"PWO_NO_DECOM\",\"PWO_ACCCODE_DECOM\",\"PWO_AMT_DECOM\",";
                strQry += "  \"PEST_DTC_CODE\", (SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=CAST(\"PEST_DTC_CODE\" AS TEXT)) \"DT_NAME\",";
                strQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT)=SUBSTR(CAST(\"PEST_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "')) AS \"SUBDIVISION\",";
                strQry += "(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE CAST(\"OM_CODE\" AS TEXT)=SUBSTR(CAST(\"PEST_LOC_CODE\" AS TEXT),1,'" + Constants.Section + "')) AS \"SECTION\",";
                strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\"  AS TEXT)=SUBSTR(CAST(\"PEST_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "')) AS \"DIVISION\",";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=SUBSTR(CAST(\"PEST_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "') AND CAST(\"US_ROLE_ID\" AS TEXT)='1' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"STO_USERNAME\" ";
                strQry += " ,(SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=CAST(\"PEST_LOC_CODE\" AS TEXT) AND \"US_ROLE_ID\"='4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SO_USERNAME\" FROM \"TBLTCMASTER\",";
                strQry += " \"TBLPERMANENTWORKORDER\",\"TBLPERMANENTESTIMATIONDETAILS\" where ";
                strQry += "   CAST(\"PWO_SLNO\" AS TEXT)='" + sWOSlno + "'";
                strQry += " AND \"PEST_ID\"=\"PWO_PEF_ID\" AND \"TC_CODE\"=\"PEST_TC_CODE\" ";
                dtIndentDetails = ObjCon.FetchDataTable(strQry);
                return dtIndentDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtIndentDetails;

            }

        }



        public DataTable RIReportpermanent(string sDecommId)
        {
            DataTable dtRiDetails = new DataTable();
            string strQry = string.Empty;
            try
            {

                strQry = "select  \"PTR_RI_NO\",TO_CHAR(\"PTR_RI_DATE\",'DD-MON-YYYY')\"PTR_RI_DATE\",TO_CHAR(\"PEST_DATE\",'DD-MON-YYYY')\"PEST_DATE\", ";
                strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT) = SUBSTR(CAST(\"PEST_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "')) \"DIVISION\", ";
                strQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT) =  SUBSTR(CAST(\"PEST_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "')) \"SUBDIVISION\",";
                strQry += " (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_CODE\" = \"PEST_LOC_CODE\") \"SECTION\",\"PTR_MANUAL_ACKRV_NO\",";
                strQry += " (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\" = \"TC_MAKE_ID\") \"MAKE\", \"TC_SLNO\", TO_CHAR(\"TC_MANF_DATE\",'DD-MON-YYYY') AS \"TC_MANF_DATE\",\"TC_CODE\",\"PTR_OIL_QUNTY\",";
                strQry += " (SELECT TO_CHAR(\"TM_MAPPING_DATE\",'DD-MON-YY') FROM \"TBLTRANSDTCMAPPING\" WHERE \"TM_ID\" = ";
                strQry += " (SELECT MAX(\"TM_ID\") FROM \"TBLTRANSDTCMAPPING\" WHERE \"TM_TC_ID\" = \"TC_CODE\")) AS \"DTRCOMMISIONDATE\",";
                strQry += " CAST(\"TC_CAPACITY\" AS TEXT)\"TC_CAPACITY\", (select \"SM_NAME\" from \"TBLSTOREMAST\" where \"SM_ID\"=\"PTR_STORE_SLNO\")\"SM_NAME\",";
                strQry += " (SELECT \"PWO_NO_DECOM\" FROM \"TBLPERMANENTWORKORDER\" WHERE \"PWO_PEF_ID\"=\"PEST_ID\") \"PWO_NO_DECOM\",\"PEST_DTC_CODE\",";
                strQry += " (SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=CAST(\"PEST_DTC_CODE\" AS TEXT)) \"DTC_NAME\",\"PTR_OIL_QTY_BYSK\",\"PTR_RV_NO\" \"ACK_NO\", TO_CHAR(\"PTR_RV_DATE\",'DD-MON-YYYY') AS \"ACK_DATE\", ";
                strQry += " \"PEST_NO\", ";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=SUBSTR(CAST(\"PEST_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "') AND cast(\"US_ROLE_ID\" AS TEXT)='1' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SDO_USERNAME\",";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=CAST(\"PEST_LOC_CODE\" AS TEXT) AND CAST(\"US_ROLE_ID\" AS TEXT)='4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SO_USERNAME\", ";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=SUBSTR(CAST(\"PEST_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "') AND CAST(\"US_ROLE_ID\" AS TEXT)='2' AND \"US_MMS_ID\" IS NULL AND CAST(\"US_STATUS\" AS TEXT)='A' LIMIT 1) \"STO_USERNAME\",";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=SUBSTR(CAST(\"PEST_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "') AND CAST(\"US_ROLE_ID\" AS TEXT)='5' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SK_USERNAME\"";
                strQry += " FROM \"TBLTCMASTER\",\"TBLPERMANENTTCREPLACE\",\"TBLPERMANENTESTIMATIONDETAILS\",\"TBLPERMANENTWORKORDER\"  WHERE ";
                strQry += " \"PEST_ID\"=\"PWO_PEF_ID\" AND \"PTR_WO_SLNO\"=\"PWO_SLNO\" AND  \"PEST_TC_CODE\"=\"TC_CODE\"  AND  CAST(\"PTR_ID\" AS TEXT)='" + sDecommId + "'";

                dtRiDetails = ObjCon.FetchDataTable(strQry);
                return dtRiDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtRiDetails;
            }
        }

        public DataTable RIReportpermanentsk(string sDecommId, string officecode)
        {
            DataTable dtRiDetails = new DataTable();
            string strQry = string.Empty;
            try
            {

                strQry = "select  \"PTR_RI_NO\",TO_CHAR(\"PTR_RI_DATE\",'DD-MON-YYYY')\"PTR_RI_DATE\",TO_CHAR(\"PEST_DATE\",'DD-MON-YYYY')\"PEST_DATE\", ";
                strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT) = SUBSTR(CAST(\"PEST_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "')) \"DIVISION\", ";
                strQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT) =  SUBSTR(CAST(\"PEST_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "')) \"SUBDIVISION\",";
                strQry += " (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_CODE\" = \"PEST_LOC_CODE\") \"SECTION\",\"PTR_MANUAL_ACKRV_NO\",";
                strQry += " (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\" = \"TC_MAKE_ID\") \"MAKE\", \"TC_SLNO\", TO_CHAR(\"TC_MANF_DATE\",'DD-MON-YYYY') AS \"TC_MANF_DATE\",\"TC_CODE\",\"PTR_OIL_QUNTY\",";
                strQry += " (SELECT TO_CHAR(\"TM_MAPPING_DATE\",'DD-MON-YY') FROM \"TBLTRANSDTCMAPPING\" WHERE \"TM_ID\" = ";
                strQry += " (SELECT MAX(\"TM_ID\") FROM \"TBLTRANSDTCMAPPING\" WHERE \"TM_TC_ID\" = \"TC_CODE\")) AS \"DTRCOMMISIONDATE\",";
                strQry += " CAST(\"TC_CAPACITY\" AS TEXT)\"TC_CAPACITY\", (select \"SM_NAME\" from \"TBLSTOREMAST\" where \"SM_ID\"=\"PTR_STORE_SLNO\")\"SM_NAME\",";
                strQry += " (SELECT \"PWO_NO_DECOM\" FROM \"TBLPERMANENTWORKORDER\" WHERE \"PWO_PEF_ID\"=\"PEST_ID\") \"PWO_NO_DECOM\",\"PEST_DTC_CODE\",";
                strQry += " (SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=CAST(\"PEST_DTC_CODE\" AS TEXT)) \"DTC_NAME\",\"PTR_OIL_QTY_BYSK\",\"PTR_RV_NO\" \"ACK_NO\", TO_CHAR(\"PTR_RV_DATE\",'DD-MON-YYYY') AS \"ACK_DATE\", ";
                strQry += " \"PEST_NO\", ";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=SUBSTR(CAST(\"PEST_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "') AND cast(\"US_ROLE_ID\" AS TEXT)='1' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SDO_USERNAME\",";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=CAST(\"PEST_LOC_CODE\" AS TEXT) AND CAST(\"US_ROLE_ID\" AS TEXT)='4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SO_USERNAME\", ";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"='" + officecode + "' AND CAST(\"US_ROLE_ID\" AS TEXT)='2' AND \"US_MMS_ID\" IS NULL AND CAST(\"US_STATUS\" AS TEXT)='A' LIMIT 1) \"STO_USERNAME\",";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"='" + officecode + "' AND CAST(\"US_ROLE_ID\" AS TEXT)='5' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SK_USERNAME\"";
                strQry += " FROM \"TBLTCMASTER\",\"TBLPERMANENTTCREPLACE\",\"TBLPERMANENTESTIMATIONDETAILS\",\"TBLPERMANENTWORKORDER\" WHERE ";
                strQry += " \"PEST_ID\"=\"PWO_PEF_ID\"   AND \"PTR_WO_SLNO\"=\"PWO_SLNO\" AND  \"PEST_TC_CODE\"=\"TC_CODE\"  AND  CAST(\"PTR_ID\" AS TEXT)='" + sDecommId + "'";

                dtRiDetails = ObjCon.FetchDataTable(strQry);
                return dtRiDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtRiDetails;
            }
        }


        public DataTable RIReportpermanentso(string sDtrcode, string sFailurId)
        {
            DataTable dtRiDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "select DISTINCT  TO_CHAR(\"PEST_DATE\",'DD-MON-YYYY')\"PEST_DATE\",'' AS \"PTR_RI_NO\", '' AS \"ACK_NO\", ";
                strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT) = SUBSTR(CAST(\"PEST_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "')) \"DIVISION\", ";
                strQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT) =  SUBSTR(CAST(\"PEST_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "')) \"SUBDIVISION\",";
                strQry += " (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_CODE\" = \"PEST_LOC_CODE\") \"SECTION\",";
                strQry += " (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\" = \"TC_MAKE_ID\") \"MAKE\", \"TC_SLNO\", TO_CHAR(\"TC_MANF_DATE\",'DD-MON-YYYY') AS \"TC_MANF_DATE\",\"TC_CODE\",";
                strQry += " (SELECT TO_CHAR(\"TM_MAPPING_DATE\",'DD-MON-YY') FROM \"TBLTRANSDTCMAPPING\" WHERE \"TM_ID\" = ";
                strQry += " (SELECT MAX(\"TM_ID\") FROM \"TBLTRANSDTCMAPPING\" WHERE \"TM_TC_ID\" = \"TC_CODE\")) AS \"DTRCOMMISIONDATE\",";
                strQry += " CAST(\"TC_CAPACITY\" AS TEXT)\"TC_CAPACITY\", (select \"SM_NAME\" from \"TBLSTOREMAST\" where \"SM_ID\"=\"TC_STORE_ID\")\"SM_NAME\",";
                strQry += "  \"PWO_NO_DECOM\",CAST(\"PEST_DTC_CODE\" AS TEXT),";
                strQry += " (SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=CAST(\"PEST_DTC_CODE\" AS TEXT)) \"DTC_NAME\", ";
                strQry += " ";
                strQry += "  \"PEST_NO\", ";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=SUBSTR(CAST(\"PEST_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "') AND cast(\"US_ROLE_ID\" AS TEXT)='1' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SDO_USERNAME\",";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=CAST(\"PEST_LOC_CODE\" AS TEXT) AND CAST(\"US_ROLE_ID\" AS TEXT)='4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SO_USERNAME\", ";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=SUBSTR(CAST(\"PEST_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "') AND CAST(\"US_ROLE_ID\" AS TEXT)='2' AND \"US_MMS_ID\" IS NULL AND CAST(\"US_STATUS\" AS TEXT)='A' LIMIT 1) \"STO_USERNAME\",";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=SUBSTR(CAST(\"PEST_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "') AND CAST(\"US_ROLE_ID\" AS TEXT)='5' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SK_USERNAME\"";
                strQry += " FROM \"TBLTCMASTER\",\"TBLPERMANENTESTIMATIONDETAILS\",\"TBLPERMANENTWORKORDER\" WHERE ";
                strQry += " \"PEST_TC_CODE\"=\"TC_CODE\" and  \"PEST_ID\"=\"PWO_PEF_ID\" and  \"TC_CODE\"='" + sDtrcode + "' and \"PEST_ID\"='" + sFailurId + "'";

                dtRiDetails = ObjCon.FetchDataTable(strQry);
                return dtRiDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtRiDetails;
            }
        }

        public DataTable CompletionReportpermanent(string sDecommId)
        {
            DataTable dtCompleteReport = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "  SELECT DISTINCT \"PWO_NO_DECOM\",TO_CHAR(\"PWO_DATE_DECOM\",'DD-MM-YYYY') \"PWO_DATE_DECOM\",CAST(\"PWO_DTC_CAP\" AS TEXT)\"EST_FAULT_CAPACITY\",\"DT_NAME\",";
                strQry += " TO_CHAR(\"TC_MANF_DATE\",'DD-MON-YYYY') AS \"TC_MANF_DATE\",CAST(\"TC_CODE\" AS TEXT)\"TC_CODE\",\"TC_SLNO\", ";
                strQry += " \"PTR_RV_NO\" \"ACK_NO\", TO_CHAR(\"PTR_RV_DATE\",'DD-MON-YYYY') AS \"ACK_DATE\",TO_CHAR(\"PEST_DATE\",'DD-MON-YYYY')\"PEST_DATE\",\"PEST_NO\",to_char(\"PEST_CRON\",'DD-MON-YYYY')\"PEST_CRON\",\"PEST_DTC_CODE\",";
                strQry += " CAST(\"PWO_NEW_CAP\" AS TEXT)\"PEST_REPLACE_CAPACITY\",\"PWO_DEVICE_ID\",";
                strQry += " \"PTR_RI_NO\",TO_CHAR(\"PTR_RI_DATE\",'DD-MON-YYYY')\"PTR_RI_DATE\",";
                strQry += " \"PTR_RV_NO\",TO_CHAR(\"PTR_RV_DATE\",'DD-MON-YYYY')\"PTR_RV_DATE\",(with Grp as( SELECT rank() over (partition by \"WO_RECORD_ID\" order by \"WO_ID\" desc),\"WO_CR_ON\" FROM \"TBLWORKFLOWOBJECTS\" WHERE ";
                strQry += " \"WO_RECORD_ID\"=\"PTR_ID\" and \"WO_BO_ID\"='67')  SELECT TO_CHAR(\"WO_CR_ON\",'DD-MON-YYYY') \"PTR_COMM_DATE\" FROM Grp WHERE rank =1),";
                strQry += " (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT) = SUBSTR(CAST(\"PEST_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "')) \"DIVISION\",";
                strQry += " (SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT)=SUBSTR(CAST(\"PEST_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "')) AS \"SUBDIVISION\",";
                strQry += " (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE CAST(\"OM_CODE\" AS TEXT)=SUBSTR(CAST(\"PEST_LOC_CODE\" AS TEXT),1,'" + Constants.Section + "')) AS \"SECTION\",";
                strQry += " (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\") \"MAKE\",";
                strQry += "  \"PWO_AMT_DECOM\" AS \"PEST_UNIT_PRICE\",\"PWO_AMT_DECOM\",\"PTR_INVENTORY_QTY\",\"PTR_DECOM_INV_QTY\",TO_CHAR(\"DT_TRANS_COMMISION_DATE\",'DD/MON/YYYY')\"DTC_COMMISSION_DATE\", ";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=SUBSTR(CAST(\"PEST_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "') AND \"US_ROLE_ID\"='1' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A') SDO_USERNAME,";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=CAST(\"PEST_LOC_CODE\" AS TEXT) AND CAST(\"US_ROLE_ID\" AS TEXT)='4'AND \"PEST_CRBY\"=\"US_ID\" AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A') \"SO_USERNAME\" ";
                strQry += " FROM \"TBLTCMASTER\",\"TBLPERMANENTWORKORDER\",\"TBLPERMANENTTCREPLACE\",\"TBLPERMANENTESTIMATIONDETAILS\",\"TBLDTCMAST\",\"TBLTRANSDTCMAPPING\" WHERE  ";
                strQry += " \"PTR_WO_SLNO\"=\"PWO_SLNO\" AND \"PEST_ID\"=\"PWO_PEF_ID\"  AND \"PEST_TC_CODE\" = \"TC_CODE\" AND \"DT_CODE\" = CAST(\"PEST_DTC_CODE\" AS TEXT) ";
                strQry += " AND \"TM_DTC_ID\"=CAST(\"PEST_DTC_CODE\" AS TEXT) AND \"TM_ID\" = (SELECT \"TM_ID\" FROM ( SELECT \"TM_ID\",\"TM_DTC_ID\",\"TM_TC_ID\", row_number() over (partition by \"TM_DTC_ID\",\"TM_TC_ID\" order by \"TM_ID\" desc) as rn FROM ";
                strQry += " \"TBLTRANSDTCMAPPING\" WHERE \"TM_DTC_ID\"=CAST(\"PEST_DTC_CODE\" AS TEXT)) A WHERE \"TM_DTC_ID\"=CAST(\"PEST_DTC_CODE\" AS TEXT) AND \"TM_TC_ID\"=\"PEST_TC_CODE\" AND rn = 1)  AND  \"PTR_ID\"='" + sDecommId + "'";

                dtCompleteReport = ObjCon.FetchDataTable(strQry);
                return dtCompleteReport;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtCompleteReport;

            }

        }


        public DataTable GetDtcFailDetail(clsReports objReport)
        {
            DataTable dtDtcFailDetails = new DataTable();
            string StrQry = string.Empty;
            try
            {

                StrQry = "SELECT \"DF_DTC_CODE\",\"DF_EQUIPMENT_ID\",TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')\"DF_DATE\",\"TC_SLNO\",\"DF_PGRS_DOCKET\",TO_CHAR(\"DF_PGRS_DOCKET_DATE\",'YYYY/MM/DD') \"DF_PGRS_DOCKET_DATE\",\"DT_TIMS_CODE\",";
                StrQry += "'" + objReport.sFromDate + "' AS \"FROM_DATE\",'" + objReport.sTodate + "'AS \"TODATE\",TO_CHAR(CURRENT_DATE,'YYYY/MM/DD')AS \"TODAY\"";
                StrQry += "FROM \"TBLDTCFAILURE\" INNER JOIN \"TBLDTCMAST\" ON \"DT_CODE\"=\"DF_DTC_CODE\" INNER JOIN \"TBLTCMASTER\"";
                StrQry += "ON \"DF_EQUIPMENT_ID\"=\"TC_CODE\" WHERE  CAST(\"DF_LOC_CODE\" AS TEXT) LIKE '" + objReport.sOfficeCode + "%'";

                if (objReport.sFailureStatus != "0")
                {
                    StrQry += " AND \"DF_REPLACE_FLAG\"='" + objReport.sFailureStatus + "'";
                }


                if (objReport.sTodate == null && (objReport.sFromDate != null))
                {
                    StrQry += " AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD')";

                }
                if (objReport.sFromDate == null && (objReport.sTodate != null))
                {
                    StrQry += "AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                }
                if (objReport.sFromDate == null && objReport.sTodate == null)
                {
                    StrQry += " AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<=TO_CHAR(CURRENT_DATE,'YYYY/MM/DD') ";
                }
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    StrQry += "AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                }
                if (objReport.sFeeder != null && objReport.sFeeder != "")
                {
                    StrQry += " AND \"DT_FDRSLNO\" = '" + objReport.sFeeder + "'";
                }
                dtDtcFailDetails = ObjCon.FetchDataTable(StrQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtDtcFailDetails;
            }
            return dtDtcFailDetails;
        }

        public DataTable getAbstractDtrDetails(string sofficeCode)
        {
            DataTable dtAbstractDTrDetails = new DataTable();
            try
            {
                string StrQry = string.Empty;

                //StrQry = "SELECT \"TC_LOCATION_ID\",B.\"SUBDIV\",\"ZONE_NAME\",\"CIRCLE_NAME\",\"DIVISION_NAME\",\"SUBDIVISION_NAME\",\"SECTION_NAME\",\"TOTAL_COUNT\",\"SUBDIV_TOTAL_COUNT\" FROM (SELECT \"TC_LOCATION_ID\",substr(cast(\"TC_LOCATION_ID\" as text),1,4) as \"SUBDIV\",(SELECT cast(\"ZO_NAME\" as text) FROM \"TBLZONE\"";
                //StrQry += " WHERE cast(\"ZO_ID\" as text)=substr(cast(\"TC_LOCATION_ID\" as text),1,1)) AS \"ZONE_NAME\",(SELECT cast(\"CM_CIRCLE_NAME\" as text) FROM \"TBLCIRCLE\" WHERE cast(\"CM_CIRCLE_CODE\" as text)=substr(cast(\"TC_LOCATION_ID\" as text),1,2)) AS \"CIRCLE_NAME\",(SELECT cast(\"DIV_NAME\" as text) FROM \"TBLDIVISION\"";
                //StrQry += "  WHERE cast(\"DIV_CODE\" as text)=substr(cast(\"TC_LOCATION_ID\" as text),1,3)) AS \"DIVISION_NAME\",(SELECT cast(\"SD_SUBDIV_NAME\" as text) FROM \"TBLSUBDIVMAST\" WHERE cast(\"SD_SUBDIV_CODE\" as text)=substr(cast(\"TC_LOCATION_ID\" as text),1,4)) AS \"SUBDIVISION_NAME\",\"OM_NAME\" AS \"SECTION_NAME\", count(\"TC_ID\")";
                //StrQry += "  AS \"TOTAL_COUNT\" FROM \"TBLTCMASTER\",\"TBLOMSECMAST\" WHERE \"TC_LOCATION_ID\"=\"OM_CODE\" and \"TC_CURRENT_LOCATION\"='2' and cast(\"TC_LOCATION_ID\" as text) like '" + sofficeCode + "%' GROUP BY \"TC_LOCATION_ID\",substr(cast(\"TC_LOCATION_ID\" as text),1,4),\"OM_NAME\" )A left join (SELECT substr(cast(\"TC_LOCATION_ID\" as text),1,4)";
                //StrQry += " AS \"SUBDIV\", count(\"TC_ID\") AS \"SUBDIV_TOTAL_COUNT\" FROM \"TBLTCMASTER\",\"TBLOMSECMAST\" WHERE  \"TC_LOCATION_ID\"=\"OM_CODE\" and \"TC_CURRENT_LOCATION\"='2' and cast(\"TC_LOCATION_ID\" as text) like '" + sofficeCode + "%'  and \"TC_CODE\"<>'0' and \"TC_CAPACITY\"<>0 GROUP BY substr(cast(\"TC_LOCATION_ID\" as text),1,4) ORDER BY substr(cast(\"TC_LOCATION_ID\" as text),1,4))B ON A.\"SUBDIV\" = B.\"SUBDIV\" ORDER BY \"TC_LOCATION_ID\"";
                //dtAbstractDTrDetails = ObjCon.FetchDataTable(StrQry);
                //return dtAbstractDTrDetails;
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_abstract_dtr_details_report_clsreport");
                cmd.Parameters.AddWithValue("office_code", Convert.ToString(sofficeCode ?? ""));

                dtAbstractDTrDetails = ObjCon.FetchDataTable(cmd);

                return dtAbstractDTrDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtAbstractDTrDetails;
            }
        }

        //public DataTable GetPGRSDetails(clsReports objReport)
        //{
        //    DataTable dt = new DataTable();
        //    string strQry = string.Empty;
        //    strQry = "SELECT \"DF_DTC_CODE\",\"DF_EQUIPMENT_ID\",\"TC_SLNO\",\"DF_PGRS_DOCKET\",\"OM_NAME\" FROM \"TBLDTCFAILURE\",\"TBLDTCMAST\",";
        //    strQry += " \"TBLFEEDERMAST\",\"TBLTCMASTER\",\"TBLOMSECMAST\" WHERE \"DF_DTC_CODE\"=\"DT_CODE\" and \"DT_FDRSLNO\"=\"FD_FEEDER_CODE\" ";
        //    strQry +=" AND \"DF_EQUIPMENT_ID\"=\"TC_CODE\" and \"DF_LOC_CODE\"=\"OM_CODE\"";
        //    if(objReport.sOfficeCode != null)
        //    {
        //        strQry += " AND cast(\"DF_LOC_CODE\" as text) like '"+ objReport.sOfficeCode + "%'";
        //    }
        //    if(objReport.sFeeder != null)
        //    {
        //        strQry += " AND \"FD_FEEDER_CODE\"= '"+ objReport.sFeeder + "' ";
        //    }
        //    if(objReport.sFromDate != null)
        //    {
        //        strQry += " AND ";
        //    }
        //    return dt;
        //}

        public DataTable PrintStoreDetailsabstract(string sOfficeCode, string sFromEnumDate, string sToEnumDate, string sdatewise)
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));

            DataTable dtStoreDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                #region Commented inline query
                //if (sdatewise == "1")
                //{
                //    strQry = " select (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE cast(\"ED_OFFICECODE\" as text)=cast(\"DIV_CODE\" as text)) \"OFF_NAME\",count(distinct \"DTE_TC_CODE\") \"DTE_TC_CODE\" ";
                //    strQry += " FROM ";
                //    strQry += " \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\",\"TBLSTOREMAST\" where \"ED_LOCNAME\"=cast(\"SM_ID\" as text) and CAST(\"ED_ID\" AS TEXT)= CAST(\"DTE_ED_ID\" AS TEXT) ";
                //    strQry += "AND CAST(\"SM_CODE\" AS TEXT) Like '" + sOfficeCode + "%' ";
                //    strQry += "and  \"ED_LOCTYPE\" IN ('1','3','5') AND  \"ED_STATUS_FLAG\" IN (0,2) ";
                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }
                //    strQry += "group by \"OFF_NAME\"";
                //}
                //else if (sdatewise == "2")
                //{
                //    strQry = " select (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE cast(\"ED_OFFICECODE\" as text)=cast(\"DIV_CODE\" as text)) \"OFF_NAME\",count(distinct \"DTE_TC_CODE\") \"DTE_TC_CODE\" FROM ";
                //    strQry += " \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\",\"TBLSTOREMAST\" where \"ED_LOCNAME\"=cast(\"SM_ID\" as text) and CAST(\"ED_ID\" AS TEXT)= CAST(\"DTE_ED_ID\" AS TEXT) ";
                //    strQry += "AND CAST(\"SM_CODE\" AS TEXT) Like '" + sOfficeCode + "%' ";
                //    strQry += "and  \"ED_LOCTYPE\" IN ('1','3','5') AND  \"ED_STATUS_FLAG\" IN (1) ";
                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }
                //    strQry += "group by \"OFF_NAME\"";
                //}

                //else if (sdatewise == "4")
                //{
                //    strQry = " select (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE cast(\"ED_OFFICECODE\" as text)=cast(\"DIV_CODE\" as text)) \"OFF_NAME\",count(distinct \"DTE_TC_CODE\") \"DTE_TC_CODE\" FROM ";
                //    strQry += " \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\",\"TBLSTOREMAST\" where \"ED_LOCNAME\"=cast(\"SM_ID\" as text) and CAST(\"ED_ID\" AS TEXT)= CAST(\"DTE_ED_ID\" AS TEXT) ";
                //    strQry += "AND CAST(\"SM_CODE\" AS TEXT) Like '" + sOfficeCode + "%' ";
                //    strQry += " and  \"ED_LOCTYPE\" IN ('1','3','5') AND  \"ED_STATUS_FLAG\" IN (3) ";
                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }
                //    strQry += "group by \"OFF_NAME\"";
                //}
                //else if (sdatewise == "5")
                //{
                //    strQry = " select (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE cast(\"ED_OFFICECODE\" as text)=cast(\"DIV_CODE\" as text)) \"OFF_NAME\",count(distinct \"DTE_TC_CODE\") \"DTE_TC_CODE\" FROM ";
                //    strQry += " \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\",\"TBLSTOREMAST\" where \"ED_LOCNAME\"=cast(\"SM_ID\" as text) and CAST(\"ED_ID\" AS TEXT)= CAST(\"DTE_ED_ID\" AS TEXT) ";
                //    strQry += "AND CAST(\"SM_CODE\" AS TEXT) Like '" + sOfficeCode + "%' ";
                //    strQry += " and  \"ED_LOCTYPE\" IN ('1','3','5') AND  \"ED_STATUS_FLAG\" IN (0,1,2,3) ";
                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }
                //    strQry += "group by \"OFF_NAME\"";
                //}
                //else
                //{
                //    strQry = " select (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE cast(\"ED_OFFICECODE\" as text)=cast(\"DIV_CODE\" as text)) \"OFF_NAME\",count(distinct \"DTE_TC_CODE\") \"DTE_TC_CODE\" FROM ";
                //    strQry += " \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\",\"TBLSTOREMAST\" where \"ED_LOCNAME\"=cast(\"SM_ID\" as text) and \"ED_ID\"= \"DTE_ED_ID\" ";
                //    strQry += "AND CAST(\"SM_CODE\" AS TEXT) Like '" + sOfficeCode + "%' ";
                //    strQry += " and  \"ED_LOCTYPE\" IN ('1','3','5') AND  \"ED_STATUS_FLAG\" IN (3) ";
                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        strQry += " AND TO_CHAR(\"ED_WELD_DATE\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }
                //    strQry += "group by \"OFF_NAME\"";
                //}

                //dtStoreDetails = ObjCon.FetchDataTable(strQry);
                #endregion
                string fromdate = string.Empty;
                string todate = string.Empty;
                if (sFromEnumDate != "" && sToEnumDate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    fromdate = dFromDate.ToString("yyyyMMdd");
                    todate = dToDate.ToString("yyyyMMdd");
                }
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_store_enumerationreport_clsreport");
                cmd.Parameters.AddWithValue("sdatewise", Convert.ToString(sdatewise ?? ""));
                cmd.Parameters.AddWithValue("division_code", Convert.ToInt32(Division_code));
                cmd.Parameters.AddWithValue("subdivision_code", Convert.ToInt32(SubDiv_code));
                cmd.Parameters.AddWithValue("section_code", Convert.ToInt32(Section_code));
                cmd.Parameters.AddWithValue("office_code", Convert.ToString(sOfficeCode) ?? "");
                cmd.Parameters.AddWithValue("from_enum_date", Convert.ToString(fromdate) ?? "");
                cmd.Parameters.AddWithValue("to_enum_date", Convert.ToString(todate ?? ""));
                dtStoreDetails = ObjCon.FetchDataTable(cmd);
                return dtStoreDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtStoreDetails;

            }
        }


        public DataTable PrintStoreDetailsabstractCount(string sOfficeCode, string sFromEnumDate, string sToEnumDate, string sdatewise)
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));

            DataTable dtStoreDetails = new DataTable();
            string strQry = string.Empty;
            try
            {



                //if (sdatewise == "1")
                //{
                //    strQry = " select (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE cast(\"ED_OFFICECODE\" as text)=cast(\"DIV_CODE\" as text)) \"OFF_NAME\",count(distinct \"DTE_TC_CODE\") \"DTE_TC_CODE\" ";
                //    strQry += " FROM ";
                //    strQry += " \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\",\"TBLSTOREMAST\" where \"ED_LOCNAME\"=cast(\"SM_ID\" as text) and CAST(\"ED_ID\" AS TEXT)= CAST(\"DTE_ED_ID\" AS TEXT) ";
                //    strQry += "AND CAST(\"SM_CODE\" AS TEXT) Like '" + sOfficeCode + "%' ";

                //    strQry += "and  \"ED_LOCTYPE\" IN ('1','3','5') AND  \"ED_STATUS_FLAG\" IN (0,2) ";
                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }
                //    strQry += "group by \"OFF_NAME\"";
                //    strQry += "UNION ALL  select 'TOTAL' \"OFF_NAME\",count(distinct \"DTE_TC_CODE\") FROM  \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\" ,\"TBLSTOREMAST\" where \"ED_LOCNAME\"=cast(\"SM_ID\" as text) and CAST(\"ED_ID\" AS TEXT)= CAST(\"DTE_ED_ID\" AS TEXT)";
                //    strQry += "AND CAST(\"SM_CODE\" AS TEXT) Like '" + sOfficeCode + "%' ";

                //    strQry += " and  \"ED_LOCTYPE\" IN('1', '3', '5') AND  \"ED_STATUS_FLAG\" IN(0, 2) ";
                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }
                //}
                //else if (sdatewise == "2")
                //{
                //    strQry = " select (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE cast(\"ED_OFFICECODE\" as text)=cast(\"DIV_CODE\" as text)) \"OFF_NAME\",count(distinct \"DTE_TC_CODE\") \"DTE_TC_CODE\" ";
                //    strQry += " FROM ";
                //    strQry += " \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\" ,\"TBLSTOREMAST\" where \"ED_LOCNAME\"=cast(\"SM_ID\" as text) and  CAST(\"ED_ID\" AS TEXT)= CAST(\"DTE_ED_ID\" AS TEXT) ";
                //    strQry += "AND CAST(\"SM_CODE\" AS TEXT) Like '" + sOfficeCode + "%' ";

                //    strQry += "and  \"ED_LOCTYPE\" IN ('1','3','5') AND  \"ED_STATUS_FLAG\" IN (1) ";
                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }
                //    strQry += "group by \"OFF_NAME\"";
                //    strQry += "UNION ALL  select 'TOTAL' \"OFF_NAME\",count(distinct \"DTE_TC_CODE\") FROM  \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\" ,\"TBLSTOREMAST\" where \"ED_LOCNAME\"=cast(\"SM_ID\" as text) and CAST(\"ED_ID\" AS TEXT)= CAST(\"DTE_ED_ID\" AS TEXT) ";
                //    strQry += "AND CAST(\"SM_CODE\" AS TEXT) Like '" + sOfficeCode + "%' ";

                //    strQry += " and  \"ED_LOCTYPE\" IN('1', '3', '5') AND  \"ED_STATUS_FLAG\" IN(1) ";
                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }
                //    //strQry += " UNION ALL";

                //}

                //else if (sdatewise == "4")
                //{
                //    strQry = " select (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE cast(\"ED_OFFICECODE\" as text)=cast(\"DIV_CODE\" as text)) \"OFF_NAME\",count(distinct \"DTE_TC_CODE\") \"DTE_TC_CODE\" ";
                //    strQry += " FROM ";
                //    strQry += " \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\" ,\"TBLSTOREMAST\" where \"ED_LOCNAME\"=cast(\"SM_ID\" as text) and CAST(\"ED_ID\" AS TEXT)= CAST(\"DTE_ED_ID\" AS TEXT) ";
                //    strQry += "AND CAST(\"SM_CODE\" AS TEXT) Like '" + sOfficeCode + "%' ";

                //    strQry += " and  \"ED_LOCTYPE\" IN ('1','3','5') AND  \"ED_STATUS_FLAG\" IN (3) ";
                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }
                //    strQry += "group by \"OFF_NAME\"";
                //    strQry += "UNION ALL  select 'TOTAL' \"OFF_NAME\",count(distinct \"DTE_TC_CODE\") FROM  \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\" ,\"TBLSTOREMAST\" where \"ED_LOCNAME\"=cast(\"SM_ID\" as text) and CAST(\"ED_ID\" AS TEXT)= CAST(\"DTE_ED_ID\" AS TEXT)";
                //    strQry += "AND CAST(\"SM_CODE\" AS TEXT) Like '" + sOfficeCode + "%' ";

                //    strQry += " and  \"ED_LOCTYPE\" IN('1', '3', '5') AND  \"ED_STATUS_FLAG\" IN(3) ";
                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }
                //}
                //else if (sdatewise == "5")
                //{
                //    strQry = " select (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE cast(\"ED_OFFICECODE\" as text)=cast(\"DIV_CODE\" as text)) \"OFF_NAME\",count(distinct \"DTE_TC_CODE\") \"DTE_TC_CODE\" ";
                //    strQry += " FROM ";
                //    strQry += " \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\" ,\"TBLSTOREMAST\" where \"ED_LOCNAME\"=cast(\"SM_ID\" as text) and CAST(\"ED_ID\" AS TEXT)= CAST(\"DTE_ED_ID\" AS TEXT) ";
                //    strQry += "AND CAST(\"SM_CODE\" AS TEXT) Like '" + sOfficeCode + "%' ";

                //    strQry += " and  \"ED_LOCTYPE\" IN ('1','3','5') AND  \"ED_STATUS_FLAG\" IN (0,1,2,3) ";
                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }
                //    strQry += "group by \"OFF_NAME\"";
                //    strQry += "UNION ALL  select 'TOTAL' \"OFF_NAME\",count(distinct \"DTE_TC_CODE\") FROM  \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\" ,\"TBLSTOREMAST\" where \"ED_LOCNAME\"=cast(\"SM_ID\" as text) and CAST(\"ED_ID\" AS TEXT)= CAST(\"DTE_ED_ID\" AS TEXT)";
                //    strQry += "AND CAST(\"SM_CODE\" AS TEXT) Like '" + sOfficeCode + "%' ";

                //    strQry += " and  \"ED_LOCTYPE\" IN('1', '3', '5') AND  \"ED_STATUS_FLAG\" IN(0,1,2,3) ";
                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }
                //}
                //else
                //{
                //    //"ED_LOCNAME"=cast("TR_ID" as text)
                //    strQry = " select (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE cast(\"ED_OFFICECODE\" as text)=cast(\"DIV_CODE\" as text)) \"OFF_NAME\",count(distinct \"DTE_TC_CODE\") \"DTE_TC_CODE\" ";
                //    strQry += " FROM ";
                //    strQry += " \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\" where \"ED_ID\"= \"DTE_ED_ID\" ";
                //    if (sOfficeCode == "")
                //    {
                //        strQry += "AND CAST(\"ED_OFFICECODE\" AS TEXT) Like '%' ";
                //    }
                //    else
                //    {
                //        strQry += "AND CAST(\"ED_OFFICECODE\" AS TEXT) Like '" + sOfficeCode + "' ";
                //    }
                //    strQry += " and  \"ED_LOCTYPE\" IN ('1','3','5') AND  \"ED_STATUS_FLAG\" IN (3) ";
                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        strQry += " AND TO_CHAR(\"ED_WELD_DATE\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }
                //    strQry += "group by \"OFF_NAME\"";

                //}

                //dtStoreDetails = ObjCon.FetchDataTable(strQry);
                //return dtStoreDetails;


                string fromdate = string.Empty;
                string todate = string.Empty;

                if (sFromEnumDate != "" && sToEnumDate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    fromdate = dFromDate.ToString("yyyyMMdd");
                    todate = dToDate.ToString("yyyyMMdd");

                }

                NpgsqlCommand cmd = new NpgsqlCommand("sp_printstoredetailsabstractcount_clsreports");
                cmd.Parameters.AddWithValue("sdatewise", Convert.ToString(sdatewise));
                cmd.Parameters.AddWithValue("offcode", Convert.ToString(sOfficeCode));
                cmd.Parameters.AddWithValue("divcode", Convert.ToString(Division_code));
                cmd.Parameters.AddWithValue("subdivcode", Convert.ToString(SubDiv_code));
                cmd.Parameters.AddWithValue("sectioncode", Convert.ToString(Section_code));
                cmd.Parameters.AddWithValue("sfromenumdate", Convert.ToString(sFromEnumDate));
                cmd.Parameters.AddWithValue("stoenumdate", Convert.ToString(sToEnumDate));
                dtStoreDetails = ObjCon.FetchDataTable(cmd);
                return dtStoreDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtStoreDetails;

            }
        }



        public DataTable PrintFieldDetailsAbstract(string sFeederCode, string sOfficeCode, string sFromEnumDate, string sToEnumDate, string sdatewise)
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            DataTable dtStoreDetails = new DataTable("A");
            string strQry = string.Empty;
            try
            {




                #region Commented inline Querys
                //if (sdatewise == "1")
                //{
                //    strQry = "   select  (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + Division_code + "')=cast(\"DIV_CODE\" as text)) \"DIV\",(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + SubDiv_code + "')=cast(\"SD_SUBDIV_CODE\" as text)) \"SUB_DIV\",(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + Section_code + "')=cast(\"OM_CODE\" as text)) \"SECTION\",count(distinct \"DTE_TC_CODE\") \"DTE_TC_CODE\" ";
                //    strQry += " FROM \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\",\"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"ED_ID\"= \"DTE_ED_ID\" ";
                //    strQry += " and \"ED_LOCTYPE\"='2' AND \"ED_STATUS_FLAG\" IN (0) AND  \"FD_FEEDER_CODE\"=\"ED_FEEDERCODE\" AND \"FD_FEEDER_ID\"=\"FDO_FEEDER_ID\" ";
                //    if (sFeederCode != "")
                //    {
                //        strQry += " AND \"ED_FEEDERCODE\" Like '" + sFeederCode + "%' ";
                //    }
                //    strQry += "AND cast(\"ED_OFFICECODE\" as text) LIKE '" + sOfficeCode + "%'";

                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }
                //    strQry += "group by \"DIV\" ,\"SUB_DIV\" ,\"SECTION\" ";

                //}
                //else if (sdatewise == "2")
                //{
                //    strQry = "   select  (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + Division_code + "')=cast(\"DIV_CODE\" as text)) \"DIV\",(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + SubDiv_code + "')=cast(\"SD_SUBDIV_CODE\" as text)) \"SUB_DIV\",(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + Section_code + "')=cast(\"OM_CODE\" as text)) \"SECTION\",count(distinct \"DTE_TC_CODE\") \"DTE_TC_CODE\" ";

                //    strQry += " FROM \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\",\"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"ED_ID\"= \"DTE_ED_ID\" ";
                //    strQry += " and \"ED_LOCTYPE\"='2' AND \"ED_STATUS_FLAG\" IN (1) AND  \"FD_FEEDER_CODE\"=\"ED_FEEDERCODE\" AND \"FD_FEEDER_ID\"=\"FDO_FEEDER_ID\" ";
                //    if (sFeederCode != "")
                //    {
                //        strQry += " AND \"ED_FEEDERCODE\" Like '" + sFeederCode + "%' ";
                //    }
                //    strQry += "AND cast(\"ED_OFFICECODE\" as text) LIKE '" + sOfficeCode + "%'";

                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        strQry += " AND TO_CHAR(\"ED_WELD_DATE\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }
                //    strQry += "group by \"DIV\" ,\"SUB_DIV\" ,\"SECTION\" ";

                //}
                //else if (sdatewise == "3")
                //{
                //    strQry = "select  (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE cast(\"DT_OM_SLNO\" as text)=cast(\"OFF_CODE\" as text)) \"DIV\",(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + SubDiv_code + "')=cast(\"SD_SUBDIV_CODE\" as text)) \"SUB_DIV\",(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + Section_code + "')=cast(\"OM_CODE\" as text)) \"SECTION\" ,count(distinct \"DT_CODE\") \"DTE_TC_CODE\"";
                //    strQry += " FROM \"TBLDTCMAST\",\"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" WHERE \"DT_FDRSLNO\"=\"FD_FEEDER_CODE\" AND ";
                //    strQry += "\"FD_FEEDER_ID\"=\"FDO_FEEDER_ID\" AND \"DT_STATUS\"='1' AND cast(\"DT_OM_SLNO\" as text) LIKE '" + sOfficeCode + "%' ";
                //    if (sFeederCode != "")
                //    {
                //        strQry += " AND  CAST(\"DT_FDRSLNO\" AS TEXT) Like '" + sFeederCode + "%' ";
                //    }
                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        strQry += " AND TO_CHAR(\"DT_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }
                //    strQry += "group by \"DIV\" ,\"SUB_DIV\" ,\"SECTION\" ";
                //}
                //else if (sdatewise == "4")
                //{
                //    strQry = "   select  (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + Division_code + "')=cast(\"DIV_CODE\" as text)) \"DIV\",(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + SubDiv_code + "')=cast(\"SD_SUBDIV_CODE\" as text)) \"SUB_DIV\",(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + Section_code + "')=cast(\"OM_CODE\" as text)) \"SECTION\",count(distinct \"DTE_TC_CODE\") \"DTE_TC_CODE\" ";

                //    strQry += " FROM \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\",\"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"ED_ID\"= \"DTE_ED_ID\" ";
                //    strQry += " and \"ED_LOCTYPE\"='2' AND \"ED_STATUS_FLAG\" IN (3) AND  \"FD_FEEDER_CODE\"=\"ED_FEEDERCODE\" AND \"FD_FEEDER_ID\"=\"FDO_FEEDER_ID\" ";
                //    if (sFeederCode != "")
                //    {
                //        strQry += " AND \"ED_FEEDERCODE\" Like '" + sFeederCode + "%' ";
                //    }
                //    strQry += "AND cast(\"ED_OFFICECODE\" as text) LIKE '" + sOfficeCode + "%'";

                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        strQry += " AND TO_CHAR(\"ED_WELD_DATE\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }
                //    strQry += "group by \"DIV\" ,\"SUB_DIV\" ,\"SECTION\" ";


                //}
                //else if (sdatewise == "5")
                //{
                //    strQry = "   select  (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + Division_code + "')=cast(\"DIV_CODE\" as text)) \"DIV\",(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + SubDiv_code + "')=cast(\"SD_SUBDIV_CODE\" as text)) \"SUB_DIV\",(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + Section_code + "')=cast(\"OM_CODE\" as text)) \"SECTION\",count(distinct \"DTE_TC_CODE\") \"DTE_TC_CODE\" ";

                //    strQry += " FROM \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\",\"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"ED_ID\"= \"DTE_ED_ID\" ";
                //    strQry += " and \"ED_LOCTYPE\"='2' AND \"ED_STATUS_FLAG\" IN (0,1,2,3) AND  \"FD_FEEDER_CODE\"=\"ED_FEEDERCODE\" AND \"FD_FEEDER_ID\"=\"FDO_FEEDER_ID\" ";
                //    if (sFeederCode != "")
                //    {
                //        strQry += " AND \"ED_FEEDERCODE\" Like '" + sFeederCode + "%' ";
                //    }
                //    strQry += "AND cast(\"ED_OFFICECODE\" as text) LIKE '" + sOfficeCode + "%'";

                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        strQry += " AND TO_CHAR(\"ED_WELD_DATE\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }
                //    strQry += "group by \"DIV\" ,\"SUB_DIV\" ,\"SECTION\" ";


                //}



                //dtStoreDetails = ObjCon.FetchDataTable(strQry);
                #endregion

                string fromdate = string.Empty;
                string todate = string.Empty;
                if (sFromEnumDate != "" && sToEnumDate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    fromdate = dFromDate.ToString("yyyyMMdd");
                    todate = dToDate.ToString("yyyyMMdd");
                }
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_field_enumerationreport_clsreport");
                cmd.Parameters.AddWithValue("sdatewise", Convert.ToString(sdatewise ?? ""));
                cmd.Parameters.AddWithValue("division_code", Convert.ToInt32(Division_code));
                cmd.Parameters.AddWithValue("subdivision_code", Convert.ToInt32(SubDiv_code));
                cmd.Parameters.AddWithValue("section_code", Convert.ToInt32(Section_code));
                cmd.Parameters.AddWithValue("feeder_code", Convert.ToString(sFeederCode ?? ""));
                cmd.Parameters.AddWithValue("office_code", Convert.ToString(sOfficeCode) ?? "");
                cmd.Parameters.AddWithValue("from_enum_date", Convert.ToString(fromdate) ?? "");
                cmd.Parameters.AddWithValue("to_enum_date", Convert.ToString(todate ?? ""));
                dtStoreDetails = ObjCon.FetchDataTable(cmd);




                return dtStoreDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtStoreDetails;

            }
        }
        public DataTable PrintFieldDetailsAbstractCount(string sFeederCode, string sOfficeCode, string sFromEnumDate, string sToEnumDate, string sdatewise)
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            DataTable dtfieldabstractcount = new DataTable();
            string strQry = string.Empty;
            try
            {
                #region
                //if (sdatewise == "1")
                //{
                //    strQry = "   select  (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + Division_code + "')=cast(\"DIV_CODE\" as text)) \"DIV\",(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + SubDiv_code + "')=cast(\"SD_SUBDIV_CODE\" as text)) \"SUB_DIV\",(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + Section_code + "')=cast(\"OM_CODE\" as text)) \"SECTION\",count(distinct \"DTE_TC_CODE\") \"DTE_TC_CODE\" ";
                //    strQry += " FROM \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\",\"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"ED_ID\"= \"DTE_ED_ID\" ";
                //    strQry += " and \"ED_LOCTYPE\"='2' AND \"ED_STATUS_FLAG\" IN (0) AND  \"FD_FEEDER_CODE\"=\"ED_FEEDERCODE\" AND \"FD_FEEDER_ID\"=\"FDO_FEEDER_ID\" ";
                //    if (sFeederCode != "")
                //    {
                //        strQry += " AND \"ED_FEEDERCODE\" Like '" + sFeederCode + "%' ";
                //    }
                //    strQry += "AND cast(\"ED_OFFICECODE\" as text) LIKE '" + sOfficeCode + "%'";

                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }
                //    strQry += "group by \"DIV\" ,\"SUB_DIV\" ,\"SECTION\" ";
                //    strQry += " UNION ALL  select 'TOTAL' \"DIV\",'' as \"SUB_DIV\",'' as \"SECTION\",count(distinct \"DTE_TC_CODE\") FROM  \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\",\"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"ED_ID\"= \"DTE_ED_ID\"  and \"ED_LOCTYPE\"='2' AND \"ED_STATUS_FLAG\" IN (0) AND  \"FD_FEEDER_CODE\"=\"ED_FEEDERCODE\" AND \"FD_FEEDER_ID\"=\"FDO_FEEDER_ID\" ";
                //    if (sFeederCode != "")
                //    {
                //        strQry += " AND \"ED_FEEDERCODE\" Like '" + sFeederCode + "%' ";
                //    }
                //    strQry += " AND cast(\"ED_OFFICECODE\" as text) LIKE '" + sOfficeCode + "%'";

                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }
                //}
                //else if (sdatewise == "2")
                //{

                //    strQry = "   select  (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + Division_code + "')=cast(\"DIV_CODE\" as text)) \"DIV\",(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + SubDiv_code + "')=cast(\"SD_SUBDIV_CODE\" as text)) \"SUB_DIV\",(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + Section_code + "')=cast(\"OM_CODE\" as text)) \"SECTION\",count(distinct \"DTE_TC_CODE\") \"DTE_TC_CODE\" ";

                //    strQry += " FROM \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\",\"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"ED_ID\"= \"DTE_ED_ID\" ";
                //    strQry += " and \"ED_LOCTYPE\"='2' AND \"ED_STATUS_FLAG\" IN (1) AND  \"FD_FEEDER_CODE\"=\"ED_FEEDERCODE\" AND \"FD_FEEDER_ID\"=\"FDO_FEEDER_ID\" ";
                //    if (sFeederCode != "")
                //    {
                //        strQry += " AND \"ED_FEEDERCODE\" Like '" + sFeederCode + "%' ";
                //    }
                //    strQry += "AND cast(\"ED_OFFICECODE\" as text) LIKE '" + sOfficeCode + "%'";

                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        strQry += " AND TO_CHAR(\"ED_WELD_DATE\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }
                //    strQry += "group by \"DIV\" ,\"SUB_DIV\" ,\"SECTION\" ";

                //    strQry += "UNION ALL  select 'TOTAL' \"DIV\",'' as \"SUB_DIV\",'' as \"SECTION\",count(distinct \"DTE_TC_CODE\") FROM \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\",\"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"ED_ID\"= \"DTE_ED_ID\" ";
                //    strQry += " and \"ED_LOCTYPE\"='2' AND \"ED_STATUS_FLAG\" IN (1) AND  \"FD_FEEDER_CODE\"=\"ED_FEEDERCODE\" AND \"FD_FEEDER_ID\"=\"FDO_FEEDER_ID\" ";
                //    if (sFeederCode != "")
                //    {
                //        strQry += " AND \"ED_FEEDERCODE\" Like '" + sFeederCode + "%' ";
                //    }
                //    strQry += "AND cast(\"ED_OFFICECODE\" as text) LIKE '" + sOfficeCode + "%'";

                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        strQry += " AND TO_CHAR(\"ED_WELD_DATE\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }
                //}
                //else if (sdatewise == "3")
                //{
                //    strQry = "select  (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE cast(\"DT_OM_SLNO\" as text)=cast(\"OFF_CODE\" as text)) \"DIV\",(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + SubDiv_code + "')=cast(\"SD_SUBDIV_CODE\" as text)) \"SUB_DIV\",(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + Section_code + "')=cast(\"OM_CODE\" as text)) \"SECTION\" ,count(distinct \"DT_CODE\") \"DTE_TC_CODE\"";
                //    strQry += " FROM \"TBLDTCMAST\",\"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" WHERE \"DT_FDRSLNO\"=\"FD_FEEDER_CODE\" AND ";
                //    strQry += "\"FD_FEEDER_ID\"=\"FDO_FEEDER_ID\" AND \"DT_STATUS\"='1' AND cast(\"DT_OM_SLNO\" as text) LIKE '" + sOfficeCode + "%' ";
                //    if (sFeederCode != "")
                //    {
                //        strQry += " AND  CAST(\"DT_FDRSLNO\" AS TEXT) Like '" + sFeederCode + "%' ";
                //    }
                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        strQry += " AND TO_CHAR(\"DT_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }
                //    strQry += "group by \"DIV\" ,\"SUB_DIV\" ,\"SECTION\" ";

                //    strQry += "UNION ALL  select 'TOTAL' \"DIV\",'' as \"SUB_DIV\",'' as \"SECTION\",count(distinct \"DTE_TC_CODE\")  FROM \"TBLDTCMAST\",\"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" WHERE \"DT_FDRSLNO\"=\"FD_FEEDER_CODE\" AND ";
                //    strQry += "\"FD_FEEDER_ID\"=\"FDO_FEEDER_ID\" AND \"DT_STATUS\"='1' AND cast(\"DT_OM_SLNO\" as text) LIKE '" + sOfficeCode + "%' ";
                //    if (sFeederCode != "")
                //    {
                //        strQry += " AND  CAST(\"DT_FDRSLNO\" AS TEXT) Like '" + sFeederCode + "%' ";
                //    }
                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        strQry += " AND TO_CHAR(\"DT_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }

                //}
                //else if (sdatewise == "4")
                //{
                //    strQry = "   select  (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + Division_code + "')=cast(\"DIV_CODE\" as text)) \"DIV\",(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + SubDiv_code + "')=cast(\"SD_SUBDIV_CODE\" as text)) \"SUB_DIV\",(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + Section_code + "')=cast(\"OM_CODE\" as text)) \"SECTION\",count(distinct \"DTE_TC_CODE\") \"DTE_TC_CODE\" ";

                //    strQry += " FROM \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\",\"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"ED_ID\"= \"DTE_ED_ID\" ";
                //    strQry += " and \"ED_LOCTYPE\"='2' AND \"ED_STATUS_FLAG\" IN (3) AND  \"FD_FEEDER_CODE\"=\"ED_FEEDERCODE\" AND \"FD_FEEDER_ID\"=\"FDO_FEEDER_ID\" ";
                //    if (sFeederCode != "")
                //    {
                //        strQry += " AND \"ED_FEEDERCODE\" Like '" + sFeederCode + "%' ";
                //    }
                //    strQry += "AND cast(\"ED_OFFICECODE\" as text) LIKE '" + sOfficeCode + "%'";

                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        strQry += " AND TO_CHAR(\"ED_WELD_DATE\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }
                //    strQry += "group by \"DIV\" ,\"SUB_DIV\" ,\"SECTION\" ";
                //    strQry += " UNION ALL  select 'TOTAL' \"DIV\",'' as \"SUB_DIV\",'' as \"SECTION\",count(distinct \"DTE_TC_CODE\")  FROM \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\",\"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"ED_ID\"= \"DTE_ED_ID\" ";
                //    strQry += " and \"ED_LOCTYPE\"='2' AND \"ED_STATUS_FLAG\" IN (3) AND  \"FD_FEEDER_CODE\"=\"ED_FEEDERCODE\" AND \"FD_FEEDER_ID\"=\"FDO_FEEDER_ID\" ";
                //    if (sFeederCode != "")
                //    {
                //        strQry += " AND \"ED_FEEDERCODE\" Like '" + sFeederCode + "%' ";
                //    }
                //    strQry += "AND cast(\"ED_OFFICECODE\" as text) LIKE '" + sOfficeCode + "%'";

                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        strQry += " AND TO_CHAR(\"ED_WELD_DATE\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }
                //}
                //else if (sdatewise == "5")
                //{
                //    strQry = "   select  (SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + Division_code + "')=cast(\"DIV_CODE\" as text)) \"DIV\",(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + SubDiv_code + "')=cast(\"SD_SUBDIV_CODE\" as text)) \"SUB_DIV\",(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE SUBSTR(cast(\"ED_OFFICECODE\" as text),1,'" + Section_code + "')=cast(\"OM_CODE\" as text)) \"SECTION\",count(distinct \"DTE_TC_CODE\") \"DTE_TC_CODE\" ";

                //    strQry += " FROM \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\",\"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"ED_ID\"= \"DTE_ED_ID\" ";
                //    strQry += " and \"ED_LOCTYPE\"='2' AND \"ED_STATUS_FLAG\" IN (0,1,2,3) AND  \"FD_FEEDER_CODE\"=\"ED_FEEDERCODE\" AND \"FD_FEEDER_ID\"=\"FDO_FEEDER_ID\" ";
                //    if (sFeederCode != "")
                //    {
                //        strQry += " AND \"ED_FEEDERCODE\" Like '" + sFeederCode + "%' ";
                //    }
                //    strQry += "AND cast(\"ED_OFFICECODE\" as text) LIKE '" + sOfficeCode + "%'";

                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        strQry += " AND TO_CHAR(\"ED_WELD_DATE\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }
                //    strQry += "group by \"DIV\" ,\"SUB_DIV\" ,\"SECTION\" ";
                //    strQry += " UNION ALL  select 'TOTAL' \"DIV\",'' as \"SUB_DIV\",'' as \"SECTION\",count(distinct \"DTE_TC_CODE\")  FROM \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\",\"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"ED_ID\"= \"DTE_ED_ID\" ";
                //    strQry += " and \"ED_LOCTYPE\"='2' AND \"ED_STATUS_FLAG\" IN (0,1,2,3) AND  \"FD_FEEDER_CODE\"=\"ED_FEEDERCODE\" AND \"FD_FEEDER_ID\"=\"FDO_FEEDER_ID\" ";
                //    if (sFeederCode != "")
                //    {
                //        strQry += " AND \"ED_FEEDERCODE\" Like '" + sFeederCode + "%' ";
                //    }
                //    strQry += "AND cast(\"ED_OFFICECODE\" as text) LIKE '" + sOfficeCode + "%'";

                //    if (sFromEnumDate != "" && sToEnumDate != "")
                //    {
                //        DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //        strQry += " AND TO_CHAR(\"ED_WELD_DATE\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //    }
                //}
                //dtStoreDetails = ObjCon.FetchDataTable(strQry);
                //return dtStoreDetails;
                #endregion
                string fromdate = string.Empty;
                string todate = string.Empty;

                if (sFromEnumDate != "" && sToEnumDate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    fromdate = dFromDate.ToString("yyyyMMdd");
                    todate = dToDate.ToString("yyyyMMdd");

                }

                NpgsqlCommand cmd = new NpgsqlCommand("sp_printfielddetailsabstractcount_clsreports");
                cmd.Parameters.AddWithValue("sdatewise", Convert.ToString(sdatewise));
                cmd.Parameters.AddWithValue("offcode", Convert.ToString(sOfficeCode));
                cmd.Parameters.AddWithValue("divcode", Convert.ToString(Division_code));
                cmd.Parameters.AddWithValue("subdivcode", Convert.ToString(SubDiv_code));
                cmd.Parameters.AddWithValue("sectioncode", Convert.ToString(Section_code));
                cmd.Parameters.AddWithValue("sfromenumdate", Convert.ToString(sFromEnumDate));
                cmd.Parameters.AddWithValue("stoenumdate", Convert.ToString(sToEnumDate));
                cmd.Parameters.AddWithValue("sfeedercode", Convert.ToString(sFeederCode));
                dtfieldabstractcount = ObjCon.FetchDataTable(cmd);
                return dtfieldabstractcount;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtfieldabstractcount;

            }
        }

        //public DataTable Getcapacitywise(clsReports objReport)
        //{
        //    DataTable dt = new DataTable();
        //    string strQry = string.Empty;

        //    try
        //    {

        //        string[] strArray = new string[3];

        //        //  int Length = 0;
        //        //objReport.sOfficeCode = "";
        //        /// objReport.sFromDate = "";
        //        //objReport.sTodate = "";


        //        NpgsqlCommand cmd = new NpgsqlCommand("proc_get_capacitywise_details");
        //        cmd.Parameters.AddWithValue("officecode", objReport.sOfficeCode);
        //        cmd.Parameters.AddWithValue("fromdate", objReport.sFromDate);
        //        cmd.Parameters.AddWithValue("todate", objReport.sTodate);

        //        dt = ObjCon.FetchDataTable(cmd);

        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return dt;
        //    }
        //}

        public DataTable Getcapacitywiseinstalled(clsReports objReport)
        {
            DataTable dt = new DataTable();
            try
            {
                //string strQry = string.Empty;
                //string[] strArray = new string[3];

                //  int Length = 0;
                // objReport.sOfficeCode = "";
                // objReport.sFromDate = "";
                //objReport.sTodate = "";

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_capacitywiseinstalled_details");
                cmd.Parameters.AddWithValue("officecode", objReport.sOfficeCode);
                cmd.Parameters.AddWithValue("fromdate", objReport.sFromDate);
                cmd.Parameters.AddWithValue("todate", objReport.sTodate);
                dt = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
            return dt;
        }
        #region Operational Reports
        public DataTable GetDistrcitWiseFailureOb(clsReports objrep)
        {
            DataTable dtDistrcitWiseFailureOb = new DataTable();

            try
            {
                // strMonth format  : 08-2019 (MM-yyyy)
                NpgsqlCommand cmd = new NpgsqlCommand("proc_getdistrictwisefailureob");
                cmd.Parameters.AddWithValue("selectedmonth", objrep.sCurrentMonth);
                cmd.Parameters.AddWithValue("startFinancialYear", objrep.sFromDate);
                dtDistrcitWiseFailureOb = ObjCon.FetchDataTable(cmd);

                return dtDistrcitWiseFailureOb;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtDistrcitWiseFailureOb;
            }
        }

        #endregion


        public DataTable Getcapacitywiseadded(clsReports objReport)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;

            try
            {

                string[] strArray = new string[3];



                strQry = " SELECT \"SM_NAME\",CAST(\"10\" AS INT) a,CAST(\"15\" AS INT) b,CAST(\"25\" AS INT) c,CAST(\"50\" AS INT) d,CAST(\"63\" AS INT) e,CAST(\"100\" AS INT) f,CAST(\"125\" AS INT) g,CAST(\"150\" AS INT) ";
                strQry += "  h,CAST(\"160\" AS INT)  i,CAST(\"200\" AS INT)  j,CAST(\"250\" AS INT)  k,CAST(\"300\" AS INT)  l,CAST(\"315\" AS INT)  m,CAST(\"400\" AS INT)  n,  CAST(\"500\" AS INT)  o,CAST(\"630\" AS INT) ";
                strQry += " p,CAST(\"750\" AS INT)  q,CAST(\"1000\" AS INT)  r,CAST(\"1250\" AS INT)  s, CAST(\"10\" AS INT) + CAST(\"15\" AS INT) + CAST(\"25\" AS INT) + CAST(\"50\" AS INT) + CAST(\"63\" AS INT) + CAST(\"100\" AS INT) + CAST(\"125\" AS INT)";
                strQry += "  + CAST(\"150\" AS INT) + CAST(\"160\" AS INT) + CAST(\"200\" AS INT) + CAST(\"250\" AS INT) + CAST(\"300\" AS INT) + CAST(\"315\" AS INT) + CAST(\"400\" AS INT) + CAST(\"500\" AS INT) + CAST(\"630\" AS INT) + CAST(\"750\" AS INT) + CAST(\"1000\" AS INT)";
                strQry += " + CAST(\"1250\" AS INT) \"TOTAL\" from crosstab('SELECT DISTINCT \"SM_NAME\",  CAST(\"MD_NAME\" AS INT),CAST(COALESCE(\"NO OF TC\",0) AS VARCHAR) AS \"NO OF TC\"  from (SELECT \"MD_NAME\",\"SM_NAME\",\"SM_ID\" FROM \"TBLMASTERDATA\",\"TBLSTOREMAST\"";
                strQry += "   WHERE \"MD_TYPE\" = ''C'' ";
                if (objReport.sOfficeCode != "" && objReport.sOfficeCode != "--Select--")
                {
                    strQry += " and \"SM_ID\"=''" + objReport.sOfficeCode + "''";


                }


                strQry += "    )a LEFT JOIN  (SELECT \"TC_LOCATION_ID\" , CAST(\"TC_CAPACITY\" AS TEXT) AS \"TC_CAPACITY\" ,COUNT(\"TC_CODE\") \"NO OF TC\" FROM \"TBLTCMASTER\"  WHERE     \"TC_CAPACITY\"<>0 and \"TC_DI_NO\" is not null ";

                if (objReport.sOfficeCode != "" && objReport.sOfficeCode != "--Select--")
                {


                    strQry += " AND \"TC_STORE_ID\"=''" + objReport.sOfficeCode + "''";
                }

                if (objReport.sTodate == "" && (objReport.sFromDate != ""))
                {
                    strQry += " AND TO_CHAR(\"TC_CRON\",''YYYY/MM/DD'')>= ''" + objReport.sFromDate + "'' and TO_CHAR(\"TC_CRON\",''YYYY/MM/DD'')<=TO_CHAR(CURRENT_DATE,''YYYY/MM/DD'')";

                }
                if (objReport.sFromDate == "" && (objReport.sTodate != ""))
                {
                    strQry += "AND TO_CHAR(\"TC_CRON\",''YYYY/MM/DD'')<=''" + objReport.sTodate + "'' ";
                }
                if (objReport.sFromDate == "" && objReport.sTodate == "")
                {
                    strQry += " AND TO_CHAR(\"TC_CRON\",''YYYY/MM/DD'')<=TO_CHAR(CURRENT_DATE,''YYYY/MM/DD'') ";
                }
                if (objReport.sFromDate != "" && objReport.sTodate != "")
                {
                    strQry += "AND TO_CHAR(\"TC_CRON\",''YYYY/MM/DD'')>= ''" + objReport.sFromDate + "'' AND TO_CHAR(\"TC_CRON\",''YYYY/MM/DD'')<=''" + objReport.sTodate + "''  ";
                }


                strQry += "   GROUP BY \"TC_LOCATION_ID\",\"TC_CAPACITY\")B ON \"MD_NAME\" = CAST(\"TC_CAPACITY\" AS TEXT)  AND \"TC_LOCATION_ID\"=\"SM_ID\"     ORDER BY 1,2')  ";
                strQry += "  as (\"SM_NAME\"  VARCHAR, \"10\" VARCHAR, \"15\" VARCHAR, \"25\" VARCHAR, \"50\" VARCHAR, \"63\" VARCHAR,  \"100\" VARCHAR,  \"125\" VARCHAR, \"150\" VARCHAR, \"160\" VARCHAR, \"200\" VARCHAR, \"250\" VARCHAR,  \"300\" VARCHAR, \"315\" VARCHAR,  ";
                strQry += " \"400\" VARCHAR, \"500\" VARCHAR, \"630\" VARCHAR, \"750\" VARCHAR,  \"1000\" VARCHAR, \"1250\" VARCHAR) ";
                //NpgsqlCommand cmd = new NpgsqlCommand("proc_get_capacitywiseadded_details");
                //cmd.Parameters.AddWithValue("officecode", objReport.sOfficeCode);
                //cmd.Parameters.AddWithValue("fromdate", objReport.sFromDate);
                //cmd.Parameters.AddWithValue("todate", objReport.sTodate);

                dt = ObjCon.FetchDataTable(strQry);

                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }



        public DataTable Getcapacitywisefailure(clsReports objReport)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;

            try
            {

                string[] strArray = new string[3];

                //  int Length = 0;
                // objReport.sOfficeCode = "";
                // objReport.sFromDate = "";
                //objReport.sTodate = "";

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_capacitywisefailed_details");
                //NpgsqlCommand cmd = new NpgsqlCommand("proc_get_capacitywisefailed_details");
                cmd.Parameters.AddWithValue("officecode", objReport.sOfficeCode);
                cmd.Parameters.AddWithValue("fromdate", objReport.sFromDate);
                cmd.Parameters.AddWithValue("todate", objReport.sTodate);

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
        /// This Method used to get Store Stock status details 
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable GetBufferStock(clsReports objReport)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;

            try
            {

                //strQry = " SELECT * FROM( SELECT DISTINCT COUNT(\"TC_CAPACITY\") AS \"TC_CAPACITY\", \"SM_NAME\" , \"MD_NAME\" \"DCR_RANGE\" ,  ";
                //strQry += " (SELECT \"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\"   WHERE ";
                //strQry += "    CAST(\"CM_CIRCLE_CODE\" AS TEXT) = SUBSTR(CAST(\"STO_OFF_CODE\" AS VARCHAR),1,2)) \"CIRCLE\" ";
                //strQry += "    FROM (	SELECT \"SM_ID\" ,\"SM_NAME\", \"STO_OFF_CODE\"  ,\"MD_NAME\"  ";
                //strQry += "    FROM (SELECT  \"SM_ID\", \"SM_NAME\" , \"STO_OFF_CODE\"   FROM \"TBLSTOREMAST\" INNER join ";
                //strQry += "    \"TBLSTOREOFFCODE\"  on  \"SM_ID\" = \"STO_SM_ID\" ) A , \"TBLMASTERDATA\" where ";
                //strQry += "    \"MD_TYPE\"='C'  ORDER BY \"STO_OFF_CODE\" , cast(\"MD_NAME\" as int4) )  \"A\"  ";
                //strQry += "    LEFT JOIN  (SELECT CAST(\"TC_CAPACITY\" AS TEXT) \"TC_CAPACITY\" , \"TC_LOCATION_ID\"  ";
                //strQry += "     FROM \"TBLTCMASTER\" WHERE    \"TC_STATUS\" IN(1,2,11)    AND \"TC_CURRENT_LOCATION\" = 1 ";
                //strQry += "    AND \"TC_CAPACITY\" IS NOT NULL ) \"B\" ON  \"A\".\"SM_ID\" = \"B\".\"TC_LOCATION_ID\" AND ";
                //strQry += "    \"B\".\"TC_CAPACITY\" = \"A\".\"MD_NAME\"  GROUP BY \"SM_NAME\",\"MD_NAME\" ,\"STO_OFF_CODE\" ";
                //strQry += "      ORDER BY \"SM_NAME\" )A WHERE \"TC_CAPACITY\">0  ORDER BY cast(A.\"DCR_RANGE\" as int4)";



                NpgsqlCommand cmd = new NpgsqlCommand("sp_load_allstorestockcounts");

                dt = ObjCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }


        public DataTable GetBankStock(clsReports objReport)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;

            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_bankstockdetails");

                //strQry = " SELECT DISTINCT COUNT(\"TC_CAPACITY\") AS \"TC_CAPACITY\", \"BM_NAME\" , \"DCR_RANGE\" , ";
                //strQry += "  (SELECT \"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE CAST(\"CM_CIRCLE_CODE\" AS TEXT) = SUBSTR(CAST(\"BOC_BM_SUBDIV_CODE\" AS VARCHAR),1,2)) \"CIRCLE\"  FROM (SELECT \"BM_ID\" ,\"BM_NAME\", ";
                //strQry += "  \"BOC_BM_SUBDIV_CODE\" , \"DCR_CAPACITY\" , \"DCR_RANGE\"   FROM (SELECT  \"BM_ID\", \"BM_NAME\" , \"BOC_BM_SUBDIV_CODE\"   FROM \"TBLBANKMAST\" INNER join ";
                //strQry += " \"TBLBANKOFFICECODE\" on  \"BM_ID\" = \"BOC_BM_ID\" ) A , \"TBLDTRCAPACITYRANGE\"  ORDER BY \"BOC_BM_SUBDIV_CODE\" ,\"DCR_CAPACITY\" , \"DCR_RANGE\" )  \"A\" LEFT JOIN";
                //strQry += "  (SELECT CAST(\"TC_CAPACITY\" AS TEXT) \"TC_CAPACITY\" , \"TC_LOCATION_ID\"  FROM \"TBLTCMASTER\" WHERE    \"TC_STATUS\" IN(1,2,11) AND \"TC_CURRENT_LOCATION\" = 5  AND \"TC_CAPACITY\" IS NOT NULL )";
                //strQry += "  \"B\" ON  \"A\".\"BM_ID\" = \"B\".\"TC_LOCATION_ID\" AND \"B\".\"TC_CAPACITY\" = \"A\".\"DCR_CAPACITY\"  GROUP BY \"BM_NAME\" , \"DCR_RANGE\" ,\"BOC_BM_SUBDIV_CODE\" ORDER BY \"BM_NAME\" , \"DCR_RANGE\" ";



                //working inline query
                //strQry = " SELECT * FROM (SELECT DISTINCT COUNT(\"TC_CAPACITY\") AS \"TC_CAPACITY\", \"BM_NAME\" , \"MD_NAME\" \"DCR_RANGE\",  ";
                //strQry += "    (SELECT \"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE CAST(\"CM_CIRCLE_CODE\" AS TEXT) = ";
                //strQry += "   SUBSTR(CAST(\"BOC_BM_SUBDIV_CODE\" AS VARCHAR),1,2)) \"CIRCLE\"  FROM (SELECT \"BM_ID\" ,\"BM_NAME\", ";
                //strQry += "   \"BOC_BM_SUBDIV_CODE\" ,\"MD_NAME\"FROM (SELECT  \"BM_ID\", \"BM_NAME\" , \"BOC_BM_SUBDIV_CODE\"  ";
                //strQry += "  FROM \"TBLBANKMAST\" INNER join  \"TBLBANKOFFICECODE\" on  \"BM_ID\" = \"BOC_BM_ID\" ) A , ";
                //strQry += "    \"TBLMASTERDATA\" where \"MD_TYPE\"='C'  ORDER BY \"BOC_BM_SUBDIV_CODE\") \"A\" LEFT JOIN  ";
                //strQry += "  (SELECT CAST(\"TC_CAPACITY\" AS TEXT) \"TC_CAPACITY\" , \"TC_LOCATION_ID\"  FROM ";
                //strQry += "   \"TBLTCMASTER\" WHERE    \"TC_STATUS\" IN(1,2,11) AND \"TC_CURRENT_LOCATION\" = 5  AND ";
                //strQry += "    \"TC_CAPACITY\" IS NOT NULL )  \"B\" ON  \"A\".\"BM_ID\" = \"B\".\"TC_LOCATION_ID\" AND ";
                //strQry += "  \"B\".\"TC_CAPACITY\" = \"A\".\"MD_NAME\"  GROUP BY \"BM_NAME\" ,\"BOC_BM_SUBDIV_CODE\", ";
                //strQry += " \"MD_NAME\" ORDER BY \"BM_NAME\" )A WHERE \"TC_CAPACITY\">0 ";


                dt = ObjCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }


        public DataTable Getfailureob(clsReports objReport)
        {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable finaldt = new DataTable();
            try
            {
                string[] strArray = new string[0];
                DataTable dt3 = new DataTable();
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_failedob_details");
                cmd.Parameters.AddWithValue("officecode", objReport.sOfficeCode);
                cmd.Parameters.AddWithValue("month", objReport.sFromDate);
                dt3 = ObjCon.FetchDataTable(cmd);

                //string strQry;
                #region 
                //finaldt = dt.Copy();
                //finaldt.Clear();

                //var obCountRows = (from failureReplacement in dt.AsEnumerable()
                //                   where failureReplacement.Field<string>("STATUS").Equals("OBCOUNT")
                //                   where Convert.ToDateTime(failureReplacement.Field<string>("DF_DATE")) < Convert.ToDateTime(objReport.sFromDate)
                //                   where (Convert.ToDateTime(failureReplacement.Field<string>("IN_DATE")) >= Convert.ToDateTime(objReport.sFromDate) || failureReplacement.Field<string>("IN_DATE") == null)
                //                   where (Convert.ToDateTime(failureReplacement.Field<string>("RD_RECEIVE_DATE")) >= Convert.ToDateTime(objReport.sFromDate) || failureReplacement.Field<string>("RD_RECEIVE_DATE") == null)
                //                   orderby failureReplacement.Field<string>("DIVISION_NAME").ToString()
                //                   select failureReplacement);

                //if (obCountRows.Any())
                //{
                //    finaldt = obCountRows.CopyToDataTable<DataRow>();
                //}

                //var tobeReplacedRows = (from failureReplacement in dt.AsEnumerable()
                //                        where failureReplacement.Field<string>("STATUS").Equals("TOBEREPLACED")
                //                        where Convert.ToDateTime(failureReplacement.Field<string>("DF_DATE")) <= Convert.ToDateTime(objReport.sFromDate)
                //                        where ((failureReplacement.Field<string>("IN_DATE") == null) || (Convert.ToDateTime(failureReplacement.Field<string>("IN_DATE")) > Convert.ToDateTime(objReport.sFromDate)))
                //                        where ((failureReplacement.Field<string>("RD_RECEIVE_DATE") == null) || (Convert.ToDateTime(failureReplacement.Field<string>("RD_RECEIVE_DATE")) > Convert.ToDateTime(objReport.sFromDate)))
                //                        orderby failureReplacement.Field<string>("DIVISION_NAME").ToString()
                //                        select failureReplacement);

                //if (tobeReplacedRows.Any())
                //{
                //    if (finaldt.Rows.Count > 0)
                //    {
                //        finaldt.Merge(tobeReplacedRows.CopyToDataTable<DataRow>());
                //    }
                //    else
                //    {
                //        finaldt = tobeReplacedRows.CopyToDataTable<DataRow>();
                //    }
                //}

                //var failedDuringThisMonth = (from failureReplacement in dt.AsEnumerable()
                //                             where failureReplacement.Field<string>("STATUS").Equals("FAILED")
                //                             where Convert.ToDateTime(failureReplacement.Field<string>("DF_DATE")) == Convert.ToDateTime(objReport.sFromDate)
                //                             orderby failureReplacement.Field<string>("DIVISION_NAME").ToString()
                //                             select failureReplacement);

                //if (failedDuringThisMonth.Any())
                //{
                //    if (finaldt.Rows.Count > 0)
                //    {
                //        finaldt.Merge(failedDuringThisMonth.CopyToDataTable<DataRow>());
                //    }
                //    else
                //    {
                //        finaldt = failedDuringThisMonth.CopyToDataTable<DataRow>();
                //    }
                //}

                //var replacedDuringThisMonth = (from failureReplacement in dt.AsEnumerable()
                //                               where failureReplacement.Field<string>("STATUS").Equals("REPLACED")
                //                               where Convert.ToDateTime(failureReplacement.Field<string>("IN_DATE")) == Convert.ToDateTime(objReport.sFromDate) || Convert.ToDateTime(failureReplacement.Field<string>("RD_RECEIVE_DATE")) == Convert.ToDateTime(objReport.sFromDate)
                //                               //where Convert.ToDateTime(failureReplacement.Field<string>("RD_RECEIVE_DATE")) == Convert.ToDateTime(objReport.sFromDate)
                //                               orderby failureReplacement.Field<string>("DIVISION_NAME").ToString()
                //                               select failureReplacement);

                //if (replacedDuringThisMonth.Any())
                //{
                //    if (finaldt.Rows.Count > 0)
                //    {
                //        finaldt.Merge(replacedDuringThisMonth.CopyToDataTable<DataRow>());
                //    }
                //    else
                //    {
                //        finaldt = replacedDuringThisMonth.CopyToDataTable<DataRow>();
                //    }
                //}

                //// cannot assign to null so assigned to some varible.
                //var finalDataRow = replacedDuringThisMonth;

                //finalDataRow = (from failureReplacement in finaldt.AsEnumerable()
                //                    // where objReport.sGuarantyTypes.Contains(failureReplacement.Field<string>("GUARANTY_TYPE"))
                //                where failureReplacement.Field<string>("DIVISION_CODE").ToString().StartsWith(objReport.sOfficeCode)
                //                orderby failureReplacement.Field<string>("DIVISION_NAME").ToString()
                //                select failureReplacement);
                //dt1 = finalDataRow.CopyToDataTable<DataRow>();

                //var groupbyDCR = from failureReplacement in dt1.AsEnumerable()
                //                 group failureReplacement by new
                //                 {
                //                     //DCRRange = failureReplacement.Field<string>("DCR_RANGE"),
                //                     Status = failureReplacement.Field<string>("STATUS"),
                //                     zone = failureReplacement.Field<string>("ZONE_NAME"),
                //                     circle = failureReplacement.Field<string>("CIRCLE_NAME"),
                //                     division = failureReplacement.Field<string>("DIVISION_NAME"),


                //                     //  guarantyType = failureReplacement.Field<string>("GUARANTY_TYPE")
                //                 } into g
                //                 orderby g.Key.division

                //                 select new
                //                 {
                //                     //DCRRange = g.Key.DCRRange,
                //                     Status = g.Key.Status,
                //                     zone = g.Key.zone,
                //                     circle = g.Key.circle,
                //                     division = g.Key.division,

                //                     //guarantyType = g.Key.guarantyType,
                //                     TotalMark = g.Count()
                //                 };
                //// dt3.Columns.Add("DCR_RANGE");
                //dt3.Columns.Add("STATUS");
                //dt3.Columns.Add("ZONE_NAME");
                //dt3.Columns.Add("CIRCLE_NAME");

                //dt3.Columns.Add("DIVISION_NAME");
                //// dt3.Columns.Add("DIVISION_CODE");
                ////  dt3.Columns.Add("GUARANTY_TYPE");
                //dt3.Columns.Add("TOTAL");
                ////dt3.Columns.Add("SELECTED_GUARANTY_TYPE");
                //dt3.Columns.Add("SELECTED_MONTH");

                //dt3.Columns[4].DataType = typeof(Int32);
                //foreach (var items in groupbyDCR)
                //{
                //    dt3.Rows.Add(items.Status, items.zone, items.circle, items.division, items.TotalMark, objReport.sFromDate);
                //}
                //DataTable dt3Clone = dt3.Clone();
                //dt3Clone.Columns[4].DataType = typeof(Int32);
                #endregion

                #region Old inline query
                //strQry = "SELECT \"STATUS\" ,'' AS \"ZONE_NAME\" , '' AS \"CIRCLE_NAME\", '' AS \"DIVISION_NAME\"  ,'0' AS \"TOTAL\" ,'" + objReport.sFromDate + "' \"SELECTED_MONTH\" FROM ";
                //strQry += " (SELECT * FROM  ((SELECT 'OBCOUNT' \"STATUS\" )   union all ";
                //strQry += " (SELECT 'REPLACED' \"STATUS\" )  union all  ";
                //strQry += " (SELECT 'FAILED' \"STATUS\" )    union all  ";
                //strQry += " (SELECT 'TOBEREPLACED' \"STATUS\"))d) B ";

                //// strQry += " )e ";
                //// strQry += " ) C ";
                //dt3.Merge(ObjCon.FetchDataTable(strQry));
                #endregion

                NpgsqlCommand failedobDef_details = new NpgsqlCommand("proc_get_failedob_def_details");
                failedobDef_details.Parameters.AddWithValue("p_fromdate", Convert.ToString(objReport.sFromDate ?? ""));
                dt3.Merge(ObjCon.FetchDataTable(failedobDef_details));

                return dt3;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable GetRepairerob(clsReports objReport)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {


                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_repairerob_details");

                cmd.Parameters.AddWithValue("officecode", objReport.sOfficeCode);
                cmd.Parameters.AddWithValue("month", objReport.sFromDate);
                dt = ObjCon.FetchDataTable(cmd);

                //        strQry = " SELECT * FROM (SELECT  \"TC_CAPACITY\",(SELECT \"DIV_CODE\" ||'-'|| \"DIV_NAME\"  FROM \"TBLDIVISION\" WHERE \"DIV_CODE\" = \"OFF_CODE\" )\"OFF_CODE\",  
                //        strQry += " (SELECT \"CM_CIRCLE_CODE\"  || '-' || \"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE SUBSTR(cast(\"OFF_CODE\" as text),1,2) = cast(\"CM_CIRCLE_CODE\" as text) )\"CIRCLE\" ,
                //        strQry += " (SELECT \"CM_CIRCLE_CODE\"  FROM \"TBLCIRCLE\" WHERE SUBSTR(cast(\"OFF_CODE\" as text),1,2) = cast(\"CM_CIRCLE_CODE\" as text)  ) \"CIRCLE_CODE\" ,  (SELECT \"DIV_CODE\"   FROM \"TBLDIVISION\" WHERE \"DIV_CODE\" = \"OFF_CODE\") \"DIVISION_CODE\"
                //       strQry += "  , \"MD_NAME\" ,\"STATUS\",\"DCR_RANGE\",\"GUARANTY_TYPE\", \"TR_NAME\" FROM (SELECT * FROM (
                //         strQry += "  SELECT COUNT(\"TC_CAPACITY\") \"TC_CAPACITY\" , \"OFF_CODE\" , \"MD_NAME\", 'OBCOUNT' AS \"STATUS\" , \"DCR_RANGE\",\"GUARANTY_TYPE\", \"TR_NAME\" FROM (SELECT * FROM (SELECT  \"OFF_CODE\", \"MD_NAME\", 'OBCOUNT' AS \"STATUS\", \"DCR_RANGE\" FROM  \"TBLMASTERDATA\",
                //         strQry += "   \"VIEW_ALL_OFFICES\", \"TBLDTRCAPACITYRANGE\" WHERE  \"MD_TYPE\"= 'C'  AND LENGTH(cast(\"OFF_CODE\" as text)) = 3 AND \"MD_NAME\"=   \"DCR_CAPACITY\")h CROSS JOIN (SELECT  * FROM  (SELECT 'WGP' \"GUARANTY_TYPE\" )j
                //     strQry += "  UNION ALL(SELECT 'AGP' \"GUARANTY_TYPE\" ))i) A  	LEFT JOIN   (SELECT \"RSM_GUARANTY_TYPE\", \"RSD_TC_CODE\", \"RSM_NEW_DIV_CODE\", \"TC_CAPACITY\", \"TR_NAME\" FROM \"TBLREPAIRSENTMASTER\" , \"TBLREPAIRSENTDETAILS\" ,\"TBLTCMASTER\" , \"TBLTRANSREPAIRER\" A  WHERE   \"TR_ID\" = \"RSM_SUPREP_ID\" AND 
                //     strQry += "  \"RSM_SUPREP_TYPE\" = 2 AND  \"RSM_ID\" = \"RSD_RSM_ID\" AND  \"RSD_TC_CODE\" = \"TC_CODE\" and TO_DATE(TO_CHAR(\"RSM_INV_DATE\",'MMyyyy'),'MMyyyy') < TO_DATE('062019','MMyyyy')  AND  (\"RSD_DELIVARY_DATE\" is NULL or TO_DATE(TO_CHAR(\"RSD_DELIVARY_DATE\",'MMyyyy'),'MMyyyy') >= TO_DATE('062019','MMyyyy') ))B 
                //  strQry += "  ON B.\"RSM_NEW_DIV_CODE\" = A.\"OFF_CODE\" AND A.\"MD_NAME\" = cast(B.\"TC_CAPACITY\" as VARCHAR) AND  \"GUARANTY_TYPE\" = B.\"RSM_GUARANTY_TYPE\"  GROUP BY \"MD_NAME\",\"DCR_RANGE\",\"STATUS\", \"OFF_CODE\",\"GUARANTY_TYPE\" ,\"TR_NAME\"  ORDER BY \"OFF_CODE\", \"DCR_RANGE\" )A 
                //     strQry += "  UNION ALL  SELECT * FROM(SELECT COUNT(\"TC_CAPACITY\") \"TC_CAPACITY\" , \"OFF_CODE\" , \"MD_NAME\", 'ISSUED' AS \"STATUS\" , \"DCR_RANGE\" ,\"GUARANTY_TYPE\", \"TR_NAME\" FROM (SELECT   * FROM (SELECT  \"OFF_CODE\" , \"MD_NAME\", 'ISSUED' AS \"STATUS\" , \"DCR_RANGE\" FROM \"TBLMASTERDATA\" , 
                //     strQry += "  \"VIEW_ALL_OFFICES\" , \"TBLDTRCAPACITYRANGE\" WHERE  \"MD_TYPE\"  = 'C'  AND LENGTH(cast(\"OFF_CODE\" as text)) = 3 AND \"MD_NAME\"  = \"DCR_CAPACITY\" )m CROSS JOIN (SELECT * FROM  (SELECT 'WGP' \"GUARANTY_TYPE\")l UNION ALL  (SELECT 'AGP' \"GUARANTY_TYPE\" ))j) A LEFT JOIN   (SELECT \"RSM_GUARANTY_TYPE\" ,
                //  strQry += "  \"RSD_TC_CODE\", \"RSM_NEW_DIV_CODE\" , \"TC_CAPACITY\" ,   \"TR_NAME\"  FROM \"TBLREPAIRSENTMASTER\" , \"TBLREPAIRSENTDETAILS\" ,  \"TBLTCMASTER\" ,\"TBLTRANSREPAIRER\" A WHERE   \"TR_ID\" = \"RSM_SUPREP_ID\" AND \"RSM_SUPREP_TYPE\" = 2 AND  \"RSM_ID\" = \"RSD_RSM_ID\" AND  \"RSD_TC_CODE\" = \"TC_CODE\" and 
                //     strQry += "  TO_DATE(TO_CHAR(\"RSM_INV_DATE\",'MMyyyy'),'MMyyyy') = TO_DATE('062019','MMyyyy') )B   ON B.\"RSM_NEW_DIV_CODE\" = A.\"OFF_CODE\" AND A.\"MD_NAME\" = cast(B.\"TC_CAPACITY\" as VARCHAR) and A.\"GUARANTY_TYPE\" = B.\"RSM_GUARANTY_TYPE\"  GROUP BY \"MD_NAME\",\"DCR_RANGE\",\"STATUS\", \"OFF_CODE\",\"GUARANTY_TYPE\" ,
                //     strQry += "  \"TR_NAME\" ORDER BY \"OFF_CODE\",  \"DCR_RANGE\")B  
                // strQry += " UNION ALL  SELECT * FROM(SELECT COUNT(\"TC_CAPACITY\") \"TC_CAPACITY\" , \"OFF_CODE\" , \"MD_NAME\", 'RECEIVED' AS \"STATUS\", \"DCR_RANGE\",\"GUARANTY_TYPE\", \"TR_NAME\" FROM (  SELECT * FROM (SELECT  \"OFF_CODE\", \"MD_NAME\", 'ISSUED' AS \"STATUS\" , \"DCR_RANGE\" FROM \"TBLMASTERDATA\" ,
                //strQry += " \"VIEW_ALL_OFFICES\" , \"TBLDTRCAPACITYRANGE\" WHERE  \"MD_TYPE\" = 'C'  AND LENGTH(cast(\"OFF_CODE\"as text)) = 3 AND \"MD_NAME\" = \"DCR_CAPACITY\")j CROSS JOIN (SELECT * FROM  (SELECT 'WGP'  \"GUARANTY_TYPE\") i
                //strQry += " UNION ALL(SELECT 'AGP' \"GUARANTY_TYPE\"))k) A   LEFT JOIN  (SELECT \"RSM_GUARANTY_TYPE\" , \"RSD_TC_CODE\" , \"RSM_NEW_DIV_CODE\", \"TC_CAPACITY\", \"TR_NAME\"  FROM \"TBLREPAIRSENTMASTER\" , \"TBLREPAIRSENTDETAILS\" ,\"TBLTCMASTER\",  \"TBLTRANSREPAIRER\" A  WHERE \"TR_ID\"= \"RSM_SUPREP_ID\"AND \"RSM_SUPREP_TYPE\"= 2 AND 
                //strQry += " \"RSM_ID\"= \"RSD_RSM_ID\"AND  \"RSD_TC_CODE\"= \"TC_CODE\"  AND   TO_DATE(TO_CHAR(\"RSD_DELIVARY_DATE\",'MMyyyy'),'MMyyyy') = TO_DATE('052019','MMyyyy')  )B   ON B.\"RSM_NEW_DIV_CODE\"= A.\"OFF_CODE\" AND A.\"MD_NAME\"=cast(B.\"TC_CAPACITY\" as varchar) AND A.\"GUARANTY_TYPE\" = B.\"RSM_GUARANTY_TYPE\"  GROUP BY \"MD_NAME\",\"DCR_RANGE\",
                // strQry += " \"STATUS\", \"OFF_CODE\",\"GUARANTY_TYPE\",\"TR_NAME\" ORDER BY \"OFF_CODE\",  \"DCR_RANGE\")C   

                // strQry += " UNION ALL
                //strQry += " SELECT * FROM(SELECT COUNT(\"TC_CAPACITY\") \"TC_CAPACITY\" , \"OFF_CODE\" , \"MD_NAME\", 'PENDING' AS \"STATUS\", \"DCR_RANGE\" ,\"GUARANTY_TYPE\" , \"TR_NAME\" FROM (  SELECT *  FROM (SELECT  \"OFF_CODE\", \"MD_NAME\", 'ISSUED' AS \"STATUS\" , \"DCR_RANGE\" FROM \"TBLMASTERDATA\" , \"VIEW_ALL_OFFICES\" , \"TBLDTRCAPACITYRANGE\" WHERE  \"MD_TYPE\" = 'C'  
                //strQry += " AND LENGTH(cast(\"OFF_CODE\" as varchar)) = 3 AND \"MD_NAME\"= \"DCR_CAPACITY\" )i CROSS JOIN (SELECT * FROM  (SELECT 'WGP'   \"GUARANTY_TYPE\")k UNION ALL (SELECT 'AGP' \"GUARANTY_TYPE\") )j) A  LEFT JOIN   (SELECT \"RSM_GUARANTY_TYPE\" , \"RSD_TC_CODE\" , \"RSM_NEW_DIV_CODE\"  , \"TC_CAPACITY\" ,  \"TR_NAME\"    FROM \"TBLREPAIRSENTMASTER\", \"TBLREPAIRSENTDETAILS\" ,
                // strQry += " \"TBLTCMASTER\",\"TBLTRANSREPAIRER\" A  WHERE   \"TR_ID\" = \"RSM_SUPREP_ID\" AND \"RSM_SUPREP_TYPE\" = 2 AND  \"RSM_ID\" = \"RSD_RSM_ID\" AND  \"RSD_TC_CODE\" = \"TC_CODE\"   AND   TO_DATE(TO_CHAR(\"RSM_INV_DATE\",'MMyyyy'),'MMyyyy') <= TO_DATE('062019','MMyyyy') and (\"RSD_DELIVARY_DATE\" is NULL or TO_DATE(TO_CHAR(  \"RSD_DELIVARY_DATE\",'MMyyyy'),'MMyyyy') > TO_DATE('062019','MMyyyy') ) )B 
                //strQry += " ON B.\"RSM_NEW_DIV_CODE\" = A.\"OFF_CODE\" AND A.\"MD_NAME\" = cast(B.\"TC_CAPACITY\"as varchar) AND A.\"GUARANTY_TYPE\"= B.\"RSM_GUARANTY_TYPE\" GROUP BY \"MD_NAME\",\"DCR_RANGE\",\"STATUS\", \"OFF_CODE\",\"GUARANTY_TYPE\",\"TR_NAME\" ORDER BY \"OFF_CODE\", \"DCR_RANGE\") D   ) E  ) F ";


                //  dt = ObjCon.FetchDataTable(strQry);
                return dt;






            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable getrepairerincurreddetails(clsReports objReport)
        {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable finaldt = new DataTable();

            try
            {

                string[] strArray = new string[3];


                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_repairerincurred_details");
                cmd.Parameters.AddWithValue("officecode", objReport.sOfficeCode);
                cmd.Parameters.AddWithValue("fromdate", objReport.sFromDate);
                cmd.Parameters.AddWithValue("todate", objReport.sTodate);

                dt = ObjCon.FetchDataTable(cmd);


                return dt;



            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }


        public DataTable printDtcRepairDetails(clsReports objValue)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                //strQry = " SELECT \"CM_CIRCLE_NAME\" ,\"DIV_NAME\",\"TR_NAME\",\"MRI_REPAIR_CENTER\",\"MRI_PO_NO\", \"MRI_PO_DATE\"  FROM \"TBLCIRCLE\" ";

                //strQry += "INNER JOIN \"TBLDIVISION\" on \"CM_CIRCLE_CODE\" = \"DIV_CICLE_CODE\" INNER JOIN \"TBLMINORREPAIRITEMRATEMASTER\" ON  \"DIV_ID\"=\"MRI_DIV_ID\" ";
                //strQry += " INNER JOIN \"TBLTRANSREPAIRER\" ON \"MRI_TR_ID\"=\"TR_ID\"  WHERE cast(\"DIV_CODE\" as text) like '" + objValue.sOfficeCode + "%' GROUP BY \"DIV_NAME\",\"CM_CIRCLE_NAME\" ,\"DIV_NAME\",\"TR_NAME\",\"MRI_REPAIR_CENTER\",\"MRI_PO_NO\", \"MRI_PO_DATE\" ";

                strQry = " SELECT \"CM_CIRCLE_CODE\", \"CM_CIRCLE_NAME\" ,\"DIV_NAME\",\"TR_NAME\",\"MRI_REPAIR_CENTER\",\"MRI_PO_NO\", \"MRI_PO_DATE\"  FROM \"TBLCIRCLE\" ";

                strQry += "INNER JOIN \"TBLDIVISION\" on \"CM_CIRCLE_CODE\" = \"DIV_CICLE_CODE\" INNER JOIN \"TBLMINORREPAIRITEMRATEMASTER\" ON  \"DIV_ID\"=\"MRI_DIV_ID\" ";
                strQry += " INNER JOIN \"TBLTRANSREPAIRER\" ON \"MRI_TR_ID\"=\"TR_ID\"  WHERE cast(\"DIV_CODE\" as text) like '" + objValue.sOfficeCode + "%' GROUP BY \"CM_CIRCLE_CODE\",\"DIV_NAME\",\"CM_CIRCLE_NAME\" ,\"DIV_NAME\",\"TR_NAME\",\"MRI_REPAIR_CENTER\",\"MRI_PO_NO\", \"MRI_PO_DATE\" ";

                strQry += " ORDER BY \"CM_CIRCLE_CODE\" ASC";
                dt = ObjCon.FetchDataTable(strQry);
                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }
        }


        public DataTable getSchemedetails(string previosyear, string presentyear)
        {
            DataTable dt = new DataTable();
            //string strQry = string.Empty;
            try
            {

                //strQry = " SELECT case when \"DTC_COUNT\" is null then '0' else \"DTC_COUNT\" end as  \"DTC_COUNT\",*from(SELECT TO_CHAR(generate_series(timestamp without time zone '2018-01-01', timestamp without time zone '2018-12-01', '1 Month'), 'MON') \"MONTHS\", TO_CHAR(generate_series(timestamp without time  zone '2018-01-01', timestamp without time zone '2018-12-01', '1 Month'), 'MM') \"MON\")a left join";
                //strQry += "(SELECT to_char(\"DF_DATE\",'MON') as \"MONTHS\",to_char(\"DF_DATE\",'MM') AS \"MONTH\",COUNT(*) \"DTC_COUNT\"";
                //strQry += "  ,CASE WHEN \"SCHM_NAME\" IS NOT NULL THEN \"SCHM_NAME\" else 'zOTHERS'  END \"STATUS\"  FROM \"TBLDTCFAILURE\"";
                //strQry += "LEFT JOIN \"TBLDTCMAST\" ON \"DF_DTC_CODE\"=\"DT_CODE\" LEFT JOIN \"TBLDTCSCHEME\" ON \"DT_PROJECTTYPE\"=\"SCHM_ID\" WHERE ";
                //strQry += " \"DF_STATUS_FLAG\" IN (1,4) AND  TO_CHAR(\"DF_DATE\",'YYYY-MM')>'" + previosyear + "-03-31' AND TO_CHAR(\"DF_DATE\",'YYYY-MM')<'" + presentyear + "-04-01'  ";
                //strQry += " GROUP BY \"SCHM_NAME\",\"SCHM_ID\",to_char(\"DF_DATE\",'MON'),to_char(\"DF_DATE\",'MM'))B on a.\"MON\"=B.\"MONTH\" ";

                #region old inline query
                //strQry = " SELECT case when \"DTC_COUNT\" is null then '0' else \"DTC_COUNT\" end as  \"DTC_COUNT\",* from ";
                //strQry += " (SELECT TO_CHAR(generate_series(timestamp without time zone '2018-01-01', timestamp without time zone '2018-12-01', '1 Month'), 'MON') \"MONTHS\", ";
                //strQry += " TO_CHAR(generate_series(timestamp without time  zone '2018-01-01', timestamp without time zone '2018-12-01', '1 Month'), 'MM') \"MON\")a left join ";
                //strQry += " (SELECT to_char(\"DF_DATE\",'MON') as \"MONTHS\",to_char(\"DF_DATE\",'MM') AS \"MONTH\",COUNT(*) \"DTC_COUNT\", ";
                //strQry += " CASE WHEN \"MD_NAME\" IS NOT NULL THEN \"MD_NAME\" else 'zOTHERS'  END \"STATUS\"  FROM \"TBLDTCFAILURE\" ";
                //strQry += " LEFT JOIN \"TBLDTCMAST\" ON \"DF_DTC_CODE\"=\"DT_CODE\" LEFT JOIN \"TBLMASTERDATA\" ON \"DT_PROJECTTYPE\"=\"MD_ID\" WHERE ";
                //strQry += " \"MD_TYPE\"='PT' and \"DF_STATUS_FLAG\" IN (1,4) AND TO_CHAR(\"DF_DATE\",'YYYY-MM')>'" + previosyear + "-03-31' ";
                //strQry += " AND TO_CHAR(\"DF_DATE\",'YYYY-MM')<'" + presentyear + "-04-01' ";
                //strQry += " GROUP BY \"MD_NAME\",\"MD_ID\",to_char(\"DF_DATE\",'MON'),to_char(\"DF_DATE\",'MM'))B on a.\"MON\"=B.\"MONTH\" ";
                //dt = ObjCon.FetchDataTable(strQry);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_scheme_details");
                cmd.Parameters.AddWithValue("p_previosyear", Convert.ToString(previosyear ?? ""));
                cmd.Parameters.AddWithValue("p_presentyear", Convert.ToString(presentyear ?? ""));
                dt = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
            return dt;
        }



        public DataTable getReplacedtails(string previosyear, string presentyear)
        {
            DataTable dt = new DataTable();
            //string strQry = string.Empty;
            try
            {
                #region Old Inline query
                //strQry = " SELECT 'Total No. of DTs replaced' as \"STATUS\",case when \"DTC_COUNT\" is null then '0' else \"DTC_COUNT\" end as \"DTC_COUNT\", * from ";
                //strQry += " (SELECT TO_CHAR(generate_series(timestamp without time zone '2018-01-01',  timestamp without time zone '2018-12-01', '1 Month'), 'MON') ";
                //strQry += " \"MONTHS\",TO_CHAR(generate_series(timestamp without time  zone '2018-01-01', timestamp without time zone '2018-12-01','1 Month'),'MM') \"MON\")a left join ";
                //strQry += " (SELECT to_char(\"IN_DATE\",'MON') as \"MONTHS\",to_char(\"IN_DATE\",'MM') AS \"MONTH\",count(*) \"DTC_COUNT\"   FROM \"TBLDTCINVOICE\" ";
                //strQry += " WHERE  TO_CHAR(\"IN_DATE\",'YYYY-MM')>'" + previosyear + "-03-31' ";
                //strQry += " AND TO_CHAR(\"IN_DATE\",'YYYY-MM')<'" + presentyear + "-04-01' GROUP BY to_char(\"IN_DATE\",'MON'),to_char(\"IN_DATE\",'MM') ";
                //strQry += " union all SELECT to_char(\"RD_RECEIVE_DATE\", 'MON') as \"MONTHS\",to_char(\"RD_RECEIVE_DATE\", 'MM') AS \"MONTH\", ";
                //strQry += " count(*) \"DTC_COUNT\"   FROM \"TBLRECEIVEDTR\" WHERE TO_CHAR(\"RD_RECEIVE_DATE\", 'YYYY-MM')> '" + previosyear + "-03-31' ";
                //strQry += " AND TO_CHAR(\"RD_RECEIVE_DATE\", 'YYYY-MM')<'" + presentyear + "-04-01' ";
                //strQry += " GROUP BY to_char(\"RD_RECEIVE_DATE\", 'MON'),to_char(\"RD_RECEIVE_DATE\", 'MM')  )b on a.\"MON\"=B.\"MONTH\" ";
                //dt = ObjCon.FetchDataTable(strQry);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_replace_details");
                cmd.Parameters.AddWithValue("p_previosyear", Convert.ToString(previosyear ?? ""));
                cmd.Parameters.AddWithValue("p_presentyear", Convert.ToString(presentyear ?? ""));
                dt = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
            return dt;
        }


        public DataTable getdetailsview(string previosyear, string presentyear)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = " SELECT sum(\"RSD_TC_CODE\") as \"REPAIREDS\" from( SELECT count(\"RSD_TC_CODE\") as \"RSD_TC_CODE\"   FROM \"TBLREPAIRSENTMASTER\" , \"TBLREPAIRSENTDETAILS\" ,  \"TBLTCMASTER\"  A WHERE    \"RSM_SUPREP_TYPE\" = 2 AND  \"RSM_ID\" = \"RSD_RSM_ID\" AND  \"RSD_TC_CODE\" = \"TC_CODE\"  and TO_CHAR(\"RSD_DELIVARY_DATE\",'YYYY-MM')>'" + previosyear + "-03-31' AND TO_CHAR(\"RSD_DELIVARY_DATE\",'YYYY-MM')<'" + presentyear + "-04-01'";
                strQry += " union all ";
                strQry += " SELECT count(\"DF_EQUIPMENT_ID\")  as \"DF_EQUIPMENT_ID\" from \"TBLDTCFAILURE\" left join \"TBLESTIMATIONDETAILS\" on \"DF_ID\"=\"EST_FAILUREID\" left join \"TBLWORKORDER\" on \"DF_ID\"=\"WO_DF_ID\" left join  \"TBLRECEIVEDTR\" on \"RD_WO_SLNO\"=\"WO_SLNO\" WHERE \"EST_FAIL_TYPE\"=1   and TO_CHAR(\"RD_RECEIVE_DATE\",'YYYY-MM')>'" + previosyear + "-03-31' AND TO_CHAR(\"RD_RECEIVE_DATE\",'YYYY-MM')<'" + presentyear + "-04-01')a";
                strQry += " union all ";
                strQry += "SELECT sum(\"NO OF TC\") as  \"STOCK\" from (SELECT \"MD_NAME\",\"SM_NAME\",\"SM_ID\" FROM \"TBLMASTERDATA\",\"TBLSTOREMAST\" WHERE \"MD_TYPE\" = 'C'  )a LEFT JOIN  (SELECT \"TC_LOCATION_ID\" , CAST(\"TC_CAPACITY\" AS TEXT) AS \"TC_CAPACITY\" ,COUNT(\"TC_CODE\") \"NO OF TC\" FROM \"TBLTCMASTER\"  WHERE  \"TC_STATUS\" IN(1,2)   AND \"TC_CURRENT_LOCATION\" = 1  AND \"TC_CAPACITY\" IS NOT NULL GROUP BY \"TC_LOCATION_ID\",\"TC_CAPACITY\" )B ON \"MD_NAME\" = CAST(\"TC_CAPACITY\" AS TEXT)  AND \"TC_LOCATION_ID\"=\"SM_ID\"";
                strQry += " union all ";
                strQry += "SELECT distinct count(*) as \"REPAIRER\"  from \"TBLTRANSREPAIRER\" inner join \"TBLTRANSREPAIREROFFCODE\" on \"TR_ID\"=\"TRO_TR_ID\" ";
                strQry += " union all ";
                strQry += "SELECT COUNT(\"TC_CODE\") as \"NEWTCS\"  FROM \"TBLTCMASTER\"  WHERE   \"TC_CAPACITY\"<>0 and \"TC_DI_NO\" is not null  and TO_CHAR(\"TC_CRON\",'YYYY-MM')>'" + previosyear + "-03-31' AND TO_CHAR(\"TC_CRON\",'YYYY-MM')<'" + presentyear + "-04-01' ";



                dt = ObjCon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }
        }


        public DataTable getreplacerepairview(string previosyear, string presentyear, string month)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                //ob
                strQry = " SELECT sum(\"DTR_CODE\") as \"SUM\" from (";
                strQry += "SELECT count(\"DF_EQUIPMENT_ID\")  \"DTR_CODE\"  FROM \"TBLDTCFAILURE\" left join \"TBLESTIMATIONDETAILS\" on \"EST_FAILUREID\"=\"DF_ID\" left JOIN \"TBLWORKORDER\" ON \"WO_DF_ID\" = \"DF_ID\"  left JOIN \"TBLINDENT\" ON \"WO_SLNO\" = \"TI_WO_SLNO\" left JOIN \"TBLDTCINVOICE\" ON \"TI_ID\"  = \"IN_TI_NO\" WHERE  \"DF_STATUS_FLAG\" IN (1,4)  and  TO_DATE(TO_CHAR(\"DF_DATE\",'MM-yyyy'),'MM-yyyy') < TO_DATE('" + month + "','MM-yyyy')  and \"EST_FAIL_TYPE\"=2  AND  (\"IN_DATE\" is NULL or TO_DATE(TO_CHAR(\"IN_DATE\",'MM-yyyy'),'MM-yyyy') >= TO_DATE('" + month + "','MM-yyyy') ) ";
                strQry += " union all ";
                strQry += "SELECT  count(\"DF_EQUIPMENT_ID\") \"DTR_CODE\" FROM \"TBLDTCFAILURE\" left join \"TBLESTIMATIONDETAILS\" on \"EST_FAILUREID\"=\"DF_ID\" left JOIN \"TBLWORKORDER\" ON \"WO_DF_ID\" = \"DF_ID\"  left JOIN \"TBLINDENT\" ON \"WO_SLNO\" = \"TI_WO_SLNO\" left join \"TBLRECEIVEDTR\" on \"WO_SLNO\"=\"RD_WO_SLNO\" WHERE  \"DF_STATUS_FLAG\" IN (1,4) and \"EST_FAIL_TYPE\"=1 and TO_DATE(TO_CHAR(\"DF_DATE\",'MM-yyyy'),'MM-yyyy') < TO_DATE('" + month + "','MM-yyyy')  AND  (\"RD_RECEIVE_DATE\" is NULL or TO_DATE(TO_CHAR(\"RD_RECEIVE_DATE\",'MM-yyyy'),'MM-yyyy') >= TO_DATE('" + month + "','MM-yyyy') ) )a";

                strQry += " union all ";
                //failed
                strQry += "SELECT sum(\"DTR_CODE\") FROM ";
                strQry += "(SELECT count(\"DF_EQUIPMENT_ID\") \"DTR_CODE\"  FROM \"TBLDTCFAILURE\" left join \"TBLESTIMATIONDETAILS\" on \"EST_FAILUREID\"=\"DF_ID\" left JOIN \"TBLWORKORDER\" ON \"WO_DF_ID\" = \"DF_ID\"  left JOIN \"TBLINDENT\" ON \"WO_SLNO\" = \"TI_WO_SLNO\"   left join \"TBLRECEIVEDTR\" on \"WO_SLNO\"=\"RD_WO_SLNO\"  WHERE \"DF_STATUS_FLAG\" IN (1,4) and  TO_DATE(TO_CHAR(\"DF_DATE\",'MM-yyyy'),'MM-yyyy')=TO_DATE('" + month + "','MM-yyyy') and  \"EST_FAIL_TYPE\" in(1,2) )a";



                strQry += " union all ";
                //replaced
                strQry += "SELECT sum(\"DTR_CODE\") from (";
                strQry += "SELECT count(\"DF_EQUIPMENT_ID\")  \"DTR_CODE\"  FROM \"TBLDTCFAILURE\" left join \"TBLESTIMATIONDETAILS\" on \"EST_FAILUREID\"=\"DF_ID\"  left JOIN \"TBLWORKORDER\" ON \"WO_DF_ID\" = \"DF_ID\"  left JOIN \"TBLINDENT\" ON \"WO_SLNO\" = \"TI_WO_SLNO\" left JOIN \"TBLDTCINVOICE\" ON \"TI_ID\"  = \"IN_TI_NO\"   WHERE  \"DF_STATUS_FLAG\" IN (1,4) and \"EST_FAIL_TYPE\"=2 and  TO_DATE(TO_CHAR(\"IN_DATE\",'MM-yyyy'),'MM-yyyy') = TO_DATE('" + month + "','MM-yyyy')   ";
                strQry += " union all ";
                strQry += "SELECT  count(\"DF_EQUIPMENT_ID\") \"DTR_CODE\" FROM \"TBLDTCFAILURE\" left join \"TBLESTIMATIONDETAILS\" on \"EST_FAILUREID\"=\"DF_ID\" left JOIN \"TBLWORKORDER\" ON \"WO_DF_ID\" = \"DF_ID\"  left JOIN \"TBLINDENT\" ON \"WO_SLNO\" = \"TI_WO_SLNO\"   left join \"TBLRECEIVEDTR\" on \"WO_SLNO\"=\"RD_WO_SLNO\" WHERE  \"DF_STATUS_FLAG\" IN (1,4) and \"EST_FAIL_TYPE\"=1  and TO_DATE(TO_CHAR(\"RD_RECEIVE_DATE\",'MM-yyyy'),'MM-yyyy') = TO_DATE('" + month + "','MM-yyyy')   )a";


                strQry += " union all";
                //pending

                strQry += " SELECT sum(\"DTR_CODE\") from (";
                strQry += " SELECT count(\"DF_EQUIPMENT_ID\")  \"DTR_CODE\"  FROM \"TBLDTCFAILURE\" left join \"TBLESTIMATIONDETAILS\" on \"EST_FAILUREID\"=\"DF_ID\" left JOIN \"TBLWORKORDER\" ON \"WO_DF_ID\" = \"DF_ID\"  left JOIN \"TBLINDENT\" ON \"WO_SLNO\" = \"TI_WO_SLNO\" left JOIN \"TBLDTCINVOICE\" ON \"TI_ID\"  = \"IN_TI_NO\"  WHERE  \"DF_STATUS_FLAG\" IN (1,4) and \"EST_FAIL_TYPE\"=2 AND  TO_DATE(TO_CHAR(\"DF_DATE\",'MM-yyyy'),'MM-yyyy') <= TO_DATE('" + month + "','MM-yyyy')   AND  (\"IN_DATE\" is NULL or TO_DATE(TO_CHAR(\"IN_DATE\",'MM-yyyy'),'MM-yyyy') > TO_DATE('" + month + "','MM-yyyy'))  AND CAST(\"DF_LOC_CODE\" AS TEXT) LIKE '%'   ";
                strQry += " union all ";
                strQry += "SELECT  count(\"DF_EQUIPMENT_ID\") FROM \"TBLDTCFAILURE\" left join \"TBLESTIMATIONDETAILS\" on \"EST_FAILUREID\"=\"DF_ID\" left JOIN \"TBLWORKORDER\" ON \"WO_DF_ID\" = \"DF_ID\"  left JOIN \"TBLINDENT\" ON \"WO_SLNO\" = \"TI_WO_SLNO\"  left join \"TBLRECEIVEDTR\" on \"WO_SLNO\"=\"RD_WO_SLNO\" WHERE   \"DF_STATUS_FLAG\" IN (1,4)  and \"EST_FAIL_TYPE\"=1  AND  TO_DATE(TO_CHAR(\"DF_DATE\",'MM-yyyy'),'MM-yyyy') <= TO_DATE('" + month + "','MM-yyyy') and (\"RD_RECEIVE_DATE\" is NULL or TO_DATE(TO_CHAR(\"RD_RECEIVE_DATE\",'MM-yyyy'),'MM-yyyy') > TO_DATE('" + month + "','MM-yyyy') ) AND CAST(\"DF_LOC_CODE\" AS TEXT) LIKE '%' )a";

                strQry += " union all ";
                // totalrepairer
                strQry += "SELECT count(*) from \"TBLTRANSREPAIRER\" inner join \"TBLTRANSREPAIREROFFCODE\" on \"TR_ID\"=\"TRO_TR_ID\" ";

                strQry += " union all ";
                //repairerdmonth
                strQry += "SELECT sum( \"DTR_CODE\") FROM (  SELECT count(\"RSD_TC_CODE\")  \"DTR_CODE\" FROM \"TBLREPAIRSENTMASTER\" , \"TBLREPAIRSENTDETAILS\" ,\"TBLTCMASTER\",  \"TBLTRANSREPAIRER\" A  WHERE \"TR_ID\"= \"RSM_SUPREP_ID\"AND \"RSM_SUPREP_TYPE\"= 2 AND \"RSM_ID\"= \"RSD_RSM_ID\"AND  \"RSD_TC_CODE\"= \"TC_CODE\"  AND   TO_DATE(TO_CHAR(\"RSD_DELIVARY_DATE\",'MM-yyyy'),'MM-yyyy') = TO_DATE('" + month + "','MM-yyyy') ";
                strQry += " union all ";

                strQry += "SELECT count(\"DF_EQUIPMENT_ID\")  \"DTR_CODE\" from\"TBLDTCFAILURE\"left join \"TBLESTIMATIONDETAILS\" on \"DF_ID\"=\"EST_FAILUREID\" left join \"TBLWORKORDER\" on \"DF_ID\"=\"WO_DF_ID\" left join  \"TBLRECEIVEDTR\" on \"RD_WO_SLNO\"=\"WO_SLNO\" left join \"TBLTRANSREPAIRER\" on \"EST_REPAIRER\"=\"TR_ID\" WHERE \"EST_FAIL_TYPE\"=1 and TO_DATE(TO_CHAR(\"RD_RECEIVE_DATE\",'MM-yyyy'),'MM-yyyy') = TO_DATE('" + month + "','MM-yyyy')  ";
                strQry += ")B";



                strQry += " union all ";
                // finalcialyear

                strQry += " SELECT sum( \"DTR_CODE\") FROM (  SELECT count(\"RSD_TC_CODE\")  \"DTR_CODE\" FROM \"TBLREPAIRSENTMASTER\" , \"TBLREPAIRSENTDETAILS\" ,\"TBLTCMASTER\",  \"TBLTRANSREPAIRER\" A  WHERE \"TR_ID\"= \"RSM_SUPREP_ID\"AND \"RSM_SUPREP_TYPE\"= 2 AND \"RSM_ID\"= \"RSD_RSM_ID\"AND  \"RSD_TC_CODE\"= \"TC_CODE\"  AND   TO_DATE(TO_CHAR(\"RSD_DELIVARY_DATE\",'yyyy-mm-dd'),'yyyy-mm-dd') > TO_DATE('" + previosyear + "-03-31','yyyy-mm-dd') and TO_DATE(TO_CHAR(\"RSD_DELIVARY_DATE\",'yyyy-mm-dd'),'yyyy-mm-dd') < TO_DATE('" + presentyear + "-04-01','yyyy-mm-dd')";
                strQry += " union all ";

                strQry += " SELECT count(\"DF_EQUIPMENT_ID\")  \"DTR_CODE\" from\"TBLDTCFAILURE\"left join \"TBLESTIMATIONDETAILS\" on \"DF_ID\"=\"EST_FAILUREID\" left join \"TBLWORKORDER\" on \"DF_ID\"=\"WO_DF_ID\" left join  \"TBLRECEIVEDTR\" on \"RD_WO_SLNO\"=\"WO_SLNO\" left join \"TBLTRANSREPAIRER\" on \"EST_REPAIRER\"=\"TR_ID\" WHERE \"EST_FAIL_TYPE\"=1 and  TO_DATE(TO_CHAR(\"RD_RECEIVE_DATE\",'yyyy-mm-dd'),'yyyy-mm-dd') > TO_DATE('" + previosyear + "-03-31','yyyy-mm-dd') and TO_DATE(TO_CHAR(\"RD_RECEIVE_DATE\",'yyyy-mm-dd'),'yyyy-mm-dd') < TO_DATE('" + presentyear + "-04-01','yyyy-mm-dd') ";
                strQry += " )B ";



                dt = ObjCon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }
        }


        public DataTable getreplacedetails(string previosyear, string presentyear, string month)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                //ob
                strQry = " SELECT sum(\"DTR_CODE\") as \"SUM\" from (";
                strQry += " SELECT count(\"DF_EQUIPMENT_ID\")  \"DTR_CODE\"  FROM \"TBLDTCFAILURE\" left join \"TBLESTIMATIONDETAILS\" on \"EST_FAILUREID\"=\"DF_ID\" left JOIN \"TBLWORKORDER\" ON \"WO_DF_ID\" = \"DF_ID\"  left JOIN \"TBLINDENT\" ON \"WO_SLNO\" = \"TI_WO_SLNO\" left JOIN \"TBLDTCINVOICE\" ON \"TI_ID\"  = \"IN_TI_NO\" WHERE  \"DF_STATUS_FLAG\" IN (1,4)  and  TO_DATE(TO_CHAR(\"DF_DATE\",'MM-yyyy'),'MM-yyyy') < TO_DATE('" + month + "','MM-yyyy')  and \"EST_FAIL_TYPE\"=2  AND  (\"IN_DATE\" is NULL or TO_DATE(TO_CHAR(\"IN_DATE\",'MM-yyyy'),'MM-yyyy') >= TO_DATE('" + month + "','MM-yyyy') ) ";
                strQry += " union all ";
                strQry += " SELECT  count(\"DF_EQUIPMENT_ID\") \"DTR_CODE\" FROM \"TBLDTCFAILURE\" left join \"TBLESTIMATIONDETAILS\" on \"EST_FAILUREID\"=\"DF_ID\" left JOIN \"TBLWORKORDER\" ON \"WO_DF_ID\" = \"DF_ID\"  left JOIN \"TBLINDENT\" ON \"WO_SLNO\" = \"TI_WO_SLNO\" left join \"TBLRECEIVEDTR\" on \"WO_SLNO\"=\"RD_WO_SLNO\" WHERE  \"DF_STATUS_FLAG\" IN (1,4) and \"EST_FAIL_TYPE\"=1 and TO_DATE(TO_CHAR(\"DF_DATE\",'MM-yyyy'),'MM-yyyy') < TO_DATE('" + month + "','MM-yyyy')  AND  (\"RD_RECEIVE_DATE\" is NULL or TO_DATE(TO_CHAR(\"RD_RECEIVE_DATE\",'MM-yyyy'),'MM-yyyy') >= TO_DATE('" + month + "','MM-yyyy') ) )a";

                strQry += " union all ";
                //failed
                strQry += " SELECT sum(\"DTR_CODE\") FROM ";
                strQry += " (SELECT count(\"DF_EQUIPMENT_ID\") \"DTR_CODE\"  FROM \"TBLDTCFAILURE\" left join \"TBLESTIMATIONDETAILS\" on \"EST_FAILUREID\"=\"DF_ID\" left JOIN \"TBLWORKORDER\" ON \"WO_DF_ID\" = \"DF_ID\"  left JOIN \"TBLINDENT\" ON \"WO_SLNO\" = \"TI_WO_SLNO\"   left join \"TBLRECEIVEDTR\" on \"WO_SLNO\"=\"RD_WO_SLNO\"  WHERE \"DF_STATUS_FLAG\" IN (1,4) and  TO_DATE(TO_CHAR(\"DF_DATE\",'MM-yyyy'),'MM-yyyy')=TO_DATE('" + month + "','MM-yyyy') and  \"EST_FAIL_TYPE\" in(1,2) )a";



                strQry += " union all ";
                //replaced
                strQry += " SELECT sum(\"DTR_CODE\") from (";
                strQry += " SELECT count(\"DF_EQUIPMENT_ID\")  \"DTR_CODE\"  FROM \"TBLDTCFAILURE\" left join \"TBLESTIMATIONDETAILS\" on \"EST_FAILUREID\"=\"DF_ID\"  left JOIN \"TBLWORKORDER\" ON \"WO_DF_ID\" = \"DF_ID\"  left JOIN \"TBLINDENT\" ON \"WO_SLNO\" = \"TI_WO_SLNO\" left JOIN \"TBLDTCINVOICE\" ON \"TI_ID\"  = \"IN_TI_NO\"   WHERE  \"DF_STATUS_FLAG\" IN (1,4) and \"EST_FAIL_TYPE\"=2 and  TO_DATE(TO_CHAR(\"IN_DATE\",'MM-yyyy'),'MM-yyyy') = TO_DATE('" + month + "','MM-yyyy')   ";
                strQry += " union all";
                strQry += " SELECT  count(\"DF_EQUIPMENT_ID\") \"DTR_CODE\" FROM \"TBLDTCFAILURE\" left join \"TBLESTIMATIONDETAILS\" on \"EST_FAILUREID\"=\"DF_ID\" left JOIN \"TBLWORKORDER\" ON \"WO_DF_ID\" = \"DF_ID\"  left JOIN \"TBLINDENT\" ON \"WO_SLNO\" = \"TI_WO_SLNO\"   left join \"TBLRECEIVEDTR\" on \"WO_SLNO\"=\"RD_WO_SLNO\" WHERE  \"DF_STATUS_FLAG\" IN (1,4) and \"EST_FAIL_TYPE\"=1  and TO_DATE(TO_CHAR(\"RD_RECEIVE_DATE\",'MM-yyyy'),'MM-yyyy') = TO_DATE('" + month + "','MM-yyyy')  )a";


                strQry += " union all ";
                //pending

                strQry += " SELECT sum(\"DTR_CODE\") from ( ";
                strQry += " SELECT count(\"DF_EQUIPMENT_ID\")  \"DTR_CODE\"  FROM \"TBLDTCFAILURE\" left join \"TBLESTIMATIONDETAILS\" on \"EST_FAILUREID\"=\"DF_ID\" left JOIN \"TBLWORKORDER\" ON \"WO_DF_ID\" = \"DF_ID\"  left JOIN \"TBLINDENT\" ON \"WO_SLNO\" = \"TI_WO_SLNO\" left JOIN \"TBLDTCINVOICE\" ON \"TI_ID\"  = \"IN_TI_NO\"  WHERE  \"DF_STATUS_FLAG\" IN (1,4) and \"EST_FAIL_TYPE\"=2 AND  TO_DATE(TO_CHAR(\"DF_DATE\",'MM-yyyy'),'MM-yyyy') <= TO_DATE('" + month + "','MM-yyyy')   AND  (\"IN_DATE\" is NULL or TO_DATE(TO_CHAR(\"IN_DATE\",'MM-yyyy'),'MM-yyyy') > TO_DATE('" + month + "','MM-yyyy'))     ";
                strQry += " union all ";
                strQry += " SELECT  count(\"DF_EQUIPMENT_ID\") FROM \"TBLDTCFAILURE\" left join \"TBLESTIMATIONDETAILS\" on \"EST_FAILUREID\"=\"DF_ID\" left JOIN \"TBLWORKORDER\" ON \"WO_DF_ID\" = \"DF_ID\"  left JOIN \"TBLINDENT\" ON \"WO_SLNO\" = \"TI_WO_SLNO\"  left join \"TBLRECEIVEDTR\" on \"WO_SLNO\"=\"RD_WO_SLNO\" WHERE   \"DF_STATUS_FLAG\" IN (1,4)  and \"EST_FAIL_TYPE\"=1  AND  TO_DATE(TO_CHAR(\"DF_DATE\",'MM-yyyy'),'MM-yyyy') <= TO_DATE('" + month + "','MM-yyyy') and (\"RD_RECEIVE_DATE\" is NULL or TO_DATE(TO_CHAR(\"RD_RECEIVE_DATE\",'MM-yyyy'),'MM-yyyy') > TO_DATE('" + month + "','MM-yyyy') )  )a";




                strQry += " union all ";
                //finalcialyearfail

                strQry += " SELECT sum(\"DTR_CODE\") FROM ";
                strQry += " (SELECT count(\"DF_EQUIPMENT_ID\") \"DTR_CODE\"  FROM \"TBLDTCFAILURE\" left join \"TBLESTIMATIONDETAILS\" on \"EST_FAILUREID\"=\"DF_ID\" left JOIN \"TBLWORKORDER\" ON \"WO_DF_ID\" = \"DF_ID\"  left JOIN \"TBLINDENT\" ON \"WO_SLNO\" = \"TI_WO_SLNO\"   left join \"TBLRECEIVEDTR\" on \"WO_SLNO\"=\"RD_WO_SLNO\"  WHERE \"DF_STATUS_FLAG\" IN (1,4) and  TO_DATE(TO_CHAR(\"DF_DATE\",'yyyy-mm-dd'),'yyyy-mm-dd') > TO_DATE('" + previosyear + "-03-31','yyyy-mm-dd') and TO_DATE(TO_CHAR(\"DF_DATE\",'yyyy-mm-dd'),'yyyy-mm-dd') < TO_DATE('" + presentyear + "-04-01','yyyy-mm-dd') and  \"EST_FAIL_TYPE\" in(1,2)  )a";

                //replacedfyyear
                strQry += " union all ";
                strQry += " SELECT sum(\"DTR_CODE\") from (";
                strQry += " SELECT count(\"DF_EQUIPMENT_ID\")  \"DTR_CODE\"  FROM \"TBLDTCFAILURE\" left join \"TBLESTIMATIONDETAILS\" on \"EST_FAILUREID\"=\"DF_ID\"  left JOIN \"TBLWORKORDER\" ON \"WO_DF_ID\" = \"DF_ID\"  left JOIN \"TBLINDENT\" ON \"WO_SLNO\" = \"TI_WO_SLNO\" left JOIN \"TBLDTCINVOICE\" ON \"TI_ID\"  = \"IN_TI_NO\"   WHERE  \"DF_STATUS_FLAG\" IN (1,4) and \"EST_FAIL_TYPE\"=2 and  TO_DATE(TO_CHAR(\"IN_DATE\",'yyyy-mm-dd'),'yyyy-mm-dd') > TO_DATE('" + previosyear + "-03-31','yyyy-mm-dd') and TO_DATE(TO_CHAR(\"DF_DATE\",'yyyy-mm-dd'),'yyyy-mm-dd') < TO_DATE('" + presentyear + "-04-01','yyyy-mm-dd')  ";
                strQry += " union all ";
                strQry += " SELECT  count(\"DF_EQUIPMENT_ID\") \"DTR_CODE\" FROM \"TBLDTCFAILURE\" left join \"TBLESTIMATIONDETAILS\" on \"EST_FAILUREID\"=\"DF_ID\" left JOIN \"TBLWORKORDER\" ON \"WO_DF_ID\" = \"DF_ID\"  left JOIN \"TBLINDENT\" ON \"WO_SLNO\" = \"TI_WO_SLNO\"   left join \"TBLRECEIVEDTR\" on \"WO_SLNO\"=\"RD_WO_SLNO\" WHERE  \"DF_STATUS_FLAG\" IN (1,4) and \"EST_FAIL_TYPE\"=1  and TO_DATE(TO_CHAR(\"RD_RECEIVE_DATE\",'yyyy-mm-dd'),'yyyy-mm-dd') > TO_DATE('" + previosyear + "-03-31','yyyy-mm-dd') and TO_DATE(TO_CHAR(\"DF_DATE\",'yyyy-mm-dd'),'yyyy-mm-dd') < TO_DATE('" + presentyear + "-04-01','yyyy-mm-dd')  )a";


                strQry += " union all";
                //dtc count

                strQry += " SELECT COUNT(\"DT_CODE\") FROM \"TBLDTCMAST\" WHERE   \"DT_TC_ID\"<>0  and TO_DATE(TO_CHAR(\"DT_CRON\",'yyyy-mm-dd'),'yyyy-mm-dd') > TO_DATE('" + previosyear + "-03-31','yyyy-mm-dd') and TO_DATE(TO_CHAR(\"DT_CRON\",'yyyy-mm-dd'),'yyyy-mm-dd') < TO_DATE('" + presentyear + "-04-01','yyyy-mm-dd') ";



                dt = ObjCon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }
        }


        public DataTable Detailsaddedabstract(string previosyear, string presentyear, string month, string previousmonth)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                //ob
                strQry = " SELECT COUNT(\"DT_CODE\") as \"DT_CODE\" FROM \"TBLDTCMAST\" WHERE \"DT_TC_ID\"<>0 and  TO_DATE(TO_CHAR(\"DT_CRON\",'MM-yyyy'),'MM-yyyy') <= TO_DATE('" + previousmonth + "','MM-yyyy')";

                strQry += " union all ";
                //addmonth
                strQry += " SELECT count(\"TC_CODE\") from";
                strQry += " \"TBLDTCMAST\" inner join \"TBLFEEDERMAST\" on \"DT_FDRSLNO\"=\"FD_FEEDER_CODE\" inner join \"TBLFEEDERCATEGORY\" on \"FD_FC_ID\" =\"FC_ID\" INNER JOIN \"TBLFDRTYPE\" ON \"FC_FT_ID\"=\"FT_ID\" inner join \"TBLTCMASTER\" on \"TC_CODE\"=\"DT_TC_ID\" INNER join \"TBLTCDRAWN\" on \"TC_CODE\"=\"TD_TC_NO\" inner join \"TBLDTCINVOICE\" on  \"IN_NO\"=\"TD_INV_NO\"  WHERE \"TC_LOCATION_ID\"<>0 and \"TC_CURRENT_LOCATION\"=2 and \"TC_CAPACITY\"<>0 and \"TC_CODE\"<>0   and \"TD_DF_ID\" is null and TO_DATE(TO_CHAR(\"DT_CRON\",'MM-yyyy'),'MM-yyyy') = TO_DATE('" + month + "','MM-yyyy')";

                strQry += " union all ";
                //fyadded
                strQry += " SELECT count(\"TC_CODE\") from ";
                strQry += " \"TBLDTCMAST\" inner join \"TBLFEEDERMAST\" on \"DT_FDRSLNO\"=\"FD_FEEDER_CODE\" inner join \"TBLFEEDERCATEGORY\" on \"FD_FC_ID\" =\"FC_ID\" INNER JOIN \"TBLFDRTYPE\" ON \"FC_FT_ID\"=\"FT_ID\" inner join \"TBLTCMASTER\" on \"TC_CODE\"=\"DT_TC_ID\" INNER join \"TBLTCDRAWN\" on \"TC_CODE\"=\"TD_TC_NO\" inner join \"TBLDTCINVOICE\" on  \"IN_NO\"=\"TD_INV_NO\"  WHERE \"TC_LOCATION_ID\"<>0 and \"TC_CURRENT_LOCATION\"=2 and \"TC_CAPACITY\"<>0 and \"TC_CODE\"<>0   and \"TD_DF_ID\" is null and   TO_DATE(TO_CHAR(\"DT_CRON\",'yyyy-mm-dd'),'yyyy-mm-dd') > TO_DATE('" + previosyear + "-03-31','yyyy-mm-dd') and TO_DATE(TO_CHAR(\"DT_CRON\",'yyyy-mm-dd'),'yyyy-mm-dd') < TO_DATE('" + presentyear + "-04-01','yyyy-mm-dd') ";



                dt = ObjCon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }
        }



        public DataTable Getabstractfprevadded(string previousmonth)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                //ob
                strQry = "SELECT COUNT(\"DT_CODE\")\"No.of DTs existing as at the end of previous month\" FROM \"TBLDTCMAST\" WHERE   \"DT_TC_ID\"<>0  and  TO_DATE(TO_CHAR(\"DT_CRON\",'MM-yyyy'),'MM-yyyy')<TO_DATE('" + previousmonth + "','MM-yyyy')";


                dt = ObjCon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }
        }


        public DataTable Getabstractfadded(string month)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                //ob
                strQry = " SELECT *,CAST(COALESCE(\"DEDICATED WATER SUPPLY WORKS\",0) AS bigint)+CAST(COALESCE(\"REGULAIZATION UAIP\",0) AS bigint)+CAST(COALESCE(\"SERVICES CONNECTION\",0) AS bigint)+CAST(COALESCE(\"WATER SUPPLY\",0) AS bigint)+  CAST(COALESCE(\"GK BACKWARD CLASS DEVELOPMENT CORPORATION\",0) AS bigint)+CAST(COALESCE(\"GK MINORITY CLASS DEVELOPMENT CORPORATION\",0) AS bigint)+CAST(COALESCE(\"GK SC CLASS DEVELOPMENT CORPORATION\",0) AS bigint)+CAST(COALESCE(\"GK ST CLASS DEVELOPMENT CORPORATION\",0) AS bigint)+CAST(COALESCE(\"SHEEGRA VIDYUTH YOJANE\",0) AS bigint)+CAST(COALESCE(\"ADDITIONAL TRANSFORMER CENTERS\",0) AS bigint)+  CAST(COALESCE(\"SELF EXECUTION WORKS\",0) AS bigint)+CAST(COALESCE(\"DCW\",0) AS bigint)+CAST(COALESCE(\"SCP\",0) AS bigint)+CAST(COALESCE(\"TSP\" ,0)AS bigint)+CAST(COALESCE(\"DDUGJY\",0) AS bigint)+CAST(COALESCE(\"E&I/CAPEX\",0) AS bigint)+CAST(COALESCE(\"CWIP REPLACEMENT OF DISTRIBUTION TRANSFORMER\",0) AS bigint)+CAST(COALESCE(\"OTHERS\",0) AS bigint) \"TOTAL\" from crosstab ('SELECT ''HESCOM'' as \"Company\",CASE WHEN \"SCHM_NAME\" IS NOT NULL THEN \"SCHM_NAME\" else ''OTHERS''  END \"STATUS\",COALESCE (COUNT(b.\"DT_CODE\"),0)  \"DT_CODE\" FROM ";
                strQry += "(SELECT \"SCHM_NAME\",\"SCHM_ID\"from \"TBLDTCSCHEME\" WHERE \"SCHM_TYPE\"=''0'')a left join ";
                strQry += "(SELECT \"DT_CODE\",\"DT_PROJECTTYPE\",\"DT_WO_ID\",\"DT_CRON\"  from \"TBLDTCMAST\"  WHERE \"DT_WO_ID\"<>0 and TO_DATE(TO_CHAR(\"DT_CRON\",''MM-yyyy''),''MM-yyyy'')=TO_DATE(''" + month + "'',''MM-yyyy''))b on b.\"DT_PROJECTTYPE\"=a.\"SCHM_ID\"    ";
                strQry += "GROUP BY \"SCHM_NAME\",\"SCHM_ID\"')as ct (\"Company\" text,\"DEDICATED WATER SUPPLY WORKS\" bigint, \"REGULAIZATION UAIP\" bigint, \"SERVICES CONNECTION\" bigint, \"WATER SUPPLY\" bigint, \"GK BACKWARD CLASS DEVELOPMENT CORPORATION\" bigint,  \"GK MINORITY CLASS DEVELOPMENT CORPORATION\" bigint,  \"GK SC CLASS DEVELOPMENT CORPORATION\" bigint, \"GK ST CLASS DEVELOPMENT CORPORATION\" bigint, \"SHEEGRA VIDYUTH YOJANE\" bigint, \"ADDITIONAL TRANSFORMER CENTERS\" bigint, \"SELF EXECUTION WORKS\" bigint,  \"DCW\" bigint, \"SCP\" bigint,  \"TSP\" bigint, \"DDUGJY\" bigint, \"E&I/CAPEX\" bigint, \"CWIP REPLACEMENT OF DISTRIBUTION TRANSFORMER\" bigint,\"OTHERS\" bigint) ";


                dt = ObjCon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }
        }


        public DataTable Getabstractfpresentadded(string month)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                //ob
                strQry = "SELECT COUNT(\"DT_CODE\") \"Total No. of DTs as at the end of Present Month\" FROM \"TBLDTCMAST\" WHERE   \"DT_TC_ID\"<>0  and  TO_DATE(TO_CHAR(\"DT_CRON\",'MM-yyyy'),'MM-yyyy')<=TO_DATE('" + month + "','MM-yyyy') ";


                dt = ObjCon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }
        }


        public DataTable Getabstractadded(string previousyear, string presentyear)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                //ob
                strQry = "SELECT *,CAST(COALESCE(\"DEDICATED WATER SUPPLY WORKS\",0) AS bigint)+CAST(COALESCE(\"REGULAIZATION UAIP\",0) AS bigint)+CAST(COALESCE(\"SERVICES CONNECTION\",0) AS bigint)+CAST(COALESCE(\"WATER SUPPLY\",0) AS bigint)+  CAST(COALESCE(\"GK BACKWARD CLASS DEVELOPMENT CORPORATION\",0) AS bigint)+CAST(COALESCE(\"GK MINORITY CLASS DEVELOPMENT CORPORATION\",0) AS bigint)+CAST(COALESCE(\"GK SC CLASS DEVELOPMENT CORPORATION\",0) AS bigint)+CAST(COALESCE(\"GK ST CLASS DEVELOPMENT CORPORATION\",0) AS bigint)+CAST(COALESCE(\"SHEEGRA VIDYUTH YOJANE\",0) AS bigint)+CAST(COALESCE(\"ADDITIONAL TRANSFORMER CENTERS\",0) AS bigint)+  CAST(COALESCE(\"SELF EXECUTION WORKS\",0) AS bigint)+CAST(COALESCE(\"DCW\",0) AS bigint)+CAST(COALESCE(\"SCP\",0) AS bigint)+CAST(COALESCE(\"TSP\" ,0)AS bigint)+CAST(COALESCE(\"DDUGJY\",0) AS bigint)+CAST(COALESCE(\"E&I/CAPEX\",0) AS bigint)+CAST(COALESCE(\"CWIP REPLACEMENT OF DISTRIBUTION TRANSFORMER\",0) AS bigint)+CAST(COALESCE(\"OTHERS\",0) AS bigint) \"TOTAL\" from crosstab ('SELECT ''HESCOM'' as \"Company\",CASE WHEN \"SCHM_NAME\" IS NOT NULL THEN \"SCHM_NAME\" else ''OTHERS''  END \"STATUS\",COALESCE (COUNT(b.\"DT_CODE\"),0)  \"DT_CODE\" FROM ";
                strQry += " (SELECT \"SCHM_NAME\",\"SCHM_ID\" from \"TBLDTCSCHEME\" WHERE \"SCHM_TYPE\"=''0'')a left join ";
                strQry += "(SELECT \"DT_CODE\",\"DT_PROJECTTYPE\",\"DT_WO_ID\",\"DT_CRON\"  from \"TBLDTCMAST\"  WHERE \"DT_WO_ID\"<>0 and   TO_DATE(TO_CHAR(\"DT_CRON\",''yyyy-mm-dd''),''yyyy-mm-dd'') > TO_DATE(''" + previousyear + "-03-31'',''yyyy-mm-dd'') and TO_DATE(TO_CHAR(\"DT_CRON\",''yyyy-mm-dd''),''yyyy-mm-dd'') < TO_DATE(''" + presentyear + "-04-01'',''yyyy-mm-dd''))b on b.\"DT_PROJECTTYPE\"=a.\"SCHM_ID\"    ";
                strQry += "GROUP BY \"SCHM_NAME\",\"SCHM_ID\"')as ct (\"Company\" text,\"DEDICATED WATER SUPPLY WORKS\" bigint, \"REGULAIZATION UAIP\" bigint, \"SERVICES CONNECTION\" bigint, \"WATER SUPPLY\" bigint, \"GK BACKWARD CLASS DEVELOPMENT CORPORATION\" bigint,  \"GK MINORITY CLASS DEVELOPMENT CORPORATION\" bigint,  \"GK SC CLASS DEVELOPMENT CORPORATION\" bigint, \"GK ST CLASS DEVELOPMENT CORPORATION\" bigint, \"SHEEGRA VIDYUTH YOJANE\" bigint, \"ADDITIONAL TRANSFORMER CENTERS\" bigint, \"SELF EXECUTION WORKS\" bigint,  \"DCW\" bigint, \"SCP\" bigint,  \"TSP\" bigint, \"DDUGJY\" bigint, \"E&I/CAPEX\" bigint, \"CWIP REPLACEMENT OF DISTRIBUTION TRANSFORMER\" bigint,\"OTHERS\" bigint)  ";


                dt = ObjCon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }
        }



        public DataTable getSchemedetailscapacitywise(string fromdate)
        {
            DataTable dt = new DataTable();
            //DataTable dt1 = new DataTable();
            //DataTable finaldt = new DataTable();
            //string[] strArray = new string[3];
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_schemewise_details");
                cmd.Parameters.AddWithValue("todate", fromdate);
                dt = ObjCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }


        public DataTable Getrepairerabstract(string previousyear, string presentyear)
        {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable finaldt = new DataTable();

            try
            {

                string[] strArray = new string[3];


                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_repairerwise_details");
                cmd.Parameters.AddWithValue("previousyear", previousyear);
                cmd.Parameters.AddWithValue("presentyear", presentyear);


                dt = ObjCon.FetchDataTable(cmd);


                return dt;



            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable getrepaireryearwiseabstract()
        {
            DataTable dt = new DataTable();


            try
            {

                string[] strArray = new string[3];


                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_repaireryearwise_details");



                dt = ObjCon.FetchDataTable(cmd);


                return dt;



            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        public DataTable GetDTRFailureAbstract(string strMonth)
        {
            DataTable dtDTRFailureAbstract = new DataTable();
            try
            {
                // strMonth format  : 08-2019 (MM-yyyy)
                NpgsqlCommand cmd = new NpgsqlCommand("proc_getDTRFailureAbstract");
                cmd.Parameters.AddWithValue("selectedmonth", strMonth);
                dtDTRFailureAbstract = ObjCon.FetchDataTable(cmd);

                return dtDTRFailureAbstract;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtDTRFailureAbstract;
            }
        }


        public DataTable CalcEstimatedReport(string sFailId, string sFailType, string sInsulationType, string starrating, string StatusFalg)
        {
            // sFailType 1-> single coil, 2-> multi coil
            DataTable dtDetailedReport = new DataTable();
            string strQry = string.Empty;
            sEmployeeCost = ConfigurationSettings.AppSettings["EmployeeCost"];
            sESI = ConfigurationSettings.AppSettings["ESI"];
            ServiceTax = ConfigurationSettings.AppSettings["ServiceTax"];
            DecomLabourCost = ConfigurationSettings.AppSettings["DecomLabourCost"];
            try
            {
                int tcrating;
                if (starrating != "0")
                {
                    tcrating = Convert.ToInt32(starrating);

                }
                else
                {
                    //tcrating = Convert.ToInt32(ObjCon.get_value("SELECT \"TC_RATING\" from \"TBLTCMASTER\",\"TBLDTCFAILURE\"  WHERE  cast(\"DF_EQUIPMENT_ID\" as numeric)=\"TC_CODE\" AND CAST(\"DF_ID\" AS TEXT) ='" + sFailId + "'"));
                    tcrating = Convert.ToInt32(ObjCon.get_value("SELECT \"TC_RATING\" from \"TBLTCMASTER\",\"TBLDTCFAILURE\"  WHERE  cast(\"DF_EQUIPMENT_ID\" as TEXT)=\"TC_CODE\" AND CAST(\"DF_ID\" AS TEXT) ='" + sFailId + "'"));


                }

                strQry = "SELECT \"DF_ENHANCE_CAPACITY\" FROM \"TBLDTCFAILURE\" WHERE CAST(\"DF_ID\" AS TEXT) ='" + sFailId + "'";
                string sEnhanceCapacity = ObjCon.get_value(strQry);

                strQry = "SELECT * FROM (SELECT \"DF_DTC_CODE\",CAST(\"DF_EQUIPMENT_ID\" AS TEXT) AS \"DF_EQUIPMENT_ID\",TO_CHAR(\"DF_DATE\",'dd/MM/yyyy') AS \"DF_DATE\",\"DF_LOC_CODE\",";
                strQry += "(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\"AS TEXT) = SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "')) AS \"SUBDIVISION\", ";
                strQry += "(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where CAST(\"OM_CODE\" AS TEXT) = SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Section + "')) as \"LOCATION\",(select CAST(\"TC_CAPACITY\" AS TEXT) from \"TBLTCMASTER\" where \"TC_CODE\"=\"DF_EQUIPMENT_ID\")\"Capacity\", ";
                strQry += "(SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.Division + "')) as \"Division\", ";
                strQry += "(SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"=\"DF_DTC_CODE\") \"DT_NAME\", 'No' AS \"UNIT\",'1' as \"QUANTITY\",";
                strQry += "(SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1,'" + Constants.SubDivision + "') AND \"US_ROLE_ID\"='1' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1)";
                strQry += "\"SDO_USERNAME\", (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=CAST(\"DF_LOC_CODE\" AS TEXT) AND \"US_ROLE_ID\"='4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' ";
                strQry += "AND \"US_ID\"=(SELECT \"DF_CRBY\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\"='" + sFailId + "')) \"SO_USERNAME\"  FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\"='" + sFailId + "')A ";
                strQry += "RIGHT JOIN (SELECT \"TE_RATE\" as \"Price\",1*\"TE_RATE\" AS \"TotalAmount\", \"TE_COMMLABOUR\" as \"labourcharge\",(\"TE_COMMLABOUR\"*'" + sEmployeeCost + "')/100 as ";
                strQry += "\"EmployeeCost\",(\"TE_COMMLABOUR\"*'" + sESI + "')/100 as \"ESI\",(\"TE_COMMLABOUR\"*'" + ServiceTax + "')/100 as \"ServiceTax\", (\"TE_RATE\"*2)/100 as \"ContingencyCost\", ";
                strQry += "(\"TE_RATE\"+\"TE_COMMLABOUR\"+(\"TE_RATE\"*2)/100+(\"TE_COMMLABOUR\"*10)/100) as \"FinalTotal\",null AS \"EST_NO\", \"TC_CAPACITY\",  \"DF_ENHANCE_CAPACITY\" ,\"TIT_INSULATION_NAME\", \"TT_NAME\" FROM \"TBLITEMMASTER\",\"TBLTRANSINSULATIONTYPE\",\"TBLTRANSCORETYPE\"";
                strQry += ",\"TBLDTCFAILURE\",\"TBLTCMASTER\" WHERE \"DF_EQUIPMENT_ID\"=\"TC_CODE\"AND CAST(\"DF_ID\" AS TEXT)='" + sFailId + "' ";

                if (StatusFalg == "2" || StatusFalg == "4")
                {
                    strQry += " AND \"TE_CAPACITY\"=" + sEnhanceCapacity + " AND \"TE_TIT_ID\"=\"TIT_ID\" AND \"TT_ID\"= \"TIT_TT_ID\"  ";
                }
                else
                {
                    strQry += " AND \"TE_CAPACITY\"=\"TC_CAPACITY\" AND \"TE_TIT_ID\"=\"TIT_ID\" AND \"TT_ID\"= \"TIT_TT_ID\"  ";
                }

                if (sInsulationType == null || sInsulationType == "")
                {
                    // strQry += " AND CAST( \"TE_TIT_ID\" AS TEXT)='" + sInsulationType + "'";


                }
                else
                {
                    strQry += " AND CAST( \"TE_TIT_ID\" AS TEXT)='" + sInsulationType + "'";
                }
                if (tcrating > 3)
                {

                    strQry += " AND '0'=COALESCE(\"TE_STAR_RATE\",0))B ON ";
                }
                else
                {
                    if (starrating != "0")
                    {
                        strQry += " AND '" + starrating + "'=COALESCE(\"TE_STAR_RATE\",0))B ON ";
                    }
                    else
                    {
                        strQry += " AND COALESCE(\"TC_RATING\",0)=COALESCE(\"TE_STAR_RATE\",0))B ON ";
                    }
                }
                // AND COALESCE(\"TC_RATING\",0)=COALESCE(\"TE_STAR_RATE\",0))B ON ";
                strQry += " A.\"Capacity\"=CAST(B.\"TC_CAPACITY\" AS TEXT)";




                dtDetailedReport = ObjCon.FetchDataTable(strQry);
                return dtDetailedReport;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtDetailedReport;

            }
        }




        public DataTable CalcDecomEstimatedReport(string sFailId, string FailType, string sInsulationType, string starrating, string StatusFalg)
        {
            DataTable dtDetailedReport = new DataTable();
            string strQry = string.Empty;
            sEmployeeCost = ConfigurationSettings.AppSettings["EmployeeCost"];
            sESI = ConfigurationSettings.AppSettings["ESI"];
            ServiceTax = ConfigurationSettings.AppSettings["ServiceTax"];
            DecomLabourCost = ConfigurationSettings.AppSettings["DecomLabourCost"];

            try
            {
                int tcrating;
                if (starrating != "0")
                {
                    tcrating = Convert.ToInt32(starrating);


                }
                else
                {
                    //tcrating = Convert.ToInt32(ObjCon.get_value("SELECT \"TC_RATING\" from \"TBLTCMASTER\",\"TBLDTCFAILURE\"  WHERE  cast(\"DF_EQUIPMENT_ID\" as numeric)=\"TC_CODE\" AND CAST(\"DF_ID\" AS TEXT) ='" + sFailId + "'"));
                    tcrating = Convert.ToInt32(ObjCon.get_value("SELECT \"TC_RATING\" from \"TBLTCMASTER\",\"TBLDTCFAILURE\"  WHERE  cast(\"DF_EQUIPMENT_ID\" as TEXT)=\"TC_CODE\" AND CAST(\"DF_ID\" AS TEXT) ='" + sFailId + "'"));



                }
                strQry = "SELECT \"DF_EQUIPMENT_ID\" FROM \"TBLDTCFAILURE\" WHERE CAST(\"DF_ID\" AS TEXT)='" + sFailId + "'";
                string tc_code = ObjCon.get_value(strQry);

                //strQry = "SELECT \"RSM_GUARANTY_TYPE\" FROM \"TBLREPAIRSENTMASTER\",\"TBLREPAIRSENTDETAILS\" WHERE \"RSM_ID\"=\"RSD_RSM_ID\" AND \"RSD_ID\" = (SELECT MAX(\"RSD_ID\") FROM \"TBLREPAIRSENTDETAILS\",\"TBLTCMASTER\"  WHERE \"RSD_TC_CODE\"= \"TC_CODE\" AND CAST(\"TC_CODE\" AS TEXT)='" + tc_code + "')";
                //string res = ObjCon.get_value(strQry);

                strQry = "SELECT \"DF_ENHANCE_CAPACITY\" FROM \"TBLDTCFAILURE\" WHERE CAST(\"DF_ID\" AS TEXT) ='" + sFailId + "'";
                string sEnhanceCapacity = ObjCon.get_value(strQry);

                //if (res == null || res == "")
                //{


                strQry = " SELECT \"DF_ID\",\"DF_DTC_CODE\", \"FD_FEEDER_NAME\",\"DT_NAME\",CAST(\"TC_CODE\" AS TEXT) \"DTR_CODE\",(SELECT \"DIV_NAME\" FROM \"TBLDIVISION\"  WHERE CAST(\"DIV_CODE\" AS TEXT)= SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.Division + ")) as \"DIVISION\",";
                strQry += " CAST(\"DF_EQUIPMENT_ID\" AS TEXT) \"DF_EQUIPMENT_ID\",TO_CHAR(\"DF_DATE\",'dd/MM/yyyy') AS \"DF_DATE\",Replace(\"DF_REASON\",'ç',',') \"DF_REASON\",\"DF_LOC_CODE\",TO_CHAR(\"DF_DTR_COMMISSION_DATE\",'DD/MON/YYYY') AS \"DTR_COMMISSION_DATE\",TO_CHAR(\"DT_TRANS_COMMISION_DATE\",'dd/MON/yyyy') \"DTC_COMMISSION_DATE\",";
                strQry += " \"TE_RATE\" as Price ,(\"TE_COMMLABOUR\" *'" + DecomLabourCost + "') as \"TE_DECOMLABOUR\",CAST(\"TC_CAPACITY\" AS TEXT) \"TC_CAPACITY\",\"TC_CODE\",\"TC_SLNO\",'OLD' AS \"REP\", \"TE_COMMLABOUR\",";
                strQry += " (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TC_MAKE_ID\"=\"TM_ID\") \"TM_NAME\",\"DT_TOTAL_CON_KW\",(\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + sEmployeeCost + "')/100 as \"EMPLOYEECOST\",";
                strQry += " (\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' * '" + sESI + "')/100 as \"ESI\",(\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' *'" + ServiceTax + "')/100 as \"SERVICETAX\",((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "')/100)*2 as CONTINGENCYCOST, ((\"TE_COMMLABOUR\" * '" + DecomLabourCost + "')+((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' * '" + sEmployeeCost + "')/100)+((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' * '" + sESI + "')/100)+((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "' * '" + ServiceTax + "')/100)+((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "')/100)*2) as \"FINALTOTAL\",";
                strQry += " 'No' as \"UNIT\",'1' as \"QUANTITY\",(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\"  WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT)= SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ")) as SubDivision,";
                strQry += " (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where CAST(\"OM_CODE\" AS TEXT) = SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ")) as \"LOCATION\",";
                strQry += " \"TR_NAME\" AS \"TR_NAME\",\"DF_GUARANTY_TYPE\" AS \"TR_GUARANTY\" ";
                strQry += " ,null AS \"EST_NO\",(1 * \"TE_COMMLABOUR\" * '" + DecomLabourCost + "') \"LABOUR_COST\",\"DF_ENHANCE_CAPACITY\"   , \"TIT_INSULATION_NAME\", \"TT_NAME\", ";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Constants.SubDivision + ") AND \"US_ROLE_ID\"='1' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' LIMIT 1) \"SDO_USERNAME\",";
                strQry += " (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE CAST(\"US_OFFICE_CODE\" AS TEXT)=CAST(\"DF_LOC_CODE\" AS TEXT) AND \"US_ROLE_ID\"='4' AND \"US_MMS_ID\" IS NULL AND \"US_STATUS\"='A' AND \"US_ID\" =(SELECT \"DF_CRBY\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\" ='" + sFailId + "' LIMIT 1)) \"SO_USERNAME\" ";
                //strQry += " from  \"TBLDTCFAILURE\",\"TBLITEMMASTER\",\"TBLTCMASTER\",\"TBLDTCMAST\",\"TBLFEEDERMAST\",\"TBLTRANSINSULATIONTYPE\",\"TBLTRANSCORETYPE\" WHERE \"DF_DTC_CODE\"=\"DT_CODE\" AND ";
                strQry += " from  \"TBLDTCFAILURE\",\"TBLITEMMASTER\",\"TBLTCMASTER\",\"TBLDTCMAST\",\"TBLFEEDERMAST\",\"TBLTRANSINSULATIONTYPE\",\"TBLTRANSCORETYPE\",\"TBLTRANSREPAIRER\" WHERE \"DF_DTC_CODE\"=\"DT_CODE\" AND \"DF_REPAIRER_ID\"=\"TR_ID\" and ";

                strQry += " \"DF_EQUIPMENT_ID\"=\"TC_CODE\" AND \"DF_ID\"='" + sFailId + "'";
                if (StatusFalg == "2" || StatusFalg == "4")
                {
                    strQry += "AND " + sEnhanceCapacity + "=\"TE_CAPACITY\" AND \"TE_TIT_ID\"=\"TIT_ID\" AND \"TT_ID\"= \"TIT_TT_ID\"   ";
                }
                else
                {
                    strQry += "AND \"TC_CAPACITY\"=\"TE_CAPACITY\" AND \"TE_TIT_ID\"=\"TIT_ID\" AND \"TT_ID\"= \"TIT_TT_ID\"   ";
                }

                if (sInsulationType == null || sInsulationType == "")
                {
                    // strQry += " AND CAST( \"TE_TIT_ID\" AS TEXT)='" + sInsulationType + "'";   ,\"TBLTRANSREPAIRER\" WHERE \"DF_DTC_CODE\"=\"DT_CODE\" AND  \"DF_REPAIRER_ID\"=\"TR_ID\" and


                }
                else
                {
                    strQry += " AND CAST( \"TE_TIT_ID\" AS TEXT)='" + sInsulationType + "'";
                }
                if (tcrating > 3)
                {

                    strQry += " AND '0'=COALESCE(\"TE_STAR_RATE\",0) ";
                }
                else
                {
                    if (starrating != "0")
                    {
                        strQry += " AND '" + starrating + "'=COALESCE(\"TE_STAR_RATE\",0) ";
                    }
                    else
                    {
                        strQry += " AND COALESCE(\"TC_RATING\",0)=COALESCE(\"TE_STAR_RATE\",0)";
                    }

                }

                //AND COALESCE(\"TC_RATING\",0)=COALESCE(\"TE_STAR_RATE\",0)";
                //strQry += " AND \"FD_FEEDER_CODE\"=SUBSTR(\"DF_DTC_CODE\",1," + Constants.Feeder + ")";
                strQry += " AND \"FD_FEEDER_CODE\"=\"DT_FDRSLNO\"";






                //}


                dtDetailedReport = ObjCon.FetchDataTable(strQry);
                if (dtDetailedReport.Rows.Count > 0)
                {
                    if (String.IsNullOrEmpty(dtDetailedReport.Rows[0]["DTR_COMMISSION_DATE"].ToString()))
                    {
                        string df_id = dtDetailedReport.Rows[0]["DF_ID"].ToString();
                        string dtc_code = dtDetailedReport.Rows[0]["DF_DTC_CODE"].ToString();

                        strQry = "SELECT MAX(A.\"RANKID\") FROM (SELECT \"WO_WFO_ID\" \"RANKID\", \"dense_rank\"() over(ORDER BY \"WO_CR_ON\") from  \"TBLWORKFLOWOBJECTS\" WHERE \"WO_DATA_ID\"='" + dtc_code + "' AND \"WO_RECORD_ID\"='" + df_id + "' AND \"WO_BO_ID\"='9') \"A\" ";
                        string WFO_ID = ObjCon.get_value(strQry);

                        clsApproval objApproval = new clsApproval();
                        DataTable dtFailureDetails = new DataTable();
                        dtFailureDetails = objApproval.GetDatatableFromXML(WFO_ID);
                        if (dtFailureDetails.Rows.Count > 0)
                        {
                            //dtFailureDetails.Columns.RemoveAt(0);
                            if (dtFailureDetails.Columns.Contains("DTR_COMISSION_DATE"))
                            {
                                dtDetailedReport.Columns["DTR_COMMISSION_DATE"].ReadOnly = false;
                                dtDetailedReport.Rows[0]["DTR_COMMISSION_DATE"] = dtFailureDetails.Rows[0]["DTR_COMISSION_DATE"].ToString();
                                //dtDetailedReport.Columns.RemoveAt(10);
                                //dtDetailedReport.Columns.Add("DTR_COMISSION_DATE", typeof(string));
                                //DataRow dtrow = dtDetailedReport.NewRow();
                                //dtrow["DTR_COMISSION_DATE"] = dtFailureDetails.Rows[0]["DTR_COMISSION_DATE"].ToString();
                                //dtDetailedReport.Rows.Add(dtrow);
                            }
                            else
                            {
                                dtDetailedReport.Columns["DTR_COMMISSION_DATE"].ReadOnly = false;
                                dtDetailedReport.Rows[0]["DTR_COMMISSION_DATE"] = "";
                            }
                        }
                    }
                }


                return dtDetailedReport;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtDetailedReport;

            }
        }

        public DataTable GetRepairerEstandWorkorderexcel(clsReports objReport)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;

            try
            {

                string[] strArray = new string[3];

                //  int Length = 0;
                //objReport.sOfficeCode = "";
                /// objReport.sFromDate = "";
                //objReport.sTodate = "";
                NpgsqlCommand cmd;
                if (objReport.sType == "1")
                {
                    cmd = new NpgsqlCommand("proc_get_repairerestimationdetails");
                }
                else
                {
                    cmd = new NpgsqlCommand("proc_get_repairerworkorderdetails");
                }
                cmd.Parameters.AddWithValue("officecode", objReport.sOfficeCode);
                cmd.Parameters.AddWithValue("fromdate", objReport.sFromDate);
                cmd.Parameters.AddWithValue("todate", objReport.sTodate);
                cmd.Parameters.AddWithValue("capacity", objReport.sCapacity);
                cmd.Parameters.AddWithValue("make", objReport.sMake);
                cmd.Parameters.AddWithValue("wound", objReport.sWoundType);
                cmd.Parameters.AddWithValue("coiltype", objReport.sCoilType);
                cmd.Parameters.AddWithValue("ratetype", objReport.sStarType);
                cmd.Parameters.AddWithValue("gurantee", objReport.sGuranteeType);

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
        /// Daily Report
        /// The purpose of this report is to provide daily details of failures for a selected month.
        /// Additionally, users can analyze the most frequent reasons for failures and check the status of the transformer that caused the failure.
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable PrintFailuresDailyReport(clsReports objReport)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_failures_dailyreport");
                cmd.Parameters.AddWithValue("p_loccode", Convert.ToString(objReport.sOfficeCode ?? ""));
                cmd.Parameters.AddWithValue("p_fromdate", Convert.ToString(objReport.sFromDate ?? ""));
                cmd.Parameters.AddWithValue("p_todate", Convert.ToString(objReport.sTodate ?? ""));
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
        /// Getting Failure Replacement Details In Excel
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable GetExportExcelFailureReplacement(clsReports objReport)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_failure_replacemnt");
                cmd.Parameters.AddWithValue("p_loccode", Convert.ToString(objReport.sOfficeCode ?? ""));
                cmd.Parameters.AddWithValue("m_date", Convert.ToString(objReport.sFromDate ?? ""));
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
        /// Sub-Division wise Transformer Failure
        /// The purpose of this report is to provide Sub-Division wise Transformer Failure Based on Capacity for a selected month.
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable PrintSubDivisionwiseTransformerFailureReport(clsReports objReport)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                if ((objReport.sType ?? "").Length == 0)
                {
                    //for export excel
                    NpgsqlCommand cmd = new NpgsqlCommand("proc_get_subdivision_wise_failure_details");
                    cmd.Parameters.AddWithValue("p_loccode", Convert.ToString(objReport.sOfficeCode ?? ""));
                    cmd.Parameters.AddWithValue("p_fromdate", Convert.ToString(objReport.sFromDate ?? ""));
                    cmd.Parameters.AddWithValue("p_todate", Convert.ToString(objReport.sTodate ?? ""));
                    dt = ObjCon.FetchDataTable(cmd);
                }
                else
                {
                    //for Cristel report
                    NpgsqlCommand cmd = new NpgsqlCommand("proc_get_subdivwise_failure_for_crreport");
                    cmd.Parameters.AddWithValue("p_loccode", Convert.ToString(objReport.sOfficeCode ?? ""));
                    cmd.Parameters.AddWithValue("p_fromdate", Convert.ToString(objReport.sFromDate ?? ""));
                    cmd.Parameters.AddWithValue("p_todate", Convert.ToString(objReport.sTodate ?? ""));
                    dt = ObjCon.FetchDataTable(cmd);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
            return dt;
        }
        /// <summary>
        /// This method used to fetch the repairer details 
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable GetCircleWiseDTrRepairer(clsReports objReport)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_circlewise_dtr_repirer_clsreport");
                cmd.Parameters.AddWithValue("financialyear_id", Convert.ToInt32(objReport.FinancialYearId));
                cmd.Parameters.AddWithValue("off_code", Convert.ToString(objReport.sOfficeCode ?? ""));
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
        /// this method used to get records for abstract report
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable GetCircleWiseDTrRepairerAbstract(clsReports objReport)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_circlewise_dtr_repirer_abstract_clsreport");
                cmd.Parameters.AddWithValue("financialyear_id", Convert.ToInt32(objReport.FinancialYearId));
                cmd.Parameters.AddWithValue("off_code", Convert.ToString(objReport.sOfficeCode ?? ""));
                dt = ObjCon.FetchDataTable(cmd);


                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }
        }


        public DataTable GetCircleWiseRepairerAmount(clsReports objReport)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_circlewise_repirer_poamt_clsreport");
                cmd.Parameters.AddWithValue("financialyear_id", Convert.ToInt32(objReport.FinancialYearId));
                cmd.Parameters.AddWithValue("off_code", Convert.ToString(objReport.sOfficeCode ?? ""));
                dt = ObjCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }
        }
    }
}

 
#endregion