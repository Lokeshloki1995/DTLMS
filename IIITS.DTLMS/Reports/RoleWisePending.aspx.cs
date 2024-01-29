using IIITS.DTLMS.BL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.Reports
{
    public partial class RoleWisePending : System.Web.UI.Page
    {
        string strFormCode = "RoleWisePending";
        clsSession objSession;
        int Zone_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Zone_code"]);
        int Circle_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Circle_code"]);
        int Division_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Division_code"]);
        int SubDiv_code = Convert.ToInt32(ConfigurationSettings.AppSettings["SubDiv_code"]);
        int Section_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Section_code"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }

                string stroffCode = string.Empty;
                objSession = (clsSession)Session["clsSession"];
                if (objSession.OfficeCode.Length <= 2 && objSession.OfficeCode.Length != 0)
                {

                    stroffCode = clsStoreOffice.GetZone_Circle_Div_Offcode(objSession.OfficeCode, objSession.RoleId).Substring(0, Circle_code);
                }
                else
                {
                    stroffCode = objSession.OfficeCode;
                }
                string stroffCode1 = stroffCode;
                if (!IsPostBack)
                {
                    if (stroffCode == null || stroffCode == "")
                    {
                        Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone);

                    }

                    if (stroffCode.Length >= 1)
                    {
                        Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone);

                        stroffCode = stroffCode1.Substring(0, Zone_code);
                        cmbZone.Items.FindByValue(stroffCode).Selected = true;
                        stroffCode = stroffCode1;
                        cmbZone.Enabled = false;
                    }
                    if (stroffCode.Length >= 1)
                    {
                        Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_ZO_ID\"='" + cmbZone.SelectedValue + "'", "--Select--", cmbCircle);

                        if (stroffCode.Length >= 2)
                        {
                            stroffCode = stroffCode1.Substring(0, Circle_code);
                            cmbCircle.Items.FindByValue(stroffCode).Selected = true;
                            stroffCode = stroffCode1;
                            cmbCircle.Enabled = false;
                        }
                    }

                    if (stroffCode.Length >= 2)
                    {
                        if(objSession.sRoleType=="2")
                        {
                            string div = clsStoreOffice.GetOfficeCode(objSession.OfficeCode, "DIV_CODE");
                            Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE  "+ div + "", "--Select--", cmbDiv);
                        }
                        else
                        {
                           
                            Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE \"DIV_CICLE_CODE\"='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDiv);
                        }
                       

                        if (stroffCode.Length >= 3)
                        {
                            stroffCode = stroffCode1.Substring(0, Division_code);
                            cmbDiv.Items.FindByValue(stroffCode).Selected = true;
                            stroffCode = stroffCode1;
                            cmbDiv.Enabled = false;
                        }
                    }

                    //if (stroffCode.Length >= 3)
                    //{
                    //    Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE \"SD_DIV_CODE\"='" + cmbDiv.SelectedValue + "'", "--Select--", cmbSubDivision);

                    //    if (stroffCode.Length >= 4)
                    //    {
                    //        stroffCode = stroffCode1.Substring(0, SubDiv_code);
                    //       // cmbSubDivision.Items.FindByValue(stroffCode).Selected = true;
                    //        stroffCode = stroffCode1;
                    //       // cmbSubDivision.Enabled = false;
                    //    }
                    //}
                    //if (stroffCode.Length >= 4)
                    //{
                    //    Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_SUBDIV_CODE\"='" + cmbSubDivision.SelectedValue + "'", "--Select--", cmbOMSection);


                    //    if (stroffCode.Length >= 5)
                    //    {
                    //        stroffCode = stroffCode1.Substring(0, Section_code);
                    //       // cmbOMSection.Items.FindByValue(stroffCode).Selected = true;
                    //        stroffCode = stroffCode1;
                    //       // cmbOMSection.Enabled = false;
                    //    }

                    //}
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
                    //cmbSubDivision.Items.Clear();
                    //cmbOMSection.Items.Clear();
                }
                else
                {
                    cmbCircle.Items.Clear();
                    cmbDiv.Items.Clear();
                    //cmbSubDivision.Items.Clear();
                    //cmbOMSection.Items.Clear();

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
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

                    //cmbSubDivision.Items.Clear();
                    //cmbOMSection.Items.Clear();

                }
                else
                {
                    cmbDiv.Items.Clear();
                    //cmbSubDivision.Items.Clear();
                    //cmbOMSection.Items.Clear();

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        //protected void cmbDiv_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (cmbDiv.SelectedIndex > 0)
        //        {
        //            Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE CAST(\"SD_DIV_CODE\" AS TEXT)='" + cmbDiv.SelectedValue + "'", "--Select--", cmbSubDivision);
        //            cmbOMSection.Items.Clear();

        //        }
        //        else
        //        {
        //            cmbSubDivision.Items.Clear();
        //            cmbOMSection.Items.Clear();

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = ex.Message;
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}

        //protected void cmbSubDivision_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (cmbSubDivision.SelectedIndex > 0)
        //        {
        //            Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE CAST(\"OM_SUBDIV_CODE\" AS TEXT)='" + cmbSubDivision.SelectedValue + "'", "--Select--", cmbOMSection);
        //        }
        //        else
        //        {
        //            cmbOMSection.Items.Clear();

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = ex.Message;
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                clsReports objReport = new clsReports();

                string sResult = string.Empty;


                //if (cmbOMSection.SelectedIndex > 0)
                //{
                //    objReport.sOfficeCode = cmbOMSection.SelectedValue;
                //}

                //else if (cmbSubDivision.SelectedIndex > 0)
                //{
                //    objReport.sOfficeCode = cmbSubDivision.SelectedValue;
                //}
                if (cmbDiv.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbDiv.SelectedValue;
                }
                else if (cmbCircle.SelectedIndex > 0)
                    objReport.sOfficeCode = cmbCircle.SelectedValue;
                else if (cmbZone.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbZone.SelectedValue.ToString();
                }
                else objReport.sOfficeCode = "";


                string sParam = "id=RolewiseCount&Officecode=" + objReport.sOfficeCode;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + sParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmdReset_Click(object sender, EventArgs e)
        {
            string stroffCode = string.Empty;
            objSession = (clsSession)Session["clsSession"];
            if (objSession.OfficeCode.Length <= 2 && objSession.OfficeCode.Length != 0)
            {
                stroffCode = clsStoreOffice.GetZone_Circle_Div_Offcode(objSession.OfficeCode, objSession.RoleId).Substring(0, Circle_code);
            }
            else
            {
                stroffCode = objSession.OfficeCode;
            }
            if (stroffCode.Length >= 1)
            {
                cmbZone.Items.Clear();
            }
            if (stroffCode.Length >= 2)
            {
                cmbCircle.Items.Clear();
            }
            if (stroffCode.Length >= 3)
            {
                cmbDiv.Items.Clear();
            }
            //if (stroffCode.Length >= 4)
            //{
            //    cmbSubDivision.Items.Clear();
            //}
            //if (stroffCode.Length >= 5)
            //{
            //    cmbOMSection.Items.Clear();
            //}

        }

    }
}