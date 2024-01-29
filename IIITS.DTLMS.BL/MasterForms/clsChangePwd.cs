using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using IIITS.PGSQL.DAL;
using Npgsql;
using System.Reflection;
using NpgsqlTypes;

namespace IIITS.DTLMS.BL
{
    public class clsChangePwd
    {
        PGSqlConnection Objcon = new PGSqlConnection(Constants.Password);

        string strFormCode = "clsChangePwd";
        public string strOldPwd { get; set; }
        public string strNewPwd { get; set; }
        public string strConfirmPwd { get; set; }
        public string struserId { get; set; }

        NpgsqlCommand NpgsqlCommand;
        public String[] ChangePwd(clsChangePwd objChangepwd)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string sQry = string.Empty;
            string[] Arr = new string[2];
            string sOldPwd = string.Empty;
            try
            {
                #region inline query
                //NpgsqlCommand.Parameters.AddWithValue("uid", Convert.ToInt32(objChangepwd.struserId));
                //sQry = "SELECT \"US_PWD\" FROM \"TBLUSER\" WHERE \"US_ID\" =:uid ";
                //sOldPwd = Objcon.get_value(sQry, NpgsqlCommand);
                #endregion
                DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clschangepwd");
                cmd.Parameters.AddWithValue("p_key", "GETUSERPWD");
                cmd.Parameters.AddWithValue("p_value", objChangepwd.struserId);
                cmd.Parameters.AddWithValue("p_offcode", "");
                sOldPwd = objDatabse.StringGetValue(cmd);

                if (Genaral.CompareLogin(Convert.ToString(sOldPwd), objChangepwd.strOldPwd) == true)
                {
                    //NpgsqlCommand.Parameters.AddWithValue("NewPwd", Genaral.EncryptPassword(objChangepwd.strNewPwd));
                    //NpgsqlCommand.Parameters.AddWithValue("userId", Convert.ToInt32(objChangepwd.struserId));
                    //NpgsqlCommand.Parameters.AddWithValue("NewPassword",objChangepwd.strNewPwd);
                    //sQry = "UPDATE \"TBLUSER\" SET \"US_PWD\" =:NewPwd, \"US_CHPWD_ON\"=NOW() WHERE \"US_ID\" =:userId";

                    //Objcon.ExecuteQry(sQry, NpgsqlCommand);

                    //NpgsqlCommand.Parameters.AddWithValue("userId1", Convert.ToInt32(objChangepwd.struserId));
                    //NpgsqlCommand.Parameters.AddWithValue("OldPwd", sOldPwd);
                    //NpgsqlCommand.Parameters.AddWithValue("userId2", Convert.ToInt32(objChangepwd.struserId));

                    //sQry = "INSERT INTO \"TBLUSER_OLD_PASSWORD\" (\"UOP_ID\",\"UOP_US_ID\",\"UOP_PWD\",\"UOP_CR_ON\",\"UOP_CR_BY\") VALUES ";
                    //sQry += " ((SELECT COALESCE(MAX(\"UOP_ID\"),0)+1 FROM \"TBLUSER_OLD_PASSWORD\"),:userId1,";
                    //sQry += " :OldPwd,NOW(),:userId2)";
                    //Objcon.ExecuteQry(sQry, NpgsqlCommand);

                    //Arr[0] = "Password successfully changed.";
                    //Arr[1] = "1";


                    cmd = new NpgsqlCommand("proc_update_user_password");
                    cmd.Parameters.AddWithValue("new_pwd", Convert.ToString(Genaral.EncryptPassword(objChangepwd.strNewPwd)));
                    cmd.Parameters.AddWithValue("us_id", Convert.ToString(objChangepwd.struserId));
                    cmd.Parameters.AddWithValue("old_pwd", Convert.ToString(sOldPwd));
                    cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                    cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                    Arr[1] = "op_id";
                    Arr[0] = "msg";
                    Arr = Objcon.Execute(cmd, Arr, 2);
                }
                else
                {
                    Arr[0] = "Invalid Old Password";
                    Arr[1] = "0";
                }



            }
            catch (Exception ex)
            {
                Arr[0] = "Invalid User Details";
                Arr[1] = "0";
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return Arr;
        }


    }
}
