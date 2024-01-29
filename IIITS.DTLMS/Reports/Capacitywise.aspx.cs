using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.IO;
using ClosedXML.Excel;
using System.Reflection;
using System.Data;
using System.Drawing;
using System.Configuration;

namespace IIITS.DTLMS.Reports
{
    public partial class Capacitywise : System.Web.UI.Page
    {
        string strFormCode = "Capacitywise";
        clsSession objSession;
        int Zone_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Zone_code"]);
        int Circle_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Circle_code"]);
        int Division_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Division_code"]);
        int SubDiv_code = Convert.ToInt32(ConfigurationSettings.AppSettings["SubDiv_code"]);
        int Section_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Section_code"]);
        int Feeder_code = Convert.ToInt32(ConfigurationSettings.AppSettings["feeder_code"]);


        private double Ten = 0;
        private double Fifteen = 0;
        private double Twentyfive = 0;
        private double Fifty = 0;
        private double SixThree = 0;
        private double Hund = 0;
        private double OnetwentyFive = 0;
        private double OneFifty = 0;
        private double OneSixty = 0;
        private double TwoHun = 0;
        private double TwoFifty = 0;
        private double ThreeHun = 0;
        private double ThreeFifteen = 0;
        private double FourHun = 0;
        private double FiveHun = 0;
        private double SixThirty = 0;
        private double SevenFifty = 0;
        private double Thousand = 0;
        private double ThousandTwoFifty = 0;
        private double GrandTotal = 0;
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

                txtFromDate1.Attributes.Add("readonly", "readonly");
                txtToDate1.Attributes.Add("readonly", "readonly");

                //txtFromDate2.Attributes.Add("readonly", "readonly");
                //txtToDate2.Attributes.Add("readonly", "readonly");

                CalendarExtender3.EndDate = System.DateTime.Now;
                CalendarExtender4.EndDate = System.DateTime.Now;

                CalendarExtender1.EndDate = System.DateTime.Now;
                CalendarExtender2.EndDate = System.DateTime.Now;

