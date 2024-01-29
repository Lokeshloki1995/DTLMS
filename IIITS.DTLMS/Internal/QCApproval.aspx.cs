using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using IIITS.DTLMS.BL;
using System.Data;
using System.Configuration;



namespace IIITS.DTLMS.Internal
{
    public partial class QCApproval : System.Web.UI.Page
    {
        string strFormCode = "QCApproval";
        clsSession objSession;
        int Circle_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Circle_code"]);
        int Division_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Division_code"]);
        int SubDiv_code = Convert.ToInt32(ConfigurationSettings.AppSettings["SubDiv_code"]);
        int Section_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Section_code"]);
        int Feeder_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Feeder_code"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] Delete_Session_array = new string[7];
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                
                lblMessage.Text = string.Empty;
                objSession = (clsSession)Session["clsSession"];
              //  CheckAccessRights(objSession.UserType);
                if (Session["arrSave_ImageSession_String"] != null)
                {
                    Delete_Session_array = Session["arrSave_ImageSession_String"] as string[];
                    for (int i = 0; i < 7; i++)
                    {
                        if (Delete_Session_array[i] != "")
                        {
                            Session.Remove(Delete_Session_array[i]);
                        }
                    }
                    Session.Remove("arrSave_ImageSession_String");
                }

                if (!IsPostBack)
                {

                    Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_CODE\" || '-' || \"DIV_NAME\" FROM \"TBLDIVISION\" ORDER BY \"DIV_CODE\"", "--Select--", cmbDivision);

                    Genaral.Load_Combo("SELECT \"SM_ID\", \"SM_NAME\" FROM \"TBLSTOREMAST\" ORDER BY \"SM_NAME\"", "--Select--", cmbStore);
                    Genaral.Load_Combo("SELECT \"TR_ID\", \"TR_NAME\" FROM \"TBLTRANSREPAIRER\" ORDER BY \"TR_NAME\"", "--Select--", cmbRepairer);
                    rdbLocationType.SelectedValue = "4";
                    rdbPendingQC.SelectedValue = "0";
                    string stroffCode = string.Empty;
                    if (objSession.OfficeCode.Length <= 2 && objSession.OfficeCode.Length != 0)
                    {
                        stroffCode = clsStoreOffice.GetZone_Circle_Div_Offcode(objSession.OfficeCode, objSession.RoleId).Substring(0, Constants.Division);
                    }
                    else
                    {
                        stroffCode = objSession.OfficeCode;
                    }

                  
                    string stroffCode1 = stroffCode;

                    if (stroffCode.Length > 0)
                    {
                        Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\"  WHERE \"DIV_CODE\"='" + stroffCode.Substring(0, Division_code) + "' ORDER BY \"DIV_CODE\"", "--Select--", cmbDivision);
                    }

                    string strQry = string.Empty;

                    if (stroffCode.Length >= 2)
                    {
                        if (stroffCode.Length >= 3)
                        {
                            stroffCode = stroffCode.Substring(0, Division_code);
                            cmbDivision.Items.FindByValue(stroffCode).Selected = true;
                            cmbDivision.Enabled = false;



                            stroffCode = stroffCode1;
                        }
                    }
                    if (objSession.sRoleType == "2")
                    {
                        Genaral.Load_Combo("SELECT \"SM_ID\" || '~' || \"STO_OFF_CODE\",\"DIV_NAME\" FROM \"TBLSTOREMAST\", \"TBLSTOREOFFCODE\",  \"TBLDIVISION\" WHERE \"SM_ID\" = \"STO_SM_ID\" And \"SM_ID\" = '" + objSession.OfficeCode + "'  AND \"STO_OFF_CODE\" = \"DIV_CODE\" ORDER BY \"DIV_NAME\"", "--Select--", cmbStore);
                        cmbSubdivision.Enabled = false;
                        cmbSection.Enabled = false;
                        cmbFeeder.Enabled = false;
                        cmbRepairer.Enabled = false;
                    }

