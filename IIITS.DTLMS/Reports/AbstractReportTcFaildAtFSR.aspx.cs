using System;
using IIITS.DTLMS.BL;
using System.IO;
using ClosedXML.Excel;
using System.Data;


namespace IIITS.DTLMS.Reports
{
    public partial class AbstractReportTcFaildAtFSR : System.Web.UI.Page
    {
        string strFormCode = "AbstractReportTcFaildAtFSR";
        clsSession objSession = new clsSession();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }

                objSession = (clsSession)Session["clsSession"];
                string stroffCode = string.Empty;
                if (objSession.OfficeCode.Length <= 2 && objSession.OfficeCode.Length != 0)
                {
                    //stroffCode = objSession.OfficeCode.Substring(0, 2);
                    stroffCode = clsStoreOffice.GetZone_Circle_Div_Offcode(objSession.OfficeCode, objSession.RoleId).Substring(0, Constants.Circle);
                }
                else
                {
                    stroffCode = objSession.OfficeCode;
                }
                string stroffCode1 = stroffCode;

                txtFromDate1.Attributes.Add("readonly", "readonly");
                txtToDate1.Attributes.Add("readonly", "readonly");

             txtFromDate_CalendarExtender1.EndDate = System.DateTime.Now;
              txtToDate_CalendarExtender1.EndDate = System.DateTime.Now;
  

