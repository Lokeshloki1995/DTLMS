using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.IO;
using IIITS.PGSQL.DAL;
using Npgsql;
using System.Reflection;
using NpgsqlTypes;

namespace IIITS.DTLMS.BL
{
    public class clsDeviceRegister
    {
        public string sUserId { get; set; }
        public string sFullName { get; set; }
        public string sMuId { get; set; }
        public string sDeviceId { get; set; }
        public string sRequestedBy { get; set; }
        public string sApprovalStatus { get; set; }
        public string sApprovedBy { get; set; }
        public string sCrOn { get; set; }
        public string soffcode { get; set; }
        public string DeviceStatus { get; set; }

        PGSqlConnection Objcon = new PGSqlConnection(Constants.Password);
        NpgsqlCommand NpgsqlCommand;
        NpgsqlCommand cmd;
        /// <summary>
        /// Load Device Grid
        /// </summary>
        /// <param name="objdevice"></param>
        /// <returns></returns>
        public DataTable LoadDeviceGrid(clsDeviceRegister objdevice)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string Qry = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                #region inline query
                //Qry = " SELECT \"US_ID\",\"MR_REQUEST_BY\",\"MR_ID\",\"MR_DEVICE_ID\",\"US_FULL_NAME\", ";
                //Qry += " CASE WHEN \"MR_APPROVE_STATUS\"='1' and \"MR_STATUS\" = 'A' THEN 'APPROVED' WHEN \"MR_APPROVE_STATUS\"='1' and \"MR_STATUS\" = 'D' ";
                //Qry += " THEN 'DISABLED' ELSE 'PENDING' END AS \"MR_APPROVE_STATUS\", ";
                //Qry += " TO_CHAR(\"MR_CRON\",'DD-MON-YYYY') \"MR_CRON\" FROM \"TBLMOBILEREGISTER\" ";
                //Qry += " INNER JOIN \"TBLUSER\" ON \"US_ID\" = \"MR_REQUEST_BY\" WHERE ";
                //if (objdevice.DeviceStatus == "A")
                //{
                //    Qry += " \"MR_APPROVE_STATUS\" = '1' and \"MR_STATUS\" = 'A' and ";
                //}
                //else if (objdevice.DeviceStatus == "D")
                //{
                //    Qry += " \"MR_APPROVE_STATUS\" = '1' and \"MR_STATUS\" = 'D' and ";
                //}
                //else if (objdevice.DeviceStatus == "P")
                //{
                //    Qry += " \"MR_APPROVE_STATUS\" = '0' and \"MR_STATUS\" = 'A' and ";
                //}
                //if (objdevice.sDeviceId != null && objdevice.sDeviceId != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("mrdeviceId", objdevice.sDeviceId.ToUpper());
                //    Qry += " UPPER(\"MR_DEVICE_ID\") LIKE :mrdeviceId||'%' AND ";
                //}
                //if (objdevice.sFullName != null && objdevice.sFullName != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("usfullName", objdevice.sFullName.ToUpper());
                //    Qry += " UPPER(\"US_FULL_NAME\") LIKE :usfullName||'%' AND ";
                //}
                //if (objdevice.soffcode != null && objdevice.soffcode != "")
                //{
                //    Qry += " CAST(\"US_OFFICE_CODE\" AS TEXT) LIKE '" + objdevice.soffcode + "%' ";
                //    Qry += " AND \"US_ID\" = '" + objdevice.sUserId + "' ";
                //}
                //else
                //{
                //    Qry += " CAST(\"US_OFFICE_CODE\" AS TEXT) LIKE '%' ";
                //}
                //dt = Objcon.FetchDataTable(Qry, NpgsqlCommand);
                #endregion
                cmd = new NpgsqlCommand("sp_get_mob_device_grid");
                cmd.Parameters.AddWithValue("device_status", (objdevice.DeviceStatus ?? ""));
                cmd.Parameters.AddWithValue("device_id", (objdevice.sDeviceId ?? ""));
                cmd.Parameters.AddWithValue("full_name", (objdevice.sFullName ?? ""));
                cmd.Parameters.AddWithValue("offcode", (objdevice.soffcode ?? ""));
                cmd.Parameters.AddWithValue("us_id", (objdevice.sUserId ?? ""));
                dt = Objcon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
            return dt;
        }

