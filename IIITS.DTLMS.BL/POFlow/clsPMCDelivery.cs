using IIITS.PGSQL.DAL;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using NpgsqlTypes;
using System.Reflection;

namespace IIITS.DTLMS.BL.POFlow
{
    public class clsPMCDelivery
    {
        PGSqlConnection Objcon = new PGSqlConnection(Constants.Password);
        NpgsqlCommand cmd;
        NpgsqlCommand NpgsqlCommand;

        public DataTable Dt_PMCDelivery { get; set; } = new DataTable("TBLPMC_DELIVERYINSTMASTER");

        public string strFormCode = "clsPMCDelivery";

        //Delivery Inst Variables.
        public string PMCDIid { get; set; }
        public string PMCLecNAme { get; set; }
        public string PMCDINumber { get; set; }
        public string PMCDIDate { get; set; }
        public string PMCDueDate { get; set; }
        public string PmcLecNum { get; set; }
        public string PmcDwaStatus { get; set; }
        public string PmcDwaValUpTO { get; set; }

        public string PmcLecStatus { get; set; }
        public string PmcLecUpTo { get; set; }

        //PO Varabls.
        public string PMCPoid { get; set; }
        public string PMCPoNumber { get; set; }

        //DWA Variabls.       
        public string PMCDWANumber { get; set; }

        // LEC Variabls.
        public string PmcDwaExp { get; set; }
        public string PmcPoDate { get; set; }

