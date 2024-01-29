using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data;

namespace IIITS.DTLMS.BL
{
    public class clsRevertBack
    {

        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);

        public string sDTCCode { get; set; }
        public string sEdID { get; set; }
        public string sTcCode { get; set; }
        public string sTcLoc { get; set; }



        public DataTable getDtcDetails(clsRevertBack objRevert)
        {
            DataTable dtDtcDetails = new DataTable();
            string strQry = string.Empty;
            strQry += "SELECT ED_ID,DTE_DTCCODE,ED_STATUS_FLAG,ED_ENUM_TYPE,ED_LOCTYPE FROM TBLENUMERATIONDETAILS,TBLDTCENUMERATION WHERE ED_ID=DTE_ED_ID ";
            if (objRevert.sDTCCode != "")
            {
                strQry += "AND DTE_DTCCODE='" + objRevert.sDTCCode + "'"; 
            }
            if (objRevert.sTcCode != "")
            {
                strQry += "AND DTE_TC_CODE='" + objRevert.sTcCode + "'";
            }
            dtDtcDetails = ObjCon.getDataTable(strQry);
            return dtDtcDetails;
        }

        public DataTable GetTcCode(string tccode,string LocType)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                if (LocType == "2")
                {
                    //strQry = "SELECT TC_STATUS,TC_CURRENT_LOCATION,TC_CODE,SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)OFF_NAME FROM TBLTCMASTER,TBLDTCMAST,VIEW_ALL_OFFICES WHERE TC_CODE=DT_TC_ID AND OFF_CODE=DT_OM_SLNO AND  DT_CODE='" + tccode + "'";
                    strQry = "SELECT TC_STATUS,TC_CURRENT_LOCATION,TC_CODE,SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)OFF_NAME FROM TBLTCMASTER,TBLDTCMAST,TBLDTCFAILURE,VIEW_ALL_OFFICES WHERE DT_CODE=DF_DTC_CODE AND DF_EQUIPMENT_ID=TC_CODE AND OFF_CODE=DT_OM_SLNO AND  DT_CODE='" + tccode + "'";
                    dt = ObjCon.getDataTable(strQry);
                }
                if (LocType == "1")
                {
                    //strQry = "SELECT TC_STATUS,TC_CURRENT_LOCATION,TC_CODE,SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)OFF_NAME FROM TBLTCMASTER,VIEW_ALL_OFFICES WHERE OFF_CODE=TC_LOCATION_ID AND  TC_CODE='" + tccode + "'";
                    strQry = "SELECT TC_STATUS,TC_CURRENT_LOCATION,TC_CODE,SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)OFF_NAME FROM TBLTCMASTER,TBLDTCMAST,TBLDTCFAILURE,VIEW_ALL_OFFICES WHERE DT_CODE=DF_DTC_CODE AND DF_EQUIPMENT_ID=TC_CODE AND OFF_CODE=DT_OM_SLNO AND  TC_CODE='" + tccode + "'";
                    dt = ObjCon.getDataTable(strQry);
                }
            }
            catch (Exception ex)
            {
                
            }
            return dt;
        }


        public DataTable GetTCdetails(string tccode, string LocType)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                if (LocType == "2")
                {
                    strQry = "SELECT TC_STATUS,TC_CURRENT_LOCATION,TC_CODE,SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)OFF_NAME FROM TBLTCMASTER,TBLDTCMAST,VIEW_ALL_OFFICES WHERE TC_CODE=DT_TC_ID AND OFF_CODE=DT_OM_SLNO AND  DT_CODE='" + tccode + "'";
                    //strQry = "SELECT TC_STATUS,TC_CURRENT_LOCATION,TC_CODE,SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)OFF_NAME FROM TBLTCMASTER,TBLDTCMAST,TBLDTCFAILURE,VIEW_ALL_OFFICES WHERE DT_CODE=DF_DTC_CODE AND DF_EQUIPMENT_ID=TC_CODE AND OFF_CODE=DT_OM_SLNO AND  DT_CODE='" + tccode + "'";
                    dt = ObjCon.getDataTable(strQry);
                }
                if (LocType == "1")
                {
                    strQry = "SELECT TC_STATUS,TC_CURRENT_LOCATION,TC_CODE,SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)OFF_NAME FROM TBLTCMASTER,VIEW_ALL_OFFICES WHERE OFF_CODE=TC_LOCATION_ID AND  TC_CODE='" + tccode + "'";
                    //strQry = "SELECT TC_STATUS,TC_CURRENT_LOCATION,TC_CODE,SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)OFF_NAME FROM TBLTCMASTER,TBLDTCMAST,TBLDTCFAILURE,VIEW_ALL_OFFICES WHERE DT_CODE=DF_DTC_CODE AND DF_EQUIPMENT_ID=TC_CODE AND OFF_CODE=DT_OM_SLNO AND  TC_CODE='" + tccode + "'";
                    dt = ObjCon.getDataTable(strQry);
                }
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public bool TcRepairTransaction(string Tccode)
        {
            string strQry = string.Empty;
            strQry = "SELECT RSD_ID FROM TBLREPAIRSENTDETAILS WHERE RSD_TC_CODE='" + Tccode + "'";
            string val = ObjCon.get_value(strQry);
            if (val == null || val == "")
            {
                return true;
            }
            else
                return false;
        }

        public string[] DeletDTRDetailsAtStore(clsRevertBack objRevert)
        {
            DataTable dtAllId = new DataTable();
            string[] sArr = new string[2];
            string strQry = string.Empty;

            try
            {
                strQry = " SELECT ED_ID,QA_ID,QAO_ID,TC_ID,DTE_DTCCODE,ED_STATUS_FLAG,ED_ENUM_TYPE FROM TBLENUMERATIONDETAILS,";
                strQry += " TBLTCMASTER,TBLDTCENUMERATION,TBLQCAPPROVED,TBLQCAPPROVEDOBJECTS where ED_ID=DTE_ED_ID and QA_ID=QAO_QA_ID AND ED_LOCTYPE='1' ";
                strQry += " and QAO_TC_CODE=DTE_TC_CODE AND TC_CODE=QAO_TC_CODE AND DTE_TC_CODE='" + objRevert.sTcCode + "' AND ED_LOCTYPE=1";
                dtAllId = ObjCon.getDataTable(strQry);

                if (dtAllId.Rows.Count == 0)
                {
                    sArr[0] = "-1";
                    sArr[1] = "In All Table(TBLENUMERATIONDETAILS,TBLTCMASTER,TBLDTCENUMERATION,TBLQCAPPROVED,TBLQCAPPROVEDOBJECTS) Data May be Not There";
                    return sArr;
                }

                //Change Status In TBLENUMERATIONDETAILS
                ObjCon.BeginTrans();
                strQry = "UPDATE TBLENUMERATIONDETAILS SET ED_STATUS_FLAG='5' WHERE ED_ID='" + dtAllId.Rows[0]["ED_ID"].ToString() + "'";
                ObjCon.Execute(strQry);

                //Delete Data From Main Table

                //TBLQCAPPROVEDOBJECTS
                strQry = "DELETE FROM TBLQCAPPROVEDOBJECTS WHERE QAO_ID='" + dtAllId.Rows[0]["QAO_ID"].ToString() + "'";
                ObjCon.Execute(strQry);

                //TBLQCAPPROVED
                strQry = " DELETE FROM TBLQCAPPROVED WHERE QA_ID='" + dtAllId.Rows[0]["QA_ID"].ToString() + "'";
                ObjCon.Execute(strQry);


                //TBLTCMASTER
                strQry = " DELETE FROM TBLTCMASTER WHERE TC_ID='" + dtAllId.Rows[0]["TC_ID"].ToString() + "'";
                ObjCon.Execute(strQry);

                //TBLDTRTRANSACTION
                strQry = "DELETE FROM TBLDTRTRANSACTION WHERE DRT_ID IN(SELECT DRT_ID  FROM TBLDTRTRANSACTION where DRT_DTR_CODE='" + objRevert.sTcCode + "')";
                ObjCon.Execute(strQry);


                ObjCon.CommitTrans();
                sArr[0] = "0";
                sArr[1] = "Data Deleted Successfully In Store";
                return sArr;
            }
            catch (Exception ex)
            {
                ObjCon.RollBack();
                sArr[0] = "1";
                sArr[1] = "Exception Occured '" + ex.Message + "'";
                return sArr;
            }


        }

        public string[] DeletDTCDetailsAtField(clsRevertBack objRevert)
        {
            DataTable dtAllId = new DataTable();
            string[] sArr = new string[2];
            string strQry = string.Empty;

            try
            {
                strQry = " select ED_ID,QA_ID,QAO_ID,DT_ID,DTE_ID,TC_ID,DTE_DTCCODE,ED_STATUS_FLAG,ED_ENUM_TYPE from TBLENUMERATIONDETAILS,";
                strQry += " TBLDTCENUMERATION,TBLQCAPPROVED,TBLQCAPPROVEDOBJECTS,TBLTCMASTER,TBLDTCMAST ";
                strQry += " WHERE ED_ID=DTE_ED_ID AND ED_ID=QA_ED_ID AND TC_CODE=DT_TC_ID AND QA_ID=QAO_QA_ID AND";
                strQry += " DT_CODE=DTE_DTCCODE  AND DTE_DTCCODE='" + objRevert.sDTCCode + "' AND ED_LOCTYPE=2";
                dtAllId = ObjCon.getDataTable(strQry);

                if (dtAllId.Rows.Count == 0)
                {
                    sArr[0] = "-1";
                    sArr[1] = "In All Table(TBLENUMERATIONDETAILS,TBLTCMASTER,TBLDTCENUMERATION,TBLQCAPPROVED,TBLQCAPPROVEDOBJECTS) Data May be Not There";
                    return sArr;
                }

                //Change Status In TBLENUMERATIONDETAILS
                ObjCon.BeginTrans();
                strQry = "UPDATE TBLENUMERATIONDETAILS SET ED_STATUS_FLAG='5' WHERE ED_ID='" + dtAllId.Rows[0]["ED_ID"].ToString() + "'";
                ObjCon.Execute(strQry);

                //Delete Data From Main Table

                //TBLQCAPPROVEDOBJECTS
                strQry = "DELETE FROM TBLQCAPPROVEDOBJECTS WHERE QAO_ID='" + dtAllId.Rows[0]["QAO_ID"].ToString() + "'";
                ObjCon.Execute(strQry);

                //TBLQCAPPROVED
                strQry = " DELETE FROM TBLQCAPPROVED WHERE QA_ID='" + dtAllId.Rows[0]["QA_ID"].ToString() + "'";
                ObjCon.Execute(strQry);


                //TBLTCMASTER
                strQry = " DELETE FROM TBLTCMASTER WHERE TC_ID='" + dtAllId.Rows[0]["TC_ID"].ToString() + "'";
                ObjCon.Execute(strQry);

                //TBLDTRTRANSACTION
                strQry = "DELETE FROM TBLDTRTRANSACTION WHERE DRT_ID IN(SELECT DRT_ID  FROM TBLDTRTRANSACTION where DRT_DTR_CODE='" + objRevert.sTcCode + "')";
                ObjCon.Execute(strQry);

                //TBLTRANSDTCMAPPING
                strQry = "DELETE FROM TBLTRANSDTCMAPPING WHERE TM_ID IN (SELECT TM_ID FROM TBLTRANSDTCMAPPING WHERE TM_DTC_ID='" + objRevert.sDTCCode + "')";
                ObjCon.Execute(strQry);


                ObjCon.CommitTrans();
                sArr[0] = "0";
                sArr[1] = "Data Deleted Successfully In Field";
                return sArr;
            }
            catch (Exception ex)
            {
                ObjCon.RollBack();
                sArr[0] = "1";
                sArr[1] = "Exception Occured";
                return sArr;
            }


        }

        public string[] RevertDTRDetailsAtStore(clsRevertBack objRevert)
        {
            DataTable dtAllId = new DataTable();
            string[] sArr = new string[2];
            string strQry = string.Empty;

            try
            {
                strQry = " SELECT ED_ID,QA_ID,QAO_ID,TC_ID,DTE_DTCCODE,ED_STATUS_FLAG,ED_ENUM_TYPE FROM TBLENUMERATIONDETAILS,";
                strQry += " TBLTCMASTER,TBLDTCENUMERATION,TBLQCAPPROVED,TBLQCAPPROVEDOBJECTS where ED_ID=DTE_ED_ID and QA_ID=QAO_QA_ID AND ED_LOCTYPE='1' ";
                strQry += " and QAO_TC_CODE=DTE_TC_CODE AND TC_CODE=QAO_TC_CODE AND DTE_TC_CODE='" + objRevert.sTcCode + "' AND ED_LOCTYPE=1";
                dtAllId = ObjCon.getDataTable(strQry);

                if (dtAllId.Rows.Count == 0)
                {
                    sArr[0] = "-1";
                    sArr[1] = "In All Table(TBLENUMERATIONDETAILS,TBLTCMASTER,TBLDTCENUMERATION,TBLQCAPPROVED,TBLQCAPPROVEDOBJECTS) Data May be Not There";
                    return sArr;
                }

                //Change Status In TBLENUMERATIONDETAILS
                ObjCon.BeginTrans();
                strQry = "UPDATE TBLENUMERATIONDETAILS SET ED_STATUS_FLAG='0' WHERE ED_ID='" + dtAllId.Rows[0]["ED_ID"].ToString() + "'";
                ObjCon.Execute(strQry);

                //Delete Data From Main Table

                //TBLQCAPPROVEDOBJECTS
                strQry = "DELETE FROM TBLQCAPPROVEDOBJECTS WHERE QAO_ID='" + dtAllId.Rows[0]["QAO_ID"].ToString() + "'";
                ObjCon.Execute(strQry);

                //TBLQCAPPROVED
                strQry = " DELETE FROM TBLQCAPPROVED WHERE QA_ID='" + dtAllId.Rows[0]["QA_ID"].ToString() + "'";
                ObjCon.Execute(strQry);


                //TBLTCMASTER
                strQry = " DELETE FROM TBLTCMASTER WHERE TC_ID='" + dtAllId.Rows[0]["TC_ID"].ToString() + "'";
                ObjCon.Execute(strQry);

                //TBLDTRTRANSACTION
                strQry = "DELETE FROM TBLDTRTRANSACTION WHERE DRT_ID IN(SELECT DRT_ID  FROM TBLDTRTRANSACTION where DRT_DTR_CODE='" + objRevert.sTcCode + "')";
                ObjCon.Execute(strQry);


                ObjCon.CommitTrans();
                sArr[0] = "0";
                sArr[1] = "Data Reverted Successfully In Store";
                return sArr;
            }
            catch (Exception ex)
            {
                ObjCon.RollBack();
                sArr[0] = "1";
                sArr[1] = "Exception Occured '" + ex.Message + "'";
                return sArr;
            }


        }

        public string[] RevertDTCDetailsAtField(clsRevertBack objRevert)
        {
            DataTable dtAllId = new DataTable();
            string[] sArr = new string[2];
            string strQry = string.Empty;

            try
            {
                strQry = " select ED_ID,QA_ID,QAO_ID,DT_ID,DTE_ID,TC_ID,DTE_DTCCODE,ED_STATUS_FLAG,TC_CODE,ED_ENUM_TYPE from TBLENUMERATIONDETAILS,";
                strQry += " TBLDTCENUMERATION,TBLQCAPPROVED,TBLQCAPPROVEDOBJECTS,TBLTCMASTER,TBLDTCMAST ";
                strQry += " WHERE ED_ID=DTE_ED_ID AND ED_ID=QA_ED_ID AND TC_CODE=DT_TC_ID AND QA_ID=QAO_QA_ID AND";
                strQry += " DT_CODE=DTE_DTCCODE  AND DTE_DTCCODE='" + objRevert.sDTCCode + "' AND ED_LOCTYPE=2";
                dtAllId = ObjCon.getDataTable(strQry);
                if (dtAllId.Rows.Count == 0)
                {
                    sArr[0] = "-1";
                    sArr[1] = "In All Table(TBLENUMERATIONDETAILS,TBLDTCENUMERATION,TBLQCAPPROVED,TBLQCAPPROVEDOBJECTS,TBLTCMASTER,TBLDTCMAST) Data May be Not There";
                    return sArr;
                }



                //Change Status In TBLENUMERATIONDETAILS
                ObjCon.BeginTrans();
                strQry = "UPDATE TBLENUMERATIONDETAILS SET ED_STATUS_FLAG='0' WHERE ED_ID='" + dtAllId.Rows[0]["ED_ID"].ToString() + "'";
                ObjCon.Execute(strQry);

                //Delete Data From Main Table

                //TBLQCAPPROVEDOBJECTS
                strQry = "DELETE FROM TBLQCAPPROVEDOBJECTS WHERE QAO_ID='" + dtAllId.Rows[0]["QAO_ID"].ToString() + "'";
                ObjCon.Execute(strQry);

                //TBLQCAPPROVED
                strQry = " DELETE FROM TBLQCAPPROVED WHERE QA_ID='" + dtAllId.Rows[0]["QA_ID"].ToString() + "'";
                ObjCon.Execute(strQry);

                //TBLDTCMAST
                strQry = "DELETE FROM TBLDTCMAST WHERE DT_ID='" + dtAllId.Rows[0]["DT_ID"].ToString() + "'";
                ObjCon.Execute(strQry);

                //TBLTCMASTER
                strQry = " DELETE FROM TBLTCMASTER WHERE TC_ID='" + dtAllId.Rows[0]["TC_ID"].ToString() + "'";
                ObjCon.Execute(strQry);

                //TBLDTRTRANSACTION
                strQry = "DELETE FROM TBLDTRTRANSACTION WHERE DRT_ID IN(SELECT DRT_ID  FROM TBLDTRTRANSACTION where DRT_DTR_CODE IN";
                strQry += " (SELECT dte_tc_code FROM TBLDTCENUMERATION,TBLENUMERATIONDETAILS WHERE ED_ID=DTE_ED_ID AND dte_DTCCODE='" + objRevert.sDTCCode + "'))";
                ObjCon.Execute(strQry);

                //TBLDTCTRANSACTION
                strQry = "DELETE FROM TBLDTCTRANSACTION WHERE DCT_ID IN(";
                strQry += "SELECT DCT_ID  FROM TBLDTCTRANSACTION WHERE DCT_DTC_CODE='" + objRevert.sDTCCode + "')";
                ObjCon.Execute(strQry);

                //TBLTRANSDTCMAPPING
                strQry = "DELETE FROM TBLTRANSDTCMAPPING WHERE TM_ID IN (SELECT TM_ID FROM TBLTRANSDTCMAPPING WHERE TM_DTC_ID='" + objRevert.sDTCCode + "')";
                ObjCon.Execute(strQry);

                ObjCon.CommitTrans();
                sArr[0] = "0";
                sArr[1] = "Data Reverted Successfully In Field";
                return sArr;
            }
            catch (Exception ex)
            {
                ObjCon.RollBack();
                sArr[0] = "1";
                sArr[1] = "Exception Occured";
                return sArr;
            }


        }


    }
}

