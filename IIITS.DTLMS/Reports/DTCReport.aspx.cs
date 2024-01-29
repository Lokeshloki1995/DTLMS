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
using System.Configuration;


namespace IIITS.DTLMS.Reports
{
    public partial class DTCReport : System.Web.UI.Page
    {

        string strFormCode = "DTCReport";
        clsSession objSession;
        int Zone_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Zone_code"]);
        int Circle_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Circle_code"]);
        int Division_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Division_code"]);
        int SubDiv_code = Convert.ToInt32(ConfigurationSettings.AppSettings["SubDiv_code"]);
        int Section_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Section_code"]);
        int Feeder_code = Convert.ToInt32(ConfigurationSettings.AppSettings["feeder_code"]);
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

                    objSession = (clsSession)Session["clsSession"];
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
                    Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" from \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='PT'", "--Select--", cmbSchemeType);
                    Genaral.Load_Combo("SELECT \"MD_ORDER_BY\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='C' ORDER BY \"MD_ORDER_BY\"", "--Select--", cmbCapacity);
                    string stroffCode1 = stroffCode;
                    if (stroffCode == null || stroffCode == "")
                    {
                        Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone);
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
                    if (stroffCode.Length >= 3)
                    {
                        Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE CAST(\"SD_DIV_CODE\" AS TEXT)='" + cmbDiv.SelectedValue + "'", "--Select--", cmbSubDivision);

                        if (stroffCode.Length >= 4)
                        {
                            stroffCode = stroffCode.Substring(0, SubDiv_code);
                            cmbSubDivision.Items.FindByValue(stroffCode).Selected = true;
                            cmbSubDivision.Enabled = false;
                            stroffCode = stroffCode1;
                        }
                    }
                    if (stroffCode.Length >= 4)
                    {
                        Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_SUBDIV_CODE\"='" + cmbSubDivision.SelectedValue + "'", "--Select--", cmbOMSection);
                        //Genaral.Load_Combo("SELECT DISTINCT \"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" and CAST(\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbSubDivision.SelectedValue + "%'  ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName);

