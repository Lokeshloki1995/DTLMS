using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using System.Data;
using System.Data.OleDb;
using IIITS.PGSQL.DAL;

namespace IIITS.DTLMS.BL
{
    public class clsStoreEnumeration
    {
       
        string strFormCode = "clsStoreEnumeration";
       

        public string sEnumDetailsId { get; set; }
        public string sEnumDTCId { get; set; }

        public string sCrBy { get; set; }

        public string sDivCode { get; set; }
        public string sLocName { get; set; }
        public string sLocAdd { get; set; }
        public string sLoctype { get; set; }
        public int sLoctypeInt { get; set; }
        public string sWeldDate { get; set; }
        public string sTcCode { get; set; }
        public string sTCSlno { get; set; }
        public string sTCCapacity { get; set; }
        public string sTCManfDate { get; set; }
        public string sTCMake { get; set; }
        public string sTCType { get; set; }
        public string sEnumDate { get; set; }
        public string sOperator1 { get; set; }
        public string sOperator2 { get; set; }
        public string sAddress { get; set; }
        public string sValue { get; set; }
        public string sSelectedValue { get; set; }
        public string sEnumType { get; set; }
        public string sMakeName { get; set; }

        public string sTankCapacity { get; set; }
        public string sTCWeight { get; set; }
        public string sRating { get; set; }
        public string sStarRate { get; set; }

        public bool bTCSlNoNotExists { get; set; }
        public string sNamePlatePhotoPath { get; set; }
        public string sSSPlatePhotoPath { get; set; }

        public string sStatus { get; set; }
        public string sApproveStatus { get; set; }
        public string sUserType { get; set; }

        public string sTaggedDTR { get; set; }

        public string sSpecialCase { get; set; }

        public string sPriorityLevel { get; set; }
        public string sClientIP { get; set; }

        public string smdid { get; set; }
        public string smdname { get; set; }
        public string sofficecode { get; set; }
        
        public string[] SaveStoreEnumerationDetails(clsStoreEnumeration objEnumeration)
        {
            string[] Arr = new string[2];
            string strQry = string.Empty;
            string sRes = string.Empty;            
            PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);

