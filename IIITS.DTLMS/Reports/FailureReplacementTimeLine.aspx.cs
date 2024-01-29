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
using ClosedXML.Excel;
using System.Drawing;

namespace IIITS.DTLMS.DtcMissMatch
{
    public partial class FailureReplacementTimeLine : System.Web.UI.Page
    {
        string strFormCode = "FailureReplacementTimeLine";
        clsSession objSession;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }

                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");

                CalendarExtender4.EndDate = System.DateTime.Now;
                CalendarExtender3.EndDate = System.DateTime.Now;
                #region UnUsed Method

                //if (!IsPostBack)
                //{
                //    clsFailureRepTImeLine objFailureRep = new clsFailureRepTImeLine();
                //    DataTable dtLoadFailureDetails = new DataTable();
                //    dtLoadFailureDetails = objFailureRep.LoadZoneDetails(txtFromDate.Text,txtToDate.Text);
                //    if (dtLoadFailureDetails.Rows.Count > 0)
                //    {
                //        //GrdZone.DataSource = dtLoadFailureDetails;
                //        //GrdZone.DataBind();
                //    }
                //    else
                //    {
                //        ShowMsgBox("No Records Found");
                //    }
                //}
                #endregion

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        #region UnUsed Method
        //protected void GrdZone_OnRowDataBound(object sender, GridViewRowEventArgs e)

        //{
        //    try
        //    {
        //        if (e.Row.RowType == DataControlRowType.DataRow)
        //        {
        //            Label lblZone = (Label)e.Row.FindControl("lblZone");
        //            GridView gv_Child = (GridView)e.Row.FindControl("GrdCircle");
        //            string txtZone = lblZone.Text;
        //            clsFailureRepTImeLine objFailureRep = new clsFailureRepTImeLine();
        //            DataTable dtLoadFailureDetails = new DataTable();
        //            dtLoadFailureDetails = objFailureRep.LoadCircleDetails(txtZone,txtFromDate.Text, txtToDate.Text);
        //            if (dtLoadFailureDetails.Rows.Count > 0)
        //            {

        //                gv_Child.DataSource = dtLoadFailureDetails;

        //                gv_Child.DataBind();

        //                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
        //                TableHeaderCell cell = new TableHeaderCell();
        //                cell.Text = "View";
        //                cell.Width = 80;
        //                cell.ColumnSpan = 1;
        //                row.Controls.Add(cell);

        //                cell = new TableHeaderCell();
        //                cell.ColumnSpan = 1;
        //                cell.Text = "Circle";
        //                cell.Width = 100;
        //                row.Controls.Add(cell);

        //                cell = new TableHeaderCell();
        //                cell.ColumnSpan = 2;
        //                cell.Text = "Within1Day";
        //                row.Controls.Add(cell);

        //                cell = new TableHeaderCell();
        //                cell.ColumnSpan = 2;
        //                cell.Text = "Between 1to7 Days";
        //                row.Controls.Add(cell);


        //                cell = new TableHeaderCell();
        //                cell.ColumnSpan = 2;
        //                cell.Text = "Between 7to15 Days";
        //                row.Controls.Add(cell);

        //                cell = new TableHeaderCell();
        //                cell.ColumnSpan = 2;
        //                cell.Text = "Between 15to30 Days";
        //                row.Controls.Add(cell);

        //                cell = new TableHeaderCell();
        //                cell.ColumnSpan = 2;
        //                cell.Text = "Above 30 Days";
        //                row.Controls.Add(cell);


        //                cell = new TableHeaderCell();
        //                cell.ColumnSpan = 2;
        //                cell.Text = "Total";
        //                row.Controls.Add(cell);

        //                row.BackColor = ColorTranslator.FromHtml("#3AC0F2");
        //                gv_Child.HeaderRow.Parent.Controls.AddAt(0, row);
        //            }
        //            else
        //            {
        //                ShowMsgBox("No Records Found");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}


