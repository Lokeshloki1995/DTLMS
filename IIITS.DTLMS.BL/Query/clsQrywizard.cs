using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IIITS.DAL;
using IIITS.PGSQL.DAL;
using System.Configuration;
using Npgsql;

namespace IIITS.DTLMS.BL
{
    public class clsQrywizard
    {
        //CustOledbConnection objCon = new CustOledbConnection(Constants.Password);
        PGSqlConnection objCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
        NpgsqlCommand cmd = new NpgsqlCommand();
        NpgsqlConnection con = new NpgsqlConnection();

        public DataTable GetResult(string strRequestQry, string strUserid, string status)
        {
            string strResult = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                objCon.BeginTransaction();
                string strQuery = string.Empty;

                strQuery = "INSERT INTO \"TBLQUERYLOG\"(\"QL_ID\",\"QL_TEXT\",\"QL_ENTRYAUTH\",\"QL_EXCECUTED_IN_FORM\",\"QL_DESCRIPTION\") VALUES ";
                strQuery += " ('" + objCon.Get_max_no("QL_ID", "TBLQUERYLOG") + "','" + strRequestQry.Replace("'", "''") + "','" + strUserid + "','S','SUCCEED')";
                objCon.ExecuteQry(strQuery);
                //  DatatoQueryLog(strRequestQry, strUserid, "S", "SUCCEED");

                if (strRequestQry.ToUpper().Contains("SELECT"))
                { 
                dt = objCon.FetchDataTable(strRequestQry.ToUpper());
                }
               else if (strRequestQry.ToUpper().Contains("UPDATE")|| strRequestQry.ToUpper().Contains("INSERT")|| strRequestQry.ToUpper().Contains("DELETE") )
                {
                    if(status=="C")
                    {
                        objCon.CommitTransaction();
                        dt.Columns.Clear();
                        dt.Columns.Add("Information");
                        dt.Rows.Add("Committed Successfully");
                    }
                    else
                    {
                        objCon.ExecuteQry(strRequestQry.ToUpper());
                        dt.Columns.Clear();
                        dt.Columns.Add("Information");
                        dt.Rows.Add("Successfull");
                    }                                       
                    
                }
                return dt;
            }
            catch (Exception ex)
            {
                objCon.RollBackTrans();
                dt.Columns.Add("Error");
                dt.Rows.Add(ex.Message);
                return dt;
            }
            finally
            {
                objCon.close();
               
            }
        } 

