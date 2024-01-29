using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;


namespace IIITS.DTLMS.Maintainance
{
    public partial class PrevMaintanance : System.Web.UI.Page
    {
        string strFormCode = "PrevMaintanance";
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
                    lblMessage.Text = string.Empty;
                    objSession = (clsSession)Session["clsSession"];
                    if (!IsPostBack)
                    {

                        if (objSession.OfficeCode.Length >= 4)
                        {
                            objSession.OfficeCode = objSession.OfficeCode.Substring(0, 3);
                        }

                        string strQry = "SELECT \"FD_FEEDER_CODE\",\"FD_FEEDER_CODE\" || '-' || \"FD_FEEDER_NAME\"  FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" ";
                        strQry += " WHERE  \"FD_FEEDER_ID\" = \"FDO_FEEDER_ID\" AND (CAST(\"FDO_OFFICE_CODE\" AS TEXT) LIKE '" + objSession.OfficeCode + "%' ";
                        strQry += " OR \"FDO_OFFICE_CODE\" IS NULL) ORDER BY \"FD_FEEDER_CODE\"";
                        Genaral.Load_Combo(strQry, "-Select-", cmbIndexSelection);
                        //LoadPrevMaintainanceDetails();
                    }
                }
                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }         
        }

        public void LoadPrevMaintainanceDetails(string strFeederId)
        {

            string strId = string.Empty;
            try
            {
                clsTcMaintainance objDetails = new clsTcMaintainance();
                objDetails.sFeederId = strFeederId;
                objDetails.sOfficeCode = objSession.OfficeCode;
                DataTable dtLocDetails = objDetails.LoadPrevMaintainance(objDetails);
                grdPrevMaintainance.DataSource = dtLocDetails;
                grdPrevMaintainance.DataBind();              
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdPrevMaintainance_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
               
                if (e.CommandName == "Maintainance")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);                  
                    string strDtcCode = ((Label)row.FindControl("lblDtcCode")).Text;                  
                  
                    strDtcCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(strDtcCode));
                    Response.Redirect("TcMaintainance.aspx?DtcCode=" + strDtcCode + "", false);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbIndexSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {               
                LoadPrevMaintainanceDetails(cmbIndexSelection.SelectedValue);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

    }
}