using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Configuration;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.Mail;
using IIITS.PGSQL.DAL;
using Npgsql;
using System.Data;
using System.Reflection;
using NpgsqlTypes;
using IIITS.DTLMS.BL.DataBase;

namespace IIITS.DTLMS.BL
{
    public class clsCommunication
    {
        
        PGSqlConnection Objcon = new PGSqlConnection(Constants.Password);
        string strFormCode = "clscommunication";

        public string sSMSTemplate { get; set; }
        public string sSMSTemplateID { get; set; }
        public string sSMSkey { get; set; }

        string[] Arr = new string[3];
        //public string sendSMS(string SMS, string Mobileno, string strUserName)
        //{
            
        //    try
        //    {
                
        //        string strMobile1 = "91" + Mobileno;
        //        string sResult = SMSSendFuction(strMobile1, SMS);

        //        string strQry = string.Empty;
        //        string sMaxNo = Convert.ToString(objCon.Get_max_no("SMS_ID", "TBLSMSLOG"));
        //        strQry = "INSERT INTO TBLSMSLOG (SMS_ID,SMS_PHONE_NO,SMS_MESSAGE,SMS_STATUS) VALUES (";//proc_saveupdate_sent_sms()
        //        strQry += " '" + sMaxNo + "','" + Mobileno + "','" + SMS + "','" + sResult + "')";
        //        objCon.Execute(strQry);
        //        return "Sent Successfully";
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "sendSMS");
        //    }
        //    return "";
        //}

        public string sendSMS(string SMS, string Mobileno, string strUserName)
        {
            try
            {

                string strMobile1 = "91" + Mobileno;
                File.AppendAllText("D:\\ERRORLOG\\SMS.text", " sendSMS---- " + strMobile1);               
                string sResult = "";
                //string sResult = SMSSendFuction(strMobile1, SMS);
                //  File.AppendAllText("D:\\ERRORLOG\\DBERROR.txt", sResult + " - " + strMobile1 + Environment.NewLine);
                string sMaxNo = Convert.ToString(Objcon.Get_max_no("SMS_ID", "TBLSMSLOG"));
                NpgsqlCommand cmd = new NpgsqlCommand("proc_saveupdate_sent_sms");
                cmd.Parameters.AddWithValue("sms_id", sMaxNo);
                cmd.Parameters.AddWithValue("sms_mobileno", Mobileno);
                cmd.Parameters.AddWithValue("sms", SMS);
                cmd.Parameters.AddWithValue("sms_result", sResult);

                cmd.Parameters.Add("pk_id", NpgsqlDbType.Text);
                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd.Parameters.Add("msg", NpgsqlDbType.Text);

                cmd.Parameters["pk_id"].Direction = ParameterDirection.Output;
                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                cmd.Parameters["msg"].Direction = ParameterDirection.Output;

                Arr[0] = "pk_id";
                Arr[1] = "op_id";
                Arr[2] = "msg";

                Arr = Objcon.Execute(cmd, Arr, 3);                
            }
            catch (Exception ex)
            {
                //File.AppendAllText("D:\\ERRORLOG\\DBERROR.txt", ex.Message + " - " + ex.Message + Environment.NewLine);

                File.AppendAllText("D:\\ERRORLOG\\SMS.text", "Exception sendSMS----" + ex.StackTrace);              
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return Arr[2];
        }

        //public void DumpSms(string MobileNo, string SMSTEXT)
        //{
        //    try
        //    {
        //        long strID = objCon.Get_max_no("TS_ID", "TBLSMSDUMP");
        //        string strQry = string.Empty;
        //        strQry = "INSERT INTO TBLSMSDUMP(TS_ID,TS_MOBILE_NUMBER,TS_CONTENT,TS_SENDER_ID,TS_SENDER_TYPE) VALUES(";//proc_saveupdate_dump_sms
        //        strQry += " '" + strID + "','" + MobileNo + "', '" + SMSTEXT + "','DTCLMS','WEB')";
        //        objCon.Execute(strQry);
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "DumpSms");
        //    }

