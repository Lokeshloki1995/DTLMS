using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using IIITS.PGSQL.DAL;
using Npgsql;

namespace IIITS.DTLMS.BL
{
    public class clsInterDashboard
    {
        string strFormCode = "clsInterDashboard";
      //  CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);

        //public string sUserId { get; set; }
        public string sFromDate { get; set; }
        public string sToDate { get; set; }
        public bool bCurrentDate { get; set; }
        public string sOfficeCode { get; set; }
        public string sFeederCode { get; set; }
        public string sVendorId { get; set; }

        int Circle_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Circle_code"]);
        int Division_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Division_code"]);
        int SubDiv_code = Convert.ToInt32(ConfigurationSettings.AppSettings["SubDiv_code"]);
        int Section_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Section_code"]);
        public string GetToatlEnumerationCount(string sUserId = "", bool bDate = false, string sFromDate = "", string sTodate = "",
            string sOfficeCode = "", string sFeederCode = "")
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            try
            {
                string strQry = string.Empty;
                //strQry = "SELECT  NVL(COUNT(ED_ENUM_TYPE),0) FROM TBLENUMERATIONDETAILS WHERE ED_ID IS NOT NULL AND ED_STATUS_FLAG<>'5'";
                //if (sUserId != "")
                //{
                //    strQry += " AND (ED_OPERATOR1='" + sUserId + "' OR ED_OPERATOR2='" + sUserId + "')";
                //}

                //if (bDate == true)
                //{
                //    strQry += " AND TO_CHAR(ED_CRON,'DD/MM/YYYY')=TO_CHAR(SYSDATE,'DD/MM/YYYY')";
                //}
                //if (sFromDate != "" && sTodate!="")
                //{
                //    DateTime dFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //    DateTime dToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //    strQry += " AND TO_CHAR(ED_CRON,'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //}
                //if (sOfficeCode != "")
                //{
                //    strQry += " AND ED_OFFICECODE LIKE '"+ sOfficeCode +"%'";
                //}
                //if (sFeederCode != "")
                //{
                //    strQry += " AND ED_FEEDERCODE LIKE '" + sFeederCode + "%'";
                //}


                strQry = "SELECT  COALESCE(COUNT(\"ED_ENUM_TYPE\"),0) FROM \"TBLENUMERATIONDETAILS\" WHERE \"ED_ID\" IS NOT NULL AND \"ED_STATUS_FLAG\"<>'5'";
                if (sUserId != "")
                {
                    strQry += " AND (\"ED_OPERATOR1\"='" + sUserId + "' OR \"ED_OPERATOR2\"='" + sUserId + "')";
                }

                if (bDate == true)
                {
                    strQry += " AND TO_CHAR(\"ED_CRON\",'DD/MM/YYYY')=TO_CHAR(now(),'DD/MM/YYYY')";
                }
                if (sFromDate != "" && sTodate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    DateTime dToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                }
                if (sOfficeCode != "")
                {
                    strQry += " AND cast(\"ED_OFFICECODE\" as text) LIKE '" + sOfficeCode + "%'";
                }
                if (sFeederCode != "")
                {
                    strQry += " AND cast(\"ED_FEEDERCODE\" as text) LIKE '" + sFeederCode + "%'";
                }

                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetToatlEnumerationCount");
                return ex.Message;
            }
        }

