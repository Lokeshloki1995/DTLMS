using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Configuration;

namespace IIITS.DTLMS.MasterForms
{
    public partial class BudgetMaster : System.Web.UI.Page
    {
        string Officecode = string.Empty;
        string strFormCode = "BudgetMaster";
        clsSession objSession;
        clsBudgetMaster objbudgetmast = new clsBudgetMaster();
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
                    lblErrormsg.Text = string.Empty;
                    int Division = Convert.ToInt32(ConfigurationSettings.AppSettings["Division_code"]);
                    txtBudgetDate.Attributes.Add("readonly", "readonly");

                    CalendarExtender_txtBudgetDate.EndDate = System.DateTime.Now;


                    txtobdate.Attributes.Add("readonly", "readonly");

                    CalendarExtender_txtobdate.EndDate = System.DateTime.Now;

                    if (!IsPostBack)
                    {
                        CheckAccessRights("4");
                        if (objSession.OfficeCode.Length > 1)
                        {
                            Officecode = objSession.OfficeCode.Substring(0, Division);
                        }
                        else
                        {
                            Officecode = objSession.OfficeCode;
                        }
                        Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\" WHERE   CAST(\"DIV_CODE\" AS TEXT) = '" + Officecode + "' ORDER BY \"DIV_CODE\"", "--Select--", cmbDivision);
                        Genaral.Load_Combo("SELECT \"SCHM_ID\" , \"SCHM_ACCCODE\" ||'~'|| \"SCHM_NAME\" from \"TBLDTCSCHEME\"  ORDER BY \"SCHM_ID\"", "--Select--", cmbAccCode);
                        Genaral.Load_Combo("SELECT \"FY_YEARS\",\"FY_YEARS\" FROM \"TBLFINANCIALYEAR\" WHERE \"FY_STATUS\"='1'", "--Select--", cmbFinYear);

                        cmbDivision.SelectedValue = objbudgetmast.getdivcode(Officecode);
                        if (Request.QueryString["BudgetId"] != null && Convert.ToString(Request.QueryString["BudgetId"]) != "")
                        {
                            txtBudgetId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["BudgetId"]));
                            GetBudgetDetails();
                            txtBudgetNo.Enabled = false;
                            cmbAccCode.Enabled = false;
                            cmdReset.Visible = false;
                            txtOb.Enabled = false;
                        }

                    }
                }
                
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }
        public void GetBudgetDetails()
        {
            try
            {
                clsBudgetMaster objBudgetMaster = new clsBudgetMaster();
                DataTable DtBudget = objBudgetMaster.GetBudgetDetails(txtBudgetId.Text);
                if (DtBudget.Rows.Count > 0)
                {
                    txtBudgetNo.Text = Convert.ToString(DtBudget.Rows[0]["BM_NO"]);
                    txtBudgetDate.Text = Convert.ToString(DtBudget.Rows[0]["BM_DATE"]);
                    txtBudgetAmount.Text = Convert.ToString(DtBudget.Rows[0]["BM_AMOUNT"]);
                    txtOb.Text = Convert.ToString(DtBudget.Rows[0]["BM_OB_AMNT"]);
                    txtobdate.Text = Convert.ToString(DtBudget.Rows[0]["BM_OB_DATE"]);
                    cmbAccCode.SelectedValue = Convert.ToString(DtBudget.Rows[0]["BM_ACC_CODE"]);
                    cmbDivision.SelectedValue = Convert.ToString(DtBudget.Rows[0]["BM_DIV_CODE"]);
                    cmbFinYear.SelectedValue = Convert.ToString(DtBudget.Rows[0]["BM_FIN_YEAR"]);


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
                string[] ArrResult = new string[2];
                bool bAccResult;
               
                 bAccResult = CheckAccessRights("4");
                
                if (bAccResult == false)
                {
                    return;
                }
               clsBudgetMaster objbudgetmast = new clsBudgetMaster();
               objbudgetmast.budgetno = txtBudgetNo.Text.Trim().ToUpper();
               objbudgetmast.budgetdate = txtBudgetDate.Text.Trim().ToUpper();
               objbudgetmast.budgetamount = txtBudgetAmount.Text.Trim().ToUpper();
               objbudgetmast.budgetaccCode = cmbAccCode.SelectedValue;
               objbudgetmast.budgetDivcode = cmbDivision.SelectedValue;
               objbudgetmast.budgetFinyear = cmbFinYear.SelectedValue;
               objbudgetmast.budgetobamnt = txtOb.Text.Trim().ToUpper();

               objbudgetmast.budgetobdate = txtobdate.Text.Trim().ToUpper();


               if (objbudgetmast.budgetamount == "")
               {
                   objbudgetmast.budgetamount ="0";
               }
               else  if (objbudgetmast.budgetobamnt == "")
               {
                   objbudgetmast.budgetobamnt = "0";
               }


               objbudgetmast.budgetId = Convert.ToInt64(txtBudgetId.Text);

               ArrResult = objbudgetmast.Save_updatebudgetmast(objbudgetmast);

                if (objSession.sTransactionLog == "1")
                {
                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " Budget Master ");
                }


                if (ArrResult[1].ToString() == "0")
               {
                   ShowMsgBox(ArrResult[0].ToString());
                   Reset();
               }
               if (ArrResult[1].ToString() == "1")
               {
                   ShowMsgBox(ArrResult[0].ToString());
                  
               }
               if (ArrResult[1].ToString() == "2")
               {
                   ShowMsgBox(ArrResult[0].ToString());

               }
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void Reset()
        {
            try
            {
                txtBudgetAmount.Text = string.Empty;
                txtOb.Text = string.Empty;
                cmbAccCode.SelectedIndex = 0;
                cmbFinYear.SelectedIndex = 0;
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
                txtBudgetNo.Text = string.Empty;
                txtBudgetDate.Text = string.Empty;
                txtBudgetAmount.Text = string.Empty;
                txtOb.Text = string.Empty;
                //cmbAccCode.SelectedIndex = 0;
                cmbFinYear.SelectedIndex = 0;
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

        private void ShowMsgBox(string sMsg)
        {
            try
            {
                String sShowMsg = string.Empty;
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