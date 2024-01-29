using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.IO;
using ClosedXML.Excel;
using System.Data;

namespace IIITS.DTLMS.Reports
{
    public partial class DTCAddDetails : System.Web.UI.Page
    {
        string strFormCode = "DTCAddDetails";
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
                //CalendarExtender3.EndDate = System.DateTime.Now.AddDays(0);
                //CalendarExtender4.EndDate = System.DateTime.Now.AddDays(0);
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");

                CalendarExtender4.EndDate = System.DateTime.Now;
                CalendarExtender3.EndDate = System.DateTime.Now;

                if (!IsPostBack)
                {
                    Genaral.Load_Combo("SELECT \"FT_NAME\",\"FT_NAME\" FROM \"TBLFDRTYPE\"", "--Select--", cmbFeederType);
                    Genaral.Load_Combo("SELECT \"MD_ORDER_BY\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='C' ORDER BY \"MD_ORDER_BY\"", "--Select--", cmbCapacity);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                string strOfficeCode = string.Empty;
                string strParam = string.Empty;

                clsReports objReport = new clsReports();
                string sResult = string.Empty;
                if (txtFromDate.Text != "" )
                {
                    sResult = Genaral.DateValidation(txtFromDate.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtFromDate.Focus();
                        return;
                    }

                }
                if ( txtToDate.Text != "")
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

                objReport.sFromDate = txtFromDate.Text;
                objReport.sTodate = txtToDate.Text;
                objReport.sType = rdbReportType.SelectedValue.ToString();
                if (cmbFeederType.SelectedItem.Text != "--Select--")
                {
                    objReport.sFeederType = cmbFeederType.SelectedValue;
                }
                if (cmbCapacity.SelectedItem.Text != "--Select--")
                {
                    objReport.sCapacity = cmbCapacity.SelectedValue;
                }
                //if (cmbCapacity.SelectedIndex > 15)
                //{
                //    objReport.sCapacity = cmbCapacity.SelectedValue;
                //    objReport.sGreaterVal = "TRUE";
                //}
                //else objReport.sFeederType = "";

                strParam = "id=DTCAddDetails&OfficeCode=" + objSession.OfficeCode + "&FromDate=" + objReport.sFromDate + "&ToDate=" + objReport.sTodate + "&Type=" + objReport.sType + "&FeederType=" + objReport.sFeederType+"&Capacity="+objReport.sCapacity;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
            }
            catch (Exception ex)

            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            txtFromDate.Text = "";
            txtToDate.Text = "";
            cmbFeederType.SelectedIndex = 0;
            cmbCapacity.SelectedIndex = 0;
        }
        protected void Export_click(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            clsReports objReport = new clsReports();
            string sResult = string.Empty;

            if (txtFromDate.Text != "" )
            {
                 sResult = Genaral.DateValidation(txtFromDate.Text);
                if (sResult != "")
                {
                    ShowMsgBox(sResult);
                    txtFromDate.Focus();
                    return;
                }
            }
            if ( txtToDate.Text != "")
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
            //objReport.sFromDate = txtFromDate.Text;
            //objReport.sTodate = txtToDate.Text;
            objReport.sType = rdbReportType.SelectedValue.ToString();

            if (txtFromDate.Text != null && txtFromDate.Text != "")
            {
                objReport.sFromDate = txtFromDate.Text;
            }
            if (txtToDate.Text != null && txtToDate.Text != "")
            {
                objReport.sTodate = txtToDate.Text;
            }

            if (cmbFeederType.SelectedItem.Text != "--Select--")
            {
                objReport.sFeederType = cmbFeederType.SelectedValue;
            }
            if (cmbCapacity.SelectedItem.Text != "--Select--")
            {
                objReport.sCapacity = cmbCapacity.SelectedValue;
                objReport.sGreaterVal = "TRUE";
            }

            dt = objReport.DTCAddedReport(objReport);

            if (dt.Rows.Count > 0)
            {
                string[] arrAlpha = Genaral.getalpha();

              
                using (XLWorkbook wb = new XLWorkbook())
                {


                    dt.Columns["COUNT"].ColumnName = "Count";
                    dt.Columns["FD_DTC_CAPACITY"].ColumnName = "Tc Capacity";
                    dt.Columns["SUBDIVOFF"].ColumnName = "SubDiv Off Code";
                    
                   // dt.Columns["SUBDIVISION"].ColumnName = "SubDivision";
                    dt.Columns["FT_ID"].ColumnName = "Feeder ID";
                    dt.Columns["FT_NAME"].ColumnName = "Feeder Type";
                   // dt.Columns["CIRCLE"].ColumnName = "Circle";

                    dt.Columns["CIRCLE"].SetOrdinal(0);
                    dt.Columns["CIRCLE"].ColumnName = "Circle";
                    dt.Columns["SUBDIVISION"].SetOrdinal(2);
                    dt.Columns["SUBDIVISION"].ColumnName = "SubDivision";
                    dt.Columns["COUNT"].SetOrdinal(3);
                    dt.Columns["COUNT"].ColumnName = "Count";
                    dt.Columns["DIVISION"].SetOrdinal(1);
                    dt.Columns["DIVISION"].ColumnName = "Division";
                    dt.Columns.Remove("FROMDATE");
                    dt.Columns.Remove("TODATE");
                    dt.Columns.Remove("TODAY");


                    wb.Worksheets.Add(dt, "DTCAddDetails");

                    wb.Worksheet(1).Row(1).InsertRowsAbove(3);
                    string sMergeRange = arrAlpha[dt.Columns.Count - 1];

                    var rangehead = wb.Worksheet(1).Range("A1:" + sMergeRange + "1");
                    rangehead.Merge().Style.Font.SetBold().Font.FontSize = 16;
                    rangehead.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    rangehead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangehead.SetValue("Hubli Electricity Supply Company Limited, (HESCOM)");

                    var rangeReporthead = wb.Worksheet(1).Range("A2:" + sMergeRange + "2");
                    rangeReporthead.Merge().Style.Font.SetBold().Font.FontSize = 12;
                    rangeReporthead.Merge().Style.Fill.BackgroundColor = XLColor.AirForceBlue;
                    rangeReporthead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    if (txtFromDate.Text != "" && txtToDate.Text != "")
                    {
                        rangeReporthead.SetValue("DTC Added Report  From " + objReport.sFromDate + "  To " + objReport.sTodate);
                    }
                    if (txtFromDate.Text != "" && txtToDate.Text == "")
                    {
                        rangeReporthead.SetValue("DTC Added Report  From " + objReport.sFromDate + "  To " + DateTime.Now);
                    }
                    if (txtFromDate.Text == "" && txtToDate.Text != "")
                    {
                        rangeReporthead.SetValue("DTC Added Report  as on " + objReport.sTodate);
                    }
                    if (txtFromDate.Text == "" && txtToDate.Text == "")
                    {
                        rangeReporthead.SetValue("DTC Added Report as on " + DateTime.Now);
                    }

                    //if (objReport.sFromDate != null)
                    //{
                    //    rangeReporthead.SetValue("DTC Added Report As On " +DateTime.Now);
                    //}
                    //else
                    //{
                    //    rangeReporthead.SetValue("DTC Added Report ");

                    //}

                    wb.Worksheet(1).Cell(3, 8).Value = DateTime.Now;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    string FileName = "DTCAddDetails " + DateTime.Now + ".xls";
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
                ShowMsgBox("No Records Found");
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
    }
}