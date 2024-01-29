using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<int> input = new List<int>();

            // first read input till there are nonempty items, means they are not null and not ""
            // also add read item to list do not need to read it again    
            string line;
            while ((line = Console.ReadLine()) != null && line != "")
            {
                input.Add(int.Parse(line));
            }

            // there is no need to use ElementAt in C# lists, you can simply access them by 
            // their index in O(1):
            
        }

       
    }
}