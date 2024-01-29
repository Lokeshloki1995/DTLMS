using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using IIITS.PGSQL.DAL;
using Npgsql;
using System.Reflection;
using NpgsqlTypes;

namespace IIITS.DTLMS.BL
{
    public class clsTransSupplier
    {

        public string strFormCode = "clsTransSupplier";
        PGSqlConnection Objcon = new PGSqlConnection(Constants.Password);
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string RegisterAddress { get; set; }
        public string SupplierPhoneNo { get; set; }
        public string SupplierEmail { get; set; }
        public string SupplierType { get; set; }
        public string SupplierBlacklisted { get; set; }
        public string SupplierBlackedupto { get; set; }
        public string sCrby { get; set; }
        public string CommAddress { get; set; }
        public string sStatus { get; set; }
        public string sContactPerson { get; set; }
        public string sFax { get; set; }
        public string sMobileNo { get; set; }

        //public string[] SaveDetails(clsTransSupplier objSupplier)
        //{
        //    string[] Arr = new string[2];

        //    try
        //    {
        //        OleDbDataReader dr;
        //        string strQry = string.Empty;
        //        if (objSupplier.SupplierId == "")
        //        {

        //            dr = objcon.Fetch("Select * from TBLTRANSSUPPLIER  WHERE TS_STATUS='A' AND UPPER(TS_NAME)='" + objSupplier.SupplierName + "'");
        //            if (dr.Read())
        //            {
        //                dr.Close();
        //                Arr[0] = "Supplier Name already exists";
        //                Arr[1] = "4";

        //                return Arr;
        //            }
        //            dr.Close();
        //            dr = objcon.Fetch("Select * from TBLTRANSSUPPLIER  WHERE TS_STATUS='A' AND TS_PHONE='" + objSupplier.SupplierPhoneNo + "'");
        //            if (dr.Read())
        //            {
        //                dr.Close();
        //                Arr[0] = "Phone No already exists";
        //                Arr[1] = "4";

        //                return Arr;
        //            }
        //            dr.Close();

        //            dr = objcon.Fetch("Select * from TBLTRANSSUPPLIER  WHERE TS_STATUS='A' AND  TS_EMAIL='" + objSupplier.SupplierEmail + "'");
        //            if (dr.Read())
        //            {
        //                dr.Close();
        //                Arr[0] = "EmailId already exists";
        //                Arr[1] = "4";
        //                return Arr;
        //            }
        //            dr.Close();

        //            string StrGetMaxNo = objcon.Get_max_no("TS_ID", "TBLTRANSSUPPLIER").ToString();
        //            strQry = "INSERT INTO TBLTRANSSUPPLIER(TS_ID,TS_NAME,TS_ADDRESS,TS_PHONE,TS_EMAIL,";
        //            strQry += "TS_BLACK_LISTED,TS_BLACKED_UPTO,TS_ENTRY_AUTH,TS_COMM_ADDRESS,TS_CONT_PERSON_NAME,TS_FAX,TS_MOBILE_NO)";
        //            strQry += " VALUES('" + StrGetMaxNo + "','" + objSupplier.SupplierName + "',";
        //            strQry += "'" + objSupplier.RegisterAddress + "','" + objSupplier.SupplierPhoneNo + "', '" + objSupplier.SupplierEmail + "',";
        //            strQry += "'" + objSupplier.SupplierBlacklisted + "',";
        //            strQry += "TO_DATE('" + objSupplier.SupplierBlackedupto + "','dd/MM/yyyy') ,'" + objSupplier.sCrby + "','" + objSupplier.CommAddress + "',";
        //            strQry += " '"+ objSupplier.sContactPerson +"','"+ objSupplier.sFax +"','"+ objSupplier.sMobileNo +"')";
        //            objcon.Execute(strQry);
        //            Arr[0] = StrGetMaxNo.ToString();
        //            Arr[1] = "0";
        //            return Arr;

        //        }
        //        else
        //        {

        //            dr = objcon.Fetch("Select * from TBLTRANSSUPPLIER  WHERE TS_STATUS='A' AND UPPER(TS_NAME)='" + objSupplier.SupplierName + "' AND TS_ID<>'" + objSupplier.SupplierId + "'");
        //            if (dr.Read())
        //            {
        //                dr.Close();
        //                Arr[0] = "Supplier Name already exists";
        //                Arr[1] = "4";
        //                return Arr;
        //            }
        //            dr.Close();

