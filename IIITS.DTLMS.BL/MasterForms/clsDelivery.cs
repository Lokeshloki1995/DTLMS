using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IIITS.PGSQL.DAL;
using System.Data;
using Npgsql;
using System.Configuration;
using NpgsqlTypes;

namespace IIITS.DTLMS.BL
{

    public class clsDelivery
    {
        public string TotalTC { get; set; }
        public string PoId { get; set; }
        public string DIid { get; set; }
        public string DINo { get; set; }
        public string PONumber { get; set; }
        public DataTable dtDelivery { get; set; }
        public Byte[] POFile { get; set; }
        public string Capacity { get; set; }
        public string TcMake { get; set; }
        public string Store { get; set; }
        public string FileExt { get; set; }
        public string Crby { get; set; }

        public string strFormCode = "clsDelivery";

        PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
        NpgsqlCommand NpgsqlCommand;

        public Object GetPurchaseCount(clsDelivery obj)
        {
            DataTable dt = new DataTable();
            try
            {
                string sQry = string.Empty;
                NpgsqlCommand cmd = new NpgsqlCommand("public.sp_get_purchasecount");
                cmd.Parameters.AddWithValue("phno", obj.PONumber);
                dt = objcon.FetchDataTable(cmd);

                if (dt.Rows.Count > 0 )
                {
                    int intValue = Convert.ToInt32(dt.Rows[0][0]);
                    
                    obj.TotalTC = intValue.ToString();                  
                }
                else
                {
                   
                    obj.TotalTC = "0"; 
                }

                return obj.TotalTC;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "0"; 
            }
        }

        public string GetPOId(clsDelivery obj)
        {
            DataTable dt = new DataTable();

            try
            {
                string sQry = string.Empty;
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getpoid");
                cmd.Parameters.AddWithValue("phno", obj.PONumber);
                dt = objcon.FetchDataTable(cmd);

                // Check if the DataTable has rows and contains at least one column
                if (dt.Rows.Count > 0 && dt.Columns.Count > 0)
                {
                    // Assuming the value is in the first row and the first column
                    obj.PoId = dt.Rows[0][0].ToString();
                }
                else
                {
                    // Handle the case when there's no data in the DataTable
                    obj.PoId = "0"; // or any default string value you prefer
                }

                return obj.PoId;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "0";
            }
        }

        public DataTable GetAllotmentCount(clsDelivery objDelivery)
        {
            NpgsqlCommand = new NpgsqlCommand();
            DataTable dtIndentDetails = new DataTable();
            try
            {
                // string strQry = string.Empty;
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getallotmentcount");
                cmd.Parameters.AddWithValue("alt_capacity", Convert.ToString(objDelivery.Capacity));
                cmd.Parameters.AddWithValue("alt_make_id", Convert.ToString(objDelivery.TcMake));
                cmd.Parameters.AddWithValue("alt_store_id", Convert.ToString(objDelivery.Store));
                dtIndentDetails = objcon.FetchDataTable(cmd);
                return dtIndentDetails;

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtIndentDetails;
            }

        }
        //public DataTable GetDIDetails(clsDelivery objDelivery)
        //{
        //    PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
        //    NpgsqlCommand = new NpgsqlCommand();
        //    DataTable dtIndentDetails = new DataTable();
        //    try
        //    {
        //        string strQry = string.Empty;
        //        NpgsqlCommand.Parameters.AddWithValue("DINo", Convert.ToString(objDelivery.DINo));
        //        strQry = "select \"DI_ID\",\"DI_NO\",\"DI_CONSIGNEE\",TO_CHAR(\"DI_DUEDATE\",'dd-MM-yyyy')\"DI_DUEDATE\",\"DI_STORE_ID\",TO_CHAR(\"DI_DATE\",'dd-MM-yyyy') ";
        //        strQry+=" \"DI_DATE\",\"DI_MAKE_ID\", \"DI_CAPACITY\",\"DI_CAPACITY_ID\", \"DI_STARTTYPE\", ";
        //        strQry += "\"DI_QUANTITY\"  from \"TBLDELIVERYINSTRUCTION\" WHERE  \"DI_NO\"=:DINo ";


