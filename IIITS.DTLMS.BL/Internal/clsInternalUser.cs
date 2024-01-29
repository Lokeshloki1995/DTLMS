using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.IO;
using IIITS.PGSQL.DAL;
using Npgsql;
using System.Configuration;

namespace IIITS.DTLMS.BL
{
    public class clsInternalUser
    {
        string strFormCode = "clsInternalUser";
        public string sUserId { get; set; }
        public string sFullName { get; set; }
        public string sLoginName { get; set; }
        public string sPassword { get; set; }
        public string sDob { get; set; }
        public string sDoj { get; set; }
        public string sPhoneNo { get; set; }
        public string sAddress { get; set; }
        public string sUserType { get; set; }
        public string sCrby { get; set; }
        public string sCrOn { get; set; }
        public string sSupervisorId { get; set; }
        public string slogname { get; set; }
        public string svendorid { get; set; }

       // CustOledbConnection objCon = new CustOledbConnection(Constants.Password);

        public string[] SaveUpdateInternalUserDetails(clsInternalUser objUser)
        {
            PGSqlConnection objCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            string[] Arr = new string[2];
            NpgsqlDataReader drUser;
            string strQry = string.Empty;
            try
            {
                if (objUser.sUserId == "")
                {

                    drUser = objCon.Fetch("select * from \"TBLINTERNALUSERS\" where  \"IU_MOBILENO\"='" + objUser.sPhoneNo + "'  ");
                    if (drUser.Read())
                    {
                        drUser.Close();
                        Arr[0] = "Mobile No Already Exists";
                        Arr[1] = "2";
                        return Arr;
                    }
                    drUser.Close();

                    if (objUser.sSupervisorId == null || objUser.sSupervisorId == "")
                    {
                        objUser.sSupervisorId = "0";
                    }

                    objUser.sUserId = Convert.ToString(objCon.Get_max_no("IU_ID", "TBLINTERNALUSERS"));
                    strQry = "insert into \"TBLINTERNALUSERS\" (\"IU_ID\",\"IU_FULLNAME\",\"IU_LG_NAME\",\"IU_DOB\",\"IU_MOBILENO\",\"IU_ADDRESS\",\"IU_USERTYPE\",\"IU_DOJ\",\"IU_CRON\",\"IU_CRBY\",\"IU_PWD\",\"IU_SUPERVISORID\",\"IU_VENDOR_ID\") ";
                    strQry += "values ('" + objUser.sUserId + "','" + objUser.sFullName + "','" + objUser.sPhoneNo + "',TO_DATE('" + objUser.sDob + "','dd/MM/yyyy'),'" + objUser.sPhoneNo + "','" + objUser.sAddress + "',";
                    strQry += "'" + objUser.sUserType + "',TO_DATE('" + objUser.sDoj + "','dd/MM/yyyy'),now(),'" + objUser.sCrby + "',";
                    strQry += " '" + Genaral.Encrypt(objUser.sPassword) + "','" + objUser.sSupervisorId + "','" + objUser.svendorid + "')";
                    objCon.ExecuteQry(strQry);

                    Arr[0] = "User Details Saved Successfully";
                    Arr[1] = "0";
                    return Arr;
                }
                else
                {

                    drUser = objCon.Fetch("select * from \"TBLINTERNALUSERS\" where  \"IU_MOBILENO\"='" + objUser.sPhoneNo + "' AND \"IU_ID\"<>'" + objUser.sUserId + "' ");
                    if (drUser.Read())
                    {
                        drUser.Close();
                        Arr[0] = "Mobile No Already Exists";
                        Arr[1] = "2";
                        return Arr;
                    }
                    drUser.Close();

                    if (objUser.sSupervisorId == null || objUser.sSupervisorId == "")
                    {
                        objUser.sSupervisorId = "0";
                    }
                    strQry = "UPDATE \"TBLINTERNALUSERS\" SET \"IU_FULLNAME\"='" + objUser.sFullName.ToUpper() + "',\"IU_LG_NAME\"='" + objUser.sPhoneNo.ToUpper() + "'";
                    strQry += " ,\"IU_DOB\"=TO_DATE('" + objUser.sDob.ToUpper() + "','dd/MM/yyyy'),\"IU_MOBILENO\"='" + objUser.sPhoneNo.ToUpper() + "',\"IU_ADDRESS\"='" + objUser.sAddress + "'";
                    strQry += " ,\"IU_USERTYPE\"='" + objUser.sUserType + "',\"IU_DOJ\"=TO_DATE('" + objUser.sDoj + "','dd/MM/yyyy'), \"IU_PWD\"='" + Genaral.Encrypt(objUser.sPassword) + "',";
                    strQry += " \"IU_VENDOR_ID\"='" + objUser.svendorid + "',\"IU_SUPERVISORID\"='" + objUser.sSupervisorId + "' WHERE \"IU_ID\" = '" + objUser.sUserId + "'";
                    objCon.ExecuteQry(strQry);
                    Arr[0] = "User Details Updated Successfully";
                    Arr[1] = "1";
                    return Arr;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveUpdateInternalUserDetails");
                return Arr;
            }

        }

        public object GetUserDetails(clsInternalUser objuser)
        {
            PGSqlConnection objCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            DataTable dtUserDetails = new DataTable();
            try
            {

                NpgsqlDataReader drUser;
                string strQry = string.Empty;
                //strQry = "SELECT IU_ID,IU_FULLNAME,IU_LG_NAME,to_char(IU_DOB,'dd-MM-yyyy')IU_DOB,IU_MOBILENO,IU_ADDRESS,IU_USERTYPE,";
                //strQry += " to_char(IU_DOJ,'dd-MM-yyyy') IU_DOJ,IU_CRON,IU_CRBY,IU_PWD,IU_SUPERVISORID,IU_VENDOR_ID ";
                //strQry += " FROM TBLINTERNALUSERS WHERE IU_ID='" + objuser.sUserId + "' ";

                strQry = "SELECT \"IU_ID\",\"IU_FULLNAME\",\"IU_LG_NAME\",to_char(\"IU_DOB\",'dd-MM-yyyy')IU_DOB,\"IU_MOBILENO\",\"IU_ADDRESS\",\"IU_USERTYPE\",";
                strQry += " to_char(\"IU_DOJ\",'dd-MM-yyyy')IU_DOJ,\"IU_CRON\",\"IU_CRBY\",\"IU_PWD\",\"IU_SUPERVISORID\",\"IU_VENDOR_ID\" ";
                strQry += " FROM \"TBLINTERNALUSERS\" WHERE \"IU_ID\"='" + objuser.sUserId + "' ";
                drUser = objCon.Fetch(strQry);

                dtUserDetails.Load(drUser);
                if (dtUserDetails.Rows.Count > 0)
                {
                    objuser.sUserId = Convert.ToString(dtUserDetails.Rows[0]["IU_ID"]);
                    objuser.sFullName = Convert.ToString(dtUserDetails.Rows[0]["IU_FULLNAME"]);
                    objuser.sLoginName = Convert.ToString(dtUserDetails.Rows[0]["IU_LG_NAME"]);
                    objuser.sDoj = Convert.ToString(dtUserDetails.Rows[0]["IU_DOJ"]);
                    objuser.sDob = Convert.ToString(dtUserDetails.Rows[0]["IU_DOB"]);
                    objuser.sPhoneNo = Convert.ToString(dtUserDetails.Rows[0]["IU_MOBILENO"]);
                    objuser.sAddress = Convert.ToString(dtUserDetails.Rows[0]["IU_ADDRESS"]);
                    objuser.sUserType = Convert.ToString(dtUserDetails.Rows[0]["IU_USERTYPE"]);
                    objuser.sPassword = Convert.ToString(dtUserDetails.Rows[0]["IU_PWD"]);
                    objuser.sSupervisorId = Convert.ToString(dtUserDetails.Rows[0]["IU_SUPERVISORID"]);
                    objuser.svendorid = Convert.ToString(dtUserDetails.Rows[0]["IU_VENDOR_ID"]);
                }
                return objuser;
            }

            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetUserDetails");
                return objuser;
            }
        }

