using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Configuration;

namespace IIITS.DTLMS
{
    public partial class ViewDetailsgrid : System.Web.UI.Page
    {
        string strFormCode = "ViewDetailsgrid";
        clsSession objSession;
        int zone_code = Convert.ToInt32(ConfigurationManager.AppSettings["Zone_code"]);
        int Circle_code = Convert.ToInt32(ConfigurationManager.AppSettings["Circle_code"]);
        int Division_code = Convert.ToInt32(ConfigurationManager.AppSettings["Division_code"]);
        int SubDiv_code = Convert.ToInt32(ConfigurationManager.AppSettings["SubDiv_code"]);
        int Section_code = Convert.ToInt32(ConfigurationManager.AppSettings["Section_code"]);


        protected void Page_Load(object sender, EventArgs e)
        {
            string[] Delete_Session_array = new string[7];
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }

                objSession = (clsSession)Session["clsSession"];

                if (Session["arrSave_ImageSession_String"] != null)
                {
                    Delete_Session_array = Session["arrSave_ImageSession_String"] as string[];
                    for (int i = 0; i < 7; i++)
                    {
                        if (Delete_Session_array[i] != "")
                        {
                            Session.Remove(Delete_Session_array[i]);
                        }
                    }
                    Session.Remove("arrSave_ImageSession_String");
                }

