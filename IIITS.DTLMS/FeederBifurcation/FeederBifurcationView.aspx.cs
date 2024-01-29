using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI.WebControls;
using System.Data;
using IIITS.DTLMS.BL;
using System.Collections;
using System.Configuration;


namespace IIITS.DTLMS.FeederBifurcation
{
    public partial class FeederBifurcationView : System.Web.UI.Page
    {
        string strFormCode = "FeederBifurcationView";
        clsSession objsession;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")    
            {
                Response.Redirect("~/Login.aspx", false);
            }
            //Genaral.Load_Combo("", );
            objsession = (clsSession)Session["clsSession"];

            if (!Page.IsPostBack)
            {
                CheckAccessRights("2", "1");
                LoadGridView();
            }

            //if (!IsPostBack)
            //{
            //    CheckAccessRights("2", "1");
            //    LoadGridView();
               
            //}
        }

        public  void LoadGridView()
        {
            try
            {
                clsDtcMaster obj = new clsDtcMaster();
                obj.sOfficeCode = objsession.OfficeCode;
                DataTable dt = obj.GetFeederBfcnRecords(obj);
                ViewState["FeederBifurcation"] = dt;
                    grdFdrView.DataSource = dt;
                    grdFdrView.DataBind();
                

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadGridView");
            }
        }

        public bool CheckAccessRights(string sAccessType, string flag)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "FeederBifurcationSO";
                objApproval.sRoleId = objsession.RoleId;
                objApproval.sAccessType = "3" + "," + sAccessType;
                bool bResult = objApproval.CheckAccessRights(objApproval);
                if (flag == "2")
                {
                    //&& objSession.UserId != "39"
                    if (UserValid() == false)
                    {
                        if (bResult == true)
                        {
                            Response.Redirect("~/UserRestrict.aspx", false);
                            bResult = false;
                        }
                    }

                }
                else if (flag == "1")
                {
                    if (UserValid() == false)
                    {
                        if (bResult == false)
                        {
                            Response.Redirect("~/UserRestrict.aspx", false);
                        }
                    }
                }

                return bResult;
            }
            catch (Exception ex)
            {
               
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CheckAccessRights");
                return false;

            }
        }

        public bool UserValid()
        {
            bool res = true;
            try
            {
                string Userid = Convert.ToString(ConfigurationSettings.AppSettings["SELECTEDUSER"]);
                string[] sUserid = Userid.Split(',');
                for (int i = 0; i < sUserid.Length; i++)
                {
                    if (objsession.UserId != sUserid[i])
                    {
                        res = false;
                    }
                    else
                    {
                        res = true;
                        return res;
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CheckAccessRights");
                return false;
            }
        }

        protected void grdFdrView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "Edit")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                    string strFBDId = ((Label)row.FindControl("lblFBS_ID")).Text;
                    strFBDId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(strFBDId));
                    Response.Redirect("FeederBifurcationSO.aspx?FBS_ID=" + strFBDId + "", false);

                }
                if (e.CommandName == "GenerateReport")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    string strFBDId = ((Label)row.FindControl("lblFBS_ID")).Text;
                    string strParam = "id=FeederBifurcationSO&FBS_Id=" + strFBDId;
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                }
                if (e.CommandName == "Search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtOldFeederCode = (TextBox)row.FindControl("txtOldFeederCode");
                    TextBox txtNewFeederCode = (TextBox)row.FindControl("txtNewFeederCode");
                    TextBox txtSectionOfficer = (TextBox)row.FindControl("txtSectionOfficer");
                    TextBox txtStatus = (TextBox)row.FindControl("txtStatus");
                    DataTable dt = (DataTable)ViewState["FeederBifurcation"];
                    dv = dt.DefaultView;
                    if (txtOldFeederCode.Text != "")
                    {
                        sFilter += " OLD_FEEDER_CODE Like '%" + txtOldFeederCode.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtNewFeederCode.Text != "")
                    {
                        sFilter += " NEW_FEEDER_CODE Like '%" + txtNewFeederCode.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtSectionOfficer.Text != "")
                    {
                        sFilter += " SECTION_OFFICER Like '%" + txtSectionOfficer.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtStatus.Text != "")
                    {
                        sFilter += " STATUS Like '%" + txtStatus.Text.Replace("'", "`") + "%' AND";
                    }

                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdFdrView.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdFdrView.DataSource = dv;
                            ViewState["FeederBifurcation"] = dv.ToTable();
                            grdFdrView.DataBind();
                        }
                        else
                        {
                            ViewState["FeederBifurcation"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdFdrView_RowCommand");

            }
        }
        protected void grdFdrView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //Set the edit index.
            grdFdrView.EditIndex = e.NewEditIndex;
        }
        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("FBS_ID");
                dt.Columns.Add("OLD_FEEDER_CODE");
                dt.Columns.Add("NEW_FEEDER_CODE");
                dt.Columns.Add("COUNT_DTC");
                dt.Columns.Add("SECTION_OFFICER");
                dt.Columns.Add("CREATED_ON");
                dt.Columns.Add("APPROVED_ON");
                dt.Columns.Add("APPROVED_BY");
                dt.Columns.Add("STATUS");
              

                grdFdrView.DataSource = dt;
                grdFdrView.DataBind();

                int iColCount = grdFdrView.Rows[0].Cells.Count;
                grdFdrView.Rows[0].Cells.Clear();
                grdFdrView.Rows[0].Cells.Add(new TableCell());
                grdFdrView.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdFdrView.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            { 
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowEmptyGrid");

            }
        }
        protected void grdFdrView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdFdrView.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["FeederBifurcation"];
                grdFdrView.DataSource = SortDataTable(dt as DataTable, true); 
                grdFdrView.DataBind();
            }
            catch (Exception ex)
            {
                //lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdFailureDetails_PageIndexChanging");
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
                        ViewState["FeederBifurcation"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["FeederBifurcation"] = dataView.ToTable();

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
        protected void grdFdrView_Editing(object sender, GridViewCommandEventArgs e)
        { }
        
    }
}