using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IIITS.PGSQL.DAL;
using System.Data;
using Npgsql;
using System.Configuration;
using NpgsqlTypes;
using System.Globalization;
using System.Reflection;
using IIITS.DTLMS.BL.DataBase;

namespace IIITS.DTLMS.BL
{
    public class clsAllotement
    {
        public string sTotalTC { get; set; }
        public string sCapacity { get; set; }
        public string sStoreId { get; set; }
        public string sRatingId { get; set; }
        public string sDivId { get; set; }
        public string sMakeId { get; set; }
        public string sDiId { get; set; }
        public string sALTid { get; set; }
        public string sDINo { get; set; }
        public string sALTNumber { get; set; }
        public DataTable dtAllotement { get; set; }
        public Byte[] ALTFile { get; set; }
        public string sFileExt { get; set; }
        public string sCrby { get; set; }
        public string sDimid { get; set; }
        public string sDino { get; set; }
        public string sStorename { get; set; }
        public string sRating { get; set; }
        public string sTotqty { get; set; }
        public string sPendingQty { get; set; }
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
        public StringBuilder Validation { set; get; }


        public string strFormCode = "clsAllotement";

        PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
        DataBseConnection ObjBseCon = new DataBseConnection(Constants.Password);
        NpgsqlCommand NpgsqlCommand;

       
        /// <summary>
        /// to get a dispatch count 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public object GetDispatchCount(clsAllotement obj)
        {
            DataTable DtDispatchCount = new DataTable();
            try
            {
                #region Old Query
                //string sQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //sQry = " select \"DIM_ID\" from \"TBLDELIVERYINSTMASTER\" where \"DIM_DI_NO\"='" + obj.sDINo + "'";
                //obj.sDimid = objcon.get_value(sQry, NpgsqlCommand);
                //if (obj.sDimid != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("DIMid", Convert.ToInt16(obj.sDimid));
                //    sQry = "SELECT cast(SUM(\"DI_QUANTITY\") as int4) FROM \"TBLDELIVERYINSTRUCTION\"  WHERE \"DI_DIM_ID\" =:DIMid  and \"DI_STATUS\"=1 ";
                //    if (obj.sCapacity != "" && obj.sCapacity != null)
                //    {
                //        NpgsqlCommand.Parameters.AddWithValue("Capacity", obj.sCapacity);
                //        sQry += " AND cast(\"DI_CAPACITY\" as text)=:Capacity ";
                //    }
                //    var val = objcon.get_value(sQry, NpgsqlCommand);
                //    obj.sTotalTC = val;
                //}
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_dispatch_count");
                cmd.Parameters.AddWithValue("p_dino", (obj.sDINo ?? ""));
                cmd.Parameters.AddWithValue("p_capacity", (obj.sCapacity ?? ""));
                DtDispatchCount = objcon.FetchDataTable(cmd);
                if (DtDispatchCount.Rows.Count > 0)
                {
                    obj.sTotalTC = Convert.ToString(DtDispatchCount.Rows[0]["DI_QUANTITY"]);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "0";
            }
            return obj;
        }
        /// <summary>
        /// to get a inward count
        /// </summary>
        /// <param name="objAllot"></param>
        /// <returns></returns>
        public DataTable GetInwardedCount(clsAllotement objAllot)
        {            
            DataTable dtIndentDetails = new DataTable();
            try
            {
                #region Old GetInwardedCount
                //string strQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("AltNo", Convert.ToString(objAllot.sALTNumber));
                //strQry = " select  COUNT(\"TC_CAPACITY\") as \"INWARDED\" from \"TBLTCMASTER\" where \"TC_ALT_NO\"=:AltNo  and ";
                //strQry += " \"TC_CAPACITY\"='" + objAllot.sCapacity + "'  AND  \"TC_MAKE_ID\" ='" + objAllot.sMakeId + "' and \"TC_STORE_ID\"= ";
                //strQry += "  '" + objAllot.sStoreId + "' AND \"TC_DIV_ID\"='" + objAllot.sDivId + "'   GROUP BY \"TC_CAPACITY\" ";
                //dtIndentDetails = objcon.FetchDataTable(strQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_inwarded_count");
                cmd.Parameters.AddWithValue("p_altno", Convert.ToString(objAllot.sALTNumber ?? ""));
                cmd.Parameters.AddWithValue("p_capacity", (objAllot.sCapacity ?? ""));
                cmd.Parameters.AddWithValue("p_makeid", (objAllot.sMakeId ?? ""));
                cmd.Parameters.AddWithValue("p_storeid", (objAllot.sStoreId ?? ""));
                cmd.Parameters.AddWithValue("p_divid", (objAllot.sDivId ?? ""));
                dtIndentDetails = objcon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtIndentDetails;
            }
            return dtIndentDetails;
        }
       
        /// <summary>
        /// to get a allotment details
        /// </summary>
        /// <param name="sAlt_no"></param>
        /// <returns></returns>
        public DataTable GetAllotedDetails(string sAlt_no)
        {            
            DataTable dt = new DataTable();
            try
            {
                #region Old GetAllotedDetails
                //string sQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //NpgsqlCommand.Parameters.AddWithValue("Altno", sAlt_no.ToUpper());
                //sQry = "  select \"ALT_ID\",\"DI_NO\" AS \"ALT_DI_NO\",\"ALT_NO\",TO_CHAR(\"ALT_DATE\",'DD/MM/YYYY') AS \"ALT_DATE\", ";
                //sQry += " \"ALT_STORE_ID\",\"SM_NAME\" AS \"ALT_STORE_NAME\",\"DIV_NAME\" AS \"DIV_NAME\",\"ALT_CAPACITY\",\"ALT_CAPACITY_ID\" ";
                //sQry += " ,\"TM_NAME\" AS \"MAKE_NAME\",\"ALT_STAR_TYPE\" ,\"MD_NAME\" AS \"ALT_STARRATENAME\" ,";
                //sQry += " \"ALT_DIV_ID\",\"ALT_MAKE_ID\",\"ALT_QUANTITY\" FROM  \"TBLDIVISION\", \"TBLALLOTEMENT\",\"TBLDELIVERYINSTRUCTION\" ";
                //sQry += " ,\"TBLTRANSMAKES\",\"TBLSTOREMAST\",\"TBLMASTERDATA\" WHERE \"ALT_STAR_TYPE\"=\"MD_ID\" AND \"MD_TYPE\"='SR' AND ";
                //sQry += " \"DI_NO\"=\"ALT_DI_NO\"  AND \"TM_ID\"=\"ALT_MAKE_ID\"   AND\"ALT_DIV_ID\"=\"DIV_ID\" AND \"ALT_STATUS\"=1  ";
                //sQry += " AND \"SM_ID\"=\"ALT_STORE_ID\" ";
                //if (sAlt_no != "" && sAlt_no != null)
                //{
                //    sQry += "  and \"ALT_NO\"= :Altno ";
                //}
                //else
                //{
                //    sQry += "  and \"ALT_NO\" like '%' ";
                //}
                //sQry += " GROUP BY  \"ALT_ID\",\"DI_NO\",\"ALT_NO\",\"ALT_DATE\",\"ALT_STORE_ID\",\"SM_NAME\",\"DIV_NAME\" ";
                //sQry += " ,\"ALT_CAPACITY\",\"ALT_CAPACITY_ID\",\"MD_NAME\" ,\"ALT_MAKE_ID\" ,\"TM_NAME\" ,\"ALT_STAR_TYPE\", ";
                //sQry += " \"ALT_QUANTITY\",\"ALT_DIV_ID\"  ORDER BY \"ALT_ID\"";
                //dt = objcon.FetchDataTable(sQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_tc_allotement_details");
                cmd.Parameters.AddWithValue("p_alt_no", Convert.ToString(sAlt_no ?? "").ToUpper());
                dt = objcon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
            return dt;
        }
        /// <summary>
        ///  to get a delevery details for allotment
        /// </summary>
        /// <param name="objAllot"></param>
        /// <returns></returns>
        public DataTable GetDeliveryDetails(clsAllotement objAllot)
        {
            NpgsqlCommand = new NpgsqlCommand();
            DataTable dt = new DataTable();
            try
            {
                #region // old query
                //sQry =" select \"DI_ID\",\"DI_NO\",  \"SM_NAME\" AS \"STORE_NAME\",\"TM_NAME\" AS \"MAKE_NAME\",\"DI_CAPACITY\",\"MD_NAME\"AS \"STAR_RATE\",";
                //sQry +="  \"DI_QUANTITY\" AS \"TOTAL\",COALESCE((\"DI_QUANTITY\") - SUM(COALESCE(\"ALT_QUANTITY\",0)),0) \"PENDING\" FROM \"TBLDELIVERYINSTRUCTION\",";
                //sQry += " \"TBLTRANSMAKES\",\"TBLALLOTEMENT\",\"TBLSTOREMAST\",\"TBLMASTERDATA\" WHERE \"DI_STORE_ID\"=\"SM_ID\" AND \"TM_ID\"=\"DI_MAKE_ID\" AND \"MD_TYPE\"='SR' ";
                // if (objDel.sDINo != "" && objDel.sDINo != null)
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("DINO", objDel.sDINo);
                //    sQry += "AND \"DI_NO\"=:DINO  ";
                //}
                //if (objDel.sDiId != "" && objDel.sDiId != null)
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("DiID", objDel.sDiId);
                //    sQry += "AND cast(\"DI_ID\" as text)=:DiID  ";
                //}
                //sQry += " AND \"MD_ID\"=\"DI_STARTTYPE\" GROUP BY  \"DI_ID\",\"DI_NO\", \"SM_NAME\",\"MAKE_NAME\" ,\"DI_CAPACITY\",\"MD_NAME\",\"DI_QUANTITY\" ORDER BY \"DI_ID\"";

                //dt = objcon.FetchDataTable(sQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("sp_getpending_allotment");
                cmd.Parameters.AddWithValue("di_no", objAllot.sDINo);
                dt = objcon.FetchDataTable(cmd);                
            }
            catch (Exception ex)
            {
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
            return dt;
        }        
        /// <summary>
        /// to get delevery details for allotment
        /// </summary>
        /// <param name="objAllot"></param>
        /// <returns></returns>
        public DataTable GetDeliveryViewDetails(clsAllotement objAllot)
        {            
            DataTable dt = new DataTable();
            try
            {
                #region Old Queary
                //string sQry = string.Empty;
                //NpgsqlCommand = new NpgsqlCommand();
                //sQry = " SELECT a.\"DI_ID\",A.\"DIM_DI_NO\",SUM(a.\"DI_QUANTITY\") AS \"TOTAL\" ,COALESCE(SUM(a.\"DI_QUANTITY\") - ";
                //sQry += " SUM(COALESCE(b.\"ALT_QUANTITY\",0)),0) \"PENDING\",a.\"DI_MAKE_ID\",";
                //sQry += " a.\"SM_NAME\" AS \"STORE_NAME\", a.\"TM_NAME\" as \"MAKE_NAME\",a.\"DI_CAPACITY\" ,a.\"MD_NAME\" ";
                //sQry += " AS \"STAR_RATE\" ,a.\"DI_TC_START_RANGE\" as \"START_RANGE\",a.\"DI_TC_END_RANGE\" as \"END_RANGE\" ,\"DIM_ID\",\"DIM_PO_ID\" ";
                //sQry += " ,case when \"DI_ID\" in (select \"TCP_DI_ID\" from \"TBLTCRANGEALLOTMENT\" ) then 'Approved' else 'Pending' end \"STATUS\"";
                //sQry += "  from (SELECT SUM(\"DI_QUANTITY\") as \"DI_QUANTITY\",\"DI_ID\" ,\"DIM_DI_NO\", \"SM_NAME\",\"DI_STATUS\",";
                //sQry += " \"DI_MAKE_ID\",\"DI_STORE_ID\",\"TM_NAME\",\"DI_CAPACITY\",\"DI_CAPACITY_ID\",\"MD_NAME\",\"DI_STARTTYPE\",'H' ";
                //sQry += " ||\"DI_TC_START_RANGE\" as \"DI_TC_START_RANGE\",'H'||\"DI_TC_END_RANGE\" as \"DI_TC_END_RANGE\",\"DIM_ID\",\"DIM_PO_ID\" ";
                //sQry += "  FROM \"TBLDELIVERYINSTMASTER\" inner join \"TBLDELIVERYINSTRUCTION\" on \"DIM_ID\"=\"DI_DIM_ID\" ,\"TBLSTOREMAST\", ";
                //sQry += " \"TBLTRANSMAKES\",\"TBLMASTERDATA\" where \"DI_STORE_ID\"=\"SM_ID\" and \"DI_MAKE_ID\" = \"TM_ID\" ";
                //if (objAllot.sDINo != "" && objAllot.sDINo != null)
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("DINO", objAllot.sDINo);
                //    sQry += "AND cast(\"DIM_DI_NO\" as text)=:DINO  ";
                //}
                //if (objAllot.sDiId != "" && objAllot.sDiId != null)
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("DiID", objAllot.sDiId);
                //    sQry += "AND cast(\"DI_ID\" as text)=:DiID  ";
                //}
                //sQry += "  and \"DI_STARTTYPE\"=\"MD_ID\" AND \"MD_TYPE\"='SR' and \"DI_STATUS\"=1  GROUP BY \"DI_ID\",\"DIM_DI_NO\",\"DI_CAPACITY\" ";
                //sQry += " ,\"DI_QUANTITY\",\"SM_NAME\", \"TM_NAME\",\"DI_CAPACITY\",\"DI_CAPACITY_ID\" ,\"MD_NAME\",\"DI_STORE_ID\",\"DI_STARTTYPE\"  ";
                //sQry += " , \"DI_STATUS\",\"DI_MAKE_ID\",\"DIM_ID\",\"DIM_PO_ID\" )a ";
                //sQry += " LEFT join (SELECT sum(\"ALT_QUANTITY\") as \"ALT_QUANTITY\",\"ALT_DI_NO\" ,\"ALT_CAPACITY_ID\",\"ALT_MAKE_ID\",\"ALT_STORE_ID\" ";
                //sQry += " ,\"ALT_STAR_TYPE\",\"ALT_STATUS\" from \"TBLALLOTEMENT\" GROUP BY \"ALT_CAPACITY\",\"ALT_DI_NO\",\"ALT_CAPACITY_ID\",\"ALT_MAKE_ID\" ";
                //sQry += " ,\"ALT_STORE_ID\",\"ALT_STAR_TYPE\",\"ALT_STATUS\")b  on a.\"DIM_DI_NO\"=b.\"ALT_DI_NO\" ";
                //sQry += "  AND A.\"DI_CAPACITY_ID\" =B.\"ALT_CAPACITY_ID\" AND A.\"DI_STORE_ID\"=B.\"ALT_STORE_ID\" AND A.\"DI_MAKE_ID\"=B.\"ALT_MAKE_ID\" ";
                //sQry += " AND A.\"DI_STARTTYPE\"=B.\"ALT_STAR_TYPE\" AND B.\"ALT_STATUS\"=1 AND A.\"DI_STATUS\"=1 GROUP BY \"DI_ID\",\"DIM_DI_NO\" ";
                //sQry += " ,\"SM_NAME\", \"TM_NAME\" ,\"DI_CAPACITY\",\"MD_NAME\" ,\"DI_MAKE_ID\",\"DI_CAPACITY_ID\",\"DI_STORE_ID\",\"DI_TC_START_RANGE\" ";
                //sQry += " ,\"DI_TC_END_RANGE\",\"DIM_ID\",\"DIM_PO_ID\"";
                //dt = objcon.FetchDataTable(sQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_delivery_view_details");
                cmd.Parameters.AddWithValue("p_dino", Convert.ToString(objAllot.sDINo ?? ""));
                cmd.Parameters.AddWithValue("p_diid", Convert.ToString(objAllot.sDiId ?? ""));
                dt = objcon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
            return dt;
        }        
        /// <summary>
        ///  Print Di
        /// </summary>
        /// <param name="objDi"></param>
        /// <returns></returns>
        public DataTable PrintDi(clsAllotement objDi)
        {
            DataTable dt = new DataTable();
            DataTable diDetails = new DataTable();
            string strQry = string.Empty;
            int start = Convert.ToInt32(objDi.sStartrange);
            int End = Convert.ToInt32(objDi.sEndrange);
            string pono = string.Empty;
            string QryKey = string.Empty;

            try
            {
                #region old inline queary
                //strQry = "select \"PO_NO\" from \"TBLPOMASTER\"  WHERE \"PO_ID\"='" + objDi.sPoid + "' ";
                //pono = objcon.get_value(strQry);
                #endregion

                QryKey = "GET_PO_NO";
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsallotement");
                cmd.Parameters.AddWithValue("p_key", QryKey);
                cmd.Parameters.AddWithValue("p_value_1", Convert.ToString(objDi.sPoid ?? ""));
                cmd.Parameters.AddWithValue("p_value_2", "");
                pono = ObjBseCon.StringGetValue(cmd);


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
                for (int j = start; j <= End; j++)
                {
                    DataRow dRow = dt.NewRow();
                    dRow["DI_id"] = objDi.sDino;
                    dRow["DI_DTRCODE"] = "H" + start;
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
                    dt.Rows.Add(dRow);
                    dt.AcceptChanges();
                    start++;
                }
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }       
        /// <summary>
        /// Load ALlotment details
        /// </summary>
        /// <param name="objDi"></param>
        /// <returns></returns>
        public DataTable LoadALlotmentdetails(clsAllotement objDi)
        {
            DataTable dt = new DataTable();
            try
            {
                #region old LoadALlotmentdetails
                //string strQry = string.Empty;
                //strQry = "select \"TCP_ID\",\"DI_ID\",\"TCP_TC_CODE\",\"PO_NO\",\"DIM_DI_NO\",\"SM_NAME\",\"MD_NAME\",\"DI_CAPACITY\",\"TM_NAME\",\"TCP_TC_SL_NO\" ";
                //strQry += ",to_char(\"TCP_MANUFACTURE_DATE\",'dd-MM-yyyy') as \"TCP_MANUFACTURE_DATE\",\"TCP_TC_LIFE_SPAN\",\"TCP_TC_WARRENTY_PERIOD\" ";
                //strQry += ",(CASE WHEN \"TCP_OIL_TYPE\"='1' THEN 'MINERAL OIL' WHEN \"TCP_OIL_TYPE\"='2' THEN 'ESTER OIL' END) \"TCP_OIL_TYPE\",";
                //strQry += "\"TCP_OIL_CAPACITY\",\"TCP_OIL_WEIGHT\" from \"TBLDELIVERYINSTMASTER\" inner join ";
                //strQry += "\"TBLDELIVERYINSTRUCTION\" on \"DIM_ID\"=\"DI_DIM_ID\",\"TBLSTOREMAST\",\"TBLMASTERDATA\",\"TBLTRANSMAKES\",";
                //strQry += "\"TBLTCRANGEALLOTMENT\",\"TBLPOMASTER\" where\"DI_STORE_ID\"=\"SM_ID\" and \"PO_ID\"=\"DI_PO_ID\" and \"DI_MAKE_ID\" = ";
                //strQry += "\"TM_ID\" AND \"TCP_DI_ID\"=\"DI_ID\"  AND cast(\"DIM_DI_NO\" as text)='" + objDi.sDino + "'and \"DI_STARTTYPE\"=\"MD_ID\" AND ";
                //strQry += "\"MD_TYPE\"='SR' and \"DI_STATUS\"=1 and  \"TCP_TC_CODE\" >= '" + objDi.sStartrange + "' AND  \"TCP_TC_CODE\" <= '" + objDi.sEndrange + "' ORDER BY \"TCP_TC_CODE\"";
                //dt = objcon.FetchDataTable(strQry);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_allotment_details");
                cmd.Parameters.AddWithValue("p_dino", Convert.ToString(objDi.sDino ?? ""));
                cmd.Parameters.AddWithValue("p_startrange", Convert.ToString(objDi.sStartrange ?? ""));
                cmd.Parameters.AddWithValue("p_endrange", Convert.ToString(objDi.sEndrange ?? ""));
                dt = objcon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
            return dt;
        }
        /// <summary>
        /// Save Allotment Upload Details
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sPath"></param>
        /// <returns></returns>
        public bool SaveAllotmentUploadDetails(DataTable dt, string sPath, string CrBy)
        {
            NpgsqlCommand = new NpgsqlCommand();

            DataTable dtnew = new DataTable();
            bool result = false;
            string strQry = string.Empty;
            string[] Arr = new string[3];
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objcon.BeginTransaction();

                    string sDim_Id = Convert.ToString(dt.Rows[i]["DI_id"]);
                    string sDtrcode = Convert.ToString(dt.Rows[i]["DTR Code"]);
                    string sSlNo = Convert.ToString(dt.Rows[i]["Sl No"]);

                    string sPoNo = Convert.ToString(dt.Rows[i]["DI_PONO"]);
                    string sDino = Convert.ToString(dt.Rows[i]["DI No"]);
                    string sMakeid = Convert.ToString(dt.Rows[i]["Make Id"]);
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
                    string sStatus = "0";


                    clsException.WriteLogFile("clsAllotement", "SaveAllotmentUploadDetails", sManufacturedate, "before");
                    if (sManufacturedate != "")
                    {
                        string Flag = "1";
                        if (Flag == ConfigurationManager.AppSettings["AllotmentLiveFlag"])
                        {
                            //This Line For Testing Server * Mandatory *
                            DateTime stoDate = DateTime.ParseExact(sManufacturedate, "M/d/yyyy h:m:s tt", System.Globalization.CultureInfo.InvariantCulture);
                             sManufacturedate = Convert.ToDateTime(stoDate).ToString("yyyy/M/d");
                            //sManufacturedate = Convert.ToDateTime(stoDate).ToString("d/M/yyyy");
                        }
                        else
                        {
                            DateTime source = Convert.ToDateTime(sManufacturedate);
                            sManufacturedate = source.ToString("yyyy-MM-dd");
                        }
                        clsException.WriteLogFile("clsAllotement", "SaveAllotmentUploadDetails", sManufacturedate, "After");
                    }
                    if (sOiltype.Trim().ToUpper() == "MINERAL")
                    {
                        sOiltype = "1";
                    }
                    if (sOiltype.Trim().ToUpper() == "ESTER")
                    {
                        sOiltype = "2";
                    }
                    NpgsqlCommand cmd = new NpgsqlCommand("proc_save_allotmentDetailsfromexcel");
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
                    cmd.Parameters.AddWithValue("crby", Convert.ToString(CrBy));

                    cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd.Parameters.Add("pk_id", NpgsqlDbType.Text);
                    cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                    cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                    cmd.Parameters["pk_id"].Direction = ParameterDirection.Output;
                    Arr[2] = "pk_id";
                    Arr[1] = "op_id";
                    Arr[0] = "msg";
                    Arr = objcon.Execute(cmd, Arr, 3);
                }
                if (Arr[1] == "0")
                {
                    objcon.CommitTransaction();
                    return result = true;
                }
                else
                {
                    objcon.RollBackTrans();
                }
                return result;
            }
            catch (Exception ex)
            {
                objcon.RollBackTrans();
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return result;
            }
        }
        public clsAllotement ValidateExcelSheet(DataTable dt, string diid, string dino)
        {
            clsAllotement objDIS = new clsAllotement();
            string QryKey = string.Empty;
            try
            {
                int iRow = 2;
                string strQry = string.Empty;
                objDIS.Validation = new StringBuilder();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sResult = "";
                    string sDim_Id = Convert.ToString(dt.Rows[i]["DI_id"]);
                    string sDtrcode = Convert.ToString(dt.Rows[i]["DTR Code"]);
                    string sSlNo = Convert.ToString(dt.Rows[i]["Sl No"]);
                    string sPoNo = Convert.ToString(dt.Rows[i]["DI_PONO"]);
                    string sDino = Convert.ToString(dt.Rows[i]["DI No"]);
                    string sMakeid = Convert.ToString(dt.Rows[i]["Make Id"]);
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
                    string sStart_Range = string.Empty;
                    string sEnd_Range = string.Empty;
                    
                    #region Old inline query
                    //sStart_Range = objcon.get_value("SELECT \"DI_TC_START_RANGE\" FROM \"TBLDELIVERYINSTRUCTION\" WHERE \"DI_ID\" ='" + diid + "'");
                    #endregion

                    QryKey = "GET_DI_TC_START_RANGE";
                    NpgsqlCommand cmd_START_RANGE = new NpgsqlCommand("fetch_getvalue_clsallotement");
                    cmd_START_RANGE.Parameters.AddWithValue("p_key", QryKey);
                    cmd_START_RANGE.Parameters.AddWithValue("p_value_1", Convert.ToString(diid ?? ""));
                    cmd_START_RANGE.Parameters.AddWithValue("p_value_2", "");
                    sStart_Range = ObjBseCon.StringGetValue(cmd_START_RANGE);

                    #region Old inline query
                    //sEnd_Range = objcon.get_value("SELECT \"DI_TC_END_RANGE\" FROM \"TBLDELIVERYINSTRUCTION\" WHERE \"DI_ID\" ='" + diid + "'");
                    #endregion

                    QryKey = "GET_DI_TC_END_RANGE";
                    NpgsqlCommand cmd_END_RANGE = new NpgsqlCommand("fetch_getvalue_clsallotement");
                    cmd_END_RANGE.Parameters.AddWithValue("p_key", QryKey);
                    cmd_END_RANGE.Parameters.AddWithValue("p_value_1", Convert.ToString(diid ?? ""));
                    cmd_END_RANGE.Parameters.AddWithValue("p_value_2", "");
                    sEnd_Range = ObjBseCon.StringGetValue(cmd_END_RANGE);

                    string tdstartcode = sDtrcode.Substring(1, 6);
                    if (Convert.ToInt32(tdstartcode) >= Convert.ToInt32(sStart_Range) && Convert.ToInt32(tdstartcode) <= Convert.ToInt32(sEnd_Range))
                    {
                        string Temp = string.Empty;

                        #region Old inline query
                        //strQry = "select \"TCP_ID\" from \"TBLTCRANGEALLOTMENT\"  WHERE \"TCP_TC_SL_NO\"='" + sSlNo + "' and \"TCP_TC_CODE\"<>'" + sDtrcode + "' ";
                        //Temp = objcon.get_value(strQry);
                        #endregion

                        QryKey = "GET_TCP_ID";
                        NpgsqlCommand cmd_TCP_ID = new NpgsqlCommand("fetch_getvalue_clsallotement");
                        cmd_TCP_ID.Parameters.AddWithValue("p_key", QryKey);
                        cmd_TCP_ID.Parameters.AddWithValue("p_value_1", Convert.ToString(sSlNo ?? ""));
                        cmd_TCP_ID.Parameters.AddWithValue("p_value_2", Convert.ToString(sDtrcode ?? ""));
                        Temp = ObjBseCon.StringGetValue(cmd_TCP_ID);

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
                            if (sDtrcode.Trim().Length != 7)
                            {
                                sResult += "Tc Code : " + iRow + " ,Tc Code Length Should Be 7 ||";
                            }
                            if (!sDtrcode.StartsWith("H"))
                            {
                                sResult += "Tc Code : " + iRow + " ,Tc Code Length Should Be Start With H ||";
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
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objDIS;
            }
        }

        #region UnUsed Queary
        ///// <summary>
        ///// to get allotment details
        ///// </summary>
        ///// <param name="objAllotement"></param>
        ///// <returns></returns>
        //public DataTable GetAllotmentDetails(clsAllotement objAllotement)
        //{
        //    NpgsqlCommand = new NpgsqlCommand();
        //    DataTable dtIndentDetails = new DataTable();
        //    try
        //    {
        //        string strQry = string.Empty;
        //        NpgsqlCommand.Parameters.AddWithValue("AltID", Convert.ToInt32(objAllotement.sALTid));
        //        strQry = "select \"ALT_ID\",\"ALT_NO\",\"ALT_DI_NO\",TO_CHAR(\"ALT_DATE\",'dd-MM-yyyy')\"ALT_DATE\",\"ALT_STORE_ID\", ";
        //        strQry += " \"ALT_DIV_ID\" ,\"SM_NAME\" AS \"ALT_STORE_NAME\" , \"ALT_CAPACITY\",\"ALT_CAPACITY_ID\", \"ALT_STAR_TYPE\", ";
        //        strQry += " \"MD_NAME\"  AS \"ALT_STARRATENAME\",\"ALT_QUANTITY\", \"ALT_FILE_PATH\" from \"TBLALLOTEMENT\" ,\"TBLSTOREMAST\",\"TBLMASTERDATA\" ";
        //        strQry += " WHERE \"ALT_STAR_TYPE\"=\"MD_ID\" AND \"MD_TYPE\"='SR'  AND \"SM_ID\"=\"ALT_STORE_ID\" AND  \"ALT_ID\"=:AltID  AND \"ALT_STATUS\"=1 ";

        //        dtIndentDetails = objcon.FetchDataTable(strQry, NpgsqlCommand);

        //        return dtIndentDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return dtIndentDetails;
        //    }
        //}
        ///// <summary>
        ///// to get pending tc details
        ///// </summary>
        ///// <param name="objAllotement"></param>
        ///// <returns></returns>
        //public DataTable GetPendingTc(clsAllotement objAllotement)
        //{
        //    NpgsqlCommand = new NpgsqlCommand();
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        string strQry = string.Empty;

        //        strQry = " SELECT a.\"DI_ID\",SUM(a.\"DI_QUANTITY\"),COALESCE(SUM(a.\"DI_QUANTITY\") - SUM(COALESCE(b.\"ALT_QUANTITY\",0)),0) ";
        //        strQry += " \"PENDING\" from (SELECT SUM(\"DI_QUANTITY\") as \"DI_QUANTITY\",\"DI_ID\",\"DI_NO\" ,\"DI_CAPACITY_ID\",\"DI_STORE_ID\" ";
        //        strQry += " ,\"DI_MAKE_ID\",\"DI_STARTTYPE\" FROM \"TBLDELIVERYINSTRUCTION\" where \"DI_STATUS\"=1 AND";

        //        if (objAllotement.sDINo != "" && objAllotement.sDINo != null)
        //        {
        //            NpgsqlCommand.Parameters.AddWithValue("DINO", objAllotement.sDINo);
        //            strQry += "  \"DI_NO\"=:DINO  and ";
        //        }
        //        if (objAllotement.sCapacity != "" && objAllotement.sCapacity != null)
        //        {
        //            NpgsqlCommand.Parameters.AddWithValue("DiCap", objAllotement.sCapacity);
        //            strQry += " cast(\"DI_CAPACITY\" as text)=:DiCap  and ";
        //        }
        //        if (objAllotement.sStoreId != "" && objAllotement.sStoreId != null)
        //        {
        //            NpgsqlCommand.Parameters.AddWithValue("St_id", objAllotement.sStoreId);
        //            strQry += " cast(\"DI_STORE_ID\" as text)=:St_id  and ";
        //        }
        //        if (objAllotement.sMakeId != "" && objAllotement.sMakeId != null)
        //        {
        //            NpgsqlCommand.Parameters.AddWithValue("DiMake", objAllotement.sMakeId);
        //            strQry += " cast(\"DI_MAKE_ID\" as text)=:DiMake AND  ";
        //        }
        //        if (objAllotement.sRatingId != "" && objAllotement.sRatingId != null)
        //        {
        //            NpgsqlCommand.Parameters.AddWithValue("DiRating", objAllotement.sRatingId);
        //            strQry += " cast(\"DI_STARTTYPE\" as text)=:DiRating  ";
        //        }


        //        strQry += "  GROUP BY \"DI_ID\",\"DI_CAPACITY\",\"DI_NO\",\"DI_CAPACITY_ID\",\"DI_STORE_ID\",\"DI_MAKE_ID\",\"DI_STARTTYPE\") ";
        //        strQry += " a left join (SELECT sum(\"ALT_QUANTITY\") as \"ALT_QUANTITY\",\"ALT_DI_NO\",\"ALT_CAPACITY_ID\" ,\"ALT_STORE_ID\", ";
        //        strQry += " \"ALT_MAKE_ID\",\"ALT_STATUS\",\"ALT_STAR_TYPE\" from \"TBLALLOTEMENT\" ";
        //        strQry += " GROUP BY \"ALT_CAPACITY\",\"ALT_DI_NO\",\"ALT_CAPACITY_ID\" ,\"ALT_STORE_ID\",\"ALT_MAKE_ID\",\"ALT_STATUS\", ";
        //        strQry += " \"ALT_STAR_TYPE\" )b on a.\"DI_NO\"=b.\"ALT_DI_NO\" AND A.\"DI_CAPACITY_ID\"=B.\"ALT_CAPACITY_ID\" AND A.\"DI_STORE_ID\" ";
        //        strQry += " =B.\"ALT_STORE_ID\"  AND A.\"DI_STARTTYPE\"=B.\"ALT_STAR_TYPE\" AND A.\"DI_MAKE_ID\"=B.\"ALT_MAKE_ID\" AND \"ALT_STATUS\"=1 ";
        //        strQry += " GROUP BY \"DI_ID\",\"DI_NO\" ,\"DI_CAPACITY_ID\",\"DI_STORE_ID\",\"DI_MAKE_ID\"";

        //        dt = objcon.FetchDataTable(strQry, NpgsqlCommand);

        //        return dt;

        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return dt;
        //    }
        //}
        //public string[] SaveDeliveryDetails(clsAllotement objAllotement)
        //{
        //    NpgsqlCommand = new NpgsqlCommand();
        //    string[] Arr = new string[3];
        //    try
        //    {
        //        string sQry = string.Empty;
        //        DataTable dt = new DataTable();
        //        dt = objAllotement.dtAllotement;
        //        string str;
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            string sAlt_Id = Convert.ToString(dt.Rows[i]["ALT_ID"]);
        //            string sALTNo = Convert.ToString(dt.Rows[i]["ALT_NO"]);
        //            string sDiNO = Convert.ToString(dt.Rows[i]["ALT_DI_NO"]);
        //            string sStoreId = Convert.ToString(dt.Rows[i]["ALT_STORE_ID"]);
        //            string sDivId = Convert.ToString(dt.Rows[i]["ALT_DIV_ID"]);
        //            string sAltDate = Convert.ToString(dt.Rows[i]["ALT_DATE"]);
        //            string sCapacity = Convert.ToString(dt.Rows[i]["ALT_CAPACITY"]);
        //            string sCapacityID = Convert.ToString(dt.Rows[i]["ALT_CAPACITY_ID"]);
        //            string sStartype = Convert.ToString(dt.Rows[i]["ALT_STAR_TYPE"]);
        //            string sQuantity = Convert.ToString(dt.Rows[i]["ALT_QUANTITY"]);
        //            string sMake = Convert.ToString(dt.Rows[i]["ALT_MAKE_ID"]);

        //            long Id = objcon.Get_max_no("ALT_ID", "TBLALLOTEMENT");
        //            if (sAlt_Id == "")
        //            {
        //                NpgsqlCommand.Parameters.AddWithValue("Alt_no", sALTNo.ToUpper());
        //                NpgsqlCommand.Parameters.AddWithValue("DiNo", sDiNO.ToUpper());
        //                string qry = "select * from \"TBLALLOTEMENT\",\"TBLDELIVERYINSTRUCTION\" where cast(\"ALT_NO\" as text) ";
        //                qry += " =:Alt_no  AND \"DI_NO\"=\"ALT_DI_NO\" AND CAST(\"DI_NO\" AS text) <>:DiNo ";
        //                str = objcon.get_value(qry, NpgsqlCommand);
        //                if (str != null && str != "")
        //                {

        //                    Arr[0] = "Entered Allotment Number Already mapped with some other DI  Number";
        //                    Arr[1] = "2";
        //                    return Arr;
        //                }
        //                NpgsqlCommand cmd = new NpgsqlCommand("proc_saveupdate_altmaster");
        //                cmd.Parameters.AddWithValue("alt_id", sAlt_Id);
        //                cmd.Parameters.AddWithValue("alt_no", sALTNo.ToUpper());
        //                cmd.Parameters.AddWithValue("di_no", sDiNO.ToUpper());
        //                cmd.Parameters.AddWithValue("div_id", sDivId);
        //                cmd.Parameters.AddWithValue("alt_crby", objAllotement.sCrby);
        //                cmd.Parameters.AddWithValue("alt_date", sAltDate);
        //                cmd.Parameters.AddWithValue("alt_store_id", sStoreId);
        //                cmd.Parameters.AddWithValue("alt_star_rate", sStartype);
        //                cmd.Parameters.AddWithValue("alt_make_id", sMake);
        //                cmd.Parameters.AddWithValue("alt_quantity", sQuantity);
        //                cmd.Parameters.AddWithValue("capacity", sCapacity);
        //                cmd.Parameters.AddWithValue("capacity_id", sCapacityID);
        //                cmd.Parameters.AddWithValue("alt_satus", "1");

        //                cmd.Parameters.Add("msg", NpgsqlDbType.Text);
        //                cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
        //                cmd.Parameters.Add("pk_id", NpgsqlDbType.Text);

        //                cmd.Parameters["msg"].Direction = ParameterDirection.Output;
        //                cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
        //                cmd.Parameters["pk_id"].Direction = ParameterDirection.Output;
        //                Arr[0] = "msg";
        //                Arr[1] = "op_id";
        //                Arr[2] = "pk_id";
        //                Arr = objcon.Execute(cmd, Arr, 3);
        //                objAllotement.sALTid = Arr[2].ToString();

        //            }
        //            else
        //            {
        //                NpgsqlCommand.Parameters.AddWithValue("AltNo", sALTNo.ToUpper());
        //                str = "UPDATE \"TBLALLOTEMENT\" SET \"ALT_STATUS\"=0 WHERE \"ALT_NO\"=:AltNo";
        //                objcon.ExecuteQry(str, NpgsqlCommand);
        //                for (int j = 0; j < dt.Rows.Count; j++)
        //                {
        //                    sAlt_Id = Convert.ToString(dt.Rows[j]["ALT_ID"]);
        //                    sALTNo = Convert.ToString(dt.Rows[j]["ALT_NO"]);
        //                    sDiNO = Convert.ToString(dt.Rows[j]["ALT_DI_NO"]);
        //                    sStoreId = Convert.ToString(dt.Rows[j]["ALT_STORE_ID"]);
        //                    sDivId = Convert.ToString(dt.Rows[j]["ALT_DIV_ID"]);
        //                    sAltDate = Convert.ToString(dt.Rows[j]["ALT_DATE"]);
        //                    sCapacity = Convert.ToString(dt.Rows[j]["ALT_CAPACITY"]);
        //                    sCapacityID = Convert.ToString(dt.Rows[j]["ALT_CAPACITY_ID"]);
        //                    sStartype = Convert.ToString(dt.Rows[j]["ALT_STAR_TYPE"]);
        //                    sQuantity = Convert.ToString(dt.Rows[j]["ALT_QUANTITY"]);
        //                    sMake = Convert.ToString(dt.Rows[j]["ALT_MAKE_ID"]);

        //                    NpgsqlCommand cmd1 = new NpgsqlCommand("proc_saveupdate_altmaster");
        //                    cmd1.Parameters.AddWithValue("alt_id", sAlt_Id);
        //                    cmd1.Parameters.AddWithValue("alt_no", sALTNo.ToUpper());
        //                    cmd1.Parameters.AddWithValue("di_no", sDiNO.ToUpper());
        //                    cmd1.Parameters.AddWithValue("div_id", sDivId);
        //                    cmd1.Parameters.AddWithValue("alt_crby", objAllotement.sCrby);
        //                    cmd1.Parameters.AddWithValue("alt_date", sAltDate);
        //                    cmd1.Parameters.AddWithValue("alt_store_id", sStoreId);
        //                    cmd1.Parameters.AddWithValue("alt_star_rate", sStartype);
        //                    cmd1.Parameters.AddWithValue("alt_make_id", sMake);
        //                    cmd1.Parameters.AddWithValue("alt_quantity", sQuantity);
        //                    cmd1.Parameters.AddWithValue("capacity", sCapacity);
        //                    cmd1.Parameters.AddWithValue("capacity_id", sCapacityID);
        //                    cmd1.Parameters.AddWithValue("alt_satus", "1");

        //                    cmd1.Parameters.Add("msg", NpgsqlDbType.Text);
        //                    cmd1.Parameters.Add("op_id", NpgsqlDbType.Text);
        //                    cmd1.Parameters.Add("pk_id", NpgsqlDbType.Text);

        //                    cmd1.Parameters["msg"].Direction = ParameterDirection.Output;
        //                    cmd1.Parameters["op_id"].Direction = ParameterDirection.Output;
        //                    cmd1.Parameters["pk_id"].Direction = ParameterDirection.Output;
        //                    Arr[0] = "msg";
        //                    Arr[1] = "op_id";
        //                    Arr[2] = "pk_id";
        //                    Arr = objcon.Execute(cmd1, Arr, 3);
        //                    objAllotement.sALTid = Arr[2].ToString();

        //                    if (objAllotement.sFileExt.Length > 0)
        //                    {
        //                        NpgsqlParameter DeliveryNote = new NpgsqlParameter();
        //                        NpgsqlCommand comd = new NpgsqlCommand();
        //                        sQry = " UPDATE \"TBLALLOTEMENT\" SET \"ALT_FILE_PATH\" ";
        //                        sQry += " ='" + objAllotement.sFileExt + "' WHERE \"ALT_ID\" = '" + objAllotement.sALTid + "'";
        //                        NpgsqlConnection objconn = new NpgsqlConnection();
        //                        string strConnectionString = ConfigurationManager.ConnectionStrings["pgSQL"].ConnectionString + "Idea@12345+";
        //                        objconn.ConnectionString = strConnectionString; // "Server=localhost;Port=5432;Database=TestTRM;User Id=postgres;Password =Idea123";  
        //                        objconn.Open();
        //                        objcon.ExecuteQry(sQry, cmd1);
        //                        objconn.Close();
        //                    }
        //                }
        //                Arr[0] = "Updated Successfully";
        //                Arr[1] = "0";
        //                return Arr;
        //            }
        //            if (objAllotement.sFileExt.Length > 0)
        //            {
        //                NpgsqlParameter DeliveryNote = new NpgsqlParameter();
        //                NpgsqlCommand comd = new NpgsqlCommand();
        //                sQry = "  UPDATE \"TBLALLOTEMENT\" SET \"ALT_FILE_PATH\"='" + objAllotement.sFileExt + "' WHERE \"ALT_ID\" = '" + objAllotement.sALTid + "'";
        //                NpgsqlConnection objconn = new NpgsqlConnection();
        //                string strConnectionString = ConfigurationManager.ConnectionStrings["pgSQL"].ConnectionString + "Idea@12345+";
        //                objconn.ConnectionString = strConnectionString; // "Server=localhost;Port=5432;Database=TestTRM;User Id=postgres;Password =Idea123";  
        //                objconn.Open();
        //                objcon.ExecuteQry(sQry, NpgsqlCommand);
        //                objconn.Close();
        //            }
        //        }
        //        Arr[0] = "Saved Successfully";
        //        Arr[1] = "0";
        //        return Arr;
        //    }
        //    catch (Exception ex)
        //    {
        //        Arr[0] = "Failed to Save";
        //        Arr[1] = "1";
        //        clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return Arr;
        //    }
        //}

        #endregion
    }
}
