using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;
using System.Configuration;

namespace IIITS.DTLMS.BL
{
    public class clsInvoice
    {

        string strFormCode = "clsInvoice";

        public string sWarrentyPeriod { get; set; }
        public string sDtrWarrentyTime { get; set; }
        public string sInvoiceSlNo { get; set; }
        public string sStoreId { get; set; }
        public string sDtcFailId { get; set; }
        public string sTcSlNo { get; set; }
        public string sInvoiceNo { get; set; }
        public string sInvoiceDate { get; set; }
        public string sInvoiceDescription { get; set; }
        public string sFailDate { get; set; }
        public string sAmount { get; set; }

        public string sCreatedBy { get; set; }
        public string sTcMake { get; set; }
        public string sTcCapacity { get; set; }
        public string sWOSlno { get; set; }
        public string sTcNewCapacity { get; set; }
        public string sIndentId { get; set; }
        public string sIndentNo { get; set; }
        public string sIndentCrby { get; set; }
        public string sIndentDate { get; set; }
        public string sTcCode { get; set; }
        public string sDTCName { get; set; }
        public string sOldTcSlno { get; set; }
        public string sOldTcCode { get; set; }
        public string sTCId { get; set; }
        public string sDTCId { get; set; }
        public string sDTCCODE { get; set; }

        public string sManualInvoiceNo { get; set; }

        public string sTaskType { get; set; }
        public string sOfficeCode { get; set; }

        // Workflow
        public string sFormName { get; set; }
        public string sClientIP { get; set; }
        public string sWFOId { get; set; }
        public string sWFDataId { get; set; }
        public string sWFAutoId { get; set; }

        //Gate Pass
        public string sVehicleNumber { get; set; }
        public string sReceiptientName { get; set; }
        public string sChallenNo { get; set; }
        public string sGatePassId { get; set; }
        public string sDTCCode { get; set; }
        public string sIssueQty { get; set; }
        public string sStoreType { get; set; }
        public string smanufactureDate { get; set; }
        public string sTcRating { get; set; }

