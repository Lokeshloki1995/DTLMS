using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIITS.DTLMS.BL.MasterForms
{
    public class ClsWorkOrderRangeAllocation
    {
        public string offcode { get; set; }
        public string Divcode { get; set; }
        public string Accounthead { get; set; }
        public string Allocationdate { get; set; }
        public string Workorderstartrange { get; set; }
        public string Financialyear { get; set; }
        public string Description { get; set; }
        public string Quantity { get; set; }
        public string Workorderendrange { get; set; }
        public string Startrangenumber { get; set; }
        public string Endrangenumber { get; set; }
        public string StartrangeAlphabet { get; set; }
        public string EndrangeAlphabet { get; set; }
        public string sboid { get; set; }
        public string Crby { get; set; }
        public string Comm_Decomm_flag { get; set; }
        public string sClientIP { get; set; }
        public string sRecordId { get; set; }
        public string sWFObjectId { get; set; }
        public string sWFAutoId { get; set; }
        public string sWFDataId { get; set; }
        public string sActionType { get; set; }
        PGSqlConnection objCon = new PGSqlConnection(Constants.Password);

        /// <summary>
        /// to fetch work order range allocated details
        /// </summary>
        /// <param name="objwora"></param>
        /// <returns></returns>
        public DataTable LoadWOGrid(ClsWorkOrderRangeAllocation objwora)
        {
            DataTable dtWODetails = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_workorder_range_allocated_details");
                cmd.Parameters.AddWithValue("sOffCode", objwora.offcode == null ? "" : objwora.offcode);
                dtWODetails = objCon.FetchDataTable(cmd);
                return dtWODetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtWODetails;
            }
        }
        /// <summary>
        /// to get current financial year
        /// </summary>
        /// <returns></returns>
        public string GetFinancialYearWO()
        {
            string year = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("get_financialyear_forwo");
                dt = objCon.FetchDataTable(cmd);

                if (dt.Rows.Count > 0)
                {
                    year = Convert.ToString(dt.Rows[0]["rt_financialyear"]);
                }
                return year;
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return year;
            }
        }
        /// <summary>
        /// to get account head description
        /// </summary>
        /// <param name="accounthead"></param>
        /// <returns></returns>
        public string GetAccountHeadDescription(string accounthead)
        {
            string AccDesc = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("get_accountheaddetails");
                cmd.Parameters.AddWithValue("acchead", accounthead);
                dt = objCon.FetchDataTable(cmd);
                if (dt.Rows.Count > 0)
                {
                    AccDesc = Convert.ToString(dt.Rows[0]["MD_NAME"]);
                }
                return AccDesc;
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return AccDesc;
            }
        }
        /// <summary>
        ///  to get the commissioning or decommissiong value 
        /// </summary>
        /// <param name="accounthead"></param>
        /// <returns></returns>
        public string getCommDecommStatus(string accounthead)
        {
            string id = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("get_comm_decomm_status");
                cmd.Parameters.AddWithValue("acchead", accounthead);
                dt = objCon.FetchDataTable(cmd);
                if (dt.Rows.Count > 0)
                {
                    id = Convert.ToString(dt.Rows[0]["COMM_DECOMM_ID"]);
                }
                return id;
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return id;
            }
        }
        /// <summary>
        /// to get the id for commissioning and decommissioning
        /// </summary>
        /// <param name="accounthead"></param>
        /// <param name="diccode"></param>
        /// <param name="financialyear"></param>
        /// <returns></returns>
        public bool checkCommDecommID(string accounthead, string diccode, string financialyear)
        {
            bool flag = true;
            string id = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("check_comm_decommid");
                cmd.Parameters.AddWithValue("acchead", accounthead);
                cmd.Parameters.AddWithValue("divcode", diccode);
                cmd.Parameters.AddWithValue("finyear", financialyear);
                dt = objCon.FetchDataTable(cmd);
                if (dt.Rows.Count > 0)
                {
                    id = Convert.ToString(dt.Rows[0]["WRA_COMM_DECOMM_FLAG"]);
                    if (id == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["WOCOMMID"]))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                return flag;
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return flag;
            }
        }
        /// <summary>
        /// to get the id for commissioning and decommissioning
        /// </summary>
        /// <param name="accounthead"></param>
        /// <param name="diccode"></param>
        /// <param name="financialyear"></param>
        /// <returns></returns>
        public string[] GetWOSeries(string accounthead, string diccode, string financialyear)
        {
            string[] Arr = new string[3];
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("get_wo_start_range");
                cmd.Parameters.AddWithValue("acchead", accounthead);
                cmd.Parameters.AddWithValue("divcode", diccode);
                cmd.Parameters.AddWithValue("finyear", financialyear);
                dt = objCon.FetchDataTable(cmd);
                if (dt.Rows.Count > 0)
                {
                    Arr[0] = Convert.ToString(dt.Rows[0]["WRA_WO_SERIES"]);
                    Arr[1] = Convert.ToString(dt.Rows[0]["WRA_ID"]);
                }
                return Arr;
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }
        /// <summary>
        /// save method to save the details for work order range allocation
        /// </summary>
        /// <param name="Objworangeallocation"></param>
        /// <returns></returns>
        public string[] SaveWorkorderallocationdetails(ClsWorkOrderRangeAllocation Objworangeallocation)
        {
            string[] Arr = new string[3];
            string[] Arrmsg = new string[3];
            bool bResult = false;
            string Qry = string.Empty;
            try
            {
                if (Objworangeallocation.sActionType == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ActionTypeApprove"])
                    || Objworangeallocation.sActionType == "")
                {

                    NpgsqlCommand cmdworkorderexist = new NpgsqlCommand("workorder_checkduplicate_bulk");
                    cmdworkorderexist.Parameters.AddWithValue("startrange", Objworangeallocation.Startrangenumber);
                    cmdworkorderexist.Parameters.AddWithValue("endrange", Objworangeallocation.Endrangenumber);
                    cmdworkorderexist.Parameters.AddWithValue("wo_startrange", Objworangeallocation.Workorderstartrange);
                    cmdworkorderexist.Parameters.AddWithValue("wo_endrange", Objworangeallocation.Workorderendrange);
                    cmdworkorderexist.Parameters.AddWithValue("divcode", Convert.ToInt32(Objworangeallocation.Divcode));
                    cmdworkorderexist.Parameters.AddWithValue("financialyear", Objworangeallocation.Financialyear);
                    cmdworkorderexist.Parameters.AddWithValue("accounthead", Objworangeallocation.Accounthead);
                    cmdworkorderexist.Parameters.AddWithValue("wo_no_alphabet", Objworangeallocation.StartrangeAlphabet);
                    cmdworkorderexist.Parameters.Add("wo_no", NpgsqlDbType.Text);
                    cmdworkorderexist.Parameters.Add("status", NpgsqlDbType.Text);
                    cmdworkorderexist.Parameters["status"].Direction = ParameterDirection.Output;
                    cmdworkorderexist.Parameters["wo_no"].Direction = ParameterDirection.Output;

                    Arr[0] = "wo_no";
                    Arr[1] = "status";
                    Arr = objCon.Execute(cmdworkorderexist, Arr, 2);
                    if (Arr[1] == "-1")
                    {
                        Arr[0] = "Work Order Number " + Arr[0] + " Already Exist ";
                        Arr[1] = "2";
                        return Arr;
                    }
                    else
                    {
                        NpgsqlCommand cmd = new NpgsqlCommand("save_workorder_rangeallocation");
                        cmd.Parameters.AddWithValue("startrange", Objworangeallocation.Startrangenumber);
                        cmd.Parameters.AddWithValue("endrange", Objworangeallocation.Endrangenumber);
                        cmd.Parameters.AddWithValue("wo_startrange", Objworangeallocation.Workorderstartrange);
                        cmd.Parameters.AddWithValue("wo_endrange", Objworangeallocation.Workorderendrange);
                        cmd.Parameters.AddWithValue("divcode", Convert.ToInt32(Objworangeallocation.Divcode));
                        cmd.Parameters.AddWithValue("financialyear", Objworangeallocation.Financialyear);
                        cmd.Parameters.AddWithValue("allotmentdate", Objworangeallocation.Allocationdate);
                        cmd.Parameters.AddWithValue("quantity", Convert.ToInt32(Objworangeallocation.Quantity));
                        cmd.Parameters.AddWithValue("accounthead", Objworangeallocation.Accounthead);
                        cmd.Parameters.AddWithValue("wo_no_alphabet", Objworangeallocation.StartrangeAlphabet);
                        cmd.Parameters.AddWithValue("usid", Convert.ToInt32(Objworangeallocation.Crby));
                        cmd.Parameters.AddWithValue("commdecomflag", Convert.ToInt32(Objworangeallocation.Comm_Decomm_flag));

                        cmd.Parameters.Add("pk_id", NpgsqlDbType.Text);
                        cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                        cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                        cmd.Parameters["pk_id"].Direction = ParameterDirection.Output;
                        cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                        cmd.Parameters["msg"].Direction = ParameterDirection.Output;

                        Arrmsg[0] = "msg";
                        Arrmsg[1] = "op_id";
                        Arrmsg[2] = "pk_id";
                        Arrmsg = objCon.Execute(cmd, Arrmsg, 3);

                        bResult = true;
                        if (bResult == true)
                        {
                            Arrmsg[0] = "Work order range allocation Saved Successfully";
                            Arrmsg[1] = "0";
                        }
                        else
                        {
                            Arrmsg[0] = "No Work order Exists to Save";
                            Arrmsg[1] = "2";
                        }
                    }
                }
                else if (Objworangeallocation.sActionType == "M")
                {
                    NpgsqlCommand cmdworkorderexist = new NpgsqlCommand("workorder_checkduplicate_bulk_modifyapprove");
                    cmdworkorderexist.Parameters.AddWithValue("startrange", Objworangeallocation.Startrangenumber);
                    cmdworkorderexist.Parameters.AddWithValue("endrange", Objworangeallocation.Endrangenumber);
                    cmdworkorderexist.Parameters.AddWithValue("wo_startrange", Objworangeallocation.Workorderstartrange);
                    cmdworkorderexist.Parameters.AddWithValue("wo_endrange", Objworangeallocation.Workorderendrange);
                    cmdworkorderexist.Parameters.AddWithValue("divcode", Convert.ToInt32(Objworangeallocation.Divcode));
                    cmdworkorderexist.Parameters.AddWithValue("financialyear", Objworangeallocation.Financialyear);
                    cmdworkorderexist.Parameters.AddWithValue("accounthead", Objworangeallocation.Accounthead);
                    cmdworkorderexist.Parameters.AddWithValue("wo_no_alphabet", Objworangeallocation.StartrangeAlphabet);
                    cmdworkorderexist.Parameters.AddWithValue("record_id", Convert.ToInt32(Objworangeallocation.sRecordId));

                    cmdworkorderexist.Parameters.Add("wo_no", NpgsqlDbType.Text);
                    cmdworkorderexist.Parameters.Add("status", NpgsqlDbType.Text);
                    cmdworkorderexist.Parameters["status"].Direction = ParameterDirection.Output;
                    cmdworkorderexist.Parameters["wo_no"].Direction = ParameterDirection.Output;

                    Arr[0] = "wo_no";
                    Arr[1] = "status";
                    Arr = objCon.Execute(cmdworkorderexist, Arr, 2);
                    if (Arr[1] == "-1")
                    {
                        Arr[0] = "Work Order Number " + Arr[0] + " Already Exist "; 
                        Arr[1] = "-1";
                        return Arr;
                    }
                    else
                    {
                        NpgsqlCommand cmdworkorderrange = new NpgsqlCommand("workorder_rangeallocation_modifyapprove");
                        cmdworkorderrange.Parameters.AddWithValue("startrange", Objworangeallocation.Startrangenumber);
                        cmdworkorderrange.Parameters.AddWithValue("endrange", Objworangeallocation.Endrangenumber);
                        cmdworkorderrange.Parameters.AddWithValue("wo_startrange", Objworangeallocation.Workorderstartrange);
                        cmdworkorderrange.Parameters.AddWithValue("wo_endrange", Objworangeallocation.Workorderendrange);
                        cmdworkorderrange.Parameters.AddWithValue("divcode", Convert.ToInt32(Objworangeallocation.Divcode));
                        cmdworkorderrange.Parameters.AddWithValue("financialyear", Objworangeallocation.Financialyear);
                        cmdworkorderrange.Parameters.AddWithValue("accounthead", Objworangeallocation.Accounthead);
                        cmdworkorderrange.Parameters.AddWithValue("wo_no_alphabet", Objworangeallocation.StartrangeAlphabet);
                        cmdworkorderrange.Parameters.AddWithValue("recordid", Convert.ToInt32(Objworangeallocation.sRecordId));
                        cmdworkorderrange.Parameters.AddWithValue("quantity", Convert.ToInt32(Objworangeallocation.Quantity));

                        cmdworkorderrange.Parameters.Add("pk_id", NpgsqlDbType.Integer);
                        cmdworkorderrange.Parameters.Add("msg", NpgsqlDbType.Text);
                        cmdworkorderrange.Parameters.Add("status", NpgsqlDbType.Text);
                        cmdworkorderrange.Parameters["pk_id"].Direction = ParameterDirection.Output;
                        cmdworkorderrange.Parameters["status"].Direction = ParameterDirection.Output;
                        cmdworkorderrange.Parameters["msg"].Direction = ParameterDirection.Output;

                        Arrmsg[0] = "msg";
                        Arrmsg[1] = "status";
                        Arrmsg[2] = "pk_id";
                        Arrmsg = objCon.Execute(cmdworkorderrange, Arrmsg, 3);
                        if (Arrmsg[1] == "1")
                        {
                            Arrmsg[0] = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SuccessModify"]);
                            Arrmsg[1] = "2";
                        }
                    }
                }


                Qry = "UPDATE \"TBLWORKORDERRANGEALLOCATION\" SET \"WRA_EE_APPROVE\" ='1' WHERE \"WRA_ID\"='" + Arrmsg[2] + "'";
                Qry = Qry.Replace("'", "''");

                clsApproval objApproval = new clsApproval();

                //string Query = " select \"BO_NAME\"  from \"TBLBUSINESSOBJECT\" where \"BO_ID\"='"+ Objworangeallocation.sboid + "'";
                //objApproval.sFormName = objCon.get_value(Query);
                objApproval.sFormName = objApproval.GetFormName(Objworangeallocation.sboid);

                objApproval.sRecordId = Arrmsg[2];
                objApproval.sOfficeCode = Objworangeallocation.offcode;
                objApproval.sClientIp = Objworangeallocation.sClientIP;
                objApproval.sCrby = Objworangeallocation.Crby;
                objApproval.sWFObjectId = Objworangeallocation.sWFObjectId;
                objApproval.sWFAutoId = Objworangeallocation.sWFAutoId;
                objApproval.sDataReferenceId = Objworangeallocation.Accounthead;
                objApproval.sRefOfficeCode = Objworangeallocation.Divcode;
                objApproval.sQryValues = Qry;

                objApproval.sDescription = "Workorder Range Allocation from " + Objworangeallocation.Workorderstartrange + "" +
                    " to " + Objworangeallocation.Workorderendrange + " For Account Head  " + Objworangeallocation.Accounthead + "";

                objApproval.sColumnNames = "WRA_DIV,WRA_FINANCIALYEAR,WRA_ACC_HEAD,WRA_ALLOTMENT_DATE,";
                objApproval.sColumnNames += "WRA_QUANTITY,WRA_START_RANGE,WRA_END_RANGE,WRA_SR_SLNO,";
                objApproval.sColumnNames += "WRA_ER_SLNO,WRA_WO_SERIES,WRA_COMM_DECOMM_FLAG";
                objApproval.sColumnValues = "" + Objworangeallocation.Divcode + "," + Objworangeallocation.Financialyear
                    + "," + Objworangeallocation.Accounthead + "," + Objworangeallocation.Allocationdate
                    + "," + Objworangeallocation.Quantity + "," + Objworangeallocation.Workorderstartrange + ",";
                objApproval.sColumnValues += "" + Objworangeallocation.Workorderendrange + ","
                    + Objworangeallocation.Startrangenumber + "," + Objworangeallocation.Endrangenumber
                    + "," + Objworangeallocation.StartrangeAlphabet + "," + Objworangeallocation.Comm_Decomm_flag + "";
                objApproval.sTableNames = "TBLWORKORDERRANGEALLOCATION";

                bResult = objApproval.CheckDuplicateApprove(objApproval);
                if (bResult == false)
                {
                    Arr[0] = "Selected Record Already Approved";
                    Arr[1] = "2";
                    return Arr;
                }
                if (Objworangeallocation.sActionType == Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ActionTypeModifyApprove"]))
                {
                    objApproval.SaveWorkFlowData(objApproval);
                    Objworangeallocation.sWFDataId = objApproval.sWFDataId;
                }
                else
                {
                    objApproval.SaveWorkFlowData(objApproval);
                    Objworangeallocation.sWFDataId = objApproval.sWFDataId;
                    objApproval.SaveWorkflowObjects(objApproval);
                }


                //Arr[0] = "Approved Successfully";
                //Arr[1] = "0";
                return Arrmsg;

            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }
        /// <summary>
        ///  to get work order range allocation details from XML
        /// </summary>
        /// <param name="objworkorderrangedetails"></param>
        /// <returns></returns>
        public ClsWorkOrderRangeAllocation GetworangeDetailsFromXML(ClsWorkOrderRangeAllocation objworkorderrangedetails)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable dtWODetails = new DataTable();

                dtWODetails = objApproval.GetDatatableFromXML(objworkorderrangedetails.sWFDataId);
                if (dtWODetails.Rows.Count > 0)
                {
                    objworkorderrangedetails.Divcode = Convert.ToString(dtWODetails.Rows[0]["WRA_DIV"]).Trim();
                    objworkorderrangedetails.Financialyear = Convert.ToString(dtWODetails.Rows[0]["WRA_FINANCIALYEAR"]).Trim();
                    objworkorderrangedetails.Accounthead = Convert.ToString(dtWODetails.Rows[0]["WRA_ACC_HEAD"]).Trim();
                    objworkorderrangedetails.Allocationdate = Convert.ToString(dtWODetails.Rows[0]["WRA_ALLOTMENT_DATE"]).Trim();
                    objworkorderrangedetails.Quantity = Convert.ToString(dtWODetails.Rows[0]["WRA_QUANTITY"]).Trim();
                    objworkorderrangedetails.Workorderstartrange = Convert.ToString(dtWODetails.Rows[0]["WRA_START_RANGE"]).Trim();
                    objworkorderrangedetails.Workorderendrange = Convert.ToString(dtWODetails.Rows[0]["WRA_END_RANGE"]).Trim();
                    objworkorderrangedetails.Startrangenumber = Convert.ToString(dtWODetails.Rows[0]["WRA_SR_SLNO"]).Trim();
                    objworkorderrangedetails.Endrangenumber = Convert.ToString(dtWODetails.Rows[0]["WRA_ER_SLNO"]).Trim();
                    objworkorderrangedetails.StartrangeAlphabet = Convert.ToString(dtWODetails.Rows[0]["WRA_WO_SERIES"]).Trim();
                    objworkorderrangedetails.Comm_Decomm_flag = Convert.ToString(dtWODetails.Rows[0]["WRA_COMM_DECOMM_FLAG"]).Trim();
                }
                return objworkorderrangedetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objworkorderrangedetails;
            }
        }

    }
}
