using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections;
using System.IO;
using System.Configuration;



namespace IIITS.DTLMS.MasterForms
{
    public partial class FeederMastApp : System.Web.UI.Page
    {

        string strUserLogged = string.Empty;
        string Officecode = string.Empty;
        ArrayList userdetails = new ArrayList();
        string strFormCode = "FeederMast";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/InternalLogin.aspx", false);
                }

                objSession = (clsSession)Session["clsSession"];
                lblErrormsg.Text = string.Empty;
                txtOfficeCode.Enabled = false;
                Update.Visible = false;
                UpdateFeeder.Visible = false;
                if (!IsPostBack)
                {
                    //if (objSession.UserType != "4")
                    //{
                    //    Response.Redirect("~/UserRestrict.aspx", false);
                    //}

                    //Genaral.Load_Combo("SELECT \"ST_ID\",\"ST_NAME\" from \"TBLSTATION\" ORDER BY \"ST_ID\" ", "--Select--", cmbStation);
                    Genaral.Load_Combo("SELECT \"DT_CODE\",\"DT_NAME\" from \"TBLDIST\" ORDER BY \"DT_CODE\" ", "--Select--", cmbdistrict);
                    Genaral.Load_Combo("SELECT \"FC_ID\",\"FC_NAME\" from \"TBLFEEDERCATEGORY\" ORDER BY \"FC_ID\" asc", "--Select--", cmbCat);
                    Genaral.Load_Combo("SELECT \"FT_ID\",\"FT_NAME\" FROM \"TBLFDRTYPE\"", "--Select--", cmbType);
                    Genaral.Load_Combo("SELECT \"MD_NAME\",\"MD_NAME\" from \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='C' ORDER BY \"MD_ORDER_BY\"", "-Select-", cmbCapacity);



                    if (Request.QueryString["FeederId"] != null && Request.QueryString["FeederId"].ToString() != "")
                    {
                        txtFeederId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["FeederId"]));
                        GetFeederDetails();

                        //cmbDistrict_SelectedIndexChanged(sender, e);
                        //cmbTaluk.SelectedValue = hdfTaluk.Value;
                        //cmbTaluk_SelectedIndexChanged(sender, e);
                        //cmbStation.SelectedValue = hdfStation.Value;

                        cmbStation_SelectedIndexChanged(sender, e);
                        cmbBank.SelectedValue = hdfBank.Value;
                        cmbBank_SelectedIndexChanged(sender, e);
                        cmbbus.SelectedValue = hdfBus.Value;
                        Create.Visible = false;
                        CreateFeeder.Visible = false;

                        Update.Visible = true;
                        UpdateFeeder.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                string[] Arrmsg = new string[2];


                //Check AccessRights
                //bool bAccResult;
                //if (cmdSave.Text == "Update")
                //{
                //    bAccResult = CheckAccessRights("3");
                //}
                //else
                //{
                //    bAccResult = CheckAccessRights("2");
                //}

                //if (bAccResult == false)
                //{
                //    return;
                //}

                if (cmbdistrict.SelectedIndex == 0 || cmbdistrict.SelectedIndex == -1)
                {
                    ShowMsgBox("Select the District");
                    cmbStation.Focus();
                    return;
                }
                if (cmbTaluk.SelectedIndex == 0 || cmbTaluk.SelectedIndex == -1)
                {
                    ShowMsgBox("Select the Taluk");
                    cmbStation.Focus();
                    return;
                }


                if (cmbStation.SelectedIndex == 0 || cmbStation.SelectedIndex == -1)
                {
                    ShowMsgBox("Select the Station");
                    cmbStation.Focus();
                    return;
                }
                //if (cmbBank.SelectedIndex == 0)
                //{
                //    ShowMsgBox("Select the Bank");
                //    cmbBank.Focus();
                //    return;
                //}

                //if (cmbbus.SelectedIndex == 0)
                //{
                //    ShowMsgBox("Select the Bus");
                //    cmbbus.Focus();
                //    return;
                //}

                if (txtOfficeCode.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Office Code length must not be empty");
                    txtOfficeCode.Focus();
                    return;
                }

                if (txtFeederCode.Text.Trim().Length != 6)
                {
                    ShowMsgBox("Feeder Code Must Be 6 Digit Number");
                    txtFeederCode.Focus();
                    return;
                }
                if (txtFeederName.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Enter the Feeder Name");
                    txtFeederName.Focus();
                    return;
                }


