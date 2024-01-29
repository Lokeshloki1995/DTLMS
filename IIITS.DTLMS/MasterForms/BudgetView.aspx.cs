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
    public partial class BudgetView : System.Web.UI.Page
    {
        string strFormCode = "BudgetView";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
               
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                else
                {
                    objSession = (clsSession)Session["clsSession"];
                    CheckAccessRights("4");
                    if (!IsPostBack)
                    {
                        Genaral.Load_Combo("SELECT \"FY_YEARS\",\"FY_YEARS\" FROM \"TBLFINANCIALYEAR\"", "--Select--", cmbFinYear);
                        LoadBudgetGrid("", "");
                    }
                }
                
            }
            catch (Exception ex)
            {

                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sFormName = "BudgetMast";
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

        protected void cmdNew_Click(object sender, EventArgs e)
        {
            try
            {

                Response.Redirect("BudgetMaster.aspx", false);

            }
            catch (Exception ex)
            {

                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadBudgetGrid(string sFinYear, string strBudgetno = "")
        {
            try
            {
                clsBudgetMaster objBudgetMaster = new clsBudgetMaster();

                DataTable dt = new DataTable();
                string divcode = objSession.OfficeCode;
                dt = objBudgetMaster.LoadBudgetDetails(sFinYear, divcode,strBudgetno);

                if (dt.Rows.Count <= 0)
                {
                    DataTable dtBudgetDetails = new DataTable();
                    DataRow newRow = dtBudgetDetails.NewRow();
                    dtBudgetDetails.Rows.Add(newRow);
                    dtBudgetDetails.Columns.Add("BM_ID");
                    dtBudgetDetails.Columns.Add("BM_NO");
                    dtBudgetDetails.Columns.Add("BM_ACC_CODE");
                    dtBudgetDetails.Columns.Add("BM_DIV_CODE");
                    dtBudgetDetails.Columns.Add("BM_FIN_YEAR");
                    dtBudgetDetails.Columns.Add("DIV_NAME");
                    dtBudgetDetails.Columns.Add("FY_STATUS");
                    dtBudgetDetails.Columns.Add("BM_AMOUNT");
                    dtBudgetDetails.Columns.Add("BM_OB_AMNT");
                    

                    grdBudget.DataSource = dtBudgetDetails;
                    grdBudget.DataBind();

                    int iColCount = grdBudget.Rows[0].Cells.Count;
                    grdBudget.Rows[0].Cells.Clear();
                    grdBudget.Rows[0].Cells.Add(new TableCell());
                    grdBudget.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdBudget.Rows[0].Cells[0].Text = "No Records Found";


                }

                else
                {

                    grdBudget.DataSource = dt;
                    grdBudget.DataBind();
                    ViewState["Budget"] = dt;
                }


            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void grdBudget_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdBudget.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Budget"];
                dt.Columns["BM_NO"].AllowDBNull = true;
                grdBudget.DataSource = dt;
                grdBudget.DataBind();


            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdBudget_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                    TextBox txtBMNO = (TextBox)row.FindControl("txtBMNO");


                    DataTable dt = (DataTable)ViewState["Budget"];
                    DataView dv = new DataView();
                    dv = dt.DefaultView;
                    if (txtBMNO.Text != "")
                    {
                        sFilter = "BM_NO Like '%" + txtBMNO.Text.Replace("'", "`") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdBudget.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdBudget.DataSource = dv;
                            ViewState["Budget"] = dv.ToTable();
                            grdBudget.DataBind();

                        }
                        else
                        {
                            ViewState["Budget"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadBudgetGrid("", "");
                    }
                }

                if (e.CommandName == "create")
                {

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                    string sBudgetId = ((Label)row.FindControl("lblBudgetId")).Text;
                    sBudgetId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sBudgetId));
                    Response.Redirect("BudgetMaster.aspx?BudgetId=" + sBudgetId + "", false);


                }
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("BM_ID");
                dt.Columns.Add("BM_NO");
                dt.Columns.Add("BM_ACC_CODE");
                dt.Columns.Add("BM_DIV_CODE");
                dt.Columns.Add("BM_FIN_YEAR");
                dt.Columns.Add("DIV_NAME");
                dt.Columns.Add("BM_AMOUNT");
                dt.Columns.Add("FY_STATUS");
                dt.Columns.Add("BM_OB_AMNT");
                
                grdBudget.DataSource = dt;
                grdBudget.DataBind();

                int iColCount = grdBudget.Rows[0].Cells.Count;
                grdBudget.Rows[0].Cells.Clear();
                grdBudget.Rows[0].Cells.Add(new TableCell());
                grdBudget.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdBudget.Rows[0].Cells[0].Text = "No Records Found";

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
                string sFinYear = cmbFinYear.SelectedValue;
                if (sFinYear == "0" || sFinYear == "--Select--")
                {
                    ShowMsgBox("Select the Financial Year");
                    cmbFinYear.Focus();
                    return;
                }
                LoadBudgetGrid(sFinYear);
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

        protected void grdBudget_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblStatus;
                    lblStatus = (Label)e.Row.FindControl("lblStatus");
                    ImageButton imgBtnEdit;
                    imgBtnEdit = (ImageButton)e.Row.FindControl("imgBtnEdit");
                    if (lblStatus.Text == "0")
                    {

                        imgBtnEdit.Enabled = false;
                        imgBtnEdit.ToolTip = "You Can Edit Only the Current Financial Year...!";
                    }
                    else
                    {
                        imgBtnEdit.Enabled = true;
                        imgBtnEdit.ToolTip = "";
                    }
                }
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}