        //protected void GrdCircle_OnRowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Row.RowType == DataControlRowType.DataRow)
        //        {
        //            Label lblCircle = (Label)e.Row.FindControl("lblCircle");
        //           // GridView gv_Child = (GridView)e.Row.FindControl("grdDivision");
        //            GridView gv_NestedChild = (GridView)e.Row.FindControl("grdDivision");
        //            string txtcircle = lblCircle.Text;
        //            clsFailureRepTImeLine objFailureRep = new clsFailureRepTImeLine();
        //            DataTable dtLoadFailureDetails = new DataTable();
        //            dtLoadFailureDetails = objFailureRep.LoadDiviSionDetails(txtcircle,txtFromDate.Text, txtToDate.Text);
        //            if (dtLoadFailureDetails.Rows.Count > 0)
        //            {

        //                gv_NestedChild.DataSource = dtLoadFailureDetails;

        //                gv_NestedChild.DataBind();

        //                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
        //                TableHeaderCell cell = new TableHeaderCell();
        //                cell.Text = "View";
        //                cell.Width = 100;
        //                cell.ColumnSpan = 1;
        //                row.Controls.Add(cell);

        //                cell = new TableHeaderCell();
        //                cell.ColumnSpan = 1;
        //                cell.Text = "Division";
        //                cell.Width = 100;
        //                row.Controls.Add(cell);

        //                cell = new TableHeaderCell();
        //                cell.ColumnSpan = 2;
        //                cell.Text = "Within1Day";
        //                row.Controls.Add(cell);

        //                cell = new TableHeaderCell();
        //                cell.ColumnSpan = 2;
        //                cell.Text = "Between 1to7 Days";
        //                row.Controls.Add(cell);


        //                cell = new TableHeaderCell();
        //                cell.ColumnSpan = 2;
        //                cell.Text = "Between 7to15 Days";
        //                row.Controls.Add(cell);

        //                cell = new TableHeaderCell();
        //                cell.ColumnSpan = 2;
        //                cell.Text = "Between 15to30 Days";
        //                row.Controls.Add(cell);

        //                cell = new TableHeaderCell();
        //                cell.ColumnSpan = 2;
        //                cell.Text = "Above 30 Days";
        //                row.Controls.Add(cell);


        //                cell = new TableHeaderCell();
        //                cell.ColumnSpan = 2;
        //                cell.Text = "Total";
        //                row.Controls.Add(cell);

        //                row.BackColor = ColorTranslator.FromHtml("#3AC0F2");
        //                gv_NestedChild.HeaderRow.Parent.Controls.AddAt(0, row);

