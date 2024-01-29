using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;

namespace IIITS.DTLMS
{
    public partial class ApprovalHistoryView : System.Web.UI.UserControl
    {
        public string strFormCode = "ApprovalHistoryView";

        private string RecordId;
        public string BOID = string.Empty;
        public string DataId = string.Empty;
        public string WFAutoID = string.Empty;
        public string sRecordId { get{return RecordId; }set { RecordId = value;}}
        public string sBOID { get { return BOID; } set { BOID = value; } }
     
        //public string sRecordId = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {

                for (int i = 0; i < Session.Contents.Count; i++)
                {
                    var key = Session.Keys[i];
                    var value = Session[i];
                    if (key.ToString() == "DataId")
                    {
                        hdfDataID.Value = Session["DataId"].ToString();
                    }
                    else if (key.ToString() == "AutoID")
                    {
                        hdfAutoID.Value = Session["AutoID"].ToString();
                    }

                }                
                
                
                Session["DataId"] = "";
                Session["AutoID"] = "";

                //if(Session["BOId"].ToString() != "" )
                //{
                //    if (Session["BOId"].ToString() == "1020")
                //    {
                //        sBOID = Session["BOId"].ToString();
                //    }
                //}

            }
            LoadDetails(sRecordId, sBOID);
        }

        protected void LoadDetails(string sRecordId, string sBOID)
        {
            try
            {
                clsAprovalHistory obj = new clsAprovalHistory();
                DataTable dt = new DataTable();
                dt = obj.LoadApprovalFullHistory(sRecordId, sBOID, hdfDataID.Value, hdfAutoID.Value);
                grdApprovalHistory.DataSource = dt;
                ViewState["HISTORY"] = dt;
                grdApprovalHistory.DataBind();
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdApprovalHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdApprovalHistory.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Approval"];
                grdApprovalHistory.DataBind();
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}