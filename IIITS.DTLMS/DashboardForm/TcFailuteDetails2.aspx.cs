using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;

namespace IIITS.DTLMS.DashboardForm
{
    public partial class TcFailuteDetails : System.Web.UI.Page
    {
        string strFormCode = "FailureDtrDetails";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                else
                {
                    objSession = (clsSession)Session["clsSession"];
                    if (!IsPostBack)
                    {
                        if (Request.QueryString["OfficeCode"] != null && Convert.ToString(Request.QueryString["OfficeCode"]) != "")
                        {
                            hdfOffCode.Value = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["OfficeCode"]));
                        }
                        else
                        {
                            hdfOffCode.Value = objSession.OfficeCode;
                        }

                        LoadFailurePendingDetails();
                    }
                }
                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        private void LoadFailurePendingDetails()
        {
            try
            {
                clsDashboard objDashboard = new clsDashboard();
                DataTable dtLoadDetails = new DataTable();
                dtLoadDetails = objDashboard.LoadFailureDtrDetails(hdfOffCode.Value);
                grdFailureDtrDetails.DataSource = dtLoadDetails;
                grdFailureDtrDetails.DataBind();
                ViewState["FailureDtrDetails"] = dtLoadDetails;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}