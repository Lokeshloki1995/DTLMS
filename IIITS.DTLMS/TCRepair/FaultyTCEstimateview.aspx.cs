using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Collections;
using System.Configuration;

namespace IIITS.DTLMS.TCRepair
{
    public partial class FaultyTCEstimateview : System.Web.UI.Page
    {
        string strFormCode = "FaultyTCEstimateview";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }

                lblMessage.Text = string.Empty;
                objSession = (clsSession)Session["clsSession"];
                if (!IsPostBack)
                {

                    // LoadComboFiled();
                    //GetStoreId();

                    // CheckAccessRights("4");

                    LoadFaultTc();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void LoadFaultTc()
        {
            try
            {
                DataTable dt = new DataTable();
                clsDTrRepairActivity objTcFailure = new clsDTrRepairActivity();


                objTcFailure.sOfficeCode = objSession.OfficeCode;

                objTcFailure.sroletype = objSession.sRoleType;

                objTcFailure.UserId = objSession.UserId;
                objTcFailure.sroleid = objSession.RoleId;


                dt = objTcFailure.LoadFaultTC(objTcFailure);
                if (dt.Rows.Count > 0)
                {
                    ViewState["FaultTC"] = dt;
                    grdFaultTC.DataSource = SortDataTable(dt as DataTable, true);

                    grdFaultTC.DataBind();
                    foreach (GridViewRow row in grdFaultTC.Rows)
                    {
                        Label lblstatus = (Label)row.FindControl("lblStatus");
                        if (lblstatus.Text == "Already Sent")
                        {
                            //((CheckBox)row.FindControl("chkSelect")).Enabled = false;
                        }
                    }
                    grdFaultTC.Visible = true;
                    //cmdSend.Visible = true;
                }
                else
                {
                    grdFaultTC.Visible = true;
                   // cmdSend.Visible = false;
                    ViewState["FaultTC"] = dt;
                    grdFaultTC.DataSource = dt;  //sort datatable
                    grdFaultTC.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void grdFaultTC_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdFaultTC.PageIndex;
            DataTable dt = (DataTable)ViewState["FaultTC"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdFaultTC.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdFaultTC.DataSource = dt;
            }
            grdFaultTC.DataBind();
            grdFaultTC.PageIndex = pageIndex;
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
                        if (dataView.Sort.ToString() == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FaultTC"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FaultTC"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["FaultTC"] = dataView.ToTable();
                        }


                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        if (dataView.Sort.ToString() == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FaultTC"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FaultTC"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["FaultTC"] = dataView.ToTable();
                        }

                    }


                }
                return dataView;
            }
            else
            {
                return new DataView();
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

        protected void grdFaultTC_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                //SaveCheckedValues();
                grdFaultTC.PageIndex = e.NewPageIndex;
                //LoadFaultTc();
                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["FaultTC"];
                grdFaultTC.DataSource = dt;
                grdFaultTC.DataBind();
                foreach (GridViewRow rows in grdFaultTC.Rows)
                {
                    Label lblstatus = (Label)rows.FindControl("lblStatus");
                    if (lblstatus.Text == "Already Sent")
                    {
                       // ((CheckBox)rows.FindControl("chkSelect")).Enabled = false;
                    }
                }
               // PopulateCheckedValues();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void grdFaultTC_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "Create" || e.CommandName == "CreateNew") 
                {
                    //bool bAccResult;
                    if (e.CommandName == "CreateNew")
                    {
                        //Check AccessRights
                        // bAccResult = CheckAccessRights("2");
                        //if (bAccResult == false)
                        //{
                        //    return;
                        //}
                    }

                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
               
                    Label lbltcCode = (Label)row.FindControl("lbltcCode");
                    Label lblrestdid = (Label)row.FindControl("lblrestdid");
                   // if(objSession.RoleId=="12")
                    Label lblOfficeCode = (Label)row.FindControl("lblOfficeCode");
                    string[] Arr = new string[2];
                    Arr = CheckAlreadyFaultyEnry(lbltcCode.Text);
                    if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0].ToString());
                        return;
                    }

