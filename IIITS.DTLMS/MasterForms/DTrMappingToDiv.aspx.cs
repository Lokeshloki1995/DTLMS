using IIITS.DTLMS.BL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL.MasterForms;
using System.Configuration;

namespace IIITS.DTLMS.MasterForms
{
    public partial class DTrMappingToDiv : System.Web.UI.Page
    {
        string trFormCode = "DTrMappingToDiv";
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
                lblMessage.Text = string.Empty;
                if (!IsPostBack)
                {
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// search button to map dtr to division
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.mdlPopup.Show();
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
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to display divisions for dtr mapping
        /// </summary>
        /// <param name="OfficeCode"></param>
        /// <param name="OffName"></param>
        public void LoadOffice(string OfficeCode = "", string OffName = "")
        {
            try
            {
                DataTable dtPageDetaiils = new DataTable();
                clsFeederMast objStation = new clsFeederMast();
                objStation.OfficeCode = OfficeCode;
                objStation.OfficeName = OffName;
                dtPageDetaiils = objStation.LoadOfficeDtrMapping(objStation);
                GrdOffices.DataSource = dtPageDetaiils;
                GrdOffices.DataBind();
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to show selectedcheck boxes for division 
        /// </summary>
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
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to save selected divisions to dtr's
        /// </summary>
        private void SaveCheckedValues()
        {
            try
            {
                ArrayList userdetails = new ArrayList();
                ArrayList tmpArrayList = new ArrayList();
                int index = -1;
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
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to show indexing for division grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to work as commands passed to the grid as searching
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to bind data to division grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to bind selected divisions to textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                string OfficeCode = string.Empty;
                for (int i = 0; i < arrChecked.Count; i++)
                {
                    OfficeCode += arrChecked[i];
                    if (OfficeCode.EndsWith(",") == false)
                    {
                        OfficeCode = OfficeCode + ",";
                    }
                }
                if (OfficeCode.EndsWith(",") == true)
                {
                    OfficeCode = OfficeCode.Remove(OfficeCode.Length - 1);
                }
                txtOfficeCode.Text = OfficeCode;
                txtOfficeCode.Enabled = false;

            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// save button to map dtr's to divisions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string[] Arrmsg = new string[3];
                if (ValidateForm() == true)
                {
                    ClsDTrMappingToDiv objDtrMappingToDiv = new ClsDTrMappingToDiv();
                    objDtrMappingToDiv.OffCode = txtOfficeCode.Text;
                    objDtrMappingToDiv.Startrange = txtDTrStartRange.Text;
                    objDtrMappingToDiv.Quantity = txtQuantity.Text;
                    objDtrMappingToDiv.Endrange = txtDTrEndRange.Text;
                    objDtrMappingToDiv.CrBy = objSession.UserId;
                    Arrmsg = objDtrMappingToDiv.CheckTheDtrExistied(objDtrMappingToDiv);
                    if (Arrmsg[1].ToString() == "0")
                    {
                        btnSave.Enabled = false;
                        ShowMsgBox(Arrmsg[0]);
                        return;
                    }
                    if (Arrmsg[1].ToString() == "2")
                    {
                        ShowMsgBox(Arrmsg[0]);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to validate the field before saving
        /// </summary>
        /// <returns></returns>
        bool ValidateForm()
        {
            bool bValidate = false;
            try
            {
                if (txtOfficeCode.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Please Select Division Code");
                    txtOfficeCode.Focus();
                    return bValidate;
                }

                if (txtDTrStartRange.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Please Enter DTr start Range");
                    txtDTrStartRange.Focus();
                    return bValidate;
                }
                if (txtQuantity.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Please Enter Quantity");
                    txtQuantity.Focus();
                    return bValidate;
                }
                if (txtQuantity.Text.Contains("."))
                {
                    ShowMsgBox("Please Enter Valid Quantity");
                    txtQuantity.Focus();
                    return bValidate;
                }
                if (txtDTrStartRange.Text.Contains("."))
                {
                    ShowMsgBox("Please Enter Valid DTr Start Range");
                    txtDTrStartRange.Focus();
                    return bValidate;
                }
                if (txtQuantity.Text.Trim() == "0")
                {
                    ShowMsgBox("Quantity can not be zero");
                    txtQuantity.Focus();
                    return bValidate;
                }
                if (txtDTrStartRange.Text.Trim().Length != 7)
                {
                    ShowMsgBox("Please Enter Valid DTr start Range");
                    txtDTrStartRange.Focus();
                    return bValidate;
                }
                if (txtDTrStartRange.Text.ToUpper().Trim('H').Length == 6)
                {
                    if (!txtDTrStartRange.Text.Trim().ToUpper().StartsWith("H"))
                    {
                        ShowMsgBox("Please Enter Valid DTr Start Range");
                        return bValidate;
                    }
                    //commented for dtr start range validation
                    //if (txtDTrStartRange.Text[1] == '0')
                    //{
                    //    ShowMsgBox("Please Enter Valid DTr Start Range");
                    //    return bValidate;
                    //}
                }
                else
                {
                    ShowMsgBox("Please Enter Valid DTr Start Range");
                    return bValidate;
                }
                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return bValidate;
            }
        }
        /// <summary>
        /// to show popup alert messege 
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
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// on change function for quantity to generate dtr end range
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtQuantity_OnTextChanged(object sender, EventArgs e)
        {
            try
            {
                clsStation objStation = new clsStation();
                Int64 StartRange = 0;
                string EndRange = string.Empty;
                if (txtQuantity.Text != "" && txtDTrStartRange.Text != "")
                {
                    if (txtQuantity.Text.Contains("."))
                    {
                        ShowMsgBox("Please Enter Valid Quantity");
                        return;
                    }
                    if (txtDTrStartRange.Text.Contains("."))
                    {
                        ShowMsgBox("Please Enter Valid DTr Start Range");
                        return;
                    }
                    if (txtDTrStartRange.Text.ToUpper().Trim('H').Length == 6)
                    {
                        if (!txtDTrStartRange.Text.Trim().ToUpper().StartsWith("H"))
                        {
                            ShowMsgBox("Please Enter Valid DTr Start Range");
                            return;
                        }
                    }
                    else
                    {
                        ShowMsgBox("Please Enter Valid DTr Start Range");
                        return;
                    }
                    if (txtDTrStartRange.Text != "" && txtQuantity.Text != "")
                    {
                        StartRange = Convert.ToInt64(txtDTrStartRange.Text.ToUpper().Trim('H'));
                        EndRange = Convert.ToString(StartRange + Convert.ToInt64(txtQuantity.Text) - 1);
                        if (EndRange.Length < 6)
                        {
                            switch (EndRange.Length)
                            {
                                case 5:
                                    EndRange = "0" + EndRange;
                                    break;
                                case 4:
                                    EndRange = "00" + EndRange;
                                    break;
                                case 3:
                                    EndRange = "000" + EndRange;
                                    break;
                                case 2:
                                    EndRange = "0000" + EndRange;
                                    break;
                                case 1:
                                    EndRange = "00000" + EndRange;
                                    break;
                            }
                        }

                        if (Convert.ToInt32(StartRange) == Convert.ToInt32(ConfigurationManager.AppSettings["DTRSTARTRANGE"]))
                        {
                            ShowMsgBox("Please enter valid DTr Start Range");
                            return;
                        }
                        if (Convert.ToInt32(EndRange) > Convert.ToInt32(ConfigurationManager.AppSettings["DTRENDRANGE"]))
                        {
                            ShowMsgBox("DTr End Range limit Exceeds 300000");
                            return;
                        }
                        txtDTrEndRange.Text = "H" + EndRange;
                    }
                    txtDTrStartRange.Text = Convert.ToString(txtDTrStartRange.Text.ToUpper());
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// to reset the fields entered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtQuantity.Text = string.Empty;
                txtDTrStartRange.Text = string.Empty;
                txtDTrEndRange.Text = string.Empty;
                txtOfficeCode.Text = string.Empty;
                ViewState["CHECKED_ITEMS"] = null;
                GrdOffices.AllowPaging = true;
                GrdOffices.PageIndex = 0;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}