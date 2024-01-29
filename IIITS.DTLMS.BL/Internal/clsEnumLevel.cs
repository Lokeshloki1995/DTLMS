using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Configuration;


namespace IIITS.DTLMS.BL
{
   public class clsEnumLevel
    {
       string strFormCode = "clsEnumLevel";
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
       
        
        public string sOfficeCode { get; set; }
        public string sFeederCode { get; set; }
        public string sLevel { get; set; }
        public string sRemarks { get; set; }

        public string sCrBy { get; set; }

        int Circle_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Circle_code"]);
        int Division_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Division_code"]);
        int SubDiv_code = Convert.ToInt32(ConfigurationSettings.AppSettings["SubDiv_code"]);
        int Section_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Section_code"]);

        public string[] SaveEnumerationLevel(clsEnumLevel objEnum)
        {
            string[] Arr = new string[2];
            try
            {
                    string strQry = string.Empty;
                    strQry = "UPDATE TBLENUMERATIONDETAILS SET ED_ENUM_LEVEL='" + objEnum.sLevel + "',";
                    strQry += "ED_LEVEL_REMRAKS='" + objEnum.sRemarks.Replace("'", "`") + "',ED_LEVEL_CRBY='"+ objEnum.sCrBy +"', ";
                    strQry += " ED_LEVEL_CRON=SYSDATE WHERE ED_FEEDERCODE='" + objEnum.sFeederCode + "'";
                    ObjCon.Execute(strQry);

                    Arr[0] = "Saved Successfully ";
                    Arr[1] = "0";
                    return Arr;

            }

            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveEnumerationLevel");
                return Arr;
            }


        }








    }
}