        //            }
        //            else
        //            {
        //                ShowMsgBox("No Records Found");
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}
        //protected void grdDivision_OnRowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Row.RowType == DataControlRowType.DataRow)
        //        {
        //            Label lblDivision = (Label)e.Row.FindControl("lblDivision");
        //            GridView gv_NestedChild = (GridView)e.Row.FindControl("grdSubdivision");
        //            string txtdivision = lblDivision.Text;

        //            clsFailureRepTImeLine objFailureRep = new clsFailureRepTImeLine();
        //            DataTable dtLoadFailureDetails = new DataTable();
        //            dtLoadFailureDetails = objFailureRep.LoadSubDiviSionDetails(txtdivision, txtFromDate.Text, txtToDate.Text);
        //            gv_NestedChild.DataSource = dtLoadFailureDetails;

        //            gv_NestedChild.DataBind();



        //            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
        //            TableHeaderCell cell = new TableHeaderCell();
        //            cell.Text = "View";
        //            cell.Width = 150;
        //            cell.ColumnSpan = 1;
        //            row.Controls.Add(cell);

        //            cell = new TableHeaderCell();
        //            cell.ColumnSpan = 1;
        //            cell.Text = "SubDivision";
        //            cell.Width=100;
        //            row.Controls.Add(cell);

        //            cell = new TableHeaderCell();
        //            cell.ColumnSpan = 2;
        //            cell.Text = "Within1Day";
        //            row.Controls.Add(cell);

        //            cell = new TableHeaderCell();
        //            cell.ColumnSpan = 2;
        //            cell.Text = "Between 1to7 Days";
        //            row.Controls.Add(cell);


        //            cell = new TableHeaderCell();
        //            cell.ColumnSpan = 2;
        //            cell.Text = "Between 7to15 Days";
        //            row.Controls.Add(cell);

        //            cell = new TableHeaderCell();
        //            cell.ColumnSpan = 2;
        //            cell.Text = "Between 15to30 Days";
        //            row.Controls.Add(cell);

        //            cell = new TableHeaderCell();
        //            cell.ColumnSpan = 2;
        //            cell.Text = "Above 30 Days";
        //            row.Controls.Add(cell);


        //            cell = new TableHeaderCell();
        //            cell.ColumnSpan = 2;
        //            cell.Text = "Total";
        //            row.Controls.Add(cell);

        //            row.BackColor = ColorTranslator.FromHtml("#3AC0F2");
        //            gv_NestedChild.HeaderRow.Parent.Controls.AddAt(0, row);


        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}




        //protected void grdSubdivision_OnRowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Row.RowType == DataControlRowType.DataRow)
        //        {
        //            Label lblSubDivision = (Label)e.Row.FindControl("lblSubDivision");
        //            GridView gv_NestedChild = (GridView)e.Row.FindControl("grdSection");
        //            string txtsubdivision = lblSubDivision.Text;

        //            clsFailureRepTImeLine objFailureRep = new clsFailureRepTImeLine();
        //            DataTable dtLoadFailureDetails = new DataTable();
        //            dtLoadFailureDetails = objFailureRep.LoadSectionDetails(txtsubdivision, txtFromDate.Text, txtToDate.Text);
        //            gv_NestedChild.DataSource = dtLoadFailureDetails;
        //            gv_NestedChild.DataBind();


        //            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
        //            TableHeaderCell cell = new TableHeaderCell();
        //            cell.Text = "View";
        //            cell.Width = 200;
        //            cell.ColumnSpan = 1;
        //            row.Controls.Add(cell);

        //            cell = new TableHeaderCell();
        //            cell.ColumnSpan = 1;
        //            cell.Text = "Section";
        //            cell.Width = 300;
        //            row.Controls.Add(cell);

        //            cell = new TableHeaderCell();
        //            cell.ColumnSpan = 2;
        //            cell.Text = "Within1Day";
        //            row.Controls.Add(cell);

        //            cell = new TableHeaderCell();
        //            cell.ColumnSpan = 2;
        //            cell.Text = "Between 1to7 Days";
        //            row.Controls.Add(cell);


        //            cell = new TableHeaderCell();
        //            cell.ColumnSpan = 2;
        //            cell.Text = "Between 7to15 Days";
        //            row.Controls.Add(cell);

        //            cell = new TableHeaderCell();
        //            cell.ColumnSpan = 2;
        //            cell.Text = "Between 15to30 Days";
        //            row.Controls.Add(cell);

        //            cell = new TableHeaderCell();
        //            cell.ColumnSpan = 2;
        //            cell.Text = "Above 30 Days";
        //            row.Controls.Add(cell);


        //            cell = new TableHeaderCell();
        //            cell.ColumnSpan = 2;
        //            cell.Text = "Total";
        //            row.Controls.Add(cell);

        //            row.BackColor = ColorTranslator.FromHtml("#3AC0F2");
        //            gv_NestedChild.HeaderRow.Parent.Controls.AddAt(0, row);

        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}



        //protected void grdSection_OnRowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Row.RowType == DataControlRowType.DataRow)
        //        {
        //            Label lblSection = (Label)e.Row.FindControl("lblSection");
        //            GridView gv_NestedChild = (GridView)e.Row.FindControl("grdCategory");
        //            string  txtsection = lblSection.Text;

        //            clsFailureRepTImeLine objFailureRep = new clsFailureRepTImeLine();
        //            DataTable dtLoadFailureDetails = new DataTable();
        //            dtLoadFailureDetails = objFailureRep.LoadCategoryWiseDetails(txtsection, txtFromDate.Text, txtToDate.Text);
        //            gv_NestedChild.DataSource = dtLoadFailureDetails;
        //            gv_NestedChild.DataBind();


        //            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
        //            TableHeaderCell cell = new TableHeaderCell();
        //            cell.Text = "Category";
        //            cell.Width = 200;
        //            cell.ColumnSpan = 1;
        //            row.Controls.Add(cell);



        //            cell = new TableHeaderCell();
        //            cell.ColumnSpan = 2;
        //            cell.Text = "Within1Day";
        //            row.Controls.Add(cell);

        //            cell = new TableHeaderCell();
        //            cell.ColumnSpan = 2;
        //            cell.Text = "Between 1to7 Days";
        //            row.Controls.Add(cell);


        //            cell = new TableHeaderCell();
        //            cell.ColumnSpan = 2;
        //            cell.Text = "Between 7to15 Days";
        //            row.Controls.Add(cell);

        //            cell = new TableHeaderCell();
        //            cell.ColumnSpan = 2;
        //            cell.Text = "Between 15to30 Days";
        //            row.Controls.Add(cell);

        //            cell = new TableHeaderCell();
        //            cell.ColumnSpan = 2;
        //            cell.Text = "Above 30 Days";
        //            row.Controls.Add(cell);


        //            cell = new TableHeaderCell();
        //            cell.ColumnSpan = 2;
        //            cell.Text = "Total";
        //            row.Controls.Add(cell);

        //            row.BackColor = ColorTranslator.FromHtml("#3AC0F2");
        //            gv_NestedChild.HeaderRow.Parent.Controls.AddAt(0, row);

        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }
        //}

        //protected void cmdLoad_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (txtFromDate.Text != string.Empty && txtToDate.Text != string.Empty)
        //        {
        //            string sResult = Genaral.DateValidation(txtFromDate.Text);
        //            if (sResult != "")
        //            {
        //                ShowMsgBox(sResult);
        //                return;
        //            }

        //            sResult = Genaral.DateValidation(txtToDate.Text);
        //            if (sResult != "")
        //            {
        //                ShowMsgBox(sResult);
        //                return;
        //            }


        //            sResult = Genaral.DateComparision(txtToDate.Text, txtFromDate.Text, false, false);
        //            if (sResult == "2")
        //            {
        //                ShowMsgBox("To Date should be Greater than From Date");
        //                txtToDate.Focus();
        //                return;

        //            }
        //        }

        //        clsFailureRepTImeLine objFailureRep = new clsFailureRepTImeLine();
        //        DataTable dtLoadFailureDetails = new DataTable();
        //        dtLoadFailureDetails = objFailureRep.LoadZoneDetails(txtFromDate.Text, txtToDate.Text);
        //        if (dtLoadFailureDetails.Rows.Count > 0)
        //        {
        //            GrdZone.DataSource = dtLoadFailureDetails;
        //            GrdZone.DataBind();
        //            GrdZone.Visible = true;
        //        }
        //        else
        //        {
        //            ShowMsgBox("No Records Found");
        //            GrdZone.Visible = false;
        //            txtFromDate.Text = string.Empty;
        //            txtToDate.Text = string.Empty;
        //        }
        //    }


        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }

        //}
        #endregion

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

        #region UnUsed Method
        //protected void grdCategory_RowCommand(object sender, GridViewCommandEventArgs e)
        //{

        //}

        //protected void grdCategory_OnRowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //}


        //protected void cmdExport_Click(object sender, EventArgs e)
        //{


        //    try
        //    {
        //        clsFailureRepTImeLine objFailureRep = new clsFailureRepTImeLine();
        //        DataTable dt = new DataTable();

        //        dt = objFailureRep.LoadAllDetails(txtFromDate.Text, txtToDate.Text);
        //        ViewState["allDetails"] = dt;

        //        if (dt.Rows.Count > 0)
        //        {
        //            string[] arrAlpha = Genaral.getalpha();

        //            //string sMergeRange = arrAlpha[dt.Columns.Count - 1];
        //            using (XLWorkbook wb = new XLWorkbook())
        //            {
        //                dt.Columns.Remove("CircleCode");
        //                dt.Columns.Remove("DIVISIONCODE");
        //                dt.Columns.Remove("SUBDIVISIONCODE");
        //                dt.Columns.Remove("SECTIONCODE");



        //                dt.Columns["LESSTHAN1DAY"].ColumnName = "COMPLETED FOR LESSTHAN1DAY";
        //                dt.Columns["LESSTHAN1DAYNEW"].ColumnName = "PENDING FOR LESSTHAN1DAY";

        //                dt.Columns["BW1TO7"].ColumnName = "COMPLETED FOR BW1TO7";
        //                dt.Columns["BW1TO7NEW"].ColumnName = "PENDING FOR BW1TO7";

        //                dt.Columns["BW7TO15"].ColumnName = "COMPLETED FOR BW7TO15";
        //                dt.Columns["BW7TO15NEW"].ColumnName = "PENDING FOR BW7TO15";

        //                dt.Columns["BW15TO30"].ColumnName = "COMPLETED FOR BW15TO30";
        //                dt.Columns["BW15TO30NEW"].ColumnName = "PENDING FOR BW15TO30";

        //                dt.Columns["ABOVE30"].ColumnName = "COMPLETED FOR ABOVE30";
        //                dt.Columns["ABOVE30NEW"].ColumnName = "PENDING FOR ABOVE30";

        //                dt.Columns["TOTAL"].ColumnName = "COMPLETED";
        //                dt.Columns["TOTALNEW"].ColumnName = "PENDING";

        //                dt.Columns["PENDING FOR LESSTHAN1DAY"].SetOrdinal(5);
        //                dt.Columns["PENDING FOR BW1TO7"].SetOrdinal(7);
        //                dt.Columns["PENDING FOR BW7TO15"].SetOrdinal(9);
        //                dt.Columns["PENDING FOR BW15TO30"].SetOrdinal(11);
        //                dt.Columns["PENDING FOR ABOVE30"].SetOrdinal(13);


        //                wb.Worksheets.Add(dt, "FailureTimeLine");
        //                wb.Worksheet(1).Row(1).InsertRowsAbove(2);
        //                wb.Worksheet(1).Row(1).InsertRowsAbove(2);

        //                string sMergeRange = arrAlpha[dt.Columns.Count - 1];

        //                var rangeheader = wb.Worksheet(1).Range("A1:" + sMergeRange + "1");
        //                rangeheader.Merge().Style.Font.SetBold().Font.FontSize = 16;
        //                rangeheader.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
        //                rangeheader.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //                rangeheader.SetValue("Hubli Electricity Supply Company Limited, (HESCOM)");

        //                var rangeheaderForLessthan1day = wb.Worksheet(1).Range("E4:F4");
        //                rangeheaderForLessthan1day.Merge().Style.Font.SetBold().Font.FontSize = 13;
        //                rangeheaderForLessthan1day.Merge().Style.Fill.BackgroundColor = XLColor.LightBlue;
        //                rangeheaderForLessthan1day.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //                rangeheaderForLessthan1day.SetValue("LESSTHAN1DAY");

        //                var rangeheaderBW1TO7 = wb.Worksheet(1).Range("G4:H4");
        //                rangeheaderBW1TO7.Merge().Style.Font.SetBold().Font.FontSize = 13;
        //                rangeheaderBW1TO7.Merge().Style.Fill.BackgroundColor = XLColor.LightBlue;
        //                rangeheaderBW1TO7.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //                rangeheaderBW1TO7.SetValue("BW1TO7");

        //                var rangeheaderBW7TO15 = wb.Worksheet(1).Range("I4:J4");
        //                rangeheaderBW7TO15.Merge().Style.Font.SetBold().Font.FontSize = 13;
        //                rangeheaderBW7TO15.Merge().Style.Fill.BackgroundColor = XLColor.LightBlue;
        //                rangeheaderBW7TO15.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //                rangeheaderBW7TO15.SetValue("BW7TO15");

        //                var rangeheaderBW15TO30 = wb.Worksheet(1).Range("K4:L4");
        //                rangeheaderBW15TO30.Merge().Style.Font.SetBold().Font.FontSize = 13;
        //                rangeheaderBW15TO30.Merge().Style.Fill.BackgroundColor = XLColor.LightBlue;
        //                rangeheaderBW15TO30.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //                rangeheaderBW15TO30.SetValue("BW15TO30");

        //                var rangeheaderABOVE30 = wb.Worksheet(1).Range("M4:N4");
        //                rangeheaderABOVE30.Merge().Style.Font.SetBold().Font.FontSize = 13;
        //                rangeheaderABOVE30.Merge().Style.Fill.BackgroundColor = XLColor.LightBlue;
        //                rangeheaderABOVE30.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //                rangeheaderABOVE30.SetValue("ABOVE30");

        //                var rangeheaderTOTAL = wb.Worksheet(1).Range("O4:P4");
        //                rangeheaderTOTAL.Merge().Style.Font.SetBold().Font.FontSize = 13;
        //                rangeheaderTOTAL.Merge().Style.Fill.BackgroundColor = XLColor.LightBlue;
        //                rangeheaderTOTAL.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //                rangeheaderTOTAL.SetValue("TOTAL");

        //                wb.Worksheet(1).Cell(2, 8).Value = DateTime.Now;


        //                Response.Clear();
        //                Response.Buffer = true;
        //                Response.Charset = "";
        //                string FileName = "FailureTimeLine " + DateTime.Now + ".xls";
        //                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //                Response.AddHeader("content-disposition", "attachment;filename=" + FileName);

        //                using (MemoryStream MyMemoryStream = new MemoryStream())
        //                {
        //                    wb.SaveAs(MyMemoryStream);
        //                    MyMemoryStream.WriteTo(Response.OutputStream);
        //                    Response.Flush();
        //                    Response.End();


        //                }
        //            }
        //        }
        //        else
        //        {
        //            ShowMsgBox("No Records Found");
        //        }

        //    }


        //    catch (Exception ex)
        //    {
        //        clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //    }

        //}
        #endregion

        /// <summary>
        /// Get failure replacement time line  report in excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdExport_Click(object sender, EventArgs e)
        {
            try
            {
                clsFailureRepTImeLine objFailureRep = new clsFailureRepTImeLine();
                DataTable dt = new DataTable();
                string FromDate = txtFromDate.Text;
                string ToDate = txtToDate.Text;
                string strResult;

                if (FromDate != "" && FromDate != null && ToDate != "" && ToDate != null)
                {
                    strResult = Genaral.DateComparision(FromDate, ToDate, false, false);
                    if (strResult == "1")
                    {
                        ShowMsgBox("To Date should be Greater than or Equal to From Date");
                        return;
                    }
                }

                if (FromDate != "" && FromDate != null)
                {
                    DateTime myDateTime = DateTime.ParseExact(FromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    FromDate = Convert.ToDateTime(myDateTime).ToString("yyyy-MM-dd");
                }

                if (ToDate != "" && ToDate != null)
                {
                    DateTime myDateTime1 = DateTime.ParseExact(ToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    ToDate = Convert.ToDateTime(myDateTime1).ToString("yyyy-MM-dd");
                }

                dt = objFailureRep.LoadAllDetails(txtFromDate.Text, txtToDate.Text);
                ViewState["allDetails"] = dt;

                if (dt.Rows.Count > 0)
                {
                    string[] arrAlpha = Genaral.getalpha();

                    //string sMergeRange = arrAlpha[dt.Columns.Count - 1];
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        dt.Columns.Remove("CircleCode");
                        dt.Columns.Remove("DIVISIONCODE");
                        dt.Columns.Remove("SUBDIVISIONCODE");
                        dt.Columns.Remove("SECTIONCODE");



                        dt.Columns["LESSTHAN1DAY"].ColumnName = "COMPLETED FOR LESSTHAN1DAY";
                        dt.Columns["LESSTHAN1DAYNEW"].ColumnName = "PENDING FOR LESSTHAN1DAY";

                        dt.Columns["BW1TO7"].ColumnName = "COMPLETED FOR BW1TO7";
                        dt.Columns["BW1TO7NEW"].ColumnName = "PENDING FOR BW1TO7";

                        dt.Columns["BW7TO15"].ColumnName = "COMPLETED FOR BW7TO15";
                        dt.Columns["BW7TO15NEW"].ColumnName = "PENDING FOR BW7TO15";

                        dt.Columns["BW15TO30"].ColumnName = "COMPLETED FOR BW15TO30";
                        dt.Columns["BW15TO30NEW"].ColumnName = "PENDING FOR BW15TO30";

                        dt.Columns["ABOVE30"].ColumnName = "COMPLETED FOR ABOVE30";
                        dt.Columns["ABOVE30NEW"].ColumnName = "PENDING FOR ABOVE30";

                        dt.Columns["TOTAL"].ColumnName = "COMPLETED";
                        dt.Columns["TOTALNEW"].ColumnName = "PENDING";

                        dt.Columns["PENDING FOR LESSTHAN1DAY"].SetOrdinal(5);
                        dt.Columns["PENDING FOR BW1TO7"].SetOrdinal(7);
                        dt.Columns["PENDING FOR BW7TO15"].SetOrdinal(9);
                        dt.Columns["PENDING FOR BW15TO30"].SetOrdinal(11);
                        dt.Columns["PENDING FOR ABOVE30"].SetOrdinal(13);


                        wb.Worksheets.Add(dt, "FailureTimeLine");
                        wb.Worksheet(1).Row(1).InsertRowsAbove(2);
                        wb.Worksheet(1).Row(1).InsertRowsAbove(2);

                        string sMergeRange = arrAlpha[dt.Columns.Count - 1];

                        var rangeheader = wb.Worksheet(1).Range("A1:" + sMergeRange + "1");
                        rangeheader.Merge().Style.Font.SetBold().Font.FontSize = 16;
                        rangeheader.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
                        rangeheader.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        rangeheader.SetValue("Hubli Electricity Supply Company Limited, (HESCOM)");

                        var rangeheaderForLessthan1day = wb.Worksheet(1).Range("E4:F4");
                        rangeheaderForLessthan1day.Merge().Style.Font.SetBold().Font.FontSize = 13;
                        rangeheaderForLessthan1day.Merge().Style.Fill.BackgroundColor = XLColor.LightBlue;
                        rangeheaderForLessthan1day.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        rangeheaderForLessthan1day.SetValue("LESSTHAN1DAY");

                        var rangeheaderBW1TO7 = wb.Worksheet(1).Range("G4:H4");
                        rangeheaderBW1TO7.Merge().Style.Font.SetBold().Font.FontSize = 13;
                        rangeheaderBW1TO7.Merge().Style.Fill.BackgroundColor = XLColor.LightBlue;
                        rangeheaderBW1TO7.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        rangeheaderBW1TO7.SetValue("BW1TO7");

                        var rangeheaderBW7TO15 = wb.Worksheet(1).Range("I4:J4");
                        rangeheaderBW7TO15.Merge().Style.Font.SetBold().Font.FontSize = 13;
                        rangeheaderBW7TO15.Merge().Style.Fill.BackgroundColor = XLColor.LightBlue;
                        rangeheaderBW7TO15.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        rangeheaderBW7TO15.SetValue("BW7TO15");

                        var rangeheaderBW15TO30 = wb.Worksheet(1).Range("K4:L4");
                        rangeheaderBW15TO30.Merge().Style.Font.SetBold().Font.FontSize = 13;
                        rangeheaderBW15TO30.Merge().Style.Fill.BackgroundColor = XLColor.LightBlue;
                        rangeheaderBW15TO30.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        rangeheaderBW15TO30.SetValue("BW15TO30");

                        var rangeheaderABOVE30 = wb.Worksheet(1).Range("M4:N4");
                        rangeheaderABOVE30.Merge().Style.Font.SetBold().Font.FontSize = 13;
                        rangeheaderABOVE30.Merge().Style.Fill.BackgroundColor = XLColor.LightBlue;
                        rangeheaderABOVE30.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        rangeheaderABOVE30.SetValue("ABOVE30");

                        var rangeheaderTOTAL = wb.Worksheet(1).Range("O4:P4");
                        rangeheaderTOTAL.Merge().Style.Font.SetBold().Font.FontSize = 13;
                        rangeheaderTOTAL.Merge().Style.Fill.BackgroundColor = XLColor.LightBlue;
                        rangeheaderTOTAL.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        rangeheaderTOTAL.SetValue("TOTAL");

                        wb.Worksheet(1).Cell(2, 8).Value = DateTime.Now;


                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        string FileName = "FailureTimeLine " + DateTime.Now + ".xls";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=" + FileName);

                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();


                        }
                    }
                }
                else
                {
                    ShowMsgBox("No Records Found");
                }

            }


            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }


        #region UnUsed Methods

        //added now for splitting the columns

        //protected void OnDataBound(object sender, EventArgs e)
        //{
        //    GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
        //    TableHeaderCell cell = new TableHeaderCell();
        //    cell.Text = "View";
        //    cell.Width = 100;
        //    cell.ColumnSpan = 1;
        //    row.Controls.Add(cell);

        //    cell = new TableHeaderCell();
        //    cell.Text = "Zone";
        //    cell.ColumnSpan = 1;
        //    cell.Width = 100;
        //    row.Controls.Add(cell);

        //    cell = new TableHeaderCell();
        //    cell.ColumnSpan = 2;
        //    cell.Text = "Within1Day";
        //    row.Controls.Add(cell);

        //    cell = new TableHeaderCell();
        //    cell.ColumnSpan = 2;
        //    cell.Text = "Between 1to7 Days";
        //    row.Controls.Add(cell);


        //    cell = new TableHeaderCell();
        //    cell.ColumnSpan = 2;
        //    cell.Text = "Between 7to15 Days";
        //    row.Controls.Add(cell);

        //    cell = new TableHeaderCell();
        //    cell.ColumnSpan = 2;
        //    cell.Text = "Between 15to30 Days";
        //    row.Controls.Add(cell);

        //    cell = new TableHeaderCell();
        //    cell.ColumnSpan = 2;
        //    cell.Text = "Above 30 Days";
        //    row.Controls.Add(cell);


        //    cell = new TableHeaderCell();
        //    cell.ColumnSpan = 2;
        //    cell.Text = "Total";
        //    row.Controls.Add(cell);

        //    row.BackColor = ColorTranslator.FromHtml("#3AC0F2");
        //    GrdZone.HeaderRow.Parent.Controls.AddAt(0, row);


        //}
        #endregion

    }

}