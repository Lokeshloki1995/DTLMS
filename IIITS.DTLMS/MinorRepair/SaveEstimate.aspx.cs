using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.IO;
using ClosedXML.Excel;
using System.Net;
namespace IIITS.DTLMS.MinorRepair
{
    public partial class SaveEstimate : System.Web.UI.Page
    {

        string strFormCode = "SaveEstimate";
        clsSession objSession = new clsSession();
        DataTable dtMeasurement = new DataTable();
       
        string sFromDate = string.Empty;
        string sToDate = string.Empty;
        string sFileServerPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["EstimatioinVirtualPath"]);
        string sUserName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_USER"]);
        string sPassword = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_PASS"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                cmdSave.Enabled = true;
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");
                txtPoDate.Attributes.Add("readonly", "readonly");

                CalendarExtender1.EndDate = System.DateTime.Now;
                CheckAccessRights("4");
                if (!IsPostBack)
                {
                    Genaral.Load_Combo("SELECT \"DIV_ID\", \"DIV_NAME\" FROM \"TBLDIVISION\" ORDER BY \"DIV_CODE\"", "--Select--", cmbDivision);
                    Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='C' ORDER BY \"MD_ORDER_BY\"", "--Select--", cmbCapacity);
                    LoadMeasurement();
                    if (Request.QueryString["RecordId"] != null && Request.QueryString["RecordId"].ToString() != "")
                    {
                        txtRepId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["RecordId"]));
                        LoadExistingDetails();
                    }
                    else
                    {
                        LoadMaterialDetailes();
                        LoadLabourDetailes();
                        LoadSalvageDetailes();
                    }                  
                   
                }
            }


            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "SaveEstimate";
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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;

            }
        }

        public void LoadExistingDetails()
        {
            try
            {
                clsEstimate obj = new clsEstimate();
                obj.sRecordId = txtRepId.Text;
                obj.GetExistingEstimationDetails(obj);
                cmbDivision.SelectedValue = obj.eDivision;
                cmbSelectType.SelectedValue = obj.eVendortype;
                cmbSelectType_SelectedIndexChanged(this, EventArgs.Empty);
                if (obj.eVendortype == "2")
                {
                    cmbSupplier.SelectedValue = obj.eUser;
                }
                else
                {
                    cmbRepairer.SelectedValue = obj.eUser;
                }
             
                cmbCapacity.SelectedValue = obj.eCapacity;
                txtFromDate.Text = obj.eFromDate;
                txtToDate.Text = obj.eToDate;
                txtPoNo.Text = obj.ePONumber;
                txtPoDate.Text = obj.ePODate;
                cmbwound.SelectedValue = obj.ewoundType;
                cmbRate.SelectedValue = obj.erateType;
                obj.GetExistingMaterial(obj);
                DataTable dtExistingMatrials = new DataTable();
                dtExistingMatrials = obj.dtMaterials;

                DataTable dtMaterials = new DataTable();

                var MaterialList = dtExistingMatrials.AsEnumerable()
                 .Where(r => r.Field<Int64>("MRIM_ITEM_TYPE") == 1);
                if (MaterialList.Any())
                {
                    dtMaterials = MaterialList.CopyToDataTable();
                }

                if(dtMaterials.Rows.Count > 0)
                {
                    grdMaterialMast.DataSource = dtMaterials;
                    grdMaterialMast.DataBind();
                }

                var LabourList = dtExistingMatrials.AsEnumerable()
                 .Where(r => r.Field<Int64>("MRIM_ITEM_TYPE") == 2);
                if (LabourList.Any())
                {
                    dtMaterials = LabourList.CopyToDataTable();
                }

                if (dtMaterials.Rows.Count > 0)
                {
                    grdLabourMast.DataSource = dtMaterials;
                    grdLabourMast.DataBind();
                }

                var SalvageList = dtExistingMatrials.AsEnumerable()
                 .Where(r => r.Field<Int64>("MRIM_ITEM_TYPE") == 3);
                if (LabourList.Any())
                {
                    dtMaterials = SalvageList.CopyToDataTable();
                }

                if (dtMaterials.Rows.Count > 0)
                {
                    grdSalvageMast.DataSource = dtMaterials;
                    grdSalvageMast.DataBind();
                }

                cmdSave.Text = "Update";

            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void LoadMeasurement()
        {           
            clsEstimate obj = new clsEstimate();
            dtMeasurement = obj.LoadMeasurement();
        }


        public void LoadMaterialDetailes()
        {
            try
            {
                DataTable dt = new DataTable();
                clsEstimate objEstimate = new clsEstimate();
                dt = objEstimate.LoadAllMaterialDetails();
                ViewState["Material"] = dt;
                grdMaterialMast.DataSource = dt;
                grdMaterialMast.DataBind();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadLabourDetailes()
        {
            try
            {
                DataTable dt = new DataTable();
                clsEstimate objEstimate = new clsEstimate();
                dt = objEstimate.LoadAllLabourDetails();
                ViewState["Labour"] = dt;
                grdLabourMast.DataSource = dt;
                grdLabourMast.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void LoadSalvageDetailes()
        {
            try
            {
                DataTable dt = new DataTable();
                clsEstimate objEstimate = new clsEstimate();
                dt = objEstimate.LoadAllSalvageDetailes();
                ViewState["Salvage"] = dt;
                grdSalvageMast.DataSource = dt;
                grdSalvageMast.DataBind();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void cmbSelectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSelectType.SelectedIndex == 1)
                {
                    cmbRepairer.Enabled = true;
                    txtRpCenterName.Enabled = true;
                    Genaral.Load_Combo("SELECT \"TR_ID\",\"TR_NAME\" FROM \"TBLTRANSREPAIRER\" WHERE \"TR_STATUS\"='A'", "--Select--", cmbRepairer);
                    cmbSupplier.Items.Clear();
                    cmbSupplier.Enabled = false;
                }

                else
                {
                    cmbSupplier.Enabled = true;
                    Genaral.Load_Combo("SELECT \"TS_ID\",\"TS_NAME\" FROM \"TBLTRANSSUPPLIER\" WHERE \"TS_STATUS\"='A'", "--Select--", cmbSupplier);
                    cmbRepairer.Items.Clear();
                    cmbRepairer.Enabled = false;
                    txtRpCenterName.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }


        protected void grdMaterial_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                grdMaterialMast.PageIndex = e.NewPageIndex;
                dt = (DataTable)ViewState["Material"];
                //grdMaterialMast.DataSource = SortDataTable(dt as DataTable, true);
                grdMaterialMast.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void grdLabour_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                grdLabourMast.PageIndex = e.NewPageIndex;
                dt = (DataTable)ViewState["Labour"];
                //grdMaterialMast.DataSource = SortDataTable(dt as DataTable, true);
                grdLabourMast.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void grdSalvage_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                grdSalvageMast.PageIndex = e.NewPageIndex;
                dt = (DataTable)ViewState["Salvage"];
                //grdMaterialMast.DataSource = SortDataTable(dt as DataTable, true);
                grdSalvageMast.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "ASC"; }
            set { ViewState["SortDirection"] = value; }


        }


        private string GridViewSortExpression
        {
            get { return ViewState["SortExpression"] as string ?? string.Empty; }
            set { ViewState["SortExpression"] = value; }
        }


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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdSave1_Click(object sender, EventArgs e)
        {
            try
            {
                clsEstimate objEstimate = new clsEstimate();
                string[] Arr = new string[3];
                if (ValidateForm() == true)
                {                    
                    objEstimate.eVendortype = cmbSelectType.SelectedValue;
                    if (cmbSelectType.SelectedValue == "1")
                    {
                        objEstimate.eUser = cmbRepairer.SelectedValue;
                        objEstimate.eRpCenterName = txtRpCenterName.Text;
                        
                    }
                    else
                    {
                        objEstimate.eUser = cmbSupplier.SelectedValue;
                    }
              
                    objEstimate.eFromDate = txtFromDate.Text.Trim().Replace("'", "");
                    objEstimate.eToDate = txtToDate.Text.Trim().Replace("'", "");
                    objEstimate.eCapacity = cmbCapacity.SelectedValue;
                    objEstimate.eDivision = cmbDivision.SelectedValue;
                    objEstimate.ePODate = txtPoDate.Text.Trim();
                    objEstimate.ePONumber = txtPoNo.Text.Trim().Replace("'", "").ToUpper();
                    objEstimate.ewoundType = cmbwound.SelectedValue;
                    objEstimate.erateType = cmbRate.SelectedValue;
                    objEstimate.sCrBy = objSession.UserId;

                    int i = 0;
                    string[] sMateriallist = new string[grdMaterialMast.Rows.Count];
                    bool bChecked = false;
                    foreach (GridViewRow row in grdMaterialMast.Rows)
                    {
                        if (((CheckBox)row.FindControl("CheckBx1")).Checked == true)
                        {
                            sMateriallist[i] = ((Label)row.FindControl("lblMaterialId")).Text.Trim() + "~" + ((TextBox)row.FindControl("txtMqty")).Text.Trim() + "~" +
                                ((TextBox)row.FindControl("txtMBRate")).Text.Trim() + "~" + ((TextBox)row.FindControl("txtMTRate")).Text.Trim() + "~" +
                                ((TextBox)row.FindControl("txtMTtal")).Text.Trim() + "~" + ((DropDownList)row.FindControl("cmbMeasurement")).SelectedValue;
                            bChecked = true;
                        }
                        i++;
                    }

                    if (bChecked == false)
                    {
                        ShowMsgBox("Please Select the Material Cost Details");
                        return;
                    }


                    int j = 0;
                    string[] sLabourlist = new string[grdLabourMast.Rows.Count];
                    bChecked = false;
                    foreach (GridViewRow row in grdLabourMast.Rows)
                    {
                        if (((CheckBox)row.FindControl("CheckBox2")).Checked == true)
                        {
                            sLabourlist[j] = ((Label)row.FindControl("lblLabourId")).Text.Trim() + "~" + ((TextBox)row.FindControl("txtLqty")).Text.Trim() + "~" +
                                ((TextBox)row.FindControl("txtLBRate")).Text.Trim() + "~" + ((TextBox)row.FindControl("txtLTRate")).Text.Trim() + "~" +
                                ((TextBox)row.FindControl("txtLTotal")).Text.Trim() + "~" + ((DropDownList)row.FindControl("cmbLabMeasurement")).SelectedValue;
                            bChecked = true;
                        }
                        j++;
                    }

                    if (bChecked == false)
                    {
                        ShowMsgBox("Please Select the Labour Cost Details");
                        return;
                    }

                    
                    int k = 0;
                    string[] sSalvageslist = new string[grdSalvageMast.Rows.Count];
                    bChecked = false;
                    foreach (GridViewRow row in grdSalvageMast.Rows)
                    {
                        if (((CheckBox)row.FindControl("CheckBox3")).Checked == true)
                        {
                            sSalvageslist[k] = ((Label)row.FindControl("lblSalvageId")).Text.Trim() + "~" + ((TextBox)row.FindControl("txtSqty")).Text.Trim() + "~" +
                                ((TextBox)row.FindControl("txtSBRate")).Text.Trim() + "~" + ((TextBox)row.FindControl("txtSTRate")).Text.Trim() + "~" +
                                ((TextBox)row.FindControl("txtLSTotal")).Text.Trim() + "~" + ((DropDownList)row.FindControl("cmbSalMeasurement")).SelectedValue;
                            bChecked = true;
                        }
                        k++;
                    }

                    if (bChecked == false)
                    {
                        ShowMsgBox("Please Select the Salvage Cost Details");
                        return;
                    }
                    

                    Arr = objEstimate.SaveEstimate(objEstimate,sMateriallist, sLabourlist, sSalvageslist);

                    if (Arr[1].ToString() == "0")
                    {
                        objEstimate.sRecordId = txtRepId.Text;
                        objEstimate.UpdateEffectiveDate(objEstimate);
                        ShowMsgBox(Arr[0].ToString());
                        //cmdSave.Enabled = false;
                        return;
                    }
                    else
                    {
                        ShowMsgBox(Arr[0].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        


        bool ValidateForm()
        {
            bool bValidate = false;
            try
            {               
                string sResult = string.Empty;

                if (txtFromDate.Text != "")
                {
                    sResult = Genaral.DateValidation(txtFromDate.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtFromDate.Focus();
                        return bValidate;
                    }
                }

                if (txtToDate.Text != "")
                {
                    sResult = Genaral.DateValidation(txtToDate.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtToDate.Focus();
                        return bValidate;
                    }
                }

                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    sResult = Genaral.DateComparision( txtFromDate.Text, txtToDate.Text , true, false);
                    if (sResult == "2")
                    {
                        ShowMsgBox("From Date should be Greater than Current Date");
                        txtFromDate.Focus();
                        return bValidate;

                    }

                    sResult = Genaral.DateComparision(txtFromDate.Text, txtToDate.Text, false , false);
                    if (sResult == "1")
                    {
                        ShowMsgBox("To Date should be Greater than From Date");
                        txtToDate.Focus();
                        return bValidate;

                    }
                }

                if (cmbwound.SelectedIndex == 0)
                {
                    ShowMsgBox("Please Select the Wound Type");
                    return bValidate;
                }
                if (cmbRate.SelectedIndex == 0)
                {
                    ShowMsgBox("Please Select the Rate Type");
                    return bValidate;
                }

                if (cmbDivision.SelectedValue == "--Select--")
                {
                    ShowMsgBox("Please Select any Division");
                    return bValidate;
                }
                else if (cmbSelectType.SelectedValue == "0")
                {
                    ShowMsgBox("Please Select any Type");
                    return bValidate;
                }

                string sQuantity = string.Empty;
                string sBaseRate = string.Empty;
                string sTaxRate = string.Empty;
                string sTotal = string.Empty;
                int index = 0;

                foreach (GridViewRow row in grdMaterialMast.Rows)
                {

                    if (((CheckBox)row.FindControl("CheckBx1")).Checked == true)
                    {
                        sQuantity = ((TextBox)row.FindControl("txtMqty")).Text;

                        if (sQuantity.Length == 0)
                        {
                            ShowMsgBox("Please Enter Material Quatity");
                            ((TextBox)row.FindControl("txtMqty")).Focus();
                            return bValidate;
                        }

                        sBaseRate = ((TextBox)row.FindControl("txtMBRate")).Text;

                        if (sQuantity.Length == 0)
                        {
                            ShowMsgBox("Please Enter Material Base Rate");
                            ((TextBox)row.FindControl("txtMBRate")).Focus();
                            return bValidate;
                        }

                        sTaxRate = ((TextBox)row.FindControl("txtMTRate")).Text;

                        if (sQuantity.Length == 0)
                        {
                            ShowMsgBox("Please Enter Material Tax Rate");
                            ((TextBox)row.FindControl("txtMTRate")).Focus();
                            return bValidate;
                        }

                        sTotal = ((TextBox)row.FindControl("txtMTtal")).Text;

                        if (sQuantity.Length == 0)
                        {
                            ShowMsgBox("Please Enter Material Total");
                            ((TextBox)row.FindControl("txtMTtal")).Focus();
                            return bValidate;
                        }

                        index = ((DropDownList)row.FindControl("cmbMeasurement")).SelectedIndex;

                        if (index == 0)
                        {
                            ShowMsgBox("Please Select the Mesurement");
                            ((DropDownList)row.FindControl("cmbMeasurement")).Focus();
                            return bValidate;
                        }
                    }
                }


                foreach (GridViewRow row1 in grdLabourMast.Rows)
                {

                    if (((CheckBox)row1.FindControl("CheckBox2")).Checked == true)
                    {
                        sQuantity = ((TextBox)row1.FindControl("txtLqty")).Text;

                        if (sQuantity.Length == 0)
                        {
                            ShowMsgBox("Please Enter Labour Quatity");
                            ((TextBox)row1.FindControl("txtLqty")).Focus();
                            return bValidate;
                        }

                        sBaseRate = ((TextBox)row1.FindControl("txtLBRate")).Text;

                        if (sQuantity.Length == 0)
                        {
                            ShowMsgBox("Please Enter Labour Base Rate");
                            ((TextBox)row1.FindControl("txtLBRate")).Focus();
                            return bValidate;
                        }

                        sTaxRate = ((TextBox)row1.FindControl("txtLTRate")).Text;

                        if (sQuantity.Length == 0)
                        {
                            ShowMsgBox("Please Enter Labour Tax Rate");
                            ((TextBox)row1.FindControl("txtLTRate")).Focus();
                            return bValidate;
                        }

                        sTotal = ((TextBox)row1.FindControl("txtLTotal")).Text;

                        if (sQuantity.Length == 0)
                        {
                            ShowMsgBox("Please Enter Labour Total");
                            ((TextBox)row1.FindControl("txtLTotal")).Focus();
                            return bValidate;
                        }

                        index = ((DropDownList)row1.FindControl("cmbLabMeasurement")).SelectedIndex;

                        if (index == 0)
                        {
                            ShowMsgBox("Please Select the Labour Mesurement");
                            ((DropDownList)row1.FindControl("cmbLabMeasurement")).Focus();
                            return bValidate;
                        }
                    }
                }


                foreach (GridViewRow row2 in grdSalvageMast.Rows)
                {

                    if (((CheckBox)row2.FindControl("CheckBox3")).Checked == true)
                    {
                        sQuantity = ((TextBox)row2.FindControl("txtSqty")).Text;

                        if (sQuantity.Length == 0)
                        {
                            ShowMsgBox("Please Enter Labour Quatity");
                            ((TextBox)row2.FindControl("txtSqty")).Focus();
                            return bValidate;
                        }

                        sBaseRate = ((TextBox)row2.FindControl("txtSBRate")).Text;

                        if (sQuantity.Length == 0)
                        {
                            ShowMsgBox("Please Enter Labour Base Rate");
                            ((TextBox)row2.FindControl("txtSBRate")).Focus();
                            return bValidate;
                        }

                        sTaxRate = ((TextBox)row2.FindControl("txtSTRate")).Text;

                        if (sQuantity.Length == 0)
                        {
                            ShowMsgBox("Please Enter Labour Tax Rate");
                            ((TextBox)row2.FindControl("txtSTRate")).Focus();
                            return bValidate;
                        }

                        sTotal = ((TextBox)row2.FindControl("txtLSTotal")).Text;

                        if (sQuantity.Length == 0)
                        {
                            ShowMsgBox("Please Enter Labour Total");
                            ((TextBox)row2.FindControl("txtLSTotal")).Focus();
                            return bValidate;
                        }

                        index = ((DropDownList)row2.FindControl("cmbSalMeasurement")).SelectedIndex;

                        if (index == 0)
                        {
                            ShowMsgBox("Please Select the Salvage Mesurement");
                            ((DropDownList)row2.FindControl("cmbSalMeasurement")).Focus();
                            return bValidate;
                        }
                    }
                }

                bValidate = true;
                return bValidate;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return bValidate;
            }
        }

        //protected void FTPUpload(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        //FTP Folder name. Leave blank if you want to upload to root folder.
        //        // string ftpFolder = "Uploads/";      
        //        if (cmbDivision.Text != "" && cmbRepairer.Text != "")
        //        {

        //            string fileName = Path.GetFileName(FileUpload1.FileName);

        //            if (fileName == "" || fileName == null)
        //            {
        //                ShowMsgBox("Please Select the file");
        //                return;

        //            }
        //            string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["MaterialallotmentFormat"]);
        //            string sAnxFileExt = System.IO.Path.GetExtension(FileUpload1.FileName).ToString().ToLower();
        //            sAnxFileExt = ";" + sAnxFileExt.Remove(0, 1) + ";";
        //            clsFtp objFtp = new clsFtp(sFileServerPath, sUserName, sPassword);
        //            bool Isuploaded;
        //            bool IsFileExiest;
        //            string sMainFolderName = "ESTIMATIONRATES";
        //            if (!sFileExt.Contains(sAnxFileExt))
        //            {
        //                ShowMsgBox("Invalid Image Format");
        //                return;
        //            }

        //            // string sFileName = Path.GetFileName(fupAnx.PostedFile.FileName).Replace(",", "");

        //            FileUpload1.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + fileName));
        //            string sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + fileName);


        //            DataTable dt = new DataTable("NEWTABLE");

        //            if (ViewState["DOCUMENTS"] == null)
        //            {
        //                dt.Columns.Add("ID");
        //                dt.Columns.Add("NAME");
        //                dt.Columns.Add("TYPE");
        //                dt.Columns.Add("PATH");
        //            }
        //            else
        //            {

        //                dt = (DataTable)ViewState["DOCUMENTS"];
        //            }
        //            DataTable dtDocs = new DataTable();


        //            //if (dtDocs != null)
        //            //{
        //            //    for (int i = 0; i < dtDocs.Rows.Count; i++)
        //            //    {
        //                  //  string sName = Convert.ToString(dtDocs.Rows[i]["NAME"]);
        //                  // string sPath = Convert.ToString(dtDocs.Rows[i]["PATH"]);

        //                    if (File.Exists(sDirectory))
        //                    {

        //                        bool IsExists = objFtp.FtpDirectoryExists(sFileServerPath + "/" +sMainFolderName +"/" + cmbDivision.Text , sUserName, sPassword);
        //                        if (IsExists == false)
        //                        {

        //                            objFtp.createDirectory(sMainFolderName+ "/"+cmbDivision.Text  );
        //                        }
        //                         IsExists = objFtp.FtpDirectoryExists(sFileServerPath + "/" + sMainFolderName + "/" + cmbDivision.Text + "/" + cmbRepairer.Text, sUserName, sPassword);
        //                        if (IsExists == false)
        //                        {

        //                            objFtp.createDirectory(sMainFolderName + "/" + cmbDivision.Text + "/" + cmbRepairer.Text);
        //                        }
        //                        IsFileExiest = objFtp.IsfileExiest(sMainFolderName + "/" + cmbDivision.Text + "/" + cmbRepairer.Text);
        //                        if (IsFileExiest == false)
        //                        {
        //                            Isuploaded = objFtp.upload(sMainFolderName + "/" + cmbDivision.Text + "/" + cmbRepairer.Text + "/" + fileName, sDirectory);
        //                            if (Isuploaded == true & File.Exists(sDirectory))
        //                            {
        //                                File.Delete(sDirectory);
        //                                sDirectory = cmbDivision.Text + "/" + cmbRepairer.Text + "/" + fileName;
        //                                ShowMsgBox("Successfully Uploaded your File!");
        //                                return;
        //                            }
        //                        }

                               
                               
        //                    }
        //                   // dtDocs.Rows[i]["PATH"] = sPath;
        //              //  }
        //            //}

        //           // dtDocs = (DataTable)ViewState["DOCUMENTS"];

        //            //if (File.Exists(sDirectory))
        //            //{
        //            //    bool IsExists = objFtp.FtpDirectoryExists(sFileServerPath, sUserName, sPassword);


        //            //    Isuploaded = objFtp.upload(fileName, sDirectory);
        //            //    if (Isuploaded == true & File.Exists(sDirectory))
        //            //    {
        //            //        File.Delete(sDirectory);
        //            //        sDirectory = fileName;
        //            //    }

        //            //}
        //                    //else
        //                    //{
        //                    //    viewstate["documents"] = dtdocs;
        //                    //    showmsgbox("someting went wrong not uploaded your file!");
        //                    //    return;
        //                    //}
        //        }
        //        else
        //        {
        //            ShowMsgBox("Please select Division and Repairer");
        //            return;
        //        }

        //    }
        //    catch (WebException ex)
        //    {
        //        throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
        //    }
        //}
       

        
        protected void grdMaterialMast_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList ddl = ((DropDownList)e.Row.FindControl("cmbMeasurement"));
                    ddl.DataSource = dtMeasurement;
                    ddl.DataTextField = "MD_NAME";
                    ddl.DataValueField = "MD_ID";
                    ddl.DataBind();

                    Label lblMeasure = ((Label)e.Row.FindControl("lblItemType"));
                    CheckBox chk = ((CheckBox)e.Row.FindControl("CheckBx1"));
                    if (lblMeasure.Text.Length > 0)
                    {
                        ddl.SelectedValue = lblMeasure.Text;                        
                        chk.Checked = true;
                    }
                    
                }

                //if (dtMeasurement.Rows.Count > 0)
                //{
                //    foreach (GridViewRow row1 in grdMaterialMast.Rows)
                //    {
                //        DropDownList ddl = ((DropDownList)row1.FindControl("cmbMeasurement"));
                //        ddl.DataSource = dtMeasurement;
                //        ddl.DataTextField = "MD_NAME";
                //        ddl.DataValueField = "MD_ID";
                //        ddl.DataBind();
                //    }

                //    foreach (GridViewRow row2 in grdLabourMast.Rows)
                //    {
                //        DropDownList ddl = ((DropDownList)row2.FindControl("cmbLabMeasurement"));
                //        ddl.DataSource = dtMeasurement;
                //        ddl.DataTextField = "MD_NAME";
                //        ddl.DataValueField = "MD_ID";
                //        ddl.DataBind();
                //    }

                //    foreach (GridViewRow row3 in grdSalvageMast.Rows)
                //    {
                //        DropDownList ddl = ((DropDownList)row3.FindControl("cmbSalMeasurement"));
                //        ddl.DataSource = dtMeasurement;
                //        ddl.DataTextField = "MD_NAME";
                //        ddl.DataValueField = "MD_ID";
                //        ddl.DataBind();
                //    }
                //}
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtFromDate.Text = string.Empty;
                txtToDate.Text = string.Empty;
                cmbCapacity.SelectedIndex = 0;
                cmbDivision.SelectedIndex = 0;
                txtPoDate.Text.Trim();
                txtPoNo.Text = string.Empty;
                cmbwound.SelectedIndex = 0;
                cmbRate.SelectedIndex = 0;
                if (cmbRepairer.SelectedIndex >= 0)
                    cmbRepairer.SelectedIndex = 0;
                if (cmbSupplier.SelectedIndex >= 0)
                    cmbSupplier.SelectedIndex = 0;
                cmbSelectType.SelectedIndex = 0;
                cmbRepairer.Enabled = true;
                cmbSupplier.Enabled = true;
                LoadMaterialDetailes();
                LoadLabourDetailes();
                LoadSalvageDetailes();

            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdLabourMast_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList ddl = ((DropDownList)e.Row.FindControl("cmbLabMeasurement"));
                    ddl.DataSource = dtMeasurement;
                    ddl.DataTextField = "MD_NAME";
                    ddl.DataValueField = "MD_ID";
                    ddl.DataBind();

                    Label lblMeasure = ((Label)e.Row.FindControl("lblLabItemType"));
                    if (lblMeasure.Text.Length > 0)
                    {
                        ddl.SelectedValue = lblMeasure.Text;
                        CheckBox chk = ((CheckBox)e.Row.FindControl("CheckBox2"));
                        chk.Checked = true;
                    }
                }
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdSalvageMast_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList ddl = ((DropDownList)e.Row.FindControl("cmbSalMeasurement"));
                    ddl.DataSource = dtMeasurement;
                    ddl.DataTextField = "MD_NAME";
                    ddl.DataValueField = "MD_ID";
                    ddl.DataBind();

                    Label lblMeasure = ((Label)e.Row.FindControl("lblsalItemType"));
                    if (lblMeasure.Text.Length > 0)
                    {
                        ddl.SelectedValue = lblMeasure.Text;
                        CheckBox chk = ((CheckBox)e.Row.FindControl("CheckBox3"));
                        chk.Checked = true;

                    }
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}
