using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Data.OleDb;
using System.Configuration;

namespace IIITS.DTLMS.Transaction
{
    public partial class DTrAllocation : System.Web.UI.Page
    {
        string strFormCode = "DTrAllocation";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }

                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;
                if (!IsPostBack)
                {

                    AdminAccess();

                    string strQry = string.Empty;
                    strQry += "Title=Search and Select DTR Code Details&";
                    strQry += "Query=select  \"TC_CODE\",\"TC_SLNO\" FROM \"TBLTCMASTER\"  WHERE \" TC_LOCATION_ID\" LIKE '" + objSession.OfficeCode + "%' AND {0} like %{1}% order by \"TC_CODE\" &";
                    strQry += "DBColName=\"TC_CODE\"~\"TC_SLNO\"&";
                    strQry += "ColDisplayName=DTr Code~DTr Slno&";

                    strQry = strQry.Replace("'", @"\'");

                    btnSearchId.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtTcCode.ClientID + "&btn=" + btnSearchId.ClientID + "',520,520," + txtTcCode.ClientID + ")");

                   
                    strQry = "Title=Search and Select DTC  Details&";
                    strQry += "Query=select \"DT_CODE\",\"DT_NAME\" FROM \"TBLDTCMAST\" WHERE \"DT_OM_SLNO\" LIKE '" + objSession.OfficeCode + "%' AND ";
                    strQry += " {0} like %{1}% order by \"DT_CODE\" &";
                    strQry += "DBColName=\"DT_CODE\"~\"DT_NAME\"&";
                    strQry += "ColDisplayName=DTC Code~DTC Name&";

                    strQry = strQry.Replace("'", @"\'");

