using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIITS.DTLMS.BL
{
  public  class clsBank
    {

        string strFormCode = "clsBank";
        public string sSlNo { get; set; }
        public string sBankCode { get; set; }
        public string sBankName { get; set; }
        public string sBankDescription { get; set; }
        public string sOfficeCode { get; set; }
        public string sStoreId { get; set; }
        public string sCrby { get; set; }
        public string sBankIncharge { get; set; }
        public string sAddress { get; set; }
        public string sEmailId { get; set; }
        public string sPhoneNo { get; set; }
        public string sMobile { get; set; }
        public string sStatus { get; set; }

        public string sEffectFrom { get; set; }
        public string sReason { get; set; }

        public string sOfficeName { get; set; }

        PGSqlConnection Objcon = new PGSqlConnection(Constants.Password);
        NpgsqlCommand NpgsqlCommand;
        public string[] SaveUpdateBankDetails(clsBank objBank)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string[] Arr = new string[3];
            try
            {

                string[] strQryVallist = null;

                if (objBank.sOfficeCode != "")
                {
                    strQryVallist = objBank.sOfficeCode.Split(',');
                }

                if (objBank.sSlNo.Length == 0)
                {
                    foreach (string OfficeCode in strQryVallist)
                    {
                        string sQry = string.Empty;
                        NpgsqlCommand.Parameters.AddWithValue("offcode", Convert.ToInt32(OfficeCode));
                        sQry = "SELECT \"BOC_ID\" FROM \"TBLBANKOFFICECODE\" WHERE \"BOC_BM_SUBDIV_CODE\"  = :offcode";
                        string sResult = Objcon.get_value(sQry, NpgsqlCommand);
                        if (sResult.Length > 0)
                        {
                            Arr[0] = "Location Already Allocated to some other Bank";
                            Arr[1] = "4";
                            return Arr;
                        }
                    }
                }
                else
                {
                    foreach (string OfficeCode in strQryVallist)
                    {
                        string sQry = string.Empty;
                        NpgsqlCommand.Parameters.AddWithValue("offcode1", Convert.ToInt32(OfficeCode));
                        NpgsqlCommand.Parameters.AddWithValue("slno", Convert.ToInt32(objBank.sSlNo));
                        sQry = "SELECT \"BOC_ID\" FROM \"TBLBANKOFFICECODE\" WHERE  \"BOC_BM_SUBDIV_CODE\" =:offcode1 AND \"BOC_BM_ID\" <> :slno";
                        string sResult = Objcon.get_value(sQry, NpgsqlCommand);
                        if (sResult.Length > 0)
                        {
                            Arr[0] = "Location Already Allocated to some other Bank";
                            Arr[1] = "4";
                            return Arr;
                        }
                    }
                }
                

                    NpgsqlCommand cmd = new NpgsqlCommand("proc_saveupdate_bank1");
                    cmd.Parameters.AddWithValue("bank_id", objBank.sSlNo);
                    //cmd.Parameters.AddWithValue("bank_code", objBank.sBankCode);
                    cmd.Parameters.AddWithValue("bank_name", objBank.sBankName);
                    cmd.Parameters.AddWithValue("bank_desc", objBank.sBankDescription);
                    cmd.Parameters.AddWithValue("bank_offcode", objBank.sOfficeCode);
                    cmd.Parameters.AddWithValue("bank_crby", objBank.sCrby);
                    cmd.Parameters.AddWithValue("bank_storeid", objBank.sStoreId);
                    cmd.Parameters.AddWithValue("bank_incharge", objBank.sBankIncharge);
                    cmd.Parameters.AddWithValue("bank_mobile", objBank.sMobile);
                    //cmd.Parameters.AddWithValue("str_phone", objBank.sPhoneNo);
                    cmd.Parameters.AddWithValue("bank_address", objBank.sAddress);
                    cmd.Parameters.AddWithValue("bank_emailid", objBank.sEmailId);

                    cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd.Parameters.Add("sd_id", NpgsqlDbType.Text);

                    cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                    cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                    cmd.Parameters["sd_id"].Direction = ParameterDirection.Output;

                    Arr[0] = "msg";
                    Arr[1] = "op_id";
                    Arr[2] = "sd_id";


                    
                    Arr = Objcon.Execute(cmd, Arr, 3);
                

                if (Arr[1] == "0" || Arr[1] == "1")
                {
                    if (strQryVallist.Length > 0)
                    {
                        string sQry = string.Empty;
                        NpgsqlCommand.Parameters.AddWithValue("arr", Convert.ToInt32(Arr[2]));
                        sQry = "DELETE FROM \"TBLBANKOFFICECODE\" WHERE \"BOC_BM_ID\" =:arr";
                        Objcon.ExecuteQry(sQry, NpgsqlCommand);
                    }

                    foreach (string OfficeCode in strQryVallist)
                    {

                        string sQry = string.Empty;
                        int sMaxNo = Convert.ToInt32(Objcon.Get_max_no("BOC_ID", "TBLBANKOFFICECODE"));
                        // NpgsqlCommand.Parameters.AddWithValue("Getmaxno",Convert.ToInt32( Objcon.Get_max_no("STO_ID", "TBLSTOREOFFCODE")));
                        // NpgsqlCommand.Parameters.AddWithValue("arr1",Convert.ToInt32(Arr[2]));
                        //NpgsqlCommand.Parameters.AddWithValue("offcode",Convert.ToInt32( OfficeCode));
                        sQry = "INSERT INTO \"TBLBANKOFFICECODE\"(\"BOC_ID\", \"BOC_BM_ID\", \"BOC_BM_SUBDIV_CODE\")";
                        sQry += " VALUES('" + sMaxNo + "','" + Convert.ToInt32(Arr[2]) + "','" + Convert.ToInt32(OfficeCode) + "')";
                        Objcon.ExecuteQry(sQry);
                      //  string sQry = string.Empty;
                      ////  NpgsqlCommand.Parameters.AddWithValue("Getmaxno", Convert.ToInt32(Objcon.Get_max_no("BOC_ID", "TBLBANKOFFICECODE")));
                      // // sQry = "SELECT MAX(\"BOC_ID\")+1 FROM \"TBLBANKOFFICECODE\"";
                      //  NpgsqlCommand.Parameters.AddWithValue("arr1", Convert.ToInt32(Arr[2]));
                      //  NpgsqlCommand.Parameters.AddWithValue("offcode", Convert.ToInt32(OfficeCode));                          
                      //  sQry = "INSERT INTO \"TBLBANKOFFICECODE\"(\"BOC_ID\", \"BOC_BM_ID\", \"BOC_BM_SUBDIV_CODE\")";
                      //  sQry += " VALUES((SELECT MAX(\"BOC_ID\")+1 FROM \"TBLBANKOFFICECODE\"),:arr1,:offcode)";
                      //  Objcon.ExecuteQry(sQry, NpgsqlCommand);
                    }
                }
                return Arr;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return Arr;
        }
        public DataTable LoadBankGrid(clsBank objBank)
        {
            DataTable dtBankDetails = new DataTable();
            try
            {
                if (objBank.sOfficeCode.Length > 3)
                {
                    objBank.sOfficeCode = objBank.sOfficeCode.Substring(0,4);
                }
                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_bankdetails");
                cmd.Parameters.AddWithValue("office_code", objBank.sOfficeCode);

                dtBankDetails = Objcon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return dtBankDetails;
        }
        public object GetBankDetails(clsBank objBank)
        {
            DataTable dtBankDetails = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_bankview_details");
                cmd.Parameters.AddWithValue("bank_id", objBank.sSlNo);

                dtBankDetails = Objcon.FetchDataTable(cmd);

                if (dtBankDetails.Rows.Count > 0)
                {
                    objBank.sSlNo = Convert.ToString(dtBankDetails.Rows[0]["BM_ID"]);
                    //objStore.sStoreCode = Convert.ToString(dtStoreDetails.Rows[0]["SM_CODE"]);
                    objBank.sBankDescription = Convert.ToString(dtBankDetails.Rows[0]["BM_DESC"]);
                    objBank.sBankName = Convert.ToString(dtBankDetails.Rows[0]["BM_NAME"]);
                    objBank.sOfficeCode = Convert.ToString(dtBankDetails.Rows[0]["BM_SUBDIV_CODE"]);

                    objBank.sBankIncharge = Convert.ToString(dtBankDetails.Rows[0]["BM_BANK_INCHARGE"]);
                    objBank.sStoreId = Convert.ToString(dtBankDetails.Rows[0]["BM_STORE_ID"]);
                    //objStore.sPhoneNo = Convert.ToString(dtStoreDetails.Rows[0]["SM_PHONENO"]);
                    objBank.sAddress = Convert.ToString(dtBankDetails.Rows[0]["BM_ADDRESS"]);
                    objBank.sEmailId = Convert.ToString(dtBankDetails.Rows[0]["BM_EMAILID"]);
                    objBank.sMobile = Convert.ToString(dtBankDetails.Rows[0]["BM_MOBILENO"]);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return objBank;
        }
        public bool ActiveDeactiveBank(clsBank objBank)
        {
            string[] Arr = new string[3];
            bool bRes = false;
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_active_deactive_bank");
                cmd.Parameters.AddWithValue("sm_status", objBank.sStatus);
                //cmd.Parameters.AddWithValue("effect_from", objBank.sEffectFrom);
               // cmd.Parameters.AddWithValue("reason", objBank.sReason);
                cmd.Parameters.AddWithValue("slno", objBank.sSlNo);

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

        public DataTable LoadOfficeDet(clsBank objBank)
        {
            NpgsqlCommand = new NpgsqlCommand();
            DataTable dtLocation = new DataTable();
            try
            {
                string strQry = string.Empty;


                strQry = "select \"OFF_CODE\",\"OFF_NAME\" FROM \"VIEW_ALL_OFFICES\" WHERE  \"OFF_NAME\" IS NOT NULL AND LENGTH(CAST(\"OFF_CODE\" AS TEXT)) = 3 ";
                if (objBank.sOfficeCode != "")
                {
                    NpgsqlCommand.Parameters.AddWithValue("offcode", objBank.sOfficeCode);
                    strQry += " AND CAST(\"OFF_CODE\" AS TEXT) LIKE :offcode||'%'";
                }
                if (objBank.sOfficeName != "")
                {
                    NpgsqlCommand.Parameters.AddWithValue("offname", objBank.sOfficeName.ToUpper());
                    strQry += " AND UPPER(CAST(\"OFF_NAME\" AS TEXT)) LIKE :offname||'%'";
                }
                strQry += " order by \"OFF_CODE\"";
                dtLocation = Objcon.FetchDataTable(strQry, NpgsqlCommand);
                return dtLocation;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtLocation;
            }
        }
    }
}
