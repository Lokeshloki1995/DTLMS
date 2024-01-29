using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Configuration;
using IIITS.PGSQL.DAL;
using Npgsql;

namespace IIITS.DTLMS.BL
{
    public class clsInternalLogin
    {
       public string sLoginName { get; set; }
       public string sFullName { get; set; }
       public string sOfficeCode { get; set; }
       public string sUserType { get; set; }
       public string sUserId { get; set; }
       public string sPassword { get; set; }
       public string sMessage { get; set; }
       public string sEmail { get; set; }
       public string sRoleId { get; set; }
       public string sOfficeName { get; set; }
       public string sDesignation { get; set; }
       public string sMobileNo { get; set; }

       string strFormCode = "clsInternalLogin";
        // CustOledbConnection objCon = new CustOledbConnection(Constants.Password);
       
        public clsInternalLogin UserLogin(clsInternalLogin objLogin)
       {
           try
           {
              
               string sQry = string.Empty;
               DataTable dt = new DataTable();
                PGSqlConnection objCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
                //  PGSqlConnection objCon = new PGSqlConnection(Convert.ToString("SWRlYUAxMjM0NSs="));
                // sQry = "SELECT IU_ID,IU_FULLNAME,IU_LG_NAME,IU_PWD,IU_USERTYPE FROM TBLINTERNALUSERS ";
                //sQry += " WHERE UPPER(IU_LG_NAME)='" + objLogin.sLoginName.ToUpper() + "' AND IU_PWD='" + Genaral.Encrypt(objLogin.sPassword) + "'";
                sQry = "SELECT \"IU_ID\",\"IU_FULLNAME\",\"IU_LG_NAME\",\"IU_PWD\",\"IU_USERTYPE\" FROM \"TBLINTERNALUSERS\" ";
                sQry += " WHERE UPPER(\"IU_LG_NAME\")='" + objLogin.sLoginName.ToUpper() + "' AND \"IU_PWD\"='" + Genaral.Encrypt(objLogin.sPassword) + "'";

                 dt = objCon.FetchDataTable(sQry);

                if (dt.Rows.Count > 0)
                {

                    objLogin.sUserId = dt.Rows[0]["IU_ID"].ToString();
                    objLogin.sFullName = dt.Rows[0]["IU_FULLNAME"].ToString();
                    objLogin.sUserType = dt.Rows[0]["IU_USERTYPE"].ToString();

                }
                else
                {
                    objLogin.sMessage = "Enter Valid User Name and Password";
                }
                return objLogin;

           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "UserLogin");
               objLogin.sMessage = "An exception occurred while processing your request.";
               return objLogin;
           }
       }

       public string Getofficename(string sOfficecode)
       {
           try
           {
                PGSqlConnection objCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
                // PGSqlConnection objCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["SWRlYUAxMjM0NSs="]));
                string strQry = string.Empty;
               strQry = "SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':')+1) AS OFFICENAME  FROM VIEW_ALL_OFFICES WHERE OFF_CODE='" + sOfficecode + "'";
               string Offname = objCon.get_value(strQry);

               if (Offname == null || Offname == "")
               {
                   Offname = "CORP OFFICE";
               }
               return Offname;
           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Getofficename");
               return ex.Message;
           }

       }

        public clsInternalLogin ForgtPassword(clsInternalLogin objLogin)
        {
            try
            {
                PGSqlConnection objCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
               // PGSqlConnection objCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["SWRlYUAxMjM0NSs="]));
                string sQry = string.Empty;
                DataTable dt = new DataTable();

                sQry = "SELECT \"IU_FULLNAME\",\"IU_LG_NAME\",\"IU_PWD\",\"IU_ID\" FROM \"TBLINTERNALUSERS\" where \"IU_MOBILENO\"='" + objLogin.sMobileNo + "'";
                dt = objCon.FetchDataTable(sQry);
                if (dt.Rows.Count > 0)
                {
                    objLogin.sPassword = dt.Rows[0]["IU_PWD"].ToString();
                    objLogin.sFullName = dt.Rows[0]["IU_FULLNAME"].ToString();
                    objLogin.sLoginName = dt.Rows[0]["IU_LG_NAME"].ToString();
                    objLogin.sUserId = dt.Rows[0]["IU_ID"].ToString();
                    //SendMailForgotPwd(objLogin);

                    clsCommunication objComm = new clsCommunication();

                    string sSMSText = String.Format(Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SMStoForgotPwd"]), objLogin.sFullName, Genaral.Decrypt(objLogin.sPassword));
                    objComm.sendSMS(sSMSText, objLogin.sMobileNo, objLogin.sFullName);
                }
                else
                {
                    objLogin.sMessage = "Enter Valid Mobile No";
                }

                return objLogin;

            }
            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdLogin_Click");
                return objLogin;
            }
        }

        public void SendMailForgotPwd(clsInternalLogin objLogin)
       {
           string strMailMsg = string.Empty;
           string strmailFormat = string.Empty;
           clsCommunication objComm = new clsCommunication();

           using (StreamReader sr = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/EmailFormats/CreateUser.txt")))
           {
               String line;
               // Read and display lines from the file until the end of
               // the file is reached.
               while ((line = sr.ReadLine()) != null)
               {
                   strMailMsg += line;
               }
           }
           strmailFormat = String.Format(strMailMsg, objLogin.sFullName, objLogin.sLoginName, objLogin.sPassword);
           objComm.SendMail("DTLMS – Forgot Password", objLogin.sEmail, strmailFormat, objLogin.sUserId);
       }
    }
}