            try
            {
                strQry = "SELECT \"TG_DTE_TC_CODE\" FROM \"TBLTAGGEDDTR\" WHERE \"TG_DTE_TC_CODE\" =  '" + objEnumeration.sTcCode + "' and \"TG_ED_STATUS_FLAG\" =  1 ";
                sRes = ObjCon.get_value(strQry);

                if (sRes == "" || sRes == null)
                {
                    
                    //strQry = "SELECT \"VM_ID\" || '~' ||\"VM_NAME\" FROM \"TBLTCPLATEALLOCATIONMASTER\",\"TBLTCPLATEALLOCATION\",\"TBLVENDORMASTER\" WHERE \"TCPM_ID\"=\"TCP_TCPM_ID\" AND ";
                    //strQry += " \"VM_ID\" =\"TCPM_VENDOR_ID\" AND \"TCP_TC_CODE\"='" + objEnumeration.sTcCode + "'";
                    //string sVmDetails = ObjCon.get_value(strQry);

                    //if (sVmDetails.Length == 0)
                    //{
                    //    Arr[0] = "Plate Number " + objEnumeration.sTcCode + "  Not Exist in the Allocation";
                    //    Arr[1] = "2";
                    //    return Arr;
                    //}

           
                    //strQry = "SELECT \"IU_VENDOR_ID\" FROM \"TBLINTERNALUSERS\" WHERE \"IU_ID\"='" + objEnumeration.sOperator1 + "'";
                    //string sLogUs_VM_Id = ObjCon.get_value(strQry);

                    //if (sLogUs_VM_Id == sVmDetails.Split('~').GetValue(0).ToString())
                   
                        //strQry = "SELECT * FROM \"TBLTCPLATEALLOCATIONMASTER\",\"TBLTCPLATEALLOCATION\" WHERE \"TCPM_ID\"=\"TCP_TCPM_ID\" AND \"TCP_STATUS_FLAG\" IN (0,4) AND ";
                        //strQry += " \"TCP_TC_CODE\" ='" + objEnumeration.sTcCode + "'";
                        //string res = ObjCon.get_value(strQry);

                        //if (res == null)
                        //{
                        //    Arr[0] = "Plate Number " + objEnumeration.sTcCode + "  Not Possible to save because entered tc code may be Approved, deleted or Rejected ";
                        //    Arr[1] = "2";
                        //    return Arr;
                        //}
                    
                    //else
                    //{
                    //    Arr[0] = "Plate Number " + objEnumeration.sTcCode + "  Already allocated to vendor " + sVmDetails.Split('~').GetValue(1).ToString();
                    //    Arr[1] = "2";
                    //    return Arr;
                    //}
                }

                if (objEnumeration.sEnumDetailsId == "")
                {
                    sRes = ObjCon.get_value("SELECT * FROM \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\" WHERE \"DTE_TC_CODE\"='" + objEnumeration.sTcCode + "' AND \"ED_ID\"=\"DTE_ED_ID\" AND \"ED_STATUS_FLAG\" NOT IN ('3','5')");
                    if (sRes != "")
                    {
                        Arr[0] = "Plate Number " + objEnumeration.sTcCode + "  Already Exist";
                        Arr[1] = "2";                        
                        return Arr;
                    }

                    if (objEnumeration.sTCMake != "")
                    {
                        sRes = ObjCon.get_value("SELECT * FROM \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\" WHERE \"DTE_MAKE\"='" + objEnumeration.sTCMake + "' AND \"DTE_TC_SLNO\"='" + objEnumeration.sTCSlno + "' AND \"ED_ID\"=\"DTE_ED_ID\" AND \"ED_STATUS_FLAG\" NOT IN ('3','5')");
                        if (sRes != "")
                        {
                            Arr[0] = "Combination of Transformer Sl No " + objEnumeration.sTCSlno + " and Make Name  " + objEnumeration.sMakeName + " Already Exist";
                            Arr[1] = "2";
                            return Arr;
                        }
                    }

                    //  objEnumeration.sSpecialCase /ED_ENUMERATION_CASE  = 1 : Tagged DTR .

                    if (objEnumeration.sTaggedDTR == "1")
                    {
                        objEnumeration.sSpecialCase = "1";
                    }
                    ObjCon.BeginTransaction();
                    objEnumeration.sEnumDetailsId = Convert.ToString(ObjCon.Get_max_no("ED_ID", "TBLENUMERATIONDETAILS"));

                    //strQry = "INSERT INTO \"TBLENUMERATIONDETAILS\"(\"ED_ID\",\"ED_OFFICECODE\",\"ED_ENUM_TYPE\",\"ED_LOCTYPE\",\"ED_LOCNAME\",\"ED_LOCADDRESS\",\"ED_OPERATOR1\",\"ED_OPERATOR2\",\"ED_WELD_DATE\",\"ED_CRBY\",\"ED_CRON\",\"ED_ENUMERATION_CASE\" , \"ED_APPROVE_STATUS\") ";
                    //strQry += " VALUES ('" + objEnumeration.sEnumDetailsId + "','" + objEnumeration.sDivCode + "','" + objEnumeration.sLoctype + "','" + objEnumeration.sLoctype + "','" + objEnumeration.sLocName + "', ";
                    //strQry += " '" + objEnumeration.sLocAdd + "','" + objEnumeration.sOperator1 + "', ";
                    //strQry += " '" + objEnumeration.sOperator2 + "',TO_DATE('" + objEnumeration.sWeldDate + "','dd/mm/yyyy'),'" + objEnumeration.sCrBy + "',now(),  '"+objEnumeration.sTaggedDTR + "' , ";

                    strQry = "INSERT INTO \"TBLENUMERATIONDETAILS\"(\"ED_ID\",\"ED_OFFICECODE\",\"ED_ENUM_TYPE\",\"ED_LOCTYPE\",\"ED_LOCNAME\",\"ED_LOCADDRESS\",\"ED_WELD_DATE\",\"ED_CRBY\",\"ED_CRON\",\"ED_ENUMERATION_CASE\" , \"ED_APPROVE_STATUS\") ";
                    strQry += " VALUES ('" + objEnumeration.sEnumDetailsId + "','" + objEnumeration.sDivCode + "','" + objEnumeration.sLoctype + "','" + objEnumeration.sLoctype + "','" + objEnumeration.sLocName + "', ";
                    strQry += " '" + objEnumeration.sLocAdd + "', ";
                    strQry += " TO_DATE('" + objEnumeration.sWeldDate + "','dd/mm/yyyy'),'" + objEnumeration.sCrBy + "',now(),  '" + objEnumeration.sTaggedDTR + "' , ";

                    if (objEnumeration.sUserType == "1" || objEnumeration.sUserType == "3" || objEnumeration.sUserType == "5" )
                    {
                        strQry += "'1')";
                    }
                    else if (objEnumeration.sUserType == "6")
                    {
                        strQry += "'2')";
                    }
                    else if (objEnumeration.sUserType == "2" || objEnumeration.sUserType == "4")
                    {
                        strQry += "'3')";
                    }
                    else if(objEnumeration.sUserType == null)
                    {
                        strQry += "'1')";
                    }

                    ObjCon.ExecuteQry(strQry);

                    strQry = "UPDATE \"TBLTEMPENUMERATIONDETAILS\" SET \"TE_TC_STATUS\" = '1' WHERE \"TE_TC_CODE\" = '" + objEnumeration.sTcCode + "' AND \"TE_CRBY\" = '" + objEnumeration.sCrBy + "' "; 
                    ObjCon.ExecuteQry(strQry);

                    objEnumeration.sEnumDTCId = Convert.ToString(ObjCon.Get_max_no("DTE_ID", "TBLDTCENUMERATION"));                    

                    if(objEnumeration.sTCManfDate == null)
                    {
                        strQry = "INSERT INTO \"TBLDTCENUMERATION\" (\"DTE_ID\",\"DTE_ED_ID\",\"DTE_TC_CODE\",\"DTE_MAKE\",\"DTE_CAPACITY\",\"DTE_TC_TYPE\",\"DTE_TC_SLNO\",\"DTE_CRBY\",";
                        strQry += " \"DTE_CRON\",\"DTE_TANK_CAPACITY\",\"DTE_TC_WEIGHT\",\"DTE_RATING\",\"DTE_STAR_RATE\")";
                        strQry += " VALUES ('" + objEnumeration.sEnumDTCId + "','" + objEnumeration.sEnumDetailsId + "','" + objEnumeration.sTcCode + "', ";
                        strQry += " '" + objEnumeration.sTCMake + "'," + objEnumeration.sTCCapacity + ", ";
                        strQry += " " + objEnumeration.sTCType + ",'" + objEnumeration.sTCSlno + "',";
                        strQry += " '" + objEnumeration.sCrBy + "',now(),'" + objEnumeration.sTankCapacity + "','" + objEnumeration.sTCWeight + "',";
                        strQry += " '" + objEnumeration.sRating + "','" + objEnumeration.sStarRate + "')";
                    }
                    else
                    {
                        strQry = "INSERT INTO \"TBLDTCENUMERATION\" (\"DTE_ID\",\"DTE_ED_ID\",\"DTE_TC_CODE\",\"DTE_MAKE\",\"DTE_CAPACITY\",\"DTE_TC_TYPE\",\"DTE_TC_SLNO\",\"DTE_TC_MANFDATE\",\"DTE_CRBY\",";
                        strQry += " \"DTE_CRON\",\"DTE_TANK_CAPACITY\",\"DTE_TC_WEIGHT\",\"DTE_RATING\",\"DTE_STAR_RATE\")";
                        strQry += " VALUES ('" + objEnumeration.sEnumDTCId + "','" + objEnumeration.sEnumDetailsId + "','" + objEnumeration.sTcCode + "', ";
                        strQry += " '" + objEnumeration.sTCMake + "'," + objEnumeration.sTCCapacity + ", ";
                        strQry += " " + objEnumeration.sTCType + ",'" + objEnumeration.sTCSlno + "',TO_DATE('" + objEnumeration.sTCManfDate + "','dd/MM/yyyy'),";
                        strQry += " '" + objEnumeration.sCrBy + "',now(),'" + objEnumeration.sTankCapacity + "','" + objEnumeration.sTCWeight + "',";
                        strQry += " '" + objEnumeration.sRating + "','" + objEnumeration.sStarRate + "')";
                    }
                    ObjCon.ExecuteQry(strQry);

                    // insert into TBLTAGGEDDTR if DTR is tagged .
                    if (objEnumeration.sTaggedDTR == "1")
                    {
                       string staggedDTRId = Convert.ToString(ObjCon.Get_max_no("TG_ID", "TBLTAGGEDDTR"));
                        strQry = "INSERT into \"TBLTAGGEDDTR\" (\"TG_ID\", \"TG_ED_ID\" , \"TG_ED_OFFICECODE\" ,  \"TG_ED_LOCTYPE\", \"TG_ED_LOCADDRESS\" , \"TG_ED_LOCNAME\" , ";
                        strQry += "\"TG_ED_STATUS_FLAG\", \"TG_ED_CRBY\" ,\"TG_ED_CRON\", \"TG_DTE_TC_CODE\",\"TG_TC_MAKE\", \"TG_DTE_TC_SLNO\" , \"TG_DTE_CAPACITY\" , \"TG_DTE_TC_TYPE\" , \"TG_DTE_TC_WEIGHT\" ) ";
                        strQry += " VALUES (" + staggedDTRId + " , " + objEnumeration.sEnumDetailsId + " ," + objEnumeration.sDivCode + " , 1 , '" + objEnumeration.sLocName + "' , ";
                        strQry += " '" + objEnumeration.sLocAdd + "' ,0 , " + objEnumeration.sCrBy + ", now() , " + objEnumeration.sTcCode + " ," + objEnumeration.sTCMake + " ,'" + objEnumeration.sTCSlno + "', ";
                        strQry += "'"+ objEnumeration.sTCCapacity + "'  , '"+ objEnumeration.sTCType +"' , '" + objEnumeration.sTCWeight + "') ";
                         ObjCon.ExecuteQry(strQry);
                      
                        
                    }


                    ObjCon.CommitTransaction();


                    Arr[0] = "Store Enumeration Details Saved Successfully";
                    Arr[1] = "0";
                    ObjCon.close();
                    return Arr;
                }


                else
                {
                    ObjCon.BeginTransaction();
                    // by pass here

                    strQry = "SELECT \"TG_DTE_TC_CODE\" FROM \"TBLTAGGEDDTR\" WHERE \"TG_DTE_TC_CODE\" =  '" + objEnumeration.sTcCode + "' and \"TG_ED_STATUS_FLAG\" =  1 ";
                    sRes = ObjCon.get_value(strQry);

                    if (sRes == "" || sRes == null)
                    {

                        sRes = ObjCon.get_value("SELECT * FROM \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\" WHERE \"DTE_TC_CODE\"='" + objEnumeration.sTcCode + "' AND \"DTE_ED_ID\" <>'" + objEnumeration.sEnumDetailsId + "' AND \"ED_ID\"=\"DTE_ED_ID\" AND \"ED_STATUS_FLAG\" NOT IN ('3','5')");
                        if (sRes != "")
                        {
                            Arr[0] = "Plate Number " + objEnumeration.sTcCode + "  Already Exist";
                            Arr[1] = "2";
                            return Arr;
                        }


                        sRes = ObjCon.get_value("SELECT * FROM \"TBLDTCENUMERATION\",\"TBLENUMERATIONDETAILS\" WHERE \"DTE_MAKE\"='" + objEnumeration.sTCMake + "' AND \"DTE_TC_SLNO\"='" + objEnumeration.sTCSlno + "' AND \"DTE_ED_ID\" <>'" + objEnumeration.sEnumDetailsId + "' AND \"ED_ID\"=\"DTE_ED_ID\" AND \"ED_STATUS_FLAG\" NOT IN ('3','5')");
                        if (sRes != "")
                        {
                            Arr[0] = "Combination of Transformer Sl No " + objEnumeration.sTCSlno + " and Make Name  " + objEnumeration.sMakeName + " Already Exist";
                            Arr[1] = "2";
                            return Arr;
                        }
                    }
                    //strQry = "UPDATE \"TBLENUMERATIONDETAILS\" SET \"ED_OFFICECODE\"='" + objEnumeration.sDivCode + "',\"ED_ENUM_TYPE\"='" + objEnumeration.sLoctype + "',";
                    //strQry += " \"ED_LOCTYPE\"='" + objEnumeration.sLoctype + "',\"ED_LOCNAME\"='" + objEnumeration.sLocName + "',\"ED_LOCADDRESS\"= ";
                    //strQry += " '" + objEnumeration.sLocAdd + "',\"ED_OPERATOR1\"='" + objEnumeration.sOperator1 + "',\"ED_OPERATOR2\"='" + objEnumeration.sOperator2 + "', ";
                    //strQry += " \"ED_WELD_DATE\"=TO_DATE('" + objEnumeration.sWeldDate + "','dd/mm/yyyy'),\"ED_UPDATE_BY\"='" + objEnumeration.sCrBy + "', ";
                    //strQry += " \"ED_UPDATE_ON\"=now() ,";

                    strQry = "UPDATE \"TBLENUMERATIONDETAILS\" SET \"ED_OFFICECODE\"='" + objEnumeration.sDivCode + "',\"ED_ENUM_TYPE\"='" + objEnumeration.sLoctype + "',";
                    strQry += " \"ED_LOCTYPE\"='" + objEnumeration.sLoctype + "',\"ED_LOCNAME\"='" + objEnumeration.sLocName + "',\"ED_LOCADDRESS\"= ";
                    strQry += " '" + objEnumeration.sLocAdd + "', ";
                    strQry += " \"ED_WELD_DATE\"=TO_DATE('" + objEnumeration.sWeldDate + "','dd/mm/yyyy'),\"ED_UPDATE_BY\"='" + objEnumeration.sCrBy + "', ";
                    strQry += " \"ED_UPDATE_ON\"=now() ,";

                    //if (objEnumeration.sUserType == "1" || objEnumeration.sUserType == "3" || objEnumeration.sUserType == "5")
                    //{
                    //    strQry += "\"ED_APPROVE_STATUS\" = '1'";
                    //}
                    //else if (objEnumeration.sUserType == "6")
                    //{
                    //    strQry += "\"ED_APPROVE_STATUS\" = '2'";
                    //}
                    //else if (objEnumeration.sUserType == "2" || objEnumeration.sUserType == "4")
                    //{
                    //    strQry += "\"ED_APPROVE_STATUS\" = '3'";
                    //}

                    clsPreQCApproval objpreQC = new clsPreQCApproval();
                    string sLevel = string.Empty;
                    //sLevel = objpreQC.GetNextApprovalLevel(objEnumeration.sUserType);
                    //strQry += "\"ED_APPROVALPRIORITY\" = '" + sLevel + "'";
                    strQry += "\"ED_APPROVALPRIORITY\" = '" + 5 + "'";

                    strQry += " WHERE \"ED_ID\"='" + objEnumeration.sEnumDetailsId + "'";
                    ObjCon.ExecuteQry(strQry);


                    if (objEnumeration.sUserType == "6" || objEnumeration.sUserType == "7" || objEnumeration.sUserType == "8")
                    {
                        string Qry = "INSERT INTO \"TBLINTERNALAPPROVALHISTORY\" (\"IA_ED_ID\",\"IA_ED_APPROVED_BY\",\"IA_ED_APPROVED_TYPE\") ";
                        Qry += " VALUES('" + objEnumeration.sEnumDetailsId + "','" + objEnumeration.sCrBy + "','0')";
                        ObjCon.ExecuteQry(Qry);
                    }

                    strQry = "UPDATE \"TBLDTCENUMERATION\" SET \"DTE_TC_CODE\"='" + objEnumeration.sTcCode + "',\"DTE_MAKE\"='" + objEnumeration.sTCMake + "',";
                    strQry += " \"DTE_CAPACITY\"='" + objEnumeration.sTCCapacity + "',\"DTE_TC_TYPE\"='" + objEnumeration.sTCType + "',\"DTE_TC_SLNO\"= ";
                    strQry += " '" + objEnumeration.sTCSlno + "',\"DTE_TC_MANFDATE\"=TO_DATE('" + objEnumeration.sTCManfDate + "','dd/MM/yyyy'), ";
                    strQry += " \"DTE_TANK_CAPACITY\"='" + objEnumeration.sTankCapacity + "',\"DTE_TC_WEIGHT\"='" + objEnumeration.sTCWeight + "', ";
                    strQry += " \"DTE_RATING\"='" + objEnumeration.sRating + "',\"DTE_STAR_RATE\"='" + objEnumeration.sStarRate + "' WHERE \"DTE_ED_ID\"='" + objEnumeration.sEnumDetailsId + "'";
                    ObjCon.ExecuteQry(strQry);

                    if (objEnumeration.sStatus == "2")
                    {
                        strQry = "UPDATE \"TBLENUMERATIONDETAILS\" SET \"ED_STATUS_FLAG\"='0' WHERE \"ED_ID\"='" + objEnumeration.sEnumDetailsId + "'";
                        ObjCon.ExecuteQry(strQry);
                    }


                    string sQry = "SELECT \"ED_INITIAL_UPDATE_ON\" FROM \"TBLENUMERATIONDETAILS\" WHERE \"ED_ID\"='" + objEnumeration.sEnumDetailsId + "'";
                    string sLastUpdate = ObjCon.get_value(sQry);
                    if (sLastUpdate.Length == 0)
                    {
                        sQry = "UPDATE \"TBLENUMERATIONDETAILS\" SET \"ED_INITIAL_UPDATE_BY\" = '" + objEnumeration.sCrBy + "',  \"ED_INITIAL_UPDATE_ON\" = NOW() WHERE \"ED_ID\"='" + objEnumeration.sEnumDetailsId + "' ";
                        ObjCon.ExecuteQry(sQry);
                    }

                    //coded by pradeep - start
                    //strQry = "UPDATE \"TBLTCPLATEALLOCATION\" SET  \"TCP_STATUS_FLAG\" = '1' WHERE \"TCP_TC_CODE\" ='" + objEnumeration.sTcCode + "'";
                    //ObjCon.ExecuteQry(strQry);
                    //coded by pradeep - end


                    ObjCon.CommitTransaction();
                    ObjCon.close();
                    Arr[0] = "Store Enumeration Details Updated Successfully";
                    Arr[1] = "1";
                    return Arr;
                }

            }
            catch (Exception ex)
            {
                ObjCon.RollBackTrans();
                //clsException.LogError(ex.StackTrace, ex.Message + strQry, strFormCode, "SaveStoreEnumerationDetails");
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }

