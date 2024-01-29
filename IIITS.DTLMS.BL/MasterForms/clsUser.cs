using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Reflection;
using System.Configuration;

namespace IIITS.DTLMS.BL
{
    public class clsUser
    {
        string strFormCode = "clsUser";
        public string lSlNo { get; set; }
        public string sOfficeCode { get; set; }
        public string sFullName { get; set; }
        public string sLoginName { get; set; }
        public string sPassword { get; set; }
        public string sMobileNo { get; set; }
        public string sDesignation { get; set; }
        public string sRole { get; set; }
        public string sEmail { get; set; }
        public string sPhoneNo { get; set; }
        public string sAddress { get; set; }
        public string sUserType { get; set; }
        public string sCrby { get; set; }
        public string sStatus { get; set; }
        public Byte[] sSignImage { get; set; }
        public string sEffectFrom { get; set; }
        public string sReason { get; set; }
        public string sOffCode { get; set; }
        public string sRoleType { get; set; }
        public string sOTP { get; set; }
        public string struserId { get; set; }

        public string zone { get; set; }
        public string circle { get; set; }
        public string division { get; set; }
        public string subdivision { get; set; }
        public string section { get; set; }
        public string rolename { get; set; }
        public string ActiveUser { get; set; }
        public bool FetchStore { get; set; } // add new on 21-01-2023


        PGSqlConnection objCon = new PGSqlConnection(Constants.Password);
        NpgsqlCommand NpgsqlCommand;
        DataTable DtUserGridDetails = new DataTable("UserGridDetails");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public string[] SaveUpdateUserDetails(clsUser objUser)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string[] Arr = new string[2];

