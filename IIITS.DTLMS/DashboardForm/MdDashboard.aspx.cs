using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Configuration;
using System.Web.UI.DataVisualization.Charting;
using IIITS.DTLMS.BL.Dashboard;
using System.Web.Services;

namespace IIITS.DTLMS.DashboardForm
{
    public partial class MdDashboard : System.Web.UI.Page
    {
        string strFormCode = "MdDashboard";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];
                //CheckAccessRights("4");
                if (objSession.sRoleType == "1")
                {
                    //Alternativepowersuppl.Visible = true;
                   // RepairerPerformanc.Visible = false;
                    PendingReplacemen.Visible = false;
                    RIstatuses.Visible = true;
                }

                if (objSession.sRoleType == "2")
                {
                   // RepairerPerformanc.Visible = true;
                    PendingReplacemen.Visible = true;
                    //Alternativepowersuppl.Visible = false;
                    RIstatuses.Visible = false;
                }


            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "MdDashboard";
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sAccessType = "1" + "," + sAccessType;
                bool bResult = objApproval.CheckAccessRights(objApproval);
                if (bResult == false)
                {
                    if (sAccessType == "4")
                    {
                        Response.Redirect("~/UserRestrict.aspx", false);
                    }
                    else
                    {
                        ShowMsgBox("Sorry , You are not authorized to Access");
                    }
                }
                return bResult;

            }
            catch (Exception ex)
            {
               
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;

            }
        }

        private void ShowMsgBox(string sMsg)
        {
            try
            {
                string sShowMsg = string.Empty;
                sShowMsg = "<script language=javascript> alert ('" + sMsg + "')</script>";
                this.Page.RegisterStartupScript("Msg", sShowMsg);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        [WebMethod]
        public static List<string> FailureAnalysis()
        {
            string str = "";
            List<string> kkk = new List<string>();
            clsDashboardUcForms objDashBoard = new clsDashboardUcForms();
            objDashBoard.officecode = "";
            DataTable dt = objDashBoard.LoadFailureDetailsstackgraph(objDashBoard);

            var x = (from r in dt.AsEnumerable()
                     select r["off_name"]).Distinct().ToList();
            str = "[";
            foreach (string strValues in x)
            {
                var  rows = dt.AsEnumerable()
                           .Where(r => r.Field<Int32?>("presentcount") != 0).Where(r => r.Field<string>("off_name") == strValues)
                          ;

                DataTable selectedTable;
                if (rows.Any())
                    selectedTable = rows.CopyToDataTable();
                else
                    selectedTable = dt.Clone();
                str += "{";
                str += string.Format("name: '{0}',", strValues);
                str += string.Format("data: [");
                foreach (DataRow drValues in selectedTable.Rows)
                {

                    str += string.Format("{0},", drValues["presentcount"]);
                }
                str = str.TrimEnd(',');
                str += string.Format("]");
                // [5, 3, 4, 7, 2]
                str += "},";
            }
            str = str.TrimEnd(',');
            str += "]";
            // == value

            kkk.Add(str);
            return kkk;
        }
        [WebMethod]
        public static List<string> RepairerStatus(string strOfficeCode)
        {
            string str = "";
            clsSession objSession = (clsSession)HttpContext.Current.Session["clsSession"];
            List<string> kkk = new List<string>();
            clsDashboardUcForms objDashBoard = new clsDashboardUcForms();
            if (strOfficeCode == null) {
                strOfficeCode = "";
            }
            objDashBoard.officecode = strOfficeCode;
            objDashBoard.roleType = objSession.sRoleType;
            objDashBoard.roleId = objSession.RoleId;
            DataTable  dt = objDashBoard.LoadRepairerPerformanceDetails(objDashBoard);

            if (dt.Rows.Count > 0)
            {
                str = "[";
                foreach (DataRow drDetails in dt.Rows) {
                    str += string.Format("['{0}',{1}],", drDetails["status"], drDetails["total_count"]);
                }
                str = str.TrimEnd(',');
                str += "]";
            }
            else
            {
                str = "[";
                str += "['Pending',0],";
                str += "['Completed',0]";
                str += "]";
            }
            // == value

            kkk.Add(str);
            return kkk;
        }
        [WebMethod]
        public static List<string> FrequentlyFailedDtc()
        {
            string str = "";
            clsSession objSession = (clsSession)HttpContext.Current.Session["clsSession"];
            List<string> kkk = new List<string>();
            clsDashboardUcForms objDashBoard = new clsDashboardUcForms();
         
            objDashBoard.officecode = objSession.OfficeCode;
            objDashBoard.roleType = objSession.sRoleType;
            objDashBoard.roleId = objSession.RoleId;
            DataTable dt = objDashBoard.Loadfrequentlyfailedgraph(objDashBoard);

            if (dt.Rows.Count > 0)
            {
                str = "[";
                foreach (DataRow drDetails in dt.Rows)
                {
                    str += string.Format("['{0}',{1}],", drDetails["off_Name"], drDetails["total"]);
                }
                str = str.TrimEnd(',');
                str += "]";
            }
           
            // == value

            kkk.Add(str);
            return kkk;
        }
        [WebMethod]
        public static List<string> FrequentlyFailedDtr()
        {
            string str = "";
            clsSession objSession = (clsSession)HttpContext.Current.Session["clsSession"];
            List<string> kkk = new List<string>();
            clsDashboardUcForms objDashBoard = new clsDashboardUcForms();

            objDashBoard.officecode = objSession.OfficeCode;
            objDashBoard.roleType = objSession.sRoleType;
            objDashBoard.roleId = objSession.RoleId;
            DataTable dt = objDashBoard.Loadfrequentlyfaileddtrgraph(objDashBoard);
            var xOfficeName = (from r in dt.AsEnumerable()
                               select r["off_name"]).Distinct().ToList();
            string joined = "[";
            foreach (string strVal in xOfficeName)
            {
                joined += string.Format("'{0}',", strVal);
            }
            joined = joined + "]";
            joined = joined.TrimEnd(',');
            kkk.Add(joined);
            var xCategorys = (from r in dt.AsEnumerable()
                             
                              select r["capacity"]).Distinct().ToList();
            str = "[";
            foreach (string strValues in xCategorys)
            {
                // status,total_count
                // Create a table from the query.
          

                DataTable selectedTable = dt.AsEnumerable()
                           .Where(r => r.Field<string>("capacity") == strValues)
                           .CopyToDataTable();
                str += "{";
                str += string.Format("name: '{0}',", strValues);
                str += string.Format("data: [");
                foreach (DataRow drValues in selectedTable.Rows)
                {
                    str += string.Format("{0},", drValues["total_count"]);
                }
                str = str.TrimEnd(',');
                str += string.Format("]");
                // [5, 3, 4, 7, 2]
                str += "},";
            }
            str = str.TrimEnd(',');
            str += "]";

            // == value

            kkk.Add(str);
            return kkk;
        }



        [WebMethod]
        public static List<string> DTrConditionsField()
        {
            string str = "";
            List<string> kkk = new List<string>();
            clsDashboardUcForms objDashBoard = new clsDashboardUcForms();
            clsSession objSession = (clsSession)HttpContext.Current.Session["clsSession"];
            objDashBoard.officecode = objSession.OfficeCode;

            objDashBoard.roleType = objSession.sRoleType;
            objDashBoard.roleId = objSession.RoleId;
            DataTable dt = objDashBoard.LoadTcStatusDetails(objDashBoard);

            var xOfficeName = (from r in dt.AsEnumerable()
                     select r["off_name"]).Distinct().ToList();
            string joined = "[";
            foreach (string strVal in xOfficeName) {
                joined += string.Format("'{0}',", strVal);
            }
            joined = joined + "]";
            joined = joined.TrimEnd(',');
            kkk.Add(joined);
            var xCategorys = (from r in dt.AsEnumerable()
                     select r["status"]).Distinct().ToList();
            str = "[";
            foreach (string strValues in xCategorys)
            {
               // status,total_count

                DataTable selectedTable = dt.AsEnumerable()
                           .Where(r => r.Field<string>("status") == strValues)
                           .CopyToDataTable();
                str += "{";
                str += string.Format("name: '{0}',", strValues);
                str += string.Format("data: [");
                foreach (DataRow drValues in selectedTable.Rows)
                {
                    str += string.Format("{0},", drValues["total_count"]);
                }
                str = str.TrimEnd(',');
                str += string.Format("]");
                // [5, 3, 4, 7, 2]
                str += "},";
            }
            str = str.TrimEnd(',');
            str += "]";
            // == value

            kkk.Add(str);
            return kkk;
        }
        [WebMethod]
        public static List<string> DTrConditionsStore()
        {
            string str = "";
            List<string> kkk = new List<string>();
            clsDashboardUcForms objDashBoard = new clsDashboardUcForms();
            clsSession objSession = (clsSession)HttpContext.Current.Session["clsSession"];
            objDashBoard.officecode = objSession.OfficeCode;

            objDashBoard.roleType = objSession.sRoleType;
            objDashBoard.roleId = objSession.RoleId;
            DataTable dt = objDashBoard.LoadTcStatusDetailsadmin(objDashBoard);

            var xOfficeName = (from r in dt.AsEnumerable()
                               select r["OFF_NAME"]).Distinct().ToList();
            string joined = "[";
            foreach (string strVal in xOfficeName)
            {
                joined += string.Format("'{0}',", strVal);
            }
            joined = joined + "]";
            joined = joined.TrimEnd(',');
            kkk.Add(joined);
            var xCategorys = (from r in dt.AsEnumerable()
                              select r["status"]).Distinct().ToList();
            str = "[";
            foreach (string strValues in xCategorys)
            {
                // status,total_count

                DataTable selectedTable = dt.AsEnumerable()
                           .Where(r => r.Field<string>("STATUS") == strValues)
                           .CopyToDataTable();
                str += "{";
                str += string.Format("name: '{0}',", strValues);
                str += string.Format("data: [");
                foreach (DataRow drValues in selectedTable.Rows)
                {
                    str += string.Format("{0},", drValues["TOTAL_COUNT"]);
                }
                str = str.TrimEnd(',');
                str += string.Format("]");
                // [5, 3, 4, 7, 2]
                str += "},";
            }
            str = str.TrimEnd(',');
            str += "]";
            // == value

            kkk.Add(str);
            return kkk;
        }

        [WebMethod]
        public static List<string> ReplaceDtcStatus(string strOfficeCode)
        {
            string str = "";
            clsSession objSession = (clsSession)HttpContext.Current.Session["clsSession"];
            List<string> kkk = new List<string>();
            clsDashboardUcForms objDashBoard = new clsDashboardUcForms();
            if (strOfficeCode == null)
            {
                strOfficeCode = "";
            }
            objDashBoard.officecode = strOfficeCode;
            objDashBoard.roleType = objSession.sRoleType;
            objDashBoard.roleId = objSession.RoleId;
            DataTable dt = objDashBoard.LoadPendingReplacementDetails(objDashBoard);

            if (dt.Rows.Count > 0)
            {
                str = "[";
                foreach (DataRow drDetails in dt.Rows)
                {
                    str += string.Format("['{0}',{1}],", drDetails["status"], drDetails["total_count"]);
                }
                str = str.TrimEnd(',');
                str += "]";
            }
            else
            {
                str = "[";
                str += "['1 Day',0],";
                str += "['1-3 Days',0]";
                str += "['>3 Days',0]";
                str += "]";
            }
            // == value

            kkk.Add(str);
            return kkk;
        }
    }
}