        //            dr = objcon.Fetch("Select * from TBLTRANSSUPPLIER where TS_PHONE='" + objSupplier.SupplierPhoneNo + "' AND TS_ID<>'" + objSupplier.SupplierId + "'  AND TS_STATUS='A'");
        //            if (dr.Read())
        //            {
        //                dr.Close();
        //                Arr[0] = "Phone No already exists";
        //                Arr[1] = "4";

        //                return Arr;
        //            }
        //            dr.Close();

        //            dr = objcon.Fetch("Select * from TBLTRANSSUPPLIER  WHERE TS_STATUS='A' AND TS_EMAIL='" + objSupplier.SupplierEmail + "' AND TS_ID<>'" + objSupplier.SupplierId + "'");
        //            if (dr.Read())
        //            {
        //                dr.Close();
        //                Arr[0] = "EmailId already exists";
        //                Arr[1] = "4";
        //                return Arr;
        //            }
        //            dr.Close();

        //            strQry = "UPDATE TBLTRANSSUPPLIER SET TS_NAME='" + objSupplier.SupplierName + "',";
        //            strQry += "TS_ADDRESS='" + objSupplier.RegisterAddress + "', TS_PHONE='" + objSupplier.SupplierPhoneNo + "',";
        //            strQry += "TS_EMAIL='" + objSupplier.SupplierEmail + "',";
        //            strQry += "TS_BLACK_LISTED='" + objSupplier.SupplierBlacklisted + "',TS_COMM_ADDRESS='"+ objSupplier.CommAddress +"',";
        //            strQry += "TS_BLACKED_UPTO=TO_DATE('" + objSupplier.SupplierBlackedupto + "','dd/MM/yyyy') ,";
        //            strQry += " TS_CONT_PERSON_NAME='" + objSupplier.sContactPerson + "',TS_FAX='" + objSupplier.sFax + "',TS_MOBILE_NO='" + objSupplier.sMobileNo + "'";
        //            strQry += " WHERE TS_ID= '" + objSupplier.SupplierId + "'";
        //            objcon.Execute(strQry);
        //            Arr[0] = "Updated Successfully ";
        //            Arr[1] = "1";
        //            return Arr;
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        clsException.LogError(ex.StackTrace,ex.Message, "clsTransSupplier", "SaveDetails");
        //        return Arr;
        //    }
        //}

        public string[] SaveDetails(clsTransSupplier objSupplier)
        {
            string[] Arr = new string[3];
            try
            {

                NpgsqlCommand cmd = new NpgsqlCommand("proc_save_update_supplier");

                cmd.Parameters.AddWithValue("supplier_Id", objSupplier.SupplierId);
                cmd.Parameters.AddWithValue("supplier_name", objSupplier.SupplierName);
                cmd.Parameters.AddWithValue("supplier_address", objSupplier.RegisterAddress);
                cmd.Parameters.AddWithValue("supplier_phoneno", objSupplier.SupplierPhoneNo);
                cmd.Parameters.AddWithValue("supplier_email", objSupplier.SupplierEmail);
                cmd.Parameters.AddWithValue("supplier_blacklisted", objSupplier.SupplierBlacklisted);
                cmd.Parameters.AddWithValue("supplier_blackedupto", objSupplier.SupplierBlackedupto);
                cmd.Parameters.AddWithValue("supplier_sCrby", objSupplier.sCrby);
                cmd.Parameters.AddWithValue("supplier_commaddress", objSupplier.CommAddress);
                cmd.Parameters.AddWithValue("supplier_contact_personname", objSupplier.sContactPerson);
                cmd.Parameters.AddWithValue("supplier_fax", objSupplier.sFax);
                cmd.Parameters.AddWithValue("supplier_mobileno", objSupplier.sMobileNo);

                cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);


                cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;


                Arr[0] = "msg";
                Arr[1] = "op_id";

                Arr = Objcon.Execute(cmd, Arr, 2);
                //objSupplier.SupplierId = Arr[2].ToString();
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return Arr;
        }

