using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIITS.DTLMS.BL.POFlow
{
     public class ClsPMCPoMaster
    {
         PGSqlConnection Objcon = new PGSqlConnection(Constants.Password);
        string strFormCode = "ClsPMCPoMaster";
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
        public string DWAId { get; set; }
        public string DWANO { get; set; }
        public string WorkName { get; set; }
        public string DWAAmount { get; set; }
        public string LECNo { get; set; }
        public string DWADate { get; set; }
        public string DWAExpiryDate { get; set; }
        public string AvlAmount { get; set; }
        public string LECName { get; set; }
        public Int32  sRowIndex { get; set; }
        NpgsqlCommand NpgsqlCommand;
        /// <summary>
        /// to save po details
        /// </summary>
        /// <param name="objPoMaster"></param>
        /// <param name="Buffer"></param>
        /// <returns></returns>
        public string[] SavePoMaster(ClsPMCPoMaster objPoMaster,byte[] Buffer)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string strQry = string.Empty;
            string[] Arr = new string[3];
            string str;
            byte[] imageData = null;
            NpgsqlParameter docPhoto = new NpgsqlParameter();
            NpgsqlCommand comd = new NpgsqlCommand();
            try
            {
                if (objPoMaster.sPoId == "")
                {
                    #region inline query
                    //NpgsqlCommand.Parameters.AddWithValue("pono", objPoMaster.sPoNo.ToUpper());
                    //str = Objcon.get_value("select \"PMC_PO_NO\" from \"TBLPMC_POMASTER\" where\"PMC_PO_NO\"=:pono", NpgsqlCommand);
                    #endregion
                    DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
                    NpgsqlCommand cmd = new NpgsqlCommand("proc_get_pmc_pono");
                    cmd.Parameters.AddWithValue("p_key", "GETPMCPONO");
                    cmd.Parameters.AddWithValue("p_value", objPoMaster.sPoNo.ToUpper());
                    cmd.Parameters.AddWithValue("p_offcode", "");
                    str = objDatabse.StringGetValue(cmd);


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

                    cmd = new NpgsqlCommand("proc_saveupdate_pmc_po_masterdetails");
                    cmd.Parameters.AddWithValue("pomaster_id", objPoMaster.sPoId);
                    cmd.Parameters.AddWithValue("dwa_id", objPoMaster.DWAId);
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

                    #region inline query
                    //strQry = " UPDATE \"TBLPMC_POMASTER\" SET \"PMC_PO_DOC_PATH\"='" + objPoMaster.sFileExtension + "'   WHERE \"PMC_PO_ID\" = '" + objPoMaster.sPoId + "'";
                    //NpgsqlConnection objconn = new NpgsqlConnection();
                    //string strConnectionString = ConfigurationManager.ConnectionStrings["pgSQL"].ConnectionString + "Idea@12345+";
                    //objconn.ConnectionString = strConnectionString; 
                    //objconn.Open();
                    //comd = new NpgsqlCommand(strQry, objconn);
                    //comd.Parameters.Add(docPhoto);
                    //comd.ExecuteNonQuery();
                    //objconn.Close();
                    #endregion 
                    cmd = new NpgsqlCommand("proc_update_pmc_po_filepath");
                    cmd.Parameters.AddWithValue("doc_path", objPoMaster.sFileExtension);
                    cmd.Parameters.AddWithValue("po_id", objPoMaster.sPoId);
                    Objcon.Execute(cmd, Arr, 0);

                    for (int i = 0; i < objPoMaster.ddtCapacityGrid.Rows.Count; i++)
                    {
                        string[] Arr1 = new string[3];
                        NpgsqlCommand cmd1 = new NpgsqlCommand("proc_saveupdate_pmc_po_objectsdetails");
                        objPoMaster.sPbId = Convert.ToString(Objcon.Get_max_no("PMC_PB_ID", "TBLPMC_POOBJECTS"));
                        cmd1.Parameters.AddWithValue("pbobj_id", " ");
                        cmd1.Parameters.AddWithValue("pbobj_mas_id", objPoMaster.sPoId);
                        cmd1.Parameters.AddWithValue("pbobj_make", Convert.ToString(objPoMaster.ddtCapacityGrid.Rows[i]["PMC_MAKE_ID"]));
                        cmd1.Parameters.AddWithValue("pbobj_capacity", Convert.ToString(objPoMaster.ddtCapacityGrid.Rows[i]["PMC_PB_CAPACITY"]));
                        cmd1.Parameters.AddWithValue("pbobj_quantity", Convert.ToString(objPoMaster.ddtCapacityGrid.Rows[i]["PMC_PB_QUANTITY"]));
                        cmd1.Parameters.AddWithValue("pbobj_crby", objPoMaster.sCrBy);
                        cmd1.Parameters.AddWithValue("pbobj_rating", objPoMaster.ddtCapacityGrid.Rows[i]["PMC_PB_STARRATE"]);
                        cmd1.Parameters.AddWithValue("pbobj_status", "1");
                        Objcon.Execute(cmd1, Arr1, 0);
                    }
                    Arr[0] = "Saved Successfully";
                    Arr[1] = "0";
                    return Arr;
                }
                else
                {
                    #region inline query
                    //NpgsqlCommand.Parameters.AddWithValue("PoId",Convert.ToInt32(objPoMaster.sPoId));
                    //strQry = "UPDATE \"TBLPMC_POOBJECTS\" SET \"PMC_PB_PO_STATUS\"=0 WHERE \"PMC_PB_PO_ID\"=:PoId";
                    //Objcon.ExecuteQry(strQry, NpgsqlCommand);
                    #endregion
                    NpgsqlCommand cmd = new NpgsqlCommand("proc_update_pmc_po_status");
                    cmd.Parameters.AddWithValue("po_id", objPoMaster.sPoId);
                    Objcon.Execute(cmd, Arr, 0);

                     cmd = new NpgsqlCommand("proc_saveupdate_pmc_po_masterdetails");
                    cmd.Parameters.AddWithValue("pomaster_id", objPoMaster.sPoId);
                    cmd.Parameters.AddWithValue("dwa_id", objPoMaster.DWAId);
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
                        string[] Arr1 = new string[3];
                        NpgsqlCommand cmd1 = new NpgsqlCommand("proc_saveupdate_pmc_po_objectsdetails");
                        cmd1.Parameters.AddWithValue("pbobj_id", " ");
                        cmd1.Parameters.AddWithValue("pbobj_mas_id", objPoMaster.sPoId);
                        cmd1.Parameters.AddWithValue("pbobj_make", Convert.ToString(objPoMaster.ddtCapacityGrid.Rows[i]["PMC_MAKE_ID"]));
                        cmd1.Parameters.AddWithValue("pbobj_capacity", Convert.ToString(objPoMaster.ddtCapacityGrid.Rows[i]["PMC_PB_CAPACITY"]));
                        cmd1.Parameters.AddWithValue("pbobj_quantity", Convert.ToString(objPoMaster.ddtCapacityGrid.Rows[i]["PMC_PB_QUANTITY"]));
                        cmd1.Parameters.AddWithValue("pbobj_crby", objPoMaster.sCrBy);
                        cmd1.Parameters.AddWithValue("pbobj_rating", objPoMaster.ddtCapacityGrid.Rows[i]["PMC_PB_STARRATE"]);
                        cmd1.Parameters.AddWithValue("pbobj_status","1");
                        Objcon.Execute(cmd1, Arr1, 0);
                    }
                    return Arr;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }

        public DataTable GetPODoc(string sPoid)
        {
            NpgsqlCommand = new NpgsqlCommand();
            DataTable DtPODoc = new DataTable();
            try
            {
                #region inline query
                //NpgsqlCommand.Parameters.AddWithValue("poid",Convert.ToInt32(sPoid));
                //string strqry = "SELECT '' AS \"PO_DOC\",\"PO_DOC_EXT\" FROM \"TBLPMC_POMASTER\" WHERE \"PMC_PO_ID\"=:poid";
                //return Objcon.FetchDataTable(strqry, NpgsqlCommand);
                #endregion
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_pmc_po_doc");
                cmd.Parameters.AddWithValue("p_poid", Convert.ToInt32(sPoid));
                DtPODoc = Objcon.FetchDataTable(cmd);
                return DtPODoc;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return DtPODoc;
            }
        }

        public DataTable LoadPMCPoDetailGrid(ClsPMCPoMaster objPoMaster)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string strQry = string.Empty;
            DataTable dtPoDetails = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("public.proc_get_pmc_po_details_bydwa ");
                cmd.Parameters.AddWithValue("dwa_id", objPoMaster.DWANO.ToUpper());
                cmd.Parameters.AddWithValue("po_no", objPoMaster.sPoNo.ToUpper());
                return dtPoDetails = Objcon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtPoDetails;
            }
        }


        public object GetPoDetails(ClsPMCPoMaster objPoMaster)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string strQry = string.Empty;
            try
            {
                DataTable dtIndentDetails = new DataTable();
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_pmcpo_details_topo");
                cmd.Parameters.AddWithValue("po_id", objPoMaster.sPoId);
                dtIndentDetails = Objcon.FetchDataTable(cmd);


                objPoMaster.sPoId = Convert.ToString(dtIndentDetails.Rows[0]["PMC_PO_ID"]);
                objPoMaster.sPoNo = Convert.ToString(dtIndentDetails.Rows[0]["PMC_PO_NO"]);
                objPoMaster.sDate = Convert.ToString(dtIndentDetails.Rows[0]["PMC_PO_DATE"]);
                objPoMaster.sSupplierId = Convert.ToString(dtIndentDetails.Rows[0]["PMC_PO_SUPPLIER_ID"]);
                objPoMaster.sPoRate = Convert.ToString(dtIndentDetails.Rows[0]["PMC_PO_AMOUNT"]);
                objPoMaster.sDeliveryDate = Convert.ToString(dtIndentDetails.Rows[0]["PMC_PO_DLVR_SCLD"]);
                return objPoMaster;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objPoMaster;
            }
        }
        public DataTable LoadDeliveredCount(ClsPMCPoMaster objPoMaster)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string strQry = string.Empty;
            DataTable dtCapacityDetails = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_pmc_didetails_topo");
                cmd.Parameters.AddWithValue("po_id", objPoMaster.sPoId);
                cmd.Parameters.AddWithValue("capacity", objPoMaster.sCapacity);
                cmd.Parameters.AddWithValue("make_id", objPoMaster.sTcMake);
                dtCapacityDetails = Objcon.FetchDataTable(cmd);
                return dtCapacityDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtCapacityDetails;

            }
        }
        public DataTable LoadCapacityGrid(ClsPMCPoMaster objPoMaster)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string strQry = string.Empty;
            DataTable dtCapacityDetails = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_pmc_tcdetails_topo");
                cmd.Parameters.AddWithValue("po_id", objPoMaster.sPoId);
                dtCapacityDetails = Objcon.FetchDataTable(cmd);

                return dtCapacityDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtCapacityDetails;

            }
        }
        public object GetDWADetailstoPO(ClsPMCPoMaster objPO)
        {
            try
            {
                DataTable dtContractorDetails = new DataTable();

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_pmc_dwa_details_topo");
                cmd.Parameters.AddWithValue("dwa_id", objPO.DWAId);
                dtContractorDetails = Objcon.FetchDataTable(cmd);

                if (dtContractorDetails.Rows.Count > 0)
                {
                    objPO.WorkName = Convert.ToString(dtContractorDetails.Rows[0]["dm_name"]);
                    objPO.DWAAmount = Convert.ToString(dtContractorDetails.Rows[0]["dm_amount"]);
                    objPO.DWADate = Convert.ToString(dtContractorDetails.Rows[0]["dm_date"]);
                    objPO.DWAExpiryDate = Convert.ToString(dtContractorDetails.Rows[0]["dm_extended_upto"]);
                    objPO.AvlAmount = Convert.ToString(dtContractorDetails.Rows[0]["avl_amount"]);
                    objPO.LECNo = Convert.ToString(dtContractorDetails.Rows[0]["lm_number"]);
                    objPO.LECName = Convert.ToString(dtContractorDetails.Rows[0]["lm_contractor_name"]);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return objPO;
        }

        public DataTable GetLECExpiryDate(string DWAId)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string strQry = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("get_lec_expirtdate");
                cmd.Parameters.AddWithValue("dwa_id",Convert.ToInt32(DWAId));
                dt = Objcon.FetchDataTable(cmd);

                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }
        }

        public string getLECNo(string DWAId)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string strQry = string.Empty;
            string LECNo = string.Empty;
            try
            {
                #region inline query
                //string sQry = string.Empty;
                //sQry = "select \"LM_CONTRACTOR_NAME\" from \"TBLLECMASTER\" INNER JOIN \"TBLDWAMASTER\" ON \"DM_LM_ID\" = \"LM_ID\" WHERE \"DM_ID\"= '" + DWAId + "'";
                //LECNo = Objcon.get_value(sQry);
                #endregion
                DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_pmc_pono");
                cmd.Parameters.AddWithValue("p_key", "GETPMCLECNO");
                cmd.Parameters.AddWithValue("p_value", DWAId);
                cmd.Parameters.AddWithValue("p_offcode", "");
                LECNo = objDatabse.StringGetValue(cmd);

                return LECNo;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return LECNo;

            }
        }
        public string getDWAExpiry(int DWAId)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string strQry = string.Empty;
            string LECNo = string.Empty;
            try
            {
                #region inline query
                //string sQry = string.Empty;
                //sQry = "select  (CASE WHEN to_char(\"DM_EXTENDED_UPTO\",'mm/dd/yyyy') >= to_char(now(),'mm/dd/yyyy') AND \"DM_STATUS\" = 'A' THEN '1' ELSE '0' END ) ";
                //sQry += " AS \"dwa_flag\" from  \"TBLDWAMASTER\"  WHERE \"DM_ID\"= '" + DWAId + "'";
                //LECNo = Objcon.get_value(sQry);
                #endregion
                DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_pmc_pono");
                cmd.Parameters.AddWithValue("p_key", "GETPMCDWAEXPIRY");
                cmd.Parameters.AddWithValue("p_value", Convert.ToString(DWAId));
                cmd.Parameters.AddWithValue("p_offcode", "");
                LECNo = objDatabse.StringGetValue(cmd);

                return LECNo;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return LECNo;

            }
        }
        public DataTable GetDICreateOnPO(string DWAId)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string strQry = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("get_dicreate_inpo");
                cmd.Parameters.AddWithValue("dwa_id", Convert.ToInt32(DWAId));
                dt = Objcon.FetchDataTable(cmd);

                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;

            }
        }
    }
}
