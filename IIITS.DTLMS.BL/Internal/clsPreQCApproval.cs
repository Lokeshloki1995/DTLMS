using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using IIITS.PGSQL.DAL;
using Npgsql;
using System.Configuration;
using System.Collections;

namespace IIITS.DTLMS.BL
{
    public class clsPreQCApproval
    {
        public string sFormCode = "clsPreQCApproval";
        public string GetApprovalLevel(string sUsertype)
        {
            PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
            try
            {
                string sQry = string.Empty;
                sQry = "SELECT CAST(\"WM_LEVEL\" AS TEXT) || '~' ||CAST(\"WM_LEVEL\" + 1 AS TEXT) FROM \"TBLWORKFLOWMASTER\" WHERE  \"WM_APPTYPE\" = 2 AND \"WM_ROLEID\" = '" + sUsertype + "'  ";
                //sQry += " AND \"WM_LEVEL\" NOT IN (SELECT MAX(\"WM_LEVEL\") FROM \"TBLWORKFLOWMASTER\")";
                string sLevel = objcon.get_value(sQry);
                //if(sLevel==null)
                //{
                //    sLevel = "5";
                //}
                return sLevel;
            }
            catch(Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, sFormCode, "GetEnumearionDetails");
                return "0";
            }
        }

        public string GetNextApprovalLevel(string sUsertype)
        {
            PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
            try
            {
                string sQry = string.Empty;
                sQry = "SELECT MIN(\"WM_LEVEL\") FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_APPTYPE\" = 2 AND  \"WM_LEVEL\" > (SELECT \"WM_LEVEL\" FROM \"TBLWORKFLOWMASTER\" WHERE cast(\"WM_ROLEID\" as text) = '" + sUsertype + "' AND  \"WM_APPTYPE\" = 2 )";
                string sLevel = objcon.get_value(sQry);
                if(sLevel=="")
                {
                    sLevel = "5";
                }
                return sLevel;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, sFormCode, "GetEnumearionDetails");
                return "0";
            }
        }

        public string GetCurrentApprover(string sLevel)
        {
            PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
            try
            {
                string sQry = string.Empty;
                int PreLevel = Convert.ToInt32(sLevel) - 1;
                sQry = " SELECT string_agg(CAST(\"WM_ROLEID\" AS TEXT),',') FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_APPTYPE\" = 2 AND  (\"WM_LEVEL\" = '" + sLevel + "' OR \"WM_LEVEL\" = '" + PreLevel + "') ";
                string sApprover = objcon.get_value(sQry);
                return sApprover;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, sFormCode, "GetEnumearionDetails");
                return "0";
            }
        }


        public string[] GetEnumerationInfoForApprove(string sUsertype, ArrayList sEnumIdList,string sUserId)
        {
            string[] Arr = new string[1];
            string sQry = string.Empty;
            DataTable dt = new DataTable();
            PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
            try
            {
                string sEnumId = string.Empty;
                for (int i = 0; i < sEnumIdList.Count; i++)
                {
                    if(sEnumId.Length == 0)
                    {
                        sEnumId = sEnumIdList[i].ToString();
                    }
                    else
                    {
                        sEnumId = sEnumId + "," + sEnumIdList[i] ;
                    }
                    sQry = "INSERT INTO \"TBLINTERNALAPPROVALHISTORY\" (\"IA_ED_ID\",\"IA_ED_APPROVED_BY\",\"IA_ED_APPROVED_TYPE\") ";
                    sQry += " VALUES('" + sEnumIdList[i] + "','" + sUserId + "','1')";
                    ObjCon.ExecuteQry(sQry);
                }
                
                sQry = "UPDATE \"TBLENUMERATIONDETAILS\" SET \"ED_APPROVALPRIORITY\" = '" + GetNextApprovalLevel(sUsertype) + "' ";
                sQry += " WHERE \"ED_ID\" IN ( " + sEnumId + " )";
                ObjCon.ExecuteQry(sQry);
                Arr[0] = "Approved Successfully";
                return Arr;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, sFormCode, "GetEnumerationInfoForApprove");
                return Arr;
            }
        }

        public bool GetReworkRecord(string sED_ID)
        {
            bool res = false;
            string sQry = string.Empty;
            DataTable dtED_ID = new DataTable();
            PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
            try
            {
                sQry = "SELECT \"ER_ED_ID\"  FROM \"TBLENUMERATIONREMARKS\" WHERE  \"ER_STATUS\" = 0 AND \"ER_REWORK\" = 1 AND \"ER_ED_ID\" IN ("+ sED_ID + ") ";
                dtED_ID = ObjCon.FetchDataTable(sQry);
                if(dtED_ID.Rows.Count > 0)
                {
                    return res;
                }
                else
                {
                    res = true;
                    return res;
                }               
            }
            catch(Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, sFormCode, "GetReworkRecord");
                return res;
            }
        }
        
    }
}
