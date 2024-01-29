using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IIITS.PGSQL.DAL;
using System.Data;
using Npgsql;


namespace IIITS.DTLMS.BL.Dashboard
{
    public class clsSlaDashboard
    {
        string strFormCode = "clsSlaDashboard";
        PGSqlConnection objcon = new PGSqlConnection(Constants.Password);

        public DataTable GetSLADetails(string sOfficeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                //NpgsqlCommand cmd = new NpgsqlCommand("sp_getsladetails");
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getsladetailsfromtrans");
                cmd.Parameters.AddWithValue("officecode", sOfficeCode);
                dt = objcon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable GetSLADetailsview(string sOfficeCode, string moduleid)
        {
            DataTable dt = new DataTable();
            try
            {

                
                //NpgsqlCommand cmd = new NpgsqlCommand("sp_getsladetailsview");
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getsladetailsfromtransview");
                cmd.Parameters.AddWithValue("officecode", sOfficeCode);
                cmd.Parameters.AddWithValue("moduleid", moduleid);
                dt = objcon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        //public string GetEstimation(string sOfficeCode)
        //{
        //    string sEstimation = string.Empty;
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        string sQry = string.Empty;
        //        sQry = " SELECT SUM(\"3 DAYS\" + \"7 DAYS\" + \"15 DAYS\" + \"30 DAYS\" + \"MORE THAN 30 DAYS\") \"ESTIMATION\" FROM ";
        //        sQry += " \"VIEW_SLA_MR_ALERTS\" WHERE CAST(\"OM_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%' AND \"MODULE\" = 'ESTIMATION' ";
        //        sEstimation = objcon.get_value(sQry);
        //        if (sEstimation.Length == 0)
        //        {
        //            sEstimation = "0";
        //        }
        //        return sEstimation;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return sEstimation;
        //    }
        //}
        //public string GetWorkOrder(string sOfficeCode)
        //{
        //    string sWorder = string.Empty;
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        string sQry = string.Empty;
        //        sQry = " SELECT SUM(\"3 DAYS\" + \"7 DAYS\" + \"15 DAYS\" + \"30 DAYS\" + \"MORE THAN 30 DAYS\") \"WORKORDER\" FROM ";
        //        sQry += " \"VIEW_SLA_MR_ALERTS\" WHERE CAST(\"OM_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%' AND \"MODULE\" = 'WORK ORDER' ";
        //        sWorder = objcon.get_value(sQry);

        //        if (sWorder.Length == 0)
        //        {
        //            sWorder = "0";
        //        }
        //        return sWorder;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return sWorder;
        //    }
        //}
        //public string GetRcveDTR(string sOfficeCode)
        //{
        //    string sRcveDtr = string.Empty;
        //    try
        //    {
        //        string sQry = string.Empty;
        //        sQry = " SELECT SUM(\"3 DAYS\" + \"7 DAYS\" + \"15 DAYS\" + \"30 DAYS\" + \"MORE THAN 30 DAYS\") \"RcveDTR\" FROM ";
        //        sQry += " \"VIEW_SLA_MR_ALERTS\" WHERE CAST(\"OM_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%' AND \"MODULE\" = 'RECEIVE DTR' ";
        //        sRcveDtr = objcon.get_value(sQry);
        //        if (sRcveDtr.Length == 0)
        //        {
        //            sRcveDtr = "0";
        //        }
        //        return sRcveDtr;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return sRcveDtr;
        //    }
        //}
        //public string GetInvcDTR(string sOfficeCode)
        //{
        //    string sInvcDtr = string.Empty;
        //    try
        //    {
        //        string sQry = string.Empty;
        //        sQry = " SELECT SUM(\"3 DAYS\" + \"7 DAYS\" + \"15 DAYS\" + \"30 DAYS\" + \"MORE THAN 30 DAYS\") \"InvcDTR\" FROM ";
        //        sQry += " \"VIEW_SLA_MR_ALERTS\" WHERE CAST(\"OM_CODE\" AS TEXT) LIKE '" + sOfficeCode + "%' AND \"MODULE\" = 'INVOICE DTR' ";
        //        sInvcDtr = objcon.get_value(sQry);
        //        if (sInvcDtr.Length == 0)
        //        {
        //            sInvcDtr = "0";
        //        }
        //        return sInvcDtr;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return sInvcDtr;
        //    }
        //}

        /*  public DataTable LoadDetails(string sOfficeCode, string sSubDiv = "", string sSection = "", string sPhase = "")
          {
              DataTable dt = new DataTable();
              try
              {
                  NpgsqlCommand cmd = new NpgsqlCommand("sp_getsladetails");
                  cmd.Parameters.AddWithValue("officecode", sOfficeCode);
                  dt = objcon.FetchDataTable(cmd);
                  string sQry = string.Empty;
                  if (sSubDiv != "")
                  {
                      cmd.Parameters.AddWithValue("sSubDiv", sSubDiv);
                      dt = objcon.FetchDataTable(cmd);
                  }
                  if (sSection != "")
                  {
                      cmd.Parameters.AddWithValue("sSection", sSection);
                      dt = objcon.FetchDataTable(cmd);
                  }
                  if (sPhase != "")
                  {
                      cmd.Parameters.AddWithValue("sPhase", sPhase);
                      dt = objcon.FetchDataTable(cmd);
                  }
                  sQry += " LIMIT 100";
                  NpgsqlDataReader drcorp = objcon.Fetch(sQry);
                  dt.Load(drcorp);
                  return dt;


              }
              catch (Exception ex)
              {
                  clsException.LogError(ex.StackTrace, ex.Message, sFormName, "LoadDetails");
                  return dt;

              }

          }*/
    }
}





