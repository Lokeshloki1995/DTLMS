using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.IO;
using IIITS.PGSQL.DAL;
using Npgsql;
using System.Reflection;
using System.Configuration;
using System.Globalization;
using System.Security.Cryptography;
using NpgsqlTypes;
using System.Net;
using Newtonsoft.Json;
using IIITS.DTLMS.BL.MasterForms;
using System.Text.RegularExpressions;

namespace IIITS.DTLMS.BL
{
    public class clsLogin
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
        public string sOfficeNamewithType { get; set; }
        public string sChangePwd { get; set; }
        public string sRoleType { get; set; }
        public string sOTP { get; set; }
        public string sResult { get; set; }
        public string LoginUserType { set; get; }
        public string LoginExpireCheck { set; get; }
        public string PwdMessage { set; get; }
        public string MailSessionExpiryTime { set; get; }
        public int SubmitClick { set; get; }
        public int SMSDumppkId { set; get; }
        public string MobileNo { set; get; }
        public string TemplateId { set; get; }
        public string SenderId { set; get; }
        public string ExtraText { set; get; }
        public string SMSText { set; get; }
        public string Client { set; get; }
        public bool VLDEmail_Or_Mob { set; get; }
        PGSqlConnection Objcon = new PGSqlConnection(Constants.Password);
        NpgsqlCommand NpgsqlCommand;

        /// <summary>
        /// Encrypt.
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static string Encrypt(string pwd)
        {
            //this will return the encrypted string
            int n, i;
            string temp;
            temp = "";
            n = pwd.Length;
            for (i = 0; i < n; i++)
            {
                temp = temp + (char)((int)pwd[i] + 123);
            }
            //temp[i]='\0';
            return (temp);
        }
        /// <summary>
        /// User Login.
        /// </summary>
        /// <param name="objLogin"></param>
        /// <returns></returns>
        public clsLogin UserLogin(clsLogin objLogin)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                string[] Arr = new string[3];
                string sQry = string.Empty;
                DataTable dt = new DataTable();
                bool bActiveResult = false;
                string pswd = Encryptmtd("Jan@2023+");

                #region Commented inline query
                //sQry = " SELECT \"US_ID\",\"US_FULL_NAME\",\"US_OFFICE_CODE\",\"US_ROLE_ID\", ";
                //sQry += " TO_CHAR(\"US_EFFECT_FROM\",'DD/MM/YYYY') US_EFFECT_FROM, ";
                //sQry += " \"US_STATUS\",\"US_PWD\", \"US_CHPWD_ON\",(SELECT \"DM_NAME\" FROM \"TBLDESIGNMAST\" ";
                //sQry += " WHERE \"DM_DESGN_ID\"=\"US_DESG_ID\") DM_NAME,(SELECT \"RO_TYPE\" FROM \"TBLROLES\" ";
                //sQry += " WHERE \"RO_ID\"=\"US_ROLE_ID\")RO_TYPE,\"US_EMAIL\" FROM \"TBLUSER\" ";
                //sQry += " WHERE UPPER(\"US_LG_NAME\")=:LoginName ";
                //NpgsqlCommand.Parameters.AddWithValue("LoginName", objLogin.sLoginName.ToUpper());
                //dt = Objcon.FetchDataTable(sQry, NpgsqlCommand);
                #endregion

                #region  Converted to sp
                NpgsqlCommand cmd = new NpgsqlCommand("clslogin_fetch_user_login_details");
                cmd.Parameters.AddWithValue("loginname", Convert.ToString(objLogin.sLoginName.ToUpper()));
                dt = Objcon.FetchDataTable(cmd);
                #endregion

                bool Passwordstatus = false;
                if (dt.Rows.Count > 0)
                {
                    Passwordstatus = Genaral.CompareLogin(Convert.ToString(dt.Rows[0]["US_PWD"]), objLogin.sPassword);
                }
                if (Passwordstatus == false)
                {
                    dt = null;
                }
                if (dt != null)
                {
                    //Check for EffectFrom Condition
                    string sEffectFrom = Convert.ToString(dt.Rows[0]["US_EFFECT_FROM"]);
                    string sStatus = Convert.ToString(dt.Rows[0]["US_STATUS"]);
                    if (sEffectFrom != "" && sStatus == "D")
                    {
                        string sResult = Genaral.DateComparision(sEffectFrom, "", true, false);
                        if (sResult == "1")
                        {
                            bActiveResult = true;
                            sStatus = "A";
                        }
                    }

                    if (sStatus == "A" || bActiveResult == true)
                    {
                        objLogin.sUserId = dt.Rows[0]["us_id"].ToString();
                        objLogin.sFullName = dt.Rows[0]["US_FULL_NAME"].ToString();
                        objLogin.sOfficeCode = dt.Rows[0]["US_OFFICE_CODE"].ToString();
                        objLogin.sRoleType = dt.Rows[0]["RO_TYPE"].ToString();
                        objLogin.sRoleId = dt.Rows[0]["US_ROLE_ID"].ToString();
                        objLogin.sOfficeName = Getofficename(dt.Rows[0]["US_OFFICE_CODE"].ToString(), objLogin.sRoleType);
                        objLogin.sDesignation = dt.Rows[0]["DM_NAME"].ToString();
                        objLogin.sChangePwd = dt.Rows[0]["US_CHPWD_ON"].ToString();

                        if (dt.Columns.Contains("US_EMAIL"))
                        {
                            objLogin.sEmail = dt.Rows[0]["US_EMAIL"].ToString();
                        }
                        objLogin.sOfficeNamewithType = GetofficeNameWithType(objLogin.sOfficeCode, objLogin.sRoleType);

                        #region commented inline query
                        //NpgsqlCommand.Parameters.AddWithValue("UserID", objLogin.sUserId);
                        //sQry = " UPDATE tbluserloginattempt set ula_status=1 ";
                        //sQry += " WHERE cast(ula_user_id as text)=:UserID and ula_status =0 ";
                        //Objcon.ExecuteQry(sQry, NpgsqlCommand);
                        #endregion

                        #region converted to sp
                        NpgsqlCommand cmdloginattempt = new NpgsqlCommand("proc_update_clslogin_userlogin_attempt");
                        cmdloginattempt.Parameters.AddWithValue("user_id", Convert.ToInt32(objLogin.sUserId));
                        Objcon.Execute(cmdloginattempt, Arr, 0);
                        #endregion
                    }
                    else
                    {
                        objLogin.sMessage = "User is Disabled,Please contact Administrator";
                    }
                }
                #region Commented inline query
                //else
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("LoginName1", objLogin.sLoginName.ToUpper());
                //    string strQry = " SELECT \"US_ID\" FROM \"TBLUSER\" ";
                //    strQry += " WHERE UPPER(\"US_LG_NAME\")=:LoginName1 AND \"US_STATUS\"='A' ";
                //    string userID = Objcon.get_value(strQry, NpgsqlCommand);
                //    int loginattempts = 0;
                //    int TotalAttempts = Convert.ToInt32(ConfigurationManager.AppSettings["LoginAttempts"]);
                //    int TotalSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["LoginAttemptsTimeRange"]);
                //    int LoginAttemptsApply = Convert.ToInt32(ConfigurationManager.AppSettings["LoginAttemptsApply"]);
                //    int pendingAttempts = 0;
                //    string res = string.Empty;
                //    int dateDifference = 0;
                //    if (userID != "" && userID != null)
                //    {
                //        NpgsqlCommand.Parameters.AddWithValue("UserID1", Convert.ToInt16(userID));
                //        strQry = " SELECT abs(round(extract(epoch from now() - ula_cron))) FROM (SELECT ula_id, ";
                //        strQry += " ula_cron,row_number() over (PARTITION by ula_user_id ORDER BY ula_attempts desc) ";
                //        strQry += " FROM tbluserloginattempt ";
                //        strQry += " WHERE ula_user_id=:UserID1 and ula_status = 0)A WHERE row_number=1 ";
                //        res = Objcon.get_value(strQry, NpgsqlCommand);
                //        if (res != null && res != "")
                //        {
                //            dateDifference = Convert.ToInt32(res);
                //        }
                //        if (dateDifference < TotalSeconds)
                //        {
                //            NpgsqlCommand.Parameters.AddWithValue("UserID2", Convert.ToInt16(userID));
                //            strQry = " SELECT COALESCE(max(ula_attempts),0)+1 FROM tbluserloginattempt ";
                //            strQry += " WHERE ula_user_id=:UserID2 AND ula_status =0";
                //            loginattempts = Convert.ToInt32(Objcon.get_value(strQry, NpgsqlCommand));

