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
    public partial class DetailedReport : System.Web.UI.Page
    {
        string strFormCode = "DetailedReport";
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
                    objSession = (clsSession)Session["clsSession"];                    
                }
                    
            }
            catch(Exception ex)
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
                    cmbSubDivision.Items.Clear();
                    cmbOMSection.Items.Clear();
                    cmbFeederName.Items.Clear();
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
                    if(cmbLoctype.SelectedIndex == 1)
                    {
                        Genaral.Load_Combo("SELECT  distinct \"SM_ID\", \"SM_NAME\" FROM \"TBLSTOREMAST\", \"TBLSTOREOFFCODE\" WHERE \"SM_ID\" = \"STO_SM_ID\" AND CAST(\"STO_OFF_CODE\" AS TEXT) LIKE '" + cmbCircle.SelectedValue + "%' ORDER BY \"SM_NAME\" ", "--Select--", cmbDiv);
                    }
                    else
                    {
                        Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE \"DIV_CICLE_CODE\"='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDiv);
                    }                   
                    cmbSubDivision.Items.Clear();
                    cmbOMSection.Items.Clear();
                    cmbFeederName.Items.Clear();
                }
                else
                {
                    cmbDiv.Items.Clear();
                    cmbSubDivision.Items.Clear();
                    cmbOMSection.Items.Clear();
                    cmbFeederName.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDiv.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE CAST(\"SD_DIV_CODE\" AS TEXT)='" + cmbDiv.SelectedValue + "'", "--Select--", cmbSubDivision);
                    cmbOMSection.Items.Clear();
                    cmbFeederName.Items.Clear();
                }
                else
                {
                    cmbSubDivision.Items.Clear();
                    cmbOMSection.Items.Clear();
                    cmbFeederName.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbSubDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubDivision.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE CAST(\"OM_SUBDIV_CODE\" AS TEXT)='" + cmbSubDivision.SelectedValue + "'", "--Select--", cmbOMSection);
                }
                else
                {
                    cmbOMSection.Items.Clear();
                    cmbFeederName.Items.Clear();
                 
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

          protected void cmbsec_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbOMSection.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT DISTINCT \"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\",\"TBLFEEDEROFFCODE\" where \"FDO_FEEDER_ID\"=\"FD_FEEDER_ID\" and CAST(\"FDO_OFFICE_CODE\"AS TEXT) like '" + cmbSubDivision.SelectedValue + "%'  ORDER BY \"FD_FEEDER_CODE\" ", "--Select--", cmbFeederName);
                }
                else
                {
                    cmbFeederName.Items.Clear();
                 
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }



        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                cmbFeederName.Items.Clear();
                cmbZone.SelectedIndex = 0;
                cmbCircle.Items.Clear();
                cmbDiv.Items.Clear();
                cmbSubDivision.Items.Clear();
                cmbOMSection.Items.Clear();
                cmbFeederName.Items.Clear();
                cmbLoctype.SelectedIndex = 0;
                cmbType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdReport_Click(object sender, EventArgs e)
        {
            try
            {
                string sOfficeCode = string.Empty;
                string sFeedercode = string.Empty;
                DataTable dt = new DataTable();

                if (cmbType.SelectedIndex == 0)
                {
                    ShowMsgBox("Select Report Type");
                    return;
                }

                if (cmbLoctype.SelectedIndex == 0)
                {
                    ShowMsgBox("Select Location Type");
                    return;
                }
                if (cmbLoctype.SelectedIndex == 1)
                {
                    if (cmbZone.SelectedIndex > 0 && (!(cmbCircle.SelectedIndex > 0) || !(cmbDiv.SelectedIndex > 0)))
                    {
                        ShowMsgBox("Please Select Upto Division");
                        return;
                    }
                }

                    clsReports objReport = new clsReports();

                if (cmbLoctype.SelectedValue == "1")
                {
                    //if (cmbZone.SelectedIndex == 0)
                    //{
                    //    ShowMsgBox("Select the Zone");
                    //    return;
                    //}

                    //    if (cmbCircle.SelectedIndex == 0)
                    //    {
                    //        ShowMsgBox("Select the Circle");
                    //        return;
                    //    }

                    if (cmbDiv.SelectedIndex > 0)
                    {
                        sOfficeCode = cmbDiv.SelectedValue;
                    }

                    dt = objReport.GetDetiailedReport(sOfficeCode, cmbLoctype.SelectedValue, cmbType.SelectedValue);
                }
                else
                {
                    //if (cmbZone.SelectedIndex == 0)
                    //{
                    //    ShowMsgBox("Select the Zone");
                    //    return;
                    //}

                    //if (cmbCircle.SelectedIndex == 0)
                    //{
                    //    ShowMsgBox("Select the Circle");
                    //    return;
                    //}

                    if (cmbFeederName.SelectedIndex > 0)
                    {
                        sFeedercode = cmbFeederName.SelectedValue;
                    }

                    if (cmbOMSection.SelectedIndex > 0)
                    {
                        sOfficeCode = cmbOMSection.SelectedValue;
                    }
                    else if (cmbSubDivision.SelectedIndex > 0)
                    {
                        sOfficeCode = cmbSubDivision.SelectedValue;
                    }
                    else if (cmbDiv.SelectedIndex > 0)
                    {
                        sOfficeCode = cmbDiv.SelectedValue;
                    }
                    else if (cmbCircle.SelectedIndex > 0)
                    {
                        sOfficeCode = cmbCircle.SelectedValue;
                    }
                    else if (cmbZone.SelectedIndex > 0)
                    {
                        sOfficeCode = cmbZone.SelectedValue;
                    }

                    dt = objReport.GetDetiailedReport(sOfficeCode, cmbLoctype.SelectedValue, cmbType.SelectedValue, sFeedercode);
                }

                if (dt.Rows.Count > 0)
                {
                    if (cmbType.SelectedValue == "1" && cmbLoctype.SelectedValue == "2")
                    {
                        dt.Columns["DISTRICT"].SetOrdinal(0);
                        dt.Columns["ZONE_NAME"].SetOrdinal(1);
                        dt.Columns["CIRCLE_NAME"].SetOrdinal(2);
                        dt.Columns["DIV_NAME"].SetOrdinal(3);
                        dt.Columns["SD_NAME"].SetOrdinal(4);
                        dt.Columns["OM_NAME"].SetOrdinal(5);
                        dt.Columns["DT_FDRSLNO"].SetOrdinal(6);
                        dt.Columns["DT_CODE"].SetOrdinal(7);
                        dt.Columns["DT_NAME"].SetOrdinal(8);
                        //  dt.Columns["TC_RATING"].SetOrdinal(6);
                        dt.Columns["DT_INTERNAL_CODE"].SetOrdinal(9);
                        dt.Columns["DT_TIMS_CODE"].SetOrdinal(10);
                        dt.Columns["DT_TRANS_COMMISION_DATE"].SetOrdinal(11);
                        dt.Columns["DT_TOTAL_CON_KW"].SetOrdinal(12);
                        dt.Columns["DT_TOTAL_CON_HP"].SetOrdinal(13);
                        dt.Columns["DT_BREAKER_TYPE"].SetOrdinal(14);
                        dt.Columns["DT_ARRESTERS"].SetOrdinal(15);
                        dt.Columns["DT_DTCMETERS"].SetOrdinal(16);
                        dt.Columns["DT_LT_PROTECT"].SetOrdinal(17);
                        dt.Columns["DT_HT_PROTECT"].SetOrdinal(18);
                        dt.Columns["DT_GROUNDING"].SetOrdinal(19);
                        dt.Columns["DT_HT_LINE"].SetOrdinal(20);
                        dt.Columns["DT_LT_LINE"].SetOrdinal(21);
                        dt.Columns["DT_PLATFORM"].SetOrdinal(22);
                        dt.Columns["DT_LATITUDE"].SetOrdinal(23);
                        dt.Columns["DT_LONGITUDE"].SetOrdinal(24);
                        dt.Columns["DT_GOS"].SetOrdinal(25);

                        dt.Columns["DISTRICT"].ColumnName = "DISTRICT";
                        dt.Columns["ZONE_NAME"].ColumnName = "ZONE";
                        dt.Columns["CIRCLE_NAME"].ColumnName = "CIRCLE";
                        dt.Columns["DIV_NAME"].ColumnName = "DIVISION";
                        dt.Columns["SD_NAME"].ColumnName = "SUBDIVISION";
                        dt.Columns["OM_NAME"].ColumnName = "OM SECTION";
                        dt.Columns["DT_FDRSLNO"].ColumnName = "FEEDER";
                        dt.Columns["DT_CODE"].ColumnName = "TRANSFROMER CENTRE CODE";
                        dt.Columns["DT_NAME"].ColumnName = "TRANSFROMER CENTRE NAME";
                        // dt.Columns["TC_RATING"].ColumnName = "TRANSFROMER RATE";
                        dt.Columns["DT_INTERNAL_CODE"].ColumnName = "INFOSYS CODE";
                        dt.Columns["DT_TIMS_CODE"].ColumnName = "TIMS CODE";
                        dt.Columns["DT_TRANS_COMMISION_DATE"].ColumnName = "COMMISSION DATE";
                        dt.Columns["DT_TOTAL_CON_KW"].ColumnName = "CONN KW";
                        dt.Columns["DT_TOTAL_CON_HP"].ColumnName = "CONN HP";
                        dt.Columns["DT_BREAKER_TYPE"].ColumnName = "BREAKER TYPE";
                        dt.Columns["DT_ARRESTERS"].ColumnName = "ARRESTERS";
                        dt.Columns["DT_DTCMETERS"].ColumnName = "DTCMETERS";
                        dt.Columns["DT_LT_PROTECT"].ColumnName = "LT PROTECT";
                        dt.Columns["DT_HT_PROTECT"].ColumnName = "HT PROTECT";
                        dt.Columns["DT_GROUNDING"].ColumnName = "GROUNDING";
                        dt.Columns["DT_HT_LINE"].ColumnName = "HT LINE LENGTH";
                        dt.Columns["DT_LT_LINE"].ColumnName = "LT LINE LENGTH";
                        dt.Columns["DT_PLATFORM"].ColumnName = "PLATFORM TYPE";
                        dt.Columns["DT_LATITUDE"].ColumnName = "LATITUDE";
                        dt.Columns["DT_LONGITUDE"].ColumnName = "LONGITUDE";
                        dt.Columns["DT_GOS"].ColumnName = "GOS";


                        List<string> listtoRemove = new List<string> { "" };
                        string filename = "TransformerCenter.xls";
                        string pagetitle = "TransformerCenter Details";

                        Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                    }
                    else if (cmbType.SelectedValue == "2" && cmbLoctype.SelectedValue == "2")
                    {
                        dt.Columns["ZONE_NAME"].SetOrdinal(0);
                        dt.Columns["CIRCLE_NAME"].SetOrdinal(1);
                        dt.Columns["DIV_NAME"].SetOrdinal(2);
                        dt.Columns["SD_NAME"].SetOrdinal(3);
                        dt.Columns["OM_NAME"].SetOrdinal(4);
                        dt.Columns["TC_CODE"].SetOrdinal(5);
                        dt.Columns["TC_SLNO"].SetOrdinal(6);
                        dt.Columns["TM_NAME"].SetOrdinal(7);
                        dt.Columns["TC_RATING"].SetOrdinal(8);
                        dt.Columns["TC_MANF_DATE"].SetOrdinal(9);
                        dt.Columns["TC_CAPACITY"].SetOrdinal(10);
                        dt.Columns["TC_WEIGHT"].SetOrdinal(11);

                        dt.Columns["ZONE_NAME"].ColumnName = "ZONE";
                        dt.Columns["CIRCLE_NAME"].ColumnName = "CIRCLE";
                        dt.Columns["DIV_NAME"].ColumnName = "DIVISION";
                        dt.Columns["SD_NAME"].ColumnName = "SUBDIVSION";
                        dt.Columns["OM_NAME"].ColumnName = "SECTION";
                        dt.Columns["TC_CODE"].ColumnName = "DTR CODE";
                        dt.Columns["TC_SLNO"].ColumnName = "DTR SLNO";
                        dt.Columns["TC_CAPACITY"].ColumnName = "DTR CAPACITY";
                        dt.Columns["TM_NAME"].ColumnName = "DTR MAKE";
                        dt.Columns["TC_RATING"].ColumnName = "DTR RATE";
                        dt.Columns["TC_MANF_DATE"].ColumnName = "MANF DATE";
                        dt.Columns["TC_WEIGHT"].ColumnName = "DTR WEIGHT";

                        List<string> listtoRemove = new List<string> { "" };
                        string filename = "FEILDDTRList.xls";
                        string pagetitle = "Transformer Details";

                        Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                    }
                    else
                    {
                        dt.Columns["SM_NAME"].SetOrdinal(0);
                        dt.Columns["TC_CODE"].SetOrdinal(1);
                        dt.Columns["TC_SLNO"].SetOrdinal(2);
                        dt.Columns["TM_NAME"].SetOrdinal(3);
                        dt.Columns["TC_RATING"].SetOrdinal(4);
                        dt.Columns["TC_MANF_DATE"].SetOrdinal(5);
                        dt.Columns["TC_CAPACITY"].SetOrdinal(6);
                        dt.Columns["TC_WEIGHT"].SetOrdinal(7);

                        dt.Columns["SM_NAME"].ColumnName = "STORE";
                        dt.Columns["TC_CODE"].ColumnName = "DTR CODE";
                        dt.Columns["TC_SLNO"].ColumnName = "DTR SLNO";
                        dt.Columns["TC_CAPACITY"].ColumnName = "DTR CAPACITY";
                        dt.Columns["TM_NAME"].ColumnName = "DTR MAKE";
                        dt.Columns["TC_RATING"].ColumnName = "DTR RATE";
                        dt.Columns["TC_MANF_DATE"].ColumnName = "MANF DATE";
                        dt.Columns["TC_WEIGHT"].ColumnName = "DTR WEIGHT";

                        List<string> listtoRemove = new List<string> { "" };
                        string filename = "STOREDTRList.xls";
                        string pagetitle = "Transformer Details";

                        Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                    }
                }
                else
                {
                    ShowMsgBox("No record found");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
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

        protected void cmbLoctype_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(cmbLoctype.SelectedIndex > 0)
                {
                    cmbZone.Items.Clear();
                    cmbCircle.Items.Clear();
                    cmbDiv.Items.Clear();
                    cmbSubDivision.Items.Clear();
                    cmbOMSection.Items.Clear();
                    cmbFeederName.Items.Clear(); 

                    Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone);
                    Genaral.Load_Combo("SELECT \"MD_NAME\",\"MD_NAME\" \"MD_NAME1\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='C' ORDER BY \"MD_ORDER_BY\"", "--Select--", cmbCapacity);

                    if (cmbLoctype.SelectedIndex == 1)
                    {
                        cmbSubDivision.Enabled = false;
                        cmbOMSection.Enabled = false;
                        cmbFeederName.Enabled = false;
                        cmbType.SelectedValue = "2";
                        cmbType.Enabled = false;
                    }
                    else
                    {
                        cmbSubDivision.Enabled = true;
                        cmbOMSection.Enabled = true;
                        cmbFeederName.Enabled = true;
                        cmbType.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbType.SelectedIndex > 0)
                {
                    if (cmbLoctype.SelectedIndex == 2)
                    {
                        cmbCapacity.Enabled = true;
                    }
                    else
                    {
                        cmbCapacity.Enabled = false;
                    }
                }
            }
            catch(Exception ex)
            {
                lblMessage.Text = ex.Message;
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}