using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;


namespace IIITS.DTLMS
{
    public partial class SearchWindow : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {               
                if (!IsPostBack)
                {
                    if (LoadData())
                    {
                        pnlControls.Visible = true;
                    }
                    else
                    {
                        pnlControls.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                // CustomExceptionHandler.ExceptionHandler(ex, CustomExceptionHandler.ErrorLayer.UI);
            }
        }
        /// <summary>
        /// this will loads necessary data and verifies the required data in the query string
        /// </summary>
        /// <returns></returns>
        bool LoadData()
        {
            //==========================================================================================
            //  Slno  Query String              Optional               Purpose   
            //------------------------------------------------------------------------------------------
            //  1.      Title                       NO              This will be displayed on the page (Page Title)
            //  2.      Query                       No              Select query
            //  3.      DBColName                   No              Table column name for where clause
            //  4.      ColDisplayName              No              Table column respective display name in combo
            //  5.      AllowEmptySearch            Yes(Value=ON)   This will disables validator controls, so that all serach is allowed
            //  6.      AutoSearch                  Yes(Value=ON)   If this value is ON, then by default search will be called once the page is loaded.
            //
            //
            //
            //
            //  Sample 1: /SelectWindow.aspx?Title=Search and Select Item.&Query=select * from tblstock where ST_STORE_SLNO=1 and {0} like {1}%&DBColName=ST_ITEM_CODE~ST_QTY&ColDisplayName=Item Code~Stock Qty
            //  Sample 2: /SelectWindow.aspx?Title=Search and Select Item.&Query=select * from tblstock where ST_STORE_SLNO=1 and {0} like {1}%&DBColName=ST_ITEM_CODE~ST_QTY&ColDisplayName=Item Code~Stock Qty&AllowEmptySearch=ON&AutoSearch=ON
            //==========================================================================================
            char[] separator = { '~' };
            if (Request.QueryString["Title"] == null)
            {
                Response.Write("Title Value not found,hence cant load..");
                return (false);
            }
            else
            {
                lblTitle.Text = Request.QueryString["Title"].ToString();
            }

            if (Request.QueryString["Query"] == null)
            {
                Response.Write("Query Value not found,hence cant load..");
                return (false);
            }

            if (Request.QueryString["DBColName"] == null)
            {
                Response.Write("DBColName Value not found,hence cant load..");
                return (false);
            }

            if (Request.QueryString["ColDisplayName"] == null)
            {
                Response.Write("ColDisplayName Value not found,hence cant load.. ");
                return (false);
            }
            else
            {
                string[] strDisplayName;
                string[] strDBColName;
                strDisplayName = Request.QueryString["ColDisplayName"].ToString().Split(separator, StringSplitOptions.RemoveEmptyEntries);
                strDBColName = Request.QueryString["DBColName"].ToString().Split(separator, StringSplitOptions.RemoveEmptyEntries);

                //compare the no of cols present in the display name
                if (strDBColName.Length != strDisplayName.Length)
                {
                    Response.Write("No of columns present in ColDisplayName and DBColName are not matching, hence cant load..");
                    return (false);
                }


                //load the display name into drop down
                for (int i = 0; i < strDBColName.Length; i++)
                {
                    ListItem itm = new ListItem();
                    itm.Value = strDBColName[i];
                    itm.Text = strDisplayName[i];
                    cmbFilterType.Items.Add(itm);
                }

            }

            //allow all serach, i.e allow search without search pharse..
            // by default is is not allowed
            if (Request.QueryString["AllowEmptySearch"] != null)
            {
                if (Request.QueryString["AllowEmptySearch"].ToString().ToUpper().Equals("ON"))
                {
                    //disable validator field
                    //rfvCombo.Enabled = false;
                    //rfvText.Enabled = false;

                    //if auto search enabled, populate all items.
                    if (Request.QueryString["AutoSearch"] != null)
                    {
                        if (Request.QueryString["AutoSearch"].ToString().ToUpper().Equals("ON"))
                        {
                            PopulateTable();
                        }
                    }
                }
            }

            //every thing is fine, hence show controls
            return (true);


        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Text = string.Empty;
                tblResult.Visible = true;
                PopulateTable();
            }
            catch (Exception ex)
            {
                //CustomExceptionHandler.ExceptionHandler(ex, CustomExceptionHandler.ErrorLayer.UI);
            }
        }
        void PopulateTable()
        {
            //here data is populated into table..
          
            string strFinalQry = string.Empty;
            if (cmbFilterType.SelectedIndex > 0)
            {
                //search phrase id selected
                string strQry = Request.QueryString["Query"].ToString();

                //add ' char in the qry
                strQry = strQry.Replace("%{1}%", "'{1}%'");
                strFinalQry = string.Format(strQry, cmbFilterType.SelectedValue, txtSearch.Text.ToUpper());
            }
            else
            {
                //no search phrase, hence populate all data
                strFinalQry = string.Format(Request.QueryString["Query"].ToString(), "1", "1");
            }

            // single quote will be converted into \\' in the query string, hence fix that, otherwise would cause sql error
            strFinalQry = strFinalQry.Replace("\\'", "'");
            strFinalQry = strFinalQry.Replace("\'", "'");
            strFinalQry = strFinalQry.Replace("8TT8", "+");

            Genaral.Load_Table(strFinalQry, tblResult);
            if (tblResult.Rows.Count == 1)
            {
                tblResult.Visible = false;
                lblMsg.Text = "No Details Available for this Search";
            }

        }
    }
}
