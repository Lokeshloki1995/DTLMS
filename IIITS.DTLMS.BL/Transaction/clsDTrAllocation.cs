using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;

namespace IIITS.DTLMS.BL
{
    public class clsDTrAllocation
    {
        PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
        string strFormCode = "clsDTrAllocation";

        public string sCrby { get; set; }
        public string sDTrCode { get; set; }
        public string sDTCCode { get; set; }
        public string sOfficeCode { get; set; }
        public string sStoreId { get; set; }
        public string sUserName { get; set; }
        public string sStoreName { get; set; }
        public string sNewDTCCode { get; set; }

        public string sFirstDTrCode { get; set; }
        public string sSecondDTrCode { get; set; }
        public string sFirstDTCCode { get; set; }
        public string sSecondDTCCode { get; set; }
      
       
        public string sCapacity { get; set; }
        public string sMakeName { get; set; }
        public string sTcSlNo { get; set; }

        public class example : clsDTrAllocation
        {
            public string sTrail { get; set; }           

        }

        NpgsqlCommand NpgsqlCommand;
        #region unused methods
        //public object GetDTrDetails(clsDTrAllocation objAllocation)
        //{
        //    NpgsqlCommand = new NpgsqlCommand();
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        string strQry = string.Empty;
        //        NpgsqlCommand.Parameters.AddWithValue("FirstDTrCode", objAllocation.sFirstDTrCode);
        //        strQry = " SELECT \"TC_CODE\",\"TC_SLNO\",\"TM_NAME\",CAST(\"TC_CAPACITY\" AS TEXT) TC_CAPACITY,";
        //        strQry += " (SELECT \"DT_CODE\" FROM \"TBLDTCMAST\" WHERE \"DT_TC_ID\"=\"TC_CODE\") DTC_CODE";
        //        strQry += " FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\",\"VIEW_ALL_OFFICES\"  WHERE  \"TC_LOCATION_ID\"=\"OFF_CODE\" ";
        //        strQry += "  AND \"TC_MAKE_ID\"= \"TM_ID\" AND \"TC_CODE\" =:FirstDTrCode";

        //        dt = objcon.FetchDataTable(strQry, NpgsqlCommand);

        //        if (dt.Rows.Count > 0)
        //        {
        //            objAllocation.sFirstDTCCode = dt.Rows[0]["DTC_CODE"].ToString();
        //            objAllocation.sFirstDTrCode = dt.Rows[0]["TC_CODE"].ToString();
        //            objAllocation.sTcSlNo = dt.Rows[0]["TC_SLNO"].ToString();
        //            objAllocation.sMakeName = dt.Rows[0]["TM_NAME"].ToString();
        //            objAllocation.sCapacity = dt.Rows[0]["TC_CAPACITY"].ToString();

        //        }
        //        return objAllocation;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return dt;
        //    }
        //}
        #endregion

