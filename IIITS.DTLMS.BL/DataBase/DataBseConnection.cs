using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIITS.DTLMS.BL.DataBase
{
    public class DataBseConnection
    {
        NpgsqlConnection con = new NpgsqlConnection();
        NpgsqlTransaction trans;
        bool blTransaction = false;
        public DataBseConnection(string Password)
        {
            try
            {
                string PassWord = string.Empty;
                string strConnectionString = ConfigurationManager.ConnectionStrings["PgSql"].ConnectionString + Decrypt(Password);
                con.ConnectionString = strConnectionString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public NpgsqlTransaction BeginTransaction()
        {
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            if (con.State == ConnectionState.Open)
            {
                trans = con.BeginTransaction(IsolationLevel.ReadCommitted);
                blTransaction = true;
            }
            return trans;
        }
        public void CommitTransaction()
        {
            if (con.State == ConnectionState.Open)
            {
                if (blTransaction == true)
                    trans.Commit();
                con.Close();
            }
        }
        public void RollBackTrans()
        {
            if (con.State == ConnectionState.Open)
            {
                if (blTransaction == true)
                    trans.Rollback();
                con.Close();
            }
        }
        public long Get_max_no(string Col_name, string Tab_name)
        {
            try
            {
                NpgsqlDataReader reader;
                long lngrReturn = 0;
                string strQry = "SELECT MAX(\"" + Col_name + "\") FROM \"" + Tab_name + "\"";
                reader = Fetch(strQry);
                if (reader.Read())
                {
                    if (reader.GetValue(0).ToString() == "")
                    {
                        reader.Close();
                        if (blTransaction == false)
                        {
                            con.Close();
                        }

                        return (1);
                    }
                    else
                    {
                        lngrReturn = (Convert.ToInt64(reader.GetValue(0)) + 1);
                        reader.Close();
                        if (blTransaction == false)
                        {
                            con.Close();
                        }
                        return lngrReturn;
                    }

                }
                else
                {
                    return (-1);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int ExecuteQry(string sSql)
        {
            try
            {
                int Result = 0;
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSql;

                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                cmd.Connection = con;
                Result = cmd.ExecuteNonQuery();
                if (blTransaction == false)
                {
                    con.Close();
                }
                return Result;
            }
            catch (Exception ex)
            {
                if (blTransaction == false)
                {
                    con.Close();
                }
                throw ex;
            }
        }
        /// <summary>
        /// this was created newly.
        /// </summary>
        /// <param name="Qry"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public int ExecuteQry(string Qry, NpgsqlCommand cmd)
        {
            int n = 0;
            try
            {
                if (this.con.State != ConnectionState.Open)
                    this.con.Open();
                if (this.blTransaction)
                    cmd.Transaction = this.trans;
                cmd.Connection = this.con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = Qry;
                n = cmd.ExecuteNonQuery();
                if (this.blTransaction)
                    return n;
                if (blTransaction == false)
                {
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }            
            return n;
        }

        /// <summary>
        /// this was created newly.
        /// </summary>
        /// <param name="strQry"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public string get_value(string Qry, NpgsqlCommand cmd)
        {
            string ResultQry = string.Empty;
            try
            {
                if (this.con.State != ConnectionState.Open)
                    this.con.Open();
                string empty = string.Empty;
                cmd.Connection = this.con;
                cmd.CommandText = Qry;
                ResultQry = Convert.ToString(cmd.ExecuteScalar());
                if (blTransaction == false)
                {
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ResultQry;
        }
        /// <summary>
        /// newly Created
        /// </summary>
        /// <param name="Qry"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public DataTable FetchDataTable(string Qry, NpgsqlCommand cmd)
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable();
            try
            {
                if (this.con.State != ConnectionState.Open)
                    this.con.Open();
                NpgsqlDataAdapter npgsqlDataAdapter = new NpgsqlDataAdapter();
                cmd.CommandText = Qry;
                cmd.Connection = this.con;
                npgsqlDataAdapter.SelectCommand = cmd;
                npgsqlDataAdapter.Fill(dataSet);
                if (dataSet.Tables[0].Rows.Count > 0)
                    dataTable = dataSet.Tables[0];
                if (blTransaction == false)
                {
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dataTable;
        }

        public string get_value(string strQry)
        {
            try
            {
                NpgsqlDataReader oblReader;
                string strResult = string.Empty;
                oblReader = Fetch(strQry);
                if (oblReader.Read())
                {
                    strResult = oblReader.GetValue(0).ToString();
                }
                oblReader.Close();
                if (blTransaction == false)
                {
                    con.Close();
                }
                return (strResult);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public NpgsqlDataReader Fetch(string Qry)
        {
            try
            {
                NpgsqlDataReader reader = null;
                NpgsqlCommand comm = new NpgsqlCommand();

                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                comm.Connection = con;
                comm.CommandText = Qry;
                reader = comm.ExecuteReader(System.Data.CommandBehavior.Default);
                return (reader);
            }
            catch (Exception ex)
            {
                if (blTransaction == false)
                {
                    con.Close();
                }
                throw ex;
            }
        }
        public string Encrypt(string pwd)
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[pwd.Length];
            encode = Encoding.UTF8.GetBytes(pwd);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;
        }

        public string Decrypt(string pwd)
        {
            string decryptpwd = string.Empty;
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder Decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(pwd);
            int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            decryptpwd = new String(decoded_char);
            return decryptpwd;
        }

        public DataSet FetchDataSet(NpgsqlCommand cmd)
        {
            NpgsqlDataReader dreader = null;

            try
            {
                DataTable dtable = new DataTable();
                DataSet dSet = new DataSet();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                con.Close();
                return dSet;
            }
            catch (Exception ex)
            {
                con.Close();
                dreader.Close();
                throw ex;
            }
        }
        public DataSet FetchDataSetCursor(NpgsqlCommand cmd)
        {
            NpgsqlDataReader dreader = null;
            NpgsqlTransaction tran = null;
            try
            {
                DataTable dtable = new DataTable();
                DataTable dtable1 = new DataTable();
                DataSet dSet = new DataSet();
                cmd.Connection = con;
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                tran = con.BeginTransaction();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                dreader = cmd.ExecuteReader();
                dtable.Load(dreader);
                dreader.Close();
                for (int j = 0; j < dtable.Rows.Count; j++)
                {
                    dtable1 = new DataTable("table" + j);
                    cmd = new NpgsqlCommand("FETCH ALL IN " + "\"" + dtable.Rows[j][0].ToString() + "\"", con); //use plpgsql fetch command to get data back
                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                    da.Fill(dtable1);
                    dSet.Tables.Add(dtable1);
                }
                tran.Commit();
                con.Close();
                return dSet;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                dreader.Close();
                con.Close();
                throw ex;
            }
        }

        public DataSet FetchDataSet(string strQry)
        {
            try
            {
                DataSet dSet = new DataSet();
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(strQry, con);
                adapter.Fill(dSet);
                return dSet;
            }
            catch (Exception ex)
            {
                if (blTransaction == false)
                {
                    con.Close();
                }
                throw ex;
            }
        }

        public DataTable FetchDataTable(NpgsqlCommand cmd)
        {
            NpgsqlDataReader dreader = null;
            try
            {
                DataTable dtable = new DataTable();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                dreader = cmd.ExecuteReader();
                dtable.Load(dreader);

                dreader.Close();
                if (blTransaction == false)
                {
                    con.Close();
                }
                return dtable;
            }
            catch (Exception ex)
            {
                if (blTransaction == false)
                {
                    con.Close();
                }
                dreader.Close();
                throw ex;
            }
        }
        public DataTable FetchDataTable(string strQry)
        {
            NpgsqlDataReader dreader = null;

            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand();
                DataTable dtable = new DataTable();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = strQry;

                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                cmd.Connection = con;
                dreader = cmd.ExecuteReader();
                dtable.Load(dreader);
                dreader.Close();
                if (blTransaction == false)
                {
                    con.Close();
                }
                return dtable;
            }
            catch (Exception ex)
            {
                if (blTransaction == false)
                {
                    con.Close();
                }
                dreader.Close();
                throw ex;
            }
        }



        public string[] Execute(NpgsqlCommand cmd, string[] strArray, int n)
        {
            try
            {
                string[] strResult = new string[n];
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;

                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                cmd.ExecuteNonQuery();
                for (int i = 0; i < n; i++)
                {
                    strResult[i] = Convert.ToString(cmd.Parameters[strArray[i]].Value);
                }
                if (blTransaction == false)
                {
                    con.Close();
                }
                return strResult;
            }
            catch (Exception ex)
            {
                if (blTransaction == false)
                {
                    con.Close();
                }
                throw ex;
            }

        }
        public string StringGetValue(NpgsqlCommand cmd)
        {
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                return Convert.ToString(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                if (!blTransaction)
                {
                    con.Close();
                }

                throw ex;
            }
        }



    }
}

