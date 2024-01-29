using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DTLMS.BL;
using System.Data.OleDb;
using System.Data;
using IIITS.PGSQL.DAL;
using NpgsqlTypes;
using Npgsql;

namespace IIITS.DTLMS.BL
{
   public class clsStoreInvoice
    {
        //CustOledbConnection objCon = new CustOledbConnection(Constants.Password);
        PGSqlConnection objcon = new PGSqlConnection(Constants.Password);

        string strFormCode = "clsStoreInvoice";

        public string sIssueQty { get; set; }
        public string sFilepath { get; set; }

        public string sQuantity { get; set; }
        public string sTcCapacity { get; set; }
        public string sIndentNo { get; set; }
        public string sIndentDate { get; set; }
        public string sDescription { get; set; }
        public string sInvoiceNo { get; set; }
        public string sInvoiceDate { get; set; }
        public string sRemarks{ get; set; }
        public string sInvoiceId { get; set; }
        public string sCrBy { get; set; }
        public DataTable ddtTcGrid { get; set; }
        public string sIndentId { get; set; }
        public string sTcSlNo { get; set; }
        public string sInvoiceObjectId { get; set; }
        public string sSiId { get; set; }
        public string sFromStoreId { get; set; }
        public string sTcCode{ get; set; }
        public string sTcName { get; set; }
        public string sTcId { get; set; }
        public DataTable dIndentTcGrid { get; set; }

        // Workflow
        public string sFormName { get; set; }
        public string sClientIP { get; set; }
        public string sWFDataId { get; set; }
        public string sActionType { get; set; }
        public string sWFOId { get; set; }

        public string sOfficeCode { get; set; }
        public string sRefOfficeCode { get; set; }
        public string sRecordId { get; set; }
        public string sCrby { get; set; }

        public DataTable dtDocuments { get; set; }

        //To Load Indent details to grid      
       public DataTable LoadInvoiceGrid(string sOfficeCode)
       {
           string strQry = string.Empty;
           DataTable dtIndentDetails = new DataTable();
           try
           {
               //if (sOfficeCode.Length >= 3)
               //{
               //    sOfficeCode = sOfficeCode.Substring(0, Constants.Division);
               //}

                //strQry = "SELECT SI_ID, SI_DATE,SI_NO, REQ_QNTY, SI_FROM_STORE, (COALESCE(REQ_QNTY,0) - COALESCE(SENT_QNT,0)) AS PENDINGCOUNT,IS_NO  ";
                //strQry += "FROM (SELECT SI_ID,TO_CHAR(SI_DATE,'dd-MON-yyyy')SI_DATE, SI_NO, SUM(SO_QNTY) REQ_QNTY, ";
                //strQry += "(SELECT SM_NAME FROM TBLSTOREMAST WHERE SM_ID=SI_TO_STORE) SI_TO_STORE ,";
                //strQry += " (SELECT SM_NAME FROM TBLSTOREMAST WHERE SM_ID=SI_FROM_STORE) SI_FROM_STORE ";
                //strQry += "FROM TBLSTOREINDENT, TBLSINDENTOBJECTS WHERE SI_ID = SO_SI_ID and SI_TRANSFER_FLAG=0  ";
                //strQry += "AND (SELECT SM_OFF_CODE FROM TBLSTOREMAST WHERE SM_ID=SI_TO_STORE) like '" + sOfficeCode + "%' GROUP BY SI_ID,SI_DATE,SI_TO_STORE, SI_NO,SI_FROM_STORE)A,";
                //strQry += "(SELECT IS_SI_ID, COUNT(IO_CAPACITY) AS SENT_QNT,IS_NO ";
                //strQry += "FROM TBLSTOREINVOICE, TBLSINVOICEOBJECTS WHERE IS_ID = IO_IS_ID GROUP BY IS_SI_ID,IS_NO )B WHERE A.SI_ID= B.IS_SI_ID(+) AND B.IS_NO IS NULL ORDER BY SI_NO DESC";

                //dtIndentDetails=objCon.getDataTable(strQry);

                NpgsqlCommand cmd = new NpgsqlCommand("sp_loadinvoicegrid");
                cmd.Parameters.AddWithValue("offcode", sOfficeCode);
                dtIndentDetails = objcon.FetchDataTable(cmd);

                return dtIndentDetails;
           }
           catch (Exception ex)
           {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtIndentDetails;
           }
       }



