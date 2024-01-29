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
namespace IIITS.DTLMS.DtcMissMatch
{
    public partial class FailureTimeLineDetails : System.Web.UI.Page
    {
        string strFormCode = "FailureTimeLineDetails";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
              
                if (!IsPostBack)
                {
                    string strFromDate = string.Empty;
                    string strToDate = string.Empty;
                    if (Request.QueryString["FeederCatId"] != null && Request.QueryString["SectionCode"].ToString() != "")
                    {
                        string strCatId = Request.QueryString["FeederCatId"];
                        string strSectionCode = Request.QueryString["SectionCode"];
                        if (Request.QueryString["fromDate"] !="")
                        {
                           strFromDate = Request.QueryString["fromDate"];
                            
                        }
                        if (Request.QueryString["ToDate"].ToString() !="")
                        {
                            strToDate = Request.QueryString["ToDate"];
                        }
                        LoadFeederCatDetails(strCatId, strSectionCode, strFromDate, strToDate);
                    }

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }


        public void LoadFeederCatDetails(string strCategoryId, string strSectionCode, string strFromDate, string strToDate)
        {
            try
            {
                clsFailureRepTImeLine objFailureRep = new clsFailureRepTImeLine();
                DataTable dtLoadDetails = new DataTable();
                dtLoadDetails = objFailureRep.LoadFeederDetails(strCategoryId, strSectionCode, strFromDate, strToDate);
                if (dtLoadDetails.Rows.Count > 0)
                {
                    grdFailuretc.DataSource = dtLoadDetails;
                    grdFailuretc.DataBind();
                    ViewState["TcFailure"] = dtLoadDetails;
                }
                else
                {
                    ShowMsgBox("No Records Found");
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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdFailuretc_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdFailuretc.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["TcFailure"];
                grdFailuretc.DataSource = dt;
                grdFailuretc.DataBind();
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }


        protected void cmdExport_Click(object sender, EventArgs e)
        {
          
          
            try
            {
            
                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["TcFailure"];
                if (dt.Rows.Count > 0)
                {
                    string[] arrAlpha = Genaral.getalpha();

                
                    using (XLWorkbook wb = new XLWorkbook())
                    {

                        //dt.Columns["FC_NAME"].ColumnName = "Circle Name";
                        dt.Columns["DT_CODE"].ColumnName = "Dtc Code";
                        dt.Columns["TC_CODE"].ColumnName = "DTR Code";
                        //dt.Columns["FD_FEEDER_NAME"].ColumnName = "Feeder Name";
                        dt.Columns["TC_SLNO"].ColumnName = "Tc Serial Number";
                        dt.Columns["FAILUREDATE"].ColumnName = "Failure Date";
                        dt.Columns["STATUS"].ColumnName = "Status";
                        

                        dt.Columns.Remove("FC_ID");
                        dt.Columns.Remove("DECOMISSIONWONO");
                        dt.Columns.Remove("COMISSIONWONO");
                        dt.Columns.Remove("FD_FEEDER_NAME");
                        wb.Worksheets.Add(dt, "FailureTimeLine");
                        wb.Worksheet(1).Row(1).InsertRowsAbove(2);
                       

                        string sMergeRange = arrAlpha[dt.Columns.Count - 1];
                        var rangeheader = wb.Worksheet(1).Range("A1:" + sMergeRange + "1");
                        rangeheader.Merge().Style.Font.SetBold().Font.FontSize = 16;
                        rangeheader.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
                        rangeheader.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        rangeheader.SetValue("Chamundeswhari Electricity Supply Board, (CESC Mysore)");

                        wb.Worksheet(1).Cell(2, 3).Value = DateTime.Now;


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
                            //Response.Flush();
                            HttpContext.Current.Response.Flush();
                            HttpContext.Current.Response.SuppressContent = true;
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                          
                          //Response.SuppressContent = true;
                          //ApplicationInstance.CompleteRequest();
                          // thread.Sleep(1);
                            //Response.End();
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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

    }
}