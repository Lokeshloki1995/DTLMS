using System;
using System.Text;
using IIITS.PGSQL.DAL;
using System.Data;
using Npgsql;
using System.Configuration;
using NpgsqlTypes;
using System.Reflection;

namespace IIITS.DTLMS.BL.POFlow
{
    public class clsPMCAllotment
    {
        public string sTotalTC { get; set; }
        public string sCapacity { get; set; }
        public string sMakeId { get; set; }
        public string sDiId { get; set; }
        public string sDINo { get; set; }
        public string sCrby { get; set; }
        public string sDimid { get; set; }
        public string sDino { get; set; }
        public string sStorename { get; set; }
        public string sRating { get; set; }
        public string sTotqty { get; set; }
        public string sStartrange { get; set; }
        public string sEndrange { get; set; }
        public string sMake { get; set; }
        public string sPoid { get; set; }
        public string sDimDino { get; set; }
        public string sfilename { get; set; }
        public string sUserId { get; set; }
        public string sofficecode { get; set; }
        public string sFilepath { get; set; }
        public string sDimno { get; set; }
        public Int16 statusId { get; set; }
        public Int16 Tcamountstatus { get; set; }

        public string Ponumber { get; set; }
        public string Poamount { get; set; }
        public string Dwavalidupto { get; set; }
        public string Lecvalidupto { get; set; }
        public string Poavailableamount { get; set; }
        public StringBuilder Validation { set; get; }

        public string LecNumber { get; set; }
        public string DwaNumber { get; set; }
        public string Lecstatus { get; set; }
        public string Dwastatus { get; set; }


        PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
        NpgsqlCommand NpgsqlCommand;
        /// <summary>
        /// to get a dispatch count 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public object GetDispatchCount(clsPMCAllotment obj)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string Qty = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("sp_get_pmc_detailsforallotment");
                cmd.Parameters.AddWithValue("di_no", obj.sDINo);

                dt = objcon.FetchDataTable(cmd);

                if (dt.Rows.Count > 0)
                {
                    obj.sTotalTC = Convert.ToString(dt.Rows[0]["quantity"]);
                    obj.sDimid = Convert.ToString(dt.Rows[0]["pmc_dim_id"]);
                    obj.Ponumber = Convert.ToString(dt.Rows[0]["pmc_po_no"]);
                    obj.Poamount = Convert.ToString(dt.Rows[0]["pmc_po_amount"]);
                    obj.Dwavalidupto = Convert.ToString(dt.Rows[0]["dm_extended_upto"]);
                    obj.Lecvalidupto = Convert.ToString(dt.Rows[0]["lm_valid_upto"]);
                    obj.LecNumber = Convert.ToString(dt.Rows[0]["lm_contractor_name"]);
                    obj.DwaNumber = Convert.ToString(dt.Rows[0]["dm_number"]);
                    obj.Lecstatus = Convert.ToString(dt.Rows[0]["lm_status"]);
                    obj.Dwastatus = Convert.ToString(dt.Rows[0]["dm_status"]);
                }

