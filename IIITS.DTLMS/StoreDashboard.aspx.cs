using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL.Dashboard;
using System.Data;
using System.Configuration;
using System.Web.UI.DataVisualization.Charting;
using IIITS.DTLMS.BL;


namespace IIITS.DTLMS
{
    public partial class StoreDashboard : System.Web.UI.Page
    {
        string strFormCode = "StoreDashboard";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
            {
                Response.Redirect("~/Login.aspx", false);
            }

            objSession = (clsSession)Session["clsSession"];

            if (!IsPostBack)
            {

                if (objSession.sRoleType != "2")
                {
                    Response.Redirect("~/UserRestrict.aspx", false);
                    return;
                }
                if (objSession != null)
                {
                    lblLocation.Text = objSession.OfficeName;
                    hdfLocationCode.Value = objSession.OfficeCode;
                }

              
                GetConditionOfTC();
                GetCapacityWiseTC();
                GetPendingTC();
                Getbankcount();
            }
        }
        public void DashboardFunctions()
        {
            try
            {
                GetConditionOfTC();
                GetCapacityWiseTC();
                Getbankcount();
                GetPendingTC();
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
     
    public void GetConditionOfTC()
    {
        try
        {
           
            clsStoreDashboard objSDashboard = new clsStoreDashboard();

            objSDashboard.sOfficeCode = hdfLocationCode.Value;

            if (objSDashboard.sOfficeCode == "0")
            {
                objSDashboard.sOfficeCode = null;
            }


            lblNewTC.Text = objSDashboard.GetNewTCCount(objSDashboard);
            lblRepairGood.Text = objSDashboard.GetRepaireGoodCount(objSDashboard);

            lblReleaseGood.Text = objSDashboard.GetReleaseGoodCount(objSDashboard);


            lblFaulty.Text = objSDashboard.GetFaultyCount(objSDashboard);


            lblScrapTC.Text = objSDashboard.GetMobileTCCount(objSDashboard);

            
        }
        catch (Exception ex)
        {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
    }
    public void GetCapacityWiseTC()
    {
        try
        {

            clsStoreDashboard objSDashboard = new clsStoreDashboard();

            objSDashboard.sOfficeCode = hdfLocationCode.Value;

            if (objSDashboard.sOfficeCode == "0")
            {
                objSDashboard.sOfficeCode = null;
            }

            lblCapacityless25.Text = objSDashboard.GetCapacityless25(objSDashboard);
            lblCapacity25_100.Text = objSDashboard.GetCapacity25_100(objSDashboard);
            lblCapacity125_250.Text = objSDashboard.GetCapacity125_250(objSDashboard);

            lblCapacitygreater250.Text = objSDashboard.GetCapacitygreater250(objSDashboard);



        }
        catch (Exception ex)
        {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
    }
    public void GetPendingTC()
    {
        try
        {

            clsStoreDashboard objSDashboard = new clsStoreDashboard();

            objSDashboard.sOfficeCode = hdfLocationCode.Value;

            if (objSDashboard.sOfficeCode == "0")
            {
                objSDashboard.sOfficeCode = null;
            }


                objSDashboard.sroletype = objSession.sRoleType;

                lblTotalPendingfor_Issue.Text = objSDashboard.GetIssuePendingCount(objSDashboard);

            lblTotalPendingfor_Repair.Text = objSDashboard.GetRepairPendingCount(objSDashboard);


            lblTotalPendingto_Recive.Text = objSDashboard.GetRecivePendingCount(objSDashboard);

        }
        catch (Exception ex)
        {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
    }

        public void Getbankcount()
        {
            try
            {

                clsStoreDashboard objSDashboard = new clsStoreDashboard();

                objSDashboard.sOfficeCode = hdfLocationCode.Value;

                if (objSDashboard.sOfficeCode == "0")
                {
                    objSDashboard.sOfficeCode = null;
                }


                objSDashboard.sroletype = objSession.sRoleType;
                //lblNewTC.Text = objSDashboard.GetNewTCCount(objSDashboard);
                // lblReleaseGood1.Text = "0";//objSDashboard.GetReleasegoodCount(objSDashboard);
                //lblGood.Text = "0";//objSDashboard.GetbankGoodCount(objSDashboard);
                //lblRepairgood1.Text = "0";//objSDashboard.GetRepairGoodCount(objSDashboard);
                lblGood.Text = objSDashboard.GetbankGoodCount(objSDashboard);
                lblReleaseGood1.Text = objSDashboard.GetReleasegoodCount(objSDashboard);
                lblRepairgood1.Text = objSDashboard.GetRepairGoodCount(objSDashboard);





            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void lnkConditionPending(object sender, EventArgs e)
        {
            try
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePendingOverview.aspx?RefId=Condition&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','_blank');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void NewTC_Click(object sender, EventArgs e)
        {
            string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
            string url = "/DashboardForm/FailurePendingOverview.aspx?Value=NewTCcount&OfficeCode=" + sOfficeCode;
            string s = "window.open('" + url + "','_blank');"; // '_newtab'
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }
        protected void RepairGood_Click(object sender, EventArgs e)
        {
            string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
            string url = "/DashboardForm/FailurePendingOverview.aspx?Value=RepairTCcount&OfficeCode=" + sOfficeCode;
            string s = "window.open('" + url + "','_blank');"; // '_newtab'
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }
        protected void ReleaseGood_Click(object sender, EventArgs e)
        {
            string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
            string url = "/DashboardForm/FailurePendingOverview.aspx?Value=ReleaseTCcount&OfficeCode=" + sOfficeCode;
            string s = "window.open('" + url + "','_blank');"; // '_newtab'
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }
        protected void Faulty_Click(object sender, EventArgs e)
        {
            string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
            string url = "/DashboardForm/FailurePendingOverview.aspx?Value=FaultyTCcount&OfficeCode=" + sOfficeCode;
            string s = "window.open('" + url + "','_blank');"; // '_newtab'
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }
        protected void ScrapTC_Click(object sender, EventArgs e)
        {
            string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
            string url = "/DashboardForm/FailurePendingOverview.aspx?Value=mobileTCcount&OfficeCode=" + sOfficeCode;
            string s = "window.open('" + url + "','_blank');"; // '_newtab'
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }
        protected void lnkCapacityWiseTransformer_Click(object sender, EventArgs e)
        {
            try
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePendingOverview.aspx?RefId=CapacityWise&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','_blank');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void lnkbank_Click(object sender, EventArgs e)
        {
            try
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePendingOverview.aspx?RefId=CapacityWise&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','_blank');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void Capacity125_250_Click(object sender, EventArgs e)
        {
            string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
            string url = "/DashboardForm/FailurePendingOverview.aspx?Value=TC125_250_count&OfficeCode=" + sOfficeCode;
            string s = "window.open('" + url + "','_blank');"; // '_newtab'
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }
        protected void Capacitygreater250_Click(object sender, EventArgs e)
        {
            string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
            string url = "/DashboardForm/FailurePendingOverview.aspx?Value=TCgreater250_count&OfficeCode=" + sOfficeCode;
            string s = "window.open('" + url + "','_blank');"; // '_newtab'
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }
        protected void Capacity25_100_Click(object sender, EventArgs e)
        {
            string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
            string url = "/DashboardForm/FailurePendingOverview.aspx?Value=TC25_100_count&OfficeCode=" + sOfficeCode;
            string s = "window.open('" + url + "','_blank');"; // '_newtab'
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }
        protected void Capacityless25_Click(object sender, EventArgs e)
        {
            string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
            string url = "/DashboardForm/FailurePendingOverview.aspx?Value=TCless25_count&OfficeCode=" + sOfficeCode;
            string s = "window.open('" + url + "','_blank');"; // '_newtab'
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }
        protected void lnkTransformer_Click(object sender, EventArgs e)
        {
            string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
            string url = "/DashboardForm/FailurePendingOverview.aspx?RefId=TCpending&OfficeCode=" + sOfficeCode;
            string s = "window.open('" + url + "','_blank');"; // '_newtab'
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }
        protected void TotalPendingfor_Issue_Click(object sender, EventArgs e)
        {
            string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
            string url = "/DashboardForm/FailurePendingOverview.aspx?Value=TCpending_issue_count&OfficeCode=" + sOfficeCode;
            string s = "window.open('" + url + "','_blank');"; // '_newtab'
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }
        protected void TotalPendingfor_Repair_Click(object sender, EventArgs e)
        {
            string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
            string url = "/DashboardForm/FailurePendingOverview.aspx?Value=TCpending_repair_count&OfficeCode=" + sOfficeCode;
            string s = "window.open('" + url + "','_blank');"; // '_newtab'
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }
        protected void TotalPendingto_Recive_Click(object sender, EventArgs e)
        {
            string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
            string url = "/DashboardForm/FailurePendingOverview.aspx?Value=TCpending_release_count&OfficeCode=" + sOfficeCode;
            string s = "window.open('" + url + "','_blank');"; // '_newtab'
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }

        protected void getbankgooddetails(object sender, EventArgs e)
        {
            string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
            string url = "/DashboardForm/FailurePendingOverview.aspx?Value=TCless25_count&OfficeCode=" + sOfficeCode;
            string s = "window.open('" + url + "','_blank');"; // '_newtab'
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }
        protected void getreleasegooddetails(object sender, EventArgs e)
        {
            string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
            string url = "/DashboardForm/FailurePendingOverview.aspx?Value=TC25_100_count&OfficeCode=" + sOfficeCode;
            string s = "window.open('" + url + "','_blank');"; // '_newtab'
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }

        protected void getrepairgooddetails(object sender, EventArgs e)
        {
            string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
            string url = "/DashboardForm/FailurePendingOverview.aspx?Value=Good&OfficeCode=" + sOfficeCode;
            string s = "window.open('" + url + "','_blank');"; // '_newtab'
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }

        protected void lnkMD_Dashboard_Click(object sender, EventArgs e)
        {
            try
            {
                string url = "/DashboardForm/MdDashboard.aspx";
                string s = "window.open('" + url + "','_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void lnk_Good_click(object sender, EventArgs e)
        {
            try
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePendingOverview.aspx?Value=TcGoodCount&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','_blank');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);

            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void lnk_Release_Good_click(object sender, EventArgs e)
        {
            try
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePendingOverview.aspx?Value=TcReleaseGoodCount&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','_blank');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void lnk_Repair_Good_click(object sender, EventArgs e)
        {
            try
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePendingOverview.aspx?Value=TcRepairGoodCount&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','_blank');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        
    }
}