                        Genaral.Load_Combo(" SELECT  DISTINCT \"FD_FEEDER_ID\", \"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\" INNER JOIN \"TBLFEEDEROFFCODE\" ON \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" WHERE CAST (\"FDO_OFFICE_CODE\"AS TEXT) like '\" + cmbSubDivision.SelectedValue + \"%'  ORDER BY \"FD_FEEDER_ID\" ", "--Select--", cmbFeederName);
                        if (stroffCode.Length >= 5)
                        {


                            stroffCode = stroffCode.Substring(0, Section_code);
                            cmbOMSection.Items.FindByValue(stroffCode).Selected = true;
                            cmbOMSection.Enabled = false;
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                cmbCapacity.SelectedIndex = 0;
                cmbSchemeType.SelectedIndex = 0;

                cmbZone.SelectedIndex = 0;

                cmbCircle.Items.Clear();
                cmbDiv.Items.Clear();
                cmbSubDivision.Items.Clear();
                cmbOMSection.Items.Clear();
                cmbFeederName.Items.Clear();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
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
                    Genaral.Load_Combo("  SELECT  DISTINCT\"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" " +
                        " FROM \"TBLFEEDERMAST\" INNER JOIN \"TBLFEEDEROFFCODE\" ON \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" " +
                        " WHERE CAST (\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbZone.SelectedValue + "%' " +
                        " ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName);
                    cmbDiv.Items.Clear();
                    cmbSubDivision.Items.Clear();
                    cmbOMSection.Items.Clear();

                }

                else
                {
                    cmbCircle.Items.Clear();
                    cmbDiv.Items.Clear();
                    cmbSubDivision.Items.Clear();
                    cmbOMSection.Items.Clear();
                    cmbFeederName.Items.Clear();

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
                    Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE \"DIV_CICLE_CODE\"='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDiv);
                    Genaral.Load_Combo("  SELECT  DISTINCT\"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" " +
                       " FROM \"TBLFEEDERMAST\" INNER JOIN \"TBLFEEDEROFFCODE\" ON \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" " +
                       " WHERE CAST (\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbCircle.SelectedValue + "%' " +
                       " ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName);
                    cmbSubDivision.Items.Clear();
                    cmbOMSection.Items.Clear();
                   // cmbFeederName.Items.Clear();
                }
                else
                {

                    cmbDiv.Items.Clear();
                    cmbSubDivision.Items.Clear();
                    cmbOMSection.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDiv.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE CAST(\"SD_DIV_CODE\" AS TEXT)='" + cmbDiv.SelectedValue + "'", "--Select--", cmbSubDivision);
                    Genaral.Load_Combo("  SELECT  DISTINCT\"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" " +
                      " FROM \"TBLFEEDERMAST\" INNER JOIN \"TBLFEEDEROFFCODE\" ON \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" " +
                      " WHERE CAST (\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbDiv.SelectedValue + "%' " +
                      " ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName);
                    cmbOMSection.Items.Clear();
                   // cmbFeederName.Items.Clear();
                }
                else
                {
                    cmbSubDivision.Items.Clear();
                    cmbOMSection.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbSubDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubDivision.SelectedIndex > 0)
                {
                    //Genaral.Load_Combo("SELECT \"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" and CAST(\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbSubDivision.SelectedValue + "%'  ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName);

                    Genaral.Load_Combo("  SELECT  DISTINCT\"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\" INNER JOIN \"TBLFEEDEROFFCODE\" ON \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" WHERE CAST (\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbSubDivision.SelectedValue + "%'  ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName);
                    Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE CAST(\"OM_SUBDIV_CODE\" AS TEXT)='" + cmbSubDivision.SelectedValue + "'", "--Select--", cmbOMSection);
                    //Genaral.Load_Combo("  SELECT  DISTINCT\"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" " +
                    //     " FROM \"TBLFEEDERMAST\" INNER JOIN \"TBLFEEDEROFFCODE\" ON \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" " +
                    //     " WHERE CAST (\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbSubDivision.SelectedValue + "%' " +
                    //     " ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName);
                   
                }

                else
                {
                    cmbFeederName.Items.Clear();
                    cmbOMSection.Items.Clear();
                    cmbFeederName.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void cmbOMSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbOMSection.SelectedIndex > 0)
                {
                    //Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE CAST(\"OM_SUBDIV_CODE\" AS TEXT)='" + cmbSubDivision.SelectedValue + "'", "--Select--", cmbOMSection);

                    //Genaral.Load_Combo("SELECT \"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" and CAST(\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbSubDivision.SelectedValue + "%'  ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName);

                    //Genaral.Load_Combo("  SELECT  DISTINCT\"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\" INNER JOIN \"TBLFEEDEROFFCODE\" ON \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" WHERE CAST (\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbSubDivision.SelectedValue + "%'  ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName);
                    Genaral.Load_Combo("  SELECT  DISTINCT\"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" " +
                         " FROM \"TBLFEEDERMAST\" INNER JOIN \"TBLFEEDEROFFCODE\" ON \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" " +
                         " WHERE CAST (\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbOMSection.SelectedValue + "%' " +
                         " ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName);

                }

                else
                {
                    cmbFeederName.Items.Clear();
                    cmbOMSection.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
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
            if (cmbSubDivision.SelectedIndex > 0)
            {
                strOfficeId = cmbSubDivision.SelectedValue.ToString();
            }
            if (cmbOMSection.SelectedIndex > 0)
            {
                strOfficeId = cmbOMSection.SelectedValue.ToString();
            }
            return (strOfficeId);
        }

        protected void cmdReport_Click(object sender, EventArgs e)
        {
            try
            {
                string strOfficeCode = string.Empty;
                string strParam = string.Empty;

                clsReports objReport = new clsReports();
                if (cmbFeederName.SelectedIndex != 0)
                {
                    objReport.sFeeder = cmbFeederName.SelectedValue;
                }
                if (cmbSchemeType.SelectedIndex != 0)
                {
                    objReport.sSchemeType = cmbSchemeType.SelectedValue;
                }
                if (cmbCapacity.SelectedIndex != 0)
                {
                    objReport.sCapacity = cmbCapacity.SelectedValue;
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
                else
                {
                    string strofficecode = GetOfficeID();
                    objReport.sOfficeCode = strofficecode;

                }


                strParam = "id=DTCReportFeeder&OfficeCode=" + objReport.sOfficeCode + "&FeederName=" + objReport.sFeeder + "&SchemaType=" + objReport.sSchemeType + "&Capacity=" + objReport.sCapacity;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

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
        protected void Export_click(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            clsReports objReport = new clsReports();

            if (cmbFeederName.SelectedIndex != 0)
            {
                objReport.sFeeder = cmbFeederName.SelectedValue;
            }
            if (cmbSchemeType.SelectedIndex != 0)
            {
                objReport.sSchemeType = cmbSchemeType.SelectedValue;
            }
            if (cmbCapacity.SelectedIndex != 0)
            {
                objReport.sCapacity = cmbCapacity.SelectedValue;
            }
            string strofficecode = GetOfficeID();
            objReport.sOfficeCode = strofficecode;

            dt = objReport.PrintDTCCReport(objReport);

            if (dt.Rows.Count > 0)
            {
                string[] arrAlpha = Genaral.getalpha();
                string sMergeRange = arrAlpha[dt.Columns.Count - 1];
                using (XLWorkbook wb = new XLWorkbook())
                {

                    dt.Columns["DT_CODE"].ColumnName = "DTC Code";
                    dt.Columns["DT_NAME"].ColumnName = "DTC Name";
                    dt.Columns["TC_MAKE_ID"].ColumnName = "Make Name";
                    dt.Columns["TC_CODE"].ColumnName = "DTR Code";
                    dt.Columns["TC_CAPACITY"].ColumnName = "Capacity(in KVA)";

                    dt.Columns["FD_FEEDER_NAME"].ColumnName = "Feeder Name";
                    dt.Columns["FD_FEEDER_CODE"].ColumnName = "Feeder Code";

                    dt.Columns["ZONE"].SetOrdinal(0);
                    dt.Columns["ZONE"].ColumnName = "Zone";
                    dt.Columns["CIRCLE"].SetOrdinal(1);
                    dt.Columns["CIRCLE"].ColumnName = "Circle";
                    dt.Columns["DIVISION"].SetOrdinal(2);
                    dt.Columns["DIVISION"].ColumnName = "Division";
                    dt.Columns["SUBDIVISION"].SetOrdinal(3);
                    dt.Columns["SUBDIVISION"].ColumnName = "SubDivision";
                    dt.Columns["SECTION"].SetOrdinal(4);
                    dt.Columns["SECTION"].ColumnName = "Section";

                    wb.Worksheets.Add(dt, "DTCReport");

                    wb.Worksheet(1).Row(1).InsertRowsAbove(3);

                    var rangeheader = wb.Worksheet(1).Range("A1:" + sMergeRange + "1");
                    rangeheader.Merge().Style.Font.SetBold().Font.FontSize = 16;
                    rangeheader.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    rangeheader.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangeheader.SetValue("Hubli Electricity Supply Company Limited, (HESCOM)");

                    var rangeReporthaed = wb.Worksheet(1).Range("A2:" + sMergeRange + "2");
                    rangeReporthaed.Merge().Style.Font.SetBold().Font.FontSize = 12;
                    rangeReporthaed.Merge().Style.Fill.BackgroundColor = XLColor.AirForceBlue;
                    rangeReporthaed.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangeReporthaed.SetValue("List of DTC with Details ");

                    // wb.Worksheet(1).Cell(3, 10).Value = DateTime.Now;

                    //wb.Worksheet(1).Cell(2, 1).Value = "List of DTC with Details ";
                    //wb.Worksheet(1).Cell(2, 1).Style.Font.FontColor = XLColor.AirForceBlue;
                    //wb.Worksheet(1).Cell(2, 1).Style.Font.FontSize = 14;


                    wb.Worksheet(1).Cell(3, 10).Value = DateTime.Now;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    string FileName = "DTCReport " + DateTime.Now + ".xls";
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


    }
}