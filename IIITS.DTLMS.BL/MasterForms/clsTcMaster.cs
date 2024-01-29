using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data;
using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;
using IIITS.DTLMS.BL.DataBase;

namespace IIITS.DTLMS.BL
{
    public class clsTcMaster
    {
        string strFormCode = "clsTcMaster";
        string sWarranty = string.Empty;

        PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
        DataBseConnection ObjBseCon = new DataBseConnection(Constants.Password);
        NpgsqlCommand NpgsqlCommand;

        public string sTimeId { get; set; }
        public DataTable dtTable { get; set; }
        public string sTcId { get; set; }
        public string sTcMakeId { get; set; }
        public string sTcSlNo { get; set; }
        public string sTcCapacity { get; set; }
        public string sTcLifeSpan { get; set; }
        public string sManufacDate { get; set; }
        public string sAllotementDate { get; set; }
        public string sAltNo { get; set; }
        public string sDINo { get; set; }
        public string sPoNo { get; set; }
        public string sPrice { get; set; }
        public string sSupplierId { get; set; }
        public string sWarrentyPeriod { get; set; }
        public string sLastServiceDate { get; set; }
        public string sQuantity { get; set; }
        public string sAltId { get; set; }
        public string sTcCode { get; set; }
        public string sTcLiveFlag { get; set; }
        public int sStatus { get; set; }
        public string sCurrentLocation { get; set; }
        public string sDivId { get; set; }
        public string sLocationId { get; set; }
        public string sLastRepairerId { get; set; }
        public string sUpdatedEvent { get; set; }
        public string sUpdateEventId { get; set; }
        public string sCrBy { get; set; }
        public string sOfficeCode { get; set; }
        public string sStoreId { get; set; }
        public string sRating { get; set; }
        public string sStarRate { get; set; }
        public string sOilCapacity { get; set; }
        public string sOilType { get; set; }
        public string sWeight { get; set; }
        public string srepairOffCode { get; set; }
        public string sDtcCodes { get; set; }
        public string sPurchaseno { get; set; }
        public string sDino { get; set; }
        public string sColumnNames { get; set; }
        public string sColumnValues { get; set; }
        public string sTableNames { get; set; }
        public string sQryValues { get; set; }
        public string sDescription { get; set; }
        public string sParameterValues { get; set; }
        public string sWFDataId { get; set; }
        public string sXmlData { get; set; }
        public string sBOId { get; set; }
        public string sFormName { get; set; }
        public string sClientIP { get; set; }
        public string sLastFaildate { get; set; }
        public string sLastFailuretype { get; set; }
        public string sLastRepaircost { get; set; }
        public string sLastRepaircount { get; set; }
        public string sConditiontc { get; set; }
        public string sCooling { get; set; }
        public string sType { get; set; }
        public string sCore { get; set; }
        public string sTapeCharger { get; set; }
        public string sDepreciation { get; set; }
        public string sInsurance { get; set; }
        public string sOriginalCost { get; set; }
        public string sFailCount { get; set; }
        public string sComponentId { get; set; }
        public string sInfosysId { get; set; }
        public string sDTrImagePath { get; set; }
        public string sroletype { get; set; }
        public string sNamePlateImagePath { get; set; }
        public string sDTRcommissionYear { get; set; }
        public string sDTRcommissionDate { get; set; }
        public string locationtype { get; set; }
        public string sFeederCode { get; set; }
        public string sOldDTCCode { get; set; }

        /// <summary>
        /// Save/Update Transformer Details
        /// </summary>
        /// <param name="objTcMaster"></param>
        /// <returns></returns>
        public string[] SaveUpdateTransformerDetails(clsTcMaster objTcMaster)
        {
            string QryKey = string.Empty;
            string[] Arr = new string[2];
            string strQry = string.Empty;
            string Res = string.Empty;

            try
            {
                if (objTcMaster.sAllotementDate != null && objTcMaster.sAllotementDate != "")
                {
                    DateTime dPurchaseDate = DateTime.ParseExact(objTcMaster.sAllotementDate, "dd/MM/yyyy", null);

                    if (objTcMaster.sWarrentyPeriod != "")
                    {
                        sWarranty = Convert.ToString(dPurchaseDate.AddYears(Convert.ToInt32(objTcMaster.sWarrentyPeriod)));
                        sWarranty = Convert.ToDateTime(sWarranty).ToString("dd/MM/yyyy");
                    }
                }

                //Get Store Id
                if (objTcMaster.sLocationId != "" && objTcMaster.sLocationId != null)
                    if (objTcMaster.sroletype == "2")
                    {
                        sStoreId = objTcMaster.sLocationId;
                    }
                    else
                    {
                        sStoreId = GetStoreId(objTcMaster.sLocationId);
                        if (sStoreId == "")
                        {
                            sStoreId = "0";
                        }
                    }
                else
                    sStoreId = "0";
                #region OLD inline queary
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("TcMakeid", objTcMaster.sTcMakeId);
                //NpgsqlCommand.Parameters.AddWithValue("TcSlno", objTcMaster.sTcSlNo);
                //NpgsqlCommand.Parameters.AddWithValue("Dtrcode", objTcMaster.sTcCode);
                //Res = ObjCon.get_value("select \"TC_SLNO\" from \"TBLTCMASTER\" where cast(\"TC_MAKE_ID\" as text)=:TcMakeid and \"TC_SLNO\"=:TcSlno and \"TC_CODE\"<>:Dtrcode", NpgsqlCommand);
                #endregion

                QryKey = "GET_TC_SLNO";
                NpgsqlCommand cmd_TC_SLNO = new NpgsqlCommand("fetch_getvalue_clstcmaster");
                cmd_TC_SLNO.Parameters.AddWithValue("p_key", QryKey);
                cmd_TC_SLNO.Parameters.AddWithValue("p_value_1", Convert.ToString(objTcMaster.sTcMakeId ?? ""));
                cmd_TC_SLNO.Parameters.AddWithValue("p_value_2", Convert.ToString(objTcMaster.sTcSlNo ?? ""));
                cmd_TC_SLNO.Parameters.AddWithValue("p_value_3", Convert.ToString(objTcMaster.sTcCode ?? ""));
                Res = ObjBseCon.StringGetValue(cmd_TC_SLNO);


                if (Res != "")
                {

                    Arr[0] = "Combination of DTr Make and DTr SlNo Already Exist.";
                    Arr[1] = "2";
                    return Arr;
                }
                int NoofTimes = 0;
                LOOP:
                NpgsqlCommand cmd = new NpgsqlCommand("proc_saveupdatetransfdetailsnew");
                cmd.Parameters.AddWithValue("tc_id", objTcMaster.sTcId);
                cmd.Parameters.AddWithValue("tc_code", objTcMaster.sTcCode);
                cmd.Parameters.AddWithValue("tc_serialno", objTcMaster.sTcSlNo);
                cmd.Parameters.AddWithValue("tc_makeid", objTcMaster.sTcMakeId == "" ? "0" : objTcMaster.sTcMakeId);
                cmd.Parameters.AddWithValue("tc_capacity", objTcMaster.sTcCapacity == "" ? "0" : objTcMaster.sTcCapacity);
                cmd.Parameters.AddWithValue("tc_manf_date", objTcMaster.sManufacDate);
                cmd.Parameters.AddWithValue("tc_purchase_date", objTcMaster.sAllotementDate == null ? "" : objTcMaster.sAllotementDate);
                cmd.Parameters.AddWithValue("tc_supplier_id", objTcMaster.sSupplierId == null ? "0" : objTcMaster.sSupplierId);
                cmd.Parameters.AddWithValue("tc_po_no", objTcMaster.sPoNo);
                cmd.Parameters.AddWithValue("tc_price", objTcMaster.sPrice == "" ? "0" : objTcMaster.sPrice);
                cmd.Parameters.AddWithValue("tc_warrantyperiod", objTcMaster.sWarranty == "" ? "0" : objTcMaster.sWarranty);
                cmd.Parameters.AddWithValue("tc_life_span", objTcMaster.sTcLifeSpan == "" ? "0" : objTcMaster.sTcLifeSpan);
                cmd.Parameters.AddWithValue("tc_last_servicedate", objTcMaster.sLastServiceDate);
                cmd.Parameters.AddWithValue("tc_curr_loc", objTcMaster.sCurrentLocation);
                cmd.Parameters.AddWithValue("tc_crby", objTcMaster.sCrBy);
                cmd.Parameters.AddWithValue("tc_warranty", objTcMaster.sWarrentyPeriod == "" ? "0" : objTcMaster.sWarrentyPeriod);
                cmd.Parameters.AddWithValue("tc_store_id", objTcMaster.sStoreId == "" ? "0" : objTcMaster.sStoreId);
                cmd.Parameters.AddWithValue("tc_loc_id", objTcMaster.sOfficeCode == null ? "0" : objTcMaster.sOfficeCode);
                cmd.Parameters.AddWithValue("tc_rating", objTcMaster.sRating == "" ? "0" : objTcMaster.sRating);
                cmd.Parameters.AddWithValue("tc_star_rate", objTcMaster.sStarRate == "" ? "0" : objTcMaster.sStarRate);
                cmd.Parameters.AddWithValue("tc_oil_capacity", objTcMaster.sOilCapacity == "" ? "0" : objTcMaster.sOilCapacity);
                cmd.Parameters.AddWithValue("tc_weight", objTcMaster.sWeight == null ? "0" : objTcMaster.sWeight);
                cmd.Parameters.AddWithValue("tc_condition", objTcMaster.sConditiontc == null ? "0" : objTcMaster.sConditiontc);
                cmd.Parameters.AddWithValue("tc_cooling", objTcMaster.sCooling == null ? "0" : objTcMaster.sCooling);
                cmd.Parameters.AddWithValue("tc_core", objTcMaster.sCore == null ? "0" : objTcMaster.sCore);
                cmd.Parameters.AddWithValue("tc_type", objTcMaster.sType == null ? "0" : objTcMaster.sType);
                cmd.Parameters.AddWithValue("tc_tap_charger", objTcMaster.sTapeCharger == null ? "0" : objTcMaster.sTapeCharger);
                cmd.Parameters.AddWithValue("tc_depreciation", objTcMaster.sDepreciation == null ? "0" : objTcMaster.sDepreciation);
                cmd.Parameters.AddWithValue("tc_insurance", objTcMaster.sInsurance == null ? "0" : objTcMaster.sInsurance);
                cmd.Parameters.AddWithValue("tc_original_cost", objTcMaster.sOriginalCost == null ? "0" : objTcMaster.sOriginalCost);
                cmd.Parameters.AddWithValue("tc_component_id", objTcMaster.sComponentId == null ? "0" : objTcMaster.sComponentId);
                cmd.Parameters.AddWithValue("tc_asset_id", objTcMaster.sInfosysId == null ? "0" : objTcMaster.sInfosysId);
                cmd.Parameters.AddWithValue("tc_oil_type", objTcMaster.sOilType == null ? "0" : objTcMaster.sOilType);
                cmd.Parameters.AddWithValue("tc_status", objTcMaster.sType == "0" ? "1" : objTcMaster.sType);
                cmd.Parameters.AddWithValue("tc_location", objTcMaster.locationtype == "" ? "0" : objTcMaster.locationtype);
                cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                Arr[0] = "msg";
                Arr[1] = "op_id";
                try
                {
                    Arr = ObjCon.Execute(cmd, Arr, 2);
                }
                catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(100);
                    NoofTimes++;
                    if (NoofTimes < 3)
                    {
                        goto LOOP;
                    }
                    else
                    {
                        throw ex;
                    }
                }

                if (Arr[1] == "0" || Arr[1] == "1")
                {
                    #region old inline queary
                    //strQry = "UPDATE \"TBLDTRALLOCATION\" SET \"DA_STATUS\" = 1 WHERE \"DA_DTR_CODE\" = '" + objTcMaster.sTcCode + "' ";
                    //ObjCon.ExecuteQry(strQry);
                    #endregion

                    string[] Arr_DTRALLOCATION = new string[2];
                    NpgsqlCommand cmd_DA_STATUS = new NpgsqlCommand("proc_update_da_status_to_tbldtrallocation_for_clstcmaster");
                    cmd_DA_STATUS.Parameters.AddWithValue("p_da_dtr_code", Convert.ToString(objTcMaster.sTcCode ?? ""));
                    cmd_DA_STATUS.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd_DA_STATUS.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd_DA_STATUS.Parameters["msg"].Direction = ParameterDirection.Output;
                    cmd_DA_STATUS.Parameters["op_id"].Direction = ParameterDirection.Output;
                    Arr_DTRALLOCATION[0] = "msg";
                    Arr_DTRALLOCATION[1] = "op_id";
                    Arr_DTRALLOCATION = ObjCon.Execute(cmd_DA_STATUS, Arr_DTRALLOCATION, 2);
                }

                if (Arr[1] == "3")
                {
                    return Arr;
                }

                #region old inline queary
                ////update for purchase date
                //if (objTcMaster.sAllotementDate != null && objTcMaster.sAllotementDate != "")
                //{
                //    strQry = "UPDATE \"TBLTCMASTER\" SET \"TC_PURCHASE_DATE\"=TO_DATE('" + objTcMaster.sAllotementDate + "','dd/MM/yyyy') WHERE \"TC_CODE\"='" + objTcMaster.sTcCode + "' ";
                //    ObjCon.ExecuteQry(strQry);
                //}
                //else
                //{
                //    strQry = "UPDATE \"TBLTCMASTER\" SET \"TC_PURCHASE_DATE\"=null WHERE \"TC_CODE\"='" + objTcMaster.sTcCode + "' ";
                //    ObjCon.ExecuteQry(strQry);
                //}

                ////update for warrenty period
                //if (objTcMaster.sWarrentyPeriod != null && objTcMaster.sWarrentyPeriod != "")
                //{
                //    //strQry = "UPDATE \"TBLTCMASTER\" SET \"TC_WARANTY_PERIOD\"=TO_DATE('" + objTcMaster.sWarrentyPeriod + "','dd/MM/yyyy') WHERE \"TC_CODE\"='" + objTcMaster.sTcCode + "' ";
                //    //strQry = "UPDATE \"TBLTCMASTER\" SET \"TC_WARANTY_PERIOD\"='" + objTcMaster.sWarrentyPeriod + "' WHERE \"TC_CODE\"='" + objTcMaster.sTcCode + "' ";

                //    //ObjCon.ExecuteQry(strQry);
                //}
                //else
                //{
                //    strQry = "UPDATE \"TBLTCMASTER\" SET \"TC_WARANTY_PERIOD\"=null WHERE \"TC_CODE\"='" + objTcMaster.sTcCode + "' ";
                //    ObjCon.ExecuteQry(strQry);
                //}
                ////update for last service date
                //if (objTcMaster.sLastServiceDate != null && objTcMaster.sLastServiceDate != "")
                //{
                //    strQry = "UPDATE \"TBLTCMASTER\" SET \"TC_LAST_SERVICE_DATE\"=TO_DATE('" + objTcMaster.sLastServiceDate + "','dd/MM/yyyy') WHERE \"TC_CODE\"='" + objTcMaster.sTcCode + "' ";
                //    ObjCon.ExecuteQry(strQry);
                //}
                //else
                //{
                //    strQry = "UPDATE \"TBLTCMASTER\" SET \"TC_LAST_SERVICE_DATE\"=null WHERE \"TC_CODE\"='" + objTcMaster.sTcCode + "' ";
                //    ObjCon.ExecuteQry(strQry);
                //}
                #endregion

                string[] Arr_tbltcmaster = new string[2];
                NpgsqlCommand cmd_tbltcmaster = new NpgsqlCommand("proc_update_tbltcmaster_for_clstcmaster");
                cmd_tbltcmaster.Parameters.AddWithValue("p_tc_code", Convert.ToString(objTcMaster.sTcCode ?? ""));
                cmd_tbltcmaster.Parameters.AddWithValue("p_purchase_date", Convert.ToString(objTcMaster.sAllotementDate ?? ""));
                cmd_tbltcmaster.Parameters.AddWithValue("p_waranty_period", Convert.ToString(objTcMaster.sWarrentyPeriod ?? ""));
                cmd_tbltcmaster.Parameters.AddWithValue("p_last_service_date", Convert.ToString(objTcMaster.sLastServiceDate ?? ""));
                cmd_tbltcmaster.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd_tbltcmaster.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd_tbltcmaster.Parameters["msg"].Direction = ParameterDirection.Output;
                cmd_tbltcmaster.Parameters["op_id"].Direction = ParameterDirection.Output;
                Arr_tbltcmaster[0] = "msg";
                Arr_tbltcmaster[1] = "op_id";
                Arr_tbltcmaster = ObjCon.Execute(cmd_tbltcmaster, Arr_tbltcmaster, 2);


                return Arr;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }
        /// <summary>
        /// Get Tc And Srl Num Details
        /// </summary>
        /// <param name="objDtrCComm"></param>
        /// <returns></returns>
        public string[] GetTcAndSrlNumDetails(clsTcMaster objDtrCComm)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string[] Arr = new string[3];
            string strQry = string.Empty;
            string sQryVal = string.Empty;
            string QryKey = string.Empty;
            try
            {
                Arr[1] = "0";
                #region old inline query
                //NpgsqlCommand.Parameters.AddWithValue("tccd", objDtrCComm.sTcCode.ToUpper());
                //sQryVal = ObjCon.get_value("select \"TC_ID\" from \"TBLTCMASTER\" where cast(\"TC_CODE\" as text)=:tccd", NpgsqlCommand);
                #endregion

                QryKey = "GET_TC_ID";
                NpgsqlCommand cmd_TC_ID = new NpgsqlCommand("fetch_getvalue_clstcmaster");
                cmd_TC_ID.Parameters.AddWithValue("p_key", QryKey);
                cmd_TC_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(objDtrCComm.sTcCode ?? "").ToUpper());
                cmd_TC_ID.Parameters.AddWithValue("p_value_2", "");
                cmd_TC_ID.Parameters.AddWithValue("p_value_3", "");
                sQryVal = ObjBseCon.StringGetValue(cmd_TC_ID);


