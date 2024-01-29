using IIITS.DTLMS.BL;
using IIITS.DTLMS.BL.POFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.POFlow
{
    public partial class LECCreate : System.Web.UI.Page
    {
        clsSession objSession;
        /// <summary>
        /// This Function used for page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    CheckAccessRights();
                    LecRegisterCalendarExtender.EndDate = System.DateTime.Now;

                    if (Request.QueryString["LECId"] != null && Request.QueryString["LECId"].ToString() != "")
                    {
                        hdnlecid.Value = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["LECId"]));
                    }
                    if(hdnlecid.Value.Length>0)
                    {
                        GetLecmasterdetails();
                    }

                    txtLecRegisterdate.Attributes.Add("readonly", "readonly");
                    txtLecValidupto.Attributes.Add("readonly", "readonly");
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// This function used to check access rights
        /// </summary>
        /// <param name="sAccessType"></param>
        /// <returns></returns>
        public bool CheckAccessRights()
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "LECView";
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sAccessType = Constants.CheckAccessRights.CheckAccessRightsAll + "," 
                    + Constants.CheckAccessRights.CheckAccessRightsCreate
                    + "," + Constants.CheckAccessRights.CheckAccessRightsModify;
                bool bResult = objApproval.CheckAccessRights(objApproval);
                if (bResult == false)
                {
                        Response.Redirect("~/UserRestrict.aspx", false);
                }
                return bResult;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;

            }
        }
        /// <summary>
        /// This function used for save the lec master details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                ClsLec ObjLecCreate = new ClsLec();
                string[] Arr = new string[3];
                if (ValidateForm() == true)
                {
                    ObjLecCreate.LecId = hdnlecid.Value;
                    ObjLecCreate.LecContractorName = txtContractorname.Text;
                    ObjLecCreate.LecLicenceNumber = txtLicenceNumber.Text;
                    ObjLecCreate.Lecregistereddate = txtLecRegisterdate.Text;
                    ObjLecCreate.Lecvalidupto = txtLecValidupto.Text;
                    ObjLecCreate.LecGstnumber = txtGstnumber.Text;
                    ObjLecCreate.LecContactnumber = txtContactnumber.Text;
                    ObjLecCreate.Lecemail = txtEmailId.Text;
                    ObjLecCreate.LecAddress = txtAddress.Text;
                    ObjLecCreate.Crby = objSession.UserId;

                    Arr = ObjLecCreate.SaveLECMaster(ObjLecCreate);

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "LEC Master");
                    }

                    if (Arr[1].ToString() == "0"|| Arr[1].ToString() == "1")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", 
                            "alert('"+ Arr[0] + "');window.location ='LECView.aspx';", true);

                        ShowMsgBox(Arr[0]);
                        cmdSave.Enabled = false;
                        return;
                    }
                    else if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0]);
                        return;
                    }
                    else
                    {
                        ShowMsgBox("Something Went wrong");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// This function used for validations
        /// </summary>
        /// <returns></returns>
        public bool ValidateForm()
        {
            bool bValidate = false;
            try
            {
                if (txtContractorname.Text.Length == 0)
                {
                    txtContractorname.Focus();
                    ShowMsgBox("Please Enter Name of the contractor/firm");
                    return bValidate;
                }
                if (txtLicenceNumber.Text.Length == 0)
                {
                    txtLicenceNumber.Focus();
                    ShowMsgBox("Please Enter Licence Number");
                    return bValidate;
                }
                if (txtLecRegisterdate.Text.Length == 0)
                {
                    txtLecRegisterdate.Focus();
                    ShowMsgBox("Please Select Licence Registration date");
                    return bValidate;
                }
                if (txtLecValidupto.Text.Length == 0)
                {
                    txtLecValidupto.Focus();
                    ShowMsgBox("Please Select Licence Valid Upto");
                    return bValidate;
                }

                string sResultt = Genaral.DateComparision(txtLecValidupto.Text, txtLecRegisterdate.Text, false, false);
                if (sResultt == "2")
                {
                    ShowMsgBox("Licence Valid Upto Should be Greater Than Or Equal To Licence Registration Date");
                    return bValidate;
                }
                if (txtGstnumber.Text.Length == 0)
                {
                    txtGstnumber.Focus();
                    ShowMsgBox("Please Enter GST Number");
                    return bValidate;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(
                    txtGstnumber.Text,"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$"))
                {
                    txtEmailId.Focus();
                    ShowMsgBox("Please Enter Valid GST Number");
                    return bValidate;
                }

                if (txtContactnumber.Text.Trim().Length == 0)
                {
                    txtContactnumber.Focus();
                    ShowMsgBox("Please Enter Contact Number");
                    return bValidate;
                }
                if (txtContactnumber.Text.Trim('0').Length == 0)
                {
                    txtContactnumber.Focus();
                    ShowMsgBox("Please Enter Valid Contact Number");
                    return bValidate;
                }
                if (txtContactnumber.Text[0] == '0' || txtContactnumber.Text[0] == '1' || txtContactnumber.Text[0] == '2')
                {
                    ShowMsgBox("Please Enter Valid Mobile Number");
                    txtContactnumber.Focus();
                    return false;
                }
                if (txtContactnumber.Text.Length < 10)
                {
                    ShowMsgBox("Please Enter Valid Contact Number");
                    txtContactnumber.Focus();
                    return bValidate;
                }

                if (txtEmailId.Text.Trim() == "")
                {
                    txtEmailId.Focus();
                    ShowMsgBox("Please Enter Email Id.");
                    return bValidate;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmailId.Text,
                    "^\\s*[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+[a-zA-Z0-9]\\s*$"))
                {
                    txtEmailId.Focus();
                    ShowMsgBox("Please Enter Valid Email (xyz@aaa.com)");
                    return bValidate;
                }

                if (txtAddress.Text.Trim() == "")
                {
                    txtAddress.Focus();
                    ShowMsgBox("Please Enter Office Address");
                    return bValidate;
                }

                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return bValidate;
            }
        }
        /// <summary>
        /// This function used display the pop up messages
        /// </summary>
        /// <param name="Msg"></param>
        private void ShowMsgBox(string Msg)
        {
            try
            {
                string ShowMsg = string.Empty;
                ShowMsg = "<script language=javascript> alert ('" + Msg + "')</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "Msg", ShowMsg);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// This function will call while click on reset button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        /// <summary>
        /// This function used to reset the fields data
        /// </summary>
        public void Reset()
        {
            objSession = (clsSession)Session["clsSession"];
            try
            {
                txtContractorname.Text = string.Empty;
                txtContactnumber.Text = string.Empty;
                txtGstnumber.Text = string.Empty;
                txtAddress.Text = string.Empty;
                txtEmailId.Text = string.Empty;
                txtLecValidupto.Text = string.Empty;
                txtLicenceNumber.Text = string.Empty;
                if (hdnlecid.Value.Length == 0)
                {
                    txtLecRegisterdate.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// This function used to call the GetLecDetailstoupdate method and fetched data will be bind to text fileds.
        /// </summary>
        public void GetLecmasterdetails()
        {
            try
            {
                ClsLec objgetlec = new ClsLec();

                objgetlec.LecId = hdnlecid.Value;

                objgetlec.GetLecDetailstoupdate(objgetlec);

                txtContractorname.Text = objgetlec.LecContractorName;
                txtLicenceNumber.Text = objgetlec.LecLicenceNumber;
                txtGstnumber.Text = objgetlec.LecGstnumber;
                txtLecRegisterdate.Text = objgetlec.Lecregistereddate;
                txtLecValidupto.Text = objgetlec.Lecvalidupto;
                txtContactnumber.Text = objgetlec.LecContactnumber;
                txtEmailId.Text = objgetlec.Lecemail;
                txtAddress.Text = objgetlec.LecAddress;
                cmdSave.Text = Constants.ButtonClass.ButtenUpdate;
                txtLecRegisterdate.Enabled = false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }
    }
}
