using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using ClosedXML.Excel;
using System.IO;

namespace IIITS.DTLMS.TCRepair
{
    public partial class DeliverPendingSearch : System.Web.UI.Page
    {
        string strFormCode = "DeliverPendingSearch";
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
                    if (CheckAccessRights("4"))
                    {
                        string stroffCode = string.Empty;
                        string stroffCode1 = string.Empty;
                        stroffCode = clsStoreOffice.GetZone_Circle_Div_Offcode(objSession.OfficeCode, objSession.RoleId);
                        stroffCode1 = stroffCode;

                    //    Genaral.Load_Combo("SELECT DISTINCT \"TR_ID\", \"TR_NAME\"  FROM \"TBLTRANSREPAIRER\",\"TBLTRANSREPAIREROFFCODE\" WHERE \"TR_STATUS\"='A' AND \"TR_ID\"=\"TRO_TR_ID\" AND \"TR_ID\" NOT IN (SELECT \"TR_ID\" FROM \"TBLTRANSREPAIRER\",\"TBLTRANSREPAIREROFFCODE\" WHERE  \"TR_ID\"=\"TRO_TR_ID\" AND \"TR_BLACK_LISTED\" =1 AND \"TR_BLACKED_UPTO\" >= NOW()) AND  CAST(\"TRO_OFF_CODE\" AS TEXT) LIKE '" + stroffCode.Substring(0, 2) + "%' ORDER BY \"TR_NAME\" ", "--SELECT--", cmbRepairer);

                        Genaral.Load_Combo("SELECT DISTINCT \"TR_ID\", \"TR_NAME\"  FROM \"TBLTRANSREPAIRER\",\"TBLTRANSREPAIREROFFCODE\" WHERE \"TR_STATUS\"='A' AND \"TR_ID\"=\"TRO_TR_ID\" AND \"TR_ID\" NOT IN (SELECT \"TR_ID\" FROM \"TBLTRANSREPAIRER\",\"TBLTRANSREPAIREROFFCODE\" WHERE  \"TR_ID\"=\"TRO_TR_ID\" AND \"TR_BLACK_LISTED\" =1 AND \"TR_BLACKED_UPTO\" >= NOW()) ORDER BY \"TR_NAME\" ", "--SELECT--", cmbRepairer);

                        Genaral.Load_Combo("SELECT  \"TS_ID\", \"TS_NAME\"  FROM \"TBLTRANSSUPPLIER\"  WHERE \"TS_STATUS\" ='A' AND \"TS_ID\" NOT IN (SELECT \"TS_ID\" FROM \"TBLTRANSSUPPLIER\" WHERE \"TS_BLACK_LISTED\"=1 AND \"TS_BLACKED_UPTO\" >= NOW()) ORDER BY \"TS_NAME\" ", "--SELECT--", cmbSupplier);

                        string strQry = string.Empty;
                        clsDTrRepairActivity objTcRepair = new clsDTrRepairActivity();
                        LoadTestingPassedDetails();
                        LoadTestingPendingDetails();
                        if (objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                        {
                            txtsstoreid.Text = clsStoreOffice.GetStoreID(objSession.OfficeCode);
                        }

                        if (objSession.RoleId == Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SupAdminRole"]))
                        {
                            strQry = "Title=Search and Select Purchase Order Details&";
                            strQry += "Query=SELECT DISTINCT \"RSM_PO_NO\",\"RSM_INV_NO\" FROM \"TBLREPAIRSENTMASTER\",\"TBLREPAIRSENTDETAILS\",\"TBLINSPECTIONDETAILS\" WHERE CAST(\"RSM_DIV_CODE\" AS TEXT) LIKE '" + txtsstoreid.Text + "%' AND \"RSM_ID\"=\"RSD_RSM_ID\" ";
                            strQry += " AND \"RSD_DELIVARY_DATE\" IS NULL AND \"IND_RSD_ID\"=\"RSD_ID\" and {0} like %{1}% &";
                            strQry += "DBColName=CAST(\"RSM_PO_NO\" AS TEXT)~CAST(\"RSM_INV_NO\" AS TEXT)&";
                            strQry += "ColDisplayName=PO No~Invoice No&";
                        }
                        else
                        {
                            strQry = "Title=Search and Select Purchase Order Details&";
                            //strQry += "Query=SELECT DISTINCT \"RSM_PO_NO\",\"RSM_INV_NO\" FROM \"TBLREPAIRSENTMASTER\",\"TBLREPAIRSENTDETAILS\",\"TBLINSPECTIONDETAILS\" WHERE CAST(\"RSM_DIV_CODE\" AS TEXT) LIKE '" + objSession.OfficeCode + "%' AND \"RSM_ID\"=\"RSD_RSM_ID\" ";
                            strQry += "Query=SELECT DISTINCT \"RSM_PO_NO\",\"RSM_INV_NO\" FROM \"TBLREPAIRSENTMASTER\",\"TBLREPAIRSENTDETAILS\",\"TBLINSPECTIONDETAILS\" WHERE CAST(\"RSM_DIV_CODE\" AS TEXT) LIKE '" + objTcRepair.getstrcode(objSession.OfficeCode) + "%' AND \"RSM_ID\"=\"RSD_RSM_ID\" ";
                            strQry += " AND \"RSD_DELIVARY_DATE\" IS NULL AND \"IND_RSD_ID\"=\"RSD_ID\" and {0} like %{1}% &";
                            strQry += "DBColName=CAST(\"RSM_PO_NO\" AS TEXT)~CAST(\"RSM_INV_NO\" AS TEXT)&";
                            strQry += "ColDisplayName=PO No~Invoice No&";
                        }
                        strQry = strQry.Replace("'", @"\'");

                        cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtWoNo.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + txtWoNo.ClientID + ")");
                    }
                    
                }
            }
            catch (Exception ex)
            {               
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        private void LoadTestingPassedDetails()
        {
            try
            {
                clsDTrRepairActivity objDeliverPending = new clsDTrRepairActivity();            
                
                if (cmbRepairer.SelectedIndex > 0)
                {
                    objDeliverPending.sRepairerId = cmbRepairer.SelectedValue;
                }
                if (cmbSupplier.SelectedIndex > 0)
                {
                    objDeliverPending.sSupplierId = cmbSupplier.SelectedValue;
                }

                objDeliverPending.sPurchaseOrderNo = txtWoNo.Text.Trim();
              

                objDeliverPending.sOfficeCode = clsStoreOffice.GetZone_Circle_Div_Offcode(objSession.OfficeCode, objSession.RoleId);
                objDeliverPending.sTestingDone = "1";
                objDeliverPending.UserId = objSession.UserId;

                DataTable dt = new DataTable();
                dt = objDeliverPending.LoadTestingPassedDetails(objDeliverPending);
             
                grdTestingPass.DataSource = dt;
                grdTestingPass.DataBind();
                ViewState["TestPass"] = dt;
                CheckPendingquantity();


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

                objApproval.sFormName = "ReceivePending";
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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;

            }
        }

        private void LoadTestingPendingDetails()
        {
            try
            {
                clsDTrRepairActivity objDeliverPending = new clsDTrRepairActivity();

                if (cmbRepairer.SelectedIndex > 0)
                {
                    objDeliverPending.sRepairerId = cmbRepairer.SelectedValue;
                }
                if (cmbSupplier.SelectedIndex > 0)
                {
                    objDeliverPending.sSupplierId = cmbSupplier.SelectedValue;
                }

                objDeliverPending.sPurchaseOrderNo = txtWoNo.Text.Trim();

                objDeliverPending.sOfficeCode = clsStoreOffice.GetZone_Circle_Div_Offcode(objSession.OfficeCode, objSession.RoleId);
                objDeliverPending.sTestingDone = "1";

                DataTable dt = new DataTable();
                dt = objDeliverPending.LoadPendingForTestingDetails(objDeliverPending);

                grdTestPending.DataSource = dt;
                grdTestPending.DataBind();
                ViewState["TestPending"] = dt;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public DataSet getDataSetExportToExcel()
        {
            DataSet ds = new DataSet();
            try
            {
                
                clsDTrRepairActivity objDeliverPending = new clsDTrRepairActivity();

                if (cmbRepairer.SelectedIndex > 0)
                {
                    objDeliverPending.sRepairerId = cmbRepairer.SelectedValue;
                }
                if (cmbSupplier.SelectedIndex > 0)
                {
                    objDeliverPending.sSupplierId = cmbSupplier.SelectedValue;
                }

                objDeliverPending.sPurchaseOrderNo = txtWoNo.Text.Trim();

                objDeliverPending.sOfficeCode = objSession.OfficeCode;
                objDeliverPending.sTestingDone = "1";

                DataTable dttesting = new DataTable();
                dttesting = objDeliverPending.LoadPendingForTestingDetails(objDeliverPending);



                if (cmbRepairer.SelectedIndex > 0)
                {
                    objDeliverPending.sRepairerId = cmbRepairer.SelectedValue;
                }
                if (cmbSupplier.SelectedIndex > 0)
                {
                    objDeliverPending.sSupplierId = cmbSupplier.SelectedValue;
                }

                objDeliverPending.sPurchaseOrderNo = txtWoNo.Text.Trim();


                objDeliverPending.sOfficeCode = objSession.OfficeCode;
                objDeliverPending.sTestingDone = "1";

                DataTable dttestingpassed = new DataTable();
                dttestingpassed = objDeliverPending.LoadTestingPassedDetails(objDeliverPending);

                DataTable dt = dttesting.Copy();
                DataTable dt1 = dttestingpassed.Copy();
                dt.TableName = "PendingForTesting";
                dt1.TableName = "TestingPassedDetails";

                ds.Tables.Add(dt);

                ds.Tables.Add(dt1);
               
               
            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            return ds;
           
            
        }

        protected void Export_ClickPendingTesting(object sender, EventArgs e)
        {
            //clsDTrRepairActivity objDeliverPending = new clsDTrRepairActivity();

            //if (cmbRepairer.SelectedIndex > 0)
            //{
            //    objDeliverPending.sRepairerId = cmbRepairer.SelectedValue;
            //}
            //if (cmbSupplier.SelectedIndex > 0)
            //{
            //    objDeliverPending.sSupplierId = cmbSupplier.SelectedValue;
            //}

            //objDeliverPending.sPurchaseOrderNo = txtWoNo.Text.Trim();

            //objDeliverPending.sOfficeCode = objSession.OfficeCode;
            //objDeliverPending.sTestingDone = "1";

            //DataTable dt = new DataTable();
            //dt = objDeliverPending.LoadPendingForTestingDetails(objDeliverPending);


            DataTable dt = new DataTable();
            if (ViewState["TestPending"] != null)
            {
                dt = (DataTable)ViewState["TestPending"];
            }
            else
            {
                clsDTrRepairActivity objDeliverPending = new clsDTrRepairActivity();

                objDeliverPending.sOfficeCode = objSession.OfficeCode;
                objDeliverPending.sTestingDone = "1";
                objDeliverPending.sPurchaseOrderNo = "";
                dt = objDeliverPending.LoadPendingForTestingDetails(objDeliverPending);
            }
            if (dt.Rows.Count > 0)
            {

                //dt.Columns["RSM_PO_NO"].ColumnName = "pono";
                dt.Columns["PONO"].ColumnName = "Po No";
                dt.Columns["PODATE"].ColumnName = "Po Date";
                dt.Columns["ISSUEDATE"].ColumnName = "IssueDate";
                dt.Columns["SUP_REPNAME"].ColumnName = "Supplier/Repairer";
                dt.Columns["PO_QUANTITY"].ColumnName = "Total Quantity";
                dt.Columns["PENDING_QNTY"].ColumnName = "Pending Qty for Testing";
                // dt.Columns["DELIVERED_QNTY"].ColumnName = "Recieved Quantity";

                //dt.Columns["PO No"].ColumnName = "RSM_PO_NO";
                //dt.Columns["PO Date"].ColumnName = "PODATE";
                //dt.Columns["Issue Date"].ColumnName = "ISSUEDATE";
                //dt.Columns["Supplier/Repairer"].ColumnName = "SUP_REPNAME";
                //dt.Columns["Total Quantity"].ColumnName = "PO_QUANTITY";
                //dt.Columns["Pending Qty for Testing"].ColumnName = "PENDING_QNTY";


                //dttesting.Columns["MAKE NAME"].SetOrdinal(3);
                List<string> listtoRemove = new List<string> { "DELIVERED_QNTY" };
                // List<string> listtoRemove = new List<string>() ; 
                string filename = "PendingForTestingDetails" + DateTime.Now + ".xls";
                string pagetitle = "Pending For Testing Details";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");
            }
        }

        protected void Export_ClickDeliverPendingSearch(object sender, EventArgs e)
        {

            //clsDTrRepairActivity objDeliverPending = new clsDTrRepairActivity();

            //if (cmbRepairer.SelectedIndex > 0)
            //{
            //    objDeliverPending.sRepairerId = cmbRepairer.SelectedValue;
            //}
            //if (cmbSupplier.SelectedIndex > 0)
            //{
            //    objDeliverPending.sSupplierId = cmbSupplier.SelectedValue;
            //}

            //objDeliverPending.sPurchaseOrderNo = txtWoNo.Text.Trim();


            //objDeliverPending.sOfficeCode = objSession.OfficeCode;
            //objDeliverPending.sTestingDone = "1";

            //DataTable dt = new DataTable();
            //dt = objDeliverPending.LoadTestingPassedDetails(objDeliverPending);

            DataTable dt = (DataTable)ViewState["TestPass"];

            if (dt.Rows.Count > 0)
            {

                dt.Columns["RSM_PO_NO"].ColumnName = "PO No";
                dt.Columns["PODATE"].ColumnName = "PO Date";
                dt.Columns["ISSUEDATE"].ColumnName = "Issue Date";
                dt.Columns["SUP_REPNAME"].ColumnName = "Supplier/Repairer";
                dt.Columns["PO_QUANTITY"].ColumnName = "Total Quantity";
                dt.Columns["PENDING_QNTY"].ColumnName = "Pending Qty for Recieve";
                dt.Columns["DELIVERED_QNTY"].ColumnName = "Recieved Quantity";


               // dttestingpassed.Columns["MAKE NAME"].SetOrdinal(3);
                List<string> listtoRemove = new List<string> {  "RSM_ID" };
                string filename = "TestingPassedDetails" + DateTime.Now + ".xls";
                string pagetitle = "Testing Passed Details";


                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");
            }


        }
        //protected void Export_ClickDeliverPendingSearch(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DataSet ds = getDataSetExportToExcel();

        //        if (ds.Tables[0].Rows.Count > 0 || ds.Tables[1].Rows.Count > 0)
        //        {

        //            //foreach (DataTable dt in ds.Tables)
        //            //{
        //                DataTable dt = new DataTable();
        //                DataRow newRow = dt.NewRow();
        //                dt.Rows.Add(newRow);

        //                dt.Columns.Add("RSM_PO_NO");
        //                dt.Columns.Add("PODATE");
        //                dt.Columns.Add("ISSUEDATE");
        //                dt.Columns.Add("SUP_REPNAME");
        //                dt.Columns.Add("PO_QUANTITY");
        //                dt.Columns.Add("PENDING_QNTY");
        //                dt.Columns.Add("DELIVERED_QNTY");
        //            //}



        //        }



               
        //        if (ds.Tables[0].Rows.Count > 0 || ds.Tables[1].Rows.Count > 0)
        //        {
        //            using (XLWorkbook wb = new XLWorkbook())
        //            {
        //                 //DataTable dt = new DataTable();

        //                //int Count = 1;
        //                 foreach (DataTable dt in ds.Tables)
        //                {

        //                   // dt.Columns.Remove("RSM_ID");

        //                    wb.Worksheets.Add(dt);

        //                    //wb.Worksheet(1).Row(1).InsertRowsAbove(2);
                           

        //                }

        //                 HttpContext.Current.Response.Clear();
        //                 HttpContext.Current.Response.Buffer = true;
        //                 HttpContext.Current.Response.Charset = "";
        //                 string FileName = "DeliverPendingSearch" + DateTime.Now + ".xls";
        //                 HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //                 HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + FileName);

        //                 using (MemoryStream MyMemoryStream = new MemoryStream())
        //                 {
        //                     wb.SaveAs(MyMemoryStream);
        //                     MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
        //                     HttpContext.Current.Response.Flush();
        //                     HttpContext.Current.Response.End();
        //                 }

        //                //Export the Excel file.
                        
        //            }
        //        }

        //        else
        //        {
        //            ShowMsgBox("no records found");
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

       


        //protected void Export_ClickDeliverPendingSearch(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DataSet ds = getDataSetExportToExcel();

        //        if (ds.Tables[0].Rows.Count > 0 || ds.Tables[1].Rows.Count > 0)
        //        {
        //            string filename = "DeliverPending";
        //            Genaral.getexcelds(ds, filename);
        //        }
        //        else
        //        {
        //            ShowMsgBox("No record found");
        //        }

               
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Export_ClickTcsearch");
        //    }


        //}

        protected void imgBtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton imgEdit = (ImageButton)sender;
                GridViewRow rw = (GridViewRow)imgEdit.NamingContainer;

                String strTCcode = ((Label)rw.FindControl("lblTcCode")).Text;
                strTCcode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(strTCcode));
                Response.Redirect("DeliverTC.aspx?QryTccode=" + strTCcode + "");
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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
          
            txtWoNo.Text = string.Empty;
            cmbRepairer.SelectedIndex = 0;
         
            //grdTestingPass.DataSource = null;
            //grdTestingPass.DataBind();
            cmbSupplier.SelectedIndex = 0;
        }

        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                LoadTestingPassedDetails();
                LoadTestingPendingDetails();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void grdTestingPass_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Recieve")
                {
                   
                    if(objSession.RoleId=="43")
                    {
                        ShowMsgBox("Sorry You Are Not Authorized To Access");
                        return;
                    }
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    Label lblRepairMasterId = (Label)row.FindControl("lblRepairMasterId");
                    Label lblInsResultId = (Label)row.FindControl("lblInsResult");

                  //  grdDelivery.Columns[17].Visible = false;
                    //if (grdTestingPass.Column.Contain)
                    string sRepairMasterId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblRepairMasterId.Text));
                    string sInsResultId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblRepairMasterId.Text));
                    Response.Redirect("DeliverTC.aspx?InsResult="+sInsResultId+"&RepairMasterId=" + sRepairMasterId, false);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }



        public void CheckPendingquantity()
        {
            try
            {
                foreach (GridViewRow grd in grdTestingPass.Rows)
                {
                    Label lblpending = (Label)(grd.FindControl("lblPendingQty"));
                   


                    if (lblpending.Text.Contains('0'))
                    {
                        
                        ImageButton img = (ImageButton)grd.FindControl("imgbtnRecieve");
                        img.Enabled = false;
                       
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }







        private void PopulateCheckedValues()
        {
            try
            {
                ArrayList arrCheckedValues = (ArrayList)ViewState["CHECKED_ITEMS"];
                if (arrCheckedValues != null && arrCheckedValues.Count > 0)
                {
                    foreach (GridViewRow gvrow in grdTestPending.Rows)
                    {
                        int index = Convert.ToInt32(grdTestPending.DataKeys[gvrow.RowIndex].Values[0]);
                        if (arrCheckedValues.Contains(index))
                        {
                            CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkSelect");
                            myCheckBox.Checked = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }



        protected void grdTestPending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdTestPending.PageIndex = e.NewPageIndex;
                PopulateCheckedValues();
                DataTable dt = (DataTable)ViewState["TestPending"];
                grdTestPending.DataSource = SortDataTablePending(dt as DataTable, true);
                grdTestPending.DataBind();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdTestingPass_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                grdTestingPass.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["TestPass"];
                grdTestingPass.DataSource = SortDataTablePassed(dt as DataTable, true);
                grdTestingPass.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdTestingPass_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdTestingPass.PageIndex;
            DataTable dt = (DataTable)ViewState["TestPass"];
            string sortingDirection = string.Empty;

            grdTestingPass.DataSource = SortDataTablePassed(dt as DataTable, false);
            grdTestingPass.DataBind();
            grdTestingPass.PageIndex = pageIndex;
        }
        protected void grdTestPending_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdTestPending.PageIndex;
            DataTable dt = (DataTable)ViewState["TestPending"];
            string sortingDirection = string.Empty;

            grdTestPending.DataSource = SortDataTablePending(dt as DataTable, false);
            grdTestPending.DataBind();
            grdTestPending.PageIndex = pageIndex;
        }

        protected DataView SortDataTablePassed(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);

                        ViewState["TestPass"] = dataView.ToTable();
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());

                        ViewState["TestPass"] = dataView.ToTable();
                    }


                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }
        protected DataView SortDataTablePending(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        ViewState["TestPending"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["TestPending"] = dataView.ToTable();

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

      
    }
}