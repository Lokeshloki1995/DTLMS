using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IIITS.PGSQL.DAL;
using System.Data;
using Npgsql;

namespace IIITS.DTLMS.BL
{
    public class clsBankIndent
    {
        PGSqlConnection objCon = new PGSqlConnection(Constants.Password);
        public string strFormCode = "clsBankIndent";

        public string sIndentNo { get; set; }
        public string sIndentDate { get; set; }
        public string sOMNo { get; set; }
        public string sOMDate { get; set; }
        public string sFilepath { get; set; }
        public string sUserId { get; set; }
        public string sOfficeCode { get; set; }

        public string sCapacity { get; set; }
        public string sCount { get; set; }
        public string sID { get; set; }
        public DataTable dtCapacity { get; set; }
        public string sClientIP { get; set; }

        public string sActionType { get; set; }
        public string sWFDataId { get; set; }
        public string sWFAutoId { get; set; }
        public string sWFO_id { get; set; }

        NpgsqlCommand NpgsqlCommand;
        public string[] SaveBankIndent(clsBankIndent objBank)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string[] Arr = new string[3];
            try
            {
                NpgsqlCommand.Parameters.AddWithValue("OMNo", objBank.sOMNo);
                string sInNo = objCon.get_value("SELECT \"BI_OM_NO\" FROM \"TBLBANKINDENT\" WHERE \"BI_OM_NO\" =:OMNo", NpgsqlCommand);
                if (sInNo.Length > 0)
                {
                    Arr[0] = "Entered OM Number Already Exists";
                    Arr[1] = "2";
                    return Arr;
                }
                string sQry = string.Empty;

                sQry = "INSERT INTO \"TBLBANKINDENT\"(\"BI_ID\", \"BI_NO\", \"BI_DATE\", \"BI_OM_NO\", \"BI_OM_DATE\",\"BI_OM_FILEPATH\",\"BI_OFF_CODE\",\"BI_CRBY\")";
                sQry += " VALUES('{0}','" + objBank.sIndentNo + "',TO_DATE('" + objBank.sIndentDate + "','DD/MM/YYYY'),'" + objBank.sOMNo + "', ";
                sQry += " TO_DATE('" + objBank.sOMDate + "','DD/MM/YYYY'), '" + objBank.sFilepath + "', '" + objBank.sOfficeCode + "', '" + objBank.sUserId + "')";
                sQry = sQry.Replace("'", "''");

                StringBuilder sbQuery = new StringBuilder();
                String sQry1 = string.Empty;
                         

                for (int i = 0; i < objBank.dtCapacity.Rows.Count; i++)
                {
                    objBank.sCapacity += Convert.ToString(objBank.dtCapacity.Rows[i]["SO_CAPACITY"]) + "`";
                    objBank.sCount += Convert.ToString(objBank.dtCapacity.Rows[i]["SO_QNTY"]) + "`";
                    objBank.sID += Convert.ToString(objBank.dtCapacity.Rows[i]["SO_ID"]) + "`";

                    sQry1 = "INSERT INTO \"TBLBIOBJECTS\"(\"BO_BI_ID\", \"BO_CAPACITY\", \"BO_QUANTITY\") VALUES ('{0}','" + Convert.ToString(objBank.dtCapacity.Rows[i]["SO_CAPACITY"]) + "', ";
                    sQry1 += "'" + Convert.ToString(objBank.dtCapacity.Rows[i]["SO_QNTY"]) + "')";
                    sbQuery.Append(sQry1);
                    sbQuery.Append(";");
                }

                sbQuery = sbQuery.Replace("'", "''");
                string sParam = "SELECT COALESCE(MAX(\"BI_ID\"),0)+1 FROM \"TBLBANKINDENT\"";

                clsApproval objApproval = new clsApproval();
                objApproval.sFormName = "BankIndent";
                objApproval.sOfficeCode = objBank.sOfficeCode;
                objApproval.sClientIp = objBank.sClientIP;
                objApproval.sCrby = objBank.sUserId;
                objApproval.sQryValues = sQry + ";" + sbQuery;
                objApproval.sParameterValues = sParam;
                objApproval.sMainTable = "TBLBANKINDENT";
                objApproval.sDataReferenceId = objBank.sOfficeCode;
                objApproval.sDescription = "Indent for Requesting the DTR for Transformer Bank";
                objApproval.sRefOfficeCode = objBank.sOfficeCode;
                string sPrimaryKey = "{0}";


                objApproval.sColumnNames = "BI_ID,BI_NO,BI_DATE,BI_OM_NO,BI_OM_DATE,BI_OM_FILEPATH,BI_OFF_CODE,BI_CRBY";
                objApproval.sColumnNames += ";SO_ID,SO_CAPACITY,SO_QNTY";
                objApproval.sColumnValues = "" + sPrimaryKey + "," + objBank.sIndentNo + "," + objBank.sIndentDate + "," + objBank.sOMNo + "," + objBank.sOMDate + "," + objBank.sFilepath + "," + objBank.sOfficeCode + "," + objBank.sUserId + "";
                objApproval.sColumnValues += "; " + objBank.sID + "," + objBank.sCapacity + "," + objBank.sCount + "";
                objApproval.sTableNames = "TBLWORKAWARD;TBLWORKAWARDDETAILS";

                bool bApproveResult = objApproval.CheckDuplicateApprove(objApproval);
                if (bApproveResult == false)
                {
                    Arr[0] = "Selected Record Already Approved";
                    Arr[1] = "2";
                    return Arr;
                }

                if (objBank.sActionType == "M")
                {
                    objApproval.SaveWorkFlowData(objApproval);
                    objBank.sWFDataId = objApproval.sWFDataId;
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

        public void GetIndentDetailsFromXML(clsBankIndent obj)
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
                            obj.sIndentNo = Convert.ToString(dt.Rows[0]["BI_NO"]);
                            obj.sIndentDate = Convert.ToString(dt.Rows[0]["BI_DATE"]);
                            obj.sOMNo = Convert.ToString(dt.Rows[0]["BI_OM_NO"]);
                            obj.sOMDate = Convert.ToString(dt.Rows[0]["BI_OM_DATE"]);
                            obj.sFilepath = Convert.ToString(dt.Rows[0]["BI_OM_FILEPATH"]);
                        }
                       
                        else if (i == 1)
                        {
                            obj.sID = Convert.ToString(dt.Rows[0]["SO_ID"]);
                            obj.sCapacity = Convert.ToString(dt.Rows[0]["SO_CAPACITY"]);
                            obj.sCount = Convert.ToString(dt.Rows[0]["SO_QNTY"]);
                            obj.dtCapacity = CreateDatatableFromString(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public DataTable CreateDatatableFromString(clsBankIndent objWo)
        {


            DataTable dt = new DataTable();

            dt.Columns.Add("SO_ID");
            dt.Columns.Add("SO_CAPACITY");
            dt.Columns.Add("SO_QNTY");

            string[] Id = objWo.sID.Split('`');
            string[] Capacity = objWo.sCapacity.Split('`');
            string[] Quantity = objWo.sCount.Split('`');

            for (int i = 0; i < Id.Length; i++)
            {
                for (int j = 0; j < Capacity.Length; j++)
                {
                    for (int k = 0; k < Quantity.Length; k++)
                    {
                        if (Quantity[k] != "" && Quantity[k] != " ")
                        {
                            DataRow dRow = dt.NewRow();
                            dRow["SO_ID"] = Id[i];
                            dRow["SO_CAPACITY"] = Capacity[j];
                            dRow["SO_QNTY"] = Quantity[k];                            
                            dt.Rows.Add(dRow);
                            dt.AcceptChanges();
                        }
                        i++;
                        j++;                      
                    }
                }
            }
            return dt;
        }
    }
}
