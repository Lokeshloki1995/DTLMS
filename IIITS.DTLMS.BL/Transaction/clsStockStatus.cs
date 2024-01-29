using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using IIITS.PGSQL.DAL;
using Npgsql;
namespace IIITS.DTLMS.BL
{
   public class clsStockStatus
    {
       string strFormCode = "clsStockStatus";
       PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
       public string sStoreId { get; set; }
       public string sStoreName { get; set; }
       public string sStorelocation { get; set; }
       public string sStockCount { get; set; }
       public string sCapacity { get; set; }
        public string sMake { get; set; }
        public string sRating { get; set; }

        public string roletype { get; set; }

        NpgsqlCommand NpgsqlCommand;
        #region unused methods
        // public DataTable LoadStockGrid(clsStockStatus ObjStore)
        //{
        //    NpgsqlCommand = new NpgsqlCommand();
        //    DataTable dtStoreDetails = new DataTable();
        //    string strQry = string.Empty;
        //    try
        //    {
        //         //strQry = "SELECT * FROM ( select SM_ID,SM_NAME ,(select SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':')+2) from VIEW_ALL_OFFICES where OFF_CODE=SM_OFF_CODE) SM_OFF_CODE,";
        //         //strQry+= " TO_CHAR(TC_CAPACITY) AS TC_CAPACITY,COUNT(TC_CODE)TC_CODE from TBLTCMASTER,TBLSTOREMAST WHERE TC_STORE_ID=SM_ID and TC_STATUS IN(1,2) AND ";
        //         //strQry += " TC_CURRENT_LOCATION=1  AND TC_CAPACITY IS NOT NULL GROUP BY SM_ID,SM_NAME,TC_CAPACITY,SM_OFF_CODE ORDER BY SM_NAME ) WHERE ";
        //         //strQry += " UPPER(COALESCE(SM_NAME,0)) like '%" + ObjStore.sStoreName.ToUpper() + "%' AND UPPER(COALESCE(SM_OFF_CODE,0)) LIKE '%" + ObjStore.sStorelocation.ToUpper() + "%'";
        //         //SUBSTR(\"OFF_NAME\", INSTR(\"OFF_NAME\", ':')+2)
        //        NpgsqlCommand.Parameters.AddWithValue("StoreName", ObjStore.sStoreName.ToUpper());
        //        strQry = "SELECT  \"SM_ID\",\"SM_NAME\",COALESCE(\"TC_CAPACITY\",'TOTAL') \"TC_CAPACITY\",SUM(\"TC_CODE\") \"TC_CODE\" FROM ( SELECT \"SM_ID\",\"SM_NAME\" ,(select \"OFF_NAME\"  from \"VIEW_ALL_OFFICES\" where \"OFF_CODE\"=\"SM_OFF_CODE\") \"SM_OFF_CODE\",";
        //        strQry += " CAST(\"TC_CAPACITY\" AS TEXT) AS \"TC_CAPACITY\" ,COUNT(\"TC_CODE\") \"TC_CODE\" from \"TBLTCMASTER\",\"TBLSTOREMAST\" WHERE \"TC_STORE_ID\"=\"SM_ID\" and \"TC_STATUS\" IN(1,2) AND ";
        //        strQry += " \"TC_CURRENT_LOCATION\"=1  AND \"TC_CAPACITY\" IS NOT NULL GROUP BY \"SM_ID\",\"SM_NAME\",\"TC_CAPACITY\",\"SM_OFF_CODE\" ORDER BY \"SM_NAME\" )A WHERE ";
        //        strQry += " UPPER(COALESCE(CAST(\"SM_NAME\" AS TEXT),'0')) like :StoreName||'%' ";
        //        if (ObjStore.sCapacity != "")
        //        {
        //            NpgsqlCommand.Parameters.AddWithValue("Capacity", Convert.ToDouble(ObjStore.sCapacity));
        //            strQry += " AND CAST(\"TC_CAPACITY\" AS NUMERIC)=:Capacity";
        //        }
        //        NpgsqlCommand.Parameters.AddWithValue("Storelocation", ObjStore.sStorelocation.ToUpper());
        //        strQry += " AND UPPER(COALESCE(CAST(\"SM_OFF_CODE\" AS TEXT), '0')) LIKE :Storelocation||'%'";
        //        strQry += " GROUP BY GROUPING SETS ((\"SM_ID\",\"SM_NAME\",\"SM_OFF_CODE\",\"TC_CAPACITY\",\"TC_CODE\"),\"SM_NAME\",()) ";
        //        strQry += "  ORDER BY \"SM_NAME\" ";


