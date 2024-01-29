using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;
namespace IIITS.DTLMS.BL.TCRepair
{
    public class ClsRepairerEstimate
    {

        string strFormCode = "ClsRepairerEstimate";

        public string coiltype { get; set; }
        public string sOfficeCode { get; set; }
        public string sFailureId { get; set; }
        public string sEstimationId { get; set; }
        public string sEstimationNo { get; set; }
        public string sFaultCapacity { get; set; }
        public string sReplaceCapacity { get; set; }
        public string sUnit { get; set; }
        public string sQuantity { get; set; }
        public string sUnitPrice { get; set; }
        public string sAmount { get; set; }
        public string sUnitLabour { get; set; }
        public string sTotalLabour { get; set; }
        public string sLabourCharge { get; set; }
        public string s10PercLabourCharge { get; set; }
        public string sContig2Perc { get; set; }
        public string sTotal { get; set; }
        public string sDecommUnitPrice { get; set; }
        public string sDecommUnitLabour { get; set; }
        public string sDecommTotalLabour { get; set; }
        public string sDecommLabourCharge { get; set; }
        public string sDecomm10PercLabourCharge { get; set; }
        public string sDecommContig2Perc { get; set; }
        public string sDecommTotal { get; set; }
        public string sLastRepair { get; set; }

        public string sCrby { get; set; }
       

        public string sEmployeeCost { get; set; }
        public string sESI { get; set; }
        public string ServiceTax { get; set; }
        public string DecomLabourCost { get; set; }

        public string sFormName { get; set; }
        public string sClientIP { get; set; }


        public string sMaterialID { get; set; }
        public string sMaterialName { get; set; }
        public string sMaterialQnty { get; set; }
        public string sMaterialRate { get; set; }
        public string sMaterialTax { get; set; }
        public string sMaterialTotal { get; set; }
        public string sMaterialunit { get; set; }
        public string sMaterialunitName { get; set; }

        public DataTable dtMaterial { get; set; }
        public DataTable dtLabour { get; set; }
        public DataTable dtSalvage { get; set; }
        public DataTable dtDocuments { get; set; }

        public DataTable dtOil { get; set; }
        public string sWFO_id { get; set; }
        public string sFailType { get; set; }
        public string sEstComment { get; set; }
        public string sDtrCode { get; set; }
        public string sActionType { get; set; }
        public string sWFDataId { get; set; }
        public string sDtcCode { get; set; }

        public string scapacity { get; set; }

        public string sWoundType { get; set; }
        public string srateType { get; set; }
        public string sFileId { get; set; }
        public string sFileName { get; set; }
        public string sFileType { get; set; }
        public string sFilePath { get; set; }
        public string sGuaranteetype { get; set; }
        public string sremarks { get; set; }
        public string sEstDate { get; set; }
        public string sFinalTotalAmount { get; set; }
        public string sMaterialItemId { get; set; }

        public string sEstid { get; set; }

        public string sDtcId { get; set; }
        public string sTCId { get; set; }
        public string ssOfficeCode { get; set; }

        public string sStatus_flag { get; set; }


        public string sRepairer { get; set; }

        public string sDtcServicedate { get; set; }
        public string sDtcLoadKw { get; set; }
        public string sDtcLoadHp { get; set; }
        public string sCommissionDate { get; set; }
        public string sDtcCapacity { get; set; }
        public string sDtcLocation { get; set; }
        public string sDtcTcSlno { get; set; }
        public string sDtcTcMake { get; set; }
        public string sDtcTcCode { get; set; }
        public string sManfDate { get; set; }

        public string sDtcName { get; set; }

        public string sPhase { get; set; }
        public string sPhases { get; set; }


        public string soilfinaltotalamnt { get; set; }




        PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
        NpgsqlCommand NpgsqlCommand;
        public string GenerateEstimationNo(string sOfficeCode)
        {
            try
            {
                string sMaxNo = string.Empty;
                if (sOfficeCode == null || sOfficeCode == "")
                {
                    sOfficeCode = "";
                }
                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", sOfficeCode);


                string sEstNo = objcon.get_value("SELECT CAST(MAX(\"EST_NO\")+1 AS TEXT) FROM \"VIEW_ESTIMATION\" WHERE CAST(\"EST_NO\" AS TEXT) LIKE :sOfficeCode||'%' ", NpgsqlCommand);
                if (sEstNo == "")
                {

                    sEstNo = sOfficeCode + "001";
                }


                return sEstNo;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        public void SaveEstimationDetails(ClsRepairerEstimate objEstimation)
        {
            try
            {
                string strQry = string.Empty;


                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Parameters.AddWithValue("sFailureId", Convert.ToInt32(objEstimation.sEstid));
                string sEstId = objcon.get_value("SELECT \"RESTD_ID\" FROM \"TBLREPAIRERESTIMATIONDETAILS\" WHERE \"RESTD_ID\" =:sFailureId", NpgsqlCommand);
                if (sEstId.Length > 0)
                {
                    return;
                }

                // GetCommEstimatedDetails(objEstimation);
                // GetDecomEstimatedDetails(objEstimation);

                objEstimation.sEstimationNo = GenerateEstimationNo(objEstimation.sOfficeCode);

                if (objEstimation.sLastRepair == null || objEstimation.sLastRepair == "")
                {
                    objEstimation.sLastRepair = "0";
                }

                string[] Arr = new string[2];
                NpgsqlCommand cmd = new NpgsqlCommand("sp_saveestimationdetailsrepairer");
                cmd.Parameters.AddWithValue("sestid", sEstId);
                cmd.Parameters.AddWithValue("sestimationno", sEstimationNo);
                cmd.Parameters.AddWithValue("sfaultcapacity", sFaultCapacity);
                cmd.Parameters.AddWithValue("sreplacecapacity", sReplaceCapacity);
                cmd.Parameters.AddWithValue("sunit", sUnit);
                cmd.Parameters.AddWithValue("squantity", sQuantity);
                cmd.Parameters.AddWithValue("sunitprice", sUnitPrice);
                cmd.Parameters.AddWithValue("samount", sAmount);
                cmd.Parameters.AddWithValue("sunitlabour", sUnitLabour);
                cmd.Parameters.AddWithValue("stotallabour", sTotalLabour);
                cmd.Parameters.AddWithValue("slabourcharge", sLabourCharge);
                cmd.Parameters.AddWithValue("s10perclabourcharge", s10PercLabourCharge);
                cmd.Parameters.AddWithValue("scontig2perc", sContig2Perc);
                cmd.Parameters.AddWithValue("stotal", sTotal);
                cmd.Parameters.AddWithValue("sdecommunitprice", sDecommUnitPrice);
                cmd.Parameters.AddWithValue("sdecommunitlabour", sDecommUnitLabour);
                cmd.Parameters.AddWithValue("sdecommtotallabour", sDecommTotalLabour);
                cmd.Parameters.AddWithValue("sdecommlabourcharge", sDecommLabourCharge);
                cmd.Parameters.AddWithValue("sdecomm10perclabourcharge", sDecomm10PercLabourCharge);
                cmd.Parameters.AddWithValue("sdecommcontig2perc", sDecommContig2Perc);
                cmd.Parameters.AddWithValue("sdecommtotal", sDecommTotal);
                cmd.Parameters.AddWithValue("scrby", sCrby);
                cmd.Parameters.AddWithValue("slastrepair", sLastRepair);
                objcon.Execute(cmd, Arr, 0);





            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public ClsRepairerEstimate GetCommEstimatedDetails(ClsRepairerEstimate objEstimation)
        {

            DataTable dtDetailedReport = new DataTable();
            string strQry = string.Empty;
            try
            {
                NpgsqlCommand = new NpgsqlCommand();
                sEmployeeCost = ConfigurationSettings.AppSettings["EmployeeCost"];
                sESI = ConfigurationSettings.AppSettings["ESI"];
                ServiceTax = ConfigurationSettings.AppSettings["ServiceTax"];
                DecomLabourCost = ConfigurationSettings.AppSettings["DecomLabourCost"];


                int Division = Convert.ToInt32(ConfigurationSettings.AppSettings["Division_code"]);
                int SubDivision = Convert.ToInt32(ConfigurationSettings.AppSettings["SubDiv_code"]);
                int Section = Convert.ToInt32(ConfigurationSettings.AppSettings["Section_code"]);


                strQry = " select \"DF_DTC_CODE\",\"DF_EQUIPMENT_ID\",TO_CHAR(\"DF_DATE\",'dd/MM/yyyy')DF_DATE,\"DF_LOC_CODE\",(SELECT \"SD_SUBDIV_NAME\" FROM ";
                strQry += " \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT) =SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + SubDivision + ")) as SubDivision,(SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" ";
                strQry += " where CAST(\"OM_CODE\" AS TEXT) =SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Section + ")) as Location,'No' AS Unit,'1' as Quantity,(select CAST(\"TC_CAPACITY\" AS TEXT) ";
                strQry += " from \"TBLTCMASTER\" where \"TC_CODE\"=\"DF_EQUIPMENT_ID\") Capacity,";
                strQry += " \"TE_RATE\" as Price,1* \"TE_RATE\" AS TotalAmount,\"TE_COMMLABOUR\" as labourcharge,(\"TE_COMMLABOUR\" *'" + sEmployeeCost + "')/100 as EmployeeCost,";
                strQry += " ((\"TE_RATE\"+((\"TE_COMMLABOUR\" *'" + sEmployeeCost + "')/100)+ \"TE_COMMLABOUR\" )/100)*2 as ContingencyCost,(\"TE_RATE\"+\"TE_COMMLABOUR\" +((\"TE_COMMLABOUR\" *'" + sEmployeeCost + "')/100)+((\"TE_COMMLABOUR\" *'" + sESI + "')/100)+((\"TE_COMMLABOUR\" *'" + ServiceTax + "')/100)+((\"TE_RATE\" +((\"TE_COMMLABOUR\" *'" + sEmployeeCost + "')/100)+ \"TE_COMMLABOUR\" )/100)*2) as FinalTotal";
                strQry += " FROM \"TBLDTCFAILURE\",\"TBLITEMMASTER\",\"TBLTCMASTER\" where CAST(\"DF_ID\" AS TEXT) ='" + objEstimation.sFailureId + "' AND \"TC_CODE\"=\"DF_EQUIPMENT_ID\" AND  \"TC_CAPACITY\"=\"TE_CAPACITY\" ";
                strQry += " AND COALESCE(\"TC_STAR_RATE\",0)=COALESCE(\"TE_STAR_RATE\",0)";

                dtDetailedReport = objcon.FetchDataTable(strQry);

                if (dtDetailedReport.Rows.Count > 0)
                {
                    objEstimation.sFaultCapacity = Convert.ToString(dtDetailedReport.Rows[0]["Capacity"]);
                    objEstimation.sReplaceCapacity = Convert.ToString(dtDetailedReport.Rows[0]["Capacity"]);
                    objEstimation.sUnit = Convert.ToString(dtDetailedReport.Rows[0]["Unit"]);
                    objEstimation.sQuantity = Convert.ToString(dtDetailedReport.Rows[0]["Quantity"]);
                    objEstimation.sUnitPrice = Convert.ToString(dtDetailedReport.Rows[0]["Price"]);
                    objEstimation.sAmount = Convert.ToString(dtDetailedReport.Rows[0]["TotalAmount"]);
                    objEstimation.sUnitLabour = Convert.ToString(dtDetailedReport.Rows[0]["labourcharge"]);
                    objEstimation.sTotalLabour = Convert.ToString(dtDetailedReport.Rows[0]["labourcharge"]);
                    objEstimation.sLabourCharge = Convert.ToString(dtDetailedReport.Rows[0]["labourcharge"]);
                    objEstimation.s10PercLabourCharge = Convert.ToString(dtDetailedReport.Rows[0]["EmployeeCost"]);
                    objEstimation.sContig2Perc = Convert.ToString(dtDetailedReport.Rows[0]["ContingencyCost"]);
                    objEstimation.sTotal = Convert.ToString(dtDetailedReport.Rows[0]["FinalTotal"]);
                }
                else
                {
                    objEstimation.sFaultCapacity = "0";
                    objEstimation.sReplaceCapacity = "0";
                    objEstimation.sUnit = "0";
                    objEstimation.sQuantity = "0";
                    objEstimation.sUnitPrice = "0";
                    objEstimation.sAmount = "0";
                    objEstimation.sUnitLabour = "0";
                    objEstimation.sTotalLabour = "0";
                    objEstimation.sLabourCharge = "0";
                    objEstimation.s10PercLabourCharge = "0";
                    objEstimation.sContig2Perc = "0";
                    objEstimation.sTotal = "0";
                }

                return objEstimation;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objEstimation;

            }
        }

        public ClsRepairerEstimate GetDecomEstimatedDetails(ClsRepairerEstimate objEstimation)
        {
            DataTable dtDetailedReport = new DataTable();
            string strQry = string.Empty;
            try
            {
                int Division = Convert.ToInt32(ConfigurationSettings.AppSettings["Division_code"]);
                int SubDivision = Convert.ToInt32(ConfigurationSettings.AppSettings["SubDiv_code"]);
                int Section = Convert.ToInt32(ConfigurationSettings.AppSettings["Section_code"]);

                strQry = " SELECT \"DF_EQUIPMENT_ID\",to_char(\"DF_DATE\",'dd/MM/yyyy')DF_DATE,\"DF_REASON\",\"DF_LOC_CODE\",TO_CHAR(\"DF_CRON\",'dd/MM/yyyy')DF_CRON,";
                strQry += " \"TE_RATE\" as Price ,(1* \"TE_COMMLABOUR\" *'" + DecomLabourCost + "') as labourcharge,CAST(\"TC_CAPACITY\" AS TEXT)TC_CAPACITY,\"TC_CODE\",\"TC_SLNO\",'OLD' AS Rep,";
                strQry += " (SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE \"TC_MAKE_ID\"=\"TM_ID\")TM_NAME,\"DT_TOTAL_CON_KW\" ,(\"TE_COMMLABOUR\" *'" + sEmployeeCost + "')/100 as EmployeeCost,";
                strQry += " ((((\"TE_COMMLABOUR\" *'" + sEmployeeCost + "')/100)+(\"TE_COMMLABOUR\" *'" + DecomLabourCost + "'))/100)*2 as ContingencyCost, ";
                strQry += " ((\"TE_COMMLABOUR\" *'" + DecomLabourCost + "')+((\"TE_COMMLABOUR\" *'" + sEmployeeCost + "')/100)+((\"TE_COMMLABOUR\" *'" + sESI + "')/100)+((\"TE_COMMLABOUR\" *'" + ServiceTax + "')/100)+((((\"TE_COMMLABOUR\" *'" + sEmployeeCost + "')/100)+(\"TE_COMMLABOUR\" *'" + DecomLabourCost + "'))/100)*2) as FinalTotal,";
                strQry += " 'No' as Unit,'1' as Quantity,(SELECT \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\"  WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT) =SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + SubDivision + ")) as SubDivision ,";
                strQry += " (SELECT \"OM_NAME\" FROM \"TBLOMSECMAST\" where CAST(\"OM_CODE\" AS TEXT)=SUBSTR(CAST(\"DF_LOC_CODE\" AS TEXT),1," + Section + ")) as Location ";
                strQry += "  from  \"TBLDTCFAILURE\",\"TBLITEMMASTER\",\"TBLTCMASTER\",\"TBLDTCMAST\" WHERE \"DF_DTC_CODE\"=\"DT_CODE\" AND \"DF_EQUIPMENT_ID\"=\"TC_CODE\" AND \"TC_CAPACITY\"=\"TE_CAPACITY\" ";
                strQry += " AND \"DF_ID\"='" + objEstimation.sFailureId + "' AND COALESCE(\"TC_STAR_RATE\" ,0)=COALESCE(\"TE_STAR_RATE\",0)";
                dtDetailedReport = objcon.FetchDataTable(strQry);
                if (dtDetailedReport.Rows.Count > 0)
                {

                    objEstimation.sDecommUnitPrice = Convert.ToString(dtDetailedReport.Rows[0]["Price"]);
                    //objEstimation.s = Convert.ToString(dtDetailedReport.Rows[0]["TotalAmount"]);
                    objEstimation.sDecommUnitLabour = Convert.ToString(dtDetailedReport.Rows[0]["labourcharge"]);
                    objEstimation.sDecommTotalLabour = Convert.ToString(dtDetailedReport.Rows[0]["labourcharge"]);
                    objEstimation.sDecommLabourCharge = Convert.ToString(dtDetailedReport.Rows[0]["labourcharge"]);
                    objEstimation.sDecomm10PercLabourCharge = Convert.ToString(dtDetailedReport.Rows[0]["EmployeeCost"]);
                    objEstimation.sDecommContig2Perc = Convert.ToString(dtDetailedReport.Rows[0]["ContingencyCost"]);
                    objEstimation.sDecommTotal = Convert.ToString(dtDetailedReport.Rows[0]["FinalTotal"]);
                }
                else
                {
                    objEstimation.sDecommUnitPrice = "0";
                    objEstimation.sDecommUnitLabour = "0";
                    objEstimation.sDecommTotalLabour = "0";
                    objEstimation.sDecommLabourCharge = "0";
                    objEstimation.sDecomm10PercLabourCharge = "0";
                    objEstimation.sDecommContig2Perc = "0";
                    objEstimation.sDecommTotal = "0";

                }
                return objEstimation;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objEstimation;

            }
        }

        public ClsRepairerEstimate GetCommAndDecommAmount(ClsRepairerEstimate objEst)
        {
            try
            {
                string strQry = string.Empty;

                NpgsqlCommand cmd = new NpgsqlCommand("sp_getcommanddecommamount");
                cmd.Parameters.AddWithValue("sfailureid", objEst.sFailureId);
                DataTable dt = objcon.FetchDataTable(cmd);

                if (dt.Rows.Count > 0)
                {
                    objEst.sTotal = Convert.ToString(dt.Rows[0]["EST_TOTAL"]);
                    objEst.sDecommTotal = Convert.ToString(dt.Rows[0]["EST_DECOM_TOTAL"]);
                }

                return objEst;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objEst;

            }
        }

        public string[] SaveEstimation(ClsRepairerEstimate objestDetails, string[] sMaterial, string[] sLabour, string[] sSalvage, string[] sOil,string statusflag)
        {
            //string sPhases = string.Empty;
            string[] Arr = new string[4];
            try
            {
                string sQry = string.Empty;

                //int n = objestDetails.sPhase.Length;
                //for(int p=0; p<n; p++)
                //{
                if (statusflag != "2" && objestDetails.sFailType != "2") // Not For Enhancement Record
                {
                    objestDetails.sFailType = "1";
                    if (objestDetails.sFailType == "")
                    {
                        objestDetails.sFailType = "0";
                    }

                    if (objestDetails.sActionType == "M")
                    {
                        objestDetails.sOfficeCode = objcon.get_value("SELECT \"TC_PREV_OFFCODE\" FROM \"TBLTCMASTER\" WHERE \"TC_CODE\" ='" + objestDetails.sDtrCode + "'");
                    }

                    objestDetails.sFailureId = objcon.get_value("select COALESCE(max(\"DF_ID\"), 0) from \"TBLDTCFAILURE\"   where \"DF_EQUIPMENT_ID\" = '" + objestDetails.sDtrCode + "'");

                    string oilcapacity = objcon.get_value("SELECT \"TC_OIL_CAPACITY\" from \"TBLTCMASTER\" WHERE \"TC_CODE\" = '" + objestDetails.sDtrCode + "'");

                    sQry = "INSERT INTO \"TBLREPAIRERESTIMATIONDETAILS\" (\"RESTD_ID\", \"RESTD_FAILUREID\", \"RESTD_NO\",\"RESTD_CAPACITY\",\"RESTD_WOUNDTYPE\",\"RESTD_REPAIRER\",\"RESTD_CRBY\",\"RESTD_FAIL_TYPE\", \"RESTD_GUARANTEETYPE\",\"RESTD_ITEM_TOTAL\",\"RESTD_DATE\",\"RESTD_TC_CODE\",\"RESTD_OFF_CODE\",\"RESTD_TC_OILCAP\",\"RESTD_COIL_TYPE\",\"RESTD_PHASE\",\"RESTD_RATETYPE\",\"RESTD_OIL_TOTAL\")";
                    sQry += " VALUES('{0}', '" + objestDetails.sFailureId + "',(SELECT \"REPAIRERESTIMATIONNUMBER\"('" + objestDetails.sOfficeCode + "')),";
                    sQry += " '" + objestDetails.sFaultCapacity + "', '" + objestDetails.sWoundType + "', '" + objestDetails.sLastRepair + "', '" + objestDetails.sCrby + "','" + objestDetails.sFailType + "','" + objestDetails.sGuaranteetype + "','" + objestDetails.sFinalTotalAmount + "', TO_DATE('" + objestDetails.sEstDate + "','dd/MM/yyyy'),'" + objestDetails.sDtrCode + "','" + objestDetails.sOfficeCode + "','" + oilcapacity + "','" + objestDetails.coiltype + "','" + objestDetails.sPhase + "','" + objestDetails.srateType + "','"+ objestDetails.soilfinaltotalamnt + "')";

                    sQry = sQry.Replace("'", "''");

                    objestDetails.sPhases = sPhase.Replace(",", "`");
                    //}
                }


                string[] sMaterialVal = sMaterial.ToArray();
                string[] sLabourVal = sLabour.ToArray();
                string[] sSalvageVal = sSalvage.ToArray();
                string[] sOilVal = sOil.ToArray();

                StringBuilder sbQuery = new StringBuilder();

                string sMatID = string.Empty;
                string sMatQnty = string.Empty;
                string sMatRate = string.Empty;
                string sMatTax = string.Empty;
                string sMatTotal = string.Empty;
                string sMatUnit = string.Empty;
                string sMatName = string.Empty;
                string sMatUnitName = string.Empty;
                string sMattaxamount = string.Empty;
                string sMatamount = string.Empty;
                string sMaterialremarks = string.Empty;
                string sLabMaterialremarks = string.Empty;
                string slabtaxamount = string.Empty;
                string slabamount = string.Empty;
                string sMatItemId = string.Empty;

                String sQry1 = string.Empty;
                for (int i = 0; i < sMaterialVal.Length; i++)
                {
                    if (sMaterialVal[i] != null)
                    {
                        if (sMaterialVal[i].Substring(0, 1) != "~")
                        {
                            double TaxAmount = 0;
                            TaxAmount = Convert.ToDouble(sMaterialVal[i].Split('~').GetValue(4).ToString()) -
                                (Convert.ToDouble(sMaterialVal[i].Split('~').GetValue(1).ToString()) *
                                Convert.ToDouble(sMaterialVal[i].Split('~').GetValue(2).ToString()));

                            //"ESTM_ITEM_TOTAL" - "ESTM_ITEM_TAX" "AMOUNT"
                            double Amount = 0;
                            Amount = Convert.ToDouble(sMaterialVal[i].Split('~').GetValue(4).ToString()) - TaxAmount;

                            sQry1 = "INSERT INTO \"TBLREPAIRERESTIMATIONMATERIAL\" (\"RESTM_ID\", \"RESTM_EST_ID\", \"RESTM_ITEM_ID\", \"RESTM_ITEM_QNTY\", \"RESTM_ITEM_RATE\", \"RESTM_ITEM_TAX\", \"RESTM_ITEM_TOTAL\")";
                            sQry1 += " VALUES ((SELECT COALESCE(MAX(\"RESTM_ID\"),0)+1 FROM \"TBLREPAIRERESTIMATIONMATERIAL\"), '{0}' , '" + sMaterialVal[i].Split('~').GetValue(0).ToString() + "', '" + sMaterialVal[i].Split('~').GetValue(1).ToString() + "', ";
                            sQry1 += " '" + sMaterialVal[i].Split('~').GetValue(2).ToString() + "', '" + TaxAmount + "', ";
                            sQry1 += " '" + sMaterialVal[i].Split('~').GetValue(4).ToString() + "')";

                            sbQuery.Append(sQry1);
                            sbQuery.Append(";");

                            sMatID += sMaterialVal[i].Split('~').GetValue(0).ToString() + "`";
                            sMatQnty += sMaterialVal[i].Split('~').GetValue(1).ToString() + "`";
                            sMatRate += sMaterialVal[i].Split('~').GetValue(2).ToString() + "`";
                            sMatTax += sMaterialVal[i].Split('~').GetValue(3).ToString() + "`";
                            sMatTotal += sMaterialVal[i].Split('~').GetValue(4).ToString() + "`";
                            sMatUnit += sMaterialVal[i].Split('~').GetValue(5).ToString() + "`";
                            sMatName += sMaterialVal[i].Split('~').GetValue(6).ToString() + "`";
                            sMatUnitName += sMaterialVal[i].Split('~').GetValue(7).ToString() + "`";
                            sMaterialremarks += sMaterialVal[i].Split('~').GetValue(8).ToString() + "`";
                            sMattaxamount += Convert.ToString(TaxAmount) + "`";
                            sMatamount += Convert.ToString(Amount) + "`";
                            sMatItemId += sMaterialVal[i].Split('~').GetValue(9).ToString() + "`";
                        }

                    }
                }


                string sLabID = string.Empty;
                string sLabQnty = string.Empty;
                string sLabRate = string.Empty;
                string sLabTax = string.Empty;
                string sLabTotal = string.Empty;
                string sLabUnit = string.Empty;
                string sLabName = string.Empty;
                string sLabUnitName = string.Empty;
                string sLabItemId = string.Empty;

                String sQry2 = string.Empty;
                for (int i = 0; i < sLabourVal.Length; i++)
                {
                    if (sLabourVal[i] != null)
                    {
                        if (sLabourVal[i].Substring(0, 1) != "~")
                        {
                            double TaxAmount = 0;
                            TaxAmount = Convert.ToDouble(sLabourVal[i].Split('~').GetValue(4).ToString()) -
                                (Convert.ToDouble(sLabourVal[i].Split('~').GetValue(1).ToString()) *
                                Convert.ToDouble(sLabourVal[i].Split('~').GetValue(2).ToString()));

                            double Amount = 0;
                            Amount = Convert.ToDouble(sLabourVal[i].Split('~').GetValue(4).ToString()) - TaxAmount;

                            sQry2 = "INSERT INTO \"TBLREPAIRERESTIMATIONMATERIAL\" (\"RESTM_ID\", \"RESTM_EST_ID\", \"RESTM_ITEM_ID\", \"RESTM_ITEM_QNTY\", \"RESTM_ITEM_RATE\", \"RESTM_ITEM_TAX\", \"RESTM_ITEM_TOTAL\")";
                            sQry2 += " VALUES ((SELECT COALESCE(MAX(\"RESTM_ID\"),0)+1 FROM \"TBLREPAIRERESTIMATIONMATERIAL\"), '{0}' , '" + sLabourVal[i].Split('~').GetValue(0).ToString() + "', '" + sLabourVal[i].Split('~').GetValue(1).ToString() + "', ";
                            sQry2 += " '" + sLabourVal[i].Split('~').GetValue(2).ToString() + "', '" + TaxAmount + "', ";
                            sQry2 += " '" + sLabourVal[i].Split('~').GetValue(4).ToString() + "')";

                            sbQuery.Append(sQry2);
                            sbQuery.Append(";");

                            sLabID += sLabourVal[i].Split('~').GetValue(0).ToString() + "`";
                            sLabQnty += sLabourVal[i].Split('~').GetValue(1).ToString() + "`";
                            sLabRate += sLabourVal[i].Split('~').GetValue(2).ToString() + "`";
                            sLabTax += sLabourVal[i].Split('~').GetValue(3).ToString() + "`";
                            sLabTotal += sLabourVal[i].Split('~').GetValue(4).ToString() + "`";
                            sLabUnit += sLabourVal[i].Split('~').GetValue(5).ToString() + "`";
                            sLabName += sLabourVal[i].Split('~').GetValue(6).ToString() + "`";
                            sLabUnitName += sLabourVal[i].Split('~').GetValue(7).ToString() + "`";
                            sLabMaterialremarks += sLabourVal[i].Split('~').GetValue(8).ToString() + "`";
                            slabtaxamount += Convert.ToString(TaxAmount) + "`";
                            slabamount += Convert.ToString(Amount) + "`";
                            sLabItemId += sLabourVal[i].Split('~').GetValue(9).ToString() + "`";
                        }

                    }
                }


                string sSalID = string.Empty;
                string sSalQnty = string.Empty;
                string sSalRate = string.Empty;
                string sSalTax = string.Empty;
                string sSalTotal = string.Empty;
                string sSalUnit = string.Empty;
                string sSalName = string.Empty;
                string sSalUnitName = string.Empty;
                string sslaveMaterialremarks = string.Empty;
                string sSaltaxamount = string.Empty;
                string sSalamount = string.Empty;
                string sSalItemId = string.Empty;

                String sQry3 = string.Empty;
                for (int i = 0; i < sSalvageVal.Length; i++)
                {
                    if (sSalvageVal[i] != null)
                    {
                        if (sSalvageVal[i].Substring(0, 1) != "~")
                        {
                            double TaxAmount = 0;
                            TaxAmount = Convert.ToDouble(sSalvageVal[i].Split('~').GetValue(4).ToString()) -
                            (Convert.ToDouble(sSalvageVal[i].Split('~').GetValue(1).ToString()) *
                            Convert.ToDouble(sSalvageVal[i].Split('~').GetValue(2).ToString()));

                            double Amount = 0;
                            Amount = Convert.ToDouble(sSalvageVal[i].Split('~').GetValue(4).ToString()) - TaxAmount;

                            sQry3 = "INSERT INTO \"TBLREPAIRERESTIMATIONMATERIAL\" (\"RESTM_ID\", \"RESTM_EST_ID\", \"RESTM_ITEM_ID\", \"RESTM_ITEM_QNTY\", \"RESTM_ITEM_RATE\", \"RESTM_ITEM_TAX\", \"RESTM_ITEM_TOTAL\")";
                            sQry3 += " VALUES ((SELECT COALESCE(MAX(\"RESTM_ID\"),0)+1 FROM \"TBLREPAIRERESTIMATIONMATERIAL\"), '{0}' , '" + sSalvageVal[i].Split('~').GetValue(0).ToString() + "', '" + sSalvageVal[i].Split('~').GetValue(1).ToString() + "', ";
                            sQry3 += " '" + sSalvageVal[i].Split('~').GetValue(2).ToString() + "', '" + TaxAmount + "', ";
                            sQry3 += " '" + sSalvageVal[i].Split('~').GetValue(4).ToString() + "')";


                            sbQuery.Append(sQry3);
                            sbQuery.Append(";");

                            sSalID += sSalvageVal[i].Split('~').GetValue(0).ToString() + "`";
                            sSalQnty += sSalvageVal[i].Split('~').GetValue(1).ToString() + "`";
                            sSalRate += sSalvageVal[i].Split('~').GetValue(2).ToString() + "`";
                            sSalTax += sSalvageVal[i].Split('~').GetValue(3).ToString() + "`";
                            sSalTotal += sSalvageVal[i].Split('~').GetValue(4).ToString() + "`";
                            sSalUnit += sSalvageVal[i].Split('~').GetValue(5).ToString() + "`";
                            sSalName += sSalvageVal[i].Split('~').GetValue(6).ToString() + "`";
                            sSalUnitName += sSalvageVal[i].Split('~').GetValue(7).ToString() + "`";
                            sslaveMaterialremarks += sSalvageVal[i].Split('~').GetValue(8).ToString() + "`";
                            sSaltaxamount += Convert.ToString(TaxAmount) + "`";
                            sSalamount += Convert.ToString(Amount) + "`";
                            sSalItemId += sSalvageVal[i].Split('~').GetValue(9).ToString() + "`";
                        }

                    }
                }




                string sOilID = string.Empty;
                string sOilQnty = string.Empty;
                string sOilRate = string.Empty;
                string sOilTax = string.Empty;
                string sOilTotal = string.Empty;
                string sOilUnit = string.Empty;
                string sOilName = string.Empty;
                string sOilUnitName = string.Empty;
                string sOilMaterialremarks = string.Empty;
                string sOiltaxamount = string.Empty;
                string sOilamount = string.Empty;
                string sOilItemId = string.Empty;

                String sQry4 = string.Empty;
                for (int i = 0; i < sOilVal.Length; i++)
                {
                    if (sOilVal[i] != null)
                    {
                        if (sOilVal[i].Substring(0, 1) != "~")
                        {
                            double TaxAmount = 0;
                           // TaxAmount = Convert.ToDouble(sOilVal[i].Split('~').GetValue(4).ToString()) -
                           // (Convert.ToDouble(sOilVal[i].Split('~').GetValue(1).ToString()) *
                           // Convert.ToDouble(sOilVal[i].Split('~').GetValue(2).ToString()));

                            double Amount = 0;
                            Amount = Convert.ToDouble(sOilVal[i].Split('~').GetValue(4).ToString()) - TaxAmount;

                            sQry4 = "INSERT INTO \"TBLREPAIRERESTIMATIONMATERIAL\" (\"RESTM_ID\", \"RESTM_EST_ID\", \"RESTM_ITEM_ID\", \"RESTM_ITEM_QNTY\", \"RESTM_ITEM_RATE\", \"RESTM_ITEM_TAX\", \"RESTM_ITEM_TOTAL\")";
                            sQry4 += " VALUES ((SELECT COALESCE(MAX(\"RESTM_ID\"),0)+1 FROM \"TBLREPAIRERESTIMATIONMATERIAL\"), '{0}' , '" + sOilVal[i].Split('~').GetValue(0).ToString() + "', '" + sOilVal[i].Split('~').GetValue(1).ToString() + "', ";
                            sQry4 += " '" + sOilVal[i].Split('~').GetValue(2).ToString() + "', '" + TaxAmount + "', ";
                            sQry4 += " '" + sOilVal[i].Split('~').GetValue(4).ToString() + "')";


                            sbQuery.Append(sQry4);
                            sbQuery.Append(";");

                            sOilID += sOilVal[i].Split('~').GetValue(0).ToString() + "`";
                            sOilQnty += sOilVal[i].Split('~').GetValue(1).ToString() + "`";
                            sOilRate += sOilVal[i].Split('~').GetValue(2).ToString() + "`";
                            sOilTax += sOilVal[i].Split('~').GetValue(3).ToString() + "`";
                            sOilTotal += sOilVal[i].Split('~').GetValue(4).ToString() + "`";
                            sOilUnit += sOilVal[i].Split('~').GetValue(5).ToString() + "`";
                            sOilName += sOilVal[i].Split('~').GetValue(6).ToString() + "`";
                            sOilUnitName += sOilVal[i].Split('~').GetValue(7).ToString() + "`";
                            sOilMaterialremarks += sOilVal[i].Split('~').GetValue(8).ToString() + "`";
                            sOiltaxamount += Convert.ToString(TaxAmount) + "`";
                            sOilamount += Convert.ToString(Amount) + "`";
                            sOilItemId += sOilVal[i].Split('~').GetValue(9).ToString() + "`";
                        }

                    }
                }

                //if (objestDetails.sFailType == "1") // 1 single coil , 2 multi coil
                //{

                //    string LocCode = objestDetails.sOfficeCode.Substring(0, 3);

                //    String sQry4 = string.Empty;
                //    sQry4 = "UPDATE \"TBLTCMASTER\" SET \"TC_LOCATION_ID\"='" + clsStoreOffice.GetStoreID(LocCode) + "',\"TC_CURRENT_LOCATION\"='3',\"TC_LAST_FAILURE_TYPE\"='" + objFailureDetails.sFailType + "' WHERE \"TC_CODE\"='" + objFailureDetails.sDtrCode + "'";
                //    sbQuery.Append(sQry4);
                //    sbQuery.Append(";");
                //}
                //else
                //{
                //    String sQry4 = string.Empty;
                //    sQry4 = "UPDATE \"TBLTCMASTER\" SET \"TC_LAST_FAILURE_TYPE\"='" + objestDetails.sFailType + "' WHERE \"TC_CODE\"='" + objFailureDetails.sDtrCode + "'";
                //    sbQuery.Append(sQry4);
                //    sbQuery.Append(";");
                //}


                string sFileID = string.Empty;
                string sFileName = string.Empty;
                string sFilePath = string.Empty;
                string sFileType = string.Empty;
                String sQry5 = string.Empty;
                DataTable dt = new DataTable();

                if (objestDetails.dtDocuments != null)
                {
                    dt = objestDetails.dtDocuments;
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sQry5 = "INSERT INTO \"TBLREPAIRERESTIMATIONDOCS\" (\"RESD_ID\" , \"RESD_EST_ID\" , \"RESD_DOC_NAME\", \"RESD_DOC_PATH\" , \"RESD_DOC_TYPE\" ) ";
                            sQry5 += " VALUES ((SELECT COALESCE(MAX(\"RESD_ID\"),0)+1 FROM \"TBLREPAIRERESTIMATIONDOCS\"), '{0}',  ";
                            sQry5 += " '" + Convert.ToString(dt.Rows[i]["NAME"]) + "', '" + Convert.ToString(dt.Rows[i]["PATH"]) + "', '" + Convert.ToString(dt.Rows[i]["TYPE"]) + "' ) ";

                            sbQuery.Append(sQry5);
                            sbQuery.Append(";");

                            sFileID += Convert.ToString(dt.Rows[i]["ID"]) + "`";
                            sFileName += Convert.ToString(dt.Rows[i]["NAME"]) + "`";
                            sFilePath += Convert.ToString(dt.Rows[i]["PATH"]) + "`";
                            sFileType += Convert.ToString(dt.Rows[i]["TYPE"]) + "`";

                        }
                    }
                }



                sbQuery = sbQuery.Replace("'", "''");

                string sParam = "SELECT COALESCE(MAX(\"RESTD_ID\"),0)+1 FROM \"TBLREPAIRERESTIMATIONDETAILS\"";

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "FaultyEstimateCreation";
                // objApproval.sRecordId = objFailureDetails.sFailureId;
                objApproval.sOfficeCode = objestDetails.sOfficeCode;
                objApproval.sClientIp = objestDetails.sClientIP;
                objApproval.sCrby = objestDetails.sCrby;
                objApproval.sQryValues = sQry + ";" + sbQuery;
                objApproval.sParameterValues = sParam;
                objApproval.sMainTable = "TBLREPAIRERESTIMATIONDETAILS";
                objApproval.sDataReferenceId = objestDetails.sDtrCode;
                objApproval.sDescription = "Repairer Estimation Request for DTR CODE  " + objestDetails.sDtrCode;
                objApproval.sRefOfficeCode = objestDetails.sOfficeCode;

                objApproval.sWFObjectId = objestDetails.sWFO_id;

                string sPrimaryKey = "{0}";
                string sSecPrimaryKey = "{1}";

                objApproval.sColumnNames = "RESTD_ID,RESTD_FAILUREID,RESTD_NO,RESTD_CAPACITY,RESTD_WOUNDTYPE,RESTD_RATETYPE,RESTD_REPAIRER,RESTD_CRBY,RESTD_FAIL_TYPE,RESTD_GUARANTEETYPE,RESTD_DATE,RESTD_COIL_TYPE,RESTD_TC_CODE,RESTD_PHASE";
                objApproval.sColumnNames += ";RESTM_ID,MRIM_ID,RESTM_ITEM_QNTY,MRI_BASE_RATE,MRI_TAX,MRI_TOTAL,MRIM_ITEM_NAME,MRI_MEASUREMENT,MD_NAME,MRIM_REMARKS,RESTM_ITEM_TAX,AMOUNT,MRIM_ITEM_ID";
                objApproval.sColumnNames += ";RESTM_ID,MRIM_ID,RESTM_ITEM_QNTY,MRI_BASE_RATE,MRI_TAX,MRI_TOTAL,MRIM_ITEM_NAME,MRI_MEASUREMENT,MD_NAME,MRIM_REMARKS,RESTM_ITEM_TAX,AMOUNT,MRIM_ITEM_ID";
                objApproval.sColumnNames += ";RESTM_ID,MRIM_ID,RESTM_ITEM_QNTY,MRI_BASE_RATE,MRI_TAX,MRI_TOTAL,MRIM_ITEM_NAME,MRI_MEASUREMENT,MD_NAME,MRIM_REMARKS,RESTM_ITEM_TAX,AMOUNT,MRIM_ITEM_ID";
                objApproval.sColumnNames += ";ID,NAME,TYPE,PATH";
                objApproval.sColumnNames += ";RESTM_ID,MRIM_ID,RESTM_ITEM_QNTY,MRI_BASE_RATE,MRI_TAX,MRI_TOTAL,MRIM_ITEM_NAME,MRI_MEASUREMENT,MD_NAME,MRIM_REMARKS,RESTM_ITEM_TAX,AMOUNT,MRIM_ITEM_ID";

                objApproval.sColumnValues = "" + sPrimaryKey + "," + objestDetails.sFailureId + "," + objestDetails.sEstimationNo + ",";
                objApproval.sColumnValues += "" + objestDetails.sFaultCapacity + "," + objestDetails.sWoundType + "," + objestDetails.srateType + "," + objestDetails.sLastRepair + "," + objestDetails.sCrby + "," + objestDetails.sFailType + "," + objestDetails.sGuaranteetype + "," + objestDetails.sEstDate + "," + objestDetails.coiltype + "," + objestDetails.sDtrCode + "," + objestDetails.sPhases + "";
                objApproval.sColumnValues += ";" + sSecPrimaryKey + "," + sMatID + "," + sMatQnty + "," + sMatRate + "," + sMatTax + "," + sMatTotal + "," + sMatName + "," + sMatUnit + "," + sMatUnitName + "," + sMaterialremarks + "," + sMattaxamount + ", " + sMatamount + "," + sMatItemId + "";
                objApproval.sColumnValues += ";" + sSecPrimaryKey + "," + sLabID + "," + sLabQnty + "," + sLabRate + "," + sLabTax + "," + sLabTotal + "," + sLabName + "," + sLabUnit + "," + sLabUnitName + "," + sLabMaterialremarks + "," + slabtaxamount + "," + slabamount + "," + sLabItemId + "";
                objApproval.sColumnValues += ";" + sSecPrimaryKey + "," + sSalID + "," + sSalQnty + "," + sSalRate + "," + sSalTax + "," + sSalTotal + "," + sSalName + "," + sSalUnit + "," + sSalUnitName + "," + sslaveMaterialremarks + "," + sSaltaxamount + "," + sSalamount + "," + sSalItemId + "";
                objApproval.sColumnValues += ";" + sFileID + "," + sFileName + "," + sFileType + "," + sFilePath + "";
                objApproval.sColumnValues += ";" + sSecPrimaryKey + "," + sOilItemId + "," + sOilQnty + "," + sOilRate + "," + sOilTax + "," + sOilTotal + "," + sOilName + "," + sOilUnit + "," + sOilUnitName + "," + sOilMaterialremarks + "," + sOiltaxamount + "," + sOilamount + "," + sOilID + "";

                objApproval.sTableNames = "TBLREPAIRERESTIMATIONDETAILS;TBLREPAIRERESTIMATIONMATERIAL;TBLREPAIRERESTIMATIONLABMATERIAL;TBLREPAIRERESTIMATIONSALMATERIAL;TBLREPAIRERESTIMATIONDOCS;TBLREPAIRERESTIMATIONOILMATERIAL";

                if (objestDetails.sActionType == "M")
                {
                    objApproval.SaveWorkFlowData(objApproval);
                    objestDetails.sWFDataId = objApproval.sWFDataId;
                    Arr[2] = objApproval.sWFDataId;
                }
                else
                {
                    objApproval.SaveWorkFlowData(objApproval);
                    objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow();
                    objApproval.SaveWorkflowObjects(objApproval);
                    Arr[2] = objApproval.sWFDataId;
                    Arr[3] = objApproval.sRecordId;
                }

                Arr[0] = "Saved Successfully";
                Arr[1] = "0";
                return Arr;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }

