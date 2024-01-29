using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using IIITS.DTLMS.BL;

namespace IIITS.DTLMS.TCRepair
{
    public partial class ViewPODetails : System.Web.UI.Page
    {
        string strFormCode = "ViewPODetails";
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
                    string strQry = string.Empty;

                    strQry = "Title=Search and Select Purchase Order Details&";
                    strQry += "Query=SELECT DISTINCT \"RSM_PO_NO\",\"RSM_INV_NO\" FROM \"TBLREPAIRSENTMASTER\",\"TBLREPAIRSENTDETAILS\" WHERE CAST(\"RSM_DIV_CODE\" AS TEXT) ";
                    //strQry += "LIKE '" + objSession.OfficeCode + "%' AND \"RSM_ID\"=\"RSD_RSM_ID\" ";
                    strQry += "LIKE '" + clsStoreOffice.Getofficecode(objSession.OfficeCode) + "%' AND \"RSM_ID\"=\"RSD_RSM_ID\" ";
                    strQry += " AND {0} like %{1}% &";
                    strQry += "DBColName=CAST(\"RSM_PO_NO\" AS TEXT)~CAST(\"RSM_INV_NO\" AS TEXT)&";
                    strQry += "ColDisplayName=PO No~Invoice No&";

                    strQry = strQry.Replace("'", @"\'");

                    cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtPONo.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + txtPONo.ClientID + ")");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                loadPoDetails();
                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void loadPoDetails()
        {
            DataTable dt = new DataTable();
            try
            {
                clsDTrRepairActivity objPoDetails = new clsDTrRepairActivity();
                objPoDetails.sPurchaseOrderNo = txtPONo.Text;
                int Division = Convert.ToInt32(ConfigurationSettings.AppSettings["Division_code"]);
                objPoDetails.sOfficeCode = objSession.OfficeCode;
                objPoDetails.sroletype = objSession.sRoleType;
                objPoDetails.sroleid = objSession.RoleId;
                dt = objPoDetails.GetRepairPoDetails(objPoDetails);


                if (dt.Rows.Count > 0)
                {
                    ViewState["podetails"] = dt;
                    grdRepairerPoDetails.DataSource = SortDataTable(dt as DataTable, true);
                    grdRepairerPoDetails.DataBind();
                }
                else
                {
                    ViewState["podetails"] = dt;
                    grdRepairerPoDetails.DataSource = dt; 
                    grdRepairerPoDetails.DataBind();
                }

                //ViewState["podetails"] = dt;
                //grdRepairerPoDetails.DataSource = SortDataTable(dt as DataTable, true);
                //grdRepairerPoDetails.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void Export_clickPODetails(object sender, EventArgs e)
        {
            //DataTable dt = new DataTable();
            //clsDTrRepairActivity objPoDetails = new clsDTrRepairActivity();
            //objPoDetails.sPurchaseOrderNo = txtPONo.Text;
            //objPoDetails.sOfficeCode = objSession.OfficeCode.Substring(0, 2);
            //dt = objPoDetails.GetRepairPoDetails(objPoDetails);
            DataTable dt = (DataTable)ViewState["podetails"];

            if (dt.Rows.Count > 0)
            {

                dt.Columns["TC_CODE"].ColumnName = "DTR Code";
                dt.Columns["TC_SLNO"].ColumnName = "DTR SlNo";
                dt.Columns["TM_NAME"].ColumnName = "Make Name";
                dt.Columns["TC_CAPACITY"].ColumnName = "Capacity(in KVA)";
                dt.Columns["TC_MANF_DATE"].ColumnName = "Manufacture Date";
                dt.Columns["RSM_GUARANTY_TYPE"].ColumnName = "Guaranty Type";
                dt.Columns["STATUS"].ColumnName = "Status";

               // dt.Columns["FEEDER NAME"].SetOrdinal(0);

                List<string> listtoRemove = new List<string> {""};
                string filename = "RepairPoDetails" + DateTime.Now + ".xls";
                string pagetitle = "Repair Po Details";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");
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

        protected void grdRepairerPoDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdRepairerPoDetails.PageIndex = e.NewPageIndex;
                loadPoDetails();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

         protected void grdRepairerPoDetails_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdRepairerPoDetails.PageIndex;
            DataTable dt = (DataTable)ViewState["podetails"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdRepairerPoDetails.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdRepairerPoDetails.DataSource = dt;
            }
            grdRepairerPoDetails.DataBind();
            grdRepairerPoDetails.PageIndex = pageIndex;
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
                            ViewState["podetails"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["podetails"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["podetails"] = dataView.ToTable();
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
                            ViewState["podetails"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["podetails"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["podetails"] = dataView.ToTable();
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

      
    

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtPONo.Text = string.Empty;
                grdRepairerPoDetails.DataSource = null;
                grdRepairerPoDetails.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}