        //DTr variabls.       
        public string DTrCapacity { get; set; }
        public string FileExt { get; set; }
        public string TotalTC { get; set; }
        public string DIid { get; set; }
        public string DINo { get; set; }
        public string PONumber { get; set; }
        public DataTable dtDelivery { get; set; }
        public Byte[] POFile { get; set; }
        public string Capacity { get; set; }
        public string TcMake { get; set; }
        public string Store { get; set; }
        public string Crby { get; set; }
        /// <summary>
        /// Get's DeliveryIns Details.
        /// </summary>
        /// <returns></returns>
        public DataTable GetDeliveryInstDetails(clsPMCDelivery Obj)
        {
            try
            {
                cmd = new NpgsqlCommand("proc_get_pmc_deliveryinst_details");
                cmd.Parameters.AddWithValue("p_pmc_dim_id", Obj.PMCDIid);
                Dt_PMCDelivery = Objcon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return Dt_PMCDelivery;
        }
        /// <summary>
        /// Filter the Grid Data bsed on User in put.
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public DataTable FetchPMCDeliveryInsDetails(clsPMCDelivery Obj)
        {
            DataTable DtPMCDIDetails = new DataTable();
            try
            {
                cmd = new NpgsqlCommand("proc_fetch_pmcdeliveryins_details");
                cmd.Parameters.AddWithValue("p_pmc_dwa_no", (Obj.PMCDWANumber ?? ""));
                cmd.Parameters.AddWithValue("p_pmc_po_no", (Obj.PMCPoNumber ?? ""));
                cmd.Parameters.AddWithValue("p_pmc_di_no", (Obj.PMCDINumber ?? ""));
                DtPMCDIDetails = Objcon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return DtPMCDIDetails;
        }
        /// <summary>
        /// get tc Quantity
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public object GetPurchaseCount(clsPMCDelivery obj)
        {
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("sp_get_pmcgetpurchasecount");
                cmd.Parameters.AddWithValue("po_no", PMCPoNumber);
                dt = Objcon.FetchDataTable(cmd);
                if (dt.Rows.Count > 0)
                {
                    obj.TotalTC = Convert.ToString(dt.Rows[0]["sPMC_PB_QUANTITY"]);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "0";
            }
            return obj;
        }
        /// <summary>
        /// get poId
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetPOId(clsPMCDelivery obj)
        {
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("sp_get_pmcgetpoid");
                cmd.Parameters.AddWithValue("po_no", PMCPoNumber);
                dt = Objcon.FetchDataTable(cmd);

                if (dt.Rows.Count > 0)
                {
                    obj.PMCPoid = Convert.ToString(dt.Rows[0]["sPMC_PO_ID"]);
                }
                return obj.PMCPoid;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "0";
            }
        }
        /// <summary>
        /// get allotment count
        /// </summary>
        /// <param name="objDelivery"></param>
        /// <returns></returns>
        public DataTable GetAllotmentCount(clsPMCDelivery objDelivery)
        {
            PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
            NpgsqlCommand NpgsqlCommand = new NpgsqlCommand();
            DataTable dtIndentDetails = new DataTable();
            try
            {
                #region inline query
                //string strQry = string.Empty;
                //NpgsqlCommand.Parameters.AddWithValue("DINo", Convert.ToString(objDelivery.DINo));

                //strQry = " select \"ALT_NO\", sum(\"ALT_QUANTITY\") as \"ALLOTED\" from \"TBLALLOTEMENT\" where ";
                //strQry += " \"ALT_DI_NO\"=:DINo  and \"ALT_CAPACITY\"='" + objDelivery.Capacity + "' ";
                //strQry += " AND  \"ALT_MAKE_ID\" ='" + objDelivery.TcMake + "' ";
                //strQry += " and \"ALT_STORE_ID\"= '" + objDelivery.Store + "'    GROUP BY \"ALT_NO\"";
                //dtIndentDetails = objcon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_pmc_allotment_count");
                cmd.Parameters.AddWithValue("di_no", Convert.ToString(objDelivery.DINo));
                cmd.Parameters.AddWithValue("capacity", objDelivery.Capacity);
                cmd.Parameters.AddWithValue("tc_make", objDelivery.TcMake);
                cmd.Parameters.AddWithValue("store", objDelivery.Store);
                dtIndentDetails = Objcon.FetchDataTable(cmd);

                return dtIndentDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtIndentDetails;
            }
        }
        #region unused methods
        /// <summary>
        /// get DiDetails
        /// </summary>
        /// <param name="objDelivery"></param>
        /// <returns></returns>
        //public DataTable GetDIDetails(clsPMCDelivery objDelivery)
        //{
        //    PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
        //    NpgsqlCommand NpgsqlCommand = new NpgsqlCommand();
        //    DataTable dtIndentDetails = new DataTable();
        //    try
        //    {
        //        string strQry = string.Empty;
        //        NpgsqlCommand.Parameters.AddWithValue("DINo", Convert.ToString(objDelivery.DINo));
        //        strQry = "select \"DI_ID\",\"DI_NO\",\"DI_CONSIGNEE\",TO_CHAR(\"DI_DUEDATE\",'dd-MM-yyyy')\"DI_DUEDATE\",";
        //        strQry += " \"DI_STORE_ID\",TO_CHAR(\"DI_DATE\",'dd-MM-yyyy') ";
        //        strQry += " \"DI_DATE\",\"DI_MAKE_ID\", \"DI_CAPACITY\",\"DI_CAPACITY_ID\", \"DI_STARTTYPE\", ";
        //        strQry += "\"DI_QUANTITY\"  from \"TBLDELIVERYINSTRUCTION\" WHERE  \"DI_NO\"=:DINo ";
        //        dtIndentDetails = objcon.FetchDataTable(strQry, NpgsqlCommand);
        //        return dtIndentDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(
        //            MethodBase.GetCurrentMethod().DeclaringType.Name,
        //            MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return dtIndentDetails;
        //    }
        //}

        /// <summary>
        /// Get PO Image
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        //public object GetPOImage(clsPMCDelivery obj)
        //{
        //    cmd = new NpgsqlCommand();
        //    try
        //    {
        //        cmd.Parameters.AddWithValue("phno", obj.PONumber);
        //        string sQry = "SELECT \"PO_DOC\", \"PO_DOC_EXT\" FROM \"TBLPOMASTER\" ";
        //        sQry += " WHERE  \"PO_DOC\" IS NOT NULL AND \"PO_ID\" = (SELECT DISTINCT \"DI_PO_ID\" FROM ";
        //        sQry += " \"TBLDELIVERYINSTRUCTION\" WHERE \"DIM_DI_NO\" =:phno)";
        //        DataTable dt = new DataTable();
        //        dt = Objcon.FetchDataTable(sQry, cmd);
        //        if (dt.Rows.Count > 0)
        //        {
        //            Byte[] bytes = (Byte[])dt.Rows[0]["PO_DOC"];
        //            obj.FileExt = Convert.ToString(dt.Rows[0]["PO_DOC_EXT"]);
        //            obj.POFile = bytes;
        //        }
        //        else
        //        {
        //            obj.FileExt = "";
        //            obj.POFile = null;
        //        }
        //        return obj;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(
        //            MethodBase.GetCurrentMethod().DeclaringType.Name,
        //            MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return obj;
        //    }
        //}
        ///// <summary>
        ///// Get DI Image
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public object GetDIImage(clsPMCDelivery obj)
        //{
        //    cmd = new NpgsqlCommand();
        //    Byte[] POImage = null;
        //    try
        //    {
        //        cmd.Parameters.AddWithValue("phno", obj.PONumber);
        //        string sQry = "SELECT DISTINCT \"DI_INST_FILE\", \"DI_INST_FILE_EXT\" FROM \"TBLDELIVERYINSTRUCTION\" ";
        //        sQry += " WHERE \"DIM_DI_NO\" =:phno AND \"DI_INST_FILE\" IS NOT NULL ";
        //        DataTable dt = new DataTable();
        //        dt = Objcon.FetchDataTable(sQry, cmd);
        //        if (dt.Rows.Count > 0)
        //        {
        //            Byte[] bytes = (Byte[])dt.Rows[0]["DI_INST_FILE"];
        //            obj.FileExt = Convert.ToString(dt.Rows[0]["DI_INST_FILE_EXT"]);
        //            obj.POFile = bytes;
        //        }
        //        else
        //        {
        //            obj.FileExt = "";
        //            obj.POFile = null;
        //        }
        //        return obj;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(
        //            MethodBase.GetCurrentMethod().DeclaringType.Name,
        //            MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return POImage;
        //    }
        //}
        #endregion
        /// <summary>
        /// get Di Completed Details
        /// </summary>
        /// <param name="sDI_no"></param>
        /// <returns></returns>
        public DataTable GetDeliveredDetails(string sDI_no)
        {
            NpgsqlCommand = new NpgsqlCommand();
            DataTable dt = new DataTable();
            try
            {
                #region inline query 
                //string sQry = string.Empty;
                //NpgsqlCommand.Parameters.AddWithValue("DIno", sDI_no);
                //sQry = "  SELECT \"PMC_DI_ID\" as \"DI_ID\",\"PMC_DIM_DI_NO\" as \"DIM_DI_NO\",\"PMC_DI_PO_ID\" as \"DI_PO_ID\", ";
                //sQry += "  TO_CHAR(\"PMC_DIM_DI_DATE\",'DD/MM/YYYY') AS \"DI_DATE\", ";
                //sQry += " TO_CHAR(\"PMC_DIM_DUE_DATE\",'DD/MM/YYYY') AS \"DI_DUEDATE\", \"PMC_DI_STORE_ID\" as ";
                //sQry += " \"DI_STORE_ID\",\"SM_NAME\" AS \"DI_STORE\",";
                //sQry += " \"PMC_DI_MAKE_ID\" as \"DI_MAKE_ID\" ,\"TM_NAME\" AS \"DI_MAKE\",\"PMC_DI_CAPACITY\" as ";
                //sQry += " \"DI_CAPACITY\",\"PMC_DI_CAPACITY_ID\" as \"DI_CAPACITY_ID\" ";
                //sQry += " ,\"MD_NAME\"AS \"DI_STARRATENAME\", ";
                //sQry += " \"PMC_DI_STARTTYPE\" AS \"DI_STARRATE\",\"PMC_DI_QUANTITY\" as \"DI_QUANTITY\"  ,";
                //sQry += " \"PMC_DI_FILE_EXT\" as \"DI_FILE_EXT\",\"PMC_DI_TC_START_RANGE\" AS ";
                //sQry += " \"DI_START_RANGE\",\"PMC_DI_TC_END_RANGE\" AS \"DI_END_RANGE\",'HP'||\"PMC_DI_TC_START_RANGE_VAR\" AS ";
                //sQry += " \"PMC_DI_TC_START_RANGE_VAR\",'HP'||\"PMC_DI_TC_END_RANGE_VAR\" AS \"PMC_DI_TC_END_RANGE_VAR\" FROM ";
                //sQry += " \"TBLPMC_DELIVERYINSTRUCTION\" ,";
                //sQry += " \"TBLPMC_DELIVERYINSTMASTER\" ,\"TBLTRANSMAKES\",\"TBLSTOREMAST\"  ,\"TBLMASTERDATA\" WHERE ";
                //sQry += "\"PMC_DIM_ID\"=\"PMC_DI_DIM_ID\" ";
                //sQry += " and \"PMC_DI_STORE_ID\"=\"SM_ID\"  AND \"TM_ID\"=\"PMC_DI_MAKE_ID\" AND \"MD_TYPE\"='SR'";
                //sQry += "AND \"MD_ID\"=\"PMC_DI_STARTTYPE\" ";
                //sQry += " and \"PMC_DIM_DI_NO\" like  :DIno||'%'  ORDER BY \"PMC_DI_ID\" ";
                //dt = Objcon.FetchDataTable(sQry, NpgsqlCommand);
                #endregion
                {
                    cmd = new NpgsqlCommand("sp_get_pmcget_delivereddetails");
                    cmd.Parameters.AddWithValue("DIno", sDI_no);
                    dt = Objcon.FetchDataTable(cmd);
                }

              return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        /// <summary>
        /// to get po  and dwa and lec details
        /// </summary>
        /// <param name="objDel"></param>
        /// <returns></returns>
        public DataTable GetDeliveryDetails(clsPMCDelivery objDel)
        {
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("sp_get_pmcdeliverydetails");
                cmd.Parameters.AddWithValue("poid", objDel.PMCPoid == null ? "" : objDel.PMCPoid);
                cmd.Parameters.AddWithValue("po_no", objDel.PMCPoNumber == null ? "" : objDel.PMCPoNumber);
                cmd.Parameters.AddWithValue("capcity", objDel.DTrCapacity == null ? "" : objDel.DTrCapacity);
                dt = Objcon.FetchDataTable(cmd);
                if (dt.Rows.Count > 0)
                {
                    objDel.PMCLecNAme = Convert.ToString(dt.Rows[0]["sLM_CONTRACTOR_NAME"]);
                    objDel.PMCDINumber = Convert.ToString(dt.Rows[0]["sDM_NUMBER"]);
                }
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        /// <summary>
        /// Get Dwa ExpDate And PoDate
        /// </summary>
        /// <param name="objDel"></param>
        /// <returns></returns>
        public DataTable GetDwaExpAndPoDate(clsPMCDelivery objDel)
        {
            DataTable dt = new DataTable();
            string strResult = string.Empty;

            try
            {

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_pmc_dwaexpiredate_and_podate");

                cmd.Parameters.AddWithValue("poid", objDel.PMCPoid == null ? "" : objDel.PMCPoid);

                dt = Objcon.FetchDataTable(cmd);
                if (dt.Rows.Count > 0)
                {
                    objDel.PmcDwaExp = Convert.ToString(dt.Rows[0]["sDM_EXTENDED_UPTO"]);
                    objDel.PmcPoDate = Convert.ToString(dt.Rows[0]["sPMC_PO_DATE"]);
                    objDel.PmcLecUpTo = Convert.ToString(dt.Rows[0]["sLM_VALID_UPTO"]);
                    objDel.PmcLecStatus = Convert.ToString(dt.Rows[0]["sLM_STATUS"]);
                    objDel.PmcDwaValUpTO = Convert.ToString(dt.Rows[0]["sDM_EXTENDED_UPTO"]);
                    objDel.PmcDwaStatus = Convert.ToString(dt.Rows[0]["sDM_STATUS"]);
                    objDel.PMCDINumber = Convert.ToString(dt.Rows[0]["sDM_NUMBER"]);
                    objDel.PmcLecNum = Convert.ToString(dt.Rows[0]["sLM_NUMBER"]);
                    objDel.PMCLecNAme = Convert.ToString(dt.Rows[0]["sLM_CONTRACTOR_NAME"]);
                }

                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        /// <summary>
        /// to save DI Details 
        /// </summary>
        /// <param name="objdelivery"></param>
        /// <returns></returns>
        public string[] SaveDeliveryDetails(clsPMCDelivery objdelivery)
        {
            PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
            NpgsqlCommand NpgsqlCommand = new NpgsqlCommand();
            string[] Arr = new string[3];
            try
            {
                string sQry = string.Empty;
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                dt = objdelivery.dtDelivery;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sDi_Id = Convert.ToString(dt.Rows[i]["DI_ID"]);
                    string sDiNo = Convert.ToString(dt.Rows[i]["DIM_DI_NO"]);
                    string sPoNo = Convert.ToString(dt.Rows[i]["DI_PO_ID"]);
                    string sStoreId = Convert.ToString(dt.Rows[i]["DI_STORE_ID"]);
                    string sDueDate = Convert.ToString(dt.Rows[i]["DI_DUEDATE"]);
                    
                    string sMake = Convert.ToString(dt.Rows[i]["DI_MAKE_ID"]);
                    string sCapacity = Convert.ToString(dt.Rows[i]["DI_CAPACITY"]);
                    string sCapacityID = Convert.ToString(dt.Rows[i]["DI_CAPACITY_ID"]);
                    string sStartype = Convert.ToString(dt.Rows[i]["DI_STARRATE"]);
                    string sQuantity = Convert.ToString(dt.Rows[i]["DI_QUANTITY"]);
                    string sConsignee = "0";
                    string sDiDate = Convert.ToString(dt.Rows[i]["DI_DATE"]);
                    string sFileExt = Convert.ToString(dt.Rows[i]["DI_FILE_EXT"]);
                    string sStartRange = Convert.ToString(dt.Rows[i]["DI_START_RANGE"]);
                    string sEndRange = Convert.ToString(dt.Rows[i]["DI_END_RANGE"]);
                    string sStartRangevar = Convert.ToString(dt.Rows[i]["PMC_DI_TC_START_RANGE_VAR"]);
                    string sEndRangevar = Convert.ToString(dt.Rows[i]["PMC_DI_TC_END_RANGE_VAR"]);
                    long Id = objcon.Get_max_no("DI_ID", "TBLDELIVERYINSTRUCTION");

                    if (sStartRangevar.Contains("HP"))
                    {
                        sStartRangevar = sStartRangevar.Replace("HP", "");
                    }

                    if (sEndRangevar.Contains("HP"))
                    {
                        sEndRangevar = sEndRangevar.Replace("HP", "");
                    }

                    if (sDi_Id == "")
                    {
                        NpgsqlCommand Npgsql = new NpgsqlCommand();


                        NpgsqlCommand cmd1 = new NpgsqlCommand("sp_check_di_number_exists");
                        cmd1.Parameters.AddWithValue("di_no", sDiNo.ToUpper());
                        cmd1.Parameters.AddWithValue("po_id", sPoNo.ToUpper());
                        dt1 = Objcon.FetchDataTable(cmd1);
                        if (dt1.Rows.Count > 0)
                        {
                            Arr[0] = "Entered DI Number Already mapped with some other PO Number";
                            Arr[1] = "2";
                            return Arr;
                        }
                        NpgsqlCommand cmd = new NpgsqlCommand("proc_saveupdate_pmc_dimaster_new");
                        cmd.Parameters.AddWithValue("di_id", sDi_Id);
                        cmd.Parameters.AddWithValue("di_number", sDiNo.ToUpper());
                        cmd.Parameters.AddWithValue("di_consignee", sConsignee);
                        cmd.Parameters.AddWithValue("di_po_id", sPoNo);
                        cmd.Parameters.AddWithValue("di_crby", objdelivery.Crby);
                        cmd.Parameters.AddWithValue("di_date", sDiDate);
                        cmd.Parameters.AddWithValue("di_store_id", sStoreId);
                        cmd.Parameters.AddWithValue("di_star_rate", sStartype);
                        cmd.Parameters.AddWithValue("di_duedate", sDueDate);
                        cmd.Parameters.AddWithValue("di_make_id", sMake);
                        cmd.Parameters.AddWithValue("di_quantity", sQuantity);
                        cmd.Parameters.AddWithValue("di_capacity", sCapacity);
                        cmd.Parameters.AddWithValue("di_capacity_id", sCapacityID);
                        cmd.Parameters.AddWithValue("di_start_range", sStartRange);
                        cmd.Parameters.AddWithValue("di_end_range", sEndRange);
                        cmd.Parameters.AddWithValue("di_start_range_var", sStartRangevar);
                        cmd.Parameters.AddWithValue("di_end_range_var", sEndRangevar);
                        cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                        cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                        cmd.Parameters.Add("pk_id", NpgsqlDbType.Text);
                        cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                        cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                        cmd.Parameters["pk_id"].Direction = ParameterDirection.Output;
                        Arr[0] = "msg";
                        Arr[1] = "op_id";
                        Arr[2] = "pk_id";
                        Arr = objcon.Execute(cmd, Arr, 3);
                        objdelivery.DIid = Arr[2].ToString();
                    }
                    else
                    {
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            sDi_Id = Convert.ToString(dt.Rows[j]["DI_ID"]);
                            sDiNo = Convert.ToString(dt.Rows[j]["DIM_DI_NO"]);
                            sConsignee = "0";
                            sPoNo = Convert.ToString(dt.Rows[j]["DI_PO_ID"]);
                            sStoreId = Convert.ToString(dt.Rows[j]["DI_STORE_ID"]);
                            sDueDate = Convert.ToString(dt.Rows[j]["DI_DUEDATE"]);
                            sMake = Convert.ToString(dt.Rows[j]["DI_MAKE_ID"]);
                            sCapacity = Convert.ToString(dt.Rows[j]["DI_CAPACITY"]);
                            sCapacityID = Convert.ToString(dt.Rows[j]["DI_CAPACITY_ID"]);
                            sStartype = Convert.ToString(dt.Rows[j]["DI_STARRATE"]);
                            sQuantity = Convert.ToString(dt.Rows[j]["DI_QUANTITY"]);
                            sDiDate = Convert.ToString(dt.Rows[j]["DI_DATE"]);
                            sFileExt = Convert.ToString(dt.Rows[j]["DI_FILE_EXT"]);
                            sStartRange = Convert.ToString(dt.Rows[i]["DI_START_RANGE"]);
                            sEndRange = Convert.ToString(dt.Rows[i]["DI_END_RANGE"]);

                            NpgsqlCommand cmd1 = new NpgsqlCommand("proc_saveupdate_pmc_dimaster_new");
                            cmd1.Parameters.AddWithValue("di_id", sDi_Id);
                            cmd1.Parameters.AddWithValue("di_number", sDiNo.ToUpper());
                            cmd1.Parameters.AddWithValue("di_consignee", sConsignee);
                            cmd1.Parameters.AddWithValue("di_po_id", sPoNo);
                            cmd1.Parameters.AddWithValue("di_crby", objdelivery.Crby);
                            cmd1.Parameters.AddWithValue("di_date", sDiDate);
                            cmd1.Parameters.AddWithValue("di_store_id", sStoreId);
                            cmd1.Parameters.AddWithValue("di_star_rate", sStartype);
                            cmd1.Parameters.AddWithValue("di_duedate", sDueDate);
                            cmd1.Parameters.AddWithValue("di_make_id", sMake);
                            cmd1.Parameters.AddWithValue("di_quantity", sQuantity);
                            cmd1.Parameters.AddWithValue("di_capacity", sCapacity);
                            cmd1.Parameters.AddWithValue("di_capacity_id", sCapacityID);
                            cmd1.Parameters.AddWithValue("di_status", "1");
                            cmd1.Parameters.AddWithValue("di_start_range", sStartRange);
                            cmd1.Parameters.AddWithValue("di_end_range", sEndRange);
                            cmd1.Parameters.Add("msg", NpgsqlDbType.Text);
                            cmd1.Parameters.Add("op_id", NpgsqlDbType.Text);
                            cmd1.Parameters.Add("pk_id", NpgsqlDbType.Text);
                            cmd1.Parameters["msg"].Direction = ParameterDirection.Output;
                            cmd1.Parameters["op_id"].Direction = ParameterDirection.Output;
                            cmd1.Parameters["pk_id"].Direction = ParameterDirection.Output;
                            Arr[0] = "msg";
                            Arr[1] = "op_id";
                            Arr[2] = "pk_id";
                            Arr = objcon.Execute(cmd1, Arr, 3);
                            objdelivery.DIid = Arr[2].ToString();

                            if (objdelivery.FileExt.Length > 0)
                            {
                                //NpgsqlParameter DeliveryNote = new NpgsqlParameter();
                                //NpgsqlCommand comd = new NpgsqlCommand();
                                //sQry = " UPDATE \"TBLPMC_DELIVERYINSTRUCTION\" SET \"PMC_DI_FILE_EXT\"='" + objdelivery.FileExt + "' ";
                                //sQry += " WHERE \"PMC_DI_ID\" = '" + objdelivery.DIid + "'";
                                //NpgsqlConnection objconn = new NpgsqlConnection();
                                //string strConnectionString = ConfigurationManager.ConnectionStrings["pgSQL"].ConnectionString + "Idea@12345+";
                                //// "Server=localhost;Port=5432;Database=TestTRM;User Id=postgres;Password =Idea123";  
                                //objconn.ConnectionString = strConnectionString; 
                                //objconn.Open();
                                //objcon.ExecuteQry(sQry, NpgsqlCommand);
                                //objconn.Close();

                                NpgsqlCommand cmd = new NpgsqlCommand("proc_update_pmc_di_file_path");
                                cmd.Parameters.AddWithValue("file_path", objdelivery.FileExt);
                                cmd.Parameters.AddWithValue("di_id", objdelivery.DIid);
                                objcon.Execute(cmd, Arr, 0);
                            }
                        }
                        Arr[0] = "Updated Successfully";
                        Arr[1] = "0";
                        return Arr;
                    }

                    if (objdelivery.FileExt.Length > 0)
                    {
                        //NpgsqlParameter DeliveryNote = new NpgsqlParameter();
                        //NpgsqlCommand comd = new NpgsqlCommand();
                        //sQry = " UPDATE \"TBLPMC_DELIVERYINSTRUCTION\" SET \"PMC_DI_FILE_EXT\"='" + objdelivery.FileExt + "' ";
                        //sQry += " WHERE \"PMC_DI_ID\" = '" + objdelivery.DIid + "'";
                        //NpgsqlConnection objconn = new NpgsqlConnection();
                        //string strConnectionString = ConfigurationManager.ConnectionStrings["pgSQL"].ConnectionString + "Idea@12345+";
                        //// "Server=localhost;Port=5432;Database=TestTRM;User Id=postgres;Password =Idea123";  
                        //objconn.ConnectionString = strConnectionString; 
                        //objconn.Open();
                        //objcon.ExecuteQry(sQry, NpgsqlCommand);
                        //string sQry1 = " UPDATE \"TBLPMC_DELIVERYINSTMASTER\" SET \"PMC_DIM_FILE_EXT\"='" + objdelivery.FileExt + "' ";
                        //sQry1 += "  WHERE \"PMC_DIM_PO_ID\" = '" + sPoNo + "' AND \"PMC_DIM_DI_NO\" = '" + sDiNo + "' ";
                        //objcon.ExecuteQry(sQry1, NpgsqlCommand);
                        //objconn.Close();

                        NpgsqlCommand cmd = new NpgsqlCommand("proc_update_pmc_di_file_path_withpo");
                        cmd.Parameters.AddWithValue("file_path", objdelivery.FileExt);
                        cmd.Parameters.AddWithValue("di_id", objdelivery.DIid);
                        cmd.Parameters.AddWithValue("di_no", sDiNo);
                        cmd.Parameters.AddWithValue("po_no", sPoNo);
                        objcon.Execute(cmd, Arr, 0);
                    }
                }
                Arr[0] = "Saved Successfully";
                Arr[1] = "0";
                return Arr;
            }
            catch (Exception ex)
            {
                Arr[0] = "Failed to Save";
                Arr[1] = "1";
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }
        #region Refrence
        ///// <summary>
        ///// to update due date
        ///// </summary>
        ///// <param name="sDueDate"></param>
        ///// <param name="sDiNo"></param>
        ///// <param name="sPoNo"></param>
        ///// <returns></returns>
        //public string[] UpdateDIDetails(string sDueDate, string sDiNo, int sPoNo)
        //{
        //    string[] Arr = new string[3];
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        // sp to check DI not has got allotment
        //        NpgsqlCommand cmd = new NpgsqlCommand("sp_get_allotment_id");
        //        cmd.Parameters.AddWithValue("DiNo", sDiNo);
        //        cmd.Parameters.Add("allotmentid", NpgsqlDbType.Text);
        //        cmd.Parameters["allotmentid"].Direction = ParameterDirection.Output;
        //        DataTable dt1 = Objcon.FetchDataTable(cmd);
        //        string saltId = dt1.Rows[0]["allotmentid"].ToString();
        //        if (saltId == "")
        //        {
        //            // sp to update the due date
        //            NpgsqlCommand cmd1 = new NpgsqlCommand("proc_update_di_details");
        //            cmd1.Parameters.AddWithValue("DueDate", sDueDate);
        //            cmd1.Parameters.AddWithValue("DiNo", sDiNo);
        //            cmd1.Parameters.AddWithValue("PoNo", sPoNo);
        //            cmd1.Parameters.Add("msg", NpgsqlDbType.Text);
        //            cmd1.Parameters.Add("op_id", NpgsqlDbType.Text);
        //            cmd1.Parameters["msg"].Direction = ParameterDirection.Output;
        //            cmd1.Parameters["op_id"].Direction = ParameterDirection.Output;
        //            Arr[0] = "msg";
        //            Arr[1] = "op_id";
        //            Arr = Objcon.Execute(cmd1, Arr, 2);
        //        }
        //        else
        //        {
        //            Arr[0] = "TC Allotment is Done for this DI";
        //            Arr[1] = "0";
        //        }
        //        return Arr;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return Arr;
        //    }
        //}
        #endregion
     
        /// <summary>
        /// to Get TC Range
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataTable GetTCRange(clsPMCDelivery obj)
        {
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_pmc_max_dtrrange");
                dt = Objcon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        /// <summary>
        /// to check duplicate DI no
        /// </summary>
        /// <param name="ponumber"></param>
        /// <param name="dinumber"></param>
        /// <returns></returns>
        public string[] CheckduplicateDI(string ponumber, string dinumber)
        {
            DataTable dt = new DataTable();
            string[] Arr = new string[3];
            string r = string.Empty;
            string str = string.Empty;
            try
            {
                NpgsqlCommand Npgsql = new NpgsqlCommand();
                NpgsqlCommand cmd = new NpgsqlCommand("sp_check_duplicate_di");
                cmd.Parameters.AddWithValue("di_no", dinumber);
                dt = Objcon.FetchDataTable(cmd);
                if (dt.Rows.Count > 0)
                {
                    Arr[0] = "Entered DI Number Already Exists";
                    Arr[1] = "2";
                    return Arr;
                }
                Arr[0] = "";
                Arr[1] = "0";
                return Arr;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }
        /// <summary>
        /// Get Start and End Range
        /// </summary>
        /// <param name="dtold"></param>
        /// <returns></returns>
        public DataTable ArrangedtRange(DataTable dtold)
        {
            DataTable dt = new DataTable();
            DataTable dtnew = new DataTable();
            int startrange = 0;
            int endrange = 0;
            try
            {
                dtnew.Columns.Add("DI_ID");
                dtnew.Columns.Add("DIM_DI_NO");
                dtnew.Columns.Add("DI_PO_ID");
                dtnew.Columns.Add("DI_STORE_ID");
                dtnew.Columns.Add("DI_DUEDATE");
                dtnew.Columns.Add("DI_MAKE_ID");
                dtnew.Columns.Add("DI_CAPACITY");
                dtnew.Columns.Add("DI_CAPACITY_ID");
                dtnew.Columns.Add("DI_STARRATE");
                dtnew.Columns.Add("DI_QUANTITY");
                dtnew.Columns.Add("DI_DATE");
                dtnew.Columns.Add("DI_FILE_EXT");
                dtnew.Columns.Add("DI_START_RANGE");
                dtnew.Columns.Add("DI_END_RANGE");
                dtnew.Columns.Add("PMC_DI_TC_START_RANGE_VAR");
                dtnew.Columns.Add("PMC_DI_TC_END_RANGE_VAR");
                dtnew.Columns.Add("DI_STORE");
                dtnew.Columns.Add("DI_MAKE");
                dtnew.Columns.Add("DI_STARRATENAME");

                for (int i = 0; i < dtold.Rows.Count; i++)
                {
                    string sDi_Id = Convert.ToString(dtold.Rows[i]["DI_ID"]);
                    string sDiNo = Convert.ToString(dtold.Rows[i]["DIM_DI_NO"]);
                    string sPoNo = Convert.ToString(dtold.Rows[i]["DI_PO_ID"]);
                    string sStoreId = Convert.ToString(dtold.Rows[i]["DI_STORE_ID"]);
                    string sDueDate = Convert.ToString(dtold.Rows[i]["DI_DUEDATE"]);
                    string sMake = Convert.ToString(dtold.Rows[i]["DI_MAKE_ID"]);
                    string sCapacity = Convert.ToString(dtold.Rows[i]["DI_CAPACITY"]);
                    string sCapacityID = Convert.ToString(dtold.Rows[i]["DI_CAPACITY_ID"]);
                    string sStartype = Convert.ToString(dtold.Rows[i]["DI_STARRATE"]);
                    string sQuantity = Convert.ToString(dtold.Rows[i]["DI_QUANTITY"]);
                    string sDiDate = Convert.ToString(dtold.Rows[i]["DI_DATE"]);
                    string sFileExt = Convert.ToString(dtold.Rows[i]["DI_FILE_EXT"]);
                    string sStore = Convert.ToString(dtold.Rows[i]["DI_STORE"]);
                    string sMakee = Convert.ToString(dtold.Rows[i]["DI_MAKE"]);
                    string sStartratename = Convert.ToString(dtold.Rows[i]["DI_STARRATENAME"]);
                    string sStartRange = Convert.ToString(dtold.Rows[i]["DI_START_RANGE"]);
                    string sEndRange = Convert.ToString(dtold.Rows[i]["DI_END_RANGE"]);
                    string sStartRangevar = Convert.ToString(dtold.Rows[i]["PMC_DI_TC_START_RANGE_VAR"]);
                    string sEndRangevar = Convert.ToString(dtold.Rows[i]["PMC_DI_TC_END_RANGE_VAR"]);

                    NpgsqlCommand cmd = new NpgsqlCommand("proc_pmc_max_dtrrange");
                    dt = Objcon.FetchDataTable(cmd);
                    if (dt.Rows.Count > 0)
                    {
                        if (i >= 1)
                        {
                            startrange = Convert.ToInt32(endrange) + 1;
                            endrange = startrange + Convert.ToInt32(sQuantity) - 1;

                            if ((Convert.ToString(startrange) ?? "").Length > 0 && (Convert.ToString(endrange) ?? "").Length > 0)
                            {

                                sStartRangevar = addprefix(Convert.ToString(startrange).Length, Convert.ToString(startrange));
                                sEndRangevar = addprefix(Convert.ToString(endrange).Length, Convert.ToString(endrange));

                            }
                        }
                        else
                        {
                            startrange = Convert.ToInt32(dt.Rows[0]["proc_pmc_max_dtrrange"]);
                            endrange = startrange + Convert.ToInt32(sQuantity) - 1;

                            if ((Convert.ToString(startrange) ?? "").Length > 0 && (Convert.ToString(endrange) ?? "").Length > 0)
                            {

                                sStartRangevar = addprefix(Convert.ToString(startrange).Length, Convert.ToString(startrange));
                                sEndRangevar = addprefix(Convert.ToString(endrange).Length, Convert.ToString(endrange));

                            }

                        }
                    }
                    DataRow dRow = dtnew.NewRow();
                    dRow["DI_ID"] = sDi_Id;
                    dRow["DIM_DI_NO"] = sDiNo;
                    dRow["DI_PO_ID"] = sPoNo;
                    dRow["DI_STORE_ID"] = sStoreId;
                    dRow["DI_DUEDATE"] = sDueDate;
                    dRow["DI_MAKE_ID"] = sMake;
                    dRow["DI_CAPACITY"] = sCapacity;
                    dRow["DI_CAPACITY_ID"] = sCapacityID;
                    dRow["DI_STARRATE"] = sStartype;
                    dRow["DI_QUANTITY"] = sQuantity;
                    dRow["DI_DATE"] = sDiDate;
                    dRow["DI_FILE_EXT"] = sFileExt;
                    dRow["DI_STORE"] = sStore;
                    dRow["DI_MAKE"] = sMakee;
                    dRow["DI_STARRATENAME"] = sStartratename;
                    dRow["DI_START_RANGE"] = startrange;
                    dRow["DI_END_RANGE"] = endrange;
                    if ((sStartRangevar ?? "").Length > 0 && (sEndRangevar ?? "").Length > 0)
                    {
                        dRow["PMC_DI_TC_START_RANGE_VAR"] = (sStartRangevar.Contains("HP") == true) ? sStartRangevar : "HP" + sStartRangevar;
                        dRow["PMC_DI_TC_END_RANGE_VAR"] = (sEndRangevar.Contains("HP") == true) ? sEndRangevar : "HP" + sEndRangevar;
                    }
                    dtnew.Rows.Add(dRow);
                    dtnew.AcceptChanges();
                }
                return dtnew;
            }
            catch (Exception ex)
            {
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                 MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        /// <summary>
        /// to add prefix to the Start range and End Range.
        /// </summary>
        /// <param name="length"></param>
        /// <param name="Maxrange"></param>
        /// <returns></returns>
        public string addprefix(int length, string Maxrange)
        {
            var prefix = string.Empty;
            for (int i = 6; i > length; i--)
            {
                prefix += "0";
            }
            var Result = prefix + Maxrange;
            return Result;
        }
    }
}
