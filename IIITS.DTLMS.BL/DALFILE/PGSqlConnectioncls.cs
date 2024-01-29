// Decompiled with JetBrains decompiler
// Type: IIITS.PGSQL.DAL.PGSqlConnection
// Assembly: IIITS.PGSQL.DAL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DF79EE52-986D-40D5-A95E-DC8FC016A536
// Assembly location: F:\HESCOM DTLMS\WEB_APPLICATIONS\HESCOM_PROJECT_LIVE_CODE\IIITS.DTLMS\bin\IIITS.PGSQL.DAL.dll

using Npgsql;
using System;
using System.Configuration;
using System.Data;
using System.Text;

namespace IIITS.PGSQL.DAL
{
    public class PGSqlConnectioncls
    {
        private NpgsqlConnection con = new NpgsqlConnection();
        private NpgsqlTransaction trans;
        private bool blTransaction = false;

        public PGSqlConnectioncls(string Password)
        {
            try
            {
                string empty = string.Empty;
                this.con.ConnectionString = ConfigurationManager.ConnectionStrings["pgSQL"].ConnectionString + this.Decrypt(Password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Encrypt(string pwd)
        {
            string empty = string.Empty;
            byte[] numArray = new byte[pwd.Length];
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(pwd));
        }

        public string Decrypt(string pwd)
        {
            string empty = string.Empty;
            Decoder decoder = new UTF8Encoding().GetDecoder();
            byte[] bytes = Convert.FromBase64String(pwd);
            char[] chars = new char[decoder.GetCharCount(bytes, 0, bytes.Length)];
            decoder.GetChars(bytes, 0, bytes.Length, chars, 0);
            return new string(chars);
        }

        public string[] Execute(NpgsqlCommand cmd, string[] strArray, int n)
        {
            try
            {
                string[] strArray1 = new string[n];
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = this.con;
                if (this.con.State != ConnectionState.Open)
                    this.con.Open();
                cmd.ExecuteNonQuery();
                for (int index = 0; index < n; ++index)
                    strArray1[index] = Convert.ToString(cmd.Parameters[strArray[index]].Value);
                if (!this.blTransaction)
                    this.con.Close();
                return strArray1;
            }
            catch (Exception ex)
            {
                this.con.Close();
                throw ex;
            }
        }

        public DataSet FetchDataSet(NpgsqlCommand cmd)
        {
            NpgsqlDataReader npgsqlDataReader = (NpgsqlDataReader)null;
            try
            {
                DataTable dataTable = new DataTable();
                DataSet dataSet = new DataSet();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = this.con;
                if (this.con.State != ConnectionState.Open)
                    this.con.Open();
                new NpgsqlDataAdapter(cmd).Fill(dataSet);
                this.con.Close();
                return dataSet;
            }
            catch (Exception ex)
            {
                this.con.Close();
                npgsqlDataReader.Close();
                throw ex;
            }
        }

        public DataTable FetchDataTable(NpgsqlCommand cmd)
        {
            NpgsqlDataReader reader = (NpgsqlDataReader)null;
            try
            {
                DataTable dataTable = new DataTable();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = this.con;
                if (this.con.State != ConnectionState.Open)
                    this.con.Open();
                reader = cmd.ExecuteReader();
                dataTable.Load((IDataReader)reader);
                reader.Close();
                if (!this.blTransaction)
                    this.con.Close();
                return dataTable;
            }
            catch (Exception ex)
            {
                this.con.Close();
                reader.Close();
                throw ex;
            }
        }

        public DataTable FetchDataTable(string strQry, NpgsqlCommand cmd)
        {
            try
            {
                if (this.con.State != ConnectionState.Open)
                    this.con.Open();
                NpgsqlDataAdapter npgsqlDataAdapter = new NpgsqlDataAdapter();
                cmd.CommandText = strQry;
                cmd.Connection = this.con;
                npgsqlDataAdapter.SelectCommand = cmd;
                DataSet dataSet = new DataSet();
                DataTable dataTable = new DataTable();
                npgsqlDataAdapter.Fill(dataSet);
                if (dataSet.Tables[0].Rows.Count > 0)
                    dataTable = dataSet.Tables[0];
                if (!this.blTransaction)
                    this.con.Close();
                return dataTable;
            }
            catch (Exception ex)
            {
                this.con.Close();
                throw ex;
            }
        }

        public DataTable FetchDataTable(string strQry)
        {
            NpgsqlDataReader reader = (NpgsqlDataReader)null;
            try
            {
                NpgsqlCommand npgsqlCommand = new NpgsqlCommand();
                DataTable dataTable = new DataTable();
                npgsqlCommand.CommandType = CommandType.Text;
                npgsqlCommand.CommandText = strQry;
                if (this.con.State != ConnectionState.Open)
                    this.con.Open();
                npgsqlCommand.Connection = this.con;
                reader = npgsqlCommand.ExecuteReader();
                dataTable.Load((IDataReader)reader);
                reader.Close();
                if (!this.blTransaction)
                    this.con.Close();
                return dataTable;
            }
            catch (Exception ex)
            {
                this.con.Close();
                reader.Close();
                throw ex;
            }
        }

        public string StringGetValue(NpgsqlCommand cmd)
        {
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = this.con;
                if (this.con.State != ConnectionState.Open)
                    this.con.Open();
                if (!this.blTransaction)
                    this.con.Close();
                return Convert.ToString(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                this.con.Close();
                throw ex;
            }
        }

        public void close()
        {
            try
            {
                if (this.con.State != ConnectionState.Open)
                    return;
                this.con.Close();
            }
            catch (Exception ex)
            {
                this.con.Close();
                throw ex;
            }
        }

        public int ExecuteQry(string sSql, NpgsqlCommand cmd)
        {
            try
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSql;
                if (this.con.State != ConnectionState.Open)
                    this.con.Open();
                cmd.Connection = this.con;
                int num = cmd.ExecuteNonQuery();
                if (!this.blTransaction)
                    this.con.Close();
                return num;
            }
            catch (Exception ex)
            {
                this.con.Close();
                throw ex;
            }
        }

        public int ExecuteQry(string sSql)
        {
            try
            {
                NpgsqlCommand npgsqlCommand = new NpgsqlCommand();
                npgsqlCommand.CommandType = CommandType.Text;
                npgsqlCommand.CommandText = sSql;
                if (this.con.State != ConnectionState.Open)
                    this.con.Open();
                npgsqlCommand.Connection = this.con;
                int num = npgsqlCommand.ExecuteNonQuery();
                if (!this.blTransaction)
                    this.con.Close();
                return num;
            }
            catch (Exception ex)
            {
                this.con.Close();
                throw ex;
            }
        }

        public int ExecuteQrynon(string sSql)
        {
            try
            {
                NpgsqlCommand npgsqlCommand = new NpgsqlCommand();
                npgsqlCommand.CommandType = CommandType.Text;
                npgsqlCommand.CommandText = sSql;
                if (this.con.State != ConnectionState.Open)
                    this.con.Open();
                npgsqlCommand.Connection = this.con;
                con.BeginTransaction();
                int num = npgsqlCommand.ExecuteNonQuery();
                if (!this.blTransaction)
                    this.con.Close();
                return num;
            }
            catch (Exception ex)
            {
                this.con.Close();
                throw ex;
            }
        }

        public long Get_max_no(string Col_name, string Tab_name)
        {
            try
            {
                if (this.con.State != ConnectionState.Open)
                    this.con.Open();
                NpgsqlDataReader npgsqlDataReader = this.Fetch("SELECT MAX(\"" + Col_name + "\") FROM \"" + Tab_name + "\"");
                if (npgsqlDataReader.Read())
                {
                    if (npgsqlDataReader.GetValue(0).ToString() == "")
                    {
                        npgsqlDataReader.Close();
                        if (!this.blTransaction)
                            this.con.Close();
                        return 1;
                    }
                    long maxNo = Convert.ToInt64(npgsqlDataReader.GetValue(0)) + 1L;
                    npgsqlDataReader.Close();
                    if (!this.blTransaction)
                        this.con.Close();
                    return maxNo;
                }
                if (!this.blTransaction)
                    this.con.Close();
                return -1;
            }
            catch (Exception ex)
            {
                this.con.Close();
                throw ex;
            }
        }

        public NpgsqlDataReader Fetch(string Qry)
        {
            try
            {
                NpgsqlCommand npgsqlCommand = new NpgsqlCommand();
                if (this.con.State != ConnectionState.Open)
                    this.con.Open();
                npgsqlCommand.Connection = this.con;
                npgsqlCommand.CommandText = Qry;
                return npgsqlCommand.ExecuteReader(CommandBehavior.Default);
            }
            catch (Exception ex)
            {
                this.con.Close();
                throw ex;
            }
        }

        public string get_value(string strQry)
        {
            try
            {
                string empty = string.Empty;
                if (this.con.State != ConnectionState.Open)
                    this.con.Open();
                NpgsqlDataReader npgsqlDataReader = this.Fetch(strQry);
                if (npgsqlDataReader.Read())
                    empty = npgsqlDataReader.GetValue(0).ToString();
                npgsqlDataReader.Close();
                if (!this.blTransaction)
                    this.con.Close();
                return empty;
            }
            catch (Exception ex)
            {
                this.con.Close();
                throw ex;
            }
        }

        public string get_value(string strQry, NpgsqlCommand cmd)
        {
            try
            {
                if (this.con.State != ConnectionState.Open)
                    this.con.Open();
                cmd.CommandText = strQry;
                cmd.Connection = this.con;
                string str = Convert.ToString(cmd.ExecuteScalar());
                if (!this.blTransaction)
                    this.con.Close();
                return str;
            }
            catch (Exception ex)
            {
                this.con.Close();
                throw ex;
            }
        }

        public NpgsqlTransaction BeginTransaction()
        {
            if (this.con.State == ConnectionState.Open && !this.blTransaction)
            {
                this.trans = this.con.BeginTransaction(IsolationLevel.ReadCommitted);
                this.blTransaction = true;
            }
            return this.trans;
        }

        public void CommitTransaction()
        {
            if (this.con.State != ConnectionState.Open)
                return;
            if (this.blTransaction)
                this.trans.Commit();
            this.con.Close();
        }

        public void RollBackTrans()
        {
            if (this.con.State != ConnectionState.Open)
                return;
            if (this.blTransaction)
                this.trans.Rollback();
            this.con.Close();
        }

        public string StringGetValue1(NpgsqlCommand cmd)
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
