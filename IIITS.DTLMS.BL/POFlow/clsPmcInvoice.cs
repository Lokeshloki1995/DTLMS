using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IIITS.DTLMS.BL.POFlow
{
    public class clsPmcInvoice
    {
        PGSqlConnection Objcon = new PGSqlConnection(Constants.Password);
        NpgsqlCommand NpgsqlCommand;
        public string PmcId { get; set; }
        public string fdId { get; set; }
        public string sCrBy { get; set; }
        public string sOfficeCode { get; set; }
        public string sFeederCode { get; set; }
    
        //TC Details
        public string sTcCode { get; set; }
        public string sStarRate { get; set; }
        public string sConnectionDate { get; set; }
        public string sArresters { get; set; }
        public string sDTCMeters { get; set; }
        public string sProjecttype { get; set; }
        public string sRemark { get; set; }
        public string sRoleID { get; set; }

        //Store
        public string sTCType { get; set; }
        public string PmcEnumerationDate { get; set; }
        public string PmcDTrCode { get; set; }
        public string PmcDTrMake { get; set; }
        public string PmcCapacity { get; set; }
        public string PmcSlno { get; set; }
        public string PmcLifespan { get; set; }
        public string PmcOilType { get; set; }
        public string PmcOilType_ID { get; set; }
        public string PmcTcWeight { get; set; }
        public string PmcManfdate { get; set; }
        public string PmcPurchasedate { get; set; }
        public string PmcTankCapacity { get; set; }
        public string PmcLocType { get; set; }
        public string PmcRating { get; set; }
        public string PmcNamePlatePhoto { get; set; }
        public string PmcDtrCodePhoto { get; set; }
        public string PmcInvoiceno { get; set; }
        public string PmcInvoiceDate { get; set; }
        public string PmcIndentno { get; set; }
        public string PmcIndentDate { get; set; }
        public string PmcPONo { get; set; }
        public string PmcPODate { get; set; }
        public string PmcComments { get; set; }
        public string PmcDTCCode { get; set; }
        public string PmcDTCCodeIp { get; set; }
        public string PmcTIMSCode { get; set; }
        public string PmcDTCName { get; set; }
        public string PmcDTCCodePhoto { get; set; }
        public string PmcDTCCodePhotoIP { get; set; }
        public string PmcTIMSCodePhoto { get; set; }
        public string PmcDTCPhoto { get; set; }
        public string PmcInternalCode { get; set; }
        public string PmcConnectedKW { get; set; }
        public string PmcConnectedHP { get; set; }
        public string PmcDTCCommisiondate { get; set; }
        public string PmcDTrCommisiondate { get; set; }
        public string PmcLastServiceDate { get; set; }
        public string PmcPlatformType { get; set; }
        public string PmcBreakerType { get; set; }
        public string PmcDTCMetersAvail { get; set; }
        public string PmcKWHReading { get; set; }
        public string PmcHTProtection { get; set; }
        public string PmcLTProtection { get; set; }
        public string PmcGrounding { get; set; }
        public string PmcLightningArrears { get; set; }
        public string PmcLoadType { get; set; }
        public string PmcProjType { get; set; }
        public string PmcLTLine { get; set; }
        public string PmcSupplierID { get; set; }
        public string PmcHTLine { get; set; }
        public string PmcDepreciation { get; set; }
        public string PmcLatitude { get; set; }
        public string PmcLongitude { get; set; }
        public string StoreId { get; set; }
        public string OilCapacity { get; set; }
        public string tcCore { get; set; }
        public string tcStatus { get; set; }

        // Workflow
        public string WFDataId { get; set; }
        public string InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string IndentOffCode { get; set; }
        public string sClientIP { get; set; }
        public string sFormName { get; set; }
        public string hdfWFOId { get; set; }
        public string IndentId { get; set; }
        public string TcWarrantyPeriod { get; set; }
        public string TcOilCap { get; set; }
        public string TcAmt { get; set; }
        public string ApproveStatus { get; set; }

        public DataTable DataSource { get; set; } = new DataTable();
        public DataTable DtPmcDetails { get; set; } = new DataTable("");
        DataTable Dt { get; set; } = new DataTable();


        public string getIndentID(string InvoiceId)
        {
            return Objcon.get_value("SELECT \"PMC_PI_ID\" from \"TBLPMC_INVOICE\" WHERE \"PMC_ID\" = " + InvoiceId);
        }

        /// <summary>
        /// Get Invoice Details
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public object GetInvoiceDetails(clsPmcInvoice Obj)
        {
            DataTable Dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("public.proc_savepmcinvoice");
                cmd.Parameters.AddWithValue("p_pi_id", Obj.IndentId);
                Dt = Objcon.FetchDataTable(cmd);
                if (Dt.Rows.Count > 0)
                {
                    Obj.PmcEnumerationDate = Dt.Rows[0]["PI_ENUMERATION_DATE"].ToString();
                    Obj.PmcDTrCode = Dt.Rows[0]["PI_TC_CODE"].ToString();
                    Obj.PmcDTrMake = Dt.Rows[0]["PI_MAKE"].ToString();
                    Obj.PmcCapacity = Dt.Rows[0]["PI_CAPACITY"].ToString();
                    Obj.PmcSlno = Dt.Rows[0]["PI_TC_SLNO"].ToString();
                    Obj.PmcLifespan = Dt.Rows[0]["PI_LIFESPAN"].ToString();
                    Obj.PmcOilType = Dt.Rows[0]["PI_OIL_TYPE"].ToString();
                    Obj.PmcOilType_ID = Dt.Rows[0]["PI_OIL_TYPE_ID"].ToString();
                    Obj.PmcTcWeight = Dt.Rows[0]["PI_TC_WEIGHT"].ToString();
                    Obj.PmcManfdate = Dt.Rows[0]["PI_TC_MANFDATE"].ToString();
                    Obj.PmcTankCapacity = Dt.Rows[0]["PI_TANK_CAPACITY"].ToString();
                    Obj.PmcLocType = Dt.Rows[0]["PI_LOCTYPE"].ToString();
                    Obj.PmcRating = Dt.Rows[0]["PI_RATING"].ToString();
                    Obj.PmcIndentno = Dt.Rows[0]["TPIE_INDENT_NO"].ToString();
                    Obj.PmcIndentDate = Dt.Rows[0]["TPIE_INDENT_DATE"].ToString();
                    Obj.PmcPONo = Dt.Rows[0]["TPIE_PO_NO"].ToString();
                    Obj.PmcPODate = Dt.Rows[0]["PMC_PO_DATE"].ToString();
                    Obj.PmcDTCCode = Dt.Rows[0]["PI_DTCCODE"].ToString();
                    Obj.PmcDTCCodeIp = Dt.Rows[0]["PI_IPENUM"].ToString();
                    Obj.PmcTIMSCode = Dt.Rows[0]["PI_TIMS_CODE"].ToString();
                    Obj.PmcDTCName = Dt.Rows[0]["PI_DTC_NAME"].ToString();
                    Obj.PmcNamePlatePhoto = Dt.Rows[0]["PIP_NAME_PLATE"].ToString();
                    Obj.PmcDtrCodePhoto = Dt.Rows[0]["PIP_DTR_CODE_PHOTO"].ToString();
                    Obj.PmcDTCCodePhoto = Dt.Rows[0]["PIP_DTC_CODE_PHOTO"].ToString();
                    Obj.PmcDTCCodePhotoIP = Dt.Rows[0]["PIP_DTC_IPENUM"].ToString();
                    Obj.PmcTIMSCodePhoto = Dt.Rows[0]["PIP_TIMS_CODE"].ToString();
                    Obj.PmcDTCPhoto = Dt.Rows[0]["PIP_DTC_CODE_PHOTO"].ToString();
                    Obj.PmcInternalCode = Dt.Rows[0]["PI_INTERNAL_CODE"].ToString();
                    Obj.PmcConnectedKW = Dt.Rows[0]["PI_TOTAL_CON_KW"].ToString();
                    Obj.PmcConnectedHP = Dt.Rows[0]["PI_TOTAL_CON_HP"].ToString();
                    Obj.PmcDTCCommisiondate = Dt.Rows[0]["PI_TRANS_COMMISION_DATE"].ToString();
                    Obj.PmcDTrCommisiondate = Dt.Rows[0]["PI_DTR_COMMISION_DATE"].ToString();
                    Obj.PmcLastServiceDate = Dt.Rows[0]["PI_LAST_SERVICE_DATE"].ToString();
                    Obj.PmcPlatformType = Dt.Rows[0]["PI_PLATFORM"].ToString();
                    Obj.PmcBreakerType = Dt.Rows[0]["PI_BREAKER_TYPE"].ToString();
                    Obj.PmcDTCMetersAvail = Dt.Rows[0]["PI_DTCMETERS"].ToString();
                    Obj.PmcKWHReading = Dt.Rows[0]["PI_KWH_READING"].ToString();
                    Obj.PmcHTProtection = Dt.Rows[0]["PI_HT_PROTECT"].ToString();
                    Obj.PmcLTProtection = Dt.Rows[0]["PI_LT_PROTECT"].ToString();
                    Obj.PmcGrounding = Dt.Rows[0]["PI_GROUNDING"].ToString();
                    Obj.PmcLightningArrears = Dt.Rows[0]["PI_ARRESTERS"].ToString();
                    Obj.PmcLoadType = Dt.Rows[0]["PI_LOADTYPE"].ToString();
                    Obj.PmcProjType = Dt.Rows[0]["PI_PROJECTTYPE"].ToString();
                    Obj.PmcLTLine = Dt.Rows[0]["PI_LT_LINE"].ToString();
                    Obj.PmcHTLine = Dt.Rows[0]["PI_HT_LINE"].ToString();
                    Obj.PmcDepreciation = Dt.Rows[0]["PI_DEPRECIATION"].ToString();
                    Obj.PmcLatitude = Dt.Rows[0]["PI_LATITUDE"].ToString();
                    Obj.PmcLongitude = Dt.Rows[0]["PI_LONGITUDE"].ToString();
                    Obj.sOfficeCode = Dt.Rows[0]["TPIE_OFFICECODE"].ToString();
                    Obj.sFeederCode = Dt.Rows[0]["PI_FEEDERCODE"].ToString();
                    Obj.fdId = Dt.Rows[0]["PI_ID"].ToString();
                    Obj.TcWarrantyPeriod = Dt.Rows[0]["pi_warranty_period"].ToString();
                    Obj.TcOilCap = Dt.Rows[0]["pi_tc_oil_capacity"].ToString();
                    Obj.TcAmt = Dt.Rows[0]["pi_dtr_amount"].ToString();
                    
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
                throw ex;
            }
            return Obj;
        }

        /// <summary>
        /// method to get invoicecompleted details
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>

        public object GetInvoiceCompletedDetails(clsPmcInvoice Obj)
        {
            DataTable Dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("public.sp_get_pmc_invoice_completed_records");
                cmd.Parameters.AddWithValue("inv_id", Obj.PmcId);
                Dt = Objcon.FetchDataTable(cmd);
                if (Dt.Rows.Count > 0)
                {
                    Obj.PmcInvoiceno = Dt.Rows[0]["sPMC_INVOICE_NO"].ToString();
                    Obj.PmcInvoiceDate = Dt.Rows[0]["sPMC_INVOICE_DATE"].ToString();
                    Obj.PmcComments = Dt.Rows[0]["sPMC_COMMENTS"].ToString();
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
                throw ex;
            }
            return Obj;
        }


   /// <summary>
   /// save pmc details
   /// </summary>
   /// <param name="objInvoice"></param>
   /// <returns></returns>
        public string[] SavePMCInvoiceDetails(clsPmcInvoice objInvoice)
        {
            DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);

            string[] Arr = new string[3];
            try
            {
                string strQry = string.Empty;
                string strQry0 = string.Empty;
                DataTable dt = new DataTable();

                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Parameters.AddWithValue("invoiceno", objInvoice.InvoiceNo);
                string sResult = Objcon.get_value("SELECT \"PMC_INVOICE_NO\" FROM \"TBLPMC_INVOICE\" WHERE \"PMC_INVOICE_NO\" =:invoiceno", NpgsqlCommand);
                if (sResult.Length != 0)
                {
                    Arr[0] = "Entered Invoice No Already Exist";
                    Arr[1] = "2";
                    return Arr;
                }

                NpgsqlCommand cmd = new NpgsqlCommand("proc_save_pmcinvoice");
                cmd.Parameters.AddWithValue("pmc_pi_id", objInvoice.fdId);
                cmd.Parameters.AddWithValue("pmc_invoice_no", objInvoice.InvoiceNo);
                cmd.Parameters.AddWithValue("pmc_invoice_date", objInvoice.InvoiceDate);
                cmd.Parameters.AddWithValue("pmc_comments", objInvoice.sRemark);
                cmd.Parameters.AddWithValue("pmc_crby", objInvoice.sCrBy);

                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd.Parameters.Add("pk_id", NpgsqlDbType.Text);
                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                cmd.Parameters["pk_id"].Direction = ParameterDirection.Output;
                Arr[0] = "op_id";
                Arr[1] = "msg";
                Arr[2] = "pk_id";
                Arr = Objcon.Execute(cmd, Arr, 3);

                if (Arr[0] != "0")
                {
                    return Arr;
                }

                //Workflow / Approval
                #region Workflow

                strQry = "INSERT INTO \"TBLTCMASTER\" (\"TC_ID\",\"TC_CODE\",\"TC_SLNO\",\"TC_MAKE_ID\",\"TC_CAPACITY\",\"TC_MANF_DATE\",";
                strQry += " \"TC_PURCHASE_DATE\",\"TC_LIFE_SPAN\",\"TC_SUPPLIER_ID\",\"TC_PO_NO\",\"TC_PRICE\",\"TC_WARANTY_PERIOD\",\"TC_LAST_SERVICE_DATE\", ";
                strQry += " \"TC_CURRENT_LOCATION\",\"TC_CRBY\",\"TC_WARRENTY\",\"TC_STORE_ID\",\"TC_LOCATION_ID\",\"TC_RATING\",\"TC_STAR_RATE\",";
                strQry += " \"TC_OIL_CAPACITY\",\"TC_WEIGHT\",\"TC_CONDITION\",\"TC_COOLING\",\"TC_CORE\",\"TC_TYPE\",\"TC_TAP_CHARGER\",\"TC_ASSET_ID\",";
                strQry += " \"TC_COMPONENT_ID\",\"TC_ORIGINAL_COST\",\"TC_INSURANCE\",\"TC_DEPRECIATION\",\"TC_OIL_TYPE\",\"TC_STATUS\",\"TC_LOCATION\",\"TC_PMC_INVOICE_ID\") values(";
                strQry += " (SELECT MAX(\"TC_ID\")+1 FROM \"TBLTCMASTER\"),'" + objInvoice.sTcCode + "','" + objInvoice.PmcSlno + "',";
                strQry += " '" + objInvoice.PmcDTrMake + "','" + objInvoice.PmcCapacity + "',TO_DATE('" + objInvoice.PmcManfdate + "','dd/MM/yyyy'),";
                strQry += " TO_DATE('" + objInvoice.PmcPurchasedate + "','dd/MM/yyyy'),";
                strQry += " '" + objInvoice.PmcLifespan + "','" + objInvoice.PmcSupplierID + "','" + objInvoice.PmcPONo + "','" + objInvoice.TcAmt + "',null,null,'2','" + objInvoice.sCrBy + "','" + objInvoice.TcWarrantyPeriod + "',";
                strQry += " '" + objInvoice.StoreId + "','" + objInvoice.IndentOffCode + "','" + objInvoice.PmcRating + "','" + objInvoice.sStarRate + "',";
                strQry += " '" + objInvoice.OilCapacity + "','" + objInvoice.PmcTcWeight + "','0','0','0','0', ";
                strQry += " '0','0','0','0','0','"+objInvoice.PmcDepreciation+"',Cast('" + objInvoice.PmcOilType + "' as int8),'" + objInvoice.tcStatus + "','" + objInvoice.IndentOffCode + "'," + Convert.ToInt32(Arr[2]) + ")";

                strQry = strQry.Replace("'", "''");

                strQry0 = "INSERT INTO \"TBLDTCMAST\"(\"DT_ID\",\"DT_CODE\",\"DT_NAME\",\"DT_OM_SLNO\",\"DT_TOTAL_CON_KW\",\"DT_TOTAL_CON_HP\",\"DT_KWH_READING\" ";
                strQry0 += " ,\"DT_INTERNAL_CODE\",\"DT_TC_ID\",\"DT_CON_DATE\",\"DT_LAST_SERVICE_DATE\",\"DT_TRANS_COMMISION_DATE\",\"DT_FDRCHANGE_DATE\", ";
                strQry0 += " \"DT_FDRSLNO\",\"DT_CRBY\",\"DT_WO_ID\",\"DT_PROJECTTYPE\",\"DT_TIMS_CODE\",\"DT_MLA_CONST\",\"DT_MP_CONST\",\"DT_ELE_DATE\", ";
                strQry0 += " \"DT_ELE_RATENO\",\"DT_PMC_INVOICE_ID\",\"DT_LATITUDE\",\"DT_LONGITUDE\",\"DT_LOCATION\",\"DT_HT_LINE\",\"DT_LT_LINE\", ";
                strQry0 += " \"DT_HT_PROTECT\",\"DT_LT_PROTECT\",\"DT_GROUNDING\",\"DT_LOADTYPE\",\"DT_BREAKER_TYPE\",\"DT_DTCMETERS\",\"DT_ARRESTERS\",\"DT_PLATFORM\") VALUES (";
                strQry0 += " (SELECT MAX(\"DT_ID\")+1 FROM \"TBLDTCMAST\"),'" + objInvoice.PmcDTCCode + "','" + objInvoice.PmcDTCName + "',";
                strQry0 += " '" + objInvoice.IndentOffCode + "','" + objInvoice.PmcConnectedKW + "','" + objInvoice.PmcConnectedHP + "',";
                strQry0 += "'" + objInvoice.PmcKWHReading + "','"+objInvoice.PmcInternalCode+"','" + objInvoice.sTcCode + "',null";
                strQry0 += " ,TO_DATE('" + objInvoice.PmcLastServiceDate + "','dd/MM/yyyy'),TO_DATE('" + objInvoice.PmcDTCCommisiondate + "','dd/MM/yyyy'),null,";
                strQry0 += " '" + objInvoice.sFeederCode + "','" + objInvoice.sCrBy + "','0','" + objInvoice.sProjecttype + "', ";
                strQry0 += "  '" + objInvoice.PmcTIMSCode + "','0','0',null,'0'," + Convert.ToInt32(Arr[2]) + ",'" + objInvoice.PmcLatitude + "', ";
                strQry0 += "  '" + objInvoice.PmcLongitude + "','" + objInvoice.PmcLocType + "','" + objInvoice.PmcHTLine + "','" + objInvoice.PmcLTLine + "', ";
                strQry0 += " '" + objInvoice.PmcHTProtection + "','" + objInvoice.PmcLTProtection + "','" + objInvoice.PmcGrounding + "','" + objInvoice.PmcLoadType + "', ";
                strQry0 += "'" + objInvoice.PmcBreakerType + "','" + objInvoice.sDTCMeters + "','" + objInvoice.sArresters + "','"+objInvoice.PmcPlatformType+"')";
                
                strQry0 = strQry0.Replace("'", "''");

                string strQry1 = " UPDATE \"TBLPMC_INVOICE\" SET \"PMC_STATUS\"=1 ,\"PMC_APPON\" =now(),\"PMC_APPBY\" ='" + objInvoice.sCrBy + "'";
                strQry1 += "WHERE \"PMC_INVOICE_NO\" ='" + objInvoice.InvoiceNo + "' and \"PMC_PI_ID\" ='" + objInvoice.fdId + "'";

                strQry1 = strQry1.Replace("'", "''");

                string strQry2 = "INSERT INTO \"TBLTRANSDTCMAPPING\" (\"TM_ID\",\"TM_MAPPING_DATE\",\"TM_TC_ID\",";
                strQry2 += "\"TM_DTC_ID\",\"TM_LIVE_FLAG\",\"TM_CRBY\",";
                strQry2 += "\"TM_CRON\") VALUES ((SELECT COALESCE(MAX(\"TM_ID\"),0)+1 FROM \"TBLTRANSDTCMAPPING\"),";
                strQry2 += "TO_DATE('" + objInvoice.PmcDTrCommisiondate + "','dd/MM/yyyy'),'" + objInvoice.sTcCode + "','" + objInvoice.PmcDTCCode + "','1',";
                strQry2 += "'" + objInvoice.sCrBy + "',now())";

                strQry2 = strQry2.Replace("'", "''");

               int altID= Convert.ToInt32( GetAllotmentID(objInvoice.sTcCode));

                string strQry5 = " UPDATE \"TBLPMC_DTR_RANGE_ALLOCATION\" SET \"PDRA_INVOICE_STATUS\"=1 WHERE \"PDRA_ID\" ='" + altID + "' ";

                strQry5 = strQry5.Replace("'", "''");
               
                string sParam = " ";

                clsApproval objApproval = new clsApproval();



                objApproval.sFormName = objInvoice.sFormName;
                objApproval.sOfficeCode = objInvoice.sOfficeCode;
                objApproval.sClientIp = objInvoice.sClientIP;
                objApproval.sCrby = objInvoice.sCrBy;
                objApproval.sQryValues = strQry + ";" + strQry0 + ";" + strQry1 + ";" + strQry2 + ";" + strQry5;
                objApproval.sParameterValues = sParam;
                objApproval.sMainTable = "TBLPMC_INVOICE";
                objApproval.sDataReferenceId = objInvoice.sTcCode;
                objApproval.sbfm_id = "41";
                objApproval.sBOId = "1020";
                objApproval.sDescription = "PMC Invoice for Invoice No " + objInvoice.InvoiceNo + " with DTC Code " + objInvoice.PmcDTCCode + " and TC Code " + objInvoice.sTcCode;
                objApproval.sRefOfficeCode = objInvoice.IndentOffCode;
                objApproval.sRoleId = objInvoice.sRoleID;
                string sPrimaryKey = "{0}";

                objApproval.sColumnNames = "PMC_ID,PMC_PI_ID,PMC_INVOICE_NO,PMC_INVOICE_DATE,PMC_REMARKS,PMC_STATUS,PMC_CRON,PMC_CRBY,DTC_CODE,TC_CODE";
                objApproval.sColumnValues = "" + sPrimaryKey + "," + objInvoice.fdId + "," + objInvoice.InvoiceNo + ", " + objInvoice.InvoiceDate + ",";
                objApproval.sColumnValues += "" + objInvoice.sRemark + ",0,NOW()," + objInvoice.sCrBy + "," + objInvoice.PmcDTCCode + "," + objInvoice.sTcCode + "";

                objApproval.sTableNames = "TBLPMC_INVOICE";




                //Check for Duplicate Approval
                bool bApproveResult = objApproval.CheckDuplicateApprove(objApproval);
                if (bApproveResult == false)
                {
                    Arr[0] = "Selected Record Already Approved";
                    Arr[1] = "2";
                    return Arr;
                }


                objDatabse.BeginTransaction();
                objApproval.SaveWorkFlowData_Latest(objApproval, objDatabse);
                objApproval.sRecordId = Convert.ToString(Arr[2]); // primary key of invoice Master
                objApproval.sWFObjectId = objInvoice.hdfWFOId; // assigning the value from the qrey string.

                objApproval.SaveWorkflowObjects_Latest(objApproval, objDatabse);
                objInvoice.WFDataId = objApproval.sWFDataId;
                objInvoice.InvoiceId = objApproval.sNewRecordId;
                objDatabse.CommitTransaction();

                #endregion

                Arr[0] = "Saved Successfully.";
                Arr[1] = "0";
                return Arr;

            }

            catch (Exception ex)
            {
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                      MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }
        
        /// <summary>
        /// get grid details
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public DataTable GetPmcGridDetails(clsPmcInvoice Obj)
        {
            DataTable DtPmcDetails = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_pmcinvoiceview");
                cmd.Parameters.AddWithValue("dtc_code", Obj.PmcDTCCode == null ? "" : Obj.PmcDTCCode);
                cmd.Parameters.AddWithValue("tc_code", Obj.PmcDTrCode == null ? "" : Obj.PmcDTrCode);
                cmd.Parameters.AddWithValue("indent_no", Obj.PmcIndentno == null ? "" : Obj.PmcIndentno);
                cmd.Parameters.AddWithValue("po_no", Obj.PmcPONo == null ? "" : Obj.PmcPONo);
                cmd.Parameters.AddWithValue("inv_no", Obj.PmcInvoiceno == null ? "" : Obj.PmcInvoiceno);
                cmd.Parameters.AddWithValue("capacity", Obj.PmcInvoiceno == null ? "" : Obj.PmcCapacity);
                DtPmcDetails = Objcon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);

            }
            return DtPmcDetails;
        }
        #region WorkFlow XML
        /// <summary>
        /// Get Failure Details From XML
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public clsPmcInvoice GetFailureDetailsFromXML(clsPmcInvoice Obj)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable DtXMLDetails = new DataTable();

                DtXMLDetails = objApproval.GetDatatableFromXML(Obj.WFDataId);

                if (DtXMLDetails.Rows.Count > 0)
                {

                }
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
            return Obj;
        }
        #endregion

        /// <summary>
        /// method to get invoicedetails
        /// </summary>
        /// <param name="Inv_Id"></param>
        /// <returns></returns>
        public DataTable getinvoiceDetails(string Inv_Id)
        {
            string StrQry = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand = new NpgsqlCommand();
                StrQry = "SELECT \"PMC_INVOICE_NO\",TO_CHAR(\"PMC_INVOICE_DATE\",'dd/MM/yyyy') \"PMC_INVOICE_DATE\",\"PMC_REMARKS\" FROM \"TBLPMC_INVOICE\" where \"PMC_ID\"=:Inv_id";
                NpgsqlCommand.Parameters.AddWithValue("Inv_id", Convert.ToInt32(Inv_Id));

                dt = Objcon.FetchDataTable(StrQry, NpgsqlCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                  MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace); return dt;
            }
        }
        /// <summary>
        /// method to get allotment id
        /// </summary>
        /// <param name="TcCode"></param>
        /// <returns></returns>
        public string GetAllotmentID(string TcCode)
        {
            PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
            string AltId = string.Empty;
            try
            {
           
            string sQry = "select \"PDRA_ID\" from \"TBLPMC_DTR_RANGE_ALLOCATION\" where \"PDRA_TC_CODE\"='" + TcCode + "'";

            AltId = objcon.get_value(sQry);
                return AltId;
            }
            catch (Exception ex)
            {
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                  MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return AltId;
            }
        }


        public void Updatestatusafterreject(clsPmcInvoice Objstatus)
        {
            try
            {
                //string strQry1 = " UPDATE \"TBLPMC_INVOICE\" SET \"PMC_STATUS\"='"+ Objstatus.ApproveStatus + "' ,\"PMC_APPON\" =now(),\"PMC_APPBY\" ='" + Objstatus.sCrBy + "'";
                //strQry1 += "WHERE \"PMC_INVOICE_NO\" ='" + Objstatus.InvoiceNo + "' and \"PMC_PI_ID\" ='" + Objstatus.fdId + "'";
                //Objcon.ExecuteQry(strQry1);
                int altID = Convert.ToInt32(GetAllotmentID(Objstatus.sTcCode));
                //string strQry5 = " UPDATE \"TBLPMC_DTR_RANGE_ALLOCATION\" SET \"PDRA_INVOICE_STATUS\"='"+ Objstatus.ApproveStatus + "' WHERE \"PDRA_ID\" ='" + altID + "' ";
                //Objcon.ExecuteQry(strQry5);
                string[] strArray = new string[3];
                NpgsqlCommand cmd = new NpgsqlCommand("sp_update_pmcinvoice_reject");
                cmd.Parameters.AddWithValue("approvestatus", Convert.ToInt32(Objstatus.ApproveStatus));
                cmd.Parameters.AddWithValue("invoiceno", Convert.ToString(Objstatus.InvoiceNo));
                cmd.Parameters.AddWithValue("fdid", Convert.ToString(Objstatus.fdId));
                cmd.Parameters.AddWithValue("altid", Convert.ToString(altID));
                Objcon.Execute(cmd, strArray, 0);
            }
            catch(Exception ex)
            {
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                  MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}