       public DataTable LoadCompletedInvoiceGrid(string sOfficeCode)
       {
           string strQry = string.Empty;
           DataTable dtIndentDetails = new DataTable();
           try
           {
               //if (sOfficeCode.Length >= 3)
               //{
               //    sOfficeCode = sOfficeCode.Substring(0, Constants.Division);
               //}

                //strQry = "SELECT SI_ID, SI_DATE,SI_NO, REQ_QNTY, SI_FROM_STORE, (COALESCE(REQ_QNTY,0) - COALESCE(SENT_QNT,0)) AS PENDINGCOUNT,IS_NO  ";
                //strQry += "FROM (SELECT SI_ID,TO_CHAR(SI_DATE,'dd-MON-yyyy')SI_DATE, SI_NO, SUM(SO_QNTY) REQ_QNTY, ";
                //strQry += "(SELECT SM_NAME FROM TBLSTOREMAST WHERE SM_ID=SI_TO_STORE) SI_TO_STORE ,";
                //strQry += " (SELECT SM_NAME FROM TBLSTOREMAST WHERE SM_ID=SI_FROM_STORE) SI_FROM_STORE ";
                //strQry += "FROM TBLSTOREINDENT, TBLSINDENTOBJECTS WHERE SI_ID = SO_SI_ID and SI_TRANSFER_FLAG=1  ";
                //strQry += "AND (SELECT SM_OFF_CODE FROM TBLSTOREMAST WHERE SM_ID=SI_TO_STORE) like '" + sOfficeCode + "%' GROUP BY SI_ID,SI_DATE,SI_TO_STORE, SI_NO,SI_FROM_STORE)A,";
                //strQry += "(SELECT IS_SI_ID, COUNT(IO_CAPACITY) AS SENT_QNT,IS_NO ";
                //strQry += "FROM TBLSTOREINVOICE, TBLSINVOICEOBJECTS WHERE IS_ID = IO_IS_ID GROUP BY IS_SI_ID,IS_NO )B WHERE A.SI_ID= B.IS_SI_ID(+) ORDER BY SI_NO DESC";

                //dtIndentDetails = objCon.getDataTable(strQry);

                NpgsqlCommand cmd = new NpgsqlCommand("sp_loadcompletedinvoicegrid");
                cmd.Parameters.AddWithValue("offcode", sOfficeCode);
                dtIndentDetails = objcon.FetchDataTable(cmd);

                return dtIndentDetails;
           }
           catch (Exception ex)
           {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtIndentDetails;
           }
       }

       //Function to Populate indent grid values to textbox
       NpgsqlCommand NpgsqlCommand;
       public object LoadIndentDetails(clsStoreInvoice objInvoice)
       {
           NpgsqlCommand = new NpgsqlCommand();
           string strQry = string.Empty;
           string strIndentNo = string.Empty;
           DataTable dtIndentDetails = new DataTable();
           DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);

            try
            {

                if (objInvoice.sIndentNo != "")
                {
                    //NpgsqlCommand.Parameters.AddWithValue("IndentNo", objInvoice.sIndentNo);
                    //string strIndentNo = objcon.get_value("SELECT \"SI_NO\" FROM \"TBLSTOREINDENT\" WHERE \"SI_NO\" =:IndentNo", NpgsqlCommand);
                    NpgsqlCommand cmd1 = new NpgsqlCommand("proc_get_store_indent_value");
                    cmd1.Parameters.AddWithValue("indent_no", (objInvoice.sIndentNo ?? ""));
                    strIndentNo = objDatabse.StringGetValue(cmd1);

                    if (strIndentNo != "")
                    {
                    }
                    else
                    {
                        objInvoice.sIndentNo = "";
                        return objInvoice;
                    }
                }

                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_indent_details");
                cmd.Parameters.AddWithValue("indent_id", (objInvoice.sIndentId ?? ""));
                cmd.Parameters.AddWithValue("indent_no", (objInvoice.sIndentNo ?? ""));
                cmd.Parameters.AddWithValue("store_ind_no", (strIndentNo ?? ""));
                dtIndentDetails = objcon.FetchDataTable(cmd);


                //strQry = "SELECT \"SI_ID\",TO_CHAR(\"SI_DATE\",'dd/MM/yyyy')SI_DATE,\"SI_NO\",SUM(\"SO_QNTY\")SO_QNTY,\"SI_FROM_STORE\" FROM \"TBLSTOREINDENT\",\"TBLSINDENTOBJECTS\" where \"SI_ID\"=\"SO_SI_ID\"  ";
                //if (objInvoice.sIndentId != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("IndentId", Convert.ToInt32(objInvoice.sIndentId));
                //    strQry += " AND \"SI_ID\" =:IndentId";
                //}
                //if (objInvoice.sIndentNo != "")
                //{
                //    NpgsqlCommand.Parameters.AddWithValue("IndentNo", objInvoice.sIndentNo);
                //    strIndentNo = objcon.get_value("SELECT \"SI_NO\" FROM \"TBLSTOREINDENT\" WHERE \"SI_NO\" =:IndentNo", NpgsqlCommand);
                //    if (strIndentNo != "")
                //    {
                //        NpgsqlCommand.Parameters.AddWithValue("sIndentNo1", objInvoice.sIndentNo);
                //        strQry += " AND \"SI_NO\" =:sIndentNo1";
                //    }
                //    else
                //    {
                //        objInvoice.sIndentNo = "";
                //        return objInvoice;
                //    }
                //}
                //strQry += " GROUP BY \"SI_NO\",\"SI_ID\",\"SI_DATE\",\"SI_FROM_STORE\"";
                //dtIndentDetails = objcon.FetchDataTable(strQry, NpgsqlCommand);

                objInvoice.sIndentId = Convert.ToString(dtIndentDetails.Rows[0]["SI_ID"]);
               objInvoice.sIndentNo = Convert.ToString(dtIndentDetails.Rows[0]["SI_NO"]);
               objInvoice.sIndentDate = Convert.ToString(dtIndentDetails.Rows[0]["SI_DATE"]);
               objInvoice.sQuantity = Convert.ToString(dtIndentDetails.Rows[0]["SO_QNTY"]);
               objInvoice.sFromStoreId= Convert.ToString(dtIndentDetails.Rows[0]["SI_FROM_STORE"]);
               return objInvoice;
           }
           catch (Exception ex)
           {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objInvoice;
           }
       }
       //Function to load capacity grid 
       public DataTable LoadCapacityGrid(clsStoreInvoice objInvoice)
       {
           NpgsqlCommand = new NpgsqlCommand();
           string strQry = string.Empty;
           DataTable dtCapacityDetails = new DataTable();
           try
           {
                //NpgsqlCommand.Parameters.AddWithValue("IndentId",Convert.ToInt32(objInvoice.sIndentId));
                //strQry = "SELECT \"SI_ID\",\"CAPACITY\",\"REQ_QNTY\",(COALESCE(\"REQ_QNTY\",0) - COALESCE(\"SENT_QNT\",0)) AS PENDINGCOUNT  FROM"; 
                //strQry += " (SELECT \"SI_ID\",SUM(\"SO_QNTY\") \"REQ_QNTY\",\"SO_CAPACITY\" AS \"CAPACITY\" FROM \"TBLSTOREINDENT\", \"TBLSINDENTOBJECTS\" WHERE \"SI_ID\" = \"SO_SI_ID\" GROUP BY \"SI_ID\", \"SO_CAPACITY\")A LEFT OUTER JOIN";
                //strQry += " (SELECT \"IS_SI_ID\",\"IO_CAPACITY\",COUNT(\"IO_CAPACITY\") AS \"SENT_QNT\" FROM \"TBLSTOREINVOICE\", \"TBLSINVOICEOBJECTS\" WHERE \"IS_ID\" = \"IO_IS_ID\" GROUP BY \"IS_SI_ID\",\"IO_CAPACITY\")B";
                //strQry += " ON A.\"SI_ID\" = B.\"IS_SI_ID\" AND A.\"CAPACITY\" =B.\"IO_CAPACITY\" WHERE A.\"SI_ID\" =:IndentId";
                //dtCapacityDetails = objcon.FetchDataTable(strQry, NpgsqlCommand);
                //return dtCapacityDetails;

                NpgsqlCommand cmd = new NpgsqlCommand("proc_load_store_invoice_capacity_grid");
                cmd.Parameters.AddWithValue("indent_id", (objInvoice.sIndentId ?? ""));
                dtCapacityDetails = objcon.FetchDataTable(cmd);
                return dtCapacityDetails;

            }
           catch (Exception ex)
           {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtCapacityDetails;
           }
       }
       //Function to save invoice details
       public string[] SaveStoreInvoice(clsStoreInvoice objInvoice)
       {
           
           string strQry = string.Empty;
           string[] Arr = new string[3];
           OleDbDataReader dr;
           clsApproval objApproval = new clsApproval();
           DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);

