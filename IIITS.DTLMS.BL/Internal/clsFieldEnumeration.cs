using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.PGSQL.DAL;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Collections;
using System.Configuration;
using Npgsql;
using NpgsqlTypes;

namespace IIITS.DTLMS.BL
{
    public class clsFieldEnumeration
    {
        string strFormCode = "clsFieldEnumeration";
        //  CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        public string sTcId { get; set; }
        public string sCrBy { get; set; }
        public string sTimeId { get; set; }

        public string sOfficeCode { get; set; }
        public string sFeederCode { get; set; }
        public string sWeldDate { get; set; }
        public string sOperator1 { get; set; }
        public string sOperator2 { get; set; }

        public string sEnumDetailsID { get; set; }
        public string sEnumDTCID { get; set; }
        public string sClientIP { get; set; }


        public string sDTCWithoutDTR { get; set; }
        //TC Details
        public string sTcCode { get; set; }
        public string sTCSlno { get; set; }
        public string sEDFeederBifurcation { get; set; }

        public string sTCCapacity { get; set; }
        public string sTCManfDate { get; set; }
        public string sTCMake { get; set; }
        public string sNamePlatePhotoPath { get; set; }
        public string sSSPlatePhotoPath { get; set; }
        public string sMakeName { get; set; }
        public string sTankCapacity { get; set; }
        public string sTCWeight { get; set; }
        public string sRating { get; set; }
        public string sStarRate { get; set; }
        public string sDTCOldDTCCode { get; set; }


        //    public string sDtcWithoutDTR { get; set; }
        public bool bTCSlNoNotExists { get; set; }

        //DTC Details
        public string sDTCName { get; set; }
        public string sDTCCode { get; set; }
        public string sOldDTCCode { get; set; }
        public string sIPDTCCode { get; set; }
        public string sEnumDate { get; set; }
        public string sOldCodePhotoPath { get; set; }
        public string sDTLMSCodePhotoPath { get; set; }
        public string sIPEnumCodePhotoPath { get; set; }
        public string sInfosysCodePhotoPath { get; set; }
        public string sDTCPhotoPath { get; set; }
        public string sInfosysAsset { get; set; }
        public string sLocation { get; set; }

        //Other DTC Details       
        public string sConnectedKW { get; set; }
        public string sConnectedHP { get; set; }
        public string sInternalCode { get; set; }
        public string sPlatformType { get; set; }
        public string sConnectionDate { get; set; }
        public string sInspectionDate { get; set; }
        public string sLastServiceDate { get; set; }
        public string sCommisionDate { get; set; }
        public string sDtrcommissiondate { get; set; }
        public string sKWHReading { get; set; }
        public string sLTlinelength { get; set; }
        public string sHTlinelength { get; set; }
        public string sArresters { get; set; }
        public string sGrounding { get; set; }
        public string sHTProtect { get; set; }
        public string sLTProtect { get; set; }
        public string sDTCMeters { get; set; }
        public string sBreakertype { get; set; }
        public string sLatitude { get; set; }
        public string sLongitude { get; set; }
        public string sProjecttype { get; set; }
        public string sLoadtype { get; set; }
        public string sDepreciation { get; set; }

        public bool bIsIPDetails { get; set; }
        public bool bIsCESCDetails { get; set; }
        public bool bIsDTLMSDetails { get; set; }
        public string sIPCESCValue { get; set; }

        //Reject & Pending
        public string sRemark { get; set; }
        public string sQcRejectId { get; set; }
        public string sPendingForClarId { get; set; }
        public string sQCApprovalId { get; set; }

        //Store
        public string sLocType { get; set; }
        public string sLocName { get; set; }
        public string sLocAddress { get; set; }
        public string sTCType { get; set; }

        public string sEnumType { get; set; }
        public string sStatus { get; set; }

        public string sIsIPEnumDone { get; set; }
        public string sApproveStatus { get; set; }
        public string sUserType { get; set; }
        public string sPriorityLevel { get; set; }
        public string stempdtccode { get; set; }

        public string sTimsCode { get; set; }
        public string sDtcWithoutDtrFlag { get; set; }
        public string staggedDTR { get; set; }
        // public string sLocation { get; set; }
        public string sSpecialCase { get; set; }
        public string stempstatus { get; set; }
        public string locationtype { get; set; }



        public string sThroughApp { get; set; }

        int Circle_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Circle_code"]);
        int Division_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Division_code"]);
        int SubDiv_code = Convert.ToInt32(ConfigurationSettings.AppSettings["SubDiv_code"]);
        int Section_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Section_code"]);
        int Feeder_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Feeder_code"]);

        public string[] SaveFieldEnumerationDetails(clsFieldEnumeration objFieldEnum)
        {
            string[] Arr = new string[3];
            string[] Arr1 = new string[2];

            string strQry = string.Empty;
            string strOfficeCode = string.Empty;
            string sRes = string.Empty;
            PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);