        //CustOledbConnection objCon = new CustOledbConnection(Constants.Password);
        PGSqlConnection objCon = new PGSqlConnection(Constants.Password);
        NpgsqlCommand NpgsqlCommand;
        /// <summary>
        /// Save Update Invoice Details
        /// </summary>
        /// <param name="objInvoice"></param>
        /// <returns></returns>
        public string[] SaveUpdateInvoiceDetails(clsInvoice objInvoice)
        {
            DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
            string[] Arr = new string[2];
            string strQry = string.Empty;
            try
            {
                objDatabse.BeginTransaction();
                string[] strResult = new string[2];
                string[] strArray = new string[2];
                ////Check Work Order no exists or not
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sIndentNo", objInvoice.sIndentNo.ToUpper());
                //string sInNo = objDatabse.get_value("SELECT \"TI_INDENT_NO\" FROM \"TBLINDENT\" WHERE UPPER(\"TI_INDENT_NO\")=:sIndentNo", NpgsqlCommand);
                //if (sInNo.Length == 0)
                //{
                //    Arr[0] = "Enter Valid Indent No";
                //    Arr[1] = "2";
                //    return Arr;
                //}
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sTcCode", Convert.ToString(objInvoice.sTcCode));
                //string sTccode = objDatabse.get_value("SELECT \"TC_CODE\" FROM \"TBLTCMASTER\" WHERE \"TC_CODE\" =:sTcCode ", NpgsqlCommand);
                //if (sTccode.Length == 0)
                //{
                //    Arr[0] = "Enter Valid DTr Code";
                //    Arr[1] = "2";
                //    return Arr;
                //}
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sTcCode1", Convert.ToString(objInvoice.sTcCode));
                //sTccode = objDatabse.get_value("SELECT \"TC_CODE\" FROM \"TBLTCMASTER\" WHERE \"TC_CODE\" =:sTcCode1 AND \"TC_STATUS\" IN ('1','2') AND \"TC_CURRENT_LOCATION\" in (1,5)", NpgsqlCommand);
                //if (sTccode.Length == 0)
                //{
                //    Arr[0] = "Entered DTr Code not in Store or Not in good condition";
                //    Arr[1] = "2";
                //    return Arr;
                //}
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sTcCode2", Convert.ToString(objInvoice.sTcCode));
                //NpgsqlCommand.Parameters.AddWithValue("sTcNewCapacity", Convert.ToDouble(objInvoice.sTcNewCapacity));
                //sTccode = objDatabse.get_value("SELECT \"TC_CODE\" FROM \"TBLTCMASTER\" WHERE \"TC_CODE\" =:sTcCode2 AND \"TC_CAPACITY\" =:sTcNewCapacity", NpgsqlCommand);
                //if (sTccode.Length == 0)
                //{
                //    Arr[0] = "Entered DTr Code Capacity not Matching with Requested Capacity";
                //    Arr[1] = "2";
                //    return Arr;
                //}
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sDTCCODE", objInvoice.sDTCCode);
                //NpgsqlCommand.Parameters.AddWithValue("sOldTcCode", objInvoice.sOldTcCode);
                //NpgsqlCommand.Parameters.AddWithValue("sIndentId", objInvoice.sIndentId);
                //sInNo = objDatabse.get_value("SELECT \"TI_INDENT_NO\" FROM \"TBLINDENT\",\"TBLWORKORDER\",\"TBLDTCFAILURE\",\"TBLDTCINVOICE\" WHERE \"DF_ID\"=\"WO_DF_ID\" AND \"WO_SLNO\"=\"TI_WO_SLNO\" AND \"TI_ID\"=\"IN_TI_NO\" AND CAST(\"DF_DTC_CODE\" AS TEXT) =:sDTCCODE AND CAST(\"DF_EQUIPMENT_ID\" AS TEXT)=:sOldTcCode  AND CAST(\"TI_ID\" AS TEXT)=:sIndentId", NpgsqlCommand);
                //if (sInNo.Length > 0)
                //{
                //    Arr[0] = "Invoice Already done for this DTC";
                //    Arr[1] = "2";
                //    return Arr;
                //}

                #region Converted to sp
                string sResult = string.Empty;
                string[] sArray = new string[2];
                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand cmddtccode = new NpgsqlCommand("fetch_saveupdate_existvalue_clsinvoice");
                cmddtccode.Parameters.AddWithValue("sindentno", objInvoice.sIndentNo.ToUpper());
                cmddtccode.Parameters.AddWithValue("stccode", Convert.ToString(objInvoice.sTcCode));
                cmddtccode.Parameters.AddWithValue("stcnewcapacity", Convert.ToDouble(objInvoice.sTcNewCapacity));
                cmddtccode.Parameters.AddWithValue("sdtccode", objInvoice.sDTCCode);
                cmddtccode.Parameters.AddWithValue("soldtccode", objInvoice.sOldTcCode);
                cmddtccode.Parameters.AddWithValue("sindentid", objInvoice.sIndentId);
                cmddtccode.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmddtccode.Parameters.Add("msg", NpgsqlDbType.Text);
                cmddtccode.Parameters["op_id"].Direction = ParameterDirection.Output;
                cmddtccode.Parameters["msg"].Direction = ParameterDirection.Output;
                sArray[0] = "op_id";
                sArray[1] = "msg";
                string[] stResult = objDatabse.Execute(cmddtccode, sArray, 2);

                if (stResult[0] == "2")
                {
                    Arr[0] = stResult[1];
                    Arr[1] = stResult[0];
                    return Arr;
                }
                #endregion
                if (objInvoice.sInvoiceSlNo == "")
                {
                    //NpgsqlCommand = new NpgsqlCommand();
                    //NpgsqlCommand.Parameters.AddWithValue("sInvoiceNo", objInvoice.sInvoiceNo.ToUpper());
                    //string sId = objDatabse.get_value("SELECT \"IN_NO\" FROM \"TBLDTCINVOICE\" WHERE  UPPER(\"IN_INV_NO\")=:sInvoiceNo ", NpgsqlCommand);
                    //if (sId.Length > 0)
                    //{
                    //    Arr[0] = "Invoice No. Already Exists";
                    //    Arr[1] = "2";
                    //    return Arr;
                    //}


                    #region Converted to sp
                    NpgsqlCommand = new NpgsqlCommand();
                    NpgsqlCommand cmdinvoice = new NpgsqlCommand("fetch_invoice_existvalue_clsinvoice");
                    cmdinvoice.Parameters.AddWithValue("sinvoiceno", objInvoice.sInvoiceNo.ToUpper());
                    cmdinvoice.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmdinvoice.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmdinvoice.Parameters["op_id"].Direction = ParameterDirection.Output;
                    cmdinvoice.Parameters["msg"].Direction = ParameterDirection.Output;
                    sArray[0] = "op_id";
                    sArray[1] = "msg";
                    stResult = objDatabse.Execute(cmdinvoice, sArray, 2);

                    if (stResult[0] == "2")
                    {
                        Arr[0] = stResult[1];
                        Arr[1] = stResult[0];
                        return Arr;
                    }
                    #endregion
                    NpgsqlCommand cmd = new NpgsqlCommand("sp_save_invoice");
                    cmd.Parameters.AddWithValue("sinvoiceno", (objInvoice.sInvoiceNo.ToUpper() ?? ""));
                    cmd.Parameters.AddWithValue("sinvoicedate", (objInvoice.sInvoiceDate ?? ""));
                    cmd.Parameters.AddWithValue("sinvoicedescription", (objInvoice.sInvoiceDescription ?? ""));
                    cmd.Parameters.AddWithValue("samount", (objInvoice.sAmount ?? ""));
                    cmd.Parameters.AddWithValue("sindentid", (objInvoice.sIndentId ?? ""));
                    cmd.Parameters.AddWithValue("screatedby", (objInvoice.sCreatedBy ?? ""));
                    cmd.Parameters.AddWithValue("smanualinvoiceno", (objInvoice.sManualInvoiceNo.ToUpper() ?? ""));
                    cmd.Parameters.AddWithValue("sdtcfailid", (objInvoice.sDtcFailId ?? ""));
                    cmd.Parameters.AddWithValue("stccode", (objInvoice.sTcCode ?? ""));
                    cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                    cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                    strArray[0] = "op_id";
                    strArray[1] = "msg";
                    strResult = objDatabse.Execute(cmd, strArray, 2);
                    objInvoice.sInvoiceSlNo = strResult[0];
                    //string invoicenum = objDatabse.get_value("SELECT \"TR_IN_NO\" from \"TBLTCREPLACE\" WHERE \"TR_WO_SLNO\" = '" + objInvoice.sIndentId + "' limit 1");

                    #region Converted to sp
                    NpgsqlCommand cmdtr_in_no = new NpgsqlCommand("fetch_getvalue_clsinvoice");
                    cmdtr_in_no.Parameters.AddWithValue("p_key", "GET_TR_IN_NO");
                    cmdtr_in_no.Parameters.AddWithValue("p_value", objInvoice.sIndentId);
                    string invoicenum = objDatabse.StringGetValue(cmdtr_in_no);
                    #endregion
                    if (invoicenum.Contains('-'))
                    {
                        //string upqry = "UPDATE \"TBLTCREPLACE\" set \"TR_IN_NO\"='" + objInvoice.sInvoiceSlNo + "' WHERE \"TR_WO_SLNO\"='" + objInvoice.sIndentId + "'";
                        //objDatabse.ExecuteQry(upqry);


                        NpgsqlCommand cmd1 = new NpgsqlCommand("proc_update_tr_in_no_clsinvoice");
                        cmd1.Parameters.AddWithValue("invoiceslno", Convert.ToInt32(objInvoice.sInvoiceSlNo));
                        cmd1.Parameters.AddWithValue("indentid", Convert.ToInt32(objInvoice.sIndentId));
                        cmd1.Parameters.Add("msg", NpgsqlDbType.Text);
                        cmd1.Parameters.Add("op_id", NpgsqlDbType.Text);
                        cmd1.Parameters["msg"].Direction = ParameterDirection.Output;
                        cmd1.Parameters["op_id"].Direction = ParameterDirection.Output;
                        Arr[0] = "msg";
                        Arr[1] = "op_id";
                        Arr = objDatabse.Execute(cmd1, Arr, 2);
                    }
                    if (objInvoice.sTaskType != "3")
                    {
                        //string officecode = objDatabse.get_value("SELECT \"DF_LOC_CODE\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\"='" + objInvoice.sDtcFailId + "'");

                        #region Converted to sp
                        NpgsqlCommand cmdget_df_loc_code = new NpgsqlCommand("fetch_getvalue_clsinvoice");
                        cmdget_df_loc_code.Parameters.AddWithValue("p_key", "GET_DF_LOC_CODE");
                        cmdget_df_loc_code.Parameters.AddWithValue("p_value", objInvoice.sDtcFailId);
                        string officecode = objDatabse.StringGetValue(cmdget_df_loc_code);
                        #endregion

                        //string faildtrcode = objDatabse.get_value("SELECT \"DF_EQUIPMENT_ID\" FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\"='" + objInvoice.sDtcFailId + "'");

                        #region Converted to sp
                        NpgsqlCommand cmdget_equipment_id = new NpgsqlCommand("fetch_getvalue_clsinvoice");
                        cmdget_equipment_id.Parameters.AddWithValue("p_key", "GET_DF_EQUIPMENT_ID");
                        cmdget_equipment_id.Parameters.AddWithValue("p_value", objInvoice.sDtcFailId);
                        string faildtrcode = objDatabse.StringGetValue(cmdget_equipment_id);
                        #endregion

                        // update invoicing dtr  from store to field  
                        //string strQry1 = "UPDATE \"TBLTCMASTER\" SET \"TC_UPDATED_EVENT\"='REPLACED DTR FROM STORE TO FIELD WITH DTC " + objInvoice.sDTCCode + "',\"TC_UPDATED_EVENT_ID\"='" + objInvoice.sInvoiceSlNo + "',";
                        //strQry1 += " \"TC_CURRENT_LOCATION\"=2,\"TC_LOCATION_ID\"='" + officecode + "' WHERE ";
                        //strQry1 += " \"TC_CODE\" ='" + objInvoice.sTcCode + "'";
                        //objDatabse.ExecuteQry(strQry1);

                        #region Converted to sp
                        NpgsqlCommand cmd1 = new NpgsqlCommand("proc_update_storetofield_clsinvoice");
                        cmd1.Parameters.AddWithValue("dtccode", Convert.ToString(objInvoice.sDTCCode));
                        cmd1.Parameters.AddWithValue("invoiceslno", Convert.ToInt32(objInvoice.sInvoiceSlNo));
                        cmd1.Parameters.AddWithValue("officecode", Convert.ToInt32(officecode));
                        cmd1.Parameters.AddWithValue("tccode", Convert.ToString(objInvoice.sTcCode));
                        cmd1.Parameters.Add("msg", NpgsqlDbType.Text);
                        cmd1.Parameters.Add("op_id", NpgsqlDbType.Text);
                        cmd1.Parameters["msg"].Direction = ParameterDirection.Output;
                        cmd1.Parameters["op_id"].Direction = ParameterDirection.Output;
                        Arr[0] = "msg";
                        Arr[1] = "op_id";
                        Arr = objDatabse.Execute(cmd1, Arr, 2);
                        #endregion

                        // unmap faulty dtr , get the dtr  using faild id  
                        //string strQry2 = "UPDATE \"TBLTRANSDTCMAPPING\" SET \"TM_LIVE_FLAG\" = '0', \"TM_UNMAP_CRON\" = NOW() ,\"TM_UNMAP_CRBY\" ='" + objInvoice.sCreatedBy + "',";
                        //strQry2 += " \"TM_UNMAP_REASON\" ='FROM FAILURE ENTRY' WHERE \"TM_TC_ID\" ='" + faildtrcode + "'";
                        //strQry2 += " AND \"TM_LIVE_FLAG\"='1' AND \"TM_DTC_ID\" ='" + objInvoice.sDTCCode + "'";
                        //objDatabse.ExecuteQry(strQry2);


                        #region Converted to sp
                        NpgsqlCommand cmdmapdtr = new NpgsqlCommand("proc_update_mapdtr_clsinvoice");
                        cmdmapdtr.Parameters.AddWithValue("crby", Convert.ToInt32(objInvoice.sCreatedBy));
                        cmdmapdtr.Parameters.AddWithValue("faildtrcode", Convert.ToString(faildtrcode));
                        cmdmapdtr.Parameters.AddWithValue("dtccode", Convert.ToString(objInvoice.sDTCCode));
                        cmdmapdtr.Parameters.Add("msg", NpgsqlDbType.Text);
                        cmdmapdtr.Parameters.Add("op_id", NpgsqlDbType.Text);
                        cmdmapdtr.Parameters["msg"].Direction = ParameterDirection.Output;
                        cmdmapdtr.Parameters["op_id"].Direction = ParameterDirection.Output;
                        Arr[0] = "msg";
                        Arr[1] = "op_id";
                        Arr = objDatabse.Execute(cmdmapdtr, Arr, 2);
                        #endregion


                        // update invoiced dtr to dtcmast  
                        //string strQry3 = "UPDATE \"TBLDTCMAST\" SET \"DT_TC_ID\" ='" + objInvoice.sTcCode + "' WHERE \"DT_CODE\" ='" + objInvoice.sDTCCode + "'";
                        //objDatabse.ExecuteQry(strQry3);

                        #region Converted to sp
                        NpgsqlCommand cmdinvoiceddtr = new NpgsqlCommand("proc_update_invoiceddtr_clsinvoice");
                        cmdinvoiceddtr.Parameters.AddWithValue("tccode", Convert.ToString(objInvoice.sTcCode));
                        cmdinvoiceddtr.Parameters.AddWithValue("dtccode", Convert.ToString(objInvoice.sDTCCode));
                        cmdinvoiceddtr.Parameters.Add("msg", NpgsqlDbType.Text);
                        cmdinvoiceddtr.Parameters.Add("op_id", NpgsqlDbType.Text);
                        cmdinvoiceddtr.Parameters["msg"].Direction = ParameterDirection.Output;
                        cmdinvoiceddtr.Parameters["op_id"].Direction = ParameterDirection.Output;
                        Arr[0] = "msg";
                        Arr[1] = "op_id";
                        Arr = objDatabse.Execute(cmdinvoiceddtr, Arr, 2);
                        #endregion

                        // insert into  transdtcmapping  saying dtc and  new dtr has invoiced .
                        //string strQry4 = "INSERT INTO \"TBLTRANSDTCMAPPING\"(\"TM_ID\",\"TM_MAPPING_DATE\",\"TM_TC_ID\",\"TM_DTC_ID\",\"TM_LIVE_FLAG\",\"TM_CRBY\",\"TM_CRON\")";
                        //strQry4 += " VALUES((SELECT COALESCE(MAX(\"TM_ID\"),0)+1 FROM \"TBLTRANSDTCMAPPING\"), NOW() ,'" + objInvoice.sTcCode + "',";
                        //strQry4 += " '" + objInvoice.sDTCCode + "','1','" + objInvoice.sCreatedBy + "', NOW())";
                        //objDatabse.ExecuteQry(strQry4);

                        #region Converted to sp
                        NpgsqlCommand cmdinsert = new NpgsqlCommand("proc_insert_transdtcmapping_clsinvoice");
                        cmdinsert.Parameters.AddWithValue("tccode", Convert.ToString(objInvoice.sTcCode));
                        cmdinsert.Parameters.AddWithValue("dtccode", Convert.ToString(objInvoice.sDTCCode));
                        cmdinsert.Parameters.AddWithValue("crby", Convert.ToInt32(objInvoice.sCreatedBy));
                        cmdinsert.Parameters.Add("msg", NpgsqlDbType.Text);
                        cmdinsert.Parameters.Add("op_id", NpgsqlDbType.Text);
                        cmdinsert.Parameters["msg"].Direction = ParameterDirection.Output;
                        cmdinsert.Parameters["op_id"].Direction = ParameterDirection.Output;
                        Arr[0] = "msg";
                        Arr[1] = "op_id";
                        Arr = objDatabse.Execute(cmdinsert, Arr, 2);
                        #endregion

                    }
                    //string strQryS = string.Empty;

                    //NpgsqlCommand = new NpgsqlCommand();
                    //strQryS = "SELECT \"DF_ID\" FROM \"TBLDTCFAILURE\" WHERE \"DF_DTC_CODE\" =:DtcCode AND \"DF_REPLACE_FLAG\" ='0'";
                    //NpgsqlCommand.Parameters.AddWithValue("DtcCode", objInvoice.sDTCCode);
                    //string df_id = objDatabse.get_value(strQryS, NpgsqlCommand);
                    //string sReplaceTCCode = objInvoice.sTcCode;

                    #region Converted to sp
                    NpgsqlCommand cmddf_id = new NpgsqlCommand("fetch_getvalue_clsinvoice");
                    cmddf_id.Parameters.AddWithValue("p_key", "GET_DF_ID");
                    cmddf_id.Parameters.AddWithValue("p_value", objInvoice.sDTCCode);
                    string df_id = objDatabse.StringGetValue(cmddf_id);
                    string sReplaceTCCode = objInvoice.sTcCode;
                    #endregion

                    //NpgsqlCommand = new NpgsqlCommand();
                    //strQry = "SELECT CASE WHEN NOW() < \"TC_WARANTY_PERIOD\" THEN 1 ELSE 0 END FROM \"TBLTCMASTER\" WHERE \"TC_CODE\" =:sReplaceTCCode";
                    //NpgsqlCommand.Parameters.AddWithValue("sReplaceTCCode", Convert.ToString(sReplaceTCCode));
                    //string sval = objDatabse.get_value(strQry, NpgsqlCommand);

                    #region Converted to sp
                    NpgsqlCommand cmdval = new NpgsqlCommand("fetch_getvalue_clsinvoice");
                    cmdval.Parameters.AddWithValue("p_key", "GET_VAL");
                    cmdval.Parameters.AddWithValue("p_value", Convert.ToString(sReplaceTCCode));
                    string sval = objDatabse.StringGetValue(cmdval);
                    #endregion

                    if (sval == "0")
                    {
                        //NpgsqlCommand = new NpgsqlCommand();
                        //strQry = "SELECT TO_CHAR(ADD_MONTHS(\"TM_MAPPING_DATE\",\"WARENTY_MONTH\"),'YYYY-MM-DD') WARENTY_DATE FROM (SELECT \"RSD_GUARRENTY_TYPE\" ,";
                        //strQry += " (\"RSD_WARENTY_PERIOD\" * 12) \"WARENTY_MONTH\", \"TC_CODE\", \"TM_MAPPING_DATE\" FROM \"TBLTRANSDTCMAPPING\", \"TBLREPAIRSENTDETAILS\",";
                        //strQry += " \"TBLTCMASTER\" WHERE  \"TC_CODE\"=\"RSD_TC_CODE\" AND \"RSD_TC_CODE\"=\"TM_TC_ID\" AND \"TC_CODE\"=:sReplaceTCCode1 AND \"TM_LIVE_FLAG\" =1)A";
                        //NpgsqlCommand.Parameters.AddWithValue("sReplaceTCCode1", Convert.ToString(sReplaceTCCode));
                        //sDtrWarrentyTime = objDatabse.get_value(strQry, NpgsqlCommand);

                        #region Converted to sp
                        NpgsqlCommand cmdwarrentytime = new NpgsqlCommand("fetch_getvalue_clsinvoice");
                        cmdwarrentytime.Parameters.AddWithValue("p_key", "GET_DTRWARRENTYTIME");
                        cmdwarrentytime.Parameters.AddWithValue("p_value", Convert.ToString(sReplaceTCCode));
                        sDtrWarrentyTime = objDatabse.StringGetValue(cmdwarrentytime);
                        #endregion

                        //NpgsqlCommand = new NpgsqlCommand();
                        //strQry = "SELECT \"RSD_WARENTY_PERIOD\" FROM \"TBLTRANSDTCMAPPING\", \"TBLREPAIRSENTDETAILS\", \"TBLTCMASTER\" WHERE \"TC_CODE\" = \"RSD_TC_CODE\" AND ";
                        //strQry += " \"RSD_TC_CODE\" = \"TM_TC_ID\"  AND \"TC_CODE\" =:sReplaceTCCode2 AND \"TM_LIVE_FLAG\" =1";
                        //NpgsqlCommand.Parameters.AddWithValue("sReplaceTCCode2", Convert.ToString(sReplaceTCCode));
                        //sWarrentyPeriod = objDatabse.get_value(strQry, NpgsqlCommand);

                        #region Converted to sp
                        NpgsqlCommand cmdget_rsd_warenty_period = new NpgsqlCommand("fetch_getvalue_clsinvoice");
                        cmdget_rsd_warenty_period.Parameters.AddWithValue("p_key", "GET_RSD_WARENTY_PERIOD");
                        cmdget_rsd_warenty_period.Parameters.AddWithValue("p_value", Convert.ToString(sReplaceTCCode));
                        sWarrentyPeriod = objDatabse.StringGetValue(cmdget_rsd_warenty_period);
                        #endregion

                    }
                    if (sval == "0")
                    {
                        NpgsqlCommand = new NpgsqlCommand();
                        strQry = "UPDATE \"TBLTCMASTER\" SET \"TC_WARANTY_PERIOD\" =TO_DATE(:sDtrWarrentyTime,'yyyy-MM-dd'), \"TC_WARRENTY\" =:sWarrentyPeriod1 WHERE ";
                        strQry += " \"TC_CODE\" =:sReplaceTCCode3";
                        NpgsqlCommand.Parameters.AddWithValue("sDtrWarrentyTime", sDtrWarrentyTime);
                        if (sWarrentyPeriod == "" || sWarrentyPeriod == null)
                        {
                            sWarrentyPeriod = "0";
                        }
                        NpgsqlCommand.Parameters.AddWithValue("sWarrentyPeriod1", Convert.ToInt64(sWarrentyPeriod));
                        NpgsqlCommand.Parameters.AddWithValue("sReplaceTCCode3", Convert.ToString(sReplaceTCCode));
                        objDatabse.ExecuteQry(strQry, NpgsqlCommand);
                    }

                    #region WorkFlow
                    //Workflow / Approval
                    clsApproval objApproval = new clsApproval();
                    objApproval.sFormName = objInvoice.sFormName;
                    objApproval.sRecordId = objInvoice.sInvoiceSlNo;
                    objApproval.sOfficeCode = objInvoice.sOfficeCode;
                    objApproval.sClientIp = objInvoice.sClientIP;
                    objApproval.sCrby = objInvoice.sCreatedBy;
                    objApproval.sWFObjectId = objInvoice.sWFOId;
                    objApproval.sDataReferenceId = objInvoice.sIndentId;
                    objApproval.sWFAutoId = objInvoice.sWFAutoId;
                    if (objInvoice.sTaskType == "3")
                    {
                        objApproval.sStoreType = "2";
                    }
                    else
                    {
                        objApproval.sStoreType = "1";
                    }
                    objApproval.sDescription = "Invoice Creation for Indent No " + objInvoice.sIndentNo + ", DTC code" + objInvoice.sDTCCode;
                    if (objInvoice.sTaskType != "3")
                    {
                        //NpgsqlCommand = new NpgsqlCommand();
                        //NpgsqlCommand.Parameters.AddWithValue("sIndentId2", Convert.ToInt32(objInvoice.sIndentId));
                        //objApproval.sRefOfficeCode = objDatabse.get_value("SELECT \"DF_LOC_CODE\" FROM \"TBLDTCFAILURE\",\"TBLWORKORDER\",\"TBLINDENT\" WHERE \"DF_ID\"=\"WO_DF_ID\" AND \"WO_SLNO\"=\"TI_WO_SLNO\" AND \"WO_SLNO\"=:sIndentId2", NpgsqlCommand);

                        #region Converted to sp
                        NpgsqlCommand cmdget_ref_officecode = new NpgsqlCommand("fetch_getvalue_clsinvoice");
                        cmdget_ref_officecode.Parameters.AddWithValue("p_key", "GET_REF_OFFICECODE");
                        cmdget_ref_officecode.Parameters.AddWithValue("p_value", Convert.ToString(objInvoice.sIndentId));
                        objApproval.sRefOfficeCode = objDatabse.StringGetValue(cmdget_ref_officecode);
                        #endregion
                    }
                    else
                    {
                        //NpgsqlCommand = new NpgsqlCommand();
                        //NpgsqlCommand.Parameters.AddWithValue("sIndentId3", Convert.ToInt32(objInvoice.sIndentId));
                        //objApproval.sRefOfficeCode = objDatabse.get_value("SELECT \"WO_REQUEST_LOC\" FROM \"TBLWORKORDER\",\"TBLINDENT\" WHERE \"WO_SLNO\"=\"TI_WO_SLNO\" AND \"WO_SLNO\"=:sIndentId3", NpgsqlCommand);

                        #region Converted to sp
                        NpgsqlCommand cmdget_request_loc = new NpgsqlCommand("fetch_getvalue_clsinvoice");
                        cmdget_request_loc.Parameters.AddWithValue("p_key", "GET_WO_REQUEST_LOC");
                        cmdget_request_loc.Parameters.AddWithValue("p_value", Convert.ToString(objInvoice.sIndentId));
                        objApproval.sRefOfficeCode = objDatabse.StringGetValue(cmdget_request_loc);
                        #endregion
                    }
                    //bool bResult = objApproval.CheckDuplicateApprove(objApproval);
                    bool bResult = objApproval.CheckDuplicateApprove_Latest(objApproval, objDatabse);
                    if (bResult == false)
                    {
                        Arr[0] = "Selected Record Already Approved";
                        Arr[1] = "2";
                        return Arr;
                    }
                    //objApproval.SaveWorkflowObjects(objApproval);
                    objApproval.SaveWorkflowObjects_Latest(objApproval, objDatabse);
                    #endregion
                    Arr[0] = "Invoice Created Successfully";
                    Arr[1] = "0";
                    objDatabse.CommitTransaction();
                    return Arr;
                }
                else
                {
                    NpgsqlCommand ncmd = new NpgsqlCommand("sp_update_invoice");
                    ncmd.Parameters.AddWithValue("sinvoiceno", objInvoice.sInvoiceNo.ToUpper());
                    ncmd.Parameters.AddWithValue("sinvoicedate", objInvoice.sInvoiceDate);
                    ncmd.Parameters.AddWithValue("sinvoicedescription", objInvoice.sInvoiceDescription);
                    ncmd.Parameters.AddWithValue("samount", objInvoice.sAmount);
                    ncmd.Parameters.AddWithValue("sinvoiceslno", objInvoice.sInvoiceSlNo);
                    ncmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                    ncmd.Parameters.Add("msg", NpgsqlDbType.Text);
                    ncmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                    ncmd.Parameters["msg"].Direction = ParameterDirection.Output;
                    strArray[0] = "op_id";
                    strArray[1] = "msg";
                    strResult = objCon.Execute(ncmd, strArray, 2);
                    return strResult;
                }
            }
            catch (Exception ex)
            {
                objDatabse.RollBackTrans();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                //return Arr;
                throw ex;
            }
        }


