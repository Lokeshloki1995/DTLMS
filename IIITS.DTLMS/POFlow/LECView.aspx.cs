using IIITS.DTLMS.BL;
using IIITS.DTLMS.BL.POFlow;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.POFlow
{
    public partial class LECView : System.Web.UI.Page
    {
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

                if (CheckAccessRights(Constants.CheckAccessRights.CheckAccessRightsAll))
                {
                    if (!IsPostBack)
                    {
                        CheckUpdateLECStatus();
                        hidStatus.Value = "";
                        string Status = hidStatus.Value;
                        LoadLECDetails(Status);
                    }
                }
                else
                {
                    Response.Redirect("~/UserRestrict.aspx", false);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }

        public void CheckUpdateLECStatus()
        {
            ClsLec Obj = new ClsLec();
            try
            {
                Obj.CheckUpdateLECStatus();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }
        /// <summary>
        /// Loads Gid data.
        /// </summary>
        public void LoadLECDetails(string Status)
        {
            ClsLec Obj = new ClsLec();
            try
            {
                Obj.Status = Status;
                Obj = Obj.GetLECDetails(Obj);

                if (Obj.DtLECDetails.Rows.Count > 0)
                {
                    grdLEC.DataSource = Obj.DtLECDetails;
                    grdLEC.DataBind();
                }
                else
                {
                    ShowEmptyGrid();
                }
                ViewState["LECDetails"] = Obj.DtLECDetails;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }
        /// <summary>
        /// Row Data Bound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdLEC_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblStatus;
                    lblStatus = (Label)e.Row.FindControl("lblStatus");
                    ImageButton imgBtnEdit;
                    ImageButton imgDeActive;
                    ImageButton imgActive;
                    imgBtnEdit = (ImageButton)e.Row.FindControl("imgBtnEdit");

                    if (lblStatus.Text == "A")
                    {
                        imgActive = (ImageButton)e.Row.FindControl("imgActive");
                        imgDeActive = (ImageButton)e.Row.FindControl("imgDeActive");
                        imgActive.Visible = true;
                        imgDeActive.Visible = false;
                        imgBtnEdit.Enabled = true;
                        imgBtnEdit.ToolTip = "";
                    }
                    else
                    {
                        imgDeActive = (ImageButton)e.Row.FindControl("imgDeActive");
                        imgActive = (ImageButton)e.Row.FindControl("imgActive");
                        imgDeActive.Visible = true;
                        imgActive.Visible = false;
                        imgBtnEdit.Enabled = true;
                        imgBtnEdit.ToolTip = "";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }
        /// <summary>
        /// RowCommand
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdLEC_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "create")
                {
                    #region Refrence
                    //if (CheckAccessRights(Constants.CheckAccessRights.CheckAccessRightsModify))
                    //{
                    //    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    //    string LECId = ((Label)row.FindControl("lblLM_ID")).Text;
                    //    LECId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(LECId));
                    //    Response.Redirect("LECCreate.aspx?LECId=" + LECId + "", false);                                               
                    //}else
                    //{
                    //    ShowMsgBox("Sorry, you are not authorized to access.");
                    //}        
                    #endregion
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    string LECId = ((Label)row.FindControl("lblLM_ID")).Text;
                    LECId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(LECId));
                    Response.Redirect("LECCreate.aspx?LECId=" + LECId + "", false);
                }
                if (e.CommandName == "status")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    string LECId = ((Label)row.FindControl("lblLM_ID")).Text;
                    string sStatus = ((Label)row.FindControl("lblStatus")).Text;

                    ImageButton imgDeactive = new ImageButton();
                    ImageButton imgActive = new ImageButton();
                    imgDeactive = (ImageButton)row.FindControl("imgDeactive");
                    imgActive = (ImageButton)row.FindControl("imgActive");

                    ClsLec Obj = new ClsLec();
                    Obj.LecId = LECId;
                    Obj.Status = sStatus;
                    Obj.Upby = objSession.UserId;

                    string[] Arr = new string[2];
                    Arr = Obj.ChecKLECRecord(Obj);
                    if (Arr[0] == "0")
                    {
                        ShowMsgBox(Arr[1]);
                    }
                    else
                    {
                        ShowMsgBox(Arr[1]);
                    }

                    // Reloading the Grid as default value.
                    string Status = "";
                    LoadLECDetails(Status);
                    cmdActiveLEC.BackColor = System.Drawing.Color.Empty;
                    cmdInActiveLEC.BackColor = System.Drawing.Color.Empty;
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }

        /// <summary>
        /// ShowEmptyGrid
        /// </summary>
        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("LM_ID");
                dt.Columns.Add("LM_CONTRACTOR_NAME");
                dt.Columns.Add("LM_EMAIL_ID");
                dt.Columns.Add("LM_NUMBER");
                dt.Columns.Add("LM_REGISTRATION_DATE");

                dt.Columns.Add("LM_VALID_UPTO");
                dt.Columns.Add("LM_GST_NUMBER");
                dt.Columns.Add("LM_CONTACT_NUMBER");
                dt.Columns.Add("LM_CR_BY");
                dt.Columns.Add("LM_UPDATED_ON");
                dt.Columns.Add("LM_ADDRESS");
                dt.Columns.Add("Status");

                grdLEC.DataSource = dt;
                grdLEC.DataBind();

                int iColCount = grdLEC.Rows[0].Cells.Count;
                grdLEC.Rows[0].Cells.Clear();
                grdLEC.Rows[0].Cells.Add(new TableCell());
                grdLEC.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdLEC.Rows[0].Cells[0].Text = Convert.ToString(ConfigurationManager.AppSettings["EmptyData"]);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }
        /// <summary>
        /// CheckAccessRights
        /// </summary>
        /// <param name="sAccessType"></param>
        /// <returns></returns>
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY
                clsApproval objApproval = new clsApproval();
                objApproval.sFormName = MethodBase.GetCurrentMethod().DeclaringType.Name;
                objApproval.sRoleId = objSession.RoleId; // for Admin there is no role.
                objApproval.sAccessType = Constants.CheckAccessRights.CheckAccessRightsAll + "," + sAccessType; //  "1"
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
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdActiveLEC_click(object sender, EventArgs e)
        {
            ClsLec Obj = new ClsLec();
            try
            {
                cmdInActiveLEC.BackColor = System.Drawing.Color.Empty;
                hidStatus.Value = "A";
                Obj.Status = hidStatus.Value;
                Obj = Obj.GetLECDetails(Obj);
                if (Obj.DtLECDetails.Rows.Count > 0)
                {
                    grdLEC.DataSource = Obj.DtLECDetails;
                    grdLEC.DataBind();
                }
                else
                {
                    ShowEmptyGrid();
                }
                ViewState["LECDetails"] = Obj.DtLECDetails;
                cmdActiveLEC.BackColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdInActiveLEC_click(object sender, EventArgs e)
        {
            ClsLec Obj = new ClsLec();
            try
            {
                cmdActiveLEC.BackColor = System.Drawing.Color.Empty;
                hidStatus.Value = "D";
                Obj.Status = hidStatus.Value;
                Obj = Obj.GetLECDetails(Obj);
                if (Obj.DtLECDetails.Rows.Count > 0)
                {
                    grdLEC.DataSource = Obj.DtLECDetails;
                    grdLEC.DataBind();

                }
                else
                {
                    ShowEmptyGrid();
                }
                ViewState["LECDetails"] = Obj.DtLECDetails;
                cmdInActiveLEC.BackColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ExportLEC_click(object sender, EventArgs e)
        {
            ClsLec Obj = new ClsLec();
            //dtLECDetails = (DataTable)ViewState["LECDetails"];
            Obj.Status = hidStatus.Value;
            Obj = Obj.GetLECDetails(Obj);
            DataTable dtLECDetails = new DataTable();
            dtLECDetails = Obj.DtLECDetails;
            if (dtLECDetails.Rows.Count > 0)
            {
                #region adding Slno and changing A to Active and D to In Active logic
                int Slno = 0;
                dtLECDetails.Columns.Add("SlNo", typeof(int));

                // Iterate through the rows in the DataTable
                foreach (DataRow row in dtLECDetails.Rows)
                {
                    #region adding Slno.
                    Slno = Slno + 1;
                    row["SlNo"] = Slno;
                    #endregion

                    #region Changing from A to Active for Status field.
                    if (Convert.ToString(row["Status"]) == "A")
                    {
                        row["Status"] = "Active";
                    }
                    else if (Convert.ToString(row["Status"]) == "D")
                    {
                        row["Status"] = "In-Active";
                    }
                    #endregion
                }
                #endregion
            
                dtLECDetails.Columns["LM_CONTRACTOR_NAME"].ColumnName = "Contractor/Firm Name";
                dtLECDetails.Columns["LM_EMAIL_ID"].ColumnName = "Email Id";
                dtLECDetails.Columns["LM_NUMBER"].ColumnName = "Licence Number";
                dtLECDetails.Columns["LM_REGISTRATION_DATE"].ColumnName = "Registration Date";
                dtLECDetails.Columns["LM_VALID_UPTO"].ColumnName = "Expiring Date";
                dtLECDetails.Columns["LM_GST_NUMBER"].ColumnName = "GST Number";
                dtLECDetails.Columns["LM_CONTACT_NUMBER"].ColumnName = "Contact Number";
                dtLECDetails.Columns["LM_CR_BY"].ColumnName = "Created By";
                dtLECDetails.Columns["LM_UPDATED_ON"].ColumnName = "Updated On";
                dtLECDetails.Columns["LM_ADDRESS"].ColumnName = "Address";

                dtLECDetails.Columns["SlNo"].SetOrdinal(0);
                dtLECDetails.Columns["Contractor/Firm Name"].SetOrdinal(1);
                dtLECDetails.Columns["Email Id"].SetOrdinal(2);
                dtLECDetails.Columns["Licence Number"].SetOrdinal(3);
                dtLECDetails.Columns["Registration Date"].SetOrdinal(4);
                dtLECDetails.Columns["Expiring Date"].SetOrdinal(5);
                dtLECDetails.Columns["GST Number"].SetOrdinal(6);
                dtLECDetails.Columns["Contact Number"].SetOrdinal(7);
                dtLECDetails.Columns["Created By"].SetOrdinal(8);
                dtLECDetails.Columns["Updated On"].SetOrdinal(9);
                dtLECDetails.Columns["Address"].SetOrdinal(10);

                List<string> listtoRemove = new List<string> { "LM_ID" };
                string filename = "LECDetails" + DateTime.Now + ".xls";
                string pagetitle = "LEC Details";

                Genaral.getexcel(dtLECDetails, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowEmptyGrid();
                ShowMsgBox(Convert.ToString(ConfigurationManager.AppSettings["EmptyData"]));
            }
        }
        /// <summary>
        /// Redirects to the LECCreate Form on Checking Access Rights.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CreNewLEC_click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("LECCreate.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }
        /// <summary>
        /// ShowMsgBox
        /// </summary>
        /// <param name="sMsg"></param>
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
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }
    }
}