        //modified by Shridhar st
        public DataTable getUpdateResult(string strReqQry,string strUserId,string status,string TicketId, string Remarks)
        {
            string strResult = string.Empty;
            DataTable dt = new DataTable();
            string UpdateLimit = ConfigurationSettings.AppSettings["UpdateLimit"];
            try
            {
                //if (status != "COMMIT")
                //{
                //    DatatoQueryLog(strReqQry, strUserId, "U-P", "SUCCEED");
                //}

                objCon.BeginTransaction();

                long max = objCon.Get_max_no("QL_ID", "TBLQUERYLOG");
                //string strQry = string.Empty;
                //strQry = "INSERT INTO \"TBLQUERYLOG\"(\"QL_ID\",\"QL_TEXT\",\"QL_ENTRYAUTH\",\"QL_EXCECUTED_IN_FORM\",\"QL_DESCRIPTION\") VALUES ";
                //strQry += " ('" + max + "','" + strReqQry.Replace("'", "''") + "','" + strUserId + "','U','SUCCEED')";
                //objCon.ExecuteQry(strQry);


                if (strReqQry.ToUpper().Contains("UPDATE") || strReqQry.ToUpper().Contains("INSERT") || strReqQry.ToUpper().Contains("DELETE"))
                {
                    if (status == "COMMIT")
                    {
                        //  DatatoQueryLog(strReqQry, strUserId, "U-C", "SUCCEED");

                        string strQry = string.Empty;
                        strQry = "INSERT INTO \"TBLQUERYLOG\"(\"QL_ID\",\"QL_TEXT\",\"QL_ENTRYAUTH\",\"QL_EXCECUTED_IN_FORM\",\"QL_DESCRIPTION\",\"QL_TICKET_NO\",\"QL_REMARKS\") VALUES ";
                        strQry += " ('" + max + "','" + strReqQry.Replace("'", "''") + "','" + strUserId + "','U','SUCCEED','" + TicketId + "','" + Remarks + "')";
                        objCon.ExecuteQry(strQry);

                        int n = objCon.ExecuteQry(strReqQry);
                        if (n <= Convert.ToInt16(UpdateLimit))
                        {
                            strReqQry = "update \"TBLQUERYLOG\"  set \"QL_AFFECTED_ROWS\"=" + n + "   WHERE \"QL_ID\"='" + max + "'";
                            objCon.ExecuteQry(strReqQry);

                            objCon.CommitTransaction();
                            dt.Clear();
                            dt.Columns.Add("Information");
                            dt.Rows.Add(n + " : Rows Affected");
                            dt.Rows.Add("Committed Successfully!");
                        }
                        else
                        {
                            dt.Clear();
                            dt.Columns.Add("Information");
                            dt.Rows.Add("An Updation/Deletion/Inserting is not Possible because Exceed the limit of " + Convert.ToInt16(UpdateLimit) + " Affected Rows.!");
                            //   DatatoQueryLog(strReqQry, strUserId, "U", "An Updation/Deletion/Inserting is not Possible because Exceed the limit of " + Convert.ToInt16(UpdateLimit) + " Affected Rows.!");
                            string strQuery = "INSERT INTO \"TBLQUERYLOG\"(\"QL_ID\",\"QL_TEXT\",\"QL_ENTRYAUTH\",\"QL_EXCECUTED_IN_FORM\",\"QL_DESCRIPTION\",\"QL_TICKET_NO\",\"QL_REMARKS\") VALUES ";
                            strQuery += " ('" + objCon.Get_max_no("QL_ID", "TBLQUERYLOG") + "','" + strQry.Replace("'", "''") + "','" + strUserId + "','U','An Updation/Deletion/Inserting is not Possible because Exceed the limit of " + Convert.ToInt16(UpdateLimit) + " Affected Rows.!','" + TicketId + "','" + Remarks + "')";
                            objCon.ExecuteQry(strQuery);
                        }

                    }
                    else
                    {
                        //int n = objCon.ExecuteQry(strReqQry);
                        // objCon.BeginTransaction();
                        // int n = objCon.ExecuteQry(strReqQry, cmd);
                        //  int n = npgsqlCommand.ExecuteNonQuery(); (strReqQry, cmd);

                        //cmd.CommandType = CommandType.Text;
                        //cmd.CommandText = strReqQry;
                        //if (this.con.State != ConnectionState.Open)
                        //   con.Open();
                        //cmd.Connection = this.con;
                        //int n = cmd.ExecuteNonQuery();
                        //  PGSqlConnection PG = new PGSqlConnection();

                        //con.Open();

                        PGSqlConnectioncls pgrsconnew = new PGSqlConnectioncls(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
                        int n = pgrsconnew.ExecuteQrynon(strReqQry);


                        dt.Clear();

                        if (n <= Convert.ToInt16(UpdateLimit))
                        {
                           string strReqQrys = " update 	\"TBLQUERYLOG\"  set \"QL_AFFECTED_ROWS\"="+ n + "   WHERE \"QL_ID\"='" + max + "'";
                            objCon.ExecuteQry(strReqQrys);
                            dt.Columns.Add(n + " : Rows Will be Affected (Please Check the Query and Proceed)");
                            dt.Rows.Add(strReqQry);
                        }
                        else
                        {
                            objCon.RollBackTrans();
                            dt.Clear();
                            dt.Columns.Add("Information");
                            dt.Rows.Add("An Updation/Deletion/Inserting is not Possible because Exceed the limit of " + Convert.ToInt16(UpdateLimit) + " Affected Rows.!");
                            // DatatoQueryLog(strReqQry, strUserId, "U", "An Updation/Deletion/Inserting is not Possible because Exceed the limit of " + Convert.ToInt16(UpdateLimit) + " Affected Rows.!");
                            string strQuerys = "INSERT INTO \"TBLQUERYLOG\"(\"QL_ID\",\"QL_TEXT\",\"QL_ENTRYAUTH\",\"QL_EXCECUTED_IN_FORM\",\"QL_DESCRIPTION\",\"QL_TICKET_NO\",\"QL_REMARKS\") VALUES ";
                            strQuerys += " ('" + objCon.Get_max_no("QL_ID", "TBLQUERYLOG") + "','" + strReqQry.Replace("'", "''") + "','" + strUserId + "','U','An Updation/Deletion/Inserting is not Possible because Exceed the limit of " + Convert.ToInt16(UpdateLimit) + " Affected Rows.!','" + TicketId + "','" + Remarks + "')";
                            objCon.ExecuteQry(strQuerys);
                        }
                    }
                }
                return dt;
            }
            catch(Exception ex)
            {
                dt.Columns.Add("ERROR!");
                dt.Rows.Add(ex.Message);
                objCon.RollBackTrans();
                string strQuery = "INSERT INTO \"TBLQUERYLOG\"(\"QL_ID\",\"QL_TEXT\",\"QL_ENTRYAUTH\",\"QL_EXCECUTED_IN_FORM\",\"QL_DESCRIPTION\",\"QL_TICKET_NO\",\"QL_REMARKS\") VALUES ";
                strQuery += " ('" + objCon.Get_max_no("QL_ID", "TBLQUERYLOG") + "','" + strReqQry.Replace("'", "''") + "','" + strUserId + "','U','" + ex.Message + "','" + TicketId + "','" + Remarks + "')";
                objCon.ExecuteQry(strQuery);
                return dt;
            }
            finally
            {
                objCon.close();
            }
        }
        public void DatatoQueryLog(string strReqQrys, string strUserId, string sts, string description, string TicketId, string Remarks)
        {
            string strQuery = string.Empty;

            strQuery = "INSERT INTO \"TBLQUERYLOG\"(\"QL_ID\",\"QL_TEXT\",\"QL_ENTRYAUTH\",\"QL_EXCECUTED_IN_FORM\",\"QL_DESCRIPTION\",\"QL_TICKET_NO\",\"QL_REMARKS\") VALUES ";
            strQuery += " ('" + objCon.Get_max_no("QL_ID", "TBLQUERYLOG") + "','" + strReqQrys.Replace("'", "''") + "','" + strUserId + "','" + sts + "','" + description + "','" + TicketId + "','" + Remarks + "')";
            objCon.ExecuteQry(strQuery);
        }
        public DataTable getRollbackResult(String status)
        {
            DataTable dt = new DataTable();
            try
            {
                objCon.BeginTransaction();
                objCon.RollBackTrans();
                dt.Columns.Clear();
                dt.Columns.Add("Information");
                dt.Rows.Add("Rollback done Successfully!");
                return dt;
            }
            catch (Exception ex)
            {
                dt.Columns.Add("ERROR!");
                dt.Rows.Add(ex.Message);
                // string strQuery = "INSERT INTO \"TBLQUERYLOG\"(\"QL_ID\",\"QL_TEXT\",\"QL_ENTRYAUTH\",\"QL_EXCECUTED_IN_FORM\",\"QL_DESCRIPTION\") VALUES ";
                //strQuery += " ('" + objCon.Get_max_no("QL_ID", "TBLQUERYLOG") + "','" + strReqQry.Replace("'", "''") + "','" + strUserId + "','U','" + ex.Message + "')";
                //objCon.ExecuteQry(strQuery);
                return dt;
            }

        }
     
    }
}
