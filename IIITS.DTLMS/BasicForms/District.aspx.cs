using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Collections;

namespace IIITS.DTLMS.BasicForms
{
    public partial class District : System.Web.UI.Page
    {
        string strFormCode = "CircleView.aspx";
        clsSession objSession;
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
                    Update.Visible = false;
                    UpdateDistrict.Visible = false;
                    Form.DefaultButton = cmdSave.UniqueID;
                    objSession = (clsSession)Session["clsSession"];
                    if (!IsPostBack)
                    {
                        GenerateDistrictCode();
                       // Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_CODE\" || '-' || \"DIV_NAME\" FROM \"TBLDIVISION\" ORDER BY \"DIV_CODE\"", "-Select-", cmbDivsionName);

                        if (Request.QueryString["DistrictId"] != null && Convert.ToString(Request.QueryString["DistrictId"]) != "")
                        {
                            CheckAccessRights("4");
                            txtDistId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["DistrictId"]));
                            GetDistrictDetails(txtDistId.Text);
    
                            Create.Visible = false;
                            CreateDistrict.Visible = false;

                            Update.Visible = true;
                            UpdateDistrict.Visible = true;
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
               
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
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
                if (txtDistrictCode.Text.Trim().Length == 0 )
                {
                    ShowMsgBox("Please Enter District Code");
                    txtDistrictCode.Focus();
                    return;
                }
                if (txtDistrictName.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Please Enter District Name");
                    txtDistrictName.Focus();
                    return;
                }
                if (txtDistrictName.Text.Trim().StartsWith("."))
                {
                    ShowMsgBox("Please Enter valid District Name");
                    txtDistrictName.Focus();
                    return;
                }



                if (txtOfficeCode.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Please select the Division Name");
                    txtOfficeCode.Focus();
                    return;
                }

                clsDistrict objDis = new clsDistrict();
                string[] Arr = new string[2];
                 
                objDis.sDistId = txtDistId.Text;
                objDis.sDistrictCode = txtDistrictCode.Text;
                objDis.sButtonname = cmdSave.Text;
                objDis.sOfficeCode = txtOfficeCode.Text;
                String name = txtDistrictName.Text.Replace("'","''");
                
                objDis.sDistrictName = name;

                Arr = objDis.SaveDetails(objDis);

                if (objSession.sTransactionLog == "1")
                {
                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, "District Master ");
                }
                if (Arr[1].ToString() == "1"|| Arr[1].ToString() == "4")
                {
                    ShowMsgBox(Arr[0].ToString());
                    return;
                }

                    if (Arr[1].ToString() == "0")
                {
                    txtDistId.Text = objDis.sDistId;
                    cmdSave.Text = "Update";
                    ShowMsgBox(Arr[0].ToString());
                    txtDistrictCode.Text = string.Empty;
                    txtDistrictName.Text = string.Empty;
                    txtDistId.Text = string.Empty;
                    txtDistrictCode.Enabled = true;
                    cmdSave.Text = "Save";
                    txtDistrictCode.ReadOnly = false;
                    GenerateDistrictCode();
                   
                }
                else
                {
                    //ShowMsgBox(Arr[0].ToString());
                    //txtDistrictCode.Text = string.Empty;
                    //txtDistrictName.Text = string.Empty;
                    //txtDistId.Text = string.Empty;
                    //txtDistrictCode.Enabled = true;
                    //cmdSave.Text = "Save";
                    //txtDistrictCode.ReadOnly = false;
                    //GenerateDistrictCode();
                    string DistrictID = HttpUtility.UrlEncode(Genaral.UrlEncrypt(objDis.sDistId));
                    Response.Redirect("District.aspx?DistrictId=" + DistrictID + "", false);
                }
               
            }
            catch (Exception ex)
            {
               
                lblMessage.Text = clsException.ErrorMsg();
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
        protected void grdLoadDiv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                grdLoadDiv.PageIndex = 0;
                grdLoadDiv.PageIndex = e.NewPageIndex;
                dt = (DataTable)ViewState["ITEMS"];
                grdLoadDiv.DataSource = dt;
                grdLoadDiv.DataBind();
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
                lblMessage.Text = clsException.ErrorMsg();
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

        protected void cmdClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtDistrictCode.Text = string.Empty;
                txtDistrictName.Text = string.Empty;
                txtDistId.Text = string.Empty;
                txtDistrictCode.Enabled = true;
                cmdSave.Text = "Save";
                txtDistrictCode.ReadOnly = false;
                txtOfficeCode.Text = string.Empty;
                GenerateDistrictCode();
                grdLoadDiv.Visible = false;
                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void GetDistrictDetails(string strDistrictId)
        {
            try
            {
                ArrayList arrOffCode = new ArrayList();
                ArrayList arrOffCodeValue = new ArrayList();

                clsDistrict objDistrict = new clsDistrict();
                DataTable dtDetails = new DataTable();
                objDistrict.sDistId = Convert.ToString(strDistrictId);
              
                objDistrict.GetDistDetails(objDistrict);
                txtDistrictCode.Text = Convert.ToString(objDistrict.sDistrictCode);
                txtDistrictName.Text = Convert.ToString(objDistrict.sDistrictName);
                txtDistId.Text = objDistrict.sDistId;
                txtOfficeCode.Text = objDistrict.sOfficeCode;
                if (txtOfficeCode.Text != "" && txtOfficeCode.Text != null)
                {

                    if (txtOfficeCode.Text.StartsWith(";"))
                    {
                        txtOfficeCode.Text = txtOfficeCode.Text.Substring(1, txtOfficeCode.Text.Length - 1);
                    }
                    if (txtOfficeCode.Text.EndsWith(";"))
                    {
                        txtOfficeCode.Text = txtOfficeCode.Text.Substring(0, txtOfficeCode.Text.Length - 1);
                    }
                    txtOfficeCode.Text = txtOfficeCode.Text.Replace(";", ",");

                    arrOffCode.AddRange(txtOfficeCode.Text.Split(','));


                    for (int i = 0; i < arrOffCode.Count; i++)
                    {
                        arrOffCodeValue.Add(Convert.ToInt32(arrOffCode[i]));
                    }
                    

                    LoadOffice();
                    PopulateCheckedValues();
                }
                dtDetails = objDistrict.sDt;
                ViewState["ITEMS"] = dtDetails;
                grdLoadDiv.DataSource = dtDetails;
                grdLoadDiv.DataBind();

                txtDistrictCode.Enabled = false;
                cmdSave.Text = "Update";


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.mdlPopup.Show();
                //  DivmdlPoppup.Style.Add("display","block");
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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void LoadOffice(string sOfficeCode = "", string sOffName = "")
        {
            try
            {
                DataTable dtPageDetaiils = new DataTable();
                clsDistrict ObjDist = new clsDistrict();
                ObjDist.sOfficeCode = sOfficeCode;
                ObjDist.sOfficeName = sOffName;
                dtPageDetaiils = ObjDist.LoadOfficeDet(ObjDist);
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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
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


        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "District";
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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;

            }
        }

        #endregion

        public void GenerateDistrictCode()
        {
            try
            {
                clsDistrict objDistCode = new clsDistrict();

                txtDistrictCode.Text = objDistCode.GenerateDistrictCode();
                txtDistrictCode.ReadOnly = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }



       
    }
}