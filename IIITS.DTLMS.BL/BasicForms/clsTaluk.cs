using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using Npgsql;
using System.Reflection;
using NpgsqlTypes;
using IIITS.PGSQL.DAL;

namespace IIITS.DTLMS.BL
{
    public class clsTaluk
    {
        string strFormCode = "ClsTaluk";
        //CustOledbConnection objcon = new CustOledbConnection(Constants.Password);
        PGSqlConnection Objcon = new PGSqlConnection(Constants.Password);
        public string sTalukId { get; set; }
        public string sDistrictName { get; set; }
        public string sTalukCode { get; set; }
        public string sTalukName { get; set; }
        public string sButtonName { get; set; }
        
        NpgsqlCommand NpgsqlCommand;
        public string[] SaveDetails(clsTaluk objTlk)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string[] Arr = new string[3];
            string strId = string.Empty;
            string strQry = string.Empty;
            DataTable dt = new DataTable();
            try
            {
            //    NpgsqlCommand.Parameters.AddWithValue("DistrictName",Convert.ToInt32(objTlk.sDistrictName));
            //    strQry = "select \"DT_CODE\" from \"TBLDIST\" where \"DT_CODE\" = :DistrictName";
            //    string sDistcode = Objcon.get_value(strQry, NpgsqlCommand);

                DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getvalueto_generate_taluk_code");
                cmd.Parameters.AddWithValue("p_key", "GETDTCODE");
                cmd.Parameters.AddWithValue("p_value_1", objTlk.sDistrictName);
                cmd.Parameters.AddWithValue("p_value_2", "");
                string sDistcode = objDatabse.StringGetValue(cmd);

                if (objTlk.sButtonName == "Save")
                {
                    if (objTlk.sTalukId == "")
                    {
                        //insert
                         cmd = new NpgsqlCommand("proc_save_update_taluk");
                        cmd.Parameters.AddWithValue("taluk_id", strId);
                        cmd.Parameters.AddWithValue("taluk_code", objTlk.sTalukCode);
                        cmd.Parameters.AddWithValue("taluk_name", objTlk.sTalukName);
                        cmd.Parameters.AddWithValue("dis_code", sDistcode);

                        cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                        cmd.Parameters.Add("op_id", NpgsqlDbType.Text);


                        cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                        cmd.Parameters["op_id"].Direction = ParameterDirection.Output;

                        Arr[0] = "msg";
                        Arr[1] = "op_id";

                        Arr = Objcon.Execute(cmd, Arr, 2);

                    }

                    else
                    {
                        //NpgsqlCommand.Parameters.AddWithValue("DistrictName1", Convert.ToInt32(objTlk.sDistrictName));
                        //strId = "SELECT * from \"TBLDIST\" where \"DT_CODE\" = :DistrictName1";
                        //dt = Objcon.FetchDataTable(strId, NpgsqlCommand);

                        cmd = new NpgsqlCommand("proc_get_distict_details");
                        cmd.Parameters.AddWithValue("dist_code", objTlk.sDistrictName);
                        dt = Objcon.FetchDataTable(cmd);

                        string sDistCode = Convert.ToString(dt.Rows[0]["DT_CODE"]);

                        //NpgsqlCommand.Parameters.AddWithValue("TalukCode", Convert.ToInt32(objTlk.sTalukCode.ToUpper()));
                        //NpgsqlCommand.Parameters.AddWithValue("DistCode", Convert.ToInt32(sDistCode));
                        //strId = "SELECT * FROM \"TBLTALQ\" WHERE \"TQ_CODE\" = :TalukCode and \"TQ_DT_ID\" = :DistCode";
                        //dt = Objcon.FetchDataTable(strId, NpgsqlCommand);

                        cmd = new NpgsqlCommand("proc_get_taluq_details");
                        cmd.Parameters.AddWithValue("TalukCode", Convert.ToInt32(objTlk.sTalukCode.ToUpper()));
                        cmd.Parameters.AddWithValue("DistCode", Convert.ToInt32(sDistCode));
                        dt = Objcon.FetchDataTable(cmd);


                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["TQ_NAME"].ToString() == objTlk.sTalukName.Trim().Replace(" ", ""))
                            {
                                Arr[0] = "Taluk Name Exist ";
                                Arr[1] = "1";

                            }
                            else
                            {
                                //update
                                 cmd = new NpgsqlCommand("proc_save_update_taluk");
                                cmd.Parameters.AddWithValue("taluk_id", objTlk.sTalukId);
                                cmd.Parameters.AddWithValue("taluk_code", objTlk.sTalukCode);
                                cmd.Parameters.AddWithValue("taluk_name", objTlk.sTalukName);
                                cmd.Parameters.AddWithValue("dis_code", sDistcode);
                                cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);


                                cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;

                                Arr[0] = "msg";
                                Arr[1] = "op_id";

                                Arr = Objcon.Execute(cmd, Arr, 2);

                            }
                        }

