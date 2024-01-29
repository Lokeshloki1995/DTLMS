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
    public partial class DTrRepairCenterDetails : System.Web.UI.Page
    {
        clsSession objSession;
        string strFormCode = "DTrRepairCenterReport";
         int Zone_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Zone_code"]);
        int Circle_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Circle_code"]);
        int Division_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Division_code"]);

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

                    string strQry = string.Empty;

                    objSession = (clsSession)Session["clsSession"];
                    string stroffCode = string.Empty;
                    //if (objSession.OfficeCode.Length <= 2 && objSession.OfficeCode.Length != 0)
                    //{
                    //    stroffCode = clsStoreOffice.GetZone_Circle_Div_Offcode(objSession.OfficeCode, objSession.RoleId).Substring(0, Constants.Circle);
                    //}
                    //else
                    //{
                    //    stroffCode = objSession.OfficeCode;
                    //}
                
                if (stroffCode == null || stroffCode == "")
                    {
                        Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone);
                         
                    }

                }
            }
            catch (Exception ex)
            {

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


         protected void cmdReset_Click(object sender, EventArgs e)
         {
             try
             {

                 cmbZone.SelectedIndex = 0;
                 cmbCircle.SelectedIndex = 0;

                 cmbDiv.SelectedIndex = 0;
                

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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void cmdReport_Click(object sender, EventArgs e)
        {
            try
            {
                string strOfficeCode = string.Empty;
                string strParam = string.Empty;
                string strValue = string.Empty;
                string stroffcode = string.Empty;


                clsReports objReport = new clsReports();
                 

                if (cmbZone.SelectedIndex > -1)
                {
                    objReport.sOfficeCode = cmbZone.SelectedValue;

                }
                
                 if (cmbCircle.SelectedIndex != 0)
                {
                    objReport.sOfficeCode = cmbCircle.SelectedValue;
                }
                 if (cmbDiv.SelectedIndex > 0)
                {
                    objReport.sOfficeCode = cmbDiv.SelectedValue;
                }
                else
                {
                    stroffcode = GetOfficeID();
                    objReport.sOfficeCode = stroffcode;
                }
                
                string param="id=RepairCenterDetails&OfficeCode=" + objReport.sOfficeCode;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + param + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");



            }
            catch (Exception ex)
            {

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
            if(cmbDiv.SelectedIndex > 0  )
            {
                strOfficeId = cmbDiv.SelectedValue.ToString();
            }
            else 
            {
                strOfficeId = clsStoreOffice.GetStoreID(cmbDiv.SelectedValue.ToString());
            }


            return (strOfficeId);
        }


        }
    }
 