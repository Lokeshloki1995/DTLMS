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
    public partial class Repairerabstract : System.Web.UI.Page
    {
        string Officecode = string.Empty;
        string strFormCode = "Repairerabstract";
        clsSession objSession;
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

                    Genaral.Load_Combo("SELECT \"FY_YEARS\",\"FY_YEARS\" FROM \"TBLFINANCIALYEAR\"  WHERE \"FY_STATUS\"='1'", "--Select--", cmbFinYear);

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
                string fnclyear = string.Empty;

                string sResult = string.Empty;
                if (cmbFinYear.Text == "")
                {
                    ShowMsgBox("Please Select The Financial Year");
                    cmbFinYear.Focus();
                    return;

                }

                string previousyear = cmbFinYear.SelectedValue.Split('-').GetValue(0).ToString();
                string presentyear = cmbFinYear.SelectedValue.Split('-').GetValue(1).ToString();
                
                previousyear = previousyear + "-0" + 3 +'-'+ 31;

                presentyear = presentyear + "-0" + 4 + "-0" + 1;

                dt = objrep.Getrepairerabstract(previousyear, presentyear);




                using (XLWorkbook wb = new XLWorkbook())
                {
                    List<string> listtoRemove = new List<string> { "" };


                    if (listtoRemove[0] != "")
                    {
                        foreach (var index in listtoRemove)
                        {
                            dt.Columns.Remove(index);
                        }
                    }
                    dt.Columns["COMPANY"].ColumnName = "COMPANY";

                    dt = dt.DefaultView.ToTable();


                    wb.Worksheets.Add(dt, "sheet1");

                    wb.Worksheet(1).Row(1).InsertRowsAbove(6);


                    int count = dt.Columns.Count;


                    var rngstart = wb.Worksheet(1).Cell(1,1).Address.ColumnLetter;
                    var rngend = wb.Worksheet(1).Cell(1, count).Address.ColumnLetter;

                    //var header = wb.Worksheet(1).Range("A3:A5");
                    //header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    //header.Merge().Value = "Company";

                    var header1 = wb.Worksheet(1).Range("A3:A5");
                    header1.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    header1.Merge().Value = "Company";



                    var header2 = wb.Worksheet(1).Range("B3:B5");
                    header2.Merge().Value = "APR";
                    header2.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header3 = wb.Worksheet(1).Range("B4:B5");
                    header3.Merge().Value = "No. of tr repaired";
                    var header4 = wb.Worksheet(1).Range("C4:C5");
                    header4.Merge().Value = "Cost incurred";


                    var header5 = wb.Worksheet(1).Range("D3:D5");
                    header5.Merge().Value = "MAY";
                    header5.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header6 = wb.Worksheet(1).Range("D4:D5");
                    header6.Merge().Value = "No. of tr repaired";
                    var header7 = wb.Worksheet(1).Range("E4:E5");
                    header7.Merge().Value = "Cost incurred";

                    var header8 = wb.Worksheet(1).Range("F3:F5");
                    header8.Merge().Value = "JUN";
                    header8.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header9 = wb.Worksheet(1).Range("F4:F5");
                    header9.Merge().Value = "No. of tr repaired";
                    var header10 = wb.Worksheet(1).Range("G4:G5");
                    header10.Merge().Value = "Cost incurred";

                    var header11 = wb.Worksheet(1).Range("H3:H5");
                    header11.Merge().Value = "JUL";
                    header11.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header12 = wb.Worksheet(1).Range("H4:H5");
                    header12.Merge().Value = "No. of tr repaired";
                    var header13 = wb.Worksheet(1).Range("I4:I5");
                    header13.Merge().Value = "Cost incurred";


                    var header14 = wb.Worksheet(1).Range("J3:J5");
                    header14.Merge().Value = "AUG";
                    header14.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header15 = wb.Worksheet(1).Range("J4:J5");
                    header15.Merge().Value = "No. of tr repaired";
                    var header16 = wb.Worksheet(1).Range("K4:K5");
                    header16.Merge().Value = "Cost incurred";


                    var header17 = wb.Worksheet(1).Range("L3:L5");
                    header17.Merge().Value = "SEP";
                    header17.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header18 = wb.Worksheet(1).Range("L4:L5");
                    header18.Merge().Value = "No. of tr repaired";
                    var header19 = wb.Worksheet(1).Range("M4:M5");
                    header19.Merge().Value = "Cost incurred";


                    var header20 = wb.Worksheet(1).Range("N3:N5");
                    header20.Merge().Value = "OCT";
                    header20.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header21 = wb.Worksheet(1).Range("N4:N5");
                    header21.Merge().Value = "No. of tr repaired";
                    var header22 = wb.Worksheet(1).Range("O4:O5");
                    header22.Merge().Value = "Cost incurred";

                    var header23 = wb.Worksheet(1).Range("P3:P5");
                    header23.Merge().Value = "NOV";
                    header23.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header24 = wb.Worksheet(1).Range("P4:P5");
                    header24.Merge().Value = "No. of tr repaired";
                    var header25 = wb.Worksheet(1).Range("Q4:Q5");
                    header25.Merge().Value = "Cost incurred";

                    var header26 = wb.Worksheet(1).Range("R3:R5");
                    header26.Merge().Value = "DEC";
                    header26.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header27 = wb.Worksheet(1).Range("R4:R5");
                    header27.Merge().Value = "No. of tr repaired";
                    var header28 = wb.Worksheet(1).Range("S4:S5");
                    header28.Merge().Value = "Cost incurred ";


                    var header29 = wb.Worksheet(1).Range("T3:T5");
                    header29.Merge().Value = "JAN";
                    header29.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header30 = wb.Worksheet(1).Range("T4:T5");
                    header30.Merge().Value = "No. of tr repaired";
                    var header31 = wb.Worksheet(1).Range("U4:U5");
                    header31.Merge().Value = "Cost incurred ";

                    var header32 = wb.Worksheet(1).Range("V3:V5");
                    header32.Merge().Value = "FEB";
                    header32.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header33 = wb.Worksheet(1).Range("V4:V5");
                    header33.Merge().Value = "No. of tr repaired";
                    var header34 = wb.Worksheet(1).Range("W4:W5");
                    header34.Merge().Value = "Cost incurred ";

                    var header35 = wb.Worksheet(1).Range("X3:X5");
                    header35.Merge().Value = "MAR";
                    header35.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header36 = wb.Worksheet(1).Range("X4:X5");
                    header36.Merge().Value = "No. of tr repaired";
                    var header37 = wb.Worksheet(1).Range("Y4:Y5");
                    header37.Merge().Value = "Cost incurred ";

                    var header38 = wb.Worksheet(1).Range("Z3:Z5");
                    header38.Merge().Value = "GRAND TOTAL";
                    header38.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var header39 = wb.Worksheet(1).Range("Z4:Z5");
                    header39.Merge().Value = "Total Nos. repaired";
                    var header40 = wb.Worksheet(1).Range("AA4:AA5");
                    header40.Merge().Value = "Total cost incurred";

      
                    var headrrrr = wb.Worksheet(1).Range("A3:AA5");
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
                    // FROM '"+ objrep.sFromDate + "' TO  '"+ objrep.sTodate + "'
                   
                        PageTitle = "Transformer Repair Details from "+previousyear+ " to " + presentyear + " ";
                   
                    var rangeReporthead = wb.Worksheet(1).Range("A2:" + rngend + "2");
                    rangeReporthead.Merge().Style.Font.SetBold().Font.FontSize = 12;
                    rangeReporthead.Merge().Style.Fill.BackgroundColor = XLColor.AirForceBlue;
                    rangeReporthead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangeReporthead.SetValue("" + PageTitle);

                    string filename = "Repairer wise" + ".xls";


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
                string fnclyear = string.Empty;

                string sResult = string.Empty;


                string sParam = "id=repairerabstractyearwise";
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + sParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");














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





    }
}