        //}
        public clsCommunication GetsmsTempalte(clsCommunication objcom)
        {
            string strId = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                if (Convert.ToString(ConfigurationSettings.AppSettings["SendSMS"]).ToUpper().Equals("ON"))
                {

                    NpgsqlCommand cmd = new NpgsqlCommand("sp_loadsms_template");
                    cmd.Parameters.AddWithValue("sms_key", objcom.sSMSkey);
                   dt = Objcon.FetchDataTable(cmd);
                    if(dt.Rows.Count>0)
                    {
                        objcom.sSMSTemplate= Convert.ToString(dt.Rows[0]["SMT_TEMPLATE"]);
                        objcom.sSMSTemplateID = Convert.ToString(dt.Rows[0]["SMT_TRANSACTION_ID"]);
                    }
                }
                return objcom;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objcom;
            }

        }

        public clsCommunication GetsmsTempalte_Latest(clsCommunication objcom, DataBseConnection objDatabse)
        {
            string strId = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                if (Convert.ToString(ConfigurationManager.AppSettings["SendSMS"]).ToUpper().Equals("ON"))
                {

                    NpgsqlCommand cmd = new NpgsqlCommand("sp_loadsms_template");
                    cmd.Parameters.AddWithValue("sms_key", objcom.sSMSkey);
                    dt = objDatabse.FetchDataTable(cmd);
                    if (dt.Rows.Count > 0)
                    {
                        objcom.sSMSTemplate = Convert.ToString(dt.Rows[0]["SMT_TEMPLATE"]);
                        objcom.sSMSTemplateID = Convert.ToString(dt.Rows[0]["SMT_TRANSACTION_ID"]);
                    }
                }
                return objcom;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objcom;
            }

        }

        public void DumpSms(string MobileNo, string SMSTEXT,string templateId, string Type)
        {
            string strId = string.Empty;
            try
            {
                if (Convert.ToString(ConfigurationSettings.AppSettings["SendSMS"]).ToUpper().Equals("ON"))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand("proc_saveupdate_dump_sms");
                    cmd.Parameters.AddWithValue("smsd_id", strId);
                    cmd.Parameters.AddWithValue("smsd_mobileno", MobileNo);
                    cmd.Parameters.AddWithValue("smsd_text", SMSTEXT);
                    cmd.Parameters.AddWithValue("smsd_template_id", templateId);
                    cmd.Parameters.AddWithValue("sms_type", Type);

                    cmd.Parameters.Add("pk_id", NpgsqlDbType.Text);
                    cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd.Parameters.Add("msg", NpgsqlDbType.Text);

                    cmd.Parameters["pk_id"].Direction = ParameterDirection.Output;
                    cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                    cmd.Parameters["msg"].Direction = ParameterDirection.Output;

                    Arr[0] = "pk_id";
                    Arr[1] = "op_id";
                    Arr[2] = "msg";
                    Arr = Objcon.Execute(cmd, Arr, 3);
                }

                

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        /// <summary>
        /// newly added DumpSms with DataBseConnection objDatabse
        /// </summary>
        /// <param name="MobileNo"></param>
        /// <param name="SMSTEXT"></param>
        /// <param name="templateId"></param>
        /// <param name="Type"></param>
        /// <param name="objDatabse"></param>
        public void DumpSms_Latest(string MobileNo, string SMSTEXT, string templateId, string Type, DataBseConnection objDatabse)
        {
            string strId = string.Empty;
            try
            {
                if (Convert.ToString(ConfigurationManager.AppSettings["SendSMS"]).ToUpper().Equals("ON"))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand("proc_saveupdate_dump_sms");
                    cmd.Parameters.AddWithValue("smsd_id", strId);
                    cmd.Parameters.AddWithValue("smsd_mobileno", MobileNo);
                    cmd.Parameters.AddWithValue("smsd_text", SMSTEXT);
                    cmd.Parameters.AddWithValue("smsd_template_id", templateId);
                    cmd.Parameters.AddWithValue("sms_type", Type);

                    cmd.Parameters.Add("pk_id", NpgsqlDbType.Text);
                    cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd.Parameters.Add("msg", NpgsqlDbType.Text);

                    cmd.Parameters["pk_id"].Direction = ParameterDirection.Output;
                    cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                    cmd.Parameters["msg"].Direction = ParameterDirection.Output;

                    Arr[0] = "pk_id";
                    Arr[1] = "op_id";
                    Arr[2] = "msg";
                    Arr = objDatabse.Execute(cmd, Arr, 3);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        public void DumpSms(string MobileNo, string SMSTEXT, string TemplateId)
        {
            try
            { 
                if (Convert.ToString(ConfigurationSettings.AppSettings["SendSMS"]).ToUpper().Equals("ON") && TemplateId != null )
                {
                    // IDADTC new old DTCLMS
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    long strID = Objcon.Get_max_no("TS_ID", "TBLSMSDUMP");
                    string strQry = string.Empty;
                    strQry = "INSERT INTO TBLSMSDUMP(TS_ID,TS_MOBILE_NUMBER,TS_CONTENT,TS_SENDER_ID,TS_SENDER_TYPE,TS_TEMPLATEID) VALUES(";
                    strQry += " :strID,:MobileNo, :SMSTEXT,'IDADTC','WEB',:TemplateId)";
                    cmd.Parameters.AddWithValue("strID", strID);
                    cmd.Parameters.AddWithValue("MobileNo", MobileNo);
                    cmd.Parameters.AddWithValue("SMSTEXT", SMSTEXT== null ?"" : SMSTEXT);
                    cmd.Parameters.AddWithValue("TemplateId", TemplateId == null ? "" : TemplateId);
                    Objcon.ExecuteQry(strQry, cmd);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "DumpSms");
            }

        }
        private string SMSSendFuction(string MobileNo, string SMSTEXT)
        {
            string strResult = string.Empty;
            try
            {
                if (SMSTEXT.Contains("#"))
                {
                    SMSTEXT = SMSTEXT.Replace("#", "%23");
                }
                if (SMSTEXT.Contains("&"))
                {
                    SMSTEXT = SMSTEXT.Replace("&", "%26");
                }
                string strSenderId = Convert.ToString(ConfigurationSettings.AppSettings["VENDOR_SENDERID"]);

                if (Convert.ToString(ConfigurationSettings.AppSettings["SendSMS"]).ToUpper().Equals("ON"))
                {
                    string strUsername = Convert.ToString(ConfigurationSettings.AppSettings["VENDOR_USERNAME"]);
                    string strPassword = Convert.ToString(ConfigurationSettings.AppSettings["VENDOR_PASS"]);

                    File.AppendAllText("D:\\ERRORLOG\\SMS.text", "------SMSSendFuction" + Environment.NewLine);
                    // File.AppendAllText("D:\\ERRORLOG\\DBERROR.txt", strUsername + " - " + strPassword + Environment.NewLine);

                    string strLog = "sendSMS : '~' Mobile No : " + MobileNo + ", Subject : " + SMSTEXT + "";
                    string baseurl = "http://smslogin.mobi/spanelv2/api.php?username=" + strUsername + "&password=" + strPassword;
                    baseurl += "&to=" + System.Uri.EscapeUriString(MobileNo) + "&from=" + strSenderId + "&message=";
                    baseurl += "" + System.Uri.EscapeUriString(SMSTEXT) + "";
                    File.AppendAllText("D:\\ERRORLOG\\SMS.text", "------SMSSendFuction BaseURL"+ baseurl + Environment.NewLine);

                    ///  File.AppendAllText("D:\\ERRORLOG\\DBERROR.txt", SMSTEXT + " - " + MobileNo + Environment.NewLine);

                    String result = GetPageContent(baseurl);
                    if (result == "Invalid Parameters")
                    {
                        strResult = "Error - Invalid MobileNumber";
                    }
                    else
                    {
                        strResult = GetPageContent("http://smslogin.mobi/spanelv2/api.php?username=" + strUsername + "&password=" + strPassword + "&msgid=" + result);
                    }
                    File.AppendAllText("D:\\ERRORLOG\\SMS.text", "------SMSSendFuction result" + strResult + Environment.NewLine);
                }
                return strResult;
            }
            catch (Exception ex)
            {
                //File.AppendAllText("D:\\ERRORLOG\\DBERROR.txt", ex.Message + " - " + ex.Message + Environment.NewLine);
                File.AppendAllText("D:\\ERRORLOG\\SMS.text", "------Exception" + ex.StackTrace + Environment.NewLine);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }

        private static string GetPageContent(string FullUri)
        {
            HttpWebRequest Request;
            StreamReader ResponseReader;
            Request = ((HttpWebRequest)(WebRequest.Create(FullUri)));
            ResponseReader = new StreamReader(Request.GetResponse().GetResponseStream());
            return ResponseReader.ReadToEnd();
        }



        //public string sendSMS(string SMS, string Mobileno, string strUserType, string strUserName, string strSource)
        //{
        //   try
        //    {             
        //        string strMobile1 = "91" + Mobileno;
        //        if (Convert.ToString(ConfigurationSettings.AppSettings["SendSMS"]).ToUpper().Equals("ON"))
        //        {

        //            WebClient client = new WebClient();
        //            //string baseurl = "http://172.16.1.222/Projectsnew/balwant/smsapp_old/apiv2/?api=http&workingkey=147424c7q013520t8u075&sender=CONDOC&to=";
        //            //  baseurl+="&to=" + System.Uri.EscapeUriString(MobileNo) + "&message=";
        //            //  baseurl+=""+ System.Uri.EscapeUriString(SMSTEXT) + "";
        //            string baseurl = "http://alerts.solutionsinfini.com/api/web2sms.php?workingkey=147424c7q013520t8u075&sender=CONDOC";
        //            baseurl += "&to=" + System.Uri.EscapeUriString(strMobile1) + "&message=";
        //            baseurl += "" + System.Uri.EscapeUriString(SMS) + "";
        //            Stream data = client.OpenRead(baseurl);
        //            StreamReader reader = new StreamReader(data);
        //            string s = reader.ReadToEnd();
        //            string SMSStatus = string.Empty;
        //            if (SMSStatus.StartsWith("Message GID="))
        //            {
        //                SMSStatus = "SMSSent + " + SMSStatus + "";
        //            }
        //            else
        //            {
        //                SMSStatus = s.Trim();
        //            }
        //            // Console.Write(SMSStatus);
        //            data.Close();
        //            reader.Close();


        //            long strID = objCon.Get_max_no("SMS_ID", "TBLSMSLOG");
        //            string strQry = string.Empty;
        //            strQry = "INSERT INTO TBLSMSLOG(SMS_ID,SMS_PHONE_NO,SMS_MESSAGE,SMS_USER_TYPE,SMS_USER_NAME,SMS_SOURCE,SMS_ENTRY_DATE) VALUES(";
        //            strQry += " '" + strID + "','" + Mobileno + "','" + SMS + "','" + strUserType + "','" + strUserName + "','" + strSource + "',SYSDATE)";


        //            objCon.Execute(strQry);

        //            return "Sent Successfully";
        //            //return SMSStatus;
        //        }
        //        return "";

        //    }
        //    catch (Exception ex)
        //    {

        //        return ex.Message;
        //    }
        //}

        /// <summary>
        /// Code for Sending Email
        /// </summary>
        /// <param name="strSubject"></param>
        /// <param name="strMailid"></param>
        /// <param name="strMailMsg"></param>
        /// <returns></returns>
        /// 

    
        public void  SendMail(string strSubject, string strMailid, string strMailMsg,string sCrby)
        {
            string sFolderPath = Convert.ToString(ConfigurationSettings.AppSettings["LOGFILEPATH"]) + DateTime.Now.ToString("yyyyMM");
            if (!Directory.Exists(sFolderPath))
            {
                Directory.CreateDirectory(sFolderPath);
            }
            string sPath = sFolderPath + "//" +  DateTime.Now.ToString("yyyyMMdd") + "-SMSLOG.txt";
            try
            {
                if (Convert.ToString(ConfigurationSettings.AppSettings["SendEmail"]).ToUpper().Equals("ON"))
                {
                    

                    MailMessage mail = new MailMessage();
                    mail.To.Add(strMailid);
                    //mail.From = new mailaddress("ims@ideainfinityit.com", "i~Tracker");
                    mail.From = new MailAddress(Convert.ToString(ConfigurationSettings.AppSettings["SENDMAILID"]), "HESCOM~DTLMS");
                   // mailto: mail.cc.add("support.itracker@ideainfinityit.com");
                    //if (closedByMail != "" && closedByMail != null)
                    //{
                    //    mail.CC.Add(closedByMail);
                    //}
                    mail.Subject = strSubject;
                    mail.IsBodyHtml = true;
                    string Body = strMailMsg;
                    mail.Body = Body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient(Convert.ToString(ConfigurationSettings.AppSettings["SENDSMTP"]), Convert.ToInt32(ConfigurationSettings.AppSettings["SENDSMTPPORT"]));
                    smtp.Credentials = new System.Net.NetworkCredential(Convert.ToString(ConfigurationSettings.AppSettings["SENDMAILID"]), Convert.ToString(ConfigurationSettings.AppSettings["SENDPWD"]));//Password need to be entered
                    smtp.EnableSsl = true;
                    ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                    smtp.Send(mail);

                   
                  
                    File.AppendAllText(sPath, "SendMail After Send()");
                    string strId = string.Empty;                  

                    NpgsqlCommand cmd = new NpgsqlCommand("proc_saveupdate_send_email");
                    cmd.Parameters.AddWithValue("em_id", strId);
                    cmd.Parameters.AddWithValue("em_email_id", strMailid);
                    cmd.Parameters.AddWithValue("em_subject", strSubject);
                    cmd.Parameters.AddWithValue("em_crby", sCrby);                
                 
                    cmd.Parameters.Add("pk_id", NpgsqlDbType.Text);
                    cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd.Parameters.Add("msg", NpgsqlDbType.Text);

                    cmd.Parameters["pk_id"].Direction = ParameterDirection.Output;
                    cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                    cmd.Parameters["msg"].Direction = ParameterDirection.Output;

                    Arr[0] = "pk_id";
                    Arr[1] = "op_id";
                    Arr[2] = "msg";
                    //File.AppendAllText("D:\\ERRORLOG\\SMS.text", "SendMail before " + Arr[0]);
                    Arr = Objcon.Execute(cmd, Arr, 3);
                    
                }                
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                File.AppendAllText(sPath, ex.StackTrace + Environment.NewLine);
                //File.AppendAllText(path, ex.StackTrace + Environment.NewLine);
            }

        }

      

        //public void SaveSMSDetails(string sMobileNo,string sMessage,string sRoleId)//doubt
        //{
        //    try
        //    {
        //        long sMaxNo = objCon.Get_max_no("SMD_ID", "TBLSMSDUMP");
        //        string strQry = string.Empty;

        //         //For Reference Saving RoleId also. It will be usefull in future
        //        strQry = "INSERT INTO TBLSMSDUMP(SMD_ID,SMD_PHONE_NO,SMD_MESSAGE,SMD_ENTRY_DATE,SMD_ROLE_ID) VALUES (";
        //        strQry += " '" + sMaxNo + "','" + sMobileNo + "','" + sMessage + "',SYSDATE,'"+ sRoleId +"')";
        //        objCon.Execute(strQry);

        //    }
        //    catch (Exception ex)
        //    {
        //        //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveSMSDetails");
        //        clsException.LogError("procedure - proc_saveupdate_dump_sms ", ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);

        //    }
        //}

        public void SaveSMSDetails(string sMobileNo, string sMessage, string sRoleId)//doubt
        {
            string strId = string.Empty;
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_saveupdate_dump_sms");
                cmd.Parameters.AddWithValue("smsd_id", strId);
                cmd.Parameters.AddWithValue("smsd_mobileno", sMobileNo);
                cmd.Parameters.AddWithValue("smsd_text", sMessage);

                cmd.Parameters.Add("pk_id", NpgsqlDbType.Text);
                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd.Parameters.Add("msg", NpgsqlDbType.Text);

                cmd.Parameters["pk_id"].Direction = ParameterDirection.Output;
                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                cmd.Parameters["msg"].Direction = ParameterDirection.Output;

                Arr[0] = "pk_id";
                Arr[1] = "op_id";
                Arr[2] = "msg";

                Arr = Objcon.Execute(cmd, Arr, 3);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
       
    }
}