                    if (stroffCode.Length >= 3)
                    {


                        //Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\", \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\"  WHERE \"SD_SUBDIV_CODE\"='" + stroffCode.Substring(0, SubDiv_code) + "' AND \"SD_DIV_CODE\"='" + cmbDivision.SelectedValue + "' ORDER BY \"SD_SUBDIV_CODE\"", "--Select--", cmbSubdivision);

                        Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\", \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\"  WHERE  \"SD_DIV_CODE\"='" + cmbDivision.SelectedValue + "' ORDER BY \"SD_SUBDIV_CODE\"", "--Select--", cmbSubdivision);

                        if (stroffCode.Length >= 4)
                        {
                            //Genaral.Load_Combo("SELECT \"OM_CODE\", \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE  \"OM_SUBDIV_CODE\" = '" + cmbSubdivision.SelectedValue + "' ORDER BY \"OM_CODE\"", "--Select--", cmbSection);

                            stroffCode = stroffCode.Substring(0, SubDiv_code);
                            cmbSubdivision.Items.FindByValue(stroffCode).Selected = true;
                            cmbSubdivision.Enabled = false;



                            stroffCode = stroffCode1;
                        }
                        if (stroffCode.Length >= 4)
                        {
                            Genaral.Load_Combo("SELECT \"OM_CODE\", \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE  \"OM_SUBDIV_CODE\" = '" + cmbSubdivision.SelectedValue + "' ORDER BY \"OM_CODE\"", "--Select--", cmbSection);
                            if (stroffCode.Length >= 4)
                            {
                                string stroffCode2 = string.Empty;
                                stroffCode2 = stroffCode.Substring(0, SubDiv_code);
                                //cmbSection.Items.FindByValue(stroffCode).Selected = true;
                                //cmbSection.Enabled = false;
                                //stroffCode = stroffCode1;
                                //cmbSection_SelectedIndexChanged(sender, e);

                            }

                        }
                        if (stroffCode.Length >= 5)
                        {

                            Genaral.Load_Combo("SELECT \"OM_CODE\", \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_CODE\"='" + stroffCode.Substring(0, Section_code) + "' AND \"OM_SUBDIV_CODE\" = '" + cmbSubdivision.SelectedValue + "' ORDER BY \"OM_CODE\"", "--Select--", cmbSection);

                            if (stroffCode.Length >= 5)
                            {
                                string stroffCode2 = string.Empty;
                                stroffCode = stroffCode.Substring(0, Section_code);
                                cmbSection.Items.FindByValue(stroffCode).Selected = true;
                                cmbSection.Enabled = false;
                                stroffCode = stroffCode1;
                                //stroffCode2 = hdfFeeder.Value;
                                cmbSection_SelectedIndexChanged(sender, e);

                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
             //   clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Page_Load");
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "QC EnumApprove";
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sAccessType = "1" + "," + sAccessType;
                bool bResult = objApproval.CheckAccessRights(objApproval);
                if (bResult == false)
                {
                    if (sAccessType == "4")
                    {
                        Response.Redirect("~/UserRestrict.aspx", false);
                    }
                    else
                    {
                        ShowMsgBox("Sorry , You are not authorized to Access");
                    }
                }
                return bResult;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;

            }
        }

        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm() == true)
                {
                    LoadEnumeration();
                    //cmdApprove.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
               // clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdLoad_Click");
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
       
        public void LoadEnumeration(string sDTCCode = "", string sDTCName = "", string sTCCode = "", string sSlNo = "")
        {
            try
            {
                clsQCApproval objQC = new clsQCApproval();

                if (rdbPendingQC.SelectedValue == "1")
                {
                    grdEnumerationDetails.Columns[8].Visible = false;// Supervisor Column
                    //grdEnumerationDetails.Columns[1].Visible = false;// Check box column
                    grdEnumerationDetails.Columns[7].Visible = true; // hide approve button
                }
                else
                {
                   // grdEnumerationDetails.Columns[7].Visible = true;// Supervisor Column
                    grdEnumerationDetails.Columns[1].Visible = true;// Check box column
                    grdEnumerationDetails.Columns[7].Visible = true;// visible approve button
                    grdEnumerationDetails.Columns[8].Visible = true;
                }

                if (cmbDivision.SelectedIndex > 0)
                {
                    objQC.sOffcode = cmbDivision.SelectedValue;
                }

                if (cmbSubdivision.SelectedIndex > 0)
                {
                    objQC.sOffcode = cmbSubdivision.SelectedValue;
                }

                if (cmbSection.SelectedIndex > 0)
                {
                    objQC.sOffcode = cmbSection.SelectedValue;
                }

                if (cmbFeeder.SelectedIndex > 0)
                {
                    objQC.sFeeder = cmbFeeder.SelectedValue;
                }

                if (cmbStore.SelectedIndex > 0)
                {
                    // objQC.sStore = cmbStore.SelectedValue;
                    objQC.sStore = cmbStore.SelectedValue.Split('~').GetValue(0).ToString();
                }

                if (cmbRepairer.SelectedIndex > 0)
                {
                    objQC.sRepairer = cmbRepairer.SelectedValue;
                }

                objQC.sPendingforQC = rdbPendingQC.SelectedValue;
                //if(objQC.sPendingforQC == "1")
                //{
                    
                //}
                objQC.sLocationType = rdbLocationType.SelectedValue;

                if (rdbLocationType.SelectedValue == "4")
                {
                    objQC.sLocationType = "";
                }

                objQC.sDtcCode = sDTCCode;
                objQC.sDtcName = sDTCName;
                objQC.sDtrCode = sTCCode;
                //objQC.sOperator1 = sOperator1;
                objQC.sSerialNo = sSlNo;
                objQC.sUserType = objSession.UserType;
                if (objQC.sOffcode=="")
                {
                    objQC.sOffcode = objSession.OfficeCode;
                }
                objQC.sRoleType = objSession.sRoleType;
                objQC.sRoleId = objSession.RoleId;

                DataTable dt = new DataTable();
                dt = objQC.LoadEnumearionDetails(objQC);
                if (dt.Rows.Count > 0)
                {
                    grdEnumerationDetails.DataSource = dt;
                    grdEnumerationDetails.DataBind();
                    ViewState["QC"] = dt;
                    //cmdApprove.Visible = false;
                }
                else
                {
                    ShowEmptyGrid();

                    //cmdApprove.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
               // clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadEnumeration");
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        bool ValidateForm()
        {
            bool bValidate = false;

            if (rdbPendingQC.SelectedItem.Value == "")
            {
                rdbPendingQC.Focus();
                ShowMsgBox("Please Select the QC Type");
                return false;
            }
            if (rdbLocationType.SelectedItem.Value == "")
            {
                rdbLocationType.Focus();
                ShowMsgBox("Please Select the Location Type");
                return false;
            }
                     

            bValidate = true;
            return bValidate;
        }

        private void ShowMsgBox(string sMsg)
        {
            try
            {
                String sShowMsg = string.Empty;
                sShowMsg = "<script language=javascript> alert ('" + sMsg + "')</script>";
                this.Page.RegisterStartupScript("Msg", sShowMsg);
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDivision.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_CODE\" || '-' || \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\" WHERE \"SD_DIV_CODE\" = '" + cmbDivision.SelectedValue + "' ORDER BY \"SD_SUBDIV_CODE\"", "--Select--", cmbSubdivision);
                    //Genaral.Load_Combo("SELECT FD_FEEDER_CODE, FD_FEEDER_NAME FROM TBLFEEDERMAST, TBLFEEDEROFFCODE WHERE FD_FEEDER_ID = FDO_FEEDER_ID AND FDO_OFFICE_CODE LIKE '" + cmbDivision.SelectedValue + "%' ORDER BY FD_FEEDER_CODE", "--Select--", cmbFeeder);

                    //Commented By sandeep
                    //string strQry = "SELECT \"FD_FEEDER_CODE\",\"FD_FEEDER_CODE\" || '-' || \"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\", \"TBLFEEDEROFFCODE\" WHERE  CAST(\"FD_FEEDER_ID\" AS TEXT) = CAST(\"FDO_FEEDER_ID\" AS TEXT) AND";
                    //strQry += " CAST(\"FDO_OFFICE_CODE\" AS TEXT) LIKE '" + cmbDivision.SelectedValue + "%' ORDER BY \"FD_FEEDER_CODE\"";
                    //Genaral.Load_Combo(strQry, "--Select--", cmbFeeder);

                    string strQry = "SELECT DISTINCT \"FD_FEEDER_CODE\",\"FD_FEEDER_CODE\" || '-' || \"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\", \"TBLFEEDEROFFCODE\" WHERE  \"FD_FEEDER_ID\" = \"FDO_FEEDER_ID\" AND";
                    strQry += " cast(\"FDO_OFFICE_CODE\" as text) LIKE '" + cmbDivision.SelectedValue + "%' ORDER BY \"FD_FEEDER_CODE\"";
                    Genaral.Load_Combo(strQry, "--Select--", cmbFeeder);
                }
                else
                {
                    cmbSection.Items.Clear();
                    cmbSubdivision.Items.Clear();
                    cmbFeeder.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbSubdivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubdivision.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_CODE\" || '-' || \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_SUBDIV_CODE\" = '" + cmbSubdivision.SelectedValue + "' ORDER BY \"OM_CODE\"", "--Select--", cmbSection);
                    //Genaral.Load_Combo("SELECT FD_FEEDER_CODE, FD_FEEDER_NAME FROM TBLFEEDERMAST, TBLFEEDEROFFCODE WHERE FD_FEEDER_ID = FDO_FEEDER_ID AND FDO_OFFICE_CODE LIKE '" + cmbSubdivision.SelectedValue + "%' ORDER BY FD_FEEDER_CODE", "--Select--", cmbFeeder);

                    //string strQry = "SELECT \"FD_FEEDER_CODE\",\"FD_FEEDER_CODE\" || '-' || \"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\", \"TBLFEEDEROFFCODE\" WHERE  CAST(\"FD_FEEDER_ID\" AS TEXT) = CAST(\"FDO_FEEDER_ID\" AS TEXT) AND";
                    //strQry += " CAST(\"FDO_OFFICE_CODE\" AS TEXT) LIKE '" + cmbDivision.SelectedValue + "%' ORDER BY \"FD_FEEDER_CODE\"";
                    //Genaral.Load_Combo(strQry, "--Select--", cmbFeeder);


                    string strQry = "SELECT DISTINCT \"FD_FEEDER_CODE\",\"FD_FEEDER_CODE\" || '-' || \"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\", \"TBLFEEDEROFFCODE\" WHERE  \"FD_FEEDER_ID\" = \"FDO_FEEDER_ID\" AND";
                    strQry += " cast(\"FDO_OFFICE_CODE\" as text) LIKE '" + cmbSubdivision.SelectedValue + "%' ORDER BY \"FD_FEEDER_CODE\"";
                    Genaral.Load_Combo(strQry, "--Select--", cmbFeeder);
                }
                else
                {
                    cmbSection.Items.Clear();
                    cmbFeeder.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubdivision.SelectedIndex > 0)
                {
                    string strQry = "SELECT  DISTINCT \"FD_FEEDER_CODE\",\"FD_FEEDER_CODE\" || '-' || \"FD_FEEDER_NAME\" FROM \"TBLFEEDERMAST\", \"TBLFEEDEROFFCODE\" WHERE  \"FD_FEEDER_ID\" = \"FDO_FEEDER_ID\" AND";
                    strQry += " cast(\"FDO_OFFICE_CODE\" as text) LIKE '" + cmbSection.SelectedValue + "%' ORDER BY \"FD_FEEDER_CODE\"";
                    Genaral.Load_Combo(strQry, "--Select--", cmbFeeder);
                }
                else
                {
                    cmbSection.Items.Clear();
                    cmbFeeder.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }



        protected void grdEnumerationDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {
                if (e.CommandName == "Submit")
                {
                    GridViewRow row = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
                    int rowindex = row.RowIndex;
                    Label lblEnumDetailsId = (Label)grdEnumerationDetails.Rows[rowindex].FindControl("lblEnumDetailsId");
                    Label lblEnumType = (Label)grdEnumerationDetails.Rows[rowindex].FindControl("lblEnumType");
                    string sStatusFlag = ((Label)row.FindControl("lblStatusFlag")).Text;

                    string sEnumDetailsId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblEnumDetailsId.Text));
                    string sEnumType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblEnumType.Text));
                    sStatusFlag = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sStatusFlag));


                    grdEnumerationDetails.AllowPaging = false;
                  
                    //DataTable dt = (DataTable)ViewState["QC"];
                    //grdEnumerationDetails.DataSource = ViewState["QC"];
                    //grdEnumerationDetails.DataBind();

                    int i = 0;
                    string[] strQryVallist = new string[grdEnumerationDetails.Rows.Count];
                    foreach (GridViewRow row1 in grdEnumerationDetails.Rows)
                    {

                        strQryVallist[i] = ((Label)row1.FindControl("lblEnumDetailsId")).Text.Trim() + "`" + ((Label)row1.FindControl("lblEnumType")).Text.Trim();
                        i++;

                    }

                 


                    string sSelectedValue = string.Empty;
                    for (int j = 0; j < strQryVallist.Length; j++)
                    {
                        if (strQryVallist[j] != null)
                        {
                            sSelectedValue += strQryVallist[j].ToString() + "~";
                        }
                    }

                    //string sAllEnumDetails = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sSelectedValue));
                    Session["AllEnumID"] = sSelectedValue;
                    //Session["AllEnumID"]=strQryVallist;

                    Response.Redirect("QCEnumApprove.aspx?QryEnumId=" + sEnumDetailsId + "&EnumType=" + sEnumType  + "&Status=" + sStatusFlag,false);
                    //+ "&AllEnumID=" + sAllEnumDetails
                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDTCcode = (TextBox)row.FindControl("txtDTCcode");
                    TextBox txtDTCName = (TextBox)row.FindControl("txtDTCName");
                    TextBox txtTCcode = (TextBox)row.FindControl("txtTCcode");
                    //TextBox txtOperator1 = (TextBox)row.FindControl("txtOperator1");
                    TextBox txtMake = (TextBox)row.FindControl("txtmake");
                    TextBox txtSlNo = (TextBox)row.FindControl("txtSlNo");

                    LoadEnumeration(txtDTCcode.Text.Replace("'", "`"), txtDTCName.Text.ToUpper().Replace("'", "`"),
                    txtTCcode.Text.Replace("'", "`"),  txtSlNo.Text.ToUpper().Replace("'", "`"));

                    //LoadEnumeration(txtDTCcode.Text.Replace("'", "`"), txtDTCName.Text.ToUpper().Replace("'", "`"),
                    //txtTCcode.Text.Replace("'", "`"), txtOperator1.Text.ToUpper().Replace("'", "`"), txtSlNo.Text.ToUpper().Replace("'", "`"));

                    //DataTable dt = (DataTable)ViewState["QC"];
                    //dv = dt.DefaultView;
                    //if (txtDTCcode.Text != "")
                    //{
                    //    sFilter = "DTE_DTCCODE Like '%" + txtDTCcode.Text.Replace("'", "`") + "%' AND";
                    //}
                    //if (txtDTCName.Text != "")
                    //{
                    //    sFilter = "DTE_NAME Like '%" + txtDTCName.Text.ToUpper().Replace("'", "`") + "%' AND";
                    //}
                    //if (txtTCcode.Text != "")
                    //{
                    //    sFilter = "DTE_TC_CODE Like '%" + txtTCcode.Text.Replace("'", "`") + "%' AND";
                    //}
                    //if (txtOperator1.Text != "")
                    //{
                    //    sFilter += " OPERATOR1 Like '%" + txtOperator1.Text.ToUpper().Replace("'", "`") + "%' AND";
                    //}
                    //if (txtOperator2.Text != "")
                    //{
                    //    sFilter += " OPERATOR2 Like '%" + txtOperator2.Text.ToUpper().Replace("'", "`") + "%' AND";
                    //}
                    //if (sFilter.Length > 0)
                    //{
                    //    sFilter = sFilter.Remove(sFilter.Length - 3);
                    //    grdEnumerationDetails.PageIndex = 0;
                    //    dv.RowFilter = sFilter;
                    //    if (dv.Count > 0)
                    //    {
                    //        grdEnumerationDetails.DataSource = dv;
                    //        ViewState["QC"] = dv.ToTable();
                    //        grdEnumerationDetails.DataBind();

                    //    }
                    //    else
                    //    {

                    //        ShowEmptyGrid();
                    //    }
                    //}
                    //else
                    //{
                    //    cmdLoad_Click(sender, e);

                    //}
                }

              
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void grdEnumerationDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdEnumerationDetails.PageIndex = e.NewPageIndex;
                //DataTable dt = (DataTable)ViewState["QC"];
                grdEnumerationDetails.DataSource = ViewState["QC"];
                grdEnumerationDetails.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("ED_ID");
                dt.Columns.Add("TYPE");
                dt.Columns.Add("DTE_DTCCODE");
                dt.Columns.Add("DTE_NAME");
                dt.Columns.Add("DTE_TC_CODE");
                dt.Columns.Add("OPERATOR1");
                dt.Columns.Add("MAKE");
                dt.Columns.Add("DTE_TC_SLNO");
                dt.Columns.Add("ED_LOCTYPE");
                dt.Columns.Add("ED_STATUS_FLAG");


                grdEnumerationDetails.DataSource = dt;
                grdEnumerationDetails.DataBind();

                int iColCount = grdEnumerationDetails.Rows[0].Cells.Count;
                grdEnumerationDetails.Rows[0].Cells.Clear();
                grdEnumerationDetails.Rows[0].Cells.Add(new TableCell());
                grdEnumerationDetails.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdEnumerationDetails.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }

        // UnUsed Method

        //protected void cmdApprove_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string[] arr = new string[2];
        //        ArrayList strQryValist = new ArrayList();
        //        //string[] strQryVallist = new string[grdEnumerationDetails.Rows.Count];
        //        foreach (GridViewRow row in grdEnumerationDetails.Rows)
        //        {
        //            if (((CheckBox)row.FindControl("chkSelect")).Checked == true)
        //            {
        //                strQryValist.Add(((Label)row.FindControl("lblEnumDetailsId")).Text.Trim());
        //                //strQryVallist[i] = ((Label)row.FindControl("lblEnumDetailsId")).Text.Trim();

        //            }

        //        }
        //        if (strQryValist.Count == 0)
        //        {
        //            ShowMsgBox("Please Select DTC to Approve");
        //            return;
        //        }

        //        clsFieldEnumeration objFieldEnum = new clsFieldEnumeration();
        //        arr = objFieldEnum.GetEnumerationInfoForApprove(objFieldEnum, strQryValist);
        //        if (arr[1] != null)
        //        {
        //            ShowMsgBox(arr[0].ToString());
        //            LoadEnumeration();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}
    }
}