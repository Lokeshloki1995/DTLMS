using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IIITS.DTLMS.BL.POFlow
{
   public class ClsLec
    {
        PGSqlConnection Objcon = new PGSqlConnection(Constants.Password);
        public string LecId { get; set; }
        public string LecContractorName { get; set; }
        public string LecLicenceNumber { get; set; }

        public string Lecregistereddate { get; set; }
        public string Lecvalidupto { get; set; }
        public string LecGstnumber { get; set; }

        public string LecContactnumber { get; set; }
        public string Lecemail { get; set; }
        public string LecAddress { get; set; }
        public string Crby { get; set; }
        public string Status { get; set; }
        public string Upby { get; set; }

        public DataTable DtLECDetails = new DataTable("TBLLECMASTER");
        /// <summary>
        /// This method used to insert and update the lec master details
        /// </summary>
        /// <param name="ObjLecCreate"></param>
        /// <returns></returns>
        public string[] SaveLECMaster(ClsLec ObjLecCreate)
        {
            string[] Arr = new string[3];
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_saveupdate_lecmaster");
                cmd.Parameters.AddWithValue("lec_id", ObjLecCreate.LecId == null ? "" : ObjLecCreate.LecId);
                cmd.Parameters.AddWithValue("lec_contractorname", ObjLecCreate.LecContractorName);
                cmd.Parameters.AddWithValue("lec_licencenumber", ObjLecCreate.LecLicenceNumber.ToUpper());
                cmd.Parameters.AddWithValue("lec_registereddate", ObjLecCreate.Lecregistereddate);
                cmd.Parameters.AddWithValue("lec_validupto", ObjLecCreate.Lecvalidupto);
                cmd.Parameters.AddWithValue("lec_gstnumber", ObjLecCreate.LecGstnumber);
                cmd.Parameters.AddWithValue("lec_contactnumber", ObjLecCreate.LecContactnumber.ToUpper());
                cmd.Parameters.AddWithValue("lec_email", ObjLecCreate.Lecemail);
                cmd.Parameters.AddWithValue("lec_address", ObjLecCreate.LecAddress);
                cmd.Parameters.AddWithValue("lec_crby", ObjLecCreate.Crby);

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
        /// <summary>
        /// gets the LEC Details.
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public ClsLec GetLECDetails(ClsLec Obj)
        {
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_lecdetails");
                cmd.Parameters.AddWithValue("p_status", Obj.Status);
                Obj.DtLECDetails = Objcon.FetchDataTable(cmd);
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
        /// auto checks the expery date and 
        /// up date the status to D
        /// </summary>
        public void CheckUpdateLECStatus()
        {
            try
            {
                string[] arr = new string[2];

                NpgsqlCommand cmd = new NpgsqlCommand("proc_checkupdate_lecstatus");
                cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);


                cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;

                arr[0] = "op_id";
                arr[1] = "msg";
                arr = Objcon.Execute(cmd, arr, 2);
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
        /// <summary>
        /// Checks the LEC Status and veryfie the Expireedate 
        /// with current and changes the status to "A" or "D"
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public string[] ChecKLECRecord(ClsLec Obj)
        {
            string[] OutResult = new string[2];
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_check_lec_record");
                cmd.Parameters.AddWithValue("p_lecid", Obj.LecId);                
                cmd.Parameters.AddWithValue("p_lecStatus", Obj.Status);
                cmd.Parameters.AddWithValue("p_lec_updated_by", Obj.Upby);


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
        /// <summary>
        /// This method used to fetch the lec details
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public ClsLec GetLecDetailstoupdate(ClsLec Obj)
        {
            try
            {

                DataTable dtlecdetails = new DataTable();
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getlecdetails");
                cmd.Parameters.AddWithValue("lec_id", Obj.LecId);
                dtlecdetails = Objcon.FetchDataTable(cmd);

                if (dtlecdetails.Rows.Count > 0)
                {
                    Obj.LecContractorName = dtlecdetails.Rows[0]["lm_contractor_name"].ToString();
                    Obj.LecLicenceNumber = dtlecdetails.Rows[0]["lm_number"].ToString();
                    Obj.LecGstnumber = dtlecdetails.Rows[0]["lm_gst_number"].ToString();
                    Obj.Lecregistereddate = dtlecdetails.Rows[0]["lm_registration_date"].ToString();
                    Obj.Lecvalidupto = dtlecdetails.Rows[0]["lm_valid_upto"].ToString();
                    Obj.LecContactnumber = dtlecdetails.Rows[0]["lm_contact_number"].ToString();
                    Obj.Lecemail = dtlecdetails.Rows[0]["lm_email_id"].ToString();
                    Obj.LecAddress = dtlecdetails.Rows[0]["lm_address"].ToString();
                }
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
    }
}
