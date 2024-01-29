using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.PGSQL.DAL;
using IIITS.DTLMS.BL;
using System.Data;
using System.Data.OleDb;
using Npgsql;

namespace IIITS.DTLMS.BL
{
    public class clsFailureRepTImeLine
    {
        //CustOledbConnection objcon = new CustOledbConnection(Constants.Password);
        PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
        string strFormCode = "clsFailureRepTImeLine";
        string strQry = string.Empty;

        #region UnUsed Method
        //public DataTable LoadCircleDetails(string sFromDate, string sTodate)
        //{
        //    DataTable dtDetails = new DataTable();
        //    try
        //    {

        //        strQry = "  SELECT NVL(CircleCode, CircleCode1)CIRCLECODE, NVL(CIRCLE, CIRCLE1)CIRCLE,nvl(LESSTHAN1DAY,0)LESSTHAN1DAY,nvl(BW1TO7,0)BW1TO7,nvl(BW7TO15,0)BW7TO15,";
        //        strQry += "nvl(BW15TO30,0)BW15TO30,nvl(ABOVE30,0)ABOVE30,nvl(TOTAL,0)TOTAL,nvl(LESSTHAN1DAYnew,0)LESSTHAN1DAYnew,nvl(BW1TO7new,0)BW1TO7new,";
        //        strQry += "nvl(BW7TO15new,0)BW7TO15new,nvl(BW15TO30new,0)BW15TO30new,nvl(ABOVE30new,0)ABOVE30new,nvl(TOTALnew,0)TOTALnew FROM ";
        //        strQry += "(SELECT NVL(CircleCode,0)CircleCode, CIRCLE, NVL(SUM(LESSTHAN1),0)LESSTHAN1DAY, NVL(SUM(BETWEEN1TO7),0)BW1TO7,  ";
        //        strQry += "NVL(SUM(BETWEEN7TO15),0)BW7TO15, NVL(SUM(BETWEEN15TO30),0)BW15TO30, NVL(SUM(ABOVE30),0)ABOVE30,  NVL(SUM(TOTAL),0)TOTAL ";
        //        strQry += "FROM(SELECT(SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES WHERE   OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 1))";
        //        strQry += "CIRCLE, SUBSTR(DF_LOC_CODE, 0, 1)CircleCode, CASE WHEN(TR_DECOMM_DATE - DF_DATE)   BETWEEN 0 AND 1 THEN  COUNT(*) ELSE 0 END ";
        //        strQry += "LESSTHAN1, CASE WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 2 AND 7 THEN   COUNT(*) ELSE 0 END BETWEEN1TO7, ";
        //        strQry += "CASE WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 8 AND 15 THEN  COUNT(*) ELSE 0 END  BETWEEN7TO15, CASE WHEN(TR_DECOMM_DATE - DF_DATE)  ";
        //        strQry += "BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0 END BETWEEN15TO30,    CASE WHEN(TR_DECOMM_DATE - DF_DATE) > 30 THEN  COUNT(DF_DTC_CODE) ";
        //        strQry += "ELSE 0 END ABOVE30, FD_FEEDER_NAME, COUNT(*) TOTAL   FROM TBLFEEDERMAST INNER JOIN TBLDTCMAST ON  DT_FDRSLNO = FD_FEEDER_CODE ";
        //        strQry += "INNER JOIN    TBLDTCFAILURE ON DF_DTC_CODE = DT_CODE INNER JOIN   TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN   TBLINDENT ON ";
        //        strQry += "WO_SLNO = TI_WO_SLNO INNER JOIN TBLDTCINVOICE  ON TI_ID = IN_TI_NO INNER JOIN TBLTCREPLACE   ON IN_NO = TR_IN_NO AND ";
        //        strQry += "DF_REPLACE_FLAG <> 1  AND DF_STATUS_FLAG IN(1, 4) ";
        //        if (sFromDate != "" && sTodate != "")
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            sFromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            sTodate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == "")
        //        {

        //            sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            sFromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            sTodate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        else if (sFromDate == string.Empty && sTodate != "")
        //        {
        //            strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }

        //        strQry += "GROUP BY DF_ID, DF_DATE, TR_DECOMM_DATE,    DF_LOC_CODE, DF_DTC_CODE, FD_FEEDER_NAME) A ";
        //        strQry += "GROUP BY CIRCLE, CircleCode ORDER BY CircleCode)A full outer join   (SELECT NVL(CircleCode,0) AS CircleCode1,CIRCLE AS CIRCLE1,";
        //        strQry += "NVL(SUM(LESSTHAN1),0)LESSTHAN1DAYNEW,NVL(SUM(BETWEEN1TO7),0)BW1TO7NEW,  NVL(SUM(BETWEEN7TO15),0)BW7TO15NEW, ";
        //        strQry += "NVL(SUM(BETWEEN15TO30),0)BW15TO30NEW,NVL(SUM(ABOVE30),0)ABOVE30NEW,NVL(SUM(TOTAL),0)TOTALNEW FROM    ";
        //        strQry += "(SELECT IN_NO,(SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES WHERE    OFF_CODE = ";
        //        strQry += "SUBSTR(DF_LOC_CODE, 0, 1))CIRCLE, SUBSTR(DF_LOC_CODE, 0, 1)CircleCode,  CASE WHEN   (IN_DATE - DF_DATE)  BETWEEN 0 AND 1 THEN  ";
        //        strQry += "COUNT(*) ELSE 0 END LESSTHAN1, CASE WHEN(IN_DATE - DF_DATE)    BETWEEN 2 AND 7 THEN  COUNT(*) ELSE 0 END BETWEEN1TO7, ";
        //        strQry += "CASE WHEN(IN_DATE - DF_DATE)  BETWEEN 8 AND 15 THEN    COUNT(*) ELSE 0 END BETWEEN7TO15, CASE WHEN(IN_DATE - DF_DATE)  ";
        //        strQry += "BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0 END    BETWEEN15TO30, CASE WHEN(IN_DATE - DF_DATE) > 30 THEN  COUNT(DF_DTC_CODE) ";
        //        strQry += "ELSE 0 END ABOVE30, FD_FEEDER_NAME,   COUNT(*) TOTAL  FROM TBLFEEDERMAST INNER JOIN TBLDTCMAST ON  DT_FDRSLNO = FD_FEEDER_CODE ";
        //        strQry += "INNER JOIN     TBLDTCFAILURE ON DF_DTC_CODE = DT_CODE INNER JOIN   TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN  TBLINDENT    ";
        //        strQry += "ON WO_SLNO = TI_WO_SLNO INNER JOIN TBLDTCINVOICE  ON TI_ID = IN_TI_NO AND DF_REPLACE_FLAG <> 1 AND DF_STATUS_FLAG   IN(1, 4)   ";

        //        if (sFromDate != "" && sTodate != "")
        //        {
        //            //DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            //sFromDate = DFromDate.ToString("yyyyMMdd");
        //            //DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            //sTodate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == "")
        //        {

        //            //sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //            //DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            //sFromDate = DFromDate.ToString("yyyyMMdd");
        //            //DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            //sTodate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        else if (sFromDate == string.Empty && sTodate != "")
        //        {
        //            strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        strQry += " GROUP BY DF_ID, DF_DATE, IN_NO, IN_DATE, ";
        //        strQry += "DF_LOC_CODE, DF_DTC_CODE, FD_FEEDER_NAME) LEFT JOIN TBLTCREPLACE    ON IN_NO = TR_IN_NO  where IN_NO IS NOT NULL AND TR_IN_NO IS ";
        //        strQry += "NULL GROUP BY CIRCLE, CircleCode ORDER BY CircleCode)B  ON A.CircleCode = B.CircleCode1";
        //        dtDetails = objcon.FetchDataTable(strQry);
        //        return dtDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadCircleDetails");
        //        return dtDetails;

        //    }

        //}

        // 16/12/2021 modifed new query
        //public DataTable LoadZoneDetails(string sFromDate, string sTodate)
        //{
        //    DataTable dtDetails = new DataTable();
        //    try
        //    {

