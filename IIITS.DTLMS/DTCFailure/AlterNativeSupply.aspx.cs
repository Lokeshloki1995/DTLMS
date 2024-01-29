using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;


namespace IIITS.DTLMS.DTCFailure
{
    public partial class AlterNativeSupply : System.Web.UI.Page
    {
        string strFormCode = "AlterNativeSupply";
        string sDFId;

        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
            {
                Response.Redirect("~/Login.aspx", false);
            }
            else
            {
                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;
                if (!IsPostBack)
                {
                    Genaral.Load_Combo("SELECT \"MD_ID\", \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_TYPE\" = 'ARM' ORDER BY \"MD_ORDER_BY\" ", "--Select--", cmbReplaceEntry);
                    LoadSearchWindow();
                }
            }           
        }

        public void LoadSearchWindow()
        {
            try
            {
                string strQry = string.Empty;
                strQry = "Title=Search and Select Transformer Centre Failure Details&";
                strQry += "Query=select \"DT_CODE\",\"DF_ID\" FROM \"TBLDTCMAST\",\"TBLDTCFAILURE\" WHERE \"DF_DTC_CODE\" = \"DT_CODE\" AND  CAST(\"DT_OM_SLNO\" AS TEXT) LIKE '" + objSession.OfficeCode + "%' AND ";
                strQry += " \"DF_REPLACE_FLAG\" = 0  AND {0} like %{1}% order by \"DT_CODE\" &";
                strQry += "DBColName=\"DT_CODE\"~\"DF_ID\"&";
                strQry += "ColDisplayName=Transformer Centre Code~DF ID&";

                strQry = strQry.Replace("'", @"\'");

                cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtDTCCode.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + txtDTCCode.ClientID + ")");

                strQry = string.Empty;
                strQry = "Title=Search and Select Transformer Centre Failure Details&";
                strQry += "Query=select \"TC_CODE\" ,\"DF_ID\" FROM \"TBLTCMASTER\",\"TBLDTCFAILURE\" WHERE  \"DF_EQUIPMENT_ID\" = \"TC_CODE\" AND CAST(\"TC_LOCATION_ID\" AS TEXT) = '" + clsStoreOffice.GetStoreID( objSession.OfficeCode) + "' AND ";
                strQry += " \"DF_REPLACE_FLAG\" = 0  AND {0} like %{1}% order by \"TC_CODE\" &";
                strQry += "DBColName=\"TC_CODE\"~\"DF_ID\"&";
                strQry += "ColDisplayName=DTr Code~DF ID&";

                strQry = strQry.Replace("'", @"\'");

                cmdDTRSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtTcCode.ClientID + "&btn=" + cmdDTRSearch.ClientID + "',520,520," + txtTcCode.ClientID + ")");

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdDTRSearch_Click(object sender, EventArgs e)
        {
            try
            {
                
                clsAlterPowerSupply obj = new clsAlterPowerSupply();
                string sFailid = obj.GetFailureId(txtTcCode.Text, "0");
                hdfDTCcode.Value = sFailid;
                LoadFailureDetails(sFailid);

                txtTcCode.ReadOnly = true;  // add newly on 24-08-2023 bs we remove Readonly from the clint side to Server side.
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
                clsAlterPowerSupply obj = new clsAlterPowerSupply();
                string sFailid = obj.GetFailureId("0", txtDTCCode.Text);
                hdfDTCcode.Value = sFailid;
                LoadFailureDetails(sFailid);

                txtTcCode.ReadOnly = true; // add newly on 24-08-2023 bs we remove Readonly from the clint side to Server side.
            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void LoadFailureDetails(string sFailureId)
        {
            clsFailureEntry objFailure = new clsFailureEntry();
            objFailure.sFailureId = sFailureId;
            objFailure.GetFailureDetails(objFailure);

            if (objFailure.sDtcName != null)
            {
                sDFId = objFailure.sFailureId;
                txtDTCName.Text = objFailure.sDtcName;
                txtDtcId.Text = objFailure.sDtcId;
                txtDTCCode.Text = objFailure.sDtcCode;
                txtDTCName.Text = objFailure.sDtcName;
                txtLoadKW.Text = objFailure.sDtcLoadKw;
                txtLoadHP.Text = objFailure.sDtcLoadHp;
                txtCapacity.Text = objFailure.sDtcCapacity;
                txtLocation.Text = objFailure.sDtcLocation;
                txtTcCode.Text = objFailure.sDtcTcCode;
                txtTCSlno.Text = objFailure.sDtcTcSlno;
                txtTCId.Text = objFailure.sTCId;
                txtTCMake.Text = objFailure.sDtcTcMake;
                txtLastRepairDate.Text = objFailure.sLastRepairedDate;
                txtLastRepairer.Text = objFailure.sLastRepairedBy;
                txtLocation.Text = objFailure.sDtcLocation;
                txtrate.Text = objFailure.sRating;
                txtConditionOfTC.Text = objFailure.sConditionoftc;
                cmbReplaceEntry.SelectedValue = objFailure.sAlternateReplaceType;
            }
          
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(cmbReplaceEntry.SelectedIndex == 0)
                {
                    ShowMsgBox("Please Select the Alternative Repalcement");
                    return;
                }

                clsAlterPowerSupply obj = new clsAlterPowerSupply();
                bool status = obj.validateFailure(hdfDTCcode.Value);
                if(status == false)
                {
                    ShowMsgBox("Estimation already approved for Selected Transformer Centre/Transformer");
                    return;
                }

                status = obj.SaveAlternateSupplydetails(cmbReplaceEntry.SelectedValue, hdfDTCcode.Value);
                if (objSession.sTransactionLog == "1")
                {
                    Genaral.TransactionLog(objSession.sClientIP, objSession.UserId, " Alternative PowerSupply ");
                }
                if (status == true)
                {
                    ShowMsgBox("Update Successfullt");
                    return;
                }

            }
            catch(Exception ex)
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

        protected void cmdClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
                {
                    Response.Redirect("/Approval/ApprovalInbox.aspx", false);
                }
                else
                {
                    Response.Redirect("FailureEntryView.aspx", false);
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