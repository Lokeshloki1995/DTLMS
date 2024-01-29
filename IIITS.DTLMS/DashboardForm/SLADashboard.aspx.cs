using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Configuration;
using System.Web.UI.DataVisualization.Charting;
using IIITS.DTLMS.BL.Dashboard;

namespace IIITS.DTLMS.DashboardForm
{
    public partial class SLADashboard : System.Web.UI.Page
    {
        string strFormCode = "SLADashboard";
        clsSession objSession;
        clsSlaDashboard objSlaDashbord = new clsSlaDashboard();
        clsDashboard objDashboard = new clsDashboard();

        protected int Total { get; set; }

        //protected int AddNumberToTotal(object obj)
        //{
            
        //    int number = Convert.ToInt32(((DataRowView)obj));
        //    this.Total += number;
        //    return number;
        //}
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];
                CheckAccessRights("4");
                if (!IsPostBack)
                {
                    if (objSession != null)
                    {
                        //lblLocation.Text = objSession.OfficeName;
                       

                        hdfLocationCode.Value = objSession.OfficeCode;
                        string str = hdfLocationCode.Value;
                        DataTable dt1 = new DataTable();
                        dt1 = objSlaDashbord.GetSLADetails(hdfLocationCode.Value);
                        GrdSlaDetails.DataSource = dt1;
                        GrdSlaDetails.DataBind();
                        ViewState["grddetails"] = dt1;

                        backbtn.Visible = false;

                        DataTable dt2 = objDashboard.getFailurePendingCounts(hdfLocationCode.Value);
                       
                 
                        string failure= Convert.ToString(dt2.Rows[0]["FAILURE_APPROVE"]);
                        string sEstimation = Convert.ToString(dt2.Rows[0]["PENDING_ESTIMATION"]);
                        string sWorder = Convert.ToString(dt2.Rows[0]["PEN_SINGLE_COIL_WOR"]);
                        string sRcveDTR = Convert.ToString(dt2.Rows[0]["PENDING_RECIEVE_DTR"]);
                        string sInvDTR = Convert.ToString(dt2.Rows[0]["PENDING_MAJOR_INV"]);


                        string sMultiWorder = Convert.ToString(dt2.Rows[0]["PEN_MULTI_COIL_WOR"]);
                        string sindent = Convert.ToString(dt2.Rows[0]["PENDING_INDENT"]);
                        string multicommission = Convert.ToString(dt2.Rows[0]["PENDING_COMMISS"]);
                        string decomm = Convert.ToString(dt2.Rows[0]["PENDING_DECOMMI"]);
                        string ri = Convert.ToString(dt2.Rows[0]["PENDING_RI"]);
                        string cr = Convert.ToString(dt2.Rows[0]["PENDING_CR"]);

                        //string sEstimation = objSlaDashbord.GetEstimation(hdfLocationCode.Value);
                        //string sWorder = objSlaDashbord.GetWorkOrder(hdfLocationCode.Value);
                        //string sRcveDTR = objSlaDashbord.GetRcveDTR(hdfLocationCode.Value);
                        //string sInvDTR = objSlaDashbord.GetInvcDTR(hdfLocationCode.Value);
                        DataTable dt = new DataTable();

                        //dt.Columns.AddRange(new DataColumn[4] { new DataColumn("ESTIMATION", typeof(string)), new DataColumn("WORK ORDER", typeof(string)),
                        //new DataColumn("RECEIVEDTR",typeof(string)),new DataColumn("INVCDTR", typeof(string)) });
                        //dt.Rows.Add(sEstimation, sWorder, sRcveDTR, sInvDTR);
                        dt.Columns.AddRange(new DataColumn[11] { new DataColumn("FAILURE", typeof(string)),new DataColumn("ESTIMATION", typeof(string)), new DataColumn("MINOR WORK ORDER", typeof(string)),
                        new DataColumn("RECEIVE DTR",typeof(string)),new DataColumn("INVOICE DTR", typeof(string)),new DataColumn("MAJOR WORK ORDER", typeof(string)),new DataColumn("INDENT", typeof(string)),new DataColumn("COMMISSION", typeof(string)),new DataColumn("DECOMMISSION", typeof(string)),new DataColumn("RI", typeof(string)),new DataColumn("CR", typeof(string)) });
                        dt.Rows.Add(failure, sEstimation, sWorder, sRcveDTR, sInvDTR, sMultiWorder, sindent, multicommission, decomm, ri,cr);
                        GridSlaAbstarct.DataSource = dt;
                        GridSlaAbstarct.DataBind();
                        if (dt1.Rows.Count > 0)
                        {

                            // string strSubject = "DTLMS:Minor Repair Approval Statuss";
                            DateTime dtTodayDate = DateTime.Now.Date.AddDays(0);
                            string strTodayFormat = dtTodayDate.ToString("dd-MMM-yyyy");
                            string strHTML = string.Empty;
                            //clsEmailSMS objEmail = new clsEmailSMS();

                            string strHeader = "Approval Pending as on " + strTodayFormat + "";

                            strHTML = "<html>";
                            strHTML += "<h3 align ='center'> DTLMS </h3>";
                            strHTML += "<p>";
                            strHTML += "<span style = 'font-size:10.5pt;line-height:150%;font-family:'Helvetica',sans-serif;color:#505050'>";
                            strHTML += "<b>Dear All</b>";
                            strHTML += "<br>";
                            strHTML += "Greetings from Team DTLMS";
                            strHTML += "<span>";
                            strHTML += "</p>";

                            strHTML += "<table cellpadding='0' cellspacing='0' border='4' bordercolour='red' borderstyle='Outser' 'width='150%'>";
                            strHTML += "<thead ><tr bgcolor='#337ab7' align='center' 'width='0%'><td colspan='38' align='center'; style='font-size:larger''width='0%' ><font color='White' > Abstract </font></td></tr>";
                            strHTML += " </thead>";
                            strHTML += "<tr class ='Stockgrid' bgcolor='#33C4FF'>";
                            strHTML += "<td align='center' style='font-size:15px;' width='150'><b>ESTIMATION</b></td>";
                            strHTML += "<td  align='center' style='font-size:15px;' width='150'><b>WORK ORDER</b></td>";
                            strHTML += "<td  align='center' style='font-size:15px;' width='150'><b>RECEIVE DTR</b></td>";
                            strHTML += "<td  align='center' style='font-size:15px;' width='150'><b>INVOICE DTR</b></td>";
                            strHTML += "</tr>";
                            strHTML += "<tr class ='Stockgrid' >";
                            strHTML += "<td align='center'  style='font-size:15px;'>" + sEstimation + "</td>";
                            strHTML += "<td align='center'  style='font-size:15px;'>" + sWorder + "</td>";
                            strHTML += "<td align='center'  style='font-size:15px;'>" + sRcveDTR + "</td>";
                            strHTML += "<td align='center'  style='font-size:15px;'>" + sInvDTR + "</td>";
                            strHTML += "<tr/>";
                            strHTML += "</table>";
                            strHTML += "<br/>";
                            strHTML += "<br/>";
                            strHTML += "<br/>";


                            strHTML += "<table cellpadding='0' cellspacing='0' border='4' bordercolour='red' borderstyle='Outser' 'width='150%'>";
                            strHTML += "<thead ><tr bgcolor='#337ab7' align='center' 'width='0%'><td colspan='38' align='center'; style='font-size:larger''width='0%' ><font color='White' > " + strHeader + " </font></td></tr>";
                            strHTML += " </thead>";
                            strHTML += "<tr class ='Stockgrid' bgcolor='#33C4FF'>";
                            strHTML += "<td align='center' style='font-size:15px;' width='150'><b>SLNO</b></td>";
                            strHTML += "<td  align='center' style='font-size:15px;' width='150'><b>SUB DIVISION</b></td>";
                            strHTML += "<td  align='center' style='font-size:15px;' width='150'><b>OM SECTION</b></td>";
                            strHTML += "<td  align='center' style='font-size:15px;' width='150'><b>PHASE</b></td>";
                            strHTML += "<td  align='center' style='font-size:15px;' width='150' bgcolor='#25F806'><b>PENDING < 3 DAYS</ b></td>";
                            strHTML += "<td  align='center' style='font-size:15px;' width='150' bgcolor='#F07C0F'><b>PENDING FROM 3 TO 7 DAYS</b></td>";
                            strHTML += "<td  align='center' style='font-size:15px;' width='150' bgcolor='#FE0000'><b>PENDING FROM 8 TO 15 DAYS</b></td>";
                            strHTML += "<td  align='center' style='font-size:15px;' width='150' bgcolor='#FE0000'><b>PENDING FROM 16 TO 30 DAYS</b></td>";
                            strHTML += "<td  align='center' style='font-size:15px;' width='150' bgcolor='#FE0000'><b>PENDING FROM > 30 DAYS</b></td>";
                            strHTML += "</tr>";

                            int j = 0;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                j++;
                                strHTML += "<tr class ='Stockgrid' >";
                                strHTML += "<td align='center'  style='font-size:15px;'>" + j + "</td>";
                                strHTML += "<td align='center'  style='font-size:15px;'>" + Convert.ToString(dt1.Rows[i]["headoffname"]) + "</td>";
                                strHTML += "<td align='center'  style='font-size:15px;'>" + Convert.ToString(dt1.Rows[i]["offname"]) + "</td>";
                                strHTML += "<td align='center'  style='font-size:15px;'>" + Convert.ToString(dt1.Rows[i]["sam_name"]) + "</td>";
                                strHTML += "<td align='center'  style='font-size:15px;'>" + Convert.ToString(dt1.Rows[i]["three_days"]) + "</td>";
                                strHTML += "<td align='center'  style='font-size:15px;'>" + Convert.ToString(dt1.Rows[i]["seven_days"]) + "</td>";
                                strHTML += "<td align='center'  style='font-size:15px;'>" + Convert.ToString(dt1.Rows[i]["fifteen_days"]) + "</td>";
                                strHTML += "<td align='center'  style='font-size:15px;'>" + Convert.ToString(dt1.Rows[i]["thirty_days"]) + "</td>";
                                strHTML += "<td align='center'  style='font-size:15px;'>" + Convert.ToString(dt1.Rows[i]["more_than_thirty_days"]) + "</td>";

                            }

                            if (dt.Rows.Count > 0)
                            {
                                string s3days = dt1.Compute("SUM(" + dt1.Columns["three_days"].ToString() + ")", "").ToString();
                                string s7days = dt1.Compute("SUM(" + dt1.Columns["seven_days"].ToString() + ")", "").ToString();
                                string s15days = dt1.Compute("SUM(" + dt1.Columns["fifteen_days"].ToString() + ")", "").ToString();
                                string s30days = dt1.Compute("SUM(" + dt1.Columns["thirty_days"].ToString() + ")", "").ToString();
                                string smore30days = dt1.Compute("SUM(" + dt1.Columns["more_than_thirty_days"].ToString() + ")", "").ToString();

                                strHTML += "<tr>";
                                strHTML += "<td  colspan='4' align='center'><font color='Black'>Grand Total</font></td>";
                                strHTML += "<td  align='center' style='style='font-size:25px;'> " + s3days + " </td>";
                                strHTML += "<td  align='center' style='style='font-size:25px;'> " + s7days + " </td>";
                                strHTML += "<td  align='center' style='style='font-size:25px;'> " + s15days + " </td>";
                                strHTML += "<td  align='center' style='style='font-size:25px;'> " + s30days + " </td>";
                                strHTML += "<td  align='center' style='style='font-size:25px;'> " + smore30days + " </td>";
                                strHTML += "<tr/>";
                            }

                            strHTML += "</table>";
                            strHTML += "<br/>";
                            strHTML += "<span style = 'font-size:10.5pt;line-height:150%;font-family:'Helvetica',sans-serif;color:#505050' >";
                            strHTML += "<b>Regards</b>";
                            strHTML += "<br/>";
                            strHTML += "Team DTLMS";
                            strHTML += "</span>";
                            strHTML += "<br/>";
                            strHTML += "<span style = 'font-size:10.5pt;line-height:150%;font-family:'Helvetica',sans-serif;color:#505050;align:center'>";
                            strHTML += "<br/><center><b>Copyright © 2018 Idea Infinity IT Solutions Pvt.Ltd..Terms & Conditions Apply</b></center>";
                            strHTML += "</span>";


                            // objEmail.SendMail(strSubject, "srujan.chekka@ideainfinityit.com,bescomdtlms.manager@ideainfinityit.com", strHTML);
                            //,bescomdtlms.manager@ideainfinityit.com
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "SLADashboard";
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
        protected void GrdSlaDetails_SelectedIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dt1 = (DataTable)ViewState["grddetails"];
                GrdSlaDetails.PageIndex = e.NewPageIndex;
                GrdSlaDetails.DataSource = dt1;
                GrdSlaDetails.DataBind();
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void GrdSlaDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "view")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);