        public string GetQCPendingEnumCount(string sUserId = "", bool bDate = false, string sFromDate = "", string sTodate = "",
            string sOfficeCode = "", string sFeederCode = "")
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            try
            {
                string strQry = string.Empty;
                //strQry = "SELECT  NVL(COUNT(ED_ENUM_TYPE),0) FROM TBLENUMERATIONDETAILS WHERE ED_STATUS_FLAG='0' ";
                //if (sUserId != "")
                //{
                //    strQry += " AND (ED_OPERATOR1='" + sUserId + "' OR ED_OPERATOR2='" + sUserId + "') ";

                //}
                //if (bDate == true)
                //{
                //    strQry += " AND TO_CHAR(ED_CRON,'DD/MM/YYYY')=TO_CHAR(SYSDATE,'DD/MM/YYYY')";
                //}
                //if (sFromDate != "" && sTodate != "")
                //{
                //    DateTime dFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //    DateTime dToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //    strQry += " AND TO_CHAR(ED_CRON,'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //}
                //if (sOfficeCode != "")
                //{
                //    strQry += " AND ED_OFFICECODE LIKE '" + sOfficeCode + "%'";
                //}
                //if (sFeederCode != "")
                //{
                //    strQry += " AND ED_FEEDERCODE LIKE '" + sFeederCode + "%'";
                //}


                strQry = "SELECT  COALESCE(COUNT(\"ED_ENUM_TYPE\"),0) FROM \"TBLENUMERATIONDETAILS\" WHERE \"ED_STATUS_FLAG\"='0' ";
                if (sUserId != "")
                {
                    strQry += " AND (\"ED_OPERATOR1\"='" + sUserId + "' OR \"ED_OPERATOR2\"='" + sUserId + "') ";

                }
                if (bDate == true)
                {
                    strQry += " AND TO_CHAR(\"ED_CRON\",'DD/MM/YYYY')=TO_CHAR(now(),'DD/MM/YYYY')";
                }
                if (sFromDate != "" && sTodate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    DateTime dToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                }
                if (sOfficeCode != "")
                {
                    strQry += " AND CAST(\"ED_OFFICECODE\" AS TEXT) LIKE '" + sOfficeCode + "%'";
                }
                if (sFeederCode != "")
                {
                    strQry += " AND CAST(\"ED_FEEDERCODE\" AS TEXT) LIKE '" + sFeederCode + "%'";
                }
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetToatlEnumerationCount");
                return ex.Message;
            }
        }

        public string GetQCDoneEnumCount(string sUserId = "", bool bDate = false, string sFromDate = "", string sTodate = "",
             string sOfficeCode = "", string sFeederCode = "")
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            try
            {
                string strQry = string.Empty;
                //strQry = "SELECT  NVL(COUNT(ED_ENUM_TYPE),0) FROM TBLENUMERATIONDETAILS WHERE ED_STATUS_FLAG='1' ";
                //if (sUserId != "")
                //{
                //    strQry += " AND (ED_OPERATOR1='" + sUserId + "' OR ED_OPERATOR2='" + sUserId + "') ";

                //}
                //if (bDate == true)
                //{
                //    strQry += " AND TO_CHAR(ED_CRON,'DD/MM/YYYY')=TO_CHAR(SYSDATE,'DD/MM/YYYY')";
                //}
                //if (sFromDate != "" && sTodate != "")
                //{
                //    DateTime dFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //    DateTime dToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //    strQry += " AND TO_CHAR(ED_CRON,'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //}
                //if (sOfficeCode != "")
                //{
                //    strQry += " AND ED_OFFICECODE LIKE '" + sOfficeCode + "%'";
                //}
                //if (sFeederCode != "")
                //{
                //    strQry += " AND ED_FEEDERCODE LIKE '" + sFeederCode + "%'";
                //}

                strQry = "SELECT  COALESCE(COUNT(\"ED_ENUM_TYPE\"),0) FROM \"TBLENUMERATIONDETAILS\" WHERE \"ED_STATUS_FLAG\"='1' ";
                if (sUserId != "")
                {
                    strQry += " AND (\"ED_OPERATOR1\"='" + sUserId + "' OR \"ED_OPERATOR2\"='" + sUserId + "') ";

                }
                if (bDate == true)
                {
                    strQry += " AND TO_CHAR(\"ED_CRON\",'DD/MM/YYYY')=TO_CHAR(NOW(),'DD/MM/YYYY')";
                }
                if (sFromDate != "" && sTodate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    DateTime dToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                }
                if (sOfficeCode != "")
                {
                    strQry += " AND CAST(\"ED_OFFICECODE\" AS TEXT) LIKE '" + sOfficeCode + "%'";
                }
                if (sFeederCode != "")
                {
                    strQry += " AND CAST(\"ED_FEEDERCODE\" AS TEXT) LIKE '" + sFeederCode + "%'";
                }

                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetToatlEnumerationCount");
                return ex.Message;
            }
        }

        public string GetRejectEnumCount(string sUserId = "", bool bDate = false, string sFromDate = "", string sTodate = "",
            string sOfficeCode = "", string sFeederCode = "")
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            try
            {
                string strQry = string.Empty;
                //strQry = "SELECT  NVL(COUNT(ED_ENUM_TYPE),0) FROM TBLENUMERATIONDETAILS WHERE ED_STATUS_FLAG='3' ";
                //if (sUserId != "")
                //{
                //    strQry += " AND (ED_OPERATOR1='" + sUserId + "' OR ED_OPERATOR2='" + sUserId + "') ";

                //}
                //if (bDate == true)
                //{
                //    strQry += " AND TO_CHAR(ED_CRON,'DD/MM/YYYY')=TO_CHAR(SYSDATE,'DD/MM/YYYY')";
                //}
                //if (sFromDate != "" && sTodate != "")
                //{
                //    DateTime dFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //    DateTime dToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //    strQry += " AND TO_CHAR(ED_CRON,'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //}
                //if (sOfficeCode != "")
                //{
                //    strQry += " AND ED_OFFICECODE LIKE '" + sOfficeCode + "%'";
                //}
                //if (sFeederCode != "")
                //{
                //    strQry += " AND ED_FEEDERCODE LIKE '" + sFeederCode + "%'";
                //}


                strQry = "SELECT  COALESCE(COUNT(\"ED_ENUM_TYPE\"),0) FROM \"TBLENUMERATIONDETAILS\" WHERE \"ED_STATUS_FLAG\"='3' ";
                if (sUserId != "")
                {
                    strQry += " AND (\"ED_OPERATOR1\"='" + sUserId + "' OR \"ED_OPERATOR2\"='" + sUserId + "') ";

                }
                if (bDate == true)
                {
                    strQry += " AND TO_CHAR(\"ED_CRON\",'DD/MM/YYYY')=TO_CHAR(NOW(),'DD/MM/YYYY')";
                }
                if (sFromDate != "" && sTodate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    DateTime dToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                }
                if (sOfficeCode != "")
                {
                    strQry += " AND CAST(\"ED_OFFICECODE\" AS TEXT) LIKE '" + sOfficeCode + "%'";
                }
                if (sFeederCode != "")
                {
                    strQry += " AND CAST(\"ED_FEEDERCODE\" AS TEXT) LIKE '" + sFeederCode + "%'";
                }

                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetRejectEnumCount");
                return ex.Message;
            }
        }

        public string GetPendingForClarificationEnumCount(string sUserId = "", bool bDate = false, string sFromDate = "", string sTodate = "",
            string sOfficeCode = "", string sFeederCode = "")
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            try
            {
                string strQry = string.Empty;
                //strQry = "SELECT  NVL(COUNT(ED_ENUM_TYPE),0) FROM TBLENUMERATIONDETAILS WHERE ED_STATUS_FLAG='2' ";
                //if (sUserId != "")
                //{
                //    strQry += " AND (ED_OPERATOR1='" + sUserId + "' OR ED_OPERATOR2='" + sUserId + "') ";

                //}
                //if (bDate == true)
                //{
                //    strQry += " AND TO_CHAR(ED_CRON,'DD/MM/YYYY')=TO_CHAR(SYSDATE,'DD/MM/YYYY')";
                //}
                //if (sFromDate != "" && sTodate != "")
                //{
                //    DateTime dFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //    DateTime dToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //    strQry += " AND TO_CHAR(ED_CRON,'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //}
                //if (sOfficeCode != "")
                //{
                //    strQry += " AND ED_OFFICECODE LIKE '" + sOfficeCode + "%'";
                //}
                //if (sFeederCode != "")
                //{
                //    strQry += " AND ED_FEEDERCODE LIKE '" + sFeederCode + "%'";
                //}


                strQry = "SELECT  COALESCE(COUNT(\"ED_ENUM_TYPE\"),0) FROM \"TBLENUMERATIONDETAILS\" WHERE \"ED_STATUS_FLAG\"='2' ";
                if (sUserId != "")
                {
                    strQry += " AND (\"ED_OPERATOR1\"='" + sUserId + "' OR \"ED_OPERATOR2\"='" + sUserId + "') ";

                }
                if (bDate == true)
                {
                    strQry += " AND TO_CHAR(\"ED_CRON\",'DD/MM/YYYY')=TO_CHAR(NOW(),'DD/MM/YYYY')";
                }
                if (sFromDate != "" && sTodate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    DateTime dToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                }
                if (sOfficeCode != "")
                {
                    strQry += " AND CAST(\"ED_OFFICECODE\" AS TEXT) LIKE '" + sOfficeCode + "%'";
                }
                if (sFeederCode != "")
                {
                    strQry += " AND CAST(\"ED_FEEDERCODE\" AS TEXT) LIKE '" + sFeederCode + "%'";
                }


                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetRejectEnumCount");
                return ex.Message;
            }
        }

        public string GetOperatorCountForSuperVisor(string sUserId)
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT COALESCE(COUNT(\"IU_ID\"),0) FROM \"TBLINTERNALUSERS\" WHERE \"IU_SUPERVISORID\"='" + sUserId + "' ";
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetOperatorCountForSuperVisor");
                return ex.Message;
            }
        }

        public string GetTotalEnumCountForSuperVisor(string sUserId, bool bDate = false, string sFromDate = "", string sTodate = "",
             string sOfficeCode = "", string sFeederCode = "")
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            try
            {
                string strQry = string.Empty;
                //strQry = "SELECT NVL(COUNT(ED_ENUM_TYPE),0) FROM TBLENUMERATIONDETAILS WHERE  ((ED_OPERATOR2 IN ";
                //strQry += " (SELECT IU_ID FROM TBLINTERNALUSERS WHERE IU_SUPERVISORID='"+ sUserId +"'))";
                //strQry += " OR (ED_OPERATOR1 IN (SELECT IU_ID FROM TBLINTERNALUSERS WHERE IU_SUPERVISORID='" + sUserId + "'))) ";
                //if (bDate == true)
                //{
                //    strQry += " AND TO_CHAR(ED_CRON,'DD/MM/YYYY')=TO_CHAR(SYSDATE,'DD/MM/YYYY')";
                //}
                //if (sFromDate != "" && sTodate != "")
                //{
                //    DateTime dFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //    DateTime dToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //    strQry += " AND TO_CHAR(ED_CRON,'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //}
                //if (sOfficeCode != "")
                //{
                //    strQry += " AND ED_OFFICECODE LIKE '" + sOfficeCode + "%'";
                //}
                //if (sFeederCode != "")
                //{
                //    strQry += " AND ED_FEEDERCODE LIKE '" + sFeederCode + "%'";
                //}


                strQry = "SELECT COALESCE(COUNT(\"ED_ENUM_TYPE\"),0) FROM \"TBLENUMERATIONDETAILS\" WHERE  ((cast( COALESCE(\"ED_OPERATOR2\",'0') as int) IN ";
                strQry += " (SELECT \"IU_ID\" FROM \"TBLINTERNALUSERS\" WHERE \"IU_SUPERVISORID\"='" + sUserId + "'))";
                strQry += " OR (cast(\"ED_OPERATOR1\" as int) IN (SELECT \"IU_ID\" FROM \"TBLINTERNALUSERS\" WHERE \"IU_SUPERVISORID\"='" + sUserId + "'))) ";
                if (bDate == true)
                {
                    strQry += " AND TO_CHAR(\"ED_CRON\",'DD/MM/YYYY')=TO_CHAR(NOW(),'DD/MM/YYYY')";
                }
                if (sFromDate != "" && sTodate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    DateTime dToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                }
                if (sOfficeCode != "")
                {
                    strQry += " AND CAST(\"ED_OFFICECODE\" AS TEXT) LIKE '" + sOfficeCode + "%'";
                }
                if (sFeederCode != "")
                {
                    strQry += " AND CAST(\"ED_FEEDERCODE\" AS TEXT) LIKE '" + sFeederCode + "%'";
                }
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetTotalEnumCountForSuperVisor");
                return ex.Message;
            }
        }

        public string GetQCDoneCountForSuperVisor(string sUserId, bool bDate = false, string sFromDate = "", string sTodate = "",
            string sOfficeCode = "", string sFeederCode = "")
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            try
            {
                string strQry = string.Empty;
                //strQry = "SELECT NVL(COUNT(ED_ENUM_TYPE),0) FROM TBLENUMERATIONDETAILS WHERE  ((ED_OPERATOR2 IN ";
                //strQry += " (SELECT IU_ID FROM TBLINTERNALUSERS WHERE IU_SUPERVISORID='" + sUserId + "'))";
                //strQry += " OR (ED_OPERATOR1 IN (SELECT IU_ID FROM TBLINTERNALUSERS WHERE IU_SUPERVISORID='" + sUserId + "'))) ";
                //strQry += " AND  ED_STATUS_FLAG='1'";
                //if (bDate == true)
                //{
                //    strQry += " AND TO_CHAR(ED_CRON,'DD/MM/YYYY')=TO_CHAR(SYSDATE,'DD/MM/YYYY')";
                //}
                //if (sFromDate != "" && sTodate != "")
                //{
                //    DateTime dFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //    DateTime dToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //    strQry += " AND TO_CHAR(ED_CRON,'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //}
                //if (sOfficeCode != "")
                //{
                //    strQry += " AND ED_OFFICECODE LIKE '" + sOfficeCode + "%'";
                //}
                //if (sFeederCode != "")
                //{
                //    strQry += " AND ED_FEEDERCODE LIKE '" + sFeederCode + "%'";
                //}


                strQry = "SELECT COALESCE(COUNT(\"ED_ENUM_TYPE\"),0) FROM \"TBLENUMERATIONDETAILS\" WHERE  ((cast( COALESCE(\"ED_OPERATOR2\",'0') as int) IN ";
                strQry += " (SELECT \"IU_ID\" FROM \"TBLINTERNALUSERS\" WHERE \"IU_SUPERVISORID\"='" + sUserId + "'))";
                strQry += " OR ( cast(\"ED_OPERATOR1\"as int) IN (SELECT \"IU_ID\" FROM \"TBLINTERNALUSERS\" WHERE \"IU_SUPERVISORID\"='" + sUserId + "'))) ";
                strQry += " AND  \"ED_STATUS_FLAG\"='1'";
                if (bDate == true)
                {
                    strQry += " AND TO_CHAR(\"ED_CRON\",'DD/MM/YYYY')=TO_CHAR(NOW(),'DD/MM/YYYY')";
                }
                if (sFromDate != "" && sTodate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    DateTime dToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                }
                if (sOfficeCode != "")
                {
                    strQry += " AND CAST(\"ED_OFFICECODE\" AS TEXT) LIKE '" + sOfficeCode + "%'";
                }
                if (sFeederCode != "")
                {
                    strQry += " AND CAST(\"ED_FEEDERCODE\" AS TEXT)  LIKE '" + sFeederCode + "%'";
                }

                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetOperatorCountForSuperVisor");
                return ex.Message;
            }
        }

        public string GetQCPendingCountForSuperVisor(string sUserId, bool bDate = false, string sFromDate = "", string sTodate = "",
            string sOfficeCode = "", string sFeederCode = "")
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            string strQry = string.Empty;
            try
            {
                
                //strQry = "SELECT NVL(COUNT(ED_ENUM_TYPE),0) FROM TBLENUMERATIONDETAILS WHERE  ((ED_OPERATOR2 IN ";
                //strQry += " (SELECT IU_ID FROM TBLINTERNALUSERS WHERE IU_SUPERVISORID='" + sUserId + "'))";
                //strQry += " OR (ED_OPERATOR1 IN (SELECT IU_ID FROM TBLINTERNALUSERS WHERE IU_SUPERVISORID='" + sUserId + "'))) ";
                //strQry += " AND  ED_STATUS_FLAG='0'";
                //if (bDate == true)
                //{
                //    strQry += " AND TO_CHAR(ED_CRON,'DD/MM/YYYY')=TO_CHAR(SYSDATE,'DD/MM/YYYY')";
                //}
                //if (sFromDate != "" && sTodate != "")
                //{
                //    DateTime dFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //    DateTime dToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //    strQry += " AND TO_CHAR(ED_CRON,'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //}
                //if (sOfficeCode != "")
                //{
                //    strQry += " AND ED_OFFICECODE LIKE '" + sOfficeCode + "%'";
                //}
                //if (sFeederCode != "")
                //{
                //    strQry += " AND ED_FEEDERCODE LIKE '" + sFeederCode + "%'";
                //}

                strQry = "SELECT COALESCE(COUNT(\"ED_ENUM_TYPE\"),0) FROM \"TBLENUMERATIONDETAILS\" WHERE  ((cast( COALESCE(\"ED_OPERATOR2\",'0') as int) IN ";
                strQry += " (SELECT \"IU_ID\" FROM \"TBLINTERNALUSERS\" WHERE \"IU_SUPERVISORID\"='" + sUserId + "'))";
                strQry += " OR (cast(\"ED_OPERATOR1\" as int) IN (SELECT \"IU_ID\" FROM \"TBLINTERNALUSERS\" WHERE \"IU_SUPERVISORID\"='" + sUserId + "'))) ";
                strQry += " AND  \"ED_STATUS_FLAG\"='0'";
                if (bDate == true)
                {
                    strQry += " AND TO_CHAR(\"ED_CRON\",'DD/MM/YYYY')=TO_CHAR(NOW(),'DD/MM/YYYY')";
                }
                if (sFromDate != "" && sTodate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    DateTime dToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                }
                if (sOfficeCode != "")
                {
                    strQry += " AND CAST(\"ED_OFFICECODE\" AS TEXT) LIKE '" + sOfficeCode + "%'";
                }
                if (sFeederCode != "")
                {
                    strQry += " AND CAST(\"ED_FEEDERCODE\" AS TEXT) LIKE '" + sFeederCode + "%'";
                }


                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message + strQry, strFormCode, "GetQCPendingCountForSuperVisor");
                return ex.Message;
            }
        }

        public string GetQCRejectCountForSuperVisor(string sUserId, bool bDate = false, string sFromDate = "", string sTodate = "",
            string sOfficeCode = "", string sFeederCode = "")
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            try
            {
                string strQry = string.Empty;
                //strQry = "SELECT NVL(COUNT(ED_ENUM_TYPE),0) FROM TBLENUMERATIONDETAILS WHERE  ((ED_OPERATOR2 IN ";
                //strQry += " (SELECT IU_ID FROM TBLINTERNALUSERS WHERE IU_SUPERVISORID='" + sUserId + "'))";
                //strQry += " OR (ED_OPERATOR1 IN (SELECT IU_ID FROM TBLINTERNALUSERS WHERE IU_SUPERVISORID='" + sUserId + "'))) ";
                //strQry += " AND  ED_STATUS_FLAG='3'";
                //if (bDate == true)
                //{
                //    strQry += " AND TO_CHAR(ED_CRON,'DD/MM/YYYY')=TO_CHAR(SYSDATE,'DD/MM/YYYY')";
                //}
                //if (sFromDate != "" && sTodate != "")
                //{
                //    DateTime dFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //    DateTime dToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //    strQry += " AND TO_CHAR(ED_CRON,'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //}
                //if (sOfficeCode != "")
                //{
                //    strQry += " AND ED_OFFICECODE LIKE '" + sOfficeCode + "%'";
                //}
                //if (sFeederCode != "")
                //{
                //    strQry += " AND ED_FEEDERCODE LIKE '" + sFeederCode + "%'";
                //}

                strQry = "SELECT COALESCE(COUNT(\"ED_ENUM_TYPE\"),0) FROM \"TBLENUMERATIONDETAILS\" WHERE  ((cast( COALESCE(\"ED_OPERATOR2\",'0') as int) IN ";
                strQry += " (SELECT \"IU_ID\" FROM \"TBLINTERNALUSERS\" WHERE \"IU_SUPERVISORID\"='" + sUserId + "'))";
                strQry += " OR (cast(\"ED_OPERATOR1\" as int) IN (SELECT \"IU_ID\" FROM \"TBLINTERNALUSERS\" WHERE \"IU_SUPERVISORID\"='" + sUserId + "'))) ";
                strQry += " AND  \"ED_STATUS_FLAG\"='3'";
                if (bDate == true)
                {
                    strQry += " AND TO_CHAR(\"ED_CRON\",'DD/MM/YYYY')=TO_CHAR(NOW(),'DD/MM/YYYY')";
                }
                if (sFromDate != "" && sTodate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    DateTime dToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                }
                if (sOfficeCode != "")
                {
                    strQry += " AND CAST(\"ED_OFFICECODE\" AS TEXT) LIKE '" + sOfficeCode + "%'";
                }
                if (sFeederCode != "")
                {
                    strQry += " AND CAST(\"ED_FEEDERCODE\" AS TEXT) LIKE '" + sFeederCode + "%'";
                }

                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetQCRejectCountForSuperVisor");
                return ex.Message;
            }
        }

        public string GetPendingForClarificationCountForSuperVisor(string sUserId, bool bDate = false, string sFromDate = "", string sTodate = "",
             string sOfficeCode = "", string sFeederCode = "")
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            try
            {
                string strQry = string.Empty;
                //strQry = "SELECT NVL(COUNT(ED_ENUM_TYPE),0) FROM TBLENUMERATIONDETAILS WHERE  ((ED_OPERATOR2 IN ";
                //strQry += " (SELECT IU_ID FROM TBLINTERNALUSERS WHERE IU_SUPERVISORID='" + sUserId + "'))";
                //strQry += " OR (ED_OPERATOR1 IN (SELECT IU_ID FROM TBLINTERNALUSERS WHERE IU_SUPERVISORID='" + sUserId + "'))) ";
                //strQry += " AND  ED_STATUS_FLAG='2'";
                //if (bDate == true)
                //{
                //    strQry += " AND TO_CHAR(ED_CRON,'DD/MM/YYYY')=TO_CHAR(SYSDATE,'DD/MM/YYYY')";
                //}
                //if (sFromDate != "" && sTodate != "")
                //{
                //    DateTime dFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //    DateTime dToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //    strQry += " AND TO_CHAR(ED_CRON,'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //}
                //if (sOfficeCode != "")
                //{
                //    strQry += " AND ED_OFFICECODE LIKE '" + sOfficeCode + "%'";
                //}
                //if (sFeederCode != "")
                //{
                //    strQry += " AND ED_FEEDERCODE LIKE '" + sFeederCode + "%'";
                //}

                strQry = "SELECT COALESCE(COUNT(\"ED_ENUM_TYPE\"),0) FROM \"TBLENUMERATIONDETAILS\" WHERE  ((cast( COALESCE(\"ED_OPERATOR2\",'0') as int) IN ";
                strQry += " (SELECT \"IU_ID\" FROM \"TBLINTERNALUSERS\" WHERE \"IU_SUPERVISORID\"='" + sUserId + "'))";
                strQry += " OR (cast(\"ED_OPERATOR1\" as int) IN (SELECT \"IU_ID\" FROM \"TBLINTERNALUSERS\" WHERE \"IU_SUPERVISORID\"='" + sUserId + "'))) ";
                strQry += " AND  \"ED_STATUS_FLAG\"='2'";
                if (bDate == true)
                {
                    strQry += " AND TO_CHAR(\"ED_CRON\",'DD/MM/YYYY')=TO_CHAR(NOW(),'DD/MM/YYYY')";
                }
                if (sFromDate != "" && sTodate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    DateTime dToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                }
                if (sOfficeCode != "")
                {
                    strQry += " AND CAST(\"ED_OFFICECODE\" AS TEXT) LIKE '" + sOfficeCode + "%'";
                }
                if (sFeederCode != "")
                {
                    strQry += " AND CAST(\"ED_FEEDERCODE\" AS TEXT) LIKE '" + sFeederCode + "%'";
                }


                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetPendingForClarificationCountForSuperVisor");
                return ex.Message;
            }
        }

        public string GetOperatorCount()
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT COALESCE(COUNT(\"IU_ID\"),0) FROM \"TBLINTERNALUSERS\" ";
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetOperatorCount");
                return ex.Message;
            }
        }

        public DataTable LoadStatusReportLocationWise(clsInterDashboard objDashBoard)
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                string sOffcodeLength = string.Empty;


                if (objDashBoard.sOfficeCode.Length == 0)
                {
                    sOffcodeLength = Division_code.ToString();
                }
                else if (objDashBoard.sOfficeCode.Length == 3)
                {
                    sOffcodeLength = SubDiv_code.ToString();
                }
                else
                {
                    sOffcodeLength = SubDiv_code.ToString();
                }
                //strQry = "  SELECT (SELECT OFF_NAME FROM VIEW_ALL_OFFICES WHERE OFF_CODE=SUBSTR(ED_OFFICECODE,1," + sOffcodeLength + ")) as LOCATION,";
                //strQry += " SUBSTR(ED_OFFICECODE,1," + sOffcodeLength + ") OFFICECODE,'' AS FD_FEEDER_NAME,";
                //strQry += " NVL(SUM(CASE WHEN ED_STATUS_FLAG<>'5' THEN 1 ELSE 0 END ),0) TOTAL,";
                //strQry += " NVL(SUM(CASE WHEN ED_STATUS_FLAG='0' THEN 1 ELSE 0 END ),0) QCPENDING,";
                //strQry += " NVL(SUM(CASE WHEN ED_STATUS_FLAG='1' THEN 1 ELSE 0 END ),0) QCDONE, ";
                //strQry += " NVL(SUM(CASE WHEN ED_STATUS_FLAG='3' THEN 1 ELSE 0 END ),0) QCREJECT,";
                //strQry += " NVL(SUM(CASE WHEN ED_STATUS_FLAG='2' THEN 1 ELSE 0 END ),0) PENDING_CLAR,";
                //if (sOffcodeLength == Division_code.ToString())
                //{
                //    strQry += " '' as ENUMTYPE ";
                //}
                //else
                //{
                //    strQry += " DECODE(ED_LOCTYPE,'1','STORE','2','FIELD','3','REPAIRER','5','TRANSFORMER BANK') as ENUMTYPE";
                //}
                //strQry += " FROM TBLENUMERATIONDETAILS WHERE ED_STATUS_FLAG<>'5' AND ED_OFFICECODE<>'8888'  ";
                ////AND LENGTH(SUBSTR(ED_OFFICECODE,1," + sOffcodeLength + ")) = " + sOffcodeLength + "
                //if (bCurrentDate == true)
                //{
                //    strQry += " AND TO_CHAR(ED_CRON,'DD/MM/YYYY')= TO_CHAR(SYSDATE,'DD/MM/YYYY') ";
                //}
                //if (sOfficeCode != "")
                //{
                //    strQry += " AND ED_OFFICECODE LIKE '" + objDashBoard.sOfficeCode + "%'";
                //}
                //if (objDashBoard.sFromDate != "" && objDashBoard.sToDate != "")
                //{
                //    DateTime dFromDate = DateTime.ParseExact(objDashBoard.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //    DateTime dToDate = DateTime.ParseExact(objDashBoard.sToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //    strQry += " AND TO_CHAR(ED_CRON,'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //}
                //strQry += " GROUP BY  SUBSTR(ED_OFFICECODE,1," + sOffcodeLength + ")";

                //if (sOffcodeLength != Division_code.ToString())
                //{
                //    strQry += ",ED_LOCTYPE";
                //}


                strQry = "  SELECT \"VM_ID\",\"VM_NAME\",\"OFF_NAME\" as \"LOCATION\",";
                strQry += " SUBSTRING(CAST(\"ED_OFFICECODE\" AS TEXT),1," + sOffcodeLength + ") \"OFFICECODE\",'' AS \"FD_FEEDER_NAME\",";
                strQry += " COALESCE(SUM(CASE WHEN \"ED_STATUS_FLAG\"<>'5' THEN 1 ELSE 0 END ),0) \"TOTAL\",";
                strQry += " COALESCE(SUM(CASE WHEN \"ED_STATUS_FLAG\"='0' THEN 1 ELSE 0 END ),0) \"QCPENDING\",";
                strQry += " COALESCE(SUM(CASE WHEN \"ED_STATUS_FLAG\"='1' THEN 1 ELSE 0 END ),0) \"QCDONE\", ";
                strQry += " COALESCE(SUM(CASE WHEN \"ED_STATUS_FLAG\"='3' THEN 1 ELSE 0 END ),0) \"QCREJECT\",";
                strQry += " COALESCE(SUM(CASE WHEN \"ED_STATUS_FLAG\"='2' THEN 1 ELSE 0 END ),0) \"PENDING_CLAR\",";
                if (sOffcodeLength == Division_code.ToString())
                {
                    strQry += " '' as \"ENUMTYPE\" ";
                }
                else
                {
                    strQry += " CASE WHEN \"ED_LOCTYPE\"=1 THEN 'STORE' WHEN \"ED_LOCTYPE\"=2 THEN 'FIELD' WHEN \"ED_LOCTYPE\"=3 THEN 'REPAIRER' WHEN ";
                    strQry += " \"ED_LOCTYPE\"=5 THEN 'TRANSFORMER BANK' END as \"ENUMTYPE\"";
                }
                strQry += " FROM \"TBLENUMERATIONDETAILS\",\"TBLINTERNALUSERS\",\"TBLVENDORMASTER\",\"VIEW_ALL_OFFICES\" WHERE \"IU_ID\"=\"ED_OPERATOR1\" ";
                strQry += " AND \"IU_VENDOR_ID\"=\"VM_ID\" AND CAST(\"OFF_CODE\" AS TEXT)=SUBSTRING(CAST(\"ED_OFFICECODE\" AS TEXT),1,'" + sOffcodeLength + "') ";
                strQry += " AND \"ED_STATUS_FLAG\"<>'5' AND \"ED_OFFICECODE\"<>'8888'  ";
                //AND LENGTH(SUBSTRING(\"ED_OFFICECODE\",1," + sOffcodeLength + ")) = " + sOffcodeLength + "
                if (bCurrentDate == true)
                {
                    strQry += " AND TO_CHAR(\"ED_CRON\",'DD/MM/YYYY')= TO_CHAR(NOW(),'DD/MM/YYYY') ";
                }
                if (sOfficeCode != "")
                {
                    strQry += " AND CAST(\"ED_OFFICECODE\" AS TEXT) LIKE '" + objDashBoard.sOfficeCode + "%'";
                }
                if (objDashBoard.sVendorId != "")
                {
                    strQry += " AND \"VM_ID\" = "+objDashBoard.sVendorId+" ";
                }
                if (objDashBoard.sFromDate != "" && objDashBoard.sToDate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(objDashBoard.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    DateTime dToDate = DateTime.ParseExact(objDashBoard.sToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                }
                strQry += " GROUP BY SUBSTRING(CAST(\"ED_OFFICECODE\" AS TEXT),1,'"+ sOffcodeLength + "'),\"LOCATION\",\"VM_NAME\",\"VM_ID\" "; 

                if (sOffcodeLength != Division_code.ToString())
                {
                    strQry += ",\"ED_LOCTYPE\"";
                }

                return ObjCon.FetchDataTable(strQry);

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadStatusReportLocationWise");
                return dt;
            }
        }

        public DataTable LoadStatusReportFeederWise(clsInterDashboard objDashBoard)
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;

                // strQry = "SELECT ED_FEEDERCODE || ' - ' || FD_FEEDER_NAME AS FD_FEEDER_NAME,ED_FEEDERCODE,'' AS LOCATION,'' AS OFFICECODE,";
                //strQry+= " NVL(SUM(CASE WHEN ED_STATUS_FLAG<>'5' THEN 1 ELSE 0 END ),0) TOTAL,";
                //strQry+= " NVL(SUM(CASE WHEN ED_STATUS_FLAG='0' THEN 1 ELSE 0 END ),0) QCPENDING,";
                //strQry+= " NVL(SUM(CASE WHEN ED_STATUS_FLAG='1' THEN 1 ELSE 0 END ),0) QCDONE,";
                //strQry+= " NVL(SUM(CASE WHEN ED_STATUS_FLAG='3' THEN 1 ELSE 0 END ),0) QCREJECT,";
                //strQry+= " NVL(SUM(CASE WHEN ED_STATUS_FLAG='2' THEN 1 ELSE 0 END ),0) PENDING_CLAR, ";
                //strQry += " '' AS ENUMTYPE";
                //strQry+= " FROM TBLENUMERATIONDETAILS,TBLFEEDERMAST,TBLFEEDEROFFCODE WHERE  ED_STATUS_FLAG<>'5' AND FD_FEEDER_ID=FDO_FEEDER_ID ";
                //strQry += " AND ED_FEEDERCODE=FD_FEEDER_CODE AND FDO_OFFICE_CODE LIKE '" + objDashBoard.sOfficeCode + "%' ";
                //if (objDashBoard.bCurrentDate == true)
                //{
                //    strQry += " AND TO_CHAR(ED_CRON,'DD/MM/YYYY')= TO_CHAR(SYSDATE,'DD/MM/YYYY') ";
                //}
                //if (objDashBoard.sFeederCode != "")
                //{
                //    strQry += " AND ED_FEEDERCODE LIKE '" + objDashBoard.sFeederCode + "%' ";
                //}
                //if (objDashBoard.sFromDate != "" && objDashBoard.sToDate != "")
                //{
                //    DateTime dFromDate = DateTime.ParseExact(objDashBoard.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //    DateTime dToDate = DateTime.ParseExact(objDashBoard.sToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                //    strQry += " AND TO_CHAR(ED_CRON,'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                //}
                //strQry += " GROUP BY ED_FEEDERCODE,FD_FEEDER_NAME";

                strQry = "SELECT \"VM_ID\",\"VM_NAME\",\"ED_FEEDERCODE\" || ' - ' || \"FD_FEEDER_NAME\" AS \"FD_FEEDER_NAME\",\"ED_FEEDERCODE\",'' AS \"LOCATION\",'' AS \"OFFICECODE\",";
                strQry += " COALESCE(SUM(CASE WHEN \"ED_STATUS_FLAG\"<>'5' THEN 1 ELSE 0 END ),0) \"TOTAL\",";
                strQry += " COALESCE(SUM(CASE WHEN \"ED_STATUS_FLAG\"='0' THEN 1 ELSE 0 END ),0) \"QCPENDING\",";
                strQry += " COALESCE(SUM(CASE WHEN \"ED_STATUS_FLAG\"='1' THEN 1 ELSE 0 END ),0) \"QCDONE\", ";
                strQry += " COALESCE(SUM(CASE WHEN \"ED_STATUS_FLAG\"='3' THEN 1 ELSE 0 END ),0) \"QCREJECT\",";
                strQry += " COALESCE(SUM(CASE WHEN \"ED_STATUS_FLAG\"='2' THEN 1 ELSE 0 END ),0) \"PENDING_CLAR\",";
                strQry += " '' as \"ENUMTYPE\" ";
                strQry += " FROM \"TBLENUMERATIONDETAILS\",\"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\",\"TBLINTERNALUSERS\",\"TBLVENDORMASTER\" WHERE \"IU_ID\"=\"ED_OPERATOR1\" AND \"IU_VENDOR_ID\"=\"VM_ID\" AND \"ED_STATUS_FLAG\"<>'5' AND CAST(\"FD_FEEDER_ID\" AS TEXT)=CAST(\"FDO_FEEDER_ID\" AS TEXT) ";
                strQry += " AND CAST(\"ED_FEEDERCODE\" AS TEXT)=CAST(\"FD_FEEDER_CODE\" AS TEXT) AND CAST(\"FDO_OFFICE_CODE\" AS TEXT) LIKE '" + objDashBoard.sOfficeCode + "%' ";
                if (objDashBoard.bCurrentDate == true)
                {
                    strQry += " AND TO_CHAR(\"ED_CRON\",'DD/MM/YYYY')= TO_CHAR(NOW(),'DD/MM/YYYY') ";
                }
                if (objDashBoard.sFeederCode != "")
                {
                    strQry += " AND CAST(\"ED_FEEDERCODE\" AS TEXT) LIKE '" + objDashBoard.sFeederCode + "%' ";
                }
                if (objDashBoard.sFromDate != "" && objDashBoard.sToDate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(objDashBoard.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    DateTime dToDate = DateTime.ParseExact(objDashBoard.sToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                }
                strQry += " GROUP BY \"ED_FEEDERCODE\",\"FD_FEEDER_NAME\",\"VM_NAME\",\"VM_ID\" ";


                return ObjCon.FetchDataTable(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadStatusReportFeederWise");
                return dt;
            }
        }

        public DataTable GetEnumeration_Record()
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            string strQry = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                strQry = "select count(*) TOTAL_RECORD,sum(case when \"ED_STATUS_FLAG\"='0' THEN 1 ELSE 0 END)PENDING, sum(case when ";
                strQry += " \"ED_STATUS_FLAG\"='1' THEN 1 ELSE 0 END)QC_DONE,to_char(\"ED_CRON\",'dd')PRESENT_MONTH_DAYS,(select to_char(NOW(),'MONTH'))PRESENT_MONTH FROM \"TBLENUMERATIONDETAILS\" ";
                strQry+= " WHERE (select to_char(NOW(),'MM'))=to_char(\"ED_CRON\",'MM')  GROUP BY to_char(\"ED_CRON\",'dd') ORDER BY to_char(\"ED_CRON\",'dd')";
                dt = ObjCon.FetchDataTable(strQry);
                return dt;
            }
            catch(Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetEnumeration_Record");
                return dt;
            }
        }


        public DataTable LoadStatusReportVendorWise(clsInterDashboard objDashBoard)
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                string sOffcodeLength = string.Empty;

                strQry = "  SELECT \"VM_NAME\", COALESCE(SUM(CASE WHEN \"ED_STATUS_FLAG\"<>'5' THEN 1 ELSE 0 END ),0) \"TOTAL\", ";
                strQry += " COALESCE(SUM(CASE WHEN \"ED_STATUS_FLAG\"='0' THEN 1 ELSE 0 END ),0) \"QCPENDING\", COALESCE(SUM(CASE WHEN ";
                strQry += " \"ED_STATUS_FLAG\" = '1' THEN 1 ELSE 0 END ),0) \"QCDONE\",  COALESCE(SUM(CASE WHEN \"ED_STATUS_FLAG\"='3' ";
                strQry += " THEN 1 ELSE 0 END ),0) \"QCREJECT\", COALESCE(SUM(CASE WHEN \"ED_STATUS_FLAG\"='2' THEN 1 ELSE 0 END ),0) ";
                strQry += " \"PENDING_CLAR\", \"VM_ID\"  FROM \"TBLENUMERATIONDETAILS\",\"TBLINTERNALUSERS\",\"TBLVENDORMASTER\" WHERE \"IU_ID\"=\"ED_OPERATOR1\" AND \"IU_VENDOR_ID\"=\"VM_ID\" AND \"ED_STATUS_FLAG\"<>'5' AND \"ED_OFFICECODE\"<>'8888' ";
                strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '20180301' AND '20180326' GROUP BY  \"VM_ID\"";


                strQry = "  SELECT \"VM_NAME\",\"VM_ID\",'' AS \"FD_FEEDER_NAME\",";
                //strQry += " SUBSTRING(CAST(\"ED_OFFICECODE\" AS TEXT),1," + sOffcodeLength + ") \"OFFICECODE\",'' AS \"FD_FEEDER_NAME\",";
                strQry += " COALESCE(SUM(CASE WHEN \"ED_STATUS_FLAG\"<>'5' THEN 1 ELSE 0 END ),0) \"TOTAL\",";
                strQry += " COALESCE(SUM(CASE WHEN \"ED_STATUS_FLAG\"='0' THEN 1 ELSE 0 END ),0) \"QCPENDING\",";
                strQry += " COALESCE(SUM(CASE WHEN \"ED_STATUS_FLAG\"='1' THEN 1 ELSE 0 END ),0) \"QCDONE\", ";
                strQry += " COALESCE(SUM(CASE WHEN \"ED_STATUS_FLAG\"='3' THEN 1 ELSE 0 END ),0) \"QCREJECT\",";
                strQry += " COALESCE(SUM(CASE WHEN \"ED_STATUS_FLAG\"='2' THEN 1 ELSE 0 END ),0) \"PENDING_CLAR\",";
                if (sOffcodeLength == "")
                {
                    strQry += " '' as \"ENUMTYPE\",'' AS OFFICECODE, '' AS LOCATION ";
                }
                else
                {
                    strQry += " CASE WHEN \"ED_LOCTYPE\"=1 THEN 'STORE' WHEN \"ED_LOCTYPE\"=2 THEN 'FIELD' WHEN \"ED_LOCTYPE\"=3 THEN 'REPAIRER' WHEN \"ED_LOCTYPE\"=5 THEN 'TRANSFORMER BANK' END as \"ENUMTYPE\"";
                }
                strQry += " FROM \"TBLENUMERATIONDETAILS\",\"TBLINTERNALUSERS\",\"TBLVENDORMASTER\" WHERE \"IU_ID\"=\"ED_OPERATOR1\" AND \"IU_VENDOR_ID\"=\"VM_ID\" AND \"ED_STATUS_FLAG\"<>'5' AND \"ED_OFFICECODE\"<>'8888'  ";
                
                if (bCurrentDate == true)
                {
                    strQry += " AND TO_CHAR(\"ED_CRON\",'DD/MM/YYYY')= TO_CHAR(NOW(),'DD/MM/YYYY') ";
                }
                if (sOfficeCode != "")
                {
                    strQry += " AND CAST(\"ED_OFFICECODE\" AS TEXT) LIKE '" + objDashBoard.sOfficeCode + "%'";
                }
                if (objDashBoard.sFromDate != "" && objDashBoard.sToDate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(objDashBoard.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    DateTime dToDate = DateTime.ParseExact(objDashBoard.sToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    strQry += " AND TO_CHAR(\"ED_CRON\",'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                }
                strQry += " GROUP BY \"VM_ID\" ";

                return ObjCon.FetchDataTable(strQry);

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadStatusReportLocationWise");
                return dt;
            }
        }

        public string GetApprovalDetails()
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            try
            {
                string sQry = string.Empty;

                sQry = "SELECT COALESCE(COUNT(*),0) FROM \"TBLENUMERATIONDETAILS\" WHERE \"ED_APPROVALPRIORITY\" = '1'";
                String sFirst = ObjCon.get_value(sQry);

                sQry = "SELECT COALESCE(COUNT(*),0) FROM \"TBLENUMERATIONDETAILS\" WHERE \"ED_APPROVALPRIORITY\" = '2'";
                String sSecond = ObjCon.get_value(sQry);

                sQry = "SELECT COALESCE(COUNT(*),0) FROM \"TBLENUMERATIONDETAILS\" WHERE \"ED_APPROVALPRIORITY\" = '3'";
                String sThird = ObjCon.get_value(sQry);

                sQry = "SELECT COALESCE(COUNT(*),0) FROM \"TBLENUMERATIONDETAILS\" WHERE \"ED_APPROVALPRIORITY\" = '4'";
                String sFourth = ObjCon.get_value(sQry);

                sQry = "SELECT COALESCE(COUNT(*),0) FROM \"TBLENUMERATIONDETAILS\" WHERE \"ED_APPROVALPRIORITY\" = '5'";
                String sFifth = ObjCon.get_value(sQry);

                return sFirst + "~" + sSecond + "~" + sThird + "~" + sFourth + "~" + sFifth;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetApprovalDetails");
                return "";
            }
        }

        public bool CheckFeederCompletion(string sOffCode)
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            try
            {
                string sQry = string.Empty;
                sQry = "SELECT COUNT(*) FROM \"TBLENUMERATIONDETAILS\" WHERE \"ED_APPROVALPRIORITY\" <= 3 AND \"ED_OFFICECODE\" = '"+ sOffCode + "' ";
                sQry += "AND \"ED_STATUS_FLAG\" = 0";
                string sResult = ObjCon.get_value(sQry);
                if(Convert.ToInt32(sResult) > 0)
                {
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CheckFeeder");
                return false;
            }
        }

    }
}
