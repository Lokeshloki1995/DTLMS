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
    public partial class SubDivisionView : System.Web.UI.Page
    {
        string strFormCode = "SubDivisionView";
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
                    Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone);
                    CheckAccessRights("4");
                    LoadSubDivisionOffices();
                }
            }
            
        }

        public void LoadSubDivisionOffices()
        {
            try
            {
                clsSubDiv ObjSubDivOffice = new clsSubDiv();
                DataTable dt = new DataTable();
                dt = ObjSubDivOffice.LoadSubDivOffDet();
                grdZoneOffice.DataSource = dt;
                grdZoneOffice.DataBind();
                ViewState["SubDivision"] = dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdZoneOffice_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
          
            grdZoneOffice.PageIndex = e.NewPageIndex;
            DataTable dt = (DataTable)ViewState["SubDivision"];

            grdZoneOffice.DataSource = SortDataTable(dt as DataTable, true);
            grdZoneOffice.DataBind();
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
                        ViewState["SubDivision"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["SubDivision"] = dataView.ToTable();

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

        protected void grdZoneOffice_Sorting(object sender, GridViewSortEventArgs e)
        {


            int columnIndex = 0;
            foreach (DataControlFieldHeaderCell headerCell in grdZoneOffice.HeaderRow.Cells)
            {
                if (headerCell.ContainingField.SortExpression == e.SortExpression)
                {
                    columnIndex = grdZoneOffice.HeaderRow.Cells.GetCellIndex(headerCell);
                }
            }

            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdZoneOffice.PageIndex;
            DataTable dt = (DataTable)ViewState["SubDivision"];
            string sortingDirection = string.Empty;

            Image sortImage = new Image();
            Image sortImageboth = new Image();


            if (dt.Rows.Count > 0)
            {
                grdZoneOffice.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdZoneOffice.DataSource = dt;
            }

            grdZoneOffice.DataBind();
            grdZoneOffice.PageIndex = pageIndex;
        }
        protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
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

                String strSubDivisionId = (Convert.ToString(((HiddenField)rw.FindControl("hfID")).Value));
                strSubDivisionId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(strSubDivisionId));

                Response.Redirect("SubDivision.aspx?SubDivId=" + strSubDivisionId + "", false);
            }
            catch (Exception ex)
            {

                //.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void imbBtnDelete_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton imgDel = (ImageButton)sender;
            GridViewRow rw = (GridViewRow)imgDel.NamingContainer;
        }


        protected void cmdNewSubDivision_Click(object sender, EventArgs e)
        {
            try
            {
                //Check AccessRights
                bool bAccResult = CheckAccessRights("2");
                if (bAccResult == false)
                {
                    return;
                }
                Response.Redirect("SubDivision.aspx", false);
            }
            catch (Exception ex)
            {
                // lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdZoneOffice_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();
                    LoadSubDivisionOffices();
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtCircleName = (TextBox)row.FindControl("txtcircleName");
                    TextBox txtDivName = (TextBox)row.FindControl("txtDivisionName");
                    TextBox txtSubDivName = (TextBox)row.FindControl("txtsubDivisionName");
                    TextBox txtSubDivCode = (TextBox)row.FindControl("txtSubDivCode");

                    DataTable dt = (DataTable)ViewState["SubDivision"];
                    dv = dt.DefaultView;

                    if (txtCircleName.Text != "")
                    {
                        sFilter = "CM_CIRCLE_NAME Like '%" + txtCircleName.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtDivName.Text != "")
                    {
                        sFilter += "DIV_NAME Like '%" + txtDivName.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtSubDivName.Text != "")
                    {
                        sFilter += " SD_SUBDIV_NAME Like '%" + txtSubDivName.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtSubDivCode.Text != "")
                    {
                        sFilter += " SD_SUBDIV_CODE Like '%" + txtSubDivCode.Text.Replace("'", "'") + "%' AND";
                    }

                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdZoneOffice.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdZoneOffice.DataSource = dv;
                            ViewState["SubDivision"] = dv.ToTable();
                            grdZoneOffice.DataBind();

                        }
                        else
                        {
                            ViewState["SubDivision"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadSubDivisionOffices();
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
                dt.Columns.Add("SD_ID");
                dt.Columns.Add("CM_CIRCLE_NAME");
                dt.Columns.Add("DIV_NAME");
                dt.Columns.Add("SD_SUBDIV_NAME");
                dt.Columns.Add("SD_SUBDIV_CODE");
                dt.Columns.Add("SD_MOBILE");
                dt.Columns.Add("SD_HEAD_EMP");
               
                grdZoneOffice.DataSource = dt;
                grdZoneOffice.DataBind();

                int iColCount = grdZoneOffice.Rows[0].Cells.Count;
                grdZoneOffice.Rows[0].Cells.Clear();
                grdZoneOffice.Rows[0].Cells.Add(new TableCell());
                grdZoneOffice.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdZoneOffice.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {

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

                objApproval.sFormName = "SubDivision";
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

        protected void Export_clicKSubDivision(object sender, EventArgs e)
        {
            //clsSubDiv ObjSubDivOffice = new clsSubDiv();
            //DataTable dt = new DataTable();
            //dt = ObjSubDivOffice.LoadSubDivOffDet();

            DataTable dt = (DataTable)ViewState["SubDivision"];

            if (dt.Rows.Count > 0)
            {

                dt.Columns["CM_CIRCLE_NAME"].ColumnName = "CIRCLE NAME";
                dt.Columns["DIV_NAME"].ColumnName = "DIVISION NAME";
                dt.Columns["SD_SUBDIV_NAME"].ColumnName = "SUB-DIVISION NAME";
                dt.Columns["SD_SUBDIV_CODE"].ColumnName = "SUB-DIVISION CODE";
                dt.Columns["SD_MOBILE"].ColumnName = "Mobile No";
                dt.Columns["SD_HEAD_EMP"].ColumnName = "Office Head";
                dt.Columns["SD_ADDRESS"].ColumnName = "SUB-DIVISION ADDRESS";

                dt.Columns["CIRCLE NAME"].SetOrdinal(0);
                dt.Columns["DIVISION NAME"].SetOrdinal(1);
                dt.Columns["SUB-DIVISION NAME"].SetOrdinal(2);


                List<string> listtoRemove = new List<string> { "SD_ID", "SD_TQ_ID", "SD_PHONE", "SD_EMAIL", "SD_DIV_CODE" };
                string filename = "SubDivisionDetails" + DateTime.Now + ".xls";
                string pagetitle = "SubDivision Details";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");
                ShowEmptyGrid();
            }

        }

        protected void cmbZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbZone.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_ZO_ID\"='" + cmbZone.SelectedValue + "'", "--Select--", cmbCircle);
                    cmbDivision.Items.Clear();
                }
                else
                {
                    cmbCircle.Items.Clear();
                    cmbDivision.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE \"DIV_CICLE_CODE\"='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDivision);                  
                }

                else
                {
                    cmbDivision.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                string sFilter = string.Empty;
                DataView dv = new DataView();
                LoadSubDivisionOffices();               

                DataTable dt = (DataTable)ViewState["SubDivision"];
                dv = dt.DefaultView;

                if(cmbDivision.SelectedIndex > 0)
                {
                    sFilter = "SD_SUBDIV_CODE Like '" + cmbDivision.SelectedValue.Replace("'", "'") + "%' AND";
                }
                else if(cmbCircle.SelectedIndex > 0)
                {
                    sFilter = "SD_SUBDIV_CODE Like '" + cmbCircle.SelectedValue.Replace("'", "'") + "%' AND";
                }
                else if (cmbZone.SelectedIndex > 0)
                {
                    sFilter = "SD_SUBDIV_CODE Like '" + cmbZone.SelectedValue.Replace("'", "'") + "%' AND";
                }
                else
                {
                    sFilter = "SD_SUBDIV_CODE Like '%' AND";
                }

                if (sFilter.Length > 0)
                {
                    sFilter = sFilter.Remove(sFilter.Length - 3);
                    grdZoneOffice.PageIndex = 0;
                    dv.RowFilter = sFilter;
                    if (dv.Count > 0)
                    {
                        grdZoneOffice.DataSource = dv;
                        ViewState["SubDivision"] = dv.ToTable();
                        grdZoneOffice.DataBind();

                    }
                    else
                    {
                        ViewState["SubDivision"] = dv.ToTable();
                        ShowEmptyGrid();
                    }
                }
                else
                {
                    LoadSubDivisionOffices();
                }
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}