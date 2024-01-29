using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;
using System.Data;
using System.Data.OleDb;

namespace IIITS.DTLMS.BL
{
   public  class clsBillPayment
    {
       string strFormCode = "clsBillPayment";
       PGSqlConnection objCon = new PGSqlConnection(Constants.Password);

       public string sWONo { get; set; }
       public string sBRNo { get; set; }
       public string sAmount { get; set; }
       public string sPaymentDate { get; set; }
       public string sBillId { get; set; }
       public string sCrby { get; set; }
       public string sPONo { get; set; }
       public string sPODate { get; set; }

       NpgsqlCommand NpgsqlCommand;

       public DataTable  LoadBillDetails(clsBillPayment objPayment)
       {
           DataTable dt = new DataTable();
           try
           {
               string strQry = string.Empty;
               strQry = "SELECT \"TC_CODE\",(CASE WHEN \"TR_DELIVERY_LOCATION\" IS NULL THEN 'NO' ELSE 'YES' END ) DELIVERED,\"SM_NAME\",";
               strQry += "TO_CHAR(\"TR_DELIVERY_DATE\",'DD-MON-YYYY') DELIVERY_DATE,\"TR_RI_NO\"  FROM \"TBLTRANFORMERREPAIRS\"  INNER JOIN \"TBLTCMASTER\" ON  ";
               strQry += " \"TC_CODE\"=\"TR_DEVICE_ID\" LEFT OUTER JOIN  \"TBLSTOREMAST\" ON \"SM_ID\" = \"TR_DELIVERY_LOCATION\" WHERE \"TR_WO_NO\" =:sWONo";
               NpgsqlCommand = new NpgsqlCommand();
               NpgsqlCommand.Parameters.AddWithValue("sWONo", objPayment.sWONo);
               dt = objCon.FetchDataTable(strQry, NpgsqlCommand);
               return dt;
           }
           catch (Exception ex)
           {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
           }
       }

       public string[] SaveBillPayment(clsBillPayment objPayment)
       {
           string[] Arr = new string[2];
           string strQry = string.Empty;
           try
           {
               objCon.BeginTransaction();

               strQry = " Update \"TBLBILLTC\" set \"BT_BR_NO\" =:sBRNo, \"BT_AMOUNT\" =:sAmount,\"BT_PAYMENT_AUTH\"=:sCrby,";
               strQry += " \"BT_PAYMENT_DATE\" =to_date(:sPaymentDate,'DD/MM/YYYY'), \"BT_PAYMENT_FLAG\" =1, \"BT_PAYMENT_ENTRY_DATE\" = NOW()";
               strQry += " WHERE \"BT_ID\"=:sBillId";
               NpgsqlCommand = new NpgsqlCommand();
               NpgsqlCommand.Parameters.AddWithValue("sBRNo", objPayment.sBRNo.ToUpper());
               NpgsqlCommand.Parameters.AddWithValue("sAmount", objPayment.sAmount.Trim().ToUpper());
               NpgsqlCommand.Parameters.AddWithValue("sCrby",Convert.ToInt32(objPayment.sCrby));
               NpgsqlCommand.Parameters.AddWithValue("sPaymentDate", objPayment.sPaymentDate);
               NpgsqlCommand.Parameters.AddWithValue("sBillId", Convert.ToInt32(objPayment.sBillId));

               objCon.ExecuteQry(strQry, NpgsqlCommand);

               objCon.CommitTransaction();
               Arr[0] = "Bill Passed Successfully To Account Section";
               Arr[1] = "0";
               return Arr;
               
           }
           catch (Exception ex)
           {
               objCon.RollBackTrans();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
           }
       }

       public clsBillPayment   GetBillDetails(clsBillPayment objBill)
       {
           DataTable dtBill = new DataTable();
           try
           {
               string strQry = string.Empty;

               strQry = "  SELECT \"BT_WO_NO\",CAST(\"BT_ID\" AS TEXT) BT_ID,\"BT_PO_NO\",to_char(\"BT_PO_DATE\",'DD/MM/YYYY') BT_PO_DATE FROM \"TBLBILLTC\" WHERE \"BT_PAYMENT_FLAG\" =0 AND \"BT_WO_NO\" =:sWONo";
               NpgsqlCommand = new NpgsqlCommand();
               NpgsqlCommand.Parameters.AddWithValue("sWONo", objBill.sWONo);
               dtBill = objCon.FetchDataTable(strQry, NpgsqlCommand);
               if (dtBill.Rows.Count > 0)
               {
                   objBill.sBillId = dtBill.Rows[0]["BT_ID"].ToString();
                   objBill.sWONo = dtBill.Rows[0]["BT_WO_NO"].ToString();
                   objBill.sPONo = dtBill.Rows[0]["BT_PO_NO"].ToString();
                   objBill.sPODate = dtBill.Rows[0]["BT_PO_DATE"].ToString();
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
