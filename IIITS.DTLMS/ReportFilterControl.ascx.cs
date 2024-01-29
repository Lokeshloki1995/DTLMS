using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;

namespace IIITS.DTLMS
{
    public partial class ReportFilterControl : System.Web.UI.UserControl
    {
        string strFormCode = "ReportFilterControl.ascx";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_CODE\" || '-' || \"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" ORDER BY \"CM_CIRCLE_CODE\"", "--Select--", cmbCircle);
                Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone);

            }
           
        }

        //public void BindData(string strOfficeID)
        //{
        //    cmbCorpOffice.Enabled = true;
        //    //cmbSubDiv.Enabled = true;
        //    //cmbDivision.Enabled = true;
        //    //cmbCircle.Enabled = true;
        //    //cmbZone.Enabled = true;

        //    Genaral.Load_Combo("SELECT CO_ID,CO_NAME FROM TBLCORPOFF ORDER BY CO_ID", "--Select--", cmbCorpOffice);

        //    if (cmbCorpOffice.SelectedIndex == 0)
        //    {

        //        cmbSubDiv.Enabled = false;
        //        cmbDivision.Enabled = false;
        //        cmbCircle.Enabled = false;
        //        cmbZone.Enabled = false;
        //        cmbAccUnit.Enabled = false;

        //    }

        //    }

       //  already  //Genaral.Load_Combo("SELECT ZO_ID,ZO_NAME,ZO_CO_ID FROM TBLZONE WHERE ZO_CO_ID LIKE  '" + cmbCorpOffice.SelectedValue + "%' ORDER BY ZO_ID", "--Select--", cmbZone);

        //    if (strOfficeID.Length > 0)
        //    {
        //        //load zone
        //        string strZoneID = Genaral.get_value("SELECT ZO_ID FROM TBLZONE WHERE ZO_CO_ID = '" + strOfficeID.Substring(0, 1) + "'");
        //        cmbZone.SelectedValue = strZoneID ;
        //        //  already    //cmbZone.SelectedIndex = 1;
        //        cmbZone.Enabled = false;

        //        cmbZone_SelectedIndexChanged(this, new EventArgs());
        //    }
        //    if (strOfficeID.Length > 0)
        //    {
        //        //  already   //cmbZone.Enabled = true;
        //        //circle
        //        if (cmbZone.SelectedIndex > 0)
        //        {

        //            string str = Genaral.get_value("SELECT CM_CIRCLE_CODE FROM TBLCIRCLE WHERE CM_CIRCLE_CODE = '" + strOfficeID.Substring(0, 1) + "' AND CM_ZO_ID='" + cmbZone.SelectedValue.ToString() + "'");
        //            cmbCircle.SelectedValue = str ;
        //            cmbCircle.Enabled = false;
        //            cmbCircle_SelectedIndexChanged(this, new EventArgs());
        //        }

        //    }
        //    if (strOfficeID.Length > 1)
        //    {
        //        //Division
                
        //        string str = Genaral.get_value("SELECT DIV_CODE FROM TBLDIVISION WHERE DIV_CODE = '" + strOfficeID.Substring(0, 2) + "' AND DIV_CICLE_CODE='" + cmbCircle.SelectedValue.ToString() + "' ORDER BY DIV_CODE");
        //        cmbDivision.SelectedValue = str ;
        //        //  already  //cmbDivision.Enabled = false;//first
        //        cmbDivision_SelectedIndexChanged(this, new EventArgs());
        //    }
        //    if (strOfficeID.Length > 2)
        //    {
        //        //Sub Division

        //        string str = Genaral.get_value("SELECT SD_SUBDIV_CODE FROM TBLSUBDIVMAST WHERE SD_SUBDIV_CODE = '" + strOfficeID.Substring(0, 3) + "' AND SD_DIV_CODE='" + cmbDivision.SelectedValue.ToString().ToString() + "'");
        //        cmbSubDiv.SelectedValue = str;
        //        //  already   //cmbSubDiv.Enabled = false;
        //    }
        //}

        /// <summary>
        /// this will calculateoffice ID depending upon comboselection
        /// </summary>
        /// <returns></returns>
        public string GetOfficeID()
        {
            
            string strOfficeId = string.Empty;
            //if (cmbCorpOffice.SelectedIndex == -1)
            //{

            //    cmbSubDiv.Enabled = false;
            //    cmbDivision.Enabled = false;
            //    cmbCircle.Enabled = false;
            //    cmbZone.Enabled = false;
            //    cmbAccUnit.Enabled = false;

            //}



            //if (cmbCorpOffice.SelectedIndex > 0)
            //{
            //    strOfficeId = cmbCorpOffice.SelectedValue.ToString();
               
            //}


            if (cmbZone.SelectedIndex > 0)
            {
                strOfficeId = cmbZone.SelectedValue.ToString();
            }

            if (cmbCircle.SelectedIndex > 0)
            {
                strOfficeId = cmbCircle.SelectedValue.ToString();
            }

            if (cmbDivision.SelectedIndex > 0)
            {
                strOfficeId = cmbDivision.SelectedValue.ToString();
            }

            if (cmbSubDiv.SelectedIndex > 0)
            {
                strOfficeId = cmbSubDiv.SelectedValue.ToString();
            }

            if (cmbAccUnit.SelectedIndex > 0)
            {
                strOfficeId = cmbAccUnit.SelectedValue.ToString();
            }
           //0 is the index and first two char from string
            return (strOfficeId);
        }


        /// <summary>
        /// this will iffice name  depending upon comboselection
        /// </summary>
        /// <returns></returns>
        public string GetOfficeName()
        {

            string strOfficeName = string.Empty;
            //if (cmbCorpOffice.SelectedIndex == -1)
            //{

            //    cmbSubDiv.Enabled = false;
            //    cmbDivision.Enabled = false;
            //    cmbCircle.Enabled = false;
            //    cmbZone.Enabled = false;
            //    cmbAccUnit.Enabled = false;

            //}



            //if (cmbCorpOffice.SelectedIndex > 0)
            //{
            //    strOfficeId = cmbCorpOffice.SelectedValue.ToString();

            //}


            if (cmbZone.SelectedIndex > 0)
            {
                strOfficeName = cmbZone.SelectedItem.ToString();
            }

            if (cmbCircle.SelectedIndex > 0)
            {
                strOfficeName = cmbCircle.SelectedItem.ToString();
            }

            if (cmbDivision.SelectedIndex > 0)
            {
                strOfficeName = cmbDivision.SelectedItem.ToString();
            }

            if (cmbSubDiv.SelectedIndex > 0)
            {
                strOfficeName = cmbSubDiv.SelectedItem.ToString();
            }

            if (cmbAccUnit.SelectedIndex > 0)
            {
                strOfficeName = cmbAccUnit.SelectedValue.ToString();
            }
            //0 is the index and first two char from string
            return (strOfficeName);
        }
        protected void cmbZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                
                if (cmbZone.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE \"CM_ZO_ID\"='" + cmbZone.SelectedValue + "'", "--Select--", cmbCircle);
                    clsUser objUser = new clsUser();         
                    objUser.sOffCode = cmbZone.SelectedValue;
                    

                }
                else
                {

                    clsUser objUser = new clsUser(); 
                    objUser.sOffCode = cmbZone.SelectedValue;             
                    cmbCircle.Items.Clear();
                    cmbDivision.Items.Clear();
                    cmbSubDiv.Items.Clear();
                    cmbAccUnit.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                //lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbZone_SelectedIndexChanged");
            }
        }

        //protected void cmbZone_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (cmbZone.SelectedIndex > 0)
        //    {
        //        cmbCircle.Enabled = true;
        //        cmbDivision.Enabled = false;
        //        cmbSubDiv.Enabled = false;
        //        cmbDivision.Items.Clear();
        //        cmbSubDiv.Items.Clear();
        //        //cmbAccUnit.Items.Clear();
        //        //cmbAccUnit.Enabled = false;
        //        Genaral.Load_Combo("select CM_CIRCLE_CODE,CM_CIRCLE_NAME,CM_ZO_ID from TBLCIRCLE  WHERE CM_ZO_ID LIKE  '" + cmbZone.SelectedValue + "%' ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle);
        //    }
        //    else
        //    {
        //        cmbCircle.Items.Clear();
        //        cmbDivision.Items.Clear();
        //        cmbSubDiv.Items.Clear();
        //    }
        //}
        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCircle.SelectedIndex > 0)
            {
               
                //cmbSubDiv.Enabled = false;
               // cmbAccUnit.Enabled = false;
                cmbSubDiv.Items.Clear();
                cmbAccUnit.Items.Clear();
                Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_CODE\" || '-' || \"DIV_NAME\" FROM \"TBLDIVISION\"  WHERE CAST(\"DIV_CICLE_CODE\" AS TEXT) LIKE  '" + cmbCircle.SelectedValue + "%' ORDER BY \"DIV_CODE\" ", "--Select--", cmbDivision);
            }
            else
            {
                cmbDivision.Items.Clear();
                cmbSubDiv.Items.Clear();
                cmbAccUnit.Items.Clear();
            }
         
        }

        protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDivision.SelectedIndex > 0)
            {
                
              //  cmbAccUnit.Enabled = false;
                cmbAccUnit.Items.Clear();
                Genaral.Load_Combo("SELECT \"SD_SUBDIV_CODE\",\"SD_SUBDIV_CODE\" || '-' || \"SD_SUBDIV_NAME\" FROM \"TBLSUBDIVMAST\"  WHERE CAST(\"SD_DIV_CODE\" AS TEXT) LIKE '" + cmbDivision.SelectedValue + "%'  ORDER BY \"SD_SUBDIV_CODE\"", "--Select--", cmbSubDiv);
            }
            else
            {
                cmbSubDiv.Items.Clear();
                cmbAccUnit.Items.Clear();
            }
        }

        protected void cmbCorpOffice_SelectedIndexChanged(object sender, EventArgs e)
        {
          //  Genaral.Load_Combo("SELECT ZO_ID,ZO_NAME,ZO_CO_ID FROM TBLZONE WHERE ZO_CO_ID LIKE  '" + cmbCorpOffice.SelectedValue + "%' ORDER BY ZO_ID", "--Select--", cmbZone);
          //  cmbZone.Enabled = true;
         
          //  cmbCircle.Enabled = false;
          //  cmbDivision.Enabled = false;
          //  cmbSubDiv.Enabled = false;
          ////  cmbAccUnit.Enabled = false;
          //  cmbCircle.Items.Clear();
          //  cmbDivision.Items.Clear();
          //  cmbSubDiv.Items.Clear();
          // // cmbAccUnit.Items.Clear();

        }

       

        protected void cmbAccUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
          
           
        }

        protected void cmbSubDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSubDiv.SelectedIndex > 0)
            {
                Genaral.Load_Combo("SELECT \"OM_CODE\",\"OM_CODE\" || '-' || \"OM_NAME\" FROM \"TBLOMSECMAST\" WHERE CAST(\"OM_SUBDIV_CODE\" AS TEXT) LIKE  '" + cmbSubDiv.SelectedValue + "%' ORDER BY \"OM_CODE\"", "--Select--", cmbAccUnit);
            }
            else
            {               
                cmbAccUnit.Items.Clear();
            }
          
            
        }

        public void Reset()
        {
            try
            {
                if(cmbZone.SelectedIndex > 0)
                {
                    cmbZone.SelectedIndex = 0;
                }

                cmbCircle.Items.Clear();
                cmbDivision.Items.Clear();
                cmbSubDiv.Items.Clear();
                cmbAccUnit.Items.Clear();                
            }
            catch(Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Reset");
            }
        }    
    }
}