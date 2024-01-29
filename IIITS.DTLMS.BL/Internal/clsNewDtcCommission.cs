using System;
using System.Data.OleDb;
using IIITS.PGSQL.DAL;
using Npgsql;

namespace IIITS.DTLMS.BL
{
    public class clsNewDtcCommission : clsFieldEnumeration
    {
        string strFormCode = "clsDTCCommision";
        public string lDtcId { get; set; }
        public string sDtcCode { get; set; }
        public string sDtcName { get; set; }
        public string sOMSectionName { get; set; }
        public string iConnectedKW { get; set; }
        public string iConnectedHP { get; set; }
        public string sInternalCode { get; set; }
        public string sPlatformType { get; set; }
        public string sServiceDate { get; set; }
        public string sCommisionDate { get; set; }
        public string sFeederChangeDate { get; set; }
        public string iKWHReading { get; set; }
        public string sTCMakeName { get; set; }
        public string sTCCapacity { get; set; }
        public string sTcCode { get; set; }
        public string sTcSlno { get; set; }
        public string sOldTcCode { get; set; }
        public string sCrBy { get; set; }
        public string sProjecttype { get; set; }
        public string sSaveType { get; set; }

        public string sTimeId { get; set; }

        public string sWOslno { get; set; }
        public string sOfficeCode { get; set; }

        public string sDTCImagePath { get; set; }
        public string sDTrImagePath { get; set; }

        // Workflow
        public string sFormName { get; set; }
        public string sWFOId { get; set; }

        public string sfeederoff { get; set; }
        public string sApproveObjectId { get; set; }
        

        //CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);

