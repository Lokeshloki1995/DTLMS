using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IIITS.DTLMS.BL.POFlow
{
    public class ClsDwa
    {
        PGSqlConnection Objcon = new PGSqlConnection(Constants.Password);
        public string Id { get; set; }
        public string sSlNo { get; set; }
        public string ContractorName { get; set; }
        public string DWANumber { get; set; }
        public string AwardDate { get; set; }
        public string ValidUpto { get; set; }
        public string DwaProject { get; set; }
        public string ProjectName { get; set; }
        public string AwardAmount { get; set; }
        public string Address { get; set; }
        public string Mobileno { get; set; }
        public string ActiveUser { get; set; }
        public string Status { get; set; }
        public string UpBy { get; set; }
        public string LicenceNum { get; set; }
        public string GstNum { get; set; }
        public string LicenceRegDate { get; set; }
        public string LicenceValidUpTo { get; set; }
        public string MailId { get; set; }
        public string DwaDate { get; set; }
        public string DwaPeriod { get; set; }
        public string DwaExtendedUpTo { get; set; }
        public string DwaProjectType { get; set; }
        public string DwaId { get; set; }
        public string FileName { get; set; }
        public string DwaFilePath { get; set; }
        public string ContractorId { get; set; }
        public string CrBy { get; set; }
        public string ContactNum { get; set; }

        public string DwaNum { get; set; }
        public string DwaWorkName { get; set; }
        public string DwaAmt { get; set; }
        public string Addr { get; set; }
       // public string divProjectName { get; set; }

        public DataTable DataSource { get; set; }

        public DataTable DtDWADetails = new DataTable("TBLDWAMASTER");
        /// <summary>
        /// load  DWAdetails to grid 
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        NpgsqlCommand NpgsqlCommand;


        public ClsDwa GetDWAGridDetails(ClsDwa Obj)
        {
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("public.proc_load_dwadetails");
                cmd.Parameters.AddWithValue("p_status", Convert.ToString(Obj.Status == null ? "" : Obj.Status));
                Obj.DtDWADetails = Objcon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
                throw ex;
            }
            return Obj;

        }
        /// <summary>
        /// method to check whether user should be active or In-active
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public string[] ChecldwaRecord(ClsDwa Obj)
        {
            string[] OutResult = new string[2];
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_check_dwa_record");
                cmd.Parameters.AddWithValue("p_id", Convert.ToInt64(Obj.Id));
                cmd.Parameters.AddWithValue("p_Status", Obj.Status);
                cmd.Parameters.AddWithValue("p_upby", Obj.UpBy);
                cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);


                cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;

                OutResult[0] = "op_id";
                OutResult[1] = "msg";
                OutResult = Objcon.Execute(cmd, OutResult, 2);
            }
            catch (Exception ex)
            {
                clsException.LogError(
                       MethodBase.GetCurrentMethod().DeclaringType.Name,
                       MethodBase.GetCurrentMethod().Name,
                       ex.Message,
                       ex.StackTrace);
                throw ex;
            }
            return OutResult;
        }





        public void CheckDWAStatus()
        {
            string[] OutResult = new string[2];
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_checkupdate_dwastatus");
                cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);


                cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;

                OutResult[0] = "op_id";
                OutResult[1] = "msg";
                OutResult = Objcon.Execute(cmd, OutResult, 2);
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
                throw ex;
            }


        }

        public object GetContractorDetails(ClsDwa objContractor)
        {
            try
            {
                DataTable dtContractorDetails = new DataTable();

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_contractor_details_toview");
                cmd.Parameters.AddWithValue("contractor_id", objContractor.Id);
                dtContractorDetails = Objcon.FetchDataTable(cmd);

                if (dtContractorDetails.Rows.Count > 0)
                {
                    objContractor.Id = Convert.ToString(dtContractorDetails.Rows[0]["lm_id"]);
                    objContractor.ContractorName = Convert.ToString(dtContractorDetails.Rows[0]["lm_contractor_name"]);
                    objContractor.LicenceNum = Convert.ToString(dtContractorDetails.Rows[0]["lm_number"]);
                    objContractor.GstNum = Convert.ToString(dtContractorDetails.Rows[0]["lm_gst_number"]);
                    objContractor.Address = Convert.ToString(dtContractorDetails.Rows[0]["lm_address"]);
                    objContractor.LicenceRegDate = Convert.ToString(dtContractorDetails.Rows[0]["lm_registration_date"]);
                    objContractor.LicenceValidUpTo = Convert.ToString(dtContractorDetails.Rows[0]["lm_valid_upto"]);
                    objContractor.MailId = Convert.ToString(dtContractorDetails.Rows[0]["lm_email_id"]);
                    objContractor.Mobileno = Convert.ToString(dtContractorDetails.Rows[0]["lm_contact_number"]);
                }
            }

            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return objContractor;

        }

        public object GetContractorDwaDetails(ClsDwa objContractorDwa)
        {
            try
            {
                DataTable dtContractorDwaDetails = new DataTable();

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_contractor_dwa_details_toview");
                cmd.Parameters.AddWithValue("dwa_id", objContractorDwa.DwaId);
                dtContractorDwaDetails = Objcon.FetchDataTable(cmd);

                if (dtContractorDwaDetails.Rows.Count > 0)
                {
                    objContractorDwa.Id = Convert.ToString(dtContractorDwaDetails.Rows[0]["lm_id"]);

                    objContractorDwa.ContractorName = Convert.ToString(dtContractorDwaDetails.Rows[0]["lm_contractor_name"]);
                    objContractorDwa.LicenceNum = Convert.ToString(dtContractorDwaDetails.Rows[0]["lm_number"]);
                    objContractorDwa.GstNum = Convert.ToString(dtContractorDwaDetails.Rows[0]["lm_gst_number"]);
                    objContractorDwa.Address = Convert.ToString(dtContractorDwaDetails.Rows[0]["lm_address"]);
                    objContractorDwa.LicenceRegDate = Convert.ToString(dtContractorDwaDetails.Rows[0]["lm_registration_date"]);
                    objContractorDwa.LicenceValidUpTo = Convert.ToString(dtContractorDwaDetails.Rows[0]["lm_valid_upto"]);
                    objContractorDwa.MailId = Convert.ToString(dtContractorDwaDetails.Rows[0]["lm_email_id"]);
                    objContractorDwa.Mobileno = Convert.ToString(dtContractorDwaDetails.Rows[0]["lm_contact_number"]);
                    objContractorDwa.DWANumber = Convert.ToString(dtContractorDwaDetails.Rows[0]["dm_number"]);
                    objContractorDwa.DwaDate = Convert.ToString(dtContractorDwaDetails.Rows[0]["dm_date"]);
                    objContractorDwa.DwaPeriod = Convert.ToString(dtContractorDwaDetails.Rows[0]["dm_period"]);
                    objContractorDwa.DwaExtendedUpTo = Convert.ToString(dtContractorDwaDetails.Rows[0]["dm_extended_upto"]);
                    objContractorDwa.DwaProjectType = Convert.ToString(dtContractorDwaDetails.Rows[0]["dm_prjtyp"]);
                    objContractorDwa.DwaProject = Convert.ToString(dtContractorDwaDetails.Rows[0]["dm_projectname"]);
                    objContractorDwa.DwaWorkName = Convert.ToString(dtContractorDwaDetails.Rows[0]["dm_name"]);
                   // objContractorDwa.DwaWorkName = Convert.ToString(dtContractorDwaDetails.Rows[0][""]);
                    objContractorDwa.AwardAmount = Convert.ToString(dtContractorDwaDetails.Rows[0]["dm_amount"]);

                }
            }

            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return objContractorDwa;

        }

        /// <summary>
        /// Extending DWA Contract
        /// </summary>
        /// <param name="objExtendContractor"></param>
        /// <returns></returns>
        public string[] UpdateExtendedUpTo(ClsDwa objExtendContractor)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string[] Arr = new string[2];
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_update_extended_upto");
                cmd.Parameters.AddWithValue("dwa_id", objExtendContractor.DwaId);
                cmd.Parameters.AddWithValue("dwa_extended_upto", objExtendContractor.DwaExtendedUpTo);


                cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);


                cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;

                Arr[0] = "msg";
                Arr[1] = "op_id";

                Arr = Objcon.Execute(cmd, Arr, 2);

            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return Arr;
        }

        public string[] checkIsExtended(ClsDwa Obj)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string[] Arr = new string[2];
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_is_extended_upto");
                cmd.Parameters.AddWithValue("p_dwa_id", Obj.DwaId);
                cmd.Parameters.Add("o_isextemded", NpgsqlDbType.Text);
                cmd.Parameters.Add("o_extendedupto", NpgsqlDbType.Text);
                cmd.Parameters["o_isextemded"].Direction = ParameterDirection.Output;
                cmd.Parameters["o_extendedupto"].Direction = ParameterDirection.Output;

                Arr[0] = "o_isextemded";
                Arr[1] = "o_extendedupto";

                Arr = Objcon.Execute(cmd, Arr, 2);
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return Arr;
        }
        public string[] SaveContractorDetails(ClsDwa objContractor)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string[] Arr = new string[3];
            try
            {               
                    NpgsqlCommand cmd = new NpgsqlCommand("proc_saveupdate_dwamaster");
                    cmd.Parameters.AddWithValue("dwa_id", objContractor.DwaId == null ? "" : objContractor.DwaId);
                    cmd.Parameters.AddWithValue("dwa_contractorid", objContractor.Id);
                    cmd.Parameters.AddWithValue("dwa_num", objContractor.DWANumber);
                    cmd.Parameters.AddWithValue("dwa_date", objContractor.DwaDate);
                    cmd.Parameters.AddWithValue("dwa_period", objContractor.DwaPeriod);
                    cmd.Parameters.AddWithValue("dwa_extendedupto", objContractor.DwaExtendedUpTo);
                    cmd.Parameters.AddWithValue("dwa_projecttype", objContractor.DwaProjectType);
                   cmd.Parameters.AddWithValue("dwa_workname", objContractor.ProjectName);
                    cmd.Parameters.AddWithValue("dm_projectname",objContractor.DwaProject == null ? "" : objContractor.DwaProject);
                   
                    cmd.Parameters.AddWithValue("dwa_amt", objContractor.AwardAmount);
                    cmd.Parameters.AddWithValue("dwa_crby", objContractor.UpBy);

                    cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd.Parameters.Add("p_key", NpgsqlDbType.Text);


                    cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                    cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                    cmd.Parameters["p_key"].Direction = ParameterDirection.Output;

                    Arr[0] = "msg";
                    Arr[1] = "op_id";
                    Arr[2] = "p_key";

                    Arr = Objcon.Execute(cmd, Arr, 3);
                              
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return Arr;
        }
        /// <summary>
        /// Created on 11-09-2023
        /// </summary>
        /// <param name="objContractor"></param>
        /// <returns></returns>
        public bool checkDWANumberExist(ClsDwa Obj)
        {
            bool Exist=false;
            try
            {
                string[] Arr = new string[1];
                NpgsqlCommand cmd = new NpgsqlCommand("proc_check_dwa_number_exists");
                cmd.Parameters.AddWithValue("p_dwa_num", Obj.DWANumber);
                cmd.Parameters.Add("msg", NpgsqlDbType.Boolean);
                cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                Arr[0] = "msg";
                Arr = Objcon.Execute(cmd, Arr, 1);
                Exist = Convert.ToBoolean(Arr[0]);
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return Exist;
        }
        public string[] SaveContractorFilePath(ClsDwa objContractor) // s
        {
            NpgsqlCommand = new NpgsqlCommand();
            string[] Arr = new string[2];
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_savedwafilepath");
                cmd.Parameters.AddWithValue("dwa_crby", objContractor.UpBy);
                cmd.Parameters.AddWithValue("dwa_filepath", objContractor.DwaFilePath);
                cmd.Parameters.AddWithValue("dwa_id", objContractor.DwaId);
                cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;

                Arr[0] = "msg";
                Arr[1] = "op_id";
                Arr = Objcon.Execute(cmd, Arr, 2);
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return Arr;
        }
    }
}