                lblMessage.Text = string.Empty;
                if (!IsPostBack)
                {
                    if (Request.QueryString["RefId"] != null && Request.QueryString["RefId"].ToString() != "")
                    {
                        hdfRefId.Value = Request.QueryString["RefId"].ToString();
                    }

                    if (Request.QueryString["Gridid"] != null && Request.QueryString["Gridid"].ToString() != "")
                    {
                        hdfGridId.Value = Request.QueryString["Gridid"].ToString();
                    }

                    if (hdfRefId.Value == "true")
                    {
                        if (hdfGridId.Value == "FailureDetails")
                        {

                            LoadFailureDetailsview();

                        }

                        else if (hdfGridId.Value == "TCDetails")
                        {

                            LoadTCDetailsview();

                        }

                        else if (hdfGridId.Value == "TcStatus")
                        {

                            LoadTCstatusview();

                        }

                        else if (hdfGridId.Value == "Alternativepowersupply")
                        {

                            Loadalternativesupplyview();

                        }

                        else if (hdfGridId.Value == "RepairerPerformance")
                        {

                            LoadRepairerPerformanceview();

                        }

                       else if (hdfGridId.Value == "PendingReplacement")
                        {

                            LoadPendingReplacementview();

                        }

                        else if (hdfGridId.Value == "RIstatus")
                        {

                            LoadRistatusview();

                        }
                        else if (hdfGridId.Value == "Frequentlyfailed")
                        {

                            LoadFrequentlyfailedview();

                        }

                        else if (hdfGridId.Value == "Frequentlyfaileddtr")
                        {

                            LoadFrequentlyfaileddtrview();

                        }

                        else if (hdfGridId.Value == "Expenditure")
                        {

                            Loadexpenditureview();

                        }

                        else if (hdfGridId.Value == "Po")
                        {

                            LoadPoview();

                        }


                    }
                    else
                    {

                        cmdBack.Visible = false;
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsgPL(ex.Message);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }


        public void LoadFailureDetailsview(string sOfficeCode = "")
        {
            try
            {
                clsDashboardUcForms objDashboard = new clsDashboardUcForms();
                DataTable dt = new DataTable();

                bool CurrentDate = false;

                if (hdfRefId.Value == "true")
                {

                    lblDetailsview.Text = "View Failure Status  as On " + System.DateTime.Now.ToString("dd-MMM-yyyy");
                }
                objDashboard.roleId = objSession.RoleId;
                objDashboard.roleType = objSession.sRoleType;

                objDashboard.officecode = sOfficeCode;

                dt = objDashboard.LoadFailureDetailsview(objDashboard);
                if (dt.Rows.Count == 0)
                {

                    ShowEmptyfailureGrid();
                    ViewState["FailureDetailview"] = dt;
                }
                else
                {
                    grdlfailureDetailsview.DataSource = dt;
                    grdlfailureDetailsview.DataBind();
                    ViewState["FailureDetailview"] = dt;

                }


            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }


        }

        public void LoadTCDetailsview(string sOfficeCode = "")
        {
            try
            {
                clsDashboardUcForms objDashboard = new clsDashboardUcForms();
                DataTable dt = new DataTable();

                bool CurrentDate = false;

                if (hdfRefId.Value == "true")
                {

                    lblDetailsview.Text = "View TC Guarantee Status  as On " + System.DateTime.Now.ToString("dd-MMM-yyyy");
                }

                objDashboard.roleId = objSession.RoleId;
                objDashboard.roleType = objSession.sRoleType;
                objDashboard.officecode = sOfficeCode;

                dt = objDashboard.LoadTransformerDetailsstackgridview(objDashboard);
                if (dt.Rows.Count == 0)
                {

                    ShowEmptyTCGrid();
                    ViewState["TCDetailview"] = dt;
                }
                else
                {
                    grdlTCDetailsview.DataSource = dt;
                    grdlTCDetailsview.DataBind();
                    ViewState["TCDetailview"] = dt;

                }


            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }


        }

        public void LoadTCstatusview(string sOfficeCode = "")
        {
            try
            {
                clsDashboardUcForms objDashboard = new clsDashboardUcForms();
                DataTable dt = new DataTable();

                bool CurrentDate = false;

                if (hdfRefId.Value == "true")
                {

                    lblDetailsview.Text = "View TC Condition Status  as On " + System.DateTime.Now.ToString("dd-MMM-yyyy");
                }

                objDashboard.roleId = objSession.RoleId;
                objDashboard.roleType = objSession.sRoleType;
                objDashboard.officecode = sOfficeCode;

                dt = objDashboard.LoadTcStatusgridview(objDashboard);
                if (dt.Rows.Count == 0)
                {

                    ShowEmptyTCstatusGrid();
                    ViewState["TCstatusview"] = dt;
                }
                else
                {
                    grdlTCStatusview.DataSource = dt;
                    grdlTCStatusview.DataBind();
                    ViewState["TCstatusview"] = dt;

                }


            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }


        }

        public void Loadalternativesupplyview(string sOfficeCode = "")
        {
            try
            {
                clsDashboardUcForms objDashboard = new clsDashboardUcForms();
                DataTable dt = new DataTable();

                bool CurrentDate = false;

                if (hdfRefId.Value == "true")
                {

                    lblDetailsview.Text = "View Alternate Power Supply Status  as On " + System.DateTime.Now.ToString("dd-MMM-yyyy");
                }

                objDashboard.roleId = objSession.RoleId;
                objDashboard.roleType = objSession.sRoleType;
                objDashboard.officecode = sOfficeCode;

                dt = objDashboard.LoadAlternativeDetailsgridview(objDashboard);
                if (dt.Rows.Count == 0)
                {

                    ShowEmptyalternativeGrid();
                    ViewState["alternativeview"] = dt;
                }
                else
                {
                    grdlalternativeview.DataSource = dt;
                    grdlalternativeview.DataBind();
                    ViewState["alternativeview"] = dt;

                }


            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }


        }

        public void LoadRepairerPerformanceview(string sOfficeCode = "")
        {
            try
            {
                clsDashboardUcForms objDashboard = new clsDashboardUcForms();
                DataTable dt = new DataTable();

                bool CurrentDate = false;

                if (hdfRefId.Value == "true")
                {

                    lblDetailsview.Text = "View Repairer Performanceview  Details  as On " + System.DateTime.Now.ToString("dd-MMM-yyyy");
                }

                objDashboard.roleId = objSession.RoleId;
                objDashboard.roleType = objSession.sRoleType;
                objDashboard.officecode = sOfficeCode;

                dt = objDashboard.LoadRepairerPerformanceDetailsgridview(objDashboard);
                if (dt.Rows.Count == 0)
                {

                    ShowRepairerPerformanceGrid();
                    ViewState["repairerperformance"] = dt;
                }
                else
                {
                    grdRepairerPerformanceview.DataSource = dt;
                    grdRepairerPerformanceview.DataBind();
                    ViewState["repairerperformance"] = dt;

                }


            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }


        }

        public void LoadPendingReplacementview(string sOfficeCode = "")
        {
            try
            {
                clsDashboardUcForms objDashboard = new clsDashboardUcForms();
                DataTable dt = new DataTable();

                bool CurrentDate = false;

                if (hdfRefId.Value == "true")
                {

                    lblDetailsview.Text = "View Pending Replacement view  Details  as On " + System.DateTime.Now.ToString("dd-MMM-yyyy");
                }

                objDashboard.roleId = objSession.RoleId;
                objDashboard.roleType = objSession.sRoleType;
                objDashboard.officecode = sOfficeCode;

                dt = objDashboard.LoadPendingReplacementDetailsgridview(objDashboard);
                if (dt.Rows.Count == 0)
                {

                    ShowpendingreplacementviewGrid();
                    ViewState["PendingReplacement"] = dt;
                }
                else
                {
                    grdpendingreplacementview.DataSource = dt;
                    grdpendingreplacementview.DataBind();
                    ViewState["PendingReplacement"] = dt;

                }


            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }


        }

        public void LoadRistatusview(string sOfficeCode = "")
        {
            try
            {
                clsDashboardUcForms objDashboard = new clsDashboardUcForms();
                DataTable dt = new DataTable();

                bool CurrentDate = false;

                if (hdfRefId.Value == "true")
                {

                    lblDetailsview.Text = "RI Status view  Details  as On " + System.DateTime.Now.ToString("dd-MMM-yyyy");
                }

                objDashboard.roleId = objSession.RoleId;
                objDashboard.roleType = objSession.sRoleType;
                objDashboard.officecode = sOfficeCode;

                dt = objDashboard.Loadridetalsgridview(objDashboard);
                if (dt.Rows.Count == 0)
                {

                    ShowridetailsviewGrid();
                    ViewState["Ridetails"] = dt;
                }
                else
                {
                    Ridetails.DataSource = dt;
                    Ridetails.DataBind();
                    ViewState["Ridetails"] = dt;

                }


            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }


        }

        public void LoadFrequentlyfailedview(string sOfficeCode = "")
        {
            try
            {
                clsDashboardUcForms objDashboard = new clsDashboardUcForms();
                DataTable dt = new DataTable();

                bool CurrentDate = false;

                if (hdfRefId.Value == "true")
                {

                    lblDetailsview.Text = "Frequently Failed dtc  view    as On " + System.DateTime.Now.ToString("dd-MMM-yyyy");
                }

                objDashboard.roleId = objSession.RoleId;
                objDashboard.roleType = objSession.sRoleType;
                objDashboard.officecode = sOfficeCode;

                dt = objDashboard.Loadfrequentlyfailedgridview(objDashboard);
                if (dt.Rows.Count == 0)
                {

                    ShowfrequentlyfailedviewGrid();
                    ViewState["frequentlyfailed"] = dt;
                }
                else
                {
                    frequentlyfaildtc.Visible = false;
                    frequentlyfail.Visible = true;
                    frequentlyfail.DataSource = dt;
                    frequentlyfail.DataBind();
                   
                    ViewState["frequentlyfailed"] = dt;

                }


            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }


        }

        public void LoadFrequentlyfaileddtrview(string sOfficeCode = "")
        {
            try
            {
                clsDashboardUcForms objDashboard = new clsDashboardUcForms();
                DataTable dt = new DataTable();

                bool CurrentDate = false;

                if (hdfRefId.Value == "true")
                {

                    lblDetailsview.Text = "Frequently Failed dtr  view    as On " + System.DateTime.Now.ToString("dd-MMM-yyyy");
                }

                objDashboard.roleId = objSession.RoleId;
                objDashboard.roleType = objSession.sRoleType;
                objDashboard.officecode = sOfficeCode;

                dt = objDashboard.Loadfrequentlyfaileddtrgridview(objDashboard);
                if (dt.Rows.Count == 0)
                {

                    ShowfrequentlyfaileddtrviewGrid();
                    ViewState["frequentlyfaileddtr"] = dt;
                }
                else
                {
                    //frequentlyfaildtc.Visible = false;
                    frequentlyfaildtr.Visible = true;
                    frequentlyfaildtr.DataSource = dt;
                    frequentlyfaildtr.DataBind();

                    ViewState["frequentlyfaileddtr"] = dt;

                }


            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }


        }


        public void Loadexpenditureview(string sOfficeCode = "")
        {
            try
            {
                clsDashboardUcForms objDashboard = new clsDashboardUcForms();
                DataTable dt = new DataTable();

                bool CurrentDate = false;

                if (hdfRefId.Value == "true")
                {

                    lblDetailsview.Text = "View Expenditure  as On " + System.DateTime.Now.ToString("dd-MMM-yyyy");
                }
                objDashboard.roleId = objSession.RoleId;
                objDashboard.roleType = objSession.sRoleType;

                objDashboard.officecode = sOfficeCode;

                dt = objDashboard.Loadexpenditureview(objDashboard);
                if (dt.Rows.Count == 0)
                {

                    ShowEmptyexpenditureGrid();
                    ViewState["Expenditureview"] = dt;
                }
                else
                {
                    Gridexpenditure.DataSource = dt;
                    Gridexpenditure.DataBind();
                    ViewState["Expenditureview"] = dt;

                }


            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }


        }

        public void LoadPoview(string sOfficeCode = "")
        {
            try
            {
                clsDashboardUcForms objDashboard = new clsDashboardUcForms();
                DataTable dt = new DataTable();

                bool CurrentDate = false;

                if (hdfRefId.Value == "true")
                {

                    lblDetailsview.Text = "View Po  as On " + System.DateTime.Now.ToString("dd-MMM-yyyy");
                }
                objDashboard.roleId = objSession.RoleId;
                objDashboard.roleType = objSession.sRoleType;

                objDashboard.officecode = sOfficeCode;

                dt = objDashboard.LoadPoDetails(objDashboard);
                if (dt.Rows.Count == 0)
                {

                    ShowEmptypoGrid();
                    ViewState["poview"] = dt;
                }
                else
                {
                    Gridpodetails.DataSource = dt;
                    Gridpodetails.DataBind();
                    ViewState["poview"] = dt;

                }


            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }


        }



        public void LoadFailureDetailsviewLocationWise(string sOfficeCode = "")
        {
            try
            {
                clsDashboardUcForms objDashboard = new clsDashboardUcForms();
                DataTable dt = new DataTable();


                if (hdfRefId.Value == "true")
                {
                    lblDetailsview.Text = "Failure Status view as On " + System.DateTime.Now.ToString("dd-MMM-yyyy");
                }

                objDashboard.officecode = sOfficeCode;
                objDashboard.roleId = objSession.RoleId;
                objDashboard.roleType = objSession.sRoleType;


                dt = objDashboard.LoadFailureDetailsviewLocationWise(objDashboard);
                if (dt.Rows.Count == 0)
                {

                    ShowEmptyfailureGrid();
                    grdlfailureDetailsview.ShowFooter = true;
                    ViewState["FailureDetailview"] = dt;

                }
                else
                {
                    grdlfailureDetailsview.ShowFooter = true;
                    grdlfailureDetailsview.DataSource = dt;
                    grdlfailureDetailsview.DataBind();
                    ViewState["FailureDetailview"] = dt;

                }
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void LoadTCDetailsviewLocationWise(string sOfficeCode = "")
        {
            try
            {
                clsDashboardUcForms objDashboard = new clsDashboardUcForms();
                DataTable dt = new DataTable();


                if (hdfRefId.Value == "true")
                {
                    lblDetailsview.Text = "TC Guarantee Status view as On " + System.DateTime.Now.ToString("dd-MMM-yyyy");
                }

                objDashboard.roleId = objSession.RoleId;
                objDashboard.roleType = objSession.sRoleType;
                objDashboard.officecode = sOfficeCode;


                dt = objDashboard.LoadTransformerDetailsstackgridview(objDashboard);
                if (dt.Rows.Count == 0)
                {

                    ShowEmptyTCGrid();
                    grdlTCDetailsview.ShowFooter = true;
                    ViewState["TCDetailview"] = dt;

                }
                else
                {
                    grdlTCDetailsview.ShowFooter = true;
                    grdlTCDetailsview.DataSource = dt;
                    grdlTCDetailsview.DataBind();
                    ViewState["TCDetailview"] = dt;

                }
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadTCstatusviewLocationWise(string sOfficeCode = "")
        {
            try
            {
                clsDashboardUcForms objDashboard = new clsDashboardUcForms();
                DataTable dt = new DataTable();


                if (hdfRefId.Value == "true")
                {
                    lblDetailsview.Text = "TC Condition Status as On " + System.DateTime.Now.ToString("dd-MMM-yyyy");
                }

                objDashboard.roleId = objSession.RoleId;
                objDashboard.roleType = objSession.sRoleType;
                objDashboard.officecode = sOfficeCode;

                dt = objDashboard.LoadTcStatusgridview(objDashboard);
                if (dt.Rows.Count == 0)
                {

                    ShowEmptyTCstatusGrid();
                    grdlTCStatusview.ShowFooter = true;
                    ViewState["TCstatusview"] = dt;

                }
                else
                {
                    grdlTCStatusview.ShowFooter = true;
                    grdlTCStatusview.DataSource = dt;
                    grdlTCStatusview.DataBind();
                    ViewState["TCstatusview"] = dt;

                }
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadalternativeLocationWise(string sOfficeCode = "")
        {
            try
            {
                clsDashboardUcForms objDashboard = new clsDashboardUcForms();
                DataTable dt = new DataTable();


                if (hdfRefId.Value == "true")
                {
                    lblDetailsview.Text = "Alternate Power Supply Status as On " + System.DateTime.Now.ToString("dd-MMM-yyyy");
                }

                objDashboard.roleId = objSession.RoleId;
                objDashboard.roleType = objSession.sRoleType;
                objDashboard.officecode = sOfficeCode;


                dt = objDashboard.LoadAlternativeDetailsgridview(objDashboard);
                if (dt.Rows.Count == 0)
                {

                    ShowEmptyalternativeGrid();
                    grdlalternativeview.ShowFooter = true;
                    ViewState["alternativeview"] = dt;

                }
                else
                {
                    grdlalternativeview.ShowFooter = true;
                    grdlalternativeview.DataSource = dt;
                    grdlalternativeview.DataBind();
                    ViewState["alternativeview"] = dt;

                }
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void LoadrepairerperformenceLocationWise(string sOfficeCode = "")
        {
            try
            {
                clsDashboardUcForms objDashboard = new clsDashboardUcForms();
                DataTable dt = new DataTable();


                if (hdfRefId.Value == "true")
                {
                    lblDetailsview.Text = "View Repairer Performanceview as On " + System.DateTime.Now.ToString("dd-MMM-yyyy");
                }

                objDashboard.roleId = objSession.RoleId;
                objDashboard.roleType = objSession.sRoleType;
                objDashboard.officecode = sOfficeCode;



                dt = objDashboard.LoadRepairerPerformanceDetailsgridview(objDashboard);
                if (dt.Rows.Count == 0)
                {

                    ShowRepairerPerformanceGrid();
                    grdRepairerPerformanceview.ShowFooter = true;
                    ViewState["repairerperformance"] = dt;

                }
                else
                {
                    grdRepairerPerformanceview.ShowFooter = true;
                    grdRepairerPerformanceview.DataSource = dt;
                    grdRepairerPerformanceview.DataBind();
                    ViewState["repairerperformance"] = dt;

                }
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadPendingReplacementLocationWise(string sOfficeCode = "")
        {
            try
            {
                clsDashboardUcForms objDashboard = new clsDashboardUcForms();
                DataTable dt = new DataTable();


                if (hdfRefId.Value == "true")
                {
                    lblDetailsview.Text = "View Pending Replacement as On " + System.DateTime.Now.ToString("dd-MMM-yyyy");
                }

                objDashboard.roleId = objSession.RoleId;
                objDashboard.roleType = objSession.sRoleType;
                objDashboard.officecode = sOfficeCode;



                dt = objDashboard.LoadPendingReplacementDetailsgridview(objDashboard);
                if (dt.Rows.Count == 0)
                {

                    ShowpendingreplacementviewGrid();
                    grdpendingreplacementview.ShowFooter = true;
                    ViewState["PendingReplacement"] = dt;

                }
                else
                {
                    grdpendingreplacementview.ShowFooter = true;
                    grdpendingreplacementview.DataSource = dt;
                    grdpendingreplacementview.DataBind();
                    ViewState["PendingReplacement"] = dt;

                }
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadridetailsLocationWise(string sOfficeCode = "")
        {
            try
            {
                clsDashboardUcForms objDashboard = new clsDashboardUcForms();
                DataTable dt = new DataTable();


                if (hdfRefId.Value == "true")
                {
                    lblDetailsview.Text = "View RI Details as On " + System.DateTime.Now.ToString("dd-MMM-yyyy");
                }

                objDashboard.roleId = objSession.RoleId;
                objDashboard.roleType = objSession.sRoleType;
                objDashboard.officecode = sOfficeCode;



                dt = objDashboard.Loadridetalsgridview(objDashboard);
                if (dt.Rows.Count == 0)
                {

                    ShowridetailsviewGrid();
                    Ridetails.ShowFooter = true;
                    ViewState["Ridetails"] = dt;

                }
                else
                {
                    Ridetails.ShowFooter = true;
                    Ridetails.DataSource = dt;
                    Ridetails.DataBind();
                    ViewState["Ridetails"] = dt;

                }
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void LoadfrequentlyfailedLocationWise(string sOfficeCode = "")
        {
            try
            {
                clsDashboardUcForms objDashboard = new clsDashboardUcForms();
                DataTable dt = new DataTable();


                if (hdfRefId.Value == "true")
                {
                    lblDetailsview.Text = "Frequently Failed  view  as On " + System.DateTime.Now.ToString("dd-MMM-yyyy");
                }

                objDashboard.roleId = objSession.RoleId;
                objDashboard.roleType = objSession.sRoleType;
                objDashboard.officecode = sOfficeCode;

                if (sOfficeCode.Length != 5)
                {

                    dt = objDashboard.Loadfrequentlyfailedgridview(objDashboard);

                    if (dt.Rows.Count == 0)
                    {
                        frequentlyfaildtc.Visible = false;
                        frequentlyfail.Visible = true;
                        ShowfrequentlyfailedviewGrid();
                        frequentlyfail.ShowFooter = true;
                        
                        ViewState["frequentlyfailed"] = dt;

                    }
                    else
                    {


                        frequentlyfaildtc.Visible = false;
                        frequentlyfail.Visible = true;
                        frequentlyfail.ShowFooter = true;
                        frequentlyfail.DataSource = dt;
                        frequentlyfail.DataBind();
                     
                        ViewState["frequentlyfailed"] = dt;




                    }
                }
                else
                {
                    DataTable dtsection = new DataTable();
                    dtsection = objDashboard.Loadfrequentlyfailedsectiongridview(objDashboard);

                    if (dtsection.Rows.Count == 0)
                    {
                        frequentlyfail.Visible = false;
                        frequentlyfaildtc.Visible = false;
                        ShowfrequentlyfailedsectionviewGrid();
                        frequentlyfaildtc.ShowFooter = true;
                       
                       // ViewState["frequentlyfailedsection"] = dtsection;
                    }
                    else
                    {
                        frequentlyfail.Visible = false;
                        frequentlyfaildtc.Visible = true;
                        frequentlyfaildtc.ShowFooter = true;
                        frequentlyfaildtc.DataSource = dtsection;
                        frequentlyfaildtc.DataBind();
                       // ViewState["frequentlyfailedsection"] = dtsection;
                    }

                }

                
             
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void LoadfrequentlyfaileddtrLocationWise(string sOfficeCode = "")
        {
            try
            {
                clsDashboardUcForms objDashboard = new clsDashboardUcForms();
                DataTable dt = new DataTable();


                if (hdfRefId.Value == "true")
                {
                    lblDetailsview.Text = "Frequently Failed dtr view  as On " + System.DateTime.Now.ToString("dd-MMM-yyyy");
                }

                objDashboard.roleId = objSession.RoleId;
                objDashboard.roleType = objSession.sRoleType;
                objDashboard.officecode = sOfficeCode;

               
                    dt = objDashboard.Loadfrequentlyfaileddtrgridview(objDashboard);

                    if (dt.Rows.Count == 0)
                    {
                        //frequentlyfaildtc.Visible = false;
                        frequentlyfaildtr.Visible = true;
                        ShowfrequentlyfaileddtrviewGrid();
                        frequentlyfaildtr.ShowFooter = true;

                        ViewState["frequentlyfaileddtr"] = dt;

                    }
                    else
                    {


                       // frequentlyfaildtc.Visible = false;
                        frequentlyfaildtr.Visible = true;
                        frequentlyfaildtr.ShowFooter = true;
                        frequentlyfaildtr.DataSource = dt;
                        frequentlyfaildtr.DataBind();

                        ViewState["frequentlyfaileddtr"] = dt;




                    }
                
                



            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void LoadexpenditureLocationWise(string sOfficeCode = "")
        {
            try
            {
                clsDashboardUcForms objDashboard = new clsDashboardUcForms();
                DataTable dt = new DataTable();


                if (hdfRefId.Value == "true")
                {
                    lblDetailsview.Text = "Expenditure Status view as On " + System.DateTime.Now.ToString("dd-MMM-yyyy");
                }

                objDashboard.officecode = sOfficeCode;
                objDashboard.roleId = objSession.RoleId;
                objDashboard.roleType = objSession.sRoleType;


                dt = objDashboard.Loadexpenditureview(objDashboard);
                if (dt.Rows.Count == 0)
                {

                    ShowEmptyexpenditureGrid();
                    Gridexpenditure.ShowFooter = true;
                    ViewState["Expenditureview"] = dt;

                }
                else
                {
                    Gridexpenditure.ShowFooter = true;
                    Gridexpenditure.DataSource = dt;
                    Gridexpenditure.DataBind();
                    ViewState["Expenditureview"] = dt;

                }
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        //public void LoadStatusReportFeederWise(string sOfficeCode = "")
        //{

        //    try
        //    {
        //        clsDashboardUcForms objDashboard = new clsDashboardUcForms();
        //        DataTable dt = new DataTable();
        //        bool bCurrentDate = false;

        //        if (hdfRefId.Value == "true")
        //        {
        //            bCurrentDate = true;
        //        }

        //        objDashboard.officecode = sOfficeCode;
        //        objDashboard.FromDate = hdfFromDate.Value;
        //        objDashboard.ToDate = hdfToDate.Value;

        //        dt = objDashboard.LoadStatusReportFeederWise(objDashboard);
        //        if (dt.Rows.Count == 0)
        //        {

        //            ShowEmptyGrid();
        //            ViewState["StatusReport"] = dt;
        //            grdlDetailsviewReport.Columns[2].Visible = true;
        //            grdlDetailsviewReport.Columns[1].Visible = false;

        //        }
        //        else
        //        {
        //            grdlDetailsviewReport.DataSource = dt;
        //            grdlDetailsviewReport.DataBind();
        //            ViewState["StatusReport"] = dt;

        //            grdlDetailsviewReport.ShowFooter = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadStatusReportFeederWise");
        //    }
        //}

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


        public void ShowEmptyfailureGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("MONTHS");
                dt.Columns.Add("OFF_NAME");
                dt.Columns.Add("OFF_CODE");
                dt.Columns.Add("DT_COUNT");
                dt.Columns.Add("PRESENTYEAR");
                dt.Columns.Add("PRESENTMONTH");
                dt.Columns.Add("PRESENTCOUNT");
                dt.Columns.Add("PREVIOUSYEAR");
                dt.Columns.Add("PREVIOUSMONTH");
                dt.Columns.Add("PREVIOUSCOUNT");



                grdlfailureDetailsview.DataSource = dt;
                grdlfailureDetailsview.DataBind();

                int iColCount = grdlfailureDetailsview.Rows[0].Cells.Count;
                grdlfailureDetailsview.Rows[0].Cells.Clear();
                grdlfailureDetailsview.Rows[0].Cells.Add(new TableCell());
                grdlfailureDetailsview.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdlfailureDetailsview.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void ShowEmptyTCGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);

                dt.Columns.Add("OFF_NAME");
                dt.Columns.Add("OFF_CODE");
                dt.Columns.Add("AGP");
                dt.Columns.Add("WGP");
                dt.Columns.Add("UNKNOWN");





                grdlTCDetailsview.DataSource = dt;
                grdlTCDetailsview.DataBind();

                int iColCount = grdlTCDetailsview.Rows[0].Cells.Count;
                grdlTCDetailsview.Rows[0].Cells.Clear();
                grdlTCDetailsview.Rows[0].Cells.Add(new TableCell());
                grdlTCDetailsview.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdlTCDetailsview.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void ShowEmptyTCstatusGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);

                dt.Columns.Add("OFF_NAME");
                dt.Columns.Add("OFF_CODE");
                dt.Columns.Add("BRAND_NEW");
                dt.Columns.Add("REPAIR_GOOD");
                dt.Columns.Add("FAULTY");

                grdlTCStatusview.DataSource = dt;
                grdlTCStatusview.DataBind();

                int iColCount = grdlTCStatusview.Rows[0].Cells.Count;
                grdlTCStatusview.Rows[0].Cells.Clear();
                grdlTCStatusview.Rows[0].Cells.Add(new TableCell());
                grdlTCStatusview.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdlTCStatusview.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void ShowEmptyalternativeGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);

                dt.Columns.Add("OFF_NAME");
                dt.Columns.Add("OFF_CODE");
                dt.Columns.Add("ARRANGED");
                dt.Columns.Add("NOT_ARRANGED");


                grdlalternativeview.DataSource = dt;
                grdlalternativeview.DataBind();

                int iColCount = grdlalternativeview.Rows[0].Cells.Count;
                grdlalternativeview.Rows[0].Cells.Clear();
                grdlalternativeview.Rows[0].Cells.Add(new TableCell());
                grdlalternativeview.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdlalternativeview.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void ShowRepairerPerformanceGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);

                dt.Columns.Add("OFFICE_CODE");
                dt.Columns.Add("OFFICE_NAME");
                dt.Columns.Add("PENDING");
                dt.Columns.Add("COMPLETED");

                grdRepairerPerformanceview.DataSource = dt;
                grdRepairerPerformanceview.DataBind();

                int iColCount = grdRepairerPerformanceview.Rows[0].Cells.Count;
                grdRepairerPerformanceview.Rows[0].Cells.Clear();
                grdRepairerPerformanceview.Rows[0].Cells.Add(new TableCell());
                grdRepairerPerformanceview.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdRepairerPerformanceview.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void ShowpendingreplacementviewGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);

                dt.Columns.Add("OFFICE_CODE");
                dt.Columns.Add("OFFICE_NAME");
                dt.Columns.Add("1-3 Days");
                dt.Columns.Add(">3 Days");
                dt.Columns.Add("1-Day");


                grdpendingreplacementview.DataSource = dt;
                grdpendingreplacementview.DataBind();

                int iColCount = grdpendingreplacementview.Rows[0].Cells.Count;
                grdpendingreplacementview.Rows[0].Cells.Clear();
                grdpendingreplacementview.Rows[0].Cells.Add(new TableCell());
                grdpendingreplacementview.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdpendingreplacementview.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void ShowridetailsviewGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);

                dt.Columns.Add("OFFICE_CODE");
                dt.Columns.Add("OFFICE_NAME");
                dt.Columns.Add("1-3 Days");
                dt.Columns.Add(">3 Days");
                dt.Columns.Add("1-Day");


                Ridetails.DataSource = dt;
                Ridetails.DataBind();

                int iColCount = Ridetails.Rows[0].Cells.Count;
                Ridetails.Rows[0].Cells.Clear();
                Ridetails.Rows[0].Cells.Add(new TableCell());
                Ridetails.Rows[0].Cells[0].ColumnSpan = iColCount;
                Ridetails.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void ShowfrequentlyfailedviewGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);

                dt.Columns.Add("OFF_CODE");
                dt.Columns.Add("OFF_NAME");
                dt.Columns.Add("TOTAL");


                frequentlyfaildtc.Visible = false;
                frequentlyfail.Visible = true;
                frequentlyfail.DataSource = dt;
                frequentlyfail.DataBind();

                int iColCount = frequentlyfail.Rows[0].Cells.Count;
                frequentlyfail.Rows[0].Cells.Clear();
                frequentlyfail.Rows[0].Cells.Add(new TableCell());
                frequentlyfail.Rows[0].Cells[0].ColumnSpan = iColCount;
                frequentlyfail.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void ShowfrequentlyfailedsectionviewGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);

                dt.Columns.Add("OFF_CODE");
                dt.Columns.Add("OFF_NAME");
                dt.Columns.Add("DF_DTC_CODE");


                frequentlyfail.Visible = false;
                frequentlyfaildtc.Visible = true;
                frequentlyfaildtc.DataSource = dt;
                frequentlyfaildtc.DataBind();

                int iColCount = frequentlyfaildtc.Rows[0].Cells.Count;
                frequentlyfaildtc.Rows[0].Cells.Clear();
                frequentlyfaildtc.Rows[0].Cells.Add(new TableCell());
                frequentlyfaildtc.Rows[0].Cells[0].ColumnSpan = iColCount;
                frequentlyfaildtc.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void ShowfrequentlyfaileddtrviewGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);

                dt.Columns.Add("OFF_CODE");
                dt.Columns.Add("OFF_NAME");
                dt.Columns.Add("< 63");
                dt.Columns.Add("> 250");
                dt.Columns.Add("100-250");
                dt.Columns.Add("63-100");


                //frequentlyfaildtc.Visible = false;
                frequentlyfaildtr.Visible = true;
                frequentlyfaildtr.DataSource = dt;
                frequentlyfaildtr.DataBind();

                int iColCount = frequentlyfaildtr.Rows[0].Cells.Count;
                frequentlyfaildtr.Rows[0].Cells.Clear();
                frequentlyfaildtr.Rows[0].Cells.Add(new TableCell());
                frequentlyfaildtr.Rows[0].Cells[0].ColumnSpan = iColCount;
                frequentlyfaildtr.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        public void ShowEmptyexpenditureGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("MONTHS");
                dt.Columns.Add("OFF_NAME");
                dt.Columns.Add("OFF_CODE");
                //dt.Columns.Add("DT_COUNT");
                dt.Columns.Add("PRESENTYEAR");
                dt.Columns.Add("PRESENTMONTH");
                dt.Columns.Add("PRESENTCOUNT");
                dt.Columns.Add("PREVIOUSYEAR");
                dt.Columns.Add("PREVIOUSMONTH");
                dt.Columns.Add("PREVIOUSCOUNT");



                Gridexpenditure.DataSource = dt;
                Gridexpenditure.DataBind();

                int iColCount = Gridexpenditure.Rows[0].Cells.Count;
                Gridexpenditure.Rows[0].Cells.Clear();
                Gridexpenditure.Rows[0].Cells.Add(new TableCell());
                Gridexpenditure.Rows[0].Cells[0].ColumnSpan = iColCount;
                Gridexpenditure.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void ShowEmptypoGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("PREVIOUSYEAR");
                dt.Columns.Add("PREVIOUS_PO_COUNT");
                dt.Columns.Add("PREVIOUS_TOTAL_QUANTITY");           
                dt.Columns.Add("PREVIOUS_PO_AMOUNT");
                dt.Columns.Add("PRESENTYEAR");
                dt.Columns.Add("PRESENT_PO_COUNT");
                dt.Columns.Add("PRESENT_TOTAL_QUANTITY");
                dt.Columns.Add("PRESENT_PO_AMOUNT");
                



                Gridpodetails.DataSource = dt;
                Gridpodetails.DataBind();

                int iColCount = Gridpodetails.Rows[0].Cells.Count;
                Gridpodetails.Rows[0].Cells.Clear();
                Gridpodetails.Rows[0].Cells.Add(new TableCell());
                Gridpodetails.Rows[0].Cells[0].ColumnSpan = iColCount;
                Gridpodetails.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void grdlfailureDetailsview_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "view")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    string sOfficeCode = ((Label)row.FindControl("lblOffCode")).Text;
                    hdfOfficeCode.Value = sOfficeCode;



                    LoadFailureDetailsviewLocationWise(sOfficeCode);


                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtLocName = (TextBox)row.FindControl("txtLocName");


                    DataTable dt = (DataTable)ViewState["FailureDetailview"];
                    dv = dt.DefaultView;

                    if (txtLocName.Text != "")
                    {
                        sFilter = "OFF_NAME Like '%" + txtLocName.Text.Replace("'", "'") + "%' AND";
                    }


                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdlfailureDetailsview.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdlfailureDetailsview.DataSource = dv;
                            ViewState["FailureDetailview"] = dv.ToTable();
                            grdlfailureDetailsview.DataBind();
                        }
                        else
                        {
                            ShowEmptyfailureGrid();
                        }
                    }
                    else
                    {

                        LoadFailureDetailsviewLocationWise(hdfOfficeCode.Value);

                    }

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void grdlTCDetailsview_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {

                if (e.CommandName == "view")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    string sOfficeCode = ((Label)row.FindControl("lblOffCode")).Text;
                    hdfOfficeCode.Value = sOfficeCode;




                    LoadTCDetailsviewLocationWise(sOfficeCode);


                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtLocName = (TextBox)row.FindControl("txtLocName");


                    DataTable dt = (DataTable)ViewState["TCDetailview"];
                    dv = dt.DefaultView;

                    if (txtLocName.Text != "")
                    {
                        sFilter = "OFF_NAME Like '%" + txtLocName.Text.Replace("'", "'") + "%' AND";
                    }


                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdlTCDetailsview.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdlTCDetailsview.DataSource = dv;
                            ViewState["TCDetailview"] = dv.ToTable();
                            grdlTCDetailsview.DataBind();
                        }
                        else
                        {
                            ShowEmptyTCGrid();
                        }
                    }
                    else
                    {

                        LoadTCDetailsviewLocationWise(hdfOfficeCode.Value);

                    }

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdlTCStatusview_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {

                if (e.CommandName == "view")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    string sOfficeCode = ((Label)row.FindControl("lblOffCode")).Text;
                    hdfOfficeCode.Value = sOfficeCode;


                    LoadTCstatusviewLocationWise(sOfficeCode);


                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtLocName = (TextBox)row.FindControl("txtLocName");


                    DataTable dt = (DataTable)ViewState["TCstatusview"];
                    dv = dt.DefaultView;

                    if (txtLocName.Text != "")
                    {
                        sFilter = "OFF_NAME Like '%" + txtLocName.Text.Replace("'", "'") + "%' AND";
                    }


                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdlTCStatusview.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdlTCStatusview.DataSource = dv;
                            ViewState["TCstatusview"] = dv.ToTable();
                            grdlTCStatusview.DataBind();
                        }
                        else
                        {
                            ShowEmptyTCstatusGrid();
                        }
                    }
                    else
                    {

                        LoadTCstatusviewLocationWise(hdfOfficeCode.Value);

                    }

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdlalternativeview_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {

                if (e.CommandName == "view")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    string sOfficeCode = ((Label)row.FindControl("lblOffCode")).Text;
                    hdfOfficeCode.Value = sOfficeCode;


                    LoadalternativeLocationWise(sOfficeCode);


                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtLocName = (TextBox)row.FindControl("txtLocName");


                    DataTable dt = (DataTable)ViewState["alternativeview"];
                    dv = dt.DefaultView;

                    if (txtLocName.Text != "")
                    {
                        sFilter = "OFF_NAME Like '%" + txtLocName.Text.Replace("'", "'") + "%' AND";
                    }


                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdlalternativeview.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdlalternativeview.DataSource = dv;
                            ViewState["alternativeview"] = dv.ToTable();
                            grdlalternativeview.DataBind();
                        }
                        else
                        {
                            ShowEmptyalternativeGrid();
                        }
                    }
                    else
                    {

                        LoadalternativeLocationWise(hdfOfficeCode.Value);

                    }

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void grdRepairerPerformanceview_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {

                if (e.CommandName == "view")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    string sOfficeCode = ((Label)row.FindControl("lblOffCode")).Text;
                    hdfOfficeCode.Value = sOfficeCode;


                    LoadrepairerperformenceLocationWise(sOfficeCode);


                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtLocName = (TextBox)row.FindControl("txtLocName");


                    DataTable dt = (DataTable)ViewState["repairerperformance"];
                    dv = dt.DefaultView;

                    if (txtLocName.Text != "")
                    {
                        sFilter = "OFFICE_NAME Like '%" + txtLocName.Text.Replace("'", "'") + "%' AND";
                    }


                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdRepairerPerformanceview.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdRepairerPerformanceview.DataSource = dv;
                            ViewState["repairerperformance"] = dv.ToTable();
                            grdRepairerPerformanceview.DataBind();
                        }
                        else
                        {
                            ShowRepairerPerformanceGrid();
                        }
                    }
                    else
                    {

                        LoadrepairerperformenceLocationWise(hdfOfficeCode.Value);

                    }

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdpendingreplacementview_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {

                if (e.CommandName == "view")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    string sOfficeCode = ((Label)row.FindControl("lblOffCode")).Text;
                    hdfOfficeCode.Value = sOfficeCode;


                    LoadPendingReplacementLocationWise(sOfficeCode);


                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtLocName = (TextBox)row.FindControl("txtLocName");


                    DataTable dt = (DataTable)ViewState["PendingReplacement"];
                    dv = dt.DefaultView;

                    if (txtLocName.Text != "")
                    {
                        sFilter = "OFFICE_NAME Like '%" + txtLocName.Text.Replace("'", "'") + "%' AND";
                    }


                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdpendingreplacementview.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdpendingreplacementview.DataSource = dv;
                            ViewState["PendingReplacement"] = dv.ToTable();
                            grdpendingreplacementview.DataBind();
                        }
                        else
                        {
                            ShowpendingreplacementviewGrid();
                        }
                    }
                    else
                    {

                        LoadPendingReplacementLocationWise(hdfOfficeCode.Value);

                    }

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdRidetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {

                if (e.CommandName == "view")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    string sOfficeCode = ((Label)row.FindControl("lblOffCode")).Text;
                    hdfOfficeCode.Value = sOfficeCode;


                    LoadridetailsLocationWise(sOfficeCode);


                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtLocName = (TextBox)row.FindControl("txtLocName");


                    DataTable dt = (DataTable)ViewState["Ridetails"];
                    dv = dt.DefaultView;

                    if (txtLocName.Text != "")
                    {
                        sFilter = "OFFICE_NAME Like '%" + txtLocName.Text.Replace("'", "'") + "%' AND";
                    }


                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        Ridetails.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            Ridetails.DataSource = dv;
                            ViewState["Ridetails"] = dv.ToTable();
                            Ridetails.DataBind();
                        }
                        else
                        {
                            ShowridetailsviewGrid();
                        }
                    }
                    else
                    {

                        LoadridetailsLocationWise(hdfOfficeCode.Value);

                    }

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void grdfrequentlyfail_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {

                if (e.CommandName == "view")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    string sOfficeCode = ((Label)row.FindControl("lblOffCode")).Text;
                    hdfOfficeCode.Value = sOfficeCode;


                    LoadfrequentlyfailedLocationWise(sOfficeCode);


                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtLocName = (TextBox)row.FindControl("txtLocName");


                    DataTable dt = (DataTable)ViewState["frequentlyfailed"];
                    dv = dt.DefaultView;

                    if (txtLocName.Text != "")
                    {
                        sFilter = "OFF_NAME Like '%" + txtLocName.Text.Replace("'", "'") + "%' AND";
                    }


                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        frequentlyfail.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            frequentlyfail.DataSource = dv;
                            ViewState["frequentlyfailed"] = dv.ToTable();
                            frequentlyfail.DataBind();
                        }
                        else
                        {
                            ShowfrequentlyfailedviewGrid();
                        }
                    }
                    else
                    {

                        LoadfrequentlyfailedLocationWise(hdfOfficeCode.Value);

                    }

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdfrequentlyfaildtr_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {

                if (e.CommandName == "view")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    string sOfficeCode = ((Label)row.FindControl("lblOffCode")).Text;
                    hdfOfficeCode.Value = sOfficeCode;


                    LoadfrequentlyfaileddtrLocationWise(sOfficeCode);


                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtLocName = (TextBox)row.FindControl("txtLocName");


                    DataTable dt = (DataTable)ViewState["frequentlyfaileddtr"];
                    dv = dt.DefaultView;

                    if (txtLocName.Text != "")
                    {
                        sFilter = "OFF_NAME Like '%" + txtLocName.Text.Replace("'", "'") + "%' AND";
                    }


                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        frequentlyfaildtr.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            frequentlyfaildtr.DataSource = dv;
                            ViewState["frequentlyfaileddtr"] = dv.ToTable();
                            frequentlyfaildtr.DataBind();
                        }
                        else
                        {
                            ShowfrequentlyfaileddtrviewGrid();
                        }
                    }
                    else
                    {

                        LoadfrequentlyfaileddtrLocationWise(hdfOfficeCode.Value);

                    }

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void grdlexpenditureview_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "view")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    string sOfficeCode = ((Label)row.FindControl("lblOffCode")).Text;
                    hdfOfficeCode.Value = sOfficeCode;

