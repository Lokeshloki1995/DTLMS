using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;
using System.Configuration;
using IIITS.DTLMS.BL.DataBase;

namespace IIITS.DTLMS.BL
{
    public class clsPoMaster
    {
        string strFormCode = "clsPoMaster";

        PGSqlConnection Objcon = new PGSqlConnection(Constants.Password);
        DataBseConnection ObjBasCon = new DataBseConnection(Constants.Password);
        NpgsqlCommand NpgsqlCommand;

        public DataTable ddtCapacityGrid { get; set; }
        public string sPoId { get; set; }
        public string sPoNo { get; set; }
        public string sDate { get; set; }
        public string sTcCapacity { get; set; }
        public string sTcMake { get; set; }
        public string sTcQuantity { get; set; }
        public string sCapacity { get; set; }
        public string sSupplierId { get; set; }
        public string sPoRate { get; set; }
        public string sPoDlvrdate { get; set; }
        public string sPbId { get; set; }
        public string sCrBy { get; set; }
        public string sFileName { get; set; }
        public string sDeliveryDate { get; set; }
        public string sFileExtension { get; set; }
        public Int32 sRowIndex { get; set; }

        /// <summary>
        /// Save Po Master
        /// </summary>
        /// <param name="objPoMaster"></param>
        /// <param name="Buffer"></param>
        /// <returns></returns>
        public string[] SavePoMaster(clsPoMaster objPoMaster, byte[] Buffer)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string strQry = string.Empty;
            string[] Arr = new string[3];
            string str;
            byte[] imageData = null;
            NpgsqlParameter docPhoto = new NpgsqlParameter();
            NpgsqlCommand comd = new NpgsqlCommand();
            string QryKey = string.Empty;
            try
            {
                if (objPoMaster.sPoId == "")
                {
                    //Objcon.BeginTransaction();
                    #region Old inline query
                    //NpgsqlCommand.Parameters.AddWithValue("pono", objPoMaster.sPoNo.ToUpper());
                    //str = Objcon.get_value("select \"PO_NO\" from \"TBLPOMASTER\" where\"PO_NO\"=:pono", NpgsqlCommand);
                    #endregion

                    QryKey = "GET_PO_NO";
                    NpgsqlCommand cmd_TBLPOMASTER = new NpgsqlCommand("fetch_getvalue_clspomaster");
                    cmd_TBLPOMASTER.Parameters.AddWithValue("p_key", QryKey);
                    cmd_TBLPOMASTER.Parameters.AddWithValue("p_value_1", Convert.ToString(objPoMaster.sPoNo ?? "").ToUpper());
                    cmd_TBLPOMASTER.Parameters.AddWithValue("p_value_2", "");
                    str = ObjBasCon.StringGetValue(cmd_TBLPOMASTER);

                    if (str != null && str != "")
                    {
                        Arr[0] = "Entered PO Number Already Exists";
                        Arr[1] = "2";
                        return Arr;
                    }

                    imageData = Buffer;
                    if (imageData != null)
                    {
                        docPhoto.ParameterName = "Document";
                        docPhoto.Value = imageData;
                    }
                    else
                    {
                        docPhoto.ParameterName = "Document";
                        docPhoto.Value = null;
                    }
                    #region Old inline query
                    //objPoMaster.sPoId = Convert.ToString(objCon.Get_max_no("PO_ID", "TBLPOMASTER"));
                    //strQry = "INSERT INTO TBLPOMASTER(PO_ID,PO_NO,PO_DATE,PO_CRBY,PO_CRON,PO_SUPPLIER_ID,PO_RATE)";
                    //strQry += " VALUES('" + objPoMaster.sPoId + "','" + objPoMaster.sPoNo + "',TO_DATE('" + objPoMaster.sDate + "','DD/MM/YYYY')";
                    //strQry += ",'" + objPoMaster.sCrBy + "',SYSDATE,'" + objPoMaster.sSupplierId + "','"+ objPoMaster.sPoRate +"')";
                    //objCon.Execute(strQry);
                    #endregion

                    NpgsqlCommand cmd = new NpgsqlCommand("proc_saveupdate_po_master");
                    cmd.Parameters.AddWithValue("pomaster_id", objPoMaster.sPoId);
                    cmd.Parameters.AddWithValue("pomaster_number", objPoMaster.sPoNo.ToUpper());
                    cmd.Parameters.AddWithValue("pomaster_crby", objPoMaster.sCrBy);
                    cmd.Parameters.AddWithValue("pomaster_date", objPoMaster.sDate);
                    cmd.Parameters.AddWithValue("pomaster_supplier_id", objPoMaster.sSupplierId);
                    cmd.Parameters.AddWithValue("pomaster_rate", objPoMaster.sPoRate);
                    cmd.Parameters.AddWithValue("podelivery_date", objPoMaster.sPoDlvrdate);
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
                    objPoMaster.sPoId = Arr[2].ToString();

                    #region Old inline query
                    //NpgsqlConnection objconn = new NpgsqlConnection();
                    //string strConnectionString = ConfigurationManager.ConnectionStrings["pgSQL"].ConnectionString + "Idea@12345+";
                    //objconn.ConnectionString = strConnectionString; // "Server=localhost;Port=5432;Database=TestTRM;User Id=postgres;Password =Idea123";  
                    //objconn.Open();
                    //strQry = " UPDATE \"TBLPOMASTER\" SET \"PO_DOC_EXT\"='" + objPoMaster.sFileExtension + "'   WHERE \"PO_ID\" = '" + objPoMaster.sPoId + "'";
                    //comd = new NpgsqlCommand(strQry, objconn);
                    //comd.Parameters.Add(docPhoto);
                    //comd.ExecuteNonQuery();
                    //objconn.Close();
                    #endregion


                    string[] Arr_TBLPOMASTER = new string[2];
                    NpgsqlCommand cmd_TBLPOOBJECTS = new NpgsqlCommand("proc_update_doc_ext_for_tblpomaster");
                    cmd_TBLPOOBJECTS.Parameters.AddWithValue("p_po_docext", Convert.ToString(objPoMaster.sFileExtension ?? ""));
                    cmd_TBLPOOBJECTS.Parameters.AddWithValue("p_poid", Convert.ToString(objPoMaster.sPoId ?? ""));
                    cmd_TBLPOOBJECTS.Parameters.Add("op_msg", NpgsqlDbType.Text);
                    cmd_TBLPOOBJECTS.Parameters.Add("o_op_id", NpgsqlDbType.Text);
                    cmd_TBLPOOBJECTS.Parameters["op_msg"].Direction = ParameterDirection.Output;
                    cmd_TBLPOOBJECTS.Parameters["o_op_id"].Direction = ParameterDirection.Output;                   
                    Arr_TBLPOMASTER[0] = "op_msg";
                    Arr_TBLPOMASTER[1] = "o_op_id";
                    Arr_TBLPOMASTER = Objcon.Execute(cmd_TBLPOOBJECTS, Arr_TBLPOMASTER, 2);

                    for (int i = 0; i < objPoMaster.ddtCapacityGrid.Rows.Count; i++)
                    {
                        string[] Arr1 = new string[3];
                        NpgsqlCommand cmd1 = new NpgsqlCommand("proc_saveupdate_po_objects");
                        objPoMaster.sPbId = Convert.ToString(Objcon.Get_max_no("PB_ID", "TBLPOOBJECTS"));
                        #region Old inline query
                        //strQry = "INSERT INTO TBLPOOBJECTS(PB_ID,PB_PO_ID,PB_MAKE,PB_CAPACITY,PB_QUANTITY,PB_CRBY,PB_CRON)";
                        //strQry += " VALUES('" + objPoMaster.sPbId + "','" + objPoMaster.sPoId + "','" + objPoMaster.ddtCapacityGrid.Rows[i]["MAKE_ID"] + "'";
                        //strQry += ",'" + objPoMaster.ddtCapacityGrid.Rows[i]["PB_CAPACITY"] + "','" + objPoMaster.ddtCapacityGrid.Rows[i]["PB_QUANTITY"] + "','" + objPoMaster.sCrBy + "',SYSDATE)";
                        //objCon.Execute(strQry);
                        #endregion
                        cmd1.Parameters.AddWithValue("pbobj_id", " ");
                        cmd1.Parameters.AddWithValue("pbobj_mas_id", objPoMaster.sPoId);
                        cmd1.Parameters.AddWithValue("pbobj_make", Convert.ToString(objPoMaster.ddtCapacityGrid.Rows[i]["MAKE_ID"]));
                        cmd1.Parameters.AddWithValue("pbobj_capacity", Convert.ToString(objPoMaster.ddtCapacityGrid.Rows[i]["PB_CAPACITY"]));
                        cmd1.Parameters.AddWithValue("pbobj_capacityid", Convert.ToString(objPoMaster.ddtCapacityGrid.Rows[i]["PB_CAPACITY_ID"]));
                        cmd1.Parameters.AddWithValue("pbobj_quantity", Convert.ToString(objPoMaster.ddtCapacityGrid.Rows[i]["PB_QUANTITY"]));
                        cmd1.Parameters.AddWithValue("pbobj_crby", objPoMaster.sCrBy);
                        cmd1.Parameters.AddWithValue("pbobj_rating", objPoMaster.ddtCapacityGrid.Rows[i]["PB_STARRATE"]);
                        cmd1.Parameters.AddWithValue("pbobj_status", "1");
                        Objcon.Execute(cmd1, Arr1, 0);
                    }
                    //Objcon.CommitTransaction();
                    Arr[0] = "Saved Successfully";
                    Arr[1] = "0";
                    return Arr;

                }
                else
                {

                    //Objcon.BeginTransaction();
                    #region Old inline query
                    //NpgsqlCommand.Parameters.AddWithValue("PoId", Convert.ToInt32(objPoMaster.sPoId));
                    //strQry = "UPDATE \"TBLPOOBJECTS\" SET \"PB_PO_STATUS\"=0 WHERE \"PB_PO_ID\"=:PoId";
                    //Objcon.ExecuteQry(strQry, NpgsqlCommand);
                    #endregion

                    string[] Arr_TBLPOOBJECTS = new string[2];
                    NpgsqlCommand cmd_TBLPOOBJECTS = new NpgsqlCommand("proc_update_tblpoobjects");
                    cmd_TBLPOOBJECTS.Parameters.AddWithValue("p_poid", Convert.ToString(objPoMaster.sPoId ?? ""));
                    cmd_TBLPOOBJECTS.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd_TBLPOOBJECTS.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd_TBLPOOBJECTS.Parameters["msg"].Direction = ParameterDirection.Output;
                    cmd_TBLPOOBJECTS.Parameters["op_id"].Direction = ParameterDirection.Output;
                    Arr_TBLPOOBJECTS[0] = "msg";
                    Arr_TBLPOOBJECTS[1] = "op_id";
                    Arr_TBLPOOBJECTS = Objcon.Execute(cmd_TBLPOOBJECTS, Arr_TBLPOOBJECTS, 2);


                    NpgsqlCommand cmd = new NpgsqlCommand("proc_saveupdate_po_master");
                    cmd.Parameters.AddWithValue("pomaster_id", objPoMaster.sPoId);
                    cmd.Parameters.AddWithValue("pomaster_number", objPoMaster.sPoNo.ToUpper());
                    cmd.Parameters.AddWithValue("pomaster_crby", objPoMaster.sCrBy);
                    cmd.Parameters.AddWithValue("pomaster_date", objPoMaster.sDate);
                    cmd.Parameters.AddWithValue("pomaster_supplier_id", objPoMaster.sSupplierId);
                    cmd.Parameters.AddWithValue("pomaster_rate", objPoMaster.sPoRate);
                    cmd.Parameters.AddWithValue("podelivery_date", objPoMaster.sPoDlvrdate);
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
                    objPoMaster.sPoId = Arr[2].ToString();

                    for (int i = 0; i < objPoMaster.ddtCapacityGrid.Rows.Count; i++)
                    {

                        objPoMaster.sPbId = Convert.ToString(Objcon.Get_max_no("PB_ID", "TBLPOOBJECTS"));

                        #region Old inline query
                        //strQry = "INSERT INTO TBLPOOBJECTS(PB_ID,PB_PO_ID,PB_MAKE,PB_CAPACITY,PB_QUANTITY,PB_CRBY,PB_CRON)";
                        //strQry += " VALUES('" + objPoMaster.sPbId + "','" + objPoMaster.sPoId + "','" + objPoMaster.ddtCapacityGrid.Rows[i]["MAKE_ID"] + "'";
                        //strQry += ",'" + objPoMaster.ddtCapacityGrid.Rows[i]["PB_CAPACITY"] + "','" + objPoMaster.ddtCapacityGrid.Rows[i]["PB_QUANTITY"] + "','" + objPoMaster.sCrBy + "',SYSDATE)";
                        //objCon.Execute(strQry);
                        #endregion

                        string[] Arr1 = new string[3];
                        NpgsqlCommand cmd1 = new NpgsqlCommand("proc_saveupdate_po_objects");
                        cmd1.Parameters.AddWithValue("pbobj_id", " ");
                        cmd1.Parameters.AddWithValue("pbobj_mas_id", objPoMaster.sPoId);
                        cmd1.Parameters.AddWithValue("pbobj_make", Convert.ToString(objPoMaster.ddtCapacityGrid.Rows[i]["MAKE_ID"]));
                        cmd1.Parameters.AddWithValue("pbobj_capacity", Convert.ToString(objPoMaster.ddtCapacityGrid.Rows[i]["PB_CAPACITY"]));
                        cmd1.Parameters.AddWithValue("pbobj_capacityid", Convert.ToString(objPoMaster.ddtCapacityGrid.Rows[i]["PB_CAPACITY_ID"]));
                        cmd1.Parameters.AddWithValue("pbobj_quantity", Convert.ToString(objPoMaster.ddtCapacityGrid.Rows[i]["PB_QUANTITY"]));
                        cmd1.Parameters.AddWithValue("pbobj_crby", objPoMaster.sCrBy);
                        cmd1.Parameters.AddWithValue("pbobj_rating", objPoMaster.ddtCapacityGrid.Rows[i]["PB_STARRATE"]);
                        cmd1.Parameters.AddWithValue("pbobj_status", "1");
                        Objcon.Execute(cmd1, Arr1, 0);
                    }
                    #region
                    //Objcon.CommitTransaction();
                    //Arr[0] = "Updated Successfully";
                    //Arr[1] = "0";
                    #endregion
                    return Arr;
                }
            }
            catch (Exception ex)
            {
                // Objcon.RollBackTrans();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }
        /// <summary>
        /// Get PO Doc
        /// </summary>
        /// <param name="sPoid"></param>
        /// <returns></returns>
        public DataTable GetPODoc(string sPoid)
        {
            DataTable DtPODoc = new DataTable();
            try
            {
                #region old GetPODoc
                //string Qry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("poid", Convert.ToInt32(sPoid));
                //Qry = "SELECT \"PO_DOC\",\"PO_DOC_EXT\" FROM \"TBLPOMASTER\" WHERE \"PO_ID\"=:poid";
                //DtPODoc = Objcon.FetchDataTable(Qry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_po_doc");
                cmd.Parameters.AddWithValue("p_poid", (sPoid ?? ""));
                DtPODoc = Objcon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return DtPODoc;
            }
            return DtPODoc;
        }
        /// <summary>
        /// Load Po Detail Grid
        /// </summary>
        /// <param name="objPoMaster"></param>
        /// <returns></returns>
        public DataTable LoadPoDetailGrid(clsPoMaster objPoMaster)
        {
            DataTable dtPoDetails = new DataTable();
            try
            {
                #region OLD LoadPoDetailGrid
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = " SELECT \"PO_ID\",\"PO_NO\",TO_CHAR(\"PO_DATE\",'dd-MON-yyyy') \"PO_DATE\",SUM(\"PB_QUANTITY\")\"PB_QUANTITY\", ";
                //strQry += " (SELECT \"TS_NAME\" FROM \"TBLTRANSSUPPLIER\" WHERE \"TS_ID\"=\"PO_SUPPLIER_ID\") ";
                //strQry += " \"PO_SUPPLIER_ID\" FROM \"TBLPOMASTER\",";
                //strQry += " \"TBLPOOBJECTS\" WHERE \"PO_ID\"=\"PB_PO_ID\" and \"PB_PO_STATUS\"=1 ";
                //if (objPoMaster.sPoNo != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("pono", objPoMaster.sPoNo.ToUpper());
                //    strQry += " and  UPPER(\"PO_NO\") like :pono||'%'";
                //}
                //strQry += " GROUP BY \"PO_ID\",\"PO_NO\",\"PO_DATE\",\"PO_SUPPLIER_ID\" ";
                //dtPoDetails = Objcon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_lode_po_grid_details");
                cmd.Parameters.AddWithValue("p_pono", objPoMaster.sPoNo.ToUpper());
                dtPoDetails = Objcon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtPoDetails;
            }
            return dtPoDetails;
        }
        /// <summary>
        /// Get Po Details
        /// </summary>
        /// <param name="objPoMaster"></param>
        /// <returns></returns>
        public object GetPoDetails(clsPoMaster objPoMaster)
        {
            DataTable dtIndentDetails = new DataTable();
            try
            {
                #region Old GetPoDetails
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("poid", Convert.ToInt32(objPoMaster.sPoId));
                //strQry = " select \"PO_ID\",\"PO_NO\",TO_CHAR(\"PO_DATE\",'dd/MM/yyyy')\"PO_DATE\",\"PO_SUPPLIER_ID\", ";
                //strQry += " CAST(\"PO_RATE\" AS TEXT),TO_CHAR(\"PO_DLVR_SCLD\",'dd/MM/yyyy') \"PO_DLVR_SCLD\" from \"TBLPOMASTER\" ";
                //strQry += " WHERE  \"PO_ID\"=:poid";
                //dtIndentDetails = Objcon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_po_details");
                cmd.Parameters.AddWithValue("p_poid", objPoMaster.sPoId);
                dtIndentDetails = Objcon.FetchDataTable(cmd);

                if (dtIndentDetails.Rows.Count > 0)
                {
                    objPoMaster.sPoId = Convert.ToString(dtIndentDetails.Rows[0]["PO_ID"]);
                    objPoMaster.sPoNo = Convert.ToString(dtIndentDetails.Rows[0]["PO_NO"]);
                    objPoMaster.sDate = Convert.ToString(dtIndentDetails.Rows[0]["PO_DATE"]);
                    objPoMaster.sSupplierId = Convert.ToString(dtIndentDetails.Rows[0]["PO_SUPPLIER_ID"]);
                    objPoMaster.sPoRate = Convert.ToString(dtIndentDetails.Rows[0]["PO_RATE"]);
                    objPoMaster.sDeliveryDate = Convert.ToString(dtIndentDetails.Rows[0]["PO_DLVR_SCLD"]);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objPoMaster;
            }
            return objPoMaster;
        }
        /// <summary>
        /// Load Delivered Count
        /// </summary>
        /// <param name="objPoMaster"></param>
        /// <returns></returns>
        public DataTable LoadDeliveredCount(clsPoMaster objPoMaster)
        {
            DataTable dtCapacityDetails = new DataTable();
            try
            {
                #region old LoadDeliveredCount
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("poid", Convert.ToInt16(objPoMaster.sPoId));
                //strQry = " select \"DIM_DI_NO\", sum(\"DI_QUANTITY\") as \"DELIVERED\" from \"TBLDELIVERYINSTRUCTION\" ";
                //strQry += " INNER JOIN \"TBLDELIVERYINSTMASTER\" ON \"DIM_ID\"=\"DI_DIM_ID\" where \"DI_PO_ID\"=:poid ";
                //strQry += " AND \"DI_CAPACITY\"=" + objPoMaster.sCapacity + " AND \"DI_STATUS\"=1 ";
                //strQry += " AND \"DI_MAKE_ID\"=" + objPoMaster.sTcMake + " GROUP BY \"DIM_DI_NO\"";
                //dtCapacityDetails = Objcon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_delivered_count");
                cmd.Parameters.AddWithValue("p_poid", objPoMaster.sPoId);
                cmd.Parameters.AddWithValue("p_capacity", objPoMaster.sCapacity);
                cmd.Parameters.AddWithValue("p_tcmake", objPoMaster.sTcMake);
                dtCapacityDetails = Objcon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtCapacityDetails;
            }
            return dtCapacityDetails;
        }
        /// <summary>
        /// Load Capacity Grid
        /// </summary>
        /// <param name="objPoMaster"></param>
        /// <returns></returns>
        public DataTable LoadCapacityGrid(clsPoMaster objPoMaster)
        {
            DataTable dtCapacityDetails = new DataTable();
            try
            {
                #region Old LoadCapacityGrid
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("poid", Convert.ToInt32(objPoMaster.sPoId));
                //strQry = "select \"PO_ID\",\"PO_NO\",TO_CHAR(\"PO_DATE\",'dd-MON-yyyy') PO_DATE,(SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"PB_MAKE\") PB_MAKE ,\"PB_MAKE\" AS \"MAKE_ID\", CAST(\"PB_CAPACITY\" as TEXT) as PB_CAPACITY ,";
                //strQry += " \"PB_QUANTITY\",\"PB_CAPACITY_ID\",\"PB_PO_ID\",\"PB_ID\",\"PO_RATING\" \"PB_STARRATE\",\"MD_NAME\" AS \"PB_STAR_NAME\" from \"TBLPOMASTER\",\"TBLMASTERDATA\",\"TBLPOOBJECTS\"  WHERE \"PO_ID\"= \"PB_PO_ID\" and \"MD_TYPE\"='SR' and cast(\"PO_RATING\" as int8)=\"MD_ID\" and \"PO_ID\"=:poid and \"PB_PO_STATUS\"=1 ";
                //dtCapacityDetails = Objcon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_capacity_grid");
                cmd.Parameters.AddWithValue("p_poid", Convert.ToInt32(objPoMaster.sPoId));
                dtCapacityDetails = Objcon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtCapacityDetails;
            }
            return dtCapacityDetails;
        }
    }
}