            try
            {

                if (objFieldEnum.sOfficeCode.Length >= Section_code)
                {
                    strOfficeCode = objFieldEnum.sOfficeCode.Substring(0, SubDiv_code);
                }

                if (objFieldEnum.bIsDTLMSDetails == true)
                {
                    sIPCESCValue = "1";
                }
                if (objFieldEnum.bIsCESCDetails == true)
                {
                    sIPCESCValue = "2";
                }
                if (objFieldEnum.bIsIPDetails == true)
                {
                    sIPCESCValue = "3";
                }
                else
                {
                    sIPCESCValue = "0";
                }

                if (objFieldEnum.sLTlinelength == "")
                {
                    objFieldEnum.sLTlinelength = "0";
                }

                if (objFieldEnum.sIsIPEnumDone == null)
                {
                    objFieldEnum.sIsIPEnumDone = "0";
                }

                clsPreQCApproval objpreQC = new clsPreQCApproval();
                string sLevel = string.Empty;
                sLevel = objpreQC.GetNextApprovalLevel(objFieldEnum.sUserType);

                try
                {
                    //NpgsqlCommand cmd = new NpgsqlCommand("sp_savefieldenumerationdetails");
                    //  NpgsqlCommand cmd = new NpgsqlCommand("sp_savefieldenumerationdetailsnew");
                    NpgsqlCommand cmd = new NpgsqlCommand("sp_savefieldenumerationdetailsnewweb");
                    cmd.Parameters.AddWithValue("stccode", objFieldEnum.sTcCode);
                    //cmd.Parameters.AddWithValue("soperator1", objFieldEnum.sOperator1);
                    cmd.Parameters.AddWithValue("strofficecode", objFieldEnum.sOfficeCode);
                    cmd.Parameters.AddWithValue("senumdetailsid", objFieldEnum.sEnumDetailsID);
                    cmd.Parameters.AddWithValue("sdtccode", objFieldEnum.sDTCCode);
                    cmd.Parameters.AddWithValue("stcmake", objFieldEnum.sTCMake);
                    cmd.Parameters.AddWithValue("stccapacity", objFieldEnum.sTCCapacity);
                    cmd.Parameters.AddWithValue("smakename", objFieldEnum.sMakeName);
                    cmd.Parameters.AddWithValue("stcslno", objFieldEnum.sTCSlno);
                    //cmd.Parameters.AddWithValue("soperator2", objFieldEnum.sOperator2);
                    cmd.Parameters.AddWithValue("sfeedercode", objFieldEnum.sFeederCode);
                    cmd.Parameters.AddWithValue("sisipenumdone", objFieldEnum.sIsIPEnumDone);
                    cmd.Parameters.AddWithValue("swelddate", objFieldEnum.sWeldDate);
                    cmd.Parameters.AddWithValue("sdtcname", objFieldEnum.sDTCName);
                    cmd.Parameters.AddWithValue("solddtccode", objFieldEnum.sOldDTCCode);
                    cmd.Parameters.AddWithValue("sipdtccode", objFieldEnum.sIPDTCCode);
                    cmd.Parameters.AddWithValue("stcmanfdate", objFieldEnum.sTCManfDate);
                    cmd.Parameters.AddWithValue("sconnectedkw", objFieldEnum.sConnectedKW);
                    cmd.Parameters.AddWithValue("sconnectedhp", objFieldEnum.sConnectedHP);
                    cmd.Parameters.AddWithValue("skwhreading", objFieldEnum.sKWHReading);
                    cmd.Parameters.AddWithValue("senumdate", objFieldEnum.sWeldDate);
                    cmd.Parameters.AddWithValue("splatformtype", objFieldEnum.sPlatformType);
                    cmd.Parameters.AddWithValue("scommisiondate", objFieldEnum.sCommisionDate == null ? "" : objFieldEnum.sCommisionDate);
                    cmd.Parameters.AddWithValue("sdtrcommissiondate", objFieldEnum.sDtrcommissiondate == null ? "" : objFieldEnum.sDtrcommissiondate);
                    cmd.Parameters.AddWithValue("slastservicedate", objFieldEnum.sLastServiceDate);
                    cmd.Parameters.AddWithValue("sbreakertype", objFieldEnum.sBreakertype);
                    cmd.Parameters.AddWithValue("sinternalcode", objFieldEnum.sInternalCode);
                    cmd.Parameters.AddWithValue("sdtcmeters", objFieldEnum.sDTCMeters);
                    cmd.Parameters.AddWithValue("shtprotect", objFieldEnum.sHTProtect);
                    cmd.Parameters.AddWithValue("sltprotect", objFieldEnum.sLTProtect);
                    cmd.Parameters.AddWithValue("sgrounding", objFieldEnum.sGrounding);
                    cmd.Parameters.AddWithValue("sarresters", objFieldEnum.sArresters);
                    cmd.Parameters.AddWithValue("sloadtype", objFieldEnum.sLoadtype);
                    cmd.Parameters.AddWithValue("sprojecttype", objFieldEnum.sProjecttype);
                    cmd.Parameters.AddWithValue("sltlinelength", objFieldEnum.sLTlinelength);
                    cmd.Parameters.AddWithValue("shtlinelength", objFieldEnum.sHTlinelength);
                    cmd.Parameters.AddWithValue("slongitude", objFieldEnum.sLongitude);
                    cmd.Parameters.AddWithValue("slatitude", objFieldEnum.sLatitude);
                    cmd.Parameters.AddWithValue("sdepreciation", objFieldEnum.sDepreciation);
                    cmd.Parameters.AddWithValue("scrby", Convert.ToString(objFieldEnum.sCrBy ?? "0"));
                    cmd.Parameters.AddWithValue("sipcescvalue", objFieldEnum.sIPCESCValue);
                    cmd.Parameters.AddWithValue("stankcapacity", objFieldEnum.sTankCapacity);
                    cmd.Parameters.AddWithValue("stcweight", objFieldEnum.sTCWeight == null ? "0" : objFieldEnum.sTCWeight);
                    cmd.Parameters.AddWithValue("sinfosysasset", objFieldEnum.sInfosysAsset == null ? "" : objFieldEnum.sInfosysAsset);
                    cmd.Parameters.AddWithValue("srating", objFieldEnum.sRating);
                    cmd.Parameters.AddWithValue("sstarrate", objFieldEnum.sStarRate);
                    cmd.Parameters.AddWithValue("slevel", sLevel == "" ? "5" : sLevel);
                    cmd.Parameters.AddWithValue("susertype", objFieldEnum.sUserType == null ? "" : objFieldEnum.sUserType);
                    cmd.Parameters.AddWithValue("sstatus", objFieldEnum.sStatus);
                    cmd.Parameters.AddWithValue("ssubdivcode", strOfficeCode);
                    cmd.Parameters.AddWithValue("enumerationCase", objFieldEnum.staggedDTR);
                    cmd.Parameters.AddWithValue("locationtype", objFieldEnum.locationtype);
                    cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd.Parameters.Add("id", NpgsqlDbType.Text);
                    cmd.Parameters.Add("enumid", NpgsqlDbType.Text);
                    cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                    cmd.Parameters["id"].Direction = ParameterDirection.Output;
                    cmd.Parameters["enumid"].Direction = ParameterDirection.Output;
                    Arr[2] = "enumid";
                    Arr[1] = "id";
                    Arr[0] = "msg";
                    Arr = ObjCon.Execute(cmd, Arr, 3);
                    if (Arr[1] == "0")
                    {
                        objFieldEnum.sEnumDetailsID = Arr[2].ToString();
                    }

                    //strQry = "UPDATE \"TBLDTCENUMERATION\" SET \"DTE_LOCATION\"='" + objFieldEnum.locationtype + "' WHERE \"DTE_TC_CODE\"='" + objFieldEnum.sTcCode + "' ";
                    //ObjCon.ExecuteQry(strQry);
                    ////update for transcommission date
                    //if (objFieldEnum.sCommisionDate != null && objFieldEnum.sCommisionDate != "")
                    //{
                    //    strQry = "UPDATE \"TBLDTCENUMERATION\" SET \"DTE_TRANS_COMMISION_DATE\"=TO_DATE('" + objFieldEnum.sCommisionDate + "','dd/MM/yyyy'),\"DTE_DTR_COMMISION_DATE\"=TO_DATE('" + objFieldEnum.sDtrcommissiondate + "','dd/MM/yyyy') WHERE \"DTE_TC_CODE\"='" + objFieldEnum.sTcCode + "' ";
                    //    ObjCon.ExecuteQry(strQry);
                    //}
                    //else
                    //{
                    //    strQry = "UPDATE \"TBLDTCENUMERATION\" SET \"DTE_TRANS_COMMISION_DATE\"=null WHERE \"DTE_TC_CODE\"='" + objFieldEnum.sTcCode + "' ";
                    //    ObjCon.ExecuteQry(strQry);
                    //}
                    ////update for last service date
                    //if (objFieldEnum.sLastServiceDate != null && objFieldEnum.sLastServiceDate != "")
                    //{
                    //    strQry = "UPDATE \"TBLDTCENUMERATION\" SET \"DTE_LAST_SERVICE_DATE\"=TO_DATE('" + objFieldEnum.sLastServiceDate + "','dd/MM/yyyy') WHERE \"DTE_TC_CODE\"='" + objFieldEnum.sTcCode + "' ";
                    //    ObjCon.ExecuteQry(strQry);
                    //}
                    //else
                    //{
                    //    strQry = "UPDATE \"TBLDTCENUMERATION\" SET \"DTE_LAST_SERVICE_DATE\"=null WHERE \"DTE_TC_CODE\"='" + objFieldEnum.sTcCode + "' ";
                    //    ObjCon.ExecuteQry(strQry);
                    //}
                    ////update for enum date
                    //if (objFieldEnum.sEnumDate != null && objFieldEnum.sEnumDate != "")
                    //{
                    //    strQry = "UPDATE \"TBLDTCENUMERATION\" SET \"DTE_ENUM_DATE\"=TO_DATE('" + objFieldEnum.sEnumDate + "','dd/MM/yyyy') WHERE \"DTE_TC_CODE\"='" + objFieldEnum.sTcCode + "' ";
                    //    ObjCon.ExecuteQry(strQry);
                    //}
                    //else
                    //{
                    //    strQry = "UPDATE \"TBLDTCENUMERATION\" SET \"DTE_ENUM_DATE\"=null WHERE \"DTE_TC_CODE\"='" + objFieldEnum.sTcCode + "' ";
                    //    ObjCon.ExecuteQry(strQry);
                    //}

                    //if (objFieldEnum.sDtrcommissiondate == null && objFieldEnum.sDtrcommissiondate == "")
                    //{
                    //    strQry = "UPDATE \"TBLDTCENUMERATION\" SET \"DTE_DTR_COMMISION_DATE\"=null WHERE \"DTE_TC_CODE\"='" + objFieldEnum.sTcCode + "' ";
                    //    ObjCon.ExecuteQry(strQry);
                    //}

                    ////strQry = "UPDATE \"TBLDTCENUMERATION\" SET \"DTE_TRANS_COMMISION_DATE\" = '', \"DTE_LAST_SERVICE_DATE\" = '',\"DTE_ENUM_DATE\" = '' WHERE \"DTE_TC_CODE\" = '"+ objFieldEnum.sTcCode + "'";
                    ////ObjCon.ExecuteQry(strQry);


                    //// objFieldEnum.sWeldDate

                    //if ((objFieldEnum.staggedDTR == "2" || objFieldEnum.staggedDTR == "1") && (objFieldEnum.sEnumType == "2"))
                    //{
                    //    strQry = "SELECT \"TG_ID\" FROM \"TBLTAGGEDDTR\" WHERE \"TG_DTE_DTCCODE\" = '" + objFieldEnum.sDTCCode + "' AND \"TG_DTE_TC_CODE\" = '" + objFieldEnum.sTcCode + "' ";
                    //    if (ObjCon.get_value(strQry) == "")
                    //    {


                    //        string staggedDTRId = Convert.ToString(ObjCon.Get_max_no("TG_ID", "TBLTAGGEDDTR"));
                    //        strQry = "INSERT into \"TBLTAGGEDDTR\" (\"TG_ID\", \"TG_ED_ID\" , \"TG_ED_OFFICECODE\" ,  \"TG_ED_LOCTYPE\", \"TG_ED_LOCADDRESS\" , \"TG_ED_LOCNAME\" , ";
                    //        strQry += "\"TG_ED_STATUS_FLAG\", \"TG_ED_CRBY\" ,\"TG_ED_CRON\", \"TG_DTE_TC_CODE\",\"TG_TC_MAKE\", \"TG_DTE_TC_SLNO\" , \"TG_DTE_CAPACITY\" , \"TG_DTE_TC_TYPE\" , \"TG_DTE_TC_WEIGHT\" ,";
                    //        strQry += " \"TG_DTE_NAME\" , \"TG_DTE_DTCCODE\"  , \"TG_DTE_TOTAL_CON_KW\"  , \"TG_DTE_TOTAL_CON_HP\" , \"TG_DTE_KWH_READING\" , \"TG_DTE_TRANS_COMMISION_DATE\" ,";
                    //        strQry += "\"TG_DTE_LAST_SERVICE_DATE\" , \"TG_DTE_PLATFORM\" , \"TG_DTE_BREAKER_TYPE\" , \"TG_DTE_INTERNAL_CODE\" , \"TG_DTE_DTCMETERS\" , \"TG_DTE_HT_PROTECT\" ,";
                    //        strQry += "\"TG_DTE_LT_PROTECT\" , \"TG_DTE_GROUNDING\" ,  \"TG_DTE_ARRESTERS\" , \"TG_DTE_LOADTYPE\" , \"TG_DTE_PROJECTTYPE\" , \"TG_DTE_HT_LINE\" , \"TG_DTE_LT_LINE\" ,";
                    //        strQry += " \"TG_DTE_LONGITUDE\" , \"TG_DTE_LATITUDE\" ";
                    //        strQry += "  ) ";
                    //        strQry += " VALUES (" + staggedDTRId + " , '" + objFieldEnum.sEnumDetailsID + "' ," + objFieldEnum.sOfficeCode + " , 1 , '" + objFieldEnum.sLocName + "' , ";
                    //        strQry += " '" + objFieldEnum.sLocAddress + "' ,0 , " + objFieldEnum.sCrBy + ", now() , " + objFieldEnum.sTcCode + " ," + objFieldEnum.sTCMake + " ,'" + objFieldEnum.sTCSlno + "', ";
                    //        strQry += "'" + objFieldEnum.sTCCapacity + "'  , '" + objFieldEnum.sTCType + "' , '" + objFieldEnum.sTCWeight + "' ,";
                    //        strQry += "'" + objFieldEnum.sDTCName + "' , '" + objFieldEnum.sDTCCode + "','" + objFieldEnum.sConnectedKW + "' , '" + objFieldEnum.sConnectedHP + "','" + objFieldEnum.sKWHReading + "' , TO_DATE('" + objFieldEnum.sCommisionDate + "', 'dd/mm/yyyy'),";
                    //        strQry += "  TO_DATE('" + objFieldEnum.sLastServiceDate + "', 'dd/mm/yyyy') , '" + objFieldEnum.sPlatformType + "' , '" + objFieldEnum.sBreakertype + "' , '" + objFieldEnum.sInternalCode + "', '" + objFieldEnum.sDTCMeters + "' , '" + objFieldEnum.sHTProtect + "' ,  ";
                    //        strQry += " '" + objFieldEnum.sLTProtect + "' , '" + objFieldEnum.sGrounding + "' , '" + objFieldEnum.sArresters + "' , '" + objFieldEnum.sLoadtype + "' ,  '" + objFieldEnum.sProjecttype + "' , '" + objFieldEnum.sHTlinelength + "' , '" + objFieldEnum.sLTlinelength + "' ,  ";
                    //        strQry += "  '" + objFieldEnum.sLongitude + "' , '" + objFieldEnum.sLatitude + "' ";
                    //        strQry += ") ";
                    //        ObjCon.ExecuteQry(strQry);
                    //    }

                    //}



                    NpgsqlCommand cmd1 = new NpgsqlCommand("proc_update_field_enumeration_details");
                    cmd1.Parameters.AddWithValue("location_type", Convert.ToString(objFieldEnum.locationtype ?? ""));
                    cmd1.Parameters.AddWithValue("tc_code", Convert.ToString(objFieldEnum.sTcCode ?? ""));
                    cmd1.Parameters.AddWithValue("commission_date", Convert.ToString(objFieldEnum.sCommisionDate ?? ""));
                    cmd1.Parameters.AddWithValue("dtr_commission_date", Convert.ToString(objFieldEnum.sDtrcommissiondate ?? ""));
                    cmd1.Parameters.AddWithValue("last_service_date", Convert.ToString(objFieldEnum.sLastServiceDate ?? ""));
                    cmd1.Parameters.AddWithValue("enum_date", Convert.ToString(objFieldEnum.sEnumDate ?? ""));
                    cmd1.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd1.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd1.Parameters["msg"].Direction = ParameterDirection.Output;
                    cmd1.Parameters["op_id"].Direction = ParameterDirection.Output;
                    Arr1[0] = "msg";
                    Arr1[1] = "op_id";
                    Arr1 = ObjCon.Execute(cmd1, Arr1, 2);

                    //UpdateApproverpriority(objFieldEnum.sUserType, objFieldEnum.sEnumDetailsID, objFieldEnum.sCrBy);

                    return Arr;
                }
                catch (Exception ex)
                {

                    clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                    return Arr;
                }
                #region

                //strQry = "SELECT \"TCP_TC_CODE\" FROM \"TBLTCPLATEALLOCATION\" WHERE \"TCP_TC_CODE\"='" + objFieldEnum.sTcCode + "'";
                //string Tc_Code = ObjCon.get_value(strQry);

                //if (Tc_Code == "")
                //{
                //    Arr[0] = "Plate Number " + objFieldEnum.sTcCode + "  Not Allocated to Any Vendor";
                //    Arr[1] = "2";
                //    return Arr;
                //}

                //strQry = "SELECT \"VM_ID\" || '~' ||\"VM_NAME\" FROM \"TBLTCPLATEALLOCATIONMASTER\",\"TBLTCPLATEALLOCATION\",\"TBLVENDORMASTER\" WHERE \"TCPM_ID\"=\"TCP_TCPM_ID\" AND ";
                //strQry += " \"VM_ID\" =\"TCPM_VENDOR_ID\" AND \"TCP_TC_CODE\"='" + objFieldEnum.sTcCode + "'";
                //string sVmDetails = ObjCon.get_value(strQry);

                //strQry = "SELECT \"IU_VENDOR_ID\" FROM \"TBLINTERNALUSERS\" WHERE \"IU_ID\"='" + objFieldEnum.sOperator1 + "'";
                //string sLogUs_VM_Id = ObjCon.get_value(strQry);

                //if (sLogUs_VM_Id == sVmDetails.Split('~').GetValue(0).ToString())
                //{
                //    strQry = "SELECT * FROM \"TBLTCPLATEALLOCATIONMASTER\",\"TBLTCPLATEALLOCATION\" WHERE \"TCPM_ID\"=\"TCP_TCPM_ID\" AND \"TCP_STATUS_FLAG\" IN (0,4) AND ";
                //    strQry += " \"TCP_TC_CODE\" ='" + objFieldEnum.sTcCode + "'";
                //    string res = ObjCon.get_value(strQry);

                //    if (res == "")
                //    {
                //        Arr[0] = "Plate Number " + objFieldEnum.sTcCode + "  Not Possible to save because entered tc code may be Approved, deleted or Rejected ";
                //        Arr[1] = "2";
                //        return Arr;
                //    }
                //}
                //else
                //{
                //    Arr[0] = "Plate Number " + objFieldEnum.sTcCode + "  Already allocated to vendor " + sVmDetails.Split('~').GetValue(1).ToString();
                //    Arr[1] = "2";
                //    return Arr;
                //}

                //if (objFieldEnum.sOfficeCode.Length >= Section_code)
                //{
                //    strOfficeCode = objFieldEnum.sOfficeCode.Substring(0, SubDiv_code);
                //}

                //strQry = "select \"FD_FEEDER_ID\" from \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" WHERE \"FD_FEEDER_CODE\"='" + objFieldEnum.sDTCCode.ToString().Substring(0, Feeder_code) + "' ";
                //strQry += " AND  \"FD_FEEDER_ID\"=\"FDO_FEEDER_ID\" AND cast(\"FDO_OFFICE_CODE\" as text) LIKE '" + strOfficeCode + "%'";
                //sRes = ObjCon.get_value(strQry);
                //if (sRes == "")
                //{
                //    Arr[0] = "Code Does Not Match With The Feeder Code";
                //    Arr[1] = "2";
                //    return Arr;

                //}                

                ///// //If sEnumDetailsID="" then Insert else Update;
                //if (objFieldEnum.sEnumDetailsID == "")
                //{

                //    ObjCon.BeginTransaction();


                //    strQry = "SELECT \"DTE_TC_CODE\" FROM \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\" WHERE \"DTE_TC_CODE\"='" + objFieldEnum.sTcCode + "' AND \"ED_ID\"=\"DTE_ED_ID\" AND \"ED_STATUS_FLAG\" NOT IN ('3','5')";
                //    sRes = ObjCon.get_value(strQry);
                //    if (sRes != "")
                //    {
                //        Arr[0] = "Plate Number " + objFieldEnum.sTcCode + "  Already Exist";
                //        Arr[1] = "2";
                //        return Arr;
                //    }

                //    strQry = "SELECT \"DTE_DTCCODE\" FROM \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\" WHERE \"DTE_DTCCODE\"='" + objFieldEnum.sDTCCode + "' AND \"ED_ID\"=\"DTE_ED_ID\" AND \"ED_STATUS_FLAG\" NOT IN ('3','5')";
                //    sRes = ObjCon.get_value(strQry);
                //    if (sRes != "")
                //    {
                //        Arr[0] = "DTC Code(DTLMS) " + objFieldEnum.sDTCCode + "  Already Exist";
                //        Arr[1] = "2";
                //        return Arr;
                //    }

                //    if (objFieldEnum.sTCMake != "")
                //    {
                //        strQry = "SELECT * FROM \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\" WHERE \"DTE_MAKE\"='" + objFieldEnum.sTCMake + "' AND \"DTE_TC_SLNO\"='" + objFieldEnum.sTCSlno + "' AND \"ED_ID\"=\"DTE_ED_ID\" AND \"ED_STATUS_FLAG\" NOT IN ('3','5')";
                //        sRes = ObjCon.get_value(strQry);
                //        if (sRes != "")
                //        {
                //            Arr[0] = "Combination of Transformer Sl No " + objFieldEnum.sTCSlno + " and Make Name  " + objFieldEnum.sMakeName + " Already Exist";
                //            Arr[1] = "2";
                //            return Arr;
                //        }
                //    }

                //    // Save to Enumeration Details (Basic Information in Enumeration)
                //    objFieldEnum.sEnumDetailsID = Convert.ToString(ObjCon.Get_max_no("ED_ID", "TBLENUMERATIONDETAILS"));

                //    strQry = "INSERT INTO \"TBLENUMERATIONDETAILS\" (\"ED_ID\",\"ED_OFFICECODE\",\"ED_OPERATOR1\",\"ED_OPERATOR2\",";
                //    if (objFieldEnum.sWeldDate == null)
                //    {
                //        strQry += "\"ED_FEEDERCODE\",\"ED_ENUM_TYPE\",\"ED_LOCTYPE\",\"ED_CRBY\",\"ED_IP_ENUM_DONE\",\"ED_APPROVE_STATUS\") VALUES (";
                //    }
                //    else
                //    {
                //        strQry += "\"ED_WELD_DATE\",\"ED_FEEDERCODE\",\"ED_ENUM_TYPE\",\"ED_LOCTYPE\",\"ED_CRBY\",\"ED_IP_ENUM_DONE\",\"ED_APPROVE_STATUS\") VALUES (";
                //    }

                //    strQry += " '" + objFieldEnum.sEnumDetailsID + "','" + objFieldEnum.sOfficeCode + "','" + objFieldEnum.sOperator1 + "','" + objFieldEnum.sOperator2 + "',";

                //    if (objFieldEnum.sWeldDate == null)
                //    {
                //        strQry += " '" + objFieldEnum.sFeederCode + "','2','2','" + objFieldEnum.sCrBy + "','" + objFieldEnum.sIsIPEnumDone + "',";
                //    }
                //    else
                //    {
                //        strQry += " TO_DATE('" + objFieldEnum.sWeldDate + "','dd/MM/yyyy'),'" + objFieldEnum.sFeederCode + "','2','2','" + objFieldEnum.sCrBy + "','" + objFieldEnum.sIsIPEnumDone + "',";
                //    }

                //    if (objFieldEnum.sUserType == "1" || objFieldEnum.sUserType == "3" || objFieldEnum.sUserType == "5")
                //    {
                //        strQry += "'1')";
                //    }
                //    else if (objFieldEnum.sUserType == "6")
                //    {
                //        strQry += "'2')";
                //    }
                //    else if (objFieldEnum.sUserType == "2" || objFieldEnum.sUserType == "4")
                //    {
                //        strQry += "'3')";
                //    }

                //    ObjCon.ExecuteQry(strQry);

                //    //Save to DTC and TC Details
                //    objFieldEnum.sEnumDTCID = Convert.ToString(ObjCon.Get_max_no("DTE_ID", "TBLDTCENUMERATION"));                    


                //    if (objFieldEnum.bIsDTLMSDetails == true)
                //    {
                //        sIPCESCValue = "1";
                //    }
                //    if (objFieldEnum.bIsCESCDetails == true)
                //    {
                //        sIPCESCValue = "2";
                //    }
                //    if (objFieldEnum.bIsIPDetails == true)
                //    {
                //        sIPCESCValue = "3";
                //    }

                //    strQry = "INSERT INTO \"TBLDTCENUMERATION\"(\"DTE_ID\",\"DTE_ED_ID\",\"DTE_TC_CODE\",\"DTE_TC_SLNO\",\"DTE_MAKE\",\"DTE_CAPACITY\",";
                //    if (objFieldEnum.sTCManfDate == "")
                //    {
                //        strQry += "\"DTE_NAME\",\"DTE_DTCCODE\",\"DTE_CESCCODE\",\"DTE_IPCODE\",";
                //    }
                //    else
                //    {
                //        strQry += "\"DTE_TC_MANFDATE\",\"DTE_NAME\",\"DTE_DTCCODE\",\"DTE_CESCCODE\",\"DTE_IPCODE\",";
                //    }
                //    if (objFieldEnum.sEnumDate == "")
                //    {
                //        strQry += "\"DTE_TOTAL_CON_KW\",\"DTE_TOTAL_CON_HP\",\"DTE_KWH_READING\",";
                //    }
                //    else
                //    {
                //        strQry += "\"DTE_ENUM_DATE\",\"DTE_TOTAL_CON_KW\",\"DTE_TOTAL_CON_HP\",\"DTE_KWH_READING\",";
                //    }
                //    if (objFieldEnum.sCommisionDate == "")
                //    {
                //        strQry += " \"DTE_PLATFORM\",";
                //    }
                //    else
                //    {
                //        strQry += " \"DTE_TRANS_COMMISION_DATE\",\"DTE_PLATFORM\",";
                //    }
                //    if (objFieldEnum.sLastServiceDate == "")
                //    {
                //        strQry += "\"DTE_BREAKER_TYPE\",\"DTE_INTERNAL_CODE\",\"DTE_DTCMETERS\",\"DTE_HT_PROTECT\",\"DTE_LT_PROTECT\",\"DTE_GROUNDING\",";
                //    }
                //    else
                //    {
                //        strQry += " \"DTE_LAST_SERVICE_DATE\",\"DTE_BREAKER_TYPE\",\"DTE_INTERNAL_CODE\",\"DTE_DTCMETERS\",\"DTE_HT_PROTECT\",\"DTE_LT_PROTECT\",\"DTE_GROUNDING\",";
                //    }

                //    strQry += " \"DTE_ARRESTERS\",\"DTE_LOADTYPE\",\"DTE_PROJECTTYPE\",\"DTE_LT_LINE\",\"DTE_HT_LINE\",\"DTE_LONGITUDE\",\"DTE_LATITUDE\",\"DTE_DEPRECIATION\",\"DTE_CRBY\",\"DTE_ISIPCESC\",\"DTE_TANK_CAPACITY\",";
                //    strQry += " \"DTE_TC_WEIGHT\",\"DTE_INFOSYS_ASSET\",\"DTE_RATING\",\"DTE_STAR_RATE\") VALUES (";
                //    strQry += " '" + objFieldEnum.sEnumDTCID + "','" + objFieldEnum.sEnumDetailsID + "','" + objFieldEnum.sTcCode + "','" + objFieldEnum.sTCSlno + "',";
                //    strQry += " '" + objFieldEnum.sTCMake + "','" + objFieldEnum.sTCCapacity + "',";
                //    if (objFieldEnum.sTCManfDate == "")
                //    {
                //        strQry += " '" + objFieldEnum.sDTCName.ToUpper() + "','" + objFieldEnum.sDTCCode + "','" + objFieldEnum.sOldDTCCode + "','" + objFieldEnum.sIPDTCCode + "',";
                //    }
                //    else
                //    {
                //        strQry += "TO_DATE('" + objFieldEnum.sTCManfDate + "','dd/MM/yyyy'),'" + objFieldEnum.sDTCName + "','" + objFieldEnum.sDTCCode + "','" + objFieldEnum.sOldDTCCode + "','" + objFieldEnum.sIPDTCCode + "',";
                //    }
                //    if (objFieldEnum.sEnumDate == "")
                //    {
                //        strQry += "'" + objFieldEnum.sConnectedKW + "','" + objFieldEnum.sConnectedHP + "','" + objFieldEnum.sKWHReading + "',";
                //    }
                //    else
                //    {
                //        strQry += " TO_DATE('" + objFieldEnum.sEnumDate + "','dd/MM/yyyy'),'" + objFieldEnum.sConnectedKW + "','" + objFieldEnum.sConnectedHP + "','" + objFieldEnum.sKWHReading + "',";
                //    }
                //    if (objFieldEnum.sCommisionDate == "")
                //    {
                //        strQry += "'" + objFieldEnum.sPlatformType + "',";
                //    }
                //    else
                //    {
                //        strQry += " TO_DATE('" + objFieldEnum.sCommisionDate + "','dd/MM/yyyy'),'" + objFieldEnum.sPlatformType + "',";
                //    }
                //    if (objFieldEnum.sLastServiceDate == "")
                //    {
                //        strQry += " '" + objFieldEnum.sBreakertype + "','" + objFieldEnum.sInternalCode + "','" + objFieldEnum.sDTCMeters + "','" + objFieldEnum.sHTProtect + "',";
                //    }
                //    else
                //    {
                //        strQry += " TO_DATE('" + objFieldEnum.sLastServiceDate + "','dd/MM/yyyy'),'" + objFieldEnum.sBreakertype + "','" + objFieldEnum.sInternalCode + "','" + objFieldEnum.sDTCMeters + "','" + objFieldEnum.sHTProtect + "',";
                //    }
                //    strQry += " '" + objFieldEnum.sLTProtect + "','" + objFieldEnum.sGrounding + "','" + objFieldEnum.sArresters + "',";
                //    strQry += " '" + objFieldEnum.sLoadtype + "','" + objFieldEnum.sProjecttype + "','" + objFieldEnum.sLTlinelength + "','" + objFieldEnum.sHTlinelength + "','" + objFieldEnum.sLongitude + "',";
                //    strQry += " '" + objFieldEnum.sLatitude + "','" + objFieldEnum.sDepreciation + "','" + objFieldEnum.sCrBy + "','" + sIPCESCValue + "',";
                //    strQry += " '" + objFieldEnum.sTankCapacity + "','" + objFieldEnum.sTCWeight + "','" + objFieldEnum.sInfosysAsset + "','" + objFieldEnum.sRating + "','" + objFieldEnum.sStarRate + "')";
                //    ObjCon.ExecuteQry(strQry);

                //    ObjCon.CommitTransaction();

                //    Arr[0] = "Enumeration Details Saved Successfully";
                //    Arr[1] = "0";

                //    return Arr;
                //}
                //else
                //{
                //    ObjCon.BeginTransaction();

                //    strQry = "SELECT \"DTE_TC_CODE\" FROM \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\" WHERE \"DTE_TC_CODE\"='" + objFieldEnum.sTcCode + "' AND \"DTE_ED_ID\" <>'" + objFieldEnum.sEnumDetailsID + "' AND \"ED_ID\"=\"DTE_ED_ID\" AND \"ED_STATUS_FLAG\" NOT IN ('3','5')";
                //    sRes = ObjCon.get_value(strQry);
                //    if (sRes != "")
                //    {
                //        Arr[0] = "Plate Number " + objFieldEnum.sTcCode + "  Already Exist";
                //        Arr[1] = "2";
                //        return Arr;
                //    }

                //    strQry = "SELECT \"DTE_DTCCODE\" FROM \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\" WHERE \"DTE_DTCCODE\"='" + objFieldEnum.sDTCCode + "' AND \"DTE_ED_ID\" <>'" + objFieldEnum.sEnumDetailsID + "' AND \"ED_ID\"=\"DTE_ED_ID\" AND \"ED_STATUS_FLAG\" NOT IN ('3','5')";
                //    sRes = ObjCon.get_value(strQry);
                //    if (sRes != "")
                //    {
                //        Arr[0] = "DTC Code(DTLMS) " + objFieldEnum.sDTCCode + "  Already Exist";
                //        Arr[1] = "2";
                //        return Arr;
                //    }

                //    strQry = "SELECT \"DTE_TC_SLNO\" FROM \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\" WHERE \"DTE_MAKE\"='" + objFieldEnum.sTCMake + "' AND \"DTE_TC_SLNO\"='" + objFieldEnum.sTCSlno + "' AND \"DTE_ED_ID\" <>'" + objFieldEnum.sEnumDetailsID + "' AND \"ED_ID\"=\"DTE_ED_ID\" AND \"ED_STATUS_FLAG\" NOT IN ('3','5')";
                //    sRes = ObjCon.get_value(strQry);
                //    if (sRes != "")
                //    {
                //        Arr[0] = "Combination of Transformer Sl No " + objFieldEnum.sTCSlno + " and Make Name  " + objFieldEnum.sMakeName + " Already Exist";
                //        Arr[1] = "2";
                //        return Arr;
                //    }

                //    strQry = "UPDATE \"TBLENUMERATIONDETAILS\" SET \"ED_OFFICECODE\"='" + objFieldEnum.sOfficeCode + "',\"ED_OPERATOR1\"='" + objFieldEnum.sOperator1 + "',\"ED_OPERATOR2\"='" + objFieldEnum.sOperator2 + "'";
                //    if (objFieldEnum.sWeldDate == "")
                //    {
                //        strQry += ",\"ED_FEEDERCODE\"='" + objFieldEnum.sFeederCode + "',  ";
                //    }
                //    else
                //    {
                //        strQry += " ,\"ED_WELD_DATE\"=TO_DATE('" + objFieldEnum.sWeldDate + "','dd/MM/yyyy'),\"ED_FEEDERCODE\"='" + objFieldEnum.sFeederCode + "',  ";
                //    }

                //    if (objFieldEnum.sUserType == "1" || objFieldEnum.sUserType == "3" || objFieldEnum.sUserType == "5")
                //    {
                //        strQry += " \"ED_UPDATE_BY\"='" + objFieldEnum.sCrBy + "', \"ED_UPDATE_ON\"=NOW(), ";
                //    }

                //    strQry += " \"ED_IP_ENUM_DONE\"='" + objFieldEnum.sIsIPEnumDone + "', ";

                //    //if(objFieldEnum.sUserType == "1"|| objFieldEnum.sUserType == "3" || objFieldEnum.sUserType == "5")
                //    //{
                //    //    strQry += "\"ED_APPROVE_STATUS\" = '1'";
                //    //}
                //    //else if(objFieldEnum.sUserType == "6")
                //    //{
                //    //    strQry += "\"ED_APPROVE_STATUS\" = '2'";
                //    //}
                //    //else if (objFieldEnum.sUserType == "2" || objFieldEnum.sUserType == "4")
                //    //{
                //    //    strQry += "\"ED_APPROVE_STATUS\" = '3'";
                //    //}
                //    clsPreQCApproval objpreQC = new clsPreQCApproval();
                //    string sLevel =  string.Empty;
                //    sLevel = objpreQC.GetNextApprovalLevel(objFieldEnum.sUserType);
                //    strQry += "\"ED_APPROVALPRIORITY\" = '"+ sLevel + "'";


                //    strQry += " WHERE \"ED_ID\"='" + objFieldEnum.sEnumDetailsID + "'";
                //    ObjCon.ExecuteQry(strQry);

                //    if (objFieldEnum.sUserType == "6" || objFieldEnum.sUserType == "7" || objFieldEnum.sUserType == "8")
                //    {
                //        string Qry = "INSERT INTO \"TBLINTERNALAPPROVALHISTORY\" (\"IA_ED_ID\",\"IA_ED_APPROVED_BY\",\"IA_ED_APPROVED_TYPE\") ";
                //        Qry += " VALUES('" + objFieldEnum.sEnumDetailsID + "','" + objFieldEnum.sCrBy + "','0')";
                //        ObjCon.ExecuteQry(Qry);
                //    }

                //    if (objFieldEnum.sProjecttype == "" || objFieldEnum.sProjecttype == null)
                //    {
                //        objFieldEnum.sProjecttype = "0";
                //    }

                //    strQry = "UPDATE \"TBLDTCENUMERATION\" SET \"DTE_TC_CODE\"='" + objFieldEnum.sTcCode + "',\"DTE_TC_SLNO\"='" + objFieldEnum.sTCSlno + "',\"DTE_MAKE\"='" + objFieldEnum.sTCMake + "'";
                //    strQry += " ,\"DTE_CAPACITY\"='" + objFieldEnum.sTCCapacity + "',";
                //    if (objFieldEnum.sTCManfDate == "")
                //    {
                //        strQry += " \"DTE_NAME\"='" + objFieldEnum.sDTCName.Replace("'", "''").ToUpper() + "',\"DTE_DTCCODE\"='" + objFieldEnum.sDTCCode + "',\"DTE_CESCCODE\"='" + objFieldEnum.sOldDTCCode + "',\"DTE_IPCODE\"='" + objFieldEnum.sIPDTCCode + "',";
                //    }
                //    else
                //    {
                //        strQry += " \"DTE_TC_MANFDATE\"=TO_DATE('" + objFieldEnum.sTCManfDate + "','dd/MM/yyyy'),\"DTE_NAME\"='" + objFieldEnum.sDTCName.Replace("'", "''") + "',\"DTE_DTCCODE\"='" + objFieldEnum.sDTCCode + "',\"DTE_CESCCODE\"='" + objFieldEnum.sOldDTCCode + "',\"DTE_IPCODE\"='" + objFieldEnum.sIPDTCCode + "',";
                //    }
                //    if(objFieldEnum.sEnumDate == "")
                //    {
                //        strQry += "\"DTE_TOTAL_CON_KW\"='" + objFieldEnum.sConnectedKW + "',\"DTE_TOTAL_CON_HP\"='" + objFieldEnum.sConnectedHP + "',";
                //    }
                //    else
                //    {
                //        strQry += " \"DTE_ENUM_DATE\"=TO_DATE('" + objFieldEnum.sEnumDate + "','dd/MM/yyyy'),\"DTE_TOTAL_CON_KW\"='" + objFieldEnum.sConnectedKW + "',\"DTE_TOTAL_CON_HP\"='" + objFieldEnum.sConnectedHP + "',";
                //    }
                //    if(objFieldEnum.sCommisionDate == "")
                //    {
                //        strQry += " \"DTE_KWH_READING\"='" + objFieldEnum.sKWHReading + "',";
                //    }
                //    else
                //    {
                //        strQry += " \"DTE_KWH_READING\"='" + objFieldEnum.sKWHReading + "',\"DTE_TRANS_COMMISION_DATE\"=TO_DATE('" + objFieldEnum.sCommisionDate + "','dd/MM/yyyy'),";
                //    }
                //    if(objFieldEnum.sLastServiceDate == "")
                //    {
                //        strQry += "\"DTE_PLATFORM\"='" + objFieldEnum.sPlatformType + "',\"DTE_BREAKER_TYPE\"='" + objFieldEnum.sBreakertype + "',";
                //    }
                //    else
                //    {
                //        strQry += " \"DTE_LAST_SERVICE_DATE\"=TO_DATE('" + objFieldEnum.sLastServiceDate + "','dd/MM/yyyy'),\"DTE_PLATFORM\"='" + objFieldEnum.sPlatformType + "',\"DTE_BREAKER_TYPE\"='" + objFieldEnum.sBreakertype + "',";
                //    }

                //    strQry += " \"DTE_INTERNAL_CODE\"='" + objFieldEnum.sInternalCode + "',\"DTE_DTCMETERS\"='" + objFieldEnum.sDTCMeters + "',\"DTE_HT_PROTECT\"='" + objFieldEnum.sHTProtect + "',\"DTE_LT_PROTECT\"='" + objFieldEnum.sLTProtect + "'";
                //    strQry += " ,\"DTE_GROUNDING\"='" + objFieldEnum.sGrounding + "',";
                //    strQry += " \"DTE_ARRESTERS\"='" + objFieldEnum.sArresters + "',\"DTE_LOADTYPE\"='" + objFieldEnum.sLoadtype + "',\"DTE_PROJECTTYPE\"='" + objFieldEnum.sProjecttype + "',";
                //    strQry += " \"DTE_LT_LINE\"='" + objFieldEnum.sLTlinelength + "',\"DTE_HT_LINE\"='" + objFieldEnum.sHTlinelength + "',\"DTE_LONGITUDE\"='" + objFieldEnum.sLongitude + "',\"DTE_LATITUDE\"='" + objFieldEnum.sLatitude + "',";
                //    strQry += " \"DTE_DEPRECIATION\"='" + objFieldEnum.sDepreciation + "',\"DTE_TANK_CAPACITY\"='" + objFieldEnum.sTankCapacity + "',\"DTE_TC_WEIGHT\"='" + objFieldEnum.sTCWeight + "',\"DTE_INFOSYS_ASSET\"='" + objFieldEnum.sInfosysAsset + "',";
                //    strQry += " \"DTE_RATING\"='" + objFieldEnum.sRating + "',\"DTE_STAR_RATE\"='" + objFieldEnum.sStarRate + "' WHERE \"DTE_ED_ID\"='" + objFieldEnum.sEnumDetailsID + "'";

                //    ObjCon.ExecuteQry(strQry);

                //    if (objFieldEnum.sStatus == "2")
                //    {
                //        strQry = "UPDATE \"TBLENUMERATIONDETAILS\" SET \"ED_STATUS_FLAG\"='0' WHERE \"ED_ID\"='" + objFieldEnum.sEnumDetailsID + "'";
                //        ObjCon.ExecuteQry(strQry);
                //    }

                //    string sQry = "SELECT \"ED_INITIAL_UPDATE_ON\" FROM \"TBLENUMERATIONDETAILS\" WHERE \"ED_ID\"='" + objFieldEnum.sEnumDetailsID + "'";
                //    string sLastUpdate = ObjCon.get_value(sQry);
                //    if(sLastUpdate.Length == 0)
                //    {
                //        sQry = "UPDATE \"TBLENUMERATIONDETAILS\" SET \"ED_INITIAL_UPDATE_BY\" = '"+ objFieldEnum.sCrBy + "',  \"ED_INITIAL_UPDATE_ON\" = NOW() WHERE \"ED_ID\"='" + objFieldEnum.sEnumDetailsID + "' ";
                //        ObjCon.ExecuteQry(sQry);
                //    }

                //    ObjCon.CommitTransaction();

                //    Arr[0] = "Enumeration Details Updated Successfully";
                //    Arr[1] = "1";

                //    return Arr;
                //}
                #endregion

            }
            catch (Exception ex)
            {
                ObjCon.RollBackTrans();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }

