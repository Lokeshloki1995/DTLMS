using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.IO;
using System.Net;
using System.Configuration;
using System.Data.OleDb;

namespace IIITS.DTLMS.Transaction
{
    public partial class Repairerwoexcel : System.Web.UI.Page
    {
        string strFormCode = "Repairerwoexcel";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {

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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}