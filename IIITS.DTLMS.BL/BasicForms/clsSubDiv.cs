using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using Npgsql;
using System.Reflection;
using NpgsqlTypes;
using IIITS.PGSQL.DAL;
using IIITS.DTLMS.BL.DataBase;

namespace IIITS.DTLMS.BL
{
   public class clsSubDiv
    {
       string strQry = string.Empty;
       string strFormCode = "clsSubDiv";
       PGSqlConnection Objcon = new PGSqlConnection(Constants.Password);
       public string sDivCode { get; set; }

       public string[] SaveUpdateSubDivisionDetails(string strSubDivID, string strDivCode, string strSubDivCode, string strName, string strHead, string strMobile, string strPhone, string strEmail, bool IsSave, string strUserLogged,string address)
       {
           string[] Arr = new string[2];
           try
           {
               string strId=string.Empty;
               if (IsSave)
               {
                   NpgsqlCommand cmd = new NpgsqlCommand("proc_save_subdiv");
                   cmd.Parameters.AddWithValue("subdiv_id", strId);
                   cmd.Parameters.AddWithValue("subdiv_code", strSubDivCode);
                   cmd.Parameters.AddWithValue("subdiv_name", strName.Trim().ToUpper().Replace("'", "''"));
                   cmd.Parameters.AddWithValue("div_code", strDivCode);
                   cmd.Parameters.AddWithValue("subdiv_head", strHead.Trim().ToUpper());
                   cmd.Parameters.AddWithValue("subdiv_mobileno", strMobile);
                   cmd.Parameters.AddWithValue("subdiv_phone", strPhone);
                   cmd.Parameters.AddWithValue("subdiv_email", strEmail);
                   cmd.Parameters.AddWithValue("userlogged", strUserLogged);
                   cmd.Parameters.AddWithValue("subdiv_address", address);
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
                   NpgsqlCommand cmd = new NpgsqlCommand("proc_update_subdiv");
                   cmd.Parameters.AddWithValue("subdiv_id", strSubDivID);
                   cmd.Parameters.AddWithValue("subdiv_code", strSubDivCode);
                   cmd.Parameters.AddWithValue("subdiv_name", strName.Trim().ToUpper().Replace("'", "''"));
                   cmd.Parameters.AddWithValue("div_code", strDivCode);
                   cmd.Parameters.AddWithValue("subdiv_head", strHead.Trim().ToUpper());
                   cmd.Parameters.AddWithValue("subdiv_mobileno", strMobile);
                   cmd.Parameters.AddWithValue("subdiv_phone", strPhone);
                   cmd.Parameters.AddWithValue("subdiv_email", strEmail);
                   cmd.Parameters.AddWithValue("userlogged", strUserLogged);
                   cmd.Parameters.AddWithValue("subdiv_address", address);
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
       NpgsqlCommand NpgsqlCommand;
        public DataTable LoadSubDivOffDet(string strSubDivID = "")
        {
            NpgsqlCommand = new NpgsqlCommand();
            DataTable DtDivOffDet = new DataTable();
            try
            {
                //strQry = string.Empty;
                //strQry = "SELECT \"SD_ID\",CAST(\"SD_SUBDIV_CODE\" AS TEXT)\"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\",\"DIV_NAME\",\"SD_HEAD_EMP\",";
                //strQry+="\"SD_DIV_CODE\",\"SD_TQ_ID\",\"SD_PHONE\",\"SD_MOBILE\",\"SD_EMAIL\",\"CM_CIRCLE_NAME\", \"SD_ADDRESS\" FROM \"TBLDIVISION\",";
                //strQry += "\"TBLSUBDIVMAST\",\"TBLCIRCLE\" WHERE \"SD_DIV_CODE\"=\"DIV_CODE\"  AND \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\" ";
                //if (strSubDivID != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("subDivId", strSubDivID);
                //    strQry += "AND cast(\"SD_ID\" as text) like :subDivId||'%'";
                //}
                //strQry += "ORDER BY \"SD_SUBDIV_CODE\"";
                //DtDivOffDet = Objcon.FetchDataTable(strQry, NpgsqlCommand);

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_subdiv_details");
                cmd.Parameters.AddWithValue("subdiv_id", strSubDivID);
                DtDivOffDet = Objcon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return DtDivOffDet;
        }
        DataBseConnection ObjBasCon = new DataBseConnection(Constants.Password);

        public string GenerateSubDivCode(clsSubDiv objSubDivision)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string sSubDivCode = string.Empty;
            try
            {
                #region inline query
                //NpgsqlCommand.Parameters.AddWithValue("subDivId1", objSubDivision.sDivCode);
                //sSubDivCode = Objcon.get_value("SELECT COALESCE(MAX(\"SD_SUBDIV_CODE\"),0)+1 FROM \"TBLSUBDIVMAST\"  where cast(\"SD_DIV_CODE\" as text) = :subDivId1", NpgsqlCommand);
                #endregion
                NpgsqlCommand cmd = new NpgsqlCommand("proc_generate_subdiv_code");
                cmd.Parameters.AddWithValue("div_code", Convert.ToString(objSubDivision.sDivCode ?? ""));
                sSubDivCode = ObjBasCon.StringGetValue(cmd);

                if (Convert.ToInt16(sSubDivCode) <= 1)
                {
                    sSubDivCode = objSubDivision.sDivCode + "1";
                }
            }
            catch (Exception ex)
             {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return sSubDivCode;
        }

    }
}