        public DataTable LoadUserGrid(clsInternalUser objuser)
        {
            PGSqlConnection objCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            DataTable dtUserDetails = new DataTable();
            NpgsqlDataReader drUser;
            string strQry = string.Empty;
            try
            {
                //strQry = "SELECT IU_ID,UPPER(IU_FULLNAME) IU_FULLNAME,(SELECT UPPER(UT_NAME) FROM TBLUSERTYPE WHERE UT_ID= IU_USERTYPE) AS IU_USERTYPE";
                //strQry += " ,IU_LG_NAME,IU_MOBILENO,to_char(IU_DOJ,'dd-MON-yyyy')IU_DOJ, IU_PWD ";
                //strQry += " FROM TBLINTERNALUSERS ";

                strQry = "SELECT \"IU_ID\",UPPER(\"IU_FULLNAME\") IU_FULLNAME,(SELECT UPPER(\"UT_NAME\") FROM \"TBLUSERTYPE\" WHERE \"UT_ID\"= \"IU_USERTYPE\") AS IU_USERTYPE";
                strQry += " ,\"IU_LG_NAME\",\"IU_MOBILENO\",to_char(\"IU_DOJ\",'dd-MON-yyyy')IU_DOJ,\"IU_PWD\" ";
                strQry += " FROM \"TBLINTERNALUSERS\" ";

                //if (objuser.sFullName != "")
                //{
                //    strQry += " WHERE UPPER(IU_FULLNAME) like '%" + objuser.sFullName.ToUpper() + " %'  ";
                //}
                strQry += " ORDER BY \"IU_ID\" DESC ";
                drUser = objCon.Fetch(strQry);
                dtUserDetails.Load(drUser);
                return dtUserDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadUserGrid");
                return dtUserDetails;

            }
        }

        public string GetUserPassword(clsInternalUser objuser)
        {
            PGSqlConnection objCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
            DataTable dtUserDetails = new DataTable();
            string Result = string.Empty;
            string Password = string.Empty;
            string msg = string.Empty;
            try
            {
                // OleDbDataReader drUser;
                string strQry = string.Empty;
                //string Password = string.Empty;
                string sQry = string.Empty;

                DataTable dt = new DataTable();


                sQry = "SELECT \"US_FULL_NAME\"  FROM \"TBLUSER\" WHERE \"US_LG_NAME\" = '" + objuser.slogname.ToUpper() + "'  ";

                NpgsqlDataReader dr = objCon.Fetch(sQry);
                dt.Load(dr);
                if (dt.Rows.Count > 0)
                {
                    strQry = "SELECT \"US_PWD\"  FROM \"TBLUSER\" WHERE \"US_LG_NAME\" = '" + objuser.slogname.ToUpper() + "'  ";
                    Password = objCon.get_value(strQry);

                    return Password;
                }
                else
                {

                    msg = "User Does not exist";
                    return msg;
                }



            }

            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetUserPassword");
                return ex.Message;
            }
        }

    }
}
