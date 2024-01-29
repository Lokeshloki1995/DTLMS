using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IIITS.PGSQL.DAL;
using System.Data;
using System.Configuration;
using Npgsql;
using IIITS.DTLMS.BL.DataBase;

namespace IIITS.DTLMS.BL
{
    public static class clsStoreOffice
    {
        public static string strFormCode = "clsStoreOffice";

        #region unused methods
        //public static string GetOfficeCodenot(string sStoreID, string sColumn)
        //{
        //    PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
        //    try
        //    {
        //        string sResult = string.Empty;
        //        string sQry = string.Empty;
        //        sQry = "SELECT string_agg(CAST(\"STO_OFF_CODE\" AS TEXT), ',') AS \"OFFCODE\" FROM \"TBLSTOREMAST\", \"TBLSTOREOFFCODE\" ";
        //        sQry += " WHERE \"SM_ID\" = \"STO_SM_ID\" AND \"SM_ID\" = '" + sStoreID + "'";
        //        string sStoreOffCode = objcon.get_value(sQry);

        //        if (sStoreOffCode.Length > 1)
        //        {
        //            string[] sArr = new string[4];
        //            sArr = sStoreOffCode.Split(',');

        //            sResult = "( ";
        //            for (int i = 0; i < sArr.Length; i++)
        //            {
        //                sResult += " CAST(\"" + sColumn + "\" AS TEXT) <> '" + sArr[i] + "' ";
        //                if (i < sArr.Length - 1)
        //                {
        //                    sResult += " AND ";
        //                }
        //            }
        //            sResult += ") ";
        //        }

        //        return sResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return "0";
        //    }
        //}

        #endregion


        public static string GetStoreCode(string sStoreID, string sColumn)
        {
            PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
            try
            {
                string sResult = string.Empty;
                string sQry = string.Empty;
                #region inline query
                //sQry = "SELECT string_agg(CAST(\"SM_ID\" AS TEXT), ',') AS \"OFFCODE\" FROM \"TBLSTOREMAST\", \"TBLSTOREOFFCODE\" ";
                //sQry += " WHERE \"SM_ID\" = \"STO_SM_ID\" AND cast(\"SM_ID\" as text) = '" + sStoreID + "'";
                //string sStoreOffCode = objcon.get_value(sQry);
                #endregion

                DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsstoreoffice");
                cmd.Parameters.AddWithValue("p_key", "GETSTORECODE");
                cmd.Parameters.AddWithValue("p_value", sStoreID);
                cmd.Parameters.AddWithValue("p_offcode", "");
                string sStoreOffCode = objDatabse.StringGetValue(cmd);

                if (sStoreOffCode.Length > 1)
                {
                    string[] sArr = new string[4];
                    sArr = sStoreOffCode.Split(',');

                    sResult = "( ";
                    for (int i = 0; i < sArr.Length; i++)
                    {
                        sResult += " CAST(\"" + sColumn + "\" AS TEXT) = '" + sArr[i] + "' ";
                        if (i < sArr.Length - 1)
                        {
                            sResult += " OR ";
                        }
                    }
                    sResult += ") ";
                }

                return sResult;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "0";
            }
        }
        public static string GetOfficeCode(string sStoreID, string sColumn)
        {
            PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
            try
            {
                string sResult = string.Empty;
                string sQry = string.Empty;

                //sQry = "SELECT string_agg(CAST(\"STO_OFF_CODE\" AS TEXT), ',') AS \"OFFCODE\" FROM \"TBLSTOREMAST\", \"TBLSTOREOFFCODE\" ";
                //sQry += " WHERE \"SM_ID\" = \"STO_SM_ID\" AND cast(\"SM_ID\" as text) = '" + sStoreID + "'";
                //string sStoreOffCode = objcon.get_value(sQry);

                DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsstoreoffice");
                cmd.Parameters.AddWithValue("p_key", "GETOFFICECODE");
                cmd.Parameters.AddWithValue("p_value", sStoreID);
                cmd.Parameters.AddWithValue("p_offcode", "");
                string sStoreOffCode = objDatabse.StringGetValue(cmd);

                if (sStoreOffCode.Length > 1)
                {
                    string[] sArr = new string[4];
                    sArr = sStoreOffCode.Split(',');

                    sResult = "( ";
                    for (int i = 0; i < sArr.Length; i++)
                    {
                        sResult += " CAST(\""+sColumn+"\" AS TEXT) LIKE '" + sArr[i] + "%' ";
                        if(i < sArr.Length-1)
                        {
                            sResult += " OR ";
                        }
                    }
                    sResult += ") ";
                }

                return sResult;
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "0";
            }
        }

  
        public static string GetStoreID(string sOfficeCode)
        {
            PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
            string sStoreId = string.Empty;
            try
            {
                if(sOfficeCode.Length > 3)
                {
                    sOfficeCode = sOfficeCode.Substring(0, Constants.Division);
                }
              //  string sQry = string.Empty;
                if (sOfficeCode == "")
                {
                   // sQry = "";
                    sStoreId = "";
                }
                else
                {
                  //  sQry = "SELECT \"STO_SM_ID\" FROM \"TBLSTOREOFFCODE\" WHERE \"STO_OFF_CODE\" = '" + sOfficeCode + "' ";

                    DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsstoreoffice");
                    cmd.Parameters.AddWithValue("p_key", "GETSTOREID");
                    cmd.Parameters.AddWithValue("p_value", sOfficeCode);
                    cmd.Parameters.AddWithValue("p_offcode", "");
                     sStoreId = objDatabse.StringGetValue(cmd);
                }
               // sStoreId = objcon.get_value(sQry);
                return sStoreId;
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return sStoreId;
            }
        }