        //        strQry = "  SELECT COALESCE(\"ZoneCode\",\"ZoneCode1\")\"ZONECODE\", COALESCE(\"ZONE\",\"ZONE1\")\"ZONE\",COALESCE(\"LESSTHAN1DAY\",0)\"LESSTHAN1DAY\",COALESCE(\"BW1TO7\",0)\"BW1TO7\",COALESCE(\"BW7TO15\",0)\"BW7TO15\",";
        //        strQry += " COALESCE(\"BW15TO30\",0)\"BW15TO30\",COALESCE(\"ABOVE30\",0)\"ABOVE30\",COALESCE(\"TOTAL\",0)\"TOTAL\",COALESCE(\"LESSTHAN1DAYNEW\",0)\"LESSTHAN1DAYnew\",COALESCE(\"BW1TO7NEW\",0)\"BW1TO7new\",";
        //        strQry += "COALESCE(\"BW7TO15NEW\",0)\"BW7TO15new\",COALESCE(\"BW15TO30NEW\",0)\"BW15TO30new\",COALESCE(\"ABOVE30NEW\",0)\"ABOVE30new\",COALESCE(\"TOTALNEW\",0)\"TOTALnew\" FROM  ";
        //        strQry += " (SELECT COALESCE(cast(\"ZoneCode\" as INTEGER),0)\"ZoneCode\", \"ZONE\", COALESCE(SUM(\"LESSTHAN1\"),0)\"LESSTHAN1DAY\", COALESCE(SUM(\"BETWEEN1TO7\"),0)\"BW1TO7\", ";
        //        strQry += "  COALESCE(SUM(\"BETWEEN7TO15\"),0)\"BW7TO15\", COALESCE(SUM(\"BETWEEN15TO30\"),0)\"BW15TO30\", COALESCE(SUM(\"ABOVE30\"),0)\"ABOVE30\",  COALESCE(SUM(\"TOTAL\"),0)\"TOTAL\" ";
        //        strQry += "FROM(SELECT \"DT_OM_SLNO\",(SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE   CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DF_LOC_CODE\" as text), 1," + Constants.Zone + "))";
        //        strQry += "\"ZONE\", SUBSTR(cast(\"DF_LOC_CODE\" as text), 1," + Constants.Zone + ")\"ZoneCode\", CASE WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")   BETWEEN 0 AND 1 THEN  COUNT(*) ELSE 0 END ";
        //        strQry += " \"LESSTHAN1\", CASE WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")  BETWEEN 2 AND 7 THEN   COUNT(*) ELSE 0 END \"BETWEEN1TO7\", ";
        //        strQry += " CASE WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")  BETWEEN 8 AND 15 THEN  COUNT(*) ELSE 0 END  \"BETWEEN7TO15\", CASE WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")  ";
        //        strQry += " BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0 END\"BETWEEN15TO30\",    CASE WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\") > 30 THEN  COUNT(\"DF_DTC_CODE\") ";
        //        strQry += " ELSE 0 END \"ABOVE30\",\"FD_FEEDER_NAME\", COUNT(*) \"TOTAL\"   FROM \"TBLFEEDERMAST\" INNER JOIN \"TBLDTCMAST\" ON  \"DT_FDRSLNO\" = \"FD_FEEDER_CODE\" ";
        //        strQry += " INNER JOIN  \"TBLDTCFAILURE\" ON \"DF_DTC_CODE\" = \"DT_CODE\" INNER JOIN   \"TBLWORKORDER\" ON \"DF_ID\" = \"WO_DF_ID\" INNER JOIN  \"TBLINDENT\" ON ";
        //        strQry += " \"WO_SLNO\" = \"TI_WO_SLNO\" INNER JOIN \"TBLDTCINVOICE\"  ON \"TI_ID\" = \"IN_TI_NO\" INNER JOIN \"TBLTCREPLACE\" ON \"IN_NO\" = \"TR_IN_NO\" and ";
        //        strQry += " \"DF_REPLACE_FLAG\" <> 1  AND \"DF_STATUS_FLAG\" IN(1, 4)  and cast(\"DT_OM_SLNO\" as text) like '3%' ";
        //        if (sFromDate != "" && sTodate != "")
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == "")
        //        {

        //            sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate == string.Empty && sTodate != "")
        //        {
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == string.Empty)
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "'";
        //        }

        //        strQry += " GROUP BY \"DF_ID\",\"DF_DATE\", \"TR_DECOMM_DATE\",\"DF_LOC_CODE\", \"DF_DTC_CODE\", \"FD_FEEDER_NAME\",\"DT_OM_SLNO\") A ";
        //        strQry += " GROUP BY \"ZONE\",\"ZoneCode\" ORDER BY \"ZoneCode\")A FULL OUTER JOIN   (SELECT COALESCE(cast(\"ZoneCode\" as INTEGER),0) AS \"ZoneCode1\",\"ZONE\" AS \"ZONE1\",";
        //        strQry += " COALESCE(SUM(\"LESSTHAN1\"),0) \"LESSTHAN1DAYNEW\",COALESCE(SUM(\"BETWEEN1TO7\"),0)\"BW1TO7NEW\",  COALESCE(SUM(\"BETWEEN7TO15\"),0)\"BW7TO15NEW\", ";
        //        strQry += " COALESCE(SUM(\"BETWEEN15TO30\"),0) \"BW15TO30NEW\",COALESCE(SUM(\"ABOVE30\"),0)\"ABOVE30NEW\",COALESCE(SUM(\"TOTAL\"),0)\"TOTALNEW\" FROM   ";
        //        strQry += "(SELECT \"DT_OM_SLNO\", \"IN_NO\",(SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE    CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(CAST";
        //        strQry += " (\"DF_LOC_CODE\" AS text), 1, " + Constants.Zone + "))\"ZONE\", SUBSTR(CAST(\"DF_LOC_CODE\" as text), 1, " + Constants.Zone + ")\"ZoneCode\",   CASE WHEN   (\"IN_DATE\" - \"DF_DATE\")  BETWEEN 0 AND 1 THEN ";
        //        strQry += " COUNT(*) ELSE 0 END \"LESSTHAN1\", CASE WHEN(\"IN_DATE\" - \"DF_DATE\") BETWEEN 2 AND 7 THEN  COUNT(*) ELSE 0 END \"BETWEEN1TO7\", ";
        //        strQry += " CASE WHEN(\"IN_DATE\" - \"DF_DATE\")  BETWEEN 8 AND 15 THEN    COUNT(*) ELSE 0 END \"BETWEEN7TO15\", CASE WHEN(\"IN_DATE\" -\"DF_DATE\")  ";
        //        strQry += " BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0 END \"BETWEEN15TO30\", CASE WHEN(\"IN_DATE\" - \"DF_DATE\") > 30 THEN  COUNT(\"DF_DTC_CODE\") ";
        //        strQry += " ELSE 0 END \"ABOVE30\",\"FD_FEEDER_NAME\",COUNT(*) \"TOTAL\"  FROM \"TBLFEEDERMAST\" INNER JOIN \"TBLDTCMAST\" ON  \"DT_FDRSLNO\" = \"FD_FEEDER_CODE\" ";
        //        strQry += " INNER JOIN \"TBLDTCFAILURE\" ON \"DF_DTC_CODE\" = \"DT_CODE\" INNER JOIN   \"TBLWORKORDER\" ON \"DF_ID\" = \"WO_DF_ID\" INNER JOIN \"TBLINDENT\" ";
        //        strQry += "  ON \"WO_SLNO\" = \"TI_WO_SLNO\" INNER JOIN \"TBLDTCINVOICE\"  ON \"TI_ID\" = \"IN_TI_NO\" AND \"DF_REPLACE_FLAG\" <> 1 AND \"DF_STATUS_FLAG\"   IN(1, 4)";

        //        if (sFromDate != "" && sTodate != "")
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == "")
        //        {

        //            sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate == string.Empty && sTodate != "")
        //        {
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == string.Empty)
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "'";
        //        }
        //        strQry += "  GROUP BY \"DF_ID\",\"DF_DATE\",\"IN_NO\",\"IN_DATE\", ";
        //        strQry += " \"DF_LOC_CODE\",\"DF_DTC_CODE\",\"FD_FEEDER_NAME\",\"DT_OM_SLNO\") a LEFT JOIN \"TBLTCREPLACE\"    ON \"IN_NO\" = \"TR_IN_NO\"  where \"IN_NO\" IS NOT NULL AND \"TR_IN_NO\" IS ";
        //        strQry += "NULL and cast(\"DT_OM_SLNO\" as text) like '3%' GROUP BY \"ZONE\", \"ZoneCode\" ORDER BY \"ZoneCode\")B  ON A.\"ZoneCode\" = B.\"ZoneCode1\"";
        //        dtDetails = objcon.FetchDataTable(strQry);
        //        return dtDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return dtDetails;

        //    }

        //}


        //old query to display all 4 zones details

        //public DataTable LoadZoneDetails(string sFromDate, string sTodate)
        //{
        //    DataTable dtDetails = new DataTable();
        //    try
        //    {

        //        strQry = "  SELECT COALESCE(\"ZoneCode\",\"ZoneCode1\")\"ZONECODE\", COALESCE(\"ZONE\",\"ZONE1\")\"ZONE\",COALESCE(\"LESSTHAN1DAY\",0)\"LESSTHAN1DAY\",COALESCE(\"BW1TO7\",0)\"BW1TO7\",COALESCE(\"BW7TO15\",0)\"BW7TO15\",";
        //        strQry += " COALESCE(\"BW15TO30\",0)\"BW15TO30\",COALESCE(\"ABOVE30\",0)\"ABOVE30\",COALESCE(\"TOTAL\",0)\"TOTAL\",COALESCE(\"LESSTHAN1DAYNEW\",0)\"LESSTHAN1DAYnew\",COALESCE(\"BW1TO7NEW\",0)\"BW1TO7new\",";
        //        strQry += "COALESCE(\"BW7TO15NEW\",0)\"BW7TO15new\",COALESCE(\"BW15TO30NEW\",0)\"BW15TO30new\",COALESCE(\"ABOVE30NEW\",0)\"ABOVE30new\",COALESCE(\"TOTALNEW\",0)\"TOTALnew\" FROM  ";
        //        strQry += " (SELECT COALESCE(cast(\"ZoneCode\" as INTEGER),0)\"ZoneCode\", \"ZONE\", COALESCE(SUM(\"LESSTHAN1\"),0)\"LESSTHAN1DAY\", COALESCE(SUM(\"BETWEEN1TO7\"),0)\"BW1TO7\", ";
        //        strQry += "  COALESCE(SUM(\"BETWEEN7TO15\"),0)\"BW7TO15\", COALESCE(SUM(\"BETWEEN15TO30\"),0)\"BW15TO30\", COALESCE(SUM(\"ABOVE30\"),0)\"ABOVE30\",  COALESCE(SUM(\"TOTAL\"),0)\"TOTAL\" ";
        //        strQry += "FROM(SELECT(SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE   CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DF_LOC_CODE\" as text), 1," + Constants.Zone + "))";
        //        strQry += "\"ZONE\", SUBSTR(cast(\"DF_LOC_CODE\" as text), 1," + Constants.Zone + ")\"ZoneCode\", CASE WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")   BETWEEN 0 AND 1 THEN  COUNT(*) ELSE 0 END ";
        //        strQry += " \"LESSTHAN1\", CASE WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")  BETWEEN 2 AND 7 THEN   COUNT(*) ELSE 0 END \"BETWEEN1TO7\", ";
        //        strQry += " CASE WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")  BETWEEN 8 AND 15 THEN  COUNT(*) ELSE 0 END  \"BETWEEN7TO15\", CASE WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")  ";
        //        strQry += " BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0 END\"BETWEEN15TO30\",    CASE WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\") > 30 THEN  COUNT(\"DF_DTC_CODE\") ";
        //        strQry += " ELSE 0 END \"ABOVE30\",\"FD_FEEDER_NAME\", COUNT(*) \"TOTAL\"   FROM \"TBLFEEDERMAST\" INNER JOIN \"TBLDTCMAST\" ON  \"DT_FDRSLNO\" = \"FD_FEEDER_CODE\" ";
        //        strQry += " INNER JOIN  \"TBLDTCFAILURE\" ON \"DF_DTC_CODE\" = \"DT_CODE\" INNER JOIN   \"TBLWORKORDER\" ON \"DF_ID\" = \"WO_DF_ID\" INNER JOIN  \"TBLINDENT\" ON ";
        //        strQry += " \"WO_SLNO\" = \"TI_WO_SLNO\" INNER JOIN \"TBLDTCINVOICE\"  ON \"TI_ID\" = \"IN_TI_NO\" INNER JOIN \"TBLTCREPLACE\" ON \"IN_NO\" = \"TR_IN_NO\" and ";
        //        strQry += " \"DF_REPLACE_FLAG\" <> 1  AND \"DF_STATUS_FLAG\" IN(1, 4) ";
        //        if (sFromDate != "" && sTodate != "")
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == "")
        //        {

        //            sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate == string.Empty && sTodate != "")
        //        {
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == string.Empty)
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "'";
        //        }

        //        strQry += " GROUP BY \"DF_ID\",\"DF_DATE\", \"TR_DECOMM_DATE\",\"DF_LOC_CODE\", \"DF_DTC_CODE\", \"FD_FEEDER_NAME\") A ";
        //        strQry += " GROUP BY \"ZONE\",\"ZoneCode\" ORDER BY \"ZoneCode\")A FULL OUTER JOIN   (SELECT COALESCE(cast(\"ZoneCode\" as INTEGER),0) AS \"ZoneCode1\",\"ZONE\" AS \"ZONE1\",";
        //        strQry += " COALESCE(SUM(\"LESSTHAN1\"),0) \"LESSTHAN1DAYNEW\",COALESCE(SUM(\"BETWEEN1TO7\"),0)\"BW1TO7NEW\",  COALESCE(SUM(\"BETWEEN7TO15\"),0)\"BW7TO15NEW\", ";
        //        strQry += " COALESCE(SUM(\"BETWEEN15TO30\"),0) \"BW15TO30NEW\",COALESCE(SUM(\"ABOVE30\"),0)\"ABOVE30NEW\",COALESCE(SUM(\"TOTAL\"),0)\"TOTALNEW\" FROM   ";
        //        strQry += "(SELECT \"IN_NO\",(SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE    CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(CAST";
        //        strQry += " (\"DF_LOC_CODE\" AS text), 1, " + Constants.Zone + "))\"ZONE\", SUBSTR(CAST(\"DF_LOC_CODE\" as text), 1, " + Constants.Zone + ")\"ZoneCode\",   CASE WHEN   (\"IN_DATE\" - \"DF_DATE\")  BETWEEN 0 AND 1 THEN ";
        //        strQry += " COUNT(*) ELSE 0 END \"LESSTHAN1\", CASE WHEN(\"IN_DATE\" - \"DF_DATE\") BETWEEN 2 AND 7 THEN  COUNT(*) ELSE 0 END \"BETWEEN1TO7\", ";
        //        strQry += " CASE WHEN(\"IN_DATE\" - \"DF_DATE\")  BETWEEN 8 AND 15 THEN    COUNT(*) ELSE 0 END \"BETWEEN7TO15\", CASE WHEN(\"IN_DATE\" -\"DF_DATE\")  ";
        //        strQry += " BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0 END \"BETWEEN15TO30\", CASE WHEN(\"IN_DATE\" - \"DF_DATE\") > 30 THEN  COUNT(\"DF_DTC_CODE\") ";
        //        strQry += " ELSE 0 END \"ABOVE30\",\"FD_FEEDER_NAME\",COUNT(*) \"TOTAL\"  FROM \"TBLFEEDERMAST\" INNER JOIN \"TBLDTCMAST\" ON  \"DT_FDRSLNO\" = \"FD_FEEDER_CODE\" ";
        //        strQry += " INNER JOIN \"TBLDTCFAILURE\" ON \"DF_DTC_CODE\" = \"DT_CODE\" INNER JOIN   \"TBLWORKORDER\" ON \"DF_ID\" = \"WO_DF_ID\" INNER JOIN \"TBLINDENT\" ";
        //        strQry += "  ON \"WO_SLNO\" = \"TI_WO_SLNO\" INNER JOIN \"TBLDTCINVOICE\"  ON \"TI_ID\" = \"IN_TI_NO\" AND \"DF_REPLACE_FLAG\" <> 1 AND \"DF_STATUS_FLAG\"   IN(1, 4)";

        //        if (sFromDate != "" && sTodate != "")
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == "")
        //        {

        //            sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate == string.Empty && sTodate != "")
        //        {
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == string.Empty)
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "'";
        //        }
        //        strQry += "  GROUP BY \"DF_ID\",\"DF_DATE\",\"IN_NO\",\"IN_DATE\", ";
        //        strQry += " \"DF_LOC_CODE\",\"DF_DTC_CODE\",\"FD_FEEDER_NAME\") a LEFT JOIN \"TBLTCREPLACE\"    ON \"IN_NO\" = \"TR_IN_NO\"  where \"IN_NO\" IS NOT NULL AND \"TR_IN_NO\" IS ";
        //        strQry += "NULL GROUP BY \"ZONE\", \"ZoneCode\" ORDER BY \"ZoneCode\")B  ON A.\"ZoneCode\" = B.\"ZoneCode1\"";
        //        dtDetails = objcon.FetchDataTable(strQry);
        //        return dtDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return dtDetails;

        //    }

        //}
        //public DataTable LoadCircleDetails(string ZoneCode, string sFromDate, string sTodate)
        //{
        //    DataTable dtDetails = new DataTable();
        //    try
        //    {

        //        strQry = "  SELECT COALESCE(\"ZoneCode\",\"ZoneCode1\")\"ZONECODE\", COALESCE(\"ZONE\",\"ZONE1\")\"ZONE\",COALESCE(\"CircleCode\",\"CircleCode1\")\"CIRCLECODE\", COALESCE(\"CIRCLE\",\"CIRCLE1\")\"CIRCLE\",COALESCE(\"LESSTHAN1DAY\",0)\"LESSTHAN1DAY\",COALESCE(\"BW1TO7\",0)\"BW1TO7\",COALESCE(\"BW7TO15\",0)\"BW7TO15\",";
        //        strQry += " COALESCE(\"BW15TO30\",0)\"BW15TO30\",COALESCE(\"ABOVE30\",0)\"ABOVE30\",COALESCE(\"TOTAL\",0)\"TOTAL\",COALESCE(\"LESSTHAN1DAYNEW\",0)\"LESSTHAN1DAYnew\",COALESCE(\"BW1TO7NEW\",0)\"BW1TO7new\",";
        //        strQry += "COALESCE(\"BW7TO15NEW\",0)\"BW7TO15new\",COALESCE(\"BW15TO30NEW\",0)\"BW15TO30new\",COALESCE(\"ABOVE30NEW\",0)\"ABOVE30new\",COALESCE(\"TOTALNEW\",0)\"TOTALnew\" FROM  ";
        //        strQry += " (SELECT COALESCE(cast(\"ZoneCode\" as INTEGER),0)\"ZoneCode\",\"ZONE\",COALESCE(cast(\"CircleCode\" as INTEGER),0)\"CircleCode\", \"CIRCLE\", COALESCE(SUM(\"LESSTHAN1\"),0)\"LESSTHAN1DAY\", COALESCE(SUM(\"BETWEEN1TO7\"),0)\"BW1TO7\", ";
        //        strQry += "  COALESCE(SUM(\"BETWEEN7TO15\"),0)\"BW7TO15\", COALESCE(SUM(\"BETWEEN15TO30\"),0)\"BW15TO30\", COALESCE(SUM(\"ABOVE30\"),0)\"ABOVE30\",  COALESCE(SUM(\"TOTAL\"),0)\"TOTAL\" ";
        //        strQry += "FROM(SELECT(SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE   CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DF_LOC_CODE\" as text), 1," + Constants.Zone + "))";
        //        strQry += "\"ZONE\", SUBSTR(cast(\"DF_LOC_CODE\" as text), 1," + Constants.Zone + ")\"ZoneCode\",(SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE   CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DF_LOC_CODE\" as text), 1," + Constants.Circle + "))\"CIRCLE\", SUBSTR(cast(\"DF_LOC_CODE\" as text), 1," + Constants.Circle + ")\"CircleCode\", CASE WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")   BETWEEN 0 AND 1 THEN  COUNT(*) ELSE 0 END ";
        //        strQry += " \"LESSTHAN1\", CASE WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")  BETWEEN 2 AND 7 THEN   COUNT(*) ELSE 0 END \"BETWEEN1TO7\", ";
        //        strQry += " CASE WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")  BETWEEN 8 AND 15 THEN  COUNT(*) ELSE 0 END  \"BETWEEN7TO15\", CASE WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")  ";
        //        strQry += " BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0 END\"BETWEEN15TO30\",    CASE WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\") > 30 THEN  COUNT(\"DF_DTC_CODE\") ";
        //        strQry += " ELSE 0 END \"ABOVE30\",\"FD_FEEDER_NAME\", COUNT(*) \"TOTAL\"   FROM \"TBLFEEDERMAST\" INNER JOIN \"TBLDTCMAST\" ON  \"DT_FDRSLNO\" = \"FD_FEEDER_CODE\" ";
        //        strQry += " INNER JOIN  \"TBLDTCFAILURE\" ON \"DF_DTC_CODE\" = \"DT_CODE\" INNER JOIN   \"TBLWORKORDER\" ON \"DF_ID\" = \"WO_DF_ID\" INNER JOIN  \"TBLINDENT\" ON ";
        //        strQry += " \"WO_SLNO\" = \"TI_WO_SLNO\" INNER JOIN \"TBLDTCINVOICE\"  ON \"TI_ID\" = \"IN_TI_NO\" INNER JOIN \"TBLTCREPLACE\" ON \"IN_NO\" = \"TR_IN_NO\" and ";
        //        strQry += " \"DF_REPLACE_FLAG\" <> 1  AND \"DF_STATUS_FLAG\" IN(1, 4) AND SUBSTR";
        //        strQry += "  (CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.Zone + ") = '" + ZoneCode + "'";
        //        if (sFromDate != "" && sTodate != "")
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == "")
        //        {

        //            sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate == string.Empty && sTodate != "")
        //        {
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == string.Empty)
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "'";
        //        }

        //        strQry += " GROUP BY \"DF_ID\",\"DF_DATE\", \"TR_DECOMM_DATE\",\"DF_LOC_CODE\", \"DF_DTC_CODE\", \"FD_FEEDER_NAME\") A ";
        //        strQry += " GROUP BY \"ZONE\",\"ZoneCode\",\"CIRCLE\",\"CircleCode\" ORDER BY \"ZoneCode\")A FULL OUTER JOIN   (SELECT COALESCE(cast(\"ZoneCode\" as INTEGER),0) AS \"ZoneCode1\", \"ZONE\" AS \"ZONE1\",COALESCE(cast(\"CircleCode\" as INTEGER),0) AS \"CircleCode1\",\"CIRCLE\" AS \"CIRCLE1\",";
        //        strQry += " COALESCE(SUM(\"LESSTHAN1\"),0) \"LESSTHAN1DAYNEW\",COALESCE(SUM(\"BETWEEN1TO7\"),0)\"BW1TO7NEW\",  COALESCE(SUM(\"BETWEEN7TO15\"),0)\"BW7TO15NEW\", ";
        //        strQry += " COALESCE(SUM(\"BETWEEN15TO30\"),0) \"BW15TO30NEW\",COALESCE(SUM(\"ABOVE30\"),0)\"ABOVE30NEW\",COALESCE(SUM(\"TOTAL\"),0)\"TOTALNEW\" FROM   ";
        //        strQry += "(SELECT \"IN_NO\",(SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE    CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(CAST (\"DF_LOC_CODE\" AS text), 1," + Constants.Zone + "))\"ZONE\", SUBSTR(CAST(\"DF_LOC_CODE\" as text), 1, " + Constants.Zone + ")\"ZoneCode\",(SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE    CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(CAST";
        //        strQry += " (\"DF_LOC_CODE\" AS text), 1, " + Constants.Circle + "))\"CIRCLE\", SUBSTR(CAST(\"DF_LOC_CODE\" as text), 1, " + Constants.Circle + ")\"CircleCode\",   CASE WHEN   (\"IN_DATE\" - \"DF_DATE\")  BETWEEN 0 AND 1 THEN ";
        //        strQry += " COUNT(*) ELSE 0 END \"LESSTHAN1\", CASE WHEN(\"IN_DATE\" - \"DF_DATE\") BETWEEN 2 AND 7 THEN  COUNT(*) ELSE 0 END \"BETWEEN1TO7\", ";
        //        strQry += " CASE WHEN(\"IN_DATE\" - \"DF_DATE\")  BETWEEN 8 AND 15 THEN    COUNT(*) ELSE 0 END \"BETWEEN7TO15\", CASE WHEN(\"IN_DATE\" -\"DF_DATE\")  ";
        //        strQry += " BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0 END \"BETWEEN15TO30\", CASE WHEN(\"IN_DATE\" - \"DF_DATE\") > 30 THEN  COUNT(\"DF_DTC_CODE\") ";
        //        strQry += " ELSE 0 END \"ABOVE30\",\"FD_FEEDER_NAME\",COUNT(*) \"TOTAL\"  FROM \"TBLFEEDERMAST\" INNER JOIN \"TBLDTCMAST\" ON  \"DT_FDRSLNO\" = \"FD_FEEDER_CODE\" ";
        //        strQry += " INNER JOIN \"TBLDTCFAILURE\" ON \"DF_DTC_CODE\" = \"DT_CODE\" INNER JOIN   \"TBLWORKORDER\" ON \"DF_ID\" = \"WO_DF_ID\" INNER JOIN \"TBLINDENT\" ";
        //        strQry += "  ON \"WO_SLNO\" = \"TI_WO_SLNO\" INNER JOIN \"TBLDTCINVOICE\"  ON \"TI_ID\" = \"IN_TI_NO\" AND \"DF_REPLACE_FLAG\" <> 1 AND \"DF_STATUS_FLAG\"   IN(1, 4)";

        //        if (sFromDate != "" && sTodate != "")
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == "")
        //        {

        //            sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate == string.Empty && sTodate != "")
        //        {
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == string.Empty)
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "'";
        //        }
        //        strQry += "  GROUP BY \"DF_ID\",\"DF_DATE\",\"IN_NO\",\"IN_DATE\", ";
        //        strQry += " \"DF_LOC_CODE\",\"DF_DTC_CODE\",\"FD_FEEDER_NAME\") a LEFT JOIN \"TBLTCREPLACE\"    ON \"IN_NO\" = \"TR_IN_NO\"  where \"IN_NO\" IS NOT NULL AND \"TR_IN_NO\" IS ";
        //        strQry += "NULL GROUP BY \"ZONE\",\"ZoneCode\",\"CIRCLE\", \"CircleCode\" ORDER BY \"ZoneCode\")B  ON A.\"CircleCode\" = B.\"CircleCode1\" ";
        //        dtDetails = objcon.FetchDataTable(strQry);
        //        return dtDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return dtDetails;

        //    }

        //}

        //public DataTable LoadAllDetails(string sFromDate, string sTodate)
        //{
        //    DataTable dtDetails = new DataTable();
        //    try
        //    {

        //        strQry = " SELECT NVL(CircleCode, CircleCode1)CIRCLECODE, NVL(CIRCLE, CIRCLE1)CIRCLE, NVL(DivisionCode, DivisionCode1)DIVISIONCODE, ";
        //        strQry += " NVL(Division, DivisionName1)DIVISION, NVL(SUBDIVISION, SubDivisionName1)SUBDIVISION, NVL(SubDivisionCode, SubDivisionCode1) ";
        //        strQry += " SUBDIVISIONCODE, NVL(sectioncode, sectioncode1)SECTIONCODE, NVL(SECTION, SectionName1)SECTION, NVL(LESSTHAN1DAY, 0)LESSTHAN1DAY, ";
        //        strQry += " NVL(BW1TO7, 0)BW1TO7, NVL(BW7TO15, 0)BW7TO15, NVL(BW15TO30, 0)BW15TO30, NVL(ABOVE30, 0)ABOVE30, NVL(TOTAL, 0)TOTAL,";
        //        strQry += " NVL(LESSTHAN1DAYNEW, 0)LESSTHAN1DAYNEW, NVL(BW1TO7NEW, 0)BW1TO7NEW, NVL(BW7TO15NEW, 0)BW7TO15NEW, NVL(BW15TO30NEW, 0)BW15TO30NEW, ";
        //        strQry += " NVL(ABOVE30NEW, 0)ABOVE30NEW, NVL(TOTALNEW, 0)TOTALNEW  FROM(SELECT CircleCode, CIRCLE, DivisionCode, DivisionName as Division,";
        //        strQry += " SubDivisionName as SUBDIVISION, SubDivisionCode, Sectionname as SECTION, sectioncode, SUM(LESSTHAN1)LESSTHAN1DAY, ";
        //        strQry += " SUM(BETWEEN1TO7)BW1TO7, SUM(BETWEEN7TO15)BW7TO15, SUM(BETWEEN15TO30)BW15TO30, SUM(ABOVE30)ABOVE30, SUM(TOTAL)TOTAL ";
        //        strQry += " fROM(SELECT(SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1)  FROM VIEW_ALL_OFFICES WHERE  OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 1)) ";
        //        strQry += " CIRCLE, SUBSTR(DF_LOC_CODE, 0, 1)CircleCode, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES   ";
        //        strQry += " WHERE  OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 2)) DivisionName, SUBSTR(DF_LOC_CODE, 0, 2)DivisionCode, (SELECT SUBSTR(OFF_NAME, ";
        //        strQry += " INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES WHERE  OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 3))  SubDivisionName, ";
        //        strQry += " SUBSTR(DF_LOC_CODE, 0, 3)SubDivisionCode, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1)   FROM VIEW_ALL_OFFICES ";
        //        strQry += "  WHERE OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 4)) SectionName, SUBSTR(DF_LOC_CODE, 0, 4)sectioncode,   CASE WHEN(TR_DECOMM_DATE - DF_DATE)";
        //        strQry += " BETWEEN 0 AND 1 THEN  COUNT(*) ELSE 0 END LESSTHAN1, CASE   WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 2 AND 7 THEN  ";
        //        strQry += "  COUNT(*) ELSE 0 END  BETWEEN1TO7, CASE   WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 8 AND 15 THEN  COUNT(*) ELSE 0 ";
        //        strQry += " END BETWEEN7TO15, CASE    WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0 END BETWEEN15TO30,";
        //        strQry += " CASE  WHEN(TR_DECOMM_DATE - DF_DATE) > 30 THEN  COUNT(DF_DTC_CODE) ELSE 0 END ABOVE30, FD_FEEDER_NAME, COUNT(*) TOTAL ";
        //        strQry += " FROM TBLFEEDERMAST INNER JOIN TBLDTCMAST ON  DT_FDRSLNO = FD_FEEDER_CODE INNER JOIN TBLDTCFAILURE    ";
        //        strQry += "  ON DF_DTC_CODE = DT_CODE INNER JOIN   TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN  TBLINDENT ON    ";
        //        strQry += " WO_SLNO = TI_WO_SLNO INNER JOIN TBLDTCINVOICE  ON TI_ID = IN_TI_NO INNER JOIN TBLTCREPLACE ON   ";
        //        strQry += " IN_NO = TR_IN_NO AND DF_REPLACE_FLAG <> 1  AND DF_STATUS_FLAG IN(1, 4)  ";
        //        strQry += " GROUP BY DF_ID, DF_DATE, TR_DECOMM_DATE, DF_LOC_CODE, DF_DTC_CODE, FD_FEEDER_NAME) A ";
        //        if (sFromDate != "" && sTodate != "")
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            sFromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            sTodate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == "")
        //        {

        //            sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            sFromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            sTodate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        else if (sFromDate == string.Empty && sTodate != "")
        //        {
        //            strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        strQry += " GROUP BY CIRCLE, CircleCode, ";
        //        strQry += " DivisionCode, DivisionName, SubDivisionName, SubDivisionCode, SectionName, sectioncode ORDER BY CircleCode,  ";
        //        strQry += " DivisionCode,SubDivisionCode)A full outer JOIN   (SELECT DivisionCode1, DivisionName1, SubDivisionName1, SubDivisionCode1,";
        //        strQry += " SectionName1, sectioncode1, CircleCode AS  CircleCode1, CIRCLE AS CIRCLE1, SUM(LESSTHAN1)LESSTHAN1DAYNEW, ";
        //        strQry += " SUM(BETWEEN1TO7)BW1TO7NEW, SUM(BETWEEN7TO15)BW7TO15NEW, SUM(BETWEEN15TO30)BW15TO30NEW, SUM(ABOVE30)ABOVE30NEW, ";
        //        strQry += " SUM(TOTAL)TOTALNEW FROM(SELECT IN_NO, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES ";
        //        strQry += " WHERE  OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 1))CIRCLE, SUBSTR(DF_LOC_CODE, 0, 1)CircleCode, (SELECT  ";
        //        strQry += " SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES   WHERE OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 2)) ";
        //        strQry += " DivisionName1,SUBSTR(DF_LOC_CODE, 0, 2)DivisionCode1,   (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM ";
        //        strQry += " VIEW_ALL_OFFICES WHERE OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 3))   SubDivisionName1, SUBSTR(DF_LOC_CODE, 0, 3)SubDivisionCode1,";
        //        strQry += " (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1)   FROM VIEW_ALL_OFFICES WHERE OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 4))  ";
        //        strQry += " SectionName1, SUBSTR(DF_LOC_CODE, 0, 4)  sectioncode1,  CASE WHEN  (IN_DATE - DF_DATE)  BETWEEN 0 AND 1 THEN COUNT(*) ";
        //        strQry += " ELSE 0 END LESSTHAN1, CASE WHEN(IN_DATE - DF_DATE)  BETWEEN 2 AND 7 THEN  COUNT(*) ELSE 0 END BETWEEN1TO7,";
        //        strQry += " CASE WHEN(IN_DATE - DF_DATE)  BETWEEN 8 AND 15 THEN  COUNT(*) ELSE 0 END BETWEEN7TO15, CASE WHEN(IN_DATE - DF_DATE) ";
        //        strQry += " BETWEEN 16 AND 30 THEN    COUNT(*) ELSE 0 END BETWEEN15TO30, CASE   WHEN(IN_DATE - DF_DATE) > 30 THEN ";
        //        strQry += " COUNT(DF_DTC_CODE) ELSE 0 END ABOVE30, FD_FEEDER_NAME, COUNT(*) TOTAL  FROM TBLFEEDERMAST INNER JOIN TBLDTCMAST ON ";
        //        strQry += " DT_FDRSLNO = FD_FEEDER_CODE INNER JOIN   TBLDTCFAILURE   ON DF_DTC_CODE = DT_CODE INNER JOIN   TBLWORKORDER ON ";
        //        strQry += "  DF_ID = WO_DF_ID INNER JOIN  TBLINDENT ON   WO_SLNO = TI_WO_SLNO INNER JOIN TBLDTCINVOICE  ON TI_ID = IN_TI_NO AND";
        //        strQry += "  DF_REPLACE_FLAG <> 1 AND  DF_STATUS_FLAG  IN(1, 4)  ";
        //        if (sFromDate != "" && sTodate != "")
        //        {
        //            //DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            //sFromDate = DFromDate.ToString("yyyyMMdd");
        //            //DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            //sTodate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == "")
        //        {

        //            //sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //            //DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            //sFromDate = DFromDate.ToString("yyyyMMdd");
        //            //DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            //sTodate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        else if (sFromDate == string.Empty && sTodate != "")
        //        {
        //            strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        strQry += " GROUP BY DF_ID, DF_DATE, IN_NO, IN_DATE, DF_LOC_CODE,";
        //        strQry += " DF_DTC_CODE, FD_FEEDER_NAME)  LEFT JOIN TBLTCREPLACE ON IN_NO = TR_IN_NO  where IN_NO IS NOT NULL AND TR_IN_NO IS NULL";
        //        strQry += " GROUP BY CIRCLE, CircleCode, DivisionCode1, DivisionName1, SubDivisionName1, SubDivisionCode1, SectionName1,";
        //        strQry += " sectioncode1 ORDER BY CircleCode,DivisionCode1)B on   A.CircleCode = B.CircleCode1 and A.DivisionCode = B.DivisionCode1  ";
        //        strQry += "  and A.SubDivisionCode = b.SubDivisionCode1   and A.sectioncode = B.sectioncode1 ORDER BY";
        //        strQry += "  NVL(sectioncode, sectioncode1)";
        //        dtDetails = objcon.FetchDataTable(strQry);
        //        return dtDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadCircleDetails");
        //        return dtDetails;

        //    }

        //}



        //old code for export excel
        //public DataTable LoadAllDetails(string sFromDate, string sTodate)
        //{
        //    DataTable dtDetails = new DataTable();
        //    try
        //    {

        //        strQry = " SELECT COALESCE(\"CircleCode\", \"CircleCode1\")\"CIRCLECODE\", COALESCE(\"CIRCLE\",\"CIRCLE1\")CIRCLE, COALESCE(\"DivisionCode\",\"DivisionCode1\")\"DIVISIONCODE\", ";
        //        strQry += " COALESCE(\"Division\", \"DivisionName1\")\"DIVISION\", COALESCE(\"SUBDIVISION\",\"SubDivisionName1\")\"SUBDIVISION\", COALESCE(\"SubDivisionCode\",\"SubDivisionCode1\") ";
        //        strQry += " \"SUBDIVISIONCODE\", COALESCE(\"sectioncode\", \"sectioncode1\")\"SECTIONCODE\", COALESCE(\"SECTION\", \"SectionName1\")\"SECTION\", COALESCE(\"LESSTHAN1DAY\", 0)\"LESSTHAN1DAY\", ";
        //        strQry += " COALESCE(BW1TO7, 0)BW1TO7, COALESCE(BW7TO15, 0)BW7TO15, COALESCE(BW15TO30, 0)BW15TO30, COALESCE(ABOVE30, 0)ABOVE30, COALESCE(\"TOTAL\", 0)\"TOTAL\",";
        //        strQry += " COALESCE(\"LESSTHAN1DAYNEW\", 0)\"LESSTHAN1DAYNEW\", COALESCE(\"BW1TO7NEW\", 0)\"BW1TO7NEW\", COALESCE(\"BW7TO15NEW\", 0)\"BW7TO15NEW\", COALESCE(\"BW15TO30NEW\", 0)\"BW15TO30NEW\", ";
        //        strQry += " COALESCE(\"ABOVE30NEW\", 0)\"ABOVE30NEW\", COALESCE(\"TOTALNEW\", 0)\"TOTALNEW\"  FROM(SELECT \"CircleCode\", \"CIRCLE\",\"DivisionCode\", \"DivisionName\" as \"Division\",";
        //        strQry += " \"SubDivisionName\" as \"SUBDIVISION\", \"SubDivisionCode\", \"Sectionname\" as \"SECTION\", \"sectioncode\", SUM(\"LESSTHAN1\")\"LESSTHAN1DAY\", ";
        //        strQry += " SUM(\"BETWEEN1TO7\")\"BW1TO7\", SUM(\"BETWEEN7TO15\")\"BW7TO15\", SUM(\"BETWEEN15TO30\")\"BW15TO30\", SUM(\"ABOVE30\")\"ABOVE30\", SUM(\"TOTAL\")\"TOTAL\" ";
        //        strQry += " fROM(SELECT(SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1)  FROM \"VIEW_ALL_OFFICES\" WHERE  CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DF_LOC_CODE\" as text), 1, " + Constants.Circle + ")) ";
        //        strQry += " \"CIRCLE\", SUBSTR(cast(\"DF_LOC_CODE\" as text), 1,  " + Constants.Circle + ")\"CircleCode\", (SELECT SUBSTR(\"OFF_NAME\", INSTR(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\"";
        //        strQry += " WHERE  CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DF_LOC_CODE\" as text),1,  " + Constants.Division + ")) \"DivisionName\", SUBSTR(cast(\"DF_LOC_CODE\" as text), 1,  " + Constants.Division + ")\"DivisionCode\", (SELECT SUBSTR(\"OFF_NAME, ";
        //        strQry += " INSTR(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE  CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DF_LOC_CODE\" as text), 1,  " + Constants.SubDivision + ")) \"SubDivisionName\", ";
        //        strQry += " SUBSTR(cast(\"DF_LOC_CODE\" as text), 1, " + Constants.SubDivision + ")\"SubDivisionCode\", (SELECT SUBSTR(\"OFF_NAME\" , INSTR(\"OFF_NAME\", ':') + 1)   FROM \"VIEW_ALL_OFFICES\" ";
        //        strQry += "  WHERE CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DF_LOC_CODE\" as text) , 1, " + Constants.Section + ") SectionName, SUBSTR(cast(DF_LOC_CODE  as text), 1, " + Constants.Section + ")sectioncode,   CASE WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")";
        //        strQry += " BETWEEN 0 AND 1 THEN  COUNT(*) ELSE 0 END \"LESSTHAN1\", CASE   WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")  BETWEEN 2 AND 7 THEN  ";
        //        strQry += "  COUNT(*) ELSE 0 END  \"BETWEEN1TO7\", CASE   WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")  BETWEEN 8 AND 15 THEN  COUNT(*) ELSE 0 ";
        //        strQry += " END \"BETWEEN7TO15\", CASE WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")  BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0 END \"BETWEEN15TO30\",";
        //        strQry += " CASE  WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\") > 30 THEN  COUNT(\"DF_DTC_CODE\") ELSE 0 END \"ABOVE30\", \"FD_FEEDER_NAME\", COUNT(*) \"TOTAL\" ";
        //        strQry += " FROM \"TBLFEEDERMAST\" INNER JOIN \"TBLDTCMAST\" ON  \"DT_FDRSLNO\" = \"FD_FEEDER_CODE\" INNER JOIN \"TBLDTCFAILURE\"    ";
        //        strQry += "  ON \"DF_DTC_CODE\" = \"DT_CODE\" INNER JOIN   \"TBLWORKORDER\" ON \"DF_ID\" = \"WO_DF_ID\" INNER JOIN  \"TBLINDENT\" ON    ";
        //        strQry += " \"WO_SLNO\" = \"TI_WO_SLNO\" INNER JOIN \"TBLDTCINVOICE\"  ON \"TI_ID\" = \"IN_TI_NO\" INNER JOIN \"TBLTCREPLACE\" ON   ";
        //        strQry += " \"IN_NO\" = \"TR_IN_NO\" AND \"DF_REPLACE_FLAG\" <> 1  AND \"DF_STATUS_FLAG\" IN(1, 4)  ";
        //        strQry += " GROUP BY \"DF_ID\",\"DF_DATE\", \"TR_DECOMM_DATE\", \"DF_LOC_CODE\", \"DF_DTC_CODE\", \"FD_FEEDER_NAME\") A ";
        //        if (sFromDate != "" && sTodate != "")
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == "")
        //        {

        //            sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate == string.Empty && sTodate != "")
        //        {
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == string.Empty)
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "'";
        //        }
        //        strQry += " GROUP BY \"CIRCLE\",\"CircleCode\", ";
        //        strQry += " \"DivisionCode\",\"DivisionName\",\"SubDivisionName\",\"SubDivisionCode\",\"SectionName\",\"sectioncode\" ORDER BY \"CircleCode\",  ";
        //        strQry += " DivisionCode,SubDivisionCode)A FULL OUTER JOIN   (SELECT \"DivisionCode1\",\"DivisionName1\",\"SubDivisionName1\",\"SubDivisionCode1\",";
        //        strQry += " \"SectionName1\",\"sectioncode1\",\"CircleCode\" AS  \"CircleCode1\", \"CIRCLE\" AS \"CIRCLE1\", SUM(\"LESSTHAN1\")\"LESSTHAN1DAYNEW\", ";
        //        strQry += " SUM(\"BETWEEN1TO7\")\"BW1TO7NEW\", SUM(\"BETWEEN7TO15\")\"BW7TO15NEW\", SUM(\"BETWEEN15TO30\")\"BW15TO30NEW\", SUM(\"ABOVE30\")\"ABOVE30NEW\", ";
        //        strQry += " SUM(\"TOTAL\")TOTALNEW FROM(SELECT \"IN_NO\", (SELECT SUBSTR(\"OFF_NAME\", INSTR(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" ";
        //        strQry += " WHERE  CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DF_LOC_CODE\" as text), 1, " + Constants.Circle + ")) \"CIRCLE\", SUBSTR(cast(\"DF_LOC_CODE\" as text), 1,  " + Constants.Circle + ")\"CircleCode\", (SELECT ";
        //        strQry += " SUBSTR(\"OFF_NAME\", INSTR(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE  CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DF_LOC_CODE\" as text),1,  " + Constants.Division + ")) ";
        //        strQry += " \"DivisionName1\",SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.Division + ")\"DivisionCode1\",(SELECT SUBSTR(\"OFF_NAME\", INSTR(\"OFF_NAME\", ':') + 1) FROM ";
        //        strQry += " \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TXET) = SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.SubDivision + "))\"SubDivisionName1\", SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.SubDivision + ")SubDivisionCode1,";
        //        strQry += " (SELECT SUBSTR(\"OFF_NAME\", INSTR(\"OFF_NAME\", ':') + 1)   FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT)= SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.Section + "))  ";
        //        strQry += " \"SectionName1\", SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.Section + ")  \"sectioncode1\",  CASE WHEN  (\"IN_DATE\" - \"DF_DATE\")  BETWEEN 0 AND 1 THEN COUNT(*) ";
        //        strQry += " ELSE 0 END \"LESSTHAN1\", CASE WHEN(\"IN_DATE\" - \"DF_DATE\")  BETWEEN 2 AND 7 THEN  COUNT(*) ELSE 0 END BETWEEN1TO7,";
        //        strQry += " CASE WHEN(\"IN_DATE\" - \"DF_DATE\")  BETWEEN 8 AND 15 THEN  COUNT(*) ELSE 0 END \"BETWEEN7TO15\", CASE WHEN(\"IN_DATE\" - \"DF_DATE\") ";
        //        strQry += " BETWEEN 16 AND 30 THEN    COUNT(*) ELSE 0 END \"BETWEEN15TO30\", CASE   WHEN(\"IN_DATE\" - \"DF_DATE\") > 30 THEN ";
        //        strQry += " COUNT(\"DF_DTC_CODE\") ELSE 0 END \"ABOVE30\", \"FD_FEEDER_NAME\", COUNT(*) \"TOTAL\"  FROM \"TBLFEEDERMAST\" INNER JOIN \"TBLDTCMAST\" ON ";
        //        strQry += " \"DT_FDRSLNO\" = \"FD_FEEDER_CODE\" INNER JOIN   \"TBLDTCFAILURE\"   ON \"DF_DTC_CODE\" = \"DT_CODE\" INNER JOIN  \"TBLWORKORDER\" ON ";
        //        strQry += " \"DF_ID\" = \"WO_DF_ID\" INNER JOIN  \"TBLINDENT\" ON  \"WO_SLNO\" = \"TI_WO_SLNO\" INNER JOIN \"TBLDTCINVOICE\"  ON \"TI_ID\" = \"IN_TI_NO\" AND";
        //        strQry += "  \"DF_REPLACE_FLAG\" <> 1 AND  \"DF_STATUS_FLAG\"  IN(1, 4)  ";
        //        if (sFromDate != "" && sTodate != "")
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == "")
        //        {

        //            sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate == string.Empty && sTodate != "")
        //        {
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == string.Empty)
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' ";
        //        }
        //        strQry += " GROUP BY \"DF_ID\", \"DF_DATE\", \"IN_NO\", \"IN_DATE\",\"DF_LOC_CODE\",";
        //        strQry += " \"DF_DTC_CODE\",\"FD_FEEDER_NAME\")LEFT JOIN \"TBLTCREPLACE\" ON \"IN_NO\" = \"TR_IN_NO\"  where \"IN_NO\" IS NOT NULL AND \"TR_IN_NO\" IS NULL";
        //        strQry += " GROUP BY \"CIRCLE\", \"CircleCode\", \"DivisionCode1\", \"DivisionName1\", \"SubDivisionName1\", \"SubDivisionCode1\", \"SectionName1\",";
        //        strQry += " \"sectioncode1\" ORDER BY \"CircleCode\",\"DivisionCode1\")B on   A.\"CircleCode\" = B.\"CircleCode1\" and A.\"DivisionCode\" = B.\"DivisionCode1\"  ";
        //        strQry += "  and A.\"SubDivisionCode\" = b.\"SubDivisionCode1\"   and A.\"sectioncode\" = B.\"sectioncode1\" ORDER BY";
        //        strQry += "  COALESCE(\"sectioncode\",\"sectioncode1\")";
        //        dtDetails = objcon.FetchDataTable(strQry);
        //        return dtDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return dtDetails;

        //    }

        //}
        #endregion



        //new query for export excel
        /// <summary>
        /// Get failure replacement time line  report in excel
        /// </summary>
        /// <param name="sFromDate"></param>
        /// <param name="sTodate"></param>
        /// <returns></returns>
        public DataTable LoadAllDetails(string sFromDate, string sTodate)
        {
            DataTable dtDetails = new DataTable();
            try
            {
                #region Inline Queries
                ////strQry = " SELECT COALESCE(\"CircleCode\", \"CircleCode1\")\"CIRCLECODE\", COALESCE(\"CIRCLE\",\"CIRCLE1\") \"CIRCLE\", COALESCE(\"DivisionCode\",\"DivisionCode1\")\"DIVISIONCODE\", ";
                ////strQry += " COALESCE(\"Division\", \"DivisionName1\")\"DIVISION\", COALESCE(\"SubDivisionCode\",\"SubDivisionCode1\")  \"SUBDIVISIONCODE\", COALESCE(\"SUBDIVISION\",\"SubDivisionName1\")\"SUBDIVISION\", ";
                ////strQry += " COALESCE(\"sectioncode\", \"sectioncode1\")\"SECTIONCODE\", COALESCE(\"SECTION\", \"SectionName1\")\"SECTION\", COALESCE(\"LESSTHAN1DAY\", 0)\"LESSTHAN1DAY\",  COALESCE(\"BW1TO7\", 0) \"BW1TO7\", ";
                ////strQry += " COALESCE(\"BW7TO15\", 0)\"BW7TO15\", COALESCE(\"BW15TO30\", 0)\"BW15TO30\", COALESCE(\"ABOVE30\", 0)\"ABOVE30\", COALESCE(\"TOTAL\", 0)\"TOTAL\", COALESCE(\"LESSTHAN1DAYNEW\", 0)\"LESSTHAN1DAYNEW\", ";
                ////strQry += " COALESCE(\"BW1TO7NEW\", 0)\"BW1TO7NEW\", COALESCE(\"BW7TO15NEW\", 0)\"BW7TO15NEW\", COALESCE(\"BW15TO30NEW\", 0)\"BW15TO30NEW\",  COALESCE(\"ABOVE30NEW\", 0)\"ABOVE30NEW\", COALESCE(\"TOTALNEW\", 0)\"TOTALNEW\" ";
                ////strQry += " FROM(SELECT \"CircleCode\", \"CIRCLE\",\"DivisionCode\", \"DivisionName\" as \"Division\", \"SubDivisionName\" as \"SUBDIVISION\", \"SubDivisionCode\", \"SectionName\" as \"SECTION\", \"sectioncode\", ";
                ////strQry += " SUM(\"LESSTHAN1\")\"LESSTHAN1DAY\",  SUM(\"BETWEEN1TO7\")\"BW1TO7\", SUM(\"BETWEEN7TO15\")\"BW7TO15\", SUM(\"BETWEEN15TO30\")\"BW15TO30\", SUM(\"ABOVE30\")\"ABOVE30\", SUM(\"TOTAL\")\"TOTAL\" ";
                ////strQry += " FROM ( SELECT(SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1)  FROM \"VIEW_ALL_OFFICES\" WHERE  CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DF_LOC_CODE\" as text), 1, " + Constants.Circle + "))  \"CIRCLE\", ";
                ////strQry += " SUBSTR(cast(\"DF_LOC_CODE\" as text), 1, " + Constants.Circle + ")\"CircleCode\", (SELECT SUBSTR(\"OFF_NAME\", strpos(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE  CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DF_LOC_CODE\" as text),1, " + Constants.Division + ")) \"DivisionName\", ";
                ////strQry += " SUBSTR(cast(\"DF_LOC_CODE\" as text), 1, " + Constants.Division + ")\"DivisionCode\", (SELECT SUBSTR(\"OFF_NAME\",  strpos(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE  CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DF_LOC_CODE\" as text), 1,  " + Constants.SubDivision + ")) \"SubDivisionName\", ";
                ////strQry += " SUBSTR(cast(\"DF_LOC_CODE\" as text), 1, " + Constants.SubDivision + ")\"SubDivisionCode\", (SELECT SUBSTR(\"OFF_NAME\" , strpos(\"OFF_NAME\", ':') + 1)   FROM \"VIEW_ALL_OFFICES\"   WHERE CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DF_LOC_CODE\" as text) , 1, " + Constants.Section + ")) \"SectionName\", ";
                ////strQry += " SUBSTR(cast(\"DF_LOC_CODE\"  as text), 1, " + Constants.Section + ")sectioncode,   CASE WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\") BETWEEN 0 AND 1 THEN  COUNT(*) ELSE 0 END \"LESSTHAN1\", CASE   WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")  BETWEEN 2 AND 7 THEN  ";
                ////strQry += " COUNT(*) ELSE 0 END  \"BETWEEN1TO7\", CASE   WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")  BETWEEN 8 AND 15 THEN  COUNT(*) ELSE 0  END \"BETWEEN7TO15\", CASE WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")  BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0 END \"BETWEEN15TO30\", ";
                ////strQry += " CASE  WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\") > 30 THEN  COUNT(\"DF_DTC_CODE\") ELSE 0 END \"ABOVE30\", \"FD_FEEDER_NAME\", COUNT(*) \"TOTAL\"  FROM \"TBLFEEDERMAST\" INNER JOIN \"TBLDTCMAST\" ON  \"DT_FDRSLNO\" = \"FD_FEEDER_CODE\" INNER JOIN \"TBLDTCFAILURE\"  ";
                ////strQry += " ON \"DF_DTC_CODE\" = \"DT_CODE\" INNER JOIN   \"TBLWORKORDER\" ON \"DF_ID\" = \"WO_DF_ID\" INNER JOIN  \"TBLINDENT\" ON  \"WO_SLNO\" = \"TI_WO_SLNO\" INNER JOIN \"TBLDTCINVOICE\"  ON \"TI_ID\" = \"IN_TI_NO\" INNER JOIN \"TBLTCREPLACE\" ON    \"IN_NO\" = \"TR_IN_NO\" AND ";
                ////strQry += " \"DF_REPLACE_FLAG\" <> 1  AND \"DF_STATUS_FLAG\" IN(1, 4) ";


                ////if (sFromDate != "" && sTodate != "")
                ////{
                ////    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                ////    string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
                ////    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                ////    string sFinal_Todate = DToDate.ToString("yyyyMMdd");
                ////    strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
                ////}
                ////else if (sFromDate != "" && sTodate == "")
                ////{

                ////    sTodate = DateTime.Now.ToString("dd/MM/yyyy");
                ////    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                ////    string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
                ////    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                ////    string sFinal_Todate = DToDate.ToString("yyyyMMdd");
                ////    strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
                ////}
                ////else if (sFromDate == string.Empty && sTodate != "")
                ////{
                ////    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                ////    string sFinal_Todate = DToDate.ToString("yyyyMMdd");
                ////    strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
                ////}
                ////else if (sFromDate != "" && sTodate == string.Empty)
                ////{
                ////    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                ////    string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
                ////    strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "'";
                ////}
                ////strQry += " GROUP BY \"DF_ID\",\"DF_DATE\", \"TR_DECOMM_DATE\", \"DF_LOC_CODE\", \"DF_DTC_CODE\", \"FD_FEEDER_NAME\" ) A  ";
                ////strQry += "  GROUP BY \"CIRCLE\",\"CircleCode\",  \"DivisionCode\",\"DivisionName\",\"SubDivisionName\",\"SubDivisionCode\",\"SectionName\",\"sectioncode\" ORDER BY \"CircleCode\", ";
                ////strQry += " \"DivisionCode\",\"SubDivisionCode\")A FULL OUTER JOIN   (SELECT \"DivisionCode1\",\"DivisionName1\",\"SubDivisionName1\",\"SubDivisionCode1\", \"SectionName1\",\"sectioncode1\", ";
                ////strQry += " \"CircleCode\" AS  \"CircleCode1\", \"CIRCLE\" AS \"CIRCLE1\", SUM(\"LESSTHAN1\")\"LESSTHAN1DAYNEW\",  SUM(\"BETWEEN1TO7\")\"BW1TO7NEW\", SUM(\"BETWEEN7TO15\")\"BW7TO15NEW\", ";
                ////strQry += " SUM(\"BETWEEN15TO30\")\"BW15TO30NEW\", SUM(\"ABOVE30\")\"ABOVE30NEW\",  SUM(\"TOTAL\") \"TOTALNEW\" FROM ( SELECT \"IN_NO\", (SELECT SUBSTR(\"OFF_NAME\", strpos(\"OFF_NAME\", ':') + 1) ";
                ////strQry += " FROM \"VIEW_ALL_OFFICES\"  WHERE  CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DF_LOC_CODE\" as text), 1, " + Constants.Circle + ")) \"CIRCLE\", SUBSTR(cast(\"DF_LOC_CODE\" as text), 1,  " + Constants.Circle + ")\"CircleCode\", ";
                ////strQry += " (SELECT  SUBSTR(\"OFF_NAME\", strpos(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE  CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DF_LOC_CODE\" as text),1,  " + Constants.Division + ")) ";
                ////strQry += " \"DivisionName1\",SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.Division + ")\"DivisionCode1\",(SELECT SUBSTR(\"OFF_NAME\", strpos(\"OFF_NAME\", ':') + 1) FROM  \"VIEW_ALL_OFFICES\" ";
                ////strQry += " WHERE CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.SubDivision + "))\"SubDivisionName1\", SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.SubDivision + ") \"SubDivisionCode1\", ";
                ////strQry += " (SELECT SUBSTR(\"OFF_NAME\", strpos(\"OFF_NAME\", ':') + 1)   FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT)= SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.Section + ")) \"SectionName1\", ";
                ////strQry += " SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.Section + ")  \"sectioncode1\",  CASE WHEN  (\"IN_DATE\" - \"DF_DATE\")  BETWEEN 0 AND 1 THEN COUNT(*)  ELSE 0 END \"LESSTHAN1\", CASE WHEN(\"IN_DATE\" - \"DF_DATE\") ";
                ////strQry += " BETWEEN 2 AND 7 THEN  COUNT(*) ELSE 0 END \"BETWEEN1TO7\", CASE WHEN(\"IN_DATE\" - \"DF_DATE\")  BETWEEN 8 AND 15 THEN  COUNT(*) ELSE 0 END \"BETWEEN7TO15\", CASE WHEN(\"IN_DATE\" - \"DF_DATE\") ";
                ////strQry += " BETWEEN 16 AND 30 THEN    COUNT(*) ELSE 0 END \"BETWEEN15TO30\", CASE   WHEN(\"IN_DATE\" - \"DF_DATE\") > 30 THEN  COUNT(\"DF_DTC_CODE\") ELSE 0 END \"ABOVE30\", \"FD_FEEDER_NAME\", COUNT(*) \"TOTAL\" ";
                ////strQry += " FROM \"TBLFEEDERMAST\" INNER JOIN \"TBLDTCMAST\" ON  \"DT_FDRSLNO\" = \"FD_FEEDER_CODE\" INNER JOIN   \"TBLDTCFAILURE\"   ON \"DF_DTC_CODE\" = \"DT_CODE\" INNER JOIN  \"TBLWORKORDER\" ON  \"DF_ID\" = \"WO_DF_ID\" ";
                ////strQry += " INNER JOIN  \"TBLINDENT\" ON  \"WO_SLNO\" = \"TI_WO_SLNO\" INNER JOIN \"TBLDTCINVOICE\"  ON \"TI_ID\" = \"IN_TI_NO\" AND  \"DF_REPLACE_FLAG\" <> 1 AND  \"DF_STATUS_FLAG\"  IN(1, 4)  ";


                ////if (sFromDate != "" && sTodate != "")
                ////{
                ////    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                ////    string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
                ////    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                ////    string sFinal_Todate = DToDate.ToString("yyyyMMdd");
                ////    strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
                ////}
                ////else if (sFromDate != "" && sTodate == "")
                ////{

                ////    sTodate = DateTime.Now.ToString("dd/MM/yyyy");
                ////    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                ////    string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
                ////    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                ////    string sFinal_Todate = DToDate.ToString("yyyyMMdd");
                ////    strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
                ////}
                ////else if (sFromDate == string.Empty && sTodate != "")
                ////{
                ////    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                ////    string sFinal_Todate = DToDate.ToString("yyyyMMdd");
                ////    strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
                ////}
                ////else if (sFromDate != "" && sTodate == string.Empty)
                ////{
                ////    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                ////    string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
                ////    strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' ";
                ////}
                ////strQry += "  GROUP BY \"DF_ID\", \"DF_DATE\", \"IN_NO\", \"IN_DATE\",\"DF_LOC_CODE\", \"DF_DTC_CODE\",\"FD_FEEDER_NAME\")S   LEFT JOIN \"TBLTCREPLACE\" ON \"IN_NO\" = \"TR_IN_NO\"  where ";
                ////strQry += " \"IN_NO\" IS NOT NULL AND \"TR_IN_NO\" IS NULL GROUP BY \"CIRCLE\", \"CircleCode\", \"DivisionCode1\", \"DivisionName1\", \"SubDivisionName1\", \"SubDivisionCode1\", \"SectionName1\", ";
                ////strQry += " \"sectioncode1\" ORDER BY \"CircleCode\",\"DivisionCode1\")B on   A.\"CircleCode\" = B.\"CircleCode1\" and A.\"DivisionCode\" = B.\"DivisionCode1\"    and A.\"SubDivisionCode\" = b.\"SubDivisionCode1\" ";
                ////strQry += " and A.\"sectioncode\" = B.\"sectioncode1\" ORDER BY  COALESCE(\"sectioncode\",\"sectioncode1\")";

                ////dtDetails = objcon.FetchDataTable(strQry);
                ////return dtDetails;

                //strQry = " SELECT COALESCE(\"CircleCode\", \"CircleCode1\")\"CIRCLECODE\", COALESCE(\"CIRCLE\",\"CIRCLE1\") \"CIRCLE\", COALESCE(\"DivisionCode\",\"DivisionCode1\")\"DIVISIONCODE\", ";
                //strQry += " COALESCE(\"Division\", \"DivisionName1\")\"DIVISION\", COALESCE(\"SubDivisionCode\",\"SubDivisionCode1\")  \"SUBDIVISIONCODE\", COALESCE(\"SUBDIVISION\",\"SubDivisionName1\")\"SUBDIVISION\", ";
                //strQry += " COALESCE(\"sectioncode\", \"sectioncode1\")\"SECTIONCODE\", COALESCE(\"SECTION\", \"SectionName1\")\"SECTION\", COALESCE(\"LESSTHAN1DAY\", 0)\"LESSTHAN1DAY\",  COALESCE(\"BW1TO7\", 0) \"BW1TO7\", ";
                //strQry += " COALESCE(\"BW7TO15\", 0)\"BW7TO15\", COALESCE(\"BW15TO30\", 0)\"BW15TO30\", COALESCE(\"ABOVE30\", 0)\"ABOVE30\", COALESCE(\"TOTAL\", 0)\"TOTAL\", COALESCE(\"LESSTHAN1DAYNEW\", 0)\"LESSTHAN1DAYNEW\", ";
                //strQry += " COALESCE(\"BW1TO7NEW\", 0)\"BW1TO7NEW\", COALESCE(\"BW7TO15NEW\", 0)\"BW7TO15NEW\", COALESCE(\"BW15TO30NEW\", 0)\"BW15TO30NEW\",  COALESCE(\"ABOVE30NEW\", 0)\"ABOVE30NEW\", COALESCE(\"TOTALNEW\", 0)\"TOTALNEW\" ";
                //strQry += " FROM(SELECT \"CircleCode\", \"CIRCLE\",\"DivisionCode\", \"DivisionName\" as \"Division\", \"SubDivisionName\" as \"SUBDIVISION\", \"SubDivisionCode\", \"SectionName\" as \"SECTION\", \"sectioncode\", ";
                //strQry += " SUM(\"LESSTHAN1\")\"LESSTHAN1DAY\",  SUM(\"BETWEEN1TO7\")\"BW1TO7\", SUM(\"BETWEEN7TO15\")\"BW7TO15\", SUM(\"BETWEEN15TO30\")\"BW15TO30\", SUM(\"ABOVE30\")\"ABOVE30\", SUM(\"TOTAL\")\"TOTAL\" ";
                //strQry += " FROM ( SELECT(SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1)  FROM \"VIEW_ALL_OFFICES\" WHERE  CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DF_LOC_CODE\" as text), 1, " + Constants.Circle + "))  \"CIRCLE\", ";
                //strQry += " SUBSTR(cast(\"DF_LOC_CODE\" as text), 1, " + Constants.Circle + ")\"CircleCode\", (SELECT SUBSTR(\"OFF_NAME\", strpos(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE  CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DF_LOC_CODE\" as text),1, " + Constants.Division + ")) \"DivisionName\", ";
                //strQry += " SUBSTR(cast(\"DF_LOC_CODE\" as text), 1, " + Constants.Division + ")\"DivisionCode\", (SELECT SUBSTR(\"OFF_NAME\",  strpos(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE  CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DF_LOC_CODE\" as text), 1,  " + Constants.SubDivision + ")) \"SubDivisionName\", ";
                //strQry += " SUBSTR(cast(\"DF_LOC_CODE\" as text), 1, " + Constants.SubDivision + ")\"SubDivisionCode\", (SELECT SUBSTR(\"OFF_NAME\" , strpos(\"OFF_NAME\", ':') + 1)   FROM \"VIEW_ALL_OFFICES\"   WHERE CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DF_LOC_CODE\" as text) , 1, " + Constants.Section + ")) \"SectionName\", ";
                //strQry += " SUBSTR(cast(\"DF_LOC_CODE\"  as text), 1, " + Constants.Section + ")sectioncode,   CASE WHEN(\"IN_DATE\" - \"DF_DATE\") BETWEEN 0 AND 1 THEN  COUNT(*) ELSE 0 END \"LESSTHAN1\", CASE   WHEN(\"IN_DATE\" - \"DF_DATE\")  BETWEEN 2 AND 7 THEN  ";
                //strQry += " COUNT(*) ELSE 0 END  \"BETWEEN1TO7\", CASE   WHEN(\"IN_DATE\" - \"DF_DATE\")  BETWEEN 8 AND 15 THEN  COUNT(*) ELSE 0  END \"BETWEEN7TO15\", CASE WHEN(\"IN_DATE\" - \"DF_DATE\")  BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0 END \"BETWEEN15TO30\", ";
                //strQry += " CASE  WHEN(\"IN_DATE\" - \"DF_DATE\") > 30 THEN  COUNT(\"DF_DTC_CODE\") ELSE 0 END \"ABOVE30\", \"FD_FEEDER_NAME\", COUNT(*) \"TOTAL\"  FROM \"TBLFEEDERMAST\" INNER JOIN \"TBLDTCMAST\" ON  \"DT_FDRSLNO\" = \"FD_FEEDER_CODE\" INNER JOIN \"TBLDTCFAILURE\"  ";
                //strQry += " ON \"DF_DTC_CODE\" = \"DT_CODE\" INNER JOIN   \"TBLWORKORDER\" ON \"DF_ID\" = \"WO_DF_ID\" INNER JOIN  \"TBLINDENT\" ON  \"WO_SLNO\" = \"TI_WO_SLNO\" INNER JOIN \"TBLDTCINVOICE\"  ON \"TI_ID\" = \"IN_TI_NO\"  AND ";
                //strQry += " \"DF_STATUS_FLAG\" IN(1, 4) ";


                //if (sFromDate != "" && sTodate != "")
                //{
                //    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
                //    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    string sFinal_Todate = DToDate.ToString("yyyyMMdd");
                //    strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
                //}
                //else if (sFromDate != "" && sTodate == "")
                //{

                //    sTodate = DateTime.Now.ToString("dd/MM/yyyy");
                //    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
                //    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    string sFinal_Todate = DToDate.ToString("yyyyMMdd");
                //    strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
                //}
                //else if (sFromDate == string.Empty && sTodate != "")
                //{
                //    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    string sFinal_Todate = DToDate.ToString("yyyyMMdd");
                //    strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
                //}
                //else if (sFromDate != "" && sTodate == string.Empty)
                //{
                //    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
                //    strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "'";
                //}
                //strQry += " GROUP BY \"DF_ID\",\"DF_DATE\", \"IN_DATE\", \"DF_LOC_CODE\", \"DF_DTC_CODE\", \"FD_FEEDER_NAME\" ) A  ";
                //strQry += "  GROUP BY \"CIRCLE\",\"CircleCode\",  \"DivisionCode\",\"DivisionName\",\"SubDivisionName\",\"SubDivisionCode\",\"SectionName\",\"sectioncode\" ORDER BY \"CircleCode\", ";
                //strQry += " \"DivisionCode\",\"SubDivisionCode\")A FULL OUTER JOIN   (SELECT \"DivisionCode1\",\"DivisionName1\",\"SubDivisionName1\",\"SubDivisionCode1\", \"SectionName1\",\"sectioncode1\", ";
                //strQry += " \"CircleCode\" AS  \"CircleCode1\", \"CIRCLE\" AS \"CIRCLE1\", SUM(\"LESSTHAN1\")\"LESSTHAN1DAYNEW\",  SUM(\"BETWEEN1TO7\")\"BW1TO7NEW\", SUM(\"BETWEEN7TO15\")\"BW7TO15NEW\", ";
                //strQry += " SUM(\"BETWEEN15TO30\")\"BW15TO30NEW\", SUM(\"ABOVE30\")\"ABOVE30NEW\",  SUM(\"TOTAL\") \"TOTALNEW\" FROM ( SELECT \"DF_ID\", (SELECT SUBSTR(\"OFF_NAME\", strpos(\"OFF_NAME\", ':') + 1) ";
                //strQry += " FROM \"VIEW_ALL_OFFICES\"  WHERE  CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DF_LOC_CODE\" as text), 1, " + Constants.Circle + ")) \"CIRCLE\", SUBSTR(cast(\"DF_LOC_CODE\" as text), 1,  " + Constants.Circle + ")\"CircleCode\", ";
                //strQry += " (SELECT  SUBSTR(\"OFF_NAME\", strpos(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE  CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DF_LOC_CODE\" as text),1,  " + Constants.Division + ")) ";
                //strQry += " \"DivisionName1\",SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.Division + ")\"DivisionCode1\",(SELECT SUBSTR(\"OFF_NAME\", strpos(\"OFF_NAME\", ':') + 1) FROM  \"VIEW_ALL_OFFICES\" ";
                //strQry += " WHERE CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.SubDivision + "))\"SubDivisionName1\", SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.SubDivision + ") \"SubDivisionCode1\", ";
                //strQry += " (SELECT SUBSTR(\"OFF_NAME\", strpos(\"OFF_NAME\", ':') + 1)   FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT)= SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.Section + ")) \"SectionName1\", ";
                //strQry += " SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.Section + ")  \"sectioncode1\",  CASE WHEN  (CURRENT_DATE - \"DF_DATE\")  BETWEEN 0 AND 1 THEN COUNT(*)  ELSE 0 END \"LESSTHAN1\", CASE WHEN(CURRENT_DATE - \"DF_DATE\") ";
                //strQry += " BETWEEN 2 AND 7 THEN  COUNT(*) ELSE 0 END \"BETWEEN1TO7\", CASE WHEN(CURRENT_DATE - \"DF_DATE\")  BETWEEN 8 AND 15 THEN  COUNT(*) ELSE 0 END \"BETWEEN7TO15\", CASE WHEN(CURRENT_DATE - \"DF_DATE\") ";
                //strQry += " BETWEEN 16 AND 30 THEN    COUNT(*) ELSE 0 END \"BETWEEN15TO30\", CASE   WHEN(CURRENT_DATE - \"DF_DATE\") > 30 THEN  COUNT(\"DF_DTC_CODE\") ELSE 0 END \"ABOVE30\", \"FD_FEEDER_NAME\", COUNT(*) \"TOTAL\" ";
                //strQry += " FROM \"TBLFEEDERMAST\" INNER JOIN \"TBLDTCMAST\" ON  \"DT_FDRSLNO\" = \"FD_FEEDER_CODE\" INNER JOIN   \"TBLDTCFAILURE\"   ON \"DF_DTC_CODE\" = \"DT_CODE\"  AND  \"DF_REPLACE_FLAG\" <> 1 AND  \"DF_STATUS_FLAG\"  IN(1, 4)  ";


                //if (sFromDate != "" && sTodate != "")
                //{
                //    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
                //    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    string sFinal_Todate = DToDate.ToString("yyyyMMdd");
                //    strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
                //}
                //else if (sFromDate != "" && sTodate == "")
                //{

                //    sTodate = DateTime.Now.ToString("dd/MM/yyyy");
                //    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
                //    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    string sFinal_Todate = DToDate.ToString("yyyyMMdd");
                //    strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
                //}
                //else if (sFromDate == string.Empty && sTodate != "")
                //{
                //    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    string sFinal_Todate = DToDate.ToString("yyyyMMdd");
                //    strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
                //}
                //else if (sFromDate != "" && sTodate == string.Empty)
                //{
                //    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
                //    strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' ";
                //}
                //strQry += "  GROUP BY \"DF_ID\", \"DF_DATE\",\"DF_LOC_CODE\", \"DF_DTC_CODE\",\"FD_FEEDER_NAME\")S   LEFT JOIN \"TBLTCDRAWN\" ON \"DF_ID\" = \"TD_DF_ID\"  where ";
                //strQry += "  \"TD_DF_ID\" IS NULL GROUP BY \"CIRCLE\", \"CircleCode\", \"DivisionCode1\", \"DivisionName1\", \"SubDivisionName1\", \"SubDivisionCode1\", \"SectionName1\", ";
                //strQry += " \"sectioncode1\" ORDER BY \"CircleCode\",\"DivisionCode1\")B on   A.\"CircleCode\" = B.\"CircleCode1\" and A.\"DivisionCode\" = B.\"DivisionCode1\"    and A.\"SubDivisionCode\" = b.\"SubDivisionCode1\" ";
                //strQry += " and A.\"sectioncode\" = B.\"sectioncode1\" ORDER BY  COALESCE(\"sectioncode\",\"sectioncode1\")";

                //dtDetails = objcon.FetchDataTable(strQry);
                //return dtDetails;
                #endregion

                string CircleDigit = Convert.ToString(Constants.Circle);
                string DivDigit = Convert.ToString(Constants.Division);
                string SubDivDigit = Convert.ToString(Constants.SubDivision);
                string SectionDigit = Convert.ToString(Constants.Section);

                NpgsqlCommand cmdfailureTimeLine = new NpgsqlCommand("proc_load_all_details_failure_replacement_time_line_report");

                if (sFromDate != "" && sTodate != "")
                {
                    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
                    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string sFinal_Todate = DToDate.ToString("yyyyMMdd");

                    cmdfailureTimeLine.Parameters.AddWithValue("from_date", sFinal_FromDate);
                    cmdfailureTimeLine.Parameters.AddWithValue("to_date", sFinal_Todate);
                    
                }
                else if (sFromDate != "" && sTodate == "")
                {

                    sTodate = DateTime.Now.ToString("dd/MM/yyyy");
                    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
                    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string sFinal_Todate = DToDate.ToString("yyyyMMdd");

                    cmdfailureTimeLine.Parameters.AddWithValue("from_date", sFinal_FromDate);
                    cmdfailureTimeLine.Parameters.AddWithValue("to_date", (sFinal_Todate ?? ""));
                    
                }
                else if (sFromDate == string.Empty && sTodate != "")
                {
                    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string sFinal_Todate = DToDate.ToString("yyyyMMdd");

                    cmdfailureTimeLine.Parameters.AddWithValue("from_date", "");
                    cmdfailureTimeLine.Parameters.AddWithValue("to_date", sFinal_Todate);
                    
                }
                else if (sFromDate != "" && sTodate == string.Empty)
                {
                    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");

                    cmdfailureTimeLine.Parameters.AddWithValue("from_date", sFinal_FromDate);
                    cmdfailureTimeLine.Parameters.AddWithValue("to_date", "");
                    
                }
                else if (sFromDate == "" && sTodate == "")
                {
                    cmdfailureTimeLine.Parameters.AddWithValue("from_date","");
                    cmdfailureTimeLine.Parameters.AddWithValue("to_date", "");
                }

                cmdfailureTimeLine.Parameters.AddWithValue("circle_digit", CircleDigit);
                cmdfailureTimeLine.Parameters.AddWithValue("div_digit", DivDigit);
                cmdfailureTimeLine.Parameters.AddWithValue("subdiv_digit", SubDivDigit);
                cmdfailureTimeLine.Parameters.AddWithValue("section_digit", SectionDigit);
                dtDetails = objcon.FetchDataTable(cmdfailureTimeLine);
                return dtDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtDetails;

            }
        }

        #region UnUsed Methods
        //public DataTable LoadDiviSionDetails(string CirclCode, string sFromDate, string sTodate)
        //{
        //    DataTable dtDetails = new DataTable();
        //    try
        //    {
        //        //strQry = " SELECT NVL(CircleCode, CircleCode1)CIRCLECODE, NVL(CIRCLE, CIRCLE1)CIRCLE, NVL(DivisionCode, DivisionCode1)DIVISIONCODE,";
        //        //strQry += "  NVL(Division, DivisionName1)DIVISION, NVL(LESSTHAN1DAY, 0)LESSTHAN1DAY, NVL(BW1TO7, 0)BW1TO7, NVL(BW7TO15, 0)BW7TO15,";
        //        //strQry += "  NVL(BW15TO30, 0)BW15TO30, NVL(ABOVE30, 0)ABOVE30, NVL(TOTAL, 0)TOTAL, NVL(LESSTHAN1DAYNEW, 0)LESSTHAN1DAYNEW,";
        //        //strQry += " NVL(BW1TO7NEW, 0)BW1TO7NEW, NVL(BW7TO15NEW, 0)  BW7TO15NEW, NVL(BW15TO30NEW, 0)BW15TO30NEW, NVL(ABOVE30NEW, 0) ";
        //        //strQry += " ABOVE30NEW, NVL(TOTALNEW, 0)TOTALNEW FROM(SELECT CircleCode, CIRCLE, DivisionCode, DivisionName as Division, ";
        //        //strQry += " SUM(LESSTHAN1)LESSTHAN1DAY, SUM(BETWEEN1TO7)BW1TO7, SUM(BETWEEN7TO15)BW7TO15, SUM(BETWEEN15TO30)BW15TO30,";
        //        //strQry += " SUM(ABOVE30)ABOVE30, SUM(TOTAL)TOTAL fROM(SELECT(SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1)  FROM  ";
        //        //strQry += "  VIEW_ALL_OFFICES WHERE  OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 1))CIRCLE, SUBSTR(DF_LOC_CODE, 0, 1)CircleCode, ";
        //        //strQry += " (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES WHERE   ";
        //        //strQry += "  OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 2)) DivisionName, SUBSTR(DF_LOC_CODE, 0, 2)DivisionCode, ";
        //        //strQry += "   CASE WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 0 AND 1 THEN  COUNT(*) ELSE 0 END LESSTHAN1, ";
        //        //strQry += "  CASE WHEN(TR_DECOMM_DATE - DF_DATE)    BETWEEN 2 AND 7 THEN  COUNT(*) ELSE 0 END  BETWEEN1TO7, ";
        //        //strQry += "  CASE WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 8 AND 15 THEN    COUNT(*) ELSE 0 END BETWEEN7TO15, CASE ";
        //        //strQry += "   WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0   END BETWEEN15TO30, CASE ";
        //        //strQry += "   WHEN(TR_DECOMM_DATE - DF_DATE) > 30 THEN  COUNT(DF_DTC_CODE) ELSE 0 END ABOVE30, FD_FEEDER_NAME, ";
        //        //strQry += " COUNT(*) TOTAL FROM TBLFEEDERMAST INNER JOIN TBLDTCMAST ON  DT_FDRSLNO = FD_FEEDER_CODE INNER   JOIN ";
        //        //strQry += " TBLDTCFAILURE  ON DF_DTC_CODE = DT_CODE INNER JOIN   TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN    ";
        //        //strQry += " TBLINDENT ON    WO_SLNO = TI_WO_SLNO INNER JOIN TBLDTCINVOICE  ON TI_ID = IN_TI_NO INNER JOIN TBLTCREPLACE ";
        //        //strQry += " ON IN_NO = TR_IN_NO AND DF_REPLACE_FLAG <> 1  AND DF_STATUS_FLAG IN(1, 4) AND SUBSTR(DF_LOC_CODE, 0, 1) = '"+ CirclCode + "' ";

        //        //if (sFromDate != "" && sTodate != "")
        //        //{
        //        //    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sFromDate = DFromDate.ToString("yyyyMMdd");
        //        //    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sTodate = DToDate.ToString("yyyyMMdd");
        //        //    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        //}
        //        //else if (sFromDate != "" && sTodate == "")
        //        //{

        //        //    sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //        //    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sFromDate = DFromDate.ToString("yyyyMMdd");
        //        //    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sTodate = DToDate.ToString("yyyyMMdd");
        //        //    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        //}
        //        //else if (sFromDate == string.Empty && sTodate != "")
        //        //{
        //        //    strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        //}

        //        //strQry += "  GROUP BY DF_ID, DF_DATE, TR_DECOMM_DATE, DF_LOC_CODE, DF_DTC_CODE, FD_FEEDER_NAME) A GROUP BY CIRCLE, CircleCode, ";
        //        //strQry += " DivisionCode, DivisionName ORDER BY CircleCode, DivisionCode)A ";
        //        //strQry += " FULL OUTER JOIN ";
        //        //strQry += " (SELECT DivisionCode1, DivisionName1, CircleCode AS CircleCode1, CIRCLE AS CIRCLE1, SUM(LESSTHAN1)LESSTHAN1DAYNEW, ";
        //        //strQry += " SUM(BETWEEN1TO7)BW1TO7NEW, SUM(BETWEEN7TO15)BW7TO15NEW, SUM(BETWEEN15TO30)BW15TO30NEW, SUM(ABOVE30)ABOVE30NEW,";
        //        //strQry += " SUM(TOTAL)TOTALNEW FROM(SELECT IN_NO, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES  ";
        //        //strQry += " WHERE    OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 1))CIRCLE, SUBSTR(DF_LOC_CODE, 0, 1)CircleCode, (SELECT     ";
        //        //strQry += "  SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES WHERE OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 2))   ";
        //        //strQry += "  DivisionName1,SUBSTR(DF_LOC_CODE, 0, 2)DivisionCode1,   CASE WHEN  (IN_DATE - DF_DATE)  BETWEEN 0 AND 1 ";
        //        //strQry += " THEN COUNT(*) ELSE 0 END LESSTHAN1, CASE WHEN(IN_DATE - DF_DATE)    BETWEEN 2 AND 7 THEN  COUNT(*) ELSE 0 END BETWEEN1TO7, ";
        //        //strQry += "  CASE WHEN(IN_DATE - DF_DATE)  BETWEEN 8 AND 15 THEN    COUNT(*) ELSE 0 END BETWEEN7TO15, CASE ";
        //        //strQry += "  WHEN(IN_DATE - DF_DATE)  BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0   END BETWEEN15TO30, CASE  ";
        //        //strQry += "  WHEN(IN_DATE - DF_DATE) > 30 THEN  COUNT(DF_DTC_CODE) ELSE 0 END ABOVE30, FD_FEEDER_NAME, COUNT(*) TOTAL  ";
        //        //strQry += "  FROM TBLFEEDERMAST INNER JOIN TBLDTCMAST ON  DT_FDRSLNO = FD_FEEDER_CODE   INNER JOIN TBLDTCFAILURE   ";
        //        //strQry += "  ON DF_DTC_CODE = DT_CODE INNER JOIN   TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN    TBLINDENT ON ";
        //        //strQry += "  WO_SLNO = TI_WO_SLNO INNER JOIN TBLDTCINVOICE  ON TI_ID = IN_TI_NO AND DF_REPLACE_FLAG <> 1    AND ";
        //        //strQry += "  DF_STATUS_FLAG IN(1, 4) ";
        //        //     if (sFromDate != "" && sTodate != "")
        //        //{
        //        //    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sFromDate = DFromDate.ToString("yyyyMMdd");
        //        //    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sTodate = DToDate.ToString("yyyyMMdd");
        //        //    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        //}
        //        //else if (sFromDate != "" && sTodate == "")
        //        //{

        //        //    sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //        //    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sFromDate = DFromDate.ToString("yyyyMMdd");
        //        //    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sTodate = DToDate.ToString("yyyyMMdd");
        //        //    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        //}
        //        //else if (sFromDate == string.Empty && sTodate != "")
        //        //{
        //        //    strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        //}
        //        //strQry += " GROUP BY DF_ID, DF_DATE, IN_NO, IN_DATE, DF_LOC_CODE, DF_DTC_CODE, FD_FEEDER_NAME)  ";
        //        //strQry += " LEFT JOIN TBLTCREPLACE ON IN_NO = TR_IN_NO  where IN_NO IS NOT NULL AND TR_IN_NO IS NULL GROUP BY CIRCLE, ";
        //        //strQry += " CircleCode, DivisionCode1, DivisionName1 ORDER BY CircleCode,DivisionCode1)B on    A.CircleCode = B.CircleCode1 ";
        //        //strQry += " and A.DivisionCode = B.DivisionCode1  WHERE B.CircleCode1 = '"+ CirclCode + "' ";
        //        //strQry += "  ORDER BY NVL(DivisionCode, DivisionCode1), NVL(CIRCLE, CIRCLE1)";


        //        strQry = " SELECT NVL(CircleCode, CircleCode1)CIRCLECODE, NVL(CIRCLE, CIRCLE1)CIRCLE, NVL(DivisionCode, DivisionCode1) ";
        //        strQry += " DIVISIONCODE, NVL(Division, DivisionName1)DIVISION, NVL(LESSTHAN1DAY, 0)LESSTHAN1DAY, NVL(BW1TO7, 0)BW1TO7, ";
        //        strQry += "  NVL(BW7TO15, 0)BW7TO15, NVL(BW15TO30, 0)BW15TO30, NVL(ABOVE30, 0)ABOVE30, NVL(TOTAL, 0)TOTAL,  ";
        //        strQry += " NVL(LESSTHAN1DAYNEW, 0)LESSTHAN1DAYNEW, NVL(BW1TO7NEW, 0)BW1TO7NEW, NVL(BW7TO15NEW, 0)  BW7TO15NEW, ";
        //        strQry += "  NVL(BW15TO30NEW, 0)BW15TO30NEW, NVL(ABOVE30NEW, 0)  ABOVE30NEW, NVL(TOTALNEW, 0)TOTALNEW FROM(SELECT  ";
        //        strQry += " SUBSTR(DIV_CODE, 0, 1)CircleCode, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES ";
        //        strQry += " WHERE  OFF_CODE = SUBSTR(DIV_CODE, 0, 1))CIRCLE, SUBSTR(DIV_CODE, 0, 2)DivisionCode, (SELECT   ";
        //        strQry += "  SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES   WHERE OFF_CODE = SUBSTR(DIV_CODE, 0, 2))";
        //        strQry += "  Division,LESSTHAN1DAY,BW1TO7,BW7TO15,BW15TO30,ABOVE30,TOTAL FROM(SELECT CircleCode, CIRCLE, DivisionCode,";
        //        strQry += "  DivisionName as Division, SUM(LESSTHAN1)LESSTHAN1DAY, SUM(BETWEEN1TO7)BW1TO7, SUM(BETWEEN7TO15)BW7TO15, ";
        //        strQry += "  SUM(BETWEEN15TO30)BW15TO30, SUM(ABOVE30)ABOVE30, SUM(TOTAL)TOTAL fROM(SELECT(SELECT SUBSTR(OFF_NAME, ";
        //        strQry += "  INSTR(OFF_NAME, ':') + 1)  FROM    VIEW_ALL_OFFICES WHERE  OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 1))CIRCLE, ";
        //        strQry += "  SUBSTR(DF_LOC_CODE, 0, 1)CircleCode, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES";
        //        strQry += " WHERE     OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 2)) DivisionName, SUBSTR(DF_LOC_CODE, 0, 2)DivisionCode, ";
        //        strQry += "  CASE WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 0 AND 1 THEN  COUNT(*) ELSE 0 END LESSTHAN1, CASE ";
        //        strQry += "  WHEN(TR_DECOMM_DATE - DF_DATE)    BETWEEN 2 AND 7 THEN  COUNT(*) ELSE 0 END  BETWEEN1TO7, CASE ";
        //        strQry += "  WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 8 AND 15 THEN    COUNT(*) ELSE 0 END BETWEEN7TO15, CASE   ";
        //        strQry += "  WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0   END BETWEEN15TO30, CASE  ";
        //        strQry += "  WHEN(TR_DECOMM_DATE - DF_DATE) > 30 THEN  COUNT(DF_DTC_CODE) ELSE 0 END ABOVE30, FD_FEEDER_NAME, ";
        //        strQry += " COUNT(*) TOTAL FROM TBLFEEDERMAST INNER JOIN TBLDTCMAST ON  DT_FDRSLNO = FD_FEEDER_CODE INNER   ";
        //        strQry += "  JOIN  TBLDTCFAILURE  ON DF_DTC_CODE = DT_CODE INNER JOIN   TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN  ";
        //        strQry += "  TBLINDENT ON    WO_SLNO = TI_WO_SLNO INNER JOIN TBLDTCINVOICE  ON TI_ID = IN_TI_NO INNER JOIN ";
        //        strQry += "  TBLTCREPLACE  ON IN_NO = TR_IN_NO AND DF_REPLACE_FLAG <> 1  AND DF_STATUS_FLAG IN(1, 4) AND SUBSTR";
        //        strQry += "  (DF_LOC_CODE, 0, 1) = '" + CirclCode + "' ";
        //        if (sFromDate != "" && sTodate != "")
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            sFromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            sTodate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == "")
        //        {

        //            sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            sFromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            sTodate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        else if (sFromDate == string.Empty && sTodate != "")
        //        {
        //            strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        strQry += " GROUP BY DF_ID, DF_DATE, TR_DECOMM_DATE, DF_LOC_CODE, DF_DTC_CODE,";
        //        strQry += " FD_FEEDER_NAME) A GROUP BY CIRCLE, CircleCode, DivisionCode, DivisionName ORDER BY CircleCode, ";
        //        strQry += "  DivisionCode)A RIGHT JOIN(SELECT DIV_CODE FROM TBLDIVISION WHERE DIV_CICLE_CODE = '" + CirclCode + "')B ON ";
        //        strQry += "  A.DivisionCode = B.DIV_CODE)A LEFT  JOIN(SELECT SUBSTR(DIV_CODE, 0, 1)CircleCode1, (SELECT SUBSTR(OFF_NAME, ";
        //        strQry += "  INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES   WHERE  OFF_CODE = SUBSTR(DIV_CODE, 0, 1))CIRCLE1, SUBSTR(DIV_CODE, 0, 2) ";
        //        strQry += "   DivisionCode1, (SELECT   SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES ";
        //        strQry += "  WHERE OFF_CODE = SUBSTR(DIV_CODE, 0, 2)) DivisionName1,LESSTHAN1DAYNEW,BW1TO7NEW,BW7TO15NEW,BW15TO30NEW, ";
        //        strQry += "   ABOVE30NEW,TOTALNEW FROM(SELECT DivisionCode1, DivisionName1, CircleCode AS CircleCode1, CIRCLE AS CIRCLE1,";
        //        strQry += "  SUM(LESSTHAN1)LESSTHAN1DAYNEW, SUM(BETWEEN1TO7)BW1TO7NEW, SUM(BETWEEN7TO15)BW7TO15NEW, ";
        //        strQry += "  SUM(BETWEEN15TO30)BW15TO30NEW, SUM(ABOVE30)ABOVE30NEW, SUM(TOTAL)TOTALNEW FROM(SELECT IN_NO, ";
        //        strQry += " (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES   WHERE   ";
        //        strQry += "  OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 1))CIRCLE, SUBSTR(DF_LOC_CODE, 0, 1)CircleCode, (SELECT       ";
        //        strQry += "  SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES WHERE OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 2)) ";
        //        strQry += "  DivisionName1,SUBSTR(DF_LOC_CODE, 0, 2)DivisionCode1,   CASE WHEN  (IN_DATE - DF_DATE)  BETWEEN 0 AND 1 ";
        //        strQry += "  THEN COUNT(*) ELSE 0 END LESSTHAN1, CASE WHEN(IN_DATE - DF_DATE)    BETWEEN 2 AND 7 THEN  COUNT(*) ";
        //        strQry += "  ELSE 0 END BETWEEN1TO7, CASE WHEN(IN_DATE - DF_DATE)  BETWEEN 8 AND 15 THEN    COUNT(*) ELSE 0 END BETWEEN7TO15,";
        //        strQry += "  CASE   WHEN(IN_DATE - DF_DATE)  BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0   END BETWEEN15TO30, CASE  ";
        //        strQry += "  WHEN(IN_DATE - DF_DATE) > 30 THEN  COUNT(DF_DTC_CODE) ELSE 0 END ABOVE30, FD_FEEDER_NAME, COUNT(*) TOTAL  ";
        //        strQry += "  FROM TBLFEEDERMAST INNER JOIN TBLDTCMAST ON  DT_FDRSLNO = FD_FEEDER_CODE   INNER JOIN TBLDTCFAILURE    ";
        //        strQry += "  ON DF_DTC_CODE = DT_CODE INNER JOIN   TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN    TBLINDENT ON  ";
        //        strQry += "  WO_SLNO = TI_WO_SLNO INNER JOIN TBLDTCINVOICE  ON TI_ID = IN_TI_NO AND DF_REPLACE_FLAG <> 1    AND  ";
        //        strQry += "  DF_STATUS_FLAG IN(1, 4) ";
        //        if (sFromDate != "" && sTodate != "")
        //        {
        //            //DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            //sFromDate = DFromDate.ToString("yyyyMMdd");
        //            //DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            //sTodate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == "")
        //        {

        //            //sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //            //DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            //sFromDate = DFromDate.ToString("yyyyMMdd");
        //            //DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            //sTodate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        else if (sFromDate == string.Empty && sTodate != "")
        //        {
        //            strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        strQry += "  GROUP BY DF_ID, DF_DATE, IN_NO, IN_DATE, DF_LOC_CODE, DF_DTC_CODE, FD_FEEDER_NAME)  ";
        //        strQry += "  LEFT JOIN TBLTCREPLACE ON IN_NO = TR_IN_NO  where IN_NO IS NOT NULL AND TR_IN_NO IS NULL GROUP BY CIRCLE,";
        //        strQry += "  CircleCode, DivisionCode1, DivisionName1 ORDER BY CircleCode,DivisionCode1)A RIGHT JOIN(SELECT DIV_CODE ";
        //        strQry += "  FROM TBLDIVISION WHERE DIV_CICLE_CODE = '" + CirclCode + "')B ON ";
        //        strQry += "  A.DivisionCode1 = B.DIV_CODE)B on    A.CircleCode = B.CircleCode1  and A.DivisionCode = B.DivisionCode1 ";
        //        strQry += "  WHERE B.CircleCode1 = '" + CirclCode + "'   ORDER BY NVL(DivisionCode, DivisionCode1), NVL(CIRCLE, CIRCLE1) ";
        //        dtDetails = objcon.FetchDataTable(strQry);
        //        return dtDetails;
        //    }


        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadDiviSionDetails");
        //        return dtDetails;

        //    }

        //}

        //public DataTable LoadDiviSionDetails(string CirclCode, string sFromDate, string sTodate)
        //{
        //    DataTable dtDetails = new DataTable();
        //    try
        //    {


        //        strQry = " SELECT COALESCE(\"CIRCLECODE\", \"CIRCLECODE1\")\"CIRCLECODE\", COALESCE(\"CIRCLE\", \"CIRCLE1\")\"CIRCLE\", COALESCE(\"DIVISIONCODE\", \"DIVISIONCODE1\") ";
        //        strQry += " \"DIVISIONCODE\", COALESCE(\"DIVISION\", \"DIVISIONNAME1\")\"DIVISION\", COALESCE(\"LESSTHAN1DAY\", 0)\"LESSTHAN1DAY\", COALESCE(\"BW1TO7\", 0)\"BW1TO7\", ";
        //        strQry += "  COALESCE(\"BW7TO15\", 0)\"BW7TO15\", COALESCE(\"BW15TO30\", 0)\"BW15TO30\", COALESCE(\"ABOVE30\", 0)\"ABOVE30\", COALESCE(\"TOTAL\", 0)\"TOTAL\",  ";
        //        strQry += " COALESCE(\"LESSTHAN1DAYNEW\", 0)\"LESSTHAN1DAYNEW\", COALESCE(\"BW1TO7NEW\", 0)\"BW1TO7NEW\", COALESCE(\"BW7TO15NEW\", 0)  \"BW7TO15NEW\", ";
        //        strQry += "  COALESCE(\"BW15TO30NEW\", 0)\"BW15TO30NEW\", COALESCE(\"ABOVE30NEW\", 0)  \"ABOVE30NEW\", COALESCE(\"TOTALNEW\", 0)\"TOTALNEW\" FROM(SELECT  ";
        //        strQry += " SUBSTR(cast(\"DIV_CODE\" as text), 1," + Constants.Circle + ")\"CIRCLECODE\", (SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" ";
        //        strQry += " WHERE  CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DIV_CODE\" as text), 1," + Constants.Circle + "))\"CIRCLE\", SUBSTR(CAST(\"DIV_CODE\" as TEXT), 1," + Constants.Division + ")\"DIVISIONCODE\", (SELECT   ";
        //        strQry += "  SUBSTR(cast(\"OFF_NAME\" as TEXT), STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\"   WHERE CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(CAST(\"DIV_CODE\" as TEXT), 1, " + Constants.Division + "))";
        //        strQry += "  \"DIVISION\",\"LESSTHAN1DAY\",\"BW1TO7\",\"BW7TO15\",\"BW15TO30\",\"ABOVE30\",\"TOTAL\" FROM(SELECT \"CIRCLECODE\", \"CIRCLE\", \"DIVISIONCODE\",";
        //        strQry += "  \"DIVISIONNAME\" as \"DIVISION\", SUM(\"LESSTHAN1\")\"LESSTHAN1DAY\", SUM(\"BETWEEN1TO7\")\"BW1TO7\", SUM(\"BETWEEN7TO15\")\"BW7TO15\", ";
        //        strQry += "  SUM(\"BETWEEN15TO30\")\"BW15TO30\", SUM(\"ABOVE30\")\"ABOVE30\", SUM(\"TOTAL\")\"TOTAL\" fROM(SELECT(SELECT SUBSTR(\"OFF_NAME\", ";
        //        strQry += "  STRPOS(\"OFF_NAME\", ':') + 1)  FROM    \"VIEW_ALL_OFFICES\" WHERE  CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DF_LOC_CODE\" as text), 1, " + Constants.Circle + "))\"CIRCLE\", ";
        //        strQry += "  SUBSTR(cast(\"DF_LOC_CODE\" as text), 1, " + Constants.Circle + ")\"CIRCLECODE\", (SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\"";
        //        strQry += " WHERE     CAST(\"OFF_CODE\" AS TEXT)= SUBSTR(cast(\"DF_LOC_CODE\" as TEXT), 1, " + Constants.Division + ")) \"DIVISIONNAME\", SUBSTR(cast(\"DF_LOC_CODE\" as TEXT), 1," + Constants.Division + " )\"DIVISIONCODE\", ";
        //        strQry += "  CASE WHEN(\"TR_DECOMM_DATE\"- \"DF_DATE\")  BETWEEN 0 AND 1 THEN  COUNT(*) ELSE 0 END \"LESSTHAN1\", CASE ";
        //        strQry += "  WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")    BETWEEN 2 AND 7 THEN  COUNT(*) ELSE 0 END  \"BETWEEN1TO7\", CASE ";
        //        strQry += "  WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")  BETWEEN 8 AND 15 THEN    COUNT(*) ELSE 0 END \"BETWEEN7TO15\", CASE   ";
        //        strQry += "  WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")  BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0   END \"BETWEEN15TO30\", CASE  ";
        //        strQry += "  WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\") > 30 THEN  COUNT(\"DF_DTC_CODE\") ELSE 0 END \"ABOVE30\", \"FD_FEEDER_NAME\", ";
        //        strQry += " COUNT(*) \"TOTAL\" FROM \"TBLFEEDERMAST\" INNER JOIN \"TBLDTCMAST\" ON  \"DT_FDRSLNO\" = \"FD_FEEDER_CODE\" INNER   ";
        //        strQry += "  JOIN  \"TBLDTCFAILURE\"  ON \"DF_DTC_CODE\" = \"DT_CODE\" INNER JOIN   \"TBLWORKORDER\" ON \"DF_ID\" = \"WO_DF_ID\" INNER JOIN  ";
        //        strQry += "  \"TBLINDENT\" ON    \"WO_SLNO\" = \"TI_WO_SLNO\" INNER JOIN \"TBLDTCINVOICE\"  ON \"TI_ID\" = \"IN_TI_NO\" INNER JOIN ";
        //        strQry += "  \"TBLTCREPLACE\"  ON \"IN_NO\" = \"TR_IN_NO\" AND \"DF_REPLACE_FLAG\" <> 1  AND \"DF_STATUS_FLAG\" IN(1, 4) AND SUBSTR";
        //        strQry += "  (CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.Circle + ") = '" + CirclCode + "' ";
        //        if (sFromDate != "" && sTodate != "")
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == "")
        //        {

        //            sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate == string.Empty && sTodate != "")
        //        {
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == string.Empty)
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "'";
        //        }
        //        strQry += " GROUP BY \"DF_ID\", \"DF_DATE\", \"TR_DECOMM_DATE\", \"DF_LOC_CODE\", \"DF_DTC_CODE\",";
        //        strQry += " \"FD_FEEDER_NAME\") A GROUP BY \"CIRCLE\", \"CIRCLECODE\", \"DIVISIONCODE\", \"DIVISIONNAME\" ORDER BY \"CIRCLECODE\", ";
        //        strQry += "  \"DIVISIONCODE\")A INNER JOIN(SELECT \"DIV_CODE\" FROM \"TBLDIVISION\" WHERE \"DIV_CICLE_CODE\" = " + CirclCode + ")B ON ";
        //        strQry += "  A.\"DIVISIONCODE\" = CAST(B.\"DIV_CODE\" AS TEXT))A FULL OUTER  JOIN(SELECT SUBSTR(cast(\"DIV_CODE\" as TEXT), 1, " + Constants.Circle + ")\"CIRCLECODE1\", (SELECT SUBSTR(\"OFF_NAME\", ";
        //        strQry += "  STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\"   WHERE  CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DIV_CODE\" as TEXT), 1, " + Constants.Circle + "))\"CIRCLE1\", SUBSTR(cast(\"DIV_CODE\" as TEXT), 1, " + Constants.Division + ") ";
        //        strQry += "   \"DIVISIONCODE1\", (SELECT   SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" ";
        //        strQry += "  WHERE CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DIV_CODE\" as TEXT), 1, " + Constants.Division + ")) \"DIVISIONNAME1\",\"LESSTHAN1DAYNEW\",\"BW1TO7NEW\",\"BW7TO15NEW\",\"BW15TO30NEW\", ";
        //        strQry += "   \"ABOVE30NEW\",\"TOTALNEW\" FROM(SELECT \"DIVISIONCODE1\", \"DIVISIONNAME1\", \"CIRCLECODE\" AS \"CIRCLECODE1\", \"CIRCLE\" AS \"CIRCLE1\",";
        //        strQry += "  SUM(\"LESSTHAN1\")\"LESSTHAN1DAYNEW\", SUM(\"BETWEEN1TO7\")\"BW1TO7NEW\", SUM(\"BETWEEN7TO15\")\"BW7TO15NEW\", ";
        //        strQry += "  SUM(\"BETWEEN15TO30\")\"BW15TO30NEW\", SUM(\"ABOVE30\")\"ABOVE30NEW\", SUM(\"TOTAL\")\"TOTALNEW\" FROM(SELECT \"IN_NO\", ";
        //        strQry += " (SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\"   WHERE   ";
        //        strQry += "  CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DF_LOC_CODE\" as TEXT), 1, " + Constants.Circle + "))\"CIRCLE\", SUBSTR(cast(\"DF_LOC_CODE\" as text), 1, " + Constants.Circle + ")\"CIRCLECODE\", (SELECT       ";
        //        strQry += "  SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DF_LOC_CODE\" as TEXT), 1, " + Constants.Division + ")) ";
        //        strQry += "  \"DIVISIONNAME1\",SUBSTR(cast(\"DF_LOC_CODE\" as text), 1, " + Constants.Division + ")\"DIVISIONCODE1\",   CASE WHEN  (\"IN_DATE\" - \"DF_DATE\")  BETWEEN 0 AND 1 ";
        //        strQry += "  THEN COUNT(*) ELSE 0 END \"LESSTHAN1\", CASE WHEN(\"IN_DATE\" - \"DF_DATE\")    BETWEEN 2 AND 7 THEN  COUNT(*) ";
        //        strQry += "  ELSE 0 END \"BETWEEN1TO7\", CASE WHEN(\"IN_DATE\" - \"DF_DATE\")  BETWEEN 8 AND 15 THEN    COUNT(*) ELSE 0 END \"BETWEEN7TO15\",";
        //        strQry += "  CASE   WHEN(\"IN_DATE\" - \"DF_DATE\")  BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0   END \"BETWEEN15TO30\", CASE  ";
        //        strQry += "  WHEN(\"IN_DATE\" - \"DF_DATE\") > 30 THEN  COUNT(\"DF_DTC_CODE\") ELSE 0 END \"ABOVE30\", \"FD_FEEDER_NAME\", COUNT(*) \"TOTAL\"  ";
        //        strQry += "  FROM \"TBLFEEDERMAST\" INNER JOIN \"TBLDTCMAST\" ON  \"DT_FDRSLNO\" = \"FD_FEEDER_CODE\"   INNER JOIN \"TBLDTCFAILURE\"    ";
        //        strQry += "  ON \"DF_DTC_CODE\" = \"DT_CODE\" INNER JOIN   \"TBLWORKORDER\" ON \"DF_ID\" = \"WO_DF_ID\" INNER JOIN    \"TBLINDENT\" ON  ";
        //        strQry += "  \"WO_SLNO\" = \"TI_WO_SLNO\" INNER JOIN \"TBLDTCINVOICE\"  ON \"TI_ID\" = \"IN_TI_NO\" AND \"DF_REPLACE_FLAG\" <> 1    AND  ";
        //        strQry += "  \"DF_STATUS_FLAG\" IN(1, 4) ";
        //        if (sFromDate != "" && sTodate != "")
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == "")
        //        {

        //            sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate == string.Empty && sTodate != "")
        //        {
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == string.Empty)
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "'";
        //        }
        //        strQry += "  GROUP BY \"DF_ID\", \"DF_DATE\", \"IN_NO\", \"IN_DATE\", \"DF_LOC_CODE\", \"DF_DTC_CODE\", \"FD_FEEDER_NAME\") a ";
        //        strQry += "  LEFT JOIN \"TBLTCREPLACE\" ON \"IN_NO\" = \"TR_IN_NO\"  where \"IN_NO\" IS NOT NULL AND \"TR_IN_NO\" IS NULL GROUP BY \"CIRCLE\",";
        //        strQry += "  \"CIRCLECODE\", \"DIVISIONCODE1\", \"DIVISIONNAME1\" ORDER BY \"CIRCLECODE\",\"DIVISIONCODE1\")A INNER JOIN(SELECT \"DIV_CODE\" ";
        //        strQry += "  FROM \"TBLDIVISION\" WHERE \"DIV_CICLE_CODE\" = '" + CirclCode + "')B ON ";
        //        strQry += "  A.\"DIVISIONCODE1\" = CAST(B.\"DIV_CODE\" AS TEXT))B on    A.\"CIRCLECODE\" = B.\"CIRCLECODE1\"  and A.\"DIVISIONCODE\" = B.\"DIVISIONCODE1\" ";
        //        strQry += "   ORDER BY COALESCE(\"DIVISIONCODE\", \"DIVISIONCODE1\"), COALESCE(\"CIRCLE\", \"CIRCLE1\") ";
        //        dtDetails = objcon.FetchDataTable(strQry);
        //        return dtDetails;
        //    }


        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return dtDetails;

        //    }

        //}


        //public DataTable LoadDiviSionDetails(string CirclCode, string sFromDate, string sTodate)
        //{
        //    DataTable dtDetails = new DataTable();
        //    try
        //    {


        //        strQry = " SELECT COALESCE(\"CircleCode\", \"CircleCode1\")\"CIRCLECODE\", COALESCE(\"CIRCLE\", \"CIRCLE1\")\"CIRCLE\", COALESCE(\"DivisionCode\", \"DivisionCode1\") ";
        //        strQry += " \"DIVISIONCODE\", COALESCE(\"Division\", \"DivisionName1\")\"DIVISION\", COALESCE(\"LESSTHAN1DAY\", 0)\"LESSTHAN1DAY\", COALESCE(\"BW1TO7\", 0)\"BW1TO7\", ";
        //        strQry += "  COALESCE(\"BW7TO15\", 0)\"BW7TO15\", COALESCE(\"BW15TO30\", 0)\"BW15TO30\", COALESCE(\"ABOVE30\", 0)\"ABOVE30\", COALESCE(\"TOTAL\", 0)\"TOTAL\",  ";
        //        strQry += " COALESCE(\"LESSTHAN1DAYNEW\", 0)\"LESSTHAN1DAYNEW\", COALESCE(\"BW1TO7NEW\", 0)\"BW1TO7NEW\", COALESCE(\"BW7TO15NEW\", 0)  \"BW7TO15NEW\", ";
        //        strQry += "  COALESCE(\"BW15TO30NEW\", 0)\"BW15TO30NEW\", COALESCE(\"ABOVE30NEW\", 0)  \"ABOVE30NEW\", COALESCE(\"TOTALNEW\", 0)\"TOTALNEW\" FROM(SELECT  ";
        //        strQry += " SUBSTR(\"DIV_CODE\", 0," + Constants.Circle + ")\"CircleCode\", (SELECT SUBSTR(\"OFF_NAME\", INSTR(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" ";
        //        strQry += " WHERE  \"OFF_CODE\" = SUBSTR(\"DIV_CODE\", 0," + Constants.Circle + "))\"CIRCLE\", SUBSTR(\"DIV_CODE\", 0," + Constants.Division + ")\"DivisionCode\", (SELECT   ";
        //        strQry += "  SUBSTR(\"OFF_NAME\", INSTR(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\"   WHERE \"OFF_CODE\" = SUBSTR(\"DIV_CODE\", 0, " + Constants.Division + "))";
        //        strQry += "  Division,LESSTHAN1DAY,BW1TO7,BW7TO15,BW15TO30,ABOVE30,TOTAL FROM(SELECT CircleCode, CIRCLE, DivisionCode,";
        //        strQry += "  DivisionName as Division, SUM(LESSTHAN1)LESSTHAN1DAY, SUM(BETWEEN1TO7)BW1TO7, SUM(BETWEEN7TO15)BW7TO15, ";
        //        strQry += "  SUM(BETWEEN15TO30)BW15TO30, SUM(ABOVE30)ABOVE30, SUM(TOTAL)TOTAL fROM(SELECT(SELECT SUBSTR(OFF_NAME, ";
        //        strQry += "  INSTR(OFF_NAME, ':') + 1)  FROM    VIEW_ALL_OFFICES WHERE  OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 1))CIRCLE, ";
        //        strQry += "  SUBSTR(DF_LOC_CODE, 0, 1)CircleCode, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES";
        //        strQry += " WHERE     OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 2)) DivisionName, SUBSTR(DF_LOC_CODE, 0, 2)DivisionCode, ";
        //        strQry += "  CASE WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 0 AND 1 THEN  COUNT(*) ELSE 0 END LESSTHAN1, CASE ";
        //        strQry += "  WHEN(TR_DECOMM_DATE - DF_DATE)    BETWEEN 2 AND 7 THEN  COUNT(*) ELSE 0 END  BETWEEN1TO7, CASE ";
        //        strQry += "  WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 8 AND 15 THEN    COUNT(*) ELSE 0 END BETWEEN7TO15, CASE   ";
        //        strQry += "  WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0   END BETWEEN15TO30, CASE  ";
        //        strQry += "  WHEN(TR_DECOMM_DATE - DF_DATE) > 30 THEN  COUNT(DF_DTC_CODE) ELSE 0 END ABOVE30, FD_FEEDER_NAME, ";
        //        strQry += " COUNT(*) TOTAL FROM TBLFEEDERMAST INNER JOIN TBLDTCMAST ON  DT_FDRSLNO = FD_FEEDER_CODE INNER   ";
        //        strQry += "  JOIN  TBLDTCFAILURE  ON DF_DTC_CODE = DT_CODE INNER JOIN   TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN  ";
        //        strQry += "  TBLINDENT ON    WO_SLNO = TI_WO_SLNO INNER JOIN TBLDTCINVOICE  ON TI_ID = IN_TI_NO INNER JOIN ";
        //        strQry += "  TBLTCREPLACE  ON IN_NO = TR_IN_NO AND DF_REPLACE_FLAG <> 1  AND DF_STATUS_FLAG IN(1, 4) AND SUBSTR";
        //        strQry += "  (DF_LOC_CODE, 0, 1) = '" + CirclCode + "' ";
        //        if (sFromDate != "" && sTodate != "")
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            sFromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            sTodate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == "")
        //        {

        //            sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            sFromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            sTodate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        else if (sFromDate == string.Empty && sTodate != "")
        //        {
        //            strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        strQry += " GROUP BY DF_ID, DF_DATE, TR_DECOMM_DATE, DF_LOC_CODE, DF_DTC_CODE,";
        //        strQry += " FD_FEEDER_NAME) A GROUP BY CIRCLE, CircleCode, DivisionCode, DivisionName ORDER BY CircleCode, ";
        //        strQry += "  DivisionCode)A RIGHT JOIN(SELECT DIV_CODE FROM TBLDIVISION WHERE DIV_CICLE_CODE = '" + CirclCode + "')B ON ";
        //        strQry += "  A.DivisionCode = B.DIV_CODE)A LEFT  JOIN(SELECT SUBSTR(DIV_CODE, 0, 1)CircleCode1, (SELECT SUBSTR(OFF_NAME, ";
        //        strQry += "  INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES   WHERE  OFF_CODE = SUBSTR(DIV_CODE, 0, 1))CIRCLE1, SUBSTR(DIV_CODE, 0, 2) ";
        //        strQry += "   DivisionCode1, (SELECT   SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES ";
        //        strQry += "  WHERE OFF_CODE = SUBSTR(DIV_CODE, 0, 2)) DivisionName1,LESSTHAN1DAYNEW,BW1TO7NEW,BW7TO15NEW,BW15TO30NEW, ";
        //        strQry += "   ABOVE30NEW,TOTALNEW FROM(SELECT DivisionCode1, DivisionName1, CircleCode AS CircleCode1, CIRCLE AS CIRCLE1,";
        //        strQry += "  SUM(LESSTHAN1)LESSTHAN1DAYNEW, SUM(BETWEEN1TO7)BW1TO7NEW, SUM(BETWEEN7TO15)BW7TO15NEW, ";
        //        strQry += "  SUM(BETWEEN15TO30)BW15TO30NEW, SUM(ABOVE30)ABOVE30NEW, SUM(TOTAL)TOTALNEW FROM(SELECT IN_NO, ";
        //        strQry += " (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES   WHERE   ";
        //        strQry += "  OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 1))CIRCLE, SUBSTR(DF_LOC_CODE, 0, 1)CircleCode, (SELECT       ";
        //        strQry += "  SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES WHERE OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 2)) ";
        //        strQry += "  DivisionName1,SUBSTR(DF_LOC_CODE, 0, 2)DivisionCode1,   CASE WHEN  (IN_DATE - DF_DATE)  BETWEEN 0 AND 1 ";
        //        strQry += "  THEN COUNT(*) ELSE 0 END LESSTHAN1, CASE WHEN(IN_DATE - DF_DATE)    BETWEEN 2 AND 7 THEN  COUNT(*) ";
        //        strQry += "  ELSE 0 END BETWEEN1TO7, CASE WHEN(IN_DATE - DF_DATE)  BETWEEN 8 AND 15 THEN    COUNT(*) ELSE 0 END BETWEEN7TO15,";
        //        strQry += "  CASE   WHEN(IN_DATE - DF_DATE)  BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0   END BETWEEN15TO30, CASE  ";
        //        strQry += "  WHEN(IN_DATE - DF_DATE) > 30 THEN  COUNT(DF_DTC_CODE) ELSE 0 END ABOVE30, FD_FEEDER_NAME, COUNT(*) TOTAL  ";
        //        strQry += "  FROM TBLFEEDERMAST INNER JOIN TBLDTCMAST ON  DT_FDRSLNO = FD_FEEDER_CODE   INNER JOIN TBLDTCFAILURE    ";
        //        strQry += "  ON DF_DTC_CODE = DT_CODE INNER JOIN   TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN    TBLINDENT ON  ";
        //        strQry += "  WO_SLNO = TI_WO_SLNO INNER JOIN TBLDTCINVOICE  ON TI_ID = IN_TI_NO AND DF_REPLACE_FLAG <> 1    AND  ";
        //        strQry += "  DF_STATUS_FLAG IN(1, 4) ";
        //        if (sFromDate != "" && sTodate != "")
        //        {
        //            //DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            //sFromDate = DFromDate.ToString("yyyyMMdd");
        //            //DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            //sTodate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == "")
        //        {

        //            //sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //            //DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            //sFromDate = DFromDate.ToString("yyyyMMdd");
        //            //DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            //sTodate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        else if (sFromDate == string.Empty && sTodate != "")
        //        {
        //            strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        strQry += "  GROUP BY DF_ID, DF_DATE, IN_NO, IN_DATE, DF_LOC_CODE, DF_DTC_CODE, FD_FEEDER_NAME)  ";
        //        strQry += "  LEFT JOIN TBLTCREPLACE ON IN_NO = TR_IN_NO  where IN_NO IS NOT NULL AND TR_IN_NO IS NULL GROUP BY CIRCLE,";
        //        strQry += "  CircleCode, DivisionCode1, DivisionName1 ORDER BY CircleCode,DivisionCode1)A RIGHT JOIN(SELECT DIV_CODE ";
        //        strQry += "  FROM TBLDIVISION WHERE DIV_CICLE_CODE = '" + CirclCode + "')B ON ";
        //        strQry += "  A.DivisionCode1 = B.DIV_CODE)B on    A.CircleCode = B.CircleCode1  and A.DivisionCode = B.DivisionCode1 ";
        //        strQry += "  WHERE B.CircleCode1 = '" + CirclCode + "'   ORDER BY COALESCE(DivisionCode, DivisionCode1), COALESCE(CIRCLE, CIRCLE1) ";
        //        dtDetails = objcon.FetchDataTable(strQry);
        //        return dtDetails;
        //    }


        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadDiviSionDetails");
        //        return dtDetails;

        //    }

        //}


        //public DataTable LoadSubDiviSionDetails(string DivisionCode, string sFromDate, string sTodate)
        //{
        //    DataTable dtDetails = new DataTable();
        //    try
        //    {

        //        //strQry = " SELECT NVL(CircleCode, CircleCode1)CIRCLECODE, NVL(CIRCLE, CIRCLE1)CIRCLE, NVL(DivisionCode, DivisionCode1)DIVISIONCODE, ";
        //        //strQry += " NVL(Division, DivisionName1)DIVISION, NVL(SUBDIVISION, SubDivisionName1)SUBDIVISION, NVL(SubDivisionCode, ";
        //        //strQry += "  SubDivisionCode1)SUBDIVISIONCODE, NVL(LESSTHAN1DAY, 0)LESSTHAN1DAY, NVL(BW1TO7, 0)BW1TO7, NVL(BW7TO15, 0)BW7TO15, ";
        //        //strQry += "  NVL(BW15TO30, 0)BW15TO30, NVL(ABOVE30, 0)ABOVE30, NVL(TOTAL, 0)TOTAL, NVL(LESSTHAN1DAYNEW, 0)LESSTHAN1DAYNEW, ";
        //        //strQry += " NVL(BW1TO7NEW, 0)BW1TO7NEW, NVL(BW7TO15NEW, 0)  BW7TO15NEW, NVL(BW15TO30NEW, 0)BW15TO30NEW, NVL(ABOVE30NEW, 0)ABOVE30NEW, ";
        //        //strQry += "  NVL(TOTALNEW, 0)TOTALNEW FROM(SELECT CircleCode, CIRCLE, DivisionCode, DivisionName as Division, ";
        //        //strQry += "  SubDivisionName as SUBDIVISION, SubDivisionCode, SUM(LESSTHAN1)LESSTHAN1DAY, SUM(BETWEEN1TO7)BW1TO7, ";
        //        //strQry += "  SUM(BETWEEN7TO15)BW7TO15, SUM(BETWEEN15TO30)BW15TO30, SUM(ABOVE30)ABOVE30, SUM(TOTAL)TOTAL fROM(SELECT ";
        //        //strQry += "  (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1)  FROM  VIEW_ALL_OFFICES WHERE  ";
        //        //strQry += "  OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 1))CIRCLE, SUBSTR(DF_LOC_CODE, 0, 1)CircleCode, (SELECT SUBSTR(OFF_NAME,";
        //        //strQry += "  INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES WHERE    OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 2)) DivisionName,";
        //        //strQry += "  SUBSTR(DF_LOC_CODE, 0, 2)DivisionCode, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES ";
        //        //strQry += "  WHERE  OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 3))  SubDivisionName, SUBSTR(DF_LOC_CODE, 0, 3)SubDivisionCode, ";
        //        //strQry += "  CASE WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 0 AND 1 THEN  COUNT(*) ELSE 0 END LESSTHAN1, ";
        //        //strQry += "  CASE WHEN(TR_DECOMM_DATE - DF_DATE)    BETWEEN 2 AND 7 THEN  COUNT(*) ELSE 0 END  BETWEEN1TO7, CASE ";
        //        //strQry += "  WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 8 AND 15 THEN    COUNT(*) ELSE 0 END BETWEEN7TO15, CASE  ";
        //        //strQry += "  WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0   END BETWEEN15TO30, CASE ";
        //        //strQry += "  WHEN(TR_DECOMM_DATE - DF_DATE) > 30 THEN  COUNT(DF_DTC_CODE) ELSE 0 END ABOVE30, FD_FEEDER_NAME, ";
        //        //strQry += "  COUNT(*) TOTAL FROM TBLFEEDERMAST INNER JOIN TBLDTCMAST ON  DT_FDRSLNO = FD_FEEDER_CODE INNER   ";
        //        //strQry += "  JOIN TBLDTCFAILURE  ON DF_DTC_CODE = DT_CODE INNER JOIN   TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN   ";
        //        //strQry += "  TBLINDENT ON    WO_SLNO = TI_WO_SLNO INNER JOIN TBLDTCINVOICE  ON TI_ID = IN_TI_NO INNER JOIN TBLTCREPLACE  ";
        //        //strQry += "  ON IN_NO = TR_IN_NO AND DF_REPLACE_FLAG <> 1  AND DF_STATUS_FLAG IN(1, 4) AND SUBSTR(DF_LOC_CODE, 0, 2) = '"+ DivisionCode + "' ";
        //        //if (sFromDate != "" && sTodate != "")
        //        //{
        //        //    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sFromDate = DFromDate.ToString("yyyyMMdd");
        //        //    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sTodate = DToDate.ToString("yyyyMMdd");
        //        //    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        //}
        //        //else if (sFromDate != "" && sTodate == "")
        //        //{

        //        //    sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //        //    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sFromDate = DFromDate.ToString("yyyyMMdd");
        //        //    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sTodate = DToDate.ToString("yyyyMMdd");
        //        //    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        //}
        //        //else if (sFromDate == string.Empty && sTodate != "")
        //        //{
        //        //    strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        //}

        //        //strQry += "  GROUP BY DF_ID, DF_DATE, TR_DECOMM_DATE, DF_LOC_CODE, DF_DTC_CODE, FD_FEEDER_NAME) A GROUP BY CIRCLE, CircleCode, ";
        //        //strQry += "  DivisionCode, DivisionName, SubDivisionName, SubDivisionCode ORDER BY CircleCode, DivisionCode,SubDivisionCode)A ";
        //        //strQry += "  FULL OUTER JOIN ";
        //        //strQry += "  (SELECT DivisionCode1, DivisionName1, SubDivisionName1, SubDivisionCode1, CircleCode AS CircleCode1, CIRCLE AS CIRCLE1, ";
        //        //strQry += " SUM(LESSTHAN1)LESSTHAN1DAYNEW, SUM(BETWEEN1TO7)BW1TO7NEW, SUM(BETWEEN7TO15)BW7TO15NEW, SUM(BETWEEN15TO30)BW15TO30NEW,";
        //        //strQry += "  SUM(ABOVE30)ABOVE30NEW, SUM(TOTAL)TOTALNEW FROM(SELECT IN_NO, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM";
        //        //strQry += "  VIEW_ALL_OFFICES   WHERE    OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 1))CIRCLE, SUBSTR(DF_LOC_CODE, 0, 1)CircleCode, (SELECT   ";
        //        //strQry += "  SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES WHERE OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 2))   ";
        //        //strQry += "  DivisionName1,SUBSTR(DF_LOC_CODE, 0, 2)DivisionCode1,   (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM ";
        //        //strQry += "  VIEW_ALL_OFFICES WHERE OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 3))  SubDivisionName1, ";
        //        //strQry += "  SUBSTR(DF_LOC_CODE, 0, 3)SubDivisionCode1,    CASE WHEN  (IN_DATE - DF_DATE)  BETWEEN 0 AND 1 THEN COUNT(*) ";
        //        //strQry += "  ELSE 0 END LESSTHAN1, CASE WHEN(IN_DATE - DF_DATE)    BETWEEN 2 AND 7 THEN  COUNT(*) ELSE 0 END BETWEEN1TO7, ";
        //        //strQry += "  CASE WHEN(IN_DATE - DF_DATE)  BETWEEN 8 AND 15 THEN    COUNT(*) ELSE 0 END BETWEEN7TO15, ";
        //        //strQry += "  CASE WHEN(IN_DATE - DF_DATE)  BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0   END BETWEEN15TO30, CASE  ";
        //        //strQry += " WHEN(IN_DATE - DF_DATE) > 30 THEN  COUNT(DF_DTC_CODE) ELSE 0 END ABOVE30, FD_FEEDER_NAME, COUNT(*) TOTAL  ";
        //        //strQry += "  FROM TBLFEEDERMAST INNER JOIN TBLDTCMAST ON  DT_FDRSLNO = FD_FEEDER_CODE   INNER JOIN TBLDTCFAILURE  ";
        //        //strQry += "  ON DF_DTC_CODE = DT_CODE INNER JOIN   TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN    TBLINDENT ON  ";
        //        //strQry += "  WO_SLNO = TI_WO_SLNO INNER JOIN TBLDTCINVOICE  ON TI_ID = IN_TI_NO AND DF_REPLACE_FLAG <> 1  ";
        //        //strQry += "  AND  DF_STATUS_FLAG IN(1, 4)  ";

        //        //if (sFromDate != "" && sTodate != "")
        //        //{
        //        //    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sFromDate = DFromDate.ToString("yyyyMMdd");
        //        //    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sTodate = DToDate.ToString("yyyyMMdd");
        //        //    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        //}
        //        //else if (sFromDate != "" && sTodate == "")
        //        //{

        //        //    sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //        //    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sFromDate = DFromDate.ToString("yyyyMMdd");
        //        //    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sTodate = DToDate.ToString("yyyyMMdd");
        //        //    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        //}
        //        //else if (sFromDate == string.Empty && sTodate != "")
        //        //{
        //        //    strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        //}
        //        //strQry += " GROUP BY DF_ID, DF_DATE, IN_NO, IN_DATE, DF_LOC_CODE, DF_DTC_CODE, FD_FEEDER_NAME)    ";
        //        //strQry += "   LEFT JOIN TBLTCREPLACE ON IN_NO = TR_IN_NO  where IN_NO IS NOT NULL AND TR_IN_NO IS NULL GROUP BY CIRCLE, CircleCode, DivisionCode1,";
        //        //strQry += " DivisionName1, SubDivisionName1, SubDivisionCode1 ORDER BY CircleCode,DivisionCode1)B on   ";
        //        //strQry += " A.CircleCode = B.CircleCode1 and A.DivisionCode = B.DivisionCode1  and A.SubDivisionCode = b.SubDivisionCode1  WHERE B.DivisionCode1 = '"+ DivisionCode + "' ";
        //        //strQry += "  ORDER BY NVL(SubDivisionCode, SubDivisionCode1) ";


        //        strQry = " SELECT NVL(CircleCode, CircleCode1)CIRCLECODE, NVL(CIRCLE, CIRCLE1)CIRCLE, NVL(DivisionCode, DivisionCode1)";
        //        strQry += " DIVISIONCODE, NVL(Division, DivisionName1)DIVISION, NVL(SUBDIVISION, SubDivisionName1)SUBDIVISION, ";
        //        strQry += "  NVL(SubDivisionCode, SubDivisionCode1)SUBDIVISIONCODE, NVL(LESSTHAN1DAY, 0)LESSTHAN1DAY, NVL(BW1TO7, 0)BW1TO7, ";
        //        strQry += " NVL(BW7TO15, 0)BW7TO15, NVL(BW15TO30, 0)BW15TO30, NVL(ABOVE30, 0)ABOVE30, NVL(TOTAL, 0)TOTAL,";
        //        strQry += "  NVL(LESSTHAN1DAYNEW, 0)LESSTHAN1DAYNEW, NVL(BW1TO7NEW, 0)BW1TO7NEW, NVL(BW7TO15NEW, 0)  BW7TO15NEW, ";
        //        strQry += " NVL(BW15TO30NEW, 0)BW15TO30NEW, NVL(ABOVE30NEW, 0)ABOVE30NEW, NVL(TOTALNEW, 0)TOTALNEW FROM(SELECT";
        //        strQry += " SUBSTR(SD_SUBDIV_CODE, 0, 1)CircleCode, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES ";
        //        strQry += " WHERE  OFF_CODE = SUBSTR(SD_SUBDIV_CODE, 0, 1))CIRCLE, SUBSTR(SD_SUBDIV_CODE, 0, 2)DivisionCode, ";
        //        strQry += " (SELECT   SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES  ";
        //        strQry += " WHERE OFF_CODE = SUBSTR(SD_SUBDIV_CODE, 0, 2)) Division, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) ";
        //        strQry += "  FROM VIEW_ALL_OFFICES WHERE OFF_CODE = SUBSTR(SD_SUBDIV_CODE, 0, 3)) SUBDIVISION,  SUBSTR(SD_SUBDIV_CODE, 0, 3) ";
        //        strQry += "  SubDivisionCode,LESSTHAN1DAY,BW1TO7,BW7TO15,BW15TO30,ABOVE30,TOTAL FROM (SELECT CircleCode, CIRCLE, ";
        //        strQry += "  DivisionCode, DivisionName as Division, SubDivisionName as SUBDIVISION, SubDivisionCode, ";
        //        strQry += "  SUM(LESSTHAN1)LESSTHAN1DAY, SUM(BETWEEN1TO7)BW1TO7, SUM(BETWEEN7TO15)BW7TO15, SUM(BETWEEN15TO30)BW15TO30, ";
        //        strQry += "  SUM(ABOVE30)ABOVE30, SUM(TOTAL)TOTAL fROM(SELECT(SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) ";
        //        strQry += "  FROM  VIEW_ALL_OFFICES WHERE    OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 1))CIRCLE, SUBSTR(DF_LOC_CODE, 0, 1) ";
        //        strQry += "  CircleCode, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES WHERE   ";
        //        strQry += "  OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 2)) DivisionName, SUBSTR(DF_LOC_CODE, 0, 2)DivisionCode,";
        //        strQry += "  (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES   WHERE ";
        //        strQry += " OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 3))  SubDivisionName, SUBSTR(DF_LOC_CODE, 0, 3)SubDivisionCode,  ";
        //        strQry += "  CASE WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 0 AND 1 THEN  COUNT(*) ELSE 0 END LESSTHAN1,";
        //        strQry += " CASE WHEN(TR_DECOMM_DATE - DF_DATE)    BETWEEN 2 AND 7 THEN  COUNT(*) ELSE 0 END  BETWEEN1TO7, CASE ";
        //        strQry += "  WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 8 AND 15 THEN    COUNT(*) ELSE 0 END BETWEEN7TO15, CASE   ";
        //        strQry += " WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0   END BETWEEN15TO30, CASE ";
        //        strQry += "  WHEN(TR_DECOMM_DATE - DF_DATE) > 30 THEN  COUNT(DF_DTC_CODE) ELSE 0 END ABOVE30, FD_FEEDER_NAME, COUNT(*) ";
        //        strQry += "  TOTAL FROM TBLFEEDERMAST INNER JOIN TBLDTCMAST ON  DT_FDRSLNO = FD_FEEDER_CODE INNER     ";
        //        strQry += "  JOIN TBLDTCFAILURE  ON DF_DTC_CODE = DT_CODE INNER JOIN   TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN   ";
        //        strQry += "  TBLINDENT ON    WO_SLNO = TI_WO_SLNO INNER JOIN TBLDTCINVOICE  ON TI_ID = IN_TI_NO INNER JOIN TBLTCREPLACE ";
        //        strQry += " ON IN_NO = TR_IN_NO AND DF_REPLACE_FLAG <> 1  AND DF_STATUS_FLAG IN(1, 4) AND SUBSTR(DF_LOC_CODE, 0, 2) = '" + DivisionCode + "'   ";

        //        if (sFromDate != "" && sTodate != "")
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            sFromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            sTodate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == "")
        //        {

        //            sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            sFromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            sTodate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        else if (sFromDate == string.Empty && sTodate != "")
        //        {
        //            strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        strQry += " GROUP BY DF_ID, DF_DATE, TR_DECOMM_DATE, DF_LOC_CODE, DF_DTC_CODE, FD_FEEDER_NAME) A GROUP BY CIRCLE, ";
        //        strQry += " CircleCode, DivisionCode, DivisionName, SubDivisionName, SubDivisionCode ORDER BY CircleCode, ";
        //        strQry += " DivisionCode,SubDivisionCode)A RIGHT JOIN(SELECT SD_SUBDIV_CODE FROM TBLSUBDIVMAST WHERE SD_DIV_CODE = '" + DivisionCode + "' )B ";
        //        strQry += " ON A.SUBDIVISIONCODE = B.SD_SUBDIV_CODE ";
        //        strQry += " )A LEFT JOIN   (SELECT SUBSTR(SD_SUBDIV_CODE, 0, 1)CircleCode1, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM ";
        //        strQry += " VIEW_ALL_OFFICES   WHERE  OFF_CODE = SUBSTR(SD_SUBDIV_CODE, 0, 1))CIRCLE1, SUBSTR(SD_SUBDIV_CODE, 0, 2)DivisionCode1, ";
        //        strQry += " (SELECT   SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES  ";
        //        strQry += " WHERE OFF_CODE = SUBSTR(SD_SUBDIV_CODE, 0, 2)) DivisionNAME1, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) ";
        //        strQry += "  FROM VIEW_ALL_OFFICES WHERE OFF_CODE = SUBSTR(SD_SUBDIV_CODE, 0, 3)) SUBDIVISIONNAME1, ";
        //        strQry += " SUBSTR(SD_SUBDIV_CODE, 0, 3)SubDivisionCode1,LESSTHAN1DAYNEW,BW1TO7NEW,BW7TO15NEW,BW15TO30NEW,ABOVE30NEW,TOTALNEW";
        //        strQry += " FROM ";
        //        strQry += " (SELECT DivisionCode1, DivisionName1, SubDivisionName1, SubDivisionCode1, CircleCode AS CircleCode1, CIRCLE AS CIRCLE1,";
        //        strQry += " SUM(LESSTHAN1)LESSTHAN1DAYNEW, SUM(BETWEEN1TO7)BW1TO7NEW, SUM(BETWEEN7TO15)BW7TO15NEW, SUM(BETWEEN15TO30)BW15TO30NEW, ";
        //        strQry += "  SUM(ABOVE30)ABOVE30NEW, SUM(TOTAL)TOTALNEW FROM(SELECT IN_NO, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM  ";
        //        strQry += " VIEW_ALL_OFFICES   WHERE    OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 1))CIRCLE, SUBSTR(DF_LOC_CODE, 0, 1)CircleCode, (SELECT    ";
        //        strQry += " SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES WHERE OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 2))    ";
        //        strQry += "  DivisionName1,SUBSTR(DF_LOC_CODE, 0, 2)DivisionCode1,   (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES ";
        //        strQry += " WHERE OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 3))  SubDivisionName1,   SUBSTR(DF_LOC_CODE, 0, 3)SubDivisionCode1, ";
        //        strQry += "  CASE WHEN  (IN_DATE - DF_DATE)  BETWEEN 0 AND 1 THEN COUNT(*)   ELSE 0 END LESSTHAN1, CASE WHEN(IN_DATE - DF_DATE) ";
        //        strQry += "  BETWEEN 2 AND 7 THEN  COUNT(*) ELSE 0 END BETWEEN1TO7, CASE WHEN(IN_DATE - DF_DATE)  BETWEEN 8 AND 15 THEN ";
        //        strQry += "  COUNT(*) ELSE 0 END BETWEEN7TO15, CASE WHEN(IN_DATE - DF_DATE)  BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0   ";
        //        strQry += "  END BETWEEN15TO30, CASE   WHEN(IN_DATE - DF_DATE) > 30 THEN  COUNT(DF_DTC_CODE) ELSE 0 END ABOVE30, ";
        //        strQry += " FD_FEEDER_NAME, COUNT(*) TOTAL    FROM TBLFEEDERMAST INNER JOIN TBLDTCMAST ON  DT_FDRSLNO = FD_FEEDER_CODE   ";
        //        strQry += " INNER JOIN TBLDTCFAILURE    ON DF_DTC_CODE = DT_CODE INNER JOIN   TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN ";
        //        strQry += " TBLINDENT ON    WO_SLNO = TI_WO_SLNO INNER JOIN TBLDTCINVOICE  ON TI_ID = IN_TI_NO AND DF_REPLACE_FLAG <> 1 ";
        //        strQry += " AND  DF_STATUS_FLAG IN(1, 4) ";


        //        if (sFromDate != "" && sTodate != "")
        //        {
        //            //DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            //sFromDate = DFromDate.ToString("yyyyMMdd");
        //            //DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            //sTodate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == "")
        //        {

        //            //sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //            //DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            //sFromDate = DFromDate.ToString("yyyyMMdd");
        //            //DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            //sTodate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        else if (sFromDate == string.Empty && sTodate != "")
        //        {
        //            strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        strQry += " GROUP BY DF_ID, DF_DATE, IN_NO, IN_DATE, DF_LOC_CODE, DF_DTC_CODE, FD_FEEDER_NAME) ";
        //        strQry += " LEFT JOIN TBLTCREPLACE ON IN_NO = TR_IN_NO  where IN_NO IS NOT NULL AND TR_IN_NO IS NULL GROUP BY CIRCLE,";
        //        strQry += " CircleCode, DivisionCode1, DivisionName1, SubDivisionName1, SubDivisionCode1 ORDER BY CircleCode,DivisionCode1)A ";
        //        strQry += " RIGHT JOIN(SELECT SD_SUBDIV_CODE FROM TBLSUBDIVMAST WHERE SD_DIV_CODE ='" + DivisionCode + "')B ON ";
        //        strQry += "  A.SubDivisionCode1 = B.SD_SUBDIV_CODE)B on    A.CircleCode = B.CircleCode1 and A.DivisionCode = B.DivisionCode1";
        //        strQry += " and A.SubDivisionCode = b.SubDivisionCode1  WHERE B.DivisionCode1 = '" + DivisionCode + "'   ORDER BY NVL(SubDivisionCode, ";
        //        strQry += " SubDivisionCode1)";
        //        dtDetails = objcon.FetchDataTable(strQry);
        //        return dtDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadSubDiviSionDetails");
        //        return dtDetails;

        //    }

        //}



        //public DataTable LoadSubDiviSionDetails(string DivisionCode, string sFromDate, string sTodate)
        //{
        //    DataTable dtDetails = new DataTable();
        //    try
        //    {




        //        strQry = " SELECT COALESCE(\"CircleCode\",\"CircleCode1\")\"CIRCLECODE\", COALESCE(\"CIRCLE\",\"CIRCLE1\")\"CIRCLE\", COALESCE(\"DivisionCode\",\"DivisionCode1\")";
        //        strQry += " \"DIVISIONCODE\", COALESCE(\"Division\",\"DivisionName1\")\"DIVISION\", COALESCE(\"SUBDIVISION\",\"SubDivisionName1\")\"SUBDIVISION\", ";
        //        strQry += "  COALESCE(\"SubDivisionCode\", \"SubDivisionCode1\")\"SUBDIVISIONCODE\", COALESCE(\"LESSTHAN1DAY\", 0)\"LESSTHAN1DAY\", COALESCE(\"BW1TO7\", 0)\"BW1TO7\", ";
        //        strQry += " COALESCE(\"BW7TO15\", 0)\"BW7TO15\", COALESCE(\"BW15TO30\", 0)\"BW15TO30\", COALESCE(\"ABOVE30\", 0)\"ABOVE30\", COALESCE(\"TOTAL\", 0)\"TOTAL\",";
        //        strQry += "  COALESCE(\"LESSTHAN1DAYNEW\", 0)\"LESSTHAN1DAYNEW\", COALESCE(\"BW1TO7NEW\", 0)\"BW1TO7NEW\", COALESCE(\"BW7TO15NEW\", 0)  \"BW7TO15NEW\", ";
        //        strQry += " COALESCE(\"BW15TO30NEW\", 0)\"BW15TO30NEW\", COALESCE(\"ABOVE30NEW\", 0)\"ABOVE30NEW\", COALESCE(\"TOTALNEW\", 0)\"TOTALNEW\" FROM(SELECT";
        //        strQry += " SUBSTR(\"SD_SUBDIV_CODE\", 1, "+Constants.Circle+")\"CircleCode\", (SELECT SUBSTR(\"OFF_NAME\", INSTR(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" ";
        //        strQry += " WHERE  CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(\"SD_SUBDIV_CODE\", 1, "+Constants.Circle+"))\"CIRCLE\", SUBSTR(\"SD_SUBDIV_CODE\", 1, "+Constants.Division+")\"DivisionCode\", ";
        //        strQry += " (SELECT SUBSTR(\"OFF_NAME\", INSTR(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\"  ";
        //        strQry += " WHERE cast(\"OFF_CODE\" AS TEXT) = SUBSTR(\"SD_SUBDIV_CODE\", 1, 2)) Division, (SELECT SUBSTR(\"OFF_NAME\", INSTR(\"OFF_NAME\", ':') + 1) ";
        //        strQry += "  FROM \"VIEW_ALL_OFFICES\" WHERE \"OFF_CODE\" = SUBSTR(\"SD_SUBDIV_CODE\", 1, " + Constants.SubDivision + ")) SUBDIVISION,  SUBSTR(SD_SUBDIV_CODE, 0, 3) ";
        //        strQry += "  SubDivisionCode,LESSTHAN1DAY,BW1TO7,BW7TO15,BW15TO30,ABOVE30,TOTAL FROM (SELECT CircleCode, CIRCLE, ";
        //        strQry += "  DivisionCode, DivisionName as Division, SubDivisionName as SUBDIVISION, SubDivisionCode, ";
        //        strQry += "  SUM(LESSTHAN1)LESSTHAN1DAY, SUM(BETWEEN1TO7)BW1TO7, SUM(BETWEEN7TO15)BW7TO15, SUM(BETWEEN15TO30)BW15TO30, ";
        //        strQry += "  SUM(ABOVE30)ABOVE30, SUM(TOTAL)TOTAL fROM(SELECT(SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) ";
        //        strQry += "  FROM  VIEW_ALL_OFFICES WHERE    OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 1))CIRCLE, SUBSTR(DF_LOC_CODE, 0, 1) ";
        //        strQry += "  CircleCode, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES WHERE   ";
        //        strQry += "  OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 2)) DivisionName, SUBSTR(DF_LOC_CODE, 0, 2)DivisionCode,";
        //        strQry += "  (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES   WHERE ";
        //        strQry += " OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 3))  SubDivisionName, SUBSTR(DF_LOC_CODE, 0, 3)SubDivisionCode,  ";
        //        strQry += "  CASE WHEN(TR_DECOMM_DATE - \"DF_DATE\")  BETWEEN 0 AND 1 THEN  COUNT(*) ELSE 0 END LESSTHAN1,";
        //        strQry += " CASE WHEN(TR_DECOMM_DATE - \"DF_DATE\")    BETWEEN 2 AND 7 THEN  COUNT(*) ELSE 0 END  BETWEEN1TO7, CASE ";
        //        strQry += "  WHEN(TR_DECOMM_DATE - \"DF_DATE\")  BETWEEN 8 AND 15 THEN    COUNT(*) ELSE 0 END BETWEEN7TO15, CASE   ";
        //        strQry += " WHEN(TR_DECOMM_DATE - \"DF_DATE\")  BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0   END BETWEEN15TO30, CASE ";
        //        strQry += "  WHEN(TR_DECOMM_DATE - \"DF_DATE\") > 30 THEN  COUNT(DF_DTC_CODE) ELSE 0 END ABOVE30, FD_FEEDER_NAME, COUNT(*) ";
        //        strQry += "  TOTAL FROM TBLFEEDERMAST INNER JOIN TBLDTCMAST ON  DT_FDRSLNO = FD_FEEDER_CODE INNER     ";
        //        strQry += "  JOIN TBLDTCFAILURE  ON DF_DTC_CODE = DT_CODE INNER JOIN   TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN   ";
        //        strQry += "  TBLINDENT ON    WO_SLNO = TI_WO_SLNO INNER JOIN TBLDTCINVOICE  ON TI_ID = IN_TI_NO INNER JOIN TBLTCREPLACE ";
        //        strQry += " ON IN_NO = TR_IN_NO AND DF_REPLACE_FLAG <> 1  AND DF_STATUS_FLAG IN(1, 4) AND SUBSTR(DF_LOC_CODE, 0, 2) = '" + DivisionCode + "'   ";

        //        if (sFromDate != "" && sTodate != "")
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            sFromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            sTodate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == "")
        //        {

        //            sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            sFromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            sTodate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        else if (sFromDate == string.Empty && sTodate != "")
        //        {
        //            strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        strQry += " GROUP BY DF_ID, DF_DATE, TR_DECOMM_DATE, DF_LOC_CODE, DF_DTC_CODE, FD_FEEDER_NAME) A GROUP BY CIRCLE, ";
        //        strQry += " CircleCode, DivisionCode, DivisionName, SubDivisionName, SubDivisionCode ORDER BY CircleCode, ";
        //        strQry += " DivisionCode,SubDivisionCode)A RIGHT JOIN(SELECT SD_SUBDIV_CODE FROM TBLSUBDIVMAST WHERE SD_DIV_CODE = '" + DivisionCode + "' )B ";
        //        strQry += " ON A.SUBDIVISIONCODE = B.SD_SUBDIV_CODE ";
        //        strQry += " )A LEFT JOIN   (SELECT SUBSTR(SD_SUBDIV_CODE, 0, 1)CircleCode1, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM ";
        //        strQry += " VIEW_ALL_OFFICES   WHERE  OFF_CODE = SUBSTR(SD_SUBDIV_CODE, 0, 1))CIRCLE1, SUBSTR(SD_SUBDIV_CODE, 0, 2)DivisionCode1, ";
        //        strQry += " (SELECT   SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES  ";
        //        strQry += " WHERE OFF_CODE = SUBSTR(SD_SUBDIV_CODE, 0, 2)) DivisionNAME1, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) ";
        //        strQry += "  FROM VIEW_ALL_OFFICES WHERE OFF_CODE = SUBSTR(SD_SUBDIV_CODE, 0, 3)) SUBDIVISIONNAME1, ";
        //        strQry += " SUBSTR(SD_SUBDIV_CODE, 0, 3)SubDivisionCode1,LESSTHAN1DAYNEW,BW1TO7NEW,BW7TO15NEW,BW15TO30NEW,ABOVE30NEW,TOTALNEW";
        //        strQry += " FROM ";
        //        strQry += " (SELECT DivisionCode1, DivisionName1, SubDivisionName1, SubDivisionCode1, CircleCode AS CircleCode1, CIRCLE AS CIRCLE1,";
        //        strQry += " SUM(LESSTHAN1)LESSTHAN1DAYNEW, SUM(BETWEEN1TO7)BW1TO7NEW, SUM(BETWEEN7TO15)BW7TO15NEW, SUM(BETWEEN15TO30)BW15TO30NEW, ";
        //        strQry += "  SUM(ABOVE30)ABOVE30NEW, SUM(TOTAL)TOTALNEW FROM(SELECT IN_NO, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM  ";
        //        strQry += " VIEW_ALL_OFFICES   WHERE    OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 1))CIRCLE, SUBSTR(DF_LOC_CODE, 0, 1)CircleCode, (SELECT    ";
        //        strQry += " SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES WHERE OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 2))    ";
        //        strQry += "  DivisionName1,SUBSTR(DF_LOC_CODE, 0, 2)DivisionCode1,   (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES ";
        //        strQry += " WHERE OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 3))  SubDivisionName1,   SUBSTR(DF_LOC_CODE, 0, 3)SubDivisionCode1, ";
        //        strQry += "  CASE WHEN  (IN_DATE - DF_DATE)  BETWEEN 0 AND 1 THEN COUNT(*)   ELSE 0 END LESSTHAN1, CASE WHEN(IN_DATE - DF_DATE) ";
        //        strQry += "  BETWEEN 2 AND 7 THEN  COUNT(*) ELSE 0 END BETWEEN1TO7, CASE WHEN(IN_DATE - DF_DATE)  BETWEEN 8 AND 15 THEN ";
        //        strQry += "  COUNT(*) ELSE 0 END BETWEEN7TO15, CASE WHEN(IN_DATE - DF_DATE)  BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0   ";
        //        strQry += "  END BETWEEN15TO30, CASE   WHEN(IN_DATE - DF_DATE) > 30 THEN  COUNT(DF_DTC_CODE) ELSE 0 END ABOVE30, ";
        //        strQry += " FD_FEEDER_NAME, COUNT(*) TOTAL    FROM TBLFEEDERMAST INNER JOIN TBLDTCMAST ON  DT_FDRSLNO = FD_FEEDER_CODE   ";
        //        strQry += " INNER JOIN TBLDTCFAILURE    ON DF_DTC_CODE = DT_CODE INNER JOIN   TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN ";
        //        strQry += " TBLINDENT ON    WO_SLNO = TI_WO_SLNO INNER JOIN TBLDTCINVOICE  ON TI_ID = IN_TI_NO AND DF_REPLACE_FLAG <> 1 ";
        //        strQry += " AND  DF_STATUS_FLAG IN(1, 4) ";


        //        if (sFromDate != "" && sTodate != "")
        //        {
        //            //DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            //sFromDate = DFromDate.ToString("yyyyMMdd");
        //            //DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            //sTodate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == "")
        //        {

        //            //sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //            //DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            //sFromDate = DFromDate.ToString("yyyyMMdd");
        //            //DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            //sTodate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        else if (sFromDate == string.Empty && sTodate != "")
        //        {
        //            strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        strQry += " GROUP BY DF_ID, DF_DATE, IN_NO, IN_DATE, DF_LOC_CODE, DF_DTC_CODE, FD_FEEDER_NAME) ";
        //        strQry += " LEFT JOIN TBLTCREPLACE ON IN_NO = TR_IN_NO  where IN_NO IS NOT NULL AND TR_IN_NO IS NULL GROUP BY CIRCLE,";
        //        strQry += " CircleCode, DivisionCode1, DivisionName1, SubDivisionName1, SubDivisionCode1 ORDER BY CircleCode,DivisionCode1)A ";
        //        strQry += " RIGHT JOIN(SELECT SD_SUBDIV_CODE FROM TBLSUBDIVMAST WHERE SD_DIV_CODE ='" + DivisionCode + "')B ON ";
        //        strQry += "  A.SubDivisionCode1 = B.SD_SUBDIV_CODE)B on    A.CircleCode = B.CircleCode1 and A.DivisionCode = B.DivisionCode1";
        //        strQry += " and A.SubDivisionCode = b.SubDivisionCode1  WHERE B.DivisionCode1 = '" + DivisionCode + "'   ORDER BY COALESCE(SubDivisionCode, ";
        //        strQry += " SubDivisionCode1)";
        //        dtDetails = objcon.FetchDataTable(strQry);
        //        return dtDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadSubDiviSionDetails");
        //        return dtDetails;

        //    }

        //}
        //public DataTable LoadSubDiviSionDetails(string DivisionCode, string sFromDate, string sTodate)
        //{
        //    DataTable dtDetails = new DataTable();
        //    try
        //    {

        //        strQry = " SELECT COALESCE(\"CIRCLECODE\",\"CIRCLECODE1\")\"CIRCLECODE\", COALESCE(\"CIRCLE\",\"CIRCLE1\")\"CIRCLE\", COALESCE(\"DIVISIONCODE\",\"DIVISIONCODE1\")";
        //        strQry += " \"DIVISIONCODE\", COALESCE(\"DIVISION\",\"DIVISIONNAME1\")\"DIVISION\", COALESCE(\"SUBDIVISION\",\"SUBDIVISIONNAME1\")\"SUBDIVISION\", ";
        //        strQry += "  COALESCE(\"SUBDIVISIONCODE\", \"SUBDIVISIONCODE1\")\"SUBDIVISIONCODE\", COALESCE(\"LESSTHAN1DAY\", 0)\"LESSTHAN1DAY\", COALESCE(\"BW1TO7\", 0)\"BW1TO7\", ";
        //        strQry += " COALESCE(\"BW7TO15\", 0)\"BW7TO15\", COALESCE(\"BW15TO30\", 0)\"BW15TO30\", COALESCE(\"ABOVE30\", 0)\"ABOVE30\", COALESCE(\"TOTAL\", 0)\"TOTAL\",";
        //        strQry += "  COALESCE(\"LESSTHAN1DAYNEW\", 0)\"LESSTHAN1DAYNEW\", COALESCE(\"BW1TO7NEW\", 0)\"BW1TO7NEW\", COALESCE(\"BW7TO15NEW\", 0)  \"BW7TO15NEW\", ";
        //        strQry += " COALESCE(\"BW15TO30NEW\", 0)\"BW15TO30NEW\", COALESCE(\"ABOVE30NEW\", 0)\"ABOVE30NEW\", COALESCE(\"TOTALNEW\", 0)\"TOTALNEW\" FROM(SELECT";
        //        strQry += " SUBSTR(cast(\"SD_SUBDIV_CODE\" as text), 1, " + Constants.Circle + ")\"CIRCLECODE\", (SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" ";
        //        strQry += " WHERE  CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"SD_SUBDIV_CODE\" as text), 1, " + Constants.Circle + "))\"CIRCLE\", SUBSTR(cast(\"SD_SUBDIV_CODE\" as text), 1, " + Constants.Division + ")\"DIVISIONCODE\", ";
        //        strQry += " (SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\"  ";
        //        strQry += " WHERE cast(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"SD_SUBDIV_CODE\" as text), 1, 2)) \"DIVISION\", (SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) ";
        //        strQry += "  FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT)= SUBSTR(cast(\"SD_SUBDIV_CODE\" as text), 1, " + Constants.SubDivision + ")) \"SUBDIVISION\",  SUBSTR(cast(\"SD_SUBDIV_CODE\" as text), 1," + Constants.SubDivision + " ) ";
        //        strQry += "  \"SUBDIVISIONCODE\",\"LESSTHAN1DAY\",\"BW1TO7\",\"BW7TO15\",\"BW15TO30\",\"ABOVE30\",\"TOTAL\" FROM (SELECT \"CIRCLECODE\", \"CIRCLE\", ";
        //        strQry += "  \"DIVISIONCODE\", \"DIVISIONNAME\" as \"DIVISION\", \"SUBDIVISIONNAME\" as \"SUBDIVISION\", \"SUBDIVISIONCODE\", ";
        //        strQry += "  SUM(\"LESSTHAN1\")\"LESSTHAN1DAY\", SUM(\"BETWEEN1TO7\")\"BW1TO7\", SUM(\"BETWEEN7TO15\")\"BW7TO15\", SUM(\"BETWEEN15TO30\")\"BW15TO30\", ";
        //        strQry += "  SUM(\"ABOVE30\")\"ABOVE30\", SUM(\"TOTAL\")\"TOTAL\" fROM(SELECT(SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) ";
        //        strQry += "  FROM  \"VIEW_ALL_OFFICES\" WHERE    CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DF_LOC_CODE\" as text), 1," + Constants.Circle + " ))\"CIRCLE\", SUBSTR(cast(\"DF_LOC_CODE\" as text), 1, " + Constants.Division + ") ";
        //        strQry += "  \"CIRCLECODE\", (SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE   ";
        //        strQry += "  CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DF_LOC_CODE\" as text), 1, " + Constants.Division + ")) \"DIVISIONNAME\", SUBSTR(cast(\"DF_LOC_CODE\" as text), 1, " + Constants.Division + ")\"DIVISIONCODE\",";
        //        strQry += "  (SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\"   WHERE ";
        //        strQry += " CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DF_LOC_CODE\" as text), 1, " + Constants.SubDivision + "))\"SUBDIVISIONNAME\", SUBSTR(cast(\"DF_LOC_CODE\" as text), 1, " + Constants.SubDivision + ")\"SUBDIVISIONCODE\",  ";
        //        strQry += "  CASE WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")  BETWEEN 0 AND 1 THEN  COUNT(*) ELSE 0 END \"LESSTHAN1\",";
        //        strQry += " CASE WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")    BETWEEN 2 AND 7 THEN  COUNT(*) ELSE 0 END  \"BETWEEN1TO7\", CASE ";
        //        strQry += "  WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")  BETWEEN 8 AND 15 THEN    COUNT(*) ELSE 0 END \"BETWEEN7TO15\", CASE   ";
        //        strQry += " WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")  BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0   END \"BETWEEN15TO30\", CASE ";
        //        strQry += "  WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\") > 30 THEN  COUNT(\"DF_DTC_CODE\") ELSE 0 END \"ABOVE30\", \"FD_FEEDER_NAME\", COUNT(*) ";
        //        strQry += "  \"TOTAL\" FROM \"TBLFEEDERMAST\" INNER JOIN \"TBLDTCMAST\" ON  \"DT_FDRSLNO\" = \"FD_FEEDER_CODE\" INNER     ";
        //        strQry += "  JOIN \"TBLDTCFAILURE\"  ON \"DF_DTC_CODE\" = \"DT_CODE\" INNER JOIN   \"TBLWORKORDER\" ON \"DF_ID\" = \"WO_DF_ID\" INNER JOIN   ";
        //        strQry += "  \"TBLINDENT\" ON    \"WO_SLNO\" = \"TI_WO_SLNO\" INNER JOIN \"TBLDTCINVOICE\"  ON \"TI_ID\" = \"IN_TI_NO\" INNER JOIN \"TBLTCREPLACE\" ";
        //        strQry += " ON \"IN_NO\" = \"TR_IN_NO\" AND \"DF_REPLACE_FLAG\" <> 1  AND \"DF_STATUS_FLAG\" IN(1, 4) AND SUBSTR(cast(\"DF_LOC_CODE\" as text), 1, " + Constants.Division + ") = '" + DivisionCode + "'   ";

        //        if (sFromDate != "" && sTodate != "")
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == "")
        //        {

        //            sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate == string.Empty && sTodate != "")
        //        {
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == string.Empty)
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "'";
        //        }
        //        strQry += " GROUP BY \"DF_ID\", \"DF_DATE\", \"TR_DECOMM_DATE\", \"DF_LOC_CODE\", \"DF_DTC_CODE\", \"FD_FEEDER_NAME\") A GROUP BY \"CIRCLE\", ";
        //        strQry += " \"CIRCLECODE\", \"DIVISIONCODE\", \"DIVISIONNAME\", \"SUBDIVISIONNAME\", \"SUBDIVISIONCODE\" ORDER BY \"CIRCLECODE\", ";
        //        strQry += " \"DIVISIONCODE\",\"SUBDIVISIONCODE\")A INNER JOIN(SELECT \"SD_SUBDIV_CODE\" FROM \"TBLSUBDIVMAST\" WHERE \"SD_DIV_CODE\" = '" + DivisionCode + "' )B ";
        //        strQry += " ON A.\"SUBDIVISIONCODE\" = cast(B.\"SD_SUBDIV_CODE\" as text) ";
        //        strQry += " )A FULL OUTER JOIN   (SELECT SUBSTR(cast(\"SD_SUBDIV_CODE\" as text), 1," + Constants.Circle + " )\"CIRCLECODE1\", (SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM ";
        //        strQry += " \"VIEW_ALL_OFFICES\"   WHERE  CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"SD_SUBDIV_CODE\" as text), 1," + Constants.Circle + " ))\"CIRCLE1\", SUBSTR(cast(\"SD_SUBDIV_CODE\" as text), 1," + Constants.Division + " )\"DIVISIONCODE1\", ";
        //        strQry += " (SELECT   SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\"  ";
        //        strQry += " WHERE CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"SD_SUBDIV_CODE\" as text), 1," + Constants.Division + " ))\"DIVISIONNAME1\", (SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) ";
        //        strQry += "  FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"SD_SUBDIV_CODE\" as text), 1, " + Constants.SubDivision + "))\"SUBDIVISIONNAME1\", ";
        //        strQry += " SUBSTR(cast(\"SD_SUBDIV_CODE\" as text), 1, " + Constants.SubDivision + ")\"SUBDIVISIONCODE1\",\"LESSTHAN1DAYNEW\",\"BW1TO7NEW\",\"BW7TO15NEW\",\"BW15TO30NEW\",\"ABOVE30NEW\",\"TOTALNEW\"";
        //        strQry += " FROM ";
        //        strQry += " (SELECT \"DIVISIONCODE1\", \"DIVISIONNAME1\", \"SUBDIVISIONNAME1\", \"SUBDIVISIONCODE1\", \"CIRCLECODE\" AS \"CIRCLECODE1\", \"CIRCLE\" AS \"CIRCLE1\",";
        //        strQry += " SUM(\"LESSTHAN1\")\"LESSTHAN1DAYNEW\", SUM(\"BETWEEN1TO7\")\"BW1TO7NEW\", SUM(\"BETWEEN7TO15\")\"BW7TO15NEW\", SUM(\"BETWEEN15TO30\")\"BW15TO30NEW\", ";
        //        strQry += "  SUM(\"ABOVE30\")\"ABOVE30NEW\", SUM(\"TOTAL\")\"TOTALNEW\" FROM(SELECT \"IN_NO\", (SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM  ";
        //        strQry += " \"VIEW_ALL_OFFICES\"   WHERE    CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DF_LOC_CODE\" as text), 1," + Constants.Circle + " ))\"CIRCLE\", SUBSTR(cast(\"DF_LOC_CODE\" as text), 1, " + Constants.Circle + ")\"CIRCLECODE\", (SELECT    ";
        //        strQry += " SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DF_LOC_CODE\" as text), 1, " + Constants.Division + "))    ";
        //        strQry += "  \"DIVISIONNAME1\",SUBSTR(cast(\"DF_LOC_CODE\" as text), 1," + Constants.Division + " )\"DIVISIONCODE1\",   (SELECT SUBSTR(cast(\"OFF_NAME\" as text), STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" ";
        //        strQry += " WHERE CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"DF_LOC_CODE\" as text), 1," + Constants.SubDivision + " ))\"SUBDIVISIONNAME1\",   SUBSTR(cast(\"DF_LOC_CODE\" as text), 1," + Constants.SubDivision + " )\"SUBDIVISIONCODE1\", ";
        //        strQry += "  CASE WHEN  (\"IN_DATE\" - \"DF_DATE\")  BETWEEN 0 AND 1 THEN COUNT(*)   ELSE 0 END \"LESSTHAN1\", CASE WHEN(\"IN_DATE\" - \"DF_DATE\") ";
        //        strQry += "  BETWEEN 2 AND 7 THEN  COUNT(*) ELSE 0 END \"BETWEEN1TO7\", CASE WHEN(\"IN_DATE\" - \"DF_DATE\")  BETWEEN 8 AND 15 THEN ";
        //        strQry += "  COUNT(*) ELSE 0 END \"BETWEEN7TO15\", CASE WHEN(\"IN_DATE\" - \"DF_DATE\")  BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0   ";
        //        strQry += "  END \"BETWEEN15TO30\", CASE   WHEN(\"IN_DATE\" - \"DF_DATE\") > 30 THEN  COUNT(\"DF_DTC_CODE\") ELSE 0 END \"ABOVE30\", ";
        //        strQry += " \"FD_FEEDER_NAME\", COUNT(*) \"TOTAL\"    FROM \"TBLFEEDERMAST\" INNER JOIN \"TBLDTCMAST\" ON  \"DT_FDRSLNO\" = \"FD_FEEDER_CODE\"   ";
        //        strQry += " INNER JOIN \"TBLDTCFAILURE\"    ON \"DF_DTC_CODE\" = \"DT_CODE\" INNER JOIN   \"TBLWORKORDER\" ON \"DF_ID\" = \"WO_DF_ID\" INNER JOIN ";
        //        strQry += " \"TBLINDENT\" ON    \"WO_SLNO\"= \"TI_WO_SLNO\" INNER JOIN \"TBLDTCINVOICE\"  ON \"TI_ID\" = \"IN_TI_NO\" AND \"DF_REPLACE_FLAG\" <> 1 ";
        //        strQry += " AND  \"DF_STATUS_FLAG\" IN(1, 4) ";


        //        if (sFromDate != "" && sTodate != "")
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == "")
        //        {

        //            sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate == string.Empty && sTodate != "")
        //        {
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == string.Empty)
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "'";
        //        }
        //        strQry += " GROUP BY \"DF_ID\", \"DF_DATE\", \"IN_NO\", \"IN_DATE\", \"DF_LOC_CODE\", \"DF_DTC_CODE\", \"FD_FEEDER_NAME\")A ";
        //        strQry += " LEFT JOIN \"TBLTCREPLACE\" ON \"IN_NO\" = \"TR_IN_NO\"  where \"IN_NO\" IS NOT NULL AND \"TR_IN_NO\" IS NULL GROUP BY \"CIRCLE\",";
        //        strQry += " \"CIRCLECODE\", \"DIVISIONCODE1\", \"DIVISIONNAME1\", \"SUBDIVISIONNAME1\", \"SUBDIVISIONCODE1\" ORDER BY \"CIRCLECODE\",\"DIVISIONCODE1\")A ";
        //        strQry += " INNER JOIN(SELECT \"SD_SUBDIV_CODE\" FROM \"TBLSUBDIVMAST\" WHERE \"SD_DIV_CODE\" ='" + DivisionCode + "')B ON ";
        //        strQry += "  A.\"SUBDIVISIONCODE1\" =cast( B.\"SD_SUBDIV_CODE\" as text))B on    A.\"CIRCLECODE\"= B.\"CIRCLECODE1\" and A.\"DIVISIONCODE\" = B.\"DIVISIONCODE1\"";
        //        strQry += " and A.\"SUBDIVISIONCODE\" =cast( b.\"SUBDIVISIONCODE1\" as text)  ORDER BY COALESCE(\"SUBDIVISIONCODE\", ";
        //        strQry += " \"SUBDIVISIONCODE1\")";
        //        dtDetails = objcon.FetchDataTable(strQry);
        //        return dtDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return dtDetails;

        //    }

        //}


        //public DataTable LoadSectionDetails(string SubDivisionCode, string sFromDate, string sTodate)
        //{
        //    DataTable dtDetails = new DataTable();
        //    try
        //    {

        //        //strQry = " SELECT NVL(CircleCode, CircleCode1)CIRCLECODE, NVL(CIRCLE, CIRCLE1)CIRCLE, NVL(DivisionCode, DivisionCode1)DIVISIONCODE, ";
        //        //strQry += " NVL(Division, DivisionName1)DIVISION, NVL(SUBDIVISION, SubDivisionName1)SUBDIVISION, NVL(SubDivisionCode,";
        //        //strQry += " SubDivisionCode1)SUBDIVISIONCODE, NVL(sectioncode, sectioncode1)SECTIONCODE, NVL(SECTION, SectionName1)SECTION,";
        //        //strQry += " NVL(LESSTHAN1DAY, 0)LESSTHAN1DAY, NVL(BW1TO7, 0)BW1TO7, NVL(BW7TO15, 0)BW7TO15, NVL(BW15TO30, 0)BW15TO30,";
        //        //strQry += " NVL(ABOVE30, 0)ABOVE30, NVL(TOTAL, 0)TOTAL, NVL(LESSTHAN1DAYNEW, 0)LESSTHAN1DAYNEW, NVL(BW1TO7NEW, 0)BW1TO7NEW,";
        //        //strQry += "  NVL(BW7TO15NEW, 0)BW7TO15NEW, NVL(BW15TO30NEW, 0)BW15TO30NEW, NVL(ABOVE30NEW, 0)ABOVE30NEW, NVL(TOTALNEW, 0)TOTALNEW ";
        //        //strQry += " FROM(SELECT CircleCode, CIRCLE, DivisionCode, DivisionName as Division, SubDivisionName as SUBDIVISION,";
        //        //strQry += "  SubDivisionCode, Sectionname as SECTION, sectioncode, SUM(LESSTHAN1)LESSTHAN1DAY, SUM(BETWEEN1TO7)BW1TO7, ";
        //        //strQry += " SUM(BETWEEN7TO15)BW7TO15, SUM(BETWEEN15TO30)BW15TO30, SUM(ABOVE30)ABOVE30, SUM(TOTAL)TOTAL fROM(SELECT ";
        //        //strQry += "  (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1)  FROM VIEW_ALL_OFFICES WHERE  OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 1))";
        //        //strQry += "  CIRCLE, SUBSTR(DF_LOC_CODE, 0, 1)CircleCode, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES ";
        //        //strQry += "  WHERE  OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 2)) DivisionName, SUBSTR(DF_LOC_CODE, 0, 2)DivisionCode, ";
        //        //strQry += "  (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES WHERE  OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 3))";
        //        //strQry += "  SubDivisionName, SUBSTR(DF_LOC_CODE, 0, 3)SubDivisionCode, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) ";
        //        //strQry += "  FROM VIEW_ALL_OFFICES WHERE OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 4)) SectionName, SUBSTR(DF_LOC_CODE, 0, 4)sectioncode, ";
        //        //strQry += "  CASE WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 0 AND 1 THEN  COUNT(*) ELSE 0 END LESSTHAN1, CASE ";
        //        //strQry += "  WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 2 AND 7 THEN  COUNT(*) ELSE 0 END  BETWEEN1TO7, CASE ";
        //        //strQry += "  WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 8 AND 15 THEN  COUNT(*) ELSE 0 END BETWEEN7TO15, CASE  ";
        //        //strQry += "  WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0 END BETWEEN15TO30, CASE ";
        //        //strQry += " WHEN(TR_DECOMM_DATE - DF_DATE) > 30 THEN  COUNT(DF_DTC_CODE) ELSE 0 END ABOVE30, FD_FEEDER_NAME, COUNT(*) TOTAL";
        //        //strQry += " FROM TBLFEEDERMAST INNER JOIN TBLDTCMAST ON  DT_FDRSLNO = FD_FEEDER_CODE INNER JOIN TBLDTCFAILURE  ";
        //        //strQry += "  ON DF_DTC_CODE = DT_CODE INNER JOIN   TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN  TBLINDENT ON   ";
        //        //strQry += "  WO_SLNO = TI_WO_SLNO INNER JOIN TBLDTCINVOICE  ON TI_ID = IN_TI_NO INNER JOIN TBLTCREPLACE ON ";
        //        //strQry += "  IN_NO = TR_IN_NO AND DF_REPLACE_FLAG <> 1  AND DF_STATUS_FLAG IN(1, 4) AND SUBSTR(DF_LOC_CODE, 0, 3) = '"+ SubDivisionCode + "'  ";
        //        //if (sFromDate != "" && sTodate != "")
        //        //{
        //        //    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sFromDate = DFromDate.ToString("yyyyMMdd");
        //        //    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sTodate = DToDate.ToString("yyyyMMdd");
        //        //    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        //}
        //        //else if (sFromDate != "" && sTodate == "")
        //        //{

        //        //    sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //        //    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sFromDate = DFromDate.ToString("yyyyMMdd");
        //        //    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sTodate = DToDate.ToString("yyyyMMdd");
        //        //    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        //}
        //        //else if (sFromDate == string.Empty && sTodate != "")
        //        //{
        //        //    strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        //}


        //        //strQry += " GROUP BY DF_ID, DF_DATE, TR_DECOMM_DATE, DF_LOC_CODE, DF_DTC_CODE, FD_FEEDER_NAME) A GROUP BY CIRCLE, CircleCode, ";
        //        //strQry += "  DivisionCode, DivisionName, SubDivisionName, SubDivisionCode, SectionName, sectioncode ORDER BY CircleCode,";
        //        //strQry += "  DivisionCode,SubDivisionCode)A full outer JOIN ";
        //        //strQry += "  (SELECT DivisionCode1, DivisionName1, SubDivisionName1, SubDivisionCode1, SectionName1, sectioncode1, CircleCode AS";
        //        //strQry += "  CircleCode1, CIRCLE AS CIRCLE1, SUM(LESSTHAN1)LESSTHAN1DAYNEW, SUM(BETWEEN1TO7)BW1TO7NEW, SUM(BETWEEN7TO15)BW7TO15NEW,";
        //        //strQry += " SUM(BETWEEN15TO30)BW15TO30NEW, SUM(ABOVE30)ABOVE30NEW, SUM(TOTAL)TOTALNEW FROM(SELECT IN_NO, (SELECT SUBSTR(OFF_NAME,";
        //        //strQry += "  INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES   WHERE  OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 1))CIRCLE, ";
        //        //strQry += "  SUBSTR(DF_LOC_CODE, 0, 1)CircleCode, (SELECT   SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES ";
        //        //strQry += "  WHERE OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 2))  DivisionName1,SUBSTR(DF_LOC_CODE, 0, 2)DivisionCode1, ";
        //        //strQry += "  (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) FROM VIEW_ALL_OFFICES WHERE OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 3)) ";
        //        //strQry += "  SubDivisionName1, SUBSTR(DF_LOC_CODE, 0, 3)SubDivisionCode1,  (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':') + 1) ";
        //        //strQry += "  FROM VIEW_ALL_OFFICES WHERE OFF_CODE = SUBSTR(DF_LOC_CODE, 0, 4)) SectionName1, SUBSTR(DF_LOC_CODE, 0, 4) ";
        //        //strQry += " sectioncode1,  CASE WHEN  (IN_DATE - DF_DATE)  BETWEEN 0 AND 1 THEN COUNT(*) ELSE 0 END LESSTHAN1, ";
        //        //strQry += " CASE WHEN(IN_DATE - DF_DATE)  BETWEEN 2 AND 7 THEN  COUNT(*) ELSE 0 END BETWEEN1TO7, CASE WHEN(IN_DATE - DF_DATE) ";
        //        //strQry += " BETWEEN 8 AND 15 THEN  COUNT(*) ELSE 0 END BETWEEN7TO15, CASE WHEN(IN_DATE - DF_DATE)  BETWEEN 16 AND 30 THEN  ";
        //        //strQry += "  COUNT(*) ELSE 0 END BETWEEN15TO30, CASE   WHEN(IN_DATE - DF_DATE) > 30 THEN  COUNT(DF_DTC_CODE) ELSE 0 END ABOVE30,";
        //        //strQry += "  FD_FEEDER_NAME, COUNT(*) TOTAL  FROM TBLFEEDERMAST INNER JOIN TBLDTCMAST ON  DT_FDRSLNO = FD_FEEDER_CODE INNER JOIN ";
        //        //strQry += "  TBLDTCFAILURE   ON DF_DTC_CODE = DT_CODE INNER JOIN   TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN  TBLINDENT ON ";
        //        //strQry += "  WO_SLNO = TI_WO_SLNO INNER JOIN TBLDTCINVOICE  ON TI_ID = IN_TI_NO AND DF_REPLACE_FLAG <> 1 AND  DF_STATUS_FLAG ";
        //        //strQry += " IN(1, 4) ";
        //        //if (sFromDate != "" && sTodate != "")
        //        //{
        //        //    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sFromDate = DFromDate.ToString("yyyyMMdd");
        //        //    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sTodate = DToDate.ToString("yyyyMMdd");
        //        //    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        //}
        //        //else if (sFromDate != "" && sTodate == "")
        //        //{

        //        //    sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //        //    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sFromDate = DFromDate.ToString("yyyyMMdd");
        //        //    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sTodate = DToDate.ToString("yyyyMMdd");
        //        //    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        //}
        //        //else if (sFromDate == string.Empty && sTodate != "")
        //        //{
        //        //    strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        //}


        //        //strQry += "  GROUP BY DF_ID, DF_DATE, IN_NO, IN_DATE, DF_LOC_CODE, DF_DTC_CODE, FD_FEEDER_NAME)  LEFT JOIN TBLTCREPLACE ";
        //        //strQry += "  ON IN_NO = TR_IN_NO  where IN_NO IS NOT NULL AND TR_IN_NO IS NULL GROUP BY CIRCLE, CircleCode, DivisionCode1,";
        //        //strQry += " DivisionName1, SubDivisionName1, SubDivisionCode1, SectionName1, sectioncode1 ORDER BY CircleCode,DivisionCode1)B ";
        //        //strQry += "  on   A.CircleCode = B.CircleCode1 and A.DivisionCode = B.DivisionCode1  and A.SubDivisionCode = b.SubDivisionCode1 ";
        //        //strQry += "  and A.sectioncode = B.sectioncode1 where b.SubDivisionCode1 = '"+ SubDivisionCode + "'";
        //        //strQry += " ORDER BY  NVL(sectioncode, sectioncode1) ";


        //        strQry = " SELECT COALESCE(\"CIRCLECODE\", \"CIRCLECODE1\")\"CIRCLECODE\", COALESCE(\"CIRCLE\", \"CIRCLE1\")\"CIRCLE\", COALESCE(\"DIVISIONCODE\", \"DIVISIONCODE1\")\"DIVISIONCODE\",";
        //        strQry += " COALESCE(\"DIVISION\",\"DIVISIONNAME1\")\"DIVISION\", COALESCE(\"SUBDIVISION\", \"SUBDIVISIONNAME1\")\"SUBDIVISION\", COALESCE(\"SUBDIVISIONCODE\", ";
        //        strQry += " \"SUBDIVISIONCODE1\")\"SUBDIVISIONCODE\", COALESCE(\"SECTIONCODE\", \"SECTIONCODE1\")\"SECTIONCODE\", COALESCE(\"SECTION\", \"SECTIONNAME1\")\"SECTION\",";
        //        strQry += " COALESCE(\"LESSTHAN1DAY\", 0)\"LESSTHAN1DAY\", COALESCE(\"BW1TO7\", 0)\"BW1TO7\", COALESCE(\"BW7TO15\", 0)\"BW7TO15\", COALESCE(\"BW15TO30\", 0)\"BW15TO30\", ";
        //        strQry += "  COALESCE(\"ABOVE30\", 0)\"ABOVE30\", COALESCE(\"TOTAL\", 0)\"TOTAL\", COALESCE(\"LESSTHAN1DAYNEW\", 0)\"LESSTHAN1DAYNEW\", COALESCE(\"BW1TO7NEW\", 0)\"BW1TO7NEW\",";
        //        strQry += " COALESCE(\"BW7TO15NEW\", 0)\"BW7TO15NEW\", COALESCE(\"BW15TO30NEW\", 0)\"BW15TO30NEW\", COALESCE(\"ABOVE30NEW\", 0)\"ABOVE30NEW\", COALESCE(\"TOTALNEW\", 0)\"TOTALNEW\" ";
        //        strQry += "  FROM(SELECT SUBSTR(cast(\"OM_CODE\" as text), 1, " + Constants.Circle + ")\"CIRCLECODE\", (SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" ";
        //        strQry += "  WHERE  CAST(\"OFF_CODE\" AS TEXT)= SUBSTR(cast(\"OM_CODE\" as text), 1, " + Constants.Circle + "))\"CIRCLE\", SUBSTR(cast(\"OM_CODE\" as text), 1, " + Constants.Division + ")\"DIVISIONCODE\", (SELECT  ";
        //        strQry += "  SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\"   WHERE CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(cast(\"OM_CODE\" as text), 1, " + Constants.Division + ")) ";
        //        strQry += "  \"DIVISION\", (SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT) = ";
        //        strQry += " SUBSTR(CAST(\"OM_CODE\" AS TEXT), 1," + Constants.SubDivision + " )) \"SUBDIVISION\",  SUBSTR(cast(\"OM_CODE\" as text), 1, " + Constants.SubDivision + ")\"SUBDIVISIONCODE\",(SELECT SUBSTR(\"OFF_NAME\", ";
        //        strQry += " STRPOS(\"OFF_NAME\", ':') + 1)   FROM \"VIEW_ALL_OFFICES\" WHERE \"OFF_CODE\" = \"OM_CODE\") \"SECTION\",\"OM_CODE\" as ";
        //        strQry += " \"SECTIONCODE\",\"LESSTHAN1DAY\",\"BW1TO7\",\"BW7TO15\",\"BW15TO30\",\"ABOVE30\",\"TOTAL\" FROM(SELECT \"CIRCLECODE\", \"CIRCLE\", ";
        //        strQry += " \"DIVISIONCODE\", \"DIVISIONNAME\" as \"DIVISION\", \"SUBDIVISIONNAME\" as \"SUBDIVISION\", \"SUBDIVISIONCODE\", ";
        //        strQry += "  \"SECTIONNAME\" as \"SECTION\", \"SECTIONCODE\", SUM(\"LESSTHAN1\")\"LESSTHAN1DAY\", SUM(\"BETWEEN1TO7\")\"BW1TO7\", ";
        //        strQry += "  SUM(\"BETWEEN7TO15\")\"BW7TO15\", SUM(\"BETWEEN15TO30\")\"BW15TO30\", SUM(\"ABOVE30\")\"ABOVE30\", SUM(\"TOTAL\")\"TOTAL\" ";
        //        strQry += "  fROM(SELECT(SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1)  FROM \"VIEW_ALL_OFFICES\" WHERE ";
        //        strQry += "  CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.Circle + ")) \"CIRCLE\", SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.Circle + ")\"CIRCLECODE\", (SELECT ";
        //        strQry += " SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\"   WHERE  ";
        //        strQry += " CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.Division + ")) \"DIVISIONNAME\", SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.Division + ")\"DIVISIONCODE\",";
        //        strQry += "  (SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE  CAST(\"OFF_CODE\" AS TEXT) = ";
        //        strQry += " SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.SubDivision + ")) \"SUBDIVISIONNAME\", SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.SubDivision + ")\"SUBDIVISIONCODE\",";
        //        strQry += "  (SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1)   FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.Section + ")) ";
        //        strQry += " \"SECTIONNAME\", SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.Section + ")\"SECTIONCODE\",   CASE WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")  BETWEEN 0 AND 1 THEN ";
        //        strQry += " COUNT(*) ELSE 0 END \"LESSTHAN1\", CASE   WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")  BETWEEN 2 AND 7 THEN  COUNT(*) ELSE 0 END ";
        //        strQry += " \"BETWEEN1TO7\", CASE   WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")  BETWEEN 8 AND 15 THEN  COUNT(*) ELSE 0 END \"BETWEEN7TO15\",";
        //        strQry += " CASE    WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")  BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0 END \"BETWEEN15TO30\", CASE ";
        //        strQry += " WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\") > 30 THEN  COUNT(\"DF_DTC_CODE\") ELSE 0 END \"ABOVE30\", \"FD_FEEDER_NAME\",";
        //        strQry += " COUNT(*) \"TOTAL\" FROM \"TBLFEEDERMAST\" INNER JOIN \"TBLDTCMAST\" ON  \"DT_FDRSLNO\" = \"FD_FEEDER_CODE\" INNER JOIN \"TBLDTCFAILURE\"";
        //        strQry += "  ON \"DF_DTC_CODE\" = \"DT_CODE\" INNER JOIN   \"TBLWORKORDER\" ON \"DF_ID\" = \"WO_DF_ID\" INNER JOIN  \"TBLINDENT\" ON  ";
        //        strQry += "  \"WO_SLNO\" = \"TI_WO_SLNO\"INNER JOIN \"TBLDTCINVOICE\"  ON \"TI_ID\" = \"IN_TI_NO\" INNER JOIN \"TBLTCREPLACE\" ON  ";
        //        strQry += "  \"IN_NO\" = \"TR_IN_NO\" AND \"DF_REPLACE_FLAG\" <> 1  AND \"DF_STATUS_FLAG\" IN(1, 4) AND SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.SubDivision + ") = '" + SubDivisionCode + "'  ";

        //        if (sFromDate != "" && sTodate != "")
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == "")
        //        {

        //            sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate == string.Empty && sTodate != "")
        //        {
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == string.Empty)
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "'";
        //        }

        //        strQry += "  GROUP BY \"DF_ID\",\"DF_DATE\", \"TR_DECOMM_DATE\", \"DF_LOC_CODE\", \"DF_DTC_CODE\", \"FD_FEEDER_NAME\") A GROUP BY \"CIRCLE\",";
        //        strQry += " \"CIRCLECODE\", \"DIVISIONCODE\", \"DIVISIONNAME\", \"SUBDIVISIONNAME\", \"SUBDIVISIONCODE\", \"SECTIONNAME\", \"SECTIONCODE\" ORDER BY ";
        //        strQry += "  \"CIRCLECODE\",  \"DIVISIONCODE\",\"SUBDIVISIONCODE\" )A INNER JOIN(SELECT \"OM_CODE\" FROM \"TBLOMSECMAST\" WHERE ";
        //        strQry += " \"OM_SUBDIV_CODE\" = '" + SubDivisionCode + "')B ON A.\"SECTIONCODE\" = CAST(B.\"OM_CODE\" AS TEXT))A FULL OUTER JOIN(SELECT(SELECT SUBSTR(\"OFF_NAME\", ";
        //        strQry += "  STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\"   WHERE  CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(CAST(\"OM_CODE\" AS TEXT), 1, " + Constants.Circle + "))\"CIRCLE1\",";
        //        strQry += " SUBSTR(CAST(\"OM_CODE\" AS TEXT), 1, " + Constants.Circle + ")\"CIRCLECODE1\", SUBSTR(CAST(\"OM_CODE\" AS TEXT), 1, " + Constants.Division + ")\"DIVISIONCODE1\", (SELECT   SUBSTR(\"OFF_NAME\", ";
        //        strQry += "  STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\"   WHERE CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(CAST(\"OM_CODE\" AS TEXT), 1, " + Constants.Division + "))  \"DIVISIONNAME1\",";
        //        strQry += "  (SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(CAST(\"OM_CODE\" AS TEXT), 1, " + Constants.SubDivision + "))";
        //        strQry += "  \"SUBDIVISIONNAME1\", SUBSTR(CAST(\"OM_CODE\" AS TEXT), 1, " + Constants.SubDivision + ")\"SUBDIVISIONCODE1\", (SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1)  ";
        //        strQry += " FROM  \"VIEW_ALL_OFFICES\" WHERE \"OFF_CODE\" = \"OM_CODE\") \"SECTIONNAME1\",\"OM_CODE\" as \"SECTIONCODE1\",\"LESSTHAN1DAYNEW\",";
        //        strQry += " \"BW1TO7NEW\",\"BW7TO15NEW\",\"BW15TO30NEW\",\"ABOVE30NEW\",\"TOTALNEW\" FROM (SELECT \"DIVISIONCODE1\", \"DIVISIONNAME1\", ";
        //        strQry += "  \"SUBDIVISIONNAME1\", \"SUBDIVISIONCODE1\", \"SECTIONNAME1\", \"SECTIONCODE1\", \"CIRCLECODE\" AS  \"CIRCLECODE1\", ";
        //        strQry += " \"CIRCLE\" AS \"CIRCLE1\", SUM(\"LESSTHAN1\")\"LESSTHAN1DAYNEW\", SUM(\"BETWEEN1TO7\")\"BW1TO7NEW\", SUM(\"BETWEEN7TO15\")\"BW7TO15NEW\", ";
        //        strQry += "  SUM(\"BETWEEN15TO30\")\"BW15TO30NEW\", SUM(\"ABOVE30\")\"ABOVE30NEW\", SUM(\"TOTAL\")\"TOTALNEW\" FROM(SELECT \"IN_NO\", (SELECT ";
        //        strQry += "  SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\"   WHERE  CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(CAST(\"DF_LOC_CODE\" as  TEXT), 1, " + Constants.Circle + "))";
        //        strQry += "  \"CIRCLE\", SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.Circle + ")\"CIRCLECODE\", (SELECT   SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM ";
        //        strQry += "  \"VIEW_ALL_OFFICES\"   WHERE CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.Division + "))  \"DIVISIONNAME1\",SUBSTR(cast(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.Division + ")";
        //        strQry += "  \"DIVISIONCODE1\",   (SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1) FROM \"VIEW_ALL_OFFICES\" WHERE ";
        //        strQry += "  CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.SubDivision + "))   \"SUBDIVISIONNAME1\", SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.SubDivision + ")\"SUBDIVISIONCODE1\",";
        //        strQry += "  (SELECT SUBSTR(\"OFF_NAME\", STRPOS(\"OFF_NAME\", ':') + 1)   FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT) = SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.Section + "))";
        //        strQry += " \"SECTIONNAME1\", SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.Section + ")  \"SECTIONCODE1\",  CASE WHEN  (\"IN_DATE\" - \"DF_DATE\")  BETWEEN 0 AND 1 THEN ";
        //        strQry += " COUNT(*) ELSE 0 END \"LESSTHAN1\", CASE WHEN(\"IN_DATE\" - \"DF_DATE\")  BETWEEN 2 AND 7 THEN  COUNT(*) ELSE 0 END ";
        //        strQry += "  \"BETWEEN1TO7\", CASE WHEN(\"IN_DATE\" - \"DF_DATE\")  BETWEEN 8 AND 15 THEN  COUNT(*) ELSE 0 END \"BETWEEN7TO15\", ";
        //        strQry += "  CASE WHEN(\"IN_DATE\" - \"DF_DATE\")  BETWEEN 16 AND 30 THEN    COUNT(*) ELSE 0 END \"BETWEEN15TO30\", CASE  ";
        //        strQry += " WHEN(\"IN_DATE\" - \"DF_DATE\") > 30 THEN  COUNT(\"DF_DTC_CODE\") ELSE 0 END \"ABOVE30\", \"FD_FEEDER_NAME\", COUNT(*) \"TOTAL\" ";
        //        strQry += " FROM \"TBLFEEDERMAST\" INNER JOIN \"TBLDTCMAST\" ON  \"DT_FDRSLNO\" = \"FD_FEEDER_CODE\" INNER JOIN   \"TBLDTCFAILURE\"   ";
        //        strQry += " ON \"DF_DTC_CODE\" = \"DT_CODE\" INNER JOIN   \"TBLWORKORDER\" ON \"DF_ID\" = \"WO_DF_ID\" INNER JOIN  \"TBLINDENT\" ON   ";
        //        strQry += " \"WO_SLNO\" = \"TI_WO_SLNO\" INNER JOIN \"TBLDTCINVOICE\"  ON \"TI_ID\" = \"IN_TI_NO\" AND \"DF_REPLACE_FLAG\" <> 1 AND  ";
        //        strQry += "  \"DF_STATUS_FLAG\"  IN(1, 4)  ";

        //        if (sFromDate != "" && sTodate != "")
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == "")
        //        {

        //            sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate == string.Empty && sTodate != "")
        //        {
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == string.Empty)
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "'";
        //        }
        //        strQry += "  GROUP BY \"DF_ID\", \"DF_DATE\", \"IN_NO\", \"IN_DATE\", \"DF_LOC_CODE\", \"DF_DTC_CODE\", \"FD_FEEDER_NAME\")A ";
        //        strQry += "  LEFT JOIN \"TBLTCREPLACE\" ON \"IN_NO\" = \"TR_IN_NO\"  where \"IN_NO\" IS NOT NULL AND \"TR_IN_NO\" IS NULL GROUP BY ";
        //        strQry += "  \"CIRCLE\",\"CIRCLECODE\", \"DIVISIONCODE1\", \"DIVISIONNAME1\", \"SUBDIVISIONNAME1\", \"SUBDIVISIONCODE1\", \"SECTIONNAME1\",";
        //        strQry += "  \"SECTIONCODE1\" ORDER BY \"CIRCLECODE\",\"DIVISIONCODE1\")A INNER JOIN(SELECT \"OM_CODE\" FROM \"TBLOMSECMAST\" WHERE";
        //        strQry += "  \"OM_SUBDIV_CODE\" = '" + SubDivisionCode + "')B ON A.\"SECTIONCODE1\" = CAST(B.\"OM_CODE\" AS TEXT))B on   A.\"CIRCLECODE\" = B.\"CIRCLECODE1\" ";
        //        strQry += "  and A.\"DIVISIONCODE\" = B.\"DIVISIONCODE1\"  and A.\"SUBDIVISIONCODE\" = b.\"SUBDIVISIONCODE1\" AND ";
        //        strQry += "  A.\"SECTIONCODE\" = b.\"SECTIONCODE1\"  ORDER BY  COALESCE(\"SECTIONCODE\", \"SECTIONCODE1\")";
        //        dtDetails = objcon.FetchDataTable(strQry);
        //        return dtDetails;

        //    }


        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return dtDetails;

        //    }

        //}




        //public DataTable LoadCategoryWiseDetails(string SectionCode, string sFromDate, string sTodate)
        //{
        //    DataTable dtDetails = new DataTable();
        //    try
        //    {

        //        //strQry = " SELECT FC_ID1 AS FC_ID, NVL(TO_CHAR(FC_NAME), FC_NAME1)FC_NAME, NVL(TO_CHAR(sectioncode), sectioncode1)sectioncode,";
        //        //strQry += "  NVL(TO_CHAR(LESSTHAN1DAY), 0)LESSTHAN1DAY, NVL(TO_CHAR(BW1TO7), 0)BW1TO7, NVL(TO_CHAR(BW7TO15), 0)BW7TO15,  ";
        //        //strQry += "  NVL(TO_CHAR(BW15TO30), 0)BW15TO30, NVL(TO_CHAR(ABOVE30), 0)ABOVE30, NVL(TO_CHAR(TOTAL), 0)TOTAL,  ";
        //        //strQry += "   NVL(LESSTHAN1DAYNEW, 0)LESSTHAN1DAYNEW,NVL(BW1TO7NEW, 0)BW1TO7NEW, NVL(BW7TO15NEW, 0)BW7TO15NEW,NVL(BW15TO30NEW, 0)";
        //        //strQry += "  BW15TO30NEW, NVL(ABOVE30NEW, 0)ABOVE30NEW, NVL(TOTALNEW, 0)TOTALNEW FROM(SELECT sectioncode, FC_ID, ";
        //        //strQry += "   FC_NAME, SUM(LESSTHAN1)LESSTHAN1DAY, SUM(BETWEEN1TO7)BW1TO7, SUM(BETWEEN7TO15)BW7TO15, ";
        //        //strQry += "   SUM(BETWEEN15TO30)BW15TO30, SUM(ABOVE30)ABOVE30, SUM(TOTAL)TOTAL  fROM(SELECT  DF_DATE, ";
        //        //strQry += "   SUBSTR(DF_LOC_CODE, 0, 4)AS sectioncode, CASE WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 0 AND 1 THEN  ";
        //        //strQry += "   COUNT(*) ELSE 0 END   LESSTHAN1, CASE WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 2 AND 7 THEN  COUNT(*) ";
        //        //strQry += "  ELSE 0 END   BETWEEN1TO7, CASE  WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 8 AND 15 THEN  COUNT(*) ELSE 0 ";
        //        //strQry += "   END BETWEEN7TO15, CASE  WHEN(TR_DECOMM_DATE - DF_DATE)  BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0 END ";
        //        //strQry += "   BETWEEN15TO30, CASE   WHEN(TR_DECOMM_DATE - DF_DATE) > 30 THEN  COUNT(DF_DTC_CODE)  ELSE 0 END ABOVE30, ";
        //        //strQry += "   FD_FEEDER_NAME, FC_NAME, DT_OM_SLNO, COUNT(*) TOTAL, FC_ID  FROM TBLFEEDERMAST INNER JOIN TBLFEEDERCATEGORY ";
        //        //strQry += "   ON FD_FC_ID = FC_ID   INNER JOIN TBLDTCMAST ON DT_FDRSLNO = FD_FEEDER_CODE INNER JOIN TBLDTCFAILURE ";
        //        //strQry += "   ON DF_DTC_CODE = DT_CODE    INNER JOIN   TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN  TBLINDENT  ";
        //        //strQry += "   ON WO_SLNO = TI_WO_SLNO INNER JOIN TBLDTCINVOICE  ON TI_ID = IN_TI_NO INNER JOIN TBLTCREPLACE ON ";
        //        //strQry += "   IN_NO = TR_IN_NO AND DF_REPLACE_FLAG <> 1  AND   DF_STATUS_FLAG IN(1, 4)  GROUP BY DF_ID, DF_DATE,";
        //        //strQry += "   TR_DECOMM_DATE, DF_LOC_CODE, DF_DTC_CODE, FC_NAME, FD_FEEDER_NAME, DT_OM_SLNO, FC_ID) A WHERE  ";
        //        //strQry += "   DT_OM_SLNO = '" + SectionCode + "'  ";
        //        //if (sFromDate != "" && sTodate != "")
        //        //{
        //        //    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sFromDate = DFromDate.ToString("yyyyMMdd");
        //        //    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sTodate = DToDate.ToString("yyyyMMdd");
        //        //    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        //}
        //        //else if (sFromDate != "" && sTodate == "")
        //        //{

        //        //    sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //        //    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sFromDate = DFromDate.ToString("yyyyMMdd");
        //        //    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sTodate = DToDate.ToString("yyyyMMdd");
        //        //    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        //}
        //        //else if (sFromDate == string.Empty && sTodate != "")
        //        //{
        //        //    strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        //}

        //        //strQry += "   GROUP BY FC_NAME, sectioncode, FC_ID)A full outer join(SELECT FC_ID AS FC_ID1, ";
        //        //strQry += "   TO_CHAR(FC_NAME)FC_NAME1, TO_CHAR(sectioncode)sectioncode1, TO_CHAR(LESSTHAN1)LESSTHAN1DAYNEW, ";
        //        //strQry += "   TO_CHAR(BETWEEN1TO7)BW1TO7NEW, TO_CHAR(BETWEEN7TO15)BW7TO15NEW, TO_CHAR(BETWEEN15TO30)BW15TO30NEW, ";
        //        //strQry += "   TO_CHAR(ABOVE30)ABOVE30NEW, TO_CHAR(TOTAL)TOTALNEW FROM(select FC_ID, FC_NAME, sectioncode, sum(LESSTHAN1)LESSTHAN1,";
        //        //strQry += "   sum(BETWEEN1TO7)BETWEEN1TO7, sum(BETWEEN7TO15)BETWEEN7TO15, sum(BETWEEN15TO30)BETWEEN15TO30, ";
        //        //strQry += "   sum(ABOVE30)ABOVE30, sum(TOTAL) TOTAL  from(SELECT FC_ID, FC_NAME, DF_LOC_CODE AS sectioncode, ";
        //        //strQry += "   CASE WHEN(IN_DATE - DF_DATE)   BETWEEN 0 AND 1 THEN  COUNT(*) ELSE 0 END   LESSTHAN1, CASE WHEN(IN_DATE - DF_DATE)";
        //        //strQry += "   BETWEEN 2 AND 7 THEN   COUNT(*) ELSE 0  END BETWEEN1TO7, CASE WHEN(IN_DATE - DF_DATE)  BETWEEN 8 AND 15 THEN  ";
        //        //strQry += "   COUNT(*) ELSE 0 END  BETWEEN7TO15, CASE WHEN(IN_DATE - DF_DATE)  BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0 END ";
        //        //strQry += "    BETWEEN15TO30, CASE  WHEN(IN_DATE - DF_DATE) > 30 THEN  COUNT(DF_DTC_CODE)  ELSE 0 END ABOVE30, COUNT(*) TOTAL ";
        //        //strQry += "   FROM TBLDTCFAILURE INNER JOIN   TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN  TBLINDENT ON WO_SLNO = TI_WO_SLNO ";
        //        //strQry += "  INNER JOIN   TBLDTCINVOICE  ON TI_ID = IN_TI_NO INNER JOIN TBLFEEDERMAST ON  SUBSTR(df_dtc_code, 0, 4) = FD_FEEDER_CODE";
        //        //strQry += "  INNER JOIN TBLFEEDERCATEGORY on fc_id = FD_FC_ID  AND IN_NO NOT IN(SELECT TR_IN_NO FROM TBLTCREPLACE) AND ";
        //        //strQry += "  DF_LOC_CODE = '" + SectionCode + "'  ";
        //        //if (sFromDate != "" && sTodate != "")
        //        //{
        //        //    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sFromDate = DFromDate.ToString("yyyyMMdd");
        //        //    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sTodate = DToDate.ToString("yyyyMMdd");
        //        //    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        //}
        //        //else if (sFromDate != "" && sTodate == "")
        //        //{

        //        //    sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //        //    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sFromDate = DFromDate.ToString("yyyyMMdd");
        //        //    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //        //    sTodate = DToDate.ToString("yyyyMMdd");
        //        //    strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        //}
        //        //else if (sFromDate == string.Empty && sTodate != "")
        //        //{
        //        //    strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        //}

        //        //strQry += " GROUP BY IN_DATE, DF_DATE, FC_NAME, DF_LOC_CODE, FC_ID) GROUP by FC_NAME, sectioncode, FC_ID))B ";
        //        //strQry += "  ON A.sectioncode = B.sectioncode1 ";

        //        strQry = " SELECT COALESCE(\"SECTIONCODE\", \"SECTIONCODE1\")\"SECTIONCODE\", COALESCE(\"FC_ID\",\"FC_ID1\")\"FC_ID\", COALESCE(\"FC_NAME\", \"FC_NAME1\")\"FC_NAME\", COALESCE(\"LESSTHAN1DAY\", '0')";
        //        strQry += " \"LESSTHAN1DAY\", COALESCE(\"BW1TO7\", '0')\"BW1TO7\", COALESCE(\"BW7TO15\", '0')\"BW7TO15\", COALESCE(\"BW15TO30\",'0')\"BW15TO30\", COALESCE(\"ABOVE30\", '0')\"ABOVE30\", COALESCE(\"TOTAL\", '0')";
        //        strQry += " \"TOTAL\", COALESCE(\"LESSTHAN1DAYNEW\", '0')\"LESSTHAN1DAYNEW\", COALESCE(\"BW1TO7NEW\", '0')\"BW1TO7NEW\", COALESCE(\"BW7TO15NEW\", '0')\"BW7TO15NEW\", COALESCE(\"BW15TO30NEW\", '0')";
        //        strQry += " \"BW15TO30NEW\", COALESCE(\"ABOVE30NEW\", '0')\"ABOVE30NEW\", COALESCE(\"TOTALNEW\", '0')\"TOTALNEW\" from(SELECT \"SECTIONCODE\", \"FC_ID\", \"FC_NAME\", SUM(\"LESSTHAN1\")";
        //        strQry += " \"LESSTHAN1DAY\", SUM(\"BETWEEN1TO7\")\"BW1TO7\", SUM(\"BETWEEN7TO15\")\"BW7TO15\", SUM(\"BETWEEN15TO30\")\"BW15TO30\", SUM(\"ABOVE30\")\"ABOVE30\", ";
        //        strQry += " SUM(\"TOTAL\")\"TOTAL\"  fROM(SELECT  \"DF_DATE\", SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1, " + Constants.Section + ")AS \"SECTIONCODE\", CASE WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\") ";
        //        strQry += " BETWEEN 0 AND 1 THEN     COUNT(*) ELSE 0 END   \"LESSTHAN1\", CASE WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")  BETWEEN 2 AND 7 THEN ";
        //        strQry += " COUNT(*)   ELSE 0 END   \"BETWEEN1TO7\", CASE  WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")  BETWEEN 8 AND 15 THEN  COUNT(*) ELSE 0   ";
        //        strQry += " END \"BETWEEN7TO15\", CASE  WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\")  BETWEEN 16 AND 30 THEN  COUNT(*) ELSE 0 END   \"BETWEEN15TO30\",";
        //        strQry += " CASE   WHEN(\"TR_DECOMM_DATE\" - \"DF_DATE\") > 30 THEN  COUNT(\"DF_DTC_CODE\")  ELSE 0 END \"ABOVE30\", \"FD_FEEDER_NAME\", \"FC_NAME\",";
        //        strQry += "  \"DT_OM_SLNO\", COUNT(*) \"TOTAL\", \"FC_ID\"  FROM \"TBLFEEDERMAST\" INNER JOIN \"TBLFEEDERCATEGORY\"    ON \"FD_FC_ID\" = \"FC_ID\"   INNER JOIN ";
        //        strQry += " \"TBLDTCMAST\" ON \"DT_FDRSLNO\" =\"FD_FEEDER_CODE\" INNER JOIN \"TBLDTCFAILURE\"    ON \"DF_DTC_CODE\" = \"DT_CODE\"    INNER JOIN   ";
        //        strQry += "  \"TBLWORKORDER\" ON \"DF_ID\" = \"WO_DF_ID\" INNER JOIN  \"TBLINDENT\"     ON \"WO_SLNO\" = \"TI_WO_SLNO\" INNER JOIN \"TBLDTCINVOICE\" ";
        //        strQry += " ON \"TI_ID\" = \"IN_TI_NO\" INNER JOIN \"TBLTCREPLACE\" ON    \"IN_NO\" = \"TR_IN_NO\" AND \"DF_REPLACE_FLAG\" <> 1  AND   \"DF_STATUS_FLAG\" IN(1, 4) ";
        //        if (sFromDate != "" && sTodate != "")
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == "")
        //        {

        //            sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate == string.Empty && sTodate != "")
        //        {
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == string.Empty)
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "'";
        //        }
        //        strQry += " GROUP BY \"DF_ID\", \"DF_DATE\", \"TR_DECOMM_DATE\", \"DF_LOC_CODE\", \"DF_DTC_CODE\", \"FC_NAME\", \"FD_FEEDER_NAME\",\"DT_OM_SLNO\", \"FC_ID\") A WHERE  ";
        //        strQry += "  \"DT_OM_SLNO\" = '" + SectionCode + "'     GROUP BY \"FC_NAME\", \"SECTIONCODE\", \"FC_ID\")a full outer join ";
        //        strQry += " (SELECT \"FC_ID\" AS \"FC_ID1\", CAST(\"FC_NAME\" AS TEXT)\"FC_NAME1\", CAST(\"SECTIONCODE\"  AS TEXT)\"SECTIONCODE1\", CAST(\"LESSTHAN1\"  AS TEXT)\"LESSTHAN1DAYNEW\", ";
        //        strQry += "  CAST(\"BETWEEN1TO7\"  AS TEXT)\"BW1TO7NEW\", CAST(\"BETWEEN7TO15\"  AS TEXT)\"BW7TO15NEW\", CAST(\"BETWEEN15TO30\"  AS TEXT)\"BW15TO30NEW\", CAST(\"ABOVE30\"  AS TEXT)\"ABOVE30NEW\", ";
        //        strQry += "  CAST(\"TOTAL\"  AS TEXT)\"TOTALNEW\" FROM(SELECT \"FC_ID\", \"FC_NAME\",\"SECTIONCODE\",sum(\"LESSTHAN1\")\"LESSTHAN1\",sum(\"BETWEEN1TO7\")\"BETWEEN1TO7\",";
        //        strQry += " sum(\"BETWEEN7TO15\")\"BETWEEN7TO15\",sum(\"BETWEEN15TO30\")\"BETWEEN15TO30\",sum(\"ABOVE30\")\"ABOVE30\",sum(\"TOTAL\")\"TOTAL\" FROM (SELECT \"FC_ID\",";
        //        strQry += " \"FC_NAME\", \"DF_LOC_CODE\" AS \"SECTIONCODE\", CASE WHEN(\"IN_DATE\" - \"DF_DATE\") BETWEEN 0 AND 1 THEN  COUNT(*) ELSE 0 END  \"LESSTHAN1\",  ";
        //        strQry += " CASE WHEN(\"IN_DATE\" -\"DF_DATE\")   BETWEEN 2 AND 7 THEN   COUNT(*) ELSE 0  END \"BETWEEN1TO7\", CASE WHEN(\"IN_DATE\" - \"DF_DATE\")   BETWEEN 8 AND 15 THEN  ";
        //        strQry += " COUNT(*) ELSE 0 END  \"BETWEEN7TO15\", CASE WHEN(\"IN_DATE\" - \"DF_DATE\")  BETWEEN 16 AND 30 THEN  COUNT(*)  ELSE 0 END \"BETWEEN15TO30\", CASE ";
        //        strQry += " WHEN(\"IN_DATE\" - \"DF_DATE\") > 30 THEN  COUNT(\"DF_DTC_CODE\")  ELSE 0 END \"ABOVE30\", COUNT(*) \"TOTAL\"   FROM \"TBLDTCFAILURE\" INNER JOIN ";
        //        strQry += " \"TBLWORKORDER\" ON \"DF_ID\" = \"WO_DF_ID\" INNER JOIN  \"TBLINDENT\" ON \"WO_SLNO\" = \"TI_WO_SLNO\"   INNER JOIN   \"TBLDTCINVOICE\"  ON \"TI_ID\" = \"IN_TI_NO\" ";
        //        strQry += " INNER JOIN \"TBLFEEDERMAST\" ON  SUBSTR(CAST(\"DF_DTC_CODE\" AS TEXT), 1, " + Constants.Feeder + ") = \"FD_FEEDER_CODE\"  INNER JOIN   \"TBLFEEDERCATEGORY\" ";
        //        strQry += " on \"FC_ID\" = \"FD_FC_ID\"  AND \"IN_NO\" NOT IN(SELECT \"TR_IN_NO\" FROM \"TBLTCREPLACE\") AND   \"DF_LOC_CODE\" = '" + SectionCode + "'  ";

        //        if (sFromDate != "" && sTodate != "")
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == "")
        //        {

        //            sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate == string.Empty && sTodate != "")
        //        {
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_Todate = DToDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == string.Empty)
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
        //            strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "'";
        //        }
        //        strQry += "  GROUP BY \"IN_DATE\", \"DF_DATE\",\"FC_NAME\", \"DF_LOC_CODE\", \"FC_ID\")A  GROUP BY \"FC_ID\", \"FC_NAME\",\"SECTIONCODE\")A)b on a.\"FC_ID\" = b.\"FC_ID1\" and ";
        //        strQry += "  \"SECTIONCODE\" = \"SECTIONCODE1\" and \"FC_NAME\" = \"FC_NAME1\" ";
        //        dtDetails = objcon.FetchDataTable(strQry);
        //        return dtDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return dtDetails;

        //    }

        //}




        //public DataTable LoadFeederDetails(string CategoryId, string SectionCode, string sFromDate, string sTodate)
        //{
        //    DataTable dtDetails = new DataTable();
        //    try
        //    {

        //        strQry = " (SELECT TO_CHAR(sectioncode)sectioncode, TO_CHAR(FC_ID)FC_ID,TO_CHAR(FD_FEEDER_NAME)FD_FEEDER_NAME, ";
        //        strQry += " TO_CHAR(TC_CODE)TC_CODE,TO_CHAR(tc_slno)TC_SLNO,TO_CHAR(WO_NO) AS COMISSIONWONO, TO_CHAR(WO_NO_DECOM)AS ";
        //        strQry += " DECOMISSIONWONO, TO_CHAR(DF_DATE, 'DD-MON-YYYY')AS FAILUREDATE, STATUS, TO_CHAR(DT_CODE)DT_CODE ";
        //        strQry += " fROM(SELECT DT_CODE, DT_TC_ID, WO_NO_DECOM, WO_NO, TC_CODE, TC_SLNO, DF_DATE, SUBSTR(DF_LOC_CODE, 0, 4)AS sectioncode,";
        //        strQry += " FD_FEEDER_NAME, FC_NAME, DT_OM_SLNO, FC_ID, 'COMPLETED' AS STATUS  FROM TBLFEEDERMAST INNER JOIN TBLFEEDERCATEGORY ";
        //        strQry += " ON FD_FC_ID = FC_ID   INNER JOIN  TBLDTCMAST ON DT_FDRSLNO = FD_FEEDER_CODE INNER JOIN TBLDTCFAILURE ON DF_DTC_CODE = DT_CODE ";
        //        strQry += " INNER JOIN   TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN  TBLINDENT ON WO_SLNO = TI_WO_SLNO INNER JOIN ";
        //        strQry += " TBLDTCINVOICE  ON TI_ID = IN_TI_NO INNER JOIN TBLTCREPLACE ON IN_NO = TR_IN_NO  INNER JOIN TBLTCMASTER ON   ";
        //        strQry += " DF_EQUIPMENT_ID = TC_CODE  AND DF_REPLACE_FLAG <> 1  AND   DF_STATUS_FLAG IN(1, 4)    GROUP BY DF_ID, DF_DATE, ";
        //        strQry += " TR_DECOMM_DATE, DF_LOC_CODE, DF_DTC_CODE, FC_NAME, FD_FEEDER_NAME, DT_OM_SLNO, FC_ID, dt_code, TC_CODE,";
        //        strQry += " TC_SLNO, WO_NO, WO_NO_DECOM, TR_DECOMM_DATE, DT_TC_ID, DT_CODE) A WHERE DT_OM_SLNO = '" + SectionCode + "'   AND FC_ID = '" + CategoryId + "'";
        //        if (sFromDate != "" && sTodate != "")
        //        {
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            sFromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            sTodate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == "")
        //        {

        //            sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //            DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            sFromDate = DFromDate.ToString("yyyyMMdd");
        //            DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            sTodate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        else if (sFromDate == string.Empty && sTodate != "")
        //        {
        //            strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }

        //        strQry += "  GROUP BY  sectioncode, FC_ID, FD_FEEDER_NAME, TC_CODE, tc_slno, DF_DATE, WO_NO, WO_NO_DECOM, STATUS, TC_CODE, DT_CODE) ";
        //        strQry += " UNION ALL   (SELECT TO_CHAR(DF_LOC_CODE)sectioncode, TO_CHAR(FC_ID)FC_ID, TO_CHAR(FD_FEEDER_NAME)FD_FEEDER_NAME, ";
        //        strQry += "  TO_CHAR(TC_CODE)TC_CODE, TO_CHAR(tc_slno)TC_SLNO, TO_CHAR(WO_NO) AS COMISSIONWONO, TO_CHAR(WO_NO_DECOM)  ";
        //        strQry += " AS DECOMISSIONWONO, TO_CHAR(DF_DATE, 'DD-MON-YYYY')AS FAILUREDATE, 'PENDING' AS STATUS, DF_DTC_CODE AS DT_CODE ";
        //        strQry += " FROM TBLDTCFAILURE INNER JOIN    TBLWORKORDER ON DF_ID = WO_DF_ID INNER JOIN  TBLINDENT ON WO_SLNO = TI_WO_SLNO ";
        //        strQry += "  INNER JOIN TBLDTCINVOICE   ON TI_ID = IN_TI_NO INNER JOIN TBLFEEDERMAST ON  SUBSTR(df_dtc_code, 0, 4) = FD_FEEDER_CODE INNER JOIN ";
        //        strQry += "  TBLFEEDERCATEGORY on fc_id = FD_FC_ID INNER JOIN TBLTCMASTER ON  DF_EQUIPMENT_ID = TC_CODE   INNER JOIN TBLDTCMAST ";
        //        strQry += "  ON DT_FDRSLNO = FD_FEEDER_CODE AND IN_NO NOT IN(SELECT TR_IN_NO FROM TBLTCREPLACE)   AND DF_LOC_CODE = '" + SectionCode + "'  AND FC_ID = '" + CategoryId + "' ";
        //        if (sFromDate != "" && sTodate != "")
        //        {
        //            //DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            //sFromDate = DFromDate.ToString("yyyyMMdd");
        //            //DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            //sTodate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        else if (sFromDate != "" && sTodate == "")
        //        {

        //            //sTodate = DateTime.Now.ToString("dd/MM/yyyy");
        //            //DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            //sFromDate = DFromDate.ToString("yyyyMMdd");
        //            //DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            //sTodate = DToDate.ToString("yyyyMMdd");
        //            strQry += "  AND TO_CHAR(DF_DATE,'YYYYMMDD')>='" + sFromDate + "' AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        else if (sFromDate == string.Empty && sTodate != "")
        //        {
        //            strQry += " AND TO_CHAR(DF_DATE,'YYYYMMDD')<='" + sTodate + "'";
        //        }
        //        strQry += " GROUP BY IN_DATE, DF_DATE, DF_LOC_CODE, FD_FEEDER_NAME, FC_ID, TC_CODE, TC_SLNO, WO_NO_DECOM, WO_NO, DF_DTC_CODE)";

        //        dtDetails = objcon.FetchDataTable(strQry);
        //        return dtDetails;

        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadFeederDetails");
        //        return dtDetails;

        //    }

        //}
        #endregion

        public DataTable LoadFeederDetails(string CategoryId, string SectionCode, string sFromDate, string sTodate)
        {
            DataTable dtDetails = new DataTable();
            try
            {
                strQry = " SELECT* FROM (SELECT UPPER(\"MD_NAME\")\"MD_NAME\",\"SECTIONCODE\", CAST(\"FC_ID\" AS TEXT)\"FC_ID\",CAST(\"FD_FEEDER_NAME\" AS TEXT)\"FD_FEEDER_NAME\", ";
                strQry += " CAST(\"TC_CODE\" AS TEXT)\"TC_CODE\",CAST(\"TC_SLNO\" AS TEXT)\"TC_SLNO\",CAST(\"WO_NO\" AS TEXT) AS\"COMISSIONWONO\", CAST(\"WO_NO_DECOM\" AS TEXT)AS ";
                strQry += " \"DECOMISSIONWONO\", TO_CHAR(\"DF_DATE\", 'DD-MON-YYYY')AS\"FAILUREDATE\", \"STATUS\", CAST(\"DT_CODE\" AS TEXT)\"DT_CODE\" ";
                strQry += " FROM (SELECT \"MD_NAME\",\"DT_CODE\", \"DT_TC_ID\",\"WO_NO_DECOM\",\"WO_NO\", \"TC_CODE\", \"TC_SLNO\", \"DF_DATE\", SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT), 1," + Constants.Section + ")AS \"SECTIONCODE\",";
                strQry += " \"FD_FEEDER_NAME\",\"FC_NAME\", \"DT_OM_SLNO\", \"FC_ID\", 'COMPLETED' AS \"STATUS\"  FROM \"TBLFEEDERMAST\" INNER JOIN \"TBLFEEDERCATEGORY\" ";
                strQry += " ON \"FD_FC_ID\" = \"FC_ID\"   INNER JOIN  \"TBLDTCMAST\" ON \"DT_FDRSLNO\" = \"FD_FEEDER_CODE\" INNER JOIN \"TBLDTCFAILURE\" ON \"DF_DTC_CODE\" = \"DT_CODE\" ";
                strQry += " INNER JOIN   \"TBLWORKORDER\" ON \"DF_ID\" = \"WO_DF_ID\" INNER JOIN  \"TBLINDENT\" ON \"WO_SLNO\" = \"TI_WO_SLNO\" INNER JOIN ";
                strQry += "  \"TBLDTCINVOICE\"  ON \"TI_ID\" = \"IN_TI_NO\" INNER JOIN \"TBLTCREPLACE\" ON \"IN_NO\" = \"TR_IN_NO\"  INNER JOIN \"TBLTCMASTER\" ON  ";
                strQry += " \"DF_EQUIPMENT_ID\" = \"TC_CODE\" INNER JOIN \"TBLMASTERDATA\" ON CAST(\"MD_ID\" AS TEXT)= \"DF_ALTERNATE_RPMT\" AND \"MD_TYPE\"='ARM' AND \"DF_REPLACE_FLAG\" <> 1  AND   \"DF_STATUS_FLAG\" IN(1, 4)    GROUP BY \"MD_NAME\",\"DF_ID\",\"DF_DATE\", ";
                strQry += " \"TR_DECOMM_DATE\", \"DF_LOC_CODE\",\"DF_DTC_CODE\", \"FC_NAME\", \"FD_FEEDER_NAME\", \"DT_OM_SLNO\", \"FC_ID\",\"DT_CODE\", \"TC_CODE\",";
                strQry += " \"TC_SLNO\", \"WO_NO\", \"WO_NO_DECOM\", \"TR_DECOMM_DATE\", \"DT_TC_ID\", \"DT_CODE\") A WHERE \"DT_OM_SLNO\" = '" + SectionCode + "'   AND \"FC_ID\" = '" + CategoryId + "'";
                if (sFromDate != "" && sTodate != "")
                {
                    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
                    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string sFinal_Todate = DToDate.ToString("yyyyMMdd");
                    strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
                }
                else if (sFromDate != "" && sTodate == "")
                {

                    sTodate = DateTime.Now.ToString("dd/MM/yyyy");
                    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
                    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string sFinal_Todate = DToDate.ToString("yyyyMMdd");
                    strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
                }
                else if (sFromDate == string.Empty && sTodate != "")
                {
                    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string sFinal_Todate = DToDate.ToString("yyyyMMdd");
                    strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
                }
                else if (sFromDate != "" && sTodate == string.Empty)
                {
                    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
                    strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "'";
                }

                strQry += "  GROUP BY  \"MD_NAME\",\"SECTIONCODE\", \"FC_ID\", \"FD_FEEDER_NAME\", \"TC_CODE\", \"TC_SLNO\", \"DF_DATE\", \"WO_NO\", \"WO_NO_DECOM\", \"STATUS\", \"TC_CODE\",\"DT_CODE\")A ";
                strQry += " UNION ALL (SELECT \"MD_NAME\",CAST(\"DF_LOC_CODE\" AS TEXT)\"SECTIONCODE\", CAST(\"FC_ID\" AS TEXT)\"FC_ID\", CAST(\"FD_FEEDER_NAME\" AS TEXT)\"FD_FEEDER_NAME\", ";
                strQry += "   CAST(\"TC_CODE\" AS TEXT)\"TC_CODE\", CAST(\"TC_SLNO\" AS TEXT)\"TC_SLNO\", CAST(\"WO_NO\" AS TEXT) AS \"COMISSIONWONO\", CAST(\"WO_NO_DECOM\" AS TEXT) ";
                strQry += "  AS \"DECOMISSIONWONO\", TO_CHAR(\"DF_DATE\", 'DD-MON-YYYY')AS \"FAILUREDATE\", 'PENDING' AS \"STATUS\", \"DF_DTC_CODE\" AS \"DT_CODE\" ";
                strQry += " FROM \"TBLDTCFAILURE\" INNER JOIN    \"TBLWORKORDER\" ON \"DF_ID\" = \"WO_DF_ID\" INNER JOIN  \"TBLINDENT\" ON \"WO_SLNO\" = \"TI_WO_SLNO\" ";
                strQry += "  INNER JOIN \"TBLDTCINVOICE\"   ON \"TI_ID\" = \"IN_TI_NO\" INNER JOIN \"TBLFEEDERMAST\" ON  SUBSTR(CAST(\"DF_DTC_CODE\" AS TEXT), 1, " + Constants.Feeder + ") = \"FD_FEEDER_CODE\" INNER JOIN ";
                strQry += "  \"TBLFEEDERCATEGORY\" ON \"FC_ID\" = \"FD_FC_ID\" INNER JOIN \"TBLTCMASTER\" ON  \"DF_EQUIPMENT_ID\" = \"TC_CODE\"   INNER JOIN \"TBLDTCMAST\" ";
                strQry += "   ON \"DT_FDRSLNO\" = \"FD_FEEDER_CODE\" INNER JOIN \"TBLMASTERDATA\" ON CAST(\"MD_ID\" AS TEXT)=\"DF_ALTERNATE_RPMT\" AND \"MD_TYPE\"='ARM' AND ";
                strQry += " \"IN_NO\" NOT IN(SELECT \"TR_IN_NO\" FROM \"TBLTCREPLACE\")   AND \"DF_LOC_CODE\" = '" + SectionCode + "'  AND \"FC_ID\" = '" + CategoryId + "' ";
                if (sFromDate != "" && sTodate != "")
                {
                    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
                    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string sFinal_Todate = DToDate.ToString("yyyyMMdd");
                    strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
                }
                else if (sFromDate != "" && sTodate == "")
                {

                    sTodate = DateTime.Now.ToString("dd/MM/yyyy");
                    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
                    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string sFinal_Todate = DToDate.ToString("yyyyMMdd");
                    strQry += "  AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "' AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
                }
                else if (sFromDate == string.Empty && sTodate != "")
                {
                    DateTime DToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string sFinal_Todate = DToDate.ToString("yyyyMMdd");
                    strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')<='" + sFinal_Todate + "'";
                }
                else if (sFromDate != "" && sTodate == string.Empty)
                {
                    DateTime DFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string sFinal_FromDate = DFromDate.ToString("yyyyMMdd");
                    strQry += " AND TO_CHAR(\"DF_DATE\",'YYYYMMDD')>='" + sFinal_FromDate + "'";
                }
                strQry += " GROUP BY \"MD_NAME\",\"IN_DATE\", \"DF_DATE\", \"DF_LOC_CODE\", \"FD_FEEDER_NAME\", \"FC_ID\", \"TC_CODE\", \"TC_SLNO\", \"WO_NO_DECOM\", \"WO_NO\", \"DF_DTC_CODE\")";

                dtDetails = objcon.FetchDataTable(strQry);
                return dtDetails;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtDetails;

            }

        }

    }
}