        public string[] SaveUpdateDtcDetails(clsNewDtcCommission objDtcMaster)
        {
            string[] Arr = new string[2];
            try
            {
                NpgsqlDataReader dr;
                string strQry = string.Empty;

                if (objDtcMaster.sOfficeCode.Length >= 4)
                {
                    objDtcMaster.sfeederoff = objDtcMaster.sOfficeCode.Substring(0, 4);
                }


                //strQry = "select * from \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" WHERE \"FD_FEEDER_CODE\"='" + objDtcMaster.sDtcCode.ToString().Substring(0, 6) + "' ";
                //strQry += " AND  \"FD_FEEDER_ID\"=\"FDO_FEEDER_ID\" AND CAST(\"FDO_OFFICE_CODE\" AS TEXT) LIKE '" + objDtcMaster.sfeederoff + "%'";
            
                //dr = ObjCon.Fetch(strQry);
                //if (!dr.Read())
                //{
                //    dr.Close();
                //    Arr[0] = "Code Does Not Match With The Feeder Code";
                //    Arr[1] = "2";
                //    return Arr;

                //}
                //dr.Close();

                if (objDtcMaster.sSaveType == "Save")
                {
                    if (objDtcMaster.lDtcId == null)
                    {
                        dr = ObjCon.Fetch("select \"DT_CODE\" from \"TBLDTCMAST\" where \"DT_CODE\"='" + objDtcMaster.sDtcCode + "'");
                        if (dr.Read())
                        {
                            dr.Close();
                            Arr[0] = "DTC Code Already Exists";
                            Arr[1] = "2";
                            return Arr;
                        }
                        dr.Close();


                        dr = ObjCon.Fetch("select * from \"TBLOMSECMAST\" where \"OM_CODE\"='" + objDtcMaster.sOMSectionName + "'");
                        if (!dr.Read())
                        {
                            dr.Close();
                            Arr[0] = "Enter Valid O&M Section Code ";
                            Arr[1] = "2";
                            return Arr;

                        }
                        dr.Close();
                        //dr = ObjCon.Fetch("select * from TBLTCMASTER where TC_CODE='" + objDtcMaster.sTcCode + "'");
                        //if (!dr.Read())
                        //{

                        //    dr.Close();
                        //    Arr[0] = "Enter Valid TC Code ";
                        //    Arr[1] = "2";
                        //    return Arr;

                        //}
                        //dr.Close();

                        ObjCon.BeginTransaction();

                        #region insert into Enumeration

                        objDtcMaster.sEnumDetailsID = Convert.ToString(ObjCon.Get_max_no("ED_ID", "TBLENUMERATIONDETAILS"));
                        if (objDtcMaster.sIsIPEnumDone == null || objDtcMaster.sIsIPEnumDone == "")
                        {
                            objDtcMaster.sIsIPEnumDone = "0";
                        }

                        strQry = "INSERT INTO \"TBLENUMERATIONDETAILS\" (\"ED_ID\",\"ED_OFFICECODE\",\"ED_OPERATOR1\",\"ED_OPERATOR2\",\"ED_WELD_DATE\",\"ED_FEEDERCODE\",\"ED_ENUM_TYPE\",\"ED_LOCTYPE\",\"ED_CRBY\",\"ED_IP_ENUM_DONE\",\"ED_STATUS_FLAG\",\"ED_UPDATE_ON\",\"ED_UPDATE_BY\") VALUES (";
                        strQry += " '" + objDtcMaster.sEnumDetailsID + "','" + objDtcMaster.sOfficeCode + "','" + objDtcMaster.sCrBy + "','" + objDtcMaster.sCrBy + "',";
                        strQry += "now(),'" + objDtcMaster.sFeederCode + "','2','2','" + objDtcMaster.sCrBy + "','" + objDtcMaster.sIsIPEnumDone + "','1' , now() , " + objDtcMaster.sCrBy + ")";
                       
                        ObjCon.ExecuteQry(strQry);

                        //strQry = "INSERT INTO \"TBLENUMERATIONDETAILS\" (\"ED_ID\",\"ED_OFFICECODE\",\"ED_OPERATOR1\",\"ED_OPERATOR2\",\"ED_FEEDERCODE\",\"ED_WELD_DATE\",\"ED_ENUM_TYPE\",\"ED_LOCTYPE\",\"ED_CRBY\",\"ED_IP_ENUM_DONE\",\"ED_UPDATE_BY\") VALUES (";
                        //strQry += " '" + objDtcMaster.sEnumDetailsID + "','" + objDtcMaster.sOfficeCode + "','" + objDtcMaster.sFeederCode + "',now(),";
                        //strQry += " '2','2','" + objDtcMaster.sCrBy + "','" + objDtcMaster.sIsIPEnumDone + "','1','" + objDtcMaster.sCrBy + "')";
                        //ObjCon.ExecuteQry(strQry);

                        if (objDtcMaster.bIsDTLMSDetails == true)
                        {
                            sIPCESCValue = "1";
                        }
                        if (objDtcMaster.bIsCESCDetails == true)
                        {
                            sIPCESCValue = "2";
                        }
                        if (objDtcMaster.bIsIPDetails == true)
                        {
                            sIPCESCValue = "3";
                        }


                        if (objDtcMaster.sTCSlno == null || objDtcMaster.sTCSlno == "")
                        {
                            objDtcMaster.sTCSlno = "0";
                        }
                        if (objDtcMaster.sTCMake == null || objDtcMaster.sTCMake == "")
                        {
                            objDtcMaster.sTCMake = "0";
                        }
                        if (objDtcMaster.sTCCapacity == null || objDtcMaster.sTCCapacity == "")
                        {
                            objDtcMaster.sTCCapacity = "0";
                        }
                        if (objDtcMaster.sOldDTCCode == null || objDtcMaster.sOldDTCCode == "")
                        {
                            objDtcMaster.sOldDTCCode = "0";
                        }
                        if (objDtcMaster.sIPDTCCode == null || objDtcMaster.sIPDTCCode == "")
                        {
                            objDtcMaster.sIPDTCCode = "0";
                        }
                        if (objDtcMaster.iConnectedKW == null || objDtcMaster.iConnectedKW == "")
                        {
                            objDtcMaster.iConnectedKW = "0";
                        }
                        if (objDtcMaster.iConnectedHP == null || objDtcMaster.iConnectedHP == "")
                        {
                            objDtcMaster.iConnectedHP = "0";
                        }
                        if (objDtcMaster.iKWHReading == null || objDtcMaster.iKWHReading == "")
                        {
                            objDtcMaster.iKWHReading = "0";
                        }
                        if (objDtcMaster.sPlatformType == null || objDtcMaster.sPlatformType == "")
                        {
                            objDtcMaster.sPlatformType = "0";
                        }
                        if (objDtcMaster.sBreakertype == null || objDtcMaster.sBreakertype == "")
                        {
                            objDtcMaster.sBreakertype = "0";
                        }
                        if (objDtcMaster.sInternalCode == null || objDtcMaster.sInternalCode == "")
                        {
                            objDtcMaster.sInternalCode = "0";
                        }
                        if (objDtcMaster.sDTCMeters == null || objDtcMaster.sDTCMeters == "")
                        {
                            objDtcMaster.sDTCMeters = "0";
                        }
                        if (objDtcMaster.sHTProtect == null || objDtcMaster.sHTProtect == "")
                        {
                            objDtcMaster.sHTProtect = "0";

                        }

                        if (objDtcMaster.sLTProtect == null || objDtcMaster.sLTProtect == "")
                        {
                            objDtcMaster.sLTProtect = "0";

                        }
                        if (objDtcMaster.sGrounding == null || objDtcMaster.sGrounding == "")
                        {
                            objDtcMaster.sGrounding = "0";
                        }
                        if (objDtcMaster.sArresters == null || objDtcMaster.sArresters == "")
                        {
                            objDtcMaster.sArresters = "0";
                        }

                        if (objDtcMaster.sLoadtype == null || objDtcMaster.sLoadtype == "")
                        {
                            objDtcMaster.sLoadtype = "0";
                        }


                        if (objDtcMaster.sLTlinelength == null || objDtcMaster.sLTlinelength == "")
                        {
                            objDtcMaster.sLTlinelength = "0";
                        }

                        if (objDtcMaster.sLongitude == null || objDtcMaster.sLongitude == "")
                        {
                            objDtcMaster.sLongitude = "";
                        }
                        
                        if (objDtcMaster.sLatitude == null || objDtcMaster.sLatitude == "")
                        {
                            objDtcMaster.sLatitude = "";
                        }
                        if (objDtcMaster.sDepreciation == null || objDtcMaster.sDepreciation == "")
                        {
                            objDtcMaster.sDepreciation = "";
                        }
                        if (objDtcMaster.sIPCESCValue == null || objDtcMaster.sIPCESCValue == "")
                        {
                            objDtcMaster.sIPCESCValue = "";
                        }
                        if (objDtcMaster.sTankCapacity == null || objDtcMaster.sTankCapacity == "")
                        {
                            objDtcMaster.sTankCapacity = "";
                        }

                        if (objDtcMaster.sTCWeight == null || objDtcMaster.sTCWeight == "")
                        {
                            objDtcMaster.sTCWeight = "";
                        }
                        if (objDtcMaster.sInfosysAsset == null || objDtcMaster.sInfosysAsset == "")
                        {
                            objDtcMaster.sInfosysAsset = "";
                        }
                        if (objDtcMaster.sRating == null || objDtcMaster.sRating == "")
                        {
                            objDtcMaster.sRating = "";
                        }
                        if (objDtcMaster.sStarRate == null || objDtcMaster.sStarRate == "")
                        {
                            objDtcMaster.sStarRate = "";
                        }

                        if (objDtcMaster.sLocType == null || objDtcMaster.sLocType == "")
                        {
                            objDtcMaster.sLocType = "0";
                        }

                        if (objDtcMaster.sEnumType == null || objDtcMaster.sEnumType == "")
                        {
                            objDtcMaster.sEnumType = "0";
                        }
                        objDtcMaster.sEnumDTCID = Convert.ToString(ObjCon.Get_max_no("DTE_ID", "TBLDTCENUMERATION"));
                       // objDtcMaster.sEnumDetailsID = ObjCon.get_value("SELECT nvl(max(DTE_ID),0)+1 FROM TBLDTCENUMERATION ");
                        strQry = "INSERT INTO \"TBLDTCENUMERATION\" (\"DTE_ID\",\"DTE_ED_ID\",\"DTE_TC_CODE\",\"DTE_TC_SLNO\",\"DTE_MAKE\",\"DTE_CAPACITY\",\"DTE_TC_MANFDATE\",";
                        strQry += " \"DTE_NAME\",\"DTE_DTCCODE\",\"DTE_CESCCODE\",\"DTE_IPCODE\",\"DTE_ENUM_DATE\",\"DTE_TOTAL_CON_KW\",\"DTE_TOTAL_CON_HP\",\"DTE_KWH_READING\",\"DTE_TRANS_COMMISION_DATE\",";
                        strQry += " \"DTE_LAST_SERVICE_DATE\",\"DTE_PLATFORM\",\"DTE_BREAKER_TYPE\",\"DTE_INTERNAL_CODE\",\"DTE_DTCMETERS\",\"DTE_HT_PROTECT\",\"DTE_LT_PROTECT\",\"DTE_GROUNDING\",";
                        strQry += " \"DTE_ARRESTERS\",\"DTE_LOADTYPE\",\"DTE_PROJECTTYPE\",\"DTE_LT_LINE\",\"DTE_LONGITUDE\",\"DTE_LATITUDE\",\"DTE_DEPRECIATION\",\"DTE_CRBY\",\"DTE_ISIPCESC\",\"DTE_TANK_CAPACITY\",";
                        strQry += " \"DTE_TC_WEIGHT\",\"DTE_INFOSYS_ASSET\",\"DTE_RATING\",\"DTE_STAR_RATE\") VALUES (";
                        strQry += " '" + objDtcMaster.sEnumDTCID + "','" + objDtcMaster.sEnumDetailsID + "','" + objDtcMaster.sTcCode + "','" + objDtcMaster.sTCSlno + "',";
                        strQry += " '" + objDtcMaster.sTCMake + "','" + objDtcMaster.sTCCapacity + "',";
                        if (objDtcMaster.sTCManfDate!=null && objDtcMaster.sTCManfDate != "")
                        {
                            strQry += " TO_DATE('" + objDtcMaster.sTCManfDate + "','dd/MM/yyyy'),";
                        }
                        else
                        {
                            strQry += " null,";
                        }
                        strQry += " '" + objDtcMaster.sDtcName + "','" + objDtcMaster.sDtcCode + "','" + objDtcMaster.sOldDTCCode + "','" + objDtcMaster.sIPDTCCode + "',";
                        if (objDtcMaster.sEnumDate != null)
                        {
                            strQry += " TO_DATE('" + objDtcMaster.sEnumDate + "','dd/MM/yyyy'), ";
                        }
                        else
                        {
                            strQry += " null,";
                        }
                        strQry += " '" + objDtcMaster.iConnectedKW + "','" + objDtcMaster.iConnectedHP + "','" + objDtcMaster.iKWHReading + "',";
                        if (objDtcMaster.sCommisionDate != null && objDtcMaster.sCommisionDate != "")
                        {
                            strQry += " TO_DATE('" + objDtcMaster.sCommisionDate + "','dd/MM/yyyy'),";
                        }
                        else
                        {
                            strQry += " null,";
                        }
                        if (objDtcMaster.sLastServiceDate != null)
                        {
                            strQry += " TO_DATE('" + objDtcMaster.sLastServiceDate + "','dd/MM/yyyy'),";
                        }
                       else
                        {
                            strQry += " null,";
                        }
                    strQry += " '" + objDtcMaster.sPlatformType + "',";
                        strQry += " '" + objDtcMaster.sBreakertype + "','" + objDtcMaster.sInternalCode + "','" + objDtcMaster.sDTCMeters + "','" + objDtcMaster.sHTProtect + "',";
                        strQry += " '" + objDtcMaster.sLTProtect + "','" + objDtcMaster.sGrounding + "','" + objDtcMaster.sArresters + "',";
                        strQry += " '" + objDtcMaster.sLoadtype + "','" + objDtcMaster.sProjecttype + "','" + objDtcMaster.sLTlinelength + "','" + objDtcMaster.sLongitude + "',";
                        strQry += " '" + objDtcMaster.sLatitude + "','" + objDtcMaster.sDepreciation + "','" + objDtcMaster.sCrBy + "','" + sIPCESCValue + "',";
                        strQry += " '" + objDtcMaster.sTankCapacity + "','" + objDtcMaster.sTCWeight + "','" + objDtcMaster.sInfosysAsset + "','" + objDtcMaster.sRating + "','" + objDtcMaster.sStarRate + "')";
                        ObjCon.ExecuteQry(strQry);


                        strQry = "UPDATE \"TBLTEMPENUMERATIONDETAILS\" SET \"TE_TC_STATUS\" = '1' WHERE CAST(\"TE_DTCCODE\" AS TEXT) = '" + objDtcMaster.sDtcCode + "' AND CAST(\"TE_CRBY\" AS TEXT)= '" + objDtcMaster.sCrBy + "'";
                        ObjCon.ExecuteQry(strQry);

                        //strQry =  "INSERT INTO \"TBLDTCENUMERATION\" (\"DTE_ID\",\"DTE_ED_ID\",\"DTE_TC_CODE\",\"DTE_TC_MANFDATE\", \"DTE_NAME\",\"DTE_DTCCODE\",";
                        //strQry += " \"DTE_ENUM_DATE\",\"DTE_TRANS_COMMISION_DATE\", \"DTE_LAST_SERVICE_DATE\",\"DTE_PROJECTTYPE\",\"DTE_CRBY\") VALUES (";
                        //strQry += " '" + objDtcMaster.sEnumDTCID + "','" + objDtcMaster.sEnumDetailsID + "','" + objDtcMaster.sTcCode + "',";
                        //strQry += " TO_DATE('" + objDtcMaster.sTCManfDate + "','dd/MM/yyyy'),";
                        //strQry += " '" + objDtcMaster.sDtcName + "','" + objDtcMaster.sDtcCode + "',";
                        //strQry += " TO_DATE('" + objDtcMaster.sEnumDate + "','dd/MM/yyyy'),";
                        //strQry += " TO_DATE('" + objDtcMaster.sCommisionDate + "','dd/MM/yyyy'),TO_DATE('" + objDtcMaster.sLastServiceDate + "','dd/MM/yyyy'),";


                        //strQry += " '" + objDtcMaster.sProjecttype + "',";
                        //strQry += " '" + objDtcMaster.sCrBy + "')";

                        //ObjCon.ExecuteQry(strQry);


                        #endregion
                        objDtcMaster.lDtcId = Convert.ToString(ObjCon.Get_max_no("DT_ID", "TBLDTCMAST"));
                        if (objDtcMaster.sWOslno == null || objDtcMaster.sWOslno == "")
                        {
                            objDtcMaster.sWOslno = "0";
                        }

                        string strFeederSlno = ObjCon.get_value("SELECT \"FD_FEEDER_ID\" FROM \"TBLFEEDERMAST\" WHERE \"FD_FEEDER_CODE\"='" + objDtcMaster.sFeederCode + "'");

                        strQry = "Insert into \"TBLDTCMAST\" (\"DT_ID\",\"DT_CODE\",\"DT_NAME\",\"DT_OM_SLNO\",\"DT_TOTAL_CON_KW\",\"DT_TOTAL_CON_HP\",\"DT_KWH_READING\",";
                        strQry += " \"DT_INTERNAL_CODE\",\"DT_TC_ID\",\"DT_CON_DATE\",\"DT_LAST_SERVICE_DATE\",\"DT_TRANS_COMMISION_DATE\",";
                        strQry += " \"DT_FDRCHANGE_DATE\",\"DT_FDRSLNO\",\"DT_CRBY\",\"DT_CRON\",\"DT_WO_ID\",\"DT_PROJECTTYPE\",\"DT_STATUS\") VALUES ('" + objDtcMaster.lDtcId + "','" + objDtcMaster.sDtcCode + "',";
                        strQry += " '" + objDtcMaster.sDtcName + "','" + objDtcMaster.sOMSectionName + "','" + objDtcMaster.iConnectedKW + "',";
                        strQry += "'" + objDtcMaster.iConnectedHP + "','" + objDtcMaster.iKWHReading + "',";
                        strQry += " '" + objDtcMaster.sInternalCode + "','" + objDtcMaster.sTcCode + "',";
                        if (objDtcMaster.sCommisionDate != null && objDtcMaster.sCommisionDate != "")
                        {
                            strQry += " TO_DATE('" + objDtcMaster.sCommisionDate + "','dd/MM/yyyy'),";
                        }
                        else
                        {
                            strQry += " null,";
                        }
                        if (objDtcMaster.sServiceDate != null && objDtcMaster.sServiceDate != "")
                        {
                            strQry += " TO_DATE('" + objDtcMaster.sServiceDate + "','dd/MM/yyyy'),";
                        }
                        else
                        {
                            strQry += " null,";
                        }
                        if (objDtcMaster.sCommisionDate != null && objDtcMaster.sCommisionDate != "")
                        {
                            strQry += " TO_DATE('" + objDtcMaster.sCommisionDate + "','dd/MM/yyyy'),";
                        }
                        else
                        {
                            strQry += " null,";
                        }
                        if (objDtcMaster.sFeederChangeDate != null && objDtcMaster.sFeederChangeDate != "")
                        {
                            strQry += " TO_DATE('" + objDtcMaster.sFeederChangeDate + "','dd/MM/yyyy'), ";
                        }
                        else
                        {
                            strQry += " null,";
                        }
                        strQry += " '" + objDtcMaster.sFeederCode + "','" + objDtcMaster.sCrBy + "',now(),'" + objDtcMaster.sWOslno + "','" + objDtcMaster.sProjecttype + "',1 )";
                        ObjCon.ExecuteQry(strQry);

                        long sTm_id = ObjCon.Get_max_no("TM_ID", "TBLTRANSDTCMAPPING");

                        strQry = "INSERT INTO \"TBLTRANSDTCMAPPING\" (\"TM_ID\",\"TM_TC_ID\",\"TM_DTC_ID\",\"TM_MAPPING_DATE\",\"TM_CRBY\") ";
                        strQry += "VALUES('" + sTm_id + "','" + objDtcMaster.sTcCode.ToUpper() + "','" + objDtcMaster.sDtcCode + "',";
                        strQry += " now(),'" + objDtcMaster.sCrBy + "')";
                        ObjCon.ExecuteQry(strQry);

                        //if (objDtcMaster.sStarRate == null || objDtcMaster.sStarRate == "")
                        //{
                        //    objDtcMaster.sStarRate = "";
                        //}

                        strQry = "INSERT INTO \"TBLDTCTRANSACTION\" (\"DCT_ID\",\"DCT_DTC_CODE\",\"DCT_DTR_CODE\",\"DCT_DTR_STATUS\",\"DCT_TRANS_DATE\",";
                        strQry += "\"DCT_ACT_REFNO\",\"DCT_ACT_REFTYPE\",\"DCT_DESC\",\"DCT_ENTRYDATE\",\"DCT_CANCEL_FLAG\",\"DCT_CANCEL_BY\") ";
                        strQry += "VALUES ((SELECT max(\"DCT_ID\")+1 FROM \"TBLDTCTRANSACTION\"),'" + objDtcMaster.sDtcCode.ToUpper() + "','0','1',now(),'','2','NEW DTC ADDED WITHOUT TC CODE (FROM INTERNAL APPLICATION)',now(),'0','0')";
                        ObjCon.ExecuteQry(strQry);


                        objDtcMaster.sQCApprovalId = Convert.ToString(ObjCon.Get_max_no("QA_ID", "TBLQCAPPROVED"));

                        strQry = "INSERT INTO \"TBLQCAPPROVED\" (\"QA_ID\",\"QA_ED_ID\",\"QA_OFFICECODE\",\"QA_OPERATOR1\",\"QA_OPERATOR2\",\"QA_WELD_DATE\",\"QA_FEEDERCODE\",";
                        strQry += " \"QA_LOCTYPE\",\"QA_LOCNAME\",\"QA_LOCADDRESS\",\"QA_TYPE\",\"QA_CRBY\",\"QA_CRON\") VALUES (";
                        strQry += " '" + objDtcMaster.sQCApprovalId + "','" + objDtcMaster.sEnumDetailsID + "','" + objDtcMaster.sOfficeCode + "',";
                        strQry += " '" + objDtcMaster.sCrBy + "','" + objDtcMaster.sCrBy + "',";
                        strQry += " TO_DATE('" + objDtcMaster.sWeldDate + "','dd/MM/yyyy'),'" + objDtcMaster.sFeederCode + "','" + objDtcMaster.sEnumType + "',";
                        strQry += " '" + objDtcMaster.sLocName + "','" + objDtcMaster.sLocAddress + "','" + objDtcMaster.sEnumType + "','" + objDtcMaster.sCrBy + "',NOW())";
                        ObjCon.ExecuteQry(strQry);

                        objDtcMaster.sApproveObjectId = Convert.ToString(ObjCon.Get_max_no("QAO_ID", "TBLQCAPPROVEDOBJECTS"));


                        if (objDtcMaster.sRating == null || objDtcMaster.sRating == "")
                        {
                            objDtcMaster.sRating = "0";
                        }
                        if (objDtcMaster.sStarRate == null || objDtcMaster.sStarRate == "")
                        {
                            objDtcMaster.sStarRate = "0";
                        }
                        if (objDtcMaster.sTCType == null || objDtcMaster.sTCType == "")
                        {
                            objDtcMaster.sTCType = "0";
                        }

                        strQry = "INSERT INTO \"TBLQCAPPROVEDOBJECTS\" (\"QAO_ID\",\"QAO_QA_ID\",\"QAO_TC_CODE\",\"QAO_TC_SLNO\",\"QAO_MAKE\",\"QAO_CAPACITY\",\"QAO_TC_TYPE\",";
                        strQry += " \"QAO_TC_MANFDATE1\",\"QAO_NAME\",\"QAO_DTCCODE\",\"QAO_CESCCODE\",\"QAO_IPCODE\",\"QAO_TOTAL_CON_KW\",\"QAO_TOTAL_CON_HP\",\"QAO_KWH_READING\",\"QAO_TRANS_COMMISION_DATE\",\"QAO_LAST_SERVICE_DATE\"";
                        strQry += " ,\"QAO_PLATFORM\",\"QAO_BREAKER_TYPE\",\"QAO_INTERNAL_CODE\",\"QAO_DTCMETERS\",\"QAO_HT_PROTECT\",\"QAO_LT_PROTECT\",\"QAO_GROUNDING\",\"QAO_ARRESTERS\",\"QAO_LOADTYPE\",\"QAO_PROJECTTYPE\",";

                        strQry += " \"QAO_LT_LINE\",\"QAO_DEPRECIATION\",\"QAO_CRBY\",\"QAO_CRON\",\"QAO_RATING\",\"QAO_STAR_RATE\",\"QAO_TANK_CAPACITY\",\"QAO_TC_WEIGHT\",\"QAO_INFOSYS_ASSET\",\"QAO_ENUM_DATE\",\"QAO_LATITUDE\",\"QAO_LONGITUDE\",\"QAO_TC_MANFDATE\",\"QAO_HT_LINE\"  ) VALUES (";
                        strQry += " '" + objDtcMaster.sApproveObjectId + "','" + objDtcMaster.sQCApprovalId + "','" + objDtcMaster.sTcCode + "','" + objDtcMaster.sTCSlno + "','" + objDtcMaster.sTCMake + "','" + objDtcMaster.sTCCapacity + "','" + objDtcMaster.sTCType + "',";
                        strQry += " TO_DATE('" + objDtcMaster.sTCManfDate + "','dd/MM/yyyy'),'" + objDtcMaster.sDtcName + "','" + objDtcMaster.sDtcCode + "','" + objDtcMaster.sOldDTCCode + "','" + objDtcMaster.sIPDTCCode + "','" + objDtcMaster.iConnectedKW + "','" + objDtcMaster.iConnectedHP + "','" + objDtcMaster.iKWHReading + "',";
                        strQry += " TO_DATE('" + objDtcMaster.sCommisionDate + "','dd/MM/yyyy'),TO_DATE('" + objDtcMaster.sLastServiceDate + "','dd/MM/yyyy'),'" + objDtcMaster.sPlatformType + "','" + objDtcMaster.sBreakertype + "','" + objDtcMaster.sInternalCode + "','" + objDtcMaster.sDTCMeters + "','" + objDtcMaster.sHTProtect + "','" + objDtcMaster.sLTProtect + "',";
                        strQry += " '" + objDtcMaster.sGrounding + "','" + objDtcMaster.sArresters + "','" + objDtcMaster.sLoadtype + "','" + objDtcMaster.sProjecttype + "','" + objDtcMaster.sLTlinelength + "','" + objDtcMaster.sDepreciation + "','" + objDtcMaster.sCrBy + "',now(),";

                        strQry += " '" + objDtcMaster.sRating + "','" + objDtcMaster.sStarRate + "','" + objDtcMaster.sTankCapacity + "','" + objDtcMaster.sTCWeight + "','" + objDtcMaster.sInfosysAsset + "',TO_DATE('" + objDtcMaster.sEnumDate + "','dd/MM/yyyy'),'" + objDtcMaster.sLatitude + "','" + objDtcMaster.sLongitude + "',TO_DATE('" + objDtcMaster.sTCManfDate + "','dd/MM/yyyy'),'" + objDtcMaster.sHTlinelength + "')";

                        ObjCon.ExecuteQry(strQry);


                        Arr[0] = "DTC Details Saved Successfully";
                        Arr[1] = "0";
                      ObjCon.CommitTransaction();
                        return Arr;
                    }
                }
                else if (objDtcMaster.sSaveType == "Update")
                {
                    strQry = "SELECT \"DT_ID\" FROM \"TBLDTCMAST\" WHERE \"DT_CODE\"='" + objDtcMaster.sDtcCode.ToUpper() + "'";
                    string sDt_id = ObjCon.get_value(strQry);

                    if (objDtcMaster.iConnectedKW == null || objDtcMaster.iConnectedKW == "")
                    {
                        objDtcMaster.iConnectedKW = "0";
                    }
                    if (objDtcMaster.iConnectedHP == null || objDtcMaster.iConnectedHP == "")
                    {
                        objDtcMaster.iConnectedHP = "0";
                    }
                    if (objDtcMaster.iKWHReading == null || objDtcMaster.iKWHReading == "")
                    {
                        objDtcMaster.iKWHReading = "0";
                    }

                    strQry = "UPDATE \"TBLDTCMAST\" SET \"DT_NAME\"='" + objDtcMaster.sDtcName + "',\"DT_INTERNAL_CODE\"='" + objDtcMaster.sInternalCode + "',\"DT_TOTAL_CON_KW\"='" + objDtcMaster.iConnectedKW + "',";
                    strQry += " \"DT_TRANS_COMMISION_DATE\"=TO_DATE('" + objDtcMaster.sCommisionDate + "','dd/MM/yyyy'),\"DT_LAST_SERVICE_DATE\"=TO_DATE('" + objDtcMaster.sServiceDate + "','dd/MM/yyyy'),\"DT_PROJECTTYPE\"='" + objDtcMaster.sProjecttype + "',";
                    strQry += " \"DT_FDRCHANGE_DATE\"=TO_DATE('" + objDtcMaster.sFeederChangeDate + "','dd/MM/yyyy'),\"DT_KWH_READING\"='" + objDtcMaster.iKWHReading + "',\"DT_TOTAL_CON_HP\"='" + objDtcMaster.iConnectedHP + "' WHERE \"DT_ID\"='" + sDt_id + "'";
                    ObjCon.ExecuteQry(strQry);
                    Arr[0] = "Updated Successfully";
                    Arr[1] = "0";
                    return Arr;
                }
                


                return Arr;

            }
            catch (Exception ex)
            {
                // ObjCon.RollBack();
                //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveUpdateDtcDetails");
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }
    }
}
