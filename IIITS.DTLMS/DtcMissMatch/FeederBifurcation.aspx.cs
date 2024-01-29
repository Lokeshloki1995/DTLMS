using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI.WebControls;
using System.Data;
using IIITS.DTLMS.BL;
using System.Collections;
using System.Configuration;


namespace IIITS.DTLMS.DtcMissMatch
{
    public partial class FeederBifurcation : System.Web.UI.Page
    {
        string strFormCode = "FeederBifurcation";
        clsSession objsession;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
            {
                Response.Redirect("~/Login.aspx", false);
            }
            txtOmDate.Attributes.Add("readonly", "readonly");
            CalendarExtender5.EndDate = DateTime.Now;

            //Genaral.Load_Combo("", );
            objsession = (clsSession)Session["clsSession"];
            if (!IsPostBack)
            {
                Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" ORDER BY \"CM_CIRCLE_CODE\"", "--Select--", cmbCircle);
                // LoadFeederDetails();
                CheckAccessRights("1", "1");
            }

        }

        #region Access Rights

        public bool CheckAccessRights(string sAccessType, string flag)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "DTCMissMatch";
                objApproval.sRoleId = objsession.RoleId;
                objApproval.sAccessType = "1" + "," + sAccessType;
                bool bResult = objApproval.CheckAccessRights(objApproval);
                if (flag == "2")
                {
                    //&& objSession.UserId != "39"
                    if (UserValid() == false)
                    {
                        if (bResult == true)
                        {
                            Response.Redirect("~/UserRestrict.aspx", false);
                            bResult = false;
                        }
                    }

                }
                else if (flag == "1")
                {
                    if (UserValid() == false)
                    {
                        if (bResult == false)
                        {
                            Response.Redirect("~/UserRestrict.aspx", false);
                        }
                    }
                }

                return bResult;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CheckAccessRights");
                return false;

            }
        }

        public bool UserValid()
        {
            bool res = true;
            try
            {
                string Userid = Convert.ToString(ConfigurationSettings.AppSettings["SELECTEDUSER"]);
                string[] sUserid = Userid.Split(',');
                for (int i = 0; i < sUserid.Length; i++)
                {
                    if (objsession.UserId != sUserid[i])
                    {
                        res = false;
                    }
                    else
                    {
                        res = true;
                        return res;
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CheckAccessRights");
                return false;
            }
        }



        #endregion

        private void LoadFeederDetails()
        {
            DataTable dt = new DataTable();
            try
            {
                clsFeederView objFeeder = new clsFeederView();
                dt = objFeeder.LoadFeederMastDet("");


                gridview2.DataSource = dt;
                gridview2.DataBind();



            }
            catch (Exception ex)
            {
                //  lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadFeederDetails");
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
                    cmbSubDivision.Items.Clear();
                    cmbFeeder.Items.Clear();

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbCircle_SelectedIndexChanged");
            }
        }


        protected void cmbdivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (cmbDivision.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_NAME\" from \"TBLSUBDIVMAST\" WHERE \"SD_DIV_CODE\"='" + cmbDivision.SelectedValue + "'", "--Select--", cmbSubDivision);

                }
                else
                {

                    cmbFeeder.Items.Clear();
                    cmbSubDivision.Items.Clear();

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbCircle_SelectedIndexChanged");
            }
        }



        protected void cmbSubdivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (cmbSubDivision.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"FD_FEEDER_CODE\" , \"FD_FEEDER_CODE\" || '-' || \"FD_FEEDER_NAME\"  FROM \"TBLFEEDERMAST\" ,  \"TBLFEEDEROFFCODE\" WHERE \"FD_FEEDER_ID\" = \"FDO_FEEDER_ID\" and CAST(\"FDO_OFFICE_CODE\" AS TEXT) LIKE '" + cmbSubDivision.SelectedValue + "%'", "--Select--", cmbFeeder);
                   

                }
                else
                {

                    cmbFeeder.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbCircle_SelectedIndexChanged");
            }
        }
        protected void gridview2_RowDataBound(object sender, EventArgs e)
        {

        }

        protected void cmbFeeder_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbFeeder.SelectedIndex > 0)
                {
                    LoadDTCDetails(cmbFeeder.SelectedValue);
                }
                if(gridview2.Rows.Count == 0)
                Genaral.Load_Combo("SELECT DISTINCT \"FD_FEEDER_CODE\" , \"FD_FEEDER_CODE\" || '-' || \"FD_FEEDER_NAME\"  FROM \"TBLFEEDERMAST\" ,  \"TBLFEEDEROFFCODE\" WHERE \"FD_FEEDER_ID\" = \"FDO_FEEDER_ID\" AND  \"FD_FEEDER_CODE\" <> '" + cmbFeeder.SelectedValue + "'  ", "--Select--", cmbNewFeeder);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbCircle_SelectedIndexChanged");
            }
        }

        protected void cmbNewFeeder_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (grdOldDTC.Rows.Count > 0)
                {
                    btnUpdate.Visible = true;

                }
                if (gridview2.Rows.Count > 0)
                {
                    gridview2.DataSource = new DataTable();
                    gridview2.DataBind();
                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbNewFeeder_SelectedIndexChanged");
            }
        }



        private void LoadDTCDetails(string p)
        {
            try
            {
                clsDtcMaster obj = new clsDtcMaster();
                obj.sFeederCode = p;
                DataTable dt = obj.GetDTCDetailsUsingFeederCode(obj);

                if (dt.Rows.Count > 0)
                {
                    grdOldDTC.DataSource = dt;
                    grdOldDTC.DataBind();

                    //gridview2.DataSource = dt;
                    //gridview2.DataBind();
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadDTCDetails");
            }
        }

        public string WorkFlowObjects()
        {
            try
            {
                string sClientIP = string.Empty;

                string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(ip))
                {
                    string[] ipRange = ip.Split(',');
                    int le = ipRange.Length - 1;
                    sClientIP = ipRange[0];
                }
                else
                {
                    sClientIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                return sClientIP;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "WorkFlowObjects");
                return null;
            }
        }

        protected void grdDtc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                    string strDtcId = ((Label)row.FindControl("lblDTCID")).Text;
                    StringBuilder sb = new StringBuilder();
                    if (ViewState["CheckedDTCCodes"] != null)
                    {
                        sb = (StringBuilder)ViewState["CheckedDTCCodes"];
                    }

                    foreach (GridViewRow grd in grdOldDTC.Rows)
                    {
                        CheckBox chk = (CheckBox)grd.FindControl("cbSelect");
                        if(strDtcId == ((Label)grd.FindControl("lblDTCID")).Text)
                        {
                            chk.Checked = false;
                        }

                        strDtcId = ((Label)row.FindControl("lblDTCID")).Text;
                        if (chk != null && chk.Checked)
                        {
                            sb.Append(grdOldDTC.DataKeys[grd.RowIndex].Value.ToString());
                            sb.Append(",");

                        }
                    }
                    if (sb.Length > 0)
                    {
                        sb.Remove(sb.Length - 1, 1);
                    }


                    string[] arr = sb.ToString().Split(',');
                    sb.Clear();
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (!(arr[i] == strDtcId))
                        {
                            sb.Append(arr[i]);
                            sb.Append(',');
                        }
                    }
                    if(sb.Length > 0)
                    {
                        sb.Remove(sb.Length - 1, 1);
                    }

                    clsDtcMaster objDTC = new clsDtcMaster();
                    objDTC.sFeederCode = cmbNewFeeder.SelectedValue;

                    DataTable dt = objDTC.GetDTCDetailsUsingIdFeederCode(objDTC, Convert.ToString(sb));
                    if (dt.Rows.Count > 0)
                    {
                        gridview2.DataSource = dt;
                        gridview2.DataBind();
                        btnbifurcate.Visible = true;
                    }
                    else
                    {
                        gridview2.DataSource = dt;
                        gridview2.DataBind();
                        ViewState["CheckedDTCCodes"] = null;
                        btnbifurcate.Visible = false;
                    }

                    DisableOldFeederCodeCheckBox();

                   
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdDtc_RowCommand");
            }

        }

        /// <summary>
        /// which calls after I click on DTC's To Be  Bifurcated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnUpdate_click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                String newFeederCode = string.Empty;
                StringBuilder sbfeederCodes = new StringBuilder();
                if (cmbNewFeeder.SelectedIndex == 0)
                {
                    ShowMsgBox("Please Select New Feeder Code");
                    return;
                }
                else
                {
                    newFeederCode = cmbNewFeeder.SelectedValue;
                }
                foreach (GridViewRow grd in grdOldDTC.Rows)
                {
                    CheckBox chk = (CheckBox)grd.FindControl("cbSelect");
                    if (chk != null && chk.Checked)
                    {
                        sb.Append(grdOldDTC.DataKeys[grd.RowIndex].Value.ToString());
                        sb.Append(",");

                    }
                }
                if (sb.Length == 0)
                {
                    ShowMsgBox("Please Select atleast one DTC Code");
                    return;
                }
                sb.Remove(sb.Length - 1, 1);

                //if (ViewState["CheckedDTCCodes"] == null)
                //{
                //    ViewState["CheckedDTCCodes"] = sb;
                //}
                //else
                //{
                //    StringBuilder tempsb = (StringBuilder)ViewState["CheckedDTCCodes"];
                //    sb.Append(",");
                //    sb.Append(tempsb);
                //    ViewState["CheckedDTCCodes"] = sb;
                //}
              

                if (chkIsMultipleFeeder.Checked == true)
                {


                    // for storing multiple dtcID's for multiple FeederCode 
                    #region VIEWSTATEDTCCODE
                        if (ViewState["CheckedDTCCodes"] == null)
                        {
                            ViewState["CheckedDTCCodes"] = sb;
                        }
                        else
                        {
                            StringBuilder tempsb = (StringBuilder)ViewState["CheckedDTCCodes"];
                            sb.Append(",");
                            sb.Append(tempsb);
                            ViewState["CheckedDTCCodes"] = sb;
                        }
                    #endregion

                    /// for storing FeederCodes

                    #region
                    if (ViewState["SelectedFeederCodes"] == null)
                    {
                        ViewState["SelectedFeederCodes"] = sbfeederCodes;
                    }
                    else
                    {
                        StringBuilder tempsbfeederCodes = (StringBuilder)ViewState["SelectedFeederCodes"];
                        tempsbfeederCodes.Append(",");
                        sbfeederCodes.Append("'" + tempsbfeederCodes + "'");
                        ViewState["SelectedFeederCodes"] = sbfeederCodes;
                    }

                    #endregion
                }

                clsDtcMaster objDTC = new clsDtcMaster();
                objDTC.sFeederCode = newFeederCode;

                /// Selected DTC's will be added to the Second GRID


                DataTable dt = objDTC.GetDTCDetailsUsingIdFeederCode(objDTC, Convert.ToString(sb));
                // DataTable dt1 = objDTC.GetDTCDetailsUsingIdFeederCode(objDTC);
                //   ViewState["AddedDTCCode"] = dt;
                if (dt.Rows.Count > 0)
                {
                    gridview2.DataSource = dt;
                    gridview2.DataBind();
                }

                DisableOldFeederCodeCheckBox();

                btnbifurcate.Visible = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "btnUpdate_click");
            }

        }

        public void DisableOldFeederCodeCheckBox()
        {
            try
            {
                foreach (GridViewRow grd in gridview2.Rows)
                {
                    Label lblOldFeed = (Label)(grd.FindControl("lblOldFeederCode"));
                    Label lblNEw = (Label)(grd.FindControl("lblNewFeederCode"));

                  
                    if (lblOldFeed.Text == lblNEw.Text)
                    {
                        CheckBox chk = (CheckBox)grd.FindControl("cbSelect");
                        chk.Enabled = false;
                        ImageButton img = (ImageButton)grd.FindControl("imgBtnEdit");
                        img.Enabled = false;
                        TextBox txtSerail = (TextBox)grd.FindControl("lblDTCseraial");
                        txtSerail.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// it will bifurcate the DTC's
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnbifurcate_click(object sender, EventArgs e)
        {
            try
            {

                ArrayList al = new ArrayList();

                StringBuilder oldDTCID = new StringBuilder();
                StringBuilder newDTCCodes = new StringBuilder();
                StringBuilder failedDTCCodes ;
                HashSet<string> hsselectedFeederCode = new HashSet<string>();
                string final;

                if(txtOmDate.Text == null || txtOmDate.Text == "")
                {
                    ShowMsgBox("Please Enter the OmDate");
                    return;
                }

                if (txtOmDate.Text != "" )
                {
                    string sResult = Genaral.DateValidation(txtOmDate.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtOmDate.Focus();
                        return;
                    }
                }

                ///from second grid add the values to final variable 
                foreach (GridViewRow grd in gridview2.Rows)
                {
                    CheckBox chk = (CheckBox)grd.FindControl("cbSelect");
                    if (chk != null && chk.Checked)
                    {
                        Label lblDTCID = (Label)(grd.FindControl("lblDTCID"));
                        Label lblDTCCode = (Label)(grd.FindControl("lblDTCCode"));
                        Label lblTCCode = (Label)(grd.FindControl("lblTCCode"));
                        Label lblOldFeederCode = (Label)(grd.FindControl("lblOldFeederCode"));
                        Label lblNewFeederCode = (Label)(grd.FindControl("lblNewFeederCode"));
                        TextBox lblDTCseraial = (TextBox)(grd.FindControl("lblDTCseraial"));

                        if (lblDTCseraial.Text.Length == 0 || lblDTCseraial.Text.Length == 1)
                        {
                            ShowMsgBox(lblDTCCode.Text + " DTC Code Serial Number is Empty");
                            return;
                        }

                        final = lblDTCID.Text + "~" + lblDTCCode.Text + "~"+ lblTCCode.Text + "~" + lblOldFeederCode.Text + "~" + lblNewFeederCode.Text + "~" + lblNewFeederCode.Text + lblDTCseraial.Text;
                        al.Add(final);
                        hsselectedFeederCode.Add(lblOldFeederCode.Text);

                        oldDTCID.Append(lblDTCID.Text);
                        oldDTCID.Append(",");
                        newDTCCodes.Append("'" + lblNewFeederCode.Text + lblDTCseraial.Text + "'");
                        newDTCCodes.Append(",");
                    }
                }
                int ar = al.Count;

                if(ar == 0)
                {
                    ShowMsgBox("No DTC Code Selected");
                    return;
                }

                if (oldDTCID.Length != 0)
                {
                    oldDTCID.Remove(oldDTCID.Length - 1, 1);
                }
                if (newDTCCodes.Length != 0)
                {
                    newDTCCodes.Remove(newDTCCodes.Length - 1, 1);
                }
                string clientIP = WorkFlowObjects();
                clsDtcMaster objDTC = new clsDtcMaster();
                objDTC.sCrBy = objsession.UserId;
                if (txtOmDate.Text != null && txtOmDate.Text != "")
                {
                    objDTC.sDate = txtOmDate.Text;
                }
               

                var status = objDTC.UpdateFeederBifurcation(al, oldDTCID, newDTCCodes, hsselectedFeederCode, objDTC, clientIP, "ADMIN");

                string[] arrStatus = status.Item1;
                List<string> failedDTC = status.Item2;

                if (failedDTC.Count > 0)
                {
                    string[] arrfailedDTC = failedDTC.ToArray();
                    failedDTCCodes = new StringBuilder();
                    for(int i =0;i<arrfailedDTC.Length;i++)
                    {
                        failedDTCCodes.Append(failedDTCCodes[i]);
                        if(i!=arrfailedDTC.Length-1)
                        {
                            failedDTCCodes.Append(",");
                        }
                    }
                    ShowMsgBox(failedDTC.ToArray() + "DTC has been Failed or DTC has been duplicated");
                    
                }

                if (arrStatus[1] == "-1")
                {
                    ShowMsgBox(arrStatus[0]);
                }
                else if (arrStatus[1] == "0")
                {
                    ShowMsgBox(arrStatus[0]);
                    if (arrStatus[2].Length > 0)
                    {
                        string strParam = arrStatus[2];
                        strParam = "id=FeederBifurcation&BifurcationID=" + strParam + "&officeCode=" + string.Empty + "&oldFeederCode=" + string.Empty + "&newFeederCode=" + string.Empty + "&ReportType=" + string.Empty + "&FromDate=" + string.Empty + "&ToDate=" +string.Empty+ " ";
                        RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

                    }
                }
                //  success 
                else if (arrStatus[1] == "1")
                {
                    ShowMsgBox(arrStatus[0]);
                    string strParam = arrStatus[2];
                    strParam = "id=FeederBifurcation&BifurcationID=" + strParam+ "&officeCode="+string.Empty+"&oldFeederCode="+string.Empty+"&newFeederCode="+string.Empty+ "&ReportType="+string.Empty+ "&FromDate=" + string.Empty + "&ToDate=" + string.Empty + "  ";
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                    
                }

                btnReset_click(sender, e);



            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "btnbifurcate_click");

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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowMsgBox");
            }
        }

        public void btnReset_click(object sender, EventArgs e)
        {
            if (grdOldDTC.Rows.Count > 0)
            {
                grdOldDTC.DataSource = new DataTable();
                grdOldDTC.DataBind();
            }
            if (gridview2.Rows.Count > 0)
            {
                gridview2.DataSource = new DataTable();
                gridview2.DataBind();
            }


            ViewState["CheckedDTCCodes"] = null;
            ViewState["SelectedFeederCodes"] = null;

            cmbCircle.SelectedIndex = 0;

            cmbDivision.Items.Clear();
            cmbSubDivision.Items.Clear();
            cmbFeeder.Items.Clear();
            cmbNewFeeder.Items.Clear();

            btnbifurcate.Visible = false;
            btnUpdate.Visible = false;
            txtOmDate.Text = "";
        }
    }
}