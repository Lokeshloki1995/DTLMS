using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Drawing;
using System.Configuration;

namespace IIITS.DTLMS.MasterForms
{
    public partial class FeederView : System.Web.UI.Page
    {
        string strFormCode = "FeederView";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }   
                objSession = (clsSession)Session["clsSession"];
                lblErrormsg.Text = string.Empty;
                if (!IsPostBack)
                {
                    CheckAccessRights("4");
                    LoadFeederGrid("", "","");
                }
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadFeederGrid(string sOfficeCode,string strFeederName="",string strFeederCode="",string sStationName="")
        {
            try
            {
                clsFeederView ObjFeeder = new clsFeederView();
               
                DataTable dt = new DataTable();
                if (sOfficeCode == "")
                {
                    sOfficeCode=objSession.OfficeCode;
                }
                dt = ObjFeeder.LoadFeederMastDet(sOfficeCode,strFeederName, strFeederCode,sStationName);

                if (dt.Rows.Count <= 0)
                {
                    DataTable dtFeederDetails = new DataTable();
                    DataRow newRow = dtFeederDetails.NewRow();
                    dtFeederDetails.Rows.Add(newRow);
                    dtFeederDetails.Columns.Add("FD_FEEDER_ID");
                    dtFeederDetails.Columns.Add("FD_FEEDER_NAME");
                    dtFeederDetails.Columns.Add("FD_FEEDER_CODE");
                    dtFeederDetails.Columns.Add("OFF_NAME");
                    dtFeederDetails.Columns.Add("ST_NAME");
                    dtFeederDetails.Columns.Add("FD_TYPE");

                    grdFeeder.DataSource = dtFeederDetails;
                    grdFeeder.DataBind();

                    int iColCount = grdFeeder.Rows[0].Cells.Count;
                    grdFeeder.Rows[0].Cells.Clear();
                    grdFeeder.Rows[0].Cells.Add(new TableCell());
                    grdFeeder.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdFeeder.Rows[0].Cells[0].Text = "No Records Found";
                    

                }

                else
                {

                    grdFeeder.DataSource = dt;
                    grdFeeder.DataBind();
                    ViewState["Feeder"] = dt;
                }

              
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void grdFeeder_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdFeeder.PageIndex = e.NewPageIndex;
                // LoadFeederGrid(strSearchFeederName, strSearchFeederCode);
             
                DataTable dt = (DataTable)ViewState["Feeder"];
                dt.Columns["FD_FEEDER_NAME"].AllowDBNull = true;
                dt.Columns["FD_FEEDER_CODE"].AllowDBNull = true;
                dt.Columns["OFF_NAME"].AllowDBNull = true;
                grdFeeder.DataSource = dt;
                grdFeeder.DataBind();
              
               
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdFeeder_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                    TextBox txtFeederName = (TextBox)row.FindControl("txtFeederName");
                    TextBox txtFeederCode = (TextBox)row.FindControl("txtFeederCode");
                    TextBox txtStation = (TextBox)row.FindControl("txtStation");

                    LoadFeederGrid("",txtFeederName.Text.Trim(), txtFeederCode.Text.Trim(),txtStation.Text.Trim());
                }

                if (e.CommandName == "create")
                {

                    //Check AccessRights
                    bool bAccResult = CheckAccessRights("3");
                    if (bAccResult == false)
                    {
                        return;
                    }

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                    string sFeederId = ((Label)row.FindControl("lblFeederId")).Text;
                    sFeederId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sFeederId));
                    Response.Redirect("FeederMastApp.aspx?FeederId=" + sFeederId + "", false);


                }
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
              string sOfficeCode=  ReportFilterControl1.GetOfficeID();
                if(sOfficeCode.Length >= 4)
                {
                    clsFeederMast objFeederCount = new clsFeederMast();
                    lblFeederCount.Text = " Total Feeders : " + objFeederCount.sGetFeederCount(sOfficeCode.Substring(0,4)) ;                    
                }
              LoadFeederGrid(sOfficeCode);
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdNew_Click(object sender, EventArgs e)
        {
            try
            {
               
                //Check AccessRights
                bool bAccResult = CheckAccessRights("2");
                if (bAccResult == false)
                {
                    return;
                }
                Response.Redirect("FeederMast.aspx", false);

            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
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
                lblErrormsg.Text = clsException.ErrorMsg();
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

                objApproval.sFormName = "FeederMast";
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

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                this.ReportFilterControl1.Reset();
                LoadFeederGrid("", "", "");
            }
            catch(Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void Export_view(object sender, EventArgs e)
        {

            
            DataTable dt = (DataTable)ViewState["Feeder"];

            if (dt.Rows.Count > 0)
            {

               

                dt.Columns["FD_FEEDER_ID"].ColumnName = "Id";
                dt.Columns["FD_FEEDER_NAME"].ColumnName = "FEEDER_NAME";
                dt.Columns["FD_FEEDER_CODE"].ColumnName = "FEEDER_CODE";
                dt.Columns["OFF_NAME"].ColumnName = "OFF_NAME";
                dt.Columns["ST_NAME"].ColumnName = "STATION_NAME";
                dt.Columns["FD_TYPE"].ColumnName = "FEEDER_TYPE";

                dt.Columns["Id"].SetOrdinal(0);
                List<string> listtoRemove = new List<string> { "" };
                string filename = "FeederDetails" + DateTime.Now + ".xls";
                string pagetitle = "FeederDetails";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);


            }
            else
            {
                ShowMsgBox("No record found");

                DataTable dtDetails = new DataTable();
                DataRow newRow = dtDetails.NewRow();
                dtDetails.Rows.Add(newRow);
                dtDetails.Columns.Add("FD_FEEDER_ID");
                dtDetails.Columns.Add("FD_FEEDER_NAME");
                dtDetails.Columns.Add("FD_FEEDER_CODE");
                dtDetails.Columns.Add("OFF_NAME");
                dtDetails.Columns.Add("ST_NAME");
                dtDetails.Columns.Add("FD_TYPE");


                

                grdFeeder.DataSource = dtDetails;
                grdFeeder.DataBind();

                int iColCount = grdFeeder.Rows[0].Cells.Count;
                grdFeeder.Rows[0].Cells.Clear();
                grdFeeder.Rows[0].Cells.Add(new TableCell());
                grdFeeder.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdFeeder.Rows[0].Cells[0].Text = "No Records Found";

            }
            



        }
    }
}