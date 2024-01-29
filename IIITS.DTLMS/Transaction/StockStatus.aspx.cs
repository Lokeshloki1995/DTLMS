using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IIITS.DTLMS.BL;

namespace IIITS.DTLMS.Transaction
{
    public partial class StockStatus : System.Web.UI.Page
    {
        string strFormCode = "StockStatus";

        private double Ten = 0;
        private double Fifteen = 0;
        private double Twentyfive = 0;
        private double Fifty = 0;
        private double SixThree = 0;
        private double Hund = 0;
        private double OnetwentyFive = 0;
        private double OneFifty = 0;
        private double OneSixty = 0;
        private double TwoHun = 0;
        private double TwoFifty = 0;
        private double ThreeHun = 0;
        private double ThreeFifteen = 0;
        private double FourHun = 0;
        private double FiveHun = 0;
        private double SixThirty = 0;
        private double SevenFifty = 0;
        private double Thousand = 0;
        private double ThousandTwoFifty = 0;
        private double GrandTotal = 0;

        clsSession objsession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                objsession = (clsSession)Session["clsSession"];
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }

                if (!IsPostBack)
                {
                  
                    Genaral.Load_Combo("SELECT \"MD_NAME\",\"MD_NAME\" from \"TBLMASTERDATA\" WHERE \"MD_TYPE\"='C' ORDER BY \"MD_ORDER_BY\" ", "-Select-", cmbCapacity);
                    Genaral.Load_Combo("SELECT \"SM_ID\",\"SM_NAME\" from \"TBLSTOREMAST\" ORDER BY \"SM_ID\" ", "-Select-", cmdStore);
                    LoadStockDetails();

                }
               
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
             
        }

       

        public void LoadStockDetails(string sStoreName = "",string sLocation = "")
        {
           try
            {
                clsStockStatus ObjStore = new clsStockStatus();
                ObjStore.sStoreName = sStoreName;
                ObjStore.sStorelocation = sLocation;

                if (cmbCapacity.SelectedIndex == 0)
                {
                    ObjStore.sCapacity = string.Empty;
                }
                else
                {
                    ObjStore.sCapacity = cmbCapacity.SelectedItem.Text.Trim();
                }

                if (cmdStore.SelectedIndex == 0)
                {
                    if (objsession.OfficeCode != "")
                    {
                        ObjStore.sStoreId = objsession.OfficeCode;
                    }
                    else
                    {
                        ObjStore.sStoreId = "";
                    }
                }
                else
                {
                    ObjStore.sStoreId = cmdStore.SelectedValue;
                }
                ObjStore.roletype = objsession.sRoleType;
               DataTable dtStoreDetails = new DataTable();
                dtStoreDetails = ObjStore.LoadStockDetails(ObjStore);

                Ten = Convert.ToDouble(dtStoreDetails.Compute("SUM(" + Convert.ToString(dtStoreDetails.Columns["a"]) + ")", ""));
                Fifteen = Convert.ToDouble(dtStoreDetails.Compute("SUM(" + Convert.ToString(dtStoreDetails.Columns["b"]) + ")", ""));
                Twentyfive = Convert.ToDouble(dtStoreDetails.Compute("SUM(" + Convert.ToString(dtStoreDetails.Columns["C"]) + ")", ""));
                Fifty = Convert.ToDouble(dtStoreDetails.Compute("SUM(" + Convert.ToString(dtStoreDetails.Columns["D"]) + ")", ""));
                SixThree = Convert.ToDouble(dtStoreDetails.Compute("SUM(" + Convert.ToString(dtStoreDetails.Columns["E"]) + ")", ""));
                Hund = Convert.ToDouble(dtStoreDetails.Compute("SUM(" + Convert.ToString(dtStoreDetails.Columns["F"]) + ")", ""));
                OnetwentyFive = Convert.ToDouble(dtStoreDetails.Compute("SUM(" + Convert.ToString(dtStoreDetails.Columns["G"]) + ")", ""));
                OneFifty = Convert.ToDouble(dtStoreDetails.Compute("SUM(" + Convert.ToString(dtStoreDetails.Columns["H"]) + ")", ""));
                OneSixty = Convert.ToDouble(dtStoreDetails.Compute("SUM(" + Convert.ToString(dtStoreDetails.Columns["I"]) + ")", ""));
                TwoHun = Convert.ToDouble(dtStoreDetails.Compute("SUM(" + Convert.ToString(dtStoreDetails.Columns["J"]) + ")", ""));
                TwoFifty = Convert.ToDouble(dtStoreDetails.Compute("SUM(" + Convert.ToString(dtStoreDetails.Columns["K"]) + ")", ""));
                ThreeHun = Convert.ToDouble(dtStoreDetails.Compute("SUM(" + Convert.ToString(dtStoreDetails.Columns["L"]) + ")", ""));
                ThreeFifteen = Convert.ToDouble(dtStoreDetails.Compute("SUM(" + Convert.ToString(dtStoreDetails.Columns["M"]) + ")", ""));
                FourHun = Convert.ToDouble(dtStoreDetails.Compute("SUM(" + Convert.ToString(dtStoreDetails.Columns["N"]) + ")", ""));
                FiveHun = Convert.ToDouble(dtStoreDetails.Compute("SUM(" + Convert.ToString(dtStoreDetails.Columns["O"]) + ")", ""));
                SixThirty = Convert.ToDouble(dtStoreDetails.Compute("SUM(" + Convert.ToString(dtStoreDetails.Columns["P"]) + ")", ""));
                SevenFifty = Convert.ToDouble(dtStoreDetails.Compute("SUM(" + Convert.ToString(dtStoreDetails.Columns["Q"]) + ")", ""));
                Thousand = Convert.ToDouble(dtStoreDetails.Compute("SUM(" + Convert.ToString(dtStoreDetails.Columns["R"]) + ")", ""));
                ThousandTwoFifty = Convert.ToDouble(dtStoreDetails.Compute("SUM(" + Convert.ToString(dtStoreDetails.Columns["S"]) + ")", ""));
                GrandTotal = Convert.ToDouble(dtStoreDetails.Compute("SUM(" + Convert.ToString(dtStoreDetails.Columns["TOTAL"]) + ")", ""));

                grdStockDetails.DataSource = dtStoreDetails;
                grdStockDetails.DataBind();

                ViewState["Stock"] = dtStoreDetails;


                //if (dtStoreDetails.Rows.Count <= 0)
                //{
                //    DataTable dtFeederDetails = new DataTable();
                //    DataRow newRow = dtFeederDetails.NewRow();
                //    dtFeederDetails.Rows.Add(newRow);
                //    dtFeederDetails.Columns.Add("SM_ID");
                //    dtFeederDetails.Columns.Add("SM_NAME");
                //   // dtFeederDetails.Columns.Add("SM_OFF_CODE");
                //    dtFeederDetails.Columns.Add("TC_CAPACITY");
                //    dtFeederDetails.Columns.Add("TC_CODE");

                //    grdStockStatus.DataSource = dtFeederDetails;
                //    grdStockStatus.DataBind();
                //    int iColCount = grdStockStatus.Rows[0].Cells.Count;
                //    grdStockStatus.Rows[0].Cells.Clear();
                //    grdStockStatus.Rows[0].Cells.Add(new TableCell());
                //    grdStockStatus.Rows[0].Cells[0].ColumnSpan = iColCount;
                //    grdStockStatus.Rows[0].Cells[0].Text = "No Records Found";

                //    ViewState["Stock"] = dtStoreDetails;

                //}
                //else
                //{
                //    dtStoreDetails.Rows.RemoveAt(dtStoreDetails.Rows.Count - 1);
                //    grdStockStatus.DataSource = dtStoreDetails;
                //    grdStockStatus.DataBind();
                //    ViewState["Stock"] = dtStoreDetails;
                //}
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
                        ViewState["Stock"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["Stock"] = dataView.ToTable();
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

        protected void grdStockStatus_Sorting(object sender, GridViewSortEventArgs e)
        {
         
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdStockStatus.PageIndex;
            DataTable dt = (DataTable)ViewState["Stock"];
            string sortingDirection = string.Empty;

            

            if (dt.Rows.Count > 0)
            {

                grdStockStatus.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdStockStatus.DataSource = dt;
            }
            grdStockStatus.DataBind();
            grdStockStatus.PageIndex = pageIndex;
        }

        //protected void grdStockStatus_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    try
        //    {

        //        if (e.CommandName == "search")
        //        {
        //            GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
        //            TextBox txtSearchName = (TextBox)row.FindControl("txtstoreName");
        //            TextBox txtLocation = (TextBox)row.FindControl("txtLocation");
        //            LoadStockDetails(txtSearchName.Text.Trim().Replace("'", ""), txtLocation.Text.Trim().Replace("'", ""));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdUser_RowCommand");
        //    }
        //}
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

        protected void Export_clickStockStatus(object sender, EventArgs e)
        {


            DataTable dt = (DataTable)ViewState["Stock"];

            if (dt.Rows.Count > 0)
            {


                dt.Columns["SM_NAME"].ColumnName = "Store Name";
                dt.Columns["a"].ColumnName = "10";
                dt.Columns["b"].ColumnName = "15";
                dt.Columns["c"].ColumnName = "25";
                dt.Columns["d"].ColumnName = "50";
                dt.Columns["e"].ColumnName = "63";
                dt.Columns["f"].ColumnName = "100";
                dt.Columns["g"].ColumnName = "125";
                dt.Columns["h"].ColumnName = "150";
                dt.Columns["i"].ColumnName = "160";
                dt.Columns["j"].ColumnName = "200";
                dt.Columns["k"].ColumnName = "250";
                dt.Columns["l"].ColumnName = "300";
                dt.Columns["m"].ColumnName = "315";

                dt.Columns["n"].ColumnName = "400";
                dt.Columns["o"].ColumnName = "500";
                dt.Columns["p"].ColumnName = "630";
                dt.Columns["q"].ColumnName = "750";
                dt.Columns["r"].ColumnName = "1000";
                dt.Columns["s"].ColumnName = "1250";
                //dt.Columns["SM_OFF_CODE"].ColumnName = "Location";
                //  dt.Columns["TC_CAPACITY"].ColumnName = "Capacity(in KVA)";
                //  dt.Columns["TC_CODE"].ColumnName = "StockCount";

                // dt.Columns["SUPPLIER NAME"].SetOrdinal(3);
                List<string> listtoRemove = new List<string> { "" };
                string filename = "Stock" + DateTime.Now + ".xls";
                string pagetitle = "Stock Status Details";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);


            }
            else
            {
                ShowMsgBox("No record found");

                DataTable dtFeederDetails = new DataTable();
                DataRow newRow = dtFeederDetails.NewRow();
                dtFeederDetails.Rows.Add(newRow);
                //dtFeederDetails.Columns.Add("SM_ID");
                dtFeederDetails.Columns.Add("SM_NAME");
               // dtFeederDetails.Columns.Add("SM_OFF_CODE");
                dtFeederDetails.Columns.Add("TC_CAPACITY");
                dtFeederDetails.Columns.Add("TC_CODE");

                grdStockStatus.DataSource = dtFeederDetails;
                grdStockStatus.DataBind();
                int iColCount = grdStockStatus.Rows[0].Cells.Count;
                grdStockStatus.Rows[0].Cells.Clear();
                grdStockStatus.Rows[0].Cells.Add(new TableCell());
                grdStockStatus.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdStockStatus.Rows[0].Cells[0].Text = "No Records Found";

            }
            


        }


        protected void grdStockStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdStockStatus.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Stock"];
                dt.Columns["SM_NAME"].AllowDBNull = true;
                dt.Columns["SM_OFF_CODE"].AllowDBNull = true;
                grdStockStatus.DataSource = SortDataTable(dt as DataTable, true);
                grdStockStatus.DataBind();

            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdStockStatus_DataBound1(object sender, EventArgs e)
        {
            for (int i = grdStockStatus.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = grdStockStatus.Rows[i];
                GridViewRow previousRow = grdStockStatus.Rows[i - 1];
                for (int j = 0; j < row.Cells.Count-1; j++)
                {
                    if (row.Cells[j].Text == previousRow.Cells[j].Text)
                    {
                        if (previousRow.Cells[j].RowSpan == 0)
                        {
                            if (row.Cells[j].RowSpan == 0)
                            {
                                previousRow.Cells[j].RowSpan += 2;
                            }
                            else
                            {
                                previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                            }
                            row.Cells[j].Visible = false;
                        }
                    }
                }
            }
        }

        protected void cmdStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStockDetails();
        }

        protected void cmbCapacity_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStockDetails();
        }

        protected void grdStockDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ((Label)e.Row.FindControl("lblf10")).Text = Convert.ToString(Ten);
                ((Label)e.Row.FindControl("lblf15")).Text = Convert.ToString(Fifteen);
                ((Label)e.Row.FindControl("lblf25")).Text = Convert.ToString(Twentyfive);
                ((Label)e.Row.FindControl("lblf50")).Text = Convert.ToString(Fifty);
                ((Label)e.Row.FindControl("lblf63")).Text = Convert.ToString(SixThree);
                ((Label)e.Row.FindControl("lblf100")).Text = Convert.ToString(Hund);
                ((Label)e.Row.FindControl("lblf125")).Text = Convert.ToString(OnetwentyFive);
                ((Label)e.Row.FindControl("lblf150")).Text = Convert.ToString(OneFifty);
                ((Label)e.Row.FindControl("lblf160")).Text = Convert.ToString(OneSixty);
                ((Label)e.Row.FindControl("lblf200")).Text = Convert.ToString(TwoHun);
                ((Label)e.Row.FindControl("lblf250")).Text = Convert.ToString(TwoFifty);
                ((Label)e.Row.FindControl("lblf300")).Text = Convert.ToString(ThreeHun);
                ((Label)e.Row.FindControl("lblf315")).Text = Convert.ToString(ThreeFifteen);
                ((Label)e.Row.FindControl("lblf400")).Text = Convert.ToString(FourHun);
                ((Label)e.Row.FindControl("lblf500")).Text = Convert.ToString(FiveHun);
                ((Label)e.Row.FindControl("lblf630")).Text = Convert.ToString(SixThirty);
                ((Label)e.Row.FindControl("lblf750")).Text = Convert.ToString(SevenFifty);
                ((Label)e.Row.FindControl("lblf1000")).Text = Convert.ToString(Thousand);
                ((Label)e.Row.FindControl("lblf1250")).Text = Convert.ToString(ThousandTwoFifty);
                ((Label)e.Row.FindControl("lblfTOTAL")).Text = Convert.ToString(GrandTotal);
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if (Convert.ToInt32(((LinkButton)e.Row.FindControl("lbl10")).Text) > 0)
                {
                    ((LinkButton)e.Row.FindControl("lbl10")).ForeColor = System.Drawing.Color.Red;
                    ((LinkButton)e.Row.FindControl("lbl10")).Font.Bold = true;
                }
                if (Convert.ToInt32(((LinkButton)e.Row.FindControl("lbl15")).Text) > 0)
                {
                    ((LinkButton)e.Row.FindControl("lbl15")).ForeColor = System.Drawing.Color.Red;
                    ((LinkButton)e.Row.FindControl("lbl15")).Font.Bold = true;
                }
                if (Convert.ToInt32(((LinkButton)e.Row.FindControl("lbl25")).Text) > 0)
                {
                    ((LinkButton)e.Row.FindControl("lbl25")).ForeColor = System.Drawing.Color.Red;
                    ((LinkButton)e.Row.FindControl("lbl25")).Font.Bold = true;
                }
                if (Convert.ToInt32(((LinkButton)e.Row.FindControl("lbl50")).Text) > 0)
                {
                    ((LinkButton)e.Row.FindControl("lbl50")).ForeColor = System.Drawing.Color.Red;
                    ((LinkButton)e.Row.FindControl("lbl50")).Font.Bold = true;
                }
                if (Convert.ToInt32(((LinkButton)e.Row.FindControl("lbl63")).Text) > 0)
                {
                    ((LinkButton)e.Row.FindControl("lbl63")).ForeColor = System.Drawing.Color.Red;
                    ((LinkButton)e.Row.FindControl("lbl63")).Font.Bold = true;
                }
                if (Convert.ToInt32(((LinkButton)e.Row.FindControl("lbl100")).Text) > 0)
                {
                    ((LinkButton)e.Row.FindControl("lbl100")).ForeColor = System.Drawing.Color.Red;
                    ((LinkButton)e.Row.FindControl("lbl100")).Font.Bold = true;
                }
                if (Convert.ToInt32(((LinkButton)e.Row.FindControl("lbl125")).Text) > 0)
                {
                    ((LinkButton)e.Row.FindControl("lbl125")).ForeColor = System.Drawing.Color.Red;
                    ((LinkButton)e.Row.FindControl("lbl125")).Font.Bold = true;
                }
                if (Convert.ToInt32(((LinkButton)e.Row.FindControl("lbl150")).Text) > 0)
                {
                    ((LinkButton)e.Row.FindControl("lbl150")).ForeColor = System.Drawing.Color.Red;
                    ((LinkButton)e.Row.FindControl("lbl150")).Font.Bold = true;
                }
                if (Convert.ToInt32(((LinkButton)e.Row.FindControl("lbl160")).Text) > 0)
                {
                    ((LinkButton)e.Row.FindControl("lbl160")).ForeColor = System.Drawing.Color.Red;
                    ((LinkButton)e.Row.FindControl("lbl160")).Font.Bold = true;
                }
                if (Convert.ToInt32(((LinkButton)e.Row.FindControl("lbl200")).Text) > 0)
                {
                    ((LinkButton)e.Row.FindControl("lbl200")).ForeColor = System.Drawing.Color.Red;
                    ((LinkButton)e.Row.FindControl("lbl200")).Font.Bold = true;
                }
                if (Convert.ToInt32(((LinkButton)e.Row.FindControl("lbl250")).Text) > 0)
                {
                    ((LinkButton)e.Row.FindControl("lbl250")).ForeColor = System.Drawing.Color.Red;
                    ((LinkButton)e.Row.FindControl("lbl250")).Font.Bold = true;
                }
                if (Convert.ToInt32(((LinkButton)e.Row.FindControl("lbl300")).Text) > 0)
                {
                    ((LinkButton)e.Row.FindControl("lbl300")).ForeColor = System.Drawing.Color.Red;
                    ((LinkButton)e.Row.FindControl("lbl300")).Font.Bold = true;
                }
                if (Convert.ToInt32(((LinkButton)e.Row.FindControl("lbl315")).Text) > 0)
                {
                    ((LinkButton)e.Row.FindControl("lbl315")).ForeColor = System.Drawing.Color.Red;
                    ((LinkButton)e.Row.FindControl("lbl315")).Font.Bold = true;
                }
                if (Convert.ToInt32(((LinkButton)e.Row.FindControl("lbl400")).Text) > 0)
                {
                    ((LinkButton)e.Row.FindControl("lbl400")).ForeColor = System.Drawing.Color.Red;
                    ((LinkButton)e.Row.FindControl("lbl400")).Font.Bold = true;
                }
                if (Convert.ToInt32(((LinkButton)e.Row.FindControl("lbl500")).Text) > 0)
                {
                    ((LinkButton)e.Row.FindControl("lbl500")).ForeColor = System.Drawing.Color.Red;
                    ((LinkButton)e.Row.FindControl("lbl500")).Font.Bold = true;
                }
                if (Convert.ToInt32(((LinkButton)e.Row.FindControl("lbl630")).Text) > 0)
                {
                    ((LinkButton)e.Row.FindControl("lbl630")).ForeColor = System.Drawing.Color.Red;
                    ((LinkButton)e.Row.FindControl("lbl630")).Font.Bold = true;
                }
                if (Convert.ToInt32(((LinkButton)e.Row.FindControl("lbl750")).Text) > 0)
                {
                    ((LinkButton)e.Row.FindControl("lbl750")).ForeColor = System.Drawing.Color.Red;
                    ((LinkButton)e.Row.FindControl("lbl750")).Font.Bold = true;
                }
                if (Convert.ToInt32(((LinkButton)e.Row.FindControl("lbl1000")).Text) > 0)
                {
                    ((LinkButton)e.Row.FindControl("lbl1000")).ForeColor = System.Drawing.Color.Red;
                    ((LinkButton)e.Row.FindControl("lbl1000")).Font.Bold = true;
                }
                if (Convert.ToInt32(((LinkButton)e.Row.FindControl("lbl1250")).Text) > 0)
                {
                    ((LinkButton)e.Row.FindControl("lbl1250")).ForeColor = System.Drawing.Color.Red;
                    ((LinkButton)e.Row.FindControl("lbl1250")).Font.Bold = true;
                }
                if (Convert.ToInt32(((LinkButton)e.Row.FindControl("lblTOTAL")).Text) > 0)
                {
                    ((LinkButton)e.Row.FindControl("lblTOTAL")).ForeColor = System.Drawing.Color.Red;
                    ((LinkButton)e.Row.FindControl("lblTOTAL")).Font.Bold = true;
                }




                //// Check your condition here
                //if (!e.Row.Cells[1].Text.Equals("0"))
                //{
                //    e.Row.Cells[1].Font.Bold = true; // This will make row bold
                //    e.Row.Cells[1].ForeColor = System.Drawing.Color.Red;
                //}
                //if (!e.Row.Cells[2].Text.Equals("0"))
                //{
                //    e.Row.Cells[2].Font.Bold = true; // This will make row bold
                //    e.Row.Cells[2].ForeColor = System.Drawing.Color.Red;
                //}
                //if (!e.Row.Cells[3].Text.Equals("0"))
                //{
                //    e.Row.Cells[3].Font.Bold = true; // This will make row bold
                //    e.Row.Cells[3].ForeColor = System.Drawing.Color.Red;
                //}
                //if (!e.Row.Cells[4].Text.Equals("0"))
                //{
                //    e.Row.Cells[4].Font.Bold = true; // This will make row bold
                //    e.Row.Cells[4].ForeColor = System.Drawing.Color.Red;
                //}
                //if (!e.Row.Cells[5].Text.Equals("0"))
                //{
                //    e.Row.Cells[5].Font.Bold = true; // This will make row bold
                //    e.Row.Cells[5].ForeColor = System.Drawing.Color.Red;
                //}
                //if (!e.Row.Cells[6].Text.Equals("0"))
                //{
                //    e.Row.Cells[6].Font.Bold = true; // This will make row bold
                //    e.Row.Cells[6].ForeColor = System.Drawing.Color.Red;
                //}
                //if (!e.Row.Cells[7].Text.Equals("0"))
                //{
                //    e.Row.Cells[7].Font.Bold = true; // This will make row bold
                //    e.Row.Cells[7].ForeColor = System.Drawing.Color.Red;
                //}
                //if (!e.Row.Cells[8].Text.Equals("0"))
                //{
                //    e.Row.Cells[8].Font.Bold = true; // This will make row bold
                //    e.Row.Cells[8].ForeColor = System.Drawing.Color.Red;
                //}
                //if (!e.Row.Cells[9].Text.Equals("0"))
                //{
                //    e.Row.Cells[9].Font.Bold = true; // This will make row bold
                //    e.Row.Cells[9].ForeColor = System.Drawing.Color.Red;
                //}
                //if (!e.Row.Cells[10].Text.Equals("0"))
                //{
                //    e.Row.Cells[10].Font.Bold = true; // This will make row bold
                //    e.Row.Cells[10].ForeColor = System.Drawing.Color.Red;
                //}
                //if (!e.Row.Cells[11].Text.Equals("0"))
                //{
                //    e.Row.Cells[11].Font.Bold = true; // This will make row bold
                //    e.Row.Cells[11].ForeColor = System.Drawing.Color.Red;
                //}
                //if (!e.Row.Cells[12].Text.Equals("0"))
                //{
                //    e.Row.Cells[12].Font.Bold = true; // This will make row bold
                //    e.Row.Cells[12].ForeColor = System.Drawing.Color.Red;
                //}
                //if (!e.Row.Cells[13].Text.Equals("0"))
                //{
                //    e.Row.Cells[13].Font.Bold = true; // This will make row bold
                //    e.Row.Cells[13].ForeColor = System.Drawing.Color.Red;
                //}
                //if (!e.Row.Cells[14].Text.Equals("0"))
                //{
                //    e.Row.Cells[14].Font.Bold = true; // This will make row bold
                //    e.Row.Cells[14].ForeColor = System.Drawing.Color.Red;
                //}
                //if (!e.Row.Cells[15].Text.Equals("0"))
                //{
                //    e.Row.Cells[15].Font.Bold = true; // This will make row bold
                //    e.Row.Cells[15].ForeColor = System.Drawing.Color.Red;
                //}
                //if (!e.Row.Cells[16].Text.Equals("0"))
                //{
                //    e.Row.Cells[16].Font.Bold = true; // This will make row bold
                //    e.Row.Cells[16].ForeColor = System.Drawing.Color.Red;
                //}
                //if (!e.Row.Cells[17].Text.Equals("0"))
                //{
                //    e.Row.Cells[17].Font.Bold = true; // This will make row bold
                //    e.Row.Cells[17].ForeColor = System.Drawing.Color.Red;
                //}
                //if (!e.Row.Cells[18].Text.Equals("0"))
                //{
                //    e.Row.Cells[18].Font.Bold = true; // This will make row bold
                //    e.Row.Cells[18].ForeColor = System.Drawing.Color.Red;
                //}
                //if (!e.Row.Cells[19].Text.Equals("0"))
                //{
                //    e.Row.Cells[19].Font.Bold = true; // This will make row bold
                //    e.Row.Cells[19].ForeColor = System.Drawing.Color.Red;
                //}
                //if (!e.Row.Cells[10].Text.Equals("0"))
                //{
                //    e.Row.Cells[19].Font.Bold = true; // This will make row bold
                //    e.Row.Cells[19].ForeColor = System.Drawing.Color.Red;
                //}
            }
        }

        protected void grdStockDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if(e.CommandName=="View")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton lblsmname = (LinkButton)row.FindControl("lblsmname");
                    LinkButton lblTOTAL = (LinkButton)row.FindControl("lblTOTAL");
                    
                   
                    if(lblTOTAL.Text == "0")
                    {
                        ShowMsgBox("No Transformers Exist in this Store");
                        return;
                    }

                    string sStore = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblsmname.Text));
                    string url = "/Transaction/StockDetails.aspx?StoreID=" + sStore;
                    string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                    ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
                }
            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}