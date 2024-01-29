using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using IIITS.PGSQL.DAL;
using System.IO;
using Npgsql;


namespace IIITS.DTLMS.BL.Billing
{
   public class clsMajorBilling
    {
        public string bookamnt { get; set; }
        public string sRoleType { get; set; }
        public string sEstID { get; set; }
        public string sEstNo { get; set; }
        public string sEstDate { get; set; }
        public string sWoNo { get; set; }
        public string sWOId { get; set; }
        public string sWODate { get; set; }
        public string sEstAmount { get; set; }
        public string sWOAmount { get; set; }
        public string sEstFinalRate { get; set; }
        public string sEstFinalTax { get; set; }
        public string sEstFinalAmount { get; set; }
        public string sFailtype { get; set; }
        public string sGuaranteetype { get; set; }
        public string sInvID { get; set; }
        public string sInvNo { get; set; }
        public string sBRNo { get; set; }
        public string sInvDate { get; set; }
        public string sInvAmount { get; set; }
        public string sFilePath { get; set; }
        public string sActualFilePath { get; set; }
        public string sFileName { get; set; }

        public string sPaymentMode { get; set; }
        public string sRefNo { get; set; }
        public string sTransdate { get; set; }
        public string sBillNo { get; set; }

        public string sUserId { get; set; }
        public DataTable dtEstDetails { get; set; }
        public DataTable dtEstMatDetails { get; set; }
        public DataTable dtEstLabDetails { get; set; }
        public DataTable dtEstSalDetails { get; set; }
        public DataTable dtInvoiceDetails { get; set; }
        public string[] MaterialList { get; set; }
        public string[] LabourList { get; set; }
        public string[] SalavageList { get; set; }

        public string sItemId { get; set; }
        public string sItemName { get; set; }
        public string sItemQty { get; set; }
        public string sItemUnit { get; set; }
        public string sItemUnitName { get; set; }
        public string sItemRate { get; set; }
        public string sItemTax { get; set; }
        public string sItemAmount { get; set; }
        public string sItemTaxAmnt { get; set; }
        public string sItemTotal { get; set; }
        public string sOfficeCode { get; set; }
        public string sClientIp { get; set; }
        public string sActionType { get; set; }
        public string sWFDataId { get; set; }
        public string sWFAutoId { get; set; }
        public string sWFO_id { get; set; }

        public string strFormCode = "clsMajorBilling";
        NpgsqlCommand NpgsqlCommand;
        public object GetEstimationDetails(clsMajorBilling obj)
        {
            try
            {
                PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
                string sQry = string.Empty;
                sQry = "SELECT \"RESTD_ID\",\"RESTD_NO\", TO_CHAR(\"RESTD_DATE\",'DD/MM/YYYY') \"RESTD_DATE\", CAST(\"RESTD_ITEM_TOTAL\" AS TEXT) \"RESTD_ITEM_TOTAL\", \"RWO_NO\", \"RWO_SLNO\", ";
                sQry += " TO_CHAR(\"RWO_DATE\",'DD/MM/YYYY') \"RWO_DATE\", CAST(\"RWO_AMT\" AS TEXT) \"RWO_AMT\" , \"RESTD_FAIL_TYPE\", \"RESTD_GUARANTEETYPE\" FROM  \"TBLREPAIRERESTIMATIONDETAILS\", ";
                sQry += " \"TBLREPAIRERWORKORDER\" WHERE \"RWO_EST_ID\" = \"RESTD_ID\"  AND ";
                sQry += " \"RESTD_NO\" = :sEstNo";
                DataTable dt = new DataTable();
                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Parameters.AddWithValue("sEstNo", Convert.ToInt32(obj.sEstNo));

                dt = objcon.FetchDataTable(sQry, NpgsqlCommand);
                if (dt.Rows.Count > 0)
                {
                    obj.sEstID = Convert.ToString(dt.Rows[0]["RESTD_ID"]);
                    obj.sEstNo = Convert.ToString(dt.Rows[0]["RESTD_NO"]);
                    obj.sEstDate = Convert.ToString(dt.Rows[0]["RESTD_DATE"]);
                    obj.sEstAmount = Convert.ToString(dt.Rows[0]["RESTD_ITEM_TOTAL"]);
                    obj.sWoNo = Convert.ToString(dt.Rows[0]["RWO_NO"]);
                    obj.sWODate = Convert.ToString(dt.Rows[0]["RWO_DATE"]);
                    obj.sWOAmount = Convert.ToString(dt.Rows[0]["RWO_AMT"]);
                    obj.sFailtype = Convert.ToString(dt.Rows[0]["RESTD_FAIL_TYPE"]);
                    obj.sGuaranteetype = Convert.ToString(dt.Rows[0]["RESTD_GUARANTEETYPE"]);
                    obj.sWOId = Convert.ToString(dt.Rows[0]["RWO_SLNO"]);
                }
                else
                {
                    obj.sEstDate = "";
                    obj.sEstAmount = "";
                    obj.sWoNo = "";
                    obj.sWODate = "";
                    obj.sWOAmount = "";
                }

                sQry = "SELECT (CASE \"MRIM_ITEM_TYPE\" WHEN 1 THEN 'MATERIAL CHARGES' WHEN 2 THEN 'LABOUR CHARGES' WHEN 3 THEN 'SALVAGE CHARGES' ";
                sQry += " ELSE '' END ) \"TYPEOFCHARGES\", CAST(ROUND(sum(\"RESTM_ITEM_TOTAL\"),2) AS TEXT)  \"AMOUNT\" FROM \"TBLREPAIRERESTIMATIONMATERIAL\", ";
                sQry += " \"TBLMINORREPAIRERITEMMASTER\" WHERE \"RESTM_ITEM_ID\" = \"MRIM_ID\" AND \"RESTM_EST_ID\" =:sEstID GROUP BY ";
                sQry += " \"MRIM_ITEM_TYPE\" ORDER BY \"MRIM_ITEM_TYPE\"";
                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Parameters.AddWithValue("sEstID", Convert.ToInt32(obj.sEstID));
                dt = objcon.FetchDataTable(sQry, NpgsqlCommand);
                obj.dtEstDetails = dt;

                string officecode = objcon.get_value("SELECT \"RESTD_OFF_CODE\" FROM \"TBLREPAIRERESTIMATIONDETAILS\" WHERE \"RESTD_ID\"=" + Convert.ToInt32(obj.sEstID) + "");

                obj.sOfficeCode = officecode;
                obj.sUserId = obj.sUserId;

                GetMaturialdetails(obj);

                return obj;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return obj;
            }
        }

