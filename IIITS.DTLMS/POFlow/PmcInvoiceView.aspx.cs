using IIITS.DTLMS.BL;
using IIITS.DTLMS.BL.POFlow;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using OfficeOpenXml;

namespace IIITS.DTLMS.POFlow
{
    public partial class PMCInvoiceView : System.Web.UI.Page
    {
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToString(Session["clsSession"] ?? "").Length == 0)
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                else
                {
                    objSession = (clsSession)Session["clsSession"];
                    if (!IsPostBack)
                    {
                        loadGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }

        /// <summary>
        /// method to load grid
        /// </summary>
        /// <param name="sId"></param>
        
        public void loadGrid(string sId = "")
        {
            try
            {
                clsPmcInvoice Obj = new clsPmcInvoice();
                DataTable dt = new DataTable();
                Obj.PmcId = sId;
                dt = Obj.GetPmcGridDetails(Obj);
                if (dt.Rows.Count > 0)
                {
                    grdPmc.DataSource = dt;
                    grdPmc.DataBind();
                }
                else
                {
                    ShowEmptyGrid();
                }
                ViewState["PMCinvoiceDetails"] = dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// method to load pmcdetails
        /// </summary>     
        public void LoadPMCDetails()
        {
            clsPmcInvoice Obj = new clsPmcInvoice();
            try
            {
                Obj.GetPmcGridDetails(Obj);
                if (Obj.DtPmcDetails.Rows.Count > 0)
                {
                    grdPmc.DataSource = Obj.DtPmcDetails;
                    grdPmc.DataBind();
                }
                else
                {
                    ShowEmptyGrid();
                }
                ViewState["PMCinvoiceDetails"] = Obj.DtPmcDetails;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }       
        protected void grdPmc_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                grdPmc.PageIndex = e.NewPageIndex;
                dt = (DataTable)ViewState["PMCinvoiceDetails"];
                grdPmc.DataSource = SortDataTable(dt as DataTable, true);
                grdPmc.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
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
                        ViewState["DistDetails"] = dataView.ToTable();
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["DistDetails"] = dataView.ToTable();
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
        /// <summary>
        /// method for search grid values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void grdPmc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "create")
                {
                    string InvoiceId = string.Empty;
                    string IndentId = string.Empty;

                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    Label lblInvoiceId = (Label)row.FindControl("lblInvoiceId");
                    Label lblIndentId = (Label)row.FindControl("lblIndentId");
                    InvoiceId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblInvoiceId.Text));
                    IndentId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblIndentId.Text));
                    Response.Redirect("PmcInvoiceCreate.aspx?InvoiceId=" + InvoiceId + "&IndentId=" + IndentId, false);
                }