                if (sQryVal != "")
                {
                    Arr[2] = objDtrCComm.sTcCode;
                    Arr[1] = "2";
                    Arr[0] = "Transformer Code " + objDtrCComm.sTcCode + "  Already Exist";
                    return Arr;
                }

                #region old inline query
                //NpgsqlCommand.Parameters.AddWithValue("slno1", objDtrCComm.sTcSlNo.ToUpper());
                //sQryVal = ObjCon.get_value("select \"TC_ID\" from \"TBLTCMASTER\" where cast(\"TC_SLNO\" as text)=:slno1", NpgsqlCommand);
                #endregion

                QryKey = "GET_TC_ID_ON_TC_SLNO";
                NpgsqlCommand cmd_TC_ID_ON_TC_SLNO = new NpgsqlCommand("fetch_getvalue_clstcmaster");
                cmd_TC_ID_ON_TC_SLNO.Parameters.AddWithValue("p_key", QryKey);
                cmd_TC_ID_ON_TC_SLNO.Parameters.AddWithValue("p_value_1", Convert.ToString(objDtrCComm.sTcSlNo ?? "").ToUpper());
                cmd_TC_ID_ON_TC_SLNO.Parameters.AddWithValue("p_value_2", "");
                cmd_TC_ID_ON_TC_SLNO.Parameters.AddWithValue("p_value_3", "");
                sQryVal = ObjBseCon.StringGetValue(cmd_TC_ID_ON_TC_SLNO);

