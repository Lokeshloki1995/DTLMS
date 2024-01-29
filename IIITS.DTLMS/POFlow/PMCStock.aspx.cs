using ClosedXML.Excel;
using IIITS.DTLMS.BL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.POFlow
{
    public partial class PMCStock : System.Web.UI.Page
    {
        clsSession objSession;
        int Zone_code = Convert.ToInt32(ConfigurationManager.AppSettings["Zone_code"]);
        int Circle_code = Convert.ToInt32(ConfigurationManager.AppSettings["Circle_code"]);
        int Division_code = Convert.ToInt32(ConfigurationManager.AppSettings["Division_code"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToString(Session["clsSession"] ?? "").Length == 0)
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                else
                {
                    objSession = (clsSession)Session["clsSession"];

                    txtFromDate.Attributes.Add("readonly", "readonly");
                    txtToDate.Attributes.Add("readonly", "readonly");
                    txtFromDate_CalendarExtender3.EndDate = System.DateTime.Now;
                    txtToDate_CalendarExtender.EndDate = System.DateTime.Now;

                    if (!IsPostBack)
                    {
                        if (CheckAccessRights(Constants.CheckAccessRights.CheckAccessRightsAll))
                        {
                            string stroffCode = string.Empty;
                            if (objSession.OfficeCode.Length <= 2 && objSession.OfficeCode.Length != 0)
                            {
                                stroffCode = clsStoreOffice.GetZone_Circle_Div_Offcode(objSession.OfficeCode, objSession.RoleId).Substring(0, Constants.Circle);
                            }
                            else
                            {
                                stroffCode = objSession.OfficeCode;
                            }
                            if (objSession.RoleId == "5" || objSession.RoleId == "2")
                            {
                                stroffCode = clsStoreOffice.Getofficecode(objSession.OfficeCode);
                            }

                            Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" from \"TBLZONE\"ORDER BY \"ZO_CO_ID\"", "--Select--", cmbZone);
                            Genaral.Load_Combo("SELECT \"SM_CODE\",\"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"SM_STATUS\"='A'", "--Select--", cmbStore);

                            string stroffCode1 = stroffCode;
                            if (stroffCode.Length >= 1)
                            {
                                Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone);
                                stroffCode = stroffCode.Substring(0, Zone_code);
                                cmbZone.Items.FindByValue(stroffCode).Selected = true;
                                cmbZone.Enabled = false;
                                stroffCode = stroffCode1;
                            }
                            if (stroffCode.Length >= 1)
                            {
                                Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" "+
                                    " WHERE \"CM_ZO_ID\"='" + cmbZone.SelectedValue + "'", "--Select--", cmbCircle);

                                if (stroffCode.Length >= 2)
                                {
                                    stroffCode = stroffCode.Substring(0, Circle_code);
                                    cmbCircle.Items.FindByValue(stroffCode).Selected = true;
                                    cmbCircle.Enabled = false;
                                    stroffCode = stroffCode1;

                                }
                            }

                            if (stroffCode.Length >= 2)
                            {
                                Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" "+
                                    " WHERE CAST(\"DIV_CICLE_CODE\" AS TEXT)='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDivision);
                                if (stroffCode.Length >= 3)
                                {
                                    stroffCode = stroffCode.Substring(0, Division_code);
                                    cmbDivision.Items.FindByValue(stroffCode).Selected = true;
                                    cmbDivision.Enabled = false;
                                    stroffCode = stroffCode1;
                                }
                            }
                            if (cmbDivision.SelectedIndex > 0)
                            {
                                Genaral.Load_Combo("SELECT \"SM_CODE\",\"SM_NAME\" FROM \"TBLSTOREMAST\" "+
                                    " WHERE \"SM_STATUS\"='A' and cast(\"SM_CODE\" as TEXT) ='" + stroffCode + "'", "--Select--", cmbStore);
                                cmbStore.Items.FindByValue(stroffCode).Selected = true;
                                cmbStore.Enabled = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }
        protected void cmbZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbZone.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" "+
                        " WHERE \"CM_ZO_ID\"='" + cmbZone.SelectedValue + "'", "--Select--", cmbCircle);
                    Genaral.Load_Combo("SELECT \"SM_CODE\",\"SM_NAME\" FROM \"TBLSTOREMAST\" "+
                        " WHERE \"SM_STATUS\"='A' and cast(\"SM_CODE\" as TEXT) like '" + cmbZone.SelectedValue + "%'", "--Select--", cmbStore);

                    cmbDivision.Items.Clear();
                }
                else
                {
                    cmbCircle.Items.Clear();
                    cmbDivision.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   MethodBase.GetCurrentMethod().DeclaringType.Name,
                   MethodBase.GetCurrentMethod().Name,
                   ex.Message,
                   ex.StackTrace);
            }
        }
        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" "+
                        " WHERE \"DIV_CICLE_CODE\"='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDivision);
                    Genaral.Load_Combo("SELECT \"SM_CODE\",\"SM_NAME\" FROM \"TBLSTOREMAST\" "+
                        " WHERE \"SM_STATUS\"='A' and cast(\"SM_CODE\" as TEXT) like '" + cmbCircle.SelectedValue + "%'", "--Select--", cmbStore);
                }

                else
                {
                    cmbDivision.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   MethodBase.GetCurrentMethod().DeclaringType.Name,
                   MethodBase.GetCurrentMethod().Name,
                   ex.Message,
                   ex.StackTrace);
            }
        }

        protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDivision.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"SM_CODE\",\"SM_NAME\" FROM \"TBLSTOREMAST\" "+
                        " WHERE \"SM_STATUS\"='A' and cast(\"SM_CODE\" as TEXT) like '" + cmbDivision.SelectedValue + "%'", "--Select--", cmbStore);
                }
                else
                {
                    cmbStore.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                   MethodBase.GetCurrentMethod().DeclaringType.Name,
                   MethodBase.GetCurrentMethod().Name,
                   ex.Message,
                   ex.StackTrace);
            }
        }

        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY
                clsApproval objApproval = new clsApproval();
                objApproval.sFormName = MethodBase.GetCurrentMethod().DeclaringType.Name;
                objApproval.sRoleId = objSession.RoleId; // for Admin there is no role.
                objApproval.sAccessType = Constants.CheckAccessRights.CheckAccessRightsAll + "," + sAccessType; //  "1"
                bool bResult = objApproval.CheckAccessRights(objApproval);
                if (bResult == false)
                {
                    Response.Redirect("~/UserRestrict.aspx", false);
                }
                return bResult;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
                return false;

            }
        }
        protected void Reset_Click(object sender, EventArgs e)
        {
            try
            {
                txtFromDate.Text = string.Empty;
                txtToDate.Text = string.Empty;

                if ((objSession.OfficeCode ?? "").Length == 0)
                {
                    cmbZone.SelectedIndex = 0;
                    cmbCircle.Items.Clear();
                    cmbDivision.Items.Clear();
                    cmbStore.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }

        public bool ValidateForm()
        {
            bool bValidate = false;
            string sResult = string.Empty;
            try
            {
                if (txtFromDate.Text != "")
                {
                    sResult = Genaral.DateValidation(txtFromDate.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtFromDate.Focus();
                        return bValidate;
                    }
                }
                if (txtToDate.Text != "")
                {
                    sResult = Genaral.DateValidation(txtToDate.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtToDate.Focus();
                        return bValidate;
                    }
                }
                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    sResult = Genaral.DateComparision(txtToDate.Text, txtFromDate.Text, false, false);
                    if (sResult == "2")
                    {
                        ShowMsgBox("To Date should be Grater than or Equal to From Date.");
                        txtToDate.Focus();
                        return bValidate;
                    }
                    bValidate = true;
                }
                bValidate = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
            return bValidate;
        }
        protected void Export_Click(object sender, EventArgs e)
        {
            clsReports objReport = new clsReports();
            DataTable dt = new DataTable();

            if (ValidateForm())
            {
                if ((txtFromDate.Text ?? "").Length > 0)
                {
                    objReport.sFromDate = txtFromDate.Text.ToString();
                    DateTime DToDate = DateTime.ParseExact(objReport.sFromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");
                }
                if ((txtToDate.Text ?? "").Length > 0)
                {
                    objReport.sTodate = txtToDate.Text.ToString();
                    DateTime DToDate = DateTime.ParseExact(objReport.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
                }

                #region logic
                if (cmbDivision.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbDivision.SelectedValue;
                }
                else if (cmbCircle.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbCircle.SelectedValue;
                }
                else if (cmbZone.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbZone.SelectedValue.ToString();
                }
                else
                {
                    objReport.sOfficeCode = "";
                }

                if (cmbStore.SelectedIndex > 0)
                {
                    objReport.StoreId = clsStoreOffice.GetStoreID(cmbStore.SelectedValue);

                }

                dt = objReport.PMCStockDetails(objReport);

                if (dt.Rows.Count > 0)
                {
                    string[] arrAlpha = Genaral.getalpha();
                    using (XLWorkbook wb = new XLWorkbook())
                    {

                        dt.Columns["sStore_Name"].ColumnName = "Store Name";
                        dt.Columns["sDTr_Code"].ColumnName = "DTr Code";
                        dt.Columns["sCapacity"].ColumnName = "Capacity";
                        dt.Columns["sDTR_Make"].ColumnName = "DTr Make";
                        dt.Columns["sDi_No"].ColumnName = "DI No";
                        dt.Columns["sDi_Date"].ColumnName = "DI Date";
                        dt.Columns["sPo_No"].ColumnName = "PO No";
                        dt.Columns["sPO_Date"].ColumnName = "PO Date";
                        dt.Columns["sDWA_No"].ColumnName = "DWA No";
                        dt.Columns["sIndent_No"].ColumnName = "Indent No";
                        dt.Columns["sIndent_Date"].ColumnName = "Indent Date";
                        dt.Columns["sInvoice"].ColumnName = "Invoice Status";
                        dt.Columns["sPMC_INVOICE_NO"].ColumnName = "Invoice No";
                        dt.Columns["sPMC_INVOICE_DATE"].ColumnName = "Invoice Date";
                        dt.Columns["sTPIE_DTCCODE"].ColumnName = "DTC Code";
                        dt.Columns["sSection_Name"].ColumnName = "Section Name";
                        dt.Columns["Project_Name"].ColumnName = "Project Name";
                        dt.Columns["sLEC_Name"].ColumnName = "LEC Name";

                        dt.Columns["Store Name"].SetOrdinal(0);
                        dt.Columns["DTr Code"].SetOrdinal(1);
                        dt.Columns["Capacity"].SetOrdinal(2);
                        dt.Columns["DTr Make"].SetOrdinal(3);
                        dt.Columns["DI No"].SetOrdinal(4);
                        dt.Columns["DI Date"].SetOrdinal(5);
                        dt.Columns["PO No"].SetOrdinal(6);
                        dt.Columns["PO Date"].SetOrdinal(7);
                        dt.Columns["LEC Name"].SetOrdinal(8);
                        dt.Columns["DWA No"].SetOrdinal(9);
                        dt.Columns["Project Name"].SetOrdinal(10);
                        dt.Columns["Indent No"].SetOrdinal(11);
                        dt.Columns["Indent Date"].SetOrdinal(12);
                        dt.Columns["Invoice Status"].SetOrdinal(13);
                        dt.Columns["Invoice No"].SetOrdinal(14);
                        dt.Columns["Invoice Date"].SetOrdinal(15);
                        dt.Columns["DTC Code"].SetOrdinal(16);
                        dt.Columns["Section Name"].SetOrdinal(17);

                        wb.Worksheets.Add(dt, "Stock Details");

                        wb.Worksheet(1).Row(1).InsertRowsAbove(3);

                        string sMergeRange = arrAlpha[dt.Columns.Count - 1];
                        var rangeheader = wb.Worksheet(1).Range("A1:" + sMergeRange + "1");
                        rangeheader.Merge().Style.Font.SetBold().Font.FontSize = 16;
                        rangeheader.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
                        rangeheader.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        rangeheader.SetValue("Hubli Electricity Supply Company Limited, (HESCOM)");

                        var rangeReporthead = wb.Worksheet(1).Range("A2:" + sMergeRange + "2");
                        rangeReporthead.Merge().Style.Font.SetBold().Font.FontSize = 12;
                        rangeReporthead.Merge().Style.Fill.BackgroundColor = XLColor.AirForceBlue;
                        rangeReporthead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        if (txtFromDate.Text != "" && txtToDate.Text != "")
                        {
                            rangeReporthead.SetValue("List of Stock Details From " + objReport.sFromDate + " To " + objReport.sTodate);
                        }
                        if (txtFromDate.Text != "" && txtToDate.Text == "")
                        {
                            rangeReporthead.SetValue("List of Stock Details From " + objReport.sFromDate + " To " + DateTime.Now);
                        }
                        if (txtFromDate.Text == "" && txtToDate.Text != "")
                        {
                            rangeReporthead.SetValue("List of Stock Details as on " + objReport.sTodate);
                        }
                        if (txtFromDate.Text == "" && txtToDate.Text == "")
                        {
                            rangeReporthead.SetValue("List of Stock Details as on " + DateTime.Now);
                        }
                        wb.Worksheet(1).Cell(3, 9).Value = DateTime.Now;
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "Stock Details";
                        string FileName = "Stock Details " + DateTime.Now + ".xls";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=" + FileName);

                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
                else
                {
                    ShowMsgBox(Convert.ToString(ConfigurationManager.AppSettings["EmptyData"]));
                }
                #endregion
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
                clsException.LogError(
                     MethodBase.GetCurrentMethod().DeclaringType.Name,
                     MethodBase.GetCurrentMethod().Name,
                     ex.Message,
                     ex.StackTrace);
            }
        }
    }
}