                if (e.CommandName == "search")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtsDtcCode = (TextBox)row.FindControl("txtsDtcCode");
                    TextBox txtstcCode = (TextBox)row.FindControl("txtstcCode");
                    TextBox txtIndentno = (TextBox)row.FindControl("txtIndentno");
                    TextBox txtPOno = (TextBox)row.FindControl("txtPOno");
                    TextBox txtsInvoiceNo = (TextBox)row.FindControl("txtsInvoiceNo");
                    TextBox txtCapacity = (TextBox)row.FindControl("txtCapacity");
                    LoadInvoiceMasterDetails(txtsDtcCode.Text, txtstcCode.Text, txtIndentno.Text, txtPOno.Text, txtsInvoiceNo.Text, txtCapacity.Text);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }

        /// <summary>
        /// method to load Invoice details
        /// </summary>
        /// <param name="dtccode"></param>
        /// <param name="tccode"></param>
        /// <param name="indentno"></param>
        /// <param name="pono"></param>
        /// <param name="invoiceno"></param>
        /// 
        public void LoadInvoiceMasterDetails(string dtccode = "", string tccode = "", string indentno = "", string pono = "", string invoiceno = "",string Capacity ="")
        {
            try
            {
                clsPmcInvoice Obj = new clsPmcInvoice();
                DataTable dt = new DataTable();
                Obj.PmcDTCCode = dtccode;
                Obj.PmcDTrCode = tccode;
                Obj.PmcIndentno = indentno;
                Obj.PmcPONo = pono;
                Obj.PmcInvoiceno = invoiceno;
                Obj.PmcCapacity = Capacity;


                dt = Obj.GetPmcGridDetails(Obj);
                if (dt.Rows.Count > 0)
                {
                    grdPmc.DataSource = dt;
                    grdPmc.DataBind();

                }
                else
                {
                    ShowEmptyGrid();                    
                }
                ViewState["PMCinvoiceDetails"] = dt;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }
        
        /// <summary>
        /// method for showemptygrid and to show no records found if no records present
        /// </summary>      
        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("PMC_ID");
                dt.Columns.Add("PMC_PI_ID");
                dt.Columns.Add("PI_DTCCODE");
                dt.Columns.Add("PI_TC_CODE");
                dt.Columns.Add("PI_CAPACITY");
                dt.Columns.Add("TPIE_INDENT_NO");
                dt.Columns.Add("TPIE_INDENT_DATE");
                dt.Columns.Add("PMC_INVOICE_NO");
                dt.Columns.Add("PMC_INVOICE_DATE");
                dt.Columns.Add("TPIE_PO_NO");

                grdPmc.DataSource = dt;
                grdPmc.DataBind();
                int iColCount = grdPmc.Rows[0].Cells.Count;
                grdPmc.Rows[0].Cells.Clear();
                grdPmc.Rows[0].Cells.Add(new TableCell());
                grdPmc.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdPmc.Rows[0].Cells[0].Text = Convert.ToString(ConfigurationManager.AppSettings["EmptyData"]);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }

        /// <summary>
        /// Method to check accessrights
        /// </summary>
        /// <param name="sAccessType"></param>
        /// <returns></returns>
        
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY
                clsApproval objApproval = new clsApproval();
                objApproval.sFormName = MethodBase.GetCurrentMethod().DeclaringType.Name;
                objApproval.sRoleId = objSession.RoleId; // for Admin there is no role.
                objApproval.sAccessType = Constants.CheckAccessRights.CheckAccessRightsAll + "," + sAccessType; //  "1"
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
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
                return false;

            }
        }

        /// <summary>
        /// Method to export excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
    
        protected void Export_ClickFailureEntry(object sender, EventArgs e)
        {
            DataTable DtPmcDetails = (DataTable)ViewState["PMCinvoiceDetails"];
            try
            {
                if (DtPmcDetails.Rows.Count > 0)
                {
                    #region adding Slno
                    int Slno = 0;
                    DtPmcDetails.Columns.Add("SlNo", typeof(int));
                    // Iterate through the rows in the DataTable
                    foreach (DataRow row in DtPmcDetails.Rows)
                    {
                        #region adding Slno.
                        Slno = Slno + 1;
                        row["SlNo"] = Slno;
                        #endregion
                    }
                    #endregion
                  
                    DtPmcDetails.Columns["PI_DTCCODE"].ColumnName = "DTC Code";
                    DtPmcDetails.Columns["PI_TC_CODE"].ColumnName = "DTr Code";
                    DtPmcDetails.Columns["PI_CAPACITY"].ColumnName = "Capacity";
                    DtPmcDetails.Columns["TPIE_INDENT_NO"].ColumnName = "Indent No";
                    DtPmcDetails.Columns["TPIE_INDENT_DATE"].ColumnName = "Indent Date";
                    DtPmcDetails.Columns["PMC_INVOICE_NO"].ColumnName = "Invoice No";
                    DtPmcDetails.Columns["PMC_INVOICE_DATE"].ColumnName = "Invoice Date";
                    DtPmcDetails.Columns["TPIE_PO_NO"].ColumnName = "PO No";

                    DtPmcDetails.Columns["SlNo"].SetOrdinal(0);
                    DtPmcDetails.Columns["DTC Code"].SetOrdinal(1);
                    DtPmcDetails.Columns["DTr Code"].SetOrdinal(2);
                    DtPmcDetails.Columns["Capacity"].SetOrdinal(3);
                    DtPmcDetails.Columns["PO No"].SetOrdinal(4);
                    DtPmcDetails.Columns["Indent No"].SetOrdinal(5);
                    DtPmcDetails.Columns["Indent Date"].SetOrdinal(6);
                    DtPmcDetails.Columns["Invoice No"].SetOrdinal(7);
                    DtPmcDetails.Columns["Invoice Date"].SetOrdinal(8);

                    List<string> listtoRemove = new List<string> { "PMC_ID", "PMC_PI_ID" };
                    string filename = "Invoice Details" + DateTime.Now + ".xls";
                    string pagetitle = "Invoice Details";
                    Genaral.getexcel(DtPmcDetails, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowEmptyGrid();
                    ShowMsgBox("No Records Found");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }

        /// <summary>
        /// method for showmsgbox
        /// </summary>
        /// <param name="sMsg"></param>
      
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
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }
    }
}




