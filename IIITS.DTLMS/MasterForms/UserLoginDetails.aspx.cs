using ClosedXML.Excel;
using IIITS.DTLMS.BL;
using IIITS.DTLMS.BL.MasterForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.MasterForms
{
    public partial class UserLoginDetails : System.Web.UI.Page
    {
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                else
                {
                    objSession = (clsSession)Session["clsSession"];
                }

                if (!IsPostBack)
                {
                    Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone);
                }

                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");
                CalendarExtender2.EndDate = System.DateTime.Now;
                CalendarExtender1.EndDate = System.DateTime.Now;

            }
            catch (Exception ex)
            {
                if (!ex.Message.ToUpper().Contains("THREAD WAS BEING ABORTED"))
                {
                    clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                    lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                }
            }
        }
        #region Requirement got 24th Jan 2023
        /*
        form to show the user's login and logout durations  
        */
        #endregion
        /// <summary>
        /// To export the datatable in the excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                string sResult = string.Empty;
                ClsUserLoginDetails objuserlogin = new ClsUserLoginDetails();

                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    sResult = Genaral.DateComparisionforUser(txtToDate.Text, txtFromDate.Text, false, false);
                    if (sResult == "2")
                    {
                        ShowMsgBox("To Date should be Greater than or equal to From Date");
                        txtToDate.Focus();
                        return;

                    }
                }

                if (cmbSec.SelectedIndex > 0)
                {
                    objuserlogin.OfficeCode = cmbSec.SelectedValue;
                }
                else if (cmbSubDiv.SelectedIndex > 0)
                {
                    objuserlogin.OfficeCode = cmbSubDiv.SelectedValue;
                }
                else if (cmbDiv.SelectedIndex > 0)
                {
                    objuserlogin.OfficeCode = cmbDiv.SelectedValue;
                }
                else if (cmbCircle.SelectedIndex > 0)
                {
                    objuserlogin.OfficeCode = cmbCircle.SelectedValue;
                }
                else if (cmbZone.SelectedIndex > 0)
                {
                    objuserlogin.OfficeCode = cmbZone.SelectedValue;
                }
                else
                {

                    objuserlogin.OfficeCode = "";
                }
                objuserlogin.storeoffcode = GetOfficeID();
                // if ((txtFromDate.Text ?? "").Length == 0)
                if (txtFromDate.Text != null && txtFromDate.Text != "")
                {
                    objuserlogin.FromDate = txtFromDate.Text;
                }
                if (txtToDate.Text != null && txtToDate.Text != "")
                {
                    objuserlogin.Todate = txtToDate.Text;
                }
                dt = objuserlogin.GetUserLoginDetails(objuserlogin);

                if (dt.Rows.Count > 0)
                {
                    string[] arrAlpha = Genaral.getalpha();

                    string MergeRange = arrAlpha[dt.Columns.Count - 1];

                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        dt.Columns["Sl_No"].ColumnName = "SL NO";
                        dt.Columns["US_FULL_NAME"].ColumnName = "USER NAME";
                        dt.Columns["RO_NAME"].ColumnName = "ROLE";
                        dt.Columns["CORPORATE"].ColumnName = "CORPORATE";
                        dt.Columns["ZONE"].ColumnName = "ZONE";
                        dt.Columns["CIRCLE"].ColumnName = "CIRCLE";
                        dt.Columns["DIVISION"].ColumnName = "DIVISION";
                        dt.Columns["SUBDIVISION"].ColumnName = "SUBDIVISION";
                        dt.Columns["SECTION"].ColumnName = "SECTION";
                        dt.Columns["US_MOBILE_NO"].ColumnName = "MOBILE NO";
                        dt.Columns["ULD_LOGIN"].ColumnName = "LAST LOGIN TIME (YYYY-MM-DD HH:MM:SS)";
                        dt.Columns["ULD_LOGOUT"].ColumnName = "LAST LOGOUT TIME (YYYY-MM-DD HH:MM:SS)";
                        dt.Columns["ULD_DURATION"].ColumnName = "TIME DURATION";

                        wb.Worksheets.Add(dt, "UserLoginDetails");
                        wb.Worksheet(1).Row(1).InsertRowsAbove(3);

                        var rangeheader = wb.Worksheet(1).Range("A1:" + MergeRange + "1");
                        rangeheader.Merge().Style.Font.SetBold().Font.FontSize = 16;
                        rangeheader.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
                        rangeheader.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        rangeheader.SetValue("Hubli Electricity Supply Company Limited, (HESCOM)");


                        var rangeReporthaed = wb.Worksheet(1).Range("A2:" + MergeRange + "2");
                        rangeReporthaed.Merge().Style.Font.SetBold().Font.FontSize = 12;
                        rangeReporthaed.Merge().Style.Fill.BackgroundColor = XLColor.AirForceBlue;
                        rangeReporthaed.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        rangeReporthaed.SetValue("List of Users Login Details ");

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        string FileName = "UserLoginDetails " + DateTime.Now + ".xls";
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
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                if (!ex.Message.ToUpper().Contains("THREAD WAS BEING ABORTED"))
                {
                    clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                }
            }
        }
        public string GetOfficeID()
        {
            // for store offcode
            string strOfficeId = string.Empty;
            try
            {

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
                else
                {
                    strOfficeId = "";
                }
                return (strOfficeId);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return strOfficeId;
            }
        }
        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                cmbZone.ClearSelection();
                cmbZone.SelectedIndex = 0;
                cmbZone_SelectedIndexChanged(sender, e);
                txtFromDate.Text = "";
                txtToDate.Text = "";
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        private void ShowMsgBox(string Msg)
        {
            try
            {
                string ShowMsg = string.Empty;
                ShowMsg = "<script language=javascript> alert ('" + Msg + "')</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "Msg", ShowMsg);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbZone.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_ZO_ID\"='" + cmbZone.SelectedValue + "' ORDER BY \"CM_CIRCLE_CODE\" ", "--Select--", cmbCircle);
                    cmbDiv.Items.Clear();
                    cmbSubDiv.Items.Clear();
                    cmbSec.Items.Clear();
                }
                else
                {
                    cmbCircle.Items.Clear();
                    cmbDiv.Items.Clear();
                    cmbSubDiv.Items.Clear();
                    cmbSec.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
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
                    cmbSec.Items.Clear();
                }
                else
                {
                    cmbDiv.Items.Clear();
                    cmbSubDiv.Items.Clear();
                    cmbSec.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDiv.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_DIV_CODE\" AS TEXT)='" + cmbDiv.SelectedValue + "'", "--Select--", cmbSubDiv);
                    cmbSec.Items.Clear();
                }
                else
                {
                    cmbSubDiv.Items.Clear();
                    cmbSec.Items.Clear();
                }
                if (objSession.OfficeCode.Length > 3)
                {
                    Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE CAST(\"SD_SUBDIV_CODE\" AS TEXT)='" + objSession.OfficeCode.Substring(0, 4) + "'", cmbSubDiv);

                }
                if (objSession.OfficeCode.Length > 4)
                {
                    Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_CODE\"='" + objSession.OfficeCode.Substring(0, 5) + "'", cmbSec);
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbSubDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubDiv.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE  CAST(\"OM_SUBDIV_CODE\" AS TEXT)='" + cmbSubDiv.SelectedValue + "'", "--Select--", cmbSec);
                }
                else
                {
                    cmbSec.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}