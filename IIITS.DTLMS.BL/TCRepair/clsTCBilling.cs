using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.PGSQL.DAL;
using System.Data;
using System.Data.OleDb;
using Npgsql;

namespace IIITS.DTLMS.BL
{
   public  class clsTCBilling
    {
       string strFormCode = "clsRepairDeliver";
       PGSqlConnection objCon = new PGSqlConnection(Constants.Password);

       public string sBillId { get; set; }
       public string sWONo { get; set; }
       public string sPONo { get; set; }
       public string sPODate { get; set; }
       public string sDescription { get; set; }
       public string sCrby { get; set; }

       NpgsqlCommand NpgsqlCommand;
       public DataTable LoadTCforBill(clsTCBilling objBill)
       {
           DataTable dt = new DataTable();
           try
           {
               string strQry = string.Empty;
               strQry = "SELECT \"TC_CODE\",\"TC_SLNO\",(CASE WHEN \"TR_DELIVERY_LOCATION\" IS NULL THEN 'NO' ELSE 'YES' END ) DELIVERED,\"SM_NAME\",";
               strQry += "TO_CHAR(\"TR_DELIVERY_DATE\",'DD-MON-YYYY') DELIVERY_DATE,\"TR_RI_NO\"  FROM \"TBLTRANFORMERREPAIRS\" INNER JOIN \"TBLTCMASTER\" ";
               strQry += " ON \"TC_CODE\"=\"TR_DEVICE_ID\" LEFT OUTER JOIN \"TBLSTOREMAST\" AND \"SM_ID\" = \"TR_DELIVERY_LOCATION\" WHERE \"TR_WO_NO\" =:sWONo";
               strQry += " AND \"TR_WO_NO\" NOT IN ( SELECT \"BT_WO_NO\" from \"TBLBILLTC\" WHERE  \"BT_PAYMENT_FLAG\" =1 ) ";
               //if (!Validation.IsAdmin)
               //{
               //    strQry += " and TR_DELIVERY_LOCATION ='" + Validation.strStoreId + "'";
               //}
               NpgsqlCommand = new NpgsqlCommand();
               NpgsqlCommand.Parameters.AddWithValue("sWONo", objBill.sWONo);

               dt = objCon.FetchDataTable(strQry, NpgsqlCommand);
               return dt;

           }
           catch (Exception ex)
           {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
           }
       }

