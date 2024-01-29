using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using DocumentFormat.OpenXml.ExtendedProperties;

namespace IIITS.DTLMS.MasterForms
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        string strFormCode = "InwardView";
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
                    // CheckAccessRights("2");
                     
                    LoadCombo();

                    if (Request.QueryString["URLRedirect"] != null && Request.QueryString["URLRedirect"].ToString() != "" && objSession.OfficeCode.Length < 5)
                    {
                        Details.Style.Add("display", "block");
                        LoadHeirachy();
                    }
                    //this.BindDummyRow();
                    LoadTcMasterDetails();

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmdNew_Click(object sender, EventArgs e)
        {
            try
            {
                //Check AccessRights
                bool bAccResult = CheckAccessRights("2");
                if (bAccResult == false)
                {
                    return;
                }
                Response.Redirect("NewTcMaster.aspx", false);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void LoadTcMasterDetails()
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            try
            {
                clsTcMaster objTCmaster = new clsTcMaster();
                if (Request.QueryString["URLRedirect"] != null && Request.QueryString["URLRedirect"].ToString() != "" && objSession.OfficeCode.Length < 5)
                {
                    if (cmbOMSection.SelectedIndex > 0)
                    {
                        objTCmaster.sOfficeCode = cmbOMSection.SelectedValue;
                    }
                    else if (cmbSubDiv.SelectedIndex > 0)
                    {
                        objTCmaster.sOfficeCode = cmbSubDiv.SelectedValue;
                    }
                    else if (cmbDiv.SelectedIndex > 0)
                    {
                        objTCmaster.sOfficeCode = cmbDiv.SelectedValue;
                    }
                    else if (cmbCircle.SelectedIndex > 0)
                    {
                        objTCmaster.sOfficeCode = cmbCircle.SelectedValue;
                    }
                    else if (cmbZone.SelectedIndex > 0)
                    {
                        objTCmaster.sOfficeCode = cmbZone.SelectedValue;
                    }
                    else
                    {
                        objTCmaster.sOfficeCode = "";
                    }
                }
                else
                {
                    objTCmaster.sOfficeCode = objSession.OfficeCode;

                }

                objTCmaster.sroletype = objSession.sRoleType;

                if (cmbMake.SelectedIndex > 0)
                {
                    objTCmaster.sTcMakeId = cmbMake.SelectedItem.Text;
                }
                if (cmbCapacity.SelectedIndex > 0)
                {
                    objTCmaster.sTcCapacity = cmbCapacity.SelectedValue;
                }
                if (cmbDivision.SelectedIndex > 0)
                {
                    objTCmaster.sDivId = cmbDivision.SelectedValue;
                }
                if (cmbstore.SelectedIndex > 0)
                {
                    objTCmaster.sLocationId = cmbstore.SelectedValue;
                }

                dt = objTCmaster.LoadInwardTcMaster(objTCmaster);

                //ds.Tables.Add(dt);
                //daresult = DataSetToJSON(ds);
                if (dt.Rows.Count == 0)
                {
                    ShowEmptyGrid();
                    ViewState["TC"] = dt;
                }
                else
                {
                    grdTcMaster.DataSource = dt;
                    grdTcMaster.DataBind();
                    ViewState["TC"] = dt;
                }
                if (Request.QueryString["URLRedirect"] != null && Request.QueryString["URLRedirect"].ToString() != "" && objSession.OfficeCode.Length < 5)
                {
                    lblTotalDTr.Text = "Total Transformer Count : " + objTCmaster.getInwardTCCount(objTCmaster.sOfficeCode, objTCmaster.sroletype, objTCmaster.sDivId,objTCmaster.sTcMakeId, objTCmaster.sTcCapacity);
                }
                else
                {
                    //lblTotalDTr.Text = "Total Transformer Count : " + objTCmaster.getInwardTCCount(objTCmaster.sOfficeCode, objTCmaster.sroletype, objTCmaster.sDivId);
                    lblTotalDTr.Text = "Total Transformer Count : " + objTCmaster.getInwardTCCount(objTCmaster.sOfficeCode, objTCmaster.sroletype, objTCmaster.sDivId,objTCmaster.sTcMakeId,objTCmaster.sTcCapacity);
                }

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdTcMaster_Sorting(object sender, GridViewSortEventArgs e)
        {

            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdTcMaster.PageIndex;
            DataTable dt = (DataTable)ViewState["TC"];
            string sortingDirection = string.Empty;

            //DataView sortedView = new DataView(dt);
            //sortedView.Sort = e.SortExpression + " " + sortingDirection;
            if (dt.Rows.Count > 0)
            {
                grdTcMaster.DataSource = SortDataTable(dt as DataTable, false);
                //grdTcMaster.DataSource = dt.AsEnumerable().OrderBy(x => x[sortExpression]);
            }
            else
            {
                grdTcMaster.DataSource = dt;
            }

            grdTcMaster.DataBind();


            grdTcMaster.PageIndex = pageIndex;          
        }

        protected void grdTcMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "create")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                    string strTcId = ((Label)row.FindControl("lblTcId")).Text;
                    strTcId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(strTcId));
                    Response.Redirect("TcMaster.aspx?TCId=" + strTcId + "", false);
                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtTCCode = (TextBox)row.FindControl("txtTCCode");
                    TextBox txtTCSlno = (TextBox)row.FindControl("txtTCSlno");
                    TextBox txtMake = (TextBox)row.FindControl("txtMake");
                    TextBox txtpono = (TextBox)row.FindControl("txtpono");
                    TextBox txtdino = (TextBox)row.FindControl("txtdino");


                    DataTable dt = (DataTable)ViewState["TC"];
                    dv = dt.DefaultView;

                    if (objSession.OfficeCode == "" || objSession.OfficeCode == null)
                    {
                        if(dt.Rows.Count > 0)
                        {
                            clsTcMaster objTcdetails = new clsTcMaster();
                            objTcdetails.sTcCode = txtTCCode.Text;
                            objTcdetails.sTcSlNo = txtTCSlno.Text;
                            objTcdetails.sTcMakeId = txtMake.Text;
                            objTcdetails.sPurchaseno = txtpono.Text;
                            objTcdetails.sDino = txtdino.Text;
                            dt = objTcdetails.LoadInwardTcMaster(objTcdetails);
                            grdTcMaster.DataSource = dt;
                            grdTcMaster.DataBind();
                        }
                        else
                        {
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        if (txtTCCode.Text != "")
                        {
                            sFilter = "Convert(TC_CODE,'System.String')   Like '%" + txtTCCode.Text.Replace("'", "'") + "%' AND";
                        }
                        if (txtTCSlno.Text != "")
                        {
                            sFilter += " TC_SLNO Like '%" + txtTCSlno.Text.Replace("'", "'") + "%' AND";
                        }
                        if (txtMake.Text != "")
                        {
                            //sFilter += " TC_MAKE_ID Like '%" + txtMake.Text.Replace("'", "'").ToUpper() + "%' AND";
                            sFilter += " Convert(TC_MAKE,'System.String') Like '" + txtMake.Text.Replace("'", "'").ToUpper() + "%' AND";
                        }
                        if (txtpono.Text != "")
                        {
                            sFilter += " TC_PO_NO Like '%" + txtpono.Text.Replace("'", "'") + "%' AND";
                        }
                        if (txtdino.Text != "")
                        {
                            sFilter += " TC_DI_NO Like '%" + txtdino.Text.ToUpper() + "%' AND";
                        }
                        if (sFilter.Length > 0)
                        {
                            sFilter = sFilter.Remove(sFilter.Length - 3);
                            grdTcMaster.PageIndex = 0;
                            dv.RowFilter = sFilter;
                            if (dv.Count > 0)
                            {
                                grdTcMaster.DataSource = dv;
                                ViewState["TC"] = dv.ToTable();
                                grdTcMaster.DataBind();

                            }
                            else
                            {
                                ViewState["TC"] = dv.ToTable();
                                ShowEmptyGrid();
                            }
                        }
                        else
                        {
                            LoadTcMasterDetails();
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

        protected void grdTcMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                //grdTcMaster.PageIndex = e.NewPageIndex;
                //DataTable dt = (DataTable)ViewState["TC"];
                //grdTcMaster.DataSource = SortDataTable(dt as DataTable, true);
                //grdTcMaster.DataBind();
                grdTcMaster.PageIndex = e.NewPageIndex;
                //Bind the results back
                DataTable dt = (DataTable)ViewState["TC"];
                grdTcMaster.DataSource = SortDataTable(dt as DataTable, true);
                grdTcMaster.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void Export_clickTCMaster(object sender, EventArgs e)
        {
            if (ViewState["TC"] != null)
            {
                DataTable dt = (DataTable)ViewState["TC"];

                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "DTR CODE";
                    dt.Columns["TC_SLNO"].ColumnName = "DTR SLNO  ";
                  //   dt.Columns["DIV_NAME"].ColumnName = "DIVISION NAME";
                    dt.Columns["TC_MAKE"].ColumnName = "MAKE NAME";
                    dt.Columns["TC_CAPACITY"].ColumnName = "CAPACITY(IN KVA)";
                    dt.Columns["TC_LIFE_SPAN"].ColumnName = "LIFE SPAN";
                 //   dt.Columns["TC_ALT_NO"].ColumnName = "TC ALLOTMENT NUMBER";
                 //   dt.Columns["DIVISION NAME"].SetOrdinal(5);
                    dt.Columns["MAKE NAME"].SetOrdinal(4);

                    List<string> listtoRemove = new List<string> { "TC_ID", "TC_MAKE_ID" };
                    string filename = "TCDetails" + DateTime.Now + ".xls";
                    string HeadTitle = "TC Details";

                    Genaral.getexcel(dt, listtoRemove, filename, HeadTitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }
            }
            else
            {
                ShowMsgBox("No record found");
                ShowEmptyGrid();
            }
            
        }
        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                LoadTcMasterDetails();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        //protected void cmbCurrentLoc_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (objSession.OfficeCode == "")
        //        {
        //            if (cmbCurrentLoc.SelectedValue != "2")
        //            {
        //                Genaral.Load_Combo("SELECT \"SM_ID\",\"SM_NAME\" FROM \"TBLSTOREMAST\"", "--Select--", cmbstore);
        //                divStore.Visible = true;
        //            }
        //            else
        //            {
        //                //cmbstore.SelectedIndex = 0;
        //                divStore.Visible = false;
        //            }
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}

        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("TC_ID");
                dt.Columns.Add("TC_CODE");
                dt.Columns.Add("TC_SLNO");
              //  dt.Columns.Add("DIV_NAME");
                dt.Columns.Add("TC_MAKE");
                dt.Columns.Add("TC_MAKE_ID");
                dt.Columns.Add("TC_CAPACITY");
                dt.Columns.Add("TC_LIFE_SPAN");
                dt.Columns.Add("TC_PO_NO");
                dt.Columns.Add("TC_DI_NO");

                grdTcMaster.DataSource = dt;
                grdTcMaster.DataBind();

                int iColCount = grdTcMaster.Rows[0].Cells.Count;
                grdTcMaster.Rows[0].Cells.Clear();
                grdTcMaster.Rows[0].Cells.Add(new TableCell());
                grdTcMaster.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdTcMaster.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbZone.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE CAST(\"CM_ZO_ID\" AS TEXT) = '" + cmbZone.SelectedValue + "'", "--Select--", cmbCircle);
                    cmbDiv.Items.Clear();
                }
                else
                {
                    cmbCircle.Items.Clear();
                    cmbDiv.Items.Clear();
                    cmbSubDiv.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE \"DIV_CICLE_CODE\"='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDiv);
                    cmbSubDiv.Items.Clear();
                }
                else
                {
                    cmbDiv.Items.Clear();
                    cmbSubDiv.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDiv.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE \"SD_DIV_CODE\" ='" + cmbDiv.SelectedValue + "'", "--Select--", cmbSubDiv);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbSubDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubDiv.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE \"OM_SUBDIV_CODE\"='" + cmbSubDiv.SelectedValue + "'", "--Select--", cmbOMSection);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void cmbCapacity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadTcMasterDetails();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
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
                        ViewState["TC"] = dataView.ToTable();


                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["TC"] = dataView.ToTable();

                    }


                }
                return dataView;
            }
            else
            {
                return new DataView();
            }




        }

        private string GetSortDirection()
        {
            switch (GridViewSortDirection)
            {
                case "ASC":
                    GridViewSortDirection = "DESC";
                    // grdTcMaster.HeaderStyle.CssClass = "descending";
                    break;
                case "DESC":
                    GridViewSortDirection = "ASC";
                    //grdTcMaster.HeaderStyle.CssClass = "ascending";
                    break;
            }


            return GridViewSortDirection;
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
        protected void LoadHeirachy()
        {
            if (objSession.OfficeCode.Length > 0)
            {
                cmbZone.SelectedValue = objSession.OfficeCode.Substring(0, 1);
                cmbZone_SelectedIndexChanged(this, EventArgs.Empty);
                cmbZone.Enabled = false;
            }

            if (objSession.OfficeCode.Length > 1)
            {
                cmbCircle.SelectedValue = objSession.OfficeCode.Substring(0, 2);
                cmbCircle_SelectedIndexChanged(this, EventArgs.Empty);
                cmbCircle.Enabled = false;
            }

            if (objSession.OfficeCode.Length > 2)
            {
                cmbDiv.SelectedValue = objSession.OfficeCode.Substring(0, 3);
                cmbDiv_SelectedIndexChanged(this, EventArgs.Empty);
                cmbDiv.Enabled = false;
            }

            if (objSession.OfficeCode.Length > 3)
            {
                cmbSubDiv.SelectedValue = objSession.OfficeCode.Substring(0, 4);
                cmbSubDiv.Enabled = false;
            }
            if (objSession.OfficeCode.Length > 4)
            {
                cmbOMSection.SelectedValue = objSession.OfficeCode;
                cmbOMSection.Enabled = false;
            }
        }

        public void LoadCombo()
        {
            try
            {
                string sOfficeCode = string.Empty;
                if (objSession.OfficeCode.Length > 3)
                {
                    sOfficeCode = objSession.OfficeCode.Substring(0, 4);
                }
                Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone);
                Genaral.Load_Combo("SELECT \"TM_ID\",\"TM_NAME\" FROM \"TBLTRANSMAKES\" ORDER BY \"TM_NAME\"", "-Select-", cmbMake);
                Genaral.Load_Combo("SELECT \"MD_NAME\",\"MD_NAME\" from \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='C' ORDER BY \"MD_ORDER_BY\"", "-Select-", cmbCapacity);
               // Genaral.Load_Combo("SELECT \"MD_ID\",\"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='TCL' ORDER BY \"MD_ORDER_BY\" ", "--Select--", cmbCurrentLoc);
                if (objSession.RoleId == "2" || objSession.RoleId == "5")
                {
                    //Genaral.Load_Combo("select  \"DIV_ID\",\"DIV_NAME\"  from \"TBLSTOREOFFCODE\", \"TBLSTOREMAST\",\"TBLDIVISION\" ,\"TBLTCMASTER\" where  \"STO_SM_ID\"=\"SM_ID\" AND \"STO_OFF_CODE\"=\"DIV_CODE\"  AND \"TC_ALT_NO\" is not null AND \"TC_DIV_ID\"=cast(\"DIV_ID\" as text) AND  \"SM_ID\"=" + objSession.OfficeCode + "  GROUP BY  \"DIV_ID\",\"DIV_NAME\"", "--Select--", cmbDivision);
                    Genaral.Load_Combo("select  \"DIV_ID\",\"DIV_NAME\"  from \"TBLDIVISION\" inner join \"TBLSTOREMAST\"  on \"DIV_ID\" = \"SM_ID\" where \"SM_ID\"=" + objSession.OfficeCode + "  GROUP BY  \"DIV_ID\",\"DIV_NAME\"", "--Select--", cmbDivision);
                }
                else
                {
                   // Genaral.Load_Combo("select  \"DIV_ID\",\"DIV_NAME\"  from \"TBLDIVISION\" GROUP BY  \"DIV_ID\",\"DIV_NAME\"", "--Select--", cmbDivision);
                    Genaral.Load_Combo("select  \"SM_ID\",\"SM_NAME\"  from \"TBLSTOREMAST\"", "--Select--", cmbDivision);
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

        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "Inward";
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