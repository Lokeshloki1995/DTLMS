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
using IIITS.DTLMS.BL.DataBase;

namespace IIITS.DTLMS.BL
{

    public class clsStore
    {
        string strFormCode = "clsStore";
        public string sSlNo { get; set; }
        public string sStoreCode { get; set; }
        public string sStoreName { get; set; }
        public string sStoreDescription { get; set; }
        public string sOfficeCode { get; set; }
        public string sCrby { get; set; }
        public string sStoreIncharge { get; set; }
        public string sAddress { get; set; }
        public string sEmailId { get; set; }
        public string sPhoneNo { get; set; }
        public string sMobile { get; set; }
        public string sStatus { get; set; }

        public string sEffectFrom { get; set; }
        public string sReason { get; set; }

        public string sOfficeName { get; set; }

        PGSqlConnection Objcon = new PGSqlConnection(Constants.Password);
        DataBseConnection ObjBseCon = new DataBseConnection(Constants.Password);
        NpgsqlCommand NpgsqlCommand;

        public string[] SaveUpdateStoreDetails(clsStore objStore)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string[] Arr = new string[3];
            string QryKey = string.Empty;
            string sResult = string.Empty;
            try
            {

                string[] strQryVallist = null;

                if (objStore.sOfficeCode != "")
                {
                    strQryVallist = objStore.sOfficeCode.Split(',');
                }

                if (objStore.sSlNo.Length == 0)
                {
                    foreach (string OfficeCode in strQryVallist)
                    {
                        #region
                        //string sQry = string.Empty;
                        //NpgsqlCommand.Parameters.AddWithValue("offcode", Convert.ToInt32(OfficeCode));
                        //sQry = "SELECT \"STO_ID\" FROM \"TBLSTOREOFFCODE\" WHERE \"STO_OFF_CODE\"  = :offcode";
                        //sResult = Objcon.get_value(sQry, NpgsqlCommand);
                        #endregion

                        QryKey = "GET_STO_ID";
                        NpgsqlCommand cmd_TC_ID_ON_TC_SLNO = new NpgsqlCommand("fetch_getvalue_clsstore");
                        cmd_TC_ID_ON_TC_SLNO.Parameters.AddWithValue("p_key", QryKey);
                        cmd_TC_ID_ON_TC_SLNO.Parameters.AddWithValue("p_value_1", Convert.ToString(OfficeCode ?? ""));
                        cmd_TC_ID_ON_TC_SLNO.Parameters.AddWithValue("p_value_2", "");
                        sResult = ObjBseCon.StringGetValue(cmd_TC_ID_ON_TC_SLNO);

                        if (sResult.Length > 0)
                        {
                            Arr[0] = "Location Already Allocated to some other Store";
                            Arr[1] = "4";
                            return Arr;
                        }
                    }
                }
                else
                {
                    foreach (string OfficeCode in strQryVallist)
                    {
                        #region Old inline query
                        //string sQry = string.Empty;
                        //NpgsqlCommand.Parameters.AddWithValue("offcode1", Convert.ToInt32(OfficeCode));
                        //NpgsqlCommand.Parameters.AddWithValue("slno", Convert.ToInt32(objStore.sSlNo));
                        //sQry = "SELECT \"STO_ID\" FROM \"TBLSTOREOFFCODE\" WHERE  \"STO_OFF_CODE\" =:offcode1 AND \"STO_SM_ID\" <> :slno";
                        //sResult = Objcon.get_value(sQry, NpgsqlCommand);
                        #endregion

                        QryKey = "GET_STO_ID_ON_STO_SM_ID";
                        NpgsqlCommand cmd_TC_ID_ON_TC_SLNO = new NpgsqlCommand("fetch_getvalue_clsstore");
                        cmd_TC_ID_ON_TC_SLNO.Parameters.AddWithValue("p_key", QryKey);
                        cmd_TC_ID_ON_TC_SLNO.Parameters.AddWithValue("p_value_1", Convert.ToString(OfficeCode ?? ""));
                        cmd_TC_ID_ON_TC_SLNO.Parameters.AddWithValue("p_value_2", Convert.ToString(objStore.sSlNo ?? ""));
                        sResult = ObjBseCon.StringGetValue(cmd_TC_ID_ON_TC_SLNO);

                        if (sResult.Length > 0)
                        {
                            Arr[0] = "Location Already Allocated to some other Store";
                            Arr[1] = "4";
                            return Arr;
                        }
                    }
                }

                NpgsqlCommand cmd = new NpgsqlCommand("proc_saveupdate_store");
                cmd.Parameters.AddWithValue("str_id", objStore.sSlNo);
                cmd.Parameters.AddWithValue("str_code", objStore.sStoreCode);
                cmd.Parameters.AddWithValue("store_name", objStore.sStoreName);
                cmd.Parameters.AddWithValue("store_desc", objStore.sStoreDescription);
                cmd.Parameters.AddWithValue("str_offcode", objStore.sOfficeCode);
                cmd.Parameters.AddWithValue("str_crby", objStore.sCrby);
                cmd.Parameters.AddWithValue("str_incharge", objStore.sStoreIncharge);
                cmd.Parameters.AddWithValue("str_mobile", objStore.sMobile);
                cmd.Parameters.AddWithValue("str_phone", objStore.sPhoneNo);
                cmd.Parameters.AddWithValue("str_address", objStore.sAddress);
                cmd.Parameters.AddWithValue("str_emailid", objStore.sEmailId);

                cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd.Parameters.Add("sd_id", NpgsqlDbType.Text);

                cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                cmd.Parameters["sd_id"].Direction = ParameterDirection.Output;

                Arr[0] = "msg";
                Arr[1] = "op_id";
                Arr[2] = "sd_id";
                Arr = Objcon.Execute(cmd, Arr, 3);

                if (Arr[1] == "0" || Arr[1] == "1")
                {
                    if (strQryVallist.Length > 0)
                    {
                        #region old inline query 
                        //string sQry = string.Empty;
                        //NpgsqlCommand.Parameters.AddWithValue("arr", Convert.ToInt32(Arr[2]));
                        //sQry = "DELETE FROM \"TBLSTOREOFFCODE\" WHERE \"STO_SM_ID\" =:arr";
                        //Objcon.ExecuteQry(sQry, NpgsqlCommand);
                        #endregion

                        string[] Arr_delete = new string[2];
                        NpgsqlCommand cmd_DELETE = new NpgsqlCommand("proc_delete_tblstoreoffcode_details");
                        cmd_DELETE.Parameters.AddWithValue("p_arr", Convert.ToString(Arr[2]));
                        cmd_DELETE.Parameters.Add("msg", NpgsqlDbType.Text);
                        cmd_DELETE.Parameters.Add("op_id", NpgsqlDbType.Text);
                        cmd_DELETE.Parameters["msg"].Direction = ParameterDirection.Output;
                        cmd_DELETE.Parameters["op_id"].Direction = ParameterDirection.Output;
                        Arr_delete[0] = "msg";
                        Arr_delete[1] = "op_id";
                        Arr_delete = Objcon.Execute(cmd_DELETE, Arr_delete, 2);
                    }


                    foreach (string OfficeCode in strQryVallist)
                    {
                        int sMaxNo = Convert.ToInt32(Objcon.Get_max_no("STO_ID", "TBLSTOREOFFCODE"));

                        #region Old inline query
                        //string sQry = string.Empty;
                        //// NpgsqlCommand.Parameters.AddWithValue("Getmaxno",Convert.ToInt32( Objcon.Get_max_no("STO_ID", "TBLSTOREOFFCODE")));
                        //// NpgsqlCommand.Parameters.AddWithValue("arr1",Convert.ToInt32(Arr[2]));
                        ////NpgsqlCommand.Parameters.AddWithValue("offcode",Convert.ToInt32( OfficeCode));
                        //sQry = "INSERT INTO \"TBLSTOREOFFCODE\"(\"STO_ID\", \"STO_SM_ID\", \"STO_OFF_CODE\")";
                        //sQry += " VALUES('" + sMaxNo + "','" + Convert.ToInt32(Arr[2]) + "','" + Convert.ToInt32(OfficeCode) + "')";
                        //Objcon.ExecuteQry(sQry);
                        #endregion

                        string[] Arr_INSERT = new string[2];
                        NpgsqlCommand cmd_INSERT = new NpgsqlCommand("proc_insert_tblstoreoffcode_details");
                        cmd_INSERT.Parameters.AddWithValue("p_sto_id", Convert.ToInt32(sMaxNo));
                        cmd_INSERT.Parameters.AddWithValue("p_sto_sm_id", Convert.ToInt32(Arr[2]));
                        cmd_INSERT.Parameters.AddWithValue("p_sto_off_code", Convert.ToInt32(OfficeCode));
                        cmd_INSERT.Parameters.Add("msg", NpgsqlDbType.Text);
                        cmd_INSERT.Parameters.Add("op_id", NpgsqlDbType.Text);
                        cmd_INSERT.Parameters["msg"].Direction = ParameterDirection.Output;
                        cmd_INSERT.Parameters["op_id"].Direction = ParameterDirection.Output;
                        Arr_INSERT[0] = "msg";
                        Arr_INSERT[1] = "op_id";
                        Arr_INSERT = Objcon.Execute(cmd_INSERT, Arr_INSERT, 2);

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
        public DataTable LoadStoreGrid(clsStore objStore)
        {
            DataTable dtStoreDetails = new DataTable();
            try
            {
                if (objStore.sOfficeCode.Length > 1)
                {
                    objStore.sOfficeCode = objStore.sOfficeCode.Substring(0, 2);
                }

                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_storedetails");
                cmd.Parameters.AddWithValue("office_code", objStore.sOfficeCode);
                dtStoreDetails = Objcon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return dtStoreDetails;
        }
        public object GetStoreDetails(clsStore objStore)
        {
            DataTable dtStoreDetails = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_storeview_details");
                cmd.Parameters.AddWithValue("store_id", objStore.sSlNo);

                dtStoreDetails = Objcon.FetchDataTable(cmd);

                if (dtStoreDetails.Rows.Count > 0)
                {
                    objStore.sSlNo = Convert.ToString(dtStoreDetails.Rows[0]["SM_ID"]);
                    //objStore.sStoreCode = Convert.ToString(dtStoreDetails.Rows[0]["SM_CODE"]);
                    objStore.sStoreDescription = Convert.ToString(dtStoreDetails.Rows[0]["SM_DESC"]);
                    objStore.sStoreName = Convert.ToString(dtStoreDetails.Rows[0]["SM_NAME"]);
                    objStore.sOfficeCode = Convert.ToString(dtStoreDetails.Rows[0]["SM_OFF_CODE"]);

                    objStore.sStoreIncharge = Convert.ToString(dtStoreDetails.Rows[0]["SM_STORE_INCHARGE"]);
                    objStore.sMobile = Convert.ToString(dtStoreDetails.Rows[0]["SM_MOBILENO"]);
                    objStore.sPhoneNo = Convert.ToString(dtStoreDetails.Rows[0]["SM_PHONENO"]);
                    objStore.sAddress = Convert.ToString(dtStoreDetails.Rows[0]["SM_ADDRESS"]);
                    objStore.sEmailId = Convert.ToString(dtStoreDetails.Rows[0]["SM_EMAILID"]);

                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return objStore;
        }
        public bool ActiveDeactiveStore(clsStore objStore)
        {
            string[] Arr = new string[3];
            bool bRes = false;
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_active_deactive_store");
                cmd.Parameters.AddWithValue("sm_status", objStore.sStatus);
                cmd.Parameters.AddWithValue("effect_from", objStore.sEffectFrom);
                cmd.Parameters.AddWithValue("reason", objStore.sReason);
                cmd.Parameters.AddWithValue("slno", objStore.sSlNo);

                cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);

                cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;


                Arr[0] = "msg";
                Arr[1] = "op_id";

                Arr = Objcon.Execute(cmd, Arr, 2);
                bRes = true;
            }
            catch (Exception ex)
            {
                bRes = false;
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return bRes;
        }
        public DataTable LoadOfficeDet(clsStore objstore)
        {
            DataTable dtLocation = new DataTable();
            try
            {
                #region Old inline query
                //NpgsqlCommand = new NpgsqlCommand();
                //string strQry = string.Empty;
                //strQry = "select \"OFF_CODE\",\"OFF_NAME\" FROM \"VIEW_ALL_OFFICES\" WHERE  \"OFF_NAME\" IS NOT NULL AND LENGTH(CAST(\"OFF_CODE\" AS TEXT)) = 3 ";
                //if (objstore.sOfficeCode != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("offcode", objstore.sOfficeCode);
                //    strQry += " AND CAST(\"OFF_CODE\" AS TEXT) LIKE :offcode||'%'";
                //}
                //if (objstore.sOfficeName != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("offname", objstore.sOfficeName.ToUpper());
                //    strQry += " AND UPPER(CAST(\"OFF_NAME\" AS TEXT)) LIKE :offname||'%'";
                //}
                //strQry += " order by \"OFF_CODE\"";
                //dtLocation = Objcon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_loadofficedet_for_clsstore");
                cmd.Parameters.AddWithValue("p_offcode", Convert.ToString(objstore.sOfficeCode ?? ""));
                cmd.Parameters.AddWithValue("p_offname", Convert.ToString(objstore.sOfficeName ?? "").ToUpper());
                dtLocation = Objcon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtLocation;
            }
            return dtLocation;
        }

        #region  Unused code
        //public string[] SaveUpdateStoreDetails(clsStore objStore)
        //{
        //    string[] Arr = new string[2];
        //    OleDbDataReader dr;
        //    string strQry = string.Empty;
        //    try
        //    {
        //        if (objStore.sSlNo == "")
        //        {

        //            dr = ObjCon.Fetch("select SM_CODE from TBLSTOREMAST where SM_CODE='" + objStore.sStoreCode + "' AND SM_STATUS='A'");
        //            if (dr.Read())
        //            {
        //                dr.Close();
        //                Arr[0] = "Store Code Already Exists";
        //                Arr[1] = "4";
        //                return Arr;

        //            }
        //            dr.Close();

        //            dr = ObjCon.Fetch("select SM_NAME from TBLSTOREMAST where UPPER(SM_NAME)='" + objStore.sStoreName.ToUpper() + "' AND SM_STATUS='A' ");
        //            if (dr.Read())
        //            {
        //                dr.Close();
        //                Arr[0] = "Store Name Already Exists";
        //                Arr[1] = "4";
        //                return Arr;

        //            }
        //            dr.Close();

        //            dr = ObjCon.Fetch("select SM_NAME from TBLSTOREMAST where SM_OFF_CODE='" + objStore.sOfficeCode + "' AND SM_STATUS='A' ");
        //            if (dr.Read())
        //            {
        //                dr.Close();
        //                Arr[0] = "Store Already Exists in Selected Division";
        //                Arr[1] = "4";
        //                return Arr;

        //            }
        //            dr.Close();

        //            string sMaxNo = Convert.ToString(ObjCon.Get_max_no("SM_ID", "TBLSTOREMAST"));
        //            strQry = "Insert into TBLSTOREMAST (SM_ID,SM_CODE,SM_NAME,SM_DESC,SM_OFF_CODE,SM_CRBY,SM_STORE_INCHARGE,";
        //            strQry+= " SM_MOBILENO,SM_PHONENO,SM_ADDRESS,SM_EMAILID) VALUES ";
        //            strQry += " ('" + sMaxNo + "','" + objStore.sStoreCode + "','" + objStore.sStoreName + "','" + objStore.sStoreDescription + "',";
        //            strQry += " '" + objStore.sOfficeCode + "','" + objStore.sCrby + "','" + objStore.sStoreIncharge + "','" + objStore.sMobile + "',";
        //            strQry+= " '" + objStore.sPhoneNo + "','" + objStore.sAddress + "','" + objStore.sEmailId + "') ";
        //            ObjCon.Execute(strQry);

        //            Arr[0] = sMaxNo;
        //            Arr[1] = "0";

        //            return Arr;

        //        }
        //        else
        //        {

        //            dr = ObjCon.Fetch("select SM_CODE from TBLSTOREMAST where SM_CODE='" + objStore.sStoreCode + "' AND SM_ID<>'" + objStore.sSlNo + "' AND SM_STATUS='A'");
        //            if (dr.Read())
        //            {
        //                dr.Close();
        //                Arr[0] = "Store Code Already Exists";
        //                Arr[1] = "4";
        //                return Arr;

        //            }
        //            dr.Close();

        //            dr = ObjCon.Fetch("select SM_NAME from TBLSTOREMAST WHERE SM_NAME='" + objStore.sStoreName + "'  AND SM_ID<>'" + objStore.sSlNo + "' AND SM_STATUS='A'");
        //            if (dr.Read())
        //            {
        //                dr.Close();
        //                Arr[0] = "Store Name Already Exists";
        //                Arr[1] = "4";
        //                return Arr;

        //            }
        //            dr.Close();

        //            dr = ObjCon.Fetch("select SM_NAME from TBLSTOREMAST where SM_OFF_CODE='" + objStore.sOfficeCode + "' AND SM_STATUS='A' AND SM_ID<>'" + objStore.sSlNo + "' ");
        //            if (dr.Read())
        //            {
        //                dr.Close();
        //                Arr[0] = "Store Already Exists in Selected Division";
        //                Arr[1] = "4";
        //                return Arr;

        //            }
        //            dr.Close();

        //            strQry = "UPDATE TBLSTOREMAST SET SM_CODE='" + objStore.sStoreCode + "',SM_NAME='" + objStore.sStoreName + "',";
        //            strQry += " SM_DESC='" + objStore.sStoreDescription + "',SM_OFF_CODE='" + objStore.sOfficeCode + "',SM_STORE_INCHARGE= ";
        //            strQry += " '" + objStore.sStoreIncharge + "',SM_MOBILENO='" + objStore.sMobile + "',SM_PHONENO='" + objStore.sPhoneNo + "', ";
        //            strQry += " SM_ADDRESS='" + objStore.sAddress + "',SM_EMAILID='" + objStore.sEmailId + "' WHERE SM_ID='" + objStore.sSlNo + "'";

        //            ObjCon.Execute(strQry);
        //            Arr[0] = "Store Details Updated Successfully";
        //            Arr[1] = "1";
        //            return Arr;

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "SaveUpdateStoreDetails");
        //        return Arr;

        //    }
        //}

        //public object GetStoreDetails(clsStore objStore)
        //{

        //    try
        //    {
        //        DataTable dtStoreDetails = new DataTable();
        //        OleDbDataReader dr;
        //        string strQry = string.Empty;

        //        strQry = "SELECT SM_ID,SM_CODE,SM_NAME,SM_DESC,SM_OFF_CODE,SM_STORE_INCHARGE,SM_MOBILENO,SM_PHONENO,SM_ADDRESS,SM_EMAILID FROM TBLSTOREMAST WHERE SM_ID='" + objStore.sSlNo + "'";
        //        dr = ObjCon.Fetch(strQry);
        //        dtStoreDetails.Load(dr);
        //        if (dtStoreDetails.Rows.Count > 0)
        //        {
        //            objStore.sSlNo = Convert.ToString(dtStoreDetails.Rows[0]["SM_ID"]);
        //            objStore.sStoreCode = Convert.ToString(dtStoreDetails.Rows[0]["SM_CODE"]);
        //            objStore.sStoreDescription = Convert.ToString(dtStoreDetails.Rows[0]["SM_DESC"]);
        //            objStore.sStoreName = Convert.ToString(dtStoreDetails.Rows[0]["SM_NAME"]);
        //            objStore.sOfficeCode = Convert.ToString(dtStoreDetails.Rows[0]["SM_OFF_CODE"]);

        //            objStore.sStoreIncharge = Convert.ToString(dtStoreDetails.Rows[0]["SM_STORE_INCHARGE"]);
        //            objStore.sMobile = Convert.ToString(dtStoreDetails.Rows[0]["SM_MOBILENO"]);
        //            objStore.sPhoneNo = Convert.ToString(dtStoreDetails.Rows[0]["SM_PHONENO"]);
        //            objStore.sAddress = Convert.ToString(dtStoreDetails.Rows[0]["SM_ADDRESS"]);
        //            objStore.sEmailId = Convert.ToString(dtStoreDetails.Rows[0]["SM_EMAILID"]);

        //        }
        //        return objStore;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadStoreDetails");
        //        return objStore;

        //    }
        //}

        //public DataTable LoadStoreGrid(clsStore objStore)
        //{
        //    OleDbDataReader dr;
        //    DataTable dtStoreDetails = new DataTable();
        //    string strQry = string.Empty;
        //    try
        //    {
        //        if (objStore.sOfficeCode.Length > 1)
        //        {
        //            objStore.sOfficeCode = objStore.sOfficeCode.Substring(0, 2);
        //        }

        //        //strQry = "SELECT (SELECT DIV_NAME from TBLDIVISION where DIV_CODE=SM_OFF_CODE) AS SM_OFF_CODE,SM_ID,SM_MOBILENO,SM_NAME,";
        //        //strQry += " SM_STORE_INCHARGE,SM_EMAILID,SM_STATUS FROM TBLSTOREMAST WHERE SM_OFF_CODE LIKE '"+ objStore.sOfficeCode +"'  ORDER BY SM_ID DESC";

        //        strQry = "SELECT (SELECT DIV_NAME from TBLDIVISION where DIV_CODE=SM_OFF_CODE) AS SM_OFF_CODE,SM_ID,SM_MOBILENO,SM_NAME,";
        //        strQry += " SM_STORE_INCHARGE,SM_EMAILID,SM_STATUS,";
        //        strQry += "CASE  WHEN TO_CHAR(SM_EFFECT_FROM,'YYYYMMDD') > TO_CHAR(SYSDATE,'YYYYMMDD') AND SM_STATUS='D' THEN 'A'  ELSE SM_STATUS END  SM_STATUS1 ";
        //        strQry += " FROM TBLSTOREMAST WHERE SM_OFF_CODE LIKE '" + objStore.sOfficeCode + "%'  ORDER BY SM_ID DESC";

        //        dr = ObjCon.Fetch(strQry);
        //        dtStoreDetails.Load(dr);
        //        return dtStoreDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadStoreGrid");
        //        return dtStoreDetails;

        //    }
        //}

        //public bool ActiveDeactiveStore(clsStore objStore)
        //{
        //    try
        //    {
        //        string strQry = string.Empty;
        //        strQry = "UPDATE TBLSTOREMAST SET SM_STATUS='" + objStore.sStatus + "',SM_EFFECT_FROM = TO_DATE('" + objStore.sEffectFrom + "','dd/MM/yyyy'),";
        //        strQry += " SM_REASON='" + objStore.sReason + "'   WHERE SM_ID='" + objStore.sSlNo + "'";
        //        ObjCon.Execute(strQry);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ActiveDeactiveStore");
        //        return false;
        //    }
        //}
        #endregion
    }
}