        // UnUsed Method

        //public DataTable LoadFieldEnumeration(string sOperator = "")
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
        //        string strQry = string.Empty;
        //        strQry = "SELECT \"ED_ID\",\"ED_OFFICECODE\",\"ED_FEEDERCODE\",TO_CHAR(\"ED_WELD_DATE\",'DD-MON-YYYY') \"ED_WELD_DATE\",\"DTE_TC_CODE\",(SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"DTE_MAKE\"=\"TM_ID\") \"MAKE\",";
        //        strQry += " \"DTE_CAPACITY\",DTE_DTCCODE,DTE_CESCCODE,DTE_IPCODE ";
        //        strQry += " FROM \"TBLENUMERATIONDETAILS\",\"TBLDTCENUMERATION\" WHERE \"ED_ID\"=\"DTE_ED_ID\" AND \"ED_CANCEL_FLAG\"='0' AND \"ED_ENUM_TYPE\"='2' ";
        //        if (sOperator != "")
        //        {
        //            strQry += " AND  (\"ED_OPERATOR1\"='" + sOperator + "' OR \"ED_OPERATOR2\"='" + sOperator + "')";
        //        }

        //        return ObjCon.FetchDataTable(strQry);
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return dt;
        //    }
        //}


        // UnUsed Method

        //public bool DeleteEnumerationDetails(clsFieldEnumeration objFieldEnum)
        //{

        //    try
        //    {
        //        PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
        //        string strQry = string.Empty;
        //        strQry = "UPDATE \"TBLENUMERATIONDETAILS\" SET \"ED_STATUS_FLAG\"='5',\"ED_NOTES\"='DELETE FROM ENUMERATION FORM',\"ED_NOTES_ON\"=now() ";
        //        strQry += " WHERE \"ED_ID\" ='" + objFieldEnum.sEnumDetailsID + "'";
        //        ObjCon.ExecuteQry(strQry);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return false;
        //    }
        //}


        // UnUsed Method
        //public bool SaveImagePathDetails(clsFieldEnumeration objFieldEnum)
        //{
        //    try
        //    {
        //        PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
        //        string strQry = string.Empty;

        //        string sMaxNo = Convert.ToString(ObjCon.Get_max_no("EP_ID", "TBLENUMERATIONPHOTOS"));

        //        strQry = "INSERT INTO \"TBLENUMERATIONPHOTOS\" (\"EP_ID\",\"EP_ED_ID\",\"EP_NAMEPLATE_PATH\",\"EP_SSPLATE_PATH\",\"EP_OLDDTC_PATH\",\"EP_DTLMSDTC_PATH\",";
        //        strQry += " \"EP_IPENUMDTC_PATH\",\"EP_INFOSYSDTC_PATH\",\"EP_DTC_PATH\",\"EP_CRBY\") VALUES ('" + sMaxNo + "','" + objFieldEnum.sEnumDetailsID + "','" + objFieldEnum.sNamePlatePhotoPath + "',";
        //        strQry += " '" + objFieldEnum.sSSPlatePhotoPath + "','" + objFieldEnum.sOldCodePhotoPath + "','" + objFieldEnum.sDTLMSCodePhotoPath + "',";
        //        strQry += " '" + objFieldEnum.sIPEnumCodePhotoPath + "','" + objFieldEnum.sInfosysCodePhotoPath + "','" + objFieldEnum.sDTCPhotoPath + "','" + objFieldEnum.sCrBy + "')";
        //        ObjCon.ExecuteQry(strQry);
        //        return true;

        //    }
        //    catch (Exception ex)
        //    {
        //        //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveImagePathDetails");
        //        clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return false;
        //    }
        //}


