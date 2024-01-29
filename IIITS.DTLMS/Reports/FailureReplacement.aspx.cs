using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using IIITS.DTLMS.BL;
using System.Data;
using System.IO;
using System.Reflection;

namespace IIITS.DTLMS.Reports
{
    public partial class FailureReplacement : System.Web.UI.Page
    {
        clsSession objSession = new clsSession();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToString(Session["clsSession"] ?? "").Length == 0)
                {
                    Response.Redirect("~/Login.aspx", false);
                }

                objSession = (clsSession)Session["clsSession"];
                string stroffCode = string.Empty;

                if (objSession.OfficeCode.Length <= 2 && objSession.OfficeCode.Length != 0)
                {
                    stroffCode = clsStoreOffice.GetZone_Circle_Div_Offcode(objSession.OfficeCode, objSession.RoleId);
                    stroffCode = (stroffCode.Length >= 3) ? stroffCode.Substring(0, Constants.Division) : stroffCode;
                }
                else
                {
                    stroffCode = objSession.OfficeCode;
                }
                string stroffCode1 = stroffCode;
                if (!IsPostBack)
                {
                    string[] monthNames = { "--Select--", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "Jan", "Feb", "Mar" };

                    // Add each month as an ListItem to the DropDownList
                    foreach (string month in monthNames)
                    {
                        ddlFromDate.Items.Add(new ListItem(month));
                    }
                    Genaral.Load_Combo("SELECT \"FY_YEARS\",\"FY_YEARS\" FROM \"TBLFINANCIALYEAR\"  WHERE \"FY_STATUS\"='1' AND \"FY_ID\" >=  6", "--Select--", cmbFinYear);
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
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                     MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Drop Down For Circle Field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbCircle.Items.Clear();
                cmbDiv.Items.Clear();

                Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE CAST(\"CM_CIRCLE_CODE\" AS TEXT) LIKE'" + cmbZone.SelectedValue + "%'", "--Select--", cmbCircle);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                     MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Drop Down For Division Field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                     MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// For Displaying Pop Msg
        /// </summary>
        /// <param name="sMsg"></param>
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
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// For Reset Text Fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnReset_Click(object sender, EventArgs e)
        {
            objSession = (clsSession)Session["clsSession"];
            string OfficeCode = string.Empty;

            if (objSession.OfficeCode.Length <= 2 && objSession.OfficeCode.Length != 0)
            {
                OfficeCode = clsStoreOffice.GetZone_Circle_Div_Offcode(objSession.OfficeCode, objSession.RoleId).Substring(0, Constants.Division);
            }
            else
            {
                OfficeCode = objSession.OfficeCode;
            }
            try
            {
                cmbFinYear.SelectedIndex = 0;
                ddlFromDate.SelectedIndex = 0;

                if ((OfficeCode ?? "").Length == 0)
                {
                    cmbZone.SelectedIndex = 0;
                    cmbCircle.Items.Clear();
                    cmbDiv.Items.Clear();
                }
                else
                {
                    switch (OfficeCode.Length)
                    {
                        case 1:
                            cmbCircle.SelectedIndex = 0;
                            cmbDiv.Items.Clear();

                            break;
                        case 2:
                            cmbDiv.SelectedIndex = 0;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        /// <summary>
        /// Getting Failure Replacement details 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Export_clickFailureReplacement(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            clsReports objReport = new clsReports();
            string previousyear = string.Empty;
            string presentyear = string.Empty;
            string selectedMonth = string.Empty;
            string CurrentYear = string.Empty;
            string CurrentMonth = string.Empty;
            string Month = string.Empty;

            if (cmbFinYear.Text == "--Select--")
            {
                ShowMsgBox("Please Select The Financial Year");
                cmbFinYear.Focus();
                return;
            }

            if (ddlFromDate.SelectedValue == "--Select--")
            {
                ShowMsgBox("Please Select The Month");
                ddlFromDate.Focus();
                return;
            }

            previousyear = cmbFinYear.SelectedValue.Split('-').GetValue(0).ToString();
            presentyear = cmbFinYear.SelectedValue.Split('-').GetValue(1).ToString();

            selectedMonth = ddlFromDate.SelectedValue;
            Month = ddlFromDate.SelectedValue;
            if (Convert.ToString(selectedMonth ?? "") != "--Select--")
            {
                if ((selectedMonth ?? "").Length > 0)
                {
                    switch (selectedMonth)
                    {
                        case "Jan":
                            selectedMonth = selectedMonth.Replace("Jan", "01");
                            break;
                        case "Feb":
                            selectedMonth = selectedMonth.Replace("Feb", "02");
                            break;
                        case "Mar":
                            selectedMonth = selectedMonth.Replace("Mar", "03");
                            break;
                        case "Apr":
                            selectedMonth = selectedMonth.Replace("Apr", "04");
                            break;
                        case "May":
                            selectedMonth = selectedMonth.Replace("May", "05");
                            break;
                        case "Jun":
                            selectedMonth = selectedMonth.Replace("Jun", "06");
                            break;
                        case "Jul":
                            selectedMonth = selectedMonth.Replace("Jul", "07");
                            break;
                        case "Aug":
                            selectedMonth = selectedMonth.Replace("Aug", "08");
                            break;
                        case "Sep":
                            selectedMonth = selectedMonth.Replace("Sep", "09");
                            break;
                        case "Oct":
                            selectedMonth = selectedMonth.Replace("Oct", "10");
                            break;
                        case "Nov":
                            selectedMonth = selectedMonth.Replace("Nov", "11");
                            break;
                        case "Dec":
                            selectedMonth = selectedMonth.Replace("Dec", "12");
                            break;
                    }

                    CurrentMonth = selectedMonth;

                    if (Convert.ToInt16(CurrentMonth) < 4)
                    {
                        objReport.sFromDate = CurrentMonth + "-" + presentyear;
                    }
                    else
                    {
                        objReport.sFromDate = CurrentMonth + "-" + previousyear;
                    }
                }
            }

            if (cmbDiv.SelectedIndex > 0)
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
                objReport.sOfficeCode = "";
            }

            dt = objReport.GetExportExcelFailureReplacement(objReport);

            if (dt.Rows.Count > 0)
            {
                #region  
                // Use Select to filter rows based on conditions
                DataRow[] filteredRows = dt.Select("DIV_NAME LIKE '%TOTAL CIRCLE:%'");
                // Update the desired columns for all matching rows
                foreach (DataRow row in filteredRows)
                {
                    row.BeginEdit();

                    row["ZONE"] = "";
                    row["CIRCLE"] = "";

                    row.EndEdit();
                }

                filteredRows = dt.Select("CIRCLE LIKE '%TOTAL ZONE :%'");
                foreach (DataRow row in filteredRows)
                {
                    row.BeginEdit();

                    row["ZONE"] = "";

                    row.EndEdit();
                }
                #endregion

                string[] arrAlpha = Genaral.getalphanew();

                string sMergeRange = arrAlpha[dt.Columns.Count - 11];
                using (XLWorkbook wb = new XLWorkbook())
                {
                    dt.Columns["ZONE"].ColumnName = "ZONE";
                    dt.Columns["CIRCLE"].ColumnName = "CIRCLE";
                    dt.Columns["DIV_NAME"].ColumnName = "DIVISION";
                    dt.Columns["PEND_BEGINING_10_KVA"].ColumnName = "10 KVA";
                    dt.Columns["PEND_BEGINING_15_KVA"].ColumnName = "15 KVA";
                    dt.Columns["PEND_BEGINING_25_KVA"].ColumnName = "25 KVA";
                    dt.Columns["PEND_BEGINING_50_KVA"].ColumnName = "50 KVA";
                    dt.Columns["PEND_BEGINING_63_KVA"].ColumnName = "63 KVA";
                    dt.Columns["PEND_BEGINING_100_KVA"].ColumnName = "100 KVA";
                    dt.Columns["PEND_BEGINING_125_KVA"].ColumnName = "125 KVA";
                    dt.Columns["PEND_BEGINING_150_KVA"].ColumnName = "150 KVA";
                    dt.Columns["PEND_BEGINING_160_KVA"].ColumnName = "160 KVA";
                    dt.Columns["PEND_BEGINING_200_KVA"].ColumnName = "200 KVA";
                    dt.Columns["PEND_BEGINING_250_KVA"].ColumnName = "250 KVA";
                    dt.Columns["PEND_BEGINING_300_KVA"].ColumnName = "300 KVA";
                    dt.Columns["PEND_BEGINING_315_KVA"].ColumnName = "315 KVA";
                    dt.Columns["PEND_BEGINING_400_KVA"].ColumnName = "400 KVA";
                    dt.Columns["PEND_BEGINING_500_KVA"].ColumnName = "500 KVA";
                    dt.Columns["PEND_BEGINING_630_KVA"].ColumnName = "630 KVA";
                    dt.Columns["PEND_BEGINING_750_KVA"].ColumnName = "750 KVA";
                    dt.Columns["PEND_BEGINING_960_KVA"].ColumnName = "960 KVA";
                    dt.Columns["PEND_BEGINING_1000_KVA"].ColumnName = "1000 KVA";
                    dt.Columns["PEND_BEGINING_1250_KVA"].ColumnName = "1250 KVA";
                    dt.Columns["TOTAL_NO_OF_PEND_BEGINING_MONTH"].ColumnName = "Total";

                    dt.Columns["COUNT_10_KVA"].ColumnName = "10 KVA ";
                    dt.Columns["COUNT_15_KVA"].ColumnName = "15 KVA ";
                    dt.Columns["COUNT_25_KVA"].ColumnName = "25 KVA ";
                    dt.Columns["COUNT_50_KVA"].ColumnName = "50 KVA ";
                    dt.Columns["COUNT_63_KVA"].ColumnName = "63 KVA ";
                    dt.Columns["COUNT_100_KVA"].ColumnName = "100 KVA ";
                    dt.Columns["COUNT_125_KVA"].ColumnName = "125 KVA ";
                    dt.Columns["COUNT_150_KVA"].ColumnName = "150 KVA ";
                    dt.Columns["COUNT_160_KVA"].ColumnName = "160 KVA ";
                    dt.Columns["COUNT_200_KVA"].ColumnName = "200 KVA ";
                    dt.Columns["COUNT_250_KVA"].ColumnName = "250 KVA ";
                    dt.Columns["COUNT_300_KVA"].ColumnName = "300 KVA ";
                    dt.Columns["COUNT_315_KVA"].ColumnName = "315 KVA ";
                    dt.Columns["COUNT_400_KVA"].ColumnName = "400 KVA ";
                    dt.Columns["COUNT_500_KVA"].ColumnName = "500 KVA ";
                    dt.Columns["COUNT_630_KVA"].ColumnName = "630 KVA ";
                    dt.Columns["COUNT_750_KVA"].ColumnName = "750 KVA ";
                    dt.Columns["COUNT_960_KVA"].ColumnName = "960 KVA ";
                    dt.Columns["COUNT_1000_KVA"].ColumnName = "1000 KVA ";
                    dt.Columns["COUNT_1250_KVA"].ColumnName = "1250 KVA ";
                    dt.Columns["TOTAL_NO_OF_FAILED_DTR"].ColumnName = "Total ";

                    dt.Columns["REPLACED_COUNT_10_KVA"].ColumnName = "10 KVA  ";
                    dt.Columns["REPLACED_COUNT_15_KVA"].ColumnName = "15 KVA  ";
                    dt.Columns["REPLACED_COUNT_25_KVA"].ColumnName = "25 KVA  ";
                    dt.Columns["REPLACED_COUNT_50_KVA"].ColumnName = "50 KVA  ";
                    dt.Columns["REPLACED_COUNT_63_KVA"].ColumnName = "63 KVA  ";
                    dt.Columns["REPLACED_COUNT_100_KVA"].ColumnName = "100 KVA  ";
                    dt.Columns["REPLACED_COUNT_125_KVA"].ColumnName = "125 KVA  ";
                    dt.Columns["REPLACED_COUNT_150_KVA"].ColumnName = "150 KVA  ";
                    dt.Columns["REPLACED_COUNT_160_KVA"].ColumnName = "160 KVA  ";
                    dt.Columns["REPLACED_COUNT_200_KVA"].ColumnName = "200 KVA  ";
                    dt.Columns["REPLACED_COUNT_250_KVA"].ColumnName = "250 KVA  ";
                    dt.Columns["REPLACED_COUNT_300_KVA"].ColumnName = "300 KVA  ";
                    dt.Columns["REPLACED_COUNT_315_KVA"].ColumnName = "315 KVA  ";
                    dt.Columns["REPLACED_COUNT_400_KVA"].ColumnName = "400 KVA  ";
                    dt.Columns["REPLACED_COUNT_500_KVA"].ColumnName = "500 KVA  ";
                    dt.Columns["REPLACED_COUNT_630_KVA"].ColumnName = "630 KVA  ";
                    dt.Columns["REPLACED_COUNT_750_KVA"].ColumnName = "750 KVA  ";
                    dt.Columns["REPLACED_COUNT_960_KVA"].ColumnName = "960 KVA  ";
                    dt.Columns["REPLACED_COUNT_1000_KVA"].ColumnName = "1000 KVA  ";
                    dt.Columns["REPLACED_COUNT_1250_KVA"].ColumnName = "1250 KVA  ";
                    dt.Columns["TOTAL_NO_OF_REPLACED_DTR"].ColumnName = "Total  ";

                    dt.Columns["OVERALL_PEND_10_KVA"].ColumnName = " 10 KVA";
                    dt.Columns["OVERALL_PEND_15_KVA"].ColumnName = " 15 KVA";
                    dt.Columns["OVERALL_PEND_25_KVA"].ColumnName = " 25 KVA";
                    dt.Columns["OVERALL_PEND_50_KVA"].ColumnName = " 50 KVA";
                    dt.Columns["OVERALL_PEND_63_KVA"].ColumnName = " 63 KVA";
                    dt.Columns["OVERALL_PEND_100_KVA"].ColumnName = " 100 KVA";
                    dt.Columns["OVERALL_PEND_125_KVA"].ColumnName = " 125 KVA";
                    dt.Columns["OVERALL_PEND_150_KVA"].ColumnName = " 150 KVA";
                    dt.Columns["OVERALL_PEND_160_KVA"].ColumnName = " 160 KVA";
                    dt.Columns["OVERALL_PEND_200_KVA"].ColumnName = " 200 KVA";
                    dt.Columns["OVERALL_PEND_250_KVA"].ColumnName = " 250 KVA";
                    dt.Columns["OVERALL_PEND_300_KVA"].ColumnName = " 300 KVA";
                    dt.Columns["OVERALL_PEND_315_KVA"].ColumnName = " 315 KVA";
                    dt.Columns["OVERALL_PEND_400_KVA"].ColumnName = " 400 KVA";
                    dt.Columns["OVERALL_PEND_500_KVA"].ColumnName = " 500 KVA";
                    dt.Columns["OVERALL_PEND_630_KVA"].ColumnName = " 630 KVA";
                    dt.Columns["OVERALL_PEND_750_KVA"].ColumnName = " 750 KVA";
                    dt.Columns["OVERALL_PEND_960_KVA"].ColumnName = " 960 KVA";
                    dt.Columns["OVERALL_PEND_1000_KVA"].ColumnName = " 1000 KVA";
                    dt.Columns["OVERALL_PEND_1250_KVA"].ColumnName = " 1250 KVA";
                    dt.Columns["OVERALL_TOTAL_PEND_REPLACEMENT"].ColumnName = " Total";

                    #region
                    dt.Columns["ZONE"].SetOrdinal(0);
                    dt.Columns["CIRCLE"].SetOrdinal(1);
                    dt.Columns["DIVISION"].SetOrdinal(2);
                    dt.Columns["10 KVA"].SetOrdinal(3);
                    dt.Columns["15 KVA"].SetOrdinal(4);
                    dt.Columns["25 KVA"].SetOrdinal(5);
                    dt.Columns["50 KVA"].SetOrdinal(6);
                    dt.Columns["63 KVA"].SetOrdinal(7);
                    dt.Columns["100 KVA"].SetOrdinal(8);
                    dt.Columns["125 KVA"].SetOrdinal(9);
                    dt.Columns["150 KVA"].SetOrdinal(10);
                    dt.Columns["160 KVA"].SetOrdinal(11);
                    dt.Columns["200 KVA"].SetOrdinal(12);
                    dt.Columns["250 KVA"].SetOrdinal(13);
                    dt.Columns["300 KVA"].SetOrdinal(14);
                    dt.Columns["315 KVA"].SetOrdinal(15);
                    dt.Columns["400 KVA"].SetOrdinal(16);
                    dt.Columns["500 KVA"].SetOrdinal(17);
                    dt.Columns["630 KVA"].SetOrdinal(18);
                    dt.Columns["750 KVA"].SetOrdinal(19);
                    dt.Columns["960 KVA"].SetOrdinal(20);
                    dt.Columns["1000 KVA"].SetOrdinal(21);
                    dt.Columns["1250 KVA"].SetOrdinal(22);
                    dt.Columns["Total"].SetOrdinal(23);

                    dt.Columns["10 KVA "].SetOrdinal(24);
                    dt.Columns["15 KVA "].SetOrdinal(25);
                    dt.Columns["25 KVA "].SetOrdinal(26);
                    dt.Columns["50 KVA "].SetOrdinal(27);
                    dt.Columns["63 KVA "].SetOrdinal(28);
                    dt.Columns["100 KVA "].SetOrdinal(29);
                    dt.Columns["125 KVA "].SetOrdinal(30);
                    dt.Columns["150 KVA "].SetOrdinal(31);
                    dt.Columns["160 KVA "].SetOrdinal(32);
                    dt.Columns["200 KVA "].SetOrdinal(33);
                    dt.Columns["250 KVA "].SetOrdinal(34);
                    dt.Columns["300 KVA "].SetOrdinal(35);
                    dt.Columns["315 KVA "].SetOrdinal(36);
                    dt.Columns["400 KVA "].SetOrdinal(37);
                    dt.Columns["500 KVA "].SetOrdinal(38);
                    dt.Columns["630 KVA "].SetOrdinal(39);
                    dt.Columns["750 KVA "].SetOrdinal(40);
                    dt.Columns["960 KVA "].SetOrdinal(41);
                    dt.Columns["1000 KVA "].SetOrdinal(42);
                    dt.Columns["1250 KVA "].SetOrdinal(43);
                    dt.Columns["Total "].SetOrdinal(44);

                    dt.Columns["10 KVA  "].SetOrdinal(45);
                    dt.Columns["15 KVA  "].SetOrdinal(46);
                    dt.Columns["25 KVA  "].SetOrdinal(47);
                    dt.Columns["50 KVA  "].SetOrdinal(48);
                    dt.Columns["63 KVA  "].SetOrdinal(49);
                    dt.Columns["100 KVA  "].SetOrdinal(50);
                    dt.Columns["125 KVA  "].SetOrdinal(51);
                    dt.Columns["150 KVA  "].SetOrdinal(52);
                    dt.Columns["160 KVA  "].SetOrdinal(53);
                    dt.Columns["200 KVA  "].SetOrdinal(54);
                    dt.Columns["250 KVA  "].SetOrdinal(55);
                    dt.Columns["300 KVA  "].SetOrdinal(56);
                    dt.Columns["315 KVA  "].SetOrdinal(57);
                    dt.Columns["400 KVA  "].SetOrdinal(58);
                    dt.Columns["500 KVA  "].SetOrdinal(59);
                    dt.Columns["630 KVA  "].SetOrdinal(60);
                    dt.Columns["750 KVA  "].SetOrdinal(61);
                    dt.Columns["960 KVA  "].SetOrdinal(62);
                    dt.Columns["1000 KVA  "].SetOrdinal(63);
                    dt.Columns["1250 KVA  "].SetOrdinal(64);
                    dt.Columns["Total  "].SetOrdinal(65);

                    dt.Columns[" 10 KVA"].SetOrdinal(66);
                    dt.Columns[" 15 KVA"].SetOrdinal(67);
                    dt.Columns[" 25 KVA"].SetOrdinal(68);
                    dt.Columns[" 50 KVA"].SetOrdinal(69);
                    dt.Columns[" 63 KVA"].SetOrdinal(70);
                    dt.Columns[" 100 KVA"].SetOrdinal(71);
                    dt.Columns[" 125 KVA"].SetOrdinal(72);
                    dt.Columns[" 150 KVA"].SetOrdinal(73);
                    dt.Columns[" 160 KVA"].SetOrdinal(74);
                    dt.Columns[" 200 KVA"].SetOrdinal(75);
                    dt.Columns[" 250 KVA"].SetOrdinal(76);
                    dt.Columns[" 300 KVA"].SetOrdinal(77);
                    dt.Columns[" 315 KVA"].SetOrdinal(78);
                    dt.Columns[" 400 KVA"].SetOrdinal(79);
                    dt.Columns[" 500 KVA"].SetOrdinal(80);
                    dt.Columns[" 630 KVA"].SetOrdinal(81);
                    dt.Columns[" 750 KVA"].SetOrdinal(82);
                    dt.Columns[" 960 KVA"].SetOrdinal(83);
                    dt.Columns[" 1000 KVA"].SetOrdinal(84);
                    dt.Columns[" 1250 KVA"].SetOrdinal(85);
                    dt.Columns[" Total"].SetOrdinal(86);
                    #endregion

                    #region
                    //dt.Columns["Name of the Division"].SetOrdinal(0);
                    //dt.Columns["10 KVA"].SetOrdinal(1);
                    //dt.Columns["15 KVA"].SetOrdinal(2);
                    //dt.Columns["25 KVA"].SetOrdinal(3);
                    //dt.Columns["50 KVA"].SetOrdinal(4);
                    //dt.Columns["63 KVA"].SetOrdinal(5);
                    //dt.Columns["100 KVA"].SetOrdinal(6);
                    //dt.Columns["125 KVA"].SetOrdinal(7);
                    //dt.Columns["150 KVA"].SetOrdinal(8);
                    //dt.Columns["160 KVA"].SetOrdinal(9);
                    //dt.Columns["200 KVA"].SetOrdinal(10);
                    //dt.Columns["250 KVA"].SetOrdinal(11);
                    //dt.Columns["300 KVA"].SetOrdinal(12);
                    //dt.Columns["315 KVA"].SetOrdinal(13);
                    //dt.Columns["400 KVA"].SetOrdinal(14);
                    //dt.Columns["500 KVA"].SetOrdinal(15);
                    //dt.Columns["630 KVA"].SetOrdinal(16);
                    //dt.Columns["750 KVA"].SetOrdinal(17);
                    //dt.Columns["960 KVA"].SetOrdinal(18);
                    //dt.Columns["1000 KVA"].SetOrdinal(19);
                    //dt.Columns["1250 KVA"].SetOrdinal(20);
                    //dt.Columns["Total"].SetOrdinal(21);

                    //dt.Columns["Failed 10 KVA"].SetOrdinal(22);
                    //dt.Columns["Failed 15 KVA"].SetOrdinal(23);
                    //dt.Columns["Failed 25 KVA"].SetOrdinal(24);
                    //dt.Columns["Failed 50 KVA"].SetOrdinal(25);
                    //dt.Columns["Failed 63 KVA"].SetOrdinal(26);
                    //dt.Columns["Failed 100 KVA"].SetOrdinal(27);
                    //dt.Columns["Failed 125 KVA"].SetOrdinal(28);
                    //dt.Columns["Failed 150 KVA"].SetOrdinal(29);
                    //dt.Columns["Failed 160 KVA"].SetOrdinal(30);
                    //dt.Columns["Failed 200 KVA"].SetOrdinal(31);
                    //dt.Columns["Failed 250 KVA"].SetOrdinal(32);
                    //dt.Columns["Failed 300 KVA"].SetOrdinal(33);
                    //dt.Columns["Failed 315 KVA"].SetOrdinal(34);
                    //dt.Columns["Failed 400 KVA"].SetOrdinal(35);
                    //dt.Columns["Failed 500 KVA"].SetOrdinal(36);
                    //dt.Columns["Failed 630 KVA"].SetOrdinal(37);
                    //dt.Columns["Failed 750 KVA"].SetOrdinal(38);
                    //dt.Columns["Failed 960 KVA"].SetOrdinal(39);
                    //dt.Columns["Failed 1000 KVA"].SetOrdinal(40);
                    //dt.Columns["Failed 1250 KVA"].SetOrdinal(41);
                    //dt.Columns["Failed Total"].SetOrdinal(42);

                    //dt.Columns["Replaced 10 KVA"].SetOrdinal(43);
                    //dt.Columns["Replaced 15 KVA"].SetOrdinal(44);
                    //dt.Columns["Replaced 25 KVA"].SetOrdinal(45);
                    //dt.Columns["Replaced 50 KVA"].SetOrdinal(46);
                    //dt.Columns["Replaced 63 KVA"].SetOrdinal(47);
                    //dt.Columns["Replaced 100 KVA"].SetOrdinal(48);
                    //dt.Columns["Replaced 125 KVA"].SetOrdinal(49);
                    //dt.Columns["Replaced 150 KVA"].SetOrdinal(50);
                    //dt.Columns["Replaced 160 KVA"].SetOrdinal(51);
                    //dt.Columns["Replaced 200 KVA"].SetOrdinal(52);
                    //dt.Columns["Replaced 250 KVA"].SetOrdinal(53);
                    //dt.Columns["Replaced 300 KVA"].SetOrdinal(54);
                    //dt.Columns["Replaced 315 KVA"].SetOrdinal(55);
                    //dt.Columns["Replaced 400 KVA"].SetOrdinal(56);
                    //dt.Columns["Replaced 500 KVA"].SetOrdinal(57);
                    //dt.Columns["Replaced 630 KVA"].SetOrdinal(58);
                    //dt.Columns["Replaced 750 KVA"].SetOrdinal(59);
                    //dt.Columns["Replaced 960 KVA"].SetOrdinal(60);
                    //dt.Columns["Replaced 1000 KVA"].SetOrdinal(61);
                    //dt.Columns["Replaced 1250 KVA"].SetOrdinal(62);
                    //dt.Columns["Replaced Total"].SetOrdinal(63);

                    //dt.Columns["OPend 10 KVA"].SetOrdinal(64);
                    //dt.Columns["OPend 15 KVA"].SetOrdinal(65);
                    //dt.Columns["OPend 25 KVA"].SetOrdinal(66);
                    //dt.Columns["OPend 50 KVA"].SetOrdinal(67);
                    //dt.Columns["OPend 63 KVA"].SetOrdinal(68);
                    //dt.Columns["OPend 100 KVA"].SetOrdinal(69);
                    //dt.Columns["OPend 125 KVA"].SetOrdinal(70);
                    //dt.Columns["OPend 150 KVA"].SetOrdinal(71);
                    //dt.Columns["OPend 160 KVA"].SetOrdinal(72);
                    //dt.Columns["OPend 200 KVA"].SetOrdinal(73);
                    //dt.Columns["OPend 250 KVA"].SetOrdinal(74);
                    //dt.Columns["OPend 300 KVA"].SetOrdinal(75);
                    //dt.Columns["OPend 315 KVA"].SetOrdinal(76);
                    //dt.Columns["OPend 400 KVA"].SetOrdinal(77);
                    //dt.Columns["OPend 500 KVA"].SetOrdinal(78);
                    //dt.Columns["OPend 630 KVA"].SetOrdinal(79);
                    //dt.Columns["OPend 750 KVA"].SetOrdinal(80);
                    //dt.Columns["OPend 960 KVA"].SetOrdinal(81);
                    //dt.Columns["OPend 1000 KVA"].SetOrdinal(82);
                    //dt.Columns["OPend 1250 KVA"].SetOrdinal(83);
                    //dt.Columns["OPend Total"].SetOrdinal(84);
                    #endregion

                    wb.Worksheets.Add(dt, "Failure and Replacement Details");
                    wb.Worksheet(1).Row(1).InsertRowsAbove(3);
                    var rangehead = wb.Worksheet(1).Range("A1:CI1");
                    rangehead.Merge().Style.Font.SetBold().Font.FontSize = 16;
                    rangehead.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    rangehead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangehead.SetValue("Hubli Electricity Supply Company Limited, (HESCOM)");

                    var rangeReporthead1 = wb.Worksheet(1).Range("A2:CI2");
                    rangeReporthead1.Merge().Style.Font.SetBold().Font.FontSize = 12;
                    rangeReporthead1.Merge().Style.Fill.BackgroundColor = XLColor.AirForceBlue;
                    rangeReporthead1.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangeReporthead1.SetValue("Failed and Replacement of Transformer (Month: " + Month + ") " + cmbFinYear.SelectedValue + " ");
                    wb.Worksheet(1).Cell(3, 8).Value = DateTime.Now;

                    var rangeReporthead2 = wb.Worksheet(1).Range("D3:X3");
                    rangeReporthead2.Merge().Style.Font.SetBold().Font.FontSize = 12;
                    rangeReporthead2.Merge().Style.Fill.BackgroundColor = XLColor.FromArgb(168, 174, 214);
                    rangeReporthead2.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangeReporthead2.SetValue("No. of DTr's Pending for Replacement in the Beginning of the Month");

                    var rangeReporthead3 = wb.Worksheet(1).Range("Y3:AS3");
                    rangeReporthead3.Merge().Style.Font.SetBold().Font.FontSize = 12;
                    rangeReporthead3.Merge().Style.Fill.BackgroundColor = XLColor.FromArgb(160, 201, 227);
                    rangeReporthead3.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangeReporthead3.SetValue("No. of DTr's Failed During the Month");

                    var rangeReporthead4 = wb.Worksheet(1).Range("AT3:BN3");
                    rangeReporthead4.Merge().Style.Font.SetBold().Font.FontSize = 12;
                    rangeReporthead4.Merge().Style.Fill.BackgroundColor = XLColor.FromArgb(243, 203, 248);
                    rangeReporthead4.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangeReporthead4.SetValue("No. of DTr's Replaced During the Month");

                    var rangeReporthead5 = wb.Worksheet(1).Range("BO3:CI3");
                    rangeReporthead5.Merge().Style.Font.SetBold().Font.FontSize = 12;
                    rangeReporthead5.Merge().Style.Fill.BackgroundColor = XLColor.FromArgb(203, 248, 218);
                    rangeReporthead5.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangeReporthead5.SetValue("No. of DTr's Pending for Replacement");

                    #region
                    FormatColumnContainingValue(wb.Worksheet(1), 24, "Total", XLColor.FromArgb(161, 171, 232));
                    FormatColumnContainingValue(wb.Worksheet(1), 45, "Total", XLColor.FromArgb(161, 171, 232));
                    FormatColumnContainingValue(wb.Worksheet(1), 66, "Total", XLColor.FromArgb(161, 171, 232));
                    FormatColumnContainingValue(wb.Worksheet(1), 87, "Total", XLColor.FromArgb(161, 171, 232));

                    // Apply formatting based on the condition
                    FormatRowsContainingValue(wb.Worksheet(1), "TOTAL CIRCLE:", XLColor.FromArgb(228, 188, 91));

                    FormatRowsContainingValue(wb.Worksheet(1), "TOTAL ZONE :", XLColor.FromArgb(198, 232, 182));

                    FormatRowsContainingValue(wb.Worksheet(1), "GRAND TOTAL", XLColor.FromArgb(117, 154, 191));
                    #endregion

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    string FileName = "Failure and Replacement Details " + DateTime.Now + ".xls";
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


        /// <summary>
        /// set Back Ground Color for Columns
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="targetColumn"></param>
        /// <param name="searchValue"></param>
        /// <param name="backgroundColor"></param>
        protected static void FormatColumnContainingValue(IXLWorksheet worksheet, int targetColumn, string searchValue, XLColor backgroundColor)
        {
            foreach (IXLRow row in worksheet.RowsUsed())
            {
                IXLCell cell = row.Cell(targetColumn);

                if (cell.GetString().Contains(searchValue))
                {
                    // Set background color for the entire column until the end of the entries
                    for (int rowNumber = row.RowNumber(); rowNumber <= worksheet.LastRowUsed().RowNumber(); rowNumber++)
                    {
                        worksheet.Cell(rowNumber, targetColumn).Style.Fill.SetBackgroundColor(backgroundColor);
                    }

                    // Make the entire column bold
                    for (int rowNumber = row.RowNumber(); rowNumber <= worksheet.LastRowUsed().RowNumber(); rowNumber++)
                    {
                        worksheet.Cell(rowNumber, targetColumn).Style.Font.SetBold();
                    }

                    break; // Break out of the loop once the condition is met for the column
                }
            }
        }

        /// <summary>
        /// set Back Groud Color For Rows
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="searchValue"></param>
        /// <param name="backgroundColor"></param>
        protected static void FormatRowsContainingValue(IXLWorksheet worksheet, string searchValue, XLColor backgroundColor)
        {
            foreach (IXLRow row in worksheet.RowsUsed())
            {
                bool foundValue = false;

                foreach (IXLCell cell in row.CellsUsed())
                {
                    if (cell.GetString().Contains(searchValue))
                    {
                        foundValue = true;
                        break; // Break out of the inner loop once the condition is met for the row
                    }
                }

                if (foundValue)
                {
                    // Set background color to the entire row until the end of the table
                    for (int col = 1; col <= row.LastCellUsed().Address.ColumnNumber; col++)
                    {
                        row.Cell(col).Style.Fill.SetBackgroundColor(backgroundColor);
                    }
                    // Make the entire row bold
                    row.Style.Font.SetBold();
                }
            }
        }
    }
}