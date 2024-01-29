using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data;
using System.Data.OleDb;
using IIITS.PGSQL.DAL;
using Npgsql;
using System.Configuration;
using NpgsqlTypes;

namespace IIITS.DTLMS.BL
{
    public class clsDTCCommision
    {
        string strFormCode = "clsDTCCommision";
        public long lGetMaxMap { get; set; }
        public string lDtcId { get; set; }
        public string sDtcCode { get; set; }
        public string sDtcName { get; set; }
        public string sOMSectionName { get; set; }
        public string iConnectedKW { get; set; }
        public string iConnectedHP { get; set; }
        public string sInternalCode { get; set; }
        public string sPlatformType { get; set; }
        public string sConnectionDate { get; set; }
        public string sInspectionDate { get; set; }
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
        public string sHtlinelength { get; set; }
        public string sLtlinelength { get; set; }
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
        public string sTimeId { get; set; }

        public string sWOslno { get; set; }
        public string sOfficeCode { get; set; }

        public string sDTCImagePath { get; set; }
        public string sDTrImagePath { get; set; }

        // Workflow
        public string sFormName { get; set; }
        public string sClientIP { get; set; }
        public string sWFOId { get; set; }
        public string sTimsCode { get; set; }

        public string sDtcWithoutDtrFlag { get; set; }

        int Circle_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Circle_code"]);
        int Division_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Division_code"]);
        int SubDiv_code = Convert.ToInt32(ConfigurationSettings.AppSettings["SubDiv_code"]);
        int Section_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Section_code"]);
        int Feeder_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Feeder_code"]);

        //CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);