        public bool UpdateImagePathDetails(clsFieldEnumeration objFieldEnum)
        {
            try
            {
                PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
                string[] Arr = new string[3];

                string strQry = string.Empty;

                strQry = "SELECT \"EP_ED_ID\" FROM \"TBLENUMERATIONPHOTOS\" WHERE \"EP_ED_ID\"='" + objFieldEnum.sEnumDetailsID + "'";
                string sResult = ObjCon.get_value(strQry);
                if (sResult != "")
                {
                    //strQry = "UPDATE \"TBLENUMERATIONPHOTOS\" SET \"EP_NAMEPLATE_PATH\"='" + objFieldEnum.sNamePlatePhotoPath + "',\"EP_SSPLATE_PATH\"='" + objFieldEnum.sSSPlatePhotoPath + "',";
                    //strQry += " \"EP_OLDDTC_PATH\"='" + objFieldEnum.sOldCodePhotoPath + "',\"EP_DTLMSDTC_PATH\"='" + objFieldEnum.sDTLMSCodePhotoPath + "',";
                    //strQry += " \"EP_IPENUMDTC_PATH\"='" + objFieldEnum.sIPEnumCodePhotoPath + "',\"EP_INFOSYSDTC_PATH\"='" + objFieldEnum.sInfosysCodePhotoPath + "',";
                    //strQry += " \"EP_DTC_PATH\"='" + objFieldEnum.sDTCPhotoPath + "' WHERE \"EP_ED_ID\"='" + objFieldEnum.sEnumDetailsID + "' ";
                    //ObjCon.ExecuteQry(strQry);

                    //sp
                    NpgsqlCommand cmd = new NpgsqlCommand("proc_update_image_path_details");
                    cmd.Parameters.AddWithValue("name_plate_photo_path", (objFieldEnum.sNamePlatePhotoPath ?? ""));
                    cmd.Parameters.AddWithValue("ss_plate_photo_path", (objFieldEnum.sSSPlatePhotoPath ?? ""));
                    cmd.Parameters.AddWithValue("old_code_photo_path", (objFieldEnum.sOldCodePhotoPath ?? ""));
                    cmd.Parameters.AddWithValue("dtlms_code_photo_path", (objFieldEnum.sDTLMSCodePhotoPath ?? ""));
                    cmd.Parameters.AddWithValue("ip_enum_code_photo_path", (objFieldEnum.sIPEnumCodePhotoPath ?? ""));
                    cmd.Parameters.AddWithValue("infosys_code_photo_path", (objFieldEnum.sInfosysCodePhotoPath ?? ""));
                    cmd.Parameters.AddWithValue("dtc_photo_path", (objFieldEnum.sDTCPhotoPath ?? ""));
                    cmd.Parameters.AddWithValue("enum_details_id", (objFieldEnum.sEnumDetailsID ?? ""));

                    cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd.Parameters.Add("pk_id", NpgsqlDbType.Text);
                    cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                    cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                    cmd.Parameters["pk_id"].Direction = ParameterDirection.Output;

                    Arr[0] = "msg";
                    Arr[1] = "op_id";
                    Arr[2] = "pk_id";
                    Arr = ObjCon.Execute(cmd, Arr, 3);

                }
                else
                {
                    //SaveImagePathDetails(objFieldEnum);
                }


                return true;

            }
            catch (Exception ex)
            {
                //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "UpdateImagePathDetails");
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }


        public object GetEnumerationDetails(clsFieldEnumeration objField)
        {
            try
            {
                string strQry = string.Empty;
                PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
                DataTable dt = new DataTable();

                //strQry = " SELECT \"ED_ID\",\"ED_OFFICECODE\",\"ED_OPERATOR1\",\"DTE_NAME\",\"ED_OPERATOR2\",\"DTE_DTCCODE\",\"DTE_CESCCODE\",\"DTE_DTC_OLDCODE\",To_char(\"ED_WELD_DATE\",'dd/mm/yyyy') \"ED_WELD_DATE\",\"ED_FEEDERCODE\",\"ED_LOCTYPE\",\"ED_LOCNAME\",\"ED_LOCADDRESS\",\"ED_ENUM_TYPE\",\"ED_CRON\",\"ED_CRBY\", \"ED_APPROVE_STATUS\",\"ED_THROUGH\", \"ED_ENUMERATION_CASE\", ";
                //strQry += " \"DTE_ID\",\"DTE_ED_ID\",\"DTE_TC_CODE\",\"DTE_TC_SLNO\",\"DTE_MAKE\",\"DTE_CAPACITY\",\"DTE_TC_TYPE\",TO_CHAR(\"DTE_TC_MANFDATE\",'dd/MM/yyyy') AS \"DTE_TC_MANFDATE\",\"DTE_INTERNAL_CODE\", CAST(\"DTE_KWH_READING\" AS text) \"DTE_KWH_READING\", CAST(\"DTE_TOTAL_CON_HP\" AS text) \"DTE_TOTAL_CON_HP\", ";
                //strQry += " CAST(\"DTE_TOTAL_CON_KW\" AS text)\"DTE_TOTAL_CON_KW\",To_char(\"DTE_TRANS_COMMISION_DATE\",'dd/mm/yyyy') \"DTE_TRANS_COMMISION_DATE\", To_char(\"DTE_DTR_COMMISION_DATE\",'dd/mm/yyyy') \"DTE_DTR_COMMISION_DATE\",To_char(\"DTE_LAST_SERVICE_DATE\",'dd/mm/yyyy') \"DTE_LAST_SERVICE_DATE\",\"DTE_PLATFORM\",\"DTE_BREAKER_TYPE\",\"DTE_DTCMETERS\",\"DTE_HT_PROTECT\", ";
                //strQry += " \"DTE_LT_PROTECT\",\"DTE_GROUNDING\",\"DTE_ARRESTERS\",\"DTE_LOADTYPE\",\"DTE_PROJECTTYPE\",\"DTE_LT_LINE\",\"DTE_HT_LINE\",\"DTE_DEPRECIATION\",\"DTE_LATITUDE\",\"DTE_LONGITUDE\"  ,\"DTE_IPCODE\",TO_CHAR(\"DTE_ENUM_DATE\",'dd/MM/yyyy') \"DTE_ENUM_DATE\",\"ED_OPERATOR1\",\"ED_OPERATOR2\",\"DTE_CRON\",\"DTE_ISIPCESC\",  ";
                //strQry += " \"DTE_TANK_CAPACITY\",\"DTE_TC_WEIGHT\",\"DTE_INFOSYS_ASSET\",\"EP_NAMEPLATE_PATH\",\"EP_SSPLATE_PATH\",\"EP_OLDDTC_PATH\",\"EP_DTLMSDTC_PATH\",\"EP_IPENUMDTC_PATH\",\"EP_INFOSYSDTC_PATH\",\"EP_DTC_PATH\",\"DTE_RATING\",\"DTE_STAR_RATE\",\"ED_IP_ENUM_DONE\",\"ED_APPROVALPRIORITY\" ,\"DTE_WITHOUT_DTR\",\"DTE_LOCATION\",\"ED_IS_FEEDER_BIFURCATION\" ";
                //strQry += " FROM \"TBLENUMERATIONDETAILS\" INNER JOIN \"TBLDTCENUMERATION\" ON \"ED_ID\"=\"DTE_ED_ID\" INNER JOIN \"TBLENUMERATIONPHOTOS\" ON \"ED_ID\"=\"EP_ED_ID\" AND \"ED_ID\"='" + objField.sEnumDetailsID + "' ";

                //DataTable dt = new DataTable();
                //dt = ObjCon.FetchDataTable(strQry);


                //sp
                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_bind_enumeration_details");

                cmd.Parameters.AddWithValue("ed_id", objField.sEnumDetailsID == null ? "" : objField.sEnumDetailsID);

                dt = ObjCon.FetchDataTable(cmd);

                if (dt.Rows.Count > 0)
                {
                    objField.sEnumDetailsID = Convert.ToString(dt.Rows[0]["ED_ID"]);
                    objField.sOfficeCode = Convert.ToString(dt.Rows[0]["ED_OFFICECODE"]);
                    objField.sFeederCode = Convert.ToString(dt.Rows[0]["ED_FEEDERCODE"]);
                    objField.sWeldDate = Convert.ToString(dt.Rows[0]["ED_WELD_DATE"]);
                    objField.sOperator1 = Convert.ToString(dt.Rows[0]["ED_OPERATOR1"]);
                    objField.sOperator2 = Convert.ToString(dt.Rows[0]["ED_OPERATOR2"]);
                    objField.sThroughApp = Convert.ToString(dt.Rows[0]["ED_THROUGH"]);

                    objField.sEnumDTCID = Convert.ToString(dt.Rows[0]["DTE_ID"]);
                    objField.sTcCode = Convert.ToString(dt.Rows[0]["DTE_TC_CODE"]);
                    objField.sTCMake = Convert.ToString(dt.Rows[0]["DTE_MAKE"]);
                    objField.sTCSlno = Convert.ToString(dt.Rows[0]["DTE_TC_SLNO"]);
                    objField.sTCManfDate = Convert.ToString(dt.Rows[0]["DTE_TC_MANFDATE"]);
                    objField.sTCCapacity = Convert.ToString(dt.Rows[0]["DTE_CAPACITY"]);

                    objField.sDTCName = Convert.ToString(dt.Rows[0]["DTE_NAME"]).Replace("'", "");
                    objField.sDTCCode = Convert.ToString(dt.Rows[0]["DTE_DTCCODE"]);
                    objField.sOldDTCCode = Convert.ToString(dt.Rows[0]["DTE_CESCCODE"]);
                    objField.sIPDTCCode = Convert.ToString(dt.Rows[0]["DTE_IPCODE"]);
                    objField.sEnumDate = Convert.ToString(dt.Rows[0]["DTE_ENUM_DATE"]);
                    objField.sInfosysAsset = Convert.ToString(dt.Rows[0]["DTE_INFOSYS_ASSET"]);

                    objField.sInternalCode = Convert.ToString(dt.Rows[0]["DTE_INTERNAL_CODE"]);
                    objField.sKWHReading = Convert.ToString(dt.Rows[0]["DTE_KWH_READING"]);
                    objField.sConnectedHP = Convert.ToString(dt.Rows[0]["DTE_TOTAL_CON_HP"]);
                    objField.sConnectedKW = Convert.ToString(dt.Rows[0]["DTE_TOTAL_CON_KW"]);
                    objField.sCommisionDate = Convert.ToString(dt.Rows[0]["DTE_TRANS_COMMISION_DATE"]);
                    objField.sLastServiceDate = Convert.ToString(dt.Rows[0]["DTE_LAST_SERVICE_DATE"]);


                    objField.sPlatformType = Convert.ToString(dt.Rows[0]["DTE_PLATFORM"]);
                    objField.sBreakertype = Convert.ToString(dt.Rows[0]["DTE_BREAKER_TYPE"]);
                    objField.sDTCMeters = Convert.ToString(dt.Rows[0]["DTE_DTCMETERS"]);
                    objField.sHTProtect = Convert.ToString(dt.Rows[0]["DTE_HT_PROTECT"]);
                    objField.sLTProtect = Convert.ToString(dt.Rows[0]["DTE_LT_PROTECT"]);

                    objField.sGrounding = Convert.ToString(dt.Rows[0]["DTE_GROUNDING"]);
                    objField.sArresters = Convert.ToString(dt.Rows[0]["DTE_ARRESTERS"]);
                    objField.sLoadtype = Convert.ToString(dt.Rows[0]["DTE_LOADTYPE"]);
                    objField.sProjecttype = Convert.ToString(dt.Rows[0]["DTE_PROJECTTYPE"]);
                    objField.sLTlinelength = Convert.ToString(dt.Rows[0]["DTE_LT_LINE"]);
                    objField.sHTlinelength = Convert.ToString(dt.Rows[0]["DTE_HT_LINE"]);
                    objField.sDepreciation = Convert.ToString(dt.Rows[0]["DTE_DEPRECIATION"]);
                    objField.sLatitude = Convert.ToString(dt.Rows[0]["DTE_LATITUDE"]);
                    objField.sLongitude = Convert.ToString(dt.Rows[0]["DTE_LONGITUDE"]);

                    objField.sIPCESCValue = Convert.ToString(dt.Rows[0]["DTE_ISIPCESC"]);

                    objField.sTankCapacity = Convert.ToString(dt.Rows[0]["DTE_TANK_CAPACITY"]);
                    objField.sTCWeight = Convert.ToString(dt.Rows[0]["DTE_TC_WEIGHT"]);

                    objField.sRating = Convert.ToString(dt.Rows[0]["DTE_RATING"]);
                    objField.sStarRate = Convert.ToString(dt.Rows[0]["DTE_STAR_RATE"]);

                    objField.sNamePlatePhotoPath = Convert.ToString(dt.Rows[0]["EP_NAMEPLATE_PATH"]);
                    objField.sSSPlatePhotoPath = Convert.ToString(dt.Rows[0]["EP_SSPLATE_PATH"]);
                    objField.sOldCodePhotoPath = Convert.ToString(dt.Rows[0]["EP_OLDDTC_PATH"]);
                    objField.sDTLMSCodePhotoPath = Convert.ToString(dt.Rows[0]["EP_DTLMSDTC_PATH"]);
                    objField.sIPEnumCodePhotoPath = Convert.ToString(dt.Rows[0]["EP_IPENUMDTC_PATH"]);
                    objField.sInfosysCodePhotoPath = Convert.ToString(dt.Rows[0]["EP_INFOSYSDTC_PATH"]);
                    objField.sDTCPhotoPath = Convert.ToString(dt.Rows[0]["EP_DTC_PATH"]);


                    objField.sLocType = Convert.ToString(dt.Rows[0]["ED_LOCTYPE"]);
                    objField.sLocName = Convert.ToString(dt.Rows[0]["ED_LOCNAME"]);
                    objField.sLocAddress = Convert.ToString(dt.Rows[0]["ED_LOCADDRESS"]);
                    objField.sTCType = Convert.ToString(dt.Rows[0]["DTE_TC_TYPE"]);

                    objField.sEnumType = Convert.ToString(dt.Rows[0]["ED_ENUM_TYPE"]);

                    objField.sIsIPEnumDone = Convert.ToString(dt.Rows[0]["ED_IP_ENUM_DONE"]);
                    objField.sApproveStatus = Convert.ToString(dt.Rows[0]["ED_APPROVE_STATUS"]);
                    objField.sPriorityLevel = Convert.ToString(dt.Rows[0]["ED_APPROVALPRIORITY"]);

                    objField.staggedDTR = Convert.ToString(dt.Rows[0]["ED_ENUMERATION_CASE"]);
                    objField.sSpecialCase = Convert.ToString(dt.Rows[0]["ED_ENUMERATION_CASE"]);
                    objField.sDTCWithoutDTR = Convert.ToString(dt.Rows[0]["DTE_WITHOUT_DTR"]);
                    objField.sEDFeederBifurcation = Convert.ToString(dt.Rows[0]["ED_IS_FEEDER_BIFURCATION"]);
                    objField.sDTCOldDTCCode = Convert.ToString(dt.Rows[0]["DTE_DTC_OLDCODE"]);
                    objField.sLocation = Convert.ToString(dt.Rows[0]["DTE_LOCATION"]);
                    objField.sDtrcommissiondate = Convert.ToString(dt.Rows[0]["DTE_DTR_COMMISION_DATE"]);
                }
                return objField;
            }
            catch (Exception ex)
            {
                //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetDetails");
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objField;
            }
        }

        // UnUsed Method

        //public clsFieldEnumeration GetIPEnumerationData(clsFieldEnumeration objFieldEnum)
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        string strQry = string.Empty;
        //        PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
        //        strQry = "SELECT \"IP_LONGITUDE\",\"IP_LATITUDE\",\"IP_ARRESTERS\",\"IP_BREAKER_TYPE\",\"IP_HT_PROTECT\",\"IP_LT_PROTECT\",\"IP_DTCMETERS\",";
        //        strQry += " \"IP_GROUNDING\",\"IP_TOTAL_CON_KW\",\"IP_TOTAL_CON_HP\",\"IP_DTC_NAME\" FROM \"TBLIPENUMERATION\" WHERE \"IP_DTC_CODE\"='" + objFieldEnum.sIPDTCCode + "'";
        //        dt = ObjCon.FetchDataTable(strQry);
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            objFieldEnum.sLatitude = Convert.ToString(dt.Rows[i]["IP_LATITUDE"]);
        //            objFieldEnum.sLongitude = Convert.ToString(dt.Rows[i]["IP_LONGITUDE"]);
        //            objFieldEnum.sArresters = Convert.ToString(dt.Rows[i]["IP_ARRESTERS"]);
        //            objFieldEnum.sBreakertype = Convert.ToString(dt.Rows[i]["IP_BREAKER_TYPE"]);
        //            objFieldEnum.sHTProtect = Convert.ToString(dt.Rows[i]["IP_HT_PROTECT"]);
        //            objFieldEnum.sLTProtect = Convert.ToString(dt.Rows[i]["IP_LT_PROTECT"]);
        //            objFieldEnum.sDTCMeters = Convert.ToString(dt.Rows[i]["IP_DTCMETERS"]);
        //            objFieldEnum.sGrounding = Convert.ToString(dt.Rows[i]["IP_GROUNDING"]);
        //            objFieldEnum.sConnectedKW = Convert.ToString(dt.Rows[i]["IP_TOTAL_CON_KW"]);
        //            objFieldEnum.sConnectedHP = Convert.ToString(dt.Rows[i]["IP_TOTAL_CON_HP"]);
        //            objFieldEnum.sDTCName = Convert.ToString(dt.Rows[i]["IP_DTC_NAME"]);
        //        }
        //        return objFieldEnum;
        //    }
        //    catch (Exception ex)
        //    {
        //        //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetIPEnumerationData");
        //        clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return objFieldEnum;
        //    }
        //}


        //UnUsed Method

        //public clsFieldEnumeration GetCESCOldData(clsFieldEnumeration objFieldEnum)
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
        //        string strQry = string.Empty;
        //        strQry = " SELECT \"CE_DTC_CODE\",\"CE_DTC_NAME\",\"CE_LONGITUDE\",\"CE_LATITUDE\",CE_ARRESTERS,CE_BREAKER_TYPE,CE_HT_PROTECT,CE_LT_PROTECT,CE_DTCMETERS,CE_GROUNDING,";
        //        strQry += " \"CE_TOTAL_CON_KW\",\"CE_TOTAL_CON_HP\",\"CE_KWH_READING\",\"CE_INTERNAL_CODE\",\"CE_LOADTYPE\",\"CE_PROJECTTYPE\",\"CE_LT_LINE\",\"CE_DEPRECIATION\",";
        //        strQry += " \"CE_INFIASSETID\",\"CE_CTRATIO\" FROM \"TBLCESCENUMERATION\" WHERE \"CE_DTC_CODE\"='" + objFieldEnum.sOldDTCCode + "'";

        //        dt = ObjCon.FetchDataTable(strQry);
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {

        //            objFieldEnum.sDTCMeters = Convert.ToString(dt.Rows[i]["CE_DTCMETERS"]);
        //            objFieldEnum.sDTCName = Convert.ToString(dt.Rows[i]["CE_DTC_NAME"]).Replace("'", "");
        //            objFieldEnum.sLatitude = Convert.ToString(dt.Rows[i]["CE_LATITUDE"]);
        //            objFieldEnum.sLongitude = Convert.ToString(dt.Rows[i]["CE_LONGITUDE"]);
        //            objFieldEnum.sArresters = Convert.ToString(dt.Rows[i]["CE_ARRESTERS"]);
        //            objFieldEnum.sBreakertype = Convert.ToString(dt.Rows[i]["CE_BREAKER_TYPE"]);
        //            objFieldEnum.sHTProtect = Convert.ToString(dt.Rows[i]["CE_HT_PROTECT"]);
        //            objFieldEnum.sLTProtect = Convert.ToString(dt.Rows[i]["CE_LT_PROTECT"]);
        //            objFieldEnum.sDTCMeters = Convert.ToString(dt.Rows[i]["CE_DTCMETERS"]);
        //            objFieldEnum.sGrounding = Convert.ToString(dt.Rows[i]["CE_GROUNDING"]);
        //            objFieldEnum.sConnectedKW = Convert.ToString(dt.Rows[i]["CE_TOTAL_CON_KW"]);
        //            objFieldEnum.sConnectedHP = Convert.ToString(dt.Rows[i]["CE_TOTAL_CON_HP"]);
        //            objFieldEnum.sKWHReading = Convert.ToString(dt.Rows[i]["CE_KWH_READING"]);
        //            objFieldEnum.sInternalCode = Convert.ToString(dt.Rows[i]["CE_INTERNAL_CODE"]);
        //            objFieldEnum.sLoadtype = Convert.ToString(dt.Rows[i]["CE_LOADTYPE"]);
        //            objFieldEnum.sProjecttype = Convert.ToString(dt.Rows[i]["CE_PROJECTTYPE"]);
        //            objFieldEnum.sLTlinelength = Convert.ToString(dt.Rows[i]["CE_LT_LINE"]);
        //            objFieldEnum.sDepreciation = Convert.ToString(dt.Rows[i]["CE_DEPRECIATION"]);
        //            objFieldEnum.sInfosysAsset = Convert.ToString(dt.Rows[i]["CE_INFIASSETID"]);


        //        }
        //        return objFieldEnum;
        //    }
        //    catch (Exception ex)
        //    {
        //        //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetCESCOldData");
        //        clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return objFieldEnum;
        //    }
        //}


        #region QC Approval / Reject

        //ED_STATUS---------->0.Pending For Approval 1.Approved 2.Pending For Clarification 3.Reject
        public string[] RejectEnumerationDetails(clsFieldEnumeration objFieldEnum)
        {
            string[] Arr = new string[3];
            PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
            DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);

            try
            {
                //string ED_STATUS_FLAG = ObjCon.get_value("SELECT \"ED_STATUS_FLAG\" from \"TBLENUMERATIONDETAILS\" where \"ED_STATUS_FLAG\" = '1' AND CAST(\"ED_ID\" AS TEXT) ='" + objFieldEnum.sEnumDetailsID + "'");

                NpgsqlCommand cmd = new NpgsqlCommand("proc_check_already_enumeration_approved");
                cmd.Parameters.AddWithValue("ed_id", (objFieldEnum.sEnumDetailsID ?? ""));
                string ED_STATUS_FLAG = objDatabse.StringGetValue(cmd);
                if (ED_STATUS_FLAG == "1")
                {
                    Arr[0] = "0";
                    Arr[1] = "Already Approved Cant Reject";
                    return Arr;
                }
                ObjCon.BeginTransaction();
                //string strQry = string.Empty;

                //objFieldEnum.sQcRejectId = Convert.ToString(ObjCon.Get_max_no("QR_ID", "TBLQCREJECT"));
                //strQry = "INSERT INTO \"TBLQCREJECT\" (\"QR_ID\",\"QR_ED_ID\",\"QR_REMARKS\",\"QR_CRON\",\"QR_CRBY\") values ('" + objFieldEnum.sQcRejectId + "',";
                //strQry += " '" + objFieldEnum.sEnumDetailsID + "','" + objFieldEnum.sRemark.Replace("'", "") + "',NOW(),";
                //strQry += " '" + objFieldEnum.sCrBy + "')";
                //ObjCon.ExecuteQry(strQry);

                //strQry = "UPDATE \"TBLENUMERATIONDETAILS\" set \"ED_STATUS_FLAG\"=3 where CAST(\"ED_ID\" AS TEXT)='" + objFieldEnum.sEnumDetailsID + "'";
                //ObjCon.ExecuteQry(strQry);
                //ObjCon.CommitTransaction();
                //Arr[0] = "1";
                //Arr[1] = "Enumeration Details Rejected Successfull";
                //return Arr;

                NpgsqlCommand cmd1 = new NpgsqlCommand("proc_reject_enumeration_details");
                cmd1.Parameters.AddWithValue("qr_id", (objFieldEnum.sQcRejectId ?? ""));
                cmd1.Parameters.AddWithValue("qr_ed_id", (objFieldEnum.sEnumDetailsID ?? ""));
                cmd1.Parameters.AddWithValue("qr_remarks", (objFieldEnum.sRemark.Replace("'", "") ?? ""));
                cmd1.Parameters.AddWithValue("qr_crby", (objFieldEnum.sCrBy ?? ""));
                cmd1.Parameters.AddWithValue("ed_id", (objFieldEnum.sEnumDetailsID ?? ""));

                cmd1.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd1.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd1.Parameters.Add("pk_id", NpgsqlDbType.Text);
                cmd1.Parameters["msg"].Direction = ParameterDirection.Output;
                cmd1.Parameters["op_id"].Direction = ParameterDirection.Output;
                cmd1.Parameters["pk_id"].Direction = ParameterDirection.Output;

                ObjCon.CommitTransaction();
                Arr[0] = "op_id";
                Arr[1] = "msg";
                Arr[2] = "pk_id";
                Arr = ObjCon.Execute(cmd1, Arr, 3);
                return Arr;
            }
            catch (Exception ex)
            {
                ObjCon.RollBackTrans();
                //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "RejectEnumerationDetails");
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;

            }
        }

        // UnUsed Method

        //public bool PendingForClarification(clsFieldEnumeration objFieldEnum)
        //{
        //    PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
        //    try
        //    {

        //        ObjCon.BeginTransaction();
        //        string strQry = string.Empty;

        //        objFieldEnum.sPendingForClarId = Convert.ToString(ObjCon.Get_max_no("QP_ID", "TBLQCPENDING"));
        //        strQry = "INSERT INTO \"TBLQCPENDING\" (\"QP_ID\",\"QP_ED_ID\",\"QP_REMARKS\",\"QP_CRON\",\"QP_CRBY\") values ('" + objFieldEnum.sPendingForClarId + "','" + objFieldEnum.sEnumDetailsID + "',";
        //        strQry += " '" + objFieldEnum.sRemark.Replace("'", "") + "',NOW(),";
        //        strQry += " '" + objFieldEnum.sCrBy + "')";
        //        ObjCon.ExecuteQry(strQry);

        //        //      strQry = "UPDATE \"TBLENUMERATIONDETAILS\" SET \"ED_STATUS_FLAG\"=2, \"ED_STATUS_FLAG\" = 0 WHERE CAST(\"ED_ID\" AS TEXT)='" + objFieldEnum.sEnumDetailsID + "'";

        //        strQry = "UPDATE \"TBLENUMERATIONDETAILS\" SET \"ED_STATUS_FLAG\"=2 WHERE CAST(\"ED_ID\" AS TEXT)='" + objFieldEnum.sEnumDetailsID + "'";
        //        ObjCon.ExecuteQry(strQry);

        //        ObjCon.CommitTransaction();
        //        return true;

        //    }
        //    catch (Exception ex)
        //    {
        //        ObjCon.RollBackTrans();
        //        //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "PendingForClarification");
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return false;
        //    }
        //}


        public string[] ApproveQCEnumerationDetails(clsFieldEnumeration objFieldEnum)
        {
            string[] Arr = new string[2];
            string[] Arr1 = new string[2];
            string strQry = string.Empty;
            string sRes = string.Empty;
            string strOfficeCode = string.Empty;
            PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
            DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);

            try
            {
                if (objFieldEnum.sQCApprovalId == "")
                {
                    if (objFieldEnum.sEnumType == "2")
                    {
                        objFieldEnum.sLocType = "2";
                    }
                    string sThroughApp = string.Empty;
                    //sThroughApp = ObjCon.get_value("SELECT \"ED_THROUGH\" FROM \"TBLENUMERATIONDETAILS\" WHERE CAST(\"ED_ID\" AS TEXT)  = cast('" + objFieldEnum.sEnumDetailsID + "' as TEXT)  ");
                    NpgsqlCommand cmd1 = new NpgsqlCommand("proc_check_enumeration_through");
                    cmd1.Parameters.AddWithValue("ed_id", (objFieldEnum.sEnumDetailsID ?? ""));
                    sThroughApp = objDatabse.StringGetValue(cmd1);

                    if (objFieldEnum.sDTCCode == null)
                    {
                        objFieldEnum.sDTCCode = "";
                    }
                    if (objFieldEnum.sDTCWithoutDTR == "1")
                    {
                        objFieldEnum.sTcCode = "0";
                        objFieldEnum.sTCMake = "0";

                    }
                    if (objFieldEnum.sTcCode == null)
                    {
                        objFieldEnum.sTcCode = "0";
                    }
                    if (objFieldEnum.sTCMake == null)
                    {
                        objFieldEnum.sTCMake = "1";
                    }
                    if (objFieldEnum.sTCCapacity == null)
                    {
                        objFieldEnum.sTCCapacity = "0";
                    }
                    if (objFieldEnum.sTCType == null)
                    {
                        objFieldEnum.sTCType = "1";
                    }

                    if (objFieldEnum.sConnectedKW == null)
                    {
                        objFieldEnum.sConnectedKW = "0";
                    }
                    if (objFieldEnum.sConnectedHP == null)
                    {
                        objFieldEnum.sConnectedHP = "0";
                    }
                    if (objFieldEnum.sKWHReading == null)
                    {
                        objFieldEnum.sKWHReading = "0";
                    }

                    if (objFieldEnum.sPlatformType == null)
                    {
                        objFieldEnum.sPlatformType = "0";
                    }
                    if (objFieldEnum.sBreakertype == null)
                    {
                        objFieldEnum.sBreakertype = "0";
                    }
                    if (objFieldEnum.sDTCMeters == null)
                    {
                        objFieldEnum.sDTCMeters = "0";
                    }

                    if (objFieldEnum.sHTProtect == null)
                    {
                        objFieldEnum.sHTProtect = "0";
                    }
                    if (objFieldEnum.sLTProtect == null)
                    {
                        objFieldEnum.sLTProtect = "0";
                    }
                    if (objFieldEnum.sGrounding == null)
                    {
                        objFieldEnum.sGrounding = "0";
                    }

                    if (objFieldEnum.sArresters == null)
                    {
                        objFieldEnum.sArresters = "0";
                    }

                    if (objFieldEnum.sLoadtype == null)
                    {
                        objFieldEnum.sLoadtype = "0";
                    }

                    if (objFieldEnum.sProjecttype == null)
                    {
                        objFieldEnum.sProjecttype = "0";
                    }

                    if (objFieldEnum.sRating == null || objFieldEnum.sRating == "")
                    {
                        objFieldEnum.sRating = "0";
                    }
                    if (objFieldEnum.sStarRate == null || objFieldEnum.sStarRate == "")
                    {
                        objFieldEnum.sStarRate = "0";
                    }

                    if (objFieldEnum.sMakeName == null || objFieldEnum.sMakeName == "")
                    {
                        objFieldEnum.sMakeName = "";
                    }
                    if (objFieldEnum.sLocAddress == null || objFieldEnum.sLocAddress == "")
                    {
                        objFieldEnum.sLocAddress = "";
                    }
                    if (objFieldEnum.sLocName == null || objFieldEnum.sLocName == "")
                    {
                        objFieldEnum.sLocName = "";
                    }
                    if (objFieldEnum.sTCType == null || objFieldEnum.sTCType == "")
                    {
                        objFieldEnum.sTCType = "";
                    }
                    if (objFieldEnum.sPlatformType == null || objFieldEnum.sPlatformType == "")
                    {
                        objFieldEnum.sPlatformType = "0";
                    }
                    if (objFieldEnum.sBreakertype == null || objFieldEnum.sBreakertype == "")
                    {
                        objFieldEnum.sBreakertype = "0";
                    }
                    if (objFieldEnum.sDTCMeters == null || objFieldEnum.sDTCMeters == "")
                    {
                        objFieldEnum.sDTCMeters = "0";
                    }
                    if (objFieldEnum.sLTProtect == null || objFieldEnum.sLTProtect == "")
                    {
                        objFieldEnum.sLTProtect = "0";
                    }
                    if (objFieldEnum.sHTProtect == null || objFieldEnum.sHTProtect == "")
                    {
                        objFieldEnum.sHTProtect = "0";
                    }
                    if (objFieldEnum.sGrounding == null || objFieldEnum.sGrounding == "")
                    {
                        objFieldEnum.sGrounding = "0";
                    }
                    if (objFieldEnum.sArresters == null || objFieldEnum.sArresters == "")
                    {
                        objFieldEnum.sArresters = "0";
                    }
                    if (objFieldEnum.sLoadtype == null || objFieldEnum.sLoadtype == "")
                    {
                        objFieldEnum.sLoadtype = "0";
                    }
                    if (objFieldEnum.sProjecttype == null || objFieldEnum.sProjecttype == "")
                    {
                        objFieldEnum.sProjecttype = "0";
                    }
                    if (objFieldEnum.sHTlinelength == null || objFieldEnum.sHTlinelength == "")
                    {
                        objFieldEnum.sHTlinelength = "0";
                    }
                    if (objFieldEnum.sRating == null || objFieldEnum.sRating == "")
                    {
                        objFieldEnum.sRating = "0";
                    }
                    if (objFieldEnum.sStarRate == null || objFieldEnum.sStarRate == "")
                    {
                        objFieldEnum.sStarRate = "0";
                    }
                    if (objFieldEnum.sFeederCode == null || objFieldEnum.sFeederCode == "")
                    {
                        objFieldEnum.sFeederCode = "";
                    }
                    if (objFieldEnum.sDTCName == null || objFieldEnum.sDTCName == "")
                    {
                        objFieldEnum.sDTCName = "";
                    }
                    if (objFieldEnum.sEnumDate == null || objFieldEnum.sEnumDate == "")
                    {
                        objFieldEnum.sEnumDate = "";
                    }
                    if (objFieldEnum.sIPDTCCode == null || objFieldEnum.sIPDTCCode == "")
                    {
                        objFieldEnum.sIPDTCCode = "";
                    }
                    if (objFieldEnum.sOldDTCCode == null || objFieldEnum.sOldDTCCode == "")
                    {
                        objFieldEnum.sOldDTCCode = "";
                    }
                    if (objFieldEnum.sCommisionDate == null || objFieldEnum.sCommisionDate == "")
                    {
                        objFieldEnum.sCommisionDate = "";
                    }

                    if (objFieldEnum.sLastServiceDate == null || objFieldEnum.sLastServiceDate == "")
                    {
                        //objFieldEnum.sOldDTCCode = "";
                    }
                    if (objFieldEnum.sInternalCode == null || objFieldEnum.sInternalCode == "")
                    {
                        objFieldEnum.sInternalCode = "";
                    }

                    if (objFieldEnum.sHTlinelength == null || objFieldEnum.sHTlinelength == "")
                    {
                        objFieldEnum.sHTlinelength = "";
                    }

                    if (objFieldEnum.sLTlinelength == null || objFieldEnum.sLTlinelength == "")
                    {
                        objFieldEnum.sLTlinelength = "";
                    }


                    if (objFieldEnum.sLatitude == null || objFieldEnum.sLatitude == "")
                    {
                        objFieldEnum.sLatitude = "";
                    }

                    if (objFieldEnum.sLongitude == null || objFieldEnum.sLongitude == "")
                    {
                        objFieldEnum.sLongitude = "";
                    }

                    if (objFieldEnum.sDepreciation == null || objFieldEnum.sDepreciation == "")
                    {
                        objFieldEnum.sDepreciation = "0";
                    }

                    if (objFieldEnum.sLastServiceDate == null || objFieldEnum.sLastServiceDate == "")
                    {
                        objFieldEnum.sLastServiceDate = "";
                    }

                    if (objFieldEnum.sDTCWithoutDTR == null || objFieldEnum.sDTCWithoutDTR == "")
                    {
                        objFieldEnum.sDTCWithoutDTR = "0";
                    }
                    if (objFieldEnum.sInfosysAsset == null || objFieldEnum.sInfosysAsset == "")
                    {
                        objFieldEnum.sInfosysAsset = "";
                    }
                    int NoofTimes = 0;

                    LOOP:
                    //NpgsqlCommand cmd = new NpgsqlCommand("sp_saveqcapproved");
                    NpgsqlCommand cmd = new NpgsqlCommand("sp_saveqcapproved_new");

                    //if condition written by sandeep on 04/04/2022 bcz of feeder bifurcation data should not insert into tbldtcenumeration and tblenumeraton 
                    if (!(sThroughApp == "3"))
                    {
                        SaveFieldEnumerationDetails(objFieldEnum);
                    }


                    // cmd.Parameters.AddWithValue("operator1", objFieldEnum.sOperator1);
                    cmd.Parameters.AddWithValue("tc_code", objFieldEnum.sTcCode);
                    cmd.Parameters.AddWithValue("dtc_code", objFieldEnum.sDTCCode);
                    cmd.Parameters.AddWithValue("off_code", objFieldEnum.sOfficeCode);
                    cmd.Parameters.AddWithValue("enumdetailsid", objFieldEnum.sEnumDetailsID);
                    cmd.Parameters.AddWithValue("makename", objFieldEnum.sMakeName);
                    cmd.Parameters.AddWithValue("tcslno", objFieldEnum.sTCSlno);
                    //cmd.Parameters.AddWithValue("soperator2", objFieldEnum.sOperator2);
                    cmd.Parameters.AddWithValue("swelddate", objFieldEnum.sWeldDate);
                    cmd.Parameters.AddWithValue("sfeedercode", objFieldEnum.sFeederCode);
                    cmd.Parameters.AddWithValue("slocname", objFieldEnum.sLocName);
                    cmd.Parameters.AddWithValue("slocaddress", objFieldEnum.sLocAddress);
                    cmd.Parameters.AddWithValue("senumtype", objFieldEnum.sEnumType);
                    cmd.Parameters.AddWithValue("scrby", objFieldEnum.sCrBy);
                    cmd.Parameters.AddWithValue("stcmake", objFieldEnum.sTCMake);
                    cmd.Parameters.AddWithValue("stccapacity", objFieldEnum.sTCCapacity);
                    cmd.Parameters.AddWithValue("stcmanfdate", objFieldEnum.sTCManfDate);
                    cmd.Parameters.AddWithValue("stctype", objFieldEnum.sTCType);
                    cmd.Parameters.AddWithValue("sdtcname", objFieldEnum.sDTCName);
                    cmd.Parameters.AddWithValue("sdtccode", objFieldEnum.sDTCCode);
                    cmd.Parameters.AddWithValue("solddtccode", objFieldEnum.sOldDTCCode);
                    cmd.Parameters.AddWithValue("sipdtccode", objFieldEnum.sIPDTCCode);
                    cmd.Parameters.AddWithValue("senumdate", objFieldEnum.sEnumDate);
                    cmd.Parameters.AddWithValue("sconnectedkw", objFieldEnum.sConnectedKW);
                    cmd.Parameters.AddWithValue("sconnectedhp", objFieldEnum.sConnectedHP);
                    cmd.Parameters.AddWithValue("skwhreading", objFieldEnum.sKWHReading);
                    cmd.Parameters.AddWithValue("scommisiondate", objFieldEnum.sCommisionDate);
                    cmd.Parameters.AddWithValue("slastservicedate", objFieldEnum.sLastServiceDate);
                    cmd.Parameters.AddWithValue("splatformtype", objFieldEnum.sPlatformType);
                    cmd.Parameters.AddWithValue("sbreakertype", objFieldEnum.sBreakertype);
                    cmd.Parameters.AddWithValue("sinternalcode", objFieldEnum.sInternalCode);
                    cmd.Parameters.AddWithValue("sdtcmeters", objFieldEnum.sDTCMeters);
                    cmd.Parameters.AddWithValue("shtprotect", objFieldEnum.sHTProtect);
                    cmd.Parameters.AddWithValue("sltprotect", objFieldEnum.sLTProtect);
                    cmd.Parameters.AddWithValue("sgrounding", objFieldEnum.sGrounding);
                    cmd.Parameters.AddWithValue("sarresters", objFieldEnum.sArresters);
                    cmd.Parameters.AddWithValue("sloadtype", objFieldEnum.sLoadtype);
                    cmd.Parameters.AddWithValue("sprojecttype", objFieldEnum.sProjecttype);
                    cmd.Parameters.AddWithValue("sltlinelength", objFieldEnum.sHTlinelength);
                    cmd.Parameters.AddWithValue("shtlinelength", objFieldEnum.sLTlinelength);
                    cmd.Parameters.AddWithValue("slongitude", objFieldEnum.sLongitude);
                    cmd.Parameters.AddWithValue("slatitude", objFieldEnum.sLatitude);
                    cmd.Parameters.AddWithValue("sdepreciation", objFieldEnum.sDepreciation);
                    cmd.Parameters.AddWithValue("stankcapacity", objFieldEnum.sTankCapacity);
                    cmd.Parameters.AddWithValue("stcweight", objFieldEnum.sTCWeight == null ? "0" : objFieldEnum.sTCWeight);
                    cmd.Parameters.AddWithValue("sinfosysasset", objFieldEnum.sInfosysAsset);
                    cmd.Parameters.AddWithValue("srating", objFieldEnum.sRating);
                    cmd.Parameters.AddWithValue("sstarrate", objFieldEnum.sStarRate);
                    cmd.Parameters.AddWithValue("sqcapprovalid", objFieldEnum.sQCApprovalId);
                    cmd.Parameters.AddWithValue("sdtcwithoutdtr", objFieldEnum.sDTCWithoutDTR);
                    // cmd.Parameters.AddWithValue("sdtrcommissiondate", objFieldEnum.sDtrcommissiondate);
                    cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd.Parameters.Add("id", NpgsqlDbType.Text);
                    cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                    cmd.Parameters["id"].Direction = ParameterDirection.Output;
                    Arr[1] = "id";
                    Arr[0] = "msg";
                    // Arr = ObjCon.Execute(cmd, Arr, 2);

                    try
                    {
                        Arr = ObjCon.Execute(cmd, Arr, 2);
                    }
                    catch (Exception ex)
                    {
                        System.Threading.Thread.Sleep(100);
                        NoofTimes++;
                        if (NoofTimes < 3)
                        {
                            goto LOOP;
                        }
                        else
                        {
                            throw ex;
                        }
                    }
                    if (Arr[1] == "2")
                    {
                        return Arr;
                    }

                    // Save To Main Table




                    // by pass here for the Tagged DTR's 
                    #region TC Details

                    string tempDTRCode = string.Empty;
                    //tempDTRCode = ObjCon.get_value("SELECT \"TG_DTE_TC_CODE\" FROM \"TBLTAGGEDDTR\" WHERE CAST(\"TG_DTE_TC_CODE\" AS TEXT)  = cast('" + objFieldEnum.sTcCode + "' as TEXT) AND  \"TG_ED_STATUS_FLAG\"  = 1 ");

                    NpgsqlCommand cmd2 = new NpgsqlCommand("proc_check_tbltaggeddtr");
                    cmd2.Parameters.AddWithValue("tc_code", (objFieldEnum.sTcCode ?? ""));
                    tempDTRCode = objDatabse.StringGetValue(cmd2);

                    if (tempDTRCode == null || tempDTRCode == "")
                    {


                        // Transformer Details
                        clsTcMaster objTcMaster = new clsTcMaster();

                        objTcMaster.sTimeId = objFieldEnum.sTimeId;

                        objTcMaster.sTcId = "";
                        objTcMaster.sTcSlNo = objFieldEnum.sTCSlno;
                        objTcMaster.sTcMakeId = objFieldEnum.sTCMake;

                        objTcMaster.sTcCode = objFieldEnum.sTcCode;
                        objTcMaster.sTcCapacity = objFieldEnum.sTCCapacity;
                        objTcMaster.sManufacDate = objFieldEnum.sTCManfDate;

                        objTcMaster.sPoNo = "";
                        objTcMaster.sPrice = "";
                        objTcMaster.sWarrentyPeriod = "";
                        objTcMaster.sLastServiceDate = "";
                        objTcMaster.sCurrentLocation = objFieldEnum.sEnumType;
                        objTcMaster.sTcLifeSpan = "";
                        objTcMaster.sCrBy = objFieldEnum.sCrBy;
                        objTcMaster.sOfficeCode = objFieldEnum.sOfficeCode;

                        objTcMaster.sRating = objFieldEnum.sRating;
                        objTcMaster.sStarRate = objFieldEnum.sStarRate;
                        objTcMaster.sWeight = objFieldEnum.sTCWeight;
                        objTcMaster.sOilCapacity = objFieldEnum.sTankCapacity;
                        objTcMaster.sLocationId = objFieldEnum.sLocation;
                        objTcMaster.sType = objFieldEnum.sTCType;
                        objTcMaster.locationtype = objFieldEnum.locationtype;
                        objTcMaster.sFeederCode = objFieldEnum.sFeederCode;
                        objTcMaster.sOldDTCCode = objFieldEnum.sOldDTCCode;
                        objTcMaster.sDTRcommissionDate = objFieldEnum.sDtrcommissiondate;

                        if (objFieldEnum.locationtype == "--Select--")
                        {
                            objFieldEnum.locationtype = "0";
                            objTcMaster.locationtype = objFieldEnum.locationtype;
                        }


                        if (!(sThroughApp == "3"))
                        {
                            if (objFieldEnum.sDTCWithoutDTR != "1")
                            {
                                Arr = objTcMaster.SaveUpdateTransformerDetails(objTcMaster);
                                #region
                                // commented by siddesha
                                //if (objFieldEnum.sEnumType == "1")
                                //{
                                //    strQry = "UPDATE \"TBLTAGGEDDTR\" set \"TG_ED_STATUS_FLAG\" =  1  WHERE \"TG_DTE_TC_CODE\" = '" + objFieldEnum.sTcCode + "' and \"TG_ED_STATUS_FLAG\" =  0 ";
                                //    ObjCon.ExecuteQry(strQry);

                                //    //Commented by sandeep 25-11-2022
                                //    //strQry = "UPDATE  \"TBLTCPLATEALLOCATION\" set \"TCP_STATUS_FLAG\" = 0 , \"TCP_DESC\" = 'TAGGED DTR' WHERE \"TCP_TC_CODE\" = '" + objFieldEnum.sTcCode + "'";
                                //    //ObjCon.ExecuteQry(strQry);
                                //}

                                //if (Arr[1] == "2")
                                //{

                                //    string sqry = "DELETE FROM \"TBLQCAPPROVEDOBJECTS\" WHERE  \"QAO_TC_CODE\" = '" + objFieldEnum.sTcCode + "' ";
                                //    ObjCon.ExecuteQry(sqry);

                                //    sqry = "DELETE FROM \"TBLQCAPPROVED\" WHERE  \"QA_ED_ID\" = '" + objFieldEnum.sEnumDetailsID + "' ";
                                //    ObjCon.ExecuteQry(sqry);

                                //    return Arr;
                                //}
                                #endregion
                                if (Arr[1] == "3")
                                {
                                    return Arr;
                                }
                            }
                        }
                        #endregion
                    }
                    // commented by siddesha
                    //else
                    //{
                    //    strQry = "UPDATE \"TBLTAGGEDDTR\" SET \"TG_MOBILE_ENUMERATION\" = 1 WHERE  \"TG_DTE_TC_CODE\" = '" + objFieldEnum.sTcCode + "' AND  \"TG_MOBILE_ENUMERATION\" = 0 AND \"TG_ED_STATUS_FLAG\" = 1";
                    //    ObjCon.ExecuteQry(strQry);
                    //}


                    //EnumType-------> 1.Store 2.Field 3.Repairer
                    if (objFieldEnum.sEnumType == "2" && (tempDTRCode == null || tempDTRCode == ""))
                    {
                        // DTC Details

                        #region DTC Details

                        clsDTCCommision objDtcCommision = new clsDTCCommision();

                        objDtcCommision.sTimeId = objFieldEnum.sTimeId; 
                        objDtcCommision.lDtcId = "";
                        objDtcCommision.sDtcName = objFieldEnum.sDTCName;
                        objDtcCommision.iConnectedHP = Convert.ToString(objFieldEnum.sConnectedHP);
                        objDtcCommision.iConnectedKW = Convert.ToString(objFieldEnum.sConnectedKW);
                        objDtcCommision.sInternalCode = objFieldEnum.sInternalCode;
                        objDtcCommision.sFeederCode = objFieldEnum.sFeederCode;
                        objDtcCommision.sServiceDate = objFieldEnum.sLastServiceDate;
                        objDtcCommision.sOMSectionName = objFieldEnum.sOfficeCode;
                        objDtcCommision.sDtcCode = objFieldEnum.sDTCCode;
                        objDtcCommision.iKWHReading = objFieldEnum.sKWHReading;
                        objDtcCommision.sCommisionDate = objFieldEnum.sCommisionDate;
                        //objDtcCommision.sConnectionDate = txtConnectionDate.Text;
                        objDtcCommision.sTcCode = objFieldEnum.sTcCode;
                        objDtcCommision.sCrBy = objFieldEnum.sCrBy;
                        //objDtcCommision.sOldTcCode = txtOldTCCode.Text; 
                        objDtcCommision.sOldDtOldDtccode = objFieldEnum.sDTCOldDTCCode;


                        // objDtcCommision.sWOslno = txtWOslno.Text;
                        objDtcCommision.sOfficeCode = objFieldEnum.sOfficeCode;
                        objDtcCommision.sPlatformType = objFieldEnum.sPlatformType;
                        objDtcCommision.sBreakertype = objFieldEnum.sBreakertype;
                        objDtcCommision.sDTCMeters = objFieldEnum.sDTCMeters;
                        objDtcCommision.sHTProtect = objFieldEnum.sHTProtect;
                        objDtcCommision.sLTProtect = objFieldEnum.sLTProtect;
                        objDtcCommision.sGrounding = objFieldEnum.sGrounding;
                        objDtcCommision.sArresters = objFieldEnum.sArresters;
                        objDtcCommision.sLoadtype = objFieldEnum.sLoadtype;
                        objDtcCommision.sProjecttype = objFieldEnum.sProjecttype;
                        objDtcCommision.sLtlinelength = objFieldEnum.sLTlinelength;
                        objDtcCommision.sHtlinelength = objFieldEnum.sHTlinelength;
                        objDtcCommision.sTimsCode = objFieldEnum.sOldDTCCode;
                        objDtcCommision.sLatitude = objFieldEnum.sLatitude;
                        objDtcCommision.sLongitude = objFieldEnum.sLongitude;
                        objDtcCommision.locationtype = objFieldEnum.locationtype;
                        objDtcCommision.sTcSlno = objFieldEnum.sTCSlno;
                        objDtcCommision.sDepreciation = objFieldEnum.sDepreciation;
                        objDtcCommision.sDTrCommisionDate = objFieldEnum.sDtrcommissiondate;


                        if (objFieldEnum.sDTCWithoutDTR == "1")
                        {
                            objDtcCommision.sDtcWithoutDtrFlag = objFieldEnum.sDTCWithoutDTR;
                        }
                        else
                        {
                            objDtcCommision.sDtcWithoutDtrFlag = "0";
                        }
                        //Arr = objDtcCommision.SaveUpdateDtcDetails(objDtcCommision);
                        if (!(sThroughApp == "3"))
                        {
                            Arr = objDtcCommision.SaveUpdateDtcDetailsFldEnum(objDtcCommision);

                            // commented by siddesha

                            //strQry = "UPDATE \"TBLTAGGEDDTR\" set \"TG_ED_STATUS_FLAG\" =  1  WHERE \"TG_DTE_TC_CODE\" = '" + objFieldEnum.sTcCode + "' and \"TG_DTE_DTCCODE\" = '" + objFieldEnum.sDTCCode + "' and  \"TG_ED_STATUS_FLAG\" =  0 ";
                            //ObjCon.ExecuteQry(strQry);

                            //if (Arr[1] == "2")
                            //{
                            //    string sqry = "DELETE FROM \"TBLDTRTRANSACTION\" WHERE  \"DRT_DTR_CODE\" = '" + objFieldEnum.sTcCode + "' ";
                            //    ObjCon.ExecuteQry(sqry);

                            //    sqry = "DELETE FROM \"TBLTCMASTER\" WHERE   \"TC_CODE\" = '" + objFieldEnum.sTcCode + "' ";
                            //    ObjCon.ExecuteQry(sqry);

                            //    return Arr;
                            //}
                        }

                        #endregion
                    }



                    #region Approve Flag
                    // commented by siddesha
                    //strQry = "UPDATE \"TBLENUMERATIONDETAILS\" SET \"ED_STATUS_FLAG\"=1,\"ED_APPROVED_BY\"='" + objFieldEnum.sCrBy + "', ";
                    //strQry += " \"ED_APPROVED_ON\"=NOW() WHERE \"ED_ID\"='" + objFieldEnum.sEnumDetailsID + "'";
                    //ObjCon.ExecuteQry(strQry);

                    #endregion
                    // commented by siddesha
                    //strQry = "UPDATE \"TBLTCPLATEALLOCATION\" SET  \"TCP_STATUS_FLAG\" = '1' WHERE CAST(\"TCP_TC_CODE\" AS TEXT) ='" + objFieldEnum.sTcCode + "'";
                    //ObjCon.ExecuteQry(strQry);

                    DateTime endtime = DateTime.Now;
                    //strQry = "UPDATE \"TBLTIMELOG\" SET \"TL_END_TIME\"= TO_DATE('" + endtime + "','mm/dd/yyyy HH:MI:SSAM') WHERE \"TL_ID\"='" + ApproveTimeid + "'";
                    //ObjCon.ExecuteQry(strQry);

                    NpgsqlCommand cmd3 = new NpgsqlCommand("proc_update_enumeration_approve_flag");
                    cmd3.Parameters.AddWithValue("ed_id", Convert.ToString(objFieldEnum.sEnumDetailsID ?? ""));
                    cmd3.Parameters.AddWithValue("ed_approved_by", Convert.ToString(objFieldEnum.sCrBy ?? ""));
                    cmd3.Parameters.AddWithValue("tc_code", Convert.ToString(objFieldEnum.sTcCode ?? ""));
                    cmd3.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd3.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd3.Parameters["msg"].Direction = ParameterDirection.Output;
                    cmd3.Parameters["op_id"].Direction = ParameterDirection.Output;
                    Arr1[0] = "msg";
                    Arr1[1] = "op_id";
                    Arr1 = ObjCon.Execute(cmd3, Arr1, 2);

                    Arr[0] = "Enumeration Details Approved Successfully";
                    Arr[1] = "0";
                    //ObjCon.CommitTransaction();
                    //return Arr;
                }
                //commented by siddesha
                //else
                //{

                //    /// coded by pradeep - start
                //    strQry = "SELECT \"VM_NAME\" FROM \"TBLINTERNALUSERS\",\"TBLVENDORMASTER\",\"TBLTCPLATEALLOCATIONMASTER\",\"TBLTCPLATEALLOCATION\" WHERE";
                //    strQry += " CAST(\"IU_VENDOR_ID\" AS TEXT)=CAST(\"VM_ID\" AS TEXT) AND CAST(\"VM_ID\" AS TEXT)=CAST(\"TCPM_VENDOR_ID\" AS TEXT) AND CAST(\"TCPM_ID\" AS TEXT)=CAST(\"TCP_TCPM_ID\" AS TEXT) AND CAST(\"IU_ID\" AS TEXT)= '" + objFieldEnum.sOperator1 + "' AND \"TCP_STATUS_FLAG\"='0' AND \"TCP_TC_CODE\" = '" + objFieldEnum.sTcCode + "'";
                //    string Vendorname = ObjCon.get_value(strQry);
                //    if (Vendorname == "")
                //    {
                //        strQry = "SELECT \"VM_NAME\" FROM \"TBLVENDORMASTER\",\"TBLTCPLATEALLOCATIONMASTER\",\"TBLTCPLATEALLOCATION\" WHERE";
                //        strQry += " CAST(\"VM_ID\" AS TEXT)=CAST(\"TCPM_VENDOR_ID\" AS TEXT) AND CAST(\"TCPM_ID\" AS TEXT)=CAST(\"TCP_TCPM_ID\" AS TEXT) AND CAST(\"TCP_TC_CODE\" AS TEXT) = '" + objFieldEnum.sTcCode + "'";
                //        string Actual_Vendorname = ObjCon.get_value(strQry);
                //        if (!Actual_Vendorname.Trim().Equals(""))
                //        {

                //            strQry = "SELECT DISTINCT \"VM_NAME\" FROM \"TBLINTERNALUSERS\",\"TBLVENDORMASTER\",\"TBLTCPLATEALLOCATIONMASTER\",\"TBLTCPLATEALLOCATION\" WHERE";
                //            strQry += " CAST(\"IU_VENDOR_ID\" AS TEXT)=CAST(\"VM_ID\" AS TEXT) AND CAST(\"VM_ID\" AS TEXT)=CAST(\"TCPM_VENDOR_ID\" AS TEXT) AND CAST(\"TCPM_ID\" AS TEXT)=CAST(\"TCP_TCPM_ID\" AS TEXT) AND CAST(\"IU_ID\" AS TEXT)= '" + objFieldEnum.sOperator1 + "'";
                //            string Login_Vendorname = ObjCon.get_value(strQry);

                //            Arr[0] = "Plate Number " + objFieldEnum.sTcCode + "  Already allocated to vendor " + Actual_Vendorname + " you can not Approve the same ss plate code to vendor " + Login_Vendorname;
                //            Arr[1] = "2";
                //            return Arr;
                //        }
                //        else
                //        {
                //            Arr[0] = "SS Plate Number " + objFieldEnum.sTcCode + "  Not allocated to any vendor. please Allocate ss plate and then approve the record .";
                //            Arr[1] = "2";
                //            return Arr;
                //        }
                //    }

                //    if (objFieldEnum.sOfficeCode.Length >= Section_code)
                //    {
                //        strOfficeCode = objFieldEnum.sOfficeCode.Substring(0, SubDiv_code);
                //    }
                //    if (objFieldEnum.sDTCCode != null)
                //    {

                //        strQry = "select \"FD_FEEDER_ID\" from \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" WHERE CAST(\"FD_FEEDER_CODE\" AS TEXT)='" + objFieldEnum.sDTCCode.ToString().Substring(0, Feeder_code) + "' ";
                //        strQry += " AND  CAST(\"FD_FEEDER_ID\" AS TEXT)=CAST(\"FDO_FEEDER_ID\" AS TEXT) AND CAST(\"FDO_OFFICE_CODE\" AS TEXT) LIKE '" + strOfficeCode + "%'";
                //        sRes = ObjCon.get_value(strQry);
                //        if (sRes == "")
                //        {
                //            Arr[0] = "Code Does Not Match With The Feeder Code";
                //            Arr[1] = "2";
                //            return Arr;

                //        }
                //    }

                //    //ObjCon.BeginTransaction();

                //    strQry = "UPDATE \"TBLENUMERATIONDETAILS\" SET \"ED_OFFICECODE\"='" + objFieldEnum.sOfficeCode + "',\"ED_OPERATOR1\"='" + objFieldEnum.sOperator1 + "',\"ED_OPERATOR2\"='" + objFieldEnum.sOperator2 + "'";
                //    strQry += " ,\"ED_WELD_DATE\"=TO_DATE('" + objFieldEnum.sWeldDate + "','dd/MM/yyyy'),\"ED_FEEDERCODE\"='" + objFieldEnum.sFeederCode + "' WHERE ";
                //    strQry += " CAST(\"ED_ID\" AS TEXT)='" + objFieldEnum.sEnumDetailsID + "'";
                //    ObjCon.ExecuteQry(strQry);

                //    //If TC Slno Not Exists, Saving Enumeration DTC Id
                //    if (bTCSlNoNotExists == true)
                //    {
                //        objFieldEnum.sTCSlno = ObjCon.get_value("SELECT \"DTE_ID\" FROM \"TBLDTCENUMERATION\" WHERE CAST(\"DTE_ED_ID\" AS TEXT)='" + objFieldEnum.sEnumDetailsID + "'");
                //    }

                //    strQry = "UPDATE \"TBLDTCENUMERATION\" SET \"DTE_TC_CODE\"='" + objFieldEnum.sTcCode + "',\"DTE_TC_SLNO\"='" + objFieldEnum.sTCSlno + "',\"DTE_MAKE\"='" + objFieldEnum.sTCMake + "'";
                //    strQry += " ,\"DTE_CAPACITY\"='" + objFieldEnum.sTCCapacity + "',\"DTE_TC_MANFDATE\"=TO_DATE('" + objFieldEnum.sTCManfDate + "','dd/MM/yyyy'),";
                //    strQry += " \"DTE_NAME\"='" + objFieldEnum.sDTCName + "',\"DTE_DTCCODE\"='" + objFieldEnum.sDTCCode + "',\"DTE_CESCCODE\"='" + objFieldEnum.sOldDTCCode + "',\"DTE_IPCODE\"='" + objFieldEnum.sIPDTCCode + "',";
                //    strQry += " \"DTE_ENUM_DATE\"=TO_DATE('" + objFieldEnum.sEnumDate + "','dd/MM/yyyy'),\"DTE_TOTAL_CON_KW\"='" + objFieldEnum.sConnectedKW + "',\"DTE_TOTAL_CON_HP\"='" + objFieldEnum.sConnectedHP + "',";
                //    strQry += " \"DTE_KWH_READING\"='" + objFieldEnum.sKWHReading + "',\"DTE_TRANS_COMMISION_DATE\"=TO_DATE('" + objFieldEnum.sCommisionDate + "','dd/MM/yyyy'),";
                //    strQry += " \"DTE_LAST_SERVICE_DATE\"=TO_DATE('" + objFieldEnum.sLastServiceDate + "','dd/MM/yyyy'),\"DTE_PLATFORM\"='" + objFieldEnum.sPlatformType + "',\"DTE_BREAKER_TYPE\"='" + objFieldEnum.sBreakertype + "',";

                //    strQry += " \"DTE_INTERNAL_CODE\"='" + objFieldEnum.sInternalCode + "',\"DTE_DTCMETERS\"='" + objFieldEnum.sDTCMeters + "',\"DTE_HT_PROTECT\"='" + objFieldEnum.sHTProtect + "',\"DTE_LT_PROTECT\"='" + objFieldEnum.sLTProtect + "'";

                //    strQry += " ,\"DTE_GROUNDING\"='" + objFieldEnum.sGrounding + "',";
                //    strQry += " \"DTE_ARRESTERS\"='" + objFieldEnum.sArresters + "',\"DTE_LOADTYPE\"='" + objFieldEnum.sLoadtype + "',\"DTE_PROJECTTYPE\"='" + objFieldEnum.sProjecttype + "',";
                //    strQry += " \"DTE_LT_LINE\"='" + objFieldEnum.sLTlinelength + "',\"DTE_HT_LINE\"='" + objFieldEnum.sHTlinelength + "',\"DTE_LONGITUDE\"='" + objFieldEnum.sLongitude + "',\"DTE_LATITUDE\"='" + objFieldEnum.sLatitude + "',";
                //    strQry += " \"DTE_DEPRECIATION\"='" + objFieldEnum.sDepreciation + "' WHERE \"DTE_ED_ID\"='" + objFieldEnum.sEnumDetailsID + "'";

                //    ObjCon.ExecuteQry(strQry);

                //    strQry = "UPDATE \"TBLTCPLATEALLOCATION\" SET  \"TCP_STATUS_FLAG\" = '1' WHERE CAST(\"TCP_TC_CODE\" AS TEXT) ='" + objFieldEnum.sTcCode + "'";
                //    ObjCon.ExecuteQry(strQry);

                //    DateTime endtime = DateTime.Now;
                //    //strQry = "UPDATE \"TBLTIMELOG\" SET \"TL_END_TIME\"= TO_DATE('" + endtime + "','mm/dd/yyyy HH:MI:SSAM') WHERE \"TL_ID\"='" + ApproveTimeid + "'";
                //    //ObjCon.ExecuteQry(strQry);



                //    Arr[0] = "Enumeration Details Updated Successfully";
                //    Arr[1] = "1";

                //    return Arr;
                //}


                return Arr;

            }
            catch (Exception ex)
            {
                //ObjCon.RollBackTrans();
                //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveFieldEnumerationDetails");
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }

        // UnUsed Method

        //public string[] GetEnumerationInfoForApprove(clsFieldEnumeration objFieldEnum, ArrayList sEnumIdList)
        //{
        //    string[] Arr = new string[2];
        //    string strQry = string.Empty;
        //    DataTable dt = new DataTable();
        //    PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
        //    try
        //    {
        //        strQry = "DELETE FROM \"TBLTIMELOG\"";
        //        //ObjCon.ExecuteQry(strQry);

        //        strQry = "SELECT COALESCE(MAX(\"TL_ID\"),0)+1 FROM \"TBLTIMELOG\"";
        //        //string Timeid = ObjCon.get_value(strQry);
        //        //objFieldEnum.sTimeId = Timeid;
        //        //DateTime starttime = DateTime.Now;
        //        //strQry = "INSERT INTO \"TBLTIMELOG\" (\"TL_ID\",\"TL_PAGE_NAME\",\"TL_FUNCTION\",\"TL_START_TIME\",\"TL_TRANSACTION\")VALUES('" + Timeid + "','" + strFormCode + "',";
        //        //strQry += "'GetEnumerationInfoForApprove',now(),'" + Timeid + "')";
        //        //ObjCon.ExecuteQry(strQry);
        //        for (int i = 0; i < sEnumIdList.Count; i++)

        //        {

        //            strQry = "SELECT \"ED_ID\",\"ED_OFFICECODE\",\"ED_OPERATOR1\",\"ED_CRBY\",\"ED_OPERATOR2\",TO_CHAR(\"ED_WELD_DATE\",'DD/MM/YYYY') \"ED_WELD_DATE\",\"ED_FEEDERCODE\",\"ED_LOCTYPE\",\"ED_LOCNAME\",\"ED_LOCADDRESS\",\"ED_ENUM_TYPE\",\"ED_THROUGH\",";
        //            strQry += " \"DTE_TC_CODE\",\"DTE_TC_SLNO\",\"DTE_MAKE\",\"DTE_CAPACITY\",\"DTE_TC_TYPE\",TO_CHAR(\"DTE_TC_MANFDATE\",'DD/MM/YYYY') \"DTE_TC_MANFDATE\",\"DTE_NAME\",\"DTE_DTCCODE\",";
        //            strQry += " \"DTE_CESCCODE\",\"DTE_IPCODE\",\"DTE_TOTAL_CON_KW\",\"DTE_TOTAL_CON_HP\",\"DTE_KWH_READING\",TO_CHAR(\"DTE_TRANS_COMMISION_DATE\",'DD/MM/YYYY')\"DTE_TRANS_COMMISION_DATE\",TO_CHAR(\"DTE_LAST_SERVICE_DATE\",'DD/MM/YYYY') \"DTE_LAST_SERVICE_DATE\",";
        //            strQry += " \"DTE_BREAKER_TYPE\",\"DTE_INTERNAL_CODE\",\"DTE_DTCMETERS\",\"DTE_HT_PROTECT\",\"DTE_LT_PROTECT\",\"DTE_GROUNDING\",\"DTE_ARRESTERS\",\"DTE_PLATFORM\", \"DTE_DTC_OLDCODE\",";
        //            strQry += " \"DTE_LOADTYPE\",\"DTE_PROJECTTYPE\",\"DTE_LT_LINE\",\"DTE_HT_LINE\",\"DTE_DEPRECIATION\",\"ED_IP_ENUM_DONE\",\"";
        //            strQry += "DTE_ISIPCESC\",\"DTE_TANK_CAPACITY\",\"DTE_TC_WEIGHT\",\"DTE_INFOSYS_ASSET\",\"DTE_RATING\",\"DTE_STAR_RATE\",TO_CHAR(\"DTE_ENUM_DATE\",'DD/MM/YYYY') \"DTE_ENUM_DATE\",\"DTE_LATITUDE\",\"DTE_LONGITUDE\" ";
        //            strQry += " FROM \"TBLENUMERATIONDETAILS\",\"TBLDTCENUMERATION\" ";
        //            strQry += " WHERE CAST(\"ED_ID\" AS TEXT)=CAST(\"DTE_ED_ID\" AS TEXT) AND \"ED_ID\"='" + sEnumIdList[i] + "'";

        //            dt = ObjCon.FetchDataTable(strQry);
        //            if (dt.Rows.Count > 0)
        //            {
        //                objFieldEnum.sQCApprovalId = "";
        //                objFieldEnum.sEnumDetailsID = Convert.ToString(dt.Rows[0]["ED_ID"]);

        //                objFieldEnum.sWeldDate = Convert.ToString(dt.Rows[0]["ED_WELD_DATE"]);
        //                objFieldEnum.sOperator1 = Convert.ToString(dt.Rows[0]["ED_OPERATOR1"]);
        //                objFieldEnum.sOperator2 = Convert.ToString(dt.Rows[0]["ED_OPERATOR2"]);
        //                objFieldEnum.sThroughApp = Convert.ToString(dt.Rows[0]["ED_THROUGH"]);

        //                //TC Details

        //                objFieldEnum.sTcCode = Convert.ToString(dt.Rows[0]["DTE_TC_CODE"]);
        //                objFieldEnum.sTCMake = Convert.ToString(dt.Rows[0]["DTE_MAKE"]);
        //                objFieldEnum.sTCCapacity = Convert.ToString(dt.Rows[0]["DTE_CAPACITY"]);
        //                objFieldEnum.sTCSlno = Convert.ToString(dt.Rows[0]["DTE_TC_SLNO"]);
        //                objFieldEnum.sTCManfDate = Convert.ToString(dt.Rows[0]["DTE_TC_MANFDATE"]);

        //                objFieldEnum.sEnumType = Convert.ToString(dt.Rows[0]["ED_ENUM_TYPE"]);

        //                if (objFieldEnum.sEnumType == "2")
        //                {

        //                    objFieldEnum.sOfficeCode = Convert.ToString(dt.Rows[0]["ED_OFFICECODE"]);
        //                    objFieldEnum.sFeederCode = Convert.ToString(dt.Rows[0]["ED_FEEDERCODE"]);

        //                    // DTC Details
        //                    objFieldEnum.sDTCName = Convert.ToString(dt.Rows[0]["DTE_NAME"]).Replace("'", "");
        //                    objFieldEnum.sDTCCode = Convert.ToString(dt.Rows[0]["DTE_DTCCODE"]);
        //                    objFieldEnum.sOldDTCCode = Convert.ToString(dt.Rows[0]["DTE_CESCCODE"]);
        //                    objFieldEnum.sIPDTCCode = Convert.ToString(dt.Rows[0]["DTE_IPCODE"]);
        //                    objFieldEnum.sEnumDate = Convert.ToString(dt.Rows[0]["DTE_ENUM_DATE"]);


        //                    // DTC Other Details
        //                    objFieldEnum.sInternalCode = Convert.ToString(dt.Rows[0]["DTE_INTERNAL_CODE"]);
        //                    objFieldEnum.sConnectedKW = Convert.ToString(dt.Rows[0]["DTE_TOTAL_CON_KW"]);
        //                    objFieldEnum.sConnectedHP = Convert.ToString(dt.Rows[0]["DTE_TOTAL_CON_HP"]);
        //                    objFieldEnum.sKWHReading = Convert.ToString(dt.Rows[0]["DTE_KWH_READING"]);
        //                    objFieldEnum.sCommisionDate = Convert.ToString(dt.Rows[0]["DTE_TRANS_COMMISION_DATE"]);
        //                    objFieldEnum.sLastServiceDate = Convert.ToString(dt.Rows[0]["DTE_LAST_SERVICE_DATE"]);

        //                    //Other Info about DTC
        //                    objFieldEnum.sPlatformType = Convert.ToString(dt.Rows[0]["DTE_PLATFORM"]);
        //                    objFieldEnum.sBreakertype = Convert.ToString(dt.Rows[0]["DTE_BREAKER_TYPE"]);
        //                    objFieldEnum.sDTCMeters = Convert.ToString(dt.Rows[0]["DTE_DTCMETERS"]);
        //                    objFieldEnum.sHTProtect = Convert.ToString(dt.Rows[0]["DTE_HT_PROTECT"]);
        //                    objFieldEnum.sLTProtect = Convert.ToString(dt.Rows[0]["DTE_LT_PROTECT"]);
        //                    objFieldEnum.sGrounding = Convert.ToString(dt.Rows[0]["DTE_GROUNDING"]);
        //                    objFieldEnum.sArresters = Convert.ToString(dt.Rows[0]["DTE_ARRESTERS"]);
        //                    objFieldEnum.sLoadtype = Convert.ToString(dt.Rows[0]["DTE_LOADTYPE"]);
        //                    objFieldEnum.sProjecttype = Convert.ToString(dt.Rows[0]["DTE_PROJECTTYPE"]);
        //                    objFieldEnum.sLTlinelength = Convert.ToString(dt.Rows[0]["DTE_LT_LINE"]);
        //                    objFieldEnum.sHTlinelength = Convert.ToString(dt.Rows[0]["DTE_HT_LINE"]);
        //                    objFieldEnum.sDepreciation = Convert.ToString(dt.Rows[0]["DTE_DEPRECIATION"]);
        //                    objFieldEnum.sLatitude = Convert.ToString(dt.Rows[0]["DTE_LATITUDE"]);
        //                    objFieldEnum.sLongitude = Convert.ToString(dt.Rows[0]["DTE_LONGITUDE"]);
        //                    objFieldEnum.sDTCOldDTCCode = Convert.ToString(dt.Rows[0]["DTE_DTC_OLDCODE"]);

        //                    objFieldEnum.sIsIPEnumDone = Convert.ToString(dt.Rows[0]["ED_IP_ENUM_DONE"]);

        //                }
        //                else if (objFieldEnum.sEnumType == "1" || objFieldEnum.sEnumType == "3" || objFieldEnum.sEnumType == "5")
        //                {
        //                    objFieldEnum.sOfficeCode = Convert.ToString(dt.Rows[0]["ED_OFFICECODE"]);
        //                    objFieldEnum.sLocName = Convert.ToString(dt.Rows[0]["ED_LOCNAME"]);
        //                    objFieldEnum.sLocAddress = Convert.ToString(dt.Rows[0]["ED_LOCADDRESS"]).Trim().Replace("'", "");
        //                    objFieldEnum.sTCType = Convert.ToString(dt.Rows[0]["ED_LOCTYPE"]);
        //                }

        //                objFieldEnum.sCrBy = Convert.ToString(dt.Rows[0]["ED_CRBY"]);
        //                //objFieldEnum.sEnumType = objFieldEnum.sEnumType;

        //                objFieldEnum.sInfosysAsset = Convert.ToString(dt.Rows[0]["DTE_INFOSYS_ASSET"]);
        //                objFieldEnum.sTCWeight = Convert.ToString(dt.Rows[0]["DTE_TC_WEIGHT"]);
        //                objFieldEnum.sTankCapacity = Convert.ToString(dt.Rows[0]["DTE_TANK_CAPACITY"]);
        //                objFieldEnum.sRating = Convert.ToString(dt.Rows[0]["DTE_RATING"]);
        //                objFieldEnum.sStarRate = Convert.ToString(dt.Rows[0]["DTE_STAR_RATE"]);


        //                Arr = ApproveQCEnumerationDetails(objFieldEnum);

        //                if (Arr[1] == "2")
        //                {
        //                    return Arr;
        //                }

        //            }

        //        }

        //        DateTime endtime = DateTime.Now;
        //        //strQry = "UPDATE \"TBLTIMELOG\" SET \"TL_END_TIME\"= TO_DATE('" + endtime + "','mm/dd/yyyy HH:MI:SSAM') WHERE \"TL_ID\"='" + Timeid + "'";
        //        //ObjCon.ExecuteQry(strQry);               

        //        return Arr;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return Arr;
        //    }
        //}

        #endregion

        //UnUsed Method

        //public clsFieldEnumeration GetNextEnumerationDetails(clsFieldEnumeration objEnum)
        //{
        //    try
        //    {
        //        PGSqlConnection objcon = new PGSqlConnection(Constants.Password);


        //        string sQry = string.Empty;

        //        sQry = " SELECT  \"ED_ID\" || '~' || \"ED_STATUS_FLAG\" FROM \"TBLENUMERATIONDETAILS\", \"TBLDTCENUMERATION\" WHERE \"ED_ID\" = \"DTE_ED_ID\" ";
        //        sQry += " AND \"DTE_DTCCODE\" = ( SELECT MIN(\"DTE_DTCCODE\") FROM \"TBLENUMERATIONDETAILS\", \"TBLDTCENUMERATION\" WHERE \"ED_ID\" = \"DTE_ED_ID\" ";
        //        sQry += " AND (\"ED_STATUS_FLAG\" = 0 or \"ED_STATUS_FLAG\"<>5)  AND \"ED_ENUM_TYPE\" = 2 AND \"ED_FEEDERCODE\" = '" + objEnum.sFeederCode + "'  ";
        //        sQry += " AND \"DTE_DTCCODE\" > '" + objEnum.sDTCCode + "' ";

        //        //AND \"ED_UPDATE_ON\" IS NULL   
        //        if (objEnum.sUserType == "1" || objEnum.sUserType == "3" || objEnum.sUserType == "5")
        //        {
        //            sQry += "     AND \"ED_APPROVALPRIORITY\" = '1') ";
        //        }
        //        else if (objEnum.sUserType == "6")
        //        {
        //            sQry += " AND \"ED_APPROVALPRIORITY\" = '2') ";
        //        }
        //        else if (objEnum.sUserType == "7")
        //        {
        //            sQry += "  AND  \"ED_APPROVALPRIORITY\" = '3') ";
        //        }
        //        else if (objEnum.sUserType == "8")
        //        {
        //            sQry += "  AND  \"ED_APPROVALPRIORITY\" = '4') ";
        //        }
        //        else if (objEnum.sUserType == "2")
        //        {
        //            sQry += " AND \"ED_APPROVALPRIORITY\" = '5') ";
        //        }

        //        //sQry += " LIMIT 1 ";

        //        string sResult = objcon.get_value(sQry);
        //        if (sResult.Length > 0)
        //        {
        //            objEnum.sEnumDTCID = sResult.Split('~').GetValue(0).ToString();
        //            objEnum.sStatus = sResult.Split('~').GetValue(1).ToString();
        //        }
        //        else
        //        {
        //            objEnum.sEnumDTCID = "";
        //            objEnum.sStatus = "";
        //        }
        //        return objEnum;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        objEnum.sEnumDTCID = "";
        //        objEnum.sStatus = "";
        //        return objEnum;
        //    }
        //}

        public void UpdateApproverpriority(string sUsertype, string sEnumid, string sUserId, string sRemarks, string sRework)
        {
            string sQry = string.Empty;
            DataTable dt = new DataTable();
            PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
            string[] Arr1 = new string[3];

            try
            {
                // unused 
                //if (sRework == "1")
                //{
                //    clsPreQCApproval objPreQc = new clsPreQCApproval();
                //    string sLevels = objPreQc.GetApprovalLevel(sUserType);
                //    string sFirstLevel = sLevels.Split('~').GetValue(1).ToString();

                //    //sQry = " UPDATE \"TBLENUMERATIONREMARKS\" SET \"ER_STATUS\" = 1 WHERE \"ER_ED_ID\" = '"+ sEnumid + "' ";
                //    //ObjCon.ExecuteQry(sQry);

                //    sQry = "INSERT INTO \"TBLENUMERATIONREMARKS\" (\"ER_ED_ID\", \"ER_REWORK\", \"ER_REMARKS\",\"ER_APPROVAL_LEVEL\",\"ER_CRBY\")";
                //    sQry += " VALUES ('" + sEnumid + "','" + sRework + "','" + sRemarks + "','" + sFirstLevel + "','" + sUserId + "')";
                //    ObjCon.ExecuteQry(sQry);
                //}
                //else
                //{
                //    sQry = " UPDATE \"TBLENUMERATIONREMARKS\" SET \"ER_STATUS\" = 1, \"ER_UPDATEON\" = NOW() WHERE \"ER_ED_ID\" = '" + sEnumid + "' ";
                //    ObjCon.ExecuteQry(sQry);
                //}


                // used working query

                //sQry = "INSERT INTO \"TBLINTERNALAPPROVALHISTORY\" (\"IA_ED_ID\",\"IA_ED_APPROVED_BY\",\"IA_ED_APPROVED_TYPE\") ";
                //sQry += " VALUES('" + sEnumid + "','" + sUserId + "','1')";
                //ObjCon.ExecuteQry(sQry);

                clsPreQCApproval obj = new clsPreQCApproval();
                //sQry = "UPDATE \"TBLENUMERATIONDETAILS\" SET \"ED_APPROVALPRIORITY\" = '" + obj.GetNextApprovalLevel(sUsertype) + "' ";
                //sQry += " WHERE \"ED_ID\" IN ( " + sEnumid + " )";
                //ObjCon.ExecuteQry(sQry);

                NpgsqlCommand cmd1 = new NpgsqlCommand("proc_update_approver_priority");
                cmd1.Parameters.AddWithValue("enum_id", Convert.ToString(sEnumid ?? ""));
                cmd1.Parameters.AddWithValue("user_id", Convert.ToString(sUserId ?? ""));
                cmd1.Parameters.AddWithValue("ed_approval_prior", Convert.ToString(obj.GetNextApprovalLevel(sUsertype) ?? ""));
                cmd1.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd1.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd1.Parameters.Add("pk_id", NpgsqlDbType.Text);
                cmd1.Parameters["msg"].Direction = ParameterDirection.Output;
                cmd1.Parameters["op_id"].Direction = ParameterDirection.Output;
                cmd1.Parameters["pk_id"].Direction = ParameterDirection.Output;
                Arr1[0] = "msg";
                Arr1[1] = "op_id";
                Arr1[2] = "pk_id";
                Arr1 = ObjCon.Execute(cmd1, Arr1, 3);
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        //Unused Method

        //public void UpdateReworkStatus(string sUsertype, string sEnumid, string sUserId, string sRework)
        //{
        //    string sQry = string.Empty;
        //    DataTable dt = new DataTable();
        //    PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
        //    try
        //    {
        //        //commented by sandeep 
        //        //if (sRemarks.Trim().Length > 0 || sRework == "1")
        //        if (sRework == "1")
        //        {
        //            clsPreQCApproval objPreQc = new clsPreQCApproval();
        //            string sLevels = objPreQc.GetApprovalLevel(sUsertype);
        //            string sFirstLevel = sLevels.Split('~').GetValue(0).ToString();

        //            sQry = " UPDATE \"TBLENUMERATIONREMARKS\" SET \"ER_STATUS\" = 1, \"ER_UPDATEON\" = NOW() WHERE \"ER_ED_ID\" = '" + sEnumid + "' ";
        //            ObjCon.ExecuteQry(sQry);

        //            sQry = "INSERT INTO \"TBLENUMERATIONREMARKS\" (\"ER_ED_ID\", \"ER_REWORK\",\"ER_APPROVAL_LEVEL\",\"ER_CRBY\")";
        //            sQry += " VALUES ('" + sEnumid + "','" + sRework + "','" + sFirstLevel + "','" + sUserId + "')";
        //            ObjCon.ExecuteQry(sQry);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}
        public string Gettempstatus(clsFieldEnumeration objFieldEnum)
        {
            PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
            DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);

            //string sQry = string.Empty;
            //sQry = " SELECT \"TE_TC_STATUS\" from \"TBLTEMPENUMERATIONDETAILS\" where \"TE_CRBY\" = '" + objFieldEnum.sCrBy + "' and \"TE_DTCCODE\"='" + objFieldEnum.stempdtccode + "'  ";
            //string stempstatus = objcon.get_value(sQry);
            //return stempstatus;

            NpgsqlCommand cmd = new NpgsqlCommand("sp_get_temp_status");

            cmd.Parameters.AddWithValue("crby", (objFieldEnum.sCrBy ?? ""));
            cmd.Parameters.AddWithValue("dtc_code", (objFieldEnum.stempdtccode ?? ""));
            string stempstatus = objDatabse.StringGetValue(cmd);
            return stempstatus;
        }

        //UnUsed Method

        //public string Generatetccode(clsFieldEnumeration ObjField)
        //{
        //    PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
        //    DataTable dtDetails = new DataTable();
        //    string tempdtrcode = string.Empty;
        //    try
        //    {
        //        string sQry = string.Empty;
        //        sQry = "SELECT max(\"TE_TC_CODE\") FROM \"TBLTEMPENUMERATIONDETAILS\"";
        //        tempdtrcode = objcon.get_value(sQry);
        //        if (tempdtrcode.Length == 0)
        //        {
        //            tempdtrcode = "H100001";
        //            sQry = "UPDATE  \"TBLTEMPENUMERATIONDETAILS\"  SET \"TE_TC_CODE\"='" + tempdtrcode + "' WHERE \"TE_DTCCODE\"='" + ObjField.stempdtccode + "'";
        //            objcon.ExecuteQry(sQry);
        //        }
        //        else
        //        {
        //            sQry = " SELECT \"TE_TC_CODE\" from \"TBLTEMPENUMERATIONDETAILS\" where \"TE_CRBY\" = '" + ObjField.sCrBy + "' and \"TE_TC_STATUS\"='0' and \"TE_DEVICEID\"='" + ObjField.sClientIP + "' and \"TE_DTCCODE\"='" + ObjField.stempdtccode + "'  ";
        //            string sResult1 = objcon.get_value(sQry);
        //            if (sResult1 != "")
        //            {
        //                tempdtrcode = sResult1.Split('~').GetValue(0).ToString();
        //            }
        //            else
        //            {
        //                //tempdtrcode = Convert.ToString(objcon.Get_max_no("TE_TC_CODE", "TBLTEMPENUMERATIONDETAILS"));
        //                NpgsqlCommand cmd = new NpgsqlCommand("proc_dtrcode_autogen");
        //                dtDetails = objcon.FetchDataTable(cmd);
        //                if (dtDetails.Rows.Count > 0)
        //                {
        //                    tempdtrcode = Convert.ToString(dtDetails.Rows[0]["proc_dtrcode_autogen"]);
        //                }
        //                sQry = "UPDATE  \"TBLTEMPENUMERATIONDETAILS\"  SET \"TE_TC_CODE\"='" + tempdtrcode + "' WHERE \"TE_DTCCODE\"='" + ObjField.stempdtccode + "'";
        //                objcon.ExecuteQry(sQry);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //    return tempdtrcode;
        //}

        //Unused Method
        //public string GeneratefeederCode(clsFieldEnumeration ObjField)
        //{
        //    PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
        //    string tempdtccode = string.Empty;
        //    string tempdtrcode = string.Empty;

        //    try
        //    {
        //        string sQry = string.Empty;

        //        sQry = "SELECT max(\"TE_DTCCODE\") FROM \"TBLTEMPENUMERATIONDETAILS\"";
        //        tempdtccode = objcon.get_value(sQry);
        //        string Te_id = Convert.ToString(objcon.Get_max_no("TE_ID", "TBLTEMPENUMERATIONDETAILS"));
        //        if (tempdtccode.Length == 0)
        //        {
        //            tempdtccode = "HAAAAA";
        //            sQry = "INSERT INTO \"TBLTEMPENUMERATIONDETAILS\" (\"TE_ID\",\"TE_CRBY\",\"TE_DTCCODE\",\"TE_TC_STATUS\",\"TE_DEVICEID\",\"TE_SOURCE\",\"TE_OFFICECODE\")";
        //            sQry += " VALUES ('" + Te_id + "','" + ObjField.sCrBy + "','" + tempdtccode + "','0','" + ObjField.sClientIP + "','WEB','" + ObjField.sOfficeCode + "')";
        //            objcon.ExecuteQry(sQry);
        //        }
        //        else
        //        {
        //            sQry = " SELECT max(\"TE_DTCCODE\") from \"TBLTEMPENUMERATIONDETAILS\" where \"TE_CRBY\" = '" + ObjField.sCrBy + "' and \"TE_TC_STATUS\"='0' and \"TE_DEVICEID\"='" + ObjField.sClientIP + "' ";
        //            string sResult1 = objcon.get_value(sQry);
        //            if (sResult1 != "")
        //            {
        //                tempdtccode = sResult1.Split('~').GetValue(0).ToString();
        //            }

        //            else
        //            {

        //                string charset = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        //                //A a = new A();
        //                clsFeederMast objautoincrement = new clsFeederMast();
        //                string str = tempdtccode;
        //                char[] c = str.ToCharArray();
        //                for (int i = c.Length - 1; i > 0; i--)
        //                {
        //                    bool cont = false;
        //                    string checkSameChar = str.Substring(i - 1);

        //                    if (objautoincrement.CheckAllChar(checkSameChar))
        //                    {
        //                        continue;
        //                    }
        //                    if (c[i] != 'Z')
        //                    {
        //                        int IncrementedIndex = i;
        //                        int index = objautoincrement.GetDigitValue(c[i]);
        //                        char ch = charset[index + 1];
        //                        c[i] = ch;
        //                        string res = new string(c);
        //                        tempdtccode = res;
        //                        break;
        //                    }
        //                    else
        //                    {
        //                        string substr = str.Substring(i - 1);
        //                        int index = objautoincrement.GetDigitValue(c[i]);
        //                        if (index != 25)
        //                        {
        //                            c[i - 1] = charset[index + 1];
        //                        }

        //                        for (int j = i; j < str.Length; j++)//ABZBAA   ABZBBA
        //                        {

        //                            int ind = objautoincrement.GetDigitValue(c[j - 1]);
        //                            string s = str.Substring(j);
        //                            for (int k = 0; k < s.Length; k++)
        //                            {
        //                                c[j + k] = 'A';
        //                            }
        //                            c[j - 1] = charset[ind + 1];

        //                            break;
        //                        }
        //                        string res = new string(c);
        //                        tempdtccode = res;
        //                        break;

        //                    }
        //                }



        //                //tempdtrcode = Convert.ToString(objcon.Get_max_no("TE_TC_CODE", "TBLTEMPENUMERATIONDETAILS"));

        //                sQry = "INSERT INTO \"TBLTEMPENUMERATIONDETAILS\" (\"TE_ID\",\"TE_CRBY\",\"TE_DTCCODE\",\"TE_TC_STATUS\",\"TE_DEVICEID\",\"TE_SOURCE\",\"TE_OFFICECODE\")";
        //                sQry += " VALUES ('" + Te_id + "','" + ObjField.sCrBy + "','" + tempdtccode + "','0','" + ObjField.sClientIP + "','WEB','" + ObjField.sOfficeCode + "')";
        //                objcon.ExecuteQry(sQry);
        //                //}
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //    return tempdtccode;
        //}

        // UnUsed Method

        //public string GetReworkdetails(string sEnumId)
        //{
        //    try
        //    {
        //        PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
        //        string sQry = string.Empty;
        //        sQry = "SELECT \"ER_REWORK\" || '~' || \"ER_REMARKS\" FROM \"TBLENUMERATIONREMARKS\" WHERE \"ER_ID\" = (SELECT MAX(\"ER_ID\") FROM \"TBLENUMERATIONREMARKS\" ";
        //        sQry += " WHERE \"ER_ED_ID\" = '" + sEnumId + "' AND \"ER_STATUS\" = 0)";
        //        string sDetails = ObjCon.get_value(sQry);
        //        return sDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return "";
        //    }
        //}

        public string[] CheckMakeSerialNoOfTC(string sTcMakeId, string sTcSlNo, string sTcCode)
        {
            string[] Arr = new string[2];
            try
            {
                DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
                PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
                string sQry = string.Empty;
                string Res = string.Empty;


                //Res = ObjCon.get_value("select \"TC_SLNO\" from \"TBLTCMASTER\" where cast(\"TC_MAKE_ID\" as text)='" + sTcMakeId + "' and \"TC_SLNO\"='" + sTcSlNo + "' and \"TC_CODE\"<>'" + sTcCode + "'");

                NpgsqlCommand cmd = new NpgsqlCommand("proc_check_make_slno_of_tc");

                cmd.Parameters.AddWithValue("make_id", (sTcMakeId ?? ""));
                cmd.Parameters.AddWithValue("tc_slno", (sTcSlNo ?? ""));
                cmd.Parameters.AddWithValue("tc_code", (sTcCode ?? ""));
                Res = objDatabse.StringGetValue(cmd);

                if (Res != "")
                {
                    Arr[0] = "Combination of DTr Make and DTr SlNo Already Exist.";
                    Arr[1] = "2";
                }
                else
                {
                    Arr[0] = "";
                    Arr[1] = "0";
                }
                return Arr;
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }

    }
}