                        else
                        {
                            //insert
                             cmd = new NpgsqlCommand("proc_save_update_taluk");
                            cmd.Parameters.AddWithValue("taluk_id", strId);
                            cmd.Parameters.AddWithValue("taluk_code", objTlk.sTalukCode);
                            cmd.Parameters.AddWithValue("taluk_name", objTlk.sTalukName);
                            cmd.Parameters.AddWithValue("dis_code", sDistcode);
                            cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                            cmd.Parameters.Add("op_id", NpgsqlDbType.Text);


                            cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                            cmd.Parameters["op_id"].Direction = ParameterDirection.Output;

                            Arr[0] = "Created new taluk code and name";
                            Arr[1] = "op_id";

                            Arr = Objcon.Execute(cmd, Arr, 2);


                        }
                    }
                }


                else
                {
                    //update
                     cmd = new NpgsqlCommand("proc_save_update_taluk");
                    cmd.Parameters.AddWithValue("taluk_id", objTlk.sTalukId);
                    cmd.Parameters.AddWithValue("taluk_code", objTlk.sTalukCode);
                    cmd.Parameters.AddWithValue("taluk_name", objTlk.sTalukName);
                    cmd.Parameters.AddWithValue("dis_code", sDistcode);

                    cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd.Parameters.Add("op_id", NpgsqlDbType.Text);


                    cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                    cmd.Parameters["op_id"].Direction = ParameterDirection.Output;

                    Arr[0] = "msg";
                    Arr[1] = "op_id";
                    Arr = Objcon.Execute(cmd, Arr, 2);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return Arr;
        }

        public object GetTlkDetails(clsTaluk objTaluk)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string strQry = string.Empty;
            string sDistId = string.Empty;
            DataTable dtDetails = new DataTable();
            try
            {
                //NpgsqlCommand.Parameters.AddWithValue("TalukId",Convert.ToInt32( objTaluk.sTalukId));
                //strQry = "SELECT * FROM \"TBLTALQ\" WHERE \"TQ_SLNO\"= :TalukId";
                //dtDetails = Objcon.FetchDataTable(strQry, NpgsqlCommand);

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_taluq_details_by_slno");
                cmd.Parameters.AddWithValue("TalukId", Convert.ToInt32(objTaluk.sTalukId));
                dtDetails = Objcon.FetchDataTable(cmd);

                if (dtDetails.Rows.Count > 0)
                {
                    sDistId = Convert.ToString(dtDetails.Rows[0]["TQ_DT_ID"]);
                    objTaluk.sTalukCode = Convert.ToString(dtDetails.Rows[0]["TQ_CODE"]);
                    objTaluk.sTalukName = Convert.ToString(dtDetails.Rows[0]["TQ_NAME"]);
                    objTaluk.sDistrictName = Convert.ToString(dtDetails.Rows[0]["TQ_DT_ID"]);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return objTaluk;
        }

        public DataTable LoadAllTalkDetails()
        {
            string strQry = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                //strQry = string.Empty;
                //strQry = "SELECT \"TQ_SLNO\",CAST(\"TQ_CODE\" AS TEXT),\"TQ_NAME\",\"DT_NAME\" FROM \"TBLTALQ\",\"TBLDIST\" WHERE \"DT_CODE\"=\"TQ_DT_ID\" ORDER BY \"TQ_SLNO\"";
                //dt = Objcon.FetchDataTable(strQry);

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_all_taluq_details");
                dt = Objcon.FetchDataTable(cmd);

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return dt;
        }
     
        public string GenerateTalukCode(clsTaluk objtaluk)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string sDisCodeNo = string.Empty;
            try
            {
                //NpgsqlCommand.Parameters.AddWithValue("DistName", objtaluk.sDistrictName);
                //sDisCodeNo = Objcon.get_value(" SELECT SUBSTR(CAST(MAX(\"TQ_CODE\")AS TEXT),2,2) FROM \"TBLTALQ\"  where CAST(\"TQ_DT_ID\" AS TEXT) like :DistName||'%'", NpgsqlCommand);

                DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getvalueto_generate_taluk_code");
                cmd.Parameters.AddWithValue("p_key", "GETTALUKCODE1");
                cmd.Parameters.AddWithValue("p_value_1", objtaluk.sDistrictName);
                cmd.Parameters.AddWithValue("p_value_2", "");
                sDisCodeNo = objDatabse.StringGetValue(cmd);


                if (isNumber(sDisCodeNo))
                {
                    //NpgsqlCommand.Parameters.AddWithValue("DistName1", objtaluk.sDistrictName);
                    //sDisCodeNo = Objcon.get_value(" SELECT COALESCE(CAST(MAX(\"TQ_CODE\")AS INT) ,0)+1 FROM \"TBLTALQ\"  where CAST(\"TQ_DT_ID\" AS TEXT) like :DistName1||'%'", NpgsqlCommand);

                    cmd = new NpgsqlCommand("sp_getvalueto_generate_taluk_code");
                    cmd.Parameters.AddWithValue("p_key", "GETTALUKCODE2");
                    cmd.Parameters.AddWithValue("p_value_1", objtaluk.sDistrictName);
                    cmd.Parameters.AddWithValue("p_value_2", "");
                    sDisCodeNo = objDatabse.StringGetValue(cmd);

                    if (Convert.ToInt16(sDisCodeNo) <= 1)
                    {
                        sDisCodeNo = objtaluk.sDistrictName + "1";
                        return sDisCodeNo;
                    }
                    else
                    {
                        //NpgsqlCommand.Parameters.AddWithValue("DistName12", objtaluk.sDistrictName);
                        //sDisCodeNo = Objcon.get_value(" SELECT SUBSTR(CAST(MAX(\"TQ_CODE\")AS TEXT),2,2) FROM \"TBLTALQ\"  where CAST(\"TQ_DT_ID\" AS TEXT) like :DistName12||'%'", NpgsqlCommand);

                        cmd = new NpgsqlCommand("sp_getvalueto_generate_taluk_code");
                        cmd.Parameters.AddWithValue("p_key", "GETTALUKCODE1");
                        cmd.Parameters.AddWithValue("p_value_1", objtaluk.sDistrictName);
                        cmd.Parameters.AddWithValue("p_value_2", "");
                        sDisCodeNo = objDatabse.StringGetValue(cmd);
                    }

                    if (Convert.ToInt16(sDisCodeNo) < 9)
                    {
                        //NpgsqlCommand.Parameters.AddWithValue("DistNm12", objtaluk.sDistrictName);
                        //sDisCodeNo = Objcon.get_value(" SELECT COALESCE(CAST(MAX(\"TQ_CODE\")AS INT) ,0)+1 FROM \"TBLTALQ\"  where CAST(\"TQ_DT_ID\" AS TEXT) like :DistNm12||'%'", NpgsqlCommand);

                        cmd = new NpgsqlCommand("sp_getvalueto_generate_taluk_code");
                        cmd.Parameters.AddWithValue("p_key", "GETTALUKCODE2");
                        cmd.Parameters.AddWithValue("p_value_1", objtaluk.sDistrictName);
                        cmd.Parameters.AddWithValue("p_value_2", "");
                        sDisCodeNo = objDatabse.StringGetValue(cmd);
                    }
                    else if (isNumber(sDisCodeNo) && Convert.ToInt16(sDisCodeNo) == 9)
                    {
                        char AsciiChar = Convert.ToChar(65);
                        sDisCodeNo = objtaluk.sDistrictName + AsciiChar;
                    }
                    else
                    {
                        int AsciiInt = Convert.ToInt32(sDisCodeNo);
                        AsciiInt = AsciiInt + 1;
                        char AsciiChar = Convert.ToChar(AsciiInt);
                        sDisCodeNo = objtaluk.sDistrictName + AsciiChar;
                    }
                }
                else
                {
                    //NpgsqlCommand.Parameters.AddWithValue("distNm", objtaluk.sDistrictName);
                    //sDisCodeNo = Objcon.get_value(" SELECT SUBSTR(CAST(MAX(\"TQ_CODE\")AS TEXT),2,2) FROM \"TBLTALQ\"  where CAST(\"TQ_DT_ID\" AS TEXT) like :distNm||'%'", NpgsqlCommand);

                    cmd = new NpgsqlCommand("sp_getvalueto_generate_taluk_code");
                    cmd.Parameters.AddWithValue("p_key", "GETTALUKCODE1");
                    cmd.Parameters.AddWithValue("p_value_1", objtaluk.sDistrictName);
                    cmd.Parameters.AddWithValue("p_value_2", "");
                    sDisCodeNo = objDatabse.StringGetValue(cmd);

                    char AsciiChar = Convert.ToChar(sDisCodeNo);
                    int AsciiInt = (int)(AsciiChar);
                    AsciiInt = AsciiInt + 1;
                    AsciiChar = Convert.ToChar(AsciiInt);
                    sDisCodeNo = objtaluk.sDistrictName + AsciiChar;

                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return sDisCodeNo;
        }

        static bool isNumber(string s)
        {
            for (int i = 0; i < s.Length; i++)
                if (char.IsDigit(s[i]) == false)
                    return false;

            return true;
        }
    }
}
