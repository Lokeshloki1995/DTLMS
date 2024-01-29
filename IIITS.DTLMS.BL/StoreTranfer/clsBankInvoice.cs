using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using IIITS.PGSQL.DAL;
using Npgsql;

namespace IIITS.DTLMS.BL
{
    public class clsBankInvoice
    {
        PGSqlConnection objCon = new PGSqlConnection(Constants.Password);
        public string strFormCode = "clsBankInvoice";
        public string sIndentId { get; set; }
        public DataTable dtCapacity { get; set; }
        public DataTable dtDtrList { get; set; }
        public string sIndentNo { get; set; }
        public string sIndentDate { get; set; }
        public string sOMNo { get; set; }
        public string sOMDate { get; set; }
        public string sFilepath { get; set; }
        public string sInvId { get; set; }
        public string sInvNo { get; set; }
        public string sInvDate { get; set; }
        public string sTCSlno { get; set; }
        public string sTCCOde { get; set; }
        public string sTCCapacity { get; set; }
        public string sQuantity { get; set; }
        public string sMake { get; set; }
        public string sTCID { get; set; }
        public string sUserId { get; set; }
        public string sOfficeCode { get; set; }

        public string sVehicleNo { get; set;}
        public string sChallanNo { get; set; }
        public string sReceipient { get; set; }