        #region duplicatic Code
        //public DataTable LoadpendingDeviceGrid(clsDeviceRegister objdevice)
        //{
        //    NpgsqlCommand = new NpgsqlCommand();
        //    string sQry = string.Empty;
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        // added where condition :  AND "MR_STATUS" = 'A'
        //        sQry = "SELECT \"US_ID\",\"MR_REQUEST_BY\",\"MR_ID\",\"MR_DEVICE_ID\",\"US_FULL_NAME\",CASE WHEN \"MR_APPROVE_STATUS\"='1' THEN 'APPROVED' WHEN \"MR_APPROVE_STATUS\"='2' THEN 'DISABLED' ELSE 'PENDING' END AS \"MR_APPROVE_STATUS\"," +
        //               " TO_CHAR(\"MR_CRON\",'DD-MON-YYYY')\"MR_CRON\" FROM \"TBLMOBILEREGISTER\" INNER JOIN \"TBLUSER\" ON \"US_ID\" = \"MR_REQUEST_BY\" WHERE \"MR_APPROVE_STATUS\"='0' AND \"MR_STATUS\" = 'A' AND";

        //        if (objdevice.sDeviceId != null && objdevice.sDeviceId != "")
        //        {
        //            NpgsqlCommand.Parameters.AddWithValue("mrdeviceId", objdevice.sDeviceId.ToUpper());
        //            sQry += " UPPER(\"MR_DEVICE_ID\") LIKE :mrdeviceId||'%' AND";
        //        }
        //        if (objdevice.sFullName != null && objdevice.sFullName != "")
        //        {
        //            NpgsqlCommand.Parameters.AddWithValue("usfullName", objdevice.sFullName.ToUpper());
        //            sQry += " UPPER(\"US_FULL_NAME\") LIKE :usfullName||'%' AND ";
        //        }
        //        if (objdevice.soffcode != null && objdevice.soffcode != "")
        //        {
        //            sQry += " CAST( \"US_OFFICE_CODE\" AS TEXT) = '" + objdevice.soffcode + "' AND \"US_ID\" = '" + objdevice.sUserId + "'";
        //        }
        //        else
        //        {
        //            sQry += " CAST(\"US_OFFICE_CODE\" AS TEXT) like '" + objdevice.soffcode + "%' ";
        //        }

        //        dt = Objcon.FetchDataTable(sQry, NpgsqlCommand);
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //    return dt;
        //}

        //public DataTable LoadApprovedDeviceGrid(clsDeviceRegister objdevice)
        //{
        //    NpgsqlCommand = new NpgsqlCommand();
        //    string sQry = string.Empty;
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        // added where condition :  AND "MR_STATUS" = 'A'
        //        sQry = "SELECT \"US_ID\",\"MR_REQUEST_BY\",\"MR_ID\",\"MR_DEVICE_ID\",\"US_FULL_NAME\",CASE WHEN \"MR_APPROVE_STATUS\"='1' THEN 'APPROVED' WHEN \"MR_APPROVE_STATUS\"='2' THEN 'DISABLED' ELSE 'PENDING' END AS \"MR_APPROVE_STATUS\"," +
        //               " TO_CHAR(\"MR_CRON\",'DD-MON-YYYY')\"MR_CRON\" FROM \"TBLMOBILEREGISTER\" INNER JOIN \"TBLUSER\" ON \"US_ID\" = \"MR_REQUEST_BY\" WHERE \"MR_APPROVE_STATUS\"='1' AND \"MR_STATUS\" = 'A' AND";

        //        if (objdevice.sDeviceId != null && objdevice.sDeviceId != "")
        //        {
        //            NpgsqlCommand.Parameters.AddWithValue("mrdeviceId", objdevice.sDeviceId.ToUpper());
        //            sQry += " UPPER(\"MR_DEVICE_ID\") LIKE :mrdeviceId||'%' AND";
        //        }
        //        if (objdevice.sFullName != null && objdevice.sFullName != "")
        //        {
        //            NpgsqlCommand.Parameters.AddWithValue("usfullName", objdevice.sFullName.ToUpper());
        //            sQry += " UPPER(\"US_FULL_NAME\") LIKE :usfullName||'%' AND ";
        //        }
        //        if (objdevice.soffcode != null && objdevice.soffcode != "")
        //        {
        //            sQry += " CAST( \"US_OFFICE_CODE\" AS TEXT) = '" + objdevice.soffcode + "' AND \"US_ID\" = '" + objdevice.sUserId + "'";
        //        }
        //        else
        //        {
        //            sQry += " CAST(\"US_OFFICE_CODE\" AS TEXT) like '" + objdevice.soffcode + "%' ";
        //        }

