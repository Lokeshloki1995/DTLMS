using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.graphusercontrols
{
    public partial class FailedDTr : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void view_link_click(object sender, EventArgs e)
        {


            string url = "/ViewDetailsgrid.aspx?RefId=true&Gridid=" + "Frequentlyfaileddtr";
            string s = "window.open('" + url + "','_blank');"; // '_newtab'
            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);


        }
    }
}