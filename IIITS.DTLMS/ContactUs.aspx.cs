using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Configuration;

namespace IIITS.DTLMS
{
    public partial class ContactUs : System.Web.UI.Page
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
                   
                   LoadGrid();
                   // LoadDesignationGrid();

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadDesignationGrid()
        {
            try
            {
                DataSet set = new DataSet();
                clsContactUs objDet = new clsContactUs();
              
                set = objDet.LoadDetails();
                for (int i = 1; i <= 3; i++)
                {
                    if (i == 1)
                    {
                        DataTable t1Again = set.Tables[i];
                        grdFirstContactDetails.DataSource = t1Again;
                        grdFirstContactDetails.DataBind();
                    }
                    if (i == 2)
                    {
                        DataTable t2Again = set.Tables[i];
                        grdSecondContactDetails.DataSource = t2Again;
                        grdSecondContactDetails.DataBind();
                    }
                    if (i == 3)
                    {
                        DataTable t3Again = set.Tables[i];
                        grdThirdGrid.DataSource = t3Again;
                        grdThirdGrid.DataBind();
                    }
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void LoadGrid() 
        {
            try
            {
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                DataTable dt3 = new DataTable();
                clsContactUs objDesgn = new clsContactUs();

                dt1 = objDesgn.LoadDetailsForFirstGrid();
                if (dt1.Rows.Count > 0)
                {
                    grdFirstContactDetails.DataSource = dt1;
                    grdFirstContactDetails.DataBind();
                }
                dt2 = objDesgn.LoadDetailsForSecondGrid();
                if (dt2.Rows.Count > 0)
                {

                    grdSecondContactDetails.DataSource = dt2;
                    grdSecondContactDetails.DataBind();
                }
                dt3 = objDesgn.LoadDetailsForThirdGrid();
                if (dt3.Rows.Count > 0)
                {

                    grdThirdGrid.DataSource = dt3;
                    grdThirdGrid.DataBind();
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void lnkOSTicket_Click(object sender, EventArgs e)
        {
            string userId = string.Empty;
            string UserName = string.Empty;
            string LoginName = string.Empty;
            string Email = string.Empty;
            string MONO = string.Empty;
            string PrjId = string.Empty;
            string rolename = string.Empty;
            string circle = string.Empty;
            string division = string.Empty;
            string subdiv = string.Empty;
            string section = string.Empty;
            string officecode = string.Empty;
            string zone = string.Empty;
            string iticketRedirectPath = string.Empty;
            string url = string.Empty;

            var page = HttpContext.Current.Handler as Page;
            objSession = (clsSession)Session["clsSession"];
            clsUser objUsrDetail = new clsUser();
            objUsrDetail.lSlNo = objSession.UserId;
            objUsrDetail.sRoleType = objSession.sRoleType;
            objUsrDetail.GetUserDetails(objUsrDetail);
            if (ConfigurationManager.AppSettings["ITICKETNEWCHANGES"] == "NO")
            {
                userId = Genaral.Encrypt(objSession.UserId);
                UserName = Genaral.Encrypt(objUsrDetail.sFullName);
                Email = Genaral.Encrypt(objUsrDetail.sEmail);
                MONO = Genaral.Encrypt(objUsrDetail.sMobileNo);
                PrjId = Genaral.Encrypt("HESCOMDTLMS");
                rolename = Genaral.Encrypt(objUsrDetail.rolename);
                circle = Genaral.Encrypt(objUsrDetail.circle);
                division = Genaral.Encrypt(objUsrDetail.division);
                subdiv = Genaral.Encrypt(objUsrDetail.subdivision);
                section = Genaral.Encrypt(objUsrDetail.section);
                officecode = Genaral.Encrypt(objUsrDetail.sOfficeCode);
                zone = Genaral.Encrypt(objUsrDetail.zone);

                url = "http://iticket.co.in/Ticket/HescomDTLMSApplication?UserId=" + userId + "&UserName=" +
                   UserName + "&Email=" + Email + "&MobileNumber=" + MONO + "&ProjectId=" + PrjId + "&rolename=" + rolename
                   + "&circle=" + circle + "&division= " + division + "&subdivision= " + subdiv + "&section=" + section + "&officecode=" + officecode + "&zone=" + zone + " ";
            }
            else
            {
                userId = Genaral.EncryptTckt(objSession.UserId,"");
                LoginName = Genaral.EncryptTckt(objUsrDetail.sLoginName, "");
                UserName = Genaral.EncryptTckt(objUsrDetail.sFullName,"");
                Email = Genaral.EncryptTckt(objUsrDetail.sEmail,"");
                MONO = Genaral.EncryptTckt(objUsrDetail.sMobileNo,"");
                PrjId = Genaral.EncryptTckt(Convert.ToString(ConfigurationManager.AppSettings["ITICKETENCRYPTDECRPTKEY"]),"");
                rolename = Genaral.EncryptTckt(objUsrDetail.rolename,"");
                officecode = Genaral.EncryptTckt(objUsrDetail.sOfficeCode,"");


                iticketRedirectPath = ConfigurationManager.AppSettings["Iticket_url_path"].ToString();
                url = iticketRedirectPath + "?UserName=" + LoginName + "&Email=" + Email
                    + "&ProductSuffix=" + PrjId + "&MobileNumber=" + MONO + "&RoleName=" + rolename
                    + "&OfficeCode=" + officecode + "&UserId=" + userId + "&Name=" + UserName + " ";


                // changes done for i ticket application on 21-08-2023
                //iticketRedirectPath = ConfigurationManager.AppSettings["Iticket_url_path"].ToString();
                //url = iticketRedirectPath + "?UserName=" + UserName + "&Email=" + Email
                //    + "&ProductSuffix=" + PrjId + "&MobileNumber=" + MONO + "&RoleName=" + rolename + "&OfficeCode=" + officecode + " ";

                //url = "http://iticket.co.in/Ticket/HescomDTLMSApplication?UserId=" + userId + "&UserName=" +
                //   UserName + "&Email=" + Email + "&MobileNumber=" + MONO + "&ProjectId=" + PrjId + "&rolename=" + rolename
                //   + "&circle=" + circle + "&division= " + division + "&subdivision= " + subdiv + "&section=" + section + "&officecode=" + officecode + "&zone=" + zone + " ";

            }
            #region
            ////Response.Redirect("http://192.168.4.47:999/Ticket/DTLMSApplication?UserId=" + userId + "&UserName=" + UserName + "&Email=" + Email + "&MobileNumber=" + MONO + "&ProjectId=" + PrjId, false);

            //string url = "http://iticket.co.in/Ticket/HescomDTLMSApplication?UserId=" + userId + "&UserName=" +
            //    UserName + "&Email=" + Email + "&MobileNumber=" + MONO + "&ProjectId=" + PrjId + "&rolename=" + rolename
            //    + "&circle=" + circle + "&division= " + division + "&subdivision= " + subdiv + "&section=" + section + "&officecode=" + officecode + "&zone=" + zone + " ";

            ////string url = "http://www.iticket.co.in/Ticket/HescomDTLMSApplication?UserId=" + userId + "&UserName=" + UserName + "&Email=" + Email + "&MobileNumber=" + MONO + "&ProjectId=" + PrjId ;
            #endregion

            string s = "window.open('" + url + "','_blank');"; // '_newtab'
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);

        }

    }
}