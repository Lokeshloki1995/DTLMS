using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IIITS.PGSQL.DAL;
using System.Data;
using Npgsql;


namespace IIITS.DTLMS.BL
{
    public class clsPoTracker
    {
        public string strFormCode = "clsPoTracker";
        public DataTable dtPODetails { get; set; }
        public DataTable dtDIDetails { get; set; }
        public DataTable dtDTRDetails { get; set; }
        public string sFinId { get; set; }
        public string sPoId { get; set; }
        public string sDINo { get; set; }

        PGSqlConnection objCon = new PGSqlConnection(Constants.Password);
        NpgsqlCommand NpgsqlCommand;
        /// <summary>
        /// Get PO Details
        /// </summary>
        /// <param name="objPo"></param>
        public void GetPODetails(clsPoTracker objPo)
        {
            try
            {
                #region Old inline query
                //NpgsqlCommand = new NpgsqlCommand();
                //string sQry = string.Empty;
                //if(sFinId.Length == 0)
                //{
                //    sQry = "SELECT \"PO_ID\", \"PO_NO\", TO_CHAR(\"PO_DATE\",'DD-MON-YYYY') \"PO_DATE\", \"TS_NAME\", \"PO_RATE\", SUM(\"PB_QUANTITY\") \"NOOFDTR\" FROM \"TBLPOMASTER\", ";
                //    sQry += "\"TBLTRANSSUPPLIER\", \"TBLPOOBJECTS\" WHERE \"PO_SUPPLIER_ID\"=\"TS_ID\" AND \"PO_ID\" = \"PB_PO_ID\" GROUP BY  \"PO_ID\", ";
                //    sQry += "\"PO_NO\", \"PO_DATE\", \"TS_NAME\", \"PO_RATE\" ORDER BY \"PO_DATE\" DESC";
                //    //proc_get_po_details();
                //}
                //else
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("FinId", Convert.ToInt32(objPo.sFinId));
                //    sQry = "SELECT \"PO_ID\", \"PO_NO\", TO_CHAR(\"PO_DATE\",'DD-MON-YYYY') \"PO_DATE\", \"TS_NAME\", \"PO_RATE\", SUM(\"PB_QUANTITY\") \"NOOFDTR\" FROM \"TBLPOMASTER\", ";
                //    sQry += " \"TBLTRANSSUPPLIER\", \"TBLPOOBJECTS\", \"TBLFINANCIALYEAR\" WHERE cast(\"PO_DATE\" as varchar) BETWEEN \"FY_START_DATE\" and \"FY_END_DATE\" ";
                //    sQry += " AND \"PO_SUPPLIER_ID\"=\"TS_ID\" AND \"PO_ID\" = \"PB_PO_ID\" AND \"FY_ID\" = :FinId GROUP BY  \"PO_ID\", \"PO_NO\", \"PO_DATE\", ";
                //    sQry += " \"TS_NAME\", \"PO_RATE\" ORDER BY \"PO_DATE\" DESC ";
                //}
                //objPo.dtPODetails = objCon.FetchDataTable(sQry, NpgsqlCommand);
                #endregion
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_po_details_for_clspotracker");
                cmd.Parameters.AddWithValue("p_finid", Convert.ToString(objPo.sFinId ?? ""));
                objPo.dtPODetails = objCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// Get Delivery Details
        /// </summary>
        /// <param name="objPo"></param>
        public void GetDeliveryDetails(clsPoTracker objPo)
        {

            try
            {
                #region Old inline query
                //NpgsqlCommand = new NpgsqlCommand();
                //string sQry = string.Empty;
                ////NpgsqlCommand.Parameters.AddWithValue("PoId", Convert.ToInt16(objPo.sPoId));
                ////sQry = "SELECT \"DI_ID\", \"DI_NO\", TO_CHAR(\"DI_DATE\",'DD-MON-YYYY') \"DI_DATE\", \"SM_NAME\", \"DI_CONSIGNEE\", \"TM_NAME\", \"DI_CAPACITY\", \"DI_QUANTITY\", COUNT(*) \"INWARD\", ";
                ////sQry += " (\"DI_QUANTITY\" - COUNT(*)) \"PENDING\" FROM \"TBLDELIVERYINSTRUCTION\" LEFT JOIN \"TBLTCMASTER\" ON \"DI_NO\" = \"TC_PO_NO\" AND \"DI_CAPACITY\" = \"TC_CAPACITY\" JOIN ";
                ////sQry += " \"TBLSTOREMAST\" ON \"DI_STORE_ID\" = \"SM_ID\" JOIN \"TBLTRANSMAKES\" ON \"TM_ID\" = \"DI_MAKE_ID\" WHERE \"DI_PO_ID\" = :PoId ";
                ////sQry += " GROUP BY \"DI_ID\", \"DI_NO\", \"DI_DATE\",\"SM_NAME\", \"DI_CONSIGNEE\", \"TM_NAME\", \"DI_QUANTITY\",\"DI_CAPACITY\"";
                ////objPo.dtDIDetails = objCon.FetchDataTable(sQry, NpgsqlCommand);

                //NpgsqlCommand.Parameters.AddWithValue("PoId", Convert.ToInt16(objPo.sPoId));
                //sQry = "SELECT \"DI_ID\", \"DIM_DI_NO\" \"DI_NO\", TO_CHAR(\"DIM_DI_DATE\", 'DD-MON-YYYY') \"DI_DATE\", ";
                //sQry += "    \"SM_NAME\", \"TM_NAME\", \"DI_CAPACITY\", \"DI_QUANTITY\", COUNT(*) \"INWARD\",  ";
                //sQry += " (\"DI_QUANTITY\" - COUNT(*)) \"PENDING\" FROM \"TBLDELIVERYINSTMASTER\" inner join ";
                //sQry += "    \"TBLDELIVERYINSTRUCTION\" on \"DIM_ID\" = \"DI_DIM_ID\" LEFT JOIN \"TBLTCMASTER\" ";
                //sQry += "   ON cast(\"DIM_PO_ID\" as text) = \"TC_PO_NO\" AND \"DI_CAPACITY\" = \"TC_CAPACITY\" ";
                //sQry += "  JOIN  \"TBLSTOREMAST\" ON \"DI_STORE_ID\" = \"SM_ID\" JOIN \"TBLTRANSMAKES\" ";
                //sQry += "  ON \"TM_ID\" = \"DI_MAKE_ID\" WHERE \"DI_PO_ID\" = :PoId  GROUP BY \"DI_ID\", ";
                //sQry += "  \"DIM_DI_NO\", \"DIM_DI_DATE\", \"SM_NAME\", \"TM_NAME\", \"DI_QUANTITY\", \"DI_CAPACITY\"";
                //objPo.dtDIDetails = objCon.FetchDataTable(sQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_delivery_details_for_clspotracker");
                cmd.Parameters.AddWithValue("p_poid", Convert.ToString(objPo.sPoId ?? ""));
                objPo.dtDIDetails = objCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// Get TC Details
        /// </summary>
        /// <param name="objPo"></param>
        public void GetTCDetails(clsPoTracker objPo)
        {
            try
            {
                #region Old inline query
                //NpgsqlCommand = new NpgsqlCommand();
                //string sQry = string.Empty;
                //NpgsqlCommand.Parameters.AddWithValue("DINo", objPo.sDINo);
                //sQry = "SELECT \"TC_CODE\", \"TC_SLNO\", \"TC_CAPACITY\", \"TC_MANF_DATE\", (SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'SR' AND ";
                //sQry += " \"MD_ID\" = \"TC_RATING\") \"TC_RATING\" FROM \"TBLTCMASTER\"  WHERE \"TC_DI_NO\" = :DINo";
                ////sQry += " \"MD_ID\" = \"TC_RATING\") \"TC_RATING\" FROM \"TBLTCMASTER\"  WHERE \"TC_PO_NO\" = :DINo";
                //objPo.dtDTRDetails = objCon.FetchDataTable(sQry, NpgsqlCommand);
                #endregion

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_tc_details_for_clspotracker");
                cmd.Parameters.AddWithValue("p_dino", Convert.ToString(objPo.sDINo ?? ""));
                objPo.dtDTRDetails = objCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}
