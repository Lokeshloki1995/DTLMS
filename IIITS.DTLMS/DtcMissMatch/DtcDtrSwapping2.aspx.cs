using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;

namespace IIITS.DTLMS.DtcMissMatch
{
    public partial class DtcDtrSwapping : System.Web.UI.Page
    {
        string strFormCode = "DtcDtrSwapping";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSearchWindow();
            }
        }

        public void LoadSearchWindow()
        {
            try
            {

                //txtDtcCode.Attributes.Add("onblur", "return ValidateDate(" + txtInvoiceDate.ClientID + ");");

                string strQry = string.Empty;
                strQry = "Title=Search and Select DTr Details&";
                strQry += "Query=SELECT \"DT_CODE\",\"DT_NAME\" FROM \"TBLDTCMAST\"";
                strQry += " WHERE \"DT_CODE\" LIKE '" + txtDtcCode.Text + "%'";
                strQry += " AND {0} like %{1}% &";
                strQry += "DBColName=DT_CODE~DT_NAME&";
                strQry += "ColDisplayName=Dtc Code~Dtc Name&";

                strQry = strQry.Replace("'", @"\'");

                btnDtcSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtDtcCode.ClientID + "&btn=" + btnDtcSearch.ClientID + "',520,520," + txtDtcCode.ClientID + ")");


               
                strQry = "Title=Search and Select DTr Details&";
                strQry += "Query=SELECT \"TC_CODE\",TO_CHAR(\"TC_CAPACITY\")\"TC_CAPACITY\" FROM \"TBLTCMASTER\"";
                strQry += " WHERE  \"TC_CODE\" LIKE '" + txtDtrCode.Text + "%'";
                strQry += " AND {0} like %{1}% &";
                strQry += "DBColName=TC_CODE~TC_CAPACITY&";
                strQry += "ColDisplayName=Tc Code~Tc Capacity&";

                strQry = strQry.Replace("'", @"\'");

                btnDtrSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtDtrCode.ClientID + "&btn=" + btnDtrSearch.ClientID + "',520,520," + txtDtrCode.ClientID + ")");


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void btnDtcSearch_Click(object sender, EventArgs e)
        {
            try
            {
                clsDtcMissMatchEntry objDtcMissMatch = new clsDtcMissMatchEntry();
                objDtcMissMatch.sDtcCode = txtDtcCode.Text;
                grdDtcDetails.DataSource = objDtcMissMatch.LoadDtcDetails(objDtcMissMatch);
                grdDtcDetails.DataBind();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void btnDtrSearch_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}