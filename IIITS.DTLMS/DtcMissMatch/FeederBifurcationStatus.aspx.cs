using IIITS.DTLMS.BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.DtcMissMatch
{
    public partial class FeederBifurcationStatus : System.Web.UI.Page
    {
        string strFormCode = "FeederBifurcationStatus";
        clsSession objSession;
        string sDTCCodes = string.Empty;
        
        public void Page_Load(object sender, EventArgs e)
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
                    if (Request.QueryString["DTCCodes"] != null && Request.QueryString["DTCCodes"].ToString() != "")
                    {
                        sDTCCodes = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["OfficeCode"]));
                    }
                   

                    LoadDTCDetails();
                }
            }
            catch (Exception ex)
            {
               // lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
            }
        }

        public void LoadDTCDetails()
        {
            try
            {

            }
            catch(Exception ex)
            {

            }
        }
    }
}