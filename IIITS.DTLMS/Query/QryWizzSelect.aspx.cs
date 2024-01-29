using IIITS.DTLMS.BL;
using IIITS.PGSQL.DAL;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.Query
{
    public partial class QryWizzSelect : System.Web.UI.Page
    {
        PGSqlConnection objCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
        clsSession objSession;
        string strFormCode = "SelectQryWizz";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
            {
                Response.Redirect("~/Login.aspx", false);
                return;
            }
            objSession = (clsSession)Session["clsSession"];

            if (!IsPostBack)
            {
                //if (objSession.UserType != "4" && objSession.UserType != "9")
                //if (objSession.RoleId != "38")
                //{
                //    Response.Redirect("~/UserRestrict.aspx", false);
                //    return;
                //}
                //else
                //{
                //    GetAllTableLists();
                //}
                CheckAccessRights("4");
                GetAllTableLists();
            }

        }
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "SelectQueryWizzard";
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
                // lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;

            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }

        public void GetAllTableLists()
        {
            DataTable dt = new DataTable();
            int iCount = 0;
            string sSql = string.Empty;
            objCon.BeginTransaction();
            String ConStr = System.Configuration.ConfigurationManager.ConnectionStrings["pgSQL"].ConnectionString;
            NpgsqlConnectionStringBuilder Builder = new NpgsqlConnectionStringBuilder(ConStr);
            String Database = Builder.Database;

            sSql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_CATALOG = '" + Database + "'";
     
            dt = objCon.FetchDataTable(sSql);
            if (dt.Rows.Count > 0)
            {
                TablesName.DataSource = dt;
                TablesName.DataBind();

                foreach (GridViewRow gvRow in TablesName.Rows)
                {
                    gvRow.Attributes.Add("draggable", "true");
                    gvRow.Attributes.Add("ondragstart", "drag(event)");
                    gvRow.Attributes.Add("id", iCount.ToString());
                    iCount++;
                }
            }
        }


        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            clsQrywizard objQry = new clsQrywizard();
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                    return;
                }
                resultdiv.Visible = true;
               
                GridQuery.Columns.Clear();
                GridQuery.DataBind();
                ViewState.Remove("SelectQryWizz");
                DataTable dt = new DataTable();
                string status = string.Empty;
                string strUserId = objSession.UserId;
                string uerquery = txtQuery.Text;
                if (uerquery.Length != 0)
                {

                    if (uerquery.Length > 10)
                    {
                        if (!uerquery.Trim().ToUpper().Substring(0, 10).Contains("SELECT") || uerquery.Trim().ToUpper().Contains("UPDATE ") || uerquery.Trim().ToUpper().Contains("DELETE ") || uerquery.Trim().ToUpper().Contains("INSERT "))
                        {
                            Label1.Text = "Only Provision to do Select, No Other Operations like Insert, Update, Delete etc.";

                            Label1.Visible = true;
                            ///   objQry.DatatoQueryLog(uerquery, strUserId, "S", Label1.Text);
                            string   strQuery = "INSERT INTO \"TBLQUERYLOG\"(\"QL_ID\",\"QL_TEXT\",\"QL_ENTRYAUTH\",\"QL_EXCECUTED_IN_FORM\",\"QL_DESCRIPTION\") VALUES ";
                            strQuery += " ('" + objCon.Get_max_no("QL_ID", "TBLQUERYLOG") + "','" + uerquery.Replace("'", "''") + "','" + objSession.UserId + "','S','"+Label1.Text+"')";
                            objCon.ExecuteQry(strQuery);
                            return;
                        }
                        Label1.Text = "";
                        string strQuerys = "INSERT INTO \"TBLQUERYLOG\"(\"QL_ID\",\"QL_TEXT\",\"QL_ENTRYAUTH\",\"QL_EXCECUTED_IN_FORM\",\"QL_DESCRIPTION\") VALUES ";
                        strQuerys += " ('" + objCon.Get_max_no("QL_ID", "TBLQUERYLOG") + "','" + uerquery.Replace("'", "''") + "','" + objSession.UserId + "','S','SUCCEED')";
                        objCon.ExecuteQry(strQuerys);
                        //  strQuerys.Trim();
                        // objQry.DatatoQueryLog(uerquery, strUserId, "S", "SUCCEED");

                        //dt = objCon.getDataTable(uerquery);
                        // dt = DBHelper.DBExecDataTable(sConString, uerquery);
                        //dt = objCon.FetchDataTable(uerquery);

                        //if (uerquery.Trim().ToUpper().Contains("TBLMOBILEREGISTER"))
                        //{
                        //    dt = objCon.FetchDataTable(uerquery);
                        //}
                        //else
                        //{
                        //    dt = objCon.FetchDataTable(uerquery.ToUpper());
                        //}
                        dt = objCon.FetchDataTable(uerquery);
                        if (dt.Rows.Count > 0)
                        {
                            GridQuery.DataSource = dt;
                            ViewState["SelectQryWizz"] = dt;
                            GridQuery.DataBind();
                            ////lblsearch.Visible = true;
                            ////txtsearch.Visible = true;
                            btnExport.Visible = true;
                            ////lblCount.Text =Convert.ToString( dt.Rows.Count )+" Rows Fetched...";
                        }
                        else
                        {
                             
                            Label1.Text = "No Record Found";
                            Label1.Visible = true;
                            ////txtsearch.Visible = false;
                            ////lblsearch.Visible = false;
                            btnExport.Visible = false;
                            //lblCount.Text =  "0 Rows Fetched...";
                        }
                    }
                }
                else
                {
                    
                    Label1.Text = "No Record Found";
                    Label1.Visible = true;
                    ////txtsearch.Visible = true;
                    ////lblsearch.Visible = true;
                    btnExport.Visible = false;
                }

            }
            catch (Exception ex)
            {

                DataTable dt = new DataTable("A");
                GridQuery.Columns.Clear();
                GridQuery.DataBind();
                Label1.Text = ex.Message;
                Label1.Visible = true;
                //  objQry.DatatoQueryLog(txtQuery.Text, objSession.UserId, "S", Label1.Text);
                string strQuery = "INSERT INTO \"TBLQUERYLOG\"(\"QL_ID\",\"QL_TEXT\",\"QL_ENTRYAUTH\",\"QL_EXCECUTED_IN_FORM\",\"QL_DESCRIPTION\") VALUES ";
                strQuery += " ('" + objCon.Get_max_no("QL_ID", "TBLQUERYLOG") + "','" + txtQuery.Text.Trim().Replace("'", "''") + "','" + objSession.UserId + "','S','" + Label1.Text + "')";
                objCon.ExecuteQry(strQuery);

            }
        }
        protected void cmdClear(object sender, EventArgs e)
        {
            Response.Redirect("/Query/QryWizzSelect.aspx");
        }

        protected void cmdExport_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                dtComplete = (DataTable)ViewState["SelectQryWizz"];
                ExportToExcel(dtComplete);
            }
            catch (Exception ex)
            {
                Label1.Text = clsException.ErrorMsg();
                Label1.Visible = true;
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        private void ExportToExcel(DataTable dtComplete)
        {
            try
            {
                if (dtComplete.Rows.Count > 0)
                {
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "SELECTQRYWizard.xls"));
                    Response.ContentType = "application/ms-excel";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    GridQuery.AllowPaging = false;
                    // LoadFaultyDTRDetails();
                    //Change the Header Row back to white color
                    GridQuery.HeaderRow.Style.Add("background-color", "#FFFFFF");
                    //Applying stlye to gridview header cells
                    for (int i = 0; i < GridQuery.HeaderRow.Cells.Count; i++)
                    {
                        GridQuery.HeaderRow.Cells[i].Style.Add("background-color", "#709eea");
                    }
                    GridQuery.RenderControl(htw);
                    Response.Write(sw.ToString());
                    HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
                    HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                    HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.

                    //Response.End();
                }
                else
                {
                    ShowMsgBox("No Records Found!");
                }
            }
            catch (Exception ex)
            {
                Label1.Text = clsException.ErrorMsg();
                Label1.Visible = true;
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
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
                Label1.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdComplete_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                GridQuery.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["SelectQryWizz"];
                GridQuery.DataSource = dtComplete;
                GridQuery.DataBind();
            }
            catch (Exception ex)
            {
                Label1.Text = clsException.ErrorMsg();
                Label1.Visible = true;
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void GridQuery_PreRender(object sender, EventArgs e)
        {
            if (GridQuery.Rows.Count > 0)
            {
                GridQuery.UseAccessibleHeader = true;
                GridQuery.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(TablesName, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"] = "cursor:pointer";
            }
        }



        protected void TablesName_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = TablesName.SelectedRow.RowIndex;
            string name = TablesName.SelectedRow.Cells[0].Text;
            GetAllColumnName(name);
            txtsearchtablecol.Visible = true;
            //string country = GridView1.SelectedRow.Cells[1].Text;
            //string message = "Row Index: " + index + "\\nName: " + name + "\\nCountry: " + country;
            //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + message + "');", true);
        }
        public void GetAllColumnName(string TableName)
        {
            DataTable dt = new DataTable();
            int iCount = 500;
            string sSql = string.Empty;
            sSql = "SELECT column_name FROM Information_schema.columns WHERE table_name = '" + TableName + "'";
            // dt = DBHelper.DBExecDataTable(sConString, sSql);
            dt = objCon.FetchDataTable(sSql);
            if (dt.Rows.Count > 0)
            {
                ColName.DataSource = dt;
                ColName.DataBind();

                foreach (GridViewRow gvRow in ColName.Rows)
                {
                    gvRow.Attributes.Add("draggable", "true");
                    gvRow.Attributes.Add("ondragstart", "drag(event)");
                    gvRow.Attributes.Add("id", iCount.ToString());
                    iCount++;
                }
                GetAllTableLists();
            }
        }
    }
}