                //            NpgsqlCommand.Parameters.AddWithValue("Userid", Convert.ToInt16(userID));
                //            NpgsqlCommand.Parameters.AddWithValue("loginattempt", Convert.ToInt16(loginattempts));
                //            strQry = " INSERT into tbluserloginattempt (ula_id,ula_user_id,ula_attempts,ula_cron) ";
                //            strQry += " VALUES((SELECT COALESCE(max(ula_id),0)+1 ";
                //            strQry += " FROM tbluserloginattempt), :Userid,:loginattempt,now()) ";
                //            Objcon.ExecuteQry(strQry, NpgsqlCommand);
                //        }
                //        else
                //        {
                //            NpgsqlCommand.Parameters.AddWithValue("Userid1", Convert.ToInt16(userID));
                //            strQry = " UPDATE tbluserloginattempt set ula_status=1 ";
                //            strQry += " WHERE ula_user_id=:Userid1 and ula_status =0 ";
                //            Objcon.ExecuteQry(strQry, NpgsqlCommand);

                //            NpgsqlCommand.Parameters.AddWithValue("UserID12", Convert.ToInt16(userID));
                //            strQry = " SELECT COALESCE(max(ula_attempts),0)+1 FROM tbluserloginattempt ";
                //            strQry += " WHERE ula_user_id=:UserID12 AND ula_status =0 ";
                //            loginattempts = Convert.ToInt32(Objcon.get_value(strQry, NpgsqlCommand));

                //            NpgsqlCommand.Parameters.AddWithValue("Userid2", Convert.ToInt16(userID));
                //            NpgsqlCommand.Parameters.AddWithValue("loginattempt1", Convert.ToInt16(loginattempts));
                //            strQry = " INSERT into tbluserloginattempt (ula_id,ula_user_id,ula_attempts,ula_cron) ";
                //            strQry += " VALUES((SELECT COALESCE(max(ula_id),0)+1 ";
                //            strQry += " FROM tbluserloginattempt),:Userid2,:loginattempt1,now()) ";
                //            Objcon.ExecuteQry(strQry, NpgsqlCommand);
                //        }
                //    }
                //    if (loginattempts != 0 && (loginattempts > LoginAttemptsApply && loginattempts <= (TotalAttempts - 1)))
                //    {
                //        pendingAttempts = TotalAttempts - loginattempts;
                //        objLogin.sMessage = "You had " + pendingAttempts + " more Attempts left, Enter Valid User Name and Password";
                //    }
                //    else if (loginattempts == TotalAttempts)
                //    {
                //        NpgsqlCommand.Parameters.AddWithValue("UserID3", Convert.ToInt16(userID));
                //        strQry = "UPDATE \"TBLUSER\" set \"US_STATUS\"='D' WHERE \"US_ID\"=:UserID3 AND \"US_STATUS\"='A'";
                //        Objcon.ExecuteQry(strQry, NpgsqlCommand);

                //        NpgsqlCommand.Parameters.AddWithValue("UserID4", Convert.ToInt16(userID));
                //        strQry = "UPDATE tbluserloginattempt set ula_status=1 WHERE ula_user_id=:UserID4 and ula_status =0";
                //        Objcon.ExecuteQry(strQry, NpgsqlCommand);
                //        objLogin.sMessage = "Your Account has been Locked, kindly contact DTLMS Support";
                //    }
                //    else
                //    {
                //        objLogin.sMessage = "Enter Valid User Name and Password";
                //    }
                //}

                #endregion
                #region Converted to sp
                else
                {
                    string[] Arr1 = new string[3];
                    int loginattempts = 0;
                    int TotalAttempts = Convert.ToInt32(ConfigurationManager.AppSettings["LoginAttempts"]);
                    int TotalSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["LoginAttemptsTimeRange"]);
                    int LoginAttemptsApply = Convert.ToInt32(ConfigurationManager.AppSettings["LoginAttemptsApply"]);
                    int pendingAttempts = 0;
                    string res = string.Empty;
                    int dateDifference = 0;
                    NpgsqlCommand cmdinsert = new NpgsqlCommand("proc_insert_update_userloginattempt");
                    cmdinsert.Parameters.AddWithValue("login_name", objLogin.sLoginName.ToUpper());
                    cmdinsert.Parameters.AddWithValue("date_difference", dateDifference);
                    cmdinsert.Parameters.AddWithValue("total_seconds", TotalSeconds);
                    cmdinsert.Parameters.AddWithValue("login_attempts", loginattempts);
                    cmdinsert.Parameters.AddWithValue("login_attempts_apply", LoginAttemptsApply);
                    cmdinsert.Parameters.AddWithValue("total_attempts", TotalAttempts);
                    cmdinsert.Parameters.AddWithValue("pending_attempts", pendingAttempts);
                    cmdinsert.Parameters.Add("message", NpgsqlDbType.Text);

                    cmdinsert.Parameters["message"].Direction = ParameterDirection.Output;

                    Arr[0] = "message";