        public DataTable LoadAllInvoiceDetails(clsInvoice objInvoice)
        {
            DataTable dt = new DataTable();
            try
            {

                NpgsqlCommand cmd = new NpgsqlCommand("sp_loadallinvoicedetails");
                cmd.Parameters.AddWithValue("stasktype", objInvoice.sTaskType);
                cmd.Parameters.AddWithValue("sofficecode", objInvoice.sOfficeCode);
                dt = objCon.FetchDataTable(cmd);
                
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }


        public DataTable LoadExistingInvoice(clsInvoice objInvoice)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {

                NpgsqlCommand cmd = new NpgsqlCommand("sp_loadexistinginvoice");
                cmd.Parameters.AddWithValue("stasktype", objInvoice.sTaskType);
                cmd.Parameters.AddWithValue("sofficecode", objInvoice.sOfficeCode);
                dt = objCon.FetchDataTable(cmd);

                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public clsInvoice GetTCDetails(clsInvoice objInvoice)
        {
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();

                //if (objInvoice.sTcCapacity != null)
                //{
                //    if (objInvoice.sStoreType == "1")
                //    {
                //        NpgsqlCommand = new NpgsqlCommand();
                //        strQry = "SELECT \"TC_CODE\" FROM \"TBLTCMASTER\" WHERE CAST(\"TC_LOCATION_ID\" AS TEXT) =:sOfficeCode AND \"TC_STATUS\" in (1,2) AND ";
                //        strQry += " \"TC_CURRENT_LOCATION\" =1 AND CAST(\"TC_CAPACITY\"  AS TEXT) =:sTcCapacity AND (CAST(\"TC_CODE\" AS TEXT) =:sTcCode or  CAST(\"TC_CODE\" AS TEXT) =:sTcCode1)";
                //        NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", objInvoice.sOfficeCode);
                //        // NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", clsStoreOffice.GetStoreID(objInvoice.sOfficeCode));
                //        NpgsqlCommand.Parameters.AddWithValue("sTcCapacity", objInvoice.sTcCapacity);
                //        NpgsqlCommand.Parameters.AddWithValue("sTcCode", objInvoice.sTcCode);
                //        NpgsqlCommand.Parameters.AddWithValue("sTcCode1", objInvoice.sTcCode);
                //    }
                //    else
                //    {
                //        NpgsqlCommand = new NpgsqlCommand();
                //        strQry = "SELECT \"TC_CODE\" FROM \"TBLTCMASTER\" WHERE CAST(\"TC_LOCATION_ID\" AS TEXT) =:sOfficeCode1 AND \"TC_STATUS\" in (1,2) AND ";
                //        strQry += " \"TC_CURRENT_LOCATION\" =5 AND CAST(\"TC_CAPACITY\"  AS TEXT) =:sTcCapacity1 AND (cast(\"TC_CODE\" as text)  =:sTcCode2 or  CAST(\"TC_CODE\" AS TEXT) =:sTcCode3)";
                //        NpgsqlCommand.Parameters.AddWithValue("sOfficeCode1", clsStoreOffice.GetStoreID(objInvoice.sOfficeCode));
                //        NpgsqlCommand.Parameters.AddWithValue("sTcCapacity1", objInvoice.sTcCapacity);
                //        NpgsqlCommand.Parameters.AddWithValue("sTcCode2", objInvoice.sTcCode);
                //        NpgsqlCommand.Parameters.AddWithValue("sTcCode3", objInvoice.sTcCode);
                //    }

                //}
                //else
                //{
                //    NpgsqlCommand = new NpgsqlCommand();
                //    strQry = "SELECT \"TC_CODE\" FROM \"TBLTCMASTER\" WHERE  \"TC_STATUS\" in (1,2) AND ";
                //    strQry += " (\"TC_CODE\"  ='" + objInvoice.sTcCode + "' or  CAST(\"TC_CODE\" AS TEXT) =:sTcCode4)";
                //    NpgsqlCommand.Parameters.AddWithValue("sTcCode4", objInvoice.sTcCode);
                //}

                //string res = objCon.get_value(strQry, NpgsqlCommand);


                #region Converted to sp
                string storeid = clsStoreOffice.GetStoreID(objInvoice.sOfficeCode);
                DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
                NpgsqlCommand cmdgettcdetails = new NpgsqlCommand("fetch_gettcdetails_clsinvoice");
                cmdgettcdetails.Parameters.AddWithValue("tc_capacity", Convert.ToString(objInvoice.sTcCapacity));
                cmdgettcdetails.Parameters.AddWithValue("storetype", Convert.ToString(objInvoice.sStoreType));
                cmdgettcdetails.Parameters.AddWithValue("office_code", Convert.ToString(objInvoice.sOfficeCode));
                cmdgettcdetails.Parameters.AddWithValue("tc_code", Convert.ToString(objInvoice.sTcCode));
                cmdgettcdetails.Parameters.AddWithValue("storeid", storeid);
                string res = objDatabse.StringGetValue(cmdgettcdetails);
                #endregion

                if (res != "")
                {

                    NpgsqlCommand cmd = new NpgsqlCommand("sp_gettcdetails");
                    cmd.Parameters.AddWithValue("stccode", objInvoice.sTcCode);
                    dt = objCon.FetchDataTable(cmd);

                    if (dt.Rows.Count > 0)
                    {
                        objInvoice.sTcCode = dt.Rows[0]["TC_CODE"].ToString();
                        objInvoice.sTcSlNo = dt.Rows[0]["TC_SLNO"].ToString();
                        objInvoice.sTcMake = dt.Rows[0]["TM_NAME"].ToString();
                        objInvoice.sTcCapacity = dt.Rows[0]["TC_CAPACITY"].ToString();
                        objInvoice.sTcRating = dt.Rows[0]["TC_RATING"].ToString();
                        objInvoice.smanufactureDate = dt.Rows[0]["TC_MANF_DATE"].ToString();
                        objInvoice.sStoreId = dt.Rows[0]["TC_STORE_ID"].ToString();
                    }
                }
                else
                {
                    objInvoice.sTcCode = "";
                }
                return objInvoice;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objInvoice;
            }
        }


        public object GetBasicDetails(clsInvoice objInvoice)
        {
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();

                NpgsqlCommand cmd = new NpgsqlCommand("sp_getbasicdetails");
                cmd.Parameters.AddWithValue("sindentid", objInvoice.sIndentId);
                dt = objCon.FetchDataTable(cmd);
                
                if (dt.Rows.Count > 0)
                {
                    objInvoice.sDTCName = dt.Rows[0]["DT_NAME"].ToString(); ;
                    objInvoice.sIndentDate = dt.Rows[0]["INDENTDDATE"].ToString();
                    objInvoice.sIndentCrby = dt.Rows[0]["USERNAME"].ToString();
                    objInvoice.sDtcFailId = dt.Rows[0]["DF_ID"].ToString();
                    objInvoice.sTcNewCapacity = dt.Rows[0]["WO_NEW_CAP"].ToString();
                    objInvoice.sTcCapacity = dt.Rows[0]["WO_DTC_CAP"].ToString();
                    objInvoice.sOldTcSlno = dt.Rows[0]["TC_SLNO"].ToString();
                    objInvoice.sIndentNo = dt.Rows[0]["TI_INDENT_NO"].ToString();
                    objInvoice.sOldTcCode = dt.Rows[0]["TC_CODE"].ToString();
                    objInvoice.sDTCId = dt.Rows[0]["DT_ID"].ToString();
                    objInvoice.sTCId = dt.Rows[0]["TC_ID"].ToString();
                    objInvoice.sAmount = dt.Rows[0]["WO_AMT"].ToString();
                    objInvoice.sFailDate = dt.Rows[0]["DF_DATE"].ToString();
                    objInvoice.sDTCCode = dt.Rows[0]["DT_CODE"].ToString();


                }
                return objInvoice;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objInvoice;
            }
        }