                if (cmbCat.SelectedIndex <= 0)
                {
                    ShowMsgBox("Select the Feeder Type");
                    cmbCat.Focus();
                    return;
                }
                if (cmbInt.SelectedIndex == 0||cmbInt.SelectedIndex == -1)
                {
                    ShowMsgBox("Select Shared type");
                    cmbInt.Focus();
                    return;
                }
                if (cmbCapacity.SelectedIndex == 0 ||cmbCapacity.SelectedIndex ==-1)
                {
                    ShowMsgBox("Select Valid Transformer Centre Capacity");
                    cmbCapacity.Focus();
                    return;
                }

                string strFeederType = string.Empty;
                string StrChangeDate = string.Empty;

                clsFeederMast objFeederMaster = new clsFeederMast();
                objFeederMaster.FeederCode = Convert.ToString(txtFeederCode.Text.Trim().ToUpper());
                objFeederMaster.FeederName = txtFeederName.Text.Trim().ToUpper();
                objFeederMaster.Stationid = Convert.ToInt64(cmbStation.SelectedValue);
                string BankId = cmbBank.SelectedValue;
                string BusId = cmbbus.SelectedValue;
                if (BankId == "--Select--" || BankId == "")
                {
                    BankId = "0";
                }
                if (BusId == "--Select--" || BusId == "")
                {
                    BusId = "0";
                }
                objFeederMaster.BankId = Convert.ToInt64(BankId);
                objFeederMaster.BusId = Convert.ToInt64(BusId);
                objFeederMaster.FeederType = cmbType.SelectedValue.Trim().ToUpper();
                objFeederMaster.FeederCategory = cmbCat.SelectedValue.Trim().ToUpper();
                objFeederMaster.FeederInterflow = cmbInt.SelectedValue.Trim().ToUpper();
                objFeederMaster.FeederDCC = cmbCapacity.SelectedValue;
                objFeederMaster.MDMFeedercode = txtMdmFeederCode.Text.Trim().ToUpper();


                string strOfficeCode = txtOfficeCode.Text;

                objFeederMaster.OfficeCode = txtOfficeCode.Text;

                objFeederMaster.UserLogged = objSession.UserId;
                objFeederMaster.FeederID = Convert.ToInt64(txtFeederId.Text);