                    LoadexpenditureLocationWise(sOfficeCode);


                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtLocName = (TextBox)row.FindControl("txtLocName");


                    DataTable dt = (DataTable)ViewState["Expenditureview"];
                    dv = dt.DefaultView;

                    if (txtLocName.Text != "")
                    {
                        sFilter = "OFF_NAME Like '%" + txtLocName.Text.Replace("'", "'") + "%' AND";
                    }


                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        Gridexpenditure.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            Gridexpenditure.DataSource = dv;
                            ViewState["Expenditureview"] = dv.ToTable();
                            Gridexpenditure.DataBind();
                        }
                        else
                        {
                            ShowEmptyexpenditureGrid();
                        }
                    }
                    else
                    {

                        LoadexpenditureLocationWise(hdfOfficeCode.Value);

                    }

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

       


        protected void cmdBack_Click(object sender, EventArgs e)
        {
            try
            {

                if (hdfGridId.Value == "FailureDetails")
                {

                    LoadFailureDetailsview();

                }

                if (hdfGridId.Value == "TCDetails")
                {

                    LoadTCDetailsview();

                }

                if (hdfGridId.Value == "TcStatus")
                {

                    LoadTCstatusview();

                }

                if (hdfGridId.Value == "Alternativepowersupply")
                {

                    Loadalternativesupplyview();

                }

                if (hdfGridId.Value == "RepairerPerformance")
                {
                    LoadRepairerPerformanceview();
                }

                if (hdfGridId.Value == "PendingReplacement")
                {
                    LoadPendingReplacementview();
                }

                if (hdfGridId.Value == "RIstatus")
                {
                    LoadRistatusview();
                }

                if (hdfGridId.Value == "Frequentlyfailed")
                {

                    LoadFrequentlyfailedview();

                }


                 if (hdfGridId.Value == "Frequentlyfaileddtr")
                {

                    LoadFrequentlyfaileddtrview();

                }

                  if (hdfGridId.Value == "Expenditure")
                 {

                     Loadexpenditureview();

                 }

                   if (hdfGridId.Value == "Po")
                  {

                      LoadPoview();

                  }

               



            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdlfailureDetailsview_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdlfailureDetailsview.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["FailureDetailview"];
                grdlfailureDetailsview.DataSource = dt;
                grdlfailureDetailsview.DataBind();
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void grdlTCDetailsview_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdlTCDetailsview.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["TCDetailview"];
                grdlTCDetailsview.DataSource = dt;
                grdlTCDetailsview.DataBind();
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdlTCStatusview_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdlTCStatusview.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["TCstatusview"];
                grdlTCStatusview.DataSource = dt;
                grdlTCStatusview.DataBind();
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdlalternativeview_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdlalternativeview.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["alternativeview"];
                grdlalternativeview.DataSource = dt;
                grdlalternativeview.DataBind();
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void grdRepairerPerformanceview_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdRepairerPerformanceview.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["repairerperformance"];
                grdRepairerPerformanceview.DataSource = dt;
                grdRepairerPerformanceview.DataBind();
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdpendingreplacementview_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdpendingreplacementview.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["PendingReplacement"];
                grdpendingreplacementview.DataSource = dt;
                grdpendingreplacementview.DataBind();
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdRidetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Ridetails.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Ridetails"];
                Ridetails.DataSource = dt;
                Ridetails.DataBind();
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void grdfrequentlyfail_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                frequentlyfail.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["frequentlyfailed"];
                frequentlyfail.DataSource = dt;
                frequentlyfail.DataBind();
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void grdfrequentlyfaildtr_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                frequentlyfaildtr.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["frequentlyfaileddtr"];
                frequentlyfaildtr.DataSource = dt;
                frequentlyfaildtr.DataBind();
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        protected void grdlexpenditureview_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Gridexpenditure.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Expenditureview"];
                Gridexpenditure.DataSource = dt;
                Gridexpenditure.DataBind();
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

    }
}