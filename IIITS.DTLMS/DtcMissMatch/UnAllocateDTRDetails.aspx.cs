using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.IO;
using System.Configuration;
using System.Data;

namespace IIITS.DTLMS.DtcMissMatch
{
    public partial class UnAllocateDTRDetails : System.Web.UI.Page
    {
        string strFormCode = "UnAllocateDTRDetails";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
            {
                Response.Redirect("~/Login.aspx", false);
            }
            objSession = (clsSession)Session["clsSession"];
            if (!IsPostBack)
            {
                if (Session["OffCode"] != null && Session["OffCode"].ToString() != "")
                {
                    hdfOffCode.Value = Session["OffCode"].ToString();
                }
                else
                {
                    hdfOffCode.Value = objSession.OfficeCode;
                }

                LoadUnAllocateDTRDetails();
            }
        }

        public void LoadUnAllocateDTRDetails()
        {
            try
            {
                clsDtcMissMatchEntry objMisMatch = new clsDtcMissMatchEntry();
                DataTable dtLoadDetails = new DataTable();
                dtLoadDetails = objMisMatch.LoadUnAllocateDTRDetails(hdfOffCode.Value);
                grdUnAllocateDetails.DataSource = dtLoadDetails;
                grdUnAllocateDetails.DataBind();
                ViewState["UnAllocateDetails"] = dtLoadDetails;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdUnAllocateDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdUnAllocateDetails.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["UnAllocateDetails"];
                grdUnAllocateDetails.DataSource = SortDataTable(dtComplete as DataTable, true);
                grdUnAllocateDetails.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdUnAllocateDetails_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdUnAllocateDetails.PageIndex;
            DataTable dt = (DataTable)ViewState["UnAllocateDetails"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {

                grdUnAllocateDetails.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdUnAllocateDetails.DataSource = dt;
            }
            grdUnAllocateDetails.DataBind();
            grdUnAllocateDetails.PageIndex = pageIndex;
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
                        ViewState["UnAllocateDetails"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["UnAllocateDetails"] = dataView.ToTable();

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

        //public void ExportToExcel(DataTable dtCompleteDetails)
        //{
        //    try
        //    {
        //        Response.ClearContent();
        //        Response.Buffer = true;
        //        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "UnAllocated_Details.xls"));
        //        Response.ContentType = "application/ms-excel";
        //        StringWriter sw = new StringWriter();
        //        HtmlTextWriter htw = new HtmlTextWriter(sw);
        //        grdUnAllocateDetails.AllowPaging = false;
        //        LoadUnAllocateDTRDetails();
        //        //Change the Header Row back to white color
        //        grdUnAllocateDetails.HeaderRow.Style.Add("background-color", "#FFFFFF");
        //        //Applying stlye to gridview header cells
        //        for (int i = 0; i < grdUnAllocateDetails.HeaderRow.Cells.Count; i++)
        //        {
        //            grdUnAllocateDetails.HeaderRow.Cells[i].Style.Add("background-color", "#709eea");
        //        }
        //        grdUnAllocateDetails.RenderControl(htw);
        //        Response.Write(sw.ToString());
        //        Response.End();
        //    }

        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ExportToExcel");
        //    }
        //}

        //protected void cmbExport_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DataTable dtComplete = new DataTable();
        //        dtComplete = (DataTable)ViewState["UnAllocateDetails"];
        //        ExportToExcel(dtComplete);
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbExport_Click");
        //    }
        //}


        protected void cmbExport_Click(object sender, EventArgs e)
        {

            //clsDtcMissMatchEntry objMisMatch = new clsDtcMissMatchEntry();
            //DataTable dtLoadDetails = new DataTable();
            //dtLoadDetails = objMisMatch.LoadUnAllocateDTRDetails(hdfOffCode.Value);

            DataTable dtLoadDetails = (DataTable)ViewState["UnAllocateDetails"];

            if (dtLoadDetails.Rows.Count > 0)
            {

                dtLoadDetails.Columns["DME_EXISTING_DTR_CODE"].ColumnName = "TC CODE";
              //  dtLoadDetails.Columns["DT_CODE"].ColumnName = "DTC CODE";
                dtLoadDetails.Columns["DME_ENTRY_DATE"].ColumnName = "UNALLOCATED DATE";
                dtLoadDetails.Columns["TC_LOCATION_ID"].ColumnName = "LOCATION CODE";
                dtLoadDetails.Columns["SECTION"].ColumnName = "SECTION";
             
                //dtLoadDetails.Columns["FEEDER NAME"].SetOrdinal(0);

                List<string> listtoRemove = new List<string> { "DT_CODE" };
                string filename = "UnAllocateDTRDetails" + DateTime.Now + ".xls";
                string pagetitle = "UnAllocated DTR Details";

                Genaral.getexcel(dtLoadDetails, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");
                ShowEmptyGrid();
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
        protected void grdUnAllocateDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txttcCode = (TextBox)row.FindControl("txttcCode");

                    DataTable dt = (DataTable)ViewState["UnAllocateDetails"];
                    dv = dt.DefaultView;

                    if (txttcCode.Text != "")
                    {
                        //sFilter = " DME_EXISTING_DTR_CODE like '%" + txttcCode.Text.Replace("'", "'") + "%' ";

                        sFilter = string.Format("convert(DME_EXISTING_DTR_CODE , 'System.String') Like '%{0}%' ",
                             txttcCode.Text);
                    }
                    if (sFilter.Length > 0)
                    {
                        //sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdUnAllocateDetails.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            ViewState["UnAllocateDetails"] = dv.ToTable();
                            grdUnAllocateDetails.DataSource = dv;
                          
                            grdUnAllocateDetails.DataBind();

                        }
                        else
                        {
                            ViewState["UnAllocateDetails"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadUnAllocateDTRDetails();
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
                dt.Columns.Add("DT_CODE");
                dt.Columns.Add("DME_EXISTING_DTR_CODE");
                dt.Columns.Add("DME_ENTRY_DATE");
                dt.Columns.Add("TC_LOCATION_ID");
                dt.Columns.Add("SECTION");
                grdUnAllocateDetails.DataSource = dt;
                grdUnAllocateDetails.DataBind();

                int iColCount = grdUnAllocateDetails.Rows[0].Cells.Count;
                grdUnAllocateDetails.Rows[0].Cells.Clear();
                grdUnAllocateDetails.Rows[0].Cells.Add(new TableCell());
                grdUnAllocateDetails.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdUnAllocateDetails.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }
    }
}