        public static string GetStoreIDs(string sOfficeCode)
        {
            PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
            string sStoreId = string.Empty;
            try
            {
                if (sOfficeCode.Length > 3)
                {
                    sOfficeCode = sOfficeCode.Substring(0, Constants.Division);
                }
               // string sQry = string.Empty;
                if (sOfficeCode == "")
                {
                    //  sQry = "";
                    sStoreId = "";
                }
                else
                {
                  //  sQry = "SELECT \"STO_SM_ID\" FROM \"TBLSTOREOFFCODE\" WHERE cast(\"STO_OFF_CODE\" as text) like '" + sOfficeCode + "%' limit 1";

                    DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsstoreoffice");
                    cmd.Parameters.AddWithValue("p_key", "GETSTOREIDS");
                    cmd.Parameters.AddWithValue("p_value", sOfficeCode);
                    cmd.Parameters.AddWithValue("p_offcode", "");
                    sStoreId = objDatabse.StringGetValue(cmd);
                }
               // sStoreId = objcon.get_value(sQry);
                return sStoreId;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return sStoreId;
            }
        }

        public static string Getofficecode(string sOfficeCode)
        {
            PGSqlConnection objcon = new PGSqlConnection(Constants.Password);  
            string sStoreId = string.Empty;
            try
            {
               // string sQry = string.Empty;
                if (sOfficeCode!="")
                {
                    //sQry = "SELECT string_agg(CAST(\"STO_OFF_CODE\" AS TEXT), ',') AS \"STO_OFF_CODE\" FROM \"TBLSTOREOFFCODE\" WHERE \"STO_SM_ID\" = cast('" + sOfficeCode + "' as int4) ";
                    //sStoreId = objcon.get_value(sQry);

                    DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsstoreoffice");
                    cmd.Parameters.AddWithValue("p_key", "GETSTOREOFFICECODE");
                    cmd.Parameters.AddWithValue("p_value", sOfficeCode);
                    cmd.Parameters.AddWithValue("p_offcode", "");
                    sStoreId = objDatabse.StringGetValue(cmd);
                }
                return sStoreId;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return sStoreId;
            }
        }
        