        public object GetInvoiceDetails(clsInvoice objInvoice)
        {

            try
            {
                DataTable dtInvoiceDetails = new DataTable();
                string strQry = string.Empty;
                
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getinvoicedetails");
                cmd.Parameters.AddWithValue("sinvoiceslno", objInvoice.sInvoiceSlNo);
                dtInvoiceDetails = objCon.FetchDataTable(cmd);


                if (dtInvoiceDetails.Rows.Count > 0)
                {
                    objInvoice.sInvoiceNo = Convert.ToString(dtInvoiceDetails.Rows[0]["IN_INV_NO"]);
                    objInvoice.sInvoiceDate = Convert.ToString(dtInvoiceDetails.Rows[0]["IN_DATE"]);
                    objInvoice.sAmount = Convert.ToString(dtInvoiceDetails.Rows[0]["IN_AMT"]);
                    objInvoice.sInvoiceDescription = Convert.ToString(dtInvoiceDetails.Rows[0]["IN_DESC"]);

                    objInvoice.sTcCode = Convert.ToString(dtInvoiceDetails.Rows[0]["TC_CODE"]);
                    objInvoice.sTcSlNo = Convert.ToString(dtInvoiceDetails.Rows[0]["TC_SLNO"]);
                    objInvoice.sTcMake = Convert.ToString(dtInvoiceDetails.Rows[0]["TM_NAME"]);
                    objInvoice.sTcCapacity = Convert.ToString(dtInvoiceDetails.Rows[0]["TC_CAPACITY"]);
                    objInvoice.sManualInvoiceNo = Convert.ToString(dtInvoiceDetails.Rows[0]["IN_MANUAL_INVNO"]);
                }


                return objInvoice;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objInvoice;

            }
        }
        public bool ValidateUpdate(string sInvoiceId)
        {
            try
            {
                //DataTable dt = new DataTable();
                //NpgsqlCommand = new NpgsqlCommand();
                //string sQry = " SELECT \"IN_TI_NO\" FROM \"TBLDTCINVOICE\", \"TBLTCREPLACE\" , \"TBLINDENT\" WHERE \"TR_IN_NO\"=\"IN_NO\" ";
                //sQry += " AND \"IN_TI_NO\"= \"TI_ID\" AND \"IN_NO\" =:sInvoiceId";
                //string sResult = string.Empty;
                //NpgsqlCommand.Parameters.AddWithValue("sInvoiceId", Convert.ToInt32(sInvoiceId));
                //sResult = objCon.get_value(sQry, NpgsqlCommand);

                #region Converted to sp
                string sResult = string.Empty;
                DataBase.DataBseConnection objdatabase = new DataBase.DataBseConnection(Constants.Password);
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsinvoice");
                cmd.Parameters.AddWithValue("p_key", "GET_IN_TI_NO");
                cmd.Parameters.AddWithValue("p_value", Convert.ToString(sInvoiceId));
                sResult = objdatabase.StringGetValue(cmd);
                #endregion
                if (sResult.Length > 0)
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
        public string GenerateInvoiceNoSA(string sOfficeCode, string sRoletype, string sRoleId)
        {
            try
            {
                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;
                NpgsqlCommand = new NpgsqlCommand();
                int stempLength;
                if (sRoletype == "1")
                {
                    sOfficeCode = sOfficeCode.Substring(0, Constants.Division);
                    sOfficeCode = clsStoreOffice.GetStoreID(sOfficeCode);
                }
                else if (sRoleId == Convert.ToString(ConfigurationManager.AppSettings["SupAdminRole"]))
                {
                    if (sOfficeCode.Length > 2)
                    {
                        sOfficeCode = clsStoreOffice.GetStoreID(sOfficeCode);
                    }
                }

                stempLength = sOfficeCode.Length;
                if (sOfficeCode.Length == 1)
                {
                    sOfficeCode = "0" + sOfficeCode;
                }
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", sOfficeCode);
                //string sInvoiceNo = objCon.get_value("SELECT COALESCE(MAX(CAST(\"INV_NO\" AS INT8)),0)+1  FROM \"VIEWINVOICE\" WHERE \"LOCCODE\" =:sOfficeCode", NpgsqlCommand);

                #region Converted to sp
                DataBase.DataBseConnection objdatabase = new DataBase.DataBseConnection(Constants.Password);
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsinvoice");
                cmd.Parameters.AddWithValue("p_key", "GET_INV_NO");
                cmd.Parameters.AddWithValue("p_value", sOfficeCode);
                string sInvoiceNo = objdatabase.StringGetValue(cmd);
                #endregion 

                if (stempLength == 1)
                {
                    //sInvoiceNo = "0" + sInvoiceNo;
                    if (sInvoiceNo == "1")
                    {
                        sInvoiceNo = sOfficeCode;
                    }
                    else
                    {
                        sInvoiceNo = "0" + sInvoiceNo;
                    }

                }

                if (sInvoiceNo.Length == 1)
                {
                    //2 digit Office Code
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        sFinancialYear = System.DateTime.Now.ToString("yy") + "" + System.DateTime.Now.AddYears(1).ToString("yy");
                    }
                    else
                    {
                        sFinancialYear = System.DateTime.Now.AddYears(-1).ToString("yy") + "" + System.DateTime.Now.ToString("yy");
                    }
                    if (sRoleId == Convert.ToString(ConfigurationManager.AppSettings["SupAdminRole"]))
                    {
                        if (sOfficeCode.Length == 1)
                        {
                            sOfficeCode = "0" + sOfficeCode;
                        }

                        sInvoiceNo = sOfficeCode + sFinancialYear + "00001";
                    }
                    else
                    {
                        sInvoiceNo = sOfficeCode + sFinancialYear + "00001";
                    }
                }
                else
                {
                    //2 digit Office Code
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                        {
                            sFinancialYear = System.DateTime.Now.ToString("yy") + "" + System.DateTime.Now.AddYears(1).ToString("yy");
                        }
                        if (sRoleId == Convert.ToString(ConfigurationManager.AppSettings["SupAdminRole"]))
                        {
                            if (sInvoiceNo.Length > 2)
                            {
                                if (sFinancialYear == sInvoiceNo.Substring(2, 4))
                                {
                                    return sInvoiceNo;
                                }
                            }
                            else
                            {
                                sInvoiceNo = sOfficeCode + sFinancialYear + "00001";
                            }
                        }
                        else
                        {
                            if (sFinancialYear == sInvoiceNo.Substring(2, 4))
                            {
                                return sInvoiceNo;
                            }
                            else
                            {
                                sInvoiceNo = sOfficeCode + sFinancialYear + "00001";
                            }
                        }

                    }
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) <= 03)
                    {
                        if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                        {
                            sFinancialYear = System.DateTime.Now.ToString("yy") + "" + System.DateTime.Now.AddYears(1).ToString("yy");
                        }
                        else
                        {
                            sFinancialYear = System.DateTime.Now.AddYears(-1).ToString("yy") + "" + System.DateTime.Now.ToString("yy");
                        }
                        if (sRoleId == Convert.ToString(ConfigurationManager.AppSettings["SupAdminRole"]))
                        {
                            if (sInvoiceNo.Length > 2)
                            {
                                if (sFinancialYear == sInvoiceNo.Substring(2, 4))
                                {
                                    return sInvoiceNo;
                                }
                            }
                            else
                            {
                                if (sOfficeCode.Length == 1)
                                {
                                    sOfficeCode = "0" + sOfficeCode;
                                }

                                sInvoiceNo = sOfficeCode + sFinancialYear + "00001";
                            }

                            return sInvoiceNo;
                        }


                    }
                }

