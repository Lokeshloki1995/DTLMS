using IIITS.DTLMS.BL;
using IIITS.DTLMS.BL.MasterForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.MasterForms
{
    public partial class FeederMapping : System.Web.UI.Page
    {
        string strFormCode = "FeederMapping";
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

                 CheckAccessRights("4");
                if (!IsPostBack)
                {
                    Genaral.Load_Combo("SELECT \"DT_CODE\",\"DT_NAME\" from \"TBLDIST\" ORDER BY \"DT_CODE\" ", "--Select--", cmbdistrict);
                    LoadFeederGrid(objSession.OfficeCode);
                }

            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbTaluk_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbTaluk.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"ST_ID\",\"ST_NAME\" FROM \"TBLSTATION\" WHERE CAST(\"ST_TQ_CODE\" AS TEXT) like  '%" + cmbTaluk.SelectedValue + "%' ORDER BY \"ST_NAME\"", "--Select--", cmbStation);
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

        protected void cmbStation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbStation.SelectedIndex > 0)
                {
                    // Genaral.Load_Combo("SELECT \"FD_FEEDER_CODE\",\"FD_FEEDER_NAME\" FROM  \"TBLFEEDERMAST\"   WHERE \"FD_ST_ID\"='" + cmbStation.SelectedValue + "'", "--Select--", lblMultiSelect);
                    clsFeederMapping objfeeder = new clsFeederMapping();

                    lblMultiSelect.DataSource = objfeeder.LoadFeederDet(cmbStation.SelectedValue);
                    lblMultiSelect.DataTextField = "FD_FEEDER_CODE";
                    lblMultiSelect.DataValueField = "FD_FEEDER_ID";
                    lblMultiSelect.DataBind();
                    //foreach (ListItem listItem in lblMultiSelect.Items)
                    //{
                    //    if (listItem.Selected)
                    //    {
                    //        var val = listItem.Value;
                    //        var txt = listItem.Text;
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmbDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbdistrict.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"TQ_CODE\",\"TQ_NAME\" FROM \"TBLTALQ\" WHERE CAST(\"TQ_DT_ID\" AS TEXT)='" + cmbdistrict.SelectedValue + "'", "--Select--", cmbTaluk);
                }
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                string[] Arrmsg = new string[2];
                clsFeederMapping objfeeder = new clsFeederMapping();
                if (lblMultiSelect.SelectedIndex < 0)
                {
                    ShowMsgBox("Please Select Feeder code for Add Grid");
                    return;
                }
                if ((DataTable)ViewState["Feeder"] != null)
                {
                    objfeeder.ddtCapacityGrid = (DataTable)ViewState["Feeder"];

                    Arrmsg = objfeeder.SaveFeederOffCode(objfeeder);
                    ShowMsgBox(Arrmsg[1]);
                    return;

                }
                else
                {
                    ShowMsgBox("Please Select Atleast Any one Feeder Code ");
                    return;
                }

            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdAdd_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            try
            {
                if (ViewState["Feeder"] == null)
                {
                    dt.Columns.Add("FD_FEEDER_ID");
                    dt.Columns.Add("ST_NAME");
                    dt.Columns.Add("FD_FEEDER_NAME");
                    dt.Columns.Add("FD_FEEDER_CODE");
                    dt.Columns.Add("OFF_NAME");
                    dt.Columns.Add("OFF_CODE");
                    dt.Columns.Add("status");
                }
                else
                {
                    //load datatble from viewstate
                    dt = (DataTable)ViewState["Feeder"];
                }
                if(lblMultiSelect.SelectedIndex >= 0)
                 {
                    foreach (ListItem listItem in lblMultiSelect.Items)
                    {
                        if (listItem.Selected)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (listItem.Value == Convert.ToString(dt.Rows[i]["FD_FEEDER_ID"]))
                                {
                                    ShowMsgBox(listItem.Text + "  Feeder Already Selected");
                                    return;
                                }
                            }

                            DataRow dRow = dt.NewRow();
                            dRow["FD_FEEDER_ID"] = listItem.Value;
                            dRow["ST_NAME"] = cmbStation.SelectedItem.Text;
                            dRow["FD_FEEDER_NAME"] = listItem.Text.Split('~').GetValue(1);
                            dRow["FD_FEEDER_CODE"] = listItem.Text.Split('~').GetValue(0);
                            dRow["OFF_NAME"] = objSession.OfficeName;
                            if (objSession.OfficeCode!="")
                            {
                                dRow["OFF_CODE"] = objSession.OfficeCode;
                            }
                            else
                            {
                                dRow["OFF_CODE"] = 0;
                            }
                            dRow["status"] = "0";
                            if (ViewState["gridRowId"] != null)
                            {
                                // gridid.Text = ViewState["gridRowId"].ToString();
                                int i = Convert.ToInt32(ViewState["gridRowId"]);
                                dt.Rows[i].SetField("FD_FEEDER_ID", listItem.Value);
                                dt.Rows[i].SetField("ST_NAME", cmbStation.SelectedItem.Text);
                                dt.Rows[i].SetField("FD_FEEDER_NAME", listItem.Text.Split('~').GetValue(1));
                                dt.Rows[i].SetField("FD_FEEDER_CODE", listItem.Text.Split('~').GetValue(0));
                                dt.Rows[i].SetField("OFF_NAME", objSession.OfficeName);
                                dt.Rows[i].SetField("OFF_CODE", objSession.OfficeCode);
                                dt.Rows[i].SetField("status", "0");

                                ViewState["gridRowId"] = null;
                                ViewState["Feeder"] = dt;
                                grdFeeder.DataSource = dt;
                                grdFeeder.DataBind();
                                grdFeeder.Visible = true;
                            }
                            else
                            {
                                dt.Rows.Add(dRow);
                                ViewState["Feeder"] = dt;
                                grdFeeder.DataSource = dt;
                                grdFeeder.DataBind();
                                grdFeeder.Visible = true;
                            }

                        }
                    }
                }
                else
                {
                    ShowMsgBox("Please Select feeder code for Add Grid");
                    return;
                }

            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                cmbdistrict.ClearSelection();
                cmbTaluk.ClearSelection();
                cmbStation.ClearSelection();
                lblMultiSelect.Items.Clear();
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void grdFeeder_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton lnkdelete = (LinkButton)e.Row.FindControl("imgdelete");
                    //  LinkButton lnkReject = (LinkButton)e.Row.FindControl("lnkReject");

                    string status = ((Label)e.Row.FindControl("lblstatus")).Text;
                    if (status == "1" )
                    {
                        lnkdelete.Visible = false;
                    }

                }

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

                    LoadFeederGrid("", txtFeederName.Text.Trim(), txtFeederCode.Text.Trim(), txtStation.Text.Trim());
                }
                if (e.CommandName == "remove")
                {
                    DataTable dt = (DataTable)ViewState["Feeder"];

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        Label lblfeederid = (Label)row.FindControl("lblFeederId");


                        //to remove selected Capacity from grid
                        if (lblfeederid.Text == Convert.ToString(dt.Rows[i]["FD_FEEDER_ID"]))
                        {
                            clsFeederMapping objfeeder = new clsFeederMapping();

                            int n = objfeeder.DeleteFeederOffCode(Convert.ToString(dt.Rows[i]["FD_FEEDER_ID"]), Convert.ToString(dt.Rows[i]["OFF_CODE"]));
                            if (n == 1)
                            {
                                dt.Rows[i].Delete();
                                dt.AcceptChanges();
                            }                           
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["Feeder"] = dt;
                    }
                    else
                    {
                        ViewState["Feeder"] = null;
                    }

                    grdFeeder.DataSource = dt;
                    grdFeeder.DataBind();
                }

            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        public void LoadFeederGrid(string sOfficeCode, string strFeederName = "", string strFeederCode = "", string sStationName = "")
        {
            try
            {
                clsFeederMapping ObjFeeder = new clsFeederMapping();

                DataTable dt = new DataTable();
                if (sOfficeCode == "")
                {
                    sOfficeCode = objSession.OfficeCode;
                }
                dt = ObjFeeder.LoadFeederMastDet(sOfficeCode, strFeederName, strFeederCode, sStationName);

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
                    dtFeederDetails.Columns.Add("OFF_CODE");
                    dtFeederDetails.Columns.Add("status");

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
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "FeederMapping";
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

    }

}