using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;

namespace IIITS.DTLMS.Reports
{
    public partial class EnumReport : System.Web.UI.Page
    {
        string strFormCode = "EnumerationReport";
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
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");
                CalendarExtender2.EndDate = System.DateTime.Now;
                CalendarExtender1.EndDate = System.DateTime.Now;
                if (!IsPostBack)
                {
                    Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone);
                    gridexcel.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbZone.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_ZO_ID\"='" + cmbZone.SelectedValue + "'", "--Select--", cmbCircle);
                    cmbDiv.Items.Clear();
                    cmbSubDiv.Items.Clear();
                    cmbSection.Items.Clear();
                }
                else
                {
                    cmbCircle.Items.Clear();
                    cmbDiv.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CICLE_CODE\" AS TEXT)='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDiv);
                    cmbSubDiv.Items.Clear();
                    cmbSection.Items.Clear();
                }
                else
                {
                    cmbDiv.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDiv.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT  DISTINCT \"SD_SUBDIV_CODE\", \"SD_SUBDIV_CODE\" || '-' || \"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE \"SD_DIV_CODE\"='" + cmbDiv.SelectedValue + "' ORDER BY \"SD_SUBDIV_CODE\"", "--Select--", cmbSubDiv);
                    string strQry = "SELECT  DISTINCT \"FD_FEEDER_CODE\",\"FD_FEEDER_CODE\" || '-' || \"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\", \"TBLFEEDEROFFCODE\" WHERE  CAST(\"FD_FEEDER_ID\" AS TEXT) = CAST(\"FDO_FEEDER_ID\" AS TEXT) AND";
                    strQry += " CAST(\"FDO_OFFICE_CODE\" AS TEXT) LIKE '" + cmbDiv.SelectedValue + "%' ORDER BY \"FD_FEEDER_CODE\"";
                    Genaral.Load_Combo(strQry, "--All--", cmbFeeder);
                    cmbSection.Items.Clear();
                }
                else
                {
                    cmbSubDiv.Items.Clear();
                    cmbSection.Items.Clear();
                    cmbFeeder.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbType.SelectedValue == "1")
                {
                    cmbSubDiv.Enabled = false;
                    cmbSection.Enabled = false;
                    cmbFeeder.Enabled = false;
                }
                else
                {
                    cmbSubDiv.Enabled = true;
                    cmbSection.Enabled = true;
                    cmbFeeder.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void cmbSubDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubDiv.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"OM_CODE\", \"OM_CODE\" || '-' || \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_SUBDIV_CODE\" ='" + cmbSubDiv.SelectedValue + "' ORDER BY \"OM_CODE\"", "--Select--", cmbSection);
                    string strQry = "SELECT DISTINCT \"FD_FEEDER_CODE\",\"FD_FEEDER_CODE\" || '-' || \"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\", \"TBLFEEDEROFFCODE\" WHERE  CAST(\"FD_FEEDER_ID\" AS TEXT) = CAST(\"FDO_FEEDER_ID\" AS TEXT) AND";
                    strQry += " CAST(\"FDO_OFFICE_CODE\" AS TEXT) LIKE '" + cmbDiv.SelectedValue + "%' ORDER BY \"FD_FEEDER_CODE\"";
                    Genaral.Load_Combo(strQry, "--All--", cmbFeeder);
                }
                else
                {
                    cmbSection.Items.Clear();
                    cmbFeeder.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmpReport_Click(object sender, EventArgs e)
        {
            try
            {
                string sResult = string.Empty;
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
                string strOfficeCode = string.Empty;
                if (cmbType.SelectedIndex == 0)
                {
                    ShowMsgBox("Select Location Type");
                    return;
                }
                if (cmbType.SelectedValue == "1")
                {
                    if (cmbDiv.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbDiv.SelectedValue;
                    }
                    else if (cmbCircle.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbCircle.SelectedValue;
                    }
                    else if (cmbZone.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbZone.SelectedValue;
                    }
                    //if (txtToDate.Text == "")
                    //{
                    //    txtToDate.Text = txtFromDate.Text;
                    //}
                    if (cmbdatewise.SelectedIndex == 0)
                    {
                        ShowMsgBox("Select the Report");
                        return;
                    }
                    strOfficeCode = "id=StoreLoc&OfficeCode=" + strOfficeCode + "&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text + "&Datewise=" + cmbdatewise.SelectedValue;
                    RegisterStartupScript("Print", "<script>window.open('ReportView.aspx?" + strOfficeCode + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                }

                else if (cmbType.SelectedValue == "2")
                {
                    //if (cmbDiv.SelectedIndex == 0)
                    //{
                    //    ShowMsgBox("Please Select Division Name");
                    //    return;
                    //}

                    if (cmbdatewise.SelectedIndex == 0)
                    {
                        ShowMsgBox("Please Select the Report");
                        return;
                    }
                    if (cmbZone.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbZone.SelectedValue;
                    }
                    if (cmbCircle.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbCircle.SelectedValue;
                    }
                    if (cmbDiv.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbDiv.SelectedValue;
                    }
                    if (cmbSubDiv.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbSubDiv.SelectedValue;
                    }
                    if (cmbSection.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbSection.SelectedValue;
                    }
                    string sFeederCode = string.Empty;

                    if (cmbFeeder.SelectedIndex > 0)
                    {
                        sFeederCode = cmbFeeder.SelectedValue;
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = txtFromDate.Text;
                    }
                    strOfficeCode = "id=FieldLoc&OfficeCode=" + strOfficeCode + "&sFeeder=" + sFeederCode + "&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text + "&subdiv=" + cmbSubDiv.SelectedValue + "&Datewise=" + cmbdatewise.SelectedValue;
                    RegisterStartupScript("Print", "<script>window.open('ReportView.aspx?" + strOfficeCode + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                }

                else if (cmbType.SelectedValue == "2")
                {
                    if (cmbDiv.SelectedIndex == 0)
                    {
                        ShowMsgBox("Please Select Division Name");
                        return;
                    }
                    if (cmbSubDiv.SelectedIndex == 0)
                    {
                        ShowMsgBox("Select Sub division Name");
                        return;
                    }
                    if (cmbSection.SelectedIndex == 0)
                    {
                        ShowMsgBox("Select Section Name");
                        return;
                    }
                    if (cmbdatewise.SelectedIndex == 0)
                    {
                        ShowMsgBox("Select the Report");
                        return;
                    }
                    clsInterDashboard obj = new clsInterDashboard();
                    bool status = obj.CheckFeederCompletion(cmbSection.SelectedValue);
                    if (status == true)
                    {
                        ShowMsgBox("Unable to Generate, DTC Pending for Approval");
                        return;
                    }
                    if (cmbZone.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbZone.SelectedValue;
                    }
                    if (cmbCircle.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbCircle.SelectedValue;
                    }
                    if (cmbDiv.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbDiv.SelectedValue;
                    }
                    if (cmbSubDiv.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbSubDiv.SelectedValue;
                    }
                    if (cmbSection.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbSection.SelectedValue;
                    }
                    string sFeederCode = string.Empty;

                    if (cmbFeeder.SelectedIndex > 0)
                    {
                        sFeederCode = cmbFeeder.SelectedValue;
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = txtFromDate.Text;
                    }
                    strOfficeCode = "id=FieldLoc&OfficeCode=" + strOfficeCode + "&sFeeder=" + sFeederCode + "&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text + "&subdiv=" + cmbSubDiv.SelectedValue + "&Datewise=" + cmbdatewise.SelectedValue;
                    RegisterStartupScript("Print", "<script>window.open('ReportView.aspx?" + strOfficeCode + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmdDtrExport_Click(object sender, EventArgs e)
        {
            try
            {
                clsReports objreport = new clsReports();
                DataTable dtDetails = new DataTable();
                string sResult = string.Empty;
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
                string strOfficeCode = string.Empty;
                if (cmbType.SelectedIndex == 0)
                {
                    ShowMsgBox("Select Location Type");
                    return;
                }

                if (cmbType.SelectedValue == "1")
                {
                    if (cmbSection.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbSection.SelectedValue;
                    }
                    else if (cmbSubDiv.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbSubDiv.SelectedValue;
                    }
                    else if (cmbDiv.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbDiv.SelectedValue;
                    }
                    else if (cmbCircle.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbCircle.SelectedValue;
                    }
                    else if (cmbZone.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbZone.SelectedValue;
                    }
                    else
                    {
                        strOfficeCode = "";
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = txtFromDate.Text;
                    }
                    if (cmbdatewise.SelectedIndex == 0)
                    {
                        ShowMsgBox("Select the Report");
                        return;
                    }
                    dtDetails = objreport.PrintStoreDetails(strOfficeCode, txtFromDate.Text.Trim(), txtToDate.Text, cmbdatewise.SelectedValue);
                    if (dtDetails.Rows.Count > 0)
                    {
                        dtDetails.Columns["SM_NAME"].ColumnName = "STORE NAME";
                        dtDetails.Columns["ENUM_TYPE"].ColumnName = "ENUMARATION TYPE";
                        dtDetails.Columns["DTE_TC_CODE"].ColumnName = "DTR CODE";
                        dtDetails.Columns["DTE_TC_SLNO"].ColumnName = "DTR SLNO";
                        dtDetails.Columns["DTE_CAPACITY"].ColumnName = "CAPACITY (in KVA)";
                        dtDetails.Columns["DTE_MAKE"].ColumnName = "MAKE NAME";
                        dtDetails.Columns["DIVISION"].ColumnName = "DIVISION";
                        dtDetails.Columns["DTE_TC_MANFDATE"].ColumnName = "MANUFACTURE DATE";
                        dtDetails.Columns["DTE_TANK_CAPACITY"].ColumnName = "TANK CAPACITY";
                        dtDetails.Columns["DTE_TC_WEIGHT"].ColumnName = "TC WEIGHT";
                        dtDetails.Columns["ED_WELD_DATE"].ColumnName = "ENUMARATION DATE";

                        List<string> listtoRemove = new List<string> { "FEEDER", "SUBDIVISION", "SECTION" };
                        string filename = "StoreEnumDetails" + DateTime.Now + ".xls";
                        string pagetitle = "DTC Details";
                        Genaral.getexcel(dtDetails, listtoRemove, filename, pagetitle);
                    }
                    else
                    {
                        ShowMsgBox("No Records Found..");
                        return;
                    }
                }
                else if (cmbType.SelectedValue == "2")
                {
                    if (cmbdatewise.SelectedIndex == 0)
                    {
                        ShowMsgBox("Please Select the Report");
                        return;
                    }
                    if (cmbSection.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbSection.SelectedValue;
                    }
                    else if (cmbSubDiv.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbSubDiv.SelectedValue;
                    }
                    else if (cmbDiv.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbDiv.SelectedValue;
                    }
                    else if (cmbCircle.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbCircle.SelectedValue;
                    }
                    else if (cmbZone.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbZone.SelectedValue;
                    }
                    else
                    {
                        strOfficeCode = "";
                    }
                    string sFeederCode = string.Empty;
                    if (cmbFeeder.SelectedIndex > 0)
                    {
                        sFeederCode = cmbFeeder.SelectedValue;
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = txtFromDate.Text;
                    }
                    dtDetails = objreport.PrintFieldDetails(sFeederCode, strOfficeCode, txtFromDate.Text.Trim(), txtToDate.Text, cmbdatewise.SelectedValue);

                    if (dtDetails.Rows.Count > 0)
                    {
                        dtDetails.Columns["DTE_DTCCODE"].ColumnName = "DTC CODE";
                        dtDetails.Columns["DTE_NAME"].ColumnName = "DTC NAME";
                        dtDetails.Columns["DTE_TC_CODE"].ColumnName = "DTR CODE";
                        dtDetails.Columns["DTE_TC_SLNO"].ColumnName = "DTR SLNO";
                        dtDetails.Columns["DTE_CAPACITY"].ColumnName = "CAPACITY (in KVA)";
                        dtDetails.Columns["DTE_MAKE"].ColumnName = "MAKE NAME";
                        dtDetails.Columns["FEEDER"].ColumnName = "FEEDER NAME";
                        dtDetails.Columns["ED_FEEDERCODE"].ColumnName = "FEEDER CODE";

                        dtDetails.Columns["DIVISION"].ColumnName = "DIVISION";
                        dtDetails.Columns["SUBDIVISION"].ColumnName = "SUB DIVISION";
                        dtDetails.Columns["SECTION"].ColumnName = "O&M SECTION";
                        dtDetails.Columns["DTE_TC_MANFDATE"].ColumnName = "MANUFACTURE DATE";
                        dtDetails.Columns["DTE_TANK_CAPACITY"].ColumnName = "TANK CAPACITY";
                        dtDetails.Columns["DTE_TC_WEIGHT"].ColumnName = "TC WEIGHT";
                        dtDetails.Columns["ED_WELD_DATE"].ColumnName = "ENUMARATION DATE";

                        List<string> listtoRemove = new List<string> { "DTE_CESCCODE", "DTE_IPCODE" };
                        string filename = "FieldEnumDetails" + DateTime.Now + ".xls";
                        string pagetitle = "DTC Details";

                        Genaral.getexcel(dtDetails, listtoRemove, filename, pagetitle);
                    }
                    else
                    {
                        ShowMsgBox("No Records Found..");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                //lblMessage.Text = clsException.ErrorMsg();
                //clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
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


        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {

                if (cmbType.SelectedIndex == 1)
                {
                    cmbType.ClearSelection();
                    cmbZone.ClearSelection();
                    cmbCircle.ClearSelection();
                    cmbDiv.ClearSelection();
                    txtFromDate.Text = string.Empty;
                    txtToDate.Text = string.Empty;
                    cmbdatewise.ClearSelection();

                }
                else
                {
                    cmbType.ClearSelection();
                    cmbZone.ClearSelection();
                    cmbCircle.ClearSelection();
                    cmbDiv.ClearSelection();
                    cmbSubDiv.ClearSelection();
                    cmbSection.ClearSelection();
                    cmbFeeder.ClearSelection();
                    cmbdatewise.ClearSelection();
                    txtFromDate.Text = string.Empty;
                    txtToDate.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void gridexcel_Click(object sender, EventArgs e)
        {
            DataTable dtAbstractDTrDetails = new DataTable();
            clsReports objReport = new clsReports();
            string strOfficeCode = string.Empty;
            try
            {
                if (cmbType.SelectedValue == "1")
                {
                    if (cmbZone.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbZone.SelectedValue;
                    }
                    if (cmbCircle.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbCircle.SelectedValue;
                    }
                    if (cmbDiv.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbDiv.SelectedValue;
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = txtFromDate.Text;
                    }

                    if (cmbdatewise.SelectedIndex == 0)
                    {
                        ShowMsgBox("Select the Report");
                        return;
                    }
                    dtAbstractDTrDetails = objReport.PrintStoreDetailsabstractCount(strOfficeCode, txtFromDate.Text.Trim(), txtToDate.Text, cmbdatewise.SelectedValue);


                    if (dtAbstractDTrDetails.Rows.Count > 1)
                    {
                        dtAbstractDTrDetails.Columns["OFF_NAME"].ColumnName = "STORE NAME";
                        dtAbstractDTrDetails.Columns["DTE_TC_CODE"].ColumnName = "TC COUNT";

                        List<string> listtoRemove = new List<string> { "" };
                        string filename = "StoreEnumAbstractDetailsCount" + DateTime.Now + ".xls";
                        string pagetitle = "DTC Details Store Count";

                        Genaral.getexcel(dtAbstractDTrDetails, listtoRemove, filename, pagetitle);
                    }
                    else
                    {
                        ShowMsgBox("No Records Found..");
                        return;
                    }

                }
                else
                {
                    if (cmbZone.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbZone.SelectedValue;
                    }
                    if (cmbCircle.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbCircle.SelectedValue;
                    }
                    if (cmbDiv.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbDiv.SelectedValue;
                    }
                    if (cmbSubDiv.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbSubDiv.SelectedValue;
                    }
                    if (cmbSection.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbSection.SelectedValue;
                    }
                    string sFeederCode = string.Empty;
                    if (cmbFeeder.SelectedIndex > 0)
                    {
                        sFeederCode = cmbFeeder.SelectedValue;
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = txtFromDate.Text;
                    }
                    dtAbstractDTrDetails = objReport.PrintFieldDetailsAbstractCount(sFeederCode, strOfficeCode, txtFromDate.Text.Trim(), txtToDate.Text, cmbdatewise.SelectedValue);
                    if (dtAbstractDTrDetails.Rows.Count == 1)
                    {
                        ShowMsgBox("No Records Found..");
                    }



                    if (dtAbstractDTrDetails.Rows.Count > 1)
                    {
                        dtAbstractDTrDetails.Columns["DIV"].ColumnName = "DIV NAME";
                        dtAbstractDTrDetails.Columns["SUB_DIV"].ColumnName = "SUBDIV COUNT";
                        dtAbstractDTrDetails.Columns["SECTION"].ColumnName = "SECTION NAME";
                        dtAbstractDTrDetails.Columns["DTE_TC_CODE"].ColumnName = "TC COUNT";

                        List<string> listtoRemove = new List<string> { "" };
                        string filename = "FieldEnumAbstractDetailsCount" + DateTime.Now + ".xls";
                        string pagetitle = "DTC Details Field Count";

                        Genaral.getexcel(dtAbstractDTrDetails, listtoRemove, filename, pagetitle);

                    }
                    else
                    {
                        ShowMsgBox("No Records Found..");
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void cmdDtrAbstract_Click(object sender, EventArgs e)
        {
            clsReports objReport = new clsReports();
            DataTable dtAbstractDTrDetails = new DataTable();
            try
            {
                gridexcel.Visible = true;
                string sResult = string.Empty;
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
                string strOfficeCode = string.Empty;

                if (cmbType.SelectedIndex == 0)
                {
                    ShowMsgBox("Select Location Type");
                    return;
                }

                if (cmbType.SelectedValue == "1")
                {
                    if (cmbZone.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbZone.SelectedValue;
                    }
                    if (cmbCircle.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbCircle.SelectedValue;
                    }
                    if (cmbDiv.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbDiv.SelectedValue;
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = txtFromDate.Text;
                    }

                    if (cmbdatewise.SelectedIndex == 0)
                    {
                        ShowMsgBox("Select the Report");
                        return;
                    }
                    dtAbstractDTrDetails = objReport.PrintStoreDetailsabstract(strOfficeCode, txtFromDate.Text.Trim(), txtToDate.Text, cmbdatewise.SelectedValue);
                    ViewState["AbstractDTrDetails"] = dtAbstractDTrDetails;
                    grdstoreabstract.DataSource = dtAbstractDTrDetails;

                    double totalSalary = 0;
                    foreach (DataRow dr in dtAbstractDTrDetails.Rows)
                    {
                        totalSalary += Convert.ToInt32(dr["DTE_TC_CODE"]);
                    }

                    //--- Here 3 is the number of column where you want to show the total.  
                    grdstoreabstract.Columns[0].FooterText = "Total";
                    grdstoreabstract.Columns[0].FooterStyle.ForeColor = System.Drawing.Color.Black;
                    grdstoreabstract.Columns[0].FooterStyle.HorizontalAlign = HorizontalAlign.Left;
                    grdstoreabstract.Columns[0].FooterStyle.Font.Bold = true;
                    grdstoreabstract.Columns[1].FooterText = totalSalary.ToString();

                    grdstoreabstract.DataBind();

                    grdstoreabstract.Visible = true;
                    grdAbstractDtrDetails.Visible = false;

                }

                else if (cmbType.SelectedValue == "2")
                {
                    if (cmbdatewise.SelectedIndex == 0)
                    {
                        ShowMsgBox("Please Select the Report");
                        return;
                    }

                    if (cmbZone.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbZone.SelectedValue;
                    }
                    if (cmbCircle.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbCircle.SelectedValue;
                    }
                    if (cmbDiv.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbDiv.SelectedValue;
                    }
                    if (cmbSubDiv.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbSubDiv.SelectedValue;
                    }
                    if (cmbSection.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbSection.SelectedValue;
                    }

                    string sFeederCode = string.Empty;

                    if (cmbFeeder.SelectedIndex > 0)
                    {
                        sFeederCode = cmbFeeder.SelectedValue;
                    }

                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = txtFromDate.Text;
                    }
                    dtAbstractDTrDetails = objReport.PrintFieldDetailsAbstract(sFeederCode, strOfficeCode, txtFromDate.Text.Trim(), txtToDate.Text, cmbdatewise.SelectedValue);
                    ViewState["AbstractDTrDetails"] = dtAbstractDTrDetails;
                    grdAbstractDtrDetails.DataSource = dtAbstractDTrDetails;
                    if (dtAbstractDTrDetails.Rows.Count == 0)
                    {
                        ShowMsgBox("No Record Found");
                    }

                    double totalSalary = 0;
                    foreach (DataRow dr in dtAbstractDTrDetails.Rows)
                    {
                        totalSalary += Convert.ToInt32(dr["DTE_TC_CODE"]);
                    }

                    //--- Here 3 is the number of column where you want to show the total.  
                    grdAbstractDtrDetails.Columns[3].FooterText = totalSalary.ToString();

                    grdAbstractDtrDetails.Columns[0].FooterText = "Total";
                    grdAbstractDtrDetails.Columns[0].FooterStyle.ForeColor = System.Drawing.Color.Black;
                    grdAbstractDtrDetails.Columns[0].FooterStyle.HorizontalAlign = HorizontalAlign.Left;
                    grdAbstractDtrDetails.Columns[0].FooterStyle.Font.Bold = true;
                    grdAbstractDtrDetails.Columns[3].FooterText = totalSalary.ToString();

                    grdAbstractDtrDetails.DataBind();

                    grdAbstractDtrDetails.Visible = true;
                    grdstoreabstract.Visible = false;
                }

                else if (cmbType.SelectedValue == "2")
                {
                    if (cmbdatewise.SelectedIndex == 0)
                    {
                        ShowMsgBox("Select the Report");
                        return;
                    }

                    clsInterDashboard obj = new clsInterDashboard();
                    bool status = obj.CheckFeederCompletion(cmbSection.SelectedValue);
                    if (status == true)
                    {
                        ShowMsgBox("Unable to Generate, DTC Pending for Approval");
                        return;
                    }
                    if (cmbDiv.SelectedIndex > 0)
                    {
                        strOfficeCode = cmbDiv.SelectedValue;
                    }

                    string sFeederCode = string.Empty;

                    if (cmbFeeder.SelectedIndex > 0)
                    {
                        sFeederCode = cmbFeeder.SelectedValue;
                    }

                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = txtFromDate.Text;
                    }
                    dtAbstractDTrDetails = objReport.PrintFieldDetailsAbstract(sFeederCode, strOfficeCode, txtFromDate.Text.Trim(), txtToDate.Text, cmbdatewise.SelectedValue);
                    ViewState["AbstractDTrDetails"] = dtAbstractDTrDetails;
                    grdAbstractDtrDetails.DataSource = dtAbstractDTrDetails;

                    double totalSalary = 0;
                    foreach (DataRow dr in dtAbstractDTrDetails.Rows)
                    {
                        totalSalary += Convert.ToInt32(dr["DTE_TC_CODE"]);
                    }

                    //--- Here 3 is the number of column where you want to show the total.  
                    grdAbstractDtrDetails.Columns[1].FooterText = totalSalary.ToString();

                    grdAbstractDtrDetails.Columns[0].FooterText = "Total";
                    grdAbstractDtrDetails.Columns[0].FooterStyle.ForeColor = System.Drawing.Color.Black;
                    grdAbstractDtrDetails.Columns[0].FooterStyle.HorizontalAlign = HorizontalAlign.Left;
                    grdAbstractDtrDetails.Columns[0].FooterStyle.Font.Bold = true;
                    grdAbstractDtrDetails.Columns[1].FooterText = totalSalary.ToString();
                    grdAbstractDtrDetails.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


    }
}