                return sInvoiceNo;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }
        public string GenerateInvoiceNo(string sOfficeCode, string sRoletype)
        {
            try
            {
                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;
                NpgsqlCommand = new NpgsqlCommand();
                int stempLength;
                if (sRoletype == "1")
                {
                    sOfficeCode = sOfficeCode.Substring(0, Constants.Division);
                    sOfficeCode = clsStoreOffice.GetStoreID(sOfficeCode);
                }
                else if (sRoletype == "2")
                {
                    if (sOfficeCode.Length > 2)
                    {
                        sOfficeCode = clsStoreOffice.GetStoreID(sOfficeCode);
                    }
                }

                stempLength = sOfficeCode.Length;
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", sOfficeCode);
                //string sInvoiceNo = objCon.get_value("SELECT COALESCE(MAX(CAST(\"INV_NO\" AS INT8)),0)+1  FROM \"VIEWINVOICE\" WHERE \"LOCCODE\" =:sOfficeCode", NpgsqlCommand);

                #region Converted to sp
                DataBase.DataBseConnection objdatabase = new DataBase.DataBseConnection(Constants.Password);
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsinvoice");
                cmd.Parameters.AddWithValue("p_key", "GET_INV_NO");
                cmd.Parameters.AddWithValue("p_value", sOfficeCode);
                string sInvoiceNo = objdatabase.StringGetValue(cmd);
                #endregion

                if (stempLength == 1)
                {
                    if (sInvoiceNo == "1")
                    {
                        sInvoiceNo = sOfficeCode;
                    }
                }

                if (sInvoiceNo.Length == 1)
                {
                    //2 digit Office Code
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        sFinancialYear = System.DateTime.Now.ToString("yy") + "" + System.DateTime.Now.AddYears(1).ToString("yy");
                    }
                    else
                    {
                        sFinancialYear = System.DateTime.Now.AddYears(-1).ToString("yy") + "" + System.DateTime.Now.ToString("yy");
                    }
                    if (sRoletype == "2")
                    {
                        sInvoiceNo = sOfficeCode + sFinancialYear + "00001";
                    }
                    else
                    {
                        sInvoiceNo = sOfficeCode + sFinancialYear + "00001";
                    }
                }
                else
                {
                    //2 digit Office Code
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                        {
                            sFinancialYear = System.DateTime.Now.ToString("yy") + "" + System.DateTime.Now.AddYears(1).ToString("yy");
                        }
                        if (sRoletype == "2")
                        {
                            if (sInvoiceNo.Length > 2)
                            {
                                if (sFinancialYear == sInvoiceNo.Substring(2, 4))
                                {
                                    return sInvoiceNo;
                                }
                            }
                            else
                            {
                                sInvoiceNo = sOfficeCode + sFinancialYear + "00001";
                            }
                        }
                        else
                        {
                            if (sFinancialYear == sInvoiceNo.Substring(2, 4))
                            {
                                return sInvoiceNo;
                            }
                            else
                            {
                                sInvoiceNo = sOfficeCode + sFinancialYear + "00001";
                            }
                        }

                    }
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) <= 03)
                    {
                        if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                        {
                            sFinancialYear = System.DateTime.Now.ToString("yy") + "" + System.DateTime.Now.AddYears(1).ToString("yy");
                        }
                        else
                        {
                            sFinancialYear = System.DateTime.Now.AddYears(-1).ToString("yy") + "" + System.DateTime.Now.ToString("yy");
                        }
                        if (sRoletype == "2")
                        {
                            if (sInvoiceNo.Length > 2)
                            {
                                if (sFinancialYear == sInvoiceNo.Substring(2, 4))
                                {
                                    return sInvoiceNo;
                                }
                            }
                            else
                            {
                                sInvoiceNo = sOfficeCode + sFinancialYear + "00001";
                            }

                            return sInvoiceNo;
                        }


                    }
                }

                return sInvoiceNo;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }
        public string GenerateInvoiceNodtcinvoice(string sOfficeCode, string sRoletype)
        {
            try
            {
                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;
                NpgsqlCommand = new NpgsqlCommand();
                int stempLength;
                if (sRoletype == "1")
                {
                    sOfficeCode = sOfficeCode.Substring(0, Constants.Division);
                    sOfficeCode = clsStoreOffice.GetStoreID(sOfficeCode);
                }
                else if (sRoletype == "2")
                {
                    if (sOfficeCode.Length > 2)
                    {
                        sOfficeCode = clsStoreOffice.GetStoreID(sOfficeCode);
                    }
                }

                stempLength = sOfficeCode.Length;
                if (sOfficeCode.Length == 1)
                {
                    sOfficeCode = "0" + sOfficeCode;
                }
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", sOfficeCode);
                //string sInvoiceNo = objCon.get_value("select max(COALESCE(CAST(\"IN_INV_NO\" AS INT8),1))+1 from \"TBLDTCINVOICE\" where substr (\"IN_INV_NO\",1,2)=:sOfficeCode", NpgsqlCommand);

                #region
                DataBase.DataBseConnection objdatabase = new DataBase.DataBseConnection(Constants.Password);
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsinvoice");
                cmd.Parameters.AddWithValue("p_key", "GET_MAXINV_NO");
                cmd.Parameters.AddWithValue("p_value", sOfficeCode);
                string sInvoiceNo = objdatabase.StringGetValue(cmd);
                #endregion

                if (stempLength == 1)
                {
                    if (sInvoiceNo == "1")
                    {
                        sInvoiceNo = sOfficeCode;
                    }
                    else
                    {
                        sInvoiceNo = "0" + sInvoiceNo;
                    }

                }

                if (sInvoiceNo.Length == 1)
                {
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        sFinancialYear = System.DateTime.Now.ToString("yy") + "" + System.DateTime.Now.AddYears(1).ToString("yy");
                    }
                    else
                    {
                        sFinancialYear = System.DateTime.Now.AddYears(-1).ToString("yy") + "" + System.DateTime.Now.ToString("yy");
                    }
                    if (sRoletype == "2")
                    {
                        if (sOfficeCode.Length == 1)
                        {
                            sOfficeCode = "0" + sOfficeCode;
                        }

                        sInvoiceNo = sOfficeCode + sFinancialYear + "00001";
                    }
                    else
                    {
                        sInvoiceNo = sOfficeCode + sFinancialYear + "00001";
                    }
                }
                else
                {
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                        {
                            sFinancialYear = System.DateTime.Now.ToString("yy") + "" + System.DateTime.Now.AddYears(1).ToString("yy");
                        }
                        if (sRoletype == "2")
                        {
                            if (sInvoiceNo.Length > 2)
                            {
                                if (sFinancialYear == sInvoiceNo.Substring(2, 4))
                                {
                                    return sInvoiceNo;
                                }
                                else
                                {
                                    sInvoiceNo = sOfficeCode + sFinancialYear + "00001";
                                }
                            }
                            else
                            {
                                sInvoiceNo = sOfficeCode + sFinancialYear + "00001";
                            }
                        }
                        else
                        {
                            if (sFinancialYear == sInvoiceNo.Substring(2, 4))
                            {
                                return sInvoiceNo;
                            }
                            else
                            {
                                sInvoiceNo = sOfficeCode + sFinancialYear + "00001";
                            }
                        }

                    }
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) <= 03)
                    {
                        if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                        {
                            sFinancialYear = System.DateTime.Now.ToString("yy") + "" + System.DateTime.Now.AddYears(1).ToString("yy");
                        }
                        else
                        {
                            sFinancialYear = System.DateTime.Now.AddYears(-1).ToString("yy") + "" + System.DateTime.Now.ToString("yy");
                        }
                        if (sRoletype == "2")
                        {
                            if (sInvoiceNo.Length > 2)
                            {
                                if (sFinancialYear == sInvoiceNo.Substring(2, 4))
                                {
                                    return sInvoiceNo;
                                }
                            }
                            else
                            {
                                if (sOfficeCode.Length == 1)
                                {
                                    sOfficeCode = "0" + sOfficeCode;
                                }

                                sInvoiceNo = sOfficeCode + sFinancialYear + "00001";
                            }

                            return sInvoiceNo;
                        }
                    }
                }

                return sInvoiceNo;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }


        public string GenerateInvoiceNoForRepairerTC(string sOfficeCode, string sRoletype)
        {
            try
            {
                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;
                NpgsqlCommand = new NpgsqlCommand();
                int stempLength;
                if (sRoletype == "1")
                {
                    sOfficeCode = sOfficeCode.Substring(0, Constants.Division);
                    sOfficeCode = clsStoreOffice.GetStoreID(sOfficeCode);
                }
                else if (sRoletype == "2")
                {
                    if (sOfficeCode.Length > 2)
                    {
                        sOfficeCode = clsStoreOffice.GetStoreID(sOfficeCode);
                    }
                }
                string off = clsStoreOffice.Getofficecode(sOfficeCode);

                stempLength = sOfficeCode.Length;
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", off);
                //string sInvoiceNo = objCon.get_value("SELECT COALESCE(MAX(CAST(\"RI_INVOICE_NO\" AS INT8)),0)+1  FROM \"TBLREPAIRERINVOICE\" WHERE SUBSTR(cast(\"RI_INVOICE_NO\" as text),0,4) =:sOfficeCode", NpgsqlCommand);

                #region Converted to sp
                DataBase.DataBseConnection objdatabase = new DataBase.DataBseConnection(Constants.Password);
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsinvoice");
                cmd.Parameters.AddWithValue("p_key", "GET_RI_INVOICE_NO");
                cmd.Parameters.AddWithValue("p_value", off);
                string sInvoiceNo = objdatabase.StringGetValue(cmd);
                #endregion
                if (stempLength == 1)
                {
                    if (sInvoiceNo == "1")
                    {
                        sInvoiceNo = off;
                    }

                }

                if (sInvoiceNo.Length == 1)
                {

                    //2 digit Office Code
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        sFinancialYear = System.DateTime.Now.ToString("yy") + "" + System.DateTime.Now.AddYears(1).ToString("yy");
                    }
                    else
                    {
                        sFinancialYear = System.DateTime.Now.AddYears(-1).ToString("yy") + "" + System.DateTime.Now.ToString("yy");
                    }
                    if (sRoletype == "2")
                    {
                        sInvoiceNo = off + sFinancialYear + "00001";
                    }
                    else
                    {
                        sInvoiceNo = off + sFinancialYear + "00001";
                    }
                }
                else
                {
                    //2 digit Office Code
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                        {
                            sFinancialYear = System.DateTime.Now.ToString("yy") + "" + System.DateTime.Now.AddYears(1).ToString("yy");
                        }
                        if (sRoletype == "2")
                        {
                            if (sInvoiceNo.Length > 3)
                            {
                                if (sFinancialYear == sInvoiceNo.Substring(3, 4))
                                {
                                    return sInvoiceNo;
                                }
                                else
                                {
                                    sInvoiceNo = off + sFinancialYear + "00001";
                                }
                            }
                            else
                            {
                                sInvoiceNo = off + sFinancialYear + "00001";
                            }
                        }
                        else
                        {
                            string a = sInvoiceNo.Substring(1, 4);
                            if (sFinancialYear == sInvoiceNo.Substring(3, 4))
                            {
                                return sInvoiceNo;
                            }
                            else
                            {
                                sInvoiceNo = off + sFinancialYear + "00001";
                            }
                        }

                    }
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) <= 03)
                    {
                        if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                        {
                            sFinancialYear = System.DateTime.Now.ToString("yy") + "" + System.DateTime.Now.AddYears(1).ToString("yy");
                        }
                        else
                        {
                            sFinancialYear = System.DateTime.Now.AddYears(-1).ToString("yy") + "" + System.DateTime.Now.ToString("yy");
                        }
                        if (sRoletype == "2")
                        {
                            if (sInvoiceNo.Length > 2)
                            {
                                if (sFinancialYear == sInvoiceNo.Substring(3, 4))
                                {
                                    return sInvoiceNo;
                                }
                            }
                            else
                            {
                                sInvoiceNo = off + sFinancialYear + "00001";
                            }

                            return sInvoiceNo;
                        }


                    }
                }

                return sInvoiceNo;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }
        public string GenerateRVNoForRepairerTCReceive(string sOfficeCode, string sRoletype)
        {
            try
            {
                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;
                NpgsqlCommand = new NpgsqlCommand();
                int stempLength;
                if (sRoletype == "1")
                {
                    sOfficeCode = sOfficeCode.Substring(0, Constants.Division);
                    sOfficeCode = clsStoreOffice.GetStoreID(sOfficeCode);
                }
                else if (sRoletype == "2")
                {
                    if (sOfficeCode.Length > 2)
                    {
                        sOfficeCode = clsStoreOffice.GetStoreID(sOfficeCode);
                    }
                }

                stempLength = sOfficeCode.Length;
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", sOfficeCode);
                //string sRVNum = objCon.get_value("SELECT COALESCE(MAX(CAST(\"RSD_RV_NO\" AS INT8)),0)+1  FROM \"TBLREPAIRSENTDETAILS\" WHERE SUBSTR(cast(\"RSD_RV_NO\" as text),0,3) =:sOfficeCode", NpgsqlCommand);

                #region Converted to sp
                DataBase.DataBseConnection objdatabase = new DataBase.DataBseConnection(Constants.Password);
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsinvoice");
                cmd.Parameters.AddWithValue("p_key", "GET_RSD_RV_NO");
                cmd.Parameters.AddWithValue("p_value", sOfficeCode);
                string sRVNum = objdatabase.StringGetValue(cmd);
                #endregion
                if (stempLength == 1)
                {
                    if (sRVNum == "1")
                    {
                        sRVNum = sOfficeCode;
                    }

                }

                if (sRVNum.Length == 1)
                {

                    //2 digit Office Code
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        sFinancialYear = System.DateTime.Now.ToString("yy") + "" + System.DateTime.Now.AddYears(1).ToString("yy");
                    }
                    else
                    {
                        sFinancialYear = System.DateTime.Now.AddYears(-1).ToString("yy") + "" + System.DateTime.Now.ToString("yy");
                    }
                    if (sRoletype == "2")
                    {
                        sRVNum = sOfficeCode + sFinancialYear + "00001";
                    }
                    else
                    {
                        sRVNum = sOfficeCode + sFinancialYear + "00001";
                    }
                }
                else
                {
                    //2 digit Office Code
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                        {
                            sFinancialYear = System.DateTime.Now.ToString("yy") + "" + System.DateTime.Now.AddYears(1).ToString("yy");
                        }
                        if (sRoletype == "2")
                        {
                            if (sRVNum.Length > 2)
                            {
                                String s = sRVNum.Substring(2, 4);
                                if (sOfficeCode.Length > 1)
                                {
                                    if (sFinancialYear == sRVNum.Substring(2, 4))
                                    {
                                        return sRVNum;
                                    }
                                    else
                                    {
                                        sRVNum = sOfficeCode + sFinancialYear + "00001";
                                    }
                                }
                                else
                                {
                                    if (sFinancialYear == sRVNum.Substring(1, 4))
                                    {
                                        return sRVNum;
                                    }
                                    else
                                    {
                                        sRVNum = sOfficeCode + sFinancialYear + "00001";
                                    }
                                }
                            }
                            else
                            {
                                sRVNum = sOfficeCode + sFinancialYear + "00001";
                            }
                        }
                        else
                        {
                            string a = sRVNum.Substring(1, 4);
                            if (sFinancialYear == sRVNum.Substring(1, 4))
                            {
                                return sRVNum;
                            }
                            else
                            {
                                sRVNum = sOfficeCode + sFinancialYear + "00001";
                            }
                        }

                    }
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) <= 03)
                    {
                        if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                        {
                            sFinancialYear = System.DateTime.Now.ToString("yy") + "" + System.DateTime.Now.AddYears(1).ToString("yy");
                        }
                        else
                        {
                            sFinancialYear = System.DateTime.Now.AddYears(-1).ToString("yy") + "" + System.DateTime.Now.ToString("yy");
                        }
                        if (sRoletype == "2")
                        {
                            if (sRVNum.Length > 2)
                            {
                                if (sFinancialYear == sRVNum.Substring(2, 4))
                                {
                                    return sRVNum;
                                }
                            }
                            else
                            {
                                sRVNum = sOfficeCode + sFinancialYear + "00001";
                            }

                            return sRVNum;
                        }


                    }
                }

                return sRVNum;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }


        #region NewDTC

        public DataTable LoadAllNewDTCInvoice(clsInvoice objInvoice)
        {
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("sp_loadallnewdtcinvoice");
                cmd.Parameters.AddWithValue("sofficecode", objInvoice.sOfficeCode);
                dt = objCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable LoadAlreadyNewDTCInvoice(clsInvoice objInvoice)
        {
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("sp_loadalreadynewdtcinvoice");
                cmd.Parameters.AddWithValue("sofficecode", objInvoice.sOfficeCode);
                dt = objCon.FetchDataTable(cmd);

                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        #endregion


        public clsInvoice GetGatePassDetials(clsInvoice objInvoice)
        {
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getgatepassdetials");
                cmd.Parameters.AddWithValue("sinvoiceno", objInvoice.sInvoiceNo);
                dt = objCon.FetchDataTable(cmd);


                if (dt.Rows.Count > 0)
                {
                    objInvoice.sGatePassId = Convert.ToString(dt.Rows[0]["GP_ID"]);
                    objInvoice.sChallenNo = Convert.ToString(dt.Rows[0]["GP_CHALLEN_NO"]);
                    objInvoice.sVehicleNumber = Convert.ToString(dt.Rows[0]["GP_VEHICLE_NO"]);
                    objInvoice.sReceiptientName = Convert.ToString(dt.Rows[0]["GP_RECIEPIENT_NAME"]);
                }

                return objInvoice;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objInvoice;
            }
        }




        public string[] SaveUpdateGatePassDetails(clsInvoice objInvoice)
        {
            string[] Arr = new string[2];
            string[] strArray = new string[2];
            OleDbDataReader dr;
            string strQry = string.Empty;
            try
            {

                if (objInvoice.sTcCode == null || objInvoice.sTcCode == "NULL")
                {
                    objInvoice.sTcCode = "0.0";
                }
                if (objInvoice.sIssueQty == null || objInvoice.sIssueQty == "NULL")
                {
                    objInvoice.sIssueQty = "";
                }

                if (objInvoice.sDTCCode == null || objInvoice.sDTCCode == "NULL")
                {
                    objInvoice.sDTCCode = "";
                }
                NpgsqlCommand = new NpgsqlCommand();
                // coded by Rudra on 12-03-2020 due to no use of Query 



                NpgsqlCommand cmd = new NpgsqlCommand("sp_saveupdategatepassdetails");
                cmd.Parameters.AddWithValue("schallenno", objInvoice.sChallenNo.ToUpper());
                cmd.Parameters.AddWithValue("sgatepassid", objInvoice.sGatePassId);
                cmd.Parameters.AddWithValue("svehiclenumber", objInvoice.sVehicleNumber.ToUpper());
                cmd.Parameters.AddWithValue("sreceiptientname", objInvoice.sReceiptientName.ToUpper());
                cmd.Parameters.AddWithValue("sinvoiceno", objInvoice.sInvoiceNo);
                cmd.Parameters.AddWithValue("stccode", objInvoice.sTcCode);
                cmd.Parameters.AddWithValue("sdtccode", objInvoice.sDTCCode);
                cmd.Parameters.AddWithValue("screatedby", objInvoice.sCreatedBy);
                cmd.Parameters.AddWithValue("sissueqty", objInvoice.sIssueQty == "" ? "0" : objInvoice.sIssueQty);
                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                strArray[0] = "op_id";
                strArray[1] = "msg";
                Arr = objCon.Execute(cmd, strArray, 2);
                return Arr;


            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;


            }
        }
        public string[] SaveUpdateGatePassDetailsforstoreinvoice(clsInvoice objInvoice)
        {
            string[] Arr = new string[2];
            string[] strArray = new string[2];
            OleDbDataReader dr;
            string strQry = string.Empty;
            try
            {

                if (objInvoice.sTcCode == null || objInvoice.sTcCode == "NULL")
                {
                    objInvoice.sTcCode = "0.0";
                }
                if (objInvoice.sIssueQty == null || objInvoice.sIssueQty == "NULL")
                {
                    objInvoice.sIssueQty = "";
                }

                if (objInvoice.sDTCCode == null || objInvoice.sDTCCode == "NULL")
                {
                    objInvoice.sDTCCode = "";
                }
                NpgsqlCommand = new NpgsqlCommand();

                NpgsqlCommand cmd = new NpgsqlCommand("sp_saveupdategatepassdetailsforstrinv");
                cmd.Parameters.AddWithValue("schallenno", objInvoice.sChallenNo.ToUpper());
                cmd.Parameters.AddWithValue("sgatepassid", objInvoice.sGatePassId);
                cmd.Parameters.AddWithValue("svehiclenumber", objInvoice.sVehicleNumber.ToUpper());
                cmd.Parameters.AddWithValue("sreceiptientname", objInvoice.sReceiptientName.ToUpper());
                cmd.Parameters.AddWithValue("sinvoiceno", objInvoice.sInvoiceNo);
                cmd.Parameters.AddWithValue("stccode", objInvoice.sTcCode);
                cmd.Parameters.AddWithValue("sdtccode", objInvoice.sDTCCode);
                cmd.Parameters.AddWithValue("screatedby", objInvoice.sCreatedBy);
                cmd.Parameters.AddWithValue("sissueqty", objInvoice.sIssueQty);
                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd.Parameters.Add("pk_id", NpgsqlDbType.Text);
                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                cmd.Parameters["pk_id"].Direction = ParameterDirection.Output;
                strArray[0] = "op_id";
                strArray[1] = "msg";
                strArray[2] = "pk_id";
                Arr = objCon.Execute(cmd, strArray, 2);
                return Arr;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;


            }
        }

        #region WorkFlow XML

        public clsInvoice GetInvoiceDetailsFromXML(clsInvoice objInvoice)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable dtInvoiceDetails = new DataTable();

                dtInvoiceDetails = objApproval.GetDatatableFromXML(objInvoice.sWFDataId);
                if (dtInvoiceDetails.Rows.Count > 0)
                {
                    objInvoice.sInvoiceNo = Convert.ToString(dtInvoiceDetails.Rows[0]["IN_INV_NO"]);
                    objInvoice.sInvoiceDate = Convert.ToString(dtInvoiceDetails.Rows[0]["IN_DATE"]);
                    objInvoice.sAmount = Convert.ToString(dtInvoiceDetails.Rows[0]["IN_AMT"]);
                    objInvoice.sInvoiceDescription = Convert.ToString(dtInvoiceDetails.Rows[0]["IN_DESC"]);

                    objInvoice.sTcCode = Convert.ToString(dtInvoiceDetails.Rows[0]["TC_CODE"]);
                    objInvoice.sTcSlNo = Convert.ToString(dtInvoiceDetails.Rows[0]["TC_SLNO"]);
                    objInvoice.sTcMake = Convert.ToString(dtInvoiceDetails.Rows[0]["TM_NAME"]);
                    objInvoice.sTcCapacity = Convert.ToString(dtInvoiceDetails.Rows[0]["TC_CAPACITY"]);
                }
                return objInvoice;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objInvoice;
            }
        }

        #endregion
        public string GenerateEstimationNoForRepairer(string sOfficeCode, string sRoletype)
        {
            try
            {
                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;
                NpgsqlCommand = new NpgsqlCommand();
                int stempLength;
                string off = sOfficeCode;

                sOfficeCode = sOfficeCode.Substring(0, Constants.Division);

                stempLength = sOfficeCode.Length;
                string sInvoiceNo = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();

                //NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", sOfficeCode);
                //sInvoiceNo = objCon.get_value("select COALESCE(MAX(CAST(\"RESTD_EST_NO\" AS INT8)),0)+1 from \"TBLREPAIRERESTIMATIONDETAILS\" where SUBSTR(cast(\"RESTD_EST_NO\" as text),0,4) =:sOfficeCode", NpgsqlCommand);

                #region Converted to sp
                DataBase.DataBseConnection objdatabase = new DataBase.DataBseConnection(Constants.Password);
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsinvoice");
                cmd.Parameters.AddWithValue("p_key", "GET_RESTD_EST_NO");
                cmd.Parameters.AddWithValue("p_value", sOfficeCode);
                sInvoiceNo = objdatabase.StringGetValue(cmd);
                #endregion
                if (stempLength == 1)
                {
                    if (sInvoiceNo == "1")
                    {
                        sInvoiceNo = sOfficeCode;
                    }
                }

                if (sInvoiceNo.Length == 1)
                {

                    //2 digit Office Code
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        sFinancialYear = System.DateTime.Now.ToString("yy") + "" + System.DateTime.Now.AddYears(1).ToString("yy");
                    }
                    else
                    {
                        sFinancialYear = System.DateTime.Now.AddYears(-1).ToString("yy") + "" + System.DateTime.Now.ToString("yy");
                    }
                    if (sRoletype == "2")
                    {
                        sInvoiceNo = sOfficeCode + sFinancialYear + "00001";
                    }
                    else
                    {
                        sInvoiceNo = sOfficeCode + sFinancialYear + "00001";
                    }
                }
                else
                {
                    //2 digit Office Code
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                    {
                        if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                        {
                            sFinancialYear = System.DateTime.Now.ToString("yy") + "" + System.DateTime.Now.AddYears(1).ToString("yy");
                        }
                        if (sRoletype == "2")
                        {
                            if (sInvoiceNo.Length > 2)
                            {
                                string a = sInvoiceNo.Substring(3, 4);
                                if (sFinancialYear == sInvoiceNo.Substring(3, 4))
                                {
                                    return sInvoiceNo;
                                }
                            }
                            else
                            {
                                sInvoiceNo = sOfficeCode + sFinancialYear + "00001";
                            }
                        }
                        else
                        {

                            string a = sInvoiceNo.Substring(3, 4);
                            if (sFinancialYear == sInvoiceNo.Substring(3, 4))
                            {
                                return sInvoiceNo;
                            }
                            else
                            {
                                sInvoiceNo = sOfficeCode + sFinancialYear + "00001";
                            }

                        }
                    }
                    if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) <= 03)
                    {
                        if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                        {
                            sFinancialYear = System.DateTime.Now.ToString("yy") + "" + System.DateTime.Now.AddYears(1).ToString("yy");
                        }
                        else
                        {
                            sFinancialYear = System.DateTime.Now.AddYears(-1).ToString("yy") + "" + System.DateTime.Now.ToString("yy");
                        }
                        if (sRoletype == "2")
                        {
                            if (sInvoiceNo.Length > 2)
                            {
                                if (sFinancialYear == sInvoiceNo.Substring(3, 4))
                                {
                                    return sInvoiceNo;
                                }
                            }
                            else
                            {
                                sInvoiceNo = sOfficeCode + sFinancialYear + "00001";
                            }

                            return sInvoiceNo;
                        }


                    }
                }

                return sInvoiceNo;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }
        public string GenerateWorkorderNoForRepairer(string sOfficeCode, string sRoletype)
        {
            try
            {
                string sworkorderno = string.Empty;
                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;
                NpgsqlCommand = new NpgsqlCommand();
                int stempLength;

                string acccode = "74.1177";
                stempLength = sOfficeCode.Length;

                if (Convert.ToInt32(System.DateTime.Now.ToString("MM")) > 03)
                {
                    sFinancialYear = System.DateTime.Now.ToString("yyyy") + "-" + System.DateTime.Now.AddYears(1).ToString("yyyy");
                }
                else
                {
                    sFinancialYear = System.DateTime.Now.AddYears(-1).ToString("yyyy") + "-" + System.DateTime.Now.ToString("yyyy");
                }

                DataBase.DataBseConnection objdatabase = new DataBase.DataBseConnection(Constants.Password);



                //string DIVLOCCODE = objCon.get_value("SELECT \"DIV_LOCATION_CODE\" FROM \"TBLDIVISION\" WHERE \"DIV_CODE\"='" + sOfficeCode + "' ", NpgsqlCommand);

                #region Converted to sp
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsinvoice");
                cmd.Parameters.AddWithValue("p_key", "GET_DIV_LOCATION_CODE");
                cmd.Parameters.AddWithValue("p_value", Convert.ToString(sOfficeCode));
                string DIVLOCCODE = objdatabase.StringGetValue(cmd);
                #endregion
                string swono = DIVLOCCODE + "/" + acccode + "/" + sFinancialYear + "/";

                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", Convert.ToInt32(sOfficeCode));
                //sMaxNo = objCon.get_value("select COALESCE(MAX(CAST(\"RWO_AUTO_SLNO\" AS INT8)),0)+1 from \"TBLREPAIRERWORKORDER\" where \"RWO_OFF_CODE\"=:sOfficeCode ", NpgsqlCommand);

                #region Converted to sp
                NpgsqlCommand cmd1 = new NpgsqlCommand("fetch_getvalue_clsinvoice");
                cmd1.Parameters.AddWithValue("p_key", "GET_RWO_AUTO_SLNO");
                cmd1.Parameters.AddWithValue("p_value", Convert.ToString(sOfficeCode));
                sMaxNo = objdatabase.StringGetValue(cmd1);
                #endregion
                int max = Convert.ToInt32(sMaxNo);
                sworkorderno = swono + max;
                bool flag = false;
                while (flag == false)
                {
                    sworkorderno = swono + max;
                    //string xmlwono = objCon.get_value("select * from (select replace(cast(unnest(xpath('(./TBLREPAIRERWORKORDER/RWO_NO)/text()', \"WFO_DATA\"::XML))::text as text), '''', '') AS \"WO_NO\" from \"TBLWFODATA\"  left join \"TBLWORKFLOWOBJECTS\" on cast(\"WFO_ID\" as text)=\"WO_WFO_ID\"  where \"WO_BO_ID\"=20 and \"WFO_BO_ID\"=20 and \"WO_RECORD_ID\" <0 and \"WO_APPROVE_STATUS\"=0 and cast(\"WO_OFFICE_CODE\" as text)='" + sOfficeCode + "' )a   where   a.\"WO_NO\" ='" + sworkorderno + "' ", NpgsqlCommand);

                    #region Converted to sp
                    NpgsqlCommand cmdxml = new NpgsqlCommand("fetch_xmlwono_clsinvoice");
                    cmdxml.Parameters.AddWithValue("wono", Convert.ToString(sworkorderno));
                    cmdxml.Parameters.AddWithValue("officecode", Convert.ToString(sOfficeCode));
                    string xmlwono = objdatabase.StringGetValue(cmdxml);
                    #endregion

                    if (xmlwono == "")
                    {
                        flag = true;
                        sworkorderno = swono + max;
                        return sworkorderno;
                    }
                    else
                    {
                        max = max + 1;
                    }

                }

                return sworkorderno;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }

    }
}
