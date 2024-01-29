using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;

namespace IIITS.DTLMS.MasterForms
{
    public partial class DtcMapping : System.Web.UI.Page
    {
        string strFormCode = "TcMaster";
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
                lblMessage.Text = string.Empty;

                if (!IsPostBack)
                {
                    if (Request.QueryString["MappingId"] != null && Request.QueryString["MappingId"].ToString() != "")
                    {
                        txtMappingId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["MappingId"]));
                        GetTcMAsterDeatils();
                    }

                 
                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        public void GetTcMAsterDeatils()
        {

            try
            {
                clsDtcMapping objDtcMapping = new clsDtcMapping();
                objDtcMapping.sMappingId = txtMappingId.Text;

                objDtcMapping.GetDTCDetails(objDtcMapping);

                txtMappingId.Text = objDtcMapping.sMappingId;
                txtTcCode.Text = objDtcMapping.sTcCode;
                txtSerialNo.Text = objDtcMapping.sTcSlNo;
                txtMake.Text = objDtcMapping.sTcMakeId;
                txtTcCapacity.Text = objDtcMapping.sTcCapacity;
                txtMappingDate.Text = objDtcMapping.sMappingDate;
                txtDTCCode.Text = objDtcMapping.sDtcCode;
                txtDTCName.Text = objDtcMapping.sDtcName;
                hdfDTCId.Value = objDtcMapping.sDTCId;
                hdfTCId.Value = objDtcMapping.sTCId;

                txtTcCode.Enabled = false;

                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void lnkDTrDetails_Click(object sender, EventArgs e)
        {
            try
            {
                string sTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfTCId.Value));
                Response.Redirect("/MasterForms/TcMaster.aspx?TCId=" + sTCId, false);

               
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void lnkDTCDetails_Click(object sender, EventArgs e)
        {
            try
            {
                string sDTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfDTCId.Value));

                Response.Redirect("/MasterForms/DTCCommision.aspx?QryDtcId=" + sDTCId, false);

              
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
       
    }
}