                if (!IsPostBack)
                {

                    if (stroffCode.Length >= 1)
                    {
                        Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone);
                        stroffCode = stroffCode.Substring(0, Constants.Zone);
                        cmbZone.Items.FindByValue(stroffCode).Selected = true;
                        cmbZone.Enabled = false;

                        stroffCode = stroffCode1;
                    }
                    if (stroffCode == null || stroffCode == "")
                    {
                        Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone);

                    }
                    if (stroffCode.Length >= 1)
                    {
                        Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_ZO_ID\"='" + cmbZone.SelectedValue + "'", "--Select--", cmbCircle);
                        if (stroffCode.Length >= 2)
                        {
                            stroffCode = stroffCode.Substring(0, Constants.Circle);
                            cmbCircle.Items.FindByValue(stroffCode).Selected = true;
                            cmbCircle.Enabled = false;
                            stroffCode = stroffCode1;
                        }
                    }

                    if (stroffCode.Length >= 2)
                    {
                        Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE \"DIV_CICLE_CODE\"='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDiv);
                        if (stroffCode.Length >= 3)
                        {
                            stroffCode = stroffCode.Substring(0, Constants.Division);
                            cmbDiv.Items.FindByValue(stroffCode).Selected = true;
                            cmbDiv.Enabled = false;
                            stroffCode = stroffCode1;
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
        protected void cmbZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbZone.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE CAST(\"CM_CIRCLE_CODE\" AS TEXT) LIKE'" + cmbZone.SelectedValue + "%'", "--Select--", cmbCircle);
                    //cmbCircle.Items.Clear();
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
                   // cmbSubDiv.Items.Clear();
                    //cmbOMSection.Items.Clear();
                }

                else
                {
                    cmbDiv.Items.Clear();
                   // cmbSubDiv.Items.Clear();
                    //cmbOMSection.Items.Clear();

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        //protected void cmbDiv_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (cmbDiv.SelectedIndex > 0)
        //        {
        //            Genaral.Load_Combo("SELECT SD_SUBDIV_CODE,SD_SUBDIV_NAME from TBLSUBDIVMAST WHERE SD_DIV_CODE='" + cmbDiv.SelectedValue + "'", "--Select--", cmbSubDiv);
        //            cmbOMSection.Items.Clear();
        //        }
        //        else
        //        {
        //            cmbSubDiv.Items.Clear();
        //            cmbOMSection.Items.Clear();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbDiv_SelectedIndexChanged");
        //    }
        //}

        //protected void cmbSubDiv_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (cmbSubDiv.SelectedIndex > 0)
        //        {
        //            Genaral.Load_Combo("SELECT OM_CODE,OM_NAME FROM TBLOMSECMAST WHERE OM_SUBDIV_CODE='" + cmbSubDiv.SelectedValue + "'", "--Select--", cmbOMSection);
        //        }
        //        else
        //        {
        //            cmbOMSection.Items.Clear();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbTaluk_SelectedIndexChanged");
        //    }
        //}
        public string GetOfficeID()
        {
            string strOfficeId = string.Empty;
            if (cmbCircle.SelectedIndex > 0)
            {
                strOfficeId = cmbCircle.SelectedValue.ToString();
            }

            if (cmbDiv.SelectedIndex > 0)
            {
                strOfficeId = cmbDiv.SelectedValue.ToString();
            }

            //if (cmbSubDiv.SelectedIndex > 0)
            //{
            //    strOfficeId = cmbSubDiv.SelectedValue.ToString();
            //}
            //if (cmbOMSection.SelectedIndex > 0)
            //{
            //    strOfficeId = cmbOMSection.SelectedValue.ToString();
            //}

            return (strOfficeId);
        }
        protected void cmdReport_Click(object sender, EventArgs e)
        {
            try
            {
                string strOfficeCode = string.Empty;
                string strParam = string.Empty;
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

                clsReports objReport = new clsReports();
                objReport.sFromDate = txtFromDate1.Text;
                objReport.sTodate = txtToDate1.Text;


                if (cmbDiv.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbDiv.SelectedValue;
                }
                else if (cmbCircle.SelectedIndex > 0)
                    objReport.sOfficeCode = cmbCircle.SelectedValue;
                else if (cmbZone.SelectedIndex > 0)
                    objReport.sOfficeCode = cmbZone.SelectedValue;
                else
                    objReport.sOfficeCode = GetOfficeID();

                strParam = "id=AbstractRptTcFailed&Officecode=" + objReport.sOfficeCode +"&FromDate="+objReport.sFromDate+"&ToDate="+objReport.sTodate ;
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
            try
            {
               
               // cmbCircle.SelectedIndex = 0;
                //cmbSubDiv.Items.Clear();
               // cmbDiv.Items.Clear();
                //cmbOMSection.Items.Clear();
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
            DataTable dtcompletedRepairCount = new DataTable();
            clsReports objReport = new clsReports();

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
            objReport.sFromDate = txtFromDate1.Text;
            objReport.sTodate = txtToDate1.Text;


            string strofficecode = GetOfficeID();
            objReport.sOfficeCode = strofficecode;

            dt = objReport.PrintAbstractReportTcFailedAtFSR(objReport);
            //dtcompletedRepairCount = objReport.PrintCompletedRepairerTcCount();

            if (dt.Rows.Count > 0)
            {
                string[] arrAlpha = Genaral.getalpha();

               
                using (XLWorkbook wb = new XLWorkbook())
                {

                   
                   
                    dt.Columns["OFF_CODE"].ColumnName = "Off Code";
                    dt.Columns["FIELD_COUNT"].ColumnName = "@Field";
                    dt.Columns["FIELD_COUNT_TOBEREPLACED"].ColumnName = "Tc To Be Replace";
                    dt.Columns["STORE_COUNT"].ColumnName = "@Store";
                    dt.Columns["REPAIRER_COUNT"].ColumnName = "@Repairer";
                  

                    dt.Columns["CIRCLE"].SetOrdinal(0);
                    dt.Columns["CIRCLE"].ColumnName = "Circle";
                    dt.Columns["CAPACITY"].SetOrdinal(2);
                    dt.Columns["CAPACITY"].ColumnName = "Capacity";
                    dt.Columns["DIV"].SetOrdinal(1);
                    dt.Columns["DIV"].ColumnName = "Division";
                    dt.Columns.Remove("Off Code");
                  
                    wb.Worksheets.Add(dt, "AbstractTcFailed");
                    //wb.Worksheet(1).Cell(1,5).InsertTable(dtcompletedRepairCount);

                    wb.Worksheet(1).Row(1).InsertRowsAbove(3);
                   // wb.Worksheet(1).Column(4).Delete();

                    string sMergeRange = arrAlpha[dt.Columns.Count - 1];
                    var rangehead = wb.Worksheet(1).Range("A1:" + sMergeRange + "1");
                    rangehead.Merge().Style.Font.SetBold().Font.FontSize = 14;
                    rangehead.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    rangehead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangehead.SetValue("Hubli Electricity Supply Company Limited, HESCOM)");

                    var rangeReporthead = wb.Worksheet(1).Range("A2:" + sMergeRange + "2");
                    rangeReporthead.Merge().Style.Font.SetBold().Font.FontSize = 12;
                    rangeReporthead.Merge().Style.Fill.BackgroundColor = XLColor.AirForceBlue;
                    rangeReporthead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangeReporthead.SetValue("Details of the failed Tc at Repair centre/ Store/ Field    as on  " + DateTime.Now);



                    wb.Worksheet(1).Cell(3, 7).Value = DateTime.Now;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    string FileName = "AbstractTcFailed " + DateTime.Now + ".xls";
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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}