                if (txtFeederId.Text.Trim() == "0")
                {

                    objFeederMaster.IsSave = true;
                    //string path = "D:\\Host\\TEST.txt";
                    //File.AppendAllText(path, "SyncFeederDetails Before Query ()" + Environment.NewLine);

                    Arrmsg = objFeederMaster.FeederMaster(objFeederMaster);

                    //File.AppendAllText(path, "SyncFeederDetails Afetr Query ()" + Arrmsg[1] + Environment.NewLine);

                    if (Arrmsg[1].ToString() == "0")
                    {
                        bool bResult = objFeederMaster.SyncFeederDetailstoApp(objFeederMaster);
                        if (bResult == false)
                        {
                            ShowMsgBox("Error Occurred While Syncing in APP DB");
                            return;
                        }
                        else
                        {
                            ShowMsgBox("Feeder Details Saved Successfully");
                        }
                        Reset();
                    }
                    if (Arrmsg[1].ToString() == "4")
                    {
                        ShowMsgBox(Arrmsg[0].ToString());
                        mdlPopup.Hide();
                    }

                }
                else
                {
                    objFeederMaster.IsSave = false;
                    //string path = "D:\\Host\\TEST.txt";
                    //File.AppendAllText(path, "SyncFeederDetails Before Query (UPDATE)" + Environment.NewLine);
                    Arrmsg = objFeederMaster.FeederMaster(objFeederMaster);
                    if (Arrmsg[1].ToString() == "0")
                    {
                        bool bResult = objFeederMaster.SyncFeederDetailstoApp(objFeederMaster);

                        //File.AppendAllText(path, "SyncFeederDetails Afetr Query (After UPdate)" + bResult + Environment.NewLine);

                        if (bResult == false)
                        {
                            ShowMsgBox("Error Occurred While Syncing in APP DB");
                            return;
                        }
                        else
                        {
                            ShowMsgBox("Feeder Details Updated Successfully");
                        }
                        Reset();
                    }
                    if (Arrmsg[1].ToString() == "4")
                    {
                        ShowMsgBox(Arrmsg[0].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
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
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        private void PopulateCheckedValues()
        {
            try
            {
                ArrayList userdetails = (ArrayList)ViewState["CHECKED_ITEMS"];

                if (userdetails != null && userdetails.Count > 0)
                {
                    foreach (GridViewRow gvrow in GrdOffices.Rows)
                    {
                        int index = Convert.ToInt32(GrdOffices.DataKeys[gvrow.RowIndex].Value);
                        if (userdetails.Contains(index))
                        {
                            CheckBox myCheckBox = (CheckBox)gvrow.FindControl("cbSelect");
                            myCheckBox.Checked = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        //This method is used to save the checkedstate of values
        private void SaveCheckedValues()
        {
            try
            {
                ArrayList userdetails = new ArrayList();
                ArrayList tmpArrayList = new ArrayList();

                int index = -1;
                string strIndex = string.Empty;
                string strOk1 = string.Empty;
                foreach (GridViewRow gvrow in GrdOffices.Rows)
                {
                    index = Convert.ToInt32(GrdOffices.DataKeys[gvrow.RowIndex].Value);
                    CheckBox result = ((CheckBox)gvrow.FindControl("cbSelect"));
                    // Check in the Session
                    if ((ArrayList)ViewState["CHECKED_ITEMS"] != null)
                        userdetails = (ArrayList)ViewState["CHECKED_ITEMS"];


                    Label lblOff = (Label)gvrow.FindControl("lblOffCode");

                    if (result.Checked == true)
                    {
                        if (!userdetails.Contains(index))
                        {
                            userdetails.Add(index);
                        }
                    }

                    else
                    {
                        if (userdetails.Contains(index))
                        {
                            userdetails.Remove(index);
                        }
                    }
                }
                if (userdetails != null && userdetails.Count > 0)
                    ViewState["CHECKED_ITEMS"] = userdetails;
            }
            catch (Exception ex)
            {

                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }


        public void LoadOffice(string sOfficeCode = "", string sOffName = "")
        {
            try
            {
                DataTable dtPageDetaiils = new DataTable();
                clsFeederMast objStation = new clsFeederMast();
                objStation.OfficeCode = sOfficeCode;
                objStation.OfficeName = sOffName;
                dtPageDetaiils = objStation.LoadOfficeDet(objStation);
                //if (dtPageDetaiils.Rows.Count > 0)
                //{
                GrdOffices.DataSource = dtPageDetaiils;
                GrdOffices.DataBind();
            }

            catch (Exception ex)
            {

                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void GrdOffices_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                SaveCheckedValues();
                GrdOffices.PageIndex = 0;
                GrdOffices.PageIndex = e.NewPageIndex;
                LoadOffice(objSession.OfficeCode);
                PopulateCheckedValues();
                this.mdlPopup.Show();
            }
            catch (Exception ex)
            {

                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void GrdOffices_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                foreach (GridViewRow Row in GrdOffices.Rows)
                {

                    if (e.Row.RowType == DataControlRowType.DataRow)//except header and footer
                    {
                        TextBox txtOff = new TextBox();
                        CheckBox cbSelect = new CheckBox();
                        ArrayList arroffcode = new ArrayList();

                        cbSelect = (CheckBox)e.Row.FindControl("cbSelect");
                        Label lblOff = new Label();

                        lblOff = (Label)Row.FindControl("lblOffCode");

                    }
                }
            }
            catch (Exception ex)
            {

                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void btnOK_Click1(object sender, EventArgs e)
        {
            try
            {
                ArrayList arrChecked = new ArrayList();

                GrdOffices.AllowPaging = false;
                SaveCheckedValues();
                LoadOffice(objSession.OfficeCode);
                PopulateCheckedValues();

                foreach (GridViewRow Row in GrdOffices.Rows)
                {
                    bool result = ((CheckBox)Row.FindControl("cbSelect")).Checked;

                    if (result == true)
                    {
                        arrChecked.Add(((Label)Row.FindControl("lblOffCode")).Text);
                    }
                }

                GrdOffices.AllowPaging = true;
                SaveCheckedValues();
                LoadOffice(objSession.OfficeCode);
                PopulateCheckedValues();


                string sOfficeCode = string.Empty;

                for (int i = 0; i < arrChecked.Count; i++)
                {
                    sOfficeCode += arrChecked[i];
                    if (sOfficeCode.StartsWith(",") == false)
                    {
                        //sOfficeCode =  sOfficeCode;
                    }
                    if (sOfficeCode.EndsWith(",") == false)
                    {
                        sOfficeCode = sOfficeCode + ",";
                    }
                }


                //txtOfficeCode.Text = strOk;
                if (sOfficeCode.EndsWith(",") == true)
                {
                    sOfficeCode = sOfficeCode.Remove(sOfficeCode.Length - 1);
                }

                txtOfficeCode.Text = sOfficeCode;
                txtOfficeCode.Enabled = false;
            }
            catch (Exception ex)
            {

                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            //statement used to hide the modal pop up.
            this.mdlPopup.Hide();

        }


        protected void btnPopByID_Click(object sender, EventArgs e)
        {
            try
            {
                //CustOledbConnection objCon = new CustOledbConnection(Constants.Password);
                //string strOfficeID = txtOffID.Text.Trim();
                //txtOffID.Enabled = false;

            }
            catch (Exception ex)
            {

            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.mdlPopup.Show();
                //userdetails.Clear();     

                if (ViewState["CHECKED_ITEMS"] != null)
                {
                    SaveCheckedValues();
                    LoadOffice(objSession.OfficeCode);
                    PopulateCheckedValues();
                }
                else
                {
                    LoadOffice(objSession.OfficeCode);
                }

            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }



        public void Reset()
        {
            try
            {
                
                txtOfficeCode.Text = string.Empty;
                txtFeederCode.Text = string.Empty;
                txtFeederName.Text = string.Empty;
                
                if (cmbBank.SelectedIndex > 0)
                {
                    cmbBank.SelectedIndex = 0;
                }
                if (cmbbus.SelectedIndex > 0)
                {
                    cmbbus.SelectedIndex = 0;
                }
                if (cmbStation.SelectedIndex > 0)
                {
                    cmbStation.SelectedIndex = 0;
                }
                if (cmbCat.SelectedIndex > 0)
                {
                    cmbCat.SelectedIndex = 0;
                }
                if (cmbdistrict.SelectedIndex > 0)
                {
                    cmbdistrict.SelectedIndex = 0;
                }
                if (cmbTaluk.SelectedIndex > 0)
                {
                    cmbTaluk.SelectedIndex = 0;
                }
                if (cmbType.SelectedIndex > 0)
                {
                    cmbType.SelectedIndex = 0;
                }
                if (cmbInt.SelectedIndex > 0)
                {
                    cmbInt.SelectedIndex = 0;
                }
                if (cmbCapacity.SelectedIndex > 0)
                {
                    cmbCapacity.SelectedIndex = 0;
                }

                //  this.mdlPopup.Show();
                txtOfficeCode.Text = "";
                userdetails.Clear();

                btnSearch.Enabled = true;
                cmdSave.Text = "Save";
                
                txtMdmFeederCode.Text = "";
                txtTotalDtc.Text = string.Empty;
              //  txtCapacity.Text = "";
               // txtTotalDtc.Text = "";
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        public void GetFeederDetails()
        {
            try
            {
                Genaral.Load_Combo("SELECT \"TQ_CODE\",\"TQ_NAME\" FROM \"TBLTALQ\" ", "--Select--", cmbTaluk);
                Genaral.Load_Combo("SELECT \"ST_ID\",\"ST_NAME\" FROM \"TBLSTATION\"", "--Select--", cmbStation);
                Genaral.Load_Combo("SELECT \"BN_ID\",\"BN_NAME\" FROM \"TBLBANK\"", "--Select--", cmbBank);
                Genaral.Load_Combo("SELECT \"BS_ID\",\"BS_NAME\" FROM \"TBLBUS\"", "--Select--", cmbbus);
                clsFeederMast objFeeder = new clsFeederMast();
                ArrayList arrOffCode = new ArrayList();
                ArrayList arrOffCodeValue = new ArrayList();

                DataTable DtFeederDet = objFeeder.GetFeederDetails(txtFeederId.Text);
                if (DtFeederDet.Rows.Count > 0)
                {
                    for(int i = 0; i < DtFeederDet.Rows.Count; i++)
                    {
                        txtOfficeCode.Text += Convert.ToString(DtFeederDet.Rows[i]["OFFCODE"])+',';
                    }

                    DataTable DttxtDTC = objFeeder.GetDTCDetails(txtFeederId.Text);

                    txtFeederCode.Text = Convert.ToString(DtFeederDet.Rows[0]["FD_FEEDER_CODE"]);
                    txtFeederName.Text = Convert.ToString(DtFeederDet.Rows[0]["FD_FEEDER_NAME"]);
                    cmbInt.SelectedValue = Convert.ToString(DtFeederDet.Rows[0]["FD_IS_INTERFLOW"]);
                    cmbCapacity.SelectedValue = Convert.ToString(DtFeederDet.Rows[0]["FD_DTC_CAPACITY"]);
                    cmbStation.SelectedValue = Convert.ToString(DtFeederDet.Rows[0]["STATION_CODE"]);
                    cmbCat.SelectedValue = Convert.ToString(DtFeederDet.Rows[0]["FC_ID"]);
                    hdfBank.Value = Convert.ToString(DtFeederDet.Rows[0]["BN_ID"]);
                    hdfBus.Value = Convert.ToString(DtFeederDet.Rows[0]["BS_ID"]);
                    cmbType.SelectedValue = Convert.ToString(DtFeederDet.Rows[0]["FC_FT_ID"]);
                    txtMdmFeederCode.Text = Convert.ToString(DtFeederDet.Rows[0]["FD_MDM_FEEDERCODE"]);
                    txtTotalDtc.Text = Convert.ToString(DttxtDTC.Rows[0]["TOTALDTC"]);
                    cmbdistrict.SelectedValue = Convert.ToString(DtFeederDet.Rows[0]["DIST_CODE"]);
                    cmbTaluk.SelectedValue = Convert.ToString(DtFeederDet.Rows[0]["TALUQ_CODE"]);


                    if (txtOfficeCode.Text.StartsWith(";"))
                    {
                        txtOfficeCode.Text = txtOfficeCode.Text.Substring(1, txtOfficeCode.Text.Length - 1);
                    }
                    if (txtOfficeCode.Text.EndsWith(";"))
                    {
                        txtOfficeCode.Text = txtOfficeCode.Text.Substring(0, txtOfficeCode.Text.Length - 1);
                    }

                    txtOfficeCode.Text = txtOfficeCode.Text.Replace(";", ",");

                    if (txtOfficeCode.Text.EndsWith(","))
                    {
                        txtOfficeCode.Text = txtOfficeCode.Text.Substring(0, txtOfficeCode.Text.Length - 1);
                    }

                    arrOffCode.AddRange(txtOfficeCode.Text.Split(','));


                    if (txtOfficeCode.Text != "")
                    {
                        for (int i = 0; i < arrOffCode.Count; i++)
                        {
                            arrOffCodeValue.Add(Convert.ToInt32(arrOffCode[i]));
                        }
                    }

                    ViewState["CHECKED_ITEMS"] = arrOffCodeValue;

                    LoadOffice();
                    PopulateCheckedValues();
                }


                cmdSave.Text = "Update";
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbStation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbStation.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"BN_ID\",\"BN_NAME\" FROM \"TBLBANK\" WHERE CAST(\"BN_ST_ID\" AS TEXT)='" + cmbStation.SelectedValue + "'", "--Select--", cmbBank);
                }
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbBank.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"BS_ID\",\"BS_NAME\" FROM \"TBLBUS\" WHERE CAST(\"BS_BN_ID\" AS TEXT)='" + cmbBank.SelectedValue + "'", "--Select--", cmbbus);
                }
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "FeederMast";
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
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;

            }
        }

        #endregion

        protected void GrdOffices_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtOffCode = (TextBox)row.FindControl("txtOffCode");
                    TextBox txtOffName = (TextBox)row.FindControl("txtOffName");

                    LoadOffice(txtOffCode.Text.Trim().Replace("'", "''"), txtOffName.Text.Trim().Replace("'", "''"));

                    this.mdlPopup.Show();

                }
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbdistrict.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"TQ_CODE\",\"TQ_NAME\" FROM \"TBLTALQ\" WHERE CAST(\"TQ_DT_ID\" AS TEXT)='" + cmbdistrict.SelectedValue + "'", "--Select--", cmbTaluk);
                }
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbTaluk_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbTaluk.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"ST_ID\",\"ST_NAME\" FROM \"TBLSTATION\" WHERE SUBSTR(CAST(\"ST_OFF_CODE\" AS TEXT),1,2)='" + cmbTaluk.SelectedValue + "'", "--Select--", cmbStation);
                }
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbType.SelectedIndex == 1)
                {
                    Genaral.Load_Combo("SELECT \"FC_ID\",\"FC_NAME\" from \"TBLFEEDERCATEGORY\" WHERE \"FC_FT_ID\"=1 ORDER BY \"FC_ID\" asc", "--Select--", cmbCat);
                }
                if (cmbType.SelectedIndex == 2)
                {
                    Genaral.Load_Combo("SELECT \"FC_ID\",\"FC_NAME\" from \"TBLFEEDERCATEGORY\" WHERE \"FC_FT_ID\"=2 ORDER BY \"FC_ID\" asc", "--Select--", cmbCat);
                }
                if (cmbType.SelectedIndex == 3)
                {
                    Genaral.Load_Combo("SELECT \"FC_ID\",\"FC_NAME\" from \"TBLFEEDERCATEGORY\" WHERE \"FC_FT_ID\"=3 ORDER BY \"FC_ID\" asc", "--Select--", cmbCat);
                }
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

    }
}