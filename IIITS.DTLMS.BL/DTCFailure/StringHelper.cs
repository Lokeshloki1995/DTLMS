using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IIITS.PGSQL.DAL;
using System.Configuration;
using Npgsql;
using System.Data;

namespace IIITS.DTLMS.BL.DTCFailure
{
    public static class StringHelper
    {
  
        public static string GetTCCapacity( this string Tc_Code)
        {
            NpgsqlConnection con = new NpgsqlConnection();
            string strConnectionString = ConfigurationManager.ConnectionStrings["pgSQL"].ConnectionString + Decrypt(Constants.Password);
            con.ConnectionString = strConnectionString;
            if (con.State != ConnectionState.Open)
            {
                con.Open();

            }
            string Qry = "SELECT \"TC_CAPACITY\" FROM \"TBLTCMASTER\" WHERE \"TC_CODE\"='" + Tc_Code + "' ";
            NpgsqlCommand comm = new NpgsqlCommand();
            comm.CommandText = Qry;
            string sCap = Convert.ToString( comm.ExecuteNonQuery());
            con.Close();
            return sCap;
        }

    public static string Decrypt(string pwd)
    {
        string decryptpwd = string.Empty;
        UTF8Encoding encodepwd = new UTF8Encoding();
        Decoder Decode = encodepwd.GetDecoder();
        byte[] todecode_byte = Convert.FromBase64String(pwd);
        int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
        char[] decoded_char = new char[charCount];
        Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
        decryptpwd = new String(decoded_char);
        return decryptpwd;
    }

    }
}
