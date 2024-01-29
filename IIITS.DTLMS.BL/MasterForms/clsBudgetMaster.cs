using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Configuration;
using System.Data.SQLite;
using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;

namespace IIITS.DTLMS.BL
{
    public class clsBudgetMaster
    {
        string strFormCode = "clsBudgetMaster";

        string strQry = string.Empty;
        public string budgetno { get; set; }
        public string budgetdate { get; set; }
        public string budgetamount { get; set; }
        public string budgetaccCode { get; set; }
        public string budgetDivcode { get; set; }
        public string budgetFinyear { get; set; }
        public string budgetFinstart { get; set; }
        public string budgetFinEnd { get; set; }

        public string budgetobamnt { get; set; }
        public string budgetobdate { get; set; }

        public Int64 budgetId { get; set; }
        PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));

        NpgsqlCommand NpgsqlCommand;
        NpgsqlCommand cmd;

        public string[] Save_updatebudgetmast(clsBudgetMaster objbudgetmast)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string[] ArrResult = new string[2];
            try
            {

                //if (objbudgetmast.budgetId == 0)
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("BudgetNo", objbudgetmast.budgetno);
                //    string strAccCode = ObjCon.get_value("select \"BM_ACC_CODE\" FROM \"TBLBUDGETMAST\" WHERE \"BM_NO\"=:BudgetNo", NpgsqlCommand);
                //    if (strAccCode == objbudgetmast.budgetaccCode)
                //    {
                //        ArrResult[0] = "Account Code  Already Present to  budget no....!!";
                //        ArrResult[1] = "1";
                //        return ArrResult;
                //    }
                //    NpgsqlCommand.Parameters.AddWithValue("BudgetNo1", objbudgetmast.budgetno);
                //    NpgsqlCommand.Parameters.AddWithValue("budgetdate", objbudgetmast.budgetdate);
                //    NpgsqlCommand.Parameters.AddWithValue("budgetamount", Convert.ToInt32(objbudgetmast.budgetamount));
                //    NpgsqlCommand.Parameters.AddWithValue("budgetaccCode", objbudgetmast.budgetaccCode);
                //    NpgsqlCommand.Parameters.AddWithValue("budgetDivcode", Convert.ToInt32(objbudgetmast.budgetDivcode));
                //    NpgsqlCommand.Parameters.AddWithValue("budgetFinyear", objbudgetmast.budgetFinyear);
                //    NpgsqlCommand.Parameters.AddWithValue("budgetobamnt", Convert.ToInt32(objbudgetmast.budgetobamnt));
                //    NpgsqlCommand.Parameters.AddWithValue("budgetobdate", objbudgetmast.budgetobdate);

                //    strQry = "INSERT INTO \"TBLBUDGETMAST\" (\"BM_NO\",\"BM_DATE\",\"BM_AMOUNT\",\"BM_ACC_CODE\",\"BM_DIV_CODE\",\"BM_CRON\",\"BM_FIN_YEAR\",\"BM_OB_AMNT\",\"BM_OB_DATE\")";
                //    strQry += " VALUES(:BudgetNo1,TO_DATE(:budgetdate,'dd/MM/yyyy'),:budgetamount,:budgetaccCode,";
                //    strQry += ":budgetDivcode,NOW(),:budgetFinyear,:budgetobamnt,TO_DATE(:budgetobdate,'dd/MM/yyyy'))";
                //    ObjCon.ExecuteQry(strQry, NpgsqlCommand);

                //    ArrResult[0] = "Budget Detail Saved Successfully ";
                //    ArrResult[1] = "0";
                //    return ArrResult;
                //}
                //else
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("budgetamount1", Convert.ToInt32(objbudgetmast.budgetamount));
                //    NpgsqlCommand.Parameters.AddWithValue("budgetFinyear1", objbudgetmast.budgetFinyear);
                //    NpgsqlCommand.Parameters.AddWithValue("budgetdate1", objbudgetmast.budgetdate);
                //    NpgsqlCommand.Parameters.AddWithValue("budgetaccCode1", objbudgetmast.budgetaccCode);
                //    NpgsqlCommand.Parameters.AddWithValue("budgetId", Convert.ToInt32(objbudgetmast.budgetId));

                //    strQry = "UPDATE \"TBLBUDGETMAST\" SET \"BM_AMOUNT\"=:budgetamount1,\"BM_FIN_YEAR\"=:budgetFinyear1,";
                //    strQry += "\"BM_CRON\"=NOW(),\"BM_DATE\"=TO_DATE(:budgetdate1,'dd/MM/yyyy'),\"BM_ACC_CODE\"=:budgetaccCode1";
                //    strQry += " where \"BM_ID\" =:budgetId";
                //    ObjCon.ExecuteQry(strQry, NpgsqlCommand);
                //    ArrResult[0] = "Budget Detail Updated Successfully ";
                //    ArrResult[1] = "2";
                //    return ArrResult;

                //}


                // save sp
                if (objbudgetmast.budgetId == 0)
                {
                    NpgsqlCommand.Parameters.AddWithValue("BudgetNo", objbudgetmast.budgetno);
                    string strAccCode = ObjCon.get_value("select \"BM_ACC_CODE\" FROM \"TBLBUDGETMAST\" WHERE \"BM_NO\"=:BudgetNo", NpgsqlCommand);
                    if (strAccCode == objbudgetmast.budgetaccCode)
                    {
                        ArrResult[0] = "Account Code  Already Present to  budget no....!!";
                        ArrResult[1] = "1";
                        return ArrResult;
                    }

                    cmd = new NpgsqlCommand("proc_saveupdate_budgetmast");
                    cmd.Parameters.AddWithValue("bm_id", Convert.ToString(objbudgetmast.budgetId));
                    cmd.Parameters.AddWithValue("budgetno1", objbudgetmast.budgetno);
                    cmd.Parameters.AddWithValue("budgetdate", objbudgetmast.budgetdate);
                    cmd.Parameters.AddWithValue("budgetamount", objbudgetmast.budgetamount);
                    cmd.Parameters.AddWithValue("budgetacccode", objbudgetmast.budgetaccCode);
                    cmd.Parameters.AddWithValue("budgetdivcode", objbudgetmast.budgetDivcode);
                    cmd.Parameters.AddWithValue("budgetfinyear", objbudgetmast.budgetFinyear);
                    cmd.Parameters.AddWithValue("budgetobamnt", objbudgetmast.budgetobamnt);
                    cmd.Parameters.AddWithValue("budgetobdate", objbudgetmast.budgetobdate);

                    cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                    cmd.Parameters["op_id"].Direction = ParameterDirection.Output;

                    ArrResult[0] = "msg";
                    ArrResult[1] = "op_id";
                    ArrResult = ObjCon.Execute(cmd, ArrResult, 2);
                    return ArrResult;
                }
                else
                {
                    cmd = new NpgsqlCommand("proc_saveupdate_budgetmast");

                    cmd.Parameters.AddWithValue("bm_id", Convert.ToString(objbudgetmast.budgetId));
                    cmd.Parameters.AddWithValue("budgetno1", objbudgetmast.budgetno);
                    cmd.Parameters.AddWithValue("budgetdate", objbudgetmast.budgetdate);
                    cmd.Parameters.AddWithValue("budgetamount", objbudgetmast.budgetamount);
                    cmd.Parameters.AddWithValue("budgetacccode", objbudgetmast.budgetaccCode);
                    cmd.Parameters.AddWithValue("budgetdivcode", objbudgetmast.budgetDivcode);
                    cmd.Parameters.AddWithValue("budgetfinyear", objbudgetmast.budgetFinyear);
                    cmd.Parameters.AddWithValue("budgetobamnt", objbudgetmast.budgetobamnt);
                    cmd.Parameters.AddWithValue("budgetobdate", objbudgetmast.budgetobdate);

                    cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                    cmd.Parameters["op_id"].Direction = ParameterDirection.Output;

                    ArrResult[0] = "msg";
                    ArrResult[1] = "op_id";
                    ArrResult = ObjCon.Execute(cmd, ArrResult, 2);
                    return ArrResult;

                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ArrResult;
            }
        }

        public string getdivcode(string sOfficeCode)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                
                string strQry = string.Empty;

                NpgsqlCommand.Parameters.AddWithValue("officeCode", sOfficeCode);
                strQry = "SELECT \"DIV_CODE\" FROM \"TBLDIVISION\" WHERE cast(\"DIV_CODE\" as text)=:officeCode";
                return ObjCon.get_value(strQry, NpgsqlCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }

        public DataTable LoadBudgetDetails(string sFinYear,string divcode, string strBudgetno = "")
        {
            NpgsqlCommand = new NpgsqlCommand();
            DataTable DtDetails = new DataTable();
            string strQry = string.Empty;
            try
            {

                //NpgsqlCommand.Parameters.AddWithValue("FinYr", sFinYear);
                //NpgsqlCommand.Parameters.AddWithValue("divcd", divcode);
                //strQry = "SELECT \"BM_ID\",\"BM_NO\",TO_CHAR(\"BM_DATE\",'MM/DD/YYYY') as \"BM_DATE\",\"BM_AMOUNT\",";
                //strQry += "\"SCHM_ACCCODE\" ||'~'|| \"SCHM_NAME\" AS \"BM_ACC_CODE\",\"BM_DIV_CODE\",\"DIV_NAME\",\"BM_FIN_YEAR\",\"FY_STATUS\",\"BM_OB_AMNT\",TO_CHAR(\"BM_OB_DATE\",'MM/DD/YYYY') as \"BM_OB_DATE\" ";
                //strQry += "from \"TBLBUDGETMAST\" inner join \"TBLDTCSCHEME\" on \"BM_ACC_CODE\"=cast(\"SCHM_ID\" as text) INNER JOIN ";
                //strQry += "\"TBLDIVISION\" ON \"BM_DIV_CODE\"=\"DIV_CODE\" INNER JOIN \"TBLFINANCIALYEAR\" on \"BM_FIN_YEAR\"=\"FY_YEARS\" WHERE ";
                //strQry += " cast(\"BM_FIN_YEAR\" as text) like :FinYr||'%' and cast(\"BM_DIV_CODE\" as text)=:divcd";

                //DtDetails = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
                //return DtDetails;


                // load sp
                cmd = new NpgsqlCommand("proc_load_budget_details");
                cmd.Parameters.AddWithValue("FinYr", sFinYear);
                cmd.Parameters.AddWithValue("divcd", divcode);

                DtDetails = ObjCon.FetchDataTable(cmd);
                return DtDetails;
            }

            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return DtDetails;
            }
        }

        public DataTable GetBudgetDetails(string BudgetId)
        {
            NpgsqlCommand = new NpgsqlCommand();
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            DataTable DtBudget = new DataTable();
            try
            {

                //string strQry = string.Empty;

                //NpgsqlCommand.Parameters.AddWithValue("budId", BudgetId);
                //strQry = "SELECT \"BM_NO\",TO_CHAR(\"BM_DATE\",'MM/DD/YYYY') as \"BM_DATE\",\"BM_AMOUNT\",\"BM_ACC_CODE\",\"BM_DIV_CODE\",\"BM_FIN_YEAR\",\"BM_OB_AMNT\",TO_CHAR(\"BM_OB_DATE\",'MM/DD/YYYY') as \"BM_OB_DATE\" from \"TBLBUDGETMAST\" WHERE cast( \"BM_ID\" as text)=:budId";
                //DtBudget = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
                //return DtBudget;

                // get sp
                cmd = new NpgsqlCommand("sp_get_budgetdetails");
                cmd.Parameters.AddWithValue("budId", BudgetId);
                DtBudget = ObjCon.FetchDataTable(cmd);
                return DtBudget;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return DtBudget;
            }
        }

        public DataTable ViewBudgetstatusgrid(clsBudgetMaster objbudget)
        {
            NpgsqlCommand = new NpgsqlCommand();
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            DataTable DtBudgetstatus = new DataTable();
            try
            {
                
                string strQry = string.Empty;
                 string stracccode = string.Empty;

                 NpgsqlCommand.Parameters.AddWithValue("budcode", objbudget.budgetaccCode);
                 stracccode = ObjCon.get_value("SELECT \"SCHM_ACCCODE\"  from \"TBLDTCSCHEME\" WHERE CAST(\"SCHM_ID\" AS TEXT)=:budcode", NpgsqlCommand);


                 NpgsqlCommand.Parameters.AddWithValue("accode", stracccode);
                 NpgsqlCommand.Parameters.AddWithValue("divcode", objbudget.budgetDivcode);
                 NpgsqlCommand.Parameters.AddWithValue("finyr", objbudget.budgetFinyear);
                strQry = "SELECT \"BT_ID\",\"BT_ACC_CODE\",\"WO_NO\",TO_CHAR(\"WO_DATE\",'dd-mm-yyyy') as \"WO_DATE\",\"BT_BM_AMNT\",\"BT_DEBIT_AMNT\",\"BT_AVL_AMNT\",\"BT_CREDIT_AMNT\",\"BT_CRON\",\"BT_FIN_YEAR\",\"BT_DIV_CODE\" from \"TBLBUDGETTRANS\" LEFT JOIN \"TBLWORKORDER\" ON \"BT_WO_SLNO\"=\"WO_SLNO\"";
                strQry += " WHERE cast(\"BT_ACC_CODE\" as text)=:accode and cast(\"BT_DIV_CODE\" as text)=:divcode and cast(\"BT_FIN_YEAR\" as text)=:finyr ORDER BY \"BT_ID\"";
                DtBudgetstatus = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
                return DtBudgetstatus;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return DtBudgetstatus;
            }
        }

        public string ViewBudgetstatusaval(clsBudgetMaster objbudget)
        {
            NpgsqlCommand = new NpgsqlCommand();
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            string Budgetstatusavl = string.Empty;
            try
            {
                
                string strQry = string.Empty;
                string stracccode = string.Empty;

                NpgsqlCommand.Parameters.AddWithValue("accode", objbudget.budgetaccCode);
                stracccode = ObjCon.get_value("SELECT \"SCHM_ACCCODE\"  from \"TBLDTCSCHEME\" WHERE CAST(\"SCHM_ID\" AS TEXT)=:accode", NpgsqlCommand);


                NpgsqlCommand.Parameters.AddWithValue("acccode", stracccode);
                NpgsqlCommand.Parameters.AddWithValue("divcode", objbudget.budgetDivcode);
                NpgsqlCommand.Parameters.AddWithValue("finyr", objbudget.budgetFinyear);
                strQry = "SELECT \"BT_AVL_AMNT\" from \"TBLBUDGETTRANS\" LEFT JOIN \"TBLWORKORDER\" ON \"BT_WO_SLNO\"=\"WO_SLNO\"";
                strQry += " WHERE cast(\"BT_ACC_CODE\" as text)=:acccode and cast(\"BT_DIV_CODE\" as text)=:divcode and cast( \"BT_FIN_YEAR\" as text)=:finyr ORDER BY \"BT_ID\" DESC";
                Budgetstatusavl = ObjCon.get_value(strQry, NpgsqlCommand);
                return Budgetstatusavl;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Budgetstatusavl;
            }
        } 

        public string[] ViewBudgetstatus(clsBudgetMaster objbudget)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string[] Arr = new string[4];
            try
            {
                
                NpgsqlCommand.Parameters.AddWithValue("finyr", objbudget.budgetFinyear);
                objbudget.budgetFinstart = ObjCon.get_value("SELECT \"FY_START_DATE\" FROM \"TBLFINANCIALYEAR\" WHERE \"FY_YEARS\"=:finyr", NpgsqlCommand);

                NpgsqlCommand.Parameters.AddWithValue("finyr1", objbudget.budgetFinyear);
                objbudget.budgetFinEnd = ObjCon.get_value("SELECT \"FY_END_DATE\" FROM \"TBLFINANCIALYEAR\" WHERE \"FY_YEARS\"=:finyr1", NpgsqlCommand);

                NpgsqlCommand cmd = new NpgsqlCommand("proc_viewbudgetstatus");
                cmd.Parameters.AddWithValue("bm_acccode", objbudget.budgetaccCode);
                cmd.Parameters.AddWithValue("bm_divcode", objbudget.budgetDivcode);
                cmd.Parameters.AddWithValue("bm_finyear", objbudget.budgetFinyear);
                cmd.Parameters.AddWithValue("bm_finyearstart", objbudget.budgetFinstart);
                cmd.Parameters.AddWithValue("bm_finyearend", objbudget.budgetFinEnd); 
                cmd.Parameters.Add("TOAMNT", NpgsqlDbType.Text);
                cmd.Parameters.Add("AVAMNT", NpgsqlDbType.Text);
                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd.Parameters["TOAMNT"].Direction = ParameterDirection.Output;
                cmd.Parameters["AVAMNT"].Direction = ParameterDirection.Output;
                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                cmd.Parameters["msg"].Direction = ParameterDirection.Output;

                Arr[3] = "AVAMNT";
                Arr[2] = "TOAMNT";
                Arr[1] = "op_id";
                Arr[0] = "msg";
                Arr = ObjCon.Execute(cmd, Arr, 4);

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