                    cmdDTCSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtDTCCode.ClientID + "&btn=" + cmdDTCSearch.ClientID + "',520,520," + txtDTCCode.ClientID + ")");
                  
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


      
        protected void cmdAllocate_Click(object sender, EventArgs e)
        {
            try
            {

                if (txtTcCode.Text == "")
                {
                    ShowMsgBox("Please Enter Valid DTr Code");
                    txtTcCode.Focus();
                    return;
                }
                if (cmbType.SelectedIndex == 0)
                {
                    ShowMsgBox("Please Select Type to Allocate");
                    cmbType.Focus();
                    return;
                }
                if (cmbType.SelectedValue == "1")
                {
                    
                    if (cmbStore.SelectedIndex == 0)
                    {
                        ShowMsgBox("Please Select Store");
                        cmbStore.Focus();
                        return;
                    }
                                    
                    DTRStoreAllocation();
                }
                else if (cmbType.SelectedValue == "2")
                {
                    if (txtDTCCode.Text == "")
                    {
                        ShowMsgBox("Please Enter Valid DTC Code");
                        txtTcCode.Focus();
                        return;
                    }

                    DTRFieldAllocation();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void DTRStoreAllocation()
        {
            try
            {
                clsDTrAllocation objAllocation = new clsDTrAllocation();
                string[] Arr = new string[2];
                string sOfficeCode = string.Empty;
                int Division = Convert.ToInt32(ConfigurationSettings.AppSettings["Division_code"]);
                if (objSession.OfficeCode.Length > 2)
                {
                    sOfficeCode = objSession.OfficeCode.Substring(0, Division);
                }
                else
                {
                    sOfficeCode = objSession.OfficeCode;
                }

                objAllocation.sOfficeCode = cmbStore.SelectedValue.Split('~').GetValue(1).ToString();
                objAllocation.sStoreId = cmbStore.SelectedValue.Split('~').GetValue(0).ToString();
                objAllocation.sCrby = objSession.UserId;
                objAllocation.sDTrCode = txtTcCode.Text;
                objAllocation.sUserName = objSession.FullName;
                objAllocation.sStoreName = cmbStore.SelectedItem.Text;

                DataTable dt = (DataTable)ViewState["DTRDetails"];
                if (dt.Rows.Count > 0)
                {
                    objAllocation.sDTCCode = Convert.ToString(dt.Rows[0]["DTC_CODE"]).ToUpper();
                }

                Arr = objAllocation.DTrStoreAllocation(objAllocation);
                if (Arr[1].ToString() == "0")
                {
                    ShowMsgBox(Arr[0].ToString());
                    LoadDTrDetails();
                    cmdAllocate.Enabled = false;
                    return;
                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void DTRFieldAllocation()
        {
            try
            {
                clsDTrAllocation objAllocation = new clsDTrAllocation();
                string[] Arr = new string[2];
                string sOfficeCode = string.Empty;

                objAllocation.sCrby = objSession.UserId;
                objAllocation.sDTrCode = txtTcCode.Text;
                objAllocation.sUserName = objSession.FullName;
                objAllocation.sNewDTCCode = txtDTCCode.Text.ToUpper();

                DataTable dt = (DataTable)ViewState["DTRDetails"];
                if (dt.Rows.Count > 0)
                {
                    objAllocation.sDTCCode = Convert.ToString(dt.Rows[0]["DTC_CODE"]).ToUpper();
                }

                Arr = objAllocation.DTrFieldAllocation(objAllocation);
                if (Arr[1].ToString() == "0")
                {
                    ShowMsgBox(Arr[0].ToString());
                    LoadDTrDetails();
                    cmdAllocate.Enabled = false;
                    return;
                }
                else if (Arr[1].ToString() == "2")
                {
                    ShowMsgBox(Arr[0].ToString());
                    return;
                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdTcDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {

                grdTcDetails.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["DTRDetails"];
                grdTcDetails.DataSource = dt;
                grdTcDetails.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        bool ValidateForm()
        {
            bool bValidate = false;
            try
            {

                if (txtTcCode.Text == "")
                {
                    ShowMsgBox("Please Enter the DTr Code");
                    txtTcCode.Focus();
                    return bValidate;
                }
                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                bValidate = false;
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return bValidate;
            }
        }
        public void LoadDTrDetails()
        {
            try
            {
                if (ValidateForm() == true)
                {
                    clsDTrAllocation objAllocation = new clsDTrAllocation();
                    DataTable dt = new DataTable();

                    dt = objAllocation.LoadDTrDetails(txtTcCode.Text);
                    grdTcDetails.DataSource = dt;
                    grdTcDetails.DataBind();
                    ViewState["DTRDetails"] = dt;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdTcDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                // LOC_TYPE  1--> Store  2---> Field  3----> Repairer

                //DRT_ACT_REFTYPE   1---> Against Purchase Order  2---> After RI       3-->Failure             4--->Sent To Repairer 
                //                  5----> Recieve from Repairer  6----> Scarp Entry   7---> Scrap Disposal    8------> Inspection
                if (e.CommandName == "View")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();
                    string sUrl = string.Empty;
                    string sUrlPath = string.Empty;

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    Label lblRefType = (Label)row.FindControl("lblRefType");
                    Label lblRefNo = (Label)row.FindControl("lblRefNo");
                    Label lblDTrStatus = (Label)row.FindControl("lblDTrStatus");                
                   
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void lnkDTrDetails_Click(object sender, EventArgs e)
        {
            try
            {
                clsTcTracker objTraker = new clsTcTracker();
                string sTCId = objTraker.GetTCIdFromCode(txtTcCode.Text);

                sTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sTCId));

                string url = "/MasterForms/TcMaster.aspx?TCId=" + sTCId;
                //string s = "window.open('" + url + "', 'popup_window', 'width=300,height=100,left=100,top=100,resizable=yes');";
                string s = "window.open('" + url + "', '_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbType.SelectedValue == "1")
                {

                    dvStore.Style.Add("display", "block");
                    dvDTC.Style.Add("display", "none");

                    
                    txtDTCCode.Text = string.Empty;
                   
                }
                else if (cmbType.SelectedValue == "2")
                {
                    dvStore.Style.Add("display", "none");
                    dvDTC.Style.Add("display", "block");
                    txtDTCCode.Text = string.Empty;
                    if (cmbStore.SelectedIndex > 0)
                    {
                        cmbStore.SelectedIndex = 0;
                    }
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void btnSearchId_Click(object sender, EventArgs e)
        {
            try
            {
                dvBasicDetails.Style.Add("display", "block");
                LoadDTrDetails();
                VisibleComboType();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void VisibleComboType()
        {
            try
            {
                DataTable dt = (DataTable)ViewState["DTRDetails"];
                if (dt.Rows.Count > 0)
                {
                    string sDTCCode = Convert.ToString(dt.Rows[0]["DTC_CODE"]);
                    if (sDTCCode == "")
                    {
                        cmbType.SelectedValue = "2";
                        cmbType.Enabled = false;

                        dvStore.Style.Add("display", "none");
                        dvDTC.Style.Add("display", "block");
                    }
                    else
                    {
                        cmbType.SelectedValue = "1";
                        cmbType.Enabled = false;
                       // cmbType.SelectedIndex = 0;

                        dvStore.Style.Add("display", "block");
                        dvDTC.Style.Add("display", "none");

                        string sOfficeCode = string.Empty;
                        int Division = Convert.ToInt32(ConfigurationSettings.AppSettings["Division_code"]);
                        if (objSession.OfficeCode.Length > 2)
                        {
                            sOfficeCode = objSession.OfficeCode.Substring(0, Division);
                        }
                        else
                        {
                            sOfficeCode = objSession.OfficeCode;
                        }

                        Genaral.Load_Combo("SELECT \"SM_ID\" || '~' || \"SM_OFF_CODE\",\"SM_NAME\" FROM \"TBLSTOREMAST\" WHERE \"SM_OFF_CODE\" LIKE '" + sOfficeCode + "%'", "--Select--", cmbStore);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        #region Access Rights

        public void AdminAccess()
        {
            try
            {
                if (objSession.RoleId != "8")
                {
                    Response.Redirect("~/UserRestrict.aspx", false);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        #endregion

        protected void cmbReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtTcCode.Text = string.Empty;
                txtDTCCode.Text = string.Empty;
                cmdAllocate.Enabled = true;
                cmbStore.SelectedIndex = 0;
                cmbType.SelectedIndex = 0;

                dvStore.Style.Add("display", "none");
                dvDTC.Style.Add("display", "none");

                dvBasicDetails.Style.Add("display", "none");
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}
    