        public DataTable LoadStoreEnumeration(string sOperator = "")
        {
           
            DataTable dt = new DataTable();
            try
            {
                PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
                string strQry = string.Empty;


                strQry = "select  \"ED_ID\", \"DTE_ED_ID\",(select \"MD_NAME\" from \"TBLMASTERDATA\" where \"MD_ID\"= \"ED_LOCTYPE\" and \"MD_TYPE\"='TCL') \"ED_LOCTYPE\",";
                strQry += "  \"ED_LOCNAME\",TO_CHAR(\"ED_WELD_DATE\",'dd/MM/yyyy') \"ED_WELD_DATE\",(SELECT  \"TM_NAME\" FROM  \"TBLTRANSMAKES\" WHERE  \"DTE_MAKE\"= \"TM_ID\") MAKE, \"DTE_CAPACITY\" from ";
                strQry += "  \"TBLENUMERATIONDETAILS\", \"TBLDTCENUMERATION\" where  \"ED_ID\"= \"DTE_ED_ID\" and  \"ED_ENUM_TYPE\" IN ('1','3','5') and  \"ED_CANCEL_FLAG\"='0' ";
                if (sOperator != "")
                {
                    strQry += " AND  (\"ED_OPERATOR1\"='" + sOperator + "' OR \"ED_OPERATOR2\"='" + sOperator + "')";
                }
                 return ObjCon.FetchDataTable(strQry);

             //   return dt;

            }
            catch (Exception ex)
            {
                //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadStoreEnumeration");
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        public clsStoreEnumeration GetStoreEnumerationDetails(clsStoreEnumeration objstore)
        {
            try
            {
                PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
                string strQry = string.Empty;
                strQry= "SELECT \"ED_OFFICECODE\",\"ED_LOCNAME\",\"ED_LOCADDRESS\",\"ED_LOCTYPE\",TO_CHAR(\"ED_WELD_DATE\",'dd/mm/yyyy') \"ED_WELD_DATE\", \"ED_APPROVE_STATUS\",\"ED_ENUMERATION_CASE\",\"DTE_TC_CODE\",\"DTE_MAKE\",\"DTE_TC_SLNO\",TO_CHAR(\"DTE_TC_MANFDATE\",'dd/MM/yyyy') AS \"DTE_TC_MANFDATE\",\"DTE_CAPACITY\" ";
                strQry += " ,\"DTE_TC_TYPE\",To_char(\"DTE_ENUM_DATE\",'dd/mm/yyyy') \"DTE_ENUM_DATE\",\"ED_OPERATOR1\",\"ED_OPERATOR2\", \"DTE_TANK_CAPACITY\",\"DTE_TC_WEIGHT\",\"DTE_RATING\",\"DTE_STAR_RATE\",\"EP_NAMEPLATE_PATH\",\"EP_SSPLATE_PATH\",\"ED_APPROVALPRIORITY\" from \"TBLENUMERATIONDETAILS\",";
                strQry += "\"TBLDTCENUMERATION\",\"TBLENUMERATIONPHOTOS\" where  \"ED_ID\" = \"DTE_ED_ID\" and  \"ED_ID\"=\"EP_ED_ID\" AND \"ED_ID\"='" + objstore.sEnumDetailsId + "' ";
                DataTable dt = new DataTable();
                dt = ObjCon.FetchDataTable(strQry);
                if (dt.Rows.Count > 0)
                {
                    objstore.sDivCode = Convert.ToString(dt.Rows[0]["ED_OFFICECODE"]);
                    objstore.sLocName = Convert.ToString(dt.Rows[0]["ED_LOCNAME"]);
                    objstore.sLocAdd = Convert.ToString(dt.Rows[0]["ED_LOCADDRESS"]);
                    objstore.sLoctype = Convert.ToString(dt.Rows[0]["ED_LOCTYPE"]);
                    objstore.sWeldDate = Convert.ToString(dt.Rows[0]["ED_WELD_DATE"]);
                    objstore.sTcCode = Convert.ToString(dt.Rows[0]["DTE_TC_CODE"]);
                    objstore.sTCMake = Convert.ToString(dt.Rows[0]["DTE_MAKE"]);
                    objstore.sTCSlno = Convert.ToString(dt.Rows[0]["DTE_TC_SLNO"]);
                    objstore.sTCManfDate = Convert.ToString(dt.Rows[0]["DTE_TC_MANFDATE"]);
                    objstore.sTCCapacity = Convert.ToString(dt.Rows[0]["DTE_CAPACITY"]);
                    objstore.sTCType = Convert.ToString(dt.Rows[0]["DTE_TC_TYPE"]);
                    objstore.sOperator1 = Convert.ToString(dt.Rows[0]["ED_OPERATOR1"]);
                    objstore.sOperator2 = Convert.ToString(dt.Rows[0]["ED_OPERATOR2"]);
                    objstore.sTaggedDTR = Convert.ToString(dt.Rows[0]["ED_ENUMERATION_CASE"]);

                    if (objstore.sTaggedDTR != "0")
                    {
                        objstore.sSpecialCase = Convert.ToString(dt.Rows[0]["ED_ENUMERATION_CASE"]);
                    }

                    objstore.sTankCapacity = Convert.ToString(dt.Rows[0]["DTE_TANK_CAPACITY"]);
                    objstore.sTCWeight = Convert.ToString(dt.Rows[0]["DTE_TC_WEIGHT"]);

                    objstore.sRating = Convert.ToString(dt.Rows[0]["DTE_RATING"]);
                    objstore.sStarRate = Convert.ToString(dt.Rows[0]["DTE_STAR_RATE"]);

                    objstore.sNamePlatePhotoPath = Convert.ToString(dt.Rows[0]["EP_NAMEPLATE_PATH"]);
                    objstore.sSSPlatePhotoPath = Convert.ToString(dt.Rows[0]["EP_SSPLATE_PATH"]);
                    objstore.sApproveStatus = Convert.ToString(dt.Rows[0]["ED_APPROVE_STATUS"]);
                    objstore.sPriorityLevel = Convert.ToString(dt.Rows[0]["ED_APPROVALPRIORITY"]);
                }
                return objstore;
            }
            catch (Exception ex)
            {
                //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetDetails");
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objstore;
            }
        }

        public bool DeleteEnumerationDetails(clsStoreEnumeration objFieldEnum)
        {
            try
            {
                PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
                string strQry = string.Empty;
                strQry = "UPDATE \"TBLENUMERATIONDETAILS\" SET \"ED_CANCEL_FLAG\"='1' WHERE \"ED_ID\"='" + objFieldEnum.sEnumDetailsId + "'";
                ObjCon.ExecuteQry(strQry);
                return true;
            }
            catch (Exception ex)
            {
                //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "DeleteEnumerationDetails");
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }

        public string GetAddress(clsStoreEnumeration objstore)
        {
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();
                PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
                if( objstore.sLoctype=="1" || objstore.sLoctype == "5")
                {
                    //strQry = "SELECT \"SM_ID\" || '~' || \"STO_OFF_CODE\",\"DIV_NAME\" FROM \"TBLSTOREMAST\", \"TBLSTOREOFFCODE\",  \"TBLDIVISION\" WHERE \"SM_ID\" = \"STO_SM_ID\" And \"SM_ID\" = '" + objSession.OfficeCode + "'  AND \"STO_OFF_CODE\" = \"DIV_CODE\" ORDER BY \"DIV_NAME\"";
                    //ObjCon.Fetch(strQry);
                }
                if (objstore.sValue == "3")
                {
                    strQry = "select \"TR_ADDRESS\" as ADDRESS from \"TBLTRANSREPAIRER\" where \"TR_ID\"='" + objstore.sSelectedValue + "'";
                }
                else if (objstore.sValue == "1")
                {
                    strQry = "select \"SM_ADDRESS\"  as ADDRESS from \"TBLSTOREMAST\" where \"SM_ID\"='" + objstore.sSelectedValue + "'";
                }               
              return ObjCon.get_value(strQry);
            }

            catch (Exception ex)
            {
                //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetAddress");
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }

        }
        public string GetAutoDTrCode(clsStoreEnumeration objenumeration)
        {
            PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
            string strQry = string.Empty;
            string tempTCcode = string.Empty;
            DataTable dtDetails = new DataTable();
            try
            {
               
                strQry = "SELECT max(\"TE_TC_CODE\") FROM \"TBLTEMPENUMERATIONDETAILS\"";
                tempTCcode = ObjCon.get_value(strQry);
                string Te_id = Convert.ToString(ObjCon.Get_max_no("TE_ID", "TBLTEMPENUMERATIONDETAILS"));
                string status = "0";
                if (tempTCcode.Length == 0)
                {
                 
                    tempTCcode = "H100001";
                
                    strQry = "INSERT INTO \"TBLTEMPENUMERATIONDETAILS\" (\"TE_ID\",\"TE_TC_CODE\",\"TE_CRBY\",\"TE_TC_TYPE\",\"TE_DEVICEID\",\"TE_TC_STATUS\",\"TE_SOURCE\",\"TE_OFFICECODE\")";
                       strQry += " VALUES ('"+ Te_id + "','" + tempTCcode + "','" + sCrBy + "','" + sTCType + "','" + sClientIP + "','"+ status + "','WEB','"+ objenumeration.sofficecode + "')";
                    ObjCon.ExecuteQry(strQry);
                }
                else
                {
                    strQry = " SELECT \"TE_TC_CODE\" from \"TBLTEMPENUMERATIONDETAILS\" where \"TE_CRBY\" = '" + sCrBy + "' and \"TE_TC_STATUS\"='0' and \"TE_DEVICEID\"='" + sClientIP + "' and \"TE_TC_TYPE\"='" + sTCType + "' ";
                    string sResult1 = ObjCon.get_value(strQry);
                    if (sResult1 != "")
                    {
                        tempTCcode = sResult1.Split('~').GetValue(0).ToString();
                    }
                    else {
                        //tempTCcode = Convert.ToString(ObjCon.Get_max_no("TE_TC_CODE", "TBLTEMPENUMERATIONDETAILS"));
                        NpgsqlCommand cmd = new NpgsqlCommand("proc_dtrcode_autogen");
                        dtDetails = ObjCon.FetchDataTable(cmd);
                        if (dtDetails.Rows.Count > 0)
                        {
                            tempTCcode = Convert.ToString(dtDetails.Rows[0]["proc_dtrcode_autogen"]);
                        }
                        strQry = "INSERT INTO \"TBLTEMPENUMERATIONDETAILS\" (\"TE_ID\",\"TE_TC_CODE\",\"TE_CRBY\",\"TE_TC_TYPE\",\"TE_DEVICEID\",\"TE_SOURCE\",\"TE_TC_STATUS\",\"TE_OFFICECODE\")";
                        strQry += " VALUES ('" + Te_id + "','" + tempTCcode + "','" + sCrBy + "','" + sTCType + "','" + sClientIP + "','WEB','0','" + objenumeration.sofficecode + "')";
                        ObjCon.ExecuteQry(strQry);
                    }
                    
                }

            }
            catch(Exception e)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message, e.StackTrace);
            }

            return tempTCcode;
        }
        public DataTable getDTrType(clsStoreEnumeration objenumeration)
        {
            DataTable dt = new DataTable();
            PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("sp_dtr_type");
                cmd.Parameters.AddWithValue("loc_type",Convert.ToString( objenumeration.sLoctypeInt));
                //cmd.Parameters.AddWithValue("loc_type", objenumeration.sLoctype);
                dt = ObjCon.FetchDataTable(cmd);

               
            }
            catch(Exception e)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message, e.StackTrace);
            }
            return dt;
        }
        public bool SaveImagePathDetails(clsStoreEnumeration objStoreEnum)
        {
            try
            {
                string strQry = string.Empty;
                PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
                string sMaxNo =  Convert.ToString(ObjCon.Get_max_no("EP_ID", "TBLENUMERATIONPHOTOS"));

                strQry = "INSERT INTO \"TBLENUMERATIONPHOTOS\" (\"EP_ID\",\"EP_ED_ID\",\"EP_NAMEPLATE_PATH\",\"EP_SSPLATE_PATH\",\"EP_CRBY\") VALUES ('" + sMaxNo + "','" + objStoreEnum.sEnumDetailsId + "',";
                strQry += " '" + objStoreEnum.sNamePlatePhotoPath + "','" + objStoreEnum.sSSPlatePhotoPath + "','" + objStoreEnum.sCrBy + "')";
               ObjCon.ExecuteQry(strQry);
                return true;
            }
            catch (Exception ex)
            {
                //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveImagePathDetails");
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }

        public bool UpdateImagePathDetails(clsStoreEnumeration objStoreEnum)
        {
            try
            {
                string strQry = string.Empty;
                PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
                strQry = "UPDATE \"TBLENUMERATIONPHOTOS\" SET \"EP_NAMEPLATE_PATH\"='" + objStoreEnum.sNamePlatePhotoPath + "',\"EP_SSPLATE_PATH\"='" + objStoreEnum.sSSPlatePhotoPath + "'";
                strQry += "  WHERE \"EP_ED_ID\"='" + objStoreEnum.sEnumDetailsId + "' ";

              ObjCon.ExecuteQry(strQry);
                return true;

            }
            catch (Exception ex)
            {
                //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "UpdateImagePathDetails");
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }

        public clsStoreEnumeration GetNextEnumerationDetails(clsStoreEnumeration objEnum)
        {
            try
            {
                PGSqlConnection objcon = new PGSqlConnection(Constants.Password);

                string sQry = string.Empty;
                //sQry = "SELECT \"ED_ID\" || '~' || \"ED_STATUS_FLAG\" FROM \"TBLENUMERATIONDETAILS\" WHERE \"ED_STATUS_FLAG\" = 0 AND ";
                //sQry += " \"ED_OFFICECODE\" = '" + objEnum.sDivCode + "' AND  \"ED_ID\" <> " + objEnum.sEnumDTCId + "  AND \"ED_ENUM_TYPE\" = 1 AND ";

                sQry = " SELECT  \"ED_ID\" || '~' || \"ED_STATUS_FLAG\" FROM \"TBLENUMERATIONDETAILS\", \"TBLDTCENUMERATION\" WHERE \"ED_ID\" = \"DTE_ED_ID\" ";
                sQry += " AND \"DTE_TC_CODE\" = ( SELECT MIN(\"DTE_TC_CODE\") FROM \"TBLENUMERATIONDETAILS\", \"TBLDTCENUMERATION\" WHERE \"ED_ID\" = \"DTE_ED_ID\" ";
                sQry += " AND \"ED_STATUS_FLAG\" = 0   AND \"ED_ENUM_TYPE\" = 1  AND \"ED_OFFICECODE\" = '" + objEnum.sDivCode + "' AND ";
                sQry += "  \"DTE_TC_CODE\" > '" + objEnum.sTcCode + "' ";

                //if (objEnum.sUserType == "1" || objEnum.sUserType == "3" || objEnum.sUserType == "5")
                //{
                //    sQry += "  AND \"ED_APPROVE_STATUS\" = '0') ";
                //}
                //else if (objEnum.sUserType == "6")
                //{
                //    sQry += " AND \"ED_APPROVE_STATUS\" = '1') ";
                //}
                //else if (objEnum.sUserType == "2")
                //{
                //    sQry += "  AND  \"ED_APPROVE_STATUS\" = '2') ";
                //}
                //else if (objEnum.sUserType == "4")
                //{
                //    sQry += " AND \"ED_APPROVE_STATUS\" = '2') ";
                //}

                if (objEnum.sUserType == "1" || objEnum.sUserType == "3" || objEnum.sUserType == "5")
                {
                    sQry += "   AND \"ED_UPDATE_ON\" IS NULL  AND \"ED_APPROVALPRIORITY\" = '1') ";
                }
                else if (objEnum.sUserType == "6")
                {
                    sQry += " AND \"ED_APPROVALPRIORITY\" = '2') ";
                }
                else if (objEnum.sUserType == "7")
                {
                    sQry += "  AND  \"ED_APPROVALPRIORITY\" = '3') ";
                }
                else if (objEnum.sUserType == "8")
                {
                    sQry += "  AND  \"ED_APPROVALPRIORITY\" = '4') ";
                }
                else if (objEnum.sUserType == "2")
                {
                    sQry += " AND \"ED_APPROVALPRIORITY\" = '5') ";
                }

                string sResult = objcon.get_value(sQry);
                if (sResult.Length > 0)
                {
                    objEnum.sEnumDTCId = sResult.Split('~').GetValue(0).ToString();
                    objEnum.sStatus = sResult.Split('~').GetValue(1).ToString();
                }
                else
                {
                    objEnum.sEnumDTCId = "";
                    objEnum.sStatus = "";
                }
                return objEnum;
            }
            catch (Exception ex)
            {
                //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetIPEnumerationData");
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                objEnum.sEnumDTCId = "";
                objEnum.sStatus = "";
                return objEnum;
            }
        }

    }
}
