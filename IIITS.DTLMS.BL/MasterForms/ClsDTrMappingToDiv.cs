using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIITS.DTLMS.BL.MasterForms
{
    public class ClsDTrMappingToDiv
    {
        string strFormCode = "clsDtcMapping";

        PGSqlConnection Objcon = new PGSqlConnection(Constants.Password);
        public string OffCode { get; set; }
        public string OfficeName { get; set; }
        public string MappingId { get; set; }
        public string TcMakeId { get; set; }
        public string TcSlNo { get; set; }
        public string Startrange { get; set; }
        public string Endrange { get; set; }
        public string Quantity { get; set; }
        public string CrBy { get; set; }

        NpgsqlCommand NpgsqlCommand;
        /// <summary>
        /// to check dtr's already exist in DB and save
        /// </summary>
        /// <param name="objDtrMappingToDiv"></param>
        /// <returns></returns>
        public string[] CheckTheDtrExistied(ClsDTrMappingToDiv objDtrMappingToDiv)
        {
            DataTable dtExistiedDtr = new DataTable();
            string ExistedDtr = string.Empty;
            string ExistedEnumDtr = string.Empty;
            string ExistedRangeAllocateDtr = string.Empty;
            string[] Arr = new string[3];
            string status = string.Empty;
            string strQry = string.Empty;
            bool bResult = false;
            string[] Arrmsg = new string[3];
            try
            {
                int Startrange = Convert.ToInt32(objDtrMappingToDiv.Startrange.ToUpper().Trim('H'));
                int Endrange = Convert.ToInt32(objDtrMappingToDiv.Endrange.ToUpper().Trim('H'));
                string strtrng = Convert.ToString(Startrange);

                if (strtrng.Length < 6)
                {
                    switch (strtrng.Length)
                    {
                        case 5:
                            strtrng = "0" + strtrng;
                            break;
                        case 4:
                            strtrng = "00" + strtrng;
                            break;
                        case 3:
                            strtrng = "000" + strtrng;
                            break;
                        case 2:
                            strtrng = "0000" + strtrng;
                            break;
                        case 1:
                            strtrng = "00000" + strtrng;
                            break;
                    }
                }

                string endrng = Convert.ToString(Endrange);

                if (endrng.Length < 6)
                {
                    switch (endrng.Length)
                    {
                        case 5:
                            endrng = "0" + endrng;
                            break;
                        case 4:
                            endrng = "00" + endrng;
                            break;
                        case 3:
                            endrng = "000" + endrng;
                            break;
                        case 2:
                            endrng = "0000" + endrng;
                            break;
                        case 1:
                            endrng = "00000" + endrng;
                            break;
                    }
                }
                NpgsqlCommand cmdbulk = new NpgsqlCommand("sp_check_tc_bulks");
                cmdbulk.Parameters.AddWithValue("startrange", strtrng);
                cmdbulk.Parameters.AddWithValue("endrange", endrng);
                cmdbulk.Parameters.Add("tc_code", NpgsqlDbType.Text);
                cmdbulk.Parameters.Add("status", NpgsqlDbType.Text);
                cmdbulk.Parameters["tc_code"].Direction = ParameterDirection.Output;
                cmdbulk.Parameters["status"].Direction = ParameterDirection.Output;
                Arr[0] = "tc_code";
                Arr[1] = "status";
                Arr = Objcon.Execute(cmdbulk, Arr, 2);
                if (Arr[0].Length < 6)
                {
                    switch (Arr[0].Length)
                    {
                        case 5:
                            Arr[0] = "0" + Arr[0];
                            break;
                        case 4:
                            Arr[0] = "00" + Arr[0];
                            break;
                        case 3:
                            Arr[0] = "000" + Arr[0];
                            break;
                        case 2:
                            Arr[0] = "0000" + Arr[0];
                            break;
                        case 1:
                            Arr[0] = "00000" + Arr[0];
                            break;
                    }
                }

                if (Arr[1] == "-1")
                {
                    Arr[0] = "DTr Code Already Exist H" + Arr[0] + "";
                    Arr[1] = "2";
                    return Arr;
                }

                string[] strArray = objDtrMappingToDiv.OffCode.Split(',');
                string test = Newtonsoft.Json.JsonConvert.SerializeObject((strArray));
                string offcodes = test.Replace("\"", "");
                string yourString = offcodes.Replace("[", string.Empty).Replace("]", string.Empty);

                string startRange = 'H' + strtrng;
                string EndRange = 'H' + endrng;

                NpgsqlCommand cmd = new NpgsqlCommand("sp_savedtrrangeallocationsnew");
                cmd.Parameters.AddWithValue("dra_id", "");
                cmd.Parameters.AddWithValue("dra_startrange", startRange);
                cmd.Parameters.AddWithValue("dra_endrange", EndRange);
                cmd.Parameters.AddWithValue("dra_quantity", objDtrMappingToDiv.Quantity);
                cmd.Parameters.AddWithValue("dra_divcodes", yourString);
                cmd.Parameters.AddWithValue("startrange", strtrng);
                cmd.Parameters.AddWithValue("endrange", endrng);
                cmd.Parameters.AddWithValue("cr_by", objDtrMappingToDiv.CrBy);
                cmd.Parameters.Add("pk_id", NpgsqlDbType.Text);
                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd.Parameters["pk_id"].Direction = ParameterDirection.Output;
                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                cmd.Parameters["msg"].Direction = ParameterDirection.Output;

                Arrmsg[0] = "msg";
                Arrmsg[1] = "op_id";
                Arrmsg[2] = "pk_id";
                Arrmsg = Objcon.Execute(cmd, Arrmsg, 3);

                bResult = true;
                if (bResult == true)
                {
                    Arrmsg[0] = "DTr Details Saved Successfully";
                    Arrmsg[1] = "0";
                }
                else
                {
                    Arrmsg[0] = "No DTr Exists to Save";
                    Arrmsg[1] = "2";
                }
                return Arrmsg;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arrmsg;
            }
        }
        /// <summary>
        /// to load dtr's mapped to division 
        /// </summary>
        /// <returns></returns>
        public DataTable LoadTcMakeMaster()
        {
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_dtrdivisionmapping_details");
                dt = Objcon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return dt;
        }
    }
}