                if (sQryVal != "")
                {
                    Arr[2] = objDtrCComm.sTcSlNo;
                    Arr[1] = "2";
                    Arr[0] = "Transformer SlNo  " + objDtrCComm.sTcSlNo + "  Already Exist";
                    return Arr;

                }
                return Arr;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }

        }
        /// <summary>
        /// Save TC Details
        /// </summary>
        /// <param name="sTcDetails"></param>
        /// <param name="objTcMaster"></param>
        /// <returns></returns>
        public string[] SaveTCDetails(string[] sTcDetails, clsTcMaster objTcMaster)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string[] Arr = new string[3];
            string strQry = string.Empty;
            bool bResult = false;
            string sQryVal = string.Empty;
            string QryKey = string.Empty;
            try
            {
                #region old inline query
                //NpgsqlCommand.Parameters.AddWithValue("supId", objTcMaster.sSupplierId);
                //sQryVal = ObjCon.get_value("select \"TS_ID\" from \"TBLTRANSSUPPLIER\"  WHERE \"TS_STATUS\"='A' AND  CAST(\"TS_ID\" as TEXT)=:supId", NpgsqlCommand);
                #endregion

                QryKey = "GET_TS_ID";
                NpgsqlCommand cmd_TS_ID = new NpgsqlCommand("fetch_getvalue_clstcmaster");
                cmd_TS_ID.Parameters.AddWithValue("p_key", QryKey);
                cmd_TS_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(objTcMaster.sSupplierId ?? ""));
                cmd_TS_ID.Parameters.AddWithValue("p_value_2", "");
                cmd_TS_ID.Parameters.AddWithValue("p_value_3", "");
                sQryVal = ObjBseCon.StringGetValue(cmd_TS_ID);

                if (sQryVal == "" || sQryVal == null)
                {
                    Arr[0] = objTcMaster.sSupplierId;
                    Arr[1] = "2";
                    Arr[2] = "Enter a Valid Supplier ID";
                    return Arr;
                }
                if (objTcMaster.sroletype == "2")
                {
                    sStoreId = objTcMaster.sOfficeCode;
                }
                else
                {
                    sStoreId = GetStoreId(objTcMaster.sOfficeCode);
                }
                string[] strDetailVal = sTcDetails.ToArray();
                for (int i = 0; i < strDetailVal.Length; i++)
                {
                    #region old inline query
                    //NpgsqlCommand.Parameters.AddWithValue("tccd", strDetailVal[i].Split('~').GetValue(0).ToString());
                    //sQryVal = ObjCon.get_value("select \"TC_ID\" from \"TBLTCMASTER\" where cast(\"TC_CODE\" as text)=:tccd", NpgsqlCommand);
                    #endregion

                    QryKey = "GET_TC_ID";
                    NpgsqlCommand cmd_TC_ID = new NpgsqlCommand("fetch_getvalue_clstcmaster");
                    cmd_TC_ID.Parameters.AddWithValue("p_key", QryKey);
                    cmd_TC_ID.Parameters.AddWithValue("p_value_1", strDetailVal[i].Split('~').GetValue(0).ToString());
                    cmd_TC_ID.Parameters.AddWithValue("p_value_2", "");
                    cmd_TC_ID.Parameters.AddWithValue("p_value_3", "");
                    sQryVal = ObjBseCon.StringGetValue(cmd_TC_ID);

                    if (sQryVal != "")
                    {
                        Arr[2] = strDetailVal[i].Split('~').GetValue(0).ToString();
                        Arr[1] = "2";
                        Arr[0] = "Transformer Code " + strDetailVal[i].Split('~').GetValue(0).ToString() + "  Already Exist";
                        return Arr;
                    }

                    #region old inline query
                    //NpgsqlCommand.Parameters.AddWithValue("slno1", strDetailVal[i].Split('~').GetValue(1).ToString());
                    //sQryVal = ObjCon.get_value("select \"TC_ID\" from \"TBLTCMASTER\" where cast(\"TC_SLNO\" as text)=:slno1", NpgsqlCommand);
                    #endregion

                    QryKey = "GET_TC_ID_ON_TC_SLNO";
                    NpgsqlCommand cmd_TC_ID_ON_TC_SLNO = new NpgsqlCommand("fetch_getvalue_clstcmaster");
                    cmd_TC_ID_ON_TC_SLNO.Parameters.AddWithValue("p_key", QryKey);
                    cmd_TC_ID_ON_TC_SLNO.Parameters.AddWithValue("p_value_1", strDetailVal[i].Split('~').GetValue(1).ToString());
                    cmd_TC_ID_ON_TC_SLNO.Parameters.AddWithValue("p_value_2", "");
                    cmd_TC_ID_ON_TC_SLNO.Parameters.AddWithValue("p_value_3", "");
                    sQryVal = ObjBseCon.StringGetValue(cmd_TC_ID_ON_TC_SLNO);

                    if (sQryVal != "")
                    {
                        Arr[2] = strDetailVal[i].Split('~').GetValue(1).ToString();
                        Arr[1] = "2";
                        Arr[0] = "Transformer SlNo  " + strDetailVal[i].Split('~').GetValue(1).ToString() + "  Already Exist";
                        return Arr;
                    }
                }

                NpgsqlCommand cmd = new NpgsqlCommand("sp_savetcreceipt");
                //Insert to TBLTCRECIEPT Table
                string sRecieptID;
                cmd.Parameters.AddWithValue("tcr_id", "");
                cmd.Parameters.AddWithValue("tcr_po_no", objTcMaster.sPoNo);
                cmd.Parameters.AddWithValue("tcr_purchase_date", objTcMaster.sAllotementDate);
                cmd.Parameters.AddWithValue("tcr_qnty", objTcMaster.sQuantity);
                cmd.Parameters.AddWithValue("tcr_supplyid", objTcMaster.sSupplierId);
                cmd.Parameters.AddWithValue("tcr_crby", objTcMaster.sCrBy);
                cmd.Parameters.Add("pk_id", NpgsqlDbType.Text);
                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd.Parameters["pk_id"].Direction = ParameterDirection.Output;
                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                cmd.Parameters["msg"].Direction = ParameterDirection.Output;

                Arr[0] = "msg";
                Arr[1] = "op_id";
                Arr[2] = "pk_id";
                Arr = ObjCon.Execute(cmd, Arr, 3);
                sRecieptID = Arr[2].ToString();

                DateTime dPurchaseDate = DateTime.ParseExact(objTcMaster.sAllotementDate, "dd/MM/yyyy", null);

                for (int i = 0; i < strDetailVal.Length; i++)
                {
                    #region old inline query
                    //NpgsqlCommand.Parameters.AddWithValue("tcode", strDetailVal[i].Split('~').GetValue(0).ToString());
                    //sQryVal = ObjCon.get_value("select \"TC_ID\" from \"TBLTCMASTER\" where  cast(\"TC_CODE\" as text)='" + strDetailVal[i].Split('~').GetValue(0).ToString() + "'", NpgsqlCommand);
                    #endregion

                    QryKey = "GET_TC_ID";
                    NpgsqlCommand cmd_TC_ID = new NpgsqlCommand("fetch_getvalue_clstcmaster");
                    cmd_TC_ID.Parameters.AddWithValue("p_key", QryKey);
                    cmd_TC_ID.Parameters.AddWithValue("p_value_1", strDetailVal[i].Split('~').GetValue(0).ToString());
                    cmd_TC_ID.Parameters.AddWithValue("p_value_2", "");
                    cmd_TC_ID.Parameters.AddWithValue("p_value_3", "");
                    sQryVal = ObjBseCon.StringGetValue(cmd_TC_ID);

                    if (sQryVal != "")
                    {
                        Arr[2] = strDetailVal[i].Split('~').GetValue(0).ToString();
                        Arr[1] = "2";
                        Arr[0] = "Transformer Code " + strDetailVal[i].Split('~').GetValue(0).ToString() + "  Already Exist";
                        return Arr;
                    }

                    #region old inline query
                    //NpgsqlCommand.Parameters.AddWithValue("slNo", strDetailVal[i].Split('~').GetValue(1).ToString());
                    //sQryVal = ObjCon.get_value("select \"TC_ID\" from \"TBLTCMASTER\" where cast(\"TC_SLNO\" as text)='" + strDetailVal[i].Split('~').GetValue(1).ToString() + "'", NpgsqlCommand);
                    #endregion

                    QryKey = "GET_TC_ID_ON_TC_SLNO";
                    NpgsqlCommand cmd_TC_ID_ON_TC_SLNO = new NpgsqlCommand("fetch_getvalue_clstcmaster");
                    cmd_TC_ID_ON_TC_SLNO.Parameters.AddWithValue("p_key", QryKey);
                    cmd_TC_ID_ON_TC_SLNO.Parameters.AddWithValue("p_value_1", strDetailVal[i].Split('~').GetValue(1).ToString());
                    cmd_TC_ID_ON_TC_SLNO.Parameters.AddWithValue("p_value_2", "");
                    cmd_TC_ID_ON_TC_SLNO.Parameters.AddWithValue("p_value_3", "");
                    sQryVal = ObjBseCon.StringGetValue(cmd_TC_ID_ON_TC_SLNO);

                    if (sQryVal != "")
                    {
                        Arr[2] = strDetailVal[i].Split('~').GetValue(1).ToString();
                        Arr[1] = "2";
                        Arr[0] = "Transformer SlNo  " + strDetailVal[i].Split('~').GetValue(1).ToString() + "  Already Exist";
                        return Arr;
                    }

                    objTcMaster.sTcId = ObjCon.Get_max_no("TC_ID", "TBLTCMASTER").ToString();
                    string sWarranty = Convert.ToString(dPurchaseDate.AddMonths(Convert.ToInt32(strDetailVal[i].Split('~').GetValue(6).ToString())));
                    sWarranty = Convert.ToDateTime(sWarranty).ToString("dd/MM/yyyy");

                    //GENERATED ALWAYS AS (ADD_MONTHS(ENTERDATE,IDS))
                    NpgsqlCommand cmd1 = new NpgsqlCommand("sp_savetotcmaster");
                    cmd1.Parameters.AddWithValue("tc_id", "");
                    cmd1.Parameters.AddWithValue("inl_id", "");
                    cmd1.Parameters.AddWithValue("tc_code", strDetailVal[i].Split('~').GetValue(0).ToString());
                    cmd1.Parameters.AddWithValue("tc_serialno", strDetailVal[i].Split('~').GetValue(1).ToString());
                    cmd1.Parameters.AddWithValue("tc_makeid", strDetailVal[i].Split('~').GetValue(2).ToString());
                    cmd1.Parameters.AddWithValue("tc_capacity", strDetailVal[i].Split('~').GetValue(3).ToString());
                    cmd1.Parameters.AddWithValue("tc_manf_date", strDetailVal[i].Split('~').GetValue(4).ToString());
                    cmd1.Parameters.AddWithValue("tc_purchase_date", objTcMaster.sAllotementDate);
                    cmd1.Parameters.AddWithValue("tc_life_span", strDetailVal[i].Split('~').GetValue(5).ToString());
                    cmd1.Parameters.AddWithValue("tc_supplier_id", objTcMaster.sSupplierId);
                    cmd1.Parameters.AddWithValue("tc_po_no", objTcMaster.sPoNo);
                    cmd1.Parameters.AddWithValue("tc_warrantyperiod", sWarranty);
                    cmd1.Parameters.AddWithValue("tc_warranty", strDetailVal[i].Split('~').GetValue(6).ToString());
                    cmd1.Parameters.AddWithValue("tc_curr_loc", "1");
                    cmd1.Parameters.AddWithValue("tc_crby", objTcMaster.sCrBy);
                    cmd1.Parameters.AddWithValue("tc_store_id", sStoreId);
                    cmd1.Parameters.AddWithValue("tc_loc_id", sStoreId);
                    cmd1.Parameters.AddWithValue("tc_tcr_id", sRecieptID);
                    cmd1.Parameters.AddWithValue("tc_oil_capacity", strDetailVal[i].Split('~').GetValue(7).ToString());
                    cmd1.Parameters.AddWithValue("tc_weight", strDetailVal[i].Split('~').GetValue(8).ToString());
                    cmd1.Parameters.AddWithValue("tc_star_rate", strDetailVal[i].Split('~').GetValue(9).ToString());
                    cmd1.Parameters.AddWithValue("tc_di_no", objTcMaster.sDINo);
                    cmd1.Parameters.AddWithValue("tc_alt_no", "");
                    cmd1.Parameters.AddWithValue("tc_div_id", "");
                    cmd1.Parameters.AddWithValue("record_by", "Web");
                    cmd1.Parameters.AddWithValue("device_id", objTcMaster.sClientIP);
                    cmd1.Parameters.AddWithValue("tc_oil_type", strDetailVal[i].Split('~').GetValue(10).ToString());
                    cmd1.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd1.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd1.Parameters["msg"].Direction = ParameterDirection.Output;
                    cmd1.Parameters["op_id"].Direction = ParameterDirection.Output;
                    Arr[0] = "msg";
                    Arr[1] = "op_id";
                    Arr[2] = "msg";
                    Arr = ObjCon.Execute(cmd1, Arr, 3);
                    bResult = true;

                    #region old inline query
                    //string str = "UPDATE \"TBLTCRANGEALLOTMENT\" SET \"TCP_STATUS_M\"=1 WHERE \"TCP_TC_CODE\"='" + strDetailVal[i].Split('~').GetValue(0).ToString() + "'";
                    //ObjCon.ExecuteQry(str, NpgsqlCommand);
                    #endregion

                    string[] Arr_TBLTCRANGEALLOTMENT = new string[2];
                    NpgsqlCommand cmd_TBLTCRANGEALLOTMENT = new NpgsqlCommand("proc_update_tcp_status_m_to_tbltcrangeallotment_for_clstcmaster");
                    cmd_TBLTCRANGEALLOTMENT.Parameters.AddWithValue("p_tcp_tc_code", strDetailVal[i].Split('~').GetValue(0).ToString());
                    cmd_TBLTCRANGEALLOTMENT.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd_TBLTCRANGEALLOTMENT.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd_TBLTCRANGEALLOTMENT.Parameters["msg"].Direction = ParameterDirection.Output;
                    cmd_TBLTCRANGEALLOTMENT.Parameters["op_id"].Direction = ParameterDirection.Output;
                    Arr_TBLTCRANGEALLOTMENT[0] = "msg";
                    Arr_TBLTCRANGEALLOTMENT[1] = "op_id";
                    ObjCon.Execute(cmd_TBLTCRANGEALLOTMENT, Arr_TBLTCRANGEALLOTMENT, 2);
                }

                if (bResult == true)
                {
                    Arr[0] = "Transformers Details Saved Successfully";
                    Arr[1] = "0";
                }
                else
                {
                    Arr[0] = "No Transformer Exists to Save";
                    Arr[1] = "2";
                }
                return Arr;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }
        /// <summary>
        /// Load Tc Master
        /// </summary>
        /// <param name="objTc"></param>
        /// <returns></returns>
        public DataTable LoadTcMaster(clsTcMaster objTc)
        {
            DataTable dtTcDetails = new DataTable();
            try
            {
                #region Old LoadTcMaster
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "SELECT (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\") AS \"TC_MAKE_ID\",\"TC_ID\",\"TC_CODE\" AS \"TC_CODE\",";
                //strQry += "\"TC_SLNO\",\"TC_MAKE_ID\",\"TC_CAPACITY\" AS \"TC_CAPACITY\", \"TC_LIFE_SPAN\", \"TC_ALT_NO\" FROM \"TBLTCMASTER\" ";
                //strQry += "WHERE ";

                //if (objTc.sroletype == "2")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("offcd", objTc.sOfficeCode);
                //    NpgsqlCommand.Parameters.AddWithValue("offcd01", objTc.sOfficeCode);
                //    strQry += "cast(\"TC_STORE_ID\"as TEXT)   = :offcd and cast(\"TC_LOCATION_ID\" as text) = :offcd01  ";
                //}
                //else
                //{
                //    if (objTc.sCurrentLocation == "1" || objTc.sCurrentLocation == "3" || objTc.sCurrentLocation == "5")
                //    {
                //        NpgsqlCommand.Parameters.AddWithValue("curloc1", Convert.ToDouble(objTc.sCurrentLocation));
                //        strQry += "\"TC_CURRENT_LOCATION\"='" + Convert.ToDouble(objTc.sCurrentLocation) + "'";
                //        if (objTc.sOfficeCode.Length > 3)
                //        {
                //            sStoreId = Convert.ToString(GetStoreId(objTc.sOfficeCode.Substring(0, 3)));
                //            NpgsqlCommand.Parameters.AddWithValue("offcd1", sStoreId);
                //            strQry += " AND \"TC_STORE_ID\"='" + sStoreId + "'";
                //        }
                //        if (sStoreId != null && sStoreId != "")
                //        {
                //            NpgsqlCommand.Parameters.AddWithValue("locid", Convert.ToDouble(sStoreId));
                //            strQry += " AND cast(\"TC_LOCATION_ID\" as TEXT)  LIKE :locid||'%'";
                //        }
                //        else if (objTc.sOfficeCode != "" && objTc.sOfficeCode != null)
                //        {

                //            if (objTc.sLocationId != null && objTc.sLocationId != "")
                //            {
                //                NpgsqlCommand.Parameters.AddWithValue("locid", Convert.ToDouble(objTc.sLocationId));
                //                strQry += " AND cast(\"TC_LOCATION_ID\" as TEXT)  LIKE :locid||'%'";
                //            }
                //            else
                //            {
                //                NpgsqlCommand.Parameters.AddWithValue("locid", Convert.ToDouble(objTc.sOfficeCode));
                //                strQry += " AND cast(\"TC_LOCATION_ID\" as TEXT)  LIKE :locid||'%'";
                //            }

                //        }
                //        else
                //        {
                //            if (objTc.sLocationId != null && objTc.sLocationId != "")
                //            {
                //                NpgsqlCommand.Parameters.AddWithValue("locid", Convert.ToDouble(objTc.sLocationId));
                //                strQry += " AND cast(\"TC_LOCATION_ID\" as TEXT)  LIKE :locid||'%'";
                //            }
                //        }
                //    }
                //    else
                //    {
                //        if (objTc.sOfficeCode == "" || objTc.sOfficeCode == null)
                //        {
                //            strQry += " cast(\"TC_LOCATION_ID\" as TEXT) like'%'";
                //        }
                //        else
                //        {
                //            NpgsqlCommand.Parameters.AddWithValue("offcode1", objTc.sOfficeCode);
                //            strQry += "cast(\"TC_LOCATION_ID\" as TEXT) LIKE :offcode1||'%'";
                //        }
                //    }
                //}
                //if (objTc.sTcMakeId != null && objTc.sTcMakeId != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("tcmakeid", Convert.ToInt32(objTc.sTcMakeId.ToUpper()));
                //    strQry += " AND \"TC_MAKE_ID\"=(SELECT \"TM_ID\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=:tcmakeid)";

                //}
                //if (objTc.sTcCapacity != null && objTc.sTcCapacity != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("tccap", Convert.ToDouble(objTc.sTcCapacity));
                //    strQry += " AND \"TC_CAPACITY\"=:tccap";
                //}
                //if (objTc.sCurrentLocation == "2")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("curloc", Convert.ToDouble(objTc.sCurrentLocation));
                //    strQry += " AND \"TC_CURRENT_LOCATION\"=:curloc";
                //}
                //if (objTc.sTcCode != null && objTc.sTcCode != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("tcd", Convert.ToString(objTc.sTcCode));
                //    strQry += "AND \"TC_CODE\" =:tcd ";
                //}
                //if (objTc.sTcSlNo != null && objTc.sTcSlNo != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("sTcSlNo", objTc.sTcSlNo.ToUpper());
                //    strQry += "AND \"TC_SLNO\" =:sTcSlNo ";
                //}
                //strQry += " and \"TC_CAPACITY\"<>0 ";

                //if (objTc.sOfficeCode != null && objTc.sOfficeCode != "")
                //{
                //    strQry += " ORDER BY \"TC_ID\" DESC";
                //}
                //else
                //{
                //    strQry += " ORDER BY \"TC_ID\" DESC limit 300";
                //}
                //dtTcDetails = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_tcmaster");
                cmd.Parameters.AddWithValue("p_roletype", (objTc.sroletype ?? ""));
                cmd.Parameters.AddWithValue("p_offcd", (objTc.sOfficeCode ?? ""));
                cmd.Parameters.AddWithValue("p_curloc", (objTc.sCurrentLocation ?? ""));
                cmd.Parameters.AddWithValue("p_locid", (objTc.sLocationId ?? ""));
                cmd.Parameters.AddWithValue("p_tcmakeid", (objTc.sTcMakeId ?? ""));
                cmd.Parameters.AddWithValue("p_tccap", (objTc.sTcCapacity ?? ""));
                cmd.Parameters.AddWithValue("p_tcd", (objTc.sTcCode ?? ""));
                cmd.Parameters.AddWithValue("p_tcslno", (objTc.sTcSlNo ?? "").ToUpper());
                dtTcDetails = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtTcDetails;
            }
            return dtTcDetails;
        }
        /// <summary>
        /// Load Tc Master Search
        /// </summary>
        /// <param name="objTc"></param>
        /// <returns></returns>
        public DataTable LoadTcMasterSearch(clsTcMaster objTc)
        {
            DataTable dtTcDetails = new DataTable();
            try
            {
                #region Old LoadTcMasterSearch for only Croprate level User.
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = "SELECT \"TM_NAME\"  AS \"TC_MAKE_ID\",\"TC_ID\",\"TC_CODE\" AS \"TC_CODE\",";
                //strQry += "\"TC_SLNO\",\"TC_MAKE_ID\",\"TC_CAPACITY\" AS \"TC_CAPACITY\", \"TC_LIFE_SPAN\", \"TC_ALT_NO\" FROM \"TBLTCMASTER\" inner join \"TBLTRANSMAKES\" on  \"TC_MAKE_ID\"= \"TM_ID\" ";
                //strQry += "WHERE ";

                //if (objTc.sroletype == "2")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("offcd", objTc.sOfficeCode);
                //    strQry += "cast(\"TC_STORE_ID\"as TEXT)   = :offcd";
                //}
                //else
                //{
                //    if (objTc.sLocationId != null && objTc.sLocationId != "")
                //    {
                //        NpgsqlCommand.Parameters.AddWithValue("locid", Convert.ToDouble(objTc.sLocationId));
                //        strQry += "\"TC_LOCATION_ID\"  = :locid";
                //    }
                //    else
                //    {
                //        if (objTc.sCurrentLocation == "1" || objTc.sCurrentLocation == "3" || objTc.sCurrentLocation == "5")
                //        {
                //            NpgsqlCommand.Parameters.AddWithValue("curloc1", Convert.ToDouble(objTc.sCurrentLocation));
                //            strQry += "\"TC_CURRENT_LOCATION\"=:curloc1";
                //        }
                //        else
                //        {
                //            if (objTc.sOfficeCode == "" || objTc.sOfficeCode == null)
                //            {
                //                strQry += " cast(\"TC_LOCATION_ID\" as TEXT) like'%'";
                //            }
                //            else
                //            {
                //                NpgsqlCommand.Parameters.AddWithValue("offcode1", objTc.sOfficeCode);
                //                strQry += "cast(\"TC_LOCATION_ID\" as TEXT) LIKE :offcode1||'%'";
                //            }
                //        }
                //    }
                //}
                //if (objTc.sTcMakeId != null && objTc.sTcMakeId != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("tcmakeid", Convert.ToString(objTc.sTcMakeId.Trim().Replace("'", "''").ToUpper()));
                //    strQry += " AND UPPER(\"TM_NAME\") LIKE '%'||:tcmakeid||'%'";
                //}
                //if (objTc.sTcCapacity != null && objTc.sTcCapacity != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("tccap", Convert.ToDouble(objTc.sTcCapacity));
                //    strQry += " AND \"TC_CAPACITY\"=:tccap";
                //}
                //if (objTc.sCurrentLocation != null && objTc.sCurrentLocation != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("curloc", Convert.ToDouble(objTc.sCurrentLocation));
                //    strQry += " AND \"TC_CURRENT_LOCATION\"=:curloc";
                //}
                //if (objTc.sTcCode != null && objTc.sTcCode != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("tcd", Convert.ToString(objTc.sTcCode.Trim().Replace("'", "''").ToUpper()));
                //    strQry += "AND \"TC_CODE\" like '%'||:tcd||'%' ";
                //}
                //if (objTc.sTcSlNo != null && objTc.sTcSlNo != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("sTcSlNo", objTc.sTcSlNo.ToUpper());
                //    strQry += "AND UPPER(\"TC_SLNO\") like '%" + objTc.sTcSlNo.ToUpper() + "%' ";
                //}
                //if (objTc.sOfficeCode != null && objTc.sOfficeCode != "")
                //{
                //    strQry += " ORDER BY \"TC_ID\" DESC";
                //}
                //else
                //{
                //    strQry += " ORDER BY \"TC_ID\" DESC limit 300";
                //}
                //dtTcDetails = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_tcmaster_search");
                cmd.Parameters.AddWithValue("p_roletype", (objTc.sroletype ?? ""));
                cmd.Parameters.AddWithValue("p_offcd", (objTc.sOfficeCode ?? ""));
                cmd.Parameters.AddWithValue("p_locid", (objTc.sLocationId ?? ""));
                cmd.Parameters.AddWithValue("p_curloc", (objTc.sCurrentLocation ?? ""));
                cmd.Parameters.AddWithValue("p_tcmakeid", (objTc.sTcMakeId ?? "").ToUpper());
                cmd.Parameters.AddWithValue("p_tccap", (objTc.sTcCapacity ?? ""));
                cmd.Parameters.AddWithValue("p_tcd", (objTc.sTcCode ?? ""));
                cmd.Parameters.AddWithValue("p_tcslno", (objTc.sTcSlNo ?? "").ToUpper());
                dtTcDetails = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtTcDetails;
            }
            return dtTcDetails;
        }
        /// <summary>
        /// Load Inward Tc Master
        /// </summary>
        /// <param name="objTc"></param>
        /// <returns></returns>
        public DataTable LoadInwardTcMaster(clsTcMaster objTc)
        {
            DataTable dtTcDetails = new DataTable();
            try
            {
                #region Old LoadInwardTcMaster
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //strQry = " SELECT (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\") AS \"TC_MAKE\",\"TC_ID\",\"TC_CODE\" AS \"TC_CODE\",";
                //strQry += " \"TC_SLNO\",\"TC_MAKE_ID\",\"TC_CAPACITY\" AS \"TC_CAPACITY\", \"TC_LIFE_SPAN\",\"TC_PO_NO\",\"TC_DI_NO\"  FROM \"TBLTCMASTER\" inner join \"TBLTCRANGEALLOTMENT\" on  \"TC_CODE\" =  \"TCP_TC_CODE\" ";
                //strQry += " WHERE  \"TCP_STATUS_M\" ='1'";

                //if (objTc.sroletype == "2")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("offcd", objTc.sOfficeCode);
                //    strQry += " AND cast(\"TC_LOCATION_ID\"as TEXT)   = :offcd";
                //}
                //else
                //{
                //    if (objTc.sLocationId != null && objTc.sLocationId != "")
                //    {
                //        NpgsqlCommand.Parameters.AddWithValue("locid", Convert.ToDouble(objTc.sLocationId));
                //        strQry += " AND  \"TC_LOCATION_ID\"  = :locid";
                //    }
                //    else
                //    {
                //        if (objTc.sCurrentLocation == "1" || objTc.sCurrentLocation == "3")
                //        {
                //            NpgsqlCommand.Parameters.AddWithValue("curloc1", Convert.ToDouble(objTc.sCurrentLocation));
                //            strQry += " and \"TC_CURRENT_LOCATION\"=:curloc1";
                //        }
                //        else
                //        {
                //            if (objTc.sOfficeCode == "" || objTc.sOfficeCode == null)
                //            {
                //                strQry += " AND   cast(\"TC_LOCATION_ID\" as TEXT) like'%'";
                //            }
                //            else
                //           if (objTc.sOfficeCode.Length >= 2)
                //            {
                //                string StoreId = GetStoreId(objTc.sOfficeCode);
                //                NpgsqlCommand.Parameters.AddWithValue("storeId", StoreId);
                //                strQry += "and cast(\"TC_LOCATION_ID\" as TEXT) LIKE :storeId||'%'";
                //            }
                //            else
                //            {
                //                NpgsqlCommand.Parameters.AddWithValue("offcode1", objTc.sOfficeCode);
                //                strQry += "and cast(\"TC_LOCATION_ID\" as TEXT) LIKE :offcode1||'%'";
                //            }

                //        }
                //    }
                //}
                //if (objTc.sTcMakeId != null && objTc.sTcMakeId != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("tcmakeid", objTc.sTcMakeId.ToUpper());
                //    strQry += " AND \"TC_MAKE_ID\"=(SELECT \"TM_ID\" FROM \"TBLTRANSMAKES\" WHERE \"TM_NAME\"=:tcmakeid)";
                //}
                //if (objTc.sTcCapacity != null && objTc.sTcCapacity != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("tccap", Convert.ToDouble(objTc.sTcCapacity));
                //    strQry += " AND \"TC_CAPACITY\"=:tccap";
                //}
                //if (objTc.sDivId != null && objTc.sDivId != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("curloc", Convert.ToInt64(objTc.sDivId));
                //    strQry += " AND \"TC_STORE_ID\"=:curloc";
                //}
                //if (objTc.sTcCode != null && objTc.sTcCode != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("tcd", Convert.ToString(objTc.sTcCode));
                //    strQry += "AND \"TC_CODE\" =:tcd ";
                //}
                //if (objTc.sTcSlNo != null && objTc.sTcSlNo != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("sTcSlNo", objTc.sTcSlNo.ToUpper());
                //    strQry += "AND \"TC_SLNO\" =:sTcSlNo ";
                //}
                //if (objTc.sPurchaseno != null && objTc.sPurchaseno != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("sPurchaseno", objTc.sPurchaseno.ToUpper());
                //    strQry += "AND \"TC_PO_NO\" =:sPurchaseno ";
                //}
                //if (objTc.sDino != null && objTc.sDino != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("sDino", objTc.sDino.ToUpper());
                //    strQry += "AND \"TC_DI_NO\" =:sDino ";
                //}
                //if (objTc.sOfficeCode != null && objTc.sOfficeCode != "")
                //{
                //    strQry += " ORDER BY \"TC_ID\" DESC";
                //}
                //else
                //{
                //    strQry += " ORDER BY \"TC_ID\" DESC limit 100";
                //}
                //dtTcDetails = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_inward_tcmaster");
                cmd.Parameters.AddWithValue("p_roletype", (objTc.sroletype ?? ""));
                cmd.Parameters.AddWithValue("p_offcd", (objTc.sOfficeCode ?? ""));
                cmd.Parameters.AddWithValue("p_locid", ((objTc.sLocationId ?? "").Length > 0) ? objTc.sLocationId : "");
                cmd.Parameters.AddWithValue("p_curloc1", (objTc.sOfficeCode ?? ""));
                cmd.Parameters.AddWithValue("p_storeid", ((objTc.sOfficeCode ?? "").Length >= 2) ? GetStoreId(objTc.sOfficeCode) : "");
                cmd.Parameters.AddWithValue("p_tcmakeid", ((objTc.sTcMakeId ?? "").Length > 0) ? objTc.sTcMakeId.ToUpper() : "");
                cmd.Parameters.AddWithValue("p_tccap", (objTc.sTcCapacity ?? ""));
                cmd.Parameters.AddWithValue("p_curloc", (objTc.sDivId ?? ""));
                cmd.Parameters.AddWithValue("p_tcd", Convert.ToString((objTc.sTcCode ?? "")));
                cmd.Parameters.AddWithValue("p_tcslno", ((objTc.sTcSlNo ?? "").Length > 0) ? objTc.sTcSlNo.ToUpper() : "");
                cmd.Parameters.AddWithValue("p_purchaseno", ((objTc.sPurchaseno ?? "").Length > 0) ? objTc.sPurchaseno.ToUpper() : "");
                cmd.Parameters.AddWithValue("p_dino", ((objTc.sDino ?? "").Length > 0) ? objTc.sDino.ToUpper() : "");
                dtTcDetails = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtTcDetails;
            }
            return dtTcDetails;
        }
        /// <summary>
        /// get TC Count
        /// </summary>
        /// <param name="off_Code"></param>
        /// <param name="sroleType"></param>
        /// <param name="currentlocation"></param>
        /// <param name="makeId"></param>
        /// <param name="locationid"></param>
        /// <param name="sTcCapacity"></param>
        /// <returns></returns>
        public string getTCCount(string off_Code, string sroleType, string currentlocation, string makeId, string locationid, string sTcCapacity)
        {
            string TCCount = string.Empty;
            DataTable dtTCCount = new DataTable();
            try
            {
                #region Old getTCCount
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //if (sroleType == "2")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("offcode", off_Code);
                //    if (currentlocation != null && currentlocation != "")
                //    {
                //        NpgsqlCommand.Parameters.AddWithValue("currentlocation", currentlocation);
                //        strQry = "SELECT count(\"TC_CODE\") FROM \"TBLTCMASTER\" WHERE cast(\"TC_LOCATION_ID\" as text) =:offcode and cast(\"TC_CURRENT_LOCATION\" as text)=:currentlocation";
                //        if (makeId != null && makeId != "")
                //        {
                //            NpgsqlCommand.Parameters.AddWithValue("makeId1", makeId);
                //            strQry += " and cast(\"TC_MAKE_ID\" as text)=:makeId1";
                //        }
                //    }
                //    else
                //    {
                //        strQry = "SELECT count(\"TC_CODE\") FROM \"TBLTCMASTER\" WHERE cast(\"TC_LOCATION_ID\" as text) =:offcode ";
                //        if (makeId != null && makeId != "")
                //        {
                //            NpgsqlCommand.Parameters.AddWithValue("makeId2", makeId);
                //            strQry += " and cast(\"TC_MAKE_ID\" as text)=:makeId2";
                //        }
                //    }
                //}
                //else
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("offcode1", off_Code);
                //    if (currentlocation != null && currentlocation != "")
                //    {
                //        if (currentlocation == "1" || currentlocation == "3" || currentlocation == "5" && locationid != "")
                //        //if (currentlocation != "2" && locationid != null)
                //        {

                //            NpgsqlCommand.Parameters.AddWithValue("currentlocation1", currentlocation);
                //            //NpgsqlCommand.Parameters.AddWithValue("locid", locationid);
                //            if (locationid != "" && locationid != null)
                //            {
                //                if (locationid.Length > '3')
                //                {
                //                    sStoreId = GetStoreId(locationid.Substring(0, 3));
                //                    //NpgsqlCommand.Parameters.AddWithValue("offcd1", sStoreId);
                //                }
                //            }

                //            //strQry = "SELECT count(\"TC_CODE\") FROM \"TBLTCMASTER\" WHERE cast(\"TC_LOCATION_ID\" as text) like :offcode1||'%' and  \"TC_CODE\"<>0 and cast(\"TC_CURRENT_LOCATION\" as text)=:currentlocation1";
                //            //strQry = "SELECT count(\"TC_CODE\") FROM \"TBLTCMASTER\" WHERE cast(\"TC_LOCATION_ID\" as text) =:locid and cast(\"TC_CURRENT_LOCATION\" as text)=:currentlocation1";
                //            //strQry = "SELECT count(\"TC_CODE\") FROM \"TBLTCMASTER\" WHERE cast(\"TC_STORE_ID\" as text) ='"+sStoreId+"' and cast(\"TC_CURRENT_LOCATION\" as text)='"+ currentlocation + "'";

                //            if (sStoreId != "" && sStoreId != null)
                //            {
                //                strQry = "SELECT count(\"TC_CODE\") FROM \"TBLTCMASTER\" WHERE cast(\"TC_STORE_ID\" as text) ='" + sStoreId + "' and cast(\"TC_CURRENT_LOCATION\" as text)='" + currentlocation + "'";
                //            }
                //            else if (off_Code != "" && off_Code != null)
                //            {
                //                if (locationid != null && locationid != "")
                //                {
                //                    NpgsqlCommand.Parameters.AddWithValue("locid", locationid);
                //                    strQry = "SELECT count(\"TC_CODE\") FROM \"TBLTCMASTER\" WHERE cast(\"TC_LOCATION_ID\" as text) like :locid||'%' and \"TC_CODE\"<>'0' and cast(\"TC_CURRENT_LOCATION\" as text)=:currentlocation1";
                //                }
                //                else
                //                {
                //                    //strQry = "SELECT count(\"TC_CODE\") FROM \"TBLTCMASTER\" WHERE cast(\"TC_LOCATION_ID\" as text) ='"+ locationid + "' and cast(\"TC_CURRENT_LOCATION\" as text)=:currentlocation1";
                //                    strQry = "SELECT count(\"TC_CODE\") FROM \"TBLTCMASTER\" WHERE cast(\"TC_LOCATION_ID\" as text) like :offcode1||'%' and \"TC_CODE\"<>'0' and cast(\"TC_CURRENT_LOCATION\" as text)=:currentlocation1";
                //                }
                //            }
                //            else
                //            {
                //                if (locationid != null && locationid != "")
                //                {
                //                    NpgsqlCommand.Parameters.AddWithValue("locid", locationid);
                //                    strQry = "SELECT count(\"TC_CODE\") FROM \"TBLTCMASTER\" WHERE cast(\"TC_LOCATION_ID\" as text) like :locid||'%' and \"TC_CODE\"<>'0' and cast(\"TC_CURRENT_LOCATION\" as text)=:currentlocation1";
                //                }
                //                else
                //                {
                //                    strQry = "SELECT count(\"TC_CODE\") FROM \"TBLTCMASTER\" WHERE cast(\"TC_LOCATION_ID\" as text) like :offcode1||'%' and \"TC_CODE\"<>'0' and cast(\"TC_CURRENT_LOCATION\" as text)=:currentlocation1";
                //                }

                //            }
                //            if (makeId != null && makeId != "")
                //            {
                //                NpgsqlCommand.Parameters.AddWithValue("makeId3", makeId);
                //                strQry += " and cast(\"TC_MAKE_ID\" as text)=:makeId3";
                //            }
                //            if (sTcCapacity != null && sTcCapacity != "")
                //            {
                //                NpgsqlCommand.Parameters.AddWithValue("tccap", Convert.ToDouble(sTcCapacity));
                //                strQry += " AND \"TC_CAPACITY\"=:tccap";
                //            }
                //        }
                //        else
                //        {
                //            //if (currentlocation == "1" || currentlocation == "3" || currentlocation == "5" && locationid == null)
                //            if (currentlocation != "2" && locationid == null)
                //            {
                //                NpgsqlCommand.Parameters.AddWithValue("currentlocation1", currentlocation);
                //                //strQry = "SELECT count(\"TC_CODE\") FROM \"TBLTCMASTER\" WHERE cast(\"TC_LOCATION_ID\" as text) like :offcode1||'%' and  \"TC_CODE\"<>0 and cast(\"TC_CURRENT_LOCATION\" as text)=:currentlocation1";
                //                strQry = "SELECT count(\"TC_CODE\") FROM \"TBLTCMASTER\" WHERE  cast(\"TC_CURRENT_LOCATION\" as text)=:currentlocation1 and  \"TC_CODE\"<>'0'";
                //                if (makeId != null && makeId != "")
                //                {
                //                    NpgsqlCommand.Parameters.AddWithValue("makeId3", makeId);
                //                    strQry += " and cast(\"TC_MAKE_ID\" as text)=:makeId3";
                //                }
                //                if (sTcCapacity != null && sTcCapacity != "")
                //                {
                //                    NpgsqlCommand.Parameters.AddWithValue("tccap", Convert.ToDouble(sTcCapacity));
                //                    strQry += " AND \"TC_CAPACITY\"=:tccap";
                //                }
                //            }
                //            else
                //            {
                //                NpgsqlCommand.Parameters.AddWithValue("currentlocation1", currentlocation);
                //                //strQry = "SELECT count(\"TC_CODE\") FROM \"TBLTCMASTER\" WHERE cast(\"TC_LOCATION_ID\" as text) like :offcode1||'%' and  \"TC_CODE\"<>0 and cast(\"TC_CURRENT_LOCATION\" as text)=:currentlocation1";
                //                strQry = "SELECT count(\"TC_CODE\") FROM \"TBLTCMASTER\" WHERE cast(\"TC_LOCATION_ID\" as text) like :offcode1||'%' and cast(\"TC_CURRENT_LOCATION\" as text)=:currentlocation1 and  \"TC_CODE\"<>'0'";
                //                //strQry = "SELECT count(\"TC_CODE\") FROM \"TBLTCMASTER\" WHERE cast(\"TC_CURRENT_LOCATION\" as text)=:currentlocation1";
                //                if (makeId != null && makeId != "")
                //                {
                //                    NpgsqlCommand.Parameters.AddWithValue("makeId3", makeId);
                //                    strQry += " and cast(\"TC_MAKE_ID\" as text)=:makeId3";
                //                }
                //                if (sTcCapacity != null && sTcCapacity != "")
                //                {
                //                    NpgsqlCommand.Parameters.AddWithValue("tccap", Convert.ToDouble(sTcCapacity));
                //                    strQry += " AND \"TC_CAPACITY\"=:tccap";
                //                }
                //            }
                //        }
                //    }
                //    else
                //    {
                //        //strQry = "SELECT count(\"TC_CODE\") FROM \"TBLTCMASTER\" WHERE cast(\"TC_LOCATION_ID\" as text) like :offcode1||'%' and  \"TC_CODE\"<>0 ";
                //        strQry = "SELECT count(\"TC_CODE\") FROM \"TBLTCMASTER\" WHERE cast(\"TC_LOCATION_ID\" as text) like :offcode1||'%' and  \"TC_CODE\"<>'0' ";
                //        if (makeId != null && makeId != "")
                //        {
                //            NpgsqlCommand.Parameters.AddWithValue("makeId4", makeId);
                //            strQry += " and cast(\"TC_MAKE_ID\" as text)=:makeId4";
                //        }
                //        if (sTcCapacity != null && sTcCapacity != "")
                //        {
                //            NpgsqlCommand.Parameters.AddWithValue("tccap", Convert.ToDouble(sTcCapacity));
                //            strQry += " AND \"TC_CAPACITY\"=:tccap";
                //        }
                //    }
                //}
                //TCCount = ObjCon.get_value(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_tccount");
                cmd.Parameters.AddWithValue("p_roleType", (sroleType ?? ""));
                cmd.Parameters.AddWithValue("p_offcode", (off_Code ?? ""));
                cmd.Parameters.AddWithValue("p_curloc", (currentlocation ?? ""));
                cmd.Parameters.AddWithValue("p_locid", (locationid ?? ""));
                cmd.Parameters.AddWithValue("p_makeId", (makeId ?? ""));
                cmd.Parameters.AddWithValue("p_tccap", (sTcCapacity ?? ""));
                dtTCCount = ObjCon.FetchDataTable(cmd);
                if (dtTCCount.Rows.Count > 0)
                {
                    TCCount = dtTCCount.Rows[0]["TC_CODE_COUNT"].ToString();
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
            return TCCount;
        }
        /// <summary>
        /// get Inward TC Count
        /// </summary>
        /// <param name="off_Code"></param>
        /// <param name="sroleType"></param>
        /// <param name="sDivId"></param>
        /// <param name="sTcMakeId"></param>
        /// <param name="sTcCapacity"></param>
        /// <returns></returns>
        public string getInwardTCCount(string off_Code, string sroleType, string sDivId, string sTcMakeId, string sTcCapacity)
        {
            string InwardTCCount = string.Empty;
            DataTable dtInwardTCCount = new DataTable();
            try
            {
                #region Old getInwardTCCount
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //if (sroleType == "2")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("offcode", off_Code);

                //    if (sDivId != null && sDivId != "")
                //    {
                //        NpgsqlCommand.Parameters.AddWithValue("currentlocation", sDivId);
                //        strQry = "SELECT count(\"TC_CODE\") FROM \"TBLTCMASTER\" inner join \"TBLTCRANGEALLOTMENT\" on  \"TC_CODE\" =  \"TCP_TC_CODE\" WHERE cast(\"TC_LOCATION_ID\" as text) =:offcode and  cast(\"TC_STORE_ID\" as text)=:currentlocation and \"TCP_STATUS_M\" ='1'";
                //        if (sTcMakeId != null && sTcMakeId != "")
                //        {
                //            NpgsqlCommand.Parameters.AddWithValue("makeId3", sTcMakeId.ToUpper());
                //            strQry += " and cast(\"TC_MAKE_ID\" as int)=(SELECT \"TM_ID\" FROM \"TBLTRANSMAKES\" WHERE \"TM_NAME\"=:makeId3)";
                //        }
                //        if (sTcCapacity != null && sTcCapacity != "")
                //        {
                //            NpgsqlCommand.Parameters.AddWithValue("tccap", Convert.ToDouble(sTcCapacity));
                //            strQry += " AND \"TC_CAPACITY\"=:tccap";
                //        }
                //    }
                //    else
                //    {
                //        strQry = "SELECT count(\"TC_CODE\") FROM \"TBLTCMASTER\" inner join \"TBLTCRANGEALLOTMENT\" on  \"TC_CODE\" =  \"TCP_TC_CODE\" WHERE cast(\"TC_LOCATION_ID\" as text) =:offcode  and \"TCP_STATUS_M\" ='1'";
                //        if (sTcMakeId != null && sTcMakeId != "")
                //        {
                //            NpgsqlCommand.Parameters.AddWithValue("makeId3", sTcMakeId.ToUpper());
                //            strQry += " and cast(\"TC_MAKE_ID\" as int)=(SELECT \"TM_ID\" FROM \"TBLTRANSMAKES\" WHERE \"TM_NAME\"=:makeId3)";
                //        }
                //        if (sTcCapacity != null && sTcCapacity != "")
                //        {
                //            NpgsqlCommand.Parameters.AddWithValue("tccap", Convert.ToDouble(sTcCapacity));
                //            strQry += " AND \"TC_CAPACITY\"=:tccap";
                //        }
                //    }
                //}
                //else if (sroleType == "3")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("offcode", off_Code);
                //    if (sDivId != null && sDivId != "")
                //    {
                //        NpgsqlCommand.Parameters.AddWithValue("currentlocation", sDivId);
                //        strQry = "SELECT count(\"TC_CODE\") FROM \"TBLTCMASTER\" inner join \"TBLTCRANGEALLOTMENT\" on  \"TC_CODE\" =  \"TCP_TC_CODE\" WHERE cast(\"TC_LOCATION_ID\" as text) like :offcode||'%' and  cast(\"TC_STORE_ID\" as text)=:currentlocation and \"TCP_STATUS_M\" ='1'";
                //        if (sTcMakeId != null && sTcMakeId != "")
                //        {
                //            NpgsqlCommand.Parameters.AddWithValue("makeId3", sTcMakeId.ToUpper());
                //            strQry += " and cast(\"TC_MAKE_ID\" as int)=(SELECT \"TM_ID\" FROM \"TBLTRANSMAKES\" WHERE \"TM_NAME\"=:makeId3)";
                //        }
                //        if (sTcCapacity != null && sTcCapacity != "")
                //        {
                //            NpgsqlCommand.Parameters.AddWithValue("tccap", Convert.ToDouble(sTcCapacity));
                //            strQry += " AND \"TC_CAPACITY\"=:tccap";
                //        }
                //    }
                //    else
                //    {
                //        strQry = "SELECT count(\"TC_CODE\") FROM \"TBLTCMASTER\" inner join \"TBLTCRANGEALLOTMENT\" on  \"TC_CODE\" =  \"TCP_TC_CODE\" WHERE cast(\"TC_LOCATION_ID\" as text) like :offcode||'%' and \"TCP_STATUS_M\" ='1' ";

                //        if (sTcMakeId != null && sTcMakeId != "")
                //        {
                //            NpgsqlCommand.Parameters.AddWithValue("makeId3", sTcMakeId.ToUpper());
                //            strQry += " and cast(\"TC_MAKE_ID\" as int)=(SELECT \"TM_ID\" FROM \"TBLTRANSMAKES\" WHERE \"TM_NAME\"=:makeId3)";
                //        }
                //        if (sTcCapacity != null && sTcCapacity != "")
                //        {
                //            NpgsqlCommand.Parameters.AddWithValue("tccap", Convert.ToDouble(sTcCapacity));
                //            strQry += " AND \"TC_CAPACITY\"=:tccap";
                //        }
                //    }
                //}
                //else
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("offcode1", off_Code);
                //    if (sDivId != null && sDivId != "")
                //    {
                //        NpgsqlCommand.Parameters.AddWithValue("currentlocation1", sDivId);
                //        strQry = "SELECT count(\"TC_CODE\") FROM \"TBLTCMASTER\"  inner join \"TBLTCRANGEALLOTMENT\" on  \"TC_CODE\" =  \"TCP_TC_CODE\" WHERE cast(\"TC_LOCATION_ID\" as text) like :offcode1||'%' and  \"TC_CODE\"<>'0' and cast(\"TC_DIV_ID\" as text)=:currentlocation1 and \"TCP_STATUS_M\" ='1'";
                //    }
                //    else
                //    {
                //        string StoreId = GetStoreId(off_Code);
                //        NpgsqlCommand.Parameters.AddWithValue("storeId", StoreId);
                //        strQry = "SELECT count(\"TC_CODE\") FROM \"TBLTCMASTER\"  inner join \"TBLTCRANGEALLOTMENT\" on  \"TC_CODE\" =  \"TCP_TC_CODE\" WHERE cast(\"TC_LOCATION_ID\" as text) like :storeId||'%' and  \"TC_CODE\"<>'0' and \"TCP_STATUS_M\" ='1'";
                //    }
                //}
                //InwardTCCount = ObjCon.get_value(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_inwardtc_count");
                cmd.Parameters.AddWithValue("p_roleType", (sroleType ?? ""));
                cmd.Parameters.AddWithValue("p_offcode", (off_Code ?? ""));
                cmd.Parameters.AddWithValue("p_curloc", (sDivId ?? ""));
                cmd.Parameters.AddWithValue("p_makeId", (sTcMakeId ?? "").ToUpper());
                cmd.Parameters.AddWithValue("p_tccap", (sTcCapacity ?? ""));
                dtInwardTCCount = ObjCon.FetchDataTable(cmd);
                if (dtInwardTCCount.Rows.Count > 0)
                {
                    InwardTCCount = dtInwardTCCount.Rows[0]["TC_CODE_COUNT"].ToString();
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
            return InwardTCCount;
        }
        /// <summary>
        /// Get Image Path
        /// </summary>
        /// <param name="objDtrCComm"></param>
        /// <returns></returns>
        public clsTcMaster GetImagePath(clsTcMaster objDtrCComm)
        {
            DataTable dt = new DataTable();
            try
            {
                #region Old GetImagePath
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("tccode", Convert.ToString(objDtrCComm.sTcCode));
                //strQry = "SELECT \"EP_NAMEPLATE_PATH\",\"EP_SSPLATE_PATH\" FROM \"TBLDTCENUMERATION\",\"TBLENUMERATIONPHOTOS\",\"TBLENUMERATIONDETAILS\"  ";
                //strQry += " WHERE \"DTE_TC_CODE\"=:tccode AND \"DTE_ED_ID\"=\"EP_ED_ID\" AND \"ED_ID\"=\"DTE_ED_ID\" AND \"ED_STATUS_FLAG\"='1' ORDER BY \"EP_ID\" desc";
                //dt = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_imagepath_for_enum");
                cmd.Parameters.AddWithValue("p_tccode", Convert.ToString(objDtrCComm.sTcCode));
                dt = ObjCon.FetchDataTable(cmd);
                if (dt.Rows.Count > 0)
                {
                    objDtrCComm.sDTrImagePath = Convert.ToString(dt.Rows[0]["EP_NAMEPLATE_PATH"]);
                    objDtrCComm.sNamePlateImagePath = Convert.ToString(dt.Rows[0]["EP_SSPLATE_PATH"]);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objDtrCComm;
            }
            return objDtrCComm;
        }
        /// <summary>
        /// Get Image Path For New
        /// </summary>
        /// <param name="objDtrCComm"></param>
        /// <returns></returns>
        public clsTcMaster GetImagePathForNew(clsTcMaster objDtrCComm)
        {
            DataTable dt = new DataTable();
            try
            {
                #region Old GetImagePathForNew
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("tccode", Convert.ToString(objDtrCComm.sTcCode));
                //strQry = "SELECT \"PIP_NAME_PLATE\",\"PIP_DTR_CODE_PHOTO\" FROM \"TBLTEMP_PMC_INDENT_ENUMDETAILS\" INNER JOIN ";
                //strQry += " \"TBLPMC_INDENT\" ON \"PI_TPIE_ID\"= \"TPIE_ID\" INNER JOIN \"TBLPMC_INDENTPHOTOS\" ON \"PI_ID\"=\"PIP_PI_ID\" ";
                //strQry += " where \"PI_TC_CODE\"=:tccode ORDER BY \"TPIE_ID\" desc";
                //dt = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_imagepath_for_pmc");
                cmd.Parameters.AddWithValue("p_tccode", Convert.ToString(objDtrCComm.sTcCode));
                dt = ObjCon.FetchDataTable(cmd);
                if (dt.Rows.Count > 0)
                {
                    objDtrCComm.sDTrImagePath = Convert.ToString(dt.Rows[0]["PIP_NAME_PLATE"]);
                    objDtrCComm.sNamePlateImagePath = Convert.ToString(dt.Rows[0]["PIP_DTR_CODE_PHOTO"]);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objDtrCComm;
            }
            return objDtrCComm;
        }
        /// <summary>
        /// Get TC Details
        /// </summary>
        /// <param name="objTcMaster"></param>
        /// <returns></returns>
        public clsTcMaster GetTCDetails(clsTcMaster objTcMaster)
        {
            string strQry = string.Empty;
            DataTable dtStoreDetails = new DataTable();
            try
            {
                #region Old GetTCDetails                
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("tcid", Convert.ToInt32(objTcMaster.sTcId));
                //strQry = "SELECT \"TC_ID\",\"TC_CODE\",\"TC_ALT_NO\",\"TC_SLNO\",\"TC_MAKE_ID\",CAST(\"TC_CAPACITY\" as TEXT) \"TC_CAPACITY\",TO_CHAR(\"TC_MANF_DATE\",";
                //strQry += " 'dd/MM/yyyy')TC_MANF_DATE, TO_CHAR(\"TC_PURCHASE_DATE\",'dd/MM/yyyy')TC_PURCHASE_DATE,\"TC_LIFE_SPAN\" ,\"TC_OIL_TYPE\"  ";
                //strQry += " ,\"TC_SUPPLIER_ID\",\"TC_PO_NO\",\"TC_DI_NO\",\"TC_PRICE\",\"TC_LOCATION_ID\", TO_CHAR(\"TC_WARANTY_PERIOD\",'dd/MM/yyyy')TC_WARANTY_PERIOD,\"TC_WARRENTY\",";
                //strQry += "TO_CHAR(\"TC_LAST_SERVICE_DATE\",'dd/MM/yyyy')TC_LAST_SERVICE_DATE, \"TC_STATUS\",\"TC_CURRENT_LOCATION\",\"TC_LAST_REPAIRER_ID\"";
                //strQry += ",\"TC_RATING\",\"TC_STAR_RATE\",\"TC_OIL_CAPACITY\",\"TC_WEIGHT\",TO_CHAR(\"TC_LAST_FAILURE_DATE\",'dd/MM/yyyy')TC_LAST_FAILURE_DATE,";
                //strQry += " \"TC_LAST_REPAIR_COST\",\"TC_LAST_FAILURE_TYPE\",\"TC_CONDITION\",\"TC_COOLING\",\"TC_TYPE\",\"TC_CORE\",\"TC_TAP_CHARGER\",";
                //strQry += " \"TC_ASSET_ID\",\"TC_COMPONENT_ID\",\"TC_ORIGINAL_COST\",\"TC_INSURANCE\",\"TC_DEPRECIATION\",(SELECT to_char(\"TM_CRON\",'dd-mm-yyyy')\"COMMISSIONED_YEAR\"";
                //strQry += " FROM (SELECT \"TM_CRON\", row_number() over(ORDER BY \"TM_ID\")  FROM \"TBLTRANSDTCMAPPING\" WHERE \"TM_TC_ID\"=\"TC_CODE\")A WHERE row_number='1') ";
                //strQry += " FROM \"TBLTCMASTER\" WHERE \"TC_ID\" =:tcid";
                //dtStoreDetails = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_tc_details");
                cmd.Parameters.AddWithValue("p_tcid", objTcMaster.sTcId);
                dtStoreDetails = ObjCon.FetchDataTable(cmd);

                if (dtStoreDetails.Rows.Count > 0)
                {
                    objTcMaster.sTcId = dtStoreDetails.Rows[0]["TC_ID"].ToString();
                    objTcMaster.sTcCode = dtStoreDetails.Rows[0]["TC_CODE"].ToString();
                    objTcMaster.sTcSlNo = dtStoreDetails.Rows[0]["TC_SLNO"].ToString();
                    objTcMaster.sTcMakeId = dtStoreDetails.Rows[0]["TC_MAKE_ID"].ToString();
                    objTcMaster.sLocationId = dtStoreDetails.Rows[0]["TC_LOCATION_ID"].ToString();
                    objTcMaster.sAltNo = dtStoreDetails.Rows[0]["TC_ALT_NO"].ToString();
                    objTcMaster.sTcCapacity = dtStoreDetails.Rows[0]["TC_CAPACITY"].ToString();
                    objTcMaster.sTcLifeSpan = dtStoreDetails.Rows[0]["TC_LIFE_SPAN"].ToString();
                    objTcMaster.sManufacDate = dtStoreDetails.Rows[0]["TC_MANF_DATE"].ToString();
                    objTcMaster.sAllotementDate = dtStoreDetails.Rows[0]["TC_PURCHASE_DATE"].ToString();
                    objTcMaster.sPoNo = dtStoreDetails.Rows[0]["TC_PO_NO"].ToString();
                    objTcMaster.sDINo = dtStoreDetails.Rows[0]["TC_DI_NO"].ToString();
                    objTcMaster.sPrice = dtStoreDetails.Rows[0]["TC_PRICE"].ToString();
                    objTcMaster.sSupplierId = dtStoreDetails.Rows[0]["TC_SUPPLIER_ID"].ToString();
                    objTcMaster.sWarrentyPeriod = dtStoreDetails.Rows[0]["TC_WARRENTY"].ToString();
                    objTcMaster.sLastServiceDate = dtStoreDetails.Rows[0]["TC_LAST_SERVICE_DATE"].ToString();
                    objTcMaster.sCurrentLocation = dtStoreDetails.Rows[0]["TC_CURRENT_LOCATION"].ToString();
                    objTcMaster.sLastRepairerId = dtStoreDetails.Rows[0]["TC_LAST_REPAIRER_ID"].ToString();
                    objTcMaster.sRating = Convert.ToString(dtStoreDetails.Rows[0]["TC_RATING"]);
                    objTcMaster.sStarRate = Convert.ToString(dtStoreDetails.Rows[0]["TC_STAR_RATE"]);
                    objTcMaster.sOilCapacity = Convert.ToString(dtStoreDetails.Rows[0]["TC_OIL_CAPACITY"]);
                    objTcMaster.sOilType = Convert.ToString(dtStoreDetails.Rows[0]["TC_OIL_TYPE"]);
                    objTcMaster.sWeight = Convert.ToString(dtStoreDetails.Rows[0]["TC_WEIGHT"]);
                    objTcMaster.sLastFaildate = Convert.ToString(dtStoreDetails.Rows[0]["TC_LAST_FAILURE_DATE"]);
                    objTcMaster.sLastFailuretype = Convert.ToString(dtStoreDetails.Rows[0]["TC_LAST_FAILURE_TYPE"]);
                    objTcMaster.sLastRepaircost = Convert.ToString(dtStoreDetails.Rows[0]["TC_LAST_REPAIR_COST"]);
                    objTcMaster.sConditiontc = Convert.ToString(dtStoreDetails.Rows[0]["TC_CONDITION"]);
                    objTcMaster.sCooling = Convert.ToString(dtStoreDetails.Rows[0]["TC_COOLING"]);
                    objTcMaster.sCore = Convert.ToString(dtStoreDetails.Rows[0]["TC_TYPE"]);
                    objTcMaster.sType = Convert.ToString(dtStoreDetails.Rows[0]["TC_CORE"]);
                    objTcMaster.sTapeCharger = Convert.ToString(dtStoreDetails.Rows[0]["TC_TAP_CHARGER"]);
                    objTcMaster.sDTRcommissionYear = Convert.ToString(dtStoreDetails.Rows[0]["COMMISSIONED_YEAR"]);
                    objTcMaster.sLastRepaircount = "0";
                    objTcMaster.sInfosysId = Convert.ToString(dtStoreDetails.Rows[0]["TC_ASSET_ID"]);
                    objTcMaster.sComponentId = Convert.ToString(dtStoreDetails.Rows[0]["TC_COMPONENT_ID"]);
                    objTcMaster.sOriginalCost = Convert.ToString(dtStoreDetails.Rows[0]["TC_ORIGINAL_COST"]);
                    objTcMaster.sInsurance = Convert.ToString(dtStoreDetails.Rows[0]["TC_INSURANCE"]);
                    objTcMaster.sDepreciation = Convert.ToString(dtStoreDetails.Rows[0]["TC_DEPRECIATION"]);

                    #region old inline queary
                    //strQry = string.Empty;
                    //NpgsqlCommand = new NpgsqlCommand();
                    //NpgsqlCommand.Parameters.AddWithValue("tccd", Convert.ToString(objTcMaster.sTcCode));
                    //strQry = "SELECT count(*) FROM \"TBLDTCFAILURE\" WHERE \"DF_EQUIPMENT_ID\"=:tccd";
                    //objTcMaster.sFailCount = Convert.ToString(ObjCon.get_value(strQry, NpgsqlCommand));
                    #endregion

                    string QryKey = string.Empty;
                    QryKey = "GET_TBLDTCFAILURE_COUNT";
                    NpgsqlCommand cmd_TC_ID_ON_TC_SLNO = new NpgsqlCommand("fetch_getvalue_clstcmaster");
                    cmd_TC_ID_ON_TC_SLNO.Parameters.AddWithValue("p_key", QryKey);
                    cmd_TC_ID_ON_TC_SLNO.Parameters.AddWithValue("p_value_1", Convert.ToString(objTcMaster.sTcCode ?? ""));
                    cmd_TC_ID_ON_TC_SLNO.Parameters.AddWithValue("p_value_2", "");
                    cmd_TC_ID_ON_TC_SLNO.Parameters.AddWithValue("p_value_3", "");
                    objTcMaster.sFailCount = ObjBseCon.StringGetValue(cmd_TC_ID_ON_TC_SLNO);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objTcMaster;
            }
            return objTcMaster;
        }
        /// <summary>
        /// Get Store Id
        /// </summary>
        /// <param name="sOfficeCode"></param>
        /// <returns></returns> 
        public string GetStoreId(string sOfficeCode)
        {
            string StoreId = string.Empty;
            try
            {
                if (sOfficeCode.Length > 2)
                {
                    sOfficeCode = sOfficeCode.Substring(0, Constants.Division);
                }
                if (sOfficeCode == "")
                {
                    return "";
                }

                #region old GetStoreIds
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();               
                //NpgsqlCommand.Parameters.AddWithValue("offcd", Convert.ToInt32(sOfficeCode));
                //strQry = "SELECT \"SM_ID\" FROM \"TBLSTOREMAST\",\"TBLSTOREOFFCODE\" WHERE \"SM_ID\" = \"STO_SM_ID\" AND   \"STO_OFF_CODE\"=:offcd";
                //StoreId = ObjCon.get_value(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_storeid");
                cmd.Parameters.AddWithValue("p_offcd", Convert.ToInt32(sOfficeCode));
                StoreId = ObjBseCon.StringGetValue(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
            return StoreId;
        }
        /// <summary>
        /// Check Transformer Code Exist
        /// </summary>
        /// <param name="objTcMaster"></param>
        /// <returns></returns>
        public bool CheckTransformerCodeExist(clsTcMaster objTcMaster)
        {
            string QryVal = string.Empty;
            try
            {
                #region Old CheckTransformerCodeExist
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("tccode", Convert.ToInt32(objTcMaster.sTcCode));
                //QryVal = ObjCon.get_value("select * from \"TBLTCMASTER\" where \"TC_CODE\"=:tccode ", NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_check_dtrcode_exist");
                cmd.Parameters.AddWithValue("p_tccode", (objTcMaster.sTcCode ?? ""));
                QryVal = ObjBseCon.StringGetValue(cmd);

                if (QryVal != "")
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }
        /// <summary>
        /// Get Alloted Details
        /// </summary>
        /// <param name="objTcMaster"></param>
        /// <returns></returns>
        public DataTable GetAllotedDetails(clsTcMaster objTcMaster)
        {
            try
            {
                #region Old GetAllotedDetails
                //string sQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("Altno", objTcMaster.sAltNo);
                //NpgsqlCommand.Parameters.AddWithValue("offcd", objTcMaster.sOfficeCode);
                //NpgsqlCommand.Parameters.AddWithValue("dino", objTcMaster.sDINo);
                //sQry = "select \"DIM_ID\",\"PO_NO\",\"DIM_DI_NO\",TO_CHAR(\"PO_DATE\", 'DD/MM/YYYY') AS \"PO_DATE\",\"SM_ID\",\"SM_NAME\" AS \"STORE_NAME\",\"DIV_NAME\" AS \"DIV_NAME\",\"DI_CAPACITY\",\"DI_CAPACITY_ID\",\"TM_NAME\" AS \"MAKE_NAME\",\"DI_STARTTYPE\", ";
                //sQry += " \"MD_NAME\" AS \"ALT_STARRATENAME\" ,\"DIV_ID\",\"DI_MAKE_ID\" ,\"PO_SUPPLIER_ID\",\"TS_NAME\" AS \"SUPPLIER_NAME\" from \"TBLDELIVERYINSTRUCTION\" inner join \"TBLDELIVERYINSTMASTER\" on \"DIM_ID\" = \"DI_DIM_ID\"  inner join ";
                //sQry += " \"TBLSTOREMAST\" on \"DI_STORE_ID\" = \"SM_ID\" inner join \"TBLPOMASTER\" on  \"DI_PO_ID\" = \"PO_ID\" inner join \"TBLTRANSSUPPLIER\" on \"PO_SUPPLIER_ID\" = \"TS_ID\"  inner join \"TBLMASTERDATA\" on \"DI_STARTTYPE\" = \"MD_ID\" ";
                //sQry += " inner join \"TBLDIVISION\" on \"DIV_CODE\" = \"SM_CODE\"  inner join \"TBLTRANSMAKES\" on \"DI_MAKE_ID\" = \"TM_ID\" where   \"MD_TYPE\" = 'SR' and \"DIM_DI_NO\"=:dino ";
                //objTcMaster.dtTable = ObjCon.FetchDataTable(sQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_alloted_details");
                cmd.Parameters.AddWithValue("p_dino", (objTcMaster.sDINo ?? ""));
                objTcMaster.dtTable = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objTcMaster.dtTable;
            }
            return objTcMaster.dtTable;
        }
        /// <summary>
        /// load Count Tc
        /// </summary>
        /// <param name="objTcMaster"></param>
        /// <returns></returns>
        public string loadCountTc(clsTcMaster objTcMaster)
        {
            string sCount = string.Empty;
            try
            {
                #region Old loadCountTc
                //string Qry = string.Empty;
                //NpgsqlCommand.Parameters.AddWithValue("dino", objTcMaster.sDINo);
                //Qry = " select sum(\"DI_QUANTITY\") from \"TBLDELIVERYINSTRUCTION\" ";
                //Qry += " inner join \"TBLDELIVERYINSTMASTER\" on \"DIM_ID\"=\"DI_DIM_ID\"  where  \"DIM_DI_NO\" =:dino ";
                //sCount = ObjCon.get_value(Qry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_count_tc");
                cmd.Parameters.AddWithValue("p_dino", (objTcMaster.sDINo ?? ""));
                sCount = ObjBseCon.StringGetValue(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return sCount;
            }
            return sCount;
        }
        /// <summary>
        /// Load Tc Grid
        /// </summary>
        /// <param name="objTcMaster"></param>
        /// <returns></returns>
        public DataTable LoadTcGrid(clsTcMaster objTcMaster)
        {
            DataTable dtTcDetails = new DataTable();
            try
            {
                #region old LoadTcGrid
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //#region// old query
                //// strQry = "SELECT PO_ID,PO_NO,CAPACITY,REQ_QNTY,(NVL(REQ_QNTY,0) - NVL(SENT_QNT,0)) AS PENDINGCOUNT  FROM ";
                //// strQry += " (SELECT PO_ID,PO_NO,PB_QUANTITY AS REQ_QNTY,TO_CHAR(PB_CAPACITY) AS CAPACITY FROM TBLPOMASTER, TBLPOOBJECTS WHERE PO_ID = PB_PO_ID GROUP BY PO_ID,PO_NO,PB_CAPACITY,PB_QUANTITY)A,";
                //// strQry += " (SELECT TCR_PO_NO,TO_CHAR(PB_CAPACITY) PB_CAPACITY,SUM(TCR_QUANTITY) AS SENT_QNT FROM TBLPOMASTER,TBLTCRECIEPT,TBLPOOBJECTS,";
                //// strQry += " TBLTCMASTER WHERE  PO_ID = PB_PO_ID AND TCR_PO_NO=PO_ID AND PB_PO_ID=TCR_PO_NO AND TC_TCR_ID = TCR_ID ";
                ////strQry+= " AND PB_CAPACITY = TC_CAPACITY GROUP BY TCR_PO_NO,PB_CAPACITY)B WHERE A.PO_ID= B.TCR_PO_NO(+) ";
                ////strQry += " AND A.CAPACITY = B.PB_CAPACITY(+) AND   (NVL(REQ_QNTY,0) - NVL(SENT_QNT,0))<>0 AND A.PO_ID='" + objTcMaster.sPoId + "'";

                ////strQry = "  SELECT \"PO_ID\",\"PO_NO\",\"CAPACITY\",\"REQ_QNTY\",(COALESCE(\"REQ_QNTY\",0) - COALESCE(\"SENT_QNT\",0)) AS \"PENDINGCOUNT\",\"MAKE\" ";
                ////strQry += "  FROM (SELECT \"PO_ID\",\"PO_NO\",\"PB_QUANTITY\" AS \"REQ_QNTY\",CAST(\"PB_CAPACITY\" AS TEXT)  AS \"CAPACITY\", (SELECT \"TM_NAME\"";
                ////strQry += "  FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"PB_MAKE\") AS \"MAKE\" FROM \"TBLPOMASTER\", \"TBLPOOBJECTS\" WHERE  \"PO_ID\" = \"PB_PO_ID\"";
                ////strQry += "  GROUP BY  \"PO_ID\",\"PO_NO\",\"PB_CAPACITY\",\"PB_QUANTITY\",\"PB_MAKE\")A LEFT OUTER JOIN (SELECT \"TCR_PO_NO\",";
                ////strQry += " CAST(\"PB_CAPACITY\" AS TEXT) \"PB_CAPACITY\", count(\"PB_CAPACITY\") AS \"SENT_QNT\" FROM  \"TBLPOMASTER\",\"TBLTCRECIEPT\",\"TBLPOOBJECTS\", ";
                ////strQry += "  \"TBLTCMASTER\" WHERE  \"PO_ID\" = \"PB_PO_ID\" AND  \"TCR_PO_NO\"=CAST(\"PO_ID\" AS TEXT) AND CAST(\"PB_PO_ID\" AS TEXT)=\"TCR_PO_NO\"";
                ////strQry += " AND \"TC_TCR_ID\" =\"TCR_ID\" AND \"PB_CAPACITY\" = \"TC_CAPACITY\" GROUP BY \"TCR_PO_NO\",\"PB_CAPACITY\")B ";
                ////strQry += "ON CAST(A.\"PO_ID\" AS TEXT) = CAST(B.\"TCR_PO_NO\" AS TEXT) AND CAST(A.\"CAPACITY\" AS TEXT) = CAST(B.\"PB_CAPACITY\" ";
                ////strQry += "  AS TEXT) WHERE  A.\"PO_ID\"='" + objTcMaster.sPoId + "'";
                //#endregion
                //NpgsqlCommand.Parameters.AddWithValue("offcd", objTcMaster.sOfficeCode);
                //NpgsqlCommand.Parameters.AddWithValue("altno", objTcMaster.sAltNo);
                //NpgsqlCommand.Parameters.AddWithValue("dino", objTcMaster.sDINo);
                //#region//old Query
                ////strQry = " SELECT \"DI_ID\" \"PO_ID\",\"DI_NO\" \"PO_NO\",\"CAPACITY\",\"REQ_QNTY\",(COALESCE(\"REQ_QNTY\",0) - COALESCE(\"SENT_QNT\",0)) AS \"PENDINGCOUNT\",";
                ////strQry += "  \"MAKE\" ,\"RATING\"   FROM (SELECT \"DI_ID\", \"DI_NO\", SUM(\"DI_QUANTITY\") AS \"REQ_QNTY\", CAST(\"DI_CAPACITY\" AS TEXT) ";
                ////strQry += "  \"CAPACITY\", \"TM_NAME\"AS \"MAKE\"  , \"MD_NAME\" AS \"RATING\" FROM \"TBLDELIVERYINSTRUCTION\", \"TBLTRANSMAKES\", ";
                ////strQry += "  (SELECT \"MD_ID\", \"MD_NAME\" FROM  \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'SR')A  WHERE \"TM_ID\" = \"DI_MAKE_ID\" AND ";
                ////strQry += "  \"DI_STARTTYPE\" = \"MD_ID\" AND \"DI_STORE_ID\" =:offcd GROUP BY \"DI_ID\", \"DI_NO\", \"DI_CAPACITY\", \"TM_NAME\" , \"MD_NAME\")X  LEFT OUTER JOIN ";
                ////strQry += "  (SELECT \"TCR_PO_NO\", CAST(\"DI_CAPACITY\" AS TEXT) \"DI_CAPACITY\", \"MD_NAME\", count(\"DI_CAPACITY\") AS \"SENT_QNT\" ";
                ////strQry += "  FROM \"TBLDELIVERYINSTRUCTION\", \"TBLTCRECIEPT\", \"TBLTCMASTER\", (SELECT \"MD_ID\", \"MD_NAME\" FROM  \"TBLMASTERDATA\" ";
                ////strQry += "  WHERE \"MD_TYPE\" = 'SR')A WHERE \"TCR_PO_NO\"=CAST(\"DI_ID\" AS TEXT) AND \"TC_TCR_ID\" =\"TCR_ID\" AND \"DI_CAPACITY\" = ";
                ////strQry += "  \"TC_CAPACITY\" AND \"TC_RATING\" = \"MD_ID\" GROUP BY \"TCR_PO_NO\",\"DI_CAPACITY\",\"MD_NAME\")Y ON ";
                ////strQry += "  CAST(X.\"DI_ID\" AS TEXT) = CAST(Y.\"TCR_PO_NO\" AS TEXT) AND CAST(X.\"CAPACITY\" AS TEXT) = CAST(Y.\"DI_CAPACITY\"   AS TEXT) ";
                ////strQry += "  AND CAST(X.\"RATING\" AS TEXT) = CAST(Y.\"MD_NAME\" AS TEXT) WHERE X.\"DI_NO\"=:pono ";
                //#endregion
                //strQry = "SELECT  \"DI_NO\", \"CAPACITY\",\"TM_NAME\" , \"RATING\",\"REQ_QNTY\",(COALESCE(\"REQ_QNTY\",0) - COALESCE(\"SENT_QNT\",0)) AS \"PENDINGCOUNT\"  FROM (	SELECT  \"DI_ID\" as sDI_ID, \"DIM_DI_NO\" as \"DI_NO\" ,\"DI_STORE_ID\",\"SM_NAME\" ,\"TM_NAME\" , ";
                //strQry += " CAST(\"DI_CAPACITY\" AS TEXT)   \"CAPACITY\" , \"MD_NAME\"  as \"RATING\" ,  count(*) as \"REQ_QNTY\"  FROM \"TBLDELIVERYINSTMASTER\" INNER JOIN  \"TBLDELIVERYINSTRUCTION\" ON \"DIM_ID\" =\"DI_DIM_ID\" INNER JOIN \"TBLSTOREMAST\" ON ";
                //strQry += " \"DI_STORE_ID\" =\"SM_ID\" INNER JOIN  \"TBLTRANSMAKES\" on \"DI_MAKE_ID\" = \"TM_ID\"  INNER JOIN \"TBLTCRANGEALLOTMENT\" ON \"DI_ID\"= \"TCP_DI_ID\"  inner join \"TBLMASTERDATA\" on \"DI_STARTTYPE\"=\"MD_ID\"  and \"MD_TYPE\" = 'SR' where ";
                //strQry += " \"DIM_DI_NO\"=:dino  GROUP BY  \"DI_ID\", \"DIM_DI_NO\" ,\"DI_STORE_ID\" ,\"SM_NAME\" ,\"TM_NAME\", \"MD_NAME\" )X  LEFT OUTER JOIN  (SELECT  \"DI_ID\", \"DIM_DI_NO\" ,\"DI_STORE_ID\",\"SM_NAME\" ,\"TM_NAME\" AS \"MAKE\" ,";
                //strQry += " CAST(\"DI_CAPACITY\" AS TEXT)   \"DI_CAPACITY\" , \"MD_NAME\" , count(*) as \"SENT_QNT\" FROM \"TBLDELIVERYINSTMASTER\" INNER JOIN \"TBLDELIVERYINSTRUCTION\" ON \"DIM_ID\" =\"DI_DIM_ID\" INNER JOIN \"TBLSTOREMAST\" ON \"DI_STORE_ID\" ";
                //strQry += " =\"SM_ID\"	INNER JOIN  \"TBLTRANSMAKES\" on \"DI_MAKE_ID\" = \"TM_ID\"  INNER JOIN  \"TBLTCRANGEALLOTMENT\" ON \"DI_ID\"= \"TCP_DI_ID\"  AND \"TCP_STATUS_M\"='1'  inner join \"TBLMASTERDATA\" on \"DI_STARTTYPE\"=\"MD_ID\"  and \"MD_TYPE\" = 'SR' ";
                //strQry += " where  \"DIM_DI_NO\"=:dino GROUP BY  \"DI_ID\", \"DIM_DI_NO\" ,\"DI_STORE_ID\" ,\"SM_NAME\" ,\"TM_NAME\", \"MD_NAME\")Y  ON  CAST(X.\"CAPACITY\" AS TEXT) = CAST(Y.\"DI_CAPACITY\" AS TEXT)   AND CAST(X.\"RATING\" AS TEXT) = ";
                //strQry += " CAST(Y.\"MD_NAME\" AS TEXT)  AND CAST(X.\"DI_STORE_ID\" AS TEXT) = CAST(Y.\"DI_STORE_ID\" AS TEXT)  GROUP BY  sDI_ID,\"REQ_QNTY\",\"PENDINGCOUNT\",\"DI_NO\", \"CAPACITY\",\"RATING\",\"TM_NAME\" ";
                //strQry += "  ";
                //dtTcDetails = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_tc_grid");
                cmd.Parameters.AddWithValue("p_dino", (objTcMaster.sDINo ?? ""));
                dtTcDetails = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtTcDetails;
            }
            return dtTcDetails;
        }
        /// <summary>
        /// Get Repairer Details
        /// </summary>
        /// <param name="objTcMaster"></param>
        /// <returns></returns>
        public DataTable GetRepairerDetails(clsTcMaster objTcMaster)
        {
            DataTable dtDetails = new DataTable();
            try
            {
                #region Old GetRepairerDetails
                //string Qry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("offcd", objTcMaster.sLocationId);
                //Qry = "SELECT \"TR_NAME\",\"TR_ADDRESS\",\"TR_MOBILE_NO\",\"TR_EMAIL\"";
                //Qry += " FROM \"TBLTRANSREPAIRER\" ";
                //Qry += " WHERE \"TR_ID\"='" + objTcMaster.sLocationId + "' ";
                //dtDetails = ObjCon.FetchDataTable(Qry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_repairer_details");
                cmd.Parameters.AddWithValue("p_locid", (objTcMaster.sLocationId ?? ""));
                dtDetails = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtDetails;
            }
            return dtDetails;
        }
        /// <summary>
        /// Get Store Details
        /// </summary>
        /// <param name="objTcMaster"></param>
        /// <returns></returns>
        public DataTable GetStoreDetails(clsTcMaster objTcMaster)
        {
            DataTable dtStoreDetails = new DataTable();
            try
            {
                #region Old GetStoreDetails
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("offcode", objTcMaster.srepairOffCode);
                //NpgsqlCommand.Parameters.AddWithValue("tccode", objTcMaster.sTcCode);
                //strQry = "SELECT \"SM_NAME\",\"SM_STORE_INCHARGE\",\"SM_MOBILENO\",\"SM_ADDRESS\" FROM \"TBLSTOREMAST\" ,\"TBLTCMASTER\" WHERE CAST(\"TC_CODE\" AS TEXT)=:tccode AND  cast(\"SM_ID\" as TEXT)=cast(\"TC_STORE_ID\" as TEXT)";
                //dtStoreDetails = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_store_details");
                cmd.Parameters.AddWithValue("p_dtcode", (objTcMaster.sTcCode ?? ""));
                dtStoreDetails = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtStoreDetails;
            }
            return dtStoreDetails;
        }
        /// <summary>
        /// Get Dtc Details
        /// </summary>
        /// <param name="objtcMaster"></param>
        /// <returns></returns>
        public DataTable GetDtcDetails(clsTcMaster objtcMaster)
        {
            DataTable dtDtcDetails = new DataTable();
            try
            {
                #region Old GetDtcDetails
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("dtcode", objtcMaster.sDtcCodes);
                //strQry = " SELECT \"DT_CODE\",\"DT_NAME\",to_char(\"DT_LAST_SERVICE_DATE\",'dd/MM/yyyy')DT_LAST_SERVICE_DATE,to_char(\"DT_LAST_INSP_DATE\",'dd/MM/yyyy')DT_LAST_INSP_DATE from";
                //strQry += " \"TBLDTCMAST\" WHERE \"DT_CODE\" like :dtcode||'%'";
                //dtDtcDetails = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_dtc_details");
                cmd.Parameters.AddWithValue("p_dtcode", (objtcMaster.sDtcCodes ?? ""));
                dtDtcDetails = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtDtcDetails;
            }
            return dtDtcDetails;
        }
        /// <summary>
        /// Get Field Details
        /// </summary>
        /// <param name="objTcMaster"></param>
        /// <returns></returns>
        public DataTable GetFieldDetails(clsTcMaster objTcMaster)
        {
            DataTable dtDetails = new DataTable();
            try
            {
                #region old  GetFieldDetails
                //string Qry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("tccode", Convert.ToString(objTcMaster.sTcCode));
                //Qry = "select \"DT_CODE\",\"DT_NAME\" from \"TBLDTCMAST\",\"TBLTCMASTER\",\"TBLTRANSDTCMAPPING\" where \"TM_DTC_ID\"=\"DT_CODE\"  and \"TM_TC_ID\"=\"TC_CODE\" ";
                //Qry += " and \"TM_LIVE_FLAG\"=1  and \"TC_CODE\"=:tccode ";
                //dtDetails = ObjCon.FetchDataTable(Qry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_field_details");
                cmd.Parameters.AddWithValue("p_tccode", Convert.ToString(objTcMaster.sTcCode ?? ""));
                dtDetails = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtDetails;
            }
            return dtDetails;
        }
        /// <summary>
        /// Save Xml Data
        /// </summary>
        /// <param name="objTcMaster"></param>
        /// <returns></returns>
        public string SaveXmlData(clsTcMaster objTcMaster)
        {
            string sTcXmlData = string.Empty;

            try
            {

                string strQry = string.Empty;
                string strTemp = string.Empty;

                string sPrimaryKey = "{0}";

                objTcMaster.sColumnNames = "TC_ID,TC_CODE,TC_SLNO,TC_MAKE_ID,TC_CAPACITY,TC_MANF_DATE,TC_PURCHASE_DATE,";
                objTcMaster.sColumnNames += "TC_LIFE_SPAN,TC_SUPPLIER_ID,TC_PO_NO,TC_PRICE,TC_WARANTY_PERIOD,TC_LAST_SERVICE_DATE,";
                objTcMaster.sColumnNames += "TC_CURRENT_LOCATION,TC_CRBY,TC_WARRENTY,TC_STORE_ID,TC_LOCATION_ID,TC_RATING,TC_STAR_RATE,TC_OIL_CAPACITY,TC_WEIGHT,TC_IOL_TYPE";
                objTcMaster.sColumnValues = "" + objTcMaster.sTcId + "," + objTcMaster.sTcCode + "," + objTcMaster.sTcSlNo + ",";
                objTcMaster.sColumnValues += "" + objTcMaster.sTcMakeId + "," + objTcMaster.sTcCapacity + "," + objTcMaster.sManufacDate + ",";
                objTcMaster.sColumnValues += "" + objTcMaster.sAllotementDate + "," + objTcMaster.sTcLifeSpan + ",";
                objTcMaster.sColumnValues += "" + objTcMaster.sSupplierId + "," + objTcMaster.sAltNo + "," + objTcMaster.sPrice + ",";
                objTcMaster.sColumnValues += "" + sWarranty + "," + objTcMaster.sLastServiceDate + ",";
                objTcMaster.sColumnValues += "" + objTcMaster.sCurrentLocation + "," + objTcMaster.sCrBy + "," + objTcMaster.sWarrentyPeriod + ",";
                objTcMaster.sColumnValues += "" + sStoreId + "," + objTcMaster.sOfficeCode + "," + objTcMaster.sRating + "," + objTcMaster.sStarRate + ",";
                objTcMaster.sColumnValues += "" + objTcMaster.sOilCapacity + "," + objTcMaster.sWeight + "," + objTcMaster.sOilType + "";

                objTcMaster.sTableNames = "TBLTCMASTER";

                sTcXmlData = CreateXml(objTcMaster.sColumnNames, objTcMaster.sColumnValues, objTcMaster.sTableNames);
                return sTcXmlData;
            }

            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return sTcXmlData;
            }

        }
        /// <summary>
        /// save data in tblefodata and workflowobjects
        /// </summary>
        /// <param name="objTcMaster"></param>
        /// <returns></returns>
        public bool SaveWorkFlowData(clsTcMaster objTcMaster)
        {

            try
            {
                string[] Arr = new string[3]; string strQry = string.Empty;
                objTcMaster.sWFDataId = Convert.ToString(ObjCon.Get_max_no("WFO_ID", "TBLWFODATA"));

                NpgsqlCommand cmd = new NpgsqlCommand("sp_savewfodata");
                cmd.Parameters.AddWithValue("wf_id", objTcMaster.sWFDataId);
                if (objTcMaster.sQryValues == "" || objTcMaster.sQryValues == null)
                {
                    objTcMaster.sQryValues = "sQryValues";
                }
                cmd.Parameters.AddWithValue("wf_qry_vals", objTcMaster.sQryValues);

                if (objTcMaster.sParameterValues == "" || objTcMaster.sParameterValues == null)
                {
                    objTcMaster.sParameterValues = "sparamValues";
                }

                cmd.Parameters.AddWithValue("wf_param", objTcMaster.sParameterValues);

                if (objTcMaster.sXmlData == "" || objTcMaster.sXmlData == null)
                {
                    objTcMaster.sXmlData = "sXmlData";
                }

                cmd.Parameters.AddWithValue("wf_data", objTcMaster.sXmlData);
                cmd.Parameters.AddWithValue("wf_crby", objTcMaster.sCrBy);

                cmd.Parameters.Add("pk_id", NpgsqlDbType.Text);
                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd.Parameters.Add("msg", NpgsqlDbType.Text);

                cmd.Parameters["pk_id"].Direction = ParameterDirection.Output;
                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                cmd.Parameters["msg"].Direction = ParameterDirection.Output;

                Arr[0] = "pk_id";
                Arr[1] = "op_id";
                Arr[2] = "msg";
                Arr = ObjCon.Execute(cmd, Arr, 3);

                if (objTcMaster.sFormName != null && objTcMaster.sFormName != "")
                {
                    #region old inline query
                    //To get Business Object Id
                    //NpgsqlCommand = new NpgsqlCommand();
                    //NpgsqlCommand.Parameters.AddWithValue("formname", objTcMaster.sFormName.Trim().ToUpper());
                    //objTcMaster.sBOId = ObjCon.get_value("SELECT \"BO_ID\" FROM \"TBLBUSINESSOBJECT\" WHERE UPPER(\"BO_FORMNAME\")=:formname", NpgsqlCommand);
                    #endregion

                    //To get Business Object Id
                    string QryKey = string.Empty;
                    QryKey = "GET_BO_ID";
                    NpgsqlCommand cmd_TC_ID_ON_TC_SLNO = new NpgsqlCommand("fetch_getvalue_clstcmaster");
                    cmd_TC_ID_ON_TC_SLNO.Parameters.AddWithValue("p_key", QryKey);
                    cmd_TC_ID_ON_TC_SLNO.Parameters.AddWithValue("p_value_1", Convert.ToString(objTcMaster.sFormName ?? "").Trim().ToUpper());
                    cmd_TC_ID_ON_TC_SLNO.Parameters.AddWithValue("p_value_2", "");
                    cmd_TC_ID_ON_TC_SLNO.Parameters.AddWithValue("p_value_3", "");
                    objTcMaster.sBOId = ObjBseCon.StringGetValue(cmd_TC_ID_ON_TC_SLNO);

                }

                WorkFlowObjects(objTcMaster);

                string sWFlowId = Convert.ToString(ObjCon.Get_max_no("WO_ID", "TBLWORKFLOWOBJECTS"));
                NpgsqlCommand cmd1 = new NpgsqlCommand("sp_saveWFOobjects");
                cmd1.Parameters.AddWithValue("wo_id", sWFlowId);
                cmd1.Parameters.AddWithValue("wo_bo_id", objTcMaster.sBOId);
                cmd1.Parameters.AddWithValue("wo_record_id", objTcMaster.sTcId);
                cmd1.Parameters.AddWithValue("wo_prev_approve_id", "0");
                cmd1.Parameters.AddWithValue("wo_next_role", "0");
                cmd1.Parameters.AddWithValue("wo_office_code", objTcMaster.sOfficeCode);
                cmd1.Parameters.AddWithValue("wo_client_ip", objTcMaster.sClientIP);
                cmd1.Parameters.AddWithValue("wo_cr_by", objTcMaster.sCrBy);
                cmd1.Parameters.AddWithValue("wo_approved_by", objTcMaster.sCrBy);
                cmd1.Parameters.AddWithValue("wo_approve_status", "1");
                cmd1.Parameters.AddWithValue("wo_record_by", "WEB");
                cmd1.Parameters.AddWithValue("wo_description", objTcMaster.sDescription);
                cmd1.Parameters.AddWithValue("wo_user_comment", objTcMaster.sDescription);
                cmd1.Parameters.AddWithValue("wo_wfo_id", objTcMaster.sWFDataId);
                cmd1.Parameters.AddWithValue("wo_initial_id", sWFlowId);
                cmd1.Parameters.AddWithValue("wo_ref_offcode", objTcMaster.sOfficeCode);
                cmd.Parameters["pk_id"].Direction = ParameterDirection.Output;
                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                cmd.Parameters["msg"].Direction = ParameterDirection.Output;

                Arr[0] = "pk_id";
                Arr[1] = "op_id";
                Arr[2] = "msg";
                Arr = ObjCon.Execute(cmd, Arr, 3);
                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }
        /// <summary>
        /// To Get CLient ip
        /// </summary>
        /// <param name="objTcMaster"></param>
        public void WorkFlowObjects(clsTcMaster objTcMaster)
        {
            try
            {
                string sClientIP = string.Empty;

                string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(ip))
                {
                    string[] ipRange = ip.Split(',');
                    int le = ipRange.Length - 1;
                    sClientIP = ipRange[0];
                }
                else
                {
                    sClientIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                objTcMaster.sClientIP = sClientIP;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// creating xml data for Wfo_data insert
        /// </summary>
        /// <param name="strColumns"></param>
        /// <param name="strParameters"></param>
        /// <param name="strTableName"></param>
        /// <returns></returns>
        public string CreateXml(string strColumns, string strParameters, string strTableName)
        {
            try
            {
                DataTable dtXmlContent = new DataTable();

                DataTable dtnew = new DataTable();

                DataSet ds;
                if (strTableName.Contains(";"))
                {
                    ds = new DataSet(strTableName.Split(';').GetValue(0).ToString());
                }
                else
                {
                    ds = new DataSet(strTableName);
                }

                string[] strArrColumns = strColumns.Split(';');
                string[] strArrParameters = strParameters.Split(';');
                string[] strTableNames = strTableName.Split(';');

                int k = 0;
                //DataRow dRow = dt.NewRow();
                for (int i = 0; i < strArrColumns.Length; i++)
                {
                    DataTable dt = new DataTable();
                    DataRow dRow = dt.NewRow();
                    string[] strdtColumns = strArrColumns[i].Split(',');
                    string[] strdtParametres = strArrParameters[i].Split(',');
                    dt.TableName = strTableNames[i];
                    //DataRow dRow1 = dtnew.NewRow();
                    for (int j = 0; j < strdtColumns.Length; j++)
                    {
                        dt.Columns.Add(strdtColumns[j]);
                        if (k < strdtParametres.Length)
                        {
                            string strColumnName = strdtParametres[k];
                            dRow[dt.Columns[j]] = strdtParametres[k];
                            if (dt.Rows.Count == 0)
                            {
                                dt.Rows.Add(dRow);
                            }
                            dt.AcceptChanges();
                            //i--;
                        }
                        k++;

                    }

                    k = 0;

                    ds.Merge(dt);
                    dt.Clear();

                }
                return ds.GetXml();
                //dt.TableName = "Failure and Invoice";
                //////////////////////////////////////////////
                //dt.TableName = "TBLDTCFAILURE";
            }
            catch (Exception ex)
            {
                string strfailure = string.Empty;
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return strfailure;
                //return ds;
            }
        }
        /// <summary>
        /// Get dtr Details
        /// </summary>
        /// <param name="tccodeid"></param>
        /// <returns></returns>
        public clsTcMaster GetdtrDetails(string tccodeid)
        {
            clsTcMaster objTcMaster = new clsTcMaster();
            DataTable dt = new DataTable();
            try
            {
                #region Old GetdtrDetails
                //NpgsqlCommand = new NpgsqlCommand();
                //string strQry = string.Empty;
                //strQry = "select \"TCP_TC_SL_NO\",\"DI_MAKE_ID\",\"DI_CAPACITY_ID\",\"DI_STARTTYPE\",TO_CHAR(\"TCP_MANUFACTURE_DATE\",'DD/MM/YYYY') \"TCP_MANUFACTURE_DATE\",\"TCP_TC_LIFE_SPAN\",\"TCP_TC_WARRENTY_PERIOD\",\"TCP_OIL_TYPE\",\"TCP_OIL_CAPACITY\",\"TCP_OIL_WEIGHT\" from \"TBLDELIVERYINSTMASTER\" inner join ";
                //strQry += " \"TBLDELIVERYINSTRUCTION\" on \"DIM_ID\" = \"DI_DIM_ID\" inner join \"TBLTCRANGEALLOTMENT\" on \"DI_ID\"=\"TCP_DI_ID\"  and \"TCP_TC_CODE\"='" + tccodeid + "' ";
                //dt = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_dtr_details");
                cmd.Parameters.AddWithValue("p_tccodeid", (tccodeid ?? ""));
                dt = ObjCon.FetchDataTable(cmd);

                if (dt.Rows.Count > 0)
                {
                    objTcMaster.sTcSlNo = Convert.ToString(dt.Rows[0]["TCP_TC_SL_NO"]);
                    objTcMaster.sTcMakeId = Convert.ToString(dt.Rows[0]["DI_MAKE_ID"]);
                    objTcMaster.sTcCapacity = Convert.ToString(dt.Rows[0]["DI_CAPACITY_ID"]);
                    objTcMaster.sStarRate = Convert.ToString(dt.Rows[0]["DI_STARTTYPE"]);
                    objTcMaster.sManufacDate = Convert.ToString(dt.Rows[0]["TCP_MANUFACTURE_DATE"]);
                    objTcMaster.sTcLifeSpan = Convert.ToString(dt.Rows[0]["TCP_TC_LIFE_SPAN"]);
                    objTcMaster.sWarrentyPeriod = Convert.ToString(dt.Rows[0]["TCP_TC_WARRENTY_PERIOD"]);
                    objTcMaster.sOilType = Convert.ToString(dt.Rows[0]["TCP_OIL_TYPE"]);
                    objTcMaster.sOilCapacity = Convert.ToString(dt.Rows[0]["TCP_OIL_CAPACITY"]);
                    objTcMaster.sWeight = Convert.ToString(dt.Rows[0]["TCP_OIL_WEIGHT"]);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objTcMaster;
            }
            return objTcMaster;
        }

        #region unused Methods
        //public string GetTransformerCount(string sOfficeCode)
        //{
        //    NpgsqlCommand = new NpgsqlCommand();
        //    try
        //    {
        //        string strQry = string.Empty;
        //        if (sOfficeCode.Length > 1)
        //        {
        //            sOfficeCode = sOfficeCode.Substring(0, Constants.Division);
        //        }
        //        NpgsqlCommand.Parameters.AddWithValue("offcd", sOfficeCode);
        //        strQry = "SELECT COUNT(*) FROM \"TBLTCMASTER\" WHERE \"TC_STORE_ID\" IN (SELECT \"SM_ID\" FROM \"TBLSTOREMAST\" WHERE  \"SM_OFF_CODE\"=:offcd)";
        //        return ObjCon.get_value(strQry, NpgsqlCommand);
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return "";
        //    }
        //}
        //public object GetPoDetails(clsTcMaster objTc)
        //{
        //    NpgsqlCommand = new NpgsqlCommand();
        //    string strQry = string.Empty;
        //    DataTable dtTcDetails = new DataTable();
        //    try
        //    {

        //        NpgsqlCommand.Parameters.AddWithValue("pono", objTc.sAltNo.ToUpper());
        //        strQry = "Select \"PO_ID\",TO_CHAR(\"PO_DATE\",'DD/MM/yyyy') PO_DATE,SUM(\"PB_QUANTITY\") AS PB_QUANTITY,\"PO_SUPPLIER_ID\",\"PO_NO\" ";
        //        strQry += " FROM \"TBLPOMASTER\",\"TBLPOOBJECTS\" WHERE \"PB_PO_ID\"=\"PO_ID\" AND UPPER(\"PO_NO\")=:pono GROUP BY \"PO_ID\",\"PO_DATE\",\"PO_SUPPLIER_ID\",\"PO_NO\" ";
        //        dtTcDetails = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
        //        if (dtTcDetails.Rows.Count > 0)
        //        {
        //            objTc.sAltId = dtTcDetails.Rows[0]["PO_ID"].ToString();
        //            objTc.sAltNo = dtTcDetails.Rows[0]["PO_NO"].ToString();
        //            objTc.sAllotementDate = dtTcDetails.Rows[0]["PO_DATE"].ToString();
        //            objTc.sQuantity = dtTcDetails.Rows[0]["PB_QUANTITY"].ToString();
        //            objTc.sSupplierId = dtTcDetails.Rows[0]["PO_SUPPLIER_ID"].ToString();
        //        }
        //        return objTc;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return dtTcDetails;
        //    }
        //}
        //public object GetDeliveryDetails(clsTcMaster objTc)
        //{

        //    NpgsqlCommand = new NpgsqlCommand();
        //    string strQry = string.Empty;
        //    DataTable dtTcDetails = new DataTable();
        //    try
        //    {
        //        NpgsqlCommand.Parameters.AddWithValue("pono", objTc.sAltNo.ToUpper());
        //        strQry = "SELECT \"DI_ID\", TO_CHAR(\"DI_DATE\", 'DD/MM/yyyy') \"DI_DATE\", SUM(\"DI_QUANTITY\") AS \"DI_QUANTITY\",\"PO_SUPPLIER_ID\", ";
        //        strQry += " \"DI_NO\"  FROM \"TBLPOMASTER\", \"TBLTRANSSUPPLIER\", \"TBLDELIVERYINSTRUCTION\" WHERE \"PO_ID\" = \"DI_PO_ID\" AND ";
        //        strQry += " \"PO_SUPPLIER_ID\" = \"TS_ID\" AND UPPER(REPLACE(\"DI_NO\",'\\','')) =:pono GROUP BY \"DI_ID\", \"DI_DATE\", \"PO_SUPPLIER_ID\", \"DI_NO\" ";
        //        dtTcDetails = ObjCon.FetchDataTable(strQry, NpgsqlCommand);

        //        if (dtTcDetails.Rows.Count > 0)
        //        {
        //            objTc.sAltId = dtTcDetails.Rows[0]["DI_ID"].ToString();
        //            objTc.sAltNo = dtTcDetails.Rows[0]["DI_NO"].ToString();
        //            objTc.sAllotementDate = dtTcDetails.Rows[0]["DI_DATE"].ToString();
        //            objTc.sQuantity = dtTcDetails.Rows[0]["DI_QUANTITY"].ToString();
        //            objTc.sSupplierId = dtTcDetails.Rows[0]["PO_SUPPLIER_ID"].ToString();
        //        }
        //        return objTc;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return dtTcDetails;
        //    }
        //}
        #endregion
    }
}



