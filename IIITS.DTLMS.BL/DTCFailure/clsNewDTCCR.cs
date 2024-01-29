using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IIITS.PGSQL.DAL;
using System.Data;
using Npgsql;
using NpgsqlTypes;

namespace IIITS.DTLMS.BL
{
    public class clsNewDTCCR : clsCRReport
    {
        string strFormCode = "clsNewDTCCR";

        public string sStatus { get; set; }
        public string sTcCode { get; set; }
        public string sFeederCode { get; set; }
        public string sTCSlno { get; set; }
        public string sTCCapacity { get; set; }
        public string sTCManfDate { get; set; }
        public string sTCMake { get; set; }
        public string sRating { get; set; }
        public string sDTCCode { get; set; }
        public string sDTCName { get; set; }
        public string sStarRate { get; set; }
        public string sMakeName { get; set; }
        public string sWOSlno { get; set; }
        public string sCrBy { get; set; }
        public string sRefOfficeCode { get; set; }
        public string sEnumDetailsID { get; set; }
        public string sTransCommDate { get; set; }
        public string sRecordId { get; set; }
        public string sOldCodePhotoPath { get; set; }
        public string sNamePlatePhotoPath { get; set; }
        public string sSSPlatePhotoPath { get; set; }
        public string sDTCPhoto2Path { get; set; }
        public string sDTLMSCodePhotoPath { get; set; }
        public string sIPEnumCodePhotoPath { get; set; }
        public string sInfosysCodePhotoPath { get; set; }
        public string sDTCPhoto1Path { get; set; }
        public string sInfosysAsset { get; set; }
        public Int16 sLevel { get; set; }

        PGSqlConnection objcon = new PGSqlConnection(Constants.Password);

