using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Web.Services;

namespace IIITS.DTLMS.graphusercontrols
{
    public partial class FailureStagesGraph : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void view_link_click(object sender, EventArgs e)
        {

            // ViewDetailsgrid.ascx?RefId=true&Gridid=FailureDetails

            string url = "/ViewDetailsgrid.aspx?RefId=true&Gridid=" + "FailureDetails";
            string s = "window.open('" + url + "','_blank');"; // '_newtab'
            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);

            //Page.Response.Redirect(url);


            //var uc = (ViewDetailsgrid)Page.LoadControl("~/ViewDetailsgrid.ascx?RefId=true&Gridid=" + "FailureDetails");


        }
    }

}