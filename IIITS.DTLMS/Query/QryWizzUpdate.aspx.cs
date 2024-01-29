using IIITS.DTLMS.BL;
using IIITS.PGSQL.DAL;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.Query
{
    public partial class QryWizzUpdate : System.Web.UI.Page
    {
        string strFormCode = "QryWizzUpdate";
        clsSession objSession;
        PGSqlConnection objCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
        NpgsqlConnection NpgsqlCommand;

        protected void Page_Load(object sender, EventArgs e)

        {
           // GetAllTableLists();
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                string strQry = string.Empty;

                objSession = (clsSession)Session["clsSession"];

                if (!IsPostBack)
                {
                    //if ( objSession.UserType != "9")
                    //if (objSession.RoleId != "38")
                    //{
                    //    Response.Redirect("~/UserRestrict.aspx", false);
                    //}
                    //else
                    //{
                    //    GetAllTableLists();
                    //}
                    CheckAccessRights("4");
                    GetAllTableLists();
                }
            }
            catch (Exception ex)
            {
                Label1.Text = clsException.ErrorMsgPL(ex.Message);
                Label1.Visible = true;
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "UpdateQueryWizzard";
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
                //lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
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
               // lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                    return;
                }
                if (txtQuery.Text.Trim() == "")
                {
                    txtQuery.Focus();
                    ShowMsgBox("Please Enter the Query");
                    return;
                }
                if (txtTicketID.Text.Trim() == "")
                {
                    txtTicketID.Focus();
                    ShowMsgBox("Please Enter Ticket No");
                    return;
                }
                else
                {
                    if (!txtTicketID.Text.ToUpper().Contains("HDT") || !txtTicketID.Text.ToUpper().StartsWith("HDT"))
                    {
                        ShowMsgBox("Please Enter Valid Ticket No");
                        txtTicketID.Focus();
                        return;
                    }

                }
                if (txtRemarks.Text.Trim() == "")
                {
                    txtRemarks.Focus();
                    ShowMsgBox("Please Enter Remarks");
                    return;
                }
                LoadDetails();
                GridQuery.Style["display"] = "block";
            }
            catch (Exception ex)
            {
                Label1.Text = clsException.ErrorMsg();
                Label1.Visible = true;
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);


            }

        }
        private void LoadDetails()
        {
            clsQrywizard objQry = new clsQrywizard();
            try
            {
                string strQry = string.Empty;
                string status = string.Empty;

                string strUserId = objSession.UserId;
                strQry = txtQuery.Text.Trim();

                if (btnCommit.Visible == true)
                {
                    status = "COMMIT";
                }
                if (strQry.Length==0)
                {
                    Label1.Text = "Please Enter the Query";
                    Label1.Visible = true;
                    string strQuery = "INSERT INTO \"TBLQUERYLOG\"(\"QL_ID\",\"QL_TEXT\",\"QL_ENTRYAUTH\",\"QL_EXCECUTED_IN_FORM\",\"QL_DESCRIPTION\",\"QL_TICKET_NO\",\"QL_REMARKS\") VALUES ";
                    strQuery += " ('" + objCon.Get_max_no("QL_ID", "TBLQUERYLOG") + "','" + strQry.Replace("'", "''") + "','" + objSession.UserId + "','U','" + Label1.Text + "','"+txtTicketID.Text.ToUpper()+"','"+txtRemarks.Text+"')";
                    objCon.ExecuteQry(strQuery);
                    return;
                }
              
                DataTable dt = new DataTable();
                //if(!strQry.ToUpper().Contains("UPDATE")||!strQry.ToUpper().Contains("WHERE")||!strQry.ToUpper().Contains("SET"))
                //{
                //    Label1.Text = "Please Check the Query";
                //    Label1.Visible = true;
                //    return;
                //}

                if (strQry.Length <= 10)
                {
                    Label1.Text = "Please Enter the Correct Query";
                    Label1.Visible = true;
                    string strQuery = "INSERT INTO \"TBLQUERYLOG\"(\"QL_ID\",\"QL_TEXT\",\"QL_ENTRYAUTH\",\"QL_EXCECUTED_IN_FORM\",\"QL_DESCRIPTION\" ,\"QL_TICKET_NO\",\"QL_REMARKS\") VALUES ";
                    strQuery += " ('" + objCon.Get_max_no("QL_ID", "TBLQUERYLOG") + "','" + strQry.Replace("'", "''") + "','" + objSession.UserId + "','U','" + Label1.Text + "','" + txtTicketID.Text.ToUpper() + "','" + txtRemarks.Text + "')";
                    objCon.ExecuteQry(strQuery);
                    return;
                }
                if (strQry.ToUpper().Substring(0, 8).Contains("UPDATE ") || strQry.ToUpper().Substring(0, 10).Contains("DELETE "))
                {
                    if (!strQry.ToUpper().Contains("WHERE "))
                    {
                        Label1.Text = "Please Check the Query";
                        Label1.Visible = true;
                        /// objQry.DatatoQueryLog(strQry, strUserId, "U", Label1.Text,txtTicketID.Text.ToUpper(),txtRemarks.Text);
                        string strQuery = "INSERT INTO \"TBLQUERYLOG\"(\"QL_ID\",\"QL_TEXT\",\"QL_ENTRYAUTH\",\"QL_EXCECUTED_IN_FORM\",\"QL_DESCRIPTION\",\"QL_TICKET_NO\",\"QL_REMARKS\") VALUES ";
                        strQuery += " ('" + objCon.Get_max_no("QL_ID", "TBLQUERYLOG") + "','" + strQry.Replace("'", "''") + "','" + objSession.UserId + "','U','" + Label1.Text + "','" + txtTicketID.Text.ToUpper() + "','" + txtRemarks.Text + "')";
                        objCon.ExecuteQry(strQuery);
                        return;
                    }
                    //if (objSession.UserType != "9") 
                    //{
                    //    Response.Redirect("~/UserRestrict.aspx", false);
                    //    return;
                    //}
                    //btnCommit.Visible = true;
                    // btnRoleback.Visible = true;
                    btnConf.Visible = true;
                    btnLoad.Visible = false;
                    txtQuery.ReadOnly = true;
                    txtTicketID.ReadOnly = true;
                    txtRemarks.ReadOnly = true;
                }
                else if (strQry.ToUpper().Substring(0, 10).Contains("INSERT "))
                {
                    btnConf.Visible = true;
                    btnLoad.Visible = false;
                    txtQuery.ReadOnly = true;
                    txtTicketID.ReadOnly = true;
                    txtRemarks.ReadOnly = true;
                }
                else if (strQry.ToUpper().Substring(0, 10).Contains("SELECT "))
                {
                    Label1.Text = "Only Provision to do Insert, Update, Delete etc.";
                    Label1.Visible = true;
                    ///  objQry.DatatoQueryLog(strQry, strUserId, "U", Label1.Text);
                    string strQuery = "INSERT INTO \"TBLQUERYLOG\"(\"QL_ID\",\"QL_TEXT\",\"QL_ENTRYAUTH\",\"QL_EXCECUTED_IN_FORM\",\"QL_DESCRIPTION\",\"QL_TICKET_NO\",\"QL_REMARKS\") VALUES ";
                    strQuery += " ('" + objCon.Get_max_no("QL_ID", "TBLQUERYLOG") + "','" + strQry.Replace("'", "''") + "','" + objSession.UserId + "','U','" + Label1.Text + "','" + txtTicketID.Text.ToUpper() + "','" + txtRemarks.Text + "')";
                    objCon.ExecuteQry(strQuery);
                    return;
                }
                if (!strQry.ToUpper().Contains("UPDATE ") && !strQry.ToUpper().Contains("DELETE ") && !strQry.ToUpper().Contains("INSERT "))
                {
                    Label1.Text = "Check the SQL query ";
                    Label1.Visible = true;
                    ///  objQry.DatatoQueryLog(strQry, strUserId, "U", Label1.Text);
                    string strQuery = "INSERT INTO \"TBLQUERYLOG\"(\"QL_ID\",\"QL_TEXT\",\"QL_ENTRYAUTH\",\"QL_EXCECUTED_IN_FORM\",\"QL_DESCRIPTION\",\"QL_TICKET_NO\",\"QL_REMARKS\") VALUES ";
                    strQuery += " ('" + objCon.Get_max_no("QL_ID", "TBLQUERYLOG") + "','" + strQry.Replace("'", "''") + "','" + strUserId + "','U','" + Label1.Text + "','" + txtTicketID.Text.ToUpper() + "','" + txtRemarks.Text + "')";
                    objCon.ExecuteQry(strQuery);
                    return;
                }
                dt = objQry.getUpdateResult(strQry, strUserId, status,txtTicketID.Text.ToUpper(),txtRemarks.Text);
                if (dt.Rows.Count == 0)
                {

                    btnCommit.Visible = false;
                    btnLoad.Visible = true;
                    
                  //  dt.Rows.Add("no Rows Found");
                }

                GridQuery.DataSource = dt;
                GridQuery.DataBind();
                ViewState["Data"] = dt;

                if (dt.Columns.Contains("ERROR!"))
                {
                    btnConf.Visible = false;
                    btnLoad.Visible = true;
                    txtQuery.ReadOnly = false;
                    txtTicketID.ReadOnly = false;
                    txtRemarks.ReadOnly = false;
                }
                else if (dt.Columns.Contains("Information"))
                {
                    txtQuery.Text = "";
                    btnclear.Visible = true;
                    btnLoad.Visible = true;
                    btnCommit.Visible = false;
                    btnRollback.Visible = false;
                    btnConf.Visible = false;
                    txtTicketID.Text = string.Empty;
                    txtRemarks.Text = string.Empty;
                }

            }
            catch (Exception ex)
            {
                Label1.Text = clsException.ErrorMsg();
                Label1.Visible = true;
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                btnConf.Visible = false;
                btnLoad.Visible = true;
                txtQuery.ReadOnly = false;
                txtTicketID.ReadOnly = false;
                txtRemarks.ReadOnly = false;
                ///   objQry.DatatoQueryLog(txtQuery.Text.Trim(), objSession.UserId, "U", Label1.Text);
                string strQuery = "INSERT INTO \"TBLQUERYLOG\"(\"QL_ID\",\"QL_TEXT\",\"QL_ENTRYAUTH\",\"QL_EXCECUTED_IN_FORM\",\"QL_DESCRIPTION\",\"QL_TICKET_NO\",\"QL_REMARKS\") VALUES ";
                strQuery += " ('" + objCon.Get_max_no("QL_ID", "TBLQUERYLOG") + "','" + txtQuery.Text.Trim() + "','" + objSession.UserId + "','U','" + Label1.Text + "','" + txtTicketID.Text.ToUpper() + "','" + txtRemarks.Text + "')";
                objCon.ExecuteQry(strQuery);
            }

        }
     
        protected void cmdRollback(object sender, EventArgs e)
        {
            loadRollback();
            GridQuery.Style["display"] = "block";

        }
        protected void loadRollback()
        {
            try
            {
                String strQry = String.Empty;
                String status = String.Empty;

                if (btnRollback.Visible == true)
                {
                    status = "ROLLBACK";
                }
                clsQrywizard objQry = new clsQrywizard();
                DataTable dt = new DataTable();
                dt = objQry.getRollbackResult(status);

                GridQuery.DataSource = dt;
                GridQuery.DataBind();
                ViewState["Data"] = dt;

                //string strQuery = "INSERT INTO \"TBLQUERYLOG\"(\"QL_ID\",\"QL_TEXT\",\"QL_ENTRYAUTH\",\"QL_EXCECUTED_IN_FORM\",\"QL_DESCRIPTION\") VALUES ";
                //strQuery += " ('" + objCon.Get_max_no("QL_ID", "TBLQUERYLOG") + "','" + txtQuery.Text.Trim() + "','" + objSession.UserId + "','U','Rollback done Successfully!')";
                //objCon.ExecuteQry(strQuery);

                if (dt.Columns.Contains("ERROR!"))
                {

                }
                else if (dt.Columns.Contains("Information"))
                {
                  //  txtQuery.Text = "";
                    btnclear.Visible = true;
                    btnLoad.Visible = true;
                    btnCommit.Visible = false;
                    btnRollback.Visible = false;
                    btnConf.Visible = false;

                    string strQuery = "INSERT INTO \"TBLQUERYLOG\"(\"QL_ID\",\"QL_TEXT\",\"QL_ENTRYAUTH\",\"QL_EXCECUTED_IN_FORM\",\"QL_DESCRIPTION\",\"QL_TICKET_NO\",\"QL_REMARKS\") VALUES ";
                    strQuery += " ('" + objCon.Get_max_no("QL_ID", "TBLQUERYLOG") + "','" + txtQuery.Text.Trim().Replace("'", "''") + "','" + objSession.UserId + "','U','Rollback done Successfully!','" + txtTicketID.Text.ToUpper() + "','" + txtRemarks.Text + "')";
                    objCon.ExecuteQry(strQuery);
                    txtQuery.Text = "";
                    txtTicketID.Text = string.Empty;
                    txtRemarks.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Label1.Text = clsException.ErrorMsg();
                Label1.Visible = true;
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                string strQuery = "INSERT INTO \"TBLQUERYLOG\"(\"QL_ID\",\"QL_TEXT\",\"QL_ENTRYAUTH\",\"QL_EXCECUTED_IN_FORM\",\"QL_DESCRIPTION\",\"QL_TICKET_NO\",\"QL_REMARKS\") VALUES ";
                strQuery += " ('" + objCon.Get_max_no("QL_ID", "TBLQUERYLOG") + "','" + txtQuery.Text.Trim().Replace("'", "''") + "','" + objSession.UserId + "','U','Rollback done Successfully!','" + txtTicketID.Text.ToUpper() + "','" + txtRemarks.Text + "')";

                objCon.ExecuteQry(strQuery);
            }

        }
        protected void cmdClear(object sender, EventArgs e)
        {
            Response.Redirect("/Query/QryWizzUpdate.aspx");
        }
        protected void btnConf_Click(object sender, EventArgs e)
        {
            btnCommit.Visible = true;
            btnRollback.Visible = true;

        }




        public void GetAllTableLists()
        {

            try
            {
                int iCount = 0;
                string sSql = string.Empty;
                objCon.BeginTransaction();
                String ConStr = System.Configuration.ConfigurationManager.ConnectionStrings["pgSQL"].ConnectionString;
                NpgsqlConnectionStringBuilder Builder = new NpgsqlConnectionStringBuilder(ConStr);
                String Database = Builder.Database;
    
             
                sSql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_CATALOG = '"+ Database + "'";

                DataTable dt = new DataTable();
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
                else
                {
                    Label1.Text = "Sorry no record found";
                    Label1.Visible = true;
                }
                //DataTable dt = new DataTable();
                //int iCount = 0;
                //string sSql = string.Empty;

                //if (Convert.ToString(ConfigurationSettings.AppSettings["ConString"]).Contains("192.168.6.13"))
                //{
                //    sSql = "SELECT  table_name FROM all_tables WHERE owner = 'DTLMS10122019' ORDER BY table_name";
                //}
                //else
                //{
                //    sSql = "SELECT  table_name FROM all_tables WHERE owner = 'DTLMS_LIVE' ORDER BY table_name";

                //}
                //dt = objCon.getDataTable(sSql);
                //if (dt.Rows.Count > 0)
                //{
                //    TablesName.DataSource = dt;
                //    TablesName.DataBind();

                //    foreach (GridViewRow gvRow in TablesName.Rows)
                //    {
                //        gvRow.Attributes.Add("draggable", "true");
                //        gvRow.Attributes.Add("ondragstart", "drag(event)");
                //        gvRow.Attributes.Add("id", iCount.ToString());
                //        iCount++;
                //    }
                //}
            }
            catch (Exception ex)
            {
                Label1.Text = clsException.ErrorMsg();
                Label1.Visible = true;
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            finally
            {
                objCon.close();
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
            try
            {
                DataTable dt = new DataTable();
                int iCount = 500;
                string sSql = string.Empty;

                sSql = "SELECT column_name FROM Information_schema.columns WHERE table_name = '" + TableName + "'";

                // sSql = "SELECT COLUMN_NAME FROM [FMS].INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + TableName + "'";
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
            catch (Exception ex)
            {
                Label1.Text = clsException.ErrorMsg();
                Label1.Visible = true;
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        //modified by 'shridahr' end.

        private string GetNumberOfRows(string strQry)
        {
            try
            {
                strQry = strQry.ToUpper();
                if (strQry.Contains("UPDATE"))
                {
                    strQry = strQry.ToUpper().Replace("UPDATE", "SELECT count(*) FROM ");
                    strQry = strQry.ToUpper().Replace("SET", "WHERE");
                }
                else if (strQry.Contains("DELETE"))
                {
                    strQry.ToUpper().Replace("DELETE", "SELECT count(*)  FROM ");
                    strQry.ToUpper().Replace("SET", "WHERE");
                }
                return objCon.get_value(strQry);
            }

            catch (Exception ex)
            {
                throw ex;
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

    }
}