            string strQry = string.Empty;
            try
            {

                if (objUser.sOfficeCode != "")
                {
                    String sOffCode = String.Empty;
                    if (objUser.sRole == "2" || objUser.sRole == "5")
                    {
                        sOffCode = objUser.sOfficeCode;
                    }
                    else
                    {
                        NpgsqlCommand.Parameters.AddWithValue("offcode", Convert.ToInt32(objUser.sOfficeCode));
                        sOffCode = objCon.get_value("SELECT \"OFF_CODE\" FROM \"VIEW_ALL_OFFICES\" WHERE \"OFF_CODE\" =:offcode", NpgsqlCommand);
                    }
                    if (sOffCode.Length <= 0)
                    {
                        Arr[0] = "Enter Valid Office Code";
                        Arr[1] = "4";
                        return Arr;
                    }
                }
                if (objUser.sMobileNo != "")
                {
                    NpgsqlCommand.Parameters.AddWithValue("MobileNo", Convert.ToDouble(objUser.sMobileNo));
                    String sOffCode = objCon.get_value("SELECT \"US_MOBILE_NO\" FROM \"TBLUSER\" WHERE \"US_MOBILE_NO\" =:MobileNo  AND cast(\"US_ID\" as Text)<>'" + objUser.lSlNo + "'", NpgsqlCommand);
                    if (sOffCode.Length > 0)
                    {
                        Arr[0] = "Mobile Number Already Exists";
                        Arr[1] = "4";
                        return Arr;
                    }
                }
                if (objUser.sEmail != "")
                {
                    NpgsqlCommand.Parameters.AddWithValue("Email", objUser.sEmail);
                    String sOffCode = objCon.get_value("SELECT \"US_EMAIL\" FROM \"TBLUSER\" WHERE \"US_EMAIL\" =:Email AND cast(\"US_ID\" as Text)<>'" + objUser.lSlNo + "'", NpgsqlCommand);
                    if (sOffCode.Length > 0)
                    {
                        Arr[0] = "Email ID Already Exists";
                        Arr[1] = "4";
                        return Arr;
                    }
                }
                if (objUser.lSlNo == "")
                {
                    NpgsqlCommand.Parameters.AddWithValue("LoginName", objUser.sLoginName.ToUpper());
                    String sOffCode = objCon.get_value("SELECT \"US_LG_NAME\" FROM \"TBLUSER\" WHERE UPPER(\"US_LG_NAME\") =:LoginName", NpgsqlCommand);
                    if (sOffCode.Length > 0)
                    {
                        Arr[0] = "Login Name Already Exists";
                        Arr[1] = "4";
                        return Arr;
                    }
                    if (objUser.sOfficeCode != null && objUser.sOfficeCode != "" && objUser.sRole != null && objUser.sRole != "")
                    {
                        DataTable dt = new DataTable();
                        NpgsqlCommand.Parameters.AddWithValue("OfficeCode", Convert.ToString(sOfficeCode));
                        NpgsqlCommand.Parameters.AddWithValue("Role", Convert.ToInt32(sRole));
                        string Qry = "SELECT \"US_LG_NAME\",\"US_OFFICE_CODE\",\"US_ROLE_ID\" FROM \"TBLUSER\" WHERE \"US_OFFICE_CODE\"=:OfficeCode and \"US_ROLE_ID\"=:Role and \"US_STATUS\" = 'A'";
                        dt = objCon.FetchDataTable(Qry, NpgsqlCommand);
                        if (dt.Rows.Count > 0)
                        {
                            Arr[0] = "User Already Created For This Location And Role";
                            Arr[1] = "4";
                            return Arr;
                        }
                    }

                    string sMaxNo = Convert.ToString(objCon.Get_max_no("US_ID", "TBLUSER"));

                    NpgsqlCommand.Parameters.AddWithValue("MaxNo", Convert.ToInt32(sMaxNo));
                    NpgsqlCommand.Parameters.AddWithValue("FullName", objUser.sFullName.ToUpper());
                    NpgsqlCommand.Parameters.AddWithValue("LoginName1", objUser.sLoginName.ToUpper());
                    NpgsqlCommand.Parameters.AddWithValue("OfficeCode", objUser.sOfficeCode);
                    NpgsqlCommand.Parameters.AddWithValue("Password", Genaral.EncryptPassword(objUser.sPassword));
                    NpgsqlCommand.Parameters.AddWithValue("Role", Convert.ToInt16(objUser.sRole));
                    NpgsqlCommand.Parameters.AddWithValue("Email", objUser.sEmail);
                    NpgsqlCommand.Parameters.AddWithValue("MobileNo", Convert.ToDouble(objUser.sMobileNo));
                    NpgsqlCommand.Parameters.AddWithValue("PhoneNo", objUser.sPhoneNo);
                    NpgsqlCommand.Parameters.AddWithValue("Address", objUser.sAddress);
                    NpgsqlCommand.Parameters.AddWithValue("Crby", Convert.ToInt16(objUser.sCrby));
                    NpgsqlCommand.Parameters.AddWithValue("Designation", Convert.ToSByte(objUser.sDesignation));

                    strQry = "INSERT INTO \"TBLUSER\" (\"US_ID\",\"US_FULL_NAME\",\"US_LG_NAME\",\"US_OFFICE_CODE\",\"US_PWD\",\"US_ROLE_ID\",\"US_EMAIL\",\"US_MOBILE_NO\",";
                    strQry += " \"US_PHONE_NO\",\"US_ADDRESS\",\"US_CR_ON\",\"US_CRBY\",\"US_DESG_ID\",\"US_SIGN_IMAGE\") ";
                    strQry += "values (:MaxNo,:FullName,:LoginName1,";
                    strQry += ":OfficeCode,:Password,";
                    strQry += " :Role,:Email,";
                    strQry += ":MobileNo,:PhoneNo,:Address,NOW(),";
                    strQry += " :Crby,:Designation,':Photo')";
                    objCon.ExecuteQry(strQry, NpgsqlCommand);

                    // To send the Mail for successfull creation
                    SendMailUserSuccCreate(objUser);
                    SendSMSUserSuccCreate(objUser);

                    Arr[0] = sMaxNo;
                    Arr[1] = "0";
                    return Arr;
                }
                else
                {
                    NpgsqlCommand.Parameters.AddWithValue("LoginName2", objUser.sLoginName.ToUpper());
                    NpgsqlCommand.Parameters.AddWithValue("slno", Convert.ToInt32(objUser.lSlNo));
                    String sOffCode = objCon.get_value("SELECT \"US_LG_NAME\" FROM \"TBLUSER\" WHERE UPPER(\"US_LG_NAME\") =:LoginName2 AND \"US_ID\" <>:slno", NpgsqlCommand);
                    if (sOffCode.Length > 0)
                    {
                        Arr[0] = "Login Name Already Exists";
                        Arr[1] = "4";
                        return Arr;
                    }

                    NpgsqlCommand.Parameters.AddWithValue("FullName1", objUser.sFullName.ToUpper());
                    NpgsqlCommand.Parameters.AddWithValue("LoginName3", objUser.sLoginName.ToUpper());
                    NpgsqlCommand.Parameters.AddWithValue("OfficeCode1", objUser.sOfficeCode);
                    NpgsqlCommand.Parameters.AddWithValue("Role1", Convert.ToInt16(objUser.sRole));
                    NpgsqlCommand.Parameters.AddWithValue("Email1", objUser.sEmail);
                    NpgsqlCommand.Parameters.AddWithValue("MobileNo1", Convert.ToDouble(objUser.sMobileNo));
                    NpgsqlCommand.Parameters.AddWithValue("PhoneNo1", objUser.sPhoneNo);
                    NpgsqlCommand.Parameters.AddWithValue("Address1", objUser.sAddress);
                    NpgsqlCommand.Parameters.AddWithValue("Designation1", Convert.ToSByte(objUser.sDesignation));

                    strQry = "UPDATE \"TBLUSER\" SET \"US_FULL_NAME\" =:FullName1, \"US_LG_NAME\" =:LoginName3, \"US_OFFICE_CODE\" =:OfficeCode1,";
                    strQry += " \"US_ROLE_ID\"  =:Role1,\"US_EMAIL\" =:Email1, \"US_MOBILE_NO\" =:MobileNo1, \"US_PHONE_NO\" =:PhoneNo1,";
                    strQry += " \"US_ADDRESS\" =:Address1,\"US_DESG_ID\" =:Designation1";

                    if (objUser.sSignImage != null)
                    {
                        strQry += " , \"US_SIGN_IMAGE\" =':Photo' ";
                    }
                    NpgsqlCommand.Parameters.AddWithValue("UsId", Convert.ToInt32(objUser.lSlNo));
                    strQry += " WHERE \"US_ID\" =:UsId";
                    objCon.ExecuteQry(strQry, NpgsqlCommand);
                    Arr[0] = "Updated Successfully";
                    Arr[1] = "1";
                    return Arr;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
                return Arr;
            }

        }
        /// <summary>
        /// Load User Grid
        /// </summary>
        /// <param name="objuser"></param>
        /// <returns></returns>
        public DataTable LoadUserGrid(clsUser objuser)
        {
            DataTable dtUserDetails = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_userdetails");
                //cmd.Parameters.AddWithValue("p_offcode", objuser.sOffCode == null ? "" : objuser.sOffCode);
                cmd.Parameters.AddWithValue("p_offcode", (objuser.sOffCode ?? ""));
                cmd.Parameters.AddWithValue("p_activeuser", (objuser.ActiveUser ?? ""));
                dtUserDetails = objCon.FetchDataTable(cmd);
                return dtUserDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
                return dtUserDetails;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objuser"></param>
        /// <returns></returns>
        public object GetUserDetails(clsUser objuser)
        {
            NpgsqlCommand = new NpgsqlCommand();
            DataTable dtUserDetails = new DataTable();
            try
            {
                string strQry = string.Empty;
                if (objuser.sRoleType == "2")
                {
                    NpgsqlCommand.Parameters.AddWithValue("slno", Convert.ToInt32(objuser.lSlNo));
                    strQry = "SELECT   '' as zone,''  as circle,'' as  subdivision,\"SM_NAME\" AS DIVISION,'' as section,\"US_ID\", ";
                    strQry += "\"US_FULL_NAME\", \"US_LG_NAME\", \"US_OFFICE_CODE\", \"US_PWD\", \"US_EMAIL\", ";
                    strQry += "\"US_MOBILE_NO\", \"US_PHONE_NO\", \"US_ROLE_ID\",\"US_ADDRESS\", \"US_USER_TYPE\", ";
                    strQry += " \"US_DESG_ID\",\"RO_TYPE\",\"RO_NAME\" FROM \"TBLUSER\" INNER JOIN \"TBLROLES\" ON \"RO_ID\"=\"US_ROLE_ID\" INNER JOIN ";
                    strQry += " \"TBLSTOREMAST\" ON \"US_OFFICE_CODE\"=CAST(\"SM_ID\" AS TEXT) WHERE \"US_ID\" =:slno";
                }
                else
                {
                    NpgsqlCommand.Parameters.AddWithValue("slno6", objuser.lSlNo);
                    strQry = "SELECT (SELECT \"ZO_NAME\"  from \"TBLZONE\" WHERE SUBSTR(cast(\"ZO_CO_ID\" as text),1,1)=SUBSTR(cast(\"US_OFFICE_CODE\" as text), ";
                    strQry += " 1,1)  LIMIT 1)\"zone\",(SELECT \"CM_CIRCLE_NAME\"  from \"TBLCIRCLE\" WHERE SUBSTR(cast(\"CM_CIRCLE_CODE\" as text),1,2)= ";
                    strQry += " SUBSTR(cast(\"US_OFFICE_CODE\" as text),1,2) LIMIT 1)circle,(SELECT  \"DIV_NAME\"";
                    strQry += "from \"TBLDIVISION\" WHERE SUBSTR(cast(\"DIV_CODE\" as text),1,3)=SUBSTR(cast(\"US_OFFICE_CODE\" as text),1,3) LIMIT 1)DIVISION, ";
                    strQry += " (SELECT  \"SD_SUBDIV_NAME\"  from \"TBLSUBDIVMAST\" WHERE ";
                    strQry += "SUBSTR(cast(\"SD_SUBDIV_CODE\" as text),1,4)=SUBSTR(cast(\"US_OFFICE_CODE\" as text),1,4) LIMIT 1)subdivision, ";
                    strQry += " (SELECT  \"OM_NAME\"  from \"TBLOMSECMAST\" WHERE SUBSTR(cast(\"OM_CODE\" as text),1,5)= ";
                    strQry += " SUBSTR(cast(\"US_OFFICE_CODE\" as text),1,5)";
                    strQry += "LIMIT 1)section,\"US_ID\", \"US_FULL_NAME\", \"US_LG_NAME\", \"US_OFFICE_CODE\", \"US_PWD\", \"US_EMAIL\", \"US_MOBILE_NO\",";
                    strQry += " \"US_PHONE_NO\", \"US_ROLE_ID\", \"US_ADDRESS\", \"US_USER_TYPE\", \"US_DESG_ID\",\"RO_TYPE\",\"RO_NAME\" FROM ";
                    strQry += " \"TBLUSER\" INNER JOIN \"TBLROLES\" ON \"RO_ID\"=\"US_ROLE_ID\" WHERE CAST(\"US_ID\" as text)=:slno6";
                }
                dtUserDetails = objCon.FetchDataTable(strQry, NpgsqlCommand);
                //dtUserDetails.Load(drUser);
                if (dtUserDetails.Rows.Count > 0)
                {
                    objuser.lSlNo = Convert.ToString(dtUserDetails.Rows[0]["US_ID"]);
                    objuser.sOfficeCode = Convert.ToString(dtUserDetails.Rows[0]["US_OFFICE_CODE"]);
                    objuser.sFullName = Convert.ToString(dtUserDetails.Rows[0]["US_FULL_NAME"]);
                    objuser.sLoginName = Convert.ToString(dtUserDetails.Rows[0]["US_LG_NAME"]);
                    objuser.sPassword = Convert.ToString(dtUserDetails.Rows[0]["US_PWD"]);
                    objuser.sRole = Convert.ToString(dtUserDetails.Rows[0]["US_ROLE_ID"]);
                    objuser.sEmail = Convert.ToString(dtUserDetails.Rows[0]["US_EMAIL"]);
                    objuser.sMobileNo = Convert.ToString(dtUserDetails.Rows[0]["US_MOBILE_NO"]);
                    objuser.sPhoneNo = Convert.ToString(dtUserDetails.Rows[0]["US_PHONE_NO"]);
                    objuser.sAddress = Convert.ToString(dtUserDetails.Rows[0]["US_ADDRESS"]);
                    objuser.sUserType = Convert.ToString(dtUserDetails.Rows[0]["US_USER_TYPE"]);
                    objuser.sDesignation = Convert.ToString(dtUserDetails.Rows[0]["US_DESG_ID"]);
                    objuser.sRoleType = Convert.ToString(dtUserDetails.Rows[0]["RO_TYPE"]);

                    objuser.rolename = Convert.ToString(dtUserDetails.Rows[0]["RO_NAME"]);
                    objuser.zone = Convert.ToString(dtUserDetails.Rows[0]["zone"]);
                    objuser.circle = Convert.ToString(dtUserDetails.Rows[0]["circle"]);
                    objuser.division = Convert.ToString(dtUserDetails.Rows[0]["DIVISION"]);
                    objuser.subdivision = Convert.ToString(dtUserDetails.Rows[0]["subdivision"]);
                    objuser.section = Convert.ToString(dtUserDetails.Rows[0]["section"]);
                }
                return objuser;
            }

            catch (Exception ex)
            {
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
                return objuser;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUser"></param>
        public void SendMailUserSuccCreate(clsUser objUser)
        {
            try
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
                strmailFormat = String.Format(strMailMsg, objUser.sFullName, objUser.sLoginName, objUser.sPassword);
                objComm.SendMail("DTLMS – User Created Successfully", objUser.sEmail, strmailFormat, objUser.sCrby);
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
                return;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUser"></param>
        public void SendSMSUserSuccCreate(clsUser objUser)
        {
            try
            {
                string strSms = string.Empty;

                clsCommunication objcomm = new clsCommunication();
                objcomm.sSMSkey = "SMStoUserSuccCreat";
                objcomm = objcomm.GetsmsTempalte(objcomm);
                strSms = String.Format(objcomm.sSMSTemplate, objUser.sFullName, objUser.sLoginName, objUser.sPassword);
                //objComm.sendSMS(strSms, objUser.sMobileNo, objUser.sFullName);
                if (objcomm.sSMSTemplateID != null && objcomm.sSMSTemplateID != "")
                {
                    objcomm.DumpSms(sMobileNo, strSms, objcomm.sSMSTemplateID, "WEB");
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
                return;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public bool ActiveDeactiveUser(clsUser objUser)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                string strQry = string.Empty;
                string sQry = string.Empty;

                NpgsqlCommand.Parameters.AddWithValue("Status", objUser.sStatus);
                NpgsqlCommand.Parameters.AddWithValue("EffectFrom", objUser.sEffectFrom);
                NpgsqlCommand.Parameters.AddWithValue("Reason", objUser.sReason);
                NpgsqlCommand.Parameters.AddWithValue("UsID", Convert.ToInt32(objUser.lSlNo));
                strQry = "UPDATE \"TBLUSER\" SET \"US_STATUS\" =:Status,";
                strQry += " \"US_EFFECT_FROM\" = TO_DATE(:EffectFrom,'dd/MM/yyyy'), \"US_REASON\" =:Reason";
                strQry += " WHERE \"US_ID\" =:UsID";
                objCon.ExecuteQry(strQry, NpgsqlCommand);

                sQry = "update \"TBLMOBILEREGISTER\" set \"MR_STATUS\" = 'A' where \"MR_REQUEST_BY\" = :UsID";
                objCon.ExecuteQry(sQry, NpgsqlCommand);
                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sRoID"></param>
        /// <returns></returns>
        public string GetRoleType(string sRoID)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                string strQry = string.Empty;
                NpgsqlCommand.Parameters.AddWithValue("roid", Convert.ToInt16(sRoID));
                strQry = "SELECT \"RO_TYPE\" FROM \"TBLROLES\" WHERE \"RO_ID\"=:roid";
                return objCon.get_value(strQry, NpgsqlCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
                return "";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public string[] UpdatePwd(clsUser objUser)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string[] Arr = new string[2];
            try
            {
                string strQry = string.Empty;
                NpgsqlCommand.Parameters.AddWithValue("Password", Genaral.EncryptPassword(objUser.sPassword));
                NpgsqlCommand.Parameters.AddWithValue("lSlNo", Convert.ToInt32(objUser.lSlNo));

                strQry = "UPDATE \"TBLUSER\" set \"US_PWD\"=:Password WHERE \"US_ID\"=:lSlNo";

                objCon.ExecuteQry(strQry, NpgsqlCommand);

                NpgsqlCommand.Parameters.AddWithValue("OTP", objUser.sOTP);
                NpgsqlCommand.Parameters.AddWithValue("lSlNo1", Convert.ToInt16(objUser.lSlNo));

                strQry = "UPDATE tblotp set otp_no=:OTP,otp_change_pwd_on=now(),otp_sent_flag='1' WHERE otp_us_id=:lSlNo1 and otp_sent_flag='0'";
                //strQry = "INSERT into tblotp (otp_us_id,otp_no,otp_cron,otp_sent_flag) values ('"+ objUser.lSlNo + "','"+ objUser.sOTP + "',now(),'1')";
                objCon.ExecuteQry(strQry, NpgsqlCommand);

                Arr[0] = "1";
                Arr[1] = "Password Changed Succesfully";
                return Arr;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
                Arr[0] = "0";
                Arr[1] = "Something went wrong";
                return Arr;
            }
        }

        public string[] UpdateProfile(clsUser objUser)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string[] ArrResult = new string[2];

            string strQry = string.Empty;
            try
            {
                NpgsqlCommand.Parameters.AddWithValue("FullName", objUser.sFullName.ToUpper());
                NpgsqlCommand.Parameters.AddWithValue("Email", objUser.sEmail);
                NpgsqlCommand.Parameters.AddWithValue("MobileNo", Convert.ToInt64(objUser.sMobileNo));
                NpgsqlCommand.Parameters.AddWithValue("truserId", Convert.ToInt32(objUser.struserId));

                strQry = "UPDATE \"TBLUSER\" SET \"US_FULL_NAME\" =:FullName,";
                strQry += " \"US_EMAIL\" =:Email,\"US_MOBILE_NO\"=:MobileNo";
                strQry += " WHERE \"US_ID\"=:truserId";
                objCon.ExecuteQry(strQry, NpgsqlCommand);
                ArrResult[0] = "Updated Successfully";
                ArrResult[1] = "1";

                return ArrResult;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
                return ArrResult;
            }
        }
        /// <summary>
        /// For the MultySelect Dropdown 
        /// add on the 26-06-2023.
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public DataTable GetUserGridDetails(clsUser Obj)
        {
            string QrySelect = string.Empty;
            string QryConcatingOffcode = string.Empty;
            string QryConcatingStoreCode = string.Empty;
            string[] QryVallist = null;
            PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationManager.AppSettings["pgSQLPassword"]));
            NpgsqlCommand cmd = new NpgsqlCommand();
            try
            {
                if ((Obj.sOffCode ?? "").Length > 0)
                {
                    QryVallist = Obj.sOffCode.Split(',');
                }

                if ((Obj.sOffCode ?? "").Length == 0)
                {
                    QrySelect = " SELECT * FROM (SELECT CASE WHEN LENGTH(\"US_OFFICE_CODE\") >= 3 THEN ";
                    QrySelect += " CAST((SELECT \"ZO_NAME\"  from \"TBLZONE\" WHERE ";
                    QrySelect += " SUBSTR(cast(\"ZO_CO_ID\" as text),1,1)=SUBSTR(cast(\"US_OFFICE_CODE\" as text),1,1)  LIMIT 1) AS TEXT) ";
                    //NEWLY ADDED 2 LINES
                    QrySelect += " WHEN LENGTH(\"US_OFFICE_CODE\") <= 3 and  \"RO_TYPE\" not in (5,2)  THEN  CAST((SELECT \"ZO_NAME\"  ";
                    QrySelect += " from \"TBLZONE\" WHERE  SUBSTR(cast(\"ZO_ID\" as text),1,1)=SUBSTR(cast(\"US_OFFICE_CODE\" as text),1,1)  LIMIT 1) AS TEXT) ";
                    
                    QrySelect += " ELSE CAST((SELECT \"ZO_NAME\"  from \"TBLZONE\" WHERE SUBSTR(cast(\"ZO_CO_ID\" as text),1,1)=SUBSTR(cast((SELECT ";
                    QrySelect += " \"STO_OFF_CODE\" from \"TBLSTOREOFFCODE\" WHERE Cast(\"STO_SM_ID\" as TEXT) = \"US_OFFICE_CODE\") ";
                    QrySelect += " as text),1,1)  LIMIT 1) AS TEXT) END as \"zone\", CASE WHEN LENGTH(\"US_OFFICE_CODE\") >= 3 ";
                    QrySelect += " THEN CAST((SELECT \"CM_CIRCLE_NAME\"  from \"TBLCIRCLE\" WHERE SUBSTR(cast(\"CM_CIRCLE_CODE\" ";
                    QrySelect += " as text),1,2)=SUBSTR(cast(\"US_OFFICE_CODE\" as text),1,2) LIMIT 1) AS TEXT) ";
                    //NEWLY ADDED 2 LINES
                    QrySelect += " WHEN LENGTH(\"US_OFFICE_CODE\") <= 3 and \"RO_TYPE\" not in (5,2)THEN CAST((SELECT \"CM_CIRCLE_NAME\"  from ";
                    QrySelect += " \"TBLCIRCLE\" WHERE SUBSTR(cast(\"CM_CIRCLE_CODE\"  as text),1,2)=SUBSTR(cast(\"US_OFFICE_CODE\" as text),1,2) LIMIT 1) AS TEXT) ";


                    QrySelect += " ELSE CAST((SELECT \"CM_CIRCLE_NAME\"  from \"TBLCIRCLE\" WHERE SUBSTR(cast(\"CM_CIRCLE_CODE\" ";
                    QrySelect += " as text),1,2)=SUBSTR(cast(( SELECT \"STO_OFF_CODE\" from \"TBLSTOREOFFCODE\" ";
                    QrySelect += " WHERE Cast(\"STO_SM_ID\" as TEXT) = \"US_OFFICE_CODE\" ) as text),1,2) LIMIT 1) AS TEXT) END as \"circle\", ";
                    QrySelect += " CASE WHEN LENGTH(\"US_OFFICE_CODE\") >= 3 THEN CAST((SELECT  \"DIV_NAME\" ";
                    QrySelect += " from \"TBLDIVISION\" WHERE SUBSTR(cast(\"DIV_CODE\" as text),1,3)=SUBSTR(cast(\"US_OFFICE_CODE\" ";
                    QrySelect += " as text),1,3)  LIMIT 1) AS TEXT)	";
                    //NEWLY ADDED 2 LINES
                    QrySelect += " WHEN LENGTH(\"US_OFFICE_CODE\") <= 3 and \"RO_TYPE\" not in (5,2) THEN CAST((SELECT  \"DIV_NAME\"  from ";
                    QrySelect += " \"TBLDIVISION\" WHERE SUBSTR(cast(\"DIV_CODE\" as text),1,3)=SUBSTR(cast(\"US_OFFICE_CODE\"  as text),1,3)  LIMIT 1) AS TEXT)	";
                    
                    QrySelect += " ELSE CAST((SELECT  \"DIV_NAME\" from \"TBLDIVISION\" WHERE SUBSTR(cast(\"DIV_CODE\" ";
                    QrySelect += " as text),1,3)=SUBSTR(cast((SELECT \"STO_OFF_CODE\" from \"TBLSTOREOFFCODE\" ";
                    QrySelect += " WHERE Cast(\"STO_SM_ID\" as TEXT) = \"US_OFFICE_CODE\" ) as text),1,3) ";
                    QrySelect += " LIMIT 1) AS TEXT) END as \"division\", CAST((SELECT  \"SD_SUBDIV_NAME\" ";
                    QrySelect += " from \"TBLSUBDIVMAST\" WHERE SUBSTR(cast(\"SD_SUBDIV_CODE\" as text),1,4)=SUBSTR(cast(\"US_OFFICE_CODE\" ";
                    QrySelect += " as text),1,4) LIMIT 1) AS TEXT)\"subdivision\", ";
                    QrySelect += " CAST((SELECT  \"OM_NAME\"  from \"TBLOMSECMAST\" WHERE SUBSTR(cast(\"OM_CODE\" ";
                    QrySelect += " as text),1,5)=SUBSTR(cast(\"US_OFFICE_CODE\" as text),1,5) LIMIT 1) AS TEXT)\"section\", ";
                    QrySelect += " CAST(\"US_ID\" AS TEXT), CAST(\"US_FULL_NAME\" AS TEXT),CAST(\"US_LG_NAME\" as TEXT), ";
                    QrySelect += " UPPER(\"DM_NAME\") AS \"US_DESG_ID\", CAST(\"US_EMAIL\" AS TEXT), CAST(\"US_MOBILE_NO\" AS TEXT), ";
                    QrySelect += " CAST(\"RO_NAME\" AS TEXT), case when CAST(\"RO_ID\" AS int) in (5,2)then ( SELECT  'STORE '||': '|| \"SM_NAME\" ";
                    QrySelect += " FROM \"TBLSTOREMAST\" WHERE CAST(\"SM_ID\" AS TEXT)=\"US_OFFICE_CODE\") else  \"OFF_NAME\" end as \"OFF_NAME\", ";
                    QrySelect += " \"US_STATUS\", CASE  WHEN TO_CHAR(\"US_EFFECT_FROM\", 'YYYYMMDD') > TO_CHAR(NOW(), 'YYYYMMDD') ";
                    QrySelect += " AND CAST(\"US_STATUS\" AS TEXT)  = 'D' THEN 'A'  ELSE  \"US_STATUS\"  END  \"US_STATUS1\" ";
                    QrySelect += " from \"TBLUSER\"  inner join \"TBLROLES\" on  \"RO_ID\" = \"US_ROLE_ID\" ";
                    QrySelect += " inner join \"TBLDESIGNMAST\" on \"DM_DESGN_ID\" = \"US_DESG_ID\" ";
                    QrySelect += " left join \"VIEW_ALL_OFFICES\"  on  CAST(\"OFF_CODE\" AS TEXT) = \"US_OFFICE_CODE\" ";
                    QrySelect += " ORDER BY \"US_ID\" DESC)A ";
                }
                else
                {
                    QrySelect = " SELECT * FROM (SELECT CASE WHEN LENGTH(\"US_OFFICE_CODE\") >= 3 THEN ";
                    QrySelect += " CAST((SELECT \"ZO_NAME\"  from \"TBLZONE\" WHERE SUBSTR(cast(\"ZO_CO_ID\" ";
                    QrySelect += " as text),1,1)=SUBSTR(cast(\"US_OFFICE_CODE\" as text),1,1)  LIMIT 1) AS TEXT) ";
                    //NEWLY ADDED 2 LINES
                    QrySelect += " WHEN LENGTH(\"US_OFFICE_CODE\") <= 3 and  \"RO_TYPE\" not in (5,2)  THEN  CAST((SELECT \"ZO_NAME\"  ";
                    QrySelect += " from \"TBLZONE\" WHERE  SUBSTR(cast(\"ZO_ID\" as text),1,1)=SUBSTR(cast(\"US_OFFICE_CODE\" as text),1,1)  LIMIT 1) AS TEXT) ";

                    QrySelect += " ELSE CAST((SELECT \"ZO_NAME\"  from \"TBLZONE\" WHERE SUBSTR(cast(\"ZO_CO_ID\" ";
                    QrySelect += " as text),1,1)=SUBSTR(cast((SELECT \"STO_OFF_CODE\" from \"TBLSTOREOFFCODE\" ";
                    QrySelect += " WHERE Cast(\"STO_SM_ID\" as TEXT) = \"US_OFFICE_CODE\") as text),1,1)  LIMIT 1) AS TEXT) END as \"zone\", ";
                    QrySelect += " CASE WHEN LENGTH(\"US_OFFICE_CODE\") >= 3 THEN CAST((SELECT \"CM_CIRCLE_NAME\"  ";
                    QrySelect += " from \"TBLCIRCLE\" WHERE SUBSTR(cast(\"CM_CIRCLE_CODE\" as text),1,2)=SUBSTR(cast(\"US_OFFICE_CODE\" ";
                    QrySelect += " as text),1,2) LIMIT 1) AS TEXT) ";
                    //NEWLY ADDED 2 LINES
                    QrySelect += " WHEN LENGTH(\"US_OFFICE_CODE\") <= 3 and \"RO_TYPE\" not in (5,2)THEN CAST((SELECT \"CM_CIRCLE_NAME\"  from ";
                    QrySelect += " \"TBLCIRCLE\" WHERE SUBSTR(cast(\"CM_CIRCLE_CODE\"  as text),1,2)=SUBSTR(cast(\"US_OFFICE_CODE\" as text),1,2) LIMIT 1) AS TEXT) ";

                    QrySelect += " ELSE CAST((SELECT \"CM_CIRCLE_NAME\"  from \"TBLCIRCLE\" WHERE SUBSTR(cast(\"CM_CIRCLE_CODE\" ";
                    QrySelect += " as text),1,2)=SUBSTR(cast(( SELECT \"STO_OFF_CODE\" from \"TBLSTOREOFFCODE\" ";
                    QrySelect += " WHERE Cast(\"STO_SM_ID\" as TEXT) = \"US_OFFICE_CODE\" ) as text),1,2) LIMIT 1) AS TEXT) END as \"circle\", ";
                    QrySelect += " CASE WHEN LENGTH(\"US_OFFICE_CODE\") >= 3 THEN CAST((SELECT  \"DIV_NAME\" from \"TBLDIVISION\" ";
                    QrySelect += " WHERE SUBSTR(cast(\"DIV_CODE\" as text),1,3)=SUBSTR(cast(\"US_OFFICE_CODE\" as text),1,3)  LIMIT 1) AS TEXT)	";
                    //NEWLY ADDED 2 LINES
                    QrySelect += " WHEN LENGTH(\"US_OFFICE_CODE\") <= 3 and \"RO_TYPE\" not in (5,2) THEN CAST((SELECT  \"DIV_NAME\"  from ";
                    QrySelect += " \"TBLDIVISION\" WHERE SUBSTR(cast(\"DIV_CODE\" as text),1,3)=SUBSTR(cast(\"US_OFFICE_CODE\"  as text),1,3)  LIMIT 1) AS TEXT)	";

                    QrySelect += " ELSE CAST((SELECT  \"DIV_NAME\" from \"TBLDIVISION\" WHERE SUBSTR(cast(\"DIV_CODE\" as text),1,3)=SUBSTR(cast((SELECT ";
                    QrySelect += " \"STO_OFF_CODE\" from \"TBLSTOREOFFCODE\" WHERE Cast(\"STO_SM_ID\" as TEXT) = \"US_OFFICE_CODE\" ) ";
                    QrySelect += " as text),1,3)  LIMIT 1) AS TEXT) END as \"division\", CAST((SELECT  \"SD_SUBDIV_NAME\"  from \"TBLSUBDIVMAST\" ";
                    QrySelect += " WHERE SUBSTR(cast(\"SD_SUBDIV_CODE\" as text),1,4)=SUBSTR(cast(\"US_OFFICE_CODE\" as text),1,4) LIMIT 1) AS TEXT)\"subdivision\", ";
                    QrySelect += " CAST((SELECT  \"OM_NAME\"  from \"TBLOMSECMAST\" WHERE SUBSTR(cast(\"OM_CODE\" as text),1,5)=SUBSTR(cast(\"US_OFFICE_CODE\" ";
                    QrySelect += " as text),1,5) LIMIT 1) AS TEXT)\"section\", CAST(\"US_ID\" AS TEXT), CAST(\"US_FULL_NAME\" AS TEXT), ";
                    QrySelect += " CAST(\"US_LG_NAME\" as TEXT), UPPER(\"DM_NAME\") AS \"US_DESG_ID\", CAST(\"US_EMAIL\" AS TEXT), ";
                    QrySelect += " CAST(\"US_MOBILE_NO\" AS TEXT), CAST(\"RO_NAME\" AS TEXT), case when CAST(\"RO_ID\" AS int) in (5,2)then  ";
                    QrySelect += " (SELECT  'STORE '||': '|| \"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE CAST(\"SM_ID\" AS TEXT)=\"US_OFFICE_CODE\") ";
                    QrySelect += " else  \"OFF_NAME\" end as \"OFF_NAME\" , \"US_STATUS\", ";
                    QrySelect += " CASE  WHEN TO_CHAR(\"US_EFFECT_FROM\", 'YYYYMMDD') > TO_CHAR(NOW(), 'YYYYMMDD') ";
                    QrySelect += " AND CAST(\"US_STATUS\" AS TEXT)  = 'D' THEN 'A'  ELSE  \"US_STATUS\"  END  \"US_STATUS1\"  ";
                    QrySelect += " from \"TBLUSER\" inner join \"TBLROLES\" on  \"RO_ID\" = \"US_ROLE_ID\" ";
                    QrySelect += " inner join \"TBLDESIGNMAST\" on \"DM_DESGN_ID\" = \"US_DESG_ID\" ";
                    QrySelect += " left join \"VIEW_ALL_OFFICES\"  on  CAST(\"OFF_CODE\" AS TEXT) = \"US_OFFICE_CODE\" where ";

                    if ((Obj.sOffCode ?? "").Length > 0)
                    {
                        QryConcatingOffcode = " ( ";
                        foreach (string officeCode in QryVallist)
                        {
                            // NpgsqlCommand.Parameters.AddWithValue("officecode2", officeCode);
                            QryConcatingOffcode += "  CAST(\"OFF_CODE\" AS TEXT) LIKE '" + officeCode + "%' OR";
                        }
                        QryConcatingOffcode = QryConcatingOffcode.Substring(0, (QryConcatingOffcode.Length - 2));
                        QryConcatingOffcode += " ) ";
                    }
                    QrySelect += QryConcatingOffcode;
                    if (Obj.FetchStore == true) // till division this logic will fetch the Data.
                    {
                        QryConcatingStoreCode = " OR (cast(\"US_OFFICE_CODE\" as text)in (SELECT cast(\"STO_SM_ID\" as text) FROM \"TBLSTOREOFFCODE\" WHERE ( ";
                        foreach (string officeCode in QryVallist)
                        {
                            QryConcatingStoreCode += " cast(\"STO_OFF_CODE\" as text) LIKE '" + officeCode + "%' OR";
                        }
                        QryConcatingStoreCode = QryConcatingStoreCode.Substring(0, (QryConcatingStoreCode.Length - 2));
                        QryConcatingStoreCode += " ))) ";
                        QrySelect += QryConcatingStoreCode;
                    }
                    QrySelect += " ORDER BY \"US_ID\" DESC)A ";
                }
                //DataFilter form the DataBase for Active or inActive users Logic
                if (Obj.ActiveUser == "1")
                {
                    QrySelect += " WHERE \"US_STATUS\" = 'D' ";
                }
                else if(Obj.ActiveUser == "0")
                {
                    QrySelect += " WHERE \"US_STATUS\" = 'A' ";
                }
                Obj.DtUserGridDetails = objCon.FetchDataTable(QrySelect, cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
            return Obj.DtUserGridDetails;
        }
    }
}


