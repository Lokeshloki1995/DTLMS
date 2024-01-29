using IIITS.DTLMS.BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.ScrapEntry
{
    public partial class ScrapView : System.Web.UI.Page
    {
        string strFormCode = "ScrapView";
        clsSession objSession;
        string sUserName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_USER"]);
        string sPassword = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FTP_PASS"]);

        string SFTPPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"]);
        string SFTPmainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder"]);


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
            {
                Response.Redirect("~/Login.aspx", false);
            }
            else
            {
                objSession = (clsSession)Session["clsSession"];

                if (!IsPostBack)
                {
                    CheckAccessRights("4");
                    LoadScrapDetails();
                }
            }

        }

        protected void cmdNewCircle_Click(object sender, EventArgs e)
        {
            try
            {
                //Check AccessRights
                bool bAccResult = CheckAccessRights("2");
                if (bAccResult == false)
                {
                    return;
                }
                Response.Redirect("Circle.aspx", false);
            }
            catch (Exception ex)
            {
                // lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void LoadScrapDetails()
        {
            try
            {
                DataTable dt = new DataTable();
                clsScrap objScrapview = new clsScrap();
                string soffcode =objSession.OfficeCode;
                string RoleId = objSession.RoleId;
                dt = objScrapview.LoadAllScrapDetails(soffcode, RoleId);

                ViewState["ScrapDetails"] = dt;
                grdScrapTC.DataSource = dt;
                grdScrapTC.DataBind();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }



        protected void grdScrapTC_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                grdScrapTC.PageIndex = e.NewPageIndex;
                dt = (DataTable)ViewState["ScrapDetails"];
                grdScrapTC.DataSource = SortDataTable(dt as DataTable, true);
                grdScrapTC.DataBind();
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
                        ViewState["ScrapDetails"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["ScrapDetails"] = dataView.ToTable();

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

        protected void grdCirclemaster_Sorting(object sender, GridViewSortEventArgs e)
        {


            int columnIndex = 0;
    

            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdScrapTC.PageIndex;
            DataTable dt = (DataTable)ViewState["Circle"];
            string sortingDirection = string.Empty;

            Image sortImage = new Image();
            Image sortImageboth = new Image();


            if (dt.Rows.Count > 0)
            {

                grdScrapTC.DataSource = SortDataTable(dt as DataTable, false);
            }

            else
            {
                grdScrapTC.DataSource = dt;

            }
            grdScrapTC.DataBind();
            grdScrapTC.PageIndex = pageIndex;
        }


        protected void grdScrapTC_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                LoadScrapDetails();
                if (e.CommandName == "Search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtomcode = (TextBox)row.FindControl("txtdtrcode");
                    //TextBox txtCircleName = (TextBox)row.FindControl("txtCircleName");



                    DataTable dt = (DataTable)ViewState["ScrapDetails"];
                    dv = dt.DefaultView;

                    if (txtomcode.Text != "")
                    {
                        sFilter = "ST_OM_NO Like '%" + txtomcode.Text.Replace("'", "'") + "%' AND";
                    }
                    //if (txtCircleName.Text != "")
                    //{
                    //    sFilter += "CM_CIRCLE_NAME Like '%" + txtCircleName.Text.Replace("'", "'") + "%' AND";
                    //}

                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdScrapTC.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {


                            grdScrapTC.DataSource = dv;
                            ViewState["ScrapDetails"] = dv.ToTable();
                            grdScrapTC.DataBind();

                        }
                        else
                        {
                            ViewState["ScrapDetails"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadScrapDetails();
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
                dt.Columns.Add("ST_ID");
                dt.Columns.Add("ST_OM_NO");
                dt.Columns.Add("ST_OM_DATE");
                dt.Columns.Add("ST_CRBY");
                dt.Columns.Add("STORE_NAME");
                dt.Columns.Add("ST_QTY");





                grdScrapTC.DataSource = dt;
                grdScrapTC.DataBind();

                int iColCount = grdScrapTC.Rows[0].Cells.Count;
                grdScrapTC.Rows[0].Cells.Clear();
                grdScrapTC.Rows[0].Cells.Add(new TableCell());
                grdScrapTC.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdScrapTC.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }

    
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "ScrapView";
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sAccessType = "1" + "," + sAccessType;
                bool bResult = objApproval.CheckAccessRights(objApproval);
                if (bResult == false)
                {
                    Response.Redirect("~/UserRestrict.aspx", false);

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
        protected void cmdLoad_click(object sender, EventArgs e)
        {
            //DataTable dt = new DataTable();
            //clsScrap objDi = new clsScrap();
            try
            {


                LinkButton lnkdwn = (LinkButton)sender;
                GridViewRow rw = (GridViewRow)lnkdwn.NamingContainer;

                //Label lblDiNo = (Label)grdScrapTC.Rows[rowindex].FindControl("lblDiNo");


                string omid = ((Label)rw.FindControl("lblstdid")).Text;
                string OmNo = ((Label)rw.FindControl("lblomno")).Text;
                string OmDate = ((Label)rw.FindControl("lblomdate")).Text;

                string strOmId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(omid));
                string strOmNo = HttpUtility.UrlEncode(Genaral.UrlEncrypt(OmNo));
                string strOmDate = HttpUtility.UrlEncode(Genaral.UrlEncrypt(OmDate));


                Response.Redirect("ScrapTest.aspx?OMID=" + strOmId + "&QryOMNo="+ strOmNo + "&QryOmDate=" + strOmDate, false);  
                //Response.Redirect("/ScrapEntry/ScrapView.aspx""?OMID=" + omid, false); 
                //objDi.sMake = ((Label)rw.FindControl("lblMake")).Text;
                //objDi.sStorename = ((Label)rw.FindControl("lblstore")).Text;
                //objDi.sCapacity = ((Label)rw.FindControl("lblCapacity")).Text;
                //objDi.sRating = ((Label)rw.FindControl("lblRating")).Text;
                //objDi.sTotqty = ((Label)rw.FindControl("lblQuantity")).Text;
                //objDi.sPendingQty = ((Label)rw.FindControl("lblPenQuantity")).Text;
                //objDi.sStartrange = ((Label)rw.FindControl("lblstartrange")).Text;
                //objDi.sEndrange = ((Label)rw.FindControl("lblendrange")).Text;
                //objDi.sMakeId = ((Label)rw.FindControl("lblMakeId")).Text;
                //objDi.sDimDino = ((Label)rw.FindControl("lbldimid")).Text;
                //objDi.sPoid = ((Label)rw.FindControl("lblpoid")).Text;

                // dt = objDi.LoadALlotmentdetails(objDi);

                //if (dt.Rows.Count <= 0)
                //{

                //    DataTable dtPoDetails = new DataTable();
                //    DataRow newRow = dtPoDetails.NewRow();
                //    dtPoDetails.Rows.Add(newRow);
                //    dtPoDetails.Columns.Add("TCP_ID");
                //    dtPoDetails.Columns.Add("DIM_DI_NO");
                //    dtPoDetails.Columns.Add("TCP_TC_CODE");
                //    dtPoDetails.Columns.Add("SM_NAME");
                //    dtPoDetails.Columns.Add("MD_NAME");
                //    dtPoDetails.Columns.Add("DI_CAPACITY");

                //    grdAllotmentView.DataSource = dtPoDetails;
                //    grdAllotmentView.DataBind();

                //    int iColCount = grdAllotmentView.Rows[0].Cells.Count;
                //    grdAllotmentView.Rows[0].Cells.Clear();
                //    grdAllotmentView.Rows[0].Cells.Add(new TableCell());
                //    grdAllotmentView.Rows[0].Cells[0].ColumnSpan = iColCount;
                //    grdAllotmentView.Rows[0].Cells[0].Text = "No Records Found";
                //    ViewState["ALT_VIEW"] = dt;
                //}

                //else
                //{
                //    grdAllotmentView.DataSource = dt;
                //    grdAllotmentView.DataBind();
                //    ViewState["ALT_VIEW"] = dt;
                //    cmdexport.Visible = true;

                //}
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }


        }


    }
}