        //        dtStoreDetails = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
        //        return dtStoreDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //         clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //         return dtStoreDetails;

        //    }
        //}
        #endregion
        public DataTable LoadStockDetails(clsStockStatus objStock)
       {
           NpgsqlCommand = new NpgsqlCommand();
            DataTable dtStoreDetails = new DataTable();
            string sQry = string.Empty;
            try
            {

                if (objStock.roletype == "2")
                {
                    objStock.sStoreId = objStock.sStoreId;
                }
                else
                { 
                
                     if (objStock.sStoreId.Length < 3 && objStock.sStoreId.Length > 0)
                    {
                        objStock.sStoreId = ObjCon.get_value("SELECT \"STO_SM_ID\" FROM \"TBLSTOREOFFCODE\" WHERE \"STO_OFF_CODE\"  = '" + objStock.sStoreId + "%'");
                    }
                    else
                    {
                        objStock.sStoreId = clsStoreOffice.GetStoreID(objStock.sStoreId);

                    }
                    
                
                //if(objStock.sStoreId.Length <3 && objStock.sStoreId.Length>0)
                }



                sQry = "SELECT \"SM_NAME\",CAST(\"10\" AS INT) A,CAST(\"15\" AS INT) B,CAST(\"25\" AS INT) C,CAST(\"50\" AS INT) D,CAST(\"63\" AS INT) E,CAST(\"100\" AS INT) F,CAST(\"125\" AS INT) G,CAST(\"150\" AS INT)  H,CAST(\"160\" AS INT)  I,CAST(\"200\" AS INT)  J,CAST(\"250\" AS INT)  K,CAST(\"300\" AS INT)  L,CAST(\"315\" AS INT)  M,CAST(\"400\" AS INT)  N, ";
                sQry += " CAST(\"500\" AS INT)  O,CAST(\"630\" AS INT)  P,CAST(\"750\" AS INT)  Q,CAST(\"1000\" AS INT)  R,CAST(\"1250\" AS INT)  S, CAST(\"10\" AS INT)+CAST(\"15\" AS INT)+CAST(\"25\" AS INT)+CAST(\"50\" AS INT)+ ";
                sQry += " CAST(\"63\" AS INT)+CAST(\"100\" AS INT)+CAST(\"125\" AS INT)+CAST(\"150\" AS INT)+CAST(\"160\" AS INT)+CAST(\"200\" AS INT)+ ";
                sQry += " CAST(\"250\" AS INT)+CAST(\"300\" AS INT)+CAST(\"315\" AS INT)+CAST(\"400\" AS INT)+CAST(\"500\" AS INT)+CAST(\"630\" AS INT)+ ";
                sQry += " CAST(\"750\" AS INT)+CAST(\"1000\" AS INT)+CAST(\"1250\" AS INT) \"TOTAL\" from crosstab('SELECT \"SM_NAME\", ";
                sQry += " CAST(\"MD_NAME\" AS INT),CAST(COALESCE(\"NO OF TC\",0) AS VARCHAR) AS \"NO OF TC\" ";
                sQry += " from (SELECT \"MD_NAME\",\"SM_NAME\",\"SM_ID\" FROM \"TBLMASTERDATA\",\"TBLSTOREMAST\" WHERE \"MD_TYPE\" = ''C'' ";
                if (objStock.sStoreId.Length > 0)
                {
                   // NpgsqlCommand.Parameters.AddWithValue("StoreId", Convert.ToInt32(objStock.sStoreId));
                    sQry += " AND \"SM_ID\" = "+ Convert.ToInt32(objStock.sStoreId)+" ";
                }

                if (objStock.sCapacity.Length > 0)
                {
                  //  NpgsqlCommand.Parameters.AddWithValue("Capacity",objStock.sCapacity);
                    sQry += " AND \"MD_NAME\" = "+ objStock.sCapacity + "";
                }
                sQry += " )a LEFT JOIN ";
                sQry += " (SELECT \"TC_LOCATION_ID\" , CAST(\"TC_CAPACITY\" AS TEXT) AS \"TC_CAPACITY\" ,COUNT(\"TC_CODE\") \"NO OF TC\" FROM \"TBLTCMASTER\" ";
                sQry += " WHERE  \"TC_STATUS\" IN(1,2)   AND \"TC_CURRENT_LOCATION\" = 1  AND \"TC_CAPACITY\" IS NOT NULL ";
                sQry += " GROUP BY \"TC_LOCATION_ID\",\"TC_CAPACITY\")B ON \"MD_NAME\" = CAST(\"TC_CAPACITY\" AS TEXT)  AND \"TC_LOCATION_ID\"=\"SM_ID\" ";
                sQry += " ORDER BY 1,2') ";
                sQry += " as (\"SM_NAME\"  VARCHAR, \"10\" VARCHAR, \"15\" VARCHAR, \"25\" VARCHAR, \"50\" VARCHAR, \"63\" VARCHAR,  \"100\" VARCHAR, ";
                sQry += " \"125\" VARCHAR, \"150\" VARCHAR, \"160\" VARCHAR, \"200\" VARCHAR, \"250\" VARCHAR,  \"300\" VARCHAR, \"315\" VARCHAR, ";
                sQry += " \"400\" VARCHAR, \"500\" VARCHAR, \"630\" VARCHAR, \"750\" VARCHAR,  \"1000\" VARCHAR, \"1250\" VARCHAR) ";
                dtStoreDetails = ObjCon.FetchDataTable(sQry);
                return dtStoreDetails;
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtStoreDetails;
            }
       }


