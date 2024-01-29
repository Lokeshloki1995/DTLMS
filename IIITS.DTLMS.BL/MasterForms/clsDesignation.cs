using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using IIITS.PGSQL.DAL;
using Npgsql;
using System.Reflection;
using NpgsqlTypes;

namespace IIITS.DTLMS.BL
{
   public class clsDesignation
    {
        
        PGSqlConnection Objcon = new PGSqlConnection(Constants.Password);
        public string sDesignationId { get; set; }
        public string sDesignationName { get; set; }
        public string sDesignationDesc { get; set; }
        public string sCrby { get; set; }

        string strFormCode = "clsDesignation";

        NpgsqlCommand NpgsqlCommand;
        public string[] SaveDetails(clsDesignation objDesignation)
        {
            DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
            NpgsqlCommand = new NpgsqlCommand();
            DataTable dt = new DataTable();
            string[] iReturn = new string[3];
            string sQry = string.Empty;
            string sQryValue = string.Empty;
            int PKID = 0;
            
            try
            {
                if (objDesignation.sDesignationId == "")
                {
                    NpgsqlCommand cmd = new NpgsqlCommand("proc_loaddesignation");
                    cmd.Parameters.AddWithValue("designationname", objDesignation.sDesignationName.ToUpper());
                    dt = Objcon.FetchDataTable(cmd);

                }
                else
                {
                    //PKID = Convert.ToInt32(objDesignation.sDesignationId);
                    //NpgsqlCommand.Parameters.AddWithValue("designationName1", objDesignation.sDesignationName.ToUpper());
                    //NpgsqlCommand.Parameters.AddWithValue("designationId", Convert.ToInt32(objDesignation.sDesignationId));
                    //sQry = "SELECT \"DM_NAME\" FROM \"TBLDESIGNMAST\" WHERE UPPER(\"DM_NAME\")=:designationName1 AND \"DM_DESGN_ID\" <>:designationId";
                    PKID = Convert.ToInt32(objDesignation.sDesignationId);
                    NpgsqlCommand cmd = new NpgsqlCommand("proc_loaddesignationdetails");
                    cmd.Parameters.AddWithValue("designationname1", objDesignation.sDesignationName.ToUpper());
                    cmd.Parameters.AddWithValue("designationid", Convert.ToString(PKID));
                    sQryValue = objDatabse.StringGetValue(cmd);
                }

               // sQryValue = Objcon.get_value(sQry, NpgsqlCommand);
                if (sQryValue.Length > 0)
                {
                    iReturn[0] = "Designation Name Already Exists";
                    iReturn[1] = "2";
                }
                else
                {
                    NpgsqlCommand cmd = new NpgsqlCommand("proc_saveupdate_designation");
                    cmd.Parameters.AddWithValue("dm_desgn_id", Convert.ToString(PKID));
                    cmd.Parameters.AddWithValue("dm_name", objDesignation.sDesignationName.ToUpper());
                    cmd.Parameters.AddWithValue("dm_desc", objDesignation.sDesignationDesc);
                    cmd.Parameters.AddWithValue("dm_crby", objDesignation.sCrby);
                    cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd.Parameters.Add("op_id", NpgsqlDbType.Text);

                    cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                    cmd.Parameters["op_id"].Direction = ParameterDirection.Output;


                    iReturn[0] = "msg";
                    iReturn[1] = "op_id";
                    

                    iReturn = Objcon.Execute(cmd, iReturn, 2);
                }
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return iReturn;
        }

        public DataTable LoadDetails()
        {
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getdesigndetails");
                dt = Objcon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return dt;
        }


        public object getDesignationDetails(clsDesignation obj)
        {
            DataTable dtDesign = new DataTable();
            try
            {
                string sQry = string.Empty;
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getdesignation");
                cmd.Parameters.AddWithValue("designationid", Convert.ToInt64(obj.sDesignationId));


                dtDesign = Objcon.FetchDataTable(cmd);

                if (dtDesign.Rows.Count > 0)
                {
                    obj.sDesignationId = Convert.ToString(dtDesign.Rows[0]["DM_DESGN_ID"]);
                    obj.sDesignationName = Convert.ToString(dtDesign.Rows[0]["DM_NAME"]);
                    obj.sDesignationDesc = Convert.ToString(dtDesign.Rows[0]["DM_DESC"]);
                }
                return obj;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return obj;
        }

    }
}
