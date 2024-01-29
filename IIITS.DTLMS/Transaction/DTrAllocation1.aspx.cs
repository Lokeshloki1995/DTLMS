using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Data.OleDb;

namespace IIITS.DTLMS.Transaction
{
    public partial class DTrAllocation1 : System.Web.UI.Page
    {
        string strFormCode = "DTrAllocation1";
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
                lblMessage.Text = string.Empty;
                if (!IsPostBack)
                {

                    //CheckAccessRights("4");

                    AdminAccess();

                    string strQry = string.Empty;
                    strQry += "Title=Search and Select DTr CODE Details&";
                    strQry += "Query=select  \"TC_CODE\",\"TC_SLNO\" FROM \"TBLTCMASTER\"  WHERE \"TC_CURRENT_LOCATION\" =2 AND \"TC_STATUS\" IN (1,2) and  CAST(\"TC_LOCATION_ID\" AS TEXT) LIKE '" + objSession.OfficeCode + "%' AND {0} like %{1}% order by \"TC_CODE\" &";
                    strQry += "DBColName=CAST(\"TC_CODE\" AS TEXT)~CAST(\"TC_SLNO\" AS TEXT)&";
                    strQry += "ColDisplayName=DTr Code~DTr Slno&";
                    strQry = strQry.Replace("'", @"\'");
                    cmdSearchId.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtTcCode.ClientID + "&btn=" + cmdSearchId.ClientID + "',520,520," + txtTcCode.ClientID + ")");


                    strQry = "Title=Search and Select DTr CODE Details&";
                    strQry += "Query=select  \"TC_CODE\",\"TC_SLNO\" FROM \"TBLTCMASTER\"  WHERE \"TC_CURRENT_LOCATION\" =2  AND \"TC_STATUS\" IN (1,2) AND  CAST(\"TC_LOCATION_ID\" AS TEXT) LIKE '" + objSession.OfficeCode + "%' AND {0} like %{1}% order by \"TC_CODE\" &";
                    strQry += "DBColName=CAST(\"TC_CODE\" AS TEXT)~CAST(\"TC_SLNO\" AS TEXT)&";
                    strQry += "ColDisplayName=DTr Code~DTr Slno&";
                    strQry = strQry.Replace("'", @"\'");
                    cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtSecondDtrCode.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + txtSecondDtrCode.ClientID + ")");


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void cmdSearchId_Click(object sender, EventArgs e)
        {
            try
            {
                GetDTrDetails();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void GetDTrDetails()
        {
            try
            {
                clsDTrAllocation objAllocation = new clsDTrAllocation();
                DataTable dt = new DataTable();

                objAllocation.sFirstDTrCode = txtTcCode.Text;
                objAllocation.GetDTrDetails(objAllocation);

                txtfirstDtrSlNo.Text = objAllocation.sTcSlNo;
                txtFirstCapacity.Text = objAllocation.sCapacity;
                txtfirstDtcCode.Text = objAllocation.sFirstDTCCode;
                txtFirstDtrName.Text = objAllocation.sMakeName;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void GetSecondDTrDetails()
        {
            try
            {
                clsDTrAllocation objAllocation = new clsDTrAllocation();
                DataTable dt = new DataTable();
                objAllocation.sFirstDTrCode = txtSecondDtrCode.Text;
                objAllocation.GetDTrDetails(objAllocation);
                txtSecondDtrSlNo.Text = objAllocation.sTcSlNo;
                txtSecondCapacity.Text = objAllocation.sCapacity;
                txtSecondDtcCode.Text = objAllocation.sFirstDTCCode;
                txtSecondDtrName.Text = objAllocation.sMakeName;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                GetSecondDTrDetails();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdAllocate_Click(object sender, EventArgs e)
        {
            try
            {
                string[] Arr = new string[2];

                if (txtTcCode.Text.Trim() == txtSecondDtrCode.Text.Trim())
                {
                    ShowMsgBox("First DTr Code and Second DTr code should not be same");
                    return;
                }

                clsDTrAllocation objalloc = new clsDTrAllocation();
                objalloc.sFirstDTrCode = txtTcCode.Text;
                objalloc.sSecondDTrCode = txtSecondDtrCode.Text;
                objalloc.sFirstDTCCode = txtfirstDtcCode.Text;
                objalloc.sSecondDTCCode = txtSecondDtcCode.Text;
                objalloc.sCrby = objSession.UserId;
                objalloc.sUserName = objSession.FullName;

                Arr = objalloc.DTrAllocation(objalloc);
                if (Arr[1].ToString() == "0")
                {
                    ShowMsgBox(Arr[0].ToString());
                    cmdAllocate.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                Reset();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void Reset()
        {
            try
            {
                txtTcCode.Text = string.Empty;
                txtfirstDtrSlNo.Text = string.Empty;
                txtFirstCapacity.Text = string.Empty;
                txtfirstDtcCode.Text = string.Empty;
                txtFirstDtrName.Text = string.Empty;
                txtSecondDtrCode.Text = string.Empty;
                txtSecondDtrSlNo.Text = string.Empty;
                txtSecondCapacity.Text = string.Empty;
                txtSecondDtcCode.Text = string.Empty;
                txtSecondDtrName.Text = string.Empty;
                cmdAllocate.Enabled = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
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

                objApproval.sFormName = "UserCreate";
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

        #endregion

        public void AdminAccess()
        {
            try
            {
                if (objSession.RoleId != "8")
                {
                    Response.Redirect("~/UserRestrict.aspx", false);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}