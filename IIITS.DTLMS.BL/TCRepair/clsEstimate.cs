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
    public class clsEstimate
    {
        PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
        string strFormCode = "clsEstimate";

        public string eUser { get; set; }
        public string eVendortype { get; set; }
        public string eRpCenterName { get; set; }
        public string eDivision { get; set; }
        public string eFromDate { get; set; }
        public string eToDate { get; set; }
        public string eCapacity { get; set; }
        public string eBaseRate { get; set; }
        public string eTaxRate { get; set; }
        public string ePODate { get; set; }
        public string ePONumber { get; set; }
        public string ewoundType { get; set; }
        public string erateType { get; set; }
        public string sRecordId { get; set; }
        public string sWoundType { get; set; }
        public string srateType { get; set; }
        public string sCrBy { get; set; }
        public DataTable dtMaterials { get; set; }

        public string sOfficecode { get; set; }
        NpgsqlCommand NpgsqlCommand;
        public string[] SaveEstimate(clsEstimate objest, string[] sMaterial, string[] sLabour, string[] sSalvage)
        {
            string[] Arr = new string[2];
            try
            {
                string[] sMaterialVal = sMaterial.ToArray();
                string[] sLabourVal = sLabour.ToArray();
                string[] sSalvageVal = sSalvage.ToArray();

                long UId = objcon.Get_max_no("MRI_RECORD_ID", "TBLMINORREPAIRITEMRATEMASTER");

                for (int i = 0; i < sMaterialVal.Length; i++)
                {
                    if (sMaterialVal[i] != null)
                    {
                        long id = objcon.Get_max_no("MRI_ID", "TBLMINORREPAIRITEMRATEMASTER");
                        string sQry = string.Empty;

                        sQry = "INSERT INTO \"TBLMINORREPAIRITEMRATEMASTER\" (\"MRI_ID\", \"MRI_RECORD_ID\", \"MRI_VENDOR_TYPE\", \"MRI_TR_ID\", \"MRI_DIV_ID\", \"MRI_EFFECTIVE_FROM\", \"MRI_EFFECTIVE_TO\", \"MRI_PO_NO\", ";
                        sQry += " \"MRI_PO_DATE\", \"MRI_MRIM_ID\", \"MRI_CAPACITY\",\"MRI_WOUNDTYPE\", \"MRI_QUANTITY\", \"MRI_BASE_RATE\", \"MRI_TAX\", \"MRI_TOTAL\", \"MRI_MEASUREMENT\",\"MRI_CRBY\",\"MRI_REPAIR_CENTER\",\"MRI_RATETYPE\")";
                        sQry += " VALUES ( "+ id +","+ UId + " ,'" + objest.eVendortype + "','" + objest.eUser + "','" + objest.eDivision + "',TO_DATE('" + objest.eFromDate + "','DD/MM/YYYY'), ";
                        sQry += " TO_DATE('" + objest.eToDate + "','DD/MM/YYYY'),'" + objest.ePONumber + "',TO_DATE('" + objest.ePODate + "','DD/MM/YYYY'),'" + sMaterialVal[i].Split('~').GetValue(0).ToString() + "',";
                        sQry += " '" + objest.eCapacity + "','" + objest.ewoundType + "', '" + sMaterialVal[i].Split('~').GetValue(1).ToString() + "', '" + sMaterialVal[i].Split('~').GetValue(2).ToString() + "', ";
                        sQry += " '" + sMaterialVal[i].Split('~').GetValue(3).ToString() + "', '" + sMaterialVal[i].Split('~').GetValue(4).ToString() + "', ";
                        sQry += " '" + sMaterialVal[i].Split('~').GetValue(5).ToString() + "','" + objest.sCrBy + "','" + objest.eRpCenterName + "','" + objest.erateType + "') ";
                        objcon.ExecuteQry(sQry);

                        //long id1 = objcon.Get_max_no("MRI_ID", "TBLMINORREPAIRITEMRATEMASTER");
                        //sQry = "INSERT INTO \"TBLMINORREPAIRITEMRATEMASTER\" (\"MRI_ID\", \"MRI_IED_ID\")";
                        //sQry += " VALUES (" + id1 + ",'" + id + "' )";
                        //objcon.ExecuteQry(sQry);
                    }
                }

                for (int i = 0; i < sLabourVal.Length; i++)
                {
                    if (sLabourVal[i] != null)
                    {
                        long id = objcon.Get_max_no("MRI_ID", "TBLMINORREPAIRITEMRATEMASTER");
                        string sQry = string.Empty;

                        sQry = "INSERT INTO \"TBLMINORREPAIRITEMRATEMASTER\" ( \"MRI_ID\", \"MRI_RECORD_ID\", \"MRI_VENDOR_TYPE\", \"MRI_TR_ID\", \"MRI_DIV_ID\", \"MRI_EFFECTIVE_FROM\", \"MRI_EFFECTIVE_TO\", \"MRI_PO_NO\", ";
                        sQry += " \"MRI_PO_DATE\", \"MRI_MRIM_ID\", \"MRI_CAPACITY\",\"MRI_WOUNDTYPE\", \"MRI_QUANTITY\", \"MRI_BASE_RATE\", \"MRI_TAX\", \"MRI_TOTAL\", \"MRI_MEASUREMENT\",\"MRI_CRBY\",\"MRI_REPAIR_CENTER\",\"MRI_RATETYPE\")";
                        sQry += " VALUES ( " + id + "," + UId + " ,'" + objest.eVendortype + "','" + objest.eUser + "','" + objest.eDivision + "',TO_DATE('" + objest.eFromDate + "','DD/MM/YYYY'), ";
                        sQry += " TO_DATE('" + objest.eToDate + "','DD/MM/YYYY'),'" + objest.ePONumber + "',TO_DATE('" + objest.ePODate + "','DD/MM/YYYY'),'" + sLabourVal[i].Split('~').GetValue(0).ToString() + "',";
                        sQry += " '" + objest.eCapacity + "','" + objest.ewoundType + "', '" + sLabourVal[i].Split('~').GetValue(1).ToString() + "', '" + sLabourVal[i].Split('~').GetValue(2).ToString() + "', ";
                        sQry += " '" + sLabourVal[i].Split('~').GetValue(3).ToString() + "', '" + sLabourVal[i].Split('~').GetValue(4).ToString() + "', ";
                        sQry += " '" + sLabourVal[i].Split('~').GetValue(5).ToString() + "','" + objest.sCrBy + "','" + objest.eRpCenterName + "','" + objest.erateType + "') ";
                        objcon.ExecuteQry(sQry);

                        //long id1 = objcon.Get_max_no("MRI_ID", "TBLMINORREPAIRITEMRATEMASTER");
                        //sQry = "INSERT INTO \"TBLMINORREPAIRITEMRATEMASTER\" (\"MRI_ID\", \"MRI_IED_ID\")";
                        //sQry += " VALUES (" + id1 + ",'" + id + "' )";
                        //objcon.ExecuteQry(sQry);
                    }
                }

                for (int i = 0; i < sSalvageVal.Length; i++)
                {
                    if (sSalvageVal[i] != null)
                    {
                        long id = objcon.Get_max_no("MRI_ID", "TBLMINORREPAIRITEMRATEMASTER");
                        string sQry = string.Empty;
                        sQry = "INSERT INTO \"TBLMINORREPAIRITEMRATEMASTER\" ( \"MRI_ID\", \"MRI_RECORD_ID\", \"MRI_VENDOR_TYPE\", \"MRI_TR_ID\", \"MRI_DIV_ID\", \"MRI_EFFECTIVE_FROM\", \"MRI_EFFECTIVE_TO\", \"MRI_PO_NO\", ";
                        sQry += " \"MRI_PO_DATE\", \"MRI_MRIM_ID\", \"MRI_CAPACITY\",\"MRI_WOUNDTYPE\", \"MRI_QUANTITY\", \"MRI_BASE_RATE\", \"MRI_TAX\", \"MRI_TOTAL\", \"MRI_MEASUREMENT\",\"MRI_CRBY\",\"MRI_REPAIR_CENTER\",\"MRI_RATETYPE\")";
                        sQry += " VALUES ( " + id + "," + UId + " ,'" + objest.eVendortype + "','" + objest.eUser + "','" + objest.eDivision + "',TO_DATE('" + objest.eFromDate + "','DD/MM/YYYY'), ";
                        sQry += " TO_DATE('" + objest.eToDate + "','DD/MM/YYYY'),'" + objest.ePONumber + "',TO_DATE('" + objest.ePODate + "','DD/MM/YYYY'),'" + sSalvageVal[i].Split('~').GetValue(0).ToString() + "',";
                        sQry += " '" + objest.eCapacity + "','" + objest.ewoundType + "', '" + sSalvageVal[i].Split('~').GetValue(1).ToString() + "', '" + sSalvageVal[i].Split('~').GetValue(2).ToString() + "', ";
                        sQry += " '" + sSalvageVal[i].Split('~').GetValue(3).ToString() + "', '" + sSalvageVal[i].Split('~').GetValue(4).ToString() + "', ";
                        sQry += " '" + sSalvageVal[i].Split('~').GetValue(5).ToString() + "','" + objest.sCrBy + "','" + objest.eRpCenterName + "','" + objest.erateType + "') ";
                        objcon.ExecuteQry(sQry);

                        //long id1 = objcon.Get_max_no("MRI_ID", "TBLMINORREPAIRITEMRATEMASTER");
                        //sQry = "INSERT INTO \"TBLMINORREPAIRITEMRATEMASTER\" (\"MRI_ID\", \"MRI_QUANTITY\", \"MRI_BASE_RATE\", \"MRI_TAX\", \"MRI_TOTAL\", \"MRI_MEASUREMENT\", \"MRI_IED_ID\")";
                        //sQry += " VALUES (" + id1 + ",'" + id + "' )";
                        //objcon.ExecuteQry(sQry);
                    }
                }

                Arr[0] = "SuccessFully Saved";
                Arr[1] = "0";
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                Arr[0] = "Failed to Save, Please try Again";
                Arr[1] = "1";
                return Arr;
            }
            return Arr;
        }
        public DataTable LoadAllMaterialDetails()
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT \"MRIM_ID\",\"MRIM_ITEM_NAME\", \"MRIM_ITEM_ID\", '1' AS \"MRI_QUANTITY\", '' AS \"MRI_BASE_RATE\" , '' AS \"MRI_TAX\" , '' AS \"MRIM_ITEM_TYPE\" ,";
                strQry += " '' AS \"MRIM_ITEM_NAME\", '' AS \"MRI_MRIM_ID\", '' AS \"MRI_MEASUREMENT\"";
                strQry += " FROM \"TBLMINORREPAIRERITEMMASTER\" WHERE  \"MRIM_ITEM_TYPE\"=1  ORDER BY \"MRIM_ITEM_ORDER\" ";

                
                dt = objcon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }

        }
        public DataTable LoadAllLabourDetails()
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT \"MRIM_ID\",\"MRIM_ITEM_NAME\" ,\"MRIM_ITEM_ID\", '1' AS \"MRI_QUANTITY\", '' AS \"MRI_BASE_RATE\" , '' AS \"MRI_TAX\" , '' AS \"MRIM_ITEM_TYPE\" ,";
                strQry += " '' AS \"MRIM_ITEM_NAME\", '' AS \"MRI_MRIM_ID\", '' AS \"MRI_MEASUREMENT\"";
                strQry += " FROM \"TBLMINORREPAIRERITEMMASTER\" WHERE  \"MRIM_ITEM_TYPE\"=2  ORDER BY \"MRIM_ITEM_ORDER\" ";


                dt = objcon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }

        }
        public DataTable LoadAllSalvageDetailes()
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT \"MRIM_ID\",\"MRIM_ITEM_NAME\" ,\"MRIM_ITEM_ID\", '1' AS \"MRI_QUANTITY\", '' AS \"MRI_BASE_RATE\" , '' AS \"MRI_TAX\" , '' AS \"MRIM_ITEM_TYPE\",";
                strQry += " '' AS \"MRIM_ITEM_NAME\", '' AS \"MRI_MRIM_ID\", '' AS \"MRI_MEASUREMENT\"";
                strQry += "  FROM \"TBLMINORREPAIRERITEMMASTER\" WHERE  \"MRIM_ITEM_TYPE\"=3  ORDER BY \"MRIM_ITEM_ORDER\" ";


                dt = objcon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        public DataTable LoadMeasurement()
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT 0 \"MD_ID\", '--SELECT--' \"MD_NAME\" UNION ALL SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE  \"MD_TYPE\"='MSR'";
                dt = objcon.FetchDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable LoadExistMaterials(clsEstimate obj)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT \"MRIM_ID\", \"MRIM_ITEM_NAME\",\"MRIM_ITEM_ID\", \"MRI_QUANTITY\", \"MRI_MEASUREMENT\", CAST(\"MRI_BASE_RATE\" AS TEXT) \"MRI_BASE_RATE\", \"MD_NAME\", ";
                strQry += " trunc(\"MRI_TAX\",1) || '%' AS \"MRI_TAX\", \"MRI_TOTAL\" , 0 AS \"ESTM_ITEM_QNTY\" FROM \"TBLMINORREPAIRERITEMMASTER\" , ";
                strQry += " \"TBLMINORREPAIRITEMRATEMASTER\", \"TBLMASTERDATA\" WHERE \"MRIM_ID\" = \"MRI_MRIM_ID\" AND \"MRIM_ITEM_TYPE\" = 1 AND \"MRI_WOUNDTYPE\"=:sWoundType AND \"MRI_RATETYPE\" =:srateType";
                strQry += "  AND CAST(\"MD_ID\" AS TEXT)  = \"MRI_MEASUREMENT\"  AND  \"MD_TYPE\"='MSR'  AND CURRENT_DATE BETWEEN \"MRI_EFFECTIVE_FROM\" AND \"MRI_EFFECTIVE_TO\" ";
                strQry += " AND \"MRI_TR_ID\" = :eUser AND \"MRI_DIV_ID\" = (SELECT \"DIV_ID\" FROM \"TBLDIVISION\" WHERE \"DIV_CODE\"=:sOfficecode) AND \"MRI_CAPACITY\" = (SELECT \"MD_ID\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'C' AND \"MD_NAME\" =:eCapacity)";
                strQry += " ORDER BY \"MRIM_ITEM_ORDER\" ";
                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Parameters.AddWithValue("sWoundType", Convert.ToInt32(obj.sWoundType));
                NpgsqlCommand.Parameters.AddWithValue("srateType", Convert.ToInt32(obj.srateType));
                NpgsqlCommand.Parameters.AddWithValue("eUser", Convert.ToInt32(obj.eUser));
                NpgsqlCommand.Parameters.AddWithValue("sOfficecode", Convert.ToInt32(obj.sOfficecode));
                NpgsqlCommand.Parameters.AddWithValue("eCapacity", obj.eCapacity);
                dt = objcon.FetchDataTable(strQry, NpgsqlCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable LoadExistLabour(clsEstimate obj)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT \"MRIM_ID\", \"MRIM_ITEM_NAME\", \"MRI_QUANTITY\",\"MRIM_ITEM_ID\", \"MRI_MEASUREMENT\", CAST(\"MRI_BASE_RATE\" AS TEXT) \"MRI_BASE_RATE\", \"MD_NAME\", ";
                strQry += " trunc(\"MRI_TAX\",1) || '%' AS \"MRI_TAX\", \"MRI_TOTAL\", 0 AS \"ESTM_ITEM_QNTY\"  FROM \"TBLMINORREPAIRERITEMMASTER\" , ";
                strQry += " \"TBLMINORREPAIRITEMRATEMASTER\", \"TBLMASTERDATA\" WHERE \"MRIM_ID\" = \"MRI_MRIM_ID\" AND \"MRIM_ITEM_TYPE\" = 2 AND \"MRI_WOUNDTYPE\"=:sWoundType AND \"MRI_RATETYPE\" =:srateType";
                strQry += " AND CAST(\"MD_ID\" AS TEXT)  = \"MRI_MEASUREMENT\"  AND  \"MD_TYPE\"='MSR'   AND CURRENT_DATE BETWEEN \"MRI_EFFECTIVE_FROM\" AND \"MRI_EFFECTIVE_TO\" ";
                strQry += " AND \"MRI_TR_ID\" =:eUser AND \"MRI_DIV_ID\" = (SELECT \"DIV_ID\" FROM \"TBLDIVISION\" WHERE \"DIV_CODE\"=:sOfficecode)  AND \"MRI_CAPACITY\" = (SELECT \"MD_ID\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'C' AND \"MD_NAME\" =:eCapacity)";
                strQry += " ORDER BY \"MRIM_ITEM_ORDER\" ";
                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Parameters.AddWithValue("sWoundType", Convert.ToInt32(obj.sWoundType));
                NpgsqlCommand.Parameters.AddWithValue("srateType", Convert.ToInt32(obj.srateType));
                NpgsqlCommand.Parameters.AddWithValue("eUser", Convert.ToInt32(obj.eUser));
                NpgsqlCommand.Parameters.AddWithValue("sOfficecode", Convert.ToInt32(obj.sOfficecode));
                NpgsqlCommand.Parameters.AddWithValue("eCapacity", obj.eCapacity);
                dt = objcon.FetchDataTable(strQry, NpgsqlCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable LoadExistSalvage(clsEstimate obj)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT \"MRIM_ID\", \"MRIM_ITEM_NAME\", \"MRI_QUANTITY\",\"MRIM_ITEM_ID\", \"MRI_MEASUREMENT\", CAST(\"MRI_BASE_RATE\" AS TEXT) \"MRI_BASE_RATE\", \"MD_NAME\", ";
                strQry += " trunc(\"MRI_TAX\",1) || '%' AS \"MRI_TAX\", \"MRI_TOTAL\" , 0 AS \"ESTM_ITEM_QNTY\" FROM \"TBLMINORREPAIRERITEMMASTER\" , ";
                strQry += " \"TBLMINORREPAIRITEMRATEMASTER\", \"TBLMASTERDATA\" WHERE \"MRIM_ID\" = \"MRI_MRIM_ID\" AND \"MRIM_ITEM_TYPE\" = 3 AND \"MRI_WOUNDTYPE\"=:sWoundType AND \"MRI_RATETYPE\" =:srateType";
                strQry += "  AND CAST(\"MD_ID\" AS TEXT)  = \"MRI_MEASUREMENT\"  AND  \"MD_TYPE\"='MSR'   AND CURRENT_DATE BETWEEN \"MRI_EFFECTIVE_FROM\" AND \"MRI_EFFECTIVE_TO\" ";
                strQry += " AND \"MRI_TR_ID\" =:eUser AND \"MRI_DIV_ID\" = (SELECT \"DIV_ID\" FROM \"TBLDIVISION\" WHERE \"DIV_CODE\"=:sOfficecode)  AND \"MRI_CAPACITY\" = (SELECT \"MD_ID\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'C' AND \"MD_NAME\" =:eCapacity)";
                strQry += " ORDER BY \"MRIM_ITEM_ORDER\" ";
                NpgsqlCommand = new NpgsqlCommand();

                NpgsqlCommand.Parameters.AddWithValue("sWoundType", Convert.ToInt32(obj.sWoundType));
                NpgsqlCommand.Parameters.AddWithValue("srateType", Convert.ToInt32(obj.srateType));
                NpgsqlCommand.Parameters.AddWithValue("eUser", Convert.ToInt32(obj.eUser));
                NpgsqlCommand.Parameters.AddWithValue("sOfficecode", Convert.ToInt32(obj.sOfficecode));
                NpgsqlCommand.Parameters.AddWithValue("eCapacity", obj.eCapacity);
                dt = objcon.FetchDataTable(strQry, NpgsqlCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable GetEstimationDetails()
        {
            PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
            string sQry = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                sQry = "SELECT DISTINCT \"DIV_NAME\", CASE WHEN \"MRI_VENDOR_TYPE\" = 1 THEN 'REPAIRER' ELSE 'SUPPLIER' END \"VENDOR_TYPE\", ";
                sQry += " CASE WHEN \"MRI_VENDOR_TYPE\" = 1 THEN (SELECT  \"TR_NAME\" FROM \"TBLTRANSREPAIRER\" WHERE \"TR_ID\" = \"MRI_TR_ID\") ";
                sQry += " ELSE (SELECT  \"TS_NAME\" FROM \"TBLTRANSSUPPLIER\" WHERE \"TS_ID\" = \"MRI_TR_ID\")END \"VENDOR_NAME\", \"MD_NAME\", ";

                sQry += " CASE WHEN \"MRI_WOUNDTYPE\" = '1' THEN 'ALUMINUM' ELSE 'COPPER' END \"WOUND_TYPE\",CASE WHEN \"MRI_RATETYPE\" = '1' THEN 'STAR RATE' ELSE 'CONVENTIONAL' END \"RATE_TYPE\", TO_CHAR(\"MRI_EFFECTIVE_FROM\",'DD-MON-YYYY') ";
                sQry += " \"FROM\", TO_CHAR(\"MRI_EFFECTIVE_TO\",'DD-MON-YYYY') \"TO\", \"MRI_TR_ID\", \"MRI_CAPACITY\", \"MRI_RECORD_ID\" FROM \"TBLMINORREPAIRITEMRATEMASTER\", \"TBLDIVISION\", ";
                sQry += " \"TBLMASTERDATA\" WHERE \"MRI_DIV_ID\" = \"DIV_ID\" AND \"MD_TYPE\" = 'C' AND \"MD_ID\" = \"MRI_CAPACITY\" AND \"MRI_EFFECTIVE_TO\" >= CURRENT_DATE ";
                dt = objcon.FetchDataTable(sQry);
                return dt;
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public object GetExistingEstimationDetails(clsEstimate obj)
        {
            PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
            string sQry = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                sQry = "SELECT DISTINCT \"MRI_DIV_ID\", \"MRI_TR_ID\", \"MRI_VENDOR_TYPE\", \"MRI_CAPACITY\", \"MRI_PO_NO\", TO_CHAR(\"MRI_PO_DATE\",'DD/MM/YYYY') \"MRI_PO_DATE\", ";
                sQry += " TO_CHAR(\"MRI_EFFECTIVE_FROM\",'DD/MM/YYYY') \"MRI_EFFECTIVE_FROM\", TO_CHAR(\"MRI_EFFECTIVE_TO\",'DD/MM/YYYY') \"MRI_EFFECTIVE_TO\",\"MRI_WOUNDTYPE\",\"MRI_RATETYPE\" FROM \"TBLMINORREPAIRITEMRATEMASTER\" WHERE \"MRI_RECORD_ID\" =:sRecordId ";
                NpgsqlCommand = new NpgsqlCommand();

                NpgsqlCommand.Parameters.AddWithValue("sRecordId",Convert.ToInt32(obj.sRecordId));
                dt = objcon.FetchDataTable(sQry, NpgsqlCommand);
                if(dt.Rows.Count > 0)
                {
                    obj.eDivision = Convert.ToString(dt.Rows[0]["MRI_DIV_ID"]);
                    obj.eUser = Convert.ToString(dt.Rows[0]["MRI_TR_ID"]);
                    obj.eVendortype = Convert.ToString(dt.Rows[0]["MRI_VENDOR_TYPE"]);
                    obj.eCapacity = Convert.ToString(dt.Rows[0]["MRI_CAPACITY"]);
                    obj.ePONumber = Convert.ToString(dt.Rows[0]["MRI_PO_NO"]);
                    obj.ePODate = Convert.ToString(dt.Rows[0]["MRI_PO_DATE"]);
                    obj.eFromDate = Convert.ToString(dt.Rows[0]["MRI_EFFECTIVE_FROM"]);
                    obj.eToDate = Convert.ToString(dt.Rows[0]["MRI_EFFECTIVE_TO"]);
                    obj.ewoundType = Convert.ToString(dt.Rows[0]["MRI_WOUNDTYPE"]);
                    //if (dt.Columns.Contains("MRI_RATETYPE"))
                    //{
                    //}
                    if (dt.Rows[0]["MRI_RATETYPE"] != "" || dt.Rows[0]["MRI_RATETYPE"] != null)
                    {
                        obj.erateType = Convert.ToString(dt.Rows[0]["MRI_RATETYPE"]);
                    }
                    else
                    {
                        obj.erateType = "2";
                    }


                }
                return obj;
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return obj;
            }
        }

        public object GetExistingMaterial(clsEstimate obj)
        {
            try
            {
                string sQry = string.Empty;
                sQry = "SELECT \"MRIM_ID\", \"MRIM_ITEM_NAME\",\"MRIM_ITEM_ID\", \"MRI_MRIM_ID\", \"MRI_MEASUREMENT\", \"MRI_QUANTITY\", \"MRI_BASE_RATE\", \"MRI_TAX\", \"MRIM_ITEM_TYPE\" ";
                sQry += " FROM \"TBLMINORREPAIRITEMRATEMASTER\", \"TBLMINORREPAIRERITEMMASTER\" WHERE  \"MRI_MRIM_ID\" = \"MRIM_ID\" AND  \"MRI_RECORD_ID\" =:sRecordId";
                sQry += " ORDER BY \"MRIM_ITEM_ORDER\" ";
                NpgsqlCommand = new NpgsqlCommand();

                NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(obj.sRecordId));
                obj.dtMaterials = objcon.FetchDataTable(sQry, NpgsqlCommand);
                return obj;
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return obj;
            }
        }

        public void UpdateEffectiveDate(clsEstimate obj)
        {
            try
            {
                string sQry = string.Empty;
                if(obj.sRecordId.Length > 0)
                {
                    sQry = "UPDATE \"TBLMINORREPAIRITEMRATEMASTER\" SET \"MRI_EFFECTIVE_TO\" = CURRENT_DATE - 1 WHERE  \"MRI_RECORD_ID\" =:sRecordId ";
                    NpgsqlCommand = new NpgsqlCommand();

                    NpgsqlCommand.Parameters.AddWithValue("sRecordId", Convert.ToInt32(obj.sRecordId));
                    objcon.ExecuteQry(sQry, NpgsqlCommand);
                }               
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public DataTable LoadEstimationDetails(string sFailuretype,string officecode)
        {
            PGSqlConnection objCon = new PGSqlConnection(Constants.Password);
            DataTable dt = new DataTable();
            try
            {
                string sQry = string.Empty;
                sQry = "SELECT b.\"EST_ID\" \"EST_ID\", \"EST_FAILUREID\", \"DF_DTC_CODE\", cast(\"DF_EQUIPMENT_ID\" as text), cast(b.\"EST_NO\" as text) \"EST_NO\", TO_CHAR(b.\"EST_CRON\",'DD-MON-YYYY') \"EST_CRON\", ";
                sQry += " (CASE WHEN \"EST_WOUNDTYPE\" = 1 THEN 'ALUMINIUM' ELSE 'COPPER' END) \"WOUND_TYPE\",(CASE WHEN \"EST_RATETYPE\" = 1 THEN 'STAR RATE' ELSE 'CONVENTIONAL' END) \"RATE_TYPE\",cast(\"EST_REPLACE_CAPACITY\" as text)  as \"EST_CAPACITY\", ";
                sQry += " (CASE WHEN \"EST_FAIL_TYPE\" = 1  THEN 'MINOR' ELSE 'MAJOR' END) \"FAILURETYPE\" FROM \"TBLESTIMATIONDETAILS\"  b,\"TBLESTIMATION\", ";
                sQry += "  \"TBLDTCFAILURE\" WHERE \"EST_FAILUREID\" = \"DF_ID\"   and \"EST_DF_ID\" = \"DF_ID\"and cast(\"DF_LOC_CODE\" as text) like '" + officecode + "%' ";
                if(sFailuretype.Length > 0)
                {
                    sQry += " AND \"EST_FAIL_TYPE\" = '"+ sFailuretype + "' ";
                }

                dt = objCon.FetchDataTable(sQry);
                return dt;
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
    }
}
