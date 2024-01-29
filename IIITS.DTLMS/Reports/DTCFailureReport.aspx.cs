using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.IO;
using ClosedXML.Excel;
using System.Configuration;
namespace IIITS.DTLMS.Reports
{
    public partial class DTCFailureReport : System.Web.UI.Page
    {
        string strFormCode = "DTCFailureReport";
        clsSession objSession = new clsSession();
        int Zone_code = Convert.ToInt32(ConfigurationManager.AppSettings["Zone_code"]);
        int Circle_code = Convert.ToInt32(ConfigurationManager.AppSettings["Circle_code"]);
        int Division_code = Convert.ToInt32(ConfigurationManager.AppSettings["Division_code"]);
        int SubDiv_code = Convert.ToInt32(ConfigurationManager.AppSettings["SubDiv_code"]);
        int Section_code = Convert.ToInt32(ConfigurationManager.AppSettings["Section_code"]);
        int Feeder_code = Convert.ToInt32(ConfigurationManager.AppSettings["feeder_code"]);
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
                    stroffCode = clsStoreOffice.GetZone_Circle_Div_Offcode(objSession.OfficeCode, objSession.RoleId).Substring(0, Circle_code);
                }
                else
                {
                    stroffCode = objSession.OfficeCode;
                }
                string stroffCode1 = stroffCode;
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");
                txtFromDate2.Attributes.Add("readonly", "readonly");
                txtToDate2.Attributes.Add("readonly", "readonly");
                //txtFailMonth.Attributes.Add("readonly", "readonly");
                txtFromDate3.Attributes.Add("readonly", "readonly");
                txtToDate3.Attributes.Add("readonly", "readonly");
                CalendarExtender6.EndDate = System.DateTime.Now;
                CalendarExtender5.EndDate = System.DateTime.Now;
                //dtPickerFromDate.EndDate = System.DateTime.Now;
                //CalendarExtender2.EndDate = System.DateTime.Now;
                //CalendarExtender1.EndDate = System.DateTime.Now;
                CalendarExtender4.EndDate = System.DateTime.Now;
                CalendarExtender3.EndDate = System.DateTime.Now;
                
