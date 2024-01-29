using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using IIITS.PGSQL.DAL;
using Npgsql;
using System.Reflection;
using NpgsqlTypes;

namespace IIITS.DTLMS.BL
{

    public class clsTransRepairer
    {

        PGSqlConnection Objcon = new PGSqlConnection(Constants.Password);

        public string RepairerId { get; set; }
        public string RepairerName { get; set; }
        public string RegisterAddress { get; set; }
        public string RepairerPhoneNo { get; set; }
        public string RepairerEmail { get; set; }
        public string RepairerType { get; set; }
        public string RepairerBlacklisted { get; set; }
        public string RepairerBlackedupto { get; set; }
        public string CommAddress { get; set; }
        public string Remarks { get; set; }
        public string sCrby { get; set; }
        public string sStatus { get; set; }
        public string sContactPerson { get; set; }
        public string sFax { get; set; }
        public string sMobileNo { get; set; }
        public string sOffCode { get; set; }
        public string sContractPeriod { get; set; }
        public string sFileName { get; set; }
        public string sRepairOffCode { get; set; }

        public string sStartDate { get; set; }
        public string sEndDate { get; set; }
        public string sExtOmNo { get; set; }
        public string sExtDate { get; set; }
        public string sExtPeriod { get; set; }
        public string sExtStartDate { get; set; }
        public string sExtEndDate { get; set; }
        public string sChkEnable { get; set; }
        public string sDWANo { get; set; }
        public string sDwaDate { get; set; }
        public string sRoletype { get; set; }
        public string TypeOfRepairer { get; set; }


        public string strFormCode = "clsTransRepairer";
        NpgsqlCommand NpgsqlCommand;


