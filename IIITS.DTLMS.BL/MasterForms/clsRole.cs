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
    public class clsRole
    {
        
        PGSqlConnection Objcon = new PGSqlConnection(Constants.Password);
        public string sRoleId { get; set; }
        public string sRoleName { get; set; }
        public string sRoleDesig { get; set; }
        public string RoleDesigID { get; set; }
        public string sCrby { get; set; }

        public string RoleType { get; set; }

        public string strFormCode = "clsRole";

        #region Commented 
        //public string[] SaveDetails(clsRole objRole)
        //{

        //    string[] Arr = new string[2];
        //    try
        //    {
        //        string strQry = string.Empty;
        //        if (objRole.sRoleId == "")
        //        {

        //            OleDbDataReader dr = objcon.Fetch("SELECT RO_NAME FROM TBLROLES WHERE UPPER(RO_NAME)='" + objRole.sRoleName.ToUpper() + "'");
        //            if (dr.Read())
        //            {
        //                Arr[0] = "Role Name Already Exists";
        //                Arr[1] = "2";
        //                dr.Close();
        //                return Arr;
        //            }
        //            dr.Close();

        //            string StrGetMaxNo = objcon.Get_max_no("RO_ID", "TBLROLES").ToString();
        //            strQry = "INSERT INTO TBLROLES(RO_ID,RO_NAME,RO_DESIGNATION,RO_CRBY)";
        //            strQry += "VALUES('" + StrGetMaxNo + "','" + objRole.sRoleName.ToUpper() + "','" + objRole.sRoleDesig + "','" + objRole.sCrby + "')";
        //            objcon.Execute(strQry);
        //            Arr[0] = StrGetMaxNo.ToString();
        //            Arr[1] = "0";
        //            return Arr;
        //        }
        //        else
        //        {
        //            OleDbDataReader dr = objcon.Fetch("SELECT RO_NAME FROM TBLROLES WHERE UPPER(RO_NAME)='" + objRole.sRoleName.ToUpper() + "' AND RO_ID<>'" + objRole.sRoleId + "'");
        //            if (dr.Read())
        //            {
        //                Arr[0] = "Role Name Already Exists";
        //                Arr[1] = "2";
        //                dr.Close();
        //                return Arr;
        //            }
        //            dr.Close();

        //            strQry = "UPDATE TBLROLES SET RO_NAME='" + objRole.sRoleName.ToUpper() + "',";
        //            strQry += "RO_DESIGNATION='" + objRole.sRoleDesig + "' WHERE RO_ID='" + objRole.sRoleId + "'";
        //            objcon.Execute(strQry);
        //            Arr[0] = "Updated Successfully ";
        //            Arr[1] = "1";
        //            return Arr;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, "clsRole", "SaveDetails");
        //        return Arr;
        //    }
        //}
        #endregion
        NpgsqlCommand NpgsqlCommand;
        public string[] SaveDetails(clsRole objRole)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string[] Arr = new string[3];
            string sQry = string.Empty;
            string sRoleName = string.Empty;
            try
            {
                if (objRole.sRoleId == "")
                {
                    #region inline query
                    //NpgsqlCommand.Parameters.AddWithValue("roleName", objRole.sRoleName.ToUpper());
                    //sQry = "SELECT \"RO_NAME\" FROM \"TBLROLES\" WHERE UPPER(\"RO_NAME\")=:roleName";
                    #endregion
                    DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsroles");
                    cmd.Parameters.AddWithValue("p_key", "GETROLENAME");
                    cmd.Parameters.AddWithValue("p_value", objRole.sRoleName.ToUpper());
                    cmd.Parameters.AddWithValue("p_offcode", "");
                    sRoleName = objDatabse.StringGetValue(cmd);

                }
                else
                {
                    #region inline query
                    //NpgsqlCommand.Parameters.AddWithValue("roleName1", objRole.sRoleName.ToUpper());
                    //NpgsqlCommand.Parameters.AddWithValue("roleId",Convert.ToInt32( objRole.sRoleId));
                    //sQry = "SELECT \"RO_NAME\" FROM \"TBLROLES\" WHERE UPPER(\"RO_NAME\")=:roleName1 AND \"RO_ID\" <>:roleId";
                    #endregion
                    DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsroles");
                    cmd.Parameters.AddWithValue("p_key", "GETROLENAMEBYID");
                    cmd.Parameters.AddWithValue("p_value", objRole.sRoleName.ToUpper());
                    cmd.Parameters.AddWithValue("p_offcode", objRole.sRoleId);
                    sRoleName = objDatabse.StringGetValue(cmd);

                }
                if (sRoleName.Trim() != "")
                {
                    Arr[0] = "Role Name Already Exists";
                    Arr[1] = "2";
                }
                else
                {
                    #region Old Logic

                    //NpgsqlCommand cmd = new NpgsqlCommand("proc_saveupdate_roledetails");
                    //cmd.Parameters.AddWithValue("ro_id", objRole.sRoleId);
                    //cmd.Parameters.AddWithValue("ro_name", objRole.sRoleName.ToUpper());
                    //cmd.Parameters.AddWithValue("ro_designation", objRole.sRoleDesig);
                    //cmd.Parameters.AddWithValue("ro_crby", objRole.sCrby);

                    //cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                    //cmd.Parameters.Add("op_id", NpgsqlDbType.Text);

                    //cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                    //cmd.Parameters["op_id"].Direction = ParameterDirection.Output;



                    //Arr[0] = "msg";
                    //Arr[1] = "op_id";


                    //Arr = Objcon.Execute(cmd, Arr, 2);
                    #endregion
                    NpgsqlCommand cmd = new NpgsqlCommand("proc_saveupdate_roledetails");
                    cmd.Parameters.AddWithValue("ro_id", objRole.sRoleId);
                    cmd.Parameters.AddWithValue("ro_name", objRole.sRoleName.ToUpper());
                    cmd.Parameters.AddWithValue("ro_designation", objRole.sRoleDesig);
                    cmd.Parameters.AddWithValue("ro_crby", objRole.sCrby);
                    cmd.Parameters.AddWithValue("p_ro_type", objRole.RoleType);

                    cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd.Parameters.Add("op_id", NpgsqlDbType.Text);

                    cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                    cmd.Parameters["op_id"].Direction = ParameterDirection.Output;

                    Arr[0] = "msg";
                    Arr[1] = "op_id";
                    Arr = Objcon.Execute(cmd, Arr, 2);
                }
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return Arr;
        }

        #region commented
        //public DataTable LoadDetails()
        //{
        //    DataTable dt = new DataTable();
        //    string strQry = string.Empty;
        //    try
        //    {
        //        strQry = "Select RO_ID,RO_NAME,RO_DESIGNATION from TBLROLES ORDER BY RO_ID DESC";
        //        OleDbDataReader dr = objcon.Fetch(strQry);

        //        dt.Load(dr);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, "clsRole", "LoadDetails");
        //        return dt;
        //    }
        //}
        #endregion
        public DataTable LoadDetails()
        {
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_roledetails_toview");
                dt = Objcon.FetchDataTable(cmd);
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return dt;
        }
        #region Commented
        //public object getRoleDetails(clsRole objRole)
        //{
        //    DataTable dtDetails = new DataTable();
        //    OleDbDataReader dr = null;
        //    try
        //    {

        //        String strQry = "SELECT RO_ID,RO_NAME,RO_DESIGNATION FROM TBLROLES ";
        //        strQry += " WHERE RO_ID='" + objRole.sRoleId + "'";
        //        dr = objcon.Fetch(strQry);
        //        dtDetails.Load(dr);

        //        if (dtDetails.Rows.Count > 0)
        //        {
        //            objRole.sRoleId = Convert.ToString(dtDetails.Rows[0]["RO_ID"].ToString());
        //            objRole.sRoleName = Convert.ToString(dtDetails.Rows[0]["RO_NAME"].ToString());
        //            objRole.sRoleDesig = Convert.ToString(dtDetails.Rows[0]["RO_DESIGNATION"].ToString());

        //        }
        //        return objRole;
        //    }


        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, "clsRole", "getRoleDetails");
        //        return objRole;
        //    }

        //}
        #endregion
        public clsRole getRoleDetails(clsRole objRole)
        {
            DataTable dtDetails = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_roledetails");
                cmd.Parameters.AddWithValue("role_id", objRole.sRoleId);
                dtDetails = Objcon.FetchDataTable(cmd);
                if (dtDetails.Rows.Count > 0)
                {
                    objRole.sRoleId = Convert.ToString(dtDetails.Rows[0]["RO_ID"]);
                    objRole.sRoleName = Convert.ToString(dtDetails.Rows[0]["RO_NAME"]);
                    objRole.sRoleDesig = Convert.ToString(dtDetails.Rows[0]["RO_DESIGNATION"]);
                    objRole.RoleType = Convert.ToString(dtDetails.Rows[0]["RO_TYPE"]);
                    objRole.RoleDesigID = Convert.ToString(dtDetails.Rows[0]["RO_DESIGNATION_ID"]);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return objRole;
        }
    }
}
