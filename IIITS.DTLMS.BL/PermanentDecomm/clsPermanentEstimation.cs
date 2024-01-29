using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Threading.Tasks;


namespace IIITS.DTLMS.BL
{
    public class clsPermanentEstimation
    {

        string strFormCode = "clsPermanentEstimation";

        public string sConditionoftc { get; set; }
        public string sDtcId { get; set; }
        public string sDtcCode { get; set; }
        public string sDtcName { get; set; }
        public string sDtcServicedate { get; set; }
        public string sDtcLoadKw { get; set; }
        public string sDtcLoadHp { get; set; }
        public string sCommissionDate { get; set; }
        public string sDtcCapacity { get; set; }
        public string sDtcLocation { get; set; }
        public string sDtcTcSlno { get; set; }
        public string sDtcTcMake { get; set; }
        public string sDtcReadings { get; set; }
        public string sDtcTcCode { get; set; }
        public string sFailureId { get; set; }
        public string sAlternateReplaceType { get; set; }
        public string sCrby { get; set; }
        public string sOfficeCode { get; set; }
        public string sDtrSaveCommissionDate { get; set; }
        public string sDTrCommissionDate { get; set; }
        public string sDTrEnumerationDate { get; set; }
        public string sManfDate { get; set; }
        public string sTCId { get; set; }

        public string sReasons { get; set; }


        public string sLastRepairedBy { get; set; }
        public string sLastRepairedDate { get; set; }
        public string sGuarantyType { get; set; }
        public string sGuarantySource { get; set; }

        public string sFormName { get; set; }
        public string sRating { get; set; }
        public string sClientIP { get; set; }
        public string sWFDataId { get; set; }


        
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

        

        public string sEmployeeCost { get; set; }
        public string sESI { get; set; }
        public string ServiceTax { get; set; }
        public string DecomLabourCost { get; set; }

       
        public DataTable dtLabour { get; set; }
        
        public string sWFO_id { get; set; }
        public string sFailType { get; set; }
        public string sEstComment { get; set; }
        public string sDtrCode { get; set; }
        public string sActionType { get; set; }   
       

        public string sWoundType { get; set; }
        public string sFileId { get; set; }
        public string sFileName { get; set; }
        public string sFileType { get; set; }
        public string sFilePath { get; set; }
        public string sGuaranteetype { get; set; }
        public string sremarks { get; set; }
        public string sEstDate { get; set; }
        public string sFinalTotalAmount { get; set; }
        public string sMaterialItemId { get; set; }

        public string sMaterialID { get; set; }
        public string sMaterialName { get; set; }
        public string sMaterialQnty { get; set; }
        public string sMaterialRate { get; set; }
        public string sMaterialTax { get; set; }
        public string sMaterialTotal { get; set; }
        public string sMaterialunit { get; set; }
        public string sMaterialunitName { get; set; }

        public string eUser { get; set; }
        public string eCapacity { get; set; }

        public DataTable dtMaterial { get; set; }

        string sLabMaterialremarks = string.Empty;
        string slabtaxamount = string.Empty;
        string slabamount = string.Empty;

        public string sFirstGuarantyType = string.Empty;

        PGSqlConnection objcon = new PGSqlConnection(Constants.Password);

        public DataTable LoadAllDTCEstimate(clsPermanentEstimation objperestimate)
        {
            DataTable dtDetails = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("sp_loadalldtcestimate");
                cmd.Parameters.AddWithValue("sofficecode", Convert.ToString(objperestimate.sOfficeCode));
                dtDetails = objcon.FetchDataTable(cmd);
                return dtDetails;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtDetails;

            }

        }

        public DataTable LoadAlreadyFailure(clsPermanentEstimation objFailure)
        {
            DataTable dtDetails = new DataTable();
            try
            {
               
                NpgsqlCommand cmd = new NpgsqlCommand("sp_loadalreadypermanentestimate");
                cmd.Parameters.AddWithValue("sofficecode", Convert.ToString(objFailure.sOfficeCode));
                dtDetails = objcon.FetchDataTable(cmd);
                return dtDetails;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtDetails;

            }

        }


