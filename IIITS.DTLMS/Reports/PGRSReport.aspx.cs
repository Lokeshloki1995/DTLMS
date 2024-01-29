using IIITS.DTLMS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using ClosedXML.Excel;
using System.Data;
namespace IIITS.DTLMS.Reports
{
    public partial class PGRSReport : System.Web.UI.Page
    {
        clsSession objSession;

        string strFormCode = "PGRSReport";
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
                    objSession = (clsSession)Session["clsSession"];
                    string stroffCode = string.Empty;
                    Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone);
                }

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_ZO_ID\"='" + cmbZone.SelectedValue + "' ORDER BY \"CM_CIRCLE_CODE\" ", "--Select--", cmbCircle);

        }

        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE \"DIV_CICLE_CODE\"='" + cmbCircle.SelectedValue + "'ORDER BY \"DIV_CODE\" ", "--Select--", cmbDiv);

        }

        protected void cmbDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE \"SD_DIV_CODE\"='" + cmbDiv.SelectedValue + "' ORDER BY  \"SD_SUBDIV_CODE\" ", "--Select--", cmbSubDivision);

        }

        protected void cmbSubDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_SUBDIV_CODE\"='" + cmbSubDivision.SelectedValue + "'ORDER BY \"OM_CODE\"", "--Select--", cmbOMSection);
            Genaral.Load_Combo("SELECT \"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" and CAST(\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbSubDivision.SelectedValue + "%'  ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName);
        }

        protected void GenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                string strOfficeCode = string.Empty;
                string strParam = string.Empty;
                clsReports objReport = new clsReports();
                string sResult = string.Empty;

                if(cmbFailStage.SelectedValue == "-1")
                {
                    ShowMsgBox("Please Select any of the stage");
                    cmbFailStage.Focus();
                    return;
                }
                if (txtFromDate.Text != "")
                {
                    sResult = Genaral.DateValidation(txtFromDate.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtFromDate.Focus();
                        return;
                    }
                }
                if (txtToDate.Text != "")
                {
                    sResult = Genaral.DateValidation(txtToDate.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtToDate.Focus();
                        return;
                    }
                }
                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    sResult = Genaral.DateComparision(txtToDate.Text, txtFromDate.Text, false, false);
                    if (sResult == "2")
                    {
                        ShowMsgBox("To Date should be Greater than From Date");
                        txtToDate.Focus();
                        return;

                    }
                }


                if (cmbFeederName.SelectedIndex > 0)
                {
                    objReport.sFeeder = cmbFeederName.SelectedValue;

                }
                if (cmbOMSection.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbOMSection.SelectedValue;
                }
                else if (cmbSubDivision.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbSubDivision.SelectedValue;
                }
                else if (cmbDiv.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbDiv.SelectedValue;
                }
                else if (cmbCircle.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbCircle.SelectedValue;
                }
                else if (cmbZone.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbZone.SelectedValue;
                }
                else objReport.sOfficeCode = "";
                
                
                if (txtFromDate.Text.ToString() != null && txtFromDate.Text.ToString() != "")
                {
                    objReport.sFromDate = txtFromDate.Text.ToString();
                    DateTime DToDate = DateTime.ParseExact(objReport.sFromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");
                }
                if (txtToDate.Text.ToString() != null && txtToDate.Text.ToString() != "")
                {
                    objReport.sTodate = txtToDate.Text.ToString();
                    DateTime DToDate = DateTime.ParseExact(objReport.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
                }
                objReport.sFailureStatus = cmbFailStage.SelectedValue;
                strParam = "id=DtcFailDetails&Officecode=" + objReport.sOfficeCode + "&FeederName=" + objReport.sFeeder + "&FromDate=" + objReport.sFromDate + "&ToDate=" + objReport.sTodate + "&sFailureStatus=" + objReport.sFailureStatus;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
            }
            catch (Exception ex)
            {
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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        } 
        protected void Reset_Click(object sender, EventArgs e)
        {
            try
            {
                cmbFeederName.SelectedIndex = 0;
                txtFromDate.Text = "";
                txtToDate.Text = "";
                cmbZone.SelectedIndex = 0;
                cmbCircle.Items.Clear();
                cmbDiv.Items.Clear();
                cmbSubDivision.Items.Clear();
                cmbOMSection.Items.Clear();
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

    }
}