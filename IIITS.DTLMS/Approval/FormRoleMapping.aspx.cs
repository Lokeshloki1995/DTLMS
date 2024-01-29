using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Collections;

namespace IIITS.DTLMS.Approval
{
    public partial class FormRoleMapping : System.Web.UI.Page
    {
       
        string strFormCode = "FormRoleMapping.aspx";
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
                    objSession = (clsSession)Session["clsSession"];
                    lblMessage.Text = string.Empty;
                    AdminAccess();
                    if (!IsPostBack)
                    {
                        Genaral.Load_Combo("select \"MO_ID\",\"MO_NAME\" from \"TBLMODULES\"  ORDER BY \"MO_ID\"", "-Select--", cmbModules);
                        Genaral.Load_Combo("select \"RO_ID\",\"RO_NAME\" from \"TBLROLES\" ORDER BY \"RO_ID\"", "--Select--", cmbRoles);
                        btnSave.Visible = false;
                        btnReset.Visible = false;
                                           }
                }
                
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }


        public void LoadModuleDetails(string strSelectedValue)
        {
            clsRoleMapping objModule = new clsRoleMapping();
            try
            {

                objModule.sModuleId = strSelectedValue;
                DataTable dtDetails = objModule.LoadAllModuleDetails(objModule);
                grdAccessRights.DataSource = dtDetails;
                grdAccessRights.DataBind();

                if (dtDetails.Rows.Count > 0)
                {
                   
                    btnSave.Visible = true;
                    btnReset.Visible = true;
                    lblMsg.Visible = false;
                    ViewState["Modules"] = dtDetails;
                }
                else
                {
                  
                   
                    lblMsg.Visible = true;
                    btnSave.Visible = false;
                    btnReset.Visible = false;

                }

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }


        protected void cmbModules_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbModules.SelectedIndex==0)
                {
                    grdAccessRights.Visible = false;
                    btnSave.Visible = false;
                    btnReset.Visible = false;
                    lblMsg.Visible = false;
                }
                else
                {
                    LoadModuleDetails(cmbModules.SelectedValue);
                    if (cmbRoles.SelectedIndex > 0)
                    {
                        cmbRoles_SelectedIndexChanged(sender, e);
                    }
                    grdAccessRights.Visible = true;
                   
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }


        }


        protected void grdAccessRights_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
              
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    CheckBoxList chklstAccess = (CheckBoxList)e.Row.FindControl("chklstAccess");
                    Genaral.Load_CheckboxList("MD_NAME", "MD_ID", "select \"MD_ID\",\"MD_NAME\" from \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='AT' ORDER BY \"MD_ORDER_BY\" ", chklstAccess);
                  
               
                }
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }


        protected void chklstAccess_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string strPreviousValue = string.Empty;
            string[] strNewValue=new string[100];
            try
            {
                CheckBoxList chkList = (CheckBoxList)sender;
                GridViewRow row = (GridViewRow)chkList.NamingContainer;

                CheckBoxList chkRow = (row.FindControl("chklstAccess") as CheckBoxList);

                foreach (ListItem oItem in chkRow.Items)
                {
                    //To make all ckeckbox disable when "ALL" is checked
                    if (chkRow.SelectedValue == "1")
                    {
                        if (oItem.Value != "1")
                        {
                            oItem.Enabled = false;
                            oItem.Selected = false;
                        }
                    }
                    //To make all ckeckbox enable when "ALL" is unchecked
                    else if (chkRow.SelectedValue != "1" )
                    {
                        oItem.Enabled = true;
                    }
                    //To make all the ckeckboxlist disable when "Read Only" is checked
                    if (chkRow.SelectedValue == "4")
                    {
                        if (oItem.Value == "1")
                        {
                            oItem.Enabled = false;
                            oItem.Selected = false;
                        }
                        if (oItem.Value == "2")
                        {
                            oItem.Enabled = false;
                            oItem.Selected = false;
                        }
                        if (oItem.Value == "3")
                        {
                            oItem.Enabled = false;
                            oItem.Selected = false;
                        }
                    }

                    if (chkRow.SelectedValue == "2" || chkRow.SelectedValue == "3")
                    {
                        foreach (ListItem oItemNew in chkRow.Items)
                        {
                            if (oItemNew.Value == "4")
                            {
                                oItemNew.Enabled = false;
                                oItemNew.Selected = true;
                                //return;

                            }
                        }
                        //break;

                    }

                    //To make the "ALL" and "READ ONLY" disable When create and Modify is Checked.
                    if (chkRow.SelectedValue == "2" && oItem.Value == "3" || chkRow.SelectedValue == "3" && oItem.Value == "2")
                    {
                        if (oItem.Selected == true)
                        {
                            if (ViewState["PREVIOUSVALUE"] != null)
                            {

                                foreach (ListItem oItemNew in chkRow.Items)
                                {

                                    if (oItemNew.Value == "1")
                                    {
                                        oItemNew.Enabled = false;
                                    }
                                    if (oItemNew.Value == "4")
                                    {
                                        oItemNew.Enabled = false;
                                        ViewState["PREVIOUSVALUE"] = null;
                                        return;

                                    }
                                }
                            }
                        }
                        if (oItem.Value == "2" || oItem.Value == "3")
                        {

                            ViewState["PREVIOUSVALUE"] = chkRow.SelectedValue;
                            //foreach (ListItem oItemNew in chkRow.Items)
                            //{
                            //    if (oItemNew.Value == "4")
                            //    {
                            //        oItemNew.Enabled = false;
                            //        oItemNew.Selected = false;
                            //        return;

                            //    }
                            //}
                            break;

                        }

                    }

                  

                }

                
          }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }




        protected void btnSave_Click(object sender, EventArgs e)
        {
            clsRoleMapping objModule = new clsRoleMapping();
            try
            {

               
                string[] Arr = new string[2];
                string strValue = string.Empty;
                bool ischeked = false;
                objModule.sModuleId = cmbModules.SelectedValue;
                objModule.sRoleId = cmbRoles.SelectedValue;
                objModule.sMappingId = txtMappingId.Text;
               
                objModule.sCrby = objSession.UserId;
                string[] strAccessIds = new string[grdAccessRights.Rows.Count];
                int i = 0;
                foreach (GridViewRow row in grdAccessRights.Rows)
                {

                    CheckBoxList chkRow = (row.FindControl("chklstAccess") as CheckBoxList);
                    if (chkRow.SelectedValue != "")
                    {

                        foreach (ListItem oItem in chkRow.Items)
                        {
                            if (oItem.Selected)
                            {
                                strValue += Convert.ToString(oItem.Value) + ";";

                            }
                        }

                        if (strValue.StartsWith(";") == false)
                        {
                            strValue = ";" + strValue;
                        }
                        if (strValue.EndsWith(";") == false)
                        {
                            strValue = strValue + ";";
                        }

                        string strBoId = ((Label)row.FindControl("lblBoId")).Text.Trim();

                        strAccessIds[i] = strBoId + "" + strValue;
                        i++;
                        strValue = string.Empty;
                        ischeked = true;

                    }

                }
                if (ischeked == false)
                {
                    if (btnSave.Text == "Save")
                    {
                        ShowMsgBox("Select any of the Access Rights if required");
                        btnSave.Text = "Save";
                        return;
                    }

                }

                Arr = objModule.SaveAccessRights(objModule, strAccessIds);
                if (Convert.ToString(Arr[1]) == "0")
                {
                    ShowMsgBox("Saved Successfully");
                    txtMappingId.Text = objModule.sMappingId;
                    btnSave.Text = "Update";
                    return;
                }


                if (Convert.ToString(Arr[1]) == "1")
                {
                    ShowMsgBox("Updated Successfully");
                    return;
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
         

        public DataTable LoadRoleDetails(string strSelectedValue, string sBOId)
        {
            clsRoleMapping objModule = new clsRoleMapping();
            DataTable dt = new DataTable();
            try
            {
                
                objModule.sRoleId = strSelectedValue;
                objModule.sBusinessobjId = sBOId;
                dt = objModule.LoadAllRoleDetails(objModule);
                return dt;  

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }

        }


        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                
                cmbRoles.SelectedIndex = 0;
                foreach (GridViewRow row in grdAccessRights.Rows)
                {
                    CheckBoxList chkRow = (row.FindControl("chklstAccess") as CheckBoxList);
                    {
                        foreach (ListItem oItem in chkRow.Items)
                        {
                            oItem.Selected = false;
                            oItem.Enabled = true;
                            btnSave.Text = "Save";

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }


        protected void cmbRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (cmbModules.SelectedIndex == 0)
                {
                    ShowMsgBox("Select Module Name");
                    cmbRoles.SelectedIndex = 0;
                    return;
                }
                DataTable dt = new DataTable();
                //DataTable dtnew = new DataTable();

                foreach (GridViewRow row in grdAccessRights.Rows)
                {
                    CheckBoxList chkRow = (row.FindControl("chklstAccess") as CheckBoxList);
                    {
                        foreach (ListItem oItem in chkRow.Items)
                        {
                            oItem.Selected = false;
                            btnSave.Text = "Save";

                        }
                    }
                }

                foreach (GridViewRow row in grdAccessRights.Rows)
                {
                    CheckBoxList chkRow = (row.FindControl("chklstAccess") as CheckBoxList);
                    Label lblBoId = (Label)row.FindControl("lblBoId");
                

                    dt = LoadRoleDetails(cmbRoles.SelectedValue, lblBoId.Text);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        foreach (ListItem oItem in chkRow.Items)
                        {
                            if (oItem.Value == Convert.ToString(dt.Rows[i]["UR_ACCESSTYPE"]))
                            {
                                if (oItem.Selected == false)
                                {
                                    oItem.Selected = true;
                                    btnSave.Text = "Update";
                                    chklstAccess_OnSelectedIndexChanged(chkRow, e);
                                }
                                else
                                {
                                    oItem.Selected = false;
                                }

                            }
                          
   
                        }
                        //ViewState["CheckedItems"] = dt;

                    }
                   
                }
                //grdAccessRights.AllowPaging = true;
                //grdAccessRights.DataBind();
                
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
                if (objSession.RoleId != "11")
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

        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "FormRoleMapping";
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

    }
}