        public string[] SaveUpdateDtcDetails(clsDTCCommision objDtcMaster)
        {
            string[] Arr = new string[2];
            try
            {
                PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
                NpgsqlDataReader dr;

             
                string strQry = string.Empty;
               

                if (objDtcMaster.sOfficeCode.Length >= 4)
                {
                    objDtcMaster.sOfficeCode = objDtcMaster.sOfficeCode.Substring(0, SubDiv_code);
                }                
                            

                if (objDtcMaster.lDtcId == "")
                {
                    //dr = ObjCon.Fetch("select \"DT_CODE\" from \"TBLDTCMAST\" where \"DT_CODE\"='" + objDtcMaster.sDtcCode + "'");
                    //if (dr.Read())
                    //{
                    //    dr.Close();
                    //    Arr[0] = "DTC Code Already Exists";
                    //    Arr[1] = "2";
                    //    return Arr;
                    //}
                    //dr.Close();


                    //dr = ObjCon.Fetch("select \"OM_CODE\" from \"TBLOMSECMAST\" where \"OM_CODE\"='" + objDtcMaster.sOMSectionName + "'");
                    //if (!dr.Read())
                    //{
                    //    dr.Close();
                    //    Arr[0] = "Enter Valid O&M Section Code ";
                    //    Arr[1] = "2";
                    //    return Arr;
                    //}
                    //dr.Close();
                    //dr = ObjCon.Fetch("select \"TC_CODE\" from \"TBLTCMASTER\" where \"TC_CODE\"='" + objDtcMaster.sTcCode + "'");
                    //if (!dr.Read())
                    //{

                    //    dr.Close();
                    //    Arr[0] = "Enter Valid TC Code ";
                    //    Arr[1] = "2";
                    //    return Arr;

                    //}
                    //dr.Close();

                    ////ObjCon.BeginTransaction();
                    //objDtcMaster.lDtcId = Convert.ToString(ObjCon.Get_max_no("DT_ID", "TBLDTCMAST"));

                    //string strFeederSlno = ObjCon.get_value("SELECT \"FD_FEEDER_ID\" FROM \"TBLFEEDERMAST\" WHERE CAST(\"FD_FEEDER_CODE\" AS TEXT)='" + objDtcMaster.sDtcCode.ToString().Substring(0, Feeder_code) + "'");



                    if (objDtcMaster.sTcCode == null || objDtcMaster.sTcCode == "")
                    {
                        objDtcMaster.sTcCode = "0";
                    }
                    if (objDtcMaster.iConnectedKW == null || objDtcMaster.iConnectedKW == "")
                    {
                        objDtcMaster.iConnectedKW = "0";

                    }
                   
                    if (objDtcMaster.sProjecttype == null || objDtcMaster.sProjecttype == "")
                    {
                        objDtcMaster.sProjecttype = "0";

                    }
                    if (objDtcMaster.sTcCode == null || objDtcMaster.sTcCode == "")
                    {
                        objDtcMaster.sTcCode = "0";
                    }
                    if (objDtcMaster.iKWHReading == null || objDtcMaster.iKWHReading == "")
                    {
                        objDtcMaster.iKWHReading = "0";
                    }

                    if (objDtcMaster.iConnectedHP == null || objDtcMaster.iConnectedHP == "")
                    {
                        objDtcMaster.iConnectedHP = "0";
                    }


                    if (objDtcMaster.sWOslno == null || objDtcMaster.sWOslno == "")
                    {
                        objDtcMaster.sWOslno = "0";
                    }

                    if (objDtcMaster.sFeederChangeDate == null || objDtcMaster.sFeederChangeDate == "")
                    {
                        objDtcMaster.sFeederChangeDate = "";
                    }


                    if (objDtcMaster.sLtlinelength == null || objDtcMaster.sLtlinelength == "")
                    {
                        objDtcMaster.sLtlinelength = "0";
                    }

                    if (objDtcMaster.sHtlinelength == null || objDtcMaster.sHtlinelength == "")
                    {
                        objDtcMaster.sHtlinelength = "";
                    }
                    if(objDtcMaster.sLongitude == null || objDtcMaster.sLongitude == "")
                    {
                        objDtcMaster.sLongitude = "";
                    }
                    if(objDtcMaster.sLatitude == null || objDtcMaster.sLatitude == "")
                    {
                        objDtcMaster.sLatitude = "";
                    }

                    try
                    {
                        NpgsqlCommand cmd = new NpgsqlCommand("sp_qcsavetodtcmaster");
                        cmd.Parameters.AddWithValue("ldtcid", objDtcMaster.lDtcId);
                        cmd.Parameters.AddWithValue("sdtccode", objDtcMaster.sDtcCode);
                        cmd.Parameters.AddWithValue("sdtcname", objDtcMaster.sDtcName);
                        cmd.Parameters.AddWithValue("somsectionname", objDtcMaster.sOMSectionName);
                        cmd.Parameters.AddWithValue("iconnectedkw", objDtcMaster.iConnectedKW);
                        cmd.Parameters.AddWithValue("iconnectedhp", objDtcMaster.iConnectedHP);
                        cmd.Parameters.AddWithValue("ikwhreading", objDtcMaster.iKWHReading);
                        cmd.Parameters.AddWithValue("sinternalcode", objDtcMaster.sInternalCode);
                        cmd.Parameters.AddWithValue("stccode", objDtcMaster.sTcCode);
                        cmd.Parameters.AddWithValue("scommisiondate", objDtcMaster.sCommisionDate);
                        cmd.Parameters.AddWithValue("sservicedate", objDtcMaster.sServiceDate);
                        cmd.Parameters.AddWithValue("sfeederchangedate", objDtcMaster.sFeederChangeDate);
                        cmd.Parameters.AddWithValue("sfeederno", objDtcMaster.sDtcCode.ToString().Substring(0, Feeder_code));
                        cmd.Parameters.AddWithValue("scrby", objDtcMaster.sCrBy);
                        cmd.Parameters.AddWithValue("swoslno", objDtcMaster.sWOslno);
                        cmd.Parameters.AddWithValue("sprojecttype", objDtcMaster.sProjecttype);
                        cmd.Parameters.AddWithValue("stimscode", objDtcMaster.sTimsCode);
                        cmd.Parameters.AddWithValue("sbreakertype", objDtcMaster.sBreakertype);
                        cmd.Parameters.AddWithValue("sdtcmeters", objDtcMaster.sDTCMeters);
                        cmd.Parameters.AddWithValue("shtprotect", objDtcMaster.sHTProtect);
                        cmd.Parameters.AddWithValue("sltprotect", objDtcMaster.sLTProtect);
                        cmd.Parameters.AddWithValue("sgrounding", objDtcMaster.sGrounding);
                        cmd.Parameters.AddWithValue("sarresters", objDtcMaster.sArresters);
                        cmd.Parameters.AddWithValue("sloadtype", objDtcMaster.sLoadtype);
                        cmd.Parameters.AddWithValue("splatformtype", objDtcMaster.sPlatformType);
                        cmd.Parameters.AddWithValue("sltlinelength", objDtcMaster.sLtlinelength);
                        cmd.Parameters.AddWithValue("shtlinelength", objDtcMaster.sHtlinelength);
                        cmd.Parameters.AddWithValue("sofficecode", objDtcMaster.sOfficeCode);
                        cmd.Parameters.AddWithValue("slongitude", objDtcMaster.sLongitude);
                        cmd.Parameters.AddWithValue("slatitude", objDtcMaster.sLatitude);
                        cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                        cmd.Parameters.Add("id", NpgsqlDbType.Text);
                        cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                        cmd.Parameters["id"].Direction = ParameterDirection.Output;
                        Arr[1] = "id";
                        Arr[0] = "msg";
                        Arr = ObjCon.Execute(cmd, Arr, 2);


                        return Arr;
                    }
                    catch(Exception ex)
                    {
                        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveUpdateDtcDetails");
                        Arr[0] = ex.Message;
                        Arr[1] = "2";
                        return Arr;
                    }

                    //strQry = "Insert into \"TBLDTCMAST\" (\"DT_ID\",\"DT_CODE\",\"DT_NAME\",\"DT_OM_SLNO\",\"DT_TOTAL_CON_KW\",\"DT_TOTAL_CON_HP\",\"DT_KWH_READING\",";
                    //strQry += " \"DT_INTERNAL_CODE\",\"DT_TC_ID\",";

                    //if(objDtcMaster.sCommisionDate != null && objDtcMaster.sCommisionDate != "")
                    //{
                    //    strQry += " \"DT_CON_DATE\",\"DT_TRANS_COMMISION_DATE\",";
                    //}

                    //if (objDtcMaster.sServiceDate != null && objDtcMaster.sServiceDate != "")
                    //{
                    //    strQry += " \"DT_LAST_SERVICE_DATE\", ";
                    //}

                    //if (objDtcMaster.sFeederChangeDate != null && objDtcMaster.sFeederChangeDate != "")
                    //{
                    //    strQry += "  \"DT_FDRCHANGE_DATE\", ";
                    //}


                    //strQry += " \"DT_FDRSLNO\",\"DT_CRBY\",\"DT_CRON\",\"DT_WO_ID\",\"DT_PROJECTTYPE\", ";
                    //strQry += " \"DT_TIMS_CODE\", \"DT_BREAKER_TYPE\", \"DT_DTCMETERS\",\"DT_HT_PROTECT\", \"DT_LT_PROTECT\",\"DT_GROUNDING\",\"DT_ARRESTERS\", ";
                    //strQry += " \"DT_LOADTYPE\",\"DT_PLATFORM\",\"DT_LT_LINE\",\"DT_HT_LINE\") ";
                    //strQry += " VALUES ('" + objDtcMaster.lDtcId + "','" + objDtcMaster.sDtcCode + "',";
                    //strQry += " '" + objDtcMaster.sDtcName + "','" + objDtcMaster.sOMSectionName + "','" + objDtcMaster.iConnectedKW + "',";
                    //strQry += "'" + objDtcMaster.iConnectedHP + "','" + objDtcMaster.iKWHReading + "',";
                    //strQry += " '" + objDtcMaster.sInternalCode + "','" + objDtcMaster.sTcCode + "',";

                    //if (objDtcMaster.sCommisionDate != null && objDtcMaster.sCommisionDate != "")
                    //{
                    //    strQry += " TO_DATE('" + objDtcMaster.sCommisionDate + "','dd/MM/yyyy'),  TO_DATE('" + objDtcMaster.sCommisionDate + "','dd/MM/yyyy'), ";
                    //}

                    //if (objDtcMaster.sServiceDate != null && objDtcMaster.sServiceDate != "")
                    //{
                    //    strQry += " TO_DATE('" + objDtcMaster.sServiceDate + "','dd/MM/yyyy'),";
                    //}

                    //if (objDtcMaster.sFeederChangeDate != null && objDtcMaster.sFeederChangeDate != "")
                    //{
                    //    strQry += " TO_DATE('" + objDtcMaster.sFeederChangeDate + "','dd/MM/yyyy'), ";
                    //}

                    //strQry += " '" + objDtcMaster.sDtcCode.ToString().Substring(0, Feeder_code) + "','" + objDtcMaster.sCrBy + "',NOW(),'" + objDtcMaster.sWOslno + "','" + objDtcMaster.sProjecttype + "', ";
                    //strQry += " '" + objDtcMaster.sTimsCode + "','" + objDtcMaster.sBreakertype + "','" + objDtcMaster.sDTCMeters + "' ";
                    //strQry += " ,'" + objDtcMaster.sHTProtect + "','" + objDtcMaster.sLTProtect + "','" + objDtcMaster.sGrounding + "','" + objDtcMaster.sArresters + "' ";
                    //strQry += " ,'" + objDtcMaster.sLoadtype + "','" + objDtcMaster.sPlatformType + "','" + objDtcMaster.sLtlinelength + "', '" + objDtcMaster.sHtlinelength + "')";
                    //ObjCon.ExecuteQry(strQry);

                    //objDtcMaster.lGetMaxMap = ObjCon.Get_max_no("TM_ID", "TBLTRANSDTCMAPPING");

                    //strQry = "INSERT INTO \"TBLTRANSDTCMAPPING\" (\"TM_ID\",\"TM_TC_ID\",\"TM_DTC_ID\",\"TM_MAPPING_DATE\",\"TM_CRBY\") ";
                    //strQry += "VALUES('" + objDtcMaster.lGetMaxMap + "','" + objDtcMaster.sTcCode.ToUpper() + "','" + objDtcMaster.sDtcCode + "',";
                    //strQry += " NOW(),'" + objDtcMaster.sCrBy + "')";
                    //ObjCon.ExecuteQry(strQry);

                    //strQry = "UPDATE \"TBLTCMASTER\" set \"TC_UPDATED_EVENT\"='DTC MASTER ENTRY',\"TC_UPDATED_EVENT_ID\"='" + lGetMaxMap + "', \"TC_CURRENT_LOCATION\"=2, ";
                    //strQry += " \"TC_LOCATION_ID\"='" + objDtcMaster.sOMSectionName + "' where \"TC_CODE\"='" + objDtcMaster.sTcCode.ToUpper() + "'";
                    //ObjCon.ExecuteQry(strQry);


                    //strQry = "UPDATE \"TBLWORKORDER\" SET \"WO_REPLACE_FLG\"='1' WHERE \"WO_SLNO\"='"+ objDtcMaster.sWOslno +"'";
                    //ObjCon.ExecuteQry(strQry);

                    ////ObjCon.CommitTransaction();


                    ////Workflow / Approval
                    //clsApproval objApproval = new clsApproval();
                    //objApproval.sTimeId = objDtcMaster.sTimeId;
                    //objApproval.sFormName = objDtcMaster.sFormName;
                    //objApproval.sRecordId = objDtcMaster.lDtcId;
                    //objApproval.sOfficeCode = objDtcMaster.sOfficeCode;
                    //objApproval.sClientIp = objDtcMaster.sClientIP;
                    //objApproval.sCrby = objDtcMaster.sCrBy;
                    //objApproval.sWFObjectId = objDtcMaster.sWFOId;

                    //objApproval.sDescription = "Commissioning of DTC " + objDtcMaster.sDtcCode;


                    //if (objApproval.sBOId != null)
                    //{
                    //    objApproval.SaveWorkflowObjects(objApproval);
                    //}
                    //DateTime endtime = DateTime.Now;
                    //strQry = "UPDATE \"TBLTIMELOG\" SET \"TL_END_TIME\"= TO_DATE('" + endtime + "','mm/dd/yyyy HH:MI:SSAM') WHERE \"TL_ID\"='" + ApproveTimeid + "'";
                    //ObjCon.ExecuteQry(strQry);



                    //  objcon.Con.Close();
                    //Arr[0] = "DTC Details Saved Successfully";
                    //Arr[1] = "0";



                }
                else
                {
                    strQry = "select \"FD_FEEDER_ID\" from \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" WHERE \"FD_FEEDER_CODE\"='" + objDtcMaster.sDtcCode.ToString().Substring(0, Feeder_code) + "' ";
                    strQry += " AND  CAST(\"FD_FEEDER_ID\" AS TEXT)=CAST(\"FDO_FEEDER_ID\" AS TEXT) AND CAST(\"FDO_OFFICE_CODE\" AS TEXT) LIKE '" + objDtcMaster.sOfficeCode + "%'";
                    dr = ObjCon.Fetch(strQry);
                    if (!dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Code Does Not Match With The Feeder Code";
                        Arr[1] = "2";
                        return Arr;
                    }

                    dr.Close();

                    dr = ObjCon.Fetch("select \"DT_CODE\" from \"TBLDTCMAST\" where \"DT_CODE\"='" + objDtcMaster.sDtcCode + "' AND \"DT_ID\"<>'" + objDtcMaster.lDtcId + "'");
                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "DTC With This Id  Already Exists";
                        Arr[1] = "2";
                        return Arr;
                    }
                    dr.Close();


                    dr = ObjCon.Fetch("select \"OM_CODE\" from \"TBLOMSECMAST\" where \"OM_CODE\"='" + objDtcMaster.sOMSectionName + "' ");
                    if (!dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Enter Valid OM Section Code";
                        Arr[1] = "2";
                        return Arr;

                    }
                    dr.Close();
                    dr = ObjCon.Fetch("select \"TC_CODE\" from \"TBLTCMASTER\" where \"TC_CODE\"='" + objDtcMaster.sTcCode + "' ");
                    if (!dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Enter Valid TC Code ";
                        Arr[1] = "2";
                        return Arr;
                    }
                    dr.Close();

                    dr = ObjCon.Fetch("SELECT \"DF_DTC_CODE\" FROM \"TBLDTCFAILURE\" WHERE \"DF_DTC_CODE\"='" + objDtcMaster.sDtcCode + "' and \"DF_REPLACE_FLAG\"=0");
                    if (dr.Read())
                    {
                        dr.Close();
                        Arr[0] = "Selected DTC Cannot be updated, due to Declared as Failure/Enhancement";
                        Arr[1] = "2";
                        return Arr;
                    }
                    dr.Close();

                    NpgsqlDataReader drMapCondition = ObjCon.Fetch("SELECT COUNT(*) FROM \"TBLTRANSDTCMAPPING\"  WHERE \"TM_DTC_ID\"='" + objDtcMaster.sDtcCode + "'");
                    if (drMapCondition.Read())
                    {
                        
                        //string strFeederSlno = ObjCon.get_value("SELECT \"FD_FEEDER_ID\" FROM \"TBLFEEDERMAST\" WHERE cast(\"FD_FEEDER_CODE\" as text)='" + objDtcMaster.sDtcCode.ToString().Substring(0, Feeder_code) + "'");
                        string strCount = ObjCon.get_value("select count(*) from \"TBLTRANSDTCMAPPING\" where \"TM_TC_ID\"='" + objDtcMaster.sTcCode + "' and \"TM_LIVE_FLAG\"=1");
                        if (Convert.ToInt32(strCount) <= 1)
                        {
                            ObjCon.BeginTransaction();

                            strQry = "UPDATE \"TBLDTCMAST\" SET \"DT_CODE\"='" + objDtcMaster.sDtcCode + "',\"DT_NAME\" ='" + objDtcMaster.sDtcName + "',";
                            strQry += "\"DT_OM_SLNO\"='" + objDtcMaster.sOMSectionName + "',\"DT_TC_ID\"='" + objDtcMaster.sTcCode + "',\"DT_INTERNAL_CODE\"='" + objDtcMaster.sInternalCode + "',";
                            strQry += "\"DT_KWH_READING\"='" + objDtcMaster.iKWHReading + "',\"DT_TOTAL_CON_HP\"='" + objDtcMaster.iConnectedHP + "',\"DT_TOTAL_CON_KW\"='" + objDtcMaster.iConnectedKW + "',";
                            strQry += "\"DT_LAST_SERVICE_DATE\"=TO_DATE('" + objDtcMaster.sServiceDate + "','DD/MM/YYYY'),\"DT_TRANS_COMMISION_DATE\"=TO_DATE('" + objDtcMaster.sCommisionDate + "','DD/MM/YYYY'),";
                            strQry += "\"DT_FDRCHANGE_DATE\"=TO_DATE('" + objDtcMaster.sFeederChangeDate + "','DD/MM/YYYY') ,\"DT_FDRSLNO\"='" + objDtcMaster.sDtcCode.ToString().Substring(0, Feeder_code) + "', ";
                            strQry += "\"DT_CON_DATE\"=TO_DATE('" + objDtcMaster.sConnectionDate + "','DD/MM/YYYY'),\"DT_PROJECTTYPE\"='" + objDtcMaster.sProjecttype + "' ";
                            strQry += "\"DT_TIMS_CODE\"= '" + objDtcMaster.sTimsCode + "' , \"DT_BREAKER_TYPE\" = '" + objDtcMaster.sBreakertype + "', ";
                            strQry += "\"DT_DTCMETERS\" = '" + objDtcMaster.sDTCMeters + "',\"DT_HT_PROTECT\" = '" + objDtcMaster.sHTProtect + "', ";
                            strQry += "\"DT_LT_PROTECT\" = '" + objDtcMaster.sLTProtect + "',\"DT_GROUNDING\" = '" + objDtcMaster.sGrounding + "', ";
                            strQry += "\"DT_ARRESTERS\" = '" + objDtcMaster.sArresters + "' ,\"DT_LOADTYPE\" = '" + objDtcMaster.sLoadtype + "', ";
                            strQry += "\"DT_PLATFORM\" = '" + objDtcMaster.sPlatformType + "',\"DT_LT_LINE\" = '" + objDtcMaster.sLtlinelength + "', ";
                            strQry += "\"DT_HT_LINE\" = '" + objDtcMaster.sHtlinelength + "' WHERE \"DT_ID\"='" + objDtcMaster.lDtcId + "'";


                            ObjCon.ExecuteQry(strQry);


                            strQry = "UPDATE \"TBLTRANSDTCMAPPING\" SET \"TM_TC_ID\"='" + objDtcMaster.sTcCode.ToUpper() + "',\"TM_CRBY\"='" + objDtcMaster.sCrBy + "'";
                            //if (objDtcMaster.sConnectionDate != string.Empty)
                            //{
                            //    strQry += ",TM_MAPPING_DATE=TO_DATE('" + sConnectionDate + "','DD/MM/YYYY')";
                            //}
                            strQry += "where \"TM_DTC_ID\"='" + objDtcMaster.sDtcCode + "'";
                            ObjCon.ExecuteQry(strQry);

                            ObjCon.ExecuteQry("UPDATE \"TBLTCMASTER\" set \"TC_CURRENT_LOCATION\"=2, \"TC_LOCATION_ID\"='" + objDtcMaster.sOMSectionName + "' where \"TC_CODE\"='" + objDtcMaster.sTcCode + "'");

                            if (objDtcMaster.sTcCode != objDtcMaster.sOldTcCode && objDtcMaster.sOldTcCode != "")
                            {

                                ObjCon.ExecuteQry("UPDATE \"TBLTCMASTER\" set \"TC_CURRENT_LOCATION\"=1 where \"TC_CODE\"='" + objDtcMaster.sOldTcCode + "'");
                            }

                            ObjCon.CommitTransaction();
                            Arr[0] = "DTC Details Updated Successfully";
                            Arr[1] = "1";
                            return Arr;
                        }
                        else
                        {
                            Arr[0] = "DTC Cannot be updated as it is not in work, due to Failure";
                            Arr[1] = "2";
                            return Arr;
                        }
                    }
                    drMapCondition.Close();

                    DateTime endtime = DateTime.Now;
                    //strQry = "UPDATE \"TBLTIMELOG\" SET \"TL_END_TIME\"= TO_DATE('" + endtime + "','mm/dd/yyyy HH:MI:SSAM') WHERE \"TL_ID\"='" + ApproveTimeid + "'";
                    //ObjCon.ExecuteQry(strQry);


                }

                
                return Arr;

            }
            catch (Exception ex)
            {
                CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "SaveUpdateDtcDetails");
                return Arr;
            }
        }

        public string[] SaveUpdateDtcSpecification(clsDTCCommision objDtcMaster)
        {
            string[] Arr = new string[2];
            try
            {
                //CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
                PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
                string strQry = string.Empty;

                //strQry = "SELECT COALESCE(MAX(\"TL_ID\"),0)+1 FROM \"TBLTIMELOG\"";
                //string ApproveTimeid = ObjCon.get_value(strQry);
                //DateTime starttime = DateTime.Now;
                //strQry = "INSERT INTO \"TBLTIMELOG\" (\"TL_ID\",\"TL_PAGE_NAME\",\"TL_FUNCTION\",\"TL_START_TIME\",\"TL_TRANSACTION\")VALUES('" + ApproveTimeid + "','" + strFormCode + "',";
                //strQry += "'SaveUpdateTransformerDetails',TO_DATE('" + starttime + "','mm/dd/yyyy HH:MI:SSAM'),'" + objDtcMaster.sTimeId + "')";
                //ObjCon.ExecuteQry(strQry);

                strQry = "UPDATE \"TBLDTCMAST\" SET ";
                strQry += "\"DT_BREAKER_TYPE\" = '" + objDtcMaster.sBreakertype + "', \"DT_DTCMETERS\"= '" + objDtcMaster.sDTCMeters + "', \"DT_HT_PROTECT\" = '" + objDtcMaster.sHTProtect + "', ";
                strQry += "\"DT_LT_PROTECT\" = '" + objDtcMaster.sLTProtect + "', \"DT_GROUNDING\" = '" + objDtcMaster.sGrounding + "', \"DT_ARRESTERS\" = '" + objDtcMaster.sArresters + "', ";
                strQry += "\"DT_LT_LINE\" = '" + objDtcMaster.sLtlinelength + "', \"DT_HT_LINE\" = '" + objDtcMaster.sHtlinelength + "', \"DT_PLATFORM\"='" + objDtcMaster.sPlatformType + "', ";
                strQry += "\"DT_LOADTYPE\" = '" + objDtcMaster.sLoadtype + "', \"DT_LONGITUDE\" = '" + objDtcMaster.sLongitude + "', ";
                strQry += "\"DT_LATITUDE\" = '" + objDtcMaster.sLatitude + "',\"DT_DEPRECIATION\"='"+ objDtcMaster.sDepreciation  +"'  WHERE \"DT_ID\"='" + objDtcMaster.lDtcId + "'";

                ObjCon.ExecuteQry(strQry);

                DateTime endtime = DateTime.Now;
                //strQry = "UPDATE \"TBLTIMELOG\" SET \"TL_END_TIME\"= TO_DATE('" + endtime + "','mm/dd/yyyy HH:MI:SSAM') WHERE \"TL_ID\"='" + ApproveTimeid + "'";
                //ObjCon.ExecuteQry(strQry);

                Arr[0] = "DTC Details Saved/Updated Successfully";
                Arr[1] = "0";
                return Arr;
            }
            catch (Exception ex)
            {
                return Arr;
            }
        }


        public object GetDTCDetails(clsDTCCommision objDtcMaster)
        {
            try
            {
                CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
                DataTable dtDcDetails = new DataTable();
                string strQry = string.Empty;

                strQry = " SELECT DT_ID,DT_CODE,DT_NAME,DT_TC_ID,DT_OM_SLNO,TO_CHAR(DT_TOTAL_CON_KW) DT_TOTAL_CON_KW,TO_CHAR(DT_TOTAL_CON_HP) DT_TOTAL_CON_HP,TO_CHAR(DT_KWH_READING) DT_KWH_READING,DT_PLATFORM,";
                strQry += " DT_INTERNAL_CODE,DT_TC_ID,to_char(DT_CON_DATE,'dd/MM/yyyy')DT_CON_DATE,to_char(DT_LAST_INSP_DATE,'dd/MM/yyyy')DT_LAST_INSP_DATE,";
                strQry += " to_char(DT_LAST_SERVICE_DATE,'dd/MM/yyyy')DT_LAST_SERVICE_DATE,to_char(DT_TRANS_COMMISION_DATE,'dd/MM/yyyy')DT_TRANS_COMMISION_DATE,";
                strQry += " to_char(DT_FDRCHANGE_DATE,'dd/MM/yyyy')DT_FDRCHANGE_DATE,to_char(DT_CON_DATE,'dd/MM/yyyy') DT_CON_DATE, NVL(DT_BREAKER_TYPE,0) DT_BREAKER_TYPE,  ";
                strQry += "  NVL(DT_DTCMETERS,0) DT_DTCMETERS,  NVL(DT_HT_PROTECT,0) DT_HT_PROTECT, NVL(DT_LT_PROTECT,0) DT_LT_PROTECT, NVL( DT_GROUNDING,0) DT_GROUNDING, ";
                strQry += " NVL(DT_ARRESTERS,0) DT_ARRESTERS,DT_LT_LINE, DT_HT_LINE, NVL(DT_LOADTYPE,0) DT_LOADTYPE, NVL(DT_PROJECTTYPE,0) DT_PROJECTTYPE, DT_LONGITUDE, DT_LATITUDE,DT_DEPRECIATION FROM ";
                strQry += " TBLDTCMAST WHERE DT_ID='" + objDtcMaster.lDtcId + "'";

                OleDbDataReader dr = ObjCon.Fetch(strQry);

                dtDcDetails.Load(dr);

                objDtcMaster.lDtcId = Convert.ToString(dtDcDetails.Rows[0]["DT_ID"]);
                objDtcMaster.sDtcCode = Convert.ToString(dtDcDetails.Rows[0]["DT_CODE"]);
                objDtcMaster.sDtcName = Convert.ToString(dtDcDetails.Rows[0]["DT_NAME"]);
                objDtcMaster.sOMSectionName = Convert.ToString(dtDcDetails.Rows[0]["DT_OM_SLNO"]);
                objDtcMaster.iConnectedKW = Convert.ToString(dtDcDetails.Rows[0]["DT_TOTAL_CON_KW"]);
                objDtcMaster.iConnectedHP = Convert.ToString(dtDcDetails.Rows[0]["DT_TOTAL_CON_HP"]);
                objDtcMaster.iKWHReading = Convert.ToString(dtDcDetails.Rows[0]["DT_KWH_READING"]);
                objDtcMaster.sPlatformType = Convert.ToString(dtDcDetails.Rows[0]["DT_PLATFORM"]);
                objDtcMaster.sTcCode = Convert.ToString(dtDcDetails.Rows[0]["DT_TC_ID"]);
                objDtcMaster.sConnectionDate = Convert.ToString(dtDcDetails.Rows[0]["DT_CON_DATE"]);
                objDtcMaster.sInspectionDate = Convert.ToString(dtDcDetails.Rows[0]["DT_LAST_INSP_DATE"]);
                objDtcMaster.sServiceDate = Convert.ToString(dtDcDetails.Rows[0]["DT_LAST_SERVICE_DATE"]);
                objDtcMaster.sCommisionDate = Convert.ToString(dtDcDetails.Rows[0]["DT_TRANS_COMMISION_DATE"]);
                objDtcMaster.sFeederChangeDate = Convert.ToString(dtDcDetails.Rows[0]["DT_FDRCHANGE_DATE"]);
                objDtcMaster.sInternalCode = Convert.ToString(dtDcDetails.Rows[0]["DT_INTERNAL_CODE"]);
                objDtcMaster.sBreakertype = Convert.ToString(dtDcDetails.Rows[0]["DT_BREAKER_TYPE"]);
                objDtcMaster.sDTCMeters = Convert.ToString(dtDcDetails.Rows[0]["DT_DTCMETERS"]);
                objDtcMaster.sHTProtect = Convert.ToString(dtDcDetails.Rows[0]["DT_HT_PROTECT"]);
                objDtcMaster.sLTProtect = Convert.ToString(dtDcDetails.Rows[0]["DT_LT_PROTECT"]);
                objDtcMaster.sGrounding = Convert.ToString(dtDcDetails.Rows[0]["DT_GROUNDING"]);
                objDtcMaster.sArresters = Convert.ToString(dtDcDetails.Rows[0]["DT_ARRESTERS"]);
                objDtcMaster.sLtlinelength = Convert.ToString(dtDcDetails.Rows[0]["DT_LT_LINE"]);
                objDtcMaster.sHtlinelength = Convert.ToString(dtDcDetails.Rows[0]["DT_HT_LINE"]);
                objDtcMaster.sProjecttype = Convert.ToString(dtDcDetails.Rows[0]["DT_PROJECTTYPE"]);
                objDtcMaster.sLoadtype = Convert.ToString(dtDcDetails.Rows[0]["DT_LOADTYPE"]);
                objDtcMaster.sLongitude = Convert.ToString(dtDcDetails.Rows[0]["DT_LONGITUDE"]);
                objDtcMaster.sLatitude = Convert.ToString(dtDcDetails.Rows[0]["DT_LATITUDE"]);
                objDtcMaster.sDepreciation = Convert.ToString(dtDcDetails.Rows[0]["DT_DEPRECIATION"]);


                strQry = "SELECT TC_SLNO ||  '~' ||  TM_NAME || '~' || TO_CHAR(TC_CAPACITY) TC_CAPACITY FROM TBLTCMASTER,TBLTRANSMAKES  WHERE TC_MAKE_ID= TM_ID and TC_CODE='" + objDtcMaster.sTcCode + "'";

                string sResult = ObjCon.get_value(strQry);

                if (sResult != "")
                {
                    objDtcMaster.sTcSlno = sResult.Split('~').GetValue(0).ToString();
                    objDtcMaster.sTCMakeName = sResult.Split('~').GetValue(1).ToString();
                    objDtcMaster.sTCCapacity = sResult.Split('~').GetValue(2).ToString();
                }


                return objDtcMaster;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadDtcDetails");
                return objDtcMaster;
            }

        }

        /// <summary>
        /// To get TC Details Used in DTCMaster Form
        /// </summary>
        /// <param name="objTCMaster"></param>
        /// <returns></returns>
        public clsDtcMaster GetTCDetails(clsDtcMaster objDTCMaster)
        {
            try
            {
                CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
                string sQry = string.Empty;
                DataTable dt = new DataTable();
                sQry = "SELECT TC_SLNO,TC_CODE,TM_NAME,TO_CHAR(TC_CAPACITY) TC_CAPACITY FROM TBLTCMASTER,TBLTRANSMAKES ";
                sQry += " WHERE TC_MAKE_ID= TM_ID and TC_SLNO='" + objDTCMaster.sTcSlno + "'";
                dt = ObjCon.getDataTable(sQry);
                if (dt.Rows.Count > 0)
                {
                    objDTCMaster.sTcSlno = dt.Rows[0]["TC_SLNO"].ToString();
                    objDTCMaster.sTCMakeName = dt.Rows[0]["TM_NAME"].ToString();
                    objDTCMaster.sTCCapacity = dt.Rows[0]["TC_CAPACITY"].ToString();
                    objDTCMaster.sTcCode = dt.Rows[0]["TC_CODE"].ToString();

                }
                return objDTCMaster;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetTCDetails");
                return objDTCMaster;
            }
        }


        public clsDTCCommision GetImagePath(clsDTCCommision objDTCComm)
        {
            try
            {
                CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
                string strQry = string.Empty;
                DataTable dt = new DataTable();

                strQry = "SELECT EP_DTLMSDTC_PATH,EP_SSPLATE_PATH FROM TBLDTCENUMERATION,TBLENUMERATIONPHOTOS,TBLENUMERATIONDETAILS  ";
                strQry += " WHERE DTE_DTCCODE='" + objDTCComm.sDtcCode + "' AND DTE_ED_ID=EP_ED_ID AND ED_ID=DTE_ED_ID AND ED_STATUS_FLAG<>'5'";
                dt = ObjCon.getDataTable(strQry);
                if (dt.Rows.Count > 0)
                {
                    objDTCComm.sDTCImagePath = Convert.ToString(dt.Rows[0]["EP_DTLMSDTC_PATH"]);
                    objDTCComm.sDTrImagePath = Convert.ToString(dt.Rows[0]["EP_SSPLATE_PATH"]);
                }
                return objDTCComm;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetImagePath");
                return objDTCComm;
            }
        }


        public bool CheckSelfExecutionSchemeType(clsDTCCommision objDTcComm)
        {
            try
            {
                CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
                string strQry = string.Empty;
                bool bResult = false;
                if (objDTcComm.sProjecttype == "9" || objDTcComm.sProjecttype == "10")
                {
                    strQry = "SELECT DT_TRANS_COMMISION_DATE FROM TBLDTCMAST WHERE SYSDATE-DT_TRANS_COMMISION_DATE>365 AND DT_CODE='" + objDTcComm.sDtcCode + "'";
                    string sResult = ObjCon.get_value(strQry);
                    if (sResult == "")
                    {
                        bResult = true;
                    }

                }
                return bResult;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CheckSelfExecutionSchemeTypeFailureEntry");
                return false;
            }
        }
    }
}
