using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;

namespace IIITS.DTLMS.BasicForms
{
    public partial class StationView : System.Web.UI.Page
    {
        static string strStationId = "0";
        string strFormCode = "StationView";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
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
                        LoadStation();
                    }
                }
                
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadStation()
        {
            try
            {
                clsStation objStation = new clsStation();
                DataTable dt = new DataTable();
                string sLocation = string.Empty;
                if(cmbSubdiv.SelectedIndex > 0)
                {
                    sLocation = cmbSubdiv.SelectedValue;
                }
                else if(cmbDivision.SelectedIndex > 0)
                {
                    sLocation = cmbDivision.SelectedValue;
                }
                else if (cmbCircle.SelectedIndex > 0)
                {
                    sLocation = cmbCircle.SelectedValue;
                }
                else if (cmbZone.SelectedIndex > 0)
                {
                    sLocation = cmbZone.SelectedValue;
                }
                dt = objStation.LoadStationDet("", sLocation);
                grdStation.DataSource = dt;
                grdStation.DataBind();
                ViewState["Station"] = dt;
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
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

                strStationId = (Convert.ToString(((HiddenField)rw.FindControl("hfID")).Value));
                strStationId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(strStationId));
                Response.Redirect("Station.aspx?StationId=" + strStationId + "", false);

            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        /// <summary>
        /// protected void imbBtnDelete_Click(object sender, ImageClickEventArgs e)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbBtnDelete_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton imgDel = (ImageButton)sender;
            GridViewRow rw = (GridViewRow)imgDel.NamingContainer;


        }

        protected void grdStation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdStation.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Station"];
                grdStation.DataSource = SortDataTable(dt as DataTable, true);
                grdStation.DataBind();
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
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
                        
                            ViewState["Station"] = dataView.ToTable();
                        

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        
                            ViewState["Station"] = dataView.ToTable();
                        

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

        protected void grdStation_Sorting(object sender, GridViewSortEventArgs e)
        {

            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdStation.PageIndex;
            DataTable dt = (DataTable)ViewState["Station"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdStation.DataSource = SortDataTable(dt as DataTable, false);
            }

            else
            {
                grdStation.DataSource = dt;
            }

            grdStation.DataBind();
            grdStation.PageIndex = pageIndex;
            



        }

        protected void grdStation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                LoadStation();
                if (e.CommandName == "search")
                {
                    GridViewRow row = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;

                    TextBox txtStationName = (TextBox)row.FindControl("txtStationName");
                    TextBox txtStationCode = (TextBox)row.FindControl("txtStationCode");
                    
                    DataTable dt = (DataTable)ViewState["Station"];
                    DataView dv = new DataView();
                    //dv.Table = dt;
                    dv = dt.DefaultView;
                    
                    string sFilter = string.Empty;

                    if (txtStationCode.Text != "")
                    {
                        sFilter = " ST_STATION_CODE like '" + txtStationCode.Text.Trim().ToUpper() + "%' AND";
                    }

                    if (txtStationName.Text != "")
                    {
                        sFilter = " ST_NAME like '" + txtStationName.Text.Trim().ToUpper() + "%' AND";
                    }

                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdStation.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdStation.DataSource = dv;
                            ViewState["Station"] = dv.ToTable();
                            grdStation.DataBind();

                        }
                        else
                        {
                            ViewState["Station"] = dv.ToTable();

                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        grdStation.DataSource = dv;
                        grdStation.DataBind();
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
                dt.Columns.Add("ST_ID");
                dt.Columns.Add("ST_NAME");
                dt.Columns.Add("ST_STATION_CODE");
                dt.Columns.Add("STC_CAP_VALUE");
                dt.Columns.Add("OFFNAME");
                dt.Columns.Add("ST_DESCRIPTION");

                grdStation.DataSource = dt;
                grdStation.DataBind();

                int iColCount = grdStation.Rows[0].Cells.Count;
                grdStation.Rows[0].Cells.Clear();
                grdStation.Rows[0].Cells.Add(new TableCell());
                grdStation.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdStation.Rows[0].Cells[0].Text = "No Records Found";

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

                objApproval.sFormName = "Station";
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
                lblErrormsg.Text = clsException.ErrorMsg();
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
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdNewStation_Click(object sender, EventArgs e)
        {
            try
            {
                //Check AccessRights
                bool bAccResult = CheckAccessRights("2");
                if (bAccResult == false)
                {
                    return;
                }
                Response.Redirect("Station.aspx", false);

            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
       

        protected void Export_clickStation(object sender, EventArgs e)
        {
            //clsStation objStation = new clsStation();
            //DataTable dt = new DataTable();
            //dt = objStation.LoadStationDet();

            DataTable dt = (DataTable)ViewState["Station"];


            if (dt.Rows.Count > 0)
            {

                dt.Columns["ST_NAME"].ColumnName = "Station name";
                dt.Columns["STC_CAP_VALUE"].ColumnName = "Voltage Class";
                dt.Columns["ST_DESCRIPTION"].ColumnName = "Description";

                List<string> listtoRemove = new List<string> { "ST_ID", "ST_STATION_CODE", "ST_PARENT_STATID" };
                string filename = "StationDetails" + DateTime.Now + ".xls";
                string pagetitle = "Station Details";

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
                    cmbSubdiv.Items.Clear();
                }
                else
                {
                    cmbCircle.Items.Clear();
                    cmbDivision.Items.Clear();
                    cmbSubdiv.Items.Clear();
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
                    cmbSubdiv.Items.Clear();
                }

                else
                {
                    cmbDivision.Items.Clear();
                    cmbSubdiv.Items.Clear();

                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDivision.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE \"SD_DIV_CODE\"='" + cmbDivision.SelectedValue + "'", "--Select--", cmbSubdiv);
                   
                }
                else
                {
                    cmbSubdiv.Items.Clear();
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
                LoadStation();
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}