using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.PGSQL.DAL;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Collections;
using System.Configuration;

namespace IIITS.DTLMS.BL
{
    public class clsAllocateRange
    {

        string StrQry = string.Empty;
        PGSqlConnection ObjCon;

        public string sMake_Id { get; set; }//sMake_Id is nothing but vendor id 
        public string sStart_Range { get; set; }
        public string sEnd_Range { get; set; }
        public string sCrby { get; set; }
        public int sQty { get; set; }
        int Circle_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Circle_code"]);
        int Division_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Division_code"]);
        int SubDiv_code = Convert.ToInt32(ConfigurationSettings.AppSettings["SubDiv_code"]);
        int Section_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Section_code"]);
        public string getmaxssplate_no()
        {
            string maxno = string.Empty;
            ObjCon = new PGSqlConnection(Constants.Password);
            try
            {
                //StrQry = "SELECT  COALESCE(MAX(MD_ID),0)+1 FROM TBLALLOCATERANGEDETAILS";
                StrQry = "SELECT  COALESCE(MAX(\"TCPM_ID\"),0)+1 FROM \"TBLTCPLATEALLOCATIONMASTER\"";
                int sMaxnum = Convert.ToInt16(ObjCon.get_value(StrQry));
                if (sMaxnum == 1)
                {
                    StrQry = "SELECT COALESCE(max(\"TC_CODE\"),0)+1 FROM \"TBLTCMASTER\"";
                    maxno = ObjCon.get_value(StrQry);
                    return maxno;
                }
                else
                {
                    // StrQry = "SELECT COALESCE(MAX(MD_END_RANGE),0)+1 FROM TBLALLOCATERANGEDETAILS";
                    StrQry = "SELECT COALESCE(MAX(\"TCPM_END_RANGE\"),0)+1 FROM \"TBLTCPLATEALLOCATIONMASTER\"";
                    maxno = ObjCon.get_value(StrQry);
                    return maxno;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

                return maxno;
            }
        }


        public string[] SaveDetails(clsAllocateRange objRange)
        {
            String[] arr = new String[2];
            string j;
            string res = string.Empty;
            int[] plateArray;
            ObjCon = new PGSqlConnection(Constants.Password);
            try
            {
                plateArray = new int[Convert.ToInt32(objRange.sQty)];
                plateArray[0] = Convert.ToInt32(objRange.sStart_Range);//store the first plate number
                for (int i = 1; i < objRange.sQty; i++)
                {
                    plateArray[i] = plateArray[i - 1] + 1;
                }
                for (int i = 0; i < objRange.sQty; i++)
                {
                    StrQry = "SELECT \"TCP_TC_CODE\" || '~' ||\"VM_NAME\" FROM \"TBLTCPLATEALLOCATION\",\"TBLTCPLATEALLOCATIONMASTER\",\"TBLVENDORMASTER\" WHERE \"TCP_TCPM_ID\"=\"TCPM_ID\" AND \"TCPM_VENDOR_ID\"=\"VM_ID\" AND \"TCP_TC_CODE\"='" + plateArray[i] + "'";
                    res = ObjCon.get_value(StrQry);
                    if (res != "")
                    {
                        arr[0] = "DTR Code " + res.Split('~').GetValue(0).ToString() + " has been allocated to " + " " + res.Split('~').GetValue(1).ToString() +" "+ "Vendor ";
                        arr[1] = "1";
                        return arr;
                    }
                }

                ObjCon.BeginTransaction();
                StrQry = "INSERT INTO \"TBLTCPLATEALLOCATIONMASTER\" VALUES((SELECT COALESCE(MAX(\"TCPM_ID\")+1,1) FROM \"TBLTCPLATEALLOCATIONMASTER\"),'" + objRange.sMake_Id + "',";
                StrQry += "'" + objRange.sStart_Range + "','" + objRange.sEnd_Range + "','','" + objRange.sCrby + "',now())";
                ObjCon.ExecuteQry(StrQry);
                j = ObjCon.get_value("SELECT MAX(\"TCPM_ID\") FROM \"TBLTCPLATEALLOCATIONMASTER\"");
                for (int i = 0; i < sQty; i++)
                {
                    StrQry = "INSERT INTO \"TBLTCPLATEALLOCATION\" VALUES((SELECT COALESCE(MAX(\"TCP_ID\")+1,1) FROM \"TBLTCPLATEALLOCATION\"),'" + j + "',";
                    StrQry += " '" + plateArray[i] + "' ,'0','0','','" + objRange.sCrby + "',now())";
                    ObjCon.ExecuteQry(StrQry);
                }
                ObjCon.CommitTransaction();
                arr[0] = "Range Set Succesfully";
                arr[1] = "0";
                return arr;
            }
            catch (Exception ex)
            {
                ObjCon.RollBackTrans();
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

                arr[0] = "Exception occured";
                arr[1] = "1";
                return arr;
            }
        }

        public DataTable GetPlateAllocatedRangeDetails(string splateNumber)
        {
            ObjCon = new PGSqlConnection(Constants.Password);
            DataTable dt = new DataTable();
            try
            {
                if (!splateNumber.Equals(""))
                {
                    StrQry = "SELECT \"TCPM_ID\",\"VM_NAME\",\"TCPM_START_RANGE\",\"TCPM_END_RANGE\",\"TCPM_CRON\" FROM \"TBLTCPLATEALLOCATIONMASTER\",";
                    StrQry += "\"TBLVENDORMASTER\",\"TBLTCPLATEALLOCATION\" WHERE \"TCPM_VENDOR_ID\" = \"VM_ID\" AND \"TCPM_ID\" = \"TCP_TCPM_ID\" ";
                    StrQry += "AND \"TCP_TC_CODE\"= '" + splateNumber + "'  ORDER BY \"TCPM_START_RANGE\"";
                    dt = ObjCon.FetchDataTable(StrQry);
                }
                else
                {
                    StrQry = "SELECT \"TCPM_ID\",\"VM_NAME\",\"TCPM_START_RANGE\",\"TCPM_END_RANGE\",\"TCPM_CRON\" FROM \"TBLTCPLATEALLOCATIONMASTER\",";
                    StrQry += "\"TBLVENDORMASTER\" WHERE \"TCPM_VENDOR_ID\" = \"VM_ID\" ORDER BY \"TCPM_START_RANGE\"";
                    dt = ObjCon.FetchDataTable(StrQry);
                }
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return dt;
            }
        }

        public string[] DeleteAllocation(string sTcpm_id)
        {
            ObjCon = new PGSqlConnection(Constants.Password);
            DataTable dt = new DataTable();
            string[] arr = new string[2];
            DateTime UserLastLogin = new DateTime(); ;
            int count = 0;
            try
            {

                StrQry = "SELECT \"TCPM_START_RANGE\",\"TCPM_END_RANGE\" FROM \"TBLTCPLATEALLOCATIONMASTER\" WHERE \"TCPM_ID\"='" + sTcpm_id + "'";
                DataTable dtRange = ObjCon.FetchDataTable(StrQry);

                StrQry = "SELECT COUNT(*)TC_COUNT FROM \"TBLDTCENUMERATION\" WHERE \"DTE_TC_CODE\" BETWEEN ";
                StrQry += "'" + dtRange.Rows[0]["TCPM_START_RANGE"].ToString() + "' AND '" + dtRange.Rows[0]["TCPM_END_RANGE"].ToString() + "'";
                string sTc_Count = ObjCon.get_value(StrQry);

                if (sTc_Count != "0")
                {
                    arr[0] = "Not able to Delete Record because (" + sTc_Count + ") SS Plate already got synced, Please contact Admin to clarify further";
                    arr[1] = "1";
                    return arr;
                }
                else
                {
                    StrQry = "SELECT \"IU_ID\",TO_CHAR(\"IU_LAST_LOGIN\",'yyyy-mm-dd hh24:mi:ss')IU_LAST_LOGIN,TO_CHAR(\"TCPM_CRON\",'yyyy-mm-dd hh24:mi:ss')TCPM_CRON FROM ";
                    StrQry += " \"TBLTCPLATEALLOCATIONMASTER\",\"TBLVENDORMASTER\",\"TBLINTERNALUSERS\" WHERE \"TCPM_VENDOR_ID\" = \"VM_ID\" AND \"VM_ID\"=\"IU_VENDOR_ID\" AND \"TCPM_ID\"='" + sTcpm_id + "'";
                    dt = ObjCon.FetchDataTable(StrQry);

                    DateTime RangeCreated = Convert.ToDateTime(dt.Rows[0]["TCPM_CRON"]);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["IU_LAST_LOGIN"].ToString() != "")
                        {
                            UserLastLogin = Convert.ToDateTime(dt.Rows[i]["IU_LAST_LOGIN"]);
                        }

                        if (UserLastLogin >= RangeCreated)
                        {
                            count++;
                            UserLastLogin = Convert.ToDateTime("01-01-0001 00:00:00");
                        }
                    }

                    if (count != 0)
                    {
                        arr[0] = "Not able to Delete Record because (" + count + ") Users had Logged in after Range allocation on ( " + dt.Rows[0]["TCPM_CRON"] + " ), Please contact Admin to clarify further";
                        arr[1] = "1";
                        return arr;
                    }
                    else
                    {
                        StrQry = "DELETE FROM \"TBLTCPLATEALLOCATION\" WHERE \"TCP_TCPM_ID\"='" + sTcpm_id + "'";
                        ObjCon.ExecuteQry(StrQry);
                        StrQry = "DELETE FROM \"TBLTCPLATEALLOCATIONMASTER\" WHERE \"TCPM_ID\"='" + sTcpm_id + "'";
                        ObjCon.ExecuteQry(StrQry);
                        arr[0] = "Record Deleted Succesfully";
                        arr[1] = "1";
                    }
                }
                return arr;
            }
            catch(Exception ex)
            {
                arr[0] = "Somthing Went Wrong";
                arr[1] = "2";
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return arr;
            }
        }
    }
}
