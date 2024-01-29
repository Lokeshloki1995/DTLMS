using IIITS.DTLMS.BL.DataBase;
using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IIITS.DTLMS.BL.Query
{
    public class clsFailureTransactionDelete
    {
        PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
        DataBseConnection ObjBseCon = new DataBseConnection(Constants.Password);

        public string DtcCode { get; set; }
        public string DtrCode { get; set; }
        public string TicketNumber { get; set; }
        public string BoId { get; set; }
        public bool isReparerCenterd { get; set; }

        public string DfLocCode { get; set; }
        public string DfId { get; set; }
        public string DfStatusFlag { get; set; }
        public string DfEquipmentId { get; set; }
        public string TransDtcCode { get; set; }

        public string EstId { get; set; }
        public string EstDetId { get; set; }
        public string WoSlno { get; set; }
        public string TiId { get; set; }
        public string InIno { get; set; }
        public string TdId { get; set; }
        public string TdTcNo { get; set; }
        public string TrId { get; set; }


        StringBuilder QryStringBuilder { get; set; } = new StringBuilder();

        DataTable dtMajor { get; set; } = new DataTable();
        DataTable DtWorkFlowObjects { get; set; } = new DataTable();
        DataTable DtWOObjectAuto { get; set; } = new DataTable();
        DataTable DtDTCTran { get; set; } = new DataTable();
        DataTable DtDTCFailure { get; set; } = new DataTable();
        DataTable DtTransaction { get; set; } = new DataTable();
        DataTable DtTransDtcMap { get; set; } = new DataTable();
        /// <summary>
        /// For Load the faiure details
        /// </summary>
        /// <param name="objFailureDtcDetails"></param>
        /// <returns></returns>
        public DataTable LoadFailureDtcDetails(clsFailureTransactionDelete Obj)
        {
            DataTable dt = new DataTable("failure_details");
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_failure_details");
                cmd.Parameters.AddWithValue("p_faileddtccode", Obj.DtcCode);
                dt = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name,
                   MethodBase.GetCurrentMethod().Name);
                return dt;
            }
            return dt;
        }
        /// <summary>
        /// Check RV and Invoice Completed or not        
        /// </summary>
        /// <param name="objValidationMajorFailDelete"></param>
        /// <returns></returns>
        public DataTable ValidationMajorFailDelete(clsFailureTransactionDelete Obj, DataBseConnection ObjBseCon = null)
        {
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_major_failure_details_delete");
                cmd.Parameters.AddWithValue("p_faileddtccode", Convert.ToString(Obj.DtcCode));
                if (ObjBseCon != null)
                {
                    dt = ObjBseCon.FetchDataTable(cmd);
                }
                else
                {
                    dt = ObjCon.FetchDataTable(cmd);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name,
                   MethodBase.GetCurrentMethod().Name);
                return dt;
            }
            return dt;
        }
        /// <summary>
        /// Does RV Record as pushed to Reparer CenterRecord or not.
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public bool GetisReparerCenterdRecord(clsFailureTransactionDelete Obj)
        {

            bool isReparerCenterd = false;
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_is_reparer_centerd");
                cmd.Parameters.AddWithValue("p_dtrcode", Convert.ToString(Obj.DtrCode));
                string Result = ObjBseCon.StringGetValue(cmd);

                if (Result == "TRUE")
                {
                    isReparerCenterd = true;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name,
                   MethodBase.GetCurrentMethod().Name);
                return false;
            }
            return isReparerCenterd;
        }

        /// <summary>
        /// Delete Failure Transaction Record Details
        /// </summary>
        /// <param name="objValidationMajorFailDelete"></param>
        /// <returns></returns>
        public string[] DeleteFailureTransactionDetails(clsFailureTransactionDelete Obj)
        {
            string[] Arr = new string[3];

            ObjBseCon.BeginTransaction();
            try
            {
                switch (Obj.BoId)
                {
                    case "9": // Failure
                        Failure:
                        Arr = new string[3];
                        Arr = Obj.DeleteFailureRecordDetails(Obj, ObjBseCon);
                        ObjBseCon.CommitTransaction();

                        if (Arr[0] == "0")
                        {
                            Arr = new string[2];
                            Arr = Obj.UpdateBACKENDACTIVITYDETAILS(Obj);
                        }
                        break;

                    case "45": // Estimation 
                        Estimation:
                        Arr = new string[3];
                        Arr = Obj.DeleteEstimationRecordDetails(Obj, ObjBseCon);
                        goto Failure;

                    case "11": // Workorder
                        Workorder:
                        Arr = new string[3];
                        Arr = Obj.DeleteWorkorderRecordDetails(Obj, ObjBseCon);
                        goto Estimation;

                    case "13": // Invoice
                        Invoice:
                        Arr = new string[3];
                        Arr = Obj.DeleteInvoiceRecordDetails(Obj, ObjBseCon);
                        goto Workorder;

                    default: // DecOm/RI & CR
                        Arr = new string[3];
                        Arr = Obj.DeleteTCReplaceRecordDetails(Obj, ObjBseCon);
                        goto Invoice;
                }
            }
            catch (Exception ex)
            {
                ObjBseCon.RollBackTrans();
                clsException.LogError(ex.StackTrace, ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name,
                   MethodBase.GetCurrentMethod().Name);
                Arr[0] = "";
                Arr[1] = "";
                Arr[2] = "";
                return Arr;
            }
            return Arr;
        }
        /// <summary>
        /// Get Boid PENDINGTRANSACTION
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public clsFailureTransactionDelete GetBoid(clsFailureTransactionDelete Obj)
        {
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_pendingtransaction_boid");
                cmd.Parameters.AddWithValue("p_dtcCode", Obj.DtcCode);
                Obj.BoId = ObjBseCon.StringGetValue(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name,
                   MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
            return Obj;
        }
        /// <summary>
        /// Update BACKENDACTIVITYDETAILS Table
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public string[] UpdateBACKENDACTIVITYDETAILS(clsFailureTransactionDelete Obj)
        {
            string[] Arr = new string[2];
            string Qry = string.Empty;
            try
            {
                Obj.QryStringBuilder = Obj.QryStringBuilder.Replace("'", "''");

                Qry = " SELECT max(\"BAD_ID\")+1 from \"TBLBACKENDACTIVITYDETAILS\" ";
                string MaxBadId = ObjCon.get_value(Qry);

                string Description = " Deleted Failure Transaction of the DTC " + Obj.DtcCode + " and DTR  " + Obj.DtrCode + " ";
                Qry = "  INSERT into \"TBLBACKENDACTIVITYDETAILS\"(\"BAD_BM_ID\",";
                Qry += "\"BAD_DESCRITION\",\"BAD_DTCCODE\",\"BAD_DTRCODE\",\"BAD_QUERY\",\"BAD_TICKETNUMBER\") ";
                Qry += "  VALUES (10, '" + Description + "',";
                Qry += "'" + Obj.DtcCode + "','" + Obj.DtrCode + "',  '" + Convert.ToString(Obj.QryStringBuilder) + "' , '" + Obj.TicketNumber + "') ";
                ObjCon.ExecuteQry(Qry);

                Arr[0] = "Deleted Successfully ";
                Arr[1] = "0";

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name,
                         MethodBase.GetCurrentMethod().Name);
                return Arr;
            }
            return Arr;
        }
        /// <summary>
        /// Delete Failure Record Details
        /// this is completed.
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public string[] DeleteFailureRecordDetails(clsFailureTransactionDelete Obj, DataBseConnection ObjBseCon)
        {
            string[] Arr = new string[3];
            //DataTable Dt = new DataTable();
            string strQry = string.Empty;
            string dfStatusFlag = string.Empty;
            string woId = string.Empty;
            string dctId = string.Empty;
            string woaId = string.Empty;
            string drtId = string.Empty;
            string tmId = string.Empty;

            DataTable dt = new DataTable();
            DataTable dtWo = new DataTable();
            DataTable dtWfo = new DataTable();
            try
            {
                Obj.dtMajor = ValidationMajorFailDelete(Obj, ObjBseCon);
                if (dtMajor.Rows.Count > 0)
                {
                    Obj.DfEquipmentId = Convert.ToString(dtMajor.Rows[0]["DF_EQUIPMENT_ID"]);
                    Obj.DfLocCode = Convert.ToString(dtMajor.Rows[0]["DF_LOC_CODE"]);
                    Obj.DfId = Convert.ToString(dtMajor.Rows[0]["DF_ID"]);
                    Obj.EstId = Convert.ToString(dtMajor.Rows[0]["EST_ID"]);
                    Obj.EstDetId = Convert.ToString(dtMajor.Rows[0]["EST_DETAILS_ID"]);
                    Obj.WoSlno = Convert.ToString(dtMajor.Rows[0]["WO_SLNO"]);
                    Obj.TiId = Convert.ToString(dtMajor.Rows[0]["TI_ID"]);
                    Obj.InIno = Convert.ToString(dtMajor.Rows[0]["IN_NO"]);
                    Obj.TdId = Convert.ToString(dtMajor.Rows[0]["TD_ID"]);
                    Obj.TdTcNo = Convert.ToString(dtMajor.Rows[0]["TD_TC_NO"]);
                    Obj.TrId = Convert.ToString(dtMajor.Rows[0]["TR_ID"]);
                    Obj.TransDtcCode = Convert.ToString(dtMajor.Rows[0]["TRANS_DTC_CODE"]);
                }

                if ((Obj.DfId ?? "").Length > 0) // failure Completed only
                {
                    strQry = "SELECT \"WO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" IN (9) AND ";
                    strQry += "\"WO_RECORD_ID\"='" + Obj.DfId + "' AND \"WO_DATA_ID\"='" + Obj.DtcCode + "' AND ";
                    strQry += "\"WO_DESCRIPTION\" LIKE '%" + Obj.DtcCode + "%'   ";
                    dtWo = ObjBseCon.FetchDataTable(strQry);

                    strQry = "SELECT max(\"WO_ID\") as \"WO_ID\"  FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" IN (9) AND ";
                    strQry += "\"WO_RECORD_ID\"='" + Obj.DfId + "' AND \"WO_DATA_ID\"='" + Obj.DtcCode + "' AND ";
                    strQry += "\"WO_DESCRIPTION\" LIKE '%" + Obj.DtcCode + "%'   ";
                    dt = ObjBseCon.FetchDataTable(strQry);
                    if (dt.Rows.Count > 0)
                    {
                        woId = Convert.ToString(dt.Rows[0]["WO_ID"]);
                    }
                    strQry = "SELECT DISTINCT \"WO_WFO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" IN (9) AND ";
                    strQry += "\"WO_RECORD_ID\"='" + Obj.DfId + "' AND \"WO_DATA_ID\"='" + Obj.DtcCode + "' AND    ";
                    strQry += "\"WO_DESCRIPTION\" LIKE '%" + Obj.DtcCode + "%'   ";
                    dtWfo = ObjBseCon.FetchDataTable(strQry);

                    if ((woId ?? "").Length > 0)
                    {
                        strQry = "SELECT \"WOA_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE  ";
                        strQry += "\"WOA_PREV_APPROVE_ID\"='" + woId + "'  ";
                        dt = ObjBseCon.FetchDataTable(strQry);
                        woaId = Convert.ToString(dt.Rows[0]["WOA_ID"]);
                    }

                    strQry = "select \"DCT_ID\" from \"TBLDTCTRANSACTION\" inner join \"TBLDTCFAILURE\" on ";
                    strQry += "cast(\"DCT_ACT_REFNO\" as text)=cast(\"DF_ID\" as text) and \"DF_DTC_CODE\"=\"DCT_DTC_CODE\" ";
                    strQry += "where \"DF_ID\"='" + Obj.DfId + "' ";
                    dt = ObjBseCon.FetchDataTable(strQry);

                    if (dt.Rows.Count > 0)
                    {
                        dctId = Convert.ToString(dt.Rows[0]["DCT_ID"]);
                    }

                    strQry = "select \"DRT_ID\" from \"TBLDTRTRANSACTION\" inner join \"TBLDTCFAILURE\" on ";
                    strQry += "cast(\"DRT_ACT_REFNO\" as text)=cast(\"DF_ID\" as text) and \"DF_EQUIPMENT_ID\"=";
                    strQry += "\"DRT_DTR_CODE\" where \"DF_ID\"='" + Obj.DfId + "'  ";
                    dt = ObjBseCon.FetchDataTable(strQry);

                    if (dt.Rows.Count > 0)
                    {
                        drtId = Convert.ToString(dt.Rows[0]["DRT_ID"]);
                    }
                    strQry = "select  \"DF_ID\" from \"TBLDTCFAILURE\" where  ";
                    strQry += "\"DF_EQUIPMENT_ID\"='" + Obj.DfEquipmentId + "' ";
                    dt = ObjBseCon.FetchDataTable(strQry);

                    if (dt.Rows.Count > 1)
                    {
                        strQry = "select \"DF_ID\", \"DF_STATUS_FLAG\" from \"TBLDTCFAILURE\" where  ";
                        strQry += "\"DF_EQUIPMENT_ID\"='" + Obj.DfEquipmentId + "' ORDER BY \"DF_ID\" desc";
                        dt = ObjBseCon.FetchDataTable(strQry);
                        if (dt.Rows.Count > 0)
                        {
                            dfStatusFlag = Convert.ToString(dt.Rows[1]["DF_STATUS_FLAG"]);
                            if (dfStatusFlag == "1" || dfStatusFlag == "3")
                            {
                                strQry = "update \"TBLTCMASTER\"  set  \"TC_STATUS\"=2 ,\"TC_CURRENT_LOCATION\"=2,";
                                strQry += "\"TC_LOCATION_ID\"='" + Obj.DfLocCode + "' where \"TC_CODE\"='" + Obj.DfEquipmentId + "'  ";
                                Obj.QryStringBuilder.Append(strQry);
                                Obj.QryStringBuilder.Append(";");
                                ObjBseCon.ExecuteQry(strQry);
                            }
                            //else
                            //{
                            //    strQry = "update \"TBLTCMASTER\"  set  \"TC_STATUS\"=5 ,\"TC_CURRENT_LOCATION\"=2,";
                            //    strQry += "\"TC_LOCATION_ID\"='" + Obj.DfLocCode + "' where \"TC_CODE\"='" + Obj.DfEquipmentId + "'  ";
                            //    Obj.QryStringBuilder.Append(strQry);
                            //    Obj.QryStringBuilder.Append(";");
                            //    ObjBseCon.ExecuteQry(strQry);
                            //}
                        }
                    }
                    else
                    {
                        strQry = "update \"TBLTCMASTER\"  set  \"TC_STATUS\"=1 ,\"TC_CURRENT_LOCATION\"=2,";
                        strQry += "\"TC_LOCATION_ID\"='" + Obj.DfLocCode + "' where \"TC_CODE\"='" + Obj.DfEquipmentId + "'  ";
                        Obj.QryStringBuilder.Append(strQry);
                        Obj.QryStringBuilder.Append(";");
                        ObjBseCon.ExecuteQry(strQry);
                    }

                    for (int l = 0; l < dtWo.Rows.Count; l++)
                    {
                        strQry = "DELETE FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\"='" + dtWo.Rows[l]["WO_ID"] + "'";
                        Obj.QryStringBuilder.Append(strQry);
                        Obj.QryStringBuilder.Append(";");
                        ObjBseCon.ExecuteQry(strQry);
                    }

                    for (int l = 0; l < dtWfo.Rows.Count; l++)
                    {
                        strQry = "DELETE FROM \"TBLWFODATA\" WHERE \"WFO_ID\" = '" + dtWfo.Rows[l]["WO_WFO_ID"] + "'";
                        Obj.QryStringBuilder.Append(strQry);
                        Obj.QryStringBuilder.Append(";");
                        ObjBseCon.ExecuteQry(strQry);
                    }

                    if ((woaId ?? "").Length > 0)
                    {
                        strQry = "DELETE FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_ID\" = '" + woaId + "'";
                        Obj.QryStringBuilder.Append(strQry);
                        Obj.QryStringBuilder.Append(";");
                        ObjBseCon.ExecuteQry(strQry);
                    }

                    if ((Obj.DfId ?? "").Length > 0)
                    {
                        strQry = "DELETE FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\" = '" + Obj.DfId + "'";
                        Obj.QryStringBuilder.Append(strQry);
                        Obj.QryStringBuilder.Append(";");
                        ObjBseCon.ExecuteQry(strQry);
                    }

                    if ((dctId ?? "").Length > 0)
                    {
                        strQry = "delete from \"TBLDTCTRANSACTION\" where \"DCT_ID\"='" + dctId + "' ";
                        Obj.QryStringBuilder.Append(strQry);
                        Obj.QryStringBuilder.Append(";");
                        ObjBseCon.ExecuteQry(strQry);
                    }

                    if ((drtId ?? "").Length > 0)
                    {
                        strQry = "delete from \"TBLDTRTRANSACTION\" where \"DRT_ID\"='" + drtId + "' ";
                        Obj.QryStringBuilder.Append(strQry);
                        Obj.QryStringBuilder.Append(";");
                        ObjBseCon.ExecuteQry(strQry);
                    }

                }

                strQry = "select max(\"TM_ID\") as \"TM_ID\" from \"TBLTRANSDTCMAPPING\" where  \"TM_TC_ID\"='" + Obj.DfEquipmentId + "' ";
                strQry += "and  \"TM_DTC_ID\"='" + Obj.DtcCode + "' ";
                dt = ObjCon.FetchDataTable(strQry);

                if (dt.Rows.Count > 0)
                {
                    tmId = Convert.ToString(dt.Rows[0]["TM_ID"]);
                }

                strQry = "update \"TBLDTCMAST\"  set \"DT_TC_ID\"='" + Obj.DfEquipmentId + "'  where \"DT_CODE\"='" + Obj.DtcCode + "' ";
                Obj.QryStringBuilder.Append(strQry);
                Obj.QryStringBuilder.Append(";");
                ObjBseCon.ExecuteQry(strQry);

                if ((tmId ?? "").Length > 0)
                {
                    strQry = "update \"TBLTRANSDTCMAPPING\"  set \"TM_LIVE_FLAG\"=1  where \"TM_ID\"='" + tmId + "' ";
                    Obj.QryStringBuilder.Append(strQry);
                    Obj.QryStringBuilder.Append(";");
                    ObjBseCon.ExecuteQry(strQry);
                }

                if ((Obj.DtcCode ?? "").Length > 0)
                {
                    strQry = "delete from \"TBLPENDINGTRANSACTION\" where \"TRANS_DTC_CODE\"='" + Obj.DtcCode + "' ";
                    Obj.QryStringBuilder.Append(strQry);
                    Obj.QryStringBuilder.Append(";");
                    ObjBseCon.ExecuteQry(strQry);
                }
                Arr[0] = "0";
                Arr[1] = "Deleted Syccessfully";
                Arr[2] = Convert.ToString(Obj.QryStringBuilder);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name,
                   MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
            return Arr;
        }
        /// <summary>
        /// Delete Estimation Record Details
        /// else part need to write.
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public string[] DeleteEstimationRecordDetails(clsFailureTransactionDelete Obj, DataBseConnection ObjBseCon)
        {
            string[] Arr = new string[3];
            string strQry = string.Empty;
            string Qry = string.Empty;

            string dfStatusFlag = string.Empty;
            string woId = string.Empty;
            string dctId = string.Empty;
            string woaId = string.Empty;
            string drtId = string.Empty;

            DataTable dt = new DataTable();
            DataTable dtWo = new DataTable();
            DataTable dtWfo = new DataTable();
            try
            {
                Obj.dtMajor = ValidationMajorFailDelete(Obj, ObjBseCon);
                if (dtMajor.Rows.Count > 0)
                {
                    Obj.DfEquipmentId = Convert.ToString(dtMajor.Rows[0]["DF_EQUIPMENT_ID"]);
                    Obj.DfLocCode = Convert.ToString(dtMajor.Rows[0]["DF_LOC_CODE"]);
                    Obj.DfId = Convert.ToString(dtMajor.Rows[0]["DF_ID"]);
                    Obj.EstId = Convert.ToString(dtMajor.Rows[0]["EST_ID"]);
                    Obj.EstDetId = Convert.ToString(dtMajor.Rows[0]["EST_DETAILS_ID"]);
                }

                if ((Obj.EstId ?? "").Length > 0 && (Obj.EstDetId ?? "").Length > 0) // estimation completed.
                {
                    strQry = "SELECT \"WO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" IN (45) AND ";
                    strQry += "\"WO_RECORD_ID\"='" + Obj.EstDetId + "' AND \"WO_DATA_ID\"='" + Obj.DfId + "' AND ";
                    strQry += "\"WO_DESCRIPTION\" LIKE '%" + Obj.DtcCode + "%'   ";
                    dtWo = ObjBseCon.FetchDataTable(strQry);

                    strQry = "SELECT DISTINCT \"WO_WFO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" IN (45) AND ";
                    strQry += "\"WO_RECORD_ID\"='" + Obj.EstDetId + "' AND \"WO_DATA_ID\"='" + Obj.DfId + "' AND    ";
                    strQry += "\"WO_DESCRIPTION\" LIKE '%" + Obj.DtcCode + "%'   ";
                    dtWfo = ObjBseCon.FetchDataTable(strQry);

                    strQry = "SELECT max(\"WO_ID\") as \"WO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" IN (45) AND ";
                    strQry += "\"WO_RECORD_ID\"='" + Obj.EstDetId + "' AND \"WO_DATA_ID\"='" + Obj.DfId + "' AND ";
                    strQry += "\"WO_DESCRIPTION\" LIKE '%" + Obj.DtcCode + "%' ";
                    dt = ObjBseCon.FetchDataTable(strQry);
                    woId = Convert.ToString(dt.Rows[0]["WO_ID"]);

                    strQry = "SELECT \"WOA_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE  ";
                    strQry += "\"WOA_PREV_APPROVE_ID\"='" + woId + "'  ";
                    dt = ObjBseCon.FetchDataTable(strQry);
                    woaId = Convert.ToString(dt.Rows[0]["WOA_ID"]);

                    strQry = "select \"DCT_ID\" from \"TBLDTCTRANSACTION\" inner join \"TBLESTIMATIONDETAILS\" on ";
                    strQry += "cast(\"DCT_ACT_REFNO\" as text)=cast(\"EST_ID\" as text) inner join \"TBLDTCFAILURE\" on ";
                    strQry += "\"EST_FAILUREID\"=\"DF_ID\"  and \"DF_DTC_CODE\"=\"DCT_DTC_CODE\"   ";
                    strQry += "where \"EST_ID\"='" + Obj.EstDetId + "'  and \"DF_ID\"='" + Obj.DfId + "' ";
                    dt = ObjBseCon.FetchDataTable(strQry);
                    dctId = Convert.ToString(dt.Rows[0]["DCT_ID"]);

                    for (int l = 0; l < dtWo.Rows.Count; l++)
                    {
                        strQry = "DELETE FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\"='" + dtWo.Rows[l]["WO_ID"] + "'";
                        Obj.QryStringBuilder.Append(strQry);
                        Obj.QryStringBuilder.Append(";");
                        ObjBseCon.ExecuteQry(strQry);
                    }

                    for (int l = 0; l < dtWfo.Rows.Count; l++)
                    {
                        strQry = "DELETE FROM \"TBLWFODATA\" WHERE \"WFO_ID\" = '" + dtWfo.Rows[l]["WO_WFO_ID"] + "'";
                        Obj.QryStringBuilder.Append(strQry);
                        Obj.QryStringBuilder.Append(";");
                        ObjBseCon.ExecuteQry(strQry);
                    }

                    strQry = "DELETE FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_ID\" = '" + woaId + "'";
                    Obj.QryStringBuilder.Append(strQry);
                    Obj.QryStringBuilder.Append(";");
                    ObjBseCon.ExecuteQry(strQry);

                    strQry = "delete from \"TBLDTCTRANSACTION\" where \"DCT_ID\"='" + dctId + "' ";
                    Obj.QryStringBuilder.Append(strQry);
                    Obj.QryStringBuilder.Append(";");
                    ObjBseCon.ExecuteQry(strQry);

                    strQry = "delete from \"TBLESTIMATIONDETAILS\" where \"EST_ID\"='" + Obj.EstDetId + "' ";
                    Obj.QryStringBuilder.Append(strQry);
                    Obj.QryStringBuilder.Append(";");
                    ObjBseCon.ExecuteQry(strQry);

                    strQry = "delete from \"TBLESTIMATION\" where \"EST_ID\"='" + Obj.EstId + "' ";
                    Obj.QryStringBuilder.Append(strQry);
                    Obj.QryStringBuilder.Append(";");
                    ObjBseCon.ExecuteQry(strQry);
                }
                else
                {
                    // Failure entry done and  pending in estimation
                    strQry = "select \"DF_ID\" from \"TBLDTCFAILURE\" where \"DF_REPLACE_FLAG\"=0 and \"DF_DTC_CODE\"='" + Obj.DtcCode + "' ";
                    dt = ObjBseCon.FetchDataTable(strQry);

                    if (dt.Rows.Count > 0)
                    {
                        Obj.DfId = Convert.ToString(dt.Rows[0]["DF_ID"]);

                        strQry = "select \"DF_EQUIPMENT_ID\",\"DF_LOC_CODE\",\"DF_ID\",\"TRANS_DTC_CODE\",\"TRANS_BO_ID\" from  ";
                        strQry += "\"TBLDTCFAILURE\" inner join  \"TBLPENDINGTRANSACTION\" on \"DF_DTC_CODE\"=";
                        strQry += "\"TRANS_DTC_CODE\" where \"DF_REPLACE_FLAG\"=0 and \"DF_ID\" not in (select  \"EST_FAILUREID\" ";
                        strQry += "from \"TBLESTIMATIONDETAILS\" where \"EST_FAILUREID\"='" + Obj.DfId + "') and \"DF_ID\" not in ";
                        strQry += "(select  \"EST_DF_ID\" from \"TBLESTIMATION\" where \"EST_DF_ID\"='" + Obj.DfId + "') and  ";
                        strQry += "\"DF_DTC_CODE\"='" + Obj.DtcCode + "'  ";
                        dt = ObjBseCon.FetchDataTable(strQry);

                        if (dt.Rows.Count > 0)
                        {
                            Obj.DfEquipmentId = Convert.ToString(dt.Rows[0]["DF_EQUIPMENT_ID"]);
                            Obj.DfLocCode = Convert.ToString(dt.Rows[0]["DF_LOC_CODE"]);
                            Obj.DfId = Convert.ToString(dt.Rows[0]["DF_ID"]);

                            strQry = "SELECT \"WO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" IN (9,10) AND ";
                            strQry += "\"WO_RECORD_ID\"='" + Obj.DfId + "' AND \"WO_DATA_ID\"='" + Obj.DtcCode + "' AND ";
                            strQry += "\"WO_DESCRIPTION\" LIKE '%" + Obj.DtcCode + "%'   ";
                            dtWo = ObjBseCon.FetchDataTable(strQry);

                            strQry = "SELECT max(\"WO_ID\") as \"WO_ID\"  FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" IN (9,10) AND ";
                            strQry += "\"WO_RECORD_ID\"='" + Obj.DfId + "' AND \"WO_DATA_ID\"='" + Obj.DtcCode + "' AND ";
                            strQry += "\"WO_DESCRIPTION\" LIKE '%" + Obj.DtcCode + "%'   ";
                            dt = ObjBseCon.FetchDataTable(strQry);
                            woId = Convert.ToString(dt.Rows[0]["WO_ID"]);

                            strQry = "SELECT DISTINCT \"WO_WFO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" IN (9,10) AND ";
                            strQry += "\"WO_RECORD_ID\"='" + Obj.DfId + "' AND \"WO_DATA_ID\"='" + Obj.DtcCode + "' AND    ";
                            strQry += "\"WO_DESCRIPTION\" LIKE '%" + Obj.DtcCode + "%'   ";
                            dtWfo = ObjBseCon.FetchDataTable(strQry);

                            strQry = "SELECT \"WOA_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE  ";
                            strQry += "\"WOA_PREV_APPROVE_ID\"='" + woId + "'  ";
                            dt = ObjBseCon.FetchDataTable(strQry);
                            woaId = Convert.ToString(dt.Rows[0]["WOA_ID"]);

                            strQry = "select \"DCT_ID\" from \"TBLDTCTRANSACTION\" inner join \"TBLDTCFAILURE\" on ";
                            strQry += "cast(\"DCT_ACT_REFNO\" as text)=cast(\"DF_ID\" as text) and \"DF_DTC_CODE\"=\"DCT_DTC_CODE\" ";
                            strQry += "where \"DF_ID\"='" + Obj.DfId + "' ";
                            dt = ObjBseCon.FetchDataTable(strQry);
                            dctId = Convert.ToString(dt.Rows[0]["DCT_ID"]);

                            strQry = "select \"DRT_ID\" from \"TBLDTRTRANSACTION\" inner join \"TBLDTCFAILURE\" on ";
                            strQry += "cast(\"DRT_ACT_REFNO\" as text)=cast(\"DF_ID\" as text) and \"DF_EQUIPMENT_ID\"=";
                            strQry += "\"DRT_DTR_CODE\" where \"DF_ID\"='" + Obj.DfId + "'  ";
                            dt = ObjBseCon.FetchDataTable(strQry);
                            drtId = Convert.ToString(dt.Rows[0]["DRT_ID"]);

                            strQry = "select  \"DF_ID\" from \"TBLDTCFAILURE\" where  ";
                            strQry += "\"DF_EQUIPMENT_ID\"='" + Obj.DfEquipmentId + "' ";
                            dt = ObjBseCon.FetchDataTable(strQry);
                            if (dt.Rows.Count > 1)
                            {
                                strQry = "select \"DF_ID\", \"DF_STATUS_FLAG\" from \"TBLDTCFAILURE\" where  ";
                                strQry += "\"DF_EQUIPMENT_ID\"='" + Obj.DfEquipmentId + "' ORDER BY \"DF_ID\" desc";
                                dt = ObjBseCon.FetchDataTable(strQry);
                                if (dt.Rows.Count > 0)
                                {
                                    dfStatusFlag = Convert.ToString(dt.Rows[1]["DF_STATUS_FLAG"]);
                                    if (dfStatusFlag == "1" || dfStatusFlag == "3")
                                    {
                                        strQry = "update \"TBLTCMASTER\"  set  \"TC_STATUS\"=2 ,\"TC_CURRENT_LOCATION\"=2,";
                                        strQry += "\"TC_LOCATION_ID\"='" + Obj.DfLocCode + "' where \"TC_CODE\"='" + Obj.DfEquipmentId + "'  ";
                                        Obj.QryStringBuilder.Append(strQry);
                                        Obj.QryStringBuilder.Append(";");
                                        ObjBseCon.ExecuteQry(strQry);
                                    }
                                    else
                                    {
                                        strQry = "update \"TBLTCMASTER\"  set  \"TC_STATUS\"=5 ,\"TC_CURRENT_LOCATION\"=2,";
                                        strQry += "\"TC_LOCATION_ID\"='" + Obj.DfLocCode + "' where \"TC_CODE\"='" + Obj.DfEquipmentId + "'  ";
                                        Obj.QryStringBuilder.Append(strQry);
                                        Obj.QryStringBuilder.Append(";");
                                        ObjBseCon.ExecuteQry(strQry);
                                    }
                                }
                            }
                            else
                            {
                                strQry = "update \"TBLTCMASTER\"  set  \"TC_STATUS\"=1 ,\"TC_CURRENT_LOCATION\"=2,";
                                strQry += "\"TC_LOCATION_ID\"='" + Obj.DfLocCode + "' where \"TC_CODE\"='" + Obj.DfEquipmentId + "'  ";
                                Obj.QryStringBuilder.Append(strQry);
                                Obj.QryStringBuilder.Append(";");
                                ObjBseCon.ExecuteQry(strQry);
                            }

                            for (int l = 0; l < dtWo.Rows.Count; l++)
                            {
                                strQry = "DELETE FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\"='" + dtWo.Rows[l]["WO_ID"] + "'";
                                Obj.QryStringBuilder.Append(strQry);
                                Obj.QryStringBuilder.Append(";");
                                ObjBseCon.ExecuteQry(strQry);
                            }

                            for (int l = 0; l < dtWfo.Rows.Count; l++)
                            {
                                strQry = "DELETE FROM \"TBLWFODATA\" WHERE \"WFO_ID\" = '" + dtWfo.Rows[l]["WO_WFO_ID"] + "'";
                                Obj.QryStringBuilder.Append(strQry);
                                Obj.QryStringBuilder.Append(";");
                                ObjBseCon.ExecuteQry(strQry);
                            }

                            strQry = "DELETE FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_ID\" = '" + woaId + "'";
                            Obj.QryStringBuilder.Append(strQry);
                            Obj.QryStringBuilder.Append(";");
                            ObjBseCon.ExecuteQry(strQry);

                            strQry = "DELETE FROM \"TBLDTCFAILURE\" WHERE \"DF_ID\" = '" + Obj.DfId + "'";
                            Obj.QryStringBuilder.Append(strQry);
                            Obj.QryStringBuilder.Append(";");
                            ObjBseCon.ExecuteQry(strQry);

                            strQry = "delete from \"TBLDTCTRANSACTION\" where \"DCT_ID\"='" + dctId + "' ";
                            Obj.QryStringBuilder.Append(strQry);
                            Obj.QryStringBuilder.Append(";");
                            ObjBseCon.ExecuteQry(strQry);

                            strQry = "delete from \"TBLDTRTRANSACTION\" where \"DRT_ID\"='" + drtId + "' ";
                            Obj.QryStringBuilder.Append(strQry);
                            Obj.QryStringBuilder.Append(";");
                            ObjBseCon.ExecuteQry(strQry);

                            // delete estimation pending data 
                            strQry = "SELECT \"WO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" IN (45) AND ";
                            strQry += "\"WO_RECORD_ID\" < 0 AND \"WO_DATA_ID\"='" + Obj.DfId + "' AND ";
                            strQry += "\"WO_DESCRIPTION\" LIKE '%" + Obj.DtcCode + "%'   ";
                            dtWo = ObjBseCon.FetchDataTable(strQry);

                            strQry = "SELECT DISTINCT \"WO_WFO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" IN (45) AND ";
                            strQry += "\"WO_RECORD_ID\" < 0 AND \"WO_DATA_ID\"='" + Obj.DfId + "' AND    ";
                            strQry += "\"WO_DESCRIPTION\" LIKE '%" + Obj.DtcCode + "%'   ";
                            dtWfo = ObjBseCon.FetchDataTable(strQry);

                            for (int l = 0; l < dtWo.Rows.Count; l++)
                            {
                                strQry = "DELETE FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\"='" + dtWo.Rows[l]["WO_ID"] + "'";
                                Obj.QryStringBuilder.Append(strQry);
                                Obj.QryStringBuilder.Append(";");
                                ObjBseCon.ExecuteQry(strQry);
                            }

                            for (int l = 0; l < dtWfo.Rows.Count; l++)
                            {
                                strQry = "DELETE FROM \"TBLWFODATA\" WHERE \"WFO_ID\" = '" + dtWfo.Rows[l]["WO_WFO_ID"] + "'";
                                Obj.QryStringBuilder.Append(strQry);
                                Obj.QryStringBuilder.Append(";");
                                ObjBseCon.ExecuteQry(strQry);
                            }

                            strQry = "delete from \"TBLPENDINGTRANSACTION\" where \"TRANS_DTC_CODE\"='" + Obj.DtcCode + "' ";
                            Obj.QryStringBuilder.Append(strQry);
                            Obj.QryStringBuilder.Append(";");
                            ObjBseCon.ExecuteQry(strQry);
                        }
                    }
                }
                Arr[2] = Convert.ToString(Obj.QryStringBuilder);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name,
                   MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
            return Arr;
        }
        /// <summary>
        /// Delete Workorder Record Details
        /// this is completed.
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public string[] DeleteWorkorderRecordDetails(clsFailureTransactionDelete Obj, DataBseConnection ObjBseCon)
        {
            string[] Arr = new string[3];
            string strQry = string.Empty;
            string woId = string.Empty;
            string dctId = string.Empty;
            string woaId = string.Empty;
            string woaId1 = string.Empty;
            string drtId = string.Empty;

            DataTable dt = new DataTable();
            DataTable dtWo = new DataTable();
            DataTable dtWfo = new DataTable();
            try
            {
                Obj.dtMajor = ValidationMajorFailDelete(Obj, ObjBseCon);
                if (dtMajor.Rows.Count > 0)
                {
                    Obj.DfId = Convert.ToString(dtMajor.Rows[0]["DF_ID"]);
                    Obj.WoSlno = Convert.ToString(dtMajor.Rows[0]["WO_SLNO"]);
                }

                if ((Obj.WoSlno ?? "").Length > 0) // work order
                {
                    strQry = "SELECT \"WO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" IN (11) AND ";
                    strQry += "\"WO_RECORD_ID\"='" + Obj.WoSlno + "' AND \"WO_DATA_ID\"='" + Obj.DfId + "' AND ";
                    strQry += "\"WO_DESCRIPTION\" LIKE '%" + Obj.DtcCode + "%'   ";
                    dtWo = ObjBseCon.FetchDataTable(strQry);

                    strQry = "SELECT DISTINCT \"WO_WFO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" IN (11) AND ";
                    strQry += "\"WO_RECORD_ID\"='" + Obj.WoSlno + "' AND \"WO_DATA_ID\"='" + Obj.DfId + "' AND    ";
                    strQry += "\"WO_DESCRIPTION\" LIKE '%" + Obj.DtcCode + "%'   ";
                    dtWfo = ObjBseCon.FetchDataTable(strQry);

                    strQry = "SELECT max(\"WO_ID\") as \"WO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" IN (11) AND ";
                    strQry += "\"WO_RECORD_ID\"='" + Obj.WoSlno + "' AND \"WO_DATA_ID\"='" + Obj.DfId + "' AND ";
                    strQry += "\"WO_DESCRIPTION\" LIKE '%" + Obj.DtcCode + "%' ";
                    dt = ObjBseCon.FetchDataTable(strQry);
                    woId = Convert.ToString(dt.Rows[0]["WO_ID"]);

                    strQry = "SELECT \"WOA_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE  ";
                    strQry += "\"WOA_PREV_APPROVE_ID\"='" + woId + "'  ";
                    dt = ObjBseCon.FetchDataTable(strQry);
                    woaId = Convert.ToString(dt.Rows[0]["WOA_ID"]);
                    woaId1 = Convert.ToString(dt.Rows[1]["WOA_ID"]);

                    strQry = "select \"DCT_ID\" from \"TBLDTCTRANSACTION\" inner join \"TBLWORKORDER\" on cast(\"DCT_ACT_REFNO\" as text)";
                    strQry += "=cast(\"WO_SLNO\" as text) inner join \"TBLDTCFAILURE\" on \"WO_DF_ID\"=\"DF_ID\"  and \"DF_DTC_CODE\"=";
                    strQry += "\"DCT_DTC_CODE\"   where \"WO_SLNO\"='" + Obj.WoSlno + "'  and \"DF_ID\"='" + Obj.DfId + "' ";
                    dt = ObjBseCon.FetchDataTable(strQry);
                    dctId = Convert.ToString(dt.Rows[0]["DCT_ID"]);

                    for (int l = 0; l < dtWo.Rows.Count; l++)
                    {
                        strQry = "DELETE FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\"='" + dtWo.Rows[l]["WO_ID"] + "'";
                        Obj.QryStringBuilder.Append(strQry);
                        Obj.QryStringBuilder.Append(";");
                        ObjBseCon.ExecuteQry(strQry);
                    }

                    for (int l = 0; l < dtWfo.Rows.Count; l++)
                    {
                        strQry = "DELETE FROM \"TBLWFODATA\" WHERE \"WFO_ID\" = '" + dtWfo.Rows[l]["WO_WFO_ID"] + "'";
                        Obj.QryStringBuilder.Append(strQry);
                        Obj.QryStringBuilder.Append(";");
                        ObjBseCon.ExecuteQry(strQry);
                    }

                    strQry = "DELETE FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_ID\" = '" + woaId + "' ";
                    Obj.QryStringBuilder.Append(strQry);
                    Obj.QryStringBuilder.Append(";");
                    ObjBseCon.ExecuteQry(strQry);

                    strQry = "DELETE FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_ID\" = '" + woaId1 + "' ";
                    Obj.QryStringBuilder.Append(strQry);
                    Obj.QryStringBuilder.Append(";");
                    ObjBseCon.ExecuteQry(strQry);

                    strQry = "delete from \"TBLDTCTRANSACTION\" where \"DCT_ID\"='" + dctId + "' ";
                    Obj.QryStringBuilder.Append(strQry);
                    Obj.QryStringBuilder.Append(";");
                    ObjBseCon.ExecuteQry(strQry);

                    strQry = "delete from \"TBLWORKORDER\" where \"WO_SLNO\"='" + Obj.WoSlno + "'  ";
                    Obj.QryStringBuilder.Append(strQry);
                    Obj.QryStringBuilder.Append(";");
                    ObjBseCon.ExecuteQry(strQry);

                    // indent Records delete from maintable and TBLDTCTRANSACTION
                    strQry = "delete from \"TBLINDENT\" where \"TI_WO_SLNO\"='" + Obj.WoSlno + "' ";
                    Obj.QryStringBuilder.Append(strQry);
                    Obj.QryStringBuilder.Append(";");
                    ObjBseCon.ExecuteQry(strQry);

                    strQry = "delete from \"TBLDTCTRANSACTION\" where \"DCT_ACT_REFNO\"='" + Obj.WoSlno + "' And \"DCT_DTC_CODE\" = '" + Obj.DtcCode + "' And \"DCT_DESC\" LIKE '%INDENT CREATED%' ";
                    Obj.QryStringBuilder.Append(strQry);
                    Obj.QryStringBuilder.Append(";");
                    ObjBseCon.ExecuteQry(strQry);
                }
                else
                {
                    strQry = "SELECT \"WO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" IN (11) AND ";
                    strQry += "\"WO_RECORD_ID\" < 0 AND \"WO_DATA_ID\"='" + Obj.DfId + "' AND ";
                    strQry += "\"WO_DESCRIPTION\" LIKE '%" + Obj.DtcCode + "%'   ";
                    dtWo = ObjBseCon.FetchDataTable(strQry);

                    strQry = "SELECT DISTINCT \"WO_WFO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" IN (11) AND ";
                    strQry += "\"WO_RECORD_ID\" < 0 AND \"WO_DATA_ID\"='" + Obj.DfId + "' AND    ";
                    strQry += "\"WO_DESCRIPTION\" LIKE '%" + Obj.DtcCode + "%'   ";
                    dtWfo = ObjBseCon.FetchDataTable(strQry);

                    for (int l = 0; l < dtWo.Rows.Count; l++)
                    {
                        strQry = "DELETE FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\"='" + dtWo.Rows[l]["WO_ID"] + "'";
                        Obj.QryStringBuilder.Append(strQry);
                        Obj.QryStringBuilder.Append(";");
                        ObjBseCon.ExecuteQry(strQry);
                    }

                    for (int l = 0; l < dtWfo.Rows.Count; l++)
                    {
                        strQry = "DELETE FROM \"TBLWFODATA\" WHERE \"WFO_ID\" = '" + dtWfo.Rows[l]["WO_WFO_ID"] + "'";
                        Obj.QryStringBuilder.Append(strQry);
                        Obj.QryStringBuilder.Append(";");
                        ObjBseCon.ExecuteQry(strQry);
                    }
                }
                Arr[2] = Convert.ToString(Obj.QryStringBuilder);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name,
                   MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
            return Arr;
        }
        /// <summary>
        /// Delete Invoice Record Details
        /// this is completed.
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public string[] DeleteInvoiceRecordDetails(clsFailureTransactionDelete Obj, DataBseConnection ObjBseCon)
        {
            string[] Arr = new string[3];
            string strQry = string.Empty;
            string dctId = string.Empty;
            string dctId1 = string.Empty;
            DataTable dt = new DataTable();

            try
            {
                Obj.dtMajor = ValidationMajorFailDelete(Obj, ObjBseCon);
                if (dtMajor.Rows.Count > 0)
                {
                    Obj.DfId = Convert.ToString(dtMajor.Rows[0]["DF_ID"]);
                    Obj.WoSlno = Convert.ToString(dtMajor.Rows[0]["WO_SLNO"]);
                    Obj.InIno = Convert.ToString(dtMajor.Rows[0]["IN_NO"]);
                    Obj.TdId = Convert.ToString(dtMajor.Rows[0]["TD_ID"]);
                    Obj.TdTcNo = Convert.ToString(dtMajor.Rows[0]["TD_TC_NO"]);
                }

                if ((Obj.InIno ?? "").Length > 0 && (Obj.TdId ?? "").Length > 0) // invoice
                {
                    strQry = "SELECT \"WO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" IN (13) AND ";
                    strQry += "\"WO_RECORD_ID\"='" + Obj.InIno + "' ";
                    DtWorkFlowObjects = ObjBseCon.FetchDataTable(strQry);

                    if (DtWorkFlowObjects.Rows.Count > 0)
                    {
                        for (int l = 0; l < DtWorkFlowObjects.Rows.Count; l++)
                        {
                            strQry = "DELETE FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\"='" + DtWorkFlowObjects.Rows[l]["WO_ID"] + "'";
                            Obj.QryStringBuilder.Append(strQry);
                            Obj.QryStringBuilder.Append(";");
                            ObjBseCon.ExecuteQry(strQry);
                        }
                    }
                    strQry = "select \"DCT_ID\" from \"TBLDTCTRANSACTION\" inner join  \"TBLDTCINVOICE\" on ";
                    strQry += "cast(\"DCT_ACT_REFNO\" as text)=cast(\"IN_NO\" as text) inner join \"TBLINDENT\" ";
                    strQry += "on \"IN_TI_NO\"=\"TI_ID\" inner join \"TBLWORKORDER\" on \"WO_SLNO\"=\"TI_WO_SLNO\" ";
                    strQry += "inner join \"TBLDTCFAILURE\" on \"WO_DF_ID\"=\"DF_ID\"  and \"DF_DTC_CODE\"=";
                    strQry += "\"DCT_DTC_CODE\"   where \"IN_NO\"='" + Obj.InIno + "'  and \"DF_ID\"='" + Obj.DfId + "' ";
                    dt = ObjBseCon.FetchDataTable(strQry);
                    if (dt.Rows.Count > 0)
                    {
                        dctId = string.Empty;
                        dctId = Convert.ToString(dt.Rows[0]["DCT_ID"]);
                    }

                    strQry = "select \"DCT_ID\" from \"TBLDTCTRANSACTION\" inner join \"TBLTRANSDTCMAPPING\" on ";
                    strQry += "cast(\"DCT_ACT_REFNO\" as text)=cast(\"TM_ID\" as text) and \"TM_DTC_ID\"=\"DCT_DTC_CODE\" ";
                    strQry += "where \"TM_LIVE_FLAG\"=1 and \"TM_DTC_ID\"='" + Obj.DtcCode + "'";
                    dt = ObjBseCon.FetchDataTable(strQry);
                    if (dt.Rows.Count > 0)
                    {
                        dctId1 = string.Empty;
                        dctId1 = Convert.ToString(dt.Rows[0]["DCT_ID"]);
                    }

                    if ((dctId ?? "").Length > 0 || (dctId1 ?? "").Length > 0)
                    {
                        strQry = " delete from \"TBLDTCTRANSACTION\" where \"DCT_ID\" in ('" + dctId + "','" + dctId1 + "' ) ";
                        Obj.QryStringBuilder.Append(strQry);
                        Obj.QryStringBuilder.Append(";");
                        ObjBseCon.ExecuteQry(strQry);

                    }

                    strQry = "select \"STO_SM_ID\" from  \"TBLWORKORDER\" inner join \"TBLDIVISION\" on \"WO_OFF_CODE\"=\"DIV_CODE\" ";
                    strQry += "inner join \"TBLSTOREOFFCODE\" on \"STO_OFF_CODE\"=\"DIV_CODE\" where \"WO_SLNO\"='" + Obj.WoSlno + "' ";
                    dt = ObjBseCon.FetchDataTable(strQry);
                    if (dt.Rows.Count > 0)
                    {
                        strQry = " update \"TBLTCMASTER\"  set \"TC_CURRENT_LOCATION\" = 1, \"TC_LOCATION_ID\" = ";
                        strQry += " '" + Convert.ToString(dt.Rows[0]["STO_SM_ID"]) + "' where \"TC_CODE\"='" + Obj.TdTcNo + "' ";
                        Obj.QryStringBuilder.Append(strQry);
                        Obj.QryStringBuilder.Append(";");
                        ObjBseCon.ExecuteQry(strQry);
                    }

                    strQry = "select \"TM_ID\" from \"TBLTRANSDTCMAPPING\" WHERE \"TM_TC_ID\"='" + Obj.TdTcNo + "' ";
                    strQry += "and \"TM_DTC_ID\"='" + Obj.DtcCode + "' and \"TM_LIVE_FLAG\"=1 ";
                    dt = ObjBseCon.FetchDataTable(strQry);
                    if (dt.Rows.Count > 0)
                    {
                        strQry = " UPDATE \"TBLTRANSDTCMAPPING\"  SET  \"TM_LIVE_FLAG\"=0 WHERE \"TM_ID\"='" + Convert.ToString(dt.Rows[0]["TM_ID"]) + "' ";
                        Obj.QryStringBuilder.Append(strQry);
                        Obj.QryStringBuilder.Append(";");
                        ObjBseCon.ExecuteQry(strQry);

                        strQry = " select \"DRT_ID\" from \"TBLDTRTRANSACTION\" where \"DRT_DTR_CODE\"='" + Obj.TdTcNo + "' ";
                        strQry += " and \"DRT_LOC_TYPE\"=2 AND \"DRT_ACT_REFNO\"='" + Convert.ToString(dt.Rows[0]["TM_ID"]) + "' ";
                        dt = ObjBseCon.FetchDataTable(strQry);

                        if (dt.Rows.Count > 0)
                        {
                            strQry = " DELETE FROM \"TBLDTRTRANSACTION\" WHERE \"DRT_ID\"='" + Convert.ToString(dt.Rows[0]["DRT_ID"]) + "' ";
                            Obj.QryStringBuilder.Append(strQry);
                            Obj.QryStringBuilder.Append(";");
                            ObjBseCon.ExecuteQry(strQry);
                        }
                    }

                    if ((Obj.InIno ?? "").Length > 0 && (Obj.TdId ?? "").Length > 0)
                    {
                        strQry = "delete from \"TBLDTCINVOICE\" where \"IN_NO\"='" + Obj.InIno + "'  ";
                        Obj.QryStringBuilder.Append(strQry);
                        Obj.QryStringBuilder.Append(";");
                        ObjBseCon.ExecuteQry(strQry);

                        strQry = "delete from \"TBLTCDRAWN\" where \"TD_ID\"='" + Obj.TdId + "' ";
                        Obj.QryStringBuilder.Append(strQry);
                        Obj.QryStringBuilder.Append(";");
                        ObjBseCon.ExecuteQry(strQry);
                    }
                }
                Arr[2] = Convert.ToString(Obj.QryStringBuilder);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name,
                   MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
            return Arr;
        }
        /// <summary>
        /// Delete Decom & RI & CR records.
        /// </summary>
        /// <param name="Obj"></param>
        /// <param name="ObjBseCon"></param>
        /// <returns></returns>
        public string[] DeleteTCReplaceRecordDetails(clsFailureTransactionDelete Obj, DataBseConnection ObjBseCon)
        {
            string[] Arr = new string[3];
            string strQry = string.Empty;
            string woId = string.Empty;
            string woId1 = string.Empty;
            string woaId = string.Empty;
            string woaId1 = string.Empty;

            DataTable dt = new DataTable();
            DataTable dtWo = new DataTable();
            DataTable dtWfo = new DataTable();
            try
            {
                Obj.dtMajor = ValidationMajorFailDelete(Obj, ObjBseCon);
                if (dtMajor.Rows.Count > 0)
                {
                    Obj.WoSlno = Convert.ToString(dtMajor.Rows[0]["WO_SLNO"]);
                    Obj.TrId = Convert.ToString(dtMajor.Rows[0]["TR_ID"]);
                }

                if ((Obj.TrId ?? "").Length > 0) // Decom,RV,CR
                {
                    strQry = "SELECT \"WO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" IN (15,26) AND ";
                    strQry += "\"WO_RECORD_ID\"='" + Obj.WoSlno + "' AND \"WO_DATA_ID\"='" + Obj.DtcCode + "'  ";
                    dtWo = ObjBseCon.FetchDataTable(strQry);

                    strQry = "SELECT DISTINCT \"WO_WFO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" IN (15,26) AND ";
                    strQry += "\"WO_RECORD_ID\"='" + Obj.WoSlno + "' AND \"WO_DATA_ID\"='" + Obj.DtcCode + "' ";
                    dtWfo = ObjBseCon.FetchDataTable(strQry);

                    //strQry = "SELECT max(\"WO_ID\") as \"WO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" IN (14) AND ";
                    //strQry += "\"WO_RECORD_ID\"='" + Obj.TrId + "' AND \"WO_DATA_ID\"='" + Obj.DtcCode + "' ";
                    //dt = ObjBseCon.FetchDataTable(strQry);
                    //woId = Convert.ToString(dt.Rows[0]["WO_ID"]);

                    strQry = "SELECT max(\"WO_ID\") as \"WO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" IN (15) AND ";
                    strQry += "\"WO_RECORD_ID\"='" + Obj.WoSlno + "' AND \"WO_DATA_ID\"='" + Obj.DtcCode + "' and \"WO_NEXT_ROLE\"=0 ";
                    dt = ObjBseCon.FetchDataTable(strQry);
                    if (dt.Rows.Count > 0)
                    {
                        woId1 = Convert.ToString(dt.Rows[0]["WO_ID"]);
                        if (woId1 != null && woId1 != "")
                        {
                            strQry = "SELECT \"WOA_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE  ";
                            strQry += "\"WOA_PREV_APPROVE_ID\"='" + woId1 + "'  ";
                            dt = ObjBseCon.FetchDataTable(strQry);
                            if (dt.Rows.Count > 0)
                            {
                                woaId1 = Convert.ToString(dt.Rows[0]["WOA_ID"]);
                            }
                        }
                    }

                    //if ((woId ?? "").Length > 0)
                    //{
                    //    strQry = "SELECT \"WOA_ID\" FROM \"TBLWO_OBJECT_AUTO\" WHERE  ";
                    //    strQry += "\"WOA_PREV_APPROVE_ID\"=" + woId + "  ";
                    //    dt = ObjBseCon.FetchDataTable(strQry);
                    //    woaId = Convert.ToString(dt.Rows[0]["WOA_ID"]);
                    //}


                    for (int l = 0; l < dtWo.Rows.Count; l++)
                    {
                        strQry = "DELETE FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\"='" + dtWo.Rows[l]["WO_ID"] + "'";
                        Obj.QryStringBuilder.Append(strQry);
                        Obj.QryStringBuilder.Append(";");
                        ObjBseCon.ExecuteQry(strQry);
                    }

                    for (int l = 0; l < dtWfo.Rows.Count; l++)
                    {
                        strQry = "DELETE FROM \"TBLWFODATA\" WHERE \"WFO_ID\" = '" + dtWfo.Rows[l]["WO_WFO_ID"] + "'";
                        Obj.QryStringBuilder.Append(strQry);
                        Obj.QryStringBuilder.Append(";");
                        ObjBseCon.ExecuteQry(strQry);
                    }
                    if ((woaId ?? "").Length > 0)
                    {
                        strQry = "DELETE FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_ID\" = '" + woaId + "'";
                        Obj.QryStringBuilder.Append(strQry);
                        Obj.QryStringBuilder.Append(";");
                        ObjBseCon.ExecuteQry(strQry);
                    }


                    if (woaId1 != null && woaId1 != "")
                    {
                        strQry = "DELETE FROM \"TBLWO_OBJECT_AUTO\" WHERE \"WOA_ID\" = '" + woaId1 + "'";
                        Obj.QryStringBuilder.Append(strQry);
                        Obj.QryStringBuilder.Append(";");
                        ObjBseCon.ExecuteQry(strQry);
                    }

                    strQry = "delete from \"TBLTCREPLACE\" where \"TR_ID\"='" + Obj.TrId + "'  ";
                    Obj.QryStringBuilder.Append(strQry);
                    Obj.QryStringBuilder.Append(";");
                    ObjBseCon.ExecuteQry(strQry);
                }
                else
                {
                    strQry = "SELECT \"WO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" IN (14) AND ";
                    strQry += "\"WO_RECORD_ID\" < 0 AND \"WO_DATA_ID\"='" + Obj.DtcCode + "' AND ";
                    strQry += "\"WO_DESCRIPTION\" LIKE '%" + Obj.DtcCode + "%'   ";
                    dtWo = ObjBseCon.FetchDataTable(strQry);

                    strQry = "SELECT DISTINCT \"WO_WFO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_BO_ID\" IN (14) AND ";
                    strQry += "\"WO_RECORD_ID\" < 0 AND \"WO_DATA_ID\"='" + Obj.DtcCode + "' AND    ";
                    strQry += "\"WO_DESCRIPTION\" LIKE '%" + Obj.DtcCode + "%'   ";
                    dtWfo = ObjBseCon.FetchDataTable(strQry);

                    for (int l = 0; l < dtWo.Rows.Count; l++)
                    {
                        strQry = "DELETE FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_ID\"='" + dtWo.Rows[l]["WO_ID"] + "'";
                        Obj.QryStringBuilder.Append(strQry);
                        Obj.QryStringBuilder.Append(";");
                        ObjBseCon.ExecuteQry(strQry);
                    }
                    for (int l = 0; l < dtWfo.Rows.Count; l++)
                    {
                        strQry = "DELETE FROM \"TBLWFODATA\" WHERE \"WFO_ID\" = '" + dtWfo.Rows[l]["WO_WFO_ID"] + "'";
                        Obj.QryStringBuilder.Append(strQry);
                        Obj.QryStringBuilder.Append(";");
                        ObjBseCon.ExecuteQry(strQry);
                    }
                }
                Arr[2] = Convert.ToString(Obj.QryStringBuilder);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name,
                   MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
            return Arr;
        }
    }
}
