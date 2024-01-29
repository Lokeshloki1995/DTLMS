using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;
using System.Data.OleDb;
using System.Data;
namespace IIITS.DTLMS.BL
{
    public class clsApprovalPriority
    {
        string strFormCode = "clsApprovalPriority";
        
        PGSqlConnection objCon = new PGSqlConnection(Constants.Password);
        public string sModuleId { get; set; }
        public string sRoleId { get; set; }
        public string sPriority { get; set; }
        public DataTable dtRoles { get; set; }
        public string sApprovalId { get; set; }
        public string sCrBy { get; set; }
        public string sBOId { get; set; }

        NpgsqlCommand cmd;

        NpgsqlCommand NpgsqlCommand;


        // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY
        public DataTable GetRoleNames(clsApprovalPriority objApproval)
        {
            string strQry = string.Empty;
            DataTable dtRoleNames = new DataTable();
            try
            {
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "SELECT DISTINCT \"RO_ID\", \"RO_NAME\" FROM \"TBLROLES\",\"TBLUSERROLEMAPPING\",\"TBLBUSINESSOBJECT\" WHERE \"RO_ID\"=\"UR_ROLEID\" AND ";
                //strQry += "  \"UR_BOID\"=\"BO_ID\" AND \"BO_ID\" =:sBOId AND \"UR_ACCESSTYPE\" IN (1,2,3) ORDER BY \"RO_ID\"";
                //// strQry = "SELECT RO_NAME FROM TBLROLES,TBLUSERROLEMAPPING,TBLBUSINESSOBJECT WHERE RO_ID=UR_ROLEID AND UR_BOID=BO_ID AND BO_ID='"+objApproval.sModuleId+"'";                
                //NpgsqlCommand.Parameters.AddWithValue("sBOId", Convert.ToInt32(objApproval.sBOId));
                //dtRoleNames = objCon.FetchDataTable(strQry, NpgsqlCommand);
                //return dtRoleNames;

                //sp
                cmd = new NpgsqlCommand("sp_get_role_names");
                cmd.Parameters.AddWithValue("bo_id", objApproval.sBOId);
                dtRoleNames = objCon.FetchDataTable(cmd);
                return dtRoleNames;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtRoleNames;
            }
        }
        public DataTable LoadSavedRoles(string strModuleId)
        {
            string strQry = string.Empty;
            OleDbDataReader dr;
            DataTable dtRoleNames = new DataTable();
            try
            {
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "SELECT \"BO_ID\",\"RO_ID\",\"BO_NAME\",\"RO_NAME\",\"WM_LEVEL\" FROM \"TBLWORKFLOWMASTER\",\"TBLBUSINESSOBJECT\",\"TBLROLES\" WHERE \"WM_BOID\" =:strModuleId AND \"BO_ID\"=\"WM_BOID\" AND \"RO_ID\"=\"WM_ROLEID\"";
                //NpgsqlCommand.Parameters.AddWithValue("strModuleId", Convert.ToInt32(strModuleId));
                //dtRoleNames = objCon.FetchDataTable(strQry, NpgsqlCommand);
                //return dtRoleNames;

                //sp
                cmd = new NpgsqlCommand("sp_load_saved_roles");
                cmd.Parameters.AddWithValue("module_id", strModuleId);
                dtRoleNames = objCon.FetchDataTable(cmd);
                return dtRoleNames;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtRoleNames;
            }
        }
        
        
        public string[] SaveRoles(clsApprovalPriority objApproval)
        {
            string strQry = string.Empty;
            string[] Arr = new string[3];
            try
            {

                if (objApproval.sModuleId == "")
                {
                    objCon.BeginTransaction();
                    if (objApproval.dtRoles != null)
                    {

                        for (int i = 0; i < objApproval.dtRoles.Rows.Count; i++)
                        {
                            objApproval.sModuleId = Convert.ToString(dtRoles.Rows[i]["BO_ID"]);

                            //objApproval.sApprovalId = Convert.ToString(objCon.Get_max_no("WM_ID", "TBLWORKFLOWMASTER"));
                            //strQry = "INSERT INTO \"TBLWORKFLOWMASTER\"(\"WM_ID\",\"WM_BOID\",\"WM_ROLEID\",\"WM_LEVEL\",\"WM_CRBY\",\"WM_CRON\") VALUES(:sApprovalId,:BO_ID,:RO_ID,:WM_LEVEL,:sCrBy,NOW())";
                            //NpgsqlCommand = new NpgsqlCommand();
                            //NpgsqlCommand.Parameters.AddWithValue("sApprovalId", Convert.ToInt32(objApproval.sApprovalId));
                            //NpgsqlCommand.Parameters.AddWithValue("BO_ID", Convert.ToInt32(dtRoles.Rows[i]["BO_ID"]));
                            //NpgsqlCommand.Parameters.AddWithValue("RO_ID", Convert.ToInt32(dtRoles.Rows[i]["RO_ID"]));
                            //NpgsqlCommand.Parameters.AddWithValue("WM_LEVEL", Convert.ToInt32(dtRoles.Rows[i]["WM_LEVEL"]));
                            //NpgsqlCommand.Parameters.AddWithValue("sCrBy", Convert.ToInt32(objApproval.sCrBy));

                            //objCon.ExecuteQry(strQry, NpgsqlCommand);

                            //sp
                            NpgsqlCommand cmd1 = new NpgsqlCommand("proc_saveupdate_roles");
                            cmd1.Parameters.AddWithValue("bo_id", Convert.ToString(dtRoles.Rows[i]["BO_ID"]));
                            cmd1.Parameters.AddWithValue("ro_id", Convert.ToString(dtRoles.Rows[i]["RO_ID"]));
                            cmd1.Parameters.AddWithValue("wm_level", Convert.ToString(dtRoles.Rows[i]["WM_LEVEL"]));
                            cmd1.Parameters.AddWithValue("crby", Convert.ToString(objApproval.sCrBy));

                            cmd1.Parameters.Add("msg", NpgsqlDbType.Text);
                            cmd1.Parameters.Add("op_id", NpgsqlDbType.Text);
                            cmd1.Parameters.Add("pk_id", NpgsqlDbType.Text);

                            cmd1.Parameters["msg"].Direction = ParameterDirection.Output;
                            cmd1.Parameters["op_id"].Direction = ParameterDirection.Output;
                            cmd1.Parameters["pk_id"].Direction = ParameterDirection.Output;

                            Arr[0] = "msg";
                            Arr[1] = "op_id";
                            Arr[2] = "pk_id";

                            Arr = objCon.Execute(cmd1, Arr, 3);
                        }
                        objCon.CommitTransaction();
                        Arr[0] = "Saved Successfully";
                        Arr[1] = "0";
                        return Arr;
                    }
                    else
                    {
                        Arr[0] = "Add Roles and then Proceed";
                        Arr[1] = "0";
                        return Arr;
                    }

                }
                else
                {
                     objCon.BeginTransaction();
                    //  strQry = "DELETE FROM \"TBLWORKFLOWMASTER\" WHERE \"WM_BOID\" ='"+ objApproval.sModuleId + "'";
                    ////  NpgsqlCommand = new NpgsqlCommand();
                    // // NpgsqlCommand.Parameters.AddWithValue("sModuleId", Convert.ToInt32(objApproval.sModuleId));

                    //  objCon.ExecuteQry(strQry);

                    //sp
                    NpgsqlCommand cmd = new NpgsqlCommand("proc_delete_in_workflow");
                    cmd.Parameters.AddWithValue("module_id", objApproval.sModuleId);
                    cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                    cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                    Arr[0] = "msg";
                    Arr[1] = "op_id";
                    Arr = objCon.Execute(cmd, Arr, 2);



                    if (objApproval.dtRoles != null)
                    {
                        //deleting old records
                        for (int i = 0; i < objApproval.dtRoles.Rows.Count; i++)
                        {
                            Arr = new string[3];
                            // objApproval.sApprovalId = Convert.ToString(objCon.Get_max_no("WM_ID", "TBLWORKFLOWMASTER"));
                            //strQry = "INSERT INTO \"TBLWORKFLOWMASTER\" (\"WM_ID\",\"WM_BOID\",\"WM_ROLEID\",\"WM_LEVEL\",\"WM_CRBY\",\"WM_CRON\")";
                            //strQry += "  VALUES('" + Convert.ToInt32(objApproval.sApprovalId) + "','" + Convert.ToInt32(dtRoles.Rows[i]["BO_ID"]) + "','" + Convert.ToInt32(dtRoles.Rows[i]["RO_ID"]) + "','" + Convert.ToInt32(dtRoles.Rows[i]["WM_LEVEL"]) + "','" + Convert.ToInt32(objApproval.sCrBy) + "',NOW())";
                            //// NpgsqlCommand = new NpgsqlCommand();
                            ////  NpgsqlCommand.Parameters.AddWithValue("sApprovalId", Convert.ToInt32(objApproval.sApprovalId));
                            //// NpgsqlCommand.Parameters.AddWithValue("BO_ID", Convert.ToInt32(dtRoles.Rows[i]["BO_ID"]));
                            //// NpgsqlCommand.Parameters.AddWithValue("RO_ID", Convert.ToInt32(dtRoles.Rows[i]["RO_ID"]));
                            //// NpgsqlCommand.Parameters.AddWithValue("WM_LEVEL", Convert.ToInt32(dtRoles.Rows[i]["WM_LEVEL"]));
                            //// NpgsqlCommand.Parameters.AddWithValue("sCrBy", Convert.ToInt32(objApproval.sCrBy));
                            //objCon.ExecuteQry(strQry);


                            //sp
                            NpgsqlCommand cmd1 = new NpgsqlCommand("proc_saveupdate_roles");
                            cmd1.Parameters.AddWithValue("bo_id", Convert.ToString(dtRoles.Rows[i]["BO_ID"]));
                            cmd1.Parameters.AddWithValue("ro_id", Convert.ToString(dtRoles.Rows[i]["RO_ID"]));
                            cmd1.Parameters.AddWithValue("wm_level", Convert.ToString(dtRoles.Rows[i]["WM_LEVEL"]));
                            cmd1.Parameters.AddWithValue("crby", Convert.ToString(objApproval.sCrBy));

                            cmd1.Parameters.Add("msg", NpgsqlDbType.Text);
                            cmd1.Parameters.Add("op_id", NpgsqlDbType.Text);
                            cmd1.Parameters.Add("pk_id", NpgsqlDbType.Text);

                            cmd1.Parameters["msg"].Direction = ParameterDirection.Output;
                            cmd1.Parameters["op_id"].Direction = ParameterDirection.Output;
                            cmd1.Parameters["pk_id"].Direction = ParameterDirection.Output;

                            Arr[0] = "msg";
                            Arr[1] = "op_id";
                            Arr[2] = "pk_id";

                            Arr = objCon.Execute(cmd1, Arr, 3);

                        }
                    }
                   // objCon.CommitTransaction();
                    Arr[0] = "Updated Successfully";
                    Arr[1] = "1";
                    return Arr;
                }
            }
            catch (Exception ex)
            {
                objCon.RollBackTrans();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }
    }
}