        public string[] SavePermanentEstimation(clsPermanentEstimation objestimation,  string[] sLabour)
        {
            string[] Arr = new string[4];
            try
            {
              
                StringBuilder sbQuery = new StringBuilder();
                string sQry = string.Empty;
                sQry = "INSERT INTO \"TBLPERMANENTESTIMATIONDETAILS\" (\"PEST_ID\", \"PEST_NO\",\"PEST_CAPACITY\",\"PEST_WOUNDTYPE\",\"PEST_REPAIRER\",\"PEST_CRBY\",\"PEST_FAIL_TYPE\", \"PEST_GUARANTEETYPE\",\"PEST_ITEM_TOTAL\",\"PEST_DATE\",\"PEST_LOC_CODE\",\"PEST_DTC_CODE\",\"PEST_TC_CODE\",\"PEST_REASONS\")";
                sQry += " VALUES('{0}',(SELECT \"PERMANENTESTIMATIONNUMBER\"('" + objestimation.sOfficeCode + "')), ";
                sQry += " '" + objestimation.sFaultCapacity + "', '" + objestimation.sWoundType + "', '" + objestimation.sLastRepair + "', '" + objestimation.sCrby + "','" + objestimation.sFailType + "','" + objestimation.sGuaranteetype + "','" + objestimation.sFinalTotalAmount + "',TO_DATE('" + objestimation.sEstDate + "','dd/MM/yyyy'),'" + objestimation.sOfficeCode + "'," + objestimation.sDtcCode + "," + objestimation.sDtrCode + ",'"+objestimation.sReasons+"')";

                sQry = sQry.Replace("'", "''");

                string[] sLabourVal = sLabour.ToArray();

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

                            sQry2 = "INSERT INTO \"TBLPERMANENTESTIMATIONMATERIAL\" (\"PESTM_ID\", \"PESTM_EST_ID\", \"PESTM_ITEM_ID\", \"PESTM_ITEM_QNTY\", \"PESTM_ITEM_RATE\", \"PESTM_ITEM_TAX\", \"PESTM_ITEM_TOTAL\")";
                            sQry2 += " VALUES ((SELECT COALESCE(MAX(\"PESTM_ID\"),0)+1 FROM \"TBLPERMANENTESTIMATIONMATERIAL\"), '{0}' , '" + sLabourVal[i].Split('~').GetValue(0).ToString() + "', '" + sLabourVal[i].Split('~').GetValue(1).ToString() + "', ";
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

                if (objestimation.sFailType == "1")
                {
                    string LocCode = objestimation.sOfficeCode.Substring(0, 3);

                    String sQry4 = string.Empty;
                    sQry4 = "UPDATE \"TBLTCMASTER\" SET \"TC_LOCATION_ID\"='" + clsStoreOffice.GetStoreID(LocCode) + "',\"TC_CURRENT_LOCATION\"='3',\"TC_LAST_FAILURE_TYPE\"='" + objestimation.sFailType + "' WHERE \"TC_CODE\"='" + objestimation.sDtrCode + "'";
                    sbQuery.Append(sQry4);
                    sbQuery.Append(";");
                }
                else
                {
                    String sQry4 = string.Empty;
                    sQry4 = "UPDATE \"TBLTCMASTER\" SET \"TC_LAST_FAILURE_TYPE\"='" + objestimation.sFailType + "' WHERE \"TC_CODE\"='" + objestimation.sDtrCode + "'";
                    sbQuery.Append(sQry4);
                    sbQuery.Append(";");
                }


                string sFileID = string.Empty;
                string sFileName = string.Empty;
                string sFilePath = string.Empty;
                string sFileType = string.Empty;
                String sQry5 = string.Empty;
                sQry5 = "UPDATE \"TBLTCMASTER\" SET \"TC_CURRENT_LOCATION\"='6' WHERE \"TC_CODE\"='" + objestimation.sDtrCode + "'";
                sbQuery.Append(sQry5);
                sbQuery.Append(";");
                DataTable dt = new DataTable();

                sbQuery = sbQuery.Replace("'", "''");

                string sParam = "SELECT COALESCE(MAX(\"PEST_ID\"),0)+1 FROM \"TBLPERMANENTESTIMATIONDETAILS\"";

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "Permanentestimation";
                // objApproval.sRecordId = objFailureDetails.sFailureId;
                objApproval.sOfficeCode = objestimation.sOfficeCode;
                objApproval.sClientIp = objestimation.sClientIP;
                objApproval.sCrby = objestimation.sCrby;
                objApproval.sQryValues = sQry + ";" + sbQuery;
                objApproval.sParameterValues = sParam;
                objApproval.sMainTable = "TBLPERMANENTESTIMATIONDETAILS";
                objApproval.sDataReferenceId = objestimation.sDtcCode;
                objApproval.sDescription = "PermanentEstimation Request for DTC CODE  " + objestimation.sDtcCode;
                objApproval.sRefOfficeCode = objestimation.sOfficeCode;
                objApproval.sWFObjectId = objestimation.sWFO_id;

                string sPrimaryKey = "{0}";
                string sSecPrimaryKey = "{1}";

                objApproval.sColumnNames = "PEST_ID,PEST_NO,PEST_CAPACITY,PEST_WOUNDTYPE,PEST_REPAIRER,PEST_CRBY,PEST_FAIL_TYPE,PEST_GUARANTEETYPE,PEST_DATE,PEST_LOC_CODE,PEST_DTC_CODE,PEST_TC_CODE,PEST_REASONS";
                objApproval.sColumnNames += ";PESTM_ID,MRIM_ID,PESTM_ITEM_QNTY,MRI_BASE_RATE,MRI_TAX,MRI_TOTAL,MRIM_ITEM_NAME,MRI_MEASUREMENT,MD_NAME,MRIM_REMARKS,PESTM_ITEM_TAX,AMOUNT,MRIM_ITEM_ID";
                objApproval.sColumnValues = "" + sPrimaryKey + "," + objestimation.sEstimationNo + ",";
                objApproval.sColumnValues += "" + objestimation.sFaultCapacity + "," + objestimation.sWoundType + "," + objestimation.sLastRepair + "," + objestimation.sCrby + "," + objestimation.sFailType + "," + objestimation.sGuaranteetype + "," + objestimation.sEstDate + "," + objestimation.sOfficeCode + "," + objestimation.sDtcCode + "," + objestimation.sDtrCode + ","+objestimation.sReasons+" ";
                objApproval.sColumnValues += ";" + sSecPrimaryKey + "," + sLabID + "," + sLabQnty + "," + sLabRate + "," + sLabTax + "," + sLabTotal + "," + sLabName + "," + sLabUnit + "," + sLabUnitName + "," + sLabMaterialremarks + "," + slabtaxamount + "," + slabamount + "," + sLabItemId + "";
                objApproval.sTableNames = "TBLPERMANENTESTIMATIONDETAILS;TBLPERMANENTESTIMATIONMATERIAL;";

                if (objestimation.sActionType == "M")
                {
                    objApproval.SaveWorkFlowData(objApproval);
                    objestimation.sWFDataId = objApproval.sWFDataId;
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

        NpgsqlCommand NpgsqlCommand;
        public clsPermanentEstimation GetDetailsfromMainDB(clsPermanentEstimation obj)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                String sQry = String.Empty;
                NpgsqlCommand.Parameters.AddWithValue("EstimationId",Convert.ToInt32(obj.sEstimationId ));
                sQry = "SELECT \"PEST_DTC_CODE\",\"PEST_TC_CODE\",\"PEST_NO\",\"DT_NAME\",\"PEST_CAPACITY\",TO_CHAR(\"PEST_DATE\",'DD/MM/YYYY') \"PEST_DATE\", \"PEST_REPAIRER\", \"PEST_CRBY\", \"PEST_FAIL_TYPE\",\"PEST_WOUNDTYPE\", \"PEST_GUARANTEETYPE\" FROM ";
                sQry += " \"TBLPERMANENTESTIMATIONDETAILS\",\"TBLDTCMAST\" WHERE \"PEST_ID\" = :EstimationId and cast(\"PEST_DTC_CODE\" as text)=\"DT_CODE\" ";
                DataTable dt = new DataTable();
                dt = objcon.FetchDataTable(sQry, NpgsqlCommand);
                if (dt.Rows.Count > 0)
                {
                   // obj.sFailureId = Convert.ToString(dt.Rows[0]["EST_FAILUREID"]);
                    obj.sFaultCapacity = Convert.ToString(dt.Rows[0]["PEST_CAPACITY"]);
                    obj.sLastRepair = Convert.ToString(dt.Rows[0]["PEST_REPAIRER"]);
                    obj.sCrby = Convert.ToString(dt.Rows[0]["PEST_CRBY"]);
                    obj.sFailType = Convert.ToString(dt.Rows[0]["PEST_FAIL_TYPE"]);
                    obj.sWoundType = Convert.ToString(dt.Rows[0]["PEST_WOUNDTYPE"]);
                    obj.sGuaranteetype = Convert.ToString(dt.Rows[0]["PEST_GUARANTEETYPE"]);
                    obj.sDtcCode = Convert.ToString(dt.Rows[0]["PEST_DTC_CODE"]);
                    obj.sDtrCode = Convert.ToString(dt.Rows[0]["PEST_TC_CODE"]);
                    obj.sEstimationNo = Convert.ToString(dt.Rows[0]["PEST_NO"]);
                    obj.sDtcName = Convert.ToString(dt.Rows[0]["DT_NAME"]);
                    obj.sEstDate = Convert.ToString(dt.Rows[0]["PEST_DATE"]);
                    if (dt.Columns.Contains("PEST_REASONS"))
                    {
                        obj.sReasons = Convert.ToString(dt.Rows[0]["PEST_REASONS"]);
                    }
                    else
                    {
                         obj.sReasons ="";
                    }
                    
                   
                }



                NpgsqlCommand.Parameters.AddWithValue("EstimationId1",Convert.ToInt32(obj.sEstimationId));
                sQry = " SELECT \"PEST_ITEM_ID\" AS \"MRIM_ID\", \"MRIM_ITEM_NAME\" \"MRIM_ITEM_NAME\" , \"MRIM_ITEM_ID\" \"MRIM_ITEM_ID\",\"PESTM_ITEM_QNTY\" \"PESTM_ITEM_QNTY\", ";
                sQry += " \"PESTM_ITEM_RATE\" \"MRI_BASE_RATE\", \"PESTM_ITEM_TAX\" \"MRI_TAX\", \"PESTM_ITEM_TOTAL\" \"MRI_TOTAL\",  \"MRI_MEASUREMENT\" ";
                sQry += " \"MRI_MEASUREMENT\", B.\"MD_NAME\" \"MD_NAME\" FROM \"TBLPERMANENTESTIMATIONMATERIAL\", \"TBLMINORREPAIRERITEMMASTER\", ";
                sQry += " \"TBLPERMANENTESTIMATIONDETAILS\", \"TBLMINORREPAIRITEMRATEMASTER\", (SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" ";
                sQry += " WHERE \"MD_TYPE\" = 'C')A, (SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'MSR')B WHERE ";
                sQry += " \"PESTM_ITEM_ID\" = \"MRIM_ID\"  AND \"PEST_ID\" = \"PESTM_EST_ID\" AND \"MRIM_ID\" = \"MRI_MRIM_ID\" AND \"PEST_CAPACITY\" = ";
                sQry += " CAST(A.\"MD_NAME\" AS INT) AND A.\"MD_ID\" = \"MRI_CAPACITY\" AND \"PESTM_EST_ID\" = :EstimationId1 AND \"PEST_WOUNDTYPE\" = \"MRI_WOUNDTYPE\" ";
                sQry += " AND \"PEST_REPAIRER\" = \"MRI_TR_ID\" AND CAST(\"MRI_MEASUREMENT\" AS INT)  = B.\"MD_ID\" AND  \"MRI_STATUS_FLAG\" = 0 ";
                sQry += " AND \"PEST_CRON\" BETWEEN \"MRI_EFFECTIVE_FROM\" AND \"MRI_EFFECTIVE_TO\"   AND \"MRIM_ITEM_TYPE\" = 2 ORDER BY \"MRIM_ITEM_ORDER\" ";
                obj.dtLabour = objcon.FetchDataTable(sQry, NpgsqlCommand);

                return obj;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return obj;
            }
        }

        public clsPermanentEstimation GetEstimateDetailsFromXML(clsPermanentEstimation obj)
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
                            obj.sFailureId = Convert.ToString(dtEstimation.Rows[0]["PEST_ID"]);
                            //obj.sOfficeCode = Convert.ToString(dtEstimation.Rows[0]["SI_DATE"]);
                            obj.sFaultCapacity = Convert.ToString(dtEstimation.Rows[0]["PEST_CAPACITY"]);
                            obj.sLastRepair = Convert.ToString(dtEstimation.Rows[0]["PEST_REPAIRER"]);
                            obj.sCrby = Convert.ToString(dtEstimation.Rows[0]["PEST_CRBY"]);
                            obj.sFailType = Convert.ToString(dtEstimation.Rows[0]["PEST_FAIL_TYPE"]);
                            obj.sWoundType = Convert.ToString(dtEstimation.Rows[0]["PEST_WOUNDTYPE"]);
                            obj.sGuaranteetype = Convert.ToString(dtEstimation.Rows[0]["PEST_GUARANTEETYPE"]);
                            obj.sEstDate = Convert.ToString(dtEstimation.Rows[0]["PEST_DATE"]);
                            if (dtEstimation.Columns.Contains("PEST_REASONS"))
                            {
                                
                                 obj.sReasons = Convert.ToString(dtEstimation.Rows[0]["PEST_REASONS"]);
                            }
                            else
                            {
                                obj.sReasons = "";
                                 
                           }
                           
                        }
                        else if (i == 1)
                        {
                            obj.sMaterialID = Convert.ToString(dtEstimation.Rows[0]["MRIM_ID"]);
                            obj.sMaterialName = Convert.ToString(dtEstimation.Rows[0]["MRIM_ITEM_NAME"]).Replace("ç", ",");
                            obj.sMaterialQnty = Convert.ToString(dtEstimation.Rows[0]["PESTM_ITEM_QNTY"]);
                            obj.sMaterialRate = Convert.ToString(dtEstimation.Rows[0]["MRI_BASE_RATE"]);
                            obj.sMaterialTax = Convert.ToString(dtEstimation.Rows[0]["MRI_TAX"]);
                            obj.sMaterialTotal = Convert.ToString(dtEstimation.Rows[0]["MRI_TOTAL"]);
                            obj.sMaterialunit = Convert.ToString(dtEstimation.Rows[0]["MRI_MEASUREMENT"]);
                            obj.sMaterialunitName = Convert.ToString(dtEstimation.Rows[0]["MD_NAME"]);
                            obj.sAmount = Convert.ToString(dtEstimation.Rows[0]["AMOUNT"]);
                            obj.sMaterialItemId = Convert.ToString(dtEstimation.Rows[0]["MRIM_ITEM_ID"]);
                            
                            obj.dtLabour = CreateDatatableFromString(obj);
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


        public DataTable CreateDatatableFromString(clsPermanentEstimation objEst)
        {

            DataTable dt = new DataTable();

            dt.Columns.Add("MRIM_ID");
            dt.Columns.Add("MRIM_ITEM_NAME");
            dt.Columns.Add("PESTM_ITEM_QNTY");
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
                                                        dRow["PESTM_ITEM_QNTY"] = sQnty[k];
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

        public void SavePermenantEstimationDetails(clsPermanentEstimation objEstimation)
        {
            try
            {
                string strQry = string.Empty;

                //OleDbDataReader dr = objcon.Fetch("SELECT * FROM TBLESTIMATION WHERE EST_DF_ID='"+ objEstimation.sFailureId +"'");
                //if (dr.Read())
                //{
                //    dr.Close();
                //    return;
                //}
                //dr.Close();

                //string sEstId = objcon.get_value("SELECT \"PEST_ID\" FROM \"TBLPERMANENTESTIMATION\" WHERE \"EST_DF_ID\" ='" + objEstimation.sFailureId + "'");
                //if (sEstId.Length > 0)
                //{
                //    return;
                //}
                //string sMaxNo = Convert.ToString(objcon.Get_max_no("EST_ID", "TBLESTIMATION"));

               // GetCommEstimatedDetails(objEstimation);
                GetDecomEstimatedDetails(objEstimation);

                objEstimation.sEstimationNo = GenerateEstimationNo(objEstimation.sOfficeCode);

                if (objEstimation.sLastRepair == null || objEstimation.sLastRepair == "")
                {
                    objEstimation.sLastRepair = "0";
                }

                string[] Arr = new string[2];
                NpgsqlCommand cmd = new NpgsqlCommand("sp_savepermanentestimationdetails");
                //cmd.Parameters.AddWithValue("sfailureid", sFailureId);
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


        public clsPermanentEstimation GetDecomEstimatedDetails(clsPermanentEstimation objEstimation)
        {
            NpgsqlCommand = new NpgsqlCommand();
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

        public string GenerateEstimationNo(string sOfficeCode)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                string sMaxNo = string.Empty;
                NpgsqlCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode == null ? "" : sOfficeCode);
                string sEstNo = objcon.get_value("SELECT CAST(MAX(\"PEST_NO\")+1 AS TEXT) FROM \"TBLPERMANENTESTIMATIONDETAILS\" WHERE CAST(\"PEST_NO\" AS TEXT) LIKE :OfficeCode||'%' ", NpgsqlCommand);

                if (sEstNo == "")
                {
                    //sMaxNo = "001";
                    sEstNo = sOfficeCode + "001";
                }
                //else
                //{
                //    sEstNo = objcon.get_value("SELECT MAX(EST_ID)+1 FROM TBLESTIMATION WHERE EST_NO LIKE '" + sOfficeCode + "%'");
                //}            

                return sEstNo;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        public DataTable LoadExistLabour(clsPermanentEstimation obj)
        {
            NpgsqlCommand = new NpgsqlCommand();
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                NpgsqlCommand.Parameters.AddWithValue("WoundType",Convert.ToInt32( obj.sWoundType));
                NpgsqlCommand.Parameters.AddWithValue("User",Convert.ToInt32( obj.eUser));
                NpgsqlCommand.Parameters.AddWithValue("Capacity", obj.eCapacity);
                strQry = "SELECT \"MRIM_ID\", \"MRIM_ITEM_NAME\", \"MRI_QUANTITY\",\"MRIM_ITEM_ID\", \"MRI_MEASUREMENT\", CAST(\"MRI_BASE_RATE\" AS TEXT) \"MRI_BASE_RATE\", \"MD_NAME\", ";
                strQry += " trunc(\"MRI_TAX\",1) || '%' AS \"MRI_TAX\", \"MRI_TOTAL\", 0 AS \"PESTM_ITEM_QNTY\"  FROM \"TBLMINORREPAIRERITEMMASTER\" , ";
                strQry += " \"TBLMINORREPAIRITEMRATEMASTER\", \"TBLMASTERDATA\" WHERE \"MRIM_ID\" = \"MRI_MRIM_ID\" AND \"MRIM_ITEM_TYPE\" = 2 AND \"MRI_WOUNDTYPE\"=:WoundType";
                strQry += " AND CAST(\"MD_ID\" AS TEXT)  = \"MRI_MEASUREMENT\"  AND  \"MD_TYPE\"='MSR'   AND CURRENT_DATE BETWEEN \"MRI_EFFECTIVE_FROM\" AND \"MRI_EFFECTIVE_TO\" ";
                strQry += " AND \"MRI_TR_ID\" = :User  AND \"MRI_CAPACITY\" = (SELECT \"MD_ID\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'C' AND \"MD_NAME\" = :Capacity)";
                strQry += " ORDER BY \"MRIM_ITEM_ORDER\" ";
                dt = objcon.FetchDataTable(strQry, NpgsqlCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public bool RepairDateIsValid(string sRepair_Id)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string StrQry = string.Empty;
            try
            {
                NpgsqlCommand.Parameters.AddWithValue("RepairId", Convert.ToInt32(sRepair_Id));
                StrQry = "SELECT COUNT(*) FROM \"TBLMINORREPAIRITEMRATEMASTER\" WHERE \"MRI_TR_ID\"=:RepairId AND NOW() BETWEEN \"MRI_EFFECTIVE_FROM\" AND \"MRI_EFFECTIVE_TO\"";
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

        public object GetFailureDetails(clsPermanentEstimation objest)
        {

            DataTable dtDetails = new DataTable();
        
            try
            {
                if (objest.sDtcId != "0")
                {


                    NpgsqlCommand cmd = new NpgsqlCommand("sp_searchallestimatedetails");
                    cmd.Parameters.AddWithValue("sdtcId", Convert.ToString(objest.sDtcId));
                    dtDetails = objcon.FetchDataTable(cmd);


                    if (dtDetails.Rows.Count > 0)
                    {
                       
                        objest.sDtcId = dtDetails.Rows[0]["DT_ID"].ToString();
                        objest.sDtcCode = dtDetails.Rows[0]["DT_CODE"].ToString();
                        objest.sDtcName = dtDetails.Rows[0]["DT_NAME"].ToString();
                        objest.sDtcServicedate = dtDetails.Rows[0]["DT_LAST_SERVICE_DATE"].ToString();
                        objest.sDtcLoadKw = dtDetails.Rows[0]["DT_TOTAL_CON_KW"].ToString();
                        objest.sDtcLoadHp = dtDetails.Rows[0]["DT_TOTAL_CON_HP"].ToString();
                        objest.sCommissionDate = dtDetails.Rows[0]["DT_TRANS_COMMISION_DATE"].ToString();
                        objest.sDtcCapacity = Convert.ToDouble(dtDetails.Rows[0]["TC_CAPACITY"]).ToString();
                        objest.sDtcLocation = dtDetails.Rows[0]["TC_LOCATION_ID"].ToString();
                        objest.sDtcTcSlno = dtDetails.Rows[0]["TC_SLNO"].ToString();
                        objest.sDtcTcMake = dtDetails.Rows[0]["TC_MAKE_ID"].ToString();
                       // objest.sDtcReadings = dtDetails.Rows[0]["DF_KWH_READING"].ToString();
                        objest.sDtcTcCode = dtDetails.Rows[0]["TC_CODE"].ToString();
                       // objest.sFailureId = dtDetails.Rows[0]["DF_ID"].ToString();
                        objest.sCrby = dtDetails.Rows[0]["US_FULL_NAME"].ToString();
                         objest.sOfficeCode = dtDetails.Rows[0]["TC_LOCATION_ID"].ToString();
                        objest.sManfDate = dtDetails.Rows[0]["TC_MANF_DATE"].ToString();
                        objest.sTCId = dtDetails.Rows[0]["TC_ID"].ToString();

                        //GetDTRCommissionDate(objFailureDetails);
                      //  objest.sGuarantyType = dtDetails.Rows[0]["DF_GUARANTY_TYPE"].ToString();
                       // objest.sDTrCommissionDate = dtDetails.Rows[0]["DF_DTR_COMMISSION_DATE"].ToString();
                        GetLastRepairedDetails(objest);

                    }

                    return objest;
                }

                else
                {

                    NpgsqlCommand cmd = new NpgsqlCommand("sp_searchallestimatedetails");
                    cmd.Parameters.AddWithValue("sdtcId", Convert.ToString(objest.sDtcId));
                    dtDetails = objcon.FetchDataTable(cmd);

                    if (dtDetails.Rows.Count > 0)
                    {
                        objest.sDtcId = dtDetails.Rows[0]["DT_ID"].ToString();
                        objest.sDtcCode = dtDetails.Rows[0]["DT_CODE"].ToString();
                        objest.sDtcName = dtDetails.Rows[0]["DT_NAME"].ToString();
                        objest.sDtcServicedate = dtDetails.Rows[0]["DT_LAST_SERVICE_DATE"].ToString();
                        objest.sDtcLoadKw = dtDetails.Rows[0]["DT_TOTAL_CON_KW"].ToString();
                        objest.sDtcLoadHp = dtDetails.Rows[0]["DT_TOTAL_CON_HP"].ToString();
                        objest.sCommissionDate = dtDetails.Rows[0]["DT_TRANS_COMMISION_DATE"].ToString();
                        objest.sDtcCapacity = dtDetails.Rows[0]["TC_CAPACITY"].ToString();
                        objest.sDtcLocation = dtDetails.Rows[0]["TC_LOCATION_ID"].ToString();
                        objest.sDtcTcSlno = dtDetails.Rows[0]["TC_SLNO"].ToString();
                        objest.sDtcTcMake = dtDetails.Rows[0]["TC_MAKE_ID"].ToString();
                        objest.sDtcTcCode = dtDetails.Rows[0]["TC_CODE"].ToString();
                        objest.sManfDate = dtDetails.Rows[0]["TC_MANF_DATE"].ToString();
                        objest.sTCId = dtDetails.Rows[0]["TC_ID"].ToString();
                        objest.sRating = dtDetails.Rows[0]["MD_NAME"].ToString();

                        objest.sConditionoftc = dtDetails.Rows[0]["TC_CONDITION"].ToString();
                        string qry = string.Empty;
                        if (objest.sConditionoftc != "")
                        {
                            NpgsqlCommand.Parameters.AddWithValue("Conditionoftc",Convert.ToDouble(objest.sConditionoftc));
                            qry = objcon.get_value("SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='COTC' AND \"MD_ID\"=:Conditionoftc", NpgsqlCommand);
                        }
                        else
                        {
                            qry = "Data Not Available";
                        }

                        objest.sConditionoftc = qry;
                        //if (objest.sEnhancedCapacity == null || objest.sEnhancedCapacity == "")
                        //{
                        //    objest.sEnhancedCapacity = objest.sDtcCapacity;
                        //}

                        GetLastRepairedDetails(objest);
                        GetDTRCommissionDate(objest);
                        GetEnumerationDate(objest);

                    }

                    return objest;
                }


            }


            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objest;
            }

        }

        public clsPermanentEstimation GetLastRepairedDetails(clsPermanentEstimation objestDetails)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();
                string val = string.Empty;

      
                NpgsqlCommand cmd = new NpgsqlCommand("sp_lastrepairdetails");
                cmd.Parameters.AddWithValue("sdtctccode", Convert.ToString(objestDetails.sDtcTcCode));
                dt = objcon.FetchDataTable(cmd);


                //dt = objcon.getDataTable(strQry);
                if (dt.Rows.Count > 0)
                {
                    objestDetails.sLastRepairedDate = Convert.ToString(dt.Rows[0]["RSD_DELIVARY_DATE"]);
                    objestDetails.sLastRepairedBy = Convert.ToString(dt.Rows[0]["SUP_REPNAME"]);
                }

                if (objestDetails.sDtcId != "0")
                {
                    NpgsqlCommand.Parameters.AddWithValue("DtcTcCode", Convert.ToString(objestDetails.sDtcTcCode));
                    strQry = "SELECT CAST(\"TC_WARANTY_PERIOD\" AS TEXT) FROM \"TBLTCMASTER\" WHERE \"TC_CODE\"=:DtcTcCode";
                    val = objcon.get_value(strQry, NpgsqlCommand);

                    if (val != "")
                    {
                        NpgsqlCommand.Parameters.AddWithValue("DtcTcCode1",Convert.ToDouble( objestDetails.sDtcTcCode));
                        strQry = "SELECT CASE WHEN NOW() < \"TC_WARANTY_PERIOD\" THEN \"RSD_GUARRENTY_TYPE\" ELSE 'AGP' END FROM \"TBLTCMASTER\",\"TBLREPAIRSENTDETAILS\" WHERE ";
                        strQry += " \"TC_CODE\"=\"RSD_TC_CODE\" AND \"TC_CODE\"=:DtcTcCode1 AND \"RSD_GUARRENTY_TYPE\" IS NOT NULL AND \"RSD_WARENTY_PERIOD\" IS NOT NULL";
                        objestDetails.sGuarantyType = objcon.get_value(strQry, NpgsqlCommand);

                        if (objestDetails.sGuarantyType == "" || objestDetails.sGuarantyType == null)
                        {
                            NpgsqlCommand.Parameters.AddWithValue("DtcTcCode2",Convert.ToDouble(objestDetails.sDtcTcCode));
                            strQry = "SELECT CASE WHEN NOW() < \"TC_WARANTY_PERIOD\" THEN 'WGP' ELSE 'AGP' END FROM \"TBLTCMASTER\" WHERE \"TC_CODE\"=:DtcTcCode2";
                            objestDetails.sGuarantyType = objcon.get_value(strQry, NpgsqlCommand);
                        }
                    }
                    if (objestDetails.sGuarantyType == "" || objestDetails.sGuarantyType == null)
                    {
                        objestDetails.sGuarantyType = sFirstGuarantyType;
                    }
                }
            
           

                return objestDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objestDetails;
            }
        }

        public clsPermanentEstimation GetDTRCommissionDate(clsPermanentEstimation objest)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                string strQry = string.Empty;
                NpgsqlCommand.Parameters.AddWithValue("DtcTcCode",Convert.ToDouble(objest.sDtcTcCode));
                NpgsqlCommand.Parameters.AddWithValue("DtcCode", objest.sDtcCode);
                strQry = "SELECT TO_CHAR(\"TM_MAPPING_DATE\",'DD/MM/YYYY') TM_MAPPING_DATE FROM \"TBLTRANSDTCMAPPING\" WHERE \"TM_TC_ID\"=:DtcTcCode AND \"TM_DTC_ID\"=:DtcCode AND \"TM_LIVE_FLAG\"='1'";
                string sResult = objcon.get_value(strQry, NpgsqlCommand);
                objest.sDTrCommissionDate = sResult;
                return objest;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objest;
            }
        }

        public clsPermanentEstimation GetEnumerationDate(clsPermanentEstimation objest)
        {
            try
            {
                string strQry = string.Empty;
               
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getenumerationdate");
                cmd.Parameters.AddWithValue("sdtctccode", Convert.ToString(objest.sDtcTcCode));
                cmd.Parameters.AddWithValue("sdtccode", Convert.ToString(objest.sDtcCode));

                cmd.Parameters.Add("senumdate", NpgsqlDbType.Text);
                cmd.Parameters["senumdate"].Direction = ParameterDirection.Output;
                string sResult = objcon.StringGetValue(cmd);

                objest.sDTrEnumerationDate = sResult;
                return objest;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objest;
            }
        }

        public DataTable getFailId(string sWo_id)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string StrQry = string.Empty;
            DataTable dtEstDetails = new DataTable();
            try
            {
                NpgsqlCommand.Parameters.AddWithValue("Woid",Convert.ToDouble(sWo_id));
                StrQry = "SELECT \"WO_DATA_ID\",\"WO_WFO_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE CAST(\"WO_RECORD_ID\" AS TEXT) LIKE '-%' AND \"WO_ID\"=:Woid";
                dtEstDetails = objcon.FetchDataTable(StrQry, NpgsqlCommand);
                return dtEstDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtEstDetails;
            }
        }

        public object GettcDetails(clsPermanentEstimation objest)
        {
            NpgsqlCommand = new NpgsqlCommand();

            DataTable dtDetails = new DataTable();
          

            try
            {
                if (objest.sFailureId != "0")
                {


                    NpgsqlCommand cmd = new NpgsqlCommand("sp_searchall_permanent_details");
                    cmd.Parameters.AddWithValue("sestid", Convert.ToString(objest.sFailureId));
                    dtDetails = objcon.FetchDataTable(cmd);


                    if (dtDetails.Rows.Count > 0)
                    {

                        objest.sDtcId = dtDetails.Rows[0]["DT_ID"].ToString();
                        objest.sDtcCode = dtDetails.Rows[0]["DT_CODE"].ToString();
                        objest.sDtcName = dtDetails.Rows[0]["DT_NAME"].ToString();
                        objest.sDtcServicedate = dtDetails.Rows[0]["DT_LAST_SERVICE_DATE"].ToString();
                        objest.sDtcLoadKw = dtDetails.Rows[0]["DT_TOTAL_CON_KW"].ToString();
                        objest.sDtcLoadHp = dtDetails.Rows[0]["DT_TOTAL_CON_HP"].ToString();
                        objest.sCommissionDate = dtDetails.Rows[0]["DT_TRANS_COMMISION_DATE"].ToString();
                        objest.sDtcCapacity = Convert.ToDouble(dtDetails.Rows[0]["TC_CAPACITY"]).ToString();
                        objest.sDtcLocation = dtDetails.Rows[0]["TC_LOCATION_ID"].ToString();
                        objest.sDtcTcSlno = dtDetails.Rows[0]["TC_SLNO"].ToString();
                        objest.sDtcTcMake = dtDetails.Rows[0]["TC_MAKE_ID"].ToString();
                        // objest.sDtcReadings = dtDetails.Rows[0]["DF_KWH_READING"].ToString();
                        objest.sDtcTcCode = dtDetails.Rows[0]["TC_CODE"].ToString();
                        // objest.sFailureId = dtDetails.Rows[0]["DF_ID"].ToString();
                        objest.sCrby = dtDetails.Rows[0]["US_FULL_NAME"].ToString();
                        objest.sOfficeCode = dtDetails.Rows[0]["TC_LOCATION_ID"].ToString();
                        objest.sManfDate = dtDetails.Rows[0]["TC_MANF_DATE"].ToString();
                        objest.sTCId = dtDetails.Rows[0]["TC_ID"].ToString();
            
                        GetLastRepairedDetails(objest);

                    }

                    return objest;
                }

                else
                {

                    NpgsqlCommand cmd = new NpgsqlCommand("sp_searchall_permanent_details");
                    cmd.Parameters.AddWithValue("sestid", Convert.ToString(objest.sFailureId));
                    dtDetails = objcon.FetchDataTable(cmd);

                    if (dtDetails.Rows.Count > 0)
                    {
                        objest.sDtcId = dtDetails.Rows[0]["DT_ID"].ToString();
                        objest.sDtcCode = dtDetails.Rows[0]["DT_CODE"].ToString();
                        objest.sDtcName = dtDetails.Rows[0]["DT_NAME"].ToString();
                        objest.sDtcServicedate = dtDetails.Rows[0]["DT_LAST_SERVICE_DATE"].ToString();
                        objest.sDtcLoadKw = dtDetails.Rows[0]["DT_TOTAL_CON_KW"].ToString();
                        objest.sDtcLoadHp = dtDetails.Rows[0]["DT_TOTAL_CON_HP"].ToString();
                        objest.sCommissionDate = dtDetails.Rows[0]["DT_TRANS_COMMISION_DATE"].ToString();
                        objest.sDtcCapacity = dtDetails.Rows[0]["TC_CAPACITY"].ToString();
                        objest.sDtcLocation = dtDetails.Rows[0]["TC_LOCATION_ID"].ToString();
                        objest.sDtcTcSlno = dtDetails.Rows[0]["TC_SLNO"].ToString();
                        objest.sDtcTcMake = dtDetails.Rows[0]["TC_MAKE_ID"].ToString();
                        objest.sDtcTcCode = dtDetails.Rows[0]["TC_CODE"].ToString();
                        objest.sManfDate = dtDetails.Rows[0]["TC_MANF_DATE"].ToString();
                        objest.sTCId = dtDetails.Rows[0]["TC_ID"].ToString();
                        objest.sRating = dtDetails.Rows[0]["MD_NAME"].ToString();

                        objest.sConditionoftc = dtDetails.Rows[0]["TC_CONDITION"].ToString();
                        string qry = string.Empty;
                        if (objest.sConditionoftc != "")
                        {
                            NpgsqlCommand.Parameters.AddWithValue("Conditionoftc",Convert.ToDouble(objest.sConditionoftc));
                            qry = objcon.get_value("SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='COTC' AND \"MD_ID\"=:Conditionoftc", NpgsqlCommand);
                        }
                        else
                        {
                            qry = "Data Not Available";
                        }

                        objest.sConditionoftc = qry;
                      

                    }

                    return objest;
                }


            }


            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objest;
            }

        }

        public DataTable gettcdetails(string sDtrcode)
        {
            NpgsqlCommand = new NpgsqlCommand();
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand.Parameters.AddWithValue("Dtrcode", Convert.ToInt32(sDtrcode));
                dt = objcon.FetchDataTable("SELECT \"OM_NAME\",\"TM_NAME\",\"TC_CODE\" as \"DTR_CODE\" from \"TBLTCMASTER\" INNER JOIN \"TBLOMSECMAST\" ON \"TC_LOCATION_ID\" = \"OM_CODE\" INNER JOIN \"TBLTRANSMAKES\" ON \"TC_MAKE_ID\" =  \"TM_ID\"  WHERE \"TC_CODE\"=:Dtrcode", NpgsqlCommand);
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
