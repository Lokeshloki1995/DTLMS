using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Text.RegularExpressions;


namespace IIITS.DTLMS.BasicForms
{
    public partial class TalukView : System.Web.UI.Page
    {
        string strFormCode = "TalukView";
        clsSession objSession;
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
                    LoadTalukDetails();
                }
            }
            
        }

        protected void cmdNewTaluk_Click(object sender, EventArgs e)
        {
            try
            {
                //Check AccessRights
                bool bAccResult = CheckAccessRights("2");
                if (bAccResult == false)
                {
                    return;
                }

                Response.Redirect("Taluk.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void imgBtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                //Check AccessRights
                bool bAccResult = CheckAccessRights("3");
                if (bAccResult == false)
                {
                    return;
                }

                ImageButton imgEdit = (ImageButton)sender;
                GridViewRow rw = (GridViewRow)imgEdit.NamingContainer;

                String TalukCode = ((Label)rw.FindControl("lblTlkId")).Text;
                TalukCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(TalukCode));

                Response.Redirect("Taluk.aspx?TalukId=" + TalukCode + "", false);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadTalukDetails()
        {
            try
            {
                DataTable dt = new DataTable();
                clsTaluk objTalk = new clsTaluk();
                dt = objTalk.LoadAllTalkDetails();
                ViewState["TalkDetails"] = dt;
                grdTalukdetails.DataSource = dt;
                grdTalukdetails.DataBind();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }

        protected void grdTalukdetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                grdTalukdetails.PageIndex = e.NewPageIndex;
                dt = (DataTable)ViewState["TalkDetails"];
                grdTalukdetails.DataSource = SortDataTable(dt as DataTable, true);
                grdTalukdetails.DataBind();
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
                        if (Convert.ToString(dataView.Sort) == "TQ_CODE ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TQ_CODE")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["TalkDetails"] = dataView.ToTable();
                        }
                        else if (Convert.ToString(dataView.Sort) == "TQ_CODE DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TQ_CODE")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["TalkDetails"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["TalkDetails"] = dataView.ToTable();
                        }

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        if (Convert.ToString(dataView.Sort) == "TQ_CODE ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TQ_CODE")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["TalkDetails"] = dataView.ToTable();
                        }
                        else if (Convert.ToString(dataView.Sort) == "TQ_CODE DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TQ_CODE")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["TalkDetails"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["TalkDetails"] = dataView.ToTable();
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

        protected void grdTalukdetails_Sorting(object sender, GridViewSortEventArgs e)
        {

            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdTalukdetails.PageIndex;
            DataTable dt = (DataTable)ViewState["TalkDetails"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdTalukdetails.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdTalukdetails.DataSource = dt;
            }
            grdTalukdetails.DataBind();
            grdTalukdetails.PageIndex = pageIndex;




        }

        protected void grdTalukdetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                LoadTalukDetails();

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDistName = (TextBox)row.FindControl("txtDistrictName");
                    TextBox txtTalukCode = (TextBox)row.FindControl("txtTalukCode");
                    DataTable dt = (DataTable)ViewState["TalkDetails"];
                    dv = dt.DefaultView;

                    if (txtDistName.Text != "")
                    {
                        sFilter += " DT_NAME Like '%" + txtDistName.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtTalukCode.Text != "")
                    {
                        sFilter += " TQ_CODE Like '%" + txtTalukCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdTalukdetails.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdTalukdetails.DataSource = dv;
                            ViewState["TalkDetails"] = dv.ToTable();
                            grdTalukdetails.DataBind();

                        }
                        else
                        {
                            ViewState["TalkDetails"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadTalukDetails();
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
                dt.Columns.Add("TQ_SLNO");
                dt.Columns.Add("DT_NAME");
                dt.Columns.Add("TQ_CODE");
                dt.Columns.Add("TQ_NAME");
              


                grdTalukdetails.DataSource = dt;
                grdTalukdetails.DataBind();

                int iColCount = grdTalukdetails.Rows[0].Cells.Count;
                grdTalukdetails.Rows[0].Cells.Clear();
                grdTalukdetails.Rows[0].Cells.Add(new TableCell());
                grdTalukdetails.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdTalukdetails.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }
        }


        protected void Export_clickTaluk(object sender, EventArgs e)
        {
            //      DataTable dt = (DataTable)ViewState["DistDetails"]; dt = new DataTable();
            //    clsTaluk objTalk = new clsTaluk();
            //    dt = objTalk.LoadAllTalkDetails();

            DataTable dt = (DataTable)ViewState["TalkDetails"];



            if (dt.Rows.Count > 0)
            {

                dt.Columns["TQ_CODE"].ColumnName = "Taluk Code";
                dt.Columns["TQ_NAME"].ColumnName = "Taluk Name";
                dt.Columns["DT_NAME"].ColumnName = "District Name";

                List<string> listtoRemove = new List<string> { "TQ_SLNO" };
                string filename = "TalukDetails" + DateTime.Now + ".xls";
                string pagetitle = "Taluk Details";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");
                ShowEmptyGrid();
            }

        }
        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "Taluk";
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
                //lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;

            }
        }

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
                //lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}