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
   public class clsDistrict
    {
       string strFormCode = "ClsDistrict";
       //CustOledbConnection objcon = new CustOledbConnection(Constants.Password);
       PGSqlConnection Objcon = new PGSqlConnection(Constants.Password);
       public string sDistId { get; set; }
       public DataTable sDt = new DataTable();
       public string sDistrictCode { get; set; }
       public string sDistrictName { get; set; }
       public string sButtonname { get; set; }
       public string sOfficeCode { get; set; }
       public string sOfficeName { get; set; }
       public string sSlNo { get; set; }

       NpgsqlCommand NpgsqlCommand;
        public string[] SaveDetails(clsDistrict objDis)                                               
        {
            NpgsqlCommand = new NpgsqlCommand();
            string[] Arr = new string[3];
            try
            { 
                string[] strQryVallist = null;
                if (objDis.sOfficeCode != "")
                {
                    strQryVallist = objDis.sOfficeCode.Split(',');
                }
                if (objDis.sDistId.Length == 0)
                {
                    foreach (string OfficeCode in strQryVallist)
                    {
                        string sQry = string.Empty;
                        string sResult = string.Empty;
                        NpgsqlCommand Cmd = new NpgsqlCommand("proc_load_district_detailsbyid");
                        Cmd.Parameters.AddWithValue("dt_id", OfficeCode);
                        DataTable  dtDetails = Objcon.FetchDataTable(Cmd);
                        if (dtDetails.Rows.Count > 0)
                             sResult = Convert.ToString( dtDetails.Rows[0]["DT_CODE"]);
                        if (sResult.Length > 0)
                        {
                            Arr[0] = "Location Already Allocated to some other District";
                            Arr[1] = "4";
                            return Arr;
                        }
                    }
                }
                else
                {
                    foreach (string OfficeCode in strQryVallist)
                    {
                        string sQry = string.Empty;
                        string sResult = string.Empty;
                        NpgsqlCommand Cmd = new NpgsqlCommand("sp_dist_div_valdt");
                        Cmd.Parameters.AddWithValue("offcode", OfficeCode);
                        Cmd.Parameters.AddWithValue("slno", OfficeCode);
                        DataTable dtDetails = Objcon.FetchDataTable(Cmd);
                        if (dtDetails.Rows.Count > 0)
                            sResult = Convert.ToString(dtDetails.Rows[0]["DD_ID"]);
                        if (sResult.Length > 0)
                        {
                            Arr[0] = "Location Already Allocated to some other District";
                            Arr[1] = "4";
                            return Arr;
                        }
                    }
                }

                    NpgsqlCommand cmd = new NpgsqlCommand("proc_save_update_district");
                    cmd.Parameters.AddWithValue("dt_code", objDis.sDistrictCode);
                    cmd.Parameters.AddWithValue("dt_name", objDis.sDistrictName.ToUpper());    
                    cmd.Parameters.AddWithValue("dt_id", objDis.sDistId);

                    cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd.Parameters.Add("pk_id", NpgsqlDbType.Text);

                    cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                    cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                    cmd.Parameters["pk_id"].Direction = ParameterDirection.Output;

                    Arr[0] = "msg";
                    Arr[1] = "op_id";
                    Arr[2] = "pk_id";
                   
                    Arr = Objcon.Execute(cmd, Arr, 3);
                    if (Arr[1] == "0" || Arr[1] == "2")
                    {
                        if (strQryVallist.Length > 0)
                        {
                            //string sQry = string.Empty;
                            //NpgsqlCommand.Parameters.AddWithValue("arr", Convert.ToInt32(Arr[2]));
                            //sQry = "DELETE FROM \"TBLDISTTODIVMAPPING\" WHERE \"DD_DIST_ID\" =:arr";
                            //Objcon.ExecuteQry(sQry, NpgsqlCommand);

                        NpgsqlCommand = new NpgsqlCommand("sp_delete_div_mapped_todist");
                        NpgsqlCommand.Parameters.AddWithValue("arr",Convert.ToString( Arr[2]));
                        Objcon.Execute(NpgsqlCommand, Arr, 0);
                    }

                        foreach (string OfficeCode in strQryVallist)
                        {
                        //string sQry = string.Empty;
                        //int sMaxNo = Convert.ToInt32(Objcon.Get_max_no("DD_ID", "TBLDISTTODIVMAPPING"));
                        //sQry = "INSERT INTO \"TBLDISTTODIVMAPPING\"(\"DD_ID\", \"DD_DIST_ID\", \"DD_DIV_ID\")";
                        //sQry += " VALUES('" + sMaxNo + "','" + Convert.ToInt32(Arr[2]) + "','" + Convert.ToInt32(OfficeCode) + "')";
                        //Objcon.ExecuteQry(sQry);

                        NpgsqlCommand = new NpgsqlCommand("sp_insert_div_mapped_todist");
                        NpgsqlCommand.Parameters.AddWithValue("arr", Convert.ToString(Arr[2]));
                        NpgsqlCommand.Parameters.AddWithValue("offcode", Convert.ToString(OfficeCode));
                        Objcon.Execute(NpgsqlCommand, Arr, 0);

                    }
                    }
                    return Arr;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return Arr;
        }

       public object GetDistDetails(clsDistrict objDistrict)
        {
            DataTable dtDetails = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_district_detailsbyid");
                cmd.Parameters.AddWithValue("dt_id", objDistrict.sDistId);
                dtDetails = Objcon.FetchDataTable(cmd);
                if (dtDetails.Rows.Count > 0)
                {
                    objDistrict.sDistrictCode = Convert.ToString(dtDetails.Rows[0]["dt_code"]);
                    objDistrict.sDistrictName = Convert.ToString(dtDetails.Rows[0]["dt_name"]);
                }
                NpgsqlCommand cmd1 = new NpgsqlCommand("proc_getdistdetails");
                cmd1.Parameters.AddWithValue("sDistId", objDistrict.sDistId);
                objDistrict.sDt = Objcon.FetchDataTable(cmd1);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return objDistrict;
        }

       public DataTable LoadOfficeDet(clsDistrict objDist)
       {
           NpgsqlCommand = new NpgsqlCommand();
           DataTable dtLocation = new DataTable();
           try
           {
               string strQry = string.Empty;
                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_office_div_details");
                cmd.Parameters.AddWithValue("sOfficeCode", objDist.sOfficeCode);
                cmd.Parameters.AddWithValue("sOfficeName", objDist.sOfficeName.ToUpper());
                dtLocation = Objcon.FetchDataTable(cmd);

                return dtLocation;
           }
           catch (Exception ex)
           {
               clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
               return dtLocation;
           }
       }
        public DataTable LoadAllDistDetails()
        {
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_district_details");
                dt = Objcon.FetchDataTable(cmd); 
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
            return dt;
        }

       public string GenerateDistrictCode()
       {
           string sDistCode = string.Empty;
           try
            { 
                 sDistCode =  Convert.ToString(Objcon.Get_max_no("DT_CODE", "TBLDIST"));
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
           return sDistCode;
        }
    }
}