        /// <summary>
        /// for saving the data of new repairer or modified repairer detials
        /// </summary>
        /// <param name="objRepairer"></param>
        /// <returns></returns>
        public string[] SaveRepairerDetails(clsTransRepairer objRepairer)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string[] Arr = new string[3];
            try
            {
                string[] strQryVallist = null;

                if (objRepairer.sOffCode != "")
                {
                    strQryVallist = objRepairer.sOffCode.Split(',');
                }
                //NpgsqlCommand cmd = new NpgsqlCommand("proc_saveupdate_repairerdetails");
                NpgsqlCommand cmd = new NpgsqlCommand("proc_saveupdate_repairerdetails_new");
                cmd.Parameters.AddWithValue("repairer_id", objRepairer.RepairerId);
                cmd.Parameters.AddWithValue("repairer_name", objRepairer.RepairerName);
                cmd.Parameters.AddWithValue("repairer_address", objRepairer.RegisterAddress);
                cmd.Parameters.AddWithValue("repairer_phno", objRepairer.RepairerPhoneNo);
                cmd.Parameters.AddWithValue("email_id", objRepairer.RepairerEmail);
                cmd.Parameters.AddWithValue("repairer_off_code", objRepairer.sOffCode);
                cmd.Parameters.AddWithValue("repairer_black_listed", objRepairer.RepairerBlacklisted);
                cmd.Parameters.AddWithValue("repairer_blocked_upto", objRepairer.RepairerBlackedupto);
                cmd.Parameters.AddWithValue("entry_auth", objRepairer.sCrby);
                cmd.Parameters.AddWithValue("repairer_comm_address", objRepairer.CommAddress);

                cmd.Parameters.AddWithValue("contact_person", objRepairer.sContactPerson);
                cmd.Parameters.AddWithValue("contract_period", objRepairer.sContractPeriod);
                cmd.Parameters.AddWithValue("repairer_fax", sFax);
                cmd.Parameters.AddWithValue("repairer_mobile", objRepairer.sMobileNo);
                cmd.Parameters.AddWithValue("repairer_startdate", objRepairer.sStartDate);
                cmd.Parameters.AddWithValue("repairer_enddate", objRepairer.sEndDate);
                cmd.Parameters.AddWithValue("tr_dwa_no", objRepairer.sDWANo);
                cmd.Parameters.AddWithValue("tr_dwa_date", objRepairer.sDwaDate);

                cmd.Parameters.AddWithValue("et_om_no", objRepairer.sExtOmNo);
                cmd.Parameters.AddWithValue("et_date", objRepairer.sExtDate);
                cmd.Parameters.AddWithValue("et_period", objRepairer.sExtPeriod);
                cmd.Parameters.AddWithValue("et_ext_start_date", objRepairer.sExtStartDate);
                cmd.Parameters.AddWithValue("et_ext_end_date", objRepairer.sExtEndDate);
                cmd.Parameters.AddWithValue("repairer_remark", objRepairer.Remarks);
                cmd.Parameters.AddWithValue("chkenable", objRepairer.sChkEnable);
                cmd.Parameters.AddWithValue("repairer_type", objRepairer.TypeOfRepairer);

                cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd.Parameters.Add("tr_id", NpgsqlDbType.Text);


                cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                cmd.Parameters["tr_id"].Direction = ParameterDirection.Output;

                Arr[0] = "msg";
                Arr[1] = "op_id";
                Arr[2] = "tr_id";

                Arr = Objcon.Execute(cmd, Arr, 3);

                if (Arr[1] == "0" || Arr[1] == "1")
                {
                    if (strQryVallist.Length > 0)
                    {
                        foreach (string OfficeCode in strQryVallist)
                        {
                            string sQry = string.Empty;
                            if (Arr[2] != "")
                            {
                                NpgsqlCommand.Parameters.AddWithValue("arr", Arr[2]);
                                NpgsqlCommand.Parameters.AddWithValue("offcode", Convert.ToInt32(OfficeCode));
                                sQry = "DELETE FROM \"TBLTRANSREPAIREROFFCODE\" WHERE cast(\"TRO_TR_ID\" as text) =:arr and \"TRO_OFF_CODE\"=:offcode";
                                Objcon.ExecuteQry(sQry, NpgsqlCommand);
                            }
                        }

                    }

                    if (Arr[1] == "1")
                    {
                        foreach (string OfficeCode in strQryVallist)
                        {
                            string sQry = string.Empty;
                            string sQry1 = string.Empty;
                            NpgsqlCommand = new NpgsqlCommand();
                            NpgsqlCommand.Parameters.AddWithValue("Getmaxno", Objcon.Get_max_no("TRO_ID", "TBLTRANSREPAIREROFFCODE"));
                            NpgsqlCommand.Parameters.AddWithValue("arr1", Convert.ToInt32(Arr[2]));
                            NpgsqlCommand.Parameters.AddWithValue("offcode", Convert.ToInt32(OfficeCode));
                            //sQry = "INSERT INTO \"TBLTRANSREPAIREROFFCODE\" (\"TRO_ID\",\"TRO_TR_ID\", \"TRO_OFF_CODE\")";
                            //sQry += " VALUES(:Getmaxno,:arr1,:offcode)";
                            sQry = "INSERT INTO \"TBLTRANSREPAIREROFFCODE\" (\"TRO_ID\",\"TRO_TR_ID\", \"TRO_OFF_CODE\",\"TRO_START_DATE\", \"TRO_END_DATE\")";
                            //sQry += " VALUES(:Getmaxno,:arr1,:offcode,'" + objRepairer.sStartDate + "','" + objRepairer.sEndDate + "')";
                            sQry += " VALUES(:Getmaxno,:arr1,:offcode,TO_DATE('" + objRepairer.sStartDate + "', 'dd/MM/yyyy'),TO_DATE('" + objRepairer.sEndDate + "', 'dd/MM/yyyy'))";


                            Objcon.ExecuteQry(sQry, NpgsqlCommand);
                            if (objRepairer.RepairerId != "" && objRepairer.RepairerId != null)
                            {
                                NpgsqlCommand.Parameters.AddWithValue("arr1", Convert.ToInt32(Arr[2]));
                                NpgsqlCommand.Parameters.AddWithValue("offcode", Convert.ToInt32(OfficeCode));
                                sQry1 = " UPDATE \"TBLMINORREPAIRITEMRATEMASTER\" SET  \"MRI_EFFECTIVE_FROM\" = TO_DATE('" + objRepairer.sStartDate + "', 'dd/MM/yyyy') , \"MRI_EFFECTIVE_TO\" = TO_DATE('" + objRepairer.sEndDate + "', 'dd/MM/yyyy') ";
                                sQry1 += " WHERE \"MRI_DIV_ID\" IN(SELECT \"DIV_ID\" FROM \"TBLDIVISION\" WHERE cast(\"DIV_CODE\" as text) = :offcode) and \"MRI_TR_ID\" = '" + objRepairer.RepairerId + "'";

                                //  Objcon.ExecuteQry(sQry1, NpgsqlCommand);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Arr[0] = ex.Message;
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return Arr;
        }
        public void updatefilepath(string off, string repid, string[] srepfile)
        {
            try
            {
                string[] strQryVallist = null;
                if (off != "")
                {
                    strQryVallist = off.Split(',');
                }
                string strQry = string.Empty;
                NpgsqlCommand = new NpgsqlCommand();
                for (int i = 0; i < srepfile.Length; i++)
                {
                    strQry = "UPDATE \"TBLTRANSREPAIREROFFCODE\" set  \"TRO_AWARD_FILEPATH\" = '" + srepfile[i] + "' WHERE \"TRO_TR_ID\" = '" + repid + "' and \"TRO_OFF_CODE\"='" + strQryVallist[i] + "';";
                    Objcon.ExecuteQry(strQry, NpgsqlCommand);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);


            }
        }
        public DataTable LoadRepairerDetails(clsTransRepairer objRepairer)
        {
            DataTable dt = new DataTable();

            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_repairer_details");
                cmd.Parameters.AddWithValue("repairer_off_code", objRepairer.sRepairOffCode);
                //cmd.Parameters.AddWithValue("srole_type", objRepairer.sRoletype);
                dt = Objcon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public object GetRepairerDetails(clsTransRepairer objRepairer)
        {
            DataTable dtRepairerDetails = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_repaierer_details_toview");
                cmd.Parameters.AddWithValue("repairer_id", objRepairer.RepairerId);
                dtRepairerDetails = Objcon.FetchDataTable(cmd);

                if (dtRepairerDetails.Rows.Count > 0)
                {
                    objRepairer.RepairerId = Convert.ToString(dtRepairerDetails.Rows[0]["TR_ID"]);
                    objRepairer.RepairerName = Convert.ToString(dtRepairerDetails.Rows[0]["TR_NAME"]);
                    objRepairer.RegisterAddress = Convert.ToString(dtRepairerDetails.Rows[0]["TR_ADDRESS"]);
                    objRepairer.RepairerPhoneNo = Convert.ToString(dtRepairerDetails.Rows[0]["TR_PHONE"]);
                    objRepairer.RepairerEmail = Convert.ToString(dtRepairerDetails.Rows[0]["TR_EMAIL"]);
                    objRepairer.RepairerType = Convert.ToString(dtRepairerDetails.Rows[0]["TR_TYPE"]);
                    objRepairer.RepairerBlacklisted = Convert.ToString(dtRepairerDetails.Rows[0]["TR_BLACK_LISTED"]);
                    objRepairer.RepairerBlackedupto = Convert.ToString(dtRepairerDetails.Rows[0]["TR_BLACKED_UPTO"]);
                    objRepairer.CommAddress = Convert.ToString(dtRepairerDetails.Rows[0]["TR_COMM_ADDRESS"]);

                    objRepairer.sContactPerson = Convert.ToString(dtRepairerDetails.Rows[0]["TR_CONT_PERSON_NAME"]);
                    objRepairer.sFax = Convert.ToString(dtRepairerDetails.Rows[0]["TR_FAX"]);
                    objRepairer.sMobileNo = Convert.ToString(dtRepairerDetails.Rows[0]["TR_MOBILE_NO"]);
                    objRepairer.sContractPeriod = Convert.ToString(dtRepairerDetails.Rows[0]["TR_CONTRACT_PERIOD"]);
                    objRepairer.sOffCode = Convert.ToString(dtRepairerDetails.Rows[0]["TR_OFFICECODE"]);
                    objRepairer.sStartDate = Convert.ToString(dtRepairerDetails.Rows[0]["TR_CON_STR_DATE"]);
                    objRepairer.sEndDate = Convert.ToString(dtRepairerDetails.Rows[0]["TR_CON_END_DATE"]);
                    objRepairer.sDWANo = Convert.ToString(dtRepairerDetails.Rows[0]["TR_DWA_NO"]);
                    objRepairer.sDwaDate = Convert.ToString(dtRepairerDetails.Rows[0]["TR_DWA_DATE"]);
                    objRepairer.Remarks = Convert.ToString(dtRepairerDetails.Rows[0]["TR_REMARKS"]);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return objRepairer;
        }
        public object GetRepairerDetailsnew(clsTransRepairer objRepairer)
        {
            DataTable dtRepairerDetails = new DataTable();
            string sQry = string.Empty;
            try
            {
                //NpgsqlCommand cmd = new NpgsqlCommand("proc_get_repaierer_details");
                //cmd.Parameters.AddWithValue("repairer_id", objRepairer.RepairerId);
                //cmd.Parameters.AddWithValue("sOffCode", objRepairer.sOffCode);

                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Parameters.AddWithValue("repairer_id", objRepairer.RepairerId);
                NpgsqlCommand.Parameters.AddWithValue("sOffCode", objRepairer.sOffCode);
                sQry = "SELECT CAST(TO_CHAR(ENDDATE,'DD-MM-YYYY') AS TEXT) AS \"MRI_EFFECTIVE_TO\" ,CAST(\"TR_ID\" AS TEXT),CAST(\"TR_NAME\" AS TEXT),CAST(\"TR_ADDRESS\" AS TEXT),CAST(\"TR_PHONE\" AS TEXT), ";
                sQry += " CAST(\"TR_EMAIL\" AS TEXT),CAST(\"TR_TYPE\" AS TEXT),CAST(\"TR_BLACK_LISTED\" AS TEXT),CAST(\"TR_COMM_ADDRESS\" AS TEXT),CAST(\"TR_CONTRACT_PERIOD\" AS TEXT), ";
                sQry += " CAST(TO_CHAR(\"TR_BLACKED_UPTO\",'DD-MM-YYYY') AS TEXT) \"TR_BLACKED_UPTO\",CAST(\"TR_CONT_PERSON_NAME\" AS TEXT),CAST(\"TR_FAX\" AS TEXT),CAST(\"TR_MOBILE_NO\" AS TEXT), ";
                sQry += " CAST(TO_CHAR(\"TR_CON_STR_DATE\",'DD-MM-YYYY') AS TEXT) \"TR_CON_STR_DATE\",CAST(\"TR_DWA_NO\" AS TEXT),CAST(TO_CHAR(\"TR_DWA_DATE\",'DD-MM-YYYY') AS TEXT) TR_DWA_DATE,CAST(\"TR_REMARKS\" AS TEXT),CAST(\"TR_MEGA_R_FLAG\" AS TEXT) FROM ";
                sQry += " (select CASE WHEN \"TRO_END_DATE\"  IS NULL THEN   CASE WHEN   \"MRI_EFFECTIVE_TO\" is null  THEN \"TR_CON_END_DATE\" ELSE   \"MRI_EFFECTIVE_TO\" END  ELSE  \"TRO_END_DATE\"  END  AS ENDDATE , ";
                sQry += " \"TR_ID\",\"TR_NAME\",\"TR_ADDRESS\",\"TR_PHONE\",\"TR_EMAIL\",\"TR_TYPE\",\"TR_BLACK_LISTED\",\"TR_COMM_ADDRESS\",\"TR_CONTRACT_PERIOD\",\"TR_BLACKED_UPTO\",\"TR_CONT_PERSON_NAME\",\"TR_FAX\",\"TR_MOBILE_NO\",";
                sQry += " \"TR_CON_STR_DATE\",\"TR_DWA_NO\",\"TR_DWA_DATE\",\"TR_REMARKS\",\"TR_MEGA_R_FLAG\"  from  \"TBLTRANSREPAIRER\"  INNER JOIN \"TBLTRANSREPAIREROFFCODE\" ON \"TRO_TR_ID\"=\"TR_ID\" INNER JOIN  \"TBLDIVISION\"  ON ";
                sQry += " \"TRO_OFF_CODE\" = \"DIV_CODE\"  LEFT JOIN   \"TBLMINORREPAIRITEMRATEMASTER\" ON \"MRI_TR_ID\"=\"TR_ID\"    WHERE  cast(\"TR_ID\" as text) ='" + objRepairer.RepairerId + "'  AND \"TRO_OFF_CODE\"=\"DIV_CODE\"   AND ";
                sQry += " cast(\"DIV_CODE\" as text) like  :sOffCode||'%' LIMIT 1)D  ;";

                //  Objcon.ExecuteQry(sQry, NpgsqlCommand);


                dtRepairerDetails = Objcon.FetchDataTable(sQry, NpgsqlCommand);

                if (dtRepairerDetails.Rows.Count > 0)
                {
                    objRepairer.RepairerId = Convert.ToString(dtRepairerDetails.Rows[0]["TR_ID"]);
                    objRepairer.RepairerName = Convert.ToString(dtRepairerDetails.Rows[0]["TR_NAME"]);
                    objRepairer.RegisterAddress = Convert.ToString(dtRepairerDetails.Rows[0]["TR_ADDRESS"]);
                    objRepairer.RepairerPhoneNo = Convert.ToString(dtRepairerDetails.Rows[0]["TR_PHONE"]);
                    objRepairer.RepairerEmail = Convert.ToString(dtRepairerDetails.Rows[0]["TR_EMAIL"]);
                    objRepairer.RepairerType = Convert.ToString(dtRepairerDetails.Rows[0]["TR_TYPE"]);
                    objRepairer.RepairerBlacklisted = Convert.ToString(dtRepairerDetails.Rows[0]["TR_BLACK_LISTED"]);
                    objRepairer.RepairerBlackedupto = Convert.ToString(dtRepairerDetails.Rows[0]["TR_BLACKED_UPTO"]);
                    objRepairer.CommAddress = Convert.ToString(dtRepairerDetails.Rows[0]["TR_COMM_ADDRESS"]);

                    objRepairer.sContactPerson = Convert.ToString(dtRepairerDetails.Rows[0]["TR_CONT_PERSON_NAME"]);
                    objRepairer.sFax = Convert.ToString(dtRepairerDetails.Rows[0]["TR_FAX"]);
                    objRepairer.sMobileNo = Convert.ToString(dtRepairerDetails.Rows[0]["TR_MOBILE_NO"]);
                    objRepairer.sContractPeriod = Convert.ToString(dtRepairerDetails.Rows[0]["TR_CONTRACT_PERIOD"]);
                    //objRepairer.sOffCode = Convert.ToString(dtRepairerDetails.Rows[0]["TR_OFFICECODE"]);

                    objRepairer.sOffCode = objRepairer.sOffCode;
                    objRepairer.sStartDate = Convert.ToString(dtRepairerDetails.Rows[0]["TR_CON_STR_DATE"]);
                    //objRepairer.sEndDate = Convert.ToString(dtRepairerDetails.Rows[0]["TR_CON_END_DATE"]);
                    objRepairer.sEndDate = Convert.ToString(dtRepairerDetails.Rows[0]["MRI_EFFECTIVE_TO"]);

                    objRepairer.sDWANo = Convert.ToString(dtRepairerDetails.Rows[0]["TR_DWA_NO"]);
                    objRepairer.sDwaDate = Convert.ToString(dtRepairerDetails.Rows[0]["TR_DWA_DATE"]);
                    objRepairer.Remarks = Convert.ToString(dtRepairerDetails.Rows[0]["TR_REMARKS"]);
                    objRepairer.TypeOfRepairer = Convert.ToString(dtRepairerDetails.Rows[0]["TR_MEGA_R_FLAG"]);

                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return objRepairer;
        }

        public bool ActiveDeactiveRepairer(clsTransRepairer objRepairer)
        {
            string[] Arr = new string[3];
            bool bRes = false;
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_activate_deactivate_repairer");
                cmd.Parameters.AddWithValue("repairer_status", objRepairer.sStatus);
                cmd.Parameters.AddWithValue("repairer_id", objRepairer.RepairerId);

                cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);


                cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;



                Arr[0] = "msg";
                Arr[1] = "op_id";

                Arr = Objcon.Execute(cmd, Arr, 2);
                bRes = true;
            }
            catch (Exception ex)
            {
                bRes = false;
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return bRes;
        }

        public string GetRepairfilepath(string repid, string off)
        {
            try
            {
                string strQry = string.Empty;

                strQry = "SELECT \"TRO_AWARD_FILEPATH\" FROM \"TBLTRANSREPAIREROFFCODE\" WHERE \"TRO_TR_ID\" ='" + repid + "' and \"TRO_OFF_CODE\" ='" + off + "'";
                NpgsqlCommand = new NpgsqlCommand();
                // NpgsqlCommand.Parameters.AddWithValue("sRepairMasterId", Convert.ToInt32(sRepairMasterId));
                return Objcon.get_value(strQry, NpgsqlCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;

            }
        }

    }
}
