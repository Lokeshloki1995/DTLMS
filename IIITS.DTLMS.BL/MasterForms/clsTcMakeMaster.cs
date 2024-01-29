using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using System.Data;
using IIITS.PGSQL.DAL;
using Npgsql;
using System.Reflection;
using NpgsqlTypes;



namespace IIITS.DTLMS.BL
{
    public class clsTcMakeMaster
    {
        string strFormCode = "clsTcMakeMaster";
      
        PGSqlConnection Objcon = new PGSqlConnection(Constants.Password);
        public string sMakeId { get; set; }
        public string sMakeName { get; set; }
        public string sDescription { get; set; }
        public string sCrby { get; set; }
        public string sStatus { get; set; }

        public string sEffectFrom { get; set; }
        public string sReason { get; set; }
       
        //public string[] SaveUpdateTcMakeMaster(clsTcMakeMaster objTcMakeMaster)
        //{
        //    string strQry = string.Empty;
        //    string[] Arr = new string[2];

        //    OleDbDataReader dr;
        //    try
        //    {
        //        if (objTcMakeMaster.sMakeId  == "")
        //        {
        //            dr = ObjCon.Fetch("select TM_NAME from TBLTRANSMAKES where UPPER(TM_NAME)='" + objTcMakeMaster.sMakeName.ToUpper() + "'");
        //            if (dr.Read())
        //            {
        //                Arr[0] = "Make Name Already Exists";
        //                Arr[1] = "4";
        //                dr.Close();
        //                return Arr;
                       
        //            }
        //            dr.Close();


        //            string sMaxNo = Convert.ToString(ObjCon.Get_max_no("TM_ID", "TBLTRANSMAKES"));
        //            strQry = "Insert into TBLTRANSMAKES (TM_ID,TM_NAME,TM_DESC,TM_CRBY) VALUES ('" + sMaxNo + "',";
        //           strQry+= " '" + objTcMakeMaster.sMakeName.ToUpper()  + "','" + objTcMakeMaster.sDescription + "','"+ objTcMakeMaster.sCrby  +"') ";

        //            ObjCon.Execute(strQry);
        //            Arr[0] = sMaxNo;
        //            Arr[1] = "0";
        //            return Arr;

        //        }

        //        else
        //        {

        //            dr = ObjCon.Fetch("select TM_NAME from TBLTRANSMAKES where UPPER(TM_NAME)='" + objTcMakeMaster.sMakeName.ToUpper() + "' AND TM_ID<>'" + objTcMakeMaster.sMakeId + "'");
        //            if (dr.Read())
        //            {
        //                Arr[0] = "Make Name Already Exists";
        //                Arr[1] = "4";
        //                dr.Close();
        //                return Arr;

        //            }
        //            dr.Close();

        //            strQry = "UPDATE TBLTRANSMAKES SET TM_NAME='" + objTcMakeMaster.sMakeName.ToUpper() + "',TM_DESC='" + objTcMakeMaster.sDescription + "' ";
        //            strQry+= " where TM_ID='"+ objTcMakeMaster.sMakeId +"'";
        //            ObjCon.Execute(strQry);

        //            Arr[0] = "Updated Successfully";
        //            Arr[1] = "1";
        //            return Arr;


        //        }
        //    }

        //    catch (Exception ex)
        //    {
               
        //        clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "SaveUpdateTcMakeMaster");
        //        return Arr;

        //    }

        //}


        public string[] SaveUpdateTcMakeMaster(clsTcMakeMaster objTcMakeMaster)
        {
            string[] Arr = new string[3];
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_saveupdate_tcmakemaster");
                cmd.Parameters.AddWithValue("make_id", objTcMakeMaster.sMakeId);
                cmd.Parameters.AddWithValue("make_name", objTcMakeMaster.sMakeName.ToUpper());
                cmd.Parameters.AddWithValue("make_desc", objTcMakeMaster.sDescription);
                cmd.Parameters.AddWithValue("created_by", objTcMakeMaster.sCrby);

                cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);


                cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;



                Arr[0] = "msg";
                Arr[1] = "op_id";
               
                Arr = Objcon.Execute(cmd, Arr, 2);
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return Arr;
        }

        //public clsTcMakeMaster  GetTCMakeMasterDetails(clsTcMakeMaster objTcMakeMaster)
        //{
        //    string strQry = string.Empty;
        //    OleDbDataReader dr;
        //    DataTable dtStoreDetails = new DataTable();
        //    try
        //    {

