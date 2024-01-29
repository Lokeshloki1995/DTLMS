using IIITS.DTLMS.BL;
using IIITS.DTLMS.BL.Dashboard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.DashboardForm
{
    public partial class LatestUpdates : System.Web.UI.Page
    {
        string strFormCode = "LatestUpdates";
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

                if (!IsPostBack)
                {
                    LoadGrid();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
            }
        }
        public void LoadGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                clsLatestUpdates objLatestUpdates = new clsLatestUpdates();
                dt = objLatestUpdates.GetLatestUpdates();
                if (dt.Rows.Count > 0)
                {
                    grdLatestUpdates.DataSource = dt;
                    grdLatestUpdates.DataBind();
                }
                else
                    ShowMsgBox("No Records Found");
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
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

            }
        }
        protected void lknLoginPage_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["FullName"] != null && Session["FullName"].ToString() != "")
                {
                    Response.Redirect("Dashboard.aspx", false);
                }
                else
                {
                    Response.Redirect("Login.aspx", false);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "lknLoginPage_Click");
            }
        }
    }
}