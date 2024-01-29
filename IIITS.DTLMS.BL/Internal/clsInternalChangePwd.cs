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
    public class clsInternalChangePwd
    {
       // CustOledbConnection objcon = new CustOledbConnection(Constants.Password);
        string strFormCode = "clsInternalChangePwd";
        public string strOldPwd { get; set; }
        public string strNewPwd { get; set; }
        public string strConfirmPwd { get; set; }
        public string struserId { get; set; }

        public String[] ChangePwd(clsInternalChangePwd objChangepwd)
        {
            PGSqlConnection objcon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            string[] Arr = new string[2];
            try
            {

                NpgsqlDataReader dr = objcon.Fetch("SELECT \"IU_PWD\" FROM \"TBLINTERNALUSERS\" where \"IU_ID\"='" + objChangepwd.struserId + "' ");
                if (dr.Read())
                {
                    string oldpwd = dr.GetValue(dr.GetOrdinal("IU_PWD")).ToString();
                    if (oldpwd == Genaral.Encrypt(objChangepwd.strOldPwd))
                    {
                        if (oldpwd == Genaral.Encrypt(objChangepwd.strNewPwd))
                        {

                            Arr[0] = "Old and New password Should not be same";
                            Arr[1] = "0";
                            return Arr;
                        }
                       
                        else
                        {
                            
                            try
                            {
                                dr.Close();
                                string strQry = string.Empty;
                                strQry = "UPDATE \"TBLINTERNALUSERS\" SET \"IU_PWD\" ='" + Genaral.Encrypt(objChangepwd.strNewPwd) + "' WHERE \"IU_ID\" ='" + objChangepwd.struserId + "'";
                                objcon.ExecuteQry(strQry);

                                Arr[0] = "Password changed sucessfully";
                                Arr[1] = "0";
                                return Arr;

                            }
                            catch (Exception ex)
                            {
                               
                                Arr[0] = "Error:" + ex.Message;
                                Arr[1] = "4";

                                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ChangePwd");
                                return Arr;
                            }
                        }

                    }
                    else
                    {

                        Arr[0] = "Invalid Old Password";
                        Arr[1] = "0";
                        return Arr;
                    }
                }
                dr.Close();
                return Arr;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ChangePwd");
                return Arr;
            }
        }

    }
}

