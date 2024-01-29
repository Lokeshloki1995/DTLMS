using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Configuration;

namespace IIITS.DTLMS.BasicForms
{
    public partial class Station : System.Web.UI.Page
    {

        ArrayList userdetails = new ArrayList();
        string[] tmpuserlist = new string[50];
        string strUserLogged = string.Empty;
        string strFormCode = "Station";
        string strEmpId = string.Empty;
        clsSession objSession;
        int Circle_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Circle_code"]);
        int Division_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Division_code"]);
        int SubDiv_code = Convert.ToInt32(ConfigurationSettings.AppSettings["SubDiv_code"]);
        int Section_code = Convert.ToInt32(ConfigurationSettings.AppSettings["Section_code"]);
        int Feeder_code = Convert.ToInt32(ConfigurationSettings.AppSettings["feeder_code"]);

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

                    Form.DefaultButton = cmdSave.UniqueID;
                    Update.Visible = false;
                    UpdateStation.Visible = false;
                    lblErrormsg.Text = string.Empty;
                    if (!IsPostBack)
                    {

                        Genaral.Load_Combo("SELECT \"DT_CODE\",\"DT_CODE\" || '-' || \"DT_NAME\" FROM \"TBLDIST\" ORDER BY \"DT_CODE\"", "--Select--", cmbDistrict);
                        Genaral.Load_Combo("SELECT \"STC_CAP_ID\", \"STC_CAP_VALUE\" FROM \"TBLSTATIONCAPACITY\" ORDER BY \"STC_CAP_ID\"", "--Select--", cmbCapacity);
                        Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone);
                        if (Request.QueryString["StationId"] != null && Convert.ToString(Request.QueryString["StationId"]) != "")
                        {
                            txtStationId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["StationId"]));
                        }

                        if (txtStationId.Text != "0")
                        {

                            LoadStationDet(txtStationId.Text);
                            cmbDistrict_SelectedIndexChanged(sender, e);
                            cmbTalq.SelectedValue = Convert.ToString(txtStationCode.Text.Substring(0, 2));
                            Create.Visible = false;
                            CreateStation.Visible = false;

                            Update.Visible = true;
                            UpdateStation.Visible = true;

                        }
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

                strUserLogged = objSession.UserId;
                if (txtStatName.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Enter Station Name");
                    txtStatName.Focus();
                    return;
                }
                //if (txtStatName.Text.Trim().StartsWith(".") )
                //{
                //    ShowMsgBox("Enter the valid Station Name");
                //    txtStatName.Focus();
                //    return;
                //}
                //if (!System.Text.RegularExpressions.Regex.IsMatch(txtStatName.Text, @"^[a-zA-Z0-9_ ]*$"))
                //{
                //    txtStatName.Focus();
                //    ShowMsgBox("Please enter Valid Station Name");
                //    return;
                //}

                if (cmbDistrict.SelectedValue == "0")
                {
                    ShowMsgBox("Select District");
                    cmbDistrict.Focus();
                    return;
                }
                if (cmbTalq.SelectedValue == "0")
                {
                    ShowMsgBox("Select Taluk");
                    cmbTalq.Focus();
                    return;
                }

                if (txtStationCode.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Enter Station Code");
                    txtStationCode.Focus();
                    return;
                }
                if (txtStationCode.Text.Length != 4)
                {
                    ShowMsgBox("Station Code should be 4 digits");
                    txtStationCode.Focus();
                    return;
                }
                //if (Convert.ToString(cmbDistrict.SelectedValue) != txtStationCode.Text.Substring(0, 1))
                //{
                //    ShowMsgBox("District Code and Station Code Does not Match");
                //    txtStationCode.Focus();
                //    return;
                //}


                //if (Convert.ToString(cmbTalq.SelectedValue) != txtStationCode.Text.Substring(0, 2))
                //{
                //    ShowMsgBox("Taluk Code and Station Code Does not Match");
                //    txtStationCode.Focus();
                //    return;
                //}
                //if (!System.Text.RegularExpressions.Regex.IsMatch(txtStatName.Text, "^\\s*[a-zA-Z0-9 \\s]+\\s*[a-zA-Z0-9_.+-]\\s*$"))
                //{
                //    ShowMsgBox("Please enter Valid Station Name ");
                //    txtStatName.Focus();
                //    return;
                //}
                if (cmbCapacity.SelectedIndex == 0)
                {
                    ShowMsgBox("Select the required Voltage Class");
                    cmbCapacity.Focus();
                    return;
                }
              
                if (txtMobileNo.Text.Length != 10 && txtMobileNo.Text.Length>0)
                {
                    ShowMsgBox("Enter Valid 10 Digit Mobile Number");
                    txtMobileNo.Focus();
                    return;
                }
                
                if (txtEmailId.Text.Length>0 && !System.Text.RegularExpressions.Regex.IsMatch(txtEmailId.Text, "^\\s*[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+[a-zA-Z0-9]\\s*$"))
                {
                    ShowMsgBox("Please enter Valid Email (xyz@aaa.com)");
                    txtEmailId.Focus();
                    return;
                }

                clsStation objStation = new clsStation();
                objStation.StationId = txtStationId.Text;
                objStation.StationName = txtStatName.Text.Trim();
                objStation.StationCode = txtStationCode.Text.Trim();
                objStation.EmailId = txtEmailId.Text.Trim();
                objStation.MobileNo = txtMobileNo.Text.Trim();
                objStation.OfficeCode = cmbSubDiv.SelectedValue;

                objStation.UserLogged = strUserLogged;
                objStation.sTaluqCode = objStation.StationCode.Substring(0, 2);

                objStation.Description = txtDesc.Text.Trim();
                objStation.Capacity = cmbCapacity.SelectedValue;

                if (cmdSave.Text.Equals("Save"))
                {
                    objStation.IsSave = true;
                    Arrmsg = objStation.SaveStationDetails(objStation);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " Station Master ");
                    }

                    if (Arrmsg[1].ToString() == "0")
                    {
                        ShowMsgBox(Arrmsg[0]);


                    }
                    if (Arrmsg[1].ToString() == "4")
                    {
                        ShowMsgBox(Arrmsg[0]);
                    }

                }
                if (cmdSave.Text.Equals("Update"))
                {
                    objStation.IsSave = false;
                    Arrmsg = objStation.SaveStationDetails(objStation);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " Station Master ");
                    }

                    if (Arrmsg[1].ToString() == "0")
                    {
                        ShowMsgBox(Arrmsg[0]);


                    }
                    if (Arrmsg[1].ToString() == "4")
                    {
                        ShowMsgBox(Arrmsg[0]);
                    }

                }

                txtStationCode.Text = string.Empty;
                txtStatName.Text = string.Empty;
                txtDesc.Text = string.Empty;
                //userdetails.Clear();
                cmbDistrict.SelectedIndex = 0;
                cmbTalq.SelectedIndex = 0;
                cmbCircle.SelectedIndex = 0;
                cmbDivision.SelectedIndex = 0;
                cmbSubDiv.SelectedIndex = 0;
                txtMobileNo.Text = string.Empty;
                txtEmailId.Text = string.Empty;
                cmdSave.Text = "Save";
                cmbCapacity.SelectedIndex = 0;
                cmbDistrict.Enabled = true;
                cmbTalq.Enabled = true;
                txtStationCode.Enabled = true;
                txtStationCode.ReadOnly = false;
                //LoadStation();
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
                Response.Write(ex);
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Reset Fn
        /// </summary>
        private void Reset()
        {
            try
            {
                //txtStationCode.Text = string.Empty;
                //txtStatName.Text = string.Empty;
                //txtDesc.Text = string.Empty;
                //userdetails.Clear();
                //cmbDistrict.SelectedIndex = 0;
                //cmbTalq.SelectedIndex = 0;
                //cmbCircle.SelectedIndex = 0;
                //cmbDivision.SelectedIndex = 0;
                //cmbSubDiv.SelectedIndex = 0;
                //txtMobileNo.Text = string.Empty;
                //txtEmailId.Text = string.Empty;
                //cmdSave.Text = "Save";
                //cmbCapacity.SelectedIndex = 0;
                //cmbDistrict.Enabled = true;
                //cmbTalq.Enabled = true;
                //txtStationCode.Enabled = true;
                //txtStationCode.ReadOnly = false;
                Response.Redirect("Station.aspx", false);
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        public void LoadStationDet(string strStationId)
        {
            try
            {
                clsStation ObjStation = new clsStation();
                ArrayList arrOffCode = new ArrayList();
                ArrayList arrOffCodeValue = new ArrayList();

                DataTable DtStationDet = ObjStation.LoadStationDetail(strStationId);

                txtStatName.Text = Convert.ToString(DtStationDet.Rows[0]["ST_NAME"]);
                txtStationCode.Text = Convert.ToString(DtStationDet.Rows[0]["ST_STATION_CODE"]);
                txtDesc.Text = Convert.ToString(DtStationDet.Rows[0]["ST_DESCRIPTION"]);

                txtMobileNo.Text = Convert.ToString(DtStationDet.Rows[0]["ST_MOBILE_NO"]);
                txtEmailId.Text = Convert.ToString(DtStationDet.Rows[0]["ST_EMAILID"]);
                ObjStation.OfficeCode = Convert.ToString(DtStationDet.Rows[0]["ST_OFF_CODE"]);
                cmbCapacity.SelectedValue = Convert.ToString(DtStationDet.Rows[0]["ST_STC_CAP_ID"]);
                cmbDistrict.SelectedValue = Convert.ToString(txtStationCode.Text.Substring(0, 1));
                cmbTalq.SelectedValue = Convert.ToString(txtStationCode.Text.Substring(0, 2));
                cmbZone.SelectedValue = Convert.ToString(DtStationDet.Rows[0]["ST_OFF_CODE"]).Substring(0, Constants.Zone);
                cmbZone_SelectedIndexChanged(this, EventArgs.Empty);
                cmbCircle.SelectedValue = Convert.ToString(DtStationDet.Rows[0]["ST_OFF_CODE"]).Substring(0, Circle_code);
                cmbCircle_SelectedIndexChanged(this, EventArgs.Empty);
                cmbDivision.SelectedValue = Convert.ToString(DtStationDet.Rows[0]["ST_OFF_CODE"]).Substring(0, Division_code);
                cmbDivision_SelectedIndexChanged(this, EventArgs.Empty);
                //Genaral.Load_Combo("select \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\",\"SD_DIV_CODE\" FROM \"TBLSUBDIVMAST\" where \"SD_SUBDIV_CODE\"='" + ObjStation.OfficeCode + "' ORDER BY \"SD_SUBDIV_CODE\"", "--Select--", cmbSubDiv);
                cmbSubDiv.SelectedValue = Convert.ToString(DtStationDet.Rows[0]["ST_OFF_CODE"]);
                //Genaral.Load_Combo("select \"DIV_CODE\",\"DIV_NAME\",\"DIV_CICLE_CODE\" FROM \"TBLDIVISION\" where \"DIV_CICLE_CODE\"='" + cmbCircle.SelectedValue + "' ORDER BY \"DIV_CODE\"", "--Select--", cmbDivision);
               // cmbDivision.SelectedValue = Convert.ToString(DtStationDet.Rows[0]["ST_OFF_CODE"]).Substring(0, Division_code);
                cmbDistrict.Enabled = false;
                cmbTalq.Enabled = false;
                txtStationCode.Enabled = false;
                ViewState["CHECKED_ITEMS"] = arrOffCodeValue;

                //LoadOffice();



                cmdSave.Text = "Update";


            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void cmdReset_Click1(object sender, EventArgs e)
        {
            Reset();
        }

        public void LoadOffice(string sOfficeCode = "", string sOffName = "")
        {
            try
            {
                DataTable dtPageDetaiils = new DataTable();
                clsStation objStation = new clsStation();
                objStation.OfficeCode = sOfficeCode;
                objStation.OfficeName = sOffName;
                dtPageDetaiils = objStation.LoadOfficeDet(objStation);

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
                if (cmbDistrict.SelectedIndex > 0)
                {
                    Genaral.Load_Combo(" SELECT \"TQ_CODE\",\"TQ_CODE\" || '-' || \"TQ_NAME\" FROM \"TBLTALQ\" WHERE CAST(\"TQ_DT_ID\" AS TEXT) like '" + cmbDistrict.SelectedValue + "%'  order by \"TQ_CODE\"", "--Select--", cmbTalq);
                }
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Genaral.Load_Combo("select \"DIV_CODE\",\"DIV_NAME\",\"DIV_CICLE_CODE\" FROM \"TBLDIVISION\"  WHERE CAST(\"DIV_CICLE_CODE\" AS TEXT) LIKE  '" + cmbCircle.SelectedValue + "%' ORDER BY \"DIV_CODE\" ", "--Select--", cmbDivision);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Genaral.Load_Combo("select \"SD_SUBDIV_CODE\",\"SD_SUBDIV_CODE\" || '-' || \"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE CAST(\"SD_DIV_CODE\" AS TEXT) like '" + cmbDivision.SelectedValue + "%'  order by \"SD_SUBDIV_CODE\"", "--Select--", cmbSubDiv);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbSubDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                clsStation objStation = new clsStation();
                objStation.OfficeCode = cmbSubDiv.SelectedValue;
                //objStation.OfficeCode = objStation.GenerateSectionCode(objStation);
                //txtStationCode.ReadOnly = true;
            }
            catch (Exception ex)
            {

                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdParentStID_Click(object sender, EventArgs e)
        {

            string strLocationId = string.Empty;
            strLocationId += "Title=Search Window&";
            string strDeviceId = string.Empty;
            strEmpId += "Title=Search and Select Parent station Details&";
            strEmpId += "Query=select \"ST_ID\" StationID,\"ST_NAME\" StationName,\"ST_STATION_CODE\" StationCode FROM   \"TBLSTATION\",\"TBLSTATIONCAPACITY\"  WHERE \"ST_STC_CAP_ID\"=\"STC_CAP_ID\"  and \"STC_CAP_ID\">" + cmbCapacity.SelectedValue + " and  LOWER({0}) like %{1}% order by \"ST_NAME\"&";
            strEmpId += "DBColName=\"ST_NAME\"~\"ST_STATION_CODE\"&";
            strEmpId += "ColDisplayName=StationName~StationCode&";

        }

        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "Station";
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

        protected void cmbTalq_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                clsStation objStation = new clsStation();
                objStation.StationCode = cmbTalq.SelectedValue;
                txtStationCode.Text = objStation.GenerateSectionCode(objStation);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbZone.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("select \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" from \"TBLCIRCLE\" WHERE CAST(\"CM_CIRCLE_CODE\" AS TEXT) LIKE '"+ cmbZone.SelectedValue + "%' ORDER BY \"CM_CIRCLE_CODE\" ", "--Select--", cmbCircle);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}