        public ClsRepairerEstimate GetDetailsfromMainDB(ClsRepairerEstimate obj)
        {
            try
            {
                NpgsqlCommand = new NpgsqlCommand();
                String sQry = String.Empty;
                sQry = "SELECT \"RESTD_ID\",\"RESTD_CAPACITY\", \"RESTD_REPAIRER\", \"RESTD_CRBY\", \"RESTD_FAIL_TYPE\",\"RESTD_WOUNDTYPE\", \"RESTD_GUARANTEETYPE\",\"RESTD_COIL_TYPE\" ,\"RESTD_PHASE\" FROM ";
                sQry += " \"TBLREPAIRERESTIMATIONDETAILS\" WHERE \"RESTD_ID\" = :sEstimationId";
                DataTable dt = new DataTable();
                if (obj.sEstimationId == "" || obj.sEstimationId == null)
                {
                    obj.sEstimationId = "0";
                }
                NpgsqlCommand.Parameters.AddWithValue("sEstimationId", Convert.ToInt32(obj.sEstimationId));
                dt = objcon.FetchDataTable(sQry, NpgsqlCommand);
                if (dt.Rows.Count > 0)
                {
                    obj.sEstid = Convert.ToString(dt.Rows[0]["RESTD_ID"]);
                    obj.sFaultCapacity = Convert.ToString(dt.Rows[0]["RESTD_CAPACITY"]);
                    obj.sLastRepair = Convert.ToString(dt.Rows[0]["RESTD_REPAIRER"]);
                    obj.sCrby = Convert.ToString(dt.Rows[0]["RESTD_CRBY"]);
                    obj.sFailType = Convert.ToString(dt.Rows[0]["RESTD_FAIL_TYPE"]);
                    obj.sWoundType = Convert.ToString(dt.Rows[0]["RESTD_WOUNDTYPE"]);
                    obj.sGuaranteetype = Convert.ToString(dt.Rows[0]["RESTD_GUARANTEETYPE"]);
                    obj.coiltype = Convert.ToString(dt.Rows[0]["RESTD_COIL_TYPE"]);
                    obj.sPhases = Convert.ToString(dt.Rows[0]["RESTD_PHASE"]);
                    if (dt.Rows[0]["RESTD_RATETYPE"] != "" || dt.Rows[0]["RESTD_RATETYPE"] != null)
                    {
                        obj.srateType = Convert.ToString(dt.Rows[0]["RESTD_RATETYPE"]);
                    }
                    else
                    {
                        obj.srateType = "2";
                    }
                }
                NpgsqlCommand = new NpgsqlCommand();
                sQry = " SELECT \"RESTM_ITEM_ID\" AS \"MRIM_ID\", \"MRIM_ITEM_NAME\" \"MRIM_ITEM_NAME\" ,\"MRIM_ITEM_ID\" \"MRIM_ITEM_ID\", \"RESTM_ITEM_QNTY\" \"RESTM_ITEM_QNTY\", ";
                sQry += " \"RESTM_ITEM_RATE\" \"MRI_BASE_RATE\", \"RESTM_ITEM_TAX\" \"MRI_TAX\", \"RESTM_ITEM_TOTAL\" \"MRI_TOTAL\",  \"MRI_MEASUREMENT\" ";
                sQry += " \"MRI_MEASUREMENT\", B.\"MD_NAME\" \"MD_NAME\" FROM \"TBLREPAIRERESTIMATIONMATERIAL\", \"TBLMINORREPAIRERITEMMASTER\", ";
                sQry += " \"TBLREPAIRERESTIMATIONDETAILS\", \"TBLMINORREPAIRITEMRATEMASTER\", (SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" ";
                sQry += " WHERE \"MD_TYPE\" = 'C')A, (SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'MSR')B WHERE ";
                sQry += " \"RESTM_ITEM_ID\" = \"MRIM_ID\"  AND \"RESTD_ID\" = \"RESTM_EST_ID\" AND \"MRIM_ID\" = \"MRI_MRIM_ID\" AND \"RESTD_CAPACITY\" = ";
                sQry += " CAST(A.\"MD_NAME\" AS INT) AND A.\"MD_ID\" = \"MRI_CAPACITY\" AND \"RESTM_EST_ID\" =:sEstimationId1 AND \"RESTD_WOUNDTYPE\" = \"MRI_WOUNDTYPE\" ";
                sQry += " AND \"RESTD_REPAIRER\" = \"MRI_TR_ID\" AND CAST(\"MRI_MEASUREMENT\" AS INT)  = B.\"MD_ID\" AND  \"MRI_STATUS_FLAG\" = 0 ";
                sQry += " AND \"RESTD_CRON\" BETWEEN \"MRI_EFFECTIVE_FROM\" AND \"MRI_EFFECTIVE_TO\"   AND \"MRIM_ITEM_TYPE\" = 1 ORDER BY \"MRIM_ITEM_ORDER\" ";
                NpgsqlCommand.Parameters.AddWithValue("sEstimationId1", Convert.ToInt32(obj.sEstimationId));
                obj.dtMaterial = objcon.FetchDataTable(sQry, NpgsqlCommand);

                NpgsqlCommand = new NpgsqlCommand();
                sQry = " SELECT \"RESTM_ITEM_ID\" AS \"MRIM_ID\", \"MRIM_ITEM_NAME\" \"MRIM_ITEM_NAME\" , \"MRIM_ITEM_ID\" \"MRIM_ITEM_ID\",\"RESTM_ITEM_QNTY\" \"RESTM_ITEM_QNTY\", ";
                sQry += " \"RESTM_ITEM_RATE\" \"MRI_BASE_RATE\", \"RESTM_ITEM_TAX\" \"MRI_TAX\", \"RESTM_ITEM_TOTAL\" \"MRI_TOTAL\",  \"MRI_MEASUREMENT\" ";
                sQry += " \"MRI_MEASUREMENT\", B.\"MD_NAME\" \"MD_NAME\" FROM \"TBLREPAIRERESTIMATIONMATERIAL\", \"TBLMINORREPAIRERITEMMASTER\", ";
                sQry += " \"TBLREPAIRERESTIMATIONDETAILS\", \"TBLMINORREPAIRITEMRATEMASTER\", (SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" ";
                sQry += " WHERE \"MD_TYPE\" = 'C')A, (SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'MSR')B WHERE ";
                sQry += " \"RESTM_ITEM_ID\" = \"MRIM_ID\"  AND \"RESTD_ID\" = \"RESTM_EST_ID\" AND \"MRIM_ID\" = \"MRI_MRIM_ID\" AND \"RESTD_CAPACITY\" = ";
                sQry += " CAST(A.\"MD_NAME\" AS INT) AND A.\"MD_ID\" = \"MRI_CAPACITY\" AND \"RESTM_EST_ID\" =:sEstimationId2 AND \"RESTD_WOUNDTYPE\" = \"MRI_WOUNDTYPE\" ";
                sQry += " AND \"RESTD_REPAIRER\" = \"MRI_TR_ID\" AND CAST(\"MRI_MEASUREMENT\" AS INT)  = B.\"MD_ID\" AND  \"MRI_STATUS_FLAG\" = 0 ";
                sQry += " AND \"RESTD_CRON\" BETWEEN \"MRI_EFFECTIVE_FROM\" AND \"MRI_EFFECTIVE_TO\"   AND \"MRIM_ITEM_TYPE\" = 2 ORDER BY \"MRIM_ITEM_ORDER\" ";
                NpgsqlCommand.Parameters.AddWithValue("sEstimationId2", Convert.ToInt32(obj.sEstimationId));
                obj.dtLabour = objcon.FetchDataTable(sQry, NpgsqlCommand);

                NpgsqlCommand = new NpgsqlCommand();
                sQry = " SELECT \"RESTM_ITEM_ID\" AS \"MRIM_ID\", \"MRIM_ITEM_NAME\" \"MRIM_ITEM_NAME\" ,\"MRIM_ITEM_ID\" \"MRIM_ITEM_ID\", \"RESTM_ITEM_QNTY\" \"RESTM_ITEM_QNTY\", ";
                sQry += " \"RESTM_ITEM_RATE\" \"MRI_BASE_RATE\", \"RESTM_ITEM_TAX\" \"MRI_TAX\", \"RESTM_ITEM_TOTAL\" \"MRI_TOTAL\",  \"MRI_MEASUREMENT\" ";
                sQry += " \"MRI_MEASUREMENT\", B.\"MD_NAME\" \"MD_NAME\" FROM \"TBLREPAIRERESTIMATIONMATERIAL\", \"TBLMINORREPAIRERITEMMASTER\", ";
                sQry += " \"TBLREPAIRERESTIMATIONDETAILS\", \"TBLMINORREPAIRITEMRATEMASTER\", (SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" ";
                sQry += " WHERE \"MD_TYPE\" = 'C')A, (SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'MSR')B WHERE ";
                sQry += " \"RESTM_ITEM_ID\" = \"MRIM_ID\"  AND \"RESTD_ID\" = \"RESTM_EST_ID\" AND \"MRIM_ID\" = \"MRI_MRIM_ID\" AND \"RESTD_CAPACITY\" = ";
                sQry += " CAST(A.\"MD_NAME\" AS INT) AND A.\"MD_ID\" = \"MRI_CAPACITY\" AND \"RESTM_EST_ID\" =:sEstimationId3 AND \"RESTD_WOUNDTYPE\" = \"MRI_WOUNDTYPE\" ";
                sQry += " AND \"RESTD_REPAIRER\" = \"MRI_TR_ID\" AND CAST(\"MRI_MEASUREMENT\" AS INT)  = B.\"MD_ID\" AND  \"MRI_STATUS_FLAG\" = 0 ";
                sQry += " AND \"RESTD_CRON\" BETWEEN \"MRI_EFFECTIVE_FROM\" AND \"MRI_EFFECTIVE_TO\"   AND \"MRIM_ITEM_TYPE\" = 3 ORDER BY \"MRIM_ITEM_ORDER\" ";
                NpgsqlCommand.Parameters.AddWithValue("sEstimationId3", Convert.ToInt32(obj.sEstimationId));
                obj.dtSalvage = objcon.FetchDataTable(sQry, NpgsqlCommand);

                NpgsqlCommand = new NpgsqlCommand();
                sQry = " SELECT \"RESTM_ITEM_ID\" AS \"MRIM_ID\", \"ROI_ITEM_NAME\" \"MRIM_ITEM_NAME\" ,\"ROI_ITEM_ID\" \"MRIM_ITEM_ID\", \"RESTM_ITEM_QNTY\" \"RESTM_ITEM_QNTY\", ";
                sQry += "\"RESTM_ITEM_RATE\" \"MRI_BASE_RATE\", \"RESTM_ITEM_TAX\" \"MRI_TAX\", \"RESTM_ITEM_TOTAL\" \"MRI_TOTAL\",  B.\"MD_ID\" ";
                sQry += "\"MRI_MEASUREMENT\", B.\"MD_NAME\" \"MD_NAME\" FROM \"TBLREPAIRERESTIMATIONMATERIAL\", \"TBLREPAIREROILITEMMASTER\", ";
                sQry += "\"TBLREPAIRERESTIMATIONDETAILS\", (SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" ";
                sQry += " WHERE \"MD_TYPE\" = 'C')A, (SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'MSR')B WHERE ";
                sQry += "\"RESTM_ITEM_ID\" = \"ROI_ID\"  AND \"RESTD_ID\" = \"RESTM_EST_ID\" AND \"RESTD_CAPACITY\" = ";
                sQry += "CAST(A.\"MD_NAME\" AS INT)  AND \"RESTM_EST_ID\" =:sEstimationId4  and  B.\"MD_ID\"=7 and \"RESTM_ITEM_ID\"=\"ROI_ID\"  ORDER BY \"ROI_ITEM_ORDER\"  ";
                NpgsqlCommand.Parameters.AddWithValue("sEstimationId4", Convert.ToInt32(obj.sEstimationId));
                obj.dtOil = objcon.FetchDataTable(sQry, NpgsqlCommand);


                NpgsqlCommand = new NpgsqlCommand();
                sQry = "SELECT \"RESD_ID\" \"ID\", \"RESD_DOC_TYPE\" \"TYPE\",  \"RESD_DOC_NAME\" \"NAME\", \"RESD_DOC_PATH\" \"PATH\" FROM ";
                sQry += " \"TBLREPAIRERESTIMATIONDOCS\" WHERE  \"RESD_EST_ID\" =:sEstimationId4";
                NpgsqlCommand.Parameters.AddWithValue("sEstimationId4", Convert.ToInt32(obj.sEstimationId));
                obj.dtDocuments = objcon.FetchDataTable(sQry, NpgsqlCommand);
                return obj;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return obj;
            }
        }