       public string[] SaveTCBillDetails(clsTCBilling objBill)
       {
           string[] Arr = new string[2];
           string strQry = string.Empty;
           try
           {
               OleDbDataReader dr;

               if (objBill.sBillId == "")
               {
                   NpgsqlCommand = new NpgsqlCommand();
                   NpgsqlCommand.Parameters.AddWithValue("sWONo", objBill.sWONo.ToUpper());
                   String sWoNo = objCon.get_value("SELECT \"BT_WO_NO\" FROM \"TBLBILLTC\" WHERE \"BT_WO_NO\" =:sWONo", NpgsqlCommand);
                   if (sWoNo.Length > 0)
                   {
                       Arr[0] = "Bill already issued for this Work Order";
                       Arr[1] = "2";
                       return Arr;
                   }
                  
                   objCon.BeginTransaction();

                   objBill.sBillId  = Convert.ToString(objCon.Get_max_no("BT_ID", "TBLBILLTC"));
                   strQry = "INSERT INTO TBLBILLTC(\"BT_ID\",\"BT_WO_NO\",\"BT_PO_NO\",\"BT_PO_DATE\",\"BT_DESC\",\"BT_ENTRY_AUTH\",\"BT_ENTRY_DATE\") VALUES";
                   strQry += "(:sBillId,:sWONo,:sPONo,";
                   strQry += "TO_DATE(:sPODate,'DD/MM/YYYY'),:sDescription,:sCrby,NOW())";
                   NpgsqlCommand = new NpgsqlCommand();
                   NpgsqlCommand.Parameters.AddWithValue("sBillId",Convert.ToInt32(objBill.sBillId));
                   NpgsqlCommand.Parameters.AddWithValue("sWONo", objBill.sWONo.ToUpper());
                   NpgsqlCommand.Parameters.AddWithValue("sPONo", objBill.sPONo.ToUpper());
                   NpgsqlCommand.Parameters.AddWithValue("sPODate", objBill.sPODate);
                   NpgsqlCommand.Parameters.AddWithValue("sDescription", objBill.sDescription.ToUpper());
                   NpgsqlCommand.Parameters.AddWithValue("sCrby",Convert.ToInt32( objBill.sCrby));
                   objCon.ExecuteQry(strQry, NpgsqlCommand);
                   objCon.CommitTransaction();

                   Arr[0] = "Bill Passed Successfully To Account Section";
                   Arr[1] = "0";
                   return Arr;
               }
               else
               {
                   objCon.BeginTransaction();

                   strQry = " Update \"TBLBILLTC\" set \"BT_WO_NO\" =:sWONo, \"BT_PO_NO\" =:sPONo,";
                   strQry += " \"BT_PO_DATE\" =to_date(:sPODate,'DD/MM/YYYY'), \"BT_DESC\" =:sDescription";
                   strQry += " where \"BT_ID\" =:sBillId";
                   NpgsqlCommand = new NpgsqlCommand();
                   NpgsqlCommand.Parameters.AddWithValue("sWONo", objBill.sWONo.ToUpper());
                   NpgsqlCommand.Parameters.AddWithValue("sPONo", objBill.sPONo.ToUpper());
                   NpgsqlCommand.Parameters.AddWithValue("sPODate", objBill.sPODate);
                   NpgsqlCommand.Parameters.AddWithValue("sDescription", objBill.sDescription.ToUpper());
                   NpgsqlCommand.Parameters.AddWithValue("sBillId",Convert.ToInt32(objBill.sBillId));
                   //NpgsqlCommand.Parameters.AddWithValue("sCrby", Convert.ToInt32(objBill.sCrby));
                   objCon.ExecuteQry(strQry, NpgsqlCommand);

                   objCon.CommitTransaction();

                   Arr[0] = "Bill Details Updated Successfully";
                   Arr[1] = "1";
                   return Arr;
               }
              
           }
           catch (Exception ex)
           {
               objCon.RollBackTrans();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
           }
       }

       public DataTable LoadBillData()
       {
           DataTable dtBill = new DataTable();
           try
           {
               string strQry = string.Empty;

               strQry = "select \"BT_ID\",\"BT_WO_NO,\"\"BT_PO_NO\",TO_CHAR(\"BT_PO_DATE\",'DD-MON-YYYY') BT_PO_DATE,\"BT_DESC\" FROM \"TBLBILLTC\" where \"BT_BR_NO\" is null";
               dtBill = objCon.FetchDataTable(strQry);
               return dtBill;
           }
           catch (Exception ex)
           {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dtBill;
           }
       }

       public clsTCBilling GetBillDetails(clsTCBilling objBill)
       {
           DataTable dtBill = new DataTable();
           try
           {
               string strQry = string.Empty;

               strQry = "select \"BT_ID\",\"BT_WO_NO\",\"BT_PO_NO\",TO_CHAR(\"BT_PO_DATE\",'DD/MM/YYYY') BT_PO_DATE,\"BT_DESC\" FROM \"TBLBILLTC\" where ";
               strQry += " \"BT_BR_NO\" is null AND \"BT_ID\" =:sBillId";
               NpgsqlCommand = new NpgsqlCommand();
               NpgsqlCommand.Parameters.AddWithValue("sBillId",Convert.ToInt32(objBill.sBillId));
               dtBill = objCon.FetchDataTable(strQry, NpgsqlCommand);
               if (dtBill.Rows.Count > 0)
               {
                   objBill.sWONo = dtBill.Rows[0]["BT_WO_NO"].ToString();
                   objBill.sPONo = dtBill.Rows[0]["BT_PO_NO"].ToString();
                   objBill.sPODate = dtBill.Rows[0]["BT_PO_DATE"].ToString();
                   objBill.sDescription = dtBill.Rows[0]["BT_DESC"].ToString();
               }
               return objBill;
           }
           catch (Exception ex)
           {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objBill;
           }
       }
    }
}