                    Arr = Objcon.Execute(cmdinsert, Arr, 1);
                    objLogin.sMessage = Arr[0].ToString();
                }
                #endregion
                return objLogin;
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                objLogin.sMessage = "An exception occurred while processing your request.";
                return objLogin;
            }
        }
        /// <summary>
        /// Encryptmtd
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public string Encryptmtd(string pwd)
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[pwd.Length];
            encode = Encoding.UTF8.GetBytes(pwd);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;
        }
        /// <summary>
        /// Get office name.
        /// </summary>
        /// <param name="sOfficecode"></param>
        /// <param name="sRoleType"></param>
        /// <returns></returns>
        public string Getofficename(string sOfficecode, string sRoleType)
        {
            //NpgsqlCommand = new NpgsqlCommand();
            try
            {
                //string strQry = string.Empty;
                //if (sRoleType == "1" || sRoleType == "3")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("OfficeCode", sOfficecode);
                //    strQry = " SELECT split_part(\"OFF_NAME\", ':',2) AS OFFICENAME  FROM \"VIEW_ALL_OFFICES\" ";
                //    strQry += " WHERE CAST(\"OFF_CODE\" AS TEXT)=:OfficeCode";

                //    string Offname = Objcon.get_value(strQry, NpgsqlCommand);

                //    if (Offname == null || Offname == "")
                //    {
                //        Offname = "CORPORATE OFFICE";
                //    }
                //    return Offname;
                //}
                //else
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("OfficeCode1", sOfficecode);
                //    strQry = "SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE CAST(\"SM_ID\" AS TEXT) =:OfficeCode1";
                //    string Offname = Objcon.get_value(strQry, NpgsqlCommand);
                //    return Offname;
                //}


                #region converted to sp
                DataBase.DataBseConnection objconn = new DataBase.DataBseConnection(Constants.Password);

                NpgsqlCommand cmdofficename = new NpgsqlCommand("fetch_officename_clslogin");
                cmdofficename.Parameters.AddWithValue("office_code", sOfficecode == "" ? '0' : Convert.ToInt32(sOfficecode));
                cmdofficename.Parameters.AddWithValue("roletype", Convert.ToInt32(sRoleType));
                return objconn.StringGetValue(cmdofficename);
                #endregion

            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }

        }
        /// <summary>
        /// Get office Name With Type.
        /// </summary>
        /// <param name="sOfficecode"></param>
        /// <param name="sRoleType"></param>
        /// <returns></returns>
        public string GetofficeNameWithType(string sOfficecode, string sRoleType)
        {
            // NpgsqlCommand = new NpgsqlCommand();
            try
            {
                //string strQry = string.Empty;
                //if (sRoleType == "1" || sRoleType == "3")
                //{


                //    NpgsqlCommand.Parameters.AddWithValue("OfficeCode", sOfficecode);
                //    strQry = "SELECT \"OFF_NAME\" FROM \"VIEW_ALL_OFFICES\" WHERE CAST(\"OFF_CODE\" AS TEXT) =:OfficeCode";

                //    string Offname = Objcon.get_value(strQry, NpgsqlCommand);

                //    if (Offname == null || Offname == "")
                //    {
                //        Offname = "CORPORATE OFFICE";
                //    }


                //    return Offname;
                //}
                //else
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("OfficeCode1", sOfficecode);
                //    strQry = "SELECT \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE CAST(\"SM_ID\" AS TEXT) =:OfficeCode1";
                //    string Offname = Objcon.get_value(strQry, NpgsqlCommand);
                //    return Offname;
                //}

                #region converted to sp
                DataBase.DataBseConnection objgetvalue = new DataBase.DataBseConnection(Constants.Password);

                NpgsqlCommand cmdoffnamewithtype = new NpgsqlCommand("fetch_officenamewith_type_clslogin");
                cmdoffnamewithtype.Parameters.AddWithValue("office_code", sOfficecode == "" ? '0' : Convert.ToInt32(sOfficecode));
                cmdoffnamewithtype.Parameters.AddWithValue("roletype", Convert.ToInt32(sRoleType));
                return objgetvalue.StringGetValue(cmdoffnamewithtype);
                #endregion

            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }
        /// <summary>
        /// ForgtPassword Method.
        /// </summary>
        /// <param name="objLogin"></param>
        /// <returns></returns>
        public clsLogin ForgtPassword(clsLogin objLogin)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                string sQry = string.Empty;
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                string sSMSText = String.Empty;
                Regex r1 = new Regex(@"^\d+$");
                string sPattern = @"^\d{10}$";
                Regex r = new Regex(sPattern);
                string[] Arr = new string[3];
                if (r.IsMatch(objLogin.sEmail))
                {
                    //NpgsqlCommand.Parameters.AddWithValue("Email", objLogin.sEmail);
                    //sQry = " SELECT \"US_FULL_NAME\",\"US_LG_NAME\",\"US_PWD\",\"US_ID\",\"US_MOBILE_NO\" FROM \"TBLUSER\" ";
                    //sQry += " where \"US_STATUS\"='A' AND CAST(\"US_MOBILE_NO\" AS TEXT)=:Email ";
                    //dt = Objcon.FetchDataTable(sQry, NpgsqlCommand);

                    NpgsqlCommand cmd = new NpgsqlCommand("clslogin_fetch_user_details_bymobile");
                    cmd.Parameters.AddWithValue("mobile", objLogin.sEmail);
                    dt = Objcon.FetchDataTable(cmd);
                    if (dt.Rows.Count > 0)
                    {
                        objLogin.sPassword = dt.Rows[0]["US_PWD"].ToString();
                        objLogin.sFullName = dt.Rows[0]["US_FULL_NAME"].ToString();
                        objLogin.sLoginName = dt.Rows[0]["US_LG_NAME"].ToString();
                        objLogin.sUserId = dt.Rows[0]["US_ID"].ToString();
                        objLogin.sMobileNo = dt.Rows[0]["US_MOBILE_NO"].ToString();
                        clsCommunication objComm = new clsCommunication();
                        Random generator = new Random();
                        int OTP = generator.Next(100000, 1234567);
                        Random random = new Random();
                        int num = random.Next(0, 26);
                        int num1 = random.Next(0, 26);
                        char let = (char)('A' + num);
                        char let1 = (char)('A' + num1);
                        objLogin.sOTP = Convert.ToString(OTP) + let + let1;
                        objLogin.sOTP = Shuffle(objLogin.sOTP);
                        objComm.sSMSkey = "SMStoOTP";
                        objComm = objComm.GetsmsTempalte(objComm);
                        if (objComm.sSMSTemplate != null)
                        {
                            sSMSText = String.Format(objComm.sSMSTemplate, objLogin.sFullName, objLogin.sOTP);
                            objLogin.sMessage = sSMSText;
                            //NpgsqlCommand.Parameters.AddWithValue("userId", Convert.ToInt16(objLogin.sUserId));
                            //sQry = " SELECT to_char(otp_cron,'yyyy-MM-dd HH24:mi') || '~' || otp_sent_flag ";
                            //sQry += " FROM (SELECT otp_cron,otp_sent_flag, ";
                            //sQry += " otp_id,\"row_number\"() over(partition by otp_us_id ORDER BY ";
                            //sQry += " otp_id desc) as rownum FROM tblotp WHERE otp_us_id=:userId)A WHERE rownum=1 ";
                            //string sSentFlag = Objcon.get_value(sQry, NpgsqlCommand);

                            #region Converted to sp
                            DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
                            NpgsqlCommand cmdsentflag = new NpgsqlCommand("fetch_getvalue_clslogin");
                            cmdsentflag.Parameters.AddWithValue("p_key", "SENTFLAG");
                            cmdsentflag.Parameters.AddWithValue("p_value", objLogin.sUserId);
                            cmdsentflag.Parameters.AddWithValue("p_value2", "");

                            string sSentFlag = objDatabse.StringGetValue(cmdsentflag);
                            #endregion
                            if (sSentFlag == "")
                            {
                                //NpgsqlCommand.Parameters.AddWithValue("userId1", Convert.ToInt16(objLogin.sUserId));
                                //NpgsqlCommand.Parameters.AddWithValue("OTP", objLogin.sOTP);
                                //sQry = " INSERT INTO tblotp (otp_us_id,otp_no,otp_cron,otp_sent_flag,otp_change_pwd_on) ";
                                //sQry += " VALUES(:userId1,:OTP,now(),'0',now())";
                                //Objcon.ExecuteQry(sQry, NpgsqlCommand);

                                NpgsqlCommand cmdinsert = new NpgsqlCommand("proc_insert_otp_clslogin");
                                cmdinsert.Parameters.AddWithValue("user_id", Convert.ToInt32(objLogin.sUserId));
                                cmdinsert.Parameters.AddWithValue("otp", objLogin.sOTP);
                                cmdinsert.Parameters.AddWithValue("key", "");
                                cmdinsert.Parameters.Add("message", NpgsqlDbType.Text);
                                cmdinsert.Parameters["message"].Direction = ParameterDirection.Output;
                                Arr[0] = "message";
                                Arr = Objcon.Execute(cmdinsert, Arr, 1);


                                if (objComm.sSMSTemplateID != null && objComm.sSMSTemplateID != "")
                                {
                                    objComm.DumpSms(objLogin.sMobileNo, sSMSText, objComm.sSMSTemplateID, "WEB");
                                    // string scampid = SendSMSNew(objLogin.sMobileNo, sSMSText, "IDADTC", "", objComm.sSMSTemplateID); // old code for sms 
                                    string Result = SendSMS_Internal(objLogin.sMobileNo, sSMSText, "IDADTC", "", objComm.sSMSTemplateID);
                                    // to update the status in TBLSMSDUMP table
                                    if (Result.Contains("UniqueId"))
                                    {
                                        //NpgsqlCommand.Parameters.AddWithValue("mobileNo", Convert.ToDouble(objLogin.sMobileNo));
                                        //sQry = " SELECT \"TS_ID\" FROM \"TBLSMSDUMP\" WHERE \"TS_MOBILE_NUMBER\"=:mobileNo ";
                                        //sQry += " AND \"TS_CONTENT\" LIKE '%OTP%' AND \"TS_SENT_FLAG\"='0'";
                                        //string sDump_id = Objcon.get_value(sQry, NpgsqlCommand);

                                        NpgsqlCommand cmddumpid = new NpgsqlCommand("fetch_getvalue_clslogin");
                                        cmddumpid.Parameters.AddWithValue("p_key", "DUMPID");
                                        cmddumpid.Parameters.AddWithValue("p_value", Convert.ToString(objLogin.sMobileNo));
                                        cmddumpid.Parameters.AddWithValue("p_value2", "");

                                        string sDump_id = objDatabse.StringGetValue(cmddumpid);

                                        if (sDump_id != "" && sDump_id != null)
                                        {
                                            //NpgsqlCommand.Parameters.AddWithValue("Dump_id", Convert.ToInt32(sDump_id));
                                            //NpgsqlCommand.Parameters.AddWithValue("smsresult", Result);
                                            //sQry = " UPDATE \"TBLSMSDUMP\" SET \"TS_OTP_FLAG\"='1',\"TS_SENT_FLAG\"='1', ";
                                            //sQry += " \"TS_FAILURE_REASON\"=:smsresult WHERE \"TS_ID\"=:Dump_id ";
                                            //Objcon.ExecuteQry(sQry, NpgsqlCommand);

                                            NpgsqlCommand cmdupdate = new NpgsqlCommand("proc_update_smsdump_clslogin");
                                            cmdupdate.Parameters.AddWithValue("dump_id", Convert.ToInt32(sDump_id));
                                            cmdupdate.Parameters.AddWithValue("result", Result);
                                            cmdupdate.Parameters.Add("message", NpgsqlDbType.Text);
                                            cmdupdate.Parameters["message"].Direction = ParameterDirection.Output;
                                            Arr[0] = "message";
                                            Arr = Objcon.Execute(cmdupdate, Arr, 1);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (Convert.ToString(sSentFlag.Split('~').GetValue(1)) == "1"
                                    || Convert.ToString(sSentFlag.Split('~').GetValue(1)) == "")
                                {
                                    //sQry = " INSERT INTO tblotp (otp_us_id,otp_no,otp_cron,otp_sent_flag,otp_change_pwd_on) ";
                                    //sQry += " VALUES(:userId2,:OTP1,now(),'0',now()) ";
                                    //NpgsqlCommand.Parameters.AddWithValue("userId2", Convert.ToInt16(objLogin.sUserId));
                                    //NpgsqlCommand.Parameters.AddWithValue("OTP1", objLogin.sOTP);
                                    //Objcon.ExecuteQry(sQry, NpgsqlCommand);

                                    NpgsqlCommand cmdinsert = new NpgsqlCommand("proc_insert_otp_clslogin");
                                    cmdinsert.Parameters.AddWithValue("user_id", Convert.ToInt32(objLogin.sUserId));
                                    cmdinsert.Parameters.AddWithValue("otp", objLogin.sOTP);
                                    cmdinsert.Parameters.AddWithValue("key", "");
                                    cmdinsert.Parameters.Add("message", NpgsqlDbType.Text);
                                    cmdinsert.Parameters["message"].Direction = ParameterDirection.Output;
                                    Arr[0] = "message";
                                    Arr = Objcon.Execute(cmdinsert, Arr, 1);

                                    if (objComm.sSMSTemplateID != null && objComm.sSMSTemplateID != "")
                                    {
                                        objComm.DumpSms(objLogin.sMobileNo, sSMSText, objComm.sSMSTemplateID, "WEB");

                                        // string scampid = SendSMSNew(objLogin.sMobileNo, sSMSText, "IDADTC", "", objComm.sSMSTemplateID); // old code for sms 
                                        string Result = SendSMS_Internal(objLogin.sMobileNo, sSMSText, "IDADTC", "", objComm.sSMSTemplateID);
                                        // to update the status in TBLSMSDUMP table
                                        if (Result.Contains("UniqueId"))
                                        {
                                            //NpgsqlCommand.Parameters.AddWithValue("mobileNo", Convert.ToDouble(objLogin.sMobileNo));
                                            //sQry = " SELECT \"TS_ID\" FROM \"TBLSMSDUMP\" WHERE \"TS_MOBILE_NUMBER\"=:mobileNo ";
                                            //sQry += " AND \"TS_CONTENT\" LIKE '%OTP%' AND \"TS_SENT_FLAG\"='0' order by \"TS_ID\" desc ";
                                            //string sDump_id = Objcon.get_value(sQry, NpgsqlCommand);

                                            NpgsqlCommand cmddumpid = new NpgsqlCommand("fetch_getvalue_clslogin");
                                            cmddumpid.Parameters.AddWithValue("p_key", "DUMPID");
                                            cmddumpid.Parameters.AddWithValue("p_value", Convert.ToString(objLogin.sMobileNo));
                                            cmddumpid.Parameters.AddWithValue("p_value2", "");

                                            string sDump_id = objDatabse.StringGetValue(cmddumpid);
                                            if (sDump_id != "" && sDump_id != null)
                                            {
                                                //NpgsqlCommand.Parameters.AddWithValue("Dump_id", Convert.ToInt32(sDump_id));
                                                //NpgsqlCommand.Parameters.AddWithValue("smsresult", Result);
                                                //sQry = " UPDATE \"TBLSMSDUMP\" SET \"TS_OTP_FLAG\"='1',\"TS_SENT_FLAG\"='1', ";
                                                //sQry += " \"TS_FAILURE_REASON\"=:smsresult WHERE \"TS_ID\"=:Dump_id ";
                                                //Objcon.ExecuteQry(sQry, NpgsqlCommand);

                                                NpgsqlCommand cmdupdate = new NpgsqlCommand("proc_update_smsdump_clslogin");
                                                cmdupdate.Parameters.AddWithValue("dump_id", Convert.ToInt32(sDump_id));
                                                cmdupdate.Parameters.AddWithValue("result", Result);
                                                cmdupdate.Parameters.Add("message", NpgsqlDbType.Text);
                                                cmdupdate.Parameters["message"].Direction = ParameterDirection.Output;
                                                Arr[0] = "message";
                                                Arr = Objcon.Execute(cmdupdate, Arr, 1);

                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    DateTime PrevOTP_DATE = DateTime.ParseExact(
                                        Convert.ToString(sSentFlag.Split('~').GetValue(0)), "yyyy-MM-dd HH:mm",
                                        CultureInfo.InvariantCulture);
                                    DateTime Now_DATE = DateTime.Now;
                                    TimeSpan finalres = Now_DATE - PrevOTP_DATE;
                                    int iTotalSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["TotalSeconds"]);

                                    if (finalres.TotalSeconds < iTotalSeconds)
                                    {
                                        objLogin.sMessage = "OTP Already Sent to your mobile number please try later";
                                        objLogin.sResult = "1";
                                    }
                                    else
                                    {
                                        //NpgsqlCommand.Parameters.AddWithValue("mobileNo", Convert.ToDouble(objLogin.sMobileNo));
                                        //sQry = "SELECT \"TS_ID\" FROM \"TBLSMSDUMP\" WHERE \"TS_MOBILE_NUMBER\"=:mobileNo ";
                                        //sQry += " AND \"TS_CONTENT\" LIKE '%OTP%' AND \"TS_SENT_FLAG\"='0' ";
                                        //string sDump_id = Objcon.get_value(sQry, NpgsqlCommand);


                                        NpgsqlCommand cmddumpid = new NpgsqlCommand("fetch_getvalue_clslogin");
                                        cmddumpid.Parameters.AddWithValue("p_key", "DUMPID");
                                        cmddumpid.Parameters.AddWithValue("p_value", Convert.ToString(objLogin.sMobileNo));
                                        cmddumpid.Parameters.AddWithValue("p_value2", "");

                                        string sDump_id = objDatabse.StringGetValue(cmddumpid);

                                        if (sDump_id != "" && sDump_id != null)
                                        {
                                            //NpgsqlCommand.Parameters.AddWithValue("Dump_id", Convert.ToInt32(sDump_id));
                                            //sQry = " UPDATE \"TBLSMSDUMP\" SET \"TS_OTP_FLAG\"='1',\"TS_SENT_FLAG\"='1' ";
                                            //sQry += " WHERE \"TS_ID\"=:Dump_id ";
                                            //Objcon.ExecuteQry(sQry, NpgsqlCommand);


                                            NpgsqlCommand cmdupdate = new NpgsqlCommand("proc_update_smsdump_clslogin");
                                            cmdupdate.Parameters.AddWithValue("dump_id", Convert.ToInt32(sDump_id));
                                            cmdupdate.Parameters.AddWithValue("result", "");
                                            cmdupdate.Parameters.Add("message", NpgsqlDbType.Text);
                                            cmdupdate.Parameters["message"].Direction = ParameterDirection.Output;
                                            Arr[0] = "message";
                                            Arr = Objcon.Execute(cmdupdate, Arr, 1);
                                        }
                                        //NpgsqlCommand.Parameters.AddWithValue("UserId", Convert.ToInt16(objLogin.sUserId));
                                        //sQry = "UPDATE tblotp set otp_cancel_flag='1',otp_sent_flag='1' WHERE otp_us_id=:UserId";
                                        //Objcon.ExecuteQry(sQry, NpgsqlCommand);

                                        //NpgsqlCommand.Parameters.AddWithValue("UserId1", Convert.ToInt16(objLogin.sUserId));
                                        //NpgsqlCommand.Parameters.AddWithValue("OTP2", objLogin.sOTP);
                                        //sQry = " INSERT INTO tblotp (otp_us_id,otp_no,otp_cron,otp_sent_flag,otp_change_pwd_on) ";
                                        //sQry += " VALUES(:UserId1,:OTP2,now(),'0',now())";
                                        //Objcon.ExecuteQry(sQry, NpgsqlCommand);


                                        NpgsqlCommand cmdinsert = new NpgsqlCommand("proc_insert_otp_clslogin");
                                        cmdinsert.Parameters.AddWithValue("user_id", Convert.ToInt32(objLogin.sUserId));
                                        cmdinsert.Parameters.AddWithValue("otp", objLogin.sOTP);
                                        cmdinsert.Parameters.AddWithValue("key", "UPDATE_INSERT");
                                        cmdinsert.Parameters.Add("message", NpgsqlDbType.Text);
                                        cmdinsert.Parameters["message"].Direction = ParameterDirection.Output;
                                        Arr[0] = "message";
                                        Arr = Objcon.Execute(cmdinsert, Arr, 1);

                                        if (objComm.sSMSTemplateID != null && objComm.sSMSTemplateID != "")
                                        {
                                            objComm.DumpSms(objLogin.sMobileNo, sSMSText, objComm.sSMSTemplateID, "WEB");
                                        }
                                        // string scampid = SendSMSNew(objLogin.sMobileNo, sSMSText, "IDADTC", "", objComm.sSMSTemplateID); // old code for sms 
                                        string Result = SendSMS_Internal(objLogin.sMobileNo, sSMSText, "IDADTC", "", objComm.sSMSTemplateID);
                                        // to update the status in TBLSMSDUMP table
                                        if (Result.Contains("UniqueId"))
                                        {
                                            //NpgsqlCommand.Parameters.AddWithValue("mobileNo", Convert.ToDouble(objLogin.sMobileNo));
                                            //sQry = " SELECT \"TS_ID\" FROM \"TBLSMSDUMP\" WHERE \"TS_MOBILE_NUMBER\"=:mobileNo ";
                                            //sQry += " AND \"TS_CONTENT\" LIKE '%OTP%' AND \"TS_SENT_FLAG\"='0' ";
                                            //sQry += " order by \"TS_ID\" desc limit 1 ";
                                            //string ssDump_id = Objcon.get_value(sQry, NpgsqlCommand);

                                            NpgsqlCommand cmdsdumpid = new NpgsqlCommand("fetch_getvalue_clslogin");
                                            cmdsdumpid.Parameters.AddWithValue("p_key", "DUMPID");
                                            cmdsdumpid.Parameters.AddWithValue("p_value", Convert.ToString(objLogin.sMobileNo));
                                            cmdsdumpid.Parameters.AddWithValue("p_value2", "");

                                            string ssDump_id = objDatabse.StringGetValue(cmdsdumpid);

                                            if (ssDump_id != "" && ssDump_id != null)
                                            {
                                                //NpgsqlCommand.Parameters.AddWithValue("Dump_id", Convert.ToInt32(ssDump_id));
                                                //NpgsqlCommand.Parameters.AddWithValue("smsresult", Result);
                                                //sQry = " UPDATE \"TBLSMSDUMP\" SET \"TS_OTP_FLAG\"='1',\"TS_SENT_FLAG\"='1', ";
                                                //sQry += " \"TS_FAILURE_REASON\"=:smsresult WHERE \"TS_ID\"=:Dump_id ";
                                                //Objcon.ExecuteQry(sQry, NpgsqlCommand);


                                                NpgsqlCommand cmdupdate = new NpgsqlCommand("proc_update_smsdump_clslogin");
                                                cmdupdate.Parameters.AddWithValue("dump_id", Convert.ToInt32(sDump_id));
                                                cmdupdate.Parameters.AddWithValue("result", Result);
                                                cmdupdate.Parameters.Add("message", NpgsqlDbType.Text);
                                                cmdupdate.Parameters["message"].Direction = ParameterDirection.Output;
                                                Arr[0] = "message";
                                                Arr = Objcon.Execute(cmdupdate, Arr, 1);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            objLogin.sMessage = "SMS FAILS";
                            objLogin.sResult = "1";
                        }
                    }
                    else
                    {
                        objLogin.sMessage = "Enter Valid Registered Mobile Number";
                        objLogin.sResult = "1";
                    }
                }
                else
                {
                    if (r1.IsMatch(objLogin.sEmail))
                    {
                        objLogin.sMessage = "Enter Valid MobileNo";
                        objLogin.sResult = "1";
                    }
                    else
                    {
                        Random generator = new Random();
                        int OTP = generator.Next(1000000, 1234567);
                        //NpgsqlCommand.Parameters.AddWithValue("emailId", objLogin.sEmail);
                        //sQry = " SELECT \"US_FULL_NAME\",\"US_LG_NAME\",\"US_PWD\",\"US_ID\",\"US_MOBILE_NO\" ";
                        //sQry += " FROM \"TBLUSER\" where \"US_EMAIL\"=:emailId ";
                        //dt = Objcon.FetchDataTable(sQry, NpgsqlCommand);

                        NpgsqlCommand cmd = new NpgsqlCommand("clslogin_fetch_user_details_byemail");
                        cmd.Parameters.AddWithValue("email", objLogin.sEmail);
                        dt = Objcon.FetchDataTable(cmd);
                        if (dt.Rows.Count > 0)
                        {
                            objLogin.sPassword = dt.Rows[0]["US_PWD"].ToString();
                            objLogin.sFullName = dt.Rows[0]["US_FULL_NAME"].ToString();
                            objLogin.sLoginName = dt.Rows[0]["US_LG_NAME"].ToString();
                            objLogin.sUserId = dt.Rows[0]["US_ID"].ToString();
                            objLogin.sMobileNo = dt.Rows[0]["US_MOBILE_NO"].ToString();
                            Random random = new Random();
                            int num = random.Next(0, 26);
                            int num1 = random.Next(0, 26);
                            char let = (char)('A' + num);
                            char let1 = (char)('A' + num1);
                            objLogin.sOTP = Convert.ToString(OTP) + let + let1;
                            objLogin.sOTP = Convert.ToString(objLogin.sOTP);
                            objLogin.sOTP = Shuffle(objLogin.sOTP);
                            objLogin.sMessage = sSMSText;
                            SendMailForgotPwd(objLogin);


                            //NpgsqlCommand.Parameters.AddWithValue("userId5", Convert.ToInt16(objLogin.sUserId));
                            //sQry = " SELECT otp_sent_flag FROM tblotp WHERE otp_us_id=:userId5 ";
                            //string sSentFlag = Objcon.get_value(sQry, NpgsqlCommand);

                            DataBase.DataBseConnection objgetvalue = new DataBase.DataBseConnection(Constants.Password);

                            NpgsqlCommand cmdflag = new NpgsqlCommand("fetch_getvalue_clslogin");
                            cmdflag.Parameters.AddWithValue("p_key", "OTP_SENTFLAG");
                            cmdflag.Parameters.AddWithValue("p_value", Convert.ToString(objLogin.sUserId));
                            cmdflag.Parameters.AddWithValue("p_value2", "");
                            string sSentFlag = objgetvalue.StringGetValue(cmdflag);

                            if (sSentFlag == "1" || sSentFlag == "")
                            {
                                //NpgsqlCommand.Parameters.AddWithValue("userId6", Convert.ToInt16(objLogin.sUserId));
                                //NpgsqlCommand.Parameters.AddWithValue("OTP3", objLogin.sOTP);
                                //sQry = " INSERT INTO tblotp (otp_us_id,otp_no,otp_cron,otp_sent_flag,otp_change_pwd_on) ";
                                //sQry += " VALUES(:userId6,:OTP3,now(),'0',now()) ";
                                //Objcon.ExecuteQry(sQry, NpgsqlCommand);


                                NpgsqlCommand cmdinsert = new NpgsqlCommand("proc_insert_otp_clslogin");
                                cmdinsert.Parameters.AddWithValue("user_id", Convert.ToInt32(objLogin.sUserId));
                                cmdinsert.Parameters.AddWithValue("otp", objLogin.sOTP);
                                cmdinsert.Parameters.AddWithValue("key", "");
                                cmdinsert.Parameters.Add("message", NpgsqlDbType.Text);
                                cmdinsert.Parameters["message"].Direction = ParameterDirection.Output;
                                Arr[0] = "message";
                                Arr = Objcon.Execute(cmdinsert, Arr, 1);
                            }
                            else
                            {

                                //NpgsqlCommand.Parameters.AddWithValue("OTP4", objLogin.sOTP);
                                //NpgsqlCommand.Parameters.AddWithValue("UserId7", Convert.ToInt16(objLogin.sUserId));
                                //sQry = "UPDATE tblotp set otp_no=:OTP4,otp_cron=now() WHERE otp_us_id=:UserId7";
                                //Objcon.ExecuteQry(sQry, NpgsqlCommand);


                                NpgsqlCommand cmdinsert = new NpgsqlCommand("proc_insert_otp_clslogin");
                                cmdinsert.Parameters.AddWithValue("user_id", Convert.ToInt32(objLogin.sUserId));
                                cmdinsert.Parameters.AddWithValue("otp", objLogin.sOTP);
                                cmdinsert.Parameters.AddWithValue("key", "UPDATE");
                                cmdinsert.Parameters.Add("message", NpgsqlDbType.Text);
                                cmdinsert.Parameters["message"].Direction = ParameterDirection.Output;
                                Arr[0] = "message";
                                Arr = Objcon.Execute(cmdinsert, Arr, 1);
                            }
                        }
                        else
                        {
                            objLogin.sMessage = "Enter Valid Registered Email Id";
                            objLogin.sResult = "1";
                        }
                    }
                }
                return objLogin;
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objLogin;
            }
        }
        /// <summary>
        /// send New SMS
        /// </summary>
        /// <param name="sMobileNo"></param>
        /// <param name="sSMS"></param>
        /// <param name="sSenderID"></param>
        /// <param name="sConfigSenderId"></param>
        /// <param name="sTemplateID"></param>
        /// <returns></returns>
        public string SendSMSNew(string sMobileNo, string sSMS, string sSenderID, string sConfigSenderId, string sTemplateID)
        {
            try
            {
                string strUsername = Convert.ToString(ConfigurationManager.AppSettings["VENDOR_USERNAME1"]);
                string strPassword = Convert.ToString(ConfigurationManager.AppSettings["VENDOR_PASS1"]);
                string strLink = Convert.ToString(ConfigurationManager.AppSettings["VENDOR_LINK1"]);
                string sAPIKey = Convert.ToString(ConfigurationManager.AppSettings["APIKEY1"]);
                string IsSenderIdinConfig = Convert.ToString(ConfigurationManager.AppSettings["SenderIDFromConfig1"]);
                if (sSMS.Contains("#"))
                {
                    sSMS = sSMS.Replace("#", "%23");
                }
                if (sSMS.Contains("&"))
                {
                    sSMS = sSMS.Replace("&", "%26");
                }
                if (IsSenderIdinConfig == "ON")
                {
                    sSenderID = sConfigSenderId;
                }
                string strLog = "sendSMS : '~' Mobile No : " + sMobileNo + ", Subject : " + sSMS + "";

                string baseurl = strLink + "/v3/api.php?username=" + strUsername + "&apikey=" + sAPIKey + "&senderid=";
                baseurl += "" + sSenderID + " &templateid=" + sTemplateID + "&mobile=" + System.Uri.EscapeUriString(sMobileNo) + "";
                baseurl += "&message=" + System.Uri.EscapeUriString(sSMS).Replace("+", "%2B") + "";
                String result = GetPageContent(baseurl);
                Console.WriteLine(sMobileNo + "-" + result);
                return result;
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }
        /// <summary>
        /// SendSMS Internal.
        /// </summary>
        /// <param name="sMobileNo"></param>
        /// <param name="sSMS"></param>
        /// <param name="sSenderID"></param>
        /// <param name="sConfigSenderId"></param>
        /// <param name="sTemplateID"></param>
        /// <returns></returns>
        public string SendSMS_Internal(string sMobileNo, string sSMS, string sSenderID, string sConfigSenderId, string sTemplateID)
        {
            try
            {
                string Vender_Username = Convert.ToString(ConfigurationManager.AppSettings["SMSVENDER_USERNAME"]);
                string Vender_Password = Convert.ToString(ConfigurationManager.AppSettings["SMSVENDER_PASSWORD"]);

                clsSMSFire_API SMSFire_API = new clsSMSFire_API();
                SMSFire_API.MobileNo = sMobileNo;
                SMSFire_API.TemplateId = sTemplateID;
                SMSFire_API.SenderId = sSenderID;
                SMSFire_API.ExtraText = ExtraText;
                SMSFire_API.SMSText = sSMS;
                SMSFire_API.Client = "HESCOMDTLMS";

                string WebApiURL = Convert.ToString(ConfigurationManager.AppSettings["SMSWEBAPIURL"]
                    ?? "http://tracker.ideainfinityit.com:1996/");
                string PostController = Convert.ToString(ConfigurationManager.AppSettings["APIPOSTCONTROLLER"]
                    ?? "/api/SMSApi/FireSMS/");
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Headers["UserName"] = Vender_Username;
                client.Headers["Password"] = Vender_Password;
                client.Encoding = Encoding.UTF8;
                string results = client.UploadString(
                    WebApiURL + PostController, JsonConvert.SerializeObject(SMSFire_API)
                    );
                SMSFire_API = JsonConvert.DeserializeObject<clsSMSFire_API>(results);
                return results;
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                     System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }
        /// <summary>
        /// Get Page Content.
        /// </summary>
        /// <param name="FullUri"></param>
        /// <returns></returns>
        static string GetPageContent(string FullUri)
        {
            HttpWebRequest Request;
            StreamReader ResponseReader;
            Request = ((HttpWebRequest)(WebRequest.Create(FullUri)));
            ResponseReader = new StreamReader(Request.GetResponse().GetResponseStream());
            return ResponseReader.ReadToEnd();
        }
        /// <summary>
        /// Shuffle
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Shuffle(string str)
        {
            char[] array = str.ToCharArray();
            Random rng = new Random();
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
            return new string(array);
        }
        /// <summary>
        /// Get UserDetails ToView.
        /// </summary>
        /// <param name="objLogin"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public clsLogin GetUserDetailsToView(clsLogin objLogin, DataTable dt)
        {
            try
            {
                objLogin.sPassword = Convert.ToString(dt.Rows[0]["US_PWD"]);
                objLogin.sFullName = Convert.ToString(dt.Rows[0]["US_FULL_NAME"]);
                objLogin.sLoginName = Convert.ToString(dt.Rows[0]["US_LG_NAME"]);
                objLogin.sUserId = Convert.ToString(dt.Rows[0]["US_ID"]);
                objLogin.sMobileNo = Convert.ToString(dt.Rows[0]["US_MOBILE_NO"]);
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return objLogin;
        }
        /// <summary>
        /// Send Mail ForgotPwd
        /// </summary>
        /// <param name="objLogin"></param>
        public void SendMailForgotPwd(clsLogin objLogin)
        {
            string strMailMsg = string.Empty;
            string strmailFormat = string.Empty;
            clsCommunication objComm = new clsCommunication();
            using (StreamReader sr = new StreamReader(
                System.Web.HttpContext.Current.Server.MapPath("~/EmailFormats/ForgotPassUser.txt")
                ))
            {
                String line;
                // Read and display lines from the file until the end of
                // the file is reached.                
                while ((line = sr.ReadLine()) != null)
                {
                    strMailMsg += line;
                }
            }
            strmailFormat = String.Format(strMailMsg, objLogin.sFullName, objLogin.sLoginName, objLogin.sOTP);
            objComm.SendMail("DTLMS – Forgot Password", objLogin.sEmail, strmailFormat, objLogin.sUserId);
        }
        /// <summary>
        /// Get USerID from Tabel TBLUSER and check the OTP Details From Tabel tblotp.
        /// Get OTP Details from Tabel tblotp
        /// </summary>
        /// <param name="sOtp"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public DataTable GetOTPDetails(string sOtp, string UserName)
        {
            NpgsqlCommand = new NpgsqlCommand();
            DataTable dtOTPDetails = new DataTable();
            string sQry = string.Empty;
            try
            {
                //string QryUser = " SELECT \"US_ID\"  FROM \"TBLUSER\" WHERE \"US_LG_NAME\" =:UserName ||'' ";
                //NpgsqlCommand.Parameters.AddWithValue("UserName", UserName.ToUpper());
                //int userId = Convert.ToInt32(Objcon.get_value(QryUser, NpgsqlCommand));

                //NpgsqlCommand.Parameters.AddWithValue("Otp", sOtp);
                //NpgsqlCommand.Parameters.AddWithValue("P_UserId", userId);
                //sQry = "SELECT * FROM tblotp WHERE otp_no=:Otp and otp_sent_flag='0' and otp_us_id=:P_UserId";
                //dtOTPDetails = Objcon.FetchDataTable(sQry, NpgsqlCommand);




                NpgsqlCommand cmd = new NpgsqlCommand("fetch_clslogin_otpdetails");
                cmd.Parameters.AddWithValue("user_name", UserName.ToUpper());
                cmd.Parameters.AddWithValue("otp", sOtp);
                dtOTPDetails = Objcon.FetchDataTable(cmd);
                return dtOTPDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtOTPDetails;
            }
        }
        /// <summary>
        /// Get Configuration from Tabel TBLCONFIGURATION from Db.
        /// </summary>
        /// <returns></returns>
        public DataTable GetConfiguration()
        {
            DataTable dtConfiguration = new DataTable();
            string strQry = string.Empty;
            try
            {
                //strQry = "SELECT * FROM \"TBLCONFIGURATION\"";
                //return Objcon.FetchDataTable(strQry);

                NpgsqlCommand cmd = new NpgsqlCommand("fetch_clslogin_configuration_details");
                return Objcon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtConfiguration;
            }
        }
        /// <summary>
        /// Get Password Details.
        /// </summary>
        /// <param name="sUserId"></param>
        /// <returns></returns>
        public string GetPasswordDetails(string sUserId)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string strQry = string.Empty;
            try
            {
                //NpgsqlCommand.Parameters.AddWithValue("userId", Convert.ToInt64(sUserId));
                //strQry = " SELECT to_char(CURRENT_DATE -\"US_CHPWD_ON\",'dd') AS \"Days\" FROM \"TBLUSER\" ";
                //strQry += " WHERE \"US_ID\"=:userId ";
                //return Convert.ToString(Objcon.get_value(strQry, NpgsqlCommand));


                DataBase.DataBseConnection objgetvalue = new DataBase.DataBseConnection(Constants.Password);

                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clslogin");
                cmd.Parameters.AddWithValue("p_key", "DAYS");
                cmd.Parameters.AddWithValue("p_value", Convert.ToString(sUserId));
                cmd.Parameters.AddWithValue("p_value2", "");
                return objgetvalue.StringGetValue(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }
        /// <summary>
        /// Get Status from TBLUSER_OLD_PASSWORD.
        /// </summary>
        /// <param name="sOldPassword"></param>
        /// <param name="sUserId"></param>
        /// <returns></returns>
        public bool GetStatus(string sOldPassword, string sUserId)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string strQry = string.Empty;
            try
            {
                //NpgsqlCommand.Parameters.AddWithValue("psw", sOldPassword);
                //NpgsqlCommand.Parameters.AddWithValue("usid", sUserId);
                //strQry = "SELECT \"UOP_ID\" FROM \"TBLUSER_OLD_PASSWORD\" WHERE \"UOP_PWD\"=:psw AND \"UOP_US_ID\"=:usid";
                //string sRes = Objcon.get_value(strQry, NpgsqlCommand);


                DataBase.DataBseConnection objgetvalue = new DataBase.DataBseConnection(Constants.Password);

                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clslogin");
                cmd.Parameters.AddWithValue("p_key", "UOP_ID");
                cmd.Parameters.AddWithValue("p_value", sOldPassword);
                cmd.Parameters.AddWithValue("p_value2", sUserId);
                string sRes = objgetvalue.StringGetValue(cmd);

                if (sRes != null && sRes != "")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }
        /// <summary>
        /// Password Expiry Check.
        /// </summary>
        /// <param name="objLogin"></param>
        /// <returns></returns>
        public clsLogin PasswordExpiryCheck(clsLogin objLogin)
        {
            string[] PwdCheck = new string[3];
            DataTable dtDropdown = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("sp_check_user_password_expiry_check");
                cmd.Parameters.AddWithValue("username", Convert.ToString(objLogin.sLoginName));
                cmd.Parameters.Add("id", NpgsqlDbType.Text);
                cmd.Parameters.Add("usertype", NpgsqlDbType.Text);
                cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd.Parameters["id"].Direction = ParameterDirection.Output;
                cmd.Parameters["usertype"].Direction = ParameterDirection.Output;
                cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                PwdCheck[0] = "id";
                PwdCheck[1] = "usertype";
                PwdCheck[2] = "msg";
                PwdCheck = Objcon.Execute(cmd, PwdCheck, 3);
                objLogin.LoginExpireCheck = Convert.ToString(PwdCheck[0]);
                objLogin.LoginUserType = Convert.ToString(PwdCheck[1]);
                objLogin.PwdMessage = Convert.ToString(PwdCheck[2]);

                return objLogin;
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            finally
            {
                Objcon.close();
            }
            return objLogin;
        }
        /// <summary>
        /// Save EmailAndPassWord ExpireLog.
        /// </summary>
        /// <param name="objLogin"></param>
        /// <returns></returns>
        //public clsLogin SaveEmailAndPassWordExpireLog(clsLogin objLogin)
        //{
        //    try
        //    {
        //        string[] strResult = new string[2];
        //        NpgsqlCommand cmdSaveLog = new NpgsqlCommand("sp_insert_password_history_maillog");
        //        cmdSaveLog.Parameters.AddWithValue("usid", Convert.ToInt32(objLogin.sUserId));
        //        cmdSaveLog.Parameters.AddWithValue("mailid", Convert.ToString(objLogin.sEmail));
        //        cmdSaveLog.Parameters.AddWithValue("expirytime", Convert.ToInt32(MailSessionExpiryTime));
        //        cmdSaveLog.Parameters.AddWithValue("apptype", 1);
        //        cmdSaveLog.Parameters.Add("pkid", NpgsqlDbType.Text);
        //        cmdSaveLog.Parameters.Add("status", NpgsqlDbType.Text);
        //        cmdSaveLog.Parameters["pkid"].Direction = ParameterDirection.Output;
        //        cmdSaveLog.Parameters["status"].Direction = ParameterDirection.Output;
        //        strResult[0] = "pkid";
        //        strResult[1] = "status";
        //        strResult = Objcon.Execute(cmdSaveLog, strResult, 2);
        //        objLogin.SubmitClick = Convert.ToInt32(strResult[0]);
        //        objLogin.PwdMessage = Convert.ToString(strResult[1]);
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
        //           System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //    return objLogin;
        //}
        /// <summary>
        /// Fetch Mail History.
        /// </summary>
        /// <param name="objLogin"></param>
        /// <returns></returns>
        public string FetchMailHistory(clsLogin objLogin)
        {
            try
            {
                //string sQry = " SELECT concat(\"US_CR_ON\",'~',\"US_FULL_NAME\",'~',\"US_ID\") FROM \"TBLUSER\" WHERE \"US_ID\" = '" + objLogin.SubmitClick + "' ";

                //return Objcon.get_value(sQry);

                DataBase.DataBseConnection objgetvalue = new DataBase.DataBseConnection(Constants.Password);

                NpgsqlCommand cmdsentflag = new NpgsqlCommand("fetch_getvalue_clslogin");
                cmdsentflag.Parameters.AddWithValue("p_key", "CONCAT_USER_DETAILS");
                cmdsentflag.Parameters.AddWithValue("p_value", objLogin.SubmitClick);
                cmdsentflag.Parameters.AddWithValue("p_value2", "");
                return objgetvalue.StringGetValue(cmdsentflag);
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                throw ex;
            }
            finally
            {
                Objcon.close();
            }
        }
        /// <summary>
        /// Fetch LastChanged Passowrd CrOn
        /// </summary>
        /// <param name="objLogin"></param>
        /// <returns></returns>
        public string FetchLastChangedPassowrdCrOn(clsLogin objLogin)
        {
            try
            {
                //string sQry = " select \"US_CHPWD_ON\" from \"TBLUSER\" ";
                //sQry += " inner join tbl_password_history_maillog on \"US_ID\" = phm_us_id ";
                //sQry += " where phm_id = '" + objLogin.SubmitClick + "' ";
                //return Objcon.get_value(sQry);


                DataBase.DataBseConnection objgetvalue = new DataBase.DataBseConnection(Constants.Password);

                NpgsqlCommand cmdsentflag = new NpgsqlCommand("fetch_getvalue_clslogin");
                cmdsentflag.Parameters.AddWithValue("p_key", "US_CHPWD_ON");
                cmdsentflag.Parameters.AddWithValue("p_value", objLogin.SubmitClick);
                cmdsentflag.Parameters.AddWithValue("p_value2", "");

                return objgetvalue.StringGetValue(cmdsentflag);
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                throw ex;
            }
            finally
            {
                Objcon.close();
            }
        }
        /// <summary>
        /// this method will check the db with the user name and get Email and mobial no.
        /// and compares the provided email/mobial no with existing email or mobial no.
        /// </summary>
        /// <param name="objLogin"></param>
        /// <returns></returns>
        public clsLogin CheckEmailOrMobialNoDetails(clsLogin objLogin)
        {
            NpgsqlCommand = new NpgsqlCommand();

            string Email = string.Empty;
            string Mobile = string.Empty;
            string QryChck = string.Empty;
            int ComparedValu;
            DataTable DtCheck = new DataTable();
            try
            {
                //QryChck = " SELECT \"US_EMAIL\",\"US_MOBILE_NO\",\"US_LG_NAME\" from \"TBLUSER\" ";
                //QryChck += " WHERE \"US_LG_NAME\" =:UserName ||'' ";
                //NpgsqlCommand.Parameters.AddWithValue("UserName", objLogin.sLoginName.ToUpper());
                //DtCheck = Objcon.FetchDataTable(QryChck, NpgsqlCommand);

                #region  Converted to sp
                NpgsqlCommand cmd = new NpgsqlCommand("clslogin_fetch_user_login_details");
                cmd.Parameters.AddWithValue("loginname", Convert.ToString(objLogin.sLoginName.ToUpper()));
                DtCheck = Objcon.FetchDataTable(cmd);
                #endregion
                if (DtCheck.Rows.Count > 0)
                {
                    Email = Convert.ToString(DtCheck.Rows[0]["US_EMAIL"]);
                    Mobile = Convert.ToString(DtCheck.Rows[0]["US_MOBILE_NO"]);
                }
                else
                {
                    objLogin.VLDEmail_Or_Mob = false;
                    objLogin.sMessage = "Please enter valid user name.";
                    return objLogin;
                }

                if (objLogin.sEmail.Contains('@')) // compares email
                {
                    ComparedValu = string.Compare(Email, objLogin.sEmail);
                    // 0 means the compared string are same.
                    if (ComparedValu == 0)
                    {
                        objLogin.VLDEmail_Or_Mob = true;
                    }
                    else
                    {
                        objLogin.VLDEmail_Or_Mob = false;
                        objLogin.sMessage = "Entred Email ID is not registered.";
                    }
                }
                else if (Regex.IsMatch(objLogin.sEmail, @"^\d+$"))
                {
                    if (objLogin.sEmail.Length == 10)
                    {
                        ComparedValu = string.Compare(Mobile, objLogin.sEmail);
                        // 0 means the compared string are same.
                        if (ComparedValu == 0)
                        {
                            objLogin.VLDEmail_Or_Mob = true;

                        }
                        else
                        {
                            objLogin.VLDEmail_Or_Mob = false;
                            objLogin.sMessage = "Entred Mobile No is not registered.";
                        }
                    }
                    else
                    {
                        objLogin.VLDEmail_Or_Mob = false;
                        objLogin.sMessage = "Enter vaild Mobile No.";
                    }
                }
                else
                {
                    objLogin.VLDEmail_Or_Mob = false;
                    objLogin.sMessage = "Enter vaild registered Email ID / Mobile No.";
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                throw ex;
            }
            return objLogin;
        }

    }
}
