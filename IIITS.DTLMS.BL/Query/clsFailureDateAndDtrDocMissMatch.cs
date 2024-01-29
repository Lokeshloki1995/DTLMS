using IIITS.DAL;
using IIITS.PGSQL.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;
using Npgsql;
using System.Text.RegularExpressions;

namespace IIITS.DTLMS.BL.Query
{
    public class clsFailureDateAndDtrDocMissMatch
    {
        PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationManager.AppSettings["pgSQLPassword"]));
        public string DtcCode { get; set; }
        public string DtrCode { get; set; }
        public string DTrCommissionDate { get; set; }
        public string DTrFailureDate { get; set; }
        public string TicketNumber { get; set; }
        public string DfDtrDoc { get; set; }
        public string strEstDate { get; set; }
        public string strDfDate { get; set; }
        public string strDtcDoc { get; set; }


        /// <summary>
        /// Getting DTc Detials for updating DTr Doc
        /// </summary>
        /// <param name="objCommdateDetails"></param>
        /// <returns></returns>
        public DataTable LoadDtcDtr(clsFailureDateAndDtrDocMissMatch objCommdateDetails)
        {
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_dtc_dtr_details_for_doc_correction");
                cmd.Parameters.AddWithValue("failure_dtc", Convert.ToString(objCommdateDetails.DtcCode));
                dt = ObjCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(                             MethodBase.GetCurrentMethod().DeclaringType.Name,                             MethodBase.GetCurrentMethod().Name,                             ex.Message,                             ex.StackTrace);
                return dt;
            }
            finally
            {

            }
        }

        /// <summary>
        /// Updating DTr DOC 
        /// </summary>
        /// <param name="objCommDate"></param>
        /// <returns></returns>
        public string[] UpdateCommDate(clsFailureDateAndDtrDocMissMatch objCommDate)
        {
            string[] Arr = new string[2];
            string strQry = string.Empty;
            StringBuilder sbQry = new StringBuilder();
            string strTmId = string.Empty;
            string strDfId = string.Empty;
            string strTrId = string.Empty;
            string strResult = string.Empty;
            string DTrCommissionDate1 = string.Empty;

            StringBuilder strOldCommissionDate = new StringBuilder();

            try
            {
                NpgsqlCommand cmd1 = new NpgsqlCommand("proc_update_dtr_doc_to_tbltransdtcmapping");
                cmd1.Parameters.AddWithValue("failure_dtc", Convert.ToString(objCommDate.DtcCode));
                cmd1.Parameters.AddWithValue("failure_dtr", Convert.ToString(objCommDate.DtrCode));
                DataTable dt = ObjCon.FetchDataTable(cmd1);
                if (dt.Rows.Count == 0)
                {
                    NpgsqlCommand cmd2 = new NpgsqlCommand("proc_update_dtr_doc_to_tbltransdtcmapping1");
                    cmd2.Parameters.AddWithValue("failure_dtc", Convert.ToString(objCommDate.DtcCode));
                    cmd2.Parameters.AddWithValue("failure_dtr", Convert.ToString(objCommDate.DtrCode));
                    dt = ObjCon.FetchDataTable(cmd2);
                    if (dt.Rows.Count > 0)
                    {
                        strTmId = Convert.ToString(dt.Rows[0]["TM_ID"]);
                        strOldCommissionDate.Append(Convert.ToString(dt.Rows[0]["TM_MAPPING_DATE"]) + ";");
                    }
                }
                else
                {
                    strTmId = Convert.ToString(dt.Rows[0]["TM_ID"]);
                    strOldCommissionDate.Append(Convert.ToString(dt.Rows[0]["TM_MAPPING_DATE"]) + ";");
                }

                NpgsqlCommand cmd = new NpgsqlCommand("proc_update_dtr_doc_to_failure_records");
                cmd.Parameters.AddWithValue("failure_dtc", Convert.ToString(objCommDate.DtcCode));
                cmd.Parameters.AddWithValue("failure_dtr", Convert.ToString(objCommDate.DtrCode));
                dt = ObjCon.FetchDataTable(cmd);
                if (dt.Rows.Count > 0)
                {
                    strDfId = Convert.ToString(dt.Rows[0]["DF_ID"]);
                    strOldCommissionDate.Append(Convert.ToString(dt.Rows[0]["DF_DTR_COMMISSION_DATE"]) + ";");
                    strTrId = Convert.ToString(dt.Rows[0]["TR_ID"]);
                    strOldCommissionDate.Append(Convert.ToString(dt.Rows[0]["TR_COMM_DATE"]) + ";");
                    strDfDate = Convert.ToString(dt.Rows[0]["DF_DATE"]);
                }

                NpgsqlCommand cmd3 = new NpgsqlCommand("proc_load_dtc_doc");
                cmd3.Parameters.AddWithValue("failure_dtc", Convert.ToString(objCommDate.DtcCode));
                dt = ObjCon.FetchDataTable(cmd3);
                if (dt.Rows.Count > 0)
                {
                    strDtcDoc = Convert.ToString(dt.Rows[0]["DT_TRANS_COMMISION_DATE"]);
                }

                if (objCommDate.strDfDate != "" && objCommDate.strDfDate != null)
                {
                    strResult = Genaral.DateComparision(DTrCommissionDate, strDfDate, false, false);
                    if (strResult == "1")
                    {
                        Arr[0] = "DTr Commission Date should be Less than or Equal to Failure Date";
                        Arr[1] = "1";
                        return Arr;
                    }
                }

                if (objCommDate.strDtcDoc != "" && objCommDate.strDtcDoc != null)
                {
                    strResult = Genaral.DateComparision(DTrCommissionDate, strDtcDoc, false, false);
                    if (strResult == "2")
                    {
                        Arr[0] = "DTr Commission Date should be Greater than or Equal to DTc Commission Date";
                        Arr[1] = "1";
                        return Arr;
                    }
                }

                DTrCommissionDate1 = objCommDate.DTrCommissionDate;

                DateTime myDateTime = DateTime.ParseExact(objCommDate.DTrCommissionDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DTrCommissionDate = Convert.ToDateTime(myDateTime).ToString("yyyy-MM-dd");

                if (objCommDate.strDfDate != "" && objCommDate.strDfDate != null)
                {
                    DateTime my1DateTime = DateTime.ParseExact(objCommDate.strDfDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    strDfDate = Convert.ToDateTime(my1DateTime).ToString("yyyy-MM-dd");
                }

                if (objCommDate.strDtcDoc != "" && objCommDate.strDtcDoc != null)
                {
                    DateTime my2DateTime = DateTime.ParseExact(objCommDate.strDtcDoc.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    strDtcDoc = Convert.ToDateTime(my2DateTime).ToString("yyyy-MM-dd");
                }

                ObjCon.BeginTransaction();

                string strCommDate = strOldCommissionDate.ToString();
                strCommDate = strCommDate.Substring(0, strOldCommissionDate.Length - 1);
                if (strTmId != null && strTmId != "")
                {
                    strQry = "UPDATE \"TBLTRANSDTCMAPPING\" SET \"TM_MAPPING_DATE\"=TO_DATE('" + DTrCommissionDate1 + "','DD/MM/YYYY') WHERE ";
                    strQry += " \"TM_ID\"='" + strTmId + "' ";
                    sbQry.Append(strQry);
                    ObjCon.ExecuteQry(strQry);
                }
                if (strDfId != null && strDfId != "")
                {
                    strQry = "UPDATE \"TBLDTCFAILURE\" SET \"DF_DTR_COMMISSION_DATE\"=TO_DATE('" + DTrCommissionDate1 + "','DD/MM/YYYY') WHERE ";
                    strQry += " \"DF_ID\" ='" + strDfId + "' ";
                    sbQry.Append(strQry);
                    ObjCon.ExecuteQry(strQry);
                }
               
                sbQry = sbQry.Replace("'", "''");
                string Description = " Changed  DTRCommission Date from " + Convert.ToString(strCommDate) + " to " + DTrCommissionDate1 + " ";
                Description += "for " + DtcCode + " and " + DtrCode + "";
                strQry = "  INSERT into \"TBLBACKENDACTIVITYDETAILS\"(\"BAD_BM_ID\",\"BAD_OLDDATA\",\"BAD_NEWDATA\", ";
                strQry += "\"BAD_DESCRITION\" ";
                strQry += ",\"BAD_DTCCODE\",\"BAD_DTRCODE\",\"BAD_QUERY\",\"BAD_TICKETNUMBER\") ";
                strQry += "  VALUES (6,'" + strCommDate + "',";
                strQry += "'" + DTrCommissionDate1 + "', '" + Description + "' ";
                strQry += ",'" + DtcCode + "','" + DtrCode + "',  '" + Convert.ToString(sbQry) + "' , '" + objCommDate.TicketNumber + "') ";
                ObjCon.ExecuteQry(strQry);

                Arr[0] = "Updated Successfully ";
                Arr[1] = "0";
                ObjCon.CommitTransaction();

                return Arr;
            }

            catch (Exception ex)
            {
                ObjCon.RollBackTrans();
                clsException.LogError(                             MethodBase.GetCurrentMethod().DeclaringType.Name,                             MethodBase.GetCurrentMethod().Name,                             ex.Message,                             ex.StackTrace);
                return Arr;
            }

        }

        /// <summary>
        /// Getting DTc Details for updating failure date
        /// </summary>
        /// <param name="objFaildateDetails"></param>
        /// <returns></returns>
        public DataTable LoadFailDtcDtr(clsFailureDateAndDtrDocMissMatch objFaildateDetails)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_failure_dtc_dtr_details");
                cmd.Parameters.AddWithValue("failure_dtc", Convert.ToString(objFaildateDetails.DtcCode));
                dt = ObjCon.FetchDataTable(cmd);

                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(                             MethodBase.GetCurrentMethod().DeclaringType.Name,                             MethodBase.GetCurrentMethod().Name,                             ex.Message,                             ex.StackTrace);
                return dt;
            }
            finally
            {

            }
        }

        /// <summary>
        /// Updating Filure Date
        /// </summary>
        /// <param name="objFailDate"></param>
        /// <returns></returns>
        public string[] UpdateFailDate(clsFailureDateAndDtrDocMissMatch objFailDate)
        {
            string[] Arr = new string[2];
            string strQry = string.Empty;
            string strResult = string.Empty;
            string strDfId = string.Empty;
            string strOldFailDate = string.Empty;
            string DTrFailureDate1 = string.Empty;

            StringBuilder sbQry = new StringBuilder();

            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_validating_while_update_failure_date");
                cmd.Parameters.AddWithValue("failure_dtc", Convert.ToString(objFailDate.DtcCode));
                cmd.Parameters.AddWithValue("failure_dtr", Convert.ToString(objFailDate.DtrCode));
                DataTable dt = ObjCon.FetchDataTable(cmd);
                if (dt.Rows.Count > 0)
                {
                    strDfId = Convert.ToString(dt.Rows[0]["DF_ID"]);
                    strOldFailDate = Convert.ToString(dt.Rows[0]["DF_DATE"]);
                    strEstDate = Convert.ToString(dt.Rows[0]["EST_DATE"]);
                    DfDtrDoc = Convert.ToString(dt.Rows[0]["DF_DTR_COMMISSION_DATE"]);

                }

                if (objFailDate.DfDtrDoc != "" && objFailDate.DfDtrDoc != null)
                {
                    strResult = Genaral.DateComparision(DTrFailureDate, DfDtrDoc, false, false);
                    if (strResult == "2")
                    {
                        Arr[0] = "Failure Date should be Greater than or Equal to Commission Date";
                        Arr[1] = "1";
                        return Arr;
                    }
                }

                if (objFailDate.strEstDate != "" && objFailDate.strEstDate != null)
                {
                    strResult = Genaral.DateComparision(DTrFailureDate, strEstDate, false, false);
                    if (strResult == "1")
                    {
                        Arr[0] = "Failure Date Should be Less than or Equal to Estimation date ";
                        Arr[1] = "1";
                        return Arr;
                    }
                 }
                DTrFailureDate1 = objFailDate.DTrFailureDate;

                DateTime myDateTime = DateTime.ParseExact(objFailDate.DTrFailureDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DTrFailureDate = Convert.ToDateTime(myDateTime).ToString("yyyy-MM-dd");

                if (objFailDate.DfDtrDoc != "" && objFailDate.DfDtrDoc != null)
                {
                    DateTime my1DateTime = DateTime.ParseExact(objFailDate.DfDtrDoc.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    DfDtrDoc = Convert.ToDateTime(my1DateTime).ToString("yyyy-MM-dd");
                }

                if (objFailDate.strEstDate != "" && objFailDate.strEstDate != null)
                {
                    DateTime my2DateTime = DateTime.ParseExact(objFailDate.strEstDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    strEstDate = Convert.ToDateTime(my2DateTime).ToString("yyyy-MM-dd");
                }

                ObjCon.BeginTransaction();

                if (strDfId != null || strDfId != "")
                {
                    strQry = "UPDATE \"TBLDTCFAILURE\" SET \"DF_DATE\"=TO_DATE('" + DTrFailureDate1 + "','DD/MM/YYYY') WHERE ";
                    strQry += " \"DF_ID\" ='" + strDfId + "' ";
                    sbQry.Append(strQry);
                    ObjCon.ExecuteQry(strQry);
                }
                sbQry = sbQry.Replace("'", "''");
                string Description = " Changed  DTRFailre Date from " + Convert.ToString(strOldFailDate) + " to " + DTrFailureDate1 + " ";
                Description += "for " + DtcCode + " and " + DtrCode + "";
                strQry = "  INSERT into \"TBLBACKENDACTIVITYDETAILS\"(\"BAD_BM_ID\",\"BAD_OLDDATA\",\"BAD_NEWDATA\", ";
                strQry += "\"BAD_DESCRITION\",\"BAD_DTCCODE\",\"BAD_DTRCODE\",\"BAD_QUERY\",\"BAD_TICKETNUMBER\") ";
                strQry += "  VALUES (6,'" + strOldFailDate + "',";
                strQry += "'" + DTrFailureDate1 + "', '" + Description + "', ";
                strQry += "'" + DtcCode + "','" + DtrCode + "',  '" + Convert.ToString(sbQry) + "' , '" + objFailDate.TicketNumber + "') ";
                ObjCon.ExecuteQry(strQry);

                Arr[0] = "Updated Successfully ";
                Arr[1] = "0";
                ObjCon.CommitTransaction();

                return Arr;
            }

            catch (Exception ex)
            {
                ObjCon.RollBackTrans();
                clsException.LogError(                             MethodBase.GetCurrentMethod().DeclaringType.Name,                             MethodBase.GetCurrentMethod().Name,                             ex.Message,                             ex.StackTrace);
                return Arr;
            }
        }
    }
}