        //public object GetSupplierDetails(clsTransSupplier objSupplier)
        //{
        //    DataTable dtDetails = new DataTable();         
        //    OleDbDataReader dr = null;

        //        try
        //        {
        //            string  strQry = "SELECT TS_ID,TS_NAME,TS_ADDRESS,TS_PHONE,TS_EMAIL,TS_BLACK_LISTED,TO_CHAR(TS_BLACKED_UPTO,'DD/MM/YYYY') TS_BLACKED_UPTO,";
        //            strQry += " TS_COMM_ADDRESS,TS_CONT_PERSON_NAME,TS_FAX,TO_CHAR(TS_MOBILE_NO) TS_MOBILE_NO FROM TBLTRANSSUPPLIER  WHERE TS_ID='" + objSupplier.SupplierId + "'";
        //            dr = objcon.Fetch(strQry);
        //            dtDetails.Load(dr);
        //            if (dtDetails.Rows.Count > 0)
        //            {
        //                objSupplier.SupplierId = Convert.ToString(dtDetails.Rows[0]["TS_ID"].ToString());
        //                objSupplier.SupplierName = Convert.ToString(dtDetails.Rows[0]["TS_NAME"].ToString());
        //                objSupplier.RegisterAddress = Convert.ToString(dtDetails.Rows[0]["TS_ADDRESS"].ToString());
        //                objSupplier.SupplierPhoneNo = Convert.ToString(dtDetails.Rows[0]["TS_PHONE"].ToString());
        //                objSupplier.SupplierEmail = Convert.ToString(dtDetails.Rows[0]["TS_EMAIL"].ToString()); 
        //                objSupplier.SupplierBlacklisted = Convert.ToString(dtDetails.Rows[0]["TS_BLACK_LISTED"].ToString());
        //                objSupplier.SupplierBlackedupto = Convert.ToString(dtDetails.Rows[0]["TS_BLACKED_UPTO"].ToString());
        //                objSupplier.CommAddress = Convert.ToString(dtDetails.Rows[0]["TS_COMM_ADDRESS"].ToString());
        //                objSupplier.sContactPerson = Convert.ToString(dtDetails.Rows[0]["TS_CONT_PERSON_NAME"].ToString());
        //                objSupplier.sFax = Convert.ToString(dtDetails.Rows[0]["TS_FAX"].ToString());
        //                objSupplier.sMobileNo = Convert.ToString(dtDetails.Rows[0]["TS_MOBILE_NO"].ToString());
        //            }