        //        dtIndentDetails = objcon.FetchDataTable(strQry, NpgsqlCommand);

        //        return dtIndentDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return dtIndentDetails;
        //    }
        //}
        //to get DI details for grid
        public DataTable GetDeliveredDetails(string sDI_no)
        {
            NpgsqlCommand = new NpgsqlCommand();
            DataTable dt = new DataTable();
            try
            {
                string sQry = string.Empty;
                NpgsqlCommand cmd = new NpgsqlCommand("sp_get_deliveredetails");
                cmd.Parameters.AddWithValue("di_no", sDI_no);
                dt = objcon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }


        public DataTable GetDeliveryDetails(clsDelivery objDel)
        {
            NpgsqlCommand = new NpgsqlCommand();
            DataTable dt = new DataTable();
            try
            {
              
                NpgsqlCommand cmd = new NpgsqlCommand("sp_get_deliverydetails");

                cmd.Parameters.AddWithValue("poid", objDel.PoId == null ? "" : objDel.PoId);
                cmd.Parameters.AddWithValue("po_no", objDel.PONumber == null ? "" : objDel.PONumber);
                cmd.Parameters.AddWithValue("capcity", objDel.Capacity == null ? "" : objDel.Capacity);

                dt = objcon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        // to save DI Details 
        public string[] SaveDeliveryDetails(clsDelivery objdelivery)
        {
            PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
            NpgsqlCommand = new NpgsqlCommand();
            string[] Arr = new string[3];
            DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);

         
            try
            {
                string sQry = string.Empty;
                DataTable dt = new DataTable();
                dt = objdelivery.dtDelivery;
                string str;

                for (int i = 0; i <dt.Rows.Count; i++)
                {
                    string sDi_Id = Convert.ToString(dt.Rows[i]["DI_ID"]);
                    string sDiNo = Convert.ToString(dt.Rows[i]["DIM_DI_NO"]);
                    string sPoNo = Convert.ToString(dt.Rows[i]["DI_PO_ID"]);
                    string sStoreId = Convert.ToString(dt.Rows[i]["DI_STORE_ID"]);
                    string sDueDate = Convert.ToString(dt.Rows[i]["DI_DUEDATE"]);
                    string sMake = Convert.ToString(dt.Rows[i]["DI_MAKE_ID"]);
                    string sCapacity = Convert.ToString(dt.Rows[i]["DI_CAPACITY"]);
                    string sCapacityID = Convert.ToString(dt.Rows[i]["DI_CAPACITY_ID"]);
                    string sStartype = Convert.ToString(dt.Rows[i]["DI_STARRATE"]);
                    string sQuantity = Convert.ToString(dt.Rows[i]["DI_QUANTITY"]);
                    string sConsignee = "0";
                    string sDiDate = Convert.ToString(dt.Rows[i]["DI_DATE"]);
                    string sFileExt = Convert.ToString(dt.Rows[i]["DI_FILE_EXT"]);

                    string sStartRange = Convert.ToString(dt.Rows[i]["DI_START_RANGE"]);
                    string sEndRange = Convert.ToString(dt.Rows[i]["DI_END_RANGE"]);
                    long Id = objcon.Get_max_no("DI_ID", "TBLDELIVERYINSTRUCTION");

                    if (sDi_Id == "")
                    {
                        //NpgsqlCommand Npgsql = new NpgsqlCommand();
                        //Npgsql.Parameters.AddWithValue("DI_no", sDiNo.ToUpper());
                        //Npgsql.Parameters.AddWithValue("poId", sPoNo.ToUpper());
                        //str = objcon.get_value("select * from \"TBLDELIVERYINSTMASTER\" where cast(\"DIM_DI_NO\"as text)=:DI_no  AND cast(\"DIM_PO_ID\" as text)<> :poId", Npgsql);
                        NpgsqlCommand Cmd = new NpgsqlCommand("proc_savedi");
                        Cmd.Parameters.AddWithValue("dim_dino", sDiNo.ToUpper());
                        Cmd.Parameters.AddWithValue("dim_poid", sPoNo.ToUpper());
                       DataTable dt1 = objcon.FetchDataTable(Cmd);
                        //str = objDatabse.StringGetValue(Cmd);


                        if (dt1.Rows.Count > 0)
                        {
                            Arr[0] = "Entered DI Number Already mapped with some other PO Number";
                            Arr[1] = "2";
                            return Arr;
                        }


                        //NpgsqlCommand cmd1 = new NpgsqlCommand("sp_check_di_number_exists");
                        //cmd1.Parameters.AddWithValue("di_no", sDiNo.ToUpper());
                        //cmd1.Parameters.AddWithValue("po_id", sPoNo.ToUpper());
                        //dt1 = Objcon.FetchDataTable(cmd1);
                        //if (dt1.Rows.Count > 0)
                        //{
                        //    Arr[0] = "Entered DI Number Already mapped with some other PO Number";
                        //    Arr[1] = "2";
                        //    return Arr;
                        NpgsqlCommand cmd = new NpgsqlCommand("proc_saveupdate_dimaster_new");
                        cmd.Parameters.AddWithValue("di_id", sDi_Id);
                        cmd.Parameters.AddWithValue("di_number", sDiNo.ToUpper());
                        cmd.Parameters.AddWithValue("di_consignee", sConsignee);
                        cmd.Parameters.AddWithValue("di_po_id", sPoNo);
                        cmd.Parameters.AddWithValue("di_crby", objdelivery.Crby);
                        cmd.Parameters.AddWithValue("di_date", sDiDate);
                        cmd.Parameters.AddWithValue("di_store_id", sStoreId);
                        cmd.Parameters.AddWithValue("di_star_rate", sStartype);
                        cmd.Parameters.AddWithValue("di_duedate", sDueDate);
                        cmd.Parameters.AddWithValue("di_make_id", sMake);
                        cmd.Parameters.AddWithValue("di_quantity", sQuantity);
                        cmd.Parameters.AddWithValue("di_capacity", sCapacity);
                        cmd.Parameters.AddWithValue("di_capacity_id", sCapacityID);
                        cmd.Parameters.AddWithValue("di_status", "1");

                        cmd.Parameters.AddWithValue("di_start_range", sStartRange);
                        cmd.Parameters.AddWithValue("di_end_range", sEndRange);

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
                        objdelivery.DIid = Arr[2].ToString();

                    }
                    else
                    {
                        //NpgsqlCommand.Parameters.AddWithValue("DiNO", sDiNo.ToUpper());
                        //str = "update \"tbldeliveryinstmaster\" set \"dim_status\"=0 where \"dim_di_no\"=:dino";

                        //objcon.ExecuteQry(str, NpgsqlCommand);
                        NpgsqlCommand = new NpgsqlCommand("proc_update_dimaster_new");
                        NpgsqlCommand.Parameters.AddWithValue("dino", DINo);
                       objcon.Execute(NpgsqlCommand,Arr,0);

                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            sDi_Id = Convert.ToString(dt.Rows[j]["DI_ID"]);
                            sDiNo = Convert.ToString(dt.Rows[j]["DIM_DI_NO"]);
                            sConsignee = "0";
                            sPoNo = Convert.ToString(dt.Rows[j]["DI_PO_ID"]);
                            sStoreId = Convert.ToString(dt.Rows[j]["DI_STORE_ID"]);
                            sDueDate = Convert.ToString(dt.Rows[j]["DI_DUEDATE"]);
                            sMake = Convert.ToString(dt.Rows[j]["DI_MAKE_ID"]);
                            sCapacity = Convert.ToString(dt.Rows[j]["DI_CAPACITY"]);
                            sCapacityID = Convert.ToString(dt.Rows[j]["DI_CAPACITY_ID"]);
                            sStartype = Convert.ToString(dt.Rows[j]["DI_STARRATE"]);
                            sQuantity = Convert.ToString(dt.Rows[j]["DI_QUANTITY"]);
                            sDiDate = Convert.ToString(dt.Rows[j]["DI_DATE"]);
                            sFileExt = Convert.ToString(dt.Rows[j]["DI_FILE_EXT"]);
                            sStartRange = Convert.ToString(dt.Rows[i]["DI_START_RANGE"]);
                            sEndRange = Convert.ToString(dt.Rows[i]["DI_END_RANGE"]);

                            NpgsqlCommand cmd1 = new NpgsqlCommand("proc_saveupdate_dimaster_new");
                            cmd1.Parameters.AddWithValue("di_id", sDi_Id);
                            cmd1.Parameters.AddWithValue("di_number", sDiNo.ToUpper());
                            cmd1.Parameters.AddWithValue("di_consignee", sConsignee);
                            cmd1.Parameters.AddWithValue("di_po_id", sPoNo);
                            cmd1.Parameters.AddWithValue("di_crby", objdelivery.Crby);
                            cmd1.Parameters.AddWithValue("di_date", sDiDate);
                            cmd1.Parameters.AddWithValue("di_store_id", sStoreId);
                            cmd1.Parameters.AddWithValue("di_star_rate", sStartype);
                            //if(sDueDate == null || sDueDate == "0")
                            //    if (Obj.LocationCode == null || Obj.LocationCode == "0") Obj.LocationCode = "";
                            cmd1.Parameters.AddWithValue("di_duedate", sDueDate);
                            cmd1.Parameters.AddWithValue("di_make_id", sMake);
                            cmd1.Parameters.AddWithValue("di_quantity", sQuantity);
                            cmd1.Parameters.AddWithValue("di_capacity", sCapacity);
                            cmd1.Parameters.AddWithValue("di_capacity_id", sCapacityID);
                            cmd1.Parameters.AddWithValue("di_status", "1");
                            cmd1.Parameters.AddWithValue("di_start_range", sStartRange);
                            cmd1.Parameters.AddWithValue("di_end_range", sEndRange);
                            cmd1.Parameters.Add("msg", NpgsqlDbType.Text);
                            cmd1.Parameters.Add("op_id", NpgsqlDbType.Text);
                            cmd1.Parameters.Add("pk_id", NpgsqlDbType.Text);

                            cmd1.Parameters["msg"].Direction = ParameterDirection.Output;
                            cmd1.Parameters["op_id"].Direction = ParameterDirection.Output;
                            cmd1.Parameters["pk_id"].Direction = ParameterDirection.Output;
                            Arr[0] = "msg";
                            Arr[1] = "op_id";
                            Arr[2] = "pk_id";
                            Arr = objcon.Execute(cmd1, Arr, 3);
                            objdelivery.DIid = Arr[2].ToString();
                            if (objdelivery.FileExt.Length > 0)
                            {
                                NpgsqlParameter DeliveryNote = new NpgsqlParameter();
                                NpgsqlCommand comd = new NpgsqlCommand();

                                //sQry = " UPDATE \"TBLDELIVERYINSTRUCTION\" SET \"DI_FILE_EXT\"='" + objdelivery.FileExt + "' WHERE \"DI_ID\" = '" + objdelivery.DIid + "'";
                                //NpgsqlConnection objconn = new NpgsqlConnection();
                                //string strConnectionString = ConfigurationManager.ConnectionStrings["pgSQL"].ConnectionString + "Idea@12345+";
                                //objconn.ConnectionString = strConnectionString; // "Server=localhost;Port=5432;Database=TestTRM;User Id=postgres;Password =Idea123";  
                                //objconn.Open();
                                //objcon.ExecuteQry(sQry, NpgsqlCommand);
                                //objconn.Close();

                                //NpgsqlConnection objconn = new NpgsqlConnection();
                                //string strConnectionString = ConfigurationManager.ConnectionStrings["pgSQL"].ConnectionString + "Idea@12345+";
                               // objconn.ConnectionString = strConnectionString; // "Server=localhost;Port=5432;Database=TestTRM;User Id=postgres;Password =Idea123";  
                                NpgsqlCommand cmd = new NpgsqlCommand("proc_updatedi_fileifexists");
                                cmd.Parameters.AddWithValue("di_fileext", objdelivery.FileExt);
                                cmd.Parameters.AddWithValue("diid", objdelivery.DIid);
                                NpgsqlConnection objconn = new NpgsqlConnection();
                                string strConnectionString = ConfigurationManager.ConnectionStrings["pgSQL"].ConnectionString + "Idea@12345+";
                                objconn.ConnectionString = strConnectionString; // "Server=localhost;Port=5432;Database=TestTRM;User Id=postgres;Password =Idea123";  
                                objconn.Open();
                                objcon.Execute( cmd,Arr,0);
                                objconn.Close();
                                // objconn.Close();
                                // string strConnectionString = ConfigurationManager.ConnectionStrings["pgSQL"].ConnectionString + "Idea@12345+";
                                //  cmd.ConnectionString = strConnectionString; // "Server=localhost;Port=5432;Database=TestTRM;User Id=postgres;Password =Idea123";  

                            }

                        }
                        Arr[0] = "Updated Successfully";
                        Arr[1] = "0";
                        return Arr;
                    }

                    if (objdelivery.FileExt.Length > 0)
                    {
                        NpgsqlParameter DeliveryNote = new NpgsqlParameter();
                        NpgsqlCommand comd = new NpgsqlCommand();

                        //sQry = " UPDATE \"TBLDELIVERYINSTRUCTION\" SET \"DI_FILE_EXT\"='" + objdelivery.FileExt + "' WHERE \"DI_ID\" = '" + objdelivery.DIid + "'";

                        NpgsqlCommand cmd = new NpgsqlCommand("proc_updatedi_file");
                        cmd.Parameters.AddWithValue("di_fileext", objdelivery.FileExt);
                        cmd.Parameters.AddWithValue("diid", objdelivery.DIid);
                     //   NpgsqlConnection objconn = new NpgsqlConnection();
                     //   string strConnectionString = ConfigurationManager.ConnectionStrings["pgSQL"].ConnectionString + "Idea@12345+";
                     //   objconn.ConnectionString = strConnectionString; // "Server=localhost;Port=5432;Database=TestTRM;User Id=postgres;Password =Idea123";  
                     //   objconn.Open();
                        objcon.Execute( cmd,Arr,0);
                     //   objconn.Close();

                        NpgsqlCommand cmdd = new NpgsqlCommand("proc_updatedim_file");
                        cmdd.Parameters.AddWithValue("di_fileext", objdelivery.FileExt);
                        cmdd.Parameters.AddWithValue("pono", sPoNo); 
                        cmdd.Parameters.AddWithValue("dino", sDiNo);

                        //string sQry1 = " UPDATE \"TBLDELIVERYINSTMASTER\" SET \"DIM_FILE_EXT\"='" + objdelivery.FileExt + "' ";
                        //sQry1 += "  WHERE \"DIM_PO_ID\" = '" + sPoNo + "' AND \"DIM_DI_NO\" = '" + sDiNo + "' ";
                        // objcon.ExecuteQry(sQry1, NpgsqlCommand);
                      //  objconn.Open();
                        objcon.Execute(cmdd,Arr,0);
                       // objconn.Close();

                        //   NpgsqlCommand = new NpgsqlCommand("proc_updatedi_file");
                        //   NpgsqlCommand.Parameters.AddWithValue("di_fileext", objdelivery.FileExt);
                        //   NpgsqlCommand.Parameters.AddWithValue("diid", objdelivery.DIid);
                        //  objcon.ExecuteQry(sQry, NpgsqlCommand);
                        // NpgsqlCommand.Parameters.AddWithValue("pono", sPoNo);
                        //   NpgsqlCommand.Parameters.AddWithValue("dino", sDiNo);
                        //  NpgsqlConnection objconn = new NpgsqlConnection();
                        //   string strConnectionString = ConfigurationManager.ConnectionStrings["pgSQL"].ConnectionString + "Idea@12345+";
                        ///   objconn.ConnectionString = strConnectionString; // "Server=localhost;Port=5432;Database=TestTRM;User Id=postgres;Password =Idea123";  
                        //   objconn.Open();
                        //  objcon.ExecuteQry(sQry, NpgsqlCommand);
                        //    objconn.Close();
                    }
                }
                Arr[0] = "Saved Successfully";
                Arr[1] = "0";
                return Arr;

            }
            catch (Exception ex)
            {
                Arr[0] = "Failed to Save";
                Arr[1] = "1";
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }
        #region Requirement got 5th Jan 2022
        /*
        giving provision to update Due date 
        */
        #endregion
        // to update due date
        public string[] UpdateDIDetails(string sDueDate,string sDiNo, int sPoNo)
        {
            string[] Arr = new string[3];
            NpgsqlCommand = new NpgsqlCommand();
            DataTable dt = new DataTable();
            try
            {
                // sp to check DI not has got allotment
                NpgsqlCommand cmd = new NpgsqlCommand("sp_get_allotment_id");
                cmd.Parameters.AddWithValue("DiNo", sDiNo);
                cmd.Parameters.Add("allotmentid", NpgsqlDbType.Text);
                cmd.Parameters["allotmentid"].Direction = ParameterDirection.Output;
                DataTable dt1 = objcon.FetchDataTable(cmd);

                string saltId = dt1.Rows[0]["allotmentid"].ToString();

                if (saltId=="")
                {
                    // sp to update the due date
                    NpgsqlCommand cmd1 = new NpgsqlCommand("proc_update_di_details");
                    cmd1.Parameters.AddWithValue("DueDate", sDueDate);
                    cmd1.Parameters.AddWithValue("DiNo", sDiNo);
                    cmd1.Parameters.AddWithValue("PoNo", sPoNo);
                    cmd1.Parameters.Add("msg", NpgsqlDbType.Text);
                    cmd1.Parameters.Add("op_id", NpgsqlDbType.Text);
                    cmd1.Parameters["msg"].Direction = ParameterDirection.Output;
                    cmd1.Parameters["op_id"].Direction = ParameterDirection.Output;
                    Arr[0] = "msg";
                    Arr[1] = "op_id";

                    Arr = objcon.Execute(cmd1, Arr, 2);
                }
                else
                {
                    Arr[0] = "TC Allotment is Done for this DI";
                    Arr[1] = "0";
                }
                return Arr;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }
        public object GetPOImage(clsDelivery obj)
        {
            NpgsqlCommand = new NpgsqlCommand();
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getpoimage");
                cmd.Parameters.AddWithValue("phno", obj.PONumber);
   
                dt = objcon.FetchDataTable(cmd);
                if (dt.Rows.Count > 0)
                {
                    Byte[] bytes = (Byte[])dt.Rows[0]["PO_DOC"];
                    obj.FileExt = Convert.ToString(dt.Rows[0]["PO_DOC_EXT"]);
                    obj.POFile = bytes;
                }
                else
                {
                    obj.FileExt = "";
                    obj.POFile = null;
                }
                return obj;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return obj;
            }
        }
        #region 0 REFERENCES
        public object GetDIImage(clsDelivery obj)
        {
            NpgsqlCommand = new NpgsqlCommand();
            Byte[] POImage = null;
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("sp_getdiimage");
                cmd.Parameters.AddWithValue("phno", obj.PONumber);
                DataTable dt = new DataTable();
                dt = objcon.FetchDataTable(cmd);
                if (dt.Rows.Count > 0)
                {
                    Byte[] bytes = (Byte[])dt.Rows[0]["DI_INST_FILE"];
                    obj.FileExt = Convert.ToString(dt.Rows[0]["DI_INST_FILE_EXT"]);
                    obj.POFile = bytes;
                }
                else
                {
                    obj.FileExt = "";
                    obj.POFile = null;
                }
                return obj;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return POImage;
            }
        }
        #endregion
        // to Get TC Range
        public DataTable GetTCRange(clsDelivery obj)
        {
            NpgsqlCommand = new NpgsqlCommand();
            DataTable dt = new DataTable();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("proc_max_dtrrange");
                dt = objcon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
        // to check duplicate DI no
        //public string[] CheckduplicateDI(string ponumber, string dinumber)
        //{
        //    DataBase.DataBseConnection objDatabse = new DataBase.DataBseConnection(Constants.Password);

        //    NpgsqlCommand = new NpgsqlCommand();
        //    DataTable dt = new DataTable();
        //    string[] Arr = new string[3];
        //    string r = string.Empty;
        //    string str = string.Empty;
        //    try
        //    {
        //        NpgsqlCommand cmd = new NpgsqlCommand("sp_checkduplicatedi");
        //        cmd.Parameters.AddWithValue("di_no", dinumber.ToUpper());
        //        cmd.Parameters.AddWithValue("poid", ponumber.ToUpper());
        //        dt= objcon.FetchDataTable(cmd);
        //      //  str = objDatabse.StringGetValue(cmd);
        //        //   if (str != null && str != "")
        //        //  {
        //        //      Arr[0] = "Entered DI Number Already mapped with some other PO Number";
        //        //      Arr[1] = "2";
        //        //       return Arr;
        //        //}
        //        if (dt.Rows.Count > 0)
        //        {
        //            Arr[0] = "Entered DI Number Already Exists";
        //            Arr[1] = "2";
        //            return Arr;
        //        }
        //        Arr[0] = "";
        //        Arr[1] = "0";
        //        return Arr;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        return Arr;
        //    }
        //}
        public string[] CheckduplicateDI(string ponumber, string dinumber)
        {
            NpgsqlCommand = new NpgsqlCommand();
            DataTable dt = new DataTable();
            string[] Arr = new string[3];
            string r = string.Empty;
            string str = string.Empty;
            try
            {
                NpgsqlCommand Npgsql = new NpgsqlCommand("proc_checkduplicatedi");
                Npgsql.Parameters.AddWithValue("dino", dinumber.ToUpper());
                Npgsql.Parameters.AddWithValue("poid", Convert.ToString(ponumber));
                dt = objcon.FetchDataTable(Npgsql);
                if (dt.Rows.Count > 0)
                {
                    Arr[0] = "Entered DI Number Already Exists";
                    Arr[1] = "2";
                    return Arr;
                }
                Arr[0] = "";
                Arr[1] = "0";
                return Arr;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }

      



        public DataTable ArrangedtRange(DataTable dtold)
        {
            NpgsqlCommand = new NpgsqlCommand();
            DataTable dt = new DataTable();
            DataTable dtnew = new DataTable();
            int startrange = 0;
            int endrange = 0;
            try
            {
                dtnew.Columns.Add("DI_ID");
                dtnew.Columns.Add("DIM_DI_NO");
                dtnew.Columns.Add("DI_PO_ID");
                dtnew.Columns.Add("DI_STORE_ID");
                dtnew.Columns.Add("DI_DUEDATE");
                dtnew.Columns.Add("DI_MAKE_ID");
                dtnew.Columns.Add("DI_CAPACITY");
                dtnew.Columns.Add("DI_CAPACITY_ID");
                dtnew.Columns.Add("DI_STARRATE");
                dtnew.Columns.Add("DI_QUANTITY");
                dtnew.Columns.Add("DI_DATE");
                dtnew.Columns.Add("DI_FILE_EXT");
                dtnew.Columns.Add("DI_START_RANGE");
                dtnew.Columns.Add("DI_END_RANGE");
                dtnew.Columns.Add("DI_STORE");
                dtnew.Columns.Add("DI_MAKE");
                dtnew.Columns.Add("DI_STARRATENAME");


                for (int i = 0; i < dtold.Rows.Count; i++)
                {
                    string sDi_Id = Convert.ToString(dtold.Rows[i]["DI_ID"]);
                    string sDiNo = Convert.ToString(dtold.Rows[i]["DIM_DI_NO"]);
                    string sPoNo = Convert.ToString(dtold.Rows[i]["DI_PO_ID"]);
                    string sStoreId = Convert.ToString(dtold.Rows[i]["DI_STORE_ID"]);
                    string sDueDate = Convert.ToString(dtold.Rows[i]["DI_DUEDATE"]);
                    string sMake = Convert.ToString(dtold.Rows[i]["DI_MAKE_ID"]);
                    string sCapacity = Convert.ToString(dtold.Rows[i]["DI_CAPACITY"]);
                    string sCapacityID = Convert.ToString(dtold.Rows[i]["DI_CAPACITY_ID"]);
                    string sStartype = Convert.ToString(dtold.Rows[i]["DI_STARRATE"]);
                    string sQuantity = Convert.ToString(dtold.Rows[i]["DI_QUANTITY"]);
                    string sDiDate = Convert.ToString(dtold.Rows[i]["DI_DATE"]);
                    string sFileExt = Convert.ToString(dtold.Rows[i]["DI_FILE_EXT"]);
                    string sStore = Convert.ToString(dtold.Rows[i]["DI_STORE"]);
                    string sMakee = Convert.ToString(dtold.Rows[i]["DI_MAKE"]);
                    string sStartratename = Convert.ToString(dtold.Rows[i]["DI_STARRATENAME"]);
                    string sStartRange = Convert.ToString(dtold.Rows[i]["DI_START_RANGE"]);
                    string sEndRange = Convert.ToString(dtold.Rows[i]["DI_END_RANGE"]);

                    NpgsqlCommand cmd = new NpgsqlCommand("proc_max_dtrrange");
                    dt = objcon.FetchDataTable(cmd);
                    if (dt.Rows.Count > 0)
                    {
                        if (i >= 1)
                        {
                            startrange = Convert.ToInt32(endrange) + 1;
                            endrange = startrange + Convert.ToInt32(sQuantity) - 1;
                        }
                        else
                        {
                            startrange = Convert.ToInt32(dt.Rows[0]["proc_max_dtrrange"]);
                            endrange = startrange + Convert.ToInt32(sQuantity) - 1;
                        }
                    }
                    DataRow dRow = dtnew.NewRow();
                    dRow["DI_ID"] = sDi_Id;
                    dRow["DIM_DI_NO"] = sDiNo;
                    dRow["DI_PO_ID"] = sPoNo;
                    dRow["DI_STORE_ID"] = sStoreId;
                    dRow["DI_DUEDATE"] = sDueDate;
                    dRow["DI_MAKE_ID"] = sMake;
                    dRow["DI_CAPACITY"] = sCapacity;
                    dRow["DI_CAPACITY_ID"] = sCapacityID;
                    dRow["DI_STARRATE"] = sStartype;
                    dRow["DI_QUANTITY"] = sQuantity;
                    dRow["DI_DATE"] = sDiDate;
                    dRow["DI_FILE_EXT"] = sFileExt;
                    dRow["DI_STORE"] = sStore;
                    dRow["DI_MAKE"] = sMakee;
                    dRow["DI_STARRATENAME"] = sStartratename;
                    dRow["DI_START_RANGE"] = startrange;
                    dRow["DI_END_RANGE"] = endrange;
                    dtnew.Rows.Add(dRow);
                    dtnew.AcceptChanges();
                }
                return dtnew;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }
    }
}
