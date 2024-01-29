using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IIITS.DTLMS.BL;
using System.IO;
using ClosedXML.Excel;
using System.Configuration;
using System.Reflection;

namespace IIITS.DTLMS.MasterForms
{
    public partial class UserView : System.Web.UI.Page
    {
        //string strFormCode = "UserView";
        clsSession objSession;
        int Zone_code = Convert.ToInt32(ConfigurationManager.AppSettings["Zone_code"]);
        int Circle_code = Convert.ToInt32(ConfigurationManager.AppSettings["Circle_code"]);
        int Division_code = Convert.ToInt32(ConfigurationManager.AppSettings["Division_code"]);
        int SubDiv_code = Convert.ToInt32(ConfigurationManager.AppSettings["SubDiv_code"]);
        int Section_code = Convert.ToInt32(ConfigurationManager.AppSettings["Section_code"]);
        int Feeder_code = Convert.ToInt32(ConfigurationManager.AppSettings["feeder_code"]);
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
                lblMsg.Text = string.Empty;
                txtEffectFrom.Attributes.Add("readonly", "readonly");
                CalendarExtender1.EndDate = System.DateTime.Now;
                if (!IsPostBack)
                {
                    string Orginal_OffCode = string.Empty;
                    string DevLevelOffCode = string.Empty;
                    //CheckAccessRights("4");                   
                    bool AccessResult = CheckAccessRights(Constants.CheckAccessRights.CheckAccessRightsReadOnly);
                    if (AccessResult == true)
                    {
                        if (objSession != null) // logic for the Loacation based loading.
                        {
                            if ((objSession.OfficeCode ?? "").Length == 0)
                            {
                                Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", cmbZone);
                                Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" ", cmbCircle);
                                Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" ", cmbDivision);
                                Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" ", cmbsubdivision);
                                Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" ", cmbSection);
                                cmbCircle.Enabled = false;
                                cmbDivision.Enabled = false;
                                cmbsubdivision.Enabled = false;
                                cmbSection.Enabled = false;
                                txtSessionoffcode.Text = Constants.Roles.AdminLevel;
                            }
                            else
                            {
                                Orginal_OffCode = objSession.OfficeCode;
                                LoadDropDownOnOffCode(Orginal_OffCode); // Loads the Dropdown Based on Location
                               txtSessionoffcode.Text = Constants.Roles.OfficeLevel;
                               // txtSessionoffcode.Text = Constants.Roles.AdminLevel;
                                cmbSection.Enabled = false;
                            }
                        }
                        LoadUserDetails(Orginal_OffCode); // loads the Grid.
                        //cmdActiveUser.BackColor = System.Drawing.Color.Gray; // diffult load grid button is set to Gray.
                        //cmdActiveUser.Visible = false;
                        //cmdInActiveUser.Visible = false;
                    }
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
        private void LoadDropDownOnOffCode(string Orginal_OffCode = "")
        {
            string Extracted_OffCode = string.Empty;
            try
            {
                if ((Orginal_OffCode ?? "").Length == 0) // for Zone and Admin level Users
                {
                    Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", cmbZone);
                }
                if (Orginal_OffCode.Length >= 1) // for Zone
                {
                    Extracted_OffCode = "";
                    Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", cmbZone);
                    Extracted_OffCode = Orginal_OffCode.Substring(0, Zone_code);
                    cmbZone.Items.FindByValue(Extracted_OffCode).Selected = true;
                    cmbZone.Enabled = false;
                }
                if (Orginal_OffCode.Length >= 1)    // for Circle
                {
                    Extracted_OffCode = "";
                    Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_ZO_ID\"='"
                        + cmbZone.SelectedValue + "'", cmbCircle);
                    if (Orginal_OffCode.Length >= 2)
                    {
                        Extracted_OffCode = Orginal_OffCode.Substring(0, Circle_code);
                        cmbCircle.Items.FindByValue(Extracted_OffCode).Selected = true;
                        cmbCircle.Enabled = false;
                    }
                }
                if (Orginal_OffCode.Length >= 2)    // for Division
                {
                    Extracted_OffCode = "";
                    Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CICLE_CODE\" AS TEXT)='"
                        + cmbCircle.SelectedValue + "'", cmbDivision);
                    if (Orginal_OffCode.Length >= 3)
                    {
                        Extracted_OffCode = Orginal_OffCode.Substring(0, Division_code);
                        cmbDivision.Items.FindByValue(Extracted_OffCode).Selected = true;
                        cmbDivision.Enabled = false;
                    }
                }
                if (Orginal_OffCode.Length >= 3)    // for SubDivision
                {
                    Extracted_OffCode = "";
                    Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE CAST(\"SD_DIV_CODE\" AS TEXT)='"
                        + cmbDivision.SelectedValue + "'", cmbsubdivision);
                    if (Orginal_OffCode.Length >= 4)
                    {
                        Extracted_OffCode = Orginal_OffCode.Substring(0, SubDiv_code);
                        cmbsubdivision.Items.FindByValue(Extracted_OffCode).Selected = true;
                        cmbsubdivision.Enabled = false;
                    }
                    //else
                    //{
                    //    Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_SUBDIV_CODE\"='"
                    //   + cmbsubdivision.SelectedValue + "'", cmbSection);
                    //}
                }
                if (Orginal_OffCode.Length >= 4)    // for Section
                {
                    Extracted_OffCode = "";
                    Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_SUBDIV_CODE\"='"
                        + cmbsubdivision.SelectedValue + "'", cmbSection);
                    if (Orginal_OffCode.Length >= 5)
                    {
                        Extracted_OffCode = Orginal_OffCode.Substring(0, Section_code);
                        cmbSection.Items.FindByValue(Extracted_OffCode).Selected = true;
                        cmbSection.Enabled = false;
                    }
                }
                else
                {
                    Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" ", cmbSection);
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
        /// <summary>
        /// cmdLoad_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            cmdActiveUser.BackColor = System.Drawing.Color.Empty;
            cmdInActiveUser.BackColor = System.Drawing.Color.Empty;
            clsUser Obj = new clsUser();
            string Status = string.Empty;
            try
            {
                Status = txtlastDropdownSelected.Text;
                Obj.zone = hdcmbZone.Text;
                Obj.circle = hdcmbCircle.Text;
                Obj.division = hdcmbDivision.Text;
                Obj.subdivision = hdcmbSubDivision.Text;
                Obj.section = hdcmbSection.Text;

                switch (Status)
                {
                    case "1":
                        Obj.sOffCode = Obj.zone;
                        Obj.FetchStore = true;
                        break;
                    case "2":
                        Obj.sOffCode = Obj.circle;
                        Obj.FetchStore = true;
                        break;
                    case "3":
                        Obj.sOffCode = Obj.division;
                        Obj.FetchStore = true;
                        break;
                    case "4":
                        Obj.sOffCode = Obj.subdivision;
                        Obj.FetchStore = false;
                        break;
                    case "5":
                        Obj.sOffCode = Obj.section;
                        Obj.FetchStore = false;
                        break;
                }
                //Obj.ActiveUser = "0"; // 1 means Deactive User
                DataTable DtUserGrid = Obj.GetUserGridDetails(Obj);     // calling the Entity class to get the data for the Database
                if (DtUserGrid.Rows.Count > 0)
                {
                    grdUser.DataSource = DtUserGrid;
                    grdUser.DataBind();
                    //ViewState["USER"] = DtUserGrid;
                }
                else
                {
                    ShowEmptyGrid();
                    //ViewState["USER"] = DtUserGrid;
                }
                ViewState["USER"] = DtUserGrid;
                //cmdActiveUser.Visible = true;
                //cmdInActiveUser.Visible = true;
                //cmdActiveUser.BackColor = System.Drawing.Color.Gray; // diffult load grid button is set to Gray.

                cmbSection.Enabled = false;
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
        #region
        //protected void cmbZone_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    clsUser objUser = new clsUser();
        //    DataTable dtUserDetails = new DataTable();
        //    try
        //    {
        //        if (cmbZone.SelectedIndex > 0)
        //        {
        //            hidSelectedLoaction.Value = "";
        //            Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_ZO_ID\"='" 
        //                + cmbZone.SelectedValue + "'", "--Select--", cmbCircle);
        //            //clsUser objUser = new clsUser();
        //            //DataTable dtUserDetails = new DataTable();
        //            objUser.sOffCode = cmbZone.SelectedValue;
        //            dtUserDetails = objUser.LoadUserGrid(objUser);
        //            if (dtUserDetails.Rows.Count == 0)
        //            {
        //                ShowEmptyGrid();
        //                //ViewState["USER"] = dtUserDetails;
        //            }
        //            else
        //            {
        //                grdUser.DataSource = dtUserDetails;
        //                grdUser.DataBind();
        //                //ViewState["USER"] = dtUserDetails;
        //            }
        //            hidSelectedLoaction.Value = objUser.sOffCode;
        //        }
        //        else
        //        {
        //            //clsUser objUser = new clsUser();
        //            //DataTable dtUserDetails = new DataTable();
        //            objUser.sOffCode = cmbZone.SelectedValue;
        //            dtUserDetails = objUser.LoadUserGrid(objUser);
        //            if (dtUserDetails.Rows.Count == 0)
        //            {
        //                ShowEmptyGrid();
        //                //ViewState["USER"] = dtUserDetails;
        //            }
        //            else
        //            {
        //                grdUser.DataSource = dtUserDetails;
        //                grdUser.DataBind();
        //                //ViewState["USER"] = dtUserDetails;
        //            }
        //            cmbCircle.Items.Clear();
        //            cmbDivision.Items.Clear();
        //            cmbsubdivision.Items.Clear();
        //            cmbSection.Items.Clear();
        //        }
        //        ViewState["USER"] = dtUserDetails;
        //        cmdInActiveUser.BackColor = System.Drawing.Color.Empty;
        //        cmdActiveUser.BackColor = System.Drawing.Color.Gray;
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(
        //            MethodBase.GetCurrentMethod().DeclaringType.Name,
        //            MethodBase.GetCurrentMethod().Name,
        //            ex.Message,
        //            ex.StackTrace);
        //    }
        //}      
        //protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    clsUser objUser = new clsUser();
        //    DataTable dtUserDetails = new DataTable();
        //    try
        //    {
        //        if (cmbCircle.SelectedIndex > 0)
        //        {
        //            hidSelectedLoaction.Value = "";
        //            Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\"  WHERE \"DIV_CICLE_CODE\"='" 
        //                + cmbCircle.SelectedValue + "' ORDER BY \"DIV_CODE\"", "--Select--", cmbDivision);
        //            //clsUser objUser = new clsUser();
        //            //DataTable dtUserDetails = new DataTable();
        //            objUser.sOffCode = cmbCircle.SelectedValue;
        //            dtUserDetails = objUser.LoadUserGrid(objUser);
        //            if (dtUserDetails.Rows.Count == 0)
        //            {
        //                ShowEmptyGrid();
        //                //ViewState["USER"] = dtUserDetails;
        //            }
        //            else
        //            {
        //                grdUser.DataSource = dtUserDetails;
        //                grdUser.DataBind();
        //                //ViewState["USER"] = dtUserDetails;
        //            }
        //            hidSelectedLoaction.Value = objUser.sOffCode;
        //        }
        //        else
        //        {
        //            //clsUser objUser = new clsUser();
        //            //DataTable dtUserDetails = new DataTable();
        //            objUser.sOffCode = cmbCircle.SelectedValue;
        //            dtUserDetails = objUser.LoadUserGrid(objUser);
        //            if (dtUserDetails.Rows.Count == 0)
        //            {
        //                ShowEmptyGrid();
        //                //ViewState["USER"] = dtUserDetails;
        //            }
        //            else
        //            {
        //                grdUser.DataSource = dtUserDetails;
        //                grdUser.DataBind();
        //                //ViewState["USER"] = dtUserDetails;
        //            }
        //            cmbDivision.Items.Clear();
        //            cmbsubdivision.Items.Clear();
        //            cmbSection.Items.Clear();
        //        }
        //        ViewState["USER"] = dtUserDetails;
        //        cmdInActiveUser.BackColor = System.Drawing.Color.Empty;
        //        cmdActiveUser.BackColor = System.Drawing.Color.Gray;
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(
        //            MethodBase.GetCurrentMethod().DeclaringType.Name,
        //            MethodBase.GetCurrentMethod().Name,
        //            ex.Message,
        //            ex.StackTrace);
        //    }
        //}        
        //protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    clsUser objUser = new clsUser();
        //    DataTable dtUserDetails = new DataTable();
        //    try
        //    {
        //        if (cmbDivision.SelectedIndex > 0)
        //        {
        //            hidSelectedLoaction.Value = "";
        //            Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\", \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\"  WHERE \"SD_DIV_CODE\"='" 
        //                + cmbDivision.SelectedValue + "' ORDER BY \"SD_SUBDIV_CODE\"", "--Select--", cmbsubdivision);
        //            //clsUser objUser = new clsUser();
        //            //DataTable dtUserDetails = new DataTable();
        //            objUser.sOffCode = cmbDivision.SelectedValue;
        //            dtUserDetails = objUser.LoadUserGrid(objUser);
        //            if (dtUserDetails.Rows.Count == 0)
        //            {
        //                ShowEmptyGrid();
        //                //ViewState["USER"] = dtUserDetails;
        //            }
        //            else
        //            {
        //                grdUser.DataSource = dtUserDetails;
        //                grdUser.DataBind();
        //                //ViewState["USER"] = dtUserDetails;
        //            }
        //            hidSelectedLoaction.Value = objUser.sOffCode;
        //        }
        //        else
        //        {
        //            //clsUser objUser = new clsUser();
        //            //DataTable dtUserDetails = new DataTable();
        //            objUser.sOffCode = cmbCircle.SelectedValue;
        //            dtUserDetails = objUser.LoadUserGrid(objUser);
        //            if (dtUserDetails.Rows.Count == 0)
        //            {
        //                ShowEmptyGrid();
        //                //ViewState["USER"] = dtUserDetails;
        //            }
        //            else
        //            {
        //                grdUser.DataSource = dtUserDetails;
        //                grdUser.DataBind();
        //                //ViewState["USER"] = dtUserDetails;
        //            }
        //            cmbsubdivision.Items.Clear();
        //            cmbSection.Items.Clear();
        //        }
        //        ViewState["USER"] = dtUserDetails;
        //        cmdInActiveUser.BackColor = System.Drawing.Color.Empty;
        //        cmdActiveUser.BackColor = System.Drawing.Color.Gray;
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(
        //            MethodBase.GetCurrentMethod().DeclaringType.Name,
        //            MethodBase.GetCurrentMethod().Name,
        //            ex.Message,
        //            ex.StackTrace);
        //    }
        //}
        //protected void cmbsubdivision_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    clsUser objUser = new clsUser();
        //    DataTable dtUserDetails = new DataTable();
        //    try
        //    {
        //        if (cmbsubdivision.SelectedIndex > 0)
        //        {
        //            hidSelectedLoaction.Value = "";
        //            Genaral.Load_Combo("SELECT \"OM_CODE\", \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_SUBDIV_CODE\" = '" 
        //                + cmbsubdivision.SelectedValue + "' ORDER BY \"OM_CODE\"", "--Select--", cmbSection);
        //            //clsUser objUser = new clsUser();
        //            //DataTable dtUserDetails = new DataTable();
        //            objUser.sOffCode = cmbsubdivision.SelectedValue;
        //            dtUserDetails = objUser.LoadUserGrid(objUser);
        //            if (dtUserDetails.Rows.Count == 0)
        //            {
        //                ShowEmptyGrid();
        //                //ViewState["USER"] = dtUserDetails;
        //            }
        //            else
        //            {
        //                grdUser.DataSource = dtUserDetails;
        //                grdUser.DataBind();
        //                //ViewState["USER"] = dtUserDetails;
        //            }
        //            hidSelectedLoaction.Value = objUser.sOffCode;
        //        }
        //        else
        //        {
        //            //clsUser objUser = new clsUser();
        //            //DataTable dtUserDetails = new DataTable();
        //            objUser.sOffCode = cmbDivision.SelectedValue;
        //            dtUserDetails = objUser.LoadUserGrid(objUser);
        //            if (dtUserDetails.Rows.Count == 0)
        //            {
        //                ShowEmptyGrid();
        //                //ViewState["USER"] = dtUserDetails;
        //            }
        //            else
        //            {
        //                grdUser.DataSource = dtUserDetails;
        //                grdUser.DataBind();
        //                //ViewState["USER"] = dtUserDetails;
        //            }
        //            cmbSection.Items.Clear();
        //        }
        //        ViewState["USER"] = dtUserDetails;
        //        cmdInActiveUser.BackColor = System.Drawing.Color.Empty;
        //        cmdActiveUser.BackColor = System.Drawing.Color.Gray;
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(
        //            MethodBase.GetCurrentMethod().DeclaringType.Name,
        //            MethodBase.GetCurrentMethod().Name,
        //            ex.Message,
        //            ex.StackTrace);
        //    }
        //}       
        //protected void cmbSection_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    clsUser objUser = new clsUser();
        //    DataTable dtUserDetails = new DataTable();
        //    try
        //    {
        //        if (cmbSection.SelectedIndex > 0)
        //        {
        //            hidSelectedLoaction.Value = "";                   
        //            objUser.sOffCode = cmbSection.SelectedValue;
        //            dtUserDetails = objUser.LoadUserGrid(objUser);
        //            if (dtUserDetails.Rows.Count == 0)
        //            {
        //                ShowEmptyGrid();
        //                //ViewState["USER"] = dtUserDetails;
        //            }
        //            else
        //            {
        //                grdUser.DataSource = dtUserDetails;
        //                grdUser.DataBind();
        //                //ViewState["USER"] = dtUserDetails;
        //            }
        //            hidSelectedLoaction.Value = objUser.sOffCode;
        //        }
        //        else
        //        {
        //            //clsUser objUser = new clsUser();
        //            //DataTable dtUserDetails = new DataTable();
        //            objUser.sOffCode = cmbsubdivision.SelectedValue;
        //            dtUserDetails = objUser.LoadUserGrid(objUser);
        //            if (dtUserDetails.Rows.Count == 0)
        //            {
        //                ShowEmptyGrid();
        //                //ViewState["USER"] = dtUserDetails;
        //            }
        //            else
        //            {
        //                grdUser.DataSource = dtUserDetails;
        //                grdUser.DataBind();
        //                //ViewState["USER"] = dtUserDetails;
        //            }

        //        }
        //        ViewState["USER"] = dtUserDetails;
        //        cmdInActiveUser.BackColor = System.Drawing.Color.Empty;
        //        cmdActiveUser.BackColor = System.Drawing.Color.Gray;
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(
        //            MethodBase.GetCurrentMethod().DeclaringType.Name,
        //            MethodBase.GetCurrentMethod().Name,
        //            ex.Message,
        //            ex.StackTrace);
        //    }
        //}
        #endregion
        /// <summary>
        /// LoadUserDetails
        /// </summary>
        public void LoadUserDetails(string OfficCode = "")
        {
            clsUser objUser = new clsUser();
            try
            {
                if ((OfficCode ?? "").Length > 0)
                {
                    objUser.sOffCode = OfficCode;
                    if ((OfficCode ?? "").Length == 3)
                    {
                        objUser.FetchStore = true;
                    }
                }
                DataTable dtUserDetails = new DataTable();
                //dtUserDetails = objUser.LoadUserGrid(objUser); // old call
                dtUserDetails = objUser.GetUserGridDetails(objUser);
                if (dtUserDetails.Rows.Count == 0)
                {
                    ShowEmptyGrid();
                    //ViewState["USER"] = dtUserDetails;
                }
                else
                {
                    grdUser.DataSource = dtUserDetails;
                    grdUser.DataBind();
                    //ViewState["USER"] = dtUserDetails;
                }
                ViewState["USER"] = dtUserDetails;
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
                dt.Columns.Add("zone");
                dt.Columns.Add("circle");
                dt.Columns.Add("division");
                dt.Columns.Add("subdivision");
                dt.Columns.Add("section");

                dt.Columns.Add("US_ID");
                dt.Columns.Add("US_FULL_NAME");
                dt.Columns.Add("US_EMAIL");
                dt.Columns.Add("US_LG_NAME");
                dt.Columns.Add("US_MOBILE_NO");
                dt.Columns.Add("RO_NAME");
                dt.Columns.Add("US_DESG_ID");
                dt.Columns.Add("OFF_NAME");
                dt.Columns.Add("US_STATUS1");

                grdUser.DataSource = dt;
                grdUser.DataBind();

                int iColCount = grdUser.Rows[0].Cells.Count;
                grdUser.Rows[0].Cells.Clear();
                grdUser.Rows[0].Cells.Add(new TableCell());
                grdUser.Rows[0].Cells[0].ColumnSpan = iColCount;
                //grdUser.Rows[0].Cells[0].Text = "No Records Found";
                grdUser.Rows[0].Cells[0].Text = Convert.ToString(ConfigurationManager.AppSettings["EmptyData"]);
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
        /// cmdNew_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdNew_Click(object sender, EventArgs e)
        {
            try
            {
                //Check AccessRights
                //bool bAccResult = CheckAccessRights("2");
                bool bAccResult = CheckAccessRights(Constants.CheckAccessRights.CheckAccessRightsCreate);
                if (bAccResult == false)
                {
                    return;
                }
                Response.Redirect("UserCreate.aspx", false);
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
        /// <summary>
        /// grdUser_PageIndexChanging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdUser.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["USER"];
                grdUser.DataSource = SortDataTable(dt as DataTable, true);
                grdUser.DataBind();
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
        /// SortDataTable
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="isPageIndexChanging"></param>
        /// <returns></returns>
        protected DataView SortDataTable(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        ViewState["USER"] = dataView.ToTable();
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["USER"] = dataView.ToTable();
                    }
                }
                return dataView;
            }
            else
            {
                return new DataView();
            }
        }
        /// <summary>
        /// GridViewSortDirection
        /// </summary>
        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "ASC"; }
            set { ViewState["SortDirection"] = value; }
        }
        /// <summary>
        /// GridViewSortExpression
        /// </summary>
        private string GridViewSortExpression
        {
            get { return ViewState["SortExpression"] as string ?? string.Empty; }
            set { ViewState["SortExpression"] = value; }
        }
        /// <summary>
        /// GetSortDirection
        /// </summary>
        /// <returns></returns>
        private string GetSortDirection()
        {
            switch (GridViewSortDirection)
            {
                case "ASC":
                    GridViewSortDirection = "DESC";

                    break;
                case "DESC":
                    GridViewSortDirection = "ASC";

                    break;
            }
            return GridViewSortDirection;
        }
        /// <summary>
        /// grdUser_Sorting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdUser_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdUser.PageIndex;
            DataTable dt = (DataTable)ViewState["USER"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdUser.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdUser.DataSource = dt;
            }
            grdUser.DataBind();
            grdUser.PageIndex = pageIndex;
        }
        /// <summary>
        /// grdUser_RowCommand
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdUser_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "create")
                {
                    //Check AccessRights
                    //bool bAccResult = CheckAccessRights("3");
                    bool bAccResult = CheckAccessRights(Constants.CheckAccessRights.CheckAccessRightsModify);
                    if (bAccResult == false)
                    {
                        return;
                    }
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    string strUserId = ((Label)row.FindControl("lblUserId")).Text;
                    strUserId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(strUserId));
                    Response.Redirect("UserCreate.aspx?QryUserId=" + strUserId + "", false);
                }
                if (e.CommandName == "status")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    string sUserId = ((Label)row.FindControl("lblUserId")).Text;
                    string sStatus = ((Label)row.FindControl("lblStatus")).Text;

                    ImageButton imgDeactive = new ImageButton();
                    ImageButton imgActive = new ImageButton();
                    imgDeactive = (ImageButton)row.FindControl("imgDeactive");
                    imgActive = (ImageButton)row.FindControl("imgActive");

                    clsUser objUser = new clsUser();
                    objUser.lSlNo = sUserId;
                    ViewState["ID"] = sUserId;
                    ViewState["US_STATUS1"] = sStatus;

                    txtEffectFrom.Text = string.Empty;
                    txtReason.Text = string.Empty;

                    mdlPopup.Show();
                }
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtSearchName = (TextBox)row.FindControl("txtsFullName");
                    TextBox txtSearchDesignation = (TextBox)row.FindControl("txtsDesignation");
                    DataTable dt = (DataTable)ViewState["USER"];
                    dv = dt.DefaultView;

                    if (txtSearchName.Text != "")
                    {
                        sFilter = "US_FULL_NAME Like '%" + txtSearchName.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdUser.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdUser.DataSource = dv;
                            ViewState["USER"] = dv.ToTable();
                            grdUser.DataBind();
                        }
                        else
                        {
                            ViewState["USER"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadUserDetails();
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdUser_RowDataBound(object sender, GridViewRowEventArgs e)
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
                        imgBtnEdit.Enabled = false;
                        imgBtnEdit.ToolTip = "User is DeActivated,You Cannot Edit";
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdSubmit_Click(object sender, EventArgs e)
        {
            string Status = string.Empty;
            clsUser objUser = new clsUser();
            DataTable DtUserDetails = new DataTable();
            objSession = (clsSession)Session["clsSession"];
            try
            {
                objUser.zone = hdcmbZone.Text;
                objUser.circle = hdcmbCircle.Text;
                objUser.division = hdcmbDivision.Text;
                objUser.subdivision = hdcmbSubDivision.Text;
                objUser.section = hdcmbSection.Text;

                Status = txtlastDropdownSelected.Text;
                if ((Status ?? "").Length == 0)
                {
                    objUser.sOffCode = objSession.OfficeCode;
                    if ((objUser.sOffCode ?? "").Length == 3)
                    {
                        objUser.FetchStore = true;
                    }
                }
                
                switch (Status)
                {
                    case "1":
                        objUser.sOffCode = objUser.zone;
                        objUser.FetchStore = true;
                        break;
                    case "2":
                        objUser.sOffCode = objUser.circle;
                        objUser.FetchStore = true;
                        break;
                    case "3":
                        objUser.sOffCode = objUser.division;
                        objUser.FetchStore = true;
                        break;
                    case "4":
                        objUser.sOffCode = objUser.subdivision;
                        objUser.FetchStore = false;
                        break;
                    case "5":
                        objUser.sOffCode = objUser.section;
                        objUser.FetchStore = false;
                        break;
                }
               

                if (ValidateEnableDisable() == true)
                {
                    //objSession = (clsSession)Session["clsSession"];

                    objUser.sReason = txtReason.Text;
                    objUser.sEffectFrom = txtEffectFrom.Text;
                    objUser.lSlNo = Convert.ToString(ViewState["ID"]);
                    objUser.sStatus = Convert.ToString(ViewState["US_STATUS1"]);
                    ImageButton imgDeactive = new ImageButton();
                    ImageButton imgActive = new ImageButton();
                    string officeCode = string.Empty;
                    if (objUser.sStatus == "A")
                    {
                        objUser.sStatus = "D";
                        bool bResult = objUser.ActiveDeactiveUser(objUser);
                        if (bResult == true)
                        {
                            imgDeactive.Visible = true;
                            imgActive.Visible = false;
                            ShowMsgBox("User Deactivated Successfully");

                            if (objSession.sRoleType == Constants.Roles.OfficeLevel) // logic for the EE_DO // "1"
                            {
                                if ((objSession.OfficeCode ?? "").Length > 0)
                                {
                                    officeCode = objSession.OfficeCode;
                                }
                            }
                            //LoadUserDetails(officeCode);
                            //objUser.ActiveUser = "1"; // 1 means Deactive User
                            DtUserDetails = objUser.GetUserGridDetails(objUser);
                            if (DtUserDetails.Rows.Count == 0)
                            {
                                ShowEmptyGrid();
                                //ViewState["USER"] = DtInActiveUser;
                            }
                            else
                            {
                                grdUser.DataSource = DtUserDetails;
                                grdUser.DataBind();
                                //ViewState["USER"] = DtInActiveUser;
                            }
                            ViewState["USER"] = DtUserDetails;

                            txtEffectFrom.Text = "";
                            txtReason.Text = "";



                            cmdActiveUser.BackColor = System.Drawing.Color.Empty;
                            cmdInActiveUser.BackColor = System.Drawing.Color.Empty;
                        }
                    }
                    else
                    {
                        objUser.sStatus = "A";
                        bool bResult = objUser.ActiveDeactiveUser(objUser);
                        if (bResult == true)
                        {
                            imgDeactive.Visible = false;
                            imgActive.Visible = true;
                            ShowMsgBox("User Activated Successfully");
                            if (objSession.sRoleType == Constants.Roles.OfficeLevel) // logic for the EE_DO //  "1"
                            {
                                if ((objSession.OfficeCode ?? "").Length > 0)
                                {
                                    officeCode = objSession.OfficeCode;
                                }
                            }
                            //LoadUserDetails(officeCode);
                            //objUser.ActiveUser = "0"; // 1 means Deactive User
                            DtUserDetails = objUser.GetUserGridDetails(objUser);

                            if (DtUserDetails.Rows.Count == 0)
                            {
                                ShowEmptyGrid();
                                //ViewState["USER"] = DtInActiveUser;
                            }
                            else
                            {
                                grdUser.DataSource = DtUserDetails;
                                grdUser.DataBind();
                                //ViewState["USER"] = DtInActiveUser;
                            }
                            ViewState["USER"] = DtUserDetails;

                            txtEffectFrom.Text = "";
                            txtReason.Text = "";

                            cmdActiveUser.BackColor = System.Drawing.Color.Empty;
                            cmdInActiveUser.BackColor = System.Drawing.Color.Empty;

                        }
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
        /// ValidateEnableDisable
        /// </summary>
        /// <returns></returns>
        public bool ValidateEnableDisable()
        {
            bool bValidate = false;
            try
            {
                if (txtEffectFrom.Text.Trim() == "")
                {
                    lblMsg.Text = "Please Enter Effect From";
                    txtEffectFrom.Focus();
                    mdlPopup.Show();
                    return bValidate;
                }
                if (txtReason.Text.Trim() == "")
                {
                    lblMsg.Text = "Please Enter Reason";
                    txtReason.Focus();
                    mdlPopup.Show();
                    return bValidate;
                }
                if (txtReason.Text.Length > 500)
                {
                    lblMsg.Text = "Please Enter Below 500 charecters";
                    txtReason.Focus();
                    mdlPopup.Show();
                    return bValidate;
                }

                string sResult = Genaral.DateComparision(txtEffectFrom.Text, "", true, false);
                if (sResult == "2")
                {
                    ShowMsgBox("Effect From Date should be Greater than Current Date");
                    txtEffectFrom.Focus();
                    mdlPopup.Show();
                    return bValidate;
                }

                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
                return bValidate;
            }
        }
        #region Access Rights
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
                objApproval.sFormName = "UserCreate";
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sAccessType = Constants.CheckAccessRights.CheckAccessRightsAll + "," + sAccessType; //  "1"
                bool bResult = objApproval.CheckAccessRights(objApproval);
                if (bResult == false)
                {
                    if (sAccessType == Constants.CheckAccessRights.CheckAccessRightsReadOnly)
                    {
                        Response.Redirect("~/UserRestrict.aspx", false);
                    }
                    else
                    {
                        //ShowMsgBox("Sorry , You are not authorized to Access");
                        ShowMsgBox(Convert.ToString(ConfigurationManager.AppSettings["AccessRightsIfDenied"]));
                    }
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

        #endregion
        /// <summary>
        /// cmdReset_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                objSession = (clsSession)Session["clsSession"];
                string officeCode = string.Empty;
                if (objSession.sRoleType == "1") // logic for the EE_DO
                {
                    if ((objSession.OfficeCode ?? "").Length > 0)
                    {
                        officeCode = objSession.OfficeCode;
                    }
                    LoadUserDetails(officeCode);
                    //cmbsubdivision.Items.Clear();
                    //cmbSection.Items.Clear();
                }
                else
                {
                    LoadUserDetails();
                    cmbZone.SelectedIndex = 0;
                    cmbCircle.SelectedIndex = 0;
                    cmbDivision.SelectedIndex = 0;
                    //cmbCircle.Items.Clear();
                    //cmbDivision.Items.Clear();
                    //cmbsubdivision.Items.Clear();
                    //cmbSection.Items.Clear();
                }
                cmbsubdivision.SelectedIndex = 0;
                cmbSection.SelectedIndex = 0;
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
        /// for In Active Grid 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdInActiveUser_click(object sender, EventArgs e)
        {
            string Status = string.Empty;
            clsUser objUser = new clsUser();
            objSession = (clsSession)Session["clsSession"];
            DataTable DtInActiveUser = new DataTable();
            try
            {
                cmdActiveUser.BackColor = System.Drawing.Color.Empty;
                //objUser.sOffCode = hidSelectedLoaction.Value;
                //if ((hidSelectedLoaction.Value ?? "").Length == 0)
                //{
                //    if (objSession.sRoleType == "1")
                //    {
                //        objUser.sOffCode = objSession.OfficeCode;
                //    }
                //}
                objUser.ActiveUser = "1"; // 1 means Deactive User

                Status = txtlastDropdownSelected.Text;
                objUser.zone = hdcmbZone.Text;
                objUser.circle = hdcmbCircle.Text;
                objUser.division = hdcmbDivision.Text;
                objUser.subdivision = hdcmbSubDivision.Text;
                objUser.section = hdcmbSection.Text;
                switch (Status)
                {
                    case "1":
                        objUser.sOffCode = objUser.zone;
                        objUser.FetchStore = true;
                        break;
                    case "2":
                        objUser.sOffCode = objUser.circle;
                        objUser.FetchStore = true;
                        break;
                    case "3":
                        objUser.sOffCode = objUser.division;
                        objUser.FetchStore = true;
                        break;
                    case "4":
                        objUser.sOffCode = objUser.subdivision;
                        objUser.FetchStore = false;
                        break;
                    case "5":
                        objUser.sOffCode = objUser.section;
                        objUser.FetchStore = false;
                        break;
                }
                if ((Status ?? "").Length == 0)
                {
                    objUser.sOffCode = objSession.OfficeCode;
                    if ((objUser.sOffCode ?? "").Length == 3)
                    {
                        objUser.FetchStore = true;
                    }
                }
                //DtInActiveUser = objUser.LoadUserGrid(objUser); // old call
                DtInActiveUser = objUser.GetUserGridDetails(objUser);
                if (DtInActiveUser.Rows.Count == 0)
                {
                    ShowEmptyGrid();
                    //ViewState["USER"] = DtInActiveUser;
                }
                else
                {
                    grdUser.DataSource = DtInActiveUser;
                    grdUser.DataBind();
                    //ViewState["USER"] = DtInActiveUser;
                }
                ViewState["USER"] = DtInActiveUser;
                cmdInActiveUser.BackColor = System.Drawing.Color.Green;
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
        /// for Active Grid 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdActiveUser_click(object sender, EventArgs e)
        {
            string Status = string.Empty;
            clsUser objUser = new clsUser();
            objSession = (clsSession)Session["clsSession"];
            DataTable DtInActiveUser = new DataTable();
            try
            {
                cmdInActiveUser.BackColor = System.Drawing.Color.Empty;
                //objUser.sOffCode = hidSelectedLoaction.Value;
                //if ((hidSelectedLoaction.Value ?? "").Length == 0)
                //{
                //    if (objSession.sRoleType == "1")
                //    {
                //        objUser.sOffCode = objSession.OfficeCode;
                //    }
                //}                
                Status = txtlastDropdownSelected.Text;
                objUser.zone = hdcmbZone.Text;
                objUser.circle = hdcmbCircle.Text;
                objUser.division = hdcmbDivision.Text;
                objUser.subdivision = hdcmbSubDivision.Text;
                objUser.section = hdcmbSection.Text;
                switch (Status)
                {
                    case "1":
                        objUser.sOffCode = objUser.zone;
                        objUser.FetchStore = true;
                        break;
                    case "2":
                        objUser.sOffCode = objUser.circle;
                        objUser.FetchStore = true;
                        break;
                    case "3":
                        objUser.sOffCode = objUser.division;
                        objUser.FetchStore = true;
                        break;
                    case "4":
                        objUser.sOffCode = objUser.subdivision;
                        objUser.FetchStore = false;
                        break;
                    case "5":
                        objUser.sOffCode = objUser.section;
                        objUser.FetchStore = false;
                        break;
                }

                if ((Status ?? "").Length == 0)
                {
                    objUser.sOffCode = objSession.OfficeCode;
                    if ((objUser.sOffCode ?? "").Length == 3)
                    {
                        objUser.FetchStore = true;
                    }
                }


                //DtInActiveUser = objUser.LoadUserGrid(objUser); // Old call 
                objUser.ActiveUser = "0"; // 1 means Deactive User
                DtInActiveUser = objUser.GetUserGridDetails(objUser);
                if (DtInActiveUser.Rows.Count == 0)
                {
                    ShowEmptyGrid();
                    ViewState["USER"] = DtInActiveUser;

                }
                else
                {
                    grdUser.DataSource = DtInActiveUser;
                    grdUser.DataBind();
                    ViewState["USER"] = DtInActiveUser;
                }
                cmdActiveUser.BackColor = System.Drawing.Color.Green;
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
        public SortDirection direction
        {
            get
            {
                if (ViewState["directionState"] == null)
                {
                    ViewState["directionState"] = SortDirection.Ascending;
                }
                return (SortDirection)ViewState["directionState"];
            }
            set
            {
                ViewState["directionState"] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Export_click(object sender, EventArgs e)
        {
            DataTable dtUserDetails = (DataTable)ViewState["USER"];

            if (dtUserDetails.Rows.Count > 0)
            {
                dtUserDetails.Columns["zone"].ColumnName = "Zone Name";
                dtUserDetails.Columns["circle"].ColumnName = "Circle Name";
                dtUserDetails.Columns["division"].ColumnName = "Division Name";
                dtUserDetails.Columns["subdivision"].ColumnName = "Sub Division Name";
                dtUserDetails.Columns["section"].ColumnName = "Section Name";

                dtUserDetails.Columns["RO_NAME"].ColumnName = "Role Name";
                dtUserDetails.Columns["US_FULL_NAME"].ColumnName = "User Name";
                dtUserDetails.Columns["US_LG_NAME"].ColumnName = "User ID";
                dtUserDetails.Columns["US_EMAIL"].ColumnName = "User E-MAIL";
                dtUserDetails.Columns["US_MOBILE_NO"].ColumnName = "User Phone";
                dtUserDetails.Columns["US_STATUS"].ColumnName = "Current Status";
                //dtUserDetails.Columns["US_DESG_ID"].ColumnName = "DESIGNATION ";
                //dtUserDetails.Columns["OFF_NAME"].ColumnName = "LOCATION";

                dtUserDetails.Columns["ROLE NAME"].SetOrdinal(5);

                //List<int> listtoRemove = new List<int> { dtUserDetails.Columns["US_ID"].Ordinal, dtUserDetails.Columns["US_STATUS"].Ordinal,
                //    dtUserDetails.Columns["US_STATUS1"].Ordinal };
                //"US_DESG_ID",
                List<string> listtoRemove = new List<string> { "US_ID","US_DESG_ID","OFF_NAME",
                "US_STATUS1" };
                string filename = "UsersDetails" + DateTime.Now + ".xls";
                string pagetitle = "Users Details";

                Genaral.getexcel(dtUserDetails, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowEmptyGrid();
                ShowMsgBox("No Records Found");
            }
        }
    }
}