                if (!IsPostBack)
                {
                    Genaral.Load_Combo("SELECT \"TM_ID\",\"TM_NAME\" FROM \"TBLTRANSMAKES\" ORDER BY \"TM_ID\"", "--Select--", cmbMake);
                    Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='FT' ORDER BY \"MD_ID\"", "--Select--", cmbFailureType);
                    Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='FT' ORDER BY \"MD_ID\"", "--Select--", cmbFailureType1);
                    Genaral.Load_Combo("SELECT \"MD_ORDER_BY\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='C' ORDER BY \"MD_ORDER_BY\"", "--Select--", cmbCapacity1);

                    if (stroffCode == null || stroffCode == "")
                    {
                        Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone);
                        Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone1);
                        Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone2);
                    }
                    if (stroffCode.Length >= 1)
                    {
                        Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone);
                        Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone1);
                        Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone2);
                        stroffCode = stroffCode1.Substring(0, Zone_code);
                        cmbZone.Items.FindByValue(stroffCode).Selected = true;
                        cmbZone1.Items.FindByValue(stroffCode).Selected = true;
                        cmbZone2.Items.FindByValue(stroffCode).Selected = true;
                        cmbZone.Enabled = false;
                        cmbZone1.Enabled = false;
                        cmbZone2.Enabled = false;
                        stroffCode = stroffCode1;
                    }
                    if (stroffCode.Length >= 1)
                    {
                        Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_ZO_ID\"='" + cmbZone.SelectedValue + "'", "--Select--", cmbCircle);
                        Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_ZO_ID\"='" + cmbZone.SelectedValue + "'", "--Select--", cmbCircle2);
                        Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_ZO_ID\"='" + cmbZone.SelectedValue + "'", "--Select--", cmbCircle3);

                        if (stroffCode.Length >= 2)
                        {
                            stroffCode = stroffCode1.Substring(0, Circle_code);
                            cmbCircle.Items.FindByValue(stroffCode).Selected = true;
                            cmbCircle2.Items.FindByValue(stroffCode).Selected = true;
                            cmbCircle3.Items.FindByValue(stroffCode).Selected = true;
                            cmbCircle.Enabled = false;
                            cmbCircle2.Enabled = false;
                            cmbCircle3.Enabled = false;
                            stroffCode = stroffCode1;
                        }
                    }
                    if (stroffCode.Length >= 2)
                    {
                        Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE \"DIV_CICLE_CODE\"='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDiv);
                        Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE \"DIV_CICLE_CODE\"='" + cmbCircle2.SelectedValue + "'", "--Select--", cmbDiv2);
                        Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE \"DIV_CICLE_CODE\"='" + cmbCircle3.SelectedValue + "'", "--Select--", cmbDivision3);
                        if (stroffCode.Length >= 3)
                        {
                            stroffCode = stroffCode1.Substring(0, Division_code);
                            cmbDiv.Items.FindByValue(stroffCode).Selected = true;
                            cmbDiv2.Items.FindByValue(stroffCode).Selected = true;
                            cmbDivision3.Items.FindByValue(stroffCode).Selected = true;
                            cmbDiv.Enabled = false;
                            cmbDiv2.Enabled = false;
                            cmbDivision3.Enabled = false;
                            stroffCode = stroffCode1;
                        }
                    }
                    if (stroffCode.Length >= 3)
                    {
                        Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE \"SD_DIV_CODE\"='" + cmbDiv.SelectedValue + "'", "--Select--", cmbSubDiv);
                        Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE \"SD_DIV_CODE\"='" + cmbDiv2.SelectedValue + "'", "--Select--", cmbSubDiv2);
                        Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE \"SD_DIV_CODE\"='" + cmbDivision3.SelectedValue + "'", "--Select--", cmbSubDivision3);
                        if (stroffCode.Length >= 4)
                        {
                            stroffCode = stroffCode1.Substring(0, SubDiv_code);
                            cmbSubDiv.Items.FindByValue(stroffCode).Selected = true;
                            cmbSubDiv2.Items.FindByValue(stroffCode).Selected = true;
                            cmbSubDivision3.Items.FindByValue(stroffCode).Selected = true;
                            cmbSubDiv.Enabled = false;
                            cmbSubDiv2.Enabled = false;
                            cmbSubDivision3.Enabled = false;
                            stroffCode = stroffCode1;
                        }
                    }
                    if (stroffCode.Length >= 4)
                    {
                        Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_SUBDIV_CODE\"='" + cmbSubDiv.SelectedValue + "'", "--Select--", cmbOMSection);
                        Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_SUBDIV_CODE\"='" + cmbSubDiv2.SelectedValue + "'", "--Select--", cmbSection);
                        Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_SUBDIV_CODE\"='" + cmbSubDivision3.SelectedValue + "'", "--Select--", cmbOMSection3);
                        Genaral.Load_Combo("SELECT DISTINCT \"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" " +
                            " where \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" and CAST(\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbSubDiv.SelectedValue + "%' " +
                            " ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName1);
                        Genaral.Load_Combo("SELECT DISTINCT \"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" and CAST(\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbSubDiv2.SelectedValue + "%'  ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName2);
                        Genaral.Load_Combo("SELECT DISTINCT \"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" " +
                            " where \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" and CAST(\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbSubDivision3.SelectedValue + "%' " +
                            " ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName3);
                        if (stroffCode.Length >= 5)
                        {
                            stroffCode = stroffCode1.Substring(0, Section_code);
                            cmbOMSection.Items.FindByValue(stroffCode).Selected = true;
                            cmbSection.Items.FindByValue(stroffCode).Selected = true;
                            cmbOMSection3.Items.FindByValue(stroffCode).Selected = true;
                            cmbOMSection.Enabled = false;
                            cmbSection.Enabled = false;
                            cmbOMSection3.Enabled = false;
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
        #region Pending / Completed Failure Details Dropdown OnChange
        protected void cmbZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbZone.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" " +
                        " WHERE \"CM_ZO_ID\"='" + cmbZone.SelectedValue + "'", "--Select--", cmbCircle);

                    Genaral.Load_Combo("SELECT DISTINCT \"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" " +
                       " where \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" and CAST(\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbZone.SelectedValue + "%' " +
                       " ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName1);

                    cmbDiv.Items.Clear();
                    cmbSubDiv.Items.Clear();
                    cmbOMSection.Items.Clear();
                    cmbFeederName1.Items.Clear();
                }
                else
                {
                    cmbCircle.Items.Clear();
                    cmbDiv.Items.Clear();
                    cmbSubDiv.Items.Clear();
                    cmbOMSection.Items.Clear();
                    cmbFeederName1.Items.Clear();
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
                    Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" " +
                        " WHERE \"DIV_CICLE_CODE\"='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDiv);

                    Genaral.Load_Combo("SELECT DISTINCT \"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" " +
                       " where \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" and CAST(\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbCircle.SelectedValue + "%' " +
                       " ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName1);
                    cmbSubDiv.Items.Clear();
                    cmbOMSection.Items.Clear();
                    cmbFeederName1.Items.Clear();
                }
                else
                {
                    cmbDiv.Items.Clear();
                    cmbSubDiv.Items.Clear();
                    cmbOMSection.Items.Clear();
                    cmbFeederName1.Items.Clear();
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
                    Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" " +
                        " WHERE \"SD_DIV_CODE\"='" + cmbDiv.SelectedValue + "'", "--Select--", cmbSubDiv);

                    Genaral.Load_Combo("SELECT DISTINCT \"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" " +
                       " where \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" and CAST(\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbDiv.SelectedValue + "%' " +
                       " ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName1);
                    cmbOMSection.Items.Clear();
                    //cmbFeederName1.Items.Clear();
                }
                else
                {
                    cmbSubDiv.Items.Clear();
                    cmbOMSection.Items.Clear();
                    cmbFeederName1.Items.Clear();
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
                    Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" " +
                        " WHERE \"OM_SUBDIV_CODE\"='" + cmbSubDiv.SelectedValue + "'", "--Select--", cmbOMSection);
                    Genaral.Load_Combo("SELECT DISTINCT \"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" " +
                        " where \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" and CAST(\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbSubDiv.SelectedValue + "%' " +
                        " ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName1);
                }

                else
                {
                    cmbOMSection.Items.Clear();
                    cmbFeederName1.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbOMSection.SelectedIndex > 0)
                {

                    Genaral.Load_Combo("SELECT DISTINCT \"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" " +
                        " where \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" and CAST(\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbOMSection.SelectedValue + "%' " +
                        " ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName1);
                }
                else
                {
                    cmbFeederName1.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        #endregion

        #region FREQUENTLY FAILING DTC'S Dropdowns on change
        protected void cmbZone2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbZone2.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" " +
                        " WHERE \"CM_ZO_ID\"='" + cmbZone2.SelectedValue + "'", "--Select--", cmbCircle3);

                    Genaral.Load_Combo("SELECT DISTINCT \"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" " +
                        " where \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" and CAST(\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbZone2.SelectedValue + "%' " +
                        " ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName3);
                    cmbDivision3.Items.Clear();
                    cmbSubDivision3.Items.Clear();
                    cmbOMSection3.Items.Clear();
                }
                else
                {
                    cmbCircle3.Items.Clear();
                    cmbDivision3.Items.Clear();
                    cmbSubDivision3.Items.Clear();
                    cmbOMSection3.Items.Clear();
                    cmbFeederName3.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbCircle3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle3.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE \"DIV_CICLE_CODE\"='" + cmbCircle3.SelectedValue + "'", "--Select--", cmbDivision3);
                    Genaral.Load_Combo("SELECT DISTINCT \"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" " +
                        " where \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" and CAST(\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbCircle3.SelectedValue + "%' " +
                        " ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName3);
                    cmbSubDivision3.Items.Clear();
                    cmbOMSection3.Items.Clear();
                }
                else
                {
                    cmbDivision3.Items.Clear();
                    cmbSubDivision3.Items.Clear();
                    cmbOMSection3.Items.Clear();
                    cmbFeederName3.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbDivision3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDivision3.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE \"SD_DIV_CODE\"='" + cmbDivision3.SelectedValue + "'", "--Select--", cmbSubDivision3);
                    Genaral.Load_Combo("SELECT DISTINCT \"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" " +
                        " where \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" and CAST(\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbDivision3.SelectedValue + "%' " +
                        " ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName3);
                    cmbOMSection3.Items.Clear();
                }
                else
                {
                    cmbSubDivision3.Items.Clear();
                    cmbOMSection3.Items.Clear();
                    cmbFeederName3.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbSubDivision3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubDivision3.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" " +
                        " WHERE \"OM_SUBDIV_CODE\"='" + cmbSubDivision3.SelectedValue + "'", "--Select--", cmbOMSection3);
                    Genaral.Load_Combo("SELECT \"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" " +
                        " where \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" and CAST(\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbSubDivision3.SelectedValue + "%' " +
                        "  ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName3);
                }
                else
                {
                    cmbOMSection3.Items.Clear();
                    cmbFeederName3.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbSection3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbOMSection3.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" " +
                        " where \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" and CAST(\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbOMSection3.SelectedValue + "%' " +
                        "  ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName3);
                }
                else
                {
                    cmbFeederName3.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        #endregion
        #region Not Used/Not implemented Logic
        protected void cmbZone1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbZone1.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" " +
                        " WHERE \"CM_ZO_ID\"='" + cmbZone1.SelectedValue + "'", "--Select--", cmbCircle2);
                    cmbDiv2.Items.Clear();
                    cmbSubDiv2.Items.Clear();
                    cmbSection.Items.Clear();
                }
                else
                {
                    cmbCircle2.Items.Clear();
                    cmbDiv2.Items.Clear();
                    cmbSubDiv2.Items.Clear();
                    cmbSection.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbCircle1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle2.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" " +
                        " WHERE \"DIV_CICLE_CODE\"='" + cmbCircle2.SelectedValue + "'", "--Select--", cmbDiv2);
                    cmbSubDiv2.Items.Clear();
                    cmbSection.Items.Clear();
                }
                else
                {
                    cmbDiv2.Items.Clear();
                    cmbSubDiv2.Items.Clear();
                    cmbSection.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbSubDiv1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubDiv2.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_SUBDIV_CODE\"='" + cmbSubDiv2.SelectedValue + "'", "--Select--", cmbSection);
                    Genaral.Load_Combo("SELECT \"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" and CAST(\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbSubDiv2.SelectedValue + "%'  ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName2);
                }
                else
                {
                    cmbSection.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbDiv1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDiv2.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" " +
                        " WHERE \"SD_DIV_CODE\"='" + cmbDiv2.SelectedValue + "'", "--Select--", cmbSubDiv2);
                    cmbOMSection.Items.Clear();
                }
                else
                {
                    cmbSubDiv2.Items.Clear();
                    cmbSection.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                clsReports objReport = new clsReports();
                string sResult = string.Empty;
                if (txtFromDate2.Text != "")
                {
                    sResult = Genaral.DateValidation(txtFromDate2.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtFromDate2.Focus();
                        return;
                    }
                }

                if (txtToDate2.Text != "")
                {
                    sResult = Genaral.DateValidation(txtToDate2.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtToDate2.Focus();
                        return;
                    }
                }

                if (txtFromDate2.Text != "" && txtToDate2.Text != "")
                {
                    sResult = Genaral.DateComparision(txtToDate2.Text, txtFromDate2.Text, false, false);
                    if (sResult == "2")
                    {
                        ShowMsgBox("To Date should be Greater than From Date");
                        txtToDate2.Focus();
                        return;
                    }
                }

                if (cmbFeederName2.SelectedIndex != 0)
                {
                    objReport.sFeeder = cmbFeederName2.SelectedValue;
                }
                if (cmbCoil1.SelectedIndex > 0)
                    objReport.sFailType = cmbCoil1.SelectedValue.ToString();

                objReport.sType = cmbStage.SelectedValue.ToString();

                if (cmbSection.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbSection.SelectedValue;
                }

                else if (cmbSubDiv2.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbSubDiv2.SelectedValue;
                }
                else if (cmbDiv2.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbDiv2.SelectedValue;
                }
                else if (cmbCircle2.SelectedIndex > 0)
                    objReport.sOfficeCode = cmbCircle2.SelectedValue;
                else if (cmbZone1.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbZone1.SelectedValue.ToString();
                }
                else objReport.sOfficeCode = "";

                objReport.sFromDate = txtFromDate2.Text;
                objReport.sTodate = txtToDate2.Text;

                string sParam = "id=WorkOderReg&Officecode=" + objReport.sOfficeCode + "&FromDate=" + objReport.sFromDate + "&ToDate=" + objReport.sTodate + "&CoilType=" + objReport.sFailType + "&FeederName=" + objReport.sFeeder;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + sParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void BtnWOReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtFromDate2.Text = "";
                txtToDate2.Text = "";
                cmbZone1.SelectedIndex = 0;
                cmbCircle2.Items.Clear();
                cmbMake.SelectedIndex = 0;
                cmbFailureType.SelectedIndex = 0;
                cmbDiv2.Items.Clear();
                cmbSubDiv2.Items.Clear();
                cmbSection.Items.Clear();
                cmbCoil1.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public string GetOfficeIDs()
        {
            string strOfficeId = string.Empty;
            if (cmbZone1.SelectedIndex > 0)
            {
                strOfficeId = cmbZone1.SelectedValue.ToString();
            }
            if (cmbCircle2.SelectedIndex > 0)
            {
                strOfficeId = cmbCircle2.SelectedValue.ToString();
            }

            if (cmbDiv2.SelectedIndex > 0)
            {
                strOfficeId = cmbDiv2.SelectedValue.ToString();
            }

            if (cmbSubDiv2.SelectedIndex > 0)
            {
                strOfficeId = cmbSubDiv2.SelectedValue.ToString();
            }
            if (cmbSection.SelectedIndex > 0)
            {
                strOfficeId = cmbSection.SelectedValue.ToString();
            }

            return (strOfficeId);
        }
        protected void Export_clickworkorder(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
        clsReports objReport = new clsReports();

        string sResult = string.Empty;
            if (txtFromDate2.Text != "")
            {
                sResult = Genaral.DateValidation(txtFromDate2.Text);
                if (sResult != "")
                {
                    ShowMsgBox(sResult);
        txtFromDate2.Focus();
                    return;
                }
}

            if (txtToDate2.Text != "")
            {
                sResult = Genaral.DateValidation(txtToDate2.Text);
                if (sResult != "")
                {
                    ShowMsgBox(sResult);
txtToDate2.Focus();
                    return;
                }
            }

            if (txtFromDate2.Text != "" && txtToDate2.Text != "")
            {
                sResult = Genaral.DateComparision(txtToDate2.Text, txtFromDate2.Text, false, false);
                if (sResult == "2")
                {
                    ShowMsgBox("To Date should be Greater than From Date");
txtToDate2.Focus();
                    return;
                }
            }

            string strofficecodes = GetOfficeIDs();

objReport.sOfficeCode = strofficecodes;
            if (cmbFeederName2.SelectedIndex != 0)
            {
                objReport.sFeeder = cmbFeederName2.SelectedValue;
            }
            objReport.sType = cmbStage.SelectedValue.ToString();
            objReport.sFailType = cmbCoil1.SelectedValue.ToString();
            dt = objReport.WoRegDetails(objReport);

            if (dt.Rows.Count > 0)
            {
                string[] arrAlpha = Genaral.getalpha();


                using (XLWorkbook wb = new XLWorkbook())
                {

                    dt.Columns["DF_DTC_CODE"].ColumnName = "DTC Code";

                    dt.Columns["DF_EQUIPMENT_ID"].ColumnName = "DTR Code";
                    dt.Columns["EST_NO"].ColumnName = "Estimation No";
                    dt.Columns["EST_CRON"].ColumnName = "Estimate Date";
                    dt.Columns["RSM_GUARANTY_TYPE"].ColumnName = "Guaranty Type";
                    dt.Columns["WO_NO"].ColumnName = "Wo No";
                    dt.Columns["WO_NO_DECOM"].ColumnName = "DeCommission No";
                    dt.Columns["WO_AMT"].ColumnName = "Wo Amount";
                    dt.Columns["WO_AMT_DECOM"].ColumnName = "Wo DeCommission Amount";
                    dt.Columns["WO_DATE_DECOM"].ColumnName = "DeCommission Date";
                    dt.Columns["REPLACE_CAPACITY"].ColumnName = "Replace Capacity";
                    dt.Columns["TC_CAPACITY"].ColumnName = "Tc Capacity";
                    dt.Columns["DF_REASON"].ColumnName = "Reason For Failure";

                    dt.Columns["CIRCLE"].SetOrdinal(0);
dt.Columns["CIRCLE"].ColumnName = "Circle";
                    dt.Columns["DIVISION"].SetOrdinal(1);
dt.Columns["DIVISION"].ColumnName = "Division";
                    dt.Columns["SUBDIVISION"].SetOrdinal(2);
dt.Columns["SUBDIVISION"].ColumnName = "SubDivision";
                    dt.Columns["SECTION"].SetOrdinal(3);
dt.Columns["SECTION"].ColumnName = "Section";
                    dt.Columns["TODATE"].SetOrdinal(5);
dt.Columns["TODATE"].ColumnName = "TODATE";

                    dt.Columns.Remove("FROMDATE");
                    dt.Columns.Remove("TODATE");
                    dt.Columns.Remove("today");

                    wb.Worksheets.Add(dt, "DTC WorkOrder Failure ");

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
                        rangeReporthead.SetValue("WORK ORDER REGISTER ABSTRACT  From " + objReport.sFromDate + "  To " + objReport.sTodate);
                    }
                    if (txtFromDate.Text != "" && txtToDate.Text == "")
                    {
                        rangeReporthead.SetValue("WORK ORDER REGISTER ABSTRACT  From " + objReport.sFromDate + "  To " + DateTime.Now);
                    }
                    if (txtFromDate.Text == "" && txtToDate.Text != "")
                    {
                        rangeReporthead.SetValue("WORK ORDER REGISTER ABSTRACT  as on " + objReport.sTodate);
                    }
                    if (txtFromDate.Text == "" && txtToDate.Text == "")
                    {
                        rangeReporthead.SetValue("WORK ORDER REGISTER ABSTRACT as on " + DateTime.Now);
                    }

                    wb.Worksheet(1).Cell(3, 16).Value = DateTime.Now;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    string FileName = "DTC WorkOrder Failure " + DateTime.Now + ".xls";
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

        protected void btnFailreset_Click(object sender, EventArgs e)
        {
            txtFailMonth.Text = string.Empty;
        }

        protected void btnFailGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                clsReports objReport = new clsReports();
                if (txtFailMonth.Text == "")
                {
                    ShowMsgBox("Please Select The Month");
                    txtFailMonth.Focus();
                    return;
                }

                objReport.sMonth = txtFailMonth.Text;

                objReport.sReportType = cmbAbstract_ReportType.SelectedValue;
                //DateTime test = txtFailMonth.Text.ToString("YYYY-MM");

                string sParam = "id=FailureAbstract&Officecode=" + objSession.OfficeCode + "&Month=" + objReport.sMonth + "&ReportType=" + objReport.sReportType;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + sParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void Export_clickFailure(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            clsReports objReport = new clsReports();

            if (txtFailMonth.Text == "")
            {
                ShowMsgBox("Please Select The Month");
                txtFailMonth.Focus();
                return;
            }

            objReport.sMonth = txtFailMonth.Text;
            objReport.sOfficeCode = objSession.OfficeCode;

            dt = objReport.FailureAbstract(objReport);

            if (dt.Rows.Count > 0)
            {
                string[] arrAlpha = Genaral.getalpha();


                using (XLWorkbook wb = new XLWorkbook())
                {
                    dt.Columns["EST_PEND"].ColumnName = "Estimate Pending";
                    dt.Columns["WO_Pending"].ColumnName = "WorkOrder Pending";
                    dt.Columns["Indent_Pending"].ColumnName = "Indent Pending";
                    dt.Columns["Invoice_Pending"].ColumnName = "Invoice Pending";
                    dt.Columns["CR_RI_Pending"].ColumnName = "CR/RI Pending";

                    dt.Columns["DIV_NAME"].SetOrdinal(0);
                    dt.Columns["DIV_NAME"].ColumnName = "Division";
                    dt.Columns["TOTAL_DTC"].SetOrdinal(1);
                    dt.Columns["TOTAL_DTC"].ColumnName = "Total Dtc";
                    dt.Columns["Total_Fail_Pending"].SetOrdinal(2);
                    dt.Columns["Total_Fail_Pending"].ColumnName = "Total TC's Fail ";
                    dt.Columns["PENDING_FOR_REPLACEMENT"].SetOrdinal(3);
                    dt.Columns["PENDING_FOR_REPLACEMENT"].ColumnName = "Pending for replacement";

                    dt.Columns.Remove("CURRENT_MONTH");
                    dt.Columns.Remove("wo_office_code");


                    wb.Worksheets.Add(dt, "Failure Abstract");
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

                    if (txtFailMonth.Text != "" || txtFailMonth.Text != null)
                    {
                        rangeReporthead.SetValue("Comparision Of DTLMS Details With Stastics During " + objReport.sMonth);
                    }

                    wb.Worksheet(1).Cell(3, 9).Value = DateTime.Now;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    string FileName = "Failure Abstract " + DateTime.Now + ".xls";
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
        //protected void cmbCoil_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (cmbCoil.SelectedValue == "1")
        //        {
        //            minorlist11.Enabled = true;
        //            minorlist12.Enabled = true;
        //            minorlist13.Enabled = true;
        //            majorlist4.Enabled = false;
        //            majorlist5.Enabled = false;
        //            majorlist6.Enabled = false;
        //            majorlist7.Enabled = false;
        //            majorlist8.Enabled = false;
        //            majorlist9.Enabled = false;
        //            majorlist10.Enabled = false;
        //        }
        //        else
        //        {
        //            minorlist11.Enabled = false;
        //            minorlist12.Enabled = false;
        //            minorlist13.Enabled = false;
        //            majorlist4.Enabled = true;
        //            majorlist5.Enabled = true;
        //            majorlist6.Enabled = true;
        //            majorlist7.Enabled = true;
        //            majorlist8.Enabled = true;
        //            majorlist9.Enabled = true;
        //            majorlist10.Enabled = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}
        #endregion
        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtFromDate.Text = "";
                txtToDate.Text = "";
                // cmbZone.SelectedIndex = 0;
                // cmbCircle.Items.Clear();
                cmbReportType.SelectedIndex = 0;
                // cmbDiv.Items.Clear();
                // cmbSubDiv.Items.Clear();
                // cmbOMSection.Items.Clear();
                cmbStage.SelectedIndex = 0;
                cmbMake.SelectedIndex = 0;
                cmbFailureType.SelectedIndex = 0;
                cmbCapacity1.SelectedIndex = 0;
                cmbGrntyType.SelectedIndex = 0;
                //cmbCoil.SelectedIndex = 0; // Removed as mentioned in Jira Id : DTLMS-1598
                objSession = (clsSession)Session["clsSession"];
                if (objSession.OfficeCode.Length == 0)
                {
                    cmbZone.SelectedIndex = 0;
                    cmbCircle.Items.Clear();
                    cmbDiv.Items.Clear();
                    cmbSubDiv.Items.Clear();
                    cmbOMSection.Items.Clear();
                }
                else
                {
                    string OfficeCode = objSession.OfficeCode;
                    switch (OfficeCode.Length)
                    {
                        case 1:
                            cmbCircle.SelectedIndex = 0;
                            cmbDiv.Items.Clear();
                            cmbSubDiv.Items.Clear();
                            cmbOMSection.Items.Clear();
                            break;
                        case 2:
                            cmbDiv.SelectedIndex = 0;
                            cmbSubDiv.Items.Clear();
                            cmbOMSection.Items.Clear();
                            break;
                        case 3:
                            cmbSubDiv.SelectedIndex = 0;
                            Items.Clear();
                            cmbOMSection.Items.Clear();
                            break;
                        case 4:
                            cmbOMSection.SelectedIndex = 0;
                            break;
                    }

                }
                cmbFeederName1.Items.Clear();
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

            if (cmbSubDiv.SelectedIndex > 0)
            {
                strOfficeId = cmbSubDiv.SelectedValue.ToString();
            }
            if (cmbOMSection.SelectedIndex > 0)
            {
                strOfficeId = cmbOMSection.SelectedValue.ToString();
            }
            return (strOfficeId);
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
        /// <summary>
        /// Clistel Report Click for
        /// Pending / Completed Failure Details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                clsReports objReport = new clsReports();
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
                if (cmbReportType.SelectedValue == "1")
                {
                    ShowMsgBox("Please Select Report Type");
                    cmbReportType.Focus();
                    return;
                }
                objReport.sType = cmbStage.SelectedValue.ToString();
                

                if (cmbFeederName1.SelectedIndex > 0)
                {
                    objReport.sFeeder = cmbFeederName1.SelectedValue;
                }

                //if (cmbCoil.SelectedIndex > 0) // // Removed as mentioned in Jira Id : DTLMS-1598
                //{
                //    objReport.sFailType = cmbCoil.SelectedValue.ToString();
                //}

                if (cmbOMSection.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbOMSection.SelectedValue;
                }
                else if (cmbSubDiv.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbSubDiv.SelectedValue;
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
                    objReport.sOfficeCode = cmbZone.SelectedValue.ToString();
                }
                else
                {
                    objReport.sOfficeCode = "";
                }

                if (cmbMake.SelectedIndex > 0)
                {
                    objReport.sMake = cmbMake.SelectedValue;
                }

                if (cmbFailureType.SelectedIndex > 0)
                {
                    objReport.sFailureType = cmbFailureType.SelectedValue;
                }
                else
                {
                    objReport.sFailureType = "";
                }

                if (cmbCapacity1.SelectedIndex > 0)
                {
                    objReport.sCapacity = cmbCapacity1.SelectedItem.Text;
                }

                if (cmbGrntyType.SelectedIndex > 0)
                {
                    objReport.sGuranteeType = cmbGrntyType.SelectedValue;
                }
               

                objReport.sFromDate = txtFromDate.Text;
                objReport.sTodate = txtToDate.Text;
                //objReport.sReportType = cmbReportType.SelectedValue; // we need to featch the text not the value.
                objReport.SelectStage = cmbStage.SelectedItem.Text;
                objReport.sReportType = cmbReportType.SelectedItem.Text;
                string sParam = "id=TCFail&Type=" + objReport.sType + "&Officecode=" + objReport.sOfficeCode + "&FromDate=" + objReport.sFromDate + "&ToDate=" + objReport.sTodate + "&FailType=" + objReport.sFailureType + "&Make=" + objReport.sMake + "&ReportType=" + objReport.sReportType + "&Capacity=" + objReport.sCapacity + "&GrntyType=" + objReport.sGuranteeType + "&coilType=" + objReport.sFailType + "&FeederName=" + objReport.sFeeder + "&Stage=" + objReport.SelectStage;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + sParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// Export EXCEL for 
        /// Pending / Completed Failure Details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Export_click(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            clsReports objReport = new clsReports();
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

            objReport.sType = cmbStage.SelectedValue.ToString(); // failure ,estimation,workorder etc..
            objReport.SelectStage = cmbStage.SelectedItem.Text; // 
            objReport.sReportType = cmbReportType.SelectedItem.Text;

            if (cmbFeederName1.SelectedIndex > 0)
            {
                objReport.sFeeder = cmbFeederName1.SelectedValue;
            }
            if (cmbMake.SelectedIndex > 0)
            {
                objReport.sMake = cmbMake.SelectedValue;
            }

            if (cmbCapacity1.SelectedIndex > 0)
            {
                objReport.sCapacity = cmbCapacity1.SelectedItem.Text;
            }

            if (cmbGrntyType.SelectedIndex > 0)
            {
                objReport.sGuranteeType = cmbGrntyType.SelectedValue;
            }

            if (cmbFailureType.SelectedIndex > 0)
            {
                objReport.sFailureType = cmbFailureType.SelectedValue;
            }
            else
            {
                objReport.sFailureType = null;
            }

            if (txtFromDate.Text != null && txtFromDate.Text != "")
            {
                objReport.sFromDate = txtFromDate.Text;

                // objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                DateTime DToDate = DateTime.ParseExact(objReport.sFromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");
            }
            if (txtToDate.Text != null && txtToDate.Text != "")
            {
                objReport.sTodate = txtToDate.Text;

                //objReport.sTodate = Request.QueryString["ToDate"].ToString();
                DateTime DToDate = DateTime.ParseExact(objReport.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
            }


            string strofficecode = GetOfficeID();
            objReport.sOfficeCode = strofficecode;

            //objReport.sFailType = cmbCoil.SelectedValue.ToString();  // Removed as mentioned in Jira Id : DTLMS-1598

            dt = objReport.TCFailReport(objReport);

            if (dt.Rows.Count > 0)
            {
                int arrAlpha = dt.Columns.Count;
                using (XLWorkbook wb = new XLWorkbook())
                {
                    //dt.Columns["WO_NO"].ColumnName = "Wo No";
                    //dt.Columns["TI_INDENT_NO"].ColumnName = "Indent Number";                 
                    //dt.Columns["COILTYPE"].ColumnName = "CoilType";  mentioned in Jira id : DTLMS-1598 to hide this field 
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

                    dt.Columns["DT_CODE"].SetOrdinal(5);
                    dt.Columns["DT_CODE"].ColumnName = "DTC Code";

                    dt.Columns["DT_NAME"].SetOrdinal(6);
                    dt.Columns["DT_NAME"].ColumnName = "DTC Name";

                    dt.Columns["TC_CODE"].SetOrdinal(7);
                    dt.Columns["TC_CODE"].ColumnName = "DTr Code";

                    dt.Columns["TC_CAPACITY"].SetOrdinal(8);
                    dt.Columns["TC_CAPACITY"].ColumnName = "TC Capacity";

                    dt.Columns["TC_MAKE_ID"].SetOrdinal(9);
                    dt.Columns["TC_MAKE_ID"].ColumnName = "Make Name";

                    dt.Columns["FD_FEEDER_CODE"].SetOrdinal(10);
                    dt.Columns["FD_FEEDER_CODE"].ColumnName = "Feeder Code";

                    dt.Columns["FD_FEEDER_NAME"].SetOrdinal(11);
                    dt.Columns["FD_FEEDER_NAME"].ColumnName = "Feeder Name";

                    dt.Columns["REPORT_TYPE"].SetOrdinal(12);
                    dt.Columns["REPORT_TYPE"].ColumnName = "Report Type";

                    dt.Columns["REPORT_CATEGORY"].SetOrdinal(13);
                    dt.Columns["REPORT_CATEGORY"].ColumnName = "Report Category";


                    dt.Columns["DF_DATE"].SetOrdinal(14);
                    dt.Columns["DF_DATE"].ColumnName = "Fail Date";

                    dt.Columns["DF_LOC_CODE"].SetOrdinal(15);
                    dt.Columns["DF_LOC_CODE"].ColumnName = "Failure Loc Code";

                    dt.Columns["MD_NAME"].SetOrdinal(16);
                    dt.Columns["MD_NAME"].ColumnName = "Failure Type";

                    dt.Columns["COMPLAINNUMBER"].SetOrdinal(17);
                    dt.Columns["COMPLAINNUMBER"].ColumnName = "Complain Number";

                    dt.Columns["COMPLAINDATE"].SetOrdinal(18);
                    dt.Columns["COMPLAINDATE"].ColumnName = "Complain Date";

                    dt.Columns["ESTIMATION_NO"].SetOrdinal(19);
                    dt.Columns["ESTIMATION_NO"].ColumnName = "Estimation Number";

                    dt.Columns["WO_NO"].SetOrdinal(20);
                    dt.Columns["WO_NO"].ColumnName = "WorkOrder No";

                    dt.Columns["COMMISSION"].SetOrdinal(21);
                    dt.Columns["COMMISSION"].ColumnName = "Com(Amount)";

                    dt.Columns["DECOMMISSION"].SetOrdinal(22);
                    dt.Columns["DECOMMISSION"].ColumnName = "Decom(Amount)";

                    dt.Columns["IN_INV_NO"].SetOrdinal(23);
                    dt.Columns["IN_INV_NO"].ColumnName = "Invoice Number";

                    dt.Columns["IN_DATE"].SetOrdinal(24);
                    dt.Columns["IN_DATE"].ColumnName = "Invoice Date";

                    dt.Columns["TR_RI_NO"].SetOrdinal(25);
                    dt.Columns["TR_RI_NO"].ColumnName = "RI No";

                    dt.Columns["TR_RI_DATE"].SetOrdinal(26);
                    dt.Columns["TR_RI_DATE"].ColumnName = "RI Date";


                    //dt.Columns.Remove("COILTYPE");
                    dt.Columns.Remove("TI_INDENT_DATE");
                    dt.Columns.Remove("TI_INDENT_NO");
                    dt.Columns.Remove("WO_DATE");
                    dt.Columns.Remove("TR_RV_DATE");

                    dt.Columns.Remove("FROMDATE");
                    dt.Columns.Remove("TODATE");
                    dt.Columns.Remove("TODAY");

                    wb.Worksheets.Add(dt, "DTCFAILURE");
                    wb.Worksheet(1).Row(1).InsertRowsAbove(3);
                    var rngend = wb.Worksheet(1).Cell(1, arrAlpha).Address.ColumnLetter;
                    //  string sMergeRange = arrAlpha[dt.Columns.Count - 1];
                    var rangeheader = wb.Worksheet(1).Range("A1:" + rngend + "1");
                    rangeheader.Merge().Style.Font.SetBold().Font.FontSize = 16;
                    rangeheader.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    rangeheader.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    rangeheader.SetValue("Hubli Electricity Supply Company Limited, (HESCOM)");

                    var rangeReporthead = wb.Worksheet(1).Range("A2:" + rngend + "2");
                    rangeReporthead.Merge().Style.Font.SetBold().Font.FontSize = 12;
                    rangeReporthead.Merge().Style.Fill.BackgroundColor = XLColor.AirForceBlue;
                    rangeReporthead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    if (txtFromDate.Text != "" && txtToDate.Text != "")
                    {
                        rangeReporthead.SetValue("List of DTC with Details  From " + objReport.sFromDate + "  To " + objReport.sTodate);
                    }
                    if (txtFromDate.Text != "" && txtToDate.Text == "")
                    {
                        rangeReporthead.SetValue("List of DTC with Details  From " + objReport.sFromDate + "  To " + DateTime.Now);
                    }
                    if (txtFromDate.Text == "" && txtToDate.Text != "")
                    {
                        rangeReporthead.SetValue("List of DTC with Details  as on " + objReport.sTodate);
                    }
                    if (txtFromDate.Text == "" && txtToDate.Text == "")
                    {
                        rangeReporthead.SetValue("List of DTC with Details as on " + DateTime.Now);
                    }
                    //rangeReporthead.SetValue("List of DTC with Details ");
                    wb.Worksheet(1).Cell(3, 16).Value = DateTime.Now;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    string FileName = "DTCFAILURE " + DateTime.Now + ".xls";
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
        /// Cristel Report for 
        /// FREQUENTLY FAILING DTC'S
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGenerate3_Click(object sender, EventArgs e)
        {
            clsReports objReport = new clsReports();
            try
            {
                string sResult = string.Empty;
                if (txtFromDate3.Text != "")
                {
                    sResult = Genaral.DateValidation(txtFromDate3.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtFromDate3.Focus();
                        return;
                    }
                }
                if (txtToDate3.Text != "")
                {
                    sResult = Genaral.DateValidation(txtToDate3.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtToDate3.Focus();
                        return;
                    }
                }
                if (txtFromDate3.Text != "" && txtToDate3.Text != "")
                {
                    sResult = Genaral.DateComparision(txtToDate3.Text, txtFromDate3.Text, false, false);
                    if (sResult == "2")
                    {
                        ShowMsgBox("To Date should be Greater than From Date");
                        txtToDate3.Focus();
                        return;
                    }
                }
                if (cmbFeederName3.SelectedIndex > 0)
                {
                    objReport.sFeeder = cmbFeederName3.SelectedValue;
                }
                if (cmbguranteetype1.SelectedIndex > 0)
                {
                    objReport.sGuranteeType = cmbguranteetype1.SelectedValue.Trim();
                }
                if (txtDTCCode.Text != "")
                {
                    objReport.sDtcCode = txtDTCCode.Text;
                }
                if (txtDTRCode.Text != "")
                {
                    objReport.sDtrCode = txtDTRCode.Text;
                }

                if (cmbOMSection3.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbOMSection3.SelectedValue;
                }
                else if (cmbSubDivision3.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbSubDivision3.SelectedValue;
                }
                else if (cmbDivision3.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbDivision3.SelectedValue;
                }
                else if (cmbCircle3.SelectedIndex > 0)
                    objReport.sOfficeCode = cmbCircle3.SelectedValue;
                else if (cmbZone2.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbZone2.SelectedValue.ToString();
                }
                else
                {
                    objReport.sOfficeCode = "";
                }
                if (cmbFailureType1.SelectedIndex > 0)
                {
                    objReport.sFailureType = cmbFailureType1.SelectedValue;
                }
                else
                {
                    objReport.sFailureType = "";
                }
                objReport.sFromDate = txtFromDate3.Text;
                objReport.sTodate = txtToDate3.Text;

                //if (cmbCoil2.SelectedIndex > 0)
                //    objReport.sFailType = cmbCoil2.SelectedValue.ToString(); // Removed as mentioned in Jira Id : DTLMS-1598

                string sParam = "id=FrequentTCFail&Officecode=" + objReport.sOfficeCode + "&FromDate=" + objReport.sFromDate + "&ToDate=" + objReport.sTodate + "&FailType=" + objReport.sFailureType + "&GuranteeType=" + objReport.sGuranteeType + "&DTCCode=" + objReport.sDtcCode + "&DTRCode=" + objReport.sDtrCode + "&coilType=" + objReport.sFailType + "&FeederName=" + objReport.sFeeder;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + sParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// Export EXCEL for 
        /// FREQUENTLY FAILING DTC'S
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Export_click3(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            clsReports objReport = new clsReports();
            string sResult = string.Empty;
            if (txtFromDate3.Text != "")
            {
                sResult = Genaral.DateValidation(txtFromDate3.Text);
                if (sResult != "")
                {
                    ShowMsgBox(sResult);
                    txtFromDate3.Focus();
                    return;
                }
            }
            if (txtToDate3.Text != "")
            {
                sResult = Genaral.DateValidation(txtToDate3.Text);
                if (sResult != "")
                {
                    ShowMsgBox(sResult);
                    txtToDate3.Focus();
                    return;
                }
            }
            if (txtFromDate3.Text != "" && txtToDate3.Text != "")
            {
                sResult = Genaral.DateComparision(txtToDate3.Text, txtFromDate3.Text, false, false);
                if (sResult == "2")
                {
                    ShowMsgBox("To Date should be Greater than From Date");
                    txtToDate3.Focus();
                    return;
                }
            }
            if (txtFromDate3.Text != null && txtFromDate3.Text != "")
            {
                objReport.sFromDate = txtFromDate3.Text;
            }
            if (txtToDate3.Text != null && txtToDate3.Text != "")
            {
                objReport.sTodate = txtToDate3.Text;
            }


            if (cmbOMSection3.SelectedIndex > 0)
            {
                objReport.sOfficeCode = cmbOMSection3.SelectedValue;
            }
            else if (cmbSubDivision3.SelectedIndex > 0)
            {
                objReport.sOfficeCode = cmbSubDivision3.SelectedValue;
            }
            else if (cmbDivision3.SelectedIndex > 0)
            {
                objReport.sOfficeCode = cmbDivision3.SelectedValue;
            }
            else if (cmbCircle3.SelectedIndex > 0)
            {
                objReport.sOfficeCode = cmbCircle3.SelectedValue;
            }
            else if (cmbZone2.SelectedIndex > 0)
            {
                objReport.sOfficeCode = cmbZone2.SelectedValue.ToString();
            }
            else
            {
                objReport.sOfficeCode = "";
            }

            if (cmbFailureType1.SelectedIndex > 0)
            {
                objReport.sFailureType = cmbFailureType1.SelectedValue;
            }
            else
            {
                objReport.sFailureType = null;
            }

            if (cmbFeederName3.SelectedIndex > 0)
            {
                objReport.sFeeder = cmbFeederName3.SelectedValue;
            }

            if (cmbguranteetype1.SelectedIndex > 0)
            {
                objReport.sGuranteeType = cmbguranteetype1.SelectedValue.Trim();
            }

            if (txtDTCCode.Text != "")
            {
                objReport.sDtcCode = txtDTCCode.Text;
            }

            if (txtDTRCode.Text != "")
            {
                objReport.sDtrCode = txtDTRCode.Text;
            }

            if (txtFromDate3.Text.ToString() != null && txtFromDate3.Text.ToString() != "")
            {
                objReport.sFromDate = txtFromDate3.Text.ToString();
                DateTime DToDate = DateTime.ParseExact(objReport.sFromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");
            }
            if (txtToDate3.Text.ToString() != null && txtToDate3.Text.ToString() != "")
            {
                objReport.sTodate = txtToDate3.Text.ToString();
                DateTime DToDate = DateTime.ParseExact(objReport.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
            }

            dt = objReport.FrequentTcFail(objReport);

            if (dt.Rows.Count > 0)
            {
                string[] arrAlpha = Genaral.getalpha();
                using (XLWorkbook wb = new XLWorkbook())
                {

                    dt.Columns["TC_CAPACITY"].ColumnName = "TC Capacity";
                    dt.Columns["TC_SLNO"].ColumnName = "TC Serial No";
                    dt.Columns["DT_CODE"].ColumnName = "DTC Code";
                    dt.Columns["DF_DATE"].ColumnName = "Fail Date";
                    dt.Columns["DF_LOC_CODE"].ColumnName = "Failure Loc Code";
                    dt.Columns["MD_NAME"].ColumnName = "Failure Type";
                    dt.Columns["DF_GUARANTY_TYPE"].ColumnName = "Guaranty Type";
                    dt.Columns["FD_FEEDER_CODE"].ColumnName = "Feeder Code";
                    dt.Columns["FD_FEEDER_NAME"].ColumnName = "Feeder Name";
                    dt.Columns["DT_NAME"].ColumnName = "DTC Name";

                    dt.Columns["ComplainNumber"].ColumnName = "Complain Number";
                    dt.Columns["ComplainDate"].ColumnName = "Complain Date";

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

                    dt.Columns["TC_CODE"].SetOrdinal(5);
                    dt.Columns["TC_CODE"].ColumnName = "DTr Code";

                    dt.Columns["TC_MAKE_ID"].SetOrdinal(6);
                    dt.Columns["TC_MAKE_ID"].ColumnName = "Make Name";



                    dt.Columns.Remove("DF_DTC_CODE");
                    dt.Columns.Remove("FROMDATE");
                    dt.Columns.Remove("TODATE");
                    dt.Columns.Remove("TODAY");

                    wb.Worksheets.Add(dt, "FREQUENTDTCFAILURE");

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

                    if (txtFromDate3.Text != "" && txtToDate3.Text != "")
                    {
                        rangeReporthead.SetValue("List of Frequent Failed DTC with Details  From " + objReport.sFromDate + "  To " + objReport.sTodate);
                    }
                    if (txtFromDate3.Text != "" && txtToDate3.Text == "")
                    {
                        rangeReporthead.SetValue("List of Frequent Failed DTC with Details  From " + objReport.sFromDate + "  To " + DateTime.Now);
                    }
                    if (txtFromDate3.Text == "" && txtToDate3.Text != "")
                    {
                        rangeReporthead.SetValue("List of Frequent Failed DTC with Details  as on " + objReport.sTodate);
                    }
                    if (txtFromDate3.Text == "" && txtToDate3.Text == "")
                    {
                        rangeReporthead.SetValue("List of Frequent Failed DTC with Details as on " + DateTime.Now);
                    }
                    //rangeReporthead.SetValue("List of DTC with Details ");
                    wb.Worksheet(1).Cell(3, 9).Value = DateTime.Now;
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    string FileName = "FREQUENTDTCFAILURE " + DateTime.Now + ".xls";
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
        protected void Back_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Dashboard.aspx");
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void BtnReset3_Click(object sender, EventArgs e)
        {
            try
            {
                txtDTCCode.Text = "";
                txtDTRCode.Text = "";
                txtFromDate3.Text = "";
                txtToDate3.Text = "";
                cmbguranteetype1.SelectedIndex = 0;
                cmbReportType.SelectedIndex = 0;
                cmbStage.SelectedIndex = 0;
                cmbFailureType1.SelectedIndex = 0;
                //cmbCoil2.SelectedIndex = 0; // Removed as mentioned in Jira Id : DTLMS-1598
                objSession = (clsSession)Session["clsSession"];
                if (objSession.OfficeCode.Length == 0)
                {
                    cmbZone2.SelectedIndex = 0;
                    cmbCircle3.Items.Clear();
                    cmbDivision3.Items.Clear();
                    cmbSubDivision3.Items.Clear();
                    cmbOMSection3.Items.Clear();
                }
                else
                {
                    string OfficeCode = objSession.OfficeCode;
                    switch (OfficeCode.Length)
                    {
                        case 1:
                            cmbCircle3.SelectedIndex = 0;
                            cmbDivision3.Items.Clear();
                            cmbSubDivision3.Items.Clear();
                            cmbOMSection3.Items.Clear();
                            break;
                        case 2:
                            cmbDivision3.SelectedIndex = 0;
                            cmbSubDivision3.Items.Clear();
                            cmbOMSection3.Items.Clear();
                            break;
                        case 3:
                            cmbSubDivision3.SelectedIndex = 0;
                            cmbOMSection3.Items.Clear();
                            break;
                        case 4:
                            cmbOMSection3.SelectedIndex = 0;
                            break;
                    }
                }
                cmbFeederName3.Items.Clear();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}