using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Data.OleDb;

namespace IIITS.DTLMS.Approval
{
    public partial class ApprovalPriority : System.Web.UI.Page
    {
        string strFormCode = "ApprovalPriority";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                objSession = (clsSession)Session["clsSession"];
                
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                else
                {
                    lblMessage.Text = string.Empty;
                    if (!IsPostBack)
                    {
                        AdminAccess();
                        Genaral.Load_Combo("SELECT \"MO_ID\",\"MO_NAME\" FROM \"TBLMODULES\"  WHERE \"MO_ID\"<> '11' ORDER BY \"MO_ID\"", "--Select--", cmbModule);

                    }
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
                string sShowMsg = string.Empty;
                sShowMsg = "<script language=javascript> alert ('" + sMsg + "')</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "Msg", sShowMsg);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmdAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbRoles.SelectedIndex < 0)
                {
                    ShowMsgBox("Please select Role");
                    return;
                }

                AddRolesToGrid();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }
        public void AddRolesToGrid()
        {
            try
            {
                clsApprovalPriority objApproval = new clsApprovalPriority();
                //To Check whether Roles Added or Not
                if (ViewState["Roles"] != null)
                {
                    DataTable dtRoles = (DataTable)ViewState["Roles"];
                    DataRow drow;

                    for (int i = 0; i < dtRoles.Rows.Count; i++)
                    {
                        if (cmbRoles.SelectedItem.Text == Convert.ToString(dtRoles.Rows[i]["RO_NAME"]) && cmbForm.SelectedItem.Text == Convert.ToString(dtRoles.Rows[i]["BO_NAME"]))
                        {
                            ShowMsgBox("Roles Already Added");
                            return;
                        }
                    }
                    if (dtRoles.Rows.Count > 0)
                    {

                        drow = dtRoles.NewRow();
                        drow["BO_ID"] = cmbForm.SelectedValue;
                        drow["RO_ID"] = cmbRoles.SelectedValue;
                        drow["BO_NAME"] = cmbForm.SelectedItem.Text;
                        drow["RO_NAME"] = cmbRoles.SelectedItem.Text;
                        drow["WM_LEVEL"] = dtRoles.Rows.Count + 1;
                        dtRoles.Rows.Add(drow);
                        grdRoles.DataSource = dtRoles;
                        grdRoles.DataBind();
                        //txtTcCode.Text = string.Empty;
                        ViewState["Roles"] = dtRoles;
                    }
                }

                else
                {
                    DataTable dtRoles = new DataTable();
                    DataRow drow;
                    dtRoles.Columns.Add(new DataColumn("BO_ID"));
                    dtRoles.Columns.Add(new DataColumn("RO_ID"));
                    dtRoles.Columns.Add(new DataColumn("BO_NAME"));
                    dtRoles.Columns.Add(new DataColumn("RO_NAME"));
                    dtRoles.Columns.Add(new DataColumn("WM_LEVEL"));
                    drow = dtRoles.NewRow();
                    drow["BO_ID"] = cmbForm.SelectedValue;
                    drow["RO_ID"] = cmbRoles.SelectedValue;
                    drow["BO_NAME"] = cmbForm.SelectedItem.Text;
                    drow["RO_NAME"] = cmbRoles.SelectedItem.Text;
                    //dtRoles.Rows.Add(drow);
                    drow["WM_LEVEL"] = "1";
                    dtRoles.Rows.Add(drow);
                    grdRoles.DataSource = dtRoles;
                    grdRoles.DataBind();
                    //txtTcCode.Text = string.Empty;
                    ViewState["Roles"] = dtRoles;
                    grdRoles.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(cmbRoles.SelectedIndex <= 0)
                {
                    ShowMsgBox("Select Role Name");
                    return;
                }
                SaveRoles();

                if (ViewState["Roles"] != null)
                {
                   
                }
                else
                {
                    ShowMsgBox("Add Roles and then Proceed");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void SaveRoles()
        {
            clsApprovalPriority objApproval = new clsApprovalPriority();
            DataTable dtTCDetails;
            try
            {
                string[] Arr = new string[3];
                dtTCDetails = (DataTable)ViewState["Roles"];
                objApproval.dtRoles = dtTCDetails;
                objApproval.sCrBy = objSession.UserId;
                objApproval.sModuleId = txtModules.Text;
                //if (txtSiId.Text != "")
                //{
                //    objTransfer.sSiId = txtSiId.Text;
                //}
                Arr = objApproval.SaveRoles(objApproval);
                if (Convert.ToString(Arr[1]) == "0")
                {
                    //txtSiId.Text = objTransfer.sSiId;
                    btnSave.Text = "Update";
                    txtModules.Text = objApproval.sModuleId;
                    ShowMsgBox(Arr[0]);
                    return;
                }
                else
                {
                    ShowMsgBox(Arr[0]);
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadRoles(DataTable dt)
        {
            try
            {
                //dt = objTcTransfer.LoadCapacity();
                grdRoles.DataSource = dt;
                grdRoles.DataBind();
                grdRoles.Visible = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdRoles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            clsApprovalPriority objApproval = new clsApprovalPriority();
            try
            {
                if (e.CommandName == "Remove")
                {
                    DataTable dt = (DataTable)ViewState["Roles"];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                        Label lblRoleName = (Label)row.FindControl("lblRoleName");
                        //to remove Selected RoleName from Grid
                        if (lblRoleName.Text == Convert.ToString(dt.Rows[i]["RO_NAME"]))
                        {
                            dt.Rows[i].Delete();
                            dt.AcceptChanges();
                        }
                    }

                    dt.AcceptChanges();
                    if (dt.Rows.Count > 0)
                    {
                        int counter = 0;

                        foreach (DataRow row in dt.Rows)
                        {
                            counter++;
                            row["WM_LEVEL"] = counter;

                        }
                        ViewState["Roles"] = dt;

                    }
                    else
                    {
                        ViewState["Roles"] = null;
                    }
                    LoadRoles(dt);
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }



        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                //dt = objTcTransfer.LoadCapacity();
                cmbModule.SelectedIndex = 0;
                if(cmbForm.SelectedIndex > 0)
                {
                    cmbForm.SelectedItem.Text = "--Select--";   
                }
                if(cmbRoles.SelectedIndex > 0)
                {
                    cmbRoles.SelectedItem.Text = "--Select";
                }
                
                lblMessage.Text = string.Empty;
                lblRoles.Text = string.Empty;
                grdRoles.Visible = false;
                cmbForm.Enabled = false;
                cmbRoles.Enabled = false;
                ViewState["Roles"] = null;
                txtModules.Text = string.Empty;
                btnSave.Text = "Save";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdRoles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                clsApprovalPriority objApproval = new clsApprovalPriority();
                DataTable dtSavedRoles;
                grdRoles.PageIndex = e.NewPageIndex;
                dtSavedRoles = (DataTable)ViewState["Roles"];
                grdRoles.DataSource = dtSavedRoles;
                grdRoles.DataBind();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }



        protected void cmbModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbForm.Enabled = true;
                Genaral.Load_Combo("SELECT \"BO_ID\",\"BO_NAME\" FROM \"TBLBUSINESSOBJECT\" where \"BO_MO_ID\" ='" + cmbModule.SelectedValue + "' ORDER BY \"BO_ID\"", "--Select--", cmbForm);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbForm_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strRoleNames = string.Empty;
                string strNewRoleNames = string.Empty;
              
                clsApprovalPriority objApproval = new clsApprovalPriority();
                DataTable dtSavedRoles;
                cmbRoles.Enabled = true;

                //string strQry = "SELECT DISTINCT \"RO_ID\", \"RO_NAME\" FROM \"TBLROLES\",\"TBLUSERROLEMAPPING\",\"TBLBUSINESSOBJECT\" WHERE \"RO_ID\"=\"UR_ROLEID\" AND ";
                //strQry += "  \"UR_BOID\"=\"BO_ID\" AND \"BO_ID\"='" + cmbForm.SelectedValue + "' AND \"UR_ACCESSTYPE\" IN (1,2,3) ORDER BY \"RO_ID\"";

                string strQry = "SELECT DISTINCT \"RO_ID\", \"RO_NAME\" FROM \"TBLROLES\"";

                Genaral.Load_Combo(strQry, "--Select--", cmbRoles);

                DataTable dtRoles;

                if (ViewState["Roles"] == null)
                {
                    objApproval.sBOId = cmbForm.SelectedValue;
                    dtRoles = objApproval.GetRoleNames(objApproval);
                    if (dtRoles.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtRoles.Rows.Count; i++)
                        {
                            lblRoles.Visible = true;
                            strRoleNames += Convert.ToString(dtRoles.Rows[i]["RO_NAME"]) + ",";
                            lblRoles.Text = "Creator:" + strRoleNames.Substring(0, strRoleNames.Length - 1);
                        }
                    }
                    else
                    {
                        lblRoles.Visible = false;
                    }
                }
                else
                {
                    //ShowMsgBox("Please Save The Details For the Selected Module and Then Proceed...");
                }
                
                dtSavedRoles = objApproval.LoadSavedRoles(cmbForm.SelectedValue);
                if (dtSavedRoles.Rows.Count > 0)
                {
                    ViewState["Roles"] = dtSavedRoles;
                    txtModules.Text = cmbForm.SelectedValue;
                    grdRoles.DataSource = dtSavedRoles;
                    grdRoles.DataBind();
                    grdRoles.Visible = true;
                    btnSave.Text = "Update";
                }
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

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

    }
}
        
    