        public DataTable LoadTcDetails(clsStockStatus objStock)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string strQry = string.Empty;
            DataTable dtTcDetails = new DataTable();
            try
            {

                NpgsqlCommand.Parameters.AddWithValue("StoreId", objStock.sStoreId);
                strQry = "SELECT (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TM_ID\"=\"TC_MAKE_ID\") AS \"TC_MAKE_ID\",\"TC_ID\",cast(\"TC_CODE\" as TEXT) \"TC_CODE\",";
                strQry += "\"TC_SLNO\",CAST(\"TC_CAPACITY\" AS TEXT) AS \"TC_CAPACITY\", \"TC_LIFE_SPAN\" , (SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'SR' AND \"MD_ID\" = \"TC_RATING\") \"TC_RATING\"  FROM \"TBLTCMASTER\" ";
                strQry += " WHERE  \"TC_CURRENT_LOCATION\" = 1  AND CAST(\"TC_LOCATION_ID\" as TEXT) = :StoreId";
                if (objStock.sMake != null)
                {
                    NpgsqlCommand.Parameters.AddWithValue("Make", objStock.sMake);
                    strQry += " AND \"TC_MAKE_ID\"=:Make";
                }
                if (objStock.sCapacity != null)
                {
                    NpgsqlCommand.Parameters.AddWithValue("Capacity", Convert.ToDouble(objStock.sCapacity));
                    strQry += " AND \"TC_CAPACITY\"=:Capacity";
                }
                if (objStock.sRating != null)
                {
                    NpgsqlCommand.Parameters.AddWithValue("Rating", Convert.ToInt32(objStock.sRating));
                    strQry += " AND \"TC_RATING\"=:Rating";
                }
               
                strQry += " ORDER BY \"TC_ID\" DESC";
                dtTcDetails = ObjCon.FetchDataTable(strQry, NpgsqlCommand);
                return dtTcDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtTcDetails;
            }
        }

        public string GetStoreID(string sStoreName)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                NpgsqlCommand.Parameters.AddWithValue("StoreName", sStoreName);
                string sQry = string.Empty;
                sQry = "SELECT \"SM_ID\" FROM \"TBLSTOREMAST\" WHERE \"SM_NAME\" = :StoreName";
                string sStoreId = ObjCon.get_value(sQry, NpgsqlCommand);
                return sStoreId;
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "0";
            }
        }
    }
}
