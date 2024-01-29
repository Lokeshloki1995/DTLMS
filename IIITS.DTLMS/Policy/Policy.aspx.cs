using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Text;

namespace IIITS.DTLMS
{
    public partial class Policy : System.Web.UI.Page
    {
        string strFormCode = "ContactUs.aspx";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];

                if (!IsPostBack)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("<span style='font-family: Calibri; font-size: 15pt'>* Note:</span><span style='margin-left:13px; position:absolute' >  </span>");
                    //sb.AppendLine("<span style='font-family: Calibri; font-size: 15pt; margin-left:-10px; position:absolute'>* Note:</span>" +  " Please Select Repairer to popup rates");
                    sb.AppendLine("<span style='margin-left:70px;' > app owner : Idea Infinity IT Solutions </span>");
                    sb.AppendLine("<span style='margin-left:70px;' > User information collected from End User </span>");
                    sb.AppendLine("<span style='margin-left:70px;' > ANNEXURE-3 : Joint Inspection Report (INTERNAL) </span>");
                    sb.AppendLine("<span style='margin-left:70px;' > Data Collected for Transactions </span>");
                    sb.AppendLine("<span style='margin-left:70px;' > No third parties will have access the information </span>");
                    sb.AppendLine("<span style='margin-left:70px;' > Effective Date : 01-12-2018 </span>");
                    lblNote.Text = sb.ToString().Replace(Environment.NewLine, "<br />");


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
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