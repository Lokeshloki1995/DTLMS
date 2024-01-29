using IIITS.DTLMS.BL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.Reports
{
    public partial class UserDetailsReport : System.Web.UI.Page
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
                lblErrormsg.Text = string.Empty;
                txtZoneCode.Enabled = false;
                txtCircleCode.Enabled = false;
                txtDivisionCode.Enabled = false;
                txtSubDivisionCode.Enabled = false;
                txtSectionCode.Enabled = false;
                if (!IsPostBack)
                {
                    
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, 
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                lblErrormsg.Text = clsException.ErrorMsgPL(ex.Message);
            }
        }
        //This method is used to save the checkedstate of values
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
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, 
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }
        protected void cmdReset_Click(object sender, EventArgs e)
        {
            txtZoneCode.Text = string.Empty;
            txtCircleCode.Text = string.Empty;
            txtDivisionCode.Text = string.Empty;
            txtSubDivisionCode.Text = string.Empty;
            txtSectionCode.Text = string.Empty;
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
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, 
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void btnOK_Click1(object sender, EventArgs e)
        {
            try
            {
                ArrayList arrChecked = new ArrayList();
              string val=  hdfStatus.Value;
                GrdOffices.AllowPaging = false;
                SaveCheckedValues();
                LoadOffice(objSession.OfficeCode, "", hdfStatus.Value);
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
              string status=  LoadOffice(objSession.OfficeCode,"",hdfStatus.Value);
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
                
                if (sOfficeCode.EndsWith(",") == true)
                {
                    sOfficeCode = sOfficeCode.Remove(sOfficeCode.Length - 1);
                }
                //if (hdfStatus.Value == "1")
                //{
                //    txtZoneCode.Text = sOfficeCode;
                //    txtZoneCode.Enabled = false;
                //}
                //else
                //{
                //    txtCircleCode.Text = sOfficeCode;
                //    txtCircleCode.Enabled = false;
                //}
                switch(hdfStatus.Value)
                {
                    case "1":
                        txtZoneCode.Text = sOfficeCode;
                        txtZoneCode.Enabled = false;
                        break;
                    case "2":
                        txtCircleCode.Text = sOfficeCode;
                        txtCircleCode.Enabled = false;
                        break;
                    case "3":
                        txtDivisionCode.Text = sOfficeCode;
                        txtDivisionCode.Enabled = false;
                        break;
                    case "4":
                        txtSubDivisionCode.Text = sOfficeCode;
                        txtSubDivisionCode.Enabled = false;
                        break;
                    case "5":
                        txtSectionCode.Text = sOfficeCode;
                        txtSectionCode.Enabled = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, 
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }
        protected void btnSearchZone_Click(object sender, EventArgs e)
        {
            try
            {
                this.mdlPopup.Show();
                hdfStatus.Value = "1";
                if (ViewState["CHECKED_ITEMS"] != null)
                {
                    SaveCheckedValues();
                    LoadOffice(objSession.OfficeCode, "", hdfStatus.Value);
                    PopulateCheckedValues();
                }
                else
                {
                    LoadOffice(objSession.OfficeCode, "", hdfStatus.Value);
                }



            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, 
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void btnSearchCircle_Click(object sender, EventArgs e)
        {
            try
            {
                this.mdlPopup.Show();
                hdfStatus.Value = "2";
                if (ViewState["CHECKED_ITEMS"] != null)
                {
                    SaveCheckedValues();
                  //  LoadOffice(objSession.OfficeCode,"", hdfStatus.Value);
                    PopulateCheckedValues();
                }
                else
                {
                  //  LoadOffice(objSession.OfficeCode, "", hdfStatus.Value);
                }
                LoadOffice(objSession.OfficeCode, "", hdfStatus.Value);


            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void btnSearchDivision_Click(object sender, EventArgs e)
        {
            try
            {
                this.mdlPopup.Show();
                hdfStatus.Value = "3";
                if (ViewState["CHECKED_ITEMS"] != null)
                {
                    SaveCheckedValues();
                    //  LoadOffice(objSession.OfficeCode,"", hdfStatus.Value);
                    PopulateCheckedValues();
                }
                else
                {
                    //  LoadOffice(objSession.OfficeCode, "", hdfStatus.Value);
                }
                LoadOffice(objSession.OfficeCode, "", hdfStatus.Value);


            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void btnSearchSubDivision_Click(object sender, EventArgs e)
        {
            try
            {
                this.mdlPopup.Show();
                hdfStatus.Value = "4";
                if (ViewState["CHECKED_ITEMS"] != null)
                {
                    SaveCheckedValues();
                    //  LoadOffice(objSession.OfficeCode,"", hdfStatus.Value);
                    PopulateCheckedValues();
                }
                else
                {
                    //  LoadOffice(objSession.OfficeCode, "", hdfStatus.Value);
                }
                LoadOffice(objSession.OfficeCode, "", hdfStatus.Value);


            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void btnSearchSection_Click(object sender, EventArgs e)
        {
            try
            {
                this.mdlPopup.Show();
                hdfStatus.Value = "5";
                if (ViewState["CHECKED_ITEMS"] != null)
                {
                    SaveCheckedValues();
                    //  LoadOffice(objSession.OfficeCode,"", hdfStatus.Value);
                    PopulateCheckedValues();
                }
                else
                {
                    //  LoadOffice(objSession.OfficeCode, "", hdfStatus.Value);
                }
                LoadOffice(objSession.OfficeCode, "", hdfStatus.Value);


            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public string  LoadOffice(string sOfficeCode = "", string sOffName = "", string status = "")
        {
            try
            {
                DataTable dtPageDetaiils = new DataTable();
                clsReports obj = new clsReports();
                obj.OfficeCode = sOfficeCode;
                obj.OfficeName = sOffName;
                obj.UserStatusFlag = status;
                switch (status)
                {
                    case "2":
                        obj.offcodeDT = txtZoneCode.Text;
                        break;
                    case "3":
                        obj.offcodeDT = txtCircleCode.Text;
                        break;
                    case "4":
                        obj.offcodeDT = txtDivisionCode.Text;
                        break;
                    case "5":
                        obj.offcodeDT = txtSubDivisionCode.Text;
                        break;
                }
                dtPageDetaiils = obj.LoadOfficeDet(obj);
              
                GrdOffices.DataSource = dtPageDetaiils;
                GrdOffices.DataBind();
                return status;
            }

            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, 
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return status;
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
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, 
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
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
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, 
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }

        protected void GrdOffices_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                SaveCheckedValues();
                GrdOffices.PageIndex = 0;
                GrdOffices.PageIndex = e.NewPageIndex;
                LoadOffice(objSession.OfficeCode,"", hdfStatus.Value);
                PopulateCheckedValues();
                this.mdlPopup.Show();
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, 
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}