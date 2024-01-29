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
using System.IO;
using ClosedXML.Excel;
using OfficeOpenXml;

namespace IIITS.DTLMS.POFlow
{
    public partial class DWAView : System.Web.UI.Page
    {
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
                if (CheckAccessRights(Constants.CheckAccessRights.CheckAccessRightsAll))
                {
                    if (!IsPostBack)
                    {
                        string Status = "";
                     LoadDWADetails(Status);
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
        protected void grdDWA_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblStatus;
                    lblStatus = (Label)e.Row.FindControl("lblStatus");
                    ImageButton imgBtnEdit;
                    ImageButton imgDeActive;
                    ImageButton imgActive;
                    imgBtnEdit = (ImageButton)e.Row.FindControl("imgBtnEdit");
                    if (lblStatus.Text == "Active")
                    {
                        imgActive = (ImageButton)e.Row.FindControl("imgActive");
                        imgDeActive = (ImageButton)e.Row.FindControl("imgDeActive");
                        imgActive.Visible = true;
                        imgDeActive.Visible = false;
                        imgBtnEdit.Enabled = true;
                        imgBtnEdit.ToolTip = "";
                    }
                    else
                    {
                        imgDeActive = (ImageButton)e.Row.FindControl("imgDeActive");
                        imgActive = (ImageButton)e.Row.FindControl("imgActive");
                        imgDeActive.Visible = true;
                        imgActive.Visible = false;
                        imgBtnEdit.Enabled = true;
                        imgBtnEdit.ToolTip = "";
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
        protected void grdDWA_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "edit")
            {

                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                string DWAId = ((Label)row.FindControl("lblDmId")).Text;
                DWAId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(DWAId));
                Response.Redirect("DWACreate.aspx?QryDWAId=" + DWAId + "", true);


            }
            if (e.CommandName == "status")
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                string sId = ((Label)row.FindControl("lblDmId")).Text;
                string sStatus = ((Label)row.FindControl("lblStatus")).Text;

                ImageButton imgDeactive = new ImageButton();
                ImageButton imgActive = new ImageButton();
                imgDeactive = (ImageButton)row.FindControl("imgDeactive");
                imgActive = (ImageButton)row.FindControl("imgActive");

                ClsDwa Obj = new ClsDwa();
                Obj.Id = sId;
                Obj.Status = sStatus;
                Obj.UpBy = objSession.UserId;

                string[] Arr = new string[2];
                Arr = Obj.ChecldwaRecord(Obj);

                if (Arr[0] == "0")
                {
                    ShowMsgBox(Arr[1]);
                }
                else
                {
                    ShowMsgBox(Arr[1]);
                }

                string Status = "";
                LoadDWADetails(Status);
                cmdInActiveUser.BackColor = System.Drawing.Color.Empty;
                cmdActiveUser.BackColor = System.Drawing.Color.Empty;
                
            }
          
        }


        /// <summary>
        /// CheckAccessRights
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