                //CalendarExtender5.EndDate = System.DateTime.Now;
                //CalendarExtender6.EndDate = System.DateTime.Now;
                if (!IsPostBack)
                {

                    string strQry = string.Empty;

                    objSession = (clsSession)Session["clsSession"];
                    string stroffCode = string.Empty;

                    stroffCode = objSession.OfficeCode;
                    string stroffCode1 = stroffCode;


                    string stroffCode2 = string.Empty;

                    stroffCode2 = objSession.OfficeCode;
                    string stroffCode3 = stroffCode2;

                    // Genaral.Load_Combo("SELECT \"SM_ID\",\"SM_NAME\" FROM \"TBLSTOREMAST\" ORDER BY \"SM_ID\"", "--Select--", cmbstore);


                    if (stroffCode == null || stroffCode == "")
                    {
                        Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone);
                    }
                    if (stroffCode2 == null || stroffCode2 == "")
                    {
                        Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone1);
                    }

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


                        Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_ZO_ID\"='" + cmbZone.SelectedValue + "'", "--Select--", cmbCircle);
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
                        Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CICLE_CODE\" AS TEXT)='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDiv);
                        if (stroffCode.Length >= 3)
                        {
                            stroffCode = stroffCode.Substring(0, Division_code);
                            cmbDiv.Items.FindByValue(stroffCode).Selected = true;
                            cmbDiv.Enabled = false;



                            stroffCode = stroffCode1;
                        }
                    }

                    if (stroffCode2.Length >= 1)
                    {
                        Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone1);
                        stroffCode2 = stroffCode2.Substring(0, Zone_code);

                        cmbZone.Items.FindByValue(stroffCode2).Selected = true;
                        cmbZone.Enabled = false;



                        stroffCode2 = stroffCode3;
                    }
                    if (stroffCode2.Length >= 1)
                    {


                        Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_ZO_ID\"='" + cmbZone1.SelectedValue + "'", "--Select--", cmbCircle1);
                        if (stroffCode2.Length >= 2)
                        {
                            stroffCode2 = stroffCode2.Substring(0, Circle_code);
                            cmbCircle1.Items.FindByValue(stroffCode2).Selected = true;
                            cmbCircle1.Enabled = false;



                            stroffCode2 = stroffCode3;
                        }
                    }

                    if (stroffCode2.Length >= 2)
                    {
                        Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CICLE_CODE\" AS TEXT)='" + cmbCircle1.SelectedValue + "'", "--Select--", cmbDiv1);
                        if (stroffCode2.Length >= 3)
                        {
                            stroffCode2 = stroffCode2.Substring(0, Division_code);
                            cmbDiv1.Items.FindByValue(stroffCode2).Selected = true;
                            cmbDiv1.Enabled = false;



                            stroffCode2 = stroffCode3;
                        }
                    }

                }

            }
            catch (Exception ex)
            {

                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }

        }



        protected void Export_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                clsReports objrep = new clsReports();
                string strOfficeCode = string.Empty;
                string strParam = string.Empty;
                string strValue = string.Empty;
                string stroffcode = string.Empty;


                //if (cmbreporttype.SelectedItem.Text == "--Select--")
                //{
                //    cmbreporttype.Focus();
                //    ShowMsgBox("Select The Report Type");
                //    return;
                //}




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
                objrep.sFromDate = txtFromDate.Text;
                objrep.sTodate = txtToDate.Text;

                if (objrep.sFromDate.ToString() != null && objrep.sFromDate.ToString() != "")
                {
                    // objrep.sFromDate = Request.QueryString["FromDate"].ToString();
                    DateTime DToDate = DateTime.ParseExact(objrep.sFromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    objrep.sFromDate = DToDate.ToString("yyyy/MM/dd");
                }
                if (objrep.sTodate.ToString() != null && objrep.sTodate.ToString() != "")
                {
                    // objrep.sTodate = Request.QueryString["ToDate"].ToString();
                    DateTime DToDate = DateTime.ParseExact(objrep.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    objrep.sTodate = DToDate.ToString("yyyy/MM/dd");
                }

                if (cmbDiv.SelectedIndex > 0)
                {
                    objrep.sOfficeCode = cmbDiv.SelectedValue;
                }
                else if (cmbCircle.SelectedIndex != 0)
                {
                    objrep.sOfficeCode = cmbCircle.SelectedValue;
                }
                else if (cmbZone.SelectedIndex > -1)
                {
                    objrep.sOfficeCode = cmbZone.SelectedValue;
                }

                else
                {
                    stroffcode = GetOfficeID();
                    objrep.sOfficeCode = stroffcode;
                }
                dt = objrep.Getcapacitywiseinstalled(objrep);

                using (XLWorkbook wb = new XLWorkbook())
                {
                    List<string> listtoRemove = new List<string> { "ZONE", "CIRCLE", "division", "no of tc" };


                    if (listtoRemove[0] != "")
                    {
                        foreach (var index in listtoRemove)
                        {
                            dt.Columns.Remove(index);
                        }
                    }
                    dt.Columns["value_hescom"].ColumnName = "Division Name";

                    dt = dt.DefaultView.ToTable();

                    wb.Worksheets.Add(dt, "sheet1");

                    wb.Worksheet(1).Row(1).InsertRowsAbove(6);

                    int count = dt.Columns.Count;

                    var rngstart = wb.Worksheet(1).Cell(1, 1).Address.ColumnLetter;
                    var rngend = wb.Worksheet(1).Cell(1, count).Address.ColumnLetter;

                    var header = wb.Worksheet(1).Range("A3:A5");
                    header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    header.Merge().Value = "Sl.no";

                    var header1 = wb.Worksheet(1).Range("B3:B5");
                    header1.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    header1.Merge().Value = "Division Name";

                    var header2 = wb.Worksheet(1).Range("C3:C5");
                    header2.Merge().Value = "10 KVA";
                    header2.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header3 = wb.Worksheet(1).Range("C4:C5");
                    header3.Merge().Value = "U";
                    var header4 = wb.Worksheet(1).Range("D4:D5");
                    header4.Merge().Value = "R";

                    var header5 = wb.Worksheet(1).Range("E3:E5");
                    header5.Merge().Value = "15 KVA";
                    header5.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header6 = wb.Worksheet(1).Range("E4:E5");
                    header6.Merge().Value = "U";
                    var header7 = wb.Worksheet(1).Range("F4:F5");
                    header7.Merge().Value = "R";

                    var header8 = wb.Worksheet(1).Range("G3:G5");
                    header8.Merge().Value = "25 KVA";
                    header8.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header9 = wb.Worksheet(1).Range("G4:G5");
                    header9.Merge().Value = "U";
                    var header10 = wb.Worksheet(1).Range("H4:H5");
                    header10.Merge().Value = "R";

                    var header11 = wb.Worksheet(1).Range("I3:I5");
                    header11.Merge().Value = "50 KVA";
                    header11.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header12 = wb.Worksheet(1).Range("I4:I5");
                    header12.Merge().Value = "U";
                    var header13 = wb.Worksheet(1).Range("J4:J5");
                    header13.Merge().Value = "R";

                    var header14 = wb.Worksheet(1).Range("K3:K5");
                    header14.Merge().Value = "63 KVA";
                    header14.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header15 = wb.Worksheet(1).Range("K4:K5");
                    header15.Merge().Value = "U";
                    var header16 = wb.Worksheet(1).Range("L4:L5");
                    header16.Merge().Value = "R";

                    var header17 = wb.Worksheet(1).Range("M3:M5");
                    header17.Merge().Value = "100 KVA";
                    header17.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header18 = wb.Worksheet(1).Range("M4:M5");
                    header18.Merge().Value = "U";
                    var header19 = wb.Worksheet(1).Range("N4:N5");
                    header19.Merge().Value = "R";

                    var header20 = wb.Worksheet(1).Range("O3:O5");
                    header20.Merge().Value = "125 KVA";
                    header20.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header21 = wb.Worksheet(1).Range("O4:O5");
                    header21.Merge().Value = "U";
                    var header22 = wb.Worksheet(1).Range("P4:P5");
                    header22.Merge().Value = "R";

                    var header23 = wb.Worksheet(1).Range("Q3:Q5");
                    header23.Merge().Value = "150 KVA";
                    header23.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header24 = wb.Worksheet(1).Range("Q4:Q5");
                    header24.Merge().Value = "U";
                    var header25 = wb.Worksheet(1).Range("R4:R5");
                    header25.Merge().Value = "R";

                    var header26 = wb.Worksheet(1).Range("S3:S5");
                    header26.Merge().Value = "160 KVA";
                    header26.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header27 = wb.Worksheet(1).Range("S4:S5");
                    header27.Merge().Value = "U";
                    var header28 = wb.Worksheet(1).Range("T4:T5");
                    header28.Merge().Value = "R";

                    var header29 = wb.Worksheet(1).Range("U3:U5");
                    header29.Merge().Value = "200 KVA";
                    header29.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header30 = wb.Worksheet(1).Range("U4:U5");
                    header30.Merge().Value = "U";
                    var header31 = wb.Worksheet(1).Range("V4:V5");
                    header31.Merge().Value = "R";

                    var header32 = wb.Worksheet(1).Range("W3:W5");
                    header32.Merge().Value = "250 KVA";
                    header32.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header33 = wb.Worksheet(1).Range("W4:W5");
                    header33.Merge().Value = "U";
                    var header34 = wb.Worksheet(1).Range("X4:X5");
                    header34.Merge().Value = "R";

                    var header35 = wb.Worksheet(1).Range("Y3:Y5");
                    header35.Merge().Value = "300 KVA";
                    header35.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header36 = wb.Worksheet(1).Range("Y4:Y5");
                    header36.Merge().Value = "U";
                    var header37 = wb.Worksheet(1).Range("Z4:Z5");
                    header37.Merge().Value = "R";

                    var header38 = wb.Worksheet(1).Range("AA3:AA5");
                    header38.Merge().Value = "315 KVA";
                    header38.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header39 = wb.Worksheet(1).Range("AA4:AA5");
                    header39.Merge().Value = "U";
                    var header40 = wb.Worksheet(1).Range("AB4:AB5");
                    header40.Merge().Value = "R";

                    var header41 = wb.Worksheet(1).Range("AC3:AC5");
                    header41.Merge().Value = "400 KVA";
                    header41.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header42 = wb.Worksheet(1).Range("AC4:AC5");
                    header42.Merge().Value = "U";
                    var header43 = wb.Worksheet(1).Range("AD4:AD5");
                    header43.Merge().Value = "R";

                    var header44 = wb.Worksheet(1).Range("AE3:AE5");
                    header44.Merge().Value = "500 KVA";
                    header44.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header45 = wb.Worksheet(1).Range("AE4:AE5");
                    header45.Merge().Value = "U";
                    var header46 = wb.Worksheet(1).Range("AF4:AF5");
                    header46.Merge().Value = "R";

                    var header47 = wb.Worksheet(1).Range("AG3:AG5");
                    header47.Merge().Value = "630 KVA";
                    header47.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header48 = wb.Worksheet(1).Range("AG4:AG5");
                    header48.Merge().Value = "U";
                    var header49 = wb.Worksheet(1).Range("AH4:AH5");
                    header49.Merge().Value = "R";

                    var header50 = wb.Worksheet(1).Range("AI3:AI5");
                    header50.Merge().Value = "750 KVA";
                    header50.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header51 = wb.Worksheet(1).Range("AI4:AI5");
                    header51.Merge().Value = "U";
                    var header52 = wb.Worksheet(1).Range("AJ4:AJ5");
                    header52.Merge().Value = "R";

                    var header53 = wb.Worksheet(1).Range("AK3:AK5");
                    header53.Merge().Value = "1000 KVA";
                    header53.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header54 = wb.Worksheet(1).Range("AK4:AK5");
                    header54.Merge().Value = "U";
                    var header55 = wb.Worksheet(1).Range("AL4:AL5");
                    header55.Merge().Value = "R";

                    var header56 = wb.Worksheet(1).Range("AM3:AM5");
                    header56.Merge().Value = "1250 KVA";
                    header56.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header57 = wb.Worksheet(1).Range("AM4:AM5");
                    header57.Merge().Value = "U";
                    var header58 = wb.Worksheet(1).Range("AN4:AN5");
                    header58.Merge().Value = "R";

                    var headrrrr = wb.Worksheet(1).Range("A3:AN5");
                    headrrrr.Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    headrrrr.Style.Font.SetBold().Font.FontSize = 10;
                    headrrrr.Style.Border.InsideBorder = XLBorderStyleValues.Medium;
                    headrrrr.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                    headrrrr.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                    headrrrr.Style.Border.LeftBorder = XLBorderStyleValues.Medium;

                    var aaa = wb.Worksheet(1).Range("A1:" + rngend + "1");
                    wb.Worksheet(1).Row(7).Hide();
                    wb.Worksheet(1).Row(3).Height = 18.0;
                    //var rngstart = wb.Worksheet(1).Cell(1, 1).Address.ColumnLetter;
                    //var rngend = wb.Worksheet(1).Cell(1, count).Address.ColumnLetter;
                    wb.Worksheet(1).Cell(1, 1).RichText.AddText("TOTAL").SetFontColor(XLColor.Black).SetBold();
                    // wb.Worksheet(1).RichText.AddText(" BIG ").SetFontColor(XLColor.Blue).SetBold();
                    var rangehead = wb.Worksheet(1).Range("A1:" + rngend + "1");
                    rangehead.Merge().Style.Font.SetBold().Font.FontSize = 13;
                    rangehead.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    rangehead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangehead.SetValue("Hubli Electricity Supply Company Limited,(HESCOM)");
                    //page title
                    // string PageTitle;
                    // FROM '"+ objrep.sFromDate + "' TO  '"+ objrep.sTodate + "'
                    //if (cmbreporttype.SelectedItem.Value == "1")
                    //{
                    //     PageTitle = "DIVISION WISE AND CAPACITY WISE DISTRIBUTION TRANSFORMERS EXISTING ";
                    //}
                    //else
                    //{
                    //     PageTitle = "DIVISION WISE AND CAPACITY WISE DISTRIBUTION TRANSFORMERS INSTALLED ";

                    //}
                    var rangeReporthead = wb.Worksheet(1).Range("A2:" + rngend + "2");
                    rangeReporthead.Merge().Style.Font.SetBold().Font.FontSize = 12;
                    rangeReporthead.Merge().Style.Fill.BackgroundColor = XLColor.AirForceBlue;
                    rangeReporthead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    //rangeReporthead.SetValue("" + PageTitle);
                    string filename = "Capacity wise" + ".xls";
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    // string FileName = "CregAbstract " + DateTime.Now + ".xls";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + filename);

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
                        HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
                        HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void Export_Click1(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                clsReports objrep = new clsReports();
                string strOfficeCode = string.Empty;
                string strParam = string.Empty;
                string strValue = string.Empty;
                string stroffcode = string.Empty;


                string sResult = string.Empty;
                if (txtFromDate1.Text != "")
                {
                    sResult = Genaral.DateValidation(txtFromDate1.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtFromDate1.Focus();
                        return;
                    }
                }

                if (txtToDate1.Text != "")
                {
                    sResult = Genaral.DateValidation(txtToDate1.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtToDate1.Focus();
                        return;
                    }
                }

                if (txtFromDate1.Text != "" && txtToDate1.Text != "")
                {
                    sResult = Genaral.DateComparision(txtToDate1.Text, txtFromDate1.Text, false, false);
                    if (sResult == "2")
                    {
                        ShowMsgBox("To Date should be Greater than From Date");
                        txtToDate1.Focus();
                        return;

                    }
                }
                objrep.sFromDate = txtFromDate1.Text;
                objrep.sTodate = txtToDate1.Text;

                if (objrep.sFromDate.ToString() != null && objrep.sFromDate.ToString() != "")
                {
                    // objrep.sFromDate = Request.QueryString["FromDate"].ToString();
                    DateTime DToDate = DateTime.ParseExact(objrep.sFromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    objrep.sFromDate = DToDate.ToString("yyyy/MM/dd");
                }
                if (objrep.sTodate.ToString() != null && objrep.sTodate.ToString() != "")
                {
                    // objrep.sTodate = Request.QueryString["ToDate"].ToString();
                    DateTime DToDate = DateTime.ParseExact(objrep.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    objrep.sTodate = DToDate.ToString("yyyy/MM/dd");
                }

                if (cmbDiv1.SelectedIndex > 0)
                {
                    objrep.sOfficeCode = cmbDiv1.SelectedValue;
                }
                else if (cmbCircle1.SelectedIndex != 0)
                {
                    objrep.sOfficeCode = cmbCircle1.SelectedValue;
                }
                else if (cmbZone1.SelectedIndex > -1)
                {
                    objrep.sOfficeCode = cmbZone1.SelectedValue;
                }
                else
                {
                    stroffcode = GetOfficeID1();
                    objrep.sOfficeCode = stroffcode;
                }
                dt = objrep.Getcapacitywisefailure(objrep);

                using (XLWorkbook wb = new XLWorkbook())
                {
                    List<string> listtoRemove = new List<string> { "ZONE", "CIRCLE", "division" };

                    if (listtoRemove[0] != "")
                    {
                        foreach (var index in listtoRemove)
                        {
                            dt.Columns.Remove(index);
                        }
                    }
                    dt.Columns["value_bescom"].ColumnName = "Division Name";

                    dt = dt.DefaultView.ToTable();


                    wb.Worksheets.Add(dt, "sheet1");

                    wb.Worksheet(1).Row(1).InsertRowsAbove(6);


                    int count = dt.Columns.Count;


                    var rngstart = wb.Worksheet(1).Cell(1, 1).Address.ColumnLetter;
                    var rngend = wb.Worksheet(1).Cell(1, count).Address.ColumnLetter;

                    var header = wb.Worksheet(1).Range("A3:A5");
                    header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    header.Merge().Value = "Sl.no";

                    var header1 = wb.Worksheet(1).Range("B3:B5");
                    header1.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    header1.Merge().Value = "Division Name";

                    var header2 = wb.Worksheet(1).Range("C3:C5");
                    header2.Merge().Value = "10/15/25 kVA";
                    header2.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header3 = wb.Worksheet(1).Range("C4:C5");
                    header3.Merge().Value = "U";
                    var header4 = wb.Worksheet(1).Range("D4:D5");
                    header4.Merge().Value = "R";

                    var header5 = wb.Worksheet(1).Range("E3:E5");
                    header5.Merge().Value = "50/63/75 kVA";
                    header5.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header6 = wb.Worksheet(1).Range("E4:E5");
                    header6.Merge().Value = "U";
                    var header7 = wb.Worksheet(1).Range("F4:F5");
                    header7.Merge().Value = "R";

                    var header8 = wb.Worksheet(1).Range("G3:G5");
                    header8.Merge().Value = "100/160 kVA";
                    header8.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header9 = wb.Worksheet(1).Range("G4:G5");
                    header9.Merge().Value = "U";
                    var header10 = wb.Worksheet(1).Range("H4:H5");
                    header10.Merge().Value = "R";

                    var header11 = wb.Worksheet(1).Range("I3:I5");
                    header11.Merge().Value = "200/250 kVA";
                    header11.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header12 = wb.Worksheet(1).Range("I4:I5");
                    header12.Merge().Value = "U";
                    var header13 = wb.Worksheet(1).Range("J4:J5");
                    header13.Merge().Value = "R";


                    var header14 = wb.Worksheet(1).Range("K3:K5");
                    header14.Merge().Value = "300/400/500 kVA";
                    header14.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header15 = wb.Worksheet(1).Range("K4:K5");
                    header15.Merge().Value = "U";
                    var header16 = wb.Worksheet(1).Range("L4:L5");
                    header16.Merge().Value = "R";


                    var header17 = wb.Worksheet(1).Range("M3:M5");
                    header17.Merge().Value = "Above 500 kVA";
                    header17.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header18 = wb.Worksheet(1).Range("M4:M5");
                    header18.Merge().Value = "U";
                    var header19 = wb.Worksheet(1).Range("N4:N5");
                    header19.Merge().Value = "R";

                    var headrrrr = wb.Worksheet(1).Range("A3:N5");
                    headrrrr.Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    headrrrr.Style.Font.SetBold().Font.FontSize = 10;
                    headrrrr.Style.Border.InsideBorder = XLBorderStyleValues.Medium;
                    headrrrr.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                    headrrrr.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                    headrrrr.Style.Border.LeftBorder = XLBorderStyleValues.Medium;

                    var aaa = wb.Worksheet(1).Range("A1:" + rngend + "1");
                    wb.Worksheet(1).Row(7).Hide();
                    wb.Worksheet(1).Row(3).Height = 18.0;
                    //var rngstart = wb.Worksheet(1).Cell(1, 1).Address.ColumnLetter;
                    //var rngend = wb.Worksheet(1).Cell(1, count).Address.ColumnLetter;
                    wb.Worksheet(1).Cell(1, 1).RichText.AddText("TOTAL").SetFontColor(XLColor.Black).SetBold();
                    // wb.Worksheet(1).RichText.AddText(" BIG ").SetFontColor(XLColor.Blue).SetBold();
                    var rangehead = wb.Worksheet(1).Range("A1:" + rngend + "1");
                    rangehead.Merge().Style.Font.SetBold().Font.FontSize = 13;
                    rangehead.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    rangehead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangehead.SetValue("Hubli Electricity Supply Company Limited,(HESCOM)");
                    //page title
                    string PageTitle;
                    //from   '" + objrep.sFromDate + "' to  '" + objrep.sTodate + "'
                    PageTitle = "DIVISION WISE AND CAPACITY WISE DISTRIBUTION TRANSFORMERS FAILED  ";

                    var rangeReporthead = wb.Worksheet(1).Range("A2:" + rngend + "2");
                    rangeReporthead.Merge().Style.Font.SetBold().Font.FontSize = 12;
                    rangeReporthead.Merge().Style.Fill.BackgroundColor = XLColor.AirForceBlue;
                    rangeReporthead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangeReporthead.SetValue("" + PageTitle);
                    string filename = "Capacity wise" + ".xls";
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    // string FileName = "CregAbstract " + DateTime.Now + ".xls";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + filename);

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
                        HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
                        HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        #region 
        //protected void Export_Click2(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        clsReports objrep = new clsReports();
        //        string strOfficeCode = string.Empty;
        //        string strParam = string.Empty;
        //        string strValue = string.Empty;
        //        string stroffcode = string.Empty;


        //        string sResult = string.Empty;
        //        if (txtFromDate2.Text != "")
        //        {
        //            sResult = Genaral.DateValidation(txtFromDate2.Text);
        //            if (sResult != "")
        //            {
        //                ShowMsgBox(sResult);
        //                txtFromDate2.Focus();
        //                return;
        //            }
        //        }

        //        if (txtToDate2.Text != "")
        //        {
        //            sResult = Genaral.DateValidation(txtToDate2.Text);
        //            if (sResult != "")
        //            {
        //                ShowMsgBox(sResult);
        //                txtToDate2.Focus();
        //                return;
        //            }
        //        }

        //        if (txtFromDate2.Text != "" && txtToDate2.Text != "")
        //        {
        //            sResult = Genaral.DateComparision(txtToDate2.Text, txtFromDate2.Text, false, false);
        //            if (sResult == "2")
        //            {
        //                ShowMsgBox("To Date should be Greater than From Date");
        //                txtToDate2.Focus();
        //                return;

        //            }
        //        }
        //        objrep.sFromDate = txtFromDate2.Text;
        //        objrep.sTodate = txtToDate2.Text;

        //        if (objrep.sFromDate.ToString() != null && objrep.sFromDate.ToString() != "")
        //        {
        //            // objrep.sFromDate = Request.QueryString["FromDate"].ToString();
        //            DateTime DFromDate = DateTime.ParseExact(objrep.sFromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            objrep.sFromDate = DFromDate.ToString("yyyy/MM/dd");
        //        }
        //        if (objrep.sTodate.ToString() != null && objrep.sTodate.ToString() != "")
        //        {
        //            // objrep.sTodate = Request.QueryString["ToDate"].ToString();
        //            DateTime DToDate = DateTime.ParseExact(objrep.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            objrep.sTodate = DToDate.ToString("yyyy/MM/dd");
        //        }



        //            objrep.sOfficeCode = cmbstore.SelectedValue;



        //        dt = objrep.Getcapacitywiseadded(objrep);


        //        Ten = Convert.ToDouble(dt.Compute("SUM("+Convert.ToString(dt.Columns["a"])+")",""));
        //        Fifteen = Convert.ToDouble(dt.Compute("SUM(" + Convert.ToString(dt.Columns["b"]) + ")", ""));
        //        Twentyfive = Convert.ToDouble(dt.Compute("SUM(" + Convert.ToString(dt.Columns["c"]) + ")", ""));
        //        Fifty = Convert.ToDouble(dt.Compute("SUM(" + Convert.ToString(dt.Columns["d"]) + ")", ""));
        //        SixThree = Convert.ToDouble(dt.Compute("SUM(" + Convert.ToString(dt.Columns["e"]) + ")", ""));
        //        Hund = Convert.ToDouble(dt.Compute("SUM(" + Convert.ToString(dt.Columns["f"]) + ")", ""));
        //        OnetwentyFive = Convert.ToDouble(dt.Compute("SUM(" + Convert.ToString(dt.Columns["g"]) + ")", ""));
        //        OneFifty = Convert.ToDouble(dt.Compute("SUM(" + Convert.ToString(dt.Columns["h"]) + ")", ""));
        //        OneSixty = Convert.ToDouble(dt.Compute("SUM(" + Convert.ToString(dt.Columns["i"]) + ")", ""));
        //        TwoHun = Convert.ToDouble(dt.Compute("SUM(" + Convert.ToString(dt.Columns["j"]) + ")", ""));
        //        TwoFifty = Convert.ToDouble(dt.Compute("SUM(" + Convert.ToString(dt.Columns["k"]) + ")", ""));
        //        ThreeHun = Convert.ToDouble(dt.Compute("SUM(" + Convert.ToString(dt.Columns["l"]) + ")", ""));
        //        ThreeFifteen = Convert.ToDouble(dt.Compute("SUM(" + Convert.ToString(dt.Columns["m"]) + ")", ""));
        //        FourHun = Convert.ToDouble(dt.Compute("SUM(" + Convert.ToString(dt.Columns["n"]) + ")", ""));
        //        FiveHun = Convert.ToDouble(dt.Compute("SUM(" + Convert.ToString(dt.Columns["o"]) + ")", ""));
        //        SixThirty = Convert.ToDouble(dt.Compute("SUM(" + Convert.ToString(dt.Columns["p"]) + ")", ""));
        //        SevenFifty = Convert.ToDouble(dt.Compute("SUM(" + Convert.ToString(dt.Columns["q"]) + ")", ""));
        //        Thousand = Convert.ToDouble(dt.Compute("SUM(" + Convert.ToString(dt.Columns["r"]) + ")", ""));
        //        ThousandTwoFifty = Convert.ToDouble(dt.Compute("SUM(" + Convert.ToString(dt.Columns["s"]) + ")", ""));
        //        GrandTotal = Convert.ToDouble(dt.Compute("SUM(" + Convert.ToString(dt.Columns["TOTAL"]) + ")", ""));

        //        DataRow row = dt.NewRow();
        //        row["SM_NAME"] = "Total";
        //        row["a"] = Ten;
        //        row["b"] = Fifteen;
        //        row["c"] = Twentyfive;
        //        row["d"] = Fifty;
        //        row["e"] = SixThree;
        //        row["f"] = Hund;
        //        row["g"] = OnetwentyFive;
        //        row["h"] = OneFifty;
        //        row["i"] = OneSixty;
        //        row["j"] = TwoHun;
        //        row["k"] = TwoFifty;
        //        row["l"] = ThreeHun;
        //        row["m"] = ThreeFifteen;
        //        row["n"] = FourHun;
        //        row["o"] = FiveHun;

        //        row["p"] = SixThirty;
        //        row["q"] = SevenFifty;
        //        row["r"] = Thousand;
        //        row["s"] = ThousandTwoFifty;
        //        row["TOTAL"] = GrandTotal;


        //        dt.Rows.Add(row);




        //        dt.Columns["SM_NAME"].ColumnName = "Store Name";
        //        dt.Columns["a"].ColumnName = "10 kva";
        //        dt.Columns["b"].ColumnName = "15 kva";

        //        dt.Columns["c"].ColumnName = "25 kva";
        //        dt.Columns["d"].ColumnName = "50 kva";
        //        dt.Columns["e"].ColumnName = "63 kva";

        //        dt.Columns["f"].ColumnName = "100 kva";
        //        dt.Columns["g"].ColumnName = "125 kva";
        //        dt.Columns["h"].ColumnName = "150 kva";

        //        dt.Columns["i"].ColumnName = "160 kva";
        //        dt.Columns["j"].ColumnName = "200 kva";
        //        dt.Columns["k"].ColumnName = "250 kva";

        //        dt.Columns["l"].ColumnName = "300 kva";
        //        dt.Columns["m"].ColumnName = "315 kva";
        //        dt.Columns["n"].ColumnName = "400 kva";

        //        dt.Columns["o"].ColumnName = "500 kva";
        //        dt.Columns["p"].ColumnName = "630 kva";
        //        dt.Columns["q"].ColumnName = "750 kva";

        //        dt.Columns["r"].ColumnName = "1000 kva";
        //        dt.Columns["s"].ColumnName = "1250 kva";

        //        List<string> listtoRemove = new List<string> { "" };
        //        string filename = "Capacity" + DateTime.Now + ".xls";
        //        string pagetitle = "CAPACITY WISE DISTRIBUTION TRANSFORMERS ADDED";

        //        Genaral.getexcel(dt, listtoRemove, filename, pagetitle);


        //        //using (XLWorkbook wb = new XLWorkbook())
        //        //{

        //        //    //dt.Columns["SM_NAME"].ColumnName = "Division Name";

        //        //    dt = dt.DefaultView.ToTable();


        //        //    wb.Worksheets.Add(dt, "sheet1");

        //        //    wb.Worksheet(1).Row(1).InsertRowsAbove(6);


        //        //    int count = dt.Columns.Count;


        //        //    var rngstart = wb.Worksheet(1).Cell(1, 1).Address.ColumnLetter;
        //        //    var rngend = wb.Worksheet(1).Cell(1, count).Address.ColumnLetter;



        //        //    var aaa = wb.Worksheet(1).Range("A1:" + rngend + "1");



        //        //    wb.Worksheet(1).Row(7).Hide();
        //        //    wb.Worksheet(1).Row(3).Height = 18.0;

        //        //    //var rngstart = wb.Worksheet(1).Cell(1, 1).Address.ColumnLetter;
        //        //    //var rngend = wb.Worksheet(1).Cell(1, count).Address.ColumnLetter;

        //        //    wb.Worksheet(1).Cell(1, 1).RichText.AddText("TOTAL").SetFontColor(XLColor.Black).SetBold();

        //        //    // wb.Worksheet(1).RichText.AddText(" BIG ").SetFontColor(XLColor.Blue).SetBold();



        //        //    var rangehead = wb.Worksheet(1).Range("A1:" + rngend + "1");
        //        //    rangehead.Merge().Style.Font.SetBold().Font.FontSize = 13;
        //        //    rangehead.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
        //        //    rangehead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //        //    rangehead.SetValue("Bangalore Electricity Supply Company Ltd,(BESCOM)");
        //        //    //page title
        //        //    string PageTitle;
        //        //    //from   '" + objrep.sFromDate + "' to  '" + objrep.sTodate + "'
        //        //    PageTitle = "DIVISION WISE AND CAPACITY WISE DISTRIBUTION TRANSFORMERS ADDED  ";


        //        //    var rangeReporthead = wb.Worksheet(1).Range("A2:" + rngend + "2");
        //        //    rangeReporthead.Merge().Style.Font.SetBold().Font.FontSize = 12;
        //        //    rangeReporthead.Merge().Style.Fill.BackgroundColor = XLColor.AirForceBlue;
        //        //    rangeReporthead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //        //    rangeReporthead.SetValue("" + PageTitle);

        //        //    string filename = "Capacity wise" + ".xls";


        //        //    HttpContext.Current.Response.Clear();
        //        //    HttpContext.Current.Response.Buffer = true;
        //        //    HttpContext.Current.Response.Charset = "";
        //        //    // string FileName = "CregAbstract " + DateTime.Now + ".xls";
        //        //    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        //    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + filename);

        //        //    using (MemoryStream MyMemoryStream = new MemoryStream())
        //        //    {
        //        //        wb.SaveAs(MyMemoryStream);
        //        //        MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
        //        //        HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
        //        //        HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
        //        //        HttpContext.Current.ApplicationInstance.CompleteRequest();
        //        //    }
        //        //}






        //    }
        //    catch (Exception ex)
        //    {

        //        //clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

        //    }

        //}
        #endregion
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
        public string GetOfficeID()
        {
            string strOfficeId = string.Empty;
            if (cmbZone.SelectedIndex > 0)
            {
                strOfficeId = cmbZone.SelectedValue.ToString();
            }
            if (cmbCircle.SelectedIndex > 0)
            {
                strOfficeId = cmbCircle.SelectedValue.ToString();
            }
            if (cmbDiv.SelectedIndex > 0)
            {
                strOfficeId = cmbDiv.SelectedValue.ToString();
            }
            return (strOfficeId);
        }

        public string GetOfficeID1()
        {
            string strOfficeId = string.Empty;
            if (cmbZone1.SelectedIndex > 0)
            {
                strOfficeId = cmbZone1.SelectedValue.ToString();
            }
            if (cmbCircle1.SelectedIndex > 0)
            {
                strOfficeId = cmbCircle1.SelectedValue.ToString();
            }
            if (cmbDiv1.SelectedIndex > 0)
            {
                strOfficeId = cmbDiv1.SelectedValue.ToString();
            }
            return (strOfficeId);
        }
        protected void cmbZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbZone.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_ZO_ID\"='" + cmbZone.SelectedValue + "'", "--Select--", cmbCircle);
                    cmbDiv.Items.Clear();
                }
                else
                {
                    cmbCircle.Items.Clear();
                    cmbDiv.Items.Clear();
                }
            }
            catch (Exception ex)
            {
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
                }
                else
                {
                    cmbDiv.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbZone1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbZone1.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_ZO_ID\"='" + cmbZone1.SelectedValue + "'", "--Select--", cmbCircle1);
                    cmbDiv1.Items.Clear();
                }
                else
                {
                    cmbCircle1.Items.Clear();
                    cmbDiv1.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbCircle1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle1.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CICLE_CODE\" AS TEXT)='" + cmbCircle1.SelectedValue + "'", "--Select--", cmbDiv1);
                }
                else
                {
                    cmbDiv1.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}