                NpgsqlCommand cmdamt = new NpgsqlCommand("sp_get_pmcallotment_availableamt");
                cmdamt.Parameters.AddWithValue("po_no", obj.Ponumber);
                cmdamt.Parameters.AddWithValue("po_amt", obj.Poamount);
                DataTable dtAvailableamt = objcon.FetchDataTable(cmdamt);
                if (dtAvailableamt.Rows.Count > 0)
                {
                    double sum = Convert.ToDouble(dtAvailableamt.Rows[0]["pda_po_available_amt"]);
                    sum = Math.Round(sum, 2);
                    obj.Poavailableamount = Convert.ToString(sum);
                }
                return obj;
            }
            catch (Exception ex)
            {
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "0";
            }
        }


        /// <summary>
        ///  to get a delevery details for pmc allotment
        /// </summary>
        /// <param name="objAllot"></param>
        /// <returns></returns>
        public DataTable GetPMCDeliveryDetails(clsPMCAllotment objAllot)
        {
            NpgsqlCommand = new NpgsqlCommand();
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getpending_pmcallotment");
                cmd.Parameters.AddWithValue("di_no", objAllot.sDINo);

                dt = objcon.FetchDataTable(cmd);

                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        /// <summary>
        /// to get delevery details for allotment
        /// </summary>
        /// <param name="objAllot"></param>
        /// <returns></returns>
        public DataTable GetDeliveryViewDetails(clsPMCAllotment objAllot)
        {
            NpgsqlCommand = new NpgsqlCommand();
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("sp_get_view_allotement");
                cmd.Parameters.AddWithValue("di_no", objAllot.sDINo == null ? "" : objAllot.sDINo);

                dt = objcon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable PrintDi(clsPMCAllotment objDi)
        {
            DataTable dt = new DataTable();
            DataTable diDetails = new DataTable();
            string strQry = string.Empty;
            int start = Convert.ToInt32(objDi.sStartrange);
            int End = Convert.ToInt32(objDi.sEndrange);

            try
            {
                strQry = "select \"PMC_PO_NO\" from \"TBLPMC_POMASTER\"  WHERE \"PMC_PO_ID\"='" + objDi.sPoid + "' ";
                string pono = objcon.get_value(strQry);

                dt.Columns.Add("DI_id");
                dt.Columns.Add("DI_DTRCODE");
                dt.Columns.Add("DI_PONO");
                dt.Columns.Add("DIM_DI_NO");
                dt.Columns.Add("DI_MAKE_ID");
                dt.Columns.Add("MAKE_NAME");
                dt.Columns.Add("STORE_NAME");
                dt.Columns.Add("DI_CAPACITY");
                dt.Columns.Add("STAR_RATE");
                dt.Columns.Add("DI_SLNO");
                dt.Columns.Add("TC_MANUFACTURE_DATE");
                dt.Columns.Add("TC_LIFE_SPAN");
                dt.Columns.Add("TC_WARRENTY_PERIOD");
                dt.Columns.Add("TC_OIL_TYPE");
                dt.Columns.Add("TC_OIL_CAPACITY");
                dt.Columns.Add("TC_OIL_WEIGHT");
                dt.Columns.Add("TC_AMOUNT");
                for (int j = start; j <= End; j++)
                {
                    DataRow dRow = dt.NewRow();
                    dRow["DI_id"] = objDi.sDino;
                    string strtrng = Convert.ToString(start);
                    if (strtrng.Length < 6)
                    {
                        switch (strtrng.Length)
                        {
                            case 5:
                                strtrng = "0" + strtrng;
                                break;
                            case 4:
                                strtrng = "00" + strtrng;
                                break;
                            case 3:
                                strtrng = "000" + strtrng;
                                break;
                            case 2:
                                strtrng = "0000" + strtrng;
                                break;
                            case 1:
                                strtrng = "00000" + strtrng;
                                break;
                        }
                    }
                    dRow["DI_DTRCODE"] = "HP" + strtrng;
                    dRow["DI_PONO"] = pono;
                    dRow["DIM_DI_NO"] = objDi.sDimno;
                    dRow["DI_MAKE_ID"] = objDi.sMakeId;
                    dRow["MAKE_NAME"] = objDi.sMake;
                    dRow["STORE_NAME"] = objDi.sStorename;
                    dRow["DI_CAPACITY"] = objDi.sCapacity;
                    dRow["STAR_RATE"] = objDi.sRating;
                    dRow["DI_SLNO"] = "";
                    dRow["TC_MANUFACTURE_DATE"] = "";
                    dRow["TC_LIFE_SPAN"] = "";
                    dRow["TC_WARRENTY_PERIOD"] = "";
                    dRow["TC_OIL_TYPE"] = "";
                    dRow["TC_OIL_CAPACITY"] = "";
                    dRow["TC_OIL_WEIGHT"] = "";
                    dRow["TC_AMOUNT"] = "";
                    dt.Rows.Add(dRow);
                    dt.AcceptChanges();
                    start++;
                }
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        /// <summary>
        /// This method used to fetch the allocation details to bind in view page for grid
        /// </summary>
        /// <param name="objDi"></param>
        /// <returns></returns>
        public DataTable LoadALlotmentdetails(clsPMCAllotment objDi)
        {
            DataTable dt = new DataTable();
            DataTable diDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("sp_get_pmc_viewallotment_details");
                cmd.Parameters.AddWithValue("di_no", objDi.sDino);
                cmd.Parameters.AddWithValue("start_range", objDi.sStartrange);
                cmd.Parameters.AddWithValue("end_range", objDi.sEndrange);


                dt = objcon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        /// <summary>
        /// This method used to save the pmc allotment details 
        /// Records will be inserted in TBLPMC_DTR_ALLOCATION AND TBLPMC_DTR_RANGE_ALLOCATION
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sPath"></param>
        /// <param name="objDtrUpload"></param>
        /// <returns></returns>
        public bool SaveAllotmentUploadDetails(DataTable dt, string sPath, clsPMCAllotment objDtrUpload)
        {
            DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);

            NpgsqlCommand = new NpgsqlCommand();

            DataTable dtnew = new DataTable();
            bool result = false;
            string strQry = string.Empty;
            string[] ArrAllotment = new string[3];
            string[] Arr = new string[3];
            try
            {
                objDatabse.BeginTransaction();

                double RemainingAmount = 0;
                double sum = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    sum += Convert.ToDouble(dr["DTr Amount"]);
                }
                sum = Math.Round(sum, 2);

                #region
                //string srange = objcon.get_value("SELECT \"PMC_DI_TC_START_RANGE_VAR\" FROM \"TBLPMC_DELIVERYINSTRUCTION\" WHERE \"PMC_DI_ID\" = '" + Convert.ToInt32(dt.Rows[0]["DI ID"]) + "'");
                //string erange = objcon.get_value("SELECT \"PMC_DI_TC_END_RANGE_VAR\"  FROM \"TBLPMC_DELIVERYINSTRUCTION\" WHERE \"PMC_DI_ID\" = '" + Convert.ToInt32(dt.Rows[0]["DI ID"]) + "'");

                //string ExistingAllotedamt = objcon.get_value("SELECT \"PDA_ALLOCATION_AMT\" FROM \"TBLPMC_DTR_ALLOCATION\" WHERE UPPER(\"PDA_DI_NO\") = '"
                //      + Convert.ToString(dt.Rows[0]["DI No"]) + "' AND \"PDA_PO_NO\" ='" + Convert.ToString(dt.Rows[0]["DI Po No"])
                //      + "' AND  \"PDA_START_RANGE\"='HP" + srange + "' AND \"PDA_END_RANGE\" = 'HP" + erange + "' ORDER BY \"PDA_ID\" DESC LIMIT 1");

                #endregion
                NpgsqlCommand cmdexistingamt = new NpgsqlCommand("proc_get_existingallocatedamt");
                cmdexistingamt.Parameters.AddWithValue("di_id", Convert.ToInt32(dt.Rows[0]["DI ID"]));
                cmdexistingamt.Parameters.AddWithValue("di_no", Convert.ToString(dt.Rows[0]["DI No"]));
                cmdexistingamt.Parameters.AddWithValue("di_po_no", Convert.ToString(dt.Rows[0]["DI Po No"]));
                DataTable dtExistamt = objDatabse.FetchDataTable(cmdexistingamt);

                if (dtExistamt.Rows.Count > 0)
                {
                    double ExistingAllotedamt = Convert.ToDouble(dtExistamt.Rows[0]["pda_allocation_amt"]);
                    double Afteradd = Convert.ToDouble(objDtrUpload.Poavailableamount) + ExistingAllotedamt;
                    RemainingAmount = Afteradd - sum;
                }
                #region
                //    if ((ExistingAllotedamt ?? "").Length > 0)
                //{
                //    double Afteradd = Convert.ToDouble(objDtrUpload.Poavailableamount) + Convert.ToDouble(ExistingAllotedamt);
                //    RemainingAmount = Afteradd - sum;
                //}
                #endregion
                else
                {
                    RemainingAmount = Convert.ToDouble(objDtrUpload.Poavailableamount) - sum;
                }


                NpgsqlCommand cmdallotment = new NpgsqlCommand("proc_save_pmcallotmentdetails");
                cmdallotment.Parameters.AddWithValue("di_id", Convert.ToInt32(dt.Rows[0]["DI ID"]));
                cmdallotment.Parameters.AddWithValue("di_no", Convert.ToString(dt.Rows[0]["DI No"]));
                cmdallotment.Parameters.AddWithValue("di_po_no", Convert.ToString(dt.Rows[0]["DI Po No"]));
                cmdallotment.Parameters.AddWithValue("total_qty", Convert.ToString(objDtrUpload.sTotqty));
                cmdallotment.Parameters.AddWithValue("po_amt", Convert.ToString(objDtrUpload.Poamount));
                cmdallotment.Parameters.AddWithValue("sum_amt", Convert.ToString(sum));
                cmdallotment.Parameters.AddWithValue("remaining_amt", Convert.ToString(RemainingAmount));



                cmdallotment.Parameters.Add("msg", NpgsqlDbType.Text);
                cmdallotment.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmdallotment.Parameters.Add("pk_id", NpgsqlDbType.Text);
                cmdallotment.Parameters["msg"].Direction = ParameterDirection.Output;
                cmdallotment.Parameters["op_id"].Direction = ParameterDirection.Output;
                cmdallotment.Parameters["pk_id"].Direction = ParameterDirection.Output;
                ArrAllotment[2] = "pk_id";
                ArrAllotment[1] = "op_id";
                ArrAllotment[0] = "msg";
                ArrAllotment = objDatabse.Execute(cmdallotment, ArrAllotment, 3);

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string sDim_Id = Convert.ToString(dt.Rows[i]["DI ID"]);
                    string sDtrcode = Convert.ToString(dt.Rows[i]["DTR Code"]);
                    string sSlNo = Convert.ToString(dt.Rows[i]["Sl No"]);

                    string sPoNo = Convert.ToString(dt.Rows[i]["DI Po No"]);
                    string sDino = Convert.ToString(dt.Rows[i]["DI No"]);
                    string sMakeid = Convert.ToString(dt.Rows[i]["Make ID"]);
                    string sMakename = Convert.ToString(dt.Rows[i]["Make Name"]);
                    string sStorename = Convert.ToString(dt.Rows[i]["Store Name"]);
                    string sCapacity = Convert.ToString(dt.Rows[i]["Capacity"]);
                    string sStarrate = Convert.ToString(dt.Rows[i]["Star Rate"]);
                    string sManufacturedate = Convert.ToString(dt.Rows[i]["Manufacture Date"]);
                    string sLifespan = Convert.ToString(dt.Rows[i]["Life Span"]);
                    string sWarrenty = Convert.ToString(dt.Rows[i]["Warrenty Period"]);
                    string sOiltype = Convert.ToString(dt.Rows[i]["Oil Type"]);

                    string sOilcap = Convert.ToString(dt.Rows[i]["Tc Oil Capacity"]);
                    string sOilweight = Convert.ToString(dt.Rows[i]["Tc Weight"]);
                    string sDtrAmount = Convert.ToString(dt.Rows[i]["DTr Amount"]);

                    string sStatus = "0";

                    clsException.WriteLogFile("clsAllotement", "SaveAllotmentUploadDetails", sManufacturedate, "before");
                    if (sManufacturedate != "")
                    {
                        string Flag = "1";
                        if (Flag == ConfigurationManager.AppSettings["PMCAllotmentLiveFlag"])
                        {
                            DateTime stoDate = DateTime.ParseExact(sManufacturedate, "M/d/yyyy h:m:s tt",
                                System.Globalization.CultureInfo.InvariantCulture);
                            sManufacturedate = Convert.ToDateTime(stoDate).ToString("d/M/yyyy");
                        }
                        else
                        {
                            DateTime source = Convert.ToDateTime(sManufacturedate);
                            sManufacturedate = source.ToString("yyyy-MM-dd");
                        }
                        clsException.WriteLogFile("clsPMCAllotement", "SaveAllotmentUploadDetails", sManufacturedate, "After");

                    }


                    if (sOiltype.Trim().ToUpper() == "MINERAL")
                    {
                        sOiltype = "1";
                    }
                    if (sOiltype.Trim().ToUpper() == "ESTER")
                    {
                        sOiltype = "2";
                    }
                    NpgsqlCommand cmd = new NpgsqlCommand("proc_save_pmcallotmentdetailsfromexcel");
                    cmd.Parameters.AddWithValue("tc_code", Convert.ToString(sDtrcode));
                    cmd.Parameters.AddWithValue("di_id", Convert.ToString(sDim_Id));
                    cmd.Parameters.AddWithValue("tc_slno", Convert.ToString(sSlNo == "" ? "0" : sSlNo));
                    cmd.Parameters.AddWithValue("manufacture_date", Convert.ToString(sManufacturedate));
                    cmd.Parameters.AddWithValue("tc_life_span", Convert.ToString(sLifespan == "" ? "0" : sLifespan));
                    cmd.Parameters.AddWithValue("tc_warrenty_period", Convert.ToString(sWarrenty == "" ? "0" : sWarrenty));
                    cmd.Parameters.AddWithValue("oil_type", Convert.ToString(sOiltype == "" ? "0" : sOiltype));
                    cmd.Parameters.AddWithValue("oil_capacity", Convert.ToString(sOilcap == "" ? "0" : sOilcap));
                    cmd.Parameters.AddWithValue("oil_weight", Convert.ToString(sOilweight == "" ? "0" : sOilweight));
                    cmd.Parameters.AddWithValue("status", Convert.ToString(sStatus));
                    cmd.Parameters.AddWithValue("excel_path", Convert.ToString(sPath));
                    cmd.Parameters.AddWithValue("tc_amount", Convert.ToString(sDtrAmount));
                    cmd.Parameters.AddWithValue("max_pda_id", Convert.ToString(ArrAllotment[2]));


                    cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd.Parameters.Add("pk_id", NpgsqlDbType.Text);
                    cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                    cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                    cmd.Parameters["pk_id"].Direction = ParameterDirection.Output;
                    Arr[2] = "pk_id";
                    Arr[1] = "op_id";
                    Arr[0] = "msg";
                    Arr = objDatabse.Execute(cmd, Arr, 3);
                }
                if (Arr[1] == "0")
                {
                    objDatabse.CommitTransaction();
                    return result = true;
                }
                else
                {
                    objDatabse.RollBackTrans();
                }
                return result;
            }
            catch (Exception ex)
            {
                objDatabse.RollBackTrans();
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return result;
            }
        }
        /// <summary>
        /// This method used to check the validations for excel sheet
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="diid"></param>
        /// <param name="dino"></param>
        /// <param name="PoAvailableamt"></param>
        /// <returns></returns>
        public clsPMCAllotment ValidateExcelSheet(DataTable dt, string diid, string dino, double PoAvailableamt)
        {
            clsPMCAllotment objDIS = new clsPMCAllotment();
            try
            {
                int iRow = 2;
                string strQry = string.Empty;
                objDIS.Validation = new StringBuilder();

                //string QryExistamt = "select \"PDA_ALLOCATION_AMT\" from \"TBLPMC_DTR_ALLOCATION\" inner join  ";
                //QryExistamt += " \"TBLPMC_DTR_RANGE_ALLOCATION\" on \"PDA_ID\" = \"PDRA_PDA_ID\"  inner join ";
                //QryExistamt += " \"TBLPMC_DELIVERYINSTRUCTION\" on \"PDRA_DI_ID\" = \"PMC_DI_ID\" ";
                //QryExistamt += " where \"PMC_DI_ID\"='" + diid + "' ";
                //string Exist_Allotedamt = objcon.get_value(QryExistamt);


                NpgsqlCommand cmdexistingamt = new NpgsqlCommand("proc_get_existingallocatedamt");
                cmdexistingamt.Parameters.AddWithValue("di_id", Convert.ToInt32(diid));
                cmdexistingamt.Parameters.AddWithValue("di_no", "");
                cmdexistingamt.Parameters.AddWithValue("di_po_no","");
                DataTable dtExistamt = objcon.FetchDataTable(cmdexistingamt);

                if (dtExistamt.Rows.Count > 0)
                {
                    double Exist_Allotedamt = Convert.ToDouble(dtExistamt.Rows[0]["pda_allocation_amt"]);
               
                    PoAvailableamt = PoAvailableamt + Exist_Allotedamt;
                }


                //if ((Exist_Allotedamt ?? "").Length > 0)
                //{
                //    PoAvailableamt = PoAvailableamt + Convert.ToDouble(Exist_Allotedamt);
                //}
                double sum = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    sum += Convert.ToDouble(dr["DTr Amount"]);
                }
                sum = Math.Round(sum, 2);
                if (sum > PoAvailableamt)
                {
                    objDIS.Tcamountstatus = -1;
                    return objDIS;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sResult = "";
                    string sDim_Id = Convert.ToString(dt.Rows[i]["DI ID"]);
                    string sDtrcode = Convert.ToString(dt.Rows[i]["DTR Code"]);
                    string sSlNo = Convert.ToString(dt.Rows[i]["Sl No"]);
                    string sPoNo = Convert.ToString(dt.Rows[i]["DI Po No"]);
                    string sDino = Convert.ToString(dt.Rows[i]["DI No"]);
                    string sMakeid = Convert.ToString(dt.Rows[i]["Make ID"]);
                    string sMakename = Convert.ToString(dt.Rows[i]["Make Name"]);
                    string sStorename = Convert.ToString(dt.Rows[i]["Store Name"]);
                    string sCapacity = Convert.ToString(dt.Rows[i]["Capacity"]);
                    string sStarrate = Convert.ToString(dt.Rows[i]["Star Rate"]);
                    string sManufacture = Convert.ToString(dt.Rows[i]["Manufacture Date"]);
                    string sLifespan = Convert.ToString(dt.Rows[i]["Life Span"]);
                    string sWarrenty = Convert.ToString(dt.Rows[i]["Warrenty Period"]);
                    string sOiltype = Convert.ToString(dt.Rows[i]["Oil Type"]);
                    string sOilcap = Convert.ToString(dt.Rows[i]["Tc Oil Capacity"]);
                    string sOilweight = Convert.ToString(dt.Rows[i]["Tc Weight"]);
                    string sDtrAmount = Convert.ToString(dt.Rows[i]["DTr Amount"]);

                    string sStart_Range = string.Empty;
                    string sEnd_Range = string.Empty;
                    string sSQry = string.Empty;
                    string sEQry = string.Empty;
                    sSQry = "SELECT \"PMC_DI_TC_START_RANGE_VAR\" FROM \"TBLPMC_DELIVERYINSTRUCTION\" WHERE \"PMC_DI_ID\" ='" + diid + "'";
                    sStart_Range = objcon.get_value(sSQry);
                    sEQry = "SELECT \"PMC_DI_TC_END_RANGE_VAR\" FROM \"TBLPMC_DELIVERYINSTRUCTION\" WHERE \"PMC_DI_ID\" ='" + diid + "'";
                    sEnd_Range = objcon.get_value(sEQry);
                    string tdstartcode = sDtrcode.Substring(2, 6);
                    if (Convert.ToInt32(tdstartcode) >= Convert.ToInt32(sStart_Range) && 
                        Convert.ToInt32(tdstartcode) <= Convert.ToInt32(sEnd_Range))
                    {
                        strQry = "select \"PDRA_ID\" from \"TBLPMC_DTR_RANGE_ALLOCATION\"  WHERE \"PDRA_TC_SL_NO\"='"
                            + sSlNo + "' and \"PDRA_TC_CODE\"<>'" + sDtrcode + "' ";
                        string Temp = objcon.get_value(strQry);
                        if (sDtrcode != "" && sDtrcode != null)
                        {
                            if (sDim_Id == "" || sDim_Id == null)
                            {
                                sResult += "DI ID : " + iRow + " ,DI ID  should not be empty ||";
                            }
                            if (sDtrcode == "" || sDtrcode == null)
                            {
                                sResult += "Tc Code : " + iRow + " ,Tc Code  should not be empty ||";
                            }
                            if (sDtrcode.Trim().Length != 8)
                            {
                                sResult += "Tc Code : " + iRow + " ,Tc Code Length Should Be 7 ||";
                            }
                            if (!sDtrcode.StartsWith("H"))
                            {
                                sResult += "Tc Code : " + iRow + " ,Tc Code Length Should Be Start With HP ||";
                            }
                            if (sSlNo == "" || sSlNo == null)
                            {
                                sResult += "Sl No : " + iRow + " , Sl No  should not be empty ||";
                            }
                            if (Temp != "")
                            {
                                sResult += "Sl No : " + iRow + " , Sl No  Already Exist ||";
                            }
                            if (sLifespan == "" || sLifespan == null)
                            {
                                sResult += "Life Span : " + iRow + " ,  Life Span  should not be empty ||";
                            }
                            if (sWarrenty == "" || sWarrenty == null)
                            {
                                sResult += "Warrenty Period : " + iRow + " , Warrenty Period  should not be empty||";
                            }
                            if (sOiltype == "" || sOiltype == null)
                            {
                                sResult += "Oiltype: " + iRow + " , Oiltype  should not be empty||";
                            }
                            if (sOiltype.Trim().ToUpper() != "MINERAL" && sOiltype.Trim().ToUpper() != "ESTER")
                            {
                                sResult += "Oiltype: " + iRow + " ,Oiltype Should Be MINERAL OR ESTER ||";
                            }
                            if (sOilcap == "" || sOilcap == null)
                            {
                                sResult += "Oil Capacity: " + iRow + " , Oil Capacity  should not be empty||";
                            }
                            if (sOilweight == "" || sOilweight == null)
                            {
                                sResult += "Oilweight: " + iRow + " , Oilweight  should not be empty||";
                            }
                            if (sDtrAmount == "" || sDtrAmount == null)
                            {
                                sResult += "DtrAmount: " + iRow + " , DtrAmount  should not be empty||";
                            }
                            if (sResult != "")
                            {
                                objDIS.statusId = -6;
                                objDIS.Validation.Append(sResult);
                                objDIS.Validation.AppendLine();
                            }
                        }
                        else
                        {
                            sResult += "DTr : " + iRow + " ,Dtr Code should not be empty";
                            if (sResult != "")
                            {
                                objDIS.statusId = -6;
                                objDIS.Validation.Append(sResult);
                                objDIS.Validation.AppendLine();
                            }
                        }
                        iRow++;
                    }
                    else
                    {
                        sResult += "DTr : " + iRow + " ,Dtr Code Should be in DI Start and End Range";
                        if (sResult != "")
                        {
                            objDIS.statusId = -6;
                            objDIS.Validation.Append(sResult);
                            objDIS.Validation.AppendLine();
                        }
                    }
                }
                return objDIS;
            }
            catch (Exception ex)
            {
                objDIS.statusId = 1;
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objDIS;
            }
        }
        /// <summary>
        /// This method used to check the indent status for particular dtr 
        /// </summary>
        /// <param name="objCheckIndentstatus"></param>
        /// <returns></returns>
        public DataTable CheckIndentstatus(clsPMCAllotment objCheckIndentstatus)
        {
            NpgsqlCommand = new NpgsqlCommand();
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getindentdone_pmcallotment");
                cmd.Parameters.AddWithValue("srange", objCheckIndentstatus.sStartrange);
                cmd.Parameters.AddWithValue("erange", objCheckIndentstatus.sEndrange);
                cmd.Parameters.AddWithValue("di_id", objCheckIndentstatus.sDiId == null ?
                    '0' : Convert.ToInt32(objCheckIndentstatus.sDiId));


                dt = objcon.FetchDataTable(cmd);

                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
    }
}