            try
            {
               if (objInvoice.sInvoiceId == "")
               {
                   // NpgsqlCommand = new NpgsqlCommand();
                   // //To get the Store id of logged in user
                   // NpgsqlCommand.Parameters.AddWithValue("InvoiceNo", objInvoice.sInvoiceNo);
                   //string sNo = objcon.get_value("select \"IS_NO\" from \"TBLSTOREINVOICE\" where \"IS_NO\" =:InvoiceNo", NpgsqlCommand);
                   //if (sNo.Length > 0)
                   //{                       
                   //    Arr[0] = "Entered Invoice Number Already Exists";
                   //    Arr[1] = "2";
                   //    return Arr;
                   //}

                    NpgsqlCommand cmd2 = new NpgsqlCommand("proc_get_value_store_invoice_num");
                    cmd2.Parameters.AddWithValue("is_no", (objInvoice.sInvoiceNo ?? ""));

                    string sNo = objDatabse.StringGetValue(cmd2);
                    if (sNo.Length > 0)
                    {
                        Arr[0] = "Entered Invoice Number Already Exists";
                        Arr[1] = "2";
                        return Arr;
                    }


                    DataTable dtDoc = new DataTable();
                    dtDoc = objInvoice.dtDocuments;

                    
                    //objInvoice.sInvoiceId = Convert.ToString(objcon.Get_max_no("IS_ID", "TBLSTOREINVOICE"));

                    //NpgsqlCommand.Parameters.AddWithValue("InvoiceId", Convert.ToInt32(objInvoice.sInvoiceId));
                    //NpgsqlCommand.Parameters.AddWithValue("InvoiceNo1", objInvoice.sInvoiceNo);
                    //NpgsqlCommand.Parameters.AddWithValue("IndentId1", Convert.ToInt32(objInvoice.sIndentId));
                    //NpgsqlCommand.Parameters.AddWithValue("InvoiceDate", objInvoice.sInvoiceDate);
                    //NpgsqlCommand.Parameters.AddWithValue("Remarks", objInvoice.sRemarks);
                    //NpgsqlCommand.Parameters.AddWithValue("CrBy", Convert.ToInt32(objInvoice.sCrBy));
                    //NpgsqlCommand.Parameters.AddWithValue("dtDoc", Convert.ToString(dtDoc.Rows[0]["PATH"]));

                    //strQry = "INSERT INTO \"TBLSTOREINVOICE\" (\"IS_ID\",\"IS_NO\",\"IS_SI_ID\",\"IS_DATE\",\"IS_REMARKS\",\"IS_CRBY\",\"IS_CRON\",\"IS_FILE_PATH\") VALUES ";
                    //strQry += " (:InvoiceId,:InvoiceNo1,:IndentId1,";
                    //strQry += " to_date(:InvoiceDate,'dd/MM/yyyy'),:Remarks,:CrBy,NOW(),  :dtDoc)";
                    //objcon.ExecuteQry(strQry, NpgsqlCommand);


                    NpgsqlCommand cmd = new NpgsqlCommand("proc_save_store_invoice");
                    cmd.Parameters.AddWithValue("is_no", (objInvoice.sInvoiceNo ?? ""));
                    cmd.Parameters.AddWithValue("is_si_id", (objInvoice.sIndentId ?? ""));
                    cmd.Parameters.AddWithValue("is_date", (objInvoice.sInvoiceDate ?? ""));
                    cmd.Parameters.AddWithValue("is_remarks", (objInvoice.sRemarks ?? ""));
                    cmd.Parameters.AddWithValue("is_crby", (objInvoice.sCrBy ?? ""));
                    cmd.Parameters.AddWithValue("is_file_path", (dtDoc.Rows[0]["PATH"] ?? ""));
                    cmd.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd.Parameters.Add("pk_id", NpgsqlDbType.Text);
                    cmd.Parameters["msg"].Direction = ParameterDirection.Output;
                    cmd.Parameters["op_id"].Direction = ParameterDirection.Output;
                    cmd.Parameters["pk_id"].Direction = ParameterDirection.Output;
                    Arr[0] = "msg";
                    Arr[1] = "op_id";
                    Arr[2] = "pk_id";
                    Arr = objcon.Execute(cmd, Arr, 3);
                    objInvoice.sInvoiceId = Arr[2].ToString();



                    for (int i = 0; i < objInvoice.ddtTcGrid.Rows.Count; i++)
                   {
                        string[] Arr1 = new string[3];

                        //NpgsqlCommand = new NpgsqlCommand();

                        //objInvoice.sInvoiceObjectId = Convert.ToString(objcon.Get_max_no("IO_ID", "TBLSINVOICEOBJECTS"));

                        //NpgsqlCommand.Parameters.AddWithValue("InvoiceObjectId", Convert.ToInt32(objInvoice.sInvoiceObjectId));
                        //NpgsqlCommand.Parameters.AddWithValue("InvoiceId2", Convert.ToInt32(objInvoice.sInvoiceId));
                        //NpgsqlCommand.Parameters.AddWithValue("ddtTcGrid", Convert.ToDouble(objInvoice.ddtTcGrid.Rows[i]["TC_CAPACITY"]));
                        //NpgsqlCommand.Parameters.AddWithValue("ddtTcGrid1", Convert.ToString(objInvoice.ddtTcGrid.Rows[i]["TC_CODE"]));
                        //NpgsqlCommand.Parameters.AddWithValue("CrBy1", Convert.ToInt32(objInvoice.sCrBy));

                        //strQry = "INSERT INTO \"TBLSINVOICEOBJECTS\"(\"IO_ID\",\"IO_IS_ID\",\"IO_CAPACITY\",\"IO_TCCODE\",\"IO_CRBY\",\"IO_CRON\") VALUES ";
                        //strQry += " (:InvoiceObjectId,:InvoiceId2,";
                        //strQry += " :ddtTcGrid,:ddtTcGrid1,";
                        //strQry += " :CrBy1,NOW())";
                        //objcon.ExecuteQry(strQry, NpgsqlCommand);

                        //NpgsqlCommand.Parameters.AddWithValue("ddtTcGrid2", Convert.ToString(ddtTcGrid.Rows[i]["TC_CODE"]));
                        //strQry = "Update \"TBLTCMASTER\" SET \"TC_CURRENT_LOCATION\" =4 WHERE \"TC_CODE\" =:ddtTcGrid2";
                        //objcon.ExecuteQry(strQry, NpgsqlCommand);


                        NpgsqlCommand cmd1 = new NpgsqlCommand("proc_save_store_invoice_objects");
                        cmd1.Parameters.AddWithValue("io_is_id", Convert.ToString(objInvoice.sInvoiceId ?? ""));
                        cmd1.Parameters.AddWithValue("io_capacity", Convert.ToString(objInvoice.ddtTcGrid.Rows[i]["TC_CAPACITY"] ?? ""));
                        cmd1.Parameters.AddWithValue("io_tccode", Convert.ToString(objInvoice.ddtTcGrid.Rows[i]["TC_CODE"] ?? ""));
                        cmd1.Parameters.AddWithValue("io_crby", Convert.ToString(objInvoice.sCrBy ?? ""));
                        //cmd1.Parameters.AddWithValue("tc_code", (ddtTcGrid.Rows[i]["TC_CODE"] ?? ""));
                        cmd1.Parameters.AddWithValue("tc_code", Convert.ToString(objInvoice.ddtTcGrid.Rows[i]["TC_CODE"] ?? ""));

                        cmd1.Parameters.Add("msg", NpgsqlDbType.Text);
                        cmd1.Parameters.Add("op_id", NpgsqlDbType.Text);
                        cmd1.Parameters.Add("pk_id", NpgsqlDbType.Text);
                        cmd1.Parameters["msg"].Direction = ParameterDirection.Output;
                        cmd1.Parameters["op_id"].Direction = ParameterDirection.Output;
                        cmd1.Parameters["pk_id"].Direction = ParameterDirection.Output;
                        Arr1[0] = "msg";
                        Arr1[1] = "op_id";
                        Arr1[2] = "pk_id";
                        Arr1 = objcon.Execute(cmd1, Arr1, 3);
                    }

                   UpdateIndentStatus(objInvoice);


                   #region WorkFlow

                   //Workflow / Approval
                   
                   objApproval.sFormName = objInvoice.sFormName;
                   objApproval.sRecordId = objInvoice.sRecordId;
                   objApproval.sNewRecordId = objInvoice.sInvoiceId;
                   objApproval.sOfficeCode = objInvoice.sRefOfficeCode;
                   objApproval.sClientIp = objInvoice.sClientIP;
                   objApproval.sCrby = objInvoice.sCrBy;
                   objApproval.sWFObjectId = objInvoice.sWFOId;
                   
                  // objApproval.sRefOfficeCode = objcon.get_value("SELECT \"STO_OFF_CODE\" FROM \"TBLSTOREOFFCODE\" WHERE \"STO_SM_ID\" ='" + objInvoice.sOfficeCode + "'");

                   objApproval.sRefOfficeCode = objInvoice.sRefOfficeCode;

                   objApproval.sDescription = "Store Invoice Creation for Indent No " + objInvoice.sIndentNo;
                   objApproval.sApproveStatus = "1";
                   objApproval.sApproveComments = "Approved";
                   objApproval.ApproveWFRequest(objApproval);

                   #endregion

                   Arr[0] = "Saved Successfully";
                   Arr[1] = "0";
                   //return Arr;
               }
                return Arr;

                #region Update Store Inovoice

                //else
                //{
                //     NpgsqlCommand = new NpgsqlCommand();
                //     objcon.BeginTransaction();
                //     NpgsqlCommand.Parameters.AddWithValue("InvoiceNo2", objInvoice.sInvoiceNo);
                //     NpgsqlCommand.Parameters.AddWithValue("InvoiceId3", Convert.ToInt32(objInvoice.sInvoiceId));
                //     string sNo = objcon.get_value("select \"IS_NO\" from \"TBLSTOREINVOICE\" where \"IS_NO\" =:InvoiceNo2 and \"IS_ID\" <>:InvoiceId3", NpgsqlCommand);
                //     if (sNo.Length > 0)
                //     {
                //         Arr[0] = "Entered Invoice Number Already Exists";
                //         Arr[1] = "2";
                //         return Arr;
                //     }

                //     NpgsqlCommand.Parameters.AddWithValue("InvoiceNo3", objInvoice.sInvoiceNo);
                //     NpgsqlCommand.Parameters.AddWithValue("Remarks1", objInvoice.sRemarks);
                //     NpgsqlCommand.Parameters.AddWithValue("InvoiceDate1", objInvoice.sInvoiceDate);
                //     NpgsqlCommand.Parameters.AddWithValue("InvoiceId4", Convert.ToInt32(objInvoice.sInvoiceId));
                //     strQry = "UPDATE \"TBLSTOREINVOICE\" SET \"IS_NO\" =:InvoiceNo3, \"IS_REMARKS\" =:Remarks1, \"IS_DATE\" =to_date(:InvoiceDate1,'dd/MM/yyyy') WHERE \"IS_ID\" =:InvoiceId4";
                //     objcon.ExecuteQry(strQry, NpgsqlCommand);
                //     //deleting old records
                //     NpgsqlCommand.Parameters.AddWithValue("InvoiceId5", Convert.ToInt32(objInvoice.sInvoiceId));
                //     strQry = "DELETE FROM \"TBLSINVOICEOBJECTS\" WHERE \"IO_IS_ID\" =:InvoiceId5";
                //     objcon.ExecuteQry(strQry, NpgsqlCommand);
                //     for (int i = 0; i < objInvoice.ddtTcGrid.Rows.Count; i++)
                //     {
                //         NpgsqlCommand = new NpgsqlCommand();
                //         //    objInvoice.sInvoiceObjectId = Convert.ToString(objcon.Get_max_no("\"IO_ID\"","\"TBLSINVOICEOBJECTS\""));
                //         NpgsqlCommand.Parameters.AddWithValue("InvoiceObjectId1", Convert.ToInt32(objInvoice.sInvoiceObjectId));
                //         NpgsqlCommand.Parameters.AddWithValue("InvoiceId5", Convert.ToInt32(objInvoice.sInvoiceId));
                //         NpgsqlCommand.Parameters.AddWithValue("ddtTcGrid3", Convert.ToDouble(objInvoice.ddtTcGrid.Rows[i]["TC_CAPACITY"]));
                //         NpgsqlCommand.Parameters.AddWithValue("ddtTcGrid4", Convert.ToString(objInvoice.ddtTcGrid.Rows[i]["TC_CODE"]));
                //         NpgsqlCommand.Parameters.AddWithValue("CrBy2", Convert.ToInt32(objInvoice.sCrBy));

                //         //strQry = "INSERT INTO \"TBLSINVOICEOBJECTS\"(\"IO_ID\",\"IO_IS_ID\",\"IO_CAPACITY\",\"IO_TCCODE\",\"IO_CRBY\",\"IO_CRON\") VALUES(:InvoiceObjectId1,:InvoiceId5,:ddtTcGrid3,:ddtTcGrid4,:CrBy2,NOW())";
                //         strQry = "INSERT INTO \"TBLSINVOICEOBJECTS\"(\"IO_ID\",\"IO_IS_ID\",\"IO_CAPACITY\",\"IO_TCCODE\",\"IO_CRBY\",\"IO_CRON\") VALUES((SELECT MAX(\"IO_ID\")+1 FROM \"TBLSINVOICEOBJECTS\"),:InvoiceId5,:ddtTcGrid3,:ddtTcGrid4,:CrBy2,NOW())";
                //         objcon.ExecuteQry(strQry, NpgsqlCommand);

                //         NpgsqlCommand.Parameters.AddWithValue("ddtTcGrid5", Convert.ToString(ddtTcGrid.Rows[i]["TC_CODE"]));
                //         strQry = "Update \"TBLTCMASTER\" SET \"TC_CURRENT_LOCATION\"=4 WHERE \"TC_CODE\" =:ddtTcGrid5 AND \"TC_CURRENT_LOCATION\" <>4";
                //         objcon.ExecuteQry(strQry, NpgsqlCommand);

                //     }
                //     UpdateIndentStatus(objInvoice);

                //     objcon.CommitTransaction();
                //     Arr[0] = "Updated Successfully";
                //     Arr[1] = "1";
                //     return Arr;


                // }
                #endregion


            }
            catch (Exception ex)
           {
               objcon.RollBackTrans();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
           }
       }
       public void UpdateIndentStatus(clsStoreInvoice objInvoice)
       {
           NpgsqlCommand = new NpgsqlCommand();
           string strQry = string.Empty;
           DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);
           string[] Arr1 = new string[3];

            try
            {
               //NpgsqlCommand.Parameters.AddWithValue("IndentId",Convert.ToInt32(objInvoice.sIndentId));
               //string strInvoiceCount = objcon.get_value("SELECT COUNT(\"IO_CAPACITY\")AS Count FROM \"TBLSINVOICEOBJECTS\",\"TBLSTOREINVOICE\" WHERE \"IO_IS_ID\"=\"IS_ID\" AND \"IS_SI_ID\" =:IndentId ", NpgsqlCommand);

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_value_of_count_invoceobject_capacity");

                cmd.Parameters.AddWithValue("indent_id", (objInvoice.sIndentId ?? ""));
                string strInvoiceCount = objDatabse.StringGetValue(cmd);

                //NpgsqlCommand.Parameters.AddWithValue("IndentId1", Convert.ToInt32(objInvoice.sIndentId));
                //strQry = "UPDATE \"TBLSTOREINDENT\" SET \"SI_TRANSFER_FLAG\" =1 WHERE \"SI_ID\" =:IndentId1";
                //objcon.ExecuteQry(strQry, NpgsqlCommand);

                NpgsqlCommand cmd1 = new NpgsqlCommand("proc_update_transfer_flag_tblstoreindent");
                cmd1.Parameters.AddWithValue("indent_id", Convert.ToString(objInvoice.sIndentId ?? ""));
                cmd1.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd1.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd1.Parameters.Add("pk_id", NpgsqlDbType.Text);
                cmd1.Parameters["msg"].Direction = ParameterDirection.Output;
                cmd1.Parameters["op_id"].Direction = ParameterDirection.Output;
                cmd1.Parameters["pk_id"].Direction = ParameterDirection.Output;
                Arr1[0] = "msg";
                Arr1[1] = "op_id";
                Arr1[2] = "pk_id";
                Arr1 = objcon.Execute(cmd1, Arr1, 3);
            }
           catch (Exception ex)
           {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
       }


       //public object CheckTc(clsStoreInvoice objInvoice)
       //{
       //    NpgsqlCommand = new NpgsqlCommand();
       //    string strQry = string.Empty;
       //    try
       //    {
       //        NpgsqlCommand.Parameters.AddWithValue("TcCode",Convert.ToDouble(objInvoice.sTcCode));
       //        string strTcCode = objcon.get_value("Select \"TC_CODE\" FROM \"TBLTCMASTER\" WHERE \"TC_CODE\" =:TcCode", NpgsqlCommand);
       //        if (strTcCode == "")
       //       {
       //           return objInvoice.sTcCode = "";
       //       }
       //       else
       //       {
       //           objInvoice.sTcCode = strTcCode;
       //           return objInvoice.sTcCode;
       //       }

       //    }
       //     catch (Exception ex)
       //     {
       //         clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
       //         return objInvoice;
       //     }
       //}


        //Function to Load Tc Details Grid
        public object LoadTcDetails(clsStoreInvoice objInvoice)
       {
           NpgsqlCommand = new NpgsqlCommand();
            string strQry = string.Empty;
            DataTable dtTcDetails = new DataTable();
            DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);

            try
            {
                //NpgsqlCommand.Parameters.AddWithValue("TcCode",Convert.ToString(objInvoice.sTcCode));
                //string strTcCode = objcon.get_value("SELECT \"IO_TCCODE\" FROM \"TBLSINVOICEOBJECTS\",\"TBLSTOREINVOICE\" WHERE \"IO_TCCODE\" =:TcCode AND \"IS_APPROVE_FLAG\" ='0' AND \"IO_IS_ID\"=\"IS_ID\"", NpgsqlCommand);

                NpgsqlCommand cmd = new NpgsqlCommand("proc_get_value_of_tc_invoceobject");
                cmd.Parameters.AddWithValue("tc_code", (objInvoice.sTcCode ?? ""));
                string strTcCode = objDatabse.StringGetValue(cmd);

                if (strTcCode != "")
                {
                    objInvoice.sTcCode = "";
                }
                else
                {
                    //NpgsqlCommand.Parameters.AddWithValue("TcCode1", Convert.ToString(objInvoice.sTcCode));
                    //NpgsqlCommand.Parameters.AddWithValue("OfficeCode", Convert.ToInt32(objInvoice.sOfficeCode));
                    //NpgsqlCommand.Parameters.AddWithValue("IndentId", Convert.ToInt32(objInvoice.sIndentId));
                    //strQry = "SELECT \"TC_ID\",\"TC_SLNO\",\"TC_CODE\",CAST(\"TC_CAPACITY\" AS TEXT)TC_CAPACITY,(SELECT \"TM_NAME\" FROM \"TBLTRANSMAKES\" WHERE ";
                    //strQry += " \"TM_ID\" = \"TC_MAKE_ID\" )TM_NAME FROM \"TBLTCMASTER\" WHERE \"TC_CODE\" =:TcCode1 AND ";
                    //strQry += " \"TC_CURRENT_LOCATION\" =1 AND \"TC_STORE_ID\" = :OfficeCode AND \"TC_CAPACITY\" IN ";
                    //strQry += "(SELECT \"SO_CAPACITY\" FROM \"TBLSINDENTOBJECTS\" WHERE \"SO_SI_ID\" =:IndentId)";

                    //dtTcDetails = objcon.FetchDataTable(strQry, NpgsqlCommand);

                    NpgsqlCommand cmd1 = new NpgsqlCommand("sp_load_store_inv_tc_details");
                    cmd1.Parameters.AddWithValue("tc_code1", (objInvoice.sTcCode ?? ""));
                    cmd1.Parameters.AddWithValue("office_code", (objInvoice.sOfficeCode ?? ""));
                    cmd1.Parameters.AddWithValue("indent_id", (objInvoice.sIndentId ?? ""));
                    dtTcDetails = objcon.FetchDataTable(cmd1);

                    if (dtTcDetails.Rows.Count > 0)
                    {
                        objInvoice.sTcId = Convert.ToString(dtTcDetails.Rows[0]["TC_ID"]);
                        objInvoice.sTcSlNo = Convert.ToString(dtTcDetails.Rows[0]["TC_SLNO"]);
                        objInvoice.sTcCode = Convert.ToString(dtTcDetails.Rows[0]["TC_CODE"]);
                        objInvoice.sTcCapacity = Convert.ToString(dtTcDetails.Rows[0]["TC_CAPACITY"]);
                        objInvoice.sTcName = Convert.ToString(dtTcDetails.Rows[0]["TM_NAME"]);
                    }
                    else
                    {
                        objInvoice.sIndentId = "";
                        objInvoice.sTcId = "";
                    }
                    return objInvoice;
                }
                return objInvoice;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objInvoice;

            }
        }

        public void UpdateDeleteItem(clsStoreInvoice objInvoice)
        {
            NpgsqlCommand = new NpgsqlCommand();
            string strQry = string.Empty;
            string[] Arr1 = new string[3];

            try
            {
               //NpgsqlCommand.Parameters.AddWithValue("TcCode",Convert.ToString(objInvoice.sTcCode));
               //strQry = "UPDATE \"TBLTCMASTER\" SET \"TC_CURRENT_LOCATION\" =1 WHERE \"TC_CODE\"=:TcCode";
               //objcon.ExecuteQry(strQry, NpgsqlCommand);


                NpgsqlCommand cmd1 = new NpgsqlCommand("proc_update_delete_curloc_tcmaster");
                cmd1.Parameters.AddWithValue("tc_code", Convert.ToString(objInvoice.sTcCode ?? ""));
                cmd1.Parameters.Add("msg", NpgsqlDbType.Text);
                cmd1.Parameters.Add("op_id", NpgsqlDbType.Text);
                cmd1.Parameters.Add("pk_id", NpgsqlDbType.Text);
                cmd1.Parameters["msg"].Direction = ParameterDirection.Output;
                cmd1.Parameters["op_id"].Direction = ParameterDirection.Output;
                cmd1.Parameters["pk_id"].Direction = ParameterDirection.Output;
                Arr1[0] = "msg";
                Arr1[1] = "op_id";
                Arr1[2] = "pk_id";
                Arr1 = objcon.Execute(cmd1, Arr1, 3);
            }
           catch (Exception ex)
           {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
       }

       public DataTable LoadDtrDetails(clsStoreInvoice objInvoice)
        {
            NpgsqlCommand = new NpgsqlCommand();
           string strQry = string.Empty;
           DataTable dtBasicDetails = new DataTable();
           try
           {
               //strQry = "select \"TC_CODE\",CAST(\"TC_CAPACITY\" AS TEXT)TC_CAPACITY,\"TC_SLNO\",\"TC_MAKE_ID\",\"IS_NO\",TO_CHAR(\"IS_DATE\",'dd/MM/yyyy')IS_DATE,(select \"TM_NAME\" from \"TBLTRANSMAKES\"  where \"TM_ID\"=\"TC_MAKE_ID\") as Make ";
               //strQry += "from \"TBLSINVOICEOBJECTS\",\"TBLTCMASTER\",\"TBLSTOREINVOICE\" where \"IO_TCCODE\"=\"TC_CODE\" and \"IS_ID\"=\"IO_IS_ID\"  ";
               //if (objInvoice.sIndentId != "")
               //{
               //    NpgsqlCommand.Parameters.AddWithValue("IndentId",Convert.ToInt32(objInvoice.sIndentId));
               //    strQry += " AND \"IS_SI_ID\" =:IndentId";
               //}
               //dtBasicDetails = objcon.FetchDataTable(strQry, NpgsqlCommand);
               //return dtBasicDetails;


                NpgsqlCommand cmd1 = new NpgsqlCommand("proc_load_store_invoice_dtr_details");
                cmd1.Parameters.AddWithValue("indent_id", (objInvoice.sIndentId ?? ""));
                dtBasicDetails = objcon.FetchDataTable(cmd1);
                return dtBasicDetails;
            }
            catch (Exception ex)
           {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtBasicDetails;
           }
       }

       public clsStoreInvoice GetStoreInvoiceDetails(clsStoreInvoice objInvoice)
       {
           NpgsqlCommand = new NpgsqlCommand();
           try
           {
               //string strQry = string.Empty;
               DataTable dt = new DataTable();
               //NpgsqlCommand.Parameters.AddWithValue("IndentId",Convert.ToInt32(objInvoice.sIndentId));
               //strQry = "SELECT \"IS_NO\",TO_CHAR(\"IS_DATE\",'DD/MM/YYYY') IS_DATE,\"IS_REMARKS\" FROM \"TBLSTOREINVOICE\" WHERE \"IS_SI_ID\" =:IndentId";
               //dt = objcon.FetchDataTable(strQry, NpgsqlCommand);

                NpgsqlCommand cmd1 = new NpgsqlCommand("proc_get_store_inv_details");
                cmd1.Parameters.AddWithValue("indent_id", (objInvoice.sIndentId ?? ""));
                dt = objcon.FetchDataTable(cmd1);

                if (dt.Rows.Count > 0)
               {
                   objInvoice.sInvoiceNo = Convert.ToString(dt.Rows[0]["IS_NO"]);
                   objInvoice.sInvoiceDate = Convert.ToString(dt.Rows[0]["IS_DATE"]);
                   objInvoice.sRemarks = Convert.ToString(dt.Rows[0]["IS_REMARKS"]);
                  
               }
               return objInvoice;
           }
           catch (Exception ex)
           {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objInvoice;
           }
       }

       public string[] SaveWFOdata(clsStoreInvoice objInvoice)
       {

           string[] Arr = new string[2];
           try
           {
              
               string strQry = string.Empty;
               StringBuilder sbQuery = new StringBuilder();

               clsApproval objApproval = new clsApproval();
               DataTable dtDoc = new DataTable();
               dtDoc = objInvoice.dtDocuments;
               objApproval.sCrby = objInvoice.sCrby;

              // string sPrimaryKey = "{0}";

               objApproval.sColumnNames = "NAME,PATH";
               objApproval.sColumnValues = "" + Convert.ToString(dtDoc.Rows[0]["NAME"]) + ", " + Convert.ToString(dtDoc.Rows[0]["PATH"]) + "";
               objApproval.sTableNames = "TBLSTOREINVOICE";
                objApproval.sBOId = "23";

               objApproval.SaveWorkFlowData(objApproval);
               objInvoice.sWFDataId = objApproval.sWFDataId;


               return Arr;
           }
           catch (Exception ex)
           {
               objcon.RollBackTrans();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
           }
       }

       public DataTable GetMafFilePath(string sWFO_ID)
       {
           DataTable dt = new DataTable();
           try
           {
               clsApproval objApproval = new clsApproval();
               dt = objApproval.GetDatatableFromXML(sWFO_ID);
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