        public DataTable LoadDTrDetails(string sDTrCode)
        {
            NpgsqlCommand = new NpgsqlCommand();
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                example objExam = new example();
                NpgsqlCommand.Parameters.AddWithValue("DTrCode", Convert.ToDouble(sDTrCode));
                strQry = "SELECT \"TC_CODE\",\"TC_SLNO\",\"TM_NAME\",CAST(\"TC_CAPACITY\" AS TEXT) TC_CAPACITY,CASE \"TC_CURRENT_LOCATION\" WHEN 1 THEN 'STORE' WHEN 2 THEN 'FIELD' WHEN 2 THEN 'REPAIRER'";
                strQry += "  END || ' ; ' || OFF_NAME AS CURRENT_LOCATION,";
                strQry += " (SELECT \"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_TC_ID\"=\"TC_CODE\") DTC_NAME,(SELECT \"DT_CODE\" FROM \"TBLDTCMAST\" WHERE \"DT_TC_ID\"=\"TC_CODE\") DTC_CODE";
                strQry += " FROM \"TBLTCMASTER\",\"TBLTRANSMAKES\",\"VIEW_ALL_OFFICES\"  WHERE  \"TC_LOCATION_ID\"=\"OFF_CODE\" ";
                strQry += " AND \"TC_MAKE_ID\"= \"TM_ID\" AND \"TC_CODE\" =:DTrCode";
                dt = objcon.FetchDataTable(strQry, NpgsqlCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public string[] DTrStoreAllocation(clsDTrAllocation objAllocation)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string[] Arr = new string[2];
            try
            {
                string strQry = string.Empty;
                string sDesc = string.Empty;
                string sDTrId = string.Empty;
                NpgsqlCommand.Parameters.AddWithValue("DTrCode",Convert.ToDouble( objAllocation.sDTrCode));
                sDTrId = objcon.get_value("SELECT \"TC_ID\" FROM \"TBLTCMASTER\" WHERE \"TC_CODE\" =:DTrCode", NpgsqlCommand);

                objcon.BeginTransaction();

                if (objAllocation.sDTCCode != "")
                {
                    NpgsqlCommand.Parameters.AddWithValue("Crby", Convert.ToInt32(objAllocation.sCrby));
                    NpgsqlCommand.Parameters.AddWithValue("DTrCode1", Convert.ToDouble(objAllocation.sDTrCode));
                    NpgsqlCommand.Parameters.AddWithValue("DTCCode", objAllocation.sDTCCode);
                    strQry = "UPDATE \"TBLTRANSDTCMAPPING\" SET \"TM_LIVE_FLAG\" = '0', \"TM_UNMAP_CRON\" = NOW(),\"TM_UNMAP_CRBY\"=:Crby,";
                    strQry += " \"TM_UNMAP_REASON\" = 'UNMAP BY DTR Allocation' WHERE \"TM_TC_ID\" =:DTrCode1";
                    strQry += " AND \"TM_LIVE_FLAG\" ='1' AND \"TM_DTC_ID\"  =:DTCCode";
                    objcon.ExecuteQry(strQry, NpgsqlCommand);

                    NpgsqlCommand.Parameters.AddWithValue("DTCCode1", objAllocation.sDTCCode);
                    strQry = "UPDATE \"TBLDTCMAST\" SET \"DT_TC_ID\" ='0' WHERE \"DT_CODE\" =:DTCCode1";
                    objcon.ExecuteQry(strQry, NpgsqlCommand);


                    sDesc = "DTR DEALLOCATED FROM DTC CODE " + objAllocation.sDTCCode + " BY " + objAllocation.sUserName + " AND MOVED TO STORE " + objAllocation.sStoreName;
                }
                else
                {
                    sDesc = "DTR MOVED TO STORE " + objAllocation.sStoreName + " BY " + objAllocation.sUserName;
                }

                NpgsqlCommand.Parameters.AddWithValue("GetStoreID", clsStoreOffice.GetStoreID(objAllocation.sOfficeCode));
                NpgsqlCommand.Parameters.AddWithValue("DTrCode2", Convert.ToDouble(objAllocation.sDTrCode));
                strQry = "UPDATE \"TBLTCMASTER\" SET \"TC_UPDATED_EVENT\" ='FROM DTR ALLOCATION',";
                strQry += " \"TC_CURRENT_LOCATION\" =1,\"TC_LOCATION_ID\" =:GetStoreID WHERE ";
                strQry += " \"TC_CODE\" =:DTrCode2";
                objcon.ExecuteQry(strQry, NpgsqlCommand);

                long sMaxNo = objcon.Get_max_no("\"DRT_ID\"", "\"TBLDTRTRANSACTION\"");

                NpgsqlCommand.Parameters.AddWithValue("MaxNo", sMaxNo);
                NpgsqlCommand.Parameters.AddWithValue("DTrCode3", Convert.ToDouble(objAllocation.sDTrCode));
                NpgsqlCommand.Parameters.AddWithValue("StoreId", Convert.ToInt16(objAllocation.sStoreId));
                NpgsqlCommand.Parameters.AddWithValue("DTrId", sDTrId);
                NpgsqlCommand.Parameters.AddWithValue("Desc", sDesc);
                strQry = "INSERT INTO \"TBLDTRTRANSACTION\" (\"DRT_ID\",\"DRT_DTR_CODE\",\"DRT_LOC_ID\",\"DRT_LOC_TYPE\",\"DRT_TRANS_DATE\", ";
                strQry += " \"DRT_ACT_REFNO\",\"DRT_ACT_REFTYPE\",\"DRT_DESC\",\"DRT_DTR_STATUS\") VALUES (:MaxNo,:DTrCode3,";
                strQry += " :StoreId,'1',NOW(),:DTrId,'9',:Desc,'1')";
                objcon.ExecuteQry(strQry, NpgsqlCommand);

                objcon.CommitTransaction();

                Arr[0] = "Allocated Successfully";
                Arr[1] = "0";
                return Arr;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }


        public string[] DTrFieldAllocation(clsDTrAllocation objAllocation)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string[] Arr = new string[2];
            try
            {
                string strQry = string.Empty;
                string sDesc = string.Empty;
                string sDTCOffCode = string.Empty;
                string sResult = string.Empty;
                NpgsqlCommand.Parameters.AddWithValue("NewDTCCode", objAllocation.sNewDTCCode.ToUpper());
                sDTCOffCode = objcon.get_value("SELECT \"DT_OM_SLNO\" FROM \"TBLDTCMAST\" WHERE UPPER(\"DT_CODE\")=:NewDTCCode", NpgsqlCommand);

                if (sDTCOffCode == "")
                {
                    Arr[0] = "Please enter Valid DTC Code";
                    Arr[1] = "2";
                    return Arr;
                }

                NpgsqlCommand.Parameters.AddWithValue("NewDTCCode1", objAllocation.sNewDTCCode.ToUpper());
                strQry = "SELECT \"TM_ID\" FROM \"TBLTRANSDTCMAPPING\" WHERE UPPER(\"TM_DTC_ID\")=:NewDTCCode1 AND \"TM_LIVE_FLAG\" ='1'";
                sResult = objcon.get_value(strQry, NpgsqlCommand);
                if (sResult != "")
                {
                    Arr[0] = "Already DTR Exists for Selected DTC Code " + objAllocation.sNewDTCCode;
                    Arr[1] = "2";
                    return Arr;

                }

                objcon.BeginTransaction();

                NpgsqlCommand.Parameters.AddWithValue("DTCOffCode",Convert.ToDouble(sDTCOffCode));
                NpgsqlCommand.Parameters.AddWithValue("DTrCode", Convert.ToDouble(objAllocation.sDTrCode));
                strQry = "UPDATE \"TBLTCMASTER\" SET \"TC_UPDATED_EVENT\" ='FROM DTR ALLOCATION',";
                strQry += " \"TC_CURRENT_LOCATION\"=2,\"TC_LOCATION_ID\" =:DTCOffCode WHERE ";
                strQry += " \"TC_CODE\" =:DTrCode";
                objcon.ExecuteQry(strQry, NpgsqlCommand);

                         
                //Map the New TC for DTC
                NpgsqlCommand.Parameters.AddWithValue("MaxNo", objcon.Get_max_no("\"TM_ID\"", "\"TBLTRANSDTCMAPPING\""));
                NpgsqlCommand.Parameters.AddWithValue("DTrCode1", objAllocation.sDTrCode);
                NpgsqlCommand.Parameters.AddWithValue("NewDTCCode2", objAllocation.sNewDTCCode);
                NpgsqlCommand.Parameters.AddWithValue("Crby", objAllocation.sCrby);
                strQry = "INSERT INTO \"TBLTRANSDTCMAPPING\" (\"TM_ID\",\"TM_MAPPING_DATE\",\"TM_TC_ID\",\"TM_DTC_ID\",\"TM_LIVE_FLAG\",\"TM_CRBY\",\"TM_CRON\")";
                strQry += " VALUES(:MaxNo, NOW(),:DTrCode1,";
                strQry += " :NewDTCCode2,'1',:Crby,NOW())";
                objcon.ExecuteQry(strQry, NpgsqlCommand);


                // Update in DTC Table
                NpgsqlCommand.Parameters.AddWithValue("DTrCode2", Convert.ToDouble(objAllocation.sDTrCode));
                NpgsqlCommand.Parameters.AddWithValue("NewDTCCode3", objAllocation.sNewDTCCode);
                strQry = "UPDATE \"TBLDTCMAST\" SET \"DT_TC_ID\" =:DTrCode2 WHERE \"DT_CODE\" =:NewDTCCode3";
                objcon.ExecuteQry(strQry, NpgsqlCommand);


                if (objAllocation.sDTCCode != "")
                {
                    NpgsqlCommand.Parameters.AddWithValue("DTCCode", objAllocation.sDTCCode);
                    strQry = "UPDATE \"TBLDTCMAST\" SET \"DT_TC_ID\" ='0' WHERE \"DT_CODE\" =:DTCCode";
                    objcon.ExecuteQry(strQry, NpgsqlCommand);
                }


                objcon.CommitTransaction();

                Arr[0] = "Allocated Successfully";
                Arr[1] = "0";
                return Arr;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;

            }
        }


     

        public string[] DTrAllocation(clsDTrAllocation objAllocation)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string[] Arr = new string[2];
            try
            {
                DataTable dt = new DataTable();
                string strQry = string.Empty;

                objcon.BeginTransaction();

                // Updating or Interchanging the TC(DT_TC_ID) with DTC(DT_CODE)

                NpgsqlCommand.Parameters.AddWithValue("SecondDTrCode", Convert.ToDouble(objAllocation.sSecondDTrCode));
                NpgsqlCommand.Parameters.AddWithValue("FirstDTrCode", objAllocation.sFirstDTCCode);
                strQry = "UPDATE \"TBLDTCMAST\" SET \"DT_TC_ID\" =:SecondDTrCode WHERE \"DT_CODE\" =:FirstDTrCode";
                objcon.ExecuteQry(strQry, NpgsqlCommand);

                NpgsqlCommand.Parameters.AddWithValue("FirstDTrCode1", Convert.ToDouble(objAllocation.sFirstDTrCode));
                NpgsqlCommand.Parameters.AddWithValue("SecondDTCCode1", objAllocation.sSecondDTCCode);
                strQry = "UPDATE \"TBLDTCMAST\" SET \"DT_TC_ID\" =:FirstDTrCode1 WHERE \"DT_CODE\" =:SecondDTCCode1";
                objcon.ExecuteQry(strQry, NpgsqlCommand);

                // Updating LiveFlag to '0' to the table TBLTRANSDTCMAPPING

                NpgsqlCommand.Parameters.AddWithValue("FirstDTrCode2", Convert.ToDouble(objAllocation.sFirstDTrCode));
                NpgsqlCommand.Parameters.AddWithValue("FirstDTCCode3", objAllocation.sFirstDTCCode);
                strQry = "UPDATE \"TBLTRANSDTCMAPPING\" SET \"TM_LIVE_FLAG\" ='0' WHERE \"TM_TC_ID\" =:FirstDTrCode2 AND";
                strQry += " \"TM_DTC_ID\" =:FirstDTCCode3 AND \"TM_LIVE_FLAG\" ='1'";
                objcon.ExecuteQry(strQry, NpgsqlCommand);

                NpgsqlCommand.Parameters.AddWithValue("SecondDTrCode2", Convert.ToDouble(objAllocation.sSecondDTrCode));
                NpgsqlCommand.Parameters.AddWithValue("SecondDTrCode3", objAllocation.sSecondDTCCode);
                strQry = "UPDATE \"TBLTRANSDTCMAPPING\" SET \"TM_LIVE_FLAG\" ='0' WHERE \"TM_TC_ID\" =:SecondDTrCode2 AND";
                strQry += " \"TM_DTC_ID\" =:SecondDTrCode3 AND \"TM_LIVE_FLAG\" ='1'";
                objcon.ExecuteQry(strQry, NpgsqlCommand);

                /// Insert to DTR Transaction Table

                string sDTrId = string.Empty;
                string sDesc = string.Empty;
                string sDTCLocCode = string.Empty;


                ///First DTR Code
                NpgsqlCommand.Parameters.AddWithValue("FirstDTrCode4", Convert.ToDouble(objAllocation.sFirstDTrCode));
               
                
                sDTrId = objcon.get_value("SELECT \"TC_ID\" FROM \"TBLTCMASTER\" WHERE \"TC_CODE\" =:FirstDTrCode4", NpgsqlCommand);
               
                sDesc = "DTR REALLOCATED FROM DTC CODE " + objAllocation.sFirstDTCCode  + " TO " + objAllocation.sSecondDTCCode + " BY " + objAllocation.sUserName + "";

                NpgsqlCommand.Parameters.AddWithValue("FirstDTCCode5", sFirstDTCCode);
                sDTCLocCode = objcon.get_value("SELECT \"DT_OM_SLNO\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\" = :FirstDTCCode5", NpgsqlCommand);

                long sMaxNo = Convert.ToInt64 (objcon.get_value("SELECT COALESCE(MAX(\"DRT_ID\"),0)+1 FROM \"TBLDTRTRANSACTION\""));
                //long sMaxNo = objcon.Get_max_no("\"DRT_ID\"", "\"TBLDTRTRANSACTION\"");

                NpgsqlCommand.Parameters.AddWithValue("MaxNo", sMaxNo);
                NpgsqlCommand.Parameters.AddWithValue("FirstDTrCode6", Convert.ToDouble(objAllocation.sFirstDTrCode));
                NpgsqlCommand.Parameters.AddWithValue("DTCLocCode",Convert.ToInt16( sDTCLocCode));
                NpgsqlCommand.Parameters.AddWithValue("DTrId", Convert.ToInt16(sDTrId));
                NpgsqlCommand.Parameters.AddWithValue("Desc", sDesc);

                strQry = "INSERT INTO \"TBLDTRTRANSACTION\" (\"DRT_ID\",\"DRT_DTR_CODE\",\"DRT_LOC_ID\",\"DRT_LOC_TYPE\",\"DRT_TRANS_DATE\", ";
                strQry += " \"DRT_ACT_REFNO\",\"DRT_ACT_REFTYPE\",\"DRT_DESC\",\"DRT_DTR_STATUS\" ) VALUES (:MaxNo,:FirstDTrCode6,";
                strQry += " :DTCLocCode,'2',NOW(),:DTrId,'9',:Desc,'1')";
                objcon.ExecuteQry(strQry, NpgsqlCommand);


                ///Second DTR Code
                NpgsqlCommand.Parameters.AddWithValue("SecondDTrCode4", Convert.ToDouble(objAllocation.sSecondDTrCode));
                sDTrId = objcon.get_value("SELECT \"TC_ID\" FROM \"TBLTCMASTER\" WHERE \"TC_CODE\" =:SecondDTrCode4", NpgsqlCommand);
                
                sDesc = "DTR REALLOCATED FROM DTC CODE " + objAllocation.sSecondDTCCode + " TO " + objAllocation.sFirstDTCCode + " BY " + objAllocation.sUserName + "";

                NpgsqlCommand.Parameters.AddWithValue("SecondDTrCode5", sSecondDTCCode);
                sDTCLocCode = objcon.get_value("SELECT \"DT_OM_SLNO\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\" = :SecondDTrCode5", NpgsqlCommand);

                sMaxNo = Convert.ToInt64(objcon.get_value("SELECT COALESCE(MAX(\"DRT_ID\"),0)+1 FROM \"TBLDTRTRANSACTION\""));
                //sMaxNo = objcon.Get_max_no("DRT_ID", "TBLDTRTRANSACTION");

                NpgsqlCommand.Parameters.AddWithValue("MaxNo1", sMaxNo);
                NpgsqlCommand.Parameters.AddWithValue("SecondDTrCode6", Convert.ToDouble(objAllocation.sSecondDTrCode));
                NpgsqlCommand.Parameters.AddWithValue("DTCLocCode1",Convert.ToInt16( sDTCLocCode));
                NpgsqlCommand.Parameters.AddWithValue("DTrId1", Convert.ToInt16(sDTrId));
                NpgsqlCommand.Parameters.AddWithValue("Desc1", sDesc);

                strQry = "INSERT INTO \"TBLDTRTRANSACTION\" (\"DRT_ID\",\"DRT_DTR_CODE\",\"DRT_LOC_ID\",\"DRT_LOC_TYPE\",\"DRT_TRANS_DATE\", ";
                strQry += " \"DRT_ACT_REFNO\",\"DRT_ACT_REFTYPE\",\"DRT_DESC\",\"DRT_DTR_STATUS\" ) VALUES (:MaxNo1,:SecondDTrCode6,";
                strQry += ":DTCLocCode1,'2',NOW(),:DTrId1,'9',:Desc1,'1')";
                objcon.ExecuteQry(strQry, NpgsqlCommand);


                // Inserting New Row with LiveFlag as '1' to the table TBLTRANSDTCMAPPING

                sMaxNo = Convert.ToInt64(objcon.get_value("SELECT COALESCE(MAX(\"TM_ID\"),0)+1 FROM \"TBLTRANSDTCMAPPING\""));
                //sMaxNo = objcon.Get_max_no("TM_ID", "TBLTRANSDTCMAPPING");

                NpgsqlCommand.Parameters.AddWithValue("MaxNo2", sMaxNo);
                NpgsqlCommand.Parameters.AddWithValue("FirstDTrCode7", Convert.ToDouble(objAllocation.sFirstDTrCode));
                NpgsqlCommand.Parameters.AddWithValue("SecondDTCCode7", objAllocation.sSecondDTCCode);
                NpgsqlCommand.Parameters.AddWithValue("Crby", Convert.ToInt32(objAllocation.sCrby));

                strQry = "INSERT INTO \"TBLTRANSDTCMAPPING\" (\"TM_ID\",\"TM_MAPPING_DATE\",\"TM_TC_ID\",\"TM_DTC_ID\",\"TM_LIVE_FLAG\",\"TM_CRBY\",\"TM_CRON\")";
                strQry += " VALUES (:MaxNo2,NOW(),:FirstDTrCode7,:SecondDTCCode7,'1',:Crby,NOW())";
                objcon.ExecuteQry(strQry, NpgsqlCommand);

                sMaxNo = Convert.ToInt64(objcon.get_value("SELECT COALESCE(MAX(\"TM_ID\"),0)+1 FROM \"TBLTRANSDTCMAPPING\""));
                //sMaxNo = objcon.Get_max_no("TM_ID", "TBLTRANSDTCMAPPING");

                NpgsqlCommand.Parameters.AddWithValue("MaxNo3", sMaxNo);
                NpgsqlCommand.Parameters.AddWithValue("SecondDTrCode8", Convert.ToDouble(objAllocation.sSecondDTrCode));
                NpgsqlCommand.Parameters.AddWithValue("FirstDTCCode8", objAllocation.sFirstDTCCode);
                NpgsqlCommand.Parameters.AddWithValue("Crby1", objAllocation.sCrby);

                strQry = "INSERT INTO \"TBLTRANSDTCMAPPING\" (\"TM_ID\",\"TM_MAPPING_DATE\",\"TM_TC_ID\",\"TM_DTC_ID\",\"TM_LIVE_FLAG\",\"TM_CRBY\",\"TM_CRON\")";
                strQry += " VALUES (:MaxNo3,NOW(),:SecondDTrCode8,:FirstDTCCode8,'1',:Crby1,NOW())";
                objcon.ExecuteQry(strQry, NpgsqlCommand);

                objcon.CommitTransaction();

                Arr[0] = "DTr Allocated Successfully";
                Arr[1] = "0";
                return Arr;

            }
            catch (Exception ex)
            {
                objcon.RollBackTrans();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;

            }
            
        }
    }
}
