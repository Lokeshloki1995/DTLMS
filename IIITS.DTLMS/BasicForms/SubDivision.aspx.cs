using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Text.RegularExpressions;
using System.Configuration;

namespace IIITS.DTLMS.BasicForms
{
    public partial class SubDivision : System.Web.UI.Page
    {        
        string strFormCode = "SubDivision";
        string tempDepName = string.Empty;
        string strUserLogged = string.Empty;
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
                    Form.DefaultButton = cmdSave.UniqueID;
                    objSession = (clsSession)Session["clsSession"];
                    Update.Visible = false;
                    UpdateSubDivision.Visible = false;
                    if (!IsPostBack)
                    {

                        Genaral.Load_Combo("select \"CM_CIRCLE_CODE\", \"CM_CIRCLE_NAME\" from \"TBLCIRCLE\" ORDER BY \"CM_CIRCLE_CODE\"", "--Select--", cmbCircle);
                        if (Request.QueryString["SubDivId"] != null && Convert.ToString(Request.QueryString["SubDivId"]) != "")
                        {
                            CheckAccessRights("4");
                            txtSubDivId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["SubDivId"]));
                            LoadSubDivOffDet(txtSubDivId.Text);
                            Create.Visible = false;
                            CreateSubDivision.Visible = false;

                            Update.Visible = true;
                            UpdateSubDivision.Visible = true;
                        }

                    }
                }
                
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
           
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {

                //Check AccessRights
                bool bAccResult;
                if (cmdSave.Text == "Update")
                {
                    bAccResult = CheckAccessRights("3");
                }
                else
                {
                    bAccResult = CheckAccessRights("2");
                }

                if (bAccResult == false)
                {
                    return;
                }


                string[] Arrmsg = new string[2];
                strUserLogged = objSession.UserId;
                if (txtName.Text == "" || txtName.Text.Trim().StartsWith(".") ||txtOfficeHead.Text.Trim().StartsWith(".")|| txtMobileNo.Text == "" || txtOfficeHead.Text == "" || txtSubDivCode.Text == "" || cmbDivision.SelectedIndex <= 0 || txtEmail.Text == "")
                {
                   ShowMsgBox("Enter All The Details Correctly");
                    return;
                }



                if (txtSubDivCode.Text.Trim().Length != 4)
                {
                    txtSubDivCode.Focus();
                    ShowMsgBox("SubdivCode must be of length 4");
                    return;
                }

                if (Convert.ToString(cmbCircle.SelectedValue) != txtSubDivCode.Text.Substring(0, Circle_code))
                {
                    cmbCircle.Focus();
                    ShowMsgBox("Circle Code and Sub Division Code Does not Match");
                    return;
                }

                if (Convert.ToString(cmbDivision.SelectedValue) != txtSubDivCode.Text.Substring(0,Division_code))
                {
                    txtSubDivCode.Focus();
                   ShowMsgBox("Division Code and Sub Division Code Code Does not Match");
                    return;
                }
             
                if (txtMobileNo.Text.Length != 10)
                {
                    txtMobileNo.Focus();
                   ShowMsgBox("Mobile No Should Be 10 Digits");
                    return;
                }

                if (txtMobileNo.Text[0] == '0' || txtMobileNo.Text[0] == '1' || txtMobileNo.Text[0] == '2')
                {
                    ShowMsgBox("Please Enter Valid Mobile No");
                    txtMobileNo.Focus();
                    return;
                }

                if (txtPhoneNo.Text != "")
                {
                    //if (txtPhoneNo.Text.Length < 12)
                    //{
                    //    txtPhoneNo.Focus();
                    //    ShowMsgBox("Enter Valid Phone Number");
                    //    return;
                    //}
                    if ((txtPhoneNo.Text.Length - txtPhoneNo.Text.Replace("-", "").Length) >= 2)
                    {
                        txtPhoneNo.Focus();
                        ShowMsgBox("Please Enter Valid Phone Number");
                        return;
                    }
                    if (txtPhoneNo.Text.Contains(".") == true)
                    {
                        txtPhoneNo.Focus();
                        ShowMsgBox("Please Enter Valid Phone Number");
                        return;
                    }

                    if (txtPhoneNo.Text.Replace("0", "").Length == 0)
                    {
                        txtPhoneNo.Focus();

                        ShowMsgBox("Please Enter Valid Phone Number");

                        return;
                    }
                }
                //if (!System.Text.RegularExpressions.Regex.IsMatch(txtName.Text, "^\\s*[a-zA-Z0-9]+\\s*[a-zA-Z0-9_.+-]\\s*$"))
                //{
                //    ShowMsgBox("Please enter Valid SubDivision Name ");
                //    txtName.Focus();
                //    return;
                //}
                //if (!System.Text.RegularExpressions.Regex.IsMatch(txtOfficeHead.Text, "^\\s*[a-zA-Z0-9]+\\s*[a-zA-Z0-9_.+-]\\s*$"))
                //{
                //    ShowMsgBox("Please enter Valid SubDivision Head Name ");
                //    txtOfficeHead.Focus();
                //    return;
                //}
                if (txtEmail.Text.Trim() == "")
                {
                    txtEmail.Focus();
                    ShowMsgBox("Please Enter Email Id");
                    return;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text, "^\\s*[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+[a-zA-Z0-9]\\s*$"))
                {
                    txtEmail.Focus();
                    ShowMsgBox("Please enter Valid Email (xyz@aaa.com)");
                    return ;
                }

                bool isSave = true;
                if (cmdSave.Text.Equals("Update"))
                {
                    isSave = false;
                }
                clsSubDiv ObjSubDivOffice = new clsSubDiv();
                Arrmsg = ObjSubDivOffice.SaveUpdateSubDivisionDetails(txtSubDivId.Text , Convert.ToString(cmbDivision.SelectedValue), 
                    txtSubDivCode.Text.Trim().ToUpper(), txtName.Text.Trim().ToUpper(), txtOfficeHead.Text.Trim().ToUpper().Replace("'",""),
                    txtMobileNo.Text.Trim(), txtPhoneNo.Text.Trim(), txtEmail.Text.Trim(), isSave, strUserLogged, txtaddress.Text.Trim().ToUpper());

                if (objSession.sTransactionLog == "1")
                {
                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " SubDivision Master ");
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
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return;
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

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        public  void LoadSubDivOffDet( string strSubDivisionId)
        {
            try
            {
                clsSubDiv ObjSubDivOffice = new clsSubDiv();               

                DataTable DtCircleOffDet = ObjSubDivOffice.LoadSubDivOffDet(Convert.ToString(strSubDivisionId));

                txtSubDivCode.Text = Convert.ToString(DtCircleOffDet.Rows[0]["SD_SUBDIV_CODE"]);

                cmbCircle.Enabled = false;
                cmbDivision.Enabled = false;
                txtSubDivCode.Enabled = false;

                txtName.Text = Convert.ToString(DtCircleOffDet.Rows[0]["SD_SUBDIV_NAME"]);
                txtPhoneNo.Text = Convert.ToString(DtCircleOffDet.Rows[0]["SD_PHONE"]);
                txtEmail.Text = Convert.ToString(DtCircleOffDet.Rows[0]["SD_EMAIL"]);
                txtOfficeHead.Text = Convert.ToString(DtCircleOffDet.Rows[0]["SD_HEAD_EMP"]);
                txtMobileNo.Text = Convert.ToString(DtCircleOffDet.Rows[0]["SD_MOBILE"]);
                Genaral.Load_Combo("select \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" from \"TBLCIRCLE\" ORDER BY \"CM_CIRCLE_CODE\"", "--Select--", cmbCircle);
                cmbCircle.SelectedValue = Convert.ToString(DtCircleOffDet.Rows[0]["SD_SUBDIV_CODE"]).Substring(0, Circle_code);
                Genaral.Load_Combo("select \"DIV_CODE\",\"DIV_NAME\",\"DIV_CICLE_CODE\" FROM \"TBLDIVISION\" where \"DIV_CICLE_CODE\"='" + cmbCircle.SelectedValue + "' ORDER BY \"DIV_CODE\"", "--Select--", cmbDivision);
                cmbDivision.SelectedValue = Convert.ToString(DtCircleOffDet.Rows[0]["SD_SUBDIV_CODE"]).Substring(0, Division_code);
                txtaddress.Text = Convert.ToString(DtCircleOffDet.Rows[0]["SD_ADDRESS"]);

                cmdSave.Text = "Update";
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }




        }

        /// <summary>
        /// protected void imbBtnDelete_Click(object sender, ImageClickEventArgs e)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbBtnDelete_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton imgDel = (ImageButton)sender;
            GridViewRow rw = (GridViewRow)imgDel.NamingContainer;
        }

        


        /// <summary>
        /// Reset Fn
        /// </summary>
        private void Reset()
        {
            try
            {
                //txtSubDivId.Text = string.Empty;
                //txtName.Text = string.Empty;
                //txtPhoneNo.Text = string.Empty;
                //txtEmail.Text = string.Empty;
                //txtOfficeHead.Text = string.Empty;
                //txtMobileNo.Text = string.Empty;
                //cmbCircle.SelectedIndex = 0;
                //txtSubDivCode.Text = string.Empty;
                //cmbDivision.SelectedIndex = 0;
                //cmbCircle.Enabled = true;
                //cmbDivision.Enabled = true;
                //txtSubDivCode.Enabled = true;
                //cmdSave.Text = "Save";
                //txtSubDivCode.ReadOnly = false;
                Response.Redirect("SubDivision.aspx", false);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

       

        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Genaral.Load_Combo("select \"DIV_CODE\",\"DIV_CODE\" || '-' || \"DIV_NAME\" FROM \"TBLDIVISION\"  WHERE CAST(\"DIV_CICLE_CODE\" AS TEXT) LIKE  '" + cmbCircle.SelectedValue + "%'ORDER BY \"DIV_CODE\"", "--Select--", cmbDivision);
            }
            catch (Exception ex)
            {
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

                objApproval.sFormName = "SubDivision";
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
                //lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;

            }
        }

        #endregion

        protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                clsSubDiv objSubdiv = new clsSubDiv();
                objSubdiv.sDivCode = cmbDivision.SelectedValue;
               txtSubDivCode.Text = objSubdiv.GenerateSubDivCode(objSubdiv);
               txtSubDivCode.ReadOnly = true;
            }
            catch (Exception ex)
            {

                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}