        public string sClientIP { get; set; }
        public string sActionType { get; set; }
        public string sWFDataId { get; set; }
        public string sWFAutoId { get; set; }
        public string sWFO_id { get; set; }
        NpgsqlCommand NpgsqlCommand;
        public void LoadTcDetails(clsBankInvoice objInvoice)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string strQry = string.Empty;
            DataTable dtTcDetails = new DataTable();
            try
            {
                NpgsqlCommand.Parameters.AddWithValue("TCCOde",Convert.ToDouble(objInvoice.sTCCOde));
                NpgsqlCommand.Parameters.AddWithValue("OfficeCode",Convert.ToInt32(objInvoice.sOfficeCode));            
                strQry = "SELECT \"TC_ID\",\"TC_SLNO\",\"TC_CODE\",CAST(\"TC_CAPACITY\" AS TEXT)TC_CAPACITY,(SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE ";
                strQry += " \"TM_ID\" = \"TC_MAKE_ID\" )TM_NAME FROM \"TBLTCMASTER\" WHERE \"TC_CODE\" =:TCCOde AND ";
                strQry += " \"TC_CURRENT_LOCATION\" =1 AND \"TC_LOCATION_ID\" = :OfficeCode ";

                dtTcDetails = objCon.FetchDataTable(strQry, NpgsqlCommand);
                if (dtTcDetails.Rows.Count > 0)
                {
                    objInvoice.sTCID = Convert.ToString(dtTcDetails.Rows[0]["TC_ID"]);
                    objInvoice.sTCSlno = Convert.ToString(dtTcDetails.Rows[0]["TC_SLNO"]);
                    objInvoice.sTCCOde = Convert.ToString(dtTcDetails.Rows[0]["TC_CODE"]);
                    objInvoice.sTCCapacity = Convert.ToString(dtTcDetails.Rows[0]["TC_CAPACITY"]);
                    objInvoice.sMake = Convert.ToString(dtTcDetails.Rows[0]["TM_NAME"]);
                }                   
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetIndentDetails(clsBankInvoice objInvoice)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                string sQry = string.Empty;
                NpgsqlCommand.Parameters.AddWithValue("IndentId",Convert.ToInt16(sIndentId));
                sQry = "SELECT \"BI_ID\", \"BI_NO\", TO_CHAR(\"BI_DATE\",'DD/MM/YYYY') \"BI_DATE\", \"BI_OM_NO\", TO_CHAR(\"BI_OM_DATE\",'DD/MM/YYYY') \"BI_OM_DATE\", ";
                sQry += " \"BI_OM_FILEPATH\" FROM \"TBLBANKINDENT\" WHERE \"BI_ID\" = :IndentId";
                DataTable dt = new DataTable();
                dt = objCon.FetchDataTable(sQry, NpgsqlCommand);
                if(dt.Rows.Count > 0)
                {
                    objInvoice.sIndentId = Convert.ToString(dt.Rows[0]["BI_ID"]);
                    objInvoice.sIndentNo = Convert.ToString(dt.Rows[0]["BI_NO"]);
                    objInvoice.sIndentDate = Convert.ToString(dt.Rows[0]["BI_DATE"]);
                    objInvoice.sOMNo = Convert.ToString(dt.Rows[0]["BI_OM_NO"]);
                    objInvoice.sOMDate = Convert.ToString(dt.Rows[0]["BI_OM_DATE"]);
                    objInvoice.sFilepath = Convert.ToString(dt.Rows[0]["BI_OM_FILEPATH"]);
                }
                NpgsqlCommand.Parameters.AddWithValue("IndentId1",Convert.ToInt16(sIndentId));
                sQry = "SELECT \"BO_ID\", \"BO_CAPACITY\",  \"BO_QUANTITY\"  FROM \"TBLBIOBJECTS\" WHERE \"BO_BI_ID\" = :IndentId1";
                dt = objCon.FetchDataTable(sQry, NpgsqlCommand);
                objInvoice.dtCapacity = dt;
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public string[] SaveInvoiceDetails(clsBankInvoice objInv)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string[] Arr = new string[3];
            try
            {
                NpgsqlCommand.Parameters.AddWithValue("InvNo",  objInv.sInvNo );
                string sInNo = objCon.get_value("SELECT \"BV_NO\" FROM \"TBLBANKINVOICE\" WHERE \"BV_NO\" =:InvNo", NpgsqlCommand);
                if (sInNo.Length > 0)
                {
                    Arr[0] = "Entered Invoice Number Already Exists";
                    Arr[1] = "2";
                    return Arr;
                }

                string sQry = string.Empty;
                sQry = "INSERT INTO \"TBLBANKINVOICE\" (\"BV_ID\", \"BV_BI_NO\" , \"BV_NO\", \"BV_DATE\", \"BV_OFF_CODE\", \"BV_CRBY\")";
                sQry += " VALUES({0}, '" + objInv.sIndentId + "', '" + objInv.sInvNo + "', TO_DATE('" + objInv.sInvDate + "','DD/MM/YYYY') , ";
                sQry += " '" + objInv.sOfficeCode + "','" + objInv.sUserId + "')";
                sQry = sQry.Replace("'", "''");
                StringBuilder sbQuery = new StringBuilder();
                String sQry1 = string.Empty;

                for (int i = 0; i < objInv.dtDtrList.Rows.Count; i++)
                {
                    objInv.sTCCOde += Convert.ToString(objInv.dtDtrList.Rows[i]["TC_CODE"]) + "`";
                    objInv.sTCSlno += Convert.ToString(objInv.dtDtrList.Rows[i]["TC_SLNO"]) + "`";
                    objInv.sTCID += Convert.ToString(objInv.dtDtrList.Rows[i]["TC_ID"]) + "`";
                    objInv.sTCCapacity += Convert.ToString(objInv.dtDtrList.Rows[i]["TC_CAPACITY"]) + "`";
                    objInv.sMake += Convert.ToString(objInv.dtDtrList.Rows[i]["TM_NAME"]) + "`";


                    sQry1 = "INSERT INTO \"TBLBVOBJECTS\"(\"BVO_BV_ID\", \"BV_DTRCODE\") VALUES ('{0}','" + Convert.ToString(objInv.dtDtrList.Rows[i]["TC_CODE"]) + "') ";
                    sbQuery.Append(sQry1);
                    sbQuery.Append(";");
                }

                sbQuery = sbQuery.Replace("'", "''");
                string sParam = "SELECT COALESCE(MAX(\"BV_ID\"),0)+1 FROM \"TBLBANKINVOICE\"";

                clsApproval objApproval = new clsApproval();
                objApproval.sFormName = "TransBankInvoice";
                objApproval.sOfficeCode = objInv.sOfficeCode;
                objApproval.sClientIp = objInv.sClientIP;
                objApproval.sCrby = objInv.sUserId;
                objApproval.sQryValues = sQry + ";" + sbQuery;
                objApproval.sParameterValues = sParam;
                objApproval.sWFObjectId = objInv.sWFO_id;
                objApproval.sMainTable = "TBLBANKINDENT";
                objApproval.sDataReferenceId = objInv.sIndentId;
                objApproval.sDescription = "Invoice for Transformer Bank";

                //objApproval.sRefOfficeCode = objCon.get_value("SELECT \"BI_OFF_CODE\" FROM \"TBLBANKINDENT\" WHERE \"BI_ID\"='" + objInv.sIndentId + "'");
                objApproval.sRefOfficeCode = objCon.get_value("SELECT substr(\"BI_NO\",1,4)\"BI_NO\" FROM \"TBLBANKINDENT\" WHERE \"BI_ID\"='" + objInv.sIndentId + "'");

                //objApproval.sRefOfficeCode = objInv.sOfficeCode;
                string sPrimaryKey = "{0}";

                objApproval.sColumnNames = "BV_ID,BV_BI_NO,BV_NO,BV_DATE,BV_OFF_CODE,BV_CRBY";
                objApproval.sColumnNames += ";TC_ID,TC_SLNO,TC_CODE,TC_CAPACITY,TM_NAME";
                objApproval.sColumnValues = "" + sPrimaryKey + "," + objInv.sIndentId + "," + objInv.sInvNo + "," + objInv.sInvDate + "," + objInv.sOfficeCode + "," + objInv.sUserId + "";
                objApproval.sColumnValues += "; " + objInv.sTCID + "," + objInv.sTCSlno + "," + objInv.sTCCOde + "," + objInv.sTCCapacity + "," + objInv.sMake + "";
                objApproval.sTableNames = "TBLBANKINVOICE;TBLBVOBJECTS";

                bool bApproveResult = objApproval.CheckDuplicateApprove(objApproval);
                if (bApproveResult == false)
                {
                    Arr[0] = "Selected Record Already Approved";
                    Arr[1] = "2";
                    return Arr;
                }

                if (objInv.sActionType == "M")
                {
                    objApproval.SaveWorkFlowData(objApproval);
                    objInv.sWFDataId = objApproval.sWFDataId;
                }
                else
                {
                    objApproval.SaveWorkFlowData(objApproval);
                    objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow();
                    objApproval.SaveWorkflowObjects(objApproval);
                }


                Arr[0] = "Saved Successfully";
                Arr[1] = "0";
                Arr[2] = objApproval.sNewRecordId;
                return Arr;

            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                Arr[0] = "Error While Saving";
                Arr[1] = "1";
                return Arr;
            }
        }