        public string[] SaveInvoiceDetails(clsMajorBilling obj)
        {
            string[] Arr = new string[3];
            string sQry = string.Empty;
            PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
            try
            {
                sQry = "SELECT \"MJB_ID\" FROM \"TBLMAJORBILLING\" WHERE \"MJB_INV_NO\" =:sInvNo";
                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Parameters.AddWithValue("sInvNo", obj.sInvNo);
                string sValue = objcon.get_value(sQry, NpgsqlCommand);
                if (sValue.Length > 0)
                {
                    Arr[0] = "Entered Invoice Number Already Exists";
                    Arr[1] = "2";
                    return Arr;
                }

                if (obj.sFilePath.Length > 0)
                {
                    SaveDocumments(obj);
                }


                sQry = "INSERT INTO \"TBLMAJORBILLING\" (\"MJB_ID\", \"MJB_EST_ID\", \"MJB_INV_NO\",\"MJB_INV_DATE\", \"MJB_INV_AMOUNT\", \"MJB_CRBY\",\"MJB_INV_FILEPATH\",\"MJB_BR_NO\",\"MJB_BOOK_AMNT\")";

                sQry += " VALUES('{0}','" + obj.sEstID + "','" + obj.sInvNo + "',TO_DATE('" + obj.sInvDate + "','DD/MM/YYYY'),'" + obj.sInvAmount + "','" + obj.sUserId + "','" + obj.sActualFilePath + "','" + obj.sBRNo + "','" + obj.bookamnt + "' )";

                sQry = sQry.Replace("'", "''");


                string[] sMaterialVal = obj.MaterialList.ToArray();
                string[] sLabourVal = obj.LabourList.ToArray();
                string[] sSalvageVal = obj.SalavageList.ToArray();

                StringBuilder sbQuery = new StringBuilder();

                string sMatID = string.Empty;
                string sMatName = string.Empty;
                string sMatQnty = string.Empty;
                string sMatUnit = string.Empty;
                string sMatUnitName = string.Empty;
                string sMatRate = string.Empty;
                string sMatTaxRate = string.Empty;
                string sMatAmount = string.Empty;
                string sMatTax = string.Empty;
                string sMatTotal = string.Empty;

                String sQry1 = string.Empty;
                for (int i = 0; i < sMaterialVal.Length; i++)
                {
                    if (sMaterialVal[i] != null)
                    {
                        if (sMaterialVal[i].Substring(0, 1) != "~")
                        {

                            sQry1 = "INSERT INTO \"TBLMAJORBILLINGDETAILS\" (\"MJBD_MB_ID\", \"MJBD_ITEM_ID\", \"MJBD_ITEM_QNTY\", \"MJBD_ITEM_RATE\", \"MJBD_ITEM_TAX\", \"MJBD_ITEM_TOTAL\")";
                            sQry1 += " VALUES ('{0}' , '" + sMaterialVal[i].Split('~').GetValue(0).ToString() + "', '" + sMaterialVal[i].Split('~').GetValue(2).ToString() + "', ";
                            sQry1 += " '" + sMaterialVal[i].Split('~').GetValue(7).ToString() + "', '" + sMaterialVal[i].Split('~').GetValue(8).ToString() + "', ";
                            sQry1 += " '" + sMaterialVal[i].Split('~').GetValue(9).ToString() + "')";

                            sbQuery.Append(sQry1);
                            sbQuery.Append(";");

                            sMatID += sMaterialVal[i].Split('~').GetValue(0).ToString() + "`";
                            sMatName += sMaterialVal[i].Split('~').GetValue(1).ToString() + "`";
                            sMatQnty += sMaterialVal[i].Split('~').GetValue(2).ToString() + "`";
                            sMatUnit += sMaterialVal[i].Split('~').GetValue(3).ToString() + "`";
                            sMatUnitName += sMaterialVal[i].Split('~').GetValue(4).ToString() + "`";
                            sMatRate += sMaterialVal[i].Split('~').GetValue(5).ToString() + "`";
                            sMatTaxRate += sMaterialVal[i].Split('~').GetValue(6).ToString() + "`";
                            sMatAmount += sMaterialVal[i].Split('~').GetValue(7).ToString() + "`";
                            sMatTax += sMaterialVal[i].Split('~').GetValue(8).ToString() + "`";
                            sMatTotal += sMaterialVal[i].Split('~').GetValue(9).ToString() + "`";

                        }

                    }
                }


                string sLabID = string.Empty;
                string sLabName = string.Empty;
                string sLabQnty = string.Empty;
                string sLabUnit = string.Empty;
                string sLabUnitName = string.Empty;
                string sLabRate = string.Empty;
                string sLabTaxRate = string.Empty;
                string sLabAmount = string.Empty;
                string sLabTax = string.Empty;
                string sLabTotal = string.Empty;


                for (int i = 0; i < sLabourVal.Length; i++)
                {
                    if (sLabourVal[i] != null)
                    {
                        if (sLabourVal[i].Substring(0, 1) != "~")
                        {

                            sQry1 = "INSERT INTO \"TBLMAJORBILLINGDETAILS\" ( \"MJBD_MB_ID\", \"MJBD_ITEM_ID\", \"MJBD_ITEM_QNTY\", \"MJBD_ITEM_RATE\", \"MJBD_ITEM_TAX\", \"MJBD_ITEM_TOTAL\")";
                            sQry1 += " VALUES ( '{0}' , '" + sLabourVal[i].Split('~').GetValue(0).ToString() + "', '" + sLabourVal[i].Split('~').GetValue(2).ToString() + "', ";
                            sQry1 += " '" + sLabourVal[i].Split('~').GetValue(7).ToString() + "', '" + sLabourVal[i].Split('~').GetValue(8).ToString() + "', ";
                            sQry1 += " '" + sLabourVal[i].Split('~').GetValue(9).ToString() + "')";

                            sbQuery.Append(sQry1);
                            sbQuery.Append(";");

                            sLabID += sLabourVal[i].Split('~').GetValue(0).ToString() + "`";
                            sLabName += sLabourVal[i].Split('~').GetValue(1).ToString() + "`";
                            sLabQnty += sLabourVal[i].Split('~').GetValue(2).ToString() + "`";
                            sLabUnit += sLabourVal[i].Split('~').GetValue(3).ToString() + "`";
                            sLabUnitName += sLabourVal[i].Split('~').GetValue(4).ToString() + "`";
                            sLabRate += sLabourVal[i].Split('~').GetValue(5).ToString() + "`";
                            sLabTaxRate += sLabourVal[i].Split('~').GetValue(6).ToString() + "`";
                            sLabAmount += sLabourVal[i].Split('~').GetValue(7).ToString() + "`";
                            sLabTax += sLabourVal[i].Split('~').GetValue(8).ToString() + "`";
                            sLabTotal += sLabourVal[i].Split('~').GetValue(9).ToString() + "`";

                        }

                    }
                }


                string sSalID = string.Empty;
                string sSalName = string.Empty;
                string sSalQnty = string.Empty;
                string sSalUnit = string.Empty;
                string sSalUnitName = string.Empty;
                string sSalRate = string.Empty;
                string sSalTaxRate = string.Empty;
                string sSalAmount = string.Empty;
                string sSalTax = string.Empty;
                string sSalTotal = string.Empty;


                for (int i = 0; i < sSalvageVal.Length; i++)
                {
                    if (sSalvageVal[i] != null)
                    {
                        if (sSalvageVal[i].Substring(0, 1) != "~")
                        {

                            sQry1 = "INSERT INTO \"TBLMAJORBILLINGDETAILS\" (\"MJBD_MB_ID\", \"MJBD_ITEM_ID\", \"MJBD_ITEM_QNTY\", \"MJBD_ITEM_RATE\", \"MJBD_ITEM_TAX\", \"MJBD_ITEM_TOTAL\")";
                            sQry1 += " VALUES ( '{0}' , '" + sSalvageVal[i].Split('~').GetValue(0).ToString() + "', '" + sSalvageVal[i].Split('~').GetValue(2).ToString() + "', ";
                            sQry1 += " '" + sSalvageVal[i].Split('~').GetValue(7).ToString() + "', '" + sSalvageVal[i].Split('~').GetValue(8).ToString() + "', ";
                            sQry1 += " '" + sSalvageVal[i].Split('~').GetValue(9).ToString() + "')";

                            sbQuery.Append(sQry1);
                            sbQuery.Append(";");

                            sSalID += sSalvageVal[i].Split('~').GetValue(0).ToString() + "`";
                            sSalName += sSalvageVal[i].Split('~').GetValue(1).ToString() + "`";
                            sSalQnty += sSalvageVal[i].Split('~').GetValue(2).ToString() + "`";
                            sSalUnit += sSalvageVal[i].Split('~').GetValue(3).ToString() + "`";
                            sSalUnitName += sSalvageVal[i].Split('~').GetValue(4).ToString() + "`";
                            sSalRate += sSalvageVal[i].Split('~').GetValue(5).ToString() + "`";
                            sSalTaxRate += sSalvageVal[i].Split('~').GetValue(6).ToString() + "`";
                            sSalAmount += sSalvageVal[i].Split('~').GetValue(7).ToString() + "`";
                            sSalTax += sSalvageVal[i].Split('~').GetValue(8).ToString() + "`";
                            sSalTotal += sSalvageVal[i].Split('~').GetValue(9).ToString() + "`";

                        }

                    }
                }

                sbQuery = sbQuery.Replace("'", "''");
                string sParam = "SELECT COALESCE(MAX(\"MJB_ID\"),0)+1 FROM \"TBLMAJORBILLING\"";

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "MajorDTRBilling";
                objApproval.sOfficeCode = obj.sOfficeCode;
                objApproval.sClientIp = obj.sClientIp;
                objApproval.sCrby = obj.sUserId;
                objApproval.sQryValues = sQry + ";" + sbQuery;
                objApproval.sParameterValues = sParam;
                objApproval.sMainTable = "TBLMAJORBILLING";
                objApproval.sDataReferenceId = obj.sEstNo;
                objApproval.sDescription = "Invoice Against for the Estimation No: " + obj.sEstNo;
                objApproval.sRefOfficeCode = obj.sEstNo.Substring(0, 5);
                string sPrimaryKey = "{0}";

                objApproval.sColumnNames = "MJB_ID,MJB_EST_ID,MJB_INV_NO,MJB_INV_DATE,MJB_INV_AMOUNT,MJB_INV_FILEPATH,RESTD_NO,MJB_BR_NO,MJB_BOOK_AMNT";
                objApproval.sColumnNames += ";MRIM_ID,MRIM_ITEM_NAME,RESTM_ITEM_QNTY,MD_NAME,MRI_MEASUREMENT,MRI_BASE_RATE,MRI_TAX,AMOUNT,TAX,MRI_TOTAL";
                objApproval.sColumnNames += ";MRIM_ID,MRIM_ITEM_NAME,RESTM_ITEM_QNTY,MD_NAME,MRI_MEASUREMENT,MRI_BASE_RATE,MRI_TAX,AMOUNT,TAX,MRI_TOTAL";
                objApproval.sColumnNames += ";MRIM_ID,MRIM_ITEM_NAME,RESTM_ITEM_QNTY,MD_NAME,MRI_MEASUREMENT,MRI_BASE_RATE,MRI_TAX,AMOUNT,TAX,MRI_TOTAL";
                objApproval.sColumnValues = "" + sPrimaryKey + "," + obj.sEstID + "," + obj.sInvNo + "," + obj.sInvDate + "," + obj.sInvAmount + "," + obj.sActualFilePath + "," + obj.sEstNo + "," + obj.sBRNo + "," + obj.bookamnt + "";
                objApproval.sColumnValues += "; " + sMatID + "," + sMatName + "," + sMatQnty + "," + sMatUnit + "," + sMatUnitName + "," + sMatRate + "," + sMatTaxRate + "," + sMatAmount + "," + sMatTax + "," + sMatTotal + "";
                objApproval.sColumnValues += "; " + sLabID + "," + sLabName + "," + sLabQnty + "," + sLabUnit + "," + sLabUnitName + "," + sLabRate + "," + sLabTaxRate + "," + sLabAmount + "," + sLabTax + "," + sLabTotal + "";
                objApproval.sColumnValues += "; " + sSalID + "," + sSalName + "," + sSalQnty + "," + sSalUnit + "," + sSalUnitName + "," + sSalRate + "," + sSalTaxRate + "," + sSalAmount + "," + sSalTax + "," + sSalTotal + "";
                objApproval.sTableNames = "TBLMAJORBILLING;TBLMAJORBILLINGMATDETAILS;TBLMAJORBILLINGLABDETAILS;TBLMAJORBILLINGSALDETAILS";




                //Check for Duplicate Approval
                bool bApproveResult = objApproval.CheckDuplicateApprove(objApproval);
                if (bApproveResult == false)
                {
                    Arr[0] = "Selected Record Already Approved";
                    Arr[1] = "2";
                    return Arr;
                }

                if (obj.sActionType == "M")
                {
                    objApproval.SaveWorkFlowData(objApproval);
                    obj.sWFDataId = objApproval.sWFDataId;
                }
                else
                {
                    objApproval.SaveWorkFlowData(objApproval);
                    objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow();
                    objApproval.SaveWorkflowObjects(objApproval);
                }

                SaveDocumments(obj);

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


        public void SaveDocumments(clsMajorBilling obj)
        {
            try
            {
                //FTP Parameter
                string sFTPLink = string.Empty;
                string sFTPUserName = string.Empty;
                string sFTPPassword = string.Empty;
                string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
                string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);

                clsCommon objComm = new clsCommon();
                DataTable dt = objComm.GetAppSettings();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPMAINLINK")
                    {
                        sFTPLink = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
                    }
                    else if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPUSERNAME")
                    {
                        sFTPUserName = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
                    }
                    else if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPPASSWORD")
                    {
                        sFTPPassword = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
                    }
                }

                //clsFtp objFtp = new clsFtp(sFTPLink, sFTPUserName, sFTPPassword);
                clsSFTP objFtp = new clsSFTP(SFTPPath, sFTPUserName, sFTPPassword);

                string sMainFolderName = "INVOICEDOCS";
                string sMajor = "MAJOR";
                bool IsFileExiest;
                if (File.Exists(obj.sFilePath))
                {
                    bool IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName + "/"  );
                    if (IsExists == false)
                    {

                        objFtp.createDirectory(SFTPmainfolder + sMainFolderName + "/" );
                    }
                    IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName + "/" + sMajor + "/" );
                    if (IsExists == false)
                    {

                        objFtp.createDirectory(SFTPmainfolder + sMainFolderName + "/" + sMajor + "/" );
                    }
                    IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName + "/" + sMajor + "/" + obj.sInvNo + "/");
                    if (IsExists == false)
                    {

                        objFtp.createDirectory(SFTPmainfolder + sMainFolderName + "/" + sMajor + "/" + obj.sInvNo);
                    }
                    IsExists = objFtp.FtpDirectoryExists(SFTPmainfolder + sMainFolderName + "/" + sMajor + "/" + obj.sInvNo + "/" + obj.sEstID + "/");
                    if (IsExists == false)
                    {

                        objFtp.createDirectory(SFTPmainfolder + sMainFolderName + "/" + sMajor + "/" + obj.sInvNo + "/" + obj.sEstID + "/");
                    }
                    IsFileExiest = objFtp.IsfileExiest(SFTPmainfolder + sMainFolderName + "/" + sMajor + "/" + obj.sInvNo + "/" + obj.sEstID + "/");
                    if (IsFileExiest == false)
                    {
                        bool Isuploaded = objFtp.upload(SFTPmainfolder + sMainFolderName + "/" + sMajor + "/" + obj.sInvNo + "/" + obj.sEstID + "/", obj.sFileName, obj.sFilePath);

                        if (Isuploaded == true & File.Exists(obj.sFilePath))
                        {
                            File.Delete(obj.sFilePath);
                            obj.sActualFilePath = SFTPmainfolder + sMainFolderName + "/" + sMajor + "/" + obj.sInvNo + "/" + obj.sEstID +  "/" + obj.sFileName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void GetMaturialdetails(clsMajorBilling obj)
        {
            PGSqlConnection objcon = new PGSqlConnection(Constants.Password);

            try
            {
                string sQry = string.Empty;
              
               // string officecode = objcon.get_value("SELECT SUBSTRING(\"US_OFFICE_CODE\", 1, 3)\"US_OFFICE_CODE\" from \"TBLUSER\" WHERE \"US_ID\" = '" + obj.sUserId + "'");
                string div_id = objcon.get_value("SELECT \"DIV_ID\" from \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT) like '" + obj.sOfficeCode.Substring(0,3) + "%'");

                sQry = "SELECT  \"MRIM_ID\", \"MRIM_ITEM_NAME\" \"MRIM_ITEM_NAME\" , \"RESTM_ITEM_QNTY\" \"RESTM_ITEM_QNTY\",   ";
                sQry += " COALESCE(\"RESTM_ITEM_RATE\",\"MRI_BASE_RATE\") \"MRI_BASE_RATE\",  \"MRI_TAX\", \"RESTM_ITEM_TOTAL\" \"MRI_TOTAL\",  \"MRI_MEASUREMENT\" ";
                sQry += " \"MRI_MEASUREMENT\", B.\"MD_NAME\" \"MD_NAME\", \"MRI_BASE_RATE\" *   \"RESTM_ITEM_QNTY\" \"AMOUNT\" ,  \"RESTM_ITEM_TAX\" \"TAX\" ";
                sQry += " FROM \"TBLMINORREPAIRERITEMMASTER\" JOIN \"TBLMINORREPAIRITEMRATEMASTER\" ON ";
                sQry += " \"MRIM_ID\" = \"MRI_MRIM_ID\" LEFT JOIN(SELECT \"RESTM_ITEM_ID\",\"RESTM_ITEM_QNTY\",\"RESTM_ITEM_RATE\",\"RESTM_ITEM_TAX\", ";
                sQry += " \"RESTM_ITEM_TOTAL\" FROM  \"TBLREPAIRERESTIMATIONMATERIAL\"  WHERE  \"RESTM_EST_ID\" =:sEstID) X ON X.\"RESTM_ITEM_ID\" = \"MRIM_ID\" ";
                sQry += " JOIN (SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\"  WHERE \"MD_TYPE\" = 'C')Y ON Y.\"MD_ID\" = \"MRI_CAPACITY\" ";
                sQry += " JOIN \"TBLREPAIRERESTIMATIONDETAILS\" ON  CAST(\"RESTD_CAPACITY\" AS TEXT)  = CAST(Y.\"MD_NAME\" AS TEXT) AND \"RESTD_REPAIRER\" = \"MRI_TR_ID\" JOIN (SELECT \"MD_ID\", ";
                sQry += " \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'MSR')B ON CAST(\"MRI_MEASUREMENT\" AS INT)  = B.\"MD_ID\" WHERE ";
                sQry += " \"RESTD_ID\" =:sEstID1 AND \"MRIM_ITEM_TYPE\" = 1 AND \"RESTD_RATETYPE\"=\"MRI_RATETYPE\" AND \"MRI_STATUS_FLAG\" = 0  and \"MRI_DIV_ID\"=:div_id AND \"RESTD_CRON\" BETWEEN \"MRI_EFFECTIVE_FROM\" ";
                sQry += " AND \"MRI_EFFECTIVE_TO\" ORDER BY \"MRIM_ID\"";
                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Parameters.AddWithValue("sEstID", Convert.ToInt32(obj.sEstID));
                NpgsqlCommand.Parameters.AddWithValue("sEstID1", Convert.ToInt32(obj.sEstID));
                NpgsqlCommand.Parameters.AddWithValue("div_id", Convert.ToInt32(div_id));

                obj.dtEstMatDetails = objcon.FetchDataTable(sQry, NpgsqlCommand);


                sQry = "SELECT  \"MRIM_ID\", \"MRIM_ITEM_NAME\" \"MRIM_ITEM_NAME\" , \"RESTM_ITEM_QNTY\" \"RESTM_ITEM_QNTY\",   ";
                sQry += " COALESCE(\"RESTM_ITEM_RATE\",\"MRI_BASE_RATE\")\"MRI_BASE_RATE\", \"MRI_TAX\", \"RESTM_ITEM_TOTAL\" \"MRI_TOTAL\",  \"MRI_MEASUREMENT\" ";
                sQry += " \"MRI_MEASUREMENT\", B.\"MD_NAME\" \"MD_NAME\", \"MRI_BASE_RATE\" *   \"RESTM_ITEM_QNTY\" \"AMOUNT\" ,  \"RESTM_ITEM_TAX\" \"TAX\" ";
                sQry += " FROM \"TBLMINORREPAIRERITEMMASTER\" JOIN \"TBLMINORREPAIRITEMRATEMASTER\" ON ";
                sQry += " \"MRIM_ID\" = \"MRI_MRIM_ID\" LEFT JOIN(SELECT \"RESTM_ITEM_ID\",\"RESTM_ITEM_QNTY\",\"RESTM_ITEM_RATE\",\"RESTM_ITEM_TAX\", ";
                sQry += " \"RESTM_ITEM_TOTAL\" FROM  \"TBLREPAIRERESTIMATIONMATERIAL\"  WHERE  \"RESTM_EST_ID\" =:sEstID) X ON X.\"RESTM_ITEM_ID\" = \"MRIM_ID\" ";
                sQry += " JOIN (SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\"  WHERE \"MD_TYPE\" = 'C')Y ON Y.\"MD_ID\" = \"MRI_CAPACITY\" ";
                sQry += " JOIN \"TBLREPAIRERESTIMATIONDETAILS\" ON  CAST(\"RESTD_CAPACITY\" AS TEXT)  = CAST(Y.\"MD_NAME\" AS TEXT) AND \"RESTD_REPAIRER\" = \"MRI_TR_ID\" JOIN (SELECT \"MD_ID\", ";
                sQry += " \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'MSR')B ON CAST(\"MRI_MEASUREMENT\" AS INT)  = B.\"MD_ID\" WHERE ";
                sQry += " \"RESTD_ID\" =:sEstID1  AND \"RESTD_RATETYPE\"=\"MRI_RATETYPE\" AND \"MRIM_ITEM_TYPE\" = 2 AND \"MRI_STATUS_FLAG\" = 0 and \"MRI_DIV_ID\"=:div_id1 AND \"RESTD_CRON\" BETWEEN \"MRI_EFFECTIVE_FROM\" ";
                sQry += " AND \"MRI_EFFECTIVE_TO\" ORDER BY \"MRIM_ID\"";

                //sQry = " SELECT \"ESTM_ITEM_ID\" AS \"MRIM_ID\", \"MRIM_ITEM_NAME\" \"MRIM_ITEM_NAME\" , \"ESTM_ITEM_QNTY\" \"ESTM_ITEM_QNTY\", ";
                //sQry += " \"ESTM_ITEM_RATE\" \"MRI_BASE_RATE\", \"ESTM_ITEM_TAX\" \"MRI_TAX\", \"ESTM_ITEM_TOTAL\" \"MRI_TOTAL\",  \"MRI_MEASUREMENT\" ";
                //sQry += " \"MRI_MEASUREMENT\", B.\"MD_NAME\" \"MD_NAME\" FROM \"TBLESTIMATIONMATERIAL\", \"TBLMINORREPAIRERITEMMASTER\", ";
                //sQry += " \"TBLESTIMATIONDETAILS\", \"TBLMINORREPAIRITEMRATEMASTER\", (SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" ";
                //sQry += " WHERE \"MD_TYPE\" = 'C')A, (SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'MSR')B WHERE ";
                //sQry += " \"ESTM_ITEM_ID\" = \"MRIM_ID\"  AND \"EST_ID\" = \"ESTM_EST_ID\" AND \"MRIM_ID\" = \"MRI_MRIM_ID\" AND \"EST_CAPACITY\" = ";
                //sQry += " CAST(A.\"MD_NAME\" AS INT) AND A.\"MD_ID\" = \"MRI_CAPACITY\" AND \"ESTM_EST_ID\" =  " + obj.sEstID + " AND \"EST_WOUNDTYPE\" = \"MRI_WOUNDTYPE\" ";
                //sQry += " AND \"EST_REPAIRER\" = \"MRI_TR_ID\" AND CAST(\"MRI_MEASUREMENT\" AS INT)  = B.\"MD_ID\" AND  \"MRI_STATUS_FLAG\" = 0 ";
                //sQry += " AND \"EST_CRON\" BETWEEN \"MRI_EFFECTIVE_FROM\" AND \"MRI_EFFECTIVE_TO\"   AND \"MRIM_ITEM_TYPE\" = 2 ORDER BY \"MRIM_ITEM_ORDER\" ";
                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Parameters.AddWithValue("sEstID", Convert.ToInt32(obj.sEstID));
                NpgsqlCommand.Parameters.AddWithValue("sEstID1", Convert.ToInt32(obj.sEstID));
                NpgsqlCommand.Parameters.AddWithValue("div_id1", Convert.ToInt32(div_id));
                obj.dtEstLabDetails = objcon.FetchDataTable(sQry, NpgsqlCommand);

                //sQry = " SELECT \"ESTM_ITEM_ID\" AS \"MRIM_ID\", \"MRIM_ITEM_NAME\" \"MRIM_ITEM_NAME\" , \"ESTM_ITEM_QNTY\" \"ESTM_ITEM_QNTY\", ";
                //sQry += " \"ESTM_ITEM_RATE\" \"MRI_BASE_RATE\", \"ESTM_ITEM_TAX\" \"MRI_TAX\", \"ESTM_ITEM_TOTAL\" \"MRI_TOTAL\",  \"MRI_MEASUREMENT\" ";
                //sQry += " \"MRI_MEASUREMENT\", B.\"MD_NAME\" \"MD_NAME\" FROM \"TBLESTIMATIONMATERIAL\", \"TBLMINORREPAIRERITEMMASTER\", ";
                //sQry += " \"TBLESTIMATIONDETAILS\", \"TBLMINORREPAIRITEMRATEMASTER\", (SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" ";
                //sQry += " WHERE \"MD_TYPE\" = 'C')A, (SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'MSR')B WHERE ";
                //sQry += " \"ESTM_ITEM_ID\" = \"MRIM_ID\"  AND \"EST_ID\" = \"ESTM_EST_ID\" AND \"MRIM_ID\" = \"MRI_MRIM_ID\" AND \"EST_CAPACITY\" = ";
                //sQry += " CAST(A.\"MD_NAME\" AS INT) AND A.\"MD_ID\" = \"MRI_CAPACITY\" AND \"ESTM_EST_ID\" =  " + obj.sEstID + " AND \"EST_WOUNDTYPE\" = \"MRI_WOUNDTYPE\" ";
                //sQry += " AND \"EST_REPAIRER\" = \"MRI_TR_ID\" AND CAST(\"MRI_MEASUREMENT\" AS INT)  = B.\"MD_ID\" AND  \"MRI_STATUS_FLAG\" = 0 ";
                //sQry += " AND \"EST_CRON\" BETWEEN \"MRI_EFFECTIVE_FROM\" AND \"MRI_EFFECTIVE_TO\"   AND \"MRIM_ITEM_TYPE\" = 3 ORDER BY \"MRIM_ITEM_ORDER\" ";

                sQry = "SELECT  \"MRIM_ID\", \"MRIM_ITEM_NAME\" \"MRIM_ITEM_NAME\" , \"RESTM_ITEM_QNTY\" \"RESTM_ITEM_QNTY\",   ";
                sQry += " COALESCE(\"RESTM_ITEM_RATE\",\"MRI_BASE_RATE\") \"MRI_BASE_RATE\",  \"MRI_TAX\", \"RESTM_ITEM_TOTAL\" \"MRI_TOTAL\",  \"MRI_MEASUREMENT\" ";
                sQry += " \"MRI_MEASUREMENT\", B.\"MD_NAME\" \"MD_NAME\", \"MRI_BASE_RATE\" *   \"RESTM_ITEM_QNTY\" \"AMOUNT\" ,  \"RESTM_ITEM_TAX\" \"TAX\" ";
                sQry += " FROM \"TBLMINORREPAIRERITEMMASTER\" JOIN \"TBLMINORREPAIRITEMRATEMASTER\" ON ";
                sQry += " \"MRIM_ID\" = \"MRI_MRIM_ID\" LEFT JOIN(SELECT \"RESTM_ITEM_ID\",\"RESTM_ITEM_QNTY\",\"RESTM_ITEM_RATE\",\"RESTM_ITEM_TAX\", ";
                sQry += " \"RESTM_ITEM_TOTAL\" FROM  \"TBLREPAIRERESTIMATIONMATERIAL\"  WHERE  \"RESTM_EST_ID\" =:sEstID) X ON X.\"RESTM_ITEM_ID\" = \"MRIM_ID\" ";
                sQry += " JOIN (SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\"  WHERE \"MD_TYPE\" = 'C')Y ON Y.\"MD_ID\" = \"MRI_CAPACITY\" ";
                sQry += " JOIN \"TBLREPAIRERESTIMATIONDETAILS\" ON  CAST(\"RESTD_CAPACITY\" AS TEXT)  = CAST(Y.\"MD_NAME\" AS TEXT) AND \"RESTD_REPAIRER\" = \"MRI_TR_ID\" JOIN (SELECT \"MD_ID\", ";
                sQry += " \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'MSR')B ON CAST(\"MRI_MEASUREMENT\" AS INT)  = B.\"MD_ID\" WHERE ";
                sQry += " \"RESTD_ID\" =:sEstID1  AND \"RESTD_RATETYPE\"=\"MRI_RATETYPE\" AND \"MRIM_ITEM_TYPE\" = 3 AND \"MRI_STATUS_FLAG\" = 0 and \"MRI_DIV_ID\"=:div_id2  AND \"RESTD_CRON\" BETWEEN \"MRI_EFFECTIVE_FROM\" ";
                sQry += " AND \"MRI_EFFECTIVE_TO\" ORDER BY \"MRIM_ID\"";
                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Parameters.AddWithValue("sEstID", Convert.ToInt32(obj.sEstID));
                NpgsqlCommand.Parameters.AddWithValue("sEstID1", Convert.ToInt32(obj.sEstID));
                NpgsqlCommand.Parameters.AddWithValue("div_id2", Convert.ToInt32(div_id));
                obj.dtEstSalDetails = objcon.FetchDataTable(sQry, NpgsqlCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetEstimateDetailsFromXML(clsMajorBilling obj)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable dt = new DataTable();

                DataSet ds = new DataSet();

                ds = objApproval.GetDatatableFromMultipleXML(obj.sWFDataId);

                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    if (ds.Tables[i].Rows.Count > 0)
                    {
                        dt = ds.Tables[i];
                        if (i == 0)
                        {
                            obj.sEstID = Convert.ToString(dt.Rows[0]["MJB_EST_ID"]);
                            obj.sEstNo = Convert.ToString(dt.Rows[0]["RESTD_NO"]);
                            obj.sInvNo = Convert.ToString(dt.Rows[0]["MJB_INV_NO"]);
                            obj.sInvDate = Convert.ToString(dt.Rows[0]["MJB_INV_DATE"]);
                            obj.sInvAmount = Convert.ToString(dt.Rows[0]["MJB_INV_AMOUNT"]);
                            obj.sFilePath = Convert.ToString(dt.Rows[0]["MJB_INV_FILEPATH"]);
                            obj.sBRNo = Convert.ToString(dt.Rows[0]["MJB_BR_NO"]);
                            if (dt.Columns.Contains("MJB_BOOK_AMNT"))
                            {
                                obj.bookamnt = Convert.ToString(dt.Rows[0]["MJB_BOOK_AMNT"]);
                            }
                        }
                        else if (i == 1)
                        {
                            obj.sItemId = Convert.ToString(dt.Rows[0]["MRIM_ID"]);
                            obj.sItemName = Convert.ToString(dt.Rows[0]["MRIM_ITEM_NAME"]).Replace("ç", ",");
                            obj.sItemQty = Convert.ToString(dt.Rows[0]["RESTM_ITEM_QNTY"]);
                            obj.sItemUnit = Convert.ToString(dt.Rows[0]["MD_NAME"]);
                            obj.sItemUnitName = Convert.ToString(dt.Rows[0]["MRI_MEASUREMENT"]);
                            obj.sItemRate = Convert.ToString(dt.Rows[0]["MRI_BASE_RATE"]);
                            obj.sItemTax = Convert.ToString(dt.Rows[0]["MRI_TAX"]);
                            obj.sItemAmount = Convert.ToString(dt.Rows[0]["AMOUNT"]);
                            obj.sItemTaxAmnt = Convert.ToString(dt.Rows[0]["TAX"]);
                            obj.sItemTotal = Convert.ToString(dt.Rows[0]["MRI_TOTAL"]);
                            obj.dtEstMatDetails = CreateDatatableFromString(obj);
                        }
                        else if (i == 2)
                        {
                            obj.sItemId = Convert.ToString(dt.Rows[0]["MRIM_ID"]);
                            obj.sItemName = Convert.ToString(dt.Rows[0]["MRIM_ITEM_NAME"]).Replace("ç", ",");
                            obj.sItemQty = Convert.ToString(dt.Rows[0]["RESTM_ITEM_QNTY"]);
                            obj.sItemUnit = Convert.ToString(dt.Rows[0]["MD_NAME"]);
                            obj.sItemUnitName = Convert.ToString(dt.Rows[0]["MRI_MEASUREMENT"]);
                            obj.sItemRate = Convert.ToString(dt.Rows[0]["MRI_BASE_RATE"]);
                            obj.sItemTax = Convert.ToString(dt.Rows[0]["MRI_TAX"]);
                            obj.sItemAmount = Convert.ToString(dt.Rows[0]["AMOUNT"]);
                            obj.sItemTaxAmnt = Convert.ToString(dt.Rows[0]["TAX"]);
                            obj.sItemTotal = Convert.ToString(dt.Rows[0]["MRI_TOTAL"]);
                            obj.dtEstLabDetails = CreateDatatableFromString(obj);
                        }
                        else if (i == 3)
                        {
                            obj.sItemId = Convert.ToString(dt.Rows[0]["MRIM_ID"]);
                            obj.sItemName = Convert.ToString(dt.Rows[0]["MRIM_ITEM_NAME"]).Replace("ç", ",");
                            obj.sItemQty = Convert.ToString(dt.Rows[0]["RESTM_ITEM_QNTY"]);
                            obj.sItemUnit = Convert.ToString(dt.Rows[0]["MD_NAME"]);
                            obj.sItemUnitName = Convert.ToString(dt.Rows[0]["MRI_MEASUREMENT"]);
                            obj.sItemRate = Convert.ToString(dt.Rows[0]["MRI_BASE_RATE"]);
                            obj.sItemTax = Convert.ToString(dt.Rows[0]["MRI_TAX"]);
                            obj.sItemAmount = Convert.ToString(dt.Rows[0]["AMOUNT"]);
                            obj.sItemTaxAmnt = Convert.ToString(dt.Rows[0]["TAX"]);
                            obj.sItemTotal = Convert.ToString(dt.Rows[0]["MRI_TOTAL"]);
                            obj.dtEstSalDetails = CreateDatatableFromString(obj);
                        }
                    }
                }

                PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
                string sQry = string.Empty;
                sQry = "SELECT \"RESTD_ID\",\"RESTD_NO\", TO_CHAR(\"RESTD_DATE\",'DD/MM/YYYY') \"RESTD_DATE\", CAST(\"RESTD_ITEM_TOTAL\" AS TEXT) \"RESTD_ITEM_TOTAL\", \"RWO_NO\", \"RWO_SLNO\", ";
                sQry += " TO_CHAR(\"RWO_DATE\",'DD/MM/YYYY') \"RWO_DATE\", CAST(\"RWO_AMT\" AS TEXT) \"RWO_AMT\" , \"RESTD_FAIL_TYPE\", \"RESTD_GUARANTEETYPE\"  FROM  \"TBLREPAIRERESTIMATIONDETAILS\", ";
                sQry += "  \"TBLREPAIRERWORKORDER\" WHERE \"RWO_EST_ID\" = \"RESTD_ID\"  AND ";
                sQry += " \"RESTD_NO\" =:sEstNo";
                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Parameters.AddWithValue("sEstNo", Convert.ToInt32(obj.sEstNo));
                //NpgsqlCommand.Parameters.AddWithValue("sEstID1", Convert.ToInt32(obj.sEstID));
                dt = objcon.FetchDataTable(sQry, NpgsqlCommand);
                if (dt.Rows.Count > 0)
                {
                    obj.sEstID = Convert.ToString(dt.Rows[0]["RESTD_ID"]);
                    obj.sEstNo = Convert.ToString(dt.Rows[0]["RESTD_NO"]);
                    obj.sEstDate = Convert.ToString(dt.Rows[0]["RESTD_DATE"]);
                    obj.sEstAmount = Convert.ToString(dt.Rows[0]["RESTD_ITEM_TOTAL"]);
                    obj.sWoNo = Convert.ToString(dt.Rows[0]["RWO_NO"]);
                    obj.sWODate = Convert.ToString(dt.Rows[0]["RWO_DATE"]);
                    obj.sWOAmount = Convert.ToString(dt.Rows[0]["RWO_AMT"]);
                    obj.sFailtype = Convert.ToString(dt.Rows[0]["RESTD_FAIL_TYPE"]);
                    obj.sGuaranteetype = Convert.ToString(dt.Rows[0]["RESTD_GUARANTEETYPE"]);
                    obj.sWOId = Convert.ToString(dt.Rows[0]["RWO_SLNO"]);
                }

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public DataTable CreateDatatableFromString(clsMajorBilling objEst)
        {

            DataTable dt = new DataTable();

            dt.Columns.Add("MRIM_ID");
            dt.Columns.Add("MRIM_ITEM_NAME");
            dt.Columns.Add("RESTM_ITEM_QNTY");
            dt.Columns.Add("MD_NAME");
            dt.Columns.Add("MRI_MEASUREMENT");
            dt.Columns.Add("MRI_BASE_RATE");
            dt.Columns.Add("MRI_TAX");
            dt.Columns.Add("AMOUNT");
            dt.Columns.Add("TAX");
            dt.Columns.Add("MRI_TOTAL");


            string[] sItemId = objEst.sItemId.Split('`');
            string[] sItemName = objEst.sItemName.Split('`');
            string[] sItemQty = objEst.sItemQty.Split('`');
            string[] sItemUnit = objEst.sItemUnit.Split('`');
            string[] sItemUnitName = objEst.sItemUnitName.Split('`');
            string[] sItemRate = objEst.sItemRate.Split('`');
            string[] sItemTax = objEst.sItemTax.Split('`');
            string[] sItemAmount = objEst.sItemAmount.Split('`');
            string[] sItemTaxAmnt = objEst.sItemTaxAmnt.Split('`');
            string[] sItemTotal = objEst.sItemTotal.Split('`');

            for (int i = 0; i < sItemId.Length; i++)
            {
                for (int j = 0; j < sItemName.Length; j++)
                {
                    for (int k = 0; k < sItemQty.Length; k++)
                    {
                        for (int l = 0; l < sItemUnit.Length; l++)
                        {
                            for (int m = 0; m < sItemUnitName.Length; m++)
                            {
                                for (int n = 0; n < sItemRate.Length; n++)
                                {
                                    for (int o = 0; o < sItemTax.Length; o++)
                                    {
                                        for (int p = 0; p < sItemAmount.Length; p++)
                                        {
                                            for (int q = 0; q < sItemTaxAmnt.Length; q++)
                                            {
                                                for (int r = 0; r < sItemTotal.Length; r++)
                                                {
                                                    if (sItemTotal[r] != "" && sItemTotal[r] != " ")
                                                    {
                                                        DataRow dRow = dt.NewRow();
                                                        dRow["MRIM_ID"] = sItemId[i];
                                                        dRow["MRIM_ITEM_NAME"] = sItemName[j];
                                                        dRow["RESTM_ITEM_QNTY"] = sItemQty[k];
                                                        dRow["MD_NAME"] = sItemUnit[l];
                                                        dRow["MRI_MEASUREMENT"] = sItemUnitName[m];
                                                        dRow["MRI_BASE_RATE"] = sItemRate[n];
                                                        dRow["MRI_TAX"] = sItemTax[o];
                                                        dRow["AMOUNT"] = sItemAmount[p];
                                                        dRow["TAX"] = sItemTaxAmnt[q];
                                                        dRow["MRI_TOTAL"] = sItemTotal[r];
                                                        dt.Rows.Add(dRow);
                                                        dt.AcceptChanges();
                                                    }
                                                    i++;
                                                    j++;
                                                    k++;
                                                    l++;
                                                    m++;
                                                    n++;
                                                    o++;
                                                    p++;
                                                    q++;
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

        public object GetInvoiceDetails(clsMajorBilling obj)
        {
            try
            {
                PGSqlConnection objCon = new PGSqlConnection(Constants.Password);
                string sQry = string.Empty;
                sQry = "SELECT \"MJB_INV_NO\",\"MJB_INV_AMOUNT\",TO_CHAR(\"MJB_INV_DATE\",'DD/MM/YYYY') \"MJB_INV_DATE\",\"MJB_ID\",\"MJB_BR_NO\" FROM \"TBLMAJORBILLING\" WHERE \"MJB_ID\" =:sInvID";
                DataTable dt = new DataTable();
                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Parameters.AddWithValue("sInvID", Convert.ToInt16(obj.sInvID));
                dt = objCon.FetchDataTable(sQry, NpgsqlCommand);
                if (dt.Rows.Count > 0)
                {
                    obj.sInvNo = Convert.ToString(dt.Rows[0]["MJB_INV_NO"]);
                    obj.sInvAmount = Convert.ToString(dt.Rows[0]["MJB_INV_AMOUNT"]);
                    obj.sInvDate = Convert.ToString(dt.Rows[0]["MJB_INV_DATE"]);
                    obj.sInvID = Convert.ToString(dt.Rows[0]["MJB_ID"]);
                    obj.sBRNo = Convert.ToString(dt.Rows[0]["MJB_BR_NO"]);
                }
                else
                {
                    obj.sInvNo = "";
                    obj.sInvAmount = "";
                    obj.sInvDate = "";
                }
                return obj;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return obj;
            }
        }

        public string[] SaveTransactionDetails(clsMajorBilling obj)
        {
            string[] Arr = new string[3];
            PGSqlConnection objCon = new PGSqlConnection(Constants.Password);
            try
            {
                string sQry = string.Empty;
                sQry = "UPDATE \"TBLMAJORBILLING\" SET \"MJB_PAYMENEMODE\" = '" + obj.sPaymentMode + "', \"MJB_REFNO\" = '" + obj.sRefNo + "', ";
                sQry += "\"MJB_TRANSDATE\" = TO_DATE('" + obj.sTransdate + "','DD/MM/YYYY'), \"MJB_BILLNO\" = '" + obj.sBillNo + "', \"MJB_UPDATEDON\" = NOW(), ";
                sQry += "\"MJB_UPDATEDBY\" = '" + obj.sUserId + "' WHERE \"MJB_ID\" = '" + obj.sInvID + "' ";

                sQry = sQry.Replace("'", "''");

                clsApproval objApproval = new clsApproval();
                //objApproval.sFormName = "ReceiveMinorTC";
                // objApproval.sRecordId = objFailureDetails.sFailureId;

                string sParam = "SELECT COALESCE(MAX(\"MJB_ID\"),0)+1 FROM \"TBLMAJORBILLING\" WHERE \"MJB_ID\" = '" + obj.sInvID + "'";
                sParam = sParam.Replace("'", "''");
                objApproval.sFormName = "MajorPaymentDetails";

                objApproval.sOfficeCode = obj.sOfficeCode;
                objApproval.sClientIp = obj.sClientIp;
                objApproval.sCrby = obj.sUserId;
                objApproval.sQryValues = sQry;
                objApproval.sParameterValues = sParam;
                objApproval.sMainTable = "TBLMAJORBILLING";
                objApproval.sDataReferenceId = obj.sInvID;
                objApproval.sDescription = "Major Payment Details Against for the Invoice Id: " + obj.sInvID;
                objApproval.sRefOfficeCode = obj.sOfficeCode;
                objApproval.sWFObjectId = obj.sWFO_id;

                string sPrimaryKey = "SELECT COALESCE(MAX(\"MJB_ID\"),0)+1 FROM \"TBLMAJORBILLING\" WHERE \"MJB_ID\" = '" + obj.sInvID + "'";

                objApproval.sColumnNames = "MJB_PAYMENEMODE,MJB_REFNO,MJB_TRANSDATE,MJB_BILLNO,MJB_UPDATEDBY";
                objApproval.sColumnValues = "" + obj.sPaymentMode + "," + obj.sRefNo + "," + obj.sTransdate + "," + obj.sBillNo + "," + obj.sUserId + "";
                objApproval.sTableNames = "TBLRECEIVEDTR";

                objApproval.SaveWorkFlowData(objApproval);
                objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow();
                objApproval.SaveWorkflowObjects(objApproval);


                Arr[0] = "0";
                Arr[1] = "Saved Successfully";
                return Arr;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                Arr[0] = "1";
                Arr[1] = "Failed to Save";
                return Arr;
            }
        }

        public void GetBillingDetails(clsMajorBilling obj)
        {
            try
            {
                PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
                string sQry = string.Empty;
                if (obj.sRoleType != "2")
                {
                    NpgsqlCommand = new NpgsqlCommand();
                    sQry = "SELECT \"RESTD_ID\", \"MJB_ID\", CAST(\"RWO_NO\" AS TEXT) \"RWO_NO\", CAST(\"RESTD_NO\" AS TEXT) \"RESTD_NO\", CAST(\"MJB_INV_NO\" AS TEXT) \"MJB_INV_NO\", \"MJB_INV_AMOUNT\" FROM \"TBLMAJORBILLING\", \"TBLREPAIRERWORKORDER\", ";
                    sQry += " \"TBLREPAIRERESTIMATIONDETAILS\"  WHERE \"MJB_EST_ID\" = \"RESTD_ID\"  ";
                    sQry += " AND \"RESTD_ID\" = \"RWO_EST_ID\" and cast(\"RESTD_OFF_CODE\" as text) like :sOfficeCode||'%' ";
                    NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", obj.sOfficeCode);
                    obj.dtInvoiceDetails = objcon.FetchDataTable(sQry, NpgsqlCommand);
                }
                else
                {
                    string sOffCode = clsStoreOffice.GetOfficeCode(obj.sOfficeCode, "RESTD_OFF_CODE");
                    NpgsqlCommand = new NpgsqlCommand();
                    sQry = "SELECT \"RESTD_ID\", \"MJB_ID\", CAST(\"RWO_NO\" AS TEXT) \"RWO_NO\", CAST(\"RESTD_NO\" AS TEXT) \"RESTD_NO\", CAST(\"MJB_INV_NO\" AS TEXT) \"MJB_INV_NO\", \"MJB_INV_AMOUNT\" FROM \"TBLMAJORBILLING\", \"TBLREPAIRERWORKORDER\", ";
                    sQry += " \"TBLREPAIRERESTIMATIONDETAILS\"  WHERE \"MJB_EST_ID\" = \"RESTD_ID\"  ";
                    sQry += " AND \"RESTD_ID\" = \"RWO_EST_ID\" and  "+ sOffCode + "";
                    //NpgsqlCommand.Parameters.AddWithValue("sOfficeCode", obj.sOfficeCode);
                    obj.dtInvoiceDetails = objcon.FetchDataTable(sQry);
                }

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetActualMaterialDetails(clsMajorBilling obj)
        {
            try
            {
                PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
                string sQry = string.Empty;
                string estid = objcon.get_value("SELECT \"MJB_EST_ID\" from \"TBLMAJORBILLING\" where \"MJB_ID\"='" + Convert.ToInt16(obj.sInvID) + "' ");
                string divcode = objcon.get_value("SELECT SUBSTR(\"RESTD_OFF_CODE\",1,3) from \"TBLREPAIRERESTIMATIONDETAILS\" WHERE \"RESTD_ID\" = '" + estid +"'");
                string divid = objcon.get_value("SELECT \"DIV_ID\" FROM \"TBLDIVISION\" WHERE \"DIV_CODE\"='"+ divcode + "'");
                sQry = "SELECT distinct \"MRIM_ID\", \"MRIM_ITEM_NAME\",  \"MJBD_ITEM_QNTY\", \"MJBD_ITEM_QNTY\" AS  \"RESTM_ITEM_QNTY\", (SELECT \"MD_NAME\" FROM ";
                sQry += " \"TBLMASTERDATA\"  WHERE \"MD_TYPE\" = 'MSR' AND CAST(\"MD_ID\" AS TEXT) =   \"MRI_MEASUREMENT\") \"MD_NAME\" , '' AS  \"MRI_MEASUREMENT\", ";
                sQry += " \"MRI_BASE_RATE\",  \"MRI_TAX\", \"MJBD_ITEM_RATE\" \"AMOUNT\", \"MJBD_ITEM_TAX\" \"TAX\", \"MJBD_ITEM_TOTAL\"  \"MRI_TOTAL\" FROM \"TBLMAJORBILLINGDETAILS\", ";
                sQry += " \"TBLMINORREPAIRERITEMMASTER\", \"TBLMINORREPAIRITEMRATEMASTER\", \"TBLREPAIRERESTIMATIONDETAILS\", \"TBLMAJORBILLING\", ";
                sQry += " \"TBLMASTERDATA\"  WHERE \"RESTD_REPAIRER\" = \"MRI_TR_ID\" and  \"MRIM_ID\" = \"MRI_MRIM_ID\" AND \"MJBD_ITEM_ID\" = \"MRIM_ID\" AND  \"MJBD_MB_ID\" =:sInvID AND ";
                sQry += " \"MJB_ID\" = \"MJBD_MB_ID\" AND \"MJB_EST_ID\" = \"RESTD_ID\"  AND \"MD_TYPE\" = 'C' AND CAST(\"RESTD_CAPACITY\" AS TEXT) = ";
                sQry += " \"MD_NAME\" AND \"MD_ID\" = \"MRI_CAPACITY\" AND \"RESTD_CRON\" BETWEEN \"MRI_EFFECTIVE_FROM\" AND \"MRI_EFFECTIVE_TO\" AND ";
                sQry += " \"MRIM_ITEM_TYPE\" = 1 and \"MRI_DIV_ID\"='"+ divid + "' ORDER BY \"MRIM_ID\"";
                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Parameters.AddWithValue("sInvID", Convert.ToInt16(obj.sInvID));
                obj.dtEstMatDetails = objcon.FetchDataTable(sQry, NpgsqlCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetActualLabourDetails(clsMajorBilling obj)
        {
            try
            {
                PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
                string sQry = string.Empty;
                string estid = objcon.get_value("SELECT \"MJB_EST_ID\" from \"TBLMAJORBILLING\" where \"MJB_ID\"='" + Convert.ToInt16(obj.sInvID) + "' ");
                string divcode = objcon.get_value("SELECT SUBSTR(\"RESTD_OFF_CODE\",1,3) from \"TBLREPAIRERESTIMATIONDETAILS\" WHERE \"RESTD_ID\" = '" + estid + "'");
                string divid = objcon.get_value("SELECT \"DIV_ID\" FROM \"TBLDIVISION\" WHERE \"DIV_CODE\"='" + divcode + "'");

                sQry = "SELECT distinct \"MRIM_ID\", \"MRIM_ITEM_NAME\",  \"MJBD_ITEM_QNTY\", \"MJBD_ITEM_QNTY\" AS  \"RESTM_ITEM_QNTY\", (SELECT \"MD_NAME\" FROM ";
                sQry += " \"TBLMASTERDATA\"  WHERE \"MD_TYPE\" = 'MSR' AND CAST(\"MD_ID\" AS TEXT) =   \"MRI_MEASUREMENT\") \"MD_NAME\" , '' AS  \"MRI_MEASUREMENT\", ";
                sQry += " \"MRI_BASE_RATE\",  \"MRI_TAX\", \"MJBD_ITEM_RATE\" \"AMOUNT\", \"MJBD_ITEM_TAX\" \"TAX\", \"MJBD_ITEM_TOTAL\"  \"MRI_TOTAL\" FROM \"TBLMAJORBILLINGDETAILS\", ";
                sQry += " \"TBLMINORREPAIRERITEMMASTER\", \"TBLMINORREPAIRITEMRATEMASTER\", \"TBLREPAIRERESTIMATIONDETAILS\", \"TBLMAJORBILLING\", ";
                sQry += " \"TBLMASTERDATA\"  WHERE  \"RESTD_REPAIRER\" = \"MRI_TR_ID\" and  \"MRIM_ID\" = \"MRI_MRIM_ID\" AND \"MJBD_ITEM_ID\" = \"MRIM_ID\" AND  \"MJBD_MB_ID\" =:sInvID AND ";
                sQry += " \"MJB_ID\" = \"MJBD_MB_ID\" AND \"MJB_EST_ID\" = \"RESTD_ID\"  AND \"MD_TYPE\" = 'C' AND CAST(\"RESTD_CAPACITY\" AS TEXT) = ";
                sQry += " \"MD_NAME\" AND \"MD_ID\" = \"MRI_CAPACITY\" AND \"RESTD_CRON\" BETWEEN \"MRI_EFFECTIVE_FROM\" AND \"MRI_EFFECTIVE_TO\" AND ";
                sQry += " \"MRIM_ITEM_TYPE\" = 2  and \"MRI_DIV_ID\"='" + divid + "' ORDER BY \"MRIM_ID\"";
                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Parameters.AddWithValue("sInvID", Convert.ToInt16(obj.sInvID));
                obj.dtEstLabDetails = objcon.FetchDataTable(sQry, NpgsqlCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetActualSalvageDetails(clsMajorBilling obj)
        {
            try
            {
                PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
                string sQry = string.Empty;

                string estid = objcon.get_value("SELECT \"MJB_EST_ID\" from \"TBLMAJORBILLING\" where \"MJB_ID\"='" + Convert.ToInt16(obj.sInvID) + "' ");
                string divcode = objcon.get_value("SELECT SUBSTR(\"RESTD_OFF_CODE\",1,3) from \"TBLREPAIRERESTIMATIONDETAILS\" WHERE \"RESTD_ID\" = '" + estid + "'");
                string divid = objcon.get_value("SELECT \"DIV_ID\" FROM \"TBLDIVISION\" WHERE \"DIV_CODE\"='" + divcode + "'");

                sQry = "SELECT distinct \"MRIM_ID\", \"MRIM_ITEM_NAME\",  \"MJBD_ITEM_QNTY\", \"MJBD_ITEM_QNTY\" AS  \"RESTM_ITEM_QNTY\", (SELECT \"MD_NAME\" FROM ";
                sQry += " \"TBLMASTERDATA\"  WHERE \"MD_TYPE\" = 'MSR' AND CAST(\"MD_ID\" AS TEXT) =   \"MRI_MEASUREMENT\") \"MD_NAME\" , '' AS  \"MRI_MEASUREMENT\" ,";
                sQry += " \"MRI_BASE_RATE\",  \"MRI_TAX\", \"MJBD_ITEM_RATE\" \"AMOUNT\", \"MJBD_ITEM_TAX\" \"TAX\", \"MJBD_ITEM_TOTAL\"  \"MRI_TOTAL\" FROM \"TBLMAJORBILLINGDETAILS\", ";
                sQry += " \"TBLMINORREPAIRERITEMMASTER\", \"TBLMINORREPAIRITEMRATEMASTER\", \"TBLREPAIRERESTIMATIONDETAILS\", \"TBLMAJORBILLING\", ";
                sQry += " \"TBLMASTERDATA\"  WHERE  \"RESTD_REPAIRER\" = \"MRI_TR_ID\" and \"MRIM_ID\" = \"MRI_MRIM_ID\" AND \"MJBD_ITEM_ID\" = \"MRIM_ID\" AND  \"MJBD_MB_ID\" =:sInvID AND ";
                sQry += " \"MJB_ID\" = \"MJBD_MB_ID\" AND \"MJB_EST_ID\" = \"RESTD_ID\"  AND \"MD_TYPE\" = 'C' AND CAST(\"RESTD_CAPACITY\" AS TEXT) = ";
                sQry += " \"MD_NAME\" AND \"MD_ID\" = \"MRI_CAPACITY\" AND \"RESTD_CRON\" BETWEEN \"MRI_EFFECTIVE_FROM\" AND \"MRI_EFFECTIVE_TO\" AND ";
                sQry += " \"MRIM_ITEM_TYPE\" = 3  and \"MRI_DIV_ID\"='" + divid + "' ORDER BY \"MRIM_ID\"";
                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Parameters.AddWithValue("sInvID", Convert.ToInt16(obj.sInvID));
                obj.dtEstSalDetails = objcon.FetchDataTable(sQry, NpgsqlCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetEstimationNo(clsMajorBilling obj)
        {
            try
            {
                PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
                string sQry = string.Empty;
                sQry = "SELECT \"RESTD_NO\" FROM \"TBLMAJORBILLING\", \"TBLREPAIRERESTIMATIONDETAILS\"  WHERE \"RESTD_ID\" = \"MJB_EST_ID\" AND  \"MJB_ID\" =:sInvID ";
                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Parameters.AddWithValue("sInvID", Convert.ToInt16(obj.sInvID));
                obj.sEstNo = objcon.get_value(sQry, NpgsqlCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

    }
}