                    string sTCcode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lbltcCode.Text));
                    string sRestID = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblrestdid.Text));
                    string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblOfficeCode.Text));
                    string sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt("C"));

                    Response.Redirect("FaultyEstimateCreation.aspx?TCcode=" + sTCcode + "&RestId=" + sRestID + "&OfficeCode=" + sOfficeCode + "&ActionType=" + sActionType, false);

                }

                if (e.CommandName == "Preview" || e.CommandName == "Preview")
                {
                    //bool bAccResult;
                    if (e.CommandName == "Preview")
                    {
                        //Check AccessRights
                        // bAccResult = CheckAccessRights("2");
                        //if (bAccResult == false)
                        //{
                        //    return;
                        //}
                    }

                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
               
                    Label lbltcCode = (Label)row.FindControl("lbltcCode");
                    Label lblrestdid = (Label)row.FindControl("lblrestdid");
                    Label lblOfficeCode = (Label)row.FindControl("lblOfficeCode");
                    string[] Arr = new string[3];
                    Arr = GetPreviewReport(lbltcCode.Text);
                    if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0].ToString());
                        return;
                    }

                    string sTCcode = lbltcCode.Text;
                    string sRestID = lblrestdid.Text;
                    string sOfficeCode = lblOfficeCode.Text;
                    string sActionType = HttpUtility.UrlEncode(Genaral.UrlEncrypt("C"));
                    string strParam1;
                    
                        strParam1 = "id=RefinedEstimationSOrepairer&sWFOID=" +Arr[2] + "&sDtrcode=" + sTCcode + "&FailType=" + "1";
                        RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam1 + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                        return;

                     

                    //strParam = "id=RefinedEstimationrepairer&EstimationId=" + EstimationId;
                        RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam1 + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");


                    //Response.Redirect("FaultyEstimateCreation.aspx?TCcode=" + sTCcode + "&RestId=" + sRestID + "&ActionType=" + sActionType, false);

                }
                if (e.CommandName == "Submit")
                {
                    GridViewRow row = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
                    int rowindex = row.RowIndex;
                    Label lblTCId = (Label)grdFaultTC.Rows[rowindex].FindControl("lblTCId");
                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtrCode = (TextBox)row.FindControl("txtDTRCode");
                    TextBox txtDtrSlNo = (TextBox)row.FindControl("txtSlNo");

                    clsDTrRepairActivity objTcRepair = new clsDTrRepairActivity();
                    if (txtDtrCode.Text != "")
                    {
                        objTcRepair.sTcCode = txtDtrCode.Text;
                    }
                    if (txtDtrSlNo.Text != "")
                    {
                        objTcRepair.sTcSlno = txtDtrSlNo.Text;
                    }

                   
                    objTcRepair.sOfficeCode = objSession.OfficeCode;
                    DataTable dt = new DataTable();
                   if (objSession.RoleId=="12")
                    {
                        objTcRepair.UserId = objSession.UserId;
                        objTcRepair.sroleid = objSession.RoleId;
                        //  objTcRepair.sOfficeCode = sOfficeCode;
                        dt = objTcRepair.LoadFaultTC(objTcRepair);
                        dv = dt.DefaultView;
                    }
                    else
                    {
                        // dt = (DataTable)ViewState["FaultTC"];
                        //dv = dt.DefaultView;
                        dt = objTcRepair.LoadFaultTC(objTcRepair);
                        dv = dt.DefaultView;
                    }
                 //   //DataTable dt = objTcRepair.LoadFaultTC(objTcRepair);
                  //  DataTable dt = (DataTable)ViewState["FaultTC"];
                 //   dv = dt.DefaultView;
                //    //sFilter = string.Format("convert(DME_EXISTING_DTR_CODE , 'System.String') Like '%{0}%' ",txttcCode.Text);

                    if (txtDtrCode.Text != "")
                    {
                        sFilter = string.Format("convert(TC_CODE , 'System.String') Like '%{0}%' ", txtDtrCode.Text.Replace("'", "'"));
                        //sFilter = "TC_CODE Like '%" + txtDtrCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtDtrSlNo.Text != "")
                    {
                        sFilter = string.Format("convert(TC_SLNO , 'System.String') Like '%{0}%' ", txtDtrSlNo.Text.Replace("'", "'"));
                        //sFilter += " TC_SLNO Like '%" + txtDtrSlNo.Text.Replace("'", "'") + "%' AND";
                    }

                    if (sFilter.Length > 0)
                    {

                        grdFaultTC.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdFaultTC.DataSource = dv;
                            ViewState["FaultTC"] = dv.ToTable();
                            grdFaultTC.DataBind();
                            foreach (GridViewRow rows in grdFaultTC.Rows)
                            {
                                Label lblstatus = (Label)rows.FindControl("lblStatus");
                                if (lblstatus.Text == "Already Sent")
                                {
                                    ((CheckBox)rows.FindControl("chkSelect")).Enabled = false;
                                }
                            }

                        }
                        else
                        {
                            ViewState["FaultTC"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        dt = objTcRepair.LoadFaultTC(objTcRepair);
                        grdFaultTC.DataSource = dt;
                        grdFaultTC.DataBind();
                        foreach (GridViewRow rows in grdFaultTC.Rows)
                        {
                            Label lblstatus = (Label)rows.FindControl("lblStatus");
                            if (lblstatus.Text == "Already Sent")
                            {
                               // ((CheckBox)rows.FindControl("chkSelect")).Enabled = false;
                            }
                        }
                        ViewState["FaultTC"] = dt;

                    }
                  


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("TC_ID");
                dt.Columns.Add("RESTD_ID");
                dt.Columns.Add("TC_CODE");
                dt.Columns.Add("TC_SLNO");
                dt.Columns.Add("TC_PREV_OFFCODE");
                dt.Columns.Add("TM_NAME");
                dt.Columns.Add("TC_CAPACITY");
                dt.Columns.Add("TC_MANF_DATE");
                dt.Columns.Add("TC_PURCHASE_DATE");
                dt.Columns.Add("TC_WARANTY_PERIOD");
                dt.Columns.Add("TS_NAME");
                dt.Columns.Add("RCOUNT");
                dt.Columns.Add("TC_GUARANTY_TYPE");
                dt.Columns.Add("STATUS");

                grdFaultTC.DataSource = dt;
                grdFaultTC.DataBind();
                int iColCount = grdFaultTC.Rows[0].Cells.Count;
                grdFaultTC.Rows[0].Cells.Clear();
                grdFaultTC.Rows[0].Cells.Add(new TableCell());
                grdFaultTC.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdFaultTC.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }


        protected void grdFailureDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    LinkButton lnkUpdate = (LinkButton)e.Row.FindControl("lnkUpdate");
                    LinkButton lnkCreate = (LinkButton)e.Row.FindControl("lnkCreate");
                   LinkButton lnkCreateDTC = (LinkButton)e.Row.FindControl("lnkCreateTC");
                   // LinkButton lnkCreateDTR = (LinkButton)e.Row.FindControl("lnkCreateDTR");
                    Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                    Label lblTCCode = (Label)e.Row.FindControl("lblTCCode");
                    LinkButton lnkWaiting = (LinkButton)e.Row.FindControl("lnkWaiting");
                    LinkButton lnkEstPrev = (LinkButton)e.Row.FindControl("lnkEstPrev");
                  


                    if (lblStatus.Text == "YES")
                    {
                        lnkUpdate.Visible = true;
                        lnkCreate.Visible = false;
                        //lnkCreateDTC.Enabled = false;
                        //lnkCreateDTR.Enabled = false;
                    }
                    else
                    {
                        lnkUpdate.Visible = false;
                        lnkCreate.Visible = true;
                        //lnkCreateDTC.Enabled = true;
                        //lnkCreateDTR.Enabled = true;

                        // Check DTC in waiting for approval

                        string[] Arr = new string[2];

                       
                        Arr = CheckAlreadyFaultyEnry(lblTCCode.Text);

                        if (Arr[1].ToString() == "2")
                        {
                            lnkWaiting.Visible = true;
                            lnkEstPrev.Visible = true;
                            lnkCreate.Visible = false;
                           // lnkCreateDTC.Enabled = false;
                           // lnkCreateDTR.Enabled = false;
                        }
                        else
                        {
                            lnkWaiting.Visible = false;
                            lnkEstPrev.Visible = false;

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

        public string[] CheckAlreadyFaultyEnry(string sTCCode)
        {
            string[] Arr = new string[2];
            try
            {
                clsApproval objApproval = new clsApproval();
                bool bResult = objApproval.CheckAlreadyExistEntryfaulty(sTCCode, "71");
                if (bResult == true)
                {
                    Arr[0] = " " + sTCCode + ", Waiting for Approval";
                    Arr[1] = "2";
                    return Arr;

                }

                bResult = objApproval.CheckAlreadyExistEntryfaulty(sTCCode, "72");
                if (bResult == true)
                {
                    Arr[0] = "" + sTCCode + ", Waiting for Approval";
                    Arr[1] = "2";
                    return Arr;

                }


                Arr[0] = "Success";
                Arr[1] = "0";
                return Arr;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;

            }
        }
         public string[] GetPreviewReport(string sTCCode)
        {
            string[] Arr = new string[3];
            try
            {
                clsApproval objApproval = new clsApproval();
                Arr = objApproval.GetApprovedpreview(sTCCode, "71");
                if (Arr[1] == "2")
                {
                    Arr[0] = "Data Not Available!";
                    Arr[1] = "2";
                    return Arr;
                }
                return Arr;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;

            }
        }
        private void ShowMsgBox(string sMsg)
        {
            try
            {
                String sShowMsg = string.Empty;
                sShowMsg = "<script language=javascript> alert ('" + sMsg + "')</script>";
                this.Page.RegisterStartupScript("Msg", sShowMsg);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


    }
}