        public void GetIndentId(clsBankInvoice obj)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                string sQry = string.Empty;
                NpgsqlCommand.Parameters.AddWithValue("IndentId", Convert.ToInt32(obj.sIndentId));
                sQry = "SELECT \"WO_DATA_ID\" FROM \"TBLWORKFLOWOBJECTS\" WHERE \"WO_RECORD_ID\" = :IndentId";
                obj.sIndentId = objCon.get_value(sQry, NpgsqlCommand);
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetIndentDetailsFromXML(clsBankInvoice obj)
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
                            obj.sInvNo = Convert.ToString(dt.Rows[0]["BV_NO"]);
                            obj.sInvDate = Convert.ToString(dt.Rows[0]["BV_DATE"]);
                        }

                        else if (i == 1)
                        {
                            obj.sTCCOde = Convert.ToString(dt.Rows[0]["TC_CODE"]);
                            obj.sTCSlno = Convert.ToString(dt.Rows[0]["TC_SLNO"]);
                            obj.sTCID = Convert.ToString(dt.Rows[0]["TC_ID"]);
                            obj.sTCCapacity = Convert.ToString(dt.Rows[0]["TC_CAPACITY"]);
                            obj.sMake = Convert.ToString(dt.Rows[0]["TM_NAME"]);
                            obj.dtDtrList = CreateDatatableFromString(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public DataTable CreateDatatableFromString(clsBankInvoice objWo)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("TC_CODE");
            dt.Columns.Add("TC_SLNO");
            dt.Columns.Add("TC_ID");
            dt.Columns.Add("TC_CAPACITY");
            dt.Columns.Add("TM_NAME");

            string[] Id = objWo.sTCID.Split('`');
            string[] Code = objWo.sTCCOde.Split('`');
            string[] Slno = objWo.sTCSlno.Split('`');
            string[] Capacity = objWo.sTCCapacity.Split('`');
            string[] Make = objWo.sMake.Split('`');

            for (int i = 0; i < Id.Length; i++)
            {
                for (int j = 0; j < Code.Length; j++)
                {
                    for (int k = 0; k < Slno.Length; k++)
                    {
                        for (int l = 0; l < Capacity.Length; l++)
                        {
                            for (int m = 0; m < Make.Length; m++)
                            {
                                if (Make[k] != "" && Make[k] != " ")
                                {
                                    DataRow dRow = dt.NewRow();
                                    dRow["TC_ID"] = Id[i];
                                    dRow["TC_CODE"] = Code[j];
                                    dRow["TC_SLNO"] = Slno[k];
                                    dRow["TC_CAPACITY"] = Capacity[l];
                                    dRow["TM_NAME"] = Make[m];
                                    dt.Rows.Add(dRow);
                                    dt.AcceptChanges();
                                }
                                i++;
                                j++;
                                k++;
                                l++;
                            }
                        }   
                    }
                }
            }
            return dt;
        }

        public DataTable GetGatepassDetails(string sInvId)
        {
            NpgsqlCommand = new NpgsqlCommand();
            DataTable dt = new DataTable();
            try
            {
                string sQry = string.Empty;
                NpgsqlCommand.Parameters.AddWithValue("InvId",Convert.ToInt16(sInvId));
                sQry = " SELECT \"TC_CODE\" \"RSD_TC_CODE\", '' AS \"DIV_NAME\", '' AS \"RSD_DELIVARY_DATE\" ,\"BV_NO\" \"RSM_INV_NO\", \"BV_DATE\" \"RSM_INV_DATE\", ";
                sQry += " \"BV_VEHICLENO\" \"GP_VEHICLE_NO\", \"BV_RECEIPIENT\" \"GP_RECIEPIENT_NAME\", \"BV_CHALLAN\" \"GP_CHALLEN_NO\" ,\"TC_SLNO\", \"TC_CAPACITY\", ";
                sQry += " \"TM_NAME\", TO_CHAR(\"TC_MANF_DATE\",'dd/MM/yyyy') \"TC_MANF_DATE\", (SELECT \"US_FULL_NAME\" FROM \"TBLUSER\" WHERE \"US_ID\" = \"BV_CRBY\") ";
                sQry += " \"STO_USERNAME\"  FROM \"TBLBANKINVOICE\", \"TBLBVOBJECTS\", \"TBLTCMASTER\", \"TBLTRANSMAKES\" WHERE \"BV_ID\" = \"BVO_BV_ID\" AND ";
                sQry += " \"BV_DTRCODE\" = \"TC_CODE\" AND \"TM_ID\" = \"TC_MAKE_ID\" AND \"BV_ID\" = :InvId";

                dt = objCon.FetchDataTable(sQry, NpgsqlCommand);
                return dt;
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public void UpdateGatepassDetails(clsBankInvoice objWo)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                string sQry = string.Empty;
                NpgsqlCommand.Parameters.AddWithValue("VehicleNo", objWo.sVehicleNo);
                NpgsqlCommand.Parameters.AddWithValue("Receipient", objWo.sReceipient);
                NpgsqlCommand.Parameters.AddWithValue("ChallanNo", objWo.sChallanNo);
                NpgsqlCommand.Parameters.AddWithValue("InvId",Convert.ToInt16(objWo.sInvId));

                sQry = "UPDATE \"TBLBANKINVOICE\" SET \"BV_VEHICLENO\" = :VehicleNo, \"BV_RECEIPIENT\" = :Receipient, ";
                sQry += "\"BV_CHALLAN\" = :ChallanNo WHERE \"BV_ID\" = :InvId";
                objCon.ExecuteQry(sQry, NpgsqlCommand);
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetInvoiceDetails(clsBankInvoice objInv)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                string sQry = string.Empty;
                NpgsqlCommand.Parameters.AddWithValue("InvId",Convert.ToInt16(objInv.sInvId));
                sQry = "SELECT * FROM \"TBLBANKINVOICE\" WHERE \"BV_ID\" = :InvId";
                DataTable dt = new DataTable();
                dt = objCon.FetchDataTable(sQry, NpgsqlCommand);
                if(dt.Rows.Count>0)
                {
                    objInv.sInvNo = Convert.ToString(dt.Rows[0]["BV_NO"]);
                    objInv.sIndentId = Convert.ToString(dt.Rows[0]["BV_BI_NO"]);
                    objInv.sInvDate = Convert.ToString(dt.Rows[0]["BV_DATE"]);
                }
                NpgsqlCommand.Parameters.AddWithValue("InvId1",Convert.ToInt16(objInv.sInvId));
                sQry = "SELECT * FROM \"TBLBANKINDENT\" WHERE \"BI_ID\" = :InvId1";
                dt = objCon.FetchDataTable(sQry, NpgsqlCommand);
                if (dt.Rows.Count > 0)
                {
                    objInv.sIndentNo = Convert.ToString(dt.Rows[0]["BI_NO"]);
                    objInv.sIndentDate = Convert.ToString(dt.Rows[0]["BI_DATE"]);
                    objInv.sOMNo = Convert.ToString(dt.Rows[0]["BI_OM_NO"]);
                    objInv.sOMDate = Convert.ToString(dt.Rows[0]["BI_OM_DATE"]);
                }
                NpgsqlCommand.Parameters.AddWithValue("InvId2",Convert.ToInt16(objInv.sInvId));
                sQry = " SELECT \"TC_CODE\",\"TC_SLNO\",CAST(\"TC_CAPACITY\" AS TEXT) \"TC_CAPACITY\",(SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE ";
                sQry += " \"TM_ID\"=\"TC_MAKE_ID\") \"TM_NAME\",CASE WHEN \"TC_STATUS\"='1' THEN 'BRAND NEW' WHEN \"TC_STATUS\"='2' THEN 'REPAIR GOOD' ";
                sQry += " WHEN \"TC_STATUS\"='3' THEN 'FAULTY' END \"STATUS\" FROM \"TBLTCMASTER\",\"TBLBVOBJECTS\" ";
                sQry += " WHERE \"BVO_BV_ID\" =:InvId2 AND \"BV_DTRCODE\"=\"TC_CODE\" ";
                objInv.dtDtrList = objCon.FetchDataTable(sQry, NpgsqlCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

    }
}