        //        strQry = "SELECT TM_ID,TM_NAME,TM_DESC FROM TBLTRANSMAKES WHERE TM_ID='" + objTcMakeMaster.sMakeId  + "'";
        //        dr = ObjCon.Fetch(strQry);
        //        dtStoreDetails.Load(dr);             
        //        if (dtStoreDetails.Rows.Count > 0)
        //        {

        //            objTcMakeMaster.sMakeId  = dtStoreDetails.Rows[0]["TM_ID"].ToString();
        //            objTcMakeMaster.sMakeName  = dtStoreDetails.Rows[0]["TM_NAME"].ToString();
        //            objTcMakeMaster.sDescription = dtStoreDetails.Rows[0]["TM_DESC"].ToString();
                   
        //        }
        //        return objTcMakeMaster;
        //    }

            
        //    catch (Exception ex)
        //    {

        //        clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadTCMakeMasterDetails");
        //        return objTcMakeMaster;
        //    }

        //}

        public clsTcMakeMaster GetTCMakeMasterDetails(clsTcMakeMaster objTcMakeMaster)
        {
            DataTable dtStoreDetails = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_tcmakemaster_details");
                cmd.Parameters.AddWithValue("make_id", objTcMakeMaster.sMakeId);
                dtStoreDetails = Objcon.FetchDataTable(cmd);
                if (dtStoreDetails.Rows.Count > 0)
                {
                    objTcMakeMaster.sMakeId = Convert.ToString(dtStoreDetails.Rows[0]["TM_ID"]);
                    objTcMakeMaster.sMakeName = Convert.ToString(dtStoreDetails.Rows[0]["TM_NAME"]);
                    objTcMakeMaster.sDescription = Convert.ToString(dtStoreDetails.Rows[0]["TM_DESC"]);
                }
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return objTcMakeMaster;
        }

        //public DataTable LoadTcMakeMaster()
        //{
           
        //    string strQry = string.Empty;
        //    OleDbDataReader dr;
        //    DataTable dtStoreDetails = new DataTable();
        //    try
        //    {

        //        strQry = "SELECT TM_ID,TM_NAME,TM_DESC,TM_STATUS,TM_EFFECT_FROM,";
        //        strQry += " CASE   WHEN TO_CHAR(TM_EFFECT_FROM,'YYYYMMDD') > TO_CHAR(SYSDATE,'YYYYMMDD') AND TM_STATUS='D' THEN 'A' ";
        //        strQry += " ELSE TM_STATUS END  TM_STATUS1 ";
        //        strQry += " FROM TBLTRANSMAKES ORDER BY TM_ID DESC ";

        //        dr = ObjCon.Fetch(strQry);
        //        dtStoreDetails.Load(dr);
        //        return dtStoreDetails;
        //    }
        //    catch (Exception ex)
        //    {
               
        //        clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadTcMakeMaster");
        //        return dtStoreDetails;
        //    }
        //}

        public DataTable LoadTcMakeMaster()
        {
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_tcmakemaster_details");
                dt = Objcon.FetchDataTable(cmd);
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return dt;
        }

        //public bool ActiveDeactiveMake(clsTcMakeMaster objMakeMaster)
        //{
        //    try
        //    {
        //        string strQry = string.Empty;
        //        strQry = "UPDATE TBLTRANSMAKES SET TM_STATUS='" + objMakeMaster.sStatus + "',TM_EFFECT_FROM = TO_DATE('" + objMakeMaster.sEffectFrom + "','dd/MM/yyyy'),";
        //        strQry += "TM_REASON='" + objMakeMaster.sReason + "' WHERE TM_ID='" + objMakeMaster.sMakeId + "'";
        //        ObjCon.Execute(strQry);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ActiveDeactiveMake");
        //        return false;

        //    }
        //}

        public bool ActiveDeactiveMake(clsTcMakeMaster objMakeMaster)
        {
            string[] Arr = new string[3];
            bool bRes = false;
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_tcmakemaster_details");
                cmd.Parameters.AddWithValue("make_id", objMakeMaster.sMakeId);
                cmd.Parameters.AddWithValue("effect_from", objMakeMaster.sEffectFrom);
                cmd.Parameters.AddWithValue("make_status", objMakeMaster.sStatus);
                cmd.Parameters.AddWithValue("make_reason", objMakeMaster.sReason);
                Objcon.Execute(cmd, Arr, 0);
                bRes = true;
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                bRes = false;
            }
            return bRes;
        }

    }
}

