using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;
using System.Data;
using System.Data.OleDb;


namespace IIITS.DTLMS.BL
{
    public class clsStockAlert
    {
        PGSqlConnection objCon = new PGSqlConnection(Constants.Password);
        string strFormCode = "clsStockAlert";
       
        
        public string sFailureId { get; set; }
        public string sTcCapacity { get; set; }
        public string sIndentNo { get; set; }
        public string sCreatedBy { get; set; }
        public string sIndentid { get; set; }
        NpgsqlCommand NpgsqlCommand;
        public object GetTcDetails(clsStockAlert objStockAlert)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                string sQry = string.Empty;
                DataTable dt = new DataTable();
                string[] Arr = new string[2];
                //to check whether entered indent number exists or not
                NpgsqlCommand.Parameters.AddWithValue("IndentNo", objStockAlert.sIndentNo);
                String sIndentNo = objCon.get_value("select  \"TI_INDENT_NO\" from \"TBLINDENT\" where \"TI_INDENT_NO\"=:IndentNo", NpgsqlCommand);
                if (sIndentNo.Length > 0)
                {
                    NpgsqlCommand.Parameters.AddWithValue("IndentNo1",  objStockAlert.sIndentNo);
                    sQry = "SELECT \"TI_ID\", \"DF_ID\" , CAST(\"TC_CAPACITY\" AS TEXT)TC_CAPACITY FROM \"TBLTCMASTER\" ,\"TBLDTCFAILURE\",\"TBLINDENT\",\"TBLWORKORDER\" ";
                    sQry += " WHERE \"WO_SLNO\"=\"TI_WO_SLNO\" and \"WO_DF_ID\" = \"DF_ID\" AND \"TC_CODE\"=\"DF_EQUIPMENT_ID\" and  \"TI_INDENT_NO\" =:IndentNo1";
                    dt = objCon.FetchDataTable(sQry, NpgsqlCommand);
                    if (dt.Rows.Count > 0)
                    {
                        objStockAlert.sFailureId = dt.Rows[0]["DF_ID"].ToString();
                        objStockAlert.sTcCapacity = dt.Rows[0]["TC_CAPACITY"].ToString();
                        objStockAlert.sIndentid = dt.Rows[0]["TI_ID"].ToString();
                    }
                }
                else
                {
                    objStockAlert.sIndentid = "";
                }
                
                return objStockAlert;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objStockAlert;
            }
        }

        public string[] SaveStockAlert(clsStockAlert objStockAlert)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string strQry = string.Empty;
            string[] Arr = new string[2];
          
            try
            {

                //to check whether indent created user and loggedin user are the same
                NpgsqlCommand.Parameters.AddWithValue("CreatedBy", Convert.ToInt32(objStockAlert.sCreatedBy));
                NpgsqlCommand.Parameters.AddWithValue("Indentid", Convert.ToInt32(objStockAlert.sIndentid) );
                string sINNo = objCon.get_value("SELECT  \"TI_INDENT_NO\" from \"TBLINDENT\" where \"TI_CRBY\" =:CreatedBy AND \"TI_ID\" = :Indentid", NpgsqlCommand);
                if (sINNo.Length == 0)
                {                  
                    Arr[0] = "Authorised Person only can created the Alert for this Indent";
                    Arr[1] = "2";
                    return Arr;
                }

                //to check whether alert has already been generated for entered indent number
                NpgsqlCommand.Parameters.AddWithValue("Indentid1", Convert.ToInt32(objStockAlert.sIndentid));
                sINNo = objCon.get_value("select  \"TI_INDENT_NO\" from \"TBLINDENT\" where  \"TI_ID\" = :Indentid1 AND \"TI_ALERT_FLAG\" =1 ", NpgsqlCommand);
                if (sINNo.Length > 0)
                {                   
                    Arr[0] = "Alert has been already created for this Indent ";
                    Arr[1] = "2";
                    return Arr;
                }

                //to check whether invoice has been generated for the entered indent number
                NpgsqlCommand.Parameters.AddWithValue("Indentid2", Convert.ToInt32(objStockAlert.sIndentid));
                sINNo = objCon.get_value("select  \"IN_TI_NO\" from \"TBLDTCINVOICE\" where  \"IN_TI_NO\" =:Indentid2 ", NpgsqlCommand);
                if (sINNo.Length > 0)
                {
                   
                    Arr[0] = "Invoice Already Issued for this Indent";
                    Arr[1] = "2";
                    return Arr;
                }
                
                //to check whether tc available for the entered indent number
                NpgsqlCommand.Parameters.AddWithValue("Capacity", objStockAlert.sTcCapacity);
                sINNo = objCon.get_value("select \"TC_ID\" from \"TBLTCMASTER\" where \"TC_STATUS\" IN (1,2) and \"TC_CURRENT_LOCATION\"=1 and \"TC_CAPACITY\" =:Capacity", NpgsqlCommand);
                if (sINNo.Length > 0)
                {
                   
                    Arr[0] = "Requested Transformer Available in Store, Please collect it";
                    Arr[1] = "2";
                    return Arr;
                }
                
                //updating alert flag to 1 in Indent table
                NpgsqlCommand.Parameters.AddWithValue("IndentNo",  objStockAlert.sIndentNo );
                strQry = "UPDATE \"TBLINDENT\" SET \"TI_ALERT_FLAG\" =1 WHERE \"TI_INDENT_NO\" =:IndentNo";
                objCon.ExecuteQry(strQry, NpgsqlCommand);
                Arr[0] = "Alert has been Created, Once it is Available you will get the Alert";
                Arr[1] = "0";
                return Arr;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }
    }
}