        public static string GetCurrentOfficeCode(string sWoID, string sType)
        {
            PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
            try
            {
                string sOfficeCode = string.Empty;
                if(sType == "1")
                {
                  //  sQry = "SELECT \"WO_OFFICE_CODE\" FROM \"TBLWORKFLOWOBJECTS\" WHERE cast(\"WO_ID\" as text) = '" + sWoID + "'";
                    DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsstoreoffice");
                    cmd.Parameters.AddWithValue("p_key", "GETCURRECNTOFFICECODEWO");
                    cmd.Parameters.AddWithValue("p_value", sWoID);
                    cmd.Parameters.AddWithValue("p_offcode", "");
                    sOfficeCode = objDatabse.StringGetValue(cmd);
                }
                else
                {
                  //  sQry = "SELECT \"WOA_OFFICE_CODE\" FROM \"TBLWO_OBJECT_AUTO\" WHERE cast(\"WOA_ID\" as text) = '" + sWoID + "'";

                    DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsstoreoffice");
                    cmd.Parameters.AddWithValue("p_key", "GETCURRECNTOFFICECODEWOA");
                    cmd.Parameters.AddWithValue("p_value", sWoID);
                    cmd.Parameters.AddWithValue("p_offcode", "");
                    sOfficeCode = objDatabse.StringGetValue(cmd);
                }
                //string sOfficeCode = objcon.get_value(sQry);
                return sOfficeCode;
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "0";
            }
        }

        public static string GetRICurrentOfficeCode(string sWoID, string sType)
        {
            PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
            try
            {
                //string sQry = string.Empty;
                //sQry = "SELECT \"WO_OFFICE_CODE\" FROM \"TBLWORKFLOWOBJECTS\" WHERE cast(\"WO_ID\" as text)= '" + sWoID + "'";                
                //string sOfficeCode = objcon.get_value(sQry);

                DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsstoreoffice");
                cmd.Parameters.AddWithValue("p_key", "GETRICURRENTOFFICECODE");
                cmd.Parameters.AddWithValue("p_value", sWoID);
                cmd.Parameters.AddWithValue("p_offcode", "");
               string sOfficeCode = objDatabse.StringGetValue(cmd);

                if (sOfficeCode.Length > 3)
                {
                    sOfficeCode = sOfficeCode.Substring(0, 3);
                }
                return sOfficeCode;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "0";
            }
        }

        public static string GetZone_Circle_Div_Offcode(string sOfficeCode, string sRoleID)
        {
            PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
            string offCode = string.Empty;
            try
            {
                if (sRoleID.Equals("2") || sRoleID.Equals("5"))
                {
                    //string sQry = string.Empty;
                    //sQry = "SELECT \"STO_OFF_CODE\" FROM \"TBLSTOREOFFCODE\" WHERE \"STO_SM_ID\" = '" + sOfficeCode + "' limit 1 ";
                    //offCode = objcon.get_value(sQry);

                    DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
                    NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsstoreoffice");
                    cmd.Parameters.AddWithValue("p_key", "GETZONECIRCLEDIVOFFCODE");
                    cmd.Parameters.AddWithValue("p_value", sOfficeCode);
                    cmd.Parameters.AddWithValue("p_offcode", "");
                    offCode = objDatabse.StringGetValue(cmd);

                    return offCode;
                }
                else
                {
                    if (sRoleID.Equals(Convert.ToString(ConfigurationManager.AppSettings["SupAdminRole"])))
                    {
                        //string sQry = string.Empty;
                        //sQry = "SELECT \"STO_OFF_CODE\" FROM \"TBLSTOREOFFCODE\" WHERE \"STO_SM_ID\" = '" + sOfficeCode + "' limit 1 ";
                        //offCode = objcon.get_value(sQry);

                        DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
                        NpgsqlCommand cmd = new NpgsqlCommand("fetch_getvalue_clsstoreoffice");
                        cmd.Parameters.AddWithValue("p_key", "GETZONECIRCLEDIVOFFCODE");
                        cmd.Parameters.AddWithValue("p_value", sOfficeCode);
                        cmd.Parameters.AddWithValue("p_offcode", "");
                        offCode = objDatabse.StringGetValue(cmd);
                        return offCode;
                    }
                    else
                    {
                        return sOfficeCode;
                    }
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return offCode;
            }
        }
    }
}