        int SubDiv_code = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["SubDiv_code"]);
        int Section_code = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["Section_code"]);
        public string[] SaveCompletionReport(clsNewDTCCR objCR)
        {
            string[] Arr = new string[2];
            string strQry = string.Empty;
            try
            {
                #region Workflow

                string strQry1 = "UPDATE \"TBLWORKORDER\" SET \"WO_REPLACE_FLG\" =1,\"WO_NEW_DTC_CR_DATE\"=TO_DATE('" + objCR.sCRDate + "','dd/MM/yyyy') WHERE \"WO_SLNO\" ='" + objCR.sWOSlno + "'; UPDATE \"TBLDTCMAST\" SET \"DT_NEWDTC_CR_FLAG\" =1 WHERE \"DT_CODE\" ='" + objCR.sDTCCode + "'";

                strQry1 = strQry1.Replace("'", "''");

                clsApproval objApproval = new clsApproval();
                objApproval.sFormName = objCR.sFormName;
                objApproval.sRecordId = objCR.sRecordId;
                objApproval.sOfficeCode = objCR.sOfficeCode;
                objApproval.sClientIp = objCR.sClientIP;
                objApproval.sCrby = objCR.sCrby;
                objApproval.sWFObjectId = objCR.sWFObjectId;
                objApproval.sWFAutoId = objCR.sWFAutoId;
                objApproval.sDataReferenceId = objCR.sRecordId;
                objApproval.sRefOfficeCode = objCR.sRefOfficeCode;
                objApproval.sQryValues = strQry1;

                objApproval.sDescription = "Completion Report For DTC Code " + objCR.sDTCCode;

                objApproval.sColumnNames = "CR_DATE,EP_NAMEPLATE_PATH,EP_SSPLATE_PATH,EP_DTC_PATH,EP_DTLMSDTC_PATH,EP_INFOSYSDTC_PATH,ED_IP_ENUM_DONE,EP_OLDDTC_PATH,ED_ID,ED_APPROVALPRIORITY";
                objApproval.sColumnValues = "" + objCR.sCRDate + "," + objCR.sNamePlatePhotoPath + "," + objCR.sSSPlatePhotoPath + "," + objCR.sDTCPhoto1Path + ",";
                objApproval.sColumnValues += "" + objCR.sDTLMSCodePhotoPath + "," + objCR.sInfosysCodePhotoPath + "," + objCR.sIPEnumCodePhotoPath + "," + objCR.sDTCPhoto2Path + "," + objCR.sEnumDetailsID + "," + objCR.sLevel + "";
                objApproval.sTableNames = "TBLTEMPTABLE";

                bool bResult = objApproval.CheckDuplicateApprove(objApproval);
                if (bResult == false)
                {
                    Arr[0] = "Selected Record Already Approved";
                    Arr[1] = "2";
                    return Arr;
                }

                //objApproval.SaveWorkflowObjects(objApproval);

                if (objCR.sActionType == "M")
                {
                    objApproval.SaveWorkFlowData(objApproval);
                    objCR.sWFDataId = objApproval.sWFDataId;
                }
                else
                {
                    objApproval.SaveWorkFlowData(objApproval);
                    objCR.sWFDataId = objApproval.sWFDataId;
                    //objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow();
                    objApproval.SaveWorkflowObjects(objApproval);
                }


                Arr[0] = "Approved Successfully";
                Arr[1] = "0";
                return Arr;
                #endregion
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }
        public bool SaveImagePathDetails(clsNewDTCCR objCR)
        {
            try
            {
                PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
                string strQry = string.Empty;

                string sMaxNo = Convert.ToString(ObjCon.Get_max_no("EP_ID", "TBLENUMERATIONPHOTOS"));

                strQry = "INSERT INTO \"TBLENUMERATIONPHOTOS\" (\"EP_ID\",\"EP_ED_ID\",\"EP_NAMEPLATE_PATH\",\"EP_SSPLATE_PATH\",\"EP_OLDDTC_PATH\",\"EP_DTLMSDTC_PATH\",";
                strQry += " \"EP_IPENUMDTC_PATH\",\"EP_INFOSYSDTC_PATH\",\"EP_DTC_PATH\",\"EP_CRBY\") VALUES ('" + sMaxNo + "','" + objCR.sEnumDetailsID + "','" + objCR.sNamePlatePhotoPath + "',";
                strQry += " '" + objCR.sSSPlatePhotoPath + "','" + objCR.sDTCPhoto2Path + "','" + objCR.sDTLMSCodePhotoPath + "',";
                strQry += " '" + objCR.sIPEnumCodePhotoPath + "','" + objCR.sInfosysCodePhotoPath + "','" + objCR.sDTCPhoto1Path + "','" + objCR.sCrby + "')";
                ObjCon.ExecuteQry(strQry);
                return true;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveImagePathDetails");
                return false;
            }
        }
        public string[] SaveFieldEnumerationDetails(clsNewDTCCR objCR)
        {
            string[] Arr = new string[3];
            string strQry = string.Empty;
            string strOfficeCode = string.Empty;
            string sRes = string.Empty;
            PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);

            try
            {
                objCR.sFeederCode = objCR.sDTCCode.Substring(0, 6);
                if (objCR.sRefOfficeCode.Length >= Section_code)
                {
                    strOfficeCode = objCR.sRefOfficeCode.Substring(0, SubDiv_code);
                }
                if (objCR.sEnumDetailsID == null)
                {
                    objCR.sEnumDetailsID = "";
                }

                try
                {
                    NpgsqlCommand cmd = new NpgsqlCommand("sp_savefieldenumerationdetails");
                    cmd.Parameters.AddWithValue("stccode", objCR.sTcCode);
                    cmd.Parameters.AddWithValue("strofficecode", objCR.sOfficeCode);
                    cmd.Parameters.AddWithValue("ssubdivcode", strOfficeCode);
                    cmd.Parameters.AddWithValue("senumdetailsid", objCR.sEnumDetailsID);
                    cmd.Parameters.AddWithValue("sdtccode", objCR.sDTCCode);
                    cmd.Parameters.AddWithValue("scommisiondate", objCR.sTransCommDate);

                    cmd.Parameters.AddWithValue("stcmake", objCR.sTCMake);
                    cmd.Parameters.AddWithValue("stccapacity", objCR.sTCCapacity);
                    cmd.Parameters.AddWithValue("smakename", objCR.sTCMake);
                    cmd.Parameters.AddWithValue("stcslno", objCR.sTcSlno);
                    cmd.Parameters.AddWithValue("sfeedercode", objCR.sFeederCode);
                    cmd.Parameters.AddWithValue("soperator1", objCR.sCrby);
                    cmd.Parameters.AddWithValue("scrby", objCR.sCrby);
                    cmd.Parameters.AddWithValue("sdtcname", objCR.sDTCName);
                    cmd.Parameters.AddWithValue("srating", objCR.sRating);
                    cmd.Parameters.AddWithValue("sstarrate", objCR.sRating);
                    cmd.Parameters.AddWithValue("slevel", objCR.sLevel);
                    cmd.Parameters.AddWithValue("sstatus", '0');



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
                        objCR.sEnumDetailsID = Arr[2].ToString();
                    }
                    return Arr;

                }
                catch (Exception ex)
                {

                    clsException.LogError(ex.StackTrace, ex.Message + strQry, strFormCode, "SaveFieldEnumerationDetails");
                    return Arr;
                }

              

            }
            catch (Exception ex)
            {
                ObjCon.RollBackTrans();
                clsException.LogError(ex.StackTrace, ex.Message + strQry, strFormCode, "SaveFieldEnumerationDetails");
                return Arr;
            }
        }


        public bool UpdateImagePathDetails(clsNewDTCCR objCR)
        {
            try
            {
                PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
                string strQry = string.Empty;

                strQry = "SELECT \"EP_ED_ID\" FROM \"TBLENUMERATIONPHOTOS\" WHERE \"EP_ED_ID\"='" + objCR.sEnumDetailsID + "'";
                string sResult = ObjCon.get_value(strQry);
                if (sResult != "")
                {
                    strQry = "UPDATE \"TBLENUMERATIONPHOTOS\" SET \"EP_NAMEPLATE_PATH\"='" + objCR.sNamePlatePhotoPath + "',\"EP_SSPLATE_PATH\"='" + objCR.sSSPlatePhotoPath + "',";
                    strQry += " \"EP_OLDDTC_PATH\"='" + objCR.sOldCodePhotoPath + "',\"EP_DTLMSDTC_PATH\"='" + objCR.sDTLMSCodePhotoPath + "',";
                    strQry += " \"EP_IPENUMDTC_PATH\"='" + objCR.sIPEnumCodePhotoPath + "',\"EP_INFOSYSDTC_PATH\"='" + objCR.sInfosysCodePhotoPath + "',";
                    strQry += " \"EP_DTC_PATH\"='" + objCR.sDTCPhoto1Path + "' WHERE \"EP_ED_ID\"='" + objCR.sEnumDetailsID + "' ";
                    ObjCon.ExecuteQry(strQry);
                }
                else
                {
                    SaveImagePathDetails(objCR);
                }


                return true;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "UpdateImagePathDetails");
                return false;
            }
        }


        public clsNewDTCCR GetCRDetailsFromXML(clsNewDTCCR objRIApproval)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable dt = new DataTable();

                dt = objApproval.GetDatatableFromXML(objRIApproval.sWFDataId);
                if (dt.Rows.Count > 0)
                {
                    objRIApproval.sCRDate = Convert.ToString(dt.Rows[0]["CR_DATE"]);
                    if (dt.Columns.Contains("EP_NAMEPLATE_PATH"))
                    {
                        objRIApproval.sNamePlatePhotoPath = Convert.ToString(dt.Rows[0]["EP_NAMEPLATE_PATH"]);
                    }
                    if (dt.Columns.Contains("EP_SSPLATE_PATH"))
                    {
                        objRIApproval.sSSPlatePhotoPath = Convert.ToString(dt.Rows[0]["EP_SSPLATE_PATH"]);
                    }
                    if (dt.Columns.Contains("EP_DTC_PATH"))
                    {
                        objRIApproval.sDTCPhoto1Path = Convert.ToString(dt.Rows[0]["EP_DTC_PATH"]);
                    }
                    if (dt.Columns.Contains("EP_OLDDTC_PATH"))
                    {
                        objRIApproval.sDTCPhoto2Path = Convert.ToString(dt.Rows[0]["EP_OLDDTC_PATH"]);
                    }
                    if (dt.Columns.Contains("EP_DTLMSDTC_PATH"))
                    {
                        objRIApproval.sDTLMSCodePhotoPath = Convert.ToString(dt.Rows[0]["EP_DTLMSDTC_PATH"]);
                    }
                    if (dt.Columns.Contains("ED_ID"))
                    {
                        objRIApproval.sEnumDetailsID = Convert.ToString(dt.Rows[0]["ED_ID"]);
                    }
                    if (dt.Columns.Contains("EP_INFOSYSDTC_PATH"))
                    {
                        objRIApproval.sInfosysCodePhotoPath = Convert.ToString(dt.Rows[0]["EP_INFOSYSDTC_PATH"]);
                    }
                    if (dt.Columns.Contains("ED_IP_ENUM_DONE"))
                    {
                        objRIApproval.sIPEnumCodePhotoPath = Convert.ToString(dt.Rows[0]["ED_IP_ENUM_DONE"]);
                    }
                    if (dt.Columns.Contains("ED_APPROVALPRIORITY"))
                    {
                        objRIApproval.sLevel = Convert.ToInt16(dt.Rows[0]["ED_APPROVALPRIORITY"]);
                    }
                }
                return objRIApproval;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objRIApproval;
            }
        }
    }
}
