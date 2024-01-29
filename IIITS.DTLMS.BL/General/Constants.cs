using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace IIITS.DTLMS.BL
{
    /// <summary>
    /// This class is used to store all the global constants through the project
    /// </summary>
    public static class Constants
    {
        public static string Password = Convert.ToString(ConfigurationManager.AppSettings["pgSQLPassword"]);
        //public static string Password = "TG9nTWVASUlJVFM=";
        public static int Zone = Convert.ToInt32(ConfigurationManager.AppSettings["Zone_code"]);
        public static  int Division = Convert.ToInt32(ConfigurationManager.AppSettings["Division_code"]);
        public static int SubDivision = Convert.ToInt32(ConfigurationManager.AppSettings["SubDiv_code"]);
        public static int Section = Convert.ToInt32(ConfigurationManager.AppSettings["Section_code"]);
        public static int Feeder = Convert.ToInt32(ConfigurationManager.AppSettings["feeder_code"]);
        public static int Circle = Convert.ToInt32(ConfigurationManager.AppSettings["Circle_code"]);

        //To Check Access Rights  1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY
        public class CheckAccessRights
        {
            public static string CheckAccessRightsAll = "1";
            public static string CheckAccessRightsCreate = "2";
            public static string CheckAccessRightsModify = "3";
            public static string CheckAccessRightsReadOnly = "4";
        }
        public class General
        {
            public static string AccessDenaidMsg = "Sorry , You are not authorized to Access";
        }
        //Work order
        public class DTCFailure
        {
            public static string TypeFailure = "1";
            public static string TypeEnhancement = "2";
            public static string TypeFailureWithEnhancement = "4";
            public static string TypeNewDTC = "3";
        }
        public class Roles
        {
            //Roles Type from TBLROLS
            public static string OfficeLevel = "1";
            public static string StoreLevel = "2";
            public static string AdminLevel = "3";

        }
        public class ButtonClass
        {
            public static string ButtenUpdate = "Update";
            public static string ButtenSave = "Save";
        }

    }
}