        //            return objSupplier;
        //        }
        //        catch (Exception ex)
        //        {
        //            clsException.LogError(ex.StackTrace,ex.Message, "clsTransSupplier", "GetSupplierDetails");
        //            return objSupplier;
        //        }
        //}
        #region Old GetSupplierDetails
        NpgsqlCommand NpgsqlCommand;
        public object GetSupplierDetails(clsTransSupplier objSupplier)
        {
            NpgsqlCommand = new NpgsqlCommand();
            DataTable dtDetails = new DataTable();
            string sQry = string.Empty;
            try
            {
                NpgsqlCommand.Parameters.AddWithValue("supid", objSupplier.SupplierId);
                sQry = "SELECT \"TS_ID\",\"TS_NAME\",\"TS_ADDRESS\",\"TS_PHONE\",\"TS_EMAIL\",\"TS_BLACK_LISTED\", ";
                sQry += "TO_CHAR(\"TS_BLACKED_UPTO\",'DD/MM/YYYY') \"TS_BLACKED_UPTO\", \"TS_COMM_ADDRESS\",\"TS_CONT_PERSON_NAME\",\"TS_FAX\",\"TS_MOBILE_NO\" FROM";
                sQry += "\"TBLTRANSSUPPLIER\" WHERE cast(\"TS_ID\" as text) =:supid";
                dtDetails = Objcon.FetchDataTable(sQry, NpgsqlCommand);
                if (dtDetails.Rows.Count > 0)
                {
                    objSupplier.SupplierId = Convert.ToString(dtDetails.Rows[0]["TS_ID"]);
                    objSupplier.SupplierName = Convert.ToString(dtDetails.Rows[0]["TS_NAME"]);
                    objSupplier.RegisterAddress = Convert.ToString(dtDetails.Rows[0]["TS_ADDRESS"]);
                    objSupplier.SupplierPhoneNo = Convert.ToString(dtDetails.Rows[0]["TS_PHONE"]);
                    objSupplier.SupplierEmail = Convert.ToString(dtDetails.Rows[0]["TS_EMAIL"]);
                    objSupplier.SupplierBlacklisted = Convert.ToString(dtDetails.Rows[0]["TS_BLACK_LISTED"]);
                    objSupplier.SupplierBlackedupto = Convert.ToString(dtDetails.Rows[0]["TS_BLACKED_UPTO"]);
                    objSupplier.CommAddress = Convert.ToString(dtDetails.Rows[0]["TS_COMM_ADDRESS"]);
                    objSupplier.sContactPerson = Convert.ToString(dtDetails.Rows[0]["TS_CONT_PERSON_NAME"]);
                    objSupplier.sFax = Convert.ToString(dtDetails.Rows[0]["TS_FAX"]);
                    objSupplier.sMobileNo = Convert.ToString(dtDetails.Rows[0]["TS_MOBILE_NO"]);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return objSupplier;
        }
        #endregion


        //public DataTable LoadSupplierDetails()
        //{
        //    string strQry = string.Empty;
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        strQry = "Select TS_ID,TS_NAME,TS_ADDRESS,TS_PHONE,TS_EMAIL,DECODE(TS_BLACK_LISTED,'0','NO','1','YES') AS TS_BLACK_LISTED,TS_STATUS,";
        //        strQry += " TO_CHAR(TS_BLACKED_UPTO,'DD-MON-YYYY') TS_BLACKED_UPTO,TS_COMM_ADDRESS FROM TBLTRANSSUPPLIER ORDER BY TS_ID DESC";
        //        OleDbDataReader dr = objcon.Fetch(strQry);
        //        dt.Load(dr);
        //        return dt;

        //    }

        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace,ex.Message, "clsTransSupplier", "LoadDetails");
        //        return dt;
        //    }
        //}
        #region old LoadSupplierDetails
        public DataTable LoadSupplierDetails()
        {
            string strQry = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                strQry = "SELECT \"TS_ID\",\"TS_NAME\",\"TS_ADDRESS\",\"TS_PHONE\",\"TS_EMAIL\", CASE WHEN \"TS_BLACK_LISTED\" = 0 THEN 'NO' ELSE 'YES' END AS \"TS_BLACK_LISTED\"," +
                         " \"TS_STATUS\", TO_CHAR(\"TS_BLACKED_UPTO\",'DD-MON-YYYY') \"TS_BLACKED_UPTO\",\"TS_COMM_ADDRESS\" FROM \"TBLTRANSSUPPLIER\" ORDER BY \"TS_ID\" DESC";
                dt = Objcon.FetchDataTable(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return dt;
        }
        #endregion
        //public bool ActiveDeactiveSupplier(clsTransSupplier objSupplier)
        //{
        //    try
        //    {
        //        string strQry = string.Empty;
        //        strQry = "UPDATE TBLTRANSSUPPLIER SET TS_STATUS='" + objSupplier.sStatus + "' WHERE TS_ID='" + objSupplier.SupplierId + "'";
        //        objcon.Execute(strQry);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace,ex.Message, "clsTransSupplier", "ActiveDeactiveSupplier");
        //        return false;

        //    }
        //}

        public bool ActiveDeactiveSupplier(clsTransSupplier objSupplier)
        {
            string[] Arr = new string[3];
            bool bRes = false;
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_activate_deactivate_supplier");
                cmd.Parameters.AddWithValue("trans_supply_status", objSupplier.sStatus);
                cmd.Parameters.AddWithValue("trans_supply_id", objSupplier.SupplierId);

                Objcon.Execute(cmd, Arr, 2);
                bRes = true;
            }
            catch (Exception ex)
            {
                bRes = false;
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return bRes;
        }
    }

    public class Shape
    {
        public virtual void Draw()
        {
        }
    }

    public class Rectangle : Shape
    {
        public override void Draw()
        {

        }
    }
}
