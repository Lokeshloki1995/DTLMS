using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using IIITS.PGSQL.DAL;
using System.Configuration;

namespace IIITS.DTLMS.BL
{
    public class ClsExcelUpload
    {

        string strFormCode = "ClsExcelUpload";
        public string sExcelId { get; set; }
        

        public static PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
        public string  ExecuteUploadExecl(DataTable dtExcelData,string Userid)
        {
            string Result = string.Empty;

            try
            {
                String sQry = string.Empty;
                String sQryfetch = string.Empty;
                String sQrylog = string.Empty;
               
                if (dtExcelData.Rows.Count > 0)
                {

                    for (int i = 0; i < dtExcelData.Rows.Count; i++)
                    {
                       // string ExcelLogid = Convert.ToString(objcon.Get_max_no("EU_ID", "TBLEXCELUPLOAD"));
                       string kwhreading=Convert.ToString(dtExcelData.Rows[i]["DT_KWH_READING"]);
                       string Dtccode = Convert.ToString(dtExcelData.Rows[i]["DT_CODE"]);
                       if (Dtccode != "")
                       {
                           if (kwhreading != "")
                           {
                               string OldReading = objcon.get_value("SELECT \"DT_KWH_READING\" FROM \"TBLDTCMAST\" WHERE  \"DT_CODE\"='" + Convert.ToString(dtExcelData.Rows[i]["DT_CODE"]) + "'");

                               sQrylog = "INSERT INTO \"TBLEXCELUPLOAD\" (\"EU_DTC_CODE\",\"EU_OLD_KWH_READING\",\"EU_NEW_KWH_READING\",\"EU_CRBY\",\"EU_CRON\",\"EU_AVG_LOAD\",\"EU_PEAK_LOAD\",\"EU_SURPLUS_CAP\")";
                               sQrylog += "values ('" + Convert.ToString(dtExcelData.Rows[i]["DT_CODE"]) + "','" + OldReading + "',";
                               if (Convert.ToString(dtExcelData.Rows[i]["DT_KWH_READING"]) != "")
                               {
                                   sQrylog += "'" + Convert.ToString(dtExcelData.Rows[i]["DT_KWH_READING"]) + "','" + Userid + "',";
                               }
                               else
                               {
                                   sQrylog += "'0','" + Userid + "',";
                               }
                               if (Convert.ToString(dtExcelData.Rows[i]["DT_AVERAGE_LOAD(in KVA)"]) != "")
                               {
                                   sQrylog += "Now(),'" + Convert.ToString(dtExcelData.Rows[i]["DT_AVERAGE_LOAD(in KVA)"]) + "',";
                               }
                               else
                               {
                                   sQrylog += "Now(),'0',";
                               }
                               if (Convert.ToString(dtExcelData.Rows[i]["DT_PEAK_LOAD(in KVA)"]) != "")
                               {
                                   sQrylog += "'" + Convert.ToString(dtExcelData.Rows[i]["DT_PEAK_LOAD(in KVA)"]) + "',";
                               }
                               else
                               {
                                   sQrylog += "'0',";
                               }
                               if (Convert.ToString(dtExcelData.Rows[i]["DT_SURPLUS_CAPACITY(in KVA)"]) != "")
                               {
                                   sQrylog += " '" + Convert.ToString(dtExcelData.Rows[i]["DT_SURPLUS_CAPACITY(in KVA)"]) + "')";
                               }
                               else
                               {
                                   sQrylog += " '0')";
                               }
                               objcon.ExecuteQry(sQrylog);



                               sQry = "UPDATE \"TBLDTCMAST\" SET";
                               if (Convert.ToString(dtExcelData.Rows[i]["DT_KWH_READING"]) != "")
                               {
                                   sQry += " \"DT_KWH_READING\"='" + Convert.ToString(dtExcelData.Rows[i]["DT_KWH_READING"]) + "' ";
                               }
                               else
                               {
                                   sQry += " \"DT_KWH_READING\"='0'";
                               }

                               if (Convert.ToString(dtExcelData.Rows[i]["DT_AVERAGE_LOAD(in KVA)"]) != "")
                               {
                                   sQry += ",\"DT_AVG_LOAD\"='" + Convert.ToString(dtExcelData.Rows[i]["DT_AVERAGE_LOAD(in KVA)"]) + "'";
                               }

                               if (Convert.ToString(dtExcelData.Rows[i]["DT_PEAK_LOAD(in KVA)"]) != "")
                               {
                                   sQry += ",\"DT_PEAK_LOAD\"='" + Convert.ToString(dtExcelData.Rows[i]["DT_PEAK_LOAD(in KVA)"]) + "'";
                               }
                               if (Convert.ToString(dtExcelData.Rows[i]["DT_SURPLUS_CAPACITY(in KVA)"]) != "")
                               {
                                   sQry += " , \"DT_SURPLUS_CAP\"='" + Convert.ToString(dtExcelData.Rows[i]["DT_SURPLUS_CAPACITY(in KVA)"]) + "'";
                               }
                                sQry+=" WHERE \"DT_CODE\"='" + Convert.ToString(dtExcelData.Rows[i]["DT_CODE"]) + "' ";
                               objcon.ExecuteQry(sQry);
                           }
                       }
                                           
                    }

                }
                Result = "1";
                return Result;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                Result = "2";
                return Result;
              
            }
        }

        
    }
}
