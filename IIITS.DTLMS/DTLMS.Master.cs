using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using IIITS.DTLMS.BL;
using IIITS.DTLMS.BL.MasterForms;

namespace IIITS.DTLMS
{
    public partial class DTLMS : System.Web.UI.MasterPage
    {
        clsSession objSession = new clsSession();
        string strFormCode = "DTLMS.MASTER";
        protected void Page_Load(object sender, EventArgs e)
        {
            bool res = true;

            try
            {
                if (!IsPostBack)
                {
                    if (Session["FullName"] != null && Session["FullName"].ToString() != "")
                    {
                        lblUserName.Text = "Welcome " + Session["FullName"].ToString();
                        clsSession objSession = (clsSession)Session["clsSession"];

                        string strAdminRole = string.Empty;
                        strAdminRole = objSession.RoleId;

                        string Userroleid = Convert.ToString(ConfigurationSettings.AppSettings["SELECTEDADMIN"]);
                        string[] srole = Userroleid.Split(',');
                        for (int i = 0; i < srole.Length; i++)
                        {
                            if (objSession.RoleId != srole[i])
                            {
                                lioptReport.Style.Add("display", "none");
                            }
                            else
                            {
                                lioptReport.Style.Add("display", "block");
                                break;
                            }
                        }
                        ClsUserLoginDetails objuserlogin = new ClsUserLoginDetails();
                        objuserlogin.UpdateDIDetails(objSession.UserId);
                      string logintime=  objuserlogin.getLastlogintime(objSession.UserId);
                        LastloginTime.Text = "Last Login Date and Time : " + logintime;
                        Version_Id.Text= objuserlogin.getVersion();
                        if (strAdminRole == Convert.ToString(ConfigurationSettings.AppSettings["AdminRole"]) ||
                            strAdminRole == Convert.ToString(ConfigurationSettings.AppSettings["AdminRole1"]) || 
                            strAdminRole == Convert.ToString(ConfigurationSettings.AppSettings["SupAdminRole"]))
                        {
                            liAdminActivities.Visible = true;
                            liKanAdminActivities.Visible = true;
                        }
                        else
                        {
                            liAdminActivities.Visible = false;
                            liKanAdminActivities.Visible = false;
                        }
                        if (strAdminRole == Convert.ToString(ConfigurationSettings.AppSettings["suppact"]) || strAdminRole == Convert.ToString(ConfigurationSettings.AppSettings["suppact1"]))
                        {
                            supportactivitys.Visible = true;
                        }
                        else
                        {
                            supportactivitys.Visible = false;
                        }
                      
                        if (objSession.OfficeCode != "" && objSession.OfficeCode != "0")
                        {
                            // lblOfficeName.Text = objSession.OfficeName + " [ " + objSession.OfficeCode + " ]";
                            lblOfficeName.Text = objSession.OfficeNameWithType;
                        }
                        else
                        {
                            lblOfficeName.Text = objSession.OfficeName;
                        }
                        lblDesign.Text = objSession.Designation;

                        if (objSession.UserType != null)
                        {
                            InternalUserLogin(objSession.UserType, objSession.UserId);
                        }
                        if (Session["ChangPwd"] == null || Session["ChangPwd"].ToString() == "")
                        {
                            liDashboard.Style.Add("display", "none");
                            liMaster.Style.Add("display", "none");
                            liFailure.Style.Add("display", "none");
                            liRepair.Style.Add("display", "none");
                            liScrap.Style.Add("display", "none");
                            liMaintainance.Style.Add("display", "none");
                            liPermanentdec.Style.Add("display", "none");
                            liInterStore.Style.Add("display", "none");
                            liUser.Style.Add("display", "none");
                            liApproval.Style.Add("display", "none");
                            liTransaction.Style.Add("display", "none");
                            liOffDesg.Style.Add("display", "none");
                            liMainReport.Style.Add("display", "none");
                            li1.Style.Add("display", "none");
                            liMismatch.Style.Add("display", "none");
                            liAdminActivities.Style.Add("display", "none");
                            liEnumeration.Style.Add("display", "none");
                            liDeviceReg.Style.Add("display", "none");
                            liFeederMapping.Style.Add("display", "none");
                            liVideoTutorial.Style.Add("display", "none");
                        }
                        string Roleid = Convert.ToString(ConfigurationSettings.AppSettings["SELECTEDUSER1"]);
                        string[] Roid = Roleid.Split(',');
                        for (int i = 0; i < Roid.Length; i++)
                        {
                            if (objSession.RoleId != Roid[i])
                            {
                                liMismatch.Style.Add("display", "none");
                            }
                            else
                            {
                                liMismatch.Style.Add("display", "block");
                            }
                        }
                    }
                }

                if (Session["Lang"] == "Kannada")
                {
                    rdbKannada.Checked = true;
                    liKanDashboard.Visible = true;
                    liDashboard.Visible = false;
                    liKanUser.Visible = true;
                    liUser.Visible = false;
                    liKanMaster.Visible = true;
                    liMaster.Visible = false;
                    liKanFailure.Visible = true;
                    liFailure.Visible = false;
                    liKanRepair.Visible = true;
                    liRepair.Visible = false;
                    liKanScrap.Visible = true;
                    liScrap.Visible = false;
                    liKanMaintainance.Visible = true;
                    liMaintainance.Visible = false;
                    liKanBilling.Visible = true;
                    liKanPermanentdec.Visible = true;
                    liPermanentdec.Visible = false;
                    liKanInterStore.Visible = true;
                    liInterStore.Visible = false;
                    liKanInterBank.Visible = true;
                    liKanApproval.Visible = true;
                    liApproval.Visible = false;
                    liKanTransaction.Visible = true;
                    liTransaction.Visible = false;
                    liKanMainReport.Visible = true;
                    liMainReport.Visible = false;
                    liKan1.Visible = true;
                    li1.Visible = false;
                    liKanOffDesg.Visible = true;
                    liOffDesg.Visible = false;
                    Label1.Text = lblOfficeName.Text;
                    Label2.Text = lblDesign.Text;
                    liAdminActivities.Visible = false;
                }
                else
                {
                    rdbEnglish.Checked = true;
                    liDashboard.Visible = true;
                    liKanDashboard.Visible = false;
                    liKanUser.Visible = false;
                    liKanMaster.Visible = false;
                    liKanFailure.Visible = false;
                    liKanRepair.Visible = false;
                    liKanScrap.Visible = false;
                    liKanMaintainance.Visible = false;
                    liKanBilling.Visible = false;
                    liKanPermanentdec.Visible = false;
                    liKanInterStore.Visible = false;
                    liKanInterBank.Visible = false;
                    liKanApproval.Visible = false;
                    liKanTransaction.Visible = false;
                    liKanMainReport.Visible = false;
                    liKan1.Visible = false;
                    liKanOffDesg.Visible = false;
                    liKanAdminActivities.Visible = false;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        
        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            DataTable dtConfiguration = new DataTable();
            clsLogin objLogin = new clsLogin();
            clsSession objSession = (clsSession)Session["clsSession"];
            try
            {
                dtConfiguration = objLogin.GetConfiguration();
                if (dtConfiguration.Rows.Count > 0)
                {
                    if (Convert.ToString(dtConfiguration.Rows[0]["CG_GEN_LOG"]) == "1") // Login Logout Log
                    {
                        Genaral.GeneralLog(objSession.sClientIP, objSession.UserId, "LOGOUT");
                    }
                }
                Session.Abandon();
                Session.Clear();
                ClsUserLoginDetails objuserlogin = new ClsUserLoginDetails();
                objuserlogin.UpdateDIDetails(objSession.UserId);
                
                if (!liOffDesg.Style.Value.Contains("none"))
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                else
                {
                    if (Session["ChangPwd"] == null || Session["ChangPwd"].ToString() == "")
                    {
                        Response.Redirect("~/Login.aspx", false);
                    }
                    else
                    {
                        Response.Redirect("~/InternalLogin.aspx", false);
                    }
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                Response.Redirect("~/Login.aspx", false);
            }
        }

        public void InternalUserLogin(string sUserType, string sUserId)
        {
            try
            {
                liDashboard.Style.Add("display", "none");
                liMaster.Style.Add("display", "none");
                liFailure.Style.Add("display", "none");
                liRepair.Style.Add("display", "none");
                liScrap.Style.Add("display", "none");
                liMaintainance.Style.Add("display", "none");
                liPermanentdec.Style.Add("display", "none");
                liInterStore.Style.Add("display", "none");
                liUser.Style.Add("display", "none");
                liApproval.Style.Add("display", "none");
                liTransaction.Style.Add("display", "none");
                liOffDesg.Style.Add("display", "none");
                liMainReport.Style.Add("display", "none");
                liInterDash.Style.Add("display", "block");
                liInterReports.Style.Add("display", "block");

                //QC Executive
                if (sUserType == "2")
                {
                    liQC.Style.Add("display", "block");
                }
                //Operator and Supervisor
                if (sUserType == "1" || sUserType == "3" || sUserType == "5")
                {
                    liEnumeration.Style.Add("display", "block");
                }
                // Internal Admin
                if (sUserType == "4")
                {
                    liQC.Style.Add("display", "block");
                    liEnumeration.Style.Add("display", "block");
                    liInterUser.Style.Add("display", "block");
                    liFeeder.Style.Add("display", "block");
                    if (sUserId == "7")
                    {
                        liEnumStage.Style.Add("display", "block");
                    }
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void rdbEnglish_CheckedChanged(object sender, EventArgs e)
        {
            rdbKannada.Checked = false;
            rdbEnglish.Checked = true;
            Session["Lang"] = "English";
            Response.Redirect("~/Dashboard.aspx", false);
        }

        protected void rdbKannada_CheckedChanged(object sender, EventArgs e)
        {
            rdbEnglish.Checked = false;
            rdbKannada.Checked = true;
            Session["Lang"] = "Kannada";
            Response.Redirect("~/Dashboard.aspx", false);
        }
    }
}