        public ClsRepairerEstimate GetEstimateDetailsFromXML(ClsRepairerEstimate obj)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable dt = new DataTable();
                DataTable dtEstimation = new DataTable();

                DataSet ds = new DataSet();

                ds = objApproval.GetDatatableFromMultipleXML(obj.sWFO_id);

                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    if (ds.Tables[i].Rows.Count > 0)
                    {
                        dtEstimation = ds.Tables[i];
                        if (i == 0)
                        {
                            //objStoreIndent.sSiId = Convert.ToString(dtIndentDetails.Rows[0]["SI_ID"]);
                            obj.sEstid = Convert.ToString(dtEstimation.Rows[0]["RESTD_ID"]);
                            //obj.sOfficeCode = Convert.ToString(dtEstimation.Rows[0]["SI_DATE"]);
                            obj.sFaultCapacity = Convert.ToString(dtEstimation.Rows[0]["RESTD_CAPACITY"]);
                            obj.sLastRepair = Convert.ToString(dtEstimation.Rows[0]["RESTD_REPAIRER"]);
                            obj.sCrby = Convert.ToString(dtEstimation.Rows[0]["RESTD_CRBY"]);
                            obj.sFailType = Convert.ToString(dtEstimation.Rows[0]["RESTD_FAIL_TYPE"]);
                            obj.sWoundType = Convert.ToString(dtEstimation.Rows[0]["RESTD_WOUNDTYPE"]);
                            if (dtEstimation.Columns.Contains("RESTD_RATETYPE"))
                            {
                                if (dtEstimation.Rows[0]["RESTD_RATETYPE"] != "" || dtEstimation.Rows[0]["RESTD_RATETYPE"] != null)
                                {
                                    obj.srateType = Convert.ToString(dtEstimation.Rows[0]["RESTD_RATETYPE"]);
                                }
                            }
                            else
                            {
                                obj.srateType = "2";
                            }
                            obj.sGuaranteetype = Convert.ToString(dtEstimation.Rows[0]["RESTD_GUARANTEETYPE"]);
                            obj.sEstDate = Convert.ToString(dtEstimation.Rows[0]["RESTD_DATE"]);
                            obj.coiltype = Convert.ToString(dtEstimation.Rows[0]["RESTD_COIL_TYPE"]);
                            obj.sDtrCode = Convert.ToString(dtEstimation.Rows[0]["RESTD_TC_CODE"]);
                            if (dtEstimation.Columns.Contains("RESTD_PHASE"))
                            {
                                obj.sPhases = Convert.ToString(dtEstimation.Rows[0]["RESTD_PHASE"]);
                            }

                            obj.sEstimationNo = Convert.ToString(dtEstimation.Rows[0]["RESTD_NO"]);
                        }
                        else if (i == 1)
                        {
                            obj.sMaterialID = Convert.ToString(dtEstimation.Rows[0]["MRIM_ID"]);
                            obj.sMaterialName = Convert.ToString(dtEstimation.Rows[0]["MRIM_ITEM_NAME"]).Replace("ç", ",");
                            obj.sMaterialQnty = Convert.ToString(dtEstimation.Rows[0]["RESTM_ITEM_QNTY"]);
                            obj.sMaterialRate = Convert.ToString(dtEstimation.Rows[0]["MRI_BASE_RATE"]);
                            obj.sMaterialTax = Convert.ToString(dtEstimation.Rows[0]["MRI_TAX"]);
                            obj.sMaterialTotal = Convert.ToString(dtEstimation.Rows[0]["MRI_TOTAL"]);
                            obj.sMaterialunit = Convert.ToString(dtEstimation.Rows[0]["MRI_MEASUREMENT"]);
                            obj.sMaterialunitName = Convert.ToString(dtEstimation.Rows[0]["MD_NAME"]);
                            obj.sAmount = Convert.ToString(dtEstimation.Rows[0]["AMOUNT"]);
                            obj.sMaterialItemId = Convert.ToString(dtEstimation.Rows[0]["MRIM_ITEM_ID"]);
                            obj.dtMaterial = CreateDatatableFromString(obj);
                        }
                        else if (i == 2)
                        {
                            obj.sMaterialID = Convert.ToString(dtEstimation.Rows[0]["MRIM_ID"]);
                            obj.sMaterialName = Convert.ToString(dtEstimation.Rows[0]["MRIM_ITEM_NAME"]).Replace("ç", ",");
                            obj.sMaterialQnty = Convert.ToString(dtEstimation.Rows[0]["RESTM_ITEM_QNTY"]);
                            obj.sMaterialRate = Convert.ToString(dtEstimation.Rows[0]["MRI_BASE_RATE"]);
                            obj.sMaterialTax = Convert.ToString(dtEstimation.Rows[0]["MRI_TAX"]);
                            obj.sMaterialTotal = Convert.ToString(dtEstimation.Rows[0]["MRI_TOTAL"]);
                            obj.sMaterialunit = Convert.ToString(dtEstimation.Rows[0]["MRI_MEASUREMENT"]);
                            obj.sMaterialunitName = Convert.ToString(dtEstimation.Rows[0]["MD_NAME"]);
                            obj.sAmount = Convert.ToString(dtEstimation.Rows[0]["AMOUNT"]);
                            obj.sMaterialItemId = Convert.ToString(dtEstimation.Rows[0]["MRIM_ITEM_ID"]);
                            obj.dtLabour = CreateDatatableFromString(obj);
                        }
                        else if (i == 3)
                        {
                            obj.sMaterialID = Convert.ToString(dtEstimation.Rows[0]["MRIM_ID"]);
                            obj.sMaterialName = Convert.ToString(dtEstimation.Rows[0]["MRIM_ITEM_NAME"]).Replace("ç", ",");
                            obj.sMaterialQnty = Convert.ToString(dtEstimation.Rows[0]["RESTM_ITEM_QNTY"]);
                            obj.sMaterialRate = Convert.ToString(dtEstimation.Rows[0]["MRI_BASE_RATE"]);
                            obj.sMaterialTax = Convert.ToString(dtEstimation.Rows[0]["MRI_TAX"]);
                            obj.sMaterialTotal = Convert.ToString(dtEstimation.Rows[0]["MRI_TOTAL"]);
                            obj.sMaterialunit = Convert.ToString(dtEstimation.Rows[0]["MRI_MEASUREMENT"]);
                            obj.sAmount = Convert.ToString(dtEstimation.Rows[0]["AMOUNT"]);
                            obj.sMaterialunitName = Convert.ToString(dtEstimation.Rows[0]["MD_NAME"]);
                            obj.sMaterialItemId = Convert.ToString(dtEstimation.Rows[0]["MRIM_ITEM_ID"]);
                            obj.dtSalvage = CreateDatatableFromString(obj);
                        }
                        else if (i == 4)
                        {
                            obj.sFileId = Convert.ToString(dtEstimation.Rows[0]["ID"]);
                            obj.sFileName = Convert.ToString(dtEstimation.Rows[0]["NAME"]);
                            obj.sFileType = Convert.ToString(dtEstimation.Rows[0]["TYPE"]);
                            obj.sFilePath = Convert.ToString(dtEstimation.Rows[0]["PATH"]);
                            obj.dtDocuments = CreateDatatable(obj);
                        }

                        else if (i == 5)
                        {
                            if (dtEstimation.Columns.Contains("MRIM_ID"))
                            {
                                obj.sMaterialID = Convert.ToString(dtEstimation.Rows[0]["MRIM_ID"]);
                            }
                            if (dtEstimation.Columns.Contains("MRIM_ITEM_NAME"))
                            {
                                obj.sMaterialName = Convert.ToString(dtEstimation.Rows[0]["MRIM_ITEM_NAME"]).Replace("ç", ",");
                            }
                            if (dtEstimation.Columns.Contains("RESTM_ITEM_QNTY"))
                            {
                                obj.sMaterialQnty = Convert.ToString(dtEstimation.Rows[0]["RESTM_ITEM_QNTY"]);
                            }

                            if (dtEstimation.Columns.Contains("MRI_BASE_RATE"))
                            {
                                obj.sMaterialRate = Convert.ToString(dtEstimation.Rows[0]["MRI_BASE_RATE"]);
                            }
                            if (dtEstimation.Columns.Contains("MRI_TAX"))
                            {
                                obj.sMaterialTax = Convert.ToString(dtEstimation.Rows[0]["MRI_TAX"]);
                            }
                            if (dtEstimation.Columns.Contains("MRI_TOTAL"))
                            {
                                obj.sMaterialTotal = Convert.ToString(dtEstimation.Rows[0]["MRI_TOTAL"]);
                            }
                            if (dtEstimation.Columns.Contains("MRI_MEASUREMENT"))
                            {
                                obj.sMaterialunit = Convert.ToString(dtEstimation.Rows[0]["MRI_MEASUREMENT"]);
                            }
                            if (dtEstimation.Columns.Contains("AMOUNT"))
                            {
                                obj.sAmount = Convert.ToString(dtEstimation.Rows[0]["AMOUNT"]);
                            }
                            if (dtEstimation.Columns.Contains("MD_NAME"))
                            {
                                obj.sMaterialunitName = Convert.ToString(dtEstimation.Rows[0]["MD_NAME"]);
                            }
                            if (dtEstimation.Columns.Contains("MRIM_ITEM_ID"))
                            {
                                obj.sMaterialItemId = Convert.ToString(dtEstimation.Rows[0]["MRIM_ITEM_ID"]);
                            }
                            obj.dtOil = CreateDatatableFromString(obj);
                        }
                    }
                }
                return obj;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return obj;
            }
        }


        public DataTable CreateDatatable(ClsRepairerEstimate objEst)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("ID");
            dt.Columns.Add("NAME");
            dt.Columns.Add("TYPE");
            dt.Columns.Add("PATH");

            string[] sFileId = objEst.sFileId.Split('`');
            string[] sFileName = objEst.sFileName.Split('`');
            string[] sFileType = objEst.sFileType.Split('`');
            string[] sFilePath = objEst.sFilePath.Split('`');

            for (int i = 0; i < sFileId.Length; i++)
            {
                for (int j = 0; j < sFileName.Length; j++)
                {
                    for (int k = 0; k < sFileType.Length; k++)
                    {
                        for (int l = 0; l < sFilePath.Length; l++)
                        {
                            if (sFilePath[l] != "" && sFilePath[l] != " ")
                            {
                                DataRow dRow = dt.NewRow();
                                dRow["ID"] = sFileId[i];
                                dRow["NAME"] = sFileName[j];
                                dRow["TYPE"] = sFileType[k];
                                dRow["PATH"] = sFilePath[l];
                                dt.Rows.Add(dRow);
                                dt.AcceptChanges();
                            }
                            i++;
                            j++;
                            k++;
                        }

                    }
                }
            }
            return dt;
        }

        public DataTable CreateDatatableFromString(ClsRepairerEstimate objEst)
        {

            DataTable dt = new DataTable();

            dt.Columns.Add("MRIM_ID");
            dt.Columns.Add("MRIM_ITEM_NAME");
            dt.Columns.Add("RESTM_ITEM_QNTY");
            dt.Columns.Add("MRI_MEASUREMENT");
            dt.Columns.Add("MRI_BASE_RATE");
            dt.Columns.Add("MRI_TAX");
            dt.Columns.Add("MRI_TOTAL");
            dt.Columns.Add("MD_NAME");
            dt.Columns.Add("MRIM_ITEM_ID");
            dt.Columns.Add("AMOUNT");

            string[] sItemid = objEst.sMaterialID.Split('`');
            string[] sName = objEst.sMaterialName.Split('`');
            string[] sQnty = objEst.sMaterialQnty.Split('`');
            string[] sMeasure = objEst.sMaterialunit.Split('`');
            string[] sRate = objEst.sMaterialRate.Split('`');
            string[] sTax = objEst.sMaterialTax.Split('`');
            string[] sTotal = objEst.sMaterialTotal.Split('`');
            string[] sUnitName = objEst.sMaterialunitName.Split('`');
            string[] sAmounts = objEst.sAmount.Split('`');
            string[] sMaterialItemid = objEst.sMaterialItemId.Split('`');

            for (int i = 0; i < sItemid.Length; i++)
            {
                for (int j = 0; j < sName.Length; j++)
                {
                    for (int k = 0; k < sQnty.Length; k++)
                    {
                        for (int l = 0; l < sMeasure.Length; l++)
                        {
                            for (int m = 0; m < sRate.Length; m++)
                            {
                                for (int q = 0; q < sAmounts.Length; q++)
                                {
                                    for (int n = 0; n < sTax.Length; n++)
                                    {
                                        for (int o = 0; o < sTotal.Length; o++)
                                        {
                                            for (int r = 0; r < sMaterialItemid.Length; r++)
                                            {
                                                for (int p = 0; p < sUnitName.Length; p++)
                                                {
                                                    if (sUnitName[p] != "" && sUnitName[p] != " ")
                                                    {
                                                        DataRow dRow = dt.NewRow();
                                                        dRow["MRIM_ID"] = sItemid[i];
                                                        dRow["MRIM_ITEM_NAME"] = sName[j];
                                                        dRow["RESTM_ITEM_QNTY"] = sQnty[k];
                                                        dRow["MRI_MEASUREMENT"] = sMeasure[l];
                                                        dRow["MRI_BASE_RATE"] = sRate[m];
                                                        dRow["MRI_TAX"] = sTax[n];
                                                        dRow["MRI_TOTAL"] = sTotal[o];
                                                        dRow["MD_NAME"] = sUnitName[p];
                                                        dRow["AMOUNT"] = sAmounts[q];
                                                        dRow["MRIM_ITEM_ID"] = sMaterialItemid[r];
                                                        dt.Rows.Add(dRow);
                                                        dt.AcceptChanges();
                                                    }
                                                    i++;
                                                    j++;
                                                    k++;
                                                    l++;
                                                    m++;
                                                    q++;
                                                    n++;
                                                    o++;
                                                    r++;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return dt;
        }

        public bool RepairDateIsValid(string sRepair_Id)
        {
            string StrQry = string.Empty;
            try
            {
                NpgsqlCommand = new NpgsqlCommand();
                StrQry = "SELECT COUNT(*) FROM \"TBLMINORREPAIRITEMRATEMASTER\" WHERE \"MRI_TR_ID\"=:sRepair_Id AND NOW() BETWEEN \"MRI_EFFECTIVE_FROM\" AND \"MRI_EFFECTIVE_TO\"";
                NpgsqlCommand.Parameters.AddWithValue("sRepair_Id", Convert.ToInt32(sRepair_Id));
                int count = Convert.ToInt16(objcon.get_value(StrQry, NpgsqlCommand));
                if (count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }

        public bool RepairDateIsValid(string sRepair_Id,string offcode)
        {
            string StrQry = string.Empty;
            try
            {
                NpgsqlCommand = new NpgsqlCommand();
                StrQry = "SELECT COUNT(*) FROM \"TBLMINORREPAIRITEMRATEMASTER\" WHERE \"MRI_TR_ID\"=:sRepair_Id AND NOW() BETWEEN \"MRI_EFFECTIVE_FROM\" AND \"MRI_EFFECTIVE_TO\" and \"MRI_DIV_ID\" IN(SELECT \"DIV_ID\" FROM \"TBLDIVISION\" WHERE cast(\"DIV_CODE\" as text) = :offcode)";
                NpgsqlCommand.Parameters.AddWithValue("sRepair_Id", Convert.ToInt32(sRepair_Id));
                NpgsqlCommand.Parameters.AddWithValue("offcode", offcode);
                int count = Convert.ToInt16(objcon.get_value(StrQry, NpgsqlCommand));
                if (count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }
        public string GetFailId(string sFail_Id, string View = "")
        {
            string StrQry = string.Empty;
            try
            {
                NpgsqlCommand = new NpgsqlCommand();
                if (sFail_Id.Contains("-"))
                {
                    StrQry = "SELECT \"RESTD_ID\" FROM \"TBLREPAIRERESTIMATIONDETAILS\",\"TBLWORKFLOWOBJECTS\" WHERE \"WO_DATA_ID\"=CAST(\"RESTD_TC_CODE\" AS TEXT) AND \"WO_RECORD_ID\"=:sFail_Id";
                    NpgsqlCommand.Parameters.AddWithValue("sFail_Id", Convert.ToInt32(sFail_Id));
                }
                else
                {
                    if (View == "")
                    {
                        NpgsqlCommand = new NpgsqlCommand();
                        StrQry = "SELECT \"RESTD_ID\" FROM \"TBLREPAIRERESTIMATIONDETAILS\" WHERE \"RESTD_ID\"=:sFail_Id1";
                        NpgsqlCommand.Parameters.AddWithValue("sFail_Id1", Convert.ToInt32(sFail_Id));
                    }
                    else
                    {
                        NpgsqlCommand = new NpgsqlCommand();
                        StrQry = "SELECT \"RESTD_ID\" FROM \"TBLREPAIRERESTIMATIONDETAILS\" WHERE \"RESTD_ID\"=:sFail_Id2";
                        NpgsqlCommand.Parameters.AddWithValue("sFail_Id2", Convert.ToInt32(sFail_Id));
                    }
                }
                return objcon.get_value(StrQry, NpgsqlCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }

        public DataTable gettcdetails(string sDtrcode)
        {
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Parameters.AddWithValue("sDtrcode", Convert.ToDouble(sDtrcode));
                dt = objcon.FetchDataTable(" SELECT \"OM_NAME\",\"TM_NAME\",\"TC_CODE\" as \"DTR_CODE\" from  \"TBLTCMASTER\"  JOIN  \"TBLTRANSMAKES\" ON \"TC_MAKE_ID\" =  \"TM_ID\" JOIN \"TBLOMSECMAST\" ON \"TC_PREV_OFFCODE\" = cast(\"OM_CODE\" as text) WHERE \"TC_CODE\"=:sDtrcode", NpgsqlCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }

        }


        public object GetTcDetails(ClsRepairerEstimate objrepairer)
        {

            DataTable dtDetails = new DataTable();
            OleDbDataReader dr = null;

            try
            {

                NpgsqlCommand cmd = new NpgsqlCommand("sp_tcdetails");
                cmd.Parameters.AddWithValue("stccode", Convert.ToString(objrepairer.sDtrCode));
                dtDetails = objcon.FetchDataTable(cmd);

                //dr = objcon.Fetch(strQry);
                //dtDetails.Load(dr);

                if (dtDetails.Rows.Count > 0)
                {

                    //objrepairer.sDtcId = dtDetails.Rows[0]["DT_ID"].ToString();
                    // objrepairer.sDtcCode = dtDetails.Rows[0]["DT_CODE"].ToString();
                    //objrepairer.sDtcName = dtDetails.Rows[0]["DT_NAME"].ToString();
                    // objrepairer.sDtcServicedate = dtDetails.Rows[0]["DT_LAST_SERVICE_DATE"].ToString();
                    // objrepairer.sDtcLoadKw = dtDetails.Rows[0]["DT_TOTAL_CON_KW"].ToString();
                    // objrepairer.sDtcLoadHp = dtDetails.Rows[0]["DT_TOTAL_CON_HP"].ToString();
                    // objrepairer.sCommissionDate = dtDetails.Rows[0]["DT_TRANS_COMMISION_DATE"].ToString();

                    objrepairer.sDtcCapacity = Convert.ToDouble(dtDetails.Rows[0]["TC_CAPACITY"]).ToString();
                    objrepairer.sDtcLocation = dtDetails.Rows[0]["TC_LOCATION_ID"].ToString();
                    objrepairer.sDtcTcSlno = dtDetails.Rows[0]["TC_SLNO"].ToString();
                    objrepairer.sDtcTcMake = dtDetails.Rows[0]["TC_MAKE_ID"].ToString();

                    objrepairer.sDtcTcCode = dtDetails.Rows[0]["TC_CODE"].ToString();
                    objrepairer.sManfDate = dtDetails.Rows[0]["TC_MANF_DATE"].ToString();
                    objrepairer.sTCId = dtDetails.Rows[0]["TC_ID"].ToString();




                }

                return objrepairer;


                //else
                //{

                //    NpgsqlCommand cmd = new NpgsqlCommand("sp_getfailuredtcdetails");
                //    cmd.Parameters.AddWithValue("sdtccode", Convert.ToString(objrepairer.sDtcId));
                //    dtDetails = objcon.FetchDataTable(cmd);

                //    if (dtDetails.Rows.Count > 0)
                //    {
                //        objrepairer.sDtcId = dtDetails.Rows[0]["DT_ID"].ToString();
                //        objrepairer.sDtcCode = dtDetails.Rows[0]["DT_CODE"].ToString();
                //        objrepairer.sDtcName = dtDetails.Rows[0]["DT_NAME"].ToString();
                //        objrepairer.sDtcServicedate = dtDetails.Rows[0]["DT_LAST_SERVICE_DATE"].ToString();
                //        objrepairer.sDtcLoadKw = dtDetails.Rows[0]["DT_TOTAL_CON_KW"].ToString();
                //        objrepairer.sDtcLoadHp = dtDetails.Rows[0]["DT_TOTAL_CON_HP"].ToString();
                //        objrepairer.sCommissionDate = dtDetails.Rows[0]["DT_TRANS_COMMISION_DATE"].ToString();
                //        objrepairer.sDtcCapacity = dtDetails.Rows[0]["TC_CAPACITY"].ToString();
                //        objrepairer.sDtcLocation = dtDetails.Rows[0]["TC_LOCATION_ID"].ToString();
                //        objrepairer.sDtcTcSlno = dtDetails.Rows[0]["TC_SLNO"].ToString();
                //        objrepairer.sDtcTcMake = dtDetails.Rows[0]["TC_MAKE_ID"].ToString();
                //        objrepairer.sDtcTcCode = dtDetails.Rows[0]["TC_CODE"].ToString();
                //        objrepairer.sManfDate = dtDetails.Rows[0]["TC_MANF_DATE"].ToString();
                //        objrepairer.sTCId = dtDetails.Rows[0]["TC_ID"].ToString();
                //        objrepairer.sRating = dtDetails.Rows[0]["MD_NAME"].ToString();

                //        objrepairer.sConditionoftc = dtDetails.Rows[0]["TC_CONDITION"].ToString();
                //        string qry = string.Empty;
                //        if (objrepairer.sConditionoftc != "")
                //        {
                //            NpgsqlCommand = new NpgsqlCommand();
                //            NpgsqlCommand.Parameters.AddWithValue("sConditionoftc", Convert.ToDouble(objrepairer.sConditionoftc));
                //            qry = objcon.get_value("SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='COTC' AND \"MD_ID\"=:sConditionoftc", NpgsqlCommand);
                //        }
                //        else
                //        {
                //            qry = "Data Not Available";
                //        }

                //        objrepairer.sConditionoftc = qry;
                //        if (objrepairer.sEnhancedCapacity == null || objrepairer.sEnhancedCapacity == "")
                //        {
                //            objrepairer.sEnhancedCapacity = objrepairer.sDtcCapacity;
                //        }



                //    }

                //    return objrepairer;
                //}


            }

            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objrepairer;
            }

        }


        public DataTable LoadExistMaterials(ClsRepairerEstimate obj)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT \"MRIM_ID\", \"MRIM_ITEM_NAME\",\"MRIM_ITEM_ID\", \"MRI_QUANTITY\", \"MRI_MEASUREMENT\", CAST(\"MRI_BASE_RATE\" AS TEXT) \"MRI_BASE_RATE\", \"MD_NAME\", ";
                strQry += " trunc(\"MRI_TAX\",1) || '%' AS \"MRI_TAX\", \"MRI_TOTAL\" , 0 AS \"RESTM_ITEM_QNTY\" FROM \"TBLMINORREPAIRERITEMMASTER\" , ";
                strQry += " \"TBLMINORREPAIRITEMRATEMASTER\", \"TBLMASTERDATA\" WHERE \"MRIM_ID\" = \"MRI_MRIM_ID\" AND \"MRIM_ITEM_TYPE\" = 1 AND \"MRI_WOUNDTYPE\"=:sWoundType AND \"MRI_RATETYPE\" =:srateType";
                strQry += "  AND CAST(\"MD_ID\" AS TEXT)  = \"MRI_MEASUREMENT\"  AND  \"MD_TYPE\"='MSR'  AND CURRENT_DATE BETWEEN \"MRI_EFFECTIVE_FROM\" AND \"MRI_EFFECTIVE_TO\" ";
                strQry += " AND \"MRI_TR_ID\" = :eUser AND \"MRI_DIV_ID\" = (SELECT \"DIV_ID\" FROM \"TBLDIVISION\" WHERE \"DIV_CODE\"=:sOfficecode) AND \"MRI_CAPACITY\" = (SELECT \"MD_ID\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'C' AND \"MD_NAME\" =:eCapacity)";
                strQry += " ORDER BY \"MRIM_ITEM_ORDER\" ";
                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Parameters.AddWithValue("sWoundType", Convert.ToInt32(obj.sWoundType));
                NpgsqlCommand.Parameters.AddWithValue("srateType", Convert.ToInt32(obj.srateType));
                NpgsqlCommand.Parameters.AddWithValue("eUser", Convert.ToInt32(obj.sLastRepair));
                NpgsqlCommand.Parameters.AddWithValue("sOfficecode", Convert.ToInt32(obj.sOfficeCode));
                NpgsqlCommand.Parameters.AddWithValue("eCapacity", obj.scapacity);
                dt = objcon.FetchDataTable(strQry, NpgsqlCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable LoadExistLabour(ClsRepairerEstimate obj)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT \"MRIM_ID\", \"MRIM_ITEM_NAME\", \"MRI_QUANTITY\",\"MRIM_ITEM_ID\", \"MRI_MEASUREMENT\", CAST(\"MRI_BASE_RATE\" AS TEXT) \"MRI_BASE_RATE\", \"MD_NAME\", ";
                strQry += " trunc(\"MRI_TAX\",1) || '%' AS \"MRI_TAX\", \"MRI_TOTAL\", 0 AS \"RESTM_ITEM_QNTY\"  FROM \"TBLMINORREPAIRERITEMMASTER\" , ";
                strQry += " \"TBLMINORREPAIRITEMRATEMASTER\", \"TBLMASTERDATA\" WHERE \"MRIM_ID\" = \"MRI_MRIM_ID\" AND \"MRIM_ITEM_TYPE\" = 2 AND \"MRI_WOUNDTYPE\"=:sWoundType AND \"MRI_RATETYPE\"=:srateType";
                strQry += " AND CAST(\"MD_ID\" AS TEXT)  = \"MRI_MEASUREMENT\"  AND  \"MD_TYPE\"='MSR'   AND CURRENT_DATE BETWEEN \"MRI_EFFECTIVE_FROM\" AND \"MRI_EFFECTIVE_TO\" ";
                strQry += " AND \"MRI_TR_ID\" =:eUser AND \"MRI_DIV_ID\" = (SELECT \"DIV_ID\" FROM \"TBLDIVISION\" WHERE \"DIV_CODE\"=:sOfficecode)  AND \"MRI_CAPACITY\" = (SELECT \"MD_ID\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'C' AND \"MD_NAME\" =:eCapacity)";
                strQry += " ORDER BY \"MRIM_ITEM_ORDER\" ";
                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Parameters.AddWithValue("sWoundType", Convert.ToInt32(obj.sWoundType));
                NpgsqlCommand.Parameters.AddWithValue("srateType", Convert.ToInt32(obj.srateType));
                NpgsqlCommand.Parameters.AddWithValue("eUser", Convert.ToInt32(obj.sLastRepair));
                NpgsqlCommand.Parameters.AddWithValue("sOfficecode", Convert.ToInt32(obj.sOfficeCode));
                NpgsqlCommand.Parameters.AddWithValue("eCapacity", obj.scapacity);
                dt = objcon.FetchDataTable(strQry, NpgsqlCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable LoadExistSalvage(ClsRepairerEstimate obj)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT \"MRIM_ID\", \"MRIM_ITEM_NAME\", \"MRI_QUANTITY\",\"MRIM_ITEM_ID\", \"MRI_MEASUREMENT\", CAST(\"MRI_BASE_RATE\" AS TEXT) \"MRI_BASE_RATE\", \"MD_NAME\", ";
                strQry += " trunc(\"MRI_TAX\",1) || '%' AS \"MRI_TAX\", \"MRI_TOTAL\" , 0 AS \"RESTM_ITEM_QNTY\" FROM \"TBLMINORREPAIRERITEMMASTER\" , ";
                strQry += " \"TBLMINORREPAIRITEMRATEMASTER\", \"TBLMASTERDATA\" WHERE \"MRIM_ID\" = \"MRI_MRIM_ID\" AND \"MRIM_ITEM_TYPE\" = 3 AND \"MRI_WOUNDTYPE\"=:sWoundType AND \"MRI_RATETYPE\" =:srateType";
                strQry += "  AND CAST(\"MD_ID\" AS TEXT)  = \"MRI_MEASUREMENT\"  AND  \"MD_TYPE\"='MSR'   AND CURRENT_DATE BETWEEN \"MRI_EFFECTIVE_FROM\" AND \"MRI_EFFECTIVE_TO\" ";
                strQry += " AND \"MRI_TR_ID\" =:eUser AND \"MRI_DIV_ID\" = (SELECT \"DIV_ID\" FROM \"TBLDIVISION\" WHERE \"DIV_CODE\"=:sOfficecode)  AND \"MRI_CAPACITY\" = (SELECT \"MD_ID\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'C' AND \"MD_NAME\" =:eCapacity)";
                strQry += " ORDER BY \"MRIM_ITEM_ORDER\" ";
                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Parameters.AddWithValue("sWoundType", Convert.ToInt32(obj.sWoundType));
                NpgsqlCommand.Parameters.AddWithValue("srateType", Convert.ToInt32(obj.srateType));
                NpgsqlCommand.Parameters.AddWithValue("eUser", Convert.ToInt32(obj.sLastRepair));
                NpgsqlCommand.Parameters.AddWithValue("sOfficecode", Convert.ToInt32(obj.sOfficeCode));
                NpgsqlCommand.Parameters.AddWithValue("eCapacity", obj.scapacity);
                dt = objcon.FetchDataTable(strQry, NpgsqlCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }


        public DataTable LoadExistOil(ClsRepairerEstimate obj)
        {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            try
            {
                string strQry = string.Empty;
                strQry = " SELECT \"ROI_ID\" as \"MRIM_ID\",\"ROI_ITEM_NAME\" as \"MRIM_ITEM_NAME\",trunc(18,1) || '%' AS \"MRI_TAX\",1 as \"ROI_QUANTITY\",";
                   strQry += " \"ROI_ITEM_ID\" as \"MRIM_ITEM_ID\", \"ROI_ITEM_RATE\" as \"MRI_BASE_RATE\",\"MD_ID\" as \"MRI_MEASUREMENT\",\"MD_NAME\",0 as \"MRI_TOTAL\" ,cast(0 as numeric) AS \"RESTM_ITEM_QNTY\" from \"TBLREPAIREROILITEMMASTER\",\"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'MSR' and \"MD_ID\"=7  ";
               
                dt = objcon.FetchDataTable(strQry);

                strQry = "SELECT (cast(a.\"TC_OIL_CAPACITY\" as numeric)/100.00)*80 as \"OIL_CAPACITY\" from  (SELECT \"TC_OIL_CAPACITY\" from \"TBLTCMASTER\" WHERE \"TC_CODE\"='"+ obj.sDtcTcCode + "')a";
                string oil = objcon.get_value(strQry);

                dt.Rows[2][9] =Convert.ToDouble(oil);

                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
    }
}

