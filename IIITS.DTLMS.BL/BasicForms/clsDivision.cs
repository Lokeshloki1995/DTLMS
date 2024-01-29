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
    public class clsDivision
    {
        //CustOledbConnection objcon = new CustOledbConnection(Constants.Password);
        PGSqlConnection Objcon = new PGSqlConnection(Constants.Password);
        string strFormCode = "clsDivision";
        public string sCircleCode { get; set; }
        public string sDivisionCode { get; set; }
        public string sDivisionName { get; set; }
        public string sName { get; set; }
        public string sPhone { get; set; }
        public string sMobileNo { get; set; }
        public string sEmail { get; set; }
        public string sMaxid { get; set; }
        public string sAddress { get; set; }


        //public string[] SaveDivision(clsDivision objDivision)
        //{
        //    string strQry = string.Empty;
        //    string sQryValue = string.Empty;
        //    string[] Arr = new string[2];
        //    try
        //    {


        //        if (objDivision.sMaxid == "")
        //        {
        //            OleDbDataReader dr = objcon.Fetch("Select * from TBLDIVISION where DIV_CODE='" + objDivision.sDivisionCode + "'");
        //            if (dr.Read())
        //            {
        //                dr.Close();
        //                Arr[0] = "division code already exists";
        //                Arr[1] = "3";
        //                return Arr;
        //            }
        //            dr.Close();

        //            objDivision.sMaxid = objcon.Get_max_no("DIV_ID", "TBLDIVISION").ToString();

        //            strQry = "INSERT INTO TBLDIVISION(DIV_ID, DIV_CODE ,DIV_NAME,DIV_CICLE_CODE,DIV_HEAD_EMP,DIV_MOBILE_NO,DIV_PHONE,DIV_EMAIL)";
        //            strQry += "VALUES('"+objDivision.sMaxid+"', '" + objDivision.sDivisionCode + "','" + objDivision.sDivisionName + "','" + objDivision.sCircleCode + "',";
        //            strQry += "'" + objDivision.sName + "','" + objDivision.sMobileNo + "','" + objDivision.sPhone + "','" + objDivision.sEmail + "'  )";
        //            objcon.Execute(strQry);
        //            Arr[0] = "Saved Successfully ";
        //            Arr[1] = "0";
        //            return Arr;
        //        }
        //        else
        //        {
        //            strQry = " UPDATE TBLDIVISION SET DIV_NAME= '" + objDivision.sDivisionName + "', DIV_HEAD_EMP='" + objDivision.sName + "', DIV_CODE='" + objDivision.sDivisionCode + "'";
        //            strQry += " ,DIV_MOBILE_NO='" + objDivision.sMobileNo + "', DIV_PHONE='" + objDivision.sPhone + "',DIV_EMAIL='" + objDivision.sEmail + "' where DIV_ID = '" + objDivision.sMaxid + "'";
        //            objcon.Execute(strQry);
        //            Arr[0] = "Updated Successfully ";
        //            Arr[1] = "1";
        //            return Arr;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveDivision");
        //        return Arr;
        //    }
        //}

        public string[] SaveDivision(clsDivision objDivision)
        {
            string[] Arr = new string[3];
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_save_update_division");
                cmd.Parameters.AddWithValue("div_id", objDivision.sMaxid);
                cmd.Parameters.AddWithValue("div_code", objDivision.sDivisionCode);
                cmd.Parameters.AddWithValue("div_name", objDivision.sDivisionName);
                cmd.Parameters.AddWithValue("div_cicle_code", objDivision.sCircleCode);
                cmd.Parameters.AddWithValue("div_head_emp", objDivision.sName);
                cmd.Parameters.AddWithValue("div_mobile_no", objDivision.sMobileNo);
                cmd.Parameters.AddWithValue("div_phone", objDivision.sPhone);
                cmd.Parameters.AddWithValue("div_email", objDivision.sEmail);
                cmd.Parameters.AddWithValue("div_address", objDivision.sAddress);

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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return Arr;
        }
        //public DataTable LoadAllDivisionDetails()
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {

        //        string strQry = string.Empty;
        //        strQry = "SELECT DIV_ID,  To_char(DIV_CODE)DIV_CODE ,DIV_NAME FROM TBLDIVISION";
        //        dt = objcon.getDataTable(strQry);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadAllDivisionDetails");
        //        return dt;
        //    }

        //}

        public DataTable LoadAllDivisionDetails()
        {

            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(" proc_load_division_details");
                dt = Objcon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return dt;
        }

        //public object getDivisionDetails(clsDivision objDivision)
        //{
        //    DataTable dtDetails = new DataTable();
        //    try
        //    {
        //        String strQry = "SELECT  DIV_CODE ,DIV_NAME,DIV_CICLE_CODE,DIV_HEAD_EMP,DIV_MOBILE_NO,DIV_PHONE,DIV_EMAIL FROM TBLDIVISION ";
        //        strQry += " WHERE DIV_ID ='" + objDivision.sMaxid + "'";
        //        dtDetails = objcon.getDataTable(strQry);

        //        if (dtDetails.Rows.Count > 0)
        //        {
        //            objDivision.sDivisionName = Convert.ToString(dtDetails.Rows[0]["DIV_NAME"].ToString());
        //            objDivision.sDivisionCode = Convert.ToString(dtDetails.Rows[0]["DIV_CODE"].ToString());
        //            objDivision.sCircleCode = Convert.ToString(dtDetails.Rows[0]["DIV_CICLE_CODE"].ToString());
        //            objDivision.sName = Convert.ToString(dtDetails.Rows[0]["DIV_HEAD_EMP"].ToString());
        //            objDivision.sMobileNo = Convert.ToString(dtDetails.Rows[0]["DIV_MOBILE_NO"].ToString());
        //            objDivision.sPhone = Convert.ToString(dtDetails.Rows[0]["DIV_PHONE"].ToString());
        //            objDivision.sEmail = Convert.ToString(dtDetails.Rows[0]["DIV_EMAIL"].ToString());
        //        }
        //        return objDivision;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "getCircleDetails");
        //        return objDivision;
        //    }

        //}

        public object getDivisionDetails(clsDivision objDivision)
        {
            DataTable dtDetails = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_division_details");
                cmd.Parameters.AddWithValue("divsion_id", objDivision.sMaxid);
                dtDetails = Objcon.FetchDataTable(cmd);

                if (dtDetails.Rows.Count > 0)
                {
                    objDivision.sDivisionName = Convert.ToString(dtDetails.Rows[0]["div_name"]);
                    objDivision.sDivisionCode = Convert.ToString(dtDetails.Rows[0]["div_code"]);
                    objDivision.sCircleCode = Convert.ToString(dtDetails.Rows[0]["div_cicle_code"]);
                    objDivision.sName = Convert.ToString(dtDetails.Rows[0]["div_head_emp"]);
                    objDivision.sMobileNo = Convert.ToString(dtDetails.Rows[0]["div_Mobile_no"]);
                    objDivision.sPhone = Convert.ToString(dtDetails.Rows[0]["div_phone"]);
                    objDivision.sEmail = Convert.ToString(dtDetails.Rows[0]["div_email"]);
                    objDivision.sAddress = Convert.ToString(dtDetails.Rows[0]["div_address"]);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return objDivision;
        }

        //public string GenerateDivCode(clsDivision objDivision)
        //{
        //    try
        //    {
        //        string sMaxNo = string.Empty;
        //        string sFinancialYear = string.Empty;
        //        string sCircleCodeNo = objcon.get_value(" SELECT NVL(MAX(DIV_CODE),0)+1 FROM TBLDIVISION  where DIV_CICLE_CODE='" + objDivision.sCircleCode + "'");
        //        if (sCircleCodeNo.Length > 0)
        //        {
        //            sCircleCode = sCircleCodeNo.ToString();
        //        }
        //        return sCircleCodeNo;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GenerateDivCode");
        //        return "";
        //    }
        //}
        NpgsqlCommand NpgsqlCommand;
        public string GenerateDivCode(clsDivision objDivision)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string sCircleCodeNo = string.Empty;
            try
            {
                NpgsqlCommand.Parameters.AddWithValue("divCode", objDivision.sCircleCode);
                sCircleCodeNo = Objcon.get_value(" SELECT CAST(COALESCE(MAX(\"DIV_CODE\"),0)+1 AS TEXT) FROM \"TBLDIVISION\"  where CAST(\"DIV_CICLE_CODE\"AS TEXT) like :divCode||'%'", NpgsqlCommand);
              if (Convert.ToInt16(sCircleCodeNo) <= 1)
              {
                  sCircleCodeNo = objDivision.sCircleCode + "1";
              }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return sCircleCodeNo;
        }

    }
}