        public void LoadDWADetails(string Status)
        {
            ClsDwa Obj = new ClsDwa();
            try
            {
                Obj.CheckDWAStatus();
                Obj.Status = Status;
                Obj = Obj.GetDWAGridDetails(Obj);
               
                if (Obj.DtDWADetails.Rows.Count > 0)
                {
                    grdDWA.DataSource = Obj.DtDWADetails;
                    grdDWA.DataBind();

                }
                else
                {
                    ShowEmptyGrid();
                }
                ViewState["DWA"] = Obj.DtDWADetails;
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





        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
               
                dt.Columns.Add("DM_ID");
                dt.Columns.Add("DM_LM_ID");
                dt.Columns.Add("LM_CONTRACTOR_NAME");
                dt.Columns.Add("DM_NUMBER");
                dt.Columns.Add("DM_DATE");
                dt.Columns.Add("DM_EXTENDED_UPTO");
                dt.Columns.Add("MD_NAME");
                dt.Columns.Add("DM_AMOUNT");
                dt.Columns.Add("LM_ADDRESS");
                dt.Columns.Add("LM_CONTACT_NUMBER");
                dt.Columns.Add("DIM_STATUS");
                dt.Columns.Add("DM_PROJECTNAME");


                grdDWA.DataSource = dt;
                grdDWA.DataBind();

                int iColCount = grdDWA.Rows[0].Cells.Count;
                grdDWA.Rows[0].Cells.Clear();
                grdDWA.Rows[0].Cells.Add(new TableCell());
                grdDWA.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdDWA.Rows[0].Cells[0].Text = Convert.ToString(ConfigurationManager.AppSettings["EmptyData"]);
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
        protected void cmdNew_Click(object sender, EventArgs e)
        {
            try
            {
                
                bool bAccResult = CheckAccessRights(Constants.CheckAccessRights.CheckAccessRightsCreate);
                if (bAccResult == false)
                {
                    return;
                }
                Response.Redirect("DWACreate.aspx", false);
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
        protected void Export_DWAClick(object sender, EventArgs e)
        {

            DataTable dtDWADetails = (DataTable)ViewState["DWA"];
            try
            {
                if (dtDWADetails.Rows.Count > 0)
                {
                    #region adding Slno
                    int Slno = 0;
                    dtDWADetails.Columns.Add("SlNo", typeof(int));
                    // Iterate through the rows in the DataTable
                    foreach (DataRow row in dtDWADetails.Rows)
                    {
                        #region adding Slno.
                        Slno = Slno + 1;
                        row["SlNo"] = Slno;
                        #endregion
                    }
                    #endregion

                    dtDWADetails.Columns["LM_CONTRACTOR_NAME"].ColumnName = "Contractor Name";
                    dtDWADetails.Columns["DM_NUMBER"].ColumnName = "DWA No";
                    dtDWADetails.Columns["DM_DATE"].ColumnName = "Award Date";
                    dtDWADetails.Columns["DM_EXTENDED_UPTO"].ColumnName = "Valid Upto";
                    dtDWADetails.Columns["MD_NAME"].ColumnName = "Project";
                    dtDWADetails.Columns["DM_PROJECTNAME"].ColumnName = "Project Name";

                    dtDWADetails.Columns["DM_AMOUNT"].ColumnName = "Award Amount";
                    dtDWADetails.Columns["LM_ADDRESS"].ColumnName = "Address";
                    dtDWADetails.Columns["LM_CONTACT_NUMBER"].ColumnName = "Mobile No";
                    dtDWADetails.Columns["DIM_STATUS"].ColumnName = "Status";

                    dtDWADetails.Columns["SlNo"].SetOrdinal(0);
                    dtDWADetails.Columns["Contractor Name"].SetOrdinal(1);
                    dtDWADetails.Columns["DWA No"].SetOrdinal(2);
                    dtDWADetails.Columns["Award Date"].SetOrdinal(3);
                    dtDWADetails.Columns["Valid Upto"].SetOrdinal(4);
                    dtDWADetails.Columns["Project"].SetOrdinal(5);
                    dtDWADetails.Columns["Project Name"].SetOrdinal(6);
                    dtDWADetails.Columns["Award Amount"].SetOrdinal(7);
                    dtDWADetails.Columns["Address"].SetOrdinal(8);
                    dtDWADetails.Columns["Mobile No"].SetOrdinal(9);
                    dtDWADetails.Columns["Status"].SetOrdinal(10);


                    // dtDWADetails.Columns["LM_CONTRACTOR_NAME"].SetOrdinal(5);
                    List<string> listtoRemove = new List<string> { "DM_ID", "DM_LM_ID", "DM_STATUS", "DM_PRJTYP" };

                    string filename = "DWADetails" + DateTime.Now + ".xls";
                    string pagetitle = "DWA Details";
                    Genaral.getexcel(dtDWADetails, listtoRemove, filename, pagetitle);

                }
                else
                {
                    ShowEmptyGrid();
                    ShowMsgBox("No Records Found");
                }
            }
            catch (Exception ex)
            {
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
                clsException.LogError(
                    MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name,
                    ex.Message,
                    ex.StackTrace);
            }
        }


        protected void CreateNewDWA_click(object sender, EventArgs e)
        {
            try
            {

                Response.Redirect("DWACreate.aspx", false);
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
        protected void cmdActiveUser_click(object sender, EventArgs e)
        {

            ClsDwa objDwa = new ClsDwa();
           
            try
            {
                cmdInActiveUser.BackColor = System.Drawing.Color.Empty;
                objDwa.Status = "A";
                
                objDwa = objDwa.GetDWAGridDetails(objDwa);
                
                if (objDwa.DtDWADetails.Rows.Count > 0)
                {
                    grdDWA.DataSource = objDwa.DtDWADetails;
                    grdDWA.DataBind();
                }
                else
                {
                    ShowEmptyGrid();
                }
                ViewState["DWA"] = objDwa.DtDWADetails;
                cmdActiveUser.BackColor = System.Drawing.Color.Green;

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
        protected void cmdInActiveUser_click(object sender, EventArgs e)
        {
          
            ClsDwa Obj = new ClsDwa();
            
            try
            {
                cmdActiveUser.BackColor = System.Drawing.Color.Empty;
                Obj.Status = "D";
                Obj = Obj.GetDWAGridDetails(Obj);
              
                if (Obj.DtDWADetails.Rows.Count > 0)
                {
                    grdDWA.DataSource = Obj.DtDWADetails;
                    grdDWA.DataBind();
                   
                }
                else
                {
                    ShowEmptyGrid();
                }
                ViewState["DWA"] = Obj.DtDWADetails;

                cmdInActiveUser.BackColor = System.Drawing.Color.Green;
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