                    string sOfficeCode = objSession.OfficeCode;
                    string offcode = ((Label)row.FindControl("lbloffcode")).Text;
                    string moduleid = ((Label)row.FindControl("lblmoduleorder")).Text;

                    hdoffcode.Value = offcode;
                    hdmoduleid.Value = moduleid;




                    DataTable dt1 = new DataTable();
                    dt1 = objSlaDashbord.GetSLADetailsview(offcode, moduleid);
                    GrdSlaDetails.DataSource = dt1;
                    GrdSlaDetails.DataBind();


                    backbtn.Visible = true;

                    //LoadFailureDetailsviewLocationWise(sOfficeCode);


                }
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtsubdiv = (TextBox)row.FindControl("txtsubdiv");
                    TextBox txtsection = (TextBox)row.FindControl("txtsection");
                    TextBox txtphase = (TextBox)row.FindControl("txtphase");
                    DataTable dt = (DataTable)ViewState["grddetails"];
                    dv = dt.DefaultView;
                    if (txtsubdiv.Text != "")
                    {
                        sFilter = "headoffname Like '%" + txtsubdiv.Text + "%' AND";
                    }
                    if (txtsection.Text != "")
                    {
                        sFilter = "offname Like '%" + txtsection.Text + "%' AND";
                    }
                    if (txtphase.Text != "")
                    {
                        sFilter = "boname Like '%" + txtphase.Text + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        GrdSlaDetails.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            GrdSlaDetails.DataSource = dv;
                            ViewState["grddetails"] = dv.ToTable();
                            GrdSlaDetails.DataBind();
                        }
                        else
                        {
                            ViewState["grddetails"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
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
                dt.Columns.Add("headoffname");
                dt.Columns.Add("offname");
                dt.Columns.Add("offcode");
                dt.Columns.Add("bo_order");
                dt.Columns.Add("boname");
                dt.Columns.Add("three_days");
                dt.Columns.Add("seven_days");
                dt.Columns.Add("fifteen_days");
                dt.Columns.Add("thirty_days");
                dt.Columns.Add("more_than_thirty_days");
                GrdSlaDetails.DataSource = dt;
                GrdSlaDetails.DataBind();

                int iColCount = GrdSlaDetails.Rows[0].Cells.Count;
                GrdSlaDetails.Rows[0].Cells.Clear();
                GrdSlaDetails.Rows[0].Cells.Add(new TableCell());
                GrdSlaDetails.Rows[0].Cells[0].ColumnSpan = iColCount;
                GrdSlaDetails.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }








        /*public void LoadSlaDetailsGrid(string sOfficeCode, string sSubDiv, string sSection, string sPhase)
        {
            clsSlaDashboard objSlaDashbord = new clsSlaDashboard();
            DataTable dt = new DataTable();
            if (sOfficeCode == "")
            {
                sOfficeCode = "";
            }
            dt = objSlaDashbord.LoadDetails(sOfficeCode, sSubDiv, sSection, sPhase);

        }*/







        protected void GrdSlaDetails_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = GrdSlaDetails.PageIndex;
            DataTable dt = (DataTable)ViewState["grddetails"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                GrdSlaDetails.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                GrdSlaDetails.DataSource = dt;
            }
            GrdSlaDetails.DataBind();
            GrdSlaDetails.PageIndex = pageIndex;
        }


        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "ASC"; }
            set { ViewState["SortDirection"] = value; }
        }
        private string GridViewSortExpression
        {
            get { return ViewState["SortExpression"] as string ?? "string.empty"; }
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
                    GridViewSortDirection = "DESC";
                    break;
            }
            return GridViewSortDirection;
        }
        protected DataView SortDataTable(DataTable dataTable, bool isPageIndexChanging)
        {

            if (dataTable != null)
            {

                DataView dataview = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataview.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        ViewState["grddetails"] = dataview.ToTable();

                    }
                    else
                    {
                        dataview.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["grddetails"] = dataview.ToTable();

                    }
                }
                return dataview;
            }
            else
            {
                return new DataView();
            }
        }



        protected void cmdBack_Click(object sender, EventArgs e)
        {
            try
            {
                string officecode = hdoffcode.Value;
                string moduleid = hdmoduleid.Value;

                officecode = officecode.Substring(0, officecode.Length - 1);
                if (officecode == "0")
                {
                    hdoffcode.Value = "";

                }
                else
                {
                    hdoffcode.Value = officecode;
                }

                if (hdoffcode.Value == objSession.OfficeCode)
                {
                    backbtn.Visible = false;
                    moduleid = "";
                }
                else
                {
                    backbtn.Visible = true;
                }

                DataTable dt1 = new DataTable();
                dt1 = objSlaDashbord.GetSLADetailsview(hdoffcode.Value, moduleid);
                GrdSlaDetails.DataSource = dt1;
                GrdSlaDetails.DataBind();



            }

            catch (Exception ex)
            {

                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}