        //        dt = Objcon.FetchDataTable(sQry, NpgsqlCommand);
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //    return dt;
        //}

        //public DataTable LoadDisabledDeviceGrid(clsDeviceRegister objdevice)
        //{
        //    NpgsqlCommand = new NpgsqlCommand();
        //    string sQry = string.Empty;
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        // added where condition :  AND "MR_STATUS" = 'A'
        //        sQry = "SELECT \"US_ID\",\"MR_REQUEST_BY\",\"MR_ID\",\"MR_DEVICE_ID\",\"US_FULL_NAME\",CASE WHEN \"MR_APPROVE_STATUS\"='1' THEN 'APPROVED' WHEN \"MR_APPROVE_STATUS\"='2' THEN 'DISABLED' ELSE 'PENDING' END AS \"MR_APPROVE_STATUS\"," +
        //               " TO_CHAR(\"MR_CRON\",'DD-MON-YYYY')\"MR_CRON\" FROM \"TBLMOBILEREGISTER\" INNER JOIN \"TBLUSER\" ON \"US_ID\" = \"MR_REQUEST_BY\" WHERE \"MR_APPROVE_STATUS\"='2' AND \"MR_STATUS\" = 'D' AND";

        //        if (objdevice.sDeviceId != null && objdevice.sDeviceId != "")
        //        {
        //            NpgsqlCommand.Parameters.AddWithValue("mrdeviceId", objdevice.sDeviceId.ToUpper());
        //            sQry += " UPPER(\"MR_DEVICE_ID\") LIKE :mrdeviceId||'%' AND";
        //        }
        //        if (objdevice.sFullName != null && objdevice.sFullName != "")
        //        {
        //            NpgsqlCommand.Parameters.AddWithValue("usfullName", objdevice.sFullName.ToUpper());
        //            sQry += " UPPER(\"US_FULL_NAME\") LIKE :usfullName||'%' AND ";
        //        }
        //        if (objdevice.soffcode != null && objdevice.soffcode != "")
        //        {
        //            sQry += " CAST( \"US_OFFICE_CODE\" AS TEXT) = '" + objdevice.soffcode + "' AND \"US_ID\" = '" + objdevice.sUserId + "'";
        //        }
        //        else
        //        {
        //            sQry += " CAST(\"US_OFFICE_CODE\" AS TEXT) like '" + objdevice.soffcode + "%' ";
        //        }

        //        dt = Objcon.FetchDataTable(sQry, NpgsqlCommand);
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //    return dt;
        //}
        #endregion

        /// <summary>
        /// Update Device Status
        /// </summary>
        /// <param name="objdevice"></param>
        /// <returns></returns>
        public bool UpdateDeviceStatus(clsDeviceRegister objdevice)
        {
            string[] iReturn = new string[0];
            bool bRes = false;
            string QryUPD = string.Empty;
            try
            {
                if (objdevice.sApprovalStatus == "2")
                {
                    #region inline query
                    //QryUPD = " UPDATE \"TBLMOBILEREGISTER\" SET \"MR_APPROVE_STATUS\"= '1', ";
                    //QryUPD += " \"MR_APPROVED_BY\"= '" + objdevice.sApprovedBy + "', ";
                    //QryUPD += " \"MR_APPROVED_ON\"= Now(),\"MR_STATUS\"='D' where \"MR_ID\" = '" + objdevice.sMuId + "' ";
                    //Objcon.FetchDataTable(QryUPD);
                    #endregion
                    NpgsqlCommand cmd = new NpgsqlCommand("proc_update_device_status");
                    cmd.Parameters.AddWithValue("approved_by", objdevice.sApprovedBy);
                    cmd.Parameters.AddWithValue("mobile_id", objdevice.sMuId);
                    Objcon.Execute(cmd, iReturn, 0);

                    bRes = true;
                }
                else
                {
                    NpgsqlCommand cmd = new NpgsqlCommand("proc_update_device_details");
                    cmd.Parameters.AddWithValue("requested_by", objdevice.sRequestedBy);
                    cmd.Parameters.AddWithValue("approved_by", objdevice.sApprovedBy);
                    cmd.Parameters.AddWithValue("mu_id", objdevice.sMuId);
                    Objcon.Execute(cmd, iReturn, 0);
                    bRes = true;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
                bRes = false;
            }
            return bRes;
        }
    }
}
